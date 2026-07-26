#!/usr/bin/env python3
"""Build a per-volume glossary from the manuscripts' key-terms tables.

The glossary is not a separate document to be kept in step by hand — it is *derived*, so it cannot
drift from the chapters. Each Knowledge Area's `### Key terms — KA N.K` table is the source; this
script consolidates them into one alphabetical glossary per volume with:

  * ONE canonical definition per term — the fullest one, which is almost always the definition given
    by the Knowledge Area that owns the concept rather than one of the later cross-references;
  * every KA that defines the term, so a reader can go to the treatment rather than the gloss;
  * a *context reading* line where a later KA gives the term a materially different sense, which is
    kept rather than discarded because the difference is usually the point.

It also reports two classes of defect, because a glossary is the cheapest place to find them:

  COLLISION  one word, two genuinely different concepts. Per registries/TERMINOLOGY.md these must
             carry an explicit context flag in the manuscript, not be silently merged here.
             A term whose later definitions CITE the owning Knowledge Area, or carry an explicit
             "Context flag:", is reported as CROSS-REFERENCED and is not a defect — following the
             corpus's cite-rather-than-restate rule must not be punished as duplication.
  REDUNDANT  the same term defined twice with substantially the same words, usually in adjacent KAs
             of one domain — repetition the pattern spec treats as a defect.

Usage:  python3 make_glossary.py            # write both volumes' glossaries, report defects
        python3 make_glossary.py --check    # report only; exit 1 if any defect is open
"""
import pathlib
import re
import sys
from difflib import SequenceMatcher

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parent

BOOKS = {
    "pml-ai": "PML-AI Body of Knowledge",
    "pfl-ai": "PFL-AI Body of Knowledge",
}

KEY_TERMS_RE = re.compile(
    r"### Key terms — KA (\d+\.\d+)\s*\n\n\| Term \| Meaning \|\n\|[-| ]+\|\n((?:\|.*\|\n)+)")

# Terms whose repeated definition is deliberate and documented, with the reason. A term listed here is
# reported as RESOLVED rather than as an open defect. Keep this list short and justified — it is an
# exception register, not a suppression list.
ACCEPTED_MULTI = {
    # (book, lowercased term): why more than one definition is correct
    ("pfl-ai", "cfads"): "named in D1, shown to be a *defined* term in D2, and given the facility's "
                         "operative definition in D10 — the progression is the teaching point",
    ("pml-ai", "point of total assumption (pta)"): "derived in D7 and re-read behaviourally in D10; "
                                                  "the D10 entry cites D7 rather than restating it",
}


def strip_md(s: str) -> str:
    return re.sub(r"[`*]", "", s).strip()


def harvest(book: str) -> dict:
    """term_lower -> list of (domain_number, ka, display_term, meaning)."""
    out = {}
    for f in sorted((ROOT / book / "manuscript").glob("domain-*.md")):
        dn = int(re.match(r"domain-(\d+)-", f.name).group(1))
        text = f.read_text(encoding="utf-8")
        for m in KEY_TERMS_RE.finditer(text):
            ka, rows = m.group(1), m.group(2)
            for row in rows.strip().split("\n"):
                cells = [c.strip() for c in row.strip().strip("|").split("|")]
                if len(cells) < 2 or not cells[0]:
                    continue
                term = strip_md(cells[0])
                if not term:
                    continue
                out.setdefault(term.lower(), []).append((dn, ka, term, cells[1].strip()))
    return out


def classify(book: str, key: str, entries: list):
    """Return (kind, note) for a multiply-defined term: RESOLVED, REDUNDANT or COLLISION."""
    if (book, key) in ACCEPTED_MULTI:
        return "RESOLVED", ACCEPTED_MULTI[(book, key)]
    meanings = [e[3] for e in entries]
    # A definition that CITES another Knowledge Area is a deliberate cross-reference, not a duplicate
    # or a collision — "cite rather than re-derive" is the corpus rule, and an entry that follows it
    # must not be reported as a defect for doing so. Likewise an explicit context flag is the
    # documented resolution of a genuine word collision, so it closes the finding rather than raising
    # one. Only the entries doing NEITHER are compared for similarity.
    def is_deliberate(m):
        return bool(re.search(r"\b(?:Domain \d+|KA \d+\.\d+)\b", m)) or "Context flag:" in m
    plain = [m for m in meanings if not is_deliberate(m)]
    if len(plain) < 2:
        return "CROSS-REFERENCED", ("one canonical definition; the others cite the owning KA or carry "
                                   "an explicit context flag")
    best = max(SequenceMatcher(None, a.lower(), b.lower()).ratio()
               for i, a in enumerate(plain) for b in plain[i + 1:])
    same_domain = len({e[0] for e in entries}) == 1
    if best >= 0.72:
        return ("REDUNDANT", f"near-identical wording (similarity {best:.2f})"
                             + (" in one domain" if same_domain else ""))
    if best <= 0.30:
        return "COLLISION", f"materially different senses (similarity {best:.2f})"
    return "LAYERED", f"complementary depth (similarity {best:.2f})"


def canonical(entries: list) -> tuple:
    """The fullest definition wins; ties break to the earliest KA, which usually owns the concept."""
    return max(entries, key=lambda e: (len(e[3]), -e[0]))


def build(book: str) -> tuple:
    terms = harvest(book)
    defects, groups = [], {}
    for key, entries in terms.items():
        if len(entries) > 1:
            kind, note = classify(book, key, entries)
            if kind in ("COLLISION", "REDUNDANT"):
                defects.append((kind, entries[0][2], [e[1] for e in entries], note))
        dn, ka, term, meaning = canonical(entries)
        others = [e for e in entries if (e[1], e[3]) != (ka, meaning)]
        letter = term[0].upper() if term[0].isalpha() else "#"
        groups.setdefault(letter, []).append((term, ka, meaning, others))

    lines = [f"# Glossary — {BOOKS[book]}", "",
             "> **Derived, not maintained.** Every entry below is generated from the",
             "> `### Key terms — KA N.K` tables in the manuscripts by `_build/make_glossary.py`, so the",
             "> glossary cannot drift from the chapters. To change a definition, change it in the",
             "> Knowledge Area that owns it and regenerate. **KA references are the authority** — the",
             "> gloss is a pointer to the treatment, never a substitute for it.", "",
             f"**{len(terms)} terms**, consolidated from "
             f"{sum(len(v) for v in terms.values())} key-terms entries across "
             f"{len({e[0] for v in terms.values() for e in v})} domains.", ""]
    for letter in sorted(groups):
        lines += [f"## {letter}", ""]
        for term, ka, meaning, others in sorted(groups[letter], key=lambda x: x[0].lower()):
            lines.append(f"**{term}** — {meaning} *(KA {ka})*")
            for _, oka, _, omeaning in others:
                lines.append(f"  <br/>· *Also read at KA {oka}:* {omeaning}")
            lines.append("")
    return "\n".join(lines).rstrip() + "\n", defects


def main() -> int:
    check_only = "--check" in sys.argv
    total_defects = 0
    for book in BOOKS:
        text, defects = build(book)
        out = ROOT / book / "GLOSSARY.md"
        if not check_only:
            out.write_text(text, encoding="utf-8")
        # Count real entries, not "\n**" occurrences — the stats line begins that way too and
        # made this report off by one, which is exactly the kind of number a tool must not get
        # wrong while auditing someone else's numbers.
        n_terms = len(re.findall(r"^\*\*(?!\d)(.+?)\*\* — ", text, re.M))
        print(f"{book}: {n_terms} terms -> {out.relative_to(ROOT.parent)}"
              f"{' (not written, --check)' if check_only else ''}")
        for kind, term, kas, note in sorted(defects):
            print(f"  {kind}  '{term}' at KA {', '.join(kas)} — {note}")
            total_defects += 1
    print(f"\nopen glossary defects: {total_defects}")
    if check_only and total_defects:
        print("A COLLISION needs a context flag in the manuscript (registries/TERMINOLOGY.md);")
        print("a REDUNDANT entry needs one of the duplicate definitions removed or differentiated.")
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
