# PCI Book Pattern Specification

**Programme:** PML-AI and PFL-AI Bodies of Knowledge
**Derived from:** the previously approved PCI book — the **PCL-AI Body of Knowledge, First Edition**
(authored and stored under its pre-rename designation **PCL-AI**; the credential was renamed in place
to PCL-AI, "PCI AI Project Controls Leader", by the Master Naming Update — `backend/Data/MultiCert.cs`)
**Status:** Phase 0 deliverable — the binding publishing pattern for the two new books
**Owner:** Chief editorial coordinator
**Version:** 1.0

---

## 1. The previous book: what exists, and which version is approved

The repository contains **one previously completed PCI book** with two source generations and one
published PDF:

| Artefact | Location | Role |
|---|---|---|
| Published First Edition PDF (~950–970 pp, A4) | `docs/bok/PCL-AI-Body-of-Knowledge-v1.pdf` | The **published approved edition** (produced by the `_build/` Chromium pipeline; PDF producer `Skia/PDF`, HeadlessChrome) |
| Per-KA source corpus (61 KA files in 13 folders) | `docs/bok/<NN-slug>/<D.K>.md` + `docs/bok/_build/` | **First-generation** source that produced the v1 PDF |
| Whole-domain source corpus (13 domain files + appendices, ~207k words) | `docs/bok/domain-*.md`, `appendices.md` | **Second-generation (current) maintained source**, listed as the live corpus in `docs/bok/README.md` |
| Style Spine (authoring standard) | `docs/bok/00-style-spine.md` | The book family's documented authoring standard — terminology, symbols, worked-example/figure/MCQ formats, citation rules |
| Premium print pipeline | `docs/bok/build/build_pdf.py` + `print.css` + `figures/*.svg` + `cover.jpg` | **Current typesetting standard** (pandoc → WeasyPrint, A4, premium chapter/part openers, generated TOC + index) |
| Served student editions | `backend/books/*.md` + `*.pdf`, delivered through the platform's document system | Distribution channel (watermarking/versioning handled by the platform, see §12) |

**Ruling (Phase 0):** the pattern for PML-AI and PFL-AI is taken from the **second-generation corpus +
the `build/` premium pipeline**, which is the maintained standard the Institute's README designates as
current. The first-generation per-KA corpus and `_build/` pipeline are treated as historical and are
**not** the pattern source. The published v1 PDF is used as evidence of the approved *editorial* pattern
(front matter, structure, apparatus); the `build/` pipeline is the approved *typesetting* pattern.

---

## 2. Book identity and front matter

Pattern (from `build/build_pdf.py` TITLE_HTML and the v1 edition):

1. **Full-bleed cover** — exact A4 native PDF page, no margins, prepended after render
   (`build/cover.jpg`). Restrained composition; brand colours; no stock photography.
2. **Title page** — top-aligned block: book designation + "Body of Knowledge" (Inter 800, 34 pt),
   subtitle line naming the credential in full and the book's three-part promise, a short crimson rule,
   then the edition line, year, publisher (**Project Controls Institute Global**) and the governing
   principle in italics.
3. **Copyright & edition notice** (own page): © year + publisher, all-rights-reserved formula; the
   continuous-review edition statement; the **educational disclaimer** (not accounting/tax/legal/
   financial/professional advice; jurisdiction and time variability; illustrative rates); the
   **standards-and-trademarks paragraph** (frameworks referred to by name, described in the book's own
   words, no text reproduced, trademarks acknowledged to their owners, no endorsement implied, no
   governmental approval implied); the **originality paragraph** (all examples/cases/figures/templates/
   questions original; organisations fictional; sample questions separate from live exam banks).
4. **How to use this reference** (own page): the domain-group map with weightings; the numbered
   Domain → Knowledge Area → Topic hierarchy; the one-shape-per-domain explanation; guidance for three
   reader modes (study / practice / exam preparation).
5. **Contents** — generated, two levels (domains + their `##` sections), leader dots, page numbers.

PML-AI/PFL-AI adaptations required: new titles, subtitles, credential names, weightings and part maps;
PFL-AI's copyright page additionally carries the extended finance/investment disclaimer (see decision
register D-14).

---

## 3. Structural hierarchy

- **Part** → **Domain (chapter)** → **Knowledge Area (KA)** → **Topic**, numbered `Domain.KA.Topic`
  (e.g. `6.3.2`). Cross-references are **by number, never by repeating content**.
- Parts carry divider pages: ghost part number (112 pt, near-white), part kicker in crimson small caps,
  part title (24 pt Inter 800), a 3–5 line part description naming the domains and their weighting
  share, and a brand-blue bar.
- Domain openers: ghost two-digit chapter number (56 pt), crimson "DOMAIN N" kicker, title (21 pt
  Inter 800), crimson short rule + full-width blue hairline. The `h1` sets the running-header string.
- KA openers (`h2.ka`): ink top rule; "KNOWLEDGE AREA N.N" kicker in crimson small caps; title
  (13.5 pt Inter 800). Topics are `### N.N.N Title` headings.
- Recurring apparatus headings (Key terms / Sample MCQs / Self-check) render as small-cap mini-heads
  with a crimson square marker.

## 4. The domain (chapter) shape

Every domain follows one shape (evidenced across all 13 PCL-AI domains):

1. `# Domain N — Title` + a **binding blockquote**: group, page target, "binds to the Style Spine",
   language/currency conventions.
2. `## Why this domain exists` — narrative motivation ending with **learning objectives** ("After this
   domain a candidate can …") and, in quantitative domains, a **master worked project** whose figures
   run through the whole chapter.
3. `## Knowledge Area N.1 … N.k` — each opens with its topic list (`*Topics: N.K.1 … · N.K.t*`), then
   per topic: definition & purpose (real standard named), the underlying "why", formulae with every
   variable and unit defined, worked examples, pitfalls, an **"AI in this KA/domain"** treatment, a
   **key-terms box** (table: Term | Meaning), **sample MCQs** and **self-check questions**.
4. `## Advanced topics — Domain N` — practitioner-level extensions (numbered `N.A.1 …`).
5. `## Case study — Domain N` (+ optionally `Case study B`) — sector-anchored, applying the whole
   domain, with computed numbers.
6. `## Executive perspective — Domain N` — what a director cannot delegate.
7. `## Calculation exercises — Domain N` (quantitative domains) — multi-step problems with full
   solutions, numbered `Exercise N.n`.
8. `## Practitioner's toolkit — Domain N` — adoption-ready templates/checklists numbered `N.T.n`.
9. `## Exam preparation — Domain N` — known calculation traps and reflection questions.
10. `## Domain N summary`.

Sections that genuinely do not fit a domain are omitted rather than forced (the pattern is consistent,
not mechanical).

## 5. Worked-example format

Five-step skeleton, rendered as a labelled panel ("WORKED EXAMPLE", crimson left rule):

1. **Setup** — scenario + given data. 2. **Formula** — stated, variables and units named.
3. **Substitution** — numbers shown explicitly. 4. **Result** — rounded per convention, unit stated.
5. **Interpretation** — what the number means for the decision (never optional).

Inherently tabular computations (network passes, multi-scenario comparisons) may replace steps 1–4 with
a labelled table, but the Interpretation line remains. **Numbers must actually compute** — every example
is independently re-verified (see §14 of the programme charter and the formula registry).

## 6. Figures

- Numbered `Fig D.K.n`; each carries a caption, the underlying data (reproducibility), and a
  render-ready spec. Specs live in the source as blockquotes beginning `> **Fig D.K.n — Title.**`;
  the build injects the rendered SVG (`build/figures/fig_D_K_n.svg`) above the spec and **hides the
  spec** in print (`blockquote.figspec { display:none }`), so the reader sees figure + caption only.
- Style: clean, professional, brand blue `#1D4ED8`; labelled axes/series with sample values; no
  decorative clutter; accessible contrast; original artwork only ("PCI original").
- Digital-only **animation storyboards** may be specified where motion aids understanding, clearly
  marked digital-only; print uses the static figure from the same spec.

## 7. Tables, formula panels, callouts

- Tables: Inter 8.1 pt, brand-blue header row (white text), zebra striping, hairline rules, no vertical
  borders, `page-break-inside: avoid`.
- Formula/code panels: light panel with blue left rule; inline code (`JetBrains Mono`) for symbols.
- Blockquotes render as soft blue callout boxes — used for binding notes, principle statements,
  governance/ethics/AI callouts ("AI proposes; the professional verifies, decides and remains accountable"), and figure specs.

## 8. MCQ format

3–8 per KA, certification standard: exactly four options A–D; one correct, marked; a 1–3 sentence
rationale explaining the right answer **and** why each plausible distractor is wrong (distractors are
the results of common errors, never filler); each item tagged `[D.K.T · Recall/Application/Analysis]`;
numerical items wherever the topic is quantitative. Sample items are study material kept **separate from
live exam banks** — stated in the front matter and appendices.

## 9. Back matter (appendices pattern)

A — Master formula sheet · B — Global glossary (assembled from every key-terms box) · C — Standards &
frameworks referenced · D — Figure & animation index · E — Self-check answers · F — Sample-MCQ bank ·
G — Integrated capstone (one project across all domains) · **Alphabetical index** — generated from the
key-terms boxes, two columns, letter groups, page numbers resolved at layout.

## 10. Page architecture & typography (the `print.css` standard)

- **A4**; margins `21mm 17mm 19mm 17mm`; body **IBM Plex Serif 9.4 pt / 1.52**, justified, hyphenated;
  display/labels **Inter**; mono **JetBrains Mono**.
- Brand palette: blue `#1D4ED8`, ink `#0F172A`, crimson accent `#C13329`, slate greys; restrained use —
  colour marks structure, never decoration.
- Running headers: book title top-left (grey small caps), current chapter top-right (blue small caps),
  set via `string-set` from the chapter `h1`. Page number bottom-centre above a hairline. Title page,
  part pages, TOC and index are `page: plain` (no headers).
- Orphans/widows ≥ 2; apparatus panels avoid page breaks.

## 11. Language, numbers, citation rules

- **British English**; professional international register; active voice where appropriate.
- Currency: primary USD; parallel SAR where context helps (`USD 1 ≈ SAR 3.75`, indicative, stated at
  point of use). Money to whole units unless cents matter; ratios two decimals; percentages one decimal;
  thousands commas; adverse amounts in parentheses in statements.
- **Name real frameworks; never fabricate; never reproduce protected text.** Standards are described in
  the book's own words with the standard named inline. No invented citations, clause numbers or quotes.
  Honesty about AI capabilities and limits; the governing principle **"AI proposes; the professional verifies, decides and remains accountable"** (carried into the new books as *AI proposes; the professional verifies, decides and
  remains accountable* — see decision register D-11).

## 12. Secured student editions & distribution

The platform's document system is the distribution channel, and its contract is fixed
(**preserve exactly**, decision D-26):

- **File-name contract:** on boot, `MultiCert.EnsureBookFiles` (`backend/Data/MultiCert.cs`) resolves
  `backend/books/<code-lowercase>-bok.pdf` (`pml-ai-bok.pdf`, `pfl-ai-bok.pdf`) into content-addressed
  private storage and attaches it to each certification's `bok` document row (`watermark=1`,
  `published=1`). Admin-uploaded replacements are never clobbered; masters are never modified.
- **Student delivery** (`backend/Endpoints/Books.cs`): entitlement-gated download/inline view; each
  copy is watermarked per student (stable per-copy ID, diagonal name/ID/designation line, footer
  "Personal Copy – Not for Redistribution | Copy … | Downloaded …") and every access is audited.
- **Versioning:** admin uploads snapshot the outgoing file; full version history with view/restore.
- **Document title** rendered to students is `"PCI <designation> Body of Knowledge"` — the books'
  cover/title pages must agree with those catalogue names.
- The books carry **"First Edition"** (the PCI convention for framework content; policy documents use
  "Version 1.0") — the interim books carried no edition marker (corrected, D-28).

The new books publish through this channel unchanged: web-readable content from the markdown source;
the secured student PDF from the premium pipeline, regenerated whenever the manuscript changes (the
stale-PDF defect of the interim editions must be impossible: the build is committed and CI-checkable).

## 13. Accessibility treatment

The pattern requires: searchable/selectable text (the pipeline outputs real text, never rasterised
pages); logical heading hierarchy mirroring Part → Domain → KA → Topic; table header rows; figure
captions plus alt text carried in the figure registry; accessible contrast (ink/slate on white; white on
brand blue); Unicode fonts; bookmarks from the generated TOC. Gaps found in the v1 edition (no tagged-
PDF structure tree, alt text not embedded) are logged as **correct-before-reuse** items in the decision
register (D-16) — the new books must close them, not inherit them.

## 14. Source organisation & build (the go-forward pipeline)

```
docs/books/<designation>/          one folder per book (this programme's home)
  manuscript/00-style-spine.md     book-specific spine binding to the shared registries
  manuscript/domain-NN-slug.md     one file per domain (the PCL-AI convention)
  manuscript/appendices.md
  build/build_pdf.py + print.css   premium pipeline, adapted per book identity
  build/figures/fig_D_K_n.svg      rendered original figures
backend/books/<designation>-bok.md|pdf   published outputs served by the platform
docs/books/registries/             shared cross-book registries (terminology, formulas, sources, figures)
```

One stable chapter ID (`domain-NN-slug.md`) and one owner per file; no two agents edit the same chapter
file simultaneously; every domain binds to its spine and the shared registries.

---

## 15. Pattern-element disposition summary

Each element above is classified in the companion **decision register**
(`PATTERN_DECISION_REGISTER.md`) as *Preserve exactly* / *Preserve with minor improvement* / *Adapt for
the new certification* / *Correct before reuse* / *Do not reuse*, with reasons. Material departures from
this specification require a documented reason and chief-editor approval; where the pattern conflicts
with technical accuracy, accessibility, law or current PCI policy, those higher requirements win.
