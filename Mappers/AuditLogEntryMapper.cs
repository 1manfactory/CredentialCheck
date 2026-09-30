using CredentialCheck.Dtos;
using CredentialCheck.Models;

namespace CredentialCheck.Mappers;

// Bereitet Einträge aus dem Änderungsverlauf für die API vor.
public static class AuditLogEntryMapper
{
	// Wandelt einen gespeicherten Eintrag in die API-Antwort um.
	public static AuditLogEntryResponse ToResponse(AuditLogEntry auditLogEntry)
	{
		return new AuditLogEntryResponse(
			auditLogEntry.Id,
			auditLogEntry.QualificationCaseId,
			auditLogEntry.Action,
			auditLogEntry.Details,
			auditLogEntry.CreatedAt
		);
	}
}
