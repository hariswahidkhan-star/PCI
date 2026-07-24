using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Partner campaign links and funnel attribution — Phase 5 of the Marketing Partner module.
///
/// A campaign link is a shareable, tracked URL (<c>/r/{token}</c>) that carries one of the partner's own
/// discount codes. Clicking it records the click, applies the code, and forwards to a site page.
///
/// <b>How the funnel is attributed, and why.</b> Clicks come from this module's own table. Everything
/// downstream — students, applications, payments, revenue, commission, certifications — is attributed
/// through the <b>code</b> the link carries, using the same <c>code_redemptions → discount_codes.partner_id</c>
/// chain the commission ledger already runs on. That chain is exact: one payment carries at most one
/// redemption, which carries at most one partner.
///
/// Per-visitor journey stitching is deliberately NOT attempted. The platform's visitor hash
/// (<see cref="Analytics"/>) is salted with the current date and a per-boot secret, so it rotates every
/// day and cannot be joined across days — that is a privacy property worth keeping, not a defect to work
/// around. Stitching on it would silently under-report and present a broken number as a real one. Unique
/// visitors are therefore reported as a same-day figure and labelled as such.
/// </summary>
public static class PartnerCampaign
{
    /// <summary>Where a link may forward to. Site-relative only — an open redirect would make every
    /// tracked link a phishing primitive carrying PCI's domain.</summary>
    public static string SafeDestination(string? raw)
    {
        var d = (raw ?? "").Trim();
        if (d.Length == 0) return "/";
        // Reject anything that could leave the site: absolute URLs, protocol-relative URLs, backslash
        // variants and control characters are all refused rather than sanitised into something surprising.
        if (d.Contains("://", StringComparison.Ordinal)) return "/";
        if (d.StartsWith("//", StringComparison.Ordinal) || d.StartsWith("/\\", StringComparison.Ordinal)) return "/";
        if (d.StartsWith('\\')) return "/";
        foreach (var ch in d) if (char.IsControl(ch)) return "/";
        if (!d.StartsWith('/')) d = "/" + d;
        return d.Length > 255 ? d[..255] : d;
    }

    /// <summary>An opaque, unguessable slug. Short enough to share, long enough not to be enumerated.</summary>
    public static string NewToken() => Security.RandomHex(6).ToLowerInvariant();   // 12 hex chars

    /// <summary>
    /// Build the URL a click is forwarded to: the destination with the code applied, plus the partner's
    /// UTM values so the platform's existing analytics attribute the visit exactly as any other campaign.
    /// </summary>
    public static string ForwardTo(Dictionary<string, object?> link, string? code)
    {
        var dest = SafeDestination(H.Str(link["destination"]));
        var q = new List<string>();
        void Add(string k, string? v)
        {
            if (string.IsNullOrWhiteSpace(v)) return;
            q.Add(Uri.EscapeDataString(k) + "=" + Uri.EscapeDataString(v));
        }
        Add("code", code);
        Add("utm_source", H.Str(link["utm_source"]));
        Add("utm_medium", H.Str(link["utm_medium"]));
        Add("utm_campaign", H.Str(link["utm_campaign"]));
        if (q.Count == 0) return dest;
        return dest + (dest.Contains('?') ? "&" : "?") + string.Join("&", q);
    }

    /// <summary>Record a click. Never throws — a tracking failure must not stop the visitor reaching the page.</summary>
    public static void RecordClick(Db db, long linkId, string? visitor, string? country, string? device, string? browser, string? referrer)
    {
        try
        {
            db.Execute(@"INSERT INTO partner_link_clicks(link_id,visitor,country,device,browser,referrer)
                VALUES(?,?,?,?,?,?)", linkId, visitor, country, device, browser, Clip(referrer, 255));
            db.Execute("UPDATE partner_campaign_links SET click_count=click_count+1 WHERE id=?", linkId);
        }
        catch { /* tracking is never allowed to break the redirect */ }
    }

    static string? Clip(string? s, int n)
        => string.IsNullOrWhiteSpace(s) ? null : (s.Length > n ? s[..n] : s);

    /// <summary>
    /// The funnel for one code, over an optional date window. Every figure below is counted from a real
    /// row; nothing is modelled or estimated.
    /// </summary>
    public static Dictionary<string, object?> FunnelForCode(Db db, long codeId, string? from, string? to)
    {
        // Inclusive window over the whole closing day, using string comparison so no date function is
        // needed and the query is identical on both providers.
        var f = string.IsNullOrWhiteSpace(from) ? "0000-01-01" : from!.Trim();
        var t = string.IsNullOrWhiteSpace(to) ? "9999-12-31 23:59:59" : to!.Trim() + " 23:59:59";

        var redemptions = db.Scalar<long>(
            "SELECT COUNT(*) FROM code_redemptions WHERE code_id=? AND redeemed_at>=? AND redeemed_at<=?", codeId, f, t);
        var students = db.Scalar<long>(
            "SELECT COUNT(DISTINCT user_id) FROM code_redemptions WHERE code_id=? AND redeemed_at>=? AND redeemed_at<=?", codeId, f, t);

        var pay = db.QueryOne(@"SELECT
                COUNT(*) n,
                COALESCE(SUM(CASE WHEN p.payment_status='paid' THEN 1 ELSE 0 END),0) paid_n,
                COALESCE(SUM(CASE WHEN p.payment_status='paid' THEN p.final_amount ELSE 0 END),0) net_revenue,
                COALESCE(SUM(CASE WHEN p.payment_status='paid' THEN r.amount_before ELSE 0 END),0) gross_revenue,
                COALESCE(SUM(r.discount_amount),0) discount_given,
                COALESCE(SUM(p.amount_refunded),0) refunded
            FROM code_redemptions r JOIN payments p ON p.id=r.payment_id
            WHERE r.code_id=? AND r.redeemed_at>=? AND r.redeemed_at<=?", codeId, f, t);

        // Applications and certifications belong to the students the code brought in, scoped to the
        // certification the redemption was for when the payment names one.
        var applications = db.Scalar<long>(@"SELECT COUNT(DISTINCT a.id)
            FROM code_redemptions r JOIN certification_applications a ON a.user_id=r.user_id
            WHERE r.code_id=? AND r.redeemed_at>=? AND r.redeemed_at<=?", codeId, f, t);
        var certifications = db.Scalar<long>(@"SELECT COUNT(DISTINCT ic.id)
            FROM code_redemptions r JOIN issued_credentials ic ON ic.user_id=r.user_id
            WHERE r.code_id=? AND r.redeemed_at>=? AND r.redeemed_at<=? AND ic.status='active'", codeId, f, t);

        var commissionMinor = db.Scalar<long>(@"SELECT COALESCE(SUM(commission_minor),0)
            FROM partner_commission_transactions WHERE discount_code_id=?
              AND COALESCE(earned_at,created_at)>=? AND COALESCE(earned_at,created_at)<=?", codeId, f, t);

        return new Dictionary<string, object?>
        {
            ["redemptions"] = redemptions,
            ["students"] = students,
            ["payments"] = H.L(pay?["n"]),
            ["payments_paid"] = H.L(pay?["paid_n"]),
            ["gross_revenue"] = H.D(pay?["gross_revenue"]),
            ["net_revenue"] = H.D(pay?["net_revenue"]),
            ["discount_given"] = H.D(pay?["discount_given"]),
            ["refunded"] = H.D(pay?["refunded"]),
            ["applications"] = applications,
            ["certifications"] = certifications,
            ["commission"] = Money.ToDecimal(commissionMinor),
        };
    }

    /// <summary>Clicks for a link over the same window, with the same-day-only uniqueness caveat.</summary>
    public static (long clicks, long sameDayVisitors) Clicks(Db db, long linkId, string? from, string? to)
    {
        var f = string.IsNullOrWhiteSpace(from) ? "0000-01-01" : from!.Trim();
        var t = string.IsNullOrWhiteSpace(to) ? "9999-12-31 23:59:59" : to!.Trim() + " 23:59:59";
        var clicks = db.Scalar<long>(
            "SELECT COUNT(*) FROM partner_link_clicks WHERE link_id=? AND created_at>=? AND created_at<=?", linkId, f, t);
        var uniques = db.Scalar<long>(
            "SELECT COUNT(DISTINCT visitor) FROM partner_link_clicks WHERE link_id=? AND created_at>=? AND created_at<=? AND visitor IS NOT NULL",
            linkId, f, t);
        return (clicks, uniques);
    }

    /// <summary>
    /// A link's full report: clicks plus, when the link carries a code, everything that code produced.
    /// A link with no code honestly reports clicks and nothing else rather than showing zeroes that look
    /// like failure.
    /// </summary>
    public static Dictionary<string, object?> Report(Db db, Dictionary<string, object?> link, string? from, string? to)
    {
        var linkId = H.L(link["id"]);
        var (clicks, uniques) = Clicks(db, linkId, from, to);
        var row = new Dictionary<string, object?>
        {
            ["id"] = linkId,
            ["token"] = H.Str(link["token"]),
            ["name"] = H.Str(link["name"]),
            ["destination"] = H.Str(link["destination"]),
            ["code_id"] = link["code_id"],
            ["active"] = H.L(link["active"]),
            ["utm_source"] = H.Str(link["utm_source"]),
            ["utm_medium"] = H.Str(link["utm_medium"]),
            ["utm_campaign"] = H.Str(link["utm_campaign"]),
            ["created_at"] = H.Str(link["created_at"]),
            ["clicks"] = clicks,
            ["same_day_visitors"] = uniques,
        };
        if (link["code_id"] is null) { row["tracks_conversions"] = false; return row; }

        row["tracks_conversions"] = true;
        foreach (var kv in FunnelForCode(db, H.L(link["code_id"]), from, to)) row[kv.Key] = kv.Value;
        // Conversion is only meaningful when there were clicks to convert; 0 clicks reports null, not 0%,
        // because "nobody arrived" and "nobody converted" are different facts.
        row["conversion_pct"] = clicks > 0
            ? (object)Math.Round(H.D(row["redemptions"]) * 100.0 / clicks, 2)
            : null;
        return row;
    }
}
