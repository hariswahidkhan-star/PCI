using System.Net;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — server-rendered public pages (docs/pciworld/ARCHITECTURE.md §3, §5).
///
/// Complete HTML from the server: indexable without a JS build, fast, and visually independent of
/// the Institute website. The design direction is a calm project-control room — strong typography,
/// tabular numerals, restrained colour, no decorative noise. Two strings are product law and render
/// on every page from these constants: the Institute link and the operated-by disclosure; the
/// practice-not-certification notice renders on every challenge/result surface. There are no slots
/// anywhere for participant counts, testimonials, partner logos or rankings — honesty is structural.
/// All dynamic text is HTML-encoded here; embedded JSON escapes "&lt;/" to keep script context safe.
/// </summary>
public static class WorldPages
{
    public const string OperatedBy =
        "PCI World is a global learning and challenge platform operated by the Project Controls Institute.";
    public const string PracticeNotice =
        "PCI World challenges are educational practice and professional-development evidence. " +
        "They are not PCI certification examinations, and completing one does not grant or affect " +
        "any PCI certification, membership or credential.";
    public const string InstituteLinkLabel = "Visit the Project Controls Institute";

    public static string E(string? s) => WebUtility.HtmlEncode(s ?? "");

    /// <summary>Marks the nav item for the page being viewed. aria-current is the accessible signal;
    /// the underline in the stylesheet hangs off the same attribute, so the visible state and the
    /// announced state can never disagree. "/world" matches only itself — every other World page
    /// lives under a longer path, so a prefix test would light up Today's Challenge everywhere.</summary>
    static string Cur(string canonicalPath, string href) =>
        canonicalPath == href || (href != "/world" && canonicalPath.StartsWith(href + "/", StringComparison.Ordinal))
            ? " aria-current=\"page\"" : "";
    public static string Json(object o) =>
        System.Text.Json.JsonSerializer.Serialize(o).Replace("</", "<\\/");

    public static string InstituteUrl(Db db) =>
        Settings.Str(db, "world_institute_url", "https://projectcontrolsinstitute.org");

    /// <summary>Brand type is served from this origin, not from a font CDN.
    ///
    /// It used to be a stylesheet appended from fonts.googleapis.com after window load. That never
    /// blocked paint, which was the point — but it meant every visit painted in a system fallback and
    /// then reflowed into Archivo, and this design system is Archivo 900 at 52px with tight tracking,
    /// so the swap is the most visible thing on the page. Worse, a network that cannot reach Google
    /// (corporate proxies, some countries, privacy blockers, an offline demo) never got the brand at
    /// all — it silently rendered in Helvetica.
    ///
    /// Self-hosted and preloaded, the right typeface is there on first paint, and no third party is
    /// told who is reading an Institute page. These are variable subsets: one file spans Archivo
    /// 700-900 and one spans Inter 400-700, and unicode-range means an English page fetches only the
    /// two latin files (~83 KB) while latin-ext stays unrequested until a page actually contains
    /// those characters. A glyph outside both subsets — a Passport holder whose name is in Cyrillic
    /// or Greek — falls back per-glyph to a system face, which is the browser behaving correctly
    /// rather than a hole. Regenerate with tools/fetch-brand-fonts.sh.</summary>
    const string FontLoader = """
        <link rel="preload" href="/assets/fonts/archivo-latin.woff2" as="font" type="font/woff2" crossorigin>
        <link rel="preload" href="/assets/fonts/inter-latin.woff2" as="font" type="font/woff2" crossorigin>
        """;

    /// <summary>One behaviour, inline so it costs no request and blocks no paint: the header's hairline
    /// once the page has scrolled. Nothing here is load-bearing — stickiness is pure CSS and this only
    /// adds the shadow, so with scripting off the page is complete.
    ///
    /// A scroll-triggered entrance for the sections was tried and removed. It starts content at
    /// opacity:0 and depends on IntersectionObserver to bring it back, which means anything that
    /// renders the page without scrolling it — print, a full-page capture, a reader view — gets blank
    /// space where the sections should be. That is a poor trade for an effect this file's own design
    /// direction ("no decorative noise") does not ask for.</summary>
    const string Behaviour = """
        <script>
        (function(){
          var h=document.querySelector('header.world');
          if(!h) return;
          var t=function(){h.classList.toggle('is-stuck',window.scrollY>4)};
          addEventListener('scroll',t,{passive:true});t();
        })();
        </script>
        """;

    // Design system: the PCI brand (backend/wwwroot/assets/styles.css) applied to PCI World —
    // Archivo 800/900 display type with tight tracking, Inter text, ink/noir/blue/crimson tokens,
    // crimson eyebrows + underline strokes, squared CTAs, layered blue-tinted shadows. Light-only,
    // exactly like the Institute site (meta color-scheme in Layout) — a brand commitment, not an
    // omission. Class names are stable API for the workspace/admin scripts and the E2E suite.
    const string Css = """
        @font-face{font-family:'Archivo';font-style:normal;font-weight:700 900;font-display:swap;
             src:url(/assets/fonts/archivo-latin.woff2) format('woff2');
             unicode-range:U+0000-00FF,U+0131,U+0152-0153,U+02BB-02BC,U+02C6,U+02DA,U+02DC,U+0304,U+0308,U+0329,U+2000-206F,U+20AC,U+2122,U+2191,U+2193,U+2212,U+2215,U+FEFF,U+FFFD}
        @font-face{font-family:'Archivo';font-style:normal;font-weight:700 900;font-display:swap;
             src:url(/assets/fonts/archivo-latin-ext.woff2) format('woff2');
             unicode-range:U+0100-02BA,U+02BD-02C5,U+02C7-02CC,U+02CE-02D7,U+02DD-02FF,U+0304,U+0308,U+0329,U+1D00-1DBF,U+1E00-1E9F,U+1EF2-1EFF,U+2020,U+20A0-20AB,U+20AD-20C0,U+2113,U+2C60-2C7F,U+A720-A7FF}
        @font-face{font-family:'Inter';font-style:normal;font-weight:400 700;font-display:swap;
             src:url(/assets/fonts/inter-latin.woff2) format('woff2');
             unicode-range:U+0000-00FF,U+0131,U+0152-0153,U+02BB-02BC,U+02C6,U+02DA,U+02DC,U+0304,U+0308,U+0329,U+2000-206F,U+20AC,U+2122,U+2191,U+2193,U+2212,U+2215,U+FEFF,U+FFFD}
        @font-face{font-family:'Inter';font-style:normal;font-weight:400 700;font-display:swap;
             src:url(/assets/fonts/inter-latin-ext.woff2) format('woff2');
             unicode-range:U+0100-02BA,U+02BD-02C5,U+02C7-02CC,U+02CE-02D7,U+02DD-02FF,U+0304,U+0308,U+0329,U+1D00-1DBF,U+1E00-1E9F,U+1EF2-1EFF,U+2020,U+20A0-20AB,U+20AD-20C0,U+2113,U+2C60-2C7F,U+A720-A7FF}
        :root{--ink:#0F172A;--paper:#FFFFFF;--paper-2:#F1F5F9;--noir:#0E1525;--line:#E3E8EF;
              --slate:#475569;--mist:#64748B;--blue:#1D4ED8;--blue-deep:#1E3A8A;--crimson:#C13329;
              --gilt:#C8A24B;--ok:#15803D;--bad:#C2410C;--muted:var(--slate);--field:#94A3B8;
              --display:'Archivo',system-ui,sans-serif;--sans:'Inter',system-ui,sans-serif;
              --shadow-rest:0 1px 2px rgba(13,32,90,.05),0 10px 28px -20px rgba(29,78,216,.14);
              --shadow-hover:0 2px 5px rgba(13,32,90,.06),0 26px 56px -24px rgba(29,78,216,.25),0 0 0 1px rgba(29,78,216,.10);
              --ease:cubic-bezier(.22,.61,.36,1)}
        *{box-sizing:border-box;margin:0}
        html{-webkit-text-size-adjust:100%}
        /* A cool wash across the top of the page so the first screen reads as a lit control room
           rather than a blank sheet. Painted as a background layer rather than an absolutely
           positioned element: a full-bleed div wider than the viewport adds a horizontal scrollbar
           (it did — the page measured 1470px in a 1440px window and 488px on a 390px phone), whereas
           a background can never overflow its box. */
        body{background-color:var(--paper);color:var(--ink);font:16.5px/1.62 var(--sans);-webkit-font-smoothing:antialiased;
             background-image:radial-gradient(56% 44% at 66% 0,rgba(29,78,216,.10),transparent 68%),
                              radial-gradient(46% 32% at 4% 0,rgba(193,51,41,.055),transparent 70%);
             background-repeat:no-repeat;background-size:100% 720px}
        a{color:var(--blue);text-decoration-thickness:1px;text-underline-offset:3px}
        a:focus-visible,button:focus-visible,input:focus-visible,select:focus-visible,textarea:focus-visible,
        summary:focus-visible,[tabindex]:focus-visible,details:focus-visible{outline:3px solid var(--blue);outline-offset:2px}
        /* The blue ring is 2.76:1 on the noir header/footer — below the 3:1 WCAG 1.4.11 requires of a
           focus indicator. Dark surfaces get a light ring instead (10.25:1). */
        header.world a:focus-visible,header.world button:focus-visible,
        footer.world a:focus-visible,.card--noir a:focus-visible,
        .card--noir button:focus-visible,.card--noir input:focus-visible{outline-color:#93C5FD}
        /* Programmatic focus targets (skip-link destination, revealed panels) are not controls:
           they receive focus so screen readers announce arrival, but a page-wide ring around a
           whole section is noise, not indication. Controls inside them keep their rings. */
        #main:focus,#me:focus,#auth:focus,#work:focus,#result:focus{outline:none}
        .shell{max-width:1020px;margin:0 auto;padding:0 22px}
        /* Sticky, because on a page whose job is "start the challenge" the way back to it should never
           scroll away. The hairline only appears once the page has moved, so the header sits flush
           with the hero at rest. */
        header.world{background:var(--noir);color:#E2E8F0;position:sticky;top:0;z-index:60;
             box-shadow:0 1px 0 rgba(255,255,255,0);transition:box-shadow .2s var(--ease)}
        header.world.is-stuck{box-shadow:0 1px 0 rgba(255,255,255,.09),0 18px 40px -30px rgba(0,0,0,.9)}
        /* The Institute relationship is product law on every page. Given its own rail it reads as
           provenance rather than as one more nav item competing with the primary journey. */
        .toprail{border-bottom:1px solid rgba(255,255,255,.07);background:#0A101C}
        .toprail .shell{display:flex;flex-wrap:wrap;gap:6px 20px;align-items:center;justify-content:space-between;
             padding:9px 22px;font-size:12.5px;letter-spacing:.01em;color:#7F8EA3}
        .toprail a.ext{color:#A9C6FF;font-weight:600;text-decoration:none;white-space:nowrap}
        .toprail a.ext:hover{color:#fff;text-decoration:underline;text-underline-offset:3px}
        header.world .shell{display:flex;flex-wrap:wrap;gap:12px 26px;align-items:center;padding:16px 22px}
        .brand{display:flex;align-items:center;gap:13px;color:#fff;text-decoration:none}
        .brand .wordmark{font-family:var(--display);font-weight:900;font-size:23px;letter-spacing:-.035em;line-height:.9;white-space:nowrap}
        /* The lockup is ONE unit: the crimson rule is drawn as the endorsement line's own border, not
           as a separate element. A breakpoint that hides the words therefore cannot strand the rule.
           That is precisely what shipped before — the tagline was display:none under 680px while the
           divider stayed, so phones read "PCI World |" with nothing after it. */
        .brand small{display:block;font-family:var(--sans);font-weight:600;font-size:11.5px;letter-spacing:.02em;
             color:#94A3B8;line-height:1.25;max-width:150px;white-space:normal;
             padding:3px 0 3px 13px;border-inline-start:2px solid var(--crimson)}
        .brand:hover small{color:#B6C2D2}
        header.world nav{display:flex;flex-wrap:wrap;gap:8px 22px;margin-left:auto;font-size:15px;align-items:center}
        header.world nav a{color:#CBD5E1;text-decoration:none;font-weight:500;padding:4px 0;position:relative}
        header.world nav a:hover{color:#fff}
        header.world nav a::after{content:"";position:absolute;left:0;right:0;bottom:-2px;height:2px;
             background:var(--crimson);transform:scaleX(0);transform-origin:left;transition:transform .2s var(--ease)}
        header.world nav a:hover::after,header.world nav a[aria-current="page"]::after{transform:scaleX(1)}
        header.world nav a[aria-current="page"]{color:#fff;font-weight:600}
        main{padding:56px 0 80px}
        .kicker{display:block;font-family:var(--sans);font-weight:700;font-size:12.5px;letter-spacing:.16em;
             text-transform:uppercase;color:var(--crimson);margin-bottom:14px}
        h1{font-family:var(--display);font-weight:800;font-size:clamp(34px,5.4vw,58px);line-height:1.04;
             letter-spacing:-.025em;margin:0 0 18px;text-wrap:balance;max-width:21ch}
        h2{font-family:var(--display);font-weight:800;font-size:22px;letter-spacing:-.015em;line-height:1.15;margin:0 0 14px}
        h2.sec{margin:52px 0 6px}
        .uline{width:64px;height:3px;background:var(--crimson);border-radius:2px;margin:0 0 22px}
        p.lede{font-size:19px;line-height:1.6;color:var(--slate);max-width:58ch}
        .num,td.num,.score{font-variant-numeric:tabular-nums}
        .card{background:var(--paper);border:1.5px solid var(--line);border-radius:14px;padding:30px;margin:22px 0;
             box-shadow:var(--shadow-rest)}
        .card .kicker{margin-bottom:10px}
        .card--noir{background:var(--noir);border-color:var(--noir);color:#E2E8F0}
        .card--noir h2{color:#fff}
        .card--noir .kicker{color:#F0A9A3}
        .meta{display:flex;flex-wrap:wrap;gap:9px;margin:14px 0 0;padding:0;list-style:none}
        .meta span{border:1.5px solid var(--line);border-radius:30px;padding:6px 15px;font-weight:600;
             font-size:13.5px;color:var(--ink);background:var(--paper)}
        .card--noir .meta span{border-color:#26334a;color:#CBD5E1;background:transparent}
        .btn{display:inline-flex;align-items:center;gap:10px;background:var(--blue);color:#fff;border:2px solid var(--blue);
             font-family:var(--sans);font-weight:600;font-size:15.5px;padding:15px 28px;cursor:pointer;text-decoration:none;
             border-radius:0;transition:background .16s var(--ease),border-color .16s var(--ease),color .16s var(--ease)}
        .btn:hover{background:var(--blue-deep);border-color:var(--blue-deep)}
        .btn.secondary{background:transparent;color:var(--ink);border:1.5px solid var(--field);padding:15.5px 28px}
        .btn.secondary:hover{border-color:var(--ink);background:var(--paper)}
        /* On a noir surface the default ink-on-transparent secondary button is unreadable. It needs
           its own light treatment or it fails contrast wherever the dark card is used. */
        .card--noir .btn.secondary,.ppt-cover .btn.secondary{color:#F1F5F9;border-color:#64748B}
        .card--noir .btn.secondary:hover,.ppt-cover .btn.secondary:hover{border-color:#F1F5F9;background:rgba(255,255,255,.06)}
        .btn+.btn{margin-left:12px}
        .notice{border-inline-start:3px solid var(--crimson);background:var(--paper-2);padding:16px 20px;
             color:var(--slate);font-size:14.5px;line-height:1.6;margin:26px 0;border-radius:0 10px 10px 0}
        .tbl-wrap{overflow-x:auto}
        table{border-collapse:collapse;width:100%;font-size:15.5px}
        caption{text-align:left}
        th{text-align:left;font-family:var(--display);font-weight:800;font-size:12.5px;letter-spacing:.05em;
             text-transform:uppercase;color:var(--ink)}
        th,td{padding:13px 14px;border-bottom:1px solid var(--line);vertical-align:top}
        tbody tr:hover{background:var(--paper-2)}
        td.num{text-align:right;white-space:nowrap;font-weight:600}
        .ok{color:var(--ok);font-weight:700}.bad{color:var(--bad);font-weight:700}
        label{display:block;font-weight:600;margin:18px 0 7px;font-size:15px}
        /* --field is the border colour for anything a person types into. --line (1.23:1 on white)
           is fine for decorative card edges but fails WCAG 1.4.11's 3:1 for a control boundary —
           the field is literally invisible to some users. --field is 3.03:1. */
        input[type=text],input[type=number],input[type=email],input[type=password]{width:100%;max-width:360px;
             padding:14px 16px;font-size:16px;border:1.5px solid var(--field);border-radius:0;background:var(--paper);
             color:var(--ink);font-variant-numeric:tabular-nums;font-family:var(--sans)}
        input:hover{border-color:var(--slate)}
        select,textarea{font-family:var(--sans);font-size:15.5px;color:var(--ink);background:var(--paper);
             border:1.5px solid var(--field);border-radius:0;padding:12px 14px}
        input[type=radio],input[type=checkbox]{accent-color:var(--blue)}
        fieldset{border:1.5px solid var(--line);border-radius:12px;padding:20px 22px 14px;margin:20px 0}
        legend{font-family:var(--display);font-weight:800;font-size:16.5px;letter-spacing:-.01em;padding:0 8px}
        .opt{display:flex;gap:12px;align-items:flex-start;padding:10px 8px;border-radius:8px}
        .opt:hover{background:var(--paper-2)}
        .opt input{margin-top:5px;accent-color:var(--blue);width:16px;height:16px;flex:0 0 auto}
        .opt label{font-weight:500;margin:0;line-height:1.5}
        .dim{display:flex;gap:40px;flex-wrap:wrap;margin:14px 0}
        .dim div{min-width:118px}
        /* --mist (#64748B) on white is 4.42:1 — an AA failure at this size. These labels carry the
           meaning of the numbers beside them, so they use --slate (7.59:1). */
        .dim .kicker{margin-bottom:4px;color:var(--slate)}
        .card--noir .dim .kicker{color:#94A3B8}
        .dim b{font-family:var(--display);font-weight:800;font-size:38px;letter-spacing:-.02em;display:block;line-height:1.05}
        details.card summary{cursor:pointer;font-weight:600}
        .steps{padding-left:0;list-style:none;counter-reset:step;display:grid;gap:22px}
        .steps li{counter-increment:step;display:grid;grid-template-columns:56px 1fr;gap:18px;align-items:start}
        .steps li::before{content:counter(step,decimal-leading-zero);font-family:var(--display);font-weight:800;
             font-size:30px;color:var(--crimson);line-height:1.1}
        .steps b{font-family:var(--display);font-weight:800;font-size:17px;letter-spacing:-.01em;display:block;margin-bottom:3px}
        .steps li div{color:var(--slate)}
        /* The panel is the product, so it gets real material: a lit top edge, a soft floor shadow and
           a faint control-room grid — depth from light, not from decoration. */
        .hero-panel{background:linear-gradient(168deg,#141D30 0%,var(--noir) 46%,#0A101C 100%);
             border-radius:18px;padding:26px 26px 18px;margin:34px 0 6px;overflow:hidden;position:relative;
             box-shadow:0 1px 0 rgba(255,255,255,.10) inset,0 0 0 1px rgba(255,255,255,.05),
                        0 40px 70px -46px rgba(8,17,40,.85)}
        .hero-panel::before{content:"";position:absolute;inset:0;pointer-events:none;opacity:.5;
             background:linear-gradient(rgba(255,255,255,.028) 1px,transparent 1px) 0 0/100% 34px,
                        linear-gradient(90deg,rgba(255,255,255,.028) 1px,transparent 1px) 0 0/34px 100%;
             -webkit-mask-image:radial-gradient(120% 90% at 70% 0,#000 25%,transparent 78%);
             mask-image:radial-gradient(120% 90% at 70% 0,#000 25%,transparent 78%)}
        .hero-panel>*{position:relative}
        .hero-panel .plabel{display:flex;justify-content:space-between;flex-wrap:wrap;gap:8px;
             font-family:var(--sans);font-weight:700;font-size:11.5px;letter-spacing:.14em;text-transform:uppercase;color:#94A3B8}
        .hero-panel svg{width:100%;height:auto;display:block;margin-top:10px}
        .legend-row{display:flex;gap:22px;flex-wrap:wrap;margin-top:12px;font-size:12.5px;font-weight:600;color:#CBD5E1}
        .legend-row span::before{content:"";display:inline-block;width:18px;height:3px;border-radius:2px;
             margin-right:8px;vertical-align:middle;background:var(--swatch,#fff)}
        /* The three plotted series draw in left-to-right on first paint, in the order a controller
           reads them: the plan, then what was earned, then what it cost. Purely additive — the paths
           are already in the markup, so with JS off or motion reduced the chart is simply there. */
        @media (prefers-reduced-motion:no-preference){
          .hero-panel .draw{stroke-dasharray:var(--len,1200);stroke-dashoffset:var(--len,1200);
               animation:draw 1.15s var(--ease) forwards;animation-delay:var(--d,0s)}
          /* The baseline is dashed on purpose — "planned value" is the promise, drawn as a promise —
             and stroke-dasharray is how that dash pattern is expressed. A draw-on animation needs the
             same property, and CSS beats the presentation attribute, so animating this line that way
             silently turns it solid and the legend starts describing something the chart isn't doing.
             It fades in instead. */
          .hero-panel .dot,.hero-panel .fade-in{opacity:0;animation:fade .6s var(--ease) forwards;
               animation-delay:var(--d,0s)}
          @keyframes draw{to{stroke-dashoffset:0}}
          @keyframes fade{to{opacity:1}}
        }

        /* ---- home: hero ---------------------------------------------------------------------- */
        /* Copy and evidence share the first screen at desktop width: the claim on the left, the
           project position it refers to on the right. Below 980px it stacks in reading order. */
        .hero{display:grid;grid-template-columns:minmax(0,1.02fr) minmax(0,.98fr);gap:20px 54px;align-items:center;
             margin-bottom:8px}
        .hero .hero-copy{min-width:0}
        .hero .hero-panel{margin:0}
        /* The global 21ch cap keeps long-form headings readable, but inside the hero the column is
           already the measure — capping again forces a five-line rag against empty space. */
        .hero h1{max-width:none;font-size:clamp(34px,4.1vw,52px)}
        .hero .lede{max-width:46ch}
        .hero .legend-row{gap:8px 18px;font-size:12px}
        @media (max-width:980px){.hero{grid-template-columns:1fr;gap:30px}.hero .hero-panel{margin-top:4px}
          .hero h1{font-size:clamp(34px,5.4vw,50px)}}
        .hero-wrap{position:relative}
        .hero h1{margin-bottom:16px}
        .cta-row{display:flex;flex-wrap:wrap;gap:12px;margin-top:26px}
        .cta-row .btn+.btn{margin-left:0}
        /* Facts about the offer that are already true elsewhere on this page — no invented numbers. */
        .hero-facts{display:flex;flex-wrap:wrap;gap:8px 22px;margin-top:22px;padding:0;list-style:none;
             font-size:13.5px;font-weight:600;color:var(--slate)}
        .hero-facts li{display:flex;align-items:center;gap:8px}
        .hero-facts li::before{content:"";width:5px;height:5px;border-radius:50%;background:var(--crimson);flex:0 0 auto}

        /* ---- home: sections ------------------------------------------------------------------ */
        /* The trailing 10px is not slack: the .uline that follows has no top margin, so without it the
           rule sits on the heading's descenders and reads as a strike-through rather than an underline. */
        .sec-head{display:flex;align-items:baseline;justify-content:space-between;gap:18px;flex-wrap:wrap;margin:64px 0 10px}
        .sec-head h2{margin:0}
        .grid-3{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:18px;margin:22px 0}
        .grid-2{display:grid;grid-template-columns:repeat(2,minmax(0,1fr));gap:18px;margin:22px 0}
        @media (max-width:860px){.grid-3,.grid-2{grid-template-columns:1fr}}
        .tile{background:var(--paper);border:1.5px solid var(--line);border-radius:14px;padding:26px 24px;margin:0;
             box-shadow:var(--shadow-rest);transition:transform .22s var(--ease),box-shadow .22s var(--ease),border-color .22s var(--ease)}
        .tile:hover{transform:translateY(-3px);box-shadow:var(--shadow-hover);border-color:rgba(29,78,216,.22)}
        .tile .step-n{font-family:var(--display);font-weight:800;font-size:13px;letter-spacing:.14em;
             color:var(--crimson);display:block;margin-bottom:12px}
        .tile h3{font-family:var(--display);font-weight:800;font-size:18.5px;letter-spacing:-.012em;margin:0 0 8px}
        .tile p{color:var(--slate);margin:0;font-size:15.5px}
        .tile .go{display:inline-flex;align-items:center;gap:8px;margin-top:16px;font-weight:600;
             font-size:15px;text-decoration:none}
        .tile .go svg{transition:transform .22s var(--ease)}
        .tile:hover .go svg{transform:translateX(4px)}
        /* A hairline that grows out of the numeral, so the three steps read as one sequence. */
        .grid-3 .tile{position:relative;overflow:hidden}
        .grid-3 .tile::after{content:"";position:absolute;left:0;top:0;height:3px;width:100%;
             background:linear-gradient(90deg,var(--crimson),rgba(193,51,41,0));transform:scaleX(0);
             transform-origin:left;transition:transform .3s var(--ease)}
        .grid-3 .tile:hover::after{transform:scaleX(1)}
        footer.world{background:var(--noir);color:#94A3B8;font-size:14.5px;margin-top:30px;border-top:3px solid var(--crimson)}
        footer.world .shell{padding:54px 22px 44px}
        footer.world a{color:#CBD5E1}
        footer.world a:hover{color:#fff}
        footer.world .ft-brand{display:flex;align-items:center;gap:12px;margin-bottom:14px}
        footer.world .ft-brand .wordmark{font-family:var(--display);font-weight:900;font-size:19px;letter-spacing:-.03em;color:#fff}
        /* Same one-piece lockup as the header. The footer previously rendered the crimson rule with
           nothing after it at every width, because there was no endorsement line here at all. */
        footer.world .ft-brand small{font-family:var(--sans);font-weight:600;font-size:11.5px;letter-spacing:.02em;
             color:#8A99AD;line-height:1.25;padding:3px 0 3px 12px;border-inline-start:2px solid var(--crimson)}
        footer.world .fine{font-size:13px;line-height:1.65;color:#7C8CA0;max-width:88ch}
        .ft-grid{display:grid;gap:36px 44px;grid-template-columns:minmax(240px,1.5fr) repeat(3,minmax(150px,1fr))}
        .ft-h{font-family:var(--sans);font-weight:700;font-size:11.5px;letter-spacing:.18em;text-transform:uppercase;
             color:#7C8CA0;margin:0 0 14px}
        .ft-grid ul{list-style:none;margin:0;padding:0;display:grid;gap:10px}
        .ft-grid a{text-decoration:none}
        .ft-grid a:hover{text-decoration:underline;text-underline-offset:3px}
        .ft-base{border-top:1px solid #1E293B;margin-top:40px;padding-top:24px;display:grid;gap:10px}
        @media (max-width:860px){.ft-grid{grid-template-columns:1fr 1fr}}
        @media (max-width:520px){.ft-grid{grid-template-columns:1fr}}
        /* ── the Passport artefact ─────────────────────────────────────────────
           A passport cover, not a web card: deep noir with a corner sheen, hairline gilt frame,
           engraved guilloché rings (pure CSS, deterministic), a seal, and letter-spaced labels.
           Gilt (#C8A24B) is 7.5:1 on noir; slate labels 5.2:1; the name is white. */
        .ppt-cover{position:relative;background:radial-gradient(130% 150% at 88% -30%,#1C2C4E 0%,var(--noir) 56%);
             border-radius:18px;color:#CBD5E1;padding:42px 42px 34px;margin:28px 0;overflow:hidden;
             box-shadow:0 2px 6px rgba(8,15,35,.16),0 40px 80px -34px rgba(8,15,35,.55)}
        .ppt-cover::before{content:"";position:absolute;inset:11px;border:1px solid rgba(200,162,75,.42);
             border-radius:11px;pointer-events:none}
        .ppt-cover::after{content:"";position:absolute;inset:14px;border:1px solid rgba(200,162,75,.16);
             border-radius:9px;pointer-events:none}
        .ppt-lines{position:absolute;inset:0;pointer-events:none;
             background:repeating-radial-gradient(circle at 106% 112%,transparent 0 10px,rgba(200,162,75,.075) 10px 11px),
                        repeating-radial-gradient(circle at -8% -14%,transparent 0 13px,rgba(148,163,184,.05) 13px 14px)}
        .ppt-top{position:relative;display:flex;align-items:center;gap:16px;flex-wrap:wrap;margin-bottom:30px}
        .ppt-top .wordmark{font-family:var(--display);font-weight:900;font-size:19px;letter-spacing:-.03em;color:#fff;white-space:nowrap}
        .ppt-top .bar{width:2px;height:24px;background:var(--crimson);border-radius:2px;flex:0 0 auto}
        /* The endorsement after the crimson rule — the same lockup the header draws: PCI World,
           the red line, then who it is from. */
        .ppt-top .ppt-from{font-weight:600;font-size:11.5px;letter-spacing:.02em;color:#94A3B8;
             line-height:1.25;max-width:150px}
        .ppt-top .ppt-word{margin-left:auto;font-weight:700;font-size:12px;letter-spacing:.42em;
             text-transform:uppercase;color:var(--gilt);padding-left:4px}
        /* The owner's photograph, framed like the artefact's other engraved elements. */
        .ppt-photo{flex:0 0 auto;width:112px;height:140px;object-fit:cover;border-radius:9px;
             border:1px solid rgba(200,162,75,.6);box-shadow:0 0 0 5px rgba(200,162,75,.12);
             display:block;background:#0A101C}
        .ppt-kicker{position:relative;display:block;font-weight:700;font-size:11.5px;letter-spacing:.22em;
             text-transform:uppercase;color:var(--gilt);margin-bottom:12px}
        .ppt-name{position:relative;font-family:var(--display);font-weight:800;font-size:clamp(30px,4.8vw,46px);
             line-height:1.05;letter-spacing:-.02em;color:#fff;margin:0 0 10px;text-wrap:balance;max-width:24ch}
        .ppt-sub{position:relative;color:#94A3B8;font-size:15px;max-width:60ch;margin:0}
        .ppt-cover h2{position:relative}
        .ppt-stats{position:relative;display:flex;flex-wrap:wrap;gap:0;margin-top:30px;padding-top:6px;
             border-top:1px solid rgba(148,163,184,.28)}
        .ppt-stats>div{padding:16px 36px 0 0;margin-right:36px;border-right:1px solid rgba(148,163,184,.16)}
        .ppt-stats>div:last-child{border-right:0;margin-right:0;padding-right:0}
        .ppt-stats .kicker{color:#94A3B8;margin-bottom:5px;font-size:11.5px;letter-spacing:.18em}
        .ppt-stats b{font-family:var(--display);font-weight:800;font-size:40px;letter-spacing:-.02em;
             display:block;line-height:1.05;color:#fff;font-variant-numeric:tabular-nums}
        .ppt-seal{flex:0 0 auto;display:block}
        .ppt-foot{position:relative;display:flex;gap:10px 26px;flex-wrap:wrap;margin-top:26px;
             font-size:12px;font-weight:600;letter-spacing:.08em;text-transform:uppercase;color:#7C8CA0}
        /* Disclosure switches: still real checkboxes (the tests and assistive tech depend on that),
           drawn as instrument toggles. The control itself is the track, so it stays visible,
           focusable and clickable exactly as before. */
        input[type=checkbox].switch{appearance:none;-webkit-appearance:none;width:46px;height:26px;
             border-radius:26px;background:#CBD5E1;border:1.5px solid #64748B;position:relative;
             cursor:pointer;margin:0;flex:0 0 auto;transition:background .18s var(--ease),border-color .18s var(--ease)}
        input[type=checkbox].switch::after{content:"";position:absolute;top:2px;left:2px;width:19px;height:19px;
             border-radius:50%;background:#fff;box-shadow:0 1px 2px rgba(15,23,42,.4);transition:transform .18s var(--ease)}
        input[type=checkbox].switch:checked{background:var(--blue);border-color:var(--blue)}
        input[type=checkbox].switch:checked::after{transform:translateX(20px)}
        .opt-row{display:flex;align-items:center;justify-content:space-between;gap:18px;
             padding:15px 2px;border-bottom:1px solid var(--line)}
        .opt-row:last-of-type{border-bottom:0}
        .opt-row label{margin:0;font-weight:600;font-size:15.5px}
        .opt-row small{display:block;font-weight:400;color:var(--slate);font-size:13.5px;margin-top:2px}
        .ev-check{width:19px;height:19px;accent-color:var(--blue)}
        .defn{display:grid;gap:16px;margin:6px 0 0}
        .defn>div{display:grid;grid-template-columns:150px 1fr;gap:16px;align-items:start;
             padding:14px 0;border-bottom:1px solid var(--line)}
        .defn>div:last-child{border-bottom:0}
        .defn b{font-family:var(--display);font-weight:800;font-size:14.5px;letter-spacing:-.01em}
        .defn span{color:var(--slate);font-size:15px;line-height:1.6}
        .auth-alt{border-inline-start:1.5px solid var(--line);padding-inline-start:34px}
        @media (max-width:700px){.auth-alt{border-inline-start:0;padding-inline-start:0;
             border-top:1.5px solid var(--line);padding-top:26px}}
        @media (max-width:560px){
          .ppt-cover{padding:28px 22px 24px}
          .ppt-stats>div{padding-right:22px;margin-right:22px}
          .ppt-stats b{font-size:31px}
          .ppt-photo{width:88px;height:110px}
          .defn>div{grid-template-columns:1fr;gap:4px}
        }
        .crumbs{font-size:14px;color:var(--slate);margin-bottom:18px}
        .crumbs a{color:var(--slate)}
        .prose{max-width:70ch;font-size:17.5px;line-height:1.72}
        .prose h2{margin:38px 0 12px}
        .prose h3{font-family:var(--display);font-weight:800;font-size:18px;margin:28px 0 8px}
        .prose p{margin:0 0 18px}
        .prose ul,.prose ol{margin:0 0 20px;padding-inline-start:24px}
        .prose li{margin:0 0 8px}
        .prose blockquote{border-inline-start:3px solid var(--crimson);margin:22px 0;padding:2px 0 2px 18px;color:var(--slate)}
        .prose code{background:var(--paper-2);padding:2px 6px;font-size:.92em}
        article.card h2 a{text-decoration:none}
        article.card h2 a:hover{text-decoration:underline}
        .visually-hidden{position:absolute;width:1px;height:1px;overflow:hidden;clip:rect(0 0 0 0);white-space:nowrap}
        /* A skip link that stays invisible when focused is not a skip link (WCAG 2.4.7). Focusing
           it brings it back into the page as the first visible control. */
        a.visually-hidden:focus{position:fixed;top:12px;left:12px;width:auto;height:auto;clip:auto;
             z-index:100;background:var(--paper);color:var(--ink);border:2px solid var(--blue);
             padding:12px 20px;font-weight:600;text-decoration:none;box-shadow:var(--shadow-hover)}
        @media (max-width:680px){
          main{padding:38px 0 60px}
          .card{padding:20px;border-radius:12px}
          .btn{width:100%;justify-content:center}
          .btn+.btn{margin-left:0;margin-top:10px}
          .cta-row{gap:10px}
          .dim{gap:24px}
          .dim b{font-size:31px}
          header.world nav{gap:6px 16px;font-size:14px}
          /* Not sticky on phones. The brand row plus a two-line nav plus the rail is ~200px of chrome;
             pinned to a 844px viewport that is a quarter of the screen permanently spent on
             navigation. Sticky is a desktop affordance here, where the header is a single 64px row. */
          header.world{position:static}
          .toprail .shell{justify-content:center;text-align:center}
          .toprail .rail-note{display:none}
          .hero-panel{padding:20px 18px 16px;border-radius:14px}
          .sec-head{margin-top:48px}
          .tile{padding:22px 20px}
          /* The tagline stays on phones — it fits, and it is the endorsement the wordmark is trading
             on. Only below 380px does it go, and then the crimson rule goes with it because the rule
             IS its border. There is no width at which one can appear without the other. */
          .brand{gap:11px}
          .brand .wordmark{font-size:21px}
          .brand small{font-size:10.5px;max-width:132px;padding-left:11px}
        }
        /* 340px is where the lockup genuinely stops fitting: wordmark 118 + rule 2 + gaps 22 + tagline
           132 = 274px against 320-44=276px of usable width. Above it the endorsement stays, including
           on 360 and 375px phones. Below it both halves go together. */
        @media (max-width:339px){
          .brand small{display:none}
        }
        @media (prefers-reduced-motion:reduce){*{transition:none!important}}
        """;

    /// <summary>Every PCI World page: institute link in header and footer, operated-by disclosure,
    /// unique title/description, canonical, Open Graph.</summary>
    public static string Layout(Db db, string title, string metaDesc, string body,
        string canonicalPath, string? ogTitle = null, string? ogDesc = null, bool noindex = false)
    {
        var inst = E(InstituteUrl(db));
        return $"""
            <!doctype html>
            <html lang="en">
            <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <meta name="color-scheme" content="light only">
            <meta name="theme-color" content="#0E1525">
            <title>{E(title)}</title>
            <meta name="description" content="{E(metaDesc)}">
            {(noindex
                ? "<meta name=\"robots\" content=\"noindex, nofollow\">"
                // Being explicit on indexable pages is worth the line: it authorises a large image
                // preview and a full snippet, which the default leaves to each engine's guess.
                : "<meta name=\"robots\" content=\"index, follow, max-image-preview:large, max-snippet:-1\">")}
            <link rel="canonical" href="{E(WorldUrl.Base() + canonicalPath)}">
            <link rel="icon" href="data:image/svg+xml,%3Csvg xmlns=%27http://www.w3.org/2000/svg%27 viewBox=%270 0 64 64%27%3E%3Crect width=%2764%27 height=%2764%27 rx=%2714%27 fill=%27%230E1525%27/%3E%3Crect x=%2712%27 y=%2744%27 width=%2740%27 height=%274%27 rx=%272%27 fill=%27%23C13329%27/%3E%3Ctext x=%2732%27 y=%2738%27 font-family=%27Archivo,Arial%27 font-weight=%27900%27 font-size=%2722%27 fill=%27white%27 text-anchor=%27middle%27 letter-spacing=%27-1%27%3EPW%3C/text%3E%3C/svg%3E">
            <meta property="og:site_name" content="PCI World">
            <meta property="og:title" content="{E(ogTitle ?? title)}">
            <meta property="og:description" content="{E(ogDesc ?? metaDesc)}">
            <meta property="og:type" content="website">
            <meta property="og:url" content="{E(WorldUrl.Base() + canonicalPath)}">
            <meta property="og:locale" content="en">
            <meta name="twitter:card" content="summary">
            <meta name="twitter:title" content="{E(ogTitle ?? title)}">
            <meta name="twitter:description" content="{E(ogDesc ?? metaDesc)}">
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            {FontLoader}
            <style>{Css}</style>
            </head>
            <body>
            <a class="visually-hidden" href="#main">Skip to content</a>
            <header class="world">
              <div class="toprail">
                <div class="shell">
                  <span class="rail-note">{E(OperatedBy)}</span>
                  <a class="ext" href="{inst}" target="_blank" rel="noopener noreferrer">{InstituteLinkLabel} <span aria-hidden="true">&#8599;</span><span class="visually-hidden">(opens the official Institute website in a new tab)</span></a>
                </div>
              </div>
              <div class="shell">
                <a class="brand" href="/world">
                  <span class="wordmark">PCI World</span>
                  <small>From the Project<br>Controls Institute</small>
                </a>
                <nav aria-label="Primary">
                  <a href="/world"{Cur(canonicalPath, "/world")}>Today&rsquo;s Challenge</a>
                  <a href="/world/archive"{Cur(canonicalPath, "/world/archive")}>Challenge Library</a>
                  <a href="/world/blog"{Cur(canonicalPath, "/world/blog")}>Writing</a>
                  <a href="/world/account"{Cur(canonicalPath, "/world/account")}>Passport</a>
                  <a href="/world/about"{Cur(canonicalPath, "/world/about")}>About</a>
                </nav>
              </div>
            </header>
            <main id="main" class="shell" tabindex="-1">
            {body}
            </main>
            <footer class="world">
              <div class="shell">
                <div class="ft-grid">
                  <div>
                    <div class="ft-brand"><span class="wordmark">PCI World</span><small>From the Project Controls Institute</small></div>
                    <p style="margin:0;max-width:38ch;line-height:1.65">{E(OperatedBy)}</p>
                    <p style="margin:14px 0 0;max-width:38ch;line-height:1.65;color:#7C8CA0;font-size:13.5px">
                      A realistic project decision every day. Deterministic scoring, synthetic data,
                      and evidence you choose to keep.</p>
                  </div>
                  <nav aria-label="Practise">
                    <p class="ft-h">Practise</p>
                    <ul>
                      <li><a href="/world">Today&rsquo;s Challenge</a></li>
                      <li><a href="/world/archive">Challenge Library</a></li>
                      <li><a href="/world/blog">Writing</a></li>
                      <li><a href="/world/news">Newsroom</a></li>
                    </ul>
                  </nav>
                  <nav aria-label="Passport">
                    <p class="ft-h">Passport</p>
                    <ul>
                      <li><a href="/world/account">Your Passport</a></li>
                      <li><a href="/world/verify">Verify a Passport</a></li>
                      <li><a href="/world/about">How scoring works</a></li>
                    </ul>
                  </nav>
                  <nav aria-label="The Institute">
                    <p class="ft-h">The Institute</p>
                    <ul>
                      <li><a href="{inst}" target="_blank" rel="noopener noreferrer">{InstituteLinkLabel} <span aria-hidden="true">&#8599;</span></a></li>
                      <li><a href="/world/about">About PCI World</a></li>
                    </ul>
                  </nav>
                </div>
                <div class="ft-base">
                  <div class="fine">{E(PracticeNotice)}</div>
                </div>
              </div>
            </footer>
            {Behaviour}
            </body>
            </html>
            """;
    }

    public static string Home(Db db, Dictionary<string, object?>? today, Dictionary<string, object?>? version)
    {
        // On a world-only deployment the Simulation Lab lives on the Institute platform, not on
        // this host — the progression link must never dead-end inside our own allowlist.
        var simlab = WorldOnly.Enabled ? E(InstituteUrl(db)) : E(Settings.Str(db, "world_simlab_url", "/app/lab"));
        var primaryHref = today is not null ? $"/world/challenge/{E(H.Str(today["code"]))}" : "/world/archive";
        var todayCard = today is null || version is null
            ? """
              <div class="card"><span class="kicker">Today's challenge</span>
              <h2>The first challenge is being prepared</h2>
              <p style="color:var(--slate)">PCI World rotates a new project challenge every day at 00:00 UTC. Check back shortly.</p></div>
              """
            : $"""
              <div class="card card--noir">
                <span class="kicker">Today&rsquo;s challenge &middot; rotates daily at 00:00 UTC</span>
                <h2>{E(H.Str(version["title"]))}</h2>
                <p style="color:#CBD5E1;max-width:64ch">{E(H.Str(version["hook"]))}</p>
                <div class="meta">
                  <span>{E(H.Str(version["industry"]))}</span>
                  <span>{E(Cap(H.Str(version["difficulty"])))}</span>
                  <span class="num">~{H.L(version["est_minutes"])} minutes</span>
                  <span>Free &middot; no account needed</span>
                </div>
                <p style="margin-top:24px">
                  <a class="btn" href="/world/challenge/{E(H.Str(today["code"]))}">Take today&rsquo;s challenge
                    <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" aria-hidden="true"><path d="M5 12h14M13 6l6 6-6 6"/></svg></a>
                </p>
              </div>
              """;
        return Layout(db,
            "PCI World — Make the decision. Control the outcome.",
            "A free daily project challenge from the Project Controls Institute. Step into a realistic project situation, examine the evidence and decide what happens next.",
            $"""
            {WorldSeo.HomeJsonLd(db)}
            <div class="hero-wrap">
              <div class="hero">
                <div class="hero-copy">
                  <span class="kicker">PCI World Challenge</span>
                  <h1>The project is already moving. The decision is now yours.</h1>
                  <div class="uline" aria-hidden="true"></div>
                  <p class="lede">Step into a realistic project situation, examine the evidence and decide what happens next. Five to ten minutes. Free. No project experience required.</p>
                  <div class="cta-row">
                    <a class="btn" href="{primaryHref}">Take today&rsquo;s challenge
                      <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" aria-hidden="true"><path d="M5 12h14M13 6l6 6-6 6"/></svg></a>
                    <a class="btn secondary" href="/world/about">See how PCI World works</a>
                  </div>
                  <ul class="hero-facts">
                    <li>Free</li><li>No account needed</li><li>New challenge daily at 00:00 UTC</li>
                  </ul>
                </div>
                <div class="hero-panel" role="img" aria-label="A project performance chart: planned value as a dashed baseline, earned value tracking below plan, and actual cost running above earned value at the data date — the situation a PCI World challenge drops you into.">
                  <div class="plabel"><span>Live project position &middot; synthetic data</span><span>Data date &middot; month 4 of 12</span></div>
                  <svg viewBox="0 0 720 232" aria-hidden="true">
                    <g stroke="#1c2739" stroke-width="1">
                      <line x1="16" y1="192" x2="704" y2="192"/><line x1="16" y1="136" x2="704" y2="136"/>
                      <line x1="16" y1="80" x2="704" y2="80"/><line x1="16" y1="24" x2="704" y2="24"/>
                    </g>
                    <line x1="432" y1="14" x2="432" y2="206" stroke="#3b4a63" stroke-width="1.5" stroke-dasharray="3 5"/>
                    <text x="424" y="224" text-anchor="end" fill="#64748B" font-size="11" font-weight="700" letter-spacing="1.4">DATA DATE</text>
                    <path class="fade-in" style="--d:.05s" d="M16,206 C170,201 280,158 400,104 S600,26 704,14" fill="none" stroke="#94A3B8" stroke-width="2" stroke-dasharray="7 6"/>
                    <path class="draw" style="--len:520;--d:.30s" d="M16,206 C160,203 260,178 350,150 S415,128 432,122" fill="none" stroke="#C13329" stroke-width="2.5"/>
                    <path class="draw" style="--len:520;--d:.52s" d="M16,206 C150,202 250,172 345,138 S415,108 432,100" fill="none" stroke="#5B8DEF" stroke-width="2.5"/>
                    <circle class="dot" style="--d:1.5s" cx="432" cy="122" r="4.5" fill="#C13329"/>
                    <circle class="dot" style="--d:1.62s" cx="432" cy="100" r="4.5" fill="#5B8DEF"/>
                  </svg>
                  <div class="legend-row">
                    <span style="--swatch:#94A3B8">Planned value — the promise</span>
                    <span style="--swatch:#C13329">Earned value — the truth</span>
                    <span style="--swatch:#5B8DEF">Actual cost — the bill</span>
                  </div>
                </div>
              </div>
            </div>
            {todayCard}
            <div class="sec-head"><h2>How it works</h2></div>
            <div class="uline" aria-hidden="true"></div>
            <div class="grid-3">
              <div class="tile"><span class="step-n">STEP 01</span><h3>Read the situation</h3>
                <p>A real-shaped project moment with the evidence in front of you — synthetic data, real methods.</p></div>
              <div class="tile"><span class="step-n">STEP 02</span><h3>Do the work</h3>
                <p>Compute the measures that matter and make the judgement calls a professional would face.</p></div>
              <div class="tile"><span class="step-n">STEP 03</span><h3>See the consequences</h3>
                <p>Deterministic scoring, your professional decision profile, and what each choice would have caused.</p></div>
            </div>
            <div class="sec-head"><h2>The Passport</h2></div>
            <div class="uline" aria-hidden="true"></div>
            <div class="ppt-cover">
              <div class="ppt-lines" aria-hidden="true"></div>
              <div class="ppt-top">
                <span class="wordmark">PCI World</span><span class="bar" aria-hidden="true"></span>
                <span class="ppt-from">From the Project<br>Controls Institute</span>
                {SealSvg(66)}
                <span class="ppt-word">Passport</span>
              </div>
              <span class="ppt-kicker">Verified virtual project experience</span>
              <h2 style="color:#fff;font-size:clamp(24px,3.6vw,34px);max-width:24ch;margin-bottom:12px">Every challenge you complete becomes evidence you own</h2>
              <p class="ppt-sub" style="max-width:64ch">A free account keeps each completed challenge as a verified record — the situation, the difficulty, your score and your decision profile. You choose what appears, publish one link, and anyone you hand it to can check it against the live record. Withdraw it whenever you like.</p>
              <p style="position:relative;margin:26px 0 0">
                <a class="btn" href="/world/account">Start your Passport</a>
                <a class="btn secondary" href="/world/verify">Verify one you&rsquo;ve been given</a>
              </p>
            </div>
            <div class="sec-head"><h2>Built to be trusted</h2></div>
            <div class="uline" aria-hidden="true"></div>
            <div class="card">
              <div class="defn">
                <div><b>Deterministic scoring</b><span>The same answers always earn the same result. A score here is reproducible arithmetic, never opinion.</span></div>
                <div><b>Synthetic data, real methods</b><span>Every situation is authored from synthetic project data, so nothing confidential is ever behind a challenge — but the techniques are the ones the profession actually uses.</span></div>
                <div><b>No rankings, no leaderboards</b><span>Nothing here compares you with anyone else. A Passport states what its owner practised and how they decided — that is all it claims.</span></div>
                <div><b>Consent before publication</b><span>Nothing about you is public until you publish it, item by item and field by field. Answers are never shown to anyone.</span></div>
              </div>
            </div>
            <div class="sec-head"><h2>Where it leads</h2></div>
            <div class="uline" aria-hidden="true"></div>
            <div class="grid-2">
              <div class="tile"><span class="step-n">GO DEEPER</span><h3>PCI Simulation Lab</h3>
                <p>Multi-step simulations, coaching and competency tracking — the full discipline rather than a single decision.</p>
                <a class="go" href="{simlab}">Open the Simulation Lab
                  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" aria-hidden="true"><path d="M5 12h14M13 6l6 6-6 6"/></svg></a></div>
              <div class="tile"><span class="step-n">BE RECOGNISED</span><h3>Institute certifications</h3>
                <p>PCI World is practice with evidence. Formal recognition is earned through the Institute&rsquo;s own examinations.</p>
                <a class="go" href="{E(InstituteUrl(db))}" target="_blank" rel="noopener noreferrer">Explore certifications
                  <span aria-hidden="true">&#8599;</span><span class="visually-hidden">(opens the official Institute website in a new tab)</span></a></div>
            </div>
            <p class="notice">{E(PracticeNotice)}</p>
            """,
            "/world");
    }

    public static string Cap(string? s) => string.IsNullOrEmpty(s) ? "" : char.ToUpperInvariant(s[0]) + s[1..];

    /// <summary>The Passport seal — an engraved-style emblem drawn as inline SVG. Deterministic,
    /// self-contained (no external references) and decorative by role: it never carries meaning
    /// the surrounding text does not state.</summary>
    public static string SealSvg(int size = 92) => $"""
        <svg class="ppt-seal" width="{size}" height="{size}" viewBox="0 0 96 96" aria-hidden="true" focusable="false">
          <circle cx="48" cy="48" r="45" fill="none" stroke="#C8A24B" stroke-width="1.5" opacity=".9"/>
          <circle cx="48" cy="48" r="40.5" fill="none" stroke="#C8A24B" stroke-width=".7" opacity=".5"/>
          <circle cx="48" cy="48" r="31" fill="none" stroke="#C8A24B" stroke-width=".9" opacity=".65"/>
          <g stroke="#C8A24B" stroke-width="1.1" opacity=".75">
            <line x1="89" y1="48" x2="93" y2="48"/><line x1="83.5" y1="68.5" x2="87" y2="70.5"/>
            <line x1="68.5" y1="83.5" x2="70.5" y2="87"/><line x1="48" y1="89" x2="48" y2="93"/>
            <line x1="27.5" y1="83.5" x2="25.5" y2="87"/><line x1="12.5" y1="68.5" x2="9" y2="70.5"/>
            <line x1="7" y1="48" x2="3" y2="48"/><line x1="12.5" y1="27.5" x2="9" y2="25.5"/>
            <line x1="27.5" y1="12.5" x2="25.5" y2="9"/><line x1="48" y1="7" x2="48" y2="3"/>
            <line x1="68.5" y1="12.5" x2="70.5" y2="9"/><line x1="83.5" y1="27.5" x2="87" y2="25.5"/>
          </g>
          <text x="48" y="55" text-anchor="middle" font-family="Archivo,Arial,sans-serif" font-weight="900"
                font-size="24" fill="#C8A24B" letter-spacing="-1">PW</text>
          <rect x="39" y="61" width="18" height="2.5" rx="1.25" fill="#C13329"/>
        </svg>
        """;

    /// <summary>Challenge briefing + workspace. The embedded JSON is the allow-listed PublicView
    /// only — qualities, consequences and reference values arrive exclusively in the submit
    /// response, after grading.</summary>
    public static string Workspace(Db db, string code, Dictionary<string, object?> version, object publicView, string? inviteToken)
    {
        var payload = Json(new
        {
            code,
            invite = inviteToken,
            title = H.Str(version["title"]),
            version = H.L(version["version"]),
            view = publicView,
        });
        var title = H.Str(version["title"]) ?? "PCI World Challenge";
        return Layout(db,
            $"{title} — PCI World Challenge",
            H.Str(version["hook"]) ?? "A free daily project challenge from the Project Controls Institute.",
            $"""
            {WorldSeo.ChallengeJsonLd(db, code, version)}
            {Breadcrumb(("PCI World", "/world"), ("Challenge Library", "/world/archive"), (title, null))}
            <span class="kicker">PCI World Challenge &middot; {E(H.Str(version["industry"]))} &middot; {E(Cap(H.Str(version["difficulty"])))}</span>
            <h1>{E(title)}</h1>
            <p class="lede">{E(H.Str(version["hook"]))}</p>
            <div class="meta">
              <span>Role: {E(H.Str(version["role"]))}</span>
              <span class="num">~{H.L(version["est_minutes"])} minutes</span>
              <span>Free &middot; anonymous &middot; synthetic project data</span>
            </div>
            <p class="notice">{E(PracticeNotice)}</p>
            <div id="app" data-state="brief">
              <div class="card" id="brief">
                <h2 style="margin-top:0">The situation</h2>
                <p id="context"></p>
                <p style="margin-top:16px"><button class="btn" id="start">Start the challenge</button>
                <span id="starterr" role="alert" class="bad"></span></p>
              </div>
              <!-- No aria-describedby="savestate": that pointed the form's description at a LIVE
                   region, so the form announced "Progress saved." as its description. Autosave
                   state belongs in its own polite region, which is what #savestate is. -->
              <form id="work" hidden tabindex="-1">
                <div class="card">
                  <h2 style="margin-top:0">Evidence</h2>
                  <table id="evidence"><caption class="visually-hidden">Project evidence</caption>
                    <thead><tr><th scope="col">Item</th><th scope="col">Value</th></tr></thead><tbody></tbody></table>
                </div>
                <div class="card" id="asks"></div>
                <div class="card" id="decisions"></div>
                <p><button class="btn" id="submit" type="submit">Submit my answers</button>
                   <span id="savestate" aria-live="polite" style="margin-left:12px;color:var(--muted);font-size:14px"></span></p>
              </form>
              <!-- Deliberately NOT aria-live. The whole result — scores, measure table, decision
                   replay — is written here in one go; as a live region a screen reader announces
                   the entire page as a single utterance. Focus is moved here instead, which is
                   both quieter and navigable. -->
              <section id="result" hidden tabindex="-1" aria-labelledby="resulthead"></section>
              <p id="submiterr" role="alert" class="bad"></p>
            </div>
            <details class="card">
              <summary style="cursor:pointer;font-weight:600">Report an issue with this challenge</summary>
              <p style="margin-top:10px;color:var(--muted)">Spotted a content error, a calculation problem or an accessibility barrier? The PCI World content team reviews every report. No personal details are required.</p>
              <label for="rep_cat">What kind of issue?</label>
              <select id="rep_cat" style="padding:9px 10px;border:1.5px solid var(--field);border-radius:8px">
                <option value="content_error">Content error</option>
                <option value="calculation">Calculation problem</option>
                <option value="accessibility">Accessibility barrier</option>
                <option value="inappropriate">Inappropriate content</option>
                <option value="other">Something else</option>
              </select>
              <label for="rep_msg">Describe it</label>
              <textarea id="rep_msg" rows="4" maxlength="2000" style="width:100%;padding:10px 12px;border:1.5px solid var(--field);border-radius:8px;font:inherit"></textarea>
              <p style="margin-top:10px"><button class="btn secondary" type="button" id="rep_go">Send report</button>
                 <span id="rep_out" role="status"></span></p>
            </details>
            <script>const WORLD = {payload};</script>
            <script>{WorkspaceJs}</script>
            """,
            $"/world/challenge/{E(code)}");
    }

    const string WorkspaceJs = """
        (function(){
        'use strict';
        var att = null, saveTimer = null;
        function $(id){ return document.getElementById(id); }
        function esc(s){ var d = document.createElement('span'); d.textContent = s == null ? '' : String(s); return d.innerHTML; }
        function api(path, body){
          // The signed-in account travels with EVERY workspace call (journey repair P0-01):
          // without it, challenges completed while signed in stayed anonymous and never reached
          // the account's history or Passport until some later login happened to claim them.
          return fetch(path, { method:'POST', headers:{ 'Content-Type':'application/json',
            'X-World-Session': localStorage.getItem('world_session') || '',
            'X-World-Account': localStorage.getItem('world_account') || '' },
            body: JSON.stringify(body || {}) }).then(function(r){ return r.json().then(function(j){ if(!r.ok) throw j; return j; }); });
        }
        function mintSession(){
          localStorage.removeItem('world_session');
          return api('/api/world/session').then(function(r){ localStorage.setItem('world_session', r.token); });
        }
        function ensureSession(){
          return localStorage.getItem('world_session') ? Promise.resolve() : mintSession();
        }
        // A session token in this browser is a CLAIM about a row on the server, not proof of one.
        // The row can be gone — the participant cleared it, it aged out of the retention sweep, or
        // the deployment's storage was replaced. Previously the client trusted the key's mere
        // presence, so a stale token produced "Start a session first." on every attempt for ever,
        // with no way out but clearing site data: a permanently broken page.
        //
        // So every session-scoped call runs through here. On the server's `no_session` answer we
        // discard the dead token, mint a fresh one and retry EXACTLY once — enough to recover from
        // a vanished session, never enough to loop if something else is wrong.
        function lostSession(e){ return !!e && (e.error === 'no_session' || e.error === 'not_found'); }
        function withSession(call){
          return ensureSession().then(call).catch(function(e){
            if (!e || e.error !== 'no_session') throw e;
            return mintSession().then(call);
          });
        }
        function renderBrief(){ $('context').textContent = WORLD.view.context || ''; }
        function renderWork(saved){
          var tb = $('evidence').querySelector('tbody');
          (WORLD.view.evidence || []).forEach(function(e){
            var tr = document.createElement('tr');
            tr.innerHTML = '<td>' + esc(e.label) + '</td><td class="num">' + esc(e.value) + '</td>';
            tb.appendChild(tr);
          });
          var asks = $('asks');
          if ((WORLD.view.ask || []).length){
            asks.innerHTML = '<h2 style="margin-top:0">Work the numbers</h2>';
            WORLD.view.ask.forEach(function(a){
              var id = 'ask_' + a.key;
              if (a.type === 'bool'){
                // Yes/no judgement — real choices, never a numeric keypad.
                var fs = document.createElement('fieldset');
                var lg = document.createElement('legend'); lg.textContent = a.label; fs.appendChild(lg);
                ['yes','no'].forEach(function(v){
                  var oid = id + '_' + v;
                  var row = document.createElement('div'); row.className = 'opt';
                  var r = document.createElement('input'); r.type = 'radio';
                  r.name = 'ask_' + a.key; r.value = v; r.id = oid;
                  if (saved && String(saved[a.key]).toLowerCase() === v) r.checked = true;
                  var lb = document.createElement('label'); lb.setAttribute('for', oid);
                  lb.textContent = v === 'yes' ? 'Yes' : 'No';
                  row.appendChild(r); row.appendChild(lb); fs.appendChild(row);
                });
                asks.appendChild(fs);
                return;
              }
              var l = document.createElement('label'); l.setAttribute('for', id); l.textContent = a.label;
              var i = document.createElement('input');
              i.type = 'text'; i.id = id; i.name = a.key; i.autocomplete = 'off';
              if (a.type === 'set'){
                i.inputMode = 'text'; i.autocapitalize = 'characters';
                i.placeholder = 'Comma-separated — e.g. A,C,E';
              } else {
                i.inputMode = 'decimal';
              }
              if (saved && saved[a.key] != null) i.value = saved[a.key];
              asks.appendChild(l); asks.appendChild(i);
            });
          } else asks.hidden = true;
          var ds = $('decisions');
          if ((WORLD.view.decisions || []).length){
            ds.innerHTML = '<h2 style="margin-top:0">Make the call</h2>';
            WORLD.view.decisions.forEach(function(d){
              var fs = document.createElement('fieldset');
              var lg = document.createElement('legend'); lg.textContent = d.prompt; fs.appendChild(lg);
              (d.options || []).forEach(function(o){
                var name = 'decision_' + d.key, id = name + '_' + o.key;
                var row = document.createElement('div'); row.className = 'opt';
                var r = document.createElement('input'); r.type = 'radio'; r.name = name; r.value = o.key; r.id = id;
                if (saved && saved[name] === o.key) r.checked = true;
                var lb = document.createElement('label'); lb.setAttribute('for', id); lb.style.fontWeight = '400';
                lb.style.margin = '0'; lb.textContent = o.label;
                row.appendChild(r); row.appendChild(lb); fs.appendChild(row);
              });
              ds.appendChild(fs);
            });
          } else ds.hidden = true;
        }
        function answers(){
          var out = {};
          (WORLD.view.ask || []).forEach(function(a){
            if (a.type === 'bool'){
              var sel = document.querySelector('input[name="ask_' + a.key + '"]:checked');
              if (sel) out[a.key] = sel.value;
              return;
            }
            var el = $('ask_' + a.key);
            if (!el) return;
            var v = el.value.trim(); if (v.length) out[a.key] = v;
          });
          (WORLD.view.decisions || []).forEach(function(d){
            var sel = document.querySelector('input[name="decision_' + d.key + '"]:checked');
            if (sel) out['decision_' + d.key] = sel.value;
          });
          return out;
        }
        function autosave(){
          if (!att || att.completed) return;
          clearTimeout(saveTimer);
          saveTimer = setTimeout(function(){
            api('/api/world/attempts/' + att.attempt_id + '/save', { answers: answers() })
              .then(function(){ $('savestate').textContent = 'Progress saved.'; })
              .catch(function(e){
                // An attempt belongs to the session that started it, so a lost session cannot be
                // repaired by minting a new one — the honest thing is to say so while the answers
                // are still on screen, rather than to fail silently until submit.
                $('submiterr').textContent = lostSession(e)
                  ? 'Your session has ended, so progress is no longer being saved. Copy anything you want to keep, then reload the page to start again.'
                  : '';
                $('savestate').textContent = lostSession(e) ? '' : 'Could not save — will retry on your next change.';
              });
          }, 1200);
        }
        function shareLinks(url){
          var u = encodeURIComponent(location.origin + url);
          return '<p><a class="btn secondary" target="_blank" rel="noopener noreferrer" href="https://www.linkedin.com/sharing/share-offsite/?url=' + u + '">Share on LinkedIn</a> ' +
                 '<a class="btn secondary" target="_blank" rel="noopener noreferrer" href="https://wa.me/?text=' + u + '">WhatsApp</a> ' +
                 '<a class="btn secondary" target="_blank" rel="noopener noreferrer" href="https://x.com/intent/post?url=' + u + '">Post on X</a> ' +
                 '<button class="btn secondary" type="button" onclick="navigator.clipboard.writeText(location.origin + \'' + url + '\');this.textContent=\'Copied\'">Copy link</button></p>' +
                 '<p><a href="' + url + '">' + esc(location.origin + url) + '</a></p>';
        }
        function renderResult(r){
          $('work').hidden = true;
          var el = $('result'); el.hidden = false;
          var h = '<div class="card"><h2 id="resulthead"><span class="kicker">Your result</span></h2>' +
            '<div class="dim">' +
            '<div><span class="kicker">Overall</span><b class="score num">' + esc(r.score) + '</b></div>' +
            (r.calculation != null ? '<div><span class="kicker">Calculation</span><b class="score num">' + esc(r.calculation) + '</b></div>' : '') +
            (r.decision != null ? '<div><span class="kicker">Decision quality</span><b class="score num">' + esc(r.decision) + '</b></div>' : '') +
            '</div>' +
            '<h2 style="margin-top:8px">' + esc(r.profile) + '</h2>' +
            '<p>' + esc(r.profile_reason) + '</p>' +
            '<p><b>Improve next:</b> ' + esc(r.improvement) + '</p></div>';
          if ((r.measures || []).length){
            h += '<div class="card"><h2 style="margin-top:0">The numbers</h2><table>' +
                 '<thead><tr><th scope="col">Measure</th><th scope="col">Your answer</th><th scope="col">Reference</th><th scope="col">Verdict</th></tr></thead><tbody>';
            r.measures.forEach(function(m){
              h += '<tr><td>' + esc(m.label) + '</td><td class="num">' + esc(m.yours == null ? '—' : m.yours) +
                   '</td><td class="num">' + esc(m.reference) + '</td><td class="' + (m.correct ? 'ok' : 'bad') + '">' +
                   (m.correct ? 'Correct' : 'Check the method') + '</td></tr>';
            });
            h += '</tbody></table></div>';
          }
          if ((r.decisions || []).length){
            h += '<div class="card"><h2 style="margin-top:0">Decision replay</h2>';
            r.decisions.forEach(function(d){
              h += '<p><b>' + esc(d.prompt) + '</b><br>Your call: ' + esc(d.chosen_label || 'No decision made') +
                   (d.consequence ? '<br><span class="kicker">Consequence</span> ' + esc(d.consequence) : '') +
                   (d.principle ? '<br><span class="kicker">Principle</span> ' + esc(d.principle) : '') +
                   (!d.best && d.best_label ? '' : '') + '</p>';
              if (d.best_label && d.chosen_label !== d.best_label)
                h += '<p class="notice">Strongest available call: ' + esc(d.best_label) + '</p>';
            });
            h += '</div>';
          }
          h += '<div class="card"><h2 style="margin-top:0">Keep the evidence</h2>' +
               '<label for="dispname" style="margin-top:0">Name on your public result (leave blank to stay anonymous)</label>' +
               '<input type="text" id="dispname" maxlength="80" autocomplete="name">' +
               '<p style="margin-top:12px"><button class="btn" type="button" id="mkshare">Get my verified result link</button> ' +
               '<button class="btn secondary" type="button" id="mkinvite">Challenge a friend</button> ' +
               '<button class="btn secondary" type="button" id="mkretake">Retake this challenge</button></p>' +
               '<div id="sharebox"></div><div id="invitebox"></div>' +
               '<p style="margin-top:14px"><a href="/world/account">Create your free PCI World Passport</a> to keep this result as verified evidence — challenges completed in this browser are added automatically.</p>' +
               '<p class="notice">Your answers are never shown on the public result page.</p></div>';
          el.innerHTML = h;
          $('mkshare').addEventListener('click', function(){
            api('/api/world/attempts/' + att.attempt_id + '/share', { display_name: $('dispname').value.trim() })
              .then(function(s){ $('sharebox').innerHTML = shareLinks(s.url); })
              .catch(function(){ $('sharebox').innerHTML = '<p class="bad">Could not create the link — try again.</p>'; });
          });
          $('mkinvite').addEventListener('click', function(){
            api('/api/world/attempts/' + att.attempt_id + '/invite', { name: $('dispname').value.trim() })
              .then(function(s){ $('invitebox').innerHTML =
                '<p>Send this to someone who thinks they can do better:</p>' + shareLinks(s.url); })
              .catch(function(){ $('invitebox').innerHTML = '<p class="bad">Could not create the invitation — try again.</p>'; });
          });
          // Retake is a DELIBERATE act (journey repair P0-04): it opens a fresh linked attempt on the
          // server, then reloads so the workspace resumes it — the completed original is untouched.
          $('mkretake').addEventListener('click', function(){
            withSession(function(){
              return api('/api/world/attempts', { code: WORLD.code, invite: WORLD.invite, retake: true });
            }).then(function(){ location.reload(); })
              .catch(function(){ $('invitebox').innerHTML = '<p class="bad">Could not start a retake — try again.</p>'; });
          });
          // Honour prefers-reduced-motion: a smooth scroll is motion the user asked us not to make.
          var smooth = !(window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches);
          el.scrollIntoView(smooth ? { behavior: 'smooth' } : undefined);
          // Focus the result. Hiding the form would otherwise drop focus to <body>, stranding
          // keyboard and screen-reader users at the top of the document with no announcement.
          el.focus();
        }
        $('start').addEventListener('click', function(){
          $('starterr').textContent = '';
          withSession(function(){
            return api('/api/world/attempts', { code: WORLD.code, invite: WORLD.invite });
          }).then(function(r){
            att = r;
            renderWork(r.answers || null);
            $('work').hidden = false;
            $('start').hidden = true;
            if (r.completed && r.result) { renderResult(r.result); return; }
            $('work').addEventListener('input', autosave);
            $('work').addEventListener('change', autosave);
            // Focus moves to the revealed form: the button that had focus was just hidden.
            $('work').focus();
          }).catch(function(e){
            $('starterr').textContent = (e && e.message) || 'Could not start — please try again.';
          });
        });
        $('work').addEventListener('submit', function(ev){
          ev.preventDefault();
          if (!att) return;
          var btn = $('submit'); btn.disabled = true;
          api('/api/world/attempts/' + att.attempt_id + '/submit', { answers: answers() })
            .then(function(r){ att.completed = true; renderResult(r); })
            .catch(function(e){
              btn.disabled = false;
              // A failed submission is an error, not a status: it goes to role="alert", not to the
              // polite autosave region where it was previously easy to miss entirely.
              $('submiterr').textContent = lostSession(e)
                ? 'Your session has ended, so this attempt can no longer be submitted. Reload the page to start the challenge again.'
                : ((e && e.message) || 'Submission failed — your work is saved, try again.');
            });
        });
        $('rep_go').addEventListener('click', function(){
          $('rep_out').textContent = '';
          api('/api/world/report', { code: WORLD.code, category: $('rep_cat').value, message: $('rep_msg').value })
            .then(function(r){ $('rep_out').textContent = r.message + ' (ref ' + r.reference + ')'; $('rep_msg').value=''; })
            .catch(function(e){ $('rep_out').textContent = (e && e.message) || 'Could not send the report — try again.'; });
        });
        renderBrief();
        })();
        """;

    public static string Archive(Db db, List<Dictionary<string, object?>> rows,
        List<string>? industries = null, string? fIndustry = null, string? fDifficulty = null, string? fTrack = null,
        long total = -1, int page = 1, int pages = 1)
    {
        if (total < 0) total = rows.Count;
        var items = string.Join("", rows.Select(r => $"""
            <tr>
              <td><a href="/world/challenge/{E(H.Str(r["code"]))}">{E(H.Str(r["title"]))}</a></td>
              <td>{E(H.Str(r["industry"]))}</td>
              <td>{E(Cap(H.Str(r["difficulty"])))}</td>
              <td class="num">~{H.L(r["est_minutes"])} min</td>
            </tr>
            """));
        string Opt(string value, string label, string? current) =>
            $"<option value=\"{E(value)}\"{(value == (current ?? "") ? " selected" : "")}>{E(label)}</option>";
        var industryOpts = Opt("", "All industries", fIndustry) +
            string.Join("", (industries ?? new()).Select(i => Opt(i, i, fIndustry)));
        var difficultyOpts = Opt("", "All difficulties", fDifficulty) +
            string.Join("", WorldContent.Difficulties.Select(d => Opt(d, Cap(d), fDifficulty)));
        var trackOpts = Opt("", "All tracks", fTrack) +
            string.Join("", WorldContent.Tracks.Select(t => Opt(t, Cap(t.Replace('_', ' ')), fTrack)));
        // Pagination keeps the whole catalogue reachable — and crawlable — at any bank size. The
        // links carry the active filters so paging never silently resets them.
        string PageHref(int p)
        {
            var qs = new List<string>();
            if (!string.IsNullOrEmpty(fIndustry)) qs.Add("industry=" + Uri.EscapeDataString(fIndustry));
            if (!string.IsNullOrEmpty(fDifficulty)) qs.Add("difficulty=" + Uri.EscapeDataString(fDifficulty));
            if (!string.IsNullOrEmpty(fTrack)) qs.Add("track=" + Uri.EscapeDataString(fTrack));
            if (p > 1) qs.Add("page=" + p);
            return "/world/archive" + (qs.Count > 0 ? "?" + string.Join("&amp;", qs) : "");
        }
        var pager = pages <= 1 ? "" : $"""
            <nav class="pager" aria-label="Challenge library pages" style="display:flex;gap:12px;align-items:center;margin-top:14px">
              {(page > 1 ? $"<a class=\"btn secondary\" href=\"{PageHref(page - 1)}\" rel=\"prev\">Previous</a>" : "")}
              <span class="kicker">Page {page} of {pages}</span>
              {(page < pages ? $"<a class=\"btn secondary\" href=\"{PageHref(page + 1)}\" rel=\"next\">Next</a>" : "")}
            </nav>
            """;
        return Layout(db,
            "Challenge Library — PCI World",
            "Every published PCI World challenge: realistic project situations across industries, free to enter.",
            $"""
            <span class="kicker">Challenge Library</span>
            <h1>Every challenge stays open</h1>
            <p class="lede">The daily rotation brings one challenge forward each day — the archive keeps them all playable.</p>
            <form class="card" method="get" action="/world/archive" style="display:flex;gap:12px;flex-wrap:wrap;align-items:end">
              <div><label for="f_ind" style="margin-top:0">Industry</label>
                <select id="f_ind" name="industry" style="padding:9px 10px;border:1.5px solid var(--field);border-radius:8px">{industryOpts}</select></div>
              <div><label for="f_dif" style="margin-top:0">Difficulty</label>
                <select id="f_dif" name="difficulty" style="padding:9px 10px;border:1.5px solid var(--field);border-radius:8px">{difficultyOpts}</select></div>
              <div><label for="f_trk" style="margin-top:0">Track</label>
                <select id="f_trk" name="track" style="padding:9px 10px;border:1.5px solid var(--field);border-radius:8px">{trackOpts}</select></div>
              <div><button class="btn secondary" type="submit">Filter</button></div>
            </form>
            <div class="card">
            <p class="kicker" style="margin-bottom:10px">{total} challenge{(total == 1 ? "" : "s")}{(pages > 1 ? $" &middot; page {page} of {pages}" : "")}</p>
            <!-- .tbl-wrap is what keeps a four-column table from widening the whole document on a
                 phone: its min-content width is ~567px, so without a scroll container every PCI World
                 page rendered at 320-414px got a horizontal scrollbar and the layout drifted. -->
            <div class="tbl-wrap">
              <table>
                <caption class="visually-hidden">Published PCI World challenges, filtered by the controls above</caption>
                <thead><tr><th scope="col">Challenge</th><th scope="col">Industry</th><th scope="col">Difficulty</th><th scope="col">Time</th></tr></thead>
                <tbody>{items}</tbody>
              </table>
            </div>
            {pager}
            </div>
            """,
            "/world/archive");
    }

    public static string About(Db db)
    {
        var inst = E(InstituteUrl(db));
        return Layout(db,
            "About PCI World — operated by the Project Controls Institute",
            "What PCI World is, how challenges are built and scored, and how it relates to the Project Controls Institute and its certifications.",
            $"""
            <span class="kicker">About</span>
            <h1>About PCI World</h1>
            <p class="lede">You do not need project experience to enter. You will leave with evidence that you can think like a project professional.</p>
            <div class="card">
              <h2 style="margin-top:0">What a challenge is</h2>
              <p>Each challenge is a realistic project moment built on synthetic data: a situation, an evidence pack, the calculations a professional would run, and the judgement calls they would face. Scoring is deterministic — the same answers always earn the same result — and every decision shows its consequence afterwards, so the result is something you can defend, not a trophy.</p>
              <p style="margin-top:14px">Challenges span five difficulty levels, from foundation to expert, across five professional tracks — project controls, project management, project finance, governed AI and cross-functional work — and a broad range of industries. The daily rotation brings one forward each day; the <a href="/world/archive">Challenge Library</a> keeps every published challenge playable.</p>
            </div>
            <div class="card">
              <h2 style="margin-top:0">How scoring works</h2>
              <p>A challenge grades two different kinds of work, because real project roles demand both:</p>
              <div class="defn">
                <div><b>Calculation</b><span>The numeric asks — variances, indices, forecasts — are graded against a reference solver with a stated tolerance. Type the numbers the way the evidence shows them; formatting is forgiven, method is not.</span></div>
                <div><b>Decision quality</b><span>The judgement calls are scored against an authored rubric in which every option carries a consequence. After you submit, the debrief replays each decision: what you chose, what it would have caused, and the principle behind the strongest available call.</span></div>
                <div><b>Decision profile</b><span>Your pattern of choices maps to a named profile — a description of <em>how</em> you decide, not a grade or a rank. Two people with the same score can carry different profiles, and both can be right.</span></div>
              </div>
            </div>
            <div class="card">
              <h2 style="margin-top:0">The Passport</h2>
              <p>A free account turns completed challenges into a PCI World Passport: a page of verified practice evidence that its owner controls entirely. Publication is consent at every level — you choose which results appear, and separately whether scores, decision profiles and completion dates are shown at all. Answers are never published, to anyone, under any setting.</p>
              <p style="margin-top:14px">A published Passport is one link. Hand it to a recruiter or a colleague and they are looking at the live record — not a claim about it. The owner can rotate the link, set it to expire, or withdraw it at any moment, and a withdrawn link simply stops resolving. The matching one-page PDF says the same thing and points back to the live page, so a stale copy can never masquerade as current.</p>
              <p style="margin-top:14px"><a href="/world/account">Start your Passport</a> &middot; <a href="/world/verify">Verify a Passport you&rsquo;ve been given</a></p>
            </div>
            <div class="card">
              <h2 style="margin-top:0">What PCI World is not</h2>
              <p>PCI World does not rank participants, publish leaderboards, count members in public, or compare one person with another — there is no honest basis for any of that in practice data. It is not a certification: completing a challenge grants no credential and affects no standing with the Institute. What it offers is narrower and more defensible — reproducible evidence of practice, on the record, on your terms.</p>
            </div>
            <div class="card">
              <h2 style="margin-top:0">The Project Controls Institute</h2>
              <p>{E(OperatedBy)} The Institute is the certification authority; PCI World is its open practice ground. Completing challenges builds skill and shareable evidence — formal credentials are earned only through the Institute&rsquo;s own examinations.</p>
              <p><a href="{inst}" target="_blank" rel="noopener noreferrer">{InstituteLinkLabel} <span aria-hidden="true">&#8599;</span></a> for certifications, membership and the profession&rsquo;s body of knowledge.</p>
            </div>
            <p class="notice">{E(PracticeNotice)}</p>
            """,
            "/world/about");
    }

    public static string PublicResult(Db db, Dictionary<string, object?> attempt, Dictionary<string, object?> version)
    {
        var name = H.Str(attempt["display_name"]);
        var who = string.IsNullOrWhiteSpace(name) ? "A PCI World participant" : name!;
        var title = H.Str(version["title"]) ?? "PCI World Challenge";
        string share = "";
        try
        {
            var cfg = System.Text.Json.JsonDocument.Parse(H.Str(version["config_json"]) ?? "{}").RootElement;
            if (cfg.TryGetProperty("share_line", out var sl) && sl.ValueKind == System.Text.Json.JsonValueKind.String)
                share = sl.GetString() ?? "";
        }
        catch { }
        var dims = "";
        try
        {
            var d = System.Text.Json.JsonDocument.Parse(H.Str(attempt["dimensions_json"]) ?? "{}").RootElement;
            if (d.TryGetProperty("calculation", out var c) && c.ValueKind == System.Text.Json.JsonValueKind.Number)
                dims += $"<div><span class=\"kicker\">Calculation</span><b class=\"score num\">{c.GetDouble():0.#}</b></div>";
            if (d.TryGetProperty("decision", out var q) && q.ValueKind == System.Text.Json.JsonValueKind.Number)
                dims += $"<div><span class=\"kicker\">Decision quality</span><b class=\"score num\">{q.GetDouble():0.#}</b></div>";
        }
        catch { }
        return Layout(db,
            $"Verified result — {title} — PCI World",
            $"{who} completed the PCI World challenge “{title}”.",
            $"""
            <span class="kicker">Verified PCI World result</span>
            <h1>{E(title)}</h1>
            <div class="card">
              <p class="lede" style="margin-bottom:14px">{E(who)}</p>
              <div class="dim">
                <div><span class="kicker">Overall</span><b class="score num">{H.D(attempt["score"]):0.#}</b></div>
                {dims}
              </div>
              <h2 style="margin-top:10px">{E(H.Str(attempt["profile_key"]))}</h2>
              {(share.Length > 0 ? $"<p>{E(share)}</p>" : "")}
              <p class="meta"><span>Completed {E((H.Str(attempt["completed_at"]) ?? "").Split(' ')[0])}</span>
                 <span>Verified by PCI World</span></p>
            </div>
            <p><a class="btn" href="/world">Take today&rsquo;s challenge yourself</a></p>
            <p class="notice">This page shows a verified practice result. Participant answers are never published. {E(PracticeNotice)}</p>
            """,
            "/world",
            ogTitle: $"{who} — {title} — PCI World Challenge",
            ogDesc: share.Length > 0 ? share : $"Verified PCI World challenge result: {title}.",
            // A shared result is for the person the participant sent it to, not for a search index.
            // Revoking the link removes the page but cannot remove it from an index or a cache, so
            // this surface stays out of the index by default while remaining fully shareable —
            // social unfurls read the og: tags directly and are unaffected.
            noindex: true);
    }

    /// <summary>
    /// Email verification page (journey repair P1-03). States: "confirm" — the token is valid and
    /// verification happens only on the deliberate button press (the POST consumes it; a GET from
    /// a mail scanner or link preview changes nothing); "already" — the account is verified, the
    /// press would be a no-op and says so; "invalid" — expired, used or malformed.
    /// </summary>
    public static string VerifyEmail(Db db, string state) => Layout(db,
        state switch
        {
            "confirm" => "Confirm your email — PCI World",
            "already" => "Email already verified — PCI World",
            _ => "Verification link invalid — PCI World",
        },
        "PCI World email verification.",
        state switch
        {
            "confirm" => """
              <span class="kicker">Account</span>
              <h1>Confirm your email address</h1>
              <div class="card" style="max-width:480px">
                <p class="lede" style="margin-top:0">Press the button to verify this email for your PCI World account.
                Nothing happens until you do — opening this link alone changes nothing.</p>
                <p><button class="btn" id="ve_go">Verify my email</button></p>
                <p id="ve_msg" role="status"></p>
              </div>
              <script>
              (function(){
              'use strict';
              var btn=document.getElementById('ve_go'),msg=document.getElementById('ve_msg');
              btn.addEventListener('click',function(){
                btn.disabled=true;
                var t=new URLSearchParams(location.search).get('t')||'';
                fetch('/api/world/account/verify-email',{method:'POST',headers:{'Content-Type':'application/json'},
                  body:JSON.stringify({token:t})})
                .then(function(r){return r.json().then(function(j){if(!r.ok)throw j;return j;});})
                .then(function(){msg.innerHTML='<span class="ok">Email verified.</span> <a href="/world/account">Go to your account</a> to continue.';})
                .catch(function(e){btn.disabled=false;
                  msg.innerHTML='<span class="bad">'+((e&&e.message)||'That link is no longer valid — request a new one from your account page.')+'</span>';});
              });
              })();
              </script>
              """,
            "already" => """
              <h1>This email is already verified</h1>
              <p class="lede">Nothing more to do — your PCI World account email is confirmed and this link has no further effect.</p>
              <p><a class="btn" href="/world/account">Go to your account</a></p>
              """,
            _ => """
              <h1>That link didn&rsquo;t work</h1>
              <p class="lede">The verification link is invalid, has expired, or was already used. Sign in and request a new one from your account page — sending again is free and takes seconds.</p>
              <p><a class="btn" href="/world/account">Go to your account</a></p>
              """,
        },
        "/world/account", noindex: true);

    public static string ResetPassword(Db db) => Layout(db,
        "Reset your password — PCI World",
        "Choose a new PCI World account password.",
        """
        <span class="kicker">Account</span>
        <h1>Choose a new password</h1>
        <div class="card" style="max-width:420px">
          <label for="rp_pw">New password (min 10 characters)</label>
          <input id="rp_pw" type="password" autocomplete="new-password">
          <p style="margin-top:12px"><button class="btn" id="rp_go">Set new password</button></p>
          <p id="rp_msg" role="alert"></p>
        </div>
        <script>
        (function(){
        'use strict';
        document.getElementById('rp_go').addEventListener('click', function(){
          var t = new URLSearchParams(location.search).get('t') || '';
          fetch('/api/world/account/reset', { method:'POST', headers:{'Content-Type':'application/json'},
            body: JSON.stringify({ token: t, password: document.getElementById('rp_pw').value }) })
          .then(function(r){ return r.json().then(function(j){ if(!r.ok) throw j; return j; }); })
          .then(function(){ document.getElementById('rp_msg').innerHTML =
            '<span class="ok">Password changed.</span> <a href="/world/account">Sign in</a>'; })
          .catch(function(e){ document.getElementById('rp_msg').textContent =
            (e && e.message) || 'This link is invalid or expired — request a new one from the sign-in page.'; });
        });
        })();
        </script>
        """,
        "/world/account", noindex: true);

    /// <summary>Public Passport: consent-based, name-led, evidence only — never an email, never an
    /// answer, never presented as a credential.</summary>
    public static string PublicPassport(Db db, string name, List<Dictionary<string, object?>> rows,
        WorldPassport.Disclosure? show = null, string? verifyUrl = null, string? token = null, string? expiresAt = null,
        string? photoUrl = null, long? totalCompleted = null, long? totalIndustries = null, long? totalTracks = null)
    {
        // Field-level disclosure is enforced HERE, at render, not by hiding columns in CSS: a value
        // the owner did not publish never reaches the page at all.
        show ??= new WorldPassport.Disclosure(true, true, true);
        var items = string.Join("", rows.Select(r => $"""
            <tr>
              <td>{E(H.Str(r["title"]))}<br>
                <small class="num" style="color:var(--slate)"><a href="/world/challenge/{E(H.Str(r["code"]))}">{E(H.Str(r["code"]))}</a> &middot; v{H.L(r["version"])}</small></td>
              <td>{E(H.Str(r["industry"]))}</td>
              <td>{E(Cap(H.Str(r["difficulty"])))}</td>
              {(show.Scores ? $"<td class=\"num\">{H.D(r["score"]):0.#}</td>" : "")}
              {(show.Profiles ? $"<td>{E(Cap((H.Str(r["profile_key"]) ?? "").Replace('_', ' ')))}</td>" : "")}
              {(show.Dates ? $"<td class=\"num\">{E((H.Str(r["completed_at"]) ?? "").Split(' ')[0])}</td>" : "")}
            </tr>
            """));
        // Whole-history totals when the caller supplies them (P1-06); the page of rows is only a
        // window, and the stats must never understate a long history.
        var completedTotal = totalCompleted ?? rows.Count;
        var industries = totalIndustries ?? rows.Select(r => H.Str(r["industry"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count();
        var tracks = totalTracks ?? rows.Select(r => H.Str(r["track"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count();
        var truncated = completedTotal > rows.Count
            ? $"<p class=\"meta\"><span>Showing the {rows.Count} most recent of {completedTotal} published challenges.</span></p>"
            : "";
        var verifyBlock = verifyUrl is null ? "" : $"""
            <div class="card" id="verify">
              <span class="kicker">Verification</span>
              <div style="display:flex;gap:30px;flex-wrap:wrap;align-items:flex-start">
                <div style="flex:0 0 auto;border:1.5px solid var(--line);border-radius:12px;padding:12px;background:#fff;
                            box-shadow:var(--shadow-rest)">{WorldPassport.QrSvg(verifyUrl)}</div>
                <div style="flex:1;min-width:260px">
                  <h2 style="margin-top:0">Verify this Passport</h2>
                  <p>This page <em>is</em> the record. Scan the code or open the address below to
                     confirm you are looking at the live version — a PDF or screenshot can be out of
                     date, and its owner can withdraw this link at any time.</p>
                  <p class="num" style="word-break:break-all"><a href="{E(verifyUrl)}">{E(verifyUrl)}</a></p>
                  <p><a class="btn secondary" href="/world/p/{E(token)}.pdf">Download as PDF</a></p>
                  {(string.IsNullOrWhiteSpace(expiresAt) ? "" : $"<p class=\"meta\"><span>Link expires {E(expiresAt.Split(' ')[0])}</span></p>")}
                </div>
              </div>
            </div>
            """;
        return Layout(db,
            $"{name} — PCI World Passport",
            $"Verified virtual project experience: {completedTotal} completed PCI World challenge{(completedTotal == 1 ? "" : "s")} across {industries} industr{(industries == 1 ? "y" : "ies")}.",
            $"""
            <div class="ppt-cover" style="margin-top:6px">
              <div class="ppt-lines" aria-hidden="true"></div>
              <div class="ppt-top">
                <span class="wordmark">PCI World</span><span class="bar" aria-hidden="true"></span>
                <span class="ppt-from">From the Project<br>Controls Institute</span>
                <span class="ppt-word">Passport</span>
              </div>
              <div style="position:relative;display:flex;gap:28px;flex-wrap:wrap;align-items:center;justify-content:space-between">
                {(photoUrl is null ? "" : $"<img class=\"ppt-photo\" src=\"{E(photoUrl)}\" alt=\"Photograph of {E(name)}, provided by the Passport owner\">")}
                <div style="flex:1;min-width:240px">
                  <span class="ppt-kicker">Verified virtual project experience</span>
                  <h1 class="ppt-name">{E(name)}</h1>
                  <p class="ppt-sub">Practice evidence published by its owner &middot; Verified by PCI World</p>
                </div>
                {SealSvg()}
              </div>
              <div class="ppt-stats">
                <div><span class="kicker">Challenges</span><b class="score num">{completedTotal}</b></div>
                <div><span class="kicker">Industries</span><b class="score num">{industries}</b></div>
                <div><span class="kicker">Tracks</span><b class="score num">{tracks}</b></div>
              </div>
              <div class="ppt-foot">
                <span>Operated by the Project Controls Institute</span>
                <span>Practice evidence &middot; not a certification</span>
              </div>
            </div>
            <div class="card">
              <h2 style="margin-top:0">Selected evidence</h2>
              <p style="color:var(--slate);margin-bottom:6px">Each row is a completed challenge its owner chose to publish{(show.Scores || show.Profiles || show.Dates ? "" : " — titles only, by their choice")}.</p>
              <div class="tbl-wrap">
              <table>
                <caption class="visually-hidden">Challenges this participant chose to publish</caption>
                <thead><tr><th scope="col">Challenge</th><th scope="col">Industry</th><th scope="col">Difficulty</th>
                {(show.Scores ? "<th scope=\"col\">Score</th>" : "")}
                {(show.Profiles ? "<th scope=\"col\">Decision profile</th>" : "")}
                {(show.Dates ? "<th scope=\"col\">Date</th>" : "")}</tr></thead>
                <tbody>{items}</tbody>
              </table>
              </div>
              {truncated}
            </div>
            <div class="card">
              <h2 style="margin-top:0">How to read this Passport</h2>
              <div class="defn">
                {(show.Scores ? "<div><b>Scores</b><span>Deterministic, out of 100: the same answers always earn the same score, so a number here is reproducible arithmetic against a reference solution — not an opinion, and not a curve.</span></div>" : "")}
                {(show.Profiles ? "<div><b>Decision profiles</b><span>A named description of how this person decides under pressure — evidence-led, schedule-first, and so on. Profiles characterise judgement style; they do not rank people.</span></div>" : "")}
                <div><b>Traceability</b><span>Every row cites the challenge code and the exact published version it was completed against. Published versions are immutable, so the challenge behind the citation is the same one this participant faced — follow the code to read the brief yourself.</span></div>
                <div><b>Consent</b><span>Every row was published deliberately, item by item. Fields the owner withheld are absent from this page entirely, and answers are never shown to anyone.</span></div>
                <div><b>The live record</b><span>This page is served from PCI World&rsquo;s records at the moment you load it. If the owner withdraws the link, it stops resolving — which is exactly what makes it worth trusting while it does.</span></div>
              </div>
            </div>
            {verifyBlock}
            <p><a class="btn" href="/world">Take today&rsquo;s challenge yourself</a></p>
            <p class="notice">This Passport shows verified practice evidence its owner chose to publish. Answers are never shown, and nothing here ranks or compares people. {E(PracticeNotice)}</p>
            """,
            "/world",
            ogTitle: $"{name} — PCI World Passport",
            ogDesc: $"Verified virtual project experience: {rows.Count} completed PCI World challenges.",
            // Same reasoning as a shared result: the Passport token is revocable and rotatable, and
            // a search index would outlive both. Sharing the link still works everywhere.
            noindex: true);
    }

    // ───────────────────────────── editorial surfaces ─────────────────────────────

    /// <summary>Listing page for a kind (blog or news). Paginated, so the archive stays reachable
    /// and crawlable however many articles exist.</summary>
    public static string ArticleIndex(Db db, string kind, List<Dictionary<string, object?>> rows,
        long total, int page, int pages)
    {
        var isNews = kind == "news";
        var items = rows.Count == 0
            ? "<p class=\"lede\">Nothing published here yet.</p>"
            : string.Join("", rows.Select(r => $"""
                <article class="card">
                  <span class="kicker">{E(FormatDate(H.Str(r["published_at"])))} &middot; {H.L(r["reading_minutes"])} min read</span>
                  <h2 style="margin-top:0"><a href="/world/{E(kind)}/{E(H.Str(r["slug"]))}">{E(H.Str(r["title"]))}</a></h2>
                  <p>{E(H.Str(r["dek"]))}</p>
                  <p class="meta"><span>{E(H.Str(r["author_name"]))}</span></p>
                </article>
                """));
        string PageHref(int p) => $"/world/{kind}" + (p > 1 ? $"?page={p}" : "");
        var pager = pages <= 1 ? "" : $"""
            <nav class="pager" aria-label="Pages" style="display:flex;gap:12px;align-items:center;margin-top:14px">
              {(page > 1 ? $"<a class=\"btn secondary\" href=\"{PageHref(page - 1)}\" rel=\"prev\">Previous</a>" : "")}
              <span class="kicker">Page {page} of {pages}</span>
              {(page < pages ? $"<a class=\"btn secondary\" href=\"{PageHref(page + 1)}\" rel=\"next\">Next</a>" : "")}
            </nav>
            """;
        return Layout(db,
            isNews ? "Newsroom — PCI World" : "Writing — PCI World",
            isNews
                ? "Project controls news, with every material claim traceable to a named source."
                : "Practical writing on project controls: how the techniques behave, and where they catch people out.",
            $"""
            {Breadcrumb(("PCI World", "/world"), (isNews ? "Newsroom" : "Writing", null))}
            <span class="kicker">{(isNews ? "Newsroom" : "Writing")}</span>
            <h1>{(isNews ? "Project controls, as it happens" : "How the techniques actually behave")}</h1>
            <p class="lede">{(isNews
                ? "Reporting on the projects and decisions that shape the profession. Every material claim carries a source you can open."
                : "Written by practitioners for practitioners. No statistics without a source, no advice without a reason, and every piece tied to something you can go and practise.")}</p>
            <p class="kicker">{total} article{(total == 1 ? "" : "s")}</p>
            {items}
            {pager}
            """,
            $"/world/{kind}");
    }

    /// <summary>
    /// A published article. Serves the immutable version snapshot, shows any corrections as a dated
    /// visible record, and emits BlogPosting/NewsArticle + BreadcrumbList structured data — only the
    /// fields the visible page actually supports, per the SEO policy.
    /// </summary>
    public static string ArticlePage(Db db, string kind, Dictionary<string, object?> article,
        Dictionary<string, object?> version, List<Dictionary<string, object?>> sources)
    {
        var isNews = kind == "news";
        var title = H.Str(version["title"]) ?? "";
        var dek = H.Str(version["dek"]) ?? "";
        var author = H.Str(version["author_name"]) ?? WorldEditorial.EditorialByline;
        var slug = H.Str(article["slug"]) ?? "";
        var published = FormatDate(H.Str(article["published_at"]));
        var corrections = WorldEditorial.ParseCorrections(H.Str(article["corrections_json"]));
        var url = WorldUrl.Base() + $"/world/{kind}/{slug}";

        var correctionBlock = corrections.Count == 0 ? "" : $"""
            <div class="card" style="border-color:var(--crimson)">
              <h2 style="margin-top:0">Corrections</h2>
              <ul>{string.Join("", corrections.Select(c => $"<li><b>{E(c.Date)}</b> — {E(c.Note)}</li>"))}</ul>
              <p><small>Published text is versioned. Corrections are appended here and never applied silently.</small></p>
            </div>
            """;

        var sourceBlock = sources.Count == 0 ? "" : $"""
            <div class="card">
              <h2 style="margin-top:0">Sources</h2>
              <ol>{string.Join("", sources.Select(s => $"""
                <li><a href="{E(H.Str(s["url"]))}" rel="nofollow noopener" target="_blank">{E(H.Str(s["title"]) ?? H.Str(s["url"]))}</a>
                    {(string.IsNullOrWhiteSpace(H.Str(s["publisher"])) ? "" : $" — {E(H.Str(s["publisher"]))}")}
                    {(string.IsNullOrWhiteSpace(H.Str(s["published_at"])) ? "" : $" ({E(FormatDate(H.Str(s["published_at"])))})")}
                    {(string.IsNullOrWhiteSpace(H.Str(s["claim"])) ? "" : $"<br><small>Supports: {E(H.Str(s["claim"]))}</small>")}</li>
                """))}</ol>
            </div>
            """;

        // Structured data describing exactly what the page shows — no ratings, no invented author
        // identity, no claims the visible page does not make.
        var ld = Json(new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = isNews ? "NewsArticle" : "BlogPosting",
            ["headline"] = title,
            ["description"] = dek,
            ["datePublished"] = H.Str(article["published_at"])?.Replace(' ', 'T'),
            ["dateModified"] = H.Str(article["updated_at"])?.Replace(' ', 'T'),
            ["author"] = new Dictionary<string, object?> { ["@type"] = "Organization", ["name"] = author },
            ["publisher"] = new Dictionary<string, object?> { ["@type"] = "Organization", ["name"] = "PCI World" },
            ["mainEntityOfPage"] = url,
            ["isAccessibleForFree"] = true,
        });

        return Layout(db, $"{title} — PCI World", dek,
            $"""
            <script type="application/ld+json">{ld}</script>
            {Breadcrumb(("PCI World", "/world"), (isNews ? "Newsroom" : "Writing", $"/world/{kind}"), (title, null))}
            <span class="kicker">{E(published)} &middot; {WorldEditorial.ReadingMinutes(H.Str(version["body_md"]))} min read</span>
            <h1>{E(title)}</h1>
            <p class="lede">{E(dek)}</p>
            <p class="meta"><span>{E(author)}</span>{(corrections.Count > 0 ? "<span>Corrected</span>" : "")}</p>
            <div class="prose">{WorldEditorial.RenderBody(H.Str(version["body_md"]))}</div>
            {sourceBlock}
            {correctionBlock}
            <div class="card card--noir">
              <span class="kicker">Practise it</span>
              <h2 style="margin-top:0">Reading about it is half the work</h2>
              <p>PCI World turns these situations into challenges you actually have to decide. Free, anonymous, no account needed.</p>
              <p><a class="btn" href="/world">Take today&rsquo;s challenge</a>
                 <a class="btn secondary" href="/world/archive">Browse the library</a></p>
            </div>
            <p class="notice">{E(PracticeNotice)}</p>
            """,
            $"/world/{kind}/{slug}",
            ogTitle: title, ogDesc: dek);
    }

    /// <summary>A visible breadcrumb trail plus its BreadcrumbList structured data — the two must
    /// agree, so they are generated together.</summary>
    static string Breadcrumb(params (string Label, string? Href)[] crumbs)
    {
        var links = string.Join(" <span aria-hidden=\"true\">/</span> ", crumbs.Select(c =>
            c.Href is null ? $"<span aria-current=\"page\">{E(c.Label)}</span>" : $"<a href=\"{E(c.Href)}\">{E(c.Label)}</a>"));
        var items = crumbs.Select((c, i) => new Dictionary<string, object?>
        {
            ["@type"] = "ListItem",
            ["position"] = i + 1,
            ["name"] = c.Label,
            ["item"] = c.Href is null ? null : WorldUrl.Base() + c.Href,
        }).ToList();
        var ld = Json(new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "BreadcrumbList",
            ["itemListElement"] = items,
        });
        return $"""
            <script type="application/ld+json">{ld}</script>
            <nav aria-label="Breadcrumb" class="crumbs">{links}</nav>
            """;
    }

    static string FormatDate(string? sqlDate) => (sqlDate ?? "").Split(' ')[0];

    /// <summary>
    /// Passport verification entry point — the page someone lands on when they have been handed a
    /// PDF or a link and want to know whether it is real. It answers exactly one question and does
    /// not editorialise: either the link resolves to a live Passport, or it does not.
    /// </summary>
    public static string VerifyPassport(Db db, string? attempted, string? _) => Layout(db,
        attempted is null ? "Verify a PCI World Passport" : "That Passport link does not resolve — PCI World",
        "Check whether a PCI World Passport link is live, and see the record it points to.",
        $"""
        <span class="kicker">Verification</span>
        <h1>Verify a PCI World Passport</h1>
        <p class="lede">Paste the link or the code from the document you were given. You will be taken
           to the live record, which is the only authority — documents can be copies, and their owners
           can withdraw them.</p>
        {(attempted is null ? "" : $"""
        <div class="card" style="border-color:var(--crimson)">
          <h2 style="margin-top:0">That link does not resolve</h2>
          <p>Nothing live is published at that address. There are three ordinary explanations:
             the owner has withdrawn or replaced the link, the link has passed the expiry its owner
             set, or the address was mistyped or truncated when it was copied.</p>
          <p class="notice">A link that does not resolve is not evidence that a document was forged —
             but it does mean nothing here can confirm it. Ask the holder for a current link.</p>
        </div>
        """)}
        <form class="card" method="get" action="/world/verify">
          <label for="vt">Passport link or code</label>
          <input id="vt" name="t" type="text" style="max-width:520px" value="{E(attempted)}"
                 placeholder="https://&hellip;/world/p/&hellip; or the code alone" autocomplete="off">
          <p style="margin-top:14px"><button class="btn" type="submit">Verify</button></p>
        </form>
        <p class="notice">{E(PracticeNotice)}</p>
        """,
        "/world/verify");

    /// <summary>404 for a PCI World-only deployment. A real not-found page, not a redirect to
    /// /world — a blanket redirect tells crawlers every mistyped URL is a live duplicate.</summary>
    public static string NotFound(Db db) => Layout(db,
        "Page not found — PCI World",
        "That page does not exist on PCI World.",
        """
        <span class="kicker">404</span>
        <h1>That page doesn&rsquo;t exist</h1>
        <p class="lede">The link may be mistyped, or the page may have moved.</p>
        <p><a class="btn" href="/world">Go to today&rsquo;s challenge</a>
           <a class="btn secondary" href="/world/archive">Browse the archive</a></p>
        """,
        "/world", noindex: true);

    /// <summary>Account page: register/sign-in, then Passport management. All state via the JSON
    /// API; this shell renders no personal data server-side.</summary>
    public static string Account(Db db) => Layout(db,
        "Your PCI World account",
        "Create a free PCI World account to keep your challenge evidence and build a shareable Passport.",
        $"""
        <span class="kicker">Account &amp; Passport</span>
        <h1>Keep the evidence</h1>
        <p class="lede">A free account turns completed challenges into a PCI World Passport — verified virtual project experience you control and can share.</p>
        <template id="sealTpl">{SealSvg(74)}</template>
        <div id="auth" class="card" hidden tabindex="-1">
          <div style="display:grid;gap:34px;grid-template-columns:repeat(auto-fit,minmax(260px,1fr))">
            <div>
              <h2 style="margin-top:0">Create your Passport</h2>
              <p style="color:var(--slate);font-size:15px;margin-bottom:4px">Free, and it always will be. Challenges you completed anonymously in this browser join your record automatically.</p>
              <label for="r_name">Display name (shown on your public Passport)</label><input id="r_name" type="text" maxlength="80" autocomplete="name">
              <label for="r_email">Email</label><input id="r_email" type="email" autocomplete="email">
              <label for="r_pw">Password (min 10 characters)</label><input id="r_pw" type="password" autocomplete="new-password">
              <p style="margin-top:16px"><button class="btn" id="doRegister">Create account</button></p>
            </div>
            <div class="auth-alt">
              <h2 style="margin-top:0">Sign in</h2>
              <p style="color:var(--slate);font-size:15px;margin-bottom:4px">Welcome back — your evidence is where you left it.</p>
              <label for="l_email">Email</label><input id="l_email" type="email" autocomplete="email">
              <label for="l_pw">Password</label><input id="l_pw" type="password" autocomplete="current-password">
              <p style="margin-top:16px"><button class="btn secondary" id="doLogin">Sign in</button>
                 <button class="btn secondary" id="doForgot" type="button">Forgot password</button></p>
            </div>
          </div>
          <p id="autherr" class="bad" role="alert"></p>
          <p class="notice">Challenges you completed anonymously in this browser are added to your account automatically. {E(PracticeNotice)}</p>
        </div>
        <div id="me" hidden tabindex="-1"></div>
        <h2 class="sec">How your Passport works</h2>
        <div class="uline" aria-hidden="true"></div>
        <div class="card">
          <ol class="steps">
            <li><div><b>Practise</b>Complete challenges — anonymously or signed in. Every completed challenge becomes a verified record: title, industry, difficulty, score, decision profile, date.</div></li>
            <li><div><b>Choose what the world sees</b>Publication is consent at every level. Pick the results that appear, then decide separately whether scores, decision profiles and completion dates are shown at all. Answers are never published.</div></li>
            <li><div><b>Share one link</b>Publish and you get a single address — and a matching one-page PDF. Anyone you hand it to is reading the live record, and you can rotate, expire or withdraw the link whenever you choose.</div></li>
          </ol>
        </div>
        <div class="card">
          <h2 style="margin-top:0">Privacy, plainly</h2>
          <div class="defn">
            <div><b>Your email</b><span>Used to sign in and to verify the account. It never appears on your Passport, the PDF, or any public page.</span></div>
            <div><b>Your answers</b><span>Grading detail stays between you and the scoring engine. No public surface shows an answer, under any setting.</span></div>
            <div><b>Your link</b><span>Stored only as a hash — the server itself cannot reprint it. Generating a new link retires the old one immediately.</span></div>
            <div><b>Leaving</b><span>Export your PCI World data as JSON at any time. Deleting your PCI World participation removes your World sign-in, Passport and every public link — your PCI student account and any certifications are untouched; completed challenges survive only as anonymous statistics.</span></div>
          </div>
        </div>
        <script>{AccountJs}</script>
        """,
        "/world/account", noindex: true);

    const string AccountJs = """
        (function(){
        'use strict';
        var KEY='world_account';
        function $(id){return document.getElementById(id);}
        function esc(s){var d=document.createElement('span');d.textContent=s==null?'':String(s);return d.innerHTML;}
        function api(path,body,method){
          return fetch(path,{method:method||(body?'POST':'GET'),headers:{'Content-Type':'application/json',
            'X-World-Account':localStorage.getItem(KEY)||'',
            'X-World-Session':localStorage.getItem('world_session')||''},
            body:body?JSON.stringify(body):undefined})
          .then(function(r){return r.json().then(function(j){if(!r.ok)throw j;return j;});});
        }
        // Every hidden/shown pair moves focus into the panel that just appeared. Toggling `hidden`
        // on the container holding the focused button silently drops focus to <body>, which strands
        // keyboard users and announces nothing at all.
        function showAuth(){$('auth').hidden=false;$('me').hidden=true;$('auth').focus();}
        // Returns its promise: load() replaces the whole panel, so anything that wants to leave a
        // message on screen has to write it AFTER the re-render, onto the node that survives.
        function load(){
          return api('/api/world/passport').then(function(p){
            $('auth').hidden=true;$('me').hidden=false;$('me').focus();
            var seal=(document.getElementById('sealTpl')||{innerHTML:''}).innerHTML;
            function pretty(s){s=(s||'').replace(/_/g,' ');return s?s.charAt(0).toUpperCase()+s.slice(1):'';}
            // The cover: the owner's own Passport drawn as the artefact a reader of the public
            // page will see — same seal, same stats, same honesty line.
            var h='<div class="ppt-cover">'+
              '<div class="ppt-lines" aria-hidden="true"></div>'+
              '<div class="ppt-top"><span class="wordmark">PCI World</span><span class="bar" aria-hidden="true"></span>'+
              '<span class="ppt-from">From the Project<br>Controls Institute</span>'+
              '<span class="ppt-word">Passport</span></div>'+
              '<div style="position:relative;display:flex;gap:28px;flex-wrap:wrap;align-items:center;justify-content:space-between">'+
              (p.has_photo?'<img id="photoPrev" class="ppt-photo" alt="Your Passport photograph">':'')+
              '<div style="flex:1;min-width:240px"><span class="ppt-kicker">Verified virtual project experience</span>'+
              '<h2 class="ppt-name" style="font-size:clamp(26px,4vw,38px)">'+esc(p.display_name||'Unnamed participant')+'</h2>'+
              '<p class="ppt-sub">'+(p.passport_public?'Published — anyone with your link sees the live record.':'Private — nothing is public until you publish.')+'</p></div>'+
              seal+'</div>'+
              '<div class="ppt-stats">'+
              '<div><span class="kicker">Completed</span><b class="score num">'+p.completed+'</b></div>'+
              '<div><span class="kicker">Industries</span><b class="score num">'+p.industries+'</b></div>'+
              '<div><span class="kicker">Tracks</span><b class="score num">'+p.tracks+'</b></div></div>'+
              '<div class="ppt-foot"><span>Operated by the Project Controls Institute</span>'+
              '<span>Practice evidence &middot; not a certification</span></div>'+
              '</div>';
            // The panel is where practice and evidence meet: the next challenge is always one
            // click away, and every completed one below is traceable to its published version.
            h+='<div class="card"><span class="kicker">Challenges</span>'+
              '<h2 style="margin-top:0">Keep practising</h2>'+
              '<p style="color:var(--slate)">A new challenge is published every day (00:00 UTC), and the archive keeps every published one playable. Each challenge you complete becomes a traceable record below &mdash; pinned to the exact published version you faced.</p>'+
              '<p><a class="btn" href="/world">Today&rsquo;s challenge</a> '+
              '<a class="btn secondary" href="/world/archive">Browse the archive</a></p></div>';
            h+='<div class="card"><span class="kicker">Identity &amp; publication</span>'+
              '<h2 style="margin-top:0">Your name on the record</h2>'+
              '<label for="dn">Display name</label><input id="dn" maxlength="80" value="'+esc(p.display_name||'')+'">'+
              '<p style="margin-top:12px"><button class="btn secondary" id="saveName">Save name</button> '+
              (p.email_verified?'<span class="ok">Email verified.</span>'
                :'<span class="bad">Email not verified.</span> <button class="btn secondary" id="resend">Resend verification</button>')+'</p>'+
              // The optional photograph. Uploading it is the consent to show it: it appears on the
              // cover above and on the public page while published, and Remove deletes the stored
              // image itself, not just the link to it.
              '<label for="photoFile">Passport photograph (optional &mdash; shown on your public Passport)</label>'+
              '<input id="photoFile" type="file" accept="image/jpeg,image/png,image/webp">'+
              '<p style="margin-top:12px"><button class="btn secondary" id="photoUp">'+
              (p.has_photo?'Replace photo':'Upload photo')+'</button> '+
              (p.has_photo?'<button class="btn secondary" id="photoRm">Remove photo</button> ':'')+
              '<span id="photomsg" role="status"></span></p>'+
              '<p><small>JPEG, PNG or WebP, up to 3&nbsp;MB. Removing it deletes the image &mdash; it is not kept anywhere.</small></p>'+
              '<p style="margin-top:16px">'+
              (p.passport_public
                ?'<button class="btn secondary" id="unpub">Make Passport private</button>'
                :'<button class="btn" id="pub">Publish my public Passport</button>')+
              ' <span id="puburl" class="num" style="word-break:break-all"></span> <span id="pubmsg" class="bad" role="alert"></span></p>'+
              // The public link is stored only as a SHA, so the server genuinely cannot show it
              // again. This browser remembers the last one it minted; from anywhere else the
              // honest answer is "generate a new link", which rotates and retires the old one.
              (p.passport_public
                ?'<p id="lastlink" class="meta"></p>'+
                 '<p><button class="btn secondary" id="regen">Generate a new link</button> '+
                 '<small>The current link stops working immediately.</small></p>'
                :((p.email_verified&&p.display_name)?''
                  :'<p style="color:var(--slate);font-size:14px">Publishing needs a verified email and a display name — the two things that make the record worth reading.</p>'))+
              '</div>';
            // Field-level disclosure: publishing WHAT you have practised should never force you to
            // publish your scores as well. These switches apply to the public page and the PDF alike.
            h+='<div class="card"><span class="kicker">Disclosure</span>'+
               '<h2 style="margin-top:0">What your Passport shows</h2>'+
               '<fieldset style="padding-top:6px"><legend>Fields visible to anyone with your link</legend>'+
               '<div class="opt-row"><label for="sw_scores">Scores'+
               '<small>Out of 100 — deterministic, so a reader can trust the number</small></label>'+
               '<input type="checkbox" class="switch" id="sw_scores"'+(p.show_scores?' checked':'')+'></div>'+
               '<div class="opt-row"><label for="sw_profiles">Decision profiles'+
               '<small>How you decide — a style, never a rank</small></label>'+
               '<input type="checkbox" class="switch" id="sw_profiles"'+(p.show_profiles?' checked':'')+'></div>'+
               '<div class="opt-row"><label for="sw_dates">Completion dates'+
               '<small>When each challenge was completed</small></label>'+
               '<input type="checkbox" class="switch" id="sw_dates"'+(p.show_dates?' checked':'')+'></div>'+
               '<p><small>Challenge titles, industries and difficulty are always shown — without them '+
               'a Passport says nothing. Your answers are never shown.</small></p></fieldset>'+
               '<label for="sw_exp">Link expiry</label>'+
               '<select id="sw_exp">'+
               // The stored expiry is its own persisted setting: the dropdown must REPRESENT it,
               // not silently overwrite it. "Keep" is selected whenever an expiry exists, and only
               // an explicit different choice is ever sent to the server (journey repair P0-06).
               (p.expires_at?'<option value="keep" selected>Keep current expiry ('+esc(p.expires_at)+')</option>':'')+
               '<option value="0"'+(p.expires_at?'':' selected')+'>Never expires'+(p.expires_at?' (removes the current expiry)':'')+'</option>'+
               '<option value="90">Expires in 90 days</option>'+
               '<option value="180">Expires in 6 months</option>'+
               '<option value="365">Expires in 12 months</option>'+
               '</select>'+
               (p.expires_at?'<p class="meta"><span>Current link expires '+esc(p.expires_at)+'</span></p>':'')+
               '<p style="margin-top:16px"><button class="btn secondary" id="saveShow">Save these settings</button> '+
               '<span id="showmsg" role="status"></span></p>'+
               '</div>';
            h+='<div class="card"><span class="kicker">Evidence</span>'+
               '<h2 style="margin-top:0">Choose what appears</h2>'+
               '<p style="color:var(--slate)">Tick the results you want on your public Passport. Nothing is shown without your choice.</p>'+
               '<div class="tbl-wrap"><table><thead><tr><th scope="col">Show</th><th scope="col">Challenge</th>'+
               '<th scope="col">Score</th><th scope="col">Profile</th><th scope="col">Date</th></tr></thead><tbody>';
            function evRow(e2){
              return '<tr><td><input type="checkbox" class="ev-check" data-att="'+e2.attempt_id+'" '+(e2.passport_visible?'checked':'')+
                 ' aria-label="Show '+esc(e2.title)+' on public Passport"></td>'+
                 '<td><b>'+esc(e2.title)+'</b>'+
                 // Traceability line: code + immutable version, linking back to the challenge —
                 // the same citation the public Passport and the PDF carry.
                 '<br><small class="num"><a href="/world/challenge/'+encodeURIComponent(e2.code||'')+'">'+esc(e2.code||'')+'</a> &middot; v'+esc(e2.version)+'</small>'+
                 '<br><small style="color:var(--slate)">'+esc(e2.industry||'')+
                 (e2.difficulty?' &middot; '+esc(pretty(e2.difficulty)):'')+'</small></td>'+
                 '<td class="num">'+esc(e2.score)+'</td><td>'+esc(pretty(e2.profile))+'</td>'+
                 '<td class="num">'+esc((e2.completed_at||'').split(' ')[0])+'</td></tr>';
            }
            (p.evidence||[]).forEach(function(e2){h+=evRow(e2);});
            h+='</tbody></table></div>'+
               // Whole-history honesty (P1-06): the table is a window; the totals above are SQL
               // truth, and every older row stays reachable through Load more.
               (p.evidence_total>(p.evidence||[]).length
                 ?'<p class="meta"><span id="evshown" role="status">Showing '+(p.evidence||[]).length+' of '+p.evidence_total+' completed challenges</span></p>'+
                  '<p><button class="btn secondary" id="evmore">Load more</button></p>'
                 :'')+
               ((p.evidence||[]).length?'':'<p style="color:var(--slate)">No completed challenges yet — '+
                 '<a href="/world">today&rsquo;s challenge</a> takes five to ten minutes, and it will appear here the moment you finish.</p>')+
               '</div>'+
               '<div class="card"><span class="kicker">Your data</span>'+
               '<h2 style="margin-top:0">Yours to take or erase</h2>'+
               // A plain link cannot work here: the export is authenticated by the X-World-Account
               // header, which a navigation never sends, so this always answered 401. It is fetched
               // and saved as a blob instead.
               '<p><button class="btn secondary" id="dlexport">Export my data (JSON)</button> '+
               '<button class="btn secondary" id="signout">Sign out</button> '+
               '<button class="btn secondary" id="delacct">Delete my account</button></p>'+
               // A labelled password field, not window.prompt(): prompt() shows the password in
               // clear text, carries no label, cannot be styled or translated, and is blocked
               // outright by some browsers.
               '<div id="delbox" hidden class="card" style="border-color:var(--crimson)">'+
               '<h2 style="margin-top:0">Delete your PCI World participation</h2>'+
               '<p>This removes your PCI World Passport, every public link you minted, your World '+
               'preferences and your World sign-in. Completed challenges are kept as anonymous '+
               'statistics with nothing that identifies you.</p>'+
               '<p><b>Your PCI student account is not affected.</b> If you also hold a Project '+
               'Controls Institute student account (certifications, exams, payments), it stays '+
               'exactly as it is — deleting the complete PCI account is a separate action in the '+
               'student portal.</p>'+
               '<label for="delpw">Confirm your PCI password</label>'+
               '<input id="delpw" type="password" autocomplete="current-password">'+
               '<p style="margin-top:14px"><button class="btn" id="delgo">Delete my account permanently</button> '+
               '<button class="btn secondary" id="delno">Keep my account</button></p></div>'+
               '<p id="acctmsg" role="status"></p></div>';
            $('me').innerHTML=h;
            $('saveName').addEventListener('click',function(){
              api('/api/world/account/profile',{display_name:$('dn').value}).then(load);
            });
            // The cover preview is authenticated by the account header, which an <img> navigation
            // never sends — so it is fetched as a blob, the same way the data export is.
            if($('photoPrev')){
              fetch('/api/world/passport/photo',{headers:{'X-World-Account':localStorage.getItem(KEY)||''}})
                .then(function(r){if(!r.ok)throw r;return r.blob();})
                .then(function(b){$('photoPrev').src=URL.createObjectURL(b);})
                .catch(function(){});
            }
            $('photoUp').addEventListener('click',function(){
              var f=($('photoFile').files||[])[0];
              if(!f){$('photomsg').textContent='Choose an image first.';return;}
              var rd=new FileReader();
              rd.onload=function(){
                api('/api/world/passport/photo',{photo:rd.result})
                  .then(load)
                  .catch(function(e2){$('photomsg').textContent=(e2&&e2.message)||(e2&&e2.error)||'Could not upload the photo.';});
              };
              rd.readAsDataURL(f);
            });
            if($('photoRm'))$('photoRm').addEventListener('click',function(){
              api('/api/world/passport/photo',{remove:true}).then(load)
                .catch(function(){$('photomsg').textContent='Could not remove the photo.';});
            });
            if($('resend'))$('resend').addEventListener('click',function(){
              api('/api/world/account/resend-verification',{}).then(function(){$('acctmsg').textContent='Verification email sent.';});
            });
            if($('pub'))$('pub').addEventListener('click',function(){
              api('/api/world/passport/publish',{publish:true})
                .then(function(r){
                  localStorage.setItem('pciworld_passport_url',location.origin+r.url);
                  return load().then(function(){
                    $('puburl').innerHTML='<a href="'+esc(r.url)+'">'+esc(location.origin+r.url)+'</a>';});})
                .catch(function(e2){$('pubmsg').textContent=(e2&&e2.message)||(e2&&e2.error)||'Could not publish.';});
            });
            if($('unpub'))$('unpub').addEventListener('click',function(){
              api('/api/world/passport/publish',{publish:false}).then(load);
            });
            function bindEv(scope){
              scope.querySelectorAll('input[data-att]').forEach(function(cb){
                if(cb.dataset.bound)return;cb.dataset.bound='1';
                cb.addEventListener('change',function(){
                  api('/api/world/passport/evidence',{attempt_id:parseInt(cb.dataset.att,10),visible:cb.checked});
                });
              });
            }
            bindEv($('me'));
            if($('evmore')){
              var evCount=(p.evidence||[]).length;
              $('evmore').addEventListener('click',function(){
                $('evmore').disabled=true;
                api('/api/world/passport?offset='+evCount).then(function(q){
                  var tb=$('me').querySelector('table tbody');
                  (q.evidence||[]).forEach(function(e2){tb.insertAdjacentHTML('beforeend',evRow(e2));});
                  bindEv(tb);
                  evCount+=(q.evidence||[]).length;
                  $('evshown').textContent='Showing '+evCount+' of '+q.evidence_total+' completed challenges';
                  $('evmore').disabled=false;
                  if(!(q.evidence||[]).length||evCount>=q.evidence_total)$('evmore').hidden=true;
                }).catch(function(){$('evmore').disabled=false;});
              });
            }
            $('signout').addEventListener('click',function(){
              api('/api/world/account/logout',{}).catch(function(){});
              localStorage.removeItem(KEY);showAuth();
            });
            if($('lastlink')){
              var saved=localStorage.getItem('pciworld_passport_url');
              $('lastlink').innerHTML=saved
                ?'<span>Your link on this device: <a href="'+esc(saved)+'">'+esc(saved)+'</a></span>'
                :'<span>This browser has not minted a link. Generate a new one to get a shareable address.</span>';
            }
            if($('regen'))$('regen').addEventListener('click',function(){
              api('/api/world/passport/publish',{publish:true})
                .then(function(r){localStorage.setItem('pciworld_passport_url',location.origin+r.url);load();})
                .catch(function(e2){$('pubmsg').textContent=(e2&&e2.message)||(e2&&e2.error)||'Could not generate a link.';});
            });
            $('saveShow').addEventListener('click',function(){
              $('showmsg').textContent='';
              // expires_in_days is included ONLY when the owner chose something other than "keep":
              // an unrelated disclosure save can never touch the stored expiry (P0-06).
              var body={show_scores:$('sw_scores').checked,show_profiles:$('sw_profiles').checked,
                show_dates:$('sw_dates').checked};
              if($('sw_exp').value!=='keep')body.expires_in_days=parseInt($('sw_exp').value,10)||0;
              api('/api/world/passport/disclosure',body)
                .then(load).then(function(){$('showmsg').textContent='Saved.';})
                .catch(function(){$('showmsg').textContent='Could not save — try again.';});
            });
            $('dlexport').addEventListener('click',function(){
              fetch('/api/world/account/export',{headers:{'X-World-Account':localStorage.getItem(KEY)||''}})
                .then(function(r){if(!r.ok)throw r;return r.blob();})
                .then(function(blob){
                  var url=URL.createObjectURL(blob),a=document.createElement('a');
                  a.href=url;a.download='pciworld-my-data.json';document.body.appendChild(a);a.click();
                  a.remove();URL.revokeObjectURL(url);
                  $('acctmsg').textContent='Your data has been downloaded.';
                })
                .catch(function(){$('acctmsg').textContent='Could not prepare the export — try again.';});
            });
            $('delacct').addEventListener('click',function(){
              $('delbox').hidden=false;$('delpw').focus();
            });
            $('delno').addEventListener('click',function(){
              $('delbox').hidden=true;$('delpw').value='';$('delacct').focus();
            });
            $('delgo').addEventListener('click',function(){
              var pw=$('delpw').value;
              if(!pw){$('acctmsg').textContent='Enter your password to confirm.';$('delpw').focus();return;}
              api('/api/world/account/delete',{password:pw})
                .then(function(){localStorage.removeItem(KEY);showAuth();})
                .catch(function(){$('acctmsg').textContent='Password incorrect — account not deleted.';$('delpw').focus();});
            });
          }).catch(function(e2){
            // A failure to LOAD is not a sign-out (journey repair P1-04): only a rejected token
            // shows the sign-in panel. A network drop or server error says so honestly, keeps the
            // stored session, and offers a retry — presenting it as "you are not registered" made
            // people re-register and lose track of their account.
            if(e2&&(e2.error==='no_token'||e2.error==='world_disabled')){localStorage.removeItem(KEY);return showAuth();}
            $('auth').hidden=true;$('me').hidden=false;
            $('me').innerHTML='<div class="card"><span class="kicker">Connection problem</span>'+
              '<h2 style="margin-top:0">We could not load your account</h2>'+
              '<p style="color:var(--slate)">'+(navigator.onLine===false
                ?'You appear to be offline. Your account and evidence are safe on the server — reconnect and try again.'
                :'The service did not answer just now. You are still signed in; nothing has been lost.')+'</p>'+
              '<p><button class="btn" id="retryLoad">Try again</button></p></div>';
            $('me').focus();
            $('retryLoad').addEventListener('click',function(){load();});
          });
        }
        $('doRegister').addEventListener('click',function(){
          $('autherr').textContent='';
          api('/api/world/account/register',{email:$('r_email').value,password:$('r_pw').value,display_name:$('r_name').value})
            .then(function(r){localStorage.setItem(KEY,r.token);load();})
            .catch(function(e2){$('autherr').textContent=(e2&&e2.message)||(e2&&e2.error)||'Could not create the account.';});
        });
        $('doLogin').addEventListener('click',function(){
          $('autherr').textContent='';
          api('/api/world/account/login',{email:$('l_email').value,password:$('l_pw').value})
            .then(function(r){localStorage.setItem(KEY,r.token);load();})
            .catch(function(e2){$('autherr').textContent=(e2&&e2.error)==='account_locked'?'Too many attempts — try later.':'Sign-in failed.';});
        });
        $('doForgot').addEventListener('click',function(){
          if(!$('l_email').value){$('autherr').textContent='Enter your email first, then press Forgot password.';return;}
          api('/api/world/account/forgot',{email:$('l_email').value})
            .then(function(r){$('autherr').textContent=r.message||'If that address has an account, a reset link is on its way.';})
            .catch(function(){$('autherr').textContent='Could not send the reset email — try again shortly.';});
        });
        // Portal handoff (P0-02): the SSO bridge sends a one-time code in the URL FRAGMENT (never
        // in a query string the server would log). Exchange it once for a session, then scrub it
        // from the address bar and history immediately.
        var hm=(location.hash||'').match(/h=([a-f0-9]{48,64})/);
        if(hm){
          history.replaceState(null,'',location.pathname);
          api('/api/world/account/handoff',{code:hm[1]})
            .then(function(r){localStorage.setItem(KEY,r.token);
              // The allow-listed destination the handoff preserved — deep entry (today's
              // challenge, a result) continues there; the account page is only the default.
              if(r.return_to&&r.return_to!=='/world/account'){location.href=r.return_to;return;}
              load();})
            .catch(function(e2){showAuth();
              $('autherr').textContent=(e2&&e2.message)||'That sign-in link has expired — sign in below, or reopen your Passport from the student portal.';});
        }
        else if(localStorage.getItem(KEY))load();else showAuth();
        })();
        """;

    public static string InvitePage(Db db, string? inviterName, Dictionary<string, object?> version, string code, string token)
    {
        var who = string.IsNullOrWhiteSpace(inviterName) ? "Someone" : inviterName!;
        var title = H.Str(version["title"]) ?? "a PCI World challenge";
        return Layout(db,
            $"You've been challenged — {title} — PCI World",
            $"{who} completed “{title}” on PCI World. Can you improve the project outcome?",
            $"""
            <span class="kicker">A challenge for you</span>
            <h1>{E(who)} completed &ldquo;{E(title)}&rdquo;.<br>Can you improve the project outcome?</h1>
            <p class="lede">Same project, same evidence, same clock. Free, anonymous, five to ten minutes.</p>
            <p><a class="btn" href="/world/challenge/{E(code)}?i={E(token)}">Accept the challenge</a></p>
            <p class="notice">You will play the exact same version of this challenge. Their answers are not shown to you — or yours to them; only completed scores are ever compared. {E(PracticeNotice)}</p>
            """,
            "/world", noindex: true);
    }
}
