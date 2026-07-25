#!/usr/bin/env python3
"""
P0-6 regression suite — schema migrations are versioned, checksummed, compatibility-gated and LOCKED.

Boots the REAL built backend against throwaway SQLite databases and asserts:

  • a fresh boot records exactly one schema_migrations row (version + non-empty checksum);
  • a second boot on the same DB is idempotent (still one row, same checksum);
  • an OLDER binary vs a NEWER schema is refused: with a future version pre-seeded, boot exits
    non-zero (EX_TEMPFAIL 75) and never serves — a failed/incompatible migration is never "ready";
  • TWO instances booting simultaneously on the same DB both come up cleanly and leave exactly one
    ledger row (the cross-instance migration lock prevents the DDL race / duplicate-key insert).

Exit code 0 iff every assertion passes.  Run from backend/:
    python3 tests/migration_versioning_test.py
"""
import json, os, socket, sqlite3, subprocess, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

def health(port):
    try:
        with urllib.request.urlopen(f"http://127.0.0.1:{port}/api/health", timeout=2) as r: return r.status == 200
    except Exception: return False

def clean(db):
    for f in (db, db + "-wal", db + "-shm", db + ".migrate.lock"):
        try: os.remove(f)
        except OSError: pass

def spawn(db, port):
    env = dict(os.environ, DATABASE_FILE=db, PORT=str(port),
               STORAGE_ROOT=os.path.join(HERE, "_migv_storage"), ASPNETCORE_ENVIRONMENT="Development")
    logf = open(os.path.join(HERE, f"_migv_boot_{port}.log"), "wb")
    return subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND, stdout=logf, stderr=subprocess.STDOUT)

def wait_health(proc, port, timeout=120):
    deadline = time.time() + timeout
    while time.time() < deadline:
        if proc.poll() is not None: return False
        if health(port): return True
        time.sleep(0.5)
    return False

def stop(proc):
    proc.terminate()
    try: proc.wait(timeout=10)
    except Exception: proc.kill()

def rows(db):
    con = sqlite3.connect(db, timeout=5); con.row_factory = sqlite3.Row
    r = con.execute("SELECT version,checksum FROM schema_migrations ORDER BY version").fetchall(); con.close()
    return r

def main():
    db = os.path.join(HERE, "_migv.db")

    # 1) fresh boot records exactly one ledger row
    clean(db)
    p = spawn(db, free_port_a := free_port())
    up = wait_health(p, free_port_a)
    chk("1 fresh boot becomes healthy", up)
    r = rows(db)
    chk("1b schema_migrations has exactly one row (version 1, non-empty checksum)",
        len(r) == 1 and r[0]["version"] == 1 and bool(r[0]["checksum"]), [dict(x) for x in r])
    checksum1 = r[0]["checksum"] if r else None
    stop(p)

    # 2) reboot on the same DB is idempotent (still one row, same checksum)
    p = spawn(db, free_port_b := free_port())
    chk("2 reboot on same DB becomes healthy", wait_health(p, free_port_b))
    r = rows(db)
    chk("2b still exactly one row, unchanged checksum", len(r) == 1 and r[0]["checksum"] == checksum1, [dict(x) for x in r])
    stop(p)

    # 3) older binary vs newer schema → refuse to start (exit 75), never serves
    con = sqlite3.connect(db, timeout=5)
    con.execute("INSERT INTO schema_migrations(version,checksum,description,applied_at) VALUES(999,'future','from a newer build', datetime('now'))")
    con.commit(); con.close()
    port_c = free_port()
    p = spawn(db, port_c)
    deadline = time.time() + 30; exited = None
    while time.time() < deadline:
        if p.poll() is not None: exited = p.returncode; break
        time.sleep(0.5)
    served = health(port_c)
    chk("3 incompatible (newer) schema refuses to start (exit 75)", exited == 75, f"exit={exited}")
    chk("3b never reports ready on an incompatible schema", served is False, served)
    if p.poll() is None: stop(p)

    # 4) two instances booting simultaneously → both healthy, exactly one ledger row (lock works)
    clean(db)
    pa, pb = free_port(), free_port()
    a = spawn(db, pa); b = spawn(db, pb)
    ok_a = wait_health(a, pa); ok_b = wait_health(b, pb)
    chk("4 both concurrent instances boot cleanly (migration lock serialized the DDL)", ok_a and ok_b, (ok_a, ok_b))
    r = rows(db)
    chk("4b concurrent boot left exactly one ledger row (no duplicate-key / DDL race)", len(r) == 1, [dict(x) for x in r])
    stop(a); stop(b)

    print(f"\n  == {passed}/{passed+failed} PASSED ==")
    raise SystemExit(1 if failed else 0)

if __name__ == "__main__":
    main()
