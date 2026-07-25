# Postmortem — Render deploy failures on `PCI-Platform` (July 2026)

**Symptom.** Consecutive Render deploy-failure emails for the `PCI-Platform` service across
unrelated commits — Simulation Lab P2 (#114), the Books docs-only merge (#153) and PCI World
(#154). "We encountered an error during the deploy process … your latest changes may not be live."

**Key observation.** #153 changed **only** `docs/books/**`, and `.dockerignore` excludes `docs/`
entirely — so that deploy's build context was byte-identical to the previous one. A commit that
changes nothing in the image cannot break a working build. The failure was therefore not
commit-specific: it was standing, and every push simply re-reported it.

---

## What was verified

| Stage | Result |
|---|---|
| Docker stage 1 — React build (`npm ci && npm run build`) | **passes** — typecheck clean, both bundles emitted |
| Docker stage 2 — `dotnet restore` + `dotnet publish -c Release` | **passes** — 0 errors, 0 warnings (with `TreatWarningsAsErrors=true`) |
| Boot with only the env `render.yaml` supplies inline | **exit 78** — reproduced |
| Boot with MySQL credentials added | **exit 78** — reproduced |
| Boot with all five blockers satisfied | preflight passes; proceeds to connect to MySQL |

The image builds. **The container was failing at boot**, before it could ever answer
`healthCheckPath: /api/health`, so Render reported a failed deploy.

## Root cause

`backend/Program.cs` runs a fail-closed production preflight (added by the platform-audit work,
PR #143) *before* any database is opened. In `Production` it requires **all** of:

1. `DB_PROVIDER=mysql` (or `mariadb`)
2. `MYSQL_HOST` + `MYSQL_PASSWORD` (or `MYSQL_CONNECTION_STRING`)
3. `APP_BASE_URL` — public **https**, non-loopback
4. `ALLOWED_ORIGIN` — explicit, never `*`
5. `CREDENTIAL_ENCRYPTION_KEY`

Reproduced verbatim:

```
[config] Refusing to open database: MySQL is selected but connection settings are incomplete
         (need MYSQL_HOST + MYSQL_PASSWORD, or MYSQL_CONNECTION_STRING).                  → exit 78
[config] Refusing to open database: APP_BASE_URL must be a public HTTPS URL;
         ALLOWED_ORIGIN must be explicit; CREDENTIAL_ENCRYPTION_KEY is required           → exit 78
```

Every one of those five is marked `sync: false` in `render.yaml`, meaning **blank until an operator
fills it in on the dashboard**. `CREDENTIAL_ENCRYPTION_KEY` was the newest of them, introduced by
the same PR that added the preflight — which is why deploys began failing at that commit and then
failed for *every* commit after it, regardless of content. The guard rail was working as designed;
what was missing was any signal that a new required secret had appeared.

**Operator action required (cannot be done from the repository):** set the values above in
Render → the service → Settings → Environment, then redeploy. `CREDENTIAL_ENCRYPTION_KEY` is now
generated automatically for newly provisioned services (see the fixes below); an existing service
still needs it supplied once.

---

## Defects found and fixed alongside

**1. An unopenable database crashed instead of failing cleanly.**
`Db.OpenWithRetry()` retried, then let the exception escape the `Db` constructor — which
`Program.cs` called outside any `try`/`catch`. A wrong MySQL host, password, TLS mode or a
not-yet-created database therefore aborted the process with **exit 134 and a ~20-line stack
trace**, burying the cause in the deploy log. This is the single commonest production deploy
failure and it produced the least usable diagnostic. Now:

```
[db] refusing to start: cannot open the 'mysql' database — Unable to connect to any of the specified MySQL hosts.
[db] check MYSQL_HOST/MYSQL_PORT reachability from this host, MYSQL_USER/MYSQL_PASSWORD, that
     MYSQL_DATABASE exists, and MYSQL_SSL (managed providers usually need 'required'). Raise
     MYSQL_CONNECT_RETRIES if the database is still starting up.                          → exit 75
```

Pinned by three regression assertions in `backend/tests/production_config_test.py` (exit code is
75, the cause and settings are named, no unhandled-exception trace).

**2. The MySQL schema generator silently dropped index key lengths — CI was red on `main`.**
`tools/sqlite_to_mysql.py` ended each `CREATE TABLE` block at the first `"\n)"`. A single-line
table (`CREATE TABLE site_settings ( skey TEXT PRIMARY KEY, svalue TEXT );`) has no newline before
its `)`, so the match ran on for ~330 lines and swallowed the `CREATE TABLE` statements that
followed. Those tables were then absent from the column-type map, so TEXT columns in their indexes
got no `(191)` prefix — invalid on MySQL 8. That is how `ix_redemptions_email` lost its prefix and
had to be hand-patched directly into `schema.mysql.sql` (commit `7972201`) without fixing the
generator. Consequences: the documented "regenerate the MySQL schema" step in `CLAUDE.md` §3.8
silently reverted the fix, and CI's **`backend-mysql` → "Generated MySQL schema is current"**
(`python3 tools/sqlite_to_mysql.py --check`, `build.yml:150`) was **failing on `main`** — unnoticed
because runs were sitting queued.

Fixed with a balanced-parenthesis scanner that is indifferent to line breaks and skips string
literals and `--` comments (an apostrophe in `-- the board's reason` was also unbalancing the scan
and hiding `honorary_awards`). Now 73/73 tables parse, `--check` reports current, and regeneration
reproduces the reviewed `schema.mysql.sql` **byte-for-byte** — the hand-patch is derivable from
source again.

**3. `render.yaml` guaranteed a failed first deploy.** `CREDENTIAL_ENCRYPTION_KEY` is required to
boot but was `sync: false`, i.e. blank on provision — so a Blueprint deploy could only ever fail
until someone read the log. Changed to `generateValue: true`: Render mints it once at creation and
preserves it across deploys and syncs, and never overwrites a value you set yourself.

**4. `DEPLOY.md`'s "refuses to start" list was stale.** It named `APP_BASE_URL`, `ALLOWED_ORIGIN`,
the database path and `STRIPE_WEBHOOK_SECRET` — omitting the MySQL requirement and
`CREDENTIAL_ENCRYPTION_KEY`, i.e. precisely the two newest blockers and the ones actually firing.
Replaced with the complete blocker table, an exit-code table (78 / 75 / 70) and a note that
`sync: false` values are blank on a new service by design.

**5. `ConfigIssues()` disagreed with the preflight on `APP_BASE_URL`.** The preflight requires an
absolute non-loopback **https** URL; `ConfigIssues()` (which backs the owner-only
`/api/admin/system-check`) only rejected empty/localhost, so an `http://` base URL showed as
healthy in system-check while being the reason boot refused. Aligned to the same test.

**6. Compiled Python bytecode was tracked in git.**
`backend/tools/__pycache__/sqlite_to_mysql.cpython-311.pyc` was committed, so it appeared as a
spurious modification whenever the generator was imported. Untracked and added to `.gitignore`.

---

## Verification

Backend `dotnet build`/`publish -c Release`: 0 errors, 0 warnings · frontend `npm run build`
(typecheck + both bundles): clean · `production_config_test.py` **8/8** (5 existing + 3 new) ·
`migration_integrity_test.py` **13/13** · `sqlite_to_mysql.py --check`: current · the six Python
logic suites (lifecycle, release, casework, settings, publication, storage): all pass ·
`smoke-test.sh` **65/65** · boot matrix re-run for exits 78 / 75 and the all-satisfied path.

## Note on CI visibility

Every workflow run on `main` after run #612 is still `queued` — no runner has picked them up — so
neither the red schema check nor anything else was surfaced. Deploy failures were the only signal
reaching anyone. Worth confirming Actions runner availability/concurrency separately: with CI
stalled, `TreatWarningsAsErrors` and the schema-currency gate protect nothing.
