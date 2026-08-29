#!/usr/bin/env python3
"""Build the PCI candidate-download PDFs from markdown, in the BoK's house style.

Each .md in this directory becomes one PDF in backend/wwwroot/downloads/.
First line must be "# Title"; optional second line "> subtitle".
Run with python3.12 (WeasyPrint needs it).
"""
import subprocess, sys, datetime, pathlib, re

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parent.parent
CSS = ROOT / "docs/bok/build/print.css"
OUT = ROOT / "backend/wwwroot/downloads"
OUT.mkdir(exist_ok=True)

FRONT = """
<div class="titlepage">
  <h1>{title}</h1>
  <div class="subtitle">{subtitle}</div>
  <div class="rule"></div>
  <div class="meta">OFFICIAL CANDIDATE DOCUMENT<br/>{year} EDITION<br/><br/>
  PROJECT CONTROLS INSTITUTE GLOBAL<br/>
  <em>AI proposes; the professional verifies, decides and remains accountable.</em></div>
</div>
<div class="frontmatter">
  <p style="page-break-before:always"><strong>Notice.</strong> This document is an educational publication of
  Project Controls Institute Global. It does not constitute accounting, tax, legal, financial or other
  professional advice. Standards and frameworks are referred to by name and described in this publication's
  own words; no standard's text is reproduced, and all trademarks remain the property of their respective
  owners. No governmental approval or third-party accreditation of the PCL-AI credential is implied.
  Examination operations described in the future tense will be confirmed before the first sitting.
  All examples are fictional and illustrative. Sample questions are study material, maintained separately
  from any live examination bank. © {year} Project Controls Institute Global. All rights reserved.</p>
</div>
"""

def build(md: pathlib.Path) -> int:
    text = md.read_text(encoding="utf-8")
    lines = text.split("\n")
    title = lines[0].lstrip("# ").strip()
    subtitle = ""
    body_start = 1
    if len(lines) > 1 and lines[1].startswith("> "):
        subtitle = lines[1][2:].strip()
        body_start = 2
    body_md = "\n".join(lines[body_start:])
    html = subprocess.run(
        ["pandoc", "-f", "gfm", "-t", "html", "--toc", "--toc-depth=2", "-s",
         "--metadata", f"title={title}"],
        input=body_md, capture_output=True, text=True, check=True).stdout
    html = html.replace("<body>", "<body>" + FRONT.format(
        title=title.replace(" — ", "<br/>"), subtitle=subtitle, year=datetime.date.today().year), 1)
    html = html.replace('<header id="title-block-header">', '<header id="title-block-header" style="display:none">', 1)
    html = re.sub(r'<h3\s+id="([^"]+)">((?:Key terms|Sample)[^<]*)</h3>', r'<h3 class="minihead" id="\1">\2</h3>', html)
    tmp = HERE / f"_{md.stem}.html"
    tmp.write_text(html, encoding="utf-8")
    from weasyprint import HTML, CSS as WCSS
    doc = HTML(filename=str(tmp)).render(stylesheets=[WCSS(filename=str(CSS))])
    pdf = OUT / f"{md.stem}.pdf"
    doc.write_pdf(str(pdf))
    tmp.unlink()
    print(f"OK {pdf.name}  pages={len(doc.pages)}")
    return len(doc.pages)

if __name__ == "__main__":
    total = 0
    for md in sorted(HERE.glob("*.md")):
        total += build(md)
    print(f"built {len(list(HERE.glob('*.md')))} documents, {total} pages")
