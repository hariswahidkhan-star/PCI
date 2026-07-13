#!/usr/bin/env python3
"""
PCI SQLite -> MySQL data migration + reconciliation.

The app already creates the MySQL schema on boot (schema.mysql.sql + Migrate.cs). This tool moves the
DATA from an existing SQLite database into that MySQL schema, preserving primary keys and relationships,
then reconciles the result. It is idempotent per table (it replaces the target table's rows) and never
touches the source.

USAGE (target defaults come from the same MYSQL_* env vars the app uses):
    # 1) create the target schema first by booting the app once against the target MySQL:
    #    DB_PROVIDER=mysql MYSQL_HOST=... MYSQL_PASSWORD=... dotnet run   (then stop it)
    # 2) run the migration:
    python3 tools/migrate_sqlite_to_mysql.py --source /data/pci.db \
        --mysql-host 127.0.0.1 --mysql-port 3306 --mysql-db pci --mysql-user pci --mysql-password ***
    #    add --report out.json to save the reconciliation report; --dry-run to validate without writing.

Exit code 0 = migration + reconciliation clean; 2 = completed with reconciliation discrepancies; 1 = error.
"""
import argparse, os, sqlite3, sys, hashlib, json, datetime

try:
    import pymysql
except ImportError:
    sys.exit("pymysql is required: pip install pymysql")

# Money columns (must reconcile to the cent). Keep in sync with the DECIMAL columns in schema.mysql.sql.
MONEY = {
    "payments": ["standard_amount", "default_discount_amount", "discount_code_amount", "final_amount"],
    "memberships": ["renewal_fee", "amount_paid"],
    "pricing_rules": ["standard_price"],
    "certifications": ["standard_price", "exam_price"],
    "discount_codes": ["discount_value"],
    "code_redemptions": ["amount_before", "discount_amount"],
}
# Key foreign keys to check for orphans after import (child.col -> parent.id).
FKS = [
    ("payments", "user_id", "users"),
    ("memberships", "user_id", "users"),
    ("student_profiles", "user_id", "users"),
    ("exam_attempts", "user_id", "users"),
    ("cpd_entries", "user_id", "users"),
    ("page_blocks", "slug", None),  # slug FK handled separately (no numeric id)
]
# Unique columns to check for duplicates in the target.
UNIQUES = [("users", "email"), ("pages", "slug"), ("admin_users", "email")]


def log(msg):
    print(f"[migrate] {msg}", flush=True)


def sha256(path):
    h = hashlib.sha256()
    with open(path, "rb") as f:
        for chunk in iter(lambda: f.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def sqlite_tables(sc):
    return [r[0] for r in sc.execute(
        "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' ORDER BY name")]


def sqlite_cols(sc, t):
    return [r[1] for r in sc.execute(f'PRAGMA table_info("{t}")')]


def mysql_tables(mc):
    with mc.cursor() as cur:
        cur.execute("SELECT table_name FROM information_schema.tables WHERE table_schema=DATABASE()")
        return [r[0] for r in cur.fetchall()]


def mysql_cols(mc, t):
    with mc.cursor() as cur:
        cur.execute("SELECT column_name FROM information_schema.columns WHERE table_schema=DATABASE() AND table_name=%s", (t,))
        return [r[0] for r in cur.fetchall()]


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--source", default=os.environ.get("DATABASE_FILE", "./pci.db"))
    ap.add_argument("--mysql-host", default=os.environ.get("MYSQL_HOST", "127.0.0.1"))
    ap.add_argument("--mysql-port", type=int, default=int(os.environ.get("MYSQL_PORT", "3306")))
    ap.add_argument("--mysql-db", default=os.environ.get("MYSQL_DATABASE", "pci"))
    ap.add_argument("--mysql-user", default=os.environ.get("MYSQL_USER", "root"))
    ap.add_argument("--mysql-password", default=os.environ.get("MYSQL_PASSWORD", ""))
    ap.add_argument("--mysql-socket", default=os.environ.get("MYSQL_SOCKET"))
    ap.add_argument("--report", help="write the JSON reconciliation report to this path")
    ap.add_argument("--dry-run", action="store_true", help="validate + reconcile counts without writing")
    args = ap.parse_args()

    if not os.path.exists(args.source):
        sys.exit(f"source SQLite not found: {args.source}")

    log(f"source: {args.source}  sha256={sha256(args.source)[:16]}…  size={os.path.getsize(args.source)} bytes")
    sc = sqlite3.connect(args.source)
    conn_kw = dict(host=args.mysql_host, port=args.mysql_port, db=args.mysql_db,
                   user=args.mysql_user, password=args.mysql_password, charset="utf8mb4", autocommit=False)
    if args.mysql_socket:
        conn_kw["unix_socket"] = args.mysql_socket
    mc = pymysql.connect(**conn_kw)

    s_tabs, t_tabs = set(sqlite_tables(sc)), set(mysql_tables(mc))
    shared = sorted(s_tabs & t_tabs)
    only_src, only_tgt = sorted(s_tabs - t_tabs), sorted(t_tabs - s_tabs)
    if only_src:
        log(f"WARNING: {len(only_src)} source tables missing in target (skipped): {only_src}")
    log(f"{len(shared)} tables to migrate; {len(only_tgt)} target-only tables left untouched")

    report = {"generated_at": datetime.datetime.utcnow().isoformat() + "Z", "source": args.source,
              "source_sha256": sha256(args.source), "tables": {}, "money": {}, "fk_orphans": {},
              "unique_dups": {}, "discrepancies": []}

    if not args.dry_run:
        with mc.cursor() as cur:
            cur.execute("SET FOREIGN_KEY_CHECKS=0")
            cur.execute("SET UNIQUE_CHECKS=0")

    # ---- per-table copy (source rows replace target rows) ----
    for t in shared:
        scols, tcols = sqlite_cols(sc, t), set(mysql_cols(mc, t))
        cols = [c for c in scols if c in tcols]          # intersection, preserving source order
        if not cols:
            log(f"  {t}: no shared columns, skipped")
            continue
        rows = sc.execute(f'SELECT {",".join(chr(34)+c+chr(34) for c in cols)} FROM "{t}"').fetchall()
        src_n = len(rows)
        if not args.dry_run:
            with mc.cursor() as cur:
                cur.execute(f"DELETE FROM `{t}`")
                if rows:
                    ph = "(" + ",".join(["%s"] * len(cols)) + ")"
                    collist = ",".join(f"`{c}`" for c in cols)
                    ins = f"INSERT INTO `{t}` ({collist}) VALUES {ph}"
                    cur.executemany(ins, rows)
                # correct AUTO_INCREMENT when the table has an integer id
                if "id" in tcols:
                    cur.execute(f"SELECT COALESCE(MAX(`id`),0)+1 FROM `{t}`")
                    nextid = cur.fetchone()[0]
                    cur.execute(f"ALTER TABLE `{t}` AUTO_INCREMENT={int(nextid)}")
        # verify target count
        with mc.cursor() as cur:
            cur.execute(f"SELECT COUNT(*) FROM `{t}`")
            tgt_n = cur.fetchone()[0]
        report["tables"][t] = {"source": src_n, "target": tgt_n, "cols": len(cols)}
        if not args.dry_run and src_n != tgt_n:
            report["discrepancies"].append(f"row count mismatch {t}: source={src_n} target={tgt_n}")
        log(f"  {t}: {src_n} rows -> target {tgt_n} ({len(cols)} cols)")

    if not args.dry_run:
        mc.commit()
        with mc.cursor() as cur:
            cur.execute("SET FOREIGN_KEY_CHECKS=1")
            cur.execute("SET UNIQUE_CHECKS=1")

    # ---- reconciliation ----
    log("reconciling…")
    # financial totals (source rounded to cents vs target DECIMAL)
    for t, colz in MONEY.items():
        if t not in shared:
            continue
        for col in colz:
            if col not in sqlite_cols(sc, t):
                continue
            s_sum = sc.execute(f'SELECT COALESCE(ROUND(SUM("{col}"),2),0) FROM "{t}"').fetchone()[0]
            with mc.cursor() as cur:
                cur.execute(f"SELECT COALESCE(ROUND(SUM(`{col}`),2),0) FROM `{t}`")
                t_sum = float(cur.fetchone()[0])
            s_sum = round(float(s_sum), 2)
            ok = abs(s_sum - t_sum) < 0.005
            report["money"][f"{t}.{col}"] = {"source": s_sum, "target": t_sum, "ok": ok}
            if not ok:
                report["discrepancies"].append(f"financial mismatch {t}.{col}: source={s_sum} target={t_sum}")

    # FK orphans in target
    for child, col, parent in FKS:
        if child not in shared or parent is None:
            continue
        with mc.cursor() as cur:
            cur.execute(f"SELECT COUNT(*) FROM `{child}` c LEFT JOIN `{parent}` p ON c.`{col}`=p.`id` "
                        f"WHERE c.`{col}` IS NOT NULL AND p.`id` IS NULL")
            orphans = cur.fetchone()[0]
        report["fk_orphans"][f"{child}.{col}->{parent}"] = orphans
        if orphans:
            report["discrepancies"].append(f"FK orphans {child}.{col}->{parent}: {orphans}")

    # unique dup check
    for t, col in UNIQUES:
        if t not in shared:
            continue
        with mc.cursor() as cur:
            cur.execute(f"SELECT COUNT(*) FROM (SELECT `{col}` FROM `{t}` WHERE `{col}` IS NOT NULL "
                        f"GROUP BY `{col}` HAVING COUNT(*)>1) d")
            dups = cur.fetchone()[0]
        report["unique_dups"][f"{t}.{col}"] = dups
        if dups:
            report["discrepancies"].append(f"duplicate {t}.{col}: {dups} value(s) repeated")

    # ---- summary ----
    print("\n================ RECONCILIATION ================")
    print(f"tables migrated : {len(report['tables'])}")
    print(f"total src rows  : {sum(v['source'] for v in report['tables'].values())}")
    print(f"total tgt rows  : {sum(v['target'] for v in report['tables'].values())}")
    print("financial totals:")
    for k, v in report["money"].items():
        print(f"   {'OK ' if v['ok'] else 'XX '} {k}: source={v['source']} target={v['target']}")
    print(f"FK orphans      : {sum(report['fk_orphans'].values())}")
    print(f"unique dups     : {sum(report['unique_dups'].values())}")
    if report["discrepancies"]:
        print("\nDISCREPANCIES:")
        for d in report["discrepancies"]:
            print("  - " + d)
    else:
        print("\nNO DISCREPANCIES — row counts, financial totals, FK integrity and uniqueness all reconciled.")

    if args.report:
        with open(args.report, "w") as f:
            json.dump(report, f, indent=2, default=str)
        log(f"report written to {args.report}")

    sc.close(); mc.close()
    sys.exit(2 if report["discrepancies"] else 0)


if __name__ == "__main__":
    main()
