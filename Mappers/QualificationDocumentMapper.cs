using CredentialCheck.Dtos;
using CredentialCheck.Models;

namespace CredentialCheck.Mappers;

// Bereitet Qualifikationsdokumente für die API vor.
public static class QualificationDocumentMapper
{
	// Wandelt ein gespeichertes Dokument in die API-Antwort um.
	public static QualificationDocumentResponse ToResponse(QualificationDocument document)
	{
		return new QualificationDocumentResponse(
			document.Id,
			document.QualificationCaseId,
			document.DocumentType.ToString(),
			document.ReviewStatus.ToString(),
			document.Title,
			document.IssuingInstitution,
			document.Country,
			document.IssueDate,
			document.OriginalFileName,
			document.ContentType,
			document.FileSizeBytes,
			document.CreatedAt
		);
	}
}
