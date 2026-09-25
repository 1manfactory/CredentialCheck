namespace CredentialCheck.Dtos;

public record DecisionNoteResponse(
	Guid QualificationCaseId,
	string Recommendation,
	string Note,
	DateTimeOffset CreatedAt
);
