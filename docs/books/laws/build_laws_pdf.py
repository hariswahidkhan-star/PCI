#!/usr/bin/env python3
"""Build the PCI Professional Laws as a single A4 publication.

Assembles the Charter, the Drafting Manual, the four law sets and the
concordance, converts to HTML via pandoc, and renders with WeasyPrint.

Usage:  python3 build_laws_pdf.py [output.pdf]
"""
import datetime
import pathlib
import subprocess
import sys

HERE = pathlib.Path(__file__).resolve().parent
OUT = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else HERE / "PCI-Professional-Laws.pdf"

ORDER = [
    "PCI_PROFESSIONAL_LAWS_CHARTER.md",
    "PCI_LAW_DRAFTING_MANUAL.md",
    "PCI_FOUNDATIONAL_LAWS.md",
    "PCL_AI_LAWS.md",
    "PFL_AI_LAWS.md",
    "PML_AI_LAWS.md",
    "LAW_CONCORDANCE.md",
    "PCI_LAW_DEFINITIONS_REGISTER.md",
]

TITLE = f"""
<div class="titlepage">
  <div class="kicker">Project Controls Institute Global</div>
  <h1>PCI Professional Laws</h1>
  <div class="subtitle">The mandatory professional requirements of the<br/>
  PCI AI certification suite</div>
  <div class="rule"></div>
  <div class="meta">PCL-AI &middot; PFL-AI &middot; PML-AI<br/>
  VERSION 1.0 &mdash; DRAFT FOR APPROVAL<br/>
  {datetime.date.today().isoformat()}</div>
  <div class="warn">
    <strong>Not legislation.</strong> PCI Professional Laws are private professional certification
    requirements established by Project Controls Institute Global. They are not legislation,
    government regulation, legal advice or substitutes for applicable laws, contractual obligations,
    regulatory requirements or authoritative professional standards. Where an applicable legal,
    regulatory, contractual or authoritative requirement imposes a higher or different obligation,
    that requirement prevails.
  </div>
  <div class="warn2">
    <strong>Not yet approved.</strong> This edition has not completed the due process in Charter
    &sect;5. Practitioner consultation, approval by a named PCI body and post-implementation review
    have not been performed, and no named human has approved any law in it.
  </div>
</div>
"""

CSS = """
@page { size: A4; margin: 20mm 18mm 18mm 18mm;
  @bottom-center { content: counter(page); font: 8pt Georgia, serif; color: #64748B; } }
@page :first { @bottom-center { content: none; } }
body { font: 9.4pt/1.5 Georgia, "DejaVu Serif", serif; color: #0F172A; }
.titlepage { box-sizing: border-box; width: 100%; min-height: 255mm;
  padding: 28mm 16mm 16mm 16mm; page-break-after: always;
  background: #7F1D1D; color: #fff; }
.titlepage .kicker { font: 8pt/1 "DejaVu Sans", sans-serif; letter-spacing: .28em;
  text-transform: uppercase; color: #FCA5A5; margin-bottom: 12mm; }
.titlepage h1 { font-size: 32pt; line-height: 1.1; margin: 0 0 6mm 0; color: #fff;
  border: none; padding: 0; page-break-before: avoid; }
.titlepage .subtitle { font-size: 12pt; color: #FECACA; line-height: 1.45; }
.titlepage .rule { width: 32mm; border-top: 2pt solid #FCA5A5; margin: 9mm 0; }
.titlepage .meta { font: 8.5pt/1.7 "DejaVu Sans", sans-serif; letter-spacing: .16em;
  text-transform: uppercase; color: #FECACA; }
.titlepage .warn, .titlepage .warn2 { margin-top: 9mm; padding: 4.5mm 6mm;
  border-left: 2.5pt solid #FCA5A5; background: rgba(255,255,255,.08);
  font-size: 8.4pt; line-height: 1.55; color: #FFF1F2; }
.titlepage .warn strong, .titlepage .warn2 strong { color: #fff; }
h1 { font-size: 17pt; margin: 0 0 5mm 0; padding-bottom: 2.5mm;
  border-bottom: 1.5pt solid #7F1D1D; page-break-before: always; page-break-after: avoid; }
h2 { font-size: 12pt; margin: 7mm 0 3mm 0; page-break-after: avoid; }
h3 { font-size: 10.2pt; margin: 6mm 0 2mm 0; color: #9B1C1C; page-break-after: avoid; }
p { margin: 0 0 2.4mm 0; }
table { border-collapse: collapse; width: 100%; font-size: 7.4pt; margin: 3mm 0 5mm 0; }
th { background: #7F1D1D; color: #fff; text-align: left; padding: 1.5mm 2mm;
  font-family: "DejaVu Sans", sans-serif; font-size: 6.9pt; }
td { border-bottom: .4pt solid #CBD5E1; padding: 1.5mm 2mm; vertical-align: top; }
tr { page-break-inside: avoid; }
tbody tr:nth-child(even) { background: #FEF7F7; }
code { font-family: "DejaVu Sans Mono", monospace; font-size: 7.6pt; background: #FDECEC;
  padding: 0 .8mm; color: #7F1D1D; }
blockquote { margin: 3mm 0; padding: 2.5mm 4mm; border-left: 2pt solid #9B1C1C;
  background: #FDECEC; font-size: 8.8pt; }
ul, ol { margin: 0 0 3mm 5mm; padding: 0; }
li { margin-bottom: 1mm; }
hr { border: none; border-top: .5pt solid #CBD5E1; margin: 5mm 0; }
a { color: #1D4ED8; text-decoration: none; }
"""


def main() -> None:
    parts = []
    for name in ORDER:
        path = HERE / name
        if not path.exists():
            print(f"skipped (missing): {name}")
            continue
        parts.append(path.read_text(encoding="utf-8"))
        print(f"included: {name}")
    combined = HERE / "_combined_laws.md"
    combined.write_text("\n\n".join(parts), encoding="utf-8")

    html = subprocess.run(
        ["pandoc", str(combined), "-f", "gfm", "-t", "html", "-s", "--toc", "--toc-depth=2",
         "--metadata", "title=PCI Professional Laws"],
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
