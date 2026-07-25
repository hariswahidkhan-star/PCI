# Simulation Lab — Next Release Implementation Matrix

_Branch: `cursor/simulation-lab-next-release-d975` · Baseline: 118 Sim unit tests pass, 9 Lab frontend tests pass (2026-07-23)._
_Slice evidence: content pack 30 / 242 asks; coach modes + hint ladder; attempt events + autosave; Admin Studio UI; Playwright student/admin specs._

## Existing vs target

| Area | Existing (Phase 5A) | Next-release target | This slice |
|------|---------------------|---------------------|------------|
| Scenarios | 15 published | 30 (10S/10M/7A/3E) | **30** unique seeded house scenarios (`SimLabContentPack`) |
| Graded tasks (`ask` measures) | ~47 | ≥240 | **242** validated asks |
| Template families / engines | 11 | 18 | **18** (`KnownTasks`) including productivity, boq, resource, procurement, portfolio, decision, data_quality |
| Industries | 12 labels | ≥8 governed | **26** distinct industries in pack |
| Tracks | NULL cert | PCL / PFL / PML | Tagged `certification_id` 1/2/3 across pack |
| Cross-disciplinary | 7 dual-comp | ≥12 | Dual/triple competency tags on ≥12 scenarios |
| Coach modes | 1 explain | 6 modes | Socratic / Guided / Explain / Review / Debrief / Language + 6-level hint ladder |
| Admin Studio | API yes / UI read-only | Full lifecycle UI | Create, validate, advance review, revise wired in `admin/pages/SimLab.tsx` |
| State/events | Static timeline Q&A | Deterministic state engine | `simulation_attempt_events` + autosave + idempotent submit |
| E2E | None | Full journeys | `frontend/e2e/portal-simlab.spec.ts` |
| Exam isolation | Separate tables | Must hold | Preserved + asserted in student E2E |
| PDL-AI | Banned in validator; docs stale | PML-AI only | Phase 0 audit docs corrected |

## Scenario matrix (house pack)

| Difficulty | Count | Maps to |
|------------|-------|---------|
| foundation | 10 | Simple |
| intermediate | 10 | Moderate |
| advanced | 7 | Advanced |
| expert | 3 | Expert / Capstone |

| Track (`certification_id`) | Count |
|----------------------------|-------|
| 1 PCL-AI | 13 |
| 2 PFL (Principles of Finance) | 6 |
| 3 PML-AI | 11 |

Total graded asks: **242** (every ask resolves via `SimCalc.Resolve`; pack validated in `SimContentTests`).

## Practice isolation (non-negotiable)

Simulation uses `simulation_*` tables only. Student/admin Lab endpoints never read or write `exam_attempts`, issued credentials, or formal exam grading. Access may *read* membership / exam entitlements for eligibility only (`SimLab.Eligible`).

## Implementation matrix

| Existing behavior | Gap | Location | Change | Test |
|-------------------|-----|----------|--------|------|
| 15 one-shot labs | Need 30 / 240+ asks / difficulty mix | `SimLabContentPack` | Seed/upsert 30 house scenarios | `SimContentTests` pack validate |
| 11 engines | Missing families | `SimCalc.cs` | +7 engines + parsers | `SimCalcNextReleaseTests` |
| Single coach explain | Modes + hints | `SimCoach`, `SimLab` endpoints, `LabRunner` | Modes + progressive hints writing `hints_used` | `SimCoachHintTests`; eval suite |
| Admin list-only | Studio lifecycle | `admin/pages/SimLab.tsx` | Create, validate, advance, revise | Vitest + Playwright admin |
| No E2E | Student journey | `frontend/e2e/` | Access → start → autosave → submit → coach | `portal-simlab.spec.ts` |
| PHASE_0 PDL naming | Doc debt | `PHASE_0_AUDIT.md` | PDL → PML | Grep clean |
| No attempt audit | Events / resume evidence | schema + endpoints | `simulation_attempt_events` + autosave | Unit + E2E autosave |

## Follow-up wiring (post-merge)

| Gap | Fix |
|-----|-----|
| Resume returned attempt id but not answers | Start/GET attempt return `answers` + `period`; `LabRunner` hydrates + debounced autosave |
| Catalogue had no filters | Client filters: track / industry / difficulty / kind / duration / competency + search |
| No mastery surface | `GET /api/me/lab/mastery` + recommended strip on Lab landing |
| Content issues hard to report | LabRunner → `POST /api/me/tickets` with scenario/attempt context |
| Coach evals covered 11 engines | `SimCoachEvalTests.AllTasks` now **18** engines |

## Deferred backlog (explicit)

- Full SignalR live period clock and multi-session capstone resume UI.
- Full Arabic catalogue copy pack (RTL shell + Coach language mode shipped; full AR scenario text deferred).
- Load test harness at 10k tasks (validation + solver coverage shipped; k6 deferred).
- Multi-dimension scoring UI (calculation / reasoning / decision / evidence / process / communication) beyond existing competency evidence rows.
- Bundle **import** (bulk export shipped — see below; applying a whole bundle in one action is deferred,
  since each scenario in a bundle can already be imported individually).

### Cleared since this matrix was written

- ~~Free Templates Library~~ — shipped end-to-end (§6A–6H): members-only catalogue in the student
  portal with topic/track filters, search, per-student download history and full i18n, plus the admin
  library with reach metrics and CSV export.
- ~~Dedicated `sim_lab` RBAC permission~~ — shipped (§5B.3). The Lab has its own first-class permission;
  `content` is grandfathered to it in `Rbac.PermsFor` so no existing operator lost access, and `sim_lab`
  alone confers no marketing-`content` rights.
- ~~Import/export of a validated manifest~~ — both halves shipped.
  - **Export** (§5B.4): `GET /api/admin/lab/scenarios/{id}/manifest` (gated `sim_lab`) emits a
    deterministic, byte-stable JSON manifest — content + governance + the live §14 validation verdict —
    checksummed over the graded content alone, with no export timestamp, no admin identities and no
    student usage. `Core/SimManifest.cs` is the pure builder; the Studio table exposes it as **Export**.
  - **Import** (§5B.5): `POST /api/admin/lab/scenarios/import` verifies the envelope and recomputes the
    checksum through the same canonical projection (so reformatting in transit is fine, but edited values
    are refused), then lands the scenario as a **draft**. Nothing in a file can grant published state, and
    the importing admin is recorded as the author so maker-checker still forces a second pair of eyes.
    Governance dates are not carried across environments. The Studio exposes it as **Import manifest**.
  - **Bundle** (§5B.6): `GET /api/admin/lab/manifest-bundle[?status=…]` returns every scenario's manifest
    in one deterministic file for backup or migration. Entries are ordered by `scenario_code` (not id), so
    two environments holding the same content produce the same bundle; the bundle checksum answers "same
    catalogue?" in one comparison while each entry keeps its own checksum to localise a difference. The
    Studio exposes it as **Export all**.
- ~~Independent multi-admin maker-checker E2E~~ — shipped (§5B.7). `portal-simlab.spec.ts` drives two real
  admin sessions against the same scenario: the author is refused approval in the Studio and a second,
  `sim_lab`-only admin approves it, with the audit trail attributing the approval to the checker. The
  Studio now explains the refusal in a sentence instead of showing the raw `maker_checker` code.

## Baseline → slice evidence

```
dotnet test … --filter FullyQualifiedName~Sim → content-pack commit Passed 119
(additional coach/calc tests added in this commit)
vitest Lab*.test.tsx SimLab.test.tsx
Playwright portal-simlab.spec.ts (student + admin)
```
