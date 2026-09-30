namespace CredentialCheck.Models;

// Beschreibt den aktuellen Prüfstatus eines Dokuments.
public enum DocumentReviewStatus
{
	PendingReview,
	Accepted,
	Rejected,
	NeedsClarification
}
