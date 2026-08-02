# CLAUDE.md

Guidance for AI assistants (and humans) working in this repository. Read this first, then the
component READMEs it points to. Keep it current when structure, workflows, or conventions change.

---

## 1. What this is

The **PCI Platform** — the full software estate for the Project Controls Institute: a public
marketing website, a student portal, an admin dashboard, and a Windows secure-exam desktop client,
all backed by one ASP.NET Core service and one database.

**One deployment serves every surface below from the same URL:**

| Surface | URL | What it is |
|---|---|---|
| Public website (~216 pages) | `/` | SEO-critical marketing/info site, server-rendered static HTML with DB-driven content injection |
| Student portal | `/student.html` (classic) and `/app/` (React) | Candidate journey: certifications, exams, credentials, CPD, billing |
| Admin dashboard | `/admin.html` (classic) and `/admin/` (React) | ~29-section operator console with RBAC |
| Exam preview | `/exam-ui.html` | In-browser exam runner preview |

Content edited in the admin dashboard appears on the website; everything students do appears in the
dashboard. There is nothing separate to wire together — one backend, one database.

A separate **Windows secure-exam desktop client** (`secureexam/`) delivers proctored exams with
kiosk lockdown; it talks to the same backend over the pinned API host using single-use launch codes.

---

## 2. Repository layout

```
PCI/
├── backend/            ASP.NET Core 8 Minimal API — the whole platform's server
│   ├── Program.cs      Boot, middleware pipeline, ~17 inline endpoints, endpoint-module wiring
│   ├── Core/           Cross-cutting services (auth, RBAC, storage, content injection, mailer…)
│   ├── Endpoints/      Feature endpoint modules (student exam, admin mgmt, payments, public…)
│   ├── Data/           Db.cs (dual-provider data layer), Migrate.cs, SeedContent.cs
│   ├── wwwroot/        The served static site: ~216 .html pages + classic panels + assets/images
│   ├── emails/         12 transactional HTML email templates
│   ├── tests/          Python logic + integration + sweep suites (run against real SQLite/MySQL)
│   ├── tools/          sqlite_to_mysql.py (regenerates schema.mysql.sql)
│   ├── schema.sql      SQLite schema — SOURCE OF TRUTH (53 tables)
│   ├── schema.mysql.sql  Generated from schema.sql for the MySQL provider
│   └── smoke-test.sh   Live HTTP smoke suite used by CI
├── frontend/           React + TypeScript (Vite) — the interactive app screens
│   └── src/            Student portal (src/pages, served /app/) + admin console (src/admin, /admin/)
├── secureexam/         .NET 8 Windows WPF secure-exam client + shared Core + companion server
│   ├── PCI.SecureExam.Core     Pure net8.0 testable security/exam logic
│   ├── PCI.SecureExam.App      net8.0-windows WPF kiosk client (OpenCV + NAudio)
│   ├── PCI.SecureExam.Server   ASP.NET server (controllers + SignalR hubs) for proctoring
│   ├── PCI.SecureExam.Tests    xUnit tests (cross-platform for Core)
│   └── PCI.SecureExam.Core.RunnableChecks  Package-free host-pinning attack checks
├── docs/               Developer guides, changelogs and audit history (handoff archive)
├── Dockerfile          Multi-stage build: React apps → .NET publish → runtime image
├── render.yaml         Render Blueprint (one Docker web service + 5 GB persistent disk at /data)
├── DEPLOY.md           Deployment guide (Render + generic Docker)
└── .github/workflows/build.yml   CI: backend (SQLite + MySQL), frontend, secureexam
```

> **`docs/` is a historical handoff archive.** Its `ARTIFACT_MANIFEST.md` / `PCI_MASTER_INDEX.md`
> reference zip bundles and earlier explorations that are **not** the working source. The live code
> is `backend/`, `frontend/`, and `secureexam/`. Treat `docs/` guides as background, not as the
> current file map.

---

## 3. The backend (`backend/`) — the heart of the system

ASP.NET Core 8 **Minimal API** (not MVC controllers). ~160+ HTTP endpoints. Targets `net8.0`,
`Nullable` + `ImplicitUsings` enabled, `InvariantGlobalization`. Packages: `Microsoft.Data.Sqlite`,
`MySqlConnector`, `BCrypt.Net-Next`, `Stripe.net`, `AWSSDK.S3`.

### 3.1 Request pipeline (order matters — see `Program.cs`)

Middleware is registered outermost-first so **every** response carries the right headers:

1. **Security headers + CSP** — `X-Content-Type-Options`, `Referrer-Policy`, `X-Frame-Options`,
   `Cross-Origin-Opener-Policy`, a scoped `Content-Security-Policy`, and HSTS when the
   TLS-terminating proxy reports `X-Forwarded-Proto: https` (matches the first proxy hop).
2. **CORS** — reflects `ALLOWED_ORIGIN` only (wildcard rejected in production); handles the 204 preflight.
3. **Rate limiting** — in-memory fixed-window (10 req/60 s) on brute-forceable POST paths
   (`/api/login`, `/api/admin/auth/login`, `/api/forgot-password`, `/api/validate-code`,
   `/api/set-password`, `/api/exam/authorize`), keyed on the first `X-Forwarded-For` hop.
4. **Boot-time config validation** — see §7.4. In production the app **exits 78** on unsafe config.
5. **Maintenance mode** — a 503 holding page for public pages while `/api/*` and admin stay up.
6. **Dynamic content injection** (Stage 2) — before static files; see §3.4.
7. **Static files** (`UseDefaultFiles` + `UseStaticFiles`) serving `wwwroot/`.
8. **React SPA fallback** (Stage 3) — terminal middleware that returns the `/app` or `/admin` shell
   for extension-less client-side routes; real assets and `/api/*` never reach it.

### 3.2 Code organisation

- **`Program.cs`** wires everything: middleware, ~17 inline endpoints (health, system-check, content,
  student/admin auth, Team & Access, settings), then registers the endpoint modules.
- **`Endpoints/*.cs`** — each is a `static class` with a `Map(app, db, logFn, …)` method. Modules:
  `StudentExam` (exam pipeline, biggest), `ExamClient` (desktop secure-client endpoints),
  `AdminProctoring`, `AdminStudents`, `Public` (pricing/codes/verify/forms), `AdminMgmt`
  (CRUD factory + management), `Payments` (Stripe), `AdminExtra`, `Reviews`, `Casework`
  (appeals/accommodations/CPD/support).
- **`Core/*.cs`** — shared services: `Auth`/`Rbac`/`Security` (sessions, permissions, hashing),
  `H` (coercion + exam-config + date helpers), `Storage` (local/S3 evidence + attachments),
  `Mailer`, `PageContent`/`CertCatalogue`/`ListSections`/`PriceTags`/`PageScan`/`SearchIndex`
  (content injection), `Lifecycle`, `RetentionService` (daily purge), `HtmlSanitize`, `CertCatalogue`.
- **`Data/*.cs`** — `Db` (data access), `Migrate` (schema + idempotent upgrades + seeds), `SeedContent`.

### 3.3 Data access — `Data/Db.cs` (dual-provider)

**All DB access goes through the shared `Db` singleton.** Never open your own connection.

- SQL is written in **SQLite dialect — the source of truth.** When `DB_PROVIDER=mysql`, `Db.Translate`
  rewrites it to MySQL/MariaDB at runtime (datetime math, upserts, `last_insert_rowid()`/`changes()`,
  `julianday`, `strftime`, partial unique indexes). App code stays provider-agnostic.
- **Datetimes are strings** in `YYYY-MM-DD HH:MM:SS` (UTC) on both providers, so all string-based date
  logic (`H.JsMillis`/`H.IsPast`/`H.After`) is identical everywhere. Compare instants via those
  helpers, never lexically (`' '` 0x20 < `'T'` 0x54 causes off-by-a-day bugs).
- Parameters use positional `?`, rewritten to `@p0, @p1, …` in `Bind`. **Always parameterise** — never
  string-concatenate user input into SQL.
- API surface: `Query`/`QueryOne` (→ `Dictionary<string,object?>` rows, case-insensitive keys),
  `Scalar<T>`, `Execute`, `ExecuteReturningId`, `ExecuteWithChanges` (atomic id+changes, used by the
  Stripe webhook idempotency gate), `Transaction(Action)`, `Columns(table)`.
- Read column values through the `H` coercion helpers (`H.L`/`H.D`/`H.Str`/`H.B`) — they hide the
  provider's object typing so ported code reads cleanly.

### 3.4 Content system (how the "editable website" works)

The ~216 static pages are editable without redeploying:

- `PageContent.SeedFromFiles` captures each page's headline as an editable block on first boot, so
  every page is admin-editable out of the box.
- On a page GET, if the slug has DB overrides (`page_blocks`/`site_content`) or a certification
  catalogue applies, the HTML is rendered server-side with those values injected (SEO-safe, works with
  JS off), then run through `CertCatalogue`, `ListSections` (nav/FAQs/BoK/governance/resources/news),
  and `PriceTags`. Pages with no overrides fall straight through to static files and pay nothing.
- Each injector caches per its own version and calls `Bump()` when its settings change.
- `assets/cms-loader.js` is a client-side fallback: with `<meta name="pci-api">` set it hydrates
  `[data-cms]` elements, the announcement banner, and the newsletter form from `/api/content`. With no
  API configured the site stays fully static.

### 3.5 Auth & RBAC

- **Two independent bearer-token session systems**, different storage keys, never shared:
  students (`/api/login` → `login_tokens`, 30-day) and admins (`/api/admin/auth/login` →
  `admin_sessions`, 12-hour). Tokens are stored **hashed** (`Security.Sha`); logout deletes the row.
- Passwords are BCrypt (`BCrypt.Net-Next`); `Security.VerifyPassword` returns `false` (never throws)
  on a malformed stored hash so login endpoints answer 401, not 500.
- **RBAC** (`Core/Security.cs` → `Rbac`): four section groups (platform/website/student/exam) map to
  granular permissions; roles (`owner`, `website_manager`, `student_manager`, `exam_manager`,
  `viewer`, `custom`) grant sets, plus per-admin extra permissions. `owner` gets everything.
- Endpoint gating: `GateFn(req, section, ok)` (403 unless owner or has `section`) for feature modules;
  `OwnerGate` for owner-only routes (Team & Access). Settings PATCH is **deny-by-default** by key
  prefix (`web_`→`set_web`, `sp_`→`set_sp`, `exam_`→`set_exam`, else owner `settings`).

### 3.6 Admin CRUD factory

Generic content collections (FAQs, BoK, questions, resources, news, nav, media, pricing rules, …) are
registered by one `Crud(name, cols, order, section)` helper in `Endpoints/AdminMgmt.cs`, exposing
uniform `GET/POST/PATCH/DELETE /api/admin/{name}`. The React admin drives them all through a single
`CrudSection` component configured in `frontend/src/admin/crudConfigs.ts` — **adding a collection is
one backend `Crud(...)` line + one config entry**, no new endpoint or component.

### 3.7 Migrations & seeds — `Data/Migrate.cs`

Runs on every boot and is **idempotent**: loads `schema.sql` (or `schema.mysql.sql`), then
`CREATE TABLE IF NOT EXISTS` / `AddCol` (guarded by `db.Columns`) for upgrades, `CREATE …INDEX IF NOT
EXISTS`, and first-run seeds (bootstrap owner admin, demo student, content via `SeedContent`). Safe to
re-run; never overwrites edited content. **When you change `schema.sql`, add the matching idempotent
upgrade here so existing databases converge, and regenerate the MySQL schema (see §3.8).**

### 3.8 Changing the schema

1. Edit `backend/schema.sql` (source of truth).
2. Add the idempotent upgrade in `Data/Migrate.cs` (`CREATE TABLE IF NOT EXISTS` / `AddCol`).
3. Regenerate the MySQL schema: `python3 tools/sqlite_to_mysql.py` (writes `schema.mysql.sql`).
4. Run the Python logic suites and, ideally, the MySQL integration run.

---

## 4. The frontend (`frontend/`) — React apps

React 18 + TypeScript + Vite. **Two independent apps/bundles** share one project, components, and the
typed API client:

- **Student portal** → `/app/` (`index.html` → `src/main.tsx`, `vite.config.ts`, base `/app/`).
- **Admin console** → `/admin/` (`admin.html` → `src/admin/main.tsx`, `vite.admin.config.ts`, base
  `/admin/`, output `dist-admin`, entry renamed to `index.html` in the Docker image).

Building separately keeps admin code out of the student bundle and vice versa. The admin console covers
the full ~29-section operator surface; the content collections are all one `CrudSection` component.

- **API client** (`src/api/client.ts`): a thin typed `fetch` wrapper. Bearer token in `sessionStorage`
  (cleared on tab close — deliberate for shared machines), `Authorization: Bearer …`, a central 401
  handler that clears the token and redirects to login. Student and admin use **separate** clients/keys
  (`pci.session.token` vs `pci.admin.token`). Reuse `makeClient(tokenKey)` — don't hand-roll fetches.
- Auth mirrors the classic portals exactly (same tokens, same endpoints).
- `tsconfig.json` is strict (`strict`, `noUnusedLocals`, `noUnusedParameters`,
  `noFallthroughCasesInSwitch`). `npm run typecheck` must pass.

**The React apps are the "Stage 3" logged-in screens only.** The ~210 SEO-critical marketing pages
stay on the server-rendered content system (Stage 2). The classic `student.html`/`admin.html` panels
remain in place and reachable.

---

## 5. The secure-exam client (`secureexam/`)

A .NET 8 solution (`PCI.SecureExam.sln`, SDK pinned to `8.0.100` via `global.json`; shared props in
`Directory.Build.props` — `LangVersion=latest`, `Nullable`/`ImplicitUsings` on, company/version metadata).
Four projects in the solution, plus one standalone check harness:

- **`PCI.SecureExam.Core`** — pure `net8.0` class library, **zero package/project references**, so it
  builds and tests on Linux/CI. Holds the wire contract (DTOs/enums), `pciexam://` launch-URI parsing,
  **API host-pinning** (`ClientConfig.IsTrustedApi`/`WithLaunch`/`EnsureTrustedOrThrow`), the baseline
  proctor/identity analyzers, and the held-result presentation rule. This is where the
  security-critical logic lives.
- **`PCI.SecureExam.App`** — `WinExe`, `net8.0-windows`, `UseWPF`+`UseWindowsForms`, `AssemblyName=PCISecureExam`.
  The downloadable Windows kiosk client. Packages: `OpenCvSharp4` (4.9.0, webcam/face), `NAudio` (2.2.1,
  mic), `Microsoft.Extensions.*` (DI/config/logging), `Microsoft.AspNetCore.SignalR.Client` (chat).
  Subfolders: `Security` (P/Invoke lockdown: `KeyboardHook`, `KioskWindow`, `ProcessGuard`,
  `DisplayGuard`, `VmDetector`, `CaptureShield`), `Proctoring` (`CameraService`, `MicMonitor`),
  `Api` (`PciApiClient`), `Exam` (`ExamFlow` state machine, `HeartbeatService`, DPAPI `SecureStore`),
  `Providers` (`AiProviderFactory` — the single AI-provider seam), `Support` (`ChatClient`),
  `Views` (`MainWindow`), `Infrastructure` (`ConfigLoader`, `SelfTest`, `UriSchemeRegistrar`), `Assets`.
- **`PCI.SecureExam.Server`** — `Microsoft.NET.Sdk.Web` (`net8.0`). An **optional reference** service:
  `Controllers/ExamController` (`api/exam` — launch-code redemption via in-memory `LaunchStore`,
  evidence + identity sinks) and `Hubs/ProctorHub` (SignalR at `/hubs/proctor`). In production the main
  `backend/` is the system of record; scoring is **never** client-side.
- **`PCI.SecureExam.Tests`** — xUnit against Core only, so it runs cross-platform (launch parsing, DTO
  contract, baseline analyzer, held-result invariant).
- **`PCI.SecureExam.Core.RunnableChecks`** — *not in the solution*; a package-free console harness that
  copies in Core's `LaunchParameters.cs`/`ClientConfig.cs` and runs 15 host-pinning attack assertions
  offline anywhere.

**Security model:** the client is **pinned** to a dot-anchored HTTPS allowlist
(`projectcontrolsinstitute.org`, `localhost`) — a malicious `api=` in the launch URI
is ignored and the client refuses to start against an untrusted host. It registers the `pciexam://`
scheme; the portal hands it a short-lived **single-use launch code** (not a bearer token), redeemed
against the pinned host. The **server owns the clock and scoring** (the heartbeat returns canonical
`RemainingSeconds`; ForceSubmit is server-driven). Kiosk lockdown is user-space and degrades honestly
(cannot block Ctrl+Alt+Del). A **held** (integrity-review) result never shows a score/pass-fail/credential.
Secrets go in gitignored `appsettings.Local.json`; any config key is overridable by a `PCI_`-prefixed
env var (e.g. `PCI_Ai__ApiKey`).

**Build/run (Windows for the WPF app; Core builds/tests anywhere):**
```powershell
cd secureexam
./build.ps1                 # restore → build -c Release → dotnet test (Core, cross-platform)
./build.ps1 -SelfTest       # machine readiness check (camera/mic/displays/VM/apps/network); exit 0=ready
./build.ps1 -Run            # runs the client against demo launch code PCIDEMO12345 (start the Server first)
./build.ps1 -Publish        # → self-contained single-file PCISecureExam.exe
```
End-to-end demo: terminal 1 `dotnet run --project PCI.SecureExam.Server` (→ http://localhost:5000),
terminal 2 `./build.ps1 -Run`. There is **no pre-built `.exe`** in the repo — publish on Windows.
Full detail in `secureexam/README-SECUREEXAM.md`.

---

## 6. Build, run & test

### 6.1 Backend
```bash
cd backend
cp .env.example .env          # optional — every value has a working default
dotnet run                    # → http://localhost:8080  (site, /student.html, /admin.html, /exam-ui.html)
```
First admin sign-in: `owner@pci.local` / `changeme-owner` (password change forced on first login).
Creates/migrates `./pci.db` on first boot. Health: `curl http://localhost:8080/api/health`.

### 6.2 Frontend
```bash
cd frontend
npm install
npm run dev          # Vite student portal on :5173, proxies /api → :8080
npm run dev:admin    # admin console on :5174
npm run typecheck    # tsc --noEmit — must pass
npm run build        # typecheck + build:student (dist/) + build:admin (dist-admin/)
```
Run the backend separately so `/api` calls resolve. **`backend/wwwroot/app/` and `…/admin/` are
git-ignored build artifacts** — never edit them by hand; they're assembled from `frontend/` in Docker.

### 6.3 Tests

**Python logic + integration suites** (real SQLite, replicate production SQL) — from `backend/`:
```bash
python3 tests/lifecycle_test.py     # result lifecycle, consents, auto-hold, entitlement, webhook idempotency
python3 tests/release_test.py       # admin release/invalidate/reinstate, pass mark, expiry-aware verify
python3 tests/casework_test.py      # appeals, accommodations, attachments, CPD, certificate
python3 tests/settings_test.py      # settings RBAC + readiness gate
python3 tests/publication_test.py   # publication policy, proctoring audit-only, technical blocks
python3 tests/storage_test.py       # storage abstraction: MIME/size/sniff/traversal/retention
python3 tests/integration_test.py   # adversarial end-to-end over live HTTP (runs against SQLite or MySQL)
python3 tests/sweep_500_test.py     # every route × anon/student/owner — asserts 0 × 500
./smoke-test.sh                     # live HTTP smoke suite (boot the backend first)
```
Suites pass when every assertion prints `PASS`/`✓`; a bare `FAIL` or `✗` fails the run.

**MySQL parity:** `TEST_DB_PROVIDER=mysql MYSQL_HOST=… MYSQL_USER=… MYSQL_PASSWORD=… MYSQL_DATABASE=… python3 tests/integration_test.py`.

### 6.4 CI (`.github/workflows/build.yml`)

Runs on push to `main` and on every PR. Jobs:
- **backend** — build → 6 Python logic suites → JS-syntax gate on the app shells → boot backend →
  `smoke-test.sh` → integration suite → 500-sweep → `system-check` gating probe (+ non-blocking S3/moto).
- **backend-mysql** — the adversarial integration suite against a MariaDB 10.11 service (MySQL parity gate).
- **frontend** — `npm ci` → `npm run typecheck` → `npm run build`; fails if either app produced no assets.
- **secureexam-windows** — restore/build the solution + `dotnet test` on `windows-latest` (WPF needs Windows).
- **secureexam-core-linux** — Core/tests on `ubuntu-latest` (cross-platform).

**Before pushing, run what CI runs for what you touched:** backend → the Python suites + a boot +
`smoke-test.sh`; frontend → `npm run typecheck && npm run build`; secureexam → `dotnet build` +
`dotnet test`.

---

## 7. Conventions & guardrails

### 7.1 Backend code style
- Minimal-API endpoints, **inline validation** in each handler (RBAC, ownership, timing, type/size,
  entitlement) — match the surrounding guard style rather than introducing a framework.
- Every response is JSON via `Results.Json(...)`; errors are `{ error, … }` with the right status code
  (401 unauthorised, 403 forbidden/`owner_only`, 400 validation, 404 not-found, 503 disabled feature).
- Log privileged actions to `audit_logs` via the `logFn`/`Log` helper.
- New endpoints go in the relevant `Endpoints/*.cs` module (or a new module wired in `Program.cs`),
  gated with `GateFn`/`OwnerGate`, using the shared `db` — keep the module's `Map(...)` signature shape.

### 7.2 Security must-haves
- Parameterise all SQL. Hash tokens before storage. Sanitise admin-authored HTML with `HtmlSanitize`.
- Don't leak secrets: `/api/content` and `system-check` redact secret/SMTP/result-policy keys.
- Evidence/attachment uploads go through `Core/Storage` (MIME sniff, size cap, path-traversal guard,
  retention); the request body is capped at 6 MB in Kestrel.
- Keep the CSP allowlist tight — only add an origin the site genuinely uses.

### 7.3 Frontend code style
- Strict TypeScript; no unused locals/params. Reuse `makeClient`, the shared `components/` and typed
  interfaces in `api/types.ts` / `admin/api.ts`. Keep student and admin bundles/tokens separate.

### 7.4 Production config validation (don't fight it)
In `Production` the app logs every config issue and **refuses to boot (exit 78)** on hard blockers:
`APP_BASE_URL` must be a public https URL; `ALLOWED_ORIGIN` must be explicit (no wildcard);
`DATABASE_FILE` must be persistent (not `/tmp`); `STRIPE_WEBHOOK_SECRET` required once
`STRIPE_SECRET_KEY` is set; legacy admin token must be off. `ALLOW_INSECURE_PRODUCTION=true` overrides —
emergencies only. Owner-only readiness: `GET /api/admin/system-check`.

### 7.5 Graceful degradation (intended behaviour, not bugs)
No Stripe key → payment endpoints answer **503**, everything else works. No `SMTP_HOST` → emails print
to the console and are recorded in the email log. Backend unreachable → the website stays fully static.

---

## 8. Environment variables (backend)

| Var | When | Notes |
|---|---|---|
| `DATABASE_FILE` | always | SQLite path; **persistent** in prod (not `/tmp`). Default `./pci.db` |
| `DB_PROVIDER` | optional | `sqlite` (default) or `mysql`/`mariadb` (+ `MYSQL_*` or `MYSQL_CONNECTION_STRING`) |
| `PORT` | optional | default 8080 |
| `ASPNETCORE_ENVIRONMENT` | prod | `Production` turns on the boot config validator |
| `APP_BASE_URL` / `SITE_BASE_URL` | prod | public HTTPS URL |
| `ALLOWED_ORIGIN` | prod | exact origin, no wildcard |
| `ADMIN_OWNER_EMAIL` / `ADMIN_OWNER_PASSWORD` | first boot | bootstrap owner; change forced at first login |
| `STRIPE_SECRET_KEY` / `STRIPE_WEBHOOK_SECRET` | payments | webhook secret required once the key is set |
| `SMTP_HOST` (+ `SMTP_PORT`/`SMTP_USER`/`SMTP_PASS`/`MAIL_FROM`) | email | without it, emails log to console |
| `STORAGE_PROVIDER` / `STORAGE_ROOT` / `S3_*` | optional | `local` (default) or `s3`-compatible (needs `S3_BUCKET`) |
| `CSP_REPORT_ONLY` | optional | `true` runs CSP report-only instead of enforcing |
| `ENABLE_LEGACY_ADMIN_TOKEN` | never in prod | app errors on boot if on in prod |
| `ALLOW_INSECURE_PRODUCTION` | escape hatch | boot despite config errors — not recommended |

See `backend/.env.example`, `backend/RUN.md`, `backend/MYSQL.md`, and `DEPLOY.md` for the full detail.

---

## 9. Deployment (summary)

One Docker image serves all surfaces (`Dockerfile`: build React → publish .NET → runtime; the React
bundles are copied into `wwwroot/app` + `wwwroot/admin`). `/data` is the single persistent mount
(SQLite DB + evidence/attachments). **Render** is the recommended path via `render.yaml` (Starter plan
or above — the free tier has no disk and would wipe the DB); or any Docker host with a TLS-terminating
reverse proxy forwarding `X-Forwarded-Proto`. Full instructions in `DEPLOY.md` and `backend/RUN.md` §8.

---

## 10. Where to look first

| Need | File |
|---|---|
| Boot, middleware, core endpoints | `backend/Program.cs` |
| Data access / SQL dialect / dual-provider | `backend/Data/Db.cs`, `backend/MYSQL.md` |
| Schema / migrations | `backend/schema.sql`, `backend/Data/Migrate.cs` |
| Auth / RBAC | `backend/Core/Auth.cs`, `backend/Core/Security.cs` |
| A feature's endpoints | `backend/Endpoints/*.cs` (named by area) |
| Content injection | `backend/Core/PageContent.cs` + `CertCatalogue`/`ListSections`/`PriceTags` |
| Build/run/deploy + verification status | `backend/RUN.md`, `DEPLOY.md` |
| React apps | `frontend/README.md`, `frontend/src/` |
| Secure-exam client | `secureexam/README-SECUREEXAM.md`, `secureexam/build.ps1` |
| CI | `.github/workflows/build.yml` |
| Turning PCI World features on | Admin console → **PCI World → Launch** (owner only); `backend/Endpoints/WorldLaunch.cs` |
| Deep background / history | `docs/PCI_DEVELOPER_GUIDE.md`, `docs/*Changelog*.md` (archive) |

> **PCI World ships switched off.** Community rooms, the forum, careers, the contributor desk and
> room images each sit behind a `site_settings` flag that seeds `'0'`, so a deployment is never a
> launch. Their routes returning 404 on a fresh install is the design working, not a fault — switch
> them on from the launch board above. Three of them refuse to move until a prerequisite is recorded
> (a moderation provider, a candidate privacy notice, contributor terms), and that refusal is
> enforced in the endpoint, not the UI.
```
