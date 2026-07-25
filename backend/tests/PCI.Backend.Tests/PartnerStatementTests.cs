using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Partner finance Phase 3 — statements.
///
/// A statement is the document a partner reconciles their bank account against, so the property that
/// matters is that it BALANCES: opening + earned + reversals − paid = closing, over any period, with
/// activity outside the period excluded. These tests pin that identity, the period boundaries (a
/// statement that silently dropped the last day of the month would be worse than no statement), and
/// that the rendered CSV and PDF are produced from the same numbers rather than recomputed.
/// </summary>
[Collection(DbCollection.Name)]
public class PartnerStatementTests
{
    static Db Fresh()
    {
        var db = TestEnv.NewMigratedDb();
        FinanceSchema.Ensure(db);
        return db;
    }

    static long Partner(Db db)
    {
        var pid = db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES('Statement Partner','registered','active',10,'marketing')");
        var aid = db.ExecuteReturningId(@"INSERT INTO partner_agreements(partner_id,agreement_number,effective_from,status,refund_hold_days)
            VALUES(?,?, '2026-01-01','active',0)", pid, "AGR-ST-" + pid);
        db.Execute(@"INSERT INTO partner_commission_rules(agreement_id,partner_id,commission_type,commission_rate_bp,commission_basis,priority,active)
            VALUES(?,?, 'percentage',1000,'net_after_discount',100,1)", aid, pid);
        return pid;
    }

    /// <summary>A settled sale on a given date. Returns (paymentId, transactionId).</summary>
    static (long payId, long txnId) Sale(Db db, long partnerId, double amount, string paidOn, int seq)
    {
        var uid = db.ExecuteReturningId(
            "INSERT INTO users(email,first_name,last_name,role,status) VALUES(?, 'St','Student','student','active')",
            $"st{partnerId}.{seq}@pci.test");
        var codeId = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id)
            VALUES(?, 'percentage',0,'exam',1,'active','campaign',?)", $"ST-{partnerId}-{seq}", partnerId);
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,amount_refunded)
            VALUES(?, 'exam',?,?, 'USD','test','paid',?,0)", uid, amount, amount, paidOn);
        db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount,redeemed_at)
            VALUES(?,?,?,?, 'exam',?,0,?)", codeId, $"ST-{partnerId}-{seq}", uid, payId, amount, paidOn);
        return (payId, PartnerCommission.EnsureForPayment(db, payId));
    }

    [Fact]
    public void A_statement_balances_opening_plus_earned_minus_paid_to_closing()
    {
        var db = Fresh();
        var pid = Partner(db);
        Sale(db, pid, 1000, "2026-03-05 09:00:00", 1);        // 100.00
        Sale(db, pid, 500, "2026-03-20 09:00:00", 2);         //  50.00

        var st = PartnerStatement.Build(db, pid, "2026-03-01", "2026-03-31");

        Assert.Equal(0L, st.OpeningMinor);
        Assert.Equal(15_000L, st.EarnedMinor);
        Assert.Equal(0L, st.ReversedMinor);
        Assert.Equal(0L, st.PaidMinor);
        Assert.Equal(st.OpeningMinor + st.EarnedMinor + st.ReversedMinor - st.PaidMinor, st.ClosingMinor);
        Assert.Equal(15_000L, st.ClosingMinor);
        Assert.Equal(2, st.Lines.Count);
    }

    [Fact]
    public void Activity_before_the_period_becomes_the_opening_balance_not_a_line()
    {
        var db = Fresh();
        var pid = Partner(db);
        Sale(db, pid, 1000, "2026-02-10 09:00:00", 1);        // February: 100.00
        Sale(db, pid, 200, "2026-03-10 09:00:00", 2);         // March:     20.00

        var march = PartnerStatement.Build(db, pid, "2026-03-01", "2026-03-31");

        Assert.Equal(10_000L, march.OpeningMinor);
        Assert.Equal(2_000L, march.EarnedMinor);
        Assert.Equal(12_000L, march.ClosingMinor);
        Assert.Single(march.Lines);                            // February is summarised, not repeated
        Assert.Equal("ST-" + pid + "-2", march.Lines[0].Code);
    }

    [Fact]
    public void The_last_day_of_the_period_is_included_in_full()
    {
        var db = Fresh();
        var pid = Partner(db);
        // 23:59 on the closing date must land inside the period — an exclusive bound would drop it and the
        // partner would chase a commission that appears in neither month.
        Sale(db, pid, 1000, "2026-03-31 23:59:00", 1);
        Sale(db, pid, 1000, "2026-04-01 00:01:00", 2);

        var march = PartnerStatement.Build(db, pid, "2026-03-01", "2026-03-31");
        Assert.Single(march.Lines);
        Assert.Equal(10_000L, march.EarnedMinor);

        var april = PartnerStatement.Build(db, pid, "2026-04-01", "2026-04-30");
        Assert.Single(april.Lines);
        Assert.Equal(10_000L, april.EarnedMinor);
        Assert.Equal(10_000L, april.OpeningMinor);             // March's commission carried forward
    }

    /// <summary>
    /// A period wide enough to contain "now". Reversals and settlements are dated when they happen, so a
    /// test that pinned them to a fixed month would pass today and fail whenever the suite is next run.
    /// </summary>
    const string AllTimeStart = "2026-01-01";
    const string AllTimeEnd = "2099-12-31";

    [Fact]
    public void A_refund_shows_as_a_reversal_line_and_reduces_the_closing_balance()
    {
        var db = Fresh();
        var pid = Partner(db);
        var (payId, _) = Sale(db, pid, 1000, "2026-03-05 09:00:00", 1);
        db.Execute("UPDATE payments SET payment_status='partially_refunded', amount_refunded=400 WHERE id=?", payId);
        PartnerCommissionReversal.EnsureForPayment(db, payId);

        var st = PartnerStatement.Build(db, pid, AllTimeStart, AllTimeEnd);

        Assert.Equal(10_000L, st.EarnedMinor);
        Assert.Equal(-4_000L, st.ReversedMinor);
        Assert.Equal(6_000L, st.ClosingMinor);
        Assert.Equal(2, st.Lines.Count);
        Assert.Contains(st.Lines, l => l.Kind == "reversal" && l.CommissionMinor == -4_000L);
        Assert.Equal(st.OpeningMinor + st.EarnedMinor + st.ReversedMinor - st.PaidMinor, st.ClosingMinor);
    }

    [Fact]
    public void A_later_reversal_does_not_retroactively_rewrite_a_closed_period()
    {
        var db = Fresh();
        var pid = Partner(db);
        var (payId, _) = Sale(db, pid, 1000, "2026-03-05 09:00:00", 1);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);
        PartnerCommissionReversal.EnsureForPayment(db, payId);

        // A statement the partner already filed for March must still say what it said. The claw-back belongs
        // to the period it occurred in — restating a closed month is precisely what an immutable ledger
        // exists to prevent.
        var march = PartnerStatement.Build(db, pid, "2026-03-01", "2026-03-31");
        Assert.Equal(10_000L, march.EarnedMinor);
        Assert.Equal(0L, march.ReversedMinor);
        Assert.Single(march.Lines);
        Assert.Equal("commission", march.Lines[0].Kind);

        // Across all time the two cancel out, so nothing is lost — only re-dated.
        var all = PartnerStatement.Build(db, pid, AllTimeStart, AllTimeEnd);
        Assert.Equal(0L, all.ClosingMinor);
    }

    [Fact]
    public void A_settlement_paid_in_the_period_clears_the_closing_balance()
    {
        var db = Fresh();
        var pid = Partner(db);
        var (_, txnId) = Sale(db, pid, 1000, "2026-03-05 09:00:00", 1);
        PartnerCommission.Transition(db, txnId, PartnerCommission.StatusApproved, "admin", 22);
        var s = PartnerSettlement.Prepare(db, pid, 11, "2026-03-01", "2026-03-31", null, null);
        PartnerSettlement.Approve(db, s.Id, 22);
        PartnerSettlement.Pay(db, s.Id, 33, 10_000, "bank", "STMT-1", null, null);

        var st = PartnerStatement.Build(db, pid, AllTimeStart, AllTimeEnd);

        Assert.Equal(10_000L, st.EarnedMinor);
        Assert.Equal(10_000L, st.PaidMinor);
        Assert.Equal(0L, st.ClosingMinor);
        Assert.Single(st.Settlements);
    }

    [Fact]
    public void An_empty_period_produces_a_statement_rather_than_an_error()
    {
        var db = Fresh();
        var pid = Partner(db);
        var st = PartnerStatement.Build(db, pid, "2026-05-01", "2026-05-31");

        Assert.Empty(st.Lines);
        Assert.Equal(0L, st.EarnedMinor);
        Assert.Equal(0L, st.ClosingMinor);
        Assert.Equal("Statement Partner", st.PartnerName);
        Assert.Contains("Opening balance,0.00", PartnerStatement.Csv(st));
    }

    [Fact]
    public void The_csv_carries_every_line_and_survives_a_comma_in_the_partner_name()
    {
        var db = Fresh();
        var pid = Partner(db);
        db.Execute("UPDATE training_partners SET name='Acme, Inc.' WHERE id=?", pid);
        Sale(db, pid, 1000, "2026-03-05 09:00:00", 1);

        var csv = PartnerStatement.Csv(PartnerStatement.Build(db, pid, "2026-03-01", "2026-03-31"));

        Assert.Contains("\"Acme, Inc.\"", csv);                // quoted, so the column count is unchanged
        Assert.Contains("Closing balance,100.00", csv);
        Assert.Contains("ST-" + pid + "-1", csv);
        Assert.Contains("Reference,Date,Type,Code,Product,Currency,Gross,Discount,Eligible net,Rate %,Commission,Status", csv);
    }

    [Fact]
    public void The_pdf_is_a_real_pdf_and_names_the_partner_and_period()
    {
        var db = Fresh();
        var pid = Partner(db);
        Sale(db, pid, 1000, "2026-03-05 09:00:00", 1);
        var st = PartnerStatement.Build(db, pid, "2026-03-01", "2026-03-31");

        var pdf = PartnerStatement.Pdf(st, "Test Org");

        Assert.True(pdf.Length > 500);
        Assert.Equal("%PDF", System.Text.Encoding.ASCII.GetString(pdf, 0, 4));
        var text = System.Text.Encoding.Latin1.GetString(pdf);
        Assert.Contains("Statement Partner", text);
        Assert.Contains("2026-03-01", text);
    }

    [Fact]
    public void A_remittance_advice_lists_the_commission_the_payment_covered()
    {
        var db = Fresh();
        var pid = Partner(db);
        var (_, txnId) = Sale(db, pid, 1000, "2026-03-05 09:00:00", 1);
        PartnerCommission.Transition(db, txnId, PartnerCommission.StatusApproved, "admin", 22);
        var s = PartnerSettlement.Prepare(db, pid, 11, "2026-03-01", "2026-03-31", null, null);
        PartnerSettlement.Approve(db, s.Id, 22);
        PartnerSettlement.Pay(db, s.Id, 33, 10_000, "bank transfer", "WIRE-4242", null, null);

        var pdf = PartnerStatement.RemittancePdf(db, s.Id, "Test Org");

        Assert.NotNull(pdf);
        Assert.Equal("%PDF", System.Text.Encoding.ASCII.GetString(pdf!, 0, 4));
        var text = System.Text.Encoding.Latin1.GetString(pdf!);
        Assert.Contains("WIRE-4242", text);
        Assert.Contains("ST-" + pid + "-1", text);             // the code the commission came from
    }

    [Fact]
    public void A_remittance_advice_for_an_unknown_settlement_is_null_not_an_exception()
    {
        var db = Fresh();
        Assert.Null(PartnerStatement.RemittancePdf(db, 999_999, "Test Org"));
    }

    [Fact]
    public void MonthOf_covers_the_whole_calendar_month_including_a_leap_February()
    {
        var (s1, e1) = PartnerStatement.MonthOf(new DateTime(2026, 3, 14, 0, 0, 0, DateTimeKind.Utc));
        Assert.Equal("2026-03-01", s1);
        Assert.Equal("2026-03-31", e1);

        var (s2, e2) = PartnerStatement.MonthOf(new DateTime(2028, 2, 9, 0, 0, 0, DateTimeKind.Utc));
        Assert.Equal("2028-02-01", s2);
        Assert.Equal("2028-02-29", e2);
    }
}
