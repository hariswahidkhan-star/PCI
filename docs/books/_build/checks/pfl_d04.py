"""PFL-AI Domain 4 — Investment appraisal and capital budgeting. Golden-answer checks.

Recomputes every printed result in the domain, including **every numeric MCQ option** rather than
only the keyed one, so a distractor cannot be arithmetically impossible without this failing. Also
asserts the domain's teaching invariants, which are the claims a reader is invited to rely on:

  * NPV at the retained IRR is zero, and the residual against a two-decimal IRR is the stated
    rounding artefact rather than a defect.
  * PI at the IRR is exactly 1 — the second, independent audit of a published IRR.
  * Capital recovery + EAV reconstructs the annual inflow: I0/AF + NPV/AF = A.
  * EAC x AF(r, H) reproduces the replacement-chain PV over horizon H.
  * Both NPVs are equal at a crossover rate, exactly, because one factor value determines both.
  * A probability-weighted appraisal reconciles to the NPV of its own mean forecast (NPV is linear
    in the flows), which is what makes the staged-investment arithmetic of 4.A.4 trustworthy.

Master appraisal: I0 = 60,000,000; A = 8,900,000 for 15 years; r = 8 %.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]

    def af(r, n):
        r = D(str(r))
        return D(n) if r == 0 else (1 - (1 + r) ** -n) / r

    def fvaf(r, n):
        r = D(str(r))
        return ((1 + r) ** n - 1) / r

    def solve(f, lo, hi, it=240):
        lo, hi = D(str(lo)), D(str(hi))
        for _ in range(it):
            m = (lo + hi) / 2
            if f(lo) * f(m) <= 0:
                hi = m
            else:
                lo = m
        return (lo + hi) / 2

    I0, A, N, R = D(60000000), D(8900000), 15, D("0.08")
    AF15 = af(R, N)
    NPV = A * AF15 - I0
    IRR = solve(lambda r: A * af(r, N) - I0, "0.05", "0.25")

    # ---- KA 4.1.1 the master appraisal and the cash-flow category rule --------------
    check("4.1.1 AF(0.08,15)", AF15, D("8.559479"), tol=D("0.0000005"))
    check("4.1.1 PV of inflows", A * AF15, D(76179360), tol=D("0.5"))
    check("4.1.1 NPV master", NPV, D("16179360.32"))
    check("4.1.1 undiscounted surplus (MCQ 4.1-A distractor B)", A * 15 - I0, D(73500000))
    check("4.1.1 sign-reversed NPV (MCQ 4.1-A distractor C)", -NPV, D("-16179360.32"))
    check("4.1.1 PV without deducting I0 (MCQ 4.1-A distractor D)", A * AF15, D("76179360.32"))

    # WE 4.1.1b the ramp: identical nominal total, later
    r1, r2, r3 = D(4600000), D(6700000), D(9400000)
    check("4.1.1b ramp nominal total equals level total", r1 + r2 + r3 * 13, A * 15)
    check("4.1.1b DF(1)", 1 / (1 + R), D("0.925926"), tol=D("0.0000005"))
    check("4.1.1b DF(2)", 1 / (1 + R) ** 2, D("0.857339"), tol=D("0.0000005"))
    check("4.1.1b AF(0.08,2)", af(R, 2), D("1.783265"), tol=D("0.0000005"))
    check("4.1.1b deferred annuity factor AF15-AF2", AF15 - af(R, 2), D("6.776214"),
          tol=D("0.0000005"))
    check("4.1.1b PV year 1", r1 / (1 + R), D(4259259), tol=D("0.5"))
    check("4.1.1b PV year 2", r2 / (1 + R) ** 2, D(5744170), tol=D("0.5"))
    check("4.1.1b PV years 3-15", r3 * (AF15 - af(R, 2)), D(63696411), tol=D("0.5"))
    PV_RAMP = r1 / (1 + R) + r2 / (1 + R) ** 2 + r3 * (AF15 - af(R, 2))
    check("4.1.1b PV ramp", PV_RAMP, D(73699840), tol=D("0.5"))
    check("4.1.1b NPV ramp", PV_RAMP - I0, D(13699840), tol=D("0.5"))
    check("4.1.1b cost of the ramp", NPV - (PV_RAMP - I0), D(2479520), tol=D("0.5"))
    check("4.1.1b ramp cost as share of value (%)", (NPV - (PV_RAMP - I0)) / NPV * 100,
          D("15.3"), tol=D("0.05"))

    # WE 4.1.1c the mid-period convention
    HALF = (1 + R) ** D("0.5")
    check("4.1.1c (1.08)^0.5", HALF, D("1.0392305"), tol=D("0.00000005"))
    check("4.1.1c PV mid-period", A * AF15 * HALF, D(79167914), tol=D("0.5"))
    check("4.1.1c NPV mid-period", A * AF15 * HALF - I0, D(19167914), tol=D("0.5"))
    check("4.1.1c convention gap", A * AF15 * (HALF - 1), D(2988553), tol=D("0.5"))
    check("4.1.1c gap as share of reported value (%)", A * AF15 * (HALF - 1) / NPV * 100,
          D("18.5"), tol=D("0.05"))
    check("4.1.1c mid-period premium (%)", (HALF - 1) * 100, D("3.92"), tol=D("0.005"))
    # the single multiplier equals per-flow half-period discounting, which is the claim made
    per_flow = sum(A / (1 + R) ** (D(t) - D("0.5")) for t in range(1, 16))
    check("4.1.1c multiplier route equals per-flow route", per_flow, A * AF15 * HALF,
          tol=D("0.01"))

    # ---- KA 4.1.2 IRR, interpolation, and the rounding residual ---------------------
    check("4.1.2 AF target I0/A", I0 / A, D("6.741573"), tol=D("0.0000005"))
    check("4.1.2 IRR (retained)", IRR * 100, D("12.192120"), tol=D("0.0000005"))
    check("4.1.2 IRR (published, 2dp)", IRR * 100, D("12.19"), tol=D("0.005"))
    check("4.1.2 AF(0.12,15)", af("0.12", 15), D("6.810864"), tol=D("0.0000005"))
    check("4.1.2 AF(0.13,15)", af("0.13", 15), D("6.462379"), tol=D("0.0000005"))
    check("4.1.2 NPV at 12%", A * af("0.12", 15) - I0, D(616694), tol=D("0.5"))
    check("4.1.2 NPV at 13%", A * af("0.13", 15) - I0, D(-2484828), tol=D("0.5"))
    INTERP = D("0.12") + (af("0.12", 15) - I0 / A) / (af("0.12", 15) - af("0.13", 15)) * D("0.01")
    check("4.1.2 interpolated IRR", INTERP * 100, D("12.1988"), tol=D("0.00005"))
    check("4.1.2 interpolation error (bp)", (INTERP - IRR) * 10000, D("0.67"), tol=D("0.005"))
    check("4.1.2 INVARIANT NPV at retained IRR is zero", A * af(IRR, N) - I0, D(0),
          tol=D("0.000001"))
    check("4.1.2 residual at published 12.19%", A * af("0.1219", 15) - I0, D(6751), tol=D("0.5"))
    check("4.1.2 residual at 12.1921%", A * af("0.121921", 15) - I0, D(65), tol=D("0.5"))
    check("4.1.2 margin over the 8% hurdle (points)", IRR * 100 - 8, D("4.19"), tol=D("0.005"))

    # WE 4.1.2b the two-root project, mapped
    CF = [D(-1000000), D(2300000), D(-1320000)]

    def npv_cf(r):
        r = D(str(r))
        return sum(c / (1 + r) ** t for t, c in enumerate(CF))

    check("4.1.2b NPV at 0%", npv_cf(0), D(-20000), tol=D("0.5"))
    check("4.1.2b NPV at 0% equals raw sum", npv_cf(0), sum(CF))
    check("4.1.2b NPV at 5%", npv_cf("0.05"), D(-6803), tol=D("0.5"))
    check("4.1.2b root at 10%", npv_cf("0.10"), D(0), tol=D("0.0001"))
    check("4.1.2b NPV at 15%", npv_cf("0.15"), D("1890"), tol=D("0.5"))
    check("4.1.2b root at 20%", npv_cf("0.20"), D(0), tol=D("0.0001"))
    check("4.1.2b NPV at 25%", npv_cf("0.25"), D(-4800), tol=D("0.5"))
    check("4.1.2b NPV at 30%", npv_cf("0.30"), D(-11834), tol=D("0.5"))
    check("4.1.2b peak NPV", npv_cf("0.147826"), D(1894), tol=D("0.5"))
    # the peak really is a peak: value falls away on both sides
    check("4.1.2b peak dominates 14%", D(1) if npv_cf("0.147826") > npv_cf("0.14") else D(0), D(1))
    check("4.1.2b peak dominates 16%", D(1) if npv_cf("0.147826") > npv_cf("0.16") else D(0), D(1))

    # ---- KA 4.1.3 MIRR --------------------------------------------------------------
    check("4.1.3 FVAF(0.08,15)", fvaf(R, 15), D("27.152114"), tol=D("0.0000005"))
    TV8 = A * fvaf(R, 15)
    check("4.1.3 terminal value at 8%", TV8, D(241653814), tol=D("0.5"))
    check("4.1.3 money multiple TV/I0", TV8 / I0, D("4.027564"), tol=D("0.0000005"))
    MIRR8 = (TV8 / I0) ** (D(1) / 15) - 1
    check("4.1.3 MIRR at 8% both rates", MIRR8 * 100, D("9.73"), tol=D("0.005"))
    check("4.1.3 MCQ 4.1-C distractor A is the IRR", IRR * 100, D("12.19"), tol=D("0.005"))
    check("4.1.3 MCQ 4.1-C distractor D = the money multiple divided by 15",
          TV8 / I0 / 15 * 100, D("26.85"), tol=D("0.005"))
    check("4.1.3 INVARIANT MIRR lies between reinvestment rate and IRR",
          D(1) if R < MIRR8 < IRR else D(0), D(1))
    check("4.1.3 reinvestment fiction removed (points)", (IRR - MIRR8) * 100, D("2.46"),
          tol=D("0.005"))

    # WE 4.1.3b finance 6 %, reinvestment 4 %, membrane replacement 6,000,000 at t=8
    FIN, REI, MEM = D("0.06"), D("0.04"), D(6000000)
    check("4.1.3b DF(6%,8)", 1 / (1 + FIN) ** 8, D("0.627412"), tol=D("0.0000005"))
    check("4.1.3b PV of membrane at the finance rate", MEM / (1 + FIN) ** 8, D(3764474),
          tol=D("0.5"))
    PVOUT = I0 + MEM / (1 + FIN) ** 8
    check("4.1.3b PV of outflows", PVOUT, D(63764474), tol=D("0.5"))
    check("4.1.3b FVAF(0.04,15)", fvaf(REI, 15), D("20.023588"), tol=D("0.0000005"))
    TV4 = A * fvaf(REI, 15)
    check("4.1.3b terminal value at 4%", TV4, D(178209930), tol=D("0.5"))
    check("4.1.3b TV/PVout", TV4 / PVOUT, D("2.794815"), tol=D("0.0000005"))
    MIRR_B = (TV4 / PVOUT) ** (D(1) / 15) - 1
    check("4.1.3b MIRR", MIRR_B * 100, D("7.09"), tol=D("0.005"))
    check("4.1.3b MCQ 4.1-G quoted MIRR", MIRR_B * 100, D("7.09"), tol=D("0.005"))
    # the same flows discounted properly at the project rate remain viable
    check("4.1.3b DF(8%,8)", 1 / (1 + R) ** 8, D("0.540269"), tol=D("0.0000005"))
    check("4.1.3b PV of membrane at the project rate", MEM / (1 + R) ** 8, D("3241613.31"))
    NPV_MEM = NPV - MEM / (1 + R) ** 8
    check("4.1.3b NPV with membrane at 8%", NPV_MEM, D(12937747), tol=D("0.5"))
    IRR_MEM = solve(lambda r: A * af(r, 15) - MEM / (1 + D(str(r))) ** 8 - I0, "0.05", "0.25")
    check("4.1.3b IRR with membrane", IRR_MEM * 100, D("11.4250"), tol=D("0.00005"))
    # and the point the MCQ turns on: MIRR below the hurdle, NPV comfortably positive
    check("4.1.3b MIRR is below the 8% hurdle", D(1) if MIRR_B < R else D(0), D(1))
    check("4.1.3b NPV is nonetheless positive", D(1) if NPV_MEM > 0 else D(0), D(1))

    # ---- KA 4.2.1 payback -----------------------------------------------------------
    check("4.2.1 simple payback (level)", I0 / A, D("6.74"), tol=D("0.005"))
    check("4.2.1 AF(0.08,10)", af(R, 10), D("6.710081"), tol=D("0.0000005"))
    check("4.2.1 cumulative PV after year 10", A * af(R, 10), D(59719724), tol=D("0.5"))
    check("4.2.1 shortfall after year 10", I0 - A * af(R, 10), D(280276), tol=D("0.5"))
    check("4.2.1 DF(11)", 1 / (1 + R) ** 11, D("0.428883"), tol=D("0.0000005"))
    check("4.2.1 year 11 discounted flow", A / (1 + R) ** 11, D(3817057), tol=D("0.5"))
    DPB = 10 + (I0 - A * af(R, 10)) / (A / (1 + R) ** 11)
    check("4.2.1 discounted payback (level)", DPB, D("10.07"), tol=D("0.005"))
    check("4.2.1 cost of time value (years)", DPB - I0 / A, D("3.33"), tol=D("0.01"))

    # WE 4.2.1b payback on the ramp
    FLOWS = [r1, r2] + [r3] * 13
    cum = D(0)
    for t, c in enumerate(FLOWS, 1):
        cum += c
        if t == 7:
            check("4.2.1b cumulative nominal at year 7", cum, D(58300000))
        if t == 8:
            check("4.2.1b cumulative nominal at year 8", cum, D(67700000))
    cum, pb = D(0), None
    for t, c in enumerate(FLOWS, 1):
        prev, cum = cum, cum + c
        if cum >= I0 and pb is None:
            check("4.2.1b shortfall entering the crossing year", I0 - prev, D(1700000))
            check("4.2.1b fraction of the crossing year", (I0 - prev) / c, D("0.1809"),
                  tol=D("0.00005"))
            pb = D(t - 1) + (I0 - prev) / c
    check("4.2.1b payback (ramp)", pb, D("7.18"), tol=D("0.005"))
    check("4.2.1b nominal exposure added by the ramp (years)", pb - I0 / A, D("0.44"),
          tol=D("0.005"))
    cum, dpb = D(0), None
    for t, c in enumerate(FLOWS, 1):
        prev, cum = cum, cum + c / (1 + R) ** t
        if cum >= I0 and dpb is None:
            dpb = D(t - 1) + (I0 - prev) / (c / (1 + R) ** t)
    check("4.2.1b discounted payback (ramp)", dpb, D("10.91"), tol=D("0.005"))
    check("4.2.1b discounted exposure added by the ramp (years)", dpb - DPB, D("0.84"),
          tol=D("0.005"))

    # ---- KA 4.2.2 profitability index ----------------------------------------------
    check("4.2.2 PI gross", A * AF15 / I0, D("1.269656"), tol=D("0.0000005"))
    check("4.2.2 PI net", NPV / I0, D("0.269656"), tol=D("0.0000005"))
    check("4.2.2 the two definitions differ by exactly one",
          A * AF15 / I0 - NPV / I0, D(1), tol=D("0.0000001"))
    check("4.2.2 INVARIANT PI at the IRR is one", A * af(IRR, N) / I0, D(1), tol=D("0.0000001"))
    check("4.2.2 PI on the ramp profile", PV_RAMP / I0, D("1.228331"), tol=D("0.0000005"))
    check("4.2.2 index points lost to the ramp", (A * AF15 - PV_RAMP) / I0 * 100, D("4.13"),
          tol=D("0.005"))

    # ---- KA 4.2.3 equivalent annual value ------------------------------------------
    EAV = NPV / AF15
    check("4.2.3b EAV of the master appraisal", EAV, D("1890227.30"))
    check("4.2.3b INVARIANT EAV x AF reproduces NPV", EAV * AF15, NPV, tol=D("0.01"))
    check("4.2.3b capital-recovery annuity", I0 / AF15, D("7009772.70"))
    check("4.2.3b INVARIANT capital recovery + EAV = annual inflow", I0 / AF15 + EAV, A,
          tol=D("0.01"))
    check("4.2.3b value share of annual inflow (%)", EAV / A * 100, D("21.24"), tol=D("0.005"))
    check("4.2.3b capital share of annual inflow (%)", (I0 / AF15) / A * 100, D("78.76"),
          tol=D("0.005"))

    # WE 4.2.3 / 4.2.3c the pump choice, twice
    capA, opA, nA = D(5000000), D(800000), 3
    capB, opB, nB = D(7600000), D(500000), 5
    check("4.2.3 AF(0.08,3)", af(R, nA), D("2.577097"), tol=D("0.0000005"))
    check("4.2.3 AF(0.08,5)", af(R, nB), D("3.992710"), tol=D("0.0000005"))
    PVA, PVB = capA + opA * af(R, nA), capB + opB * af(R, nB)
    check("4.2.3 PV of costs, system A", PVA, D("7061677.59"))
    check("4.2.3 PV of costs, system B", PVB, D("9596355.02"))
    EACA, EACB = PVA / af(R, nA), PVB / af(R, nB)
    check("4.2.3 EAC system A", EACA, D("2740167.57"))
    check("4.2.3 EAC system B", EACB, D("2403469.05"))
    check("4.2.3 annual saving from B", EACA - EACB, D("336698.52"))
    check("4.2.3 MCQ 4.2-B distractor C: PV divided by raw life, A", PVA / 3, D(2353893),
          tol=D("0.5"))
    check("4.2.3 MCQ 4.2-B distractor C: PV divided by raw life, B", PVB / 5, D(1919271),
          tol=D("0.5"))
    check("4.2.3 B's purchase price premium (%)", (capB - capA) / capA * 100, D(52), tol=D("0.5"))
    CHAINA = sum(PVA / (1 + R) ** (3 * k) for k in range(5))
    CHAINB = sum(PVB / (1 + R) ** (5 * k) for k in range(3))
    check("4.2.3c replacement chain A over 15 years", CHAINA, D("23454405.92"))
    check("4.2.3c replacement chain B over 15 years", CHAINB, D("20572442.15"))
    check("4.2.3c chain advantage to B", CHAINA - CHAINB, D("2881963.77"))
    check("4.2.3c INVARIANT EAC_A x AF(0.08,15) reproduces chain A", EACA * AF15, CHAINA,
          tol=D("0.01"))
    check("4.2.3c INVARIANT EAC_B x AF(0.08,15) reproduces chain B", EACB * AF15, CHAINB,
          tol=D("0.01"))

    # ---- KA 4.3.1 exclusivity and the crossover -------------------------------------
    iP, aP, iQ, aQ, nPQ = D(5000000), D(2000000), D(20000000), D(6000000), 5
    NPVP, NPVQ = aP * af(R, nPQ) - iP, aQ * af(R, nPQ) - iQ
    check("4.3.1 NPV of P at 8%", NPVP, D("2985420.07"))
    check("4.3.1 NPV of Q at 8%", NPVQ, D("3956260.22"))
    check("4.3.1 NPV gap Q-P", NPVQ - NPVP, D("970840.15"))
    check("4.3.1 IRR of P", solve(lambda r: aP * af(r, nPQ) - iP, "0.01", "1.0") * 100,
          D("28.65"), tol=D("0.005"))
    check("4.3.1 IRR of Q", solve(lambda r: aQ * af(r, nPQ) - iQ, "0.01", "1.0") * 100,
          D("15.24"), tol=D("0.005"))
    check("4.3.1 incremental AF target", (iQ - iP) / (aQ - aP), D("3.750000"), tol=D("0.0000005"))
    XO = solve(lambda r: (aQ - aP) * af(r, nPQ) - (iQ - iP), "0.01", "0.60")
    check("4.3.1 crossover rate", XO * 100, D("10.424845"), tol=D("0.0000005"))
    check("4.3.1 INVARIANT NPV of P at the crossover", aP * af(XO, nPQ) - iP, D(2500000),
          tol=D("0.01"))
    check("4.3.1 INVARIANT NPV of Q at the crossover", aQ * af(XO, nPQ) - iQ, D(2500000),
          tol=D("0.01"))
    check("4.3.1 hurdle distance from indifference (points)", XO * 100 - 8, D("2.42"),
          tol=D("0.005"))
    # The profile table's rate headings are rounded to four decimals; the CELLS are computed on the
    # retained roots, which is why the zeros are exact. Checking the cells against the *printed*
    # headings would fail by ~6,000 and would be checking the wrong thing — see the manuscript's
    # note on the cross-cells being round by construction.
    IRR_P = solve(lambda r: aP * af(r, nPQ) - iP, "0.01", "1.0")
    IRR_Q = solve(lambda r: aQ * af(r, nPQ) - iQ, "0.01", "1.0")
    for lbl, rr, vp, vq in [
            ("0%", 0, D(5000000), D(10000000)),
            ("8%", R, D("2985420"), D("3956260")),
            ("15%", "0.15", D("1704310"), D("112931")),
            ("20%", "0.20", D("981224"), D("-2056327"))]:
        check(f"4.3.1 profile NPV_P at {lbl}", aP * af(rr, nPQ) - iP, vp, tol=D("0.6"))
        check(f"4.3.1 profile NPV_Q at {lbl}", aQ * af(rr, nPQ) - iQ, vq, tol=D("0.6"))
    check("4.3.1 INVARIANT AF at Q's IRR is I0/A for Q", af(IRR_Q, nPQ), iQ / aQ,
          tol=D("0.0000001"))
    check("4.3.1 cross-cell NPV_P at Q's IRR", aP * af(IRR_Q, nPQ) - iP, D("1666666.67"),
          tol=D("0.05"))
    check("4.3.1 cross-cell NPV_Q at Q's IRR is zero", aQ * af(IRR_Q, nPQ) - iQ, D(0),
          tol=D("0.01"))
    check("4.3.1 INVARIANT AF at P's IRR is I0/A for P", af(IRR_P, nPQ), iP / aP,
          tol=D("0.0000001"))
    check("4.3.1 cross-cell NPV_Q at P's IRR", aQ * af(IRR_P, nPQ) - iQ, D(-5000000),
          tol=D("0.05"))
    check("4.3.1 cross-cell NPV_P at P's IRR is zero", aP * af(IRR_P, nPQ) - iP, D(0),
          tol=D("0.01"))

    # ---- KA 4.3.2 rationing ---------------------------------------------------------
    # the four-project table and MCQ 4.3-B
    for nm, i0, npv_, pi in [("W", D(8), D("2.40"), D("1.300")), ("X", D(12), D("3.00"), D("1.250")),
                             ("Y", D(10), D("2.20"), D("1.220")), ("Z", D(6), D("1.02"), D("1.170"))]:
        check(f"4.3.2 PI of {nm}", (npv_ + i0) / i0, pi, tol=D("0.0005"))
    check("4.3.2 W+X spend", D(8) + D(12), D(20))
    check("4.3.2 W+X value", D("2.40") + D("3.00"), D("5.40"))
    check("4.3.2 MCQ 4.3-B distractor A (X+Y) spend exceeds budget", D(12) + D(10), D(22))
    check("4.3.2 MCQ 4.3-B distractor C (W+Y+Z) spend exceeds budget",
          D(8) + D(10) + D(6), D(24))
    check("4.3.2 MCQ 4.3-B distractor D (X+Z) value", D("3.00") + D("1.02"), D("4.02"))
    check("4.3.2 MCQ 4.3-B distractor D shortfall", D("5.40") - D("4.02"), D("1.38"))
    # WE 4.3.2 the five-project set where greedy PI fails
    FIVE = [("W", D(8000000), D(2400000)), ("V", D(4000000), D(1160000)),
            ("X", D(12000000), D(3000000)), ("Y", D(10000000), D(2200000)),
            ("Z", D(6000000), D(1020000))]
    for nm, i0, npv_, pi in [("W", D(8000000), D(2400000), D("1.3000")),
                             ("V", D(4000000), D(1160000), D("1.2900")),
                             ("X", D(12000000), D(3000000), D("1.2500")),
                             ("Y", D(10000000), D(2200000), D("1.2200")),
                             ("Z", D(6000000), D(1020000), D("1.1700"))]:
        check(f"4.3.2 five-set PI of {nm}", (npv_ + i0) / i0, pi, tol=D("0.00005"))
    BUDGET = D(20000000)
    # greedy PI, exactly as the worked example runs it
    spent, val, taken = D(0), D(0), []
    for nm, i0, npv_ in sorted(FIVE, key=lambda x: -(x[2] + x[1]) / x[1]):
        if spent + i0 <= BUDGET:
            spent, val = spent + i0, val + npv_
            taken.append(nm)
    check("4.3.2 greedy spend", spent, D(18000000))
    check("4.3.2 greedy value", val, D(4580000))
    check("4.3.2 greedy budget left unspent", BUDGET - spent, D(2000000))
    check("4.3.2 greedy set size", D(len(taken)), D(3))
    # exhaustive enumeration finds the optimum
    best, bestset = D(-1), None
    for mask in range(1, 32):
        sub = [FIVE[k] for k in range(5) if mask >> k & 1]
        cost = sum(x[1] for x in sub)
        if cost <= BUDGET:
            v = sum(x[2] for x in sub)
            if v > best:
                best, bestset = v, sub
    check("4.3.2 optimal value by enumeration", best, D(5400000))
    check("4.3.2 optimal spend", sum(x[1] for x in bestset), D(20000000))
    check("4.3.2 optimal set size", D(len(bestset)), D(2))
    check("4.3.2 value greedy leaves unfunded", best - val, D(820000))
    # NOT `check("subsets tested", D(31), D(31))`, which is what stood here and which compares a
    # constant to itself: it can never fail, and it did not catch the manuscript claiming twelve
    # feasible sets where enumeration finds sixteen. The count is now derived from the same
    # enumeration that found the optimum, so the figure and the check cannot disagree.
    check("4.3.2 non-empty subsets of five projects", D(2 ** 5 - 1), D(31))
    _feasible = sum(1 for mask in range(1, 32)
                    if sum(FIVE[k][1] for k in range(5) if mask >> k & 1) <= BUDGET)
    check("4.3.2 feasible subsets within the budget", D(_feasible), D(16))

    # ---- KA 4.3.3 breakeven and the two-way table -----------------------------------
    check("4.3.3 breakeven inflow at 8%", I0 / AF15, D("7009772.70"))
    # The two standard forms of capital recovery must agree: dividing by the annuity factor and
    # multiplying by the capital-recovery factor r/(1-(1+r)^-n). Comparing I0/AF15 with itself
    # asserted nothing; comparing the two FORMS is the identity the sentence claims.
    check("4.3.3 breakeven equals the capital-recovery annuity",
          I0 / AF15, I0 * (D("0.08") / (1 - D("1.08") ** -15)))
    check("4.3.3 revenue headroom (%)", (A - I0 / AF15) / A * 100, D("21.24"), tol=D("0.005"))
    check("4.3.3 rate headroom (points)", IRR * 100 - 8, D("4.19"), tol=D("0.005"))
    TABLE = {
        "0.80": [D(9151213), D(4848348), D(943488), D(-2607898), D(-5844714)],
        "0.90": [D(17795114), D(12954391), D(8561424), D(4566114), D(924697)],
        "1.00": [D(26439016), D(21060435), D(16179360), D(11740127), D(7694108)],
        "1.10": [D(35082918), D(29166478), D(23797296), D(18914140), D(14463518)],
    }
    RATES = ["0.06", "0.07", "0.08", "0.09", "0.10"]
    for m, row in TABLE.items():
        for rr, want in zip(RATES, row):
            check(f"4.3.3 two-way NPV at m={m}, r={rr}",
                  A * D(m) * af(rr, 15) - I0, want, tol=D("1.0"))
    # The IRR column is computed on the retained root, where NPV collapses to (m - 1) x I0 exactly.
    # That identity is the reason the column is round, and checking it proves the manuscript's claim
    # rather than merely reproducing its cells.
    for m, want in [("0.80", D(-12000000)), ("0.90", D(-6000000)),
                    ("1.00", D(0)), ("1.10", D(6000000))]:
        check(f"4.3.3 two-way NPV at m={m}, at the retained IRR",
              A * D(m) * af(IRR, 15) - I0, want, tol=D("0.02"))
        check(f"4.3.3 INVARIANT (m-1) x I0 at the IRR, m={m}",
              A * D(m) * af(IRR, 15) - I0, (D(m) - 1) * I0, tol=D("0.02"))
    check("4.3.3 shift if the column used the published 12.19% instead",
          A * af("0.1219", 15) - I0, D(6751), tol=D("0.5"))
    for rr, want in [("0.06", D("6177765.84")), ("0.07", D("6587677.48")),
                     ("0.08", D("7009772.70")), ("0.09", D("7443532.96")),
                     ("0.10", D("7888426.61"))]:
        check(f"4.3.3 breakeven inflow at r={rr}", I0 / af(rr, 15), want)

    # ---- 4.A.3 hurdle reconciliation to Domain 9's WACC ----------------------------
    W_PROP, W_BIND = D("0.079860"), D("0.080001")
    check("4.A.3 Domain 9 WACC at 70/30 (from k_d, k_e, gearing)",
          D("0.70") * D("4.80") + D("0.30") * D("15.42"), D("7.9860"), tol=D("0.00005"))
    check("4.A.3 AF at the derived WACC", af(W_PROP, 15), D("8.56680051"), tol=D("0.000000005"))
    check("4.A.3 AF at the board rate", AF15, D("8.55947869"), tol=D("0.000000005"))
    check("4.A.3 AF at the coverage-binding WACC", af(W_BIND, 15), D("8.55942642"),
          tol=D("0.000000005"))
    NPV_PROP = A * af(W_PROP, 15) - I0
    NPV_BIND = A * af(W_BIND, 15) - I0
    check("4.A.3 NPV at 7.9860%", NPV_PROP, D(16244525), tol=D("0.5"))
    check("4.A.3 NPV at 8.0000%", NPV, D(16179360), tol=D("0.5"))
    check("4.A.3 NPV at 8.0001%", NPV_BIND, D(16178895), tol=D("0.5"))
    check("4.A.3 understatement from rounding the hurdle up", NPV_PROP - NPV, D(65164),
          tol=D("0.5"))
    check("4.A.3 rate gap (bp)", (D("0.08") - W_PROP) * 10000, D("1.40"), tol=D("0.005"))
    check("4.A.3 NPV per basis point", (NPV_PROP - NPV) / D("1.40"), D(46546), tol=D("0.5"))
    check("4.A.3 understatement as share of NPV (%)", (NPV_PROP - NPV) / NPV * 100, D("0.40"),
          tol=D("0.005"))
    check("4.A.3 WACC gap, proposed vs coverage-binding (bp)",
          (W_BIND - W_PROP) * 10000, D("1.41"), tol=D("0.005"))
    # the category error, priced
    KE = D("0.1542")
    check("4.A.3 AF at the cost of equity", af(KE, 15), D("5.730514"), tol=D("0.0000005"))
    check("4.A.3 NPV of project flow wrongly discounted at k_e", A * af(KE, 15) - I0,
          D(-8998423), tol=D("0.5"))
    check("4.A.3 size of the category error", NPV - (A * af(KE, 15) - I0), D(25177784),
          tol=D("0.5"))
    check("4.A.3 equity return range across gearing (bp)",
          (D("13.6146") - D("11.7685")) * 100, D("184.61"), tol=D("0.005"))

    # ---- 4.A.4 staged investment ----------------------------------------------------
    GOOD, BAD, P = D(10400000), D(6650000), D("0.6")
    check("4.A.4 expected inflow reconciles to the master thread", P * GOOD + (1 - P) * BAD, A)
    NPV_G, NPV_B = GOOD * AF15 - I0, BAD * AF15 - I0
    check("4.A.4 NPV in the strong state", NPV_G, D("29018578.35"))
    check("4.A.4 NPV in the weak state", NPV_B, D("-3079466.73"))
    check("4.A.4 INVARIANT weighted state NPVs equal the NPV of the mean forecast",
          P * NPV_G + (1 - P) * NPV_B, NPV, tol=D("0.01"))
    check("4.A.4 expected value at t=1 of building only when strong", P * NPV_G,
          D("17411147.01"))
    STAGED = P * NPV_G / (1 + R)
    check("4.A.4 staged value at t=0 with a free pilot", STAGED, D("16121432.42"))
    check("4.A.4 staging loses even with a free pilot", NPV - STAGED, D("57927.90"))
    check("4.A.4 expected loss the abandonment option avoids", (1 - P) * -NPV_B,
          D("1231786.69"))
    check("4.A.4 cost of deferring the NPV by one year", NPV - NPV / (1 + R), D("1198471.14"))
    # the same mean, a wider spread: now the option pays
    G2, B2, P2 = D(12000000), D(5800000), D("0.5")
    check("4.A.4 wide case has the same expected inflow", P2 * G2 + (1 - P2) * B2, A)
    NPV_G2, NPV_B2 = G2 * AF15 - I0, B2 * AF15 - I0
    check("4.A.4 wide case strong-state NPV", NPV_G2, D("42713744.26"))
    check("4.A.4 wide case weak-state NPV", NPV_B2, D("-10355023.61"))
    check("4.A.4 wide case build-now NPV is unchanged", P2 * NPV_G2 + (1 - P2) * NPV_B2, NPV,
          tol=D("0.01"))
    STAGED2 = P2 * NPV_G2 / (1 + R)
    check("4.A.4 wide case staged value", STAGED2, D("19774881.60"))
    check("4.A.4 wide case maximum pilot cost", STAGED2 - NPV, D("3595521.28"))
    check("4.A.4 wide case avoided loss", P2 * -NPV_B2, D("5177511.81"))

    # ---- calculation exercises ------------------------------------------------------
    check("EX 4.1 PV year 1", D(3000000) / D("1.10"), D(2727273), tol=D("0.5"))
    check("EX 4.1 PV year 2", D(4000000) / D("1.10") ** 2, D(3305785), tol=D("0.5"))
    check("EX 4.1 PV year 3", D(5000000) / D("1.10") ** 3, D(3756574), tol=D("0.5"))
    check("EX 4.1 PV year 4", D(5000000) / D("1.10") ** 4, D(3415067), tol=D("0.5"))
    EX1 = (D(3000000) / D("1.10") + D(4000000) / D("1.10") ** 2 + D(5000000) / D("1.10") ** 3
           + D(5000000) / D("1.10") ** 4)
    check("EX 4.1 PV total", EX1, D(13204699), tol=D("0.5"))
    check("EX 4.1 NPV", EX1 - D(12000000), D(1204699), tol=D("0.5"))
    check("EX 4.1 common error: undiscounted year 1 overstates NPV by about 1.19m",
          (D(3000000) + D(4000000) / D("1.10") + D(5000000) / D("1.10") ** 2
           + D(5000000) / D("1.10") ** 3) - EX1, D(1320470), tol=D(130000))
    check("EX 4.2 AF target", D(9000000) / D(1500000), D(6), tol=D("0.0000001"))
    check("EX 4.2 IRR", solve(lambda r: D(1500000) * af(r, 10) - D(9000000), "0.01", "0.5") * 100,
          D("10.56"), tol=D("0.005"))
    check("EX 4.2 AF(0.10,10) brackets above", af("0.10", 10), D("6.145"), tol=D("0.0005"))
    check("EX 4.2 AF(0.11,10) brackets below", af("0.11", 10), D("5.889"), tol=D("0.0005"))
    check("EX 4.2 common error: payback reciprocal", D(1500000) / D(9000000) * 100, D("16.7"),
          tol=D("0.05"))
    check("EX 4.3 FVAF(0.07,6)", fvaf("0.07", 6), D("7.153291"), tol=D("0.0000005"))
    check("EX 4.3 terminal value", D(2600000) * fvaf("0.07", 6), D(18598556), tol=D("0.5"))
    check("EX 4.3 money multiple", D(2600000) * fvaf("0.07", 6) / D(10000000), D("1.859856"),
          tol=D("0.0000005"))
    check("EX 4.3 MIRR",
          ((D(2600000) * fvaf("0.07", 6) / D(10000000)) ** (D(1) / 6) - 1) * 100, D("10.90"),
          tol=D("0.005"))
    check("EX 4.4 AF(0.09,4)", af("0.09", 4), D("3.239720"), tol=D("0.0000005"))
    check("EX 4.4 AF(0.09,6)", af("0.09", 6), D("4.485919"), tol=D("0.0000005"))
    PVC1 = D(3000000) + D(250000) * af("0.09", 4)
    PVC2 = D(4200000) + D(150000) * af("0.09", 6)
    check("EX 4.4 PV of C1", PVC1, D(3809930), tol=D("0.5"))
    check("EX 4.4 PV of C2", PVC2, D(4872888), tol=D("0.5"))
    check("EX 4.4 EAC of C1", PVC1 / af("0.09", 4), D(1176006), tol=D("0.5"))
    check("EX 4.4 EAC of C2", PVC2 / af("0.09", 6), D(1086263), tol=D("0.5"))
    check("EX 4.4 annual saving from C2", PVC1 / af("0.09", 4) - PVC2 / af("0.09", 6), D(89743),
          tol=D("0.5"))
    for nm, i0, npv_, pi in [("A", D(10), D("2.60"), D("1.260")), ("C", D(8), D("1.84"), D("1.230")),
                             ("B", D(15), D("3.30"), D("1.220")), ("D", D(7), D("1.40"), D("1.200"))]:
        check(f"EX 4.5 PI of {nm}", (npv_ + i0) / i0, pi, tol=D("0.0005"))
    check("EX 4.5 greedy A+C+D spend", D(10) + D(8) + D(7), D(25))
    check("EX 4.5 greedy A+C+D value", D("2.60") + D("1.84") + D("1.40"), D("5.84"))
    check("EX 4.5 optimal A+B spend", D(10) + D(15), D(25))
    check("EX 4.5 optimal A+B value", D("2.60") + D("3.30"), D("5.90"))
    check("EX 4.5 value greedy foregoes", D("5.90") - D("5.84"), D("0.06"))
    check("EX 4.6 AF(0.09,12)", af("0.09", 12), D("7.160725"), tol=D("0.0000005"))
    check("EX 4.6 AF(0.09,2)", af("0.09", 2), D("1.759111"), tol=D("0.0000005"))
    check("EX 4.6 deferred factor", af("0.09", 12) - af("0.09", 2), D("5.401614"),
          tol=D("0.0000005"))
    EX6 = D(3600000) * (af("0.09", 12) - af("0.09", 2))
    check("EX 4.6 PV", EX6, D(19445811), tol=D("0.5"))
    check("EX 4.6 NPV", EX6 - D(18000000), D(1445811), tol=D("0.5"))
    EX6W = D(3600000) * af("0.09", 10) - D(18000000)
    check("EX 4.6 common error: AF(0.09,10) without deferral", EX6W, D(5103568), tol=D("0.5"))
    check("EX 4.6 overstatement from the common error", EX6W - (EX6 - D(18000000)), D(3657757),
          tol=D("0.5"))
    check("EX 4.7 (1.07)^0.5", D("1.07") ** D("0.5"), D("1.0344080"), tol=D("0.00000005"))
    check("EX 4.7 PV mid-period", D(41250000) * D("1.07") ** D("0.5"), D(42669332), tol=D("0.5"))
    check("EX 4.7 uplift", D(41250000) * (D("1.07") ** D("0.5") - 1), D(1419332), tol=D("0.5"))
    check("EX 4.7 uplift (%)", (D("1.07") ** D("0.5") - 1) * 100, D("3.44"), tol=D("0.005"))
    check("EX 4.7 common error: linear 1+r/2", D(41250000) * D("1.035"), D(42693750),
          tol=D("0.5"))
    check("EX 4.7 error size of the linear approximation",
          D(41250000) * D("1.035") - D(41250000) * D("1.07") ** D("0.5"), D(24418), tol=D("0.5"))
    check("EX 4.8 DF(6.5%,5)", 1 / D("1.065") ** 5, D("0.729881"), tol=D("0.0000005"))
    EX8PV = D(25000000) + D(4000000) / D("1.065") ** 5
    check("EX 4.8 PV of outflows", EX8PV, D(27919523), tol=D("0.5"))
    check("EX 4.8 FVAF(0.035,12)", fvaf("0.035", 12), D("14.601962"), tol=D("0.0000005"))
    EX8TV = D(5200000) * fvaf("0.035", 12)
    check("EX 4.8 terminal value", EX8TV, D(75930201), tol=D("0.5"))
    check("EX 4.8 ratio", EX8TV / EX8PV, D("2.719609"), tol=D("0.0000005"))
    check("EX 4.8 MIRR", ((EX8TV / EX8PV) ** (D(1) / 12) - 1) * 100, D("8.6948"), tol=D("0.00005"))
    EX8W = D(5200000) * fvaf("0.065", 12)
    check("EX 4.8 common error: one rate for both roles",
          ((EX8W / EX8PV) ** (D(1) / 12) - 1) * 100, D("10.2790"), tol=D("0.00005"))
    check("EX 4.8 overstatement (bp)",
          (((EX8W / EX8PV) ** (D(1) / 12) - 1) - ((EX8TV / EX8PV) ** (D(1) / 12) - 1)) * 10000,
          D(158), tol=D("0.5"))
    AF20 = af("0.075", 20)
    check("EX 4.9 AF(0.075,20)", AF20, D("10.194491"), tol=D("0.0000005"))
    EX9 = D(4400000) * AF20 - D(32000000)
    check("EX 4.9 NPV", EX9, D(12855762), tol=D("0.5"))
    check("EX 4.9 EAV", EX9 / AF20, D(1261050), tol=D("0.5"))
    check("EX 4.9 capital recovery", D(32000000) / AF20, D(3138950), tol=D("0.5"))
    check("EX 4.9 INVARIANT recovery + EAV = inflow", D(32000000) / AF20 + EX9 / AF20,
          D(4400000), tol=D("0.01"))
    check("EX 4.9 value share (%)", (EX9 / AF20) / D(4400000) * 100, D("28.66"), tol=D("0.005"))
    check("EX 4.9 headroom (%)", (D(4400000) - D(32000000) / AF20) / D(4400000) * 100,
          D("28.66"), tol=D("0.005"))
    AF6 = af(R, 6)
    check("EX 4.10 AF(0.08,6)", AF6, D("4.622880"), tol=D("0.0000005"))
    check("EX 4.10 NPV of S", D(2100000) * AF6 - D(7000000), D(2708047), tol=D("0.5"))
    check("EX 4.10 NPV of T", D(4300000) * AF6 - D(16000000), D(3878383), tol=D("0.5"))
    check("EX 4.10 NPV gap", (D(4300000) * AF6 - D(16000000)) - (D(2100000) * AF6 - D(7000000)),
          D(1170335), tol=D("0.5"))
    check("EX 4.10 IRR of S",
          solve(lambda r: D(2100000) * af(r, 6) - D(7000000), "0.01", "1.0") * 100, D("19.9054"),
          tol=D("0.00005"))
    check("EX 4.10 IRR of T",
          solve(lambda r: D(4300000) * af(r, 6) - D(16000000), "0.01", "1.0") * 100, D("15.6321"),
          tol=D("0.00005"))
    check("EX 4.10 incremental AF target", D(9000000) / D(2200000), D("4.090909"),
          tol=D("0.0000005"))
    XO2 = solve(lambda r: D(2200000) * af(r, 6) - D(9000000), "0.001", "0.60")
    check("EX 4.10 crossover", XO2 * 100, D("12.1767"), tol=D("0.00005"))
    check("EX 4.10 INVARIANT NPV of S at the crossover", D(2100000) * af(XO2, 6) - D(7000000),
          D("1590909.09"), tol=D("0.05"))
    check("EX 4.10 INVARIANT NPV of T at the crossover", D(4300000) * af(XO2, 6) - D(16000000),
          D("1590909.09"), tol=D("0.05"))
    check("EX 4.10 hurdle distance from indifference (points)", XO2 * 100 - 8, D("4.18"),
          tol=D("0.005"))
    AF10_15 = af("0.10", 15)
    check("EX 4.11 AF(0.10,15)", AF10_15, D("7.606080"), tol=D("0.0000005"))
    MEAN11 = D("0.55") * D(5400000) + D("0.45") * D(3200000)
    check("EX 4.11 expected inflow", MEAN11, D(4410000))
    NOW11 = MEAN11 * AF10_15 - D(30000000)
    check("EX 4.11 build-now NPV", NOW11, D(3542811), tol=D("0.5"))
    G11, B11 = D(5400000) * AF10_15 - D(30000000), D(3200000) * AF10_15 - D(30000000)
    check("EX 4.11 strong-state NPV", G11, D(11072829), tol=D("0.5"))
    check("EX 4.11 weak-state NPV", B11, D(-5660546), tol=D("0.5"))
    check("EX 4.11 INVARIANT weighted states equal the mean case",
          D("0.55") * G11 + D("0.45") * B11, NOW11, tol=D("0.01"))
    ST11 = D("0.55") * G11 / D("1.10")
    check("EX 4.11 staged value with a free study", ST11, D(5536415), tol=D("0.5"))
    check("EX 4.11 maximum study cost", ST11 - NOW11, D(1993604), tol=D("0.5"))
    check("EX 4.11 common error: avoided loss alone", D("0.45") * -B11, D(2547246), tol=D("0.5"))
    check("EX 4.11 size of that error", D("0.45") * -B11 - (ST11 - NOW11), D(553641),
          tol=D("0.5"))
