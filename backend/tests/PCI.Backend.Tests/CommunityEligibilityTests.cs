using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;
using static PCI.Backend.Core.CommunityEligibility;

namespace PCI.Backend.Tests;

/// <summary>
/// The age and jurisdiction gate (Core/CommunityEligibility.cs) — the engineering limb of
/// CCP-P1-003 (§7.1, §19.3).
///
/// The first thing these tests establish is what the gate does NOT do. It records a declaration; it
/// does not verify one. Several assertions below exist specifically to stop the mechanism drifting
/// into something that gets described as "age verification" later — most importantly that no date
/// of birth is ever stored, which is both a privacy property and a standing reminder of what kind
/// of evidence this is.
///
/// The second is that it is fail-closed by construction: with nothing configured it admits nobody,
/// and that is asserted rather than left as a default somebody might "fix".
/// </summary>
[Collection(DbCollection.Name)]
public class CommunityEligibilityTests
{
    static readonly DateTime Today = new(2026, 07, 28, 0, 0, 0, DateTimeKind.Utc);

    static Db NewDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static Db Configured(string jurisdictions = "GB,AE,PK", string minAge = "18")
    {
        var db = NewDb();
        Settings.Put(db, JurisdictionsKey, jurisdictions);
        Settings.Put(db, MinAgeKey, minAge);
        return db;
    }

    static string Born(int yearsAgo, int monthOffset = 0) =>
        Today.AddYears(-yearsAgo).AddMonths(-monthOffset).ToString("yyyy-MM-dd");

    // ── fail-closed by construction ───────────────────────────────────────────────────────────

    [Fact]
    public void WithNothingConfiguredNobodyIsEligible()
    {
        // CCP-P1-003 is open precisely because counsel has not said which jurisdictions PCI serves.
        // Until somebody with that authority fills the list in, admitting no one is the honest
        // behaviour — guessing a default would quietly become the policy.
        var db = NewDb();
        var d = Evaluate(db, Born(40), "GB", Today);
        Assert.False(d.Allowed);
        Assert.Equal(Verdict.NoJurisdictionsConfigured, d.Verdict);
        Assert.False(CommunityEligibility.Configured(db));
    }

    [Fact]
    public void AnEmptyOrJunkJurisdictionListStillAdmitsNobody()
    {
        foreach (var value in new[] { "", "   ", ",,,", "United Kingdom", "GBR", "1", "*" })
        {
            var db = NewDb();
            Settings.Put(db, JurisdictionsKey, value);
            Assert.False(Evaluate(db, Born(40), "GB", Today).Allowed);
            Assert.False(CommunityEligibility.Configured(db));
        }
    }

    [Fact]
    public void AWildcardIsNotAWayToOpenEverywhere()
    {
        // Only well-formed two-letter codes are honoured, so nobody can open the service globally
        // by typing '*' into a settings box the way a CORS origin would allow.
        var db = NewDb();
        Settings.Put(db, JurisdictionsKey, "*");
        Assert.False(Evaluate(db, Born(40), "*", Today).Allowed);
    }

    // ── what it does not establish ────────────────────────────────────────────────────────────

    [Fact]
    public void NoDateOfBirthIsEverStored()
    {
        // Both a privacy property and a standing statement of what kind of evidence this is. A
        // birthday list for anonymous guests would be identifying personal data the service has no
        // use for and would then have to protect, disclose and delete.
        var db = Configured();
        var d = Evaluate(db, "1990-03-14", "GB", Today);
        Assert.True(d.Allowed);
        RecordDeclaration(db, 0, d, "v1");

        var cols = db.Columns("pciworld_community_eligibility_log");
        Assert.DoesNotContain("birth_date", cols);
        Assert.DoesNotContain("date_of_birth", cols);
        Assert.DoesNotContain("dob", cols);

        var row = db.QueryOne("SELECT * FROM pciworld_community_eligibility_log ORDER BY id DESC");
        Assert.NotNull(row);
        // Only a coarse band survives — enough to evidence the gate, not enough to identify anyone.
        Assert.Equal("25_39", H.Str(row!["declared_age_band"]));
    }

    [Fact]
    public void ARefusalIsRecordedEvenThoughNoSessionExists()
    {
        // The evidence that matters most is the refusal: it shows the gate was applied to someone
        // it turned away. There is no session to attach it to precisely because it was refused.
        var db = Configured();
        var d = Evaluate(db, Born(15), "GB", Today);
        Assert.False(d.Allowed);
        RecordDeclaration(db, 0, d, "v1");

        var row = db.QueryOne("SELECT outcome,declared_age_band FROM pciworld_community_eligibility_log ORDER BY id DESC");
        Assert.Equal("underminimumage", H.Str(row!["outcome"]));
    }

    [Fact]
    public void TheDecisionRecordsWhichPolicyWasInForce()
    {
        // A decision taken under last month's minimum age must still be explainable after the
        // minimum changes, or the log is a list of outcomes nobody can interpret.
        var db = Configured();
        RecordDeclaration(db, 0, Evaluate(db, Born(30), "GB", Today), "v7");
        var row = db.QueryOne("SELECT policy_version,min_age_required FROM pciworld_community_eligibility_log ORDER BY id DESC");
        Assert.Equal("v7", H.Str(row!["policy_version"]));
        Assert.Equal(18, H.L(row["min_age_required"]));
    }

    // ── the age arithmetic ────────────────────────────────────────────────────────────────────

    [Fact]
    public void SomeoneWhoseBirthdayHasNotArrivedThisYearIsStillTheYoungerAge()
    {
        // The off-by-one here is the difference between admitting a seventeen-year-old and not, so
        // it is computed by comparing month and day rather than dividing by 365.25.
        var justUnder = new DateTime(2008, 12, 31);   // turns 18 on 2026-12-31, after `Today`
        Assert.Equal(17, AgeOn(justUnder, Today));

        var justOver = new DateTime(2008, 07, 27);    // turned 18 yesterday
        Assert.Equal(18, AgeOn(justOver, Today));

        var exactlyToday = new DateTime(2008, 07, 28); // turns 18 today — eighteen, not seventeen
        Assert.Equal(18, AgeOn(exactlyToday, Today));
    }

    [Fact]
    public void TheDayBeforeTheBirthdayIsRefusedAndTheDayOfIsAdmitted()
    {
        var db = Configured();
        Assert.False(Evaluate(db, "2008-07-29", "GB", Today).Allowed);   // 18 tomorrow
        Assert.True(Evaluate(db, "2008-07-28", "GB", Today).Allowed);    // 18 today
    }

    [Fact]
    public void AnImplausibleOrFutureDateIsRefusedRatherThanComputed()
    {
        var db = Configured();
        Assert.Equal(Verdict.ImplausibleDeclaration, Evaluate(db, "2030-01-01", "GB", Today).Verdict);
        Assert.Equal(Verdict.ImplausibleDeclaration, Evaluate(db, "1850-01-01", "GB", Today).Verdict);
    }

    [Fact]
    public void AMalformedDateIsARefusalNotAnException()
    {
        // The entry form is reachable by anyone, so a junk value must not become a 500.
        var db = Configured();
        foreach (var junk in new[] { "not-a-date", "14/03/1990", "1990-13-45", "", "   ", "1990" })
            Assert.False(Evaluate(db, junk, "GB", Today).Allowed);
    }

    // ── the configured values ─────────────────────────────────────────────────────────────────

    [Fact]
    public void TheMinimumAgeIsHonouredWhenItIsRaised()
    {
        var db = Configured(minAge: "21");
        Assert.False(Evaluate(db, Born(20), "GB", Today).Allowed);
        Assert.True(Evaluate(db, Born(21), "GB", Today).Allowed);
    }

    [Fact]
    public void AnAbsurdMinimumAgeSettingIsIgnoredRatherThanObeyed()
    {
        // Someone typing 1 into a settings box must not open the service to children, and someone
        // typing 99 must not silently close it. Out-of-band values fall back to the safe default.
        foreach (var absurd in new[] { "1", "0", "-5", "99", "abc", "" })
        {
            var db = Configured(minAge: absurd);
            Assert.Equal(DefaultMinAge, MinAge(db));
            Assert.False(Evaluate(db, Born(15), "GB", Today).Allowed);
        }
    }

    [Fact]
    public void OnlyApprovedJurisdictionsAreAdmitted()
    {
        var db = Configured("GB,AE");
        Assert.True(Evaluate(db, Born(30), "GB", Today).Allowed);
        Assert.True(Evaluate(db, Born(30), "ae", Today).Allowed);      // case-insensitive
        Assert.False(Evaluate(db, Born(30), "US", Today).Allowed);
        Assert.Equal(Verdict.JurisdictionNotServed, Evaluate(db, Born(30), "US", Today).Verdict);
    }

    [Fact]
    public void AMissingJurisdictionIsRefused()
    {
        var db = Configured();
        Assert.Equal(Verdict.NoJurisdiction, Evaluate(db, Born(30), null, Today).Verdict);
        Assert.Equal(Verdict.NoJurisdiction, Evaluate(db, Born(30), "  ", Today).Verdict);
    }

    [Fact]
    public void JurisdictionIsCheckedBeforeAge()
    {
        // If the service is not open where someone is, their date of birth is not our business and
        // should not be collected — let alone recorded.
        var db = Configured("GB");
        var d = Evaluate(db, Born(12), "US", Today);
        Assert.Equal(Verdict.JurisdictionNotServed, d.Verdict);
        Assert.Null(d.DeclaredAge);
        Assert.Equal("unknown", AgeBand(d.DeclaredAge));
    }

    // ── what the person is told ───────────────────────────────────────────────────────────────

    [Fact]
    public void TheReasonCodeDoesNotLetSomeoneMapThePolicy()
    {
        // "Not served" and "no jurisdictions configured at all" are distinguishable to an operator
        // and identical to a participant, so the endpoint is not an oracle for probing the allowlist.
        var db = Configured("GB");
        Assert.Equal("not_available_in_your_location", Evaluate(db, Born(30), "US", Today).ReasonCode);

        var unconfigured = NewDb();
        Assert.Equal("not_available_in_your_location", Evaluate(unconfigured, Born(30), "US", Today).ReasonCode);
    }

    [Fact]
    public void AnEligiblePersonIsSimplyEligible()
    {
        var db = Configured();
        var d = Evaluate(db, Born(35), "PK", Today);
        Assert.True(d.Allowed);
        Assert.Equal("eligible", d.ReasonCode);
        Assert.Equal("PK", d.Jurisdiction);
        Assert.Equal(35, d.DeclaredAge);
    }

    [Fact]
    public void AgeBandsAreCoarseEnoughToBeEvidenceAndNotAProfile()
    {
        Assert.Equal("unknown", AgeBand(null));
        Assert.Equal("under_13", AgeBand(9));
        Assert.Equal("16_17", AgeBand(17));
        Assert.Equal("18_24", AgeBand(18));
        Assert.Equal("60_plus", AgeBand(75));
    }
}
