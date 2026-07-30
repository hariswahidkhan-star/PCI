"""Phase 5 — privacy-safe public verification of a PCI World Passport by Student Number.

Transcribes the SQL and decision logic of Endpoints/WorldVerify.cs against a real SQLite database
built from the real schema.sql plus the pciworld tables exactly as Data/WorldSchema.cs and
Core/WorldIdentity.Ensure create them. What is under test is the privacy contract:

  • the ONE neutral answer — unknown, malformed, private, unpublished, withdrawn, expired,
    suspended (account OR participation), erased, deactivated, retired and quarantined numbers
    must all produce a BYTE-IDENTICAL response, so no state is distinguishable from "never existed"
  • exact match only — wildcard, prefix, range and SQL-metacharacter probes resolve nothing
  • normalisation is trim-surrounding + uppercase, and nothing else
  • the registry is the authority: merged numbers follow their recorded successor; drift between
    ledger and projection refuses to verify; a duplicated pre-registry number is ambiguous → neutral
  • a success shows ONLY owner-disclosed fields — switched-off fields are ABSENT, and no internal
    identifier, email or token material appears anywhere in the payload

Run:  python3 tests/world_passport_verify_test.py
"""
import json
import os
import re
import sqlite3
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
FAILURES = []

SHAPE = re.compile(r"^PCI-\d{4}-\d{6,}$")
NEUTRAL_MESSAGE = "No public PCI World Passport is currently available for that number."
DISCLAIMER = ("PCI World Passport records selected professional practice and challenge evidence. "
              "It is not, by itself, a PCI certification, examination result, accreditation, "
              "licence, or guarantee of professional competence.")


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


# ── database: the real schema.sql + the pciworld tables as WorldSchema.cs / WorldIdentity.Ensure
#    create them (CREATE TABLE + the AddCol upgrades, flattened) ─────────────────────────────────

WORLD_DDL = """
CREATE TABLE IF NOT EXISTS pciworld_users(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    email VARCHAR(190) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    display_name TEXT,
    status VARCHAR(16) NOT NULL DEFAULT 'active',
    email_verified INTEGER DEFAULT 0,
    passport_public INTEGER DEFAULT 0,
    passport_token_sha VARCHAR(64),
    passport_show_scores INTEGER DEFAULT 1,
    passport_show_profiles INTEGER DEFAULT 1,
    passport_show_dates INTEGER DEFAULT 1,
    passport_expires_at TEXT,
    passport_photo_ref VARCHAR(255),
    passport_photo_mime VARCHAR(32),
    student_user_id INTEGER,
    created_at TEXT DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_participants(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    status VARCHAR(16) NOT NULL DEFAULT 'active',
    created_at TEXT DEFAULT (datetime('now')));
CREATE UNIQUE INDEX IF NOT EXISTS ux_worldpart_user ON pciworld_participants(user_id);
CREATE TABLE IF NOT EXISTS pciworld_user_map(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    legacy_world_id INTEGER NOT NULL,
    canonical_user_id INTEGER,
    outcome VARCHAR(16) NOT NULL,
    created_at TEXT DEFAULT (datetime('now')));
CREATE TABLE IF NOT EXISTS pciworld_challenges(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    code VARCHAR(64) UNIQUE NOT NULL,
    title TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS pciworld_challenge_versions(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    challenge_id INTEGER NOT NULL,
    version INTEGER NOT NULL,
    title TEXT NOT NULL,
    industry VARCHAR(64),
    track VARCHAR(32),
    difficulty VARCHAR(16));
CREATE TABLE IF NOT EXISTS pciworld_attempts(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    session_id INTEGER NOT NULL,
    challenge_id INTEGER NOT NULL,
    version INTEGER NOT NULL,
    status VARCHAR(16) NOT NULL DEFAULT 'in_progress',
    score REAL,
    profile_key TEXT,
    result_token_sha VARCHAR(64),
    result_revoked INTEGER DEFAULT 0,
    completed_at VARCHAR(32),
    updated_at TEXT DEFAULT (datetime('now')),
    user_id INTEGER,
    passport_visible INTEGER DEFAULT 0,
    canonical_user_id INTEGER);
"""


def fresh_db():
    d = sqlite3.connect(":memory:")
    d.row_factory = sqlite3.Row
    d.executescript(open(os.path.join(HERE, "..", "schema.sql")).read())
    d.execute("CREATE UNIQUE INDEX IF NOT EXISTS ux_student_number_registry "
              "ON pci_student_number_registry(student_number)")
    d.execute("ALTER TABLE users ADD COLUMN is_test INTEGER DEFAULT 0")   # Migrate.cs AddCol
    d.executescript(WORLD_DDL)
    return d


# ── transcription of WorldVerify.Resolve ───────────────────────────────────────────────────────

def resolve(d, number):
    """Returns (world_row, student_number) or None — the exact chain WorldVerify.Resolve runs."""
    if len(number) > 32 or not SHAPE.match(number):
        return None
    uid, was_merged = None, False
    reg = d.execute("SELECT state,resolves_to_user_id FROM pci_student_number_registry "
                    "WHERE student_number=?", (number,)).fetchone()
    if reg is not None:
        if reg["state"] not in ("issued", "merged") or reg["resolves_to_user_id"] is None:
            return None
        uid, was_merged = reg["resolves_to_user_id"], reg["state"] == "merged"
    else:
        owners = d.execute("SELECT id FROM users WHERE registration_no=? LIMIT 2", (number,)).fetchall()
        if len(owners) != 1:
            return None
        uid = owners[0]["id"]
    u = d.execute("SELECT registration_no, role, status, COALESCE(is_test,0) AS is_test "
                  "FROM users WHERE id=?", (uid,)).fetchone()
    if u is None or u["role"] != "student" or u["status"] != "active" or u["is_test"] != 0:
        return None
    current = u["registration_no"]
    if not current or not current.strip():
        return None
    if not was_merged and current != number:
        return None
    part = d.execute("SELECT status FROM pciworld_participants WHERE user_id=?", (uid,)).fetchone()
    if part is None or part["status"] != "active":
        return None
    w = d.execute("""SELECT w.* FROM pciworld_users w
        WHERE (w.student_user_id=? OR EXISTS(
                SELECT 1 FROM pciworld_user_map m
                WHERE m.legacy_world_id=w.id AND m.canonical_user_id=?))
          AND w.status='active' AND w.passport_public=1 AND w.passport_token_sha IS NOT NULL
        ORDER BY w.id LIMIT 1""", (uid, uid)).fetchone()
    if w is None:
        return None
    exp = w["passport_expires_at"]
    if exp:  # WorldPassport.Expired: a set expiry at-or-before now withdraws the record
        past = d.execute("SELECT CASE WHEN ?<=datetime('now') THEN 1 ELSE 0 END p", (exp,)).fetchone()["p"]
        if past:
            return None
    return (w, current)


def canonical_for(d, wid):
    """WorldIdentity.CanonicalUserFor — map first, then the direct link."""
    m = d.execute("SELECT canonical_user_id FROM pciworld_user_map "
                  "WHERE legacy_world_id=? AND canonical_user_id IS NOT NULL", (wid,)).fetchone()
    if m is not None:
        return m["canonical_user_id"]
    w = d.execute("SELECT student_user_id FROM pciworld_users WHERE id=?", (wid,)).fetchone()
    return w["student_user_id"] if w and w["student_user_id"] is not None else None


def evidence(d, wid):
    """WorldAccount.EvidenceRows/EvidenceStats with visibleOnly=1 (union-ownership predicate)."""
    ck = canonical_for(d, wid)
    ck = ck if ck is not None else -1
    rows = d.execute("""SELECT a.id, a.score, a.profile_key, a.completed_at, a.passport_visible,
            a.result_token_sha, a.result_revoked, c.code, a.version,
            v.title, v.industry, v.track, v.difficulty
        FROM pciworld_attempts a
        JOIN pciworld_challenges c ON c.id=a.challenge_id
        JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
        WHERE (a.user_id=? OR a.canonical_user_id=?) AND a.status='completed' AND a.passport_visible=1
        ORDER BY a.completed_at DESC, a.id DESC LIMIT 20""", (wid, ck)).fetchall()
    stats = d.execute("""SELECT COUNT(*) AS c, COUNT(DISTINCT v.industry) AS i, COUNT(DISTINCT v.track) AS t
        FROM pciworld_attempts a
        JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
        WHERE (a.user_id=? OR a.canonical_user_id=?) AND a.status='completed' AND a.passport_visible=1""",
        (wid, ck)).fetchone()
    return rows, stats


def neutral():
    """The single neutral answer — one construction site, exactly like the endpoint."""
    return {"found": False, "message": NEUTRAL_MESSAGE}


def verify(d, raw, enabled=True):
    """The endpoint's decision + payload shaping (headers/throttle/jitter excluded)."""
    number = raw.strip().upper()
    if not number:
        return {"error": "missing_number"}
    hit = resolve(d, number) if enabled else None
    if hit is None:
        return neutral()
    w, student_number = hit
    show_scores, show_profiles, show_dates = (w["passport_show_scores"] == 1,
                                              w["passport_show_profiles"] == 1,
                                              w["passport_show_dates"] == 1)
    rows, stats = evidence(d, w["id"])
    items = []
    for r in rows:
        item = {"title": r["title"], "reference": f"{r['code']} · v{r['version']}",
                "industry": r["industry"], "track": r["track"], "difficulty": r["difficulty"]}
        if show_scores:
            item["score"] = None if r["score"] is None else round(r["score"], 1)
        if show_profiles:
            item["profile"] = (r["profile_key"] or "").replace("_", " ") or None
        if show_dates:
            item["completed_on"] = (r["completed_at"] or "").split(" ")[0]
        items.append(item)
    parts = [p for p, on in (("scores", show_scores), ("decision profiles", show_profiles),
                             ("dates", show_dates)) if on]
    return {
        "found": True,
        "student_number": student_number,
        "display_name": w["display_name"] or "PCI World participant",
        "passport": {"status": "published",
                     "discloses": ", ".join(parts) if parts else "challenge titles only",
                     "expires_on": (w["passport_expires_at"] or None) and w["passport_expires_at"].split(" ")[0],
                     "completed": stats["c"], "industries": stats["i"], "tracks": stats["t"]},
        "evidence": items,
        "evidence_total": stats["c"],
        "photo": None,
        "verify_page": "/world/verify",
        "verified_at": "<runtime>",
        "disclaimer": DISCLAIMER,
    }


# ── fixture builders ───────────────────────────────────────────────────────────────────────────

def mkstudent(d, uid, number, status="active", role="student", is_test=0):
    d.execute("INSERT INTO users(id,email,first_name,role,status,created_at,registration_no,is_test) "
              "VALUES(?,?,?,?,?, '2026-01-01 00:00:00', ?, ?)",
              (uid, f"u{uid}@pci.test", "T", role, status, number, is_test))
    if number:
        d.execute("INSERT INTO pci_student_number_registry(student_number,format_version,"
                  "original_user_id,resolves_to_user_id,state) VALUES(?,'legacy_v1',?,?,'issued')",
                  (number, uid, uid))


def mkworld(d, wid, uid, public=1, wstatus="active", token_sha="toksha", expires=None,
            scores=1, profiles=1, dates=1, name="Jordan Q. Practitioner", participant="active"):
    d.execute("INSERT INTO pciworld_users(id,email,password_hash,display_name,status,email_verified,"
              "passport_public,passport_token_sha,passport_show_scores,passport_show_profiles,"
              "passport_show_dates,passport_expires_at,student_user_id) VALUES(?,?,?,?,?,1,?,?,?,?,?,?,?)",
              (wid, f"w{wid}@pci.test", "x", name, wstatus, public, token_sha,
               scores, profiles, dates, expires, uid))
    d.execute("INSERT INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome) "
              "VALUES(?,?,'linked')", (wid, uid))
    if participant is not None:
        d.execute("INSERT INTO pciworld_participants(user_id,status) VALUES(?,?)", (uid, participant))


def mkevidence(d, wid, uid, attempt_id, score=82.5, completed="2026-06-01 09:00:00",
               visible=1, code="WC-EVM-001", title="Earned value under pressure"):
    ch = d.execute("SELECT id FROM pciworld_challenges WHERE code=?", (code,)).fetchone()
    if ch is None:
        cid = d.execute("INSERT INTO pciworld_challenges(code,title) VALUES(?,?)", (code, title)).lastrowid
        d.execute("INSERT INTO pciworld_challenge_versions(challenge_id,version,title,industry,track,"
                  "difficulty) VALUES(?,?,?,?,?,?)", (cid, 1, title, "Energy", "project_controls", "professional"))
    else:
        cid = ch["id"]
    d.execute("INSERT INTO pciworld_attempts(id,session_id,challenge_id,version,status,score,"
              "profile_key,completed_at,user_id,passport_visible,canonical_user_id,result_token_sha) "
              "VALUES(?,?,?,1,'completed',?,'steady_navigator',?,?,?,?,'resulttoksha')",
              (attempt_id, 900 + attempt_id, cid, score, completed, wid, visible, uid))


print("=" * 72)
print("PCI WORLD PASSPORT — PHASE 5 PUBLIC VERIFICATION BY STUDENT NUMBER")
print("=" * 72)

N1 = "PCI-2026-000001"

# ── A. the happy path disclosés only what the owner switched on ────────────────────────────────
d = fresh_db()
mkstudent(d, 1, N1)
mkworld(d, 10, 1)
mkevidence(d, 10, 1, 500)
r = verify(d, N1)
check("A1 an eligible, published, unexpired Passport verifies", r["found"] is True)
check("A2 the Student Number in the answer is the canonical projection", r["student_number"] == N1)
check("A3 the display name is the Passport name", r["display_name"] == "Jordan Q. Practitioner")
check("A4 the practice-evidence disclaimer is present verbatim", r["disclaimer"] == DISCLAIMER)
check("A5 evidence carries score/profile/date when all disclosures are on",
      r["evidence"][0].get("score") == 82.5 and r["evidence"][0].get("profile") == "steady navigator"
      and r["evidence"][0].get("completed_on") == "2026-06-01")
check("A6 stats count the published evidence", r["passport"]["completed"] == 1 and r["evidence_total"] == 1)

# hidden evidence never appears, even on a live Passport
mkevidence(d, 10, 1, 501, visible=0, code="WC-RSK-002", title="Hidden one")
r = verify(d, N1)
check("A7 evidence the owner kept private stays out of the count and the list",
      r["passport"]["completed"] == 1 and len(r["evidence"]) == 1)

# switched-off disclosure fields are ABSENT, not null
d2 = fresh_db()
mkstudent(d2, 1, N1)
mkworld(d2, 10, 1, scores=0, profiles=0, dates=0)
mkevidence(d2, 10, 1, 500)
r2 = verify(d2, N1)
check("A8 switched-off fields are absent from every evidence item",
      all(k not in r2["evidence"][0] for k in ("score", "profile", "completed_on")),
      str(sorted(r2["evidence"][0].keys())))
check("A9 the disclosure summary says titles only", r2["passport"]["discloses"] == "challenge titles only")

# nothing private leaks anywhere in the success payload
blob = json.dumps(r, ensure_ascii=False)
check("A10 no email, token hash or internal id appears in a success payload",
      "@pci.test" not in blob and "toksha" not in blob and "resulttoksha" not in blob
      and '"id"' not in blob and "user_id" not in blob, blob[:200])

# ── B. every non-disclosing state answers with ONE byte-identical neutral response ─────────────
baseline = json.dumps(neutral(), ensure_ascii=False, sort_keys=True).encode()

def neutral_case(label, dd, number, enabled=True):
    got = json.dumps(verify(dd, number, enabled), ensure_ascii=False, sort_keys=True).encode()
    check(label, got == baseline, got[:80].decode(errors="replace"))

neutral_case("B1 unknown number → neutral", fresh_db(), "PCI-2026-000042")

db_priv = fresh_db()
mkstudent(db_priv, 1, N1)
mkworld(db_priv, 10, 1, public=0, token_sha=None)
neutral_case("B2 never-published Passport → neutral", db_priv, N1)

db_unpub = fresh_db()
mkstudent(db_unpub, 1, N1)
mkworld(db_unpub, 10, 1, public=0, token_sha=None)   # UnpublishPassport: public=0, token cleared
mkevidence(db_unpub, 10, 1, 500)
neutral_case("B3 withdrawn (unpublished) Passport → neutral", db_unpub, N1)

db_exp = fresh_db()
mkstudent(db_exp, 1, N1)
mkworld(db_exp, 10, 1, expires="2020-01-01 00:00:00")
neutral_case("B4 expired Passport link → neutral", db_exp, N1)

db_wsusp = fresh_db()
mkstudent(db_wsusp, 1, N1)
mkworld(db_wsusp, 10, 1, wstatus="suspended")
neutral_case("B5 suspended World account → neutral", db_wsusp, N1)

db_psusp = fresh_db()
mkstudent(db_psusp, 1, N1)
mkworld(db_psusp, 10, 1, participant="suspended")
neutral_case("B6 suspended World participation → neutral", db_psusp, N1)

db_nopart = fresh_db()
mkstudent(db_nopart, 1, N1)
mkworld(db_nopart, 10, 1, participant=None)
neutral_case("B7 no World participation row → neutral", db_nopart, N1)

db_erase = fresh_db()
mkstudent(db_erase, 1, N1, status="erased")
mkworld(db_erase, 10, 1)
neutral_case("B8 erased canonical account → neutral", db_erase, N1)

db_deact = fresh_db()
mkstudent(db_deact, 1, N1, status="deactivated")
mkworld(db_deact, 10, 1)
neutral_case("B9 deactivated canonical account → neutral", db_deact, N1)

db_ret = fresh_db()   # retired WITHOUT successor: reservation stays, resolves to nobody
db_ret.execute("INSERT INTO pci_student_number_registry(student_number,format_version,"
               "original_user_id,resolves_to_user_id,state) VALUES(?,'legacy_v1',1,NULL,'retired')", (N1,))
neutral_case("B10 retired-without-successor number → neutral", db_ret, N1)

db_quar = fresh_db()
db_quar.execute("INSERT INTO pci_student_number_registry(student_number,format_version,"
                "original_user_id,resolves_to_user_id,state) VALUES(?,'legacy_v1',1,NULL,'quarantined')", (N1,))
neutral_case("B11 quarantined number → neutral", db_quar, N1)

db_test = fresh_db()
mkstudent(db_test, 1, N1, is_test=1)
mkworld(db_test, 10, 1)
neutral_case("B12 test-account number → neutral (parity with the credential register)", db_test, N1)

db_admin = fresh_db()
mkstudent(db_admin, 1, N1, role="admin")
mkworld(db_admin, 10, 1)
neutral_case("B13 non-student role → neutral", db_admin, N1)

db_off = fresh_db()
mkstudent(db_off, 1, N1)
mkworld(db_off, 10, 1)
neutral_case("B14 World feature disabled → the same neutral, not a different shape", db_off, N1, enabled=False)

neutral_case("B15 malformed input → the same neutral", fresh_db(), "PCI-BOGUS")

# and the neutral is not merely equal-shaped — it is the identical serialized object every time
check("B16 the neutral answer is byte-identical across ALL states (single construction site)",
      json.dumps(verify(db_priv, N1), sort_keys=True) == json.dumps(verify(db_ret, N1), sort_keys=True)
      == json.dumps(verify(fresh_db(), "PCI-2099-999999"), sort_keys=True)
      == json.dumps(neutral(), sort_keys=True))

# ── C. exact match only — no wildcard, prefix, range or bulk probing ───────────────────────────
d3 = fresh_db()
mkstudent(d3, 1, N1)
mkworld(d3, 10, 1)
for probe in ["PCI-2026-%", "PCI-2026-0000_1", "PCI-2026-00000*", "PCI-2026-",
              "PCI-2026", "PCI-", "%", "PCI-2026-00000", "PCI-2026-0000010",
              "PCI-2026-000001 OR 1=1", "PCI-2026-000001;--", "PCI-2026-000001%"]:
    if verify(d3, probe)["found"] is not False:
        check(f"C1 probe rejected: {probe!r}", False)
        break
else:
    check("C1 wildcard/prefix/range/injection probes all resolve nothing", True)
check("C2 the exact number still verifies after the probes", verify(d3, N1)["found"] is True)
check("C3 five-digit and truncated forms do not match (>=6 digits enforced)",
      verify(d3, "PCI-2026-00001")["found"] is False)

# ── D. normalisation: surrounding trim + uppercase, nothing else ───────────────────────────────
check("D1 surrounding whitespace is trimmed", verify(d3, "   PCI-2026-000001  \n")["found"] is True)
check("D2 lowercase input is uppercased", verify(d3, "pci-2026-000001")["found"] is True)
check("D3 internal whitespace is NOT repaired", verify(d3, "PCI- 2026-000001")["found"] is False)
check("D4 an empty number is a validation error, not a lookup",
      verify(d3, "   ") == {"error": "missing_number"})

# ── E. the registry is the authority ───────────────────────────────────────────────────────────
# merged-with-successor: the old number answers under the successor owner's CURRENT number
d4 = fresh_db()
OLD, NEW = "PCI-2024-000007", "PCI-2024-000008"
mkstudent(d4, 8, NEW)
mkworld(d4, 80, 8)
d4.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
           "resolves_to_user_id,state,merged_into_student_number) VALUES(?,'legacy_v1',7,8,'merged',?)",
           (OLD, NEW))
r4 = verify(d4, OLD)
check("E1 a merged number follows its recorded successor owner", r4["found"] is True)
check("E2 …and answers under the successor's current number", r4["student_number"] == NEW)

# issued-state drift: ledger says issued, projection moved → refuse to verify
d5 = fresh_db()
d5.execute("INSERT INTO users(id,email,role,status,created_at,registration_no) "
           "VALUES(5,'drift@pci.test','student','active','2026-01-01 00:00:00','PCI-2026-000099')")
d5.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
           "resolves_to_user_id,state) VALUES('PCI-2026-000005','legacy_v1',5,5,'issued')")
mkworld(d5, 50, 5)
check("E3 registry/projection drift refuses to verify (neutral)",
      json.dumps(verify(d5, "PCI-2026-000005"), sort_keys=True) == json.dumps(neutral(), sort_keys=True))

# pre-registry historic number: no ledger row, exactly one projection → verifies
d6 = fresh_db()
d6.execute("INSERT INTO users(id,email,role,status,created_at,registration_no) "
           "VALUES(6,'hist@pci.test','student','active','2020-01-01 00:00:00','PCI-2020-000006')")
mkworld(d6, 60, 6)
check("E4 a pre-registry historic number verifies via the exact projection",
      verify(d6, "PCI-2020-000006")["found"] is True)

# duplicated pre-registry number: ambiguous identity → neutral, never a guess
d7 = fresh_db()
for uid in (11, 12):
    d7.execute("INSERT INTO users(id,email,role,status,created_at,registration_no) "
               "VALUES(?,?,'student','active','2020-01-01 00:00:00','PCI-2020-000011')",
               (uid, f"dup{uid}@pci.test"))
mkworld(d7, 70, 11)
check("E5 a duplicated pre-registry number is ambiguous → neutral",
      json.dumps(verify(d7, "PCI-2020-000011"), sort_keys=True) == json.dumps(neutral(), sort_keys=True))

# a numberless canonical account can never verify (registry row pointing at it notwithstanding)
d8 = fresh_db()
d8.execute("INSERT INTO users(id,email,role,status,created_at) "
           "VALUES(9,'none@pci.test','student','active','2026-01-01 00:00:00')")
d8.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
           "resolves_to_user_id,state) VALUES('PCI-2026-000009','legacy_v1',9,9,'issued')")
mkworld(d8, 90, 9)
check("E6 a reservation whose owner carries no number → neutral",
      json.dumps(verify(d8, "PCI-2026-000009"), sort_keys=True) == json.dumps(neutral(), sort_keys=True))

# ── F. the union-ownership evidence predicate matches the Passport page ────────────────────────
d9 = fresh_db()
mkstudent(d9, 1, N1)
mkworld(d9, 10, 1)
# one row stamped only with the LEGACY world id, one only with the CANONICAL id — one each way
mkevidence(d9, 10, None, 600, code="WC-A-1", title="Legacy-stamped")
d9.execute("UPDATE pciworld_attempts SET canonical_user_id=NULL WHERE id=600")
mkevidence(d9, None, 1, 601, code="WC-B-2", title="Canonical-stamped")
d9.execute("UPDATE pciworld_attempts SET user_id=NULL WHERE id=601")
r9 = verify(d9, N1)
check("F1 evidence owned in either namespace is counted once each",
      r9["passport"]["completed"] == 2 and len(r9["evidence"]) == 2, str(r9["passport"]["completed"]))

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All Phase 5 public-verification assertions passed.")
