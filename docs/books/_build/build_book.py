#!/usr/bin/env python3
"""Premium book build for the PML-AI / PFL-AI Bodies of Knowledge.

Pandoc-free adaptation of the approved family pipeline (docs/bok/build/build_pdf.py):
Markdown corpus → HTML (python-markdown) → premium transforms (part/chapter/KA openers,
worked-example panels, figure injection, generated contents + index) → WeasyPrint → A4 PDF.

Usage:  python3 build_book.py pml-ai|pfl-ai [output.pdf]
"""
import math
import pathlib
import re
import sys

import markdown

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parent
STANDARDS_DIR = ROOT / "laws"

# The one stylesheet for the whole publication family (docs/books/_build/DESIGN_SYSTEM.md).
# The Standards volume and the programme reports read it and append only their own cover, and
# docs/bok/build/print.css imports it, so the four volumes cannot drift apart again.
HOUSE_CSS = HERE / "print.css"

# Every page is set in British English: hyphenation is off without a language, and a justified
# measure without hyphenation is a page full of rivers.
LANG = "en-GB"


# =================================================================================================
# Shared house-style transforms. Imported by the other builders in the family — keep them
# side-effect free.
# =================================================================================================

# Pictographic markers are forbidden in a published page (DESIGN_SYSTEM.md §5, §7). A tick, a
# hexagon or a warning triangle is a glyph-availability liability — it renders as a blank box in
# whichever environment lacks it, which is worse than no marker — and none of them says anything
# the written label beside it does not. Swept at render time because the corpus is owned elsewhere.
# Arrows, mathematical operators and box-drawing characters are NOT pictographs: they carry
# meaning in formulas, process chains and ASCII diagrams, and they stay.
_ICONS = (
    "✅❌✔✖✘✓✗☑☒☐"   # ticks, crosses, ballot boxes
    "⬛⬜⬢⬣✦✧⚠⛔⭐"          # blocks, hexagons, warning, star
    "■▪●○▶◆❖"                      # geometric markers
    "️︎"                                                    # emoji variation selectors
)
_ICON_RE = re.compile("[" + _ICONS + "]|[\U0001F000-\U0001FAFF]")
# A section sign in front of an identifier that is already displayed adds nothing; the ones that
# remain are genuine prose cross-references ("Charter §5") and are left alone.
_MARKER_SECTION_RE = re.compile(r"[·|]?\s*§(?=\s*(?:<|$))|§\s*(?=(?:PCI\s+STANDARD|EXTERNAL|CAUTION)\b)")


_PRE_RE = re.compile(r"<pre\b.*?</pre>", re.S)


def sweep_icons(html: str, label: str = "") -> str:
    """Strip pictographic markers and orphaned call-out section signs from rendered HTML.

    Preformatted blocks are swept for glyphs but never re-spaced: their whitespace is the
    diagram.
    """
    n = len(_ICON_RE.findall(html)) + len(_MARKER_SECTION_RE.findall(html))

    def tidy(chunk: str) -> str:
        chunk = _MARKER_SECTION_RE.sub("", chunk)
        chunk = _ICON_RE.sub("", chunk)
        # Tidy what the removal leaves behind: doubled spaces, a space before punctuation, and a
        # separator dot left dangling at the end of a cell or a sentence.
        chunk = re.sub(r"[ \t]{2,}", " ", chunk)
        chunk = re.sub(r" +([.,;:)\]])", r"\1", chunk)
        chunk = re.sub(r"\s*[·|]\s*(?=</)", "", chunk)
        chunk = re.sub(r"\(\s*\)", "", chunk)
        chunk = re.sub(r"[ \t]+(</(?:p|td|th|li|div|span|strong|em|h[1-6])>)", r"\1", chunk)
        chunk = re.sub(r"(<(?:p|td|th|li)(?:\s[^>]*)?>)[ \t]+", r"\1", chunk)
        return chunk

    out, at = [], 0
    for m in _PRE_RE.finditer(html):
        out.append(tidy(html[at:m.start()]))
        out.append(_ICON_RE.sub("", m.group(0)))
        at = m.end()
    out.append(tidy(html[at:]))
    if label:
        print(f"pictographic markers removed [{label}]: {n}")
    return "".join(out)


_TAG_RE = re.compile(r"<[^>]+>")
_CELL_RE = re.compile(r"<(t[dh])(\s[^>]*)?>(.*?)</\1>", re.S)
_ROW_RE = re.compile(r"<tr(?:\s[^>]*)?>(.*?)</tr>", re.S)


def _cell_text(html: str) -> str:
    t = _TAG_RE.sub("", html)
    return re.sub(r"&[a-z]+;|&#\d+;", " ", t).strip()


def _is_numeric(text: str) -> bool:
    """A cell reads as a number when digits carry it and at most a unit or two rides along."""
    if not text:
        return False
    digits = sum(c.isdigit() for c in text)
    letters = sum(c.isalpha() for c in text)
    return digits > 0 and letters <= 3 and digits >= letters


def tag_table_columns(html: str, label: str = "") -> str:
    """Right-align numeric columns on tabular figures, and spare all-caps headers small caps.

    Real small caps need lowercase input: a header already set in capitals would be rendered by
    the `smcp` feature as full capitals at header size, which is not what the design asks for.
    Those headers are tagged `nosc` and set in lining figures instead.
    """
    numeric_cols = 0

    def do_table(tm):
        nonlocal numeric_cols
        table = tm.group(0)
        rows = _ROW_RE.findall(table)
        if len(rows) < 2:
            return table
        body = []
        for r in rows:
            cells = _CELL_RE.findall(r)
            if cells and all(c[0] == "td" for c in cells):
                body.append([_cell_text(c[2]) for c in cells])
        if len(body) < 2:
            return table
        width = max(len(r) for r in body)
        numeric = set()
        for col in range(width):
            vals = [r[col] for r in body if col < len(r) and r[col]]
            if len(vals) >= 2 and sum(_is_numeric(v) for v in vals) >= 0.7 * len(vals):
                numeric.add(col)
        if numeric:
            numeric_cols += len(numeric)

        def do_row(rm):
            i = [0]

            def do_cell(cm):
                tag, attrs, inner = cm.group(1), cm.group(2) or "", cm.group(3)
                col = i[0]
                i[0] += 1
                classes = []
                if col in numeric:
                    classes.append("num")
                # An identifier cell stays whole. Bounded to short strings, so a long path in a
                # cell still wraps and can never push a table past the measure.
                txt_c = _cell_text(inner)
                if "<code>" in inner and 2 < len(txt_c) <= 22 and " " not in txt_c:
                    classes.append("id")
                if tag == "th":
                    txt = _cell_text(inner)
                    if len(txt) > 3 and txt == txt.upper() and any(c.isalpha() for c in txt):
                        classes.append("nosc")
                if not classes:
                    return cm.group(0)
                if 'class="' in attrs:
                    attrs = attrs.replace('class="', 'class="' + " ".join(classes) + " ", 1)
                else:
                    attrs += f' class="{" ".join(classes)}"'
                return f"<{tag}{attrs}>{inner}</{tag}>"

            return "<tr" + (rm.group(0)[3:rm.group(0).index(">")]) + ">" + \
                _CELL_RE.sub(do_cell, rm.group(1)) + "</tr>"

        return _ROW_RE.sub(do_row, table)

    out = re.sub(r"<table(?:\s[^>]*)?>.*?</table>", do_table, html, flags=re.S)
    if label:
        print(f"numeric table columns aligned [{label}]: {numeric_cols}")
    return out


def strip_pandoc_default_css(html: str) -> str:
    """Remove the stylesheet pandoc's standalone template injects into <head>.

    It sets `body { max-width: 36em; padding: 50px }`, a print-media `font-size: 12pt`, its own
    paragraph margins and its own table rules. Because it lives in the document it wins over the
    stylesheet passed to WeasyPrint for every property that stylesheet does not explicitly set,
    which is how a designed page ends up as a 36-em column adrift on A4.
    """
    out = re.sub(r"<style>(?:(?!</style>).)*?max-width:\s*36em(?:(?!</style>).)*?</style>",
                 "", html, flags=re.S)
    if out != html:
        print("pandoc default stylesheet removed")
    return out


_TOC_RE = re.compile(r'<(h1|h2|h3|div)\b([^>]*?)data-toc="([12])"([^>]*?)>(.*?)</\1>', re.S)


def build_toc(body_html: str, relabel: dict | None = None) -> str:
    """The family's table of contents, generated from the assembled body.

    Anything that carries `data-toc="1"` or `"2"` earns a contents line, so every part of a volume
    that carries a running head is also findable from the front: domains and Knowledge Areas, the
    standards, the appendices, the glossary and the index. `relabel` maps an element id to the text
    that should be used instead — a chapter's own heading is its title alone, and a contents line
    reading "Cost, Schedule and Contingency Integration" tells the reader nothing about which
    domain it is.
    """
    relabel = relabel or {}
    items = []
    for m in _TOC_RE.finditer(body_html):
        hid = re.search(r'id="([^"]+)"', m.group(2) + m.group(4))
        if not hid:
            continue
        text = re.sub(r"\s+", " ", re.sub(r"<[^>]+>", " ", m.group(5))).strip()
        if hid.group(1) in relabel:
            text = f"{relabel[hid.group(1)]} — {text}"
        items.append((m.group(3), hid.group(1), text))
    out, open_sub = ['<nav id="TOC"><ul>'], False
    for lvl, hid, text in items:
        if lvl == "1":
            if open_sub:
                out.append("</ul></li>")
            out.append(f'<li><a href="#{hid}">{text}</a><ul>')
            open_sub = True
        else:
            out.append(f'<li><a href="#{hid}">{text}</a></li>')
    if open_sub:
        out.append("</ul></li>")
    out.append("</ul></nav>")
    print(f"contents lines: {len(items)} ({sum(1 for i in items if i[0] == '1')} top level)")
    return "".join(out)


def house_style(html: str, label: str = "") -> str:
    """The passes every PCI volume gets, whatever built it."""
    return tag_table_columns(sweep_icons(strip_pandoc_default_css(html), label), label)

BOOKS = {
    "pml-ai": {
        "title": "PML-AI<br/>Body of Knowledge",
        "run_title": "PCI PML-AI Body of Knowledge",
        "subtitle": ("The reference for the PCI Project Management Leader – AI<br/>"
                     "Leadership · delivery systems · governance · the governed use of AI"),
        # Cover. The credential line is the approved public name of the credential — the same
        # wording as the inner title page, which is the whole point of keeping them in one config.
        "cover": {
            "code": "PML-AI",
            "credential": "The reference for the PCI Project Management Leader – AI",
            "themes": "Leadership · delivery systems · governance · the governed use of AI",
            "chart": "paths",
        },
        # Certification standard set appended as back matter (skipped silently when not yet authored).
        "laws_file": "PML_AI_STANDARDS.md",
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
        "cover": {
            "code": "PFL-AI",
            "credential": "The reference for the PCI AI Project Finance Leader",
            "themes": "Structuring · modelling · coverage · the governed use of AI",
            "chart": "coverage",
        },
        "laws_file": "PFL_AI_STANDARDS.md",
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
  under the Institute's phase-gated authoring programme. It is <strong>not a released
  PCI publication</strong>: it carries no entitlement, syllabus or examination status, and the
  released edition will carry the full copyright, disclaimer and notices of the PCI book family.</p>
  <p><strong>What has been verified.</strong> Every number printed in this volume as a result — in
  worked examples, in-text calculations, multiple-choice options, exercise solutions and case studies
  — is recomputed independently with decimal arithmetic by the Institute's golden-answer verification
  suite, which must pass in full before any domain passes gate. All
  figures are PCI-original artwork generated from source specifications maintained by the Institute. No text,
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

  <h2 style="page-break-before: always;">How to use this book</h2>

  <p>This is a reference, not a course. It is written to be read in order once and consulted
  out of order thereafter, and the apparatus below exists to make the second use as easy as the
  first.</p>

  <p><strong>How anything is addressed.</strong> Content is numbered <em>Domain.KnowledgeArea.Topic</em>
  — so <em>7.3.2</em> is the second topic of the third Knowledge Area of Domain 7, and a
  cross-reference of the form <em>KA 10.2</em> is precise. Page numbers are never used for
  cross-references, because this volume is regenerated from source and its pagination is not stable
  across editions. Every reference in the text is therefore still valid in the next one.</p>

  <p><strong>The worked examples are the spine of the book.</strong> Each follows the same five
  steps — Setup, Formula, Substitution, Result, Interpretation — and the <em>Interpretation</em> is
  deliberately the longest and the most valuable. It is where the number is turned into a decision:
  what breaks the result, what the breakeven is, which assumption the conclusion is most sensitive
  to, and what a reviewer should check. If you read only one part of a worked example, read that
  one. The four steps before it exist so that you can reproduce the number and disagree with it.</p>

  <p><strong>Every number can be checked, and should be.</strong> Every figure printed as a result
  anywhere in this volume — in worked examples, in the text, in multiple-choice options, in exercise
  solutions, in case studies — is recomputed independently with decimal arithmetic by a verification
  suite that must pass in full before any chapter is accepted. Appendix C records how many checks
  stand behind each domain. If you find a number that does not reproduce from its stated method and
  inputs, that is a defect in this book and worth reporting.</p>

  <p><strong>The <em>AI in this KA</em> sections are not an appendix to the subject.</strong> Each
  states three things: where AI genuinely earns its place in that Knowledge Area, where it must not
  go, and how to verify its output concretely. They exist because the alternative — a single chapter
  about AI at the back — teaches that the question is separable from the work, and it is not. One
  principle governs all of them: <em>AI proposes; the professional verifies, decides and remains
  accountable.</em></p>

  <p><strong>What the sections at the end of each domain are for.</strong> <em>Advanced topics</em>
  extend the domain past the level a candidate is assessed on, and each closes with a list of
  invariants a reviewer can test. <em>Industry variations</em> say what changes by sector, and are
  written so that a reader in one sector can see what a reader in another is dealing with.
  <em>Case studies</em> carry real arithmetic and are the place where several Knowledge Areas have to
  work together. <em>Executive perspective</em> is what a director cannot delegate. <em>Calculation
  exercises</em> each carry a full solution and a <em>Common error</em> note, and the common error is
  frequently the more useful half. The <em>Practitioner's toolkit</em> items are meant to be adopted
  and then left stable; adapt the headings to your organisation and stop changing them.
  <em>Exam preparation</em> lists what is assessed, the calculations to be able to do under time
  pressure, and the traps — each cross-referenced to where it is taught.</p>

  <p><strong>Two conventions worth knowing before you meet them.</strong> Where a result is
  established in one domain, the others <em>cite</em> it rather than re-derive it; if two domains
  appear to derive the same thing independently, that is a defect. And where one word carries two
  genuinely different concepts, the later use states the collision explicitly rather than leaving
  you to infer it. Appendix B lists both.</p>

  <p><strong>If you are studying for the examination</strong>, work the calculation exercises before
  reading their solutions, and treat the <em>Common error</em> notes as the syllabus they effectively
  are. If you are using this at work, start from the Practitioner's toolkit of the domain you need
  and follow its cross-references back into the treatment.</p>
</div>
"""


# The one cover design in the family. Every volume fills the same five slots; nothing else about a
# cover is a decision anyone makes twice.
COVER = """
<div class="cover">
  <div class="coverhead">
    <div class="coverkicker">Project Controls Institute Global</div>
    <div class="covercode">{code}</div>
    <div class="coverbook">{book}</div>
    <div class="coverrule"></div>
    <div class="covercred">{credential}</div>
    <div class="coverthemes">{themes}</div>
  </div>
  <div class="coverband">{chart}</div>
  <div class="coverfoot">
    <div class="coverprinciple">{principle}</div>
    <div class="coverbadge"><span class="cbmain">{badge}</span>
    <span class="cbsub">{badgesub}</span></div>
    <div class="covertag">{tag}</div>
  </div>
</div>
"""

COVER_DEFAULTS = {
    "book": "Body of Knowledge",
    "principle": "AI proposes; the professional verifies,<br/>decides and remains accountable.",
    "badge": "First edition — draft",
    "badgesub": "For editorial and technical review · not for release or distribution",
    "tag": "Finance intelligently. Control predictively. Deliver successfully.",
}


def cover_html(**fields) -> str:
    """Fill the family cover. Unstated slots take the family default."""
    return COVER.format(**{**COVER_DEFAULTS, **fields})

# Cover artwork. Inline SVG rather than a bitmap so the cover stays vector at any size, and so the
# label colours are readable here in the source: everything on the cover band is light ink on the
# forest field, which is what the "debt service" label failed to be in the reviewed draft.
# The device is drawn in the two inks the cover type already uses — the cover of a professional
# body's reference is a title, not a poster, and one accent means one accent.
COVER_INK = "#F2EFE7"        # primary label ink on the forest field — always light
COVER_DIM = "#9DB3A5"        # secondary label ink
COVER_BAR = "#5C8A72"        # the mark itself
COVER_HOT = "#F2EFE7"        # the one emphasised element, in the primary ink


def _chart_coverage() -> str:
    """PFL: cash available against a debt-service line — the gap above the line is the headroom.

    The plot stops well short of the right edge so the debt-service line's label sits in clear
    field, in light ink: on the reviewed draft that label was dark-on-dark and simply disappeared.
    """
    vals = [100, 96, 92, 88, 84, 80, 76, 72, 68, 52]
    x0, base, bw, gap, scale = 34.0, 244.0, 30.0, 16.0, 1.78
    line = base - 60 * scale                      # the debt-service line
    plot_r = x0 + len(vals) * (bw + gap) - gap    # right edge of the bars
    bars = []
    for i, v in enumerate(vals):
        x = x0 + i * (bw + gap)
        h = v * scale
        fill = COVER_HOT if i == len(vals) - 1 else COVER_BAR
        bars.append(f'<rect x="{x:.1f}" y="{base - h:.1f}" width="{bw}" height="{h:.1f}" fill="{fill}"/>')
    # Headroom bracket on one bar, so the strapline has something to point at.
    bx = x0 + 2 * (bw + gap) + bw / 2
    top = base - vals[2] * scale
    bars.append(f'<line x1="{bx:.1f}" y1="{top:.1f}" x2="{bx:.1f}" y2="{line:.1f}" '
                f'stroke="{COVER_INK}" stroke-width="1.8"/>')
    bars.append(f'<path d="M{bx - 4:.1f} {top + 6:.1f} L{bx:.1f} {top:.1f} L{bx + 4:.1f} {top + 6:.1f}" '
                f'fill="none" stroke="{COVER_INK}" stroke-width="1.8"/>')
    bars.append(f'<path d="M{bx - 4:.1f} {line - 6:.1f} L{bx:.1f} {line:.1f} L{bx + 4:.1f} {line - 6:.1f}" '
                f'fill="none" stroke="{COVER_INK}" stroke-width="1.8"/>')
    bars.append(f'<text x="{bx:.1f}" y="{top - 9:.1f}" font-size="15" fill="{COVER_INK}" '
                f'font-family="Linux Biolinum O" text-anchor="middle">headroom</text>')
    return (
        '<svg class="coverchart" viewBox="0 0 640 300" xmlns="http://www.w3.org/2000/svg">'
        + "".join(bars)
        + f'<line x1="24" y1="{line:.1f}" x2="{plot_r + 10:.1f}" y2="{line:.1f}" stroke="{COVER_INK}" '
          f'stroke-width="2.2" stroke-dasharray="10 7"/>'
        + f'<text x="{plot_r + 18:.1f}" y="{line + 5:.1f}" font-size="15" fill="{COVER_INK}" '
          f'font-family="Linux Biolinum O" letter-spacing="0.6">debt service</text>'
        + f'<text x="24" y="284" font-size="15" fill="{COVER_DIM}" font-family="Linux Biolinum O">'
          'cash available · the gap is the headroom</text>'
        "</svg>")


def _chart_paths() -> str:
    """PML: the communication-path count — eight people, twenty-eight channels."""
    cx, cy, r, n = 320.0, 140.0, 104.0, 8
    pts = [(cx + r * math.cos(2 * math.pi * i / n - math.pi / 2),
            cy + r * math.sin(2 * math.pi * i / n - math.pi / 2)) for i in range(n)]
    edges = "".join(
        f'<line x1="{pts[i][0]:.1f}" y1="{pts[i][1]:.1f}" x2="{pts[j][0]:.1f}" y2="{pts[j][1]:.1f}" '
        f'stroke="{COVER_BAR}" stroke-width="1.5"/>'
        for i in range(n) for j in range(i + 1, n))
    nodes = "".join(f'<circle cx="{x:.1f}" cy="{y:.1f}" r="9" fill="{COVER_HOT}"/>' for x, y in pts)
    return (
        '<svg class="coverchart" viewBox="0 0 640 300" xmlns="http://www.w3.org/2000/svg">'
        + edges + nodes
        + f'<text x="{cx:.0f}" y="278" font-size="16" fill="{COVER_INK}" font-family="Linux Biolinum O" '
          'text-anchor="middle">n(n − 1)/2 = 28</text>'
        + f'<text x="{cx:.0f}" y="298" font-size="13" fill="{COVER_DIM}" font-family="Linux Biolinum O" '
          'text-anchor="middle">eight people · twenty-eight channels · every one a place to lose a '
          'decision</text>'
        "</svg>")


def _chart_variance() -> str:
    """PCL: the three curves of earned value — planned, earned and actual, and the two gaps.

    Drawn in the cover's own two inks: the plan and the spend as quiet lines, the earned curve as
    the emphasised one, because the whole discipline is the distance between them.
    """
    x0, y0, w, h = 40.0, 250.0, 560.0, 190.0

    def path(fs, upto=1.0, ink=COVER_BAR, width=2.0, dash=""):
        pts = []
        n = 60
        for i in range(int(n * upto) + 1):
            t = i / n
            pts.append(f"{x0 + t * w:.1f},{y0 - fs(t) * h:.1f}")
        d = f' stroke-dasharray="{dash}"' if dash else ""
        return (f'<polyline points="{" ".join(pts)}" fill="none" stroke="{ink}" '
                f'stroke-width="{width}"{d}/>')

    def s(t):                      # the S-curve: slow, fast, slow
        return t * t * (3 - 2 * t)

    cut = 0.62                     # the data date
    xd = x0 + cut * w
    planned = path(s, 1.0, COVER_BAR, 1.8, "8 6")
    actual = path(lambda t: s(t) * 1.14, cut, COVER_BAR, 1.8)
    earned = path(lambda t: s(t) * 0.78, cut, COVER_HOT, 2.6)
    pv = y0 - s(cut) * h
    ac = y0 - s(cut) * 1.14 * h
    ev = y0 - s(cut) * 0.78 * h
    # The two gaps are measured where they occur and labelled clear of each other: the cost gap is
    # bracketed above the schedule gap, and each label steps away from its bracket on a leader so
    # the two never collide however the curves are tuned.
    def bracket(x, ya, yb):
        return (f'<line x1="{x:.1f}" y1="{ya:.1f}" x2="{x:.1f}" y2="{yb:.1f}" '
                f'stroke="{COVER_INK}" stroke-width="1.8"/>'
                f'<line x1="{x - 4:.1f}" y1="{ya:.1f}" x2="{x + 4:.1f}" y2="{ya:.1f}" '
                f'stroke="{COVER_INK}" stroke-width="1.8"/>'
                f'<line x1="{x - 4:.1f}" y1="{yb:.1f}" x2="{x + 4:.1f}" y2="{yb:.1f}" '
                f'stroke="{COVER_INK}" stroke-width="1.8"/>')
    return (
        '<svg class="coverchart" viewBox="0 0 640 300" xmlns="http://www.w3.org/2000/svg">'
        + f'<line x1="{x0}" y1="{y0}" x2="{x0 + w}" y2="{y0}" stroke="{COVER_DIM}" stroke-width="1"/>'
        + planned + actual + earned
        + f'<line x1="{xd:.1f}" y1="{y0}" x2="{xd:.1f}" y2="{ac - 10:.1f}" stroke="{COVER_DIM}" '
          'stroke-width="1" stroke-dasharray="3 5"/>'
        + bracket(xd + 26, ev, ac)
        + bracket(xd + 10, ev, pv)
        + f'<text x="{xd + 34:.1f}" y="{ac - 6:.1f}" font-size="15" fill="{COVER_INK}" '
          'font-family="Linux Biolinum O">cost variance</text>'
        + f'<text x="{xd + 34:.1f}" y="{ev + 18:.1f}" font-size="15" fill="{COVER_INK}" '
          'font-family="Linux Biolinum O">schedule variance</text>'
        + f'<text x="{x0}" y="284" font-size="15" fill="{COVER_DIM}" font-family="Linux Biolinum O">'
          'planned · earned · actual — the distance between them is the discipline</text>'
        "</svg>")


CHARTS = {"coverage": _chart_coverage, "paths": _chart_paths, "variance": _chart_variance}


def slug(text: str) -> str:
    s = re.sub(r"<[^>]+>", " ", text)
    s = re.sub(r"[^a-z0-9]+", "-", s.lower()).strip("-")
    return s or "section"


def normalise_lists(text: str, announce: bool = True) -> str:
    """Give every list block the blank line python-markdown needs to see it as a list.

    Manuscripts write MCQ options directly under the stem line:

        ... its present value is closest to:
        - A. USD 370,370

    Markdown (unlike CommonMark) will not let a list interrupt a paragraph, so the whole block was
    typeset as one run-on sentence. Inserting the blank line here — not in the manuscripts, which
    other agents own — makes the options a real list without touching a word of the source.
    """
    marker = re.compile(r"^(?:[-*+]\s+|\d+[.)]\s+)")
    out, fence, prev, added = [], False, "", 0
    for line in text.split("\n"):
        stripped = line.lstrip()
        if stripped.startswith("```") or stripped.startswith("~~~"):
            fence = not fence
        elif not fence and marker.match(line) and prev.strip():
            p = prev
            if not (marker.match(p) or p[0] in "#|>" or p[0] in " \t" or p.rstrip().endswith("|")
                    or p.lstrip().startswith("<")):
                out.append("")
                added += 1
        out.append(line)
        prev = line
    if announce:
        print(f"list blocks unglued: {added}")
    return "\n".join(out)


# The manuscripts mark the correct MCQ option with ✅ (U+2705), which exists in no book font — it
# printed as blank space, so the answer key silently vanished from every MCQ. The replacement is a
# written word in real small caps inside a hairline rule: no glyph to go missing in any
# environment, legible in greyscale, and never dependent on colour.
MCQ_KEY = '<span class="mcq-key">correct</span>'


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


# ---------------------------------------------------------------------------------------------
# PCI Standards — back-matter part assembled from the shared foundational set plus the
# book's own certification standard set. Both are authored elsewhere; either may be absent mid-run, and
# an absent file is skipped rather than fatal.
# ---------------------------------------------------------------------------------------------

# Two of a standard's twenty-five elements are call-outs in their own right
# (PCI_STANDARDS_DRAFTING_MANUAL.md §8). They are matched tolerantly — with or without the element
# number, singular or plural — because a field that is boxed in one standard and not in the next
# looks like a mistake rather than a system. The element number and name are kept inside the box
# and the class's own small-caps label is suppressed there: the standards cite their elements by
# number ("named in element 19"), so the number cannot be thrown away, and the field name already
# says what the call-out is.
STD_FIELD_BOXES = ((r"External references?\.", "ext-ref field"),
                   (r"Jurisdictional cautions?\.", "pci-caution field"))


def _std_prose(md: str) -> str:
    inner = markdown.markdown(md, extensions=["tables"])
    inner = inner.replace("<hr />", "")
    # The legal-status disclaimer is set as a blockquote in source; it is a caution, so it prints
    # as one — labelled, bordered and readable without colour.
    inner = inner.replace("<blockquote>", '<div class="pci-caution">').replace("</blockquote>", "</div>")
    return inner


def _std_box(law_id: str, title: str, body_md: str) -> str:
    inner = markdown.markdown(body_md, extensions=["tables"])
    inner = inner.replace("<hr />", "")
    for pattern, cls in STD_FIELD_BOXES:
        inner = re.sub(r"(<p><strong>(?:\d+\.\s*)?" + pattern + r"</strong>(?:(?!</p>).)*</p>)"
                       r"(\s*<ul>(?:(?!</ul>).)*</ul>)?",
                       lambda m, c=cls: f'<div class="{c}">{m.group(1)}{m.group(2) or ""}</div>',
                       inner, flags=re.S)
    return (f'<div class="pci-standard" id="{slug("standard-" + law_id)}">'
            f'<div class="stdhead"><span class="stdid">{law_id}</span>'
            f'<span class="stdname">{title}</span></div>{inner}</div>')


def render_standards_file(path: pathlib.Path) -> tuple:
    """One standards file → (set title, html, standard count)."""
    text = path.read_text(encoding="utf-8")
    m = re.match(r"#\s+([^\n]+)\n", text)
    set_title = m.group(1).strip() if m else path.stem.replace("_", " ")
    body = normalise_lists(text[m.end():] if m else text, announce=False)

    # The "How to read these standards" section is explanatory prose, not a recommendation.
    # Wrapping it in the guidance call-out drew a dashed rule down the outer edge of six
    # consecutive pages, which is what a call-out looks like when it is asked to hold a chapter.
    out, buf, law = [], [], None

    def flush():
        nonlocal buf, law
        chunk = "\n".join(buf).strip()
        buf = []
        if not chunk:
            law = None
            return
        if law:
            out.append(_std_box(law[0], law[1], chunk))
        else:
            out.append(_std_prose(chunk))
        law = None

    laws = 0
    for line in body.split("\n"):
        # Law headings are matched at H2 or H3: the rebuilt sets do not agree on level,
        # and a level mismatch would silently drop the law out of its call-out box.
        head3 = re.match(r"#{2,3}\s+PCI STANDARD\s+(\S+)\s+—\s+(.+?)\s*$", line)
        head2 = re.match(r"##\s+(.+?)\s*$", line) if not head3 else None
        if head3:
            flush()
            law = (head3.group(1), head3.group(2))
            laws += 1
        elif head2:
            flush()
            out.append(f'<h3 class="stdgroup" id="{slug(set_title + "-" + head2.group(1))}" '
                       f'data-toc="2">{head2.group(1)}</h3>')
        else:
            buf.append(line)
    flush()
    return set_title, "".join(out), laws


def render_laws(book: str, cfg: dict) -> str:
    """The whole 'PCI Standards' part, or '' when no law file is present yet."""
    wanted = [STANDARDS_DIR / "PCI_FOUNDATIONAL_STANDARDS.md", STANDARDS_DIR / cfg.get("laws_file", "_none_")]
    files = [f for f in wanted if f.exists()]
    missing = [f.name for f in wanted if not f.exists()]
    if missing:
        print(f"standards: skipped (not yet authored) — {', '.join(missing)}")
    if not files:
        return ""
    sets, total = [], 0
    for f in files:
        try:
            title, html, laws = render_standards_file(f)
        except Exception as exc:                      # a malformed law file must not lose the book
            print(f"standards: FAILED to render {f} — {type(exc).__name__}: {exc}")
            continue
        total += laws
        sets.append(f'<h2 class="stdset" id="{slug(title)}" data-toc="2">{title}</h2>' + html)
    if not sets:
        return ""
    print(f"standards: {total} standards from {len(sets)} set(s)")
    return ('<div class="backmatter stdspart">'
            '<h1 class="stdstitle bmtitle" id="pci-professional-laws" data-toc="1">'
            'PCI Standards</h1>'
            '<p class="bmnote">Mandatory professional rules established by PCI Global for work '
            'within a PCI certification scope. They are not legislation and do not displace '
            'applicable law, contract or authoritative standards, and each law is cited by its '
            'stable identifier — never by page. The foundational set binds every PCI credential; '
            'the certification set that follows it binds this one.</p>'
            + "".join(sets) + "</div>")


def build(book: str, out: pathlib.Path) -> None:
    cfg = BOOKS[book]
    book_dir = ROOT / book
    files = manuscript_order(book_dir)
    print(f"manuscripts: {len(files)} — " + ", ".join(f.name for f in files))
    corpus = "\n\n".join(f.read_text(encoding="utf-8") for f in files)
    raw_corpus = corpus                       # pre-normalisation, for the key-terms index scrape
    corpus = normalise_lists(corpus)
    corpus = inject_figures(book_dir, corpus)
    keyed = corpus.count("✅")
    corpus = corpus.replace("✅", MCQ_KEY)
    print(f"MCQ answer keys marked: {keyed}")

    html = markdown.markdown(corpus, extensions=["tables", "fenced_code", "toc"],
                             extension_configs={"toc": {"anchorlink": False}})

    # Chapter openers. Label, then the number large in the display cut, then the title, then one
    # hairline to close the block — the opener takes the page and fills its top third with nothing.
    def chap(m):
        return (f'<div class="chapter"><div class="chapkicker">Domain</div>'
                f'<div class="chapnum">{int(m.group(2))}</div>'
                f'<h1 id="{m.group(1)}" data-toc="1">{m.group(3)}</h1>'
                f'<div class="chaprule"></div></div>')
    html = re.sub(r'<h1 id="([^"]+)">Domain\s+(\d+)\s+—\s+(.+?)</h1>', chap, html, flags=re.S)

    # KA openers.
    html = re.sub(r'<h2 id="([^"]+)">Knowledge\s+Area\s+(\d+\.\d+)\s+—\s+(.+?)</h2>',
                  r'<h2 class="ka" id="\1" data-toc="2"><span class="kanum">Knowledge Area \2</span>'
                  r'<span class="katitle">\3</span></h2>', html, flags=re.S)

    # MCQ option lists: mark the list that follows an MCQ stem so the options set as one option per
    # line with their own A./B./C./D. labels rather than markdown bullets.
    mcq_ul = re.compile(r"(<p><strong>MCQ (?:(?!</p>).)*</p>\s*)<ul>", re.S)
    nopts = len(mcq_ul.findall(html))
    html = mcq_ul.sub(r'\1<ul class="mcqopts">', html)
    print(f"MCQ option lists: {nopts}")

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
        kick_m = re.search(r'<div class="chapter"><div class="chapkicker">Domain</div>'
                           r'<div class="chapnum">' + str(in_part[0]) + r'</div>', html)
        if not kick_m:
            continue
        part_html = (f'<div class="partpage">'
                     f'<div class="partnum">{num}</div><div class="parttitle">{title}</div>'
                     f'<div class="partdesc">{desc}</div></div>')
        html = html[:kick_m.start()] + part_html + html[kick_m.start():]

    # Index from key-terms tables.
    terms = {}
    for m in re.finditer(r'### Key terms — KA (\d+\.\d+)\n\n\| Term \| Meaning \|\n\|[-| ]+\|\n((?:\|.*\|\n)+)', raw_corpus):
        ka, rows = m.group(1), m.group(2)
        for row in rows.strip().split("\n"):
            cells = [c.strip() for c in row.strip("|").split("|")]
            if len(cells) >= 2:
                term = re.sub(r"[`*]", "", cells[0]).strip()
                if term and term.lower() not in terms:
                    terms[term.lower()] = (term, ka)
    ka_ids = {m.group(2): m.group(1) for m in re.finditer(
        r'<h2 class="ka" id="([^"]+)"[^>]*><span class="kanum">Knowledge Area (\d+\.\d+)</span>', html)}
    groups = {}
    for term, ka in sorted(terms.values(), key=lambda t: t[0].lower()):
        hid = ka_ids.get(ka)
        if hid:
            letter = term[0].upper() if term[0].isalpha() else "#"
            groups.setdefault(letter, []).append(f'<div class="ixe"><a href="#{hid}">{term}</a></div>')
    ix = ['<div class="bookindex">'
          '<div class="ixtitle" id="book-index" data-toc="1">Index</div><div class="ixcols">']
    for letter in sorted(groups):
        ix.append(f'<div class="ixl">{letter}</div>' + "".join(groups[letter]))
    ix.append("</div></div>")
    print(f"index entries: {sum(len(v) for v in groups.values())}")

    # Back matter — the derived glossary (see make_glossary.py). Rendered from the same source as
    # the chapters' key-terms tables, so it cannot disagree with them.
    gloss_file = book_dir / "GLOSSARY.md"
    gloss_html = ""
    if gloss_file.exists():
        gtext = gloss_file.read_text(encoding="utf-8")
        # Drop the file's own H1 and the derivation note; the book supplies its own opener.
        gbody = re.sub(r"\A# [^\n]*\n+(?:> [^\n]*\n)*\n?", "", gtext)
        gbody = re.sub(r"\A\*\*\d[^\n]*\n+", "", gbody)
        inner = markdown.markdown(gbody, extensions=["tables"])
        inner = re.sub(r"<h2>([^<]+)</h2>", r'<div class="glossletter">\1</div>', inner)
        inner = re.sub(r"<p><strong>", '<p class="glossentry"><strong>', inner)
        n_terms = inner.count('class="glossentry"')
        gloss_html = ('<div class="backmatter glossary">'
                      '<h1 id="glossary" class="bmtitle" data-toc="1">Glossary</h1>'
                      '<p class="bmnote">Consolidated from every Knowledge Area\u2019s key-terms table. '
                      'The Knowledge Area reference after each entry is the authority \u2014 the gloss '
                      'points to the treatment and does not replace it.</p>'
                      + inner + "</div>")
        print(f"glossary entries: {n_terms}")

    # Back matter — the derived appendices (see make_appendices.py).
    app_file = book_dir / "APPENDICES.md"
    app_html = ""
    if app_file.exists():
        atext = app_file.read_text(encoding="utf-8")
        abody = re.sub(r"\A# [^\n]*\n+(?:> [^\n]*\n)*\n?", "", atext)
        inner = markdown.markdown(abody, extensions=["tables"])
        inner = re.sub(r"<h2>([^<]+)</h2>",
                       lambda m: f'<h2 class="apptitle" id="{slug(m.group(1))}" data-toc="1">'
                                 f"{m.group(1)}</h2>", inner)
        app_html = '<div class="backmatter appendices">' + inner + "</div>"
        print(f"appendix tables: {inner.count('<table>')}")

    # Back matter — the standards and frameworks register (Appendix F). Derived, and the one appendix
    # whose job is disclosure rather than reference: it states what the volume engages with, that
    # nothing is reproduced, and that no editions are asserted.
    std_file = book_dir / "STANDARDS.md"
    std_html = ""
    if std_file.exists():
        stext = std_file.read_text(encoding="utf-8")
        sbody = re.sub(r"\A# [^\n]*\n+(?:> [^\n]*\n)*\n?", "", stext)
        inner = markdown.markdown(sbody, extensions=["tables"])
        inner = re.sub(r"<h2>([^<]+)</h2>", r'<h3>\1</h3>', inner)
        std_html = ('<div class="backmatter appendices">'
                    '<h2 class="apptitle" id="appendix-f" data-toc="1">'
                    'Appendix F — Standards and frameworks referenced</h2>'
                    + inner + "</div>")
        # Count body rows, not `<tr>` minus `<th>` — that counted the family tables (4 and 7) and
        # reported them as entries, which is a log line that lies about coverage.
        print(f"standards register: {len(re.findall(r'<td><strong>', inner))} entries")

    # Back matter — the integrated capstones (Appendix G). Authored rather than derived: a capstone's
    # content is the agreement between domains, which no generator can compute. It follows the
    # derived appendices so the volume's lettering runs in order.
    cap_file = book_dir / "CAPSTONES.md"
    cap_html = ""
    if cap_file.exists():
        ctext = cap_file.read_text(encoding="utf-8")
        cbody = re.sub(r"\A# [^\n]*\n+", "", ctext)
        inner = markdown.markdown(cbody, extensions=["tables"])
        inner = re.sub(r"<h2>([^<]+)</h2>",
                       lambda m: f'<h2 class="apptitle" id="{slug(m.group(1))}" data-toc="1">'
                                 f"{m.group(1)}</h2>", inner)
        cap_html = ('<div class="backmatter appendices">'
                    '<h2 class="apptitle" id="appendix-g" data-toc="1">'
                    'Appendix G — Integrated capstones</h2>'
                    + inner + "</div>")
        print(f"capstone sections: {inner.count('<h3>')}")

    # Back matter — the PCI Standards part: after the appendices, before the glossary.
    laws_html = render_laws(book, cfg)

    # Contents. Generated from the assembled body rather than the chapters alone, so every part of
    # the volume that carries a running head also carries a contents line: domains and Knowledge
    # Areas, then appendices, the law part, the glossary and the index.
    body_html = f"{html}{app_html}{std_html}{cap_html}{laws_html}{gloss_html}{''.join(ix)}"
    # A chapter's own heading is the title alone — the domain number lives in the opener's kicker.
    # The contents line needs both, or "Cost, Schedule and Contingency Integration" gives the reader
    # no way to tell which domain it is.
    chap_kicker = {m.group(2): f"Domain {m.group(1)}" for m in re.finditer(
        r'<div class="chapkicker">Domain</div><div class="chapnum">(\d+)</div><h1 id="([^"]+)"',
        body_html)}
    toc = build_toc(body_html, chap_kicker)

    cv = cfg["cover"]
    cover = cover_html(code=cv["code"], credential=cv["credential"], themes=cv["themes"],
                       chart=CHARTS[cv["chart"]]())
    front = FRONT.format(title=cfg["title"], subtitle=cfg["subtitle"], domains=len(files))
    doc_html = (f"<!doctype html><html lang='{LANG}'><head><meta charset='utf-8'>"
                f"<style>html {{ string-set: booktitle \"{cfg['run_title']}\"; }}</style>"
                f"</head><body>{cover}{front}{toc}{body_html}</body></html>")
    doc_html = house_style(doc_html, book)
    html_file = book_dir / "build" / "_combined.html"
    html_file.parent.mkdir(parents=True, exist_ok=True)
    html_file.write_text(doc_html, encoding="utf-8")

    from weasyprint import CSS, HTML
    doc = HTML(filename=str(html_file)).render(stylesheets=[CSS(filename=str(HOUSE_CSS))])
    doc.write_pdf(str(out))
    print(f"OK {out}  pages={len(doc.pages)}")


if __name__ == "__main__":
    book = sys.argv[1]
    out = pathlib.Path(sys.argv[2]) if len(sys.argv) > 2 else ROOT / book / "build" / f"{book}-bok-draft.pdf"
    build(book, out)
