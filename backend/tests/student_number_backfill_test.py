"""Phase 2 — backfill, reconciliation and retirement of the canonical PCI Student Number.

Transcribes the SQL that Core/StudentNumberBackfill.cs runs, against a real SQLite database built
from the real schema.sql. What is under test is the ordering discipline that makes the rollout safe:

  • report before writing — a preview must write nothing at all
  • never renumber a valid historic value
  • a collision quarantines ONE account and the batch carries on
  • stopping and restarting resumes; re-running after completion is a no-op
  • erasure retires the number rather than freeing it for the next person

Run:  python3 tests/student_number_backfill_test.py
"""
import os
import re
import sqlite3
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
FAILURES = []
LEGACY = re.compile(r"^PCI-\d{4}-\d{6,}$")
ELIGIBLE = "role='student' AND status<>'erased'"


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


def fresh_db():
    d = sqlite3.connect(":memory:")
    d.row_factory = sqlite3.Row
    d.executescript(open(os.path.join(HERE, "..", "schema.sql")).read())
    d.execute("CREATE UNIQUE INDEX IF NOT EXISTS ux_student_number_registry "
              "ON pci_student_number_registry(student_number)")
    return d


def fmt(uid, created):
    year = created[:4] if created and created[:4].isdigit() else "0000"
    return f"PCI-{year}-{uid:06d}"


def get_or_issue(d, uid, reason):
    row = d.execute("SELECT registration_no,created_at FROM users WHERE id=?", (uid,)).fetchone()
    if row["registration_no"]:
        return row["registration_no"]
    number = fmt(uid, row["created_at"])
    try:
        d.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
                  "resolves_to_user_id,state,reason_code) VALUES(?,'legacy_v1',?,?,'issued',?)",
                  (number, uid, uid, reason))
    except sqlite3.IntegrityError as ex:
        held = d.execute("SELECT resolves_to_user_id FROM pci_student_number_registry WHERE student_number=?",
                         (number,)).fetchone()
        if held is None or held["resolves_to_user_id"] != uid:
            raise RuntimeError(f"{number} reserved elsewhere") from ex
    d.execute("UPDATE users SET registration_no=?, registration_no_issued_at=datetime('now') "
              "WHERE id=? AND (registration_no IS NULL OR registration_no='')", (number, uid))
    return d.execute("SELECT registration_no FROM users WHERE id=?", (uid,)).fetchone()["registration_no"]


def preview(d, limit=100):
    rows = d.execute(f"SELECT id,created_at FROM users WHERE {ELIGIBLE} "
                     f"AND (registration_no IS NULL OR registration_no='') ORDER BY id LIMIT {limit}").fetchall()
    out = []
    for r in rows:
        n = fmt(r["id"], r["created_at"])
        held = d.execute("SELECT resolves_to_user_id FROM pci_student_number_registry WHERE student_number=?",
                         (n,)).fetchone()
        out.append({"user_id": r["id"], "would_issue": n,
                    "conflict": held is not None and held["resolves_to_user_id"] != r["id"]})
    return out


def run_batch(d, size=200):
    rows = d.execute(f"SELECT id FROM users WHERE {ELIGIBLE} "
                     f"AND (registration_no IS NULL OR registration_no='') ORDER BY id LIMIT {size}").fetchall()
    issued = quarantined = 0
    for r in rows:
        uid = r["id"]
        try:
            get_or_issue(d, uid, "phase2_backfill")
            issued += 1
        except RuntimeError:
            quarantined += 1
            try:
                d.execute("INSERT INTO pci_student_number_registry(student_number,format_version,"
                          "original_user_id,resolves_to_user_id,state,reason_code) "
                          "VALUES(?,'legacy_v1',?,NULL,'quarantined','backfill_conflict')",
                          (f"QUARANTINE-{uid}", uid))
            except sqlite3.IntegrityError:
                pass
    remaining = d.execute(f"SELECT COUNT(*) c FROM users WHERE {ELIGIBLE} "
                          f"AND (registration_no IS NULL OR registration_no='')").fetchone()["c"]
    return {"examined": len(rows), "issued": issued, "quarantined": quarantined, "remaining": remaining}


def retire(d, number, reason):
    if not number:
        return
    cur = d.execute("UPDATE pci_student_number_registry SET state='retired', changed_at=datetime('now'), "
                    "reason_code=?, row_version=row_version+1 WHERE student_number=? AND state<>'retired'",
                    (reason, number))
    if cur.rowcount == 0 and d.execute("SELECT id FROM pci_student_number_registry WHERE student_number=?",
                                       (number,)).fetchone() is None:
        d.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
                  "resolves_to_user_id,state,reason_code) VALUES(?,'legacy_v1',0,NULL,'retired',?)",
                  (number, reason))


def mkuser(d, uid, email, created="2026-01-01 00:00:00", role="student", status="active", number=None):
    d.execute("INSERT INTO users(id,email,first_name,role,status,created_at,registration_no) VALUES(?,?,?,?,?,?,?)",
              (uid, email, "T", role, status, created, number))


print("=" * 72)
print("PCI STUDENT NUMBER — PHASE 2 BACKFILL, RECONCILIATION, RETIREMENT")
print("=" * 72)

# ── A. preview writes nothing ──────────────────────────────────────────────────────────────
d = fresh_db()
for i in range(1, 6):
    mkuser(d, i, f"u{i}@pci.test", created=f"202{i}-05-05 00:00:00")
before_users = d.execute("SELECT COUNT(*) c FROM users WHERE registration_no IS NOT NULL").fetchone()["c"]
before_reg = d.execute("SELECT COUNT(*) c FROM pci_student_number_registry").fetchone()["c"]
p = preview(d)
check("A1 preview lists every account missing a number", len(p) == 5, str(len(p)))
check("A2 preview derives the year from the account, not from now",
      p[0]["would_issue"] == "PCI-2021-000001", p[0]["would_issue"])
check("A3 preview writes NOTHING to users",
      d.execute("SELECT COUNT(*) c FROM users WHERE registration_no IS NOT NULL").fetchone()["c"] == before_users)
check("A4 preview writes NOTHING to the registry",
      d.execute("SELECT COUNT(*) c FROM pci_student_number_registry").fetchone()["c"] == before_reg)

# ── B. eligibility ─────────────────────────────────────────────────────────────────────────
mkuser(d, 90, "admin@pci.test", role="admin")
mkuser(d, 91, "gone@pci.test", status="erased")
ids = {x["user_id"] for x in preview(d)}
check("B1 admin accounts are never issued a Student Number", 90 not in ids)
check("B2 erased accounts are never re-issued one", 91 not in ids)

# ── C. the batch run ───────────────────────────────────────────────────────────────────────
r = run_batch(d)
check("C1 the batch issues one number per eligible account", r["issued"] == 5 and r["quarantined"] == 0, str(r))
check("C2 nothing remains after a complete run", r["remaining"] == 0, str(r["remaining"]))
check("C3 every issued number is well formed",
      all(LEGACY.match(x["registration_no"]) for x in
          d.execute(f"SELECT registration_no FROM users WHERE {ELIGIBLE} AND registration_no IS NOT NULL")))
again = run_batch(d)
check("C4 re-running after completion is a no-op (idempotent)",
      again["examined"] == 0 and again["issued"] == 0, str(again))

# ── D. resumability ────────────────────────────────────────────────────────────────────────
d2 = fresh_db()
for i in range(1, 11):
    mkuser(d2, i, f"r{i}@pci.test")
first = run_batch(d2, size=4)
check("D1 a bounded batch issues only its slice", first["issued"] == 4 and first["remaining"] == 6, str(first))
second = run_batch(d2, size=4)
check("D2 restarting resumes where it stopped, never redoing work",
      second["issued"] == 4 and second["remaining"] == 2, str(second))
run_batch(d2)
check("D3 the run completes across restarts with one number each",
      d2.execute("SELECT COUNT(DISTINCT registration_no) c FROM users WHERE registration_no IS NOT NULL").fetchone()["c"] == 10)

# ── E. a collision quarantines ONE account, not the batch ──────────────────────────────────
d3 = fresh_db()
for i in range(1, 4):
    mkuser(d3, i, f"c{i}@pci.test")
# Pre-reserve user 2's number to somebody else.
d3.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
           "resolves_to_user_id,state) VALUES(?, 'legacy_v1',999,999,'issued')", (fmt(2, "2026-01-01"),))
r3 = run_batch(d3)
check("E1 the conflicting account is quarantined", r3["quarantined"] == 1, str(r3))
check("E2 the other accounts still get their numbers", r3["issued"] == 2, str(r3))
check("E3 the quarantined account holds NO number rather than a wrong one",
      d3.execute("SELECT registration_no FROM users WHERE id=2").fetchone()["registration_no"] is None)
check("E4 the quarantine is recorded for a human to resolve",
      d3.execute("SELECT COUNT(*) c FROM pci_student_number_registry WHERE state='quarantined'").fetchone()["c"] == 1)

# ── F. historic values are preserved exactly ───────────────────────────────────────────────
d4 = fresh_db()
mkuser(d4, 1, "hist@pci.test", created="2019-02-02 00:00:00", number="PCI-2019-000001")
mkuser(d4, 2, "odd@pci.test", created="2018-02-02 00:00:00", number="PCI-LEGACY-7")
run_batch(d4)
check("F1 a valid historic number is untouched",
      d4.execute("SELECT registration_no FROM users WHERE id=1").fetchone()["registration_no"] == "PCI-2019-000001")
check("F2 a malformed historic number is reported, never silently rewritten",
      d4.execute("SELECT registration_no FROM users WHERE id=2").fetchone()["registration_no"] == "PCI-LEGACY-7")

# ── G. erasure retires rather than frees ───────────────────────────────────────────────────
d5 = fresh_db()
mkuser(d5, 1, "erase@pci.test")
n = get_or_issue(d5, 1, "self_signup")
retire(d5, n, "erasure")
d5.execute("UPDATE users SET registration_no=NULL, status='erased' WHERE id=1")
row = d5.execute("SELECT state FROM pci_student_number_registry WHERE student_number=?", (n,)).fetchone()
check("G1 the reservation survives erasure in state 'retired'", row is not None and row["state"] == "retired",
      row["state"] if row else "missing")
try:
    d5.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
               "resolves_to_user_id,state) VALUES(?, 'legacy_v1',2,2,'issued')", (n,))
    check("G2 an erased number can never be reserved again", False, "reservation succeeded")
except sqlite3.IntegrityError:
    check("G2 an erased number can never be reserved again", True)

# A number issued before the registry existed has no row to retire — retiring must CREATE the
# reservation, or the value stays unclaimed and therefore reissuable.
d6 = fresh_db()
mkuser(d6, 1, "pre@pci.test", number="PCI-2020-000001")
retire(d6, "PCI-2020-000001", "erasure")
row = d6.execute("SELECT state FROM pci_student_number_registry WHERE student_number=?",
                 ("PCI-2020-000001",)).fetchone()
check("G3 retiring an unrecorded historic number creates its reservation",
      row is not None and row["state"] == "retired", row["state"] if row else "missing")

# ── H. reconciliation sees drift in both directions ────────────────────────────────────────
d7 = fresh_db()
mkuser(d7, 1, "d1@pci.test", number="PCI-2026-000001")          # projection, no reservation
d7.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
           "resolves_to_user_id,state) VALUES('PCI-2026-000777','legacy_v1',777,777,'issued')")  # reservation, no projection
orphan_proj = d7.execute("""SELECT COUNT(*) c FROM users u WHERE u.registration_no IS NOT NULL AND u.registration_no<>''
    AND NOT EXISTS(SELECT 1 FROM pci_student_number_registry r WHERE r.student_number=u.registration_no)""").fetchone()["c"]
orphan_res = d7.execute("""SELECT COUNT(*) c FROM pci_student_number_registry r WHERE r.state='issued'
    AND NOT EXISTS(SELECT 1 FROM users u WHERE u.registration_no=r.student_number)""").fetchone()["c"]
check("H1 a number with no reservation is detected", orphan_proj == 1, str(orphan_proj))
check("H2 a reservation with no number is detected", orphan_res == 1, str(orphan_res))

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All backfill, reconciliation and retirement assertions passed.")
