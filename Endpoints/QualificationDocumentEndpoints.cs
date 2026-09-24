using CredentialCheck.Dtos;
using CredentialCheck.Services;

namespace CredentialCheck.Endpoints;

public static class QualificationDocumentEndpoints
{
	public static void MapQualificationDocumentEndpoints(this WebApplication app)
	{
		app.MapGet("/api/cases/{caseId:guid}/documents", async (
			Guid caseId,
			QualificationDocumentService service) =>
		{
			try
			{
				var documents = await service.GetByCaseIdAsync(caseId);
				return Results.Ok(documents);
			}
			catch (KeyNotFoundException ex)
			{
				return Results.NotFound(new { error = ex.Message });
			}
		});

		app.MapPost("/api/cases/{caseId:guid}/documents", async (
			Guid caseId,
			CreateQualificationDocumentRequest request,
			QualificationDocumentService service) =>
		{
			if (string.IsNullOrWhiteSpace(request.DocumentType))
			{
				return Results.BadRequest(new { error = "DocumentType is required." });
			}

			if (string.IsNullOrWhiteSpace(request.Title))
			{
				return Results.BadRequest(new { error = "Title is required." });
			}

			if (string.IsNullOrWhiteSpace(request.IssuingInstitution))
			{
				return Results.BadRequest(new { error = "IssuingInstitution is required." });
			}

			if (string.IsNullOrWhiteSpace(request.Country))
			{
				return Results.BadRequest(new { error = "Country is required." });
			}

			try
			{
				var document = await service.CreateAsync(caseId, request);
				return Results.Created($"/api/cases/{caseId}/documents/{document.Id}", document);
			}
			catch (KeyNotFoundException ex)
			{
				return Results.NotFound(new { error = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return Results.BadRequest(new { error = ex.Message });
			}
		});
	}
}
