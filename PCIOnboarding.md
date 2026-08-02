# PCI Platform — Engineer Onboarding Guide

Welcome to the **PCI Platform**, the full software estate of the **Project Controls Institute**.
This guide takes you from a fresh clone to confidently shipping changes. It is written to be read
top-to-bottom on your first day, then used as a reference afterwards.

> **Read order:** this file → [`CLAUDE.md`](CLAUDE.md) (conventions & guardrails, also binding for
> human contributors) → the component guides it links to (`backend/RUN.md`, `frontend/README.md`,
> `secureexam/README-SECUREEXAM.md`, `DEPLOY.md`).

---

## Table of contents

1. [What you're working on](#1-what-youre-working-on)
2. [Architecture in five minutes](#2-architecture-in-five-minutes)
3. [Repository map](#3-repository-map)
4. [Day 1 — get it running locally](#4-day-1--get-it-running-locally)
5. [Core concepts you must understand](#5-core-concepts-you-must-understand)
6. [The frontend — four React bundles](#6-the-frontend--four-react-bundles)
7. [The secure-exam desktop client](#7-the-secure-exam-desktop-client)
8. [PCI World](#8-pci-world)
9. [Testing & CI](#9-testing--ci)
10. [Common workflows (recipes)](#10-common-workflows-recipes)
11. [Environment variables](#11-environment-variables)
12. [Deployment overview](#12-deployment-overview)
13. [Security guardrails](#13-security-guardrails)
14. [Gotchas & FAQ](#14-gotchas--faq)
15. [First-week checklist](#15-first-week-checklist)
16. [Where to look — quick reference](#16-where-to-look--quick-reference)
17. [Glossary](#17-glossary)

---

## 1. What you're working on

The PCI Platform is **one deployment that serves every surface from the same URL**:

| Surface | URL | What it is |
|---|---|---|
| Public website (~235 pages) | `/` | SEO-critical marketing/info site: server-rendered static HTML with DB-driven content injection |
| Student portal | `/student.html` (classic) and `/app/` (React) | Candidate journey: certifications, exams, credentials, CPD, billing, documents, events |
| Admin dashboard | `/admin.html` (classic) and `/admin/` (React) | Multi-section operator console with role-based access control |
| Exam preview | `/exam-ui.html` | In-browser exam runner preview |
| PCI World | `/world` (+ `/world-admin`) | Community/challenge platform — ships **switched off** (see §8) |

Everything is backed by **one ASP.NET Core 8 backend and one database**. Content edited in the
admin dashboard appears on the website; everything students do appears in the dashboard. There is
nothing separate to wire together.

A separate **Windows secure-exam desktop client** (`secureexam/`) delivers proctored exams with
kiosk lockdown. It talks to the *same* backend over a pinned API host using single-use launch
codes — it is a client of the platform, not a second system.

### The mental model

- **The backend is the platform.** ~160+ HTTP endpoints, all content injection, auth, payments,
  storage, email — a single ASP.NET Core 8 Minimal API in `backend/`.
- **The website is static HTML that the server can rewrite.** Pages are real `.html` files in
  `backend/wwwroot/`; admin-edited content is injected server-side on the way out (SEO-safe,
  works with JavaScript off).
- **The React apps are only the logged-in screens.** The marketing pages never became a SPA.
- **One database, two dialects.** SQL is written once in SQLite dialect; a runtime translator
  makes the same code run on MySQL/MariaDB in production.

---

## 2. Architecture in five minutes

### 2.1 The three stages of page delivery

1. **Stage 1 — static files.** `backend/wwwroot/` holds ~235 hand-authored HTML pages plus the
   classic panels (`student.html`, `admin.html`, `exam-ui.html`) and assets. A page with no DB
   overrides is served as-is and pays no rendering cost.
2. **Stage 2 — dynamic content injection.** Before static files in the middleware pipeline, a
   page GET whose slug has DB overrides (`page_blocks` / `site_content`, certification catalogue,
   list sections, price tags) is rendered server-side with those values injected, then cached per
   injector version. This is how "the website is editable from the admin dashboard without a
   redeploy" works. See `backend/Core/PageContent.cs` and friends.
3. **Stage 3 — React SPA fallback.** Terminal middleware returns the `/app` or `/admin` (or
   world) shell for extension-less client-side routes. Real assets and `/api/*` never reach it.

### 2.2 The request pipeline (order matters)

Middleware in `backend/Program.cs` is registered outermost-first so **every** response carries the
right headers:

1. **Security headers + CSP** — `X-Content-Type-Options`, `Referrer-Policy`, `X-Frame-Options`,
   `Cross-Origin-Opener-Policy`, a scoped `Content-Security-Policy`, HSTS when the proxy reports
   `X-Forwarded-Proto: https`.
2. **CORS** — reflects `ALLOWED_ORIGIN` only (wildcard rejected in production).
3. **Rate limiting** — in-memory fixed window (10 req / 60 s) on brute-forceable POST paths
   (`/api/login`, `/api/admin/auth/login`, `/api/forgot-password`, `/api/validate-code`,
   `/api/set-password`, `/api/exam/authorize`), keyed on the first `X-Forwarded-For` hop.
4. **Boot-time config validation** — in `Production` the app **exits 78** on unsafe config (§12.2).
5. **Maintenance mode** — a 503 holding page for public pages while `/api/*` and admin stay up.
6. **Dynamic content injection** (Stage 2 above).
7. **Static files** (`UseDefaultFiles` + `UseStaticFiles`).
8. **React SPA fallback** (Stage 3 above).

### 2.3 The pieces

```
 Browser ──────────────┐
 (website /, /app/,    │        ┌─────────────────────────────┐
  /admin/, /world)     ├──────► │  backend/  ASP.NET Core 8   │      ┌──────────────┐
                       │        │  Minimal API                │◄────►│  SQLite (dev) │
 Windows kiosk client ─┤        │  · ~160+ endpoints          │      │  MySQL (prod) │
 (secureexam/, pinned  │        │  · content injection        │      └──────────────┘
  host, launch codes)  │        │  · auth / RBAC / payments   │      ┌──────────────┐
                       │        │  · storage / email / audit  │◄────►│ local disk or │
 Stripe webhooks ──────┘        └─────────────────────────────┘      │ S3 (evidence) │
                                                                     └──────────────┘
```

---

## 3. Repository map

```
PCI/
├── CLAUDE.md            Conventions & guardrails — binding; read it early
├── AGENTS.md            Cloud-agent environment notes (also useful gotchas for humans)
├── PCIOnboarding.md     This file
├── DEPLOY.md            Deployment guide (Render + generic Docker)
├── Dockerfile           Multi-stage build: React apps → .NET publish → runtime image
├── render.yaml          Render Blueprint (one Docker web service + persistent disk at /data)
├── holding/             Static "coming soon" holding page (separate static deploy)
├── PCIWorld/            Dedicated PCI-World-only deployment root (own Dockerfile, PCIWORLD_ONLY)
├── docs/                ⚠ HISTORICAL handoff archive — background only, NOT the current file map
│
├── backend/             ★ The whole platform's server (ASP.NET Core 8 Minimal API)
│   ├── Program.cs       Boot, middleware pipeline, ~17 inline endpoints, module wiring
│   ├── Core/            ~139 cross-cutting services (auth, RBAC, storage, injection, mailer,
│   │                    payments helpers, SimLab, World, marketing, moderation, …)
│   ├── Endpoints/       ~71 feature endpoint modules (student exam, admin mgmt, payments,
│   │                    public, forum, careers, community, world, …)
│   ├── Data/            Db.cs (dual-provider data layer), Migrate.cs, schema modules, seeds
│   ├── wwwroot/         The served static site: ~235 .html pages + classic panels + assets
│   ├── emails/          Transactional HTML email templates
│   ├── tests/           Python logic/integration/sweep suites + PCI.Backend.Tests (xUnit)
│   ├── tools/           sqlite_to_mysql.py (regenerates schema.mysql.sql)
│   ├── schema.sql       SQLite schema — SOURCE OF TRUTH (75 tables and counting)
│   ├── schema.mysql.sql Generated from schema.sql for the MySQL provider — never hand-edit
│   ├── smoke-test.sh    Live HTTP smoke suite used by CI
│   ├── RUN.md           Authoritative build/run/verify guide
│   └── MYSQL.md         Dual-provider details
│
├── frontend/            ★ React 18 + TypeScript + Vite — the interactive app screens
│   └── src/
│       ├── pages/       Student portal screens        → built to /app/
│       ├── admin/       Admin console (CrudSection, crudConfigs.ts) → /admin/
│       ├── world/       PCI World learner app          → /world/
│       ├── worldadmin/  PCI World admin                → /world-admin/
│       ├── api/         Typed API client (makeClient) + shared types
│       └── components/  Shared components
│
├── secureexam/          ★ .NET 8 Windows WPF secure-exam client + shared Core + companion server
│   ├── PCI.SecureExam.Core     Pure net8.0, zero dependencies — the security-critical logic
│   ├── PCI.SecureExam.App      net8.0-windows WPF kiosk client (OpenCV + NAudio)
│   ├── PCI.SecureExam.Server   Optional reference ASP.NET server (controllers + SignalR)
│   ├── PCI.SecureExam.Tests    xUnit tests (cross-platform for Core)
│   └── PCI.SecureExam.Core.RunnableChecks  Package-free host-pinning attack checks
│
└── .github/workflows/build.yml   CI matrix (see §9.3)
```

**Three rules about the map:**

- `docs/` is a **historical archive**. Its manifests reference zip bundles and earlier
  explorations that are *not* the working source. The live code is `backend/`, `frontend/`,
  `secureexam/`. Use `docs/` for background, never as the current file map.
- `backend/wwwroot/app/`, `…/admin/`, and the world equivalents are **git-ignored build
  artifacts** assembled from `frontend/` in Docker. Never edit them by hand.
- Counts in docs drift as the platform grows (CLAUDE.md may say "~216 pages / 53 tables"; the
  live numbers are already higher). When precision matters, count the real files.

---

## 4. Day 1 — get it running locally

### 4.1 Prerequisites

| Tool | Version | Used for |
|---|---|---|
| .NET SDK | 8.x | backend (and secureexam Core on Linux) |
| Node.js + npm | 18+ (22 in CI images) | frontend |
| Python 3 | 3.10+ | backend test suites |
| Docker | optional | full-image builds, MySQL parity runs |
| Windows | only for `secureexam` WPF app | the kiosk client does not build on Linux |

### 4.2 Boot the backend

```bash
cd backend
cp .env.example .env    # optional — every value has a working default
dotnet run              # → http://localhost:8080
```

First boot creates and migrates `./pci.db` (SQLite) — no separate database service needed.
Verify: `curl http://localhost:8080/api/health`.

You now have, from that one process:

- the full public website at `http://localhost:8080/`
- the classic student portal at `/student.html` and admin at `/admin.html`
- the exam preview at `/exam-ui.html`
- the JSON API under `/api/*`

**First admin sign-in:** `owner@pci.local` / `changeme-owner`. The account carries a
must-change-password flag — change it immediately (Settings → Security).

**Auth routes are split** (a classic first-day trip-up):

- Admin/owner login: `POST /api/admin/auth/login`
- Student login: `POST /api/login` · student signup: `POST /api/register`

**Optional demo data:** `SEED_DEMO_EXAM=true` seeds the demo question bank;
`DEMO_STUDENT_PASSWORD=...` creates `student@pci.local`. This does **not** grant a ready-to-sit
exam entitlement — sitting an exam still requires payment/eligibility/scheduling (payments
answer 503 without Stripe keys). For an end-to-end "new user" run, register a fresh candidate
through `/app/register`.

### 4.3 Run the React apps (dev mode)

```bash
cd frontend
npm install
npm run dev            # student portal → http://localhost:5173/app/  (proxies /api → :8080)
npm run dev:admin      # admin console  → :5174
npm run dev:world      # PCI World learner app
npm run typecheck      # tsc --noEmit — must pass before you push
```

Keep the backend running in another terminal so `/api` calls resolve. In dev, use the Vite
servers — don't build into `wwwroot` by hand.

### 4.4 Prove it end-to-end (15 minutes)

1. Log in to `/admin.html` as the owner, change the password.
2. Edit a page headline in the admin content section; reload the public page — the change is
   there, server-rendered (view source to confirm; no JS needed).
3. Register a student via `/app/register`, log in to `/app/`, click around the portal.
4. Run one test suite: `cd backend && python3 tests/lifecycle_test.py` — every line should
   print `PASS`/`✓`.

If all four work, your environment is good.

---

## 5. Core concepts you must understand

These five concepts explain most of the codebase. Internalise them before writing backend code.

### 5.1 Minimal API + endpoint modules (no MVC, no framework ceremony)

The backend is ASP.NET Core **Minimal API**. `Program.cs` wires the middleware and ~17 inline
endpoints, then calls each feature module's `Map(...)`:

- **`Endpoints/*.cs`** — each is a `static class` with a `Map(app, db, logFn, …)` method.
  Handlers do **inline validation** (RBAC, ownership, timing, type/size, entitlement) in the
  handler body — match the surrounding guard style, don't introduce a validation framework.
- Every response is JSON via `Results.Json(...)`; errors are `{ error, … }` with the right
  status code: 401 unauthorised, 403 forbidden/`owner_only`, 400 validation, 404 not-found,
  503 disabled feature.
- Privileged actions are logged to `audit_logs` via the `logFn`/`Log` helper.
- **`Core/*.cs`** — shared services used across modules (auth, storage, mailers, injectors,
  domain logic). If two endpoint modules need it, it lives in `Core/`.

### 5.2 The dual-provider data layer (`Data/Db.cs`) — the most important file in the repo

**All DB access goes through the shared `Db` singleton. Never open your own connection.**

- SQL is written in **SQLite dialect — the source of truth**. When `DB_PROVIDER=mysql`,
  `Db.Translate` rewrites it to MySQL/MariaDB at runtime (datetime math, upserts,
  `last_insert_rowid()`/`changes()`, `julianday`, `strftime`, partial unique indexes). App code
  stays provider-agnostic — you write SQLite, production runs MySQL, same C#.
- **Datetimes are strings** in `YYYY-MM-DD HH:MM:SS` (UTC) on both providers. Compare instants
  via the `H` helpers (`H.JsMillis`, `H.IsPast`, `H.After`) — **never lexically**: `' '` (0x20)
  sorts before `'T'` (0x54), so comparing against an ISO-`T` string causes off-by-a-day bugs.
- Parameters are positional `?`, rewritten to `@p0, @p1, …` internally. **Always parameterise**
  — never string-concatenate user input into SQL.
- API surface: `Query`/`QueryOne` (rows as case-insensitive `Dictionary<string,object?>`),
  `Scalar<T>`, `Execute`, `ExecuteReturningId`, `ExecuteWithChanges` (atomic id+changes; used
  by the Stripe webhook idempotency gate), `Transaction(Action)`, `Columns(table)`.
- Read column values through the `H` coercion helpers (`H.L`/`H.D`/`H.Str`/`H.B`) — they hide
  each provider's object typing.

### 5.3 The content system (why the website is "editable")

- `PageContent.SeedFromFiles` captures each page's headline as an editable block on first boot,
  so every page is admin-editable out of the box.
- On a page GET with DB overrides, HTML is rendered server-side with the values injected, then
  run through `CertCatalogue`, `ListSections` (nav/FAQs/BoK/governance/resources/news), and
  `PriceTags`. Pages with no overrides fall straight through to static files and pay nothing.
- Each injector caches per its own version and calls `Bump()` when its settings change — if you
  add an injector-affecting setting, remember the `Bump()`.
- `assets/cms-loader.js` is a client-side *fallback* (hydrates `[data-cms]`, the announcement
  banner, newsletter form). With no API configured the site stays fully static.

### 5.4 Auth & RBAC — two token systems, four section groups

- **Two independent bearer-token session systems**, different storage keys, never shared:
  - Students: `POST /api/login` → `login_tokens`, 30-day.
  - Admins: `POST /api/admin/auth/login` → `admin_sessions`, 12-hour.
- Tokens are stored **hashed** (`Security.Sha`); logout deletes the row. Passwords are BCrypt;
  `Security.VerifyPassword` returns `false` (never throws) on a malformed stored hash, so login
  endpoints answer 401, not 500.
- **RBAC** (`Core/Security.cs` → `Rbac`): four section groups (platform / website / student /
  exam) map to granular permissions. Roles (`owner`, `website_manager`, `student_manager`,
  `exam_manager`, `viewer`, `custom`) grant permission sets, plus per-admin extras. `owner`
  gets everything.
- Endpoint gating: `GateFn(req, section, ok)` (403 unless owner or holder of `section`) for
  feature modules; `OwnerGate` for owner-only routes. Settings PATCH is **deny-by-default** by
  key prefix: `web_` → `set_web`, `sp_` → `set_sp`, `exam_` → `set_exam`, anything else needs
  owner-level `settings`.

### 5.5 The admin CRUD factory — one line per collection

Generic content collections (FAQs, BoK, questions, resources, news, nav, media, pricing rules,
…) are registered by one `Crud(name, cols, order, section)` call in `Endpoints/AdminMgmt.cs`,
which exposes uniform `GET/POST/PATCH/DELETE /api/admin/{name}`. The React admin drives them
all through a single `CrudSection` component configured in `frontend/src/admin/crudConfigs.ts`.

**Adding a whole admin-managed collection is one backend `Crud(...)` line + one frontend config
entry** — no new endpoint, no new component. Check whether the factory covers your case before
writing bespoke CRUD.

### 5.6 Migrations — idempotent, run on every boot

`Data/Migrate.cs` runs on every boot: loads `schema.sql` (or `schema.mysql.sql`), then applies
`CREATE TABLE IF NOT EXISTS` / guarded `AddCol` upgrades, `CREATE … INDEX IF NOT EXISTS`, and
first-run seeds (bootstrap owner, demo student, content). Safe to re-run; never overwrites
edited content. The schema-change recipe is in §10.3 — all three steps are mandatory.

---

## 6. The frontend — four React bundles

React 18 + TypeScript + Vite. **Four independent apps** share one project, the shared
components, and the typed API client. Building separately keeps admin code out of the student
bundle (and vice versa):

| App | Entry | Vite config | Served at |
|---|---|---|---|
| Student portal | `src/main.tsx` | `vite.config.ts` | `/app/` |
| Admin console | `src/admin/main.tsx` | `vite.admin.config.ts` | `/admin/` |
| PCI World | `src/world/` | `vite.world.config.ts` | `/world/` |
| PCI World admin | `src/worldadmin/` | `vite.worldadmin.config.ts` | `/world-admin/` |

Rules of the road:

- **API client:** `src/api/client.ts` exposes `makeClient(tokenKey)` — a thin typed `fetch`
  wrapper with bearer token in `sessionStorage` (cleared on tab close — deliberate for shared
  machines) and a central 401 handler that clears the token and redirects to login. Student and
  admin use **separate** clients/keys (`pci.session.token` vs `pci.admin.token`). Reuse it —
  don't hand-roll fetches.
- Auth mirrors the classic portals exactly (same tokens, same endpoints).
- `tsconfig.json` is strict (`strict`, `noUnusedLocals`, `noUnusedParameters`,
  `noFallthroughCasesInSwitch`). `npm run typecheck` must pass.
- Component tests are **Vitest + Testing Library** (`*.test.tsx` beside each page); run with
  `npm test`. Playwright e2e exists via `npm run e2e`. Lint with `npm run lint`.
- The React apps are the **logged-in screens only**. The SEO-critical marketing pages stay on
  the server-rendered content system; the classic `student.html` / `admin.html` panels remain
  reachable.

---

## 7. The secure-exam desktop client

A .NET 8 solution (`secureexam/PCI.SecureExam.sln`, SDK pinned via `global.json`). Know the
security model even if you never touch this code, because backend exam endpoints enforce it:

- **Host pinning:** the client is pinned to a dot-anchored HTTPS allowlist
  (`projectcontrolsinstitute.org`, `localhost`). A malicious `api=` in the
  `pciexam://` launch URI is ignored; the client refuses to start against an untrusted host.
- **Single-use launch codes**, not bearer tokens: the portal hands the client a short-lived
  code redeemed against the pinned host.
- **The server owns the clock and scoring.** The heartbeat returns canonical
  `RemainingSeconds`; ForceSubmit is server-driven; scoring is never client-side.
- Kiosk lockdown is user-space and **degrades honestly** (it cannot block Ctrl+Alt+Del — this
  is documented behaviour, not a bug).
- A **held** (integrity-review) result never shows a score, pass/fail, or credential.

Project layout: `Core` (pure net8.0, zero package references — builds/tests on Linux; the
security-critical logic lives here), `App` (Windows-only WPF kiosk client), `Server` (optional
reference proctoring service — in production the main `backend/` is the system of record),
`Tests` (xUnit against Core, cross-platform), `Core.RunnableChecks` (package-free offline
host-pinning attack harness).

Build/run (Windows for the WPF app; Core anywhere):

```powershell
cd secureexam
./build.ps1              # restore → build -c Release → dotnet test (Core)
./build.ps1 -SelfTest    # machine readiness check; exit 0 = ready
./build.ps1 -Run         # run client against demo launch code PCIDEMO12345 (start Server first)
./build.ps1 -Publish     # → self-contained single-file PCISecureExam.exe
```

There is **no pre-built `.exe` in the repo** — publish on Windows. Secrets go in gitignored
`appsettings.Local.json`; any config key is overridable via `PCI_`-prefixed env vars.

---

## 8. PCI World

PCI World is the community/challenge platform (rooms, forum, careers, contributor desk,
passport, intelligence content). Two things to know:

1. **It ships switched off.** Each feature sits behind a `site_settings` flag that seeds `'0'`,
   so a deployment is never a launch. Fresh-install routes returning 404 is the design working,
   not a fault. Features are switched on from the admin console → **PCI World → Launch** (owner
   only; `backend/Endpoints/WorldLaunch.cs`). Three features refuse to enable until a
   prerequisite is recorded (a moderation provider, a candidate privacy notice, contributor
   terms) — enforced in the endpoint, not just the UI.
2. **It has a dedicated deployment root.** `PCIWorld/` at the repo root builds an image with
   `PCIWORLD_ONLY=true` baked in: it serves `/world` and `/world-admin` and nothing else — the
   Institute website and portals are unreachable on that deployment. See `PCIWorld/README.md`
   for exact Render settings; MySQL 8 is the production destination there.

The world code spans `backend/Core/World*.cs`, `backend/Endpoints/World*.cs`,
`backend/Data/World*.cs`, and `frontend/src/world` + `src/worldadmin`.

---

## 9. Testing & CI

### 9.1 Backend suites (Python — real SQLite, replicate production SQL)

From `backend/`. Suites pass when every assertion prints `PASS`/`✓`; any bare `FAIL`/`✗` fails
the run. There are ~45 suites in `backend/tests/`; the core set:

```bash
python3 tests/lifecycle_test.py     # result lifecycle, consents, auto-hold, entitlement, webhook idempotency
python3 tests/release_test.py       # admin release/invalidate/reinstate, pass mark, expiry-aware verify
python3 tests/casework_test.py      # appeals, accommodations, attachments, CPD, certificate
python3 tests/settings_test.py      # settings RBAC + readiness gate
python3 tests/publication_test.py   # publication policy, proctoring audit-only, technical blocks
python3 tests/storage_test.py       # storage abstraction: MIME/size/sniff/traversal/retention
python3 tests/integration_test.py   # adversarial end-to-end over live HTTP (SQLite or MySQL)
python3 tests/sweep_500_test.py     # every route × anon/student/owner — asserts 0 × 500
./smoke-test.sh                     # live HTTP smoke suite (boot the backend first)
```

Feature areas have their own suites (`forum_test.py`, `careers_test.py`, `community_*.py`,
`world_*.py`, `payments_replay_test.py`, …) — run the ones neighbouring your change.
`backend/tests/PCI.Backend.Tests/` holds xUnit unit tests (`dotnet test`).

**MySQL parity:**
`TEST_DB_PROVIDER=mysql MYSQL_HOST=… MYSQL_USER=… MYSQL_PASSWORD=… MYSQL_DATABASE=… python3 tests/integration_test.py`.

### 9.2 Frontend

```bash
npm run typecheck   # must pass
npm test            # Vitest component suites
npm run lint        # ESLint
npm run build       # typecheck + all four bundles
npm run e2e         # Playwright (needs a running stack)
```

### 9.3 CI (`.github/workflows/build.yml`) — runs on push to `main` and every PR

| Job | What it does |
|---|---|
| `backend` | build → Python logic suites → JS-syntax gate on app shells → boot backend → `smoke-test.sh` → integration suite → 500-sweep → `system-check` probe |
| `backend-mysql` | adversarial integration suite against MariaDB 10.11 (MySQL parity gate) |
| `backend-unit` / `backend-unit-mysql` | xUnit unit tests on SQLite and MySQL |
| `frontend` | `npm ci` → typecheck → build; fails if an app produced no assets |
| `secureexam-windows` | solution build + `dotnet test` on `windows-latest` |
| `secureexam-core-linux` | Core + tests on `ubuntu-latest` |
| `docker-image` | full image build |
| `static-quality` | static-site quality checks |

**Before pushing, run what CI runs for what you touched:**

- Backend → relevant Python suites + a boot + `smoke-test.sh`.
- Frontend → `npm run typecheck && npm run build` (plus `npm test` for touched components).
- Secureexam → `dotnet build` + `dotnet test`.

---

## 10. Common workflows (recipes)

### 10.1 Add a backend endpoint

1. Find the right module in `backend/Endpoints/` (named by area) — or create a new
   `static class` with a `Map(...)` method matching its neighbours and wire it in `Program.cs`.
2. Gate it: `GateFn(req, section, ok)` for admin features, `OwnerGate` for owner-only, student
   token checks for portal routes.
3. Validate inline; use the shared `db` with parameterised SQLite-dialect SQL; coerce reads via
   `H.*`; respond with `Results.Json(...)`; log privileged actions to `audit_logs`.
4. Add/extend a Python suite covering it, and check `sweep_500_test.py` still reports zero 500s.

### 10.2 Add an admin-managed content collection

1. One `Crud(name, cols, order, section)` line in `Endpoints/AdminMgmt.cs`.
2. One entry in `frontend/src/admin/crudConfigs.ts`.
3. If the data feeds a public page, wire the injector (`ListSections` et al.) and remember the
   cache `Bump()`.

### 10.3 Change the database schema (all steps mandatory)

1. Edit `backend/schema.sql` (source of truth).
2. Add the matching **idempotent** upgrade in `Data/Migrate.cs` (`CREATE TABLE IF NOT EXISTS` /
   guarded `AddCol`) so existing databases converge.
3. Regenerate the MySQL schema: `python3 tools/sqlite_to_mysql.py` (writes `schema.mysql.sql` —
   never hand-edit it).
4. Run the Python logic suites, and ideally the MySQL integration run.

### 10.4 Edit website content or pages

- **Content change** (headline, list, price, FAQ): do it in the admin console — that's the
  product working as designed; no code needed.
- **Structural page change**: edit the `.html` in `backend/wwwroot/`. Keep pages self-contained
  and static-safe (they must render fully with the backend down and JS off).
- New pages become admin-editable automatically via `PageContent.SeedFromFiles` on next boot.

### 10.5 Add a student-portal or admin screen (React)

1. Add the page in `frontend/src/pages/` (or `src/admin/pages/`) with a `*.test.tsx` beside it,
   following a neighbouring page's shape.
2. Use the shared client (`makeClient`) and types (`api/types.ts` / `admin/api.ts`).
3. `npm run typecheck && npm test && npm run build` before pushing.

### 10.6 Add a setting

Settings keys are RBAC-gated **by prefix** (§5.4): pick `web_` / `sp_` / `exam_` so the right
role can edit it, or accept that it becomes owner-only. If it affects rendered pages, `Bump()`
the relevant injector cache. Secret-ish keys must be redacted in `/api/content` and
`system-check` (§13).

---

## 11. Environment variables

Everything has a working default for local dev; you can boot with no `.env` at all. The ones
that matter (full detail: `backend/.env.example`, `backend/RUN.md`, `DEPLOY.md`):

| Var | When | Notes |
|---|---|---|
| `DATABASE_FILE` | always | SQLite path; **persistent** in prod (never `/tmp`). Default `./pci.db` |
| `DB_PROVIDER` | optional | `sqlite` (default) or `mysql`/`mariadb` (+ `MYSQL_*` or `MYSQL_CONNECTION_STRING`) |
| `PORT` | optional | default 8080 |
| `ASPNETCORE_ENVIRONMENT` | prod | `Production` turns on the boot config validator |
| `APP_BASE_URL` / `SITE_BASE_URL` | prod | public HTTPS URL |
| `ALLOWED_ORIGIN` | prod | exact origin, no wildcard |
| `ADMIN_OWNER_EMAIL` / `ADMIN_OWNER_PASSWORD` | first boot | bootstrap owner |
| `STRIPE_SECRET_KEY` / `STRIPE_WEBHOOK_SECRET` | payments | webhook secret required once the key is set |
| `SMTP_HOST` (+ `SMTP_PORT`/`SMTP_USER`/`SMTP_PASS`/`MAIL_FROM`) | email | without it, emails log to console + email log |
| `STORAGE_PROVIDER` / `STORAGE_ROOT` / `S3_*` | optional | `local` (default) or S3-compatible (needs `S3_BUCKET`) |
| `SEED_DEMO_EXAM` / `DEMO_STUDENT_PASSWORD` | dev | demo question bank / demo student |
| `CSP_REPORT_ONLY` | optional | `true` runs CSP report-only |
| `ENABLE_LEGACY_ADMIN_TOKEN` | never in prod | app refuses to boot if on in prod |
| `ALLOW_INSECURE_PRODUCTION` | escape hatch | boot despite config errors — emergencies only |

**Graceful degradation is intended behaviour, not bugs:** no Stripe key → payment endpoints
answer **503**, everything else works. No `SMTP_HOST` → emails print to the console and land in
the admin email log. Backend unreachable → the public website stays fully static.

---

## 12. Deployment overview

### 12.1 The shape

One Docker image serves all surfaces. The `Dockerfile` builds the React bundles, publishes the
.NET app, and copies the bundles into `wwwroot/app` + `wwwroot/admin` (+ world). `/data` is the
single persistent mount (SQLite DB + evidence/attachments).

- **Render** is the recommended path via `render.yaml` — Starter plan or above (the free tier
  has **no disk** and would wipe the DB on every deploy).
- Any Docker host works with a TLS-terminating reverse proxy forwarding `X-Forwarded-Proto`.
- `PCIWorld/Dockerfile` builds the separate world-only service (§8).
- `holding/` is a static coming-soon page for pre-launch hosting.

Full instructions: `DEPLOY.md` and `backend/RUN.md` §8.

### 12.2 The boot validator (don't fight it)

In `Production` the app logs every config issue and **refuses to boot — exit code 78** — on
hard blockers: `APP_BASE_URL` must be public https; `ALLOWED_ORIGIN` must be explicit;
`DATABASE_FILE` must be persistent (not `/tmp`); `STRIPE_WEBHOOK_SECRET` required once
`STRIPE_SECRET_KEY` is set; legacy admin token must be off. `ALLOW_INSECURE_PRODUCTION=true`
overrides — emergencies only. Owner-only readiness probe: `GET /api/admin/system-check`.

A deploy "failing at the health check with exit 78" is the validator telling you exactly what
to fix — the reasons are in the boot log. `DEPLOY.md` maps exit codes and log prefixes.

---

## 13. Security guardrails

Non-negotiables, enforced by review and by the test suites:

- **Parameterise all SQL.** Never string-concatenate user input into a query.
- **Hash tokens before storage** (`Security.Sha`); BCrypt for passwords.
- **Sanitise admin-authored HTML** with `HtmlSanitize` before storing/serving.
- **Don't leak secrets:** `/api/content` and `system-check` redact secret/SMTP/result-policy
  keys — keep new secret-ish settings on the redaction list.
- **Uploads go through `Core/Storage`** (MIME sniff, size cap, path-traversal guard,
  retention). The request body is capped at 6 MB in Kestrel.
- **Keep the CSP allowlist tight** — only add an origin the site genuinely uses.
- **Gate every new endpoint** (`GateFn`/`OwnerGate`/student token) and **audit-log privileged
  actions**.
- Exam integrity rules (server-owned clock/scoring, held results show nothing) are product
  guarantees — never weaken them for convenience.

---

## 14. Gotchas & FAQ

**The backend boots as `Production` by default.** With no `ASPNETCORE_ENVIRONMENT` set, hosting
defaults to Production — the default localhost/SQLite config still boots fine, but if you add
prod-like env vars and boot dies with exit 78, that's the validator (§12.2). Set
`ASPNETCORE_ENVIRONMENT=Development` for local work or relax the config.

**"My route 404s on a fresh install"** — if it's a PCI World feature, that's the launch gating
working (§8). Enable it from the launch board.

**"Payments/email return 503 / print to console"** — graceful degradation (§11), not a bug.

**Date comparisons look wrong by a day** — you compared datetime strings lexically against an
ISO-`T` format. Use the `H` date helpers (§5.2).

**Don't edit `backend/wwwroot/app/` or `…/admin/`** — git-ignored build artifacts, assembled
from `frontend/` in Docker. Your edits will vanish.

**Don't hand-edit `schema.mysql.sql`** — it's generated by `tools/sqlite_to_mysql.py`.

**`docs/` looks authoritative but isn't** — historical archive; the live map is §3.

**The secureexam WPF app won't build on Linux** — by design; only `PCI.SecureExam.Core` and its
tests are cross-platform. CI splits them into separate jobs.

**Kiosk lockdown "doesn't block Ctrl+Alt+Del"** — documented honest degradation, not a defect.

**Where do sessions live in the browser?** `sessionStorage` (cleared on tab close) —
deliberate for shared machines. Don't move tokens to `localStorage`.

**MySQL vs SQLite behaviour differs?** Suspect `Db.Translate` coverage — run the MySQL parity
suite (§9.1) and check `backend/MYSQL.md` before assuming app-level fault.

---

## 15. First-week checklist

**Day 1 — run it**
- [ ] Read this file and `CLAUDE.md`.
- [ ] Boot the backend; hit `/api/health`; sign in as owner and change the password.
- [ ] Run the student and admin Vite dev servers; log into both.
- [ ] Complete the end-to-end proof in §4.4.

**Day 2 — read the spine**
- [ ] Read `backend/Program.cs` top to bottom (middleware order and inline endpoints).
- [ ] Read `backend/Data/Db.cs` (`Translate`, `Bind`, the API surface) and skim `Data/Migrate.cs`.
- [ ] Skim `Core/Security.cs` (Rbac) and `Core/Auth.cs`.
- [ ] Trace one request end-to-end: browser → middleware → endpoint module → `Db` → response.

**Day 3 — the content system & frontend**
- [ ] Read `Core/PageContent.cs`; edit a block in admin and watch it render server-side.
- [ ] Read `frontend/src/api/client.ts` and one page + its test; read `admin/crudConfigs.ts`.

**Day 4 — tests & data**
- [ ] Run the core Python suites and `smoke-test.sh` locally.
- [ ] Do a practice schema change on a scratch branch following §10.3, then throw it away.

**Day 5 — the edges**
- [ ] Read `secureexam/README-SECUREEXAM.md` and `PCI.SecureExam.Core` (host pinning, held results).
- [ ] Read `DEPLOY.md`; understand `/data`, exit 78, and the Render blueprint.
- [ ] Skim the PCI World launch board and `Endpoints/WorldLaunch.cs`.
- [ ] Ship a small real change through the full loop: branch → change → suites → PR → CI green.

---

## 16. Where to look — quick reference

| Need | File |
|---|---|
| Boot, middleware, core endpoints | `backend/Program.cs` |
| Data access / SQL dialect / dual-provider | `backend/Data/Db.cs`, `backend/MYSQL.md` |
| Schema / migrations | `backend/schema.sql`, `backend/Data/Migrate.cs` |
| Auth / RBAC | `backend/Core/Auth.cs`, `backend/Core/Security.cs` |
| A feature's endpoints | `backend/Endpoints/*.cs` (named by area) |
| Content injection | `backend/Core/PageContent.cs` + `CertCatalogue`/`ListSections`/`PriceTags` |
| Admin CRUD factory | `backend/Endpoints/AdminMgmt.cs` + `frontend/src/admin/crudConfigs.ts` |
| Build/run/deploy + verification status | `backend/RUN.md`, `DEPLOY.md` |
| React apps | `frontend/README.md`, `frontend/src/` |
| Secure-exam client | `secureexam/README-SECUREEXAM.md`, `secureexam/build.ps1` |
| PCI World launch gating | `backend/Endpoints/WorldLaunch.cs`; world-only deploy: `PCIWorld/README.md` |
| CI | `.github/workflows/build.yml` |
| Conventions & guardrails | `CLAUDE.md` |
| Environment gotchas (cloud agents) | `AGENTS.md` |
| Deep background / history | `docs/` (archive — background only) |

---

## 17. Glossary

| Term | Meaning |
|---|---|
| **Classic panels** | The pre-React portals: `student.html`, `admin.html`, `exam-ui.html` — still live and reachable |
| **Stage 2 / Stage 3** | Server-side content injection / React SPA fallback, per the middleware pipeline (§2) |
| **Injector** | A server-side content rewriter (`PageContent`, `CertCatalogue`, `ListSections`, `PriceTags`) with its own version cache |
| **CRUD factory** | The `Crud(...)` helper + `CrudSection` component pair that generates admin collections (§5.5) |
| **Dual-provider** | The `Db.Translate` layer that runs SQLite-dialect SQL on MySQL/MariaDB (§5.2) |
| **Launch code** | Short-lived single-use code the portal hands the desktop client — not a bearer token |
| **Host pinning** | The desktop client's HTTPS allowlist; untrusted `api=` hosts are refused |
| **Held result** | Exam result under integrity review — never shows score/pass-fail/credential |
| **Launch board** | Admin console → PCI World → Launch — the owner-only switches that turn world features on |
| **Gate** | An RBAC guard: `GateFn` (section permission) or `OwnerGate` (owner-only) |
| **Exit 78** | The production boot validator refusing unsafe config (§12.2) |
| **`/data`** | The single persistent mount in production (DB + evidence/attachments) |
| **BoK** | Body of Knowledge — certification syllabus content |
| **CPD** | Continuing Professional Development — post-certification credit tracking |

---

*Welcome aboard. When something here disagrees with the code, the code wins — and please fix
this document (and `CLAUDE.md`) in the same PR.*
