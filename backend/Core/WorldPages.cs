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

    const string Css = """
        :root{--bg:#f6f5f2;--surface:#ffffff;--ink:#191c1f;--muted:#5b6167;--line:#e3e1da;
              --accent:#0d5c8d;--accent-ink:#ffffff;--ok:#186f47;--bad:#9f2d24;--warn:#8a5a00;
              --baseline:#7a8087;--focus:#0d5c8d}
        *{box-sizing:border-box;margin:0}
        html{-webkit-text-size-adjust:100%}
        body{background:var(--bg);color:var(--ink);font:16px/1.55 system-ui,-apple-system,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif}
        a{color:var(--accent)} a:focus-visible,button:focus-visible,input:focus-visible,[tabindex]:focus-visible{outline:3px solid var(--focus);outline-offset:2px}
        .shell{max-width:880px;margin:0 auto;padding:0 20px}
        header.world{background:#12212e;color:#e9edf1}
        header.world .shell{display:flex;flex-wrap:wrap;gap:10px 22px;align-items:center;padding:14px 20px}
        .brand{font-weight:700;letter-spacing:.4px;color:#fff;text-decoration:none;font-size:18px}
        .brand small{display:block;font-weight:400;font-size:11px;letter-spacing:.6px;color:#9fb1c1;text-transform:uppercase}
        header.world nav{display:flex;flex-wrap:wrap;gap:16px;margin-left:auto;font-size:14px}
        header.world nav a{color:#c9d4dd;text-decoration:none} header.world nav a:hover{color:#fff;text-decoration:underline}
        header.world nav a.ext{color:#9fd0ef}
        main{padding:34px 0 56px}
        h1{font-size:30px;line-height:1.2;letter-spacing:-.3px;margin:0 0 10px}
        h2{font-size:20px;margin:34px 0 10px}
        p.lede{font-size:18px;color:var(--muted);max-width:56ch}
        .num,td.num,.score{font-variant-numeric:tabular-nums}
        .card{background:var(--surface);border:1px solid var(--line);border-radius:10px;padding:22px;margin:18px 0}
        .kicker{font-size:12px;letter-spacing:1.1px;text-transform:uppercase;color:var(--muted)}
        .meta{display:flex;flex-wrap:wrap;gap:8px 18px;font-size:14px;color:var(--muted);margin:8px 0 0}
        .btn{display:inline-block;background:var(--accent);color:var(--accent-ink);border:0;border-radius:8px;
             padding:11px 20px;font-size:16px;font-weight:600;text-decoration:none;cursor:pointer}
        .btn.secondary{background:transparent;color:var(--accent);border:1px solid var(--accent)}
        .notice{border-inline-start:3px solid var(--baseline);background:var(--surface);padding:12px 16px;color:var(--muted);font-size:14px;margin:18px 0}
        table{border-collapse:collapse;width:100%;font-size:15px}
        th{text-align:left;font-size:12px;letter-spacing:.6px;text-transform:uppercase;color:var(--muted)}
        th,td{padding:8px 10px;border-bottom:1px solid var(--line);vertical-align:top}
        td.num{text-align:right;white-space:nowrap}
        .ok{color:var(--ok);font-weight:600}.bad{color:var(--bad);font-weight:600}
        label{display:block;font-weight:600;margin:16px 0 6px}
        input[type=text],input[type=number]{width:100%;max-width:340px;padding:10px 12px;font-size:16px;
             border:1px solid var(--line);border-radius:8px;background:var(--surface);font-variant-numeric:tabular-nums}
        fieldset{border:1px solid var(--line);border-radius:10px;padding:14px 16px;margin:16px 0}
        legend{font-weight:600;padding:0 6px}
        .opt{display:flex;gap:10px;align-items:flex-start;padding:8px 4px}
        .opt input{margin-top:4px}
        .dim{display:flex;gap:26px;flex-wrap:wrap;margin:12px 0}
        .dim div{min-width:120px}
        .dim b{font-size:26px;display:block}
        footer.world{border-top:1px solid var(--line);background:var(--surface);color:var(--muted);font-size:14px}
        footer.world .shell{padding:26px 20px;display:grid;gap:8px}
        .visually-hidden{position:absolute;width:1px;height:1px;overflow:hidden;clip:rect(0 0 0 0)}
        @media (max-width:640px){h1{font-size:24px} .card{padding:16px} header.world nav{gap:12px}}
        @media (prefers-reduced-motion:no-preference){.btn{transition:filter .15s} .btn:hover{filter:brightness(1.08)}}
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
            <title>{E(title)}</title>
            <meta name="description" content="{E(metaDesc)}">
            {(noindex ? "<meta name=\"robots\" content=\"noindex\">" : "")}
            <link rel="canonical" href="{E(canonicalPath)}">
            <meta property="og:site_name" content="PCI World">
            <meta property="og:title" content="{E(ogTitle ?? title)}">
            <meta property="og:description" content="{E(ogDesc ?? metaDesc)}">
            <meta property="og:type" content="website">
            <style>{Css}</style>
            </head>
            <body>
            <a class="visually-hidden" href="#main">Skip to content</a>
            <header class="world">
              <div class="shell">
                <a class="brand" href="/world">PCI World<small>From the Project Controls Institute</small></a>
                <nav aria-label="Primary">
                  <a href="/world">Today&rsquo;s Challenge</a>
                  <a href="/world/archive">Challenge Library</a>
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
                <div>{E(OperatedBy)}</div>
                <div><a href="{inst}" target="_blank" rel="noopener noreferrer">{InstituteLinkLabel} <span aria-hidden="true">&#8599;</span></a>
                     &nbsp;&middot;&nbsp; <a href="/world/about">About PCI World</a></div>
                <div>{E(PracticeNotice)}</div>
              </div>
            </footer>
            </body>
            </html>
            """;
    }

    public static string Home(Db db, Dictionary<string, object?>? today, Dictionary<string, object?>? version)
    {
        var simlab = E(Settings.Str(db, "world_simlab_url", "/app/lab"));
        var todayCard = today is null || version is null
            ? """
              <div class="card"><span class="kicker">Today's challenge</span>
              <h2 style="margin-top:6px">The first challenge is being prepared</h2>
              <p>PCI World rotates a new project challenge every day at 00:00 UTC. Check back shortly.</p></div>
              """
            : $"""
              <div class="card">
                <span class="kicker">Today&rsquo;s challenge &middot; changes daily at 00:00 UTC</span>
                <h2 style="margin-top:6px">{E(H.Str(version["title"]))}</h2>
                <p>{E(H.Str(version["hook"]))}</p>
                <div class="meta">
                  <span>{E(H.Str(version["industry"]))}</span>
                  <span>{E(Cap(H.Str(version["difficulty"])))}</span>
                  <span class="num">~{H.L(version["est_minutes"])} minutes</span>
                  <span>Free &middot; no account needed</span>
                </div>
                <p style="margin-top:18px">
                  <a class="btn" href="/world/challenge/{E(H.Str(today["code"]))}">Take today&rsquo;s challenge</a>
                  <a class="btn secondary" style="margin-left:10px" href="/world/about">See how PCI World works</a>
                </p>
              </div>
              """;
        return Layout(db,
            "PCI World — Make the decision. Control the outcome.",
            "A free daily project challenge from the Project Controls Institute. Step into a realistic project situation, examine the evidence and decide what happens next.",
            $"""
            <span class="kicker">PCI World Challenge</span>
            <h1>The project is already moving.<br>The decision is now yours.</h1>
            <p class="lede">Step into a realistic project situation, examine the evidence and decide what happens next. Five to ten minutes. Free. No project experience required.</p>
            {todayCard}
            <h2>How it works</h2>
            <div class="card">
              <ol style="padding-left:20px;display:grid;gap:8px">
                <li><b>Read the situation.</b> A real-shaped project moment with the evidence in front of you — synthetic data, real methods.</li>
                <li><b>Do the work.</b> Compute the measures that matter and make the judgement calls a professional would face.</li>
                <li><b>See the consequences.</b> Deterministic scoring, your professional decision profile, and what each choice would have caused.</li>
              </ol>
            </div>
            <h2>Where it leads</h2>
            <div class="card">
              <p>PCI World is practice with evidence. When you want the full discipline — multi-step simulations, coaching and competency tracking — continue in the <a href="{simlab}">PCI Simulation Lab</a>. When you are ready for formal recognition, explore the certifications on the official <a href="{E(InstituteUrl(db))}" target="_blank" rel="noopener noreferrer">Project Controls Institute website <span aria-hidden="true">&#8599;</span></a>.</p>
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
              var l = document.createElement('label'); l.setAttribute('for', id); l.textContent = a.label;
              var i = document.createElement('input');
              i.type = 'text'; i.id = id; i.name = a.key; i.inputMode = 'decimal'; i.autocomplete = 'off';
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
            var v = $('ask_' + a.key).value.trim(); if (v.length) out[a.key] = v;
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
            $('start').disabled = true;
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
        renderBrief();
        })();
        """;

    public static string Archive(Db db, List<Dictionary<string, object?>> rows)
    {
        var items = string.Join("", rows.Select(r => $"""
            <tr>
              <td><a href="/world/challenge/{E(H.Str(r["code"]))}">{E(H.Str(r["title"]))}</a></td>
              <td>{E(H.Str(r["industry"]))}</td>
              <td>{E(Cap(H.Str(r["difficulty"])))}</td>
              <td class="num">~{H.L(r["est_minutes"])} min</td>
            </tr>
            """));
        return Layout(db,
            "Challenge Library — PCI World",
            "Every published PCI World challenge: realistic project situations across industries, free to enter.",
            $"""
            <span class="kicker">Challenge Library</span>
            <h1>Every challenge stays open</h1>
            <p class="lede">The daily rotation brings one challenge forward each day — the archive keeps them all playable.</p>
            <div class="card">
            <table>
              <thead><tr><th scope="col">Challenge</th><th scope="col">Industry</th><th scope="col">Difficulty</th><th scope="col">Time</th></tr></thead>
              <tbody>{items}</tbody>
            </table>
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
            ogDesc: share.Length > 0 ? share : $"Verified PCI World challenge result: {title}.");
    }

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
