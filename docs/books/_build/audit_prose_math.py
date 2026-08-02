#!/usr/bin/env python3
"""Evaluate every self-contained sum written into the prose.

The formula gate proves the values a worked example *reports*. It does not read the sentences
around them, and those sentences are full of arithmetic: `77,100 − 44,900 = 32,200`, `2/40 = 5.0 %`,
`8.9 × 15 − 60 = 73.5`. Each is a complete claim a reader can check on paper in five seconds, and
each was typed by hand next to a computation rather than produced by one. Prose that summarises
arithmetic — decompositions that must sum, restatements, "which leaves" clauses — is where the
errors in this corpus have actually been found, so it deserves a gate of its own.

Only unambiguous claims are judged. A span is evaluated when both sides are pure arithmetic over
digits and operators; anything carrying a symbol (`PI = 1`, `r = 8 %`, `NPV = 0`) is a definition
or an assignment, not a sum, and is counted as skipped rather than guessed at. The skip count is
printed, because a checker that silently evaluates nothing also reports no failures.

Tolerance is interval arithmetic over the printed precision: every literal stands for the range it
could have been rounded from, and a claim fails only when its stated result is outside the range its
inputs can reach. That is what makes the gate usable — without it, correct rounding of a displayed
intermediate reads as forty defects.

The cost is real and worth stating: precision buys power. A claim over inputs printed to six
decimals is pinned to within a unit or two, while `8.9 x 15 = 133.4` cannot be faulted at all,
because 8.9 standing for anything in [8.85, 8.95] reaches [132.75, 134.25]. This gate therefore
catches transcription and derivation errors, not errors small enough to hide inside the author's own
rounding. Those need a golden answer in checks/, which is where the reported values already live.

Run:  python3 audit_prose_math.py             (exits 1 on any disagreement)
      python3 audit_prose_math.py --verbose   (lists every claim evaluated)
      python3 audit_prose_math.py --selftest  (plants wrong sums and proves they are caught)
"""
from __future__ import annotations

import decimal
import pathlib
import re
import subprocess
import sys
from decimal import Decimal as D

decimal.getcontext().prec = 40

ROOT = pathlib.Path(__file__).resolve().parent
BOOKS = ROOT.parent
BOOK_DIRS = {"pml-ai": BOOKS / "pml-ai", "pfl-ai": BOOKS / "pfl-ai"}

CODE_RE = re.compile(r"`([^`\n]{3,160})`")
# Everything an arithmetic claim may contain. Anything else makes the span symbolic.
ALLOWED = set("0123456789 .,()+-*/^%×÷−–—")
NUM_RE = re.compile(r"\d[\d,]*(?:\.\d+)?")


def normalise(side: str) -> str:
    """Typographic operators to Python ones, thousands separators away."""
    return (side.replace("×", "*").replace("÷", "/")
            .replace("−", "-").replace("–", "-").replace("—", "-")
            .replace("^", "**").replace(",", "").strip())


def decimals(side: str) -> int:
    """How precisely the claim is written, so it can be judged to its own precision."""
    return max((len(m.split(".")[1]) for m in NUM_RE.findall(side) if "." in m), default=0)


def evaluate(expr: str) -> D | None:
    """Evaluate a pure-arithmetic string in exact decimal, or None if it will not evaluate."""
    if not expr or not set(expr) <= ALLOWED:
        return None
    if not any(c.isdigit() for c in expr):
        return None
    src = normalise(expr)
    if not src or set(src) & set("%"):
        return None
    # A bare number is a value, not a claim worth re-deriving; require an operator.
    if not any(op in src for op in "+-*/"):
        return None
    try:
        # Literal-only arithmetic: parse to a Decimal tree rather than eval() on book text.
        node = compile(src, "<claim>", "eval")
        return _walk(node.co_consts, src)
    except (SyntaxError, ValueError, ZeroDivisionError, decimal.InvalidOperation):
        return None


def _walk(_consts, src: str) -> D | None:
    """Evaluate `src` over Decimal by rewriting every literal, with no names in scope."""
    py = re.sub(r"(?<![\w.])(\d+(?:\.\d+)?)", r'D("\1")', src)
    try:
        return eval(py, {"__builtins__": {}, "D": D}, {})   # noqa: S307 - literals only, no names
    except Exception:
        return None


def interval(expr: str) -> tuple[D, D] | None:
    """The range a claim can honestly evaluate to, given how precisely its inputs are printed.

    This is what makes the gate usable rather than merely noisy. A book states `42,000,000 /
    8.383844 = 5,009,635.23`: computed from the printed six-decimal factor that is 5,009,635.20,
    but the author divided by the full-precision annuity factor, and .23 is right. Both readings
    are defensible from the page, so the honest test is not "does the stated result equal the
    result computed from rounded inputs" — it is "is the stated result REACHABLE from inputs that
    would print this way". Every literal printed to d decimals stands for a value within half a
    unit in the last place; evaluating the expression at every combination of those bounds gives
    the reachable range. Only a result outside it is a defect.

    Judging without this reported forty-odd findings, essentially all of them the author correctly
    carrying full precision through a displayed intermediate.
    """
    if not expr or not set(expr) <= ALLOWED:
        return None
    src = normalise(expr)
    if not src or not any(c.isdigit() for c in src):
        return None
    lits = list(re.finditer(r"(?<![\w.])(\d+(?:\.\d+)?)", src))
    if not lits or len(lits) > 8:
        return None
    bounds = []
    for m in lits:
        txt = m.group(1)
        dp = len(txt.split(".")[1]) if "." in txt else 0
        half = D(1).scaleb(-dp) / 2 if dp else D(0)
        v = D(txt)
        bounds.append((v - half, v + half) if half else (v, v))
    lo = hi = None
    for mask in range(1 << len(lits)):
        out, prev = [], 0
        for i, m in enumerate(lits):
            out.append(src[prev:m.start()])
            out.append(f'D("{bounds[i][1 if mask >> i & 1 else 0]}")')
            prev = m.end()
        out.append(src[prev:])
        try:
            v = eval("".join(out), {"__builtins__": {}, "D": D}, {})  # noqa: S307
        except Exception:
            continue
        v = D(v)
        lo = v if lo is None or v < lo else lo
        hi = v if hi is None or v > hi else hi
    return None if lo is None else (lo, hi)


def claims(text: str):
    """Every `a op b = c` span in a markdown body, with its line number."""
    for i, line in enumerate(text.splitlines(), 1):
        for m in CODE_RE.finditer(line):
            span = m.group(1)
            if span.count("=") != 1 or "==" in span or "≠" in span:
                continue
            lhs, rhs = span.split("=")
            yield i, span, lhs, rhs


def main() -> int:
    verbose = "--verbose" in sys.argv
    checked = skipped = 0
    wrong: list[str] = []

    for book, bdir in BOOK_DIRS.items():
        for md in sorted((bdir / "manuscript").glob("*.md")) + [
                bdir / "CAPSTONES.md", bdir / "APPENDICES.md"]:
            if not md.exists():
                continue
            for line, span, lhs, rhs in claims(md.read_text()):
                # Percentages: judge only when both sides agree on the unit, so that a "%" on one
                # side alone is skipped rather than being silently scaled by a hundred.
                pct = lhs.count("%"), rhs.count("%")
                l_txt, r_txt = lhs.replace("%", ""), rhs.replace("%", "")
                scale = D(1)
                if pct[1] and not pct[0]:
                    # `2,709,000/42,000,000 = 6.45 %` states a ratio and reports it as a
                    # percentage. Judging 0.0645 against 6.45 makes every such claim a failure,
                    # which is a unit error in the checker, not in the book.
                    scale = D(100)
                elif pct[0] and not pct[1]:
                    skipped += 1
                    continue
                left = evaluate(l_txt)
                rng = interval(l_txt)
                right = evaluate(r_txt)
                if right is None and set(r_txt.strip()) <= ALLOWED and any(
                        c.isdigit() for c in r_txt):
                    try:
                        right = D(normalise(r_txt))
                    except decimal.InvalidOperation:
                        right = None
                if left is None or right is None:
                    skipped += 1
                    continue
                # Judge to the precision the RESULT is written to. A claim reporting
                # `1.20 x 5,009,635.23 = 6,011,562` has rounded to whole units on purpose;
                # demanding two decimals because an input carries two turns correct rounding into
                # a defect, and that mistake alone accounted for most of a first run's findings.
                dp = decimals(r_txt)
                q = D(1).scaleb(-dp)
                stated = right.quantize(q, rounding=decimal.ROUND_HALF_UP)
                # Percentages in this corpus are written both ways — a ratio reported as a
                # percentage, and a sum of figures already in percent. Accept either reading
                # rather than guessing, and report only a claim that fails under both.
                readings = [D(1), scale] if scale != 1 else [D(1)]
                ok = False
                for f in readings:
                    lo, hi = (rng[0] * f, rng[1] * f) if rng else (left * f, left * f)
                    if (lo - q / 2) <= stated <= (hi + q / 2):
                        ok = True
                        break
                checked += 1
                if not ok:
                    lo, hi = (rng if rng else (left, left))
                    wrong.append(f"{book}/{md.name}:{line}  `{span}`  "
                                 f"reachable range [{lo.quantize(q)}, {hi.quantize(q)}], "
                                 f"stated {stated}")
                elif verbose:
                    print(f"  ok  {book}/{md.name}:{line}  `{span}`")

    print(f"\ninline arithmetic claims evaluated: {checked}   "
          f"skipped as symbolic or unit-mixed: {skipped}")
    if wrong:
        print(f"\nopen prose-arithmetic defects: {len(wrong)}")
        for w in wrong:
            print(f"  - {w}")
        return 1
    print("open prose-arithmetic defects: 0")
    return 0


def selftest() -> int:
    """Plant wrong sums in a real manuscript and confirm each is caught."""
    bdir = BOOK_DIRS["pfl-ai"]
    md = bdir / "manuscript" / "domain-04-investment-appraisal.md"
    original = md.read_text()
    plants = [
        ("a wrong subtraction", "\n\nPlanted: `77,100 - 44,900 = 32,201` closes.\n"),
        # `8.9 * 15 = 133.4` is deliberately NOT used here: with 8.9 standing for anything in
        # [8.85, 8.95] the reachable range is [132.75, 134.25], so 133.4 is reachable and accepting
        # it is correct. That is the cost of interval arithmetic — see the note in the docstring.
        ("a wrong product", "\n\nPlanted: `8.9 * 15 = 140.0` undiscounted.\n"),
        ("a wrong division", "\n\nPlanted: `2 / 40 = 0.06` close rate.\n"),
    ]
    failures = 0
    try:
        for name, text in plants:
            md.write_text(original + text)
            out = subprocess.run([sys.executable, __file__], capture_output=True, text=True).stdout
            caught = "open prose-arithmetic defects: 1" in out
            print(f"{'PASS' if caught else 'FAIL'}  planting {name} is caught")
            failures += 0 if caught else 1
    finally:
        md.write_text(original)
    out = subprocess.run([sys.executable, __file__], capture_output=True, text=True).stdout
    clean = "open prose-arithmetic defects: 0" in out
    print(f"{'PASS' if clean else 'FAIL'}  removing the plants restores a clean run")
    failures += 0 if clean else 1
    print("\nself-test failures:", failures)
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(selftest() if "--selftest" in sys.argv else main())
