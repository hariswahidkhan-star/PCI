using PCI.SecureExam.Core;
using PCI.SecureExam.Core.Models;
using Xunit;

namespace PCI.SecureExam.Tests;

/// <summary>
/// Guards the one business-critical rule of the post-submission screen: a HELD result must never
/// surface a score or a pass/fail. The WPF screen renders straight from SubmittedView, so proving
/// the view-model here proves the screen cannot leak a held outcome.
/// </summary>
public class SubmittedViewTests
{
    private static readonly IReadOnlyList<DomainBand> NoBands = new List<DomainBand>();

    [Fact]
    public void Held_result_shows_hold_message_and_no_score()
    {
        var r = new SubmitResult(Ok: true, Percent: 0, Result: "", CredentialId: null, Breakdown: NoBands,
            Held: true, Message: null, ResultStatus: "auto_held");
        var v = SubmittedView.For(r);

        Assert.True(v.Held);
        Assert.False(v.ShowScore);
        Assert.Null(v.ScorePercent);
        Assert.Null(v.CredentialLine);
        Assert.Contains("hold", v.Title, System.StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("pass", v.Title, System.StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("%", v.Body);
    }

    [Fact]
    public void Held_result_never_shows_score_even_if_payload_carries_one()
    {
        // Defence in depth: even a malformed payload that (wrongly) includes a percent + pass on a held
        // result must not surface it. Held wins.
        var r = new SubmitResult(Ok: true, Percent: 92.5, Result: "pass", CredentialId: "PCP-AI-2026-12345",
            Breakdown: NoBands, Held: true, Message: "On hold for review.", ResultStatus: "auto_held");
        var v = SubmittedView.For(r);

        Assert.False(v.ShowScore);
        Assert.Null(v.ScorePercent);
        Assert.Null(v.CredentialLine);
        Assert.Equal("On hold for review.", v.Body);
    }

    [Fact]
    public void Passed_result_shows_score_and_credential()
    {
        var r = new SubmitResult(Ok: true, Percent: 78, Result: "pass", CredentialId: "PCP-AI-2026-54321",
            Breakdown: NoBands, Held: false, Message: null, ResultStatus: "credential_issued");
        var v = SubmittedView.For(r);

        Assert.False(v.Held);
        Assert.True(v.ShowScore);
        Assert.Equal(78, v.ScorePercent);
        Assert.Contains("passed", v.Title, System.StringComparison.OrdinalIgnoreCase);
        Assert.NotNull(v.CredentialLine);
        Assert.Contains("PCP-AI-2026-54321", v.CredentialLine!);
    }

    [Fact]
    public void Failed_result_shows_score_without_credential()
    {
        var r = new SubmitResult(Ok: true, Percent: 40, Result: "fail", CredentialId: null,
            Breakdown: NoBands, Held: false, Message: null, ResultStatus: "released_fail");
        var v = SubmittedView.For(r);

        Assert.True(v.ShowScore);
        Assert.Equal(40, v.ScorePercent);
        Assert.Null(v.CredentialLine);
        Assert.DoesNotContain("passed", v.Title, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Null_result_is_a_neutral_submitted_screen()
    {
        var v = SubmittedView.For(null);
        Assert.False(v.Held);
        Assert.False(v.ShowScore);
        Assert.Null(v.ScorePercent);
    }
}
