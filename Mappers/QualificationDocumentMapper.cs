using CredentialCheck.Dtos;
using CredentialCheck.Models;

namespace CredentialCheck.Mappers;

public static class QualificationDocumentMapper
{
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
			document.CreatedAt
		);
	}
}
