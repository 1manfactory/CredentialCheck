using CredentialCheck.Dtos;
using CredentialCheck.Services;
using CredentialCheck.Validators;

namespace CredentialCheck.Endpoints;

// Stellt API-Endpunkte für das Anlegen und Abrufen von Fällen bereit.
public static class QualificationCaseEndpoints
{
	// Registriert die Endpunkte für Fälle, Status und Prüfübersichten.
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

		app.MapGet("/api/cases/{id:guid}/audit-log", async (Guid id, QualificationCaseService service) =>
		{
			try
			{
				var entries = await service.GetAuditLogAsync(id);
				return Results.Ok(entries);
			}
			catch (KeyNotFoundException ex)
			{
				return Results.NotFound(new { error = ex.Message });
			}
		});

		app.MapGet("/api/cases/{id:guid}/review-summary", async (
			Guid id,
			QualificationCaseService service) =>
		{
			var summary = await service.GetReviewSummaryAsync(id);

			return summary is null
				? Results.NotFound()
				: Results.Ok(summary);
		});

		app.MapGet("/api/cases/{id:guid}/decision-note", async (
			Guid id,
			QualificationCaseService caseService,
			DecisionNoteService decisionNoteService) =>
		{
			var summary = await caseService.GetReviewSummaryAsync(id);

			if (summary is null)
			{
				return Results.NotFound();
			}

			var decisionNote = decisionNoteService.CreateDecisionNote(summary);

			return Results.Ok(decisionNote);
		});

	}
}