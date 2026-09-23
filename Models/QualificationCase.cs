namespace CredentialCheck.Models;

public class QualificationCase
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ApplicantName { get; set; } = string.Empty;

    public string ApplicantEmail { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public QualificationCaseStatus Status { get; set; } = QualificationCaseStatus.Draft;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}