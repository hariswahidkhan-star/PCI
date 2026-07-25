#!/usr/bin/env python3
"""DR-1 — proves the MySQL backup is genuinely restorable (a backup is only real once restored).

Boots the real backend against a DEDICATED MySQL database (never the harness's `pci`) so migrations
seed a real schema, plants a sentinel row + captures key table counts, runs the production backup
script (`tools/mysql_backup.sh`, --single-transaction gzip dump), restores the dump into a fresh
scratch database, and asserts the sentinel value + row counts survive with no duplicated seed rows.

Runs only when a MySQL/MariaDB is reachable (MYSQL_* env, same vars the app + backup script use);
skips cleanly on SQLite-only environments. Safe: it creates and drops its own `*_drrest*` databases
and never touches application data. See docs/testing/DR_RESTORE_RUNBOOK.md.
"""
import gzip as _gz, os, shutil, socket, subprocess, sys, tempfile, time, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
BACKUP_SH = os.path.join(BACKEND, "tools", "mysql_backup.sh")

MYSQL = dict(
    host=os.environ.get("MYSQL_HOST", "127.0.0.1"),
    port=os.environ.get("MYSQL_PORT", "3306"),
    user=os.environ.get("MYSQL_USER", "pci"),
    password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
)
SRC_DB = "pci_drrest"
SCRATCH_DB = "pci_drrest_scratch"

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

def mysql(sql, db=None):
    """Run a statement through the mysql CLI; return stdout (tab-separated, no header)."""
    args = ["mysql", f"-h{MYSQL['host']}", f"-P{MYSQL['port']}", f"-u{MYSQL['user']}", "-N", "-B"]
    if db: args.append(db)
    args += ["-e", sql]
    env = dict(os.environ, MYSQL_PWD=MYSQL["password"])
    return subprocess.run(args, env=env, capture_output=True, text=True, check=True).stdout.strip()

def mysql_available():
    if not shutil.which("mysql") or not shutil.which("mysqldump"):
        return False
    try:
        mysql("SELECT 1")
        return True
    except Exception:
        return False

def boot_backend_on(db):
    port = free_port()
    env = dict(os.environ, DB_PROVIDER="mysql", MYSQL_HOST=MYSQL["host"], MYSQL_PORT=MYSQL["port"],
               MYSQL_USER=MYSQL["user"], MYSQL_PASSWORD=MYSQL["password"], MYSQL_DATABASE=db,
               MYSQL_SSL="false", PORT=str(port), STORAGE_ROOT=tempfile.mkdtemp(prefix="drrest_store_"),
               ASPNETCORE_ENVIRONMENT="Development")
    logf = open(os.path.join(HERE, "_drrest_boot.log"), "wb")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND, stdout=logf, stderr=subprocess.STDOUT)
    deadline = time.time() + 120
    while time.time() < deadline:
        if proc.poll() is not None:
            raise SystemExit(f"backend exited during boot (code {proc.returncode})")
        try:
            with urllib.request.urlopen(f"http://127.0.0.1:{port}/api/health", timeout=2) as r:
                if r.status == 200:
                    return proc
        except Exception:
            pass
        time.sleep(0.5)
    proc.terminate()
    raise SystemExit("backend did not boot within 120s")

def main():
    if not mysql_available():
        print("backup_restore_test: MySQL not reachable — skipping (SQLite-only environment).")
        return 0
    if not os.path.exists(DLL):
        print(f"backup_restore_test: backend DLL missing ({DLL}) — build first; skipping.")
        return 0

    print("=== DR-1: MySQL backup → restore round-trip ===")
    # 1. Fresh source DB, migrated by the real backend.
    mysql(f"DROP DATABASE IF EXISTS {SRC_DB}")
    mysql(f"CREATE DATABASE {SRC_DB} CHARACTER SET utf8mb4")
    proc = boot_backend_on(SRC_DB)
    try:
        # 2. Plant a sentinel + capture key counts (post-migration, post-seed).
        sentinel = "drrest-" + str(free_port())
        mysql(f"INSERT INTO site_settings(skey,svalue) VALUES('dr_sentinel','{sentinel}') "
              f"ON DUPLICATE KEY UPDATE svalue='{sentinel}'", db=SRC_DB)
        def counts(db):
            return {t: int(mysql(f"SELECT COUNT(*) FROM {t}", db=db) or 0)
                    for t in ("users", "certifications", "pricing_rules", "site_settings")}
        src_counts = counts(SRC_DB)
        chk("source schema migrated (certifications seeded)", src_counts["certifications"] >= 1, src_counts)
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()

    # 3. Run the PRODUCTION backup script.
    outdir = tempfile.mkdtemp(prefix="drrest_backup_")
    env = dict(os.environ, MYSQL_HOST=MYSQL["host"], MYSQL_PORT=MYSQL["port"], MYSQL_USER=MYSQL["user"],
               MYSQL_PASSWORD=MYSQL["password"], MYSQL_DATABASE=SRC_DB, BACKUP_DIR=outdir)
    subprocess.run(["bash", BACKUP_SH], env=env, check=True, capture_output=True, text=True)
    dumps = [os.path.join(outdir, f) for f in os.listdir(outdir) if f.endswith(".sql.gz")]
    chk("backup produced a non-empty gzip dump", len(dumps) == 1 and os.path.getsize(dumps[0]) > 0,
        dumps)
    # The gzip must actually decompress to SQL (not a truncated/garbage file).
    with _gz.open(dumps[0], "rt", errors="replace") as fh:
        head = fh.read(4096)
    chk("dump decompresses to real SQL (CREATE TABLE present)", "CREATE TABLE" in head)

    # 4. Restore into a fresh scratch DB.
    mysql(f"DROP DATABASE IF EXISTS {SCRATCH_DB}")
    mysql(f"CREATE DATABASE {SCRATCH_DB} CHARACTER SET utf8mb4")
    with _gz.open(dumps[0], "rb") as fh:
        sql_bytes = fh.read()   # decompress in-process (a GzipFile's fileno is the raw compressed fd)
    restore = subprocess.run(
        ["mysql", f"-h{MYSQL['host']}", f"-P{MYSQL['port']}", f"-u{MYSQL['user']}", SCRATCH_DB],
        env=dict(os.environ, MYSQL_PWD=MYSQL["password"]), input=sql_bytes, capture_output=True)
    chk("restore into scratch DB succeeded", restore.returncode == 0,
        restore.stderr.decode("utf-8", "replace")[-400:])

    # 5. Prove the restore: sentinel survived, counts match, no duplicated seed rows.
    def counts2(db):
        return {t: int(mysql(f"SELECT COUNT(*) FROM {t}", db=db) or 0)
                for t in ("users", "certifications", "pricing_rules", "site_settings")}
    restored = counts2(SCRATCH_DB)
    restored_sentinel = mysql("SELECT svalue FROM site_settings WHERE skey='dr_sentinel'", db=SCRATCH_DB)
    chk("sentinel row restored byte-for-byte", restored_sentinel == sentinel, (restored_sentinel, sentinel))
    chk("row counts identical after restore", restored == src_counts, (restored, src_counts))
    dup = mysql("SELECT COUNT(*) FROM (SELECT skey FROM site_settings GROUP BY skey HAVING COUNT(*)>1) d",
                db=SCRATCH_DB)
    chk("no duplicated setting keys after restore", int(dup or 0) == 0, dup)

    # 6. Cleanup.
    mysql(f"DROP DATABASE IF EXISTS {SRC_DB}")
    mysql(f"DROP DATABASE IF EXISTS {SCRATCH_DB}")
    shutil.rmtree(outdir, ignore_errors=True)

    print(f"\n  ══ {passed}/{passed + failed} PASSED ══")
    return 1 if failed else 0

if __name__ == "__main__":
    sys.exit(main())
