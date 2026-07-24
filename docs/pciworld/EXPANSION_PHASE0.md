# PCI World Expansion — Phase 0: Audit, Architecture & Phased Plan

_Baseline: `main` @ `4224641`, 2026-07-24. Scope: the Scale/Daily/Passport/Content/SEO expansion
specification. Companion: EXPANSION_GOVERNANCE.md (content governance), the original
docs/pciworld/* set (ARCHITECTURE, THREAT_MODEL, DATA_AND_CONTENT_MODEL, PLAN, DEPLOY_RENDER,
DELIVERY_REPORT)._

## 1. Current architecture map (verified)

One ASP.NET Core 8 app hosts three separated surfaces: the Institute website/portals, the
Simulation Lab, and PCI World (`/world*` public + `/world-admin*` separate realm). PCI World
modules: `Data/WorldSchema.cs` (+`WorldContentPack.cs` seeder), `Core/WorldContent|WorldScore|
WorldLifecycle|WorldPages|WorldOnly`, `Endpoints/World|WorldAccount|WorldAdmin`. Data lives in
`pciworld_*` tables via the platform `Db` layer (SQLite dialect → MySQL 8 translation). Deploy
modes: combined platform, or world-only (`PCIWORLD_ONLY`, `PCIWorld/Dockerfile`). Background-job
infrastructure exists platform-side (worker leasing, outbox/scheduled publisher patterns) and is
not yet used by PCI World.

## 2. Feature maturity (verified, with test evidence)

| Capability | State | Evidence |
|---|---|---|
| Challenge bank | 30 validated challenges, immutable versions, validator w/ leakage+type rules | WorldTests 27/27 |
| Daily feature | computed day-of-year rotation + calendar override — **not** a scheduled, recorded, cycle-aware engine | audit §5 |
| Attempts | anonymous-first, version-pinned, idempotent submit, deterministic scoring incl. bool/set + lenient numerics | unit+E2E |
| Accounts/Passport | separate realm, claiming, per-item consent, verified-email + name gates, rotating tokens, export/delete | Phase 1b tests |
| Sharing | verified-result links + invitations (opaque, revocable); OG tags; **no generated share images** | E2E |
| Admin | separate app: lifecycle+maker-checker, calendar, reports queue, audit, users | E2E + unit |
| Reports/corrections | public form → admin resolve-with-note | E2E |
| Design | PCI brand system, axe WCAG AA scan green on home | Playwright 7/7 |
| Blog/News | **absent** for PCI World (institute blog exists separately and must not be duplicated) | — |
| SEO | titles/meta/OG only; gaps in §5 audit | agent audit |
| i18n/AR | **absent** on world surfaces; platform i18n machinery exists | agent audit |
| Analytics | pciworld_events (privacy-lean); no dashboards | code |

## 3. Reusable platform services (audit-confirmed)

Worker leasing + scheduled-job patterns (`WorkerLease`, `ScheduledPublisher`, dispatchers);
Mailer; i18n content system; sitemap/robots/SEO tag machinery; HtmlSanitize; Analytics;
Storage/media; Security/LoginGuard; the Db provider layer; the Playwright + axe harness; the
Simulation Lab's validator/variant discipline as the content-quality pattern.

## 4. CRITICAL DATA RISK (exit-gate blocker — must be resolved in Phase 1)

The live Render preview runs **ephemeral SQLite with the production validator waived**
(`PCIWORLD_ONLY` preview posture): every redeploy/restart discards learner attempts, accounts
and Passport evidence. Acceptable for a preview; **incompatible with this expansion's invariant
that learner history is never lost.** Phase 1 therefore starts with persistence: attach the
Render disk (immediate) and stand up MySQL 8 (managed, external) with the existing migration
tooling, ending the `ALLOW_INSECURE_PRODUCTION` waiver on the public instance. No editorial or
rotation data of value is created before this gate closes.

## 5. Audit findings — daily rotation & 10k-scale readiness

**The "daily" feature today is a stateless read-time computation, not an engine.**
`WorldLifecycle.Today()` (`Core/WorldLifecycle.cs:138-150`) checks `pciworld_calendar` for a
manual override, then falls back to `servable[DayOfYear % count]` over
`SELECT * … ORDER BY id ASC`. Verified consequences:

- **Not stable within a day.** The selection is recomputed per request from the live servable
  list; publishing, retiring or restoring any challenge shifts every ordinal, so the featured
  challenge can change mid-day for all visitors. The existing test only asserts two calls
  against an unchanged set agree.
- **Nothing is recorded.** Computed days leave zero trace ("what was featured on 2026-03-04?"
  is unanswerable). An override pointing at a challenge that later retires silently falls
  through to the modulo path — no alert, no record.
- **Year boundary is discontinuous and the reach is capped.** `DayOfYear` resets to 1 each
  1 Jan (arbitrary jump; leap-year day 366 → day 1 can repeat a challenge). With more than
  ~366 servable challenges most of the bank is *never featured* — at 10,000, ~96% is dead
  stock. No cycle counter, no shuffle, no no-repeat rule, no exhaustion restart.
- **No scheduler exists for PCI World.** `Program.cs` hosts nine platform background services
  (RetentionService, OutboxDispatcher, ScheduledPublisher, …) — none for rotation. The
  workspace's `changes_at_utc: "00:00"` is a hard-coded string, not a real boundary. Timezone
  is hard-coded UTC with no setting. Attempts are code-keyed, so no grace linkage to a day.
- **Missing states:** no `suspended` state; open content reports never affect servability (a
  challenge with 50 open calculation reports still features); no localization-completeness
  concept anywhere in the world realm.

**10k-scale defects (all verified at specific sites):**

1. `Today()` materialises **every** servable row *including full `config_json` blobs* per
   request, through the single-connection global-lock Db layer — the worst hot path.
2. Admin challenge list (`WorldAdmin.cs:105-118`): no LIMIT/OFFSET; would render a 10,000-row
   table by string concatenation. The most acute admin break.
3. Public archive: `LIMIT 200` with no paging/total — at scale 9,800 challenges become
   unreachable and unlinkable (also an SEO dead end); filter columns unindexed.
4. Missing indexes: `(retired, current_version)` on challenges (every servability query full-
   scans); `passport_token_sha` on users (a public-URL lookup key!); attempts
   `(session_id, challenge_id, version, status)` resume path; `pciworld_audit` (none at all);
   `pciworld_events` `created_at` (highest-volume table, unbounded, no retention).
5. Boot seeding (`WorldContentPack.Seed`) does per-row SELECT+writes, unbatched, no
   transaction, on every start — fine at 30, a boot-time failure at 10,000.
6. Hard caps without pagination: calendar 60, audit 200, reports 200, passport 200.

**Reusable job machinery (confirmed, to be adopted not rebuilt):** `WorkerLease`
(`TryClaim` conditional-UPDATE election, `RecoverExpired`), the `BackgroundService` +
`PeriodicTimer` + static `RunDue(db)` pattern (`ScheduledPublisher`, `OutboxDispatcher`),
idempotency via unique key + `INSERT OR IGNORE` + `ExecuteWithChanges==0`, and the
`scheduled_at<=now` sweep shape which naturally yields catch-up after downtime.

The full required-vs-existing delta (22 rows) is preserved in the audit record; §9 and §12
encode its remediation. Key additions beyond §9's entity list surfaced by this audit:
a **suspended/flagged eligibility predicate** (auto-exclude on open-report threshold), a
**rotation-order table** materialised per cycle (never computed over live `SELECT *`),
`period_id` on attempts for the grace policy, and **substitution supersedes — never
deletes — a period row** (today's same-day calendar POST does DELETE+INSERT, not atomic,
and erases what was displaced).

## 6. Audit findings — SEO baseline

All world HTML flows through one template (`WorldPages.Layout`), which emits title, meta
description, optional `noindex`, a **relative** canonical, favicon and four OG tags
(`site_name/title/description/type`) — and nothing else. Verified inventory across all 11
world page types:

- **Confirmed absent everywhere:** `og:url`, `og:image`, `og:locale`, all `twitter:*` tags,
  any JSON-LD, any `hreflang`, breadcrumbs (visible or structured), and the Search-Console/
  Bing verification metas (`SeoTags.Inject` runs only on the static-page pipeline).
- **Zero `/world*` URLs in any sitemap.** `Sitemap.Xml`, `Sitemap.Index`, `LlmsTxt` and
  IndexNow all read only `pages`/`blog_posts` and are hard-bound to the Institute canonical
  host.
- **Token pages mis-canonicalized and indexable.** `/world/r/{token}` and `/world/p/{token}`
  carry no `noindex` yet canonicalize to `/world` (wrong page). Invite pages are correctly
  `noindex`.
- **World-only host is crawl-broken by construction:** `robots.txt` serves the Institute's
  file verbatim (disallows paths that don't exist there, does *not* disallow `/world-admin`,
  and advertises four sitemaps on a different domain); `/sitemap.xml` 302s to `/world`; `/`
  and *every unknown path* 302 (temporary, not 301/404) to `/world` — a blanket soft-404
  pattern; ownership verification metas can never be delivered.
- **`/world-admin` is not in the robots disallow list or `Redirects.IsPrivatePath`** — the
  page-level `noindex` meta is the only defence.
- Relative canonicals + routes mounted on every host mean `pciworld.org/world` and
  `projectcontrolsinstitute.org/world` self-canonicalize into duplicates.

**Reusability verdict:** the platform's sitemap/robots machinery has the right *shape*
(cached, version-keyed builders emitting absolute URLs) but every generator is table- and
host-bound. The world SEO layer needs: a `/world-sitemap.xml` built from servable
challenges (+ core pages, later blog/news), registration in the sitemap index and robots,
a world-aware base-URL resolver, allowlist entries in `WorldOnly.Allowed`, absolute
canonicals, per-page OG/Twitter images (ties into the §9 share-asset generator), JSON-LD
(`Organization`/`WebSite` on home, `LearningResource` on challenges, `BreadcrumbList`),
`noindex` on token pages, 301 for `/`→`/world` plus real 404s on the world-only host, and
`/world-admin` in both robots and the private-path list. These land as the §10 Phase 7 SEO
work-package, except the token-page `noindex`, robots hardening and 404 fix, which are
small, risk-reducing and scheduled with Phase 1.

## 7. Audit findings — security & privacy posture

Fifteen ranked findings (full register with file:line evidence preserved in the audit
record). What came back clean: CSP-compliance of world pages (though `'unsafe-inline'`
means CSP provides no real XSS containment), no answer/email leakage on public renders
(everything goes through the escaper; revoked links 404), no CSRF exposure (all auth is
header-borne, no cookies), and no IDOR (ownership enforced in SQL everywhere; admin
endpoints all gated; explicit field mapping — no mass assignment).

**High (scheduled into Phase 1 hardening):**

- **H1 — Host-header injection into reset/verify emails.** Links are built from the
  untrusted `Host` header and no host filtering is configured; a forged `Host` on
  `/forgot` mails the victim a valid reset token pointing at the attacker. Fix: build from
  `APP_BASE_URL` + enable host filtering.
- **H2 — World-admin realm: default owner credential can boot, no MFA, login not in the
  platform rate-limit path list** (only per-account lockout brakes it). Fix: refuse boot on
  the default password in production postures, throttle `/api/world-admin/auth/*`, TOTP for
  owner/publisher.
- **H3 — `PCIWORLD_ONLY` waives too much.** The preview posture downgrades *every*
  production blocker — including `ALLOWED_ORIGIN` (CORS becomes `*`), the credential
  encryption key and `APP_BASE_URL` — and the validator only runs at all when the
  environment is marked production. Fix: keep origin/key/base-URL as hard blockers even in
  world-only mode; waive only payments/exam/S3 concerns. (Compounds §4: the same posture
  also permits ephemeral SQLite.)
- **H4 — All world "per-IP" throttles key on the raw socket address**, which behind
  Render's proxy is one shared IP: no per-attacker limiting, and one script can exhaust the
  shared counters to deny session/report/register/login/forgot to everyone. Fix: reuse the
  platform's trusted `ClientIp` (last XFF hop) helper.

**Medium (Phase 1–2):** session-rotation resets session-keyed throttles (cap sessions per
IP/day); several write endpoints have no throttle at all (add a default write throttle in
the pipeline); account-enumeration oracles (`duplicate_email` 409, distinguishable
`account_locked`) plus a 10-fail lockout usable as targeted DoS on a known admin; invite
tokens are documented revocable but no code path revokes them and they survive account
deletion; deletion is *unlinking not de-identification* (attempt `answers_json` +
`session_id`, report free-text, and event linkage survive; no retention job covers
`pciworld_*`; export misses those and is unreachable from the UI as coded); admin password
change does not revoke existing admin sessions and anonymous sessions never expire;
scripted challenge completion is unimpeded while public pages say "Verified"; maker-checker
can be bypassed for NULL-author (seeded) content and `PUT` edits never re-stamp authorship;
world-only short-circuit responses ship without the security headers (middleware ordering).

**Low:** public result/Passport pages are indexable with the display name in OG tags
(revocation cannot unindex — overlaps SEO gap §6); throttle state is in-memory
per-process, so limits multiply per instance and reset on deploy.

**Expansion-specific consequences:** the editorial platform (§9) inherits H2/H3/H4 unless
fixed first — so the Phase 1 exit gate includes the four Highs plus middleware ordering and
admin-session revocation. The report-queue spam and bot-completion findings shape the
Gate-A thresholds in EXPANSION_GOVERNANCE §3. The privacy findings define concrete work:
null `session_id`+`answers_json` on delete, a world retention job, invite revocation, and
an export path that actually authenticates.

## 8. Audit findings — accessibility & localization baseline

**What is solid:** complete landmark structure with labelled nav on public pages; clean
heading order on public pages; real `fieldset`/`legend` + per-radio labels on generated
asks and decisions; a well-labelled `role="img"` hero chart; broad `focus-visible` styling;
live regions and alerts wired on most async flows; `scope="col"` on the main tables; a
reduced-motion media block; no keyboard traps.

**Ranked accessibility gaps (12; file:line evidence in the audit record):**

1. Skip link has no `:focus` reveal rule — permanently invisible (2.4.7 fail); `#main`
   lacks `tabindex="-1"`.
2. Focus is destroyed on every state transition (start/submit/auth toggles hide the
   container that holds the focused button — focus drops to `<body>`).
3. `.dim .kicker` stat labels are 4.42:1 on white (AA fail) — rendered only on
   result/passport/account, precisely the pages the axe E2E does **not** scan (it covers
   `/world` home only).
4. Focus ring is 2.76:1 against the noir header/footer (1.4.11 fail).
5. Form-field borders (`--line` on white) are 1.23:1 — field boundaries effectively
   invisible (1.4.11 fail).
6. `<html lang="en">` hard-coded, no `dir` attribute anywhere.
7. **≈350–380 user-facing English strings compiled into C#/JS literals; zero
   externalized.**
8. Admin shell: broken ARIA-tabs pattern (no `tabpanel`/`aria-controls`/arrow keys), no
   `h1`, no skip link.
9. `#result` is a whole-page live region (announced as one utterance); submit failures
   routed to a polite region instead of an alert; account deletion collects a password via
   native `prompt()` — unmasked.
10. English morphology inlined into interpolations (`challenge{…"s"}`,
    `industr{y|ies}`, `error(s)`) — untranslatable into Arabic's plural system.
11. Table semantics incomplete off the happy path (missing captions/scope on
    account/admin tables; `.tbl-wrap` defined but never emitted).
12. Smooth-scroll not gated on reduced-motion; EV vs AC chart series differ by colour
    alone; native `alert()`/`prompt()` in admin.

**Localization readiness — the hard facts:** the backend builds with
`InvariantGlobalization=true` (culture-aware formatting is unavailable process-wide);
dates render as raw SQLite strings; the platform i18n system (`content_i18n`,
`AdminI18n`, `Translator` with ar coverage, `IsRtl` html rewriting) is real and proven on
the institute site (AR regression-tested) **but none of its injection paths touch world
pages** — world HTML never enters the static-page pipeline where `I18nContent.Render`
runs. Viable minimal path confirmed: run world HTML through a new `"world"` i18n scope +
`Render`, with a separate JSON string-bundle for the ~50 JS-built strings (the page
scanner skips `<script>` subtrees). CSS is only accidentally RTL-ready; ~10 physical
properties would break under `dir=rtl`, and neither Archivo nor Inter has Arabic
coverage — an Arabic-capable font pairing is a design decision for the AR phase.

**Scheduling:** ranked items 1–5 and 9 are small CSS/markup/JS fixes — they land with the
Phase 2 premium-journey polish, and the axe E2E expands to scan workspace, result,
archive, account and admin (closing the blind spot that let #3 survive). Items 6–8 and
10–12 plus string extraction, plural-safe message keys, RTL stylesheet work and the font
decision form the Phase 7 localization package, exactly as the §10 backlog orders it.

## 9. Target architecture (delta, not rebuild)

- **Rotation engine** (new `Core/WorldRotation.cs` + tables `pciworld_rotation`,
  `pciworld_rotation_items`, `pciworld_daily_periods`, `pciworld_rotation_runs`,
  `pciworld_rotation_overrides`): an idempotent, advisory-locked daily job on the platform's
  worker-lease pattern; immutable period records; cycle counter + deterministic per-cycle
  reshuffle (seeded PRNG over rotation items); no-immediate-repeat rule; exclusion filters;
  catch-up after downtime; grace policy = attempts pinned to their period's version (already
  structural) with the period row recording the boundary. Admin rotation console in the world
  admin. The current `Today()` computation remains only as the display fallback until cutover,
  then is retired.
- **Editorial platform** (new tables `pciworld_articles` (kind=blog|news), `_article_versions`,
  `_sources`, `_article_sources`, `_entities`, `_entity_mentions`, `_reviews`,
  `_institute_links`, `_media` + rights): one CMS in the world admin serving both blog and
  newsroom with the governance workflow states; server-rendered public pages `/world/blog/…`,
  `/world/news/…` with the §8 SEO contract; corrections appended, never silent.
- **Share assets**: server-generated OG/square/story images (SVG→PNG via a self-contained
  renderer; no external calls), cached under `pciworld_share_assets`, safe-text budgets, no
  answers/PII.
- **Passport premium**: verification page + one-page PDF (reuse `SimplePdf`), QR (self-drawn
  SVG), field-level disclosure (extends the shipped consent model), recruiter view, expiring
  tokens (adds `expires_at` to the token contract).
- **Search**: SQL-backed unified search (title/summary/body indexes + trigram-lite tokenting)
  across challenges/blog/news with type labels and filters; Arabic analyzer deferred to the
  localization phase.
- **SEO layer**: absolute canonicals, per-page OG image, world sitemap index
  (challenges/blog/news/core) + news sitemap, robots correctness on world-only hosts,
  BreadcrumbList/BlogPosting/NewsArticle/Organization/WebSite JSON-LD, hreflang en/ar when AR
  ships, internal-link graph + orphan report as admin tools.
- **i18n**: world strings move to the platform i18n system; `lang`/`dir` become per-request;
  AR content fields added to article/challenge localization tables when the AR phase opens.

## 10. Phased backlog (mapped to the specification, with dependencies)

| Phase | Content | Depends on | Exit gate |
|---|---|---|---|
| 0 (this) | Audit, governance, architecture, backlog, acceptance matrix | — | plan approved; critical data risk NAMED with owner/fix in P1 |
| 1 | **Persistence first** (disk+MySQL, end preview waiver on the public host), then rotation engine + admin console + migration of the 30-challenge bank into rotation; **plus the §7 High hardening pack** (H1 host-header links, H2 admin-login throttle + no-default-credential boot, H3 narrowed waiver, H4 XFF throttle keys, middleware ordering, admin-session revocation on password change) and the §6 quick risk fixes (token-page `noindex`, `/world-admin` robots/private-path, real 404s + 301 on world-only hosts) | 0 | §12 matrix green incl. cycle wrap, concurrency, catch-up, DST, override; zero learner-data loss proven across boundary; the four High findings closed with tests |
| 2 | Premium daily-challenge journey polish (incl. §8 a11y fixes 1–5, 9 and axe coverage of all world pages), Passport premium (verification page, PDF, QR, field disclosure, recruiter view), share-asset generator + referral analytics | 1 | E2E for guest/learner/returning/verifier/admin; share cards validated on target platforms; axe green on every scanned world surface |
| 3 | 50 flagship challenges (Gate A) via governed agent production + full review | 1 | zero critical content defects; review workflow proven |
| 4 | Blog CMS + the 100-article programme in batches (10/20/20/25/25) | 1 (CMS), 3 (voice) | every published article passes citation/entity/originality/SEO review |
| 5 | Newsroom + source registry + 100 current news items (researched at execution time, ≤90-day preference, primary sources) | 4 CMS | every material claim traceable to a saved source; zero copied wording |
| 6 | Bank scale: Gates B 250 → C 500 → D 1,000 | 3 | per-gate quality/ops thresholds hold |
| 7 | SEO completion, Core Web Vitals, AR/RTL localization QA, security/load hardening | 2–6 | validators green; axe+keyboard+RTL pass; load at 10k-challenge catalogue |
| 8 | Staged launch, dashboards/alerts, rollback drills, editor training docs, ops cadence | all | smoke + rollback drill evidence |

## 11. File-level change plan (Phase 1 scope)

`Data/WorldSchema.cs` (+rotation tables, the §5 missing indexes, session/token expiry),
new `Core/WorldRotation.cs`, `Endpoints/World.cs` (today = period lookup; XFF throttle
keys; archive pagination), `Endpoints/WorldAccount.cs` (links from `APP_BASE_URL`; delete =
de-identify answers+session linkage; authenticated export fetch), `Endpoints/WorldAdmin.cs`
(+rotation console APIs/UI tab; login throttling; session revocation on password change;
paginated challenge list), `Program.cs` (job registration; middleware ordering; narrowed
world-only waiver; host filtering; `/world-admin` in robots/private paths), `Core/WorldPages.cs`
(token-page `noindex`), new `tests/WorldRotationTests.cs` (the full §12 matrix) + security
regression tests, `frontend/e2e/portal-world.spec.ts` (+rotation admin journey),
`PCIWorld/README.md` + `DEPLOY_RENDER.md` (persistence + hardening steps become REQUIRED,
not optional).

## 12. Acceptance matrix (Phase 1, measurable)

1. Boundary run creates exactly one `pciworld_daily_periods` row per day — reruns create zero.
2. Two concurrent runners → one winner (lease), identical outcome, one run record + one skip record.
3. 3-day downtime → catch-up creates the missed periods in order, correct cycle math.
4. Bank exhaustion → cycle+1, reshuffled order (when enabled) with first item ≠ previous last.
5. Retired/suspended/flagged items never selected; selection reasons recorded.
6. Manual override: permissioned, audited, cancellable, never corrupts order.
7. Attempt spanning the boundary completes under grace, pinned to its version; nothing historical changes (row-count + checksum assertions).
8. DST-shifted configured timezone boundary honoured.
9. Rotation console shows current/next/cycle/health and every §4 control, each action audited.
10. All existing suites stay green; catalogue endpoints paginate correctly with 10k synthetic rows (perf smoke); `Today()`/period lookup no longer materialises config blobs.
11. Security: reset/verify links ignore the request Host header; world-admin login throttled per client IP (XFF-aware); boot refuses the default owner password outside dev; world-only responses carry the security headers; admin password change revokes other admin sessions.
12. Privacy: account deletion nulls attempt answers + session linkage; export downloads with authentication; a retention job covers world sessions/events.

## 13. Risks & blockers requiring a business decision

- **Managed MySQL 8 provider + credentials** (Render has no managed MySQL): needed at Phase 1
  persistence. Options doc'd in DEPLOY_RENDER.md. ← decision + credentials required.
- Institute URL mapping list for contextual links (which pages are approved targets).
- Editorial identities: named human authors/reviewers for publication bylines and the two-person
  approval chain (the spec forbids fake authors — real names or transparent "PCI World Editorial"
  attribution must be chosen).
- Company-logo permissions (default: no logos).
- AR translation review capacity before the AR phase exits.
