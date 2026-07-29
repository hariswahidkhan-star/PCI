#!/usr/bin/env python3
"""Dedicated parser tests for tools/sqlite_to_mysql.py.

The generator's table scanner is the single source of the column-type map that decides which
indexed columns get a (191) key-length prefix on MySQL. A parsing miss is SILENT: the table simply
vanishes from the map and its TEXT indexes come out MySQL-invalid — which is exactly how
ix_redemptions_email once lost its prefix and had to be hand-patched into schema.mysql.sql.
These cases pin the scanner directly instead of relying on generated-output parity alone.
"""
import os, re, sys

sys.path.insert(0, os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), "tools"))
from sqlite_to_mysql import table_bodies, convert, SRC

passed = failed = 0

def check(name, condition, detail=""):
    global passed, failed
    if condition:
        passed += 1
        print(f"  PASS  {name}")
    else:
        failed += 1
        print(f"  FAIL  {name}{': ' + detail if detail else ''}")

def tables(sql):
    return dict(table_bodies(sql))

# ---- one-line table followed by more tables (the site_settings regression) ----
sql = ("CREATE TABLE site_settings ( skey TEXT PRIMARY KEY, svalue TEXT );\n"
       "CREATE TABLE after_one_liner (\n  id INTEGER PRIMARY KEY AUTOINCREMENT,\n  email TEXT\n);\n")
t = tables(sql)
check("one-line CREATE TABLE is closed at its own paren", "site_settings" in t and "svalue TEXT" in t["site_settings"])
check("the table AFTER a one-liner is still discovered", "after_one_liner" in t, f"found={list(t)}")

# ---- multiline table with nested parentheses ----
sql = ("CREATE TABLE nested (\n"
       "  id INTEGER PRIMARY KEY AUTOINCREMENT,\n"
       "  status TEXT CHECK (status IN ('open','closed')),\n"
       "  created_at TEXT DEFAULT (datetime('now'))\n"
       ");\n"
       "CREATE TABLE follows_nested ( id INTEGER );\n")
t = tables(sql)
check("nested parentheses stay balanced", "nested" in t and "follows_nested" in t)
check("nested body is complete", "datetime('now')" in t.get("nested", ""))

# ---- backticked table name ----
t = tables("CREATE TABLE `quoted_name` ( id INTEGER );\nCREATE TABLE plain ( id INTEGER );\n")
check("backticked table name is matched and unquoted", "quoted_name" in t and "plain" in t, f"found={list(t)}")

# ---- parentheses and apostrophes inside string literals ----
sql = ("CREATE TABLE seeded (\n"
       "  label TEXT DEFAULT 'unbalanced ) paren ( inside',\n"
       "  note TEXT DEFAULT 'it''s (tricky)'\n"
       ");\n"
       "CREATE TABLE after_strings ( id INTEGER );\n")
t = tables(sql)
check("parens inside string literals do not unbalance the scan", "seeded" in t and "after_strings" in t, f"found={list(t)}")
check("escaped doubled quotes ('') stay inside the literal", "it''s (tricky)" in t.get("seeded", ""))

# ---- line comments containing apostrophes / parentheses (the honorary_awards regression) ----
sql = ("CREATE TABLE commented (\n"
       "  id INTEGER PRIMARY KEY AUTOINCREMENT, -- the board's reason (free text)\n"
       "  reason TEXT\n"
       ");\n"
       "CREATE TABLE after_comment ( id INTEGER );\n")
t = tables(sql)
check("apostrophe in a -- comment does not open a string", "commented" in t and "after_comment" in t, f"found={list(t)}")

# ---- index key-length prefixes ----
sql = ("CREATE TABLE code_redemptions (\n  id INTEGER PRIMARY KEY AUTOINCREMENT,\n"
       "  email TEXT NOT NULL,\n  code TEXT,\n  attempt_no INTEGER\n);\n"
       "CREATE INDEX ix_redemptions_email ON code_redemptions(email);\n"
       "CREATE UNIQUE INDEX ix_redemptions_pair ON code_redemptions(code, attempt_no);\n")
out = convert(sql)
check("TEXT column in an index gets a (191) prefix", "ON code_redemptions(email(191))" in out, out)
check("composite index prefixes only its TEXT member", "ON code_redemptions(code(191), attempt_no)" in out, out)

# ---- the real schema: every CREATE TABLE the file declares must be in the type map ----
real = open(SRC, encoding="utf-8").read()
declared = len(re.findall(r"^CREATE TABLE", real, flags=re.MULTILINE))
parsed = len(tables(real))
check(f"every CREATE TABLE in schema.sql parses ({parsed}/{declared})", parsed == declared,
      f"declared={declared} parsed={parsed}")
real_out = convert(real)
# Column types must be read from the CONVERTED output (as add_index_prefixes does): many source
# TEXT columns become VARCHAR (TEXT PRIMARY KEY / TEXT UNIQUE) and need no key length.
bad = []
tmap = {tbl: body for tbl, body in table_bodies(real_out)}
# Greedy to the closing paren of the STATEMENT: a prefixed column like slug(191) nests parens
# inside the column list, so a lazy [^)]* would truncate the capture at the first ')'.
for m in re.finditer(r"CREATE (?:UNIQUE )?INDEX(?: IF NOT EXISTS)? \w+ ON (\w+)\((.*)\)\s*;", real_out):
    body = tmap.get(m.group(1), "")
    for col in m.group(2).split(","):
        name = col.strip().split("(")[0].strip("` ")
        if "(191)" in col:
            continue
        if re.search(rf"^\s*`?{re.escape(name)}`?\s+(?:TEXT|MEDIUMTEXT|LONGTEXT)\b", body,
                     flags=re.MULTILINE | re.IGNORECASE):
            bad.append(f"{m.group(1)}.{name}")
check("no generated index leaves a TEXT column without a key length", not bad, f"missing prefix: {bad}")

print(f"\n  == {passed}/{passed + failed} PASSED ==")
raise SystemExit(0 if failed == 0 else 1)
