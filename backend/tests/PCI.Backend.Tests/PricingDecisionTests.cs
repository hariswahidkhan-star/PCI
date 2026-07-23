using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-1 / BD-2 — direct unit coverage of the money-decision core the live-HTTP suite only exercises
/// through the happy path: <see cref="Public.Pricing"/> (discount arithmetic at the boundaries),
/// <see cref="Public.MinPayableViolation"/>, and the full <see cref="Public.ValidateCode"/> rejection
/// matrix. Each test runs against a fresh migrated SQLite database (TestEnv.NewMigratedDb) — the same
/// schema CI migrates — and seeds only the rows the decision reads. Nothing here changes application
/// code; it pins the behaviour that is already shipping.
/// </summary>
public class PricingDecisionTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static void SeedRule(Db db, string product, double std, double discPct, int active = 1)
    {
        // Migration seeds a default rule per product_type and Pricing reads the first match, so replace
        // that product's row rather than adding a competing second one — this keeps each test's arithmetic
        // deterministic and reflects production (one active rule per product_type).
        db.Execute("DELETE FROM pricing_rules WHERE product_type=?", product);
        db.Execute("INSERT INTO pricing_rules(product_type,standard_price,default_discount_percentage,active) VALUES(?,?,?,?)",
                   product, std, discPct, active);
    }

    static Dictionary<string, object?> Code(string discountType, double value, string appliesTo = "all",
                                            double? minTransaction = null, double? maxDiscount = null)
        => new()
        {
            ["discount_type"] = discountType,
            ["discount_value"] = value,
            ["applies_to"] = appliesTo,
            ["min_transaction"] = minTransaction,
            ["max_discount"] = maxDiscount,
        };

    // ---- Pricing: default-discount propagation and item shape --------------------------------------

    [Fact]
    public void Pricing_Membership_AppliesDefaultDiscountPercentage()
    {
        var db = Fresh();
        SeedRule(db, "membership", 300, 10);           // 10% off => payable 270, discount 30
        var r = Public.Pricing(db, "membership", null);

        Assert.Equal(300, r.standard, 3);
        Assert.Equal(30, r.defaultDiscount, 3);
        Assert.Equal(0, r.codeAmount, 3);
        Assert.Equal(270, r.final, 3);
        var item = Assert.Single(r.items);
        Assert.Equal("membership", item.cat);
        Assert.Equal(270, item.payable, 3);
    }

    [Fact]
    public void Pricing_Bundle_SumsMembershipAndExam()
    {
        var db = Fresh();
        SeedRule(db, "membership", 300, 0);
        SeedRule(db, "exam", 500, 0);
        var r = Public.Pricing(db, "bundle", null);

        Assert.Equal(2, r.items.Count);
        Assert.Equal(800, r.standard, 3);
        Assert.Equal(800, r.final, 3);
    }

    [Fact]
    public void Pricing_InactiveRule_IsSkipped()
    {
        var db = Fresh();
        SeedRule(db, "membership", 300, 0, active: 0);
        var r = Public.Pricing(db, "membership", null);
        Assert.Empty(r.items);
        Assert.Equal(0, r.final, 3);
    }

    [Fact]
    public void Pricing_ExamPrice_OverriddenByCertificationSpecificPrice()
    {
        var db = Fresh();
        SeedRule(db, "exam", 500, 0);
        var cert = new Dictionary<string, object?> { ["exam_price"] = 725.0 };
        var r = Public.Pricing(db, "exam", null, cert);
        Assert.Equal(725, r.standard, 3);       // the cert's own price wins over the generic exam rule
        Assert.Equal(725, r.final, 3);
    }

    [Fact]
    public void Pricing_ExamPrice_NullCertPriceKeepsGenericRule()
    {
        var db = Fresh();
        SeedRule(db, "exam", 500, 0);
        var cert = new Dictionary<string, object?> { ["exam_price"] = null };
        var r = Public.Pricing(db, "exam", null, cert);
        Assert.Equal(500, r.standard, 3);
    }

    // ---- Pricing: code arithmetic boundaries ------------------------------------------------------

    [Fact]
    public void Pricing_PercentageCode_ReducesFinal()
    {
        var db = Fresh();
        SeedRule(db, "membership", 200, 0);
        var r = Public.Pricing(db, "membership", Code("percentage", 25));  // 25% of 200 = 50
        Assert.Equal(50, r.codeAmount, 3);
        Assert.Equal(150, r.final, 3);
    }

    [Fact]
    public void Pricing_FixedCode_CappedAtBaseAmount()
    {
        var db = Fresh();
        SeedRule(db, "membership", 80, 0);
        // A fixed code larger than the payable cannot make the code amount exceed the base.
        var r = Public.Pricing(db, "membership", Code("fixed", 250));
        Assert.Equal(80, r.codeAmount, 3);
        Assert.Equal(0, r.final, 3);
    }

    [Fact]
    public void Pricing_MaxDiscount_CapsTheCodeAmount()
    {
        var db = Fresh();
        SeedRule(db, "membership", 400, 0);
        // 50% of 400 = 200, but max_discount caps it at 120.
        var r = Public.Pricing(db, "membership", Code("percentage", 50, maxDiscount: 120));
        Assert.Equal(120, r.codeAmount, 3);
        Assert.Equal(280, r.final, 3);
    }

    [Fact]
    public void Pricing_MinTransaction_BelowThreshold_CodeDoesNotApply()
    {
        var db = Fresh();
        SeedRule(db, "membership", 50, 0);
        // Base 50 is below the 100 minimum → the code contributes nothing.
        var r = Public.Pricing(db, "membership", Code("percentage", 50, minTransaction: 100));
        Assert.Equal(0, r.codeAmount, 3);
        Assert.Equal(50, r.final, 3);
    }

    [Fact]
    public void Pricing_MinTransaction_AtThreshold_CodeApplies()
    {
        var db = Fresh();
        SeedRule(db, "membership", 100, 0);
        // Base 100 is NOT below the 100 minimum (the guard is `< min`), so the code applies.
        var r = Public.Pricing(db, "membership", Code("percentage", 10, minTransaction: 100));
        Assert.Equal(10, r.codeAmount, 3);
        Assert.Equal(90, r.final, 3);
    }

    [Fact]
    public void Pricing_AppliesTo_FiltersToTheNamedCategory()
    {
        var db = Fresh();
        SeedRule(db, "membership", 300, 0);
        SeedRule(db, "exam", 500, 0);
        // A code scoped to "exam" only discounts the exam leg of a bundle.
        var r = Public.Pricing(db, "bundle", Code("percentage", 10, appliesTo: "exam"));
        Assert.Equal(50, r.codeAmount, 3);   // 10% of the 500 exam leg only
        Assert.Equal(750, r.final, 3);
    }

    [Fact]
    public void Pricing_Rounding_UsesBankersRoundingToTheCent()
    {
        var db = Fresh();
        // 47.50 @ 5% = 2.375 -> *100 = 237.5 -> ToEven -> 238 -> 2.38
        SeedRule(db, "membership", 47.50, 0);
        Assert.Equal(2.38, Public.Pricing(db, "membership", Code("percentage", 5)).codeAmount, 3);

        var db2 = Fresh();
        // 42.50 @ 5% = 2.125 -> *100 = 212.5 -> ToEven -> 212 -> 2.12
        SeedRule(db2, "membership", 42.50, 0);
        Assert.Equal(2.12, Public.Pricing(db2, "membership", Code("percentage", 5)).codeAmount, 3);
    }

    [Fact]
    public void Pricing_Final_FlooredAtZero()
    {
        var db = Fresh();
        SeedRule(db, "membership", 100, 50);   // default discount already halves it to 50
        // A fixed code of 999 would over-subtract; final is clamped to 0, never negative.
        var r = Public.Pricing(db, "membership", Code("fixed", 999));
        Assert.Equal(0, r.final, 3);
        Assert.True(r.final >= 0);
    }

    // ---- MinPayableViolation ----------------------------------------------------------------------

    [Fact]
    public void MinPayable_NullFloor_NeverViolates()
    {
        var code = new Dictionary<string, object?> { ["min_payable"] = null };
        Assert.Null(Public.MinPayableViolation(code, 0));
    }

    [Fact]
    public void MinPayable_BelowFloor_ReportsViolation()
    {
        var code = new Dictionary<string, object?> { ["min_payable"] = 50.0 };
        Assert.NotNull(Public.MinPayableViolation(code, 49.99));
        Assert.Null(Public.MinPayableViolation(code, 50.0));     // equal is allowed (guard is `< min`)
        Assert.Null(Public.MinPayableViolation(code, 60.0));
    }

    // ---- ValidateCode: existence, lifecycle status, active flag -----------------------------------

    static void SeedCode(Db db, string code)
        // Insert with sensible active defaults; individual tests override specific columns via UPDATE.
        => db.Execute("INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,used_count) VALUES(?,?,?,?,1,0)",
                      code, "percentage", 10.0, "all");

    [Fact]
    public void ValidateCode_UnknownCode_Rejected()
    {
        var db = Fresh();
        var v = Public.ValidateCode(db, "NOPE", "membership", null);
        Assert.NotNull(v.Error);
        Assert.Null(v.Code);
    }

    [Fact]
    public void ValidateCode_IsCaseInsensitiveOnLookup()
    {
        var db = Fresh();
        SeedCode(db, "CASEI25");
        var v = Public.ValidateCode(db, "casei25", "membership", null);   // lower-case input
        Assert.Null(v.Error);
        Assert.NotNull(v.Code);
    }

    [Theory]
    [InlineData("draft")]
    [InlineData("pending_approval")]
    [InlineData("suspended")]
    [InlineData("rejected")]
    [InlineData("cancelled")]
    public void ValidateCode_NonLiveStatus_Rejected(string status)
    {
        var db = Fresh();
        SeedCode(db, "S1");
        db.Execute("UPDATE discount_codes SET status=? WHERE code='S1'", status);
        var v = Public.ValidateCode(db, "S1", "membership", null);
        Assert.NotNull(v.Error);
        Assert.Null(v.Code);
    }

    [Fact]
    public void ValidateCode_InactiveFlag_Rejected()
    {
        var db = Fresh();
        SeedCode(db, "OFF");
        db.Execute("UPDATE discount_codes SET active=0 WHERE code='OFF'");
        Assert.NotNull(Public.ValidateCode(db, "OFF", "membership", null).Error);
    }

    // ---- ValidateCode: scope, founding, dates -----------------------------------------------------

    [Fact]
    public void ValidateCode_CertificationScopeMismatch_Rejected()
    {
        var db = Fresh();
        SeedCode(db, "CERTONLY");
        db.Execute("UPDATE discount_codes SET certification_id=7 WHERE code='CERTONLY'");
        Assert.NotNull(Public.ValidateCode(db, "CERTONLY", "membership", null, certId: 9).Error);   // mismatch
        Assert.Null(Public.ValidateCode(db, "CERTONLY", "membership", null, certId: 7).Error);        // match
    }

    [Fact]
    public void ValidateCode_FoundingCode_RejectedInDiscountField()
    {
        var db = Fresh();
        SeedCode(db, "FOUND1");
        db.Execute("UPDATE discount_codes SET founding_route='founding' WHERE code='FOUND1'");
        var v = Public.ValidateCode(db, "FOUND1", "membership", null);
        Assert.NotNull(v.Error);
        Assert.Contains("founding", v.Error!, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ValidateCode_ExpiredEndDate_Rejected()
    {
        var db = Fresh();
        SeedCode(db, "OLD");
        db.Execute("UPDATE discount_codes SET end_date='2000-01-01' WHERE code='OLD'");
        Assert.NotNull(Public.ValidateCode(db, "OLD", "membership", null).Error);
    }

    [Fact]
    public void ValidateCode_NotYetActiveStartDate_Rejected()
    {
        var db = Fresh();
        SeedCode(db, "FUTURE");
        db.Execute("UPDATE discount_codes SET start_date='2999-01-01' WHERE code='FUTURE'");
        Assert.NotNull(Public.ValidateCode(db, "FUTURE", "membership", null).Error);
    }

    // ---- ValidateCode: usage caps -----------------------------------------------------------------

    [Fact]
    public void ValidateCode_MaxUses_BoundaryIsInclusive()
    {
        var db = Fresh();
        SeedCode(db, "CAP");
        db.Execute("UPDATE discount_codes SET max_uses=3, used_count=2 WHERE code='CAP'");
        Assert.Null(Public.ValidateCode(db, "CAP", "membership", null).Error);       // 2 < 3 → ok
        db.Execute("UPDATE discount_codes SET used_count=3 WHERE code='CAP'");
        Assert.NotNull(Public.ValidateCode(db, "CAP", "membership", null).Error);     // 3 >= 3 → capped
    }

    [Fact]
    public void ValidateCode_PerUserLimit_CountsRedemptionsForThatEmail()
    {
        var db = Fresh();
        SeedCode(db, "ONEEACH");
        var id = db.Scalar<long>("SELECT id FROM discount_codes WHERE code='ONEEACH'");
        db.Execute("UPDATE discount_codes SET per_user_limit=1 WHERE code='ONEEACH'");
        // No prior redemption for this email → valid.
        Assert.Null(Public.ValidateCode(db, "ONEEACH", "membership", "a@x.io").Error);
        // Record one redemption, then the same email is refused.
        db.Execute("INSERT INTO code_redemptions(code_id,email) VALUES(?,?)", id, "a@x.io");
        Assert.NotNull(Public.ValidateCode(db, "ONEEACH", "membership", "a@x.io").Error);
        // A different email still has headroom.
        Assert.Null(Public.ValidateCode(db, "ONEEACH", "membership", "b@x.io").Error);
    }

    // ---- ValidateCode: applies_to vs product ------------------------------------------------------

    [Fact]
    public void ValidateCode_AppliesToMismatch_Rejected()
    {
        var db = Fresh();
        SeedCode(db, "EXAMONLY");
        db.Execute("UPDATE discount_codes SET applies_to='exam' WHERE code='EXAMONLY'");
        Assert.NotNull(Public.ValidateCode(db, "EXAMONLY", "membership", null).Error);   // wrong product
        Assert.Null(Public.ValidateCode(db, "EXAMONLY", "exam", null).Error);            // right product
        Assert.Null(Public.ValidateCode(db, "EXAMONLY", "bundle", null).Error);          // bundle contains exam
    }

    // ---- ValidateCode: waiver email-lock ----------------------------------------------------------

    [Fact]
    public void ValidateCode_WaiverEmailLock_OnlyForTheNamedStudent()
    {
        var db = Fresh();
        SeedCode(db, "WAIVE");
        db.Execute("UPDATE discount_codes SET code_type='waiver', criteria_json='{\"email\":\"owner@x.io\"}' WHERE code='WAIVE'");
        Assert.NotNull(Public.ValidateCode(db, "WAIVE", "membership", "someone@else.io").Error);
        Assert.Null(Public.ValidateCode(db, "WAIVE", "membership", "owner@x.io").Error);
        Assert.Null(Public.ValidateCode(db, "WAIVE", "membership", "OWNER@x.io").Error);   // case-insensitive
    }

    // ---- ValidateCode: partner allocation + agreement window --------------------------------------

    static long SeedPartner(Db db, string status, string? agreementEnd, long? totalAllocation)
    {
        db.Execute("INSERT INTO training_partners(name,status,agreement_end,total_allocation) VALUES(?,?,?,?)",
                   "Acme Institute", status, agreementEnd, totalAllocation);
        return db.Scalar<long>("SELECT id FROM training_partners WHERE name='Acme Institute'");
    }

    [Fact]
    public void ValidateCode_PartnerInactive_Rejected()
    {
        var db = Fresh();
        var pid = SeedPartner(db, "suspended", null, null);
        SeedCode(db, "PART1");
        db.Execute("UPDATE discount_codes SET partner_id=? WHERE code='PART1'", pid);
        Assert.NotNull(Public.ValidateCode(db, "PART1", "membership", null).Error);
    }

    [Fact]
    public void ValidateCode_PartnerAllocationSpent_Rejected()
    {
        var db = Fresh();
        var pid = SeedPartner(db, "active", null, totalAllocation: 5);
        SeedCode(db, "PART2");
        // This partner's codes have already consumed the whole allocation.
        db.Execute("UPDATE discount_codes SET partner_id=?, used_count=5 WHERE code='PART2'", pid);
        Assert.NotNull(Public.ValidateCode(db, "PART2", "membership", null).Error);
    }

    [Fact]
    public void ValidateCode_PartnerWithinAllocation_Valid()
    {
        var db = Fresh();
        var pid = SeedPartner(db, "active", agreementEnd: "2999-01-01", totalAllocation: 100);
        SeedCode(db, "PART3");
        db.Execute("UPDATE discount_codes SET partner_id=?, used_count=1 WHERE code='PART3'", pid);
        Assert.Null(Public.ValidateCode(db, "PART3", "membership", null).Error);
    }

    [Fact]
    public void ValidateCode_PartnerAgreementEnded_Rejected()
    {
        var db = Fresh();
        var pid = SeedPartner(db, "active", agreementEnd: "2000-01-01", totalAllocation: null);
        SeedCode(db, "PART4");
        db.Execute("UPDATE discount_codes SET partner_id=? WHERE code='PART4'", pid);
        Assert.NotNull(Public.ValidateCode(db, "PART4", "membership", null).Error);
    }

    // ---- ValidateCode: happy path -----------------------------------------------------------------

    [Fact]
    public void ValidateCode_PlainValidCode_ReturnsTheRow()
    {
        var db = Fresh();
        SeedCode(db, "GOOD");
        var v = Public.ValidateCode(db, "GOOD", "membership", null);
        Assert.Null(v.Error);
        Assert.NotNull(v.Code);
        Assert.Equal("GOOD", v.Code!["code"]?.ToString());
    }
}
