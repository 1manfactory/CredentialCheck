namespace CredentialCheck.Dtos;

// Enthält die Angaben zu einem neuen Qualifikationsdokument.
public record CreateQualificationDocumentRequest(
	string DocumentType,
	string Title,
	string IssuingInstitution,
	string Country,
	DateOnly? IssueDate
);
