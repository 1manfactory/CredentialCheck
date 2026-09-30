namespace CredentialCheck.Dtos;

// Enthält die Entscheidungsvorlage für einen Fall.
public record DecisionNoteResponse(
	Guid QualificationCaseId,
	string Recommendation,
	string Note,
	DateTimeOffset CreatedAt
);
