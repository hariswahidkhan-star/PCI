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
.frm.stack {{ display: block; }}
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


def build():
    D = load_domains()
    assert len(D) == 13, f"expected 13 domains, parsed {len(D)}"
    F = formulas()
    S = []

    # 1 — cover
    S.append(slide(
        "<div class='eyebrow'>Course outline &middot; 2026</div>"
        "<h1>Project<br/>Controls,<br/><span class='gold'>governed</span></h1>"
        "<div class='bar'></div>"
        "<div class='lede'>The full PCI PCL-AI syllabus. 13 domains, 61 knowledge areas, and the "
        "arithmetic each one is examined on.</div>"
        f"<div class='swipe'><span>The formulas are in here</span>{ARROW}</div>",
        "PCL-AI", "navy"))

    # 2 — the hook: why the syllabus is shaped this way.
    # 2,200,000 / 1,850,000 = 1.19; with the 240,000 accrual, 2,200,000 / 2,090,000 = 1.05.
    S.append(slide(
        head("The problem", "A CPI of 1.19<br/>and 1.05 from<br/>the same month")
        + "<div class='point'>Earned value 2,200,000. Invoiced cost 1,850,000. Skip the accrual and "
          "<b>CPI = 1.19</b>. Book the 240,000 of work already done and <b>CPI = 1.05</b>.</div>"
          "<div class='point'>Fourteen points on one missing entry, and next month the un-accrued "
          "version shows an overrun that never happened.</div>"
          "<div class='callout'>Cut-off is not bookkeeping hygiene. It is the difference between a "
          "performance index that means something and one that <strong>whipsaws with invoice "
          "timing</strong>.</div>",
        "1 / 12", "tint"))

    # 3 — scale
    S.append(slide(
        head("What the syllabus covers", "Thirteen domains.<br/>No filler.")
        + "<div class='figs'>"
          "<div class='fig'><div class='n'>13</div><div class='l'>domains</div></div>"
          "<div class='fig'><div class='n'>61</div><div class='l'>knowledge areas</div></div>"
          "<div class='fig'><div class='n'>26</div><div class='l'>sector case studies</div></div>"
          "</div>"
          "<div class='callout'>Every technique arrives with <strong>the conditions under which it "
          "fails</strong>. A method you cannot break is a method you do not yet understand.</div>",
        "2 / 12"))

    # 4 — Part One
    S.append(slide(
        head("Part one &middot; 40% of the Body of Knowledge",
             "Finance, accounting<br/>and reporting")
        + "".join(dom_row(i, D[i]) for i in (1, 2, 3, 4)),
        "3 / 12"))

    # 5 — formulas: recognition
    S.append(slide(
        head("Domains 1&ndash;2 &middot; what you are examined on", "Getting the<br/>number right")
        + frm(*pick(F[1], "Assets = Liabilities + Equity")[:1],
              "the identity every ledger must satisfy")
        + frm(*pick(F[2], "PoC = cost to date / total estimated cost")[:1],
              "cost-to-cost percentage of completion")
        + frm(*pick(F[2], "revenue = PoC × transaction price")[:1],
              "revenue for the period to date")
        + frm(*pick(F[2], "period revenue = cumulative − prior")[:1],
              "what actually lands in this month")
        + frm(*pick(F[2], "Contract asset (liability) = cumulative revenue − cumulative billed")[:1],
              "over- or under-billing, on the balance sheet", stack=True)
        + "<div class='callout'>Recognition is not billing. A project can be profitable, fully "
          "billed and <strong>still carry a contract liability</strong>.</div>",
        "4 / 12", "tint"))

    # 6 — Part Two, first half
    S.append(slide(
        head("Part two &middot; 40% of the Body of Knowledge", "Project management")
        + "".join(dom_row(i, D[i]) for i in (5, 6, 7, 8)),
        "5 / 12"))

    # 7 — formulas: EVM core
    S.append(slide(
        head("Domain 6 &middot; earned value", "Four measures,<br/>one month-end")
        + frm(*pick(F[6], "CV = EV − AC")[:1], "cost variance")
        + frm(*pick(F[6], "SV = EV − PV")[:1], "schedule variance, in money")
        + frm(*pick(F[6], "CPI = EV / AC")[:1], "cost performance index")
        + frm(*pick(F[6], "SPI = EV / PV")[:1], "schedule performance index")
        + frm(*pick(F[6], "SV(t) = ES − AT")[:1], "earned schedule variance, in time")
        + frm(*pick(F[6], "SPI(t) = ES / AT")[:1], "the index that still works near completion"),
        "6 / 12", "navy"))

    # 8 — formulas: the EAC family, the flagship
    S.append(slide(
        head("Domain 6 &middot; the heart of it", "There is no<br/>&lsquo;the&rsquo; EAC")
        + frm(*pick(F[6], "EAC = AC + (BAC − EV)")[:1],
              "the overrun was one-off; the rest runs to plan", stack=True)
        + frm(*pick(F[6], "EAC = BAC / CPI")[:1],
              "performance to date persists — usually the honest default", stack=True)
        + frm(*pick(F[6], "EAC = AC + (BAC − EV)/(CPI × SPI)")[:1],
              "cost and schedule pressure both continue", stack=True)
        + frm(*pick(F[6], "EAC = AC + ETC")[:1],
              "the team re-estimated the remaining work bottom-up", stack=True)
        + "<div class='callout'>Four methods. Four assumptions. Four different answers from the "
          "same data. <strong>Naming which one you used, and why, is the skill.</strong></div>",
        "7 / 12", "tint"))

    # 9 — Part Two, second half
    S.append(slide(
        head("Part two &middot; continued", "Delivery, schedule,<br/>process and risk")
        + "".join(dom_row(i, D[i]) for i in (9, 10, 11, 12)),
        "8 / 12"))

    # 10 — formulas: variance, agile, cash, risk
    S.append(slide(
        head("Domains 4, 9, 11, 12", "Where the money<br/>actually moves")
        + frm(*pick(F[4], "Price/rate = (actual price − standard price) × actual quantity")[:1],
              "price variance", stack=True)
        + frm(*pick(F[4], "usage/efficiency = (actual quantity − standard quantity) × standard price")[:1],
              "usage variance — the other half of the story", stack=True)
        + frm(*pick(F[9], "EV = (points done / total points) × BAC")[:1],
              "earned value when scope is a backlog", stack=True)
        + frm(*pick(F[11], "CCC = DSO + DIO − DPO")[:1],
              "cash conversion cycle, in days", stack=True)
        + frm(*pick(F[12], "EMV = probability × impact")[:1],
              "expected monetary value — how risk enters a forecast", stack=True),
        "9 / 12", "navy"))

    # 11 — Part Three
    S.append(slide(
        head("Part three &middot; 20% of the Body of Knowledge", "AI, governed")
        + dom_row(13, D[13])
        + "<div class='callout'>The largest domain in the volume, and the reason the credential "
          "exists in this form. It ends where every automated output should end: with "
          "<strong>a named human who verified it</strong>.</div>",
        "10 / 12"))

    # 12 — formulas: does the AI control actually work
    S.append(slide(
        head("Domain 13 &middot; the part most courses skip", "Is the model<br/>worth running?")
        + frm(*pick(F[13], "precision = true hits ÷ total flags")[:1],
              "of everything it flagged, how much was real", stack=True)
        + frm(*pick(F[13], "recall = true hits ÷ total true cases")[:1],
              "of everything real, how much it caught", stack=True)
        + frm(*pick(F[13], "F1 = 2 × (precision × recall) ÷ (precision + recall)")[:1],
              "the single number that refuses to let you game either", stack=True)
        + frm(*pick(F[13], "net = annual saving − annual cost")[:1],
              "the only test a sponsor actually asks", stack=True)
        + "<div class='callout'>A model with 99% recall and 4% precision buries your team in false "
          "positives. <strong>&ldquo;The AI found something&rdquo; is not a control.</strong></div>",
        "11 / 12", "tint"))

    # 13 — the standards
    S.append(slide(
        head("What sits behind the syllabus", "113 standards.<br/>532 process<br/>requirements.")
        + "<div class='point'>Each states its purpose, who owns the decision, what evidence must "
          "exist, what practice is prohibited, what triggers escalation, <b>what AI may never "
          "decide, approve or certify</b> — and a compliance test an assessor can perform.</div>"
        "<div class='point'>They are certification requirements established by the Institute. They "
        "are not legislation, and nothing here is legal, tax or accounting advice.</div>",
        "12 / 12", "navy"))

    # 14 — close
    S.append(slide(
        "<div class='eyebrow'>The principle behind all of it</div>"
        "<h2>AI proposes.<br/>The professional<br/>verifies, decides<br/>and remains "
        "<span class='gold'>accountable</span>.</h2>"
        "<div class='bar'></div>"
        "<div class='lede'>The full outline and the three Bodies of Knowledge — "
        "projectcontrolsinstitute.org</div>",
        "PCL-AI", "navy"))

    return f"<style>{CSS}</style>" + "".join(S)


def main():
    OUT.mkdir(exist_ok=True)
    from weasyprint import HTML
    path = OUT / "PCI-PCL-AI-Course-Outline-LinkedIn.pdf"
    HTML(string=build(), base_url=str(HERE)).write_pdf(str(path))
    print(f"built: {path.name}")


if __name__ == "__main__":
    sys.exit(main())
