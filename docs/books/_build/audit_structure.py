#!/usr/bin/env python3
"""Audit the structure a reader navigates: question keys, cross-references, and column totals.

Three things nothing else in the pipeline reads:

  1. Multiple-choice integrity. Exactly one option marked correct, four distinct options, a rationale
     present, and no option repeated. A question with two ticks or none is unusable, and a duplicated
     option makes a keyed answer indefensible.

  2. Cross-references that resolve. The corpus points at itself constantly — `Domain 12`, `KA 10.2.1`,
     `WE 4.1.1`, `EX 8.2`, `Fig 3.2.3`, `Appendix F`, `MCQ 6.2-E`. Each pointer is a promise, and a
     reader who follows one to nothing loses confidence in every other. These are checked by
     existence: a referenced worked example, exercise, figure, appendix or question must be one the
     corpus actually contains.

  3. Column totals. A table row labelled Total must equal the sum of the numeric column above it.
     A total that does not add is the most visible arithmetic defect a book can print, and it is the
     one a reader checks first.

Run:  python3 audit_structure.py             (exits 1 on any defect)
      python3 audit_structure.py --selftest  (plants each defect class and proves it is caught)
"""
from __future__ import annotations

import decimal
import pathlib
import re
import subprocess
import sys
from decimal import Decimal as D

ROOT = pathlib.Path(__file__).resolve().parent
BOOKS = ROOT.parent
BOOK_DIRS = {"pml-ai": BOOKS / "pml-ai", "pfl-ai": BOOKS / "pfl-ai"}

MCQ_RE = re.compile(r"^\*\*MCQ\s+(\d+\.\d+-[A-Z])\s*`\[([^\]]*)\]`\*\*", re.M)
OPT_RE = re.compile(r"^-\s+([A-D])\.\s+(.*)$")
TICK = "✅"

# What the prose points at.
REFS = {
    "WE": re.compile(r"\bWE\s+(\d+\.\d+\.\d+[a-z]?)\b"),
    "EX": re.compile(r"\bEX\s+(\d+\.\d+)\b"),
    "Fig": re.compile(r"\bFig\s+(\d+\.\d+\.\d+)\b"),
    "KA": re.compile(r"\bKA\s+(\d+\.[0-9A-Z]+(?:\.\d+)?)\b"),
    "MCQ": re.compile(r"\bMCQ\s+(\d+\.\d+-[A-Z])\b"),
    "Appendix": re.compile(r"\bAppendix\s+([A-Z])\b"),
}
# What the corpus defines.
DEFS = {
    # Worked examples are headed "**Worked example 4.2.3 — ...**" and referred to as "WE 4.2.3".
    # Matching the reference form as the definition form found nothing and reported every worked
    # example in both books as a dangling pointer.
    "WE": re.compile(r"\*\*(?:WE|Worked example)\s+(\d+\.\d+\.\d+[a-z]?)"),
    "EX": re.compile(r"\*\*(?:EX|Exercise)\s+(\d+\.\d+)"),
    "Fig": re.compile(r"\*\*Fig\s+(\d+\.\d+\.\d+)"),
    "MCQ": re.compile(r"\*\*MCQ\s+(\d+\.\d+-[A-Z])"),
}

NUM_CELL = re.compile(r"^[\s*`+−-]*(\d[\d,]*(?:\.\d+)?)[\s*`%]*$")


def cell_value(cell: str) -> D | None:
    m = NUM_CELL.match(cell.strip())
    if not m:
        return None
    try:
        return D(m.group(1).replace(",", ""))
    except decimal.InvalidOperation:
        return None


def audit_mcqs(body: str, where: str) -> list[str]:
    out, lines = [], body.splitlines()
    starts = [(m.group(1), body[: m.start()].count("\n")) for m in MCQ_RE.finditer(body)]
    for qid, start in starts:
        opts: dict[str, str] = {}
        rationale = False
        for ln in lines[start:start + 60]:
            if ln.startswith("*Rationale:*"):
                rationale = True
                break
            m = OPT_RE.match(ln)
            if m:
                opts[m.group(1)] = m.group(2)
            elif opts and ln.startswith(("  ", "\t")) and opts:
                # A wrapped option line continues the previous option.
                last = sorted(opts)[-1]
                opts[last] += " " + ln.strip()
        if len(opts) != 4:
            out.append(f"{where} MCQ {qid} has {len(opts)} options, not four")
            continue
        ticks = [k for k, v in opts.items() if TICK in v]
        if len(ticks) != 1:
            out.append(f"{where} MCQ {qid} marks {len(ticks)} options correct, not one")
        if not rationale:
            out.append(f"{where} MCQ {qid} has no *Rationale:*")
        bare = {k: re.sub(r"[\s✅*`]", "", v).lower() for k, v in opts.items()}
        for a in "ABCD":
            for b in "ABCD":
                if a < b and bare.get(a) and bare.get(a) == bare.get(b):
                    out.append(f"{where} MCQ {qid} options {a} and {b} are identical")
    return out


def audit_totals(body: str, where: str) -> list[str]:
    """A row labelled Total must equal the column above it — where it really is a column sum.

    "Total" is doing two different jobs in these books. A bare `**Total**` row is an addition and
    must add. But `**Total float `TF`**`, `**Total cost of ownership**` and `Total working-capital
    tolerance from 6,984,000` are NAMED QUANTITIES that merely begin with the word, and summing the
    column above them is meaningless — treating them as additions produced thirteen findings, none
    of which was an error in the book.

    So: a bare total must add exactly. A named total is only reported when it NEARLY adds — within
    two per cent but not equal — because that is the signature of a slip in a real addition, whereas
    an unrelated derived quantity lands nowhere near the column sum.
    """
    out = []
    rows: list[list[str]] = []
    for raw in body.splitlines() + [""]:
        line = raw.strip()
        if line.startswith("|") and line.endswith("|"):
            rows.append([c.strip() for c in line.strip("|").split("|")])
            continue
        if rows:
            # A table has ended: look for a total row.
            for ri, row in enumerate(rows):
                label = re.sub(r"[\s*`]", "", row[0]).lower() if row else ""
                if not label.startswith("total") or ri == 0:
                    continue
                bare = label.rstrip(":") in ("total", "totals")
                for ci in range(1, len(row)):
                    stated = cell_value(row[ci]) if ci < len(row) else None
                    if stated is None:
                        continue
                    above = [cell_value(r[ci]) for r in rows[1:ri]
                             if len(r) > ci and not re.match(r"^[-: ]+$", r[ci])]
                    vals = [v for v in above if v is not None]
                    if len(vals) < 2:
                        continue
                    got = sum(vals)
                    # Judge to the stated row's own precision; components are often rounded.
                    dp = len(str(stated).split(".")[1]) if "." in str(stated) else 0
                    q = D(1).scaleb(-dp)
                    slack = q * len(vals) / 2 + q / 2
                    if abs(got - stated) <= slack:
                        continue
                    near = abs(got - stated) <= abs(stated) * D("0.02")
                    if bare or near:
                        out.append(f"{where} the {'bare ' if bare else ''}Total row "
                                   f"{row[0][:44]!r} states {stated} in column {ci + 1} but the "
                                   f"{len(vals)} values above it sum to {got}")
            rows = []
    return out


def corpus_of(bdir: pathlib.Path) -> str:
    files = sorted((bdir / "manuscript").glob("*.md")) + [
        bdir / "CAPSTONES.md", bdir / "APPENDICES.md", bdir / "STANDARDS.md"]
    return "\n".join(f.read_text() for f in files if f.exists())


def main() -> int:
    defects: list[str] = []
    counts = {"mcq": 0, "refs": 0, "tables": 0}
    all_corpora = {b: corpus_of(d) for b, d in BOOK_DIRS.items()}

    for book, bdir in BOOK_DIRS.items():
        # STANDARDS.md carries Appendix F and GLOSSARY/QUESTION_BANK are derived views; the
        # appendix check needs F in the corpus or it reports the register as missing.
        files = sorted((bdir / "manuscript").glob("*.md")) + [
            bdir / "CAPSTONES.md", bdir / "APPENDICES.md", bdir / "STANDARDS.md"]
        corpus = "\n".join(f.read_text() for f in files if f.exists())
        defined = {k: set(rx.findall(corpus)) for k, rx in DEFS.items()}
        counts["mcq"] += len(defined["MCQ"])
        appendices = set(re.findall(r"^#+\s*Appendix\s+([A-Z])", corpus, re.M))

        for md in files:
            if not md.exists():
                continue
            body = md.read_text()
            where = f"{book}/{md.name}"
            defects += audit_mcqs(body, where)
            defects += audit_totals(body, where)
            counts["tables"] += body.count("\n|")
            for kind, rx in REFS.items():
                for target in set(rx.findall(body)):
                    counts["refs"] += 1
                    if kind == "Appendix":
                        if appendices and target not in appendices:
                            defects.append(f"{where} points at Appendix {target}, which the corpus "
                                           f"does not contain (has {sorted(appendices)})")
                    elif kind == "KA":
                        # Knowledge areas are heading-numbered. The books cite each other by name
                        # ("PML-AI D8, KA 8.2.4"), so a pointer resolves against EITHER corpus —
                        # searching only the citing book reported two valid cross-book references
                        # as dangling.
                        if not any(re.search(rf"^#+\s*(?:KA\s*)?{re.escape(target)}\b", c, re.M)
                                   for c in all_corpora.values()):
                            defects.append(f"{where} points at KA {target}, which is not a heading "
                                           f"in either volume")
                    elif target not in defined[kind]:
                        defects.append(f"{where} points at {kind} {target}, which the corpus does "
                                       f"not define")

    print(f"\nquestions audited: {counts['mcq']}   cross-references resolved: {counts['refs']}")
    if defects:
        print(f"\nopen structural defects: {len(defects)}")
        for d in sorted(set(defects))[:60]:
            print(f"  - {d}")
        if len(set(defects)) > 60:
            print(f"  ... and {len(set(defects)) - 60} more")
        return 1
    print("open structural defects: 0")
    return 0


def selftest() -> int:
    md = BOOK_DIRS["pfl-ai"] / "manuscript" / "domain-04-investment-appraisal.md"
    original = md.read_text()
    plants = [
        ("a question with two keys",
         "\n\n**MCQ 4.9-Z `[4.1.1 · Recall]`** Planted.\n- A. one ✅\n- B. two ✅\n- C. three\n"
         "- D. four\n\n*Rationale:* planted.\n", "marks 2 options correct"),
        ("a question with no rationale",
         "\n\n**MCQ 4.9-Y `[4.1.1 · Recall]`** Planted.\n- A. one ✅\n- B. two\n- C. three\n"
         "- D. four\n\nNot a rationale line.\n", "has no *Rationale:*"),
        ("a reference to a figure that does not exist",
         "\n\nSee Fig 4.9.9 for the planted case.\n", "points at Fig 4.9.9"),
        ("a total that does not add",
         "\n\n| Item | Amount |\n|---|---|\n| A | 100 |\n| B | 200 |\n| **Total** | **310** |\n",
         "sum to 300"),
    ]
    failures = 0
    try:
        for name, text, expect in plants:
            md.write_text(original + text)
            out = subprocess.run([sys.executable, __file__], capture_output=True, text=True).stdout
            caught = expect in out
            print(f"{'PASS' if caught else 'FAIL'}  planting {name} is caught")
            failures += 0 if caught else 1
    finally:
        md.write_text(original)
    out = subprocess.run([sys.executable, __file__], capture_output=True, text=True).stdout
    clean = "open structural defects: 0" in out
    print(f"{'PASS' if clean else 'FAIL'}  removing the plants restores a clean run")
    failures += 0 if clean else 1
    print("\nself-test failures:", failures)
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(selftest() if "--selftest" in sys.argv else main())
