# PCI World — Delivery Report (Phase 0 + Phase 1 vertical slice)

_Date: 2026-07-24 · Branch: `claude/sim-lab-next-release-traceability-6mwshf` · Base: `main` @ `47f8d51`._

Scope honesty first: the master prompt's full programme (365 challenges, Passport, Coach,
Human vs AI, World Project Series, universities, employers, rankings, localization, Playwright,
load/DR) does not fit safely in one change. Per the prompt's own fallback rule, this delivery is
**Phase 0 (foundation) plus the largest complete Phase 1 vertical slice**, leaving the repository
green, with the exact backlog in PLAN.md. Nothing unbuilt is claimed below.

## Architecture and deployment

- Isolated module in the platform monorepo: `backend/Core/World*.cs`, `backend/Data/WorldSchema.cs`
  + `WorldContentPack.cs`, `backend/Endpoints/World.cs` + `WorldAdmin.cs`, tests in
  `WorldTests.cs`, docs in `docs/pciworld/` (ADR, threat model, data/content model, plan).
- Public surface `/world*` (deployable at pciworld.org), separate admin `/world-admin*`
  (deployable at admin.pciworld.org). No hard-coded URLs — Institute link and flags are
  `site_settings` (`world_*`); kill switch `world_enabled`.
- Data: `pciworld_*` tables via the platform `Db` layer — SQLite in dev, **MySQL 8 in
  production** (`DB_PROVIDER=mysql`), same as the rest of the platform; installer runs on both
  providers so parity gates apply.

## What a visitor can do today (all verified over HTTP)

Anonymous journey: open `/world` → today's challenge (UTC rotation + admin calendar override) →
briefing → workspace (evidence table, numeric asks, decision radios, autosave, resume) →
idempotent submit → deterministic result (dimension scores, defensible decision profile with
reason + improvement area, per-measure reference values, decision consequences/principles) →
opaque shareable verified-result URL (participant-controlled name, revocable, no answers on the
public page) → challenge-a-friend invitation pinned to the exact same challenge version.

## Separate administration (verified over HTTP)

Own realm (`pciworld_admin_users`/`_sessions`, bcrypt + lockout + sha-stored tokens), own UI at
`/world-admin`, roles owner/author/reviewer/publisher/viewer enforced server-side, lifecycle
draft → in_review → approved → published with independent maker-checker, validator gating at
approve AND publish, immutable version snapshots, revise-as-new-version, retire/restore, daily
calendar, append-only audit log, admin user management. The PCI admin SPA contains zero
references to PCI World (grep-verified) and a live PCI admin session token is rejected by the
world-admin realm (test-verified).

## Content

10 pilot challenges (`WC-…-001…010`), all synthetic, all passing the publication validator and a
full reference solve in CI: EVM, CPM, risk/EMV, cash flow, change control, progress, earned
schedule, PERT, EVM timeline, and a governed-AI audit challenge. Difficulty foundation×2,
developing×2, professional×3, advanced×2, expert×1; tracks project_controls×5, project_finance,
project_management, cross_functional×2, governed_ai; 10 industries. Counts by status at seed:
10 published / 0 draft. **365 is not claimed; 30/90/180/365 are later release gates.**

## Evidence (this environment, commands run)

| Gate | Result |
|---|---|
| `dotnet build -c Release` | clean |
| `dotnet test --filter FullyQualifiedName~World` | **11/11 passed** |
| `dotnet test` (full backend) | **649/649 passed** |
| `python3 tests/migration_integrity_test.py` | **13/13 passed** (MySQL provider-parity step self-skips without a MySQL service) |
| `python3 tests/integration_test.py` | **1110/1110 passed**, exit 0 |
| Live HTTP smoke | home/institute links; session→start→save→submit; double-submit 409; cross-session 404; share→verify→revoke 404; invite page; admin login/lifecycle/RBAC 403/401s; PCI-admin-token rejection |
| Public-page leakage | workspace payload and public result page contain no qualities, consequences, reference values or stored answers (test + live checks) |

## Security posture (THREAT_MODEL.md, all mitigations implemented in-slice)

Allow-listed public payloads; session-scoped ownership on every attempt route; 128-bit sha-stored
revocable tokens; first-submit-wins; server-side RBAC + maker-checker in SQL; realm-separated
sessions; bcrypt + per-account lockout + timing equalisation; parameterized SQL; HTML-encoding of
all dynamic text; per-key rate limits; explicit field mapping (no mass assignment); append-only
audit. Accepted residuals (tracked): no MFA yet, no CAPTCHA, OG-image PNG deferred.

## Institute relationship (structural)

Header + footer Institute links (`rel="noopener noreferrer"`, external marked, new tab) on every
page; footer operated-by line; practice-not-certification notice on challenge, result,
verification, invite and about pages — all rendered from constants. No PCI World code path
touches exams, entitlements, credentials, membership or `users`. **PCI World activity does not
grant or modify any formal PCI certification — structurally, not just editorially.**

## Phase 1b addendum — participant accounts + PCI World Passport (same branch, second commit)

- `pciworld_users` / `_user_sessions` / `_user_tokens`: a separate practice-identity realm — a
  world account creates no row in the platform's `users`, and a student session token is
  meaningless to the world-account resolver (test-verified). bcrypt + lockout + timing
  equalisation + sha-stored 30-day sessions.
- **Anonymous-first is preserved**: register/login "claim" the caller's anonymous session — only
  unclaimed attempts, never another account's (test-verified).
- **Passport is consent all the way down**: evidence is opt-in per completed attempt (default
  hidden); publication requires a verified email AND a chosen display name; the public URL is an
  opaque token that rotates on republish and dies on unpublish; the page vocabulary is fixed to
  "verified virtual project experience" — never a credential. Export (JSON) and delete
  (de-identify attempts, revoke all public surfaces) are self-service.
- Email verification uses the platform mailer: Resend/SMTP in production, console sink + email
  log in dev — no new email infrastructure.
- Pages: `/world/account` (register/sign-in/manage), `/world/p/{token}` (public Passport),
  `/world/verify-email`; result page now carries the Passport upsell.
- Evidence: World tests **17/17** (6 new: claiming, login hygiene/lockout, publication gates +
  token rotation, per-item consent, delete de-identification, realm separation); live HTTP smoke
  of the entire journey including the verification link, zero email leakage on the public page,
  and 404 after unpublish. Full-suite results recorded in the PR.

## Phase 2 slice addendum — 30-challenge library, archive filters, password reset

- **Content: 10 → 30 validated challenges** (`WC-…-001…030`), now spanning **all 18 deterministic
  engine families** (adding productivity, BoQ, resource levelling, procurement delay, portfolio
  scoring, weighted decision, data quality), all five tracks, five difficulty levels and 20+
  industries (oil & gas, mining, ports, water, defence, telecoms, events, real estate, marine,
  education, PMO…). Every ask reference-solves in CI; the pack test now also asserts full
  difficulty/track coverage and ≥15 industries. Counts by status at seed: 30 published, 0 draft.
  This satisfies the Foundation-launch content floor; independent SME review before public launch
  remains a stated gate, not a claim.
- **Archive filters**: server-side industry/difficulty/track filtering (exact-match parameters,
  enum-validated; injection-shaped values are simply ignored) with an accessible GET filter form.
- **Password reset**: no-enumeration responses (identical message for unknown addresses),
  2-hour single-use sha-stored tokens, reset revokes all sessions, mail via the platform sink.
  Live-verified: old password 401, new password 200, token reuse rejected.
- Test-guard refinement: the workspace no-leak test now asserts absence of rubric/solver **JSON
  keys** (`"quality":` …) rather than banning English words — structure is the guarantee.
- Evidence: World tests **17/17** with the 30-challenge gates; live smoke of filters and the
  full reset journey; full-suite results in the PR.

## Not built (do not present as existing)

Share-image PNG rendering,
AI Coach, Human vs AI live feature (one governed-AI *challenge* ships), World Project Series,
universities/employers, rankings/leaderboards (none rendered anywhere), Arabic localization of
PCI World copy, Playwright/axe automation for `/world`, MFA, hostname wiring, CV-bullet/LinkedIn
generators for the Passport. Each is scheduled in PLAN.md.
