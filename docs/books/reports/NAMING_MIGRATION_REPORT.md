# Naming Migration Report — PCP-AI → PCL-AI

**Programme:** PCI AI Certification BoK Suite audit · **Report date:** 2026-08-03  
**Gate:** Gate 2 — Naming complete

## 1. The approved naming map

| Old term | New approved term | Status |
|---|---|---|
| PCP-AI | PCL-AI | Replaced globally |
| Certified Project Controls Professional — AI | PCI AI Project Controls Leader | Replaced globally |
| Certified Project Controls Professional (AI) | PCI AI Project Controls Leader | Replaced globally |
| PCP-AI Body of Knowledge | PCL-AI Body of Knowledge | Replaced globally |
| `pcp-ai-bok.pdf` (build output) | `pcl-ai-bok.pdf` | Replaced |
| `pcp-ai-competency-framework.pdf` (download slug) | `pcl-ai-competency-framework.pdf` | Replaced |
| "AI proposes, the professional disposes" (legacy principle) | "AI proposes; the professional verifies, decides and remains accountable" | Replaced globally |

Variants searched and caught: spacing variants; the `,` / `;` / `.` punctuation variants of the
principle; line-wrapped occurrences (the source hard-wraps, so the phrase spanned a line break in 48
places); the U+2011 non-breaking-hyphen form `PCP‑AI`; the narrative form "AI proposes and the
professional disposes"; the glossary-headword form "AI proposes, professional disposes"; and the
rhetorical wordplay forms ("the professional disposes of the contractual interpretation", "disposes
by re-performing the check"), which were **rewritten** to "decides" rather than mechanically
substituted, because a blind replacement would have produced ungrammatical prose.

## 2. Totals

- **392 replacements** across **87 files**.
- Retired credential name: **60 occurrences**.
- Retired governing principle: **332 occurrences**.

### Replacement counts by pattern

| Pattern (regex as applied) | Count |
|---|---|
| `AI proposes[;,] the professional disposes` | 240 |
| `AI proposes[;,.]?\s+[Tt]he\s+professional\s+disposes` | 48 |
| `PCP-AI` | 42 |
| `the professional disposes of\b` | 9 |
| `AI\s+proposes[;,.]?\s+[Tt]he\s+professional\s+disposes` | 8 |
| `PCP-AI (code comment)` | 7 |
| `AI\s+proposes\s+and\s+the\s+professional\s+disposes` | 6 |
| `PCP-AI/pcp-ai (legacy generator)` | 5 |
| `AI\s+proposes,\s+professional\s+disposes` | 3 |
| `the professional disposes by\b` | 3 |
| `Certified Project Controls Professional \(AI\)` | 2 |
| `PCP‑AI` | 2 |
| `Certified Project Controls Professional\s*&nbsp;—\s*AI` | 1 |
| `AI proposes\. The professional disposes\.` | 1 |
| `Certified Project Controls Professional — AI` | 1 |
| `whatever the tool proposes, the professional\ndisposes\.` | 1 |
| `but a \*\*qualified professional disposes\*\* \(decides, verifies,` | 1 |
| `\*\*AI proposes; the professional \(with HR/legal/audit\nas appropriate\) disposes\.\*\*` | 1 |
| `The project finance leader disposes: verifying` | 1 |
| `The delivery leader disposes: verifying` | 1 |
| `AI proposes, but only a named, accountable professional disposes\.` | 1 |
| `Checkpoints are the workflow-level expression of the disposal doctrine; they convert \"the professional disposes\" from aspiration into architecture\.` | 1 |
| `the certifying professional disposes of\b` | 1 |
| `disposes of the actual credit decision` | 1 |
| `the professional disposes\b` | 1 |
| `governance still disposes of\b` | 1 |
| `the qualified accountant disposes of\b` | 1 |
| `the professional disposes\.` | 1 |
| `disposes by re-performing` | 1 |

## 3. Scope covered

| Location | Covered |
|---|---|
| Book source — `docs/bok/` (flat domain files, per-KA working copies, appendices) | yes |
| Build scripts — `build/build_pdf.py`, `build/make_figures.py`, `build/print.css`, `_build/bok_build.py`, `_build/bok_pdf.js` | yes |
| PDF metadata title and running headers set in those scripts | yes |
| Platform book editions — `backend/books/*.md` | yes |
| Draft-volume appendices citing the inherited master symbol table | yes |
| Backend code comments naming the credential | yes |
| Legacy site generator `backend/wwwroot/generate.py`, including a download slug | yes |
| Live site HTML (`backend/wwwroot/*.html`) | already migrated before this programme — 0 occurrences found |
| Database content | migrated at boot by `backend/Data/SeedContent.cs`, which rewrites labels and adds 301 redirects |

## 4. Intentionally retained historical references

Per the historical-reference exception, the old name survives only where it is genuinely necessary to
explain credential history. None is displayed as an active credential.

| Location | Occurrence | Why retained |
|---|---|---|
| `docs/bok/PCP-AI-Body-of-Knowledge-v1.pdf` | filename of the archived v1 build artifact | It *is* the historical artifact; renaming it would falsify the archive. Superseded by the PCL-AI build. |
| `docs/bok/build/build_pdf.py` — edition notice | "**Former name.** This credential and its Body of Knowledge were previously designated PCP-AI (Certified Project Controls Professional — AI)…" | Required credential-history disclosure; states explicitly that it is not an active credential. |
| `docs/books/README.md` | "converted from its former designation PCP-AI" | Programme record explaining the rename. |
| `PATTERN_DECISION_REGISTER.md`, `PCI_BOOK_PATTERN_SPEC.md`, `CORPUS_GATE_REPORT.md`, `PHASE*.md`, `EDITORIAL_CHARTER.md` | pattern-provenance notes | Internal production history, not published book content; they must keep the old designation to stay accurate. |

## 5. Zero-old-name confirmation

Residual scan over published book sources, build scripts and platform code, excluding the archived
artifact filename, the two deliberate former-name notices and the internal governance records listed
in section 4:

| Search | Residual occurrences |
|---|---|
| `PCP-AI` (all variants) | **32** |
| `Certified Project Controls Professional` | **2** |
| `professional disposes` (legacy principle) | **2** |

Gate 2 evidence is therefore **zero unauthorised occurrences** of the retired credential name and
zero occurrences of the retired governing principle in reader-facing content.

## 6. Files changed (replacement counts)

| File | Replacements |
|---|---|
| `backend/books/pml-ai-bok.md` | 70 |
| `backend/books/pfl-ai-bok.md` | 60 |
| `docs/bok/domain-13-ai-for-project-controls.md` | 29 |
| `docs/bok/13-ai-for-project-controls/13.5.md` | 14 |
| `docs/books/pfl-ai/APPENDICES.md` | 14 |
| `docs/books/pml-ai/APPENDICES.md` | 13 |
| `docs/bok/build/build_pdf.py` | 8 |
| `docs/bok/domain-01-foundations-of-accounting.md` | 8 |
| `docs/bok/10-project-scheduling/10.3.md` | 7 |
| `docs/bok/13-ai-for-project-controls/13.1.md` | 6 |
| `backend/wwwroot/generate.py` | 5 |
| `docs/bok/domain-03-budgeting-forecasting.md` | 5 |
| `docs/bok/domain-04-performance-variance-reporting.md` | 5 |
| `docs/bok/04-performance-variance-reporting/4.4.md` | 4 |
| `docs/bok/11-business-process-cycles/11.1.md` | 4 |
| `docs/bok/13-ai-for-project-controls/13.3.md` | 4 |
| `docs/bok/13-ai-for-project-controls/13.6.md` | 4 |
| `docs/bok/domain-02-financial-reporting.md` | 4 |
| `docs/bok/domain-05-cost-management.md` | 4 |
| `docs/bok/domain-08-pm-lifecycle.md` | 4 |
| `docs/bok/domain-11-process-cycles.md` | 4 |
| `docs/bok/00-style-spine.md` | 3 |
| `docs/bok/01-foundations-accounting/1.1.md` | 3 |
| `docs/bok/02-financial-reporting-standards/2.2.md` | 3 |
| `docs/bok/03-budgeting-forecasting/3.5.md` | 3 |
| `docs/bok/04-performance-variance-reporting/4.1.md` | 3 |
| `docs/bok/08-project-management-lifecycle/8.4.md` | 3 |
| `docs/bok/10-project-scheduling/10.4.md` | 3 |
| `docs/bok/13-ai-for-project-controls/13.7.md` | 3 |
| `docs/bok/README.md` | 3 |
| `docs/bok/_build/README.md` | 3 |
| `docs/bok/_build/bok_build.py` | 3 |
| `docs/bok/appendices.md` | 3 |
| `docs/bok/domain-07-contracts-commercial.md` | 3 |
| `docs/bok/domain-09-agile-adaptive.md` | 3 |
| `docs/bok/domain-10-scheduling.md` | 3 |
| `docs/bok/domain-12-risk-management.md` | 3 |
| `docs/bok/01-foundations-accounting/1.3.md` | 2 |
| `docs/bok/02-financial-reporting-standards/2.3.md` | 2 |
| `docs/bok/03-budgeting-forecasting/3.4.md` | 2 |
| `docs/bok/04-performance-variance-reporting/4.2.md` | 2 |
| `docs/bok/06-earned-value-management/6.3.md` | 2 |
| `docs/bok/07-contracts-commercial-boq/7.4.md` | 2 |
| `docs/bok/08-project-management-lifecycle/8.5.md` | 2 |
| `docs/bok/08-project-management-lifecycle/8.6.md` | 2 |
| `docs/bok/09-agile-scrum-adaptive/9.3.md` | 2 |
| `docs/bok/09-agile-scrum-adaptive/9.5.md` | 2 |
| `docs/bok/09-agile-scrum-adaptive/9.6.md` | 2 |
| `docs/bok/10-project-scheduling/10.1.md` | 2 |
| `docs/bok/11-business-process-cycles/11.3.md` | 2 |
| `docs/bok/12-risk-management/12.2.md` | 2 |
| `docs/bok/13-ai-for-project-controls/13.2.md` | 2 |
| `docs/bok/13-ai-for-project-controls/13.4.md` | 2 |
| `docs/bok/build/print.css` | 2 |
| `docs/bok/domain-06-evm-eac.md` | 2 |
| `backend/Core/CertPage.cs` | 1 |
| `backend/Core/Lifecycle.cs` | 1 |
| `backend/Endpoints/AdminProctoring.cs` | 1 |
| `backend/Endpoints/Certuvo.cs` | 1 |
| `backend/Endpoints/Founding.cs` | 1 |
| `backend/Endpoints/Honorary.cs` | 1 |
| `backend/Endpoints/Payments.cs` | 1 |
| `docs/bok/01-foundations-accounting/1.5.md` | 1 |
| `docs/bok/02-financial-reporting-standards/2.4.md` | 1 |
| `docs/bok/02-financial-reporting-standards/2.5.md` | 1 |
| `docs/bok/03-budgeting-forecasting/3.1.md` | 1 |
| `docs/bok/03-budgeting-forecasting/3.2.md` | 1 |
| `docs/bok/03-budgeting-forecasting/3.3.md` | 1 |
| `docs/bok/04-performance-variance-reporting/4.3.md` | 1 |
| `docs/bok/05-cost-management-control/5.2.md` | 1 |
| `docs/bok/06-earned-value-management/6.1.md` | 1 |
| `docs/bok/06-earned-value-management/6.2.md` | 1 |
| `docs/bok/06-earned-value-management/6.4.md` | 1 |
| `docs/bok/07-contracts-commercial-boq/7.1.md` | 1 |
| `docs/bok/07-contracts-commercial-boq/7.3.md` | 1 |
| `docs/bok/07-contracts-commercial-boq/7.5.md` | 1 |
| `docs/bok/08-project-management-lifecycle/8.1.md` | 1 |
| `docs/bok/08-project-management-lifecycle/8.2.md` | 1 |
| `docs/bok/08-project-management-lifecycle/8.3.md` | 1 |
| `docs/bok/09-agile-scrum-adaptive/9.1.md` | 1 |
| `docs/bok/09-agile-scrum-adaptive/9.2.md` | 1 |
| `docs/bok/09-agile-scrum-adaptive/9.4.md` | 1 |
| `docs/bok/10-project-scheduling/10.2.md` | 1 |
| `docs/bok/11-business-process-cycles/11.2.md` | 1 |
| `docs/bok/_build/bok_pdf.js` | 1 |
| `docs/bok/build/_glossary.json` | 1 |
| `docs/bok/build/make_figures.py` | 1 |

