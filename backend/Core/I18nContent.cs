using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Public-website internationalisation. Human-authored translations (NOT runtime machine translation)
/// for every captured text region — see <see cref="PageScan"/> — are stored per language in
/// <c>content_i18n</c> and injected server-side at request time: SEO-safe, works with JavaScript off,
/// cached per version. English is the source and the default; with lang=en nothing is injected and the
/// page is served byte-identical.
///
/// The active language comes from <c>?lang=</c> (persisted to a cookie) or the cookie, else English.
/// Body regions are translated with the SAME scan the English injector runs, so a region's stable key
/// lines up 1:1 with its translation; an untranslated region simply stays English. The switcher and
/// nav-label translation live in <see cref="ListSections"/> (the shared header is table-rendered there).
/// </summary>
public static class I18nContent
{
    /// <summary>Supported languages; English first (source + default).</summary>
    public static readonly string[] Langs = { "en", "ko", "ar", "es", "fr", "zh", "ru" };
    /// <summary>Native names for the switcher, in menu order.</summary>
    public static readonly (string code, string native)[] Menu =
    {
        ("en", "English"), ("ko", "한국어"), ("ar", "العربية"), ("es", "Español"),
        ("fr", "Français"), ("zh", "中文"), ("ru", "Русский"),
    };
    static readonly HashSet<string> Valid = new(Langs, StringComparer.Ordinal);
    static readonly HashSet<string> Rtl = new(StringComparer.Ordinal) { "ar" };
    public const string Cookie = "pci_lang";

    public static bool IsRtl(string lang) => Rtl.Contains(lang);
    /// <summary>True when a non-default, supported language is active (so injection should run).</summary>
    public static bool Applies(string lang) => lang != "en" && Valid.Contains(lang);

    /// <summary>Resolve the request's language: <c>?lang=</c> wins and is persisted to a year-long
    /// cookie; otherwise the cookie; otherwise English. Unknown values fall back to English.</summary>
    public static string ActiveLang(HttpContext ctx)
    {
        var q = ctx.Request.Query["lang"].ToString();
        if (!string.IsNullOrEmpty(q))
        {
            var lc = q.Trim().ToLowerInvariant();
            if (Valid.Contains(lc))
            {
                ctx.Response.Cookies.Append(Cookie, lc, new CookieOptions
                {
                    Path = "/",
                    MaxAge = TimeSpan.FromDays(365),
                    HttpOnly = false,           // purely a UI preference, not a secret
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true,
                });
                return lc;
            }
        }
        var c = ctx.Request.Cookies[Cookie];
        return !string.IsNullOrEmpty(c) && Valid.Contains(c) ? c : "en";
    }

    // ---- version + cache (invalidated when translations are edited) ----

    static int _version = 1;
    /// <summary>Called whenever content_i18n rows change so cached lookups refresh next request.</summary>
    public static void Bump() => Interlocked.Increment(ref _version);

    sealed class LangMaps
    {
        public Dictionary<string, Dictionary<string, string>> Page = new(StringComparer.OrdinalIgnoreCase); // slug → key → val
        public Dictionary<string, string> Global = new(StringComparer.Ordinal);                             // gkey → val
        public Dictionary<string, string> Nav = new(StringComparer.Ordinal);                                // English label → val
        public Dictionary<string, (string? title, string? meta)> Meta = new(StringComparer.OrdinalIgnoreCase); // slug → title/meta
    }

    static int _cacheVer = -1;
    static readonly Dictionary<string, LangMaps> _byLang = new(StringComparer.Ordinal);
    static readonly object _lock = new();

    static LangMaps Load(Db db, string lang)
    {
        var v = Volatile.Read(ref _version);
        lock (_lock)
        {
            if (_cacheVer != v) { _byLang.Clear(); _cacheVer = v; }
            if (_byLang.TryGetValue(lang, out var cached)) return cached;
            var m = new LangMaps();
            foreach (var r in db.Query("SELECT scope,slug,ckey,cvalue FROM content_i18n WHERE lang=? AND cvalue IS NOT NULL AND cvalue<>''", lang))
            {
                var scope = H.Str(r["scope"]) ?? "";
                var slug = H.Str(r["slug"]) ?? "";
                var key = H.Str(r["ckey"]) ?? "";
                var val = H.Str(r["cvalue"]) ?? "";
                switch (scope)
                {
                    case "p":
                        if (!m.Page.TryGetValue(slug, out var pm)) m.Page[slug] = pm = new(StringComparer.Ordinal);
                        pm[key] = val;
                        break;
                    case "g": m.Global[key] = val; break;
                    case "nav": m.Nav[key] = val; break;
                    case "meta":
                        m.Meta.TryGetValue(slug, out var mt);
                        m.Meta[slug] = key == "title" ? (val, mt.meta) : (mt.title, val);
                        break;
                }
            }
            _byLang[lang] = m;
            return m;
        }
    }

    // Languages that actually have translations (always includes English), in menu order — so the
    // switcher only offers a language once its content exists, and a fresh deploy shows no switcher.
    static int _availVer = -1;
    static (string code, string native)[] _avail = { Menu[0] };
    public static (string code, string native)[] AvailableLangs(Db db)
    {
        var v = Volatile.Read(ref _version);
        lock (_lock)
        {
            if (_availVer == v) return _avail;
            var present = new HashSet<string>(StringComparer.Ordinal) { "en" };
            foreach (var r in db.Query("SELECT DISTINCT lang FROM content_i18n WHERE cvalue IS NOT NULL AND cvalue<>''"))
                if (H.Str(r["lang"]) is { Length: > 0 } l) present.Add(l);
            _avail = Menu.Where(x => present.Contains(x.code)).ToArray();
            _availVer = v;
            return _avail;
        }
    }

    /// <summary>Translate a navigation label (used by <see cref="ListSections"/>); English if unset.</summary>
    public static string NavLabel(Db db, string lang, string english)
    {
        if (!Applies(lang) || string.IsNullOrEmpty(english)) return english;
        return Load(db, lang).Nav.TryGetValue(english, out var v) ? v : english;
    }

    static readonly Regex RxHtmlTag = new(@"<html\b([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static readonly Regex RxLangAttr = new(@"\s+lang\s*=\s*(""[^""]*""|'[^']*')", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static readonly Regex RxDirAttr = new(@"\s+dir\s*=\s*(""[^""]*""|'[^']*')", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static readonly Regex RxTitle = new(@"<title>.*?</title>", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static readonly Regex RxMeta = new("(<meta\\s+name=[\"']description[\"']\\s+content=[\"']).*?([\"'])", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>Translate a page's body text regions and set <c>&lt;html lang dir&gt;</c>. Runs on the
    /// already-English-rendered HTML (unedited pages are byte-identical to the file, so region keys match
    /// the translations keyed by the file's original text). A region with no translation stays English.</summary>
    public static string Render(Db db, string slug, string html, string lang)
    {
        if (!Applies(lang) || string.IsNullOrEmpty(html)) return html;
        var m = Load(db, lang);
        m.Page.TryGetValue(slug, out var pm);

        var outHtml = PageScan.Apply(html, r =>
        {
            if (pm is not null && pm.TryGetValue(r.Key, out var pv)) return (pv, r.CType);
            if (m.Global.TryGetValue(r.GKey, out var gv)) return (gv, r.CType);
            return ((string, string)?)null;
        });

        // The first <h1> is skipped by PageScan (it is the page's positional headline, keyed '_h1'),
        // so translate it here the same way PageContent does for edits.
        if (pm is not null && pm.TryGetValue("_h1", out var h1) && !string.IsNullOrEmpty(h1))
            outHtml = PageContent.RxH1.Replace(outHtml, mm => mm.Groups[1].Value + Esc(h1) + mm.Groups[3].Value, 1);

        if (m.Meta.TryGetValue(slug, out var meta))
        {
            if (!string.IsNullOrEmpty(meta.title))
                outHtml = RxTitle.Replace(outHtml, "<title>" + Esc(meta.title!) + "</title>", 1);
            if (!string.IsNullOrEmpty(meta.meta))
                outHtml = RxMeta.Replace(outHtml, mm => mm.Groups[1].Value + AttrEsc(meta.meta!) + mm.Groups[2].Value, 1);
        }

        // rewrite <html …>: replace lang, add dir=rtl for RTL languages
        var dir = IsRtl(lang) ? " dir=\"rtl\"" : "";
        outHtml = RxHtmlTag.Replace(outHtml, mm =>
        {
            var attrs = RxDirAttr.Replace(RxLangAttr.Replace(mm.Groups[1].Value, ""), "");
            return $"<html{attrs} lang=\"{lang}\"{dir}>";
        }, 1);
        return outHtml;
    }

    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    static string AttrEsc(string s) => Esc(s).Replace("\"", "&quot;");

    // ===== admin-facing translation control (backend-owned) =====

    /// <summary>One translatable source region: the same keys the injector resolves, so a translation
    /// stored against a Src actually appears on the page.</summary>
    public sealed record Src(string Scope, string Slug, string Key, string CType, string English);

    static readonly System.Text.RegularExpressions.Regex RxHasLetter = new("[A-Za-z]", System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>Skip strings that carry no translatable prose — URLs/paths, pure numbers/symbols.</summary>
    static bool Translatable(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;
        var t = s.Trim();
        if (t.StartsWith("http://") || t.StartsWith("https://") || t.StartsWith("/") || t.StartsWith("data:")) return false;
        return RxHasLetter.IsMatch(t);
    }

    /// <summary>Enumerate every translatable source region. With a slug, only that page's blocks + meta;
    /// without, the whole site including shared globals and navigation labels.</summary>
    public static List<Src> Sources(Db db, string? slug = null)
    {
        var list = new List<Src>();
        var pb = slug is null
            ? db.Query("SELECT slug,block_key,ctype,cvalue FROM page_blocks WHERE cvalue IS NOT NULL AND cvalue<>''")
            : db.Query("SELECT slug,block_key,ctype,cvalue FROM page_blocks WHERE slug=? AND cvalue IS NOT NULL AND cvalue<>''", slug);
        foreach (var r in pb)
        {
            var bk = H.Str(r["block_key"]) ?? "";
            if (bk.EndsWith("@src", StringComparison.Ordinal)) continue;
            if (bk != "_h1" && !bk.StartsWith("t:", StringComparison.Ordinal)) continue;
            var en = H.Str(r["cvalue"]) ?? "";
            if (!Translatable(en)) continue;
            list.Add(new Src("p", H.Str(r["slug"]) ?? "", bk, H.Str(r["ctype"]) ?? "text", en));
        }
        if (slug is null)
        {
            foreach (var r in db.Query("SELECT ckey,ctype,cvalue FROM site_content WHERE ckey LIKE 'g:%' AND cvalue IS NOT NULL AND cvalue<>''"))
            {
                var ck = H.Str(r["ckey"]) ?? "";
                if (ck.EndsWith("@src", StringComparison.Ordinal)) continue;
                var en = H.Str(r["cvalue"]) ?? "";
                if (!Translatable(en)) continue;
                list.Add(new Src("g", "", ck, H.Str(r["ctype"]) ?? "text", en));
            }
            foreach (var r in db.Query("SELECT DISTINCT label FROM nav_items WHERE visible=1 AND label IS NOT NULL AND label<>''"))
            {
                var lbl = H.Str(r["label"]) ?? "";
                if (!Translatable(lbl)) continue;
                list.Add(new Src("nav", "", lbl, "text", lbl));
            }
        }
        var pg = slug is null
            ? db.Query("SELECT slug,title,meta_description FROM pages")
            : db.Query("SELECT slug,title,meta_description FROM pages WHERE slug=?", slug);
        foreach (var r in pg)
        {
            var sl = H.Str(r["slug"]) ?? "";
            if (H.Str(r["title"]) is { Length: > 0 } tt && Translatable(tt)) list.Add(new Src("meta", sl, "title", "text", tt));
            if (H.Str(r["meta_description"]) is { Length: > 0 } md && Translatable(md)) list.Add(new Src("meta", sl, "meta", "text", md));
        }
        return list;
    }

    /// <summary>Set/replace one translation and refresh caches. Deletes-then-inserts (provider-agnostic).</summary>
    public static void Upsert(Db db, string lang, string scope, string slug, string ckey, string? cvalue)
    {
        db.Execute("DELETE FROM content_i18n WHERE lang=? AND scope=? AND slug=? AND ckey=?", lang, scope, slug, ckey);
        if (!string.IsNullOrEmpty(cvalue))
            db.Execute("INSERT INTO content_i18n(lang,scope,slug,ckey,cvalue) VALUES(?,?,?,?,?)", lang, scope, slug, ckey, cvalue);
        Bump();
    }

    /// <summary>Translation coverage per language against the full translatable source set.</summary>
    public static object Coverage(Db db)
    {
        var srcs = Sources(db, null);
        string SK(string sc, string sl, string k) => sc + "" + sl + "" + k;
        var langs = new Dictionary<string, object>();
        foreach (var lang in Langs)
        {
            if (lang == "en") continue;
            var have = new HashSet<string>(StringComparer.Ordinal);
            foreach (var r in db.Query("SELECT scope,slug,ckey FROM content_i18n WHERE lang=? AND cvalue IS NOT NULL AND cvalue<>''", lang))
                have.Add(SK(H.Str(r["scope"]) ?? "", H.Str(r["slug"]) ?? "", H.Str(r["ckey"]) ?? ""));
            var done = srcs.Count(s => have.Contains(SK(s.Scope, s.Slug, s.Key)));
            langs[lang] = new { done, total = srcs.Count };
        }
        return new { total = srcs.Count, langs };
    }

    /// <summary>Which translations already exist for a language, keyed scopeslugkey → value.</summary>
    public static Dictionary<string, string> Existing(Db db, string lang, string? slug = null)
    {
        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        var rows = slug is null
            ? db.Query("SELECT scope,slug,ckey,cvalue FROM content_i18n WHERE lang=?", lang)
            : db.Query("SELECT scope,slug,ckey,cvalue FROM content_i18n WHERE lang=? AND (slug=? OR scope IN ('g','nav'))", lang, slug);
        foreach (var r in rows)
            map[(H.Str(r["scope"]) ?? "") + "" + (H.Str(r["slug"]) ?? "") + "" + (H.Str(r["ckey"]) ?? "")] = H.Str(r["cvalue"]) ?? "";
        return map;
    }
}
