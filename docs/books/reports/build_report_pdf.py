#!/usr/bin/env python3
"""Build the programme audit report package as a single A4 PDF.

Concatenates the five programme reports in reading order, converts to HTML via
pandoc, styles for print, and renders with WeasyPrint.

Usage:  python3 build_report_pdf.py [output.pdf]
"""
import datetime
import pathlib
import subprocess
import sys

HERE = pathlib.Path(__file__).resolve().parent
OUT = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else HERE / "PCI-BoK-Audit-Report.pdf"

ORDER = [
    "EXECUTIVE_AUDIT_REPORT.md",
    "MASTER_ISSUE_REGISTER.md",
    "CALCULATION_ASSURANCE.md",
    "NAMING_MIGRATION_REPORT.md",
    "SIGN_OFF_REGISTER.md",
]

TITLE = f"""
<div class="titlepage">
  <div class="kicker">Project Controls Institute Global</div>
  <h1>PCI AI Certification<br/>Body of Knowledge Suite</h1>
  <div class="subtitle">Audit, verification, law framework, renaming,<br/>
  indexing and publication programme</div>
  <div class="rule"></div>
  <div class="meta">AUDIT REPORT PACKAGE<br/>
  {datetime.date.today().isoformat()}<br/><br/>
  PCL-AI &middot; PFL-AI &middot; PML-AI</div>
  <div class="warn">
    <strong>Read before quoting any readiness claim.</strong> No named human subject-matter expert has
    reviewed this material. The corpus was AI-drafted, and so was this programme's work on it.
    Gate&nbsp;3 (technical review) and Gate&nbsp;13 (final approval) are recorded as not started.
    See the Executive Audit Report &sect;5 and the Sign-Off Register &sect;1.
  </div>
</div>
"""

CSS = """
@page { size: A4; margin: 20mm 18mm 18mm 18mm;
  @bottom-center { content: counter(page); font: 8pt Georgia, serif; color: #64748B; } }
@page :first { @bottom-center { content: none; } }
body { font: 9.6pt/1.5 Georgia, "DejaVu Serif", serif; color: #0F172A; }
.titlepage { box-sizing: border-box; width: 100%; min-height: 255mm;
  padding: 30mm 16mm 18mm 16mm; page-break-after: always;
  background: #0F172A; color: #fff; }
.titlepage .kicker { font: 8pt/1 "DejaVu Sans", sans-serif; letter-spacing: .28em;
  text-transform: uppercase; color: #94A3B8; margin-bottom: 12mm; }
.titlepage h1 { font-size: 30pt; line-height: 1.15; margin: 0 0 6mm 0; color: #fff;
  border: none; padding: 0; page-break-before: avoid; }
.titlepage .subtitle { font-size: 12pt; color: #CBD5E1; line-height: 1.45; }
.titlepage .rule { width: 32mm; border-top: 2pt solid #C62828; margin: 10mm 0; }
.titlepage .meta { font: 8.5pt/1.7 "DejaVu Sans", sans-serif; letter-spacing: .16em;
  text-transform: uppercase; color: #94A3B8; }
.titlepage .warn { margin-top: 20mm; padding: 5mm 6mm; border-left: 2.5pt solid #C62828;
  background: rgba(255,255,255,.06); font-size: 8.6pt; line-height: 1.55; color: #E2E8F0; }
.titlepage .warn strong { color: #FCA5A5; }
h1 { font-size: 17pt; margin: 0 0 5mm 0; padding-bottom: 2.5mm;
  border-bottom: 1.5pt solid #0F172A; page-break-before: always; page-break-after: avoid; }
h2 { font-size: 12.5pt; margin: 8mm 0 3mm 0; page-break-after: avoid; }
h3 { font-size: 10.5pt; margin: 6mm 0 2mm 0; page-break-after: avoid; }
p { margin: 0 0 2.6mm 0; }
strong { color: #0F172A; }
table { border-collapse: collapse; width: 100%; font-size: 7.6pt; margin: 3mm 0 5mm 0;
  page-break-inside: auto; }
th { background: #0F172A; color: #fff; text-align: left; padding: 1.6mm 2mm;
  font-family: "DejaVu Sans", sans-serif; font-size: 7pt; letter-spacing: .04em; }
td { border-bottom: .4pt solid #CBD5E1; padding: 1.6mm 2mm; vertical-align: top; }
tr { page-break-inside: avoid; }
tbody tr:nth-child(even) { background: #F8FAFC; }
code { font-family: "DejaVu Sans Mono", monospace; font-size: 7.8pt; background: #F1F5F9;
  padding: 0 .8mm; }
blockquote { margin: 3mm 0; padding: 2.5mm 4mm; border-left: 2pt solid #C62828;
  background: #FDECEC; font-size: 9pt; }
ul, ol { margin: 0 0 3mm 5mm; padding: 0; }
li { margin-bottom: 1.2mm; }
hr { border: none; border-top: .5pt solid #CBD5E1; margin: 5mm 0; }
a { color: #1D4ED8; text-decoration: none; }
"""


def main() -> None:
    corpus = "\n\n".join((HERE / n).read_text(encoding="utf-8") for n in ORDER)
    combined = HERE / "_combined_report.md"
    combined.write_text(corpus, encoding="utf-8")

    html = subprocess.run(
        ["pandoc", str(combined), "-f", "gfm", "-t", "html", "-s",
         "--metadata", "title=PCI BoK Suite — Audit Report"],
        capture_output=True, text=True, check=True).stdout
    html = html.replace("<body>", "<body>" + TITLE, 1)
    html = html.replace('<header id="title-block-header">',
                        '<header id="title-block-header" style="display:none">', 1)

    from weasyprint import HTML, CSS as WCSS
    doc = HTML(string=html, base_url=str(HERE)).render(stylesheets=[WCSS(string=CSS)])
    doc.write_pdf(str(OUT))
    combined.unlink(missing_ok=True)
    print(f"OK {OUT}  pages={len(doc.pages)}")


if __name__ == "__main__":
    main()
