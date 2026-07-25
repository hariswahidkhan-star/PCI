# MySQL / MariaDB support

The backend is **dual-provider**. **Production runs MySQL/MariaDB only** (`render.yaml` sets
`DB_PROVIDER=mysql`; Production refuses to boot on SQLite unless an explicit escape hatch is set —
see `Program.cs` and `docs/MYSQL_MIGRATION.md`).

SQLite remains the zero-config default for **local development** and for the fast CI smoke jobs
(`backend`, `backend-unit`, default Playwright `e2e`). Production-parity gates are the MySQL CI jobs:
`backend-mysql`, `backend-unit-mysql` (finance filter), and `e2e-mysql`.

## Enabling MySQL

Set these environment variables (on Render: the service's Environment settings):

| Var | Example | Notes |
|---|---|---|
| `DB_PROVIDER` | `mysql` | switches the provider (default `sqlite` for local/dev) |
| `MYSQL_HOST` | `your-db-host` | |
| `MYSQL_PORT` | `3306` | default 3306 |
| `MYSQL_DATABASE` | `pci` | must exist; the app creates all tables on first boot |
| `MYSQL_USER` | `pci` | needs full rights on the database |
| `MYSQL_PASSWORD` | `••••••` | |
| `MYSQL_SSL` | `required` | set `required`/`true` for managed production; unset is connector `Preferred`, `false` only for local dev |

Or set a single `MYSQL_CONNECTION_STRING` (a MySqlConnector connection string) to override all of the above.

On boot the log prints `[boot] database provider: MySql (schema: schema.mysql.sql)`. Migrations run
automatically and idempotently, exactly like the SQLite path — nothing else to do.

## How it works

- The app is written in SQLite-dialect SQL (the source of truth). `Data/Db.cs` translates that SQL to
  MySQL at runtime when `DB_PROVIDER=mysql` (datetime expressions, `INSERT OR IGNORE`, upserts,
  `last_insert_rowid()`/`changes()`, the proctoring `julianday` maths, `strftime`, partial unique indexes).
- Datetimes are stored as **strings** in the SQLite format `YYYY-MM-DD HH:MM:SS` on both providers, so all
  of the app's date parsing/compare logic is identical everywhere.
- The base MySQL schema is `schema.mysql.sql`, generated from `schema.sql` by `tools/sqlite_to_mysql.py`.
  **Regenerate it whenever you change `schema.sql`:** `python3 tools/sqlite_to_mysql.py`.
  Money columns are emitted as `DECIMAL(12,2)` (not `DOUBLE`). Do not hand-tune types in the generated file.
- Runtime money columns created in `Data/Migrate.cs` also use `DECIMAL(12,2)`. On MySQL, `EnsureMoneyDecimals`
  idempotently upgrades any legacy DOUBLE/FLOAT money columns in place (no data deletion).

## Verification

`tests/integration_test.py` and `tests/migration_integrity_test.py` run against either provider. Locally,
with a MySQL server:

```bash
TEST_DB_PROVIDER=mysql MYSQL_HOST=127.0.0.1 MYSQL_USER=pci MYSQL_PASSWORD=pcipass MYSQL_DATABASE=pci \
  python3 tests/integration_test.py
TEST_DB_PROVIDER=mysql MYSQL_HOST=127.0.0.1 MYSQL_USER=pci MYSQL_PASSWORD=pcipass MYSQL_DATABASE=pci \
  python3 tests/migration_integrity_test.py
```

CI runs the adversarial suite and migration integrity against MariaDB 10.11 (`backend-mysql`) on every
push, plus finance xUnit on MySQL (`backend-unit-mysql`) and Playwright on MySQL (`e2e-mysql`).

## Should you use it?

| Context | Provider |
|---|---|
| Production / staging (Render) | **MySQL — required** |
| Local first boot / quick UI work | SQLite is fine |
| Anything involving money, partners, or release candidates | MySQL (match production) |
