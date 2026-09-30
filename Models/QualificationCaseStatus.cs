namespace CredentialCheck.Models;

// Beschreibt den Bearbeitungsstand eines Qualifikationsfalls.
public enum QualificationCaseStatus
{
    Draft,
    InReview,
    WaitingForDocuments,
    ReadyForDecision,
    Completed
}