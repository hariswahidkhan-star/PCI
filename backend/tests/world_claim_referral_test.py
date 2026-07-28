"""Phase 7 — the share → challenge → anonymous response → claim journey, hardened.

Transcribes the SQL that Endpoints/World.cs, Endpoints/WorldAccount.cs (ClaimSession) and
Core/WorldAttempts.cs run, against a real SQLite database built from the real schema.sql plus the
pciworld tables exactly as Data/WorldSchema.cs creates them (CREATE TABLE + the AddCol upgrades,
flattened). What is under test is the set of invariants that make anonymous play safe to claim:

  • ONE WINNER — an anonymous attempt is claimed by exactly one account; the guarded UPDATE
    (WHERE user_id IS NULL) is the lock, proven both by sequenced updates and by a real
    two-connection interleaving on a file database
  • IDEMPOTENT CLAIM — the winner retrying (re-login, replayed claim) changes nothing and
    duplicates nothing
  • IMMUTABLE RESULTS — re-submitting a completed attempt (double click, network retry) never
    creates a second scored attempt or changes the stored score
  • BACKEND-ONLY SCORING — a client-supplied score field is never read; a tampered payload
    cannot alter the stored result
  • SAFE FAILURE — a revoked result share 404s; an invitation pins the immutable challenge
    version even after the challenge is edited; a revoked invitation resolves nothing
  • PRIVACY-SAFE REFERRALS — the pciworld_referrals row links share_ref (token sha) to a numeric
    session and pinned challenge coordinates only; claimed accounts appear as referred_user_id and
    nowhere else; the sharer reads aggregate counts, never who
  • REALM ISOLATION — the whole journey writes pciworld_* tables only; certification exam tables
    are untouched by construction, asserted with a SQLite authorizer

Run:  python3 tests/world_claim_referral_test.py
"""
import json
import os
import sqlite3
import sys
import tempfile

HERE = os.path.dirname(os.path.abspath(__file__))
FAILURES = []


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


def sha(raw):
    import hashlib
    return hashlib.sha256(raw.encode()).hexdigest()


# ── database: the real schema.sql + the pciworld tables as WorldSchema.cs creates them ─────────

WORLD_DDL = """
CREATE TABLE IF NOT EXISTS pciworld_sessions(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    token_sha VARCHAR(64) UNIQUE NOT NULL,
    created_at TEXT DEFAULT (datetime('now')),
    last_seen_at VARCHAR(32) DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_challenges(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    code VARCHAR(64) UNIQUE NOT NULL,
    title TEXT NOT NULL,
    difficulty VARCHAR(16) DEFAULT 'foundation',
    status VARCHAR(16) NOT NULL DEFAULT 'draft',
    retired INTEGER DEFAULT 0,
    current_version INTEGER DEFAULT 0,
    created_at TEXT DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_challenge_versions(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    challenge_id INTEGER NOT NULL,
    version INTEGER NOT NULL,
    title TEXT NOT NULL,
    config_json TEXT NOT NULL,
    created_at TEXT DEFAULT (datetime('now')));
CREATE UNIQUE INDEX IF NOT EXISTS ux_worldver ON pciworld_challenge_versions(challenge_id, version);
CREATE TABLE IF NOT EXISTS pciworld_attempts(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    session_id INTEGER NOT NULL,
    challenge_id INTEGER NOT NULL,
    version INTEGER NOT NULL,
    status VARCHAR(16) NOT NULL DEFAULT 'in_progress',
    answers_json TEXT,
    score REAL,
    dimensions_json TEXT,
    profile_key TEXT,
    display_name TEXT,
    result_token_sha VARCHAR(64),
    result_revoked INTEGER DEFAULT 0,
    invite_id INTEGER,
    parent_attempt_id INTEGER,
    started_at TEXT DEFAULT (datetime('now')),
    completed_at VARCHAR(32),
    updated_at TEXT DEFAULT (datetime('now')),
    user_id INTEGER,
    passport_visible INTEGER DEFAULT 0,
    hints_used INTEGER DEFAULT 0,
    rotation_period_id INTEGER,
    canonical_user_id INTEGER);
CREATE TABLE IF NOT EXISTS pciworld_invites(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    attempt_id INTEGER NOT NULL,
    token_sha VARCHAR(64) UNIQUE NOT NULL,
    inviter_name TEXT,
    revoked INTEGER DEFAULT 0,
    created_at TEXT DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_users(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    email VARCHAR(190) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    display_name TEXT,
    status VARCHAR(16) NOT NULL DEFAULT 'active',
    student_user_id INTEGER,
    created_at TEXT DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_user_map(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    legacy_world_id INTEGER NOT NULL,
    canonical_user_id INTEGER,
    outcome VARCHAR(16) NOT NULL,
    created_at TEXT DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_events(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    event VARCHAR(48) NOT NULL,
    challenge_id INTEGER,
    session_id INTEGER,
    created_at VARCHAR(32) DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_referrals(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    share_ref VARCHAR(64) NOT NULL,
    anonymous_world_session_id INTEGER NOT NULL,
    referred_user_id INTEGER,
    challenge_id INTEGER NOT NULL,
    challenge_version INTEGER NOT NULL,
    started_at TEXT DEFAULT (datetime('now')),
    completed_at VARCHAR(32),
    conversion_state VARCHAR(16) NOT NULL DEFAULT 'started',
    created_at TEXT DEFAULT (datetime('now')));
CREATE UNIQUE INDEX IF NOT EXISTS ux_worldref ON pciworld_referrals(share_ref, anonymous_world_session_id, challenge_id);
CREATE INDEX IF NOT EXISTS ix_worldref_session ON pciworld_referrals(anonymous_world_session_id);
"""


def fresh_db(path=":memory:"):
    d = sqlite3.connect(path)
    d.row_factory = sqlite3.Row
    d.executescript(open(os.path.join(HERE, "..", "schema.sql")).read())
    d.executescript(WORLD_DDL)
    return d


def mk_world(d):
    """One published challenge (v1), one anonymous session."""
    ch = d.execute("INSERT INTO pciworld_challenges(code,title,status,current_version) "
                   "VALUES('WC-T-001','T','published',1)").lastrowid
    d.execute("INSERT INTO pciworld_challenge_versions(challenge_id,version,title,config_json) VALUES(?,?,?,?)",
              (ch, 1, "T v1", json.dumps({"measures": {"cpi": 0.83, "spi": 0.91}, "tolerance": 0.01})))
    s = d.execute("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", (sha("anon-1"),)).lastrowid
    return ch, s


def mk_user(d, email):
    return d.execute("INSERT INTO pciworld_users(email,password_hash) VALUES(?,?)",
                     (email, "$2a$fake")).lastrowid


def mk_attempt(d, sess, ch, version=1, status="completed", user=None, score=None):
    return d.execute("INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,user_id,score,"
                     "answers_json,completed_at) VALUES(?,?,?,?,?,?, '{}', "
                     "CASE WHEN ?='completed' THEN datetime('now') END)",
                     (sess, ch, version, status, user, score, status)).lastrowid


# ── the transcribed production SQL ──────────────────────────────────────────────────────────────

def claim_session(d, user_id, session_id):
    """WorldAccount.ClaimSession — the guarded UPDATE is the lock, plus the referral stamp."""
    canonical = None  # WorldIdentity.CanonicalUserFor: no map row in these fixtures
    n1 = d.execute("UPDATE pciworld_attempts SET user_id=?, canonical_user_id=? "
                   "WHERE session_id=? AND user_id IS NULL", (user_id, canonical, session_id)).rowcount
    n2 = d.execute("UPDATE pciworld_referrals SET referred_user_id=?, "
                   "conversion_state=CASE WHEN completed_at IS NOT NULL THEN 'claimed' ELSE conversion_state END "
                   "WHERE anonymous_world_session_id=? AND referred_user_id IS NULL",
                   (user_id, session_id)).rowcount
    return n1, n2


def grade(config, answers):
    """WorldScore's numeric convention: graded against the AUTHORED references in the immutable
    snapshot, threshold max(0.01, tol*|ref|). Nothing the client sends besides the answers is read."""
    refs = config["measures"]
    tol = config.get("tolerance", 0.01)
    ok = sum(1 for k, want in refs.items()
             if isinstance(answers.get(k), (int, float))
             and abs(answers[k] - want) <= max(0.01, tol * abs(want)))
    return round(100.0 * ok / len(refs), 1)


def submit(d, session_id, user_id, attempt_id, body):
    """World.cs POST /attempts/{id}/submit + WorldAttempts.Submit, transcribed. `body` is the raw
    client payload; the endpoint reads ONLY body['answers'] — any client 'score' field is inert."""
    answers = body.get("answers") if isinstance(body.get("answers"), dict) else {}
    att = d.execute(f"SELECT * FROM pciworld_attempts WHERE id=? AND (session_id=?{'' if user_id is None else ' OR user_id=?'})",
                    (attempt_id, session_id) if user_id is None else (attempt_id, session_id, user_id)).fetchone()
    if att is None:
        return {"error": "not_found"}
    answers_raw = json.dumps(answers, sort_keys=True)

    def replay(row):
        stored = row["answers_json"] or "{}"
        if answers_raw == "{}" or json.loads(stored) == answers:
            return {"replayed": True, "score": row["score"]}
        return {"error": "already_submitted"}

    if att["status"] == "completed":
        return replay(att)
    snap = d.execute("SELECT * FROM pciworld_challenge_versions WHERE challenge_id=? AND version=?",
                     (att["challenge_id"], att["version"])).fetchone()
    score = grade(json.loads(snap["config_json"]), answers)   # server-computed, never body['score']
    n = d.execute("UPDATE pciworld_attempts SET status='completed', answers_json=?, score=?, "
                  "completed_at=datetime('now'), updated_at=datetime('now') "
                  "WHERE id=? AND status='in_progress'", (answers_raw, score, attempt_id)).rowcount
    if n == 0:
        raced = d.execute("SELECT * FROM pciworld_attempts WHERE id=?", (attempt_id,)).fetchone()
        return replay(raced) if raced and raced["status"] == "completed" else {"error": "not_found"}
    # Referral conversion (World.RecordReferralCompletion)
    d.execute("UPDATE pciworld_referrals SET completed_at=datetime('now'), "
              "conversion_state=CASE WHEN referred_user_id IS NULL THEN 'completed' ELSE 'claimed' END "
              "WHERE anonymous_world_session_id=? AND challenge_id=? AND challenge_version=? "
              "AND completed_at IS NULL", (att["session_id"], att["challenge_id"], att["version"]))
    return {"replayed": False, "score": score}


def referral_start(d, invite_token, session_id, user_id, challenge_id, version):
    """World.RecordReferralStart — revoked/mismatched invites record nothing; duplicates are one row."""
    inv = d.execute("SELECT a.challenge_id FROM pciworld_invites i JOIN pciworld_attempts a ON a.id=i.attempt_id "
                    "WHERE i.token_sha=? AND i.revoked=0", (sha(invite_token),)).fetchone()
    if inv is None or inv["challenge_id"] != challenge_id:
        return 0
    return d.execute("INSERT OR IGNORE INTO pciworld_referrals"
                     "(share_ref,anonymous_world_session_id,referred_user_id,challenge_id,challenge_version) "
                     "VALUES(?,?,?,?,?)", (sha(invite_token), session_id, user_id, challenge_id, version)).rowcount


def public_result(d, raw_token):
    """/world/r/{token} resolution — absent and revoked are indistinguishable."""
    return d.execute("SELECT * FROM pciworld_attempts WHERE result_token_sha=? AND result_revoked=0 "
                     "AND status='completed'", (sha(raw_token),)).fetchone()


def invite_pin(d, raw_token):
    """/world/i/{token} + the Start invite branch — the version the INVITER played, or nothing."""
    return d.execute("SELECT a.challenge_id, a.version FROM pciworld_invites i "
                     "JOIN pciworld_attempts a ON a.id=i.attempt_id "
                     "WHERE i.token_sha=? AND i.revoked=0", (sha(raw_token),)).fetchone()


print("=" * 72)
print("PCI WORLD — SHARE → CHALLENGE → ANONYMOUS RESPONSE → CLAIM (PHASE 7)")
print("=" * 72)

# ── A. one anonymous attempt is claimed by exactly ONE account ─────────────────────────────────
d = fresh_db()
ch, s = mk_world(d)
a1 = mk_attempt(d, s, ch)
a2 = mk_attempt(d, s, ch)
u1, u2 = mk_user(d, "winner@x.test"), mk_user(d, "loser@x.test")
n_win, _ = claim_session(d, u1, s)
n_lose, _ = claim_session(d, u2, s)
check("A1 the first claim wins every unclaimed attempt", n_win == 2, str(n_win))
check("A2 the losing claim is a 0-row no-op", n_lose == 0, str(n_lose))
owners = {r["user_id"] for r in d.execute("SELECT user_id FROM pciworld_attempts WHERE session_id=?", (s,))}
check("A3 the loser neither steals nor co-owns", owners == {u1}, str(owners))
check("A4 no attempt was duplicated by the race",
      d.execute("SELECT COUNT(*) c FROM pciworld_attempts").fetchone()["c"] == 2)

# The same race with REAL interleaving: two connections on a file database. The winner's guarded
# UPDATE inside an open transaction excludes the loser entirely (the write lock), and after commit
# the loser's identical guarded UPDATE finds 0 rows.
with tempfile.TemporaryDirectory() as tmp:
    path = os.path.join(tmp, "race.db")
    setup = fresh_db(path)
    chF, sF = mk_world(setup)
    mk_attempt(setup, sF, chF)
    uA, uB = mk_user(setup, "a@x.test"), mk_user(setup, "b@x.test")
    setup.commit()
    setup.close()
    connA = sqlite3.connect(path, isolation_level=None)
    connB = sqlite3.connect(path, isolation_level=None)
    connB.execute("PRAGMA busy_timeout=100")
    connA.execute("BEGIN IMMEDIATE")
    nA = connA.execute("UPDATE pciworld_attempts SET user_id=? WHERE session_id=? AND user_id IS NULL",
                       (uA, sF)).rowcount
    locked = False
    try:
        connB.execute("UPDATE pciworld_attempts SET user_id=? WHERE session_id=? AND user_id IS NULL", (uB, sF))
    except sqlite3.OperationalError:
        locked = True
    connA.execute("COMMIT")
    nB = connB.execute("UPDATE pciworld_attempts SET user_id=? WHERE session_id=? AND user_id IS NULL",
                       (uB, sF)).rowcount
    winner = connB.execute("SELECT user_id FROM pciworld_attempts WHERE session_id=?", (sF,)).fetchone()[0]
    check("A5 concurrent claim: the in-flight transaction excludes the second writer", locked)
    check("A6 concurrent claim: after commit the loser matches 0 rows", nA == 1 and nB == 0, f"{nA}/{nB}")
    check("A7 concurrent claim: ownership is the winner's, once", winner == uA)
    connA.close(); connB.close()

# ── B. the winner's claim is idempotent ────────────────────────────────────────────────────────
before = [tuple(r) for r in d.execute(
    "SELECT id,user_id,score,status FROM pciworld_attempts ORDER BY id")]
n_again, _ = claim_session(d, u1, s)          # re-login / retried claim
check("B1 a retried claim by the winner changes 0 rows", n_again == 0, str(n_again))
after = [tuple(r) for r in d.execute("SELECT id,user_id,score,status FROM pciworld_attempts ORDER BY id")]
check("B2 re-login duplicates nothing and moves nothing", before == after)

# ── C. duplicate submission protection ─────────────────────────────────────────────────────────
d2 = fresh_db()
ch2, s2 = mk_world(d2)
att = mk_attempt(d2, s2, ch2, status="in_progress")
right = {"answers": {"cpi": 0.83, "spi": 0.5}}
r1 = submit(d2, s2, None, att, right)
check("C1 first submit grades and completes", r1.get("replayed") is False and r1["score"] == 50.0, str(r1))
stored = d2.execute("SELECT score,completed_at,answers_json FROM pciworld_attempts WHERE id=?", (att,)).fetchone()
r2 = submit(d2, s2, None, att, right)          # double click / network retry
check("C2 an identical retry replays the stored result", r2.get("replayed") is True and r2["score"] == 50.0, str(r2))
r3 = submit(d2, s2, None, att, {})             # empty-body retry (lost response)
check("C3 an empty retry replays too", r3.get("replayed") is True, str(r3))
now = d2.execute("SELECT score,completed_at,answers_json FROM pciworld_attempts WHERE id=?", (att,)).fetchone()
check("C4 the stored score, answers and completion instant never moved", tuple(stored) == tuple(now))
check("C5 no second scored attempt exists",
      d2.execute("SELECT COUNT(*) c FROM pciworld_attempts").fetchone()["c"] == 1)
r4 = submit(d2, s2, None, att, {"answers": {"cpi": 0.83, "spi": 0.91}})   # a BETTER answer, after grading
check("C6 a different payload against a graded attempt is refused", r4.get("error") == "already_submitted", str(r4))
check("C7 the refusal changed nothing",
      d2.execute("SELECT score FROM pciworld_attempts WHERE id=?", (att,)).fetchone()["score"] == 50.0)

# ── D. scoring is backend-only ─────────────────────────────────────────────────────────────────
d3 = fresh_db()
ch3, s3 = mk_world(d3)
att3 = mk_attempt(d3, s3, ch3, status="in_progress")
tampered = {"answers": {"cpi": 1.0, "spi": 2.0, "score": 100}, "score": 100, "result": {"score": 100}}
r = submit(d3, s3, None, att3, tampered)
check("D1 a tampered payload is graded from the snapshot, never from its own score field",
      r["score"] == 0.0, str(r))
check("D2 the stored score is the server's",
      d3.execute("SELECT score FROM pciworld_attempts WHERE id=?", (att3,)).fetchone()["score"] == 0.0)
r = submit(d3, s3, None, att3, {"answers": {"cpi": 0.83, "spi": 0.91}, "score": 100})
check("D3 nor can a tampered resubmit rescue it", r.get("error") == "already_submitted", str(r))

# ── E. revoked / expired links fail safely; invitations pin the immutable version ─────────────
d4 = fresh_db()
ch4, s4 = mk_world(d4)
att4 = mk_attempt(d4, s4, ch4)
d4.execute("UPDATE pciworld_attempts SET result_token_sha=? WHERE id=?", (sha("share-tok"), att4))
check("E1 a live share token resolves", public_result(d4, "share-tok") is not None)
d4.execute("UPDATE pciworld_attempts SET result_revoked=1 WHERE id=?", (att4,))
check("E2 a revoked share token resolves nothing (404)", public_result(d4, "share-tok") is None)
check("E3 a never-existed token is indistinguishable from a revoked one",
      public_result(d4, "no-such-token") is None)

d4.execute("INSERT INTO pciworld_invites(attempt_id,token_sha) VALUES(?,?)", (att4, sha("inv-tok")))
# The author edits the challenge: v2 published with different content; the working copy moves on.
d4.execute("INSERT INTO pciworld_challenge_versions(challenge_id,version,title,config_json) VALUES(?,?,?,?)",
           (ch4, 2, "T v2 EDITED", json.dumps({"measures": {"cpi": 9.99}, "tolerance": 0.01})))
d4.execute("UPDATE pciworld_challenges SET current_version=2 WHERE id=?", (ch4,))
pin = invite_pin(d4, "inv-tok")
check("E4 the invitation still pins the version the inviter played", pin is not None and pin["version"] == 1,
      str(dict(pin)) if pin else "None")
snap = d4.execute("SELECT title FROM pciworld_challenge_versions WHERE challenge_id=? AND version=?",
                  (ch4, pin["version"])).fetchone()
check("E5 the pinned snapshot is the immutable original, not the edit", snap["title"] == "T v1")
check("E6 a plain (non-invite) start would use the new version",
      d4.execute("SELECT current_version FROM pciworld_challenges WHERE id=?", (ch4,)).fetchone()[0] == 2)
d4.execute("UPDATE pciworld_invites SET revoked=1 WHERE token_sha=?", (sha("inv-tok"),))
check("E7 a revoked invitation resolves nothing (404)", invite_pin(d4, "inv-tok") is None)

# ── F. referral attribution, privacy-safe ──────────────────────────────────────────────────────
d5 = fresh_db()
ch5, s_inviter = mk_world(d5)
inviter_att = mk_attempt(d5, s_inviter, ch5)
d5.execute("INSERT INTO pciworld_invites(attempt_id,token_sha,inviter_name) VALUES(?,?,?)",
           (inviter_att, sha("friend-inv"), "Aisha"))
cols = {r[1] for r in d5.execute("PRAGMA table_info(pciworld_referrals)")}
check("F1 the referral table has exactly the privacy-safe columns",
      cols == {"id", "share_ref", "anonymous_world_session_id", "referred_user_id", "challenge_id",
               "challenge_version", "started_at", "completed_at", "conversion_state", "created_at"},
      str(sorted(cols)))

s_friend = d5.execute("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", (sha("anon-friend"),)).lastrowid
friend_att = mk_attempt(d5, s_friend, ch5, status="in_progress")
referral_start(d5, "friend-inv", s_friend, None, ch5, 1)
row = d5.execute("SELECT * FROM pciworld_referrals").fetchone()
check("F2 starting from a share link records the linkage", row is not None and
      row["conversion_state"] == "started" and row["referred_user_id"] is None and
      row["challenge_id"] == ch5 and row["challenge_version"] == 1)
check("F3 the share reference is the opaque hash, never the raw token or a name",
      row["share_ref"] == sha("friend-inv") and
      all("friend-inv" != str(v) and "@" not in str(v) and "Aisha" not in str(v) for v in tuple(row)))
referral_start(d5, "friend-inv", s_friend, None, ch5, 1)   # refresh / double start
check("F4 a double start is one row, not two",
      d5.execute("SELECT COUNT(*) c FROM pciworld_referrals").fetchone()["c"] == 1)
check("F5 a revoked invite records no referral",
      referral_start(d5, "revoked-inv", s_friend, None, ch5, 1) == 0)

submit(d5, s_friend, None, friend_att, {"answers": {"cpi": 0.83, "spi": 0.91}})
row = d5.execute("SELECT * FROM pciworld_referrals").fetchone()
check("F6 completion advances the referral", row["conversion_state"] == "completed" and row["completed_at"])
first_completed = row["completed_at"]
submit(d5, s_friend, None, friend_att, {"answers": {"cpi": 0.83, "spi": 0.91}})   # replay
check("F7 a submit replay never advances the referral twice",
      d5.execute("SELECT completed_at FROM pciworld_referrals").fetchone()["completed_at"] == first_completed)

uf1, uf2 = mk_user(d5, "claimer@x.test"), mk_user(d5, "rival@x.test")
_, nr1 = claim_session(d5, uf1, s_friend)
_, nr2 = claim_session(d5, uf2, s_friend)
row = d5.execute("SELECT * FROM pciworld_referrals").fetchone()
check("F8 claim attributes the referral to exactly one account",
      nr1 == 1 and nr2 == 0 and row["referred_user_id"] == uf1 and row["conversion_state"] == "claimed")

# The sharer's view: the aggregate subselects from WorldAccount.Invitations — counts, never who.
agg = d5.execute("""SELECT
      (SELECT COUNT(*) FROM pciworld_referrals r WHERE r.share_ref=i.token_sha) AS ref_started,
      (SELECT COUNT(*) FROM pciworld_referrals r WHERE r.share_ref=i.token_sha AND r.completed_at IS NOT NULL) AS ref_completed,
      (SELECT COUNT(*) FROM pciworld_referrals r WHERE r.share_ref=i.token_sha AND r.referred_user_id IS NOT NULL) AS ref_claimed
    FROM pciworld_invites i WHERE i.token_sha=?""", (sha("friend-inv"),)).fetchone()
check("F9 the sharer reads aggregate counts only", tuple(agg) == (1, 1, 1), str(tuple(agg)))
check("F10 the aggregate projection carries no identity column",
      set(agg.keys()) == {"ref_started", "ref_completed", "ref_claimed"})
check("F11 analytics rows cannot name a user (no user column exists)",
      "user_id" not in {r[1] for r in d5.execute("PRAGMA table_info(pciworld_events)")})

# Claim-before-completion order: attribution is kept, and completion still lands on 'claimed'.
s_early = d5.execute("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", (sha("anon-early"),)).lastrowid
early_att = mk_attempt(d5, s_early, ch5, status="in_progress")
referral_start(d5, "friend-inv", s_early, None, ch5, 1)
claim_session(d5, uf1, s_early)
row = d5.execute("SELECT * FROM pciworld_referrals WHERE anonymous_world_session_id=?", (s_early,)).fetchone()
check("F12 claiming before completion keeps state 'started' with the account stamped",
      row["conversion_state"] == "started" and row["referred_user_id"] == uf1)
submit(d5, s_early, uf1, early_att, {"answers": {"cpi": 0.83, "spi": 0.91}})
row = d5.execute("SELECT * FROM pciworld_referrals WHERE anonymous_world_session_id=?", (s_early,)).fetchone()
check("F13 completing after the claim promotes straight to 'claimed'", row["conversion_state"] == "claimed")

# ── G. the whole journey writes pciworld_* tables only ─────────────────────────────────────────
d6 = fresh_db()
ch6, s6 = mk_world(d6)
inviter6 = mk_attempt(d6, s6, ch6)
d6.execute("UPDATE pciworld_attempts SET result_token_sha=? WHERE id=?", (sha("g-share"), inviter6))
d6.execute("INSERT INTO pciworld_invites(attempt_id,token_sha) VALUES(?,?)", (inviter6, sha("g-inv")))
exam_tables = ["exam_attempts", "exam_entitlements", "exam_bookings", "exam_score_snapshots",
               "issued_credentials", "practice_attempts", "exam_evidence", "exam_launch_codes"]
non_world = [r[0] for r in d6.execute(
    "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'pciworld%' AND name NOT LIKE 'sqlite_%'")]
snapshot = {t: d6.execute(f"SELECT COUNT(*) FROM {t}").fetchone()[0] for t in non_world}
writes = set()
d6.set_authorizer(lambda action, a1, a2, dbn, trg:
                  (writes.add(a1) or sqlite3.SQLITE_OK)
                  if action in (sqlite3.SQLITE_INSERT, sqlite3.SQLITE_UPDATE, sqlite3.SQLITE_DELETE)
                  else sqlite3.SQLITE_OK)

# The journey: friend opens the invite, starts, saves, double-submits, then signs up and claims.
s_g = d6.execute("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", (sha("g-anon"),)).lastrowid
g_att = mk_attempt(d6, s_g, ch6, status="in_progress")
referral_start(d6, "g-inv", s_g, None, ch6, 1)
d6.execute("UPDATE pciworld_attempts SET answers_json=?, updated_at=datetime('now') "
           "WHERE id=? AND session_id=? AND status='in_progress'", (json.dumps({"cpi": 0.8}), g_att, s_g))
submit(d6, s_g, None, g_att, {"answers": {"cpi": 0.83, "spi": 0.91}, "score": 100})
submit(d6, s_g, None, g_att, {"answers": {"cpi": 0.83, "spi": 0.91}, "score": 100})
g_user = mk_user(d6, "journey@x.test")
claim_session(d6, g_user, s_g)
claim_session(d6, mk_user(d6, "rival2@x.test"), s_g)
d6.set_authorizer(None)

bad = {t for t in writes if not t.startswith("pciworld_") and t != "sqlite_sequence"}
check("G1 every table the journey wrote is a pciworld_ table", bad == set(), str(sorted(bad)))
check("G2 no certification exam table gained or lost a row",
      all(d6.execute(f"SELECT COUNT(*) FROM {t}").fetchone()[0] == snapshot[t] for t in exam_tables))
check("G3 no non-World table changed at all",
      all(d6.execute(f"SELECT COUNT(*) FROM {t}").fetchone()[0] == snapshot[t] for t in non_world))
check("G4 and the journey really happened",
      tuple(d6.execute("SELECT status,score,user_id FROM pciworld_attempts WHERE id=?", (g_att,)).fetchone())
      == ("completed", 100.0, g_user) and
      d6.execute("SELECT conversion_state FROM pciworld_referrals WHERE anonymous_world_session_id=?",
                 (s_g,)).fetchone()[0] == "claimed")

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All share→challenge→claim journey assertions passed.")
