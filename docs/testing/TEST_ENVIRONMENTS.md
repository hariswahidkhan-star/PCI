# PCI Platform — Test Environments

_Every environment a PCI test can run in, how it is provisioned, and its boundaries. Complements
`TEST_STRATEGY.md` (layers) and `TEST_DATA_PLAN.md` (data)._

## 1. Environment matrix

| Environment | DB | Purpose | Egress | Notes |
|---|---|---|---|---|
| **CI `backend`** | SQLite (temp file) | Full live-HTTP integration suite | Blocked (mock vendors on loopback) | boots the built `PCI.Backend.dll` |
| **CI `backend-mysql`** | MySQL service | Same suite against real MySQL | Blocked | `TEST_DB_PROVIDER=mysql`; dedicated DB per run |
| **CI `backend-unit`** | temp SQLite per test | xUnit decision-logic layer | none (in-process) | `TestEnv.NewMigratedDb()` |
| **CI `frontend`** | — | Vitest + lint + tsc + SPA build | npm registry only | jsdom; no browser |
| **CI `e2e`** | SQLite (booted backend) | Playwright + axe | localhost only | `globalSetup` stages SPAs into `wwwroot` |
| **CI `static-quality`** | — | dep/secret/lint/dockerfile gates | pinned container images | gitleaks blocking; nuget vuln blocking |
| **CI `secureexam-*`** | — | SecureExam Core/Tests | none | Linux (Core) + Windows (WPF) |
| **Local dev** | MariaDB via socket | Author-side verification | per host | MariaDB started on a scratchpad socket |
| **Render (staging/prod)** | Managed MySQL | Operator smoke / DR rehearsal | full | Operator-executed; see `EXTERNAL_PROVIDER_TEST_PLAN.md` |

## 2. Booting the backend for tests

The Python suites boot the **real** compiled backend (`bin/Release/net8.0/PCI.Backend.dll`) as a
subprocess and poll `/api/health` before asserting — the same mechanism `migration_integrity_test.py`
reuses for its double-boot. Key environment variables:

| Variable | Purpose |
|---|---|
| `DB_PROVIDER` / `TEST_DB_PROVIDER` | `sqlite` (default) or `mysql` |
| `DATABASE_FILE` | temp SQLite path (per run) |
| `MYSQL_HOST/PORT/USER/PASSWORD/DATABASE`, `MYSQL_SSL` | MySQL connection (CI service or local MariaDB) |
| `STORAGE_ROOT` | temp blob-storage root (per run) |
| `PORT` | a free port chosen per run |
| `STRIPE_SECRET_KEY` / `STRIPE_WEBHOOK_SECRET` | test-mode/placeholder keys for signed-webhook simulation |
| `INTEGRATIONS_ALLOW_PRIVATE_EGRESS=true` | opt-in so the loopback mock vendors are reachable past the SSRF guard |
| `ASPNETCORE_ENVIRONMENT=Development` | dev CORS fallback (`*`); production rejects wildcard/empty origin |

Isolation: each run uses its own temp `DATABASE_FILE`/`STORAGE_ROOT` and a free port; MySQL runs use a
dedicated database and strip `MYSQL_CONNECTION_STRING` from child envs so no shared state leaks.

## 3. Local MySQL for author verification

A local MariaDB (`/usr/sbin/mariadbd`) is started on a socket under the scratchpad with a `pci` user
(`pcipass`) granted on the `pci` database. Run the MySQL leg of the integration suite with:

```
TEST_DB_PROVIDER=mysql MYSQL_HOST=127.0.0.1 MYSQL_USER=pci MYSQL_PASSWORD=pcipass \
  MYSQL_DATABASE=pci MYSQL_SSL=false python3 backend/tests/integration_test.py
```

## 4. Mock-vendor seam (loopback)

Provider integrations that would otherwise require external egress are exercised against **in-repo mock
servers** bound to `127.0.0.1`:
- Exam-delivery vendors (Pearson VUE / Kryterion / Questionmark / PSI / TestReach) — `_MockVendor`.
- Certuvo external platform.
- Outbound integrations / webhooks (echo receiver; QuickBooks-as-QBO echo).

The production SSRF guard blocks loopback by default; tests opt in with
`INTEGRATIONS_ALLOW_PRIVATE_EGRESS=true`, mirroring a self-hosted deployment delivering to a private
bridge. Nothing ever leaves the process.

## 5. Egress policy & its consequences

Outbound egress is blocked in the build/test environment. Therefore live external-provider runs
(Stripe/Certuvo/exam-vendor/WhatsApp/Meta/Google), Render deployment, real backup/restore/DR, and
production-scale load are **Operator/External-pending** — documented and executed by an operator, never
simulated as if real (see `EXTERNAL_PROVIDER_TEST_PLAN.md`, `DR_RESTORE_RUNBOOK.md`).

## 6. Reproducing CI locally

- Backend: `dotnet build backend -c Release`, then run each Python suite (SQLite by default; MySQL via
  the env above).
- Backend unit: `dotnet test backend/tests/PCI.Backend.Tests`.
- Frontend: `cd frontend && npm ci && npm run lint && npm run typecheck && npm run test && npm run build`.
- E2E: `cd frontend && npx playwright test` (requires the staged SPAs + a bootable backend; runs on CI
  runners — a server bind is blocked in the sandbox).
