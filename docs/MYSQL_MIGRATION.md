# PCI — MySQL Database (migration, configuration, operations)

The PCI platform runs on **MySQL 8.x / MariaDB 10.11+** in every non-local environment
(staging, production, and DB integration tests). SQLite is permitted only for quick local
development; it is **not** a production or fallback database. The app enforces this: in
framework-default Production, explicit Production, or Staging it refuses to boot unless `DB_PROVIDER=mysql`, and there is
no silent SQLite fallback if MySQL is unreachable — it fails loudly and retries the connection.

The backend is provider-agnostic (raw SQL through `Data/Db.cs`, which translates SQLite-dialect
SQL to MySQL). No application code changes are needed to run on MySQL — only configuration.

---

## 1. MySQL server requirements

- MySQL **8.x** or MariaDB **10.11+**
- Storage engine: **InnoDB**
- Character set / collation: **utf8mb4** (`utf8mb4_unicode_ci` recommended)
- Application date/time is stored as UTC strings (VARCHAR), so the server timezone is irrelevant to correctness.

```sql
CREATE DATABASE pci CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'pci'@'%' IDENTIFIED BY '<strong-password>';
GRANT ALL PRIVILEGES ON pci.* TO 'pci'@'%';
FLUSH PRIVILEGES;
```

The schema is created automatically on first boot (`schema.mysql.sql` + idempotent migrations in
`Data/Migrate.cs`). The explicit money manifest is exact (`DECIMAL(12,2)`, with `DECIMAL(18,6)` for
provider CPC/spend values); integer-minor-unit partner-finance columns remain BIGINT.

---

## 2. Configuration (environment variables)

| Variable | Purpose | Example |
|---|---|---|
| `DB_PROVIDER` | `mysql` (or `mariadb`) selects MySQL; `sqlite` is explicit local dev; unknown values fail | `mysql` |
| `MYSQL_HOST` | server hostname | `db.internal` |
| `MYSQL_PORT` | port | `3306` |
| `MYSQL_DATABASE` | database name | `pci` |
| `MYSQL_USER` / `MYSQL_PASSWORD` | credentials (never commit the password) | `pci` / `***` |
| `MYSQL_SSL` | `required`/`true` to enforce TLS, `false` to disable (private network only) | `required` |
| `MYSQL_CONNECTION_STRING` | full override; wins over the discrete vars | |
| `MYSQL_CONNECT_TIMEOUT` | seconds to establish a connection (default 15) | `15` |
| `MYSQL_COMMAND_TIMEOUT` | seconds per command (default 30) | `30` |
| `MYSQL_POOL_MIN` / `MYSQL_POOL_MAX` | connection pool bounds (default 0 / 20) | `0` / `20` |
| `MYSQL_CONNECT_RETRIES` | connect attempts with backoff at boot (default 6) | `6` |

### Templates

**Local dev (SQLite — quick only):** leave `DB_PROVIDER` unset; `DATABASE_FILE=./pci.db`.

**Local dev / test against MySQL:**
```
DB_PROVIDER=mysql MYSQL_HOST=127.0.0.1 MYSQL_PORT=3306 MYSQL_DATABASE=pci MYSQL_USER=pci MYSQL_PASSWORD=pcipass MYSQL_SSL=false
```

**Staging / Production:** set `DB_PROVIDER=mysql`, `MYSQL_HOST/USER/PASSWORD` (secrets), `MYSQL_SSL=required`.
On Render these are in `render.yaml` (`sync:false` for secrets).

**DB integration tests on MySQL:** the Python suites honour `TEST_DB_PROVIDER=mysql` + the `MYSQL_*`
vars; CI runs the adversarial suites against MariaDB 10.11 and double-boot migration integrity
against both MariaDB 10.11 and Oracle MySQL 8.4.

---

## 3. One-time data migration (SQLite → MySQL)

Use `backend/tools/migrate_sqlite_to_mysql.py`. It preserves primary keys, relationships, password
hashes and money values, then reconciles row counts, financial totals, foreign-key integrity and
uniqueness. It only reads the source and only replaces the target's rows (idempotent per table).

```bash
# 0. Take the source SQLite offline (or snapshot it) so no new writes occur during migration.
cp /data/pci.db /backup/pci.sqlite.$(date +%F).db          # keep for rollback + audit

# 1. Create the MySQL schema by booting the app once against the target, then stop it:
DB_PROVIDER=mysql MYSQL_HOST=... MYSQL_PASSWORD=... dotnet run --project backend   # Ctrl-C after "platform is running"

# 2. Dry run (validate, no writes):
python3 backend/tools/migrate_sqlite_to_mysql.py --source /backup/pci.sqlite.<date>.db \
  --mysql-host <host> --mysql-db pci --mysql-user pci --mysql-password *** --dry-run

# 3. Migrate for real, saving the reconciliation report:
python3 backend/tools/migrate_sqlite_to_mysql.py --source /backup/pci.sqlite.<date>.db \
  --mysql-host <host> --mysql-db pci --mysql-user pci --mysql-password *** --report migration-report.json
```

Exit code `0` = clean (schema coverage, row counts, exact Decimal financial totals, FK integrity,
uniqueness all reconciled); source tables/columns absent from the target are discrepancies;
`2` = completed but with discrepancies (inspect the report / `DISCREPANCIES` list); `1` = error.
A matching row count is necessary but **not sufficient** — the tool also reconciles financial sums
and relationships. Review `migration-report.json` before cutover.

---

## 4. Cutover

1. Announce a short maintenance window.
2. Stop writes to the SQLite instance (scale the old service to 0 / enable maintenance mode).
3. Snapshot SQLite (`cp` above) — this is the rollback artifact.
4. Run the migration (section 3) and confirm the reconciliation report is clean.
5. Set `DB_PROVIDER=mysql` + `MYSQL_*` on the service and deploy.
6. Smoke-test: `/api/health`, admin login, a student login, a public page, a payment receipt.
7. Keep the SQLite snapshot for at least one retention cycle.

Do **not** run permanent dual writes to SQLite and MySQL. After cutover, MySQL is the single source of truth.

---

## 5. Backups & recovery

- **Logical backups:** `backend/tools/mysql_backup.sh` wraps `mysqldump --single-transaction`
  (consistent, non-locking on InnoDB) and gzips the output. Schedule it (cron / provider scheduler)
  daily; retain per policy; store encrypted off-box.
- **Point-in-time recovery:** enable binary logging on the MySQL server (managed providers do this by
  default) so you can replay to a specific moment between full backups.
- **Restore test:** periodically restore the latest dump into a scratch database and boot the app
  against it — a backup is only real once a restore has been proven.
- **Restore:** `gunzip -c backup.sql.gz | mysql -h <host> -u pci -p pci`

---

## 6. Rollback

If cutover fails, point `DATABASE_FILE` back at the SQLite snapshot and unset `DB_PROVIDER`
(non-production), or restore the pre-cutover MySQL dump. Because the migration never modifies the
source, the SQLite snapshot from step 3 is always a clean rollback target.
