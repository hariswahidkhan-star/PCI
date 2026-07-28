namespace PCI.Backend.Core;

/// <summary>
/// Forum trust levels (docs/pciworld/CCP_PHASE3_DESIGN.md §3, §8).
///
/// PURE. No database, no clock, no I/O — every input arrives in <see cref="Facts"/>, so the whole
/// promotion and demotion ladder is unit-testable without standing anything up, and two callers
/// cannot disagree about what someone's level is.
///
/// THE ONE THING THIS MODULE MUST NOT DO IS AFFECT A MODERATION VERDICT.
///
/// Trust decides TIMING — whether a post waits for its classification or publishes and is withdrawn
/// if the verdict goes against it. It is deliberately not an input to
/// <see cref="CommunityModeration.Resolve"/>, and there is a test asserting the engine returns the
/// same decision at every level. A trusted author who posts something the policy forbids has it
/// withheld exactly like anybody else; what they bought is a head start, not an exemption.
///
/// DEMOTION IS AS REAL AS PROMOTION.
///
/// A level that only ever rises is a ratchet an abuser plays for: post innocuously until the gate
/// opens, then use it. An upheld report costs standing, and enough of them drop someone back to
/// pre-moderation. That is why <see cref="Facts.UpheldReports"/> is an input rather than something
/// only a human can act on.
/// </summary>
public static class ForumTrust
{
    /// <summary>Ordered least to most capable — the numeric order is meaningful, and
    /// <see cref="AtLeast"/> relies on it.</summary>
    public enum Level
    {
        /// <summary>Brand new. Reads freely; every post is held for classification before anyone
        /// else sees it. The cheapest thing an abuser has is a fresh account, so this is where the
        /// friction belongs.</summary>
        New = 0,
        /// <summary>Verified contact details and one accepted post. May include links.</summary>
        Basic = 1,
        /// <summary>A sustained record. Posts publish immediately and are classified in parallel;
        /// may edit their own posts and raise flags.</summary>
        Member = 2,
        /// <summary>A long record whose flags have proved accurate. Flags carry more weight.</summary>
        Trusted = 3,
        /// <summary>Granted by a World admin, not earned by posting. Moderation capability itself
        /// still comes from WorldRbac — this level does not grant permissions.</summary>
        Staff = 4,
    }

    /// <summary>
    /// Everything the ladder is computed from. All of it is recorded fact rather than judgement, so
    /// a level can always be explained to the person holding it.
    /// </summary>
    /// <param name="EmailVerified">Contact details confirmed.</param>
    /// <param name="AcceptedPosts">Posts that were published and never withdrawn.</param>
    /// <param name="UpheldReports">Reports against this author that a human upheld. Not raw reports —
    /// counting accusations rather than findings would hand any group a demotion button.</param>
    /// <param name="DaysSinceFirstPost">Time is a weak signal on its own and a useful one alongside
    /// volume: it is what stops a burst of posts in an hour buying a gate.</param>
    /// <param name="AccurateFlags">Flags this author raised that a moderator agreed with.</param>
    /// <param name="StaffGranted">Set by an admin. The only level that is not earned.</param>
    public readonly record struct Facts(
        bool EmailVerified, int AcceptedPosts, int UpheldReports,
        int DaysSinceFirstPost, int AccurateFlags, bool StaffGranted);

    // Thresholds live here rather than being scattered through call sites, so "what does it take to
    // reach Member" has exactly one answer and a test can read it.
    public const int BasicPosts = 1;
    public const int MemberPosts = 5;
    public const int MemberDays = 2;
    public const int TrustedPosts = 30;
    public const int TrustedDays = 30;
    public const int TrustedAccurateFlags = 3;

    /// <summary>Upheld reports at which an author returns to pre-moderation regardless of how long
    /// their record is. Deliberately small: the point is to close the gate quickly, and a human can
    /// always restore standing.</summary>
    public const int DemotionUpheldReports = 2;

    /// <summary>
    /// The level these facts earn. Total and deterministic — the same facts always give the same
    /// answer, which is what lets a demotion be explained rather than merely asserted.
    /// </summary>
    public static Level Of(Facts f)
    {
        // Staff is granted, not earned, and is checked first so an admin's grant is not silently
        // overridden by a posting record.
        if (f.StaffGranted) return Level.Staff;

        // Demotion beats every promotion below. An author with upheld findings against them goes
        // back to having their posts held, however long their record is — otherwise a long-standing
        // account is worth acquiring, and accounts that are worth acquiring get acquired.
        if (f.UpheldReports >= DemotionUpheldReports) return Level.New;

        // A single upheld report costs a level rather than nothing. Computing the earned level and
        // then stepping down keeps the penalty proportional instead of cliff-edged.
        var earned = Earned(f);
        if (f.UpheldReports > 0 && earned > Level.New) earned -= 1;
        return earned;
    }

    static Level Earned(Facts f)
    {
        if (!f.EmailVerified) return Level.New;
        if (f.AcceptedPosts >= TrustedPosts && f.DaysSinceFirstPost >= TrustedDays
            && f.AccurateFlags >= TrustedAccurateFlags) return Level.Trusted;
        if (f.AcceptedPosts >= MemberPosts && f.DaysSinceFirstPost >= MemberDays) return Level.Member;
        if (f.AcceptedPosts >= BasicPosts) return Level.Basic;
        return Level.New;
    }

    public static bool AtLeast(Level actual, Level required) => actual >= required;

    /// <summary>
    /// Whether a post from this level waits for its verdict before anyone can see it.
    ///
    /// This is the ONLY thing trust changes about moderation. It does not change what the verdict
    /// is, and it does not change what happens when a verdict goes against the author.
    /// </summary>
    public static bool HoldsBeforePublication(Level level) => level < Level.Member;

    /// <summary>May include hyperlinks. Links are the single most common spam payload, so keeping
    /// them away from brand-new accounts removes most of it before classification has to.</summary>
    public static bool MayPostLinks(Level level) => level >= Level.Basic;

    /// <summary>May edit their own posts. An edit writes a new revision either way; this is about
    /// who may start one.</summary>
    public static bool MayEditOwnPosts(Level level) => level >= Level.Member;

    /// <summary>May flag a post for review. Not a sanction — see <see cref="FlagWeight"/>.</summary>
    public static bool MayFlag(Level level) => level >= Level.Member;

    /// <summary>
    /// How much one flag counts toward hiding a post pending review.
    ///
    /// CAPPED ON PURPOSE. If weight scaled with standing, a handful of trusted accounts — or one
    /// compromised one — could hide arbitrary content on their own. Crossing the threshold opens a
    /// case for a human; it never decides one, and it never sanctions the author.
    /// </summary>
    public static int FlagWeight(Level level) => level switch
    {
        Level.Staff => 3,
        Level.Trusted => 2,
        Level.Member => 1,
        _ => 0,      // below Member, a flag is recorded but carries no weight
    };

    /// <summary>Weighted flags required before a post is hidden pending human review. More than any
    /// single account can supply, by construction: the cap above is 3 and this is 5.</summary>
    public const int HideThreshold = 5;

    /// <summary>Posts allowed per hour. Graded, because a brand-new account posting at volume is
    /// the shape of spam and a long-standing member posting at volume is the shape of a forum.</summary>
    public static int HourlyPostBudget(Level level) => level switch
    {
        Level.Staff => 120,
        Level.Trusted => 60,
        Level.Member => 30,
        Level.Basic => 10,
        _ => 3,
    };

    public static string Name(Level level) => level.ToString().ToLowerInvariant();

    public static Level Parse(string? name) =>
        Enum.TryParse<Level>(name, ignoreCase: true, out var l) ? l : Level.New;
}
