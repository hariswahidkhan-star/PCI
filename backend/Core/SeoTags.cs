using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Serve-time injection of search/analytics tags into public pages, driven entirely by admin-set
/// site_settings (Admin → SEO → Search engines) — never hardcoded:
///
///   google_site_verification / bing_site_verification → verification metas (always, all pages)
///   ga4_measurement_id / gtm_container_id / clarity_project_id → CONSENT-GATED tag loader
///
/// Consent (§13.6): the analytics/marketing tags never load before the visitor accepts. The injected
/// script sets Google Consent Mode v2 defaults to 'denied', shows an accept/decline banner on the
/// first visit, persists the choice (localStorage 'pci.consent'), loads the tags only after consent,
/// and honours a stored decline silently. PCI's own first-party analytics are cookieless/server-side
/// and unaffected. Sensitive pages (checkout, sign-in, password reset, payment results, application
/// forms) never receive analytics tags at all, so session recorders cannot capture them.
/// Nothing is injected while settings are empty. Fragments are cached; Bump() on settings change.
/// </summary>
public static class SeoTags
{
    static int _version = 1;
    public static void Bump() => Interlocked.Increment(ref _version);

    // Pages where analytics/recording tags must never load (§13.6): payment, credentials, forms.
    static readonly HashSet<string> Sensitive = new(StringComparer.OrdinalIgnoreCase)
    {
        "checkout.html", "login.html", "student-login.html", "forgot-password.html", "reset-password.html",
        "payment-success.html", "payment-failed.html", "enrol.html", "honorary-application.html",
        "accommodation-form.html", "appeals-form.html",
    };

    static int _cacheVer = -1;
    static string _verify = "";   // verification metas — every page
    static string _tags = "";     // consent-gated analytics loader — non-sensitive pages only
    static readonly object _lock = new();

    static void Ensure(Db db)
    {
        var v = Volatile.Read(ref _version);
        lock (_lock)
        {
            if (_cacheVer == v) return;
            string S(string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k)?.Trim() ?? "";
            var vb = new System.Text.StringBuilder();
            if (S("google_site_verification") is { Length: > 0 } gv)
                vb.Append($"<meta name=\"google-site-verification\" content=\"{Attr(gv)}\"/>");
            if (S("bing_site_verification") is { Length: > 0 } bv)
                vb.Append($"<meta name=\"msvalidate.01\" content=\"{Attr(bv)}\"/>");
            _verify = vb.ToString();

            var ga = S("ga4_measurement_id"); if (!Safe(ga)) ga = "";
            var gtm = S("gtm_container_id"); if (!Safe(gtm)) gtm = "";
            var cl = S("clarity_project_id"); if (!Safe(cl)) cl = "";
            if (ga.Length == 0 && gtm.Length == 0 && cl.Length == 0) { _tags = ""; }
            else
            {
                // One inline script: Consent Mode v2 default-denied → banner → persisted choice → dynamic tag load.
                _tags = "<script>(function(){var GA='" + ga + "',GTM='" + gtm + "',CL='" + cl + "';" +
                    "window.dataLayer=window.dataLayer||[];function gtag(){dataLayer.push(arguments);}window.gtag=window.gtag||gtag;" +
                    "gtag('consent','default',{analytics_storage:'denied',ad_storage:'denied',ad_user_data:'denied',ad_personalization:'denied'});" +
                    "function load(){" +
                    "gtag('consent','update',{analytics_storage:'granted',ad_storage:'denied',ad_user_data:'denied',ad_personalization:'denied'});" +
                    "if(GA){var s=document.createElement('script');s.async=1;s.src='https://www.googletagmanager.com/gtag/js?id='+GA;document.head.appendChild(s);gtag('js',new Date());gtag('config',GA);}" +
                    "if(GTM){dataLayer.push({'gtm.start':new Date().getTime(),event:'gtm.js'});var g=document.createElement('script');g.async=1;g.src='https://www.googletagmanager.com/gtm.js?id='+GTM;document.head.appendChild(g);}" +
                    "if(CL){(function(c,l,a,r,i,t,y){c[a]=c[a]||function(){(c[a].q=c[a].q||[]).push(arguments)};t=l.createElement(r);t.async=1;t.src='https://www.clarity.ms/tag/'+i;y=l.getElementsByTagName(r)[0];y.parentNode.insertBefore(t,y);})(window,document,'clarity','script',CL);}}" +
                    "var c=null;try{c=localStorage.getItem('pci.consent');}catch(e){}" +
                    "if(c==='granted'){load();return;}if(c==='denied'){return;}" +
                    "function banner(){var b=document.createElement('div');b.id='pci-consent';b.setAttribute('role','dialog');b.setAttribute('aria-label','Cookie consent');" +
                    "b.style.cssText='position:fixed;left:0;right:0;bottom:0;z-index:9999;background:#0f172a;color:#fff;padding:14px 18px;display:flex;gap:12px;align-items:center;justify-content:center;flex-wrap:wrap;font:14px/1.45 system-ui,sans-serif;box-shadow:0 -8px 30px rgba(0,0,0,.25)';" +
                    "b.innerHTML='<span>We use optional analytics cookies to understand how the site is used. Essential functionality never depends on them.</span>';" +
                    "function btn(t,p){var x=document.createElement('button');x.textContent=t;x.style.cssText='padding:8px 16px;border-radius:8px;border:0;cursor:pointer;font-weight:600;'+(p?'background:#e11d48;color:#fff':'background:transparent;color:#cbd5e1;border:1px solid #475569');return x;}" +
                    "var a=btn('Accept',true),d=btn('Decline',false);" +
                    "a.onclick=function(){try{localStorage.setItem('pci.consent','granted');}catch(e){}b.remove();load();};" +
                    "d.onclick=function(){try{localStorage.setItem('pci.consent','denied');}catch(e){}b.remove();};" +
                    "b.appendChild(a);b.appendChild(d);document.body.appendChild(b);}" +
                    "if(document.readyState==='loading'){document.addEventListener('DOMContentLoaded',banner);}else{banner();}" +
                    "})();</script>";
            }
            _cacheVer = v;
        }
    }

    /// <summary>Inject configured fragments after &lt;head&gt;. Verification metas go on every page;
    /// the consent-gated analytics loader is skipped on sensitive pages.</summary>
    public static string Inject(Db db, string html, string slug)
    {
        Ensure(db);
        var frag = _verify + (Sensitive.Contains(slug) ? "" : _tags);
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
