using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Partner finance Phase 5 — campaign links and funnel attribution.
///
/// Two things are pinned here. First, a tracked link can never become an <b>open redirect</b>: the link
/// wears PCI's domain, so a destination that could leave the site would turn every partner into a
/// phishing host. Second, the funnel counts <b>real rows</b> — clicks from this module's own table, and
/// everything downstream through the code the link carries, using the same redemption chain the
/// commission ledger runs on. Nothing is modelled, estimated or stitched from a rotating visitor hash.
/// </summary>
public class PartnerCampaignTests
{
    static Db Fresh()
    {
        var db = TestEnv.NewMigratedDb();
        FinanceSchema.Ensure(db);
        return db;
    }

    // ── an open redirect is impossible ────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("https://evil.example/phish")]
    [InlineData("http://evil.example")]
    [InlineData("//evil.example/phish")]          // protocol-relative: the browser keeps the scheme
    [InlineData("/\\evil.example")]               // browsers have historically treated this as //
    [InlineData("\\\\evil.example")]
    [InlineData("javascript://example.com/%0aalert(1)")]
    public void A_destination_that_could_leave_the_site_is_refused(string hostile)
        => Assert.Equal("/", PartnerCampaign.SafeDestination(hostile));

    [Fact]
    public void A_control_character_cannot_be_smuggled_into_a_destination()
    {
        // A CR/LF in a Location header is the classic response-splitting vector.
        Assert.Equal("/", PartnerCampaign.SafeDestination("/certifications\r\nLocation: https://evil.example"));
        Assert.Equal("/", PartnerCampaign.SafeDestination("/tab\there"));
    }

    [Theory]
    [InlineData("/certifications", "/certifications")]
    [InlineData("certifications.html", "/certifications.html")]   // a bare path is made relative, not rejected
    [InlineData("", "/")]
    [InlineData(null, "/")]
    [InlineData("/pricing?plan=pro", "/pricing?plan=pro")]
    public void A_site_relative_destination_is_kept(string? given, string expected)
        => Assert.Equal(expected, PartnerCampaign.SafeDestination(given));

    [Fact]
    public void A_very_long_destination_is_truncated_rather_than_rejected()
        => Assert.Equal(255, PartnerCampaign.SafeDestination("/" + new string('a', 400)).Length);

    // ── the forward URL ───────────────────────────────────────────────────────────────────────────

    [Fact]
    public void The_forward_url_carries_the_code_and_the_partners_utm_values()
    {
        var link = new Dictionary<string, object?>
        {
            ["destination"] = "/certifications",
            ["utm_source"] = "linkedin", ["utm_medium"] = "social", ["utm_campaign"] = "autumn 26",
        };
        var url = PartnerCampaign.ForwardTo(link, "AUTUMN26");
        Assert.StartsWith("/certifications?", url);
        Assert.Contains("code=AUTUMN26", url);
        Assert.Contains("utm_source=linkedin", url);
        Assert.Contains("utm_campaign=autumn%2026", url);       // escaped, so the query cannot be split
    }

    [Fact]
    public void A_destination_that_already_has_a_query_gets_an_ampersand_not_a_second_question_mark()
    {
        var link = new Dictionary<string, object?>
        {
            ["destination"] = "/pricing?plan=pro",
            ["utm_source"] = null, ["utm_medium"] = null, ["utm_campaign"] = null,
        };
        Assert.Equal("/pricing?plan=pro&code=X1", PartnerCampaign.ForwardTo(link, "X1"));
    }

    [Fact]
    public void A_link_with_no_code_and_no_utm_forwards_to_a_clean_destination()
    {
        var link = new Dictionary<string, object?>
        {
            ["destination"] = "/certifications",
            ["utm_source"] = null, ["utm_medium"] = null, ["utm_campaign"] = null,
        };
        Assert.Equal("/certifications", PartnerCampaign.ForwardTo(link, null));
    }

    [Fact]
    public void A_hostile_destination_stored_on_a_link_is_still_neutralised_when_forwarding()
    {
        // Defence in depth: even if a hostile value reached the row, the forward is site-relative.
        var link = new Dictionary<string, object?>
        {
            ["destination"] = "https://evil.example",
            ["utm_source"] = null, ["utm_medium"] = null, ["utm_campaign"] = null,
        };
        Assert.Equal("/?code=A", PartnerCampaign.ForwardTo(link, "A"));
    }

    [Fact]
    public void A_token_is_unguessable_and_url_safe()
    {
        var a = PartnerCampaign.NewToken();
        var b = PartnerCampaign.NewToken();
        Assert.NotEqual(a, b);
        Assert.Equal(12, a.Length);
        Assert.Matches("^[0-9a-f]+$", a);
    }

    // ── the funnel counts real rows ───────────────────────────────────────────────────────────────

    /// <summary>A partner with one code, one clicked link, and two students — one paid, one refunded.</summary>
    static (long partnerId, long codeId, long linkId) Scenario(Db db)
    {
        var pid = db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES('Campaign Partner','registered','active',10,'marketing')");
        var aid = db.ExecuteReturningId(@"INSERT INTO partner_agreements(partner_id,agreement_number,effective_from,status,refund_hold_days)
            VALUES(?,?, '2026-01-01','active',0)", pid, "AGR-CMP-" + pid);
        db.Execute(@"INSERT INTO partner_commission_rules(agreement_id,partner_id,commission_type,commission_rate_bp,commission_basis,priority,active)
            VALUES(?,?, 'percentage',1000,'net_after_discount',100,1)", aid, pid);
        var codeId = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id)
            VALUES('CMP-1','percentage',10,'exam',1,'active','campaign',?)", pid);
        var linkId = db.ExecuteReturningId(@"INSERT INTO partner_campaign_links(token,partner_id,code_id,name,destination)
            VALUES('abc123def456',?,?, 'Autumn LinkedIn','/certifications')", pid, codeId);

        for (var i = 1; i <= 2; i++)
        {
            var uid = db.ExecuteReturningId(
                "INSERT INTO users(email,first_name,last_name,role,status) VALUES(?, 'C','S','student','active')", $"cmp{i}@pci.test");
            var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,amount_refunded)
                VALUES(?, 'exam',1000,900,'USD','test',?, '2026-05-10 10:00:00',?)", uid, i == 1 ? "paid" : "refunded", i == 1 ? 0 : 900);
            db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount,redeemed_at)
                VALUES(?, 'CMP-1',?,?, 'exam',1000,100,'2026-05-10 10:00:00')", codeId, uid, payId);
            if (i == 1) PartnerCommission.EnsureForPayment(db, payId);
        }
        return (pid, codeId, linkId);
    }

    [Fact]
    public void The_funnel_counts_redemptions_students_revenue_and_commission_from_real_rows()
    {
        var db = Fresh();
        var (_, codeId, _) = Scenario(db);

        var f = PartnerCampaign.FunnelForCode(db, codeId, null, null);

        Assert.Equal(2L, f["redemptions"]);
        Assert.Equal(2L, f["students"]);
        Assert.Equal(2L, f["payments"]);
        Assert.Equal(1L, f["payments_paid"]);            // the refunded one is counted, but not as paid
        Assert.Equal(900.0, f["net_revenue"]);           // only the paid sale contributes revenue
        Assert.Equal(1000.0, f["gross_revenue"]);
        Assert.Equal(200.0, f["discount_given"]);        // both redemptions gave 100.00 away
        Assert.Equal(900.0, f["refunded"]);
        Assert.Equal(90.00m, f["commission"]);           // 10% of the one settled 900.00 sale
    }

    [Fact]
    public void The_date_window_is_inclusive_of_its_closing_day()
    {
        var db = Fresh();
        var (_, codeId, _) = Scenario(db);       // both redemptions dated 2026-05-10 10:00:00

        // An exclusive upper bound would drop the whole day and report a partner's campaign as dead.
        Assert.Equal(2L, PartnerCampaign.FunnelForCode(db, codeId, "2026-05-10", "2026-05-10")["redemptions"]);
        Assert.Equal(0L, PartnerCampaign.FunnelForCode(db, codeId, "2026-05-11", "2026-05-31")["redemptions"]);
        Assert.Equal(0L, PartnerCampaign.FunnelForCode(db, codeId, "2026-01-01", "2026-05-09")["redemptions"]);
    }

    [Fact]
    public void Clicks_are_counted_and_deduplicated_within_a_day()
    {
        var db = Fresh();
        var (_, _, linkId) = Scenario(db);

        PartnerCampaign.RecordClick(db, linkId, "visitor-a", "PK", "desktop", "chrome", null);
        PartnerCampaign.RecordClick(db, linkId, "visitor-a", "PK", "desktop", "chrome", null);
        PartnerCampaign.RecordClick(db, linkId, "visitor-b", "AE", "mobile", "safari", "https://linkedin.com/");

        var (clicks, uniques) = PartnerCampaign.Clicks(db, linkId, null, null);
        Assert.Equal(3L, clicks);
        Assert.Equal(2L, uniques);
        Assert.Equal(3L, db.Scalar<long>("SELECT click_count FROM partner_campaign_links WHERE id=?", linkId));
    }

    [Fact]
    public void A_report_pairs_clicks_with_the_conversions_the_code_produced()
    {
        var db = Fresh();
        var (_, _, linkId) = Scenario(db);
        for (var i = 0; i < 8; i++) PartnerCampaign.RecordClick(db, linkId, "v" + i, "PK", "desktop", "chrome", null);

        var link = db.QueryOne("SELECT * FROM partner_campaign_links WHERE id=?", linkId)!;
        var r = PartnerCampaign.Report(db, link, null, null);

        Assert.True((bool)r["tracks_conversions"]!);
        Assert.Equal(8L, r["clicks"]);
        Assert.Equal(2L, r["redemptions"]);
        Assert.Equal(25.0, r["conversion_pct"]);         // 2 of 8
    }

    [Fact]
    public void A_link_with_no_clicks_reports_no_conversion_rate_rather_than_zero_percent()
    {
        var db = Fresh();
        var (_, _, linkId) = Scenario(db);
        var link = db.QueryOne("SELECT * FROM partner_campaign_links WHERE id=?", linkId)!;

        var r = PartnerCampaign.Report(db, link, null, null);

        Assert.Equal(0L, r["clicks"]);
        // "Nobody arrived" and "nobody converted" are different facts, and 0% would assert the second.
        Assert.Null(r["conversion_pct"]);
    }

    [Fact]
    public void A_link_carrying_no_code_reports_clicks_and_says_it_tracks_nothing_further()
    {
        var db = Fresh();
        var pid = db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES('Brand Partner','registered','active',10,'marketing')");
        var linkId = db.ExecuteReturningId(@"INSERT INTO partner_campaign_links(token,partner_id,name,destination)
            VALUES('brandtoken01',?, 'Brand awareness','/about')", pid);
        PartnerCampaign.RecordClick(db, linkId, "v1", "PK", "desktop", "chrome", null);

        var link = db.QueryOne("SELECT * FROM partner_campaign_links WHERE id=?", linkId)!;
        var r = PartnerCampaign.Report(db, link, null, null);

        Assert.Equal(1L, r["clicks"]);
        Assert.False((bool)r["tracks_conversions"]!);
        // Reporting zeroes here would read as "the campaign failed" rather than "this link measures reach".
        Assert.False(r.ContainsKey("redemptions"));
        Assert.False(r.ContainsKey("commission"));
    }

    [Fact]
    public void One_partners_funnel_never_includes_another_partners_code()
    {
        var db = Fresh();
        var (_, codeId, _) = Scenario(db);

        var other = db.ExecuteReturningId(
            "INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) VALUES('Other','registered','active',10,'marketing')");
        var otherCode = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id)
            VALUES('OTHER-1','percentage',10,'exam',1,'active','campaign',?)", other);
        var uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status) VALUES('other@pci.test','O','S','student','active')");
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,final_amount,currency,payment_provider,payment_status,payment_date,amount_refunded)
            VALUES(?, 'exam',900,'USD','test','paid','2026-05-10 10:00:00',0)", uid);
        db.Execute(@"INSERT INTO code_redemptions(code_id,code,user_id,payment_id,product_type,amount_before,discount_amount,redeemed_at)
            VALUES(?, 'OTHER-1',?,?, 'exam',1000,100,'2026-05-10 10:00:00')", otherCode, uid, payId);

        Assert.Equal(2L, PartnerCampaign.FunnelForCode(db, codeId, null, null)["redemptions"]);
        Assert.Equal(1L, PartnerCampaign.FunnelForCode(db, otherCode, null, null)["redemptions"]);
    }

    [Fact]
    public void A_tracking_failure_never_propagates_to_the_visitor()
    {
        var db = Fresh();
        // RecordClick swallows everything by contract: the visitor must still reach the page even when
        // tracking cannot be written. Removing the table is the bluntest way to prove the contract holds.
        db.Execute("DROP TABLE partner_link_clicks");
        Assert.Null(Record.Exception(() => PartnerCampaign.RecordClick(db, 1, "v1", "PK", "desktop", "chrome", null)));
    }
}
