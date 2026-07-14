#!/usr/bin/env python3
"""Build the PCP-AI Body of Knowledge PDF from the markdown corpus.

Concatenates the book files in reading order, converts to HTML via pandoc,
styles with print.css (A4), renders with WeasyPrint, and reports the page count.

Usage:  python3 build_pdf.py [output.pdf]
"""
import subprocess, sys, datetime, pathlib

BOK = pathlib.Path(__file__).resolve().parent.parent
BUILD = BOK / "build"
OUT = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else BUILD / "pcp-ai-bok.pdf"

ORDER = [
    "00-style-spine.md",
    "domain-01-foundations-of-accounting.md",
    "domain-02-financial-reporting.md",
    "domain-03-budgeting-forecasting.md",
    "domain-04-performance-variance-reporting.md",
    "domain-05-cost-management.md",
    "domain-06-evm-eac.md",
    "domain-07-contracts-commercial.md",
    "domain-08-pm-lifecycle.md",
    "domain-09-agile-adaptive.md",
    "domain-10-scheduling.md",
    "domain-11-process-cycles.md",
    "domain-12-risk-management.md",
    "domain-13-ai-for-project-controls.md",
    "appendices.md",
]

TITLE_HTML = f"""
<div class="titlepage">
  <h1>PCP-AI<br/>Body of Knowledge</h1>
  <div class="subtitle">The reference for the Certified Project Controls Professional — AI<br/>
  Project controls · project finance · the governed use of AI</div>
  <div class="rule"></div>
  <div class="meta">FIRST EDITION — Version 1 (working draft, pending SME review)<br/>
  Built {datetime.date.today().isoformat()}<br/><br/>
  PROJECT CONTROLS INSTITUTE GLOBAL, INC.<br/>
  <em>AI proposes. The professional disposes.</em></div>
</div>

<div class="frontmatter">
  <h2 style="page-break-before: always;">Copyright &amp; edition notice</h2>
  <p>© {datetime.date.today().year} Project Controls Institute Global, Inc. All rights reserved. No part of this
  publication may be reproduced, stored in a retrieval system, or transmitted in any form or by any means without
  the prior written permission of the publisher, except for brief quotations in reviews or as permitted by law.</p>
  <p><strong>First Edition — working draft.</strong> This volume is a first authored draft pending subject-matter-expert
  (SME) review in finance, agile delivery and artificial intelligence, and editorial and legal review, before final
  publication. Content marked as pending review must not be treated as final certification content.</p>
  <p><strong>Disclaimer.</strong> This reference is an educational publication. It does not constitute accounting, legal,
  financial or professional advice, and it should not be relied upon as a substitute for advice from qualified
  professionals on specific matters. Standards and frameworks — including IFRS standards, the PMBOK Guide, the AACE
  Total Cost Management framework, ISO standards, the Agile Manifesto and the Scrum Guide — are referred to by name and
  described in this publication's own words; no standard's text is reproduced, and all trademarks remain the property
  of their respective owners. References to such frameworks do not imply endorsement by, or affiliation with, their
  publishers. No governmental approval or third-party accreditation of the PCP-AI credential is implied.</p>
  <p><strong>Original content.</strong> All examples, case studies, figures, templates and examination-style questions in
  this volume are original. Organisations, projects and figures appearing in examples and case studies are fictional
  and illustrative; any resemblance to actual organisations or projects is coincidental. Sample questions are study
  material and are maintained separately from any live examination bank.</p>
  <p><em>The governing principle of this Body of Knowledge: <strong>AI proposes, the professional disposes.</strong></em></p>

  <h2 style="page-break-before: always;">How to use this reference</h2>
  <p>The book is organised as <strong>13 domains</strong> in three groups — finance, accounting &amp; reporting
  (Domains 1–4, 40&nbsp;%), project management (Domains 5–12, 40&nbsp;%), and AI knowledge &amp; practical approach
  (Domain 13, 20&nbsp;%). Every page sits under a numbered <strong>Domain → Knowledge Area → Topic</strong> hierarchy
  (e.g. 6.3.2), and cross-references use those numbers throughout. The <strong>Style Spine</strong> (the first
  chapter) defines the shared symbols, formats and conventions every domain binds to.</p>
  <p>Each domain follows one shape. The <strong>knowledge areas</strong> build the discipline topic by topic, each with
  worked examples in a five-step format (Setup → Formula → Substitution → Result → Interpretation), key terms, sample
  MCQs with rationales, and self-checks. <strong>Advanced topics</strong> extend the domain for practitioners who lead
  the function. Two <strong>sector case studies</strong> apply the whole domain to realistic projects. The
  <strong>executive perspective</strong> distils what a director cannot delegate. <strong>Calculation exercises</strong>
  (quantitative domains) provide multi-step practice with full solutions. The <strong>practitioner's toolkit</strong>
  offers adoption-ready templates and checklists, and <strong>exam preparation</strong> closes each domain with its
  known calculation traps and reflection questions. The appendices consolidate the master formula sheet, glossary,
  standards index, figure index and the sample-MCQ bank.</p>
  <p>For study, work a domain end to end and attempt every worked example before reading its solution. For practice,
  go straight to the toolkits and case studies. For examination preparation, use the exam-preparation sections, the
  MCQ bank and the calculation exercises — and note that these are study materials, kept separate from the live
  examination bank.</p>
</div>
"""

def inject_figures(corpus: str) -> str:
    """Insert each rendered SVG immediately before its figure-spec blockquote.

    Spec lines look like:  > **Fig 6.3.1 — The EAC fan.** ...
    The spec block remains beneath the image as its extended caption.
    """
    import re
    out, injected = [], 0
    for line in corpus.split("\n"):
        m = re.match(r"> \*\*Fig (\d+\.\d+\.\d+) — ([^.*]+)", line)
        if m:
            fid, title = m.group(1), m.group(2).strip().rstrip(".")
            svg = BUILD / "figures" / f"fig_{fid.replace('.', '_')}.svg"
            if svg.exists():
                out.append(f'<figure><img src="figures/{svg.name}"/>'
                           f"<figcaption>Fig {fid} — {title}</figcaption></figure>\n")
                injected += 1
        out.append(line)
    print(f"figures injected: {injected}")
    return "\n".join(out)


def main() -> None:
    # 1. Concatenate the corpus in reading order; inject rendered figures.
    corpus = "\n\n".join((BOK / name).read_text(encoding="utf-8") for name in ORDER)
    corpus = inject_figures(corpus)
    combined = BUILD / "_combined.md"
    combined.write_text(corpus, encoding="utf-8")

    # 2. Markdown -> HTML body via pandoc (with a generated table of contents).
    html_body = subprocess.run(
        ["pandoc", str(combined), "-f", "gfm", "-t", "html", "--toc", "--toc-depth=2", "-s",
         "--metadata", "title=PCP-AI Body of Knowledge"],
        capture_output=True, text=True, check=True).stdout
    # Inject the title page right after <body> and drop pandoc's default header block.
    html_body = html_body.replace("<body>", "<body>" + TITLE_HTML, 1)
    html_body = html_body.replace('<header id="title-block-header">', '<header id="title-block-header" style="display:none">', 1)

    # Premium chapter openers: rewrite each "Domain N — Title" h1 into a styled opener (id kept for TOC links).
    import re as _re
    def chap(m):
        return (f'<div class="chapter"><div class="chapnum">{int(m.group(2)):02d}</div>'
                f'<div class="chapkicker">Domain {m.group(2)}</div>'
                f'<h1 id="{m.group(1)}">{m.group(3)}</h1>'
                f'<div class="chaprule"></div><div class="chaprule2"></div></div>')
    html_body = _re.sub(r'<h1\s+id="([^"]+)">Domain\s+(\d+)\s+—\s+(.+?)</h1>', chap, html_body, flags=_re.S)

    # Mark figure-spec blockquotes so only they carry the FIGURE SPECIFICATION label.
    html_body = _re.sub(r'<blockquote>(\s*<p><strong>Fig\s)', r'<blockquote class="figspec">\1', html_body)

    # Premium KA openers: split "Knowledge Area N.N — Title" h2s into kicker + title lines.
    html_body = _re.sub(
        r'<h2\s+id="([^"]+)">Knowledge\s+Area\s+(\d+\.\d+)\s+—\s+(.+?)</h2>',
        r'<h2 class="ka" id="\1"><span class="kanum">Knowledge Area \2</span>'
        r'<span class="katitle">\3</span></h2>',
        html_body, flags=_re.S)

    # Recurring apparatus headings become small-cap mini-heads.
    html_body = _re.sub(
        r'<h3\s+id="([^"]+)">((?:Key terms|Sample MCQs|Self-check)[^<]*)</h3>',
        r'<h3 class="minihead" id="\1">\2</h3>', html_body)

    # Worked examples: wrap the heading paragraph plus its numbered steps in a labelled panel.
    wex_pat = _re.compile(
        r'<p><strong>Worked example ([^<]+)</strong>((?:(?!</p>).)*)</p>\s*'
        r'<ol((?:(?!</ol>).)*)</ol>', _re.S)
    n_wex = len(wex_pat.findall(html_body))
    html_body = wex_pat.sub(
        r'<div class="wex"><p class="wexhead"><strong>Worked example \1</strong>\2</p>'
        r'<ol\3</ol></div>', html_body)
    print(f"worked-example panels: {n_wex}")

    # Exercises: accent the problem statement.
    html_body = _re.sub(r'<p><strong>Exercise (\d+\.\d+)</strong>',
                        r'<p class="exhead"><strong>Exercise \1</strong>', html_body)

    # Part dividers before Domains 1, 5 and 13.
    PARTS = [
        (1, "Part One", "Finance, Accounting & Reporting",
         "Domains 1–4 — the accounting model, the standards (IFRS 15 at their heart), budgeting and "
         "forecasting, and performance measurement: the financial grammar of project controls. Forty per "
         "cent of the Body of Knowledge."),
        (5, "Part Two", "Project Management",
         "Domains 5–12 — cost management, earned value, contracts and commercial management, the lifecycle, "
         "agile and adaptive delivery, scheduling, business process cycles and risk: the delivery disciplines "
         "controls serves. Forty per cent of the Body of Knowledge."),
        (13, "Part Three", "AI Knowledge & Practical Approach",
         "Domain 13 — concepts, data, prompting, tools, applied workflows, governance and capability: the "
         "governed use of artificial intelligence across the whole controls lifecycle. Twenty per cent of the "
         "Body of Knowledge, under one principle: AI proposes, the professional disposes."),
    ]
    for i, (dom, num, title, desc) in enumerate(PARTS, start=1):
        kick = f'<div class="chapter"><div class="chapnum">{dom:02d}</div>'
        part_html = (f'<div class="partpage"><div class="partghost">{i:02d}</div>'
                     f'<div class="partnum">{num}</div>'
                     f'<div class="parttitle">{title}</div><div class="partdesc">{desc}</div>'
                     f'<div class="partbar"></div></div>')
        html_body = html_body.replace(kick, part_html + kick, 1)
    html_file = BUILD / "_combined.html"
    html_file.write_text(html_body, encoding="utf-8")

    # Alphabetical index: every key term, linked to its KA section, page number resolved at layout.
    def build_index(html: str, source_md: str) -> str:
        terms = {}
        for m in _re.finditer(r'### Key terms — KA (\d+\.\d+)\n\n\| Term \| Meaning \|\n\|[-| ]+\|\n((?:\|.*\|\n)+)', source_md):
            ka, rows = m.group(1), m.group(2)
            for row in rows.strip().split('\n'):
                cells = [c.strip() for c in row.strip('|').split('|')]
                if len(cells) >= 2:
                    term = _re.sub(r'[`*]', '', cells[0]).strip()
                    if term and term.lower() not in terms:
                        terms[term.lower()] = (term, ka)
        ka_ids = {m.group(2): m.group(1) for m in _re.finditer(
            r'<h2 class="ka" id="([^"]+)"><span class="kanum">Knowledge Area (\d+\.\d+)</span>', html)}
        groups: dict[str, list] = {}
        for term, ka in sorted(terms.values(), key=lambda t: t[0].lower()):
            hid = ka_ids.get(ka)
            if not hid:
                continue
            letter = term[0].upper() if term[0].isalpha() else '#'
            groups.setdefault(letter, []).append(f'<div class="ixe"><a href="#{hid}">{term}</a></div>')
        parts = ['<div class="bookindex"><div class="ixtitle">Index</div><div class="ixcols">']
        for letter in sorted(groups):
            parts.append(f'<div class="ixl">{letter}</div>' + ''.join(groups[letter]))
        parts.append('</div></div>')
        print(f"index entries: {sum(len(v) for v in groups.values())}")
        return ''.join(parts)

    html_body = html_body.replace('</body>', build_index(html_body, corpus) + '</body>', 1)
    html_file.write_text(html_body, encoding="utf-8")

    # 3. HTML -> PDF via WeasyPrint.
    from weasyprint import HTML, CSS  # imported late so --help stays fast
    doc = HTML(filename=str(html_file)).render(stylesheets=[CSS(filename=str(BUILD / "print.css"))])
    doc.write_pdf(str(OUT))

    # 4. Prepend the full-bleed book cover as a native PDF page (exact A4, no margins).
    cover = next((p for p in (BUILD / "cover.jpg", BUILD / "cover.png") if p.exists()), BUILD / "cover.png")
    pages = len(doc.pages)
    if cover.exists():
        import fitz  # PyMuPDF
        pdf = fitz.open(str(OUT))
        page = pdf.new_page(pno=0, width=595.276, height=841.890)  # A4 in points
        page.insert_image(page.rect, filename=str(cover))
        tmp = OUT.with_suffix(".tmp.pdf")
        pdf.save(str(tmp)); pdf.close()
        tmp.replace(OUT)
        pages += 1
    print(f"OK {OUT}  pages={pages}")

if __name__ == "__main__":
    main()
