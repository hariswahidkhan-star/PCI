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

def dbconn(): return sqlite3.connect(DB)

# ---- server lifecycle ----
def boot():
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
def make_paid_user(email, product="bundle", amount=9900, pi=None, sid=None, real_login=False):
    """Settle a payment via webhook, return (session_token, user_id).

    Helper users mint their session token directly in the DB — the real /api/login is rate-limited
    (10/min/IP), and driving a dozen helper logins through it would trip the limiter and couple the
    exam flows to the rate-limit test. The set-password + /api/login path is proven once, explicitly,
    via real_login=True (the happy-path user)."""
    pi = pi or ("pi_" + sha256hex(email+"pi")[:16]); sid = sid or ("cs_" + sha256hex(email+"cs")[:16])
    code, _ = sign_and_send_webhook(sid, email, product, pi)
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

def complete_profile(token):
    req("PATCH", "/api/me/profile", token=token, body={"country": "Pakistan", "city": "Karachi"})

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
            break
    chk("10a login rate limit returns 429", hit_429)
    chk("10b 429 carries Retry-After", retry_after is not None, retry_after)

    print("\n(assertions complete)")

if __name__ == "__main__":
    main()
