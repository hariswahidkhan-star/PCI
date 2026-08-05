#!/usr/bin/env python3
"""Build the PCL-AI course outline as a LinkedIn document post, in the Institute's brand.

**Brand.** Taken from `wwwroot/assets/logo.svg`, which is the only brand mark in the repository and
therefore the authority: a navy field (#1D3C92 → #13245A), the word AI in a gold gradient
(#F7EABC → #E7CB82 → #B8923E), and a crimson bar (#C13329) beneath the wordmark. The crimson bar is
the signature device and it is reproduced here as the rule under every heading. `styles.css` carries
the same crimson as `--crimson`. An earlier cut of this deck used the site's link blue (#1D4ED8)
with no gold and no crimson — that is the colour of a hyperlink, not of an institution.

**Shape.** LinkedIn renders a document at its own aspect ratio inside a fixed-width column, so an A4
page arrives with body text around 9 px on a phone. These slides are 1080 x 1350 (4:5), the tallest
ratio the feed shows without cropping.

**Substance.** A syllabus that lists topic titles proves nothing; anyone can list titles. The
formula slides carry the actual arithmetic the volume teaches — the four EAC methods and what each
one assumes, the earned-schedule pair, the variance decomposition, the cash conversion cycle, and
the precision/recall/F1 set that decides whether an AI control is worth running. Every formula is
extracted from the manuscripts by `formulas()` below rather than typed here, so none of them can be
wrong in a way the volume is not.

Usage:  python3 build_outline_post.py
"""
import html
import pathlib
import re
import sys

sys.path.insert(0, str(pathlib.Path(__file__).resolve().parent))
import charts

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parents[1]
OUT = HERE / "assets"
FONTS = (ROOT / "backend/wwwroot/assets/fonts").resolve()
BOK = ROOT / "docs/bok"

# ---- brand, from wwwroot/assets/logo.svg ------------------------------------------------------
NAVY = "#13245A"        # logo field, dark stop
NAVY_MID = "#1D3C92"    # logo field, light stop
GOLD = "#E7CB82"        # logo "AI", mid stop
GOLD_PALE = "#F7EABC"   # logo "AI", light stop
GOLD_DEEP = "#B8923E"   # logo "AI", dark stop
CRIMSON = "#C13329"     # logo accent bar; styles.css --crimson
INK = "#0F172A"
PAPER = "#FFFFFF"
PAPER_2 = "#F5F7FA"
LINE = "#E3E8EF"
SLATE = "#475569"
MIST = "#64748B"

FIELD = f"linear-gradient(158deg,{NAVY_MID} 0%,{NAVY} 78%)"
GOLD_GRAD = f"linear-gradient(180deg,{GOLD_PALE} 0%,{GOLD} 45%,{GOLD_DEEP} 100%)"

W, H = 1080, 1350

CSS = f"""
@font-face {{ font-family: Archivo; src: url(file://{FONTS}/archivo-latin.woff2) format('woff2');
              font-weight: 700 900; }}
@font-face {{ font-family: Inter; src: url(file://{FONTS}/inter-latin.woff2) format('woff2');
              font-weight: 400 700; }}

@page {{ size: {W}pt {H}pt; margin: 0; }}
* {{ box-sizing: border-box; }}
body {{ margin: 0; font-family: Inter, sans-serif; color: {INK}; }}
h1, h2 {{ font-family: Archivo, sans-serif; font-weight: 900; letter-spacing: -.032em;
          line-height: 1.02; margin: 0; }}

.slide {{ width: {W}pt; height: {H}pt; padding: 84pt 82pt 62pt 82pt;
          display: flex; flex-direction: column; page-break-after: always; background: {PAPER}; }}
.slide.navy {{ background: {FIELD}; color: {PAPER}; }}
.slide.tint {{ background: {PAPER_2}; }}
.body {{ flex: 1 1 auto; display: flex; flex-direction: column; justify-content: center; }}
.body > *:first-child {{ margin-top: 0; }}
.body > *:last-child {{ margin-bottom: 0; }}

.eyebrow {{ font-size: 17pt; font-weight: 700; letter-spacing: .17em; text-transform: uppercase;
            color: {GOLD_DEEP}; margin-bottom: 16pt; }}
.slide.navy .eyebrow {{ color: {GOLD}; }}

h1 {{ font-size: 86pt; }}
h2 {{ font-size: 58pt; }}
/* Solid gold, not the logo's gradient: WeasyPrint has no `background-clip: text`, so a gradient
   fill paints the box and leaves the letters transparent — a gold brick where the word should be.
   The logo's mid stop carries the same colour at display size. */
.gold {{ color: {GOLD}; }}

/* The crimson bar from the logo, reused as the rule under a heading. It is the one mark that says
   this is the Institute and not a template, so it appears on every slide and never changes width. */
.bar {{ width: 118pt; height: 7pt; background: {CRIMSON}; margin: 26pt 0 30pt 0; }}

.lede {{ font-size: 24pt; line-height: 1.45; color: {SLATE}; }}
.slide.navy .lede {{ color: #C9D4EC; }}
.lede b {{ color: {GOLD}; font-weight: 700; }}

.figs {{ display: flex; gap: 26pt; }}
.fig {{ flex: 1; }}
.fig .n {{ font-family: Archivo, sans-serif; font-weight: 900; font-size: 78pt; color: {NAVY_MID};
           letter-spacing: -.045em; line-height: 1; }}
.slide.navy .fig .n {{ color: {GOLD}; }}
.fig .l {{ font-size: 16pt; color: {MIST}; margin-top: 6pt; font-weight: 500; line-height: 1.3; }}
.slide.navy .fig .l {{ color: #B6C4E2; }}

/* Domain rows: number and knowledge-area count in a left rail, so the longest title still gets the
   full measure. */
.dom {{ display: flex; gap: 22pt; padding: 17pt 0; border-top: 1.5pt solid {LINE}; }}
.dom:last-child {{ border-bottom: 1.5pt solid {LINE}; }}
.dom .rail {{ width: 58pt; flex: 0 0 58pt; }}
.dom .no {{ font-family: Archivo, sans-serif; font-weight: 900; font-size: 28pt;
            color: {NAVY_MID}; letter-spacing: -.03em; line-height: 1; }}
.dom .count {{ font-size: 12.5pt; color: {MIST}; font-weight: 600; margin-top: 4pt; }}
.dom .t {{ flex: 1; }}
.dom .name {{ font-family: Archivo, sans-serif; font-weight: 800; font-size: 23pt;
              letter-spacing: -.022em; line-height: 1.14; color: {INK}; }}
.dom .kas {{ font-size: 16.5pt; line-height: 1.44; color: {SLATE}; margin-top: 8pt; }}

/* Formula rows. Tabular numerals and a touch of tracking make an equation read as an equation
   without importing a monospace face that is not in the brand. */
.frm {{ display: flex; align-items: baseline; gap: 20pt; padding: 15pt 0;
        border-top: 1pt solid rgba(255,255,255,.16); }}
.frm:last-child {{ border-bottom: 1pt solid rgba(255,255,255,.16); }}
.slide:not(.navy) .frm {{ border-color: {LINE}; }}
.frm .e {{ font-family: Inter, sans-serif; font-weight: 700; font-size: 24pt; color: {GOLD};
           font-variant-numeric: tabular-nums; letter-spacing: .005em; white-space: nowrap; }}
.slide:not(.navy) .frm .e {{ color: {NAVY_MID}; }}
.frm .d {{ font-size: 17.5pt; color: #B6C4E2; line-height: 1.35; text-align: right; flex: 1; }}
.slide:not(.navy) .frm .d {{ color: {MIST}; }}
/* A stacked row gives the formula the full measure, so it may wrap; the inline layout keeps
   nowrap because a two-column row has nowhere to wrap to. Without this the longest equations —
   capitalised borrowing cost, the target-cost fee — run clean off the page edge. */
.frm.stack {{ display: block; }}
.frm.stack .e {{ white-space: normal; line-height: 1.25; }}
.frm.stack .d {{ text-align: left; margin-top: 7pt; }}

.callout {{ background: {PAPER_2}; border-left: 7pt solid {CRIMSON}; padding: 26pt 30pt;
            font-size: 20pt; line-height: 1.5; color: {SLATE}; margin-top: 26pt; }}
.slide.navy .callout {{ background: rgba(255,255,255,.07); color: #D7E0F2; }}
.callout strong {{ color: {INK}; font-weight: 700; }}
.slide.navy .callout strong {{ color: {PAPER}; }}

.point {{ font-size: 21pt; line-height: 1.5; color: {SLATE}; margin: 0 0 16pt 0; }}
.point b {{ color: {INK}; font-weight: 700; }}
.slide.navy .point {{ color: #C9D4EC; }}
.slide.navy .point b {{ color: {PAPER}; }}

.foot {{ flex: 0 0 auto; display: flex; justify-content: space-between; align-items: baseline;
         margin-top: 30pt; padding-top: 15pt; border-top: 1.5pt solid {LINE};
         font-size: 13.5pt; color: {MIST}; font-weight: 500; }}
.slide.navy .foot {{ border-color: rgba(231,203,130,.32); color: #B6C4E2; }}
.slide.tint .foot {{ border-color: #DDE3EB; }}
.foot .pg {{ font-variant-numeric: tabular-nums; }}

/* Two-column comparison: the whole argument for the credential fits in one slide, so it gets a
   layout of its own rather than being squeezed into prose. */
.cols {{ display: flex; gap: 30pt; margin-top: 6pt; }}
.col {{ flex: 1; padding: 24pt 24pt 26pt 24pt; background: rgba(255,255,255,.06);
        border-top: 4pt solid {GOLD}; }}
.slide:not(.navy) .col {{ background: {PAPER_2}; border-top-color: {NAVY_MID}; }}
.col h3 {{ font-family: Archivo, sans-serif; font-weight: 900; font-size: 25pt; margin: 0 0 4pt 0;
           letter-spacing: -.025em; color: {PAPER}; }}
.slide:not(.navy) .col h3 {{ color: {INK}; }}
.col .sub {{ font-size: 14pt; color: {GOLD}; font-weight: 700; letter-spacing: .06em;
             text-transform: uppercase; margin-bottom: 14pt; }}
.slide:not(.navy) .col .sub {{ color: {NAVY_MID}; }}
.col ul {{ margin: 0; padding-left: 19pt; }}
.col li {{ font-size: 16.5pt; line-height: 1.4; margin-bottom: 8pt; color: #C9D4EC; }}
.slide:not(.navy) .col li {{ color: {SLATE}; }}
.col .gap {{ font-size: 16.5pt; line-height: 1.42; color: #8FA3C8;
             margin-top: 14pt; padding-top: 12pt; border-top: 1pt solid rgba(255,255,255,.18); }}
.slide:not(.navy) .col .gap {{ color: {MIST}; border-top-color: {LINE}; }}

/* Named standards, as a plain register: the reader should be able to count them. */
.std {{ display: flex; gap: 18pt; padding: 13pt 0; border-top: 1pt solid {LINE}; }}
.slide.navy .std {{ border-top-color: rgba(255,255,255,.16); }}
.std:last-child {{ border-bottom: 1pt solid {LINE}; }}
.slide.navy .std:last-child {{ border-bottom-color: rgba(255,255,255,.16); }}
.std .k {{ font-family: Archivo, sans-serif; font-weight: 800; font-size: 20pt; color: {NAVY_MID};
           width: 152pt; flex: 0 0 152pt; letter-spacing: -.02em; }}
.slide.navy .std .k {{ color: {GOLD}; }}
.std .v {{ font-size: 17pt; line-height: 1.35; color: {SLATE}; flex: 1; }}
.slide.navy .std .v {{ color: #C9D4EC; }}
/* The figures carry a viewBox and no intrinsic size, so they scale to the measure. Left at their
   authored width they render at 880 CSS px = 660 pt inside a 916 pt column — about 72% of the
   slide, which reads as a thumbnail dropped on the page rather than as the slide's subject. */
.fig-wrap {{ margin: 4pt 0 10pt 0; }}
.fig-wrap svg {{ display: block; width: 100%; height: auto; }}
.swipe {{ font-size: 17pt; font-weight: 700; color: {GOLD}; margin-top: 28pt;
          display: flex; align-items: center; gap: 10pt; }}
"""


def esc(s):
    return html.escape(s)


# ---- content pulled from the manuscripts ------------------------------------------------------

def formulas() -> dict:
    """Every symbolic formula in the PCL-AI manuscripts, keyed by domain number.

    Worked instances are dropped — a slide wants EAC = BAC / CPI, not one project's arithmetic —
    by rejecting anything carrying a multi-digit or decimal literal.

    Code spans are read by splitting each line on backticks and taking the odd segments, not with a
    regex. A regex backtracks across an unmatched span and starts its next match on a *closing*
    backtick, which silently swallows the span after it — that is how SV(t) = ES - AT went missing
    from a line that plainly contains it. Fenced blocks are read too: the earned-schedule
    definitions live in one.
    """
    found = {}
    for f in sorted(BOK.glob("domain-*.md")):
        d = int(re.match(r"domain-(\d+)", f.name).group(1))
        keep, fenced = set(), False
        for line in f.read_text().splitlines():
            if line.lstrip().startswith("```"):
                fenced = not fenced
                continue
            spans = [line] if fenced else line.split("`")[1::2]
            for raw in spans:
                s = " ".join(raw.split())
                s = re.sub(r"\s*\((?:in|e\.g\.)[^)]*\)\s*$", "", s)  # trailing unit glosses
                if "=" not in s or not re.match(r"^[A-Za-z(]", s):
                    continue
                if re.search(r"\d{2,}|\d+\.\d|\d\s*%", s):
                    continue
                keep.add(s)
        found[d] = keep
    return found


def pick(pool: set, *wanted) -> list:
    """Return each wanted formula, asserting the manuscripts really contain it.

    The assertion is the point: if a rewrite changes a formula, this build fails rather than
    publishing a slide the volume no longer supports.
    """
    out = []
    for w in wanted:
        norm = {" ".join(p.replace(" ", "")) for p in pool}
        if not any(p.replace(" ", "") == w.replace(" ", "") for p in pool):
            raise SystemExit(f"formula not found in the manuscripts: {w!r}")
        out.append(w)
    return out


def load_domains() -> dict:
    """Domain number -> (title, 'n knowledge areas', 'KA · KA · KA'), from the outline markdown."""
    md = (HERE / "course-outline-pcl-ai.md").read_text(encoding="utf-8")
    body = md.split("## 2 — The outline", 1)[1]
    blocks, buf = [], []
    for raw in body.splitlines():
        s = raw.strip()
        if s in ("", "---"):
            if buf:
                blocks.append(" ".join(buf)); buf = []
            continue
        if s.startswith(("#", "**", "*")) and buf:
            blocks.append(" ".join(buf)); buf = []
        buf.append(s)
        if s.startswith(("#", "**Domain ")):
            blocks.append(" ".join(buf)); buf = []
    if buf:
        blocks.append(" ".join(buf))

    doms, cur = {}, None
    for b in blocks:
        m = re.match(r"\*\*Domain (\d+) · (.+?)\*\*\s*—\s*(.+)", b)
        if m:
            cur = int(m.group(1))
            doms[cur] = [m.group(2), m.group(3), ""]
        elif cur and not doms[cur][2] and "·" in b and not b.startswith("*"):
            doms[cur][2] = b
    return doms


# ---- slide construction -----------------------------------------------------------------------

ARROW = ("<svg width='30' height='13' viewBox='0 0 30 13' fill='none'>"
         "<path d='M0 6.5h27M21.5 1l6 5.5-6 5.5' stroke='" + GOLD + "' stroke-width='2'"
         " stroke-linecap='round' stroke-linejoin='round'/></svg>")


def slide(body, page, cls=""):
    c = ("slide " + cls).strip()
    return (f"<div class='{c}'><div class='body'>{body}</div>"
            f"<div class='foot'><span>Project Controls Institute Global</span>"
            f"<span class='pg'>{page}</span></div></div>")


def head(eyebrow, title):
    return f"<div class='eyebrow'>{eyebrow}</div><h2>{title}</h2><div class='bar'></div>"


def dom_row(no, d):
    name, count, kas = d
    return (f"<div class='dom'><div class='rail'><div class='no'>{no}</div>"
            f"<div class='count'>{count.split()[0]} KAs</div></div>"
            f"<div class='t'><div class='name'>{esc(name)}</div>"
            f"<div class='kas'>{esc(kas)}</div></div></div>")


def frm(expr, note, stack=False):
    cls = "frm stack" if stack else "frm"
    return f"<div class='{cls}'><div class='e'>{esc(expr)}</div><div class='d'>{esc(note)}</div></div>"


def std(code, what):
    return f"<div class='std'><div class='k'>{esc(code)}</div><div class='v'>{esc(what)}</div></div>"


def build():
    D = load_domains()
    assert len(D) == 13, f"expected 13 domains, parsed {len(D)}"
    F = formulas()
    S = []

    # 1 — cover. The credential's whole argument is the title.
    S.append(slide(
        "<div class='eyebrow'>PCI PCL-AI &middot; course outline</div>"
        "<h1>Finance and<br/>delivery.<br/><span class='gold'>One profession.</span></h1>"
        "<div class='bar'></div>"
        "<div class='lede'>The syllabus for the AI Project Controls Leader — the credential that "
        "examines the accounting <b>and</b> the arithmetic of delivery, because a project needs "
        "both and almost nobody is trained in both.</div>"
        f"<div class='swipe'><span>13 domains &middot; 61 knowledge areas</span>{ARROW}</div>",
        "PCL-AI", "navy"))

    # 2 — the thesis. Two professions, and the ground neither of them is examined on.
    S.append(slide(
        head("Why this credential exists", "Two professions.<br/>One project.")
        + "<div class='cols'>"
          "<div class='col'><div class='sub'>The accountant</div>"
          "<h3>Knows the money</h3><ul>"
          "<li>When revenue may be recognised</li>"
          "<li>What a provision must satisfy</li>"
          "<li>Which costs may be capitalised</li></ul>"
          "<div class='gap'>Rarely examined on a critical path, an earning rule, or why a schedule "
          "slip becomes a cost.</div></div>"
          "<div class='col'><div class='sub'>The engineer</div>"
          "<h3>Knows the work</h3><ul>"
          "<li>Where the critical path runs</li>"
          "<li>What the float really is</li>"
          "<li>How progress is measured</li></ul>"
          "<div class='gap'>Rarely examined on cut-off, a contract asset, or the difference between "
          "billed and earned.</div></div>"
          "</div>"
          "<div class='callout'>A project lives in the overlap. <strong>The overlap is where the "
          "money is lost</strong>, and it is the one place neither training looks.</div>",
        "1 / 14", "navy"))

    # The shape
    S.append(slide(
        head("The syllabus", "Forty, forty,<br/>twenty")
        + f"<div class='fig-wrap'>{charts.weight_bar()}</div>"
        + "<div class='figs'>"
          "<div class='fig'><div class='n'>13</div><div class='l'>domains</div></div>"
          "<div class='fig'><div class='n'>61</div><div class='l'>knowledge areas</div></div>"
          "<div class='fig'><div class='n'>26</div><div class='l'>sector case studies</div></div>"
          "</div>",
        "3 / 14"))

    # 5 — the finance half, named. This is the half most delivery credentials do not have.
    S.append(slide(
        head("The finance half", "Standards a<br/>controls lead<br/>is examined on")
        + std("IFRS 15", "revenue from contracts with customers — the five-step model, over-time "
                         "recognition, variable consideration and the constraint")
        + std("IAS 37", "provisions and contingent liabilities — behind every warranty reserve, "
                        "onerous-contract charge and dispute disclosure")
        + std("IAS 2", "the materials and work-in-progress a project holds in store")
        + std("IAS 16 / IFRS 16", "the plant a project owns, and the plant and premises it leases")
        + std("IAS 23", "the financing cost of building a major asset")
        + std("IAS 1", "presentation of financial statements")
        + std("IAS 11", "the construction-contracts standard IFRS 15 replaced — covered so legacy "
                        "terminology in an older file still reads"),
        "4 / 14", "navy"))

    # 6 — finance formulas: the IFRS 15 recognition chain, end to end
    S.append(slide(
        head("Domain 2 &middot; revenue", "The recognition<br/>chain, in full")
        + frm(*pick(F[2], "Percentage of completion (PoC) = Costs incurred to date / Total estimated costs")[:1],
              "over-time recognition by the input method", stack=True)
        + frm(*pick(F[2], "Cumulative revenue = PoC × Transaction price")[:1],
              "revenue earned to date", stack=True)
        + frm(*pick(F[2], "Period revenue = Cumulative revenue − revenue recognised in prior periods")[:1],
              "what lands in this month", stack=True)
        + frm(*pick(F[2], "Contract asset (liability) = cumulative revenue − cumulative billed")[:1],
              "over- or under-billing, on the balance sheet", stack=True)
        + "<div class='callout'>Recognition is not billing. A project can be profitable, fully "
          "billed and <strong>still carry a contract liability</strong>.</div>",
        "5 / 14", "tint"))

    # 7 — finance formulas: allocation, measurement, capitalisation
    S.append(slide(
        head("Domains 1&ndash;2 &middot; measurement", "Before the<br/>number exists")
        + frm(*pick(F[1], "Assets = Liabilities + Equity")[:1], "the identity every ledger satisfies")
        + frm(*pick(F[2], "catch-up = PoC × revised price − revenue recognised to date")[:1],
              "the cumulative catch-up when the price is revised", stack=True)
        + frm(*pick(F[2], "Revised transaction price = fixed price + constrained variable consideration")[:1],
              "variable consideration, constrained", stack=True)
        + frm(*pick(F[2], "Capitalised borrowing cost = weighted-average qualifying expenditure × capitalisation rate")[:1],
              "IAS 23 — financing cost inside the asset", stack=True)
        + frm(*pick(F[2], "NRV = estimated selling price − costs to complete and sell")[:1],
              "IAS 2 — the write-down test on stored materials", stack=True),
        "6 / 14", "navy"))

    # 8 — commercial: where the contract becomes cash
    S.append(slide(
        head("Domain 7 &middot; commercial", "Contract<br/>to cash")
        + frm(*pick(F[7], "Amount = quantity × rate")[:1],
              "the bill-of-quantities line, before anything else happens", stack=True)
        + frm(*pick(F[7], "amount due = gross value − retention − previous payments")[:1],
              "the payment application", stack=True)
        + frm(*pick(F[7], "Fee = target fee + contractor's share × (target cost − actual cost)")[:1],
              "target-cost pain/gain share", stack=True)
        + frm(*pick(F[7], "LD exposure = LD rate × forecast days late")[:1],
              "liquidated damages, forecast not incurred", stack=True)
        + frm(*pick(F[11], "CCC = DSO + DIO − DPO")[:1],
              "cash conversion cycle, in days", stack=True),
        "7 / 14", "tint"))

    # 9 — the delivery half: EVM
    S.append(slide(
        head("Domain 6 &middot; earned value", "The delivery<br/>half")
        + frm(*pick(F[6], "CV = EV − AC")[:1], "cost variance")
        + frm(*pick(F[6], "SV = EV − PV")[:1], "schedule variance, in money")
        + frm(*pick(F[6], "CPI = EV / AC")[:1], "cost performance index")
        + frm(*pick(F[6], "SPI = EV / PV")[:1], "schedule performance index")
        + frm(*pick(F[6], "SV(t) = ES − AT")[:1], "earned schedule variance, in time")
        + frm(*pick(F[6], "SPI(t) = ES / AT")[:1], "the index that still works near completion"),
        "8 / 14", "navy"))

    # Variance, agile, risk
    S.append(slide(
        head("Domains 4, 9, 12", "Why the number<br/>moved")
        + frm(*pick(F[4], "Price/rate = (actual price − standard price) × actual quantity")[:1],
              "price variance", stack=True)
        + frm(*pick(F[4], "usage/efficiency = (actual quantity − standard quantity) × standard price")[:1],
              "usage variance — the other half of the story", stack=True)
        + frm(*pick(F[9], "EV = (points done / total points) × BAC")[:1],
              "earned value when scope is a backlog", stack=True)
        + frm(*pick(F[12], "EMV = probability × impact")[:1],
              "expected monetary value — how risk enters a forecast", stack=True),
        "10 / 14", "navy"))

    # 12–13 — the syllabus itself
    S.append(slide(
        head("Part one &middot; 40%", "Finance, accounting<br/>and reporting")
        + "".join(dom_row(i, D[i]) for i in (1, 2, 3, 4)),
        "11 / 14"))
    S.append(slide(
        head("Part two &middot; 40%", "Project management")
        + "".join(dom_row(i, D[i]) for i in (5, 6, 7, 8)),
        "12 / 14"))
    S.append(slide(
        head("Part two &middot; continued", "Delivery, schedule,<br/>process and risk")
        + "".join(dom_row(i, D[i]) for i in (9, 10, 11, 12)),
        "13 / 14"))

    # 14 — AI, and the test of whether a model earns its place
    S.append(slide(
        head("Part three &middot; 20%", "AI, governed")
        + dom_row(13, D[13])
        + frm(*pick(F[13], "F1 = 2 × (precision × recall) ÷ (precision + recall)")[:1],
              "whether the model is worth running at all", stack=True)
        + "<div class='callout'>A model with 99% recall and 4% precision buries the team in false "
          "positives. <strong>&ldquo;The AI found something&rdquo; is not a control.</strong></div>",
        "14 / 14", "tint"))

    # 15 — what holds the teaching up
    S.append(slide(
        head("Behind the syllabus", "113 standards.<br/>532 process<br/>requirements.")
        + "<div class='point'>Each states its purpose, who owns the decision, what evidence must "
          "exist, what practice is prohibited, what triggers escalation, <b>what AI may never "
          "decide, approve or certify</b> — and a compliance test an assessor can perform.</div>"
          "<div class='point'>They are certification requirements established by the Institute. "
          "They are not legislation, and nothing here is legal, tax or accounting advice. External "
          "standards are named and described in the Institute's own words, never reproduced.</div>",
        "PCL-AI", "navy"))

    # 16 — close
    S.append(slide(
        "<div class='eyebrow'>The principle behind all of it</div>"
        "<h2>AI proposes.<br/>The professional<br/>verifies, decides<br/>and remains "
        "<span class='gold'>accountable</span>.</h2>"
        "<div class='bar'></div>"
        "<div class='lede'>The full outline and the three Bodies of Knowledge — "
        "projectcontrolsinstitute.org</div>",
        "PCL-AI", "navy"))

    total = len(S)
    for i, s in enumerate(S):
        S[i] = re.sub(r"<span class='pg'>[^<]*</span>",
                      f"<span class='pg'>{i + 1} / {total}</span>", s)
    return f"<style>{CSS}</style>" + "".join(S)


def assert_on_brand(doc: str) -> None:
    """Fail the build if any visible character is missing from the brand fonts.

    A glyph the brand woff2 files do not carry does not error — WeasyPrint quietly substitutes
    whatever the system has, so an off-brand face lands on a slide and nothing says so. That is how
    a DejaVu arrow reached the cover and a DejaVu sigma reached two formula slides. Checking the
    document text against the fonts' own cmaps turns a silent substitution into a failed build.
    """
    from fontTools.ttLib import TTFont
    covered = set()
    for name in ("inter-latin", "inter-latin-ext", "archivo-latin", "archivo-latin-ext"):
        covered |= set(TTFont(str(FONTS / f"{name}.woff2")).getBestCmap())
    text = re.sub(r"<[^>]+>", "", re.sub(r"<style>.*?</style>", "", doc, flags=re.S))
    text = html.unescape(text)
    missing = {c for c in text if ord(c) not in covered and c not in " \n\t\r"}
    if missing:
        raise SystemExit("glyphs not in the brand fonts: "
                         + ", ".join(f"{c!r} U+{ord(c):04X}" for c in sorted(missing)))


def main():
    OUT.mkdir(exist_ok=True)
    from weasyprint import HTML
    path = OUT / "PCI-PCL-AI-Course-Outline-LinkedIn.pdf"
    doc = build()
    assert_on_brand(doc)
    HTML(string=doc, base_url=str(HERE)).write_pdf(str(path))
    print(f"built: {path.name}")


if __name__ == "__main__":
    sys.exit(main())
