# Corpus Gate Report — Phases 2, 3 and 4

**Scope:** the complete domain corpus of both volumes — PML-AI Domains 1–16 and PFL-AI Domains 1–16.
**Status:** **GATE PASSED for the domain corpus.** Phases 5–8 have not started; §10 below is the part
of this report that matters most and should be read before the numbers above it.

Phases 2, 3 and 4 are reported together because they were delivered together. The charter (§8)
sequences them as foundations → core technical batches → leadership/governance/AI, and the scaled
authorship run produced all three in one pass. Writing three separate gate reports for one run would
be a fiction; this is the standard report, once, over the whole corpus.

---

## 1. Sections completed

| | Domains | Knowledge Areas | Worked examples | Case studies | Toolkits | MCQs | Exercises |
|---|---|---|---|---|---|---|---|
| **PML-AI** | 16 | 63 | 96 | 32 | 48 | 289 | 74 |
| **PFL-AI** | 16 | 61 | 129 | 32 | 48 | 241 | 76 |
| **Total** | **32** | **124** | **225** | **64** | **96** | **530** | **150** |

Every domain carries the full family apparatus: opening blockquote, why-it-exists, learning
objectives, master-thread paragraph, per-KA topics with bold-lead definitions, five-step worked
examples, an *AI in this KA* treatment, a key-terms table, sample MCQs and a self-check; then advanced
topics closing on a reviewer-invariant list, industry variations, two case studies, an executive
perspective, calculation exercises with common-error notes, three toolkits, exam preparation and a
summary. 63 of 63 and 61 of 61 Knowledge Areas have their key-terms table.

## 2. Word and page estimates

| | Words | Typeset pages | Figures | Index entries |
|---|---|---|---|---|
| **PML-AI** | 252,180 | **503** | 33 | 479 |
| **PFL-AI** | 242,943 | **494** | 36 | 397 |
| **Total** | **495,123** | **997** | **69** | **876** |

Pages are measured, not estimated — WeasyPrint output from `_build/build_book.py` at the family's A4
premium settings.

**Against the 1,200-page target per volume, both volumes are short**, and this report does not dress
that up. The gap is roughly 700 pages of *legitimate* content that Phase 5 is supposed to produce and
has not yet: the consolidated question banks, the glossaries, the appendices (formula sheets,
notation tables, worked-solution walkthroughs), the capstone case programme the TOCs specify (four
capstones per volume), and the front and back matter. **The gap must be closed with that content, not
with typographic inflation, duplicated text or padded appendices** — which the charter forbids and
which would defeat the purpose of the target.

## 3. Competencies covered

Both competency maps (`pml-ai/COMPETENCY_MAP.md`, `pfl-ai/COMPETENCY_MAP.md`) map to domains that now
exist. The conformance matrices should be re-walked against the delivered text in Phase 6 — mapping to
a *planned* domain and mapping to a *written* one are different claims, and only the first has been
made so far.

## 4. Calculations validated

**7,176 golden-answer checks, all passing**, across 21 modules:

| Source | Checks |
|---|---|
| `_build/verify_formulas.py` (the first 12 domains, authored serially) | 581 |
| `_build/checks/pml_*.py` (9 modules) | 2,228 |
| `_build/checks/pfl_*.py` (11 modules) | 3,283 |
| loader self-test (pins the ctx contract itself) | 4 |

Every number printed as a *result* is recomputed with `decimal.Decimal` at 28-digit precision and
compared to the printed value: worked-example Result and Substitution lines, in-text figures, **every
numeric MCQ option and not only the correct one**, exercise answers, case-study figures, and numbers
inside figure specifications. Each module additionally pins the domain's teaching **invariants** — an
identity, a breakeven, a bound, an inequality — so a claim cannot silently stop being true.

Run the gate with `python3 _build/verify_formulas.py`; run one domain in isolation with
`python3 _build/run_checks.py checks/<module>.py`. Both loaders are fail-closed, and that is proven by
test rather than assumed: a module that raises exits non-zero through either path.

**Defects the harness caught during authoring** (a sample, from the domains where it mattered most):
a growth-rate drift in PFL-AI D6 that put its year-12 sponsor DSCR at 1.5945 where the domain's own
solved *g* gives 1.5940, and ran through nine years of a figure series; an equity IRR printed 13.53 %
against a solved 13.5215 %; a maximum-debt figure in PFL-AI D10 drifted by 2,736 with two values
downstream of it; an MCQ distractor of 400,605 for a stated error yielding 404,605; a truncated
capacity figure left mid-sentence; a `CPI`-to-overrun conversion stated as 13 % where 0.87 implies
14.9 %. One verifier established that its passes were derivations rather than transcriptions by
perturbing five printed values and confirming the checks then fail.

## 5. Figures and tables

69 figures, all **PCI-original**, generated deterministically from source in `_build/figures_src/`
(20 per-domain modules) and `_build/make_figures.py`. No third-party diagram is reproduced or adapted.
Every figure has an alt-text description in its manuscript specification. Key-terms tables: 124.

## 6. MCQs and exercises

530 MCQs, each tagged with its topic and cognitive level; 150 calculation exercises, each with a full
solution and a **common-error** note. Every numeric MCQ option is independently derived in the check
modules.

**MCQ integrity audit — 25 defects found and fixed**, including **two release blockers** where a
question had *two* defensible answers: PML-AI 5.2-C, and PML-AI 16.1-A, whose stem never stated the
conditions were independent, making the perfectly-correlated bound of 80.00 % arguable against the
marked 49.79 %. Both stems were fixed. Further defects: distractors with no named error, two
rationales that mis-derived their own numbers, and stems missing a datum needed to answer.

## 7. Gate results

| Gate | Result |
|---|---|
| Golden-answer suite passes in full | **PASS** — 7,176/7,176 |
| Every domain has a check module | **PASS** — 20 modules + the 12 domains in `verify_formulas.py` |
| Figures render deterministically, PCI-original | **PASS** — 69 figures, 20 modules, fail-closed loader |
| Both volumes typeset without error | **PASS** — 503 pp and 494 pp |
| Family pattern conformance | **PASS** — after normalising 21 summary headings and one toolkit series |
| Master-thread numeric continuity | **PASS** — swept; four near-misses investigated individually and all legitimate |
| MCQ integrity | **PASS** — after 25 fixes, including 2 blockers |
| IP and legal safety | **PASS** — after 32 reviewed items, several fixed (see §8) |
| Independent human review | **NOT STARTED** — Phase 6 |
| Phase 5 companions | **NOT STARTED** |

## 8. Similarity and legal findings

Swept across all 32 manuscripts and separately audited per domain by a dedicated reviewer:

- **No trademark, registered or copyright symbols** anywhere in either corpus.
- **One reference to a third-party body**: AACE International's Total Cost Management class
  progression, cited by name in PML-AI D7 with the text stating explicitly that it is *described here
  in this book's own words*. Public standards are cited by number only — ISO 19650, ISO 8000,
  ISO 9000/9001, ISO/IEC 42001, ISO/IEC 23894, IFRS 15, IFRS 16, IAS 37 — with no content reproduced
  and endorsement expressly disclaimed where frameworks are discussed.
- **Zero instances of invented-evidence phrasing** across twelve searched patterns ("studies show",
  "research demonstrates", and similar).

**Fixed rather than waved through** — the findings that mattered:

1. An **Equator Principles gloss** closely tracked the framework's own self-description. Rewritten.
2. An **invented quotation attributed to project minutes** in a PFL-AI D8 case study. Removed — an
   invented quotation attributed to a document is a release risk even in illustrative material.
3. **Jurisdiction-specific accounting stated as universal** ("the amount enters the depreciable
   base"). Now framework-dependent and a matter for the sponsor's auditors.
4. **Unsourced market statistics**: an EPC-wrap premium "typically several percentage points of
   capital cost", a 40 % fatal-flaw rate introduced as external evidence, two prevalence claims, and
   a reconstruct-to-record ratio asserted as "stable across contexts". Replaced with the fictional
   entity's own records, or made qualitative.
5. An **estimate-class ladder** reworded so it cannot read as a published classification framework.
6. **Counsel pointers added** where enforceability-sensitive mechanics — liquidated damages,
   termination compensation, full-and-final settlement, punitive dilution conversion — had been
   presented as management discipline.
7. Two **absolute claims about the world** ("which no shareholders' agreement contains") softened to
   claims about typical range.

## 9. Open decisions for a human expert

1. **The page-target strategy.** 997 against 2,400. §2 states the honest gap and the only acceptable
   way to close it. Someone with authority must confirm that Phase 5's companion content is the
   intended route and that the target is not to be met by inflation.
2. **Exam-blueprint weightings** (carried from Phase 0, OD-2) — the domain corpus assumes the TOC
   structure; the assessment weightings are not settled.
3. **PFL-AI designation wording** (Phase 0, OD-1) — "AI Project Finance Leader" vs "Project Finance
   Leader – AI" is still inconsistent across platform artefacts.
4. **Whether the four capstone cases per volume** specified in the TOCs are in scope for this edition.
5. **Islamic-finance treatment** in PFL-AI D9 is described in economic terms only, with no
   jurisdictional or religious ruling implied. A qualified reviewer should confirm the framing.

## 10. The claim this report does **not** make

**The corpus has had no human editorial or technical review.** It was AI-drafted end to end. Every
volume's front matter says so, is attributed to no named author or expert, and makes no claim of human
authorship.

A passing verification suite establishes something real but narrow: **the arithmetic is right, and the
numbers in the book are the numbers the methods produce.** It does not establish that the pedagogy is
sound, that the professional judgements are ones an experienced practitioner would endorse, that the
emphasis is right, that nothing important is missing, or that the prose is publishable. Those are
Phase 6 and Phase 7 questions and they are untouched.

Nothing here should be presented to a candidate, a regulator, an accreditation body or a customer as
reviewed material. The page count is not a proxy for readiness, and this report should not be quoted
without §10.

## 11. Files changed

`pml-ai/manuscript/` (16 domains) · `pfl-ai/manuscript/` (16 domains) ·
`_build/checks/` (20 modules + loader self-test + contract README) ·
`_build/figures_src/` (20 modules + contract README) ·
`_build/verify_formulas.py`, `run_checks.py`, `make_figures.py`, `build_book.py`, `print.css` ·
`registries/FORMULAS.md` · `PHASE2_REPORT.md` · `README.md` · this report.

## 12. Next batch

**Phase 5 — cases, exercises and companions.** In priority order, because each is a real gap rather
than filler: consolidated question banks per volume (drawn from and cross-referenced to the 530
in-domain MCQs, with fresh items to blueprint weighting); the glossaries (built from the 124 key-terms
tables, deduplicated, with one canonical definition per term); the appendices (formula sheet from
`registries/FORMULAS.md`, notation table, worked-solution walkthroughs for the hardest exercises); the
capstone cases the TOCs specify; and the front and back matter. Then **Phase 6 independent review**,
which is the gate that actually matters, and which no amount of further authorship can substitute for.
