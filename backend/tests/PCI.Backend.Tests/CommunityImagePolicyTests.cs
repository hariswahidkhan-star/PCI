using PCI.Backend.Core;
using Xunit;
using static PCI.Backend.Core.CommunityModeration;
using static PCI.Backend.Core.CommunityMediaScanners;

namespace PCI.Backend.Tests;

/// <summary>
/// The image half of the moderation policy (Core/CommunityModeration.DefaultImageMatrix,
/// Core/CommunityMediaScanners; docs/pciworld/CCP_PHASE2_DESIGN.md §6.2).
///
/// Most of these assert PROPERTIES OF THE WHOLE TABLE rather than individual rows. A row-by-row test
/// tells you the table is what it is today; a property test tells you that a change which breaks the
/// safety argument fails, whichever row someone edits. The two that matter most — no image signal
/// ejects anybody, and nothing publishes without a usable verdict — are written that way for exactly
/// that reason.
///
/// None may be relaxed to make an implementation pass (§28.3).
/// </summary>
public class CommunityImagePolicyTests
{
    static readonly IReadOnlyList<Rule> Image = DefaultImageMatrix();

    static Decision ResolveImage(string category, Band band, bool usable = true, string? context = null) =>
        Resolve(Image,
                usable ? new Signal(category, "x", band, context, true) : Signal.Unavailable,
                contentType: "image");

    // ── the two invariants ────────────────────────────────────────────────────────────────────

    [Fact]
    public void NoImageRuleEjectsAnybody()
    {
        // §28.5: a classifier result must never become an irreversible automatic sanction. Image
        // classification is materially less reliable than text classification — a photograph is
        // ambiguous in ways a slur is not — so ending a session on an image signal alone is a
        // decision a person makes, not one this table makes.
        //
        // Asserted over the whole table so the guarantee cannot be eroded one row at a time.
        Assert.DoesNotContain(Image, r => r.Outcome == Outcome.Eject);
    }

    [Fact]
    public void NothingPublishesWithoutAUsableVerdict()
    {
        // Provider absent, errored or timed out — all the same outcome, and it is not publication.
        var d = ResolveImage(Categories.Ordinary, Band.High, usable: false);
        Assert.False(d.Publishes);
        Assert.Equal(Outcome.Quarantine, d.Outcome);
        Assert.Equal("moderation_unavailable", d.ReasonCode);
    }

    [Fact]
    public void AnUnmatchedCategoryDoesNotPublish()
    {
        // The fail-closed default: a category this table has never heard of is not an implicit allow.
        var d = ResolveImage("some_category_invented_later", Band.High);
        Assert.False(d.Publishes);
    }

    [Fact]
    public void TheImageTableCarriesNoMitigatingContextRows()
    {
        // "Educational" or "quoted" are claims a TEXT classifier can find evidence for in the
        // surrounding words. A bare image has no surrounding words, so a context row here would be
        // an excuse with nothing behind it — and would be the first thing an abuser aimed at.
        Assert.DoesNotContain(Image, r => r.ContextRule is not null);
    }

    [Fact]
    public void EveryImageRuleIsScopedToImages()
    {
        // A stray "text" row in this table would be resolved for text messages too, quietly changing
        // Phase 1's behaviour from a Phase 2 file.
        Assert.All(Image, r => Assert.Equal("image", r.ContentType));
    }

    [Fact]
    public void TheTextMatrixIsUnaffectedByTheImageRows()
    {
        // The two tables are separate lists; resolving text against the image table must find
        // nothing rather than silently borrow an image rule.
        var d = Resolve(Image, new Signal(Categories.Hate, "high", Band.High, null, true), contentType: "text");
        Assert.False(d.Publishes);
    }

    // ── routing that the restricted-evidence path depends on ──────────────────────────────────

    [Theory]
    [InlineData(Categories.Sexual)]
    [InlineData(Categories.Grooming)]
    public void ChildSafetyAdjacentCategoriesEscalateRatherThanEnteringTheOrdinaryQueue(string category)
    {
        // §8.7 and §28.7: this evidence must not reach the routine moderator surface. Escalate is
        // the outcome that routes it to restricted handling instead.
        Assert.Equal(Outcome.Escalate, ResolveImage(category, Band.High).Outcome);
    }

    [Fact]
    public void ChildSafetyIndicatorsAreNeverAnOrdinaryBlock()
    {
        // A plain block would leave the item in the normal queue with a normal thumbnail. Above the
        // lowest band, this category must always route out of that surface.
        Assert.Equal(Outcome.Escalate, ResolveImage(Categories.Grooming, Band.Medium).Outcome);
        Assert.Equal(Outcome.Escalate, ResolveImage(Categories.Grooming, Band.High).Outcome);
        Assert.NotEqual(Outcome.Block, ResolveImage(Categories.Grooming, Band.Low).Outcome);
    }

    [Fact]
    public void SomeoneElsesDocumentsAreWithheldAtEveryBand()
    {
        // A screenshot of a passport or a payslip does its damage the moment it is visible, and a
        // later review does not undo it. Same reasoning as the text matrix's doxxing row.
        foreach (var band in new[] { Band.Low, Band.Medium, Band.High })
            Assert.False(ResolveImage(Categories.PiiDoxxing, band).Publishes);
    }

    [Fact]
    public void SelfHarmImageryIsHeldEvenThoughSelfHarmTextIsNot()
    {
        // The text matrix deliberately ALLOWS someone describing their own struggle — suppressing
        // that is a harm. A graphic image of self-injury is a different artefact and is not
        // published while a human looks. This test exists to stop the two being "made consistent".
        Assert.True(Resolve(DefaultMatrix(),
            new Signal(Categories.SelfHarm, "low", Band.Low, null, true), contentType: "text").Publishes);
        Assert.False(ResolveImage(Categories.SelfHarm, Band.Low).Publishes);
    }

    [Fact]
    public void AnOrdinaryImagePublishesAtEveryBand()
    {
        // Without this the engine would be safe and useless.
        foreach (var band in new[] { Band.Low, Band.Medium, Band.High })
            Assert.True(ResolveImage(Categories.Ordinary, band).Publishes);
    }

    // ── the scanners ──────────────────────────────────────────────────────────────────────────

    static MediaSubject Subject(byte[] bytes) =>
        new(bytes, "image/png", 32, 24, "");

    [Fact]
    public void TheDefaultScannerPublishesNothing()
    {
        // The posture when no provider is configured. An operator who enables images without
        // choosing a provider gets a feature that visibly does not publish — not an unscanned feed.
        var signal = new NullMediaScanner().Classify(Subject(new byte[] { 1, 2, 3 }));
        Assert.False(signal.Usable);
        Assert.False(Resolve(Image, signal, contentType: "image").Publishes);
    }

    [Fact]
    public void AProviderOutageWithholdsEverythingRatherThanPublishingEverything()
    {
        // The direction of the failure is the whole point. "Scanner is down, let it through" is the
        // single worst bug this pipeline could have.
        var scanner = new DeterministicMediaScanner(failEverything: true);
        var signal = scanner.Classify(Subject(new byte[] { 9, 9, 9 }));
        Assert.False(Resolve(Image, signal, contentType: "image").Publishes);
    }

    [Fact]
    public void TheTestScannerDrivesEveryBranchFromAHash()
    {
        // Which is what lets the restricted-escalation path be tested with an ordinary generated
        // image rather than with anything real.
        var harmless = new byte[] { 1, 2, 3, 4, 5 };
        var scanner = new DeterministicMediaScanner()
            .Register(harmless, Categories.Grooming, Band.High);

        var d = Resolve(Image, scanner.Classify(Subject(harmless)), contentType: "image");
        Assert.Equal(Outcome.Escalate, d.Outcome);

        // And an unregistered fixture takes the ordinary path, so the happy case is testable too.
        Assert.True(Resolve(Image, scanner.Classify(Subject(new byte[] { 7, 7, 7 })), contentType: "image").Publishes);
    }

    [Fact]
    public void ACallerCannotSelectItsOwnVerdictByPassingAChosenHash()
    {
        // The subject carries a Sha256 field for convenience; the scanner recomputes it. If it
        // trusted the field, anything that could influence it could pick its own moderation outcome.
        var flagged = new byte[] { 4, 4, 4, 4 };
        var scanner = new DeterministicMediaScanner().Register(flagged, Categories.Hate, Band.High);

        var lying = new MediaSubject(flagged, "image/png", 32, 24,
            "0000000000000000000000000000000000000000000000000000000000000000");

        Assert.Equal(Outcome.Escalate, Resolve(Image, scanner.Classify(lying), contentType: "image").Outcome);
    }

    [Fact]
    public void AScannerNeverThrows()
    {
        // An exception escaping a scanner would surface as a 500 and lose the upload — turning a
        // moderation failure into a data-loss bug.
        var scanner = new DeterministicMediaScanner();
        var signal = scanner.Classify(new MediaSubject(Array.Empty<byte>(), "", 0, 0, ""));
        Assert.True(signal.Usable || !signal.Usable);   // the assertion is that we got here at all
    }
}
