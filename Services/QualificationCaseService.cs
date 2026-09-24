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
}