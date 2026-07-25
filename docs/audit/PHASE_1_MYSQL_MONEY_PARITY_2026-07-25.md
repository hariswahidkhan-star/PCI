# PCI Complete Platform Audit — Phase 1 MySQL Parity & Money Types

**Date:** 2026-07-25 · **Branch:** `cursor/platform-audit-phase0-d975` · **Continues:** Phase 0 inventory (`PHASE_0_PLATFORM_AUDIT_2026-07-25.md`).

---

## 1. Executive summary

The initial Phase 1 pass was re-run adversarially. That deeper pass disproved several first-pass
assumptions: framework-default Production and Staging could still open SQLite; the PCI World
production image defaulted to ephemeral SQLite; Oracle MySQL's second boot was untested; 13
salary/marketing money columns remained DOUBLE; and Stripe metadata could disagree with the cents
actually charged. These defects are now remediated and gated.

**Production-readiness (Phase 1 only):** MySQL remains the production database; SQLite stays local/CI smoke. The deep pass found and fixed three Critical database/startup defects, but finance Criticals DEF-AUDIT-15/16 and World/Partner React migrations remain open.

---

## 2. Findings remediated

| ID | Severity | Finding | Status |
|----|----------|---------|--------|
| DEF-AUDIT-07 | High | Generator `REAL→DOUBLE` + fragile hand-tunes; Migrate money cols as `REAL` | **FIXED** |
| DEF-AUDIT-08 | Medium | Docs implied SQLite disk as production default | **FIXED** |
| DEF-AUDIT-09 | Critical | Default Production / `DOTNET_ENVIRONMENT` / Staging bypassed pre-open MySQL guard | **FIXED** |
| DEF-AUDIT-10 | Critical | PCI World production image silently booted ephemeral SQLite | **FIXED** |
| DEF-AUDIT-11 | Critical | Oracle MySQL second boot could fail on `CREATE INDEX IF NOT EXISTS` translation | **FIXED + MySQL 8.4 gate** |
| DEF-AUDIT-12 | High | Runtime salaries, ad budgets, CPC/spend/conversion money remained DOUBLE | **FIXED** |
| DEF-AUDIT-13 | High | Stripe metadata could override authoritative `amount_total` | **FIXED** |
| DEF-AUDIT-14 | High | SQLite→MySQL cutover could exit clean while skipping source tables/columns | **FIXED** |

The deep review also found finance-domain defects outside this database-parity increment
(DEF-AUDIT-15 onward). They remain explicitly OPEN; full production readiness is not claimed.

---

## 3. Remediations shipped

### 3.1 Money DECIMAL parity

1. **`backend/tools/sqlite_to_mysql.py`** — after `REAL→DOUBLE`, known money columns are rewritten to `DECIMAL(12,2)`. Header documents “do not hand-tune; regenerate.”
2. **`backend/schema.mysql.sql`** — regenerated from `schema.sql` via the updated tool.
3. **`backend/Data/Migrate.cs`** — table-qualified money manifest validates presence, type,
   precision and scale after every runtime installer. Legacy types upgrade in place; non-local boot
   fails if the invariant does not converge.
4. **Runtime schemas** — prices/salaries/budgets use `DECIMAL(12,2)`; provider CPC/spend values use
   `DECIMAL(18,6)`; percentages and scores intentionally remain DOUBLE.
5. **Migration integrity** — explicit manifest (not name heuristics), MySQL double boot, sentinel +
   seed preservation, and a planted legacy DOUBLE value upgraded to DECIMAL.
6. **Payment boundary** — checkout quantizes cents once; webhook persists Stripe's integer
   `amount_total`, never floating metadata. Partner arithmetic uses wide `Int128` intermediates;
   Google Ads multiplies before converting budgets to micros.
7. **Cutover** — source-only tables/columns are fatal discrepancies; money is quantized and
   reconciled with Python `Decimal`, not float sums.

Percentages (`commission_pct`, `default_discount_percentage`, `discount_percent`, `pass_mark_pct`) and scores remain DOUBLE by design.

### 3.2 Docs / CI clarity (SQLite = smoke, MySQL = production parity)

Updated: `backend/MYSQL.md`, `backend/.env.example`, `Dockerfile`, `DEPLOY.md` (Render/MySQL), `backend/RUN.md`, `docs/testing/TEST_ENVIRONMENTS.md` (adds `e2e-mysql` / `backend-unit-mysql`), `.github/workflows/build.yml` comments.

Runtime now derives posture from ASP.NET's actual environment, so unset environment (framework
default Production), `DOTNET_ENVIRONMENT=Production`, and Staging fail before DB open. The dedicated
PCI World image is Production+MySQL and cannot create a preview SQLite DB.

CI now runs MariaDB 10.11 plus an Oracle MySQL 8.4 double-boot gate. MySQL Playwright attests the
provider, disables server reuse and dirty-state retry, and schema generation has a `--check` gate.

**Not done (deliberate):** flipping default Playwright or full xUnit to MySQL — SQLite remains the fast smoke; MySQL jobs are the parity gates. Full `backend-unit` on MySQL still needs fixture hardening before widening the filter.

---

## 4. Verification

| Check | Result |
|-------|--------|
| Generator money → DECIMAL | verified locally after regenerate |
| Non-money REAL → DOUBLE | verified (`percent`, `pass_mark_pct`, …) |
| `dotnet build -c Release` | **PASS** |
| `migration_integrity_test.py` SQLite | **13/13 PASS** |
| production/staging pre-open regression | **5/5 PASS** |
| `migration_integrity_test.py` MySQL mode | **23/23 PASS** (MariaDB 10.11; double boot) |
| adversarial HTTP integration (SQLite) | **1159/1159 PASS** |
| adversarial HTTP integration (MariaDB) | **1159/1159 PASS** |
| complete backend xUnit | **589/589 PASS** |
| frontend Vitest | **270/270 PASS** |
| Oracle MySQL 8.4 | pending CI job `backend-mysql8` |
| No P0 open from Phase 0 | DEF-AUDIT-01…04 remain FIXED; 05/06 OPEN (architecture) |

---

## 5. Residual risks / next phases

| Item | Phase |
|------|-------|
| Full xUnit suite on MySQL (beyond finance filter) | later hardening |
| Settlement replay/refund/commission/currency defects (DEF-AUDIT-15+) | finance remediation increment |
| Seven state-sensitive browser journeys masked by CI retry (DEF-AUDIT-20) | E2E determinism hardening |
| Public + student critical journeys re-verify on MySQL E2E depth | **Phase 2** |
| Partner / World React | Phases 4 / 6 |
| SQLite still default for local `dotnet run` | intentional |

---

## 6. Recommendation

Ship Phase 1 with Phase 0. Do **not** claim full-platform DoD. Proceed to Phase 2 journey re-verification on the MySQL E2E gate.
