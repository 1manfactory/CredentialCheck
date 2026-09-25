using CredentialCheck.Data;
using CredentialCheck.Dtos;
using CredentialCheck.Mappers;
using CredentialCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace CredentialCheck.Services;

public class QualificationDocumentService
{
	private readonly AppDbContext _dbContext;

	public QualificationDocumentService(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<QualificationDocumentResponse>> GetByCaseIdAsync(Guid qualificationCaseId)
	{
		var caseExists = await _dbContext.QualificationCases
			.AnyAsync(c => c.Id == qualificationCaseId);

		if (!caseExists)
		{
			throw new KeyNotFoundException("Qualification case not found.");
		}

		var documents = await _dbContext.QualificationDocuments
			.Where(d => d.QualificationCaseId == qualificationCaseId)
			.OrderByDescending(d => d.CreatedAt)
			.ToListAsync();

		return documents
			.Select(QualificationDocumentMapper.ToResponse)
			.ToList();
	}

	public async Task<QualificationDocumentResponse> CreateAsync(
		Guid qualificationCaseId,
		CreateQualificationDocumentRequest request,
		StoredDocumentFile? storedFile = null)
	{
		var caseExists = await _dbContext.QualificationCases
			.AnyAsync(c => c.Id == qualificationCaseId);

		if (!caseExists)
		{
			throw new KeyNotFoundException("Qualification case not found.");
		}

		if (!Enum.TryParse<DocumentType>(request.DocumentType, ignoreCase: true, out var documentType))
		{
			throw new InvalidOperationException($"Invalid document type: {request.DocumentType}");
		}

		var document = new QualificationDocument
		{
			QualificationCaseId = qualificationCaseId,
			DocumentType = documentType,
			Title = request.Title.Trim(),
			IssuingInstitution = request.IssuingInstitution.Trim(),
			Country = request.Country.Trim(),
			IssueDate = request.IssueDate,
			OriginalFileName = storedFile?.OriginalFileName,
			StoredFileName = storedFile?.StoredFileName,
			ContentType = storedFile?.ContentType,
			FileSizeBytes = storedFile?.FileSizeBytes,
			StoragePath = storedFile?.StoragePath,
			CreatedAt = DateTimeOffset.UtcNow
		};

		_dbContext.QualificationDocuments.Add(document);

		var fileDetails = storedFile is null
			? "No file attached."
			: $"File: {storedFile.OriginalFileName}, {storedFile.ContentType}, {storedFile.FileSizeBytes} bytes.";

		_dbContext.AuditLogEntries.Add(new AuditLogEntry
		{
			QualificationCaseId = qualificationCaseId,
			Action = "DocumentAdded",
			Details = $"Document added: {document.Title} ({document.DocumentType}). {fileDetails}",
			CreatedAt = DateTimeOffset.UtcNow
		});

		await _dbContext.SaveChangesAsync();

		return QualificationDocumentMapper.ToResponse(document);
	}

	public async Task<QualificationDocumentResponse?> UpdateReviewStatusAsync(
		Guid qualificationCaseId,
		Guid documentId,
		UpdateQualificationDocumentReviewStatusRequest request)
	{
		if (!Enum.TryParse<DocumentReviewStatus>(request.ReviewStatus, ignoreCase: true, out var newStatus))
		{
			throw new InvalidOperationException($"Invalid document review status: {request.ReviewStatus}");
		}

		var document = await _dbContext.QualificationDocuments
			.FirstOrDefaultAsync(d =>
				d.Id == documentId &&
				d.QualificationCaseId == qualificationCaseId);

		if (document is null)
		{
			return null;
		}

		var oldStatus = document.ReviewStatus;

		document.ReviewStatus = newStatus;

		_dbContext.AuditLogEntries.Add(new AuditLogEntry
		{
			QualificationCaseId = qualificationCaseId,
			Action = "DocumentReviewStatusChanged",
			Details = $"Document review status for '{document.Title}' changed from {oldStatus} to {newStatus}.",
			CreatedAt = DateTimeOffset.UtcNow
		});

		await _dbContext.SaveChangesAsync();

		return QualificationDocumentMapper.ToResponse(document);
	}

	public async Task<QualificationDocument?> GetDocumentEntityAsync(
		Guid qualificationCaseId,
		Guid documentId)
	{
		return await _dbContext.QualificationDocuments
			.FirstOrDefaultAsync(d =>
				d.Id == documentId &&
				d.QualificationCaseId == qualificationCaseId);
	}

}
