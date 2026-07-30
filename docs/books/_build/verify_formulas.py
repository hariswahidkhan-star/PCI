#!/usr/bin/env python3
"""Golden-answer verification for the PML-AI / PFL-AI Bodies of Knowledge.

Every worked example, in-text calculation and exercise in the manuscripts has a golden test here:
the number is recomputed with decimal arithmetic and compared to the value printed in the book.
A chapter cannot pass gate while this suite fails. Run:  python3 verify_formulas.py
"""
import importlib.util
import pathlib
import re
from decimal import Decimal as D, getcontext

getcontext().prec = 28
FAILURES = []


# Attribution of each assertion to its book and domain, recorded at RUNTIME. Source counting cannot
# do this: several sections below assert inside loops, so one `check(` occurrence emits many lines.
# Appendix C states the size of the verification record to a reader deciding how much to trust the
# arithmetic, so the record counts itself rather than being estimated from the source.
CURRENT = [None, None]
TALLY = {}


def section(book, domain):
    CURRENT[0], CURRENT[1] = book, domain


def check(name, computed, printed, tol=D("0.005")):
    computed, printed = D(str(computed)), D(str(printed))
    ok = abs(computed - printed) <= tol
    print(f"{'PASS' if ok else 'FAIL'}  {name}: computed {computed}  printed {printed}")
    if CURRENT[0]:
        TALLY[(CURRENT[0], CURRENT[1])] = TALLY.get((CURRENT[0], CURRENT[1]), 0) + 1
    if not ok:
        FAILURES.append(name)


def af(r, n):  # ordinary annuity factor
    return (1 - (1 + r) ** -n) / r


# ---------- PFL-AI Domain 3 — Time value of money ----------
section("PFL-AI", 3)
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
section("PML-AI", 6)
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

# ---------- Batch 2 expansions ----------
# PFL-AI D3: level-principal schedule (WE 3.2.2b)
check("WE 3.2.2b LP y1 total", D(3500000) + D(42000000) * D("0.06"), 6020000)
check("WE 3.2.2b LP y2 total", D(3500000) + D(38500000) * D("0.06"), 5810000)
check("WE 3.2.2b LP y12 total", D(3500000) + D(3500000) * D("0.06"), 3710000)
check("WE 3.2.2b LP lifetime interest", D("0.06") * (D(42000000) + D(3500000)) / 2 * 12, 16380000)
check("WE 3.2.2b annuity lifetime interest",
      D(42000000) * D("0.06") / (1 - D("1.06") ** -12) * 12 - 42000000, D("18115623"), tol=D("0.5"))
check("WE 3.2.2b bullet lifetime interest", D(42000000) * D("0.06") * 12, 30240000)
check("WE 3.2.2b y1 LP vs annuity gap", 6020000 - D("5009635.23"), D("1010364.77"))
# annuity-due and deferred (WE 3.2.1b/c)
check("WE 3.2.1b AF(8%,5)", af(D("0.08"), 5).quantize(D("0.000001")), D("3.992710"), tol=D("0.0000005"))
check("WE 3.2.1b ordinary", D(500000) * af(D("0.08"), 5), D("1996355.02"))
check("WE 3.2.1b due", D(500000) * af(D("0.08"), 5) * D("1.08"), D("2156063.42"))
check("WE 3.2.1b due premium", D(500000) * af(D("0.08"), 5) * D("0.08"), D("159708.40"), tol=D("0.5"))
check("WE 3.2.1c deferred PV", D(5600000) * af(D("0.08"), 25) / D("1.08") ** 3, D("47454296"), tol=D("0.5"))
# day count (WE 3.3.4)
p = D(42000000) * D("0.06")
check("WE 3.3.4 30/360", p * 90 / 360, 630000)
check("WE 3.3.4 act/360", p * 92 / 360, 644000)
check("WE 3.3.4 act/365", p * 92 / 365, D("635178.08"))
# exact-date discounting (WE 3.A.2)
check("WE 3.A.2 exact-date PV", D(1000000) / (D("1.09") ** (D(500) / D(365))), D("888650"), tol=D("1"))
check("WE 3.A.2 naive year-2", D(1000000) / D("1.09") ** 2, D("841680"), tol=D("0.5"))
check("WE 3.A.2 naive year-1", D(1000000) / D("1.09"), D("917431"), tol=D("0.5"))
# new MCQs
check("MCQ 3.1-E PV 400k y2", D(400000) / D("1.09") ** 2, D("336672"), tol=D("0.5"))
check("MCQ 3.1-E PV 470k y4", D(470000) / D("1.09") ** 4, D("332960"), tol=D("0.5"))
check("MCQ 3.1-E distractor C", D(470000) / D("1.09") ** 2, D("395590"), tol=D("0.5"))
check("MCQ 3.1-E difference", D(400000) / D("1.09") ** 2 - D(470000) / D("1.09") ** 4, D("3712"), tol=D("0.5"))
check("MCQ 3.3-E compound multiple 25y", (D("1.04") ** 25).quantize(D("0.01")), D("2.67"), tol=D("0.005"))
check("SAR parallel of 356,493.09", D("356493.09") * D("3.75"), D("1336849.09"), tol=D("0.01"))
# Fig 3.1.2 / 3.3.2 data points
check("Fig 3.1.2 DF10 @6%", (1 / D("1.06") ** 10).quantize(D("0.001")), D("0.558"), tol=D("0.0005"))
check("Fig 3.1.2 DF10 @10%", (1 / D("1.10") ** 10).quantize(D("0.001")), D("0.386"), tol=D("0.0005"))
check("Fig 3.1.2 DF10 @14%", (1 / D("1.14") ** 10).quantize(D("0.001")), D("0.270"), tol=D("0.0005"))
check("Fig 3.1.2 DF25 @6%", (1 / D("1.06") ** 25).quantize(D("0.001")), D("0.233"), tol=D("0.0005"))
check("Fig 3.3.2 compound y25", D(10000000) * D("1.04") ** 25, D("26658363"), tol=D("1"))
check("Fig 3.3.2 simple y25", D(10000000) * 2, 20000000)
# PML-AI D6: lag conversion (WE 6.1.2)
check("WE 6.1.2 FS+2 completion", 4 + 2 + 6, 12, tol=D("0"))
check("WE 6.1.2 SS+1 completion", 0 + 1 + 6, 7, tol=D("0"))
# hard-cap levelling (WE 6.3.1b)
check("WE 6.3.1b extension cost", 4 * 45000, 180000, tol=D("0"))
check("WE 6.3.1b second-shift cost", 4 * 20000, 80000, tol=D("0"))
check("MCQ 6.3-C saving", 180000 - 80000, 100000, tol=D("0"))
# earned-schedule bridge
check("SPI(t) bridge", (D(20) / D(22)).quantize(D("0.01")), D("0.91"), tol=D("0.005"))
check("MCQ 6.2-D G float", 25 - 17, 8, tol=D("0"))

# ---------- Batch 3 expansions ----------
# PFL-AI D3: term structure (WE 3.A.3)
check("WE 3.A.3 curve yr1", D(1000000) / D("1.05"), D("952381"), tol=D("0.5"))
check("WE 3.A.3 curve yr2", D(1000000) / D("1.07") ** 2, D("873439"), tol=D("0.5"))
check("WE 3.A.3 curve total", D(1000000) / D("1.05") + D(1000000) / D("1.07") ** 2, D("1825820"), tol=D("0.5"))
check("WE 3.A.3 flat yr1", D(1000000) / D("1.06"), D("943396"), tol=D("0.5"))
check("WE 3.A.3 flat yr2", D(1000000) / D("1.06") ** 2, D("889996"), tol=D("0.5"))
check("WE 3.A.3 flat total", D(1000000) / D("1.06") + D(1000000) / D("1.06") ** 2, D("1833393"), tol=D("0.5"))
check("WE 3.A.3 overstatement", (D(1000000) / D("1.06") + D(1000000) / D("1.06") ** 2)
      - (D(1000000) / D("1.05") + D(1000000) / D("1.07") ** 2), D("7573"), tol=D("0.5"))
# Case A breakeven refinement
check("Case A required AF", (D(58000000) / D(5600000)).quantize(D("0.0001")), D("10.3571"), tol=D("0.00005"))
check("Case A AF(8.36%,25) brackets breakeven", af(D("0.0836"), 25).quantize(D("0.0001")), D("10.3545"), tol=D("0.005"))
# new MCQs
check("MCQ 3.1-F FV", D(250000) * D("1.07") ** 6, D("375183"), tol=D("0.5"))
check("MCQ 3.1-F distractor B", D(250000) * D("1.07") ** 5, D("350638"), tol=D("0.5"))
check("MCQ 3.1-F distractor A", D(250000) * (1 + D("0.07") * 6), 355000)
check("MCQ 3.1-F distractor D", D(250000) / D("1.07") ** 6, D("166586"), tol=D("0.5"))
check("MCQ 3.2-F perpetuity", D(800000) / D("0.08"), 10000000)
check("MCQ 3.2-F AF(8%,30)", af(D("0.08"), 30).quantize(D("0.000001")), D("11.257783"), tol=D("0.0000005"))
check("MCQ 3.2-F 30y annuity", D(800000) * af(D("0.08"), 30), D("9006227"), tol=D("0.5"))
check("MCQ 3.2-F tail", D(800000) / D("0.08") - D(800000) * af(D("0.08"), 30), D("993773"), tol=D("0.5"))
check("MCQ 3.3-F real value", D(2000000) / D("1.03") ** 5, D("1725218"), tol=D("0.5"))
check("MCQ 3.3-F distractor C", D(2000000) * (1 - D("0.03") * 5), 1700000)
check("SAR instalment parallel", D("5009635.23") * D("3.75"), D("18786132.11"), tol=D("0.01"))
check("SAR bonus parallel", D(45000) * D("3.75"), 168750)
# PML-AI D6: negative-float recovery (WE 6.A.4)
crash_ce = dict(ACTS); crash_ce["C"] = (7, ["B"]); crash_ce["E"] = (4, ["C", "D"])
check("WE 6.A.4 crash C+E → 23", cpm(crash_ce)[0], 23, tol=D("0"))
crash_cc = dict(ACTS); crash_cc["C"] = (6, ["B"])
check("WE 6.A.4 C alone twice stays 24", cpm(crash_cc)[0], 24, tol=D("0"))
check("WE 6.A.4 recovery cost", 30000 + 55000, 85000, tol=D("0"))
# new MCQs
slip_c = dict(ACTS); slip_c["C"] = (9, ["B"])
sd, sE, sEF, *_ = cpm(slip_c)
check("MCQ 6.2-F C=9 duration", sd, 26, tol=D("0"))
check("MCQ 6.2-F C=9 E window", sE["E"], 17, tol=D("0"))
check("MCQ 6.3-E excess crew-weeks", (5 - 4) * 7, 7, tol=D("0"))
check("MCQ 6.4-F te", D(3 + 4 * 4 + 8) / 6, D("4.5"))
check("MCQ 6.4-F sigma", (D(8 - 3) / 6).quantize(D("0.01")), D("0.83"), tol=D("0.005"))

# ---------- PFL-AI Domain 4 — Investment appraisal ----------
section("PFL-AI", 4)
AF15 = af(D("0.08"), 15)
check("D4 AF(8%,15)", AF15.quantize(D("0.000001")), D("8.559479"), tol=D("0.0000005"))
check("WE 4.1.1 PV inflows", D(8900000) * AF15, D("76179360"), tol=D("0.5"))
check("WE 4.1.1 NPV", D(8900000) * AF15 - 60000000, D("16179360"), tol=D("0.5"))
check("WE 4.1.1 SAR parallel", (D(8900000) * AF15 - 60000000) * D("3.75") / 1000000, D("60.7"), tol=D("0.05"))
check("Fig 4.1.1 NPV at 0%", D(8900000) * 15 - 60000000, 73500000)
check("Fig 4.1.1 NPV at 20%", D(8900000) * af(D("0.20"), 15) - 60000000, D("-18388293"), tol=D("1"))
check("4.1.2 IRR AF target", (D(60000000) / D(8900000)).quantize(D("0.000001")), D("6.741573"), tol=D("0.0000005"))
check("4.1.2 IRR substitution ~0", D(8900000) * af(D("0.1219"), 15) - 60000000, 0, tol=D("35000"))
for r in ("0.10", "0.20"):
    check(f"4.1.2 dual-root NPV at {r}", D(-1000000) + D(2300000) / (1 + D(r)) - D(1320000) / (1 + D(r)) ** 2, 0, tol=D("0.01"))
check("4.1.2 dual-root NPV at 15% positive", D(-1000000) + D(2300000) / D("1.15") - D(1320000) / D("1.15") ** 2, D("1890.36"), tol=D("0.01"))
FVAF15 = (D("1.08") ** 15 - 1) / D("0.08")
check("WE 4.1.3 FVAF(8,15)", FVAF15.quantize(D("0.000001")), D("27.152114"), tol=D("0.0000005"))
check("WE 4.1.3 terminal value", D(8900000) * FVAF15, D("241653814"), tol=D("0.5"))
check("WE 4.1.3 MIRR", ((D(8900000) * FVAF15 / 60000000) ** (D(1) / 15) - 1).quantize(D("0.0001")), D("0.0973"), tol=D("0.00005"))
check("MCQ 4.1-C distractor D", (D("4.027564") / 15).quantize(D("0.0001")), D("0.2685"), tol=D("0.00005"))
check("WE 4.2.1 simple payback", (D(60000000) / D(8900000)).quantize(D("0.01")), D("6.74"), tol=D("0.005"))
check("WE 4.2.1 cum PV yr10", D(8900000) * af(D("0.08"), 10), D("59719724"), tol=D("0.5"))
check("WE 4.2.1 shortfall", 60000000 - D(8900000) * af(D("0.08"), 10), D("280276"), tol=D("0.5"))
check("WE 4.2.1 disc flow y11", D(8900000) / D("1.08") ** 11, D("3817057"), tol=D("0.5"))
check("WE 4.2.1 DPB", (10 + (60000000 - D(8900000) * af(D("0.08"), 10)) / (D(8900000) / D("1.08") ** 11)).quantize(D("0.01")), D("10.07"), tol=D("0.005"))
check("4.2.2 PI", (D(8900000) * AF15 / 60000000).quantize(D("0.001")), D("1.270"), tol=D("0.0005"))
check("WE 4.2.3 A PV cost", D(5000000) + D(800000) * af(D("0.08"), 3), D("7061678"), tol=D("0.5"))
check("WE 4.2.3 EAC A", (D(5000000) + D(800000) * af(D("0.08"), 3)) / af(D("0.08"), 3), D("2740168"), tol=D("0.5"))
check("WE 4.2.3 B PV cost", D(7600000) + D(500000) * af(D("0.08"), 5), D("9596355"), tol=D("0.5"))
check("WE 4.2.3 EAC B", (D(7600000) + D(500000) * af(D("0.08"), 5)) / af(D("0.08"), 5), D("2403469"), tol=D("0.5"))
check("WE 4.2.3 annual saving", (D(5000000) + D(800000) * af(D("0.08"), 3)) / af(D("0.08"), 3)
      - (D(7600000) + D(500000) * af(D("0.08"), 5)) / af(D("0.08"), 5), D("336699"), tol=D("1"))
check("MCQ 4.2-B distractor C (A/3)", (D(5000000) + D(800000) * af(D("0.08"), 3)) / 3, D("2353893"), tol=D("1"))
check("MCQ 4.2-B distractor C (B/5)", (D(7600000) + D(500000) * af(D("0.08"), 5)) / 5, D("1919271"), tol=D("1"))
check("4.3.1 P NPV", D(2000000) * af(D("0.08"), 5) - 5000000, D("2985420"), tol=D("0.5"))
check("4.3.1 Q NPV", D(6000000) * af(D("0.08"), 5) - 20000000, D("3956260"), tol=D("0.5"))
check("4.3.1 NPV gap", (D(6000000) * af(D("0.08"), 5) - 20000000) - (D(2000000) * af(D("0.08"), 5) - 5000000), D("970840"), tol=D("0.5"))
check("4.3.1 P IRR ~28.65", D(2000000) * af(D("0.2865"), 5) - 5000000, 0, tol=D("2000"))
check("4.3.1 Q IRR ~15.24", D(6000000) * af(D("0.1524"), 5) - 20000000, 0, tol=D("6000"))
check("4.3.1 incremental IRR ~10.42", D(4000000) * af(D("0.1042"), 5) - 15000000, 0, tol=D("6000"))
for nm, i0, nv, pi in (("W", 8, D("2.4"), "1.300"), ("X", 12, D("3.0"), "1.250"),
                       ("Y", 10, D("2.2"), "1.220"), ("Z", 6, D("1.02"), "1.170")):
    check(f"4.3.2 PI {nm}", ((nv + i0) / i0).quantize(D("0.001")), D(pi), tol=D("0.0005"))
check("4.3.2 W+X NPV", D("2.4") + 3, D("5.4"))
check("4.3.2 W+Y NPV", D("2.4") + D("2.2"), D("4.6"))
check("MCQ 4.3-B distractor D", D("3.0") + D("1.02"), D("4.02"))
check("EX 4.1 NPV", D(3000000) / D("1.10") + D(4000000) / D("1.10") ** 2 + D(5000000) / D("1.10") ** 3
      + D(5000000) / D("1.10") ** 4 - 12000000, D("1204699.13"))
check("EX 4.2 AF target", (D(9000000) / D(1500000)).quantize(D("0.01")), D("6.00"))
check("EX 4.2 IRR ~10.56", D(1500000) * af(D("0.1056"), 10) - 9000000, 0, tol=D("3000"))
check("EX 4.3 FVAF(7,6)", ((D("1.07") ** 6 - 1) / D("0.07")).quantize(D("0.000001")), D("7.153291"), tol=D("0.0000005"))
check("EX 4.3 TV", D(2600000) * (D("1.07") ** 6 - 1) / D("0.07"), D("18598556"), tol=D("0.5"))
check("EX 4.3 MIRR", ((D(2600000) * (D("1.07") ** 6 - 1) / D("0.07") / 10000000) ** (D(1) / 6) - 1).quantize(D("0.0001")), D("0.1090"), tol=D("0.00005"))
check("EX 4.4 EAC C1", (D(3000000) + D(250000) * af(D("0.09"), 4)) / af(D("0.09"), 4), D("1176006"), tol=D("0.5"))
check("EX 4.4 EAC C2", (D(4200000) + D(150000) * af(D("0.09"), 6)) / af(D("0.09"), 6), D("1086263"), tol=D("0.5"))
check("EX 4.4 saving", (D(3000000) + D(250000) * af(D("0.09"), 4)) / af(D("0.09"), 4)
      - (D(4200000) + D(150000) * af(D("0.09"), 6)) / af(D("0.09"), 6), D("89743"), tol=D("1"))
check("EX 4.5 greedy set", D("2.6") + D("1.84") + D("1.4"), D("5.84"))
check("EX 4.5 optimal set", D("2.6") + D("3.3"), D("5.9"))
check("4.A.5 EAV invariant", ((D(8900000) * AF15 - 60000000) / AF15 * AF15), D(8900000) * AF15 - 60000000, tol=D("0.01"))

# ---------- PFL-AI Domain 1 — Foundations ----------
section("PFL-AI", 1)
check("WE 1.2.2 profit", 10000000 - 8000000, 2000000, tol=D("0"))
check("WE 1.2.2 operating cash", D(2000000) - 3000000 - 1000000 + 500000, -1500000, tol=D("0"))
check("WE 1.2.3 debt service", D(70000000) * D("0.06"), 4200000)
for cash, unlev, eqcash, lev in ((12000000, "0.12", 7800000, "0.26"), (9000000, "0.09", 4800000, "0.16"),
                                 (6000000, "0.06", 1800000, "0.06"), (4200000, "0.042", 0, "0.0")):
    check(f"WE 1.2.3 unlevered at {cash}", (D(cash) / 100000000).quantize(D("0.001")), D(unlev), tol=D("0.0005"))
    check(f"WE 1.2.3 equity cash at {cash}", D(cash) - 4200000, eqcash, tol=D("0"))
    check(f"WE 1.2.3 levered at {cash}", (D(max(cash - 4200000, 0)) / 30000000).quantize(D("0.001")), D(lev), tol=D("0.0005"))
check("WE 1.2.3 equity-zero decline", ((D(12000000) - 4200000) / 12000000).quantize(D("0.001")), D("0.65"), tol=D("0.0005"))
check("EX 1.1 operating cash", D(3500000) - 2200000 - 900000 + 600000, 1000000, tol=D("0"))
check("EX 1.1 wrong-sign variant", D(3500000) - 2200000 - 900000 - 600000, -200000, tol=D("0"))
check("EX 1.2 debt service 80m@6.5%", D(80000000) * D("0.065"), 5200000)
check("EX 1.2 base levered", ((D(12000000) - 5200000) / 20000000).quantize(D("0.001")), D("0.34"), tol=D("0.0005"))
check("EX 1.2 cliff decline", ((D(12000000) - 5200000) / 12000000).quantize(D("0.001")), D("0.567"), tol=D("0.0005"))
check("MCQ 1.2-A distractor D", D(2000000) - 3000000 - 1000000 - 500000, -2500000, tol=D("0"))
check("MCQ 1.2-B distractor D", (D(9000000) / 30000000).quantize(D("0.001")), D("0.30"), tol=D("0.0005"))

# ---------- PML-AI Domain 7 — Cost, resources and commercial awareness ----------
section("PML-AI", 7)
BAC7, PV7, EV7, AC7 = D(4000000), D(2080000), D(1920000), D(2120000)
CPI7, SPI7 = EV7 / AC7, EV7 / PV7
check("WE 7.3.2 CV", EV7 - AC7, -200000)
check("WE 7.3.2 SV", EV7 - PV7, -160000)
check("WE 7.3.2 CPI", CPI7.quantize(D("0.01")), D("0.91"), tol=D("0.005"))
check("WE 7.3.2 SPI", SPI7.quantize(D("0.01")), D("0.92"), tol=D("0.005"))
check("WE 7.3.2 percent complete", (EV7 / BAC7 * 100).quantize(D("0.1")), D("48.0"), tol=D("0.05"))
check("WE 7.3.2 percent spent", (AC7 / BAC7 * 100).quantize(D("0.1")), D("53.0"), tol=D("0.05"))
check("WE 7.3.3 EAC(a) budgeted rate", AC7 + (BAC7 - EV7), 4200000)
check("WE 7.3.3 EAC(b) BAC/CPI", BAC7 / CPI7, D("4416666.67"))
check("WE 7.3.3 EAC(c) CPIxSPI", AC7 + (BAC7 - EV7) / (CPI7 * SPI7), D("4608055.56"))
check("WE 7.3.3 EAC spread (c)-(a)", AC7 + (BAC7 - EV7) / (CPI7 * SPI7) - (AC7 + (BAC7 - EV7)), D("408055.56"))
check("EX 7.2 rounded-CPI error", D(4000000) / D("0.91"), D("4395604"), tol=D("1"))
check("WE 7.3.4 VAC(a)", BAC7 - (AC7 + (BAC7 - EV7)), -200000)
check("WE 7.3.4 VAC(b)", BAC7 - BAC7 / CPI7, D("-416666.67"))
check("WE 7.3.4 VAC(c)", BAC7 - (AC7 + (BAC7 - EV7) / (CPI7 * SPI7)), D("-608055.56"))
check("WE 7.3.4 TCPI to BAC", ((BAC7 - EV7) / (BAC7 - AC7)).quantize(D("0.01")), D("1.11"), tol=D("0.005"))
# Identity: TCPI to an EAC of BAC/CPI is exactly the current CPI ("nothing changes").
check("WE 7.3.4 TCPI to EAC(b) == CPI", (BAC7 - EV7) / (BAC7 / CPI7 - AC7), CPI7, tol=D("0.000001"))
check("EX 7.3 TCPI wrong denominator (PV)", ((BAC7 - EV7) / (BAC7 - PV7)).quantize(D("0.01")), D("1.08"), tol=D("0.005"))
check("7.A.1 earned schedule SPI(t)", (D(12) / D(13)).quantize(D("0.01")), D("0.92"), tol=D("0.005"))
# Estimating
check("WE 7.1.2 PERT cost te", (D(680000) + 4 * D(750000) + D(1000000)) / 6, 780000)
check("WE 7.1.2 PERT cost sigma", ((D(1000000) - D(680000)) / 6).quantize(D("0.01")), D("53333.33"))
check("WE 7.1.2 mean-over-mode", (D(680000) + 4 * D(750000) + D(1000000)) / 6 - 750000, 30000)
# Resource economics
check("WE 7.4.1 blended rate", (D(40) * 95 + D(25) * 140 + D(15) * 210) / 80, D("130.625"))
check("MCQ 7.4-D distractor A (unweighted)", (D(95) + 140 + 210) / 3, D("148.33"), tol=D("0.005"))
check("EX 7.5 blended rate", (D(30) * 105 + D(20) * 150 + D(10) * 220) / 60, D("139.1667"), tol=D("0.0001"))
check("EX 7.5 crew cost", 5 * 40 * 4 * ((D(30) * 105 + D(20) * 150 + D(10) * 220) / 60), D("111333.33"))
# Incentive contracts
tc7, tf7, ceiling7, buyer_share = D(2000000), D(150000), D(2450000), D("0.70")
check("WE 7.4.3 overrun", D(2300000) - tc7, 300000)
check("WE 7.4.3 fee", tf7 - (D(2300000) - tc7) * D("0.30"), 60000)
check("WE 7.4.3 buyer pays", D(2300000) + (tf7 - (D(2300000) - tc7) * D("0.30")), 2360000)
check("WE 7.4.3 target price", tc7 + tf7, 2150000)
check("WE 7.4.3 PTA", tc7 + (ceiling7 - (tc7 + tf7)) / buyer_share, D("2428571.43"))
# At the PTA the buyer's outlay equals the ceiling price exactly.
pta7 = tc7 + (ceiling7 - (tc7 + tf7)) / buyer_share
check("WE 7.4.3 PTA outlay == ceiling", pta7 + (tf7 - (pta7 - tc7) * D("0.30")), ceiling7, tol=D("0.01"))
check("EX 7.4 fee", D(180000) - (D(2650000) - D(2400000)) * D("0.20"), 130000)
check("EX 7.4 buyer pays", D(2650000) + 130000, 2780000)
check("EX 7.4 PTA", D(2400000) + (D(2900000) - (D(2400000) + D(180000))) / D("0.80"), 2800000)
# Cash
check("7.4.4 unpaid at 60-day terms", AC7 * D("0.35"), 742000)

# ---------- PML-AI Domain 1 — The project leadership profession ----------
section("PML-AI", 1)
CLIN, ADOPT, HRS, RATE, WKS = D(40), D("0.70"), D(6), D(85), D(48)
check("WE 1.3.2 adopting clinics", CLIN * ADOPT, 28)
check("WE 1.3.2 annual benefit", CLIN * ADOPT * HRS * RATE * WKS, 685440)
check("WE 1.3.2 output-based claim", CLIN * HRS * RATE * WKS, 979200)
check("WE 1.3.2 overstatement", CLIN * HRS * RATE * WKS - CLIN * ADOPT * HRS * RATE * WKS, 293760)
# The overstatement is exactly the non-adoption rate — the point the example is built to make.
check("WE 1.3.2 overstatement == non-adoption rate",
      ((CLIN * HRS * RATE * WKS - CLIN * ADOPT * HRS * RATE * WKS) / (CLIN * HRS * RATE * WKS) * 100).quantize(D("0.1")),
      D("30.0"), tol=D("0.05"))
check("WE 1.3.2 SAR parallel (millions)", (D(685440) * D("3.75") / 1000000).quantize(D("0.01")), D("2.57"), tol=D("0.005"))
for a, v in (("0.50", 489600), ("0.70", 685440), ("0.90", 881280)):
    check(f"WE 1.3.2 sensitivity at {a} adoption", CLIN * D(a) * HRS * RATE * WKS, v)
check("WE 1.3.3 benefit per week", CLIN * ADOPT * HRS * RATE, 14280)
check("WE 1.3.3 eight weeks", CLIN * ADOPT * HRS * RATE * 8, 114240)
check("WE 1.3.3 net of acceleration", CLIN * ADOPT * HRS * RATE * 8 - 60000, 54240)
check("WE 1.3.3 breakeven weeks", (D(60000) / (CLIN * ADOPT * HRS * RATE)).quantize(D("0.01")), D("4.20"), tol=D("0.005"))
check("Case A actual at 40% adoption", CLIN * D("0.40") * HRS * RATE * WKS, 391680)
check("EX 1.1 adopting sites", D(60) * D("0.65"), 39)
check("EX 1.1 benefit", D(60) * D("0.65") * 5 * 90 * 46, 807300)
check("EX 1.1 output claim", D(60) * 5 * 90 * 46, 1242000)
check("EX 1.1 overstatement pct", ((D(1242000) - D(807300)) / D(1242000) * 100).quantize(D("0.1")), D("35.0"), tol=D("0.05"))
check("EX 1.2 five-week cost of delay", CLIN * ADOPT * HRS * RATE * 5, 71400)
check("EX 1.3 adoption spread 50→90", CLIN * D("0.90") * HRS * RATE * WKS - CLIN * D("0.50") * HRS * RATE * WKS, 391680)

# ---------- PFL-AI Domain 2 — Accounting foundations ----------
section("PFL-AI", 2)
REV2, OPEX2 = D(12000000), D(4500000)
DEP2 = D(60000000) / 25            # Domain 4's I₀ over 25 years
INT2 = D(42000000) * D("0.06")     # Domain 3's year-one interest
EBITDA2 = REV2 - OPEX2
EBIT2 = EBITDA2 - DEP2
PBT2 = EBIT2 - INT2
TAX2 = PBT2 * D("0.20")
NI2 = PBT2 - TAX2
DS2 = D(42000000) * D("0.06") / (1 - D("1.06") ** -12)   # Domain 3's instalment
check("WE 2.2.1 EBITDA", EBITDA2, 7500000)
check("WE 2.2.1 depreciation", DEP2, 2400000)
check("WE 2.2.1 EBIT", EBIT2, 5100000)
check("WE 2.2.1 interest ties to D3", INT2, 2520000)
check("WE 2.2.1 PBT", PBT2, 2580000)
check("WE 2.2.1 tax", TAX2, 516000)
check("WE 2.2.1 net income", NI2, 2064000)
check("MCQ 2.2-A distractor C (tax on EBIT)", EBIT2 - EBIT2 * D("0.20"), 4080000)
check("2.2.2 plant net of depreciation", D(60000000) - DEP2, 57600000)
check("2.2.2 year-one principal ties to D3", DS2 - INT2, D("2489635.23"))
check("2.2.2 closing senior debt", D(42000000) - (DS2 - INT2), D("39510364.77"))
check("WE 2.2.4 operating cash flow", NI2 + DEP2 - 900000 + 300000, 3864000)
check("WE 2.2.4 net working-capital absorption", D(900000) - 300000, 600000)
check("MCQ 2.2-B distractor A (no WC)", NI2 + DEP2, 4464000)
check("MCQ 2.2-B distractor C (payables signed wrong)", NI2 + DEP2 - 900000 - 300000, 3264000)
check("WE 2.3.1 CFADS excl. working capital", EBITDA2 - TAX2, 6984000)
check("WE 2.3.1 DSCR excl. working capital", ((EBITDA2 - TAX2) / DS2).quantize(D("0.01")), D("1.39"), tol=D("0.005"))
check("WE 2.3.1 CFADS incl. working capital", EBITDA2 - TAX2 - 600000, 6384000)
check("WE 2.3.1 DSCR incl. working capital", ((EBITDA2 - TAX2 - 600000) / DS2).quantize(D("0.01")), D("1.27"), tol=D("0.005"))
check("WE 2.3.3 capitalised annual charge", D(1200000) / 10, 120000)
check("WE 2.3.3 profit difference", D(1200000) - D(1200000) / 10, 1080000)
check("WE 2.4.2 interest cover", (EBIT2 / INT2).quantize(D("0.01")), D("2.02"), tol=D("0.005"))
check("WE 2.4.2 debt/EBITDA", ((D(42000000) - (DS2 - INT2)) / EBITDA2).quantize(D("0.01")), D("5.27"), tol=D("0.005"))
check("Case B capitalised charge", D(9000000) / 12, 750000)
check("Case B profit lift vs expensing", D(9000000) - D(9000000) / 12, 8250000)
# Exercises
EBITDA_X = D(15000000) - D(5800000)
DEP_X = D(75000000) / 30
PBT_X = EBITDA_X - DEP_X - D(2100000)
check("EX 2.1 EBITDA", EBITDA_X, 9200000)
check("EX 2.1 depreciation", DEP_X, 2500000)
check("EX 2.1 EBIT", EBITDA_X - DEP_X, 6700000)
check("EX 2.1 PBT", PBT_X, 4600000)
check("EX 2.1 tax", PBT_X * D("0.20"), 920000)
check("EX 2.1 net income", PBT_X * D("0.80"), 3680000)
check("EX 2.1 common error (tax on EBIT)", (EBITDA_X - DEP_X) * D("0.20"), 1340000)
check("EX 2.2 operating cash flow", PBT_X * D("0.80") + DEP_X - 1100000 - 200000 + 450000, 5330000)
check("EX 2.3 DSCR before WC", ((EBITDA_X - PBT_X * D("0.20")) / D(4400000)).quantize(D("0.01")), D("1.88"), tol=D("0.005"))
check("EX 2.3 delta working capital", D(1100000) + 200000 - 450000, 850000)
check("EX 2.3 CFADS after WC", EBITDA_X - PBT_X * D("0.20") - 850000, 7430000)
check("EX 2.3 DSCR after WC", ((EBITDA_X - PBT_X * D("0.20") - 850000) / D(4400000)).quantize(D("0.01")), D("1.69"), tol=D("0.005"))
check("EX 2.4 profit difference", D(2400000) - D(2400000) / 8, 2100000)

# ---------- PML-AI Domain 8 — Risk, uncertainty and resilience ----------
section("PML-AI", 8)
import math as _math
AURIGA_RISKS = [("R1", D("0.35"), D(240000), 84000), ("R2", D("0.50"), D(180000), 90000),
                ("R3", D("0.25"), D(320000), 80000), ("R4", D("0.15"), D(400000), 60000),
                ("R5", D("0.30"), D(-120000), -36000)]
emv_total = D(0)
var_total = D(0)
for rid, p, impact, expected in AURIGA_RISKS:
    check(f"WE 8.2.2 EMV {rid}", p * impact, expected)
    emv_total += p * impact
    var_total += p * (1 - p) * impact * impact
check("WE 8.2.2 total exposure", emv_total, 278000)
check("WE 8.2.2 exposure as % of BAC", (emv_total / D(4000000) * 100).quantize(D("0.01")), D("6.95"), tol=D("0.005"))
check("WE 8.2.2 threats-only total", emv_total + 36000, 314000)
check("WE 8.2.4 worst-case sum", D(240000) + 180000 + 320000 + 400000, 1140000)
check("WE 8.2.4 worst case as % of BAC", (D(1140000) / D(4000000) * 100).quantize(D("0.1")), D("28.5"), tol=D("0.05"))
check("WE 8.2.4 variance", var_total, D("63828000000"))
sigma8 = D(str(_math.sqrt(float(var_total))))
check("WE 8.2.4 sigma", sigma8.quantize(D("1")), 252642, tol=D("1"))
check("WE 8.2.4 P80 contingency", (emv_total + D("0.8416") * sigma8).quantize(D("1")), 490624, tol=D("1"))
check("WE 8.2.4 10% rule of thumb", D(4000000) * D("0.10"), 400000)
# Decision tree / value of information
check("WE 8.2.3 proceed-directly EV", D("0.40") * D(300000), 120000)
check("WE 8.2.3 survey-first EV", D(25000) + D("0.40") * D(90000), 61000)
check("WE 8.2.3 value of information", D("0.40") * D(300000) - (D(25000) + D("0.40") * D(90000)), 59000)
check("WE 8.2.3 sensitivity destroys value at 250k", D(25000) + D("0.40") * D(250000), 125000)
# Response economics (ties to Domain 6's fast-track)
check("8.3.1 fast-track EV ties to D6", D(45000) - D("0.20") * D(60000), 33000)
check("MCQ 8.3-A EMV reduction", D(84000) - 20000, 64000)
check("MCQ 8.4-B / 8.A.2 merge bias", D("0.80") * D("0.80"), D("0.64"))
# Exercises
EX8 = [(D("0.40"), D(150000), 60000), (D("0.20"), D(500000), 100000),
       (D("0.60"), D(80000), 48000), (D("0.25"), D(-200000), -50000)]
ex_mean = D(0)
ex_var = D(0)
for p, impact, expected in EX8:
    check(f"EX 8.1 EMV p={p}", p * impact, expected)
    ex_mean += p * impact
    ex_var += p * (1 - p) * impact * impact
check("EX 8.1 total exposure", ex_mean, 158000)
check("EX 8.1 threats-only", ex_mean + 50000, 208000)
check("EX 8.1 overstatement pct", ((D(208000) - D(158000)) / D(158000) * 100).quantize(D("0.1")), D("31.6"), tol=D("0.05"))
check("EX 8.2 variance", ex_var, D("54436000000"))
sigma_ex = D(str(_math.sqrt(float(ex_var))))
check("EX 8.2 sigma", sigma_ex.quantize(D("1")), 233315, tol=D("1"))
check("EX 8.2 P80", (ex_mean + D("0.8416") * sigma_ex).quantize(D("1")), 354358, tol=D("1"))
check("EX 8.3 EMV before", D("0.30") * D(400000), 120000)
check("EX 8.3 EMV after", D("0.10") * D(400000), 40000)
check("EX 8.3 net of mitigation", (D("0.30") - D("0.10")) * D(400000) - 70000, 10000)

# ---------- PML-AI Domain 2 — Strategy, selection and business alignment ----------
section("PML-AI", 2)
R2, POT2, COST2 = D("0.07"), D(979200), D(2400000)   # POT2 is Domain 1's output-based claim
AF8 = af(R2, 8)
check("D2 AF(7%,8)", AF8.quantize(D("0.000001")), D("5.971299"), tol=D("0.0000005"))
flat_pv = POT2 * AF8
check("WE 2.2.2 flat full-potential PV", flat_pv, D("5847096"), tol=D("0.51"))
check("WE 2.2.2 flat NPV", flat_pv - COST2, D("3447096"), tol=D("0.51"))
RAMP2 = [D("0.40"), D("0.60")] + [D("0.70")] * 6
ramp_pv = sum(POT2 * a / (1 + R2) ** (t + 1) for t, a in enumerate(RAMP2))
check("WE 2.2.2 ramped PV", ramp_pv, D("3732898"), tol=D("0.5"))
check("WE 2.2.2 ramped NPV", ramp_pv - COST2, D("1332898"), tol=D("0.5"))
check("WE 2.2.2 overstatement", flat_pv - ramp_pv, D("2114198"), tol=D("0.5"))
check("WE 2.2.2 overstatement as % of honest NPV",
      ((flat_pv - ramp_pv) / (ramp_pv - COST2) * 100).quantize(D("0.1")), D("158.6"), tol=D("0.05"))
check("WE 2.2.2 yr1 benefit", POT2 * D("0.40"), 391680)
check("WE 2.2.2 yr2 benefit", POT2 * D("0.60"), 587520)
check("WE 2.2.2 steady state ties to Domain 1", POT2 * D("0.70"), 685440)
check("WE 2.2.2 breakeven flat adoption %", (COST2 / AF8 / POT2 * 100).quantize(D("0.01")), D("41.05"), tol=D("0.005"))
check("Case A actual benefit at 40% adoption", POT2 * D("0.40"), 391680)
# Weighted scoring
W2 = [D("0.35"), D("0.30"), D("0.20"), D("0.15")]
for name, scores, expected in (("Meridian", [4, 5, 3, 3], "3.95"), ("Beta", [5, 3, 4, 4], "4.05"),
                               ("Gamma", [3, 4, 5, 4], "3.85"), ("Delta", [2, 2, 5, 5], "3.05")):
    check(f"WE 2.2.3 weighted score {name}", sum(w * D(s) for w, s in zip(W2, scores)), D(expected))
# Constrained selection
check("WE 2.2.3 Meridian NPV per capacity unit", (D(1693072) / 3).quantize(D("0.01")), D("564357.33"))
check("WE 2.2.3 Beta NPV per capacity unit", D(1200000) / 2, 600000)
check("WE 2.2.3 Gamma NPV per capacity unit", D(900000) / 1, 900000)
check("WE 2.2.3 Beta+Gamma combined NPV", D(1200000) + 900000, 2100000)
check("WE 2.2.3 combined beats Meridian alone", D(2100000) - D(1693072), 406928)
# Sunk cost
check("WE 2.4.2 forward NPV", D(780000) - D(900000), -120000)
check("EX 2.4 forward NPV", D(1950000) - D(1600000), 350000)
# Exercises
R2X, POTX, COSTX = D("0.08"), D(600000), D(1300000)
AF6 = af(R2X, 6)
check("EX 2.1 AF(8%,6)", AF6.quantize(D("0.000001")), D("4.622880"), tol=D("0.0000005"))
check("EX 2.1 flat PV", POTX * AF6, D("2773728"), tol=D("0.5"))
check("EX 2.1 flat NPV", POTX * AF6 - COSTX, D("1473728"), tol=D("0.5"))
RAMPX = [D("0.30"), D("0.60")] + [D("0.75")] * 4
rampx_pv = sum(POTX * a / (1 + R2X) ** (t + 1) for t, a in enumerate(RAMPX))
check("EX 2.1 ramped PV", rampx_pv, D("1753135"), tol=D("0.5"))
check("EX 2.1 ramped NPV", rampx_pv - COSTX, D("453135"), tol=D("0.5"))
check("EX 2.1 overstatement", POTX * AF6 - rampx_pv, D("1020592"), tol=D("0.5"))
check("EX 2.1 yrs 3-6 factor sum", sum(1 / (1 + R2X) ** t for t in range(3, 7)).quantize(D("0.000001")),
      D("2.839615"), tol=D("0.0000005"))
check("EX 2.2 required annual benefit", COSTX / AF6, D("281210"), tol=D("0.5"))
check("EX 2.2 breakeven adoption %", (COSTX / AF6 / POTX * 100).quantize(D("0.01")), D("46.87"), tol=D("0.005"))
check("EX 2.2 naive undiscounted error %", (COSTX / (POTX * 6) * 100).quantize(D("0.1")), D("36.1"), tol=D("0.05"))
WX = [D("0.40"), D("0.30"), D("0.20"), D("0.10")]
check("EX 2.3 score P", sum(w * D(s) for w, s in zip(WX, [4, 5, 2, 2])), D("3.70"))
check("EX 2.3 score Q", sum(w * D(s) for w, s in zip(WX, [5, 3, 4, 3])), D("4.00"))

# ---------- PFL-AI Domain 10 — Debt sizing, covenants and credit metrics ----------
section("PFL-AI", 10)
CF10 = D(6384000)               # Kestrel documented CFADS, after working capital (Domain 2)
DS10 = D("5009635.23")          # Domain 3's annual instalment on 42,000,000 @ 6% / 12y
DEBT10, EQ10 = D(42000000), D(18000000)
AF6_12, AF6_25 = af(D("0.06"), 12), af(D("0.06"), 25)
check("D10 AF(6%,12)", AF6_12.quantize(D("0.000001")), D("8.383844"), tol=D("0.0000005"))
check("D10 AF(6%,25)", AF6_25.quantize(D("0.000001")), D("12.783356"), tol=D("0.0000005"))
check("10.1.1 CFADS before working capital", CF10 + 600000, 6984000)
check("10.1.1 DSCR on pre-WC CFADS", ((CF10 + 600000) / DS10).quantize(D("0.01")), D("1.39"))
check("10.1.1 DSCR on documented CFADS", (CF10 / DS10).quantize(D("0.01")), D("1.27"))
# WE 10.1.2 — sizing from coverage
MAXDS10 = CF10 / D("1.30")
check("WE 10.1.2 max debt service at 1.30x", MAXDS10, D("4910769.23"), tol=D("0.51"))
check("WE 10.1.2 max debt at 1.30x", MAXDS10 * AF6_12, D("41171123"), tol=D("0.51"))
check("WE 10.1.2 shortfall vs 42m request", DEBT10 - MAXDS10 * AF6_12, D("828877"), tol=D("0.51"))
check("WE 10.1.2 actual DSCR at 42m", (CF10 / DS10).quantize(D("0.0001")), D("1.2743"))
check("Fig 10.1.1 capacity at 1.10x (USD m)",
      (CF10 / D("1.10") * AF6_12 / 1000000).quantize(D("0.01")), D("48.66"))
check("Fig 10.1.1 capacity at 1.30x (USD m)",
      (CF10 / D("1.30") * AF6_12 / 1000000).quantize(D("0.01")), D("41.17"))
# WE 10.2.1 — thresholds, headroom and stress
check("WE 10.2.1 covenant CFADS at 1.20x", DS10 * D("1.20"), D("6011562"), tol=D("0.51"))
check("WE 10.2.1 lock-up CFADS at 1.15x", DS10 * D("1.15"), D("5761081"), tol=D("0.51"))
check("WE 10.2.1 headroom to covenant", CF10 - DS10 * D("1.20"), D("372438"), tol=D("0.51"))
check("WE 10.2.1 headroom as % of CFADS",
      ((CF10 - DS10 * D("1.20")) / CF10 * 100).quantize(D("0.1")), D("5.8"), tol=D("0.05"))
check("WE 10.2.1 stressed CFADS (-20%)", CF10 * D("0.80"), 5107200)
check("WE 10.2.1 stressed DSCR", (CF10 * D("0.80") / DS10).quantize(D("0.0001")), D("1.0195"))
check("WE 10.2.1 stressed cash still exceeds debt service", CF10 * D("0.80") - DS10, D("97564.77"), tol=D("0.51"))
# WE 10.2.2 — LLCR, PLCR and the level-cash identity
LLCR10 = CF10 * AF6_12 / DEBT10
PLCR10 = CF10 * AF6_25 / DEBT10
check("WE 10.2.2 LLCR", LLCR10.quantize(D("0.0001")), D("1.2743"))
check("WE 10.2.2 PLCR", PLCR10.quantize(D("0.0001")), D("1.9431"))
check("WE 10.2.2 identity: LLCR - DSCR = 0 on level cash and annuity service",
      (LLCR10 - CF10 / DS10).quantize(D("0.000001")), D("0"), tol=D("0.0000015"))
check("WE 10.2.2 PLCR exceeds LLCR (tail exists)", 1 if PLCR10 > LLCR10 else 0, 1)
check("10.A.2 tail length (years)", 25 - 12, 13)
# 10.2.3 — ICR and leverage (ties to Domain 2)
check("10.2.3 EBIT-based ICR", (D(5100000) / D(2520000)).quantize(D("0.01")), D("2.02"))
check("10.2.3 EBITDA-based ICR", (D(7500000) / D(2520000)).quantize(D("0.01")), D("2.98"))
check("10.2.3 debt/equity", (DEBT10 / EQ10).quantize(D("0.01")), D("2.33"))
# WE 10.3.2 — DSRA
DSRA10 = DS10 * 6 / 12
check("WE 10.3.2 six-month DSRA", DSRA10, D("2504818"), tol=D("0.51"))
check("WE 10.3.2 absorbable CFADS floor", DS10 - DSRA10, D("2504818"), tol=D("0.51"))
check("WE 10.3.2 floor as % of base CFADS", (DSRA10 / CF10 * 100).quantize(D("1")), D("39"))
check("WE 10.3.2 loss survivable %", (100 - DSRA10 / CF10 * 100).quantize(D("1")), D("61"))
# 10.3.3 — schedule ties to Domain 3
check("10.3.3 year-one interest", DEBT10 * D("0.06"), 2520000)
check("10.3.3 year-one principal", DS10 - DEBT10 * D("0.06"), D("2489635.23"), tol=D("0.51"))
check("10.3.3 interest + principal = debt service", D(2520000) + D("2489635.23"), DS10, tol=D("0.005"))
# MCQ distractors
check("MCQ 10.1-A distractor C (no coverage divisor)", CF10 * AF6_12, D("53522460"), tol=D("0.51"))
check("MCQ 10.1-A distractor D (10-year tenor)", MAXDS10 * af(D("0.06"), 10), D("36143689"), tol=D("0.51"))
check("MCQ 10.1-A AF(6%,10) distractor factor", af(D("0.06"), 10).quantize(D("0.000001")),
      D("7.360087"), tol=D("0.0000005"))
check("MCQ 10.2-A distractor B (inverted ratio)", (DS10 / CF10).quantize(D("0.0001")), D("0.7847"))
check("MCQ 10.2-A distractor C (pre-WC CFADS)", ((CF10 + 600000) / DS10).quantize(D("0.0001")), D("1.3941"))
check("MCQ 10.2-A distractor D (interest only)", (CF10 / D(2520000)).quantize(D("0.0001")), D("2.5333"))
check("MCQ 10.3-A distractor C (three months)", DS10 * 3 / 12, D("1252409"), tol=D("0.51"))
check("MCQ 10.3-A distractor D (one month)", DS10 / 12, D("417470"), tol=D("0.51"))
# Case study A — the four levers
check("CS 10A capacity at 1.25x base case", CF10 / D("1.25") * AF6_12, D("42817968"), tol=D("0.51"))
check("CS 10A bank stressed CFADS (-5%)", CF10 * D("0.95"), 6064800)
check("CS 10A capacity at 1.25x on stressed cash", CF10 * D("0.95") / D("1.25") * AF6_12,
      D("40677069"), tol=D("0.51"))
check("CS 10A lower ratio on lower case is no concession",
      1 if CF10 * D("0.95") / D("1.25") * AF6_12 < MAXDS10 * AF6_12 else 0, 1)
# Case study A closes at the SIZED MAXIMUM, not at a round number below it. An earlier draft closed
# it at 41,000,000, which contradicted Domain 6 — that domain models the structure seven times over
# as "Domain 10's properly sized 41,171,123", and it is right to. One canonical figure now, and the
# residual equity is exactly the gap WE 10.1.2 computed, which is the case study's point.
CS10A_DEBT = (MAXDS10 * AF6_12).quantize(D("1"))
check("CS 10A closes at the sized maximum", CS10A_DEBT, 41171123)
DS10OUT = CS10A_DEBT / AF6_12
check("CS 10A outcome debt service", DS10OUT, D("4910769"), tol=D("0.51"))
check("CS 10A outcome DSCR is exactly the requirement",
      (CF10 / DS10OUT).quantize(D("0.00001")), D("1.30000"), tol=D("0.000005"))
check("CS 10A outcome meets the 1.30x requirement", 1 if CF10 / DS10OUT >= D("1.2999") else 0, 1)
check("CS 10A residual funded as equity is WE 10.1.2's gap", DEBT10 - CS10A_DEBT, 828877)
check("CS 10A revised equity", EQ10 + (DEBT10 - CS10A_DEBT), 18828877)
check("CS 10A revised gearing debt %",
      (CS10A_DEBT / D(60000000) * 100).quantize(D("0.01")), D("68.62"))
check("CS 10A revised gearing equity %",
      ((D(60000000) - CS10A_DEBT) / D(60000000) * 100).quantize(D("0.01")), D("31.38"))
check("CS 10A revised debt/equity",
      (CS10A_DEBT / (D(60000000) - CS10A_DEBT)).quantize(D("0.0001")), D("2.1866"))
check("CS 10A agrees with Domain 6's canonical sized debt", CS10A_DEBT, 41171123)
# Case study B — paid in full and in breach
CFB, DSB = D(19700000), D(18600000)
check("CS 10B DSCR", (CFB / DSB).quantize(D("0.0001")), D("1.0591"))
check("CS 10B CFADS fall from base %", ((1 - CFB / D(24000000)) * 100).quantize(D("0.1")), D("17.9"))
check("CS 10B covenant cash at 1.25x", DSB * D("1.25"), 23250000)
check("CS 10B lock-up cash at 1.15x", DSB * D("1.15"), 21390000)
check("CS 10B locked up (CFADS below lock-up cash)", 1 if CFB < DSB * D("1.15") else 0, 1)
check("CS 10B debt service still paid from cash", 1 if CFB > DSB else 0, 1)
check("CS 10B six-month DSRA", DSB * 6 / 12, 9300000)
check("CS 10B minimum cure to clear 1.25x", DSB * D("1.25") - CFB, 3550000)
check("CS 10B ratio after 3,700,000 cure", ((CFB + 3700000) / DSB).quantize(D("0.0001")), D("1.2581"))
check("CS 10B cure headroom above minimum", D(3700000) - (DSB * D("1.25") - CFB), 150000)
# Exercises 10.1-10.4
CFX, DSCRX = D(9200000), D("1.35")
AF7_10, AF7_20 = af(D("0.07"), 10), af(D("0.07"), 20)
check("EX 10.1 AF(7%,10)", AF7_10.quantize(D("0.000001")), D("7.023582"), tol=D("0.0000005"))
DSX = CFX / DSCRX
check("EX 10.1 max debt service", DSX, D("6814815"), tol=D("0.51"))
check("EX 10.1 max debt", DSX * AF7_10, D("47864408"), tol=D("0.51"))
check("EX 10.1 common error (full CFADS)", CFX * AF7_10, D("64616950"), tol=D("0.51"))
DEBTX = D(47864408)
check("EX 10.2 AF(7%,20)", AF7_20.quantize(D("0.000001")), D("10.594014"), tol=D("0.0000005"))
check("EX 10.2 LLCR", (CFX * AF7_10 / DEBTX).quantize(D("0.0001")), D("1.3500"))
check("EX 10.2 PLCR", (CFX * AF7_20 / DEBTX).quantize(D("0.0001")), D("2.0363"))
check("EX 10.2 DSCR", (CFX / D("6814815")).quantize(D("0.0001")), D("1.3500"))
check("EX 10.2 identity holds", (CFX * AF7_10 / DEBTX - CFX / D("6814815")).quantize(D("0.0001")),
      D("0"), tol=D("0.00005"))
check("EX 10.3 covenant CFADS at 1.20x", D("6814815") * D("1.20"), D("8177778"), tol=D("0.51"))
check("EX 10.3 lock-up CFADS at 1.10x", D("6814815") * D("1.10"), D("7496297"), tol=D("0.51"))
check("EX 10.3 headroom to covenant", CFX - D("6814815") * D("1.20"), D("1022222"), tol=D("0.51"))
check("EX 10.3 headroom as % of base CFADS",
      ((CFX - D("6814815") * D("1.20")) / CFX * 100).quantize(D("0.1")), D("11.1"), tol=D("0.05"))
check("EX 10.4 stressed CFADS (-25%)", CFX * D("0.75"), 6900000)
check("EX 10.4 stressed DSCR", (CFX * D("0.75") / D("6814815")).quantize(D("0.0001")), D("1.0125"))
check("EX 10.4 covenant breached", 1 if CFX * D("0.75") / D("6814815") < D("1.20") else 0, 1)
check("EX 10.4 payment still made from cash", 1 if CFX * D("0.75") > D("6814815") else 0, 1)


# ---------- PML-AI Domain 3 — Governance, organization and decision rights ----------
section("PML-AI", 3)
COD3 = D(14280)                 # Meridian cost of delay per week (Domain 1)


def ew(M, L):                   # governance latency: expected wait for a committee decision
    return D(M) / 2 + D(L)


def ew_sim(M, L, n=200000):     # independent re-derivation by numeric integration over arrivals
    M, L, tot = D(M), D(L), D(0)
    for k in range(n):
        t = D(k) / n * M
        tot += (M - t) if t < M - L else (2 * M - t)
    return tot / n


for _M, _L, _e in ((2, 1, "2.0"), (4, 2, "4.0"), (13, 3, "9.5"), (8, 2, "6.0"), (6, 2, "5.0"),
                   (4, 1, "3.0"), (12, 4, "10.0"), (3, 2, "3.5")):
    check(f"D3 E[wait] M={_M} L={_L}", ew(_M, _L), D(_e))
check("D3 latency formula independently re-derived (M=4,L=2)",
      ew_sim(4, 2).quantize(D("0.001")), D("4.000"), tol=D("0.0005"))
check("D3 latency formula independently re-derived (M=13,L=3)",
      ew_sim(13, 3).quantize(D("0.001")), D("9.500"), tol=D("0.0005"))
check("3.2.3 lever asymmetry: 1 week off L saves 1 week", ew(4, 2) - ew(4, 1), 1)
check("3.2.3 lever asymmetry: 1 week off M saves half a week",
      (ew(4, 2) - ew(3, 2)).quantize(D("0.01")), D("0.50"))
check("3.2.3 Meridian latency cost of one escalated decision", ew(4, 2) * COD3, 57120)
# WE 3.2.2 — committee capacity
check("WE 3.2.2 capacity (13 meetings x 8 items)", 13 * 8, 104)
check("WE 3.2.2 demand at 10k threshold", 36 + 26 + 15, 77)
check("WE 3.2.2 utilisation at 10k", (D(77) / 104 * 100).quantize(D("0.1")), D("74.0"))
check("WE 3.2.2 utilisation with reports pre-read", (D(51) / 104 * 100).quantize(D("0.1")), D("49.0"))
check("WE 3.2.2 demand at 25k threshold", 24 + 26 + 15, 65)
check("WE 3.2.2 utilisation at 25k", (D(65) / 104 * 100).quantize(D("0.1")), D("62.5"))
# WE 3.2.3 — delegation threshold priced
BANDS3 = [24, 12, 15, 7, 2]                     # <=10k, 10-25k, 25-100k, 100-500k, >500k
check("WE 3.2.3 total change requests", sum(BANDS3), 60)
ESC10, ESC25 = sum(BANDS3) - BANDS3[0], sum(BANDS3) - BANDS3[0] - BANDS3[1]
check("WE 3.2.3 escalated at 10k threshold", ESC10, 36)
check("WE 3.2.3 escalated at 25k threshold", ESC25, 24)
PER3 = ew(4, 2) * COD3
check("WE 3.2.3 delay cost per delaying change", PER3, 57120)
check("WE 3.2.3 annual delay cost at 10k", D(ESC10) * D("0.25") * PER3, 514080)
check("WE 3.2.3 annual delay cost at 25k", D(ESC25) * D("0.25") * PER3, 342720)
SAV3 = (D(ESC10) - ESC25) * D("0.25") * PER3
check("WE 3.2.3 annual saving", SAV3, 171360)
check("WE 3.2.3 delegated band value", D(12) * 17000, 204000)
WORST3 = D(12) * 17000 * D("0.40")
check("WE 3.2.3 worst case (all 12 wrong, 40% destroyed)", WORST3, 81600)
check("WE 3.2.3 margin over worst case", SAV3 - WORST3, 89760)
check("WE 3.2.3 breakeven value-destruction rate %",
      (SAV3 / (D(12) * 17000) * 100).quantize(D("0.1")), D("84.0"))
check("WE 3.2.3 breakeven critical-path share %",
      (WORST3 / (D(12) * PER3) * 100).quantize(D("0.1")), D("11.9"), tol=D("0.05"))
# WE 3.3.1 — gate economics
GREV, GWKS = D(45000), D(6)
GDELAY = GWKS * COD3
GREM = D("0.30") * (D("0.80") * D(120000) + D("0.20") * D(900000))
check("WE 3.3.1 gate delay cost", GDELAY, 85680)
check("WE 3.3.1 expected remediation with gate", GREM, 82800)
check("WE 3.3.1 expected cost without gate", D("0.30") * D(900000), 270000)
check("WE 3.3.1 expected cost with gate", GREV + GDELAY + GREM, 213480)
check("WE 3.3.1 net value of the gate", D("0.30") * D(900000) - (GREV + GDELAY + GREM), 56520)
check("WE 3.3.1 breakeven elapsed weeks",
      ((D(270000) - GREV - GREM) / COD3).quantize(D("0.01")), D("9.96"))
check("WE 3.3.1 breakeven detection probability %",
      ((D(900000) - (D(270000) - GREV - GDELAY) / D("0.30")) / D(780000) * 100).quantize(D("0.01")),
      D("55.85"))
# WE 3.3.3 — three-tier escalation
TIERS3 = [(2, 1, "2.0"), (4, 2, "4.0"), (13, 3, "9.5")]
for _M, _L, _e in TIERS3:
    check(f"WE 3.3.3 tier latency M={_M} L={_L}", ew(_M, _L), D(_e))
TOT3 = sum(ew(m, l) for m, l, _ in TIERS3)
check("WE 3.3.3 total escalation latency (weeks)", TOT3, D("15.5"))
check("WE 3.3.3 total escalation cost", TOT3 * COD3, D("221340"))
check("WE 3.3.3 executive committee cost", ew(13, 3) * COD3, D("135660"))
check("WE 3.3.3 executive share of latency %", (ew(13, 3) / TOT3 * 100).quantize(D("1")), D("61"))
check("WE 3.3.3 single-tier cost", ew(4, 2) * COD3, 57120)
check("WE 3.3.3 single-tier saving", TOT3 * COD3 - ew(4, 2) * COD3, D("164220"))
check("WE 3.3.3 single-tier saving %", ((TOT3 - 4) / TOT3 * 100).quantize(D("0.1")), D("74.2"))
check("WE 3.3.3 written-resolution cost", COD3, 14280)
check("WE 3.3.3 written-resolution saving", TOT3 * COD3 - COD3, D("207060"))
check("WE 3.3.3 written-resolution saving %", ((TOT3 - 1) / TOT3 * 100).quantize(D("0.1")), D("93.5"))
# WE 3.3.4 — RACI integrity
check("WE 3.3.4 defective decision classes", 2 + 1, 3)
check("WE 3.3.4 single-A defect rate %", (D(3) / 12 * 100).quantize(D("0.1")), D("25.0"))
# MCQ distractors
check("MCQ 3.2-A E[wait] M=6 L=2", ew(6, 2), 5)
check("MCQ 3.2-A distractor A (half interval only)", D(6) / 2, 3)
check("MCQ 3.2-A distractor D (whole interval + lead)", D(6) + 2, 8)
check("MCQ 3.3-A distractor A (omits delay cost)", D(270000) - (GREV + GREM), D("142200"))
check("MCQ 3.3-A distractor D (omits remediation)", D(270000) - (GREV + GDELAY), D("139320"))
check("MCQ 3.3-C distractor D (half intervals, no lead times)",
      D(2) / 2 + D(4) / 2 + D(13) / 2, D("9.5"))
# Case study A — the four-week month
check("CS 3A latency after paper lead time cut", ew(4, 1), 3)
check("CS 3A latency weeks saved", ew(4, 2) - ew(4, 1), 1)
# Exercises 3.1-3.4
check("EX 3.1 E[wait] M=8 L=2", ew(8, 2), 6)
check("EX 3.1 cost", ew(8, 2) * COD3, 85680)
check("EX 3.1 (a) halve M to 4", ew(4, 2), 4)
check("EX 3.1 (a) saving", (ew(8, 2) - ew(4, 2)) * COD3, 28560)
check("EX 3.1 (b) halve L to 1", ew(8, 1), 5)
check("EX 3.1 (b) saving", (ew(8, 2) - ew(8, 1)) * COD3, 14280)
COD3X = D(9500)
BANDSX = [30, 20, 22, 8]
check("EX 3.2 total requests", sum(BANDSX), 80)
EX, EY = sum(BANDSX) - BANDSX[0], sum(BANDSX) - BANDSX[0] - BANDSX[1]
check("EX 3.2 escalated at 5k", EX, 50)
check("EX 3.2 escalated at 20k", EY, 30)
check("EX 3.2 E[wait]", ew(4, 1), 3)
PERX = ew(4, 1) * COD3X
check("EX 3.2 delay cost per delaying change", PERX, 28500)
check("EX 3.2 delaying changes at 5k", D(EX) * D("0.30"), 15)
check("EX 3.2 delaying changes at 20k", D(EY) * D("0.30"), 9)
check("EX 3.2 annual cost at 5k", D(EX) * D("0.30") * PERX, 427500)
check("EX 3.2 annual cost at 20k", D(EY) * D("0.30") * PERX, 256500)
SAVX = (D(EX) - EY) * D("0.30") * PERX
check("EX 3.2 saving", SAVX, 171000)
check("EX 3.2 delegated band value", D(20) * 11000, 220000)
check("EX 3.2 breakeven value-destruction rate %",
      (SAVX / (D(20) * 11000) * 100).quantize(D("0.1")), D("77.7"), tol=D("0.05"))
GREVX, GDELAYX = D(30000), D(4) * COD3X
GREMX = D("0.25") * (D("0.75") * D(80000) + D("0.25") * D(600000))
check("EX 3.3 delay cost", GDELAYX, 38000)
check("EX 3.3 expected remediation", GREMX, 52500)
check("EX 3.3 cost without gate", D("0.25") * D(600000), 150000)
check("EX 3.3 cost with gate", GREVX + GDELAYX + GREMX, 120500)
check("EX 3.3 net value", D(150000) - (GREVX + GDELAYX + GREMX), 29500)
check("EX 3.3 breakeven elapsed weeks",
      ((D(150000) - GREVX - GREMX) / COD3X).quantize(D("0.001")), D("7.105"))
check("EX 3.3 breakeven detection probability %",
      ((D(600000) - (D(150000) - GREVX - GDELAYX) / D("0.25")) / D(520000) * 100).quantize(D("0.01")),
      D("52.31"))
TIERSX = [(2, 1, "2.0"), (6, 2, "5.0"), (12, 4, "10.0")]
for _M, _L, _e in TIERSX:
    check(f"EX 3.4 tier latency M={_M} L={_L}", ew(_M, _L), D(_e))
TOTX = sum(ew(m, l) for m, l, _ in TIERSX)
check("EX 3.4 total latency (weeks)", TOTX, 17)
check("EX 3.4 total cost", TOTX * COD3X, 161500)
check("EX 3.4 investment committee cost", ew(12, 4) * COD3X, 95000)
check("EX 3.4 written-resolution saving", TOTX * COD3X - COD3X, 152000)
check("EX 3.4 written-resolution saving %", ((TOTX - 1) / TOTX * 100).quantize(D("0.1")), D("94.1"))
check("EX 3.4 portfolio-board-only cost", ew(6, 2) * COD3X, 47500)
check("EX 3.4 portfolio-board-only saving", (TOTX - ew(6, 2)) * COD3X, 114000)
check("EX 3.4 common error (intervals only, no lead times)",
      D(2) / 2 + D(6) / 2 + D(12) / 2, 10)
check("EX 3.4 understatement from omitting lead times", TOTX - 10, 7)


# ---------- PML-AI Domain 4 — Integration and delivery architecture ----------
section("PML-AI", 4)
BASE4 = D(2400000)              # Meridian approved cost baseline (Domain 2)


def mesh(n):                    # possible pairwise interfaces among n components
    return n * (n - 1) // 2


# WE 4.2.1 — the hundred-per-cent rule
KIDS4 = [D(780000), D(536000), D(310000), D(520000), D(186000)]
check("WE 4.2.1 sum of level-2 elements", sum(KIDS4), 2332000)
check("WE 4.2.1 unallocated against parent", BASE4 - sum(KIDS4), 68000)
check("WE 4.2.1 unallocated as % of parent",
      ((BASE4 - sum(KIDS4)) / BASE4 * 100).quantize(D("0.01")), D("2.83"))
OMIT4 = D(214000)               # clinician training and enabling change (Domain 2's omitted column)
check("WE 4.2.1 honest baseline with omission restored", sum(KIDS4) + OMIT4, 2546000)
check("WE 4.2.1 shortfall against approved", sum(KIDS4) + OMIT4 - BASE4, 146000)
check("WE 4.2.1 shortfall as % of approved",
      ((sum(KIDS4) + OMIT4 - BASE4) / BASE4 * 100).quantize(D("0.1")), D("6.1"), tol=D("0.05"))
check("WE 4.2.1 honest NPV (Domain 2 ramped PV less honest cost)",
      D(3732898) - (sum(KIDS4) + OMIT4), 1186898)
check("WE 4.2.1 NPV reduction vs Domain 2", (D(3732898) - BASE4) - (D(3732898) - (sum(KIDS4) + OMIT4)),
      146000)
check("WE 4.2.1 NPV reduction %",
      (D(146000) / (D(3732898) - BASE4) * 100).quantize(D("0.1")), D("11.0"), tol=D("0.05"))
# WE 4.2.3 — interface economics
N4, IC4, LAYER4 = 12, D(18000), D(320000)
check("WE 4.2.3 mesh interface count", mesh(N4), 66)
check("WE 4.2.3 layered interface count", N4, 12)
check("WE 4.2.3 mesh cost", D(mesh(N4)) * IC4, 1188000)
check("WE 4.2.3 layered cost including layer", D(N4) * IC4 + LAYER4, 536000)
check("WE 4.2.3 saving", D(mesh(N4)) * IC4 - (D(N4) * IC4 + LAYER4), 652000)
check("WE 4.2.3 saving %",
      ((D(mesh(N4)) * IC4 - (D(N4) * IC4 + LAYER4)) / (D(mesh(N4)) * IC4) * 100).quantize(D("0.1")),
      D("54.9"), tol=D("0.05"))
check("WE 4.2.3 breakeven layer cost", D(mesh(N4)) * IC4 - D(N4) * IC4, 972000)
check("WE 4.2.3 interfaces avoided", mesh(N4) - N4, 54)
check("WE 4.2.3 marginal 13th component on a mesh", D(mesh(13) - mesh(12)) * IC4, 216000)
check("WE 4.2.3 marginal 13th component layered", IC4, 18000)
check("WE 4.2.3 marginal ratio", D(mesh(13) - mesh(12)), 12)
check("Fig 4.2.1 mesh count at 20 components", mesh(20), 190)
# WE 4.3.2 — configuration audit
CONF4 = {"conforming": 296, "no version": 28, "two current": 11, "wrong version": 5}
check("WE 4.3.2 register total", sum(CONF4.values()), 340)
NONC4 = CONF4["no version"] + CONF4["two current"] + CONF4["wrong version"]
check("WE 4.3.2 non-conforming items", NONC4, 44)
check("WE 4.3.2 defect rate %", (D(NONC4) / 340 * 100).quantize(D("0.01")), D("12.94"))
# WE 4.3.3 — baseline drift by accumulation
N_CH, AVG4, AFF4, WKS4 = 34, D(6800), 14, D("0.3")
DIRECT4 = D(N_CH) * AVG4
DELAY4 = D(AFF4) * WKS4 * COD3
check("WE 4.3.3 direct drift", DIRECT4, 231200)
check("WE 4.3.3 schedule weeks affected", D(AFF4) * WKS4, D("4.2"))
check("WE 4.3.3 schedule drift cost", DELAY4, D("59976"))
check("WE 4.3.3 total drift", DIRECT4 + DELAY4, D("291176"))
check("WE 4.3.3 drift as % of baseline",
      ((DIRECT4 + DELAY4) / BASE4 * 100).quantize(D("0.1")), D("12.1"), tol=D("0.05"))
check("WE 4.3.3 each change as % of baseline", (AVG4 / BASE4 * 100).quantize(D("0.01")), D("0.28"))
check("WE 4.3.3 90-day aggregate", D(N_CH) / 4 * AVG4, D("57800"))
check("WE 4.3.3 90-day aggregate below a 100,000 rule", 1 if D(N_CH) / 4 * AVG4 < 100000 else 0, 1)
check("WE 4.3.3 180-day aggregate", D(N_CH) / 2 * AVG4, 115600)
check("WE 4.3.3 180-day aggregate trips a 100,000 rule", 1 if D(N_CH) / 2 * AVG4 > 100000 else 0, 1)
# WE 4.4.2 — assessed impact versus quoted cost
Q4 = D(40000)
COMP4 = [("direct", Q4), ("schedule", 2 * COD3), ("rework", D(22000)),
         ("interfaces", 3 * D(6000)), ("regression", D(14000)), ("documentation", D(9000))]
TRUE4 = sum(v for _, v in COMP4)
check("WE 4.4.2 schedule component", 2 * COD3, 28560)
check("WE 4.4.2 interface re-verification component", 3 * D(6000), 18000)
check("WE 4.4.2 assessed total impact", TRUE4, 131560)
check("WE 4.4.2 ratio to quoted", (TRUE4 / Q4).quantize(D("0.001")), D("3.289"))
check("WE 4.4.2 quoted as % of true cost", (Q4 / TRUE4 * 100).quantize(D("0.01")), D("30.40"))
check("WE 4.4.2 below-threshold change true cost", D(22000) + 2 * COD3, 50560)
check("WE 4.4.2 below-threshold change exceeds the 25,000 threshold",
      1 if D(22000) + 2 * COD3 > 25000 else 0, 1)
check("4.4.3 change board latency M=2 L=1", ew(2, 1), 2)
# MCQ distractors
check("MCQ 4.2-A distractor C (each pair counted twice)", 12 * 11, 132)
check("MCQ 4.2-A distractor D (n squared)", 12 ** 2, 144)
check("MCQ 4.3-C distractor D (schedule impact on all 34)",
      DIRECT4 + D(N_CH) * WKS4 * COD3, D("376856"))
check("MCQ 4.4-A distractor B (omits schedule impact)", TRUE4 - 2 * COD3, 103000)
check("MCQ 4.4-A distractor D (omits rework)", TRUE4 - D(22000), 109560)
# Case study A — the interface nobody owned
REQ4 = 31
check("CS 4A required interfaces below theoretical maximum", 1 if REQ4 < mesh(N4) else 0, 1)
check("CS 4A required interfaces above the layered promise", 1 if REQ4 > N4 else 0, 1)
check("CS 4A cost of required interfaces", D(REQ4) * IC4, 558000)
check("CS 4A planned interface cost", D(N4) * IC4, 216000)
check("CS 4A unplanned interface effort", D(REQ4) * IC4 - D(N4) * IC4, 342000)
check("CS 4A integration overrun weeks", 11 - 5, 6)
# Case study B — performance against the original baseline
check("CS 4B overrun implied by CPI 0.87 %",
      ((1 / D("0.87") - 1) * 100).quantize(D("0.1")), D("14.9"), tol=D("0.05"))
# Exercises 4.1-4.4
N41, IC41, LAY41 = 9, D(24000), D(200000)
check("EX 4.1 mesh count", mesh(N41), 36)
check("EX 4.1 mesh cost", D(mesh(N41)) * IC41, 864000)
check("EX 4.1 layered cost", D(N41) * IC41 + LAY41, 416000)
check("EX 4.1 saving", D(mesh(N41)) * IC41 - (D(N41) * IC41 + LAY41), 448000)
check("EX 4.1 breakeven layer cost", D(mesh(N41)) * IC41 - D(N41) * IC41, 648000)
check("EX 4.1 marginal 10th on a mesh", D(mesh(10) - mesh(9)) * IC41, 216000)
check("EX 4.1 marginal 10th layered", IC41, 24000)
P42 = D(6500000)
KIDS42 = [D(1900000), D(1250000), D(880000), D(1640000), D(410000)]
check("EX 4.2 children sum", sum(KIDS42), 6080000)
check("EX 4.2 unallocated", P42 - sum(KIDS42), 420000)
check("EX 4.2 unallocated %", ((P42 - sum(KIDS42)) / P42 * 100).quantize(D("0.01")), D("6.46"))
check("EX 4.2 honest total with omission", sum(KIDS42) + 690000, 6770000)
check("EX 4.2 shortfall", D(690000) - (P42 - sum(KIDS42)), 270000)
check("EX 4.2 shortfall %",
      ((D(690000) - (P42 - sum(KIDS42))) / P42 * 100).quantize(D("0.01")), D("4.15"))
Q43, C43 = D(75000), D(9500)
COMP43 = [Q43, 3 * C43, D(34000), 4 * D(7500), D(19000), D(11000)]
check("EX 4.3 schedule component", 3 * C43, 28500)
check("EX 4.3 interface component", 4 * D(7500), 30000)
check("EX 4.3 assessed total", sum(COMP43), 197500)
check("EX 4.3 ratio to quoted", (sum(COMP43) / Q43).quantize(D("0.001")), D("2.633"))
check("EX 4.3 quoted as % of true", (Q43 / sum(COMP43) * 100).quantize(D("0.1")), D("38.0"))
N44, AVG44, AFF44, WKS44, B44 = 48, D(5200), 18, D("0.25"), D(3200000)
check("EX 4.4 direct drift", D(N44) * AVG44, 249600)
check("EX 4.4 schedule weeks", D(AFF44) * WKS44, D("4.5"))
check("EX 4.4 schedule drift cost", D(AFF44) * WKS44 * C43, D("42750"))
check("EX 4.4 total drift", D(N44) * AVG44 + D(AFF44) * WKS44 * C43, D("292350"))
check("EX 4.4 drift as % of baseline",
      ((D(N44) * AVG44 + D(AFF44) * WKS44 * C43) / B44 * 100).quantize(D("0.01")), D("9.14"))
check("EX 4.4 each change as % of baseline", (AVG44 / B44 * 100).quantize(D("0.0001")), D("0.1625"))
check("EX 4.4 90-day aggregate", D(N44) / 4 * AVG44, 62400)
check("EX 4.4 a 100,000 rule would not trip", 1 if D(N44) / 4 * AVG44 < 100000 else 0, 1)


# ---------- Per-domain check modules (parallel-safe contribution point) ----------
# Domains authored concurrently must not all edit this file. Each contributes
# _build/checks/<book>_d<NN>.py exposing run(ctx); ctx carries the check helper, Decimal, the
# shared rate/annuity helpers and the master-thread constants, so a new domain adds a FILE and
# never a merge conflict. Modules run in sorted order and a missing directory is not an error.
CTX = {
    "check": check, "D": D, "af": af, "ew": ew, "mesh": mesh,
    # PML-AI master threads
    "MERIDIAN_COST_OF_DELAY": COD3, "MERIDIAN_BASELINE": BASE4,
    "MERIDIAN_BENEFIT_70PCT": D(685440), "MERIDIAN_BENEFIT_FULL": D(979200),
    "AURIGA_BAC": D(4000000), "AURIGA_WEEKS": 25,
    # PFL-AI master thread (Kestrel Water SPC)
    "KESTREL_DEBT": D(42000000), "KESTREL_EQUITY": D(18000000), "KESTREL_CAPEX": D(60000000),
    "KESTREL_RATE": D("0.06"), "KESTREL_TENOR": 12, "KESTREL_LIFE": 25,
    "KESTREL_INSTALMENT": D("5009635.23"), "KESTREL_CFADS": D(6384000),
    "KESTREL_EBITDA": D(7500000), "KESTREL_EBIT": D(5100000), "KESTREL_INTEREST_Y1": D(2520000),
    "KESTREL_DISCOUNT": D("0.08"), "KESTREL_NPV": D(16179360),
}
_checks_dir = pathlib.Path(__file__).resolve().parent / "checks"
_modules = sorted(_checks_dir.glob("*.py")) if _checks_dir.is_dir() else []
for _mod_path in _modules:
    if _mod_path.name.startswith("_"):
        continue
    # Attribute the module's assertions to ITS domain, not to whatever section ran last — otherwise
    # every module's checks land on the final domain of this file, which is how PML-AI Domain 4 came
    # to be credited with 6,595 of them.
    # Any `_suffix` is accepted, not just `_ext`, so two authors working the same domain each add
    # their own file (`pfl_d05_mcq.py`, `pfl_d05_cases.py`) instead of contending for one.
    _mm = re.match(r"(pml|pfl)_d(\d+)(_[a-z0-9_]+)?\.py$", _mod_path.name)
    section(("PML-AI" if _mm.group(1) == "pml" else "PFL-AI"), int(_mm.group(2))) if _mm \
        else section(None, None)
    _spec = importlib.util.spec_from_file_location(f"pci_checks_{_mod_path.stem}", _mod_path)
    _mod = importlib.util.module_from_spec(_spec)
    try:
        _spec.loader.exec_module(_mod)
        _mod.run(CTX)
    except Exception as _e:                       # a broken module is a failure, not a skip
        FAILURES.append(f"{_mod_path.name}: {type(_e).__name__}: {_e}")
        print(f"FAIL  {_mod_path.name} raised {type(_e).__name__}: {_e}")
print(f"\ncheck modules loaded: {len(_modules)}")


# Machine-readable attribution for the derived appendices. One line, easy to parse, and it cannot
# disagree with the assertions above because it is produced by them.
print("\nTALLY " + ";".join(f"{b}:{d}={n}" for (b, d), n in sorted(TALLY.items())))

print()
if FAILURES:
    print(f"✗ {len(FAILURES)} FAILURES:", *FAILURES, sep="\n  ")
    raise SystemExit(1)
print("✓ all golden answers verified")
