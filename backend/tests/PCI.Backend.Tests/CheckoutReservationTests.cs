using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// RES-001 / RES-002 — checkout reservation capacity holds and immutable partner attribution.
/// </summary>
public class CheckoutReservationTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static long SeedCode(Db db, string code, int maxUses, long? partnerId = null)
    {
        return db.ExecuteReturningId(
            @"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,max_uses,used_count,reserved_count,partner_id)
              VALUES(?, 'percentage', 10, 'all', 1, 'active', ?, 0, 0, ?)",
            code, maxUses, partnerId);
    }

    [Fact]
    public void Schema_exposes_reservation_and_payment_attribution_columns()
    {
        var db = Fresh();
        Assert.Contains(db.Columns("checkout_reservations"), c => c.Equals("idempotency_key", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(db.Columns("discount_codes"), c => c.Equals("reserved_count", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(db.Columns("payments"), c => c.Equals("partner_id", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(db.Columns("payments"), c => c.Equals("discount_code_id", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Same_idempotency_key_replays_one_reservation()
    {
        var db = Fresh();
        var codeId = SeedCode(db, "SAVE1", 5);
        var code = db.QueryOne("SELECT * FROM discount_codes WHERE id=?", codeId)!;
        CheckoutReservation.ReserveResult r1 = default!, r2 = default!;
        db.Transaction(() =>
        {
            r1 = CheckoutReservation.TryReserve(db, "idem-checkout-a", "a@test.io", "exam", 1, code, 14900);
            r2 = CheckoutReservation.TryReserve(db, "idem-checkout-a", "a@test.io", "exam", 1, code, 14900);
        });
        Assert.True(r1.Ok);
        Assert.False(r1.Replayed);
        Assert.True(r2.Ok);
        Assert.True(r2.Replayed);
        Assert.Equal(H.L(r1.Reservation!["id"]), H.L(r2.Reservation!["id"]));
        Assert.Equal(1, db.Scalar<long>("SELECT reserved_count FROM discount_codes WHERE id=?", codeId));
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM checkout_reservations WHERE idempotency_key=?", "idem-checkout-a"));
    }

    [Fact]
    public void Concurrent_capacity_take_allows_only_max_uses_holds()
    {
        var db = Fresh();
        var codeId = SeedCode(db, "ONCE", 1);
        var code = db.QueryOne("SELECT * FROM discount_codes WHERE id=?", codeId)!;
        CheckoutReservation.ReserveResult a = default!, b = default!;
        db.Transaction(() =>
        {
            a = CheckoutReservation.TryReserve(db, "hold-1", "one@test.io", "membership", 1, code, 9900);
            b = CheckoutReservation.TryReserve(db, "hold-2", "two@test.io", "membership", 1, code, 9900);
        });
        Assert.True(a.Ok);
        Assert.False(b.Ok);
        Assert.Equal("code_exhausted", b.Error);
        Assert.Equal(1, db.Scalar<long>("SELECT reserved_count FROM discount_codes WHERE id=?", codeId));
    }

    [Fact]
    public void Settle_converts_reserved_hold_to_used_count()
    {
        var db = Fresh();
        var codeId = SeedCode(db, "HOLD2USE", 3);
        var code = db.QueryOne("SELECT * FROM discount_codes WHERE id=?", codeId)!;
        CheckoutReservation.ReserveResult r = default!;
        db.Transaction(() => r = CheckoutReservation.TryReserve(db, "settle-key", "s@test.io", "exam", 1, code, 10000));
        Assert.True(r.Ok);
        var uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,role,status) VALUES('s@test.io','S','student','active')");
        var payId = db.ExecuteReturningId(
            @"INSERT INTO payments(user_id,product_type,final_amount,currency,payment_provider,payment_status,payment_date,reference)
              VALUES(?,'exam',100,'USD','test','paid',datetime('now'),'R1')", uid);
        bool settled = false;
        db.Transaction(() =>
        {
            CheckoutReservation.StampPaymentAttribution(db, payId, r.Reservation);
            settled = CheckoutReservation.TrySettle(db, r.Reservation, payId);
        });
        Assert.True(settled);
        Assert.Equal(0, db.Scalar<long>("SELECT reserved_count FROM discount_codes WHERE id=?", codeId));
        Assert.Equal(1, db.Scalar<long>("SELECT used_count FROM discount_codes WHERE id=?", codeId));
        Assert.Equal("settled", H.Str(db.QueryOne("SELECT status FROM checkout_reservations WHERE id=?", r.Reservation!["id"])!["status"]));
    }

    [Fact]
    public void Partner_id_is_snapshotted_at_reserve_and_survives_code_reassignment()
    {
        var db = Fresh();
        FinanceSchema.Ensure(db);
        var partnerA = db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES('A','registered','active',10,'marketing')");
        var partnerB = db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES('B','registered','active',20,'marketing')");
        var codeId = SeedCode(db, "PARTNERX", 10, partnerA);
        var code = db.QueryOne("SELECT * FROM discount_codes WHERE id=?", codeId)!;
        CheckoutReservation.ReserveResult r = default!;
        db.Transaction(() => r = CheckoutReservation.TryReserve(db, "attr-key", "p@test.io", "exam", 1, code, 14900));
        Assert.Equal(partnerA, H.L(r.Reservation!["partner_id"]));

        // Reassign the live code to partner B after reservation — snapshotted attribution must stay A.
        db.Execute("UPDATE discount_codes SET partner_id=? WHERE id=?", partnerB, codeId);

        var uid = db.ExecuteReturningId(
            "INSERT INTO users(email,first_name,role,status) VALUES('p@test.io','P','student','active')");
        var payId = db.ExecuteReturningId(
            @"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,reference)
              VALUES(?,'exam',149,134.1,'USD','test','paid',datetime('now'),'ATTR-1')", uid);
        db.Transaction(() =>
        {
            CheckoutReservation.StampPaymentAttribution(db, payId, r.Reservation);
            Assert.True(CheckoutReservation.TrySettle(db, r.Reservation, payId));
            db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount)
                VALUES(?,?,?,?,'exam',149,14.9)", codeId, "PARTNERX", uid, payId);
        });
        Assert.Equal(partnerA, H.L(db.QueryOne("SELECT partner_id FROM payments WHERE id=?", payId)!["partner_id"]));

        var txnId = PartnerCommission.EnsureForPayment(db, payId);
        Assert.True(txnId > 0);
        var txn = db.QueryOne("SELECT partner_id FROM partner_commission_transactions WHERE payment_id=?", payId)!;
        Assert.Equal(partnerA, H.L(txn["partner_id"]));
    }

    [Fact]
    public void Expire_stale_releases_capacity()
    {
        var db = Fresh();
        var codeId = SeedCode(db, "EXP1", 2);
        var code = db.QueryOne("SELECT * FROM discount_codes WHERE id=?", codeId)!;
        CheckoutReservation.ReserveResult r = default!;
        db.Transaction(() => r = CheckoutReservation.TryReserve(db, "exp-key", "e@test.io", "exam", 1, code, 5000, ttlMinutes: 5));
        db.Execute("UPDATE checkout_reservations SET expires_at=datetime('now','-1 minute') WHERE id=?", r.Reservation!["id"]);
        db.Transaction(() => CheckoutReservation.ExpireStale(db, codeId));
        Assert.Equal(0, db.Scalar<long>("SELECT reserved_count FROM discount_codes WHERE id=?", codeId));
        Assert.Equal("expired", H.Str(db.QueryOne("SELECT status FROM checkout_reservations WHERE id=?", r.Reservation!["id"])!["status"]));
    }
}
