"""Phase 2 (part 3) — the maker-checker duplicate-account merge workflow (spec §4.11).

Transcribes the SQL that Core/IdentityMerge.cs runs, against a real SQLite database built from the
real schema.sql (plus the Migrate-installed world/event tables the merge touches). What is under
test is the two-person control and the execution invariants:

  • the maker can NEVER approve their own request, even holding both permissions
  • a different admin can, and the merge executes atomically
  • the survivor keeps their Student Number untouched
  • the loser's number goes to state='merged' and resolves to the survivor
  • both accounts' sessions and outstanding handoff codes are revoked
  • the loser's email is freed for reuse
  • a decided request can never be decided again (409 already_decided)
  • per-table uniqueness collisions keep the survivor's row and mark the loser's
  • reject is terminal

Run:  python3 tests/identity_merge_test.py
"""
import json
import os
import sqlite3
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
FAILURES = []

# Tables installed by Migrate.cs / WorldSchema.cs / WorldIdentity.cs rather than schema.sql —
# transcribed to the columns the merge actually reads and writes.
EXTRA_DDL = """
CREATE TABLE IF NOT EXISTS event_registrations(id INTEGER PRIMARY KEY AUTOINCREMENT,
  event_id INTEGER NOT NULL,user_id INTEGER NOT NULL,status TEXT DEFAULT 'registered',
  registered_at TEXT DEFAULT (datetime('now')),attended_at TEXT,cpd_entry_id INTEGER);
CREATE UNIQUE INDEX IF NOT EXISTS ux_event_reg ON event_registrations(event_id,user_id);
CREATE TABLE IF NOT EXISTS pciworld_users(id INTEGER PRIMARY KEY AUTOINCREMENT,
  email TEXT,student_user_id INTEGER);
CREATE TABLE IF NOT EXISTS pciworld_user_map(id INTEGER PRIMARY KEY AUTOINCREMENT,
  legacy_world_id INTEGER NOT NULL,canonical_user_id INTEGER,outcome TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS pciworld_participants(id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL);
CREATE UNIQUE INDEX IF NOT EXISTS ux_worldpart_user ON pciworld_participants(user_id);
CREATE TABLE IF NOT EXISTS pciworld_attempts(id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER,canonical_user_id INTEGER,status TEXT,passport_visible INTEGER DEFAULT 0);
CREATE TABLE IF NOT EXISTS pciworld_handoff_codes(id INTEGER PRIMARY KEY AUTOINCREMENT,
  code_sha TEXT UNIQUE NOT NULL,world_user_id INTEGER NOT NULL,expires_at TEXT NOT NULL,
  consumed_at TEXT);
"""

BLANKET_TABLES = ["payments", "exam_entitlements", "exam_bookings", "exam_attempts",
                  "issued_credentials", "memberships", "founding_applications"]


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


def fresh_db():
    d = sqlite3.connect(":memory:", isolation_level=None)   # autocommit; approve() manages BEGIN/COMMIT itself
    d.row_factory = sqlite3.Row
    d.executescript(open(os.path.join(HERE, "..", "schema.sql")).read())
    d.execute("CREATE UNIQUE INDEX IF NOT EXISTS ux_student_number_registry "
              "ON pci_student_number_registry(student_number)")
    d.executescript(EXTRA_DDL)
    return d


def mkuser(d, uid, email, number=None, status="active", role="student"):
    d.execute("INSERT INTO users(id,email,first_name,role,status,created_at,registration_no) "
              "VALUES(?,?,?,?,?, '2026-01-01 00:00:00',?)", (uid, email, "T", role, status, number))
    if number:
        d.execute("INSERT INTO pci_student_number_registry(student_number,format_version,"
                  "original_user_id,resolves_to_user_id,state) VALUES(?, 'legacy_v1',?,?,'issued')",
                  (number, uid, uid))


# ── transcriptions of Core/IdentityMerge.cs ────────────────────────────────────────────────────

def create_request(d, source, target, reason, requested_by):
    if source == target:
        return 0, "self_merge"
    src = d.execute("SELECT id,role,status FROM users WHERE id=?", (source,)).fetchone()
    dst = d.execute("SELECT id,role,status FROM users WHERE id=?", (target,)).fetchone()
    if src is None or dst is None:
        return 0, "unknown_user"
    if src["role"] != "student" or dst["role"] != "student":
        return 0, "not_student"
    if src["status"] in ("erased", "merged") or dst["status"] in ("erased", "merged"):
        return 0, "not_mergeable"
    pending = d.execute("SELECT COUNT(*) c FROM pci_identity_merges WHERE status='pending' "
                        "AND (source_user_id IN (?,?) OR target_user_id IN (?,?))",
                        (source, target, source, target)).fetchone()["c"]
    if pending > 0:
        return 0, "already_pending"
    cur = d.execute("INSERT INTO pci_identity_merges(source_user_id,target_user_id,reason,status,"
                    "requested_by,correlation_id) VALUES(?,?,?,'pending',?,?)",
                    (source, target, reason, requested_by, f"corr{source}x{target}"))
    return cur.lastrowid, None


def counts(d, uid):
    c = {}
    c["applications"] = d.execute("SELECT COUNT(*) c FROM founding_applications WHERE user_id=?", (uid,)).fetchone()["c"]
    for t, k in [("payments", "payments"), ("exam_entitlements", "exam_entitlements"),
                 ("exam_bookings", "exam_bookings"), ("exam_attempts", "exam_attempts"),
                 ("issued_credentials", "credentials"), ("memberships", "memberships"),
                 ("cpd_entries", "cpd_entries"), ("event_registrations", "event_registrations"),
                 ("student_profiles", "student_profiles"), ("login_tokens", "sessions")]:
        c[k] = d.execute(f"SELECT COUNT(*) c FROM {t} WHERE user_id=?", (uid,)).fetchone()["c"]
    c["world_attempts"] = d.execute("SELECT COUNT(*) c FROM pciworld_attempts WHERE canonical_user_id=?", (uid,)).fetchone()["c"]
    c["passport_evidence"] = d.execute("SELECT COUNT(*) c FROM pciworld_attempts WHERE canonical_user_id=? "
                                       "AND status='completed' AND passport_visible=1", (uid,)).fetchone()["c"]
    c["handoff_codes"] = d.execute("""SELECT COUNT(*) c FROM pciworld_handoff_codes WHERE world_user_id IN (
        SELECT w.id FROM pciworld_users w WHERE w.student_user_id=?
        UNION SELECT m.legacy_world_id FROM pciworld_user_map m WHERE m.canonical_user_id=?)""",
                                   (uid, uid)).fetchone()["c"]
    return c


def snapshot(d, sid, tid):
    out = {}
    for key, uid in (("source", sid), ("target", tid)):
        u = d.execute("SELECT id,email,registration_no,status FROM users WHERE id=?", (uid,)).fetchone()
        out[key] = {"user_id": u["id"], "email": u["email"], "registration_no": u["registration_no"],
                    "status": u["status"], "counts": counts(d, uid)}
    return out


def approve(d, merge_id, approver, note=None):
    m = d.execute("SELECT * FROM pci_identity_merges WHERE id=?", (merge_id,)).fetchone()
    if m is None:
        return "not_found", None
    if m["requested_by"] == approver:
        return "maker_checker", None
    if m["status"] != "pending":
        return "already_decided", None
    sid, tid, corr = m["source_user_id"], m["target_user_id"], m["correlation_id"]
    try:
        d.execute("BEGIN")
        claimed = d.execute("UPDATE pci_identity_merges SET status='approved', decided_by=?, "
                            "decided_at=datetime('now'), decision_note=?, row_version=row_version+1 "
                            "WHERE id=? AND status='pending'", (approver, note, merge_id)).rowcount
        if claimed == 0:
            raise RuntimeError("already_decided")
        # lock/read both users, lower id first
        lo, hi = (sid, tid) if sid < tid else (tid, sid)
        d.execute("UPDATE users SET updated_at=updated_at WHERE id=?", (lo,))
        d.execute("UPDATE users SET updated_at=updated_at WHERE id=?", (hi,))
        src = d.execute("SELECT id,email,registration_no,status,role FROM users WHERE id=?", (sid,)).fetchone()
        dst = d.execute("SELECT id,email,registration_no,status,role FROM users WHERE id=?", (tid,)).fetchone()
        if src is None or dst is None:
            raise RuntimeError("unknown_user")
        if (src["status"] in ("erased", "merged") or dst["status"] in ("erased", "merged")
                or src["role"] != "student" or dst["role"] != "student"):
            raise RuntimeError("not_mergeable")
        loser_email, loser_no, survivor_no = src["email"], src["registration_no"], dst["registration_no"]
        before = snapshot(d, sid, tid)

        # handoff codes die for BOTH accounts, before world links are re-pointed
        d.execute("""DELETE FROM pciworld_handoff_codes WHERE world_user_id IN (
            SELECT w.id FROM pciworld_users w WHERE w.student_user_id IN (?,?)
            UNION SELECT m2.legacy_world_id FROM pciworld_user_map m2 WHERE m2.canonical_user_id IN (?,?))""",
                  (sid, tid, sid, tid))

        # loser's number → merged, resolving to the survivor
        if loser_no:
            changed = d.execute("""UPDATE pci_student_number_registry SET state='merged',
                merged_into_student_number=?, resolves_to_user_id=?, changed_at=datetime('now'),
                reason_code='duplicate_merge', changed_by_admin_id=?, correlation_id=?,
                row_version=row_version+1 WHERE student_number=?""",
                                (survivor_no, tid, approver, corr, loser_no)).rowcount
            if changed == 0:
                d.execute("""INSERT INTO pci_student_number_registry(student_number,format_version,
                    original_user_id,resolves_to_user_id,state,merged_into_student_number,
                    reason_code,changed_by_admin_id,correlation_id)
                    VALUES(?, 'legacy_v1',?,?,'merged',?,'duplicate_merge',?,?)""",
                          (loser_no, sid, tid, survivor_no, approver, corr))

        for t in BLANKET_TABLES:
            d.execute(f"UPDATE {t} SET user_id=? WHERE user_id=?", (tid, sid))

        # cpd: manual entries wholesale; event-credited only where the survivor holds no credit
        d.execute("UPDATE cpd_entries SET user_id=? WHERE user_id=? AND source_event_id IS NULL", (tid, sid))
        survivor_events = {r["e"] for r in d.execute(
            "SELECT source_event_id e FROM cpd_entries WHERE user_id=? AND source_event_id IS NOT NULL", (tid,))}
        for r in d.execute("SELECT id,source_event_id FROM cpd_entries WHERE user_id=? "
                           "AND source_event_id IS NOT NULL", (sid,)).fetchall():
            if r["source_event_id"] not in survivor_events:
                survivor_events.add(r["source_event_id"])
                d.execute("UPDATE cpd_entries SET user_id=? WHERE id=?", (tid, r["id"]))
            else:
                d.execute("UPDATE cpd_entries SET admin_note=?, status='merged_duplicate' WHERE id=?",
                          (f"duplicate of survivor's credit for event #{r['source_event_id']}; merge {corr}", r["id"]))

        # event registrations: same shape, same resolution
        survivor_regs = {r["e"] for r in d.execute(
            "SELECT event_id e FROM event_registrations WHERE user_id=?", (tid,))}
        for r in d.execute("SELECT id,event_id FROM event_registrations WHERE user_id=?", (sid,)).fetchall():
            if r["event_id"] not in survivor_regs:
                survivor_regs.add(r["event_id"])
                d.execute("UPDATE event_registrations SET user_id=? WHERE id=?", (tid, r["id"]))
            else:
                d.execute("UPDATE event_registrations SET status='merged_duplicate' WHERE id=?", (r["id"],))

        # student_profiles: survivor's kept where both exist
        if d.execute("SELECT id FROM student_profiles WHERE user_id=?", (tid,)).fetchone() is not None:
            d.execute("DELETE FROM student_profiles WHERE user_id=?", (sid,))
        else:
            d.execute("UPDATE student_profiles SET user_id=? WHERE user_id=?", (tid, sid))

        # world side
        d.execute("UPDATE pciworld_attempts SET canonical_user_id=? WHERE canonical_user_id=?", (tid, sid))
        if d.execute("SELECT id FROM pciworld_participants WHERE user_id=?", (tid,)).fetchone() is not None:
            d.execute("DELETE FROM pciworld_participants WHERE user_id=?", (sid,))
        else:
            d.execute("UPDATE pciworld_participants SET user_id=? WHERE user_id=?", (tid, sid))
        d.execute("UPDATE pciworld_users SET student_user_id=? WHERE student_user_id=?", (tid, sid))
        d.execute("UPDATE pciworld_user_map SET canonical_user_id=? WHERE canonical_user_id=?", (tid, sid))

        # loser row → inert shell; email freed; projection cleared (registry is the record)
        d.execute("UPDATE users SET status='merged', email=?, registration_no=NULL, "
                  "updated_at=datetime('now') WHERE id=?", (f"merged-{sid}-{loser_email}", sid))

        # revoke BOTH accounts' sessions
        d.execute("DELETE FROM login_tokens WHERE user_id IN (?,?)", (sid, tid))

        after = snapshot(d, sid, tid)
        d.execute("UPDATE pci_identity_merges SET before_json=?, after_json=? WHERE id=?",
                  (json.dumps(before), json.dumps(after), merge_id))
        d.execute("COMMIT")
        return None, {"merged_student_number": loser_no, "survivor_student_number": survivor_no}
    except RuntimeError as ex:
        d.execute("ROLLBACK")
        return str(ex), None


def reject(d, merge_id, admin, note=None):
    if d.execute("SELECT id FROM pci_identity_merges WHERE id=?", (merge_id,)).fetchone() is None:
        return "not_found"
    changed = d.execute("UPDATE pci_identity_merges SET status='rejected', decided_by=?, "
                        "decided_at=datetime('now'), decision_note=?, row_version=row_version+1 "
                        "WHERE id=? AND status='pending'", (admin, note, merge_id)).rowcount
    return None if changed else "already_decided"


# ═══════════════════════════════════════════════════════════════════════════════════════════════
print("=" * 72)
print("PCI IDENTITY — MAKER-CHECKER DUPLICATE-ACCOUNT MERGE (spec §4.11)")
print("=" * 72)

MAKER, CHECKER = 11, 22   # admin ids

# ── A. request validation ──────────────────────────────────────────────────────────────────────
d = fresh_db()
mkuser(d, 1, "loser@pci.test", "PCI-2026-000001")
mkuser(d, 2, "survivor@pci.test", "PCI-2026-000002")
mkuser(d, 3, "third@pci.test", "PCI-2026-000003")
mkuser(d, 8, "adminrole@pci.test", role="admin")
mkuser(d, 9, "gone@pci.test", status="erased")

check("A1 self-merge is refused", create_request(d, 1, 1, "x", MAKER)[1] == "self_merge")
check("A2 an unknown user is refused", create_request(d, 1, 777, "x", MAKER)[1] == "unknown_user")
check("A3 a non-student account is refused", create_request(d, 1, 8, "x", MAKER)[1] == "not_student")
check("A4 an erased account is refused", create_request(d, 9, 2, "x", MAKER)[1] == "not_mergeable")
mid, err = create_request(d, 1, 2, "duplicate signup", MAKER)
check("A5 a valid request is created pending", err is None and mid > 0
      and d.execute("SELECT status FROM pci_identity_merges WHERE id=?", (mid,)).fetchone()["status"] == "pending")
check("A6 a user already in a pending merge is refused (as source)",
      create_request(d, 1, 3, "x", MAKER)[1] == "already_pending")
check("A7 a user already in a pending merge is refused (as target)",
      create_request(d, 3, 2, "x", MAKER)[1] == "already_pending")

# ── B. the two-person rule ─────────────────────────────────────────────────────────────────────
err, _ = approve(d, mid, MAKER)
check("B1 the maker can NEVER approve their own request", err == "maker_checker")
check("B2 a refused approval executes nothing",
      d.execute("SELECT status FROM users WHERE id=1").fetchone()["status"] == "active"
      and d.execute("SELECT status FROM pci_identity_merges WHERE id=?", (mid,)).fetchone()["status"] == "pending")
err, _ = approve(d, 999, CHECKER)
check("B3 approving a request that does not exist is not_found", err == "not_found")

# ── C. execution — the full merge, on a populated pair ─────────────────────────────────────────
d = fresh_db()
mkuser(d, 1, "dupe@pci.test", "PCI-2026-000001")     # loser
mkuser(d, 2, "keep@pci.test", "PCI-2026-000002")     # survivor
for uid in (1, 2):
    d.execute("INSERT INTO payments(user_id,product_type,final_amount,payment_status) VALUES(?, 'exam',300,'paid')", (uid,))
    d.execute("INSERT INTO login_tokens(user_id,token,purpose) VALUES(?,?,'session')", (uid, f"tok-{uid}"))
d.execute("INSERT INTO exam_entitlements(user_id,product_type) VALUES(1,'exam')")
d.execute("INSERT INTO exam_bookings(user_id,scheduled_at) VALUES(1,'2026-09-01 10:00:00')")
d.execute("INSERT INTO exam_attempts(user_id,status) VALUES(1,'submitted')")
d.execute("INSERT INTO issued_credentials(credential_id,user_id) VALUES('PCP-AI-2026-00001',1)")
d.execute("INSERT INTO memberships(user_id,membership_type,status) VALUES(1,'professional','active')")
d.execute("INSERT INTO founding_applications(user_id,code_id,status) VALUES(1,5,'approved')")
# profiles on BOTH sides — survivor's must be kept, loser's deleted
d.execute("INSERT INTO student_profiles(user_id,city) VALUES(1,'LoserCity')")
d.execute("INSERT INTO student_profiles(user_id,city) VALUES(2,'SurvivorCity')")
# cpd: a manual entry, a non-colliding event credit, and a COLLIDING event credit
d.execute("INSERT INTO cpd_entries(user_id,hours,description) VALUES(1,2,'manual')")
d.execute("INSERT INTO cpd_entries(user_id,hours,source_event_id,status) VALUES(1,1,101,'approved')")
d.execute("INSERT INTO cpd_entries(user_id,hours,source_event_id,status) VALUES(1,1,102,'approved')")
d.execute("INSERT INTO cpd_entries(user_id,hours,source_event_id,status) VALUES(2,1,102,'approved')")
# event registrations: one colliding, one not
d.execute("INSERT INTO event_registrations(event_id,user_id,status) VALUES(201,1,'attended')")
d.execute("INSERT INTO event_registrations(event_id,user_id,status) VALUES(202,1,'registered')")
d.execute("INSERT INTO event_registrations(event_id,user_id,status) VALUES(201,2,'registered')")
# world: both users have linked world accounts, attempts and outstanding handoff codes
d.execute("INSERT INTO pciworld_users(id,email,student_user_id) VALUES(51,'dupe@pci.test',1)")
d.execute("INSERT INTO pciworld_users(id,email,student_user_id) VALUES(52,'keep@pci.test',2)")
d.execute("INSERT INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome) VALUES(51,1,'linked')")
d.execute("INSERT INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome) VALUES(52,2,'linked')")
d.execute("INSERT INTO pciworld_attempts(user_id,canonical_user_id,status,passport_visible) VALUES(51,1,'completed',1)")
d.execute("INSERT INTO pciworld_handoff_codes(code_sha,world_user_id,expires_at) VALUES('sha1',51,'2027-01-01 00:00:00')")
d.execute("INSERT INTO pciworld_handoff_codes(code_sha,world_user_id,expires_at) VALUES('sha2',52,'2027-01-01 00:00:00')")
d.execute("INSERT INTO pciworld_participants(user_id) VALUES(1)")
d.execute("INSERT INTO pciworld_participants(user_id) VALUES(2)")

pre = counts(d, 1)
check("C0 the preview counts the loser's estate",
      pre["payments"] == 1 and pre["cpd_entries"] == 3 and pre["event_registrations"] == 2
      and pre["world_attempts"] == 1 and pre["passport_evidence"] == 1 and pre["handoff_codes"] == 1
      and pre["sessions"] == 1, str(pre))

mid, err = create_request(d, 1, 2, "same person, two signups", MAKER)
before_users = d.execute("SELECT COUNT(*) c FROM users").fetchone()["c"]
err, result = approve(d, mid, CHECKER, "verified by support ticket 4411")
check("C1 a different admin holding id_merge_approve can approve", err is None, str(err))

srow = d.execute("SELECT * FROM users WHERE id=2").fetchone()
lrow = d.execute("SELECT * FROM users WHERE id=1").fetchone()
check("C2 the survivor keeps their Student Number unchanged",
      srow["registration_no"] == "PCI-2026-000002" and srow["status"] == "active")
reg = d.execute("SELECT * FROM pci_student_number_registry WHERE student_number='PCI-2026-000001'").fetchone()
check("C3 the loser's number is state='merged'", reg["state"] == "merged", reg["state"])
check("C4 …recording the survivor's number", reg["merged_into_student_number"] == "PCI-2026-000002")
check("C5 …and resolving to the survivor for private support resolution", reg["resolves_to_user_id"] == 2)
check("C6 the loser row is an inert shell: status merged, projection cleared",
      lrow["status"] == "merged" and lrow["registration_no"] is None)
check("C7 the loser's email is prefixed merged-{id}-{email}", lrow["email"] == "merged-1-dupe@pci.test")
try:
    d.execute("INSERT INTO users(email,role,status) VALUES('dupe@pci.test','student','active')")
    check("C8 the freed email can be used again", True)
except sqlite3.IntegrityError:
    check("C8 the freed email can be used again", False, "unique email still blocked")
check("C9 BOTH accounts' sessions are revoked",
      d.execute("SELECT COUNT(*) c FROM login_tokens WHERE user_id IN (1,2)").fetchone()["c"] == 0)
check("C10 outstanding handoff codes for both accounts are revoked",
      d.execute("SELECT COUNT(*) c FROM pciworld_handoff_codes").fetchone()["c"] == 0)

for t in BLANKET_TABLES:
    n = d.execute(f"SELECT COUNT(*) c FROM {t} WHERE user_id=1").fetchone()["c"]
    check(f"C11 {t}: every loser row re-pointed to the survivor", n == 0, f"{n} left behind")
check("C12 the survivor now holds both payments",
      d.execute("SELECT COUNT(*) c FROM payments WHERE user_id=2").fetchone()["c"] == 2)

# ── D. per-table collision handling ────────────────────────────────────────────────────────────
check("D1 manual + non-colliding CPD moved to the survivor",
      d.execute("SELECT COUNT(*) c FROM cpd_entries WHERE user_id=2").fetchone()["c"] == 3)
dup = d.execute("SELECT * FROM cpd_entries WHERE user_id=1").fetchone()
check("D2 the colliding CPD credit keeps the survivor's row and marks the loser's",
      dup is not None and dup["source_event_id"] == 102 and dup["status"] == "merged_duplicate"
      and "duplicate" in (dup["admin_note"] or ""))
check("D3 exactly ONE credit per (event,user) survives — the constraint holds",
      d.execute("SELECT COUNT(*) c FROM cpd_entries WHERE user_id=2 AND source_event_id=102").fetchone()["c"] == 1)
check("D4 the non-colliding event registration moved",
      d.execute("SELECT COUNT(*) c FROM event_registrations WHERE user_id=2 AND event_id=202").fetchone()["c"] == 1)
mreg = d.execute("SELECT * FROM event_registrations WHERE user_id=1 AND event_id=201").fetchone()
check("D5 the colliding registration keeps the survivor's row and marks the loser's",
      mreg is not None and mreg["status"] == "merged_duplicate"
      and d.execute("SELECT status FROM event_registrations WHERE user_id=2 AND event_id=201").fetchone()["status"] == "registered")
check("D6 the survivor's profile is kept, the loser's deleted",
      d.execute("SELECT city FROM student_profiles WHERE user_id=2").fetchone()["city"] == "SurvivorCity"
      and d.execute("SELECT COUNT(*) c FROM student_profiles WHERE user_id=1").fetchone()["c"] == 0)
check("D7 world attempts follow the person (canonical stamp moves)",
      d.execute("SELECT COUNT(*) c FROM pciworld_attempts WHERE canonical_user_id=2").fetchone()["c"] == 1
      and d.execute("SELECT COUNT(*) c FROM pciworld_attempts WHERE canonical_user_id=1").fetchone()["c"] == 0)
check("D8 the world participation collision keeps the survivor's row",
      d.execute("SELECT COUNT(*) c FROM pciworld_participants WHERE user_id=2").fetchone()["c"] == 1
      and d.execute("SELECT COUNT(*) c FROM pciworld_participants WHERE user_id=1").fetchone()["c"] == 0)
check("D9 world credential links re-resolve to the survivor",
      d.execute("SELECT student_user_id FROM pciworld_users WHERE id=51").fetchone()["student_user_id"] == 2
      and d.execute("SELECT canonical_user_id FROM pciworld_user_map WHERE legacy_world_id=51").fetchone()["canonical_user_id"] == 2)

# ── E. the record ──────────────────────────────────────────────────────────────────────────────
mrow = d.execute("SELECT * FROM pci_identity_merges WHERE id=?", (mid,)).fetchone()
check("E1 the decision records who and why",
      mrow["decided_by"] == CHECKER and mrow["decided_at"] is not None
      and mrow["decision_note"] == "verified by support ticket 4411")
bj, aj = json.loads(mrow["before_json"]), json.loads(mrow["after_json"])
check("E2 the before snapshot captured the pre-merge estate",
      bj["source"]["registration_no"] == "PCI-2026-000001" and bj["source"]["counts"]["payments"] == 1
      and bj["target"]["counts"]["payments"] == 1)
check("E3 the after snapshot shows the converged estate",
      aj["source"]["status"] == "merged" and aj["source"]["registration_no"] is None
      and aj["target"]["counts"]["payments"] == 2 and aj["target"]["counts"]["sessions"] == 0)
check("E4 no users row was created or destroyed by the merge itself",
      before_users == d.execute("SELECT COUNT(*) c FROM users").fetchone()["c"] - 1)  # +1 = C8's reuse insert

# ── F. terminal states ─────────────────────────────────────────────────────────────────────────
err, _ = approve(d, mid, CHECKER)
check("F1 approving an already-approved request is refused (409 already_decided)",
      err == "already_decided")
check("F2 rejecting an already-approved request is refused", reject(d, mid, CHECKER) == "already_decided")

d2 = fresh_db()
mkuser(d2, 1, "a@pci.test", "PCI-2026-000001")
mkuser(d2, 2, "b@pci.test", "PCI-2026-000002")
mid2, _ = create_request(d2, 1, 2, "mistake", MAKER)
check("F3 reject records who and why", reject(d2, mid2, CHECKER, "not the same person") is None
      and d2.execute("SELECT decided_by,decision_note,status FROM pci_identity_merges WHERE id=?",
                     (mid2,)).fetchone()["decision_note"] == "not the same person")
check("F4 a rejected request is terminal — it can never be approved",
      approve(d2, mid2, CHECKER)[0] == "already_decided")
check("F5 a reject touches neither account",
      d2.execute("SELECT status,registration_no FROM users WHERE id=1").fetchone()["registration_no"] == "PCI-2026-000001")
check("F6 after a rejection the pair can be requested again",
      create_request(d2, 1, 2, "second look", MAKER)[1] is None)

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All maker-checker merge assertions passed.")
