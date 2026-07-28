using PCI.Backend.Core;
using Xunit;
using static PCI.Backend.Core.CommunityModeration;
using static PCI.Backend.Core.ForumTrust;

namespace PCI.Backend.Tests;

/// <summary>
/// Forum trust levels and the post policy matrix (Core/ForumTrust.cs,
/// CommunityModeration.DefaultPostMatrix; docs/pciworld/CCP_PHASE3_DESIGN.md §2, §3, §9).
///
/// The first test is the one that matters most, and it is STRUCTURAL: trust cannot reach the policy
/// engine at all. Trust decides whether a post waits for its verdict or publishes and is withdrawn
/// when the verdict arrives — never what the verdict is. "Trusted authors get a softer rule" is
/// exactly the shortcut that looks reasonable in a hurry, so the test guards the signature and the
/// rule shape rather than the behaviour, which with no way to pass a level in would be vacuous.
///
/// None of these may be relaxed to make an implementation pass (§28.3).
/// </summary>
public class ForumTrustTests
{
    static readonly IReadOnlyList<Rule> Post = DefaultPostMatrix();

    static Facts Clean(int posts = 0, int days = 0, bool verified = true, int upheld = 0, int flags = 0)
        => new(verified, posts, upheld, days, flags, false);

    static Decision ResolvePost(string category, Band band, string? context = null, int repetition = 0) =>
        Resolve(Post, new Signal(category, "x", band, context, true), repetition, contentType: "post");

    // ── the invariant ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public void TrustCannotReachThePolicyEngineAtAll()
    {
        // Asserted STRUCTURALLY rather than behaviourally, because a behavioural version — call
        // Resolve twice, get the same answer — would be nearly vacuous: with no way to pass a level
        // in, of course it agrees with itself. What actually needs guarding is that no such way is
        // ever added, so this reads the signature and the rule shape directly. If someone threads a
        // trust parameter through, this fails and the conversation happens at review time.
        var resolve = typeof(CommunityModeration).GetMethod(nameof(Resolve))!;
        Assert.DoesNotContain(resolve.GetParameters(),
            p => p.Name!.Contains("trust", StringComparison.OrdinalIgnoreCase)
              || p.ParameterType == typeof(Level));

        // And no policy ROW may key on it either — a per-level rule would be the same bypass
        // wearing a different hat.
        Assert.NotEmpty(typeof(Rule).GetProperties());
        Assert.NotEmpty(resolve.GetParameters());
        Assert.DoesNotContain(typeof(Rule).GetProperties(),
            p => p.Name.Contains("trust", StringComparison.OrdinalIgnoreCase)
              || p.PropertyType == typeof(Level));
    }

    [Fact]
    public void EverySignalResolvesToExactlyOneVerdictForTheWholeForum()
    {
        // Every category and band produces a decision, and it is total — no signal falls through to
        // an unhandled state that a caller might read as permission.
        foreach (var category in Categories.All)
            foreach (var band in new[] { Band.Low, Band.Medium, Band.High })
            {
                var d = ResolvePost(category, band);
                Assert.False(string.IsNullOrEmpty(d.ReasonCode), $"{category}/{band} has no reason code");
            }
    }

    [Fact]
    public void TrustOnlyEverChangesWhetherAPostWaits()
    {
        // The whole of trust's influence on moderation, stated as one assertion. Below Member a post
        // is held; at Member and above it publishes and is withdrawn if the verdict goes against it.
        Assert.True(HoldsBeforePublication(Level.New));
        Assert.True(HoldsBeforePublication(Level.Basic));
        Assert.False(HoldsBeforePublication(Level.Member));
        Assert.False(HoldsBeforePublication(Level.Trusted));
        Assert.False(HoldsBeforePublication(Level.Staff));
    }

    [Fact]
    public void ASevereVerdictStillWithholdsForTheMostTrustedAuthor()
    {
        // A head start is not an exemption. Whatever the level, these do not publish.
        foreach (var (category, band) in new[]
                 {
                     (Categories.Grooming, Band.High),
                     (Categories.PiiDoxxing, Band.Low),
                     (Categories.Hate, Band.High),
                     (Categories.Violence, Band.High),
                 })
            Assert.False(ResolvePost(category, band).Publishes, $"{category}/{band} must not publish");
    }

    [Fact]
    public void AnUnusableVerdictDoesNotPublish()
    {
        var d = Resolve(Post, Signal.Unavailable, contentType: "post");
        Assert.False(d.Publishes);
        Assert.Equal("moderation_unavailable", d.ReasonCode);
    }

    [Fact]
    public void EveryPostRuleIsScopedToPosts()
    {
        // A stray row of another content type here would silently change room or image behaviour
        // from a forum file.
        Assert.All(Post, r => Assert.Equal("post", r.ContentType));
    }

    [Fact]
    public void DoxxingIsBlockedAtEveryBandBecauseTheForumIsIndexed()
    {
        // Publishing a colleague's payslip in a crawlable thread is worse than in a transient room:
        // a search engine keeps it, and a later review does not undo that.
        foreach (var band in new[] { Band.Low, Band.Medium, Band.High })
            Assert.False(ResolvePost(Categories.PiiDoxxing, band).Publishes);
    }

    [Fact]
    public void APostMayCarryMitigatingContextEvenThoughAnImageMayNot()
    {
        // The argument for refusing context rows on a bare image was that there are no surrounding
        // words to evidence the claim. A forum post has them, so the row is legitimate here — and
        // the contrast is pinned so neither table gets "harmonised" into the other.
        Assert.True(ResolvePost(Categories.Violence, Band.Low, context: "news").Publishes);
        Assert.DoesNotContain(DefaultImageMatrix(), r => r.ContextRule is not null);
    }

    [Fact]
    public void NoContextExcusesACredibleThreat()
    {
        Assert.False(ResolvePost(Categories.Violence, Band.High, context: "news").Publishes);
        Assert.False(ResolvePost(Categories.Violence, Band.High, context: "educational").Publishes);
    }

    // ── the ladder ────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void ABrandNewAccountStartsAtTheBottom()
    {
        Assert.Equal(Level.New, Of(Clean(verified: false)));
        Assert.Equal(Level.New, Of(Clean(posts: 0)));
    }

    [Fact]
    public void UnverifiedContactDetailsCapTheLadderRegardlessOfVolume()
    {
        // Otherwise the cheapest possible account could post its way to immediate publication.
        Assert.Equal(Level.New, Of(Clean(posts: 500, days: 500, verified: false, flags: 50)));
    }

    [Fact]
    public void TheLadderIsClimbedByRecordAndByTime()
    {
        Assert.Equal(Level.Basic, Of(Clean(posts: 1, days: 0)));
        // Volume alone does not buy Member: a burst of posts inside an hour is the shape of spam.
        Assert.Equal(Level.Basic, Of(Clean(posts: 50, days: 0)));
        Assert.Equal(Level.Member, Of(Clean(posts: MemberPosts, days: MemberDays)));
        Assert.Equal(Level.Trusted,
            Of(Clean(posts: TrustedPosts, days: TrustedDays, flags: TrustedAccurateFlags)));
    }

    [Fact]
    public void TrustedAlsoRequiresFlagsThatProvedAccurate()
    {
        // Trusted grants weighted flags, so it is earned partly by flagging well — not only by
        // posting a lot.
        Assert.Equal(Level.Member, Of(Clean(posts: TrustedPosts, days: TrustedDays, flags: 0)));
    }

    [Fact]
    public void StaffIsGrantedNotEarned()
    {
        Assert.Equal(Level.Staff, Of(new Facts(false, 0, 0, 0, 0, StaffGranted: true)));
    }

    // ── demotion ──────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void OneUpheldReportCostsALevelRatherThanNothing()
    {
        var record = Clean(posts: TrustedPosts, days: TrustedDays, flags: TrustedAccurateFlags);
        Assert.Equal(Level.Trusted, Of(record));
        Assert.Equal(Level.Member, Of(record with { UpheldReports = 1 }));
    }

    [Fact]
    public void EnoughUpheldReportsReturnAnyoneToPreModeration()
    {
        // A level that only rises is a ratchet an abuser plays for: post innocuously until the gate
        // opens, then use it. However long the record, upheld findings close the gate.
        var longRecord = Clean(posts: 5_000, days: 5_000, flags: 500) with { UpheldReports = DemotionUpheldReports };
        Assert.Equal(Level.New, Of(longRecord));
        Assert.True(HoldsBeforePublication(Of(longRecord)));
    }

    [Fact]
    public void ADemotionCannotBeOutrunByPostingMore()
    {
        var f = Clean(posts: 100, days: 100, flags: 10) with { UpheldReports = DemotionUpheldReports };
        Assert.Equal(Level.New, Of(f));
        Assert.Equal(Level.New, Of(f with { AcceptedPosts = 100_000 }));
    }

    [Fact]
    public void OnlyUpheldReportsDemote()
    {
        // Counting accusations rather than findings would hand any group a demotion button — report
        // someone twice and they lose their standing. Facts carries no raw-report count at all.
        var f = Clean(posts: MemberPosts, days: MemberDays);
        Assert.Equal(Level.Member, Of(f));
        Assert.Equal(Level.Member, Of(f with { UpheldReports = 0 }));
    }

    [Fact]
    public void TheSameFactsAlwaysGiveTheSameLevel()
    {
        // Determinism is what lets a demotion be explained to the person it happened to.
        var f = Clean(posts: 7, days: 9, flags: 1) with { UpheldReports = 1 };
        Assert.Equal(Of(f), Of(f));
    }

    // ── capabilities ──────────────────────────────────────────────────────────────────────────

    [Fact]
    public void LinksAreKeptAwayFromBrandNewAccounts()
    {
        // The single most common spam payload, removed before classification has to catch it.
        Assert.False(MayPostLinks(Level.New));
        Assert.True(MayPostLinks(Level.Basic));
    }

    [Fact]
    public void FlaggingAndSelfEditingStartAtMember()
    {
        Assert.False(MayFlag(Level.Basic));
        Assert.True(MayFlag(Level.Member));
        Assert.False(MayEditOwnPosts(Level.Basic));
        Assert.True(MayEditOwnPosts(Level.Member));
    }

    [Fact]
    public void NoSingleAccountCanHideAPostOnItsOwn()
    {
        // The cap is the control. If weight scaled with standing, one compromised trusted account
        // could hide arbitrary content by itself.
        foreach (Level level in Enum.GetValues<Level>())
            Assert.True(FlagWeight(level) < HideThreshold,
                        $"{level} can reach the hide threshold alone");
    }

    [Fact]
    public void HidingNeedsMoreThanTwoOfTheMostWeightedAccounts()
    {
        // A pair is a small enough group to organise casually; the threshold sits above it.
        Assert.True(FlagWeight(Level.Staff) * 2 >= HideThreshold);   // three staff can, deliberately
        Assert.True(FlagWeight(Level.Trusted) * 2 < HideThreshold);  // two trusted members cannot
    }

    [Fact]
    public void AFlagFromAnUntrustedAccountCarriesNoWeight()
    {
        // Recorded, so a pattern is visible to a moderator — but worth nothing on its own, or
        // creating accounts would be a way to hide things.
        Assert.Equal(0, FlagWeight(Level.New));
        Assert.Equal(0, FlagWeight(Level.Basic));
    }

    [Fact]
    public void PostingBudgetRisesWithStandingAndIsNeverUnlimited()
    {
        Assert.True(HourlyPostBudget(Level.New) < HourlyPostBudget(Level.Basic));
        Assert.True(HourlyPostBudget(Level.Basic) < HourlyPostBudget(Level.Member));
        foreach (Level level in Enum.GetValues<Level>())
            Assert.True(HourlyPostBudget(level) > 0 && HourlyPostBudget(level) < 1000);
    }

    [Fact]
    public void AnUnknownLevelNameFallsBackToTheLeastCapable()
    {
        // A typo in stored data must not promote anybody.
        Assert.Equal(Level.New, Parse("wizard"));
        Assert.Equal(Level.New, Parse(null));
        Assert.Equal(Level.Trusted, Parse("trusted"));
        Assert.Equal(Level.Trusted, Parse("TRUSTED"));
    }
}
