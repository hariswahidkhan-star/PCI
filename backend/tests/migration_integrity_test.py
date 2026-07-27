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
import hashlib, os, re, secrets, socket, sqlite3, subprocess, sys, tempfile, time, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
SCHEMA_SQL = os.path.join(BACKEND, "schema.sql")

PROVIDER = os.environ.get("TEST_DB_PROVIDER", "sqlite").lower()
if PROVIDER not in {"sqlite", "mysql"}:
    raise SystemExit(f"unknown TEST_DB_PROVIDER={PROVIDER!r}")
if os.environ.get("REQUIRE_MYSQL_TEST") == "1" and PROVIDER != "mysql":
    raise SystemExit("REQUIRE_MYSQL_TEST=1 but TEST_DB_PROVIDER is not mysql")
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
    # Cold-start allowance. A first boot runs the whole migration AND the whole seed corpus —
    # ~93k translation entries, ~13.5k page blocks, ~200 simulation scenarios — and on MySQL every
    # one of those is a network round trip to a container on a shared CI disk. On a fast runner
    # that is ~30s; on a slow one it has been observed past 120s, which is a timeout, not a defect.
    # This is an infrastructure allowance and not the thing under test: nothing below asserts how
    # FAST a boot is, only that the schema it produces is correct and idempotent. The elapsed time
    # is printed on failure so a genuine slowdown is still visible rather than absorbed.
    started = time.time()
    budget = int(os.environ.get("MIGINT_BOOT_TIMEOUT", "300"))
    url = f"http://127.0.0.1:{port}/api/health"
    while time.time() - started < budget:
        if proc.poll() is not None:  # crashed before serving health
            raise SystemExit(f"[{tag}] server exited during boot (exit {proc.returncode})\n--- log tail ---\n{tail()}")
        try:
            with urllib.request.urlopen(url, timeout=2) as r:
                if r.status == 200:
                    print(f"  ....  [{tag}] booted in {time.time() - started:.0f}s", flush=True)
                    return proc
        except Exception: pass
        time.sleep(0.5)
    proc.terminate()
    raise SystemExit(f"[{tag}] server did not boot within {budget}s\n--- log tail ---\n{tail()}")

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


# ---- money-column and index integrity (section 5) ----
# schema.mysql.sql is GENERATED by tools/sqlite_to_mysql.py: money columns are emitted as DECIMAL(12,2)
# and long keyed columns get index prefix lengths. Sections 1-4 compare NAMES only, so this section
# asserts money column TYPES on the live MySQL database after Migrate.Run (including EnsureMoneyDecimals).
def mysql_column_types():
    """{table: {column: (data_type, precision, scale)}} — live MySQL numeric types."""
    import pymysql
    con = pymysql.connect(database=MIG_DB, **MYSQL)
    cur = con.cursor()
    cur.execute("SELECT LOWER(table_name), LOWER(column_name), LOWER(data_type), numeric_precision, numeric_scale"
                " FROM information_schema.columns WHERE table_schema=%s", (MIG_DB,))
    out = {}
    for t, c, d, precision, scale in cur.fetchall():
        out.setdefault(t, {})[c] = (d, precision, scale)
    con.close()
    return out

def mysql_unique_indexes():
    """{table: {index_name: (col1, col2, ...)}} for UNIQUE indexes only."""
    import pymysql
    con = pymysql.connect(database=MIG_DB, **MYSQL)
    cur = con.cursor()
    cur.execute("SELECT LOWER(table_name), LOWER(index_name), LOWER(column_name), seq_in_index"
                " FROM information_schema.statistics WHERE table_schema=%s AND non_unique=0"
                " ORDER BY table_name, index_name, seq_in_index", (MIG_DB,))
    tmp = {}
    for t, idx, col, _ in cur.fetchall(): tmp.setdefault(t, {}).setdefault(idx, []).append(col)
    con.close()
    return {t: {i: tuple(c) for i, c in idxs.items()} for t, idxs in tmp.items()}

# Inexact binary floating point can never hold money: 0.1 + 0.2 != 0.3, and a cent lost per row is a
# reconciliation failure that no amount of downstream rounding recovers.
INEXACT = {"double", "float", "real"}

# Explicit production-money contract. Name heuristics missed salaries, ad budgets, spend and CPC,
# and could pass when a required column was absent or changed to VARCHAR/BIGINT.
MONEY_COLUMNS = {
    ("pricing_rules", "standard_price"): (12, 2),
    ("discount_codes", "discount_value"): (12, 2),
    ("discount_codes", "min_payable"): (12, 2),
    ("discount_codes", "min_transaction"): (12, 2),
    ("discount_codes", "max_discount"): (12, 2),
    ("payments", "standard_amount"): (12, 2),
    ("payments", "default_discount_amount"): (12, 2),
    ("payments", "discount_code_amount"): (12, 2),
    ("payments", "final_amount"): (12, 2),
    ("payments", "waived_amount"): (12, 2),
    ("payments", "amount_refunded"): (12, 2),
    ("memberships", "renewal_fee"): (12, 2),
    ("memberships", "amount_paid"): (12, 2),
    ("certifications", "exam_price"): (12, 2),
    ("certifications", "application_fee"): (12, 2),
    ("code_redemptions", "amount_before"): (12, 2),
    ("code_redemptions", "discount_amount"): (12, 2),
    ("partner_payouts", "amount"): (12, 2),
    ("fee_waivers", "original_amount"): (12, 2),
    ("fee_waivers", "waived_amount"): (12, 2),
    ("fee_waivers", "final_amount"): (12, 2),
    ("fee_waivers", "payable_amount"): (12, 2),
    ("analytics_events", "value"): (12, 2),
    ("certification_routes", "fee_amount"): (12, 2),
    ("job_postings", "salary_min"): (12, 2),
    ("job_postings", "salary_max"): (12, 2),
    ("mkt_campaigns", "total_budget"): (12, 2),
    ("mkt_campaigns", "alloc_linkedin"): (12, 2),
    ("mkt_campaigns", "alloc_google"): (12, 2),
    ("mkt_campaigns", "alloc_meta"): (12, 2),
    ("mkt_platform_campaigns", "daily_budget"): (12, 2),
    ("mkt_platform_campaigns", "lifetime_budget"): (12, 2),
    ("mkt_conversation_ads", "daily_budget"): (12, 2),
    ("mkt_conversation_ads", "lifetime_budget"): (12, 2),
    ("mkt_promotions", "original_amount"): (12, 2),
    ("mkt_promotions", "discount_amount"): (12, 2),
    ("mkt_promotions", "net_amount"): (12, 2),
    ("mkt_conversions", "value"): (12, 2),
    ("mkt_conversion_events", "value"): (12, 2),
    ("mkt_budget_approvals", "requested_amount"): (12, 2),
    ("mkt_keywords", "max_cpc"): (18, 6),
    ("mkt_campaign_metrics", "spend"): (18, 6),
    ("mkt_campaign_metrics", "conversion_value"): (18, 6),
}

# The finance module stores money as INTEGER MINOR UNITS. Any *_minor column that is not an integer type
# means the design was silently undone somewhere between the DDL and the live table.
INTEGER_TYPES = {"bigint", "int", "integer", "smallint", "mediumint"}

# Idempotency and the no-double-payment guarantee are enforced by these UNIQUE indexes, not by
# application checks alone. Db.Translate STRIPS the WHERE clause from a partial unique index, which would
# silently widen a constraint, so their presence is asserted explicitly rather than assumed.
# The finance module's own tables. Its money is integer minor units by design, so anything inexact here
# is a regression introduced by a change under review — not inherited platform history.
FINANCE_TABLES = {
    "partner_agreements", "partner_commission_rules", "partner_commission_transactions",
    "partner_commission_events", "partner_settlements", "partner_settlement_items",
    "partner_disputes", "partner_dispute_messages", "partner_campaign_links", "partner_link_clicks",
}

REQUIRED_UNIQUE = {
    "partner_commission_transactions": ("dedupe_key",),
    "partner_settlement_items":        ("settlement_id", "transaction_id"),
    "partner_settlements":             ("settlement_no",),
    "partner_agreements":              ("agreement_number",),
    "partner_campaign_links":          ("token",),
    "partner_disputes":                ("dispute_no",),
}
def sqlite_live_indexes():
    """Index names per table on SQLite, excluding the implicit UNIQUE autoindexes."""
    con = sqlite3.connect(DB)
    out = {}
    for (t,) in con.execute("SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'"):
        names = {r[1].lower() for r in con.execute(f'PRAGMA index_list("{t}")')
                 if not str(r[1]).startswith("sqlite_autoindex")}
        if names: out[t.lower()] = names
    con.close()
    return out


def mysql_live_indexes():
    import pymysql
    con = pymysql.connect(database=MIG_DB, **MYSQL)
    cur = con.cursor()
    cur.execute("SELECT LOWER(table_name), LOWER(index_name) FROM information_schema.statistics"
                " WHERE table_schema=%s AND index_name<>'PRIMARY'", (MIG_DB,))
    out = {}
    for t, i in cur.fetchall(): out.setdefault(t, set()).add(i)
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

    # Plant a sentinel and simulate the exact production state immediately before the PDL-AI → PML-AI
    # deployment. Boot 2 must update the existing certification row in place, not replace it, so every
    # relationship keyed to certification_id=3 remains intact.
    sent_val = secrets.token_hex(16)
    con = sqlite3.connect(DB)
    con.execute("INSERT INTO site_settings(skey,svalue) VALUES('migration_integrity_sentinel',?)", (sent_val,))
    legacy_route_ids = tuple(r[0] for r in con.execute(
        "SELECT id FROM certification_routes WHERE certification_id=3 ORDER BY id"))
    legacy_bok_id = con.execute(
        "SELECT id FROM cert_documents WHERE certification_id=3 AND kind='bok' ORDER BY id LIMIT 1").fetchone()[0]
    con.execute("UPDATE cert_documents SET filename='pdl-ai-bok.pdf', storage_ref='books/legacy-pdl-master.pdf' WHERE id=?",
                (legacy_bok_id,))
    con.execute("""INSERT INTO issued_credentials
        (credential_id,holder_name,credential,certification_id,status)
        VALUES('PCI-PDLAI-LEGACY-0001','Legacy Holder','PDL-AI',3,'active')""")
    legacy_credential_row = con.execute(
        "SELECT id FROM issued_credentials WHERE credential_id='PCI-PDLAI-LEGACY-0001'").fetchone()[0]
    con.execute("""UPDATE certifications SET
        code='PDL-AI', name='PCI AI Project Delivery Leader', public_title='PCI AI Project Delivery Leader',
        acronym='PCI PDL-AI', short_name='PDL-AI', slug='pdl-ai', credential_prefix='PDL-AI',
        category='Project Delivery'
        WHERE id=3""")
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
    con = sqlite3.connect(DB)
    migrated = con.execute("""SELECT id,code,name,slug,credential_prefix,category
        FROM certifications WHERE id=3""").fetchone()
    route_ids_after = tuple(r[0] for r in con.execute(
        "SELECT id FROM certification_routes WHERE certification_id=3 ORDER BY id"))
    cert3_count = con.execute("SELECT COUNT(*) FROM certifications WHERE id=3 OR code IN ('PDL-AI','PML-AI')").fetchone()[0]
    bok_after = con.execute("SELECT id,filename,storage_ref,sha256 FROM cert_documents WHERE id=?", (legacy_bok_id,)).fetchone()
    legacy_credential_after = con.execute("""SELECT id,credential_id,credential,certification_id,status
        FROM issued_credentials WHERE id=?""", (legacy_credential_row,)).fetchone()
    con.close()
    chk("1e PDL-AI is migrated in place to the exact PML-AI identity",
        migrated == (3, "PML-AI", "PCI Project Management Leader – AI", "pml-ai", "PML-AI", "Project Management"), migrated)
    chk("1f certification_id=3 route relationships survive the rename without duplication",
        bool(legacy_route_ids) and route_ids_after == legacy_route_ids and cert3_count == 1,
        (legacy_route_ids, route_ids_after, cert3_count))
    expected_bok_sha = hashlib.sha256(open(os.path.join(BACKEND, "books", "pml-ai-bok.pdf"), "rb").read()).hexdigest()
    chk("1f·1 the known bundled PDL-AI book master upgrades in place to the PML-AI PDF",
        bok_after is not None and bok_after[0] == legacy_bok_id and bok_after[1] == "pml-ai-bok.pdf"
        and bok_after[2] != "books/legacy-pdl-master.pdf" and bok_after[3] == expected_bok_sha,
        bok_after)
    chk("1f·2 an already-issued PDL-AI credential keeps its immutable public ID and certification link",
        legacy_credential_after == (legacy_credential_row, "PCI-PDLAI-LEGACY-0001", "PDL-AI", 3, "active"),
        legacy_credential_after)

    # An even older deployment briefly used the PML-AI code with a pre-final display identity. Because
    # the machine code already matches the final code, this exercises the guarded identity repair path.
    con = sqlite3.connect(DB)
    con.execute("""UPDATE certifications SET name='PCI Project Management Leader',
        public_title='PCI Project Management Leader', category='Project Delivery' WHERE id=3""")
    con.commit(); con.close()
    stop(boot_sqlite("sqlite_boot3_pre_pdl_alias"))
    con = sqlite3.connect(DB)
    alias_repaired = con.execute("""SELECT id,code,name,public_title,slug,credential_prefix,category
        FROM certifications WHERE id=3""").fetchone()
    route_ids_boot3 = tuple(r[0] for r in con.execute(
        "SELECT id FROM certification_routes WHERE certification_id=3 ORDER BY id"))
    con.close()
    chk("1g pre-PDL PML-AI alias state also converges once without breaking relationships",
        alias_repaired == (3, "PML-AI", "PCI Project Management Leader – AI", "PCI Project Management Leader – AI",
                           "pml-ai", "PML-AI", "Project Management")
        and route_ids_boot3 == legacy_route_ids,
        (alias_repaired, route_ids_boot3))

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
        stop(boot_mysql("mysql_boot1"))

        # Exercise the real legacy-upgrade path, not only a clean schema that is already DECIMAL.
        # A sentinel and exact value must survive a second MySQL/MariaDB boot.
        import pymysql
        con = pymysql.connect(database=MIG_DB, **MYSQL)
        cur = con.cursor()
        cur.execute("INSERT INTO site_settings(skey,svalue) VALUES(%s,%s)",
                    ("mysql_migration_integrity_sentinel", sent_val))
        cur.execute("ALTER TABLE payments MODIFY COLUMN final_amount DOUBLE NULL")
        cur.execute("""INSERT INTO payments(product_type,final_amount,payment_provider,
                    provider_payment_id,payment_status,reference)
                    VALUES('membership',45.12,'test','migint-legacy-money','paid','MIGINT-MONEY')""")
        cur.execute("SELECT COUNT(*) FROM certifications")
        mysql_cert_count1 = cur.fetchone()[0]
        con.commit(); con.close()

        stop(boot_mysql("mysql_boot2"))
        con = pymysql.connect(database=MIG_DB, **MYSQL)
        cur = con.cursor()
        cur.execute("SELECT svalue FROM site_settings WHERE skey='mysql_migration_integrity_sentinel'")
        mysql_sentinel = cur.fetchone()
        cur.execute("SELECT final_amount FROM payments WHERE provider_payment_id='migint-legacy-money'")
        legacy_money = cur.fetchone()
        cur.execute("SELECT COUNT(*) FROM certifications")
        mysql_cert_count2 = cur.fetchone()[0]
        con.close()
        chk("4a second MySQL boot preserves sentinel, seed counts and legacy money value",
            mysql_sentinel == (sent_val,) and mysql_cert_count2 == mysql_cert_count1
            and legacy_money is not None and str(legacy_money[0]) == "45.12",
            (mysql_sentinel, mysql_cert_count1, mysql_cert_count2, legacy_money))

        my = mysql_live_columns()
        lt = set(live) - PROVIDER_SPECIFIC_TABLES
        mt = set(my) - PROVIDER_SPECIFIC_TABLES
        chk("4b both providers created the same table set",
            lt == mt, f"sqlite-only: {sorted(lt - mt)}  mysql-only: {sorted(mt - lt)}")
        drift = {}
        for t in sorted(lt & mt):
            s_only, m_only = live[t] - my[t], my[t] - live[t]
            if s_only or m_only:
                drift[t] = True
                print(f"        drift {t}: sqlite-only={sorted(s_only)} mysql-only={sorted(m_only)}")
        chk(f"4c column-name parity across all {len(lt & mt)} shared tables",
            not drift, f"{len(drift)} tables drifted (detailed above)")

        # 4c. An index that SQLite builds and MySQL does not is invisible in normal operation: the
        # installers wrap index creation in catch-and-log, so the statement is skipped, the rest of
        # that installer may be abandoned, and the only symptom is a slow query — or, as with the
        # pciworld editorial tables, seven tables that never got created. Comparing what each
        # provider actually built needs no model of MySQL's key-length rules.
        s_ix, m_ix = sqlite_live_indexes(), mysql_live_indexes()
        ix_missing = {}
        for t in sorted(set(s_ix) & (set(m_ix) | (lt & mt))):
            gone = s_ix.get(t, set()) - m_ix.get(t, set())
            if gone:
                ix_missing[t] = sorted(gone)
                print(f"        missing on mysql {t}: {sorted(gone)}")
        chk("4d every index SQLite builds also exists on MySQL",
            not ix_missing, f"{len(ix_missing)} tables missing indexes on mysql (detailed above)")

        # ── 5. Money-column and index integrity (the gap sections 1-4 cannot see) ──
        print("\n=== 5. Money columns are exact, and the UNIQUE guarantees exist ===")
        types = mysql_column_types()

        # 5a. GATING, scoped to the finance module: its money is INTEGER MINOR UNITS by design, so an
        # inexact type there is a regression introduced today, not inherited history.
        inexact_finance = [f"{t}.{c} is {dt.upper()}"
                           for t in sorted(FINANCE_TABLES & set(types))
                           for c, (dt, _p, _s) in sorted(types[t].items())
                           if dt in INEXACT]
        chk("5a no finance-module column is stored as inexact floating point",
            not inexact_finance, "; ".join(inexact_finance[:8]))

        # 5b. GATING: every *_minor column is an integer type. These exist only in the finance tables, so
        # this proves the minor-unit design survived the DDL translation on MySQL, not just on SQLite.
        wrong_minor = [f"{t}.{c} is {dt.upper()}"
                       for t, cols in sorted(types.items())
                       for c, (dt, _p, _s) in sorted(cols.items())
                       if c.endswith("_minor") and dt not in INTEGER_TYPES]
        chk("5b every *_minor column is an integer type (minor units, not a float)",
            not wrong_minor, "; ".join(wrong_minor[:8]))

        # 5c. GATING: the finance UNIQUE indexes exist with exactly the columns their guarantees need.
        # Idempotent commission creation and the no-double-payment rule rest on these, not on app checks
        # alone — and Db.Translate STRIPS the WHERE from a partial unique index, silently widening it.
        uniques = mysql_unique_indexes()
        missing_unique = []
        for table, cols in sorted(REQUIRED_UNIQUE.items()):
            if table not in types:
                missing_unique.append(f"{table} (table absent)")
                continue
            have = uniques.get(table, {})
            if not any(idx_cols == cols for idx_cols in have.values()):
                missing_unique.append(f"{table} needs UNIQUE{cols}, has {sorted(have.values())}")
        chk("5c every finance UNIQUE index exists with exactly its declared columns",
            not missing_unique, "; ".join(missing_unique[:6]))

        # 5d. GATING: no finance UNIQUE index is a strict subset of the guarantee it stands in for — that
        # would be a constraint quietly narrower (and therefore stricter in the wrong place) than intended.
        over_broad = []
        for table, cols in sorted(REQUIRED_UNIQUE.items()):
            for idx, idx_cols in sorted(uniques.get(table, {}).items()):
                if idx != "primary" and set(idx_cols) < set(cols):
                    over_broad.append(f"{table}.{idx}={idx_cols}")
        chk("5d no finance UNIQUE index is narrower than the guarantee it stands in for",
            not over_broad, "; ".join(over_broad[:6]))

        # 5e. GATING (Phase 1 deep pass): every expected money column must exist and match its exact
        # table-qualified DECIMAL contract. This rejects missing, integer/string, wrong precision/scale,
        # and floating columns rather than relying on suffix inference.
        money_type_errors = []
        for (table, column), (precision, scale) in sorted(MONEY_COLUMNS.items()):
            actual = types.get(table, {}).get(column)
            if actual is None:
                money_type_errors.append(f"{table}.{column} missing")
            elif actual != ("decimal", precision, scale):
                money_type_errors.append(
                    f"{table}.{column}={actual}, expected DECIMAL({precision},{scale})")
        chk("5e every platform money column matches its explicit DECIMAL contract",
            not money_type_errors, "; ".join(money_type_errors[:12]))

        # 5g. Static gate on the generated schema file itself (catches regenerate regressions before boot).
        mysql_schema = open(os.path.join(BACKEND, "schema.mysql.sql"), encoding="utf-8").read()
        static_bad = []
        for col in sorted({column for _, column in MONEY_COLUMNS}):
            if re.search(rf"\b{re.escape(col)}\s+DOUBLE\b", mysql_schema, re.I):
                static_bad.append(col)
        chk("5f schema.mysql.sql base money columns are not DOUBLE",
            not static_bad, ", ".join(static_bad[:12]))

    else:
        print("\n=== 4. Provider parity — SKIP (set TEST_DB_PROVIDER=mysql to enable) ===")

    print(f"\n  ══ {passed}/{passed+failed} PASSED ══")
    sys.exit(0 if failed == 0 else 1)

if __name__ == "__main__":
    main()
