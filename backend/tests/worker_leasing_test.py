#!/usr/bin/env python3
"""
EXT-P0-05 regression suite — background workers claim work atomically (no double-processing).

Boots the REAL built backend and exercises the communications outbox worker (the same
WorkerLease claim/recover pattern is applied to the integration, marketing, content, Certuvo
and exam-delivery dispatchers) to prove:

  • the lease bookkeeping columns (lease_owner, lease_until) exist on every leased table;
  • a queued in-app message is delivered EXACTLY ONCE by the worker (claim → deliver);
  • two concurrent claimers cannot both take the same row — the atomic claim UPDATE affects one
    row for the winner and zero for the loser (exactly what WorkerLease.TryClaim issues);
  • a crashed lease (status='processing' past lease_until) is recovered and delivered.

Exit code 0 iff every assertion passes.  Run from backend/ (needs bin/Release/net8.0):
    python3 tests/worker_leasing_test.py
"""
import os, socket, sqlite3, subprocess, time, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_worker_lease.db")
STORAGE = os.path.join(HERE, "_worker_lease_storage")

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port(); BASE = f"http://127.0.0.1:{PORT}"

def health():
    try:
        with urllib.request.urlopen(f"{BASE}/api/health", timeout=2) as r: return r.status == 200
    except Exception: return False

def dbconn(): c = sqlite3.connect(DB, timeout=5); c.row_factory = sqlite3.Row; return c

# The exact atomic claim UPDATE that Core/WorkerLease.TryClaim issues (SQLite dialect).
CLAIM_SQL = ("UPDATE comm_outbox SET status='processing', lease_owner=?, lease_until=datetime('now','+5 minutes'), updated_at=datetime('now') "
             "WHERE id=? AND status IN ('queued','scheduled','retrying') AND (lease_until IS NULL OR lease_until<=datetime('now'))")
# The exact recovery UPDATE that Core/WorkerLease.RecoverExpired issues.
RECOVER_SQL = ("UPDATE comm_outbox SET status='retrying', lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') "
               "WHERE status='processing' AND lease_until IS NOT NULL AND lease_until<=datetime('now')")

def boot():
    for f in (DB, DB + "-wal", DB + "-shm", DB + ".migrate.lock"):
        try: os.remove(f)
        except OSError: pass
    env = dict(os.environ, DATABASE_FILE=DB, PORT=str(PORT), STORAGE_ROOT=STORAGE, ASPNETCORE_ENVIRONMENT="Development")
    log = open(os.path.join(HERE, "_worker_lease_boot.log"), "wb")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND, stdout=log, stderr=subprocess.STDOUT)
    deadline = time.time() + 120
    while time.time() < deadline:
        if proc.poll() is not None: raise SystemExit(f"server exited during boot (exit {proc.returncode})")
        if health(): return proc
        time.sleep(0.5)
    proc.terminate(); raise SystemExit("server did not boot within 120s")

def seed_user(con, email):
    return con.execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?, 'student','active')", (email, "W")).lastrowid

def wait_status(outbox_id, want, timeout=60):
    deadline = time.time() + timeout
    while time.time() < deadline:
        con = dbconn(); r = con.execute("SELECT status FROM comm_outbox WHERE id=?", (outbox_id,)).fetchone(); con.close()
        if r and r["status"] == want: return True
        time.sleep(1)
    return False

def notif_count(uid):
    con = dbconn(); n = con.execute("SELECT COUNT(*) c FROM notifications WHERE user_id=?", (uid,)).fetchone()["c"]; con.close(); return n

def main():
    proc = boot()
    try:
        # lease columns exist on every leased table
        con = dbconn()
        def cols(t): return {r["name"] for r in con.execute(f"PRAGMA table_info({t})").fetchall()}
        for t in ("comm_outbox", "integration_deliveries", "mkt_jobs", "content_jobs", "certuvo_accounts", "exam_delivery_orders"):
            c = cols(t)
            chk(f"lease columns present on {t}", "lease_owner" in c and "lease_until" in c, c)
        con.close()

        # 1) a queued in-app message is delivered EXACTLY ONCE
        con = dbconn()
        uid = seed_user(con, "once@example.com")
        oid = con.execute("INSERT INTO comm_outbox(channel,user_id,subject,body,status,max_attempts) VALUES('inapp',?,?,?, 'queued',5)",
                          (uid, "Hello", "Body once")).lastrowid
        con.commit(); con.close()
        chk("1 worker delivers the queued message (status → sent)", wait_status(oid, "sent"))
        chk("1b delivered exactly once (one notification)", notif_count(uid) == 1, notif_count(uid))

        # 2) atomic claim: first claimer wins (1 row), a concurrent claimer loses (0 rows)
        con = dbconn()
        uid2 = seed_user(con, "claim@example.com")
        # scheduled far in the future so the live worker ignores it — this isolates the claim-race check.
        cid = con.execute("INSERT INTO comm_outbox(channel,user_id,subject,body,status,max_attempts,scheduled_at) VALUES('inapp',?,?,?, 'queued',5, datetime('now','+1 day'))",
                          (uid2, "Race", "Body race")).lastrowid
        con.commit()
        first = con.execute(CLAIM_SQL, ("workerA", cid)).rowcount; con.commit()
        second = con.execute(CLAIM_SQL, ("workerB", cid)).rowcount; con.commit()
        chk("2 first concurrent claim wins exactly one row", first == 1, first)
        chk("2b second concurrent claim gets zero rows (already owned)", second == 0, second)
        # a crashed lease (past expiry) is recovered (→ retrying) and then claimable by another worker
        con.execute("UPDATE comm_outbox SET lease_until=datetime('now','-1 minute') WHERE id=?", (cid,)); con.commit()
        con.execute(RECOVER_SQL); con.commit()   # the live worker's RecoverExpired may already have run — either way the row is claimable now
        reclaimed = con.execute(CLAIM_SQL, ("workerC", cid)).rowcount; con.commit()
        chk("2c an expired lease is recovered and reclaimable by another worker", reclaimed == 1, reclaimed)
        # park the row so the live worker never delivers the race fixture
        con.execute("UPDATE comm_outbox SET status='cancelled', lease_owner=NULL, lease_until=NULL WHERE id=?", (cid,)); con.commit()
        con.close()

        # 3) a crashed lease on a real message is recovered and delivered by the worker
        con = dbconn()
        uid3 = seed_user(con, "reclaim@example.com")
        rid = con.execute("INSERT INTO comm_outbox(channel,user_id,subject,body,status,max_attempts,lease_owner,lease_until) "
                          "VALUES('inapp',?,?,?, 'processing',5,'dead-worker', datetime('now','-10 minute'))",
                          (uid3, "Reclaim", "Body reclaim")).lastrowid
        con.commit(); con.close()
        chk("3 crashed 'processing' lease is recovered and delivered", wait_status(rid, "sent"))
        chk("3b recovered message delivered exactly once", notif_count(uid3) == 1, notif_count(uid3))

        print(f"\n  == {passed}/{passed+failed} PASSED ==")
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()
    raise SystemExit(1 if failed else 0)

if __name__ == "__main__":
    main()
