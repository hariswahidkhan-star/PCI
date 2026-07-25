#!/usr/bin/env python3
"""
Seed a cohort of Simulation Lab students and hand k6 their session tokens.

WHY NOT THE PUBLIC API: /api/register is rate-limited to 10/min/IP, so registering the several
hundred students this cohort needs would take the best part of an hour and would be measuring the
registration limiter. Sessions are stored as sha256(token) (Program.cs mints Security.RandomHex(32)
and stores Security.Sha of it), so a seeder can generate its own token, store the hash, and hand the
plaintext to the load generator. Nothing here is a backdoor: it writes the same rows a real login
writes, using database access the operator already has.

WHY THE COHORT MUST BE LARGE: Endpoints/SimLab.cs throttles attempt writes to 30 per action per user
per 10 minutes. A load test that drives more than that through one student measures the throttle and
reports 429s as failures. Autosave is the binding action -- one lab task costs several autosave
writes but only one start and one submit -- so the cohort is derived from the target with --tasks
rather than guessed with --students. See README.md.

Usage:
    python3 seed_load_cohort.py --tasks 10000 --out cohort.json
    DB_PROVIDER=mysql MYSQL_HOST=... python3 seed_load_cohort.py --tasks 10000 --out cohort.json
"""
import argparse, hashlib, json, math, os, secrets, sys

RL_LIMIT_PER_WINDOW = 30          # Endpoints/SimLab.cs: RL_LIMIT
RL_WINDOW_MINUTES = 10            # Endpoints/SimLab.cs: RL_WINDOW_MS


def cohort_for(tasks: int, autosaves: int, headroom: float) -> int:
    """Students needed so that no account is throttled while the target is driven.

    The whole target is assumed to land inside a single 10-minute window, which is the conservative
    reading: a run that takes longer only gets its budget refreshed. Headroom covers the fact that
    the executor does not deal iterations perfectly evenly across VUs.
    """
    return max(1, math.ceil(tasks * max(1, autosaves) * headroom / RL_LIMIT_PER_WINDOW))


def sha(s: str) -> str:
    return hashlib.sha256(s.encode("utf-8")).hexdigest()


def connect():
    """The same database the backend under test is using — SQLite file or MySQL, chosen the way the
    rest of the test tooling chooses it, so the harness never seeds one store and drives another."""
    if (os.environ.get("DB_PROVIDER") or "").lower() in ("mysql", "mariadb"):
        import pymysql
        con = pymysql.connect(
            host=os.environ.get("MYSQL_HOST", "127.0.0.1"),
            port=int(os.environ.get("MYSQL_PORT", "3306")),
            user=os.environ.get("MYSQL_USER", "pci"),
            password=os.environ.get("MYSQL_PASSWORD", ""),
            database=os.environ.get("MYSQL_DATABASE", "pci"),
            autocommit=False,
        )
        return con, "%s"
    import sqlite3
    path = os.environ.get("DATABASE_FILE", "./pci.db")
    if not os.path.exists(path):
        sys.exit(f"no SQLite database at {path} — boot the backend once, or set DATABASE_FILE")
    return sqlite3.connect(path), "?"


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--tasks", type=int,
                    help="target lab tasks the run will drive. The cohort size is computed from this "
                         "and is the option you normally want.")
    ap.add_argument("--autosaves", type=int, default=4,
                    help="autosave writes per task; must match the k6 -e AUTOSAVES value (default 4)")
    ap.add_argument("--headroom", type=float, default=1.25,
                    help="safety factor over the computed minimum (default 1.25)")
    ap.add_argument("--students", type=int,
                    help="explicit cohort size, overriding --tasks. Each student may perform %d writes "
                         "per action per %d minutes before the app throttles them."
                         % (RL_LIMIT_PER_WINDOW, RL_WINDOW_MINUTES))
    ap.add_argument("--out", default="cohort.json", help="where to write tokens for k6")
    ap.add_argument("--prefix", default="loadlab", help="email prefix, so the cohort is identifiable and removable")
    args = ap.parse_args()

    if args.students is None:
        if args.tasks is None:
            ap.error("give --tasks (preferred, sizes the cohort for you) or --students")
        args.students = cohort_for(args.tasks, args.autosaves, args.headroom)
        print(f"sizing for {args.tasks} tasks x {args.autosaves} autosaves = "
              f"{args.tasks * args.autosaves} autosave writes / {RL_LIMIT_PER_WINDOW} per student "
              f"x {args.headroom} headroom -> {args.students} students")

    con, ph = connect()
    cur = con.cursor()

    # A published, interactive scenario to drive. Interactive means config_json is present — the
    # attempt endpoint answers 409 not_interactive without it, which would make the run measure a
    # rejection path rather than the lab.
    cur.execute("SELECT scenario_code FROM simulation_scenarios "
                "WHERE status='published' AND config_json IS NOT NULL AND config_json<>'' "
                "ORDER BY id LIMIT 1")
    row = cur.fetchone()
    if not row:
        sys.exit("no published interactive scenario — boot the backend so the content pack seeds")
    scenario_code = row[0]

    tokens = []
    for i in range(args.students):
        email = f"{args.prefix}-{i}@load.pci.local"
        cur.execute(f"SELECT id FROM users WHERE email={ph}", (email,))
        found = cur.fetchone()
        if found:
            uid = found[0]
        else:
            cur.execute(
                f"INSERT INTO users(email,first_name,last_name,role,status) "
                f"VALUES({ph},{ph},{ph},'student','active')",
                (email, "Load", f"Student{i}"))
            cur.execute(f"SELECT id FROM users WHERE email={ph}", (email,))
            uid = cur.fetchone()[0]

        # Eligibility: SimLab.Eligible defaults to membership_or_exam, so an active membership is the
        # smallest thing that opens the lab. Seeding it directly keeps the run about the lab.
        cur.execute(f"SELECT id FROM memberships WHERE user_id={ph} AND status='active'", (uid,))
        if not cur.fetchone():
            cur.execute(
                f"INSERT INTO memberships(user_id,membership_type,status,expiry_date) "
                f"VALUES({ph},'Student Membership','active',{ph})",
                (uid, "2030-01-01 00:00:00"))

        token = secrets.token_hex(32)
        cur.execute(
            f"INSERT INTO login_tokens(user_id,token,purpose,expires_at) "
            f"VALUES({ph},{ph},'session',{ph})",
            (uid, sha(token), "2030-01-01 00:00:00"))
        tokens.append(token)

    con.commit()
    con.close()

    capacity = args.students * RL_LIMIT_PER_WINDOW
    with open(args.out, "w") as f:
        json.dump({"scenario_code": scenario_code, "tokens": tokens}, f)
    print(f"seeded {len(tokens)} students · scenario {scenario_code} · wrote {args.out}")
    print(f"throttle headroom: {capacity} writes per action per {RL_WINDOW_MINUTES} minutes "
          f"({args.students} students x {RL_LIMIT_PER_WINDOW}) "
          f"= {capacity // max(1, args.autosaves)} tasks' worth of autosaves")


if __name__ == "__main__":
    main()
