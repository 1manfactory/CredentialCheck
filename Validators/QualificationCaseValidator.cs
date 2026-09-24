using System.Net.Mail;
using CredentialCheck.Dtos;

namespace CredentialCheck.Validators;

public static class QualificationCaseValidator
{
    public static List<string> Validate(CreateQualificationCaseRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.ApplicantName))
        {
            errors.Add("ApplicantName is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ApplicantEmail))
        {
            errors.Add("ApplicantEmail is required.");
        }
        else if (!IsValidEmail(request.ApplicantEmail))
        {
            errors.Add("ApplicantEmail is not valid.");
        }

        if (string.IsNullOrWhiteSpace(request.Country))
        {
            errors.Add("Country is required.");
        }

        return errors;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var address = new MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
