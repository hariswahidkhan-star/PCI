# PCI World — Participant Journey Repair: Issue Register & Traceability

Audit basis: read-only audit at commit `b678f70` (the "PCI World Shared Identity, Premium
Cross-Portal Navigation, Student Profile, Login, Dashboard, and End-to-End Journey Repair"
master prompt). Repairs landed as thirteen reviewed increments; iterations 1–6 merged to `main`
as `3c9f276`, iterations 7–13 follow on `claude/pci-world-journey-repair-5epmbn`.

Every row below reached **verified**: reproduced against the audited commit, fixed, and pinned by
regression tests that run in the ordinary backend suite (`backend/tests/PCI.Backend.Tests`).
Suite counts per iteration: it1 961 · it2 972 · it3 978 · it4 983 · it5 986 · it6 986 · it7 992 ·
it8 993 · it9 996 · it10 1000 · it11 1004 · it12 1006 · it13 1008 — all green at each push.

## Issue register

| ID | Finding (audit) | Sev | Root cause | Fix | Key files | Tests (suite) | Commit |
|----|-----------------|-----|------------|-----|-----------|---------------|--------|
| P0-00 | Separate `pciworld_users` credential account contradicts the one-identity model | P0 | World built as an isolated realm with its own email+bcrypt | Canonical-identity bridge: `pciworld_participants` keyed by `users.id` (product data only), reversible append-only `pciworld_user_map` with LINKED/CREATED/CONFLICT-quarantine rules run at boot + registration + portal bridge; World registration mints exactly one canonical `users`+`student_profiles` pair (bcrypt hash preserved); ownership dual-stamping `canonical_user_id` on attempts with idempotent backfill + `CutoverReady` audit failing closed on conflicts | `Core/WorldIdentity.cs`, `Data/WorldSchema.cs`, `Endpoints/WorldAccount.cs`, `Core/WorldAttempts.cs` | `WorldIdentityTests` (13 facts) | aab8131, 2c384ce |
| P0-01 | Signed-in challenge work not attached to the account | P0 | Attempt INSERT never wrote `user_id`; workspace never sent the account header; SSO bridge never claimed the anonymous session | Ownership at creation: start/submit stamp the owner (adopt-on-resume/submit is claim-only-unowned, never reassigned); workspace sends `X-World-Account`; SSO claims the browser's current anonymous session; cross-device resume by account | `Core/WorldAttempts.cs`, `Endpoints/World.cs`, `Core/WorldPages.cs`, `Endpoints/WorldAccount.cs` | `WorldJourneyTests` (5 ownership facts) | db76839 |
| P0-02 | Portal→World bridge shipped a reusable 30-day bearer token through the portal origin | P0 | SSO returned the raw World session token in JSON for localStorage | One-time, two-minute, hashed, single-consumption handoff codes carried in the URL **fragment**; replay/expiry/never-existed indistinguishable; allow-listed `returnTo` (no open redirect); expired/consumed codes swept; World login also accepts canonical PCI credentials directly | `Endpoints/WorldAccount.cs`, `Data/WorldSchema.cs`, `frontend/src/pages/Profile.tsx`, `Core/WorldRotation.cs` (sweep) | `WorldHandoffTests` (3), `WorldIdentityTests` same-credential facts, sweep fact in `WorldOnboardingTests` | e2b905f, aab8131, b51beb4 |
| P0-03 | Hidden random World passwords made security/deletion inconsistent | P0 | Bridge-created accounts held a password nobody knew; deletion demanded it | `VerifyAccountPassword`: the canonical PCI credential is the step-up authority for delete/password-change (legacy World hash transitional); `DeleteAccount` is explicitly **World-only scope** — canonical identity, credentials and student profile provably survive; export/deletion labels say so | `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs` | `WorldHandoffTests` step-up + deletion-scope facts | e2b905f |
| P0-04 | Submit/reload could strand a completed result | P0 | Retry got 409; start only resumed `in_progress`, so reload duplicated | Idempotent submit (structural-JSON-equal replay → stored result; different payload refused; concurrent duplicate answered from the stored row); reload of a completed challenge returns its result; retake is explicit with `parent_attempt_id` lineage | `Core/WorldAttempts.cs`, `Endpoints/World.cs`, `Data/WorldSchema.cs` | `WorldJourneyTests` idempotency/reload/retake facts | db76839 |
| P0-05 | Daily version/pause unstable | P0 | Today API and start used mutable `current_version`; pause across midnight served nothing | `CurrentPeriod`/`TodayPinned`: the rotation period is the version authority for home, Today API and attempt start; midday publish never moves the day; pause keeps the previous period on the air (`paused=true`) | `Core/WorldRotation.cs`, `Core/WorldLifecycle.cs`, `Endpoints/World.cs` | `WorldJourneyTests` pin/pause facts | db76839 |
| P0-06 | Passport expiry silently removed by unrelated saves | P0 | Dropdown defaulted to "never" and always submitted; server applied it | Expiry is an independent persisted field (`ApplyDisclosure`: absent keys change nothing); UI shows "Keep current expiry (date)" and only sends a deliberate change | `Core/WorldPassport.cs`, `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs` | `WorldJourneyTests` expiry fact | db76839 |
| P0-07 | Reusable tokens in localStorage; no cookie/session hygiene | P0 | Bearer-in-localStorage design | Transitional dual acceptance: HttpOnly, SameSite=Strict, Secure-when-HTTPS session cookie set at login/register/handoff (header wins); logout revokes both carriers; sessions listable and revocable (one/all-others) with immediate effect | `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs` | `WorldVerificationAndScaleTests` cookie facts, `WorldSessionsAndPrefsTests` | 8281d75, c04534b |
| P1-01 | `/world/account` was not a dashboard | P1 | One dense page for everything | Server-side dashboard aggregate (`WorldDashboard.Build`, one call): identity/verification, today per-owner state, whole-history progress, Passport readiness ladder, recent + open work, recommendation, streak, weekly progress, exactly ONE primary action (§9.1 order incl. onboarding); account page renders it as the page head | `Core/WorldDashboard.cs`, `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs` | `WorldDashboardTests` (4) | 6ca3f56, 6235e70 |
| P1-02 | No onboarding state machine | P1 | Registration dropped users into the full Passport manager | §7.3 machine on the participation row (`not_started→welcome→goal→preferences→privacy→completed`), order server-enforced, resume idempotent, completion stamps `onboarded_at` once; one-time grandfathering backfill for established accounts; resumable walkthrough card | `Core/WorldIdentity.cs`, `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs` | `WorldOnboardingTests` (6) | d54d406 |
| P1-03 | GET consumed the verification token | P1 | Scanner/preview fetches burned single-use links | GET only looks (confirm/already/invalid pages, token never embedded in HTML); POST consumes race-safely; resend cooldown kept; queued (not "sent") truthfulness | `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs` | `WorldVerificationAndScaleTests` (2) | 8281d75, 2c384ce |
| P1-04 | Errors masqueraded as sign-out | P1 | One catch-all showed the login panel | Only a rejected token shows sign-in; network/5xx keep the session, distinguish offline vs service-down, offer retry | `Core/WorldPages.cs` | manual + JS-parse gate (UI-only) | e2b905f |
| P1-05 | Publish-before-consent; visible-by-default disclosure | P1 | No server guard; schema defaults 1 | `PublishPassport` refuses zero selected evidence; NEW accounts default all disclosure OFF (existing rows keep behaviour — both proven) | `Endpoints/WorldAccount.cs`, `Data` inserts | `WorldAccountTests`, `WorldPassportTests` updated facts | 8e844fe |
| P1-06 | 200-row cap corrupted counts; PDF stopped silently | P1 | Stats derived from a capped list | `EvidenceStats` SQL aggregates; offset pagination + `evidence_total` + Load more; public page and PDF use whole-history counts and state "showing X of Y" when windowed | `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs`, `Core/WorldPassport.cs` | `WorldVerificationAndScaleTests` scale + PDF facts | 8281d75 |
| P1-07 | No returning-user loop | P1 | Nothing surfaced state | Account-aware navigation; resume lists; recent results; explicit retake; fair streak from `rotation_period_id` provenance (retakes/invites never count; no-blame grace); weekly progress vs chosen target; goal-aware rule-based recommendation (honestly labelled) | `Core/WorldDashboard.cs`, `Core/WorldAttempts.cs`, `Core/WorldPages.cs` | `WorldDashboardTests` streak fact, `WorldSharingTests` (2) | d54d406→b1abc95 |
| P1-08 | Existing APIs not surfaced | P1 | No coherent participant surface | Sessions view, sharing management (result links + invitations with account-scoped revoke/revoke-all), preferences, shared profile card, export — all reachable from the account page | `Core/WorldPages.cs`, `Endpoints/WorldAccount.cs` | `WorldSessionsAndPrefsTests`, `WorldSharingTests` | c04534b, b1abc95 |
| P1-09 | World buried at the bottom of Profile | P1 | No nav/Overview entry | PCI World in the portal sidebar (localized, 7 languages) + Overview "Daily practice" card with secure deep entry (`return_to` through the handoff) | `frontend/src/components/Layout.tsx`, `Overview.tsx`, `i18n/catalog.ts` | typecheck + build gates | 6ca3f56 |
| P1-10 | World never reused the canonical profile | P1 | No bridge existed | `GET/PATCH /api/world/me/profile` reads/writes the SAME canonical `users`/`student_profiles` rows (portal's allow-list minus `profile_photo`, same caps/quoting/completion recompute); edits visible both ways on next read; smuggled identity/credential fields ignored; shared data never public without Passport consent | `Core/WorldIdentity.cs`, `Endpoints/WorldAccount.cs` | `WorldSharedProfileTests` (5) | 8e844fe |
| P1-11 | Cross-product movement not a journey | P1 | One buried card, portal→World only | Bidirectional: portal nav+Overview card → World (secure deep entry); World dashboard "PCI ecosystem" card → student portal with honest linked/not-linked state; aggregate exposes per-product entry states (`onboarding_required/active`, `ready/not_linked`) | `Core/WorldPages.cs`, `Endpoints/WorldAccount.cs` | aggregate facts in `WorldOnboardingTests` | b51beb4, d54d406 |
| P2 (slices) | Admin gaps affecting the journey | P2 | — | Role-aware admin shell (`/auth/me`, WorldRbac-mirrored controls), owner-only Users tab (invite/suspend/role with self + last-active-owner guards, sessions revoked on narrowing), rotation console pinned-vs-current version display, support participant lookup (states/counts only — answers/tokens/disclosure/photos provably absent) | `Endpoints/WorldAdmin.cs`, `Core/WorldIdentity.cs` | `WorldOnboardingTests` diagnostics fact | 9c89e69, b51beb4 |
| PW-US-046 | Suspension/deactivation mislabelled or leaky | P1 | Suspended sessions collapsed to 401 "not registered"; a deactivated canonical identity kept World access via the legacy password | `AccountState` resolves "suspended" distinctly; dashboard/passport answer 403 `account_suspended` with a dedicated UI screen (support path, PCI-account reassurance); World suspension provably never touches the canonical row; global deactivation blocks the legacy World password too | `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs` | `WorldIdentityTests` suspension fact | 168db7c |
| PW-US-010/043 | Sign-out scope invisible; no central sign-out | P1 | Logout only killed the presented carrier | Logout accepts `{everywhere:true}`: revokes ALL of the account's World sessions and — when canonically linked — the canonical portal sessions (`login_tokens`, purpose session), audited; suspension never blocks sign-out; two-choice UI with plain scope copy; bystanders' sessions untouched (proven) | `Endpoints/WorldAccount.cs`, `Core/WorldPages.cs` | `WorldSessionsAndPrefsTests` sign-out fact | it16 |
| P0-00 (read flip) | Evidence reads keyed only on the legacy namespace | P0 | Pre-cutover queries | `EvidenceRows`/`EvidenceStats` (and everything they feed: account view, public Passport, PDF) now use the union ownership read (legacy OR canonical stamp) — canonical-only rows included, dual-stamped rows never double-counted, strangers excluded (proven) | `Endpoints/WorldAccount.cs` | `WorldSessionsAndPrefsTests` union fact, `WorldDashboardTests` union fact | 53fdbeb, it16 |
| P0-00 (flip complete) | Remaining owner-keyed queries on the legacy namespace | P0 | Pre-cutover queries | Union predicate everywhere an owner keys a query: streak + weekly joins, dashboard visible-evidence count, the publish no_evidence guard, sharing lists, share/invitation revoke + revoke-all, and world-only deletion's whole de-identification sweep (canonical-only rows are revoked, stripped and de-identified too — no orphaned personal rows survive) | `Core/WorldDashboard.cs`, `Endpoints/WorldAccount.cs` | `WorldSessionsAndPrefsTests` streak/publish/shares/deletion union fact | it17 |

## Architecture delta (before → after)

- **Identity**: separate `pciworld_users` email+password authority → one canonical `users` identity;
  World holds a `pciworld_participants` participation aggregate (product data only) and a
  reversible `pciworld_user_map` ledger; email collisions quarantine, never merge. The World login
  accepts canonical PCI credentials (canonical hash is the authority); registration creates
  exactly one canonical identity.
- **Sessions/tokens**: SSO minted a reusable 30-day bearer into portal localStorage → one-time
  fragment-carried handoff codes + an HttpOnly/Strict cookie set alongside the header token
  (transitional dual acceptance, header wins), listable/revocable sessions, both carriers revoked
  at logout, expired artefacts swept.
- **Attempts**: session-owned only, mutable daily version, 409 dead-ends → owned at creation in
  BOTH namespaces (`user_id` + `canonical_user_id`), rotation-period version pinning with daily
  provenance, idempotent submit, immutable results, explicit retake lineage.
- **Endpoints added** (participant): `me/dashboard`, `me/profile` (GET/PATCH), `me/preferences`
  (GET/PATCH), `me/onboarding` (GET/POST), `me/sessions` (+revoke, revoke-others), `me/shares`
  (+revoke, revoke-all, invitations/revoke, revoke-all), `account/handoff`,
  `account/verify-email` (POST). Admin: `auth/me`, `users/{id}/role`, `participants?q=`.

## Remaining program-scale work (honestly unstarted)

These are the master prompt's re-platforming blocks — team-scale efforts beyond incremental
repair, listed so nobody mistakes the register above for "done":

1. Separate per-domain React applications (PCI World participant app on `pciworld.org`, distinct
   build artefacts, CSP, sessions) — the current surfaces remain server-rendered pages with
   embedded JS on the shared origin.
2. A real OAuth 2.1/OIDC authorization service (state/nonce/PKCE, client registry, audience-scoped
   sessions) replacing the transitional handoff-code bridge for the dedicated-domain shape.
3. Attempt-namespace cutover: the APP-LAYER flip is complete — every owner-keyed read and write
   (dashboard, evidence, streak, publish guard, sharing, deletion) uses the union predicate
   (legacy OR canonical stamp) with `canonical_user_id` stamped at every ownership event and
   backfilled at boot. What remains is the DB-layer retirement: once `CutoverReady` holds in
   production, drop the union back to canonical-only and retire the legacy `user_id` column and
   `pciworld_users` credential fields through a reviewed migration.
4. Legacy `pciworld_users` credential retirement (dry-run/staging/rollback rehearsal per §11.2) —
   the map and dual-login exist; the cutover has not been executed.
5. Browser E2E, accessibility (WCAG 2.2 AA) and visual-regression suites for the new journeys;
   the current evidence is unit/integration tests plus live-boot JS-parse checks.
6. MySQL staging rehearsal of the new migrations under production-shaped data.
7. P2 admin completeness: full pagination on challenge/report/editorial/audit lists, consistent
   destructive-action confirmation states, calendar/rotation concept unification.
8. Notifications outbox (the reminder CONSENT is recorded; no delivery pipeline exists).
9. World-suspension vs global-deactivation product states, central "sign out everywhere",
   canonical email-change flow (PW-US-046/058).
