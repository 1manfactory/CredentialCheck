using CredentialCheck.Dtos;
using CredentialCheck.Models;

namespace CredentialCheck.Mappers;

public static class QualificationCaseMapper
{
    public static QualificationCaseResponse ToResponse(QualificationCase qualificationCase)
    {
        return new QualificationCaseResponse(
            qualificationCase.Id,
            qualificationCase.ApplicantName,
            qualificationCase.ApplicantEmail,
            qualificationCase.Country,
            qualificationCase.Status.ToString(),
            qualificationCase.CreatedAt
        );
    }
}
