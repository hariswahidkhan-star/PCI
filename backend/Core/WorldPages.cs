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
    public static string Json(object o) =>
        System.Text.Json.JsonSerializer.Serialize(o).Replace("</", "<\\/");

    public static string InstituteUrl(Db db) =>
        Settings.Str(db, "world_institute_url", "https://projectcontrolsinstitute.org");

    /// <summary>Brand fonts load AFTER window load so no page ever waits on a font host — system
    /// fallbacks render immediately, Archivo/Inter swap in when available (display=swap). This
    /// also keeps the E2E suite deterministic in offline environments.</summary>
    const string FontLoader = """
        <script>window.addEventListener('load',function(){var l=document.createElement('link');l.rel='stylesheet';
        l.href='https://fonts.googleapis.com/css2?family=Archivo:wght@700;800;900&family=Inter:wght@400;500;600;700&display=swap';
        document.head.appendChild(l);});</script>
        """;

    // Design system: the PCI brand (backend/wwwroot/assets/styles.css) applied to PCI World —
    // Archivo 800/900 display type with tight tracking, Inter text, ink/noir/blue/crimson tokens,
    // crimson eyebrows + underline strokes, squared CTAs, layered blue-tinted shadows. Light-only,
    // exactly like the Institute site (meta color-scheme in Layout) — a brand commitment, not an
    // omission. Class names are stable API for the workspace/admin scripts and the E2E suite.
    const string Css = """
        :root{--ink:#0F172A;--paper:#FFFFFF;--paper-2:#F1F5F9;--noir:#0E1525;--line:#E3E8EF;
              --slate:#475569;--mist:#64748B;--blue:#1D4ED8;--blue-deep:#1E3A8A;--crimson:#C13329;
              --ok:#15803D;--bad:#C2410C;--muted:var(--slate);
              --display:'Archivo',system-ui,sans-serif;--sans:'Inter',system-ui,sans-serif;
              --shadow-rest:0 1px 2px rgba(13,32,90,.05),0 10px 28px -20px rgba(29,78,216,.14);
              --shadow-hover:0 2px 5px rgba(13,32,90,.06),0 26px 56px -24px rgba(29,78,216,.25),0 0 0 1px rgba(29,78,216,.10);
              --ease:cubic-bezier(.22,.61,.36,1)}
        *{box-sizing:border-box;margin:0}
        html{-webkit-text-size-adjust:100%}
        body{background:var(--paper);color:var(--ink);font:16.5px/1.62 var(--sans);-webkit-font-smoothing:antialiased}
        a{color:var(--blue);text-decoration-thickness:1px;text-underline-offset:3px}
        a:focus-visible,button:focus-visible,input:focus-visible,select:focus-visible,textarea:focus-visible,
        summary:focus-visible{outline:3px solid var(--blue);outline-offset:2px}
        .shell{max-width:1020px;margin:0 auto;padding:0 22px}
        header.world{background:var(--noir);color:#E2E8F0}
        header.world .shell{display:flex;flex-wrap:wrap;gap:12px 26px;align-items:center;padding:18px 22px}
        .brand{display:flex;align-items:center;gap:13px;color:#fff;text-decoration:none}
        .brand .wordmark{font-family:var(--display);font-weight:900;font-size:23px;letter-spacing:-.035em;line-height:.9;white-space:nowrap}
        .brand .bar{width:2px;height:32px;background:var(--crimson);border-radius:2px;flex:0 0 auto}
        .brand small{display:block;font-family:var(--sans);font-weight:600;font-size:11.5px;letter-spacing:.02em;
             color:#94A3B8;line-height:1.25;max-width:150px;white-space:normal}
        header.world nav{display:flex;flex-wrap:wrap;gap:8px 22px;margin-left:auto;font-size:15px;align-items:center}
        header.world nav a{color:#CBD5E1;text-decoration:none;font-weight:500;padding:4px 0;position:relative}
        header.world nav a:hover{color:#fff}
        header.world nav a:not(.ext):hover::after{content:"";position:absolute;left:0;right:0;bottom:-2px;height:2px;background:var(--crimson)}
        header.world nav a.ext{color:#93C5FD;font-weight:600}
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
        .btn.secondary{background:transparent;color:var(--ink);border:1.5px solid var(--line);padding:15.5px 28px}
        .btn.secondary:hover{border-color:var(--ink);background:var(--paper)}
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
        input[type=text],input[type=number],input[type=email],input[type=password]{width:100%;max-width:360px;
             padding:14px 16px;font-size:16px;border:1.5px solid var(--line);border-radius:0;background:var(--paper);
             color:var(--ink);font-variant-numeric:tabular-nums;font-family:var(--sans)}
        input:hover{border-color:var(--mist)}
        select,textarea{font-family:var(--sans);font-size:15.5px;color:var(--ink);background:var(--paper);
             border:1.5px solid var(--line);border-radius:0;padding:12px 14px}
        fieldset{border:1.5px solid var(--line);border-radius:12px;padding:20px 22px 14px;margin:20px 0}
        legend{font-family:var(--display);font-weight:800;font-size:16.5px;letter-spacing:-.01em;padding:0 8px}
        .opt{display:flex;gap:12px;align-items:flex-start;padding:10px 8px;border-radius:8px}
        .opt:hover{background:var(--paper-2)}
        .opt input{margin-top:5px;accent-color:var(--blue);width:16px;height:16px;flex:0 0 auto}
        .opt label{font-weight:500;margin:0;line-height:1.5}
        .dim{display:flex;gap:40px;flex-wrap:wrap;margin:14px 0}
        .dim div{min-width:118px}
        .dim .kicker{margin-bottom:4px;color:var(--mist)}
        .card--noir .dim .kicker{color:#94A3B8}
        .dim b{font-family:var(--display);font-weight:800;font-size:38px;letter-spacing:-.02em;display:block;line-height:1.05}
        details.card summary{cursor:pointer;font-weight:600}
        .steps{padding-left:0;list-style:none;counter-reset:step;display:grid;gap:22px}
        .steps li{counter-increment:step;display:grid;grid-template-columns:56px 1fr;gap:18px;align-items:start}
        .steps li::before{content:counter(step,decimal-leading-zero);font-family:var(--display);font-weight:800;
             font-size:30px;color:var(--crimson);line-height:1.1}
        .steps b{font-family:var(--display);font-weight:800;font-size:17px;letter-spacing:-.01em;display:block;margin-bottom:3px}
        .steps li div{color:var(--slate)}
        .hero-panel{background:var(--noir);border-radius:16px;padding:26px 26px 18px;margin:34px 0 6px;overflow:hidden}
        .hero-panel .plabel{display:flex;justify-content:space-between;flex-wrap:wrap;gap:8px;
             font-family:var(--sans);font-weight:700;font-size:11.5px;letter-spacing:.14em;text-transform:uppercase;color:#94A3B8}
        .hero-panel svg{width:100%;height:auto;display:block;margin-top:10px}
        .legend-row{display:flex;gap:22px;flex-wrap:wrap;margin-top:12px;font-size:12.5px;font-weight:600;color:#CBD5E1}
        .legend-row span::before{content:"";display:inline-block;width:18px;height:3px;border-radius:2px;
             margin-right:8px;vertical-align:middle;background:var(--swatch,#fff)}
        footer.world{background:var(--noir);color:#94A3B8;font-size:14.5px;margin-top:30px}
        footer.world .shell{padding:44px 22px 48px;display:grid;gap:12px}
        footer.world a{color:#CBD5E1}
        footer.world a:hover{color:#fff}
        footer.world .ft-brand{display:flex;align-items:center;gap:12px;margin-bottom:6px}
        footer.world .ft-brand .wordmark{font-family:var(--display);font-weight:900;font-size:19px;letter-spacing:-.03em;color:#fff}
        footer.world .ft-brand .bar{width:2px;height:26px;background:var(--crimson);border-radius:2px}
        footer.world .fine{font-size:13px;line-height:1.65;color:#7C8CA0;max-width:88ch}
        .visually-hidden{position:absolute;width:1px;height:1px;overflow:hidden;clip:rect(0 0 0 0)}
        @media (max-width:680px){
          main{padding:38px 0 60px}
          .card{padding:20px;border-radius:12px}
          .btn{width:100%;justify-content:center}
          .btn+.btn{margin-left:0;margin-top:10px}
          .dim{gap:24px}
          .dim b{font-size:31px}
          header.world nav{gap:6px 16px;font-size:14px}
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
            {(noindex ? "<meta name=\"robots\" content=\"noindex\">" : "")}
            <link rel="canonical" href="{E(WorldUrl.Base() + canonicalPath)}">
            <link rel="icon" href="data:image/svg+xml,%3Csvg xmlns=%27http://www.w3.org/2000/svg%27 viewBox=%270 0 64 64%27%3E%3Crect width=%2764%27 height=%2764%27 rx=%2714%27 fill=%27%230E1525%27/%3E%3Crect x=%2712%27 y=%2744%27 width=%2740%27 height=%274%27 rx=%272%27 fill=%27%23C13329%27/%3E%3Ctext x=%2732%27 y=%2738%27 font-family=%27Archivo,Arial%27 font-weight=%27900%27 font-size=%2722%27 fill=%27white%27 text-anchor=%27middle%27 letter-spacing=%27-1%27%3EPW%3C/text%3E%3C/svg%3E">
            <meta property="og:site_name" content="PCI World">
            <meta property="og:title" content="{E(ogTitle ?? title)}">
            <meta property="og:description" content="{E(ogDesc ?? metaDesc)}">
            <meta property="og:type" content="website">
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            {FontLoader}
            <style>{Css}</style>
            </head>
            <body>
            <a class="visually-hidden" href="#main">Skip to content</a>
            <header class="world">
              <div class="shell">
                <a class="brand" href="/world">
                  <span class="wordmark">PCI World</span>
                  <span class="bar" aria-hidden="true"></span>
                  <small>From the Project<br>Controls Institute</small>
                </a>
                <nav aria-label="Primary">
                  <a href="/world">Today&rsquo;s Challenge</a>
                  <a href="/world/archive">Challenge Library</a>
                  <a href="/world/account">Passport</a>
                  <a href="/world/about">About</a>
                  <a class="ext" href="{inst}" target="_blank" rel="noopener noreferrer">{InstituteLinkLabel} <span aria-hidden="true">&#8599;</span><span class="visually-hidden">(opens the official Institute website in a new tab)</span></a>
                </nav>
              </div>
            </header>
            <main id="main" class="shell">
            {body}
            </main>
            <footer class="world">
              <div class="shell">
                <div class="ft-brand"><span class="wordmark">PCI World</span><span class="bar" aria-hidden="true"></span></div>
                <div>{E(OperatedBy)}</div>
                <div><a href="{inst}" target="_blank" rel="noopener noreferrer">{InstituteLinkLabel} <span aria-hidden="true">&#8599;</span></a>
                     &nbsp;&middot;&nbsp; <a href="/world/about">About PCI World</a>
                     &nbsp;&middot;&nbsp; <a href="/world/archive">Challenge Library</a></div>
                <div class="fine">{E(PracticeNotice)}</div>
              </div>
            </footer>
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
            <span class="kicker">PCI World Challenge</span>
            <h1>The project is already moving. The decision is now yours.</h1>
            <div class="uline" aria-hidden="true"></div>
            <p class="lede">Step into a realistic project situation, examine the evidence and decide what happens next. Five to ten minutes. Free. No project experience required.</p>
            <p style="margin-top:28px">
              <a class="btn" href="{primaryHref}">Take today&rsquo;s challenge
                <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" aria-hidden="true"><path d="M5 12h14M13 6l6 6-6 6"/></svg></a>
              <a class="btn secondary" href="/world/about">See how PCI World works</a>
            </p>
            <div class="hero-panel" role="img" aria-label="A project performance chart: planned value as a dashed baseline, earned value tracking below plan, and actual cost running above earned value at the data date — the situation a PCI World challenge drops you into.">
              <div class="plabel"><span>Live project position &middot; synthetic data</span><span>Data date &middot; month 4 of 12</span></div>
              <svg viewBox="0 0 720 232" aria-hidden="true">
                <g stroke="#1c2739" stroke-width="1">
                  <line x1="16" y1="192" x2="704" y2="192"/><line x1="16" y1="136" x2="704" y2="136"/>
                  <line x1="16" y1="80" x2="704" y2="80"/><line x1="16" y1="24" x2="704" y2="24"/>
                </g>
                <line x1="432" y1="14" x2="432" y2="206" stroke="#3b4a63" stroke-width="1.5" stroke-dasharray="3 5"/>
                <path d="M16,206 C170,201 280,158 400,104 S600,26 704,14" fill="none" stroke="#94A3B8" stroke-width="2" stroke-dasharray="7 6"/>
                <path d="M16,206 C160,203 260,178 350,150 S415,128 432,122" fill="none" stroke="#C13329" stroke-width="2.5"/>
                <path d="M16,206 C150,202 250,172 345,138 S415,108 432,100" fill="none" stroke="#5B8DEF" stroke-width="2.5"/>
                <circle cx="432" cy="122" r="4.5" fill="#C13329"/>
                <circle cx="432" cy="100" r="4.5" fill="#5B8DEF"/>
              </svg>
              <div class="legend-row">
                <span style="--swatch:#94A3B8">Planned value — the promise</span>
                <span style="--swatch:#C13329">Earned value — the truth</span>
                <span style="--swatch:#5B8DEF">Actual cost — the bill</span>
              </div>
            </div>
            {todayCard}
            <h2 class="sec">How it works</h2>
            <div class="uline" aria-hidden="true"></div>
            <div class="card">
              <ol class="steps">
                <li><div><b>Read the situation</b>A real-shaped project moment with the evidence in front of you — synthetic data, real methods.</div></li>
                <li><div><b>Do the work</b>Compute the measures that matter and make the judgement calls a professional would face.</div></li>
                <li><div><b>See the consequences</b>Deterministic scoring, your professional decision profile, and what each choice would have caused.</div></li>
              </ol>
            </div>
            <h2 class="sec">Where it leads</h2>
            <div class="uline" aria-hidden="true"></div>
            <div class="card">
              <p style="color:var(--slate);max-width:78ch">PCI World is practice with evidence. When you want the full discipline — multi-step simulations, coaching and competency tracking — continue in the <a href="{simlab}">PCI Simulation Lab</a>. When you are ready for formal recognition, explore the certifications on the official <a href="{E(InstituteUrl(db))}" target="_blank" rel="noopener noreferrer">Project Controls Institute website <span aria-hidden="true">&#8599;</span></a>.</p>
            </div>
            <p class="notice">{E(PracticeNotice)}</p>
            """,
            "/world");
    }

    public static string Cap(string? s) => string.IsNullOrEmpty(s) ? "" : char.ToUpperInvariant(s[0]) + s[1..];

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
              <form id="work" hidden aria-describedby="savestate">
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
              <section id="result" hidden aria-live="polite"></section>
            </div>
            <details class="card">
              <summary style="cursor:pointer;font-weight:600">Report an issue with this challenge</summary>
              <p style="margin-top:10px;color:var(--muted)">Spotted a content error, a calculation problem or an accessibility barrier? The PCI World content team reviews every report. No personal details are required.</p>
              <label for="rep_cat">What kind of issue?</label>
              <select id="rep_cat" style="padding:9px 10px;border:1px solid var(--line);border-radius:8px">
                <option value="content_error">Content error</option>
                <option value="calculation">Calculation problem</option>
                <option value="accessibility">Accessibility barrier</option>
                <option value="inappropriate">Inappropriate content</option>
                <option value="other">Something else</option>
              </select>
              <label for="rep_msg">Describe it</label>
              <textarea id="rep_msg" rows="4" maxlength="2000" style="width:100%;padding:10px 12px;border:1px solid var(--line);border-radius:8px;font:inherit"></textarea>
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
          return fetch(path, { method:'POST', headers:{ 'Content-Type':'application/json',
            'X-World-Session': localStorage.getItem('world_session') || '' },
            body: JSON.stringify(body || {}) }).then(function(r){ return r.json().then(function(j){ if(!r.ok) throw j; return j; }); });
        }
        function ensureSession(){
          if (localStorage.getItem('world_session')) return Promise.resolve();
          return api('/api/world/session').then(function(r){ localStorage.setItem('world_session', r.token); });
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
              .catch(function(){ $('savestate').textContent = 'Could not save — will retry on your next change.'; });
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
          var h = '<div class="card"><span class="kicker">Your result</span>' +
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
               '<button class="btn secondary" type="button" id="mkinvite">Challenge a friend</button></p>' +
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
          el.scrollIntoView({ behavior: 'smooth' });
        }
        $('start').addEventListener('click', function(){
          $('starterr').textContent = '';
          ensureSession().then(function(){
            return api('/api/world/attempts', { code: WORLD.code, invite: WORLD.invite });
          }).then(function(r){
            att = r;
            renderWork(r.answers || null);
            $('work').hidden = false;
            $('start').hidden = true;
            if (r.completed && r.result) { renderResult(r.result); return; }
            $('work').addEventListener('input', autosave);
            $('work').addEventListener('change', autosave);
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
              $('savestate').textContent = (e && e.message) || 'Submission failed — your work is saved, try again.';
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
                <select id="f_ind" name="industry" style="padding:9px 10px;border:1px solid var(--line);border-radius:8px">{industryOpts}</select></div>
              <div><label for="f_dif" style="margin-top:0">Difficulty</label>
                <select id="f_dif" name="difficulty" style="padding:9px 10px;border:1px solid var(--line);border-radius:8px">{difficultyOpts}</select></div>
              <div><label for="f_trk" style="margin-top:0">Track</label>
                <select id="f_trk" name="track" style="padding:9px 10px;border:1px solid var(--line);border-radius:8px">{trackOpts}</select></div>
              <div><button class="btn secondary" type="submit">Filter</button></div>
            </form>
            <div class="card">
            <p class="kicker" style="margin-bottom:10px">{total} challenge{(total == 1 ? "" : "s")}{(pages > 1 ? $" &middot; page {page} of {pages}" : "")}</p>
            <table>
              <caption class="visually-hidden">Published PCI World challenges, filtered by the controls above</caption>
              <thead><tr><th scope="col">Challenge</th><th scope="col">Industry</th><th scope="col">Difficulty</th><th scope="col">Time</th></tr></thead>
              <tbody>{items}</tbody>
            </table>
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

    public static string VerifyEmail(Db db, bool ok) => Layout(db,
        ok ? "Email verified — PCI World" : "Verification link invalid — PCI World",
        "PCI World email verification.",
        ok
            ? """
              <h1>Email verified</h1>
              <p class="lede">Your PCI World account email is confirmed. You can now publish your Passport when you choose to.</p>
              <p><a class="btn" href="/world/account">Go to your account</a></p>
              """
            : """
              <h1>That link didn&rsquo;t work</h1>
              <p class="lede">The verification link is invalid or has expired. Sign in and request a new one from your account page.</p>
              <p><a class="btn" href="/world/account">Go to your account</a></p>
              """,
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
    public static string PublicPassport(Db db, string name, List<Dictionary<string, object?>> rows)
    {
        var items = string.Join("", rows.Select(r => $"""
            <tr>
              <td>{E(H.Str(r["title"]))}</td>
              <td>{E(H.Str(r["industry"]))}</td>
              <td>{E(Cap(H.Str(r["difficulty"])))}</td>
              <td class="num">{H.D(r["score"]):0.#}</td>
              <td>{E(H.Str(r["profile_key"]))}</td>
              <td class="num">{E((H.Str(r["completed_at"]) ?? "").Split(' ')[0])}</td>
            </tr>
            """));
        var industries = rows.Select(r => H.Str(r["industry"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count();
        var tracks = rows.Select(r => H.Str(r["track"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count();
        return Layout(db,
            $"{name} — PCI World Passport",
            $"Verified virtual project experience: {rows.Count} completed PCI World challenge{(rows.Count == 1 ? "" : "s")} across {industries} industr{(industries == 1 ? "y" : "ies")}.",
            $"""
            <span class="kicker">PCI World Passport &middot; verified virtual project experience</span>
            <h1>{E(name)}</h1>
            <div class="card">
              <div class="dim">
                <div><span class="kicker">Challenges</span><b class="score num">{rows.Count}</b></div>
                <div><span class="kicker">Industries</span><b class="score num">{industries}</b></div>
                <div><span class="kicker">Tracks</span><b class="score num">{tracks}</b></div>
              </div>
            </div>
            <div class="card">
              <h2 style="margin-top:0">Selected evidence</h2>
              <table>
                <thead><tr><th scope="col">Challenge</th><th scope="col">Industry</th><th scope="col">Difficulty</th>
                <th scope="col">Score</th><th scope="col">Decision profile</th><th scope="col">Date</th></tr></thead>
                <tbody>{items}</tbody>
              </table>
            </div>
            <p><a class="btn" href="/world">Take today&rsquo;s challenge yourself</a></p>
            <p class="notice">This Passport shows verified practice evidence its owner chose to publish. Answers are never shown. {E(PracticeNotice)}</p>
            """,
            "/world",
            ogTitle: $"{name} — PCI World Passport",
            ogDesc: $"Verified virtual project experience: {rows.Count} completed PCI World challenges.",
            // Same reasoning as a shared result: the Passport token is revocable and rotatable, and
            // a search index would outlive both. Sharing the link still works everywhere.
            noindex: true);
    }

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
        <div id="auth" class="card" hidden>
          <div style="display:grid;gap:26px;grid-template-columns:repeat(auto-fit,minmax(260px,1fr))">
            <div>
              <h2 style="margin-top:0">Create your Passport</h2>
              <label for="r_name">Display name (shown on your public Passport)</label><input id="r_name" type="text" maxlength="80" autocomplete="name">
              <label for="r_email">Email</label><input id="r_email" type="email" autocomplete="email">
              <label for="r_pw">Password (min 10 characters)</label><input id="r_pw" type="password" autocomplete="new-password">
              <p style="margin-top:12px"><button class="btn" id="doRegister">Create account</button></p>
            </div>
            <div>
              <h2 style="margin-top:0">Sign in</h2>
              <label for="l_email">Email</label><input id="l_email" type="email" autocomplete="email">
              <label for="l_pw">Password</label><input id="l_pw" type="password" autocomplete="current-password">
              <p style="margin-top:12px"><button class="btn secondary" id="doLogin">Sign in</button>
                 <button class="btn secondary" id="doForgot" type="button">Forgot password</button></p>
            </div>
          </div>
          <p id="autherr" class="bad" role="alert"></p>
          <p class="notice">Challenges you completed anonymously in this browser are added to your account automatically. {E(PracticeNotice)}</p>
        </div>
        <div id="me" hidden></div>
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
        function showAuth(){$('auth').hidden=false;$('me').hidden=true;}
        function load(){
          api('/api/world/passport').then(function(p){
            $('auth').hidden=true;$('me').hidden=false;
            var h='<div class="card"><h2 style="margin-top:0">Your Passport</h2>'+
              '<div class="dim"><div><span class="kicker">Completed</span><b class="score num">'+p.completed+'</b></div>'+
              '<div><span class="kicker">Industries</span><b class="score num">'+p.industries+'</b></div>'+
              '<div><span class="kicker">Tracks</span><b class="score num">'+p.tracks+'</b></div></div>'+
              '<label for="dn">Display name</label><input id="dn" maxlength="80" value="'+esc(p.display_name||'')+'">'+
              '<p style="margin-top:10px"><button class="btn secondary" id="saveName">Save name</button> '+
              (p.email_verified?'<span class="ok">Email verified.</span>'
                :'<span class="bad">Email not verified.</span> <button class="btn secondary" id="resend">Resend verification</button>')+'</p>'+
              '<p style="margin-top:10px">'+
              (p.passport_public
                ?'<button class="btn secondary" id="unpub">Make Passport private</button>'
                :'<button class="btn" id="pub">Publish my public Passport</button>')+
              ' <span id="puburl"></span> <span id="pubmsg" class="bad" role="alert"></span></p></div>';
            h+='<div class="card"><h2 style="margin-top:0">Evidence</h2>'+
               '<p>Tick the results you want on your public Passport. Nothing is shown without your choice.</p>'+
               '<table><thead><tr><th>Show</th><th>Challenge</th><th>Score</th><th>Profile</th><th>Date</th></tr></thead><tbody>';
            (p.evidence||[]).forEach(function(e2){
              h+='<tr><td><input type="checkbox" data-att="'+e2.attempt_id+'" '+(e2.passport_visible?'checked':'')+
                 ' aria-label="Show '+esc(e2.title)+' on public Passport"></td>'+
                 '<td>'+esc(e2.title)+'</td><td class="num">'+esc(e2.score)+'</td><td>'+esc(e2.profile)+'</td>'+
                 '<td class="num">'+esc((e2.completed_at||'').split(' ')[0])+'</td></tr>';
            });
            h+='</tbody></table></div>'+
               '<div class="card"><h2 style="margin-top:0">Your data</h2>'+
               '<p><a class="btn secondary" href="/api/world/account/export">Export my data (JSON)</a> '+
               '<button class="btn secondary" id="signout">Sign out</button> '+
               '<button class="btn secondary" id="delacct">Delete my account</button></p>'+
               '<p id="acctmsg" role="status"></p></div>';
            $('me').innerHTML=h;
            $('saveName').addEventListener('click',function(){
              api('/api/world/account/profile',{display_name:$('dn').value}).then(load);
            });
            if($('resend'))$('resend').addEventListener('click',function(){
              api('/api/world/account/resend-verification',{}).then(function(){$('acctmsg').textContent='Verification email sent.';});
            });
            if($('pub'))$('pub').addEventListener('click',function(){
              api('/api/world/passport/publish',{publish:true})
                .then(function(r){$('puburl').innerHTML='<a href="'+r.url+'">'+esc(location.origin+r.url)+'</a>';load();})
                .catch(function(e2){$('pubmsg').textContent=(e2&&e2.message)||(e2&&e2.error)||'Could not publish.';});
            });
            if($('unpub'))$('unpub').addEventListener('click',function(){
              api('/api/world/passport/publish',{publish:false}).then(load);
            });
            $('me').querySelectorAll('input[data-att]').forEach(function(cb){
              cb.addEventListener('change',function(){
                api('/api/world/passport/evidence',{attempt_id:parseInt(cb.dataset.att,10),visible:cb.checked});
              });
            });
            $('signout').addEventListener('click',function(){
              api('/api/world/account/logout',{}).catch(function(){});
              localStorage.removeItem(KEY);showAuth();
            });
            $('delacct').addEventListener('click',function(){
              var pw=prompt('Deleting your account removes your Passport and all public links. Enter your password to confirm:');
              if(!pw)return;
              api('/api/world/account/delete',{password:pw})
                .then(function(){localStorage.removeItem(KEY);showAuth();})
                .catch(function(){$('acctmsg').textContent='Password incorrect — account not deleted.';});
            });
          }).catch(showAuth);
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
        if(localStorage.getItem(KEY))load();else showAuth();
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
