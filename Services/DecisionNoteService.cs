using CredentialCheck.Dtos;

namespace CredentialCheck.Services;

public class DecisionNoteService
{
	public DecisionNoteResponse CreateDecisionNote(QualificationCaseReviewSummaryResponse summary)
	{
		var note = summary.Recommendation switch
		{
			"NoDocuments" =>
				"Für diesen Fall liegen noch keine Dokumente vor. Eine fachliche Bewertung ist daher noch nicht möglich.",

			"NeedsClarification" =>
				$"Für diesen Fall liegen {summary.TotalDocuments} Dokumente vor. Mindestens ein Dokument benötigt Klärung. Vor einer Entscheidung sollten die offenen Punkte geprüft oder weitere Nachweise angefordert werden.",

			"ManualDecisionRequired" =>
				$"Für diesen Fall liegen {summary.TotalDocuments} Dokumente vor. Mindestens ein Dokument wurde abgelehnt. Eine manuelle Entscheidung ist erforderlich.",

			"ReadyForDecision" =>
				$"Für diesen Fall liegen {summary.TotalDocuments} Dokumente vor. Alle Dokumente wurden akzeptiert. Der Fall ist grundsätzlich entscheidungsreif.",

			_ =>
				$"Für diesen Fall liegen {summary.TotalDocuments} Dokumente vor. Die Prüfung ist noch nicht abgeschlossen."
		};

		return new DecisionNoteResponse(
			summary.QualificationCaseId,
			summary.Recommendation,
			note,
			DateTimeOffset.UtcNow
		);
	}
}
