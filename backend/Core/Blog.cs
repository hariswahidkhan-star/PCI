using System.Text;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Blog data helpers shared by the public renderer (BlogRender), the feeds, the sitemap and the admin
/// endpoints. A post is PUBLIC + INDEXABLE when status='published' AND published=1 AND robots_noindex=0.
/// The canonical host is the single source Redirects.CanonicalBase, so blog URLs match the rest of the site.
/// Nothing here is cert/exam/period specific — categories, authors, tags and the base path are all data.
/// </summary>
public static class Blog
{
    // A post is a NEWS item when its structured_type is NewsArticle; otherwise it belongs to the blog. News
    // and blog share ONE store (blog_posts) and ONE admin CMS/workflow, but live at separate public URL
    // spaces (/news vs /blog) with their own listings, feeds and sitemaps.
    public const string NewsType = "NewsArticle";
    public static bool IsNews(Dictionary<string, object?> post) => (H.Str(post["structured_type"]) ?? "") == NewsType;

    static string ReadBasePath(Db db, string skey, string fallback)
    {
        var p = (db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", skey) ?? fallback).Trim();
        if (p.Length == 0) p = fallback;
        if (!p.StartsWith("/")) p = "/" + p;
        return p.TrimEnd('/');
    }

    public static string BasePath(Db db) => ReadBasePath(db, "blog_base_path", "/blog");
    public static string NewsBasePath(Db db) => ReadBasePath(db, "news_base_path", "/news");
    public static string BasePathFor(Db db, bool news) => news ? NewsBasePath(db) : BasePath(db);

    /// <summary>Human label for a section's breadcrumb/heading root.</summary>
    public static string SectionLabel(bool news) => news ? "News" : "Blog";

    public static int PerPage(Db db)
    {
        // Read as string + parse in C# (avoids CAST AS INTEGER, which the MySQL translator would turn into
        // the invalid CAST AS BIGINT — MySQL cast targets are SIGNED/UNSIGNED, not BIGINT).
        var s = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='blog_posts_per_page'");
        return int.TryParse(s, out var n) && n is > 0 and <= 100 ? n : 12;
    }

    /// <summary>Canonical public URL for a post identified by slug — routed to /news or /blog by its type.</summary>
    public static string PublicUrl(Db db, string slug)
    {
        var isNews = db.Scalar<string>("SELECT structured_type FROM blog_posts WHERE slug=?", slug) == NewsType;
        return PublicUrl(db, slug, isNews);
    }
    public static string PublicUrl(Db db, string slug, bool news) => Redirects.CanonicalBase + BasePathFor(db, news) + "/" + slug;

    /// <summary>URL-safe slug from arbitrary text (lowercase, hyphenated, ascii-ish). Never empty.</summary>
    public static string Slugify(string s)
    {
        s = (s ?? "").Trim().ToLowerInvariant();
        var sb = new StringBuilder(s.Length);
        char prev = '-';
        foreach (var ch in s)
        {
            char c = (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') ? ch
                   : (ch == ' ' || ch == '-' || ch == '_' || ch == '/' || ch == '.') ? '-' : '\0';
            if (c == '\0') continue;
            if (c == '-' && prev == '-') continue;
            sb.Append(c); prev = c;
        }
        var outp = sb.ToString().Trim('-');
        return outp.Length > 0 ? (outp.Length > 80 ? outp[..80].Trim('-') : outp) : "post";
    }

    /// <summary>Ensure the slug is unique in blog_posts (append -2, -3 … as needed). exceptId ignores a row (edit).</summary>
    public static string UniqueSlug(Db db, string baseSlug, long? exceptId = null)
    {
        var slug = Slugify(baseSlug);
        var candidate = slug; int n = 1;
        while (true)
        {
            var existing = db.QueryOne("SELECT id FROM blog_posts WHERE slug=?", candidate);
            if (existing is null || (exceptId is not null && H.L(existing["id"]) == exceptId)) return candidate;
            n++; candidate = slug + "-" + n;
        }
    }

    /// <summary>Rough reading time in minutes from body text (≈200 wpm), min 1.</summary>
    public static int ReadingTime(string? body)
    {
        if (string.IsNullOrWhiteSpace(body)) return 1;
        var text = System.Text.RegularExpressions.Regex.Replace(body, "<[^>]+>", " ");
        var words = text.Split(new[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Max(1, (int)Math.Round(words / 200.0));
    }

    /// <summary>A published, indexable post by slug (public view). Null if missing/unpublished.</summary>
    public static Dictionary<string, object?>? PublishedBySlug(Db db, string slug) =>
        db.QueryOne("SELECT * FROM blog_posts WHERE slug=? AND status='published' AND published=1", (slug ?? "").Trim().ToLowerInvariant());

    // Section filter fragment: news==true → NewsArticle only; news==false → everything except NewsArticle;
    // news==null → no filter (both sections, e.g. the combined main sitemap).
    static void SectionFilter(StringBuilder sql, List<object?> args, bool? news, string col = "p")
    {
        if (news == true) { sql.Append($" AND {col}.structured_type=?"); args.Add(NewsType); }
        else if (news == false) { sql.Append($" AND ({col}.structured_type IS NULL OR {col}.structured_type<>?)"); args.Add(NewsType); }
    }

    /// <summary>List published posts (newest first) with optional category/author/tag/language/section filters.</summary>
    public static List<Dictionary<string, object?>> ListPublished(Db db, int limit, int offset,
        long? categoryId = null, long? authorId = null, long? tagId = null, string? lang = null, bool? news = null)
    {
        var sql = new StringBuilder("SELECT p.* FROM blog_posts p");
        var args = new List<object?>();
        if (tagId is not null) sql.Append(" JOIN blog_post_tags pt ON pt.post_id=p.id AND pt.tag_id=?").Also(args, tagId);
        sql.Append(" WHERE p.status='published' AND p.published=1");
        if (categoryId is not null) { sql.Append(" AND p.category_id=?"); args.Add(categoryId); }
        if (authorId is not null) { sql.Append(" AND p.author_id=?"); args.Add(authorId); }
        if (!string.IsNullOrEmpty(lang)) { sql.Append(" AND p.language=?"); args.Add(lang); }
        SectionFilter(sql, args, news);
        sql.Append(" ORDER BY COALESCE(p.published_at, p.created_at) DESC, p.id DESC LIMIT ? OFFSET ?");
        args.Add(limit); args.Add(offset);
        return db.Query(sql.ToString(), args.ToArray());
    }

    public static int CountPublished(Db db, long? categoryId = null, long? authorId = null, string? lang = null, bool? news = null)
    {
        var sql = new StringBuilder("SELECT COUNT(*) FROM blog_posts p WHERE p.status='published' AND p.published=1");
        var args = new List<object?>();
        if (categoryId is not null) { sql.Append(" AND p.category_id=?"); args.Add(categoryId); }
        if (authorId is not null) { sql.Append(" AND p.author_id=?"); args.Add(authorId); }
        if (!string.IsNullOrEmpty(lang)) { sql.Append(" AND p.language=?"); args.Add(lang); }
        SectionFilter(sql, args, news);
        return (int)db.Scalar<long>(sql.ToString(), args.ToArray());
    }

    /// <summary>Up to <paramref name="limit"/> related published posts in the SAME section — same category
    /// first, topped up with the most recent — excluding the current post.</summary>
    public static List<Dictionary<string, object?>> Related(Db db, long postId, long? categoryId, bool news, int limit)
    {
        var outp = new List<Dictionary<string, object?>>();
        var seen = new HashSet<long> { postId };
        void Take(List<Dictionary<string, object?>> src) { foreach (var r in src) { if (outp.Count >= limit) break; if (seen.Add(H.L(r["id"]))) outp.Add(r); } }
        if (categoryId is not null) Take(ListPublished(db, limit + 1, 0, categoryId, null, null, null, news));
        if (outp.Count < limit) Take(ListPublished(db, limit + 3, 0, null, null, null, null, news));
        return outp;
    }

    public static Dictionary<string, object?>? Author(Db db, object? id) =>
        id is null ? null : db.QueryOne("SELECT * FROM blog_authors WHERE id=?", H.L(id));
    public static Dictionary<string, object?>? Category(Db db, object? id) =>
        id is null ? null : db.QueryOne("SELECT * FROM blog_categories WHERE id=?", H.L(id));
    public static List<Dictionary<string, object?>> Tags(Db db, long postId) =>
        db.Query("SELECT t.* FROM blog_tags t JOIN blog_post_tags pt ON pt.tag_id=t.id WHERE pt.post_id=? ORDER BY t.name", postId);

    /// <summary>Published, indexable posts (newest first) for feeds/sitemaps, optionally limited to a section.</summary>
    public static List<Dictionary<string, object?>> PublishedForFeed(Db db, int limit, bool? news = null)
    {
        var sql = new StringBuilder("SELECT * FROM blog_posts p WHERE p.status='published' AND p.published=1 AND COALESCE(p.robots_noindex,0)=0");
        var args = new List<object?>();
        SectionFilter(sql, args, news);
        sql.Append(" ORDER BY COALESCE(p.published_at,p.created_at) DESC, p.id DESC LIMIT ?"); args.Add(limit);
        return db.Query(sql.ToString(), args.ToArray());
    }

    // small fluent helper so the tag-join arg is appended in the right order
    static StringBuilder Also(this StringBuilder sb, List<object?> args, object? v) { args.Add(v); return sb; }
}
