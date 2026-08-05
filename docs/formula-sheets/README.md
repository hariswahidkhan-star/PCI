# PCI AI Formula Sheets

Premium quantitative reference sheets for the three credentials in the **PCI AI Project Leadership
Certification Suite**. Each is a standalone A4 PDF a candidate can print, annotate and keep on a desk.

| # | Sheet | Credential | Pages | PDF |
|---|---|---|---|---|
| 01 | [PCL-AI Formula Sheet](01-pcl-ai-formula-sheet.md) | PCI AI Project Controls Leader | 8 | `/downloads/pci-pcl-ai-formula-sheet.pdf` |
| 02 | [PFL-AI Formula Sheet](02-pfl-ai-formula-sheet.md) | PCI AI Project Finance Leader | 8 | `/downloads/pci-pfl-ai-formula-sheet.pdf` |
| 03 | [PML-AI Formula Sheet](03-pml-ai-formula-sheet.md) | PCI AI Project Management Leader | 9 | `/downloads/pci-pml-ai-formula-sheet.pdf` |

## What makes these different from a formula list

A list of formulas is a commodity — every candidate can find one. These sheets carry the three things a
list does not:

- **A "watch for" column.** Against every formula, the mistake it actually attracts in practice. A
  correct expression still produces a wrong answer when `AC` excludes accruals, when interest is deducted
  before CFADS, or when the buyer's share ratio is inverted in a PTA calculation.
- **Decision aids.** *Which* EAC method, *which* progress measurement method, *which* coverage ratio —
  the professional judgement that sits on top of the arithmetic.
- **The ten most misapplied.** Each sheet closes with the errors that survive review because the
  arithmetic looks right.

Every worked example is internally consistent and reproducible from the sheet itself, so a candidate can
check their own working end to end.

## Verification

Every non-trivial formula was computed and checked before publication, including the round-trip
identities that make a sheet trustworthy:

- `BAC ÷ CPI` equals `AC × BAC ÷ EV`, and `TCPI (to EAC) = CPI` exactly when `EAC = BAC ÷ CPI`
- `TF = LS − ES` equals `LF − EF`
- `PI = PV(inflows) ÷ I₀` equals `1 + NPV ÷ I₀`
- `AF` equals the sum of the individual discount factors
- `PLCR > LLCR` for a project with a tail
- Debt sized from a target DSCR, when serviced, returns exactly that DSCR in every period
- At an actual cost equal to the `PTA`, the computed final price equals the ceiling price

Any change to a formula or a worked figure must be re-verified numerically before the sheet is rebuilt.

## Building

```bash
pip install weasyprint markdown
cd docs/formula-sheets/build
python3 build_formula_sheets.py                       # build all three
python3 build_formula_sheets.py 02-pfl-ai-formula-sheet.md   # build one
```

Output goes to `backend/wwwroot/downloads/pci-<credential>-formula-sheet.pdf`, served at
`/downloads/<file>.pdf`. The stylesheet is `build/formula.css` — the Knowledge Series navy-and-gold
system, tuned denser for quantitative reference, with formulas set in a tinted monospace panel.

**Source format.** Line 1 is `# Title`, then a blank line, then `> Subtitle` as a blockquote. The builder
generates the title page and appends the notices block, so neither belongs in the body.

## Notation rules

- **Formulas inside backticks use ASCII notation** — `^n`, `^t`, `^2`, `_i`, `_t`. Unicode
  super/subscripts are legible on screen but not at 7.7 pt in print, which defeats the purpose of a
  reference sheet. Full-height symbols (`Σ`, `Δ`, `σ`, `β`, `×`, `÷`, `−`, `≈`, `≥`) are fine and used.
- **The `PV` clash is resolved explicitly.** `PV` is Planned Value in earned-value contexts and present
  value in discounting contexts. Write "present value" in words, or `PV(x)`, when discounting.
- **Symbols follow the Body of Knowledge master symbol table** (`docs/bok/00-style-spine.md` §4). A symbol
  means the same thing on every sheet.

## Relationship to other material

`docs/downloads/master-formula-sheet.md` is the earlier candidate-download version for the controls
credential. Sheet 01 supersedes it in presentation and extends it with the watch-for column, the decision
aids and the worked examples; the formulas themselves agree, and the Knowledge Area references are
carried across unchanged. That older file still carries the pre-rename `PCP-AI` title and should not be
republished without correction — the credential is **PCL-AI**.

## Editorial standards

The same hard gates as the Knowledge Series:

- No accreditation, recognition or outcome claims.
- No standard's text reproduced; formulas stated in our own notation.
- Worked examples labelled as illustrative and internally consistent.
- Claims about what the examination provides are stated only where the Institute has published them.
  Where no blueprint exists yet, the sheet says the specification is confirmed for each published form
  rather than asserting a detail.
- British English. No emojis.
- The notices block appears on every PDF, unaltered.
