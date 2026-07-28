#!/usr/bin/env python3
"""Run ONE per-domain check module in isolation.

`verify_formulas.py` loads every module in `checks/`, which is right for the gate and wrong while
several domains are being authored at once: a verifier running the full suite would see another
domain's half-written module and report a failure that is not its own. This runner loads exactly the
module named, with the same ctx the full suite supplies, so a domain can be verified independently.

Usage:  python3 run_checks.py checks/pfl_d05.py
        python3 run_checks.py pfl_d05
Exit 0 = every check in that module passes. Exit 1 = at least one failed, or the module raised.
"""
import importlib.util
import pathlib
import sys
from decimal import Decimal as D, getcontext

getcontext().prec = 28
HERE = pathlib.Path(__file__).resolve().parent
FAILURES = []


def check(name, computed, printed, tol=D("0.005")):
    computed, printed = D(str(computed)), D(str(printed))
    ok = abs(computed - printed) <= tol
    print(f"{'PASS' if ok else 'FAIL'}  {name}: computed {computed}  printed {printed}")
    if not ok:
        FAILURES.append(name)


def af(r, n):                      # ordinary annuity factor
    return (1 - (1 + r) ** -n) / r


def ew(M, L):                      # governance latency (PML-AI D3)
    return D(M) / 2 + D(L)


def mesh(n):                       # pairwise interfaces among n components (PML-AI D4)
    return n * (n - 1) // 2


# Master-thread constants — identical to verify_formulas.py's CTX. A value shared between domains is
# read from here, never re-typed into a manuscript-specific literal.
CTX = {
    "check": check, "D": D, "af": af, "ew": ew, "mesh": mesh,
    "MERIDIAN_COST_OF_DELAY": D(14280), "MERIDIAN_BASELINE": D(2400000),
    "MERIDIAN_BENEFIT_70PCT": D(685440), "MERIDIAN_BENEFIT_FULL": D(979200),
    "AURIGA_BAC": D(4000000), "AURIGA_WEEKS": 25,
    "KESTREL_DEBT": D(42000000), "KESTREL_EQUITY": D(18000000), "KESTREL_CAPEX": D(60000000),
    "KESTREL_RATE": D("0.06"), "KESTREL_TENOR": 12, "KESTREL_LIFE": 25,
    "KESTREL_INSTALMENT": D("5009635.23"), "KESTREL_CFADS": D(6384000),
    "KESTREL_EBITDA": D(7500000), "KESTREL_EBIT": D(5100000), "KESTREL_INTEREST_Y1": D(2520000),
    "KESTREL_DISCOUNT": D("0.08"), "KESTREL_NPV": D(16179360),
}


def main() -> int:
    if len(sys.argv) != 2:
        print(__doc__)
        return 2
    arg = sys.argv[1]
    path = pathlib.Path(arg)
    if not path.suffix:
        path = HERE / "checks" / f"{arg}.py"
    if not path.is_absolute():
        path = (HERE / path) if not path.exists() else path
    if not path.exists():
        print(f"FAIL  no such check module: {path}")
        return 1

    spec = importlib.util.spec_from_file_location(f"pci_one_{path.stem}", path)
    mod = importlib.util.module_from_spec(spec)
    try:
        spec.loader.exec_module(mod)
    except Exception as e:
        print(f"FAIL  {path.name} failed to import: {type(e).__name__}: {e}")
        return 1
    if not hasattr(mod, "run"):
        print(f"FAIL  {path.name} exposes no run(ctx) — see checks/_README.md")
        return 1
    try:
        mod.run(CTX)
    except Exception as e:
        print(f"FAIL  {path.name}.run(ctx) raised {type(e).__name__}: {e}")
        return 1

    total = len(FAILURES)
    print()
    if total:
        print(f"✗ {total} FAILURES in {path.name}:", *FAILURES, sep="\n  ")
        return 1
    print(f"✓ {path.name}: all checks pass")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
