# PCI Platform — Onboarding Guide

Welcome to the **PCI Platform**, the full software estate of the Project Controls Institute.
This document gets a new developer (or AI assistant) from zero to a productive first change.
Read it top-to-bottom once; after that, use [`CLAUDE.md`](CLAUDE.md) as the day-to-day reference.

---

## 1. The one-paragraph mental model

Everything is **one ASP.NET Core 8 backend + one database**, deployed as **one Docker image**,
serving four user-facing surfaces from the same URL: a ~216-page server-rendered public website,
a student portal, an admin dashboard, and an in-browser exam preview. A separate **Windows
secure-exam desktop client** talks to that same backend for proctored exams. Content edited in
the admin dashboard appears on the website; everything students do appears in the dashboard.
There is nothing to "wire together" — it is already one system.

| Surface | URL | Notes |
|---|---|---|
| Public website | `/` | SEO-critical, server-rendered static HTML with DB-driven content injection |
| Student portal | `/student.html` (classic) · `/app/` (React) | Certifications, exams, credentials, CPD, billing |
| Admin dashboard | `/admin.html` (classic) · `/admin/` (React) | ~29-section operator console with RBAC |
| Exam preview | `/exam-ui.html` | In-browser exam runner preview |
| Secure exam client | `secureexam/` (Windows WPF) | Kiosk lockdown, single-use launch codes, pinned API host |

---

## 2. Repository map

```
PCI/
├── backend/       ASP.NET Core 8 Minimal API — the entire server (~160+ endpoints)
│   ├── Program.cs        Boot, middleware pipeline, core inline endpoints, module wiring
│   ├── Core/             Cross-cutting services (auth, RBAC, storage, content injection, mailer…)
│   ├── Endpoints/        Feature modules (StudentExam, AdminMgmt, Payments, Public, Casework…)
│   ├── Data/             Db.cs (dual-provider data layer), Migrate.cs, SeedContent.cs
│   ├── wwwroot/          The served static site (~216 .html pages + classic panels + assets)
│   ├── schema.sql        SQLite schema — SOURCE OF TRUTH (53 tables)
│   └── tests/            Python logic/integration/sweep suites + smoke-test.sh
├── frontend/      React 18 + TypeScript (Vite) — student portal (/app/) + admin console (/admin/)
├── secureexam/    .NET 8 Windows WPF secure-exam client + shared Core + companion server
├── PCIWorld/      Dockerfile + README for a PCI-World-only deployment (challenge platform)
├── holding/       Static holding page (+ _redirects)
├── docs/          Historical handoff archive — background only, NOT the current file map
├── Dockerfile     Multi-stage: React builds → .NET publish → runtime image
├── render.yaml    Render Blueprint (one Docker web service + persistent disk at /data)
└── .github/workflows/build.yml   CI
```

Two directories deserve a warning up front:

- **`docs/` is an archive.** Its manifests reference zip bundles and older explorations that are
  *not* the working source. The live code is `backend/`, `frontend/`, `secureexam/`.
- **`backend/wwwroot/app/` and `backend/wwwroot/admin/` are git-ignored build artifacts.**
  Never edit them by hand — they are assembled from `frontend/` during the Docker build.

---

## 3. Day 1 — get it running

### 3.1 Backend (this is the whole platform)

```bash
cd backend
cp .env.example .env    # optional — every value has a working default
dotnet run              # → http://localhost:8080
```

That single process serves the website, `/student.html`, `/admin.html`, and `/exam-ui.html`.
First boot creates and migrates `./pci.db` (SQLite) and seeds content plus two accounts:

- **Owner admin:** `owner@pci.local` / `changeme-owner` (password change forced on first login)
- A demo student is also seeded.

Health check: `curl http://localhost:8080/api/health`.

### 3.2 React apps (only needed for `/app/` and `/admin/` work)

```bash
cd frontend
npm install
npm run dev          # student portal on :5173, proxies /api → :8080
npm run dev:admin    # admin console on :5174
```

Keep the backend running in another terminal so `/api` calls resolve.

### 3.3 Secure-exam client (Windows only for the app; Core builds anywhere)

```powershell
cd secureexam
./build.ps1              # restore → build → dotnet test (Core tests run cross-platform)
./build.ps1 -Run         # run the client against demo launch code PCIDEMO12345 (start the Server first)
```

End-to-end demo: terminal 1 `dotnet run --project PCI.SecureExam.Server`, terminal 2 `./build.ps1 -Run`.
See `secureexam/README-SECUREEXAM.md` for the full picture.

---

## 4. The five concepts you must understand

### 4.1 The middleware pipeline (order matters)

`backend/Program.cs` registers middleware outermost-first: security headers/CSP → CORS →
rate limiting on brute-forceable POST paths → boot-time config validation → maintenance mode →
**dynamic content injection** → static files → **React SPA fallback**. When adding behaviour,
respect this order — e.g. anything that must affect every response goes before static files.

### 4.2 The content system ("the editable website")

The ~216 static pages are editable from the admin dashboard *without redeploying*. On first boot,
`PageContent.SeedFromFiles` captures each page's headline as an editable block. On a page GET,
if the slug has DB overrides, the HTML is rendered server-side with those values injected
(SEO-safe, works with JS off); pages with no overrides fall straight through to static files and
pay nothing. Each injector (`CertCatalogue`, `ListSections`, `PriceTags`, …) caches per version
and bumps its cache when settings change.

### 4.3 The dual-provider data layer

**All DB access goes through the shared `Db` singleton** (`backend/Data/Db.cs`). SQL is written
in **SQLite dialect — the source of truth**; when `DB_PROVIDER=mysql`, `Db.Translate` rewrites it
at runtime. Consequences you must internalise:

- **Datetimes are strings** in `YYYY-MM-DD HH:MM:SS` (UTC) on both providers. Compare instants
  via the `H` helpers (`H.JsMillis` / `H.IsPast` / `H.After`), never lexically.
- Parameters are positional `?` — **always parameterise**, never concatenate user input into SQL.
- Read column values through the `H` coercion helpers (`H.L`/`H.D`/`H.Str`/`H.B`).

### 4.4 Auth & RBAC — two separate worlds

Students (`/api/login` → `login_tokens`, 30-day) and admins (`/api/admin/auth/login` →
`admin_sessions`, 12-hour) use **independent bearer-token systems** with different storage keys —
never shared, never mixed. Tokens are stored hashed; passwords are BCrypt. Admin endpoints are
gated with `GateFn(req, section, ok)` (RBAC section groups) or `OwnerGate` (owner-only).
On the frontend, student and admin use separate API clients and token keys
(`pci.session.token` vs `pci.admin.token`) — reuse `makeClient(tokenKey)`.

### 4.5 The admin CRUD factory

Generic content collections (FAQs, BoK, questions, resources, news, nav, media, pricing rules…)
are registered by one `Crud(name, cols, order, section)` call in `Endpoints/AdminMgmt.cs`, and the
React admin drives them all through one `CrudSection` component configured in
`frontend/src/admin/crudConfigs.ts`. **Adding a whole new admin-editable collection is one
backend line + one config entry** — no new endpoint or component.

---

## 5. Common tasks — recipes

### Add a backend endpoint
1. Find the right module in `backend/Endpoints/` (named by area), or wire a new module in `Program.cs`.
2. Follow the surrounding style: Minimal API, inline validation, `Results.Json(...)`, errors as
   `{ error, … }` with the correct status code (401/403/400/404/503).
3. Gate it: `GateFn` for feature sections, `OwnerGate` for owner-only. Use the shared `db`.
4. Log privileged actions to `audit_logs` via the `logFn`/`Log` helper.

### Change the schema
1. Edit `backend/schema.sql` (source of truth).
2. Add the matching **idempotent** upgrade in `Data/Migrate.cs` (`CREATE TABLE IF NOT EXISTS` /
   `AddCol` guarded by `db.Columns`) so existing databases converge.
3. Regenerate the MySQL schema: `python3 tools/sqlite_to_mysql.py`.
4. Run the Python logic suites (and ideally the MySQL integration run).

### Add an admin-managed content collection
One `Crud(...)` line in `Endpoints/AdminMgmt.cs` + one entry in
`frontend/src/admin/crudConfigs.ts`. Done.

### Work on the React apps
Strict TypeScript (`npm run typecheck` must pass). Reuse `makeClient`, shared `components/`, and
the typed interfaces in `api/types.ts` / `admin/api.ts`. Keep student and admin bundles separate —
they are built independently so admin code never ships in the student bundle.

---

## 6. Testing — run what CI runs for what you touched

From `backend/` (real SQLite, replicating production SQL):

```bash
python3 tests/lifecycle_test.py     # result lifecycle, consents, auto-hold, webhook idempotency
python3 tests/release_test.py       # release/invalidate/reinstate, pass mark, expiry-aware verify
python3 tests/casework_test.py      # appeals, accommodations, attachments, CPD, certificate
python3 tests/settings_test.py      # settings RBAC + readiness gate
python3 tests/publication_test.py   # publication policy, proctoring audit-only, technical blocks
python3 tests/storage_test.py       # MIME/size/sniff/traversal/retention
python3 tests/integration_test.py   # adversarial end-to-end over live HTTP
python3 tests/sweep_500_test.py     # every route × anon/student/owner — asserts 0 × 500
./smoke-test.sh                     # live HTTP smoke suite (boot the backend first)
```

Suites pass when every assertion prints `PASS`/`✓`; a bare `FAIL`/`✗` fails the run.

| You touched… | Run before pushing |
|---|---|
| `backend/` | Python suites + boot the app + `smoke-test.sh` |
| `frontend/` | `npm run typecheck && npm run build` |
| `secureexam/` | `dotnet build` + `dotnet test` |

CI (`.github/workflows/build.yml`) additionally runs a MySQL-parity job (MariaDB 10.11), a
frontend build-assets check, and secureexam builds on both Windows and Linux.

---

## 7. Guardrails (the things that bite newcomers)

- **Security must-haves:** parameterise all SQL; hash tokens before storage; sanitise
  admin-authored HTML with `HtmlSanitize`; uploads go through `Core/Storage` (MIME sniff, size
  cap, traversal guard); keep the CSP allowlist tight; don't leak secrets (`/api/content` and
  `system-check` redact them).
- **Production boot validation is strict on purpose.** In `Production` the app exits 78 on unsafe
  config (non-https `APP_BASE_URL`, wildcard `ALLOWED_ORIGIN`, `/tmp` database, missing Stripe
  webhook secret, legacy admin token on). Don't fight it — fix the config.
- **Graceful degradation is intended behaviour, not a bug:** no Stripe key → payment endpoints
  answer 503, everything else works; no `SMTP_HOST` → emails print to console and are logged;
  backend unreachable → the website stays fully static.
- **PCI World ships switched off.** Community rooms, forum, careers, contributor desk and room
  images each sit behind a `site_settings` flag seeded to `'0'`. Their routes 404ing on a fresh
  install is the design working. Enable from Admin console → **PCI World → Launch** (owner only);
  some features refuse to launch until prerequisites are recorded — enforced in the endpoint.
- **Secure-exam client security model:** pinned HTTPS host allowlist (a malicious `api=` in the
  launch URI is ignored), single-use launch codes (not bearer tokens), server owns the clock and
  scoring, and a **held** (integrity-review) result never shows a score/pass-fail/credential.

---

## 8. Deployment in one breath

One Docker image serves everything; `/data` is the single persistent mount (SQLite DB +
evidence/attachments). **Render** via `render.yaml` is the recommended path (Starter plan or
above — the free tier has no disk and would wipe the DB). Any Docker host works with a
TLS-terminating proxy forwarding `X-Forwarded-Proto`. `PCIWorld/Dockerfile` builds a separate
PCI-World-only service. Full detail: `DEPLOY.md`, `backend/RUN.md`.

---

## 9. Suggested first week

1. **Day 1:** boot the backend, log into `/admin.html` as the owner, change a page headline in
   the content editor, and watch it appear on the public site. This exercises the whole loop.
2. **Day 2:** read `Program.cs` top-to-bottom (it's the map), then skim `Data/Db.cs` and one
   endpoint module relevant to your first task.
3. **Day 3:** run the full Python test suite locally; read one test file end-to-end — the tests
   replicate production SQL and are excellent documentation of business rules.
4. **Day 4:** build the React apps, log into `/app/` and `/admin/`, and trace one API call from
   `src/api/client.ts` through to its backend handler.
5. **Day 5:** make a small real change (a CRUD collection or an endpoint tweak), run the
   relevant suites, and open a PR.

---

## 10. Where to look

| Need | File |
|---|---|
| Boot, middleware, core endpoints | `backend/Program.cs` |
| Data access / SQL dialect | `backend/Data/Db.cs`, `backend/MYSQL.md` |
| Schema / migrations | `backend/schema.sql`, `backend/Data/Migrate.cs` |
| Auth / RBAC | `backend/Core/Auth.cs`, `backend/Core/Security.cs` |
| A feature's endpoints | `backend/Endpoints/*.cs` (named by area) |
| Content injection | `backend/Core/PageContent.cs` + `CertCatalogue`/`ListSections`/`PriceTags` |
| Build/run/deploy | `backend/RUN.md`, `DEPLOY.md` |
| React apps | `frontend/README.md`, `frontend/src/` |
| Secure-exam client | `secureexam/README-SECUREEXAM.md` |
| Environment variables | `backend/.env.example`, `CLAUDE.md` §8 |
| AI-assistant conventions | `CLAUDE.md`, `AGENTS.md` |
| Deep background / history | `docs/PCI_DEVELOPER_GUIDE.md` (archive) |
