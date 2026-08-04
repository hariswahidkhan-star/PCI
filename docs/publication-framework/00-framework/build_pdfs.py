#!/usr/bin/env python3
"""Build the PCI publication-framework PDFs from the Markdown manuscripts.

Every document in the framework becomes one A4 PDF in the Institute's house style:
a title page, a notice page carrying the founding-stage disclaimers, a contents
page with real page numbers, then the body.

    python3 docs/publication-framework/00-framework/build_pdfs.py            # everything
    python3 .../build_pdfs.py --series s09-best-practice-guides              # one series
    python3 .../build_pdfs.py --id BPG-08                                    # one document
    python3 .../build_pdfs.py --packs                                        # + per-series packs

Output goes to docs/publication-framework/_pdf/, mirroring the series folders.

Requires: markdown, weasyprint.  Install with:  pip install markdown weasyprint

Unresolved [CONFIRM: ...] placeholders are rendered highlighted rather than hidden.
A document that is not ready to publish should look unready on the page.
"""

from __future__ import annotations

import argparse
import datetime
import html as htmllib
import pathlib
import re
import sys

try:
    import markdown
except ImportError:  # pragma: no cover
    sys.exit("markdown is not installed.  pip install markdown weasyprint")

FRAMEWORK = pathlib.Path(__file__).resolve().parent.parent
CSS_PATH = FRAMEWORK / "00-framework" / "print.css"
OUT_ROOT = FRAMEWORK / "_pdf"

ORG = "Project Controls Institute Global, Inc."
SLOGAN = "AI proposes. The professional disposes."
THEME = "Finance intelligently. Control predictively. Deliver successfully."

SERIES_NAMES = {
    "s01": "Body of Knowledge — Executive Summary",
    "s02": "AI in Project Controls Guide",
    "s03": "Competency Frameworks",
    "s04": "Code of Ethics",
    "s05": "Certification Handbook",
    "s06": "Exam Blueprint",
    "s07": "Career Roadmap",
    "s08": "Salary and Skills Report",
    "s09": "Best Practice Guides",
    "s10": "Free Templates",
}

# The notice is deliberately explicit. Every clause here corresponds to a real constraint
# recorded in CANONICAL-FACTS.md; none of it is boilerplate softening.
NOTICE = """
<p><strong>Notice.</strong> This document is an educational publication of {org}, and does not
constitute accounting, tax, legal, financial or other professional advice. It must not be relied
upon as such. Where a topic depends on the contract or the governing law, entitlement and treatment
vary by jurisdiction; take local advice.</p>

<p><strong>Standards.</strong> Standards and frameworks are referred to by name and described in this
publication's own words. No standard's text, tables, diagrams or question banks are reproduced. All
trademarks remain the property of their respective owners, and no endorsement by any standards body,
employer or vendor is implied or should be inferred.</p>

<p><strong>Status.</strong> {org} is a Delaware Non-Stock Corporation. The Institute has been developed
with reference to ISO/IEC 17024 personnel-certification principles and is <strong>not accredited</strong>
by ANAB, IAS or any other accreditation body. It intends to seek 501(c)(3) recognition, which has not
been granted. No governmental approval, academic equivalence, or guaranteed employment, promotion or
salary outcome is claimed or implied.</p>

<p><strong>Examples.</strong> All worked examples are illustrative and fictional. Figures are chosen to
demonstrate a method, not to describe any real project, organisation or market. Sample examination
items, where they appear, are study material maintained separately from any live examination bank.</p>

<p><strong>Completeness.</strong> Passages marked <span class="confirm">[CONFIRM: …]</span> are
operational or market facts that have not yet been confirmed from a cited source. A document carrying
them is a working draft and is not approved for publication.</p>

<p>&#169; {year} {org} &#183; All rights reserved.</p>
"""


def parse_front_matter(text: str) -> tuple[dict, str]:
    """Return (metadata, body). Handles the subset of YAML the schema actually uses:
    top-level scalars, folded scalars (`>`) and inline lists. Nested blocks are skipped."""
    if not text.startswith("---"):
        return {}, text
    end = text.find("\n---", 3)
    if end == -1:
        return {}, text
    raw = text[3:end].strip("\n")
    body = text[end + 4:].lstrip("\n")

    meta: dict[str, str] = {}
    key = None
    folded: list[str] = []
    for line in raw.split("\n"):
        if not line.strip():
            continue
        # Continuation of a folded scalar, or a nested block we ignore.
        if line.startswith((" ", "\t")):
            if key is not None and folded is not None:
                folded.append(line.strip())
            continue
        if key is not None and folded:
            meta[key] = " ".join(folded)
        key, folded = None, []
        m = re.match(r"^([A-Za-z_][A-Za-z0-9_]*):\s*(.*)$", line)
        if not m:
            continue
        k, v = m.group(1), m.group(2).strip()
        if v in (">", "|", ">-", "|-"):
            key, folded = k, []
        elif v == "":
            key, folded = None, []          # nested block — not needed for the cover
        else:
            meta[k] = v.strip('"\'')
    if key is not None and folded:
        meta[key] = " ".join(folded)
    return meta, body


def extract_title(body: str) -> tuple[str, str, str]:
    """Pull the leading '# Title' and optional '> subtitle' out of the body."""
    lines = body.split("\n")
    title, subtitle, start = "", "", 0
    for i, line in enumerate(lines):
        if line.startswith("# "):
            title = line[2:].strip()
            start = i + 1
            break
        if line.strip():
            break
    for j in range(start, min(start + 3, len(lines))):
        if lines[j].startswith("> "):
            subtitle = lines[j][2:].strip()
            start = j + 1
            break
        if lines[j].strip():
            break
    return title, subtitle, "\n".join(lines[start:]).lstrip("\n")


def render_body(body_md: str) -> tuple[str, str]:
    md = markdown.Markdown(
        extensions=["tables", "fenced_code", "toc", "sane_lists", "attr_list", "footnotes"],
        extension_configs={"toc": {"toc_depth": "2-3"}},
    )
    return md.convert(body_md), md.toc


def post_process(html: str) -> str:
    # The template's two lead-in blocks get a panel treatment.
    html = re.sub(
        r"<p>(<strong>(?:In one paragraph|Who this is for)\.</strong>.*?)</p>",
        r'<p class="lead">\1</p>',
        html,
        flags=re.S,
    )
    # Placeholders must be visible on the page, not buried mid-sentence.
    html = re.sub(r"\[CONFIRM:([^\]]*)\]", r'<span class="confirm">[CONFIRM:\1]</span>', html)
    return html


def build(md_path: pathlib.Path, out_dir: pathlib.Path) -> tuple[pathlib.Path, int]:
    from weasyprint import HTML, CSS

    text = md_path.read_text(encoding="utf-8")
    meta, body = parse_front_matter(text)
    title, subtitle, body_md = extract_title(body)

    title = meta.get("title", title) or md_path.stem
    subtitle = meta.get("subtitle", subtitle)
    doc_id = meta.get("id", "")
    series_key = md_path.parent.name[:3]
    series = meta.get("series_name") or SERIES_NAMES.get(series_key, "Publication Framework")
    version = meta.get("version", "1.0")
    status = meta.get("status", "draft")
    date = meta.get("date", datetime.date.today().isoformat())
    year = date[:4] if re.match(r"^\d{4}", date) else str(datetime.date.today().year)

    body_html, toc_html = render_body(body_md)
    body_html = post_process(body_html)
    placeholders = len(re.findall(r"\[CONFIRM:", text))

    esc = htmllib.escape
    meta_rows = "".join(
        f"<tr><th>{esc(k)}</th><td>{esc(v)}</td></tr>"
        for k, v in [
            ("Document", doc_id),
            ("Series", series),
            ("Version", version),
            ("Status", status),
            ("Date", date),
            ("Unresolved placeholders", str(placeholders)),
        ]
        if v
    )

    summary = meta.get("summary", "")
    summary_block = f"<h2>Summary</h2><p>{esc(summary)}</p>" if summary else ""

    doc = f"""<!DOCTYPE html>
<html lang="en-GB"><head><meta charset="utf-8"><title>{esc(title)}</title></head>
<body>
<div class="titlepage">
  <div class="stringsrc">{esc(series)}</div>
  <div class="stringsrc2">{esc(title)}</div>
  {'<div class="docid">' + esc(doc_id) + ' &#183; ' + esc(series) + '</div>' if doc_id else ''}
  <h1>{esc(title)}</h1>
  <div class="subtitle">{esc(subtitle)}</div>
  <div class="rule"></div>
  <div class="meta">
    VERSION {esc(version)} &#183; {esc(status.upper())}<br/>
    {esc(date)}<br/><br/>
    {esc(ORG).upper()}<br/>
    <span class="theme">{esc(SLOGAN)}</span>
  </div>
</div>

<div class="frontmatter">
  {summary_block}
  <h2>About this document</h2>
  <table class="fmtable"><tbody>{meta_rows}</tbody></table>
  <div class="notice">{NOTICE.format(org=esc(ORG), year=year)}</div>
</div>

<nav class="toc">{toc_html}</nav>

<h1>{esc(title)}</h1>
{body_html}
</body></html>"""

    out_dir.mkdir(parents=True, exist_ok=True)
    pdf_path = out_dir / f"{md_path.stem}.pdf"
    rendered = HTML(string=doc, base_url=str(md_path.parent)).render(
        stylesheets=[CSS(filename=str(CSS_PATH))]
    )
    rendered.write_pdf(str(pdf_path))
    return pdf_path, len(rendered.pages)


def manuscripts() -> list[pathlib.Path]:
    return sorted(FRAMEWORK.glob("s*/[A-Z][A-Z][A-Z]-[0-9][0-9]-*.md"))


def framework_docs() -> list[pathlib.Path]:
    return sorted(FRAMEWORK.glob("00-framework/*.md")) + [FRAMEWORK / "README.md"]


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument("--series", help="build one series directory, e.g. s09-best-practice-guides")
    ap.add_argument("--id", help="build one document by id, e.g. BPG-08")
    ap.add_argument("--framework", action="store_true", help="also build the framework documents")
    ap.add_argument("--packs", action="store_true", help="also build one combined PDF per series")
    args = ap.parse_args()

    files = manuscripts()
    if args.series:
        files = [f for f in files if f.parent.name == args.series]
    if args.id:
        files = [f for f in files if f.stem.startswith(args.id)]
    if args.framework and not (args.series or args.id):
        files += framework_docs()

    if not files:
        print("no matching documents", file=sys.stderr)
        return 1

    total_pages, built, failed = 0, 0, []
    for md_path in files:
        rel = md_path.parent.name if md_path.parent != FRAMEWORK else "00-framework"
        try:
            pdf, pages = build(md_path, OUT_ROOT / rel)
            total_pages += pages
            built += 1
            print(f"OK  {rel}/{pdf.name}  {pages}pp")
        except Exception as exc:  # keep going; report at the end
            failed.append((md_path.name, str(exc)))
            print(f"FAIL {md_path.name}: {exc}", file=sys.stderr)

    if args.packs:
        build_packs()

    print(f"\nbuilt {built} PDFs, {total_pages} pages, output in {OUT_ROOT}")
    if failed:
        print(f"{len(failed)} failed:", file=sys.stderr)
        for name, err in failed:
            print(f"  {name}: {err}", file=sys.stderr)
        return 1
    return 0


def build_packs() -> None:
    """One combined PDF per series, for the Downloads Centre and LinkedIn document posts."""
    from pypdf import PdfWriter

    for series_dir in sorted(OUT_ROOT.glob("s*")):
        pdfs = sorted(p for p in series_dir.glob("*.pdf") if not p.stem.endswith("-pack"))
        if not pdfs:
            continue
        writer = PdfWriter()
        for p in pdfs:
            writer.append(str(p))
        out = OUT_ROOT / f"{series_dir.name}-pack.pdf"
        with open(out, "wb") as fh:
            writer.write(fh)
        print(f"PACK {out.name}  ({len(pdfs)} documents)")


if __name__ == "__main__":
    raise SystemExit(main())
