using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The public, crawlable forum read path (docs/pciworld/CCP_PHASE3_DESIGN.md §6; §18.3, D-009).
///
/// SERVER-RENDERED ON PURPOSE. §18.3 requires crawlable forum pages and D-009 keeps public SEO
/// surfaces out of React — a thread that only exists after JavaScript runs is a thread that, for
/// practical purposes, is not in the index. The authenticated composer and the moderation console
/// are React; this is not.
///
/// THE ONE RULE THIS FILE MUST NOT BREAK.
///
/// Only <c>published</c> posts render. A pending, withheld, withdrawn or deleted post must never
/// appear in the HTML, and the reason it matters more here than anywhere else in the increment is
/// that this output is CACHED BY SEARCH ENGINES. A leak in a transient chat transcript is over when
/// the message is removed; a leak here can outlive the deletion by months, in a cache nobody at PCI
/// controls. So the filter is in the SQL rather than applied afterwards in a loop somebody might
/// later refactor, and there is a test that renders a thread containing one post of every state and
/// asserts that only the published one is present.
///
/// THE STRUCTURED DATA MUST NOT CLAIM MORE THAN IS TRUE.
///
/// A thread emits <c>QAPage</c> with an <c>acceptedAnswer</c> ONLY when it genuinely has one, and
/// only when that answer is itself still published — an accepted answer that was later withdrawn
/// must stop being advertised as the answer. Otherwise it emits <c>DiscussionForumPosting</c>.
/// Telling a search engine there is an accepted answer when there is not is a lie to a machine that
/// will repeat it to people.
/// </summary>
public static class ForumRender
{
    // The DEFAULT encoder, deliberately — not UnsafeRelaxedJsonEscaping.
    //
    // This JSON is embedded inside <script type="application/ld+json">, and thread titles are
    // written by members. The relaxed encoder leaves '<' and '>' as literals, so a title containing
    // </script> closes the tag early and everything after it becomes live markup. The default
    // encoder emits \u003C, which is inert inside the script block and still valid JSON-LD to
    // every crawler. Found by a test that fed a title of exactly that shape.
    //
    // (BlogRender, which this file's structure otherwise follows, already uses the default encoder —
    // the relaxed one was my own addition here, not something inherited.)
    static readonly JsonSerializerOptions JsonOpts = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// Render one thread. Returns null for anything the public may not see — a missing thread, a
    /// hidden one, a hidden category, or the forum being switched off — so the caller answers 404
    /// rather than having to know the rules.
    /// </summary>
    public static string? Thread(Db db, string categorySlug, string threadSlug, string canonicalBase = "")
    {
        if (!ForumPosts.Enabled(db)) return null;

        var thread = db.QueryOne(
            @"SELECT t.id, t.title, t.slug, t.state, t.solved_post_id, t.created_at, t.view_count,
                     c.slug AS category_slug, c.title AS category_title, c.state AS category_state
              FROM pciworld_forum_threads t
              JOIN pciworld_forum_categories c ON c.id = t.category_id
              WHERE c.slug=? AND t.slug=?", categorySlug, threadSlug);
        if (thread is null) return null;
        // Archived and locked threads still READ — locking stops new posts, it does not unpublish a
        // discussion. Hidden does not read, in either the thread or its category.
        if (H.Str(thread["state"]) == "hidden") return null;
        if (H.Str(thread["category_state"]) is "hidden") return null;

        var threadId = H.L(thread["id"]);

        // The state filter lives HERE, in the query, not in a later loop. Anything that renders has
        // already been through it, so a refactor of the presentation cannot accidentally widen it.
        var posts = db.Query(
            @"SELECT p.id, p.kind, p.published_at, p.author_user_id,
                     r.body_rendered, r.revision_no
              FROM pciworld_forum_posts p
              JOIN pciworld_forum_post_revisions r ON r.id = p.current_revision_id
              WHERE p.thread_id=? AND p.state='published'
              ORDER BY CASE WHEN p.kind='opening' THEN 0 ELSE 1 END, p.id",
            threadId);

        if (posts.Count == 0) return null;   // nothing publishable — a bare title is not a page

        var solvedId = thread["solved_post_id"] is null ? (long?)null : H.L(thread["solved_post_id"]);
        // An accepted answer that is no longer published stops being the accepted answer. Checking
        // membership of the already-filtered set is what makes that automatic.
        var solved = solvedId is { } sid && posts.Any(p => H.L(p["id"]) == sid)
            ? posts.First(p => H.L(p["id"]) == sid) : null;

        var title = H.Str(thread["title"]) ?? "Discussion";
        var canonical = $"{canonicalBase}/world/forum/{Uri.EscapeDataString(categorySlug)}/{Uri.EscapeDataString(threadSlug)}";

        var sb = new StringBuilder();
        sb.Append("<article class=\"wf-thread\">");
        sb.Append("<nav class=\"wf-crumbs\" aria-label=\"Breadcrumb\"><ol>");
        sb.Append("<li><a href=\"/world/forum\">Forum</a></li>");
        sb.Append($"<li><a href=\"/world/forum/{Esc(categorySlug)}\">{Esc(H.Str(thread["category_title"]) ?? categorySlug)}</a></li>");
        sb.Append($"<li aria-current=\"page\">{Esc(title)}</li>");
        sb.Append("</ol></nav>");
        sb.Append($"<h1>{Esc(title)}</h1>");

        if (H.Str(thread["state"]) == "locked")
            // Stated in text, not by a padlock glyph alone — the same rule the rest of this
            // increment follows for state.
            sb.Append("<p class=\"wf-locked\" role=\"status\">This discussion is closed to new replies.</p>");

        sb.Append("<ol class=\"wf-posts\">");
        foreach (var p in posts)
        {
            var isSolved = solved is not null && H.L(p["id"]) == H.L(solved["id"]);
            sb.Append(isSolved ? "<li class=\"wf-post wf-solved\">" : "<li class=\"wf-post\">");
            if (isSolved) sb.Append("<p class=\"wf-badge\">Accepted answer</p>");
            // body_rendered was sanitised at WRITE time (ForumPosts.WriteRevision), so the read path
            // never renders unsanitised author input and a later change to the sanitiser cannot
            // retroactively expose an old post.
            sb.Append("<div class=\"wf-body\">").Append(H.Str(p["body_rendered"]) ?? "").Append("</div>");
            if (H.L(p["revision_no"]) > 1)
                // Edits are disclosed. A silently edited post is how quote-mining works.
                sb.Append("<p class=\"wf-edited\">Edited</p>");
            sb.Append("</li>");
        }
        sb.Append("</ol></article>");
        sb.Append(JsonLd(title, canonical, thread, posts, solved));
        return Document(title, Summary(posts), canonical, sb.ToString());
    }

    /// <summary>
    /// Wrap the article in a COMPLETE document.
    ///
    /// The first version of this returned a bare fragment, which is a mistake worth naming: a page
    /// with no &lt;title&gt;, no lang attribute and no canonical is not "crawlable" in any sense
    /// that matters. A crawler indexes it under whatever it can guess, a screen reader announces an
    /// untitled document, and axe flags html-has-lang and document-title immediately. Calling the
    /// fragment a crawlable page would have been the kind of claim that survives review because the
    /// word "crawlable" appears in a comment next to it.
    ///
    /// Self-contained rather than composed into the Institute blog shell: this surface is served by
    /// World-only deployments too, and inheriting that shell would put navigation to a site those
    /// deployments do not serve on every forum page.
    /// </summary>
    static string Document(string title, string description, string canonical, string body) =>
        "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"utf-8\"/>"
        + "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"/>"
        + $"<title>{Esc(title)} — PCI World forum</title>"
        + $"<meta name=\"description\" content=\"{Esc(description)}\"/>"
        + $"<link rel=\"canonical\" href=\"{Esc(canonical)}\"/>"
        + "<meta name=\"robots\" content=\"index, follow\"/>"
        + "<link rel=\"stylesheet\" href=\"/assets/world.css\"/>"
        + "</head><body>"
        // A skip link: a thread page is mostly a long list, and a keyboard user should not have to
        // traverse the breadcrumb on every one.
        + "<a class=\"wf-skip\" href=\"#wf-main\">Skip to the discussion</a>"
        + "<main id=\"wf-main\">" + body + "</main>"
        + "</body></html>";

    /// <summary>A meta description drawn from the opening post — the words that actually answer
    /// "what is this thread about", rather than a generic site line.</summary>
    static string Summary(List<Dictionary<string, object?>> posts)
    {
        var opening = posts.FirstOrDefault(p => H.Str(p["kind"]) == "opening") ?? posts[0];
        var text = Strip(H.Str(opening["body_rendered"]));
        return text.Length > 160 ? text[..157].TrimEnd() + "…" : text;
    }

    /// <summary>
    /// Structured data. <c>QAPage</c> only when there is a genuinely published accepted answer;
    /// otherwise <c>DiscussionForumPosting</c>. Plus a breadcrumb trail either way.
    /// </summary>
    static string JsonLd(string title, string canonical, Dictionary<string, object?> thread,
                         List<Dictionary<string, object?>> posts, Dictionary<string, object?>? solved)
    {
        var opening = posts.FirstOrDefault(p => H.Str(p["kind"]) == "opening") ?? posts[0];
        var created = H.Str(thread["created_at"]);

        object main = solved is not null
            ? new
            {
                context = "https://schema.org",
                type = "QAPage",
                mainEntity = new
                {
                    type = "Question",
                    name = title,
                    text = Strip(H.Str(opening["body_rendered"])),
                    dateCreated = created,
                    answerCount = posts.Count - 1,
                    acceptedAnswer = new
                    {
                        type = "Answer",
                        text = Strip(H.Str(solved["body_rendered"])),
                        dateCreated = H.Str(solved["published_at"]),
                        url = canonical,
                    },
                },
            }
            : new
            {
                context = "https://schema.org",
                type = "DiscussionForumPosting",
                headline = title,
                text = Strip(H.Str(opening["body_rendered"])),
                datePublished = H.Str(opening["published_at"]) ?? created,
                url = canonical,
                // Only posts that actually rendered are counted. Counting withheld replies would
                // advertise a livelier discussion than exists.
                commentCount = posts.Count - 1,
            };

        var crumbs = new
        {
            context = "https://schema.org",
            type = "BreadcrumbList",
            itemListElement = new object[]
            {
                new { type = "ListItem", position = 1, name = "Forum", item = "/world/forum" },
                new { type = "ListItem", position = 2, name = H.Str(thread["category_title"]), item = "/world/forum/" + H.Str(thread["category_slug"]) },
                new { type = "ListItem", position = 3, name = title, item = canonical },
            },
        };

        return Ld(main) + Ld(crumbs);
    }

    // schema.org uses '@context'/'@type'; anonymous types cannot carry '@', so they are renamed on
    // the way out. A string replace on the serialised JSON rather than custom converters — one line,
    // and the shape stays readable above.
    static string Ld(object o) =>
        "<script type=\"application/ld+json\">"
        + JsonSerializer.Serialize(o, JsonOpts).Replace("\"context\":", "\"@context\":").Replace("\"type\":", "\"@type\":")
        + "</script>";

    /// <summary>Plain text for structured data. Tags out, entities decoded, length bounded — a
    /// JSON-LD blob carrying an entire thread helps nobody and bloats every page.</summary>
    static string Strip(string? html)
    {
        if (string.IsNullOrEmpty(html)) return "";
        var text = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", " ");
        text = System.Net.WebUtility.HtmlDecode(text);
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
        return text.Length > 1200 ? text[..1200] : text;
    }

    static string Esc(string? s) => System.Net.WebUtility.HtmlEncode(s ?? "");
}
