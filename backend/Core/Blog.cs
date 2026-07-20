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
    public static string BasePath(Db db)
    {
        var p = (db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='blog_base_path'") ?? "/blog").Trim();
        if (p.Length == 0) p = "/blog";
        if (!p.StartsWith("/")) p = "/" + p;
        return p.TrimEnd('/');
    }

    public static int PerPage(Db db)
    {
        // Read as string + parse in C# (avoids CAST AS INTEGER, which the MySQL translator would turn into
        // the invalid CAST AS BIGINT — MySQL cast targets are SIGNED/UNSIGNED, not BIGINT).
        var s = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='blog_posts_per_page'");
        return int.TryParse(s, out var n) && n is > 0 and <= 100 ? n : 12;
    }

    public static string PublicUrl(Db db, string slug) => Redirects.CanonicalBase + BasePath(db) + "/" + slug;

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

    /// <summary>List published posts (newest first) with optional category/author/tag/language filters.</summary>
    public static List<Dictionary<string, object?>> ListPublished(Db db, int limit, int offset,
        long? categoryId = null, long? authorId = null, long? tagId = null, string? lang = null)
    {
        var sql = new StringBuilder("SELECT p.* FROM blog_posts p");
        var args = new List<object?>();
        if (tagId is not null) sql.Append(" JOIN blog_post_tags pt ON pt.post_id=p.id AND pt.tag_id=?").Also(args, tagId);
        sql.Append(" WHERE p.status='published' AND p.published=1");
        if (categoryId is not null) { sql.Append(" AND p.category_id=?"); args.Add(categoryId); }
        if (authorId is not null) { sql.Append(" AND p.author_id=?"); args.Add(authorId); }
        if (!string.IsNullOrEmpty(lang)) { sql.Append(" AND p.language=?"); args.Add(lang); }
        sql.Append(" ORDER BY COALESCE(p.published_at, p.created_at) DESC, p.id DESC LIMIT ? OFFSET ?");
        args.Add(limit); args.Add(offset);
        return db.Query(sql.ToString(), args.ToArray());
    }

    public static int CountPublished(Db db, long? categoryId = null, long? authorId = null, string? lang = null)
    {
        var sql = new StringBuilder("SELECT COUNT(*) FROM blog_posts p WHERE p.status='published' AND p.published=1");
        var args = new List<object?>();
        if (categoryId is not null) { sql.Append(" AND p.category_id=?"); args.Add(categoryId); }
        if (authorId is not null) { sql.Append(" AND p.author_id=?"); args.Add(authorId); }
        if (!string.IsNullOrEmpty(lang)) { sql.Append(" AND p.language=?"); args.Add(lang); }
        return (int)db.Scalar<long>(sql.ToString(), args.ToArray());
    }

    public static Dictionary<string, object?>? Author(Db db, object? id) =>
        id is null ? null : db.QueryOne("SELECT * FROM blog_authors WHERE id=?", H.L(id));
    public static Dictionary<string, object?>? Category(Db db, object? id) =>
        id is null ? null : db.QueryOne("SELECT * FROM blog_categories WHERE id=?", H.L(id));
    public static List<Dictionary<string, object?>> Tags(Db db, long postId) =>
        db.Query("SELECT t.* FROM blog_tags t JOIN blog_post_tags pt ON pt.tag_id=t.id WHERE pt.post_id=? ORDER BY t.name", postId);

    /// <summary>All published post slugs → canonical URLs (for the blog sitemap + feeds).</summary>
    public static List<Dictionary<string, object?>> PublishedForFeed(Db db, int limit) =>
        db.Query("SELECT * FROM blog_posts WHERE status='published' AND published=1 AND COALESCE(robots_noindex,0)=0 ORDER BY COALESCE(published_at,created_at) DESC, id DESC LIMIT ?", limit);

    // small fluent helper so the tag-join arg is appended in the right order
    static StringBuilder Also(this StringBuilder sb, List<object?> args, object? v) { args.Add(v); return sb; }
}
