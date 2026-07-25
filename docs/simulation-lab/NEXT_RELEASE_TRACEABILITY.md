# Simulation Lab — Next-Release Traceability

_Baseline audited: `main` @ `d21b7cc` ("Simulation Lab next release — content pack, coach, Admin
Studio (#89)"), 2026-07-23. Supersedes the prior traceability audit taken at
`codex/simulation-next-release` @ `0288f01` (pre-Phase-5A), whose findings are restated and
re-verified below._

This document reconciles the repository against the three external work orders:

1. `PCI_SIMULATION_LAB_MARKET_EXPANSION_PROMPT.md` — market-launch destination
   (120 reviewed base scenarios, 1,000+ graded tasks);
2. `PCI_SIMULATION_LAB_CURSOR_IMPLEMENTATION_PROMPT_V2.md` — same destination, implementation
   detail; and
3. `PCI_SIMULATION_NEXT_RELEASE_CURSOR_PROMPT.md` — the next increment (30 new base scenarios,
   240+ meaningful tasks, 10 Simple / 10 Moderate / 7 Advanced / 3 Expert).

The prompt files themselves are not stored in the repository. The repo-side implementation record
is `docs/simulation-lab/NEXT_RELEASE_MATRIX.md` (slice evidence) and
`docs/simulation-lab/PHASE_0_AUDIT.md`. Counts below are repository evidence verified at this
commit; the "Verified test evidence" section records which runtime gates were actually executed
during this audit and which remain unverified.

## Addendum — P0 residual closed (audited and fixed at `main` @ `fe9a0e0`)

Everything below this section describes `d21b7cc`. After that audit merged, `main` moved again
(PR #92/#93/#94/#96: the 96-scenario `simlab_scenarios_seed.json` library, catalogue filters,
catalogue-wide reference-solver + certification-coverage gates, mastery/recommendations, coach
evals, review-due/expiry governance), and the accompanying change to this document closes the P0
residual identified below plus two defects found while verifying it:

- **Historical replay is now pinned.** `simulation_scenario_versions` freezes each
  (scenario, version) config the first time it is served (`Core/SimVersion.cs`); attempt
  start/resume, load, submit and coach all replay from the frozen snapshot
  (`Endpoints/SimLab.cs`), a boot-time backfill freezes attempts that predate the table
  (`SimLabSchema.FreezeAttemptedVersions`, ordered before any seeder), and the content pack now
  bumps `version` when — and only when — a scenario's `config_json` actually changes
  (`SimLabContentPack.Upsert`). A pack-changing deploy therefore no longer rewrites what an
  existing attempt replays, grades or coaches as. Proven by `SimReplayTests.cs` (4 tests).
- **Count correction.** The evidence table below says "45 published house scenarios (15 + 30)";
  in fact the pack's 30 codes *include* the original 15 seeds (it densifies them in place), so the
  house catalogue is 30 scenarios, plus the 96-scenario JSON library seeded at app boot — 126
  published scenarios in a production boot.
- **Pre-existing defect fixed: 24 library scenarios had no certification mapping.** 24 of the 96
  `simlab_scenarios_seed.json` rows shipped without `certification_id`, violating the
  certification-coverage gate PR #93 itself added (integration assertion 43zd) on any install that
  loads the library. Certifications are now assigned (library balance restored to 32/32/32 across
  PCL-AI/PFL/PML-AI, task families kept near-even), the seed version bumped to 2 with a
  NULL-only backfill for already-seeded installs, and `SimLabContentSeedTests` now asserts the
  mapping so CI catches any recurrence. The integration harness line that crashed on the NULL
  (`sorted()` over ints and `None`) is also fixed so this gate fails cleanly instead of aborting
  the run.

Test evidence for this addendum (this container, 2026-07-24): backend Sim suite **167/167**
(includes the 4 new replay tests), full integration suite **1092/1092** (exit 0, library loaded,
coverage gate green), migration integrity **13/13**, `dotnet build -c Release` clean. The
integration suite at unmodified `fe9a0e0` crashes at assertion 43zd in any environment that loads
the JSON library — the failure predates this change and is fixed by it.

## What landed between the two audit baselines

The `0288f01` audit predates all Phase 5A and next-release work. Since then, `main` merged:

- **PR #85** — scenario governance schema, deterministic content validator (`SimContent.cs`),
  deterministic variant engine (`SimVariant.cs`).
- **PR #87** — live variants in the attempt path, authoring + enforced review/publication
  workflow (`SimReview.cs`), published-row immutability with controlled revise.
- **PR #90** — Admin Simulation Studio UI (`frontend/src/admin/pages/SimLab.tsx`).
- **PR #89** — 30-scenario content pack (`SimLabContentPack.cs`), coach modes + hint ladder,
  attempt events + autosave, student/admin Playwright journeys
  (`frontend/e2e/portal-simlab.spec.ts`).

Most rows of the earlier audit have therefore moved. The re-verified state follows.

## Current evidence (verified at this commit)

| Capability | Implemented evidence | Current count/state | Remaining gap |
|---|---|---|---|
| Scenario catalogue | `SimLabSchema.cs:164-216` (15 originals), `SimLabContentPack.cs` (30 pack) | 45 published house scenarios (15 + 30) | Launch target is 120 total reviewed scenarios |
| New-content difficulty | Pack literals in `SimLabContentPack.cs` | 10 foundation / 10 intermediate / 7 advanced / 3 expert | Matches the 10S/10M/7A/3E requirement for the 30 new scenarios |
| Tracks | `certification_id` in pack rows | 13 PCL-AI / 6 PFL / 11 PML-AI | Original 15 seeds remain untracked (NULL) |
| Industries | Pack literals | 26 distinct industries in the pack | Governed industry-pack taxonomy still informal |
| Graded work | 47 `"key"` fields in `SimLabSchema.cs`; `SimLabContentPack.TotalAskCount` | 47 (originals) + ≥240 asserted, 242 recorded (`SimContentTests.cs:252-253`, matrix) | Asks are single graded measures, mostly one-step; see task-quality note below |
| Deterministic engines | `SimCalc.KnownTasks` (`SimCalc.cs:718`) | 18 engines (11 original + productivity, boq, resource, procurement, portfolio, decision, data_quality) | — |
| Variants | `SimVariant.cs`; seed drawn per attempt (`SimLab.cs:160-166`) | Deterministic per-attempt instance; seed 0 = canonical | Variants must not be counted as unique scenarios (they are not, in the 30) |
| Content validator | `SimContent.cs` (§14) | Engine/key pairs, unique keys, tolerances, leakage checks; publication-gated (`SimReview.RequiresPublishable`) | — |
| Review workflow | `SimReview.cs`; `AdminSimLab.cs:159-235` | draft → calc/learning/safety review → pilot → approved (maker-checker) → published → retired | — |
| Published immutability | `AdminSimLab.cs:169-179` (409 on edit), `:212-235` (revise clones to new row, version+1) | API-level immutability enforced | Startup seed upsert can still rewrite house rows — see critical-gap section |
| Coach | `SimCoach.cs:22-32` (6 modes), hint ladder, `hints_used` persisted (`SimLab.cs:319`) | socratic / guided / explain / review / debrief / language + progressive hints | Provider red-team eval coverage still thin; recommendations absent |
| Student API | `SimLab.cs:70-327` | access, catalogue, start, autosave, list, load, submit, coach; attempt events (`simulation_attempt_events`, `SimLabSchema.cs:139`) | No mastery view or next-scenario recommendation endpoints |
| Student UI | `Lab.tsx`, `LabRunner.tsx` | Catalogue with difficulty/industry labels; runner with autosave, decision tasks, coach mode + hint-level controls | No catalogue search/filter controls; no uploads; multi-step linked state limited |
| Admin UI | `frontend/src/admin/pages/SimLab.tsx` | List, create, validate, advance review, revise | Preview-as-student and richer diffing not present |
| Calculation tests | `SimCalcTests.cs` (39), `SimCalcNextReleaseTests.cs` (7) | 46 Fact/Theory methods | Property/fuzz tests for numeric boundaries still absent |
| Content tests | `SimContentTests.cs` (23) | Pack-wide validate + reference-solve of every ask | — |
| Coach tests | `SimCoachTests.cs` (3), `SimCoachEvalTests.cs` (4), `SimCoachHintTests.cs` (4) | Grounding, withholding, hint ladder | Injection/Arabic/provider-output eval breadth |
| Review/variant tests | `SimReviewTests.cs` (8), `SimVariantTests.cs` (7) | Workflow + determinism | — |
| Browser E2E | `frontend/e2e/portal-simlab.spec.ts` | Student journey (start GL-EVM-001 → autosave → submit → coach) + admin journey (list, validate, create draft, student blocked from admin APIs) | Not executed in this audit (needs live backend + SPA); no revise/publish E2E |

The task-quality caveat from the earlier audit still applies, scaled up: the release counts are
**graded answer fields** (242 new asks across 30 scenarios), validated end-to-end against the
deterministic solver. They satisfy the prompt's numeric floor, but most scenarios remain
single-step measure banks. Multi-step linked state with decision consequences exists only in the
`decision` engine family; a documented product definition of "task" beyond "graded ask" has not
been reviewed. Do not present 242 asks as 242 authored multi-step tasks.

## Status of the previous audit's critical design gap

The `0288f01` audit found that attempts recorded `scenario_version` but load/submit/coach fetched
the **current** `simulation_scenarios.config_json` by `scenario_id`, so editing a published row
changed historical replay, grading and coaching.

**Partially closed.** Two of the three mutation paths are now shut:

- Admin edits to an approved/published/retired scenario return `409 immutable`
  (`AdminSimLab.cs:169-179`).
- Revision clones into a **new** row — new id, new `scenario_code`, `version+1`, starting at
  draft (`AdminSimLab.cs:212-235`) — so attempts pinned by `scenario_id` never see revised
  content. Variant scenarios additionally pin a per-attempt `seed`, replayed through
  `EffectiveConfig` (`SimLab.cs:348-356`).

**Residual gap (P0 leftover):** attempt load, submit and coach still read the live row
(`SimLab.cs:220`, `:259`, `:303`) rather than a frozen snapshot, and
`SimLabContentPack.Upsert` (`SimLabContentPack.cs:449-465`) rewrites `config_json` in place at
**every startup** for house rows (`authored_by IS NULL`) — including published rows with
historical attempts. A deploy that edits the pack therefore still mutates historical replay for
seeded content. The immutability guarantee currently protects admin-authored content only.
Closing this requires either (a) an immutable per-version snapshot table that attempts reference,
or (b) making the pack upsert version-aware (insert a new row/version instead of updating one
that has attempts). Until then, "old attempts replay identically after a new revision" holds for
Studio-authored content but **not** across code deployments of pack content.

## Traceability to the three prompts

| Requirement | Status at `d21b7cc` | Release gate |
|---|---|---|
| Preserve auth, entitlements and exam separation | Met and asserted | Isolation asserted in student E2E; `simulation_*` tables only (`NEXT_RELEASE_MATRIX.md`) |
| Immutable published scenario versions | Partial (see critical-gap section) | Old attempts replay identically after revision **and** after a pack-changing deploy |
| Deterministic reference solver per scenario/variant | Met for delivered content | `SimContentTests` resolves every ask of all 45 scenarios via `SimCalc.Resolve`; validator rejects unresolved keys |
| 30 new scenarios / 240+ tasks | Met at count level (30 / 242) | Independent content review of ask quality; counts verified by `SimContentTests` |
| 10 Simple / 10 Moderate / 7 Advanced / 3 Expert | Met (foundation/intermediate/advanced/expert mapping) | Explicit naming normalization documented in matrix |
| 120 scenarios / 1,000+ tasks | Later market-launch target (45 / ~289 today) | Must not block the accepted next release |
| Tracks, industries, roles, cross-disciplinary | Largely met for new content (13/6/11 tracks, 26 industries, ≥12 dual-competency) | Metadata validation exists; governed taxonomy + roles still informal |
| Multi-step state, decisions and event effects | Partial | `decision` engine + attempt events + autosave exist; linked multi-step scenarios with downstream effects remain limited |
| AI Coach progressive hinting and grounded citations | Largely met | 6 modes, 6-level hint ladder, `hints_used` persisted; eval breadth (injection, Arabic, provider output) still to grow |
| Explainable mastery/recommendations | Not met | Per-attempt competency evidence rows exist (`SimLab.cs:280`); no durable mastery view or recommendation engine |
| Student end-to-end journey | Implemented; not executed in this audit | `portal-simlab.spec.ts` student journey green in CI-capable env |
| Admin Simulation Studio | Met (create/validate/advance/revise UI + API) | Admin E2E green; publish/revise E2E depth can grow |
| Arabic/RTL, WCAG 2.2 AA, responsive | Partial (RTL shell + Coach language mode per matrix) | axe automation, keyboard smoke, full AR catalogue copy |
| MySQL 8, scale and concurrency | Not demonstrated | MySQL migration parity + 300/10,000 catalogue + concurrent autosave/submit/coach gates |

## Verified test evidence (this audit, 2026-07-23)

Executed in this container (Linux, .NET SDK 8.0.129 via apt, Node 22):

| Command | Result |
|---|---|
| `dotnet test tests/PCI.Backend.Tests/PCI.Backend.Tests.csproj --filter "FullyQualifiedName~Sim"` | **Passed 130, Failed 0** (exit 0) |
| `npm run typecheck` (frontend) | Pass (exit 0) |
| `npx vitest run Lab LabRunner SimLab` | **3 files, 12 tests passed** (exit 0) |
| `python3 tests/migration_integrity_test.py` | **13/13 passed** (exit 0); MySQL provider-parity step self-skipped (`TEST_DB_PROVIDER=mysql` unset) |
| `python3 tests/integration_test.py` | **1084/1084 passed** (exit 0) — includes the Simulation Lab sections (access entitlement, published catalogue, Practice Lab test-user journey) |
| `npx playwright test e2e/portal-simlab.spec.ts` | **Not run** — requires live backend + SPA + provisioned test users |
| MySQL 8 migration/integration gates | **Not run** — no MySQL service in this environment |

Evidence-honesty notes from this run, kept because they will recur in fresh containers:

- On the first attempt both Python suites printed a skip message and the *pipeline* exited 0
  because `bin/Release/net8.0/PCI.Backend.dll` was absent — and `python3 … | tail` reports
  `tail`'s exit code, not Python's. Always check the suite's own summary line, and use
  `PIPESTATUS` (or no pipe) when recording exit codes.
- The integration suite initially reported 1083/1084 (exit 1): assertion `18u` (shipped PML-AI
  BoK personalised-PDF text) failed because the container's system `cryptography`/`cffi` stack
  was broken, so `pypdf` import panicked and `_pdf_text` fell back to byte-level extraction that
  cannot read the watermark. `pip install cffi` fixed the stack; the full suite then passed
  1084/1084. This was a test-environment defect, not a product regression.

## Remaining backlog (supersedes the earlier P0–P3 phasing)

**P0 residual — replay immutability across deploys**
- Freeze attempt-time content: per-version snapshot referenced by attempts, or version-aware pack
  seeding that never updates a published row with attempts.
- Test: attempt graded identically before/after a pack edit + restart, and before/after a Studio
  revision.

**P2 leftovers — quality and evals**
- Property/fuzz tests for numeric boundaries and malformed scenario graphs.
- Coach red-team evals: malicious scenario text, fabricated facts, cross-user requests,
  assessment-answer extraction, Arabic.
- Catalogue search/filter UI; durable mastery view; explainable next-scenario recommendations.
- Documented product definition of "task" and an independent content-quality review of the 242
  asks.

**P3 — market launch (unchanged)**
- Grow to 120 reviewed scenarios / 1,000+ graded asks across families and industry packs.
- Arabic/RTL copy completion, axe/keyboard gates, MySQL 8 parity, 300/10,000 capacity, resilience
  and performance gates.

Deferred items already declared in `NEXT_RELEASE_MATRIX.md` (SignalR live clock, `sim_lab` RBAC
permission, full AR catalogue, k6 load harness, multi-admin maker-checker E2E, multi-dimension
scoring UI, manifest I/O) remain deferred and are not re-listed as gaps here.

## Environment blockers and evidence rules (unchanged)

- Playwright requires the backend and student/admin SPA running plus deterministic test users.
- MySQL gates require a reachable MySQL 8 service; the local default is SQLite.
- Provider-backed Coach tests must stay deterministic in CI (mock endpoint, no live paid model).
- `secureexam/` is Windows-only and outside this Linux gate.
- Never mark a phase complete from file counts alone; record commands, exit codes, coverage
  output, failures and explicit deferrals — and verify that an exit-0 suite actually ran.
