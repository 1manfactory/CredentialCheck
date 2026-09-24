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
		CreateQualificationDocumentRequest request)
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
			CreatedAt = DateTimeOffset.UtcNow
		};

		_dbContext.QualificationDocuments.Add(document);

		_dbContext.AuditLogEntries.Add(new AuditLogEntry
		{
			QualificationCaseId = qualificationCaseId,
			Action = "DocumentAdded",
			Details = $"Document added: {document.Title} ({document.DocumentType}).",
			CreatedAt = DateTimeOffset.UtcNow
		});

		await _dbContext.SaveChangesAsync();

		return QualificationDocumentMapper.ToResponse(document);
	}
}
