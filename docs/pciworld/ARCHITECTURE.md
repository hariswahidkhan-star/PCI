# PCI World — Architecture Decision Record & Current-System Audit

_Status: accepted for Phase 0 + Phase 1 vertical slice. Baseline: `main` @ `47f8d51`, 2026-07-24._

PCI World (pciworld.org — "Make the decision. Control the outcome.") is a global, free-to-enter
project challenge, simulation and professional-evidence platform operated by the Project Controls
Institute. This ADR records how it is built inside this repository without compromising the
Institute platform, and which product rules are structural (enforced in code) rather than
editorial.

## 1. Current-system audit (what exists that PCI World may and may not touch)

| Existing asset | Reuse decision |
|---|---|
| ASP.NET Core 8 backend, single app (`backend/`) serving website + APIs | PCI World is an isolated module inside it (`Core/World*.cs`, `Data/WorldSchema.cs`, `Endpoints/World*.cs`) with its own routes, tables and auth realm. Shared infrastructure (hosting, Db layer, mailer, security helpers) — shared UI never. |
| `Db` layer: SQLite dialect in dev, translated to **MySQL 8** at runtime (`DB_PROVIDER=mysql`) | Reused. Production data is MySQL 8; SQLite remains the dev/test default exactly as for the rest of the platform. Migration parity gates cover the new tables automatically because the schema installer runs on both providers. |
| `admin_users` / PCI admin SPA (`/admin`) | **Not touched.** PCI World admin is a separate realm (`pciworld_admin_users` + `pciworld_admin_sessions`), separate login, separate UI at `/world-admin`, no navigation links in the PCI admin SPA, and none added there. Precedent: the partner portal (`partner_users`), already "wholly separate from admin_users and students". |
| Students (`users`), exams, entitlements, credentials, membership | **Never read or written by PCI World.** Challenge play is anonymous in this slice. Structural rule: no PCI World code path touches `exam_attempts`, `exam_entitlements`, credentials, membership or `users`. |
| `SimCalc` (18 deterministic engines), tested by 200+ unit/content tests | Reused as the deterministic reference solver for numeric challenge tasks. Sharing a calculation library is infrastructure sharing, not product blending. |
| `simulation_*` Simulation Lab | Separate product, untouched. PCI World links to it as a progression path. |
| Security helpers (`Security.Sha/RandomHex/VerifyPassword`, `LoginGuard`, throttles) | Reused. |
| Static website + `PageContent` injection engine | **Not used.** PCI World public pages are self-contained server-rendered HTML from the World module — distinct identity, no coupling to the Institute page shell. |

## 2. Decisions

1. **Isolated module in the monorepo** (not a second repository). Rationale: shares the proven
   Db/MySQL translation layer, security primitives, CI gates and deploy pipeline; isolation is
   enforced by prefixes and realms (`pciworld_` tables, `/world*` routes, separate auth), which
   are testable, rather than by repo boundaries, which are not available here.
2. **Separate admin realm from day one.** `pciworld_admin_users`, bcrypt hashes, per-account
   lockout, sha-stored session tokens, role-based server-side authorization, and an append-only
   `pciworld_audit` log. The admin UI is served at `/world-admin` (deployable behind
   `admin.pciworld.org`); the PCI admin SPA contains zero references to it.
3. **Immutable published versions from day one.** Publishing snapshots the full challenge config
   into `pciworld_challenge_versions`; attempts pin `(challenge_id, version)` and replay **only**
   from the snapshot. This encodes the P0 lesson already learned (and later fixed) in the
   Simulation Lab: the live row is never the replay authority.
4. **Deterministic scoring only.** Numeric tasks resolve through `SimCalc`; decision tasks score
   against authored option-quality rubrics; the decision profile derives from dimension scores by
   fixed rules. No model output ever grades an attempt.
5. **Anonymous-first.** A visitor completes today's challenge with no account; an opaque session
   token (sha-stored, httpOnly-cookie-free — sent as a bearer-style header/localStorage value by
   the page script) keys autosave and results. Participant accounts/Passport are Phase 1b backlog.
6. **Opaque, revocable public tokens.** Result verification and friend invitations use
   `RandomHex(32)` tokens stored only as SHA-256; URLs contain no email, user id or sequence.
7. **Server-rendered public pages.** `/world`, `/world/challenge/...`, `/world/r/{token}`,
   `/world/i/{token}` render complete HTML with unique titles, meta descriptions and Open Graph
   tags — indexable without a JS build, fast, and visually independent of the Institute site.
8. **UTC challenge day.** The daily challenge rotates at 00:00 UTC (shown explicitly on the page);
   an admin calendar overrides the default deterministic rotation over the published set.
9. **Honesty is structural.** No participant counts, testimonials, partner logos, percentiles or
   leaderboards are rendered anywhere in this slice — the templates have no slots for them.
   Rankings remain feature-flagged off until governed thresholds exist (see PLAN.md).
10. **No new secret stores, no hard-coded URLs.** Institute link targets and feature flags are
    `site_settings` keys (`world_*`); production hostnames are deployment configuration.

## 3. Route map

| Route | Kind | Auth | Purpose |
|---|---|---|---|
| `GET /world` | HTML | none | Home: today's challenge, how it works, Institute links, operated-by disclosure |
| `GET /world/challenge/{code}` | HTML | none | Challenge briefing + workspace (one page, progressive enhancement) |
| `GET /world/archive` | HTML | none | Published challenge archive (metadata only, no answers) |
| `GET /world/about` | HTML | none | About PCI World + relationship to the Institute + certification explainer links |
| `GET /world/r/{token}` | HTML | opaque token | Public verified result (participant-controlled name, no answers) |
| `GET /world/i/{token}` | HTML | opaque token | Friend invitation → same challenge/version |
| `POST /api/world/session` | JSON | none | Mint anonymous session |
| `GET /api/world/today` | JSON | none | Current challenge (public fields only) |
| `POST /api/world/attempts` | JSON | session | Start/resume attempt (pins version) |
| `POST /api/world/attempts/{id}/save` | JSON | session | Autosave answers (idempotent) |
| `POST /api/world/attempts/{id}/submit` | JSON | session | Deterministic grade (idempotent, first submit wins) |
| `GET /api/world/attempts/{id}` | JSON | session | Load own attempt/result |
| `POST /api/world/attempts/{id}/share` | JSON | session | Mint/revoke result token; set display name |
| `POST /api/world/attempts/{id}/invite` | JSON | session | Mint friend invitation |
| `POST /api/world-admin/auth/login|logout|password` | JSON | world-admin | Separate admin auth |
| `GET/POST /api/world-admin/challenges…` | JSON | world-admin RBAC | Author/validate/review/approve/publish/revise/retire |
| `GET/POST /api/world-admin/calendar` | JSON | world-admin RBAC | Daily schedule |
| `GET /api/world-admin/audit` | JSON | world-admin RBAC | Audit log |
| `GET /world-admin` | HTML | world-admin | Separate admin application shell |

Deployment mapping (documented, not hard-coded): `pciworld.org` → `/world*`;
`admin.pciworld.org` → `/world-admin*` (+ its API); `projectcontrolsinstitute.org` remains the
Institute site. `pciworld.ai`, if acquired, 301s to a real route.

## 4. User-role matrix

| Role | Realm | Can |
|---|---|---|
| Anonymous visitor | public | View pages, start/complete daily challenge, see own result |
| Session holder | public | Resume, autosave, share/revoke own result, invite a friend |
| `owner` | world-admin | Everything incl. admin-user management |
| `author` | world-admin | Create/edit drafts, run validation, submit for review |
| `reviewer` | world-admin | Review, comment, approve (never own drafts — maker-checker) |
| `publisher` | world-admin | Publish approved, schedule calendar, retire/restore |
| `viewer` | world-admin | Read-only (analytics/audit) |

All enforcement is server-side per endpoint; UI affordances are never the authorization.

## 5. Non-negotiable product rules encoded in this slice

- Every public page: header link "Visit the Project Controls Institute ↗"
  (`rel="noopener noreferrer"`, marked external), footer line "PCI World is a global learning and
  challenge platform operated by the Project Controls Institute."
- Every challenge and result: "PCI World challenges are educational practice — not PCI
  certification examinations. Completing a challenge does not grant or affect any PCI
  certification, membership or credential."
- PCI World tables have no foreign keys into exam/credential tables and no code path reads them.
- Published versions immutable; attempts replay from version snapshots only.
- No fabricated social proof anywhere.
