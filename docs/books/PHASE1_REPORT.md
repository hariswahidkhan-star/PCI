# Phase 1 Report — Prototype Domains (PML-AI · PFL-AI Books Programme)

**Phase:** 1 of 8 · **Purpose:** prove the production model end to end on one representative
domain per book before scaling · **Verdict:** production model **proven**; prototypes are
**apparatus-complete at roughly 30–45 % of production depth** — the gate review (§3) mandates
specific expansions before either chapter is release-grade, and scaling may begin using this
model with the expanded depth targets.

## 1. What was built

| Artefact | Location |
|---|---|
| PFL-AI prototype — Domain 3, *Time value of money and financial mathematics* | `pfl-ai/manuscript/domain-03-time-value-of-money.md` (~6,560 words) |
| PML-AI prototype — Domain 6, *Planning, scheduling and delivery flow* | `pml-ai/manuscript/domain-06-planning-scheduling-flow.md` (~6,860 words) |
| Golden-answer verification harness (**99 checks, all passing**) | `_build/verify_formulas.py` |
| Figure generator + 5 original SVG figures | `_build/make_figures.py`, `p{ml,fl}-ai/build/figures/` |
| Premium build pipeline (pandoc-free: python-markdown → transforms → WeasyPrint) | `_build/build_book.py` + `_build/print.css` (family stylesheet, title parameterised) |
| Typeset prototype PDFs (A4, family design, "PHASE 1 PROTOTYPE" marked) | `pfl-ai/build/pfl-ai-prototype.pdf` (22 pp) · `pml-ai/build/pml-ai-prototype.pdf` (21 pp) |

Both prototypes carry the full family apparatus: binding blockquote · why-this-domain-exists +
learning objectives · a master worked project threading the chapter (Kestrel Water SPC;
Project Auriga) · KAs with topic lists, definitions, principles, five-step worked examples,
pitfalls, per-KA AI sections, key-terms boxes, tagged MCQs with full rationales, self-checks ·
advanced topics · two case studies each · executive perspective · five calculation exercises with
solutions and common errors · three-part practitioner's toolkit · exam preparation · domain
summary · numbered figure specs with rendered SVGs · cross-references by number throughout.

## 2. Calculation verification — the discipline worked

Every number printed in either prototype recomputes in `verify_formulas.py` (decimal arithmetic;
CPM passes re-derived from the network, not transcribed). The harness **caught one real authoring
error before commit**: the PFL-AI Case B refinancing instalment had been drafted as 6,079,672;
the correct value is **6,121,646** (`30,000,000 × 0.098/(1 − 1.098⁻⁷)`). The manuscript was
corrected and the suite now passes 99/99. This is the independent-verification rule doing its
job, and the harness pattern scales to every future chapter.

## 3. Gate review findings (chief editor)

**Pass:** pattern conformance (structure, apparatus, voice, conventions) · calculation
verification · originality (all scenarios fictional and original) · figure standard ·
cross-reference discipline · MCQ format · typesetting pipeline.

**Conditional — depth.** Page yield measured ≈ 21–22 typeset pages per prototype domain against
the 70–78-page production plan. Required expansions before these chapters are release-grade
(and the depth standard for all subsequent domains):
1. PFL-AI D3: add the level-principal and bullet schedule worked examples (only annuity is fully
   worked), annuity-due and deferred-annuity examples, day-count conventions topic, a second
   figure per KA, 6–8 MCQs per KA (currently 3), and the SAR parallel figures at the pattern's
   cadence.
2. PML-AI D6: add a worked lag/lead pass variant, a resource-levelling (hard-cap) worked example
   with the extension trade priced, an earned-schedule bridge stub to Domain 7, PDM edge cases
   (SS/FF float), 6–8 MCQs per KA, and a third figure (rolling-wave horizon diagram).
3. Both: expand advanced topics to the family's 3–5 subsection weight with at least one fully
   worked advanced example each.
Target after expansion: ≈ 14,000–16,000 words per quantitative domain ≈ 55–75 typeset pages with
full apparatus — consistent with the Phase 0 page architecture.

**Environment notes.** Brand fonts (IBM Plex Serif, Inter, JetBrains Mono) are not present in
this container, so the prototype PDFs render with metric-similar fallbacks — fine for structure
and yield measurement; the release build container must install the brand fonts. WeasyPrint 69
installed cleanly; pandoc is not needed by the new pipeline.

## 4. Standard phase reporting

1. **Sections completed:** 2 prototype domains (all 20 family sections each, where applicable).
2. **Word/page counts:** ~13,400 words → 43 typeset pages across the two prototypes.
3. **Competencies covered:** PML-AI competency 7 (planning and execution, primary); PFL-AI
   competencies 3/4 foundations (financial mathematics underpinning modelling and appraisal).
4. **Sources added:** none required (both domains are first-principles quantitative; frameworks
   referenced by family rules only).
5. **Calculations validated:** 99/99 golden checks; 1 authoring error caught and corrected (§2).
6. **Figures produced:** 5 original SVGs (3 PFL, 2 PML), all registered-format with alt text in
   specs.
7. **MCQs/exercises created:** 17 tagged MCQs with full rationales; 10 calculation exercises with
   solutions and common-error notes; 16 self-check questions.
8. **Quality gates:** pass with the depth condition (§3).
9. **Similarity/copyright:** all content original; no standard text reproduced; fictional
   entities only (Kestrel Water SPC, Project Auriga).
10. **Outstanding expert decisions:** OD-1…OD-3 unchanged from Phase 0; new **OD-4** — confirm
    the expanded per-domain depth target (≈ 15k words) as the production standard, which implies
    ≈ 240–260k words per book.
11. **Files changed:** `docs/books/` only (manuscripts, `_build/` toolchain, figures, prototype
    PDFs, this report).
12. **Next production batch (Phase 1 completion → Phase 2):** apply the §3 expansions to both
    prototypes and re-gate; then Phase 2 foundation domains (PML-AI D1–D4; PFL-AI D1–D2, D4)
    under the concurrent-agent model, one owner per domain file, every chapter entering the
    golden-answer harness and figure registry on submission. Recommend wiring
    `verify_formulas.py` into backend CI at that point so calculation regressions block merges.
