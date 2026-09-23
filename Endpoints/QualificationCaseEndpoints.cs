using CredentialCheck.Dtos;
using CredentialCheck.Services;

namespace CredentialCheck.Endpoints;

public static class QualificationCaseEndpoints
{
    public static void MapQualificationCaseEndpoints(this WebApplication app)
    {
        app.MapGet("/api/cases", async (QualificationCaseService service) =>
        {
            var cases = await service.GetAllAsync();
            return Results.Ok(cases);
        });

        app.MapGet("/api/cases/{id:guid}", async (Guid id, QualificationCaseService service) =>
        {
            var qualificationCase = await service.GetByIdAsync(id);

            return qualificationCase is null
                ? Results.NotFound()
                : Results.Ok(qualificationCase);
        });

        app.MapPost("/api/cases", async (CreateQualificationCaseRequest request, QualificationCaseService service) =>
        {
            if (string.IsNullOrWhiteSpace(request.ApplicantName))
            {
                return Results.BadRequest(new { error = "ApplicantName is required." });
            }

            if (string.IsNullOrWhiteSpace(request.ApplicantEmail))
            {
                return Results.BadRequest(new { error = "ApplicantEmail is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Country))
            {
                return Results.BadRequest(new { error = "Country is required." });
            }

            var qualificationCase = await service.CreateAsync(request);

            return Results.Created($"/api/cases/{qualificationCase.Id}", qualificationCase);
        });
    }
}