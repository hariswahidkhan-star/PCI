#!/usr/bin/env python3
"""
P0-1 regression suite — impersonation ("view as student") is centrally READ-ONLY.

Boots the REAL built backend against a throwaway SQLite DB, then proves that a staff
"view as student" (impersonation) session:

  • may READ everything the student can (GET /api/me works, banner flag set);
  • is refused on EVERY state-changing student route with a stable 403
    `impersonation_readonly` — enforced centrally, so even a route that has no
    per-endpoint guard (and even a non-existent /api route) fails closed;
  • causes NO database side effects for the blocked mutations (2FA secret never
    written, no erasure request created);
  • every blocked mutation is recorded in the impersonation ledger (audit trail);
  • a NORMAL student session token is NOT affected (mutations are not turned into
    `impersonation_readonly`).

Exit code 0 iff every assertion passes.  Run from backend/:
    python3 tests/impersonation_readonly_test.py
"""
import hashlib, json, os, socket, sqlite3, subprocess, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_impersonation.db")
STORAGE = os.path.join(HERE, "_impersonation_storage")

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

def sha256hex(s): return hashlib.sha256(s.encode()).hexdigest()

def dbconn():
    c = sqlite3.connect(DB, timeout=5); c.row_factory = sqlite3.Row; return c

def boot():
    for f in (DB, DB + "-wal", DB + "-shm"):
        try: os.remove(f)
        except OSError: pass
    env = dict(os.environ, DATABASE_FILE=DB, PORT=str(PORT), STORAGE_ROOT=STORAGE,
               ASPNETCORE_ENVIRONMENT="Development")
    log = open(os.path.join(HERE, "_impersonation_boot.log"), "wb")
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

# Every state-changing student route (method, path). Path params are placeholders — the central guard
# runs BEFORE endpoint routing, so the concrete id is irrelevant to the read-only decision.
STUDENT_MUTATIONS = [
    ("POST",   "/api/me/consents"),
    ("POST",   "/api/me/preferences"),
    ("PATCH",  "/api/me/profile"),
    ("POST",   "/api/me/identity-document"),
    ("POST",   "/api/me/2fa/setup"),
    ("POST",   "/api/me/2fa/verify"),
    ("POST",   "/api/me/2fa/disable"),
    ("POST",   "/api/me/sessions/revoke-others"),
    ("POST",   "/api/me/delete-request"),
    ("POST",   "/api/me/directory"),
    ("POST",   "/api/me/reviews"),
    ("POST",   "/api/me/appeals"),
    ("POST",   "/api/me/accommodations"),
    ("POST",   "/api/me/cpd"),
    ("POST",   "/api/me/cpd/declaration"),
    ("POST",   "/api/me/cpd/1/evidence"),
    ("DELETE", "/api/me/cpd/1"),
    ("POST",   "/api/me/documents/1/acknowledge"),
    ("POST",   "/api/me/documents/1/link"),
    ("POST",   "/api/me/events/1/register"),
    ("POST",   "/api/me/events/1/cancel"),
    ("POST",   "/api/me/exam/book"),
    ("POST",   "/api/me/exam/reschedule"),
    ("POST",   "/api/me/exam/start"),
    ("POST",   "/api/me/exam/submit"),
    ("POST",   "/api/me/exam/readiness"),
    ("POST",   "/api/me/exam/heartbeat"),
    ("POST",   "/api/me/exam/incident"),
    ("POST",   "/api/me/exam/launch-code"),
    ("POST",   "/api/me/readiness"),
    ("POST",   "/api/me/membership/portal"),
    ("POST",   "/api/me/membership/subscribe"),
    ("POST",   "/api/me/membership/upgrade"),
    ("POST",   "/api/me/founding-application"),
    ("POST",   "/api/me/messages/1/read"),
    ("POST",   "/api/me/messages/read-all"),
    ("POST",   "/api/me/tickets"),
    ("POST",   "/api/me/tickets/1/reply"),
    ("POST",   "/api/me/tickets/1/rate"),
    ("POST",   "/api/me/tickets/1/attachments"),
    ("POST",   "/api/me/certuvo/start"),
    ("POST",   "/api/me/certuvo/submit"),
    ("POST",   "/api/me/certuvo/resend"),
]

def main():
    proc = boot()
    try:
        # ---- owner login (+ forced first password change) ----
        c, b = jget("POST", "/api/admin/auth/login", body={"email": "owner@pci.local", "password": "changeme-owner"})
        admin = b["token"]
        jget("POST", "/api/admin/me/password", token=admin, body={"new_password": "OwnerPass99!"})

        # ---- create a real student (normal session token) ----
        email = "impersonation.subject@example.com"; pw = "Passw0rd!23"
        c, r = jget("POST", "/api/register", body={"firstName": "Imp", "lastName": "Subject",
                    "email": email, "password": pw, "confirmPassword": pw, "country": "United Kingdom"})
        student = r.get("token") if isinstance(r, dict) else None
        con = dbconn(); row = con.execute("SELECT id FROM users WHERE email=?", (email,)).fetchone(); con.close()
        chk("setup: student account created", row is not None, r)
        uid = row["id"]
        if not student:  # register did not return a token — mint one directly (deterministic)
            tok = "sess_" + sha256hex(email)[:24]
            con = dbconn(); con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 day'))", (uid, sha256hex(tok))); con.commit(); con.close()
            student = tok

        # ---- mint an impersonation ("view as student") session ----
        c, im = jget("POST", f"/api/admin/members/{uid}/impersonate", token=admin, body={"reason": "P0-1 regression"})
        chk("setup: impersonation session minted", c == 200 and im.get("token"), im)
        imp = im["token"]

        # ---- READ access is preserved under impersonation ----
        c, me = jget("GET", "/api/me", token=imp)
        chk("read: GET /api/me works under impersonation", c == 200 and me.get("user", {}).get("impersonated") is True, (c, me.get("user")))
        c, _ = jget("GET", "/api/me/documents", token=imp)
        chk("read: a student GET is not turned into impersonation_readonly", c != 403, c)

        # ---- every state-changing route is refused, centrally ----
        blocked = 0
        for method, path in STUDENT_MUTATIONS:
            c, j = jget(method, path, token=imp, body={} if method != "DELETE" else None)
            ok = c == 403 and isinstance(j, dict) and j.get("error") == "impersonation_readonly"
            chk(f"blocked: {method} {path}", ok, (c, j))
            if ok: blocked += 1

        # ---- fail-closed universality: even an unknown /api route is refused (proves it is not per-endpoint) ----
        c, j = jget("POST", "/api/me/__does_not_exist__", token=imp, body={})
        chk("fail-closed: unknown POST route still 403 impersonation_readonly",
            c == 403 and isinstance(j, dict) and j.get("error") == "impersonation_readonly", (c, j))

        # ---- NO database side effects from the blocked mutations ----
        con = dbconn()
        totp = con.execute("SELECT totp_secret FROM users WHERE id=?", (uid,)).fetchone()["totp_secret"]
        chk("no-effect: 2FA secret never written by blocked /2fa/setup", totp in (None, ""), totp)
        try:
            erasure = con.execute("SELECT COUNT(*) c FROM erasure_requests WHERE user_id=?", (uid,)).fetchone()["c"]
        except sqlite3.OperationalError:
            erasure = 0
        chk("no-effect: no erasure request created by blocked /delete-request", erasure == 0, erasure)
        # ---- blocked mutations are audited in the impersonation ledger ----
        sess = con.execute("SELECT id FROM impersonation_sessions WHERE user_id=? ORDER BY id DESC LIMIT 1", (uid,)).fetchone()
        events = con.execute("SELECT COUNT(*) c FROM impersonation_events WHERE session_id=? AND method LIKE '%[blocked]%'", (sess["id"],)).fetchone()["c"]
        con.close()
        chk("audit: blocked mutations recorded in the impersonation ledger", events >= blocked and blocked > 0, (events, blocked))

        # ---- a NORMAL student token is unaffected (not converted to impersonation_readonly) ----
        for method, path, body in [("POST", "/api/me/consents", {"accept_all": True}),
                                   ("POST", "/api/me/preferences", {}),
                                   ("POST", "/api/me/messages/read-all", {})]:
            c, j = jget(method, path, token=student, body=body)
            not_impersonation = not (isinstance(j, dict) and j.get("error") == "impersonation_readonly")
            chk(f"normal-token: {method} {path} not blocked as impersonation_readonly", c != 403 or not_impersonation, (c, j))

        print(f"\n  == {passed}/{passed+failed} PASSED ==")
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()

    raise SystemExit(1 if failed else 0)

if __name__ == "__main__":
    main()
