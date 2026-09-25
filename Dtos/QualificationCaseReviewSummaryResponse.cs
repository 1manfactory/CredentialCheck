namespace CredentialCheck.Dtos;

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
