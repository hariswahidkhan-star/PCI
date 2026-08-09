#!/usr/bin/env python3
"""Build the enrolment growth model as a self-contained interactive page.

Everything is inlined — the brand woff2 files become data URIs — because the artifact CSP blocks
external hosts, and a linked webfont there fails silently into a system fallback.

The page is a tool, not a document: it is operated, not read, so the summary sits above the detail
and the levers stay in one rail. The default prices come from `backend/schema.sql` seeds, so the
model opens on PCI's real numbers rather than invented ones.

Usage:  python3 build_growth_model.py [output.html]
"""
import base64
import pathlib
import sys

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parents[1]
FONTS = ROOT / "backend/wwwroot/assets/fonts"
OUT = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else HERE / "growth-model.html"


def data_uri(name: str) -> str:
    b64 = base64.b64encode((FONTS / name).read_bytes()).decode()
    return f"data:font/woff2;base64,{b64}"


PAGE = """<title>PCI enrolment growth model</title>
<style>
@font-face {{ font-family: Archivo; src: url({archivo}) format('woff2');
              font-weight: 700 900; font-display: block; }}
@font-face {{ font-family: InterV; src: url({inter}) format('woff2');
              font-weight: 400 700; font-display: block; }}

/* Light is the base set. Neutrals lean cool toward the navy rather than sitting at pure grey. */
:root {{
  --navy-deep:#13245A; --navy:#1D3C92; --navy-soft:#3355B4;
  --gold:#B8923E; --gold-lit:#E7CB82; --crimson:#C13329;
  --ground:#F5F7FA; --panel:#FFFFFF; --sunk:#EEF2F7;
  --line:#DFE5EE; --line-strong:#C7D0DE;
  --ink:#0F172A; --slate:#475569; --mist:#67748A;
  --accent:var(--navy); --figure:var(--navy-deep);
  --shadow:0 1px 2px rgba(15,23,42,.05), 0 12px 32px -20px rgba(19,36,90,.35);
}}
@media (prefers-color-scheme: dark) {{
  :root:not([data-theme="light"]) {{
    --ground:#0B1430; --panel:#111D40; --sunk:#0D1734;
    --line:#22315C; --line-strong:#334472;
    --ink:#EEF2FB; --slate:#B4C1DC; --mist:#8695B7;
    --accent:#7FA0E8; --figure:var(--gold-lit); --gold:var(--gold-lit);
    --shadow:0 1px 2px rgba(0,0,0,.4), 0 16px 40px -24px rgba(0,0,0,.8);
  }}
}}
:root[data-theme="dark"] {{
  --ground:#0B1430; --panel:#111D40; --sunk:#0D1734;
  --line:#22315C; --line-strong:#334472;
  --ink:#EEF2FB; --slate:#B4C1DC; --mist:#8695B7;
  --accent:#7FA0E8; --figure:var(--gold-lit); --gold:var(--gold-lit);
  --shadow:0 1px 2px rgba(0,0,0,.4), 0 16px 40px -24px rgba(0,0,0,.8);
}}

* {{ box-sizing:border-box; }}
body {{ margin:0; background:var(--ground); color:var(--ink);
       font-family:InterV,system-ui,sans-serif; font-size:15px; line-height:1.5; }}
h1,h2,h3 {{ font-family:Archivo,system-ui,sans-serif; font-weight:900;
            letter-spacing:-.025em; margin:0; text-wrap:balance; }}
.num {{ font-variant-numeric:tabular-nums; }}

.wrap {{ max-width:1240px; margin:0 auto; padding:34px 24px 64px; }}
header {{ display:flex; flex-wrap:wrap; gap:16px; align-items:flex-end;
          justify-content:space-between; padding-bottom:18px;
          border-bottom:3px solid var(--navy); }}
.eyebrow {{ font-size:12px; font-weight:700; letter-spacing:.16em; text-transform:uppercase;
            color:var(--gold); }}
h1 {{ font-size:30px; margin-top:6px; }}
.sub {{ color:var(--mist); font-size:14px; max-width:60ch; margin-top:6px; }}
.bar {{ width:92px; height:5px; background:var(--crimson); margin:14px 0 0; }}

.grid {{ display:grid; grid-template-columns:minmax(0,360px) minmax(0,1fr);
         gap:26px; margin-top:26px; align-items:start; }}
@media (max-width:940px) {{ .grid {{ grid-template-columns:1fr; }} }}
@media (max-width:560px) {{ .stats {{ grid-template-columns:repeat(2,1fr); }} }}

.panel {{ background:var(--panel); border:1px solid var(--line); border-radius:4px;
          box-shadow:var(--shadow); }}
.panel > h2 {{ font-size:13px; letter-spacing:.13em; text-transform:uppercase; color:var(--mist);
               font-weight:700; font-family:InterV,sans-serif; padding:16px 20px;
               border-bottom:1px solid var(--line); }}
.pad {{ padding:18px 20px; display:flex; flex-direction:column; gap:16px; }}

fieldset {{ border:0; margin:0; padding:0; display:flex; flex-direction:column; gap:14px; }}
legend {{ font-size:11px; font-weight:700; letter-spacing:.14em; text-transform:uppercase;
          color:var(--accent); padding:0 0 4px; }}
.lever {{ display:grid; grid-template-columns:1fr auto; gap:4px 10px; align-items:center; }}
.lever label {{ font-size:13.5px; color:var(--slate); }}
.lever .val {{ font-size:14px; font-weight:700; color:var(--ink); min-width:74px;
               text-align:right; font-variant-numeric:tabular-nums; }}
.lever input[type=range] {{ grid-column:1 / -1; width:100%; accent-color:var(--accent);
                            height:20px; margin:0; }}
.lever input[type=range]:focus-visible {{ outline:2.5px solid var(--accent); outline-offset:3px; }}
.hint {{ grid-column:1 / -1; font-size:12px; color:var(--mist); margin-top:-2px; }}

.headline {{ padding:22px 24px 20px; border-bottom:1px solid var(--line); }}
.headline .k {{ font-size:12px; font-weight:700; letter-spacing:.15em; text-transform:uppercase;
                color:var(--mist); }}
.big {{ font-family:Archivo,sans-serif; font-weight:900; font-size:60px; line-height:1.02;
        color:var(--figure); letter-spacing:-.04em; font-variant-numeric:tabular-nums;
        margin-top:4px; }}
.pill {{ display:inline-flex; align-items:center; gap:7px; font-size:12.5px; font-weight:700;
         padding:5px 11px; border-radius:999px; border:1.5px solid currentColor; margin-top:10px; }}
.pill .dot {{ width:7px; height:7px; border-radius:50%; background:currentColor; }}
.ok {{ color:var(--gold); }} .warn {{ color:var(--navy-soft); }} .bad {{ color:var(--crimson); }}

/* Six stats: an auto-fit track fitted five on a row and left the sixth alone beside a dead
   panel-width gap. Three columns divide evenly instead. */
.stats {{ display:grid; grid-template-columns:repeat(3,1fr); gap:1px;
          background:var(--line); border-bottom:1px solid var(--line); }}
.stat {{ background:var(--panel); padding:15px 20px; }}
.stat .k {{ font-size:11.5px; font-weight:600; letter-spacing:.1em; text-transform:uppercase;
            color:var(--mist); }}
.stat .v {{ font-family:Archivo,sans-serif; font-weight:800; font-size:25px; margin-top:3px;
            color:var(--ink); font-variant-numeric:tabular-nums; letter-spacing:-.02em; }}

table {{ width:100%; border-collapse:collapse; font-size:14px; }}
.scroll {{ overflow-x:auto; }}
th {{ text-align:left; font-size:11.5px; letter-spacing:.09em; text-transform:uppercase;
      color:var(--mist); font-weight:700; padding:12px 20px; border-bottom:1px solid var(--line); }}
td {{ padding:11px 20px; border-bottom:1px solid var(--line); font-variant-numeric:tabular-nums; }}
td.r, th.r {{ text-align:right; }}
tbody tr:last-child td {{ border-bottom:0; }}
.chan {{ font-weight:600; color:var(--ink); }}
.chan small {{ display:block; font-weight:400; color:var(--mist); font-size:12px; }}
/* One accent for every bar: the row label carries identity, the bar carries magnitude, so no
   categorical palette is needed and none can fail a colour-vision check. */
.track {{ height:9px; background:var(--sunk); border-radius:2px; overflow:hidden; min-width:90px; }}
.fill {{ height:100%; background:var(--accent); border-radius:2px; }}
tfoot td {{ font-weight:700; color:var(--ink); border-top:2px solid var(--line-strong); }}

.note {{ margin-top:26px; padding:18px 22px; background:var(--sunk); border-left:5px solid var(--crimson);
         border-radius:3px; font-size:13.5px; color:var(--slate); }}
.note b {{ color:var(--ink); }}
.note p {{ margin:0 0 9px; }} .note p:last-child {{ margin:0; }}
footer {{ margin-top:26px; padding-top:14px; border-top:1px solid var(--line);
          font-size:12.5px; color:var(--mist); display:flex; justify-content:space-between;
          flex-wrap:wrap; gap:10px; }}
button.reset {{ font:inherit; font-size:12.5px; font-weight:600; color:var(--accent);
                background:none; border:1px solid var(--line-strong); border-radius:3px;
                padding:5px 12px; cursor:pointer; }}
button.reset:hover {{ border-color:var(--accent); }}
button.reset:focus-visible {{ outline:2.5px solid var(--accent); outline-offset:2px; }}
@media (prefers-reduced-motion:no-preference) {{ .fill {{ transition:width .18s ease; }} }}
</style>

<div class="wrap">
<header>
  <div>
    <div class="eyebrow">Project Controls Institute Global</div>
    <h1>What 10,000 enrolments cost</h1>
    <div class="sub">Move the levers. Every figure recomputes. Prices open at the platform's own
      seeded defaults — exam $500 less 30%, membership $99 less 50%, recert $99 on a 3-year cycle.</div>
    <div class="bar"></div>
  </div>
  <button class="reset" id="reset" type="button">Reset to defaults</button>
</header>

<div class="grid">
  <section class="panel" aria-label="Assumptions">
    <h2>Levers</h2>
    <div class="pad">
      <fieldset>
        <legend>Goal &amp; price</legend>
        <div class="lever"><label for="target">Target enrolments</label><span class="val" id="target_v"></span>
          <input type="range" id="target" min="500" max="50000" step="500"></div>
        <div class="lever"><label for="price">Exam list price</label><span class="val" id="price_v"></span>
          <input type="range" id="price" min="100" max="1500" step="25"></div>
        <div class="lever"><label for="disc">Average discount</label><span class="val" id="disc_v"></span>
          <input type="range" id="disc" min="0" max="70" step="1"></div>
        <div class="lever"><label for="memb">Take a membership</label><span class="val" id="memb_v"></span>
          <input type="range" id="memb" min="0" max="100" step="5"></div>
        <div class="lever"><label for="recert">Recertify at least once</label><span class="val" id="recert_v"></span>
          <input type="range" id="recert" min="0" max="100" step="5"></div>
      </fieldset>

      <fieldset>
        <legend>Credibility</legend>
        <div class="lever"><label for="cred">Conversion vs an established body</label>
          <span class="val" id="cred_v"></span>
          <input type="range" id="cred" min="20" max="150" step="5">
          <div class="hint">A candidate choosing between a known credential and a new institute
            converts worse. This scales every lead&rarr;enrolment rate. Raise it as accreditation,
            named reviewers and published pass rates land.</div></div>
      </fieldset>

      <fieldset>
        <legend>Where the enrolments come from</legend>
        <div class="lever"><label for="s_search">High-intent search</label><span class="val" id="s_search_v"></span>
          <input type="range" id="s_search" min="0" max="100" step="5"></div>
        <div class="lever"><label for="s_social">Social, low-CPM markets</label><span class="val" id="s_social_v"></span>
          <input type="range" id="s_social" min="0" max="100" step="5"></div>
        <div class="lever"><label for="s_li">LinkedIn, professional</label><span class="val" id="s_li_v"></span>
          <input type="range" id="s_li" min="0" max="100" step="5"></div>
        <div class="lever"><label for="s_org">Organic &amp; content</label><span class="val" id="s_org_v"></span>
          <input type="range" id="s_org" min="0" max="100" step="5"></div>
        <div class="lever"><label for="s_part">Employers &amp; training partners</label><span class="val" id="s_part_v"></span>
          <input type="range" id="s_part" min="0" max="100" step="5">
          <div class="hint" id="mixnote"></div></div>
      </fieldset>

      <fieldset>
        <legend>Channel economics</legend>
        <div class="lever"><label for="cpc_search">Search cost per click</label><span class="val" id="cpc_search_v"></span>
          <input type="range" id="cpc_search" min="20" max="1200" step="10"></div>
        <div class="lever"><label for="cpc_social">Social cost per click</label><span class="val" id="cpc_social_v"></span>
          <input type="range" id="cpc_social" min="5" max="400" step="5"></div>
        <div class="lever"><label for="cpc_li">LinkedIn cost per click</label><span class="val" id="cpc_li_v"></span>
          <input type="range" id="cpc_li" min="100" max="1600" step="25"></div>
        <div class="lever"><label for="lead">Visit &rarr; lead</label><span class="val" id="lead_v"></span>
          <input type="range" id="lead" min="2" max="50" step="1"></div>
        <div class="lever"><label for="enrol">Lead &rarr; enrolment, base</label><span class="val" id="enrol_v"></span>
          <input type="range" id="enrol" min="1" max="30" step="1"></div>
        <div class="lever"><label for="org_cac">Organic cost per enrolment</label><span class="val" id="org_cac_v"></span>
          <input type="range" id="org_cac" min="0" max="200" step="5"></div>
        <div class="lever"><label for="deal">Cost to land one partner</label><span class="val" id="deal_v"></span>
          <input type="range" id="deal" min="1000" max="40000" step="1000"></div>
        <div class="lever"><label for="percand">Candidates per partner</label><span class="val" id="percand_v"></span>
          <input type="range" id="percand" min="10" max="600" step="10"></div>
      </fieldset>

      <fieldset>
        <legend>Fixed cost</legend>
        <div class="lever"><label for="team">Team &amp; content per month</label><span class="val" id="team_v"></span>
          <input type="range" id="team" min="0" max="120000" step="2500"></div>
        <div class="lever"><label for="months">Months to get there</label><span class="val" id="months_v"></span>
          <input type="range" id="months" min="3" max="48" step="1"></div>
      </fieldset>
    </div>
  </section>

  <section class="panel" aria-label="Result">
    <div class="headline">
      <div class="k">Total spend to reach the target</div>
      <div class="big num" id="total">&mdash;</div>
      <div class="pill" id="verdict"><span class="dot"></span><span id="verdict_t"></span></div>
    </div>
    <div class="stats">
      <div class="stat"><div class="k">Blended CAC</div><div class="v num" id="cac">&mdash;</div></div>
      <div class="stat"><div class="k">Value per enrolment</div><div class="v num" id="ltv">&mdash;</div></div>
      <div class="stat"><div class="k">Lifetime revenue</div><div class="v num" id="rev">&mdash;</div></div>
      <div class="stat"><div class="k">Return on spend</div><div class="v num" id="roi">&mdash;</div></div>
      <div class="stat"><div class="k">First-sale margin</div><div class="v num" id="margin">&mdash;</div></div>
      <div class="stat"><div class="k">Spend per month</div><div class="v num" id="burn">&mdash;</div></div>
    </div>
    <div class="scroll">
      <table>
        <thead><tr>
          <th>Channel</th><th class="r">Enrolments</th><th class="r">CAC</th>
          <th class="r">Spend</th><th>Share of spend</th>
        </tr></thead>
        <tbody id="rows"></tbody>
        <tfoot><tr>
          <td>Total</td><td class="r num" id="t_enrol"></td><td class="r num" id="t_cac"></td>
          <td class="r num" id="t_spend"></td><td></td>
        </tr></tfoot>
      </table>
    </div>
  </section>
</div>

<div class="note">
  <p><b>What this is not.</b> It has no measurement in it. The prices are real — they come from the
  platform's seeded pricing rules — but every conversion rate is an assumption until PCI has run
  enough traffic to replace it. Treat the output as the shape of the answer, not the answer.</p>
  <p><b>The credibility lever is the one to watch.</b> At 50% it says a new institute converts at
  half the rate of an established one, which is the realistic opening position and roughly doubles
  the spend. Moving it is not a marketing job — it needs accreditation progress, named reviewers,
  employer logos and published pass rates.</p>
  <p><b>Partners are the structural answer.</b> Ten thousand retail buyers, one at a time, is the
  expensive route. The platform already carries a training-partner portal and a sponsored route for
  employer-funded candidates; fifty employers at two hundred candidates each is the same target with
  a business-development team instead of an advertising budget.</p>
</div>

<footer>
  <span>Defaults from <code>backend/schema.sql</code> pricing rules &middot; recert on a 3-year cycle</span>
  <span>Project Controls Institute Global</span>
</footer>
</div>

<script>
const D = {{ target:10000, price:500, disc:30, memb:60, recert:45, cred:50,
  s_search:20, s_social:30, s_li:5, s_org:20, s_part:25,
  cpc_search:300, cpc_social:35, cpc_li:600, lead:20, enrol:6,
  org_cac:25, deal:8000, percand:150, team:25000, months:18 }};
const S = {{...D}};
const $ = id => document.getElementById(id);

const usd = n => n >= 1e6 ? '$' + (n/1e6).toFixed(n < 1e7 ? 2 : 1) + 'M'
               : n >= 1e3 ? '$' + Math.round(n/1e3) + 'k' : '$' + Math.round(n);
const money = n => '$' + Math.round(n).toLocaleString('en-US');
const int = n => Math.round(n).toLocaleString('en-US');

// Cost per click is held in cents so the slider can step in sensible money, not floats.
const FMT = {{
  target:v => int(v), price:v => money(v), disc:v => v + '%', memb:v => v + '%',
  recert:v => v + '%', cred:v => v + '%',
  s_search:v => v + '%', s_social:v => v + '%', s_li:v => v + '%', s_org:v => v + '%',
  s_part:v => v + '%',
  cpc_search:v => '$' + (v/100).toFixed(2), cpc_social:v => '$' + (v/100).toFixed(2),
  cpc_li:v => '$' + (v/100).toFixed(2),
  lead:v => v + '%', enrol:v => v + '%', org_cac:v => money(v),
  deal:v => money(v), percand:v => int(v), team:v => money(v), months:v => v + ' months'
}};

function model() {{
  const examNet = S.price * (1 - S.disc/100);
  // 3-year cycle: one recert inside a working life is the conservative read, so recert revenue is
  // counted once and only for the share who renew.
  const ltv = examNet + 49.5 * (S.memb/100) + 99 * (S.recert/100);

  const enrolRate = (S.enrol/100) * (S.cred/100);      // credibility scales the closing rate only
  const leadRate = S.lead/100;
  const funnelCac = cpcCents => (leadRate * enrolRate) > 0
      ? (cpcCents/100) / (leadRate * enrolRate) : Infinity;

  const raw = [
    ['High-intent search', 'paid search, cert keywords', S.s_search, funnelCac(S.cpc_search)],
    ['Social, low-CPM markets', 'South Asia, MEA, SE Asia', S.s_social, funnelCac(S.cpc_social)],
    ['LinkedIn, professional', 'cold, highest CPC', S.s_li, funnelCac(S.cpc_li)],
    ['Organic &amp; content', 'the outline, the carousels, search', S.s_org, S.org_cac],
    ['Employers &amp; partners', 'sponsored route, partner portal', S.s_part,
      S.percand > 0 ? S.deal / S.percand : Infinity]
  ];
  const shareSum = raw.reduce((a, r) => a + r[2], 0);
  const rows = raw.map(([name, sub, share, cac]) => {{
    const s = shareSum > 0 ? share / shareSum : 0;
    const enrolments = S.target * s;
    return {{ name, sub, share: s, enrolments, cac, spend: enrolments * cac }};
  }});

  const paid = rows.reduce((a, r) => a + (isFinite(r.spend) ? r.spend : 0), 0);
  const fixed = S.team * S.months;
  const total = paid + fixed;
  return {{ rows, paid, fixed, total, ltv, examNet, shareSum,
           cac: S.target > 0 ? total / S.target : 0,
           revenue: S.target * ltv }};
}}

function render() {{
  for (const k in S) {{ const el = $(k + '_v'); if (el) el.textContent = FMT[k](S[k]); }}
  const m = model();

  $('total').textContent = usd(m.total);
  $('cac').textContent = money(m.cac);
  $('ltv').textContent = money(m.ltv);
  $('rev').textContent = usd(m.revenue);
  $('roi').textContent = m.total > 0 ? (m.revenue / m.total).toFixed(1) + '\\u00D7' : '\\u2014';
  $('margin').textContent = money(m.examNet - m.cac);
  $('burn').textContent = usd(m.total / Math.max(S.months, 1));

  // Three states, judged on the first sale rather than on lifetime value: a business that cannot
  // fund the next enrolment from the last one is running on capital, whatever the LTV says.
  const first = m.examNet - m.cac;
  const p = $('verdict'), t = $('verdict_t');
  p.className = 'pill ' + (first > m.examNet * 0.3 ? 'ok' : first > 0 ? 'warn' : 'bad');
  t.textContent = first > m.examNet * 0.3
      ? 'Healthy — the first sale funds acquisition with room'
      : first > 0
      ? 'Thin — the first sale barely covers acquisition'
      : 'Underwater — every enrolment costs more than it earns';

  const max = Math.max(...m.rows.map(r => isFinite(r.spend) ? r.spend : 0), 1);
  $('rows').innerHTML = m.rows.map(r => `
    <tr>
      <td class="chan">${{r.name}}<small>${{r.sub}}</small></td>
      <td class="r num">${{int(r.enrolments)}}</td>
      <td class="r num">${{isFinite(r.cac) ? money(r.cac) : '\\u2014'}}</td>
      <td class="r num">${{isFinite(r.spend) ? usd(r.spend) : '\\u2014'}}</td>
      <td><div class="track"><div class="fill" style="width:${{
          (isFinite(r.spend) ? r.spend / max : 0) * 100}}%"></div></div></td>
    </tr>`).join('') + `
    <tr>
      <td class="chan">Team &amp; content<small>${{S.months}} months, fixed</small></td>
      <td class="r num">\\u2014</td><td class="r num">\\u2014</td>
      <td class="r num">${{usd(m.fixed)}}</td>
      <td><div class="track"><div class="fill" style="width:${{
          (m.fixed / Math.max(max, m.fixed)) * 100}}%"></div></div></td>
    </tr>`;

  $('t_enrol').textContent = int(S.target);
  $('t_cac').textContent = money(m.cac);
  $('t_spend').textContent = usd(m.total);
  $('mixnote').textContent = Math.abs(m.shareSum - 100) < 0.5
      ? 'Mix sums to 100%.'
      : `Mix sums to ${{Math.round(m.shareSum)}}% — shares are normalised, so only the ratios matter.`;
}}

for (const k in S) {{
  const el = $(k);
  if (!el) continue;
  el.value = S[k];
  el.addEventListener('input', () => {{ S[k] = +el.value; render(); }});
}}
$('reset').addEventListener('click', () => {{
  for (const k in D) {{ S[k] = D[k]; const el = $(k); if (el) el.value = D[k]; }}
  render();
}});
render();
</script>
"""


def main() -> None:
    OUT.write_text(PAGE.format(archivo=data_uri("archivo-latin.woff2"),
                               inter=data_uri("inter-latin.woff2")), encoding="utf-8")
    print(f"built: {OUT.name}  ({OUT.stat().st_size // 1024} KB)")


if __name__ == "__main__":
    sys.exit(main())
