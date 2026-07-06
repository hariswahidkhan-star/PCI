# PCI Platform — Final Production Readiness Changelog

**Date:** 2026-07-06
**Scope:** Take the platform from "code-complete but never booted" to *executed and adversarially tested*.
**Environment:** Linux, .NET 8.0.128, real NuGet (Microsoft.Data.Sqlite 8.0.7, BCrypt.Net-Next 4.0.3,
Stripe.net 45.14.0), Python 3.11, Chromium (Playwright).

The history lesson from the handover held: **compile-clean means nothing; executed-and-attacked is the bar.**
Every phase below was run, not asserted. Ten bugs that had compiled cleanly and survived every "verified"
pass were only caught by real execution.

---

## Runtime bugs found and fixed (all compiled clean; all found by execution)

| # | Where | Symptom | Root cause | Fix |
|---|---|---|---|---|
| 1 | `PCI.Backend.csproj` | Build failed `NETSDK1022` | `wwwroot/**/*` was explicitly `<Content Include>`, but `Microsoft.NET.Sdk.Web` auto-includes it; the stub harness never used the real Web SDK | `Include` → `Update` |
| 2 | `Data/Db.cs` | Build failed `CS0122` | `SqliteConnection.Transaction` is `protected internal` in the real driver (the stub exposed it public) | Track the active transaction in a private `_activeTx` field under the existing lock; preserves the historical webhook-transaction fix |
| 3 | `Data/Migrate.cs` | latent 500 | fallback `CREATE TABLE support_attachments` diverged from `schema.sql` (`data_uri NOT NULL`, no `storage_ref`) | aligned the fallback DDL with `schema.sql` |
| 4 | `tests/casework_test.py` | suite crash | inserted a legacy `data_uri` attachment without `storage_ref` (NOT NULL since the storage migration) | mirror the production INSERT (`storage_ref` + metadata) |
| 5 | `smoke-test.sh` | suite abort / wrong assertion | died under `set -u` on unbound vars; S13 asserted the legacy `x-admin-token` *authenticates* | default the vars; S13 now asserts the token is **dead** with any value (the .NET port removed it) |
| **6** | `Core/Lifecycle.cs` | **`KeyNotFoundException` on booking/dashboard for any paid user** | `BookingBlockers` read `entitlement["payment_id"]`, but callers pass a **payments** row (no such column) | read `payment_status`/deadline off the row directly |
| **7** | `schema.sql` + start/authorize INSERTs | **no exam could ever be submitted** — every submit returned `already_submitted` | `exam_attempts.status` had no default (the `DEFAULT 'in_progress'` was mis-attached to `bank_version`), so new attempts were `status=NULL` | set `status='in_progress'` explicitly in both INSERTs + fixed the schema default |
| **8** | `Endpoints/StudentExam.cs` (submit + heartbeat) | **every submit and heartbeat 500'd** | responses carried dual PascalCase+lowercase keys (`ok`+`Ok`, `result`+`Result`, `messages`+`Messages`); under ASP.NET's camelCase policy both map to one JSON name → `InvalidOperationException`. The duplicates would also have tripped the desktop client's case-insensitive deserializer | collapsed to one key per field + distinctly-named camelCase aliases (`percent`/`credentialId`/`resultStatus`/`serverTime`/`remainingSeconds`) the desktop binds case-insensitively |
| **9** | `Endpoints/ExamClient.cs` | **desktop launch could never authorize** — a code minted 0 s ago read as `code_expired` | `expires_at` (SQLite `datetime()`, space-separated) was lexically compared to `H.IsoNow` (ISO-8601 `T`); space (0x20) < `T` (0x54) makes every same-day expiry compare as past | compare numerically via `H.JsMillis` |
| **10** | `Core/H.cs` | **all desktop heartbeat/submit/evidence/identity → `no_attempt`** | `AttemptForToken` looked up the launch code `WHERE code=?` (plaintext), but codes are stored only as `code_hash` (SHA-256) | hash the code; prefer the redeemed `attempt_id` |

Bugs 6–10 are each **launch-blocking**: without them the paid journey, the exam submit, and the entire
desktop client are non-functional. All were invisible to compilation and to the 61-assertion logic suite
(which inserts rows with the correct shape/status directly, bypassing the endpoint code paths).

---

## Phase-by-phase

### Phase 0 — make it boot
- First real `restore`/`build` (bugs 1–2), first boot, idempotent-migration proof (double boot → identical
  schema fingerprint, no duplicate seeds), all six portals 200, 6 logic suites pass, **live smoke 52/52**
  (first execution ever), admin GET sweep 0×500. CI moved from the two inert `*/.github` sub-dirs to a
  repo-root `.github/workflows/build.yml` (backend job boots the DLL + runs smoke + integration;
  secureexam builds on `windows-latest`, tests on `ubuntu`).

### Phase 1 — adversarial end-to-end
- New `tests/integration_test.py`: boots the real backend with TEST Stripe keys and drives every critical
  path over HTTP with **self-signed** webhook events (Stripe.net pins api_version 2024-06-20, so no Stripe
  CLI needed). Covers: payment settle-once + **replay idempotency**, cert happy/fail, six attack paths
  (late submit, duplicate submit, foreign item ids → `item_set_mismatch`, consumed-entitlement rebook,
  refunded-then-submit → `payment_reversed`, **answer-key leakage grep** over every student-facing payload),
  held→admin release/invalidate/reinstate (held payload leaks no pass/fail), accommodations (+30 → 120 min),
  RBAC per role, rate limits (429 + Retry-After), the full **desktop launch flow**
  (launch-code → authorize → submit; reused/expired/unknown codes rejected), and storage. Found bugs 6–10.
  **66/66 assertions pass.**

### Phase 2 — hardening
- Security headers on every response: CSP (enforced; `CSP_REPORT_ONLY=true` for rollout), `nosniff`,
  `Referrer-Policy`, `X-Frame-Options: DENY` (+ `frame-ancestors 'none'`), `Cross-Origin-Opener-Policy`.
  CSP scoped to the single-file apps' inline scripts/styles and the only external origins the site uses
  (Google Fonts, googletagmanager, plausible, cdnjs). HSTS only under forwarded https.
- CORS honours `ALLOWED_ORIGIN` only (+ `Vary: Origin`). Global Kestrel body cap 6 MB (uploads still enforce
  the 3 MB decoded cap + magic-byte sniff). Daily `RetentionService` hosted service (manual endpoint stays).
- **2FA decision:** kept the honest "coming soon" — `/api/me/2fa` never enables a factor or claims protection.
- `system-check` now reports `security_headers`/`csp_enforced`/`request_body_capped`/`retention_scheduled`.

### Phase 3 — website
- Crawled all **216 pages** (258 internal targets). Fixed **6 dead PDF download links** (no such files) —
  converted "Download ↓" buttons with fabricated sizes to a disabled **"Coming soon"** chip.
- Legal/brand: qualified the bare `(501(c)(3))` suffix in 16 titles/meta to `(pursuing 501(c)(3))`.
  Verified: donations "not tax-deductible", "not currently ISO/IEC 17024 accredited", prices "USD NN",
  British English intact in prose.
- Chromium: 14 key pages — no app-level console errors or broken same-origin assets. Mobile 390px — no
  horizontal overflow on any of the six app shells. Demo mode works with no backend (0 page errors).

### Phase 4 — desktop client
- First-ever WPF compile surfaced several errors fixed for the `windows-latest` CI job: `UseWindowsForms`
  global-using collisions with WPF (`Application`/`Window`/`Brush`/`Button`/…), the WindowsDesktop
  implicit-usings profile dropping `System.IO`/`System.Net.Http`, `OpenCvSharp.Window` vs
  `System.Windows.Window`, and a missing `using` in the reference Server project.
- Built the one **known-unbuilt UI — the held-result screen.** `RenderSubmitted` previously always showed
  `Your score: {Percent}%`, so a held submission displayed `0%` and an implied outcome. The held-vs-released
  decision now lives in a pure `Core.SubmittedView` (unit-tested on Linux): a held result yields no score,
  no pass/fail, no credential. **xUnit 21/21** (was 16); fixed a stale host-pinning test to match the
  domain-pinning contract; `build.ps1 -Publish` added.
- **Windows-only remainder (documented, not run here):** publishing `PCISecureExam.exe` and the live
  `pciexam://` launch/attack pass — see RUN.md §8.1. The security logic is already proven cross-platform.

### Phase 5 — deploy readiness
- Production dry-run: with unsafe config the app **refuses to boot** (exit 78, every issue named); with
  correct vars it boots clean **without** `ALLOW_INSECURE_PRODUCTION`, CORS pinned, CSP enforced, and owner
  `system-check` returns `ok:true` / `issues:[]` (only `owner_password_changed` stays false until the forced
  first-login change). RUN.md updated: real verification table, reverse-proxy/HSTS, persistent volumes,
  backup, scheduled purge, and the Windows publish/QA steps.

---

## Phase 6 — post-phase hardening (500-sweep, S3, adversarial self-review)

After the phase plan, three more passes ran. The first two found nothing serious; the third (a multi-agent
adversarial review of the whole branch diff) found eleven real defects that had survived every green suite.

### Exhaustive 500-sweep (`tests/sweep_500_test.py`)
Calls **every** mapped route (153) under three auth contexts (anon/student/owner) with benign bodies and
fails on any 5xx. Found **bug 11**:

| # | Where | Symptom | Root cause | Fix |
|---|---|---|---|---|
| **11** | `Program.cs` `/api/login`, `/api/admin/auth/login` | **login 500'd** for any row whose stored hash isn't valid bcrypt | `BCrypt.Verify` throws `SaltParseException` on a malformed hash instead of returning false — an auth endpoint must answer 401, never crash | new `Security.VerifyPassword` wraps `Verify` in try/catch → false on any bad stored hash |

### S3 object storage (optional seam → wired)
`Core/Storage.cs` now has a real S3 provider (`STORAGE_PROVIDER=s3` + `S3_BUCKET`, optional `S3_ENDPOINT`
for MinIO/R2, AWS SDK default credential chain). `Put`/`Get`/`PurgeOlderThan` route by the reference prefix
(`local:` vs `s3:`) so mixed references survive a migration; missing `S3_BUCKET` falls back to local with a
warning (never silently drops data). New `tests/storage_s3_test.py` proves it live against a local **moto**
S3 server (**9/9**): upload lands in the bucket, DB holds an `s3:` reference only, the authed fetch streams
bytes back from S3, retention purges aged S3 objects, and the no-bucket fallback warns. Both new suites are
in CI.

### Adversarial branch review — 11 confirmed defects fixed
A four-dimension review (business-rules / security / desktop-contract / test-validity) with per-finding
adversarial verification. The verify agents were cut short by a session limit, so **every finding was
re-verified by hand against the code** before fixing — 11 of 13 were real:

| # | Where | Symptom | Fix |
|---|---|---|---|
| **12** | `Endpoints/StudentExam.cs` `GET /api/me/attempts/{id}` | **held result leaked pass/fail** — this endpoint returned the raw row (percent/result/breakdown) for an `auto_held` attempt, unlike `/api/me` | redact percent/result/score/breakdown when `result_status=='auto_held'` |
| **13** | `Endpoints/StudentExam.cs` heartbeat auto-timeout | a **timed-out pass was silently stranded** — the finalisation set no `result_status`, wrote no snapshot, issued no credential, never consumed the entitlement | publish exactly like a manual submit (release/hold, snapshot, credential on clean pass, consume entitlement); trigger only past the same +1 min hard stop the manual submit honours, so it can't swallow an on-time submit |
| **14** | `Endpoints/StudentExam.cs` heartbeat messages | desktop `HeartbeatResponse` **threw on deserialize whenever a proctor message was pending** — `at` was a SQLite `"YYYY-MM-DD HH:MM:SS"` string, unparseable to the desktop's `DateTimeOffset` | emit `at` as ISO-8601 |
| **15** | `Endpoints/StudentExam.cs` submit breakdown | desktop `DomainBand.Percent` always bound **0** — the band serialized only `pct` | `Band` now also emits `percent` (browser still reads `pct`) |
| **16** | `Core/Lifecycle.cs`, `StudentExam.cs`, `Public.cs`, `Casework.cs`, `Payments.cs` | the **same space-vs-`T` lexical date bug as launch-blocking bug 9**, still live in 8 more compares: entitlements, credential-verify, certificate, membership/recert renewal all read **expired up to ~24 h early** on their final day, and a valid booking slot before the deadline was rejected as `beyond_window` | new `H.IsPast`/`H.After` compare by parsed instant; replaced all 8 sites |
| **17** | `Program.cs` `PATCH /api/admin/settings` | **RBAC deny-by-default violated** — the `: true` fallthrough let ANY admin (even a viewer) write un-prefixed platform keys, incl. the `auto_block_result_on_*` result-holding switches and `evidence_retention_days` (drives deletion) | un-prefixed keys now require the owner-only `settings` permission |
| **18** | `Core/RetentionService.cs` | scheduled purge **fired at boot** and had **no lower bound** on `evidence_retention_days` | wait one interval before the first purge; `days<=0` disables the scheduled purge (manual owner endpoint unchanged) |
| **19** | `Core/Storage.cs` `PurgeOlderThan` | local purge deleted **every** file under `STORAGE_ROOT` by mtime, incl. any non-artefact an operator dropped there | only delete files matching the content-addressed artefact name (`<64-hex-sha>.<ext>`) |
| **20** | `Program.cs` CSP | enforced CSP **broke the admin evidence viewer** — no `frame-src`, so the `blob:` PDF iframe fell back to `default-src 'self'` | add `frame-src 'self' blob:` |
| **21** | `Program.cs` middleware order | the rate-limiter **429 (and CORS 204 / maintenance 503) shipped without any security headers** — the header middleware ran after the rate limiter | moved security-headers + CORS to the outermost position |
| **22** | `Program.cs` HSTS | HSTS **silently dropped behind chained proxies** — it string-equalled the whole `X-Forwarded-Proto`, which can be `"https, http"` | match the first hop only |

Two findings were **verified false** and left as-is: a claimed leak on a `held` payload already covered by
existing redaction, and a "no backfill for NULL-status attempts" that cannot occur (the buggy build never
ran, so no such rows exist anywhere). **9 new regression assertions** lock in fixes 12–22
(`integration_test.py` now **75/75**; the manual + moto S3 purge paths exercise 18–19).

---

## What still needs a human / a real machine (nothing is stubbed to look done)

1. **Publish + QA `PCISecureExam.exe` on Windows** and run the live `pciexam://` launch + attack pass
   (RUN.md §8.1). Security-critical logic already verified cross-platform.
2. **One real Stripe test-mode checkout** end-to-end with the Stripe CLI before go-live (the webhook path is
   proven with signed events; a live account was not exercised).
3. **Lighthouse** on the deployed public URL. (Ran locally against the live app: perf 93–100, a11y 91–96,
   best-practices 100, SEO 100 across five key pages — see RUN.md; still re-run on the deployed URL.)
4. **Object storage (S3)** is now **wired and tested against moto**; run it against your real bucket/creds
   before relying on it in production.
5. Complete the **owner first-login password change** so `system-check.owner_password_changed` goes green.
