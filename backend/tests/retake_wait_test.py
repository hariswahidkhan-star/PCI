#!/usr/bin/env python3
"""
P0-7 regression suite — retake waiting period is persisted, ENFORCED at booking, and admin-waivable.

Boots the REAL built backend, makes a fully-eligible candidate, gives them a retake seat whose
authorization carries a future retake_wait_until (the state EnsureForPayment now persists after a
failed sitting — proven at the unit tier in ExamAuthorizationTests), then over real HTTP asserts:

  • booking is refused while the cool-off is active (error `retake_wait_active`, with the date);
  • an admin with `ex_waive_wait` can waive it through the real endpoint, clearing the date;
  • booking then succeeds.

Exit code 0 iff every assertion passes.  Run from backend/:
    python3 tests/retake_wait_test.py
"""
import hashlib, json, os, socket, sqlite3, subprocess, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_retake_wait.db")
STORAGE = os.path.join(HERE, "_retake_wait_storage")
TINY_PNG = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg=="

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port(); BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, token=None, body=None):
    hdr = {}; data = None
    if body is not None: data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    if token: hdr["Authorization"] = "Bearer " + token
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r) as resp: return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e: return e.code, e.read().decode()

def jget(method, path, **kw):
    c, t = req(method, path, **kw)
    try: return c, json.loads(t)
    except Exception: return c, t

def dbconn(): c = sqlite3.connect(DB, timeout=5); c.row_factory = sqlite3.Row; return c

def boot():
    for f in (DB, DB + "-wal", DB + "-shm"):
        try: os.remove(f)
        except OSError: pass
    env = dict(os.environ, DATABASE_FILE=DB, PORT=str(PORT), STORAGE_ROOT=STORAGE, ASPNETCORE_ENVIRONMENT="Development")
    log = open(os.path.join(HERE, "_retake_wait_boot.log"), "wb")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND, stdout=log, stderr=subprocess.STDOUT)
    deadline = time.time() + 120
    while time.time() < deadline:
        if proc.poll() is not None: raise SystemExit(f"server exited during boot (exit {proc.returncode})")
        try:
            if req("GET", "/api/health")[0] == 200: return proc
        except Exception: pass
        time.sleep(0.5)
    proc.terminate(); raise SystemExit("server did not boot within 120s")

def main():
    proc = boot()
    try:
        # owner (for the waiver) — clear the forced first-password change
        _, ob = jget("POST", "/api/admin/auth/login", body={"email": "owner@pci.local", "password": "changeme-owner"})
        admin = ob["token"]; jget("POST", "/api/admin/me/password", token=admin, body={"new_password": "OwnerPass99!"})

        # a fully-eligible candidate
        email = "retaker@example.com"; pw = "Passw0rd!23"
        _, r = jget("POST", "/api/register", body={"firstName": "Re", "lastName": "Taker", "email": email,
                    "password": pw, "confirmPassword": pw, "country": "United Kingdom"})
        student = r.get("token") if isinstance(r, dict) else None
        con = dbconn(); uid = con.execute("SELECT id FROM users WHERE email=?", (email,)).fetchone()["id"]; con.close()
        if not student:
            tok = "sess_" + hashlib.sha256(email.encode()).hexdigest()[:24]
            con = dbconn(); con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 day'))",
                                        (uid, hashlib.sha256(tok.encode()).hexdigest())); con.commit(); con.close()
            student = tok
        req("PATCH", "/api/me/profile", token=student, body={"country": "United Kingdom", "city": "London"})
        req("POST", "/api/me/consents", token=student, body={"accept_all": True})
        req("POST", "/api/me/identity-document", token=student, body={"doc_kind": "passport", "filename": "id.png", "data_uri": TINY_PNG})

        # give them a RETAKE seat: a paid exam entitlement + an authorization whose cool-off is still active.
        con = dbconn()
        payid = con.execute("INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,provider_payment_id,payment_status,payment_date,reference,exam_schedule_deadline) "
                            "VALUES(?, 'exam',500,500,'USD','stripe','pi_retake','paid', datetime('now'), 'RETAKE-1', datetime('now','+30 day'))", (uid,)).lastrowid
        con.execute("INSERT INTO exam_entitlements(user_id,payment_id,product_type,certification_id,status,valid_until) "
                    "VALUES(?,?, 'exam',1,'available', datetime('now','+30 day'))", (uid, payid))
        con.execute("INSERT INTO exam_authorizations(user_id,certification_id,payment_id,eligibility_start,original_deadline,current_deadline,attempts_permitted,attempts_used,window_days,window_source,retake_wait_until,status,created_at,updated_at) "
                    "VALUES(?,1,?, datetime('now'), datetime('now','+30 day'), datetime('now','+30 day'), 1,0,180,'test', datetime('now','+45 day'),'active', datetime('now'), datetime('now'))", (uid, payid))
        con.commit()
        authid = con.execute("SELECT id FROM exam_authorizations WHERE payment_id=?", (payid,)).fetchone()["id"]
        con.close()

        slot = time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime(time.time() + 3 * 3600))

        # 1) booking is BLOCKED while the retake cool-off is active
        c, j = jget("POST", "/api/me/exam/book", token=student, body={"scheduled_at": slot, "timezone": "UTC"})
        chk("1 booking refused during the retake waiting period",
            c == 400 and isinstance(j, dict) and j.get("error") == "retake_wait_active" and j.get("retake_wait_until"), (c, j))

        # 2) an admin waives it through the real permissioned endpoint
        c, w = jget("POST", f"/api/admin/exam-authorizations/{authid}/waive-waiting-period", token=admin, body={"reason": "goodwill — support ticket"})
        chk("2 admin waives the waiting period", c == 200 and isinstance(w, dict) and w.get("ok") is True, (c, w))
        con = dbconn(); cleared = con.execute("SELECT retake_wait_until FROM exam_authorizations WHERE id=?", (authid,)).fetchone()["retake_wait_until"]; con.close()
        chk("3 waiver clears the persisted cool-off date", cleared in (None, ""), cleared)

        # 3) booking now succeeds
        c, j = jget("POST", "/api/me/exam/book", token=student, body={"scheduled_at": slot, "timezone": "UTC"})
        chk("4 booking succeeds after the waiver", c == 200 and isinstance(j, dict) and j.get("ok") is True, (c, j))

        print(f"\n  == {passed}/{passed+failed} PASSED ==")
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()
    raise SystemExit(1 if failed else 0)

if __name__ == "__main__":
    main()
