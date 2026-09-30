namespace CredentialCheck.Dtos;

// Enthält die Angaben zum Anlegen eines neuen Falls.
public record CreateQualificationCaseRequest(
    string ApplicantName,
    string ApplicantEmail,
    string Country
);