#!/usr/bin/env python3
"""Build the programme audit report package as a single A4 PDF.

Concatenates the five programme reports in reading order, converts to HTML via pandoc, and renders
with WeasyPrint using the family stylesheet — the same page, type, tables and call-outs as the
published volumes it reports on, so a reader moving between them is never re-learning the page.

Usage:  python3 build_report_pdf.py [output.pdf]
"""
import datetime
import importlib.util
import pathlib
import re
import subprocess
import sys

HERE = pathlib.Path(__file__).resolve().parent
OUT = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else HERE / "PCI-BoK-Audit-Report.pdf"

_FAMILY = HERE.parent / "_build" / "build_book.py"
_spec = importlib.util.spec_from_file_location("pci_book_family", _FAMILY)
family = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(family)

RUN_TITLE = "PCI Body of Knowledge — audit report"

ORDER = [
    "EXECUTIVE_AUDIT_REPORT.md",
    "MASTER_ISSUE_REGISTER.md",
    "CALCULATION_ASSURANCE.md",
    "NAMING_MIGRATION_REPORT.md",
    "SIGN_OFF_REGISTER.md",
]

COVER = family.cover_html(
    code="Audit",
    book="Report package",
    credential="Audit, verification, standards framework, renaming, indexing and publication",
    themes="PCL-AI · PFL-AI · PML-AI",
    chart="",
    principle="AI proposes; the professional verifies,<br/>decides and remains accountable.",
    badge="Programme audit — " + datetime.date.today().isoformat(),
    badgesub="Gate 3 (technical review) and Gate 13 (final approval) recorded as not started",
    tag="Advancing standards. Elevating professionals.")

TITLE = f"""
<div class="titlepage">
  <div class="kicker">Project Controls Institute Global</div>
  <h1>PCI AI Certification
  Body of Knowledge Suite</h1>
  <div class="subtitle">Audit, verification, standards framework, renaming,
  indexing and publication programme</div>
  <div class="rule"></div>
  <div class="meta">Audit report package<br/>
  {datetime.date.today().isoformat()}<br/>
  PCL-AI &middot; PFL-AI &middot; PML-AI</div>
  <div class="pci-caution">
    <p><strong>Read before quoting any readiness claim.</strong> No named human subject-matter
    expert has reviewed this material. The corpus was AI-drafted, and so was this programme's work
    on it. Gate&nbsp;3 (technical review) and Gate&nbsp;13 (final approval) are recorded as not
    started. See the Executive Audit Report section 5 and the Sign-Off Register section 1.</p>
  </div>
</div>
"""

# The report package differs from a book in one respect: it has no device on its cover, and its
# tables carry more columns than a book's, so they are set a step smaller.
DELTA_CSS = """
.cover .coverband { display: none; }
.cover .coverhead { padding-top: 52mm; }
.cover .covercode { font-size: 52pt; }
.cover .coverfoot { padding-top: 34mm; }
.titlepage { padding-top: 44mm; }
.titlepage .pci-caution { margin-top: 14mm; max-width: 128mm; font-size: 9.6pt; }
.titlepage .pci-caution p { text-align: left; }
table { font-size: 8pt; }
th { font-size: 8.2pt; }
"""


def main() -> None:
    parts = []
    for name in ORDER:
        path = HERE / name
        if not path.exists():
            print(f"skipped (missing): {name}")
            continue
        html = subprocess.run(["pandoc", str(path), "-f", "gfm", "-t", "html"],
                              capture_output=True, text=True, check=True).stdout
        html = re.sub(r'<h1 id="([^"]+)"[^>]*>', r'<h1 id="\1" data-toc="1">', html)
        html = re.sub(r'<h2 id="([^"]+)"[^>]*>', r'<h2 id="\1" data-toc="2">', html)
        parts.append(html)
        print(f"included: {name}")

    body_html = "".join(parts)
    toc = family.build_toc(body_html)
    doc = (f"<!doctype html><html lang='{family.LANG}'><head><meta charset='utf-8'>"
           f"<style>html {{ string-set: booktitle \"{RUN_TITLE}\"; }}</style></head>"
           f"<body>{COVER}{TITLE}{toc}{body_html}</body></html>")
    doc = family.house_style(doc, "report")

    from weasyprint import HTML, CSS as WCSS
    sheets = [WCSS(filename=str(family.HOUSE_CSS)), WCSS(string=DELTA_CSS)]
    rendered = HTML(string=doc, base_url=str(HERE)).render(stylesheets=sheets)
    rendered.write_pdf(str(OUT))
    print(f"OK {OUT}  pages={len(rendered.pages)}")


if __name__ == "__main__":
    main()
