"""Phase 4 — the MyPCI Passport summary read model (Endpoints/PassportSummary.cs).

Transcribes the resolution and DTO SQL of PassportSummary.Build against a real SQLite database
built from the real schema.sql plus the pciworld tables exactly as Data/WorldSchema.cs and
Core/WorldIdentity.Ensure create them. What is under test is the one-read-model contract:

  • the summary's counts and status are IDENTICAL to what the World side computes for the same
    account (same EvidenceStats/EvidenceRows union-ownership SQL, same readiness expression)
  • no World participation → a well-formed not_created summary with availableActions=["create"],
    never an error and never a 404 shape
  • a quarantined email collision surfaces participantState="link_pending" — and leaks NOTHING
    of the unproven account (no name, no counts, no evidence)
  • draft / private / published / expired / suspended states derive exactly as the World
    dashboard derives them
  • only a token HASH is stored, so publicUrl is ABSENT and canShowPublicUrl is false — the
    summary never reconstructs or re-mints a published link
  • no users.id, pciworld id, email or token material appears anywhere in the payload

Run:  python3 tests/passport_summary_test.py
"""
import json
import os
import sqlite3
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
FAILURES = []


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
    d.executescript(WORLD_DDL)
    return d


# ── transcription of the EXISTING World-side functions the endpoint reuses ─────────────────────

def canonical_for(d, wid):
    """WorldIdentity.CanonicalUserFor — map first, then the direct link."""
    m = d.execute("SELECT canonical_user_id FROM pciworld_user_map "
                  "WHERE legacy_world_id=? AND canonical_user_id IS NOT NULL", (wid,)).fetchone()
    if m is not None:
        return m["canonical_user_id"]
    w = d.execute("SELECT student_user_id FROM pciworld_users WHERE id=?", (wid,)).fetchone()
    return w["student_user_id"] if w and w["student_user_id"] is not None else None


def evidence_stats(d, wid, visible_only):
    """WorldAccount.EvidenceStats — union-ownership predicate, whole-history aggregates."""
    ck = canonical_for(d, wid)
    ck = ck if ck is not None else -1
    extra = "AND a.passport_visible=1" if visible_only else ""
    return d.execute(f"""SELECT COUNT(*) AS c, COUNT(DISTINCT v.industry) AS i,
            COUNT(DISTINCT v.track) AS t
        FROM pciworld_attempts a
        JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
        WHERE (a.user_id=? OR a.canonical_user_id=?) AND a.status='completed' {extra}""",
        (wid, ck)).fetchone()


def evidence_rows(d, wid, visible_only, limit=200, offset=0):
    """WorldAccount.EvidenceRows — the publication-safe row shape, completed_at DESC."""
    ck = canonical_for(d, wid)
    ck = ck if ck is not None else -1
    extra = "AND a.passport_visible=1" if visible_only else ""
    return d.execute(f"""SELECT a.id, a.score, a.profile_key, a.completed_at, a.passport_visible,
            a.result_token_sha, a.result_revoked, c.code, a.version,
            v.title, v.industry, v.track, v.difficulty
        FROM pciworld_attempts a
        JOIN pciworld_challenges c ON c.id=a.challenge_id
        JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
        WHERE (a.user_id=? OR a.canonical_user_id=?) AND a.status='completed' {extra}
        ORDER BY a.completed_at DESC, a.id DESC LIMIT {limit} OFFSET {offset}""",
        (wid, ck)).fetchall()


def expired(d, w):
    """WorldPassport.Expired — a set expiry at-or-before now."""
    exp = w["passport_expires_at"]
    if not exp or not str(exp).strip():
        return False
    return d.execute("SELECT CASE WHEN ?<=datetime('now') THEN 1 ELSE 0 END p",
                     (exp,)).fetchone()["p"] == 1


def dashboard_passport_state(d, w):
    """WorldDashboard.Build's §7.6 readiness expression — the World side's own derivation."""
    wid = w["id"]
    visible = evidence_stats(d, wid, True)["c"]
    completed = evidence_stats(d, wid, False)["c"]
    is_public = w["passport_public"] == 1
    exp = expired(d, w)
    if is_public and not exp:
        return "published_active"
    if is_public and exp:
        return "expired"
    if completed == 0:
        return "private_empty"
    if w["email_verified"] == 1 and (w["display_name"] or "").strip() and visible > 0:
        return "private_ready"
    return "private_incomplete"


# ── transcription of PassportSummary.Build ─────────────────────────────────────────────────────

def student_number_read(d, uid):
    """StudentNumbers.Read — returns None rather than issuing."""
    r = d.execute("SELECT registration_no FROM users WHERE id=?", (uid,)).fetchone()
    return r["registration_no"] if r and r["registration_no"] else None


def build(d, student_id, student_email):
    number = student_number_read(d, student_id)
    w = d.execute("""SELECT w.* FROM pciworld_users w
        WHERE w.student_user_id=? OR EXISTS(
            SELECT 1 FROM pciworld_user_map m
            WHERE m.legacy_world_id=w.id AND m.canonical_user_id=?)
        ORDER BY w.id LIMIT 1""", (student_id, student_id)).fetchone()

    if w is None:
        pending = d.execute("""SELECT w.id FROM pciworld_users w
            LEFT JOIN pciworld_user_map m
                ON m.legacy_world_id=w.id AND m.canonical_user_id IS NOT NULL
            WHERE lower(w.email)=? AND w.student_user_id IS NULL AND m.id IS NULL""",
            ((student_email or "").strip().lower(),)).fetchone()
        return {
            "schemaVersion": 1, "studentNumber": number, "displayName": None,
            "participantState": "none" if pending is None else "link_pending",
            "passportState": "not_created",
            "completedChallengeCount": 0, "selectedEvidenceCount": 0,
            "evidenceHighlights": [],
            "disclosureSettings": {"scores": False, "profiles": False, "dates": False},
            "canShowPublicUrl": False, "expiresAt": None, "lastUpdatedAt": None,
            "availableActions": ["create" if pending is None else "link"],
        }

    wid = w["id"]
    part = d.execute("SELECT status FROM pciworld_participants WHERE user_id=?",
                     (student_id,)).fetchone()
    suspended = w["status"] != "active" or (part is not None and part["status"] != "active")
    stats_all = evidence_stats(d, wid, False)
    stats_sel = evidence_stats(d, wid, True)
    show = {"scores": w["passport_show_scores"] == 1,
            "profiles": w["passport_show_profiles"] == 1,
            "dates": w["passport_show_dates"] == 1}
    is_public = w["passport_public"] == 1
    exp = expired(d, w)
    email_verified = w["email_verified"] == 1
    has_name = bool((w["display_name"] or "").strip())

    if suspended:
        state = "suspended"
    elif is_public and not exp:
        state = "published"
    elif is_public:
        state = "expired"
    elif email_verified and has_name and stats_sel["c"] > 0:
        state = "private"
    else:
        state = "draft"

    highlights = []
    for r in evidence_rows(d, wid, True, limit=3):
        item = {"title": r["title"], "reference": f"{r['code']} · v{r['version']}",
                "industry": r["industry"], "track": r["track"], "difficulty": r["difficulty"]}
        if show["scores"] and r["score"] is not None:
            item["score"] = round(r["score"], 1)
        if show["profiles"] and r["profile_key"] is not None:
            item["profile"] = r["profile_key"].replace("_", " ")
        if show["dates"]:
            item["completedOn"] = (r["completed_at"] or "").split(" ")[0]
        highlights.append(item)

    newest = evidence_rows(d, wid, False, limit=1)
    actions = {"suspended": ["contact_support"], "published": ["manage"],
               "expired": ["manage", "renew"], "private": ["manage", "publish"],
               "draft": ["manage", "select_evidence"]}[state]

    return {
        "schemaVersion": 1, "studentNumber": number, "displayName": w["display_name"],
        "participantState": "active", "passportState": state,
        "completedChallengeCount": stats_all["c"], "selectedEvidenceCount": stats_sel["c"],
        "evidenceHighlights": highlights, "disclosureSettings": show,
        # only a HASH is stored — the raw URL cannot be reconstructed and is never re-minted
        "canShowPublicUrl": False, "expiresAt": (w["passport_expires_at"] or None)
            and w["passport_expires_at"].split(" ")[0],
        "lastUpdatedAt": newest[0]["completed_at"] if newest else None,
        "availableActions": actions,
    }


# ── fixture builders ───────────────────────────────────────────────────────────────────────────

def mkstudent(d, uid, email, number=None):
    d.execute("INSERT INTO users(id,email,first_name,role,status,created_at,registration_no) "
              "VALUES(?,?,?, 'student','active','2026-01-01 00:00:00', ?)", (uid, email, "T", number))


def mkworld(d, wid, uid, email=None, public=0, wstatus="active", token_sha=None, expires=None,
            scores=0, profiles=0, dates=0, name=None, verified=0, participant="active",
            link="direct"):
    d.execute("INSERT INTO pciworld_users(id,email,password_hash,display_name,status,email_verified,"
              "passport_public,passport_token_sha,passport_show_scores,passport_show_profiles,"
              "passport_show_dates,passport_expires_at,student_user_id) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?)",
              (wid, email or f"w{wid}@pci.test", "x", name, wstatus, verified, public, token_sha,
               scores, profiles, dates, expires, uid if link == "direct" else None))
    if uid is not None and link in ("direct", "map"):
        d.execute("INSERT INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome) "
                  "VALUES(?,?,'linked')", (wid, uid))
        if participant is not None:
            d.execute("INSERT INTO pciworld_participants(user_id,status) VALUES(?,?)",
                      (uid, participant))
    if link == "conflict":
        d.execute("INSERT INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome) "
                  "VALUES(?,NULL,'conflict')", (wid,))


def mkevidence(d, wid, uid, attempt_id, score=82.5, completed="2026-06-01 09:00:00",
               visible=1, code="WC-EVM-001", title="Earned value under pressure",
               industry="Energy", track="project_controls"):
    ch = d.execute("SELECT id FROM pciworld_challenges WHERE code=?", (code,)).fetchone()
    if ch is None:
        cid = d.execute("INSERT INTO pciworld_challenges(code,title) VALUES(?,?)",
                        (code, title)).lastrowid
        d.execute("INSERT INTO pciworld_challenge_versions(challenge_id,version,title,industry,"
                  "track,difficulty) VALUES(?,?,?,?,?,?)",
                  (cid, 1, title, industry, track, "professional"))
    else:
        cid = ch["id"]
    d.execute("INSERT INTO pciworld_attempts(id,session_id,challenge_id,version,status,score,"
              "profile_key,completed_at,user_id,passport_visible,canonical_user_id,result_token_sha) "
              "VALUES(?,?,?,1,'completed',?,'steady_navigator',?,?,?,?,'resultsha')",
              (attempt_id, 900 + attempt_id, cid, score, completed, wid, visible, uid))


print("=" * 72)
print("PCI WORLD PASSPORT — PHASE 4 MYPCI SUMMARY READ MODEL")
print("=" * 72)

N1 = "PCI-2026-000001"

# ── A. no World participation: a well-formed 200 summary, never an error shape ────────────────
d = fresh_db()
mkstudent(d, 1, "s1@pci.test", N1)
r = build(d, 1, "s1@pci.test")
check("A1 no participation → participantState none / passportState not_created",
      r["participantState"] == "none" and r["passportState"] == "not_created")
check("A2 …with availableActions exactly ['create']", r["availableActions"] == ["create"])
check("A3 …and the canonical Student Number from StudentNumbers.Read", r["studentNumber"] == N1)
check("A4 …and zero counts, no highlights, no expiry — a full DTO, not an error",
      r["completedChallengeCount"] == 0 and r["selectedEvidenceCount"] == 0
      and r["evidenceHighlights"] == [] and r["expiresAt"] is None and "error" not in r)
check("A5 a numberless student still gets a summary (studentNumber null, nothing issued)",
      (lambda d2: (build(d2, 2, "s2@pci.test")["studentNumber"] is None
                   and d2.execute("SELECT registration_no FROM users WHERE id=2").fetchone()[0] is None))(
          (lambda d2: (mkstudent(d2, 2, "s2@pci.test"), d2)[1])(fresh_db())))

# ── B. link_pending: quarantined email collision surfaced, nothing of the account leaked ──────
d = fresh_db()
mkstudent(d, 1, "taken@pci.test", N1)
mkworld(d, 10, None, email="taken@pci.test", name="Unproven Claimant", verified=1, link="conflict")
mkevidence(d, 10, None, 500)
r = build(d, 1, "taken@pci.test")
check("B1 quarantined collision → participantState link_pending", r["participantState"] == "link_pending")
check("B2 …passportState stays not_created (the account has not proven it is this person)",
      r["passportState"] == "not_created")
check("B3 …availableActions exactly ['link']", r["availableActions"] == ["link"])
check("B4 …and NOTHING of the unproven account leaks — no name, no counts, no evidence",
      r["displayName"] is None and r["completedChallengeCount"] == 0
      and r["evidenceHighlights"] == [])
blob = json.dumps(r, ensure_ascii=False)
check("B5 the link_pending payload carries no email and no internal id",
      "taken@pci.test" not in blob and "Unproven" not in blob and '"id"' not in blob)

# ── C. draft / private: the dashboard's own readiness expression, folded to the DTO enum ──────
d = fresh_db()
mkstudent(d, 1, "s1@pci.test", N1)
mkworld(d, 10, 1)                        # linked, unverified, no name, no evidence
r = build(d, 1, "s1@pci.test")
check("C1 an empty linked account is draft (dashboard: private_empty)",
      r["passportState"] == "draft" and dashboard_passport_state(
          d, d.execute("SELECT * FROM pciworld_users WHERE id=10").fetchone()) == "private_empty")
check("C2 …participantState active, actions manage+select_evidence",
      r["participantState"] == "active" and r["availableActions"] == ["manage", "select_evidence"])

mkevidence(d, 10, 1, 500)                # completed evidence but still unverified/unnamed
r = build(d, 1, "s1@pci.test")
check("C3 evidence without verified email + name is still draft (dashboard: private_incomplete)",
      r["passportState"] == "draft" and dashboard_passport_state(
          d, d.execute("SELECT * FROM pciworld_users WHERE id=10").fetchone()) == "private_incomplete")

d.execute("UPDATE pciworld_users SET email_verified=1, display_name='Jordan Q.' WHERE id=10")
r = build(d, 1, "s1@pci.test")
check("C4 verified + named + selected evidence is private (dashboard: private_ready)",
      r["passportState"] == "private" and dashboard_passport_state(
          d, d.execute("SELECT * FROM pciworld_users WHERE id=10").fetchone()) == "private_ready")
check("C5 …actions manage+publish", r["availableActions"] == ["manage", "publish"])

# ── D. published / expired / suspended ────────────────────────────────────────────────────────
d.execute("UPDATE pciworld_users SET passport_public=1, passport_token_sha='tokensha' WHERE id=10")
r = build(d, 1, "s1@pci.test")
check("D1 a published, unexpired Passport is published (dashboard: published_active)",
      r["passportState"] == "published" and dashboard_passport_state(
          d, d.execute("SELECT * FROM pciworld_users WHERE id=10").fetchone()) == "published_active")
check("D2 hash-only token → publicUrl ABSENT and canShowPublicUrl false — never re-minted",
      "publicUrl" not in r and r["canShowPublicUrl"] is False)
check("D3 no token material appears anywhere in the payload",
      "tokensha" not in json.dumps(r) and "resultsha" not in json.dumps(r))

d.execute("UPDATE pciworld_users SET passport_expires_at='2020-01-01 00:00:00' WHERE id=10")
r = build(d, 1, "s1@pci.test")
check("D4 an owner-set expiry in the past makes it expired (dashboard agrees)",
      r["passportState"] == "expired" and dashboard_passport_state(
          d, d.execute("SELECT * FROM pciworld_users WHERE id=10").fetchone()) == "expired")
check("D5 …actions manage+renew, expiry surfaced as a date", r["availableActions"] == ["manage", "renew"]
      and r["expiresAt"] == "2020-01-01")

d.execute("UPDATE pciworld_users SET passport_expires_at=NULL, status='suspended' WHERE id=10")
check("D6 a suspended World account reports suspended",
      build(d, 1, "s1@pci.test")["passportState"] == "suspended")
d.execute("UPDATE pciworld_users SET status='active' WHERE id=10")
d.execute("UPDATE pciworld_participants SET status='suspended' WHERE user_id=1")
r = build(d, 1, "s1@pci.test")
check("D7 a suspended participation row reports suspended too, actions contact_support",
      r["passportState"] == "suspended" and r["availableActions"] == ["contact_support"])

# ── E. parity: identical values to the World side's own computation for the same user ─────────
d = fresh_db()
mkstudent(d, 1, "s1@pci.test", N1)
mkworld(d, 10, 1, name="Jordan Q.", verified=1, scores=1, profiles=1, dates=1)
# one attempt owned in the LEGACY namespace only, one in the CANONICAL namespace only, one hidden
mkevidence(d, 10, None, 600, code="WC-A-1", title="Legacy-stamped", completed="2026-06-02 08:00:00")
mkevidence(d, None, 1, 601, code="WC-B-2", title="Canonical-stamped", industry="Rail",
           completed="2026-06-03 08:00:00")
mkevidence(d, 10, 1, 602, code="WC-C-3", title="Hidden one", visible=0,
           completed="2026-06-04 08:00:00")
r = build(d, 1, "s1@pci.test")
world_all = evidence_stats(d, 10, False)
world_sel = evidence_stats(d, 10, True)
check("E1 completedChallengeCount equals the World side's EvidenceStats(all) — union predicate",
      r["completedChallengeCount"] == world_all["c"] == 3)
check("E2 selectedEvidenceCount equals the World side's EvidenceStats(visible)",
      r["selectedEvidenceCount"] == world_sel["c"] == 2)
check("E3 highlights are DISCLOSED items only, newest first, top 3",
      [h["title"] for h in r["evidenceHighlights"]] == ["Canonical-stamped", "Legacy-stamped"])
check("E4 lastUpdatedAt is the newest completed evidence in either visibility",
      r["lastUpdatedAt"] == "2026-06-04 08:00:00")
check("E5 highlight fields honour full disclosure (score, profile, date present)",
      r["evidenceHighlights"][0].get("score") == 82.5
      and r["evidenceHighlights"][0].get("profile") == "steady navigator"
      and r["evidenceHighlights"][0].get("completedOn") == "2026-06-03")
check("E6 the readiness state agrees with the dashboard for the same account",
      r["passportState"] == "private" and dashboard_passport_state(
          d, d.execute("SELECT * FROM pciworld_users WHERE id=10").fetchone()) == "private_ready")

# switched-off disclosure removes the fields from the highlights, exactly like the public page
d.execute("UPDATE pciworld_users SET passport_show_scores=0, passport_show_profiles=0, "
          "passport_show_dates=0 WHERE id=10")
r = build(d, 1, "s1@pci.test")
check("E7 switched-off disclosure fields are ABSENT from highlights, not null",
      all(k not in r["evidenceHighlights"][0] for k in ("score", "profile", "completedOn")))
check("E8 disclosureSettings mirrors the stored switches",
      r["disclosureSettings"] == {"scores": False, "profiles": False, "dates": False})

# ── F. resolution through the MAP link alone (no student_user_id) still finds the account ─────
d = fresh_db()
mkstudent(d, 1, "s1@pci.test", N1)
mkworld(d, 10, 1, link="map", name="Map-linked", verified=1)
mkevidence(d, 10, 1, 700)
r = build(d, 1, "s1@pci.test")
check("F1 a map-resolved account (student_user_id NULL) is found and counted",
      r["participantState"] == "active" and r["completedChallengeCount"] == 1
      and r["displayName"] == "Map-linked")

# ── G. no internal identifier, email or hash anywhere in a full payload ───────────────────────
d = fresh_db()
mkstudent(d, 7, "leak@pci.test", "PCI-2026-000007")
mkworld(d, 77, 7, name="Jordan Q.", verified=1, scores=1, dates=1, public=1, token_sha="tokensha")
mkevidence(d, 77, 7, 800)
blob = json.dumps(build(d, 7, "leak@pci.test"), ensure_ascii=False)
check("G1 no users.id / world id / email / token material in the published payload",
      "leak@pci.test" not in blob and "tokensha" not in blob and "resultsha" not in blob
      and '"id"' not in blob and "user_id" not in blob and '"77"' not in blob,
      blob[:200])
check("G2 the Student Number IS present — it is the one public identifier",
      '"PCI-2026-000007"' in blob)

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All Phase 4 Passport-summary assertions passed.")
