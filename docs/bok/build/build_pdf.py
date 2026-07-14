#!/usr/bin/env python3
"""Build the PCP-AI Body of Knowledge PDF from the markdown corpus.

Concatenates the book files in reading order, converts to HTML via pandoc,
styles with print.css (A4), renders with WeasyPrint, and reports the page count.

Usage:  python3 build_pdf.py [output.pdf]
"""
import subprocess, sys, datetime, pathlib

BOK = pathlib.Path(__file__).resolve().parent.parent
BUILD = BOK / "build"
OUT = pathlib.Path(sys.argv[1]) if len(sys.argv) > 1 else BUILD / "pcp-ai-bok.pdf"

ORDER = [
    "00-style-spine.md",
    "domain-01-foundations-of-accounting.md",
    "domain-02-financial-reporting.md",
    "domain-03-budgeting-forecasting.md",
    "domain-04-performance-variance-reporting.md",
    "domain-05-cost-management.md",
    "domain-06-evm-eac.md",
    "domain-07-contracts-commercial.md",
    "domain-08-pm-lifecycle.md",
    "domain-09-agile-adaptive.md",
    "domain-10-scheduling.md",
    "domain-11-process-cycles.md",
    "domain-12-risk-management.md",
    "domain-13-ai-for-project-controls.md",
    "appendices.md",
]

TITLE_HTML = f"""
<div class="titlepage">
  <h1>PCP-AI Body of Knowledge</h1>
  <div class="subtitle">The reference for the Certified Project Controls Professional — AI<br/>
  Project controls · project finance · the governed use of AI</div>
  <div class="meta">Version 1 (working draft — pending SME review)<br/>
  Built {datetime.date.today().isoformat()}<br/><br/>
  Project Controls Institute Global, Inc.<br/>
  <em>AI proposes. The professional disposes.</em></div>
</div>
"""

def inject_figures(corpus: str) -> str:
    """Insert each rendered SVG immediately before its figure-spec blockquote.

    Spec lines look like:  > **Fig 6.3.1 — The EAC fan.** ...
    The spec block remains beneath the image as its extended caption.
    """
    import re
    out, injected = [], 0
    for line in corpus.split("\n"):
        m = re.match(r"> \*\*Fig (\d+\.\d+\.\d+) — ([^.*]+)", line)
        if m:
            fid, title = m.group(1), m.group(2).strip().rstrip(".")
            svg = BUILD / "figures" / f"fig_{fid.replace('.', '_')}.svg"
            if svg.exists():
                out.append(f'<figure><img src="figures/{svg.name}"/>'
                           f"<figcaption>Fig {fid} — {title}</figcaption></figure>\n")
                injected += 1
        out.append(line)
    print(f"figures injected: {injected}")
    return "\n".join(out)


def main() -> None:
    # 1. Concatenate the corpus in reading order; inject rendered figures.
    corpus = "\n\n".join((BOK / name).read_text(encoding="utf-8") for name in ORDER)
    corpus = inject_figures(corpus)
    combined = BUILD / "_combined.md"
    combined.write_text(corpus, encoding="utf-8")

    # 2. Markdown -> HTML body via pandoc (with a generated table of contents).
    html_body = subprocess.run(
        ["pandoc", str(combined), "-f", "gfm", "-t", "html", "--toc", "--toc-depth=2", "-s",
         "--metadata", "title=PCP-AI Body of Knowledge"],
        capture_output=True, text=True, check=True).stdout
    # Inject the title page right after <body> and drop pandoc's default header block.
    html_body = html_body.replace("<body>", "<body>" + TITLE_HTML, 1)
    html_body = html_body.replace('<header id="title-block-header">', '<header id="title-block-header" style="display:none">', 1)
    html_file = BUILD / "_combined.html"
    html_file.write_text(html_body, encoding="utf-8")

    # 3. HTML -> PDF via WeasyPrint.
    from weasyprint import HTML, CSS  # imported late so --help stays fast
    doc = HTML(filename=str(html_file)).render(stylesheets=[CSS(filename=str(BUILD / "print.css"))])
    doc.write_pdf(str(OUT))

    # 4. Prepend the full-bleed book cover as a native PDF page (exact A4, no margins).
    cover = BUILD / "cover.png"
    pages = len(doc.pages)
    if cover.exists():
        import fitz  # PyMuPDF
        pdf = fitz.open(str(OUT))
        page = pdf.new_page(pno=0, width=595.276, height=841.890)  # A4 in points
        page.insert_image(page.rect, filename=str(cover))
        tmp = OUT.with_suffix(".tmp.pdf")
        pdf.save(str(tmp)); pdf.close()
        tmp.replace(OUT)
        pages += 1
    print(f"OK {OUT}  pages={pages}")

if __name__ == "__main__":
    main()
