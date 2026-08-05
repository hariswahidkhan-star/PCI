#!/usr/bin/env python3
"""Build the LinkedIn edition of the PCI AI formula sheets.

1080 x 1350 (4:5) slide PDFs in the brand taken from the mark itself — navy field,
gold numerals, crimson rule. Sources live in docs/formula-sheets/linkedin/.

Slide conventions in the markdown:
  # Title / > Subtitle        the gradient cover (first two lines of the file)
  # 01 | Section name         a full-bleed navy section divider; any following
                              paragraph becomes its standfirst
  ## Heading {statement}      a dark full-bleed statement slide
  ## Heading {split}          a two-column contrast; halves separated by a line of %%
  ## Heading {stat}           a blue slide led by one large figure (first line = the figure)
  ## Heading                  a data slide (the default)
  **EYEBROW**                 first paragraph of a slide becomes the eyebrow label
  `formula`                   a paragraph of only code becomes a formula card;
                              consecutive ones merge into one card
  | table |                   the dense formula-index layout
  > note                      the navy note card with a crimson edge
  the final ## section        the closing gradient slide

Usage:
    python3 build_linkedin.py                    # build all
    python3 build_linkedin.py 01-pcl-ai-linkedin.md
"""
import datetime
import pathlib
import re
import sys

import markdown
from weasyprint import CSS, HTML

import base64

HERE = pathlib.Path(__file__).resolve().parent
SRC = HERE.parent / "linkedin"
ROOT = HERE.parent.parent.parent
CSS_FILE = HERE / "linkedin.css"
OUT = ROOT / "backend" / "wwwroot" / "downloads"

# The site's own mark, embedded so the PDF is self-contained.
LOGO_SVG = ROOT / "backend" / "wwwroot" / "assets" / "logo.svg"
LOGO = "data:image/svg+xml;base64," + base64.b64encode(LOGO_SVG.read_bytes()).decode()

def lockup(org: bool = True) -> str:
    org_html = ('<span class="org">Project Controls<br/>Institute Global, Inc.</span>' if org else "")
    return (f'<div class="lockup"><img src="{LOGO}" alt="PCI AI"/>'
            f'<span class="word">PCI AI</span>{org_html}</div>')

GLYPHS = {
    "pcl": "CPI · SPI · EAC · TCPI · ES · TF · EMV · CCC",
    "pfl": "NPV · IRR · WACC · CFADS · DSCR · LLCR · PLCR",
    "pml": "PERT · CPM · EMV · E[wait] · WSJF · PTA",
}

COVER = """
<div class="cover ondark">
  {lockup}
  <div class="series">PCI AI · Formula Sheet</div>
  <h1>{title}</h1>
  <div class="rule"></div>
  <div class="subtitle">{subtitle}</div>
  <div class="glyphs">{glyphs}</div>
  <div class="imprint">
    Project Controls Institute Global, Inc. · First edition {year}<br/>
    AI proposes. The professional disposes.
  </div>
</div>
"""

DIVIDER = """
<div class="divider">
  <div class="num">{num}</div>
  <h2>{name}<span class="dot">.</span></h2>
  {standfirst}
  {lockup}
</div>
"""

CLOSING = """
<div class="closing ondark">
  {lockup}
  <h2>{heading}<span class="dot">.</span></h2>
  {body}
  <div class="imprint">
    Educational publication. PCI is not accredited by ANAB, IAS or any ISO/IEC 17024 body.
    © {year} Project Controls Institute Global, Inc.
  </div>
</div>
"""


def merge_formula_runs(html: str) -> str:
    """A paragraph that is only a code span becomes a formula card; runs merge into one."""
    pattern = re.compile(r"(?:<p><code>.*?</code></p>\s*)+", re.DOTALL)

    def repl(m: re.Match) -> str:
        codes = re.findall(r"<p>(<code>.*?</code>)</p>", m.group(0), re.DOTALL)
        return f'<div class="formula">{"<br/>".join(codes)}</div>'

    return pattern.sub(repl, html)


def tokenise(body_md: str):
    """Walk the body into ('divider'|'slide', heading, content) blocks."""
    blocks, kind, head, buf = [], None, None, []
    for line in body_md.split("\n"):
        if line.startswith("## "):
            if kind:
                blocks.append((kind, head, "\n".join(buf)))
            kind, head, buf = "slide", line[3:].strip(), []
        elif line.startswith("# "):
            if kind:
                blocks.append((kind, head, "\n".join(buf)))
            kind, head, buf = "divider", line[2:].strip(), []
        else:
            buf.append(line)
    if kind:
        blocks.append((kind, head, "\n".join(buf)))
    return blocks


def build(src: pathlib.Path) -> int:
    lines = src.read_text(encoding="utf-8").split("\n")
    title = lines[0].lstrip("# ").strip()

    subtitle, start = "", 1
    while start < len(lines) and not lines[start].strip():
        start += 1
    if start < len(lines) and lines[start].startswith("> "):
        subtitle = lines[start][2:].strip()
        start += 1

    key = src.stem.split("-")[1]
    credential = key.upper() + "-AI"
    year = datetime.date.today().year

    blocks = tokenise("\n".join(lines[start:]))
    md = markdown.Markdown(extensions=["tables", "sane_lists"], output_format="html5")

    parts, slide_no, exhibit_no = [], 0, 0
    for i, (kind, head, content) in enumerate(blocks):
        last = i == len(blocks) - 1

        if kind == "divider":
            num, _, name = head.partition("|")
            md.reset()
            standfirst = md.convert(content.strip()) if content.strip() else ""
            parts.append(
                DIVIDER.format(num=num.strip(), name=name.strip() or num.strip(),
                               standfirst=standfirst, lockup=lockup())
            )
            continue

        kindtag = ""
        m_tag = re.search(r"\s*\{(\w+)\}\s*$", head)
        if m_tag:
            kindtag = m_tag.group(1)
            head = head[: m_tag.start()].strip()

        md.reset()
        html = merge_formula_runs(md.convert(content))

        if last:
            html = re.sub(r"\s*<p><strong>(.*?)</strong></p>", "", html, count=1, flags=re.DOTALL)
            html = html.replace(
                "<p><strong>projectcontrolsinstitute.org</strong></p>",
                '<div class="url">projectcontrolsinstitute.org</div>',
            )
            parts.append(CLOSING.format(heading=head, body=html, year=year, lockup=lockup()))
            continue

        eyebrow_html, body_html = "", html
        m_eb = re.match(r"\s*<p><strong>(.*?)</strong></p>", body_html, re.DOTALL)
        if m_eb:
            eyebrow_html = f'<div class="eyebrow">{m_eb.group(1)}</div>'
            body_html = body_html[m_eb.end():]

        if kindtag in ("statement", "split", "stat"):
            slide_no += 1
            foot = (f'<div class="foot"><span class="n">{slide_no:02d}</span>'
                    f'<img src="{LOGO}" alt=""/>{credential} Formula Sheet</div>')

            if kindtag == "statement":
                parts.append(
                    f'<div class="statement">{eyebrow_html}'
                    f'<h2>{head}<span class="dot">.</span></h2>{body_html}{foot}</div>')
                continue

            if kindtag == "stat":
                figure, _, rest = body_html.partition("</p>")
                figure = re.sub(r"</?p>", "", figure)
                parts.append(
                    f'<div class="statslide">{eyebrow_html}'
                    f'<div class="bignum">{figure}</div>'
                    f'<h2>{head}</h2>{rest}{foot}</div>')
                continue

            def role(col: str) -> str:
                return re.sub(r"^\s*<p><em>(.*?)</em></p>",
                              r'<div class="role">\1</div>', col, count=1, flags=re.DOTALL)

            left, _, right = body_html.partition("<p>%%</p>")
            left, right = role(left), role(right)
            parts.append(
                f'<div class="split"><div class="head">{eyebrow_html}'
                f'<h2>{head}</h2></div><div class="cols">'
                f'<div class="col">{left}</div><div class="col">{right}</div></div>{foot}</div>')
            continue

        html = body_html
        eyebrow = eyebrow_html

        # Number every table as an exhibit, the way a curriculum body does, and tag it with
        # its row count so the stylesheet can tighten a long one rather than overflow the slide.
        def label(m):
            nonlocal exhibit_no
            exhibit_no += 1
            rows = m.group(0).count("<tr>") - 1          # less the header row
            cols = m.group(0).split("</tr>")[0].count("<th>")
            klass = "dense" if rows > 5 else ""
            if rows > 8:
                klass = "verydense"
            if cols == 2:
                klass += " pair"
            return (f'<div class="exhibit">Exhibit {exhibit_no}</div>'
                    f'<table class="{klass.strip()}">' + m.group(0)[len("<table>"):])

        html = re.sub(r"<table>.*?</table>", label, html, flags=re.DOTALL)

        slide_no += 1
        parts.append(
            f'<div class="slide">{eyebrow}<h2>{head}</h2><div class="titlerule"></div>{html}'
            f'<div class="foot"><span class="n">{slide_no:02d}</span>'
            f'<img src="{LOGO}" alt=""/>{credential} Formula Sheet</div></div>'
        )

    html_doc = (
        f"<!doctype html><html><head><meta charset='utf-8'><title>{title}</title></head><body>"
        + COVER.format(title=title, subtitle=subtitle, glyphs=GLYPHS.get(key, ""),
                     year=year, lockup=lockup())
        + "".join(parts)
        + "</body></html>"
    )

    tmp = HERE / f"_{src.stem}.html"
    tmp.write_text(html_doc, encoding="utf-8")
    doc = HTML(filename=str(tmp)).render(stylesheets=[CSS(filename=str(CSS_FILE))])
    tmp.unlink()

    OUT.mkdir(parents=True, exist_ok=True)
    pdf = OUT / f"pci-{key}-ai-formula-sheet-linkedin.pdf"
    doc.write_pdf(str(pdf))
    print(f"OK  {pdf.relative_to(ROOT)}  slides={len(doc.pages)}")
    return len(doc.pages)


if __name__ == "__main__":
    targets = [SRC / a for a in sys.argv[1:]] if len(sys.argv) > 1 else sorted(SRC.glob("*.md"))
    total = sum(build(t) for t in targets)
    print(f"built {len(targets)} deck(s), {total} slides")
