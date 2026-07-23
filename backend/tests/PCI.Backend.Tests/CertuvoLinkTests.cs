using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-6 — the Certuvo hand-off decision helpers (<see cref="CertuvoLink"/>): the pure / database-backed
/// units that decide whether a member gets a Certuvo practice account and under what identity, WITHOUT ever
/// dialing the network. Only the offline decision surface is exercised here — Enabled (the master switch
/// every other path checks first so no HTTP fires), Eligible (the operator-configured membership /
/// enrolment rule), DetectMemberType (the coarse member-category label and its precedence), and
/// NextUsername (PCI-owned username generation, sequence bump on collision, prefix fallback). Provision()
/// and the other async methods perform real HTTP I/O and are deliberately out of scope. Each test runs on a
/// fresh migrated SQLite database (TestEnv.NewMigratedDb) with FK enforcement ON, so a real user is seeded
/// before any dependent row. Assertions read the returned value / database state, not integration output.
/// Nothing here changes application code; it pins behaviour that already ships.
/// </summary>
public class CertuvoLinkTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static long SeedUser(Db db, string email = "certuvo@test.io")
    {
        // FK enforcement is ON — every membership / entitlement / certuvo_accounts row needs a real user.
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)", email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    // An active membership — the state the default eligibility rule reads.
    static void SeedActiveMembership(Db db, long uid)
        => db.Execute("INSERT INTO memberships(user_id,membership_type,status) VALUES(?, 'Student Membership','active')", uid);

    // A paid-exam entitlement. certification_id defaults to 1 (the seeded founding certification), satisfying
    // its FK; status drives whether Eligible()'s HasExam() counts it.
    static void SeedExamEntitlement(Db db, long uid, string status = "available")
        => db.Execute("INSERT INTO exam_entitlements(user_id,product_type,status,certification_id) VALUES(?, 'exam',?,1)", uid, status);

    // The latest membership/bundle/renewal payment is what DetectMemberType classifies.
    static void SeedMembershipPayment(Db db, long uid, string provider, string status, double amount, string? discountCode = null)
        => db.Execute("INSERT INTO payments(user_id,product_type,payment_provider,payment_status,final_amount,discount_code) VALUES(?, 'membership',?,?,?,?)",
            uid, provider, status, amount, discountCode);

    static void SeedHonoraryAward(Db db, long uid)
        // award_no is UNIQUE NOT NULL; conferred_by is NOT NULL (an admin id, no FK). status active.
        => db.Execute("INSERT INTO honorary_awards(award_no,recipient_name,user_id,conferred_by,status) VALUES(?, 'Test Recipient', ?, 1, 'active')",
            "HON-" + System.Guid.NewGuid().ToString("n"), uid);

    static string Year() => System.DateTime.UtcNow.ToString("yyyy");

    // ---- Enabled: the master switch every network path checks first ------------------------------

    [Fact]
    public void Enabled_Unconfigured_IsFalse()
    {
        var db = Fresh();
        // No certuvo_enabled setting is seeded → Settings.Bool falls back to false. This is what keeps the
        // whole integration dormant in every other test (Provision/RetryDue no-op the moment Enabled is false).
        Assert.False(CertuvoLink.Enabled(db));
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("true", true)]
    [InlineData("TRUE", true)]     // case-insensitive
    [InlineData("0", false)]
    [InlineData("false", false)]
    public void Enabled_ReflectsConfiguredFlag(string value, bool expected)
    {
        var db = Fresh();
        Settings.Put(db, "certuvo_enabled", value);
        Assert.Equal(expected, CertuvoLink.Enabled(db));
    }

    // ---- Eligible: the operator-configured membership / enrolment rule ---------------------------

    [Theory]
    // certuvo_requires unset → defaults to "membership": active membership required, exam alone irrelevant.
    [InlineData(null, true, false, true)]
    [InlineData(null, false, false, false)]
    // "membership" (explicit): active membership required; a paid exam without membership does NOT qualify.
    [InlineData("membership", true, false, true)]
    [InlineData("membership", false, true, false)]
    // "membership_and_enrolment": BOTH an active membership AND a paid exam entitlement.
    [InlineData("membership_and_enrolment", true, false, false)]
    [InlineData("membership_and_enrolment", true, true, true)]
    [InlineData("membership_and_enrolment", false, true, false)]
    // "membership_or_exam": EITHER qualifies; neither does not.
    [InlineData("membership_or_exam", true, false, true)]
    [InlineData("membership_or_exam", false, true, true)]
    [InlineData("membership_or_exam", false, false, false)]
    // An unrecognised rule value falls through the switch's default arm → treated as "membership".
    [InlineData("something_unknown", true, false, true)]
    [InlineData("something_unknown", false, true, false)]
    public void Eligible_ByRuleAndState(string? rule, bool hasMembership, bool hasExam, bool expected)
    {
        var db = Fresh();
        var uid = SeedUser(db);
        if (rule is not null) Settings.Put(db, "certuvo_requires", rule);
        if (hasMembership) SeedActiveMembership(db, uid);
        if (hasExam) SeedExamEntitlement(db, uid);
        Assert.Equal(expected, CertuvoLink.Eligible(db, uid));
    }

    [Fact]
    public void Eligible_InactiveMembership_IsNotEligible()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // Only status='active' counts; an inactive membership row does not satisfy the default rule.
        db.Execute("INSERT INTO memberships(user_id,membership_type,status) VALUES(?, 'Student Membership','inactive')", uid);
        Assert.False(CertuvoLink.Eligible(db, uid));
    }

    [Fact]
    public void Eligible_MembershipOrExam_IgnoresNonQualifyingEntitlementStatus()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        Settings.Put(db, "certuvo_requires", "membership_or_exam");
        // HasExam() only counts available/booked/consumed — a revoked entitlement (and no membership) fails.
        SeedExamEntitlement(db, uid, "revoked");
        Assert.False(CertuvoLink.Eligible(db, uid));
    }

    // ---- DetectMemberType: the coarse category label and its precedence -------------------------

    [Fact]
    public void DetectMemberType_TestUser_IsTest_OverridesEverything()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        db.Execute("UPDATE users SET is_test=1 WHERE id=?", uid);
        // Even with an honorary award and a paid membership payment present, the test flag wins outright.
        SeedHonoraryAward(db, uid);
        SeedMembershipPayment(db, uid, "stripe", "paid", 120);
        Assert.Equal("test", CertuvoLink.DetectMemberType(db, uid));
    }

    [Fact]
    public void DetectMemberType_HonoraryAward_IsHonorary_OverridesPayment()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedHonoraryAward(db, uid);
        // An honorary award outranks the payment-derived label (a normal paid membership here).
        SeedMembershipPayment(db, uid, "stripe", "paid", 120);
        Assert.Equal("honorary", CertuvoLink.DetectMemberType(db, uid));
    }

    [Fact]
    public void DetectMemberType_NoMembershipPayment_IsMember()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // Not a test user, no honorary award, and no membership/bundle/renewal payment → generic "member".
        Assert.Equal("member", CertuvoLink.DetectMemberType(db, uid));
    }

    [Theory]
    [InlineData("stripe", "waived", 120.0)]        // payment_status='waived'
    [InlineData("admin_waiver", "paid", 50.0)]     // provider='admin_waiver' (status/amount notwithstanding)
    public void DetectMemberType_WaivedSettlement_IsWaived(string provider, string status, double amount)
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembershipPayment(db, uid, provider, status, amount);
        Assert.Equal("waived", CertuvoLink.DetectMemberType(db, uid));
    }

    [Fact]
    public void DetectMemberType_PartnerSponsoredCode_IsSponsored()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A discount code carrying a partner_id, redeemed by this user, on a payment that used that code.
        var dcId = db.ExecuteReturningId("INSERT INTO discount_codes(code,partner_id) VALUES(?,?)", "SPON-1", 7);
        SeedMembershipPayment(db, uid, "stripe", "paid", 120, "SPON-1");
        db.Execute("INSERT INTO code_redemptions(code_id,user_id,code) VALUES(?,?,?)", dcId, uid, "SPON-1");
        // A positive amount would otherwise read as "paid" — the partner sponsorship takes precedence.
        Assert.Equal("sponsored", CertuvoLink.DetectMemberType(db, uid));
    }

    [Fact]
    public void DetectMemberType_ZeroAmountNoWaiverNoSponsor_IsComplimentary()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // Not waived, not sponsored, final_amount<=0 → complimentary.
        SeedMembershipPayment(db, uid, "stripe", "paid", 0);
        Assert.Equal("complimentary", CertuvoLink.DetectMemberType(db, uid));
    }

    [Fact]
    public void DetectMemberType_PositivePaidSettlement_IsPaid()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembershipPayment(db, uid, "stripe", "paid", 120);
        Assert.Equal("paid", CertuvoLink.DetectMemberType(db, uid));
    }

    // ---- NextUsername: PCI-owned username generation --------------------------------------------

    [Fact]
    public void NextUsername_Default_IsPrefixYearSequence()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // Default prefix "PCI", current year, monotonic counter starting from 1, zero-padded to 6 digits.
        Assert.Equal($"PCI-{Year()}-000001", CertuvoLink.NextUsername(db, uid));
    }

    [Fact]
    public void NextUsername_SequenceIncrementsAcrossCalls()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var first = CertuvoLink.NextUsername(db, uid);
        var second = CertuvoLink.NextUsername(db, uid);
        // The counter (certuvo_username_seq) advances on every mint, so consecutive values never repeat.
        Assert.Equal($"PCI-{Year()}-000001", first);
        Assert.Equal($"PCI-{Year()}-000002", second);
    }

    [Fact]
    public void NextUsername_Collision_BumpsToNextFreeSuffix()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // Another account already holds the value the counter would first produce; NextUsername must skip it.
        var other = SeedUser(db, "taken@test.io");
        db.Execute("INSERT INTO certuvo_accounts(user_id,username,status) VALUES(?,?, 'active')", other, $"PCI-{Year()}-000001");
        Assert.Equal($"PCI-{Year()}-000002", CertuvoLink.NextUsername(db, uid));
    }

    [Theory]
    [InlineData(null, "PCI")]      // unset → default prefix
    [InlineData("ACME", "ACME")]   // configured prefix honoured
    [InlineData("", "PCI")]        // blank → fallback to "PCI"
    [InlineData("   ", "PCI")]     // whitespace trims to empty → fallback to "PCI"
    public void NextUsername_PrefixHonouredOrFallsBack(string? prefix, string expectedPrefix)
    {
        var db = Fresh();
        var uid = SeedUser(db);
        if (prefix is not null) Settings.Put(db, "certuvo_username_prefix", prefix);
        Assert.Equal($"{expectedPrefix}-{Year()}-000001", CertuvoLink.NextUsername(db, uid));
    }
}
