using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Server-side rendering of the public blog: the index (/blog, paginated + category/author filters) and
/// each article (/blog/{slug}). The FULL article HTML is present in the initial response (spec §5) — no
/// client-side JS is needed to read the content — with canonical URL, Open Graph/Twitter, and JSON-LD
/// (BlogPosting/Article + BreadcrumbList + Person + Organization) rendered here. The page chrome (header/
/// footer/nav) comes from the tokenised blog-shell.html and the live nav via ListSections.Inject, exactly
/// like CertPage. Draft/unpublished posts return null → the caller serves a 404 (protected from indexing).
/// </summary>
public static class BlogRender
{
    static string? Shell(string webRoot)
    {
        var p = Path.Combine(webRoot, "blog-shell.html");
        return File.Exists(p) ? File.ReadAllText(p) : null;
    }

    // Default site-wide keyword line + robots directive, used when a page supplies nothing more specific.
    const string DefaultKeywords = "project controls, project finance, project delivery, AI, PCL-AI, PFL-AI, PDL-AI, certification, professional development";
    const string DefaultRobots = "index, follow, max-image-preview:large";

    static string Compose(Db db, string webRoot, string lang, string slugForSeo, string title, string desc,
        string canonical, string ogType, string ogImage, string headExtra, string body,
        string? robots = null, string? ogTitle = null, string? ogDesc = null, string? keywords = null)
    {
        var shell = Shell(webRoot);
        if (shell is null) return body;   // template missing → still return content
        // The shell owns the single copy of each head tag (robots/og:title/og:description/keywords) via tokens,
        // so an article never emits a second, conflicting <meta>. og:title/description fall back to the page
        // title/description; robots defaults to indexable; keywords to the site-wide line.
        var html = shell
            .Replace("<!--PCI-BLOG-->", body)
            .Replace("{{ROBOTS}}", Esc(string.IsNullOrWhiteSpace(robots) ? DefaultRobots : robots))
            .Replace("{{OGTITLE}}", Esc(string.IsNullOrWhiteSpace(ogTitle) ? title : ogTitle))
            .Replace("{{OGDESC}}", Esc(string.IsNullOrWhiteSpace(ogDesc) ? desc : ogDesc))
            .Replace("{{TITLE}}", Esc(title))
            .Replace("{{DESC}}", Esc(desc))
            .Replace("{{CANONICAL}}", Esc(canonical))
            .Replace("{{OGTYPE}}", Esc(ogType))
            .Replace("{{OGIMAGE}}", Esc(ogImage))
            .Replace("{{KEYWORDS}}", Esc(string.IsNullOrWhiteSpace(keywords) ? DefaultKeywords : keywords))
            .Replace("{{HEAD_EXTRA}}", headExtra);
        html = ListSections.Inject(db, html, lang);                 // live header/footer nav
        html = SeoTags.Inject(db, html, slugForSeo);                // admin analytics + verification metas
        return html;
    }

    static string AbsImage(Db db, string? img)
    {
        if (string.IsNullOrWhiteSpace(img)) return "https://projectcontrolsinstitute.org/assets/og-image.jpg";
        return img.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? img : Redirects.CanonicalBase + (img.StartsWith("/") ? "" : "/") + img;
    }

    // ── Article resolver (section-aware) ───────────────────────────────────────────────────────────────
    /// <summary>
    /// Resolve an article request within a section (blog or news). Returns the rendered HTML, OR a redirect
    /// target when the post actually belongs to the OTHER section (each post has exactly one canonical home,
    /// so a wrong-section URL 301s to the right one), OR (null,null) for a 404 (draft/unknown slug).
    /// </summary>
    public static (string? html, string? redirect) Article(Db db, string webRoot, string slug, string lang, bool sectionIsNews)
    {
        var p = Blog.PublishedBySlug(db, slug);
        if (p is null) return (null, null);
        var postIsNews = Blog.IsNews(p);
        if (postIsNews != sectionIsNews) return (null, Blog.BasePathFor(db, postIsNews) + "/" + slug);
        return (RenderArticle(db, webRoot, slug, lang), null);
    }

    // ── Article ──────────────────────────────────────────────────────────────────────────────────────
    public static string? RenderArticle(Db db, string webRoot, string slug, string lang)
    {
        var p = Blog.PublishedBySlug(db, slug);
        if (p is null) return null;
        var news = Blog.IsNews(p);                          // NewsArticle → /news space; else /blog
        var basePath = Blog.BasePathFor(db, news);
        var canonical = H.Str(p["canonical_url"]) is { Length: > 0 } cu ? cu : Blog.PublicUrl(db, slug, news);
        var title = (H.Str(p["seo_title"]) ?? H.Str(p["title"]) ?? "") + " | Project Controls Institute";
        var desc = H.Str(p["meta_description"]) ?? H.Str(p["summary"]) ?? "";
        var author = Blog.Author(db, p["author_id"]);
        var category = Blog.Category(db, p["category_id"]);
        var tags = Blog.Tags(db, H.L(p["id"]));
        var ogImage = AbsImage(db, H.Str(p["og_image"]) ?? H.Str(p["social_image"]) ?? H.Str(p["featured_image"]));
        var noindex = H.L(p["robots_noindex"]) == 1;

        // robots + og:title + og:description + keywords are emitted once by the shell via Compose tokens
        // (no second, conflicting <meta> here). og:title uses og_title, falling back to the post title;
        // og:description uses og_description, falling back to the meta description/summary.
        var robots = noindex ? "noindex, follow" : DefaultRobots;
        var ogTitle = H.Str(p["og_title"]) ?? H.Str(p["title"]) ?? "";
        var ogDesc = H.Str(p["og_description"]) ?? desc;
        var keywords = KeywordLine(p, tags);

        var head = new StringBuilder();
        if (H.Str(p["published_at"]) is { Length: > 0 } pub)
            head.Append("<meta property=\"article:published_time\" content=\"").Append(Esc(Iso(pub))).Append("\"/>");
        if (H.Str(p["updated_at"]) is { Length: > 0 } upd)
            head.Append("<meta property=\"article:modified_time\" content=\"").Append(Esc(Iso(upd))).Append("\"/>");
        if (author is not null) head.Append("<meta name=\"author\" content=\"").Append(Esc(H.Str(author["name"]) ?? "")).Append("\"/>");
        head.Append(JsonLd(db, p, author, category, canonical, basePath, ogImage, news));

        return Compose(db, webRoot, lang, (news ? "news/" : "blog/") + slug + ".html", title, desc, canonical,
            "article", ogImage, head.ToString(), ArticleBody(db, p, author, category, tags, basePath, news),
            robots: robots, ogTitle: ogTitle, ogDesc: ogDesc, keywords: keywords);
    }

    static string ArticleBody(Db db, Dictionary<string, object?> p, Dictionary<string, object?>? author,
        Dictionary<string, object?>? category, List<Dictionary<string, object?>> tags, string basePath, bool news = false)
    {
        var sb = new StringBuilder();
        var title = H.Str(p["title"]) ?? "";
        var subtitle = H.Str(p["subtitle"]) ?? "";
        var summary = H.Str(p["summary"]) ?? "";
        var readMin = (int)H.L(p["reading_time"]); if (readMin <= 0) readMin = Blog.ReadingTime(H.Str(p["body"]));
        var catName = category is null ? "" : H.Str(category["name"]) ?? "";
        var catSlug = category is null ? "" : H.Str(category["slug"]) ?? "";
        var authorName = author is null ? "" : H.Str(author["name"]) ?? "";

        sb.Append("<article class=\"sec\"><div class=\"wrap\" style=\"max-width:820px\">");
        // breadcrumb
        sb.Append("<div class=\"crumbbar-inline\"><a href=\"").Append(basePath).Append("\">").Append(Blog.SectionLabel(news)).Append("</a>");
        if (catName.Length > 0) sb.Append(" · <a href=\"").Append(basePath).Append("/category/").Append(Esc(catSlug)).Append("\">").Append(Esc(catName)).Append("</a>");
        sb.Append("</div>");
        if (catName.Length > 0) sb.Append("<span class=\"eyebrow\">").Append(Esc(catName)).Append("</span>");
        sb.Append("<h1>").Append(Esc(title)).Append("</h1>");
        if (subtitle.Length > 0) sb.Append("<p class=\"lead\">").Append(Esc(subtitle)).Append("</p>");
        // meta row
        sb.Append("<p class=\"blog-meta\" style=\"color:#64748b;font-size:.95rem\">");
        if (authorName.Length > 0) sb.Append("By <strong>").Append(Esc(authorName)).Append("</strong> · ");
        if (H.Str(p["published_at"]) is { Length: > 0 } pub) sb.Append("<time datetime=\"").Append(Esc(Iso(pub))).Append("\">").Append(Esc(FmtDate(pub))).Append("</time> · ");
        sb.Append(readMin).Append(" min read");
        if (H.L(p["ai_assisted"]) == 1 && H.Str(p["ai_disclosure"]) is { Length: > 0 } aid) sb.Append(" · <em>").Append(Esc(aid)).Append("</em>");
        sb.Append("</p>");
        // featured image
        if (H.Str(p["featured_image"]) is { Length: > 0 } fi)
            sb.Append("<img src=\"").Append(Esc(fi)).Append("\" alt=\"").Append(Esc(H.Str(p["featured_image_alt"]) ?? title)).Append("\" style=\"width:100%;border-radius:12px;margin:1rem 0\" loading=\"lazy\"/>");
        // summary / executive lead
        if (summary.Length > 0) sb.Append("<div class=\"blog-summary\" style=\"font-size:1.08rem;color:#334155;border-left:3px solid #1D4ED8;padding-left:1rem;margin:1.2rem 0\">").Append(Esc(summary)).Append("</div>");
        // body (sanitised at render as defence-in-depth; markdown converted if needed)
        var bodyHtml = BodyHtml(H.Str(p["body"]), H.Str(p["body_format"]));
        sb.Append("<div class=\"blog-body\">").Append(bodyHtml).Append("</div>");
        // attribution / license (for syndicated or licensed content)
        if (H.Str(p["attribution"]) is { Length: > 0 } attr)
            sb.Append("<p class=\"blog-attribution\" style=\"color:#64748b;font-size:.9rem;margin-top:1.5rem\">").Append(Esc(attr)).Append("</p>");
        if (H.Str(p["original_source_url"]) is { Length: > 0 } src)
            sb.Append("<p style=\"font-size:.9rem\"><a href=\"").Append(Esc(src)).Append("\" rel=\"nofollow\">Originally published at the source</a></p>");
        // tags
        if (tags.Count > 0)
        {
            sb.Append("<div class=\"blog-tags\" style=\"margin-top:1.5rem\">");
            foreach (var t in tags) sb.Append("<a href=\"").Append(basePath).Append("/tag/").Append(Esc(H.Str(t["slug"]) ?? "")).Append("\" class=\"tag-pill\" style=\"display:inline-block;background:#eef2ff;color:#1D4ED8;padding:.25rem .7rem;border-radius:999px;font-size:.85rem;margin:.2rem\">#").Append(Esc(H.Str(t["name"]) ?? "")).Append("</a>");
            sb.Append("</div>");
        }
        // author bio
        if (author is not null && H.Str(author["bio"]) is { Length: > 0 } bio)
        {
            sb.Append("<div class=\"blog-author\" style=\"margin-top:2rem;padding:1.2rem;background:#f8fafc;border-radius:12px\">");
            sb.Append("<strong>").Append(Esc(authorName)).Append("</strong>");
            if (H.Str(author["title"]) is { Length: > 0 } at) sb.Append(" — ").Append(Esc(at));
            sb.Append("<p style=\"margin:.4rem 0 0;color:#475569\">").Append(Esc(bio)).Append("</p></div>");
        }
        sb.Append("<p style=\"margin-top:2rem\"><a class=\"btn btn-ghost\" href=\"").Append(basePath).Append("\">← All ").Append(news ? "news" : "articles").Append("</a></p>");
        sb.Append("</div></article>");
        return sb.ToString();
    }

    // ── Index / listing ──────────────────────────────────────────────────────────────────────────────
    public static string RenderIndex(Db db, string webRoot, string lang, int page,
        long? categoryId, string? categoryLabel, long? authorId, string? authorLabel,
        long? tagId = null, string? tagLabel = null, bool news = false)
    {
        var basePath = Blog.BasePathFor(db, news);
        var per = Blog.PerPage(db);
        if (page < 1) page = 1;
        var posts = Blog.ListPublished(db, per, (page - 1) * per, categoryId, authorId, tagId, null, news);
        var total = tagId is not null ? posts.Count + (page - 1) * per + (posts.Count == per ? per : 0)
                                      : Blog.CountPublished(db, categoryId, authorId, null, news);
        var heading = categoryLabel ?? authorLabel ?? tagLabel ?? (news ? "News & Updates" : "Blog & Insights");
        var canonical = Redirects.CanonicalBase + basePath + (categoryId is not null ? "/category/" + Slug(categoryLabel) : "") + (page > 1 ? "?page=" + page : "");
        var desc = news
            ? "News, announcements and industry updates on project controls, project finance and project delivery from the Project Controls Institute."
            : "Project controls, project finance, project delivery and AI insights, research and certification news from the Project Controls Institute.";

        var sb = new StringBuilder();
        sb.Append("<section class=\"phead\"><div class=\"wrap\">");
        sb.Append("<span class=\"eyebrow\">").Append(news ? "Newsroom" : "Insights").Append("</span><h1>").Append(Esc(heading)).Append("</h1>");
        sb.Append("<p class=\"phead-lead\">").Append(Esc(desc)).Append("</p></div></section>");

        sb.Append("<section class=\"sec\"><div class=\"wrap\">");
        if (posts.Count == 0)
            sb.Append("<p class=\"lead\">No ").Append(news ? "news" : "articles").Append(" published yet. Check back soon.</p>");
        else
        {
            sb.Append("<div class=\"blog-grid\" style=\"display:grid;grid-template-columns:repeat(auto-fill,minmax(300px,1fr));gap:1.5rem\">");
            foreach (var p in posts) sb.Append(Card(db, p, basePath));
            sb.Append("</div>");
            // pagination
            var pages = (int)Math.Ceiling(total / (double)per);
            if (pages > 1)
            {
                sb.Append("<nav class=\"blog-pager\" style=\"margin-top:2rem;display:flex;gap:.5rem;justify-content:center\" aria-label=\"Pagination\">");
                if (page > 1) sb.Append("<a class=\"btn btn-ghost\" href=\"?page=").Append(page - 1).Append("\">← Newer</a>");
                sb.Append("<span style=\"padding:.5rem\">Page ").Append(page).Append(" of ").Append(pages).Append("</span>");
                if (page < pages) sb.Append("<a class=\"btn btn-ghost\" href=\"?page=").Append(page + 1).Append("\">Older →</a>");
                sb.Append("</nav>");
            }
        }
        sb.Append("</div></section>");

        // ItemList JSON-LD for the index. Paginated pages (page>1) are noindex via the shell robots token
        // (single tag) so thin duplicate listings aren't indexed; the og:title is the clean facet heading.
        var head = new StringBuilder();
        head.Append(IndexJsonLd(db, posts, basePath));

        return Compose(db, webRoot, lang, news ? "news.html" : "blog.html", heading + " | Project Controls Institute", desc,
            canonical, "website", "https://projectcontrolsinstitute.org/assets/og-image.jpg", head.ToString(), sb.ToString(),
            robots: page > 1 ? "noindex, follow" : DefaultRobots, ogTitle: heading);
    }

    static string Card(Db db, Dictionary<string, object?> p, string basePath)
    {
        var slug = H.Str(p["slug"]) ?? "";
        var title = H.Str(p["title"]) ?? "";
        var summary = H.Str(p["summary"]) ?? "";
        var cat = Blog.Category(db, p["category_id"]);
        var author = Blog.Author(db, p["author_id"]);
        var readMin = (int)H.L(p["reading_time"]); if (readMin <= 0) readMin = Blog.ReadingTime(H.Str(p["body"]));
        var sb = new StringBuilder();
        sb.Append("<article class=\"qcard\" style=\"display:flex;flex-direction:column;overflow:hidden\">");
        if (H.Str(p["featured_image"]) is { Length: > 0 } fi)
            sb.Append("<a href=\"").Append(basePath).Append("/").Append(Esc(slug)).Append("\"><img src=\"").Append(Esc(fi)).Append("\" alt=\"").Append(Esc(H.Str(p["featured_image_alt"]) ?? title)).Append("\" style=\"width:100%;height:170px;object-fit:cover\" loading=\"lazy\"/></a>");
        sb.Append("<div style=\"padding:1.1rem;display:flex;flex-direction:column;gap:.5rem\">");
        if (cat is not null) sb.Append("<span class=\"eyebrow\" style=\"font-size:.75rem\">").Append(Esc(H.Str(cat["name"]) ?? "")).Append("</span>");
        sb.Append("<h3 style=\"margin:0\"><a href=\"").Append(basePath).Append("/").Append(Esc(slug)).Append("\" style=\"color:inherit;text-decoration:none\">").Append(Esc(title)).Append("</a></h3>");
        if (summary.Length > 0) sb.Append("<p style=\"color:#475569;margin:0;font-size:.95rem\">").Append(Esc(Trunc(summary, 140))).Append("</p>");
        sb.Append("<p style=\"color:#94a3b8;font-size:.82rem;margin:.3rem 0 0\">");
        if (author is not null) sb.Append(Esc(H.Str(author["name"]) ?? "")).Append(" · ");
        if (H.Str(p["published_at"]) is { Length: > 0 } pub) sb.Append(Esc(FmtDate(pub))).Append(" · ");
        sb.Append(readMin).Append(" min");
        sb.Append("</p></div></article>");
        return sb.ToString();
    }

    // ── JSON-LD ──────────────────────────────────────────────────────────────────────────────────────
    static string JsonLd(Db db, Dictionary<string, object?> p, Dictionary<string, object?>? author,
        Dictionary<string, object?>? category, string canonical, string basePath, string ogImage, bool news = false)
    {
        var type = H.Str(p["structured_type"]) is { Length: > 0 } st ? st : "BlogPosting";
        var posting = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = type,
            ["headline"] = H.Str(p["title"]),
            ["description"] = H.Str(p["meta_description"]) ?? H.Str(p["summary"]),
            // Model the article image as an ImageObject (url + caption) rather than a bare URL string, so
            // rich-result parsers get a typed image. Dimensions are omitted (not known reliably per-post).
            ["image"] = new Dictionary<string, object?> { ["@type"] = "ImageObject", ["url"] = ogImage,
                ["caption"] = H.Str(p["featured_image_alt"]) ?? H.Str(p["title"]) },
            ["datePublished"] = H.Str(p["published_at"]) is { Length: > 0 } pub ? Iso(pub) : null,
            ["dateModified"] = H.Str(p["updated_at"]) is { Length: > 0 } upd ? Iso(upd) : null,
            ["inLanguage"] = H.Str(p["language"]) ?? "en",
            ["mainEntityOfPage"] = new Dictionary<string, object?> { ["@type"] = "WebPage", ["@id"] = canonical },
            ["author"] = author is null
                ? new Dictionary<string, object?> { ["@type"] = "Organization", ["name"] = "Project Controls Institute" }
                : new Dictionary<string, object?> { ["@type"] = "Person", ["name"] = H.Str(author["name"]),
                    ["url"] = Redirects.CanonicalBase + basePath + "/author/" + (H.Str(author["slug"]) ?? "") },
            ["publisher"] = new Dictionary<string, object?>
            {
                ["@type"] = "Organization", ["name"] = "Project Controls Institute",
                ["logo"] = new Dictionary<string, object?> { ["@type"] = "ImageObject", ["url"] = "https://projectcontrolsinstitute.org/assets/apple-touch-icon.png" }
            },
        };
        // BreadcrumbList: Home > Blog > (Category) > Title
        var crumbs = new List<object?>();
        int pos = 1;
        crumbs.Add(Crumb(pos++, "Home", Redirects.CanonicalBase + "/"));
        crumbs.Add(Crumb(pos++, Blog.SectionLabel(news), Redirects.CanonicalBase + basePath));
        if (category is not null) crumbs.Add(Crumb(pos++, H.Str(category["name"]) ?? "", Redirects.CanonicalBase + basePath + "/category/" + (H.Str(category["slug"]) ?? "")));
        crumbs.Add(Crumb(pos++, H.Str(p["title"]) ?? "", canonical));
        var breadcrumb = new Dictionary<string, object?> { ["@context"] = "https://schema.org", ["@type"] = "BreadcrumbList", ["itemListElement"] = crumbs };

        return "<script type=\"application/ld+json\">" + JsonSerializer.Serialize(posting, JsonOpts) + "</script>"
             + "<script type=\"application/ld+json\">" + JsonSerializer.Serialize(breadcrumb, JsonOpts) + "</script>";
    }

    static string IndexJsonLd(Db db, List<Dictionary<string, object?>> posts, string basePath)
    {
        var items = new List<object?>();
        int pos = 1;
        foreach (var p in posts)
            items.Add(new Dictionary<string, object?> { ["@type"] = "ListItem", ["position"] = pos++,
                ["url"] = Redirects.CanonicalBase + basePath + "/" + (H.Str(p["slug"]) ?? ""), ["name"] = H.Str(p["title"]) });
        var list = new Dictionary<string, object?> { ["@context"] = "https://schema.org", ["@type"] = "ItemList", ["itemListElement"] = items };
        return "<script type=\"application/ld+json\">" + JsonSerializer.Serialize(list, JsonOpts) + "</script>";
    }

    static Dictionary<string, object?> Crumb(int pos, string name, string url) =>
        new() { ["@type"] = "ListItem", ["position"] = pos, ["name"] = name, ["item"] = url };

    static readonly JsonSerializerOptions JsonOpts = new() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };

    // ── body formatting (html sanitised / markdown-lite → sanitised) ──────────────────────────────────
    static string BodyHtml(string? body, string? format)
    {
        if (string.IsNullOrWhiteSpace(body)) return "";
        var html = (format ?? "html").ToLowerInvariant() == "markdown" ? MarkdownLite(body) : body;
        return HtmlSanitize.Clean(html);
    }

    /// <summary>Minimal, safe Markdown → HTML (headings, bold, italic, links, lists, paragraphs). Output is
    /// sanitised by the caller, so this only needs to produce structural HTML.</summary>
    static string MarkdownLite(string md)
    {
        var lines = md.Replace("\r\n", "\n").Split('\n');
        var sb = new StringBuilder(); bool inList = false;
        void CloseList() { if (inList) { sb.Append("</ul>"); inList = false; } }
        foreach (var raw in lines)
        {
            var line = raw.TrimEnd();
            if (line.Length == 0) { CloseList(); continue; }
            var esc = Inline(line);
            if (line.StartsWith("### ")) { CloseList(); sb.Append("<h3>").Append(Inline(line[4..])).Append("</h3>"); }
            else if (line.StartsWith("## ")) { CloseList(); sb.Append("<h2>").Append(Inline(line[3..])).Append("</h2>"); }
            else if (line.StartsWith("# ")) { CloseList(); sb.Append("<h2>").Append(Inline(line[2..])).Append("</h2>"); }
            else if (line.StartsWith("- ") || line.StartsWith("* ")) { if (!inList) { sb.Append("<ul>"); inList = true; } sb.Append("<li>").Append(Inline(line[2..])).Append("</li>"); }
            else { CloseList(); sb.Append("<p>").Append(esc).Append("</p>"); }
        }
        CloseList();
        return sb.ToString();
    }

    static string Inline(string s)
    {
        s = Esc(s);
        s = System.Text.RegularExpressions.Regex.Replace(s, @"\[([^\]]+)\]\((https?://[^)\s]+)\)", "<a href=\"$2\" rel=\"noopener\">$1</a>");
        s = System.Text.RegularExpressions.Regex.Replace(s, @"\*\*([^*]+)\*\*", "<strong>$1</strong>");
        s = System.Text.RegularExpressions.Regex.Replace(s, @"(?<!\*)\*([^*]+)\*(?!\*)", "<em>$1</em>");
        return s;
    }

    // ── small utils ──────────────────────────────────────────────────────────────────────────────────
    static string Iso(string dt) => dt.Contains('T') ? dt : dt.Replace(' ', 'T') + "Z";
    static string FmtDate(string dt)
    {
        return DateTime.TryParse(dt, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out var d)
            ? d.ToString("d MMM yyyy", System.Globalization.CultureInfo.InvariantCulture) : dt;
    }
    // Build the <meta name="keywords"> line for an article from its editorial SEO fields + tags, so the
    // stored primary_keyword/secondary_keywords are actually surfaced. Dedup (case-insensitive), capped at
    // 12 terms; null → Compose falls back to the site-wide default line.
    static string? KeywordLine(Dictionary<string, object?> p, List<Dictionary<string, object?>> tags)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var outp = new List<string>();
        void Add(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return;
            foreach (var part in s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                if (outp.Count < 12 && seen.Add(part)) outp.Add(part);
        }
        Add(H.Str(p["primary_keyword"]));
        Add(H.Str(p["secondary_keywords"]));
        foreach (var t in tags) Add(H.Str(t["name"]));
        return outp.Count > 0 ? string.Join(", ", outp) : null;
    }

    static string Slug(string? s) => Blog.Slugify(s ?? "");
    static string Trunc(string s, int n) => s.Length <= n ? s : s[..n].TrimEnd() + "…";
    static string Esc(string? s) => (s ?? "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
}
