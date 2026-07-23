#!/usr/bin/env python3
"""
Migration / DB-integrity suite.

Boots the REAL built backend (same mechanism as integration_test.py's boot():
subprocess `dotnet PCI.Backend.dll` + /api/health poll + clean shutdown — NOT
imported, since that module runs code at import) against throwaway databases
and proves four properties of Data/Migrate.cs + schema.sql +
Data/CommsSeed.cs + Data/MarketingSchema.cs:

  1. Idempotence   — two boots against the SAME SQLite DATABASE_FILE leave
     sqlite_master (tables/indexes/normalised SQL) identical and do not
     duplicate seed rows (certifications, comm_sender_profiles, pages,
     pricing_rules counts stable; no duplicated seed keys).
  2. Conformance   — every table and column declared in schema.sql exists in
     the live migrated SQLite DB. Migrate's AddCol additions mean the live DB
     may carry MORE columns than schema.sql — that is fine; missing ones fail.
  3. Non-destruction — a sentinel site_settings row inserted BETWEEN boots
     survives the second migration with its value intact.
  4. Provider parity (TEST_DB_PROVIDER=mysql only) — boot once on MySQL and
     compare per-table column-NAME sets (information_schema.columns vs PRAGMA
     table_info) for every table on both providers. NAMES only: types
     legitimately differ (TEXT vs VARCHAR, INTEGER vs BIGINT — see
     Db.Translate).

Isolation: its own temp DATABASE_FILE + STORAGE_ROOT, and (in mysql mode) a
dedicated "<MYSQL_DATABASE>_migint" database — it never touches the repo's
ci.db, the integration harness's _integration.db, or the shared `pci` schema.

Exit code 0 iff every assertion passes.  Run from backend/:
    python3 tests/migration_integrity_test.py                        # SQLite-only (checks 1,2,3)
    TEST_DB_PROVIDER=mysql python3 tests/migration_integrity_test.py # full, incl. parity (needs pymysql)
"""
import os, re, secrets, socket, sqlite3, subprocess, sys, tempfile, time, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
SCHEMA_SQL = os.path.join(BACKEND, "schema.sql")

PROVIDER = os.environ.get("TEST_DB_PROVIDER", "sqlite").lower()   # "mysql" => full mode incl. parity
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"))
# Dedicated database for the parity boot so this suite can never trample the
# integration harness's `pci` database (which _reset_mysql drops and recreates).
MIG_DB = os.environ.get("MYSQL_DATABASE", "pci") + "_migint"
assert re.fullmatch(r"[A-Za-z0-9_]+", MIG_DB), f"unsafe MySQL database name: {MIG_DB!r}"

WORK = tempfile.mkdtemp(prefix="pci_migint_")   # unique per run; never the repo tree
DB = os.path.join(WORK, "migint.db")
STORAGE = os.path.join(WORK, "storage")

# Seeded tables whose row counts must be stable across a second boot. All four are
# seeded idempotently today: certifications (schema.sql INSERT OR IGNORE id 1 +
# code-keyed MultiCert.Seed), comm_sender_profiles (CommsSeed.Ensure, WHERE NOT
# EXISTS per `key`, 12 profiles), pages (schema.sql INSERT OR IGNORE per slug),
# pricing_rules (schema.sql WHERE NOT EXISTS per product_type, 3 rows).
SEED_COUNT_TABLES = ["certifications", "comm_sender_profiles", "pages", "pricing_rules"]

# Tables legitimately present on only ONE provider. Verified EMPTY today:
# schema.sql and schema.mysql.sql declare the identical 74-table set, and
# Migrate.cs / CommsSeed.cs / MarketingSchema.cs run the same (auto-translated)
# DDL on both providers. Add a name here — with a justification comment — only
# if a migration ever becomes deliberately provider-specific.
PROVIDER_SPECIFIC_TABLES = set()

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

# ---- server lifecycle (mirrors integration_test.py's boot()) ----
def boot(env, port, tag):
    log_path = os.path.join(WORK, f"boot_{tag}.log")
    logf = open(log_path, "wb")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND, stdout=logf, stderr=subprocess.STDOUT)
    def tail():
        try:
            logf.flush()
            with open(log_path, "r", errors="replace") as f: return f.read()[-4000:]
        except Exception: return "(no server log)"
    deadline = time.time() + 120     # CI cold-start headroom, same allowance as integration_test.py
    url = f"http://127.0.0.1:{port}/api/health"
    while time.time() < deadline:
        if proc.poll() is not None:  # crashed before serving health
            raise SystemExit(f"[{tag}] server exited during boot (exit {proc.returncode})\n--- log tail ---\n{tail()}")
        try:
            with urllib.request.urlopen(url, timeout=2) as r:
                if r.status == 200: return proc
        except Exception: pass
        time.sleep(0.5)
    proc.terminate()
    raise SystemExit(f"[{tag}] server did not boot within 120s\n--- log tail ---\n{tail()}")

def stop(proc):
    proc.terminate()
    try: proc.wait(timeout=10)
    except Exception: proc.kill()

def base_env():
    env = dict(os.environ, STORAGE_ROOT=STORAGE, ASPNETCORE_ENVIRONMENT="Development")
    # never let ambient provider config override this test's per-boot choices
    env.pop("MYSQL_CONNECTION_STRING", None)
    return env

def boot_sqlite(tag):
    port = free_port()
    env = base_env(); env.pop("DB_PROVIDER", None)          # force the SQLite provider
    env.update(DATABASE_FILE=DB, PORT=str(port))
    return boot(env, port, tag)

def boot_mysql(tag):
    port = free_port()
    env = base_env()
    env.update(DB_PROVIDER="mysql", PORT=str(port),
               MYSQL_HOST=MYSQL["host"], MYSQL_PORT=str(MYSQL["port"]),
               MYSQL_USER=MYSQL["user"], MYSQL_PASSWORD=MYSQL["password"],
               MYSQL_DATABASE=MIG_DB,
               DATABASE_FILE=os.path.join(WORK, "unused.db"))  # ignored on MySQL; set so /data auto-adopt can't fire
    env.setdefault("MYSQL_SSL", "false")
    return boot(env, port, tag)

# ---- SQLite inspection ----
def sqlite_schema_snapshot():
    """Set of (type, name, tbl_name, whitespace-normalised sql) for every user object."""
    con = sqlite3.connect(DB)
    rows = con.execute("SELECT type,name,tbl_name,COALESCE(sql,'') FROM sqlite_master"
                       " WHERE name NOT LIKE 'sqlite_%'").fetchall()
    con.close()
    return {(t, n, tn, re.sub(r"\s+", " ", s).strip()) for (t, n, tn, s) in rows}

def sqlite_counts(tables):
    con = sqlite3.connect(DB)
    out = {t: con.execute(f"SELECT COUNT(*) FROM {t}").fetchone()[0] for t in tables}
    con.close()
    return out

def sqlite_live_columns():
    con = sqlite3.connect(DB)
    tables = [r[0] for r in con.execute("SELECT name FROM sqlite_master WHERE type='table'"
                                        " AND name NOT LIKE 'sqlite_%'")]
    out = {}
    for t in tables:
        assert re.fullmatch(r"[A-Za-z0-9_]+", t), t
        out[t.lower()] = {r[1].lower() for r in con.execute(f"PRAGMA table_info({t})")}
    con.close()
    return out

def sqlite_dupes(table, keycol):
    con = sqlite3.connect(DB)
    rows = con.execute(f"SELECT {keycol}, COUNT(*) FROM {table} GROUP BY {keycol}"
                       " HAVING COUNT(*)>1").fetchall()
    con.close()
    return rows

# ---- schema.sql static parser (CREATE TABLE blocks only; quote- and comment-aware) ----
CONSTRAINT_STARTERS = {"primary", "unique", "check", "foreign", "constraint"}

def _balanced_body(text, lparen):
    """Comment-stripped text between the matching parens starting at text[lparen]=='('."""
    out, depth, in_str, i, n = [], 0, False, lparen, len(text)
    while i < n:
        c = text[i]
        if in_str:
            if c == "'" and text[i:i+2] == "''": out.append("''"); i += 2; continue
            if c == "'": in_str = False
            out.append(c); i += 1; continue
        if c == "'":
            in_str = True; out.append(c); i += 1; continue
        if text[i:i+2] == "--":                       # -- comment: skip to end of line
            while i < n and text[i] != "\n": i += 1
            continue
        if c == "(":
            depth += 1
            if depth == 1: i += 1; continue           # the outer paren is not part of the body
        elif c == ")":
            depth -= 1
            if depth == 0: return "".join(out)
        out.append(c); i += 1
    raise SystemExit("unbalanced parentheses while parsing schema.sql")

def _split_top_level(body):
    """Split a CREATE TABLE body on depth-0 commas (paren- and string-aware)."""
    parts, buf, depth, in_str = [], [], 0, False
    for c in body:
        if in_str:
            buf.append(c)
            if c == "'": in_str = False
            continue
        if c == "'": in_str = True; buf.append(c); continue
        if c == "(": depth += 1
        elif c == ")": depth -= 1
        if c == "," and depth == 0: parts.append("".join(buf)); buf = []
        else: buf.append(c)
    if buf: parts.append("".join(buf))
    return parts

def parse_schema_tables(path):
    """{table_name: {column_name,...}} declared by schema.sql's CREATE TABLE blocks."""
    text = open(path, encoding="utf-8").read().replace("\r\n", "\n")
    declared = {}
    for m in re.finditer(r"CREATE TABLE IF NOT EXISTS\s+[`\"\[]?([A-Za-z0-9_]+)[`\"\]]?\s*\(", text, re.I):
        cols = set()
        for part in _split_top_level(_balanced_body(text, m.end() - 1)):
            part = part.strip()
            if not part: continue
            tok = re.match(r'[`"\[]?([A-Za-z0-9_]+)', part)
            if not tok: continue
            name = tok.group(1).lower()
            if name in CONSTRAINT_STARTERS: continue  # table-level constraint, not a column
            cols.add(name)
        declared[m.group(1).lower()] = cols
    return declared

# ---- MySQL inspection (parity mode only) ----
def mysql_reset():
    import pymysql
    root = pymysql.connect(host=MYSQL["host"], port=MYSQL["port"], user=MYSQL["user"], password=MYSQL["password"])
    cur = root.cursor()
    cur.execute(f"DROP DATABASE IF EXISTS `{MIG_DB}`")
    cur.execute(f"CREATE DATABASE `{MIG_DB}` CHARACTER SET utf8mb4")
    root.commit(); root.close()

def mysql_live_columns():
    import pymysql
    con = pymysql.connect(database=MIG_DB, **MYSQL)
    cur = con.cursor()
    cur.execute("SELECT LOWER(table_name), LOWER(column_name) FROM information_schema.columns"
                " WHERE table_schema=%s", (MIG_DB,))
    out = {}
    for t, c in cur.fetchall(): out.setdefault(t, set()).add(c)
    con.close()
    return out

# ============================================================================
def main():
    if not os.path.exists(DLL):
        raise SystemExit(f"backend not built: {DLL} missing — run `dotnet build -c Release` first")
    os.makedirs(STORAGE, exist_ok=True)
    print(f"work dir: {WORK}   mode: {'full (SQLite + MySQL parity)' if PROVIDER == 'mysql' else 'sqlite-only'}")

    # ── 1. Idempotence: two boots against the SAME SQLite DATABASE_FILE ──
    print("\n=== 1. Idempotence (double boot, one SQLite DATABASE_FILE) ===")
    stop(boot_sqlite("sqlite_boot1"))
    schema1, counts1 = sqlite_schema_snapshot(), sqlite_counts(SEED_COUNT_TABLES)
    chk("1a first boot produced a populated schema and non-empty seeds " + str(counts1),
        len(schema1) > 100 and all(v > 0 for v in counts1.values()), counts1)

    # sentinel planted BETWEEN boots (used by check 3): re-migration must not destroy operator data
    sent_val = secrets.token_hex(16)
    con = sqlite3.connect(DB)
    con.execute("INSERT INTO site_settings(skey,svalue) VALUES('migration_integrity_sentinel',?)", (sent_val,))
    con.commit(); con.close()

    stop(boot_sqlite("sqlite_boot2"))
    schema2, counts2 = sqlite_schema_snapshot(), sqlite_counts(SEED_COUNT_TABLES)
    diff = schema1 ^ schema2
    chk("1b second boot leaves sqlite_master identical (tables/indexes/SQL — zero schema churn)",
        not diff, f"{len(diff)} differing entries, e.g. {sorted(e[1] for e in diff)[:5]}")
    chk("1c seed row counts unchanged by the second boot", counts2 == counts1, f"boot1={counts1} boot2={counts2}")
    dup_cert = sqlite_dupes("certifications", "code")
    dup_snd = sqlite_dupes("comm_sender_profiles", '"key"')
    dup_page = sqlite_dupes("pages", "slug")
    chk("1d no duplicated seed keys (certifications.code / comm_sender_profiles.key / pages.slug)",
        not (dup_cert or dup_snd or dup_page), (dup_cert, dup_snd, dup_page))

    # ── 2. schema.sql conformance ──
    print("\n=== 2. schema.sql conformance (every declared table/column exists live) ===")
    declared = parse_schema_tables(SCHEMA_SQL)
    live = sqlite_live_columns()
    chk(f"2a parser found a sane declaration set ({len(declared)} tables, expected >= 70)",
        len(declared) >= 70, len(declared))
    missing_tables = sorted(t for t in declared if t not in live)
    chk("2b every schema.sql table exists in the migrated DB", not missing_tables, missing_tables)
    missing_cols = {t: sorted(declared[t] - live[t]) for t in declared if t in live and declared[t] - live[t]}
    chk("2c every schema.sql column exists in its live table (AddCol extras allowed, missing = drift)",
        not missing_cols, missing_cols)

    # ── 3. No destructive re-migration ──
    print("\n=== 3. Non-destruction (sentinel inserted between boots survives boot 2) ===")
    con = sqlite3.connect(DB)
    row = con.execute("SELECT svalue FROM site_settings WHERE skey='migration_integrity_sentinel'").fetchone()
    con.close()
    chk("3a sentinel site_settings row survived re-migration with its value intact",
        row is not None and row[0] == sent_val, row)

    # ── 4. Provider parity (full mode only) ──
    if PROVIDER == "mysql":
        print("\n=== 4. Provider parity (information_schema.columns vs PRAGMA table_info) ===")
        mysql_reset()
        stop(boot_mysql("mysql_boot"))
        my = mysql_live_columns()
        lt = set(live) - PROVIDER_SPECIFIC_TABLES
        mt = set(my) - PROVIDER_SPECIFIC_TABLES
        chk("4a both providers created the same table set",
            lt == mt, f"sqlite-only: {sorted(lt - mt)}  mysql-only: {sorted(mt - lt)}")
        drift = {}
        for t in sorted(lt & mt):
            s_only, m_only = live[t] - my[t], my[t] - live[t]
            if s_only or m_only:
                drift[t] = True
                print(f"        drift {t}: sqlite-only={sorted(s_only)} mysql-only={sorted(m_only)}")
        chk(f"4b column-name parity across all {len(lt & mt)} shared tables",
            not drift, f"{len(drift)} tables drifted (detailed above)")
    else:
        print("\n=== 4. Provider parity — SKIP (set TEST_DB_PROVIDER=mysql to enable) ===")

    print(f"\n  ══ {passed}/{passed+failed} PASSED ══")
    sys.exit(0 if failed == 0 else 1)

if __name__ == "__main__":
    main()
