# Pattern-Quality Decision Register — PML-AI · PFL-AI

**Status:** Phase 0 deliverable. Every element of the previous PCI book's pattern — and of the two
existing interim PML/PFL manuscripts found in `backend/books/` — is classified here as **Preserve
exactly** / **Preserve with minor improvement** / **Adapt** / **Correct before reuse** / **Do not
reuse**, with reasons. Material departures from the approved pattern require a documented reason here
and chief-editor approval; technical accuracy, accessibility, law and current PCI policy outrank the
pattern where they conflict.

## A. Rulings on the PCL-AI (né PCP-AI) pattern — the approved family standard

*(The previous book's source lives under its pre-rename designation PCP-AI in `docs/bok/`; the live
credential and published framework pages now name it PCL-AI. Pattern rulings are unaffected by the
rename.)*

| ID | Element (evidence) | Ruling | Reason / action |
|---|---|---|---|
| D-01 | Domain → KA → Topic numbering, cross-reference-by-number (`00-style-spine.md` section 2) | **Preserve exactly** | The family's navigation and blueprint-mapping backbone |
| D-02 | Domain chapter shape (why-it-exists → KAs → advanced → cases → executive perspective → exercises → toolkit → exam prep → summary) | **Preserve exactly** | Proven across 13 PCP-AI domains; the suite's recognisable rhythm |
| D-03 | Worked-example five-step skeleton + labelled panel | **Preserve exactly** | Core pedagogy; numbers independently re-verified |
| D-04 | Key-terms boxes, self-checks, MCQ format (4 options, marked answer, rationale, `[topic · level]` tag) | **Preserve exactly** | Feeds glossary/index generation and blueprint mapping |
| D-05 | Master worked project per quantitative domain; Appendix G station-based capstone | **Preserve with minor improvement** | Extend to *four* capstones per book (brief requirement) using the same station pattern |
| D-06 | Figure regime: numbered specs with underlying data, SVG masters injected at build, spec hidden in print | **Preserve with minor improvement** | Add registered alt text (registry + tagged PDF emission) — v1 carried none |
| D-07 | Premium typesetting (`build/print.css`): A4, IBM Plex Serif/Inter, part dividers, chapter/KA openers, running headers, generated TOC + index | **Preserve exactly** | The family's visual identity; restrained, professional |
| D-08 | Front-matter suite (title page, copyright/edition + disclaimers + trademark + originality notices, how-to-use) from `build_pdf.py` | **Adapt** | Same architecture; new identities, weightings and part maps; PFL-AI adds extended finance disclaimers (D-14) |
| D-09 | Style Spine as per-book binding document + shared registries | **Preserve with minor improvement** | Registries elevate to programme level (`docs/books/registries/`) so both books + PCP-AI stay consistent |
| D-10 | British English; USD primary + indicative SAR; rounding/thousands conventions | **Preserve exactly** | House conventions; violated by the interim manuscripts (D-24) |
| D-11 | Governing principle wording "AI proposes, the professional disposes" | **Adapt** | The new books carry the suite evolution: **"AI proposes; the professional verifies, decides and remains accountable"** — same doctrine, sharper accountability language; PCP-AI text unchanged |
| D-12 | 40/40/20 weighting language | **Adapt** | Each new book states its own blueprint weighting from its competency map; the split is per-certification |
| D-13 | Citation rules (name real frameworks; own words; no invented citations) | **Preserve exactly** | Legal safety + originality; now backed by `registries/SOURCES.md` with rights status |
| D-14 | Educational disclaimer paragraph | **Adapt** | PFL-AI additionally carries finance/accounting/tax/legal/investment disclaimers and jurisdiction caveats (charter section 9) |
| D-15 | Two-generation source history (per-KA `_build/` vs whole-domain `build/`) | **Correct before reuse** | One pipeline only for the new books: the whole-domain + WeasyPrint premium pipeline; `_build/` is historical |
| D-16 | Accessibility state of v1 PDF (real text, bookmarks, contrast — but no tagged structure tree, no embedded alt text) | **Correct before reuse** | New books must produce tagged, WCAG-oriented PDFs with alt text and table headers; do not inherit the gap |
| D-17 | Trademark symbols: none in PCI branding; third-party marks acknowledged once on the copyright page | **Preserve exactly** | Matches current PCI policy; the ® symbols found once in the interim PML text are removed by D-25 |

## B. Rulings on the interim `backend/books/` PML/PFL manuscripts (found on main)

The audit (see `PHASE0_REPORT.md` section 2) found both manuscripts are machine-authored interim editions that
do **not** follow the family pattern: flat 24/19-chapter lists with no Part/Domain/KA structure,
near-uniform ~2,700-word chapters (a generation quota, not content-driven depth), no figures, no
key-terms boxes, no self-checks, no numbered cross-references, no appendices, two incompatible MCQ
formats, four chapter-heading forms, currency-convention violations (GBP-majority in PML), a stray
second H1 at each glossary, and a **stale published PML PDF** predating its own merge-conflict fix.

| ID | Element | Ruling | Reason / action |
|---|---|---|---|
| D-20 | The interim manuscripts as a *pattern* source | **Do not reuse** | They are the "renamed generic guide" failure mode the programme brief prohibits; the pattern source is PCP-AI |
| D-21 | The interim manuscripts as *content* | **Do not reuse** (reference only) | Chapter lists and glossaries may inform chapter briefs and terminology candidates; no prose is carried forward — the new books are written fresh against the pattern spec |
| D-22 | Credential naming | **Correct before reuse** (one open decision) | The platform catalogue (`MultiCert.cs`) is the naming system of record: **PML-AI — "PCI Project Management Leader – AI"** (platform and programme brief agree; PDL-AI residue purged). **PFL-AI**: the platform says **"PCI AI Project Finance Leader"** while the programme brief says "PCI Project Finance Leader – AI" — logged as outstanding expert decision **OD-1**; until PCI rules, the books follow the live catalogue name. No new content may use retired names (PCP-AI, PDL-AI, CPMD, PFIP) |
| D-23 | The stale, unreproducible published PDFs (PML PDF older than its markdown; PFL rendered with fallback fonts; no build script exists) | **Correct before reuse** | The new books ship from a committed, reproducible build (`docs/books/<book>/build/`) publishing to `backend/books/<designation>-bok.pdf`; interim PDFs remain live only until Phase 8 replacement |
| D-24 | Interim currency usage (GBP-majority, zero SAR) | **Correct before reuse** | House convention (D-10) applies to all new content |
| D-25 | One-off `PMBOK®`/`PRINCE2®` symbols (`pml-ai-bok.md:1438`) with no trademark notice | **Correct before reuse** | No ™/® in running text (D-17); acknowledgement lives on the copyright page |
| D-26 | Their serving/watermarking channel (`MultiCert.EnsureBookFiles` → content-addressed storage → per-copy watermark → audited downloads → version history/restore) | **Preserve exactly** | Platform machinery is sound and tested; the new books publish through it unchanged (file-name contract: `backend/books/<code-lowercase>-bok.pdf`) |
| D-27 | Their integrated capstones ("Halcyon Operations Centre", "Meridian Water Concession") | **Do not reuse** | New books build four original capstones each on the Appendix G station pattern; titles retired to avoid version confusion |
| D-28 | Interim books carry no edition/version marker | **Correct before reuse** | PCI convention: framework content is **"First Edition"** (policy documents are "Version 1.0"); both new books carry First Edition on the title and copyright pages |
| D-29 | Interim books' "Examination overview" defers item counts/duration/pass mark to the live examination specification | **Preserve exactly** | Correct behaviour: no PML/PFL exam blueprint exists yet (item counts pending job-task analysis; pass mark pending modified-Angoff study); books must never hard-code parameters that are admin-configurable per certification |
| D-30 | Platform disclaimers suite (no-guarantee/no-accreditation clause; educational "not advice … illustrative rather than templates" clause; policy-precedence clause; vendor-neutrality clause) | **Preserve exactly** | The books' front matter mirrors all four blocks verbatim in substance (`backend/Data/PublicDocsSeed.cs`, `website-disclaimer.html`, candidate handbook precedence formula), plus PFL-AI's extended finance disclaimers (D-14) |

## C. Standing rules

1. Any element not listed here follows the pattern spec by default.
2. New departures discovered during drafting are appended with an ID, ruling and reason before the
   affected chapter passes gate.
3. Corrections of fact (calculation errors, unsupported citations, broken references, accessibility
   defects) never require a register entry to fix — they are gate failures by definition.
