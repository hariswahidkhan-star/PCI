# PCI World Expansion — Phase 1 delivery report

_Baseline: `main` @ `2d8bf03` (Phase 0). Scope: the Phase 1 row of EXPANSION_PHASE0.md §10 —
persistence posture, the daily rotation engine, the High security findings and the scale fixes
those depend on._

## 1. What was built

### Persistence posture (§4 — the critical data risk)

The risk was never a missing feature; it was that an operator could not tell a durable deployment
from one that discards every account on redeploy. That is now impossible to miss:

- Boot prints `[pciworld] EPHEMERAL STORAGE — …DESTROYED on the next deploy or restart` whenever
  the database is a relative SQLite path, and names the fix.
- `PCIWORLD_ALLOW_SQLITE=true` replaces the blanket `ALLOW_INSECURE_PRODUCTION` waiver for world
  deployments. It downgrades only the MySQL requirement, and it **refuses to boot** if
  `DATABASE_FILE` is not an absolute path — the waiver cannot be used to run on ephemeral storage.
- DEPLOY_RENDER.md and PCIWorld/README.md now lead with durability instead of listing it as
  optional hardening.

MySQL 8 itself remains blocked on a business decision (provider + credentials). Everything above
is the bridge that makes the interim honest rather than silent.

### Daily rotation engine (`Core/WorldRotation.cs`, 3 new tables)

Replaces `servable[DayOfYear % count]` — a stateless computation with four defects — with an
append-only **period ledger**:

| Property | Before | Now |
|---|---|---|
| Stability within a day | recomputed per request; any publish/retire moved it | one recorded period per day; nothing moves it |
| History | none — "what ran last Tuesday?" was unanswerable | every day recorded with cycle, position, source and reason |
| Reach | ~366 challenges maximum (96% of a 10k bank unreachable) | the whole bank, every cycle |
| Repeats | year-boundary collisions | full consumption, then a reshuffled cycle with no repeat across the boundary |
| Scheduling | none — no worker existed | lease-guarded boundary job on the platform's worker pattern |
| Timezone | hard-coded UTC, `changes_at_utc: "00:00"` a literal string | operator-set zone (IANA or fixed offset), real boundary instant, DST-aware |

Also delivered: catch-up after downtime (with a loud, recorded truncation beyond 60 days),
eligibility filters (retired / suspended / flagged by open-report threshold), automatic
substitution when featured content is withdrawn mid-day, a run log that records *why nothing
happened*, and an admin **Rotation** console (current/next/cycle/health, run-now, pause/resume,
audited substitution with a required reason, recorded-days ledger, scheduler log).

The invariant that mattered most is tested directly: rotation decides what is *featured* and has
no authority over learner history. Attempts stay pinned to their own version across any number of
boundaries, retirements and substitutions.

### Security — the four High findings closed

| # | Finding | Fix |
|---|---|---|
| H1 | Reset/verify links built from the attacker-supplied `Host` header (account takeover) | `WorldUrl` resolves the origin from configuration → `RENDER_EXTERNAL_URL` → a *known* request host → the canonical constant. The request host is never echoed unless we know we answer on it. |
| H2 | World-admin login unthrottled; published default owner password could boot production | World credential endpoints joined the platform's IP-keyed limiter; a production boot with no `PCIWORLD_OWNER_PASSWORD` mints a random password and prints it once; a default left in place warns on every boot. |
| H3 | `PCIWORLD_ONLY` downgraded *every* production blocker | Only payments/exams/object-storage are waived. `ALLOWED_ORIGIN`, `CREDENTIAL_ENCRYPTION_KEY`, `APP_BASE_URL` and the legacy-admin-token check stay blocking. |
| H4 | "Per-IP" throttles keyed on the socket address — one shared bucket behind Render's proxy | Both world modules key on the trusted last-XFF-hop (`Security.ClientIp`), same as the platform limiter. |

Plus two structural fixes from the same audit: the world host/only middleware moved **below** the
security-headers and CORS middleware (its redirects and 404s previously shipped with no CSP,
nosniff or CORS headers), and an admin password change now revokes that admin's other sessions —
a stolen bearer token no longer outlives the action taken to revoke it.

### Scale and SEO groundwork

- Indexes for every hot path the audit named: challenge servability and facets, the attempt-resume
  filter, the public Passport token lookup (a per-request key that was unindexed), event dates,
  audit/report queues, session expiry.
- The admin challenge list and the public archive are paginated in SQL with real totals; the
  archive pager carries active filters and `rel=prev/next`, so the catalogue stays reachable and
  crawlable at any bank size.
- `Today()` no longer loads every challenge's config blob per request — it is a single indexed
  period lookup.
- Token-addressed pages (`/world/r/…`, `/world/p/…`) are `noindex`: revocation cannot un-index a
  page, so they must not enter an index. Canonicals are absolute. `/world-admin` joined the robots
  disallow list and the private-path `X-Robots-Tag` list. World-only deployments now 301 `/`→
  `/world` and return real 404s instead of soft-404 redirecting every unknown path.

## 2. Acceptance matrix (EXPANSION_PHASE0.md §12)

| # | Criterion | Evidence |
|---|---|---|
| 1 | One period per day; reruns create zero | `Boundary_run_is_idempotent` |
| 2 | Concurrent runners → one winner, recorded skip | `Concurrent_runners_elect_one_winner_and_record_the_skip` |
| 3 | Downtime catch-up in order with correct cycle maths | `Downtime_is_caught_up_in_order`, `Excessive_downtime_truncates_loudly` |
| 4 | Bank exhaustion → cycle+1, reshuffled, first ≠ previous last | `Cycle_wraps_with_a_reshuffle_and_no_immediate_repeat`, `Order_is_deterministic_for_a_cycle` |
| 5 | Retired/suspended/flagged never selected; reasons recorded | `Ineligible_challenges_are_never_selected`, `Ineligible_calendar_entry_falls_back_and_is_recorded` |
| 6 | Override permissioned, audited, never corrupts order | `Substitution_supersedes_without_erasing_and_keeps_the_cycle_position` + E2E rotation console |
| 7 | Attempt spanning the boundary is untouched | `Rotation_never_alters_an_attempt_that_spans_the_boundary` |
| 8 | DST-shifted boundary honoured | `Boundary_honours_the_configured_timezone_across_dst`, `Unresolvable_timezone_falls_back_to_utc` |
| 9 | Console shows current/next/cycle/health, every action audited | E2E `the rotation console shows the recorded day and substitutes without erasing it` |
| 10 | Suites green; catalogue paginates | full runs below |
| 11 | Security regressions | `Mailed_links_never_trust_the_request_host_header`, `World_admin_is_private_to_crawlers…` |
| 12 | Privacy work | **not in this phase** — see §4 |

## 3. Test evidence

- .NET unit/integration: **691 passed, 0 failed** (45 of them PCI World, including 15 new rotation
  tests and 3 new security regressions).
- Python integration suite: **1124/1124 passed**.
- Playwright PCI World suite: **8/8 passed** (7 existing + the new rotation console journey).
- Full Playwright run: 73 passed, 20 failed — **all 20 pre-existing and unrelated**: 18 are
  firefox/webkit/mobile-safari smoke tests for browsers not installed in this container, and the
  2 chromium failures (`public-downloads`, `portal-multicert`) are PDF-download timeouts that
  reproduce identically on a clean `main` checkout with these changes stashed.

## 4. Deliberately NOT done in this phase

Stated plainly so nothing reads as delivered that is not:

- **MySQL 8 migration** — blocked on the provider decision and credentials.
- **The Medium/Low privacy findings** (deletion de-identification, world retention job, invite
  revocation, export-through-the-UI, session expiry purge) — scheduled with Phase 2, where the
  Passport work already touches those code paths.
- **TOTP for world admins** — Phase 2 with the rest of the admin hardening.
- **The a11y fix pack and expanded axe coverage** — Phase 2, per §10.
- **The full SEO layer** (world sitemap, JSON-LD, OG images, breadcrumbs, hreflang) — Phase 7.
  Only the risk-reducing subset landed here.
- No content was added: the bank is still the 30 pilot challenges. Gate A (50 flagship) is Phase 3.

## 5. Open decisions carried forward

1. **Managed MySQL 8 provider + credentials** — the launch gate.
2. Institute URL mapping for contextual links.
3. Named editorial authors/reviewers for the blog/news programme.
4. Company-logo permissions (default: none).
5. Arabic review capacity before the localization phase exits.
