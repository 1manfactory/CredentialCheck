namespace CredentialCheck.Dtos;

public record AuditLogEntryResponse(
	Guid Id,
	Guid QualificationCaseId,
	string Action,
	string Details,
	DateTimeOffset CreatedAt
);
