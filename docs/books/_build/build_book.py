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
        # (part number, title, description, domain range) — a divider is emitted only where the
        # part's lowest-numbered EXISTING domain is found, so parts appear as authorship reaches them.
        "parts": [
            (1, "Part One", "Leading projects",
             "Domains 1–4 — the profession, strategy and selection, governance and decision rights, "
             "and delivery architecture: what a project leader is for and answerable for.", (1, 4)),
            (2, "Part Two", "Delivering the work",
             "Domains 5–10 — scope, planning and flow, cost and commercial, risk and resilience, "
             "quality and assurance, and procurement: the machinery of delivery.", (5, 10)),
            (3, "Part Three", "Leading people and organisations",
             "Domains 11–13 — stakeholders and influence, leadership and teams, and adaptive "
             "delivery: the work that is done through other people.", (11, 13)),
            (4, "Part Four", "Enterprise delivery and the digital future",
             "Domains 14–16 — digital delivery and responsible AI, programmes and portfolios, and "
             "transition and benefits realisation: delivery at enterprise scale.", (14, 16)),
        ],
    },
    "pfl-ai": {
        "title": "PFL-AI<br/>Body of Knowledge",
        "run_title": "PCI PFL-AI Body of Knowledge",
        "subtitle": ("The reference for the PCI AI Project Finance Leader<br/>"
                     "Project economics · structuring · financial mathematics · the governed use of AI"),
        "parts": [
            (1, "Part One", "Foundations",
             "Domains 1–4 — the profession, accounting foundations, financial mathematics and "
             "investment appraisal: the financial grammar of project finance leadership.", (1, 4)),
            (2, "Part Two", "Structuring and modelling",
             "Domains 5–9 — development and bankability, financial modelling, revenue and demand, "
             "cost and contingency, and funding structure: how a project becomes financeable.", (5, 9)),
            (3, "Part Three", "Executing the transaction",
             "Domains 10–13 — debt sizing and covenants, risk allocation, contracts and transaction "
             "structure, and due diligence to financial close: turning an appraised project into a "
             "funded one.", (10, 13)),
            (4, "Part Four", "Operating and the future",
             "Domains 14–16 — operations and asset management, refinancing and portfolio value, and "
             "the future of project finance: the life after close.", (14, 16)),
        ],
    },
}


def manuscript_order(book_dir: pathlib.Path) -> list:
    """Every manuscript/domain-NN-*.md, in domain-number order. Authorship adds a FILE; no list
    here needs editing, which is what lets domains be written concurrently."""
    files = []
    for f in (book_dir / "manuscript").glob("domain-*.md"):
        m = re.match(r"domain-(\d+)-", f.name)
        if m:
            files.append((int(m.group(1)), f))
    return [f for _, f in sorted(files, key=lambda x: (x[0], x[1].name))]


def domain_numbers(book_dir: pathlib.Path) -> list:
    return sorted(int(re.match(r"domain-(\d+)-", f.name).group(1))
                  for f in manuscript_order(book_dir))


FRONT = """
<div class="titlepage">
  <h1>{title}</h1>
  <div class="subtitle">{subtitle}</div>
  <div class="rule"></div>
  <div class="meta">FIRST EDITION — DRAFT FOR EDITORIAL AND TECHNICAL REVIEW<br/>
  NOT FOR RELEASE OR DISTRIBUTION<br/>
  PROJECT CONTROLS INSTITUTE GLOBAL<br/>
  <em>Finance intelligently. Control predictively. Deliver successfully.</em><br/>
  <em>AI proposes; the professional verifies, decides and remains accountable.</em></div>
</div>
<div class="frontmatter">
  <h2 style="page-break-before: always;">Status of this draft</h2>
  <p>This is a <strong>complete first draft</strong> of all {domains} domains of this volume, produced
  under the phase-gated programme recorded in <code>docs/books/</code>. It is <strong>not a released
  PCI publication</strong>: it carries no entitlement, syllabus or examination status, and the
  released edition will carry the full copyright, disclaimer and notices of the PCI book family.</p>
  <p><strong>What has been verified.</strong> Every number printed in this volume as a result — in
  worked examples, in-text calculations, multiple-choice options, exercise solutions and case studies
  — is recomputed independently with decimal arithmetic by the golden-answer suite
  (<code>_build/verify_formulas.py</code>), which must pass in full before any domain passes gate. All
  figures are PCI-original artwork generated from source in <code>_build/figures_src/</code>. No text,
  table, diagram, question or distinctive structure from any other publisher or certification body is
  reproduced; public standards are discussed and cited by name without reproducing their content. All
  organisations, projects and cases are fictitious.</p>
  <p><strong>What has not.</strong> This draft was <strong>AI-drafted and requires human editorial and
  technical review before release</strong>. It is not attributed to any named author or expert, and no
  claim of human authorship is made for it. Nothing in it should be presented as reviewed until that
  review is recorded. Where a professional judgement is stated rather than a measured effect, the text
  says so; readers should treat any unqualified generalisation as a drafting defect and report it.</p>
  <p><strong>Legal and jurisdictional note.</strong> Nothing here is legal, tax, accounting or
  investment advice. Treatments differ by jurisdiction and by reporting framework; specific matters
  must be referred to qualified professional advisers in the relevant jurisdiction.</p>
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
    files = manuscript_order(book_dir)
    print(f"manuscripts: {len(files)} — " + ", ".join(f.name for f in files))
    corpus = "\n\n".join(f.read_text(encoding="utf-8") for f in files)
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

    # Part divider before the first EXISTING chapter of each part.
    present = domain_numbers(book_dir)
    for pnum, num, title, desc, (lo, hi) in cfg["parts"]:
        in_part = [d for d in present if lo <= d <= hi]
        if not in_part:
            continue                      # part not yet reached by authorship
        kicker = f"Domain {in_part[0]}"
        kick_m = re.search(r'<div class="chapter"><div class="chapnum">\d+</div>'
                           r'<div class="chapkicker">' + re.escape(kicker) + r'</div>', html)
        if not kick_m:
            continue
        part_html = (f'<div class="partpage"><div class="partghost">{pnum:02d}</div>'
                     f'<div class="partnum">{num}</div><div class="parttitle">{title}</div>'
                     f'<div class="partdesc">{desc}</div><div class="partbar"></div></div>')
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

    front = FRONT.format(title=cfg["title"], subtitle=cfg["subtitle"], domains=len(files))
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
    out = pathlib.Path(sys.argv[2]) if len(sys.argv) > 2 else ROOT / book / "build" / f"{book}-bok-draft.pdf"
    build(book, out)
