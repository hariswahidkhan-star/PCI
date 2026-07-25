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
import re, os, sys

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

def add_index_prefixes(sql):
    # column type map: {table: {col: type_keyword}}
    types = {}
    for m in re.finditer(r"CREATE TABLE(?: IF NOT EXISTS)?\s+(\w+)\s*\((.*?)\n\)", sql, re.S):
        table, body = m.group(1), m.group(2)
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
    sql = open(SRC, encoding="utf-8").read()
    header = ("-- GENERATED from schema.sql by tools/sqlite_to_mysql.py — DO NOT EDIT BY HAND.\n"
              "-- Regenerate after changing schema.sql. Datetimes are VARCHAR strings in the SQLite\n"
              "-- format so the app's string-based date handling is identical across providers.\n"
              "-- Money columns are DECIMAL(12,2); regenerate — do not hand-tune types.\n\n"
              "SET sql_mode='';\n\n")
    open(DST, "w", encoding="utf-8").write(header + convert(sql))
    print("wrote", DST)

if __name__ == "__main__":
    main()
