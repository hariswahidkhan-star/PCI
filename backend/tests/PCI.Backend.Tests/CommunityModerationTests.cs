using PCI.Backend.Core;
using Xunit;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World community — the moderation policy engine (Core/CommunityModeration.cs; spec §8.4/§8.4.1).
///
/// These assert the SAFETY INVARIANTS of the matrix rather than transcribing its rows. A test that
/// says "hate at High maps to Escalate" only restates the table and passes whatever the table says;
/// a test that says "no Low-confidence signal, in any category, may eject" fails the moment someone
/// edits a row in a way that breaks the guarantee. The properties are the point.
/// </summary>
public class CommunityModerationTests
{
    static readonly System.Collections.Generic.IReadOnlyList<Rule> Matrix = DefaultMatrix();

    // ── The invariant the whole slice exists for ──────────────────────────────────────────────

    [Fact]
    public void OnlyOrdinaryContentIsEverPublished()
    {
        // Nothing outside the ordinary category may resolve to Allow at any band, EXCEPT where the
        // matrix deliberately permits legitimate discussion (news context, self-harm support).
        var deliberateAllows = new[] { "violence_context_ok", "illegal_context_ok", "self_harm_support_ok", "ok" };
        foreach (var category in Categories.All)
            foreach (var band in new[] { Band.Low, Band.Medium, Band.High })
            {
                var d = Resolve(Matrix, new Signal(category, "x", band, null, true));
                if (d.Publishes)
                    Assert.True(deliberateAllows.Contains(d.ReasonCode),
                        $"{category}/{band} published under an unreviewed reason '{d.ReasonCode}'");
            }
    }

    [Fact]
    public void EveryCategoryAndBandResolvesToSomething()
    {
        // A gap in the matrix must not be silent. Whatever the row says, a decision must come back
        // with a reason code that can be shown to the author and audited.
        foreach (var category in Categories.All)
            foreach (var band in new[] { Band.Low, Band.Medium, Band.High })
            {
                var d = Resolve(Matrix, new Signal(category, "x", band, null, true));
                Assert.False(string.IsNullOrWhiteSpace(d.ReasonCode), $"{category}/{band} produced no reason code");
            }
    }

    // ── Fail-closed: the three ways classification can be absent ──────────────────────────────

    [Fact]
    public void AnUnavailableClassifierNeverPublishes()
    {
        // Provider absent, errored or timed out. §28.4: no code path may treat this as permission.
        var d = Resolve(Matrix, Signal.Unavailable);
        Assert.False(d.Publishes);
        Assert.Equal(Outcome.Quarantine, d.Outcome);
        Assert.Equal("moderation_unavailable", d.ReasonCode);
    }

    [Fact]
    public void AnUnknownCategoryNeverPublishes()
    {
        // The classifier returned something the live policy has no row for. A category nobody has
        // ruled on is precisely the case a human should see — not an implicit allow.
        var d = Resolve(Matrix, new Signal("some_new_harm_type", "high", Band.High, null, true));
        Assert.False(d.Publishes);
        Assert.Equal("no_policy_rule", d.ReasonCode);
    }

    [Fact]
    public void AnEmptyPolicyNeverPublishes()
    {
        // A misconfigured or half-migrated policy version must not become an open door.
        var d = Resolve(System.Array.Empty<Rule>(), Signal.Clean());
        Assert.False(d.Publishes);
    }

    [Fact]
    public void TheNullModeratorPublishesNothingAtAll()
    {
        var m = new CommunityModerators.NullModerator();
        foreach (var text in new[] { "hello everyone", "a perfectly ordinary question about EVM", "" })
        {
            var d = Resolve(Matrix, m.Classify(text, "en"));
            Assert.False(d.Publishes, "the no-provider default must not publish");
        }
    }

    // ── Low confidence must never produce an irreversible outcome (§28.5) ─────────────────────

    [Fact]
    public void NoLowConfidenceSignalCanEjectAParticipant()
    {
        foreach (var category in Categories.All)
        {
            var d = Resolve(Matrix, new Signal(category, "x", Band.Low, null, true));
            Assert.NotEqual(Outcome.Eject, d.Outcome);
        }
    }

    [Fact]
    public void UncertainContentIsHeldForReviewRatherThanRefused()
    {
        // The innocent-participant case (PW-US-049): ambiguity goes to a human, not to a sanction.
        foreach (var category in new[] { Categories.ProfanityMild, Categories.TargetedAbuse,
                                         Categories.Hate, Categories.Sexual, Categories.Grooming })
        {
            var d = Resolve(Matrix, new Signal(category, "x", Band.Low, null, true));
            Assert.Equal(Outcome.Quarantine, d.Outcome);
        }
    }

    // ── High confidence must be decisive for severe categories ───────────────────────────────

    [Theory]
    [InlineData(Categories.ProfanityMild)]
    [InlineData(Categories.TargetedAbuse)]
    [InlineData(Categories.Hate)]
    [InlineData(Categories.Sexual)]
    [InlineData(Categories.Violence)]
    [InlineData(Categories.Grooming)]
    public void HighConfidenceSevereContentIsNeverMerelyQuarantined(string category)
    {
        var d = Resolve(Matrix, new Signal(category, "high", Band.High, null, true));
        Assert.False(d.Publishes);
        Assert.True(d.Outcome is Outcome.Eject or Outcome.Escalate,
            $"{category} at high confidence resolved to {d.Outcome}, which is not decisive");
    }

    // ── Restricted handling for the categories §8.7 keeps out of the ordinary queue ───────────

    [Theory]
    [InlineData(Categories.Grooming, Band.Medium)]
    [InlineData(Categories.Grooming, Band.High)]
    [InlineData(Categories.Sexual, Band.High)]
    [InlineData(Categories.Hate, Band.High)]
    public void ChildSafetyAndSevereHarmRouteToRestrictedEscalation(string category, Band band)
    {
        var d = Resolve(Matrix, new Signal(category, "high", band, null, true));
        Assert.Equal(Outcome.Escalate, d.Outcome);
    }

    // ── Mitigating context helps the ambiguous case and never excuses the severe one ──────────

    [Fact]
    public void QuotedContextTurnsABlockIntoAReview()
    {
        var plain = Resolve(Matrix, new Signal(Categories.ProfanityMild, "medium", Band.Medium, null, true));
        var quoted = Resolve(Matrix, new Signal(Categories.ProfanityMild, "medium", Band.Medium, "quoted", true));
        Assert.Equal(Outcome.Block, plain.Outcome);
        Assert.Equal(Outcome.Quarantine, quoted.Outcome);
    }

    [Theory]
    [InlineData(Categories.Grooming)]
    [InlineData(Categories.Hate)]
    [InlineData(Categories.Sexual)]
    [InlineData(Categories.Violence)]
    public void NoClaimedContextExcusesAHighConfidenceSevereFinding(string category)
    {
        // §8.4.1's final row: educational/quoted/documentary framing must never override a genuine
        // severe threat, grooming event or illegal-media rule.
        foreach (var context in new[] { "educational", "quoted", "news", "documentary" })
        {
            var d = Resolve(Matrix, new Signal(category, "high", Band.High, context, true));
            Assert.False(d.Publishes, $"{category} at high confidence published under '{context}' context");
        }
    }

    [Fact]
    public void LegitimateNewsDiscussionOfViolenceIsAllowed()
    {
        // The rules must not be so blunt that professionals cannot discuss an incident.
        var d = Resolve(Matrix, new Signal(Categories.Violence, "low", Band.Low, "news", true));
        Assert.True(d.Publishes);
    }

    // ── Deterministic repetition escalation ──────────────────────────────────────────────────

    [Fact]
    public void RepeatedAbuseEscalatesWithoutTheClassifierChangingItsMind()
    {
        var first = Resolve(Matrix, new Signal(Categories.TargetedAbuse, "medium", Band.Medium, null, true), repetition: 0);
        var third = Resolve(Matrix, new Signal(Categories.TargetedAbuse, "medium", Band.Medium, null, true), repetition: 3);
        Assert.Equal(Outcome.Block, first.Outcome);
        Assert.Equal(Outcome.Eject, third.Outcome);
        Assert.Equal("abuse_repeated", third.ReasonCode);
    }

    // ── Self-harm: support is not suppressed ─────────────────────────────────────────────────

    [Fact]
    public void SomeoneDescribingTheirOwnStruggleIsNotSilenced()
    {
        // The one category where blanket suppression would be the harm. Only encouragement and
        // instruction are withheld; the severe band routes to a crisis runbook, not a sanction.
        var low = Resolve(Matrix, new Signal(Categories.SelfHarm, "low", Band.Low, null, true));
        Assert.True(low.Publishes);

        var severe = Resolve(Matrix, new Signal(Categories.SelfHarm, "high", Band.High, null, true));
        Assert.Equal(Outcome.Escalate, severe.Outcome);
        Assert.NotEqual(Outcome.Eject, severe.Outcome);
    }

    // ── Personal data is withheld immediately ────────────────────────────────────────────────

    [Fact]
    public void PersonalDataIsBlockedEvenWhenUncertain()
    {
        // Publishing someone's phone number is not undone by a later review, so this is the one
        // category the matrix is deliberately stricter about at low confidence.
        var d = Resolve(Matrix, new Signal(Categories.PiiDoxxing, "low", Band.Low, null, true));
        Assert.False(d.Publishes);
        Assert.Equal(Outcome.Block, d.Outcome);
    }

    // ── The decision carries its own audit trail ─────────────────────────────────────────────

    [Fact]
    public void EveryDecisionNamesTheRuleThatProducedIt()
    {
        var d = Resolve(Matrix, new Signal(Categories.Hate, "high", Band.High, null, true));
        Assert.NotNull(d.RuleId);
        Assert.Equal(Categories.Hate, d.Category);
        Assert.Equal(Band.High, d.Confidence);
        Assert.False(string.IsNullOrWhiteSpace(d.ReasonCode));
    }

    // ── The deterministic test double ────────────────────────────────────────────────────────

    [Theory]
    [InlineData("Has anyone modelled the SPI recovery on a late-stage project?")]
    [InlineData("The forecast assumes a 4% escalation rate.")]
    [InlineData("Thanks, that helped a lot.")]
    public void OrdinaryProfessionalMessagesArePublished(string text)
    {
        var m = new CommunityModerators.DeterministicModerator();
        var d = Resolve(Matrix, m.Classify(text, "en"));
        Assert.True(d.Publishes, $"'{text}' was withheld as {d.ReasonCode}");
    }

    [Theory]
    [InlineData("you are an idiot", Outcome.Block)]
    [InlineData("f u c k this", Outcome.Eject)]          // spaced-out evasion
    [InlineData("i will kill you", Outcome.Escalate)]
    public void ObviousViolationsAreCaughtIncludingObfuscated(string text, Outcome expected)
    {
        var m = new CommunityModerators.DeterministicModerator();
        var d = Resolve(Matrix, m.Classify(text, "en"));
        Assert.False(d.Publishes);
        Assert.Equal(expected, d.Outcome);
    }

    [Theory]
    [InlineData("classic project controls")]     // contains "classic", not a slur
    [InlineData("we need to assess the risk")]   // contains "assess"
    [InlineData("Cockburn wrote about this")]    // a real surname
    public void OrdinaryWordsContainingFlaggedSubstringsAreNotBlocked(string text)
    {
        // The Scunthorpe problem: whole-word matching is why these survive.
        var m = new CommunityModerators.DeterministicModerator();
        var d = Resolve(Matrix, m.Classify(text, "en"));
        Assert.True(d.Publishes, $"'{text}' was wrongly withheld as {d.ReasonCode}");
    }

    [Fact]
    public void TheDeterministicModeratorNeverThrows()
    {
        // An exception in the send path would lose the message; an unusable verdict merely fails closed.
        var m = new CommunityModerators.DeterministicModerator();
        foreach (var text in new[] { "", "   ", "​​", new string('a', 20000), "🙂🙂🙂" })
        {
            var s = m.Classify(text, "en");
            Assert.False(Resolve(Matrix, s).Outcome == Outcome.Allow && !s.Usable);
        }
    }
}
