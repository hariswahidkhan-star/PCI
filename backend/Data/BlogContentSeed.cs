using System.Text.Json;
using PCI.Backend.Core;

namespace PCI.Backend.Data;

/// <summary>
/// Seeds the 20 publication-ready PCI blog articles from blog_seed_articles.json — as DRAFTS ONLY. Nothing
/// here is ever auto-published: each article enters at status='draft' (published=0) for human editorial
/// review, so AI-assisted content never reaches the public site without a person approving it. Idempotent by
/// slug: a slug that already exists is skipped entirely, so operator edits and deletions are never
/// overwritten or resurrected, and re-running never duplicates. Financial / accounting / standards articles
/// are additionally flagged for expert review (a pending blog_reviews record + an internal note). Content is
/// disclosed as AI-assisted (ai_assisted=1) for honesty.
/// </summary>
public static class BlogContentSeed
{
    public static void Ensure(Db db)
    {
        try
        {
            var path = File.Exists("blog_seed_articles.json") ? "blog_seed_articles.json"
                     : Path.Combine(AppContext.BaseDirectory, "blog_seed_articles.json");
            if (!File.Exists(path)) return;
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return;
            var authorId = AuthorId(db);
            int added = 0;
            foreach (var a in doc.RootElement.EnumerateArray())
            {
                var slug = Str(a, "slug"); if (string.IsNullOrWhiteSpace(slug)) continue;
                // Idempotent: an existing slug is left completely alone (never overwrite a human edit, never
                // re-create a deleted post, never duplicate).
                if (db.QueryOne("SELECT id FROM blog_posts WHERE slug=?", slug) is not null) continue;
                var catId = CategoryId(db, Str(a, "category_slug"));
                var review = Bool(a, "needs_expert_review");
                var notes = "AI-assisted draft — human editorial review required before publishing."
                          + (review ? " Financial/accounting/standards content: expert review required." : "");
                var id = db.ExecuteReturningId(@"INSERT INTO blog_posts(
                        slug,title,seo_title,summary,body,body_format,meta_description,primary_keyword,secondary_keywords,
                        author_id,category_id,structured_type,status,published,language,ai_assisted,ai_disclosure,
                        content_ownership,internal_notes,version,created_at,updated_at)
                    VALUES(?,?,?,?,?, 'html', ?,?,?, ?,?, 'BlogPosting','draft',0,'en',1,?, 'original', ?, 1, datetime('now'), datetime('now'))",
                    slug, Str(a, "title"), Str(a, "seo_title"), Str(a, "summary"), Str(a, "body_html"),
                    Str(a, "meta_description"), Str(a, "primary_keyword"), Str(a, "secondary_keywords"),
                    // ai_disclosure is a SHORT public byline suffix (rendered as "· <em>AI-assisted</em>") and the
                    // column is VARCHAR(32); the longer editorial explanation lives in internal_notes (TEXT).
                    authorId, catId, "AI-assisted", notes);

                if (a.TryGetProperty("tags", out var tags) && tags.ValueKind == JsonValueKind.Array)
                    foreach (var t in tags.EnumerateArray())
                    {
                        var name = t.GetString(); if (string.IsNullOrWhiteSpace(name)) continue;
                        var tslug = PCI.Backend.Core.Blog.Slugify(name);
                        db.Execute("INSERT OR IGNORE INTO blog_tags(slug,name,created_at) VALUES(?,?, datetime('now'))", tslug, name.Trim());
                        var trow = db.QueryOne("SELECT id FROM blog_tags WHERE slug=?", tslug);
                        if (trow is not null) db.Execute("INSERT OR IGNORE INTO blog_post_tags(post_id,tag_id) VALUES(?,?)", id, H.L(trow["id"]));
                    }

                // Version-1 snapshot (history from the very first save) + an editorial review record.
                var snap = db.QueryOne("SELECT * FROM blog_posts WHERE id=?", id);
                if (snap is not null)
                    db.Execute("INSERT INTO blog_post_versions(post_id,version,status_at,snapshot_json,change_reason,editor_id,created_at) VALUES(?,1,'draft',?,?,NULL, datetime('now'))",
                        id, JsonSerializer.Serialize(snap), "seeded draft");
                db.Execute("INSERT INTO blog_reviews(post_id,stage,decision,reviewer_id,note,created_at) VALUES(?,'editorial_review','pending',NULL,?, datetime('now'))",
                    id, "Seeded draft — awaiting editorial review before publication.");
                if (review)
                    db.Execute("INSERT INTO blog_reviews(post_id,stage,decision,reviewer_id,note,created_at) VALUES(?,'legal_review','pending',NULL,?, datetime('now'))",
                        id, "Financial/accounting/standards content — expert review required before publication.");
                added++;
            }
            if (added > 0) Console.WriteLine($"[seed] blog articles: {added} draft(s) added");
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] blog articles skipped: {e.Message}"); }
    }

    static long? AuthorId(Db db) { var r = db.QueryOne("SELECT id FROM blog_authors WHERE slug='pci-editorial-team'"); return r is null ? null : H.L(r["id"]); }
    static long? CategoryId(Db db, string? slug) { if (string.IsNullOrWhiteSpace(slug)) return null; var r = db.QueryOne("SELECT id FROM blog_categories WHERE slug=?", slug); return r is null ? null : H.L(r["id"]); }
    static string? Str(JsonElement e, string k) => e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
    static bool Bool(JsonElement e, string k) => e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.True;
}
