# Project Controls Institute — Developer Guide

> **Historical reference:** sections describing a SQLite-only backend, vanilla-only frontend, or
> `student.html`/`admin.html` as the primary portals predate the React + MySQL production architecture.
> Use `docs/audit/PHASE_0_PLATFORM_AUDIT_2026-07-25.md`, `backend/MYSQL.md`, and current code for
> authoritative deployment decisions. SQLite is local/CI smoke only.

**Version:** 1.0 · **Backend:** ASP.NET Core 8 (`PCI.Backend`) · **Status:** compile- and boot-verified (.NET 8.0.128)

This is the complete engineering reference for the Project Controls Institute (PCI) certification platform:
what the system is, how it is put together, how the backend and database work, the full API surface, and how
to run, extend, and deploy it. It is written to be read top-to-bottom by a new engineer and used afterwards
as a lookup reference.

---

## Table of contents

1. [What the platform is](#1-what-the-platform-is)
2. [Non-negotiable legal & brand rules](#2-non-negotiable-legal--brand-rules)
3. [System architecture](#3-system-architecture)
4. [Technology stack](#4-technology-stack)
5. [Repository layout](#5-repository-layout)
6. [Backend architecture](#6-backend-architecture)
7. [The data-access layer (`Db.cs`)](#7-the-data-access-layer-dbcs)
8. [Authentication & sessions](#8-authentication--sessions)
9. [Authorisation (RBAC)](#9-authorisation-rbac)
10. [Database schema](#10-database-schema)
11. [API reference](#11-api-reference)
12. [The exam pipeline](#12-the-exam-pipeline)
13. [Payments (Stripe)](#13-payments-stripe)
14. [Email system](#14-email-system)
15. [Settings & configuration](#15-settings--configuration)
16. [The front-end applications](#16-the-front-end-applications)
17. [The secure exam desktop client](#17-the-secure-exam-desktop-client)
18. [Local development](#18-local-development)
19. [Testing & CI](#19-testing--ci)
20. [Deployment](#20-deployment)
21. [Security considerations](#21-security-considerations)
22. [How to extend the platform](#22-how-to-extend-the-platform)
23. [Appendix: the Node reference backend](#23-appendix-the-node-reference-backend)

---

## 1. What the platform is

Project Controls Institute Global, Inc. is a certification body for project-controls professionals. Its
flagship credential is **PCP-AI** ("Project Controls Professional — AI"), built around the motto *"AI proposes,
the professional disposes."* The platform is the software that runs the whole operation:

- a **public website** that markets the credential and the body of knowledge;
- an **enrolment + payment** flow that sells membership, the exam, or a bundle;
- a **student portal** where members manage their profile, schedule and sit the exam, track CPD, and get support;
- a **secure exam client** (Windows desktop app) that delivers the proctored exam;
- an **admin panel** that runs the whole thing — members, payments, credentials, content, live proctoring, and a role-based team;
- a **backend API** (the subject of most of this guide) that serves all of the above from one process.

The credential itself is only ever awarded by passing the proctored exam under PCI policy. **Payment buys access
only** — never the credential. This distinction is wired deep into the code (see §13).

---

## 2. Non-negotiable legal & brand rules

These constraints are legal commitments, not stylistic preferences. Any feature, copy change, or seed data
**must** respect them:

- The organisation is a **Delaware Non-Stock Corporation *intending* 501(c)(3) status** — it has **not** been
  granted it. Never state or imply that it has.
- **Donations are NOT tax-deductible.** Never imply otherwise.
- The body is **NOT** ISO/IEC 17024 accredited. Do not claim accreditation.
- The public credential **registry is "in development."** Do not present it as a complete, authoritative registry.
- **Never fabricate** statistics, member counts, named people, testimonials, partner logos, or endorsements.
- Chapters are **"in formation,"** not established.
- Copy is **British English**. Prices are shown as **"USD X"** (e.g. `USD 99`).

**Pricing (seeded):** membership `USD 99` (with a 50% default discount → `USD 49.50`); exam `USD 500` (with a
30% default discount → `USD 350`). Discounts are data in `pricing_rules`, not hard-coded — see §13/§15.

**Brand tokens:** the primary colour is a blue (`#1D4ED8`) that is, for historical reasons, referred to in some
CSS as `--red` — it is blue. Ink `#0F172A`. Typography: Archivo (display) + Inter (body). The logo is a
typographic lockup (`span.lockup > span.lp` = "PCI", `span.lb`, `span.ln`), not an image.

---

## 3. System architecture

One backend process serves a JSON API **and** the static files for all four front-end apps from `wwwroot/`.
The desktop exam client talks to the same API over HTTPS.

```
                         ┌──────────────────────────────────────────────┐
                         │                PCI.Backend                    │
                         │        (ASP.NET Core 8, one process)          │
   Browsers ───────────▶ │  ┌────────────┐   ┌────────────────────────┐ │
   (4 web apps)          │  │  Static     │   │  JSON API (123 routes) │ │
                         │  │  files      │   │  Program.cs +          │ │
   Desktop exam   ─────▶ │  │  wwwroot/   │   │  Endpoints/*.cs        │ │
   client (WPF)          │  └────────────┘   └───────────┬────────────┘ │
                         │                    ┌───────────▼───────────┐  │
                         │                    │  Db.cs  (SQLite, WAL)  │  │
                         │                    └───────────┬───────────┘  │
                         └────────────────────────────────┼──────────────┘
                                                           ▼
                                                   pci.db (SQLite file)
                              external: Stripe (checkout+webhook), SMTP (email)
```

**The four web apps** (all served from `wwwroot/`):

| App | Entry file | Audience | Auth |
|-----|-----------|----------|------|
| Public website | `index.html` (+ ~215 pages) | Prospects | none |
| Student portal | `student.html` | Members | Bearer student session |
| Admin panel | `admin.html` | Staff | Bearer admin session (or legacy token) |
| Exam preview | `exam-ui.html` | Demo/QA | Bearer student session |

**Request flow.** A browser request first passes maintenance-mode middleware (returns a 503 holding page for
public pages when `web_maintenance_mode` is on; `/api/*` and `/admin.html` stay up), then CORS, then routing.
API routes are matched before the static-file middleware, so `/api/...` never falls through to a file. Anything
not matched by an API route is served from `wwwroot/`, with `index.html` as the default document.

---

## 4. Technology stack

**Backend**
- **.NET 8 / ASP.NET Core 8**, Minimal API style (no MVC controllers).
- **Microsoft.Data.Sqlite 8.0.7** — thin ADO.NET access to SQLite (see §7).
- **BCrypt.Net-Next 4.0.3** — password hashing.
- **Stripe.net 45.14.0** — checkout sessions + webhook signature verification.
- Database: **SQLite** with **WAL** journalling, a single shared connection guarded by a lock.

**Front-end** — dependency-free HTML/CSS/vanilla JS (no build step, no framework). Fonts: Archivo + Inter.

**Desktop exam client** — **.NET 8 / WPF** (Windows only), split into Core / App / Server / Tests projects.

**Why SQLite + Minimal API + no ORM?** The platform is a single-node certification back office, not a
high-concurrency SaaS. SQLite with WAL comfortably handles the load, keeps deployment to a single file, and
makes the schema explicit. A hand-written data layer that mirrors the (proven) Node reference keeps behaviour
identical and avoids ORM translation surprises. See §23 for the Node reference relationship.

---

## 5. Repository layout

```
PCI.Backend/
├── Program.cs                 Boot, middleware, auth/RBAC, settings, team CRUD, module wiring
├── Core/
│   ├── H.cs                   Shared helpers: JSON body parsing, SQLite type coercion,
│   │                          exam config, attempt-token resolution, ISO time helpers
│   ├── Auth.cs                AdminCtx/UserCtx records, session resolvers, Settings helpers
│   └── Security.cs            SHA-256 token hashing, RandomHex, the RBAC model (Rbac class)
├── Data/
│   ├── Db.cs                  The data-access layer (Query/Execute/Scalar/…)
│   └── Migrate.cs             Schema application + idempotent upgrades + bootstrap owner seed
├── Endpoints/
│   ├── Public.cs              pricing, validate-code, verify, set-password, forgot, inquiry,
│   │                          newsletter, form-submit                          (8 routes)
│   ├── StudentExam.cs         the student portal + exam pipeline               (28 routes)
│   ├── ExamClient.cs          secure desktop client: authorize/evidence/identity (3 routes)
│   ├── AdminProctoring.cs     live proctoring console                          (6 routes)
│   ├── AdminStudents.cs       student-360 management                          (13 routes)
│   ├── AdminMgmt.cs           CMS CRUD factory (8 tables) + admin management   (many routes)
│   ├── AdminExtra.cs          students roster, tickets, codes v2, reports, enrolment sessions
│   └── Payments.cs            Stripe checkout + webhook                         (2 routes)
├── wwwroot/                   The four web apps (static files)
├── emails/                    12 transactional email templates (HTML)
├── schema.sql                 The full database schema (42 tables)
├── PCI.Backend.csproj         Package refs, net8.0, Nullable+ImplicitUsings enabled
├── smoke-test.sh              46 live HTTP checks (used by CI)
├── .github/workflows/build.yml  CI: restore → build → boot → smoke test
├── .env.example               Every configurable value (all have safe defaults)
└── README.md
```

**Total backend code:** ~2,400 lines of C# across 14 files. Each `Endpoints/*.cs` is a static class with a
single `Map(...)` method that registers its routes; `Program.cs` calls each one after building the app.

---

## 6. Backend architecture

### 6.1 Design principles

1. **Minimal API, module-per-domain.** No controllers. Each domain area is one static endpoint class. This
   keeps related routes together and the wiring in `Program.cs` explicit and greppable.
2. **Explicit SQL, no ORM.** All persistence goes through `Db.cs` with hand-written SQL that matches the
   schema exactly. Rows come back as `Dictionary<string, object?>`.
3. **Behaviour parity with the Node reference.** The .NET backend is a faithful port of a proven Node/Express
   backend; every route and query mirrors the original (see §23).
4. **Fail safe, degrade gracefully.** No Stripe key → payment endpoints return `503`, everything else works.
   No SMTP → emails print to the console. Missing optional columns are added idempotently at boot.

### 6.2 Boot sequence (`Program.cs`, top to bottom)

1. Build the `WebApplication`.
2. Open the database (`new Db(dbPath)`) and run migrations (`Migrate.Run`) — applies `schema.sql`, then a set
   of idempotent `ALTER`s for columns added after the original schema, then seeds the **bootstrap owner** admin
   (`owner@pci.local` / `changeme-owner`, `must_change_pw = 1`).
3. Configure Stripe key (if `STRIPE_SECRET_KEY` set) and print boot notes for Stripe/SMTP.
4. Register **maintenance-mode middleware** — serves a 503 holding page for public pages when
   `web_maintenance_mode` is enabled; `/api/*` and `/admin.html` are always exempt.
5. Register **CORS** — reflects the request origin, allows `Authorization` and the methods
   `GET, POST, PATCH, DELETE, OPTIONS` (preflight → 204).
6. Register the inline routes defined directly in `Program.cs`: health, content, login, admin auth
   (login/logout/me/password), team & access CRUD (owner-only, with last-owner safeguards), and settings
   (GET + gated PATCH).
7. Register the eight endpoint modules (see §5). `StudentExam.InitScorer(db)` is called before its `Map`.
8. Register static files **last**: `UseDefaultFiles` (index.html) then `UseStaticFiles`.
9. Print the boot banner and call `app.Run()`.

The order matters: DB before routes, module registration before static files, static files before `Run`. This
is enforced and verified.

### 6.3 Request/response conventions

- **Every endpoint returns JSON** via a local `IResult J(object o) => Results.Json(o)` helper, except the CSV
  export (`text/csv`) and the webhook signature-error path (`text/plain`).
- **Errors** are `{ "error": "<machine_code>" }` with an appropriate HTTP status (`400` bad input, `401`
  unauthenticated, `403` forbidden, `404` not found, `503` provider unconfigured). Error codes are stable
  strings (e.g. `no_token`, `bad_status`, `window_lapsed`) that front-ends switch on.
- **Dual-case payloads for the exam client.** Endpoints shared with the WPF client accept and emit both
  `snake_case` (browser) and `PascalCase` (desktop) keys — e.g. `attempt_id`/`AttemptToken`,
  `answers`/`Answers`. This lets one endpoint serve both clients.
- **Request bodies** are parsed with `H.Body(req)` → `Dictionary<string, JsonElement>`; helpers `H.GetS`,
  `H.GetNum`, `H.GetEl` pull typed values and tolerate missing keys.

### 6.4 Type coercion (`Core/H.cs`)

SQLite returns loosely-typed values (INTEGER→`long`, REAL→`double`, TEXT→`string`). `H` centralises coercion so
endpoint code reads cleanly: `H.L(v)` → long, `H.D(v)` → double, `H.Str(v)` → string, `H.B(v)` → bool
(`"1"`, `"true"`, or non-zero). It also holds the exam constants and `Cfg(db)` (see §12), plus ISO time
helpers that reconcile SQLite's `datetime('now')` UTC strings with JavaScript-style millisecond math.

---

## 7. The data-access layer (`Db.cs`)

`Db` is a small, deliberate wrapper over `Microsoft.Data.Sqlite`. Key design points:

- **Single shared connection**, opened once, with **WAL** enabled, guarded by a `lock (_gate)` on every call.
  Appropriate for a single-node back office; serialises writes and avoids locking errors.
- **Placeholder rewriting.** The codebase writes SQL with `?` placeholders (matching the Node original for
  parity). `Db.Bind` rewrites each `?` to Microsoft.Data.Sqlite's `$p0, $p1, …` form in order and binds the
  arguments. (Verified safe: no SQL string contains a literal `?`.)
- **Rows as dictionaries.** `Query` returns `List<Dictionary<string, object?>>`; `QueryOne` returns the first
  row or `null`. Keys are the column names (case-insensitive lookups).

**Public API:**

| Method | Purpose |
|--------|---------|
| `List<Dict> Query(sql, …args)` | Run a SELECT, return all rows. |
| `Dict? QueryOne(sql, …args)` | Run a SELECT, return the first row or `null`. |
| `int Execute(sql, …args)` | Run an INSERT/UPDATE/DELETE, return affected rows. |
| `long ExecuteReturningId(sql, …args)` | Insert and return `last_insert_rowid()`. |
| `(long id, long changes) ExecuteWithChanges(sql, …args)` | Insert and return **both** rowid and change-count from **one** command — used for `INSERT OR IGNORE` idempotency (see §13). |
| `T? Scalar<T>(sql, …args)` | Return a single value coerced to `T` (DBNull-guarded). Money uses `Scalar<double>` to avoid cent truncation. |
| `HashSet<string> Columns(table)` | The column names of a table (used for schema-tolerant dynamic inserts). |
| `void Exec(sql)` | Run raw DDL/PRAGMA. |

**Why `ExecuteWithChanges` exists.** A naïve idempotency check reads `changes()` in a *separate* call after an
insert — but on a shared connection any interleaved statement corrupts that reading. `ExecuteWithChanges`
returns the rowid and `changes()` from a single command, so `INSERT OR IGNORE` duplicate detection is reliable.
This was a real bug found and fixed during review.

---

## 8. Authentication & sessions

There are **two independent identities**: students and admins. Both use opaque bearer tokens whose **SHA-256
hash** is stored server-side (the raw token is never persisted).

### 8.1 Student sessions
- `POST /api/login` with email + password → verifies the BCrypt hash → issues a session token. The SHA-256 of
  the token is stored in `login_tokens` with `purpose = 'session'` and an expiry.
- Requests authenticate with `Authorization: Bearer <token>`. `Auth.UserFromReq` hashes the token, looks it up,
  loads the user, and requires `status = 'active'`.
- **Account setup / password reset** uses one-time tokens in `login_tokens` with `purpose = 'set_password'`
  (`POST /api/set-password`, `POST /api/forgot-password`). Forgot-password never reveals whether an account
  exists.

### 8.2 Admin sessions
- `POST /api/admin/auth/login` with email + password → issues an admin session in `admin_sessions`.
- `Auth.AdminFromReq` accepts **either** a `Bearer <admin-session>` token **or**, for backward compatibility, a
  legacy environment token (`ADMIN_TOKEN`) which resolves to the bootstrap owner (id 0).
- `must_change_pw = 1` is advisory only — it marks an admin still on a seeded/temp password (surfaced by
  `/api/admin/me` and the deploy status endpoint) but never blocks the console. Password changes are
  self-service in Settings → Security.

### 8.3 Context records
`Auth.cs` exposes two immutable records used throughout the endpoints:
- `AdminCtx(Id, Email, Name, Role, PermissionsJson, Status, MustChangePw)`
- `UserCtx(Id, Email, FirstName, LastName, Status)`

---

## 9. Authorisation (RBAC)

Admin authorisation is section-based, defined entirely in `Core/Security.cs` (`Rbac`).

**Sections** are grouped by the app they govern:

| Group | Sections |
|-------|----------|
| `platform` | overview, reports, audit, emails, settings, team |
| `website` | set_web, pricing, codes, content, pages, news, faqs, bok, governance, resources, media, nav, sitesettings, subscribers, submissions, inquiries |
| `student` | set_sp, members, enrollments, payments, credentials, tickets |
| `exam` | set_exam, exams, proctoring, sampleq |

**Built-in roles** (`RoleGrants`):

| Role | Grants |
|------|--------|
| `owner` | **all** sections |
| `website_manager` | all `website` sections + `overview` |
| `student_manager` | all `student` sections + `overview` |
| `exam_manager` | all `exam` sections + `overview` |
| `viewer` | `overview`, `reports` |

`PermsFor(role, permissionsJson)` computes an admin's effective permissions: owners get everything; other roles
get their base grants **plus** any extra sections stored as a JSON array on the admin record (a `custom` role
gets only its explicit list). Endpoints enforce this through a `GateFn(req, section, ok)` delegate: it resolves
the admin, returns `401` if unauthenticated, `403` if the admin lacks the section, otherwise runs the handler.
Some destructive member actions use a "bare admin" check (any authenticated admin) to match the Node reference.

**Team management** (`/api/admin/team*`, owner-only) creates and edits admin accounts, with safeguards that
prevent removing or demoting the **last remaining owner**.

---

## 10. Database schema

SQLite, **42 tables**. `schema.sql` is the source of truth; `Migrate.cs` applies it and then adds a handful of
columns idempotently (for columns introduced after the first schema) and seeds the bootstrap owner. All tables
use an integer primary key `id` unless noted. Times are ISO-8601 / SQLite `datetime` text.

### 10.1 Identity & access
- **`users`** — members. `email, first_name, last_name, password_hash, role, status ('pending'|'active'|'deactivated'), created_at, updated_at`.
- **`student_profiles`** — 1:1 with users. Contact + professional fields: `mobile, country, city, preferred_language, current_role, company, industry_sector, years_experience, highest_qualification, project_controls_area, enrollment_purpose, profile_completion_percentage, linkedin_url, profile_photo`.
- **`admin_users`** — staff. `email, name, password_hash, role, permissions (JSON), status, must_change_pw, last_login_at, created_by`.
- **`admin_sessions`** — admin bearer sessions (`admin_id, token, expires_at`).
- **`login_tokens`** — student session tokens **and** one-time set-password tokens (`user_id, token, purpose, expires_at, used_at`).
- **`login_events`** — student login audit (`ip, user_agent, device, outcome`).
- **`account_requests`** — student-initiated account actions (e.g. data deletion) (`kind, detail, status`).

### 10.2 Enrolment, payments & pricing
- **`enrollment_sessions`** — the multi-step wizard state (`email, current_step, session_status ('in_progress'|'paid'), resume_token_hash, selected_product, pricing_snapshot (JSON), reminders_sent, last_reminder_at, last_activity_at`).
- **`pricing_rules`** — the price book (`currency, product_type ('membership'|'exam'), standard_price, default_discount_percentage, active, start_date, end_date`).
- **`discount_codes`** — codes & referrals (`code, discount_type ('fixed'|'percentage'), discount_value, applies_to ('all'|'membership'|'exam'), start_date, end_date, max_uses, used_count, single_use_per_email, active, code_type ('campaign'|'referral'|…), org_name, owner_user_id, batch_id, per_user_limit, notes`).
- **`code_redemptions`** — one row per redemption (`code_id, code, user_id, email, payment_id, product_type, amount_before, discount_amount, redeemed_at`).
- **`payments`** — settled/attempted payments (`user_id, enrollment_session_id, product_type, standard_amount, default_discount_amount, discount_code, discount_code_amount, final_amount, currency, payment_provider, provider_payment_id, payment_status ('paid'|'failed'|'refunded'), payment_date, invoice_url, receipt_url, reference, exam_schedule_deadline`).
- **`memberships`** — active membership per user (`membership_type, status, start_date, expiry_date, renewal_fee, renewal_cycle, amount_paid, currency`).

### 10.3 Exam & credentials
- **`exam_bookings`** — scheduled sittings (`user_id, payment_id, scheduled_at, timezone, status ('scheduled'|'completed'|'cancelled'|'missed'), reschedule_count`).
- **`exam_attempts`** — attempts (`user_id, booking_id, kind ('exam'|'practice'), violations, started_at, submitted_at, duration_minutes, item_ids (JSON), answers (JSON), score, max_score, percent, result ('pass'|'fail'|'invalidated'), domain_breakdown (JSON), status ('in_progress'|'submitted')`). Migration adds proctoring columns: `identity_result, identity_confidence, evidence_count, review_status, review_note, reviewed_at, client_kind ('browser'|'desktop'), last_heartbeat_at`.
- **`exam_launch_codes`** — single-use desktop launch codes (`code, user_id, booking_id, attempt_id, expires_at, redeemed_at`).
- **`proctor_events`** — flagged events during an attempt (`attempt_id, type, severity ('Info'|'Low'|'High'|'Critical'), detail, evidence_ref, at`).
- **`proctor_messages`** — two-way proctor⇄candidate chat (`attempt_id, sender ('proctor'|'candidate'), body, created_at, delivered_at`).
- **`exam_evidence`** — captured frames/snapshots (`attempt_id, kind, mime, data_uri, note`).
- **`identity_checks`** — AI identity-verification results (`attempt_id, result, confidence, note, face_ref, id_ref`).
- **`issued_credentials`** — awarded credentials (`credential_id (e.g. PCP-AI-2026-12345), user_id, holder_name, credential ('PCP-AI'), status ('active'|'revoked'|'expired'), issued_at, expires_at`).

### 10.4 Support & engagement
- **`tickets`** / **`ticket_messages`** — support tickets and their thread (`reference, subject, category, status ('open'|'awaiting_student'|'resolved'|'closed'), priority`).
- **`cpd_entries`** — continuing professional development log (`activity_date, category, hours, description, status`).
- **`notifications`** — in-portal messages (`category, title, body, cta_label, cta_route, dedupe_key, read_at`).
- **`inquiries`** — public enquiry intake (`type, email, first_name, topic, seats, org, message, reference, status`).
- **`form_submissions`** — generic public form intake (`form_type, name, email, subject, message, reference, status`).
- **`newsletter_subscribers`** — email list (`email, status`).

### 10.5 CMS & content
- **`site_content`** — editable content blocks (`ckey, cgroup, label, ctype, cvalue`).
- **`site_settings`** — key/value platform settings (`skey, svalue`) — see §15.
- **`pages`** — page metadata (`slug, title, meta_description, nav_group, noindex, published`).
- **`nav_items`** — navigation (`label, url, nav_group, sort_order, visible`).
- **`faqs`, `news`, `resources`, `bok_domains`, `sample_questions`, `governance_roles`, `media_assets`** — the
  content types managed by the generic CRUD factory (§11.6). `sample_questions` doubles as the exam item bank
  (`question, options` (newline-separated), `answer_index, domain, published, sort_order`).

### 10.6 Operations
- **`audit_logs`** — every significant action (`user_id, action, details`).
- **`email_logs`** — every email attempt (`email, email_type, subject, status`).

---

## 11. API reference

All routes are under `/api`. Auth column: **—** public · **S** student bearer · **A** admin (any) · **G:section**
admin gated by that RBAC section. Bodies are JSON; responses are JSON unless noted. `123 routes total.`

### 11.1 Public (`Public.cs`, `Program.cs`)
| Method | Path | Auth | Purpose |
|---|---|---|---|
| GET | `/api/health` | — | Liveness: `{ok, service, time}`. |
| GET | `/api/content` | — | Public site content blocks. |
| GET | `/api/pricing` | — | Live pricing for membership/exam/bundle. |
| POST | `/api/validate-code` | — | Validate a discount code against a product; returns computed amounts. |
| GET | `/api/verify?id=` | — | Public credential registry lookup (`{found, …}`). |
| POST | `/api/set-password` | — | Consume a one-time token, set the password. |
| POST | `/api/forgot-password` | — | Request a reset link (never reveals account existence). |
| POST | `/api/inquiry` | — | Public enquiry intake (returns a reference). |
| POST | `/api/newsletter` | — | Subscribe an email. |
| POST | `/api/form-submit` | — | Generic form intake (returns a reference). |

### 11.2 Auth (`Program.cs`)
| Method | Path | Auth | Purpose |
|---|---|---|---|
| POST | `/api/login` | — | Student login → session token. |
| POST | `/api/admin/auth/login` | — | Admin login → admin session. |
| POST | `/api/admin/auth/logout` | A | End the admin session. |
| GET | `/api/admin/me` | A | Current admin + effective permissions. |
| POST | `/api/admin/me/password` | A | Change own admin password (clears `must_change_pw`). |

### 11.3 Student portal (`StudentExam.cs`) — all **S**
`GET /api/me` (dashboard aggregate: profile, membership, payments, exam state, attempts, credentials, tickets,
CPD totals, unread count) · `PATCH /api/me/profile` · `GET /api/me/downloads` · `GET /api/me/practice` ·
`GET /api/me/config` · CPD: `GET/POST /api/me/cpd`, `DELETE /api/me/cpd/{id}` · Messages:
`GET /api/me/messages`, `POST /api/me/messages/{id}/read`, `POST /api/me/messages/read-all` ·
`GET /api/me/security` · `POST /api/me/2fa` · `POST /api/me/sessions/revoke-others` ·
`GET /api/me/account-data` (data export) · `POST /api/me/delete-request` · `GET /api/me/invoices` ·
`GET /api/me/faqs` · Tickets: `GET/POST /api/me/tickets`, `POST /api/me/tickets/{id}/reply`.

**Exam (student side):** `POST /api/me/exam/book` · `POST /api/me/exam/reschedule` ·
`POST /api/me/exam/start` (create-or-resume) · `POST /api/me/exam/submit` (score + issue credential) ·
`GET /api/me/attempts/{id}` · `POST /api/me/exam/heartbeat` (keep-alive + answers + violations + proctor chat) ·
`POST /api/me/exam/launch-code` (mint a desktop launch code).

### 11.4 Secure exam client (`ExamClient.cs`) — all **S**
| Method | Path | Purpose |
|---|---|---|
| POST | `/api/exam/authorize` | Redeem a launch code; create-or-resume the attempt; return exam payload (PascalCase). Single-use except to resume a live attempt. |
| POST | `/api/exam/evidence` | Store a captured frame/snapshot. |
| POST | `/api/exam/identity` | Store an AI identity-check result. |

### 11.5 Admin — students & proctoring
**Students (`AdminStudents.cs`, G:members unless noted):** `GET /api/admin/members` (search/paginate) ·
`GET /api/admin/members/{id}` · `POST /api/admin/members/{id}/status` (A) ·
`POST /api/admin/members/{id}/resend-setup` (A) · `POST /api/admin/members/{id}/referral-code` (A) ·
`GET /api/admin/students/{id}/panel` (full 360 view) · `PATCH /api/admin/students/{id}/profile` ·
`POST /api/admin/students/{id}/cpd` · `DELETE /api/admin/students/{id}/cpd/{cid}` ·
`POST /api/admin/students/{id}/membership` · `POST /api/admin/students/{id}/booking` ·
`POST /api/admin/students/{id}/booking/cancel` · `POST /api/admin/students/{id}/revoke-sessions`.

**Proctoring (`AdminProctoring.cs`, G:proctoring):** `GET /api/admin/exam-sessions` (all attempts) ·
`GET /api/admin/exam-sessions/live` (in-progress with heartbeat-age liveness) ·
`GET /api/admin/exam-sessions/{id}` (full detail: events, evidence, identity, messages, credential) ·
`POST /api/admin/exam-sessions/{id}/message` (message a candidate) ·
`POST /api/admin/exam-sessions/{id}/review` (`invalidate` → revokes credential; `reinstate` → restores) ·
`POST /api/admin/exam-sessions/launch-code` (mint a code for any booking).

### 11.6 Admin — management & CMS (`AdminMgmt.cs`, `AdminExtra.cs`)
**Overview & ops:** `GET /api/admin/overview` (KPIs, revenue series, product mix, funnel, recent activity) ·
`GET /api/admin/reports?from&to` (revenue daily, by product, code performance, top referrers, by country,
funnel) · `GET /api/admin/audit` · `GET /api/admin/emails` · `GET /api/admin/export?entity=` (CSV) ·
`GET /api/admin/abandoned` · `POST /api/admin/resend-resume` · `POST /api/admin/resend-welcome` ·
`POST /api/admin/run-reminders`.

**Commerce:** `GET/POST /api/admin/codes`, `PATCH /api/admin/codes/{id}` · `GET /api/admin/codes-v2` (enriched) ·
`POST /api/admin/codes/generate` (batch) · `GET /api/admin/codes/{id}/redemptions` ·
`GET /api/admin/pricing`, `PATCH /api/admin/pricing/{id}` · `GET /api/admin/payments`,
`GET /api/admin/payments/{id}` · `GET /api/admin/enrollments`, `POST /api/admin/enrollments/{id}/remind` ·
`GET /api/admin/exams`.

**Credentials & inquiries:** `GET/POST /api/admin/credentials`, `POST /api/admin/credentials/{id}/status` ·
`GET /api/admin/inquiries`, `POST /api/admin/inquiries/{id}/status`.

**Tickets (`AdminExtra.cs`):** `GET /api/admin/tickets` (G:tickets), `GET /api/admin/tickets/{id}`,
`POST /api/admin/tickets/{id}/reply`, `POST /api/admin/tickets/{id}/status`.

**Members roster (`AdminExtra.cs`):** `GET /api/admin/students` (flat roster).

**CMS content types — the generic CRUD factory.** One helper registers `GET` (list), `POST` (create),
`PATCH /{id}` (update), `DELETE /{id}` for each of: `faqs, bok_domains, sample_questions, governance_roles,
resources, news, nav_items, media_assets`. Plus page/content/subscriber/form management:
`GET /api/admin/pages`, `PATCH /api/admin/pages/{id}` · `GET /api/admin/content`,
`PATCH /api/admin/content/{id}` · `GET /api/admin/subscribers`, `PATCH /api/admin/subscribers/{id}` ·
`GET /api/admin/form_submissions`, `POST /api/admin/form_submissions/{id}/status`.

### 11.7 Settings (`Program.cs`)
`GET /api/admin/settings` (A) returns all settings. `PATCH /api/admin/settings` accepts a partial object; each
key is gated by prefix — `web_*` needs a website section, `sp_*` the student section, `exam_*` the exam section;
unknown keys are rejected and reported back in `{ok, rejected}`.

### 11.8 Payments (`Payments.cs`)
| Method | Path | Auth | Purpose |
|---|---|---|---|
| POST | `/api/create-checkout-session` | — | Create a Stripe Checkout session (503 if Stripe unconfigured). |
| POST | `/api/webhook` | — (Stripe-signed) | The only place access is granted — see §13. |

---

## 12. The exam pipeline

The exam is delivered end-to-end with power-off/crash safety. Timings come from `H.Cfg(db)` and are
settings-overridable (defaults in brackets): duration `exam_duration_minutes` [90], pass mark
`exam_pass_mark_pct` [65], open window `exam_open_before_minutes` [15] before the slot, grace
`exam_grace_minutes` [30] after, reschedule lock `RESCHED_LOCK_H` [24h], free-reschedule window
`FREE_RESCHED_H` [72h], max reschedules `MAX_RESCHED` [3].

**1 — Entitlement & booking.** A paid `exam`/`bundle` payment is the entitlement (`exam_schedule_deadline`
stamped +12 months at settlement). `POST /api/me/exam/book` validates the slot (≥2h out, within the deadline,
no existing booking) and writes `exam_bookings`.

**2 — Reschedule.** `POST /api/me/exam/reschedule` enforces the 24h lock, the max-reschedule cap, and the slot
rules; ≥72h out is a free reschedule.

**3 — Start (browser).** `POST /api/me/exam/start` opens only within the window; too early → `not_open`, too
late → the booking is marked `missed`. It builds the item set from published `sample_questions` and creates an
`exam_attempts` row — or **resumes** an existing in-progress attempt with its saved answers.

**3′ — Start (desktop).** `POST /api/exam/authorize` redeems a single-use launch code (except to resume a live
attempt), create-or-resumes a `client_kind='desktop'` attempt, and returns the exam payload in PascalCase with
`RemainingSeconds`, `OpensAt/ClosesAt`, and identity/room-scan requirements.

**4 — During the exam.** `POST /api/me/exam/heartbeat` (both clients, dual-case) persists answers, raises the
stored violation count, ingests proctor **events** and candidate **chat**, updates `last_heartbeat_at`, delivers
any queued **proctor→candidate** messages, and returns server time, remaining seconds, and a `ForceSubmit` flag
once the deadline passes. Evidence frames and identity results arrive via `/api/exam/evidence` and
`/api/exam/identity`.

**5 — Submit & score.** `POST /api/me/exam/submit` scores the saved answers against `sample_questions`
(`answer_index`), computes a percentage and a per-domain breakdown (band: `above` ≥80%, `at` ≥ pass mark, else
`below`), marks the attempt `submitted` and the booking `completed`. **On a pass**, a credential
`PCP-AI-<year>-<5 digits>` is minted into `issued_credentials` (active, +3 years). Response is dual-case.

**6 — Proctor review.** From the console, an attempt can be **invalidated** (result → `invalidated`, any issued
credential revoked) or **reinstated** (result recomputed from the percentage, credential restored if it was a
pass). All transitions are audited.

**Crash safety.** Because start/authorize are create-or-resume and every heartbeat persists answers, a client
that dies mid-exam reconnects to the same attempt with its answers and remaining time intact.

---

## 13. Payments (Stripe)

**Principle: payment grants *access*, never the credential.** All money flows through Stripe Checkout; access
is granted **only** in the webhook, and **only** after Stripe's signature is verified.

**Checkout.** `POST /api/create-checkout-session` computes the price (`Public.Pricing`, applying the price book
and any validated code), snapshots it onto the enrolment session, and creates a Stripe Checkout session whose
line item is labelled *"Access only. Certification is awarded separately under PCI policies."* Product metadata
(product type, amounts, code, name, country) rides along for the webhook. Returns the redirect `url`. If
`STRIPE_SECRET_KEY` is unset, returns `503 payments_not_configured`.

**Webhook (`POST /api/webhook`) — the settlement path.** Verifies the signature with
`EventUtility.ConstructEvent` (`STRIPE_WEBHOOK_SECRET`). On `checkout.session.completed`:
1. **Create-or-find the user** by email; new users are created `active` with a profile row; existing users are
   re-activated.
2. **Membership** for `membership`/`bundle` (3-year term).
3. **Record the payment** with `INSERT OR IGNORE` keyed on the provider id, using `ExecuteWithChanges`. If the
   change-count is `0` the delivery is a **replay** and the rest is skipped — this is the idempotency guard
   (§7).
4. **Renewal / recertification** extend the membership / credential expiry respectively.
5. **Discount redemption:** increment `used_count` (bounded by `max_uses`) and insert a `code_redemptions` row.
6. Mark the enrolment session `paid`, mint a one-time set-password token, and — for `exam`/`bundle` — stamp the
   12-month `exam_schedule_deadline`.

`checkout.session.async_payment_failed` / `payment_intent.payment_failed` record a `failed` payment. The
handler uses Stripe's typed `IHasId` (not `dynamic`) so it is compile-safe.

**Email side effects** (payment confirmation, account setup, welcome, exam confirmation) are dispatched by the
Node reference; in `PCI.Backend` these are represented as audit/log points and can be wired to the SMTP sender.

---

## 14. Email system

Twelve responsive HTML templates live in `emails/`: `payment-confirmation, credentials, welcome,
exam-confirmation, reminder-1/2/3, failed, inquiry-general, inquiry-info, inquiry-corporate,
inquiry-partnership`. Every send is recorded in `email_logs` (`email_type`, `subject`, `status`).

**Transport.** With `SMTP_HOST` configured, mail is sent normally; without it, messages print to the console
(so local development and CI never require a mail server). This fallback is deliberate and logged at boot.

---

## 15. Settings & configuration

**Runtime settings** live in `site_settings` (key/value) and are edited via `PATCH /api/admin/settings` with
prefix-based gating. Roughly 38 keys across three prefixes:

- **`web_*`** — public-site behaviour, e.g. `web_maintenance_mode` (drives the 503 holding page).
- **`sp_*`** — student-portal switches, e.g. `sp_login_enabled`, `sp_exam_booking_open`,
  `sp_reschedule_enabled`, `sp_reschedule_cutoff_hours`, `sp_results_visible`, `sp_certificate_download`,
  `sp_cpd_enabled`, `sp_cpd_target_hours`, `sp_support_tickets_enabled`, `sp_practice_enabled`,
  `sp_banner_enabled`, `sp_banner_text`.
- **`exam_*`** — exam engine, e.g. `exam_duration_minutes`, `exam_pass_mark_pct`,
  `exam_open_before_minutes`, `exam_grace_minutes`, `exam_require_identity`, `exam_require_room_scan`.

Read helpers `Settings.Num(db,key,default)` / `Settings.Bool(db,key,default)` fall back to sensible defaults, so
a missing key never breaks behaviour.

**Environment variables** (all optional; see `.env.example`):

| Var | Effect if set / unset |
|-----|-----------------------|
| `PORT` | Listen port (default 8080). |
| `DATABASE_FILE` | SQLite path (default `pci.db`). |
| `STRIPE_SECRET_KEY` | Enables checkout; unset → payment endpoints 503. |
| `STRIPE_WEBHOOK_SECRET` | Verifies webhook signatures. |
| `SMTP_HOST` (+ related) | Enables real email; unset → console output. |
| `APP_BASE_URL` | Base URL used in emails and Stripe redirect URLs. |
| `ADMIN_TOKEN` | Legacy bootstrap admin token (maps to the owner). |

---

## 16. The front-end applications

All are dependency-free HTML/CSS/vanilla JS served from `wwwroot/`; they call the same API with `fetch` and a
bearer token kept in memory.

**Public website** — ~215 pages (home, certification, body of knowledge, governance, chapters, sectors,
knowledge base, policies, blog). Static; reads public content endpoints.

**Student portal (`student.html`)** — the signed-in member experience: exam scheduling, the crash-safe secure
runner, results dial, certificate, CPD log, membership & payments/invoices, messages, and support. Talks to
`/api/me/*` and the exam routes.

**Admin panel (`admin.html`)** — organised into the platform plus three app sections (① Website, ② Student
Panel, ③ Live Exam), each with its own settings screen. Includes the student-360 drawer, the live proctoring
console (auto-refresh), and Team & Access (RBAC) management. The nav is permission-aware: sections the signed-in
admin cannot access are hidden and their routes blocked client-side (the server enforces the same gates).

**Exam preview (`exam-ui.html`)** — a browser rendering of the exam experience for demos/QA.

---

## 17. The secure exam desktop client

A **.NET 8 / WPF** Windows application (shipped separately as `PCI_SecureExam_dotnet.zip`) that delivers the
proctored exam with kiosk lockdown. Solution layout:

- **`PCI.SecureExam.Core`** — portable logic (models, proctoring, scoring seams). Compiles cross-platform.
- **`PCI.SecureExam.App`** — the WPF UI (Windows-only).
- **`PCI.SecureExam.Server`** — a small companion/service piece.
- **`PCI.SecureExam.Tests`** — unit tests.

It authenticates a sitting via a **launch code** minted by the portal or an admin, then drives
`/api/exam/authorize → heartbeat → evidence → identity → submit`, all in the PascalCase contract those endpoints
also speak. An `AiProviderFactory` abstracts identity verification (Baseline / Azure Face / AWS Rekognition
seams) so the AI backend is swappable. Plug-and-play extras: `appsettings.json`, `build.ps1`, a `--selftest`
switch, and CI. (The `Core` project is verified to compile on Linux; the WPF `App` requires Windows to build.)

---

## 18. Local development

**Prerequisites:** .NET 8 SDK. (On Debian/Ubuntu it is available from the OS feed:
`apt-get install -y dotnet-sdk-8.0`.)

```bash
# from the unzipped PCI.Backend/
cp .env.example .env         # optional — every value has a working default
dotnet run                   # builds, restores, and serves on http://localhost:8080
```

Open:
- `http://localhost:8080/` — website
- `http://localhost:8080/student.html` — student portal
- `http://localhost:8080/admin.html` — admin panel
- `http://localhost:8080/exam-ui.html` — exam preview

**First admin sign-in:** `owner@pci.local` / `changeme-owner` (a password change is forced on first login).
Stripe and SMTP are optional; without them, checkout returns 503 and emails print to the console. The SQLite
file and schema are created automatically on first run.

---

## 19. Testing & CI

**Smoke suite (`smoke-test.sh`).** 46 live HTTP checks covering health, auth, RBAC gating (e.g. an exam manager
is `403` on payments), the student/exam surface, CMS CRUD, admin management, public forms, and the
checkout-503-without-a-key path. It runs against a booted instance.

**CI (`.github/workflows/build.yml`).** On every push/PR, GitHub Actions: checks out, sets up .NET 8,
`dotnet build -c Release` (restoring the three NuGet packages), boots the backend, waits for `/api/health`, then
runs the smoke suite. This is the authoritative build-and-run proof.

**Local verification without full network.** The backend has been compiled and booted with a real .NET 8 SDK in
a restricted environment by stubbing the three external packages; `/api/health` returned `200`. For a full data
round-trip, run it where `api.nuget.org` is reachable (or CI).

---

## 20. Deployment

The whole platform is one self-contained process plus a SQLite file.

1. **Build:** `dotnet publish -c Release -o out` produces a runnable app in `out/` (includes `wwwroot/`,
   `emails/`, `schema.sql`).
2. **Configure:** set the environment variables from §15 (at minimum `APP_BASE_URL`; add Stripe/SMTP keys for
   live payments/email).
3. **Run behind TLS:** put a reverse proxy (nginx/Caddy) in front for HTTPS; proxy to the app's `PORT`.
   Configure Stripe to send webhooks to `https://<host>/api/webhook` and set `STRIPE_WEBHOOK_SECRET`.
4. **Persistence:** back up the SQLite file (`DATABASE_FILE`). WAL means also capturing `-wal`/`-shm` at
   checkpoint, or take backups via SQLite's backup API.
5. **Scaling:** this is a single-node design. Vertical scaling and a read replica strategy fit the certification
   back-office workload; a move to Postgres would be the path if multi-node writes are ever needed (the SQL is
   deliberately close to portable).

A container image is straightforward: base on `mcr.microsoft.com/dotnet/aspnet:8.0`, copy `out/`, expose the
port, set env, `ENTRYPOINT ["dotnet","PCI.Backend.dll"]`.

---

## 21. Security considerations

- **Token storage:** only SHA-256 hashes of session/one-time tokens are stored; raw tokens never touch the DB.
- **Passwords:** BCrypt (work factor per BCrypt.Net defaults).
- **Payment integrity:** access is granted solely in the signature-verified webhook, with idempotent settlement
  (§13) so retried deliveries cannot double-grant.
- **Account enumeration:** forgot-password always returns success regardless of whether the email exists.
- **RBAC everywhere:** admin routes are gated server-side by section; the client's permission-aware nav is a
  convenience, not the control.
- **SQL injection:** all user data is bound as parameters (the `?`→`$pN` rewrite binds every value; no string
  concatenation of user input into SQL).
- **CSV export safety:** values beginning with `= + - @` are prefixed to defuse spreadsheet formula injection.
- **Exam integrity:** single-use launch codes, identity checks, evidence capture, violation counting, and proctor
  review with credential revocation.
- **Maintenance mode** keeps `/api/*` and the admin panel available while showing the public a holding page.

**Hardening backlog (production):** add rate limiting on auth/enrolment endpoints, rotate the legacy
`ADMIN_TOKEN` out in favour of admin sessions only, move evidence blobs from inline data URIs to object storage,
and add security headers/HSTS at the proxy.

---

## 22. How to extend the platform

**Add a new API route.** Pick the matching module in `Endpoints/` (or add a new static class with a
`Map(WebApplication, Db, …)` method and call it from `Program.cs`). Follow the conventions: parse with
`H.Body`, coerce with `H.*`, return via the local `J(...)`, use stable `{error}` codes, and gate admin routes
with `GateFn(req, "<section>", ok)`.

**Add a managed content type.** If it is simple CRUD, add one `Crud("table", new[]{cols…}, "order")` line in
`AdminMgmt.cs` and a matching table in `schema.sql`. That yields list/create/update/delete immediately.

**Add a setting.** Choose the right prefix (`web_`/`sp_`/`exam_`) so gating and the correct admin settings
screen pick it up; read it with `Settings.Num/Bool` with a default.

**Add a column.** Add it to `schema.sql` for fresh installs **and** add an idempotent `ALTER` in `Migrate.cs`
so existing databases upgrade on boot.

**Change pricing/discounts.** Edit `pricing_rules` / `discount_codes` data — never hard-code amounts. The
pricing engine (`Public.Pricing`) and `validate-code` recompute from the tables.

**Keep parity.** When changing behaviour, mirror it in the Node reference (or consciously diverge and note it),
since the two backends are intended to be behaviourally identical.

---

## 23. Appendix: the Node reference backend

The .NET backend is a faithful port of a **Node.js / Express** backend (`pci-enrollment-backend/`, ESM,
`better-sqlite3`, `bcryptjs`) that was built and run first and passed a live HTTP suite. It remains in the
repository as the reference implementation and a cross-check: the two expose the **same 123 routes**, the
**same schema**, and the same behaviour. During the port, verification was done by route-parity audit, SQL
validity against the real schema, and — once a .NET SDK was available — an actual compile + boot. Notable bugs
found and fixed in review (in **both** implementations where applicable): a write to a non-existent
`student_profiles.updated_at` column, and the webhook idempotency reading (`ExecuteWithChanges`). Prefer
`PCI.Backend` for all new work; consult the Node version only as a behavioural reference.

---

*End of guide.*
