namespace CredentialCheck.Dtos;

public record CreateQualificationDocumentRequest(
	string DocumentType,
	string Title,
	string IssuingInstitution,
	string Country,
	DateOnly? IssueDate
);
