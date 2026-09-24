using CredentialCheck.Dtos;
using CredentialCheck.Models;

namespace CredentialCheck.Mappers;

public static class AuditLogEntryMapper
{
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
