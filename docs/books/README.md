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
  [`_build/`](_build/): `verify_formulas.py` (**581 golden checks**, all passing — no domain
  passes gate while it fails), `make_figures.py` (27 PCI-original SVG masters),
  `build_book.py` + `print.css`.
- **Phase 2 — Foundation domains:** in progress under the production loop
  ([`PHASE2_REPORT.md`](PHASE2_REPORT.md)). **11 domains delivered, 224 typeset pages:**
  PML-AI D1, D2, D3, D6, D7, D8 (129 pp) · PFL-AI D1, D2, D3, D4, D10 (95 pp). PFL-AI Part One
  is complete; PML-AI Part One needs only D4. The report's *next production batch* section is
  the loop's work queue.
- Phases 3–8 proceed per the charter §8; no phase is skipped to meet page count.

## Relationship to the rest of the repository

- `docs/bok/` — the approved PCL-AI (né PCP-AI) book (pattern source; untouched by this programme).
- `backend/books/` — the platform-served book editions (`<designation>-bok.md` + `.pdf`); this
  programme publishes into that channel at release phases only.
