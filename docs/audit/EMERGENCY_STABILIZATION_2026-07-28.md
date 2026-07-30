# Emergency stabilization audit and repair — 2026-07-28

Scope: the merge/CI/deployment stabilization assignment. Branch:
`claude/pci-emergency-audit-98hlqt`, created from `main` @
`cae8ff4fbc0b3a0fe50204ba365be3223f874ee9`. PR #158 is classified separately in
`PR158_CLASSIFICATION_2026-07-28.md`; its head is preserved as `backup/pr-158-original`.

**Nothing here merges automatically.** This branch carries only the startup, configuration,
repository-hygiene, test and Docker-CI repairs — no books, no new features, no finance changes.

---

## 1. Confirmed defect register (verified against current `main`, not taken on faith)

| # | Defect | Evidence on `main` | Status |
|---|---|---|---|
| D1 | `new Db(dbPath)` called outside any try/catch — an unreachable/misconfigured MySQL escaped as an unhandled crash (exit 134 + stack trace) instead of a diagnosable refusal | `backend/Program.cs` (DB open after the preflight block) | **Fixed here** — exit 75 with a `[db] refusing to start` diagnostic naming provider + settings, never printing secrets; migration handlers (75 schema-compat / 70 migration) untouched |
| D2 | `ConfigIssues()` validated `APP_BASE_URL` only by substring (`localhost`/`127.0.0.1`) while the pre-DB preflight required absolute + https + non-loopback — System-check could call healthy what boot refused | `backend/Program.cs` `ConfigIssues()` | **Fixed here** — one shared predicate `IsPublicHttpsUrl` now used by the preflight, `ConfigIssues()`, and the PCI World base-URL resolution; RENDER_EXTERNAL_URL adoption, PCI World-only and persistent-disk downgrades all preserved (proven by the pre-existing tests still passing) |
| D3 | Committed Python bytecode (`backend/tools/__pycache__/sqlite_to_mysql.cpython-311.pyc`) tracked; `.gitignore` had no bytecode rules | `git ls-tree origin/main` | **Fixed here** — untracked + deleted; `__pycache__/` and `*.py[cod]` ignored; `git ls-files | grep -E '(__pycache__|\.py[co]$)'` returns nothing |
| D4 | CI (11 jobs) never built or booted the deployment Docker image; hadolint/actionlint informational-only — deploys could fail at container boot with green CI | `.github/workflows/build.yml` | **Fixed here** — new **blocking** `docker-image` job: builds the root Dockerfile, boots MySQL 8.4 + the image with representative production env, waits on `/api/health`, asserts `database_provider:"mysql"`, probes `/`, `/app/`, `/admin/`, `/world-app/`, always prints container logs, always cleans up |
| D5 | The MySQL schema generator's table scanner had no dedicated tests; its output was only checked for parity with the committed artifact (green even if both were wrong) | `backend/tools/sqlite_to_mysql.py` | **Fixed here** — `main`'s scanner was already balanced-paren (the PR-claimed defect was independently fixed); adopted the PR's backticked-name tolerance and added `backend/tests/schema_generator_test.py` (12 cases: one-line tables, nesting, literals with `''` and parens, comments with apostrophes, index key lengths incl. composite, plus 75/75 real-schema parse and a no-unprefixed-TEXT-index sweep of the generated output) |
| D6 | `DEPLOY.md` stale: claimed production unconditionally refuses SQLite, omitted the persistent-disk auto-posture, PCI World-only posture, exit codes 75/70, log prefixes, key-preservation rules | `DEPLOY.md` vs `Program.cs` | **Fixed here** — rewritten from current code: three postures, exit-code table (78/75/70), blocker list, log-prefix table, encryption-key preservation/rotation, Render first-provision behavior |
| D7 | `render.yaml` blank-on-provision `CREDENTIAL_ENCRYPTION_KEY` | — | **Already fixed on `main`** (`generateValue: true` + concrete base-URL values). No change made |

Process finding (recorded, no code change): PR #187's history documents that a `git add -A`
staged an authorization change missing an `employer_id` condition (temporary cross-tenant
exposure). All commits on this branch stage explicitly named files only, reviewed via
`git status --short` / `git diff --cached` before each commit.

## 2. Verification evidence (commit SHA recorded at push; environment: Linux sandbox, .NET SDK 8.0.129, python3, node 20/22)

| Gate | Result |
|---|---|
| `dotnet build -c Release` (backend, `TreatWarningsAsErrors`) | **0 warnings, 0 errors** |
| `python3 tests/production_config_test.py` | **19/19 PASS** — all 11 pre-existing cases preserved + 3 base-URL-consistency cases + 5 unreachable-MySQL cases (exit 75, `[db]` diagnostic, no secret echo, no `Unhandled exception`, no fallback SQLite file) |
| `python3 tests/schema_generator_test.py` (new) | **12/12 PASS** |
| `python3 tools/sqlite_to_mysql.py` then `--check` | Regeneration **byte-identical**; "schema.mysql.sql is current" |
| `python3 tests/migration_integrity_test.py` | **13/13 PASS** (SQLite mode) |
| 17 logic suites (CI's backend-job list, incl. pypdf) | **All pass** (no FAIL/✗; loop exit 0) |
| Frontend `npm ci` + `typecheck` + `build` | **Pass** — student, admin and world bundles all emitted |
| Backend unit tests (`dotnet test tests/PCI.Backend.Tests -c Release`) | **1648/1648 PASS** (0 failed, 0 skipped; 32 m 23 s) |
| SecureExam Core tests (`dotnet test PCI.SecureExam.Tests -c Release`, Linux) | **21/21 PASS** |
| MariaDB 10.11 / Oracle MySQL 8.4 parity, Playwright E2E (both providers), secureexam-windows | **Not runnable in this sandbox** (no DB service; Windows job) — covered by existing blocking CI jobs which run on this branch's push/PR |
| Docker image build + boot | **Not runnable in this sandbox** — the session network policy blocks Docker Hub's CDN (`production.cloudfront.docker.com` 403). The new blocking `docker-image` CI job performs exactly this on GitHub runners; its first run on this branch is the evidence |

**Honest gap:** until the branch's CI run completes, the Docker gate and the MySQL/E2E matrices
are asserted by CI design, not by local execution. Do not merge before that run is green.

## 3. CI job inventory (after this change)

`backend` (build, config preflight, **schema-generator parser tests**, 17 logic suites, JS gate,
smoke, integration, 500-sweep, system-check), `backend-mysql` (MariaDB 10.11 integration),
`backend-mysql8` (Oracle MySQL 8.4 migration/parity), `backend-unit`, `backend-unit-mysql`,
`frontend` (typecheck/lint/audit/tests/build), `e2e` (SQLite), `e2e-mysql` (MariaDB),
`secureexam-windows`, `secureexam-core-linux`, **`docker-image` (new, blocking)**,
`static-quality` (NuGet vulns + gitleaks blocking; actionlint/hadolint still informational —
promote once their baselines are cleared, tracked as residual risk R4).

## 4. Feature-state matrix (features reported "not working")

| Feature | Flag key (site_settings) | Seeded value | Backend | Status |
|---|---|---|---|---|
| Careers | `pciworld_careers_enabled` | `'0'` | `Data/CareersSchema.cs` (seeds), endpoints gated on the flag | **Disabled by feature flag — by design.** Launch blocked on employer verification / privacy / retention / commercial approvals. Not a code defect |
| Community Rooms | `world_community_enabled` | `'0'` | `Data/CommunitySchema.cs` | **Disabled by feature flag — by design.** Launch blocked on legal/age/jurisdiction + Trust & Safety decisions |
| Forum | `pciworld_forum_enabled` | `'0'` | `Data/ForumSchema.cs` | **Disabled by feature flag — by design** |
| Community Images | `pciworld_community_images_enabled` | `'0'` | `Data/CommunityMediaSchema.cs` (comment cites CCP-P1-003 child-safety review) | **Disabled by feature flag — by design** |
| Community participation | jurisdiction configuration | empty | — | **Blocked by missing configuration — intentionally fail-closed** until approved jurisdictions are entered |

None of these gates were bypassed or altered by this branch. Enabling any of them requires the
recorded operator/legal decisions listed in §6.

## 5. Deployment configuration matrix

See the rewritten `DEPLOY.md`: (a) production MySQL (approved target; `render.yaml` defaults),
(b) SQLite-on-persistent-disk interim posture (auto-detected via writable `/data` + `/data` DB
path, or explicit `ALLOW_SQLITE_IN_PRODUCTION=true`), (c) PCI World-only
(`PCIWORLD_ONLY` + `PCIWORLD_ALLOW_SQLITE` + `/data`). Exit codes: 78 config, 75 DB
unavailable / schema-compat, 70 migration failure. Every documented behavior corresponds to a
`production_config_test.py` case or a code path cited above.

## 6. Residual-risk register (out of scope here; each needs its own PR or a recorded decision)

**P0/P1 finance correctness** (registers: `docs/finance/PARTNER_FINANCE_PHASE0_AUDIT.md`,
`PARTNER_FINANCE_PHASES_1_6_REPORT.md`): refunds not netting against approved-unpaid
settlements; partial-settlement idempotency/concurrency; append-only settlement evidence;
currency enforcement/partitioning; commission mutation failures swallowed while webhooks ack
(incl. the unverified P1-S catch-swallow); one signed financial model across refund/revenue
reporting. **Next engineering priority after this branch is green.**

**Test/security gaps**: World-schema seed races under parallel MySQL tests; no automated
SignalR/WebSocket connect-time authorization test; E2E passes that rely on retries; S3
verification non-blocking; Stripe/Certuvo/IDV/translation/proctoring unverified against real
sandboxes; audit-log actor/subject normalization.

**Product decisions required before any behavior change**: ordered vs unordered critical-path
grading (PCI World vs Simulation Lab disagree); shared-IP community restrictions; SignalR
scale-out topology; employer verification policy; careers privacy/retention; community
age/jurisdiction policy; moderation vendor + benchmark corpus; retired PCP-AI branding in
published historic downloads.

**R4**: hadolint/actionlint remain informational; promote to blocking after baseline cleanup.

## 7. Recommended PR split (replacing PR #158)

- **PR A — this branch**: startup guard, config-validation unification, config tests,
  schema-generator tests + backtick tolerance, hygiene, Docker CI gate, DEPLOY.md, audit docs.
- **PR B / PR C** — PML-AI / PFL-AI source corpora from `backup/pr-158-original`
  (`docs/books/` only, no backend files). **PR D** — the 4 built PDFs, after a storage decision
  (LFS / releases / external). Then close PR #158.

## 8. Rollback plan for this branch

All changes are code/docs/CI only — **no schema change, no data migration, no settings writes**.
Rollback = `git revert` of the branch's commits (each commit is single-purpose and independently
revertible). The Docker CI job can be removed on its own without touching the runtime fixes.
Reverting the `Program.cs` guard restores the previous crash-on-unreachable-DB behavior but
nothing else regresses; the shared URL predicate revert restores the weaker `ConfigIssues()`
check only.
