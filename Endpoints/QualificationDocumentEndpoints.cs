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

		app.MapPost("/api/cases/{caseId:guid}/documents/upload", async (
			Guid caseId,
			HttpRequest httpRequest,
			QualificationDocumentService documentService,
			DocumentFileStorageService storageService) =>
		{
			if (!httpRequest.HasFormContentType)
			{
				return Results.BadRequest(new { error = "Request must be multipart/form-data." });
			}

			var form = await httpRequest.ReadFormAsync();

			var file = form.Files.GetFile("file");

			if (file is null || file.Length == 0)
			{
				return Results.BadRequest(new { error = "File is required." });
			}

			var request = new CreateQualificationDocumentRequest(
				form["documentType"].ToString(),
				form["title"].ToString(),
				form["issuingInstitution"].ToString(),
				form["country"].ToString(),
				DateOnly.TryParse(form["issueDate"].ToString(), out var issueDate)
					? issueDate
					: null
			);

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
				var storedFile = await storageService.SaveAsync(file);
				var document = await documentService.CreateAsync(caseId, request, storedFile);

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

		app.MapPatch("/api/cases/{caseId:guid}/documents/{documentId:guid}/review-status", async (
			Guid caseId,
			Guid documentId,
			UpdateQualificationDocumentReviewStatusRequest request,
			QualificationDocumentService service) =>
		{
			try
			{
				var document = await service.UpdateReviewStatusAsync(caseId, documentId, request);

				return document is null
					? Results.NotFound()
					: Results.Ok(document);
			}
			catch (InvalidOperationException ex)
			{
				return Results.BadRequest(new { error = ex.Message });
			}
		});

		app.MapGet("/api/cases/{caseId:guid}/documents/{documentId:guid}/file", async (
			Guid caseId,
			Guid documentId,
			QualificationDocumentService documentService) =>
		{
			var document = await documentService.GetDocumentEntityAsync(caseId, documentId);

			if (document is null)
			{
				return Results.NotFound(new { error = "Document not found." });
			}

			if (string.IsNullOrWhiteSpace(document.StoragePath) ||
				!File.Exists(document.StoragePath))
			{
				return Results.NotFound(new { error = "Stored file not found." });
			}

			var contentType = string.IsNullOrWhiteSpace(document.ContentType)
				? "application/octet-stream"
				: document.ContentType;

			var downloadFileName = string.IsNullOrWhiteSpace(document.OriginalFileName)
				? document.StoredFileName ?? "document"
				: document.OriginalFileName;

			return Results.File(
				document.StoragePath,
				contentType,
				downloadFileName
			);
		});

	}
}
