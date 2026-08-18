#!/usr/bin/env python3
"""Build a PCI AI Knowledge Series publication PDF from its markdown source.

Each publication lives in its own directory under docs/publications/knowledge-series/
and supplies `01-publication.md`. The markdown must open with:

    # Title
    > Subtitle

Everything after the subtitle is the body. Output goes to
backend/wwwroot/downloads/<slug>.pdf so the site can serve it directly.

Usage:
    python3 build_publication.py                 # build every publication
    python3 build_publication.py 01-project-...  # build one directory
"""
import datetime
import pathlib
import re
import sys

import markdown
from weasyprint import CSS, HTML

HERE = pathlib.Path(__file__).resolve().parent
SERIES = HERE.parent
ROOT = SERIES.parent.parent.parent
CSS_FILE = HERE / "publication.css"
OUT = ROOT / "backend" / "wwwroot" / "downloads"

INSTITUTION = "Project Controls Institute Global, Inc."

NOTICES = """
<div class="notices">
<p><strong>Notices.</strong> {institution} is a Delaware Non-Stock Corporation. It intends to seek
recognition as a tax-exempt nonprofit organisation under Section 501(c)(3) of the U.S. Internal Revenue
Code; tax-exempt status has not yet been granted, and contributions are not currently represented as
tax-deductible.</p>
<p>PCI is not currently accredited by ANAB, IAS, or any ISO/IEC 17024 accreditation body — its
certification framework is being developed with reference to ISO/IEC 17024 personnel-certification
principles. PCI credentials can be verified at any time through the public credential lookup. PCI
certifications are not government-recognised, ISO-accredited or formally accredited. Recognition and
acceptance of PCI credentials may vary by employer, industry, institution and jurisdiction. PCI does not
guarantee employment, promotion, salary improvement, immigration benefits, licensing eligibility or
acceptance by any third party.</p>
<p>This document is an educational publication. It does not constitute accounting, tax, legal, financial
or other professional advice. Standards and frameworks are referred to by name and described in this
publication's own words; no standard's text is reproduced, and all trademarks remain the property of
their respective owners. All worked examples are illustrative and do not represent any client, project or
organisation.</p>
<p>© {year} {institution} All rights reserved.</p>
</div>
"""

TITLEPAGE = """
<div class="titlepage">
  <div class="series">Knowledge Series · {number}</div>
  <h1>{title}</h1>
  <div class="subtitle">{subtitle}</div>
  <div class="rule-solid"></div>
  <div class="rule-dotted"></div>
  <div class="imprint">
    <strong>PCI AI · PROJECT CONTROLS INSTITUTE GLOBAL</strong><br/>
    First edition · {year}<br/>
    <em>AI proposes. The professional disposes.</em>
  </div>
</div>
"""


def classify_blockquotes(html: str) -> str:
    """Tag figure-spec and equation blockquotes so the stylesheet can treat them differently."""

    def tag(match: re.Match) -> str:
        inner = match.group(1)
        plain = re.sub(r"<[^>]+>", "", inner)
        if "suggested diagram" in plain.lower() or plain.strip().startswith("Figure"):
            return f'<blockquote class="figspec">{inner}</blockquote>'
        if re.search(r"\bE\s*=\s*F\s*×\s*T\s*×\s*A\b", plain):
            return f'<blockquote class="equation">{inner}</blockquote>'
        return match.group(0)

    return re.sub(r"<blockquote>(.*?)</blockquote>", tag, html, flags=re.DOTALL)


def build(directory: pathlib.Path) -> int:
    src = directory / "01-publication.md"
    if not src.exists():
        print(f"skip {directory.name}: no 01-publication.md")
        return 0

    lines = src.read_text(encoding="utf-8").split("\n")
    title = lines[0].lstrip("# ").strip()

    # The subtitle is the first non-blank line after the title, written as a blockquote.
    subtitle = ""
    start = 1
    while start < len(lines) and not lines[start].strip():
        start += 1
    if start < len(lines) and lines[start].startswith("> "):
        subtitle = lines[start][2:].strip()
        start += 1

    body_md = "\n".join(lines[start:])

    # The imprint block at the top of the source duplicates the title page.
    body_md = re.sub(
        r"^\s*\*\*PCI AI · Project Controls Institute Global, Inc\.\*\*.*?(?=\n---)",
        "",
        body_md,
        flags=re.DOTALL,
    )

    number = directory.name.split("-")[0]
    year = datetime.date.today().year

    # Python-Markdown merges blockquotes separated only by a blank line. A callout followed by a
    # figure spec must stay two boxes, so break the run with a block-level HTML comment.
    body_md = re.sub(r"(\n>[^\n]*)\n\n(?=> )", r"\1\n\n<!-- -->\n\n", body_md)

    body_html = markdown.markdown(
        body_md,
        extensions=["tables", "attr_list", "sane_lists", "md_in_html"],
        output_format="html5",
    )
    body_html = classify_blockquotes(body_html)

    html = (
        "<!doctype html><html><head><meta charset='utf-8'>"
        f"<title>{title}</title></head><body>"
        + TITLEPAGE.format(number=number, title=title, subtitle=subtitle, year=year)
        + body_html
        + NOTICES.format(institution=INSTITUTION, year=year)
        + "</body></html>"
    )

    tmp = HERE / f"_{directory.name}.html"
    tmp.write_text(html, encoding="utf-8")
    doc = HTML(filename=str(tmp)).render(stylesheets=[CSS(filename=str(CSS_FILE))])
    tmp.unlink()

    OUT.mkdir(parents=True, exist_ok=True)
    slug = f"pci-knowledge-{directory.name}"
    pdf = OUT / f"{slug}.pdf"
    doc.write_pdf(str(pdf))
    print(f"OK  {pdf.relative_to(ROOT)}  pages={len(doc.pages)}")
    return len(doc.pages)


if __name__ == "__main__":
    targets = (
        [SERIES / arg for arg in sys.argv[1:]]
        if len(sys.argv) > 1
        else sorted(d for d in SERIES.iterdir() if d.is_dir() and d.name != "build")
    )
    total = sum(build(d) for d in targets)
    print(f"built {len(targets)} publication(s), {total} pages")
