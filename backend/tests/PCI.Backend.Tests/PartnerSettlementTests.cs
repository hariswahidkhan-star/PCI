using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Partner finance Phase 3 — settlements.
///
/// The old payout row let any single administrator write any amount against a partner, with no link to
/// what it was paying for. These tests pin the controls that replaced it, stated as the two properties
/// the module exists to guarantee: <b>overpayment is impossible</b> and <b>double payment is
/// impossible</b> — plus the separation of duties that stops one person doing both halves.
///
/// Every refusal is asserted by its reason code, not merely by "it failed", so a future change that
/// blocks the right thing for the wrong reason still shows up.
/// </summary>
[Collection(DbCollection.Name)]
public class PartnerSettlementTests
{
    const long Maker = 11, Checker = 22, Payer = 33;

    static Db Fresh()
    {
        var db = TestEnv.NewMigratedDb();
        FinanceSchema.Ensure(db);
        return db;
    }

    /// <summary>A partner on a 10% agreement. Returns the partner id.</summary>
    static long Partner(Db db, string name = "Settle Partner", int holdDays = 0)
    {
        var pid = db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES(?, 'registered','active',10,'marketing')", name);
        var aid = db.ExecuteReturningId(@"INSERT INTO partner_agreements(partner_id,agreement_number,effective_from,status,refund_hold_days)
            VALUES(?,?, '2026-01-01','active',?)", pid, "AGR-SET-" + pid, holdDays);
        db.Execute(@"INSERT INTO partner_commission_rules(agreement_id,partner_id,commission_type,commission_rate_bp,commission_basis,priority,active)
            VALUES(?,?, 'percentage',1000,'net_after_discount',100,1)", aid, pid);
        return pid;
    }

    /// <summary>One settled sale for the partner, its commission moved straight to approved_for_payment.</summary>
    static long ApprovedSale(Db db, long partnerId, double amount, int seq)
    {
        var uid = db.ExecuteReturningId(
            "INSERT INTO users(email,first_name,last_name,role,status) VALUES(?, 'Set','Student','student','active')",
            $"set{partnerId}.{seq}@pci.test");
        var codeId = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id)
            VALUES(?, 'percentage',0,'exam',1,'active','campaign',?)", $"SET-{partnerId}-{seq}", partnerId);
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,amount_refunded)
            VALUES(?, 'exam',?,?, 'USD','test','paid','2026-03-01 10:00:00',0)", uid, amount, amount);
        db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount,redeemed_at)
            VALUES(?,?,?,?, 'exam',?,0,'2026-03-01 10:00:00')", codeId, $"SET-{partnerId}-{seq}", uid, payId, amount);
        var txnId = PartnerCommission.EnsureForPayment(db, payId);
        // With a zero-day hold the commission lands 'due'; approving it is Finance's own step.
        PartnerCommission.Transition(db, txnId, PartnerCommission.StatusApproved, "admin", Checker);
        return txnId;
    }

    static string Status(Db db, long txnId)
        => H.Str(db.QueryOne("SELECT status FROM partner_commission_transactions WHERE id=?", txnId)!["status"])!;

    // ── separation of duties ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void The_preparer_cannot_approve_their_own_settlement()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);

        var prepared = PartnerSettlement.Prepare(db, pid, Maker, "2026-03-01", "2026-03-31", null, null);
        Assert.True(prepared.Ok);

        var self = PartnerSettlement.Approve(db, prepared.Id, Maker);
        Assert.False(self.Ok);
        Assert.Equal("maker_checker", self.Error);
        // Refused means unchanged — not approved-with-a-warning.
        Assert.Equal(PartnerSettlement.StatusPendingApproval,
            H.Str(db.QueryOne("SELECT status FROM partner_settlements WHERE id=?", prepared.Id)!["status"]));

        Assert.True(PartnerSettlement.Approve(db, prepared.Id, Checker).Ok);
    }

    [Fact]
    public void A_settlement_with_no_recorded_preparer_cannot_be_approved_at_all()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        var prepared = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);

        // Simulates a legacy or hand-inserted row. Unlike the platform's older maker-checker helpers this
        // one must FAIL CLOSED: an unknown maker cannot be shown to differ from the checker.
        db.Execute("UPDATE partner_settlements SET prepared_by=NULL WHERE id=?", prepared.Id);

        var r = PartnerSettlement.Approve(db, prepared.Id, Checker);
        Assert.False(r.Ok);
        Assert.Equal("maker_unknown", r.Error);
    }

    // ── overpayment is impossible ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Paying_more_than_was_approved_is_refused()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);                       // 100.00 commission
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);

        var over = PartnerSettlement.Pay(db, s.Id, Payer, 10_001, "bank", "REF-OVER", null, null);
        Assert.False(over.Ok);
        Assert.Equal("over_approved", over.Error);
        Assert.Equal(0L, H.L(db.QueryOne("SELECT amount_paid_minor FROM partner_settlements WHERE id=?", s.Id)!["amount_paid_minor"]));

        Assert.True(PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "REF-OK", null, null).Ok);
    }

    [Fact]
    public void Paying_the_same_settlement_twice_over_is_refused_on_the_second_call()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);

        Assert.True(PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "REF-1", null, null).Ok);
        // Now fully paid, so the status guard — not just the amount guard — stops a repeat.
        var again = PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "REF-2", null, null);
        Assert.False(again.Ok);
        Assert.Equal("bad_status", again.Error);
        Assert.Equal(10_000L, H.L(db.QueryOne("SELECT amount_paid_minor FROM partner_settlements WHERE id=?", s.Id)!["amount_paid_minor"]));
    }

    [Fact]
    public void A_duplicate_payment_reference_for_the_same_partner_is_refused()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        ApprovedSale(db, pid, 500, 2);

        // Two separate batches, each half the payable, so both are legitimately payable.
        var a = PartnerSettlement.Prepare(db, pid, Maker, null, null, 10_000, null);
        PartnerSettlement.Approve(db, a.Id, Checker);
        Assert.True(PartnerSettlement.Pay(db, a.Id, Payer, 10_000, "bank", "WIRE-777", null, null).Ok);

        var b = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, b.Id, Checker);
        var dup = PartnerSettlement.Pay(db, b.Id, Payer, 5_000, "bank", "WIRE-777", null, null);
        Assert.False(dup.Ok);
        Assert.Equal("duplicate_reference", dup.Error);
    }

    // ── double payment is impossible ──────────────────────────────────────────────────────────────

    [Fact]
    public void A_commission_already_allocated_cannot_be_placed_in_a_second_settlement()
    {
        var db = Fresh();
        var pid = Partner(db);
        var txnId = ApprovedSale(db, pid, 1000, 1);

        var first = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(first.Ok);

        // Everything payable is already committed to the first batch, so a second finds nothing.
        var second = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.False(second.Ok);
        Assert.Equal("nothing_payable", second.Error);

        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM partner_settlement_items WHERE transaction_id=?", txnId));
        Assert.Equal(0L, PartnerSettlement.PayableMinor(db, pid));
    }

    [Fact]
    public void Payable_excludes_what_is_already_allocated_to_a_live_batch()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);                       // 100.00
        ApprovedSale(db, pid, 1000, 2);                       // 100.00
        Assert.Equal(20_000L, PartnerSettlement.PayableMinor(db, pid));

        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, 10_000, null);
        Assert.True(s.Ok);
        Assert.Equal(10_000L, PartnerSettlement.PayableMinor(db, pid));
    }

    // ── partial payment ───────────────────────────────────────────────────────────────────────────

    [Fact]
    public void A_part_payment_leaves_the_balance_outstanding_and_the_commission_partially_paid()
    {
        var db = Fresh();
        var pid = Partner(db);
        var txnId = ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);
        Assert.Equal(PartnerCommission.StatusScheduled, Status(db, txnId));

        var part = PartnerSettlement.Pay(db, s.Id, Payer, 4_000, "bank", "PART-1", null, null);
        Assert.True(part.Ok);
        var row = db.QueryOne("SELECT * FROM partner_settlements WHERE id=?", s.Id)!;
        Assert.Equal(4_000L, H.L(row["amount_paid_minor"]));
        Assert.Equal(6_000L, H.L(row["closing_balance_minor"]));
        Assert.Equal(PartnerSettlement.StatusApproved, H.Str(row["status"]));      // still payable
        Assert.Equal(PartnerCommission.StatusPartiallyPaid, Status(db, txnId));

        // The remainder settles it.
        Assert.True(PartnerSettlement.Pay(db, s.Id, Payer, 6_000, "bank", "PART-2", null, null).Ok);
        var done = db.QueryOne("SELECT * FROM partner_settlements WHERE id=?", s.Id)!;
        Assert.Equal(PartnerSettlement.StatusPaid, H.Str(done["status"]));
        Assert.Equal(0L, H.L(done["closing_balance_minor"]));
        Assert.NotNull(H.Str(done["paid_at"]));
        Assert.Equal(PartnerCommission.StatusPaid, Status(db, txnId));
    }

    // ── lifecycle ─────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void An_unapproved_settlement_cannot_be_paid()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);

        var r = PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "EARLY", null, null);
        Assert.False(r.Ok);
        Assert.Equal("bad_status", r.Error);
    }

    [Fact]
    public void A_paid_settlement_cannot_be_cancelled()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);
        PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "FINAL", null, null);

        var r = PartnerSettlement.Cancel(db, s.Id, Checker, "changed our mind");
        Assert.False(r.Ok);
        Assert.Equal("already_paid", r.Error);
        Assert.Equal(PartnerSettlement.StatusPaid,
            H.Str(db.QueryOne("SELECT status FROM partner_settlements WHERE id=?", s.Id)!["status"]));
    }

    [Fact]
    public void Cancelling_releases_the_commission_back_into_the_payable_pool()
    {
        var db = Fresh();
        var pid = Partner(db);
        var txnId = ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);
        Assert.Equal(0L, PartnerSettlement.PayableMinor(db, pid));

        var r = PartnerSettlement.Cancel(db, s.Id, Checker, "wrong period");
        Assert.True(r.Ok);
        Assert.Equal(PartnerCommission.StatusApproved, Status(db, txnId));
        Assert.Equal(10_000L, PartnerSettlement.PayableMinor(db, pid));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM partner_settlement_items WHERE settlement_id=?", s.Id));

        // And it can then be settled properly.
        Assert.True(PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null).Ok);
    }

    [Fact]
    public void Cancelling_requires_a_reason_and_keeps_the_preparers_note()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, "March run, checked against bank file");

        Assert.Equal("reason_required", PartnerSettlement.Cancel(db, s.Id, Checker, "   ").Error);
        Assert.True(PartnerSettlement.Cancel(db, s.Id, Checker, "duplicate batch").Ok);

        var note = H.Str(db.QueryOne("SELECT internal_note FROM partner_settlements WHERE id=?", s.Id)!["internal_note"])!;
        Assert.Contains("March run, checked against bank file", note);
        Assert.Contains("duplicate batch", note);
    }

    // ── reversals interact with settlement ────────────────────────────────────────────────────────

    [Fact]
    public void Commission_recovered_after_payout_reduces_what_is_payable_next_time()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);                       // 100.00, settled below
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);
        PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "PAID-1", null, null);

        // That sale is refunded after the money went out, creating a 100.00 recoverable.
        var payId = db.Scalar<long>("SELECT payment_id FROM partner_commission_transactions WHERE partner_id=? ORDER BY id LIMIT 1", pid);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);
        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, payId) > 0);
        Assert.Equal(10_000L, PartnerCommissionReversal.RecoverableMinor(db, pid));

        // A later 1050.00 sale earns 105.00 — but 100.00 is owed back, so only 5.00 is payable.
        ApprovedSale(db, pid, 1050, 2);
        Assert.Equal(10_000L, PartnerSettlement.UnrecoveredMinor(db, pid));
        Assert.Equal(500L, PartnerSettlement.PayableMinor(db, pid));
    }

    [Fact]
    public void A_recoverable_is_recouped_once_and_never_deducted_again()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);
        PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "PAID-1", null, null);
        var payId = db.Scalar<long>("SELECT payment_id FROM partner_commission_transactions WHERE partner_id=? ORDER BY id LIMIT 1", pid);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);
        PartnerCommissionReversal.EnsureForPayment(db, payId);           // 100.00 owed back

        // A 1050.00 sale earns 105.00; the batch nets the 100.00 debt off, paying 5.00.
        ApprovedSale(db, pid, 1050, 2);
        var second = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(second.Ok);
        var row = db.QueryOne("SELECT * FROM partner_settlements WHERE id=?", second.Id)!;
        Assert.Equal(10_500L, H.L(row["eligible_commission_minor"]));
        Assert.Equal(-10_000L, H.L(row["adjustments_minor"]));
        Assert.Equal(500L, H.L(row["amount_approved_minor"]));

        // The debt is now settled. Left uncleared it would be subtracted from every future batch, quietly
        // underpaying the partner by 100.00 forever.
        Assert.Equal(0L, PartnerSettlement.UnrecoveredMinor(db, pid));
        PartnerSettlement.Approve(db, second.Id, Checker);
        PartnerSettlement.Pay(db, second.Id, Payer, 500, "bank", "PAID-2", null, null);

        ApprovedSale(db, pid, 300, 3);                                    // earns 30.00, all of it payable
        Assert.Equal(3_000L, PartnerSettlement.PayableMinor(db, pid));
    }

    [Fact]
    public void Cancelling_a_batch_releases_its_recoverable_offset_too()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);
        PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "PAID-1", null, null);
        var payId = db.Scalar<long>("SELECT payment_id FROM partner_commission_transactions WHERE partner_id=? ORDER BY id LIMIT 1", pid);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);
        PartnerCommissionReversal.EnsureForPayment(db, payId);

        ApprovedSale(db, pid, 1050, 2);
        var second = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.Equal(0L, PartnerSettlement.UnrecoveredMinor(db, pid));

        // Cancelling must give the debt back, or PCI would silently write off 100.00 it is owed.
        Assert.True(PartnerSettlement.Cancel(db, second.Id, Checker, "wrong period").Ok);
        Assert.Equal(10_000L, PartnerSettlement.UnrecoveredMinor(db, pid));
    }

    [Fact]
    public void A_partner_who_owes_more_than_they_have_earned_cannot_be_paid()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        PartnerSettlement.Approve(db, s.Id, Checker);
        PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "PAID-1", null, null);
        var payId = db.Scalar<long>("SELECT payment_id FROM partner_commission_transactions WHERE partner_id=? ORDER BY id LIMIT 1", pid);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);
        PartnerCommissionReversal.EnsureForPayment(db, payId);

        ApprovedSale(db, pid, 200, 2);                        // earns 20.00 against a 100.00 debt
        var r = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.False(r.Ok);
        Assert.Equal("offset_by_recoverable", r.Error);
    }

    // ── guards ────────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void A_partner_with_nothing_approved_cannot_have_a_settlement_prepared()
    {
        var db = Fresh();
        var pid = Partner(db);
        var r = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.False(r.Ok);
        Assert.Equal("nothing_payable", r.Error);
    }

    [Fact]
    public void Commission_still_on_refund_hold_is_not_payable()
    {
        var db = Fresh();
        var pid = Partner(db, holdDays: 30);
        var uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status) VALUES('hold@pci.test','H','S','student','active')");
        var codeId = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id)
            VALUES('HOLD-1','percentage',0,'exam',1,'active','campaign',?)", pid);
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,amount_refunded)
            VALUES(?, 'exam',1000,1000,'USD','test','paid','2026-03-01 10:00:00',0)", uid);
        db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount,redeemed_at)
            VALUES(?, 'HOLD-1',?,?, 'exam',1000,0,'2026-03-01 10:00:00')", codeId, uid, payId);
        PartnerCommission.EnsureForPayment(db, payId);

        // Earned, but not approved, so not payable — a hold that only the UI respected would be no hold.
        Assert.Equal(0L, PartnerSettlement.PayableMinor(db, pid));
        Assert.Equal("nothing_payable", PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null).Error);
    }

    [Fact]
    public void Preparing_for_an_unknown_partner_is_refused()
    {
        var db = Fresh();
        var r = PartnerSettlement.Prepare(db, 999_999, Maker, null, null, null, null);
        Assert.False(r.Ok);
        Assert.Equal("partner_not_found", r.Error);
    }

    [Fact]
    public void Re_preparing_the_same_partner_within_a_second_does_not_collide_on_the_settlement_number()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);

        // settlement_no is UNIQUE and derived from partner + second. A cancelled batch keeps its number, so
        // preparing again inside the same second is the case that would abort the insert.
        var first = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(first.Ok);
        Assert.True(PartnerSettlement.Cancel(db, first.Id, Checker, "re-run").Ok);
        var second = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);

        Assert.True(second.Ok);
        Assert.NotEqual(first.Id, second.Id);
        Assert.Equal(2L, db.Scalar<long>("SELECT COUNT(DISTINCT settlement_no) FROM partner_settlements WHERE partner_id=?", pid));
    }

    // ── refunds against an approved-but-unpaid batch ──────────────────────────────────────────────
    // A reversal never edits the source transaction, so a batch prepared before the refund carries a
    // frozen allocation that is now too large. Paying it anyway would hand the partner commission on
    // money the student got back.

    static long PaymentOf(Db db, long txnId)
        => H.L(db.QueryOne("SELECT payment_id FROM partner_commission_transactions WHERE id=?", txnId)!["payment_id"]);

    [Fact]
    public void A_full_refund_after_approval_blocks_payment_of_the_batch()
    {
        var db = Fresh();
        var pid = Partner(db);
        var txn = ApprovedSale(db, pid, 1000, 1);             // 100.00 commission
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(PartnerSettlement.Approve(db, s.Id, Checker).Ok);

        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", PaymentOf(db, txn));
        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, PaymentOf(db, txn)) > 0);

        var pay = PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "REF-REV", null, null);
        Assert.False(pay.Ok);
        Assert.Equal("stale_allocation", pay.Error);
        Assert.Equal(0L, H.L(db.QueryOne("SELECT amount_paid_minor FROM partner_settlements WHERE id=?", s.Id)!["amount_paid_minor"]));

        // The prescribed recovery leads somewhere honest: cancel, and a fresh prepare finds nothing owed.
        Assert.True(PartnerSettlement.Cancel(db, s.Id, Checker, "refunded before payment").Ok);
        Assert.Equal("nothing_payable", PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null).Error);
        Assert.Equal(0L, PartnerSettlement.PayableMinor(db, pid));
    }

    [Fact]
    public void A_partial_refund_before_preparation_shrinks_the_allocation_to_the_net_worth()
    {
        var db = Fresh();
        var pid = Partner(db);
        var txn = ApprovedSale(db, pid, 1000, 1);             // 100.00 commission
        db.Execute("UPDATE payments SET payment_status='partially_refunded', amount_refunded=250 WHERE id=?", PaymentOf(db, txn));
        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, PaymentOf(db, txn)) > 0);   // −25.00

        Assert.Equal(7_500L, PartnerSettlement.PayableMinor(db, pid));
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(s.Ok);
        Assert.Equal(7_500L, H.L(db.QueryOne("SELECT amount_approved_minor FROM partner_settlements WHERE id=?", s.Id)!["amount_approved_minor"]));
        Assert.True(PartnerSettlement.Approve(db, s.Id, Checker).Ok);

        // The net amount pays; the pre-refund amount does not.
        Assert.Equal("over_approved", PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "REF-N1", null, null).Error);
        Assert.True(PartnerSettlement.Pay(db, s.Id, Payer, 7_500, "bank", "REF-N2", null, null).Ok);
    }

    [Fact]
    public void A_partial_refund_between_approval_and_payment_forces_a_re_prepare_at_the_net_amount()
    {
        var db = Fresh();
        var pid = Partner(db);
        var txn = ApprovedSale(db, pid, 1000, 1);
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(PartnerSettlement.Approve(db, s.Id, Checker).Ok);

        db.Execute("UPDATE payments SET payment_status='partially_refunded', amount_refunded=250 WHERE id=?", PaymentOf(db, txn));
        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, PaymentOf(db, txn)) > 0);

        var pay = PartnerSettlement.Pay(db, s.Id, Payer, 10_000, "bank", "REF-P1", null, null);
        Assert.False(pay.Ok);
        Assert.Equal("stale_allocation", pay.Error);

        Assert.True(PartnerSettlement.Cancel(db, s.Id, Checker, "partial refund landed first").Ok);
        var again = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(again.Ok);
        Assert.Equal(7_500L, H.L(db.QueryOne("SELECT amount_approved_minor FROM partner_settlements WHERE id=?", again.Id)!["amount_approved_minor"]));
        Assert.True(PartnerSettlement.Approve(db, again.Id, Checker).Ok);
        Assert.True(PartnerSettlement.Pay(db, again.Id, Payer, 7_500, "bank", "REF-P2", null, null).Ok);
    }

    [Fact]
    public void Retrying_a_payment_with_the_reference_already_recorded_on_the_settlement_is_refused()
    {
        var db = Fresh();
        var pid = Partner(db);
        ApprovedSale(db, pid, 1000, 1);                       // 100.00 commission
        var s = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(PartnerSettlement.Approve(db, s.Id, Checker).Ok);

        Assert.True(PartnerSettlement.Pay(db, s.Id, Payer, 5_000, "bank", "WIRE-42", null, null).Ok);
        // The same transfer submitted again (timeout retry, double-click) must not double-record.
        var retry = PartnerSettlement.Pay(db, s.Id, Payer, 5_000, "bank", "WIRE-42", null, null);
        Assert.False(retry.Ok);
        Assert.Equal("duplicate_reference", retry.Error);
        Assert.Equal(5_000L, H.L(db.QueryOne("SELECT amount_paid_minor FROM partner_settlements WHERE id=?", s.Id)!["amount_paid_minor"]));
        // A genuine second instalment arrives under its own reference and completes the batch.
        Assert.True(PartnerSettlement.Pay(db, s.Id, Payer, 5_000, "bank", "WIRE-43", null, null).Ok);
        Assert.Equal(10_000L, H.L(db.QueryOne("SELECT amount_paid_minor FROM partner_settlements WHERE id=?", s.Id)!["amount_paid_minor"]));
    }

    // ── currency partition ────────────────────────────────────────────────────────────────────────
    // Minor units of different currencies must never be summed or offset against each other — netting a
    // EUR debt off a USD batch invents an exchange rate of 1. Commission snapshots the agreement's
    // currency at earn time, so an agreement moving from EUR to USD legitimately leaves one partner
    // holding balances in both.

    [Fact]
    public void A_recoverable_in_one_currency_never_offsets_or_blocks_a_batch_in_another()
    {
        var db = Fresh();
        var pid = Partner(db);

        // EUR era: earn, settle and pay EUR 100.00 of commission, then the sale fully refunds —
        // leaving a recoverable of EUR 100.00 (money already left; owed back).
        db.Execute("UPDATE partner_agreements SET currency='EUR' WHERE partner_id=?", pid);
        var txnE = ApprovedSale(db, pid, 1000, 1);
        var sE = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(PartnerSettlement.Approve(db, sE.Id, Checker).Ok);
        Assert.True(PartnerSettlement.Pay(db, sE.Id, Payer, 10_000, "bank", "EUR-WIRE-1", null, null).Ok);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", PaymentOf(db, txnE));
        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, PaymentOf(db, txnE)) > 0);

        // USD era: a fresh approved USD 50.00 commission.
        db.Execute("UPDATE partner_agreements SET currency='USD' WHERE partner_id=?", pid);
        ApprovedSale(db, pid, 500, 2);

        // The balances partition; nothing bleeds across the currency boundary.
        Assert.Equal(10_000L, PartnerCommissionReversal.RecoverableMinor(db, pid, "EUR"));
        Assert.Equal(0L, PartnerCommissionReversal.RecoverableMinor(db, pid, "USD"));
        Assert.Equal(10_000L, PartnerSettlement.UnrecoveredMinor(db, pid, "EUR"));
        Assert.Equal(0L, PartnerSettlement.UnrecoveredMinor(db, pid, "USD"));
        Assert.Equal(5_000L, PartnerSettlement.PayableMinor(db, pid, "USD"));
        // Negative payable in a currency = the partner owes that much in it (the EUR claw-back).
        Assert.Equal(-10_000L, PartnerSettlement.PayableMinor(db, pid, "EUR"));

        // The USD batch prepares at its full worth: no EUR offset in the adjustment, and no
        // "offset_by_recoverable" refusal computed from a debt in a different currency.
        var sU = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(sU.Ok);
        var row = db.QueryOne("SELECT currency, amount_approved_minor, adjustments_minor FROM partner_settlements WHERE id=?", sU.Id)!;
        Assert.Equal("USD", H.Str(row["currency"]));
        Assert.Equal(5_000L, H.L(row["amount_approved_minor"]));
        Assert.Equal(0L, H.L(row["adjustments_minor"]));
        Assert.True(PartnerSettlement.Approve(db, sU.Id, Checker).Ok);
        Assert.True(PartnerSettlement.Pay(db, sU.Id, Payer, 5_000, "bank", "USD-WIRE-1", null, null).Ok);

        // The EUR debt is still fully on the books, untouched by the USD settlement.
        Assert.Equal(10_000L, PartnerSettlement.UnrecoveredMinor(db, pid, "EUR"));
    }

    [Fact]
    public void A_same_currency_recoverable_still_offsets_the_next_batch()
    {
        var db = Fresh();
        var pid = Partner(db);                                // agreement currency defaults to USD

        var txn1 = ApprovedSale(db, pid, 1000, 1);            // USD 100.00, settled and paid
        var s1 = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(PartnerSettlement.Approve(db, s1.Id, Checker).Ok);
        Assert.True(PartnerSettlement.Pay(db, s1.Id, Payer, 10_000, "bank", "OFF-WIRE-1", null, null).Ok);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", PaymentOf(db, txn1));
        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, PaymentOf(db, txn1)) > 0);

        ApprovedSale(db, pid, 1500, 2);                       // USD 150.00 newly approved
        var s2 = PartnerSettlement.Prepare(db, pid, Maker, null, null, null, null);
        Assert.True(s2.Ok);
        var row = db.QueryOne("SELECT amount_approved_minor, adjustments_minor FROM partner_settlements WHERE id=?", s2.Id)!;
        // The USD debt nets off the USD batch exactly as before the currency partition.
        Assert.Equal(-10_000L, H.L(row["adjustments_minor"]));
        Assert.Equal(5_000L, H.L(row["amount_approved_minor"]));
    }
}
