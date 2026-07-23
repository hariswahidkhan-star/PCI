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

## Deferred backlog (explicit)

- Full SignalR live period clock and multi-session capstone resume UI.
- Free Templates Library (out of scope by prompt).
- Dedicated `sim_lab` RBAC permission (still uses `content`).
- Full Arabic catalogue copy pack (RTL shell + Coach language mode shipped; full AR scenario text deferred).
- Load test harness at 10k tasks (validation + solver coverage shipped; k6 deferred).
- Independent multi-admin maker-checker E2E with two live admin accounts (API maker-checker covered in integration).
- Multi-dimension scoring UI (calculation / reasoning / decision / evidence / process / communication) beyond existing competency evidence rows.
- Import/export validated manifest UI (API revise/clone path exists; full manifest I/O deferred).

## Baseline → slice evidence

```
dotnet test … --filter FullyQualifiedName~Sim → content-pack commit Passed 119
(additional coach/calc tests added in this commit)
vitest Lab*.test.tsx SimLab.test.tsx
Playwright portal-simlab.spec.ts (student + admin)
```
