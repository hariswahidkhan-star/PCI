#!/usr/bin/env python3
"""Generate the complete schema reference and the ER diagrams from a real migrated database.

Generated rather than hand-written, because a hand-written reference to 290 tables and 3,701 columns
is wrong the day after it is written. Re-run this against a fresh database to regenerate.

Relationships come from two sources and the output keeps them apart:
  DECLARED  — PRAGMA foreign_key_list. Only 19 exist. Authoritative.
  INFERRED  — a column named <thing>_id where a plausible target table exists. This is a naming
              convention, NOT a constraint the database enforces; it is labelled as such so nobody
              reads the ER as a guarantee of referential integrity.
"""
import re
import sqlite3
import sys
from collections import defaultdict
from pathlib import Path

DB = sys.argv[1] if len(sys.argv) > 1 else "/tmp/erd.db"

# Domain grouping. Order matters: the first matching rule wins, so put specific prefixes first.
DOMAINS = [
    ("PCI World — community & moderation",
     r"^pciworld_(room|message|moderation|community|policy|case|report|guest|media|image|mod_)"),
    ("PCI World — forum", r"^pciworld_(forum|thread|post|trust)"),
    ("Forum (platform)", r"^forum_"),
    ("PCI World — careers", r"^pciworld_(employer|job|application|career)"),
    ("PCI World — editorial & contributors",
     r"^pciworld_(article|version|source|correction|contributor|editorial)"),
    ("PCI World — challenges, rotation & intelligence",
     r"^pciworld_(challenge|rotation|attempt|intelligence|score|taxonom)"),
    ("PCI World — identity, passport & admin", r"^pciworld_"),
    ("Students & identity",
     r"^(users|student_|login_|admin_users|admin_sessions|admin_reset|impersonation_|revoked_|"
     r"identity|membership|member_|consent|candidate_consents|onboarding|enrollment_|"
     r"account_requests|pci_identity_merges|pci_student_number_registry|work_experiences|"
     r"qualifications|security_events|fraud_flags)"),
    ("Examinations & credentials",
     r"^(exam|sample_questions|results?|issued_credential|proctor|attempt|certificate|cert_|"
     r"credential|badge|certification|held_certifications|practice_attempts|bok_domains|"
     r"governance_roles)"),
    ("Payments, finance & partners",
     r"^(payment|invoice|code_|discount|partner_|training_|commission|settlement|fee_|refund|"
     r"pricing_rules|checkout_reservations|webhook_events)"),
    ("Simulation Lab", r"^(simulation_|sim_|lab_)"),
    ("Content, website & SEO",
     r"^(page_|site_|content_|cc_|blog_|news|faq|nav|resource|review|seo_|redirect|sitemap|"
     r"template|public_doc|announcement|i18n|pages|media_assets|ai_content)"),
    ("Marketing, social & syndication",
     r"^(mkt_|social_|campaign|syndicat|backlink|ad_|utm|analytics_events)"),
    ("Communications & notifications",
     r"^(comm_|email_|notification|message_|newsletter|inquiry|inquiries|subscriber|unsubscribe|"
     r"chat_|form_submissions)"),
    ("Support, casework & documents",
     r"^(ticket|support_|appeal|accommodation|cpd|document|doc_|erasure|error_)"),
    ("Events", r"^(event|attendance|checkin|pass_)"),
    ("Integrations & operations",
     r"^(integration|connector|outbox|worker|job_|audit|retention|honorary|founding|certuvo|"
     r"provisioning|external_|schema_migrations|career_)"),
]


def domain_of(table: str) -> str:
    for name, pattern in DOMAINS:
        if re.match(pattern, table):
            return name
    return "Other"


def main() -> None:
    con = sqlite3.connect(DB)
    tables = [r[0] for r in con.execute(
        "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' ORDER BY name")]
    tset = set(tables)

    cols = {t: list(con.execute(f'PRAGMA table_info("{t}")')) for t in tables}
    fks = {t: list(con.execute(f'PRAGMA foreign_key_list("{t}")')) for t in tables}
    idxs = defaultdict(list)
    for name, tbl, sql in con.execute(
            "SELECT name, tbl_name, sql FROM sqlite_master WHERE type='index' "
            "AND name NOT LIKE 'sqlite_%' ORDER BY tbl_name, name"):
        idxs[tbl].append((name, "UNIQUE" if sql and "UNIQUE" in sql.upper() else "", sql or ""))

    def infer(table: str, column: str):
        """Guess the target of a <thing>_id column. Convention only — never a constraint."""
        if not column.endswith("_id") or column == "id":
            return None
        stem = column[:-3]
        # The table's own prefix matters: forum_posts.thread_id means forum_threads, not a
        # non-existent `threads`. Without this, whole prefixed families (forum_, mkt_, comm_, cc_,
        # exam_, sim_) come out with no relationships at all and their diagrams look empty.
        prefix = table.split("_")[0] + "_" if "_" in table else ""
        cands = [stem, stem + "s", stem + "es", re.sub(r"y$", "ies", stem)]
        if prefix:
            cands += [prefix + stem, prefix + stem + "s", prefix + stem + "es"]
        cands += ["pciworld_" + stem, "pciworld_" + stem + "s"]
        for cand in cands:
            if cand in tset and cand != table:
                return cand
        # user-ish columns almost always mean the canonical users table
        if stem in ("user", "student", "owner", "actor", "author", "reviewer",
                    "approver", "created_by", "holder", "member"):
            return "users" if "users" in tset else None
        return None

    by_domain = defaultdict(list)
    for t in tables:
        by_domain[domain_of(t)].append(t)

    # ---------------- complete schema reference ----------------
    out = [
        "# PCI Platform — Complete Database Schema",
        "",
        "**Generated from a freshly migrated database**, not written by hand — re-run",
        "`gen_schema.py` to regenerate. Counts and types are therefore what the code actually",
        "produces, including every runtime installer, not what a design document intended.",
        "",
        f"- **Tables:** {len(tables)}",
        f"- **Columns:** {sum(len(c) for c in cols.values()):,}",
        f"- **Indexes:** {sum(len(v) for v in idxs.values())}",
        f"- **Declared foreign keys:** {sum(len(f) for f in fks.values())}",
        "",
        "> **Read the foreign-key number before you read the ER diagrams.** Nineteen constraints",
        "> across two hundred and ninety tables means referential integrity is, with a handful of",
        "> exceptions, **maintained by application code rather than enforced by the database**.",
        "> Relationships marked *inferred* in the ER diagrams are naming conventions this generator",
        "> detected — they are real relationships in the code, but nothing stops a bad write.",
        "> Delete a `users` row directly with SQL and you will orphan rows across dozens of tables.",
        "",
        "Types are as declared in SQLite. On MySQL, `Db.Translate` maps them at runtime and",
        "**datetimes are strings** in `YYYY-MM-DD HH:MM:SS` on both providers — see the developer",
        "guide's data-access section.",
        "",
        "---",
        "",
        "## Contents",
        "",
    ]
    for dom, _ in DOMAINS + [("Other", "")]:
        if by_domain.get(dom):
            anchor = re.sub(r"[^a-z0-9 -]", "", dom.lower()).replace(" ", "-")
            out.append(f"- [{dom}](#{anchor}) — {len(by_domain[dom])} tables")
    out += ["", "---", ""]

    for dom, _ in DOMAINS + [("Other", "")]:
        ts = by_domain.get(dom)
        if not ts:
            continue
        out += [f"## {dom}", "", f"*{len(ts)} tables*", ""]
        for t in sorted(ts):
            out += [f"### `{t}`", ""]
            out += ["| Column | Type | Null | Default | Key |", "|---|---|---|---|---|"]
            fkmap = {f[3]: (f[2], f[4]) for f in fks[t]}
            for _, name, ctype, notnull, dflt, pk in cols[t]:
                key = []
                if pk:
                    key.append("PK")
                if name in fkmap:
                    key.append(f"FK → `{fkmap[name][0]}.{fkmap[name][1]}`")
                else:
                    tgt = infer(t, name)
                    if tgt:
                        key.append(f"→ `{tgt}` *(inferred)*")
                d = f"`{dflt}`" if dflt is not None else ""
                out.append(
                    f"| `{name}` | {ctype or 'ANY'} | {'no' if notnull else 'yes'} | {d} | "
                    f"{' · '.join(key)} |")
            out.append("")
            if idxs.get(t):
                out.append("**Indexes:** " + ", ".join(
                    f"`{n}`{' (unique)' if u else ''}" for n, u, _ in idxs[t]))
                out.append("")

    Path("docs/PCI_DATABASE_SCHEMA.md").write_text("\n".join(out), encoding="utf-8")
    print(f"schema reference: {len(out)} lines")

    # ---------------- ER diagrams ----------------
    er = [
        "# PCI Platform — Entity-Relationship Diagrams",
        "",
        "One diagram per domain, because a single diagram of 290 tables is a picture of nothing.",
        "Generated from the live schema alongside the full column reference in",
        "[`PCI_DATABASE_SCHEMA.md`](PCI_DATABASE_SCHEMA.md).",
        "",
        "**How to read these**",
        "",
        "- Solid `||--o{` — a **declared** foreign key. There are only 19 in the whole schema.",
        "- Dotted `}o..o{` — an **inferred** relationship: a `<thing>_id` column pointing at a",
        "  plausible table. Real in the code, **not enforced by the database**.",
        "- Only join-bearing columns are drawn. Every column of every table is in the schema",
        "  reference; repeating them here would make the diagrams unreadable.",
        "",
        "> Because integrity is enforced in application code, the arrows tell you what the code",
        "> intends — not what the database guarantees. Both facts matter when you write a migration",
        "> or a manual fix.",
        "",
        "---",
        "",
    ]

    # A domain's tables often join OUTWARD (forum_posts.user_id -> users) and barely to each other.
    # Drawing only intra-domain edges silently produced no diagram at all for five domains. So each
    # diagram now carries its external anchors too, marked with the domain they belong to.
    dom_of_table = {t: domain_of(t) for t in tables}
    skipped = []

    for dom, _ in DOMAINS + [("Other", "")]:
        ts = sorted(by_domain.get(dom, []))
        if not ts:
            continue
        tsub = set(ts)
        rels, seen = [], set()

        def add(src, dst, kind, label):
            k = (src, dst, label)
            if k not in seen:
                seen.add(k)
                rels.append((src, dst, kind, label))

        for t in ts:
            for f in fks[t]:
                add(f[2], t, "||--o{", f[3])
            for _, name, *_ in cols[t]:
                tgt = infer(t, name)
                if tgt and tgt != t:
                    add(tgt, t, "}o..o{", name)

        # Keep every intra-domain edge; keep outward edges only to the busiest anchors, so a
        # diagram does not turn into a star around `users`.
        ext = defaultdict(int)
        for a, b, _, _ in rels:
            for e in (a, b):
                if e not in tsub:
                    ext[e] += 1
        anchors = {e for e, n in sorted(ext.items(), key=lambda kv: -kv[1])[:4]}
        rels = [r for r in rels if (r[0] in tsub or r[0] in anchors)
                and (r[1] in tsub or r[1] in anchors)]

        cap = 44
        trimmed = max(0, len(rels) - cap)
        rels = rels[:cap]
        if not rels:
            skipped.append(dom)
            er += [f"## {dom}", "", f"*{len(ts)} tables — no join columns detected, so there is "
                   f"nothing to draw. These are standalone or key/value tables.*", "",
                   "**Tables in this domain:** " + ", ".join(f"`{t}`" for t in ts), "", "---", ""]
            continue

        er += [f"## {dom}", "", f"*{len(ts)} tables in this domain*", "", "```mermaid", "erDiagram"]
        for e in sorted({r[0] for r in rels} | {r[1] for r in rels}):
            key_cols = [c[1] for c in cols.get(e, []) if c[5] or c[1].endswith("_id") or c[1] in
                        ("email", "slug", "code", "status", "state", "created_at")][:7]
            er.append(f"    {e} {{")
            for kc in key_cols:
                ct = next((c[2] or "ANY" for c in cols[e] if c[1] == kc), "ANY")
                er.append(f"        {re.sub(r'[^A-Za-z0-9_]', '_', ct.split('(')[0]) or 'ANY'} {kc}")
            er.append("    }")
        for a, b, kind, label in rels:
            er.append(f'    {a} {kind} {b} : "{label}"')
        er += ["```", ""]

        outside = sorted({e for r in rels for e in (r[0], r[1])} - tsub)
        if outside:
            er.append("**Drawn from other domains as anchors:** " + ", ".join(
                f"`{e}` *({dom_of_table.get(e, '?')})*" for e in outside))
            er.append("")
        if trimmed:
            er.append(f"> {trimmed} further relationship(s) omitted for legibility. "
                      f"The complete set is in the column reference.")
            er.append("")
        er.append("**Tables in this domain:** " + ", ".join(f"`{t}`" for t in ts))
        er += ["", "---", ""]

    if skipped:
        er += ["## Domains with nothing to draw", "",
               "Recorded rather than silently omitted:", ""]
        er += [f"- {d}" for d in skipped] + ["", "---", ""]

    Path("docs/PCI_DATABASE_ERD.md").write_text("\n".join(er), encoding="utf-8")
    print(f"ER diagrams: {len(er)} lines, {er.count('```mermaid')} diagrams")


if __name__ == "__main__":
    main()
