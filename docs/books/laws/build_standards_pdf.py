#!/usr/bin/env python3
"""Build the PCI Standards as a single A4 publication.

Assembles the Charter, the Drafting Manual, the four standard sets, the concordance and the
definitions register, and renders with WeasyPrint.

The volume is set in the same design as the three Bodies of Knowledge — the same page geometry,
type, tables, openers, running heads and folios, from the same stylesheet — and differs from them
only in its cover. A standard looks the same here as it does quoted in a Body of Knowledge,
because it is rendered by the same code.

Usage:  python3 build_standards_pdf.py [output.pdf]
"""
import datetime
import importlib.util
import pathlib
import re
import subprocess
import sys

HERE = pathlib.Path(__file__).resolve().parent
OUT = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else HERE / "PCI-Standards.pdf"

# The family pipeline: one stylesheet, one cover design, one standards renderer.
_FAMILY = HERE.parent / "_build" / "build_book.py"
_spec = importlib.util.spec_from_file_location("pci_book_family", _FAMILY)
family = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(family)

RUN_TITLE = "PCI Standards"

# Prose parts, rendered as documents; standard sets, rendered as standards. The order is the
# reading order of the volume.
ORDER = [
    ("prose", "PCI_STANDARDS_CHARTER.md"),
    ("prose", "PCI_STANDARDS_DRAFTING_MANUAL.md"),
    ("standards", "PCI_FOUNDATIONAL_STANDARDS.md"),
    ("standards", "PCL_AI_STANDARDS.md"),
    ("standards", "PFL_AI_STANDARDS.md"),
    ("standards", "PML_AI_STANDARDS.md"),
    ("prose", "STANDARDS_CONCORDANCE.md"),
    ("prose", "PCI_STANDARDS_DEFINITIONS_REGISTER.md"),
]

COVER = family.cover_html(
    code="PCI",
    book="Standards",
    credential="The mandatory professional requirements of the PCI AI certification suite",
    themes="PCL-AI · PFL-AI · PML-AI",
    chart="",
    principle="Mandatory professional requirements. Not legislation,<br/>"
              "and never a substitute for applicable law.",
    badge="Version 1.0 — draft for approval",
    badgesub="Charter §5 due process not complete · no named human has approved any standard",
    tag="Advancing standards. Elevating professionals.")

TITLE = f"""
<div class="titlepage">
  <div class="kicker">Project Controls Institute Global</div>
  <h1>PCI Standards</h1>
  <div class="subtitle">The mandatory professional requirements of the
  PCI AI certification suite</div>
  <div class="rule"></div>
  <div class="meta">PCL-AI &middot; PFL-AI &middot; PML-AI<br/>
  Version 1.0 &mdash; draft for approval<br/>
  {datetime.date.today().isoformat()}</div>
  <div class="pci-caution">
    <p><strong>Not legislation.</strong> PCI Standards are private professional certification
    requirements established by Project Controls Institute Global. They are not legislation,
    government regulation, legal advice or substitutes for applicable laws, contractual obligations,
    regulatory requirements or authoritative professional standards. Where an applicable legal,
    regulatory, contractual or authoritative requirement imposes a higher or different obligation,
    that requirement prevails.</p>
    <p><strong>Not yet approved.</strong> This edition has not completed the due process in Charter
    &sect;5. Practitioner consultation, approval by a named PCI body and post-implementation review
    have not been performed, and no named human has approved any standard in it.</p>
  </div>
</div>
"""

# The one place this volume differs from a Body of Knowledge: its cover carries no device, and the
# title page's two notices are set as the system's caution call-out rather than as reversed panels.
COVER_CSS = """
.cover .coverband { display: none; }
.cover .coverhead { padding-top: 56mm; }
.cover .covercode { font-size: 46pt; }
.cover .coverfoot { padding-top: 40mm; }
.titlepage { padding-top: 42mm; }
.titlepage .pci-caution { margin-top: 14mm; max-width: 128mm; font-size: 9.6pt; }
.titlepage .pci-caution p { text-align: left; }
"""


def render_prose(path: pathlib.Path) -> str:
    """One prose document → HTML, with its headings marked for the contents."""
    html = subprocess.run(["pandoc", str(path), "-f", "gfm", "-t", "html"],
                          capture_output=True, text=True, check=True).stdout
    html = re.sub(r'<h1 id="([^"]+)"[^>]*>', r'<h1 id="\1" data-toc="1">', html)
    html = re.sub(r'<h2 id="([^"]+)"[^>]*>', r'<h2 id="\1" data-toc="2">', html)
    return f'<div class="stdsdoc">{html}</div>'


def main() -> None:
    body, sets, standards = [], 0, 0
    for kind, name in ORDER:
        path = HERE / name
        if not path.exists():
            print(f"skipped (missing): {name}")
            continue
        if kind == "prose":
            body.append(render_prose(path))
        else:
            title, html, laws = family.render_standards_file(path)
            standards += laws
            sets += 1
            body.append('<div class="backmatter stdspart">'
                        f'<h1 class="stdstitle bmtitle" id="{family.slug(title)}" data-toc="1">'
                        f'{title}</h1>' + html + "</div>")
        print(f"included: {name}")
    print(f"standards: {standards} from {sets} set(s)")

    body_html = "".join(body)
    toc = family.build_toc(body_html)
    doc = (f"<!doctype html><html lang='{family.LANG}'><head><meta charset='utf-8'>"
           f"<style>html {{ string-set: booktitle \"{RUN_TITLE}\"; }}</style></head>"
           f"<body>{COVER}{TITLE}{toc}{body_html}</body></html>")
    doc = family.house_style(doc, "standards")
    (HERE / "_combined_standards.html").write_text(doc, encoding="utf-8")

    from weasyprint import HTML, CSS as WCSS
    sheets = [WCSS(filename=str(family.HOUSE_CSS)), WCSS(string=COVER_CSS)]
    rendered = HTML(string=doc, base_url=str(HERE)).render(stylesheets=sheets)
    rendered.write_pdf(str(OUT))
    print(f"OK {OUT}  pages={len(rendered.pages)}")


if __name__ == "__main__":
    main()
