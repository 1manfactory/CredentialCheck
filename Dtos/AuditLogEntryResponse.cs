namespace CredentialCheck.Dtos;

// Enthält einen Eintrag aus dem Änderungsverlauf eines Falls.
public record AuditLogEntryResponse(
	Guid Id,
	Guid QualificationCaseId,
	string Action,
	string Details,
	DateTimeOffset CreatedAt
);
