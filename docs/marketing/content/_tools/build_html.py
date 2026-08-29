#!/usr/bin/env python3
"""Render the content run as one browsable HTML page.

Design is grounded in PCI's own identity from backend/wwwroot/assets/logo.svg — navy field,
gold on the AI mark, a crimson accent bar — rather than a generic document theme, because
this is PCI's estate and it should look like it. Three type roles, each earning its place:
Archivo for display (the site's own face), Source Serif 4 for article bodies because this is
half a million words of actual reading, and Inter with tabular figures for the SEO chrome
where numbers line up in columns.

Bodies sit inside collapsed <details>. A browser does not lay out the contents of a closed
disclosure, so the page opens instantly instead of rendering 518,000 words nobody asked to
see at once; the SEO layer and the links stay visible, which is what a reviewer scans.
"""
import importlib.util
import json
import re
from pathlib import Path

HERE = Path(__file__).resolve().parent
_s = importlib.util.spec_from_file_location("md2html", HERE / "md2html.py")
md2html = importlib.util.module_from_spec(_s); _s.loader.exec_module(md2html)
_b = importlib.util.spec_from_file_location("bb", HERE / "build_bundle.py")
bb = importlib.util.module_from_spec(_b); _b.loader.exec_module(bb)

E = md2html.esc
DOMAINS = bb.DOMAINS
TERRITORY = {
    "projectcontrolsinstitute.org": "The hub — credentials, the Standards, earned value, cost control",
    "pciai.org": "AI in project controls — governance, tooling, model evaluation",
    "credentialfinder.org": "Verification and comparison",
    "pciworld.org": "Careers and community",
    "pciglobal.ai": "Regional and market-specific",
}

CSS = """
:root{
  --navy:#1D3C92; --navy-deep:#13245A; --gold:#B8923E; --gold-soft:#E7CB82;
  --crimson:#C13329;
  --ground:#F6F7FB; --surface:#FFFFFF; --sunken:#EDEFF6;
  --ink:#141A2A; --ink-soft:#3D4761; --muted:#6B7590; --line:#DDE2EE;
  --ok:#2F6E4F; --warn:#8A6212;
  --shadow:0 1px 2px rgba(19,36,90,.06), 0 8px 24px rgba(19,36,90,.06);
}
@media (prefers-color-scheme:dark){
  :root:not([data-theme="light"]){
    --navy:#7D97E8; --navy-deep:#A9BCF2; --gold:#D8B15E; --gold-soft:#8A6E30;
    --crimson:#E8695E;
    --ground:#0D111C; --surface:#141A29; --sunken:#1B2233;
    --ink:#E8ECF7; --ink-soft:#BCC5DB; --muted:#8B96B2; --line:#28314A;
    --ok:#79C79C; --warn:#DCB964;
    --shadow:0 1px 2px rgba(0,0,0,.4), 0 8px 24px rgba(0,0,0,.3);
  }
}
:root[data-theme="dark"]{
  --navy:#7D97E8; --navy-deep:#A9BCF2; --gold:#D8B15E; --gold-soft:#8A6E30;
  --crimson:#E8695E;
  --ground:#0D111C; --surface:#141A29; --sunken:#1B2233;
  --ink:#E8ECF7; --ink-soft:#BCC5DB; --muted:#8B96B2; --line:#28314A;
  --ok:#79C79C; --warn:#DCB964;
  --shadow:0 1px 2px rgba(0,0,0,.4), 0 8px 24px rgba(0,0,0,.3);
}
*{box-sizing:border-box}
body{
  margin:0; background:var(--ground); color:var(--ink);
  font-family:Inter,-apple-system,BlinkMacSystemFont,"Segoe UI",sans-serif;
  font-size:16px; line-height:1.6; -webkit-font-smoothing:antialiased;
}
.masthead{
  background:linear-gradient(135deg,#1D3C92,#13245A); color:#fff; padding:44px 28px 0;
  border-bottom:3px solid var(--crimson);
}
.mast-in{max-width:1360px;margin:0 auto;padding-bottom:36px}
.eyebrow{
  font-size:11px;letter-spacing:.16em;text-transform:uppercase;font-weight:600;
  color:var(--gold-soft);margin:0 0 10px;
}
.masthead h1{
  font-family:Archivo,Inter,sans-serif;font-weight:800;letter-spacing:-.022em;
  font-size:clamp(28px,4.2vw,46px);line-height:1.06;margin:0 0 14px;text-wrap:balance;color:#fff;
}
.mast-lede{max-width:62ch;color:#D6DDF4;font-size:16.5px;margin:0 0 26px}
.stats{display:flex;flex-wrap:wrap;gap:30px;margin:0 0 26px}
.stat b{
  display:block;font-family:Archivo,sans-serif;font-weight:800;font-size:26px;
  letter-spacing:-.02em;font-variant-numeric:tabular-nums;color:#fff;
}
.stat span{font-size:11.5px;letter-spacing:.1em;text-transform:uppercase;color:#9FB0DE}
.reach{width:100%;border-collapse:collapse;font-size:13.5px;color:#D6DDF4}
.reach th{
  text-align:left;font-size:10.5px;letter-spacing:.12em;text-transform:uppercase;
  color:#8FA3D6;font-weight:600;padding:0 14px 8px 0;border-bottom:1px solid rgba(255,255,255,.16);
}
.reach td{padding:9px 14px 9px 0;border-bottom:1px solid rgba(255,255,255,.08)}
.reach td:first-child{font-weight:600;color:#fff;white-space:nowrap}
.reach td:last-child{text-align:right;font-variant-numeric:tabular-nums;color:var(--gold-soft);font-weight:600}
.wrap{max-width:1360px;margin:0 auto;padding:0 28px;display:grid;grid-template-columns:274px 1fr;gap:44px;align-items:start}
@media(max-width:980px){.wrap{grid-template-columns:1fr;gap:0}.idx{position:static;max-height:none;margin:24px 0 0}}
.idx{
  position:sticky;top:18px;max-height:calc(100vh - 36px);overflow-y:auto;
  margin:28px 0;padding:18px;background:var(--surface);border:1px solid var(--line);
  border-radius:10px;box-shadow:var(--shadow);font-size:13px;
}
.idx h2{font-size:10.5px;letter-spacing:.14em;text-transform:uppercase;color:var(--muted);margin:0 0 12px;font-weight:600}
.idx a{display:block;padding:4px 0;color:var(--ink-soft);text-decoration:none;border-bottom:1px solid transparent}
.idx a:hover{color:var(--navy);border-bottom-color:var(--line)}
.idx .g{margin:16px 0 6px;font-weight:700;color:var(--ink);font-size:12.5px;border-top:1px solid var(--line);padding-top:12px}
.idx .g:first-of-type{border-top:0;margin-top:0;padding-top:0}
.main{padding:28px 0 80px;min-width:0}
.tools{display:flex;gap:10px;flex-wrap:wrap;margin:0 0 22px}
button{
  font:inherit;font-size:13px;font-weight:600;padding:8px 14px;border-radius:7px;
  border:1px solid var(--line);background:var(--surface);color:var(--ink-soft);cursor:pointer;
}
button:hover{border-color:var(--navy);color:var(--navy)}
button:focus-visible{outline:2px solid var(--navy);outline-offset:2px}
.grp{
  font-family:Archivo,sans-serif;font-weight:800;letter-spacing:-.02em;
  font-size:clamp(20px,2.4vw,27px);margin:46px 0 4px;padding-bottom:10px;
  border-bottom:2px solid var(--gold);color:var(--ink);text-wrap:balance;
}
.grp:first-of-type{margin-top:0}
.grp-sub{color:var(--muted);font-size:13px;margin:0 0 22px}
.piece{
  background:var(--surface);border:1px solid var(--line);border-radius:12px;
  padding:24px 26px;margin:0 0 18px;box-shadow:var(--shadow);
}
.piece > h3{
  font-family:Archivo,sans-serif;font-weight:700;letter-spacing:-.018em;
  font-size:20px;line-height:1.24;margin:0 0 4px;color:var(--ink);text-wrap:balance;
}
.num{color:var(--gold);font-variant-numeric:tabular-nums;font-weight:800;margin-right:8px}
.src{font-size:12px;color:var(--muted);font-family:ui-monospace,SFMono-Regular,Menlo,monospace;margin:0 0 16px}
.seo{
  display:grid;grid-template-columns:auto 1fr;gap:7px 18px;font-size:13.5px;
  background:var(--sunken);border-radius:9px;padding:15px 17px;margin:0 0 16px;
}
.seo dt{font-size:10.5px;letter-spacing:.1em;text-transform:uppercase;color:var(--muted);font-weight:600;padding-top:3px}
.seo dd{margin:0;color:var(--ink-soft);min-width:0;overflow-wrap:anywhere}
.len{font-variant-numeric:tabular-nums;font-size:11.5px;font-weight:600;margin-left:7px;white-space:nowrap}
.len.in{color:var(--ok)} .len.out{color:var(--warn)}
.lk{margin:0 0 16px}
.lk-h{font-size:10.5px;letter-spacing:.1em;text-transform:uppercase;color:var(--muted);font-weight:600;margin:0 0 9px}
.lk ul{list-style:none;margin:0;padding:0;display:flex;flex-direction:column;gap:7px}
.lk li{display:flex;gap:10px;align-items:baseline;font-size:13.5px;flex-wrap:wrap}
.dom{
  font-size:10.5px;font-weight:700;letter-spacing:.04em;padding:2px 8px;border-radius:20px;
  background:var(--sunken);color:var(--navy);border:1px solid var(--line);white-space:nowrap;
}
.lk a{color:var(--navy);text-decoration:none;border-bottom:1px solid var(--line)}
.lk a:hover{border-bottom-color:var(--navy)}
.lk .u{font-family:ui-monospace,SFMono-Regular,Menlo,monospace;font-size:11.5px;color:var(--muted);overflow-wrap:anywhere}
.nolink{font-size:13.5px;color:var(--muted);font-style:italic;margin:0 0 16px}
details.body{border-top:1px solid var(--line);padding-top:14px}
details.body > summary{
  cursor:pointer;font-size:12px;font-weight:600;letter-spacing:.08em;text-transform:uppercase;
  color:var(--navy);list-style:none;display:flex;align-items:center;gap:8px;
}
details.body > summary::-webkit-details-marker{display:none}
details.body > summary::before{content:"▸";font-size:12px;transition:transform .16s}
details.body[open] > summary::before{transform:rotate(90deg)}
.prose{
  font-family:"Source Serif 4",Georgia,serif;font-size:17px;line-height:1.72;
  color:var(--ink);max-width:68ch;margin-top:20px;
}
.prose h3,.prose h4,.prose h5,.prose h6{
  font-family:Archivo,sans-serif;letter-spacing:-.014em;color:var(--ink);
  text-wrap:balance;line-height:1.26;
}
.prose h3{font-size:22px;font-weight:800;margin:34px 0 12px}
.prose h4{font-size:17.5px;font-weight:700;margin:28px 0 10px}
.prose h5{font-size:15px;font-weight:700;margin:22px 0 8px;color:var(--ink-soft)}
.prose p{margin:0 0 15px}
.prose a{color:var(--navy);text-decoration:none;border-bottom:1.5px solid var(--gold-soft);font-weight:500}
.prose a:hover{border-bottom-color:var(--gold)}
.prose strong{font-weight:700}
.prose ul,.prose ol{margin:0 0 15px;padding-left:22px}
.prose li{margin:0 0 7px}
.prose blockquote{
  margin:0 0 16px;padding:2px 0 2px 18px;border-left:3px solid var(--gold);
  color:var(--ink-soft);font-style:normal;
}
.prose code{
  font-family:ui-monospace,SFMono-Regular,Menlo,monospace;font-size:.88em;
  background:var(--sunken);padding:1px 5px;border-radius:4px;
}
.prose pre{background:var(--sunken);padding:14px 16px;border-radius:8px;overflow-x:auto;font-size:13px}
.prose pre code{background:none;padding:0}
.prose hr{border:0;border-top:1px solid var(--line);margin:26px 0}
.tw{overflow-x:auto;margin:0 0 18px;border:1px solid var(--line);border-radius:8px}
.prose table{border-collapse:collapse;width:100%;font-family:Inter,sans-serif;font-size:13.5px}
.prose th,.prose td{padding:9px 13px;text-align:left;border-bottom:1px solid var(--line);vertical-align:top}
.prose th{
  background:var(--sunken);font-weight:600;font-size:11px;letter-spacing:.06em;
  text-transform:uppercase;color:var(--muted);white-space:nowrap;
}
.prose tr:last-child td{border-bottom:0}
.note{
  margin-top:18px;padding:14px 16px;background:var(--sunken);border-left:3px solid var(--navy);
  border-radius:0 8px 8px 0;font-size:13.5px;color:var(--ink-soft);
}
.note b{color:var(--ink);font-weight:700}
.note a{color:var(--navy)}
@media(prefers-reduced-motion:reduce){*{transition:none!important;animation:none!important}}
"""

JS = """
const q=document.getElementById('q'),pieces=[...document.querySelectorAll('.piece')];
document.getElementById('all').onclick=()=>{
  const open=pieces.some(p=>!p.querySelector('details.body')?.open);
  document.querySelectorAll('details.body').forEach(d=>d.open=open);
  document.getElementById('all').textContent=open?'Collapse all bodies':'Expand all bodies';
};
q.addEventListener('input',()=>{
  const t=q.value.trim().toLowerCase();
  let shown=0;
  pieces.forEach(p=>{
    const hit=!t||p.dataset.s.includes(t);
    p.hidden=!hit; if(hit)shown++;
  });
  document.querySelectorAll('.grp,.grp-sub').forEach(h=>{
    let n=h.nextElementSibling,any=false;
    while(n&&!n.classList.contains('grp')){if(n.classList.contains('piece')&&!n.hidden)any=true;n=n.nextElementSibling;}
    h.hidden=!!t&&!any;
  });
  document.getElementById('count').textContent=t?`${shown} of ${pieces.length} pieces`:'';
});
"""


def build():
    files = bb.load()
    groups = {}
    for r in files:
        groups.setdefault(r["group"], []).append(r)
    order = ([k for k in groups if k.startswith("Own site")] +
             sorted(k for k in groups if not k.startswith("Own site")))
    reach = {d: sum(1 for r in files if d in r["domains"]) for d in DOMAINS}
    nlinks = sum(len(r["links"]) for r in files)
    words = sum(r["words"] for r in files)

    h = ['<title>PCI Content Run</title>',
         '<link rel="stylesheet" href="https://fonts.googleapis.com/css2?'
         'family=Archivo:wght@600;700;800&family=Inter:wght@400;500;600;700&'
         'family=Source+Serif+4:opsz,wght@8..60,400;8..60,600;8..60,700&display=swap">',
         f"<style>{CSS}</style>",
         '<header class="masthead"><div class="mast-in">',
         '<p class="eyebrow">Project Controls Institute Global</p>',
         '<h1>The content run, with every link in place</h1>',
         '<p class="mast-lede">Every article, post, carousel and platform asset, each with the '
         'SEO layer it ships with, the links embedded in its prose, and the note recording what '
         'was placed and why.</p>',
         '<div class="stats">',
         f'<div class="stat"><b>{len(files)}</b><span>pieces</span></div>',
         f'<div class="stat"><b>{words:,}</b><span>words</span></div>',
         f'<div class="stat"><b>{nlinks}</b><span>embedded links</span></div>',
         f'<div class="stat"><b>{len(order)}</b><span>publishing surfaces</span></div>',
         '</div>',
         '<table class="reach"><thead><tr><th>Domain</th><th>Territory it owns</th>'
         '<th>Pieces linking to it</th></tr></thead><tbody>']
    for d in DOMAINS:
        h.append(f"<tr><td>{d}</td><td>{TERRITORY[d]}</td><td>{reach[d]}</td></tr>")
    h += ['</tbody></table></div></header>', '<div class="wrap">', '<nav class="idx"><h2>Index</h2>']
    n = 0
    for gname in order:
        h.append(f'<div class="g">{E(gname)}</div>')
        for r in groups[gname]:
            n += 1
            h.append(f'<a href="#p{n}">{n}. {E(r["title"])}</a>')
    h += ['</nav>', '<main class="main">',
          '<div class="tools">'
          '<input id="q" type="search" placeholder="Filter by title, keyword or platform…" '
          'style="font:inherit;font-size:13px;padding:8px 12px;border-radius:7px;'
          'border:1px solid var(--line);background:var(--surface);color:var(--ink);min-width:280px" />'
          '<button id="all">Expand all bodies</button>'
          '<span id="count" style="font-size:13px;color:var(--muted);align-self:center"></span>'
          '</div>']

    n = 0
    for gname in order:
        h.append(f'<h2 class="grp" id="g{bb.slug(gname)}">{E(gname)}</h2>')
        h.append(f'<p class="grp-sub">{len(groups[gname])} piece'
                 f'{"" if len(groups[gname]) == 1 else "s"}</p>')
        for r in groups[gname]:
            n += 1
            search = " ".join([r["title"], r["kw"], r["platform"], r["pillar"], r["credential"]]).lower()
            h.append(f'<article class="piece" id="p{n}" data-s="{E(search)}">')
            h.append(f'<h3><span class="num">{n}</span>{E(r["title"])}</h3>')
            h.append(f'<p class="src">{E(str(r["path"].relative_to(bb.ROOT)))} · '
                     f'{r["words"]:,} words</p>')

            def ln(v, lo, hi):
                cls = "in" if lo <= len(v) <= hi else "out"
                return f'<span class="len {cls}">{len(v)} ch</span>'

            h.append('<dl class="seo">')
            h.append(f'<dt>Platform</dt><dd>{E(r["platform"])}</dd>')
            if r["kw"]:
                h.append(f'<dt>Primary keyword</dt><dd>{E(r["kw"])}</dd>')
            h.append(f'<dt>Title</dt><dd>{E(r["title"])}{ln(r["title"], 48, 62)}</dd>')
            if r["meta"]:
                h.append(f'<dt>Meta</dt><dd>{E(r["meta"])}{ln(r["meta"], 135, 162)}</dd>')
            if r["schema"]:
                h.append(f'<dt>Schema</dt><dd>{E(r["schema"])}</dd>')
            if r["canonical"]:
                h.append(f'<dt>Canonical</dt><dd>{E(r["canonical"][:180])}</dd>')
            if r["pillar"] or r["credential"]:
                h.append(f'<dt>Pillar</dt><dd>{E(r["pillar"] or "—")} · {E(r["credential"] or "—")}</dd>')
            h.append('</dl>')

            if r["links"]:
                h.append('<div class="lk"><p class="lk-h">Links embedded in this piece</p><ul>')
                for l in r["links"]:
                    url = f'https://{l["domain"]}{l["path"]}'
                    anc = l["anchor"] or "(bare URL in the post)"
                    h.append(f'<li><span class="dom">{E(l["domain"])}</span>'
                             f'<a href="{E(url)}" target="_blank" rel="noopener">{E(anc)}</a>'
                             f'<span class="u">{E(l["path"] or "/")}</span></li>')
                h.append('</ul></div>')
            else:
                h.append('<p class="nolink">No link in the body — deliberate for this platform. '
                         'The note below says why.</p>')

            h.append('<details class="body"><summary>Read the piece</summary>'
                     f'<div class="prose">{md2html.convert(bb.demote(r["body"], 1), 3)}</div>')
            if r["note"]:
                note = re.sub(r"^\s*[*_]+|[*_]+\s*$", "", r["note"]).strip()
                h.append(f'<div class="note">{md2html.convert(note, 5)}</div>')
            h.append('</details></article>')

            ld = {"@context": "https://schema.org",
                  "@type": "FAQPage" if r["schema"].strip() == "FAQPage" else "Article",
                  "headline": r["title"], "description": r["meta"],
                  "publisher": {"@type": "Organization", "name": "Project Controls Institute Global"}}
            h.append('<script type="application/ld+json">' +
                     json.dumps(ld, ensure_ascii=False).replace("</", "<\\/") + '</script>')

    h += ['</main></div>', f"<script>{JS}</script>"]
    dest = bb.ROOT.parent / "PCI-content-run.html"
    dest.write_text("\n".join(h), encoding="utf-8")
    return dest, len(files), nlinks


if __name__ == "__main__":
    d, n, l = build()
    print(f"{d}  ({d.stat().st_size/1_048_576:.1f} MB)  {n} pieces, {l} links")
