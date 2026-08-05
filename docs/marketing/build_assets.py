#!/usr/bin/env python3
"""Build the shareable marketing assets: an A4 one-page overview, LinkedIn carousels, and the
PCL-AI course outline as a document post.

All of them are set in the publication design system — the same Libertine superfamily, the same
green, the same restraint — so a reader who meets a carousel and later opens a volume is not meeting
two different organisations.

Carousels are square (1080 pt) because that is what LinkedIn document posts display well. The course
outline is A4: it is read, not swiped.

The outline is rendered from `course-outline-pcl-ai.md` rather than duplicated here, so the text
someone pastes into an Article and the text in the PDF cannot drift apart.

Usage:  python3 build_assets.py
"""
import html
import pathlib
import re
import sys

HERE = pathlib.Path(__file__).resolve().parent
OUT = HERE / "assets"

GREEN = "#14432E"
GREEN_MID = "#2D6A4F"
TINT = "#F2F6F3"
INK = "#1A1A1A"
MUTED = "#5A6560"
RULE = "#C8CFC9"

BASE = f"""
  @page {{ margin: 0; }}
  * {{ box-sizing: border-box; }}
  body {{ margin: 0; font-family: "Linux Libertine O", "Bitstream Charter", serif;
          color: {INK}; font-feature-settings: "liga" 1, "onum" 1; }}
  .sans {{ font-family: "Linux Biolinum O", sans-serif; }}
  .kicker {{ font-family: "Linux Biolinum O", sans-serif; font-variant-caps: small-caps;
             font-feature-settings: "smcp" 1; letter-spacing: .18em; color: {GREEN_MID}; }}
  .num {{ font-feature-settings: "lnum" 1, "tnum" 1; }}
  strong {{ font-weight: 600; }}
  em {{ font-style: italic; }}
"""

# ---------------------------------------------------------------- one-pager

ONEPAGER = f"""
<style>
{BASE}
  @page {{ size: A4; }}
  .sheet {{ width: 210mm; height: 297mm; padding: 18mm 20mm 14mm 20mm; }}
  .top {{ border-bottom: 2pt solid {GREEN}; padding-bottom: 6mm; margin-bottom: 9mm; }}
  h1 {{ font-family: "Linux Libertine Display O", serif; font-size: 30pt; margin: 3mm 0 2mm 0;
        color: {GREEN}; font-weight: normal; line-height: 1.1; }}
  .sub {{ font-size: 11.5pt; color: {MUTED}; }}
  h2 {{ font-family: "Linux Biolinum O", sans-serif; font-variant-caps: small-caps;
        font-feature-settings: "smcp" 1; letter-spacing: .14em; font-size: 9.5pt;
        color: {GREEN}; margin: 6.5mm 0 2.5mm 0; font-weight: 600; }}
  p {{ font-size: 10pt; line-height: 1.5; margin: 0 0 3mm 0; }}
  .vols {{ display: flex; gap: 5mm; margin: 4mm 0 0 0; }}
  .vol {{ flex: 1; background: {TINT}; border-left: 2pt solid {GREEN}; padding: 4mm 4.5mm; }}
  .vol .code {{ font-family: "Linux Libertine Display O", serif; font-size: 17pt; color: {GREEN}; }}
  .vol .name {{ font-size: 8.6pt; color: {MUTED}; line-height: 1.35; margin-top: 1mm;
                min-height: 8.3mm; }}
  .vol .stat {{ font-size: 8.4pt; margin-top: 2.5mm; }}
  .figs {{ display: flex; gap: 4mm; margin-top: 3mm; }}
  .fig {{ flex: 1; border-top: 1pt solid {RULE}; padding-top: 2.5mm; }}
  .fig .n {{ font-family: "Linux Libertine Display O", serif; font-size: 21pt; color: {GREEN};
             font-feature-settings: "lnum" 1; }}
  .fig .l {{ font-size: 7.8pt; color: {MUTED}; line-height: 1.3; }}
  .principle {{ margin-top: 7mm; padding: 5mm 6mm; background: {TINT};
                border-left: 2pt solid {GREEN}; font-style: italic; font-size: 11pt; }}
  .foot {{ margin-top: 9mm; border-top: 1pt solid {RULE}; padding-top: 3mm;
           font-size: 8pt; color: {MUTED}; }}
</style>
<div class="sheet">
  <div class="top">
    <div class="kicker">Project Controls Institute Global</div>
    <h1>The AI Project Leadership<br/>Certification Suite</h1>
    <div class="sub">Three credentials. Three references. One standard of professional accountability.</div>
  </div>

  <h2>What it is</h2>
  <p>Three Bodies of Knowledge covering project controls, project finance and project management —
  each written as a working reference rather than a syllabus summary, and each treating the governed
  use of artificial intelligence as part of the discipline rather than an appendix to it.</p>

  <div class="vols">
    <div class="vol">
      <div class="code">PCL-AI</div>
      <div class="name">PCI AI Project Controls Leader</div>
      <div class="stat num">13 domains · 61 knowledge areas<br/>26 sector case studies</div>
    </div>
    <div class="vol">
      <div class="code">PFL-AI</div>
      <div class="name">PCI AI Project Finance Leader</div>
      <div class="stat num">16 domains · 61 knowledge areas<br/>33 sector case studies</div>
    </div>
    <div class="vol">
      <div class="code">PML-AI</div>
      <div class="name">PCI Project Management Leader – AI</div>
      <div class="stat num">16 domains · 63 knowledge areas<br/>33 sector case studies</div>
    </div>
  </div>

  <h2>Numbers are recomputed, not retyped</h2>
  <p>In PFL-AI and PML-AI, worked examples, in-text figures, exercise answers, case-study numbers and
  <em>every</em> multiple-choice option — not only the correct one — are recomputed from their stated
  inputs at 28-digit precision and compared against the printed value. The suite is fail-closed: a
  domain does not pass gate until it passes in full. PCL-AI has no equivalent suite yet; its numbers
  were checked by sampled independent recomputation across all thirteen domains, and building the
  full harness for it is the programme's largest open item.</p>

  <div class="figs">
    <div class="fig"><div class="n">15,613</div><div class="l">machine calculation checks in PFL-AI and PML-AI, all passing</div></div>
    <div class="fig"><div class="n">45</div><div class="l">domains across the three volumes</div></div>
    <div class="fig"><div class="n">185</div><div class="l">knowledge areas</div></div>
    <div class="fig"><div class="n">92</div><div class="l">sector case studies</div></div>
  </div>

  <h2>Backed by the PCI Standards</h2>
  <p>113 mandatory professional standards with 532 process requirements. Each states its purpose,
  who owns the decision, what evidence must exist, what practice is prohibited, what triggers
  escalation, what artificial intelligence may assist with and what it must never decide, approve or
  certify — and a <strong>compliance test an assessor can actually perform</strong>. They are
  certification requirements established by the Institute, not legislation.</p>

  <h2>On external standards</h2>
  <p>Frameworks are named and their principles described in the Institute's own words. No protected
  text, table, figure or question from any external publication is reproduced, and no reference
  implies endorsement by or affiliation with its publisher.</p>

  <div class="principle">AI proposes; the professional verifies, decides and remains accountable.</div>

  <div class="foot">Project Controls Institute Global · projectcontrolsinstitute.org<br/>
  Finance intelligently. Control predictively. Deliver successfully.</div>
</div>
"""

# ---------------------------------------------------------------- carousels

CAROUSEL_CSS = f"""
{BASE}
  @page {{ size: 1080pt 1080pt; }}
  /* A square on a feed is read whole, not scanned top-down: the composition is centred on
     the optical middle and the footer is the only thing pinned, so no slide ends in a void. */
  .slide {{ width: 1080pt; height: 1080pt; padding: 78pt 92pt 62pt 92pt;
            display: flex; flex-direction: column; page-break-after: always; }}
  .slide .body {{ flex: 1 1 auto; display: flex; flex-direction: column;
                  justify-content: center; }}
  .slide .body > *:first-child {{ margin-top: 0; }}
  .slide .body > *:last-child {{ margin-bottom: 0; }}
  .slide.dark {{ background: {GREEN}; color: #fff; }}
  .slide .kick {{ font-family: "Linux Biolinum O", sans-serif; font-variant-caps: small-caps;
                  font-feature-settings: "smcp" 1; letter-spacing: .2em; font-size: 17pt;
                  color: {GREEN_MID}; }}
  .slide.dark .kick {{ color: #9DBBAA; }}
  h1 {{ font-family: "Linux Libertine Display O", serif; font-size: 78pt; line-height: 1.05;
        margin: 26pt 0 0 0; font-weight: normal; color: {GREEN}; }}
  .slide.dark h1 {{ color: #fff; }}
  h2 {{ font-family: "Linux Libertine Display O", serif; font-size: 50pt; line-height: 1.12;
        margin: 22pt 0 18pt 0; font-weight: normal; color: {GREEN}; }}
  .slide.dark h2 {{ color: #fff; }}
  p {{ font-size: 27pt; line-height: 1.5; margin: 0 0 18pt 0; }}
  .slide.dark p {{ color: #E6EFE9; }}
  .big {{ font-family: "Linux Libertine Display O", serif; font-size: 132pt; color: {GREEN};
          font-feature-settings: "lnum" 1; line-height: 1; margin-bottom: 20pt; }}
  .slide.dark .big {{ color: #fff; }}
  .rule {{ width: 120pt; border-top: 3pt solid {GREEN}; margin: 26pt 0; }}
  .slide.dark .rule {{ border-color: #9DBBAA; }}
  .calc {{ font-family: "Linux Libertine Mono O", monospace; font-size: 25pt;
           background: {TINT}; padding: 22pt 26pt; border-left: 4pt solid {GREEN};
           font-feature-settings: "lnum" 1, "tnum" 1; line-height: 1.6;
           margin-bottom: 22pt; }}
  .foot {{ flex: 0 0 auto; margin-top: 40pt;
           font-family: "Linux Biolinum O", sans-serif; font-size: 16pt; color: {MUTED};
           border-top: 1pt solid {RULE}; padding-top: 14pt; }}
  .slide.dark .foot {{ color: #9DBBAA; border-color: #2D6A4F; }}
"""


def slide(body, dark=False, foot="Project Controls Institute Global"):
    cls = "slide dark" if dark else "slide"
    return (f'<div class="{cls}"><div class="body">{body}</div>'
            f'<div class="foot">{foot}</div></div>')


CAROUSELS = {
    "01-the-cpi-that-lies": [
        slide('<div class="kick">Project controls</div>'
              '<h1>The CPI<br/>that lies</h1><div class="rule"></div>'
              '<p>A cost performance index can look excellent and still be wrong. '
              'Here is the arithmetic.</p>', dark=True),
        slide('<h2>Month-end on a control account</h2>'
              '<p>Earned value <strong>2,200,000</strong>.<br/>'
              'Invoiced cost so far <strong>1,850,000</strong>.</p>'
              '<p>A subcontractor has done a further <strong>240,000</strong> of work. '
              'It has not been invoiced yet.</p>'),
        slide('<h2>Skip the accrual</h2>'
              '<div class="calc">CPI = 2,200,000 ÷ 1,850,000<br/>    = 1.19</div>'
              '<p>Comfortably under budget. The report writes itself.</p>'),
        slide('<h2>Now book the accrual</h2>'
              '<div class="calc">AC  = 1,850,000 + 240,000<br/>    = 2,090,000<br/><br/>'
              'CPI = 2,200,000 ÷ 2,090,000<br/>    = 1.05</div>'),
        slide('<div class="big">0.14</div>'
              '<p>The index moved fourteen points on one missing accrual. '
              'Nothing about the work changed.</p>'),
        slide('<h2>Why it matters</h2>'
              '<p>Every forecast built on CPI inherits the error. '
              'An estimate at completion computed on 1.19 is not slightly optimistic; '
              'it is answering a different question from the one the board asked.</p>'),
        slide('<h2>The professional act</h2>'
              '<p>Actual cost is not what has been invoiced. It is what has been '
              '<em>incurred</em>: invoices, plus commitments delivered and not yet billed, '
              'plus accruals, at a stated cut-off.</p>'
              '<p>Cut-off is a decision. Record it.</p>'),
        slide('<div class="kick">PCI Standards</div>'
              '<h2>Accrual completeness</h2>'
              '<p>A performance index reported without a stated cut-off and a complete accrual '
              'position is not a measurement. It is an impression.</p>'
              '<p class="kick">projectcontrolsinstitute.org</p>', dark=True),
    ],
    "02-what-a-compliance-test-looks-like": [
        slide('<div class="kick">PCI Standards</div>'
              '<h1>A rule you<br/>cannot test<br/>is a slogan</h1><div class="rule"></div>'
              '<p>What separates a professional standard from a poster.</p>', dark=True),
        slide('<h2>Most codes stop here</h2>'
              '<p><em>"The professional shall exercise due care and act with integrity."</em></p>'
              '<p>Nobody disagrees with it. Nobody can be shown to have breached it, either.</p>'),
        slide('<h2>The test question</h2>'
              '<p>Could two competent reviewers, applying this to the same evidence, '
              'reach the <strong>same</strong> answer?</p>'
              '<p>If not, it is not a requirement. It is an aspiration.</p>'),
        slide('<h2>Weak</h2>'
              '<p><em>"Compliance is demonstrated when the forecast has been properly '
              'reviewed."</em></p>'
              '<p>Properly, by whom, against what?</p>'),
        slide('<h2>Strong</h2>'
              '<p>Compliance is demonstrated when the approved forecast reconciles without '
              'unexplained difference to the cost ledger, the commitment register, the accrual '
              'schedule, the approved change register, the risk-adjusted estimate and the '
              'authorised schedule status date.</p>'),
        slide('<h2>The difference</h2>'
              '<p>The second one names the documents, so an assessor can ask for them. '
              'The first one names a feeling.</p>'),
        slide('<div class="big">532</div>'
              '<p>process requirements across 113 PCI Standards. Each carries a compliance test '
              'written to be performed, not admired.</p>'),
        slide('<div class="kick">Every standard states</div>'
              '<h2>Who decides.<br/>What evidence.<br/>What is prohibited.<br/>What AI may not do.</h2>'
              '<p class="kick">projectcontrolsinstitute.org</p>', dark=True),
    ],
    "03-ai-what-it-may-not-decide": [
        slide('<div class="kick">Governed AI</div>'
              '<h1>What AI may<br/>never decide</h1><div class="rule"></div>'
              '<p>The line the PCI Standards draw, and why it is drawn there.</p>', dark=True),
        slide('<h2>The principle</h2>'
              '<p><em>AI proposes; the professional verifies, decides and remains '
              'accountable.</em></p>'
              '<p>Every standard in the suite states both halves: what AI may assist with, and '
              'what it must never do.</p>'),
        slide('<h2>AI may</h2>'
              '<p>Draft. Extract. Classify. Detect. Forecast. Summarise. Surface an exception '
              'a human would have taken a week to find.</p>'),
        slide('<h2>AI must not</h2>'
              '<p>Decide. Approve. Certify. Sign. Waive. Authorise.</p>'
              '<p>Or be represented as having independently verified anything.</p>'),
        slide('<h2>Why the line sits there</h2>'
              '<p>Accountability attaches to a person. A credential guarantees nothing if the '
              'holder can point at a model when the number turns out wrong.</p>'),
        slide('<h2>"Review the AI output" is not verification</h2>'
              '<p>Verification names a method: independent recomputation, source tracing, '
              'clause-to-summary comparison, sampling on a stated basis, reconciliation, boundary '
              'testing, sensitivity analysis.</p>'),
        slide('<h2>The trap</h2>'
              '<p>Re-running the same tool is not an independent check. Neither is a reviewer '
              'confirming their own work.</p>'
              '<p>The standards close both routes explicitly.</p>'),
        slide('<div class="kick">PCI Standards</div>'
              '<h2>AI earns its place<br/>by being checkable</h2>'
              '<p>Not by being confident.</p>'
              '<p class="kick">projectcontrolsinstitute.org</p>', dark=True),
    ],
    "04-the-covenant-that-bites": [
        slide('<div class="kick">Project finance</div>'
              '<h1>The covenant<br/>that actually<br/>bites</h1><div class="rule"></div>'
              '<p>Coverage looks comfortable until you ask which test binds first.</p>', dark=True),
        slide('<h2>Coverage, plainly</h2>'
              '<p>Debt service coverage is cash available for debt service, divided by debt '
              'service. The ratio is the coverage; the gap above the threshold is the '
              '<em>headroom</em>.</p>'),
        slide('<h2>Two different questions</h2>'
              '<p><strong>DSCR</strong> asks whether this period pays.</p>'
              '<p><strong>LLCR</strong> asks whether the whole remaining loan life pays, '
              'discounted.</p>'),
        slide('<h2>Why both exist</h2>'
              '<p>A project can pass every periodic test and still be unable to repay over the '
              'life of the facility. It can also fail one bad quarter while remaining sound.</p>'),
        slide('<h2>The number PCI will not invent</h2>'
              '<p>The threshold is not a professional convention. It is written in the finance '
              'documents.</p>'
              '<p>The obligation is to use the <em>documented</em> figure and test against it — '
              'never a remembered one.</p>'),
        slide('<h2>Where practitioners get caught</h2>'
              '<p>A lock-up test and a default test rarely sit at the same level. Distributions '
              'stop long before an event of default. Model both, and know which one you are '
              'reporting against.</p>'),
        slide('<div class="big">1.05</div>'
              '<p>A coverage ratio of 1.05 is not "just above 1.00". It is a project whose '
              'margin for error is five per cent of one period\'s cash.</p>'),
        slide('<div class="kick">PFL-AI</div>'
              '<h2>Cash before<br/>accounting<br/>appearance</h2>'
              '<p class="kick">projectcontrolsinstitute.org</p>', dark=True),
    ],
    "05-the-five-step-method": [
        slide('<div class="kick">How the books teach</div>'
              '<h1>Five steps,<br/>every time</h1><div class="rule"></div>'
              '<p>Every worked example in all three volumes. One shape.</p>', dark=True),
        slide('<h2>1 · Setup</h2>'
              '<p>The situation and every input, stated before any arithmetic. If a number is '
              'not here, it does not appear later.</p>'),
        slide('<h2>2 · Formula</h2>'
              '<p>The relationship, written out. Not a spreadsheet cell reference — the actual '
              'expression, so it can be checked away from the tool.</p>'),
        slide('<h2>3 · Substitution</h2>'
              '<p>The inputs put into the formula. This is the step most textbooks skip, and it '
              'is the step where errors are actually caught.</p>'),
        slide('<h2>4 · Result</h2>'
              '<p>The number, at a stated precision, with its units.</p>'),
        slide('<h2>5 · Interpretation</h2>'
              '<p>What it means, what it does <em>not</em> mean, and what a professional would '
              'do next. A correct number that leads to the wrong decision has failed.</p>'),
        slide('<div class="big">15,613</div>'
              '<p>Machine checks in PFL-AI and PML-AI, all passing. Every printed result is '
              'recomputed from its inputs and compared to the page, and the suite is fail-closed: '
              'a domain does not pass gate until it passes in full.</p>'),
        slide('<div class="kick">Including</div>'
              '<h2>Every multiple-choice option.<br/>Not only the correct one.</h2>'
              '<p>A plausible wrong answer that is arithmetically impossible teaches nothing.</p>'
              '<p class="kick">projectcontrolsinstitute.org</p>', dark=True),
    ],
    "06-reading-the-eac-family": [
        slide('<div class="kick">Earned value</div>'
              '<h1>There is no<br/>such thing as<br/>"the" EAC</h1><div class="rule"></div>'
              '<p>There is a family, and choosing between them is the professional act.</p>',
              dark=True),
        slide('<h2>The question each one answers</h2>'
              '<p>Every estimate at completion embeds an assumption about what the remaining '
              'work will cost. The methods differ in that assumption, not in their arithmetic.</p>'),
        slide('<h2>Budgeted rate</h2>'
              '<div class="calc">EAC = AC + (BAC − EV)</div>'
              '<p>Assumes the overrun so far was one-off and the rest runs to plan.</p>'),
        slide('<h2>Persisting performance</h2>'
              '<div class="calc">EAC = BAC ÷ CPI</div>'
              '<p>Assumes what has happened keeps happening. Usually the honest default.</p>'),
        slide('<h2>Cost and schedule together</h2>'
              '<div class="calc">EAC = AC + (BAC − EV)<br/>          ÷ (CPI × SPI)</div>'
              '<p>Assumes schedule pressure carries a cost. The most pessimistic of the three, '
              'and sometimes the only realistic one.</p>'),
        slide('<h2>The failure</h2>'
              '<p>Reporting one number without saying which assumption produced it. '
              'The board cannot challenge an assumption it was never shown.</p>'),
        slide('<h2>The professional act</h2>'
              '<p>State the method. State why the assumption fits the variance cause. '
              'Show the family where the range matters.</p>'),
        slide('<div class="kick">PCL-AI</div>'
              '<h2>A forecast is<br/>an assumption<br/>made visible</h2>'
              '<p class="kick">projectcontrolsinstitute.org</p>', dark=True),
    ],
}


# ---------------------------------------------------------------- course outline

OUTLINE_CSS = f"""
{BASE}
  @page {{ size: A4; margin: 20mm 22mm 16mm 22mm;
           @bottom-center {{ content: counter(page); font-family: "Linux Biolinum O", sans-serif;
                             font-size: 8pt; color: {MUTED}; }} }}
  @page :first {{ @bottom-center {{ content: none; }} }}
  body {{ font-size: 10pt; line-height: 1.5; }}
  /* Hyphenation off on the title: the credential code is the one string that must never break, and
     "PCL-AI" split across two lines reads as two different things. */
  h1 {{ font-family: "Linux Libertine Display O", serif; font-size: 25pt; color: {GREEN};
        font-weight: normal; line-height: 1.12; margin: 2mm 0 3mm 0; hyphens: none; }}
  .lede {{ font-size: 11pt; color: {MUTED}; margin-bottom: 5mm; }}
  .counts {{ font-family: "Linux Biolinum O", sans-serif; font-variant-caps: small-caps;
             font-feature-settings: "smcp" 1, "onum" 1; letter-spacing: .1em; font-size: 10pt;
             color: {GREEN}; border-top: 2pt solid {GREEN}; border-bottom: 1pt solid {RULE};
             padding: 2.5mm 0; margin-bottom: 6mm; }}
  /* A part opener may not be the last thing on a page: a heading stranded above its first domain
     reads as a mistake, and on a two-page document post it is the first thing anyone notices. */
  h2 {{ font-family: "Linux Libertine Display O", serif; font-size: 15pt; color: {GREEN};
        font-weight: normal; margin: 7mm 0 1mm 0; page-break-after: avoid; }}
  .partnote {{ font-size: 9pt; color: {MUTED}; font-style: italic; margin-bottom: 3.5mm;
               page-break-after: avoid; }}
  h3 {{ font-family: "Linux Biolinum O", sans-serif; font-size: 10.2pt; font-weight: 600;
        color: {INK}; margin: 4.5mm 0 1mm 0; page-break-after: avoid; }}
  h3 .ka {{ font-weight: normal; color: {MUTED}; font-feature-settings: "onum" 1; }}
  .kas {{ font-size: 9.2pt; color: {INK}; margin: 0 0 1.5mm 0; }}
  .why {{ font-size: 9.2pt; color: {MUTED}; font-style: italic; margin: 0 0 1mm 0;
          padding-left: 4mm; border-left: 1pt solid {RULE}; }}
  /* A domain is one unit: its heading, its knowledge areas and the line explaining why it is there
     travel together. Splitting them strands a note at the top of a page with nothing above it to
     say which domain it belongs to. */
  .dom {{ page-break-inside: avoid; }}
  .behind {{ background: {TINT}; border-left: 2pt solid {GREEN}; padding: 4mm 5mm;
             margin-top: 7mm; font-size: 9.2pt; }}
  .behind h2 {{ font-size: 12pt; margin: 0 0 2mm 0; }}
  .principle {{ font-style: italic; color: {GREEN}; font-size: 10.5pt; margin-top: 3mm; }}
  .foot {{ margin-top: 6mm; border-top: 1pt solid {RULE}; padding-top: 2.5mm;
           font-family: "Linux Biolinum O", sans-serif; font-size: 8pt; color: {MUTED}; }}
"""


def _inline(s: str) -> str:
    """Markdown bold/italic to HTML, everything else escaped."""
    s = html.escape(s.strip())
    s = re.sub(r"\*\*(.+?)\*\*", r"<strong>\1</strong>", s)
    return re.sub(r"\*(.+?)\*", r"<em>\1</em>", s)


def outline_html() -> str:
    """Render the outline section of course-outline-pcl-ai.md as the document post.

    Only the part after "## 2 — The outline" is used: everything above it is publishing instructions
    for whoever posts, not content for the reader.
    """
    md = (HERE / "course-outline-pcl-ai.md").read_text(encoding="utf-8")
    body = md.split("## 2 — The outline", 1)[1]
    body = body.split("*Project Controls Institute Global")[0]

    # Paragraph-fold: the source hard-wraps, so runs of lines join into one logical block. But the
    # source also puts a heading, its domain line, its knowledge areas and its note on consecutive
    # lines with no blank between them, so a blank line alone is not enough to tell blocks apart —
    # these markers each begin a new one, and a heading or domain line is always a block by itself.
    STARTS = (re.compile(r"^#{1,6} "), re.compile(r"^\*\*"), re.compile(r"^\*[^*]"))
    SOLO = re.compile(r"^(#{1,6} |\*\*Domain )")
    blocks, buf = [], []

    def flush():
        if buf:
            blocks.append(" ".join(buf))
            buf.clear()

    for line in body.splitlines():
        s = line.strip()
        if s in ("", "---"):
            flush()
            continue
        if any(p.match(s) for p in STARTS):
            flush()
        buf.append(s)
        if SOLO.match(s):
            flush()
    flush()

    PRINCIPLE = "AI proposes"
    out = []
    in_dom = False

    def close_dom():
        nonlocal in_dom
        if in_dom:
            out.append("</div>")
            in_dom = False

    for b in blocks:
        # A new domain, a new part or the standing note all end the domain currently open.
        if b.startswith(("**Domain ", "#", "### What sits behind")):
            close_dom()
        if b.startswith("### What sits behind"):
            # The standing note opens its own boxed section, so it is an h2 inside .behind rather
            # than a second document title.
            out.append(f"<div class='behind'><h2>{_inline(b[4:])}</h2>")
        elif b.startswith("### "):
            out.append(f"<h1>{_inline(b[4:])}</h1>")
        elif b.startswith("#### "):
            out.append(f"<h2>{_inline(b[5:])}</h2>")
        elif b.strip("* ").startswith(PRINCIPLE):
            out.append(f"<p class='principle'>{_inline(b.strip('* '))}</p>")
        elif b.startswith("**Domain "):
            # "**Domain 1 · Title** — 5 knowledge areas"
            m = re.match(r"\*\*(.+?)\*\*\s*—\s*(.+)", b)
            out.append("<div class='dom'>")
            in_dom = True
            out.append(f"<h3>{_inline(m.group(1))} <span class='ka'>— {_inline(m.group(2))}</span></h3>")
        elif b.startswith("*") and b.endswith("*") and not b.startswith("**"):
            cls = "partnote" if "per cent of the Body of Knowledge" in b else "why"
            out.append(f"<p class='{cls}'>{_inline(b.strip('*'))}</p>")
        elif b.startswith("**13 domains"):
            out.append(f"<div class='counts'>{_inline(b)}</div>")
        elif b.startswith("### What sits behind") or b.startswith("What sits behind"):
            out.append(f"<h2>{_inline(b.lstrip('# '))}</h2>")
        elif "·" in b and not b.startswith("**") and len(b) > 60:
            out.append(f"<p class='kas'>{_inline(b)}</p>")
        else:
            out.append(f"<p>{_inline(b)}</p>")

    close_dom()
    doc = "".join(out)
    # The .behind box was opened at the standing-note heading; close it and sign off.
    doc += ("</div><div class='foot'>Project Controls Institute Global · "
            "projectcontrolsinstitute.org</div>")
    return f"<style>{OUTLINE_CSS}</style>{doc}"


def main() -> None:
    OUT.mkdir(exist_ok=True)
    from weasyprint import HTML

    HTML(string=ONEPAGER).write_pdf(str(OUT / "PCI-Overview-Onepager.pdf"))
    print("built: PCI-Overview-Onepager.pdf")

    path = OUT / "PCI-PCL-AI-Course-Outline.pdf"
    HTML(string=outline_html(), base_url=str(HERE)).write_pdf(str(path))
    print(f"built: {path.name}")

    for name, slides in CAROUSELS.items():
        doc = f"<style>{CAROUSEL_CSS}</style>" + "".join(slides)
        path = OUT / f"carousel-{name}.pdf"
        HTML(string=doc).write_pdf(str(path))
        print(f"built: {path.name} ({len(slides)} slides)")


if __name__ == "__main__":
    main()
