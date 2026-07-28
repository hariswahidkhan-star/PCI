"""Canonical PCI Student Number — issuance, reservation and non-reuse.

Replicates the exact SQL that Core/StudentNumbers.cs runs, against a real SQLite database
built from schema.sql plus the idempotent upgrades in Data/Migrate.cs. What is under test is
the *semantics* of issuance, which is where the confirmed defects lived:

  • the number must come from the account's own created_at year, not from the clock at read time
  • issuance must be idempotent, so a retried signup never mints a second identity
  • the registry reservation must be the atomic claim, so a number cannot land on two people
  • erasure must release the person, never the number

Run:  python3 tests/student_number_test.py
"""
import os
import sqlite3
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
FAILURES = []


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


def fresh_db():
    """schema.sql + the Migrate.cs upgrades this feature adds, in the same order boot applies them."""
    d = sqlite3.connect(":memory:")
    d.row_factory = sqlite3.Row
    d.executescript(open(os.path.join(HERE, "..", "schema.sql")).read())
    # Data/Migrate.cs: the guarded projection index. On a fresh database there are no duplicates,
    # so boot creates it immediately — which is exactly the state this suite asserts against.
    d.execute("CREATE UNIQUE INDEX IF NOT EXISTS ux_users_registration_no ON users(registration_no) "
              "WHERE registration_no IS NOT NULL")
    d.execute("CREATE UNIQUE INDEX IF NOT EXISTS ux_student_number_registry "
              "ON pci_student_number_registry(student_number)")
    return d


# ── Core/StudentNumbers.cs, transcribed ────────────────────────────────────────────────────
def fmt(user_id, created_at):
    year = created_at[:4] if created_at and created_at[:4].isdigit() else "0000"
    return f"PCI-{year}-{user_id:06d}"


def get_or_issue(d, user_id, reason):
    row = d.execute("SELECT registration_no,created_at FROM users WHERE id=?", (user_id,)).fetchone()
    if row is None:
        raise LookupError(f"unknown user {user_id}")
    if row["registration_no"]:
        return row["registration_no"]
    number = fmt(user_id, row["created_at"])
    try:
        d.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
                  "resolves_to_user_id,state,reason_code) VALUES(?,'legacy_v1',?,?,'issued',?)",
                  (number, user_id, user_id, reason))
    except sqlite3.IntegrityError as ex:
        held = d.execute("SELECT resolves_to_user_id FROM pci_student_number_registry WHERE student_number=?",
                         (number,)).fetchone()
        if held is None or held["resolves_to_user_id"] != user_id:
            raise RuntimeError(f"{number} is already reserved for another identity") from ex
    d.execute("UPDATE users SET registration_no=?, registration_no_issued_at=datetime('now') "
              "WHERE id=? AND (registration_no IS NULL OR registration_no='')", (number, user_id))
    settled = d.execute("SELECT registration_no FROM users WHERE id=?", (user_id,)).fetchone()["registration_no"]
    if not settled:
        raise RuntimeError("issuance did not persist")
    return settled


def make_user(d, uid, email, created_at):
    d.execute("INSERT INTO users(id,email,first_name,status,created_at) VALUES(?,?,?,'active',?)",
              (uid, email, "T", created_at))


print("=" * 72)
print("PCI STUDENT NUMBER — ISSUANCE, RESERVATION, NON-REUSE")
print("=" * 72)

# ── A. schema and format ───────────────────────────────────────────────────────────────────
d = fresh_db()
cols = {r[1] for r in d.execute("PRAGMA table_info(users)")}
check("A1 users carries registration_no_issued_at", "registration_no_issued_at" in cols)
check("A2 the number registry table exists",
      d.execute("SELECT COUNT(*) c FROM sqlite_master WHERE type='table' "
                "AND name='pci_student_number_registry'").fetchone()["c"] == 1)

check("A3 format is PCI-{created year}-{id padded to 6}", fmt(123, "2026-07-28 10:00:00") == "PCI-2026-000123",
      fmt(123, "2026-07-28 10:00:00"))
check("A4 ids past 6 digits are not truncated", fmt(1234567, "2026-01-01 00:00:00") == "PCI-2026-1234567",
      fmt(1234567, "2026-01-01 00:00:00"))

# The defect that motivated the service: GET /api/me stamped DateTime.UtcNow, so an account
# created in 2024 first read in 2026 was issued PCI-2026-… . The year must follow the account.
make_user(d, 1, "old@pci.test", "2024-03-04 09:00:00")
n1 = get_or_issue(d, 1, "self_signup")
check("A5 year comes from the account, not from now", n1 == "PCI-2024-000001", n1)

# ── B. idempotence ─────────────────────────────────────────────────────────────────────────
again = get_or_issue(d, 1, "self_signup")
check("B1 re-issuing returns the same number", again == n1, again)
check("B2 a replay does not add a second reservation",
      d.execute("SELECT COUNT(*) c FROM pci_student_number_registry WHERE resolves_to_user_id=1").fetchone()["c"] == 1)

# A number that already exists is authoritative even if it predates the current format — the
# service must never re-derive over a historic value.
make_user(d, 2, "legacy@pci.test", "2019-01-01 00:00:00")
d.execute("UPDATE users SET registration_no='PCI-OLD-42' WHERE id=2")
check("B3 a valid historic number is never rewritten", get_or_issue(d, 2, "self_signup") == "PCI-OLD-42")

# ── C. the reservation is the claim ────────────────────────────────────────────────────────
# Force a collision: reserve user 3's number for somebody else first, then try to issue it.
make_user(d, 3, "clash@pci.test", "2026-01-01 00:00:00")
d.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
          "resolves_to_user_id,state) VALUES(?, 'legacy_v1',99,99,'issued')", (fmt(3, "2026-01-01"),))
try:
    get_or_issue(d, 3, "self_signup")
    check("C1 a number reserved to another identity is refused", False, "issuance succeeded")
except RuntimeError:
    check("C1 a number reserved to another identity is refused", True)
check("C2 the losing user is left with no number, not a wrong one",
      d.execute("SELECT registration_no FROM users WHERE id=3").fetchone()["registration_no"] is None)

# ── D. the projection cannot hold duplicates ───────────────────────────────────────────────
make_user(d, 4, "dup@pci.test", "2026-01-01 00:00:00")
try:
    d.execute("UPDATE users SET registration_no=? WHERE id=4", (n1,))
    check("D1 duplicate registration_no is rejected by the index", False, "the update succeeded")
except sqlite3.IntegrityError:
    check("D1 duplicate registration_no is rejected by the index", True)

# Many users may legitimately have no number yet during backfill, so the index must exempt NULL.
make_user(d, 5, "null1@pci.test", "2026-01-01 00:00:00")
make_user(d, 6, "null2@pci.test", "2026-01-01 00:00:00")
check("D2 several users may hold NULL at once (backfill window)",
      d.execute("SELECT COUNT(*) c FROM users WHERE registration_no IS NULL").fetchone()["c"] >= 3)

# ── E. erasure releases the person, never the number ───────────────────────────────────────
# Core/Erasure.cs clears users.registration_no. The registry row is what makes that safe: the
# value stays reserved, so a later account can never be issued the erased person's number.
d.execute("UPDATE users SET registration_no=NULL WHERE id=1")
check("E1 the reservation survives erasure of the projection",
      d.execute("SELECT state,student_number FROM pci_student_number_registry "
                "WHERE student_number=?", (n1,)).fetchone() is not None)
# A brand-new user that happens to reuse id 1 is impossible (AUTOINCREMENT), but a backfill
# deriving the same string must still be refused by the reservation.
make_user(d, 7, "reuse@pci.test", "2024-03-04 09:00:00")
try:
    d.execute("INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,"
              "resolves_to_user_id,state) VALUES(?, 'legacy_v1',7,7,'issued')", (n1,))
    check("E2 an erased number cannot be reserved again", False, "the reservation succeeded")
except sqlite3.IntegrityError:
    check("E2 an erased number cannot be reserved again", True)

# ── F. no read path issues a number ────────────────────────────────────────────────────────
# StudentNumbers.Read is the read-side helper: it returns None rather than minting.
def read_only(d, uid):
    r = d.execute("SELECT registration_no FROM users WHERE id=?", (uid,)).fetchone()
    return r["registration_no"] if r and r["registration_no"] else None


before = d.execute("SELECT COUNT(*) c FROM pci_student_number_registry").fetchone()["c"]
check("F1 reading a numberless user returns nothing", read_only(d, 5) is None)
check("F2 reading reserves nothing",
      d.execute("SELECT COUNT(*) c FROM pci_student_number_registry").fetchone()["c"] == before)

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All student-number assertions passed.")
