# PCI AI Project Controls Simulation Lab — Phase 0: Platform Audit & Incremental Gap Matrix

_Audit-first deliverable mandated before any broad coding (spec §2, §40 Phase 0, acceptance criterion #1)._
_The Simulation Lab is an **incremental extension** of the existing PCI platform — not a rebuild, not a
parallel account/membership/AI/comms system. This document maps what already exists, the exact reuse
seams (with `file:line`), the gap per requirement, the minimum increment, and the migration / UI /
security / testing plans._

Method: seven parallel read-only code audits (student access & entitlement; AI-provider framework;
background jobs & real-time; comms/audit/analytics; DB schema & migrations; frontend student+admin;
Certuvo boundary), corroborated by direct inspection of `backend/Data/Migrate.cs`.

---

## 1. Non-negotiables carried into every increment (from the spec)

- Reuse existing student accounts, Admin Portal, auth/TOTP, RBAC, secure storage, Communication Centre,
  AI-provider framework, background jobs, audit, analytics. **No parallel systems.**
- **MySQL stays the production DB**; SQLite remains dev/test only. No destructive reseed; existing
  students/records untouched.
- Students enter the Lab through their **existing PCI account**; practice records stay **separate from
  formal exam records** (never touch `exam_attempts`, entitlements, or issued credentials).
- Critical calculations (CPM, EVM, EAC, Monte Carlo, scoring) are **deterministic + unit-tested**; AI is
  interpretation/coaching only and calls deterministic tools — it is never the numeric source of truth.
- AI must not invent scenario facts; must not leak answers in Assessment Mode; is evaluated before
  production use. Never send credentials / payment data / TOTP / recovery codes / gov IDs / other
  students' data to a provider. Use **synthetic scenario data** by default.
- Published scenario versions are **immutable**; a revision creates a new version.
- Do **not** duplicate Certuvo. Naming: **PCL-AI / PFL-AI / PDL-AI** only.

---

## 2. Existing component map & reuse seams (evidence-based)

### 2.1 Student access / entitlement — model on Certuvo, grant via one settlement seam
- `GET /api/me` aggregate: `backend/Endpoints/StudentExam.cs:33-160` (assembles user/profile/lifecycle/
  membership/credentials/cpd/…). "Entitlement" today = a settled `payments` row joined to
  `exam_entitlements` (`StudentExam.cs:22-25`) — **there is no polymorphic entitlement service**; each
  feature bolts on its own rule (Certuvo does).
- Central gating brain: `backend/Core/Lifecycle.cs` (`BuildLifecycle`, `BookingBlockers` 37-80).
- **The one idempotent grant seam:** `Settlement.EnsureDownstream` (`backend/Core/Provisioning.cs:106-180`)
  — every settlement path (Stripe webhook, admin mark-paid/waive, founding, honorary) funnels through it;
  `CertuvoLink.Provision` is called at 169-178. `Settlement.Reverse` (197-226) is the refund/revoke seam.
- Certuvo eligibility analog to copy: `CertuvoLink.Eligible` (`Provisioning.cs:251-265`), operator rule
  `certuvo_requires`; access endpoint shape `GET /api/me/certuvo/access` (`Certuvo.cs:58-63`);
  member-type resolver `DetectMemberType` (297-311: paid/waived/sponsored/complimentary/honorary/test).
- Certifications stored **clean** (`PCL-AI`/`PFL-AI`/`PDL-AI`, ids 1/2/3; `Certs.DefaultId=1`;
  `COALESCE(certification_id,1)`), `backend/Core/Certs.cs`, `backend/Data/MultiCert.cs:16-105`.
- Feature flags in `site_settings` via `Settings.Bool/Str/Num` (`Auth.cs:45-69`), `sp_`-prefixed for
  student-portal toggles.
- **Seam:** add `Core/SimLab.cs` with `Enabled(db)` (`sp_simlab_enabled`) + `Eligible(db,uid)`; grant/
  revoke in `EnsureDownstream`/`Reverse`; expose `GET /api/me/lab/access`. Keep Lab data **out of the fat
  `/api/me`** (separate endpoint, per Certuvo precedent).

### 2.2 AI-provider framework — extend `AiContent` + `ai_content_providers`, add tool-calling
- Best code shape: `backend/Core/AiContent.cs` — provider-agnostic `Generate(provider, model, system,
  prompt, maxTokens, temperature)` → uniform `Result(Ok,Text,Error,TokensIn,TokensOut)`; supports
  `openai` (gpt-4o-mini) + `anthropic`/`claude`; **keys env-only** (`OPENAI_API_KEY`/`ANTHROPIC_API_KEY`);
  never throws. Base URLs **hardcoded** (not mockable).
- DB-configured variant with **overridable endpoint + SSRF-guarded `Egress.CreateClient`**:
  `backend/Core/Translator.cs` (`translate_endpoint`; `custom` = any OpenAI-compatible URL).
- Per-use-case config schema already exists but is **unwired**: table `ai_content_providers`
  (`Migrate.cs:1238-1242`: `provider,use_case,system_prompt,model,max_tokens,temperature,key_env,
  daily_quota,require_citations,require_review,active`) — read at `ContentCentre.cs:481-486`, **no seed,
  no CRUD**; audit table `ai_content_generations`. Encryption helper `Security.EncryptSecret` (`enc:v1:`,
  AES-GCM) exists but isn't applied to AI keys.
- **Gap (exactly the spec's ask):** no tool/function calling anywhere (0 matches for `tool_calls|
  tool_choice|input_schema|response_format` in `backend/**`); no structured/JSON-schema output; no LLM
  evals/mock harness; `daily_quota` unenforced.
- **Seam:** new `Core/AiEngine.cs` styled on `AiContent` (provider switch + `Result`) **plus** a
  `tools`/`input_schema` param + the OpenAI/Anthropic tool-serialization + tool-execution round-trip;
  read persona/prompt/limits from an extended `ai_content_providers`-style table; overridable base URL
  (mock for evals); encrypt keys with `Security.EncryptSecret`, write-only `has_key`.

### 2.3 Background jobs & real-time — clone the `mkt_jobs` queue; add SignalR
- 8 `BackgroundService` workers, all one shape (`PeriodicTimer` + `WaitForNextTickAsync`, never throw,
  static `DrainOnce(db,limit)` shared by worker + admin "run now") — `Program.cs:50-59`.
- **Best template:** the generic job queue `mkt_jobs` (`Core/MarketingJobs.cs`): `Enqueue(...)` with
  UNIQUE `idempotency_key`; `DrainOnce` claims due rows, `switch(job_type)`, exponential backoff
  `min(3600, 2^attempt*30)` up to `max_attempts`; table DDL `Data/MarketingSchema.cs:240-247`. Instant
  feedback = synchronous `DrainOnce(db,3)` right after enqueue.
- **Real-time: none today.** No SignalR/WebSocket/EventSource/polling in `backend/` or `frontend/`.
  SignalR is **in-framework** (`secureexam/PCI.SecureExam.Server` uses `AddSignalR()`/`MapHub` with no
  extra NuGet). Client is greenfield (`@microsoft/signalr` + bridge pushes to `useQuery.refetch()`).
- **Seam:** `sim_jobs` table + `SimulationDispatcher : BackgroundService` (register `Program.cs:59`) for
  time-advancement / Monte Carlo / AI jobs / report gen, with a `progress` heartbeat column; add
  `AddSignalR()` (~`Program.cs:50`) + `MapHub<SimHub>("/hubs/sim")` (~918) + `IHubContext<SimHub>` into
  the dispatcher.
- **Blockers to design for:** SignalR WS handshake can't send an `Authorization` header → use the
  `access_token` query-string bridge against `login_tokens`; hand-rolled CORS (`Program.cs:167-177`) has
  no `Allow-Credentials`, CSP `connect-src` (`Program.cs:104`) must allow same-origin `wss:`; CPU-bound
  Monte Carlo must offload off the single drain loop + single `Db` singleton and chunk; `DrainOnce` job
  claim is non-atomic → **single-instance only** (fine on Render single service; documented scale limit).

### 2.4 Comms / Audit / Analytics — reuse three helpers, seed one catalogue
- **Comms:** `Comms.Fire(db, triggerCode, userId, …, dedupSuffix)` (`Core/Comms.cs:86-130`) → `comm_outbox`
  → `OutboxDispatcher` (email/WhatsApp/in-app). **No-op unless a `comm_triggers` row is seeded** → add a
  `G("Simulation …", …)` group to `Data/CommsSeed.cs` (`backend_wired=1`); reminder sweeps add a method
  to `CommsReminders.SweepOnce`. Owner alerts: `Notify.Alert(...)` (catalogue hardcoded in
  `Notifications.cs:21-34`).
- **Audit:** the `logFn` delegate `Action<long?,string,string?>` passed into every module's `Map(...)`
  (`Program.cs:914-984`) → `INSERT INTO audit_logs`. Namespace by **`sim_` action prefix** for a filtered
  view (marketing uses `LIKE 'mkt_%'`). No schema change.
- **Analytics:** `Analytics.Track(db, ctx, "sim_*", userId, value, currency, detail)` (`Core/Analytics.cs:123`)
  → `analytics_events` (free-text `event`, no whitelist). To surface on the dashboard, add one
  aggregation line to `AdminAnalytics.summary` (`AdminAnalytics.cs:29-54`, `reports`-gated).
- All three are **never-throw** side effects — follow that discipline for sim instrumentation.

### 2.5 Frontend (two SPAs from one `src/`) — the exact add-a-screen recipe
- Student: page in `frontend/src/pages/`, route in `App.tsx` (inside `<Layout>`), nav in
  `components/Layout.tsx` `NAV` **and** `TITLE_KEYS`, string in `i18n/catalog.ts` (`useT`).
- Admin (no i18n): page in `src/admin/pages/`, route in `AdminApp.tsx` wrapped in `<Perm section="…">`,
  nav in `AdminLayout.tsx` `NAV` (`group` string auto-creates a heading); **new `perm` keys must be
  registered in backend RBAC** or only owners see them. Fast path for CRUD-only: add a `CrudConfig` to
  `admin/crudConfigs.ts`.
- Data: custom `useQuery`/`useAdminQuery` (hand-rolled, not react-query) + `api`/`adminApi` (`get/post/
  patch/del`, bearer token, 401→logout). Mutations → `refetch()` / `runMutation`.
- **UI kit (`components/ui.tsx`):** `Card`, `Stat`, `Badge`, `StatusBadge`, `Spinner`, `Empty`,
  `ErrorNote`, `rowActivate`; SVG `Ring`/`CountUp`. **No charting library** — dashboards are `Stat` tiles
  + `<table class="data">` + hand-rolled SVG. Gantt/S-curve/tornado charts must be **hand-authored SVG or
  Canvas** (no proprietary UI copied), or a dependency added deliberately.

### 2.6 Certuvo boundary (do not duplicate)
- Certuvo = **externally-delivered MCQ question practice / exam prep**; student surface is an access card
  only (`frontend/src/pages/Certuvo.tsx`), external account provisioned via `CertuvoLink` +
  `certuvo_accounts`. A dormant in-portal MCQ engine exists server-side (`Certuvo.cs` overview/start/
  submit, `practice_attempts`, `sample_questions.is_practice=1`) but is intentionally hidden.
- **Boundary:** if the artifact is "a question with four options and one correct index" → Certuvo. If it
  is "a scenario the student manipulates and the system computes/visualizes" → Sim Lab. The Sim Lab is
  **in-portal**, uses **its own tables** (never `practice_attempts`/`sample_questions`), assesses
  **numerically/by tolerance** (not answer-index), and never touches `exam_attempts`/entitlements/
  credentials. Reuse the shared **auth/throttle/analytics/audit** helpers only.

---

## 3. Incremental Simulation Lab Gap Matrix

| Requirement | Existing component | Existing completion | Gap | Minimum increment | Rebuild? |
|---|---|---|---:|---|---|
| Student Lab access | Existing PCI account + Certuvo entitlement pattern | ~80% (pattern proven) | No Lab entitlement/flag | `Core/SimLab.Eligible` + `sp_simlab_enabled` + `GET /api/me/lab/access`; grant in `EnsureDownstream` | No |
| Certification mapping | `Certs`/`MultiCert` (PCL/PFL/PDL-AI, id 1/2/3) | 100% | Scenario→cert mapping table | `simulation_certification_mappings` reusing `COALESCE(cert,1)` | No |
| Exam-record separation | `exam_attempts`/entitlements firewall (Certuvo precedent) | 100% (pattern) | New sim tables must not touch them | Separate `simulation_*` tables; no FK into exam records | No |
| AI Coach / Analyst | `AiContent` + `ai_content_providers` (+ `Translator` mockable endpoint) | ~50% | No tool-calling, no structured output, no evals, unwired config | `Core/AiEngine.cs` (tool round-trip) + wire+seed provider config + eval harness | No |
| Deterministic calc engine (CPM/EVM/EAC/MC) | none | 0% | Whole engine | New `Core/Sim/*` deterministic services + unit/property tests | New code, not a rebuild |
| Background sim jobs | `mkt_jobs` queue + dispatcher pattern | ~90% (pattern) | No sim queue | `sim_jobs` + `SimulationDispatcher` (clone `MarketingJobs`) | No |
| Real-time updates | none (SignalR in-framework) | ~30% (server available) | No hub, no client | `AddSignalR()`+`MapHub<SimHub>`; add `@microsoft/signalr` client + refetch bridge | No |
| Notifications | `Comms.Fire` + `CommsSeed` + reminders | 95% | Trigger codes not seeded | Add `Simulation` trigger group to `CommsSeed`; reminder methods | No |
| Audit | `logFn` delegate + `audit_logs` | 100% | `sim_` action convention | Use `log(actor,"sim_…",detail)` | No |
| Analytics | `Analytics.Track` + `analytics_events` + reports | 90% | Dashboard aggregation lines | `Track("sim_*")` + add summary lines (reports-gated) | No |
| DB schema | `Migrate.cs` idempotent pattern (SQLite→MySQL) | 100% (pattern) | ~35 `simulation_*` tables | Add tables/cols via `db.Exec("CREATE TABLE IF NOT EXISTS …")`/`AddCol` | No |
| Student Lab UI | student SPA + UI kit (`Card`/`Stat`/`Ring`) | 70% (kit) | Lab pages + charts | New pages via the 3-edit recipe; hand-authored SVG charts | No |
| Admin Sim Centre | admin SPA + RBAC + CRUD factory | 70% | Scenario Builder + management | New admin section + `sim_lab` perm; CRUD for simple lists | No |
| Import/Export (XER/MSP XML/Excel) | `Csv`/PDF exporters; storage | 20% | Schedule import parsers | Phased: Excel/CSV/JSON first; XER/MSP XML later, report support level | No |
| LTI 1.3 / xAPI | none | 0% | Interop layer | Phase 6, optional/flagged; internal records stay source of truth | No |
| Accessibility (WCAG 2.2 AA) | existing a11y patterns + axe CI | 40% | Lab-specific (charts, timers, RTL) | Table alternatives for charts, accessible timers, axe on Lab pages | No |

---

## 4. Data-migration plan

- **How migrations work here (no numbered files):** the base schema is **single-sourced in SQLite dialect**
  (`backend/schema.sql`); `backend/schema.mysql.sql` is **generated** by `backend/tools/sqlite_to_mysql.py`;
  `Db.Translate()` (`Db.cs:114-153`) also converts SQLite-dialect DDL to MySQL at runtime. A "migration" is
  **idempotent DDL appended to `backend/Data/Migrate.cs`** (+ `CommsSeed.Ensure`/`MarketingSchema.Ensure`),
  re-run every boot; idempotency comes from `CREATE TABLE IF NOT EXISTS` and the `AddCol(table,col,ddl)`
  guard (`Migrate.cs:23-27`). There is no migration-history table.
- **The 5-step recipe for each `simulation_*` table** (must all be done or the parity test fails):
  1. Add `CREATE TABLE IF NOT EXISTS …` + `CREATE INDEX/UNIQUE INDEX IF NOT EXISTS ix_/ux_…` to
     `Migrate.cs` in **SQLite dialect** (`INTEGER PRIMARY KEY AUTOINCREMENT`, `TEXT`, `REAL`, `INTEGER` for
     bools, `TEXT DEFAULT (datetime('now'))` UTC-string timestamps, `*_json TEXT` blobs).
  2. Add the identical table to `backend/schema.sql` (the conformance test parses it).
  3. Regenerate `backend/schema.mysql.sql` — `python3 tools/sqlite_to_mysql.py` (run from `backend/`).
  4. **Hand-fix money columns to `DECIMAL(12,2)` in `schema.mysql.sql`** — the generator only maps
     `REAL→DOUBLE` and has no DECIMAL logic; the parity test checks column **names only, not types**, so a
     wrong money type is *not* auto-caught. This is a known drift in the existing generator.
  5. CI `backend-mysql` `migration_integrity_test.py` verifies idempotence + conformance + non-destruction
     + name-level MySQL/SQLite parity.
- **Actual house conventions (corrected against the codebase):** `id INTEGER PRIMARY KEY AUTOINCREMENT`;
  FKs are **advisory only** (inline `REFERENCES` stripped for MySQL) — use a plain `*_id INTEGER` + an
  `ix_` index; idempotency/uniqueness via `ux_` unique indexes (there is **no** `deleted_at` soft-delete,
  **no** `version`/optimistic-concurrency column anywhere — the platform uses `status`/`revoked_at` + unique
  keys instead); status columns are free `TEXT`/`VARCHAR(n)` with allowed values in a trailing comment (no
  `CHECK`); dates computed in **C#**, never SQL string math.
- **Spec-mandated additions the platform doesn't currently use:** §31 requires soft-delete, version fields,
  optimistic concurrency, and immutable published versions. The Sim Lab will **introduce** these on its own
  tables (`version` + a published-immutability rule enforced in the service layer; `deleted_at`; a `rev`
  guard on mutable scenario/attempt rows) — documented here as new Sim-Lab conventions, not existing ones.
- **Naming:** `simulation_*` prefix — audited free of collision with all ~230 existing tables. `project*`,
  `schedule*`, `cost*`, `risk*`, `change*`, `decision*`, `report*` are all **unused** as table prefixes;
  only the **bare** `events`/`event_registrations` names exist, so use `simulation_event*` (never bare
  `event*`). ~35 tables per spec §31.
- **No destructive changes:** additive only; no existing table altered destructively; no reseed of real
  data. Seed scenarios/labs idempotently (`WHERE NOT EXISTS`) as **Draft/Published catalogue rows**, never
  touching student records — mirroring `CommsSeed`/`MarketingSchema`.
- **Verification:** every migration runs in the existing `backend` (SQLite) + `backend-mysql` CI jobs; the
  migration-integrity suite proves idempotent double-boot, schema conformance, non-destruction, and
  name-level dual-provider parity (types not checked — hence the money hand-fix above).

## 5. UI-integration plan

- **Student:** new nav item **"Project Controls Practice Lab"** — 3 synchronized edits (`Layout.tsx` `NAV`
  + `TITLE_KEYS`, `App.tsx` route, `i18n/catalog.ts` key) + pages under `src/pages/` using the UI kit and
  `useQuery`. Access-gated by `GET /api/me/lab/access`.
- **Admin:** new section **"Simulation Lab Management"** under a new `sim_lab` RBAC perm (registered in
  `Core/Security.cs` `Rbac.Sections`/`RoleGrants`), grouped in `AdminLayout.tsx`; Scenario Builder as a
  custom page, simple lists via the CRUD factory.
- **Charts:** hand-authored SVG/Canvas for Gantt / S-curve / tornado / distributions (no proprietary UI
  copied, no chart lib assumed) — extend `components/ui.tsx` with reusable primitives; provide **data-table
  alternatives** for accessibility.
- **Colocated Vitest** for every new page (matches the 189-test suite discipline just merged).
- Student pages i18n via `useT` (EN + AR/RTL readiness); admin pages plain English.

## 6. Security plan

- **Isolation:** every `/api/me/lab/*` route resolves the user via `Core.Auth.UserFromReq`; a student may
  never read another student's attempt (ownership check in SQL). Team routes scoped to team membership.
  Admin routes `gate(req,"sim_lab",…)`; per-cert scoping via `AdminCtx.CanCert`/`CertFilterSql` where the
  Lab is certification-specific.
- **AI safety:** provider keys encrypted (`Security.EncryptSecret`) or env-only, **never** returned;
  tool allowlist per persona; strict input/output schemas; Assessment-mode answer-leak + prompt-injection
  guards; per-use-case cost/day limits (enforce `daily_quota`); log every AI interaction + tool call.
  **Never** send credentials/payment/TOTP/recovery/gov-ID/other-students' data to a provider; synthetic
  scenario data only. Grounding + citations required; missing data reported, never invented.
- **Impersonation** tokens stay read-only (existing `u.Impersonated` → 403 on writes).
- **Egress:** admin-supplied AI/import URLs validated by the SSRF `Egress` guard.
- Reuse the merged SEC-1/3/4 posture (CORS non-reflection, security headers, hostile-file robustness).

## 7. Testing plan

- **Unit + property** (new `PCI.Backend.Tests` xUnit cases): CPM (forward/backward pass, float, critical
  path), EVM/EAC formulas, progress methods, cash flow, risk scoring, **Monte Carlo reproducibility (same
  seed → same result)**, scoring, competency mastery, access rules, AI-mode restrictions; invariants (WBS
  roll-up = child total; published version immutable; one period closed once; one event processed once).
- **Integration** (extend `backend/tests/integration_test.py`, live-HTTP, **both SQLite + MySQL**, reusing
  `make_paid_user`/`jget`/`dbconn`): entitlement grant via settlement, attempt lifecycle, period
  advancement, event processing, AI tool call (against a **loopback mock** provider), scoring, versioning,
  cross-student isolation (IDOR), assessment-mode answer-refusal.
- **Frontend Vitest/RTL** colocated per page; **Playwright + axe** E2E for the student guided-lab and full
  scenario journeys (extends the existing `e2e` job).
- **AI evals** (new harness, mockable base URL): tool-selection, formula accuracy, groundedness, citation,
  hallucination resistance, assessment-leak, prompt-injection, cross-student isolation, cost/latency.
- **Gating:** each spec phase is complete only when its E2E acceptance tests pass on CI (SQLite+MySQL).

---

## 8. Phase ladder (spec §40) & sequencing

- **Phase 0 (this doc):** audit + Gap Matrix + plans. → new draft PR.
- **Phase 1:** `Core/SimLab` entitlement + `sp_simlab_enabled`; `simulation_*` foundation tables; scenario/
  lab catalogue; guided labs; attempts; competency records; student Lab page + admin scenario list; basic
  AI Coach (extend `AiContent`, no tools yet); `sim_` audit. Deterministic calc-service skeletons + tests.
- **Phase 2:** WBS/CBS; schedule workspace + **deterministic CPM**; cost-control grid; progress methods;
  **EVM/EAC** services; forecasting; dashboards (hand-authored SVG); 3 end-to-end scenarios.
- **Phase 3:** simulation clock/events/decisions/inbox/consequences/snapshots/replay + **SignalR** real-time.
- **Phase 4:** risk + **Monte Carlo** (`sim_jobs`), change/claims, cash flow, revenue, productivity;
  8 scenarios.
- **Phase 5:** tool-driven AI Analyst + stakeholder personas + report reviewer + scenario-gen assist +
  **AI evals** + cost/safety controls.
- **Phase 6:** cohorts/teams/instructor dashboards/institution reporting + optional **LTI 1.3** + **xAPI**.
- **Phase 7:** load / security / **WCAG 2.2 AA** / AI red-team / DR / monitoring / operator docs / pilot.

Each phase lands as small, individually CI-green commits (SQLite+MySQL), the same discipline that carried
the test-coverage programme (PR #73) to green.

---

## 9. Honest risks, blockers & external/operator dependencies

- **Real-time client is greenfield** (no SignalR client, no query cache) — the largest net-new frontend
  piece; the WS auth handshake needs the `access_token` bridge.
- **Monte Carlo compute** must not block the single dispatcher loop / single `Db` singleton — offload +
  chunk; job claim is non-atomic ⇒ **single-instance only** (documented; horizontal scale is out of scope).
- **AI tool-calling + evals are net-new** (nothing in the codebase does function calling) — highest-risk
  backend piece; gated behind evals before any production use.
- **Schedule import (XER / MS Project XML)** is complex — phased; Excel/CSV/JSON first, import support level
  reported honestly.
- **External/operator-pending (never faked):** live AI-provider keys (env), Render deployment/scale, Power
  BI embedding, LTI platform registration, off-box load/DR rehearsal, finance-professional review of any
  accounting content, WCAG manual audit.
- **AI-generated scenario content stays Draft** until multi-lens human review (controls/finance/education/
  AI-behaviour/copyright/privacy) — AI never publishes a scenario autonomously.

---

## 10. Status

**Phase 0 — audit complete.** The Simulation Lab is confirmed implementable as a pure incremental
extension: every reuse subsystem has a proven seam, no rebuild is required, and the net-new work
(deterministic PC calc engine, AI tool-calling + evals, SignalR real-time, scenario engine) is additive.
Proceeding to Phase 1 (Practice Lab foundation) as small, verified, CI-green increments.
