using System.Xml.Linq;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// External content import (Phase 4). Fetches an APPROVED source's RSS/Atom feed through the SSRF-guarded
/// egress client, sanitises + de-duplicates each item, and files it in the review queue — nothing is copied
/// wholesale or published automatically. Approval turns a queued item into a PCI blog DRAFT an editor
/// finalises: "link" mode writes a lead + a link back to the original (the copyright-safe default), "excerpt"
/// uses the permitted excerpt, and "full" republication is only reachable when the source's recorded licence
/// allows it. Every imported copy carries attribution and a canonical pointing at the original source.
/// </summary>
public static class ExternalImport
{
    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(25));

    /// <summary>Fetch + ingest one source's feed. Returns (added, duplicate, total, error). Never throws.</summary>
    public static async Task<(int added, int duplicate, int total, string? error)> FetchSource(Db db, long sourceId)
    {
        var s = db.QueryOne("SELECT * FROM cc_external_sources WHERE id=?", sourceId);
        if (s is null) return (0, 0, 0, "source_not_found");
        var feedUrl = H.Str(s["feed_url"]) ?? "";
        if (feedUrl.Length == 0) return (0, 0, 0, "feed_url_required");
        string xml;
        try { xml = await Http.GetStringAsync(feedUrl); }
        catch (Exception e) { db.Execute("UPDATE cc_external_sources SET last_error=?, updated_at=datetime('now') WHERE id=?", Trim(e.Message), sourceId); return (0, 0, 0, e.Message); }

        List<FeedItem> items;
        try { items = Parse(xml); }
        catch (Exception e) { db.Execute("UPDATE cc_external_sources SET last_error=? WHERE id=?", "parse: " + Trim(e.Message), sourceId); return (0, 0, 0, "parse_error"); }

        int added = 0, dup = 0;
        foreach (var it in items)
        {
            var guid = (it.Guid ?? it.Link ?? "").Trim();
            if (guid.Length == 0) continue;
            var summary = HtmlSanitize.Clean(it.Content ?? "");     // strip scripts/iframes/trackers before storage
            var hash = Security.Sha((it.Title ?? "") + "|" + summary);
            var (_, changes) = db.ExecuteWithChanges(@"INSERT OR IGNORE INTO cc_external_items(source_id,guid,source_url,title,author,summary,image_url,published_at,content_hash,status,created_at,updated_at)
                VALUES(?,?,?,?,?,?,?,?,?, 'retrieved', datetime('now'), datetime('now'))",
                sourceId, Cap(guid, 400), Cap(it.Link ?? "", 500), it.Title, Cap(it.Author ?? "", 190), summary, Cap(it.Image ?? "", 500), it.Published, hash);
            if (changes > 0) added++; else dup++;
        }
        db.Execute("UPDATE cc_external_sources SET last_fetched_at=datetime('now'), last_error=NULL, updated_at=datetime('now') WHERE id=?", sourceId);
        return (added, dup, items.Count, null);
    }

    /// <summary>Turn a queued item into a PCI blog DRAFT. mode: link | excerpt | full. Enforces the source's
    /// allowed_use for full republication. Returns (ok, postId, error).</summary>
    public static (bool ok, long? postId, string? error) ApproveItem(Db db, long itemId, string mode, long? adminId)
    {
        var it = db.QueryOne("SELECT * FROM cc_external_items WHERE id=?", itemId);
        if (it is null) return (false, null, "item_not_found");
        var s = db.QueryOne("SELECT * FROM cc_external_sources WHERE id=?", H.L(it["source_id"]));
        if (s is null) return (false, null, "source_missing");
        var allowed = H.Str(s["allowed_use"]) ?? "curated_link";
        if (mode == "full" && !(allowed == "full" && !string.IsNullOrWhiteSpace(H.Str(s["permission_ref"]))))
            return (false, null, "full_republication_not_permitted");
        if (mode == "excerpt" && allowed == "curated_link")
            return (false, null, "excerpt_not_permitted");

        var title = H.Str(it["title"]) ?? "Imported article";
        var sourceUrl = H.Str(it["source_url"]) ?? "";
        var srcName = H.Str(s["name"]) ?? H.Str(s["domain"]) ?? "the original source";
        var content = H.Str(it["summary"]) ?? "";
        var attribution = $"<p class=\"attribution\"><em>Originally published by <a href=\"{sourceUrl}\" rel=\"nofollow noopener\">{System.Net.WebUtility.HtmlEncode(srcName)}</a>.</em></p>";
        var readMore = $"<p><a href=\"{sourceUrl}\" rel=\"nofollow noopener\">Read the original article at {System.Net.WebUtility.HtmlEncode(H.Str(s["domain"]) ?? srcName)} →</a></p>";
        string body; string ownership; string itemStatus;
        switch (mode)
        {
            case "full":
                body = HtmlSanitize.Clean(content) + attribution;
                ownership = "licensed"; itemStatus = "approved_full"; break;
            case "excerpt":
                body = "<blockquote>" + Cap(Text(content), 600) + "</blockquote>" + attribution + readMore;
                ownership = "excerpt"; itemStatus = "approved_excerpt"; break;
            default: // curated link — a PCI-written lead (seeded from the excerpt) + a link to the original
                body = "<p>" + Cap(Text(content), 320) + "</p>" + readMore;
                ownership = "curated"; itemStatus = "approved_link"; break;
        }
        var slug = Blog.UniqueSlug(db, title);
        // canonical points at the ORIGINAL source (it is their content), never at PCI.
        var postId = db.ExecuteReturningId(@"INSERT INTO blog_posts(slug,title,summary,body,body_format,status,published,
                content_ownership,attribution,original_source_url,canonical_url,structured_type,created_by,created_at,updated_at)
            VALUES(?,?,?,?, 'html', 'draft', 0, ?,?,?,?, 'BlogPosting', ?, datetime('now'), datetime('now'))",
            slug, title, Cap(Text(content), 300), body, ownership, srcName, sourceUrl, sourceUrl, adminId);
        db.Execute("UPDATE cc_external_items SET status=?, pci_post_id=?, reviewed_by=?, updated_at=datetime('now') WHERE id=?",
            itemStatus, postId, adminId, itemId);
        return (true, postId, null);
    }

    // ── feed parsing (RSS 2.0 + Atom) ──────────────────────────────────────────────────────────────────
    record FeedItem(string? Title, string? Link, string? Guid, string? Content, string? Author, string? Image, string? Published);

    static List<FeedItem> Parse(string xml)
    {
        var doc = XDocument.Parse(xml);
        var root = doc.Root!;
        var list = new List<FeedItem>();
        // RSS 2.0 (<rss><channel><item>) or RDF: items named "item"
        var rssItems = root.Descendants().Where(e => e.Name.LocalName == "item").ToList();
        if (rssItems.Count > 0)
        {
            foreach (var i in rssItems)
                list.Add(new FeedItem(
                    Val(i, "title"), Val(i, "link"), Val(i, "guid") ?? Val(i, "link"),
                    Val(i, "encoded") ?? Val(i, "description") ?? Val(i, "summary"),
                    Val(i, "creator") ?? Val(i, "author"),
                    Attr(i, "enclosure", "url") ?? Attr(i, "content", "url") ?? Attr(i, "thumbnail", "url"),
                    Val(i, "pubDate") ?? Val(i, "date")));
            return list;
        }
        // Atom (<feed><entry>)
        foreach (var e in root.Descendants().Where(x => x.Name.LocalName == "entry"))
            list.Add(new FeedItem(
                Val(e, "title"), AtomLink(e), Val(e, "id") ?? AtomLink(e),
                Val(e, "content") ?? Val(e, "summary"),
                e.Elements().FirstOrDefault(x => x.Name.LocalName == "author")?.Elements().FirstOrDefault(x => x.Name.LocalName == "name")?.Value,
                null, Val(e, "published") ?? Val(e, "updated")));
        return list;
    }
    static string? Val(XElement p, string local) => p.Elements().FirstOrDefault(e => e.Name.LocalName == local)?.Value?.Trim();
    static string? Attr(XElement p, string local, string attr) => p.Elements().FirstOrDefault(e => e.Name.LocalName == local)?.Attribute(attr)?.Value;
    static string? AtomLink(XElement e)
    {
        var links = e.Elements().Where(x => x.Name.LocalName == "link").ToList();
        var alt = links.FirstOrDefault(l => (l.Attribute("rel")?.Value ?? "alternate") == "alternate");
        return (alt ?? links.FirstOrDefault())?.Attribute("href")?.Value;
    }

    static readonly System.Text.RegularExpressions.Regex RxTag = new("<[^>]+>", System.Text.RegularExpressions.RegexOptions.Compiled);
    static string Text(string html) => System.Net.WebUtility.HtmlDecode(RxTag.Replace(html, " ")).Replace("  ", " ").Trim();
    static string Cap(string s, int n) => s.Length <= n ? s : s[..(n - 1)].TrimEnd() + "…";
    static string Trim(string s) => s.Length > 300 ? s[..300] : s;
}
