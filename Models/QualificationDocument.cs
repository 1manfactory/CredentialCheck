namespace CredentialCheck.Models;

public class QualificationDocument
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid QualificationCaseId { get; set; }

	public DocumentType DocumentType { get; set; } = DocumentType.Other;

	public string Title { get; set; } = string.Empty;

	public string IssuingInstitution { get; set; } = string.Empty;

	public string Country { get; set; } = string.Empty;

	public DateOnly? IssueDate { get; set; }

	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
