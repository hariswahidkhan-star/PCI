# PCI AI Formula Sheets

Premium quantitative references for the three credentials in the **PCI AI Project Leadership
Certification Suite**, built in the platform's own brand system.

Each credential ships in **two editions**, built from separate sources because they are genuinely
different artefacts.

### LinkedIn edition — 1080 x 1350 (4:5)

The shareable one, and **complete** — 41 to 58 formulas per credential, grouped into numbered sections
with full-bleed navy dividers, type sized to stay legible when LinkedIn's document viewer scales a page
down to phone width. Density is the point: a complete reference gets saved, a highlight reel gets
scrolled past.

**Formulas only — no worked examples.** Each entry carries the expression, what it means, and the mistake
it attracts. Checks are stated as properties a reader applies to their own numbers ("total float from the
starts must equal total float from the finishes") rather than as figures to read through. Worked
arithmetic lives in the reference edition.

**The PCL-AI deck leads on the positioning, because the positioning is the product.** A chartered
accountant knows IFRS 15, IAS 37 and IAS 23 but cannot build a schedule; a planning engineer knows
earned value and critical path but cannot say when revenue becomes recognisable. Every capital project is
measured in both languages and reconciled by neither profession. The deck is built in four parts —
*the language of the accounts*, *the language of delivery*, *where the two meet*, and *reference* — so the
40/40/20 weighting is argued rather than asserted. Part III is the material that exists nowhere else: the
accrual as the shared node, the translation table between delivery events and their accounting
consequences, and the onerous-contract test where a deteriorating EAC becomes an immediate IAS 37 loss.

The PML-AI deck's section 04, **the governance arithmetic**, is original to the PML-AI Body of Knowledge
— decision latency, gate net value, committee capacity, interface topology, the hundred-per-cent rule,
assessed total impact and baseline drift. No other body publishes it.

**Presentation follows the curriculum bodies.** Every table is a numbered **Exhibit**, sections are
numbered parts with full-bleed dividers, standards are cited by number and full title at principle level
and never reproduced, and each formula carries the Knowledge Area that develops it. Long tables tighten
automatically by row count rather than overflowing a fixed-height slide.

| Deck | Credential | Slides | PDF |
|---|---|---|---|
| [Two Languages, One Project](linkedin/01-pcl-ai-linkedin.md) | PCL-AI | 36 | `/downloads/pci-pcl-ai-formula-sheet-linkedin.pdf` |
| [Project Finance Formulas](linkedin/02-pfl-ai-linkedin.md) | PFL-AI | 27 | `/downloads/pci-pfl-ai-formula-sheet-linkedin.pdf` |
| [Project Management Formulas](linkedin/03-pml-ai-linkedin.md) | PML-AI | 32 | `/downloads/pci-pml-ai-formula-sheet-linkedin.pdf` |

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

So the LinkedIn edition curates rather than shrinks, and the reference edition stays complete. Formulas
and "watch for" notes are identical wherever the two overlap; the worked examples appear only in the
reference edition, which is what someone studying at a desk actually needs them for.

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

## Carousels

`social/make_honorary_carousel.py` builds a seven-slide deck and assembles it into a PDF,
because LinkedIn's carousel is a document post rather than an image gallery. It renders
through `build_social` so the slides get the same clipping and paint checks as everything
else.

**A dense sheet is not a post.** This was learned twice. A feed is read on a phone, where a
4:5 page renders at roughly a third of its width — so a one-pager's 14px body type arrives at
about 5px, and the reader sees a grey rectangle. The one-page graphics below are references:
things someone saves, opens full-screen, and returns to. A carousel is the format for the
feed itself, and its governing constraint is not how much fits but how little can be on a
slide and still earn the swipe.

The working rule: **body copy at ~34px on a 1080px slide, and if a slide carries more than
about sixty words, it is not a slide.**

## Social graphics

Single images for LinkedIn image posts, rendered to PNG at 2x from HTML in
`social/` by `social/build_social.py`. Output lands in
`backend/wwwroot/assets/social/`.

| Graphic | What it is |
|---|---|
| `pcl-ai-one-page.png` | **The complete PCL-AI formula sheet on one page** — all 68 formulas, 16 groups, three columns, every entry cited to its Knowledge Area |
| `ai-in-project-controls.png` | **Domain 13 on one page** — the governed workflow, then that same workflow applied across nine domains, with the refusal conditions and the assurance checklist beside it |
| `pcl-ai-40-40-20.png` | The 40/40/20 examination weighting, led by a proportional bar |

**Why the AI sheet leads with a workflow.** Domain 13 is 20% of the credential and the part
most easily written as opinion, so the sheet states a shape instead: governed data in,
the model proposes, the professional verifies, a signed result out — KA 13.5.1. Every
one of the nine application rows below it is that same shape, marked **AI** and **YOU** in
the gutter, so the argument is made by repetition rather than assertion. The refusal
conditions sit on the page beside the applications deliberately: a sheet that says only
where AI applies is marketing.

**Why the one-page sheet is built the way it is.** Sixty-three formulas cannot be read
at arm's length in a feed, and pretending otherwise produces a deck instead of a poster.
The design assumes two viewings: at feed size the reader sees a dense, obviously
organised reference and saves it; at full size every line is sharp and usable. Density
is the hook, so the structure has to carry meaning on its own — colour-coded area chips,
group headers with their domain, and a KA citation under every formula.

Two of the three are **generated from data** rather than hand-written, so the content can be
checked against the Body of Knowledge without reading CSS:

```bash
python3 -m http.server 8899 &            # serve the repo root; the HTML uses absolute paths

python3 docs/formula-sheets/social/make_one_page.py       # → pcl-ai-one-page.html
python3 docs/formula-sheets/social/make_ai_one_page.py    # → ai-in-project-controls.html

python3 docs/formula-sheets/social/build_social.py            # render all
python3 docs/formula-sheets/social/build_social.py pcl-ai-one-page
```

### The build refuses to report success on a bad render

Four checks, each added after a specific failure got through:

- **Brand blue must be present.** A wrong URL renders Chromium's blank error page, which
  sails through every completeness test because there is nothing to be incomplete.
- **No unpainted strip at the foot.** The headless viewport is shorter than the window, so
  a `100vh` layout stops short and the page background fills the gap.
- **The footer region must carry ink.** A layout that overruns pushes the footer off the
  frame, leaving only its rule behind. This tests the whole bottom 12%, not a fixed strip —
  pinning it to one graphic's footer position failed a good render whose footer sat higher.
- **Nothing may be clipped.** The strongest of the four, and the only one that is not a pixel
  test. A column that overruns inside `overflow:hidden` produces an image with *nothing wrong
  with it* — fully painted, footer intact, one row simply gone. So the builder renders a
  throwaway copy of the page carrying a script that asks the browser which element is
  clipping its own content, and reads the verdict out of the title. Injecting at build time
  rather than in each page means the hand-written graphics are checked the same as the
  generated ones.

## Brand

Both editions use the platform's own design system, not a bespoke one:

The palette is **sampled from the rendered site**, not read off variable names or guessed from the
logo file. Serve `backend/wwwroot`, screenshot `index.html` with the bundled Chromium, and sample the
pixels — that is the only method that has produced the right answer:

```bash
python3 -m http.server 8899 --directory backend/wwwroot &
/opt/pw-browsers/chromium-1194/chrome-linux/chrome --headless --no-sandbox \
  --window-size=1440,1600 --screenshot=site.png http://localhost:8899/index.html
```

| Token | Value | Where it appears on the site |
|---|---|---|
| Display type | **Archivo** 700–900 | every heading |
| Body type | **Inter** 400–700 | body, navigation |
| **Primary blue** | `#1D4ED8` | the `PCI AI` wordmark, buttons, links, emphasised text |
| **Crimson** | `#C13329` | *sparingly* — the full-stop dot after a headline, the chat bubble |
| Ink | `#0F172A` | headings, the top bar |
| Ground | `#FFFFFF` | the site is light, and so are the decks |
| Logo navy | `#1D3C92` → `#13245A` | the shield tile only |

**The mark is embedded**, not approximated: `assets/logo.svg` is inlined as a data URI and appears as the
full lockup (tile + `PCI AI` + institution line) on the cover, every section divider and the closing
slide, and as a small tile in every content footer.

**The signature gesture is the crimson dot** closing a headline — the site does it on its hero
("…control projects.") and the decks do it on dividers and the closing slide. Crimson is punctuation
here, never a field or a rule.

> **Two traps worth recording, both of which caught me.** First, `--red` in `assets/styles.css` holds
> `#1D4ED8`, a blue — the variable kept its name through a palette change, so reading the name gives the
> wrong colour with total confidence. Second, `logo.svg` alone is also misleading: the mark is a navy
> tile with gold lettering, but the *site* is light with blue as its primary and gold nowhere in the UI.
> Sample the rendered page.

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
