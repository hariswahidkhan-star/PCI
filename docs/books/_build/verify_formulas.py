#!/usr/bin/env python3
"""Golden-answer verification for the PML-AI / PFL-AI Bodies of Knowledge.

Every worked example, in-text calculation and exercise in the manuscripts has a golden test here:
the number is recomputed with decimal arithmetic and compared to the value printed in the book.
A chapter cannot pass gate while this suite fails. Run:  python3 verify_formulas.py
"""
from decimal import Decimal as D, getcontext

getcontext().prec = 28
FAILURES = []


def check(name, computed, printed, tol=D("0.005")):
    computed, printed = D(str(computed)), D(str(printed))
    ok = abs(computed - printed) <= tol
    print(f"{'PASS' if ok else 'FAIL'}  {name}: computed {computed}  printed {printed}")
    if not ok:
        FAILURES.append(name)


def af(r, n):  # ordinary annuity factor
    return (1 - (1 + r) ** -n) / r


# ---------- PFL-AI Domain 3 — Time value of money ----------
check("WE 3.1.1 simple FV", D(100000) * (1 + D("0.08") * 3), 124000)
check("WE 3.1.1 compound FV", D(100000) * D("1.08") ** 3, D("125971.20"))
check("WE 3.1.2 PV of 500k @7% 5y", D(500000) / D("1.07") ** 5, D("356493.09"))
check("3.1.2 pitfall simple discount", D(500000) / (1 + D("0.07") * 5), D("370370.37"))
for t, f in enumerate([D("0.9091"), D("0.8264"), D("0.7513"), D("0.6830"), D("0.6209")], 1):
    check(f"DF({t}) @10%", (1 / D("1.10") ** t).quantize(D("0.0001")), f, tol=D("0.00005"))
check("WE 3.2.1 AF(9%,20)", af(D("0.09"), 20).quantize(D("0.000001")), D("9.128546"), tol=D("0.0000005"))
check("WE 3.2.1 PV of stream", D(3000000) * af(D("0.09"), 20), D("27385637"), tol=D("0.5"))
check("WE 3.2.1 sensitivity AF(10%,20)", af(D("0.10"), 20).quantize(D("0.000001")), D("8.513564"), tol=D("0.0000005"))
check("WE 3.2.1 sensitivity PV @10%", D(3000000) * af(D("0.10"), 20), D("25540691"), tol=D("0.5"))
check("WE 3.2.2 Kestrel instalment", D(42000000) * D("0.06") / (1 - D("1.06") ** -12), D("5009635.23"))
bal = D(42000000)
rows = []
for _ in range(3):
    i = bal * D("0.06")
    p = D(42000000) * D("0.06") / (1 - D("1.06") ** -12) - i
    bal -= p
    rows.append((i, p, bal))
check("WE 3.2.2 y1 interest", rows[0][0], 2520000)
check("WE 3.2.2 y1 principal", rows[0][1], D("2489635.23"))
check("WE 3.2.2 y3 closing", rows[2][2], D("34073997.27"))
check("WE 3.2.3 EAR quarterly", ((1 + D("0.06") / 4) ** 4 - 1).quantize(D("0.000001")), D("0.061364"), tol=D("0.0000005"))
check("3.2.3 EAR semi", ((1 + D("0.03")) ** 2 - 1).quantize(D("0.0001")), D("0.0609"), tol=D("0.00005"))
check("3.2.3 EAR monthly", ((1 + D("0.005")) ** 12 - 1).quantize(D("0.0001")), D("0.0617"), tol=D("0.00005"))
check("MCQ 3.2-A distractor D (10y factor)", D(42000000) / af(D("0.06"), 10), D("5706454"), tol=D("1"))
check("WE 3.3.1 Fisher real rate", (D("1.09") / D("1.03") - 1).quantize(D("0.0001")), D("0.0583"), tol=D("0.00005"))
check("WE 3.3.2 escalated O&M", D(10000000) * D("1.04") ** 3, 11248640)
check("WE 3.3.3 forward SAR", (D("3.75") * D("1.055") / D("1.05")).quantize(D("0.0001")), D("3.7679"), tol=D("0.00005"))
import math
check("3.A.1 continuous FV", D(str(100000 * math.exp(0.24))).quantize(D("1")), 127125, tol=D("1"))
check("Case A AF(8%,25)", af(D("0.08"), 25).quantize(D("0.000001")), D("10.674776"), tol=D("0.0000005"))
check("Case A PV of supplement", D(5600000) * af(D("0.08"), 25), D("59778747"), tol=D("0.5"))
check("Case A grantor AF(10%,25)", af(D("0.10"), 25).quantize(D("0.000001")), D("9.077040"), tol=D("0.0000005"))
check("Case A grantor PV", D(5600000) * af(D("0.10"), 25), D("50831424"), tol=D("0.5"))
check("Case B annuity instalment", D(30000000) * D("0.075") / (1 - D("1.075") ** -7), D("5664009.46"))
check("Case B annuity total interest", D(30000000) * D("0.075") / (1 - D("1.075") ** -7) * 7 - 30000000, D("9648066"), tol=D("0.5"))
check("Case B bullet total interest", D(30000000) * D("0.075") * 7, 15750000)
check("Case B refi instalment @9.8%", D(30000000) * D("0.098") / (1 - D("1.098") ** -7), D("6121646"), tol=D("0.5"))
check("EX 3.1 PV", D(850000) / D("1.11") ** 8, D("368837.52"))
check("EX 3.1 error n=7", D(850000) / D("1.11") ** 7, D("409410"), tol=D("0.5"))
check("EX 3.2 monthly payment", D(2500000) * (D("0.05") / 12) / (1 - (1 + D("0.05") / 12) ** -240), D("16498.89"))
check("EX 3.3 EAR 12% monthly", ((1 + D("0.01")) ** 12 - 1).quantize(D("0.000001")), D("0.126825"), tol=D("0.0000005"))
check("EX 3.4 perpetuity", D(1200000) / D("0.085"), D("14117647.06"))
check("EX 3.5 real return", (D("1.12") / D("1.05") - 1).quantize(D("0.0001")), D("0.0667"), tol=D("0.00005"))
check("WE 3.1.1 interp 1.08^25", (D("1.08") ** 25).quantize(D("0.01")), D("6.85"), tol=D("0.005"))
check("Fig 3.1.1 y5 compound", D(100000) * D("1.08") ** 5, 146933, tol=D("0.5"))
check("Fig 3.1.1 y10 compound", D(100000) * D("1.08") ** 10, 215892, tol=D("0.6"))
check("Fig 3.3.1 y20 real value", D(1000000) / D("1.03") ** 20, 553676, tol=D("0.5"))

# ---------- PML-AI Domain 6 — Planning, scheduling, delivery flow ----------
ACTS = {"A": (2, []), "B": (6, ["A"]), "C": (8, ["B"]), "D": (7, ["B"]),
        "E": (5, ["C", "D"]), "F": (4, ["E"]), "G": (2, ["D"])}


def cpm(acts):
    order = list(acts)
    ES, EF = {}, {}
    for k in order:
        d, pred = acts[k]
        ES[k] = max((EF[x] for x in pred), default=0)
        EF[k] = ES[k] + d
    dur = max(EF.values())
    succ = {k: [] for k in acts}
    for k, (d, pred) in acts.items():
        for x in pred:
            succ[x].append(k)
    LF, LS = {}, {}
    for k in reversed(order):
        d, _ = acts[k]
        LF[k] = min((LS[x] for x in succ[k]), default=dur)
        LS[k] = LF[k] - d
    TF = {k: LS[k] - ES[k] for k in acts}
    FF = {k: min((ES[x] for x in succ[k]), default=dur) - EF[k] for k in acts}
    return dur, ES, EF, LS, LF, TF, FF


dur, ES, EF, LS, LF, TF, FF = cpm(ACTS)
check("Auriga duration", dur, 25, tol=D("0"))
expected = {"A": (0, 2, 0, 2, 0, 0), "B": (2, 8, 2, 8, 0, 0), "C": (8, 16, 8, 16, 0, 0),
            "D": (8, 15, 9, 16, 1, 0), "E": (16, 21, 16, 21, 0, 0),
            "F": (21, 25, 21, 25, 0, 0), "G": (15, 17, 23, 25, 8, 8)}
for k, (es, ef, ls, lf, tf, ff) in expected.items():
    for label, got, want in (("ES", ES[k], es), ("EF", EF[k], ef), ("LS", LS[k], ls),
                             ("LF", LF[k], lf), ("TF", TF[k], tf), ("FF", FF[k], ff)):
        check(f"Auriga {k}.{label}", got, want, tol=D("0"))
check("Critical path sums to duration", 2 + 6 + 8 + 5 + 4, 25, tol=D("0"))
crash1 = dict(ACTS); crash1["C"] = (7, ["B"])
check("Crash C by 1 → duration", cpm(crash1)[0], 24, tol=D("0"))
crash2 = dict(ACTS); crash2["C"] = (6, ["B"])
check("Crash C by 2 → duration (co-critical)", cpm(crash2)[0], 24, tol=D("0"))
slip = dict(ACTS); slip["D"] = (10, ["B"])
sdur, sES, *_ = cpm(slip)
check("D=10 → duration", sdur, 27, tol=D("0"))
check("D=10 → E starts", sES["E"], 18, tol=D("0"))
check("WE 6.4.2 crash week1 net", 45000 - 30000, 15000, tol=D("0"))
check("WE 6.4.3 PERT te", D(4 + 4 * 5 + 12) / 6, D("6.0"))
check("WE 6.4.3 PERT sigma", (D(12 - 4) / 6).quantize(D("0.01")), D("1.33"), tol=D("0.005"))
check("EX 6.4 te", D(6 + 4 * 8 + 16) / 6, D("9.0"))
check("EX 6.4 sigma", (D(16 - 6) / 6).quantize(D("0.01")), D("1.67"), tol=D("0.005"))
check("Case A recovery: penalty exposure", 2 * 45000, 90000, tol=D("0"))
check("Case A recovery: chosen plan expected cost", 35000 + D("0.2") * 60000, 47000, tol=D("0"))
check("Fig 6.2.1 duration via B-D-E-F path", 2 + 6 + 7 + 5 + 4, 24, tol=D("0"))

print()
if FAILURES:
    print(f"✗ {len(FAILURES)} FAILURES:", *FAILURES, sep="\n  ")
    raise SystemExit(1)
print("✓ all golden answers verified")
