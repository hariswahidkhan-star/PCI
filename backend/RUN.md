# PCI Platform — Build, Run & Deploy Guide (RUN.md)

This is the practical, honest guide to building and running everything. It also states plainly **what has
been verified in the development sandbox vs. what still needs a real machine with internet access**, so you
know exactly where the trust boundary is before you rely on it.

---

## 0. What this system is (architecture in one paragraph)

- **Backend** — ASP.NET Core 8 **Minimal API** (not MVC controllers). ~157 HTTP endpoints across
  `PCI.Backend/Endpoints/*.cs`, backed by **SQLite** (`schema.sql` + idempotent `Data/Migrate.cs`).
  Validation is inline in each endpoint (~170 guards: RBAC, ownership, timing, type/size, entitlement).
- **Frontend** — static single-file HTML/CSS/vanilla-JS apps in `PCI.Backend/wwwroot/`
  (public site, `student.html`, `admin.html`, `exam-ui.html`, `checkout.html`, `enroll.html`).
  Served by the backend itself (`app.UseStaticFiles()`), so there is no separate web server to run.
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
| `DATABASE_FILE` | always | use a **persistent** path in prod (not `/tmp`) |
| `PORT` | optional | default 8080 |
| `STRIPE_SECRET_KEY` | payments on | without it, payment endpoints return 503 |
| `STRIPE_WEBHOOK_SECRET` | payments on | **required in prod**; used to verify webhook signatures |
| `APP_BASE_URL` / `SITE_BASE_URL` | prod | must be a public HTTPS URL |
| `ALLOWED_ORIGIN` | prod | explicit origin; **wildcard is rejected in prod** |
| `SMTP_HOST` (+user/pass/port) | email on | without it, emails print to console |
| `STORAGE_ROOT` | prod | evidence/attachment files; use durable storage |
| `STORAGE_PROVIDER` | optional | `local` (default). Object storage is a documented seam — see §7 |
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
# Backend logic suites (real SQLite, replicate production SQL) — from PCI.Backend/
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

CI (`.github/workflows/build.yml`) does the full loop: **restore → build → run the six logic suites →
JS syntax gate → boot the backend → live smoke suite → system-check probe.**

---

## 6. ⚠️ Verification status — read this before trusting "production-ready"

Development happened in a **sandbox where NuGet is firewalled** (`api.nuget.org` → 403). That has a concrete
consequence you must understand:

**The backend has never been booted end-to-end in development.** Its three real dependencies
(`Microsoft.Data.Sqlite`, `BCrypt.Net-Next`, `Stripe.net`) could not be downloaded, so verification used a
**compile-harness with hand-written stubs** for those libraries, plus real-SQLite logic tests and JS/SQL
static checks.

| Aspect | Status in sandbox | How |
|---|---|---|
| Backend **compiles** (0 errors/0 warnings) | ✅ verified | real Roslyn compiler, against dependency **stubs** |
| Backend **logic** (scoring, lifecycle, RBAC rules, storage, settings) | ✅ verified | 61 assertions across 6 suites vs. **real SQLite** |
| SQL validity, endpoint wiring, JS syntax | ✅ verified | static analysis / `node --check` |
| Desktop **Core** compiles | ✅ verified | real compiler, no stubs (Core has no packages) |
| Desktop **API-host pinning** security logic | ✅ **executed** | 15/15 attack cases run (see RunnableChecks) |
| Backend **boots** as the real app | ❌ **not done here** | needs NuGet → do it via CI or locally |
| Backend serves **real HTTP requests** | ❌ not done here | " |
| **Stripe** checkout/webhook end-to-end | ❌ not done here | needs real Stripe keys |
| Desktop **App** (WPF/OpenCV/NAudio) builds/runs | ❌ not possible here | **Windows-only** |
| Load / browser / full integration | ❌ not done here | needs a real deployment |

**Honest bottom line:** the code is **code-complete and statically + logically verified**, not
"proven in production." The accurate phrase is *"pending a real build-and-boot."* The CI I wrote performs
that real boot + smoke test — **but it must actually be run on GitHub (or locally where NuGet works)**, and
that green run is the thing that converts this from "should work" to "does work." Two payment-breaking bugs
in earlier iterations compiled cleanly but only surfaced under real execution — so **run §2 and §5 on a
connected machine and treat the first errors as expected**, not as a surprise.

---

## 7. Known limitations (not hidden)

- **Object storage** (`STORAGE_PROVIDER != local`) is a **documented seam**, not a live S3 integration —
  it currently falls back to local with a warning. The reference format and all call sites are already
  provider-agnostic, so wiring `PutObject`/`GetObject` is isolated to `Core/Storage.cs`.
- **TOTP 2FA** is deliberately "coming soon" (the misleading toggle was removed); login is single-factor.
- **Desktop WPF app** cannot be built/verified in a Linux sandbox; only its Core is. The full app needs a
  Windows build + manual QA before exams.
- **Retention purge** is on-demand (`POST /api/admin/storage/purge`); schedule it via cron/hosted service.
- Evidence/attachment bytes on the `local` provider assume a **persistent volume**.
- **Separate live/practice question banks** use an `is_practice` flag (safe) rather than a fully separate
  versioned bank model — that richer model is future work.
