#!/usr/bin/env python3
"""Premium book build for the PML-AI / PFL-AI Bodies of Knowledge.

Pandoc-free adaptation of the approved family pipeline (docs/bok/build/build_pdf.py):
Markdown corpus → HTML (python-markdown) → premium transforms (part/chapter/KA openers,
worked-example panels, figure injection, generated contents + index) → WeasyPrint → A4 PDF.

Usage:  python3 build_book.py pml-ai|pfl-ai [output.pdf]
"""
import pathlib
import re
import sys

import markdown

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parent

BOOKS = {
    "pml-ai": {
        "title": "PML-AI<br/>Body of Knowledge",
        "run_title": "PCI PML-AI Body of Knowledge",
        "subtitle": ("The reference for the PCI Project Management Leader – AI<br/>"
                     "Leadership · delivery systems · governance · the governed use of AI"),
        "order": ["manuscript/domain-01-profession.md",
                  "manuscript/domain-06-planning-scheduling-flow.md",
                  "manuscript/domain-07-cost-resources-commercial.md",
                  "manuscript/domain-08-risk-uncertainty-resilience.md"],
        "parts": [("Part One", "Leading projects",
                   "Domains 1–4 — the profession, strategy and selection, governance and decision rights, "
                   "and delivery architecture: what a project leader is for and answerable for.",
                   "Domain 1 —")],
    },
    "pfl-ai": {
        "title": "PFL-AI<br/>Body of Knowledge",
        "run_title": "PCI PFL-AI Body of Knowledge",
        "subtitle": ("The reference for the PCI AI Project Finance Leader<br/>"
                     "Project economics · structuring · financial mathematics · the governed use of AI"),
        "order": ["manuscript/domain-01-foundations.md",
                  "manuscript/domain-02-accounting-foundations.md",
                  "manuscript/domain-03-time-value-of-money.md",
                  "manuscript/domain-04-investment-appraisal.md"],
        "parts": [("Part One", "Foundations",
                   "Domains 1–4 — the profession, accounting foundations, financial mathematics and "
                   "investment appraisal: the financial grammar of project finance leadership.",
                   "Domain 1 —")],
    },
}

FRONT = """
<div class="titlepage">
  <h1>{title}</h1>
  <div class="subtitle">{subtitle}</div>
  <div class="rule"></div>
  <div class="meta">FIRST EDITION — PHASE 1 PROTOTYPE (NOT FOR RELEASE)<br/>
  PROJECT CONTROLS INSTITUTE GLOBAL<br/>
  <em>Finance intelligently. Control predictively. Deliver successfully.</em><br/>
  <em>AI proposes; the professional verifies, decides and remains accountable.</em></div>
</div>
<div class="frontmatter">
  <h2 style="page-break-before: always;">Prototype notice</h2>
  <p>This document is a <strong>Phase 1 production prototype</strong> of one domain, built to
  validate the book's pattern, calculation-verification harness, figures and typesetting pipeline
  before scaled authorship. It is not a released PCI publication and carries no entitlement,
  syllabus or examination status. The released edition will carry the full copyright, disclaimer
  and trademark notices of the PCI book family; all examples here are original and fictional, all
  calculations pass the golden-answer suite (<code>verify_formulas.py</code>), and no standard's
  text is reproduced.</p>
</div>
"""


def inject_figures(book_dir: pathlib.Path, corpus: str) -> str:
    out, injected = [], 0
    for line in corpus.split("\n"):
        m = re.match(r"> \*\*Fig (\d+\.\d+\.\d+) — ([^.*]+)", line)
        if m:
            fid, title = m.group(1), m.group(2).strip().rstrip(".")
            svgf = book_dir / "build" / "figures" / f"fig_{fid.replace('.', '_')}.svg"
            if svgf.exists():
                out.append(f'<figure><img src="{svgf.as_posix()}"/>'
                           f"<figcaption>Fig {fid} — {title}</figcaption></figure>\n")
                injected += 1
        out.append(line)
    print(f"figures injected: {injected}")
    return "\n".join(out)


def build(book: str, out: pathlib.Path) -> None:
    cfg = BOOKS[book]
    book_dir = ROOT / book
    corpus = "\n\n".join((book_dir / n).read_text(encoding="utf-8") for n in cfg["order"])
    corpus = inject_figures(book_dir, corpus)

    html = markdown.markdown(corpus, extensions=["tables", "fenced_code", "toc"],
                             extension_configs={"toc": {"anchorlink": False}})

    # Chapter openers.
    def chap(m):
        return (f'<div class="chapter"><div class="chapnum">{int(m.group(2)):02d}</div>'
                f'<div class="chapkicker">Domain {m.group(2)}</div>'
                f'<h1 id="{m.group(1)}">{m.group(3)}</h1>'
                f'<div class="chaprule"></div><div class="chaprule2"></div></div>')
    html = re.sub(r'<h1 id="([^"]+)">Domain\s+(\d+)\s+—\s+(.+?)</h1>', chap, html, flags=re.S)

    # KA openers.
    html = re.sub(r'<h2 id="([^"]+)">Knowledge\s+Area\s+(\d+\.\d+)\s+—\s+(.+?)</h2>',
                  r'<h2 class="ka" id="\1"><span class="kanum">Knowledge Area \2</span>'
                  r'<span class="katitle">\3</span></h2>', html, flags=re.S)

    # Apparatus mini-heads.
    html = re.sub(r'<h3 id="([^"]+)">((?:Key terms|Sample MCQs|Self-check)[^<]*)</h3>',
                  r'<h3 class="minihead" id="\1">\2</h3>', html)

    # Worked-example panels (heading para + following <ol>).
    wex = re.compile(r'<p><strong>Worked example ([^<]+)</strong>((?:(?!</p>).)*)</p>\s*'
                     r'<ol((?:(?!</ol>).)*)</ol>', re.S)
    n = len(wex.findall(html))
    html = wex.sub(r'<div class="wex"><p class="wexhead"><strong>Worked example \1</strong>\2</p>'
                   r'<ol\3</ol></div>', html)
    print(f"worked-example panels: {n}")

    # Exercises accent.
    html = re.sub(r'<p><strong>Exercise (\d+\.\d+)</strong>',
                  r'<p class="exhead"><strong>Exercise \1</strong>', html)

    # Figure-spec blockquotes hidden in print.
    html = re.sub(r'<blockquote>(\s*<p><strong>Fig\s)', r'<blockquote class="figspec">\1', html)

    # Part divider before its first chapter.
    for i, (num, title, desc, first_chap) in enumerate(cfg["parts"], start=1):
        kick_m = re.search(r'<div class="chapter"><div class="chapnum">\d+</div>'
                           r'<div class="chapkicker">' + re.escape(first_chap.split(" ")[0]) +
                           r' ' + re.escape(first_chap.split(" ")[1]), html)
        part_html = (f'<div class="partpage"><div class="partghost">{i:02d}</div>'
                     f'<div class="partnum">{num}</div><div class="parttitle">{title}</div>'
                     f'<div class="partdesc">{desc}</div><div class="partbar"></div></div>')
        if kick_m:
            html = html[:kick_m.start()] + part_html + html[kick_m.start():]

    # Contents (h1 + h2 headings, in order).
    toc_items = []
    for m in re.finditer(r'<h(1|2)(?: class="ka")? id="([^"]+)">(.*?)</h\1>', html, flags=re.S):
        text = re.sub(r"<[^>]+>", " ", m.group(3))
        text = re.sub(r"\s+", " ", text).strip()
        toc_items.append((m.group(1), m.group(2), text))
    toc = ['<nav id="TOC"><ul>']
    open_sub = False
    for lvl, hid, text in toc_items:
        if lvl == "1":
            if open_sub:
                toc.append("</ul></li>")
                open_sub = False
            toc.append(f'<li><a href="#{hid}">{text}</a><ul>')
            open_sub = True
        else:
            toc.append(f'<li><a href="#{hid}">{text}</a></li>')
    if open_sub:
        toc.append("</ul></li>")
    toc.append("</ul></nav>")

    # Index from key-terms tables.
    terms = {}
    for m in re.finditer(r'### Key terms — KA (\d+\.\d+)\n\n\| Term \| Meaning \|\n\|[-| ]+\|\n((?:\|.*\|\n)+)', corpus):
        ka, rows = m.group(1), m.group(2)
        for row in rows.strip().split("\n"):
            cells = [c.strip() for c in row.strip("|").split("|")]
            if len(cells) >= 2:
                term = re.sub(r"[`*]", "", cells[0]).strip()
                if term and term.lower() not in terms:
                    terms[term.lower()] = (term, ka)
    ka_ids = {m.group(2): m.group(1) for m in re.finditer(
        r'<h2 class="ka" id="([^"]+)"><span class="kanum">Knowledge Area (\d+\.\d+)</span>', html)}
    groups = {}
    for term, ka in sorted(terms.values(), key=lambda t: t[0].lower()):
        hid = ka_ids.get(ka)
        if hid:
            letter = term[0].upper() if term[0].isalpha() else "#"
            groups.setdefault(letter, []).append(f'<div class="ixe"><a href="#{hid}">{term}</a></div>')
    ix = ['<div class="bookindex"><div class="ixtitle">Index</div><div class="ixcols">']
    for letter in sorted(groups):
        ix.append(f'<div class="ixl">{letter}</div>' + "".join(groups[letter]))
    ix.append("</div></div>")
    print(f"index entries: {sum(len(v) for v in groups.values())}")

    front = FRONT.format(title=cfg["title"], subtitle=cfg["subtitle"])
    doc_html = ("<!doctype html><html><head><meta charset='utf-8'>"
                f"<style>html {{ string-set: booktitle \"{cfg['run_title']}\"; }}</style>"
                f"</head><body>{front}{''.join(toc)}{html}{''.join(ix)}</body></html>")
    html_file = book_dir / "build" / "_combined.html"
    html_file.parent.mkdir(parents=True, exist_ok=True)
    html_file.write_text(doc_html, encoding="utf-8")

    from weasyprint import CSS, HTML
    doc = HTML(filename=str(html_file)).render(stylesheets=[CSS(filename=str(HERE / "print.css"))])
    doc.write_pdf(str(out))
    print(f"OK {out}  pages={len(doc.pages)}")


if __name__ == "__main__":
    book = sys.argv[1]
    out = pathlib.Path(sys.argv[2]) if len(sys.argv) > 2 else ROOT / book / "build" / f"{book}-prototype.pdf"
    build(book, out)
