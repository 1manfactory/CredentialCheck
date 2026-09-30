using CredentialCheck.Dtos;
using CredentialCheck.Models;

namespace CredentialCheck.Mappers;

// Bereitet Qualifikationsfälle für die API vor.
public static class QualificationCaseMapper
{
	// Wandelt einen gespeicherten Fall in die API-Antwort um.
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
