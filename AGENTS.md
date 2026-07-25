# AGENTS.md

## Cursor Cloud specific instructions

This repo is the **PCI Platform** (Project Controls Institute). The runnable-on-Linux scope is the
web platform: an **ASP.NET Core 8 backend** (`backend/`) that serves the JSON API + website + portals,
and a **React/Vite frontend** (`frontend/`) for the student and admin SPAs. `secureexam/` is a
**Windows-only** WPF desktop client and does **not** build/run on Linux (only its `PCI.SecureExam.Core`
+ tests are cross-platform) — skip it here. Authoritative build/run reference: `backend/RUN.md`,
`frontend/README.md`, and the CI matrix in `.github/workflows/build.yml`.

### Environment (already provisioned by the update script)
- .NET SDK 8 is installed at `~/.dotnet`. It is on `PATH` for interactive shells via `~/.bashrc`
  (`DOTNET_ROOT=$HOME/.dotnet`). In a non-interactive shell, invoke `~/.dotnet/dotnet` directly.
- Node 22 / npm and Python 3.12 come with the base image. Frontend deps are installed by the update script.

### Running the services (dev mode)
- Backend (serves everything on `:8080`): from `backend/`, `dotnet run` (or run the built dll
  `dotnet bin/Release/net8.0/PCI.Backend.dll` after `dotnet build -c Release`). Health: `GET /api/health`.
  Uses embedded **SQLite** by default (`DATABASE_FILE=./pci.db`, auto-created) — no separate DB service.
- Frontend SPA (dev, hot-reload): from `frontend/`, `npm run dev` → Vite on `:5173`, base path `/app/`,
  and it **proxies `/api` to `:8080`**, so the backend must be running too. Open
  `http://localhost:5173/app/`. `npm run dev:admin` serves the admin SPA variant.
- The backend only serves the React SPAs at `/app/` and `/admin/` if they are pre-built into
  `backend/wwwroot/app` and `.../admin` (the Docker build does this; both dirs are gitignored). For dev,
  prefer the Vite dev server on `:5173` instead of building into `wwwroot`.

### Non-obvious gotchas
- The backend boots with `Hosting environment: Production` by default (no `ASPNETCORE_ENVIRONMENT` set).
  In `Production` the config validator **refuses to boot on unsafe config** (missing Stripe webhook
  secret, wildcard/localhost CORS/base URL, `/tmp` DB, legacy admin token) unless
  `ALLOW_INSECURE_PRODUCTION=true`. The default SQLite/localhost dev config boots fine; if you tighten
  prod-like env vars and boot fails with exit 78, that's the validator — relax the config or set
  `ASPNETCORE_ENVIRONMENT=Development`.
- Auth routes are split: **admin/owner** login is `POST /api/admin/auth/login`; **student** login is
  `POST /api/login`; student signup is `POST /api/register`. First-run owner is
  `owner@pci.local` / `changeme-owner` (no forced change — update the password in Settings → Security).
- Optional demo data: set `SEED_DEMO_EXAM=true` (seeds the demo question bank) and
  `DEMO_STUDENT_PASSWORD=...` (creates `student@pci.local`). This does **not** grant a ready-to-sit exam
  entitlement — sitting an exam still requires payment/eligibility/scheduling (payments need Stripe keys,
  else checkout returns 503). For an end-to-end "new user" smoke, register a fresh candidate via
  `/app/register`.
- Payments/email/S3 are optional and no-op without keys (checkout → 503; email → console/Admin email log;
  storage → local disk). The app is fully usable without them.

### Lint / test / build / run commands
- Backend build (also acts as the compile "lint"): from `backend/`, `dotnet build -c Release`.
- Backend logic tests (real SQLite, no server): from `backend/`, `python3 tests/<suite>_test.py`
  (e.g. `lifecycle`, `release`, `casework`, `settings`, `publication`, `storage`).
- Backend live smoke suite (requires the backend running on `:8080`): from `backend/`, `./smoke-test.sh`.
- Frontend lint/typecheck: from `frontend/`, `npm run typecheck`. Production build: `npm run build`
  (typechecks + builds student `dist/` and admin `dist-admin/`).
