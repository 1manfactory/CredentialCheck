namespace CredentialCheck.Dtos;

public record QualificationDocumentResponse(
	Guid Id,
	Guid QualificationCaseId,
	string DocumentType,
	string Title,
	string IssuingInstitution,
	string Country,
	DateOnly? IssueDate,
	DateTimeOffset CreatedAt
);
