# PML-AI — PCI Book Pattern Conformance Matrix (Phase 0 baseline)

Compares the planned PML-AI BoK against the approved previous PCI book (PCL-AI BoK, First Edition —
source `docs/bok/`). Status: **Planned** = committed in Phase 0 design; matrix is re-scored at every
phase gate until all rows are **Conforms**. Rulings cited are from `../PATTERN_DECISION_REGISTER.md`.

| Pattern element | Previous book (evidence) | PML-AI treatment | Ruling | Status |
|---|---|---|---|---|
| Front cover | Full-bleed A4 `build/cover.jpg` | Same construction, PML-AI identity | D-08 | Planned |
| Title-page hierarchy | Designation + subtitle + rule + edition/publisher/principle | Same; "First Edition" | D-08/D-28 | Planned |
| Copyright & disclaimers | Injected notice block (`build_pdf.py:47-67`) | Same architecture + platform disclaimer suite | D-30 | Planned |
| Foreword/preface/how-to-use | "How to use this reference" page | Same, PML-AI reader modes | D-08 | Planned |
| Part opening format | Ghost number, kicker, title, description, bar | Identical CSS, 4 parts | D-07 | Planned |
| Chapter opening format | Ghost chapnum, kicker, rules, binding blockquote | Identical, 16 domains | D-02 | Planned |
| Learning objectives | "After this domain a candidate can…" | Identical placement | D-02 | Planned |
| Terminology & definitions | Key-terms boxes → shared registry → glossary | Bound to `registries/TERMINOLOGY.md` | D-09 | Planned |
| Heading levels | `#`/`##`/`###` = Domain/KA-section/Topic | Identical numbering `D.K.T` | D-01 | Planned |
| Paragraph & narrative style | British English, justified serif, spine conventions | Identical | D-10 | Planned |
| Worked-example structure | Five-step panel | Identical | D-03 | Planned |
| Calculation presentation | Formula panels, verified numbers, rounding rules | + golden-answer tests (formula registry) | D-03 | Planned |
| Case-study structure | Sector cases per domain + station capstone | + four capstones (metro, renewables, hospital, public programme) | D-05 | Planned |
| AI-governance callouts | Per-domain AI sections + blockquote callouts | Identical; updated principle wording | D-11 | Planned |
| Ethics/leadership callouts | Executive perspective sections | Identical | D-02 | Planned |
| Tables/diagrams/captions | Brand-blue tables; numbered SVG figures with hidden specs | Identical + registered alt text | D-06 | Planned |
| Templates & checklists | Practitioner's toolkits `N.T.n` | Identical + Appendix H template library (15 named templates) | D-02 | Planned |
| Chapter summaries | `## Domain N summary` | Identical | D-02 | Planned |
| Reflection questions | Exam-preparation sections | Identical | D-02 | Planned |
| Exercises | Calculation exercises with full solutions | Identical in quantitative domains | D-02 | Planned |
| MCQs & rationales | 4 options, marked, rationale, `[topic · level]` tags | Identical; two-reviewer rule for numerical items | D-04 | Planned |
| References | Standards named in prose; Appendix C index | + `registries/SOURCES.md` with rights status | D-13 | Planned |
| Glossary | Appendix B assembled from key-terms boxes | Identical | D-09 | Planned |
| Appendices | A–G suite | A–H (adds template library) | D-05 | Planned |
| Index | Generated alphabetical, page-resolved | Identical | D-07 | Planned |
| Page size/margins/typography/colour | A4, 21/17/19/17 mm, Plex Serif + Inter, brand palette | Identical `print.css` | D-07 | Planned |
| Running headers/footers/page numbers | string-set headers, centred page number | Identical | D-07 | Planned |
| Accessibility | Real text, bookmarks, contrast — no tags/alt | **Corrected**: tagged PDF, alt text, table headers | D-16 | Planned |
| Watermarking/secured edition | Platform per-copy watermark, audit, versions | Identical channel, file-name contract kept | D-26 | Planned |
| Source organisation/pipeline | Whole-domain md + WeasyPrint build | Identical, committed under `docs/books/pml-ai/` | D-15 | Planned |

**Known intentional departures:** four capstones instead of one (brief requirement, D-05); programme-
level shared registries (D-09); updated responsible-AI principle wording (D-11); accessibility
corrections (D-16); Appendix H (D-05). All logged and approved in the decision register.
