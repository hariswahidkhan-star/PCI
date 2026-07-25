#!/usr/bin/env python3
"""
RES-026 regression suite — fee-waiver grants carry a durable client idempotency key.

Boots the REAL built backend against a throwaway SQLite DB and proves that every admin
waiver path is safe to retry across network/process boundaries:

  • POST /api/admin/students/{id}/waive (full):    a replay with the same idempotency_key
    returns the ORIGINAL payment id with replay:true — no second settlement, ledger row,
    or membership grant;
  • POST /api/admin/students/{id}/waive (partial): a replay returns the ORIGINAL student-
    locked discount code — no second single-use code (doubled discount capacity);
  • POST /api/admin/exam-fee-waiver (retake):      a replay grants no second $0 seat and
    no second ledger row;
  • an idempotency key can never bridge students — reuse for another subject is refused
    with 409 idempotency_key_conflict;
  • keyless requests keep their historical behaviour (no forced contract change).

Exit code 0 iff every assertion passes.  Run from backend/ (needs bin/Release/net8.0):
    python3 tests/waiver_idempotency_test.py
"""
import json, os, socket, sqlite3, subprocess, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_waiver_idem.db")
STORAGE = os.path.join(HERE, "_waiver_idem_storage")

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port()
BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, token=None, body=None):
    hdr = {}
    data = None
    if body is not None:
        data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    if token: hdr["Authorization"] = "Bearer " + token
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r) as resp:
            return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e:
        return e.code, e.read().decode()

def jget(method, path, **kw):
    code, txt = req(method, path, **kw)
    try: return code, json.loads(txt)
    except Exception: return code, txt

def dbconn():
    c = sqlite3.connect(DB, timeout=5); c.row_factory = sqlite3.Row; return c

def count(sql, args=()):
    con = dbconn(); n = con.execute(sql, args).fetchone()[0]; con.close(); return n

def boot():
    for f in (DB, DB + "-wal", DB + "-shm", DB + ".migrate.lock"):
        try: os.remove(f)
        except OSError: pass
    env = dict(os.environ, DATABASE_FILE=DB, PORT=str(PORT), STORAGE_ROOT=STORAGE,
               ASPNETCORE_ENVIRONMENT="Development")
    log = open(os.path.join(HERE, "_waiver_idem_boot.log"), "wb")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND, stdout=log, stderr=subprocess.STDOUT)
    deadline = time.time() + 120
    while time.time() < deadline:
        if proc.poll() is not None:
            raise SystemExit(f"server exited during boot (exit {proc.returncode})")
        try:
            if req("GET", "/api/health")[0] == 200: return proc
        except Exception: pass
        time.sleep(0.5)
    proc.terminate(); raise SystemExit("server did not boot within 120s")

def register(email):
    pw = "Passw0rd!23"
    jget("POST", "/api/register", body={"firstName": "Waiver", "lastName": "Subject",
         "email": email, "password": pw, "confirmPassword": pw, "country": "United Kingdom"})
    con = dbconn(); row = con.execute("SELECT id FROM users WHERE email=?", (email,)).fetchone(); con.close()
    return row["id"] if row else None

def main():
    proc = boot()
    try:
        # ---- owner login (+ forced first password change) ----
        c, b = jget("POST", "/api/admin/auth/login", body={"email": "owner@pci.local", "password": "changeme-owner"})
        admin = b["token"]
        jget("POST", "/api/admin/me/password", token=admin, body={"new_password": "OwnerPass99!"})

        uid = register("waiver.idem@example.com")
        chk("setup: student account created", uid is not None)

        # ===== A. full membership waiver — replay is a no-op =====
        k1 = "wvr-test-full-1"
        c, r1 = jget("POST", f"/api/admin/students/{uid}/waive", token=admin,
                     body={"product": "membership", "percent": 100, "reason": "scholarship", "idempotency_key": k1})
        chk("A1 full waiver settles (200, kind=full)", c == 200 and r1.get("kind") == "full", (c, r1))
        pay1 = r1.get("payment_id")
        c, r2 = jget("POST", f"/api/admin/students/{uid}/waive", token=admin,
                     body={"product": "membership", "percent": 100, "reason": "scholarship", "idempotency_key": k1})
        chk("A2 replay returns the ORIGINAL payment with replay:true",
            c == 200 and r2.get("replay") is True and r2.get("payment_id") == pay1, (c, r2))
        chk("A3 exactly one waived settlement", count(
            "SELECT COUNT(*) FROM payments WHERE user_id=? AND product_type='membership' AND payment_status='waived'", (uid,)) == 1)
        chk("A4 exactly one ledger row for the key", count(
            "SELECT COUNT(*) FROM fee_waivers WHERE idempotency_key=?", (k1,)) == 1)
        chk("A5 exactly one membership", count("SELECT COUNT(*) FROM memberships WHERE user_id=?", (uid,)) == 1)

        # ===== B. partial exam waiver — replay returns the SAME code =====
        k2 = "wvr-test-partial-1"
        c, p1 = jget("POST", f"/api/admin/students/{uid}/waive", token=admin,
                     body={"product": "exam", "percent": 50, "reason": "hardship", "idempotency_key": k2})
        chk("B1 partial waiver creates a student-locked code", c == 200 and p1.get("kind") == "partial" and p1.get("code"), (c, p1))
        c, p2 = jget("POST", f"/api/admin/students/{uid}/waive", token=admin,
                     body={"product": "exam", "percent": 50, "reason": "hardship", "idempotency_key": k2})
        chk("B2 replay returns the SAME code with replay:true",
            c == 200 and p2.get("replay") is True and p2.get("code") == p1.get("code"), (c, p1.get("code"), p2))
        chk("B3 exactly one live waiver code (no doubled discount capacity)", count(
            "SELECT COUNT(*) FROM discount_codes WHERE code_type='waiver' AND active=1") == 1)
        chk("B4 exactly one partial ledger row for the key", count(
            "SELECT COUNT(*) FROM fee_waivers WHERE idempotency_key=?", (k2,)) == 1)

        # ===== C. exam-fee-waiver (retake) — replay grants no second $0 seat =====
        k3 = "wvr-test-retake-1"
        c, e1 = jget("POST", "/api/admin/exam-fee-waiver", token=admin,
                     body={"user_id": uid, "certification_id": "1", "fee_type": "retake", "percent": 100,
                           "reason": "technical incident", "idempotency_key": k3})
        chk("C1 full retake waiver settles (skips checkout)", c == 200 and e1.get("skips_checkout") is True, (c, e1))
        seats = count("SELECT COUNT(*) FROM payments WHERE user_id=? AND product_type IN ('exam','bundle')", (uid,))
        c, e2 = jget("POST", "/api/admin/exam-fee-waiver", token=admin,
                     body={"user_id": uid, "certification_id": "1", "fee_type": "retake", "percent": 100,
                           "reason": "technical incident", "idempotency_key": k3})
        chk("C2 replay is flagged and grants nothing new",
            c == 200 and e2.get("replay") is True, (c, e2))
        chk("C3 no second $0 seat settled on replay", count(
            "SELECT COUNT(*) FROM payments WHERE user_id=? AND product_type IN ('exam','bundle')", (uid,)) == seats)
        chk("C4 exactly one ledger row for the key", count(
            "SELECT COUNT(*) FROM fee_waivers WHERE idempotency_key=?", (k3,)) == 1)

        # ===== D. a key can never bridge students =====
        uid2 = register("waiver.other@example.com")
        c, x = jget("POST", f"/api/admin/students/{uid2}/waive", token=admin,
                    body={"product": "membership", "percent": 100, "reason": "scholarship", "idempotency_key": k1})
        chk("D1 key reuse for another student is refused (409 idempotency_key_conflict)",
            c == 409 and isinstance(x, dict) and x.get("error") == "idempotency_key_conflict", (c, x))
        chk("D2 no settlement leaked to the other student", count(
            "SELECT COUNT(*) FROM payments WHERE user_id=?", (uid2,)) == 0)

        # ===== E. keyless requests keep their historical behaviour =====
        c, y = jget("POST", f"/api/admin/students/{uid2}/waive", token=admin,
                    body={"product": "membership", "percent": 100, "reason": "scholarship"})
        chk("E1 keyless full waiver still settles", c == 200 and y.get("kind") == "full", (c, y))
        chk("E2 oversized key refused (400)", jget("POST", f"/api/admin/students/{uid2}/waive", token=admin,
            body={"product": "exam", "percent": 50, "reason": "x", "idempotency_key": "k" * 81})[0] == 400)

        print(f"\n  == {passed}/{passed+failed} PASSED ==")
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()
    raise SystemExit(1 if failed else 0)

if __name__ == "__main__":
    main()
