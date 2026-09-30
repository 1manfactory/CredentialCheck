namespace CredentialCheck.Dtos;

// Gibt die wichtigsten Daten eines Qualifikationsfalls zurück.
public record QualificationCaseResponse(
    Guid Id,
    string ApplicantName,
    string ApplicantEmail,
    string Country,
    string Status,
    DateTimeOffset CreatedAt
);