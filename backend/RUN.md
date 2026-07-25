# PCI Platform — Build, Run & Deploy Guide (RUN.md)

This is the practical, honest guide to building and running everything. It also states plainly **what has
been verified in the development sandbox vs. what still needs a real machine with internet access**, so you
know exactly where the trust boundary is before you rely on it.

---

## 0. What this system is (architecture in one paragraph)

- **Backend** — ASP.NET Core 8 **Minimal API** (not MVC controllers). Endpoints across
  `Endpoints/*.cs`, dual-provider DB: **SQLite for local/CI smoke**, **MySQL/MariaDB for
  production** (`schema.sql` / `schema.mysql.sql` + idempotent `Data/Migrate.cs`). See `MYSQL.md`.
  Validation is inline in each endpoint (RBAC, ownership, timing, type/size, entitlement).
- **Frontend** — public site HTML under `wwwroot/`, plus React SPAs at `/app` (student) and
  `/admin` (admin console), served by the backend (`app.UseStaticFiles()`).
- **Desktop secure exam client** — `PCI.SecureExam/`, a **Windows-only** WPF app (`net8.0-windows`,
  OpenCV + NAudio). `PCI.SecureExam.Core` (pure `net8.0`) holds the shared, testable logic.
- **Payments** — Stripe (checkout + webhook), keyed off environment variables.

---

## 1. Prerequisites

| Tool | Version | For |
|---|---|---|
| .NET SDK | 8.0.x | backend + desktop |
| Internet / NuGet access | — | **required** to restore packages (see §6) |
| Node.js | 18+ | optional: running the JS syntax gate locally |
| Python 3 | 3.10+ | optional: running the logic-test suites |
| Windows 10/11 | — | **required** to build/run the desktop client only |

---

## 2. Backend — build, migrate, run

```bash
cd PCI.Backend

# 1. restore + build (needs NuGet access; pulls Microsoft.Data.Sqlite, BCrypt.Net-Next, Stripe.net)
dotnet build -c Release

# 2. run (creates/migrates ./pci.db on first boot; migrations are idempotent)
#    Minimum env for a local run:
export DATABASE_FILE=./pci.db
export PORT=8080
dotnet run -c Release
```

Then open **http://localhost:8080/** (public site), `/student.html`, `/admin.html`.
Health check: `curl http://localhost:8080/api/health`.

First-run admin (from `Data/Migrate.cs`): `owner@pci.local` / `changeme-owner` —
**change this immediately**, and in production set `ADMIN_OWNER_EMAIL` / `ADMIN_OWNER_PASSWORD`.

### Environment variables

| Var | Needed when | Notes |
|---|---|---|
| `DATABASE_FILE` | SQLite local/dev | use a persistent path locally. If `/data` is writable and provider is **not** MySQL, the app may adopt `/data/pci.db`. Production uses MySQL (`DB_PROVIDER=mysql`) — see `MYSQL.md` |
| `DB_PROVIDER` | production | `mysql` required in Production (`render.yaml`); default `sqlite` for local |
| `PORT` | optional | default 8080 |
| `STRIPE_SECRET_KEY` | payments on | without it, payment endpoints return 503 |
| `STRIPE_WEBHOOK_SECRET` | payments on | **required in prod**; used to verify webhook signatures |
| `APP_BASE_URL` / `SITE_BASE_URL` | prod | must be a public HTTPS URL |
| `ALLOWED_ORIGIN` | prod | explicit origin; **wildcard is rejected in prod** |
| `RESEND_API_KEY` | email on (easiest) | Resend HTTPS API; `MAIL_FROM` sets the verified sender. Takes precedence over SMTP |
| `SMTP_HOST` (+user/pass/port) | email on (classic) | without either provider, emails print to console |
| `STORAGE_ROOT` | prod | evidence/attachment files; use durable storage. Auto-adopts `/data/storage` when a `/data` disk is mounted and this is unset |
| `STORAGE_PROVIDER` | optional | `local` (default) or `s3` (any S3-compatible store; needs `S3_BUCKET`, optional `S3_ENDPOINT` for MinIO/R2, `S3_REGION`, AWS creds via the standard env vars) |
| `ENABLE_LEGACY_ADMIN_TOKEN` | never in prod | legacy `x-admin-token`; the app **errors on boot** if this is on in prod |
| `ALLOW_INSECURE_PRODUCTION` | escape hatch | set `true` to boot despite config errors (**not recommended**) |

> **Config validation on boot:** in `Production` the app logs every config issue and **refuses to start**
> on hard errors (missing webhook secret, wildcard/absent CORS, localhost base URL, `/tmp` database, legacy
> token enabled) unless `ALLOW_INSECURE_PRODUCTION=true`. Owner-only readiness report:
> `GET /api/admin/system-check`.

---

## 3. Frontend

Nothing to build — the HTML/JS is served by the backend from `wwwroot/`. To iterate, edit the files and
refresh. To sanity-check JS before committing:

```bash
# from repo root, checks the app shells parse
node --check <(python3 - <<'PY'
import re;h=open('PCI.Backend/wwwroot/student.html').read()
print('\n;\n'.join(m.group(2) for m in re.finditer(r'<script([^>]*)>(.*?)</script>',h,re.S) if 'src=' not in m.group(1) and 'json' not in m.group(1).lower()))
PY
)
```

The portals have a built-in **demo mode** (no backend needed) for UI review, and switch to live API calls
when served with a real session.

---

## 4. Desktop secure exam client (Windows only)

```powershell
cd PCI.SecureExam

# build (Windows, with NuGet access)
dotnet build -c Release

# run locally against a dev backend
dotnet run --project PCI.SecureExam.App

# produce a distributable, self-contained launcher (this is the .exe you deploy)
dotnet publish PCI.SecureExam.App -c Release -r win-x64 --self-contained `
  -p:PublishSingleFile=true
# → bin/Release/net8.0-windows/win-x64/publish/PCI.SecureExam.App.exe
```

Configure `appsettings.Local.json` (copy from `appsettings.Local.json.example`) — in particular
`AllowedApiHosts`, which **pins** the client to approved PCI domains. The client registers the
`pciexam://` URI scheme; the student portal's "Open in the PCI Secure Exam app" button hands it a
short-lived, single-use **launch code** (not a bearer token), which the client redeems against the
**pinned** API host.

There is **no pre-built `.exe` in this repo** — you must run the `publish` command above on Windows.

---

## 5. Tests

```bash
# Backend logic suites (fast SQLite smoke; MySQL jobs are production parity) — from PCI.Backend/
python3 tests/lifecycle_test.py   # result lifecycle, consents, auto-hold, entitlement, webhook idempotency
python3 tests/release_test.py     # admin release/invalidate/reinstate, configured pass mark, expiry-aware verify
python3 tests/casework_test.py    # appeals, accommodations (+duration effect), attachments, CPD, certificate
python3 tests/settings_test.py    # sp_* enforcement + readiness gate
python3 tests/publication_test.py # immediate publication default; proctoring audit-only; technical blocks
python3 tests/storage_test.py     # storage abstraction: MIME/size/sniff/traversal/retention

# Desktop Core security logic (no packages, no Windows) — from PCI.SecureExam/
#   see PCI.SecureExam.Core.RunnableChecks/README.md  → executes API-host pinning against attack cases

# Desktop xUnit tests (needs NuGet) — from PCI.SecureExam/
dotnet test
```

CI (`.github/workflows/build.yml`, repo root) does the full loop: **restore → build → six logic suites →
JS syntax gate → boot the backend → live smoke suite (52 checks) → adversarial integration suite
(75 assertions) → 500-sweep (every route × anon/student/owner) → system-check probe**, plus the
secureexam solution on `windows-latest` and its tests on Linux. The S3/moto live test also runs,
non-blocking (moto is unreliable on hosted runners; the test is authoritative locally).

---

## 6. Verification status — what has actually been executed

The original sandbox firewalled NuGet, so the backend was only ever *compiled against stubs* and never
booted. **That boundary has now been crossed.** The table below reflects the state after the production
readiness pass (2026-07-06), run on a real Linux machine with real NuGet. See
`PCI_Final_Production_Changelog.md` for the full account, including every runtime bug found.

| Aspect | Status | How |
|---|---|---|
| Backend `dotnet restore`/`build -c Release` (real NuGet) | ✅ **executed** | Microsoft.Data.Sqlite 8.0.7, BCrypt.Net-Next 4.0.3, Stripe.net 45.14.0 — 0 errors/0 warnings |
| Backend **boots** as the real app | ✅ **executed** | fresh boot + reboot; idempotent migrations verified (identical schema fingerprint, no duplicate seeds) |
| Backend serves **real HTTP requests** | ✅ **executed** | live smoke suite 52/52; admin GET sweep 0×500 |
| Backend **logic** (scoring, lifecycle, RBAC, storage, settings) | ✅ verified | 61 assertions across 6 SQLite suites |
| **Adversarial end-to-end** (payments, exam, attacks, RBAC, storage, headers) | ✅ **executed** | `tests/integration_test.py` — 75 assertions, live HTTP (incl. 9 regressions from the adversarial self-review) |
| **500-sweep** (every route × anon/student/owner) | ✅ **executed** | `tests/sweep_500_test.py` — 456 calls, 0 server errors; found the BCrypt login-crash bug |
| **S3 object-storage provider** | ✅ **executed locally** | `tests/storage_s3_test.py` vs a live moto S3 server — 9/9 (upload→`s3:` ref, authed fetch streams from S3, retention purge, no-bucket fallback). Re-run against a real bucket before production use |
| **Stripe** webhook settlement + replay idempotency | ✅ **executed** | self-signed events (Stripe.net pins api_version 2024-06-20) — settle-once + replay-noop proven. *Live Stripe account not exercised* (no real keys); the checkout-session **create** call needs a real `sk_test_`/`sk_live_` key |
| Security headers / CORS / body cap | ✅ **executed** | CSP+nosniff+frame-ancestors present; ALLOWED_ORIGIN honoured; >6 MB body rejected |
| Production config validation | ✅ **executed** | refuses to boot (exit 78) on unsafe config; boots clean with correct vars; `system-check` `ok:true` |
| Desktop **Core** + **xUnit tests** | ✅ **executed** | 21/21 tests (incl. held-result screen); Core RunnableChecks 15/15 host-pinning attacks |
| Desktop **App** (WPF/OpenCV/NAudio) builds/runs; `.exe` published | ⚠️ **Windows-only — not run here** | compiles on the `windows-latest` CI job; publishing the `.exe` and the live `pciexam://` launch flow must be done on Windows (see §4 / §8) |
| Lighthouse (local, against the live app) | ✅ **executed** | 5 key pages: performance 93–100, accessibility 91–96, best-practices 100, SEO 100. Re-run on the deployed URL (network conditions differ) |
| Load testing | ⚠️ deferred | needs a real deployment |

**Honest bottom line:** the backend, website, and desktop Core are **executed and adversarially tested** —
not merely compile-clean. The single remaining gap that cannot be closed on Linux is running/publishing the
**Windows WPF `.exe`** and its live `pciexam://` launch; the security-critical parts of that flow (API-host
pinning, launch-code redemption/expiry/reuse/wrong-user) are already proven via the Core checks and the
backend integration suite, so the Windows step is build-and-QA, not new logic.

---

## 7. Known limitations (not hidden)

- **Desktop WPF `.exe`** must be built, published and QA'd on **Windows** — see §8. The security logic is
  tested cross-platform; the GUI/kiosk/camera path needs a real Windows machine.
- **Object storage (S3)** is now **wired** (`STORAGE_PROVIDER=s3` + `S3_BUCKET`; optional `S3_ENDPOINT`
  for MinIO/R2) and live-tested against a moto mock (9/9). It has **not** been exercised against a real
  AWS/MinIO deployment — do that before relying on it. Missing `S3_BUCKET` falls back to local with a
  boot warning; `local:`/`s3:` references coexist, so migration is safe.
- **TOTP 2FA** is deliberately "coming soon" — `/api/me/2fa` never enables a factor and never claims
  protection (no toggle without a login challenge). Full TOTP is future work.
- **Live Stripe account** is not exercised here (no real keys). The webhook path is proven with correctly
  HMAC-signed events; before go-live, run one real test-mode checkout end-to-end with the Stripe CLI.
- **Separate live/practice question banks** use an `is_practice` flag (safe) rather than a fully separate
  versioned bank model — richer model is future work.

---

## 8. Deploy-readiness

### 8.1 Windows desktop client (must be done on Windows)

```powershell
cd secureexam
./build.ps1 -Publish
# → PCI.SecureExam.App/bin/Release/net8.0-windows/win-x64/publish/PCISecureExam.exe
```

Then, on a clean Windows machine (no SDK):
1. Copy `appsettings.Local.json.example` → `appsettings.Local.json`; set `AllowedApiHosts` to your pinned
   API domain(s) and `ApiBaseUrl` to the production exam host.
2. Run `PCISecureExam.exe --selftest` (self-tests the machine and exits 0 when ready).
3. Launch it once so it registers the `pciexam://` scheme.
4. From the student portal, book → readiness → "Open in Secure Exam app". Confirm it redeems the
   single-use launch code against the **pinned** host, sits the exam, submits, and shows the result.
5. **Attack it live:** `pciexam://start?...&api=https://evil.example` must be ignored; a reused or expired
   launch code must be rejected; a code for another user must be rejected. (These are already covered by
   `PCI.SecureExam.Core.RunnableChecks` and the backend integration suite; re-confirm on the real client.)
6. A **held** submission must show the technical-hold screen with **no** score/pass-fail — verified by
   `SubmittedViewTests`; confirm visually.

> The assembly name is `PCISecureExam` (per the csproj), so the published binary is **`PCISecureExam.exe`**.

### 8.2 Reverse proxy (TLS + HSTS)

Terminate TLS at the proxy (nginx/Caddy/ALB) and forward to the app on `PORT`. **Forward
`X-Forwarded-Proto: https`** — the app emits `Strict-Transport-Security` only when it sees that header
(never over plain http). Also set HSTS at the proxy itself as defence in depth:

```
add_header Strict-Transport-Security "max-age=31536000; includeSubDomains" always;
proxy_set_header X-Forwarded-Proto $scheme;
proxy_set_header X-Forwarded-For  $proxy_add_x_forwarded_for;
```

### 8.3 Persistent volumes + backup

- Production/staging uses managed **MySQL** (`DB_PROVIDER=mysql`) plus a persistent
  `STORAGE_ROOT` for evidence/attachment bytes. `DATABASE_FILE` is local-development only.
- **Backup**: use `tools/mysql_backup.sh` (or your managed provider's snapshot/PITR) and back up
  `STORAGE_ROOT` alongside it. Keep database and object bytes in the same restore set.

### 8.4 Scheduled retention

The app runs a daily in-process `RetentionService` that purges artefacts older than
`evidence_retention_days`. No cron needed. The manual owner endpoint `POST /api/admin/storage/purge`
remains for on-demand runs.

### 8.5 Production env checklist

Set: `ASPNETCORE_ENVIRONMENT=Production`, `DB_PROVIDER=mysql`, `MYSQL_*` (`MYSQL_SSL=required`),
`STORAGE_ROOT` (persistent),
`APP_BASE_URL` (public https), `ALLOWED_ORIGIN` (exact origin, no wildcard), `STRIPE_SECRET_KEY` +
`STRIPE_WEBHOOK_SECRET`, `CREDENTIAL_ENCRYPTION_KEY`, `ADMIN_OWNER_PASSWORD`, `SMTP_HOST` (+creds). Do **not** set
`ENABLE_LEGACY_ADMIN_TOKEN` or `ALLOW_INSECURE_PRODUCTION`. Boot, then `GET /api/admin/system-check`
(owner) must report `ok:true`. `owner_password_changed` stays false until the owner completes the forced
first-login password change — do that before go-live.
