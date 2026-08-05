#!/usr/bin/env python3
"""Build the PCI AI formula sheet PDFs from their markdown sources.

Each `NN-<slug>-formula-sheet.md` in docs/formula-sheets/ becomes one PDF in
backend/wwwroot/downloads/, so the site can serve it directly.

Source format:
    # Title
    > Subtitle
    ...body...

The builder generates the navy title page and appends the standard notices block, so
neither belongs in the body.

Usage:
    python3 build_formula_sheets.py                        # build all three
    python3 build_formula_sheets.py 02-pfl-ai-formula-sheet.md
"""
import datetime
import pathlib
import sys

import markdown
from weasyprint import CSS, HTML

HERE = pathlib.Path(__file__).resolve().parent
SHEETS = HERE.parent
ROOT = SHEETS.parent.parent
CSS_FILE = HERE / "formula.css"
OUT = ROOT / "backend" / "wwwroot" / "downloads"

INSTITUTION = "Project Controls Institute Global, Inc."

# The cover mark: a bar of the notation the sheet actually contains.
GLYPHS = {
    "pcl-ai": "CPI · SPI · EAC · TCPI · TF · EMV",
    "pfl-ai": "NPV · IRR · DSCR · LLCR · PLCR · WACC",
    "pml-ai": "PERT · CPM · PTA · EMV · WSJF · ROI",
}

TITLEPAGE = """
<div class="titlepage">
  <div class="series">Formula Sheet · {number}</div>
  <h1>{title}</h1>
  <div class="subtitle">{subtitle}</div>
  <div class="glyphs">{glyphs}</div>
  <div class="imprint">
    <strong>PCI AI · PROJECT CONTROLS INSTITUTE GLOBAL</strong><br/>
    First edition · {year}<br/>
    <em>AI proposes. The professional disposes.</em>
  </div>
</div>
"""

NOTICES = """
<div class="notices">
<p><strong>Notices.</strong> This is an educational study aid. Formulas are stated in this publication's
own words and notation; no standard's text is reproduced, and all trademarks remain the property of their
respective owners. Worked examples are illustrative and internally consistent; they do not represent any
client, project or organisation. Candidates should confirm the current examination specification,
including whether any formula reference is provided in the examination, on the official PCI website.</p>
<p>{institution} is a Delaware Non-Stock Corporation. It intends to seek recognition as a tax-exempt
nonprofit organisation under Section 501(c)(3) of the U.S. Internal Revenue Code; tax-exempt status has
not yet been granted, and contributions are not currently represented as tax-deductible.</p>
<p>PCI is not currently accredited by ANAB, IAS, or any ISO/IEC 17024 accreditation body — its
certification framework is being developed with reference to ISO/IEC 17024 personnel-certification
principles. PCI credentials can be verified at any time through the public credential lookup. PCI
certifications are not government-recognised, ISO-accredited or formally accredited. Recognition and
acceptance of PCI credentials may vary by employer, industry, institution and jurisdiction. PCI does not
guarantee employment, promotion, salary improvement, immigration benefits, licensing eligibility or
acceptance by any third party.</p>
<p>This document does not constitute accounting, tax, legal, financial or other professional advice.
© {year} {institution} All rights reserved.</p>
</div>
"""


def build(src: pathlib.Path) -> int:
    lines = src.read_text(encoding="utf-8").split("\n")
    title = lines[0].lstrip("# ").strip()

    # Subtitle is the first non-blank line after the title, written as a blockquote.
    subtitle, start = "", 1
    while start < len(lines) and not lines[start].strip():
        start += 1
    if start < len(lines) and lines[start].startswith("> "):
        subtitle = lines[start][2:].strip()
        start += 1

    body_md = "\n".join(lines[start:])

    # Drop the imprint block that duplicates the title page.
    body_md = body_md.split("---", 1)[1] if body_md.lstrip().startswith("**PCI AI") else body_md

    number = src.name.split("-")[0]
    credential = "-".join(src.stem.split("-")[1:3])
    year = datetime.date.today().year

    body_html = markdown.markdown(
        body_md, extensions=["tables", "attr_list", "sane_lists"], output_format="html5"
    )

    html = (
        f"<!doctype html><html><head><meta charset='utf-8'><title>{title}</title></head><body>"
        + TITLEPAGE.format(
            number=number,
            title=title,
            subtitle=subtitle,
            year=year,
            glyphs=GLYPHS.get(credential, ""),
        )
        + body_html
        + NOTICES.format(institution=INSTITUTION, year=year)
        + "</body></html>"
    )

    tmp = HERE / f"_{src.stem}.html"
    tmp.write_text(html, encoding="utf-8")
    doc = HTML(filename=str(tmp)).render(stylesheets=[CSS(filename=str(CSS_FILE))])
    tmp.unlink()

    OUT.mkdir(parents=True, exist_ok=True)
    pdf = OUT / f"pci-{src.stem[3:]}.pdf"
    doc.write_pdf(str(pdf))
    print(f"OK  {pdf.relative_to(ROOT)}  pages={len(doc.pages)}")
    return len(doc.pages)


if __name__ == "__main__":
    targets = (
        [SHEETS / a for a in sys.argv[1:]]
        if len(sys.argv) > 1
        else sorted(SHEETS.glob("*-formula-sheet.md"))
    )
    total = sum(build(t) for t in targets)
    print(f"built {len(targets)} sheet(s), {total} pages")
