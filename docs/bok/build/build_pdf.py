#!/usr/bin/env python3
"""Build the PCL-AI Body of Knowledge PDF from the markdown corpus.

Concatenates the book files in reading order, converts to HTML via pandoc, applies the family's
house-style transforms, styles with print.css (which imports the shared publication stylesheet),
renders with WeasyPrint, and reports the page count.

PCL-AI is built by this script and PML-AI/PFL-AI by ../../books/_build/build_book.py, but the two
pipelines now emit the SAME markup and load the SAME stylesheet — the cover, the openers, the
tables, the call-outs, the running heads and the folios are one design, not two that resemble
each other.

Usage:  python3 build_pdf.py [output.pdf]
"""
import importlib.util
import subprocess, sys, datetime, pathlib, re

BOK = pathlib.Path(__file__).resolve().parent.parent
BUILD = BOK / "build"
OUT = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else BUILD / "pcl-ai-bok.pdf"

# The family pipeline. Imported by path rather than duplicated, so the cover artwork, the icon
# sweep, the numeric-column tagging and the PCI Standards rendering are literally the same code
# the other two Bodies of Knowledge run.
_FAMILY = BOK.parent / "books" / "_build" / "build_book.py"
_spec = importlib.util.spec_from_file_location("pci_book_family", _FAMILY)
family = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(family)

RUN_TITLE = "PCI PCL-AI Body of Knowledge"

COVER_HTML = family.cover_html(
    code="PCL-AI",
    credential="The reference for the PCI AI Project Controls Leader",
    themes="Project controls · project finance · the governed use of AI",
    chart=family.CHARTS["variance"]())

ORDER = [
    "00-conventions.md",
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
  <h1>PCL-AI<br/>Body of Knowledge</h1>
  <div class="subtitle">The reference for the PCI AI Project Controls Leader<br/>
  Project controls · project finance · the governed use of AI</div>
  <div class="rule"></div>
  <div class="meta">FIRST EDITION<br/>
  {datetime.date.today().year}<br/><br/>
  PROJECT CONTROLS INSTITUTE GLOBAL<br/>
  <em>AI proposes; the professional verifies, decides and remains accountable.</em></div>
</div>

<div class="frontmatter">
  <h2 style="page-break-before: always;">Copyright &amp; edition notice</h2>
  <p>© {datetime.date.today().year} Project Controls Institute Global. All rights reserved. No part of this
  publication may be reproduced, stored in a retrieval system, or transmitted in any form or by any means without
  the prior written permission of the publisher, except for brief quotations in reviews or as permitted by law.</p>
  <p><strong>First Edition.</strong> The Body of Knowledge is maintained under a continuous review programme;
  corrections and refinements identified through technical and editorial review are incorporated in subsequent
  printings, and the current edition supersedes all earlier printings.</p>
  <p><strong>Former name.</strong> This credential and its Body of Knowledge were previously designated
  PCP-AI (Certified Project Controls Professional — AI). The designation was retired and replaced by
  PCL-AI (PCI AI Project Controls Leader); the former name is recorded here for credential history only
  and is not an active credential.</p>
  <p><strong>Disclaimer.</strong> This reference is an educational publication. It does not constitute accounting, tax, legal,
  financial or other professional advice, and it should not be relied upon as a substitute for advice from qualified
  professionals on specific matters. Tax rates, contract-law rules and regulatory requirements vary by jurisdiction and
  over time; all rates and legal rules appearing in examples are illustrative only. Standards and frameworks — including IFRS standards, the PMBOK Guide, the AACE
  Total Cost Management framework, ISO standards, the Agile Manifesto and the Scrum Guide — are referred to by name and
  described in this publication's own words; no standard's text is reproduced, and all trademarks remain the property of their
  respective owners — including PMBOK, PMI and PMP (Project Management Institute, Inc.), IFRS and IAS (IFRS
  Foundation), AACE (AACE International), FIDIC (Fédération Internationale des Ingénieurs-Conseils), the
  Scrum Guide (its authors), and SAFe (Scaled Agile, Inc.). References to such frameworks do not imply endorsement by, or affiliation with, their
  publishers. No governmental approval or third-party accreditation of the PCL-AI credential is implied.</p>
  <p><strong>Original content.</strong> All examples, case studies, figures, templates and examination-style questions in
  this volume are original. Organisations, projects and figures appearing in examples and case studies are fictional
  and illustrative; any resemblance to actual organisations or projects is coincidental. Sample questions are study
  material and are maintained separately from any live examination bank.</p>
  <p><em>The governing principle of this Body of Knowledge: <strong>AI proposes; the professional verifies, decides and remains accountable.</strong></em></p>

  <h2 style="page-break-before: always;">How to use this reference</h2>
  <p>The book is organised as <strong>13 domains</strong> in three groups — finance, accounting &amp; reporting
  (Domains 1–4, 40&nbsp;%), project management (Domains 5–12, 40&nbsp;%), and AI knowledge &amp; practical approach
  (Domain 13, 20&nbsp;%). Every page sits under a numbered <strong>Domain → Knowledge Area → Topic</strong> hierarchy
  (e.g. 6.3.2), and cross-references use those numbers throughout. <strong>Conventions of This Reference</strong>
  (the first chapter) sets out the shared symbols, formats and conventions used from the first page to the last.</p>
  <p>Each domain follows one shape. The <strong>knowledge areas</strong> build the discipline topic by topic, each with
  worked examples in a five-step format (Setup → Formula → Substitution → Result → Interpretation), key terms, sample
  MCQs with rationales, and self-checks. <strong>Advanced topics</strong> extend the domain for practitioners who lead
  the function. Two <strong>sector case studies</strong> apply the whole domain to realistic projects. The
  <strong>executive perspective</strong> distils what a director cannot delegate. <strong>Calculation exercises</strong>
  (quantitative domains) provide multi-step practice with full solutions. The <strong>practitioner's toolkit</strong>
  offers adoption-ready templates and checklists, and <strong>exam preparation</strong> closes each domain with its
  known calculation traps and reflection questions. The appendices carry the master formula sheet, the global
  glossary, the standards and figure indexes, an answer key to every self-check question in the book, the full
  bank of all 309 sample MCQs with their answers, and an integrated capstone that runs one project through all
  thirteen domains.</p>
  <p>For study, work a domain end to end and attempt every worked example before reading its solution. For practice,
  go straight to the toolkits and case studies. For examination preparation, use the exam-preparation sections, the
  MCQ bank and the calculation exercises — and note that these are study materials, kept separate from any live
  examination bank.</p>

  <h2 style="page-break-before: always;">How this volume was produced</h2>
  <p><strong>Drafting and human review.</strong> This volume was drafted with substantial assistance from
  artificial intelligence, working to the Institute's authoring standard. It is attributed to no named author or
  expert, and no claim of individual human authorship is made for it. Independent editorial and technical review
  by named subject-matter experts is a separate step, and the status of that review for this edition is recorded
  in the release approval statement below. Nothing in this volume should be described to a candidate, an employer,
  a regulator or an accreditation body as expert-reviewed unless that review is recorded. Where the text states a
  professional judgement rather than a measured effect, it says so; readers should treat any unqualified
  generalisation as a drafting defect and report it through the corrections process below.</p>
  <p><strong>Calculation verification.</strong> Numbers printed as results in this volume — in worked examples,
  in-text calculations, multiple-choice options, exercise solutions and case studies — have been recomputed from
  their stated inputs and checked against the printed value. Verification of this kind establishes that the
  arithmetic is right and that the numbers printed are the numbers the stated methods produce. It does not
  establish that the method chosen is the one a competent practitioner would choose, that an assumption is
  reasonable, or that the interpretation drawn from a correct number is sound. Those are matters for the human
  review described above. The Institute publishes a calculation assurance statement recording the coverage,
  method and any outstanding limits for each edition.</p>
  <p><strong>Governing principle.</strong> The principle that governs both the production of this volume and the
  practice it teaches is the same: <strong>AI proposes; the professional verifies, decides and remains
  accountable.</strong></p>

  <h2 style="page-break-before: always;">PCI Standards, and how to read the call-out boxes</h2>
  <p>A <strong>PCI Standard</strong> is a mandatory professional rule established by Project Controls
  Institute Global for the ethical, competent, verifiable and accountable performance of work within a PCI
  certification scope. Laws are numbered with a stable identifier (for example <code>PCL-LAW-06-03</code>) and are
  cited by that identifier rather than by page number.</p>
  <p><strong>PCI Standards are professional certification rules and standards of conduct established by
  PCI Global. They are not legislation, regulatory requirements or substitutes for applicable law, contractual
  obligations or authoritative professional standards.</strong> Where any applicable law, regulation, contract or
  authoritative professional standard imposes a stricter requirement, that requirement governs. Consequences of
  breach are limited to matters within PCI's own authority: its examination, certification, quality and conduct
  processes.</p>
  <p>Four kinds of call-out appear in this volume. Each is identified by a written label and a border
  treatment as well as by colour, so that nothing depends on colour alone — the distinctions survive
  greyscale printing, screen reading and monochrome displays. There are no icons and no section signs:
  a marker that renders as a blank box in one reader is worse than no marker, and the written label
  beside the identifier already says everything such a glyph could.</p>
  <table>
    <thead><tr><th>Call-out</th><th>Label</th><th>Rule and ground</th><th>What it means</th></tr></thead>
    <tbody>
      <tr><td>PCI Standard</td><td><strong>PCI Standard</strong>, then the identifier</td>
          <td>Solid green left rule, green tint</td>
          <td>Mandatory. A professional rule PCI holds credential holders and candidates to.</td></tr>
      <tr><td>External reference</td><td><strong>External standard or framework</strong></td>
          <td>Double green left rule, no ground</td>
          <td>A named external standard or framework, described in this book's own words. The official publication governs.</td></tr>
      <tr><td>Practice guidance</td><td><strong>PCI recommended practice</strong></td>
          <td>Dashed green left rule, no ground</td>
          <td>Recommended, not mandatory. Departures need a recorded reason.</td></tr>
      <tr><td>Caution</td><td><strong>Caution</strong></td><td>Dotted amber left rule, amber tint</td>
          <td>A jurisdictional, legal, tax or accounting limitation, or a high-stakes limit on the use of AI.</td></tr>
    </tbody>
  </table>
  <p>The distinction the labels carry is deliberate and load-bearing: a <em>mandatory PCI law</em>, a
  <em>recommended PCI practice</em>, an <em>external requirement</em> and a <em>jurisdictional requirement</em>
  are four different obligations, and this volume never presents one as another. In particular, a voluntary
  framework is never described as legislation.</p>

  <h2 style="page-break-before: always;">Corrections, editions and release approval</h2>
  <p><strong>Corrections and errata.</strong> The Institute maintains an errata record for each edition. Readers
  who identify a technical error, an unsupported generalisation, a mischaracterised standard or a calculation
  that does not reproduce are asked to report it, quoting the stable identifier (domain, Knowledge Area and topic
  number, or the law, figure or worked-example identifier) rather than the page number. Confirmed corrections are
  published to the errata record and incorporated at the next printing.</p>
  <p><strong>Contact for technical corrections.</strong> Reports should be sent to the Institute at the address
  published on its website for Body of Knowledge corrections. The report should state what is printed, what the
  reader believes is correct, and the basis for that belief.</p>
  <p><strong>Edition control.</strong> Each edition carries an edition number and year. The current edition
  supersedes all earlier printings. Where a correction changes a number, a method or a stated obligation, the
  change is recorded in the errata record rather than made silently. Where an external standard cited in this
  volume is superseded, the affected topics are reviewed at the next edition; readers should verify current
  requirements with the issuing body in the meantime.</p>
  <p><strong>Release approval.</strong> This edition is published under the Institute's editorial authority.
  A public release approval statement, naming the human reviewers who approved the technical content, the
  editorial content, the laws and the calculations, and recording any qualification on that approval, is
  maintained with the edition record. Where that statement records outstanding review, this volume is study
  material published in good faith and not a reviewed professional standard.</p>
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


def laws_back_matter() -> str:
    """The PCI Standards part, rendered exactly as PML-AI and PFL-AI render it.

    The foundational set binds all three credentials; the PCL-AI set binds this one. Both live in
    the programme's shared standards directory, and both are rendered by the family pipeline into
    labelled call-out boxes with their identifiers — not, as before, into undifferentiated pandoc
    headings that made the same standard look like a different thing in each volume. A missing
    file is skipped rather than fatal, so the book still builds before the set is approved.
    """
    return family.render_laws("pcl-ai", {"laws_file": "PCL_AI_STANDARDS.md"})


def standards_toc(laws_html: str) -> str:
    """Contents lines for the standards part, in pandoc's own TOC markup."""
    if not laws_html:
        return ""
    items = ['<li><a href="#pci-professional-laws">PCI Standards</a><ul>']
    for m in re.finditer(r'<h[23] class="(stdset|stdgroup)" id="([^"]+)"[^>]*>(.*?)</h[23]>',
                         laws_html, re.S):
        text = re.sub(r"\s+", " ", re.sub(r"<[^>]+>", " ", m.group(3))).strip()
        items.append(f'<li><a href="#{m.group(2)}">{text}</a></li>')
    items.append("</ul></li>")
    return "".join(items)


def main() -> None:
    # 1. Concatenate the corpus in reading order; inject rendered figures.
    corpus = "\n\n".join((BOK / name).read_text(encoding="utf-8") for name in ORDER)
    corpus = inject_figures(corpus)
    combined = BUILD / "_combined.md"
    combined.write_text(corpus, encoding="utf-8")

    # 2. Markdown -> HTML body via pandoc (with a generated table of contents).
    html_body = subprocess.run(
        ["pandoc", str(combined), "-f", "gfm", "-t", "html", "--toc", "--toc-depth=2", "-s",
         "--metadata", "title=PCL-AI Body of Knowledge"],
        capture_output=True, text=True, check=True).stdout
    # Inject the title page right after <body> and drop pandoc's default header block.
    # pandoc emits lang="" — with no language WeasyPrint cannot hyphenate, and an unhyphenated
    # justified measure is a page of rivers.
    html_body = html_body.replace('lang="" xml:lang=""', f'lang="{family.LANG}" xml:lang="{family.LANG}"', 1)
    html_body = html_body.replace(
        "</head>",
        f'<style>html {{ string-set: booktitle "{RUN_TITLE}"; }}</style></head>', 1)
    html_body = html_body.replace("<body>", "<body>" + COVER_HTML + TITLE_HTML, 1)
    html_body = html_body.replace('<header id="title-block-header">', '<header id="title-block-header" style="display:none">', 1)

    # Premium chapter openers: rewrite each "Domain N — Title" h1 into a styled opener (id kept for TOC links).
    import re as _re
    def chap(m):
        return (f'<div class="chapter"><div class="chapkicker">Domain</div>'
                f'<div class="chapnum">{int(m.group(2))}</div>'
                f'<h1 id="{m.group(1)}">{m.group(3)}</h1>'
                f'<div class="chaprule"></div></div>')
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
         "Body of Knowledge, under one principle: AI proposes; the professional verifies, decides and remains accountable."),
    ]
    for i, (dom, num, title, desc) in enumerate(PARTS, start=1):
        kick = (f'<div class="chapter"><div class="chapkicker">Domain</div>'
                f'<div class="chapnum">{dom}</div>')
        part_html = (f'<div class="partpage">'
                     f'<div class="partnum">{num}</div>'
                     f'<div class="parttitle">{title}</div><div class="partdesc">{desc}</div></div>')
        html_body = html_body.replace(kick, part_html + kick, 1)

    # The PCI Standards part, in the family's call-out boxes, with its own contents lines.
    laws_html = laws_back_matter()
    if laws_html:
        html_body = html_body.replace("</body>", laws_html + "</body>", 1)
        toc_close = html_body.rfind("</ul>\n</nav>")
        if toc_close == -1:
            toc_close = html_body.rfind("</ul></nav>")
        if toc_close != -1:
            html_body = (html_body[:toc_close] + standards_toc(laws_html)
                         + html_body[toc_close:])
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
            r'<h2 class="ka" id="([^"]+)"[^>]*><span class="kanum">Knowledge Area (\d+\.\d+)</span>',
            html)}
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

    # The house passes: pictographic markers out, numeric columns right-aligned on tabular figures.
    html_body = family.house_style(html_body, "pcl-ai")
    html_file.write_text(html_body, encoding="utf-8")

    # 3. HTML -> PDF via WeasyPrint. The cover is set in the document itself, in the family's
    # typographic design — not prepended as a bitmap. A stock-photograph jacket carrying icons and
    # the retired PCP-AI designation is not this book's cover.
    from weasyprint import HTML, CSS  # imported late so --help stays fast
    doc = HTML(filename=str(html_file)).render(stylesheets=[CSS(filename=str(BUILD / "print.css"))])
    doc.write_pdf(str(OUT))
    print(f"OK {OUT}  pages={len(doc.pages)}")

if __name__ == "__main__":
    main()
