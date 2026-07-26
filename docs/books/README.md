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
- **Phase 2 — Foundation domains:** complete, and scaled authorship has run past it
  ([`PHASE2_REPORT.md`](PHASE2_REPORT.md)). **All 32 domains are drafted — 992 typeset pages:**
  PML-AI 16 domains / 502 pp / 33 figures · PFL-AI 16 domains / 490 pp / 36 figures. The
  golden-answer suite stands at **4,675 checks, all passing**.
- **Known gap, not glossed:** six domains (PML-AI D5, D16; PFL-AI D5, D6, D8, D16) lost their
  verification stage when the authoring run was interrupted, so their arithmetic is unchecked and
  **they do not pass gate**. Verification is in progress; nothing in those six should be relied on
  until it lands.
- **Human review is outstanding and is not optional.** The charter requires editorial and technical
  review before release. 992 pages of AI-drafted material have had neither. A passing verification
  suite establishes that the arithmetic is right — a different and much narrower claim than the book
  being correct, well-judged and publishable.
- Phases 3–8 proceed per the charter §8; no phase is skipped to meet page count.

## Relationship to the rest of the repository

- `docs/bok/` — the approved PCL-AI (né PCP-AI) book (pattern source; untouched by this programme).
- `backend/books/` — the platform-served book editions (`<designation>-bok.md` + `.pdf`); this
  programme publishes into that channel at release phases only.
