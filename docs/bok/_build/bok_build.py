#!/usr/bin/env python3
"""Assemble the PCL-AI Body of Knowledge Markdown chapters into one styled, print-ready HTML.
Usage: python3 bok_build.py <out.html>   (chapters read from ../docs/bok)"""
import sys, os, re, glob, html
import markdown

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
BOK = os.path.join(ROOT, "docs", "bok")
OUT = sys.argv[1] if len(sys.argv) > 1 else os.path.join(ROOT, "scratchpad", "bok.html")

DOMAIN_TITLES = {
    "01": "Foundations of Accounting for Project Controls",
    "02": "Financial Reporting & the Standards",
    "03": "Budgeting & Forecasting",
    "04": "Performance Management, Variance Analysis & Management Reporting",
    "05": "Cost Management & Cost Control",
    "06": "Earned Value Management & Forecasting",
    "07": "Contracts, Commercial Management, BoQ, Invoicing & Revenue",
    "08": "Project Management Lifecycle",
    "09": "Agile, Scrum & Adaptive Delivery",
    "10": "Project Scheduling",
    "11": "Business Process Cycles (O2C, P2P & Controls)",
    "12": "Risk Management for Project Controls",
    "13": "AI for Project Controls & Project Management",
}

def ka_sort_key(path):
    m = re.search(r"/(\d+)\.(\d+)\.md$", path)
    return (int(m.group(1)), int(m.group(2))) if m else (999, 999)

# collect chapters grouped by domain dir
domain_dirs = sorted(d for d in os.listdir(BOK) if re.match(r"\d{2}-", d))
md = markdown.Markdown(extensions=["tables", "fenced_code", "sane_lists", "attr_list", "toc"])

chapters = []   # (domain_num, ka, title, html)
toc_rows = []
for d in domain_dirs:
    dnum = d[:2]
    files = sorted(glob.glob(os.path.join(BOK, d, "*.md")), key=ka_sort_key)
    for f in files:
        raw = open(f, encoding="utf-8").read()
        m = re.search(r"/(\d+\.\d+)\.md$", f)
        ka = m.group(1) if m else os.path.basename(f)
        # pull the KA title from the "## KA x.y — Title" line
        tm = re.search(r"^##\s*KA\s*[\d.]+\s*[—-]\s*(.+)$", raw, re.M)
        title = tm.group(1).strip() if tm else ka
        md.reset()
        body = md.convert(raw)
        chapters.append((dnum, ka, title, body))
        toc_rows.append((dnum, ka, title))

# build TOC grouped by domain
toc_html = ['<nav class="toc"><h1>Contents</h1>']
cur = None
for dnum, ka, title in toc_rows:
    if dnum != cur:
        cur = dnum
        toc_html.append(f'<div class="toc-dom">Domain {int(dnum)} — {html.escape(DOMAIN_TITLES.get(dnum, ""))}</div>')
    toc_html.append(f'<div class="toc-ka"><span class="toc-num">{ka}</span> {html.escape(title)}</div>')
toc_html.append("</nav>")

body_html = []
for dnum, ka, title, chtml in chapters:
    body_html.append(f'<section class="chapter" id="ka-{ka}">{chtml}</section>')

# ---- Appendices (auto-generated) ----
all_text = "\n".join(open(f, encoding="utf-8").read() for d in domain_dirs
                     for f in glob.glob(os.path.join(BOK, d, "*.md")))

# Appendix A — master formula & symbol sheet (authoritative, from the style spine §5)
APPENDIX_A_MD = """# Appendix A — Master Formula & Symbol Sheet

This sheet consolidates the canonical symbols and formulae used identically throughout the Body of
Knowledge. Where a Knowledge Area introduces additional notation it is defined locally and, where
load-bearing across chapters, added here.

## Earned Value & forecasting (Domains 3, 4, 6, 8, 9, 10)

| Symbol | Term | Definition / formula |
|---|---|---|
| PV (BCWS) | Planned Value | Budgeted cost of work scheduled to date |
| EV (BCWP) | Earned Value | Budgeted cost of work performed to date |
| AC (ACWP) | Actual Cost | Actual cost of work performed to date |
| BAC | Budget at Completion | Total budgeted cost of the project |
| CV | Cost Variance | `CV = EV − AC` |
| SV | Schedule Variance | `SV = EV − PV` |
| CPI | Cost Performance Index | `CPI = EV / AC` |
| SPI | Schedule Performance Index | `SPI = EV / PV` |
| EAC | Estimate at Completion | `AC + (BAC − EV)` · `BAC / CPI` · `AC + (BAC − EV)/(CPI × SPI)` |
| ETC | Estimate to Complete | `ETC = EAC − AC` |
| VAC | Variance at Completion | `VAC = BAC − EAC` |
| TCPI | To-Complete Performance Index | `(BAC − EV)/(BAC − AC)` or `(BAC − EV)/(EAC − AC)` |
| ES | Earned Schedule | Time at which the current EV was planned to be earned |
| SV(t), SPI(t) | Time-based variance / index | `SV(t) = ES − AT` · `SPI(t) = ES / AT` |

**Common EAC assumptions:** future work at budget → `EAC = AC + (BAC − EV)`; past cost efficiency continues
→ `EAC = BAC / CPI`; cost and schedule pressure continue → `EAC = AC + (BAC − EV)/(CPI × SPI)`.

## Accounting, finance & revenue (Domains 1–2, 5, 7)

- Accounting equation: `Assets = Liabilities + Equity`
- Percentage complete (cost-to-cost input method): `% complete = costs incurred to date / total estimated costs`
- Contract asset / (liability): `revenue recognised − amounts billed` (positive = contract asset; negative = contract liability)
- Straight-line depreciation: `(cost − residual value) / useful life`

## Risk, scheduling & agile (Domains 9, 10, 12)

- Expected monetary value: `EMV = probability × impact` (summed across the register)
- Total float: `TF = LS − ES = LF − EF`; critical path = longest path / zero-float chain
- Little's Law: `lead time = WIP / throughput`
- Velocity: `story points completed per sprint`; release forecast = `remaining points / velocity`
- AgileEVM reuses EV/CPI/SPI/EAC with the scope-variable assumptions stated per Knowledge Area 9.5.

*Currency convention: USD primary; SAR shown at an illustrative SAR 3.75 = USD 1 where used. British English throughout.*
"""

# Appendix B — standards & frameworks referenced (auto-extracted)
std_patterns = [
    r"IFRS\s?\d+", r"IAS\s?\d+", r"ISO/IEC\s?\d+", r"ISO\s?\d+(?:\s?:\s?\d+)?",
    r"AACE(?:\sTCM| RP\s?\d+[A-Z]?-\d+)?", r"PMBOK", r"APM Body of Knowledge",
    r"FIDIC", r"NEC\d?", r"RICS(?:\sNRM\d| POMI)?", r"CESMM\d?", r"COSO",
    r"NIST AI RMF", r"OECD AI Principles", r"EU AI Act", r"GDPR", r"Agile Manifesto",
    r"Scrum Guide", r"SAFe", r"LeSS", r"Kanban", r"ISAE\s?\d+", r"SOC\s?\d+",
    r"ISO 31000", r"ISO/IEC 17024", r"ISO/IEC 42001", r"IIA Three Lines",
]
found = set()
for pat in std_patterns:
    for mm in re.findall(pat, all_text):
        found.add(re.sub(r"\s+", " ", mm).strip())
# tidy: drop noise
std_list = sorted(x for x in found if len(x) > 2)
APPENDIX_B_MD = "# Appendix B — Standards & Frameworks Referenced\n\n" \
    "The Body of Knowledge references the following real standards and frameworks **by name and at " \
    "awareness level**, described in the authors' own words. It never reproduces copyrighted standard " \
    "or contract text verbatim, and clause numbers are cited only where independently verifiable. Every " \
    "reference must be confirmed against the current official source during SME verification.\n\n" \
    + "\n".join(f"- {s}" for s in std_list) + "\n"

# Appendix C — note on per-KA study apparatus
APPENDIX_C_MD = """# Appendix C — Key Terms, Sample MCQs & Self-Check Answers

Each Knowledge Area is self-contained for study: it closes with a **Key terms** box, a set of **Sample
MCQs** (four options, the correct answer marked, with a rationale and a cognitive-level tag), and
**Self-check questions with answers**. These are drawn from the same blueprint as, but kept separate
from, the live examination bank, and are never reused verbatim as live items.

**For SME verification.** Every Knowledge Area also ends with a *For SME verification* list flagging the
worked numbers, standard references and AI-capability claims a qualified finance, agile or AI subject-
matter expert must confirm before this draft is finalised into the certified text and exam blueprint.
Notable cross-chapter reconciliations flagged for consolidation include: the $600,000 performance-
measurement baseline vs the $633,000 full cost baseline (escalation/contingency) convention; the
approved-change baseline drift (BAC $600,000 → $643,000 after VO-01); and the illustrative schedule
cadence (week- vs month-based) for the running Pipeline Segment 4B example.
"""

for amd in (APPENDIX_A_MD, APPENDIX_B_MD, APPENDIX_C_MD):
    md.reset()
    body_html.append(f'<section class="chapter appendix">{md.convert(amd)}</section>')

CSS = """
:root{--red:#1D4ED8;--ink:#0F172A;--slate:#475569;--mist:#64748B;--line:#E3E8EF;--paper2:#F1F5F9;}
*{box-sizing:border-box;}
body{font-family:Georgia,'Times New Roman',serif;color:var(--ink);font-size:10.5pt;line-height:1.5;margin:0;}
h1,h2,h3,h4,.toc h1,.cover *{font-family:'Helvetica Neue',Arial,sans-serif;}
.cover{height:262mm;display:flex;flex-direction:column;justify-content:center;align-items:center;text-align:center;page-break-after:always;}
.cover .kicker{color:var(--red);font-weight:700;letter-spacing:.18em;text-transform:uppercase;font-size:11pt;margin-bottom:14mm;}
.cover h1{font-size:30pt;line-height:1.15;margin:0 0 8mm;max-width:150mm;}
.cover .sub{color:var(--slate);font-size:13pt;max-width:130mm;}
.cover .foot{margin-top:24mm;color:var(--mist);font-size:9.5pt;}
.cover .rule{width:60mm;height:3px;background:var(--red);margin:10mm 0;}
.toc{page-break-after:always;}
.toc h1{font-size:20pt;border-bottom:3px solid var(--red);padding-bottom:3mm;margin-bottom:6mm;}
.toc-dom{font-weight:700;color:var(--red);margin:5mm 0 1.5mm;font-size:11pt;}
.toc-ka{color:var(--slate);font-size:9.5pt;margin:.6mm 0 .6mm 6mm;}
.toc-num{display:inline-block;min-width:11mm;color:var(--ink);font-weight:600;}
.chapter{page-break-before:always;}
.chapter h1{font-size:19pt;color:var(--red);border-bottom:2px solid var(--line);padding-bottom:2mm;margin:0 0 4mm;}
.chapter h2{font-size:15pt;margin:6mm 0 2mm;}
.chapter h3{font-size:12pt;color:var(--slate);margin:5mm 0 1.5mm;}
.chapter h4{font-size:10.5pt;margin:3mm 0 1mm;}
p{margin:0 0 2.4mm;}
table{border-collapse:collapse;width:100%;margin:3mm 0;font-size:9pt;font-family:'Helvetica Neue',Arial,sans-serif;page-break-inside:avoid;}
th,td{border:1px solid var(--line);padding:1.4mm 2mm;text-align:left;vertical-align:top;}
th{background:var(--paper2);font-weight:700;}
code{font-family:'Courier New',monospace;background:var(--paper2);padding:.3mm 1mm;border-radius:2px;font-size:9pt;}
pre{background:var(--paper2);border:1px solid var(--line);border-radius:3px;padding:2.5mm;overflow-x:auto;font-size:8.5pt;page-break-inside:avoid;}
pre code{background:none;padding:0;}
blockquote{border-left:3px solid var(--red);margin:2mm 0;padding:.5mm 0 .5mm 4mm;color:var(--slate);}
ul,ol{margin:0 0 2.4mm;padding-left:6mm;}
li{margin:.6mm 0;}
strong{color:var(--ink);}
hr{border:none;border-top:1px solid var(--line);margin:4mm 0;}
h1,h2,h3,h4{page-break-after:avoid;}
"""

COVER = """
<div class="cover">
  <div class="kicker">Project Controls Institute</div>
  <h1>PCI AI Project Controls Leader<br>Body of Knowledge</h1>
  <div class="rule"></div>
  <div class="sub">The authoritative reference defining what a PCI AI Project Controls Leader must know — financial reporting &amp; accounting, project management, and the governed application of AI.</div>
  <div class="foot">Version 1 · SME-verifiable draft<br>An AI first draft is a strong starting point, not the final certified text — every worked number, standard reference and AI claim must be checked by a qualified subject-matter expert before finalisation.</div>
</div>
"""

doc = f"<style>{CSS}</style>\n{COVER}\n{''.join(toc_html)}\n{''.join(body_html)}"
open(OUT, "w", encoding="utf-8").write(doc)
print(f"wrote {OUT}: {len(chapters)} chapters, {len(doc):,} bytes")
