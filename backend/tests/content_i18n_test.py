#!/usr/bin/env python3
"""Learning-content translation (item_i18n) logic + wiring assertions.

Replicates Core/ItemI18n.cs against a real SQLite database — the same overlay rules the exam,
practice, sim-lab and books endpoints run — and asserts (against source) that the endpoints are
actually wired to them. The rules under test:

  1. English is always the fallback: a missing/empty translation leaves the source value.
  2. Only display text moves: ids and answer keys never change with language.
  3. An `options` translation applies ONLY when it is a JSON array with exactly the English
     option count — anything else keeps English, so answer_index can never drift.
  4. Unknown languages (and 'en') are no-ops.
  5. The admin translate path stores per-field rows keyed (entity, entity_id, lang, field).
"""
import json
import os
import re
import sqlite3
import sys

PASS = 0
FAIL = 0


def check(name, ok, detail=""):
    global PASS, FAIL
    if ok:
        PASS += 1
        print(f"PASS  {name}")
    else:
        FAIL += 1
        print(f"FAIL  {name}  {detail}")


ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))


def src(path):
    with open(os.path.join(ROOT, path), encoding="utf-8") as f:
        return f.read()


# ── replicate the schema ────────────────────────────────────────────────────────────────────
db = sqlite3.connect(":memory:")
db.executescript(
    """
    CREATE TABLE sample_questions(
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      question TEXT NOT NULL UNIQUE, options TEXT, option_a TEXT, option_b TEXT, option_c TEXT, option_d TEXT,
      answer_index INTEGER, domain TEXT, published INTEGER DEFAULT 1, sort_order INTEGER,
      is_practice INTEGER DEFAULT 0, explanation TEXT, difficulty TEXT, certification_id INTEGER DEFAULT 1);
    CREATE TABLE item_i18n(
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      entity TEXT NOT NULL, entity_id INTEGER NOT NULL, lang TEXT NOT NULL,
      field TEXT NOT NULL, value TEXT NOT NULL, updated_at TEXT DEFAULT (datetime('now')));
    CREATE UNIQUE INDEX ux_item_i18n ON item_i18n(entity, entity_id, lang, field);
    """
)

LANGS = {"ko", "ar", "es", "fr", "zh", "ru"}


def resolve(lang):
    l = (lang or "").strip().lower()
    return l if l and l != "en" and l in LANGS else "en"


def i18n_options(stored, english_count):
    """ItemI18n.Options — same-length JSON array of non-blank strings, else None."""
    if not stored or english_count <= 0:
        return None
    try:
        parsed = json.loads(stored)
    except Exception:
        return None
    if not isinstance(parsed, list) or len(parsed) != english_count:
        return None
    if any((not isinstance(x, str)) or (not x.strip()) for x in parsed):
        return None
    return parsed


def options_for(row):
    """H.OptionsFor — the four columns first, else the JSON list."""
    four = [row[k] for k in ("option_a", "option_b", "option_c", "option_d") if row.get(k) and str(row[k]).strip()]
    if four:
        return [str(x).strip() for x in four]
    try:
        return list(json.loads(row.get("options") or "[]"))
    except Exception:
        return []


def questions(lang, rows):
    """ItemI18n.Questions — the served paper projection."""
    lang = resolve(lang)
    ids = [r["id"] for r in rows]
    tr = {}
    if lang != "en" and ids:
        q = f"SELECT entity_id,field,value FROM item_i18n WHERE entity='question' AND lang=? AND entity_id IN ({','.join(str(i) for i in ids)})"
        for eid, field, value in db.execute(q, (lang,)):
            if value:
                tr[(eid, field)] = value
    out = []
    for r in rows:
        rid = r["id"]
        opts = options_for(r)
        loc = i18n_options(tr.get((rid, "options")), len(opts))
        out.append(
            {
                "id": rid,
                "question": tr.get((rid, "question"), r["question"]),
                "options": loc if loc is not None else opts,
                "domain": tr.get((rid, "domain"), r["domain"]),
            }
        )
    return out


def upsert(entity, eid, lang, field, value):
    """ItemI18n.Upsert — DELETE+INSERT (provider-agnostic)."""
    db.execute("DELETE FROM item_i18n WHERE entity=? AND entity_id=? AND lang=? AND field=?", (entity, eid, lang, field))
    if value:
        db.execute("INSERT INTO item_i18n(entity,entity_id,lang,field,value) VALUES(?,?,?,?,?)", (entity, eid, lang, field, value))


# ── seed one English item ───────────────────────────────────────────────────────────────────
db.execute(
    "INSERT INTO sample_questions(question,option_a,option_b,option_c,answer_index,domain) VALUES(?,?,?,?,?,?)",
    ("What does EVM stand for?", "Earned value management", "Extra vacation month", "Neither", 0, "Planning"),
)
QID = db.execute("SELECT id FROM sample_questions").fetchone()[0]
ROW = dict(zip([c[0] for c in db.execute("SELECT * FROM sample_questions").description],
               db.execute("SELECT * FROM sample_questions").fetchone()))

# 1 — no translations: English serves for every language (fallback, not an error)
for lang in ["ko", "es", "xx", "en", ""]:
    p = questions(lang, [ROW])[0]
    check(f"fallback: '{lang or '(blank)'}' serves English before translation",
          p["question"] == "What does EVM stand for?" and p["options"][0] == "Earned value management")

# 2 — full Korean translation: question/options/domain move, id and answer key do not
upsert("question", QID, "ko", "question", "EVM은 무엇의 약자입니까?")
upsert("question", QID, "ko", "options", json.dumps(["획득가치관리", "추가 휴가의 달", "둘 다 아님"]))
upsert("question", QID, "ko", "domain", "계획")
p = questions("ko", [ROW])[0]
check("ko: question text translated", p["question"] == "EVM은 무엇의 약자입니까?")
check("ko: options translated in order", p["options"] == ["획득가치관리", "추가 휴가의 달", "둘 다 아님"])
check("ko: domain translated", p["domain"] == "계획")
check("ko: id unchanged (answer key language-invariant)", p["id"] == QID)
check("en still byte-identical after ko exists", questions("en", [ROW])[0]["question"] == "What does EVM stand for?")

# 3 — the options integrity guard: wrong count / blank entry / bad JSON all keep English
for bad, label in [
    (json.dumps(["하나", "둘"]), "wrong option count"),
    (json.dumps(["하나", "", "셋"]), "blank option"),
    ("not-json", "unparseable options"),
    (json.dumps({"a": 1}), "non-array options"),
]:
    upsert("question", QID, "fr", "options", bad)
    p = questions("fr", [ROW])[0]
    check(f"options guard: {label} → English options kept", p["options"][0] == "Earned value management")
    upsert("question", QID, "fr", "options", "")   # clear

# 4 — partial translation: translated fields move, untranslated fall back per-field
upsert("question", QID, "es", "question", "¿Qué significa EVM?")
p = questions("es", [ROW])[0]
check("es: partial — question translated", p["question"] == "¿Qué significa EVM?")
check("es: partial — options fall back to English", p["options"][0] == "Earned value management")
check("es: partial — domain falls back to English", p["domain"] == "Planning")

# 5 — upsert is idempotent and replaces
upsert("question", QID, "es", "question", "¿Qué significa la EVM?")
p = questions("es", [ROW])[0]
check("upsert replaces the stored translation", p["question"] == "¿Qué significa la EVM?")
n = db.execute("SELECT COUNT(*) FROM item_i18n WHERE entity='question' AND entity_id=? AND lang='es' AND field='question'", (QID,)).fetchone()[0]
check("upsert leaves exactly one row per (entity,id,lang,field)", n == 1)

# ── source-wiring assertions: the endpoints actually run the overlay ────────────────────────
core = src("Core/ItemI18n.cs")
check("G source: options guard enforces exact count", "parsed.Count != englishCount" in core)
check("G source: overlay never rewrites ids", 'ItemI18n' in core and '["id"] = r["id"]' in core)
check("G source: unknown lang is a no-op", 'lang != "en" && Translator.LangNames.ContainsKey(lang)' in core)

exam = src("Endpoints/StudentExam.cs")
check("G source: exam start serves ItemI18n.Questions", "ItemI18n.Questions(db, lang," in exam)
check("G source: exam start resolves lang from the request", 'ItemI18n.Resolve(ctx, H.GetS(b, "lang", "language"))' in exam)
check("G source: cert-documents listing is overlaid", 'ItemI18n.Overlay(db, "book"' in exam)

certuvo = src("Endpoints/Certuvo.cs")
check("G source: practice paper uses the same projection", "ItemI18n.Questions(db, lang, picked)" in certuvo)
check("G source: practice review overlays question fields", 'ItemI18n.For(db, "question", lang, qids)' in certuvo)

simlab_core = src("Core/SimLab.cs")
check("G source: catalogue overlays scenario display text", 'ItemI18n.Overlay(db, "scenario", lang, scenarios, "title", "summary")' in simlab_core)

simlab_ep = src("Endpoints/SimLab.cs")
check("G source: lab attempt start overlays scenario meta", 'ItemI18n.Overlay(db, "scenario"' in simlab_ep)

admin = src("Endpoints/AdminI18n.cs")
check("G source: admin translate-content endpoint exists", "/api/admin/i18n/translate-content" in admin)
check("G source: admin coverage endpoint exists", "/api/admin/i18n/content-coverage" in admin)
check("G source: option sets stored only when complete", "kv.Value.Any(string.IsNullOrWhiteSpace)" in admin)

migrate = src("Data/Migrate.cs")
check("G source: item_i18n created idempotently", "CREATE TABLE IF NOT EXISTS item_i18n" in migrate)
check("G source: cert_documents lang column added", '"cert_documents", "lang"' in migrate)

schema = src("schema.sql")
check("G source: item_i18n in schema.sql (source of truth)", "CREATE TABLE IF NOT EXISTS item_i18n" in schema)

print("=" * 72)
if FAIL:
    print(f"{FAIL} FAILED, {PASS} passed")
    sys.exit(1)
print(f"All {PASS} learning-content translation assertions passed.")
