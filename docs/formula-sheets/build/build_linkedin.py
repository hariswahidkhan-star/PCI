#!/usr/bin/env python3
"""Build the LinkedIn edition of the PCI AI formula sheets.

1080 x 1350 (4:5) slide PDFs in the brand taken from the mark itself — navy field,
gold numerals, crimson rule. Sources live in docs/formula-sheets/linkedin/.

Slide conventions in the markdown:
  # Title / > Subtitle        the gradient cover (first two lines of the file)
  # 01 | Section name         a full-bleed navy section divider; any following
                              paragraph becomes its standfirst
  ## Heading                  a content slide
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

HERE = pathlib.Path(__file__).resolve().parent
SRC = HERE.parent / "linkedin"
ROOT = HERE.parent.parent.parent
CSS_FILE = HERE / "linkedin.css"
OUT = ROOT / "backend" / "wwwroot" / "downloads"

GLYPHS = {
    "pcl": "CPI · SPI · EAC · TCPI · ES · TF · EMV · CCC",
    "pfl": "NPV · IRR · WACC · CFADS · DSCR · LLCR · PLCR",
    "pml": "PERT · CPM · EMV · E[wait] · WSJF · PTA",
}

COVER = """
<div class="cover">
  <div class="series">PCI AI · Formula Sheet</div>
  <h1>{title}</h1>
  <div class="rule"></div>
  <div class="subtitle">{subtitle}</div>
  <div class="glyphs">{glyphs}</div>
  <div class="imprint">
    <strong>PROJECT CONTROLS INSTITUTE GLOBAL</strong>
    First edition · {year} · AI proposes. The professional disposes.
  </div>
</div>
"""

DIVIDER = """
<div class="divider">
  <div class="num">{num}</div>
  <h2>{name}</h2>
  <div class="rule"></div>
  {standfirst}
</div>
"""

CLOSING = """
<div class="closing">
  <h2>{heading}</h2>
  <div class="rule"></div>
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

    parts, slide_no = [], 0
    for i, (kind, head, content) in enumerate(blocks):
        last = i == len(blocks) - 1

        if kind == "divider":
            num, _, name = head.partition("|")
            md.reset()
            standfirst = md.convert(content.strip()) if content.strip() else ""
            parts.append(
                DIVIDER.format(num=num.strip(), name=name.strip() or num.strip(),
                               standfirst=standfirst)
            )
            continue

        md.reset()
        html = merge_formula_runs(md.convert(content))

        if last:
            html = re.sub(r"\s*<p><strong>(.*?)</strong></p>", "", html, count=1, flags=re.DOTALL)
            html = html.replace(
                "<p><strong>projectcontrolsinstitute.org</strong></p>",
                '<div class="url">projectcontrolsinstitute.org</div>',
            )
            parts.append(CLOSING.format(heading=head, body=html, year=year))
            continue

        eyebrow = ""
        m = re.match(r"\s*<p><strong>(.*?)</strong></p>", html, re.DOTALL)
        if m:
            eyebrow = f'<div class="eyebrow">{m.group(1)}</div>'
            html = html[m.end():]

        slide_no += 1
        parts.append(
            f'<div class="slide">{eyebrow}<h2>{head}</h2><div class="titlerule"></div>{html}'
            f'<div class="foot"><span>{credential} Formula Sheet</span>'
            f'<span class="n">{slide_no:02d}</span></div></div>'
        )

    html_doc = (
        f"<!doctype html><html><head><meta charset='utf-8'><title>{title}</title></head><body>"
        + COVER.format(title=title, subtitle=subtitle, glyphs=GLYPHS.get(key, ""), year=year)
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
