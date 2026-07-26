# PCI Books Programme — PML-AI & PFL-AI Bodies of Knowledge

The production home for two publication-ready professional books:

1. **PCI Project Management Leader – AI Body of Knowledge** (PML-AI)
2. **PCI Project Finance Leader – AI Body of Knowledge** (PFL-AI)

Each ≥ 1,200 genuinely useful typeset pages, following the validated pattern of the approved
**PCL-AI Body of Knowledge** (stored under its pre-rename designation PCP-AI in `docs/bok/`), under
phase-gated production. The previously written book is the family pattern; these books adapt it to
their subject matter without inheriting its defects.

## Governance documents (Phase 0)

| Document | Purpose |
|---|---|
| [`PCI_BOOK_PATTERN_SPEC.md`](PCI_BOOK_PATTERN_SPEC.md) | The extracted, binding publishing pattern of the PCI book family |
| [`PATTERN_DECISION_REGISTER.md`](PATTERN_DECISION_REGISTER.md) | Preserve / improve / adapt / correct / do-not-reuse ruling on every pattern element |
| [`EDITORIAL_CHARTER.md`](EDITORIAL_CHARTER.md) | Mission, quality rules, voice, agent-production rules, gates, definition of done |
| [`registries/`](registries/) | Shared single-source registries: terminology, formulas, sources, figures |
| [`pml-ai/`](pml-ai/) · [`pfl-ai/`](pfl-ai/) | Per-book: competency map, chapter-level TOC, conformance matrix, then manuscript + build |
| [`PHASE0_REPORT.md`](PHASE0_REPORT.md) | Phase 0 gate report (audit findings, decisions, next batch) |

## Phase state

- **Phase 0 — Governance and blueprint:** complete ([`PHASE0_REPORT.md`](PHASE0_REPORT.md)).
- **Phase 1 — Prototype domains:** production model proven; prototypes apparatus-complete with a
  depth-expansion condition ([`PHASE1_REPORT.md`](PHASE1_REPORT.md)). Toolchain in
  [`_build/`](_build/): `verify_formulas.py` (**4,675 golden checks**, all passing — no domain
  passes gate while it fails), `make_figures.py` (69 PCI-original SVG masters, 20 of them from per-domain modules),
  `build_book.py` + `print.css`.
- **Phases 2, 3 and 4 — the domain corpus:** **complete and gate-passed**
  ([`CORPUS_GATE_REPORT.md`](CORPUS_GATE_REPORT.md)). All 32 domains drafted:

  | | Domains | Typeset | Words | Figures | Worked examples | MCQs | Exercises |
  |---|---|---|---|---|---|---|---|
  | PML-AI | 16 | 503 pp | 252,180 | 33 | 96 | 289 | 74 |
  | PFL-AI | 16 | 494 pp | 242,943 | 36 | 129 | 241 | 76 |

  **7,176 golden-answer checks, all passing** across 21 modules. Every printed result is recomputed
  with decimal arithmetic — including every numeric MCQ option, not only the correct one.
- **Phase 5 — cases, exercises and companions:** **not started.** This is where the remaining ~700
  pages per volume legitimately come from: consolidated question banks, glossaries, appendices,
  capstone cases, front and back matter. Both volumes are short of the 1,200-page target and the gap
  must be closed with that content, never with typographic inflation, duplication or padded
  appendices.
- **Phase 6 independent review · Phase 7 copy edit and typesetting · Phase 8 pilot and release:**
  not started.
- **Human review is outstanding and is not optional.** The corpus was AI-drafted end to end and has
  had no editorial or technical review. A passing verification suite establishes that the arithmetic
  is right and that the numbers are the ones the methods produce. It does **not** establish that the
  pedagogy is sound, the judgements are ones an experienced practitioner would endorse, the emphasis
  is right, or that nothing important is missing. Nothing here should be presented to a candidate,
  regulator, accreditation body or customer as reviewed material.

## Relationship to the rest of the repository

- `docs/bok/` — the approved PCL-AI (né PCP-AI) book (pattern source; untouched by this programme).
- `backend/books/` — the platform-served book editions (`<designation>-bok.md` + `.pdf`); this
  programme publishes into that channel at release phases only.
