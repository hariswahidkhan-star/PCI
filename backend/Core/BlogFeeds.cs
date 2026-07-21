using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Syndication feeds and sitemaps for the two public content sections. Blog and news share one store
/// (blog_posts) but each has its own feeds (RSS 2.0, Atom 1.0, JSON Feed 1.1), its own listing sitemap, and
/// its own URL space (/blog vs /news). A feed/sitemap for a section lists ONLY that section's posts, so blog
/// and news never bleed into each other. The blog sitemap carries image entries; the news sitemap carries
/// the Google-News (news:) namespace and, per Google's rule, only articles from the last two days. All
/// timestamps are the stored "YYYY-MM-DD HH:MM:SS" UTC strings.
/// </summary>
public static class BlogFeeds
{
    static string Base => Redirects.CanonicalBase;

    static List<Dictionary<string, object?>> Items(Db db, bool? news = null) => Blog.PublishedForFeed(db, 50, news);

    // ── RSS 2.0 ──────────────────────────────────────────────────────────────────────────────────────
    public static string Rss(Db db, bool news = false)
    {
        var bp = Blog.BasePathFor(db, news);
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.Append("<rss version=\"2.0\" xmlns:atom=\"http://www.w3.org/2005/Atom\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\"><channel>");
        sb.Append("<title>Project Controls Institute — ").Append(news ? "News" : "Blog &amp; Insights").Append("</title>");
        sb.Append("<link>").Append(X(Base + bp)).Append("</link>");
        sb.Append("<description>").Append(news
            ? "News, announcements and industry updates on project controls, project finance and project delivery."
            : "Project controls, project finance, project delivery and AI insights, research and certification news.").Append("</description>");
        sb.Append("<language>en</language>");
        sb.Append("<atom:link href=\"").Append(X(Base + bp + "/feed.xml")).Append("\" rel=\"self\" type=\"application/rss+xml\"/>");
        sb.Append("<lastBuildDate>").Append(X(Rfc822(Newest(db, news)))).Append("</lastBuildDate>");
        foreach (var p in Items(db, news))
        {
            var url = Base + bp + "/" + (H.Str(p["slug"]) ?? "");
            sb.Append("<item>");
            sb.Append("<title>").Append(X(H.Str(p["title"]) ?? "")).Append("</title>");
            sb.Append("<link>").Append(X(url)).Append("</link>");
            sb.Append("<guid isPermaLink=\"true\">").Append(X(url)).Append("</guid>");
            if (Blog.Author(db, p["author_id"]) is { } a) sb.Append("<dc:creator>").Append(X(H.Str(a["name"]) ?? "")).Append("</dc:creator>");
            if (Blog.Category(db, p["category_id"]) is { } c) sb.Append("<category>").Append(X(H.Str(c["name"]) ?? "")).Append("</category>");
            sb.Append("<description>").Append(X(H.Str(p["summary"]) ?? H.Str(p["meta_description"]) ?? "")).Append("</description>");
            if (H.Str(p["published_at"]) is { Length: > 0 } pub) sb.Append("<pubDate>").Append(X(Rfc822(pub))).Append("</pubDate>");
            sb.Append("</item>");
        }
        sb.Append("</channel></rss>");
        return sb.ToString();
    }

    // ── Atom 1.0 ─────────────────────────────────────────────────────────────────────────────────────
    public static string Atom(Db db, bool news = false)
    {
        var bp = Blog.BasePathFor(db, news);
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.Append("<feed xmlns=\"http://www.w3.org/2005/Atom\">");
        sb.Append("<title>Project Controls Institute — ").Append(news ? "News" : "Blog &amp; Insights").Append("</title>");
        sb.Append("<link href=\"").Append(X(Base + bp)).Append("\"/>");
        sb.Append("<link href=\"").Append(X(Base + bp + "/atom.xml")).Append("\" rel=\"self\"/>");
        sb.Append("<id>").Append(X(Base + bp)).Append("</id>");
        sb.Append("<updated>").Append(X(Iso(Newest(db, news)))).Append("</updated>");
        foreach (var p in Items(db, news))
        {
            var url = Base + bp + "/" + (H.Str(p["slug"]) ?? "");
            sb.Append("<entry>");
            sb.Append("<title>").Append(X(H.Str(p["title"]) ?? "")).Append("</title>");
            sb.Append("<link href=\"").Append(X(url)).Append("\"/>");
            sb.Append("<id>").Append(X(url)).Append("</id>");
            if (H.Str(p["published_at"]) is { Length: > 0 } pub) sb.Append("<published>").Append(X(Iso(pub))).Append("</published>");
            if (H.Str(p["updated_at"]) is { Length: > 0 } upd) sb.Append("<updated>").Append(X(Iso(upd))).Append("</updated>");
            if (Blog.Author(db, p["author_id"]) is { } a) sb.Append("<author><name>").Append(X(H.Str(a["name"]) ?? "")).Append("</name></author>");
            if (Blog.Category(db, p["category_id"]) is { } c) sb.Append("<category term=\"").Append(X(H.Str(c["name"]) ?? "")).Append("\"/>");
            sb.Append("<summary>").Append(X(H.Str(p["summary"]) ?? H.Str(p["meta_description"]) ?? "")).Append("</summary>");
            sb.Append("</entry>");
        }
        sb.Append("</feed>");
        return sb.ToString();
    }

    // ── JSON Feed 1.1 ────────────────────────────────────────────────────────────────────────────────
    public static string Json(Db db, bool news = false)
    {
        var bp = Blog.BasePathFor(db, news);
        var items = Items(db, news).Select(p =>
        {
            var url = Base + bp + "/" + (H.Str(p["slug"]) ?? "");
            var a = Blog.Author(db, p["author_id"]);
            var c = Blog.Category(db, p["category_id"]);
            return new Dictionary<string, object?>
            {
                ["id"] = url,
                ["url"] = url,
                ["title"] = H.Str(p["title"]),
                ["summary"] = H.Str(p["summary"]) ?? H.Str(p["meta_description"]),
                ["image"] = H.Str(p["featured_image"]),
                ["date_published"] = H.Str(p["published_at"]) is { Length: > 0 } pub ? Iso(pub) : null,
                ["date_modified"] = H.Str(p["updated_at"]) is { Length: > 0 } upd ? Iso(upd) : null,
                ["language"] = H.Str(p["language"]) ?? "en",
                ["authors"] = a is null ? null : new[] { new Dictionary<string, object?> { ["name"] = H.Str(a["name"]) } },
                ["tags"] = c is null ? null : new[] { H.Str(c["name"]) },
            };
        }).ToList();
        var feed = new Dictionary<string, object?>
        {
            ["version"] = "https://jsonfeed.org/version/1.1",
            ["title"] = "Project Controls Institute — " + (news ? "News" : "Blog & Insights"),
            ["home_page_url"] = Base + bp,
            ["feed_url"] = Base + bp + "/feed.json",
            ["language"] = "en",
            ["items"] = items,
        };
        return JsonSerializer.Serialize(feed, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
    }

    // ── Blog sitemap (with images) — NON-news posts only ──────────────────────────────────────────────
    public static string Sitemap(Db db)
    {
        var bp = Blog.BasePath(db);
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\">");
        // the blog index itself
        sb.Append("<url><loc>").Append(X(Base + bp)).Append("</loc><changefreq>daily</changefreq></url>");
        foreach (var p in Blog.PublishedForFeed(db, 5000, news: false))
        {
            var url = Base + bp + "/" + (H.Str(p["slug"]) ?? "");
            sb.Append("<url><loc>").Append(X(url)).Append("</loc>");
            if (H.Str(p["updated_at"]) is { Length: > 0 } upd) sb.Append("<lastmod>").Append(X(upd.Split(' ')[0])).Append("</lastmod>");
            sb.Append("<changefreq>monthly</changefreq>");
            if (H.Str(p["featured_image"]) is { Length: > 0 } fi)
            {
                var img = fi.StartsWith("http") ? fi : Base + (fi.StartsWith("/") ? "" : "/") + fi;
                sb.Append("<image:image><image:loc>").Append(X(img)).Append("</image:loc>");
                if (H.Str(p["featured_image_alt"]) is { Length: > 0 } alt) sb.Append("<image:caption>").Append(X(alt)).Append("</image:caption>");
                sb.Append("</image:image>");
            }
            sb.Append("</url>");
        }
        sb.Append("</urlset>");
        return sb.ToString();
    }

    // ── News sitemap (Google News) — NewsArticles from the last two days only ──────────────────────────
    // Google News ignores articles older than 48h in a news sitemap, so we filter to that window. An empty
    // urlset is valid (no recent news → nothing to surface).
    public static string NewsSitemap(Db db)
    {
        var bp = Blog.NewsBasePath(db);
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">");
        foreach (var p in Blog.PublishedForFeed(db, 1000, news: true))
        {
            var pub = H.Str(p["published_at"]);
            if (string.IsNullOrEmpty(pub) || !Recent(pub, 2)) continue;
            var url = Base + bp + "/" + (H.Str(p["slug"]) ?? "");
            sb.Append("<url><loc>").Append(X(url)).Append("</loc>");
            sb.Append("<news:news>");
            sb.Append("<news:publication><news:name>Project Controls Institute</news:name><news:language>en</news:language></news:publication>");
            sb.Append("<news:publication_date>").Append(X(Iso(pub))).Append("</news:publication_date>");
            sb.Append("<news:title>").Append(X(H.Str(p["title"]) ?? "")).Append("</news:title>");
            sb.Append("</news:news>");
            sb.Append("</url>");
        }
        sb.Append("</urlset>");
        return sb.ToString();
    }

    static bool Recent(string dt, int days) =>
        DateTime.TryParse(dt, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out var d)
        && d >= DateTime.UtcNow.AddDays(-days);

    static string Newest(Db db, bool news = false)
    {
        var col = news ? "AND structured_type='NewsArticle'" : "AND (structured_type IS NULL OR structured_type<>'NewsArticle')";
        return db.Scalar<string>($"SELECT COALESCE(MAX(COALESCE(updated_at,published_at)), datetime('now')) FROM blog_posts WHERE status='published' AND published=1 {col}")
            ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }

    static string Iso(string dt) => dt.Contains('T') ? dt : dt.Replace(' ', 'T') + "Z";
    static string Rfc822(string dt) =>
        DateTime.TryParse(dt, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out var d)
        ? d.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", System.Globalization.CultureInfo.InvariantCulture)
        : DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", System.Globalization.CultureInfo.InvariantCulture);
    static string X(string s) => (s ?? "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
}
