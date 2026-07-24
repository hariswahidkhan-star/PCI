using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Partner finance Phase 2 — reversals.
///
/// When money goes back to the student the commission must follow it, without ever editing history.
/// Pins: a full refund reverses the whole commission and cancels it out to zero; a partial refund
/// reverses only that proportion; a retried webhook reverses nothing further; a *larger* later refund
/// reverses only the incremental difference; a chargeback behaves as a full refund; and a reversal after
/// payout leaves the original 'paid' while creating a recoverable balance rather than hiding it.
/// </summary>
public class PartnerCommissionReversalTests
{
    static Db Fresh()
    {
        var db = TestEnv.NewMigratedDb();
        FinanceSchema.Ensure(db);
        return db;
    }

    /// <summary>A partner with a 10% agreement and one settled 1000.00 sale (no discount), commission 100.00.</summary>
    static (long partnerId, long payId, long txnId) Sale(Db db, double listPrice = 1000, double finalAmount = 1000, int holdDays = 30)
    {
        var pid = db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES('Rev Partner','registered','active',10,'marketing')");
        var aid = db.ExecuteReturningId(@"INSERT INTO partner_agreements(partner_id,agreement_number,effective_from,status,refund_hold_days)
            VALUES(?,?, '2026-01-01','active',?)", pid, "AGR-REV-" + pid, holdDays);
        db.Execute(@"INSERT INTO partner_commission_rules(agreement_id,partner_id,commission_type,commission_rate_bp,commission_basis,priority,active)
            VALUES(?,?, 'percentage',1000,'net_after_discount',100,1)", aid, pid);
        var uid = db.ExecuteReturningId(
            "INSERT INTO users(email,first_name,last_name,role,status) VALUES(?, 'Rev','Student','student','active')", $"rev{pid}@pci.test");
        var codeId = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id)
            VALUES(?, 'percentage',0,'exam',1,'active','campaign',?)", "REV-" + pid, pid);
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,amount_refunded)
            VALUES(?, 'exam',?,?, 'USD','test','paid','2026-03-01 10:00:00',0)", uid, listPrice, finalAmount);
        db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount,redeemed_at)
            VALUES(?,?,?,?, 'exam',?,?, '2026-03-01 10:00:00')", codeId, "REV-" + pid, uid, payId, listPrice, listPrice - finalAmount);
        var txnId = PartnerCommission.EnsureForPayment(db, payId);
        return (pid, payId, txnId);
    }

    static long Commission(Db db, long partnerId)
        => PartnerCommission.Totals(db, partnerId)["commission"];

    [Fact]
    public void A_full_refund_reverses_the_whole_commission_and_nets_to_zero()
    {
        var db = Fresh();
        var (pid, payId, txnId) = Sale(db);
        Assert.Equal(10_000L, Commission(db, pid));           // 10% of 1000.00

        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);
        var revId = PartnerCommissionReversal.EnsureForPayment(db, payId);

        Assert.True(revId > 0);
        Assert.Equal(0L, Commission(db, pid));                // earned and clawed back cancel exactly
        var rev = db.QueryOne("SELECT * FROM partner_commission_transactions WHERE id=?", revId)!;
        Assert.Equal(-10_000L, H.L(rev["commission_minor"]));
        Assert.Equal(txnId, H.L(rev["reversal_of_transaction_id"]));
        Assert.Equal(PartnerCommission.StatusReversed, H.Str(rev["status"]));
        // The original is untouched in value and now closed out.
        var src = db.QueryOne("SELECT * FROM partner_commission_transactions WHERE id=?", txnId)!;
        Assert.Equal(10_000L, H.L(src["commission_minor"]));
        Assert.Equal(PartnerCommission.StatusReversed, H.Str(src["status"]));
    }

    [Fact]
    public void A_repeated_refund_webhook_reverses_nothing_further()
    {
        var db = Fresh();
        var (pid, payId, _) = Sale(db);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);

        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, payId) > 0);
        Assert.Equal(0L, PartnerCommissionReversal.EnsureForPayment(db, payId));   // retry
        Assert.Equal(0L, PartnerCommissionReversal.EnsureForPayment(db, payId));   // and again
        Assert.Equal(1L, db.Scalar<long>(
            "SELECT COUNT(*) FROM partner_commission_transactions WHERE reversal_of_transaction_id IS NOT NULL AND reversal_of_transaction_id>0 AND partner_id=?", pid));
        Assert.Equal(0L, Commission(db, pid));
    }

    [Fact]
    public void A_partial_refund_reverses_only_that_proportion()
    {
        var db = Fresh();
        var (pid, payId, _) = Sale(db);
        // 250.00 of the 1000.00 sale returned → a quarter of the 100.00 commission goes back.
        db.Execute("UPDATE payments SET payment_status='partially_refunded', amount_refunded=250 WHERE id=?", payId);

        var revId = PartnerCommissionReversal.EnsureForPayment(db, payId);
        Assert.True(revId > 0);
        var rev = db.QueryOne("SELECT * FROM partner_commission_transactions WHERE id=?", revId)!;
        Assert.Equal(-2_500L, H.L(rev["commission_minor"]));
        Assert.Equal(7_500L, Commission(db, pid));            // 75.00 still stands
        // A partial refund must NOT close the original — the rest of the sale is still earned.
        Assert.NotEqual(PartnerCommission.StatusReversed,
            H.Str(db.QueryOne("SELECT status FROM partner_commission_transactions WHERE payment_id=? AND (reversal_of_transaction_id IS NULL OR reversal_of_transaction_id=0)", payId)!["status"]));
    }

    [Fact]
    public void A_larger_later_refund_reverses_only_the_incremental_difference()
    {
        var db = Fresh();
        var (pid, payId, _) = Sale(db);
        db.Execute("UPDATE payments SET payment_status='partially_refunded', amount_refunded=250 WHERE id=?", payId);
        PartnerCommissionReversal.EnsureForPayment(db, payId);
        Assert.Equal(7_500L, Commission(db, pid));

        // The refund is later extended to 600.00 in total.
        db.Execute("UPDATE payments SET amount_refunded=600 WHERE id=?", payId);
        var secondId = PartnerCommissionReversal.EnsureForPayment(db, payId);

        Assert.True(secondId > 0);
        Assert.Equal(-3_500L, H.L(db.QueryOne("SELECT commission_minor FROM partner_commission_transactions WHERE id=?", secondId)!["commission_minor"]));
        Assert.Equal(4_000L, Commission(db, pid));            // 40.00 of commission survives 60% refunded
        Assert.Equal(2L, db.Scalar<long>(
            "SELECT COUNT(*) FROM partner_commission_transactions WHERE reversal_of_transaction_id>0 AND partner_id=?", pid));
    }

    [Fact]
    public void A_chargeback_reverses_in_full()
    {
        var db = Fresh();
        var (pid, payId, _) = Sale(db);
        db.Execute("UPDATE payments SET payment_status='reversed' WHERE id=?", payId);   // dispute funds withdrawn

        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, payId, "chargeback") > 0);
        Assert.Equal(0L, Commission(db, pid));
    }

    [Fact]
    public void A_reversal_after_payout_leaves_the_original_paid_and_records_a_recoverable()
    {
        var db = Fresh();
        var (pid, payId, txnId) = Sale(db, holdDays: 0);
        // The commission was approved and settled before the student asked for their money back.
        PartnerCommission.Transition(db, txnId, PartnerCommission.StatusApproved, "admin", 1);
        PartnerCommission.Transition(db, txnId, PartnerCommission.StatusPaid, "admin", 1);
        Assert.Equal(10_000L, PartnerCommission.Totals(db, pid)["paid"]);

        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);
        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, payId) > 0);

        // The payout genuinely happened, so the original stays 'paid' and the claw-back is visible as a
        // recoverable balance rather than being silently netted away.
        Assert.Equal(PartnerCommission.StatusPaid,
            H.Str(db.QueryOne("SELECT status FROM partner_commission_transactions WHERE id=?", txnId)!["status"]));
        Assert.Equal(10_000L, PartnerCommission.RecoverableMinor(db, pid));
        Assert.Equal(0L, Commission(db, pid));
    }

    [Fact]
    public void A_payment_that_never_earned_commission_reverses_nothing()
    {
        var db = Fresh();
        var uid = db.ExecuteReturningId(
            "INSERT INTO users(email,first_name,last_name,role,status) VALUES('plain@pci.test','Plain','Student','student','active')");
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,final_amount,currency,payment_provider,payment_status,payment_date,amount_refunded)
            VALUES(?, 'exam',1000,'USD','test','refunded','2026-03-01 10:00:00',1000)", uid);
        Assert.Equal(0L, PartnerCommissionReversal.EnsureForPayment(db, payId));
    }

    [Fact]
    public void A_paid_payment_with_nothing_refunded_reverses_nothing()
    {
        var db = Fresh();
        var (_, payId, _) = Sale(db);
        Assert.Equal(0L, PartnerCommissionReversal.EnsureForPayment(db, payId));
    }
}
