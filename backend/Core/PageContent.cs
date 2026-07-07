using System.Text.RegularExpressions;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Server-side content injection for the static website — the engine behind "fully dynamic content".
/// For a content page it applies, from the database, the page's editable title, meta description and any
/// tagged content regions (data-cms), producing the final HTML BEFORE it reaches the browser. So content
/// edited in the admin panel is live immediately, is seen by search engines, and works with JavaScript off.
///
/// Everything is cached and only recomputed when content actually changes (a global version counter that
/// the admin edit endpoints bump), so steady-state page serving stays a dictionary lookup + cached string.
/// </summary>
public static class PageContent
{
    // The single-file apps are not content pages; never inject into them.
    static readonly HashSet<string> AppShells = new(StringComparer.OrdinalIgnoreCase)
    { "student.html", "admin.html", "exam-ui.html", "checkout.html", "enroll.html", "index-launcher.html" };

    static int _version = 1;
    /// <summary>Called by any admin content edit so cached injections refresh on the next request.</summary>
    public static void Bump() => Interlocked.Increment(ref _version);

    // caches, keyed by content version so a bump transparently invalidates them
    static int _cachedVersion = -1;
    static Dictionary<string, string> _globalContent = new();                               // ckey → value
    static Dictionary<string, (string? title, string? meta)> _pageMeta = new();             // slug → title/meta
    static Dictionary<string, Dictionary<string, string>> _pageBlocks = new();              // slug → {block_key → value}
    static readonly Dictionary<string, (int ver, string html)> _rendered = new(StringComparer.OrdinalIgnoreCase);
    static readonly object _lock = new();

    static void EnsureLoaded(Db db)
    {
        var v = Volatile.Read(ref _version);
        if (_cachedVersion == v) return;
        lock (_lock)
        {
            if (_cachedVersion == v) return;
            var gc = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in db.Query("SELECT ckey,cvalue FROM site_content WHERE cvalue IS NOT NULL AND cvalue<>''"))
                gc[H.Str(r["ckey"]) ?? ""] = H.Str(r["cvalue"]) ?? "";
            var pm = new Dictionary<string, (string?, string?)>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in db.Query("SELECT slug,title,meta_description FROM pages"))
                pm[H.Str(r["slug"]) ?? ""] = (H.Str(r["title"]), H.Str(r["meta_description"]));
            var pb = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in db.Query("SELECT slug,block_key,cvalue FROM page_blocks WHERE cvalue IS NOT NULL AND cvalue<>''"))
            {
                var slug = H.Str(r["slug"]) ?? "";
                if (!pb.TryGetValue(slug, out var m)) pb[slug] = m = new(StringComparer.OrdinalIgnoreCase);
                m[H.Str(r["block_key"]) ?? ""] = H.Str(r["cvalue"]) ?? "";
            }
            _globalContent = gc; _pageMeta = pm; _pageBlocks = pb;
            _rendered.Clear();
            _cachedVersion = v;
        }
    }

    /// <summary>Structured overrides for one page (used by the /api/page-content endpoint).</summary>
    public static object ForApi(Db db, string slug)
    {
        EnsureLoaded(db);
        _pageMeta.TryGetValue(slug, out var meta);
        _pageBlocks.TryGetValue(slug, out var blocks);
        return new { slug, title = meta.title, meta_description = meta.meta, blocks = blocks ?? new(StringComparer.OrdinalIgnoreCase) };
    }

    /// <summary>True if the page has any dynamic content that must be injected (so the middleware can
    /// cheaply skip untouched pages and let the static-file middleware serve them).</summary>
    public static bool HasOverrides(Db db, string slug)
    {
        if (AppShells.Contains(slug)) return false;
        EnsureLoaded(db);
        if (_globalContent.Count > 0) return true;                        // global data-cms bindings may apply anywhere
        if (_pageMeta.TryGetValue(slug, out var m) && (m.title is not null || m.meta is not null)) return true;
        return _pageBlocks.ContainsKey(slug);
    }

    /// <summary>Inject title/meta/data-cms overrides into a page's HTML. Cached per (slug, version);
    /// the raw file is read (via <paramref name="readHtml"/>) only on a cache miss.</summary>
    public static string Render(Db db, string slug, Func<string> readHtml)
    {
        EnsureLoaded(db);
        var v = _cachedVersion;
        lock (_lock)
        {
            if (_rendered.TryGetValue(slug, out var hit) && hit.ver == v) return hit.html;
        }
        var outHtml = Inject(slug, readHtml());
        lock (_lock) { _rendered[slug] = (v, outHtml); }
        return outHtml;
    }

    static readonly Regex RxTitle = new(@"<title>.*?</title>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    static readonly Regex RxMeta = new("(<meta\\s+name=[\"']description[\"']\\s+content=[\"']).*?([\"'])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    // an element carrying a data-cms attribute: g1=open tag, g2=tag name, g4=key, g6=inner, g7=close tag
    static readonly Regex RxCms = new(@"(<([a-zA-Z0-9]+)([^>]*?)\sdata-cms=""([^""]+)""([^>]*)>)(.*?)(</\2>)", RegexOptions.Singleline);
    // the first <h1> on the page (positional headline override, key '_h1' — no HTML tagging needed)
    public static readonly Regex RxH1 = new(@"(<h1[^>]*>)(.*?)(</h1>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

    static string Inject(string slug, string html)
    {
        _pageMeta.TryGetValue(slug, out var meta);
        _pageBlocks.TryGetValue(slug, out var blocks);

        if (!string.IsNullOrEmpty(meta.title))
            html = RxTitle.Replace(html, "<title>" + Esc(meta.title!) + "</title>", 1);
        if (!string.IsNullOrEmpty(meta.meta))
            html = RxMeta.Replace(html, m => m.Groups[1].Value + EscAttr(meta.meta!) + m.Groups[2].Value, 1);

        // positional headline: replace the first <h1>'s inner text (edits every page's headline with no
        // template tagging). data-cms regions below still take precedence for finer control.
        if (blocks is not null && blocks.TryGetValue("_h1", out var h1) && !string.IsNullOrEmpty(h1))
            html = RxH1.Replace(html, m => m.Groups[1].Value + Esc(h1) + m.Groups[3].Value, 1);

        if ((blocks is { Count: > 0 }) || _globalContent.Count > 0)
        {
            html = RxCms.Replace(html, m =>
            {
                var key = m.Groups[4].Value;
                string? val = null;
                if (blocks is not null && blocks.TryGetValue(key, out var pv)) val = pv;
                else if (_globalContent.TryGetValue(key, out var gv)) val = gv;
                if (val is null) return m.Value;                        // no override → leave the static content
                return m.Groups[1].Value + Esc(val) + m.Groups[7].Value; // replace inner text only (safe)
            });
        }
        return html;
    }

    /// <summary>One-time seed (when page_blocks is empty): capture each content page's current headline
    /// as an editable '_h1' block, so the admin editor is pre-populated with the real current text and
    /// every page's headline is editable out of the box. Reads the shipped HTML files once.</summary>
    public static void SeedFromFiles(Db db, string webRoot)
    {
        try
        {
            if (db.Scalar<long>("SELECT COUNT(*) FROM page_blocks") > 0) return;
            if (!Directory.Exists(webRoot)) return;
            var stripTags = new Regex("<[^>]+>", RegexOptions.Singleline);
            foreach (var file in Directory.EnumerateFiles(webRoot, "*.html"))
            {
                var slug = Path.GetFileName(file);
                if (AppShells.Contains(slug)) continue;
                string html; try { html = File.ReadAllText(file); } catch { continue; }
                var m = RxH1.Match(html);
                if (!m.Success) continue;
                var text = System.Net.WebUtility.HtmlDecode(stripTags.Replace(m.Groups[2].Value, "")).Trim();
                if (text.Length == 0 || text.Length > 300) continue;
                db.Execute("INSERT OR IGNORE INTO page_blocks(slug,block_key,label,ctype,cvalue) VALUES(?,?,?,?,?)",
                    slug, "_h1", "Headline", "text", text);
            }
            Bump();
        }
        catch (Exception e) { Console.Error.WriteLine($"[content] headline seed skipped: {e.Message}"); }
    }

    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    static string EscAttr(string s) => Esc(s).Replace("\"", "&quot;");
}
