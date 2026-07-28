"""Phase 3 — symmetric portal handoff (World → MyPCI), the reverse of the portal→World SSO.

Transcribes the exact SQL of WorldAccount.CreatePortalHandoff and Account.RedeemPortalHandoff
against a real SQLite database built from the real schema.sql (plus the pciworld identity tables
as Data/WorldSchema.cs creates them). What is under test is the security contract the forward
direction already enforces, mirrored:

  • the code is stored ONLY as a SHA-256 hash, in the EXISTING login_tokens table
    (purpose='portal_handoff') — no new table, no raw material at rest
  • 90-second expiry; an expired code is dead
  • one redemption ever — the DELETE inside the transaction is the lock, so a replay
    (concurrent or later) consumes nothing and learns nothing
  • the user's status is re-validated AT REDEMPTION: a canonical account suspended between
    mint and redeem gets the SAME generic invalid_code as garbage, and no session is minted
  • identity_link_pending accounts (no canonical identity proven) cannot mint at all
  • a suspended canonical account cannot mint either
  • success mints a FRESH 30-day portal session (login_tokens purpose='session') with the
    /api/login response shape — never a copied or extended session, and never a World session
  • the return path is allow-listed to the portal (/app…) — never an open redirect
  • malformed, unknown, expired and replayed codes are indistinguishable failures

Run:  python3 tests/portal_handoff_test.py
"""
import hashlib
import os
import secrets
import sqlite3
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
FAILURES = []


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


def sha(s):
    # Security.Sha — SHA-256, lowercase hex.
    return hashlib.sha256(s.encode()).hexdigest()


# ── database: the real schema.sql + the pciworld identity tables (WorldSchema.cs, flattened) ────

WORLD_DDL = """
CREATE TABLE IF NOT EXISTS pciworld_users(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    email VARCHAR(190) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    display_name TEXT,
    status VARCHAR(16) NOT NULL DEFAULT 'active',
    email_verified INTEGER DEFAULT 0,
    student_user_id INTEGER,
    created_at TEXT DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_user_map(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    legacy_world_id INTEGER NOT NULL,
    canonical_user_id INTEGER,
    outcome VARCHAR(16) NOT NULL,
    created_at TEXT DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_user_sessions(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    token VARCHAR(64) NOT NULL,
    created_at TEXT DEFAULT (datetime('now')),
    expires_at TEXT);
"""


def fresh_db():
    d = sqlite3.connect(":memory:")
    d.isolation_level = None   # autocommit, so the redeem transcription's explicit BEGIN works
    d.row_factory = sqlite3.Row
    d.executescript(open(os.path.join(HERE, "..", "schema.sql")).read())
    d.executescript(WORLD_DDL)
    return d


def mk_student(d, email, status="active"):
    return d.execute(
        "INSERT INTO users(email,first_name,last_name,role,status,password_hash) VALUES(?,?,?,?,?,'x')",
        (email, "T", "S", "student", status)).lastrowid


def mk_world(d, email, student_user_id=None, mapped=True):
    wid = d.execute("INSERT INTO pciworld_users(email,password_hash,student_user_id) VALUES(?,?,?)",
                    (email, "x", student_user_id)).lastrowid
    if mapped and student_user_id is not None:
        d.execute("INSERT INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome) VALUES(?,?,'created')",
                  (wid, student_user_id))
    return wid


# ── transcription: WorldAccount.CreatePortalHandoff ─────────────────────────────────────────────

def canonical_user_for(d, world_uid):
    # WorldIdentity.CanonicalUserFor: the map first, then the direct student_user_id link.
    m = d.execute("SELECT canonical_user_id FROM pciworld_user_map "
                  "WHERE legacy_world_id=? AND canonical_user_id IS NOT NULL", (world_uid,)).fetchone()
    if m is not None:
        return m["canonical_user_id"]
    w = d.execute("SELECT student_user_id FROM pciworld_users WHERE id=?", (world_uid,)).fetchone()
    return None if w is None or w["student_user_id"] is None else w["student_user_id"]


def allowed_portal_return_to(p):
    if p is not None and p.startswith("/app") and not p.startswith("//") and "://" not in p:
        return p if len(p) <= 128 else p[:128]
    return "/app/"


def create_portal_handoff(d, world_uid, return_to=None):
    canonical = canonical_user_for(d, world_uid)
    if canonical is None:
        return ("identity_link_pending", "", "")
    cu = d.execute("SELECT status FROM users WHERE id=?", (canonical,)).fetchone()
    if cu is None or cu["status"] != "active":
        return ("account_suspended", "", "")
    code = secrets.token_hex(32)
    d.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) "
              "VALUES(?,?, 'portal_handoff', datetime('now','+90 seconds'))", (canonical, sha(code)))
    return (None, code, allowed_portal_return_to(return_to))


# ── transcription: Account.RedeemPortalHandoff + StartSession's session mint ────────────────────

def redeem_portal_handoff(d, code):
    if code is None or not code.strip() or len(code) < 32:
        return ("invalid_code", None)
    h = sha(code)
    user = None
    consumed = False
    # db.Transaction(() => { SELECT unexpired by hash+purpose; DELETE by id (the lock); re-read user })
    d.execute("BEGIN")
    try:
        row = d.execute("SELECT id, user_id FROM login_tokens "
                        "WHERE token=? AND purpose='portal_handoff' AND expires_at>datetime('now')",
                        (h,)).fetchone()
        if row is not None:
            consumed = d.execute("DELETE FROM login_tokens WHERE id=? AND purpose='portal_handoff'",
                                 (row["id"],)).rowcount == 1
            if consumed:
                user = d.execute("SELECT * FROM users WHERE id=?", (row["user_id"],)).fetchone()
        d.execute("COMMIT")
    except Exception:
        d.execute("ROLLBACK")
        raise
    if not consumed or user is None:
        return ("invalid_code", None)
    # Status re-validated at redemption time — suspended answers exactly like garbage.
    if user["status"] != "active":
        return ("invalid_code", None)
    return (None, user)


def start_session(d, user):
    # Account.StartSession / the /api/login mint: a FRESH 30-day hashed portal session.
    session = secrets.token_hex(32)
    d.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) "
              "VALUES(?,?, 'session', datetime('now','+30 day'))", (user["id"], sha(session)))
    return session, {"id": user["id"], "email": user["email"],
                     "firstName": user["first_name"], "lastName": user["last_name"]}


def redeem_endpoint(d, code):
    # The /api/portal-handoff/redeem composition: redeem, then mint, same shape as /api/login.
    err, user = redeem_portal_handoff(d, code)
    if err is not None:
        return {"status": 401, "error": "invalid_code"}
    token, u = start_session(d, user)
    return {"status": 200, "ok": True, "token": token, "user": u}


print("=" * 72)
print("PHASE 3 — SYMMETRIC PORTAL HANDOFF (WORLD → MYPCI)")
print("=" * 72)

# ── A. mint: hashed, purposed, short-lived, in the existing table ───────────────────────────────
print("\n-- A. minting --")
d = fresh_db()
student = mk_student(d, "a@x.test")
world = mk_world(d, "a@x.test", student)
err, code, return_to = create_portal_handoff(d, world)
check("A1 an active, linked World participant mints a code", err is None and len(code) == 64)
check("A2 the raw code is stored NOWHERE — only its SHA-256 hash",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE token=?", (code,)).fetchone()["c"] == 0
      and d.execute("SELECT COUNT(*) c FROM login_tokens WHERE token=?", (sha(code),)).fetchone()["c"] == 1)
row = d.execute("SELECT * FROM login_tokens WHERE token=?", (sha(code),)).fetchone()
check("A3 the row lives in the EXISTING login_tokens table with purpose='portal_handoff'",
      row["purpose"] == "portal_handoff" and row["user_id"] == student)
win = d.execute("SELECT (julianday(expires_at)-julianday('now'))*86400 s FROM login_tokens WHERE token=?",
                (sha(code),)).fetchone()["s"]
check("A4 expiry is 90 seconds out, not minutes", 80 <= win <= 91, f"{win:.0f}s")
check("A5 the default return path is the portal shell", return_to == "/app/")

# ── B. return path allow-list — never an open redirect ──────────────────────────────────────────
print("\n-- B. return path allow-list --")
for evil in ["https://evil.example/phish", "//evil.example", "/admin.html", "/world/account", None, ""]:
    _, _, rt = create_portal_handoff(d, world, evil)
    check(f"B1 {evil!r} collapses to /app/", rt == "/app/")
_, _, rt = create_portal_handoff(d, world, "/app/billing?product=membership")
check("B2 a genuine portal path rides through", rt == "/app/billing?product=membership")

# ── C. redemption: exact /api/login shape, fresh session ────────────────────────────────────────
print("\n-- C. redemption --")
d = fresh_db()
student = mk_student(d, "c@x.test")
world = mk_world(d, "c@x.test", student)
_, code, _ = create_portal_handoff(d, world)
before = d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='session'").fetchone()["c"]
r = redeem_endpoint(d, code)
check("C1 a valid code redeems", r["status"] == 200 and r.get("ok") is True)
check("C2 the response is the /api/login shape (token + user{id,email,firstName,lastName})",
      set(r["user"].keys()) == {"id", "email", "firstName", "lastName"}
      and r["user"]["email"] == "c@x.test" and isinstance(r["token"], str) and len(r["token"]) == 64)
check("C3 a FRESH portal session was minted (purpose='session', hashed)",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='session'").fetchone()["c"] == before + 1
      and d.execute("SELECT COUNT(*) c FROM login_tokens WHERE token=? AND purpose='session'",
                    (sha(r["token"]),)).fetchone()["c"] == 1)
sess_win = d.execute("SELECT julianday(expires_at)-julianday('now') dd FROM login_tokens WHERE token=?",
                     (sha(r["token"]),)).fetchone()["dd"]
check("C4 the minted session is the normal 30-day portal session", 29.9 <= sess_win <= 30.1, f"{sess_win:.2f}d")
check("C5 the handoff row is GONE — deleted, not flagged",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='portal_handoff'").fetchone()["c"] == 0)
check("C6 no World session was created or touched — sessions stay product-specific",
      d.execute("SELECT COUNT(*) c FROM pciworld_user_sessions").fetchone()["c"] == 0)

# ── D. one-use: replay is dead, concurrent replay races into 0 rows ─────────────────────────────
print("\n-- D. single use --")
r2 = redeem_endpoint(d, code)
check("D1 a second redemption of the same code is the generic 401",
      r2["status"] == 401 and r2["error"] == "invalid_code")
check("D2 the replay minted nothing",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='session'").fetchone()["c"] == before + 1)
# The concurrent shape: both requests SELECT the same row; the second DELETE hits 0 rows.
_, code2, _ = create_portal_handoff(d, world)
row = d.execute("SELECT id, user_id FROM login_tokens "
                "WHERE token=? AND purpose='portal_handoff' AND expires_at>datetime('now')",
                (sha(code2),)).fetchone()
first = d.execute("DELETE FROM login_tokens WHERE id=? AND purpose='portal_handoff'", (row["id"],)).rowcount
second = d.execute("DELETE FROM login_tokens WHERE id=? AND purpose='portal_handoff'", (row["id"],)).rowcount
check("D3 the DELETE is the lock: first wins (1 row), the racing duplicate gets 0",
      first == 1 and second == 0)

# ── E. expiry ───────────────────────────────────────────────────────────────────────────────────
print("\n-- E. expiry --")
_, code3, _ = create_portal_handoff(d, world)
d.execute("UPDATE login_tokens SET expires_at=datetime('now','-1 seconds') WHERE token=?", (sha(code3),))
r3 = redeem_endpoint(d, code3)
check("E1 an expired code answers the same generic 401", r3["status"] == 401 and r3["error"] == "invalid_code")
check("E2 expiry minted no session",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='session'").fetchone()["c"] == before + 1)

# ── F. suspended canonical user — blocked in both directions, generically ───────────────────────
print("\n-- F. suspension --")
d = fresh_db()
student = mk_student(d, "f@x.test")
world = mk_world(d, "f@x.test", student)
_, code4, _ = create_portal_handoff(d, world)
d.execute("UPDATE users SET status='suspended' WHERE id=?", (student,))
r4 = redeem_endpoint(d, code4)
check("F1 suspended-at-redemption gets the SAME generic 401 as garbage",
      r4["status"] == 401 and r4["error"] == "invalid_code")
check("F2 no session was minted for the suspended user",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='session'").fetchone()["c"] == 0)
check("F3 the code was still consumed — a dead code stays dead",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='portal_handoff'").fetchone()["c"] == 0)
err, _, _ = create_portal_handoff(d, world)
check("F4 a suspended canonical account cannot MINT either", err == "account_suspended")
check("F5 refusing to mint stored nothing",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='portal_handoff'").fetchone()["c"] == 0)

# ── G. identity_link_pending — no proven canonical identity, no crossing ────────────────────────
print("\n-- G. identity_link_pending --")
d = fresh_db()
pending = mk_world(d, "pending@x.test")   # no map row, no student_user_id → LinkPending
err, _, _ = create_portal_handoff(d, pending)
check("G1 an identity_link_pending account cannot mint (same rule as PublishPassport)",
      err == "identity_link_pending")
check("G2 the refusal stored nothing",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE purpose='portal_handoff'").fetchone()["c"] == 0)
# The map outcome may exist but be quarantined (canonical_user_id NULL) — still pending.
q = mk_world(d, "quarantine@x.test")
d.execute("INSERT INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome) VALUES(?,NULL,'quarantined')", (q,))
err, _, _ = create_portal_handoff(d, q)
check("G3 a quarantined map row (NULL canonical) is still pending", err == "identity_link_pending")

# ── H. failure shapes are indistinguishable ─────────────────────────────────────────────────────
print("\n-- H. one generic failure --")
d = fresh_db()
student = mk_student(d, "h@x.test")
world = mk_world(d, "h@x.test", student)
_, live, _ = create_portal_handoff(d, world)
replayed = live
redeem_endpoint(d, replayed)
answers = set()
for probe in [None, "", "short", "a" * 64, secrets.token_hex(32), replayed]:
    r = redeem_endpoint(d, probe)
    answers.add((r["status"], r.get("error")))
check("H1 malformed, unknown, garbage and replayed all answer identically",
      answers == {(401, "invalid_code")})

# ── I. purpose isolation: a handoff code is NOT a session, a session is NOT a handoff ───────────
print("\n-- I. purpose isolation --")
d = fresh_db()
student = mk_student(d, "i@x.test")
world = mk_world(d, "i@x.test", student)
_, code5, _ = create_portal_handoff(d, world)
# A stolen handoff HASH presented as a bearer session resolves nothing (purpose gate in Auth).
check("I1 a handoff row never authenticates as a session (purpose gate)",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE token=? AND purpose IN ('session','impersonation') "
                "AND expires_at>datetime('now')", (sha(code5),)).fetchone()["c"] == 0)
# An existing 30-day session token cannot be replayed through the handoff redeemer.
r = redeem_endpoint(d, code5)
sess_token = r["token"]
r5 = redeem_endpoint(d, sess_token)
check("I2 a portal SESSION token cannot be redeemed as a handoff code",
      r5["status"] == 401 and r5["error"] == "invalid_code")
check("I3 the session survives the failed probe untouched",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE token=? AND purpose='session'",
                (sha(sess_token),)).fetchone()["c"] == 1)

print("\n" + "=" * 72)
if FAILURES:
    print(f"RESULT: {len(FAILURES)} FAILURE(S)")
    for f in FAILURES:
        print("  ✗ " + f)
    sys.exit(1)
print("RESULT: ALL PASS")
