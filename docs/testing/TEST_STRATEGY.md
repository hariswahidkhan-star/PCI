# PCI Platform — Test Strategy

_The single reference for how the PCI platform is tested: what we test, at which layer, with which
tools, and the entry/exit criteria a change must satisfy. It is descriptive of the suite as it exists
today (not aspirational) and is kept in step with `TEST_COVERAGE_MATRIX.md` (what exists / what is
missing) and `DEFECT_REGISTER.md` (what the tests have learned about the product)._

## 1. Objectives & principles

- **Risk-based.** Money movement, authentication/2FA, authorization (RBAC/IDOR), privacy/PII, exam
  integrity and credential issuance are the highest-risk areas and carry the deepest coverage.
- **Audit-first, additive.** New coverage is added incrementally; a test that exposes a defect is
  never weakened or deleted to go green — the defect is fixed with a regression test, or logged in
  `DEFECT_REGISTER.md` with its residual risk.
- **Real dependencies where they matter.** DB-behaviour is validated against **both** SQLite and a
  real **MySQL/MariaDB**; app code is not swapped for SQLite-only shims. Provider integrations use
  the vendors' sandbox/mock facilities — never fabricated "green".
- **No secrets or PII in tests or fixtures.** Synthetic accounts only; credentials, payment data,
  TOTP secrets, recovery codes, government IDs and private student data never appear in test inputs,
  outputs or the repository.
- **Honest reporting.** Results are classified Automated / Manually-verified / External-provider-
  pending / Operator-config-pending / Residual-risk. We do not claim "100% defect-free".
- **Naming.** Certifications are **PCL-AI, PFL-AI, PML-AI**; older/interim cert names are not used.

## 2. Test layers (the pyramid, as built)

| Layer | Tooling | Where | Baseline |
|---|---|---|---|
| **Static quality / supply chain** | `-warnaserror` C#, ESLint (flat), `tsc --noEmit`, NuGet vuln gate (allow-list-aware), `npm audit --omit=dev`, gitleaks, actionlint/hadolint (informational) | `static-quality` CI job | 0 errors; 1 allow-listed residual advisory |
| **Backend unit (decision logic)** | xUnit — `backend/tests/PCI.Backend.Tests` (temp-SQLite `Db`, temp storage, no HTTP) | `backend-unit` CI job | **458 tests** |
| **Backend integration / live-HTTP** | Python — `integration_test.py` + `founding/honorary/…` suites; boots the real `dotnet` DLL | `backend` (SQLite) + `backend-mysql` (MySQL) CI jobs | **967 assertions / provider** |
| **Migration & DB integrity** | `migration_integrity_test.py` (double-boot idempotency + schema.sql conformance + SQLite↔MySQL parity) | `backend` / `backend-mysql` | 191 shared tables, 0 drift |
| **Frontend component / unit** | Vitest + React Testing Library + jsdom | `frontend` CI job | **106 tests** |
| **Browser E2E + accessibility** | Playwright (Chromium) + `@axe-core/playwright` | `e2e` CI job | 23 tests |
| **Desktop (SecureExam)** | xUnit (Core + Tests) | `secureexam-core-linux` / `secureexam-windows` | 21 tests |

## 3. What each risk area maps to

See `REQUIREMENT_TRACEABILITY_MATRIX.md` for the requirement→test→evidence mapping. In summary:

- **Auth / sessions / 2FA** — integration lockout + TOTP-replay (§14/§28), xUnit `Security` (TOTP RFC-6238
  recompute, bcrypt, AES-GCM), component `Login`/`AdminLogin` TOTP step-up.
- **Payments / settlement** — integration webhook settle/refund/dispute (§1/§29), xUnit `SettlementTests`,
  component `Billing` code-validation gate + `Payments` reconciliation.
- **Authorization (RBAC/IDOR)** — integration viewer-403 section sweep (§38) + attachment IDOR (§30),
  xUnit `Rbac`, component console RBAC gates (ExamExceptions `ex_reopen`, Payments `finance`).
- **Privacy / PII** — integration erasure lifecycle (§27), Honorary IDV metadata-only (§54), analytics
  no-raw-IP; CSV formula-injection fix (SEC-2); stored-XSS sanitizer (`HtmlSanitize`).
- **Exam integrity / credentials** — integration exam lifecycle + suspend/reinstate (§2/§31), xUnit
  `ExamAuthorization`/`CredentialCpd`/`Lifecycle`, component `Certifications`/`Credentials`.
- **Transport / headers / CORS** — integration §9b (nosniff/CSP/HSTS/COOP/Permissions-Policy/X-Robots-Tag
  + CORS non-reflection + preflight), SEC-4 hostile-file robustness.

## 4. Environments

Detailed in `TEST_ENVIRONMENTS.md`. Two DB providers (SQLite, MySQL) run in CI; local dev uses a
MariaDB socket. External egress is blocked in the build/test environment — provider/Render/DR/perf
work is classified Operator/External-pending (see `EXTERNAL_PROVIDER_TEST_PLAN.md`), never simulated
as if real. Provider integrations exercised here use in-repo mock vendors on loopback.

## 5. Test data

Detailed in `TEST_DATA_PLAN.md`. Synthetic accounts and fixtures with reserved prefixes
(`zephyr…`, `-NN@ex.co`); seed/borrowed data is restored; each run uses temp `DATABASE_FILE` /
`STORAGE_ROOT` and free ports for isolation. No real personal data, ever.

## 6. Entry / exit criteria (Definition of Done for a change)

**Entry** — a change may merge only via PR with CI configured; the coverage matrix row for the
touched area is identified.

**Exit (all must hold):**
1. All CI jobs green on the PR head: `static-quality`, `backend`, `backend-mysql`, `backend-unit`,
   `frontend`, `e2e`, `secureexam-core-linux`, `secureexam-windows`.
2. Any app-code change touching DB behaviour is verified on **both** SQLite and MySQL.
3. No assertion weakened or test deleted to obtain green; any newly-surfaced defect is fixed with a
   regression test or recorded in `DEFECT_REGISTER.md`.
4. No secrets/PII added to code, fixtures, logs or artifacts; gitleaks clean.
5. Coverage matrix + (if a finding) defect register updated in the same PR.
6. The final PR is **not** auto-merged — a human reviews and merges.

## 7. Roles

- **Author** implements the change + its tests, runs the suites locally, and updates the matrix.
- **Reviewer** (human) confirms the exit criteria and merges; they own the go/no-go using
  `RELEASE_READINESS_TEMPLATE.md`.
- **Operator** executes the External/Operator-pending items (provider sandbox runs, Render staging,
  DR rehearsal) that cannot run in CI.

## 8. Residual risk register (living)

- **CVE-2025-6965 / GHSA-2m69-gcr7-jv3q** (SQLitePCLRaw.lib.e_sqlite3, no upstream patch) — allow-listed
  in `backend/tools/nuget-vuln-allowlist.json`; low reachability (no user-supplied SQL; production uses
  MySQL). Reviewed per the date in the allow-list.
- Open product findings are tracked in `DEFECT_REGISTER.md` (e.g. DEF-2 exam retake-wait dead-end).
