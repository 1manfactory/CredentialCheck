namespace CredentialCheck.Dtos;

// Gibt die Daten eines Qualifikationsdokuments zurück.
public record QualificationDocumentResponse(
	Guid Id,
	Guid QualificationCaseId,
	string DocumentType,
	string ReviewStatus,
	string Title,
	string IssuingInstitution,
	string Country,
	DateOnly? IssueDate,
	string? OriginalFileName,
	string? ContentType,
	long? FileSizeBytes,
	DateTimeOffset CreatedAt
);