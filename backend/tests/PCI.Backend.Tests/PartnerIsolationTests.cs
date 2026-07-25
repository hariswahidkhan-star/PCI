using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Partner finance Phase 6 — isolation and permission hardening.
///
/// The whole module rests on one claim: <b>a partner sees their own money and nobody else's</b>. Every
/// query in the module is scoped by partner_id, and this suite exists to prove that scoping actually
/// holds when two partners have overlapping-looking data, rather than trusting that each WHERE clause
/// was written correctly.
///
/// It also pins the permission posture: the pf_* capabilities must never become a side-effect of a job
/// title, because "who can move money" has to stay an explicit decision.
/// </summary>
[Collection(DbCollection.Name)]
public class PartnerIsolationTests
{
    static Db Fresh()
    {
        var db = TestEnv.NewMigratedDb();
        FinanceSchema.Ensure(db);
        return db;
    }

    /// <summary>Two partners, each with one settled sale of a different size, on identical terms.</summary>
    static (long a, long b) TwoPartners(Db db)
    {
        long Make(string name, string codeStr, double amount, int seq)
        {
            var pid = db.ExecuteReturningId(
                "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES(?, 'registered','active',10,'marketing')", name);
            var aid = db.ExecuteReturningId(@"INSERT INTO partner_agreements(partner_id,agreement_number,effective_from,status,refund_hold_days)
                VALUES(?,?, '2026-01-01','active',0)", pid, "AGR-ISO-" + pid);
            db.Execute(@"INSERT INTO partner_commission_rules(agreement_id,partner_id,commission_type,commission_rate_bp,commission_basis,priority,active)
                VALUES(?,?, 'percentage',1000,'net_after_discount',100,1)", aid, pid);
            var codeId = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id)
                VALUES(?, 'percentage',0,'exam',1,'active','campaign',?)", codeStr, pid);
            var uid = db.ExecuteReturningId(
                "INSERT INTO users(email,first_name,last_name,role,status) VALUES(?, 'I','S','student','active')", $"iso{seq}@pci.test");
            var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,amount_refunded)
                VALUES(?, 'exam',?,?, 'USD','test','paid','2026-04-01 10:00:00',0)", uid, amount, amount);
            db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount,redeemed_at)
                VALUES(?,?,?,?, 'exam',?,0,'2026-04-01 10:00:00')", codeId, codeStr, uid, payId, amount);
            PartnerCommission.EnsureForPayment(db, payId);
            return pid;
        }
        return (Make("Partner A", "ISO-A", 1000, 1), Make("Partner B", "ISO-B", 5000, 2));
    }

    [Fact]
    public void Each_partners_commission_totals_contain_only_their_own_sales()
    {
        var db = Fresh();
        var (a, b) = TwoPartners(db);

        // 100.00 and 500.00 — if any query lost its partner_id scope, both would read 600.00.
        Assert.Equal(10_000L, PartnerCommission.Totals(db, a)["commission"]);
        Assert.Equal(50_000L, PartnerCommission.Totals(db, b)["commission"]);
    }

    [Fact]
    public void Each_partners_payable_balance_contains_only_their_own_commission()
    {
        var db = Fresh();
        var (a, b) = TwoPartners(db);
        foreach (var pid in new[] { a, b })
            foreach (var t in db.Query("SELECT id FROM partner_commission_transactions WHERE partner_id=?", pid))
                PartnerCommission.Transition(db, H.L(t["id"]), PartnerCommission.StatusApproved, "admin", 1);

        Assert.Equal(10_000L, PartnerSettlement.PayableMinor(db, a));
        Assert.Equal(50_000L, PartnerSettlement.PayableMinor(db, b));
    }

    [Fact]
    public void A_settlement_prepared_for_one_partner_never_allocates_another_partners_commission()
    {
        var db = Fresh();
        var (a, b) = TwoPartners(db);
        foreach (var pid in new[] { a, b })
            foreach (var t in db.Query("SELECT id FROM partner_commission_transactions WHERE partner_id=?", pid))
                PartnerCommission.Transition(db, H.L(t["id"]), PartnerCommission.StatusApproved, "admin", 1);

        var s = PartnerSettlement.Prepare(db, a, 11, null, null, null, null);
        Assert.True(s.Ok);

        var allocatedPartners = db.Query(@"SELECT DISTINCT t.partner_id FROM partner_settlement_items i
            JOIN partner_commission_transactions t ON t.id=i.transaction_id WHERE i.settlement_id=?", s.Id);
        Assert.Single(allocatedPartners);
        Assert.Equal(a, H.L(allocatedPartners[0]["partner_id"]));
        // Partner B is untouched and still owed every penny.
        Assert.Equal(50_000L, PartnerSettlement.PayableMinor(db, b));
    }

    [Fact]
    public void A_statement_for_one_partner_never_shows_another_partners_lines()
    {
        var db = Fresh();
        var (a, b) = TwoPartners(db);

        var sa = PartnerStatement.Build(db, a, "2026-01-01", "2026-12-31");
        var sb = PartnerStatement.Build(db, b, "2026-01-01", "2026-12-31");

        Assert.Single(sa.Lines);
        Assert.Single(sb.Lines);
        Assert.Equal("ISO-A", sa.Lines[0].Code);
        Assert.Equal("ISO-B", sb.Lines[0].Code);
        Assert.Equal(10_000L, sa.EarnedMinor);
        Assert.Equal(50_000L, sb.EarnedMinor);
        // The partner name on the document must be the partner it was built for.
        Assert.Equal("Partner A", sa.PartnerName);
        Assert.Equal("Partner B", sb.PartnerName);
    }

    [Fact]
    public void A_statement_never_carries_the_students_identity()
    {
        var db = Fresh();
        var (a, _) = TwoPartners(db);

        var csv = PartnerStatement.Csv(PartnerStatement.Build(db, a, "2026-01-01", "2026-12-31"));

        // A partner is entitled to know a code was redeemed and what it earned — not who redeemed it.
        Assert.DoesNotContain("iso1@pci.test", csv);
        Assert.DoesNotContain("user_id", csv, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ISO-A", csv);
    }

    [Fact]
    public void A_reversal_against_one_partner_never_moves_another_partners_balance()
    {
        var db = Fresh();
        var (a, b) = TwoPartners(db);
        var payId = db.Scalar<long>("SELECT payment_id FROM partner_commission_transactions WHERE partner_id=? LIMIT 1", a);
        db.Execute("UPDATE payments SET payment_status='refunded', amount_refunded=1000 WHERE id=?", payId);

        Assert.True(PartnerCommissionReversal.EnsureForPayment(db, payId) > 0);

        Assert.Equal(0L, PartnerCommission.Totals(db, a)["commission"]);
        Assert.Equal(50_000L, PartnerCommission.Totals(db, b)["commission"]);
        Assert.Equal(0L, PartnerCommissionReversal.RecoverableMinor(db, b));
    }

    // ── permission posture ────────────────────────────────────────────────────────────────────────

    [Fact]
    public void The_partner_finance_capabilities_are_never_implied_by_a_job_title()
    {
        var pf = Rbac.Sections["partner_finance"];
        Assert.Contains("pf_prepare", pf);
        Assert.Contains("pf_approve", pf);
        Assert.Contains("pf_pay", pf);

        // Owner excepted, no named role bundle may carry a money-moving capability: "who can pay a
        // partner" stays an explicit grant rather than a side-effect of being, say, a website manager.
        foreach (var (role, grants) in Rbac.RoleGrants)
        {
            if (role == "owner") continue;
            foreach (var cap in pf)
                Assert.False(grants.Contains(cap), $"role '{role}' should not imply '{cap}'");
        }
    }

    [Fact]
    public void The_owner_still_holds_every_partner_finance_capability()
    {
        // The owner bypass is why maker-checker is enforced in the settlement engine rather than the gate;
        // this pins that the bypass is real, so that reasoning stays true.
        var owner = Rbac.RoleGrants["owner"];
        foreach (var cap in Rbac.Sections["partner_finance"])
            Assert.Contains(cap, owner);
    }

    [Fact]
    public void Every_partner_finance_capability_is_a_grantable_section()
    {
        // The admin team picker is fed from AllSections; a capability missing there could never be granted
        // to anyone but an owner, which would quietly make the separation of duties unusable.
        foreach (var cap in Rbac.Sections["partner_finance"])
            Assert.Contains(cap, Rbac.AllSections);
    }
}
