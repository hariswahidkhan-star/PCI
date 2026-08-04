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
LAWS_DIR = ROOT / "laws"

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
        # Certification law set appended as back matter (skipped silently when not yet authored).
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


COVER = """
<div class="cover">
  <div class="coverhead">
    <div class="coverkicker">Project Controls Institute Global</div>
    <div class="covercode">{code}</div>
    <div class="coverbook">Body of Knowledge</div>
    <div class="coverrule"></div>
    <div class="covercred">{credential}</div>
    <div class="coverthemes">{themes}</div>
  </div>
  <div class="coverband">{chart}</div>
  <div class="coverfoot">
    <div class="coverprinciple">AI proposes; the professional verifies,<br/>decides and remains
    accountable.</div>
    <div class="coverbadge"><span class="cbmain">First edition — draft</span>
    <span class="cbsub">For editorial and technical review · not for release or distribution</span></div>
    <div class="covertag">Finance intelligently. Control predictively. Deliver successfully.</div>
  </div>
</div>
"""

# Cover artwork. Inline SVG rather than a bitmap so the cover stays vector at any size, and so the
# label colours are readable here in the source: everything on the cover band is light ink on the
# navy field, which is what the "debt service" label failed to be in the reviewed draft.
COVER_INK = "#F1F5F9"        # primary label ink on navy — always light
COVER_DIM = "#93A7C4"        # secondary label ink on navy
COVER_BAR = "#2A4FC4"
COVER_HOT = "#C13329"


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
                f'font-family="sans-serif" text-anchor="middle">headroom</text>')
    return (
        '<svg class="coverchart" viewBox="0 0 640 300" xmlns="http://www.w3.org/2000/svg">'
        + "".join(bars)
        + f'<line x1="24" y1="{line:.1f}" x2="{plot_r + 10:.1f}" y2="{line:.1f}" stroke="{COVER_INK}" '
          f'stroke-width="2.2" stroke-dasharray="10 7"/>'
        + f'<text x="{plot_r + 18:.1f}" y="{line + 5:.1f}" font-size="15" fill="{COVER_INK}" '
          f'font-family="sans-serif" letter-spacing="0.6">debt service</text>'
        + f'<text x="24" y="284" font-size="15" fill="{COVER_DIM}" font-family="sans-serif">'
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
        + f'<text x="{cx:.0f}" y="278" font-size="16" fill="{COVER_INK}" font-family="sans-serif" '
          'text-anchor="middle">n(n − 1)/2 = 28</text>'
        + f'<text x="{cx:.0f}" y="298" font-size="13" fill="{COVER_DIM}" font-family="sans-serif" '
          'text-anchor="middle">eight people · twenty-eight channels · every one a place to lose a '
          'decision</text>'
        "</svg>")


CHARTS = {"coverage": _chart_coverage, "paths": _chart_paths}


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


# The manuscripts mark the correct MCQ option with ✅ (U+2705), which exists in no font on the build
# host — it printed as blank space, so the answer key silently vanished from every MCQ. U+2713 is
# present in DejaVu Sans (the sans fallback), and the marker carries the word "correct" and a rule
# box as well, so it survives grayscale and never depends on colour.
MCQ_KEY = '<span class="mcq-key">✓ correct</span>'


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
# book's own certification law set. Both are authored elsewhere; either may be absent mid-run, and
# an absent file is skipped rather than fatal.
# ---------------------------------------------------------------------------------------------

# Fields the law format carries that are call-outs in their own right (SUPERSEDED_LAW_SYSTEM_v0.md §6).
LAW_FIELD_BOXES = (("External references.", "ext-ref"),
                   ("Jurisdictional caution.", "pci-caution"))


def _law_prose(md: str) -> str:
    inner = markdown.markdown(md, extensions=["tables"])
    inner = inner.replace("<hr />", "")
    # The legal-status disclaimer is set as a blockquote in source; it is a caution, so it prints
    # as one — labelled, bordered and readable without colour.
    inner = inner.replace("<blockquote>", '<div class="pci-caution">').replace("</blockquote>", "</div>")
    return inner


def _law_box(law_id: str, title: str, body_md: str) -> str:
    inner = markdown.markdown(body_md, extensions=["tables"])
    inner = inner.replace("<hr />", "")
    for label, cls in LAW_FIELD_BOXES:
        inner = re.sub(r"<p><strong>" + re.escape(label) + r"</strong>\s*(.*?)</p>",
                       lambda m, c=cls: f'<div class="{c}"><p>{m.group(1)}</p></div>',
                       inner, flags=re.S)
    return (f'<div class="pci-law" id="{slug("law-" + law_id)}">'
            f'<div class="lawhead"><span class="lawid">{law_id}</span>'
            f'<span class="lawname">{title}</span></div>{inner}</div>')


def render_law_file(path: pathlib.Path) -> tuple:
    """One law file → (set title, html, law count)."""
    text = path.read_text(encoding="utf-8")
    m = re.match(r"#\s+([^\n]+)\n", text)
    set_title = m.group(1).strip() if m else path.stem.replace("_", " ")
    body = normalise_lists(text[m.end():] if m else text, announce=False)

    out, buf, law, guidance = [], [], None, False

    def flush():
        nonlocal buf, law
        chunk = "\n".join(buf).strip()
        buf = []
        if not chunk:
            law = None
            return
        if law:
            out.append(_law_box(law[0], law[1], chunk))
        elif guidance:
            out.append('<div class="pci-guidance">' + _law_prose(chunk) + "</div>")
        else:
            out.append(_law_prose(chunk))
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
            guidance = False
            laws += 1
        elif head2:
            flush()
            guidance = "how to read" in head2.group(1).lower()
            out.append(f'<h3 class="lawgroup" id="{slug(set_title + "-" + head2.group(1))}" '
                       f'data-toc="2">{head2.group(1)}</h3>')
        else:
            buf.append(line)
    flush()
    return set_title, "".join(out), laws


def render_laws(book: str, cfg: dict) -> str:
    """The whole 'PCI Standards' part, or '' when no law file is present yet."""
    wanted = [LAWS_DIR / "PCI_FOUNDATIONAL_STANDARDS.md", LAWS_DIR / cfg.get("laws_file", "_none_")]
    files = [f for f in wanted if f.exists()]
    missing = [f.name for f in wanted if not f.exists()]
    if missing:
        print(f"laws: skipped (not yet authored) — {', '.join(missing)}")
    if not files:
        return ""
    sets, total = [], 0
    for f in files:
        try:
            title, html, laws = render_law_file(f)
        except Exception as exc:                      # a malformed law file must not lose the book
            print(f"laws: FAILED to render {f} — {type(exc).__name__}: {exc}")
            continue
        total += laws
        sets.append(f'<h2 class="lawset" id="{slug(title)}" data-toc="2">{title}</h2>' + html)
    if not sets:
        return ""
    print(f"laws: {total} laws from {len(sets)} set(s)")
    return ('<div class="backmatter lawspart">'
            '<h1 class="lawstitle bmtitle" id="pci-professional-laws" data-toc="1">'
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

    # Chapter openers.
    def chap(m):
        return (f'<div class="chapter"><div class="chapnum">{int(m.group(2)):02d}</div>'
                f'<div class="chapkicker">Domain {m.group(2)}</div>'
                f'<h1 id="{m.group(1)}" data-toc="1">{m.group(3)}</h1>'
                f'<div class="chaprule"></div><div class="chaprule2"></div></div>')
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
        kicker = f"Domain {in_part[0]}"
        kick_m = re.search(r'<div class="chapter"><div class="chapnum">\d+</div>'
                           r'<div class="chapkicker">' + re.escape(kicker) + r'</div>', html)
        if not kick_m:
            continue
        part_html = (f'<div class="partpage"><div class="partghost">{pnum:02d}</div>'
                     f'<div class="partnum">{num}</div><div class="parttitle">{title}</div>'
                     f'<div class="partdesc">{desc}</div><div class="partbar"></div></div>')
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
    chap_kicker = {m.group(2): m.group(1) for m in re.finditer(
        r'<div class="chapkicker">(Domain \d+)</div><h1 id="([^"]+)"', body_html)}
    toc_items = []
    for m in re.finditer(r'<(h1|h2|h3|div)\b([^>]*?)data-toc="([12])"([^>]*?)>(.*?)</\1>',
                         body_html, flags=re.S):
        attrs = m.group(2) + m.group(4)
        hid = re.search(r'id="([^"]+)"', attrs)
        if not hid:
            continue
        text = re.sub(r"<[^>]+>", " ", m.group(5))
        text = re.sub(r"\s+", " ", text).strip()
        if hid.group(1) in chap_kicker:
            text = f"{chap_kicker[hid.group(1)]} — {text}"
        toc_items.append((m.group(3), hid.group(1), text))
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
    print(f"contents lines: {len(toc_items)} "
          f"({sum(1 for t in toc_items if t[0] == '1')} top level)")

    cv = cfg["cover"]
    cover = COVER.format(code=cv["code"], credential=cv["credential"], themes=cv["themes"],
                         chart=CHARTS[cv["chart"]]())
    front = FRONT.format(title=cfg["title"], subtitle=cfg["subtitle"], domains=len(files))
    doc_html = ("<!doctype html><html><head><meta charset='utf-8'>"
                f"<style>html {{ string-set: booktitle \"{cfg['run_title']}\"; }}</style>"
                f"</head><body>{cover}{front}{''.join(toc)}{body_html}</body></html>")
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
