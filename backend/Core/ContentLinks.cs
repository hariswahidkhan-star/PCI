using System.Text.RegularExpressions;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Content link registry (Phase C). Extracts the hyperlinks a post publishes from its body HTML and classifies
/// each internal (relative/anchor/same-site) vs external. Editors set a rel policy and mark/approve citations.
/// On demand only, an EXTERNAL link's HTTP status is checked through the SSRF-guarded egress client — never a
/// private/internal target, and redirects are not chased under the guard. This is the editorial inventory of
/// outbound links the audit found missing; it NEVER rewrites a post's body. Links no longer present in the
/// body are marked inactive (history preserved), not deleted.
/// </summary>
public static class ContentLinks
{
    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(15));
    static readonly Regex RxAnchor = new("<a\\b[^>]*href\\s*=\\s*[\"']([^\"']+)[\"'][^>]*>(.*?)</a>",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

    /// <summary>Classify a URL as internal (relative, anchor, mail/tel, or same canonical host) or external.</summary>
    public static string Kind(string? url, string ourHost)
    {
        var u = (url ?? "").Trim();
        if (u.Length == 0) return "external";
        if (u.StartsWith("#") || u.StartsWith("/") || u.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) || u.StartsWith("tel:", StringComparison.OrdinalIgnoreCase)) return "internal";
        if (Uri.TryCreate(u, UriKind.Absolute, out var abs))
            return abs.Host.Equals(ourHost, StringComparison.OrdinalIgnoreCase) || abs.Host.EndsWith("." + ourHost, StringComparison.OrdinalIgnoreCase) ? "internal" : "external";
        return "internal";   // a relative/unknown scheme → served by us
    }

    static string Norm(string url) => (url ?? "").Trim().TrimEnd('/').ToLowerInvariant();
    static string Host(string url) { try { return new Uri(url).Host.ToLowerInvariant(); } catch { return ""; } }

    /// <summary>
    /// Extract links from the post body and upsert the registry (idempotent by post+normalised URL). Returns
    /// (found, added). Rows for links no longer in the body are marked inactive, never deleted.
    /// </summary>
    public static (int found, int added) Scan(Db db, long postId, long? adminId)
    {
        var post = db.QueryOne("SELECT body FROM blog_posts WHERE id=?", postId);
        if (post is null) return (0, 0);
        var body = H.Str(post["body"]) ?? "";
        var ourHost = Host(Redirects.CanonicalBase);
        var seen = new HashSet<string>();
        int found = 0, added = 0;
        foreach (Match m in RxAnchor.Matches(body))
        {
            var href = System.Net.WebUtility.HtmlDecode(m.Groups[1].Value).Trim();
            if (href.Length == 0 || href.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase)) continue;
            var norm = Norm(href);
            if (!seen.Add(norm)) continue;                              // dedupe within the body
            found++;
            var anchor = Regex.Replace(m.Groups[2].Value, "<[^>]+>", "").Trim();
            if (anchor.Length > 300) anchor = anchor[..300];
            var kind = Kind(href, ourHost);
            var url = href.Length > 500 ? href[..500] : href;
            var existing = db.QueryOne("SELECT id FROM cc_content_links WHERE post_id=? AND url_norm=?", postId, norm);
            if (existing is null)
            {
                db.Execute(@"INSERT INTO cc_content_links(post_id,url,url_norm,kind,anchor_text,created_by,created_at,updated_at)
                    VALUES(?,?,?,?,?,?, datetime('now'), datetime('now'))", postId, url, norm, kind, anchor, adminId);
                added++;
            }
            else
                db.Execute("UPDATE cc_content_links SET kind=?, anchor_text=?, active=1, updated_at=datetime('now') WHERE id=?", kind, anchor, H.L(existing["id"]));
        }
        // Links previously recorded but no longer present → mark inactive (kept for history/audit).
        if (seen.Count > 0)
        {
            var inClause = string.Join(",", seen.Select(_ => "?"));
            var args = new List<object?> { postId };
            args.AddRange(seen.Cast<object?>());
            db.Execute($"UPDATE cc_content_links SET active=0, updated_at=datetime('now') WHERE post_id=? AND url_norm NOT IN ({inClause})", args.ToArray());
        }
        else
            db.Execute("UPDATE cc_content_links SET active=0, updated_at=datetime('now') WHERE post_id=?", postId);
        return (found, added);
    }

    /// <summary>
    /// Check an EXTERNAL link's HTTP status through the SSRF-guarded egress client. Internal links are not
    /// fetched (they're served by us). status ∈ live | redirect | broken | unreachable | internal.
    /// </summary>
    public static async Task<(string status, int code, string? err)> Check(Db db, long id)
    {
        var row = db.QueryOne("SELECT id,url,kind FROM cc_content_links WHERE id=?", id);
        if (row is null) return ("unreachable", 0, "not_found");
        if (H.Str(row["kind"]) == "internal") { Record(db, id, "internal", 0); return ("internal", 0, null); }
        var url = H.Str(row["url"]) ?? "";
        var problem = Egress.UrlProblem(url);
        if (problem is not null) { Record(db, id, "unreachable", 0); return ("unreachable", 0, problem); }
        string status; int code = 0;
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.TryAddWithoutValidation("User-Agent", "PCI-LinkChecker/1.0 (+https://projectcontrolsinstitute.org)");
            using var resp = await Http.SendAsync(req);
            code = (int)resp.StatusCode;
            status = code is >= 300 and < 400 ? "redirect" : code >= 400 ? "broken" : "live";
        }
        catch (Exception e) { Record(db, id, "unreachable", code); return ("unreachable", code, e.Message); }
        Record(db, id, status, code);
        return (status, code, null);
    }

    static void Record(Db db, long id, string status, int code) =>
        db.Execute("UPDATE cc_content_links SET status=?, http_code=?, last_checked_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
            status, code == 0 ? (object?)null : code, id);
}
