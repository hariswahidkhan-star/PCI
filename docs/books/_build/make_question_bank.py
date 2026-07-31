#!/usr/bin/env python3
"""Build each volume's consolidated question bank from the in-domain MCQs, and audit them.

Like the glossary and the appendices, the bank is **derived**: it consolidates the `**MCQ N.K-X`
items already embedded in the Knowledge Areas rather than duplicating them into a separate file that
would drift. What it adds beyond convenience is the two things a scattered set of questions cannot
give you:

  * **Blueprint coverage** — questions per domain and per cognitive level, against the shape a
    candidate is actually assessed on. A bank with 40 Application items and no Evaluation items is
    not a bank, it is a drill sheet, and only a consolidated view shows that.
  * **An integrity audit** — every structural defect that makes a question unusable, reported rather
    than shipped. These are the checks a human reviewer would otherwise have to do 530 times.

Audited defects:

  NO_KEY        no option marked correct — unusable
  MULTI_KEY     more than one option marked correct — unusable, and the commonest serious defect
  NO_RATIONALE  no rationale, so a candidate who gets it wrong learns nothing
  FEW_OPTIONS   fewer than four options
  NO_TAG        no `[topic · level]` tag, so it cannot be mapped to a blueprint
  BAD_LEVEL     a cognitive level outside the agreed set
  STEM_ONLY     a stem with no question mark and no imperative — usually a truncated item

Usage:  python3 make_question_bank.py            # write both banks, report the audit
        python3 make_question_bank.py --check    # audit only; exit 1 if any defect is open
"""
import pathlib
import re
import sys
from collections import Counter, defaultdict

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parent

BOOKS = {"pml-ai": "PML-AI Body of Knowledge", "pfl-ai": "PFL-AI Body of Knowledge"}

# The vocabulary the corpus actually uses, in ascending cognitive demand. Taken from the
# manuscripts rather than assumed: an earlier version listed "Knowledge" and "Synthesis",
# neither of which appears anywhere, and flagged all 23 real "Recall" items as defective.
LEVELS = ["Recall", "Comprehension", "Application", "Analysis", "Evaluation"]

# An MCQ block: the bold header with its tag, the stem, the options, and the rationale that follows.
MCQ_RE = re.compile(
    r"^\*\*MCQ (?P<id>[\d]+\.[\d]+-[A-Z]+)\s*`\[(?P<tag>[^`\]]+)\]`\*\*\s*(?P<stem>[\s\S]*?)\n"
    r"(?P<options>(?:^- [\s\S]*?)(?=\n\s*\n|\n\*Rationale))"
    r"(?:\s*\n\*Rationale:\*(?P<rationale>[\s\S]*?)(?=\n\n|\Z))?",
    re.M)


def parse(book: str) -> list:
    items = []
    for f in sorted((ROOT / book / "manuscript").glob("domain-*.md")):
        dn = int(re.match(r"domain-(\d+)-", f.name).group(1))
        text = f.read_text(encoding="utf-8")
        for m in MCQ_RE.finditer(text):
            # Re-join continuation lines: an option runs until the next line beginning "- ".
            raw, opts = m.group("options").strip().split("\n"), []
            for line in raw:
                if line.startswith("- "):
                    opts.append(line[2:].strip())
                elif opts:
                    opts[-1] += " " + line.strip()
            tag = m.group("tag")
            topic, level = "", ""
            if "·" in tag:
                topic, level = [p.strip() for p in tag.split("·", 1)]
            items.append({
                "domain": dn, "id": m.group("id"), "topic": topic, "level": level,
                "stem": " ".join(m.group("stem").split()),
                "options": opts,
                "keys": [o for o in opts if "✅" in o],
                "rationale": " ".join((m.group("rationale") or "").split()),
                "file": f.name,
            })
    return items


def audit(items: list) -> list:
    out = []
    for it in items:
        where = f"D{it['domain']} MCQ {it['id']}"
        if not it["keys"]:
            out.append(("NO_KEY", where, "no option marked correct"))
        elif len(it["keys"]) > 1:
            out.append(("MULTI_KEY", where, f"{len(it['keys'])} options marked correct"))
        if not it["rationale"]:
            out.append(("NO_RATIONALE", where, "no rationale — a wrong answer teaches nothing"))
        if len(it["options"]) < 4:
            out.append(("FEW_OPTIONS", where, f"only {len(it['options'])} options"))
        if not it["topic"] or not it["level"]:
            out.append(("NO_TAG", where, f"tag not parseable: '{it['topic']} · {it['level']}'"))
        elif it["level"] not in LEVELS:
            out.append(("BAD_LEVEL", where, f"level '{it['level']}' is outside the agreed set"))
        # The imperative list is read off the corpus (the first word of each stem's closing
        # sentence), not asserted. An earlier version omitted "assess" and condemned three sound
        # Evaluation items — the same mistake as asserting a cognitive-level vocabulary instead of
        # reading one. A stem still counts as asking something if it ends in a colon, since the
        # options complete the sentence.
        if it["stem"] and not re.search(
                r"[?:]|\b(?:which|what|why|how|state|compute|identify|assess|evaluate|judge|"
                r"recommend|rank|select|choose|determine|explain|justify|calculate|find)\b",
                it["stem"], re.I):
            out.append(("STEM_ONLY", where, "stem asks nothing — possibly truncated"))
    return out


def build(book: str, items: list) -> str:
    by_domain = defaultdict(list)
    for it in items:
        by_domain[it["domain"]].append(it)
    levels = Counter(it["level"] for it in items if it["level"] in LEVELS)
    total = len(items)

    L = [f"# Question bank — {BOOKS[book]}", "",
         "> **Derived, not duplicated.** Every item is the question as it appears in its Knowledge",
         "> Area, consolidated here by `_build/make_question_bank.py`. Answer keys and rationales are",
         "> the chapters' own. To change an item, change it in its Knowledge Area and regenerate —",
         "> which is why there is no second copy to fall out of step.", "",
         f"**{total} items** across {len(by_domain)} domains. Every numeric option in every item is",
         "independently recomputed by the golden-answer suite, not only the correct one, so a",
         "distractor cannot be arithmetically impossible without the gate failing.", "",
         "## Coverage by cognitive level", "",
         "| Level | Items | Share |", "|---|---|---|"]
    for lv in LEVELS:
        n = levels.get(lv, 0)
        if n:
            L.append(f"| {lv} | {n} | {n / total * 100:.1f} % |")
    L += ["", "A bank weighted heavily to recall tests memory rather than competence; one weighted",
          "heavily to Evaluation is unanswerable under time pressure. The distribution above is a fact",
          "to be reviewed against the examination blueprint, not a claim that it is correctly balanced —",
          "the blueprint weightings are an open decision (see `CORPUS_GATE_REPORT.md` §9).", "",
          "## Coverage by domain", "",
          "| Domain | Items | Levels represented |", "|---|---|---|"]
    for dn in sorted(by_domain):
        got = sorted({it["level"] for it in by_domain[dn] if it["level"] in LEVELS},
                     key=LEVELS.index)
        L.append(f"| {dn} | {len(by_domain[dn])} | {', '.join(got) or '—'} |")
    L += ["", "---", ""]

    for dn in sorted(by_domain):
        L += [f"## Domain {dn}", ""]
        for it in sorted(by_domain[dn], key=lambda x: x["id"]):
            L.append(f"**{it['id']}** `[{it['topic']} · {it['level']}]` {it['stem']}")
            L.append("")
            for o in it["options"]:
                L.append(f"- {o}")
            if it["rationale"]:
                L += ["", f"*Rationale:* {it['rationale']}"]
            L += ["", ""]
    return "\n".join(L).rstrip() + "\n"


def main() -> int:
    check_only = "--check" in sys.argv
    defects_total = 0
    for book in BOOKS:
        items = parse(book)
        defects = audit(items)
        if not check_only:
            (ROOT / book / "QUESTION_BANK.md").write_text(build(book, items), encoding="utf-8")
        kinds = Counter(d[0] for d in defects)
        print(f"{book}: {len(items)} items"
              + (f" -> {book}/QUESTION_BANK.md" if not check_only else " (audit only)"))
        for lv in LEVELS:
            n = sum(1 for it in items if it["level"] == lv)
            if n:
                print(f"    {lv:<14} {n:>4}  {n / len(items) * 100:5.1f} %")
        if defects:
            print(f"    defects: {dict(kinds)}")
            for kind, where, note in defects[:40]:
                print(f"      {kind}  {where} — {note}")
            if len(defects) > 40:
                print(f"      ... and {len(defects) - 40} more")
        defects_total += len(defects)
    print(f"\nopen question-bank defects: {defects_total}")
    return 1 if (check_only and defects_total) else 0


if __name__ == "__main__":
    raise SystemExit(main())
