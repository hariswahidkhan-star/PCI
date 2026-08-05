#!/usr/bin/env python3
"""Build the PCL-AI course outline as a LinkedIn document post, in the company brand.

Two things separate this from the A4 outline PDF beside it.

**Shape.** A4 is 1:1.414. LinkedIn renders a document at its own aspect ratio inside a fixed-width
column, so a tall page arrives small: body text lands around 9 px on a phone and nobody reads it.
These slides are 1080 x 1350 (4:5), the tallest ratio the feed shows without cropping, which is the
largest tap target available and roughly twice the rendered type size of an A4 page.

**Brand.** The volumes are set in the editorial system — Libertine, green, old-style figures — which
is right for a 1,000-page book and wrong for a feed. This uses the website's brand instead: Archivo
for display at the tight tracking the site sets, Inter for text, and the blue palette from
`wwwroot/assets/styles.css`. Someone who taps through to projectcontrolsinstitute.org should arrive
somewhere that looks like where they came from.

Content is read from `course-outline-pcl-ai.md`, the same source as the A4 build, so the syllabus
cannot drift between formats.

Usage:  python3 build_outline_post.py
"""
import html
import pathlib
import re
import sys

HERE = pathlib.Path(__file__).resolve().parent
OUT = HERE / "assets"
FONTS = (HERE.parents[1] / "backend/wwwroot/assets/fonts").resolve()

# ---- brand tokens, from backend/wwwroot/assets/styles.css :root -------------------------------
BLUE = "#1D4ED8"        # --red   (the site's primary; the variable name is a legacy misnomer)
BLUE_LIGHT = "#3B82F6"  # --magenta
BLUE_DEEP = "#1E3A8A"   # --red-700
INK = "#0F172A"         # --ink
PAPER = "#FFFFFF"       # --paper
PAPER_2 = "#F1F5F9"     # --paper-2
LINE = "#E3E8EF"        # --line
SLATE = "#475569"       # --slate
MIST = "#64748B"        # --mist
GRAD = f"linear-gradient(160deg,{BLUE_LIGHT} 0%,{BLUE} 50%,{BLUE_DEEP} 100%)"  # --grad-brand

W, H = 1080, 1350

CSS = f"""
@font-face {{ font-family: Archivo; src: url(file://{FONTS}/archivo-latin.woff2) format('woff2');
              font-weight: 700 900; }}
@font-face {{ font-family: Inter; src: url(file://{FONTS}/inter-latin.woff2) format('woff2');
              font-weight: 400 700; }}

@page {{ size: {W}pt {H}pt; margin: 0; }}
* {{ box-sizing: border-box; }}
body {{ margin: 0; font-family: Inter, sans-serif; color: {INK};
        -webkit-font-smoothing: antialiased; }}

/* The site sets display type in Archivo 800–900 with -0.022em tracking. At poster sizes the
   tracking has to tighten further or the words drift apart. */
h1, h2, h3 {{ font-family: Archivo, sans-serif; font-weight: 900; letter-spacing: -.03em;
              line-height: 1.02; margin: 0; }}

.slide {{ width: {W}pt; height: {H}pt; padding: 86pt 84pt 66pt 84pt;
          display: flex; flex-direction: column; page-break-after: always;
          background: {PAPER}; position: relative; }}
.slide.brand {{ background: {GRAD}; color: {PAPER}; }}
.slide.tint {{ background: {PAPER_2}; }}
.body {{ flex: 1 1 auto; display: flex; flex-direction: column; justify-content: center; }}
.body > *:first-child {{ margin-top: 0; }}
.body > *:last-child {{ margin-bottom: 0; }}

.eyebrow {{ font-size: 18pt; font-weight: 700; letter-spacing: .16em; text-transform: uppercase;
            color: {BLUE}; margin-bottom: 18pt; }}
.slide.brand .eyebrow {{ color: #BFD4FF; }}

h1 {{ font-size: 88pt; }}
h2 {{ font-size: 62pt; }}
.lede {{ font-size: 25pt; line-height: 1.45; color: {SLATE}; margin-top: 26pt; font-weight: 400; }}
.slide.brand .lede {{ color: #DCE7FF; }}

.rule {{ width: 96pt; height: 5pt; background: {BLUE}; margin: 30pt 0; border-radius: 3pt; }}
.slide.brand .rule {{ background: #93B4FF; }}

/* Counts. Big lining numerals are the one place the page should shout. */
.figs {{ display: flex; gap: 30pt; margin-top: 40pt; }}
.fig {{ flex: 1; }}
.fig .n {{ font-family: Archivo, sans-serif; font-weight: 900; font-size: 76pt; color: {BLUE};
           letter-spacing: -.04em; line-height: 1; }}
.slide.brand .fig .n {{ color: {PAPER}; }}
.fig .l {{ font-size: 17pt; color: {MIST}; margin-top: 8pt; line-height: 1.3; font-weight: 500; }}
.slide.brand .fig .l {{ color: #C8DAFF; }}

/* A domain: number and count in a left rail, title and knowledge areas taking the full measure.
   The count sat in a right-hand column in the first cut and the longest domain title ran straight
   into it; moving it under the number frees the whole width for the name, which is the line people
   actually read. The number is the navigation aid — at a glance you know how far through the
   syllabus a slide sits. */
.dom {{ display: flex; gap: 24pt; padding: 19pt 0; border-top: 1.5pt solid {LINE}; }}
.dom:last-child {{ border-bottom: 1.5pt solid {LINE}; }}
.dom .rail {{ width: 62pt; flex: 0 0 62pt; }}
.dom .no {{ font-family: Archivo, sans-serif; font-weight: 900; font-size: 30pt; color: {BLUE_LIGHT};
            letter-spacing: -.03em; line-height: 1; }}
.dom .count {{ font-size: 13pt; color: {MIST}; font-weight: 600; margin-top: 5pt;
               white-space: nowrap; }}
.dom .t {{ flex: 1; }}
.dom .name {{ font-family: Archivo, sans-serif; font-weight: 800; font-size: 24pt;
              letter-spacing: -.022em; line-height: 1.14; color: {INK}; }}
.dom .kas {{ font-size: 17pt; line-height: 1.44; color: {SLATE}; margin-top: 9pt; }}

.callout {{ background: {PAPER_2}; border-left: 6pt solid {BLUE}; padding: 28pt 32pt;
            font-size: 21pt; line-height: 1.5; color: {SLATE}; margin-top: 28pt; }}
.callout strong {{ color: {INK}; font-weight: 700; }}

.point {{ font-size: 22pt; line-height: 1.5; color: {SLATE}; margin: 0 0 18pt 0; }}
.point b {{ color: {INK}; font-weight: 700; }}

.foot {{ flex: 0 0 auto; display: flex; justify-content: space-between; align-items: baseline;
         margin-top: 34pt; padding-top: 16pt; border-top: 1.5pt solid {LINE};
         font-size: 14pt; color: {MIST}; font-weight: 500; }}
.slide.brand .foot {{ border-color: rgba(255,255,255,.28); color: #C8DAFF; }}
.slide.tint .foot {{ border-color: #D8E0EA; }}
.foot .pg {{ font-variant-numeric: tabular-nums; }}

/* The swipe affordance. The arrow is drawn, not typed: U+2192 is outside the latin subset of the
   brand woff2, so a typed one silently falls back to whatever the renderer has — an off-brand glyph
   on the one slide everybody sees. An inline SVG in currentColor cannot fall back. */
.swipe {{ font-size: 17pt; font-weight: 700; letter-spacing: .04em; color: #BFD4FF; margin-top: 30pt;
          display: flex; align-items: center; gap: 10pt; }}
.swipe svg {{ display: block; }}
"""


def esc(s: str) -> str:
    return html.escape(s)


# ---- read the syllabus from the markdown, so the two formats cannot disagree -------------------

def load_domains() -> list:
    """[(number, title, 'n knowledge areas', 'KA · KA · KA', note), …] in the volume's order."""
    md = (HERE / "course-outline-pcl-ai.md").read_text(encoding="utf-8")
    body = md.split("## 2 — The outline", 1)[1]
    # Fold the hard-wrapped source into logical blocks, breaking at each structural marker.
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

    doms, cur = [], None
    for b in blocks:
        m = re.match(r"\*\*Domain (\d+) · (.+?)\*\*\s*—\s*(.+)", b)
        if m:
            if cur:
                doms.append(cur)
            cur = [m.group(1), m.group(2), m.group(3), "", ""]
        elif cur and not cur[3] and "·" in b and not b.startswith("*"):
            cur[3] = b
        elif cur and b.startswith("*") and not b.startswith("**") and "per cent" not in b:
            cur[4] = b.strip("* ")
    if cur:
        doms.append(cur)
    return doms


def dom_html(d) -> str:
    no, name, count, kas, _note = d
    n = count.split()[0]  # "5 knowledge areas" -> "5"
    return (f"<div class='dom'>"
            f"<div class='rail'><div class='no'>{no}</div>"
            f"<div class='count'>{n} KAs</div></div>"
            f"<div class='t'><div class='name'>{esc(name)}</div>"
            f"<div class='kas'>{esc(kas)}</div></div></div>")


def slide(body: str, foot_left: str, page: str, cls: str = "") -> str:
    c = ("slide " + cls).strip()
    return (f"<div class='{c}'><div class='body'>{body}</div>"
            f"<div class='foot'><span>{foot_left}</span><span class='pg'>{page}</span></div></div>")


def build() -> str:
    doms = load_domains()
    assert len(doms) == 13, f"expected 13 domains, parsed {len(doms)}"
    by_no = {int(d[0]): d for d in doms}
    F = "Project Controls Institute Global"
    S = []

    # 1 — cover
    S.append(slide(
        "<div class='eyebrow'>Course outline</div>"
        "<h1>PCI AI<br/>Project Controls<br/>Leader</h1>"
        "<div class='rule'></div>"
        "<div class='lede'>The full syllabus. 13 domains, 61 knowledge areas, and the governed use "
        "of AI treated as part of the discipline rather than a chapter at the end.</div>"
        "<div class='swipe'><span>Swipe for the whole thing</span>"
        # Stroke is stated, not inherited: WeasyPrint does not resolve currentColor into inline SVG,
        # so an inherited stroke silently renders black on the brand gradient.
        "<svg width='30' height='13' viewBox='0 0 30 13' fill='none'>"
        "<path d='M0 6.5h27M21.5 1l6 5.5-6 5.5' stroke='#BFD4FF' stroke-width='2'"
        " stroke-linecap='round' stroke-linejoin='round'/></svg></div>",
        F, "PCL-AI", "brand"))

    # 2 — the shape of it
    S.append(slide(
        "<div class='eyebrow'>What you get</div>"
        "<h2>A syllabus you can<br/>be examined on</h2>"
        "<div class='figs'>"
        "<div class='fig'><div class='n'>13</div><div class='l'>domains</div></div>"
        "<div class='fig'><div class='n'>61</div><div class='l'>knowledge areas</div></div>"
        "<div class='fig'><div class='n'>26</div><div class='l'>sector case studies</div></div>"
        "</div>"
        "<div class='callout'>Every technique arrives with <strong>the conditions under which it "
        "fails</strong>. A method you cannot break is a method you do not yet understand.</div>",
        F, "1 / 6"))

    # 3 — Part One
    S.append(slide(
        "<div class='eyebrow'>Part one &middot; 40% of the Body of Knowledge</div>"
        "<h2>Finance, accounting<br/>and reporting</h2>"
        "<div class='rule'></div>"
        + "".join(dom_html(by_no[i]) for i in (1, 2, 3, 4)),
        F, "2 / 6"))

    # 4–5 — Part Two, split so no slide becomes a wall
    S.append(slide(
        "<div class='eyebrow'>Part two &middot; 40% of the Body of Knowledge</div>"
        "<h2>Project management</h2>"
        "<div class='rule'></div>"
        + "".join(dom_html(by_no[i]) for i in (5, 6, 7, 8)),
        F, "3 / 6"))
    S.append(slide(
        "<div class='eyebrow'>Part two &middot; continued</div>"
        "<h2>Delivery, schedule,<br/>process and risk</h2>"
        "<div class='rule'></div>"
        + "".join(dom_html(by_no[i]) for i in (9, 10, 11, 12)),
        F, "4 / 6"))

    # 6 — Part Three
    S.append(slide(
        "<div class='eyebrow'>Part three &middot; 20% of the Body of Knowledge</div>"
        "<h2>AI, governed</h2>"
        "<div class='rule'></div>"
        + dom_html(by_no[13])
        + "<div class='callout'>The largest domain in the volume, and the reason the credential "
          "exists in this form. It ends where every automated output should end: with "
          "<strong>a named human who verified it</strong>.</div>",
        F, "5 / 6"))

    # 7 — what holds it up
    S.append(slide(
        "<div class='eyebrow'>What sits behind the syllabus</div>"
        "<h2>113 standards.<br/>532 process<br/>requirements.</h2>"
        "<div class='point' style='margin-top:30pt'>Each states its purpose, who owns the decision, "
        "what evidence must exist, what practice is prohibited, what triggers escalation, "
        "<b>what AI may never decide, approve or certify</b> — and a compliance test an assessor "
        "can actually perform.</div>"
        "<div class='point'>They are certification requirements established by the Institute. They "
        "are not legislation, and nothing here is legal, tax or accounting advice.</div>",
        F, "6 / 6", "tint"))

    # 8 — close
    S.append(slide(
        "<div class='eyebrow'>The principle behind all of it</div>"
        "<h2>AI proposes.<br/>The professional<br/>verifies, decides<br/>and remains<br/>"
        "accountable.</h2>"
        "<div class='lede'>Full outline and the three Bodies of Knowledge at "
        "projectcontrolsinstitute.org</div>",
        F, "PCL-AI", "brand"))

    return f"<style>{CSS}</style>" + "".join(S)


def main() -> None:
    OUT.mkdir(exist_ok=True)
    from weasyprint import HTML
    path = OUT / "PCI-PCL-AI-Course-Outline-LinkedIn.pdf"
    HTML(string=build(), base_url=str(HERE)).write_pdf(str(path))
    print(f"built: {path.name}")


if __name__ == "__main__":
    sys.exit(main())
