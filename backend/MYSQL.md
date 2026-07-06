# MySQL / MariaDB support

The backend is **dual-provider**. It runs on SQLite (default, zero config) or MySQL/MariaDB —
same code, same behaviour, proven by the same 102-assertion adversarial suite on both.

## Enabling MySQL

Set these environment variables (on Render: the service's Environment settings):

| Var | Example | Notes |
|---|---|---|
| `DB_PROVIDER` | `mysql` | switches the provider (default `sqlite`) |
| `MYSQL_HOST` | `your-db-host` | |
| `MYSQL_PORT` | `3306` | default 3306 |
| `MYSQL_DATABASE` | `pci` | must exist; the app creates all tables on first boot |
| `MYSQL_USER` | `pci` | needs full rights on the database |
| `MYSQL_PASSWORD` | `••••••` | |
| `MYSQL_SSL` | `false` | omit (or leave unset) to require TLS; set `false` only for local dev |

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

## Verification

`tests/integration_test.py` runs against either provider. Locally, with a MySQL server:

```bash
TEST_DB_PROVIDER=mysql MYSQL_HOST=127.0.0.1 MYSQL_USER=pci MYSQL_PASSWORD=pcipass MYSQL_DATABASE=pci \
  python3 tests/integration_test.py
```

CI runs the full adversarial suite against a MariaDB 10.11 service (the `backend-mysql` job) on every push,
alongside the SQLite run — so MySQL parity is a permanent gate, not a one-off.

## Should you use it?

For a single-instance deployment, SQLite on a persistent disk is simpler, faster, and has zero hosting
cost — it remains the default and the recommended choice until you genuinely need multi-instance scale or a
managed database. MySQL is here, fully tested, for when you do.
