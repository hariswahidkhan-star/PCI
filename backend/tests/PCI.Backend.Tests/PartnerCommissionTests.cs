using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Partner finance Phase 1 — the immutable commission ledger.
///
/// Pins the properties that make the ledger accounting-safe rather than a report:
///   · exactly one commission per settled, partner-attributed payment, and a replay adds nothing;
///   · the rate/basis/rule are SNAPSHOTTED — changing the agreement afterwards never restates history;
///   · rule selection honours priority, then specificity, then recency, across certification /
///     product / route / country;
///   · money is exact integer minor-unit arithmetic, including the half-away-from-zero boundary;
///   · sponsored routes and unsettled/unattributed payments earn nothing;
///   · the status machine refuses illegal moves and records every legal one.
/// Runs against a real migrated database (TestEnv.NewMigratedDb) — the same schema production migrates,
/// on SQLite by default and on MySQL/MariaDB when TEST_DB_PROVIDER=mysql.
/// </summary>
[Collection(DbCollection.Name)]
public class PartnerCommissionTests
{
    // ---- helpers ---------------------------------------------------------------------------------
    static Db Fresh()
    {
        var db = TestEnv.NewMigratedDb();
        FinanceSchema.Ensure(db);
        return db;
    }

    static long NewPartner(Db db, double commissionPct = 10, string name = "Test Marketing Partner")
        => db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES(?, 'registered','active',?, 'marketing')",
            name, commissionPct);

    static long NewUser(Db db, string email, string? country = null)
    {
        var uid = db.ExecuteReturningId(
            "INSERT INTO users(email,first_name,last_name,role,status) VALUES(?, 'Test','Redeemer','student','active')", email);
        if (country is not null) db.Execute("INSERT INTO student_profiles(user_id,country) VALUES(?,?)", uid, country);
        return uid;
    }

    static long NewCode(Db db, long partnerId, string code, long? certId = null, string? routeKey = null)
        => db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id,certification_id,route_key)
            VALUES(?, 'percentage',15,'exam',1,'active','campaign',?,?,?)", code, partnerId, certId, routeKey);

    /// <summary>A settled payment plus its redemption — the exact shape the webhook writes.</summary>
    static long NewPaidRedemption(Db db, long userId, long codeId, string code, double listPrice, double finalAmount,
        string product = "exam", string paidOn = "2026-03-10 12:00:00")
    {
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,reference)
            VALUES(?,?,?,?, 'USD','test','paid',?,?)", userId, product, listPrice, finalAmount, paidOn, "TEST-" + code);
        db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount,redeemed_at)
            VALUES(?,?,?,?,?,?,?,?)", codeId, code, userId, payId, product, listPrice, listPrice - finalAmount, paidOn);
        return payId;
    }

    static long AddAgreement(Db db, long partnerId, string number, string from, string? to = null, int holdDays = 30)
        => db.ExecuteReturningId(@"INSERT INTO partner_agreements(partner_id,agreement_number,effective_from,effective_to,status,refund_hold_days)
            VALUES(?,?,?,?, 'active',?)", partnerId, number, from, to, holdDays);

    static long AddRule(Db db, long agreementId, long partnerId, long rateBp, long? certId = null,
        string? productType = null, string? routeKey = null, string? country = null, long priority = 100,
        string basis = "net_after_discount", string? from = null, string? to = null)
        => db.ExecuteReturningId(@"INSERT INTO partner_commission_rules
            (agreement_id,partner_id,certification_id,route_key,product_type,country,commission_type,commission_rate_bp,commission_basis,effective_from,effective_to,priority,active)
            VALUES(?,?,?,?,?,?, 'percentage',?,?,?,?,?,1)",
            agreementId, partnerId, certId, routeKey, productType, country, rateBp, basis, from, to, priority);

    static Dictionary<string, object?>? Txn(Db db, long payId)
        => db.QueryOne("SELECT * FROM partner_commission_transactions WHERE payment_id=?", payId);

    // ---- Money: exact integer arithmetic ---------------------------------------------------------
    [Theory]
    [InlineData(0.0, 0L)]
    [InlineData(149.5, 14950L)]
    [InlineData(0.1, 10L)]
    [InlineData(1234.56, 123456L)]
    public void ToMinor_converts_major_units_exactly(double major, long expected)
        => Assert.Equal(expected, Money.ToMinor(major));

    [Fact]
    public void ApplyRate_uses_exact_integer_maths_and_rounds_half_away_from_zero()
    {
        Assert.Equal(1000L, Money.ApplyRate(10_000L, 1000));   // 10% of 100.00 = 10.00
        Assert.Equal(750L, Money.ApplyRate(10_000L, 750));     // 7.5% of 100.00 = 7.50
        // 7.5% of 0.07 = 0.00525 → half-away-from-zero on the cent → 0.01
        Assert.Equal(1L, Money.ApplyRate(7L, 750));
        Assert.Equal(0L, Money.ApplyRate(0L, 1000));
        Assert.Equal(0L, Money.ApplyRate(10_000L, 0));
    }

    [Fact]
    public void ApplyRate_preserves_sign_so_a_reversal_cancels_its_original_exactly()
    {
        var original = Money.ApplyRate(12_345L, 733);
        var reversal = Money.ApplyRate(-12_345L, 733);
        Assert.Equal(-original, reversal);
        Assert.Equal(0L, original + reversal);
    }

    [Fact]
    public void Apportion_splits_a_partial_refund_proportionally_and_caps_at_the_whole()
    {
        Assert.Equal(5_000L, Money.Apportion(10_000L, 50, 100));   // half
        Assert.Equal(3_333L, Money.Apportion(10_000L, 1, 3));      // a third, rounded
        Assert.Equal(10_000L, Money.Apportion(10_000L, 5, 3));     // over-refund never exceeds the whole
        Assert.Equal(0L, Money.Apportion(10_000L, 0, 3));
        Assert.Equal(0L, Money.Apportion(10_000L, 1, 0));
    }

    [Fact]
    public void Apportion_uses_wide_intermediate_math_at_DECIMAL_12_2_boundaries()
    {
        Assert.Equal(1_000_000_000L,
            Money.Apportion(2_000_000_000L, 10_000_000_000L, 20_000_000_000L));
        Assert.Equal(-1_000_000_000L,
            Money.Apportion(-2_000_000_000L, 10_000_000_000L, 20_000_000_000L));
    }

    [Fact]
    public void ToMinor_preserves_decimal_values_and_rejects_non_finite_doubles()
    {
        Assert.Equal(4_513L, Money.ToMinor(45.125m));
        Assert.Throws<ArgumentOutOfRangeException>(() => Money.ToMinor(double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => Money.ToMinor(double.PositiveInfinity));
    }

    [Fact]
    public void PercentToBasisPoints_round_trips()
    {
        Assert.Equal(750L, Money.PercentToBasisPoints(7.5));
        Assert.Equal(7.5m, Money.BasisPointsToPercent(750));
        Assert.Equal("149.50", Money.Format(14_950L));
    }

    // ---- creation + idempotency ------------------------------------------------------------------
    [Fact]
    public void A_settled_attributed_payment_creates_exactly_one_commission_and_a_replay_adds_nothing()
    {
        var db = Fresh();
        var pid = NewPartner(db, commissionPct: 10);
        var aid = AddAgreement(db, pid, "AGR-1", "2026-01-01");
        AddRule(db, aid, pid, rateBp: 1000);
        var uid = NewUser(db, "one@pci.test");
        var codeId = NewCode(db, pid, "MKT-ONE");
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-ONE", listPrice: 1000, finalAmount: 850);

        var first = PartnerCommission.EnsureForPayment(db, payId);
        Assert.True(first > 0);
        var second = PartnerCommission.EnsureForPayment(db, payId);
        Assert.Equal(0L, second);                                   // replay is a no-op
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM partner_commission_transactions WHERE payment_id=?", payId));

        var t = Txn(db, payId)!;
        Assert.Equal(85_000L, H.L(t["eligible_net_minor"]));        // 850.00 collected
        Assert.Equal(100_000L, H.L(t["gross_minor"]));              // 1000.00 list
        Assert.Equal(15_000L, H.L(t["discount_minor"]));            // 150.00 discount
        Assert.Equal(8_500L, H.L(t["commission_minor"]));           // 10% of 850.00
        Assert.Equal(1000L, H.L(t["commission_rate_bp"]));          // snapshot
        Assert.Equal("net_after_discount", H.Str(t["commission_basis"]));
        Assert.Equal(PartnerCommission.StatusOnRefundHold, H.Str(t["status"]));
        Assert.Equal(0L, H.L(t["requires_finance_review"]));
        Assert.StartsWith("PCT-", H.Str(t["txn_ref"]));
    }

    [Fact]
    public void Changing_the_agreement_rate_afterwards_does_not_restate_history()
    {
        var db = Fresh();
        var pid = NewPartner(db, commissionPct: 10);
        var aid = AddAgreement(db, pid, "AGR-2", "2026-01-01");
        var ruleId = AddRule(db, aid, pid, rateBp: 1000);
        var uid = NewUser(db, "hist@pci.test");
        var codeId = NewCode(db, pid, "MKT-HIST");
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-HIST", 1000, 1000);
        PartnerCommission.EnsureForPayment(db, payId);
        var before = H.L(Txn(db, payId)!["commission_minor"]);
        Assert.Equal(10_000L, before);

        // The partner renegotiates: headline percentage AND the rule both change.
        db.Execute("UPDATE training_partners SET commission_pct=25 WHERE id=?", pid);
        db.Execute("UPDATE partner_commission_rules SET commission_rate_bp=2500 WHERE id=?", ruleId);

        Assert.Equal(before, H.L(Txn(db, payId)!["commission_minor"]));
        Assert.Equal(1000L, H.L(Txn(db, payId)!["commission_rate_bp"]));
        Assert.Equal(10_000L, PartnerCommission.Totals(db, pid)["commission"]);
    }

    [Fact]
    public void With_no_agreement_the_current_percentage_is_snapshotted_and_flagged_for_finance()
    {
        var db = Fresh();
        var pid = NewPartner(db, commissionPct: 12.5);
        var uid = NewUser(db, "norule@pci.test");
        var codeId = NewCode(db, pid, "MKT-NORULE");
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-NORULE", 1000, 800);

        Assert.True(PartnerCommission.EnsureForPayment(db, payId) > 0);
        var t = Txn(db, payId)!;
        Assert.Equal(1250L, H.L(t["commission_rate_bp"]));          // 12.5% snapshotted
        Assert.Equal(10_000L, H.L(t["commission_minor"]));          // 12.5% of 800.00
        Assert.Equal(1L, H.L(t["requires_finance_review"]));         // never guessed silently
    }

    [Theory]
    [InlineData("waived")]
    [InlineData("refunded")]
    [InlineData("failed")]
    public void An_unsettled_payment_earns_nothing(string status)
    {
        var db = Fresh();
        var pid = NewPartner(db);
        var uid = NewUser(db, $"{status}@pci.test");
        var codeId = NewCode(db, pid, "MKT-" + status.ToUpperInvariant());
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-" + status.ToUpperInvariant(), 1000, 900);
        db.Execute("UPDATE payments SET payment_status=? WHERE id=?", status, payId);

        Assert.Equal(0L, PartnerCommission.EnsureForPayment(db, payId));
        Assert.Null(Txn(db, payId));
    }

    [Fact]
    public void A_payment_with_no_partner_attribution_earns_nothing()
    {
        var db = Fresh();
        var uid = NewUser(db, "unattributed@pci.test");
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date)
            VALUES(?, 'exam',1000,1000,'USD','test','paid','2026-03-10 12:00:00')", uid);
        Assert.Equal(0L, PartnerCommission.EnsureForPayment(db, payId));
    }

    [Fact]
    public void A_sponsored_route_earns_nothing_because_the_money_flows_the_other_way()
    {
        var db = Fresh();
        var pid = NewPartner(db);
        var uid = NewUser(db, "sponsored@pci.test");
        var codeId = NewCode(db, pid, "MKT-SPON", routeKey: "sponsored");
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-SPON", 1000, 0);
        Assert.Equal(0L, PartnerCommission.EnsureForPayment(db, payId));
    }

    // ---- rule selection --------------------------------------------------------------------------
    [Fact]
    public void The_more_specific_rule_wins_at_equal_priority()
    {
        var db = Fresh();
        var pid = NewPartner(db);
        var aid = AddAgreement(db, pid, "AGR-3", "2026-01-01");
        AddRule(db, aid, pid, rateBp: 500);                              // catch-all
        AddRule(db, aid, pid, rateBp: 2000, certId: 2, productType: "exam");  // specific

        var uid = NewUser(db, "specific@pci.test");
        var codeId = NewCode(db, pid, "MKT-SPEC", certId: 2);
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-SPEC", 1000, 1000);
        PartnerCommission.EnsureForPayment(db, payId);

        Assert.Equal(2000L, H.L(Txn(db, payId)!["commission_rate_bp"]));
        Assert.Equal(20_000L, H.L(Txn(db, payId)!["commission_minor"]));
    }

    [Fact]
    public void A_lower_priority_number_beats_a_more_specific_rule()
    {
        var db = Fresh();
        var pid = NewPartner(db);
        var aid = AddAgreement(db, pid, "AGR-4", "2026-01-01");
        AddRule(db, aid, pid, rateBp: 3000, priority: 10);                                   // wins on priority
        AddRule(db, aid, pid, rateBp: 2000, certId: 2, productType: "exam", priority: 100);  // more specific

        var uid = NewUser(db, "priority@pci.test");
        var codeId = NewCode(db, pid, "MKT-PRIO", certId: 2);
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-PRIO", 1000, 1000);
        PartnerCommission.EnsureForPayment(db, payId);

        Assert.Equal(3000L, H.L(Txn(db, payId)!["commission_rate_bp"]));
    }

    [Fact]
    public void A_rule_scoped_to_another_certification_is_ignored()
    {
        var db = Fresh();
        var pid = NewPartner(db, commissionPct: 0);
        var aid = AddAgreement(db, pid, "AGR-5", "2026-01-01");
        AddRule(db, aid, pid, rateBp: 4000, certId: 3);   // PML-AI only

        var uid = NewUser(db, "othercert@pci.test");
        var codeId = NewCode(db, pid, "MKT-OTHER", certId: 1);
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-OTHER", 1000, 1000);
        PartnerCommission.EnsureForPayment(db, payId);

        var t = Txn(db, payId)!;
        Assert.Equal(0L, H.L(t["commission_rate_bp"]));       // fell back to the 0% headline
        Assert.Equal(1L, H.L(t["requires_finance_review"]));
    }

    [Fact]
    public void A_rule_outside_its_effective_window_is_ignored()
    {
        var db = Fresh();
        var pid = NewPartner(db, commissionPct: 0);
        var aid = AddAgreement(db, pid, "AGR-6", "2026-01-01");
        AddRule(db, aid, pid, rateBp: 5000, from: "2026-06-01");   // starts after the payment

        var uid = NewUser(db, "window@pci.test");
        var codeId = NewCode(db, pid, "MKT-WIN");
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-WIN", 1000, 1000, paidOn: "2026-03-10 12:00:00");
        PartnerCommission.EnsureForPayment(db, payId);

        Assert.Equal(1L, H.L(Txn(db, payId)!["requires_finance_review"]));
    }

    [Fact]
    public void The_gross_basis_values_the_list_price_not_the_collected_amount()
    {
        var db = Fresh();
        var pid = NewPartner(db);
        var aid = AddAgreement(db, pid, "AGR-7", "2026-01-01");
        AddRule(db, aid, pid, rateBp: 1000, basis: "gross");

        var uid = NewUser(db, "gross@pci.test");
        var codeId = NewCode(db, pid, "MKT-GROSS");
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-GROSS", listPrice: 1000, finalAmount: 800);
        PartnerCommission.EnsureForPayment(db, payId);

        Assert.Equal(10_000L, H.L(Txn(db, payId)!["commission_minor"]));   // 10% of 1000.00, not 800.00
    }

    // ---- state machine ---------------------------------------------------------------------------
    [Fact]
    public void The_state_machine_allows_the_settlement_path_and_refuses_shortcuts()
    {
        Assert.True(PartnerCommission.CanTransition(PartnerCommission.StatusOnRefundHold, PartnerCommission.StatusDue));
        Assert.True(PartnerCommission.CanTransition(PartnerCommission.StatusDue, PartnerCommission.StatusApproved));
        Assert.True(PartnerCommission.CanTransition(PartnerCommission.StatusApproved, PartnerCommission.StatusPaid));
        // A commission can never be paid straight off the hold, nor revived once reversed.
        Assert.False(PartnerCommission.CanTransition(PartnerCommission.StatusOnRefundHold, PartnerCommission.StatusPaid));
        Assert.False(PartnerCommission.CanTransition(PartnerCommission.StatusReversed, PartnerCommission.StatusDue));
        Assert.False(PartnerCommission.CanTransition(null, PartnerCommission.StatusPaid));
        Assert.False(PartnerCommission.CanTransition(PartnerCommission.StatusPaid, "something_invented"));
    }

    [Fact]
    public void Transition_records_an_event_and_refuses_an_illegal_move()
    {
        var db = Fresh();
        var pid = NewPartner(db);
        var aid = AddAgreement(db, pid, "AGR-8", "2026-01-01");
        AddRule(db, aid, pid, rateBp: 1000);
        var uid = NewUser(db, "state@pci.test");
        var codeId = NewCode(db, pid, "MKT-STATE");
        var payId = NewPaidRedemption(db, uid, codeId, "MKT-STATE", 1000, 1000);
        var txnId = PartnerCommission.EnsureForPayment(db, payId);

        Assert.True(PartnerCommission.Transition(db, txnId, PartnerCommission.StatusDue, "system", null, "hold elapsed"));
        Assert.True(PartnerCommission.Transition(db, txnId, PartnerCommission.StatusApproved, "admin", 42, "finance approved"));
        Assert.False(PartnerCommission.Transition(db, txnId, PartnerCommission.StatusOnRefundHold, "admin", 42));

        var t = Txn(db, payId)!;
        Assert.Equal(PartnerCommission.StatusApproved, H.Str(t["status"]));
        Assert.Equal(42L, H.L(t["approved_by"]));
        // creation + two accepted moves; the refused one wrote nothing
        Assert.Equal(3L, db.Scalar<long>("SELECT COUNT(*) FROM partner_commission_events WHERE transaction_id=?", txnId));
        var last = db.QueryOne("SELECT * FROM partner_commission_events WHERE transaction_id=? ORDER BY id DESC LIMIT 1", txnId)!;
        Assert.Equal("admin", H.Str(last["actor_type"]));
        Assert.Equal(42L, H.L(last["actor_id"]));
    }

    [Fact]
    public void Totals_bucket_by_lifecycle_stage()
    {
        var db = Fresh();
        var pid = NewPartner(db);
        var aid = AddAgreement(db, pid, "AGR-9", "2026-01-01", holdDays: 0);   // straight to due
        AddRule(db, aid, pid, rateBp: 1000);
        var codeId = NewCode(db, pid, "MKT-TOT");
        for (var i = 0; i < 3; i++)
        {
            var uid = NewUser(db, $"tot{i}@pci.test");
            var payId = NewPaidRedemption(db, uid, codeId, "MKT-TOT", 1000, 1000);
            var txnId = PartnerCommission.EnsureForPayment(db, payId);
            if (i == 0) PartnerCommission.Transition(db, txnId, PartnerCommission.StatusApproved, "admin", 1);
        }
        var totals = PartnerCommission.Totals(db, pid);
        Assert.Equal(30_000L, totals["commission"]);
        Assert.Equal(10_000L, totals["approved"]);
        Assert.Equal(20_000L, totals["due"]);
        Assert.True(PartnerCommission.HasLedger(db, pid));
    }
}
