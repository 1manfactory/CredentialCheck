namespace CredentialCheck.Dtos;

// Enthält den neuen Prüfstatus eines Dokuments.
public record UpdateQualificationDocumentReviewStatusRequest(
	string ReviewStatus
);
