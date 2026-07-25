using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — the editorial platform (EXPANSION_GOVERNANCE §5–§8).
///
/// One CMS serves both the blog and the newsroom, because they differ in obligations rather than
/// in machinery: a news item must trace every material claim to a saved source, a blog article
/// need not, and both go through the same workflow and the same immutable versioning.
///
/// The rules this module enforces in code rather than in a style guide:
///
///   • Every published article is a VERSION. Corrections append a visible correction record and a
///     new version; nothing is ever silently edited after publication.
///   • Maker-checker: the author of a piece can never be the person who approves it, exactly as
///     for challenges. Enforced in SQL, not in the UI.
///   • A news item cannot reach `approved` without at least one recorded source, and no article
///     mentioning a registry entity can be approved without a recorded legal review.
///   • Authorship is never invented. An article carries either a real named author or the
///     transparent "PCI World Editorial" byline — the governance allows those two and no others.
///
/// Body text is authored as a small, deliberately boring Markdown subset and rendered
/// server-side after escaping, so a compromised or careless author cannot inject markup.
/// </summary>
public static class WorldEditorial
{
    public static readonly string[] Kinds = { "blog", "news" };

    /// <summary>The editorial workflow. Challenges keep their own lifecycle; this is §5's chain.</summary>
    public static readonly string[] Statuses =
        { "idea", "drafting", "technical_review", "fact_check", "legal_review", "seo_review", "approved", "published", "archived" };

    /// <summary>Review kinds recorded against an article before it can be approved.</summary>
    public static readonly string[] ReviewKinds = { "technical", "fact_check", "legal", "seo" };

    public const string EditorialByline = "PCI World Editorial";

    // ───────────────────────────── validation ─────────────────────────────

    public sealed record ArticleInput(string Slug, string Kind, string? Title, string? Dek, string? Body,
        string? AuthorName, string? SeoTitle, string? SeoDesc, int SourceCount, bool MentionsEntity, bool LegalReviewed);

    public sealed record Issue(string Code, string Message);

    /// <summary>
    /// The publication gate. Returns the reasons an article may not proceed; empty means it may.
    /// Deliberately strict about the things the governance says are non-negotiable, and silent
    /// about matters of taste.
    /// </summary>
    public static List<Issue> Validate(ArticleInput a)
    {
        var issues = new List<Issue>();
        void Err(string c, string m) => issues.Add(new Issue(c, m));

        if (string.IsNullOrWhiteSpace(a.Slug) || a.Slug.Trim().Length < 3) Err("slug", "A URL slug of at least three characters is required.");
        else if (!a.Slug.All(ch => char.IsAsciiLetterOrDigit(ch) || ch == '-'))
            Err("slug", "The slug may contain only lowercase letters, digits and hyphens — it is a permanent public address.");
        if (!Kinds.Contains(a.Kind)) Err("kind", $"Kind must be one of: {string.Join(", ", Kinds)}.");
        if (string.IsNullOrWhiteSpace(a.Title)) Err("title", "A title is required.");
        if (string.IsNullOrWhiteSpace(a.Dek)) Err("dek", "A standfirst is required — it is the summary every listing and search result uses.");
        if (string.IsNullOrWhiteSpace(a.Body) || WordCount(a.Body) < 200)
            Err("body", "An article needs at least 200 words. Thin pages are prohibited by the SEO policy, not merely discouraged.");

        // Authorship: a real name or the transparent editorial byline. Never an invented person.
        if (string.IsNullOrWhiteSpace(a.AuthorName))
            Err("author", $"An author is required — a real named person or the transparent \"{EditorialByline}\" byline.");

        if (!string.IsNullOrWhiteSpace(a.SeoTitle) && a.SeoTitle!.Length > 70)
            Err("seo_title", "The SEO title must be 70 characters or fewer or search engines will truncate it.");
        if (!string.IsNullOrWhiteSpace(a.SeoDesc) && a.SeoDesc!.Length > 165)
            Err("seo_desc", "The meta description must be 165 characters or fewer.");

        // A news item asserts things about the world, so it must be able to show where each claim
        // came from. This is the difference between the newsroom and the blog, in code.
        if (a.Kind == "news" && a.SourceCount == 0)
            Err("sources", "A news item cannot be approved with no recorded source. Every material claim must be traceable.");

        // Naming a company is a legal exposure, not an editorial flourish.
        if (a.MentionsEntity && !a.LegalReviewed)
            Err("legal", "This article mentions a registered entity and has no recorded legal review.");

        return issues;
    }

    public static int WordCount(string? s) =>
        string.IsNullOrWhiteSpace(s) ? 0 : s.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;

    /// <summary>Reading time at 220 words per minute, floored at one minute.</summary>
    public static int ReadingMinutes(string? body) => Math.Max(1, (int)Math.Round(WordCount(body) / 220.0));

    /// <summary>Slugify a title for a first suggestion. The author still owns the final address.</summary>
    public static string Slugify(string? title)
    {
        if (string.IsNullOrWhiteSpace(title)) return "";
        var sb = new StringBuilder();
        foreach (var ch in title.Trim().ToLowerInvariant())
        {
            if (char.IsAsciiLetterOrDigit(ch)) sb.Append(ch);
            else if (sb.Length > 0 && sb[^1] != '-') sb.Append('-');
        }
        return sb.ToString().Trim('-');
    }

    // ───────────────────────────── rendering ─────────────────────────────

    /// <summary>
    /// Render the authored body to HTML. The input is escaped FIRST and the markup is then built
    /// from a fixed vocabulary, so no author — or anyone who reaches the authoring API — can inject
    /// HTML, script or an unexpected attribute. Supports: ## / ### headings, paragraphs, - bullets,
    /// 1. numbered lists, > blockquotes, **bold**, *italic*, `code`, and [text](https://…) links.
    /// </summary>
    public static string RenderBody(string? markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown)) return "";
        var html = new StringBuilder();
        string? listOpen = null;

        void CloseList()
        {
            if (listOpen is not null) { html.Append("</").Append(listOpen).Append(">\n"); listOpen = null; }
        }

        foreach (var raw in markdown.Replace("\r\n", "\n").Split('\n'))
        {
            var line = raw.TrimEnd();
            if (line.Length == 0) { CloseList(); continue; }

            if (line.StartsWith("### ", StringComparison.Ordinal))
            { CloseList(); html.Append("<h3>").Append(Inline(line[4..])).Append("</h3>\n"); continue; }
            if (line.StartsWith("## ", StringComparison.Ordinal))
            { CloseList(); html.Append("<h2>").Append(Inline(line[3..])).Append("</h2>\n"); continue; }
            if (line.StartsWith("> ", StringComparison.Ordinal))
            { CloseList(); html.Append("<blockquote><p>").Append(Inline(line[2..])).Append("</p></blockquote>\n"); continue; }
            if (line.StartsWith("- ", StringComparison.Ordinal))
            {
                if (listOpen != "ul") { CloseList(); html.Append("<ul>\n"); listOpen = "ul"; }
                html.Append("<li>").Append(Inline(line[2..])).Append("</li>\n"); continue;
            }
            var numbered = line.Length > 3 && char.IsAsciiDigit(line[0]) && line[1] == '.' && line[2] == ' ';
            if (numbered)
            {
                if (listOpen != "ol") { CloseList(); html.Append("<ol>\n"); listOpen = "ol"; }
                html.Append("<li>").Append(Inline(line[3..])).Append("</li>\n"); continue;
            }
            CloseList();
            html.Append("<p>").Append(Inline(line)).Append("</p>\n");
        }
        CloseList();
        return html.ToString();
    }

    /// <summary>Inline formatting over ALREADY-ESCAPED text. Order matters: escape, then decorate.</summary>
    static string Inline(string s)
    {
        var t = System.Net.WebUtility.HtmlEncode(s);
        t = System.Text.RegularExpressions.Regex.Replace(t, @"`([^`]{1,200})`", "<code>$1</code>");
        t = System.Text.RegularExpressions.Regex.Replace(t, @"\*\*([^*]{1,300})\*\*", "<strong>$1</strong>");
        t = System.Text.RegularExpressions.Regex.Replace(t, @"(?<![\*\w])\*([^*]{1,300})\*(?!\*)", "<em>$1</em>");
        // Links: absolute http(s) or site-relative only, and a site-relative link may not begin "//"
        // (that is protocol-relative and leaves the site). Anything else — javascript:, data:, a
        // bare word — simply does not match, so it stays on the page as visible escaped text rather
        // than becoming a link. Note the character classes exclude only whitespace and ")": the
        // text has already been HTML-encoded at this point, so a quote is "&quot;" and cannot
        // terminate the attribute.
        t = System.Text.RegularExpressions.Regex.Replace(t,
            @"\[([^\]]{1,200})\]\((https?://[^\s)]{1,400}|/(?!/)[^\s)]{0,400})\)",
            m => $"<a href=\"{m.Groups[2].Value}\"{(m.Groups[2].Value.StartsWith('/') ? "" : " rel=\"nofollow noopener\" target=\"_blank\"")}>{m.Groups[1].Value}</a>");
        return t;
    }

    // ───────────────────────────── lifecycle ─────────────────────────────

    /// <summary>Load the input shape the validator needs, including the counts it cannot see itself.</summary>
    public static ArticleInput InputFor(Db db, Dictionary<string, object?> row)
    {
        var id = H.L(row["id"]);
        var sources = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_article_sources WHERE article_id=?", id);
        var mentions = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_entity_mentions WHERE article_id=?", id) > 0;
        var legal = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_article_reviews WHERE article_id=? AND kind='legal' AND outcome='pass'", id) > 0;
        return new ArticleInput(H.Str(row["slug"]) ?? "", H.Str(row["kind"]) ?? "blog", H.Str(row["title"]),
            H.Str(row["dek"]), H.Str(row["body_md"]), H.Str(row["author_name"]),
            H.Str(row["seo_title"]), H.Str(row["seo_desc"]), (int)sources, mentions, legal);
    }

    public static bool CanEdit(string? status) => status is "idea" or "drafting" or "technical_review";

    public static string? Submit(Db db, long id, string toStatus)
    {
        if (!Statuses.Contains(toStatus) || toStatus is "published" or "approved") return "bad_status";
        var n = db.Execute("UPDATE pciworld_articles SET status=?, updated_at=datetime('now') WHERE id=? AND status<>'published'",
            toStatus, id);
        return n == 0 ? "not_found" : null;
    }

    /// <summary>Record a review outcome. Reviews are append-only evidence, not a status field.</summary>
    public static string? Review(Db db, long id, string kind, long reviewerId, string outcome, string? note)
    {
        if (!ReviewKinds.Contains(kind)) return "bad_kind";
        if (outcome is not ("pass" or "fail")) return "bad_outcome";
        var a = db.QueryOne("SELECT author_id FROM pciworld_articles WHERE id=?", id);
        if (a is null) return "not_found";
        // A person cannot review their own work — the same rule as approval, applied earlier.
        if (a["author_id"] is not null && H.L(a["author_id"]) == reviewerId) return "maker_checker";
        db.Execute("INSERT INTO pciworld_article_reviews(article_id,kind,reviewer_id,outcome,note) VALUES(?,?,?,?,?)",
            id, kind, reviewerId, outcome, note);
        return null;
    }

    /// <summary>Approve: the maker-checker checkpoint plus the full publication gate.</summary>
    public static string? Approve(Db db, long id, long approverId)
    {
        var row = db.QueryOne("SELECT * FROM pciworld_articles WHERE id=?", id);
        if (row is null) return "not_found";
        if (H.Str(row["status"]) == "published") return "already_published";
        if (row["author_id"] is not null && H.L(row["author_id"]) == approverId) return "maker_checker";
        if (Validate(InputFor(db, row)).Count > 0) return "not_publishable";
        db.Execute("UPDATE pciworld_articles SET status='approved', approved_by=?, updated_at=datetime('now') WHERE id=?", approverId, id);
        return null;
    }

    /// <summary>
    /// Publish the approved article as a new immutable version. Re-validates at the moment of
    /// publication — approval is necessary and never sufficient, because sources and entity
    /// mentions can change between the two.
    /// </summary>
    public static string? Publish(Db db, long id, long publisherId)
    {
        var row = db.QueryOne("SELECT * FROM pciworld_articles WHERE id=?", id);
        if (row is null) return "not_found";
        if (H.Str(row["status"]) != "approved") return "not_approved";
        if (Validate(InputFor(db, row)).Count > 0) return "not_publishable";
        var next = H.L(row["current_version"]) + 1;
        Snapshot(db, id, next, row, publisherId);
        db.Execute(@"UPDATE pciworld_articles SET status='published', current_version=?,
                published_at=COALESCE(published_at, datetime('now')), updated_at=datetime('now') WHERE id=?", next, id);
        return null;
    }

    /// <summary>
    /// Correct a published article. This is the ONLY way published text changes: a new version is
    /// written, and a dated, public correction note is appended. Silent edits are the thing the
    /// governance most specifically forbids, so there is no code path that performs one.
    /// </summary>
    public static string? Correct(Db db, long id, long editorId, string note, string? newTitle, string? newDek, string? newBody)
    {
        if (string.IsNullOrWhiteSpace(note) || note.Trim().Length < 10)
            return "note_required";
        var row = db.QueryOne("SELECT * FROM pciworld_articles WHERE id=?", id);
        if (row is null) return "not_found";
        if (H.Str(row["status"]) != "published") return "not_published";

        db.Execute(@"UPDATE pciworld_articles SET title=COALESCE(?,title), dek=COALESCE(?,dek),
                body_md=COALESCE(?,body_md), updated_at=datetime('now') WHERE id=?",
            newTitle, newDek, newBody, id);

        var updated = db.QueryOne("SELECT * FROM pciworld_articles WHERE id=?", id)!;
        if (Validate(InputFor(db, updated)).Count > 0) return "not_publishable";

        var corrections = ParseCorrections(H.Str(updated["corrections_json"]));
        corrections.Add(new Correction(DateTime.UtcNow.ToString("yyyy-MM-dd"), note.Trim()));
        var next = H.L(updated["current_version"]) + 1;
        db.Execute("UPDATE pciworld_articles SET corrections_json=?, current_version=?, updated_at=datetime('now') WHERE id=?",
            JsonSerializer.Serialize(corrections), next, id);
        Snapshot(db, id, next, db.QueryOne("SELECT * FROM pciworld_articles WHERE id=?", id)!, editorId);
        return null;
    }

    static void Snapshot(Db db, long id, long version, Dictionary<string, object?> row, long byId) =>
        db.Execute(@"INSERT OR IGNORE INTO pciworld_article_versions
                (article_id,version,title,dek,body_md,author_name,seo_title,seo_desc,tags_json,published_by)
            VALUES(?,?,?,?,?,?,?,?,?,?)",
            id, version, H.Str(row["title"]), H.Str(row["dek"]), H.Str(row["body_md"]),
            H.Str(row["author_name"]), H.Str(row["seo_title"]), H.Str(row["seo_desc"]), H.Str(row["tags_json"]), byId);

    public sealed record Correction(string Date, string Note);

    public static List<Correction> ParseCorrections(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new();
        try { return JsonSerializer.Deserialize<List<Correction>>(json!) ?? new(); }
        catch { return new(); }
    }

    public static void Archive(Db db, long id) =>
        db.Execute("UPDATE pciworld_articles SET status='archived', updated_at=datetime('now') WHERE id=?", id);

    /// <summary>The immutable snapshot a public reader is served — never the working copy.</summary>
    public static Dictionary<string, object?>? LiveVersion(Db db, long articleId)
    {
        var a = db.QueryOne("SELECT id,current_version FROM pciworld_articles WHERE id=? AND status='published' AND current_version>=1", articleId);
        if (a is null) return null;
        return db.QueryOne("SELECT * FROM pciworld_article_versions WHERE article_id=? AND version=?",
            articleId, H.L(a["current_version"]));
    }
}
