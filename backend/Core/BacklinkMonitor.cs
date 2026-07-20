using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Backlink monitoring (Phase 5) — the ONLY automated action in the Backlink &amp; Outreach CRM. On operator
/// demand it fetches a SINGLE recorded source page through the SSRF-guarded egress client and decides
/// whether the backlink to PCI still exists. This is verification of a URL the operator already recorded,
/// not web scraping or automated discovery: discovery stays manual / CSV / referral. Under the egress guard
/// redirects are not followed (a public page 30x-ing to an internal one is the classic SSRF bypass), so a
/// 3xx source is reported as "redirected" rather than chased.
/// </summary>
public static class BacklinkMonitor
{
    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(20));

    /// <summary>Stable dedup key for a (source → target) pair, case/whitespace-insensitive.</summary>
    public static string LinkHash(string? source, string? target) =>
        Security.Sha((source ?? "").Trim().ToLowerInvariant() + "|" + (target ?? "").Trim().ToLowerInvariant());

    /// <summary>Our own site's domain, operator-tunable so a verifier still matches a link even when the
    /// recorded target_url is a slightly different PCI path.</summary>
    public static string OurDomain(Db db) => Settings.Str(db, "backlink_our_domain", "projectcontrolsinstitute.org").Trim().ToLowerInvariant();

    /// <summary>Fetch the recorded source page and classify the backlink.
    /// status ∈ live | lost | redirected | removed | unreachable. Persists last_checked_at / status /
    /// last_status_code and stamps first_seen_at the first time a link is confirmed live.</summary>
    public static async Task<(string status, int code, string? err)> VerifyLink(Db db, long id)
    {
        var row = db.QueryOne("SELECT * FROM cc_backlinks WHERE id=?", id);
        if (row is null) return ("unreachable", 0, "not_found");
        var source = H.Str(row["source_url"]) ?? "";
        var target = H.Str(row["target_url"]) ?? "";

        var problem = Egress.UrlProblem(source);
        if (problem is not null) { Record(db, id, "unreachable", 0); return ("unreachable", 0, problem); }

        string status; int code = 0;
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, source);
            req.Headers.TryAddWithoutValidation("User-Agent", "PCI-BacklinkMonitor/1.0 (+https://projectcontrolsinstitute.org)");
            using var resp = await Http.SendAsync(req);
            code = (int)resp.StatusCode;
            if (code is >= 300 and < 400) status = "redirected";
            else if (code >= 400) status = "removed";
            else
            {
                var html = await resp.Content.ReadAsStringAsync();
                status = LinkPresent(html, target, OurDomain(db)) ? "live" : "lost";
            }
        }
        catch (Exception e) { Record(db, id, "unreachable", code); return ("unreachable", code, e.Message); }

        Record(db, id, status, code);
        return (status, code, null);
    }

    static void Record(Db db, long id, string status, int code)
    {
        // Stamp first_seen_at only on the first confirmed-live check; never clear it once set.
        db.Execute(@"UPDATE cc_backlinks SET status=?, last_checked_at=datetime('now'), last_status_code=?,
            first_seen_at=CASE WHEN first_seen_at IS NULL AND ?='live' THEN datetime('now') ELSE first_seen_at END,
            updated_at=datetime('now') WHERE id=?", status, code, status, id);
    }

    /// <summary>True when the fetched HTML has an anchor pointing at our exact target URL (trailing-slash
    /// tolerant) or, failing that, anywhere at our own domain.</summary>
    static bool LinkPresent(string html, string target, string ourDomain)
    {
        if (string.IsNullOrEmpty(html)) return false;
        var tgt = target.Trim();
        foreach (System.Text.RegularExpressions.Match m in RxHref.Matches(html))
        {
            var href = System.Net.WebUtility.HtmlDecode(m.Groups[1].Value).Trim();
            if (href.Length == 0) continue;
            if (tgt.Length > 0 && href.Equals(tgt, StringComparison.OrdinalIgnoreCase)) return true;
            if (tgt.Length > 0 && href.TrimEnd('/').Equals(tgt.TrimEnd('/'), StringComparison.OrdinalIgnoreCase)) return true;
            if (ourDomain.Length > 0 && href.ToLowerInvariant().Contains(ourDomain)) return true;
        }
        return false;
    }

    static readonly System.Text.RegularExpressions.Regex RxHref =
        new("href\\s*=\\s*[\"']([^\"']+)[\"']",
            System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
}
