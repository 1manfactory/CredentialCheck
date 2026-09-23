namespace CredentialCheck.Dtos;

public record CreateQualificationCaseRequest(
    string ApplicantName,
    string ApplicantEmail,
    string Country
);