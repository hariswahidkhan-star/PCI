#!/usr/bin/env python3
"""Fail the run if any published text names an external certifying body or credential.

The rule this enforces is the one thing the comparison content cannot get wrong, and asking
fifteen writers to each remember it is not a control. This is.

Scope note: it checks the *publishable* regions only. The framework and task files name bodies
in order to prohibit them, which is correct, and an internal-linking note that says "do not
link to any awarding body" is not a naming. Front matter is checked because a keyword field or
a meta description ships to a search engine exactly like body text does.
"""
import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent

# Acronyms are matched with word boundaries and case-sensitively where a lowercase word would
# collide: "CIA" must not fire on "special", "CPA" is safe, "APM" must not fire on "apm".
ACRONYMS = ["PMI", "PMP", "CAPM", "PMI-SP", "PMI-RMP", "PMI-ACP", "AACE", "CCP", "PSP",
            "EVP", "CEP", "DRMP", "CFA", "CMA", "ACCA", "CIMA", "CPA", "CIA", "CISA",
            "CTP", "PRINCE2", "IPMA", "RICS", "ChPP", "MRICS", "CEng", "AXELOS", "ISACA",
            "IIA", "AICPA", "AFP", "PeopleCert", "CSM", "CSPO", "SAFe", "ITIL"]
NAMES = ["Project Management Institute", "AACE International", "Institute of Internal Auditors",
         "CFA Institute", "Institute of Management Accountants", "Association for Project Management",
         "Chartered Institute of Management Accountants", "Primavera", "Microsoft Project",
         "Association of Chartered Certified Accountants", "Royal Institution of Chartered Surveyors",
         "Scrum Alliance", "Axelos", "Oracle"]
# Naming by description resolves to one credential just as fast as the acronym does.
DESCRIPTIVE = [
    r"the (?:main|leading|largest|best[- ]known|most (?:recognised|recognized|popular)) "
    r"(?:American |US |UK |global |international )?(?:cost engineering|project management|"
    r"scheduling|accountancy|audit) (?:body|institute|organisation|organization|certification)",
    r"the four[- ]letter (?:scheduling|cost|project) credential",
]

FM_RE = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)


def offences(text):
    hits = []
    for a in ACRONYMS:
        for m in re.finditer(rf"(?<![A-Za-z0-9-]){re.escape(a)}(?![A-Za-z0-9-])", text):
            hits.append((a, text[max(0, m.start() - 45):m.end() + 45].replace("\n", " ")))
    for n in NAMES:
        for m in re.finditer(re.escape(n), text, re.I):
            hits.append((n, text[max(0, m.start() - 45):m.end() + 45].replace("\n", " ")))
    for d in DESCRIPTIVE:
        for m in re.finditer(d, text, re.I):
            hits.append(("(naming by description)", m.group(0)))
    return hits


def main():
    targets = sys.argv[1:] or ["comparisons"]
    rows, bad = [], 0
    for t in targets:
        d = ROOT / t
        if not d.exists():
            continue
        for f in sorted(d.glob("*.md")):
            if f.name.startswith("_"):
                continue
            text = f.read_text(encoding="utf-8")
            hits = offences(text)      # front matter included, deliberately
            rows.append({"file": str(f.relative_to(ROOT)), "offences": hits})
            if hits:
                bad += 1
                print(f"  {f.relative_to(ROOT)}")
                for what, ctx in hits[:6]:
                    print(f"      {what}: …{ctx.strip()}…")
    print(f"\nchecked {len(rows)} files | clean {len(rows)-bad} | naming an external body {bad}")
    (ROOT / "_tools" / "no_named_bodies.json").write_text(json.dumps(rows, indent=1))
    return 1 if bad else 0


if __name__ == "__main__":
    sys.exit(main())
