using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.Core;

/// <summary>
/// Pure presentation decision for the post-submission screen. Kept in Core (no WPF) so the
/// business-critical rule — a HELD result must NEVER surface a score or a pass/fail outcome — is
/// unit-tested without a Windows GUI. MainWindow.RenderSubmitted renders from this view-model.
///
/// The invariant: when <see cref="ShowScore"/> is false there is no numeric score or pass/fail to
/// display. A held submission always yields ShowScore=false and ScorePercent=null, regardless of
/// whatever the payload happens to carry (defence in depth alongside the server-side redaction).
/// </summary>
public sealed record SubmittedView(
    string Title,
    string Body,
    string? CredentialLine,
    bool ShowScore,
    double? ScorePercent,
    bool Held)
{
    private const string HoldDefault =
        "Your responses were submitted successfully. Your result is on hold pending an examination " +
        "integrity check. PCI will notify you through the Student Portal once the review is complete.";

    public static SubmittedView For(SubmitResult? r)
    {
        if (r is null)
            return new(
                "Submitted",
                "Your answers were submitted. If your result isn’t shown, it will appear in the PCI Student Portal shortly.",
                CredentialLine: null, ShowScore: false, ScorePercent: null, Held: false);

        if (r.Held)
            // Held for integrity review — NEVER a score or pass/fail, and never a credential.
            return new(
                "Examination submitted — result on hold",
                string.IsNullOrWhiteSpace(r.Message) ? HoldDefault : r.Message!,
                CredentialLine: null, ShowScore: false, ScorePercent: null, Held: true);

        var passed = string.Equals(r.Result, "pass", StringComparison.OrdinalIgnoreCase);
        var credential = string.IsNullOrEmpty(r.CredentialId)
            ? null
            : $"Credential issued: {r.CredentialId} — verifiable from the portal.";
        return new(
            passed ? "Congratulations — you passed" : "Examination submitted",
            "A full score report and any credential are available in your PCI Student Portal.",
            credential, ShowScore: true, ScorePercent: r.Percent, Held: false);
    }
}
