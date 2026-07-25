# Phase 0 Report — Governance and Blueprint (PML-AI · PFL-AI Books Programme)

**Phase:** 0 of 8 · **Gate condition:** demonstrate how both proposed books follow the established PCI
book pattern · **Verdict:** gate condition met — see §3; three outstanding expert decisions logged (§10).

---

## 1. What Phase 0 audited (evidence base)

### 1.1 The previously written PCI book — located and inspected in full

- **Latest approved version identified:** the PCL-AI Body of Knowledge, First Edition — published PDF
  `docs/bok/PCP-AI-Body-of-Knowledge-v1.pdf` (~950–970 A4 pages, produced by the `_build/` Chromium
  pipeline), with the **current maintained source** being the whole-domain corpus
  (`docs/bok/domain-01…13.md` + `appendices.md`, ~207,000 words) bound to the Style Spine
  (`00-style-spine.md`) and typeset by the premium WeasyPrint pipeline (`docs/bok/build/`).
- **Drafts/obsolete copies distinguished:** the first-generation per-KA corpus (61 files under
  `docs/bok/<NN-slug>/`) and `_build/` pipeline are historical; ruling D-15 designates the
  whole-domain + `build/` generation as the pattern source.
- **Complete-book inspection:** front matter construction (`build_pdf.py` TITLE_HTML), part/chapter/KA
  opener regime, the domain chapter shape across all 13 domains, worked-example/figure/MCQ/key-terms/
  self-check apparatus, appendices A–G, generated TOC and index, and the full `print.css` page
  architecture were read and extracted into the **Pattern Specification**.
- **Credential rename verified:** PCP-AI → PCL-AI ("PCI AI Project Controls Leader") via the Master
  Naming Update (`MultiCert.cs` MigrateCert/NameSweep; site swept; `docs/` sources retain the old name).

### 1.2 The interim PML/PFL manuscripts found on main (`backend/books/`)

Machine-authored interim editions (~68k words / 24 chapters PML; ~56k words / 19 chapters PFL),
**not** authored to the family pattern: no Part/Domain/KA/Topic structure, near-uniform ~2,700-word
chapters (quota generation), no figures, key-terms boxes, self-checks, numbered cross-references or
appendices; two incompatible MCQ formats; four chapter-heading forms; broken heading hierarchy at both
glossaries; currency convention violated (GBP-majority PML, zero SAR); a one-off `®` usage against
platform trademark policy; and a **stale published PML PDF** rendered before its own merge-conflict
fix, with no committed build script able to reproduce either PDF. Full defect list: decision register
§B (D-20…D-27). **Ruling:** pattern source *and* content — do not reuse; the platform serving channel
(watermarking, versioning, audit) — preserve exactly (D-26). The interim PDFs stay live until Phase 8
replacement.

### 1.3 Certification blueprints, policies, terminology

- **No PML-AI/PFL-AI examination blueprint exists** anywhere in the platform (no weightings, item
  counts or per-cert pass marks; `CertPage.cs` promises them "during application"; the demo exam bank
  merely clones PCL-AI items). The only competency-adjacent content is the catalogue's published
  competency lists (24 PML / 19 PFL items, `MultiCert.cs`).
- **Naming system of record:** `MultiCert.cs` — PCL-AI / PFL-AI / PML-AI, suite name "PCI AI Project
  Leadership Certification Suite", portfolio tagline "Finance intelligently. Control predictively.
  Deliver successfully." (order maps Finance = PFL, Control = PCL, Deliver = PML).
- **Trademark policy:** zero ™/® across 218 pages and all docs; `TrademarkStrip` enforces on boot.
- **Disclaimer canon:** four standing blocks (no-guarantee/no-accreditation; educational not-advice;
  policy precedence; vendor neutrality) + the AI doctrine; PCI convention "First Edition" for
  framework content.
- **Exam parameters** (90 min / 65 % / USD 500 list / 3-yr validity / 12-month window) are
  admin-overridable defaults — books cite the live examination specification instead (D-29).

## 2. Phase 0 deliverables produced

| Deliverable | File |
|---|---|
| PCI Book Pattern Specification | `PCI_BOOK_PATTERN_SPEC.md` |
| Pattern-quality decision register (preserve/adapt/correct/do-not-reuse) | `PATTERN_DECISION_REGISTER.md` |
| Editorial charter | `EDITORIAL_CHARTER.md` |
| Shared registries (terminology · formulas · sources · figures) | `registries/` |
| PML-AI competency map + detailed TOC + conformance matrix | `pml-ai/` |
| PFL-AI competency map + detailed TOC + conformance matrix | `pfl-ai/` |
| Programme home / phase state | `README.md` |

## 3. Gate demonstration — how both books follow the established pattern

Both books adopt, unchanged: the Domain → KA → Topic numbering and cross-reference-by-number regime;
the 13-domain book's proven chapter shape (why-it-exists + objectives → KAs with worked examples,
key-terms, tagged MCQs, self-checks → advanced topics → sector cases → executive perspective →
calculation exercises → toolkit → exam preparation → summary); the five-step worked-example panel; the
figure-spec/SVG-injection regime; the A4 premium typesetting system (`print.css` verbatim: fonts,
palette, part/chapter/KA openers, running headers, generated TOC and index); the appendices suite; the
front-matter architecture with the platform's disclaimer canon; British-English/USD(+SAR) conventions;
and the platform's watermarked distribution contract. Book-specific adaptations (identities, part
maps, PFL-AI finance disclaimers, four capstones each, master model thread) and corrections
(accessibility tagging, edition markers, reproducible builds) are each logged and ruled in the decision
register — none is an undocumented departure. The conformance matrices bind every element to its ruling
and will be re-scored at each phase gate.

## 4. Standard phase reporting

1. **Sections completed:** all Phase 0 governance sections (no manuscript chapters — correct for this
   phase; drafting is gated behind prototype approval).
2. **Word/page estimates:** planned typeset totals — PML-AI ≈ 1,300–1,420 pp; PFL-AI ≈ 1,300–1,450 pp;
   working estimate ≈ 240,000–280,000 words per book (PCL-AI First Edition ratio: ~207k words →
   ~950–970 pp, then scaled by the deeper apparatus and appendices).
3. **Competencies covered:** 24/24 PML-AI and 19/19 PFL-AI published competencies mapped to domains
   (competency maps §1); examination weightings proposed as indicative, pending OD-2.
4. **Sources added:** 10 seed rows in `registries/SOURCES.md` with rights posture; prohibited-use
   register established.
5. **Calculations validated:** none due this phase; formula registry seeded (10 inherited symbol
   groups verified in PCL-AI; 5 PML + 14 PFL new entries registered, verification scheduled with the
   Phase 1 golden-answer harness).
6. **Figures/tables produced:** none due; figure registry and standards established.
7. **MCQs/exercises created:** none due; formats fixed (spine §8 + two-reviewer rule).
8. **Quality-gate results:** Phase 0 gate met (§3). No chapter gates run yet.
9. **Similarity/copyright findings:** interim manuscripts contain no wrong-certification leakage and
   no conflict markers (verified); one trademark-symbol violation logged (D-25). No third-party text
   reproduction found in pattern sources.
10. **Outstanding expert decisions:**
    - **OD-1 — PFL-AI official name:** platform "PCI AI Project Finance Leader" vs brief "PCI Project
      Finance Leader – AI". Books follow the platform catalogue until PCI rules; a rename would be an
      id-stable `MigrateCert` in the platform first.
    - **OD-2 — Examination weightings:** the indicative part weightings in both competency maps
      require PCI approval (and ultimately a job-task analysis) before any examination use.
    - **OD-3 — Suite catalogue copy:** `certifications.html` still describes the third credential as
      "Project Delivery" (retired PDL framing) and `certification.html`'s badge mock-up carries the
      retired PCL full name; platform fixes outside this programme's scope — flagged to PCI.
11. **Files changed:** everything under `docs/books/` (new); no platform code, no `docs/bok/`, no
    `backend/books/` changes.
12. **Next production batch (Phase 1 — prototypes):** one representative domain per book, full
    apparatus, typeset through the adapted premium pipeline, then reviewed against every gate before
    scaling: **PML-AI Domain 6 — Planning, scheduling and delivery flow** and **PFL-AI Domain 3 —
    Time value of money and financial mathematics** (both quantitative flagships, exercising worked
    examples, figures, golden-answer verification, MCQs, toolkits and typesetting end to end).

## 5. Production-environment notes (for Phase 1)

- WeasyPrint installs cleanly in the CI/build container (verified v69.0); `pandoc` is absent — the
  Phase 1 build adaptation will render Markdown → HTML with the Python `markdown` package (present) or
  vendor a pandoc binary; Chromium (`/opt/pw-browsers`) remains a fallback renderer.
- The PCL-AI cover asset pattern (`build/cover.jpg`) needs two new original covers (illustration
  brief in Phase 1).
- A `pcl-ai-bok.pdf` gap exists on the platform (the PCL-AI book was never compiled into
  `backend/books/`, so its document row has no file) — out of scope here, but the new build pipeline
  could close it later at low cost.
