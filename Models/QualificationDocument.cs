namespace CredentialCheck.Models;

// Speichert die Angaben und Datei zu einem Qualifikationsdokument.
public class QualificationDocument
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid QualificationCaseId { get; set; }

	public DocumentType DocumentType { get; set; } = DocumentType.Other;

	public DocumentReviewStatus ReviewStatus { get; set; } = DocumentReviewStatus.PendingReview;

	public string Title { get; set; } = string.Empty;

	public string IssuingInstitution { get; set; } = string.Empty;

	public string Country { get; set; } = string.Empty;

	public DateOnly? IssueDate { get; set; }

	public string? OriginalFileName { get; set; }

	public string? StoredFileName { get; set; }

	public string? ContentType { get; set; }

	public long? FileSizeBytes { get; set; }

	public string? StoragePath { get; set; }

	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}