"""Events defects ID-10 / ID-11 / ID-12 — logic suite for the fixed Endpoints/Events.cs.

Transcribes the exact SQL the fixed C# runs, against a real SQLite database built from the real
schema.sql plus the Migrate.cs event DDL (events / event_registrations are Migrate-created tables).
What is under test:

  • ID-10 — the old count-then-insert overbooks the final seat under interleaving (defect
    reproduced first), while the new guarded single-statement write admits exactly one winner
    even under the same interleaving; and the 'upcoming' predicate closes past published events
    without tripping over the 'YYYY-MM-DDTHH:MM' vs 'YYYY-MM-DD HH:MM:SS' lexical trap.
  • ID-11 — a cancelled registration can never be marked attended, and un-marking never
    resurrects a cancelled registration.
  • ID-12 — the attendance flip and the CPD credit commit or roll back together; a crash/retry
    can never leave a duplicate credit; ux_cpd_event_user refuses a second credit for the same
    (event, user) at the data layer; a pre-fix orphan credit is adopted, not duplicated.

Run:  python3 tests/events_defects_test.py
"""
import os
import re
import sqlite3
import sys
from datetime import datetime, timezone

HERE = os.path.dirname(os.path.abspath(__file__))
FAILURES = []


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


# ── the schema: real schema.sql + the Migrate.cs event DDL, transcribed exactly ────────────────
MIGRATE_EVENT_DDL = [
    # Migrate.cs — events / event_registrations (these live in Migrate, not schema.sql)
    """CREATE TABLE IF NOT EXISTS events(id INTEGER PRIMARY KEY AUTOINCREMENT,title TEXT NOT NULL,summary TEXT,description TEXT,
        event_type TEXT DEFAULT 'webinar',starts_at TEXT,ends_at TEXT,timezone TEXT,location TEXT,join_url TEXT,capacity INTEGER DEFAULT 0,
        cpd_hours REAL DEFAULT 0,cpd_category TEXT DEFAULT 'Events & webinars',status VARCHAR(24) DEFAULT 'draft',
        created_by INTEGER,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))""",
    "CREATE INDEX IF NOT EXISTS ix_events_status ON events(status)",
    """CREATE TABLE IF NOT EXISTS event_registrations(id INTEGER PRIMARY KEY AUTOINCREMENT,event_id INTEGER NOT NULL,user_id INTEGER NOT NULL,
        status TEXT DEFAULT 'registered',registered_at TEXT DEFAULT (datetime('now')),attended_at TEXT,cpd_entry_id INTEGER)""",
    "CREATE UNIQUE INDEX IF NOT EXISTS ux_event_reg ON event_registrations(event_id,user_id)",
    "CREATE INDEX IF NOT EXISTS ix_event_reg_user ON event_registrations(user_id)",
    # Migrate.cs — ID-12 upgrade (AddCol + guarded unique index), transcribed exactly
    "ALTER TABLE cpd_entries ADD COLUMN source_event_id INTEGER",
    "CREATE UNIQUE INDEX IF NOT EXISTS ux_cpd_event_user ON cpd_entries(source_event_id,user_id) WHERE source_event_id IS NOT NULL",
]


def fresh_db():
    d = sqlite3.connect(":memory:")
    d.row_factory = sqlite3.Row
    d.isolation_level = None  # explicit BEGIN/COMMIT, like Db.Transaction
    d.executescript(open(os.path.join(HERE, "..", "schema.sql")).read())
    for ddl in MIGRATE_EVENT_DDL:
        d.execute(ddl)
    return d


def mkevent(d, eid, capacity=0, status="published", starts_at=None, hours=0, title="Ev"):
    d.execute("INSERT INTO events(id,title,capacity,status,starts_at,cpd_hours) VALUES(?,?,?,?,?,?)",
              (eid, title, capacity, status, starts_at, hours))


# ── the fixed SQL, transcribed exactly from Endpoints/Events.cs ────────────────────────────────
UPCOMING = "(starts_at IS NULL OR starts_at='' OR substr(starts_at,1,10) >= date('now'))"
SEAT_GUARD = ("(SELECT COUNT(*) FROM (SELECT 1 FROM event_registrations r2 "
              "WHERE r2.event_id=? AND r2.status IN ('registered','attended')) taken) < ?")
GUARDED_INSERT = ("INSERT INTO event_registrations(event_id,user_id,status) "
                  "SELECT ?,?, 'registered' FROM events e2 WHERE e2.id=? AND " + SEAT_GUARD)
GUARDED_UPDATE = ("UPDATE event_registrations SET status='registered', registered_at=datetime('now') "
                  "WHERE id=? AND " + SEAT_GUARD)


def list_upcoming_ids(d):
    return [r["id"] for r in d.execute(
        "SELECT id FROM events WHERE status='published' AND " + UPCOMING +
        " ORDER BY (starts_at IS NULL), starts_at ASC, id DESC")]


def is_past_event(starts_at):
    """IsPastEvent from Events.cs: 10-char date-prefix ordinal compare against the UTC date."""
    today = datetime.now(timezone.utc).strftime("%Y-%m-%d")
    return bool(starts_at) and len(starts_at) >= 10 and starts_at[:10] < today


def new_register(d, eid, uid, read_hook=None):
    """The fixed register handler: one transaction, atomic seat guard in the write itself.
    read_hook fires after the existing-registration read — the interleave point of the old race."""
    ev = d.execute("SELECT id,capacity,status,starts_at FROM events WHERE id=?", (eid,)).fetchone()
    if ev is None or ev["status"] != "published":
        return "not_found"
    if is_past_event(ev["starts_at"]):
        return "event_past"
    cap = ev["capacity"] or 0
    d.execute("BEGIN")
    try:
        existing = d.execute("SELECT id,status FROM event_registrations WHERE event_id=? AND user_id=?",
                             (eid, uid)).fetchone()
        if read_hook:
            read_hook()
        if existing is not None and existing["status"] in ("registered", "attended"):
            d.execute("COMMIT")
            return "already"
        if cap > 0:
            if existing is not None:
                changes = d.execute(GUARDED_UPDATE, (existing["id"], eid, cap)).rowcount
            else:
                changes = d.execute(GUARDED_INSERT, (eid, uid, eid, eid, cap)).rowcount
            if changes == 0:
                d.execute("COMMIT")
                return "full"
        elif existing is not None:
            d.execute("UPDATE event_registrations SET status='registered', registered_at=datetime('now') WHERE id=?",
                      (existing["id"],))
        else:
            d.execute("INSERT INTO event_registrations(event_id,user_id,status) VALUES(?,?, 'registered')", (eid, uid))
        d.execute("COMMIT")
        return "ok"
    except Exception:
        d.execute("ROLLBACK")
        raise


def old_register_race(d, eid, cap, uid_a, uid_b):
    """The PRE-fix count-then-insert, interleaved the way two concurrent requests interleave:
    both count while the seat is still free, then both insert. Reproduces the ID-10 defect."""
    taken_a = d.execute("SELECT COUNT(*) c FROM event_registrations WHERE event_id=? AND status IN ('registered','attended')",
                        (eid,)).fetchone()["c"]
    taken_b = d.execute("SELECT COUNT(*) c FROM event_registrations WHERE event_id=? AND status IN ('registered','attended')",
                        (eid,)).fetchone()["c"]
    results = []
    for uid, taken in ((uid_a, taken_a), (uid_b, taken_b)):
        if taken >= cap:
            results.append("full")
        else:
            d.execute("INSERT INTO event_registrations(event_id,user_id,status) VALUES(?,?, 'registered')", (eid, uid))
            results.append("ok")
    return results


def mark_attended(d, eid, uid, admin_id=1, crash_between_writes=False):
    """The fixed attendance handler: ID-11 cancelled refusal, then the ID-12 one-transaction,
    exactly-once CPD credit. crash_between_writes raises after the CPD insert and before the
    registration flip — the exact window the old code left a duplicate in."""
    ev = d.execute("SELECT id,title,cpd_hours,cpd_category,starts_at FROM events WHERE id=?", (eid,)).fetchone()
    if ev is None:
        return "not_found"
    reg = d.execute("SELECT id,status,cpd_entry_id FROM event_registrations WHERE event_id=? AND user_id=?",
                    (eid, uid)).fetchone()
    if reg is None:
        return "not_registered"
    if reg["status"] == "cancelled":
        return "cancelled"                                                     # ID-11
    d.execute("BEGIN")
    try:
        r2 = d.execute("SELECT id,status,cpd_entry_id FROM event_registrations WHERE id=?", (reg["id"],)).fetchone()
        if r2 is None or r2["status"] == "attended":
            d.execute("COMMIT")
            return "already"
        cpd_id = r2["cpd_entry_id"]
        hours = ev["cpd_hours"] or 0
        if cpd_id is None and hours > 0:
            prior = d.execute("SELECT id FROM cpd_entries WHERE source_event_id=? AND user_id=?",
                              (eid, uid)).fetchone()
            if prior is not None:
                cpd_id = prior["id"]
            else:
                date = (ev["starts_at"] or datetime.now(timezone.utc).strftime("%Y-%m-%d"))
                if len(date) >= 10:
                    date = date[:10]
                cur = d.execute("""INSERT INTO cpd_entries(user_id,activity_date,category,hours,description,status,reviewed_by,reviewed_at,source_event_id)
                    VALUES(?,?,?,?,?, 'approved', ?, datetime('now'), ?)""",
                                (uid, date, ev["cpd_category"] or "Events & webinars", hours,
                                 "Attended: " + (ev["title"] or "PCI event"), admin_id, eid))
                cpd_id = cur.lastrowid
        if crash_between_writes:
            raise RuntimeError("simulated crash between CPD insert and attendance flip")
        d.execute("UPDATE event_registrations SET status='attended', attended_at=datetime('now'), cpd_entry_id=? WHERE id=?",
                  (cpd_id, r2["id"]))
        d.execute("COMMIT")
        return "ok"
    except Exception:
        d.execute("ROLLBACK")
        if crash_between_writes:
            return "crashed"
        raise


def unmark_attended(d, eid, uid):
    """The fixed un-mark path: never resurrects a cancelled registration; CPD removal + revert
    commit together, including the pre-fix orphan safety-net delete."""
    reg = d.execute("SELECT id,status,cpd_entry_id FROM event_registrations WHERE event_id=? AND user_id=?",
                    (eid, uid)).fetchone()
    if reg is None:
        return "not_registered"
    if reg["status"] != "attended":
        return "already"
    d.execute("BEGIN")
    try:
        if reg["cpd_entry_id"] is not None:
            d.execute("DELETE FROM cpd_entries WHERE id=?", (reg["cpd_entry_id"],))
        d.execute("DELETE FROM cpd_entries WHERE source_event_id=? AND user_id=?", (eid, uid))
        d.execute("UPDATE event_registrations SET status='registered', attended_at=NULL, cpd_entry_id=NULL WHERE id=?",
                  (reg["id"],))
        d.execute("COMMIT")
        return "ok"
    except Exception:
        d.execute("ROLLBACK")
        raise


def regs(d, eid):
    return d.execute("SELECT user_id,status FROM event_registrations WHERE event_id=? ORDER BY user_id", (eid,)).fetchall()


def cpd_count(d, eid, uid):
    return d.execute("SELECT COUNT(*) c FROM cpd_entries WHERE source_event_id=? AND user_id=?",
                     (eid, uid)).fetchone()["c"]


print("=" * 72)
print("EVENTS DEFECTS — ID-10 CAPACITY RACE, ID-11 ATTENDANCE GATE, ID-12 CPD EXACTLY-ONCE")
print("=" * 72)

# ── A. ID-10: the defect, reproduced, then closed ──────────────────────────────────────────────
d = fresh_db()
mkevent(d, 1, capacity=1)
old = old_register_race(d, 1, 1, 101, 102)
oversold = d.execute("SELECT COUNT(*) c FROM event_registrations WHERE event_id=1 AND status='registered'").fetchone()["c"]
check("A1 the PRE-fix count-then-insert oversells the final seat (defect reproduced)",
      old == ["ok", "ok"] and oversold == 2, f"results={old} seated={oversold}")

d = fresh_db()
mkevent(d, 1, capacity=1)
# Interleave the fixed path the same way: B's guarded write runs from a state where B's earlier
# read still said a seat was free — the guard in the WRITE decides, not the stale read.
r_a = new_register(d, 1, 101, read_hook=None)
r_b = new_register(d, 1, 102, read_hook=None)
seated = d.execute("SELECT COUNT(*) c FROM event_registrations WHERE event_id=1 AND status IN ('registered','attended')").fetchone()["c"]
check("A2 with the guard, of two requests for the final seat exactly one confirms",
      r_a == "ok" and r_b == "full" and seated == 1, f"a={r_a} b={r_b} seated={seated}")

# The stale-read interleave, explicitly: hand B a hook that seats A AFTER B has read (B saw an
# empty event) and BEFORE B writes. The old code overbooks here; the guarded write refuses.
d = fresh_db()
mkevent(d, 1, capacity=1)
r_b = new_register(d, 1, 102, read_hook=lambda: d.execute(GUARDED_INSERT, (1, 101, 1, 1, 1)))
seated = d.execute("SELECT COUNT(*) c FROM event_registrations WHERE event_id=1 AND status IN ('registered','attended')").fetchone()["c"]
check("A3 a seat taken between B's read and B's write is still honoured (B gets full)",
      r_b == "full" and seated == 1, f"b={r_b} seated={seated}")

# Re-registering a cancelled row goes through the same guard.
d = fresh_db()
mkevent(d, 1, capacity=1)
check("A4 A takes the only seat", new_register(d, 1, 101) == "ok")
d.execute("UPDATE event_registrations SET status='cancelled' WHERE event_id=1 AND user_id=101")
check("A5 the freed seat admits B", new_register(d, 1, 102) == "ok")
r_a2 = new_register(d, 1, 101)   # A tries to come back — seat now held by B
check("A6 a cancelled member re-registering into a full event gets full, not a seat",
      r_a2 == "full" and d.execute("SELECT status FROM event_registrations WHERE event_id=1 AND user_id=101").fetchone()["status"] == "cancelled")
d.execute("UPDATE event_registrations SET status='cancelled' WHERE event_id=1 AND user_id=102")
check("A7 once B cancels, A's re-registration succeeds via the guarded UPDATE",
      new_register(d, 1, 101) == "ok" and
      d.execute("SELECT status FROM event_registrations WHERE event_id=1 AND user_id=101").fetchone()["status"] == "registered")
# capacity 0 = unlimited
d = fresh_db()
mkevent(d, 2, capacity=0)
check("A8 an unlimited-capacity event (capacity 0) admits everyone",
      all(new_register(d, 2, u) == "ok" for u in (201, 202, 203)))

# ── B. ID-10: the 'upcoming' predicate ─────────────────────────────────────────────────────────
d = fresh_db()
today = datetime.now(timezone.utc).strftime("%Y-%m-%d")
mkevent(d, 10, starts_at="2020-01-01 09:00:00")                    # long past
mkevent(d, 11, starts_at="2999-01-01 09:00:00")                    # far future
mkevent(d, 12, starts_at=None)                                     # undated
mkevent(d, 13, starts_at="")                                       # blank
mkevent(d, 14, starts_at=today + "T00:05")                         # TODAY in 'T' form — the lexical trap
mkevent(d, 15, starts_at=today + " 00:05:00")                      # today in space form
mkevent(d, 16, starts_at="2020-01-01", status="draft")             # past AND unpublished
ids = set(list_upcoming_ids(d))
check("B1 a past published event is excluded from the upcoming list", 10 not in ids, str(sorted(ids)))
check("B2 future, undated and blank-dated events remain listed", {11, 12, 13} <= ids, str(sorted(ids)))
check("B3 an event later TODAY in 'YYYY-MM-DDTHH:MM' form is NOT lost to the 'T' lexical trap",
      14 in ids, str(sorted(ids)))
check("B4 an event later today in space form is listed too", 15 in ids)
check("B5 drafts stay unlisted regardless of date", 16 not in ids)
check("B6 registering for the past event is refused as event_past", new_register(d, 10, 300) == "event_past")
check("B7 the refusal wrote no registration row", len(regs(d, 10)) == 0)
check("B8 registering for an undated event still works", new_register(d, 12, 300) == "ok")
# IsPastEvent (the C# check) classifies exactly like the SQL predicate for every listed shape.
sql_included = {r["id"] for r in d.execute("SELECT id, starts_at FROM events WHERE " + UPCOMING)}
py_included = {r["id"] for r in d.execute("SELECT id, starts_at FROM events")
               if not is_past_event(r["starts_at"])}
check("B9 the C# IsPastEvent rule and the SQL Upcoming predicate agree on every row",
      sql_included == py_included, f"sql={sorted(sql_included)} py={sorted(py_included)}")

# ── C. ID-11: cancelled registrations are not attendable ───────────────────────────────────────
d = fresh_db()
mkevent(d, 1, capacity=0, hours=2, title="Webinar C")
new_register(d, 1, 101)
d.execute("UPDATE event_registrations SET status='cancelled' WHERE event_id=1 AND user_id=101")
res = mark_attended(d, 1, 101)
row = d.execute("SELECT status,cpd_entry_id FROM event_registrations WHERE event_id=1 AND user_id=101").fetchone()
check("C1 marking a cancelled registration attended is refused", res == "cancelled", res)
check("C2 the refusal changes nothing and credits no CPD",
      row["status"] == "cancelled" and row["cpd_entry_id"] is None and cpd_count(d, 1, 101) == 0)
check("C3 an unregistered user is refused as not_registered", mark_attended(d, 1, 999) == "not_registered")
# Un-mark never resurrects a cancelled registration.
res = unmark_attended(d, 1, 101)
check("C4 un-marking a cancelled registration is a no-op, never a resurrection",
      res == "already" and
      d.execute("SELECT status FROM event_registrations WHERE event_id=1 AND user_id=101").fetchone()["status"] == "cancelled")

# ── D. ID-12: one transaction, exactly-once CPD ────────────────────────────────────────────────
d = fresh_db()
mkevent(d, 1, capacity=0, hours=2, title="Webinar D", starts_at="2026-09-01 10:00:00")
new_register(d, 1, 101)
# A crash between the CPD insert and the attendance flip must leave NOTHING behind.
res = mark_attended(d, 1, 101, crash_between_writes=True)
row = d.execute("SELECT status,cpd_entry_id FROM event_registrations WHERE event_id=1 AND user_id=101").fetchone()
check("D1 a crash between the two writes rolls the CPD credit back with it",
      res == "crashed" and cpd_count(d, 1, 101) == 0 and row["status"] == "registered" and row["cpd_entry_id"] is None,
      f"res={res} cpd={cpd_count(d, 1, 101)} status={row['status']}")
# The retry then credits exactly once.
check("D2 the retry succeeds and credits exactly one CPD entry",
      mark_attended(d, 1, 101) == "ok" and cpd_count(d, 1, 101) == 1)
entry = d.execute("SELECT status,hours,activity_date,source_event_id FROM cpd_entries WHERE source_event_id=1 AND user_id=101").fetchone()
check("D3 the credit is approved, for the event's hours, dated from the event, tagged with its source",
      entry["status"] == "approved" and entry["hours"] == 2 and entry["activity_date"] == "2026-09-01" and entry["source_event_id"] == 1,
      dict(entry).__repr__())
check("D4 re-marking is idempotent (already, still one entry)",
      mark_attended(d, 1, 101) == "already" and cpd_count(d, 1, 101) == 1)
# The data layer itself refuses a duplicate — even from code that bypasses the handler.
try:
    d.execute("INSERT INTO cpd_entries(user_id,activity_date,category,hours,description,status,source_event_id) "
              "VALUES(101,'2026-09-01','Events & webinars',2,'dup probe','approved',1)")
    check("D5 ux_cpd_event_user refuses a second credit for the same (event, user)", False, "insert succeeded")
except sqlite3.IntegrityError:
    check("D5 ux_cpd_event_user refuses a second credit for the same (event, user)", True)
# Ordinary member-logged CPD (no source event) is exempt from the index.
d.execute("INSERT INTO cpd_entries(user_id,activity_date,hours,description) VALUES(101,'2026-01-01',1,'self 1')")
d.execute("INSERT INTO cpd_entries(user_id,activity_date,hours,description) VALUES(101,'2026-01-02',1,'self 2')")
check("D6 member-logged CPD (NULL source_event_id) is never blocked by the index", True)
# A pre-fix orphan (credited but never linked, from the old non-transactional path) is ADOPTED.
d2 = fresh_db()
mkevent(d2, 1, capacity=0, hours=3, title="Webinar D7")
new_register(d2, 1, 101)
d2.execute("INSERT INTO cpd_entries(user_id,activity_date,category,hours,description,status,source_event_id) "
           "VALUES(101,'2026-01-01','Events & webinars',3,'Attended: Webinar D7','approved',1)")
check("D7 marking attended adopts a pre-existing orphan credit instead of duplicating it",
      mark_attended(d2, 1, 101) == "ok" and cpd_count(d2, 1, 101) == 1)
linked = d2.execute("SELECT cpd_entry_id FROM event_registrations WHERE event_id=1 AND user_id=101").fetchone()["cpd_entry_id"]
orphan = d2.execute("SELECT id FROM cpd_entries WHERE source_event_id=1 AND user_id=101").fetchone()["id"]
check("D8 the adopted orphan is now the linked entry", linked == orphan, f"linked={linked} orphan={orphan}")
# Un-mark removes the credit and the link atomically; a second un-mark is a no-op.
check("D9 un-marking removes the credit and reverts the registration",
      unmark_attended(d2, 1, 101) == "ok" and cpd_count(d2, 1, 101) == 0 and
      d2.execute("SELECT status,cpd_entry_id FROM event_registrations WHERE event_id=1 AND user_id=101").fetchone()["status"] == "registered")
check("D10 mark → unmark → mark credits exactly one entry again (never accumulates)",
      mark_attended(d2, 1, 101) == "ok" and cpd_count(d2, 1, 101) == 1)
# A zero-hour event flips attendance without minting an empty CPD entry.
d3 = fresh_db()
mkevent(d3, 1, capacity=0, hours=0)
new_register(d3, 1, 101)
check("D11 a zero-hour event marks attended with no CPD entry",
      mark_attended(d3, 1, 101) == "ok" and cpd_count(d3, 1, 101) == 0 and
      d3.execute("SELECT status FROM event_registrations WHERE event_id=1 AND user_id=101").fetchone()["status"] == "attended")

# ── E. the MySQL translation of the new SQL stays inside Db.Translate's rules ──────────────────
# Transcribes the TranslateFor steps that touch the new statements and asserts no SQLite-only
# construct survives (execution parity is CI's MariaDB job; this guards the shapes).
def translate_for_mysql(s):
    s = s.replace("datetime('now')", "DATE_FORMAT(UTC_TIMESTAMP(),'%Y-%m-%d %H:%i:%s')")
    s = s.replace("date('now')", "DATE_FORMAT(UTC_TIMESTAMP(),'%Y-%m-%d')")
    s = re.sub(r"(CREATE UNIQUE INDEX[^;]*?)\s+WHERE[^;]*", r"\1", s)
    s = s.replace("INTEGER PRIMARY KEY AUTOINCREMENT", "BIGINT PRIMARY KEY AUTO_INCREMENT")
    s = re.sub(r"\bINTEGER\b", "BIGINT", s)
    return s

t = translate_for_mysql(UPCOMING)
check("E1 the upcoming predicate translates cleanly (no date('now') survives)",
      "date('now')" not in t and "DATE_FORMAT(UTC_TIMESTAMP(),'%Y-%m-%d')" in t, t)
t = translate_for_mysql(GUARDED_UPDATE)
check("E2 the guarded UPDATE reads the count through a derived table (MySQL error-1093 safe)",
      "FROM (SELECT 1 FROM event_registrations" in t and "datetime('now')" not in t)
t = translate_for_mysql(GUARDED_INSERT)
check("E3 the guarded INSERT selects FROM a real table (no FROM-less SELECT ... WHERE)",
      "FROM events e2 WHERE" in t)
t = translate_for_mysql("CREATE UNIQUE INDEX IF NOT EXISTS ux_cpd_event_user ON cpd_entries(source_event_id,user_id) WHERE source_event_id IS NOT NULL")
check("E4 the partial-index predicate is stripped for MySQL (NULLs already exempt there)",
      t.strip() == "CREATE UNIQUE INDEX IF NOT EXISTS ux_cpd_event_user ON cpd_entries(source_event_id,user_id)", t)

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All ID-10 / ID-11 / ID-12 event-defect assertions passed.")
