using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-3 — the offline-settlement engine (<see cref="Settlement"/>): the code that grants a member exactly
/// what a real Stripe payment would, drives the idempotent downstream effects, and reverses an
/// admin-recorded settlement. The live-HTTP suite exercises the endpoints; here the decisions are pinned
/// directly against the migrated temp-SQLite Db. Assertions read database state (not the anonymous return
/// objects) so they stay robust. No application code is changed.
/// </summary>
public class SettlementTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static long SeedUser(Db db, string email = "settle@test.io")
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)", email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    static Dictionary<string, object?> Payment(Db db, long payId) =>
        db.QueryOne("SELECT * FROM payments WHERE id=?", payId)!;

    // ---- Grant: waived vs paid classification -----------------------------------------------------

    [Fact]
    public void Grant_FeeWaiver_IsRecordedAsWaivedNotPaid()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 0, "WAIVE-1", "admin_waiver");
        var p = Payment(db, pid);
        Assert.Equal("waived", H.Str(p["payment_status"]));
        Assert.Equal(0, H.D(p["final_amount"]), 3);
        // A waiver records what it forgave (the list price) as waived_amount, never disguised as revenue.
        Assert.True(H.D(p["waived_amount"]) > 0);
    }

    [Fact]
    public void Grant_HonoraryWaiver_IsAlsoWaived()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 0, "HON-1", "admin_honorary");
        Assert.Equal("waived", H.Str(Payment(db, pid)["payment_status"]));
    }

    [Fact]
    public void Grant_PaidSettlement_IsRecordedAsPaid()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 120, "PAID-1", "admin_manual");
        var p = Payment(db, pid);
        Assert.Equal("paid", H.Str(p["payment_status"]));
        Assert.Equal(120, H.D(p["final_amount"]), 3);
    }

    [Fact]
    public void Grant_NonZeroAmountWithWaiverProvider_IsStillPaid()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // waived requires amount<=0; a positive amount is a paid settlement even under a waiver provider.
        var pid = Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 50, "PART-1", "admin_waiver");
        Assert.Equal("paid", H.Str(Payment(db, pid)["payment_status"]));
    }

    [Fact]
    public void Grant_PartialPayment_RecordsForgivenAmount()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A paid-but-discounted settlement records the difference (original - amount) as waived_amount.
        var meta = new Settlement.Meta { OriginalAmount = 100 };
        var pid = Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 40, "PART-2", "admin_manual", meta);
        var p = Payment(db, pid);
        Assert.Equal("paid", H.Str(p["payment_status"]));
        Assert.Equal(60, H.D(p["waived_amount"]), 3);   // 100 - 40 forgiven
    }

    // ---- EnsureDownstream: membership + entitlement effects ----------------------------------------

    [Fact]
    public void Grant_Membership_CreatesActiveMembership()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 99, "M-1", "admin_manual");
        var m = db.QueryOne("SELECT status FROM memberships WHERE user_id=?", uid);
        Assert.NotNull(m);
        Assert.Equal("active", H.Str(m!["status"]));
    }

    [Fact]
    public void EnsureDownstream_ActivatesAnInactiveMembership()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        db.Execute("INSERT INTO memberships(user_id,membership_type,status) VALUES(?, 'Student Membership','inactive')", uid);
        var pid = Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 99, "M-2", "admin_manual");
        // Grant already ran EnsureDownstream; the pre-existing inactive row is now active.
        Assert.Equal("active", H.Str(db.QueryOne("SELECT status FROM memberships WHERE user_id=?", uid)!["status"]));
        // A second EnsureDownstream is a clean no-op (idempotent) — still exactly one membership row.
        Settlement.EnsureDownstream(db, pid);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM memberships WHERE user_id=?", uid));
    }

    [Fact]
    public void EnsureDownstream_ExamEntitlement_CreatedOnceAndIdempotent()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, "settle@test.io", "exam", 1, amount: 500, "E-1", "admin_manual");
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM exam_entitlements WHERE payment_id=?", pid));
        // Re-running never double-grants.
        Settlement.EnsureDownstream(db, pid);
        Settlement.EnsureDownstream(db, pid);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM exam_entitlements WHERE payment_id=?", pid));
    }

    [Fact]
    public void Grant_Exam_SetsScheduleDeadline()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, "settle@test.io", "exam", 1, amount: 500, "E-2", "admin_manual");
        Assert.NotNull(Payment(db, pid)["exam_schedule_deadline"]);
    }

    // ---- Reverse ----------------------------------------------------------------------------------

    [Fact]
    public void Reverse_StripePayment_IsRefused()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        db.Execute("INSERT INTO payments(user_id,product_type,final_amount,payment_provider,payment_status) VALUES(?, 'membership',99,'stripe','paid')", uid);
        var pid = db.Scalar<long>("SELECT id FROM payments WHERE user_id=?", uid);
        Settlement.Reverse(db, pid, "test", 1);
        // Stripe rows are refunded at the gateway, not here — the row is untouched.
        Assert.Equal("paid", H.Str(Payment(db, pid)["payment_status"]));
    }

    [Fact]
    public void Reverse_UnsettledPayment_IsRefused()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        db.Execute("INSERT INTO payments(user_id,product_type,final_amount,payment_provider,payment_status) VALUES(?, 'membership',99,'admin_manual','failed')", uid);
        var pid = db.Scalar<long>("SELECT id FROM payments WHERE user_id=?", uid);
        Settlement.Reverse(db, pid, "test", 1);
        Assert.Equal("failed", H.Str(Payment(db, pid)["payment_status"]));
    }

    [Fact]
    public void Reverse_Membership_LapsesWhenNoOtherLiveSettlement()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 99, "M-3", "admin_manual");
        Settlement.Reverse(db, pid, "duplicate charge", 1);
        Assert.Equal("refunded", H.Str(Payment(db, pid)["payment_status"]));
        Assert.Equal("expired", H.Str(db.QueryOne("SELECT status FROM memberships WHERE user_id=?", uid)!["status"]));
    }

    [Fact]
    public void Reverse_Membership_KeepsActiveWhenAnotherLiveSettlementExists()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var first = Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 99, "M-4a", "admin_manual");
        // A second live membership settlement still supports access.
        Settlement.Grant(db, uid, "settle@test.io", "membership", 1, amount: 99, "M-4b", "admin_manual");
        Settlement.Reverse(db, first, "one of two reversed", 1);
        Assert.Equal("active", H.Str(db.QueryOne("SELECT status FROM memberships WHERE user_id=?", uid)!["status"]));
    }

    [Fact]
    public void Reverse_Exam_RevokesTheUnconsumedEntitlement()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, "settle@test.io", "exam", 1, amount: 500, "E-3", "admin_manual");
        Settlement.Reverse(db, pid, "refund", 1);
        Assert.Equal("revoked", H.Str(db.QueryOne("SELECT status FROM exam_entitlements WHERE payment_id=?", pid)!["status"]));
    }

    // ---- ExpireDueMemberships ---------------------------------------------------------------------

    [Fact]
    public void ExpireDueMemberships_FlipsPastDueActiveMemberships()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        db.Execute("INSERT INTO memberships(user_id,membership_type,status,expiry_date) VALUES(?, 'Student Membership','active',datetime('now','-1 day'))", uid);
        var flipped = Settlement.ExpireDueMemberships(db);
        Assert.Equal(1, flipped);
        Assert.Equal("expired", H.Str(db.QueryOne("SELECT status FROM memberships WHERE user_id=?", uid)!["status"]));
    }

    [Fact]
    public void ExpireDueMemberships_LeavesFutureExpiryUntouched()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        db.Execute("INSERT INTO memberships(user_id,membership_type,status,expiry_date) VALUES(?, 'Student Membership','active',datetime('now','+30 day'))", uid);
        Assert.Equal(0, Settlement.ExpireDueMemberships(db));
        Assert.Equal("active", H.Str(db.QueryOne("SELECT status FROM memberships WHERE user_id=?", uid)!["status"]));
    }

    [Fact]
    public void ExpireDueMemberships_IgnoresRowsWithNoExpiryDate()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        db.Execute("INSERT INTO memberships(user_id,membership_type,status,expiry_date) VALUES(?, 'Student Membership','active',NULL)", uid);
        Assert.Equal(0, Settlement.ExpireDueMemberships(db));
    }
}
