#!/usr/bin/env python3
"""Build the LinkedIn edition of the PCI AI formula sheets.

1080 x 1350 (4:5) slide PDFs, sized so the type stays legible when LinkedIn's document
viewer scales a page down to phone width. Source files live in docs/formula-sheets/linkedin/.

Slide conventions in the markdown:
  # Title / > Subtitle        the gradient cover
  ## Heading                  starts a new slide
  **EYEBROW**                 first paragraph of a slide becomes the eyebrow label
  `formula`                   a paragraph containing only code becomes a formula card;
                              consecutive ones merge into a single card
  > note                      becomes the dark note card
  the final ## section        rendered as the closing gradient slide

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

HERE = pathlib.Path(__file__).resolve().parent
SRC = HERE.parent / "linkedin"
ROOT = HERE.parent.parent.parent
CSS_FILE = HERE / "linkedin.css"
OUT = ROOT / "backend" / "wwwroot" / "downloads"

GLYPHS = {
    "pcl": "CPI · SPI · EAC · TCPI · TF · EMV",
    "pfl": "NPV · IRR · DSCR · LLCR · PLCR · WACC",
    "pml": "PERT · CPM · PTA · EMV · WSJF · ROI",
}

COVER = """
<div class="cover">
  <div class="series">PCI AI · Formula Sheet</div>
  <h1>{title}</h1>
  <div class="subtitle">{subtitle}</div>
  <div class="glyphs">{glyphs}</div>
  <div class="imprint">
    <strong>PROJECT CONTROLS INSTITUTE GLOBAL</strong>
    First edition · {year} · AI proposes. The professional disposes.
  </div>
</div>
"""

CLOSING = """
<div class="closing">
  <h2>{heading}</h2>
  {body}
  <div class="imprint">
    Educational publication. PCI is not accredited by ANAB, IAS or any ISO/IEC 17024 body.
    Worked examples are illustrative. © {year} Project Controls Institute Global, Inc.
  </div>
</div>
"""


def merge_formula_runs(html: str) -> str:
    """A paragraph that is only a code span becomes a formula card; runs merge into one card."""
    pattern = re.compile(r"(?:<p><code>.*?</code></p>\s*)+", re.DOTALL)

    def repl(m: re.Match) -> str:
        codes = re.findall(r"<p>(<code>.*?</code>)</p>", m.group(0), re.DOTALL)
        lines = "<br/>".join(codes)
        return f'<div class="formula">{lines}</div>'

    return pattern.sub(repl, html)


def build_slide(heading: str, body_html: str, number: int, credential: str) -> str:
    body_html = merge_formula_runs(body_html)
    # first bold-only paragraph becomes the eyebrow
    eyebrow = ""
    m = re.match(r"\s*<p><strong>(.*?)</strong></p>", body_html, re.DOTALL)
    if m:
        eyebrow = f'<div class="eyebrow">{m.group(1)}</div>'
        body_html = body_html[m.end():]
    return (
        f'<div class="slide">{eyebrow}<h2>{heading}</h2>{body_html}'
        f'<div class="foot"><span>{credential} Formula Sheet</span>'
        f'<span class="n">{number:02d}</span></div></div>'
    )


def build(src: pathlib.Path) -> int:
    lines = src.read_text(encoding="utf-8").split("\n")
    title = lines[0].lstrip("# ").strip()

    subtitle, start = "", 1
    while start < len(lines) and not lines[start].strip():
        start += 1
    if start < len(lines) and lines[start].startswith("> "):
        subtitle = lines[start][2:].strip()
        start += 1

    body_md = "\n".join(lines[start:])
    credential = src.stem.split("-")[1].upper() + "-AI"
    key = src.stem.split("-")[1]
    year = datetime.date.today().year

    # split into slides on level-2 headings
    parts = re.split(r"^## ", body_md, flags=re.M)[1:]
    slides = []
    for part in parts:
        head, _, rest = part.partition("\n")
        slides.append((head.strip(), rest))

    md = markdown.Markdown(extensions=["tables", "sane_lists"], output_format="html5")

    html_slides = []
    for i, (head, body) in enumerate(slides[:-1], start=1):
        md.reset()
        html_slides.append(build_slide(head, md.convert(body), i, credential))

    # final section is the closing gradient slide
    close_head, close_body = slides[-1]
    md.reset()
    close_html = merge_formula_runs(md.convert(close_body))
    close_html = re.sub(
        r"\s*<p><strong>(.*?)</strong></p>", "", close_html, count=1, flags=re.DOTALL
    )
    close_html = close_html.replace(
        "<p>projectcontrolsinstitute.org</p>", ""
    ).replace(
        "<p><strong>projectcontrolsinstitute.org</strong></p>",
        '<div class="url">projectcontrolsinstitute.org</div>',
    )

    html = (
        f"<!doctype html><html><head><meta charset='utf-8'><title>{title}</title></head><body>"
        + COVER.format(title=title, subtitle=subtitle, glyphs=GLYPHS.get(key, ""), year=year)
        + "".join(html_slides)
        + CLOSING.format(heading=close_head, body=close_html, year=year)
        + "</body></html>"
    )

    tmp = HERE / f"_{src.stem}.html"
    tmp.write_text(html, encoding="utf-8")
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
