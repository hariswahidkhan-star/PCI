using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Serve-time injection of search/analytics tags into public pages, driven entirely by admin-set
/// site_settings (Admin → SEO → Search engines) — never hardcoded:
///
///   google_site_verification → &lt;meta name="google-site-verification"&gt;   (Search Console)
///   bing_site_verification   → &lt;meta name="msvalidate.01"&gt;               (Bing Webmaster Tools)
///   ga4_measurement_id       → GA4 gtag.js snippet
///   gtm_container_id         → Google Tag Manager snippet
///   clarity_project_id       → Microsoft Clarity snippet
///
/// Nothing is injected while a setting is empty (the old hardcoded G-XXXX placeholder in the files is
/// commented out and inert). The head fragment is cached and rebuilt when settings change (Bump()).
/// Private app shells never get tags.
/// </summary>
public static class SeoTags
{
    static int _version = 1;
    public static void Bump() => Interlocked.Increment(ref _version);

    static int _cacheVer = -1;
    static string _frag = "";
    static readonly object _lock = new();

    public static string HeadFragment(Db db)
    {
        var v = Volatile.Read(ref _version);
        lock (_lock)
        {
            if (_cacheVer == v) return _frag;
            string S(string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k)?.Trim() ?? "";
            var sb = new System.Text.StringBuilder();
            if (S("google_site_verification") is { Length: > 0 } gv)
                sb.Append($"<meta name=\"google-site-verification\" content=\"{Attr(gv)}\"/>");
            if (S("bing_site_verification") is { Length: > 0 } bv)
                sb.Append($"<meta name=\"msvalidate.01\" content=\"{Attr(bv)}\"/>");
            if (S("gtm_container_id") is { Length: > 0 } gtm && Safe(gtm))
                sb.Append("<script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src='https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);})(window,document,'script','dataLayer','" + gtm + "');</script>");
            if (S("ga4_measurement_id") is { Length: > 0 } ga && Safe(ga))
                sb.Append("<script async src=\"https://www.googletagmanager.com/gtag/js?id=" + ga + "\"></script><script>window.dataLayer=window.dataLayer||[];function gtag(){dataLayer.push(arguments);}gtag('js',new Date());gtag('config','" + ga + "');</script>");
            if (S("clarity_project_id") is { Length: > 0 } cl && Safe(cl))
                sb.Append("<script>(function(c,l,a,r,i,t,y){c[a]=c[a]||function(){(c[a].q=c[a].q||[]).push(arguments)};t=l.createElement(r);t.async=1;t.src=\"https://www.clarity.ms/tag/\"+i;y=l.getElementsByTagName(r)[0];y.parentNode.insertBefore(t,y);})(window,document,\"clarity\",\"script\",\"" + cl + "\");</script>");
            _frag = sb.ToString();
            _cacheVer = v;
            return _frag;
        }
    }

    /// <summary>Inject the configured tags right after &lt;head&gt;. No-op when nothing is configured.</summary>
    public static string Inject(Db db, string html)
    {
        var frag = HeadFragment(db);
        if (frag.Length == 0 || string.IsNullOrEmpty(html)) return html;
        var i = html.IndexOf("<head>", StringComparison.OrdinalIgnoreCase);
        if (i < 0) return html;
        return html.Insert(i + 6, frag);
    }

    // Tag/measurement IDs go inside inline script — restrict to the safe charset so a setting can
    // never smuggle markup or script into every page.
    static bool Safe(string id) => id.Length <= 40 && id.All(c => char.IsLetterOrDigit(c) || c is '-' or '_');
    static string Attr(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace("\"", "&quot;");
}
