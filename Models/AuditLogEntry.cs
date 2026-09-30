namespace CredentialCheck.Models;

// Speichert eine Änderung an einem Qualifikationsfall.
public class AuditLogEntry
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid QualificationCaseId { get; set; }

	public string Action { get; set; } = string.Empty;

	public string Details { get; set; } = string.Empty;

	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
