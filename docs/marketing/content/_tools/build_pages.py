#!/usr/bin/env python3
"""Render every piece as a standalone, publish-ready HTML page with a complete SEO head.

The earlier deliverables were documents *about* the content — useful for review, useless for
publishing. This produces the content itself: one page per piece, with the head tags a search
engine and a social crawler actually read, and structured data built from the prose rather
than asserted by a label.

The FAQ work is the part that earns its keep. 200 articles carry 1,068 question-and-answer
pairs written as bolded questions followed by their answer paragraph. Declaring
`schema: FAQPage` in front matter tells a crawler nothing; emitting those pairs as real
Question and Answer entities is what a rich result is built from, so they are parsed out of
the body and emitted properly.
"""
import html
import importlib.util
import json
import re
from pathlib import Path

HERE = Path(__file__).resolve().parent
_m = importlib.util.spec_from_file_location("md2html", HERE / "md2html.py")
md2html = importlib.util.module_from_spec(_m); _m.loader.exec_module(md2html)
_b = importlib.util.spec_from_file_location("bb", HERE / "build_bundle.py")
bb = importlib.util.module_from_spec(_b); _b.loader.exec_module(bb)

E = md2html.esc
ORG = "Project Controls Institute Global"
ORG_URL = "https://projectcontrolsinstitute.org"
LOGO = "https://projectcontrolsinstitute.org/assets/logo.png"

FAQ_Q = re.compile(r"^\*\*(.{6,180}\?)\*\*\s*$", re.M)


def faq_pairs(body):
    """Pull question/answer pairs out of the prose. A question is a bolded line ending in a
    question mark; its answer is the prose that follows, up to the next question or heading.
    Answers are flattened to text because schema.org wants the answer, not its markup."""
    out = []
    for m in FAQ_Q.finditer(body):
        rest = body[m.end():]
        nxt = FAQ_Q.search(rest)
        blk = rest[:nxt.start()] if nxt else rest
        blk = re.split(r"\n\s*#{1,6}\s|\n\s*---\s*\n", blk)[0]
        txt = re.sub(r"\[([^\]]*)\]\([^)]*\)", r"\1", blk)
        txt = re.sub(r"[*_`>|]", " ", txt)
        txt = " ".join(txt.split())
        if 25 <= len(txt) <= 1200:
            out.append((m.group(1).strip(), txt))
    return out


def page_url(r):
    host = None
    hm = re.match(r"Own site\s*[—\-–]\s*([a-z0-9.]+)", r["platform"], re.I)
    if hm:
        host = hm.group(1).lower()
        slug = re.sub(r"^\d+-own-site-", "", r["path"].stem)
        return f"https://{host}/{slug}", True
    c = r["canonical"] or ""
    u = re.search(r"https?://[^\s)\"']+", c)
    if u:
        return u.group(0).rstrip(".,;"), False
    p = re.search(r"->\s*(/\S+)", c)
    if p:
        return ORG_URL + p.group(1), False
    return "", False


CSS = """*{box-sizing:border-box}
:root{--navy:#1D3C92;--deep:#13245A;--gold:#B8923E;--crimson:#C13329;
--ink:#141A2A;--soft:#3D4761;--muted:#6B7590;--line:#DDE2EE;--sunken:#F1F3F9;--bg:#fff}
@media(prefers-color-scheme:dark){:root:not([data-theme=light]){--navy:#7D97E8;--deep:#A9BCF2;
--gold:#D8B15E;--crimson:#E8695E;--ink:#E8ECF7;--soft:#BCC5DB;--muted:#8B96B2;
--line:#28314A;--sunken:#161D2C;--bg:#0D111C}}
:root[data-theme=dark]{--navy:#7D97E8;--deep:#A9BCF2;--gold:#D8B15E;--crimson:#E8695E;
--ink:#E8ECF7;--soft:#BCC5DB;--muted:#8B96B2;--line:#28314A;--sunken:#161D2C;--bg:#0D111C}
body{margin:0;background:var(--bg);color:var(--ink);
font-family:"Source Serif 4",Georgia,serif;font-size:18px;line-height:1.72}
.bar{background:linear-gradient(135deg,#1D3C92,#13245A);border-bottom:3px solid var(--crimson);
padding:14px 24px;font-family:Inter,system-ui,sans-serif}
.bar a{color:#fff;text-decoration:none;font-weight:700;font-size:14px;letter-spacing:-.01em}
.bar span{color:#9FB0DE;font-size:12px;margin-left:10px}
main{max-width:72ch;margin:0 auto;padding:44px 24px 90px}
h1{font-family:Archivo,Inter,system-ui,sans-serif;font-weight:800;letter-spacing:-.022em;
font-size:clamp(28px,4.5vw,42px);line-height:1.1;margin:0 0 18px;text-wrap:balance}
h2,h3,h4,h5{font-family:Archivo,Inter,system-ui,sans-serif;letter-spacing:-.015em;
text-wrap:balance;line-height:1.25}
h2{font-size:25px;font-weight:800;margin:42px 0 14px}
h3{font-size:20px;font-weight:700;margin:32px 0 11px}
h4{font-size:17px;font-weight:700;margin:26px 0 9px;color:var(--soft)}
h5{font-size:15px;font-weight:700;margin:22px 0 8px;color:var(--muted)}
p{margin:0 0 17px}
a{color:var(--navy);text-decoration:none;border-bottom:1.5px solid rgba(184,146,62,.5)}
a:hover{border-bottom-color:var(--gold)}
a:focus-visible{outline:2px solid var(--navy);outline-offset:2px}
strong{font-weight:700}
ul,ol{margin:0 0 17px;padding-left:24px}li{margin:0 0 8px}
blockquote{margin:0 0 18px;padding:4px 0 4px 20px;border-left:3px solid var(--gold);color:var(--soft)}
code{font-family:ui-monospace,SFMono-Regular,Menlo,monospace;font-size:.86em;
background:var(--sunken);padding:2px 6px;border-radius:4px}
pre{background:var(--sunken);padding:15px 17px;border-radius:8px;overflow-x:auto;font-size:14px}
pre code{background:none;padding:0}
hr{border:0;border-top:1px solid var(--line);margin:30px 0}
.tw{overflow-x:auto;margin:0 0 20px;border:1px solid var(--line);border-radius:8px}
table{border-collapse:collapse;width:100%;font-family:Inter,system-ui,sans-serif;font-size:14.5px;
font-variant-numeric:tabular-nums}
th,td{padding:10px 14px;text-align:left;border-bottom:1px solid var(--line);vertical-align:top}
th{background:var(--sunken);font-weight:600;font-size:11.5px;letter-spacing:.06em;
text-transform:uppercase;color:var(--muted);white-space:nowrap}
tr:last-child td{border-bottom:0}
.lede{font-size:19.5px;color:var(--soft);margin:0 0 30px;padding-bottom:26px;
border-bottom:1px solid var(--line)}
footer{margin-top:56px;padding-top:22px;border-top:1px solid var(--line);
font-family:Inter,system-ui,sans-serif;font-size:13px;color:var(--muted)}
footer a{color:var(--muted)}
@media(prefers-reduced-motion:reduce){*{transition:none!important;animation:none!important}}
"""


def render(r, outdir):
    url, own = page_url(r)
    title, desc = r["title"], r["meta"]
    body_md = r["body"]
    pairs = faq_pairs(body_md)

    # The page's own <h1> is the title, so body headings start at h2 and never compete with
    # it. Demoting to h2 rather than stripping one h1 matters: a Pinterest spec and a
    # SlideShare script carry four and five top-level sections each, and an earlier cut that
    # removed only the first left five pages with multiple h1s — which splits the document
    # outline a crawler builds and wastes the one signal a title tag is meant to reinforce.
    inner = md2html.convert(bb.demote(body_md, 1), 2)
    # Where the first body heading simply repeats the title, drop it rather than say it twice.
    first = re.match(r"\s*<h2[^>]*>(.*?)</h2>", inner, re.S)
    if first and re.sub(r"<[^>]+>", "", first.group(1)).strip().lower() == title.strip().lower():
        inner = inner[first.end():]

    graph = [{
        "@type": "Article", "headline": title[:110], "description": desc,
        "inLanguage": "en-GB",
        "mainEntityOfPage": {"@type": "WebPage", "@id": url} if url else None,
        "publisher": {"@type": "Organization", "name": ORG, "url": ORG_URL,
                      "logo": {"@type": "ImageObject", "url": LOGO}},
        "about": r["pillar"] or None,
        "keywords": ", ".join(x for x in [r["kw"]] + [s.strip() for s in r["secondary"].split(",")] if x) or None,
    }]
    graph[0] = {k: v for k, v in graph[0].items() if v}
    if pairs:
        graph.append({"@type": "FAQPage", "mainEntity": [
            {"@type": "Question", "name": q,
             "acceptedAnswer": {"@type": "Answer", "text": a}} for q, a in pairs]})

    ld = json.dumps({"@context": "https://schema.org", "@graph": graph},
                    ensure_ascii=False, indent=1).replace("</", "<\\/")

    h = ['<!doctype html>', '<html lang="en-GB">', '<head>', '<meta charset="utf-8">',
         '<meta name="viewport" content="width=device-width,initial-scale=1">',
         f'<title>{E(title)}</title>']
    if desc:
        h.append(f'<meta name="description" content="{E(desc)}">')
    if url:
        h.append(f'<link rel="canonical" href="{E(url)}">')
    # A page that will never live at a URL of ours is a copy sheet for whoever posts it, not
    # something to index. Marking it so keeps 200-odd near-duplicate drafts of published
    # social copy out of any index they are accidentally served from.
    robots = ("index,follow,max-image-preview:large,max-snippet:-1" if url
              else "noindex,follow")
    h += [f'<meta name="robots" content="{robots}">',
          f'<meta property="og:type" content="article">',
          f'<meta property="og:title" content="{E(title)}">']
    if desc:
        h.append(f'<meta property="og:description" content="{E(desc)}">')
    if url:
        h.append(f'<meta property="og:url" content="{E(url)}">')
    h += [f'<meta property="og:site_name" content="{E(ORG)}">',
          '<meta property="og:locale" content="en_GB">',
          '<meta name="twitter:card" content="summary_large_image">',
          f'<meta name="twitter:title" content="{E(title)}">']
    if desc:
        h.append(f'<meta name="twitter:description" content="{E(desc)}">')
    h += ['<link rel="preconnect" href="https://fonts.googleapis.com">',
          '<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>',
          '<link rel="stylesheet" href="https://fonts.googleapis.com/css2?'
          'family=Archivo:wght@700;800&family=Inter:wght@400;600;700&'
          'family=Source+Serif+4:opsz,wght@8..60,400;8..60,600;8..60,700&display=swap">',
          f'<style>{CSS}</style>',
          f'<script type="application/ld+json">{ld}</script>', '</head>', '<body>',
          f'<div class="bar"><a href="{ORG_URL}">{E(ORG)}</a>'
          f'<span>{E(r["platform"][:70])}</span></div>',
          '<main>', '<article>', f'<h1>{E(title)}</h1>']
    if desc:
        h.append(f'<p class="lede">{E(desc)}</p>')
    h += [inner, '</article>',
          f'<footer><p>{E(ORG)} publishes certification requirements. '
          'Nothing here is legal, tax or accounting advice.</p>']
    if url:
        h.append(f'<p>Canonical: <a href="{E(url)}">{E(url)}</a></p>')
    h += ['</footer>', '</main>', '</body>', '</html>']

    dest = outdir / (r["path"].stem + ".html")
    dest.write_text("\n".join(h), encoding="utf-8")
    return url, own, len(pairs)


def build():
    files = bb.load()
    for r in files:
        m = bb.FM.match(r["path"].read_text(encoding="utf-8"))
        r["secondary"] = bb.g(m.group(1), "secondary_kw") if m else ""
    out = bb.ROOT.parent / "pages"
    out.mkdir(exist_ok=True)
    urls, faqs, own_n = [], 0, 0
    for r in files:
        u, own, nf = render(r, out)
        faqs += nf
        if own and u:
            urls.append(u); own_n += 1

    by_host = {}
    for u in urls:
        by_host.setdefault(re.sub(r"^https://([^/]+)/.*", r"\1", u), []).append(u)
    for host, us in by_host.items():
        sm = ['<?xml version="1.0" encoding="UTF-8"?>',
              '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">']
        for u in sorted(us):
            sm.append(f"  <url><loc>{u}</loc><changefreq>monthly</changefreq>"
                      f"<priority>0.8</priority></url>")
        sm.append("</urlset>")
        (out / f"sitemap-{host}.xml").write_text("\n".join(sm), encoding="utf-8")
    return len(files), own_n, faqs, len(by_host), out


if __name__ == "__main__":
    n, own, faqs, hosts, out = build()
    print(f"{n} pages -> {out}")
    print(f"{own} own-site URLs across {hosts} sitemaps | {faqs} FAQ entities in structured data")
