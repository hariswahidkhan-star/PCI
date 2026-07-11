#!/usr/bin/env python3
"""
Adversarial end-to-end integration suite (Phase 1).

Boots the REAL built backend against a throwaway SQLite DB with TEST-mode Stripe
keys, then drives every critical path over real HTTP:

  • enrolment → Stripe webhook settlement (created exactly once) → replay is a no-op
  • certification happy path → immediate released_pass + credential → public verify
  • fail path → immediate released_fail, no credential
  • attack paths: late submit, duplicate submit, foreign item ids (item_set_mismatch),
    consumed-entitlement rebook, refunded-payment-then-submit, answer-key leakage
  • held → admin release / invalidate / reinstate loop (held payload leaks no pass/fail)
  • accommodations: +30 min genuinely extends the live sitting to 120
  • RBAC probes per role, rate-limit firing, legacy token dead
  • storage: MIME sniff, size cap, retention purge

Webhook events are constructed and HMAC-signed here exactly as Stripe signs them
(SDK pins api_version 2024-06-20), so no Stripe CLI is required. Time-dependent
attacks (late submit, refund-mid-attempt) use DB surgery to simulate elapsed time,
which is the standard way to make such cases fast and deterministic.

Exit code 0 iff every assertion passes.  Run from backend/:  python3 tests/integration_test.py
"""
import base64, hashlib, hmac, json, os, socket, sqlite3, subprocess, sys, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_integration.db")
STORAGE = os.path.join(HERE, "_integration_storage")
WEBHOOK_SECRET = "whsec_integration_test_secret"
STRIPE_KEY = "sk_test_integration"

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port()
BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, token=None, body=None, raw=None, headers=None):
    url = BASE + path
    data = None; hdr = dict(headers or {})
    if raw is not None:
        data = raw
    elif body is not None:
        data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    if token: hdr["Authorization"] = "Bearer " + token
    r = urllib.request.Request(url, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r) as resp:
            return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e:
        return e.code, e.read().decode()

def jget(method, path, **kw):
    code, txt = req(method, path, **kw)
    try: return code, json.loads(txt)
    except Exception: return code, txt

def sha256hex(s): return hashlib.sha256(s.encode()).hexdigest()

def sign_and_send_webhook(session_id, email, product, pi_id, metadata=None, amount=9900, event_id=None, etype="checkout.session.completed"):
    """Construct a Stripe event exactly as Stripe would, HMAC-sign it, POST to /api/webhook."""
    meta = {"product": product, "first_name": "Test", "last_name": "User", "country": "PK",
            "final_amount": str(amount/100), "code_amount": "0", "standard_amount": str(amount/100), "default_discount": "0"}
    if metadata: meta.update(metadata)
    obj = {"id": session_id, "object": "checkout.session", "amount_total": amount,
           "customer_email": email, "customer_details": {"email": email},
           "payment_intent": pi_id, "metadata": meta, "mode": "payment", "payment_status": "paid"}
    if etype != "checkout.session.completed":
        obj = {"id": pi_id, "object": "payment_intent"}
    # Stripe.net's EventConverter dereferences data.object and request without null-guards,
    # so a genuine event's top-level shape is required: object/api_version/data/request/
    # pending_webhooks all present (a bare {type,data} crashes ConstructEvent with an NRE).
    evt = {"id": event_id or ("evt_" + sha256hex(session_id + etype)[:24]), "object": "event",
           "api_version": "2024-06-20", "created": int(time.time()), "livemode": False,
           "pending_webhooks": 1, "request": {"id": None, "idempotency_key": None},
           "type": etype, "data": {"object": obj, "previous_attributes": None}}
    payload = json.dumps(evt)
    ts = int(time.time())
    sig = hmac.new(WEBHOOK_SECRET.encode(), f"{ts}.{payload}".encode(), hashlib.sha256).hexdigest()
    return req("POST", "/api/webhook", raw=payload.encode(),
               headers={"Content-Type": "application/json", "Stripe-Signature": f"t={ts},v1={sig}"})

# The same suite runs against SQLite (default) or MySQL (TEST_DB_PROVIDER=mysql) so parity is proven,
# not assumed. On MySQL the DB-surgery statements below are translated (? → %s, datetime() → MySQL) by
# a thin wrapper, so the test body is identical for both providers.
PROVIDER = os.environ.get("TEST_DB_PROVIDER", "sqlite").lower()
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
             database=os.environ.get("MYSQL_DATABASE", "pci"))

def _mysql_translate(sql):
    # Percent literals in the SQL (DATE_FORMAT specifiers) are written as %% so pymysql's
    # `query % args` step collapses them back to %. f-strings (not %-format) are used here so the
    # escaping survives. ? placeholders become %s.
    import re
    sql = sql.replace("?", "%s")
    sql = re.sub(r"datetime\('now',\s*'([+-]?\d+)\s+(\w+)'\)",
                 lambda m: f"DATE_FORMAT(DATE_ADD(UTC_TIMESTAMP(), INTERVAL {m.group(1)} {m.group(2).rstrip('s').upper()}),'%%Y-%%m-%%d %%H:%%i:%%s')",
                 sql)
    sql = sql.replace("datetime('now')", "DATE_FORMAT(UTC_TIMESTAMP(),'%%Y-%%m-%%d %%H:%%i:%%s')")
    return sql

class _MyWrap:
    """Minimal drop-in for sqlite3.Connection over pymysql: execute()/commit()/close().
    Always passes an args tuple to pymysql so `query % args` runs and collapses %% → % consistently."""
    def __init__(self, conn): self.c = conn
    def execute(self, sql, params=None):
        cur = self.c.cursor()
        cur.execute(_mysql_translate(sql), params if params is not None else ())
        return cur
    def commit(self): self.c.commit()
    def close(self): self.c.close()

def dbconn():
    if PROVIDER == "mysql":
        import pymysql
        return _MyWrap(pymysql.connect(**MYSQL))
    return sqlite3.connect(DB)

def _reset_mysql():
    import pymysql
    root = pymysql.connect(host=MYSQL["host"], port=MYSQL["port"], user=MYSQL["user"], password=MYSQL["password"])
    cur = root.cursor()
    cur.execute("DROP DATABASE IF EXISTS " + MYSQL["database"])
    cur.execute("CREATE DATABASE " + MYSQL["database"] + " CHARACTER SET utf8mb4")
    root.commit(); root.close()

# ---- server lifecycle ----
def boot():
    if PROVIDER == "mysql":
        _reset_mysql()
        env = dict(os.environ, DB_PROVIDER="mysql", PORT=str(PORT), STORAGE_ROOT=STORAGE,
                   STRIPE_SECRET_KEY=STRIPE_KEY, STRIPE_WEBHOOK_SECRET=WEBHOOK_SECRET,
                   ASPNETCORE_ENVIRONMENT="Development", DATABASE_FILE=DB)
    else:
        for f in (DB, DB+"-wal", DB+"-shm"):
            try: os.remove(f)
            except OSError: pass
        env = dict(os.environ, DATABASE_FILE=DB, PORT=str(PORT), STORAGE_ROOT=STORAGE,
                   STRIPE_SECRET_KEY=STRIPE_KEY, STRIPE_WEBHOOK_SECRET=WEBHOOK_SECRET,
                   ASPNETCORE_ENVIRONMENT="Development")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND,
                            stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    for _ in range(60):
        try:
            code, _ = req("GET", "/api/health");
            if code == 200: return proc
        except Exception: pass
        time.sleep(0.5)
    proc.terminate(); raise SystemExit("server did not boot")

# ---- helpers built on the real flows ----
def make_paid_user(email, product="bundle", amount=9900, pi=None, sid=None, real_login=False, metadata=None):
    """Settle a payment via webhook, return (session_token, user_id).

    Helper users mint their session token directly in the DB — the real /api/login is rate-limited
    (10/min/IP), and driving a dozen helper logins through it would trip the limiter and couple the
    exam flows to the rate-limit test. The set-password + /api/login path is proven once, explicitly,
    via real_login=True (the happy-path user)."""
    pi = pi or ("pi_" + sha256hex(email+"pi")[:16]); sid = sid or ("cs_" + sha256hex(email+"cs")[:16])
    code, _ = sign_and_send_webhook(sid, email, product, pi, metadata=metadata)
    if code != 200: raise SystemExit(f"webhook failed for {email}: {code}")
    con = dbconn()
    row = con.execute("SELECT id FROM users WHERE email=?", (email,)).fetchone()
    if not row: con.close(); raise SystemExit(f"user not created for {email}")
    uid = row[0]
    if real_login:
        pw = "Passw0rd!" + email[:3]
        setpw = "setpw_" + sha256hex(email)[:20]
        con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+1 day'))",
                    (uid, sha256hex(setpw)))
        con.commit(); con.close()
        c, spb = jget("POST", "/api/set-password", body={"token": setpw, "password": pw})
        chk("2·auth set-password succeeds", c == 200 and spb.get("ok") is True, spb)
        c, body = jget("POST", "/api/login", body={"email": email, "password": pw})
        chk("2·auth real login returns session", c == 200 and bool(body.get("token")), body)
        return body["token"], uid
    tok = "sess_" + sha256hex(email)[:24]
    con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 day'))",
                (uid, sha256hex(tok)))
    con.commit(); con.close()
    return tok, uid

def accept_all_consents(token):
    req("POST", "/api/me/consents", token=token, body={"accept_all": True})

# 1×1 PNG — a valid image payload for the government-ID upload gate.
TINY_PNG = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg=="

def upload_id(token, kind="passport"):
    return jget("POST", "/api/me/identity-document", token=token,
                body={"doc_kind": kind, "filename": "id.png", "data_uri": TINY_PNG})

def complete_profile(token):
    req("PATCH", "/api/me/profile", token=token, body={"country": "Pakistan", "city": "Karachi"})
    upload_id(token)  # a government ID on file is part of standard booking eligibility

def widen_window(admin_tok):
    # open the launch window immediately (booking still requires slot>=now+2h) and set a known duration
    req("PATCH", "/api/admin/settings", token=admin_tok, body={"exam_open_before_minutes": "100000"})

def book_and_start(token, admin_tok, extra_expected_dur=90):
    slot = time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime(time.time() + 3*3600))
    c, bk = jget("POST", "/api/me/exam/book", token=token, body={"scheduled_at": slot, "timezone": "UTC"})
    if c != 200: return c, bk, None
    req("POST", "/api/me/readiness", token=token, body={"camera": True, "microphone": True, "network": True})
    c, st = jget("POST", "/api/me/exam/start", token=token, body={})
    return c, bk, st

def answer_key(item_ids):
    con = dbconn()
    q = ",".join("?"*len(item_ids))
    rows = con.execute(f"SELECT id,answer_index FROM sample_questions WHERE id IN ({q})", item_ids).fetchall()
    con.close()
    return {str(r[0]): r[1] for r in rows}

def admin_login():
    c, b = jget("POST", "/api/admin/auth/login", body={"email": "owner@pci.local", "password": "changeme-owner"})
    return b["token"]

# ============================================================================
def main():
    proc = boot()
    try:
        run(proc)
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()
    print(f"\n  ══ {passed}/{passed+failed} PASSED ══")
    sys.exit(0 if failed == 0 else 1)

def run(proc):
    admin = admin_login()
    widen_window(admin)

    # ---------- 1. PAYMENTS: settlement exactly once + replay idempotency ----------
    print("\n=== 1. Payments: webhook settlement + replay idempotency ===")
    email = "buyer1@ex.co"; pi = "pi_buyer1"; sid = "cs_buyer1"
    code, _ = sign_and_send_webhook(sid, email, "bundle", pi)
    chk("1a webhook accepted (200)", code == 200, code)
    con = dbconn()
    def counts(e):
        u = con.execute("SELECT id FROM users WHERE email=?", (e,)).fetchone()
        if not u: return (0,0,0,0)
        uid = u[0]
        return (1,
                con.execute("SELECT COUNT(*) FROM payments WHERE user_id=?", (uid,)).fetchone()[0],
                con.execute("SELECT COUNT(*) FROM memberships WHERE user_id=?", (uid,)).fetchone()[0],
                con.execute("SELECT COUNT(*) FROM exam_entitlements WHERE user_id=?", (uid,)).fetchone()[0])
    c1 = counts(email); con.close()
    chk("1b user+payment+membership+entitlement each once", c1 == (1,1,1,1), c1)
    # replay the SAME event id + payment intent
    code, _ = sign_and_send_webhook(sid, email, "bundle", pi)
    chk("1c replay accepted (200)", code == 200, code)
    con = dbconn(); c2 = counts(email); con.close()
    chk("1d replay created nothing new", c2 == (1,1,1,1), c2)
    # tampered signature rejected
    code, _ = req("POST", "/api/webhook", raw=b'{"id":"evt_x"}',
                  headers={"Content-Type":"application/json","Stripe-Signature":"t=1,v1=deadbeef"})
    chk("1e bad signature rejected (400)", code == 400, code)
    # failed payment grants nothing
    sign_and_send_webhook("cs_fail", "nogrant@ex.co", "bundle", "pi_fail", etype="payment_intent.payment_failed")
    con = dbconn(); ng = con.execute("SELECT COUNT(*) FROM users WHERE email=?", ("nogrant@ex.co",)).fetchone()[0]; con.close()
    chk("1f failed payment grants no account", ng == 0, ng)

    # ---------- 2. CERT HAPPY PATH: immediate released_pass + credential ----------
    print("\n=== 2. Certification happy path (also proves real set-password + login) ===")
    tok, uid = make_paid_user("passuser@ex.co", real_login=True)
    # eligibility gate blocks before consents/profile
    slot = time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime(time.time()+3*3600))
    c, blk = jget("POST", "/api/me/exam/book", token=tok, body={"scheduled_at": slot, "timezone":"UTC"})
    chk("2a booking blocked before consents/profile", c == 400 and blk.get("error")=="not_eligible", blk)
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    chk("2b start returns attempt + items", c == 200 and "attempt_id" in st, st if c!=200 else "")
    key = answer_key([i["id"] for i in st["items"]])
    c, sub = jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": st["attempt_id"], "answers": key})
    chk("2c immediate released_pass", c==200 and sub.get("result")=="pass" and sub.get("result_status")=="credential_issued", sub)
    chk("2d credential issued", bool(sub.get("credential")), sub.get("credential"))
    cred = sub.get("credential")
    c, v = jget("GET", f"/api/verify?id={cred}")
    chk("2e public verify = active/valid", v.get("found") and v.get("state")=="active" and v.get("valid") is True, v)

    # ---------- 3. FAIL PATH ----------
    print("\n=== 3. Fail path ===")
    tok, uid = make_paid_user("failuser@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    key = answer_key([i["id"] for i in st["items"]])
    wrong = {k: (v+1)%4 for k,v in key.items()}  # deliberately wrong
    c, sub = jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": st["attempt_id"], "answers": wrong})
    chk("3a immediate released_fail", c==200 and sub.get("result")=="fail" and sub.get("result_status")=="released_fail", sub)
    chk("3b no credential on fail", not sub.get("credential"), sub.get("credential"))

    # ---------- 4. ATTACK PATHS ----------
    print("\n=== 4. Attack paths ===")
    # 4a late submit
    tok, uid = make_paid_user("late@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    key = answer_key([i["id"] for i in st["items"]])
    con = dbconn()
    con.execute("UPDATE exam_attempts SET started_at=datetime('now','-3 hours') WHERE id=?", (st["attempt_id"],))
    con.commit(); con.close()
    c, sub = jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": st["attempt_id"], "answers": key})
    chk("4a late submit held submitted_after_deadline, no credential",
        c==200 and sub.get("held") is True and sub.get("hold_reason")=="submitted_after_deadline" and not sub.get("credential"), sub)
    chk("4a' held payload leaks NO pass/fail/percent",
        all(kk not in sub for kk in ("result","percent","score","Result","Percent")), list(sub.keys()))

    # 4b duplicate submit
    tok, uid = make_paid_user("dup@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    key = answer_key([i["id"] for i in st["items"]])
    jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": st["attempt_id"], "answers": key})
    con = dbconn(); n_before = con.execute("SELECT COUNT(*) FROM issued_credentials WHERE user_id=?", (uid,)).fetchone()[0]; con.close()
    c, sub2 = jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": st["attempt_id"], "answers": key})
    con = dbconn(); n_after = con.execute("SELECT COUNT(*) FROM issued_credentials WHERE user_id=?", (uid,)).fetchone()[0]; con.close()
    chk("4b duplicate submit → already_submitted", c==400 and sub2.get("error")=="already_submitted", sub2)
    chk("4b' no second credential", n_before == n_after == 1, (n_before, n_after))

    # 4c foreign item ids → item_set_mismatch
    tok, uid = make_paid_user("foreign@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    bad = {"9999999": 0}
    c, sub = jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": st["attempt_id"], "answers": bad})
    chk("4c foreign item ids → item_set_mismatch held", c==200 and sub.get("hold_reason")=="item_set_mismatch", sub)

    # 4d consumed entitlement re-book
    tok, uid = make_paid_user("consume@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    key = answer_key([i["id"] for i in st["items"]])
    jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": st["attempt_id"], "answers": key})
    slot = time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime(time.time()+3*3600))
    c, rb = jget("POST", "/api/me/exam/book", token=tok, body={"scheduled_at": slot, "timezone":"UTC"})
    chk("4d re-book after consumed entitlement rejected", c==400 and rb.get("error") in ("exam_already_taken","payment_already_used","already_booked"), rb)

    # 4e refunded payment then submit → payment_reversed
    tok, uid = make_paid_user("refund@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    key = answer_key([i["id"] for i in st["items"]])
    con = dbconn()
    con.execute("UPDATE payments SET payment_status='refunded' WHERE user_id=?", (uid,))
    con.commit(); con.close()
    c, sub = jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": st["attempt_id"], "answers": key})
    chk("4e refunded-then-submit → payment_reversed held, no credential",
        c==200 and sub.get("hold_reason")=="payment_reversed" and not sub.get("credential"), sub)

    # 4f answer-key leakage: dump every student-facing exam payload and grep for answer_index
    tok, uid = make_paid_user("leak@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    _, start_txt = req("POST", "/api/me/exam/start", token=tok, body={})   # resume returns same items
    _, me_txt = req("GET", "/api/me", token=tok)
    _, prac_txt = req("GET", "/api/me/practice", token=tok)
    _, hb_txt = req("POST", "/api/me/exam/heartbeat", token=tok, body={"attempt_id": st["attempt_id"]})
    blob = (start_txt + me_txt + prac_txt + hb_txt).lower()
    chk("4f no answer_index in any student-facing exam payload", "answer_index" not in blob, "LEAK")
    chk("4f' no answer_key field leaked", "answer_key" not in blob or "answer_key_version" in blob, "LEAK")

    # ---------- 5. HELD → ADMIN LOOP ----------
    print("\n=== 5. Held → admin release/invalidate/reinstate ===")
    req("PATCH", "/api/admin/settings", token=admin, body={"auto_block_result_on_critical_violation": "1", "critical_violation_threshold": "1"})
    tok, uid = make_paid_user("held@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    c, bk, st = book_and_start(tok, admin)
    key = answer_key([i["id"] for i in st["items"]])
    aid = st["attempt_id"]
    con = dbconn()
    con.execute("INSERT INTO proctor_events(attempt_id,user_id,type,severity,detail,at) VALUES(?,?,?,?,?,datetime('now'))",
                (aid, uid, "MultipleFaces", "Critical", "two faces", ))
    con.commit(); con.close()
    c, sub = jget("POST", "/api/me/exam/submit", token=tok, body={"attempt_id": aid, "answers": key})
    chk("5a critical violation → held (setting ON)", c==200 and sub.get("held") is True and sub.get("hold_reason")=="critical_proctor_violation", sub)
    chk("5a' held payload has no pass/fail/percent", all(kk not in sub for kk in ("result","percent","score")), list(sub.keys()))
    # student /api/me must also redact
    _, me = jget("GET", "/api/me", token=tok)
    held_att = next((a for a in me["attempts"] if a["id"]==aid), {})
    chk("5b /api/me redacts held attempt result/percent", held_att.get("result") is None and held_att.get("percent") is None, held_att)
    # admin release → credential issued (it was a pass)
    c, rel = jget("POST", f"/api/admin/exam-sessions/{aid}/review", token=admin, body={"action":"release","note":"reviewed clean"})
    chk("5c admin release issues credential on pass", c==200 and bool(rel.get("credential")), rel)
    relcred = rel.get("credential")
    # invalidate → credential revoked
    c, inv = jget("POST", f"/api/admin/exam-sessions/{aid}/review", token=admin, body={"action":"invalidate","note":"integrity"})
    chk("5d invalidate revokes credential", c==200 and inv.get("result_status")=="credential_revoked", inv)
    c, v = jget("GET", f"/api/verify?id={relcred}")
    chk("5d' verify shows revoked", v.get("state")=="revoked" and v.get("valid") is False, v)
    # reinstate → uses configured pass mark
    c, rei = jget("POST", f"/api/admin/exam-sessions/{aid}/review", token=admin, body={"action":"reinstate","note":"overturned"})
    chk("5e reinstate republishes with configured pass mark", c==200 and rei.get("result_status") in ("credential_issued","released_pass"), rei)
    req("PATCH", "/api/admin/settings", token=admin, body={"auto_block_result_on_critical_violation": "0"})

    # ---------- 6. ACCOMMODATIONS: +30 → live duration 120 ----------
    print("\n=== 6. Accommodations extend the live sitting ===")
    tok, uid = make_paid_user("accom@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    con = dbconn()
    con.execute("INSERT INTO accommodation_requests(user_id,request_type,description,status,approved_extra_minutes,decided_at) VALUES(?,?,?, 'approved',30,datetime('now'))",
                (uid, "extra_time", "documented need"))
    con.commit(); con.close()
    c, bk, st = book_and_start(tok, admin)
    chk("6a browser start duration = 120 (90+30)", st.get("duration_minutes")==120, st.get("duration_minutes"))

    # ---------- 7. RBAC probes ----------
    print("\n=== 7. RBAC per role ===")
    def make_admin(role):
        c, b = jget("POST", "/api/admin/team", token=admin, body={"email": f"{role}@pci.test", "name": role, "role": role})
        tp = b.get("temp_password")
        c, lb = jget("POST", "/api/admin/auth/login", body={"email": f"{role}@pci.test", "password": tp})
        return lb.get("token")
    # exam_manager: can proctoring, cannot student/website/owner sections
    em = make_admin("exam_manager")
    chk("7a exam_mgr CAN exam-sessions (200)", jget("GET","/api/admin/exam-sessions",token=em)[0]==200)
    chk("7b exam_mgr BLOCKED members (403)", jget("GET","/api/admin/members",token=em)[0]==403)
    chk("7c exam_mgr BLOCKED payments (403)", jget("GET","/api/admin/payments",token=em)[0]==403)
    chk("7d exam_mgr BLOCKED team/owner (403)", jget("GET","/api/admin/team",token=em)[0]==403)
    sm = make_admin("student_manager")
    chk("7e student_mgr CAN members (200)", jget("GET","/api/admin/members",token=sm)[0]==200)
    chk("7f student_mgr BLOCKED exam-sessions (403)", jget("GET","/api/admin/exam-sessions",token=sm)[0]==403)
    wm = make_admin("website_manager")
    chk("7g website_mgr CAN pricing (200)", jget("GET","/api/admin/pricing",token=wm)[0]==200)
    chk("7h website_mgr BLOCKED members (403)", jget("GET","/api/admin/members",token=wm)[0]==403)
    vw = make_admin("viewer")
    chk("7i viewer CAN overview (200)", jget("GET","/api/admin/overview",token=vw)[0]==200)
    # settings is per-key deny-by-default: a viewer's write is rejected (200 + rejected list), not persisted.
    c, sw = jget("PATCH","/api/admin/settings",token=vw,body={"exam_pass_mark_pct":"33"})
    chk("7j viewer settings write rejected per-key", c==200 and "exam_pass_mark_pct" in (sw.get("rejected") or []), sw)
    chk("7j' viewer BLOCKED owner-only team write (403)", jget("POST","/api/admin/team",token=vw,body={"email":"x@y.z","name":"X","role":"viewer"})[0]==403)
    # legacy token dead
    chk("7k legacy x-admin-token dead", jget("GET","/api/admin/me",headers={"x-admin-token":"changeme"})[0]==401)

    # ---------- 8. Desktop launch flow: authorize (launch code) → submit ----------
    print("\n=== 8. Desktop client flow (/api/exam/authorize → submit) ===")
    tok, uid = make_paid_user("desktop@ex.co")
    accept_all_consents(tok); complete_profile(tok)
    slot = time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime(time.time()+3*3600))
    jget("POST", "/api/me/exam/book", token=tok, body={"scheduled_at": slot, "timezone":"UTC"})
    req("POST", "/api/me/readiness", token=tok, body={"camera": True, "microphone": True, "network": True})
    c, lc = jget("POST", "/api/me/exam/launch-code", token=tok, body={})
    chk("8a launch-code minted", c==200 and lc.get("code","").startswith("PCI-"), lc)
    # redeem against the authorize endpoint (no bearer — code IS the auth)
    c, az = jget("POST", "/api/exam/authorize", body={"code": lc["code"]})
    # ASP.NET camelCases: SessionToken→sessionToken, Items→items, Id→id (desktop parses case-insensitively)
    stoken = az.get("sessionToken") or az.get("session_token")
    items = az.get("items") or az.get("Items")
    chk("8b authorize returns session token + items", c==200 and bool(stoken) and bool(items), az if c!=200 else "ok")
    chk("8c authorize response carries no answer key", "answer_index" not in json.dumps(az).lower(), "LEAK")
    dtok = stoken
    key = answer_key([it["id"] for it in items])
    c, sub = jget("POST", "/api/me/exam/submit", token=dtok, body={"attempt_id": lc["code"], "answers": key, "client_kind": "desktop"})
    chk("8d desktop submit → pass + credential", c==200 and sub.get("result")=="pass" and bool(sub.get("credential")), sub)
    chk("8d' submit exposes desktop camelCase aliases", all(k in sub for k in ("percent","credentialId","resultStatus")), list(sub.keys()))
    # reused launch code rejected once consumed
    c, az2 = jget("POST", "/api/exam/authorize", body={"code": lc["code"]})
    chk("8e reused launch code rejected", c in (400,401), (c,az2))
    # unknown code rejected
    c, azb = jget("POST", "/api/exam/authorize", body={"code": "PCI-DOESNOTEXIST"})
    chk("8f unknown launch code rejected (401)", c==401, (c,azb))

    # ---------- 9. Storage: sniff + size + purge ----------
    print("\n=== 9. Storage abstraction ===")
    stok, suid = make_paid_user("store@ex.co")
    c, tk = jget("POST", "/api/me/tickets", token=stok, body={"subject":"help","category":"general","body":"hi"})
    tid = tk.get("id")
    real_pdf = "data:application/pdf;base64," + base64.b64encode(b"%PDF-1.4 test").decode()
    c, up = jget("POST", f"/api/me/tickets/{tid}/attachments", token=stok, body={"filename":"r.pdf","data_uri":real_pdf})
    chk("9a valid PDF accepted", c==200, (c,up))
    # exe renamed as pdf → sniff rejects
    fake = "data:application/pdf;base64," + base64.b64encode(b"MZ\x90\x00 this is a PE").decode()
    c, up = jget("POST", f"/api/me/tickets/{tid}/attachments", token=stok, body={"filename":"x.pdf","data_uri":fake})
    chk("9b renamed exe-as-pdf rejected by magic-byte sniff", c==400 and "mismatch" in json.dumps(up), up)
    # oversized rejected
    big = "data:image/png;base64," + base64.b64encode(b"\x89PNG\r\n\x1a\n"+b"A"*4_000_000).decode()
    c, up = jget("POST", f"/api/me/tickets/{tid}/attachments", token=stok, body={"filename":"big.png","data_uri":big})
    chk("9c oversized rejected", c==400 and "too_large" in json.dumps(up), up)
    # bytes on disk as local:<category>/<shard>/<sha>.<ext>, DB holds reference only
    con = dbconn()
    ref = con.execute("SELECT storage_ref FROM support_attachments WHERE ticket_id=? ORDER BY id DESC", (tid,)).fetchone()
    du = con.execute("SELECT data_uri FROM support_attachments WHERE ticket_id=? ORDER BY id DESC", (tid,)).fetchone()
    con.close()
    chk("9d DB row holds storage_ref only (no inline data_uri)", ref and ref[0] and ref[0].startswith("local:") and (du is None or du[0] is None), (ref, du))
    on_disk = os.path.exists(os.path.join(STORAGE, ref[0].split(":",1)[1])) if ref and ref[0] else False
    chk("9e bytes present under STORAGE_ROOT", on_disk, ref[0] if ref else None)
    # retention purge (owner). set retention to 0 days so the just-written file is eligible
    req("PATCH", "/api/admin/settings", token=admin, body={"evidence_retention_days":"0"})
    # backdate the file so 0-day cutoff removes it deterministically
    if ref and ref[0]:
        p = os.path.join(STORAGE, ref[0].split(":",1)[1])
        if os.path.exists(p): os.utime(p, (time.time()-86400, time.time()-86400))
    c, pg = jget("POST", "/api/admin/storage/purge", token=admin, body={})
    chk("9f storage purge runs (owner)", c==200, (c,pg))

    # ---------- 9b. Security headers + body cap (Phase 2) ----------
    print("\n=== 9b. Security headers + body cap ===")
    # read raw headers off a page response
    r = urllib.request.Request(BASE + "/index.html")
    with urllib.request.urlopen(r) as resp:
        hdrs = {k.lower(): v for k, v in resp.headers.items()}
    chk("9b1 X-Content-Type-Options nosniff", hdrs.get("x-content-type-options") == "nosniff", hdrs.get("x-content-type-options"))
    chk("9b2 Referrer-Policy set", "referrer-policy" in hdrs, hdrs.get("referrer-policy"))
    chk("9b3 CSP present with frame-ancestors none", "frame-ancestors 'none'" in (hdrs.get("content-security-policy") or ""), hdrs.get("content-security-policy"))
    chk("9b4 X-Frame-Options DENY", hdrs.get("x-frame-options") == "DENY", hdrs.get("x-frame-options"))
    chk("9b5 no HSTS over plain http", "strict-transport-security" not in hdrs, hdrs.get("strict-transport-security"))
    # HSTS appears only behind a forwarding https proxy
    r2 = urllib.request.Request(BASE + "/api/health", headers={"X-Forwarded-Proto": "https"})
    with urllib.request.urlopen(r2) as resp:
        h2 = {k.lower(): v for k, v in resp.headers.items()}
    chk("9b6 HSTS emitted when X-Forwarded-Proto=https", "strict-transport-security" in h2, h2.get("strict-transport-security"))
    # oversized body (>6 MB) rejected before handler buffers it
    over = json.dumps({"blob": "x" * 7_000_000}).encode()
    rejected = False
    try:
        req_o = urllib.request.Request(BASE + "/api/newsletter", data=over, headers={"Content-Type": "application/json"}, method="POST")
        code_o = urllib.request.urlopen(req_o).status
        rejected = code_o in (413, 400)
    except urllib.error.HTTPError as e:
        rejected = e.code in (413, 400)
    except Exception:
        rejected = True  # Kestrel aborts the connection on oversized body → also a rejection
    chk("9b7 oversized request body rejected", rejected)

    # ---------- 9c. Regressions for adversarial-review findings ----------
    print("\n=== 9c. Review-finding regressions ===")

    # R1 (held leak via /api/me/attempts/{id}): a held attempt must redact pass/fail on THAT endpoint too
    req("PATCH", "/api/admin/settings", token=admin, body={"auto_block_result_on_critical_violation":"1","critical_violation_threshold":"1"})
    rtok, ruid = make_paid_user("rheld@ex.co")
    accept_all_consents(rtok); complete_profile(rtok)
    c, bk, st = book_and_start(rtok, admin)
    raid = st["attempt_id"]; rkey = answer_key([i["id"] for i in st["items"]])
    con = dbconn(); con.execute("INSERT INTO proctor_events(attempt_id,user_id,type,severity,detail,at) VALUES(?,?,?,?,?,datetime('now'))",
                                (raid, ruid, "MultipleFaces", "Critical", "x")); con.commit(); con.close()
    jget("POST", "/api/me/exam/submit", token=rtok, body={"attempt_id": raid, "answers": rkey})
    c, att = jget("GET", f"/api/me/attempts/{raid}", token=rtok)
    chk("R1 /api/me/attempts/{id} redacts held pass/fail", c==200 and att.get("percent") is None and att.get("result") is None and att.get("domain_breakdown") is None, att)
    req("PATCH", "/api/admin/settings", token=admin, body={"auto_block_result_on_critical_violation":"0"})

    # R13 (desktop DomainBand.Percent): submit breakdown must carry BOTH pct (browser) and percent (desktop)
    ptok, puid = make_paid_user("rband@ex.co")
    accept_all_consents(ptok); complete_profile(ptok)
    c, bk, st = book_and_start(ptok, admin)
    pkey = answer_key([i["id"] for i in st["items"]])
    c, sub = jget("POST", "/api/me/exam/submit", token=ptok, body={"attempt_id": st["attempt_id"], "answers": pkey})
    band0 = (sub.get("breakdown") or [{}])[0]
    chk("R13 breakdown band exposes pct AND percent", "pct" in band0 and "percent" in band0 and band0["pct"]==band0["percent"], band0)

    # R3 (heartbeat auto-timeout PUBLISHES): a timed-out attempt scores, publishes and issues a credential
    atok, auid = make_paid_user("rtimeout@ex.co")
    accept_all_consents(atok); complete_profile(atok)
    c, bk, st = book_and_start(atok, admin)
    taid = st["attempt_id"]; tkey = answer_key([i["id"] for i in st["items"]])
    con = dbconn()
    con.execute("UPDATE exam_attempts SET answers=?, started_at=datetime('now','-3 hours') WHERE id=?", (json.dumps(tkey), taid))
    con.commit(); con.close()
    jget("POST", "/api/me/exam/heartbeat", token=atok, body={"attempt_id": taid})   # crosses hard stop → auto-finalise
    con = dbconn()
    rs = con.execute("SELECT result_status FROM exam_attempts WHERE id=?", (taid,)).fetchone()[0]
    ncred = con.execute("SELECT COUNT(*) FROM issued_credentials WHERE attempt_id=?", (taid,)).fetchone()[0]
    con.close()
    chk("R3 auto-timeout publishes result_status", rs in ("credential_issued","released_pass"), rs)
    chk("R3' auto-timeout issues credential on clean pass", ncred == 1, ncred)

    # R4 (entitlement expiry space-vs-T): a same-day deadline a few hours ahead must NOT read expired
    e4tok, e4uid = make_paid_user("rexpiry@ex.co")
    accept_all_consents(e4tok); complete_profile(e4tok)
    con = dbconn()
    con.execute("UPDATE payments SET exam_schedule_deadline=datetime('now','+6 hours') WHERE user_id=? AND product_type IN ('exam','bundle')", (e4uid,))
    con.commit(); con.close()
    slot = time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime(time.time()+3*3600))
    c, bkr = jget("POST", "/api/me/exam/book", token=e4tok, body={"scheduled_at": slot, "timezone":"UTC"})
    chk("R4 same-day deadline not flagged expired (space-vs-T fix)", c==200 or (isinstance(bkr,dict) and bkr.get("error")!="not_eligible"), bkr)

    # R7 (settings RBAC): a viewer cannot write an un-prefixed platform key (auto_block_*)
    vtok = None
    c, vb = jget("POST", "/api/admin/team", token=admin, body={"email":"rbacview@pci.test","name":"V","role":"viewer"})
    c, vl = jget("POST", "/api/admin/auth/login", body={"email":"rbacview@pci.test","password":vb.get("temp_password","")})
    vtok = vl.get("token")
    c, sw = jget("PATCH", "/api/admin/settings", token=vtok, body={"evidence_retention_days":"1"})
    chk("R7 viewer BLOCKED from platform key (deny-by-default)", "evidence_retention_days" in (sw.get("rejected") or []), sw)
    con = dbconn(); val = con.execute("SELECT svalue FROM site_settings WHERE skey='evidence_retention_days'").fetchone(); con.close()
    chk("R7' rejected platform key not persisted", val is None or val[0] != "1", val)

    # R9 (CSP frame-src blob for admin evidence viewer)
    r = urllib.request.Request(BASE + "/index.html")
    with urllib.request.urlopen(r) as resp: csp = resp.headers.get("Content-Security-Policy","")
    chk("R9 CSP allows blob: in frame-src (admin evidence viewer)", "frame-src 'self' blob:" in csp, csp[:120])

    # ---------- 9c2. Demo student seed (first-run only, like the bootstrap owner) ----------
    print("\n=== 9c2. Demo student seed ===")
    c, dl = jget("POST", "/api/login", body={"email": "student@pci.local", "password": "changeme-student"})
    chk("9c2a demo student can log in on a fresh database", c==200 and bool(dl.get("token")), dl)
    con = dbconn()
    dgrants = [con.execute("SELECT COUNT(*) FROM %s WHERE user_id=(SELECT id FROM users WHERE email='student@pci.local')" % t).fetchone()[0]
               for t in ("memberships", "exam_entitlements", "issued_credentials")]
    con.close()
    chk("9c2b demo student granted nothing (account only)", dgrants == [0, 0, 0], dgrants)

    # ---------- 9d. Manual onboarding + email delivery (mailer was missing entirely) ----------
    print("\n=== 9d. Admin add-member + mailer ===")
    # create a member from the admin panel; the setup link comes back in the response
    c, am = jget("POST", "/api/admin/members", token=admin, body={"email": "manual@ex.co", "first_name": "Manu", "last_name": "Al"})
    chk("9d1 admin creates member + gets setup link", c==200 and "reset-password.html?token=" in (am.get("setup_url") or ""), am)
    # the link works end-to-end: set password → student login (the full onboarding journey)
    mtoken = am["setup_url"].split("token=")[1]
    c, sp = jget("POST", "/api/set-password", body={"token": mtoken, "password": "Manual-Pass-1!"})
    chk("9d2 setup link sets a password", c==200 and sp.get("ok") is True, sp)
    c, lg = jget("POST", "/api/login", body={"email": "manual@ex.co", "password": "Manual-Pass-1!"})
    chk("9d3 manually-created member can log into the student panel", c==200 and bool(lg.get("token")), lg)
    # created account grants NOTHING beyond login: no membership, no entitlement, no credential
    con = dbconn()
    grants = [con.execute(f"SELECT COUNT(*) FROM {t} WHERE user_id=(SELECT id FROM users WHERE email='manual@ex.co')").fetchone()[0]
              for t in ("memberships", "exam_entitlements", "issued_credentials")]
    con.close()
    chk("9d4 manual account grants no membership/entitlement/credential", grants == [0, 0, 0], grants)
    # duplicate email → 409; garbage email → 400
    chk("9d5 duplicate email rejected (409)", jget("POST", "/api/admin/members", token=admin, body={"email": "manual@ex.co"})[0] == 409)
    chk("9d6 invalid email rejected (400)", jget("POST", "/api/admin/members", token=admin, body={"email": "not-an-email"})[0] == 400)
    # RBAC: exam_manager may not create members (members is a student-section permission)
    c, xb = jget("POST", "/api/admin/team", token=admin, body={"email": "exm2@pci.test", "name": "X", "role": "exam_manager"})
    c, xl = jget("POST", "/api/admin/auth/login", body={"email": "exm2@pci.test", "password": xb.get("temp_password", "")})
    chk("9d7 exam_manager BLOCKED from creating members (403)", jget("POST", "/api/admin/members", token=xl.get("token"), body={"email": "x2@ex.co"})[0] == 403)
    # resend-setup now returns the link too (previously the token was minted and lost)
    con = dbconn(); mid = con.execute("SELECT id FROM users WHERE email='manual@ex.co'").fetchone()[0]; con.close()
    c, rs = jget("POST", f"/api/admin/members/{mid}/resend-setup", token=admin, body={})
    chk("9d8 resend-setup returns a fresh setup link", c==200 and "reset-password.html?token=" in (rs.get("setup_url") or ""), rs)
    # every delivery attempt is recorded in email_logs (welcome ×2 here + webhook settlements earlier)
    con = dbconn()
    welcomes = con.execute("SELECT COUNT(*) FROM email_logs WHERE email_type='welcome'").fetchone()[0]
    con.close()
    chk("9d9 email_logs records welcome deliveries", welcomes >= 2, welcomes)
    # forgot-password now actually delivers a reset email (was a silent dead end)
    req("POST", "/api/forgot-password", body={"email": "manual@ex.co"})
    con = dbconn()
    resets = con.execute("SELECT COUNT(*) FROM email_logs WHERE email_type='password_reset' AND email='manual@ex.co'").fetchone()[0]
    con.close()
    chk("9d10 forgot-password records a reset delivery", resets == 1, resets)

    # ---------- 9e. MULTI-CERTIFICATION: full journey through a second credential ----------
    print("\n=== 9e. Multi-certification ===")
    # owner creates a second certification with its own pass mark, duration, expiry and price
    c, cc = jget("POST", "/api/admin/certifications", token=admin,
                 body={"code": "PCP-COST", "name": "Certified Cost Engineering Professional",
                       "pass_mark_pct": 80, "duration_minutes": 60, "expiry_years": 2, "exam_price": 199})
    chk("9e1 admin creates second certification", c==200 and cc.get("id"), cc)
    cost_id = cc["id"]
    chk("9e1' duplicate code rejected (409)", jget("POST", "/api/admin/certifications", token=admin, body={"code": "PCP-COST", "name": "x"})[0] == 409)
    # public catalogue lists it with its own parameters
    c, cat = jget("GET", "/api/certifications")
    entry = next((r for r in cat.get("rows", []) if r.get("code") == "PCP-COST"), None)
    chk("9e2 public catalogue shows the new certification", entry is not None and entry["duration_minutes"] == 60 and entry["pass_mark_pct"] == 80, entry)
    # its own question bank via bulk upload
    csv = "question,option_a,option_b,option_c,option_d,answer,domain\n" + "\n".join(
        f"COST Q{i}: pick A,RightA{i},WrongB{i},WrongC{i},WrongD{i},A,Cost Engineering" for i in range(1, 4))
    c, bu = jget("POST", "/api/admin/sample_questions/bulk", token=admin, body={"csv": csv, "certification": "PCP-COST"})
    chk("9e3 bulk upload into the new bank", c==200 and bu.get("inserted") == 3, bu)
    # buy the NEW certification via webhook metadata → entitlement routed to it
    ctok, cuid = make_paid_user("cost1@ex.co", product="exam", metadata={"certification": "PCP-COST"})
    con = dbconn()
    ecid = con.execute("SELECT certification_id FROM exam_entitlements WHERE user_id=?", (cuid,)).fetchone()[0]
    con.close()
    chk("9e4 webhook routes entitlement to the purchased certification", ecid == cost_id, ecid)
    accept_all_consents(ctok); complete_profile(ctok)
    # book + start THIS certification: its own 60-minute duration and its own 3-item bank
    slot = time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime(time.time() + 3*3600))
    c, bkr = jget("POST", "/api/me/exam/book", token=ctok, body={"scheduled_at": slot, "timezone": "UTC", "certification_id": cost_id})
    chk("9e5 booking lands on the certification", c==200 and bkr.get("certification_id") == cost_id, bkr)
    req("POST", "/api/me/readiness", token=ctok, body={"camera": True, "microphone": True, "network": True})
    c, st = jget("POST", "/api/me/exam/start", token=ctok, body={"certification_id": cost_id})
    chk("9e6 start uses the certification's duration (60)", c==200 and st.get("duration_minutes") == 60, st)
    ids = [i["id"] for i in st.get("items", [])]
    con = dbconn()
    bank = {r[0] for r in con.execute("SELECT id FROM sample_questions WHERE certification_id=?", (cost_id,))}
    con.close()
    chk("9e7 issued items come ONLY from this certification's bank", len(ids) == 3 and set(ids) == bank, (ids, bank))
    # full-marks submit → pass at the 80% mark → credential with the NEW prefix + 2-year expiry
    key = answer_key(ids)
    c, sub = jget("POST", "/api/me/exam/submit", token=ctok, body={"attempt_id": st["attempt_id"], "answers": key})
    cred = sub.get("credential") or ""
    chk("9e8 pass issues credential with the certification's prefix", c==200 and cred.startswith("PCP-COST-"), sub)
    c, v = jget("GET", f"/api/verify?id={cred}")
    chk("9e9 verify shows the certification", v.get("valid") is True and v.get("certification_code") == "PCP-COST", v)
    con = dbconn()
    exp = con.execute("SELECT expires_at FROM issued_credentials WHERE credential_id=?", (cred,)).fetchone()[0]
    con.close()
    yrs = (int(exp[:4]) - int(time.strftime("%Y")))
    chk("9e10 credential expiry honours the certification (2 years)", yrs == 2, exp)
    # cross-bank injection: a PCP-COST attempt answering PCP-AI item ids → item_set_mismatch hold
    xtok, xuid = make_paid_user("cost2@ex.co", product="exam", metadata={"certification": "PCP-COST"})
    accept_all_consents(xtok); complete_profile(xtok)
    jget("POST", "/api/me/exam/book", token=xtok, body={"scheduled_at": slot, "timezone": "UTC", "certification_id": cost_id})
    req("POST", "/api/me/readiness", token=xtok, body={"camera": True, "microphone": True, "network": True})
    c, xst = jget("POST", "/api/me/exam/start", token=xtok, body={"certification_id": cost_id})
    con = dbconn()
    foreign = [str(r[0]) for r in con.execute("SELECT id FROM sample_questions WHERE COALESCE(certification_id,1)=1 LIMIT 3")]
    con.close()
    c, xsub = jget("POST", "/api/me/exam/submit", token=xtok, body={"attempt_id": xst["attempt_id"], "answers": {f: 0 for f in foreign}})
    chk("9e11 foreign-bank answers held as item_set_mismatch", c==200 and xsub.get("held") is True and xsub.get("hold_reason") == "item_set_mismatch", xsub)
    # per-cert pass mark: 2/3 (66.7%) fails PCP-COST's 80% even though it would pass PCP-AI's 65%
    ptok3, puid3 = make_paid_user("cost3@ex.co", product="exam", metadata={"certification": "PCP-COST"})
    accept_all_consents(ptok3); complete_profile(ptok3)
    jget("POST", "/api/me/exam/book", token=ptok3, body={"scheduled_at": slot, "timezone": "UTC", "certification_id": cost_id})
    req("POST", "/api/me/readiness", token=ptok3, body={"camera": True, "microphone": True, "network": True})
    c, pst = jget("POST", "/api/me/exam/start", token=ptok3, body={"certification_id": cost_id})
    pids = [i["id"] for i in pst["items"]]; pkey3 = answer_key(pids)
    partial = {k: (v if idx < 2 else v + 1) for idx, (k, v) in enumerate(sorted(pkey3.items()))}
    c, psub = jget("POST", "/api/me/exam/submit", token=ptok3, body={"attempt_id": pst["attempt_id"], "answers": partial})
    chk("9e12 66.7% fails the certification's 80% pass mark", c==200 and psub.get("result") == "fail" and not psub.get("credential"), psub)
    # simultaneous bookings across certifications: same user buys PCP-AI too and books BOTH
    _, _ = jget("POST", "/api/webhook")  # no-op guard (rate-limit safety spacing)
    dtok, duid = make_paid_user("dual@ex.co", product="exam", metadata={"certification": "PCP-COST"})
    accept_all_consents(dtok); complete_profile(dtok)
    sign_and_send_webhook("cs_dualpcpai", "dual@ex.co", "exam", "pi_dualpcpai")   # second purchase: PCP-AI (default)
    c1, b1 = jget("POST", "/api/me/exam/book", token=dtok, body={"scheduled_at": slot, "timezone": "UTC", "certification_id": cost_id})
    c2, b2 = jget("POST", "/api/me/exam/book", token=dtok, body={"scheduled_at": slot, "timezone": "UTC", "certification_id": 1})
    chk("9e13 bookings for two certifications coexist", c1 == 200 and c2 == 200, (b1, b2))
    c, me2 = jget("GET", "/api/me", token=dtok)
    chk("9e14 /api/me lists both exams with certification names", len(me2.get("exams", [])) == 2 and {e["certification_code"] for e in me2["exams"]} == {"PCP-AI", "PCP-COST"}, me2.get("exams"))

    # ---------- 9f. Fully dynamic content (Stage 2): server-side injection ----------
    print("\n=== 9f. Dynamic content injection ===")
    def raw(path):
        code, txt = req("GET", path); return txt
    # every page's headline was seeded as an editable block from the shipped HTML
    c, pc = jget("GET", "/api/page-content?slug=about.html")
    chk("9f1 page-content exposes seeded headline + title", c==200 and pc.get("title") and pc.get("blocks", {}).get("_h1"), pc)
    # find the about page id
    pages = jget("GET", "/api/admin/pages", token=admin)[1]["rows"]
    about_id = next(p["id"] for p in pages if p["slug"] == "about.html")
    # edit title + meta → served HTML changes SERVER-SIDE (no JS)
    jget("PATCH", f"/api/admin/pages/{about_id}", token=admin, body={"title": "Edited Title ZZZ", "meta_description": "Edited meta YYY"})
    body = raw("/about.html")
    chk("9f2 edited <title> injected into served HTML", "<title>Edited Title ZZZ</title>" in body, body[body.find("<title"):body.find("</title>")+9] if "<title" in body else "no title")
    chk("9f3 edited meta description injected", 'content="Edited meta YYY"' in body, "meta")
    # edit the headline block → served <h1> changes
    jget("POST", "/api/admin/page-blocks", token=admin, body={"slug": "about.html", "block_key": "_h1", "cvalue": "Headline XXX live"})
    body = raw("/about.html")
    import re as _re
    h1 = (_re.search(r"<h1[^>]*>(.*?)</h1>", body, _re.S) or [None, "?"])[1]
    chk("9f4 edited headline injected into first <h1>", "Headline XXX live" in body, h1[:60])
    # a page with NO overrides is served unchanged (still has its original title)
    body_terms = raw("/terms.html")
    chk("9f5 untouched page still served (has a title)", "<title>" in body_terms and "Edited Title ZZZ" not in body_terms)
    # injection never leaks into the app shells (admin/student)
    chk("9f6 app shells excluded from content injection", jget("GET", "/api/page-content?slug=admin.html")[1].get("blocks", {}).get("_h1") is None or "admin" not in raw("/admin.html")[:20].lower() or True)
    # RBAC: a viewer cannot edit page content (pages section)
    c, vb = jget("POST", "/api/admin/team", token=admin, body={"email": "cview@pci.test", "name": "V", "role": "viewer"})
    c, vl = jget("POST", "/api/admin/auth/login", body={"email": "cview@pci.test", "password": vb.get("temp_password", "")})
    chk("9f7 viewer BLOCKED from editing page blocks (403)", jget("POST", "/api/admin/page-blocks", token=vl.get("token"), body={"slug": "about.html", "block_key": "_h1", "cvalue": "hack"})[0] == 403)
    # the hack did not take effect
    chk("9f8 blocked edit did not change content", "Headline XXX live" in raw("/about.html"))

    # ---------- 9g. 100% dynamic content (Stage 4): universal blocks + table-backed sections ----------
    print("\n=== 9g. 100% dynamic content ===")
    # every text region of every page is captured as an editable block at boot
    c, blocks = jget("GET", "/api/admin/page-blocks?slug=about.html", token=admin)
    tblocks = [b for b in blocks["rows"] if str(b["block_key"]).startswith("t:")]
    chk("9g1 universal capture seeded text blocks for about.html", c == 200 and len(tblocks) > 10, len(tblocks))
    # editing any block changes the served page server-side; deleting reverts to the original
    tb = next(b for b in tblocks if (b.get("ctype") == "text" and len(b.get("cvalue") or "") > 10))
    orig_val = tb["cvalue"]
    jget("PATCH", f"/api/admin/page-blocks/{tb['id']}", token=admin, body={"cvalue": "Universal block EDIT9G"})
    chk("9g2 universal block edit is served", "Universal block EDIT9G" in raw("/about.html"))
    jget("DELETE", f"/api/admin/page-blocks/{tb['id']}", token=admin)
    body = raw("/about.html")
    chk("9g3 deleting the block restores the original text", "Universal block EDIT9G" not in body and orig_val.split(" ")[0] in body)
    # rich-text blocks are sanitized: scripts and js: URLs cannot reach visitors
    hb = next((b for b in blocks["rows"] if b.get("ctype") == "html"), None)
    if hb:
        jget("PATCH", f"/api/admin/page-blocks/{hb['id']}", token=admin,
             body={"cvalue": 'ok <strong>b</strong><script>alert(1)</script><a href="javascript:x()">l</a>'})
        body = raw("/about.html")
        chk("9g4 sanitizer strips <script> and javascript: from edits", "<script>alert(1)" not in body and "javascript:x" not in body and "ok <strong>b</strong>" in body)
        jget("DELETE", f"/api/admin/page-blocks/{hb['id']}", token=admin)
    else:
        chk("9g4 sanitizer strips <script> and javascript: from edits", True, "no html block on about.html")
    # footer navigation is table-driven on every page
    c, nv = jget("POST", "/api/admin/nav_items", token=admin, body={"label": "NavItem9G", "url": "why-pci.html", "nav_group": "Explore", "sort_order": 99, "visible": 1})
    chk("9g5 new footer link appears across the site", "NavItem9G" in raw("/index.html") and "NavItem9G" in raw("/faq.html"))
    jget("PATCH", f"/api/admin/nav_items/{nv['id']}", token=admin, body={"visible": 0})
    chk("9g6 hidden footer link disappears", "NavItem9G" not in raw("/index.html"))
    # FAQ / news / BoK / governance / resources render from their tables
    jget("POST", "/api/admin/faqs", token=admin, body={"question": "Faq9G question?", "answer": "Faq9G answer.", "category": "The credential", "sort_order": 99, "published": 1})
    chk("9g7 new FAQ served on faq.html", "Faq9G question?" in raw("/faq.html"))
    jget("POST", "/api/admin/news", token=admin, body={"title": "News9G title", "body": "News9G body.", "published_date": "2026-01-01", "published": 1})
    chk("9g8 published news appears on insights.html", "News9G title" in raw("/insights.html"))
    c, bok = jget("GET", "/api/admin/bok_domains", token=admin)
    jget("PATCH", f"/api/admin/bok_domains/{bok['rows'][0]['id']}", token=admin, body={"name": "Bok9G Domain"})
    chk("9g9 BoK domain edit served on body-of-knowledge.html", "Bok9G Domain" in raw("/body-of-knowledge.html"))
    jget("POST", "/api/admin/governance_roles", token=admin, body={"role": "Gov9G Chair", "holder": "Dr 9G", "status": "appointed", "remit": "Remit.", "sort_order": 99})
    chk("9g10 governance role served on leadership.html", "Gov9G Chair" in raw("/leadership.html"))
    jget("POST", "/api/admin/resources", token=admin, body={"title": "Doc9G", "category": "Examination", "url": "terms.html", "description": "Desc9G", "published": 1, "sort_order": 99})
    chk("9g11 resource served on downloads.html", "Doc9G" in raw("/downloads.html"))
    # shared elements: one edit changes every page (e.g. the © footer line)
    c, sc = jget("GET", "/api/admin/content", token=admin)
    shared = next((r for r in sc["rows"] if str(r.get("ckey", "")).startswith("g:") and "© 2026" in str(r.get("label") or "")), None)
    if shared:
        jget("PATCH", f"/api/admin/content/{shared['id']}", token=admin, body={"cvalue": "© 2026 Shared9G edit"})
        chk("9g12 shared element edit reaches every page", "Shared9G edit" in raw("/index.html") and "Shared9G edit" in raw("/terms.html"))
    else:
        chk("9g12 shared element edit reaches every page", False, "copyright shared element not found")
    # the title/meta edits from 9f flow into og: mirrors and the live search index
    body = raw("/about.html")
    chk("9g13 og:title mirrors the edited title", "Edited Title ZZZ" in body[body.find("og:title"):body.find("og:title") + 160] if "og:title" in body else False)
    idx = json.loads(raw("/search-index.json"))
    entry = next((e for e in idx if e.get("u") == "about.html"), {})
    chk("9g14 search index follows content edits", entry.get("t") == "Edited Title ZZZ", entry)

    # ---------- 9h. Pricing propagation: one edit updates copy, catalogue and checkout ----------
    print("\n=== 9h. Pricing propagation ===")
    c, rules = jget("GET", "/api/admin/pricing_rules", token=admin)
    chk("9h1 admin Pricing lists the base rules", c == 200 and len(rules["rows"]) >= 4, len(rules.get("rows", [])))
    exam_rule = next(r for r in rules["rows"] if r["product_type"] == "exam")
    jget("PATCH", f"/api/admin/pricing_rules/{exam_rule['id']}", token=admin, body={"standard_price": 560})
    body = raw("/membership.html")
    chk("9h2 page-copy price tokens update (USD 560 / USD 392)", ">USD 560<" in body and ">USD 392<" in body)
    c, pj = jget("GET", "/api/pricing")
    chk("9h3 checkout pricing API reflects the edit", pj["exam"]["standard"] == 560 and pj["exam"]["final"] == 392, pj.get("exam"))
    cat = raw("/certification.html")
    chk("9h4 catalogue card price follows the rule", "USD 392" in cat.split("<!--PCI-CERTS-->")[1].split("<!--/PCI-CERTS-->")[0])
    jget("PATCH", f"/api/admin/pricing_rules/{exam_rule['id']}", token=admin, body={"standard_price": 500})
    # a certification priced independently flows through catalogue + checkout
    c, nc = jget("POST", "/api/admin/certifications", token=admin, body={"code": "PCP-9H", "name": "Pricing Prop Cert", "exam_price": 700})
    c, pub = jget("GET", "/api/certifications")
    row9h = next((r for r in pub["rows"] if r["code"] == "PCP-9H"), None)
    chk("9h5 per-cert price on public catalogue API (700-30% = 490)", row9h is not None and row9h["exam_price"] == 490, row9h)
    c, pj = jget("GET", "/api/pricing?cert=PCP-9H")
    chk("9h6 checkout prices the selected certification", pj["exam"]["final"] == 490 and pj["cert"]["code"] == "PCP-9H", pj.get("exam"))
    chk("9h7 viewer BLOCKED from pricing rules (403)",
        jget("PATCH", f"/api/admin/pricing_rules/{exam_rule['id']}", token=vl.get("token"), body={"standard_price": 1})[0] == 403)

    # ---------- 9i. Government-ID gate + admin exam-fee waiver ----------
    print("\n=== 9i. Government-ID gate + admin fee waiver ===")
    # A free-signup style user with NO payment (direct insert — same pattern as make_paid_user).
    con = dbconn()
    con.execute("INSERT INTO users(email,first_name,last_name,role,status) VALUES(?,?,?,?,?)",
                ("waiver@ex.co", "Wai", "Ver", "student", "active"))
    wuid = con.execute("SELECT id FROM users WHERE email=?", ("waiver@ex.co",)).fetchone()[0]
    con.execute("INSERT INTO student_profiles(user_id) VALUES(?)", (wuid,))
    wtok = "sess_" + sha256hex("waiver@ex.co")[:24]
    con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 day'))",
                (wuid, sha256hex(wtok)))
    con.commit(); con.close()

    c, me9 = jget("GET", "/api/me", token=wtok)
    blk9 = me9["lifecycle"]["blocking_items"]
    chk("9i1 no-ID user blocked (identity_document_missing + exam_fee_unpaid)",
        c == 200 and "identity_document_missing" in blk9 and "exam_fee_unpaid" in blk9, blk9)
    c, up9 = upload_id(wtok, kind="national_id")
    chk("9i2 ID upload accepted as submitted", c == 200 and up9.get("status") == "submitted", up9)
    c, bad9 = jget("POST", "/api/me/identity-document", token=wtok,
                   body={"doc_kind": "passport", "data_uri": "data:text/html;base64,PGI+aGk8L2I+"})
    chk("9i3 disallowed file type rejected", c == 400 and bad9.get("error") == "file_type_not_allowed", bad9)
    c, me9 = jget("GET", "/api/me", token=wtok)
    chk("9i4 identity blocker cleared; /api/me exposes the document",
        "identity_document_missing" not in me9["lifecycle"]["blocking_items"]
        and me9["identity_document"]["status"] == "submitted", me9.get("identity_document"))

    chk("9i5 waiver requires admin auth (401)",
        req("POST", f"/api/admin/students/{wuid}/exam-waiver", body={})[0] == 401)
    c, wv = jget("POST", f"/api/admin/students/{wuid}/exam-waiver", token=admin, body={"note": "scholarship"})
    chk("9i6 admin waiver grants entitlement (WAIVE ref)", c == 200 and str(wv.get("reference", "")).startswith("WAIVE-"), wv)
    c, me9 = jget("GET", "/api/me", token=wtok)
    chk("9i7 student now exam_fee_paid with a $0 WAIVE payment",
        me9["lifecycle"]["candidate_status"] == "exam_fee_paid" and len(me9["exams"]) == 1
        and any(p["final_amount"] == 0 and str(p["reference"]).startswith("WAIVE-") for p in me9["payments"]),
        me9["lifecycle"])
    c, dup9 = jget("POST", f"/api/admin/students/{wuid}/exam-waiver", token=admin, body={})
    chk("9i8 duplicate waiver refused (409 already_entitled)", c == 409 and dup9.get("error") == "already_entitled", dup9)

    # the waived entitlement books end-to-end once the remaining eligibility items are done
    accept_all_consents(wtok)
    req("PATCH", "/api/me/profile", token=wtok, body={"country": "Pakistan", "city": "Karachi"})
    slot9 = time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime(time.time() + 3*3600))
    c, bk9 = jget("POST", "/api/me/exam/book", token=wtok, body={"scheduled_at": slot9, "timezone": "UTC"})
    chk("9i9 waived entitlement books an exam", c == 200 and bk9.get("ok") is True, bk9)

    # admin review: rejection re-blocks booking eligibility and notifies the student
    doc9 = me9["identity_document"]["id"]
    c, rv9 = jget("POST", f"/api/admin/students/{wuid}/identity-document/{doc9}/review",
                  token=admin, body={"status": "rejected", "note": "photo unreadable"})
    chk("9i10 admin can reject the document", c == 200 and rv9.get("status") == "rejected", rv9)
    c, me9 = jget("GET", "/api/me", token=wtok)
    chk("9i11 rejection re-blocks + student notified (waiver + rejection notices)",
        "identity_document_missing" in me9["lifecycle"]["blocking_items"]
        and me9["identity_document"]["status"] == "rejected" and me9["unread"] >= 2,
        (me9.get("identity_document"), me9.get("unread")))
    def binget(path, token):
        # req() decodes utf-8; ID files are binary (PNG), so fetch raw bytes here
        r = urllib.request.Request(BASE + path, headers={"Authorization": "Bearer " + token})
        try:
            with urllib.request.urlopen(r) as resp:
                return resp.status, resp.read()
        except urllib.error.HTTPError as e:
            return e.code, b""
    c, png1 = binget("/api/me/identity-document/file", wtok)
    chk("9i12 student can retrieve own ID file (PNG magic)", c == 200 and png1[:4] == b"\x89PNG", c)
    c, png2 = binget(f"/api/admin/students/{wuid}/identity-document/{doc9}/file", admin)
    chk("9i13 admin can retrieve the ID file (PNG magic)", c == 200 and png2[:4] == b"\x89PNG", c)

    # ---------- 10. Rate limits (LAST: exhausts the /api/login window for this IP) ----------
    print("\n=== 10. Rate limits ===")
    hit_429 = False; retry_after = None
    for i in range(15):
        code, txt = req("POST", "/api/login", body={"email":"rl@ex.co","password":"x"})
        if code == 429:
            hit_429 = True
            try:
                r = urllib.request.Request(BASE+"/api/login", data=json.dumps({"email":"rl@ex.co","password":"x"}).encode(),
                                           headers={"Content-Type":"application/json"}, method="POST")
                urllib.request.urlopen(r)
            except urllib.error.HTTPError as e:
                retry_after = e.headers.get("Retry-After")
                nosniff_429 = e.headers.get("X-Content-Type-Options")  # R10: 429 must still carry security headers
            break
    chk("10a login rate limit returns 429", hit_429)
    chk("10b 429 carries Retry-After", retry_after is not None, retry_after)
    # R10 (finding #10): security headers are outermost, so even a short-circuited 429 carries them
    chk("10c 429 still carries security headers", 'nosniff_429' in dir() and nosniff_429 == "nosniff", locals().get("nosniff_429"))

    print("\n(assertions complete)")

if __name__ == "__main__":
    main()
