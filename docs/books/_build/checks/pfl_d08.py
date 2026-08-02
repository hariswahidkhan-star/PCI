"""PFL-AI Domain 8 — Cost, schedule and contingency integration: golden checks.

Every number printed as a result in `domain-08-cost-schedule-contingency.md` (and in its figure
module `figures_src/pfl_d08.py`) is recomputed here from first principles. Master-thread values are
read from ctx, never re-typed.

Four engines drive the domain, and each is rebuilt from primitives below rather than trusted:

1. the estimate ladder — contingency implied by an accuracy range's upper bound on a stated base;
2. the **area rule** `IDC = r_q x g x S x SUM cum(t-1)`, checked against a full period-by-period
   drawdown recursion so the closed form is *proved*, not asserted;
3. Bernoulli register aggregation (mean = SUM EMV, variance = SUM p(1-p)impact^2) and the
   triangular translation of an accuracy range, with the normal/triangular percentile inverses;
4. Domain 6's construction funding recursion, reproduced from the certified-spend base so that the
   drawn balance at each quarter end — and therefore every slip-cost figure of 8.4.2 and Fig 8.4.1 —
   is derived here rather than copied from the manuscript.

Teaching invariants are pinned as well as arithmetic: the area rule identity, shape neutrality at
e ~ g*r, the scale-invariance of a range's P80 percentage, TCPI(BAC/CPI) == CPI, monotonicity of the
slip-cost rows through the programme, that correlation raises the P80 and leaves the mean alone, and
that an opportunity lowers the mean while raising the variance.
"""

import math


def _phi(D, z):
    """Standard normal CDF (float erf is ample for a 4-significant-figure percentile)."""
    return D(str(0.5 * (1.0 + math.erf(float(z) / math.sqrt(2.0)))))


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]

    DEBT = ctx["KESTREL_DEBT"]                 # 42,000,000
    EQ = ctx["KESTREL_EQUITY"]                 # 18,000,000
    CAPEX = ctx["KESTREL_CAPEX"]               # 60,000,000
    RATE = ctx["KESTREL_RATE"]                 # 0.06
    TENOR = ctx["KESTREL_TENOR"]               # 12
    LIFE = ctx["KESTREL_LIFE"]                 # 25
    DS = ctx["KESTREL_INSTALMENT"]             # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    DISC = ctx["KESTREL_DISCOUNT"]             # 0.08
    BAC = ctx["AURIGA_BAC"]                    # 4,000,000

    AF612 = af(RATE, TENOR)
    COV = D("1.20")
    HEADROOM = CF - COV * DS                   # 372,437.724 (Domain 10, KA 10.2.1)
    Z80, Z95 = D("0.8416"), D("1.6449")

    # =================================================================================
    # Master thread — the figures Domain 8 inherits and must not move
    # =================================================================================
    check("D8 master thread AF(6%,12)", AF612, "8.383844", D("0.0000005"))
    check("D8 master thread instalment ties to D10", DEBT / AF612, DS, D("0.005"))
    check("D8 master thread gearing 70/30", DEBT / CAPEX * 100, "70.00")
    check("D8 master thread equity share", EQ / CAPEX * 100, "30.00")
    check("D8 master thread DSCR 1.2743", CF / DS, "1.2743", D("0.00005"))
    check("D8 master thread annual headroom 372,438", HEADROOM, 372438, D("0.3"))
    check("D8 master thread year-one interest", DEBT * RATE, ctx["KESTREL_INTEREST_Y1"])
    check("D8 master thread year-one principal", DS - DEBT * RATE, 2489635, D("0.3"))

    EPC = D(48000000)
    OWNERS = D(3600000)
    DEVCOST = D(1800000)
    FEES = D(840000)
    IDC_D6 = D(2114597)
    CONT = D(3645403)
    check("D8 master thread sources = uses (envelope reconciles)",
          EPC + OWNERS + DEVCOST + FEES + IDC_D6 + CONT, CAPEX, D("0.6"))
    check("D8 master thread fees are 2.0% of the facility", FEES / DEBT * 100, "2.00")
    ESCAL = D("0.036")                         # construction escalation, unwrapped scope

    PROFILE = [D(w) for w in (6, 9, 13, 16, 17, 15, 13, 11)]
    check("D8 master thread spend profile sums to 100 per cent", sum(PROFILE), 100)

    # =================================================================================
    # KA 8.1.1 — the four cost families
    # =================================================================================
    check("8.1.1 a 500,000 opex error against annual headroom",
          D(500000) / HEADROOM, "1.34", D("0.005"))
    # invariant: an operating error recurs; over the loan life it is 12x the annual figure
    check("8.1.1 invariant: the opex error recurs for the whole loan life",
          D(500000) * TENOR / HEADROOM, "16.11", D("0.005"))

    # =================================================================================
    # KA 8.1.2 — the estimate ladder and Kestrel's 7.59 per cent
    # =================================================================================
    LADDER = [("A screening", "0.50", 24000000), ("B concept", "0.40", 19200000),
              ("C feasibility", "0.30", 14400000), ("D definition", "0.18", 8640000),
              ("E control", "0.08", 3840000)]
    for lab, up, want in LADDER:
        check(f"Table 8.1.2 Stage {lab} implied contingency to upper bound",
              EPC * D(up), want)
    check("8.1.2 ladder spans a factor of 6.25 on one base",
          (EPC * D("0.50")) / (EPC * D("0.08")), "6.25")
    # invariant: the factor is the ratio of the two upper bounds, independent of base
    check("8.1.2 invariant: the 6.25 factor is base-independent",
          D("0.50") / D("0.08"), "6.25")

    check("WE 8.1.2 contingency as % of base", CONT / EPC * 100, "7.59", D("0.005"))
    check("WE 8.1.2 coverage of the Stage E +8% band",
          CONT / (EPC * D("0.08")) * 100, "94.93", D("0.005"))
    check("WE 8.1.2 short of the full +8% position",
          EPC * D("0.08") - CONT, 194597)
    check("WE 8.1.2 coverage of the Stage C +30% band",
          CONT / (EPC * D("0.30")) * 100, "25.32", D("0.005"))
    check("WE 8.1.2 Stage C provision as a multiple of the funded amount",
          EPC * D("0.30") / CONT, "3.950", D("0.0005"))
    check("WE 8.1.2 unwrapped provision of the order of 14,400,000",
          EPC * D("0.30"), 14400000)
    check("AI 8.1 class inflation C->E is worth",
          EPC * D("0.30") - EPC * D("0.08"), 10560000)
    # invariant: the 60,000,000 envelope cannot fund the unwrapped provision
    check("8.1.2 invariant: unwrapped envelope requirement exceeds 60,000,000",
          EPC + OWNERS + DEVCOST + FEES + IDC_D6 + EPC * D("0.30") - CAPEX,
          EPC * D("0.30") - CONT, D("0.6"))
    check("8.1.2 invariant: the unwrapped overshoot of the envelope",
          EPC * D("0.30") - CONT, 10754597, D("0.6"))

    # =================================================================================
    # KA 8.1.3 — lifecycle programme, level charge, the year-seven cliff
    # =================================================================================
    LC = [(7, D(3200000), 4098909, 2391674), (12, D(1400000), 2140154, 849885),
          (14, D(3200000), 5250329, 1787533), (21, D(3200000), 6725194, 1335999),
          (24, D(1400000), 3271615, 515931)]
    nom_t = pv_t = real_t = D(0)
    for t, real, nom_p, pv_p in LC:
        nom = real * (1 + ESCAL) ** t
        pv = nom / (1 + DISC) ** t
        check(f"WE 8.1.3 year {t} nominal cost", nom, nom_p, D("0.6"))
        check(f"WE 8.1.3 year {t} present value", pv, pv_p, D("0.6"))
        real_t += real
        nom_t += nom
        pv_t += pv
    check("WE 8.1.3 total real cost", real_t, 12400000)
    check("WE 8.1.3 total nominal cost", nom_t, 21486202, D("0.6"))
    check("WE 8.1.3 total present value", pv_t, 6881021, D("0.6"))
    check("WE 8.1.3 sponsor appraisal rate is Domain 4's 8.0 per cent", DISC * 100, "8.0")
    AF825 = af(DISC, LIFE)
    check("WE 8.1.3 AF(8%,25)", AF825, "10.674776", D("0.0000005"))
    LEVEL = pv_t / AF825
    check("WE 8.1.3 level annual charge (EAV of the programme)", LEVEL, 644606, D("0.6"))
    check("WE 8.1.3 level charge as % of CFADS", LEVEL / CF * 100, "10.10", D("0.005"))
    NOM7 = D(3200000) * (1 + ESCAL) ** 7
    check("WE 8.1.3 year-seven CFADS without a reserve", CF - NOM7, 2285091, D("0.6"))
    check("WE 8.1.3 year-seven DSCR without a reserve", (CF - NOM7) / DS, "0.4561", D("0.00005"))
    check("WE 8.1.3 DSCR with a funded reserve stays at the base case",
          CF / DS, "1.2743", D("0.00005"))
    # invariants of the level-charge identity and the reserve
    check("8.1.3 invariant: level charge x AF(8%,25) reproduces the PV", LEVEL * AF825, pv_t,
          D("0.005"))
    check("8.1.3 invariant: year-seven DSCR is a payment default (< 1.00)",
          D(1) if (CF - NOM7) / DS < 1 else D(0), 1)
    check("8.1.3 invariant: the reserve is cheaper per year than the spike it removes",
          D(1) if LEVEL < NOM7 else D(0), 1)
    check("8.1.3 escalation is load-bearing: year 21 nominal against base-date cost",
          D(3200000) * (1 + ESCAL) ** 21, 6725194, D("0.6"))
    check("8.1.3 PLCR of the tail (cited from Domain 10)",
          CF * af(RATE, LIFE) / DEBT, "1.9431", D("0.00005"))

    # MCQ 8.1-C distractors
    check("MCQ 8.1-C option C: escalation forgotten",
          (CF - D(3200000)) / DS, "0.6356", D("0.00005"))
    check("MCQ 8.1-C option C intermediate CFADS", CF - D(3200000), 3184000)
    check("MCQ 8.1-C option D: level charge deducted instead of the spend",
          (CF - LEVEL) / DS, "1.1457", D("0.00005"))
    check("MCQ 8.1-C option D intermediate CFADS", CF - LEVEL, 5739394, D("0.6"))
    # MCQ 8.1-D
    check("MCQ 8.1-D opex error over headroom", D(500000) / D(372438), "1.34", D("0.005"))

    # =================================================================================
    # KA 8.2.2 — the area rule and the price of shape
    # =================================================================================
    S = EPC
    G = D("0.70")
    RQ = RATE / 4
    check("WE 8.2.2 quarterly rate", RQ * 100, "1.50")
    A = [D(w) for w in (6, 9, 13, 16, 17, 15, 13, 11)]
    B = [D(w) for w in (14, 16, 16, 14, 12, 11, 9, 8)]
    C = [D(w) for w in (5, 7, 9, 12, 15, 17, 18, 17)]
    for lab, p in (("A S-curve", A), ("B front-loaded", B), ("C back-loaded", C)):
        check(f"WE 8.2.2 profile {lab} sums to 100 per cent", sum(p), 100)

    def area(p):
        """SUM cum(t-1) as a fraction — the discrete area under the cumulative curve."""
        cum = tot = D(0)
        for w in p:
            tot += cum
            cum += w
        return tot / 100

    def idc_recursion(p, base=None, factor=None):
        """Period-by-period IDC: interest on the opening balance, draws at period end."""
        base = S if base is None else base
        bal = idc = D(0)
        for t, w in enumerate(p, start=1):
            idc += bal * RQ
            spend = base * w / 100
            if factor is not None:
                spend *= factor ** t
            bal += G * spend
        return idc

    COEF = RQ * G * S
    check("WE 8.2.2 area-rule coefficient r_q x g x S", COEF, 504000)
    for lab, p, ar, want in (("A", A, "3.1900", 1607760), ("B", B, "3.9700", 2000880),
                             ("C", C, "2.6700", 1345680)):
        check(f"WE 8.2.2 profile {lab} accumulated area", area(p), ar, D("0.00005"))
        check(f"WE 8.2.2 profile {lab} IDC", COEF * area(p), want)
        # INVARIANT: the closed form equals the full recursion, to the cent
        check(f"WE 8.2.2 invariant: area rule == recursion for profile {lab}",
              idc_recursion(p), COEF * area(p), D("0.005"))
    idcA, idcB, idcC = (COEF * area(p) for p in (A, B, C))
    check("WE 8.2.2 spread between front- and back-loaded", idcB - idcC, 655200)
    check("WE 8.2.2 spread as % of the S-curve's own IDC",
          (idcB - idcC) / idcA * 100, "40.75", D("0.005"))
    # invariants of the area rule
    check("8.2.2 invariant: IDC is monotone in the area (front > S-curve > back)",
          D(1) if idcB > idcA > idcC else D(0), 1)
    check("8.2.2 invariant: IDC is linear in the area — doubling the area doubles IDC",
          COEF * (area(A) * 2), idcA * 2, D("0.005"))
    check("8.2.2 invariant: an end-loaded unit profile has zero area",
          area([D(0)] * 7 + [D(100)]), 0)
    check("8.2.2 invariant: maximum possible area over 8 periods is 7.00",
          area([D(100)] + [D(0)] * 7), "7.00")
    # MCQ 8.2-A distractors
    check("MCQ 8.2-A option A: the S-curve area used instead", idcA, 1607760)
    check("MCQ 8.2-A option C: gearing omitted", RQ * S * area(B), 2858400)
    check("MCQ 8.2-A option D: the coefficient mistaken for the answer", COEF, 504000)

    # =================================================================================
    # KA 8.2.3 — escalation on the profile, and shape neutrality
    # =================================================================================
    FQ = (1 + ESCAL) ** (D(1) / 4)
    check("WE 8.2.3 quarterly escalation factor", FQ, "1.00888099", D("0.000000005"))
    check("8.2.3 invariant: four quarterly steps compound to the annual rate",
          FQ ** 4, 1 + ESCAL, D("0.0000000005"))

    def esc_spend(p, base=None, factor=None):
        base = S if base is None else base
        factor = FQ if factor is None else factor
        return sum(base * w / 100 * factor ** t for t, w in enumerate(p, start=1))

    TAB = (("B", B, 49750365, 1750365, 2052036, 51802401),
           ("A", A, 50093393, 2093393, 1658953, 51752346),
           ("C", C, 50324512, 2324512, 1391209, 51715720))
    tot = {}
    for lab, p, es_p, ov_p, ic_p, t_p in TAB:
        es = esc_spend(p)
        ic = idc_recursion(p, factor=FQ)
        tot[lab] = es + ic
        check(f"WE 8.2.3 profile {lab} escalated spend", es, es_p, D("0.6"))
        check(f"WE 8.2.3 profile {lab} escalation over base", es - S, ov_p, D("0.6"))
        check(f"WE 8.2.3 profile {lab} IDC on escalated draws", ic, ic_p, D("0.6"))
        check(f"WE 8.2.3 profile {lab} total funded construction cost", es + ic, t_p, D("0.6"))
    check("WE 8.2.3 escalation spread B->C",
          esc_spend(C) - esc_spend(B), 574146, D("0.6"))
    check("WE 8.2.3 IDC spread C->B",
          idc_recursion(B, factor=FQ) - idc_recursion(C, factor=FQ), 660827, D("0.6"))
    check("WE 8.2.3 total spread B->C", tot["B"] - tot["C"], 86681, D("0.6"))
    check("WE 8.2.3 back-loaded advantage over the S-curve", tot["A"] - tot["C"], 36626, D("0.6"))
    check("WE 8.2.3 total spread as % of spend", (tot["B"] - tot["C"]) / tot["A"] * 100,
          "0.17", D("0.005"))
    # INVARIANT: the components dwarf the net — the domain's central caution
    check("8.2.3 invariant: each component spread exceeds the total spread",
          D(1) if min(D(574146), D(660827)) > D(86681) else D(0), 1)
    check("8.2.3 invariant: escalation ranks opposite to IDC",
          D(1) if (esc_spend(C) > esc_spend(A) > esc_spend(B)
                   and idc_recursion(B, factor=FQ) > idc_recursion(A, factor=FQ)
                   > idc_recursion(C, factor=FQ)) else D(0), 1)

    def total_at(p, e):
        f = (1 + e) ** (D(1) / 4)
        return esc_spend(p, factor=f) + idc_recursion(p, factor=f)

    def breakeven(p1, p2):
        lo, hi = D(0), D("0.30")
        s0 = total_at(p1, lo) - total_at(p2, lo) > 0
        for _ in range(200):
            mid = (lo + hi) / 2
            if (total_at(p1, mid) - total_at(p2, mid) > 0) == s0:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    bAC, bAB, bBC = breakeven(A, C), breakeven(A, B), breakeven(B, C)
    check("WE 8.2.3 breakeven escalation, S-curve vs back-loaded", bAC * 100,
          "4.1659", D("0.00005"))
    check("WE 8.2.3 breakeven escalation, S-curve vs front-loaded", bAB * 100,
          "4.1147", D("0.00005"))
    check("MCQ 8.2-C breakeven, front- vs back-loaded", bBC * 100, "4.1352", D("0.00005"))
    check("WE 8.2.3 heuristic g x r", G * RATE * 100, "4.20")
    check("WE 8.2.3 gap between heuristic and computed, basis points",
          (G * RATE - bAC) * 10000, "3.4", D("0.05"))
    # INVARIANTS of shape neutrality
    check("8.2.3 invariant: every pairwise breakeven sits just below g x r",
          D(1) if max(bAC, bAB, bBC) < G * RATE else D(0), 1)
    check("8.2.3 invariant: all three breakevens within 10bp of g x r",
          D(1) if (G * RATE - min(bAC, bAB, bBC)) * 10000 < 10 else D(0), 1)
    check("8.2.3 invariant: above 4.17% front-loading is the cheapest of the three",
          D(1) if total_at(B, D("0.0417")) < min(total_at(A, D("0.0417")),
                                                 total_at(C, D("0.0417"))) else D(0), 1)
    check("8.2.3 invariant: below 3.6% back-loading is the cheapest of the three",
          D(1) if total_at(C, ESCAL) < min(total_at(A, ESCAL), total_at(B, ESCAL)) else D(0), 1)
    # pitfall — escalating the total
    check("8.2.3 pitfall: total escalated by 1.036^2", S * (1 + ESCAL) ** 2, 51518208)
    check("8.2.3 pitfall: overstatement against the profile-correct figure",
          S * (1 + ESCAL) ** 2 - esc_spend(A), 1424815, D("0.6"))
    check("MCQ 8.2-B error size", D(51518208) - D(50093393), 1424815)
    check("8.2.3 INVARIANT revenue escalation is not the cost escalation and must not be reused",
          1 if D("2.967") != ESCAL * 100 else 0, 1)
    check("8.2.3 gap between Domain 6's revenue escalation and this cost escalation, points",
          (ESCAL * 100 - D("2.967")).quantize(D("0.001")), D("0.633"))

    # =================================================================================
    # KA 8.3.1 / 8.3.2 — register aggregation, the triangular range, and the two P80s
    # =================================================================================
    REG = [("C1", D("0.40"), D(2400000), 960000), ("C2", D("0.30"), D(1800000), 540000),
           ("C3", D("0.35"), D(1400000), 490000), ("C4", D("0.50"), D(900000), 450000),
           ("C5", D("0.20"), D(2000000), 400000), ("C6", D("0.25"), D(-600000), -150000)]
    for rid, p, imp, want in REG:
        check(f"WE 8.3.2 {rid} EMV", p * imp, want)
    MEAN = sum(p * imp for _, p, imp, _ in REG)
    VAR = sum(p * (1 - p) * imp * imp for _, p, imp, _ in REG)
    SD = VAR.sqrt()
    check("WE 8.3.2 register mean exposure", MEAN, 2690000)
    check("WE 8.3.2 register variance", VAR, "3418700000000", D("0.6"))
    check("WE 8.3.2 register standard deviation", SD, 1848973, D("0.6"))
    P80 = MEAN + Z80 * SD
    P95 = MEAN + Z95 * SD
    check("WE 8.3.2 register P80 contingency", P80, 4246095, D("0.6"))
    check("WE 8.3.2 register P80 as % of base", P80 / EPC * 100, "8.85", D("0.005"))
    check("WE 8.3.2 register mean as % of base", MEAN / EPC * 100, "5.60", D("0.005"))
    WORST = sum(imp for _, _, imp, _ in REG if imp > 0)
    check("WE 8.3.2 worst-case sum of threats", WORST, 8500000)
    check("WE 8.3.2 worst case as % of base", WORST / EPC * 100, "17.71", D("0.005"))
    # z-value sanity: the quoted multipliers really are the 80th and 95th percentiles
    check("8.3.2 invariant: 0.8416 is the P80 z-value", _phi(D, Z80) * 100, "80.00", D("0.005"))
    check("8.3.1 invariant: 1.6449 is the P95 z-value", _phi(D, Z95) * 100, "95.00", D("0.005"))
    check("8.3.2 invariant: P80 exceeds the mean by 0.8416 sigma", P80 - MEAN, Z80 * SD, D("0.005"))

    # triangular translation of the Stage C range
    a, m, b = EPC * D("0.85"), EPC, EPC * D("1.30")
    check("WE 8.3.2 triangular minimum (-15%)", a, 40800000)
    check("WE 8.3.2 triangular mode", m, 48000000)
    check("WE 8.3.2 triangular maximum (+30%)", b, 62400000)
    TMEAN = (a + m + b) / 3
    check("WE 8.3.2 triangular mean outturn", TMEAN, 50400000)
    check("WE 8.3.2 triangular mean contingency", TMEAN - EPC, 2400000)
    check("WE 8.3.2 triangular mean contingency %", (TMEAN - EPC) / EPC * 100, "5.00")
    check("WE 8.3.2 triangular b - a", b - a, 21600000)
    check("WE 8.3.2 triangular b - m", b - m, 14400000)
    SPREAD = (b - a) * (b - m)
    ROOT = (D("0.20") * SPREAD).sqrt()
    check("WE 8.3.2 triangular sqrt(0.20(b-a)(b-m))", ROOT, 7887205, D("0.6"))
    TP80 = b - ROOT
    check("WE 8.3.2 triangular (b-a)(b-m)", SPREAD, "311040000000000", D("0.6"))
    check("WE 8.3.2 triangular P80 outturn", TP80, 54512795, D("0.6"))
    check("WE 8.3.2 range-based P80 contingency", TP80 - EPC, 6512795, D("0.6"))
    check("WE 8.3.2 range-based P80 contingency %", (TP80 - EPC) / EPC * 100, "13.57", D("0.005"))
    check("WE 8.3.2 the two P80s differ by", (TP80 - EPC) - P80, 2266700, D("0.6"))
    # INVARIANT: the triangular CDF and the P80 inverse are consistent
    check("8.3.2 invariant: F(P80) = 0.80 on the triangular",
          (1 - (b - TP80) ** 2 / SPREAD) * 100, "80.00", D("0.005"))
    check("8.3.2 invariant: the range governs unwrapped (higher of the two)",
          D(1) if (TP80 - EPC) > P80 else D(0), 1)

    # where the funded balancing line sits on each basis
    check("WE 8.3.2 funded amount percentile on the register",
          _phi(D, (CONT - MEAN) / SD) * 100, "69.7", D("0.05"))
    check("WE 8.3.2 funded amount percentile on the Stage C range",
          (1 - (b - (EPC + CONT)) ** 2 / SPREAD) * 100, "62.81", D("0.005"))
    SHORT = P80 - CONT
    check("WE 8.3.2 funded amount is short of the register P80 by", SHORT, 600692, D("0.6"))
    d2 = DEBT + SHORT
    i2 = d2 / AF612
    check("WE 8.3.2 capitalised shortfall takes debt to", d2, 42600692, D("0.6"))
    check("WE 8.3.2 instalment after capitalising the shortfall", i2, "5081284.04", D("0.05"))
    check("WE 8.3.2 DSCR after capitalising the shortfall", CF / i2, "1.2564", D("0.00005"))
    check("WE 8.3.2 headroom after capitalising the shortfall", CF - COV * i2, 286459, D("0.6"))
    check("WE 8.3.2 headroom fall", HEADROOM - (CF - COV * i2), 85979, D("0.6"))
    check("WE 8.3.2 headroom fall as %",
          (HEADROOM - (CF - COV * i2)) / HEADROOM * 100, "23.1", D("0.05"))

    # 8.3.1 contingent support to P95
    check("WE 8.3.1 1.6449 sigma", Z95 * SD, "3041375.17", D("0.05"))
    check("WE 8.3.1 P95 exposure", P95, 5731375, D("0.6"))
    check("WE 8.3.1 contingent support required", P95 - CONT, 2085972, D("0.6"))
    check("WE 8.3.1 cost of moving from P80 to P95", P95 - P80, 1485280, D("0.6"))
    check("8.3.1 invariant: tail cover costs more per point than the body",
          D(1) if (P95 - P80) / (Z95 - Z80) > (P80 - MEAN) / Z80 * D("0.999") else D(0), 1)

    # Fig 8.3.1 bar values and headline
    for lab, val, pct in (("worst case", WORST, "17.71"), ("range P80", TP80 - EPC, "13.57"),
                          ("ten per cent rule", EPC * D("0.10"), "10.00"),
                          ("register P80", P80, "8.85"), ("funded line", CONT, "7.59"),
                          ("register mean", MEAN, "5.60")):
        check(f"Fig 8.3.1 bar {lab} as % of base", val / EPC * 100, pct, D("0.005"))
    check("Fig 8.3.1 spread between the six answers", WORST / MEAN, "3.16", D("0.005"))
    check("Fig 8.3.1 x-axis maximum accommodates the largest bar",
          D(1) if WORST <= D(9000000) else D(0), 1)

    # =================================================================================
    # KA 8.3.3 — the two computable defects of the percentage method
    # =================================================================================
    TEN = EPC * D("0.10")
    check("8.3.3 ten-per-cent rule", TEN, 4800000)
    check("8.3.3 z of the ten-per-cent rule against the register",
          (TEN - MEAN) / SD, "1.1412", D("0.00005"))
    check("8.3.3 confidence the rule buys against the register",
          _phi(D, (TEN - MEAN) / SD) * 100, "87.3", D("0.05"))
    check("8.3.3 the 10%-rule outturn on the Stage C range", EPC + TEN, 52800000)
    check("8.3.3 b less the 10%-rule outturn", b - (EPC + TEN), 9600000)
    check("8.3.3 confidence the rule buys against the range",
          (1 - (b - (EPC + TEN)) ** 2 / SPREAD) * 100, "70.37", D("0.005"))
    check("8.3.3 F as a probability, printed to four places",
          1 - (b - (EPC + TEN)) ** 2 / SPREAD, "0.7037", D("0.00005"))
    check("MCQ 8.3-B option D: the average of two incommensurable percentiles",
          (D("87.3") + D("70.4")) / 2, "78.85", D("0.05"))
    # INVARIANT: the same amount is two different promises — the defect itself
    check("8.3.3 invariant: the two confidences the same 4,800,000 buys differ by",
          _phi(D, (TEN - MEAN) / SD) * 100 - (1 - (b - (EPC + TEN)) ** 2 / SPREAD) * 100,
          "16.94", D("0.05"))

    # C1 retires
    REG2 = REG[1:]
    MEAN2 = sum(p * imp for _, p, imp, _ in REG2)
    VAR2 = sum(p * (1 - p) * imp * imp for _, p, imp, _ in REG2)
    SD2 = VAR2.sqrt()
    P80B = MEAN2 + Z80 * SD2
    check("8.3.3 register mean after C1 retires", MEAN2, 1730000)
    check("8.3.3 register sigma after C1 retires", SD2, 1426990, D("0.6"))
    check("8.3.3 register P80 after C1 retires", P80B, 2930955, D("0.6"))
    check("8.3.3 contingency release available", P80 - P80B, 1315141, D("0.6"))
    check("8.A.2 release worth on Kestrel when C1 closed", P80 - P80B, 1315141, D("0.6"))
    # INVARIANT: the percentage rule does not move
    check("8.3.3 invariant: the percentage rule is unchanged by risk retirement",
          EPC * D("0.10") - TEN, 0)
    EXCESS = TEN - P80B
    check("8.3.3 excess provision after the retirement", EXCESS, 1869045, D("0.6"))
    check("8.3.3 commitment fee on the excess at 0.60%", EXCESS * D("0.006"), 11214, D("0.6"))
    ADD = EXCESS * G
    check("8.3.3 senior debt added if the excess is drawn", ADD, 1308332, D("0.6"))
    check("8.3.3 instalment increase", ADD / AF612, 156054, D("0.6"))
    i3 = DEBT / AF612 + D(1308331.5) / AF612
    check("8.3.3 instalment after drawing the excess", i3, "5165689.12", D("0.05"))
    check("8.3.3 DSCR after drawing the excess", CF / i3, "1.2358", D("0.00005"))
    check("8.3.3 covenant cash trigger after drawing the excess", COV * i3, 6198827, D("0.6"))
    check("8.3.3 headroom after drawing the excess", CF - COV * i3, 185173, D("0.6"))
    check("8.3.3 headroom fall", HEADROOM - (CF - COV * i3), 187265, D("0.6"))
    check("8.3.3 headroom fall as %",
          (HEADROOM - (CF - COV * i3)) / HEADROOM * 100, "50.3", D("0.05"))
    # INVARIANT: undrawn cost is trivial against drawn cost — the asymmetry the KA turns on
    check("8.3.3 invariant: annual drawn cost exceeds the undrawn fee many times over",
          (COV * i3 - COV * DS) / (EXCESS * D("0.006")), "16.70", D("0.05"))
    check("MCQ 8.3-C option C: the undrawn commitment fee", EXCESS * D("0.006"), 11214, D("0.6"))
    check("MCQ 8.3-C option D: the excess itself", EXCESS, 1869045, D("0.6"))
    check("MCQ 8.3-A option A: the mean", MEAN, 2690000)
    check("MCQ 8.3-A option C: the worst-case sum", WORST, 8500000)
    check("MCQ 8.3-A option D: the P95", P95, 5731375, D("0.6"))

    # 8.A.1 correlation
    s1 = (D("0.40") * D("0.60")).sqrt() * D(2400000)
    s2 = (D("0.30") * D("0.70")).sqrt() * D(1800000)
    VARC = VAR + 2 * s1 * s2                     # rho = 1 between C1 and C2
    check("8.A.1 invariant: correlation leaves the mean unchanged",
          sum(p * imp for _, p, imp, _ in REG), MEAN)
    check("8.A.1 invariant: positive correlation raises the P80",
          D(1) if MEAN + Z80 * VARC.sqrt() > P80 else D(0), 1)
    check("8.A.1 invariant: the independent P80 is therefore a lower bound",
          D(1) if VARC > VAR else D(0), 1)

    # =================================================================================
    # KA 8.4.1 — earned value to cost to complete, and the two lender tests
    # =================================================================================
    AC, EV = D(2120000), D(1920000)
    CPI = EV / AC
    PV_A = D(2080000)
    SPI = EV / PV_A
    check("WE 8.4.1 CPI", CPI, "0.905660", D("0.0000005"))
    check("WE 8.4.1 SPI", SPI, "0.923077", D("0.0000005"))
    E1 = AC + (BAC - EV)
    E2 = BAC / CPI
    E3 = AC + (BAC - EV) / (CPI * SPI)
    check("WE 8.4.1 EAC remaining at budgeted rate", E1, 4200000)
    check("WE 8.4.1 EAC = BAC/CPI", E2, 4416667, D("0.6"))
    check("WE 8.4.1 EAC on CPI x SPI", E3, 4608056, D("0.6"))
    FUND = BAC + D(300000)
    check("WE 8.4.1 total funding", FUND, 4300000)
    check("WE 8.4.1 funded contingency as % of BAC", D(300000) / BAC * 100, "7.50")
    AVAIL = FUND - AC
    check("WE 8.4.1 funds available", AVAIL, 2180000)
    for lab, e, ctc_p, sur_p in (("budgeted rate", E1, 2080000, 100000),
                                 ("BAC/CPI", E2, 2296667, -116667),
                                 ("CPI x SPI", E3, 2488056, -308056)):
        check(f"WE 8.4.1 CTC on {lab}", e - AC, ctc_p, D("0.6"))
        check(f"WE 8.4.1 surplus/(shortfall) on {lab}", AVAIL - (e - AC), sur_p, D("0.6"))
    # INVARIANT: CTC = EAC - AC, and sufficiency = available - CTC
    check("8.4.1 invariant: sufficiency on BAC/CPI equals funding less EAC",
          AVAIL - (E2 - AC), FUND - E2, D("0.005"))
    check("8.4.1 invariant: EAC order gives the reverse sufficiency order",
          D(1) if (E1 < E2 < E3 and (AVAIL - (E1 - AC)) > (AVAIL - (E2 - AC))
                   > (AVAIL - (E3 - AC))) else D(0), 1)
    # INVARIANT claimed in "AI in this KA": TCPI to the BAC/CPI forecast equals current CPI
    check("8.4.1 invariant: TCPI to the BAC/CPI forecast equals CPI",
          (BAC - EV) / (E2 - AC), CPI, D("0.0000005"))

    REM = [("R1", D("0.35"), D(240000)), ("R3", D("0.25"), D(320000)),
           ("R5", D("0.30"), D(-120000))]
    RMEAN = sum(p * i for _, p, i in REM)
    RVAR = sum(p * (1 - p) * i * i for _, p, i in REM)
    RSD = RVAR.sqrt()
    check("WE 8.4.1 remaining register EMVs sum",
          D(84000) + D(80000) - D(36000), RMEAN, D("0.005"))
    check("WE 8.4.1 remaining register mean", RMEAN, 128000)
    check("WE 8.4.1 remaining register variance", RVAR, "35328000000", D("0.6"))
    check("WE 8.4.1 remaining register sigma", RSD, 187957, D("0.6"))
    RP80 = RMEAN + Z80 * RSD
    check("WE 8.4.1 remaining register P80", RP80, 286185, D("0.6"))
    check("WE 8.4.1 contingency-adequacy shortfall", RP80 - D(120000), 166185, D("0.6"))
    check("WE 8.4.1 remaining contingency", D(300000) - D(180000), 120000)
    # INVARIANT: the adequacy test fails independently of any EAC
    check("8.4.1 invariant: adequacy fails even on the in-balance EAC",
          D(1) if (RP80 > D(120000) and AVAIL - (E1 - AC) > 0) else D(0), 1)

    # sanction register (PML-AI D8, KA 8.2.2/8.2.4) — cited, and reproduced to prove the citation
    SANC = [(D("0.35"), D(240000)), (D("0.50"), D(180000)), (D("0.25"), D(320000)),
            (D("0.15"), D(400000)), (D("0.30"), D(-120000))]
    SMEAN = sum(p * i for p, i in SANC)
    SSD = sum(p * (1 - p) * i * i for p, i in SANC).sqrt()
    check("WE 8.4.1 sanction register mean (PML-AI D8)", SMEAN, 278000)
    check("WE 8.4.1 sanction register sigma (PML-AI D8)", SSD, 252642, D("0.6"))
    check("WE 8.4.1 sanction register P80 (PML-AI D8)", SMEAN + Z80 * SSD, 490624, D("0.6"))
    check("WE 8.4.1 the 300,000 funded was a P53.5 provision",
          _phi(D, (D(300000) - SMEAN) / SSD) * 100, "53.5", D("0.05"))
    OVER = E2 - BAC
    check("WE 8.4.1 overrun on the BAC/CPI forecast", OVER, 416667, D("0.6"))
    check("WE 8.4.1 the overrun is a P70.8 event",
          _phi(D, (OVER - SMEAN) / SSD) * 100, "70.8", D("0.05"))
    # INVARIANT: the shortfall was predictable — the overrun sits inside P80, outside P53.5
    check("8.4.1 invariant: the overrun lies between the funded percentile and P80",
          D(1) if D(300000) < OVER < SMEAN + Z80 * SSD else D(0), 1)
    check("MCQ 8.4-A option C: overrun against BAC rather than funding", OVER, 416667, D("0.6"))

    # =================================================================================
    # KA 8.4.2 — the cost of a month of slip, rebuilt from Domain 6's funding recursion
    # =================================================================================
    CSBASE = EPC + OWNERS + CONT
    check("8.4.2 certified-spend base (Domain 6)", CSBASE, 55245403)
    bal = G * (FEES + DEVCOST)
    check("8.4.2 debt drawn at financial close (Domain 6)", bal, 1848000)
    drawn = [bal]
    idc6 = D(0)
    for w in PROFILE:
        i = bal * RQ
        idc6 += i
        bal += G * (CSBASE * w / 100 + i)
        drawn.append(bal)
    check("8.4.2 Domain 6 IDC reproduced from the recursion", idc6, IDC_D6, D("0.6"))
    check("8.4.2 Domain 6 closing debt balance is the facility", bal, DEBT, D("0.6"))
    check("8.4.2 cumulative debt drawn at the end of quarter six", drawn[6], 31990655, D("0.6"))

    esc_own = [OWNERS * w / 100 * FQ ** t for t, w in enumerate(PROFILE, start=1)]
    rem_own = [sum(esc_own[k:]) for k in range(9)]
    check("8.4.2 total escalated owner-retained scope", rem_own[0], 3757004, D("0.6"))
    check("8.4.2 remaining owner-retained scope after quarter six", rem_own[6], 922906, D("0.6"))
    FM = (1 + ESCAL) ** (D(1) / 12)
    check("8.4.2 monthly escalation factor", FM, "1.00295161", D("0.000000005"))
    check("8.4.2 monthly escalation in per cent", (FM - 1) * 100, "0.2952", D("0.00005"))
    check("8.4.2 invariant: twelve monthly steps compound to the annual rate",
          FM ** 12, 1 + ESCAL, D("0.0000000005"))

    ER = rem_own[6] * (FM - 1)
    IR = drawn[6] * RATE / 12
    RR = CF / 12
    TOTM = ER + IR + RR
    check("WE 8.4.2 escalation row per month", ER, 2724, D("0.6"))
    check("WE 8.4.2 extra interest row per month", IR, 159953, D("0.6"))
    check("WE 8.4.2 deferred CFADS row per month", RR, 532000)
    check("WE 8.4.2 total economic cost per month", TOTM, 694677, D("0.6"))
    check("WE 8.4.2 escalation share", ER / TOTM * 100, "0.39", D("0.005"))
    check("WE 8.4.2 interest share", IR / TOTM * 100, "23.03", D("0.005"))
    check("WE 8.4.2 deferred-revenue share", RR / TOTM * 100, "76.58", D("0.005"))
    check("WE 8.4.2 invariant: the three shares sum to 100",
          (ER + IR + RR) / TOTM * 100, 100, D("0.0000005"))
    check("WE 8.4.2 escalation over three months", ER * 3, 8172, D("0.6"))
    check("WE 8.4.2 interest over three months", IR * 3, 479860, D("0.6"))
    check("WE 8.4.2 deferred CFADS over three months", RR * 3, 1596000)
    check("WE 8.4.2 total over three months", TOTM * 3, 2084032, D("0.6"))
    DAM = D(20000) * 30
    check("WE 8.4.2 delay damages per month", DAM, 600000)
    check("WE 8.4.2 damages over three months", DAM * 3, 1800000)
    check("WE 8.4.2 damages recovery at quarter six", DAM / TOTM * 100, "86.37", D("0.005"))
    check("WE 8.4.2 uncovered per month", TOTM - DAM, 94677, D("0.6"))
    check("WE 8.4.2 uncovered over three months", TOTM * 3 - DAM * 3, 284032, D("0.6"))
    CAPD = (ER + IR) * 3
    check("WE 8.4.2 capitalised cost components", CAPD, 488032, D("0.6"))
    d4 = DEBT + CAPD
    i4 = d4 / AF612
    check("WE 8.4.2 debt after capitalising the delay cost", d4, 42488032, D("0.6"))
    check("WE 8.4.2 instalment after capitalising", i4, "5067846.24", D("0.05"))
    check("WE 8.4.2 DSCR after capitalising", CF / i4, "1.2597", D("0.00005"))
    check("WE 8.4.2 covenant cash trigger after capitalising", COV * i4, 6081415, D("0.6"))
    check("WE 8.4.2 headroom after capitalising", CF - COV * i4, 302585, D("0.6"))
    check("WE 8.4.2 headroom fall", HEADROOM - (CF - COV * i4), 69853, D("0.6"))
    check("WE 8.4.2 headroom fall as %",
          (HEADROOM - (CF - COV * i4)) / HEADROOM * 100, "18.8", D("0.05"))
    # the value of the wrap on the escalation row
    UNW = ER * ((EPC + OWNERS) / OWNERS)
    check("WE 8.4.2 unwrapped escalation row per month", UNW, 39045, D("0.6"))
    check("WE 8.4.2 the wrap is worth per month of slip", UNW - ER, 36321, D("0.6"))
    check("MCQ 8.4-C option D: the unwrapped total", UNW + IR + RR, 730998, D("0.6"))
    check("MCQ 8.4-C option A: deferred revenue alone", RR, 532000)
    check("MCQ 8.4-C option C: the revenue row omitted", ER + IR, 162677, D("0.6"))

    # the slip-cost curve through the programme (Fig 8.4.1)
    CURVE = [("close", 11089, 9240, 552329), ("q1", 10446, 20939, 563385),
             ("q2", 9473, 38561, 580033), ("q3", 8054, 64102, 604156),
             ("q4", 6293, 95713, 634006), ("q5", 4405, 129589, 665994),
             ("q6", 2724, 159953, 694677), ("q7", 1255, 186769, 720024),
             ("COD", 0, 210000, 742000)]
    esc_seq, int_seq, tot_seq = [], [], []
    for k, (lab, e_p, i_p, t_p) in enumerate(CURVE):
        e_c = rem_own[k] * (FM - 1)
        i_c = drawn[k] * RQ / 3          # one month at 6.0% p.a. on the drawn balance
        esc_seq.append(e_c)
        int_seq.append(i_c)
        tot_seq.append(e_c + i_c + RR)
        check(f"Fig 8.4.1 {lab} escalation row", e_c, e_p, D("0.6"))
        check(f"Fig 8.4.1 {lab} interest row", i_c, i_p, D("0.6"))
        check(f"Fig 8.4.1 {lab} column total", e_c + i_c + RR, t_p, D("0.6"))
    check("8.4.2 cost of a month at financial close", tot_seq[0], 552329, D("0.6"))
    check("8.4.2 cost of a month at COD", tot_seq[-1], 742000, D("0.6"))
    check("8.4.2 COD reconciles to Domain 5: interest per day", int_seq[-1] / 30, 7000, D("0.6"))
    check("8.4.2 COD reconciles to Domain 5: forgone CFADS per day",
          RR / 30, "17733.33", D("0.005"))
    check("8.4.2 COD total per day", tot_seq[-1] / 30, "24733.33", D("0.005"))
    check("8.4.2 damages recovery at COD", DAM / tot_seq[-1] * 100, "80.86", D("0.005"))
    check("8.4.2 crossing straddles quarter two", tot_seq[2], 580033, D("0.6"))
    check("8.4.2 crossing straddles quarter three", tot_seq[3], 604156, D("0.6"))
    check("MCQ 8.4-D exposure movement across the programme",
          (tot_seq[-1] / tot_seq[0] - 1) * 100, "34.3", D("0.05"))
    # INVARIANTS of the slip curve
    check("8.4.2 invariant: the interest row rises monotonically",
          D(1) if all(int_seq[k] < int_seq[k + 1] for k in range(8)) else D(0), 1)
    check("8.4.2 invariant: the escalation row falls monotonically",
          D(1) if all(esc_seq[k] > esc_seq[k + 1] for k in range(8)) else D(0), 1)
    check("8.4.2 invariant: the total rises monotonically",
          D(1) if all(tot_seq[k] < tot_seq[k + 1] for k in range(8)) else D(0), 1)
    check("8.4.2 invariant: flat damages over-recover at close and under-recover at COD",
          D(1) if tot_seq[0] < DAM < tot_seq[-1] else D(0), 1)
    check("8.4.2 invariant: the crossing lies between q2 and q3",
          D(1) if tot_seq[2] < DAM < tot_seq[3] else D(0), 1)
    check("8.4.2 invariant: deferred revenue is the largest row at every point",
          D(1) if all(RR > esc_seq[k] + int_seq[k] for k in range(9)) else D(0), 1)
    check("8.4.2 invariant: escalation is zero at COD", esc_seq[-1], 0, D("0.005"))
    check("Fig 8.4.1 y-axis 800k accommodates the tallest column",
          D(1) if max(tot_seq) <= D(800000) else D(0), 1)

    # =================================================================================
    # Case study A — the contingency that was a percentage (transport)
    # =================================================================================
    KB = D(420000000)
    Ka, Km, Kb = KB * D("0.85"), KB, KB * D("1.30")
    check("Case A triangular minimum", Ka, 357000000)
    check("Case A triangular maximum", Kb, 546000000)
    KC = KB * D("0.08")
    check("Case A funded contingency at 8% of base", KC, 33600000)
    check("Case A triangular b - a", Kb - Ka, 189000000)
    check("Case A triangular b - m", Kb - Km, 126000000)
    KSPREAD = (Kb - Ka) * (Kb - Km)
    KP80 = Kb - (D("0.20") * KSPREAD).sqrt()
    check("Case A sqrt(0.20(b-a)(b-m))", (D("0.20") * KSPREAD).sqrt(), 69013042, D("0.6"))
    check("Case A (b-a)(b-m)", KSPREAD, "23814000000000000", D("0.6"))
    check("Case A P80 outturn", KP80, 476986958, D("0.6"))
    check("Case A P80 contingency requirement", KP80 - KB, 56986958, D("0.6"))
    check("Case A P80 contingency as % of base", (KP80 - KB) / KB * 100, "13.57", D("0.005"))
    KFUND = KB + KC
    check("Case A total funding", KFUND, 453600000)
    check("Case A b less the funded outturn", Kb - KFUND, 92400000)
    check("Case A (546-453.6)m squared", (Kb - KFUND) ** 2, "8537760000000000", D("0.6"))
    check("Case A percentile of the funded provision",
          1 - (Kb - KFUND) ** 2 / KSPREAD, "0.6415", D("0.00005"))
    check("Case A percentile of the funded provision, per cent",
          (1 - (Kb - KFUND) ** 2 / KSPREAD) * 100, "64.1", D("0.05"))
    KCPI = D(231000000) / D(252000000)
    check("Case A CPI", KCPI, "0.9167", D("0.00005"))
    KEAC = KB / KCPI
    check("Case A BAC/CPI forecast", KEAC, 458181818, D("0.6"))
    check("Case A overrun against base", KEAC - KB, 38181818, D("0.6"))
    check("Case A overrun beyond the funded contingency", KEAC - KB - KC, 4581818, D("0.6"))
    check("Case A funds available", KFUND - D(252000000), 201600000)
    check("Case A cost to complete", KEAC - D(252000000), 206181818, D("0.6"))
    check("Case A in-balance shortfall",
          (KEAC - D(252000000)) - (KFUND - D(252000000)), 4581818, D("0.6"))
    check("Case A remaining contingency", KC - D(26200000), 7400000)
    check("Case A invariant: remaining contingency below the remaining P80",
          D(1) if KC - D(26200000) < D(19300000) else D(0), 1)
    KEQ = KFUND * D("0.25")
    check("Case A original equity on a 75/25 structure", KEQ, 113400000)
    check("Case A equity uplift from the injection", D(20000000) / KEQ * 100, "17.6", D("0.05"))
    KNT = KFUND + D(20000000)
    check("Case A new total funding", KNT, 473600000)
    check("Case A gearing after the injection, debt side",
          KFUND * D("0.75") / KNT * 100, "71.8", D("0.05"))
    check("Case A gearing after the injection, equity side",
          (KEQ + D(20000000)) / KNT * 100, "28.2", D("0.05"))
    check("Case A provision was short of its own P80 by", (KP80 - KB) - KC, 23386958, D("0.6"))
    check("Case A invariant: the call approximates the computable shortfall",
          D(20000000) / ((KP80 - KB) - KC), "0.855", D("0.0005"))
    # invariant: the P80 percentage is scale-free — same range, same 13.57%
    check("Case A invariant: -15/+30 returns 13.57% at P80 on any base",
          (KP80 - KB) / KB * 100 - (TP80 - EPC) / EPC * 100, 0, D("0.0000005"))

    # =================================================================================
    # Case study B — the release nobody wanted to take (solar)
    # =================================================================================
    SB = D(148000000)
    SC = SB * D("0.075")
    check("Case B funded contingency at 7.5% of base", SC, 11100000)
    TWO = [(D("0.35"), D(6000000)), (D("0.25"), D(4800000))]
    TWOM = sum(p * i for p, i in TWO)
    TWOV = sum(p * (1 - p) * i * i for p, i in TWO)
    check("Case B two dominant items' mean", TWOM, 3300000)
    check("Case B two dominant items' sigma", TWOV.sqrt(), 3536948, D("0.6"))
    V9 = D("3.9e12")
    S9 = V9.sqrt()
    SMEAN9 = D(2940000)
    SDTOT = (TWOV + V9).sqrt()
    check("Case B register mean", SMEAN9 + TWOM, 6240000)
    check("Case B register sigma", SDTOT, 4050926, D("0.6"))
    check("Case B register P80", (SMEAN9 + TWOM) + Z80 * SDTOT, 9649259, D("0.6"))
    check("Case B two items' share of total variance", TWOV / (TWOV + V9) * 100,
          "76.2", D("0.05"))
    check("Case B remaining nine mean", SMEAN9, 2940000)
    check("Case B remaining nine sigma", S9, 1974842, D("0.6"))
    SP80B = SMEAN9 + Z80 * S9
    check("Case B P80 after the two items retire", SP80B, 4602027, D("0.6"))
    SP80A = (SMEAN9 + TWOM) + Z80 * SDTOT
    check("Case B fall in the P80 requirement", SP80A - SP80B, 5047232, D("0.6"))
    check("Case B invariant: the fall is more than half the requirement",
          D(1) if (SP80A - SP80B) / SP80A > D("0.50") else D(0), 1)
    UND = SC - D(1800000)
    check("Case B undrawn commitment", UND, 9300000)
    check("Case B excess over the live P80", UND - SP80B, 4697973, D("0.6"))
    check("Case B commitment fee on the excess", (UND - SP80B) * D("0.006"), 28188, D("0.6"))
    REQ = SP80B * D("1.15")
    check("Case B reset requirement with a 15% correlation uplift", REQ, 5292331, D("0.6"))
    check("Case B commitment cancelled", UND - REQ, 4007669, D("0.6"))
    check("Case B fee saving over eighteen months",
          (UND - REQ) * D("0.006") * D(18) / 12, 36069, D("0.6"))
    DRAW = (UND - REQ) * D("0.70")
    check("Case B senior debt had the excess been drawn", DRAW, 2805368, D("0.6"))
    AF5415 = af(D("0.054"), 15)
    check("Case B AF(5.4%,15)", AF5415, "10.104624", D("0.0000005"))
    check("Case B additional annual debt service", DRAW / AF5415, 277632, D("0.6"))
    # INVARIANT: the asymmetry that makes a mechanical release the answer
    check("Case B invariant: annual drawn cost exceeds the whole fee saving",
          D(1) if DRAW / AF5415 > (UND - REQ) * D("0.006") * D(18) / 12 else D(0), 1)
    check("Case B invariant: the 15% uplift raises the requirement above the independent P80",
          REQ - SP80B, "690304", D("0.6"))

    # =================================================================================
    # Calculation exercises
    # =================================================================================
    # Exercise 8.1
    X1 = D(30000000)
    x1a, x1m, x1b = X1 * D("0.85"), X1, X1 * D("1.30")
    check("Ex 8.1 triangular a", x1a, 25500000)
    check("Ex 8.1 triangular b", x1b, 39000000)
    check("Ex 8.1 triangular b - a", x1b - x1a, 13500000)
    check("Ex 8.1 triangular b - m", x1b - x1m, 9000000)
    check("Ex 8.1 b less the 10%-rule outturn", x1b - X1 * D("1.10"), 6000000)
    check("Ex 8.1 mean outturn", (x1a + x1m + x1b) / 3, 31500000)
    check("Ex 8.1 mean contingency", (x1a + x1m + x1b) / 3 - X1, 1500000)
    check("Ex 8.1 mean contingency %", ((x1a + x1m + x1b) / 3 - X1) / X1 * 100, "5.00")
    x1sp = (x1b - x1a) * (x1b - x1m)
    check("Ex 8.1 (b-a)(b-m)", x1sp, "121500000000000", D("0.6"))
    check("Ex 8.1 0.20(b-a)(b-m)", D("0.20") * x1sp, "24300000000000", D("0.6"))
    check("Ex 8.1 b - x", (D("0.20") * x1sp).sqrt(), 4929503, D("0.6"))
    x1p80 = x1b - (D("0.20") * x1sp).sqrt()
    check("Ex 8.1 P80 outturn", x1p80, 34070497, D("0.6"))
    check("Ex 8.1 P80 contingency", x1p80 - X1, 4070497, D("0.6"))
    check("Ex 8.1 P80 contingency %", (x1p80 - X1) / X1 * 100, "13.57", D("0.005"))
    check("Ex 8.1 the 10% rule outturn", X1 * D("1.10"), 33000000)
    check("Ex 8.1 percentile the 10% rule buys",
          (1 - (x1b - X1 * D("1.10")) ** 2 / x1sp) * 100, "70.37", D("0.005"))
    # INVARIANT: both answers are scale-free functions of the range alone
    check("Ex 8.1 invariant: P80 % is independent of base",
          (x1p80 - X1) / X1 * 100 - (TP80 - EPC) / EPC * 100, 0, D("0.0000005"))
    check("Ex 8.1 invariant: the 10%-rule percentile is independent of base",
          (1 - (x1b - X1 * D("1.10")) ** 2 / x1sp) * 100
          - (1 - (b - (EPC + TEN)) ** 2 / SPREAD) * 100, 0, D("0.0000005"))

    # Exercise 8.2
    S2, G2, RQ2 = D(30000000), D("0.60"), D("0.02")
    P2 = [D(w) for w in (10, 15, 20, 25, 18, 12)]
    R2 = [D(w) for w in (12, 18, 25, 20, 15, 10)]
    check("Ex 8.2 planned profile sums to 100", sum(P2), 100)
    check("Ex 8.2 reversed profile sums to 100", sum(R2), 100)
    C2F = RQ2 * G2 * S2
    check("Ex 8.2 area-rule coefficient", C2F, 360000)
    check("Ex 8.2 planned area", area(P2), "2.3800", D("0.00005"))
    check("Ex 8.2 reversed area", area(R2), "2.6200", D("0.00005"))
    check("Ex 8.2 planned IDC", C2F * area(P2), 856800)
    check("Ex 8.2 reversed IDC", C2F * area(R2), 943200)
    check("Ex 8.2 difference on identical spend and duration",
          C2F * (area(R2) - area(P2)), 86400)
    check("Ex 8.2 closing-balance error as % of the correct figure",
          C2F / (C2F * area(P2)) * 100, "42.02", D("0.005"))
    # INVARIANT: closing-balance convention adds exactly one coefficient's worth
    bal2 = idc2 = D(0)
    for w in P2:
        bal2 += G2 * (S2 * w / 100)
        idc2 += bal2 * RQ2
    check("Ex 8.2 invariant: closing-balance IDC exceeds opening by exactly r_q x g x S",
          idc2 - C2F * area(P2), C2F, D("0.005"))

    # Exercise 8.3
    R3 = [(D("0.30"), D(1500000), 450000, "472500000000"),
          (D("0.45"), D(800000), 360000, "158400000000"),
          (D("0.20"), D(2200000), 440000, "774400000000"),
          (D("0.25"), D(1000000), 250000, "187500000000"),
          (D("0.20"), D(-500000), -100000, "40000000000")]
    for p, i, emv, v in R3:
        check(f"Ex 8.3 EMV at p={p}", p * i, emv)
        check(f"Ex 8.3 variance contribution at p={p}", p * (1 - p) * i * i, v, D("0.6"))
    M3 = sum(p * i for p, i, _, _ in R3)
    V3 = sum(p * (1 - p) * i * i for p, i, _, _ in R3)
    S3 = V3.sqrt()
    check("Ex 8.3 mean", M3, 1400000)
    check("Ex 8.3 variance", V3, "1632800000000", D("0.6"))
    check("Ex 8.3 sigma", S3, 1277811, D("0.6"))
    check("Ex 8.3 P80", M3 + Z80 * S3, 2475405, D("0.6"))
    R3R = D(22000000) * D("0.08")
    check("Ex 8.3 the 8% rule", R3R, 1760000)
    check("Ex 8.3 z of the 8% rule", (R3R - M3) / S3, "0.2817", D("0.00005"))
    check("Ex 8.3 percentile the 8% rule buys", _phi(D, (R3R - M3) / S3) * 100, "61.1", D("0.05"))
    check("Ex 8.3 shortfall against P80", (M3 + Z80 * S3) - R3R, 715405, D("0.6"))
    check("Ex 8.3 opportunity variance contribution",
          D("0.20") * D("0.80") * D(500000) ** 2, "40000000000", D("0.6"))
    # INVARIANT: the opportunity lowers the mean and raises the variance
    M3X = sum(p * i for p, i, _, _ in R3[:4])
    V3X = sum(p * (1 - p) * i * i for p, i, _, _ in R3[:4])
    check("Ex 8.3 invariant: dropping the opportunity raises the mean",
          D(1) if M3X > M3 else D(0), 1)
    check("Ex 8.3 invariant: dropping the opportunity lowers the variance",
          D(1) if V3X < V3 else D(0), 1)
    check("Ex 8.3 invariant: dropping it from the variance understates the P80",
          D(1) if M3 + Z80 * V3X.sqrt() < M3 + Z80 * S3 else D(0), 1)

    # Exercise 8.4
    FM4 = (1 + D("0.042")) ** (D(1) / 12)
    check("Ex 8.4 monthly escalation factor", FM4, "1.00343438", D("0.000000005"))
    check("Ex 8.4 monthly escalation per cent", (FM4 - 1) * 100, "0.3434", D("0.00005"))
    e4 = D(2400000) * (FM4 - 1)
    i4x = D(18000000) * D("0.07") / 12
    r4 = D(4200000) / 12
    t4 = e4 + i4x + r4
    check("Ex 8.4 escalation row", e4, 8243, D("0.6"))
    check("Ex 8.4 interest row", i4x, 105000)
    check("Ex 8.4 revenue row", r4, 350000)
    check("Ex 8.4 total cost of the month", t4, 463243, D("0.6"))
    check("Ex 8.4 damages", D(12000) * 30, 360000)
    check("Ex 8.4 damages coverage", D(360000) / t4 * 100, "77.71", D("0.005"))
    check("Ex 8.4 uncovered", t4 - D(360000), 103243, D("0.6"))
    check("Ex 8.4 share a revenue-only calibration misses",
          (e4 + i4x) / t4 * 100, "24.45", D("0.005"))
    check("Ex 8.4 invariant: the two non-revenue rows are the missed share",
          (e4 + i4x) / t4 * 100 + r4 / t4 * 100, 100, D("0.0000005"))

    # Exercise 8.5
    B5, AC5, EV5, PV5 = D(30000000), D(16800000), D(15600000), D(16000000)
    CPI5, SPI5 = EV5 / AC5, EV5 / PV5
    check("Ex 8.5 CPI", CPI5, "0.928571", D("0.0000005"))
    check("Ex 8.5 SPI", SPI5, "0.975000", D("0.0000005"))
    F5 = B5 + D(2400000)
    check("Ex 8.5 total funding", F5, 32400000)
    A5 = F5 - AC5
    check("Ex 8.5 funds available", A5, 15600000)
    check("Ex 8.5 remaining work at budget", B5 - EV5, 14400000)
    e5a = AC5 + (B5 - EV5)
    check("Ex 8.5 (a) EAC", e5a, 31200000)
    check("Ex 8.5 (a) CTC", e5a - AC5, 14400000)
    check("Ex 8.5 (a) surplus", A5 - (e5a - AC5), 1200000)
    e5b = B5 / CPI5
    check("Ex 8.5 (b) EAC", e5b, 32307692, D("0.6"))
    check("Ex 8.5 (b) CTC", e5b - AC5, 15507692, D("0.6"))
    check("Ex 8.5 (b) surplus", A5 - (e5b - AC5), 92308, D("0.6"))
    e5c = AC5 + (B5 - EV5) / (CPI5 * SPI5)
    check("Ex 8.5 CPI x SPI", CPI5 * SPI5, "0.905357", D("0.0000005"))
    check("Ex 8.5 (c) EAC", e5c, 32705325, D("0.6"))
    check("Ex 8.5 (c) CTC", e5c - AC5, 15905325, D("0.6"))
    check("Ex 8.5 (c) shortfall", A5 - (e5c - AC5), -305325, D("0.6"))
    check("Ex 8.5 the (b) surplus as % of the facility", D(92308) / F5 * 100, "0.28", D("0.005"))
    check("Ex 8.5 invariant: TCPI to the (b) forecast equals CPI",
          (B5 - EV5) / (e5b - AC5), CPI5, D("0.0000005"))
    check("Ex 8.5 invariant: the three methods rank the sufficiency in reverse",
          D(1) if (e5a < e5b < e5c
                   and A5 - (e5a - AC5) > A5 - (e5b - AC5) > A5 - (e5c - AC5)) else D(0), 1)

    # =================================================================================
    # Industry variations, executive perspective and the summary
    # =================================================================================
    check("Industry variations: MRA charge as % of Kestrel CFADS", LEVEL / CF * 100,
          "10.10", D("0.005"))
    check("Industry variations: the ladder's factor", D("0.50") / D("0.08"), "6.25")
    check("Executive perspective: Kestrel undrawn excess cost", EXCESS * D("0.006"),
          11214, D("0.6"))
    check("Executive perspective: Case B undrawn excess cost",
          (UND - SP80B) * D("0.006"), 28188, D("0.6"))
    check("Executive perspective: Case B drawn cost per year", DRAW / AF5415, 277632, D("0.6"))
    check("Summary: back-loaded IDC", idcC, 1345680)
    check("Summary: S-curve IDC", idcA, 1607760)
    check("Summary: front-loaded IDC", idcB, 2000880)
    check("Summary: IDC spread", idcB - idcC, 655200)
    check("Summary: escalation runs the other way by",
          esc_spend(C) - esc_spend(B), 574146, D("0.6"))
    check("Summary: total spread", tot["B"] - tot["C"], 86681, D("0.6"))
    check("Summary: shape-neutral escalation rate", bAC * 100, "4.1659", D("0.00005"))
    check("Summary: Auriga CTCs against 2,180,000 available",
          (E1 - AC) + (E2 - AC) + (E3 - AC), D(2080000) + D(2296667) + D(2488056), D("1.2"))
