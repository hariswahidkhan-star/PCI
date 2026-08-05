# PCI AI Formula Sheets

Premium quantitative references for the three credentials in the **PCI AI Project Leadership
Certification Suite**, built in the platform's own brand system.

Each credential ships in **two editions**, built from separate sources because they are genuinely
different artefacts.

### LinkedIn edition — 1080 x 1350 (4:5)

The shareable one. Sixteen curated formulas per credential, one idea per slide, type sized so it stays
legible when LinkedIn's document viewer scales a page down to phone width.

| Deck | Credential | Slides | PDF |
|---|---|---|---|
| [Project Controls Formulas](linkedin/01-pcl-ai-linkedin.md) | PCL-AI | 18 | `/downloads/pci-pcl-ai-formula-sheet-linkedin.pdf` |
| [Project Finance Formulas](linkedin/02-pfl-ai-linkedin.md) | PFL-AI | 18 | `/downloads/pci-pfl-ai-formula-sheet-linkedin.pdf` |
| [Project Management Formulas](linkedin/03-pml-ai-linkedin.md) | PML-AI | 19 | `/downloads/pci-pml-ai-formula-sheet-linkedin.pdf` |

### Reference edition — A4

The complete one. Every formula in the credential, dense, for printing and annotating at a desk.

| # | Sheet | Credential | Pages | PDF |
|---|---|---|---|---|
| 01 | [PCL-AI Formula Sheet](01-pcl-ai-formula-sheet.md) | Project Controls Leader | 9 | `/downloads/pci-pcl-ai-formula-sheet.pdf` |
| 02 | [PFL-AI Formula Sheet](02-pfl-ai-formula-sheet.md) | Project Finance Leader | 8 | `/downloads/pci-pfl-ai-formula-sheet.pdf` |
| 03 | [PML-AI Formula Sheet](03-pml-ai-formula-sheet.md) | Project Management Leader | 9 | `/downloads/pci-pml-ai-formula-sheet.pdf` |

### Why two editions and not one

A4 is the wrong page for LinkedIn. The document viewer fits a page to the screen, so on a ~390px phone an
A4 page renders at roughly 36% — putting 8pt reference type at about 3pt on screen. A complete sheet at
LinkedIn-legible type would run past 60 slides, which nobody scrolls.

So the LinkedIn edition curates rather than shrinks, and the reference edition stays complete. The
formulas, worked figures and "watch for" notes are identical wherever they overlap.

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

## Brand

Both editions use the platform's own design system, not a bespoke one:

| Token | Value | Source |
|---|---|---|
| Display type | **Archivo** 700–900 | `backend/wwwroot/assets/fonts/`, self-hosted |
| Body type | **Inter** 400–700 | same |
| Primary | `#1D4ED8` | `--red` in `assets/styles.css` (legacy name, blue value) |
| Bright | `#3B82F6` | `--magenta` |
| Deep | `#1E3A8A` | `--red-700` |
| Ink / noir | `#0F172A` / `#0E1525` | `--ink` / `--noir` |
| Surfaces | `#F1F5F9`, `#E3E8EF` | `--paper-2`, `--line` |
| Brand gradient | `linear-gradient(160deg,#3B82F6,#1D4ED8,#1E3A8A)` | `--grad-brand` |

Fonts are loaded from the repo by `@font-face`, so a build needs no network access and the PDFs carry the
same typefaces as the website.

## Building

```bash
pip install weasyprint markdown
cd docs/formula-sheets/build

python3 build_linkedin.py                      # the three LinkedIn decks
python3 build_formula_sheets.py                # the three A4 reference sheets

python3 build_linkedin.py 02-pfl-ai-linkedin.md          # or one at a time
python3 build_formula_sheets.py 02-pfl-ai-formula-sheet.md
```

Output goes to `backend/wwwroot/downloads/`, served at `/downloads/<file>.pdf`.
Stylesheets are `build/linkedin.css` (4:5 slides) and `build/formula.css` (A4 reference).

**Reference source format.** Line 1 `# Title`, blank line, `> Subtitle`. The builder generates the cover
and appends the notices block, so neither belongs in the body.

**LinkedIn slide conventions.** `## Heading` starts a slide. A first paragraph in bold becomes the
eyebrow label. A paragraph containing only a code span becomes a formula card, and consecutive ones merge
into a single card. A blockquote becomes the dark note card. The final `##` section is rendered as the
closing gradient slide.

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
