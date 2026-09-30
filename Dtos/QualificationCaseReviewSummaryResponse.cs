namespace CredentialCheck.Dtos;

// Fasst den Prüfstand der Dokumente eines Falls zusammen.
public record QualificationCaseReviewSummaryResponse(
	Guid QualificationCaseId,
	string CaseStatus,
	int TotalDocuments,
	int PendingReviewDocuments,
	int AcceptedDocuments,
	int RejectedDocuments,
	int NeedsClarificationDocuments,
	string Recommendation,
	DateTimeOffset? LastActivityAt
);
