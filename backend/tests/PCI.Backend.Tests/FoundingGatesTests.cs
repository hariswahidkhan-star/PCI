using System.Reflection;
using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-7 — direct unit coverage of the Founding-Stage access decision core (<see cref="Founding"/>):
/// the code-window evaluator (<c>Usable</c>), the application-criteria gate (<c>CriteriaMet</c>), and the
/// once-only settlement (<c>Founding.Grant</c>). The live-HTTP suite drives the endpoints; here the
/// decisions are pinned directly against a fresh migrated temp-SQLite Db (TestEnv.NewMigratedDb — the same
/// schema CI migrates), seeding only the rows a decision reads. Assertions read database state so they stay
/// robust. No application code is changed; this pins behaviour that is already shipping.
///
/// <para><c>Usable</c> and <c>CriteriaMet</c> are private static pure helpers (they take a code dictionary
/// and return a verdict, touching no database). They are invoked through small reflection shims below so
/// the window/criteria boundaries can be pinned as the units they are, without altering their visibility.
/// <c>Grant</c> is public and is called directly.</para>
/// </summary>
public class FoundingGatesTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    // FK enforcement is ON, so every row that references users(id) needs a real user first.
    static long SeedUser(Db db, string email = "found@test.io")
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)",
                   email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    // Grant's audit callback is irrelevant to the decisions under test.
    static readonly System.Action<long?, string, string?> NoLog = (_, _, _) => { };

    // ---- reflection shims onto the private pure helpers -------------------------------------------
    static readonly MethodInfo UsableM =
        typeof(Founding).GetMethod("Usable", BindingFlags.NonPublic | BindingFlags.Static)!;
    static readonly MethodInfo CriteriaMetM =
        typeof(Founding).GetMethod("CriteriaMet", BindingFlags.NonPublic | BindingFlags.Static)!;

    static (bool ok, string reason) Usable(Dictionary<string, object?> c)
        => ((bool ok, string reason))UsableM.Invoke(null, new object?[] { c })!;

    static bool CriteriaMet(Dictionary<string, object?> code, long expYears, string role, string qualification)
        => (bool)CriteriaMetM.Invoke(null, new object?[] { code, expYears, role, qualification })!;

    // A founding-code row shape as Usable reads it (active / window / cap). Pure — no DB needed.
    static Dictionary<string, object?> UCode(long active = 1, string? start = null, string? end = null,
                                             object? maxUses = null, long used = 0)
        => new()
        {
            ["active"] = active,
            ["start_date"] = start,
            ["end_date"] = end,
            ["max_uses"] = maxUses,
            ["used_count"] = used,
        };

    // A code row carrying only the board's criteria_json, as CriteriaMet reads it.
    static Dictionary<string, object?> CCode(string? criteria)
        => new() { ["criteria_json"] = criteria };

    // Insert a real founding code and return the row exactly as the endpoint's Code() lookup would,
    // so Grant sees genuine column types and the cap-take UPDATE has a row to hit.
    static Dictionary<string, object?> SeedFoundingCode(Db db, string code, long grantsMembership = 1,
        long grantsExam = 1, object? maxUses = null, long usedCount = 0, object? membershipMonths = null)
    {
        db.Execute(@"INSERT INTO discount_codes(code,founding_route,grants_membership,grants_exam,grants_study_access,
                     requires_application,auto_approve,membership_months,max_uses,used_count,active)
                     VALUES(?,'founding',?,?,1,0,1,?,?,?,1)",
                   code, grantsMembership, grantsExam, membershipMonths ?? 12L, maxUses, usedCount);
        return db.QueryOne("SELECT * FROM discount_codes WHERE code=?", code)!;
    }

    // ================================================================================================
    //  Usable — code window (start/end day-inclusivity), active flag, usage cap
    // ================================================================================================

    [Fact]
    public void Usable_InactiveCode_ReturnsInactive()
    {
        var (ok, reason) = Usable(UCode(active: 0));
        Assert.False(ok);
        Assert.Equal("inactive", reason);
    }

    [Theory]
    [InlineData(0, true)]    // ends TODAY — a date-only end_date is inclusive of its whole day (through 23:59:59)
    [InlineData(1, true)]    // ends tomorrow — plainly still open
    [InlineData(-1, false)]  // ended yesterday — the day AFTER, the window is closed
    public void Usable_EndDate_IsDayInclusive(int dayOffset, bool expectedUsable)
    {
        var end = System.DateTime.UtcNow.AddDays(dayOffset).ToString("yyyy-MM-dd");
        var (ok, reason) = Usable(UCode(end: end));
        Assert.Equal(expectedUsable, ok);
        if (!expectedUsable) Assert.Equal("window_closed", reason);
    }

    [Theory]
    [InlineData(1, false, "not_open")]  // opens tomorrow — not yet in window
    [InlineData(-1, true, "ok")]        // opened yesterday — open
    public void Usable_StartDate_FutureIsNotOpen(int dayOffset, bool expectedUsable, string expectedReason)
    {
        var start = System.DateTime.UtcNow.AddDays(dayOffset).ToString("yyyy-MM-dd HH:mm:ss");
        var (ok, reason) = Usable(UCode(start: start));
        Assert.Equal(expectedUsable, ok);
        Assert.Equal(expectedReason, reason);
    }

    [Fact]
    public void Usable_StartExactlyNow_IsOpen()
    {
        // Boundary: start_date == now. `After` is strict (>), so a window that opens at this instant is OPEN
        // (and wall-clock only moves `now` further past it before Usable reads it).
        var start = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        var (ok, reason) = Usable(UCode(start: start));
        Assert.True(ok);
        Assert.Equal("ok", reason);
    }

    [Fact]
    public void Usable_UsageCap_IsInclusiveBoundary()
    {
        Assert.True(Usable(UCode(maxUses: 2L, used: 1)).ok);                          // 1 < 2 → headroom
        Assert.Equal("fully_redeemed", Usable(UCode(maxUses: 2L, used: 2)).reason);   // 2 >= 2 → sold out
    }

    // ================================================================================================
    //  CriteriaMet — application thresholds; malformed board JSON must fail CLOSED
    // ================================================================================================

    [Theory]
    [InlineData(null)]   // no criteria set at all
    [InlineData("")]     // empty
    [InlineData("   ")]  // whitespace
    public void CriteriaMet_NoThresholds_AlwaysMet(string? criteria)
    {
        // The board set nothing to enforce → every applicant satisfies the (absent) criteria.
        Assert.True(CriteriaMet(CCode(criteria), 0, "", ""));
    }

    [Theory]
    [InlineData(3, true)]   // exactly the threshold is met (the guard is `< min`)
    [InlineData(5, true)]
    [InlineData(2, false)]  // one short → not met
    [InlineData(0, false)]
    public void CriteriaMet_MinExperienceYears_Boundary(long years, bool expected)
    {
        var code = CCode("{\"min_experience_years\":3}");
        Assert.Equal(expected, CriteriaMet(code, years, "planner", "BSc"));
    }

    [Theory]
    [InlineData("planner", true)]         // exact match against a listed role
    [InlineData("Senior Planner", true)]  // declared role CONTAINS a listed role (substring semantics)
    [InlineData("cost engineer", true)]   // contains "engineer"
    [InlineData("architect", false)]      // listed roles present but none match → not met
    public void CriteriaMet_AllowedRoles_SubstringAndListSemantics(string role, bool expected)
    {
        var code = CCode("{\"allowed_roles\":[\"planner\",\"engineer\"]}");
        Assert.Equal(expected, CriteriaMet(code, 0, role, ""));
    }

    [Theory]
    [InlineData("", false)]         // qualification required but none supplied
    [InlineData("   ", false)]      // whitespace is still "no qualification"
    [InlineData("BSc Civil", true)] // a real qualification satisfies it
    public void CriteriaMet_RequireQualification(string qualification, bool expected)
    {
        var code = CCode("{\"require_qualification\":true}");
        Assert.Equal(expected, CriteriaMet(code, 10, "planner", qualification));
    }

    [Fact]
    public void CriteriaMet_MalformedJson_FailsClosed()
    {
        // A broken board JSON must NEVER auto-grant on a parse error — it fails CLOSED (not met).
        Assert.False(CriteriaMet(CCode("{ this is not valid json"), 99, "planner", "PhD"));
    }

    // ================================================================================================
    //  Grant — once-only settlement, stacking guard, cap-undo
    // ================================================================================================

    [Fact]
    public void Grant_MembershipCode_SettlesFoundingMembershipAtZero()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var code = SeedFoundingCode(db, "FOUNDMEM", grantsMembership: 1, grantsExam: 0);

        var r = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.True(r.granted);
        Assert.Equal("granted", r.reason);
        Assert.StartsWith("FOUND-", r.reference);

        var pay = db.QueryOne("SELECT * FROM payments WHERE user_id=?", uid)!;
        Assert.Equal("founding_waiver", H.Str(pay["payment_provider"]));
        Assert.Equal("paid", H.Str(pay["payment_status"]));
        Assert.Equal(0, H.D(pay["final_amount"]), 3);
        Assert.Equal("membership", H.Str(pay["product_type"]));

        var m = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", uid)!;
        Assert.Equal("active", H.Str(m["status"]));
        Assert.Equal("Founding Membership", H.Str(m["membership_type"]));

        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM code_redemptions WHERE user_id=? AND code='FOUNDMEM'", uid));
        // A membership-only code grants no exam entitlement.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM exam_entitlements WHERE user_id=?", uid));
    }

    [Fact]
    public void Grant_ExamCode_CreatesAvailableEntitlementWithDeadline()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var code = SeedFoundingCode(db, "FOUNDEXAM", grantsMembership: 0, grantsExam: 1);

        var r = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.True(r.granted);

        var pay = db.QueryOne("SELECT * FROM payments WHERE user_id=?", uid)!;
        Assert.Equal("exam", H.Str(pay["product_type"]));
        Assert.NotNull(pay["exam_schedule_deadline"]);   // schedule-by date stamped when an entitlement is granted

        var e = db.QueryOne("SELECT * FROM exam_entitlements WHERE user_id=?", uid)!;
        Assert.Equal("available", H.Str(e["status"]));
        Assert.Equal(1L, H.L(e["certification_id"]));    // Certs.DefaultId
        Assert.Equal(H.L(pay["id"]), H.L(e["payment_id"]));

        // Exam-only code grants no membership.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM memberships WHERE user_id=?", uid));
    }

    [Fact]
    public void Grant_SecondRedemption_IsIdempotentPerCodeAndUser()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var code = SeedFoundingCode(db, "FOUNDONCE", grantsMembership: 1, grantsExam: 1);

        var first = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.True(first.granted);

        var second = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.False(second.granted);
        Assert.Equal("already_redeemed", second.reason);
        Assert.Null(second.reference);

        // Exactly one settlement, one cap take, one entitlement — the re-run changed nothing.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM payments WHERE user_id=? AND payment_provider='founding_waiver'", uid));
        Assert.Equal(1L, db.Scalar<long>("SELECT used_count FROM discount_codes WHERE code='FOUNDONCE'"));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM exam_entitlements WHERE user_id=?", uid));
    }

    [Fact]
    public void Grant_StackingGuard_OpenEntitlementBlocksSecondExam()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // The user already holds a LIVE (available) exam entitlement from a real paid exam.
        var priorPay = db.ExecuteReturningId(
            "INSERT INTO payments(user_id,product_type,payment_provider,payment_status) VALUES(?,'exam','stripe','paid')", uid);
        db.Execute(@"INSERT INTO exam_entitlements(user_id,payment_id,product_type,certification_id,status,valid_until)
                     VALUES(?,?,'exam',1,'available',datetime('now','+1 year'))", uid, priorPay);

        var code = SeedFoundingCode(db, "FOUNDDUP", grantsMembership: 0, grantsExam: 1);
        var r = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.True(r.granted);   // the grant still settles (a USD 0 waiver payment)...

        // ...but it must NOT stack a second live entitlement (one fee + a waiver != two sittings).
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM exam_entitlements WHERE user_id=?", uid));
        var foundingPay = db.QueryOne("SELECT * FROM payments WHERE user_id=? AND payment_provider='founding_waiver'", uid)!;
        Assert.Null(foundingPay["exam_schedule_deadline"]);   // no entitlement granted on this payment → no deadline
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM exam_entitlements WHERE payment_id=?", H.L(foundingPay["id"])));
    }

    [Fact]
    public void Grant_StackingGuard_ConsumedEntitlementAllowsRetake()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // The prior entitlement was already CONSUMED by a sitting — a board-granted founding retake is allowed.
        var priorPay = db.ExecuteReturningId(
            "INSERT INTO payments(user_id,product_type,payment_provider,payment_status) VALUES(?,'exam','stripe','paid')", uid);
        db.Execute(@"INSERT INTO exam_entitlements(user_id,payment_id,product_type,certification_id,status,valid_until)
                     VALUES(?,?,'exam',1,'consumed',datetime('now','+1 year'))", uid, priorPay);

        var code = SeedFoundingCode(db, "FOUNDRETAKE", grantsMembership: 0, grantsExam: 1);
        var r = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.True(r.granted);

        // A fresh available entitlement IS created on the founding payment (2 total now: consumed + new).
        Assert.Equal(2L, db.Scalar<long>("SELECT COUNT(*) FROM exam_entitlements WHERE user_id=?", uid));
        var foundingPay = db.QueryOne("SELECT * FROM payments WHERE user_id=? AND payment_provider='founding_waiver'", uid)!;
        var fresh = db.QueryOne("SELECT * FROM exam_entitlements WHERE payment_id=?", H.L(foundingPay["id"]))!;
        Assert.Equal("available", H.Str(fresh["status"]));
        Assert.NotNull(foundingPay["exam_schedule_deadline"]);
    }

    [Fact]
    public void Grant_FullyRedeemed_UndoesLedgerRowAndAllowsRetryAfterCapRaise()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var code = SeedFoundingCode(db, "FOUNDCAP", grantsMembership: 1, grantsExam: 0, maxUses: 1L, usedCount: 1);
        var codeId = H.L(code["id"]);
        var eventId = $"founding:{codeId}:u{uid}";

        // The cap take fails (used_count already == max_uses). The grant must UNDO its idempotency-ledger
        // row so the user is not permanently locked out, and it must settle nothing.
        var r = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.False(r.granted);
        Assert.Equal("fully_redeemed", r.reason);
        Assert.Null(r.reference);
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM payments WHERE user_id=?", uid));            // nothing settled
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM webhook_events WHERE event_id=?", eventId)); // ledger restored (undone)

        // The board lifts the cap; the SAME (code, user) can now redeem — proof the undo left no stale lock.
        db.Execute("UPDATE discount_codes SET max_uses=2 WHERE id=?", codeId);
        var code2 = db.QueryOne("SELECT * FROM discount_codes WHERE id=?", codeId)!;
        var r2 = Founding.Grant(db, NoLog, uid, code2, "redeem");
        Assert.True(r2.granted);
        Assert.Equal("granted", r2.reason);
        Assert.Equal(2L, db.Scalar<long>("SELECT used_count FROM discount_codes WHERE id=?", codeId));  // cap take now applied
    }

    [Fact]
    public void Grant_ExistingLivePaidMembership_KeepsItsOwnType()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A genuinely live, paid membership must never be clawed into a 'Founding Membership'.
        db.Execute("INSERT INTO memberships(user_id,membership_type,status,expiry_date) VALUES(?, 'Student Membership','active',datetime('now','+2 year'))", uid);
        var code = SeedFoundingCode(db, "FOUNDLIVE", grantsMembership: 1, grantsExam: 0);

        var r = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.True(r.granted);

        var m = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", uid)!;
        Assert.Equal("Student Membership", H.Str(m["membership_type"]));   // type preserved
        Assert.Equal("active", H.Str(m["status"]));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM memberships WHERE user_id=?", uid));  // extended in place, not duplicated
    }

    [Fact]
    public void Grant_ExistingExpiredMembership_RestampedFounding()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A lapsed membership: the founding grant is what revives it, so it is stamped 'Founding Membership'
        // (a later revoke can then find and lapse exactly what the waiver granted).
        db.Execute("INSERT INTO memberships(user_id,membership_type,status,expiry_date) VALUES(?, 'Student Membership','expired',datetime('now','-1 day'))", uid);
        var code = SeedFoundingCode(db, "FOUNDREVIVE", grantsMembership: 1, grantsExam: 0);

        var r = Founding.Grant(db, NoLog, uid, code, "redeem");
        Assert.True(r.granted);

        var m = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", uid)!;
        Assert.Equal("Founding Membership", H.Str(m["membership_type"]));   // restamped
        Assert.Equal("active", H.Str(m["status"]));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM memberships WHERE user_id=?", uid));
    }
}
