namespace CredentialCheck.Models;

// Legt die möglichen Empfehlungen nach der Dokumentenprüfung fest.
public enum ReviewRecommendation
{
	NoDocuments,
	InReview,
	NeedsClarification,
	ManualDecisionRequired,
	ReadyForDecision
}
