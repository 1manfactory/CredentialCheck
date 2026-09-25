using CredentialCheck.Data;
using CredentialCheck.Dtos;
using CredentialCheck.Mappers;
using CredentialCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace CredentialCheck.Services;

public class QualificationCaseService
{
	private readonly AppDbContext _dbContext;

	public QualificationCaseService(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<QualificationCaseResponse>> GetAllAsync()
	{
		var cases = await _dbContext.QualificationCases
			.OrderByDescending(c => c.CreatedAt)
			.ToListAsync();

		return cases
			.Select(QualificationCaseMapper.ToResponse)
			.ToList();
	}

	public async Task<QualificationCaseResponse?> GetByIdAsync(Guid id)
	{
		var qualificationCase = await _dbContext.QualificationCases
			.FirstOrDefaultAsync(c => c.Id == id);

		return qualificationCase is null
			? null
			: QualificationCaseMapper.ToResponse(qualificationCase);
	}

	public async Task<QualificationCaseResponse> CreateAsync(CreateQualificationCaseRequest request)
	{
		var normalizedEmail = request.ApplicantEmail.Trim().ToLowerInvariant();

		var emailExists = await _dbContext.QualificationCases
			.AnyAsync(c => c.ApplicantEmail == normalizedEmail);

		if (emailExists)
		{
			throw new InvalidOperationException("A qualification case with this email already exists.");
		}

		var qualificationCase = new QualificationCase
		{
			ApplicantName = request.ApplicantName.Trim(),
			ApplicantEmail = normalizedEmail,
			Country = request.Country.Trim(),
			Status = QualificationCaseStatus.Draft,
			CreatedAt = DateTimeOffset.UtcNow
		};

		_dbContext.QualificationCases.Add(qualificationCase);
		await _dbContext.SaveChangesAsync();

		_dbContext.AuditLogEntries.Add(new AuditLogEntry
		{
			QualificationCaseId = qualificationCase.Id,
			Action = "CaseCreated",
			Details = $"Qualification case created for {qualificationCase.ApplicantEmail}.",
			CreatedAt = DateTimeOffset.UtcNow
		});
		await _dbContext.SaveChangesAsync();

		return QualificationCaseMapper.ToResponse(qualificationCase);
	}

	public async Task<QualificationCaseResponse?> UpdateStatusAsync(Guid id, UpdateQualificationCaseStatusRequest request)
	{
		if (!Enum.TryParse<QualificationCaseStatus>(request.Status, ignoreCase: true, out var newStatus))
		{
			throw new InvalidOperationException($"Invalid status: {request.Status}");
		}

		var qualificationCase = await _dbContext.QualificationCases
			.FirstOrDefaultAsync(c => c.Id == id);

		if (qualificationCase is null)
		{
			return null;
		}

		var oldStatus = qualificationCase.Status;

		qualificationCase.Status = newStatus;

		_dbContext.AuditLogEntries.Add(new AuditLogEntry
		{
			QualificationCaseId = qualificationCase.Id,
			Action = "StatusChanged",
			Details = $"Status changed from {oldStatus} to {newStatus}.",
			CreatedAt = DateTimeOffset.UtcNow
		});

		await _dbContext.SaveChangesAsync();

		return QualificationCaseMapper.ToResponse(qualificationCase);
	}

	public async Task<List<AuditLogEntryResponse>> GetAuditLogAsync(Guid qualificationCaseId)
	{
		var caseExists = await _dbContext.QualificationCases
			.AnyAsync(c => c.Id == qualificationCaseId);

		if (!caseExists)
		{
			throw new KeyNotFoundException("Qualification case not found.");
		}

		var entries = await _dbContext.AuditLogEntries
			.Where(e => e.QualificationCaseId == qualificationCaseId)
			.OrderByDescending(e => e.CreatedAt)
			.ToListAsync();

		return entries
			.Select(AuditLogEntryMapper.ToResponse)
			.ToList();
	}

	public async Task<QualificationCaseReviewSummaryResponse?> GetReviewSummaryAsync(Guid qualificationCaseId)
	{
		var qualificationCase = await _dbContext.QualificationCases
			.FirstOrDefaultAsync(c => c.Id == qualificationCaseId);

		if (qualificationCase is null)
		{
			return null;
		}

		var documents = await _dbContext.QualificationDocuments
			.Where(d => d.QualificationCaseId == qualificationCaseId)
			.ToListAsync();

		var lastActivityAt = await _dbContext.AuditLogEntries
			.Where(e => e.QualificationCaseId == qualificationCaseId)
			.OrderByDescending(e => e.CreatedAt)
			.Select(e => (DateTimeOffset?)e.CreatedAt)
			.FirstOrDefaultAsync();

		var pendingReviewCount = documents.Count(d => d.ReviewStatus == DocumentReviewStatus.PendingReview);
		var acceptedCount = documents.Count(d => d.ReviewStatus == DocumentReviewStatus.Accepted);
		var rejectedCount = documents.Count(d => d.ReviewStatus == DocumentReviewStatus.Rejected);
		var needsClarificationCount = documents.Count(d => d.ReviewStatus == DocumentReviewStatus.NeedsClarification);

		var recommendation = ReviewRecommendation.InReview;

		if (documents.Count == 0)
		{
			recommendation = ReviewRecommendation.NoDocuments;
		}
		else if (needsClarificationCount > 0)
		{
			recommendation = ReviewRecommendation.NeedsClarification;
		}
		else if (rejectedCount > 0)
		{
			recommendation = ReviewRecommendation.ManualDecisionRequired;
		}
		else if (pendingReviewCount == 0 && acceptedCount == documents.Count)
		{
			recommendation = ReviewRecommendation.ReadyForDecision;
		}

		return new QualificationCaseReviewSummaryResponse(
			qualificationCase.Id,
			qualificationCase.Status.ToString(),
			documents.Count,
			pendingReviewCount,
			acceptedCount,
			rejectedCount,
			needsClarificationCount,
			recommendation.ToString(),
			lastActivityAt
		);
	}

}