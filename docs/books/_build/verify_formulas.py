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
check("4.A.3 EAV invariant", ((D(8900000) * AF15 - 60000000) / AF15 * AF15), D(8900000) * AF15 - 60000000, tol=D("0.01"))

# ---------- PFL-AI Domain 1 — Foundations ----------
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

print()
if FAILURES:
    print(f"✗ {len(FAILURES)} FAILURES:", *FAILURES, sep="\n  ")
    raise SystemExit(1)
print("✓ all golden answers verified")
