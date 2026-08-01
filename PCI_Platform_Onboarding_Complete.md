# PCI Platform — Complete Module-Wise Onboarding Guide

**Single source of truth for onboarding new developers and AI models to the PCI Platform.**
Covers every major module — Project Controls Institute (public website), Student Portal, Admin Portal,
Examination & Certification Engine, SecureExam desktop client, Payments & Commerce, PCI World,
PCI Global, Certuvo, Communications & Marketing, Casework & Compliance, and the Simulation Lab —
from business, functional, development, and architectural perspectives.

> Companion documents: `CLAUDE.md` (AI-assistant working conventions), `PCIOnboarding.md` (quick-start
> developer guide), `DEPLOY.md` (deployment), `docs/` (historical archive). This document is the deep,
> consolidated reference.

---

# Table of Contents

1. [Executive Overview — What the PCI Platform Is](#1-executive-overview)
2. [Brand, Product & Domain Map](#2-brand-product--domain-map)
3. [Platform Architecture (System-Wide)](#3-platform-architecture)
4. [Module 1 — Project Controls Institute: Public Website & Content System](#module-1)
5. [Module 2 — Student Portal (MyPCI)](#module-2)
6. [Module 3 — Examination & Certification Engine](#module-3)
7. [Module 4 — SecureExam Desktop Client](#module-4)
8. [Module 5 — Admin Portal (Operator Console)](#module-5)
9. [Module 6 — Payments, Pricing & Commerce](#module-6)
10. [Module 7 — PCI World](#module-7)
11. [Module 8 — PCI Global](#module-8)
12. [Module 9 — Certuvo (External Practice Platform)](#module-9)
13. [Module 10 — Communications, Content & Marketing Centre](#module-10)
14. [Module 11 — Casework, CPD, Identity & Compliance](#module-11)
15. [Module 12 — AI Project Controls Simulation Lab](#module-12)
16. [Database Reference — All Domains & Key Entities](#16-database-reference)
17. [Security Architecture & Cross-Cutting Services](#17-security-architecture)
18. [User Roles & Permissions — Master Reference](#18-roles--permissions)
19. [Configuration & Environment Variables](#19-configuration)
20. [Deployment Topology](#20-deployment)
21. [Testing & CI](#21-testing--ci)
22. [Known Documentation Drift & Gotchas](#22-gotchas)
23. [Glossary & Where-To-Look Index](#23-glossary)

---

<a name="1-executive-overview"></a>
# 1. Executive Overview — What the PCI Platform Is

## 1.1 The organisation

- **Legal name:** Project Controls Institute Global, Inc. (alternate name **PCI**)
- **Status:** an independent certifying body — a Delaware Non-Stock Corporation and registered
  nonprofit *pursuing* 501(c)(3) recognition (not yet granted; the site states this honestly)
- **Mission:** "An independent home for a discipline that didn't have one" — to certify the
  integrated discipline of **project controls, cost engineering and project finance**, to a single,
  modern, AI-ready standard. Slogan: *"AI proposes. The professional disposes."*
- **Founded:** 2025 · serves worldwide · contact `hello@projectcontrolsinstitute.org`
- **Accreditation posture:** designed around the **ISO/IEC 17024** framework; PCI is *building
  toward* formal accreditation and never claims recognition it does not hold.

## 1.2 The software estate — one system, many surfaces

The entire estate is **one ASP.NET Core 8 backend + one database + one Docker image**, serving:

| Surface | URL | What it is |
|---|---|---|
| Public website (~235 pages) | `/` | SEO-critical marketing/info site — server-rendered static HTML with DB-driven content injection; 100% admin-editable without redeploying |
| Student Portal (React) | `/app/` | The candidate journey: certifications, exams, credentials, CPD, billing, documents, appeals, Simulation Lab |
| Classic student panel | `/student.html` | Legacy single-file panel; self-retires to `/app/` except for token-carrying support/exam-launch URLs |
| Admin Portal (React) | `/admin/` | ~70-section operator console with granular RBAC (the classic `admin.html` is retired and 301s here) |
| Exam interface preview | `/exam-ui.html` | Static, no-API preview of the secure exam client UI |
| PCI World (React + SSR) | `/world`, `/world-app/`, `/world-admin` | Free global challenge/community platform — an isolated module with its own auth realm |
| Institution Partner Portal | `/partner.html` + `/api/partner/*` | Training-partner/institution self-service (codes, sponsorships, finance) |
| SecureExam desktop client | `secureexam/` (Windows WPF) | Downloadable proctored kiosk exam client, pinned to PCI API hosts, launched with single-use codes |

Content edited in the Admin Portal appears on the website; everything students do appears in the
Admin Portal. There is nothing to "wire together" — one backend (~160+ HTTP endpoints across 71
endpoint modules), one database (~194 tables), one deployment.

## 1.3 The certifications (the core product)

Seeded and managed in the `certifications` table (`backend/Data/MultiCert.cs`):

| id | Code | Name | Post-nominal | Scope |
|---|---|---|---|---|
| 1 | **PCL-AI** | PCI AI Project Controls Leader | PCI PCL-AI | Flagship: planning, cost engineering, EVM, forecasting, risk, project finance + governed AI (13 BoK domains; the AI domain carries 20% of the exam) |
| 2 | **PFL-AI** | PCI AI Project Finance Leader | PCI PFL-AI | Project finance, financial modelling, capital structure, bankability, PPP, financial close |
| 3 | **PML-AI** | PCI Project Management Leader – AI | PCI PML-AI | Comprehensive PM/leadership/delivery incl. agile/hybrid and AI-enabled PM |

`PCP-AI` ("Certified Project Controls Professional — AI") is the legacy seeded code — renamed to
PCL-AI but still present in older docs and the SecureExam README. Legacy URL slugs 301 forward
(`pcp-ai → pcl-ai`, `pfip → pfl-ai`, `cpmd/pdl-ai → pml-ai`).

**Key exam facts (PCL-AI):** scenario-based MCQs only · proctored (online or test centre) ·
eligibility 3 years' experience · pass mark 65% (per-certification override supported) ·
90 minutes (override supported) · credential valid 3 years · recertification via a 3-year CPD
cycle with an AI-currency component.

**Fees (USD, one-time):** Exam $500 → $350 (30% launch discount) · Student membership $99/yr →
$49.50 (50%) · Membership+Exam bundle $399.50 · Renewal/Recertification $99 per 3-year cycle.
All prices are DB-driven (`pricing_rules`) and injected live into the website.

**Three routes into PCI** (the examined credential is always earned by passing the exam):
1. **Standard** — open enrolment → student membership → exam fee → sit the exam.
2. **Founding member** — limited invitation code grants membership + study + exam access together
   (`Endpoints/Founding.cs`); the credential is still earned by examination.
3. **Honorary Fellow (PCI)** — board-conferred, **no examination**; lives in a completely separate
   `PCI-HON-…` number space and is never represented as a passed exam.

**Membership grades:** Student → Associate (APCI) → Professional (MPCI) → Fellow (FPCI), with
self-service and board-review upgrade paths (`Core/MembershipGrades.cs`).

## 1.4 The candidate journey (canonical business workflow)

Eight stages (see `wwwroot/candidate-journey.html`), mapped to platform surfaces:

```
1. Explore          → public website, certification pages, knowledge hub
2. Check eligibility→ eligibility pages/policies
3. Prepare          → Certuvo (official external practice platform), BoK, sample questions
4. Apply            → /app/register → enrolment wizard → Stripe checkout (or offline settlement)
5. Sit the exam     → book a slot → browser runner OR SecureExam desktop client OR external vendor
6. Decision         → server-side scoring → result lifecycle (publish / hold / review)
7. Credential       → verifiable PDF certificate + Open Badge + public /api/verify register
8. Maintain         → CPD logging + annual declaration → 3-year renewal / recertification
```

---

<a name="2-brand-product--domain-map"></a>
# 2. Brand, Product & Domain Map

A frequent source of confusion — these are the *facts as the repository states them*:

| Name / Domain | What it actually is |
|---|---|
| **Project Controls Institute** (`projectcontrolsinstitute.org`) | The organisation and the **canonical public host** (non-www). All web surfaces live here on the main deployment. |
| **PCI** | The institute's short name / brand. |
| **PCI Global** / `pci-global.org` | **Not a separate product or site.** It is a pinned *alternate API host* in the SecureExam desktop client's HTTPS allowlist (`secureexam/PCI.SecureExam.Core/ClientConfig.cs`), reserved for regional/staging exam APIs (e.g. `staging.pci-global.org`, `eu.pci-global.org`). "PCI Global" also appears as an email display name and the legal entity is "Project Controls Institute Global, Inc.". See [Module 8](#module-8). |
| `pciglobal.ai` | A **retired domain** that 301-redirects page-to-page to the canonical host (`Core/Redirects.cs`). |
| **MyPCI** / `mypci.org` | The optional **portal domain** for student/admin surfaces (noindex; `PORTAL_BASE_URL`/`PORTAL_HOSTS`, `Core/PortalDomain.cs`). |
| `exam.projectcontrolsinstitute.org` | Default SecureExam API base URL. |
| **PCI World** / `pciworld.org`, `admin.pciworld.org` | The free global challenge/community platform — an isolated module in the same codebase, deployable standalone. Tagline: *"Make the decision. Control the outcome."* See [Module 7](#module-7). |
| **Certuvo** | PCI's **official but external** study/practice platform. PCI provisions accounts into it; PCI remains the system of record. See [Module 9](#module-9). |
| **PCI Project Intelligence** | The premium practice programme inside PCI World (*"Think. Decide. Deliver."*). |
| `holding/` | A standalone static "coming soon" page for a static host (Netlify/CF Pages), fully decoupled from the .NET app. |

---

<a name="3-platform-architecture"></a>
# 3. Platform Architecture (System-Wide)

## 3.1 Technology stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 8 **Minimal API** (no MVC controllers), C#, `net8.0`, Nullable + ImplicitUsings, InvariantGlobalization |
| Backend packages | `Microsoft.Data.Sqlite`, `MySqlConnector`, `BCrypt.Net-Next`, `Stripe.net`, `AWSSDK.S3` |
| Database | SQLite (default; source-of-truth dialect) **or** MySQL 8 / MariaDB 10.11 via runtime SQL translation — one code path, dual provider |
| Frontend | React 18 + TypeScript + Vite; deps deliberately tiny: `react`, `react-dom`, `react-router-dom` only (no UI/state/data library; hand-rolled SVG charts) |
| Desktop client | .NET 8 WPF (`net8.0-windows`) + OpenCvSharp4 + NAudio + SignalR client |
| Realtime | SignalR (community hub at `/api/world/hubs/community`; optional proctor hub in the SecureExam reference server) |
| Tests | Python logic/integration/sweep suites (real SQLite/MySQL), .NET xUnit (backend + SecureExam Core), Vitest + Playwright (frontend), `smoke-test.sh` |
| Deployment | Multi-stage Docker (React builds → .NET publish → runtime); Render Blueprint (`render.yaml`); `/data` persistent disk |

## 3.2 Repository layout

```
PCI/
├── backend/            The whole platform's server
│   ├── Program.cs      Boot, middleware pipeline (2,124 lines), inline endpoints, module wiring
│   ├── Core/           143 cross-cutting service files (auth, RBAC, storage, CMS, mailer, …)
│   ├── Endpoints/      71 feature endpoint modules (~160+ HTTP endpoints, 27,758 lines)
│   ├── Data/           Db.cs (dual-provider), Migrate.cs, schema installers, seed packs
│   ├── wwwroot/        235 static .html pages + classic panels + assets
│   ├── emails/         12 transactional HTML email templates
│   ├── books/          PML-AI and PFL-AI Bodies of Knowledge (md + pdf)
│   ├── tests/          Python suites + 350-file .NET test project + smoke-test.sh
│   ├── schema.sql      SQLite schema — SOURCE OF TRUTH
│   └── schema.mysql.sql  Generated MySQL twin (CI fails on drift)
├── frontend/           FOUR Vite apps: student (/app/), admin (/admin/),
│                       world (/world-app/), worldadmin (/world-admin-app/)
├── secureexam/         .NET 8 solution: Core, WPF App, reference Server, Tests, RunnableChecks
├── PCIWorld/           Dockerfile + README for a PCI-World-only deployment
├── holding/            Static coming-soon page (index.html + _redirects)
├── docs/               Historical handoff archive (background only — NOT the current file map)
├── Dockerfile          Full-platform multi-stage build
├── render.yaml         Render Blueprint (web service + 5 GB disk at /data)
└── .github/workflows/build.yml   CI (5 jobs)
```

## 3.3 The middleware pipeline (exact order — `backend/Program.cs`)

Order matters; every response passes through the outer layers:

1. **Branded 404** (`UseStatusCodePages` → `wwwroot/404.html` for HTML-ish GETs)
2. **Error-reference net** — uncaught `/api` exceptions become 500 JSON with a student-quotable
   `PCI-YYYY-NNNNNN` reference (`Core/ErrorRefs.cs`)
3. **Canonical-domain / HTTPS 301** (`Core/Redirects.cs` — www, retired `pciglobal.ai`)
4. **Security headers + CSP** — nosniff, `Referrer-Policy`, `X-Frame-Options: DENY`, COOP,
   Permissions-Policy, scoped CSP (report-only via `CSP_REPORT_ONLY`), `X-Robots-Tag: noindex` on
   private paths, HSTS when the first `X-Forwarded-Proto` hop is https
5. **CORS** — reflects `ALLOWED_ORIGIN` (and the portal domain); 204 preflight short-circuit
6. **Portal-domain separation** (when `mypci.org` configured) — 308 redirects both ways
7. **PCI World host mapping** (when configured) — maps `pciworld.org` hosts; `PCIWORLD_ONLY`
   enforces a strict path allowlist with real 404s
8. **Rate limiter** — fixed window, 10 POSTs / 60 s per IP+path on 28 brute-forceable paths,
   keyed on the **last** `X-Forwarded-For` hop; 429 + `Retry-After`
9. **Boot config validation** — in production the app **exits 78** on unsafe config (see §19)
10. **Maintenance mode** — 503 holding page for public pages; `/api/*` and `/admin` stay up
11. **Impersonation read-only guard** — any write carrying an impersonation token → 403
12. **All endpoints** (inline + 71 modules)
13. **Dynamic content injection** (Stage 2 CMS) — the page-render pipeline (see Module 1)
14. **Static files** with per-type cache policy (immutable hashed assets; no-cache HTML)
15. **React SPA fallback** (terminal) — `/app`, `/admin`, `/world-app` shells for extension-less GETs

12 hosted background services run alongside: `RetentionService`, `IntegrationDispatcher`,
`OutboxDispatcher`, `SocialDispatcher`, `ScheduledPublisher`, `SyndicationDispatcher`,
`MarketingJobDispatcher`, `ExamDeliveryDispatcher`, `CommsReminderService`, `WorldRotationService`,
`WorldRetentionService`, `CommunityBroadcastService`.

## 3.4 The data layer — `backend/Data/Db.cs` (dual-provider)

**All DB access goes through the shared `Db` singleton.** Rules every developer must internalise:

- SQL is written in **SQLite dialect — the source of truth**. With `DB_PROVIDER=mysql`,
  `Db.Translate` rewrites it at runtime (datetime math, upserts, `last_insert_rowid()`/`changes()`,
  `julianday`, `strftime`, partial-unique-index stripping, MySQL-8 vs MariaDB quirks). ~430 call
  sites stay provider-agnostic.
- **Datetimes are strings** `YYYY-MM-DD HH:MM:SS` (UTC) on both providers. Compare instants via
  `H.JsMillis` / `H.IsPast` / `H.After` — never lexically (`' '` 0x20 sorts before `'T'` 0x54;
  a lexical compare once made every exam launch code appear expired).
- Parameters are positional `?` (rewritten to `@p0…`); **always parameterise**.
- API surface: `Query`/`QueryOne` (case-insensitive dictionary rows), `Scalar<T>`, `Execute`,
  `ExecuteReturningId`, `ExecuteWithChanges` (atomic id+changes — the webhook idempotency primitive),
  `Transaction(Action)`, `WithMigrationLock`, `WithRetryOnLockFailure` (idempotent work only),
  `Columns(table)`, `AddColumn` (the single additive-migration primitive).
- Read columns through `H.L` / `H.D` / `H.Str` / `H.B` coercion helpers.
- Money is `DECIMAL(12,2)`; `Migrate.EnsureMoneyDecimals` converges 44 columns on MySQL and a
  non-local deployment **refuses to start** with inexact money columns.

## 3.5 Migrations — `backend/Data/Migrate.cs`

Runs on every boot under a cross-instance migration lock and is **idempotent**:
`schema_migrations` ledger with `SchemaVersion` compatibility gate (an older binary refuses to boot
against a newer schema — exit 75), schema-checksum drift detection (warns, never fails), then
`Converge()` = execute `schema.sql` + 127 `CREATE TABLE IF NOT EXISTS` + 225 guarded `AddCol`
upgrades + seeds (bootstrap owner admin, demo student, content — all `INSERT OR IGNORE`/if-empty
so operator edits are never overwritten). Six runtime schema installers follow (Comms, Marketing,
SimLab, Templates, World, Finance), each individually fault-isolated.

**When you change the schema:** 1) edit `schema.sql`; 2) add the idempotent upgrade in
`Migrate.cs`; 3) regenerate `schema.mysql.sql` (`python3 tools/sqlite_to_mysql.py`); 4) run the
Python suites (and ideally the MySQL integration run).

---

<a name="module-1"></a>
# Module 1 — Project Controls Institute: Public Website & Content System

## 1. Overview & business purpose
The ~235-page public website is the institute's shop window and SEO engine: certifications,
membership, policies (~30 governance/ISO pages), knowledge hub, blog, chapters (26 country pages),
sectors (11), events, careers, verification. It must rank, load fast, work with JS off, and be
**100% editable by non-technical staff without a redeploy**.

## 2. Functional workflows & features
- Every page is served through the dynamic-content pipeline yet costs nothing when unedited
  (byte-identical fall-through to static files).
- Admin edits (Pages & content, Site content, FAQs, BoK, news, nav, pricing…) appear on the live
  site immediately — render caches are version-bumped on every save.
- Site-wide features: cookie-consent banner, ⌘K on-site search (dynamic `search-index.json`),
  newsletter band, downloads centre, per-page SEO/JSON-LD, announcement banner, live-chat widget.
- i18n: 7 languages (en, ko, ar, es, fr, zh, ru) via `content_i18n`; English serves byte-identical.
- Blog/news CMS with editorial workflow, RSS/Atom/JSON feeds, sitemaps, IndexNow pings.
- Public forms: contact/inquiry, newsletter, generic form capture, honorary application,
  training-partner application, appeals/complaints/accommodation forms.
- Credential verification: `/api/verify` public register (see Module 3).

## 3. Development & implementation
The content system has three cooperating layers (all in `backend/Core/`):

1. **`PageScan.cs` — universal capture.** At boot, every visible text region of every page
   (h1–h6, p, li, td…, plus `placeholder`/`aria-label`) is captured into `page_blocks`, keyed by a
   hash of the original content (stable across restarts). A region appearing verbatim on ≥20 pages
   becomes ONE shared `site_content` binding.
2. **`PageContent.cs` — injection engine.** On a page GET with overrides: title/meta/OG/canonical/
   noindex overrides, positional `_h1` headline, `data-cms` regions. Skips app shells
   (`student.html`, `admin.html`, `exam-ui.html`, `partner.html`).
3. **Marker-driven table sections — `ListSections.cs` / `CertCatalogue.cs` / `PriceTags.cs`.**
   Regions wrapped in `<!--PCI-X-->` markers are replaced server-side from admin tables:
   `PCI-NAV-HEADER/FOOTER`, `PCI-FAQS`, `PCI-BOK`, `PCI-GOVERNANCE`, `PCI-RESOURCES`, `PCI-NEWS`,
   `PCI-PARTNERS`, `PCI-SOCIAL`, and `PCI-CERTS` (live certification cards with prices, on 11
   pages). `data-price="exam.final"`-style tokens are replaced from the *same pricing engine
   checkout charges* (`Public.Pricing`), so the site can never advertise a price checkout won't honour.

Full render chain (GET only): `PageContent.Render` → `I18nContent.Render` → `CertCatalogue.Inject`
→ `ListSections.Inject` → `PriceTags.Inject` → `SeoTags.Inject` → `PortalDomain.RewriteLinks` →
`Analytics.PageView`. Each injector caches on its own version counter and `Bump()`s on change.

**Client-side fallback:** `assets/cms-loader.js` (3.6 KB, deferred) hydrates `[data-cms]`, the
announcement banner and the newsletter form from `/api/content` with a hard 3-second timeout —
backend down ⇒ the shipped HTML is what the visitor sees. The newsletter binds in the capture
phase *before* any fetch so Subscribe is never a no-op.

## 4. Architecture & design decisions
- **Server-side injection, not client-side hydration**, for SEO and JS-off correctness.
- Pages with no overrides pay nothing (fall through to static file serving).
- Sanitisation: admin-authored rich text passes through `HtmlSanitize.Clean` (41-tag allowlist,
  no h1/script/iframe/form; `javascript:`/`data:` URLs blocked; `target=_blank` gets `noopener`).
- SEO machinery: managed 301 redirects (single-hop enforced at write time), dynamic sitemaps,
  `llms.txt` + AI-crawler policy (`Core/AiVisibility.cs`), cookieless first-party analytics
  (daily-rotating visitor hash, never raw IPs).

## 5. Dependencies & integrations
Depends on: `site_settings`, `pages`, `page_blocks`, `site_content`, `content_i18n`, `faqs`,
`bok_domains`, `resources`, `news`, `nav_items`, `media_assets`, `certifications`, `pricing_rules`,
`social_accounts`. Feeds: search engines (sitemaps/IndexNow), GA4/GTM/Clarity (consent-gated tags).

## 6. Database
`pages` (slug, title, meta, noindex, canonical, og) · `page_blocks` (slug+block_key → value) ·
`site_content` (global keys) · `content_i18n` (lang/scope/slug/ckey) · `seo_redirects` ·
`seo_submissions` · `analytics_events` · blog model (`blog_posts` + versions/reviews/authors/
categories/tags — versions are never overwritten) · `newsletter_subscribers` · `form_submissions`.

## 7. APIs
Public: `GET /api/content`, `GET /api/page-content?slug=`, `GET /api/pricing`, `GET
/api/certifications`, `GET /api/verify`, `POST /api/inquiry|newsletter|form-submit`, `GET
/api/reviews`. Admin: `/api/admin/pages`, `/page-blocks`, `/content`, plus the CRUD factory
collections (Module 5). Dynamic routes: `/sitemap.xml`, `/robots.txt`, `/llms.txt`,
`/search-index.json`, `/blog*`, `/news*`, `/certifications*`, `/downloads*`.

## 8. Roles & permissions
Admin sections: `pages`, `content`, `faqs`, `bok`, `governance`, `resources`, `news`, `nav`,
`media`, `sitesettings`, `set_web`, `subscribers`, `submissions`, `inquiries`; blog/content-centre
has its own `cc_*` permission family; translations are owner-only.

## 9. Tech stack & structure
Static HTML + vanilla JS (`assets/styles.css` 88 KB, `premium.js`, `chat.js`, `cms-loader.js`),
self-hosted fonts, `images/` photography, `downloads/` 8 real PDFs. Static-host redirect configs
(`_redirects`, `netlify.toml`, `vercel.json`) are kept for non-backend deploys; `generate.py`
regenerates shared chrome.

---

<a name="module-2"></a>
# Module 2 — Student Portal (MyPCI)

## 1. Overview & business purpose
The authenticated candidate/member experience: everything from first registration through exam
booking, results, credentials, CPD, billing, documents, support, and privacy self-service. The
React app at `/app/` is the primary surface; the classic `student.html` panel self-retires to it
except when carrying a support/exam-launch token.

## 2. Functional workflows & features (all screens)
Routing in `frontend/src/App.tsx` (basename `/app`), sidebar in `components/Layout.tsx`:

| Route | Purpose |
|---|---|
| `/login`, `/register` | Email+password (+TOTP, Google sign-in); registration wizard |
| `/onboarding` | Full-screen first-run profile wizard |
| `/` Overview | Dashboard: visual candidate-journey, KPI tiles, action-needed callouts |
| `/certifications` | Per-certification entitlements, booking/rescheduling, eligibility holds (actionable vs state-only), identity-document upload, exam-day incident reporting |
| `/credentials` | Issued credentials, certificate PDF download, verify link, Open Badge, LinkedIn share |
| `/cpd` | CPD log + evidence upload, progress to target, **annual declaration** ("declared, not discovered": compliant / career_break / not_met) |
| `/certuvo` | Access card for the external Certuvo practice platform (credentials resend) |
| `/lab`, `/lab/:code` | AI Project Controls Simulation Lab (Module 12) |
| `/billing` | Payments/receipts, pricing + discount codes, Stripe checkout, pay-to-extend, membership grade progression (APCI/MPCI/FPCI), optional recurring dues |
| `/resources`, `/templates`, `/documents` | Member downloads; branded XLSX template library; private assigned documents + certification books (acknowledge flows, watermarked personalised copies) |
| `/events`, `/event-passes` | CPD-earning webinars/events (attendance auto-credits CPD); event-pass wallet (backend seam not yet live — renders a designed "not yet enabled" state) |
| `/messages`, `/support`, `/appeals` | Notifications; support tickets; appeals & accommodations (Module 11) |
| `/applications` | Careers applications status |
| `/profile` | Profile editing, opt-in member directory with per-field visibility, TOTP 2FA self-enrolment, session revocation, GDPR data export + erasure request, optional-comms consent |
| (embedded) | PCI World Passport summary + SSO handoff into World |

## 3. Development & implementation
- **Data layer:** `MeContext` loads the `/api/me` mega-aggregate once; pages read via `useMe()`.
- **API client** (`src/api/client.ts`): `makeClient(tokenKey)` factory; student key
  `pci.session.token` in `sessionStorage` (deliberate — clears on tab close for shared machines)
  with in-memory fallback; central `onUnauthorized` hook (fires on every 401 incl. mutations);
  boot retry 3× with backoff so a transient 5xx doesn't bounce a valid session to login;
  `humanize()` maps backend error codes to friendly copy.
- **Fragment handoffs** (scrubbed from the URL before any other work): `#world-code=` (90-second
  one-time PCI World → portal SSO code — in the fragment so it never reaches server logs) and
  `#t=<token>` (admin support-view/impersonation; drives a permanent staff-support banner).
- **Demo mode** (`src/demo/`): a full walkable product with no backend; permanent banner; never
  pre-empts a working backend; writes never persist.
- Backend module: `Endpoints/StudentExam.cs` (1,260 lines) is the whole `/api/me/*` surface;
  `Endpoints/Account.cs` handles registration/Google sign-in/profile wizard.

## 4. Architecture & design
- Student and admin are **separate bundles, separate token keys, separate auth realms** — admin
  code never ships in the student bundle.
- Every mutating `/api/me/*` route re-checks impersonation and returns 403 `impersonation_readonly`
  (server-enforced read-only support view; ledgered in `impersonation_sessions/_events`).
- Held results are **redacted server-side** at every surface (see Module 3).
- Eligibility is centralised in `Core/Lifecycle.cs`: `BookingBlockers` (fee, entitlement expiry,
  profile country, identity document, 7 required consents, account hold, booking closed) and the
  post-booking-mutable subset `LaunchBlockers` re-checked at launch.

## 5. Dependencies & integrations
Stripe checkout (Module 6) · Certuvo provisioning (Module 9) · SecureExam launch codes (Module 4)
· PCI World passport/SSO (Module 7) · comms outbox for every notification/email.

## 6. Database (primary)
`users`, `student_profiles`, `login_tokens` (session 30-day; also `set_password`, `impersonation`
1 h, `portal_handoff` 90 s), `login_events`, `candidate_consents` (7 types, versioned),
`identity_documents`, `exam_*` (Module 3), `payments`/`memberships` (Module 6), `cpd_entries`,
`cpd_declarations`, `notifications`, `tickets`, `erasure_requests`.

## 7. APIs (`/api/me/*` highlights)
`GET /api/me` (aggregate) · profile PATCH (12-field allowlist) · consents GET/POST ·
identity-document GET/POST/file · exam book/reschedule/start/heartbeat/submit/launch-code ·
attempts/results (redacted when held) · readiness · cpd GET/POST/DELETE + declaration ·
membership/upgrade · preferences · messages · security (2FA setup/verify/disable, revoke-others) ·
account-data export · delete-request · invoices + receipt PDF · tickets + attachments · appeals ·
accommodations · config (13 `sp_*` portal switches).

## 8. Roles & permissions
One student role; per-feature portal switches in `site_settings` (`sp_*`: login, booking open,
reschedule + 72 h cutoff, results visible, certificate download, CPD + target hours, tickets,
practice, readiness required, identity document required). Admin oversight via the `members`
section (Module 5).

## 9. Tech stack & structure
React 18 + TS strict; `frontend/src/pages/` (21 screens; largest: `LabRunner.tsx` 1,210 lines,
`Certifications.tsx` 699); shared `components/` (premium UI kit, hand-rolled SVG charts, passport
components); i18n catalog (7 languages, RTL for Arabic); Vitest with risk-based per-file coverage
floors; 27 Playwright e2e specs.

---

<a name="module-3"></a>
# Module 3 — Examination & Certification Engine

## 1. Overview & business purpose
The heart of the certifying body: entitlements → booking → proctored sitting (browser, desktop, or
external vendor) → server-side scoring → a defensible result lifecycle → verifiable credentials.
Design north star: **integrity that is auditable and honest** — immediate publication by default,
holds only for technical invalidity, nothing hidden client-side, nothing deleted.

## 2. Functional workflows

### The exam pipeline
```
payment/waiver → exam_entitlements(available)
  → book   POST /api/me/exam/book      booking(scheduled), entitlement(booked)
  → [desktop: launch-code → pciexam:// → /api/exam/authorize]
  → start  POST /api/me/exam/start     attempt(in_progress)   [browser path]
  → heartbeat (5 s)                    answers autosave + proctor events + chat + server clock
  → submit                             attempt(submitted), entitlement(consumed)
       ↓
  result_status: auto_held | released_pass | released_fail | credential_issued
       ↓ (if held)
  admin release / invalidate / reinstate   (AdminProctoring)
```

**Booking gates** (in order): entitlement exists → `Lifecycle.BookingBlockers` clear →
scheduling window not lapsed → not already booked *for this certification* → payment not already
used → exam not already taken on this payment → persisted retake wait elapsed → slot ≥2 h in the
future → within the window → external vendor not blocking. Booking + vendor routing is
**fail-closed**: if routing fails after insert, the booking is cancelled and the entitlement
restored — never a silent local fallback.

**Launch timing** (browser and desktop identically): openable from `slot − 15 min` to
`slot + 30 min grace`; past grace the booking flips to `missed`. `LaunchBlockers` (identity,
consents, account hold) are re-checked at launch, plus the readiness check
(camera/mic/network/fullscreen) when required.

**Submission integrity:** hard stop = `started_at + duration (+ approved extra minutes) + 1 min
network grace`. Past it, the posted payload is **discarded** and scoring runs on heartbeat-persisted
answers (anti-cheat: you can't let the clock die and inject a winning payload). Any answered id
outside the server-issued item set ⇒ `item_set_mismatch` (a technical hold). Finalisation runs in
one transaction with `WHERE status='in_progress'` as a single-winner lock. The heartbeat
force-finalises abandoned sittings server-side.

### The result lifecycle (the critical state machine)
`exam_attempts.result_status`: `not_started` → `auto_held` | `released_pass` | `released_fail` →
`credential_issued` | `invalidated` | `credential_revoked`.

- **Immediate publication is the default.** Only *technical invalidity* always holds:
  `submitted_after_deadline`, `booking_missing/invalid`, `payment_reversed`, `duplicate_attempt`,
  `item_set_mismatch`.
- Proctoring/identity signals are **audit-only** unless an operator opts in:
  `auto_block_result_on_tampered_attempt` / `_critical_violation` (+ threshold) / `_identity_fail`
  — all default **off** (`Core/Lifecycle.cs:AutoHoldReason`).
- A **held** result never discloses score/pass-fail/credential — redacted server-side at every
  surface (`/api/me`, attempt detail, score report, data export) and again in the desktop client.
- **Invalidation never deletes**: the attempt is preserved and labelled
  (`counts_as_attempt=0`, reason, actor); a linked active credential is revoked. Reinstate
  recomputes pass against the certification's configured pass mark and un-revokes rather than
  duplicating.
- `exam_score_snapshots` is **immutable** — written once at scoring so later question-bank edits
  can never retro-change a result (unique index per attempt).

### Credentials & verification
- `Lifecycle.IssueCredential`: idempotent per attempt (partial unique index); number format
  `PCI-<PREFIX>-[FND-|HON-]<YEAR>-<6 digits>`; expiry from the certification's `expiry_years`;
  route `certificate_wording` snapshotted at issue. Verifiable QR PDF (`Core/CertPdf.cs`),
  self-hosted Open Badges (`Endpoints/Badges.cs`), optional Credly export.
- `GET /api/verify`: computes state (active/expired/suspended/revoked) rather than trusting the
  stored status; `PCI-HON-` prefixes route to the honorary registry and are *never* represented as
  a passed examination; test accounts (`users.is_test=1`) report as not found; returns the PDF's
  SHA-256 for tamper evidence.

### Exam exceptions & delivery vendors
- `Endpoints/ExamExceptions.cs` (16 granular `ex_*` permissions): deadline extensions, reopen
  scheduling, replacement/additional attempts (`counts_as_attempt=0` for verified system failures),
  fee waivers, incident decisions. Scheduling windows resolve by precedence: individual > campaign
  > institution > country > exam > route > certification > global (`exam_window_rules` →
  `exam_authorizations`).
- `Endpoints/AdminExamDelivery.cs` + `Core/ExamDeliveryConnectors.cs`: five external vendors
  supported (Pearson VUE/OnVUE, Kryterion, PSI, TestReach, Questionmark) — scheduling lifecycle,
  result ingestion (a vendor-graded pass issues a PCI credential), append-only vendor API log,
  write-only secrets.

## 3. Development & implementation
Backend: `Endpoints/StudentExam.cs` (pipeline + `/api/me/*`), `ExamClient.cs` (desktop),
`AdminProctoring.cs` (console + release/invalidate/reinstate), `ExamExceptions.cs`,
`AdminExamDelivery.cs`, `Certificates.cs`, `Badges.cs`; `Core/Lifecycle.cs`, `Certs.cs`,
`ExamAuthorization.cs`, `ExamDelivery.cs`, `CertPdf.cs`. Scoring = exact answer-index match over
the server-issued item ids; pass mark from the attempt's certification (never a hardcoded 65).

## 4. Architecture & design
Server owns the clock and the scoring — always. Item banks are per-certification, live-only
(`is_practice=0`), and the answer key never leaves the server. Config precedence: per-certification
overrides (`pass_mark_pct`, `duration_minutes`, `exam_price`, `expiry_years`) over globals
(`exam_pass_mark_pct=65`, `exam_duration_minutes=90`, open-before 15, grace 30).

## 5. Dependencies & integrations
Payments (entitlements are only minted on settled money or explicit audited waivers), proctoring
evidence storage, external vendors, Credly, comms (confirmations/results), accommodations
(approved extra minutes, capped 120, MAX not sum).

## 6. Database
`exam_entitlements` (one per payment — unique index), `exam_bookings`, `exam_attempts` (the
central entity; `attempt_class`, `client_kind`, review fields), `exam_score_snapshots` (immutable),
`exam_launch_codes` (hashed, 15-min TTL), `exam_readiness_checks`, `exam_authorizations` +
`exam_window_rules` + extension/reschedule/grant/incident history tables, `exam_delivery_*`
(providers/orders/log), `issued_credentials` (+ partial unique per attempt),
`certificate_downloads` (audit), `proctor_events`, `exam_evidence`, `identity_checks`,
`proctor_messages`, `sample_questions`, `certifications`, `certification_routes`,
`certification_applications`.

## 7. APIs
Student: the `/api/me/exam/*` set + `/api/me/attempts` + `/api/me/results/{id}/report` +
`/api/me/certificate`. Desktop: `POST /api/exam/authorize|evidence|identity`. Admin:
`/api/admin/exam-sessions[...]` (live console, review actions, launch codes),
`/api/admin/certifications`, `/api/admin/credentials` (+ Credly), `/api/admin/exams`,
exceptions + delivery-vendor suites. Public: `GET /api/verify`, `GET /api/certifications`.

## 8. Roles & permissions
Admin sections `exams`, `proctoring`, `sampleq`, `exam_delivery`, `credentials`, `set_exam`, plus
the 16-key `ex_*` exceptions family. Certification-scoped admins (`cert_scope`) see only their
certifications; cert 1 is permanent (cannot be deleted/deactivated); `PCI-HON` prefix is reserved
everywhere.

## 9. Tech stack & structure
Pure C# minimal-API handlers with inline validation; deterministic scoring; Python suites
(`lifecycle_test.py`, `release_test.py`, `publication_test.py`) replicate the production SQL
against real databases and are the best executable documentation of these rules.

---

<a name="module-4"></a>
# Module 4 — SecureExam Desktop Client

## 1. Overview & business purpose
A downloadable Windows kiosk client (`PCISecureExam.exe`) delivering proctored exams with
lockdown, webcam/mic monitoring, identity capture, and crash-safe resume — in the style of
Pearson OnVUE. Its security claims are deliberately **honest**: user-space lockdown that degrades
loudly (it cannot block Ctrl+Alt+Del and says so), a rule-based baseline AI that never fabricates
a match or a score, and a server that owns the clock and all scoring.

## 2. Functional workflows
Screen state machine (`ExamFlow`, in Core's `ExamScreen` enum):
```
Launch → Consent → SystemCheck → IdentityCapture → RoomScan → Rules → Exam → Submitted
```
- **Launch:** the portal (or a proctor) mints a single-use launch code
  (`PCI-` + 20 random bytes hex, hashed at rest, 15-min TTL) and hands the client
  `pciexam://start?code=…&api=…&token=…`. The client redeems it at `POST /api/exam/authorize`
  on the **pinned** host and receives a 6-hour exam session token + the item set (never the key).
  Re-redemption is allowed only while the attempt is `in_progress` (crash-resume), never replay.
- **System check (6):** webcam, microphone, single display, environment (VM/remote — remote
  session is a hard fail, VM hint is a warn), prohibited applications (29-name denylist),
  connectivity.
- **Identity:** capture face + ID photos → provider verdict (Baseline/Azure/AWS seam) → posted to
  the backend; advance gated on `Verified`.
- **Exam:** kiosk lockdown engages (see below); heartbeat every 5 s carries answers, violations,
  proctor events and chat both ways; **the server's `RemainingSeconds` re-anchors the local clock
  every beat** — an outage, sleep, or restart can never add time; `ForceSubmit` fires exactly once.
- **Offline resilience:** failed beats re-enqueue everything and persist an encrypted local cache
  (DPAPI, `SecureStore`); on resume, server state is authoritative, local answers overlay.
- **Submitted:** rendering is built *only* from Core's `SubmittedView` — a held result shows no
  score/pass-fail/credential even if a malformed payload carries one (unit-tested invariant,
  defence-in-depth alongside server-side redaction).

## 3. Development & implementation — solution structure
`secureexam/PCI.SecureExam.sln` (SDK pinned 8.0.100):

| Project | Target | Purpose |
|---|---|---|
| `PCI.SecureExam.Core` | `net8.0`, **zero references** | The security-critical heart: wire DTOs/enums, `pciexam://` parsing (`LaunchParameters` — never throws), **host pinning** (`ClientConfig`), baseline proctor/identity analyzers, `SubmittedView` held-result rule. Builds/tests on Linux CI. |
| `PCI.SecureExam.App` | `net8.0-windows` WPF | The kiosk client. `Security/` (KeyboardHook, KioskWindow, ProcessGuard, DisplayGuard, VmDetector, CaptureShield), `Proctoring/` (CameraService ~1.25 fps face checks + 15 s evidence JPEGs; MicMonitor RMS/voice detection), `Api/PciApiClient`, `Exam/` (ExamFlow, HeartbeatService, SecureStore), `Providers/AiProviderFactory` (the single AI seam; cloud stubs fail closed), `Infrastructure/` (ConfigLoader, SelfTest, UriSchemeRegistrar). |
| `PCI.SecureExam.Server` | web | **Optional reference** harness: launch-code `LaunchStore` (in-memory), evidence/identity sinks, SignalR `ProctorHub`. Production uses the main backend. |
| `PCI.SecureExam.Tests` | xUnit | Core-only → cross-platform (launch parsing, DTO contract, analyzer, held-result invariant). |
| `PCI.SecureExam.Core.RunnableChecks` | (not in sln) | Package-free console harness proving 15 host-pinning attack cases offline. |

## 4. Architecture & security model
- **Host pinning:** dot-anchored HTTPS allowlist `{projectcontrolsinstitute.org, pci-global.org,
  localhost}`. A malicious `api=` in the launch URI is silently ignored (`WithLaunch`), and
  `EnsureTrustedOrThrow` refuses to start against an untrusted host — defence against a tampered
  installed `appsettings.json`. Proven cases: `evil.com`, `projectcontrolsinstitute.org.evil.com`
  (suffix spoof), plaintext http, substring lookalikes — all rejected; trusted subdomains accepted.
- **Kiosk lockdown (user-space, honest):** low-level keyboard hook swallows Win/Alt-Tab/Alt-F4/
  PrintScreen (cannot block Ctrl+Alt+Del — stated in-code); borderless topmost full-virtual-screen
  window; `SetWindowDisplayAffinity` capture exclusion; 2.5 s process denylist poll; 3 s monitor
  count poll; one-shot VM/remote detection. Runs `asInvoker` (not elevated) and degrades honestly.
- **Single-use launch codes, not bearer tokens**, hand off the sitting; the code doubles as the
  attempt token after redemption; the 6-hour session token is minted server-side, hashed at rest.
- **Server-authoritative everything:** items, clock, scoring, force-submit, result presentation.

## 5. Dependencies & integrations
Calls the main backend: `POST api/exam/authorize` → `api/me/exam/heartbeat` → `api/me/exam/submit`
→ `api/exam/evidence` (base64 data-URI) → `api/exam/identity`. The backend emits dual-cased JSON
keys (distinct alias names, e.g. `remaining_s`/`remainingSeconds`) specifically for this client's
case-insensitive binder. Results surface in the Student Portal and the Admin Proctoring console.

## 6. Database (backend side)
`exam_launch_codes` (code_hash unique), `exam_attempts` (`client_kind='desktop'`),
`proctor_events`, `exam_evidence` (blob refs via `Core/Storage`, never base64 in the DB),
`identity_checks`, `proctor_messages`, `login_tokens` (6-hour exam session).

## 7. APIs — see §5 above; plus the reference server's `api/exam/*` + `/hubs/proctor` (demo only; no heartbeat/submit — the full loop needs the main backend).

## 8. Roles & permissions
Candidate side: launch code + minted session. Admin side: the `proctoring` section gates the live
console, evidence access, review actions and proctor-issued launch codes; per-certification
scoping applies.

## 9. Build, test & known gaps
```powershell
cd secureexam
./build.ps1              # restore → build → dotnet test (Core, cross-platform)
./build.ps1 -SelfTest    # machine readiness (exit 0 = ready) — designed to gate rollouts
./build.ps1 -Run         # demo run against launch code PCIDEMO12345 (start the Server first)
./build.ps1 -Publish     # self-contained single-file PCISecureExam.exe (Windows)
```
No pre-built exe in the repo. CI builds/tests on both Windows (full) and Linux (Core).
**Known gaps (candid):** several `appsettings` proctoring/feature knobs are dead (hardcoded 15 s /
800 ms; server-supplied flags rule); `Terminate()`/`Terminated` screen unreachable; violation-count
inflation from undeduped High events (server takes MAX); reference `ProctorHub` has no auth;
the Haar cascade XML must be sourced per `Assets/README.txt` (falls back fail-open to "one face");
the README's "Node backend" phrasing is stale — the backend is C#/ASP.NET; no portal UI button yet
calls the launch-code mint endpoints (backend is ready).

---

<a name="module-5"></a>
# Module 5 — Admin Portal (Operator Console)

## 1. Overview & business purpose
The single operator console for the whole institute at `/admin/` — content, students, exams,
money, comms, marketing, partners, compliance, and PCI World launch control. The classic
`admin.html` is retired (301 → `/admin/`). Design creed: the server enforces everything the UI
gates ("hiding a button is never the authorization"); the console is defence-in-depth.

## 2. Functional workflows & features
**70 nav items in 16 categories** (`frontend/src/admin/AdminLayout.tsx` — the "~29 sections" in
older docs is stale):

- **Overview:** Dashboard (KPIs, revenue, funnel — test accounts excluded), Reports (CSV export,
  fixed 7-entity allowlist, formula-injection-safe, export audited).
- **Students (12):** roster + student-360 drawer, enrolments, payments, support tickets, appeals &
  accommodations, CPD review, documents, books, membership grades, member directory, identity
  merges, GDPR erasure queue.
- **Support:** Communications Centre, unified Support inbox (tickets+chats+enquiries, SLA,
  canned templates, internal notes), Error reports (searchable by the `PCI-…` reference).
- **Examinations:** certifications, registrations, Proctoring & sessions (live console),
  Exam Exceptions, delivery vendors, question bank (CRUD), credentials (+Credly), Simulation Lab
  studio.
- **Access & pricing:** discount codes (+bulk generation), founding stage, honorary fellows &
  applications (owner-only), pricing (CRUD).
- **Website (19):** Content & Distribution Centre (blog CMS + AI studio + capability registry),
  pages & content, downloads centre, templates, site content, announcement, translations
  (owner-only), reviews moderation, forum moderation, events, careers, social media, + 7 CRUD
  collections (FAQs, resources, news, media, BoK, governance, navigation).
- **SEO / Analytics / AI Visibility:** managed redirects, page SEO, audit; cookieless analytics;
  GEO readiness (llms.txt, AI-crawler policy).
- **Partners:** training partners (directory + applications), marketing partners, Partner Finance
  (immutable commission ledger, maker-checker settlements).
- **Integrations:** ERP outbox connectors (webhook/QuickBooks/Zoho/Odoo), Certuvo.
- **Marketing:** dashboards, Ads & Search Console centre, campaigns (CAN-SPAM/GDPR enforced
  server-side).
- **PCI World:** the Launch board (Module 7).
- **Community:** enquiries, form submissions, newsletter.
- **Operations (8):** email log, audit log, Identity & Student Numbers console, event check-in
  scanner, notifications, Readiness (system-check, owner-only), Settings, Team & Access
  (owner-only).

**UX:** sidebar quick-filter, ⌘K command palette, breadcrumbs, RBAC-filtered nav (an admin never
sees a section they can't open; category headings render only when non-empty).

## 3. Development & implementation
- **The CRUD factory** — the pattern to know: one backend line
  `Crud(name, cols, order, section, certCol?)` in `Endpoints/AdminMgmt.cs` generates uniform
  `GET/POST/PATCH/DELETE /api/admin/{name}` (identifier backtick-quoting, 409 on constraint,
  content-cache bumps on every mutation, optional certification scoping); one frontend entry in
  `crudConfigs.ts` renders it through the single generic `CrudSection.tsx` (list + filters +
  search + drawer editor + payload coercion). **Adding a collection = one line + one config entry.**
- Registered collections: faqs, bok_domains, sample_questions (cert-scoped), cert_documents
  (cert-scoped), governance_roles, resources, news, nav_items, media_assets, pricing_rules.
- Certifications are deliberately *not* generic CRUD: history-bearing certs can't be hard-deleted
  (409 `in_use`), cert 1 is permanent, `PCI-HON` codes are reserved, new certs auto-seed routes +
  starter documents.
- Auth context: `AdminAuth.tsx` (`can(section)`), gates `<Perm>`, `<OwnerOnly>`, `<AnyPerm>`;
  admin client `makeClient('pci.admin.token')` (12-hour sessions).
- Backend inline endpoints (Program.cs): admin login/logout, forgot/reset (1-hour tokens),
  `ADMIN_RECOVERY_CODE` break-glass (constant-time; also clears TOTP), 2FA enrol/verify/disable
  (10 hashed one-time recovery codes), Team & Access (owner-only; last-owner guards), settings
  GET/PATCH (prefix-gated), storage purge (owner-only), system-check (owner-only).

## 4. Architecture & design
Two orthogonal authorization axes: **section permissions** (`GateFn`) and **certification scope**
(`cert_scope` → `CertFilterSql`). Owner bypasses sections but scoping still applies to non-owners.
Settings are deny-by-default by key prefix (`web_`→`set_web`, `sp_`→`set_sp`, `exam_`→`set_exam`,
else `settings`), symmetric on read and write, with rejected keys reported not fatal. Every
privileged action writes `audit_logs`; sensitive *reads* (ID documents, evidence, exports) are
audited too.

## 5. Dependencies & integrations
Everything — the console is the operator surface over every other module.

## 6. Database
`admin_users` (roles, permissions JSON, cert_scope, TOTP, lockout), `admin_sessions` (12 h),
`admin_reset_tokens`, `audit_logs`, `impersonation_sessions/_events`, plus every domain table it
manages.

## 7. APIs
`/api/admin/*` — ~40 modules' worth; the factory collections; `/api/admin/overview` (any admin);
`/api/admin/system-check` and `/api/admin/integrations/health` (owner / integrations).

## 8. Roles & permissions — see [§18](#18-roles--permissions) for the full matrix.

## 9. Tech stack & structure
React 18 + TS strict; `frontend/src/admin/` (router with 64 routes, 67 pages; largest:
`ExamExceptions.tsx` 1,011, `TrainingPartners.tsx` 857, `ContentCentre.tsx` 824); separate Vite
config (`vite.admin.config.ts`, base `/admin/`, output `dist-admin`).

---

<a name="module-6"></a>
# Module 6 — Payments, Pricing & Commerce

## 1. Overview & business purpose
All money: pricing rules, discount codes, Stripe checkout + webhooks, memberships & dues,
entitlements, refunds/disputes, fee waivers, partner commissions. Doctrine (from the code):
`Payments.cs` is "the ONLY place access is granted (after verified payment)" — and financial
write failures are never swallowed behind a 200.

## 2. Functional workflows
- **Pricing engine** (`Public.Pricing`): per-category `pricing_rules` → a certification's own
  `exam_price` overrides the generic rule → default discount → code discount (fixed/percentage,
  min-transaction, max-discount caps) → floor at `min_payable`. The same engine renders website
  price tags — the site can't advertise what checkout won't honour.
- **Discount-code validation** (12 sequential rules): status lifecycle, active flag, cert scope,
  founding codes refused in the discount field, date window, `max_uses` counting reservations,
  per-user/email limits, product scope, waiver codes locked to a named email, partner allocation +
  agreement checks, country eligibility.
- **Checkout** (`POST /api/create-checkout-session`): exam-only requires an active unexpired
  membership; recert requires CPD met (checked at *checkout*, quoting required vs approved hours);
  unknown certification rejected, never coerced; code re-validated server-side; **idempotency key
  mandatory** — a capacity hold (`checkout_reservations`) is taken in a transaction, a replayed key
  returns the same Stripe URL, and a failed Stripe create releases the hold.
- **Webhook** (`POST /api/webhook`): fails closed without the secret; only fulfils on settled
  money (`paid`/`no_payment_required`); **double idempotency gate before any side effect**
  (`INSERT OR IGNORE payments` on the PaymentIntent + `webhook_events` on the event id); then in
  one transaction: user create/activate + Student Number issue, membership (3-year), entitlement +
  `exam_schedule_deadline` (+1 year) + exam authorization, code redemption + partner commission
  (rethrown on failure so Stripe redelivers), set-password token, durable welcome email enqueue.
  Blocking HTTP work (email drain, Certuvo provisioning) is deliberately deferred outside the
  transaction.
- **Refunds/disputes:** partial refund records and reverses commission but keeps access; full
  reversal revokes *unused* entitlements and future bookings but never a credential already earned;
  membership lapses only when no other paid payment supports it.
- **Subscriptions:** optional recurring dues (`invoice.paid` on `subscription_cycle` claims the
  event id atomically with the extension; reconciles to Stripe's `PeriodEnd`).
- **Offline settlement:** admins can `mark-paid` (bank/cheque/invoice) or **waive** — recorded as
  `payment_status='waived'` with a `fee_waivers` ledger row, never a fabricated `paid`.
- **Partners:** institution portal (separate `partner_users` realm) for code requests (approval
  workflow + fraud flags), bulk sponsorships, and an immutable commission ledger with
  maker-checker settlements (prepare → approve by a different person → pay).

## 3–4. Implementation & design
`Endpoints/Payments.cs` (642 lines), `Public.cs` (pricing/validation), `AdminMgmt.cs` (codes,
pricing), `AdminExtra.cs` (bulk generation, reports), `AdminOps.cs` (finance ops, waivers),
`Partners.cs`/`PartnerPortal.cs`/`TrainingPartners.cs`; `Core/Money.cs` (minor units, no floats in
the commission path), `CheckoutReservation`, `PartnerCommission*`, `PartnerSettlement`,
`FeeWaiverLedger`, `FraudChecks`. Stripe endpoints answer **503** when no key is configured —
graceful degradation, not a bug. Test accounts (`is_test=1`) are excluded from every revenue
figure and the public register.

## 5. Dependencies & integrations
Stripe (checkout, webhooks, billing portal, subscriptions) · ERP outbox events
(`payment.recorded`, `membership.activated`) · comms (payment/welcome emails) · Certuvo
provisioning trigger · exam engine (entitlements/authorizations).

## 6. Database
`pricing_rules`, `discount_codes` (+status workflow, partner/founding/waiver semantics),
`payments` (+finance-control columns), `code_redemptions` (unique per payment),
`checkout_reservations` (idempotency-unique), `memberships` (+grades, Stripe subscription mirror),
`membership_upgrades`, `enrollment_sessions` (resume-token gated), `fee_waivers`
(idempotency-unique), `webhook_events` (unique event id), `training_partners` +
applications/documents, `partner_sponsorships`, `partner_payouts`, `partner_users/_sessions`,
finance module tables (`partner_agreements`, `partner_commission_*`, `partner_settlement*`,
`partner_disputes`, campaign links/clicks).

## 7. APIs
Public: `/api/pricing`, `/api/validate-code`, `/api/create-checkout-session`, `/api/webhook`,
enrolment session save/resume (token-required). Student: `/api/me/membership/subscribe|portal`,
`/api/me/invoices`, receipt PDFs. Admin: `/api/admin/codes*`, `/pricing`, `/payments`,
`/enrollments`, `/reports`; partner portal `/api/partner/*`.

## 8. Roles & permissions
Sections `payments`, `pricing`, `codes`, `enrollments`, `reports`; high-risk `finance` (mark-paid/
waive/reverse) and the `pf_*` partner-finance family are **never** in any named role bundle —
owner + explicit grants only; settlement approval enforces approver ≠ preparer server-side.

## 9. Tech stack — Stripe.net; money as DECIMAL(12,2)/minor units; every idempotency guarantee is a database unique index, not application memory.

---

<a name="module-7"></a>
# Module 7 — PCI World

## 1. Overview & business purpose
**PCI World** (`pciworld.org` — *"Make the decision. Control the outcome."*) is a global,
free-to-enter project **challenge, simulation and professional-evidence platform** operated by the
institute. It is a growth/funnel and community product, strictly separated from certification:

> **Structural rule:** PCI World never reads or writes `users`, `exam_attempts`, entitlements,
> credentials or membership. No FKs cross from `pciworld_*` into exam/credential tables. Every
> public World page carries the disclosure that challenges are educational practice, **not** PCI
> certification examinations.

It shares only infrastructure with the institute platform: hosting, the `Db` layer, mailer,
security helpers, and `SimCalc` as the reference solver. UI is never shared — World pages are
self-contained server-rendered HTML (`Core/WorldPages.cs`, 2,657 lines) plus two dedicated React
bundles.

## 2. Functional workflows & features
- **Daily challenge platform** — anonymous-first participation, daily rotation ledger
  (`WorldRotation`), deterministic scoring (`WorldScore`/`SimCalc`), archive, sharing, invites,
  referrals. 30-challenge validated library at Phase-2 (18 engine families, 5 tracks); 90/180/365
  are *release gates, not seeds*. Honesty rules: no participant counts, testimonials, percentiles
  or leaderboards are rendered anywhere.
- **Participant accounts + PCI World Passport** — evidence profile; public verification by PCI
  Student Number (`/world/verify` — built for recruiters); SSO handoff into the student portal via
  a 90-second fragment-carried code.
- **Community rooms** — text rooms with guest sessions, moderation cases/appeals/sanctions, policy
  versions, risk restrictions, an outbox, and (separately gated) **image sharing** with a scan
  pipeline: raw bytes (`storage_ref`) are never served; only an EXIF-stripped derivative written
  when the verdict is `allowed`; suspected-illegal material moves to two-person-controlled
  restricted evidence. Realtime via SignalR (`/api/world/hubs/community`).
- **Professional forum** — public threads with a trust ladder, flags, revisions (distinct from the
  simple institute forum).
- **Careers** — job board + employer portal (employers are tenants, not admin roles), CV upload,
  application consents, CV access logging.
- **Contributor desk** — applications, editorial queue with a two-person rule.
- **PCI Project Intelligence** — the premium practice programme (*"Think. Decide. Deliver."*)
  with a governed taxonomy (8 experience types / 12 competency domains / 6 lifecycle stages / 10
  sectors) and ~1.6 MB of seeded Year-1 content packs.
- **World Admin** (`/world-admin`) — separate console + auth realm; challenge lifecycle
  draft → in_review → approved → published with maker-checker (reviewers never own drafts),
  immutable version snapshots, calendar, moderation, careers verification, audit.

## 3. The Launch Board — how features go live (`Endpoints/WorldLaunch.cs`)
**PCI World ships switched off.** Every feature flag seeds `'0'` — "installing the schema is not
launching the feature"; fresh-install 404s are the design working. The owner-only board
(Admin console → PCI World → Launch) exists because the generic settings screen made the safe and
unsafe action the identical keystroke:

| Flag | Feature | Prerequisite acknowledgement required |
|---|---|---|
| `world_community_enabled` | Community rooms | — (advisory: moderation queue needs a roster) |
| `pciworld_forum_enabled` | Professional forum | — (advisory: posts are public + indexed) |
| `pciworld_careers_enabled` | Careers | `pciworld_ack_careers_privacy` — candidate privacy notice published |
| `pciworld_contributors_enabled` | Contributor desk | `pciworld_ack_contributor_terms` — editorial policy + terms published |
| `pciworld_community_images_enabled` | Images in rooms (depends on community) | `pciworld_ack_image_moderation` — moderation provider contracted; minimum age + jurisdictions settled |

Design principles encoded in the endpoint (not the UI): the refusal lives in the server on the
POST; flags are an exact-name allowlist; enabling with a missing dependency/ack → 409; disabling
never cascades but reports `orphaned[]`; an acknowledgement is a signed JSON `{by, at, note≥8chars}`
mirrored to the append-only `pciworld_audit` — and the code is explicit that **an acknowledgement
is not evidence**, it is an owner's recorded assertion.

## 4. Architecture & deployment
- Isolation by prefix + realm: tables `pciworld_*` (~65 across World/Community/Media/Forum/
  Contributor/Careers schemas), routes `/world*`, `/api/world*`, auth in
  `pciworld_users/_user_sessions` (30-day) and `pciworld_admin_users/_admin_sessions` (8-hour),
  OAuth 2.1 + PKCE groundwork (`WorldOAuth`).
- Host mapping: `PCIWORLD_HOSTS`/`PCIWORLD_ADMIN_HOSTS` map `pciworld.org`/`admin.pciworld.org`
  onto the same deployment; `PCIWORLD_STANDALONE`/`PCIWORLD_ONLY` for dedicated deployments.
- **`PCIWorld/` deployment root:** a backend-only Dockerfile baking `PCIWORLD_ONLY=true` (skips
  the React build stages; institute surfaces return real 404s via the `WorldOnly.Allowed()` path
  allowlist). Zero-config boot uses an explicit SQLite bridge on `/data` (ephemeral without a disk
  — loud `EPHEMERAL STORAGE` banner); **MySQL 8 is the production destination** — setting
  `MYSQL_HOST` flips fail-closed to MySQL with no silent fallback (incomplete MySQL config exits 78
  before any DB is opened). Mailed links never derive from the request Host header
  (`WorldUrl.Base()` resolution order ends in a known-hosts check).
- Frontend: `frontend/src/world/` (one bundle path-split three ways: community app and
  verify-number are public; the rest authenticated) and `frontend/src/worldadmin/`. World sessions
  deliberately use `localStorage` + an `X-World-Account` header — never the institute's token
  carrier or storage key.

## 5–6. Dependencies & database
Depends on the shared platform infrastructure only. Tables (by schema installer):
`WorldSchema` 29 (challenges/versions/calendar/rotation/sessions/attempts/invites/referrals/users/
tokens/handoff/oauth/admin/audit/reports/articles/sources/entities/reviews/events),
`CommunitySchema` 13 (+`CommunityMediaSchema` 5), `ForumSchema` 9, `ContributorSchema` 4,
`CareersSchema` 9.

## 7. APIs
Public SSR: `/world`, `/world/challenge/{code}`, `/world/archive`, `/world/blog|news`,
`/world/forum`, `/world/careers`, `/world/verify`, `/world/p/{token}` (passport), `/world-admin`.
JSON: `/api/world/*` (session, today, attempts, share, invite, community, forum, careers,
contributor, project-intelligence, SignalR hub) and `/api/world-admin/*`. SPA mounts:
`/world-app/`, `/world-admin-app/`.

## 8. Roles & permissions
Separate `WorldRbac` realm: `owner`, `author`, `reviewer`, `publisher`, `viewer` (maker-checker
between author and reviewer). Employers are tenants via membership rows. The institute-side launch
board is institute-owner-only.

## 9. Docs
`docs/pciworld/` (26 files) — `ARCHITECTURE.md` (the ADR), `PLAN.md` (phase gates),
`THREAT_MODEL.md`, phase designs, runbooks, `PROJECT_INTELLIGENCE.md`, `DEPLOY_RENDER.md`.

---

<a name="module-8"></a>
# Module 8 — PCI Global

**What it is (and is not).** "PCI Global" is **not a separate product, website, or module** in
this repository. The facts:

1. **`pci-global.org` is a pinned alternate API host** in the SecureExam desktop client's
   dot-anchored HTTPS allowlist (`secureexam/PCI.SecureExam.Core/ClientConfig.cs`):
   `{ projectcontrolsinstitute.org, pci-global.org, localhost }`. Subdomains match
   (`staging.pci-global.org`, `eu.pci-global.org` are accepted in tests), implying it is reserved
   as a **regional/staging exam-API domain**. The client refuses to start against any other host.
2. **The legal entity** is "Project Controls Institute **Global**, Inc." — the "Global" in the
   company name is the likely origin of the label.
3. **Display/sender name:** the mailer's Resend test sender is `PCI Global <onboarding@resend.dev>`
   and `DEPLOY.md` uses `PCI Global <no-reply@…>` as an example `MAIL_FROM`.
4. **Do not confuse with `pciglobal.ai`** — a retired domain that 301-redirects to the canonical
   host, or with the `@pciglobal` social-handle placeholder.
5. The admin **Identity console** is titled "PCI Global identity console" — it manages PCI Student
   Numbers (see Module 11), reinforcing "PCI Global" as the institutional umbrella brand rather
   than a distinct system.

**Onboarding takeaway:** when someone says "PCI Global" they mean the institute's global brand /
legal entity / exam-API domain family — the code, data, and deployments are the ones described in
the other modules of this document.

---

<a name="module-9"></a>
# Module 9 — Certuvo (External Practice Platform)

## 1. Overview & business purpose
**Certuvo is PCI's official platform for exam preparation** — fully online study and
scenario-based practice mirroring the exam — but it is an **independent external platform**.
"Certuvo is where you prepare; the PCI examinations are where your competence is assessed."
PCI remains the system of record for membership, certification, eligibility and administration.

## 2. Functional workflow
Settled membership → `Settlement.EnsureDownstream` → `CertuvoLink.Provision` creates a Certuvo
account and emails credentials; the student portal's `/certuvo` page shows access and offers a
credentials resend.

## 3–4. Implementation & design (`Core/Provisioning.cs`, `docs/CERTUVO_INTEGRATION.md`)
- **PCI owns the login identity:** immutable PCI-generated usernames `PCI-{year}-{seq:000000}`
  (never the email), PCI-generated temporary passwords (guaranteed-complexity, default 14 chars)
  pushed with `must_change_password`.
- Secrets AES-256-GCM encrypted at rest (`enc:v1:`); email-conflict rule (`dedicated` default —
  an existing Certuvo account is never overwritten or merged); idempotency keys; retry queue with
  exponential backoff (5 min → 6 h) and an admin alert at the ceiling; inbound webhook
  `POST /api/certuvo/webhook` guarded by `X-Certuvo-Secret`; honorary conferral auto-provisions a
  full student account + waived membership + Certuvo handoff. Eligibility configurable
  (`certuvo_requires`).
- Status: "production-ready pending the external Certuvo API contract". (An older built-in
  practice engine still exists server-side — `practice_attempts`, quiz/mock — but the portal page
  was intentionally reduced to the access card; the integration doc is authoritative.)

## 5–7. Integrations, database, APIs
Triggered by payments; surfaces in Admin → Integrations → Certuvo. Table: `certuvo_accounts`
(user-unique, status/retry/suspend/revoke lifecycle, idempotency). APIs: `/api/me/certuvo/access`,
`/resend`; admin provisioning ops in `AdminOps.cs`; the webhook.

## 8. Permissions — admin `integrations` section; student self-service on their own account.

---

<a name="module-10"></a>
# Module 10 — Communications, Content & Marketing Centre

## 1. Overview & business purpose
Every outbound message, blog post, social share, ad campaign and analytics read-through, built
"honestly gated": everything records what it *would* send and reports "not configured" until an
operator activates it (`docs/ACTIVATION.md` — "nothing goes live by accident").

## 2–3. Features & implementation
- **Unified Communications Centre** (`comm_*` tables, `Endpoints/CommsCentre.cs`): sender
  profiles, WhatsApp accounts (tokens by env-var *name*, never stored), versioned approval-gated
  templates, event triggers (with `backend_wired` honesty flag, consent categories —
  transactional bypasses opt-out, marketing requires consent), a single **outbox**
  (`comm_outbox`, unique dedup key, 14-state lifecycle, retries), unified inbox conversations
  with SLA + routing rules (rules only classify and route — never adverse decisions), suppression
  list, per-user preferences. Provider secrets live only in env vars.
- **Mailer** (`Core/Mailer.cs`): Resend → SMTP → console precedence; 12 HTML templates in
  `backend/emails/`; every send logged (`email_logs`); console mode prints full messages + links
  (password resets remain recoverable from logs).
- **Content Centre** (`Endpoints/ContentCentre.cs`): blog CMS with editorial pipeline
  (author/editorial/technical/seo/legal reviews), full version snapshots (never overwritten),
  scheduled publishing, IndexNow; **Integration Capability Registry** — honest per-platform
  classification (Direct Publishing / Requires Approval / Draft Export / RSS / Manual / Read Only
  / Unsupported…); **AI Studio** — assist-only OpenAI/Anthropic drafting, `require_review`
  default on, full generation audit, keys from env only.
- **Social & syndication:** managed public profiles (`social_accounts` — free-string platforms,
  link checking, approval status, structured-data inclusion), share-button settings, publishing
  accounts (encrypted secrets), syndication destinations, external-source import (license +
  allowed-use), backlink monitoring.
- **Marketing Centre** (`mkt_*` 26 tables, `Endpoints/MarketingCentre.cs`): platform connections,
  campaigns with budget approvals, LinkedIn posts/outreach, lead forms + leads (webhook-secured),
  conversions, GSC properties/inspections/query data, keywords, jobs, metrics, alerts. Campaign
  sends: draft → audience preview → test → send, suppression enforced, CAN-SPAM postal footer
  from settings.
- **Analytics:** first-party cookieless (`analytics_events` — daily-rotating visitor hash, no raw
  IPs, country from CDN header, first-touch attribution cookie).

## 4–8. Design, dependencies, DB, APIs, permissions
All dispatchers are hosted background services with `WorkerLease` atomic claiming. SSRF guard
(`Core/Egress.cs`) blocks private-network egress on outbound connectors. DB: `comm_*` (11),
`email_logs`, `notifications`, `notification_history`, campaigns/suppression, `social_*`, `cc_*`
(syndication/backlinks/AI), `mkt_*` (26), `analytics_events`. Permissions: `comms`, `inbox`,
`emails`, the `cc_*` content family (16 keys, author→publish→legal separation), the `mkt_*`
family (11 keys — budgets and approvals split), `subscribers`, `social`.

---

<a name="module-11"></a>
# Module 11 — Casework, CPD, Identity & Compliance

## 1. Overview & business purpose
The certifying body's fairness and compliance machinery: appeals, accommodations, CPD, GDPR,
identity documents, the PCI Student Number estate, and honorary conferrals — all designed for
auditability (every staff read of student evidence is logged) and maker-checker separation.

## 2. Functional workflows
- **Appeals** (`Endpoints/Casework.cs`): result/invalidation appeals, complaints, ethics reports;
  ownership-verified references; one open appeal per attempt; decisions stamped
  (under_review/upheld/dismissed).
- **Accommodations:** disability/special-arrangement requests; approval sets
  `approved_extra_minutes` (clamped 0–120) which *genuinely extends* exam duration on both
  browser and desktop; approvals take MAX, never stack.
- **CPD:** student log + evidence → admin review (only approved hours count toward the target);
  events attendance auto-credits an approved entry **exactly once** (partial unique index on
  `(source_event_id, user_id)` — the ID-12 rule); annual declaration (compliant / career_break /
  not_met — "declared, not discovered"); recertification checkout enforces CPD-met.
- **GDPR:** self-service data export; erasure requests (one open, 30-day due clock) → admin
  acknowledge → complete (anonymise — payments and issued credentials retained de-identified) or
  reject with a legal-basis reason; student notified *before* anonymisation.
- **Identity documents:** upload (max 10), `submitted` satisfies the booking gate,
  verified/rejected on review; every staff view audited.
- **PCI Student Numbers** (`Core/StudentNumbers.cs`, Admin Identity console): the canonical
  public number lives in a **registry ledger** (`pci_student_number_registry` — states
  issued/merged/retired/quarantined) with `users.registration_no` as a projection; issuance
  happens only inside the creating transaction or an audited backfill (**deliberately no
  "type a number" endpoint or `id_issue` permission**); duplicate merges are maker-checker
  (`pci_identity_merges` — the approver can never be the requester, enforced by split
  `id_merge_request`/`id_merge_approve` permissions).
- **Honorary route:** public application → board review → shortlist-gated identity verification
  (photo + government ID only, no ID numbers stored, own retention clock) → conferral into the
  `PCI-HON-YYYY-NNNN` number space with its own verification endpoint — never touching exam data.
- **Support & error references:** unified inbox, tickets with SLA anchors, attachments through
  `Core/Storage` (ownership-joined reads, staff reads audited), student-quotable
  `PCI-YYYY-NNNNNN` error references.
- **Data retention** (`Core/RetentionService.cs`): daily background purge of evidence *bytes*
  past `evidence_retention_days` (metadata kept for audit); protected storage categories are
  never purged; `≤0` disables; first purge waits one full interval after boot.

## 3–8. Implementation, DB, APIs, permissions
Modules: `Casework.cs`, `AdminStudents.cs`, `AdminIdentity.cs`, `HonoraryApplication.cs`,
`HonoraryIdv.cs`, `Support.cs`; `Core/Erasure.cs`, `IdentityMerge.cs`, `StudentNumberBackfill.cs`.
DB: `appeals`, `accommodation_requests`, `cpd_entries`, `cpd_declarations`, `events` +
registrations, `erasure_requests`, `identity_documents`, `pci_student_number_registry`,
`pci_identity_merges`, `honorary_*` (5 tables), `tickets` + messages/notes/attachments,
`error_reports`, `security_events`, `fraud_flags`, `audit_logs`. Permissions: `tickets`,
`members`, `inbox`, `support_admin`, and the explicit-grant-only `identity` family (`id_read`,
`id_backfill`, `id_merge_request`, `id_merge_approve`, `id_audit`); honorary screens owner-only.

---

<a name="module-12"></a>
# Module 12 — AI Project Controls Simulation Lab

## 1. Overview & business purpose
A members-only lab of deterministic, scenario-based project-controls simulations (EVM, CPM,
forecasting, risk, cash flow, BoQ, portfolio…) — practice with computed measures and honest
grading, plus an authoring studio with a publication gate.

## 2–4. Features, implementation, design
- **Student side** (`/app/lab`, `LabRunner.tsx` — the largest portal screen): catalogue with
  server-side facets, attempt start/resume, multi-step scenarios where decisions affect later
  steps' givens, per-engine method reference (generic formulas, never an answer key), Assessment
  Mode withholds answers; deterministic server-side grading.
- **Engine:** `Core/SimCalc.cs` (1,197 lines, pure/deterministic — 19 engine families) shared as
  the reference solver with PCI World; `SimGrade`, `SimStep` (EffectiveGiven), `SimVariant`,
  `SimManifest`, `SimCoach`.
- **Studio** (`admin/pages/SimLab.tsx`): scenario lifecycle draft → calc_review →
  learning_review → safety_review → pilot → approved → published, with **maker-checker** approval
  and a validator that runs the reference solver over every asked measure before publish.

## 5–8. DB, APIs, permissions
Tables (`SimLabSchema`): `simulation_scenarios` (+versions), `simulation_entitlements`,
`simulation_attempts` (+events), `simulation_competency`. APIs: `/api/me/lab/*`; admin SimLab
suite. Permissions: `sim_lab` (grandfathered onto legacy `content` holders).

---

<a name="16-database-reference"></a>
# 16. Database Reference — All Domains & Key Entities

**~194 tables total:** 76 in `schema.sql` + ~57 net-new in `Migrate.cs` + ~65 via module schema
installers (World/Community/Media/Forum/Contributor/Careers/SimLab/Marketing/Finance/Templates).
Full column detail lives in `backend/schema.sql` (source of truth) and `Data/Migrate.cs`.

| Domain | Key tables |
|---|---|
| Identity & auth | `users` (+2FA/lockout/is_test), `pci_student_number_registry`, `pci_identity_merges`, `student_profiles`, `login_tokens`, `login_events`, `admin_users`, `admin_sessions`, `admin_reset_tokens`, `partner_users/_sessions`, `impersonation_sessions/_events`, `identity_documents`, `erasure_requests`, `security_events` |
| Certifications | `certifications` (+~25 catalogue/SEO cols), `certification_routes`, `certification_applications`, `bok_domains`, `sample_questions`, `issued_credentials`, `certificate_downloads`, `honorary_awards/_applications/_application_documents/_idv_documents`, `governance_roles` |
| Exams & results | `exam_entitlements`, `exam_bookings`, `exam_attempts`, `exam_score_snapshots` (immutable), `exam_launch_codes`, `exam_readiness_checks`, `exam_authorizations`, `exam_window_rules`, `exam_extension/reschedule_history`, `exam_attempt_grants`, `exam_incidents`, `exam_delivery_providers/orders/log`, `practice_attempts` |
| Proctoring | `proctor_events`, `exam_evidence`, `identity_checks`, `proctor_messages`, `candidate_consents`, `fraud_flags` |
| Money | `pricing_rules`, `discount_codes`, `payments`, `code_redemptions`, `checkout_reservations`, `memberships`, `membership_upgrades`, `enrollment_sessions`, `fee_waivers`, `webhook_events`, `training_partners` (+applications/docs), `partner_sponsorships/_payouts/_notices`, `founding_applications`, finance module (`partner_agreements`, `partner_commission_*`, `partner_settlement*`, `partner_disputes`, campaign links) |
| CMS/SEO | `pages`, `page_blocks`, `site_content`, `content_i18n`, `faqs`, `resources`, `news`, `nav_items`, `media_assets`, `seo_redirects/_submissions`, `analytics_events`, blog model (7 tables, versions immutable), content distribution (`content_capabilities`, `content_jobs`, `social_pub_accounts`, `cc_*`), documents ×3 modules (private/cert/public — all version-chained) |
| Casework/CPD | `appeals`, `accommodation_requests`, `cpd_entries`, `cpd_declarations`, `events` + registrations, `work_experiences`, `qualifications`, `held_certifications` |
| Support | `tickets` (+messages/notes/attachments), `support_templates`, `error_reports`, `inquiries`, `reviews`, `chat_sessions/messages/kb`, `forum_threads/posts/actions` |
| Comms | `email_logs`, `notifications`, `notification_history`, `email_campaigns/suppression`, `comm_*` (11 tables — outbox, templates, triggers, conversations, preferences, campaigns, routing) |
| Marketing/social | `social_accounts/_audit/_link_checks/_share_settings`, `mkt_*` (26) |
| Integrations | `integrations`, `integration_events/deliveries` (durable outbox), `certuvo_accounts` |
| Settings/audit | `site_settings` (universal k/v — nothing is hardcoded), `audit_logs`, `schema_migrations` |
| PCI World | `pciworld_*` (~65 tables across 6 schema installers) |
| SimLab / Templates | `simulation_*` (6), `templates` + download tracking |

**Idempotency by unique index (the pattern):** one score snapshot per attempt · one entitlement
per payment · one webhook event id · one code redemption per payment · exactly-once event CPD ·
one credential per attempt · outbox dedup keys · checkout/fee-waiver idempotency keys · one
registration per member per event. Partial indexes are stripped for MySQL (NULLs already exempt).

---

<a name="17-security-architecture"></a>
# 17. Security Architecture & Cross-Cutting Services

**Auth realms (never shared):**

| Realm | Table | Token TTL |
|---|---|---|
| Student | `login_tokens` | 30 days (session) · set_password 2–14 d · impersonation 1 h · portal handoff 90 s · desktop exam 6 h |
| Admin | `admin_sessions` | 12 hours |
| Partner | `partner_sessions` | 12 h / 7 d |
| World participant | `pciworld_user_sessions` | 30 days |
| World admin | `pciworld_admin_sessions` | 8 hours |

All tokens stored **SHA-256 hashed**; logout deletes the row. Passwords BCrypt (min 8 chars
everywhere); `VerifyPassword` returns false (never throws) on malformed hashes. TOTP 2FA (RFC
6238, ±1 window, replay-guarded by last-step, 10 hashed one-time recovery codes) for students and
admins.

**Brute-force defence in four layers:** IP fixed-window middleware (10/60 s on 28 POST paths,
last-XFF-hop keyed) → per-account lockout (10 fails → 15 min) → bcrypt timing equalisation for
unknown accounts → module-level throttles in 16 endpoint files.

**Storage (`Core/Storage.cs`):** 3 MB cap, MIME allowlist + magic-byte sniff + malware-scan hook
(`UploadScan`), content-addressed sharded paths, **encrypted at rest** (AES-256-GCM envelope,
`CREDENTIAL_ENCRYPTION_KEY`), traversal guards, local or S3 (misconfigured S3 throws — never a
silent local fallback), protected categories never auto-purged, retention purge deletes bytes but
keeps audit metadata.

**Other cross-cutting services worth knowing:** `H` (coercion + instant-based time helpers),
`Csv` (formula-injection-safe), `Egress` (SSRF guard), `WorkerLease` (atomic job claiming),
`ErrorRefs` (student-quotable references), `HtmlSanitize`, `Security.EncryptSecret` (`enc:v1:`),
`PdfWatermark`, `Notify`, `Lifecycle`, `MembershipGrades`.

**Recurring platform patterns (internalise these):**
1. Server-side redaction over client hiding (held results, password_hash, provider secrets).
2. Fail closed (webhook secret, vendor routing, unknown certification, missing idempotency key,
   half-configured S3/MySQL).
3. Idempotency by unique index, not application memory.
4. Never swallow a financial write failure behind a 200 (rethrow → Stripe redelivers).
5. Audit sensitive reads, not just writes.
6. High-risk capabilities (`finance`, `impersonate`, `test_users`, `identity`, `partner_finance`)
   are explicit per-person grants — never bundled into a job-title role.
7. Test accounts (`is_test=1`) are invisible to revenue, reports and the public register.
8. Backtick-quote dynamic identifiers (MySQL reserved words are invisible on SQLite).
9. Preserve and label, never delete (invalidated attempts, superseded documents, versions).

---

<a name="18-roles--permissions"></a>
# 18. User Roles & Permissions — Master Reference

**End-user roles:** Student (one role; feature switches via `sp_*` settings) · Partner user
(institution portal) · World participant · World employer member (tenant).

**Admin RBAC** (`Core/Security.cs` → `Rbac`): 13 section groups, 104 permission keys.

| Group | Keys |
|---|---|
| platform | overview, reports, audit, emails, settings, team, integrations |
| website | set_web, pricing, codes, content, sim_lab, pages, news, faqs, bok, governance, resources, media, nav, partners, social, sitesettings, subscribers, submissions, inquiries |
| student | set_sp, members, enrollments, payments, credentials, tickets, documents |
| exam | set_exam, exams, proctoring, sampleq, exam_delivery |
| exam_exceptions | ex_view, ex_extend, ex_reopen, ex_reschedule, ex_restore, ex_incidents, ex_invalidate, ex_bulk, ex_grant_replacement, ex_grant_additional, ex_approve, ex_correct_result, ex_waive_* (4) |
| operations† | finance, impersonate, test_users |
| identity† | id_read, id_backfill, id_merge_request, id_merge_approve, id_audit |
| support | inbox, support_admin, comms |
| content_centre | cc_view/author/edit/review/publish/seo/ai/social/syndicate/backlinks/integrations/legal/settings/links/archive/delete |
| marketing | mkt_view/connect/posts/publish/ads/gsc/promos/leads/leads_export/budgets/approve |
| partner_finance† | pf_view, pf_agreements, pf_prepare, pf_approve, pf_pay, pf_dispute |
| events | events_read, events_manage, events_checkin, events_attendance |

† = never in any named role bundle; owner + explicit per-person grants only. There is deliberately
no `id_issue` permission. `id_merge_request`/`id_merge_approve` and `pf_prepare`/`pf_approve` are
split so maker-checker is enforced by the permission model itself.

**Named roles:** `owner` (everything) · `website_manager` · `student_manager` · `exam_manager`
(+8 ex_* keys) · `viewer` · `support_agent` · `support_supervisor` · `content_manager` ·
`content_editor` · `content_author` · `custom` (explicit permissions JSON only). Per-admin extras
union on top; holding `content` grandfathers `sim_lab`.

**Gates:** `GateFn(req, section, ok)` → 401/403(`forbidden`+section)/ok — owner bypasses.
`OwnerGate` → 403 `owner_only` (Team & Access, honorary, translations, readiness, World launch,
storage purge). Settings key-prefix gating (read **and** write): `web_`→`set_web`, `sp_`→`set_sp`,
`exam_`→`set_exam`, else `settings`. Orthogonal axis: `cert_scope` limits non-owner admins to
specific certifications (`CertFilterSql`; legacy NULL = cert 1).

**PCI World:** separate `WorldRbac` (owner/author/reviewer/publisher/viewer, maker-checker).

---

<a name="19-configuration"></a>
# 19. Configuration & Environment Variables

~95 env vars are read; the essentials:

| Var | When | Notes |
|---|---|---|
| `DATABASE_FILE` | always | SQLite path; auto-set to `/data/pci.db` when a writable `/data` exists |
| `DB_PROVIDER` | optional | `sqlite` (default) or `mysql`/`mariadb` (+`MYSQL_*` or `MYSQL_CONNECTION_STRING`; retries with backoff, exit 75 on terminal failure) |
| `PORT` / `ASPNETCORE_ENVIRONMENT` | — | 8080; `Production` arms the validators |
| `APP_BASE_URL`, `ALLOWED_ORIGIN` | prod | public https URL; exact origin (no wildcard). Evidence-based adoption from `RENDER_EXTERNAL_URL` when unset |
| `CREDENTIAL_ENCRYPTION_KEY` | prod | secrets/files encryption; derived per-install key in dev; key loss makes artefacts unreadable (logged, not hidden) |
| `ADMIN_OWNER_EMAIL/PASSWORD` | first boot | bootstrap owner (`owner@pci.local` / `changeme-owner`), change forced |
| `ADMIN_RECOVERY_CODE`, `ADMIN_OWNER_RESET_PASSWORD` | break-glass | recovery endpoint; boot-time owner reset |
| `STRIPE_SECRET_KEY` + `STRIPE_WEBHOOK_SECRET` | payments | webhook secret required once the key is set; no key ⇒ payment endpoints 503 |
| `RESEND_API_KEY` / `SMTP_*` / `MAIL_FROM` | email | Resend → SMTP → console precedence |
| `STORAGE_PROVIDER`/`STORAGE_ROOT`/`S3_*` | storage | s3 without `S3_BUCKET` is a hard error |
| `PCIWORLD_*` | World | `ONLY`, `STANDALONE`, `HOSTS`, `ADMIN_HOSTS`, `BASE_URL`, `ALLOW_SQLITE`, `OWNER_PASSWORD` |
| `PORTAL_BASE_URL`/`PORTAL_HOSTS` | optional | mypci.org portal-domain separation |
| `ENABLE_LEGACY_ADMIN_TOKEN` | never | hard boot error in prod, never deferrable |
| `ALLOW_INSECURE_PRODUCTION`, `ALLOW_SQLITE_IN_PRODUCTION` | escape hatches | documented postures, still require `/data` persistence |

**Boot validation (two gates):** a pre-DB preflight (provider sanity, SQLite persistence proof,
MySQL completeness, URL/origin/key/webhook blockers — exit **78**) and `ConfigIssues()` after
build (email posture, S3, Stripe URL — remaining errors exit 78 unless overridden). Exit codes:
78 config, 75 DB/schema-compat, 70 migration error. Downgrade postures (SQLite-on-persistent-disk,
`PCIWORLD_ONLY`) convert specific errors to loud warnings — read the boot log.

---

<a name="20-deployment"></a>
# 20. Deployment Topology

- **Full platform** (`/Dockerfile`): node builds the four React bundles → .NET publish → runtime
  image with bundles at `wwwroot/{app,admin,world-app,world-admin-app}`. Baked:
  `DOTNET_EnableWriteXorExecute=0` (Render SIGSEGV fix) and workstation GC (512 MB containers).
  `/data` volume holds the SQLite DB + all uploads.
- **Render** (`render.yaml`): one web service, starter plan, health `/api/health`, 5 GB disk.
  Default SQLite-on-disk posture with MySQL keys pre-declared — "switching is a dashboard change";
  selecting MySQL with blank credentials still exits 78 (no silent fallback).
- **PCI World only** (`PCIWorld/Dockerfile`): backend-only image, `PCIWORLD_ONLY=true`, SQLite
  bridge or fail-closed MySQL (see Module 7).
- **holding/**: static coming-soon page for a static host.
- Any Docker host works behind a TLS-terminating proxy forwarding `X-Forwarded-Proto`.
- Domains: canonical `projectcontrolsinstitute.org`; optional `mypci.org` portal split; optional
  `pciworld.org`/`admin.pciworld.org` host mapping; retired domains 301.

---

<a name="21-testing--ci"></a>
# 21. Testing & CI

**Backend (from `backend/`):** Python suites against real databases — `lifecycle_test.py`,
`release_test.py`, `casework_test.py`, `settings_test.py`, `publication_test.py`,
`storage_test.py`, `integration_test.py` (adversarial, SQLite or MySQL via
`TEST_DB_PROVIDER=mysql`), `sweep_500_test.py` (every route × anon/student/owner — asserts zero
500s), `migration_integrity_test.py`; plus `smoke-test.sh` (live HTTP) and a 350-file .NET test
project. Suites pass only when every assertion prints `PASS`/`✓`.

**Frontend:** `npm run typecheck` (strict tsc) · `npm run build` (all four apps) · Vitest with
risk-based per-file coverage floors · 27 Playwright e2e specs (chromium full + cross-browser
smoke; `E2E_DB_PROVIDER=mysql` re-runs the same specs on MariaDB; `stage-bundles.sh` stages SPAs).

**SecureExam:** `dotnet build` + `dotnet test` (Core tests cross-platform) + the RunnableChecks
harness.

**CI (`.github/workflows/build.yml`, push to main + PRs):** `backend` (build → 6 logic suites →
boot → smoke → integration → 500-sweep → system-check probe) · `backend-mysql` (MariaDB 10.11
parity gate) · `frontend` (typecheck + build, fails on empty assets) · `secureexam-windows` ·
`secureexam-core-linux`. **Before pushing, run what CI runs for what you touched.**

---

<a name="22-gotchas"></a>
# 22. Known Documentation Drift & Gotchas

1. **Four frontend apps, not two** — older docs say two; `world`/`worldadmin` were added.
2. **Admin console has ~70 sections**, not "~29"; classic `admin.html` no longer exists
   (301 → `/admin/`), though several older docs still mention it.
3. Public site is **235 HTML files** (docs variously say 196/215/216).
4. SecureExam README calls the backend "Node… snake_case" — it is **C#/ASP.NET** emitting
   deliberate dual-cased aliases for the desktop client.
5. `/world-admin-app` is staged but has **no SPA deep-link fallback** in `Program.cs` (only
   `/app`, `/admin`, `/world-app` do).
6. `docs/` is a **historical archive** — its manifests reference zip bundles that are not the
   working source; `docs/PROJECT_STATUS.md`'s "built-in Certuvo engine" framing is superseded by
   `docs/CERTUVO_INTEGRATION.md` (external platform).
7. `.env.example` mentions a legacy `ADMIN_TOKEN` the code no longer honours.
8. PCI World routes 404ing on a fresh install is **the design working** (flags ship `'0'`).
9. Graceful degradation is intended behaviour: no Stripe → 503 payments only; no SMTP → console
   emails (logged); backend down → website fully static.
10. The exam-day browser runner and readiness check live in the classic `student.html` (token
    path), not the React portal; launch-code mint endpoints exist but no portal button calls them
    yet.
11. SecureExam has several dead config knobs and an unauthenticated reference-server hub — see
    Module 4 §9 before relying on them.

---

<a name="23-glossary"></a>
# 23. Glossary & Where-To-Look Index

| Term | Meaning |
|---|---|
| PCL-AI / PFL-AI / PML-AI | The three certifications (PCP-AI = legacy name of PCL-AI) |
| Entitlement | The right to sit one exam, minted only on settled payment or audited waiver |
| Authorization | The scheduling-window + attempt-policy record per entitlement |
| Held / auto_held | A result withheld pending review — never shows score/pass-fail anywhere |
| Launch code | Single-use, hashed, 15-min code handing a sitting to the desktop client |
| PCI Student Number | Canonical public identity number, ledgered in a registry, never hand-typed |
| PCI-HON | The honorary number space — board-conferred, never an examined credential |
| Founding route | Invitation-code route granting membership+study+exam together |
| Certuvo | External official practice platform, PCI-provisioned accounts |
| PCI World / Passport | The free challenge platform and its evidence profile |
| Launch board | Owner-only screen turning PCI World features on (flags + signed acknowledgements) |
| Crud factory | `Crud(...)` + `crudConfigs.ts` — one line + one entry = a full admin collection |
| Stage 2 / Stage 3 | Server-rendered CMS pages / the React SPA screens |
| Maker-checker | Requester and approver must differ (merges, settlements, World editorial, SimLab) |
| is_test | Test accounts — invisible to revenue, reports, and the public register |

| Need | Where |
|---|---|
| Boot, middleware, inline endpoints | `backend/Program.cs` |
| Data layer / SQL dialect | `backend/Data/Db.cs`, `backend/MYSQL.md` |
| Schema / migrations | `backend/schema.sql`, `backend/Data/Migrate.cs` |
| Auth / RBAC | `backend/Core/Auth.cs`, `backend/Core/Security.cs` |
| Exam pipeline | `backend/Endpoints/StudentExam.cs`, `ExamClient.cs`, `Core/Lifecycle.cs` |
| Result review | `backend/Endpoints/AdminProctoring.cs` |
| Payments | `backend/Endpoints/Payments.cs`, `Public.cs` |
| CMS | `backend/Core/PageContent.cs`, `PageScan.cs`, `ListSections.cs`, `CertCatalogue.cs`, `PriceTags.cs` |
| Admin CRUD factory | `backend/Endpoints/AdminMgmt.cs` + `frontend/src/admin/crudConfigs.ts` |
| PCI World | `backend/Endpoints/World*.cs`, `Core/World*.cs`, `docs/pciworld/ARCHITECTURE.md`, `PCIWorld/README.md` |
| World launch flags | `backend/Endpoints/WorldLaunch.cs` |
| SecureExam | `secureexam/` (`README-SECUREEXAM.md`, `build.ps1`, `PCI.SecureExam.Core/`) |
| Certuvo | `backend/Core/Provisioning.cs`, `docs/CERTUVO_INTEGRATION.md` |
| Comms/marketing | `backend/Endpoints/CommsCentre.cs`, `ContentCentre.cs`, `MarketingCentre.cs`, `Core/Mailer.cs` |
| Identity console | `backend/Core/StudentNumbers.cs`, `Endpoints/AdminIdentity.cs` |
| Deploy | `Dockerfile`, `render.yaml`, `DEPLOY.md`, `PCIWorld/` |
| CI | `.github/workflows/build.yml` |
| Business/brand facts | `backend/wwwroot/certification.html`, `about.html`, `candidate-journey.html` |

---

*Generated from a full-codebase sweep of the PCI repository (backend core, all 71 endpoint
modules, ~194 database tables, the four React apps, SecureExam, PCI World, and the docs archive).
When this document and the code disagree, the code wins — please update this file.*
