using System.Text;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// AI Visibility (master-plan Phase 6 — Generative Engine Optimization). Makes PCI content
/// discoverable and citable by AI answer engines (ChatGPT, Claude, Perplexity, Gemini, Copilot…)
/// while giving the admin explicit control over which AI crawlers may access the site.
///
/// Three fully self-contained capabilities, no external credentials required:
///   • <see cref="Robots"/>   — a policy-aware /robots.txt. Answer engines are allowed by default
///                              (so PCI can be cited); crawlers the admin blocks get an explicit
///                              "Disallow: /" group. Allowed bots have NO dedicated group, so they
///                              fall through to the "User-agent: *" rules and still honour the
///                              private-path disallows (robots.txt: a bot obeys only its most
///                              specific matching group, so an explicit Allow group would detach it).
///   • <see cref="LlmsTxt"/>  — /llms.txt, the emerging llmstxt.org convention: a curated Markdown
///                              map of the site's indexable pages, generated live from the pages
///                              table so it always reflects real content.
///   • <see cref="Detect"/>   — recognises known AI-crawler user-agents so their visits can be
///                              recorded (Analytics) and surfaced in Admin → AI Visibility.
///
/// The block policy is stored in site_settings (key <c>ai_bot_blocklist</c>, comma-separated
/// tokens); default empty = allow all. Cached and rebuilt on content edits or policy changes.
/// </summary>
public static class AiVisibility
{
    /// <summary>A known AI crawler. <paramref name="Ua"/> is the substring matched in a request's
    /// User-Agent for analytics; null means the crawler fetches under a shared UA (e.g. Google-Extended
    /// fetches as Googlebot) so it is robots-controllable but not separately detectable in traffic.</summary>
    public sealed record Crawler(string Token, string Label, string Vendor, string Purpose, string? Ua);

    // Purpose: "answer" feeds a live AI answer/search product (citation opportunity),
    // "training" feeds model training, "both" does both. Ordered so user/search variants that
    // could be substrings of a generic token are checked first.
    public static readonly IReadOnlyList<Crawler> Crawlers = new List<Crawler>
    {
        new("OAI-SearchBot",     "OAI-SearchBot",      "OpenAI",        "answer",   "OAI-SearchBot"),
        new("ChatGPT-User",      "ChatGPT-User",       "OpenAI",        "answer",   "ChatGPT-User"),
        new("GPTBot",            "GPTBot",             "OpenAI",        "both",     "GPTBot"),
        new("Claude-User",       "Claude-User",        "Anthropic",     "answer",   "Claude-User"),
        new("ClaudeBot",         "ClaudeBot",          "Anthropic",     "both",     "ClaudeBot"),
        new("anthropic-ai",      "anthropic-ai",       "Anthropic",     "training", "anthropic-ai"),
        new("Perplexity-User",   "Perplexity-User",    "Perplexity",    "answer",   "Perplexity-User"),
        new("PerplexityBot",     "PerplexityBot",      "Perplexity",    "answer",   "PerplexityBot"),
        new("Google-Extended",   "Google-Extended",    "Google",        "both",     null),
        new("Applebot-Extended", "Applebot-Extended",  "Apple",         "training", null),
        new("Amazonbot",         "Amazonbot",          "Amazon",        "both",     "Amazonbot"),
        new("Meta-ExternalAgent","Meta-ExternalAgent", "Meta",          "both",     "Meta-ExternalAgent"),
        new("Bytespider",        "Bytespider",         "ByteDance",     "both",     "Bytespider"),
        new("CCBot",             "CCBot (Common Crawl)","Common Crawl", "training", "CCBot"),
        new("cohere-ai",         "cohere-ai",          "Cohere",        "training", "cohere-ai"),
        new("YouBot",            "YouBot",             "You.com",       "answer",   "YouBot"),
        new("DuckAssistBot",     "DuckAssistBot",      "DuckDuckGo",    "answer",   "DuckAssistBot"),
    };

    /// <summary>Return the known AI crawler whose UA appears in <paramref name="ua"/>, or null.</summary>
    public static Crawler? Detect(string ua)
    {
        if (string.IsNullOrEmpty(ua)) return null;
        foreach (var c in Crawlers)
            if (c.Ua is { Length: > 0 } t && ua.Contains(t, StringComparison.OrdinalIgnoreCase))
                return c;
        return null;
    }

    // ---- block policy (site_settings) --------------------------------------------------------
    public static HashSet<string> Blocked(Db db)
    {
        var raw = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='ai_bot_blocklist'") ?? "";
        var valid = Crawlers.Select(c => c.Token).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                  .Where(valid.Contains).ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    // Private/app surfaces kept out of every crawler's reach (mirrors the historic static robots.txt).
    // "/world-admin" is the PCI World administration realm — a separate admin app that must stay out
    // of every index. Note "/world" itself is public and deliberately absent from this list.
    static readonly string[] Private = { "/admin", "/admin/", "/admin.html", "/app", "/app/", "/student.html", "/exam-ui.html", "/api/", "/world-admin", "/world-admin/" };

    // ---- cache -------------------------------------------------------------------------------
    static int _bump;
    static long _robotsVer = -1, _llmsVer = -1;
    static string _robots = "", _llms = "";
    static readonly object _lock = new();
    public static void Bump() => Interlocked.Increment(ref _bump);
    static long Ver => (long)PageContent.Version * 1_000_003 + _bump;

    // ---- robots.txt --------------------------------------------------------------------------
    public static string Robots(Db db)
    {
        var v = Ver;
        lock (_lock)
        {
            if (_robotsVer == v && _robots.Length > 0) return _robots;
            var host = Redirects.CanonicalBase;
            var blocked = Blocked(db);
            var sb = new StringBuilder(1024);
            sb.Append("User-agent: *\nAllow: /\n\n");
            sb.Append("# Private / authenticated surfaces — kept out of search and AI indexes. Defence in\n");
            sb.Append("# depth: also protected by authentication, noindex meta tags and an X-Robots-Tag header.\n");
            foreach (var p in Private) sb.Append("Disallow: ").Append(p).Append('\n');
            sb.Append("\n# --- AI / LLM crawlers ---------------------------------------------------------\n");
            sb.Append("# Answer engines are allowed by default (they inherit the rules above) so PCI\n");
            sb.Append("# content can be cited in AI assistants. Crawlers blocked in Admin -> AI Visibility\n");
            sb.Append("# are denied all access below.\n");
            if (blocked.Count == 0)
            {
                sb.Append("# (no AI crawlers are currently blocked)\n");
            }
            else
            {
                foreach (var c in Crawlers.Where(c => blocked.Contains(c.Token)))
                    sb.Append("\nUser-agent: ").Append(c.Token).Append("\nDisallow: /\n");
            }
            sb.Append("\n# Curated map for language models: ").Append(host).Append("/llms.txt\n");
            // Advertise the sitemap index (which links both the page + blog sitemaps) plus each sitemap
            // directly, so crawlers that don't follow an index still discover the blog sitemap.
            sb.Append("Sitemap: ").Append(host).Append("/sitemap-index.xml\n");
            sb.Append("Sitemap: ").Append(host).Append("/sitemap.xml\n");
            sb.Append("Sitemap: ").Append(host).Append("/blog-sitemap.xml\n");
            sb.Append("Sitemap: ").Append(host).Append("/news-sitemap.xml\n");
            sb.Append("Sitemap: ").Append(host).Append("/world-sitemap.xml\n");   // PCI World's own catalogue
            _robots = sb.ToString();
            _robotsVer = v;
            return _robots;
        }
    }

    // ---- llms.txt (llmstxt.org) --------------------------------------------------------------
    // Ordered buckets: a page joins the first bucket whose any keyword is a substring of its slug.
    static readonly (string heading, string[] keys)[] Buckets =
    {
        ("Certifications & exams", new[] { "cert", "exam", "credential", "recert", "eligibility", "candidate", "retake", "proctor", "digital-badge", "verify" }),
        ("Membership & fellowship", new[] { "member", "fellow", "founding", "honorary", "donate" }),
        ("Knowledge & standards", new[] { "knowledge", "body-of-knowledge", "bok", "standard", "framework", "handbook", "curriculum", "research", "white-paper", "publication", "insight", "blog", "salary", "career", "report" }),
        ("About & governance", new[] { "about", "governance", "leadership", "mission", "policy", "ethics", "board", "committee", "accreditation", "impartiality", "quality", "disclaimer", "conduct", "appeals", "complaints" }),
        ("Chapters & events", new[] { "chapter", "event", "congress", "summit", "webinar", "conference" }),
        ("Employers & sectors", new[] { "employer", "organization", "corporate", "sector", "workforce", "government", "university", "partnership" }),
    };

    public static string LlmsTxt(Db db)
    {
        var v = Ver;
        lock (_lock)
        {
            if (_llmsVer == v && _llms.Length > 0) return _llms;
            var host = Redirects.CanonicalBase;
            var summary = (db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='org_summary'") ?? "").Trim();
            if (summary.Length == 0)
                summary = db.Scalar<string>("SELECT meta_description FROM pages WHERE slug='index.html'") ?? "";
            if (summary.Length == 0)
                summary = "The Project Controls Institute (PCI AI) is the global professional body and certification "
                        + "authority for project controls — certifications, membership, standards and continuing "
                        + "professional development for cost, planning, scheduling and risk professionals worldwide.";

            var groups = new Dictionary<string, List<(string title, string url, string desc)>>();
            var order = new List<string>();
            void Add(string head, (string, string, string) item)
            {
                if (!groups.TryGetValue(head, out var l)) { groups[head] = l = new(); order.Add(head); }
                l.Add(item);
            }
            foreach (var r in db.Query("SELECT slug,title,meta_description FROM pages WHERE published=1 AND (noindex IS NULL OR noindex=0) ORDER BY slug"))
            {
                var slug = H.Str(r["slug"]) ?? "";
                if (slug.Length == 0 || slug.Contains("..") || Sitemap_Excluded(slug)) continue;
                var url = slug.Equals("index.html", StringComparison.OrdinalIgnoreCase) ? host + "/" : host + "/" + slug;
                var title = (H.Str(r["title"]) ?? "").Trim();
                if (title.Length == 0) title = slug.Replace(".html", "").Replace('-', ' ');
                var desc = (H.Str(r["meta_description"]) ?? "").Trim();
                var head = Buckets.FirstOrDefault(b => b.keys.Any(k => slug.Contains(k, StringComparison.OrdinalIgnoreCase))).heading ?? "More from PCI";
                Add(head, (title, url, desc));
            }

            var sb = new StringBuilder(8192);
            sb.Append("# Project Controls Institute (PCI AI)\n\n");
            sb.Append("> ").Append(OneLine(summary)).Append("\n\n");
            sb.Append("This file follows the llms.txt convention (https://llmstxt.org) to help language ")
              .Append("models find and cite authoritative PCI content. Canonical site: ").Append(host).Append("\n");
            // stable, human-meaningful order: defined buckets first, catch-all last
            foreach (var head in Buckets.Select(b => b.heading).Concat(new[] { "More from PCI" }).Where(order.Contains))
            {
                sb.Append("\n## ").Append(head).Append('\n');
                foreach (var (title, url, desc) in groups[head])
                {
                    sb.Append("- [").Append(OneLine(title)).Append("](").Append(url).Append(')');
                    if (desc.Length > 0) sb.Append(": ").Append(OneLine(desc));
                    sb.Append('\n');
                }
            }
            _llms = sb.ToString();
            _llmsVer = v;
            return _llms;
        }
    }

    // App shells / error pages / server-side templates are never listed (mirrors Sitemap's exclusions).
    static readonly HashSet<string> _sitemapExclude = new(StringComparer.OrdinalIgnoreCase)
    { "student.html", "admin.html", "exam-ui.html", "404.html", "500.html", "offline.html",
      "blog-shell.html", "careers-detail.html", "certification-detail.html" };
    static bool Sitemap_Excluded(string slug) => _sitemapExclude.Contains(slug);

    // llms.txt is plain text/Markdown, but titles/descriptions are stored HTML-encoded (they normally
    // land in HTML attributes) — decode entities so language models read clean prose, not "&#x27;".
    static string OneLine(string s) => System.Net.WebUtility.HtmlDecode(s).Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
}
