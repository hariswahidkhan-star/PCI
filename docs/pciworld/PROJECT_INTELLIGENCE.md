# PCI Project Intelligence — architecture decision record, bank audit and Release-1 plan

**Product name:** PCI Project Intelligence · **Tagline:** *Think. Decide. Deliver.*

PCI Project Intelligence is the premium professional-practice programme inside PCI World: daily
decision practice, a governed practice library, and a one-year governed content calendar — built
**on** the existing PCI World engine, never beside it.

This document records the architecture decision, the audit of the existing challenge bank against
the Project Intelligence specification, what Phase A (this change) delivers with traceable
verification, and the explicit deferral register for the remainder of Release 1.

---

## 1. Architecture decision: extend, do not fork

**Decision.** Project Intelligence reuses the existing PCI World foundations unchanged:

| Requirement | Reused implementation |
|---|---|
| Challenge identity + immutable versions | `pciworld_challenges` / `pciworld_challenge_versions` (`Core/WorldLifecycle.cs`) |
| Deterministic scoring | `Core/WorldScore.cs` + `Core/SimCalc.cs` (server-side only) |
| Daily release, exact-once, cycles, substitution | `Core/WorldRotation.cs` ledger (periods / order / runs / lock) |
| Attempts (pin version, resume, first-submit-wins) | `pciworld_attempts` (`Endpoints/World.cs`) |
| Participant identity + Passport | `Endpoints/WorldAccount.cs`, `Core/WorldPassport.cs` |
| Separate admin realm, RBAC, maker-checker, audit | `Endpoints/WorldAdmin.cs`, `WorldRbac`, `pciworld_audit` |
| Publication validation + answer-leakage gate | `Core/WorldContent.cs` (`Validate` / `PublicView`) |

No parallel `pi_*` attempt, schedule, sharing or identity engine was created. New code is limited to:

- **`Core/WorldIntelligence.cs`** — the governed taxonomy (8 experience types, 12 competency
  domains, 6 lifecycle stages, 10 sectors, 6 interaction patterns, 4 duration bands, 4 difficulty
  reporting bands), the executable classification of the existing bank, the premium-language gate,
  and the Year-1 plan loader + coverage computation.
- **`pciworld_challenges.pi_*` columns** — five additive, idempotently-installed catalogue facets
  (`pi_type`, `pi_domain`, `pi_lifecycle`, `pi_sector`, `pi_interaction`). Deliberately **not** on
  `pciworld_challenge_versions`: facets are how content is found and reported, never part of what
  an attempt replays, so replay immutability is untouched.
- **`Endpoints/WorldIntelligence.cs`** — the versioned, read-only learner API
  (`/api/world/v1/project-intelligence/{home,categories,catalog,daily}`) and the world-admin
  coverage report (`/api/world-admin/intelligence/coverage`, `read` action group). Attempts stay
  on the single existing pipeline in `Endpoints/World.cs`.
- **`backend/content/project-intelligence/year-1/`** — the governed Year-1 plan (see §3), shipped
  with the app (`PCI.Backend.csproj` content include) so the coverage report reads the same files
  CI verified.

**Placement note.** The specification sketches `content/project-intelligence/year-1/` at the
repository root; the files live under `backend/content/…` because the Docker build copies
`backend/` only and the publish pipeline ships backend content items. Same governed layout,
repository-consistent location.

**Boundary invariants (unchanged from the World realm):** no Project Intelligence code path reads
or writes exam, entitlement, credential, membership or platform `users` tables; participation can
never create certification evidence; learner payloads are allow-listed (`PublicView`) so reference
values, option qualities and consequences cannot leak pre-submission.

## 1b. Phase B slice 1 (second change on this branch)

Delivered on top of Phase A:

- **Progressive authored hints (PI-US-051)** — `hints` is now a first-class, validated content
  field: exactly three non-empty strings when present, scanned by the answer-leakage gate
  (a hint may teach the method, never the value). `PublicView` exposes only `hints_available`;
  hint text leaves the server solely through `POST /api/world/attempts/{id}/hint` — one hint per
  request, in authored order, in-progress attempts only, guarded increment (`hints_used`), no
  hidden score effect. Verified by four new gate tests and a live smoke (reveal 1→2→3, idempotent
  at 3, 409 after submit).
- **January fully authored** — `Data/WorldIntelligencePack.cs` adds 28 new experiences written TO
  their plan slots (11 Daily Decisions, 8 Stakeholder Dilemmas, 6 Risk Rooms with EMV reference
  solves, 2 Schedule Strategies with CPM networks, 1 Cost & Value with EVM), every one carrying
  the full editorial contract (three hints, consequence + principle per option, share line) and
  passing the publication validator and reference solve in CI. With the three existing
  January-mapped bank items pinned in place, **all 31 January days are backed: runway_days = 31**
  — still below the 60-day bar, so the runway alert correctly remains raised.
- **Plan-quality improvements to the generator** — mapped items rotate across theme-matching
  months, planned Executive Missions are capped at two per month, types are interleaved within
  each month, and sectors are apportioned largest-remainder so every sector appears all year.
  All distribution tables remain exact (CI-verified).

The bank now holds **80 published house challenges** (52 legacy + 28 Year-1 January).

## 1c. Phase B slice 2 — February fully authored

- **26 new experiences** (`WC-SCO-081`…`WC-STK-098`, `WC-EVM-099`, `WC-CHG-100`, `WC-CAP-101/102`,
  `WC-RSK-103/104`, `WC-CPM-105/106`) complete February's plan: 9 Logic & Sequence scope exercises
  (WBS roll-ups + structuring decisions), 9 Stakeholder Dilemmas on scope/requirements alignment,
  2 Cost & Value (EVM forecasting set, change-register baseline), 2 capstone Executive Missions
  (three-stage definition-gateway and platform-consolidation missions), 2 Risk Rooms (EMV), and
  2 Schedule Strategies (CPM). Every item carries the full editorial contract and passes the
  publication validator, reference solve, leakage scan and premium-language gate in CI.
- With the two legacy WBS items pinned to their February days, **all 59 January + February days
  are backed: runway_days = 59** — one day below the 60-day bar, so the runway alert correctly
  remains raised until March authoring begins. The bank now holds **106 published challenges**.
- Note on Logic & Sequence rendering: the catalogue interaction is `order_rank`; until the World
  React shell (D1) ships a dedicated ordering control, these items express sequencing through
  keyboard-accessible structured decisions and WBS roll-ups — accessible by construction.

## 1d. Phase B slice 3 — March fully authored: **the 60-day runway alert clears**

- **18 new experiences** (`WC-SCH-107..115`, `WC-CBS-116`, `WC-CSH-117`, `WC-BOQ-118`,
  `WC-RSK-119..121`, `WC-CAP-122/123`, `WC-STK-124`) complete March (theme: planning, sequencing
  and critical-path judgment). The nine Schedule Strategy items deliberately span four
  deterministic engines — CPM networks, earned schedule (ES/SPI(t)/EAC(t)), PERT three-point
  paths, and budget-weighted progress — plus CBS roll-up, cash-flow peak funding, BOQ pricing,
  two EMV Risk Rooms, one evidence-diagnosis Risk Room, two capstone Executive Missions and a
  resources-and-leadership Stakeholder Dilemma.
- With March's thirteen legacy schedule items pinned to their theme-month days, **all 90 days of
  Q1 are backed by published challenges: runway_days = 90 and the 60-day runway alert clears for
  the first time — on real content, not a claim.** Bank: 124 published challenges.
- Content-defect note found while verifying: the `boq` engine's `average_rate` is the simple mean
  of line rates. `WC-BOQ-118`'s ask was corrected pre-publication; legacy `WC-BOQ-012`/`035` carry
  the same "weighted average" label against the mean-of-rates engine — logged for the D3
  remediation pass (label fix = new immutable version).

## 1e. Phase B slices 4–13 — April through December + the reserve: **the Year-1 bank is complete**

Authored month-by-month on this branch, each slice gated on the full xUnit suite (commit only on
`Failed: 0`) with the distribution tables re-verified exactly at every step:

| Slice | Month/theme | New items | Runway after |
|---|---|---|---|
| 4 | April — cost, value & commercial | 25 | 120 |
| 5 | May — risk & uncertainty | 25 | 151 |
| 6 | June — scope, requirements & logic | 26 | 181 |
| 7 | July — delivery control, change & recovery (Project Rescue month) | 26 | 212 |
| 8 | August — leadership, teams & communication | 29 | 243 |
| 9 | September — commissioning, handover & closeout quality | 28 | 273 |
| 10 | October — digital delivery, data quality & responsible AI (all advanced band) | 27 | 304 |
| 11 | November — integrated controls & executive trade-offs (all deep band) | 26 | 334 |
| 12 | December — handover, closeout, lessons & capstones (six capstones, five executive) | 29 | **365** |
| 13 | Reserve bank PI-Y1-R001–R055 (`WorldIntelligencePack.R.cs`, WC-*-366..420) | 55 | — |

**End state: all 420 slots of the governed Year-1 plan are mapped to published, validated bank
challenges** — 365 scheduled days (runway 365, alert permanently cleared) plus the 55-slot reserve
substitution stock (20 foundation dailies, 8 practitioner rescues, 12 numeric rooms across risk/
schedule/cost engines, 4 negotiation dilemmas, 4 order/rank sequences, 2 executive capstones).
Every item carries exactly three progressive hints, passes the premium-language and answer-leakage
gates, and solves through the reference solver. D2 in the deferral register is **closed**; D3
(legacy-item remediation) remains open.

Incidents worth remembering (all caught by the gates, all fixed before merge): a banned-language
hint in June ("the whole game") that briefly reached the branch when a commit was gated on the
wrong grep pattern — the gate is now `grep -q "Failed:     0"` on the full-suite output; a
`trivia` in a September hint; four December capstones authored with four hints instead of three;
an invalid `commercial_management` track value; and five title collisions across the 420-slot
uniqueness gate. The `boq` `average_rate` label defect (simple mean, not weighted) found in April
remains logged against legacy WC-BOQ-012/035 under D3.

## 2. Audit of the existing 52-challenge bank

Every house challenge (`WC-…-001` … `WC-…-052`) was classified into the Project Intelligence
taxonomy — the classification is executable code (`WorldIntelligence.Classification`), backfilled
idempotently at boot and asserted in CI, not a spreadsheet.

**Verdict: 52 / 52 conditionally accepted — 0 fully accepted, 0 rejected.**

Gates that already pass in CI for all 52:

- schema validation and full reference solve (`WorldContent.Validate`, `WorldTests`);
- deterministic scoring and answer-leakage scan;
- synthetic-data declaration (originality of data);
- premium-language gate on titles and hooks (`WorldIntelligenceTests`);
- distinctness (unique codes, distinct scenarios, engine coverage ≥ 2 per solver).

Remediation required before any mapped item counts as **approved Year-1 coverage**:

| Gap | Applies to | Owner | Target |
|---|---|---|---|
| Three progressive authored hints per experience | all 52 | Content author + SME reviewer | Phase B |
| Pre/post-submission AI-coach boundary context | all 52 | Content author + AI safety review | Phase C (coach feature) |
| Review-by / expiry date + named SME review evidence | all 52 | Publisher | Phase B |
| Accessibility review evidence recorded per item | all 52 | Accessibility reviewer | Phase B |

The coverage report therefore counts mapped items as *backed* (a published, servable challenge
exists) but the runway alert stays raised until the plan is genuinely authored: **runway_days = 0
today**, because day 1 of the Year-1 plan is a planned slot. That number being low and honest is a
feature — coverage can never be claimed before it is real.

## 3. The governed Year-1 plan (420 slots)

`backend/tools/gen_year1_intelligence.py` deterministically generates
`backend/content/project-intelligence/year-1/`:

- `manifest.json` — totals, month file list, and the approved distribution tables;
- `01-january.json` … `12-december.json` — 365 scheduled slots `PI-Y1-D001–D365`, one per day,
  each carrying its full taxonomy (type, domain, difficulty band, duration band + minutes,
  interaction, lifecycle, sector), the monthly editorial arc as the month theme;
- `reserve.json` — 55 reserve slots `PI-Y1-R001–R055`;
- `YEAR1_CONTENT_INDEX.md` — human index.

Status of each slot:

- **`mapped` (52)** — points at an existing bank challenge (`source_challenge`), placed in the
  month whose editorial theme matches its domain, attributes derived from the live bank row.
- **`planned` (313 + 55)** — the authoring backlog: full taxonomy assigned, professional working
  title, no content yet. A planned slot is *not* coverage and is never served.

`WorldIntelligenceTests` fails the build when: any total or distribution table deviates from the
approved counts (365/55/420; monthly counts; all seven scheduled distribution tables **exactly**);
a code is missing, duplicated or out of range; a mapped slot disagrees with the C# classification
or the published bank; a working title repeats or uses banned language; a band is inconsistent
with its minutes. Regenerate with the script after changing the classification — never hand-edit
the JSON.

## 4. What Phase A ships (this change) — with verification

| Delivered | Verified by |
|---|---|
| Taxonomy vocabularies + labels (8/12/6/10/6/4/4) | `WorldIntelligenceTests.Vocabularies_have_the_approved_cardinalities` |
| Additive `pi_*` schema install (SQLite + MySQL dialect via `Db.Translate`) | boot on both providers; `Backfill_is_idempotent…` |
| Executable classification + idempotent backfill, house rows only | `Every_house_challenge_is_classified…`, `Operator_authored_rows_are_never_reclassified` |
| Premium-language gate | `Premium_language_gate…` (live bank scanned) |
| Year-1 plan, 420 slots, all distribution tables exact | 6 manifest tests (structure, monthly, 7 distributions, vocabulary, mapped-vs-bank) |
| Versioned learner API (home/categories/catalog/daily), read-only, allow-listed | live smoke (all four return taxonomy metadata only; no config_json selected anywhere) |
| World-admin coverage + runway alert (< 60 approved days ⇒ alert) | `Coverage_report_counts_honestly_and_raises_the_runway_alert` + live smoke (401 anon / 200 owner) |

## 5. Deferral register (Release 1 remainder)

Deferred items per the specification's own mechanism (owner, reason, risk, target):

| # | Deferred item | Reason | Risk | Owner | Target |
|---|---|---|---|---|---|
| D1 | World React participant + admin shells (`/world/app`, `/world-admin` SPA) | The spec makes React-shell completion an explicit prerequisite phase; current World learner surface is the server-rendered workspace, which already serves attempts safely | Learner UX stays classic until built | Frontend lead | Phase B |
| D2 | ~~Authoring the remaining planned experiences~~ **CLOSED (Phase B slices 4–13):** all 420 slots authored, validated and published — 365-day runway, reserve fully stocked | — | — | Content governance lead | Done |
| D3 | Remediation of the 52 legacy mapped items (hints, coach context, review-by dates) | The hints field and validator now exist (Phase B slice 1); backfilling 52 legacy configs is SME work | Legacy items stay "conditional" in coverage | SME reviewers | Phase B |
| D4 | Attempt-level PI features (bookmarks, mastery, recommendations, streaks) | Requires participant-account data model extensions; the single attempt pipeline must stay unforked | Learner home shows library + today only | Backend lead | Phase B |
| D5 | AI Coach (bounded hinting, leakage tests, fallback) | Depends on D2's per-item coach-boundary fields; deterministic hints must exist first | No coach at launch (spec allows: fully functional without) | AI safety engineer | Phase C |
| D6 | Shared-identity bridge to the platform student account | The one-login SSO bridge exists (`student_user_id`); the full canonical-profile prefill journey is a separate identity workstream | World accounts remain practice-identity-only (safe default) | Identity lead | Phase C |
| D7 | Localization (Arabic RTL interface test), analytics event dictionary, load tests at the 10k-DAU target | Sequenced after the React shells (D1) | — | Respective leads | Phase C |

**Explicit non-goals reconfirmed:** no second identity system, no client-side scoring, no
cross-domain cookies or tokens in URLs, no entitlement/CPD/certification effects from practice
activity, no team missions in Release 1.

## 6. Operating it

```bash
# regenerate the Year-1 plan after changing the classification or distributions
python3 backend/tools/gen_year1_intelligence.py

# run the gates
cd backend/tests/PCI.Backend.Tests && dotnet test --filter "FullyQualifiedName~WorldIntelligence"

# coverage + runway (world-admin token, read group)
GET /api/world-admin/intelligence/coverage
```

The runway alert (`runway_alert: true` below 60 consecutive approved days from day 1) is the
operational heartbeat for D2/D3: it clears only when real, approved content covers the calendar.
