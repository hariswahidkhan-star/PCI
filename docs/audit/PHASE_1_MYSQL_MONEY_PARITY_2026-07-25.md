# PCI Complete Platform Audit — Phase 1 MySQL Parity & Money Types

**Date:** 2026-07-25 · **Branch:** `cursor/platform-audit-phase0-d975` · **Continues:** Phase 0 inventory (`PHASE_0_PLATFORM_AUDIT_2026-07-25.md`).

---

## 1. Executive summary

Phase 1 closes the remaining **silent SQLite-as-production** documentation paths and the **money-column DOUBLE gap** that regenerating `schema.mysql.sql` could reintroduce. Finance-module minor-unit tables were already exact; base pricing/payment/waiver columns and runtime Migrate DDL are now DECIMAL on MySQL with gating CI assertions.

**Production-readiness (Phase 1 only):** MySQL remains the production database; SQLite stays local/CI smoke. World/Partner React migrations (DEF-AUDIT-05/06) remain open for later phases. No new P0 defects opened.

---

## 2. Findings remediated

| ID | Severity | Finding | Status |
|----|----------|---------|--------|
| DEF-AUDIT-07 | High | Generator `REAL→DOUBLE` + fragile hand-tunes; Migrate money cols as `REAL` | **FIXED** |
| DEF-AUDIT-08 | Medium | Docs implied SQLite disk as production default | **FIXED** |

No new Critical/P0 defects found in Phase 1 scope.

---

## 3. Remediations shipped

### 3.1 Money DECIMAL parity

1. **`backend/tools/sqlite_to_mysql.py`** — after `REAL→DOUBLE`, known money columns are rewritten to `DECIMAL(12,2)`. Header documents “do not hand-tune; regenerate.”
2. **`backend/schema.mysql.sql`** — regenerated from `schema.sql` via the updated tool.
3. **`backend/Data/Migrate.cs`** — runtime money DDL uses `DECIMAL(12,2)`; new **`EnsureMoneyDecimals`** (MySQL-only) idempotently `MODIFY`s legacy DOUBLE/FLOAT/REAL money columns in place (preserves nullability/default; never deletes data). Covers payments/waivers/payouts plus `certification_routes.fee_amount` and Marketing Centre promo/conversion/budget money columns.
4. **`MarketingSchema.cs` / `MultiCert.cs`** — new CREATE DDL for those money columns uses `DECIMAL(12,2)`.
5. **`backend/tests/migration_integrity_test.py`** — §5e/5f/5g are **gating**: live MySQL money columns must not be inexact; DECIMAL scale 2; generated `schema.mysql.sql` must not contain money `DOUBLE`.

Percentages (`commission_pct`, `default_discount_percentage`, `discount_percent`, `pass_mark_pct`) and scores remain DOUBLE by design.

### 3.2 Docs / CI clarity (SQLite = smoke, MySQL = production parity)

Updated: `backend/MYSQL.md`, `backend/.env.example`, `Dockerfile`, `DEPLOY.md` (Render/MySQL), `backend/RUN.md`, `docs/testing/TEST_ENVIRONMENTS.md` (adds `e2e-mysql` / `backend-unit-mysql`), `.github/workflows/build.yml` comments.

**Not done (deliberate):** flipping default Playwright or full xUnit to MySQL — SQLite remains the fast smoke; MySQL jobs are the parity gates. Full `backend-unit` on MySQL still needs fixture hardening before widening the filter.

---

## 4. Verification

| Check | Result |
|-------|--------|
| Generator money → DECIMAL | verified locally after regenerate |
| Non-money REAL → DOUBLE | verified (`percent`, `pass_mark_pct`, …) |
| `dotnet build -c Release` | **PASS** |
| `migration_integrity_test.py` SQLite | **13/13 PASS** |
| `migration_integrity_test.py` MySQL mode (§5e–5g) | **23/23 PASS** (local MariaDB 10.11) |
| No P0 open from Phase 0 | DEF-AUDIT-01…04 remain FIXED; 05/06 OPEN (architecture) |

---

## 5. Residual risks / next phases

| Item | Phase |
|------|-------|
| Full xUnit suite on MySQL (beyond finance filter) | later hardening |
| Public + student critical journeys re-verify on MySQL E2E depth | **Phase 2** |
| Partner / World React | Phases 4 / 6 |
| SQLite still default for local `dotnet run` | intentional |

---

## 6. Recommendation

Ship Phase 1 with Phase 0. Do **not** claim full-platform DoD. Proceed to Phase 2 journey re-verification on the MySQL E2E gate.
