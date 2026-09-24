namespace CredentialCheck.Dtos;

public record QualificationCaseResponse(
    Guid Id,
    string ApplicantName,
    string ApplicantEmail,
    string Country,
    string Status,
    DateTimeOffset CreatedAt
);