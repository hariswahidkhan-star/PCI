# Calculation-Content Validation — Phase 0 Report

_Inventory, risk classification and validation design for every calculation-driven learning asset
in the repository, per the platform's content-assurance standards (never trust a stored key;
independently derive every expected result; version every correction). Compiled 2026-07-25 against
`main` @ `da2ffff`. Companion documents: `FORMULA_REGISTRY.md` (formula definitions and
conventions), `DEFECT_REGISTER.md` (findings)._

## 1. Master content inventory

| # | Family | Source of truth | Items | Answer-key storage | Independent validation status |
|---|--------|-----------------|-------|--------------------|-------------------------------|
| 1 | SimLab starter scenarios | `backend/Data/SimLabSchema.cs` → `simulation_scenarios` | 16 | None stored — derived by `SimCalc.Resolve` at grade time | **VALIDATED** — engine pinned by hand-computed `SimCalcTests`; content gated by `SimContent.Validate` reference-solve in CI |
| 2 | SimLab content pack | `backend/Data/SimLabContentPack.cs` | 30 (≥240 asks) | None stored | **VALIDATED** — `SimContentTests.Content_pack_scenarios_validate_and_every_ask_resolves` |
| 3 | SimLab scale library | `backend/simlab_scenarios_seed.json` via `SimLabContentSeed` | 182 (669 asks: 643 number / 14 set / 12 bool) | None stored; `worked_solution` narrative only | **VALIDATED** — `SimLabContentSeedTests` re-runs every entry through the publication gate incl. reference solver + variant seeds 1–6 |
| 4 | PCI World challenge bank | `backend/Data/WorldContentPack.cs` → `pciworld_challenges` (+ immutable `_versions`) | 50 | None stored; decision options carry authored `quality` 0–100 | **VALIDATED** — `WorldTests` reference-solve every ask; rotation/versioning covered by `WorldRotationTests` |
| 5 | Certuvo practice MCQs | `backend/certuvo_seed.json` → `sample_questions` (`is_practice=1`) | 40 | `answer_index` + `explanation` stored | **VALIDATED (this phase)** — all 40 keys independently re-derived; 6 calculation goldens + structural gates pinned in `QuestionBankTests` |
| 6 | Demo live-exam pack (opt-in, `SEED_DEMO_EXAM`) | `backend/demo_exam_seed.json` → `sample_questions` (`is_practice=0`) | 24 (seeded ×3 certifications) | `answer_index` stored | **VALIDATED (this phase)** — all 24 keys independently re-derived; 12 calculation goldens pinned in `QuestionBankTests` |
| 7 | Public sample-questions download | `docs/downloads/sample-questions.md` → `wwwroot/downloads/sample-questions.pdf` | 16 | ✅ marker + rationale in markdown | **VALIDATED (this phase)** — all 16 keys verified; 3 numeric goldens + one-key-per-item gate pinned in `QuestionBankTests`. Naming defect DEF-21 open |
| 8 | Master formula sheet | `docs/downloads/master-formula-sheet.md` → PDF | ~40 formulas | N/A (definitions) | **VALIDATED (this phase)** — manually reconciled line-by-line against authoritative definitions; all correct. Registered in `FORMULA_REGISTRY.md`. Naming defect DEF-21 open |
| 9 | Free templates library | `backend/Data/TemplatesSchema.cs` → `templates` (inline CSV) | 15 | None — blank learner worksheets without embedded formulas | **PASS (by construction)** — no formulas to corrupt; download bytes covered by `integration_test.py`. CSV formula-injection guard: `Core/Csv.Field` (DEF-1/DEF-11 fixed) |
| 10 | AI Coach | `backend/Core/SimCoach.cs` | 6 modes | N/A — receives engine-verified values; may not compute | **VALIDATED** — `SimCoachEvalTests` asserts every printed number traces to given/engine/score; hint ladder withholds answers (`SimCoachHintTests`) |
| 11 | Other study corpora (BoK markdown, lecture PDFs, knowledge pages) | `docs/bok/**`, `docs/lectures/**`, `wwwroot/knowledge-*.html` | ~78 md + PDFs | Worked examples inline | **NOT_VALIDATED** — Phase 5 scope (see §4) |
| 12 | Admin-authored runtime content | `sample_questions` CSV bulk upload, admin SimLab/World authoring | runtime | `answer_index` / configs | Authoring-time gates only (`SimContent`/`WorldContent.Validate`); no confidential live bank is committed to source control (by policy) |

Duplicates/near-duplicates: the 15 pack codes that densify starter rows are counted once (union =
213 unique SimLab scenario codes); JSON-library variants that share the engine are validated per
scenario but correctly NOT claimed as independent derivations of each other.

## 2. Risk classification (per severity policy §20)

| Risk band | Families | Rationale |
|---|---|---|
| Critical | 6 (demo live-exam keys), 12 (admin-authored live banks) | Wrong live-examination key. Mitigated: pack keys now independently derived + CI-pinned; real live banks are authored privately, never committed |
| High | 1–4 (SimLab + World scoring), 10 (coach) | Credential-adjacent scoring and displayed "truth". Mitigated: engine hand-pinned, content reference-solved, coach forbidden to compute |
| Medium | 5, 7, 8 (practice/study keys and formulas), grading-policy consistency (DEF-20) | Misleads study; does not directly award credentials |
| Low | 9, 11 (blank templates, prose corpora) | Formatting/instructional clarity |

## 3. Independent expected-answer methodology

1. **Engine truth is pinned independently of the engine.** `SimCalcTests` /
   `SimCalcNextReleaseTests` assert hand-computed first-principles values (documented as such in the
   test headers) for every task family; the content gates then use the engine as reference solver.
   Chain: human-derived expectations → engine → content. A content item can therefore never be
   "validated" by the same artefact that computed it.
2. **MCQ keys are claims, not truth.** `QuestionBankTests` re-derives every calculation answer with
   `decimal` arithmetic inside the test (never calling platform code) and requires the stored
   `answer_index` to select exactly — and only — the option carrying the derived value. Structural
   gates additionally forbid duplicate options, out-of-range keys, missing explanations (practice
   pack) and retired certification names.
3. **Judgment questions** are validated for rubric soundness (single defensible best option,
   non-ambiguous distractors, explanation consistent with the key) by review; they are not forced
   into numeric goldens. All 74 judgment items across families 5–7 were reviewed this phase; no key
   changes were required.
4. **Stochastic content** (Monte Carlo exhibits) is seeded and reproducible; regression asserts
   statistical properties and percentile ordering, never a single random draw.
5. **Two-derivation rule for high-risk items.** Where the two derivations disagree, or a policy
   question exists (e.g. DEF-20's ordered-vs-unordered critical-path grading), the item is marked
   `blocked_for_expert_review` in the defect register rather than silently changed.

## 4. Findings this phase

| Finding | Severity | Disposition |
|---|---|---|
| All 80 committed MCQ keys (40 + 24 + 16) are **correct** — every calculation key matches its independent derivation; every judgment key is defensible with valid distractors | — | Pinned as CI gates in `QuestionBankTests` so future edits cannot silently corrupt a key |
| All ~40 master-formula-sheet formulas are **correct** (EVM family incl. both TCPI forms, earned schedule, PERT, floats, CPIF, working-capital cycle) | — | Reconciled into `FORMULA_REGISTRY.md` |
| PCI World grades `critical_path` asks as an **ordered** list while SimLab grades the identical ask type as an **unordered set** — the same learner answer can be right in one product and wrong in the other (6 World challenges affected) | Medium | **DEF-20** — `blocked_for_expert_review`: harmonising in either direction changes published scoring behaviour and needs a product ruling |
| The retired **PCP-AI** name (per `SimContent.RetiredNames`) is the branding of all eight public download documents (`docs/downloads/*.md` + committed PDFs), while the Downloads Centre lists them neutrally | Medium | **DEF-21** — open: requires a coordinated rewrite + PDF regeneration + a decision on whether the "PCP-AI Body of Knowledge, First Edition" citation is historically accurate branding or debt |

## 5. Remaining phases (not claimed complete)

- **Phase 2/5 residue:** BoK markdown corpus and lecture PDFs (family 11) worked examples are not
  yet machine-validated; sampling strategy required (~78 files).
- **Phase 6:** localization parity is currently moot for calculation content (no Arabic variants of
  any calc item exist — English-only by design); the coach's `language` mode reuses engine numbers,
  so no numeric divergence path exists today. Re-assess when Arabic content ships.
- **MySQL note:** all content gates above run in the `backend-unit` CI job; the adversarial Python
  suites also run against MariaDB in `backend-mysql`. `backend-unit-mysql` currently filters to
  Partner tests only — extending it to the Sim/World/QuestionBank content gates is a cheap
  hardening follow-up.
- No unresolved Critical or High calculation defect is open; the two Medium findings above are
  registered and quarantine is not required (neither corrupts a stored score).
