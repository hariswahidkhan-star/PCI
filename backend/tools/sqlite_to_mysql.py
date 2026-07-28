#!/usr/bin/env python3
"""
Generate schema.mysql.sql from schema.sql (the SQLite source of truth).

The app treats every datetime as a STRING in the exact SQLite format
"YYYY-MM-DD HH:MM:SS", so datetime columns stay VARCHAR here and datetime()
defaults become a DATE_FORMAT(UTC_TIMESTAMP(),...) expression producing the
same string — this keeps all the app's string-based date handling identical
across providers. Run from backend/:  python3 tools/sqlite_to_mysql.py

Money columns are emitted as DECIMAL(12,2) (not DOUBLE). Percentages, scores,
and other non-currency REAL columns remain DOUBLE. Do not hand-tune money
types in schema.mysql.sql — regenerate this file after schema.sql changes.
"""
import argparse, re, os, sys

HERE = os.path.dirname(os.path.abspath(__file__))
SRC = os.path.join(HERE, "..", "schema.sql")
DST = os.path.join(HERE, "..", "schema.mysql.sql")

# MariaDB/MySQL reserved words used as bare column names in this schema → backtick them.
# These identifiers appear ONLY as column names here (never inside strings/functions), so a
# whole-word replace is safe.
RESERVED = ["current_role", "usage"]

NOW = "DATE_FORMAT(UTC_TIMESTAMP(),'%Y-%m-%d %H:%i:%s')"

# Currency / fee columns that must be exact on MySQL. Matched only as DDL column definitions
# (`col DOUBLE` → `col DECIMAL(12,2)`). Percentages (e.g. default_discount_percentage,
# commission_pct, pass_mark_pct) and scores stay DOUBLE.
MONEY_COLUMNS = (
    "standard_price",
    "discount_value",
    "standard_amount",
    "default_discount_amount",
    "discount_code_amount",
    "final_amount",
    "renewal_fee",
    "amount_paid",
    "exam_price",
    "amount_before",
    "discount_amount",
    "application_fee",
    "waived_amount",
    "original_amount",
    "payable_amount",
    "amount_refunded",
    "min_payable",
    "min_transaction",
    "max_discount",
    "fee_amount",
    "requested_amount",
    "net_amount",
    "amount",  # partner_payouts.amount (and any other plain amount money column)
)

def translate_datetime_default(sql):
    # DEFAULT (datetime('now'))  →  DEFAULT (<NOW expression>)
    return sql.replace("DEFAULT (datetime('now'))", f"DEFAULT ({NOW})")

def backtick_reserved(sql):
    for w in RESERVED:
        sql = re.sub(rf"\b{w}\b", f"`{w}`", sql)
    return sql

def money_as_decimal(sql):
    """Rewrite known money DOUBLE columns to DECIMAL(12,2). Runs after REAL→DOUBLE."""
    out = sql
    for col in MONEY_COLUMNS:
        out = re.sub(
            rf"\b({re.escape(col)})\s+DOUBLE\b",
            r"\1 DECIMAL(12,2)",
            out,
            flags=re.IGNORECASE,
        )
    return out

def convert(sql):
    out = sql
    # ---- datetime defaults first (before generic TEXT handling) ----
    out = translate_datetime_default(out)
    # ---- primary keys ----
    out = out.replace("INTEGER PRIMARY KEY AUTOINCREMENT", "BIGINT PRIMARY KEY AUTO_INCREMENT")
    # ---- TEXT as PRIMARY KEY / UNIQUE must become VARCHAR (MySQL can't key TEXT without a prefix) ----
    out = out.replace("TEXT PRIMARY KEY", "VARCHAR(191) PRIMARY KEY")
    #   forms: "TEXT UNIQUE NOT NULL", "TEXT NOT NULL UNIQUE", "TEXT UNIQUE"
    out = out.replace("TEXT UNIQUE NOT NULL", "VARCHAR(500) UNIQUE NOT NULL")
    out = out.replace("TEXT NOT NULL UNIQUE", "VARCHAR(500) NOT NULL UNIQUE")
    out = out.replace("TEXT UNIQUE", "VARCHAR(500) UNIQUE")
    # ---- scalar types ----
    out = re.sub(r"\bINTEGER\b", "BIGINT", out)
    out = re.sub(r"\bREAL\b", "DOUBLE", out)
    # ---- money: exact fixed-point on MySQL (must follow REAL→DOUBLE) ----
    out = money_as_decimal(out)
    # ---- Oracle MySQL requires BLOB/TEXT literal defaults to be parenthesised expressions.
    # MariaDB accepts this spelling too, so the generated schema remains portable. ----
    out = re.sub(
        r"\bTEXT\b(\s+NOT\s+NULL)?\s+DEFAULT\s+('(?:''|[^'])*')",
        r"TEXT\1 DEFAULT (\2)",
        out,
        flags=re.IGNORECASE,
    )
    # ---- strip inline REFERENCES: SQLite FKs here are advisory (the app enforces relationships in
    #      code); MySQL would enforce them strictly with load-order/type constraints, changing behaviour. ----
    out = re.sub(r"\s+REFERENCES\s+\w+\s*\([^)]*\)", "", out)
    # ---- INSERT OR IGNORE (seed rows) ----
    out = out.replace("INSERT OR IGNORE", "INSERT IGNORE")
    # ---- partial unique indexes: MySQL treats NULLs as distinct already, so drop the WHERE ----
    out = re.sub(r"(CREATE UNIQUE INDEX[^\n;]*?)\s+WHERE[^\n;]*", r"\1", out)
    # ---- reserved-word column names ----
    out = backtick_reserved(out)
    # ---- TEXT columns in indexes need a prefix length in MySQL. Build a column→type map from the
    #      CREATE TABLE blocks, then add a (191) prefix to any TEXT column named in a CREATE INDEX. ----
    out = add_index_prefixes(out)
    # ---- charset on tables: append ENGINE/charset to each CREATE TABLE (...) block ----
    # done by post-processing: add a default at connection level instead (simpler + safe).
    return out

def table_bodies(sql):
    """Yield (table, body) for every CREATE TABLE by scanning to the BALANCED closing paren.
    A regex that stops at the first "\n)" mis-parses one-line tables (e.g. site_settings): the
    non-greedy match runs past the real close into later tables, silently dropping them from the
    type map — which is how code_redemptions lost its email(191) index prefix."""
    for m in re.finditer(r"CREATE TABLE(?: IF NOT EXISTS)?\s+`?(\w+)`?\s*\(", sql):
        depth, i = 1, m.end()
        while i < len(sql) and depth > 0:
            ch = sql[i]
            if ch == "'":  # skip string literals (may contain parens)
                i += 1
                while i < len(sql) and sql[i] != "'":
                    i += 1
            elif ch == "-" and sql[i:i+2] == "--":  # skip line comments (may contain parens)
                while i < len(sql) and sql[i] != "\n":
                    i += 1
            elif ch == "(":
                depth += 1
            elif ch == ")":
                depth -= 1
            i += 1
        yield m.group(1), sql[m.end():i-1]

def add_index_prefixes(sql):
    # column type map: {table: {col: type_keyword}}
    types = {}
    for table, body in table_bodies(sql):
        cols = {}
        for line in body.split("\n"):
            line = line.strip().strip(",")
            if not line or line.startswith("--") or line.upper().startswith(("PRIMARY", "UNIQUE(", "FOREIGN", "CONSTRAINT")):
                continue
            # possibly several "col TYPE ..." on one comma-separated line
            for part in line.split(","):
                part = part.strip()
                mm = re.match(r"`?(\w+)`?\s+(\w+)", part)
                if mm:
                    cols[mm.group(1).lower()] = mm.group(2).upper()
        types[table.lower()] = cols

    def fix_index(m):
        head, table, collist = m.group(1), m.group(2), m.group(3)
        tcols = types.get(table.lower(), {})
        parts = []
        for c in collist.split(","):
            name = c.strip().strip("`").split("(")[0].strip()
            if tcols.get(name.lower()) in ("TEXT", "LONGTEXT", "MEDIUMTEXT"):
                parts.append(f"{name}(191)")
            else:
                parts.append(name)
        return f"{head}{table}({', '.join(parts)})"

    return re.sub(r"(CREATE (?:UNIQUE )?INDEX(?: IF NOT EXISTS)? \w+ ON )(\w+)\(([^)]*)\)", fix_index, sql)

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--check", action="store_true",
                        help="fail if committed schema.mysql.sql is not current")
    args = parser.parse_args()
    sql = open(SRC, encoding="utf-8").read()
    header = ("-- GENERATED from schema.sql by tools/sqlite_to_mysql.py — DO NOT EDIT BY HAND.\n"
              "-- Regenerate after changing schema.sql. Datetimes are VARCHAR strings in the SQLite\n"
              "-- format so the app's string-based date handling is identical across providers.\n"
              "-- Money columns are DECIMAL(12,2); regenerate — do not hand-tune types.\n\n"
              "SET SESSION sql_mode='PIPES_AS_CONCAT';\n\n")
    generated = header + convert(sql)
    if args.check:
        current = open(DST, encoding="utf-8").read() if os.path.exists(DST) else ""
        if current != generated:
            print("schema.mysql.sql is stale; run python3 tools/sqlite_to_mysql.py", file=sys.stderr)
            return 1
        print("schema.mysql.sql is current")
        return 0
    open(DST, "w", encoding="utf-8").write(generated)
    print("wrote", DST)
    return 0

if __name__ == "__main__":
    raise SystemExit(main())
