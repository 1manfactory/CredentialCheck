using CredentialCheck.Dtos;
using CredentialCheck.Services;
using CredentialCheck.Validators;

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
			var errors = QualificationCaseValidator.Validate(request);

			if (errors.Count > 0)
			{
				return Results.BadRequest(new { errors });
			}

			try
			{
				var qualificationCase = await service.CreateAsync(request);

				return Results.Created($"/api/cases/{qualificationCase.Id}", qualificationCase);
			}
			catch (InvalidOperationException ex)
			{
				return Results.Conflict(new { error = ex.Message });
			}
		});

		app.MapPatch("/api/cases/{id:guid}/status", async (
			Guid id,
			UpdateQualificationCaseStatusRequest request,
			QualificationCaseService service) =>
		{
			try
			{
				var qualificationCase = await service.UpdateStatusAsync(id, request);

				return qualificationCase is null
					? Results.NotFound()
					: Results.Ok(qualificationCase);
			}
			catch (InvalidOperationException ex)
			{
				return Results.BadRequest(new { error = ex.Message });
			}
		});

	}
}