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

## What still needs a human / a real machine (nothing is stubbed to look done)

1. **Publish + QA `PCISecureExam.exe` on Windows** and run the live `pciexam://` launch + attack pass
   (RUN.md §8.1). Security-critical logic already verified cross-platform.
2. **One real Stripe test-mode checkout** end-to-end with the Stripe CLI before go-live (the webhook path is
   proven with signed events; a live account was not exercised).
3. **Lighthouse** on the deployed public URL.
4. **Object storage (S3)** — wire `PutObject`/`GetObject` in `Core/Storage.cs` if you outgrow the local
   provider (env-gated seam already in place).
5. Complete the **owner first-login password change** so `system-check.owner_password_changed` goes green.
