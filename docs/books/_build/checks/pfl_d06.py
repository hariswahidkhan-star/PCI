"""PFL-AI Domain 6 — Financial modelling and model governance: golden checks.

Every number printed as a result in `domain-06-financial-modelling.md` — worked-example
Substitution and Result lines, in-text computed figures, every numeric MCQ option (not only the
marked one), every exercise answer, every case-study figure and every value inside the two figure
specification blockquotes — is recomputed here from its definition rather than copied from the page.
Master-thread values come from ctx and are never re-typed: `KESTREL_CAPEX`, `KESTREL_DEBT`,
`KESTREL_EQUITY`, `KESTREL_RATE`, `KESTREL_TENOR`, `KESTREL_LIFE`, `KESTREL_INSTALMENT`,
`KESTREL_CFADS`, `KESTREL_EBITDA`, `KESTREL_EBIT`, `KESTREL_INTEREST_Y1`, `KESTREL_DISCOUNT`,
`KESTREL_NPV`.

The domain has six engines, each rebuilt from first principles:

    reconciliation  (6.1.3) Domain 4's level 15-year inflow stream is re-priced with `af`, the
                    escalation `g` that makes an EBITDA stream starting at 7,500,000 reproduce it is
                    solved by bisection and asserted to round to the printed 2.967 %, and the five
                    defensible NPVs are built from ONE post-tax unlevered cash-flow function
                    FCF(t) = (1-T)(R-C)(1+g)^(t-1) - wc(1+g)^(t-1) + T*dep, evaluated at two
                    horizons and two cases. The decomposition (basis / horizon / case) is checked
                    against the differences of the PRINTED members, since the manuscript presents it
                    as an addition a reader performs; the spread is checked both ways.
    construction    (6.2.1) The sources-and-uses fixed point is SOLVED, not asserted: capitalised
                    interest and the certified-spend base are mutually dependent (the base is the
                    envelope less fees, development and IDC), so the iteration converges on IDC and
                    the balancing contingency falls out. The four convention variants (quarterly and
                    annual x opening and average balance) are computed on the SAME uses, which is
                    what makes the 41.01 % swing a convention effect rather than a re-solved table.
                    Two invariants are pinned: cumulative debt closes on exactly 42,000,000 and
                    cumulative equity on exactly 18,000,000; and the annual/average pairing check —
                    only the coarse-timeline-plus-opening-balance combination produces the 41 %
                    error, each choice alone costing far less.
    operating       (6.2.2) The first operating year is built once and read three ways: the CFADS
                    tie (CFADS = OCF + interest paid), the balance sheet (both sides computed
                    independently and asserted equal), and closing cash asserted equal to the DSRA
                    balance. Elasticities are derived from the CFADS-on-revenue function
                    CFADS = 0.75R - 2,616,000 (itself derived from revenue, cash costs, the tax
                    computation and revenue-indexed working capital, not typed), and the ~52x
                    ratio between the distribution and coverage elasticities is checked.
    tax             (6.2.3, 6.4.1, 6.4.1b) The 15 % declining-balance allowance schedule, the
                    loss-carry-forward balance, the EBIT-instead-of-taxable-profit error and its
                    effective-rate signature, and the period-by-period DSCR profile. Monotonicity is
                    asserted as a CLAIM (bank case falls every year, sponsor case rises every year),
                    LLCR = DSCR is checked to hold on level cash and to BREAK on modelled cash, and
                    PLCR >= LLCR is checked on both.
    returns         (6.3.1, 6.3.2, 6.3.3) The amortisation invariants (closing balance nil, sum of
                    principal = drawings, interest ties to balance x rate), the waterfall including
                    two-year DSRA funding and the year-twelve release, and the project and equity
                    IRRs solved by bisection with the timing convention pinned by the equity payback.
    economics       (6.4.2, 6.4.3, 6.4.4) The sensitivity grid is generated from one NPV function
                    and one DSCR function, so every cell and every elasticity is a derivation; the
                    additivity claim is asserted as an EXACT equality (the model is linear in
                    revenue and cash cost, so one-at-a-time loses nothing arithmetically); the
                    breakeven translation inverts each ratio threshold; and the review-economics
                    engine is built once as expected_cost(fee, elapsed, p, d, correction) so the four
                    controls, the three breakevens and the ranking inequalities all come from it.

Tolerances: default 0.005 throughout. A handful of checks are named "(printed-value decomposition)"
where the manuscript deliberately reports the difference of two ROUNDED printed figures rather than
the rounded difference of two exact ones; those are checked the way a reader would perform them.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))
    Q0 = lambda x: q(x, 0)

    CAPEX = ctx["KESTREL_CAPEX"]            # 60,000,000
    DEBT = ctx["KESTREL_DEBT"]              # 42,000,000
    EQUITY = ctx["KESTREL_EQUITY"]          # 18,000,000
    R6 = ctx["KESTREL_RATE"]                # 0.06
    TEN = ctx["KESTREL_TENOR"]              # 12
    LIFE = ctx["KESTREL_LIFE"]              # 25
    INST = ctx["KESTREL_INSTALMENT"]        # 5,009,635.23 (printed, 2 dp)
    CFADS = ctx["KESTREL_CFADS"]            # 6,384,000
    EBITDA = ctx["KESTREL_EBITDA"]          # 7,500,000
    EBIT = ctx["KESTREL_EBIT"]              # 5,100,000
    INT1 = ctx["KESTREL_INTEREST_Y1"]       # 2,520,000
    R8 = ctx["KESTREL_DISCOUNT"]            # 0.08
    NPV_D4 = ctx["KESTREL_NPV"]             # +16,179,360

    REV = D(12000000)
    COST = D(4500000)
    DEP = CAPEX / LIFE                      # 2,400,000
    TAX = D("0.20")
    WC = D(600000)                           # receivables 900,000 less payables 300,000
    EPC = D(48000000)
    OWNER = D(3600000)
    DEVCOST = D(1800000)
    FEE_RATE = D("0.02")
    GEAR = D("0.70")

    # ================= master-thread values the domain restates =======================
    AF612 = af(R6, TEN)
    check("6.A instalment from 42,000,000 at 6.0 % over 12 years", q(DEBT / AF612, 2), INST)
    check("6.A year-one interest", DEBT * R6, INT1)
    check("6.A year-one principal", Q0(DEBT / AF612 - DEBT * R6), 2489635)
    check("6.A EBITDA = revenue - cash costs", REV - COST, EBITDA)
    check("6.A depreciation 60,000,000 straight line over 25 years", DEP, 2400000)
    check("6.A EBIT = EBITDA - depreciation", EBITDA - DEP, EBIT)
    check("6.A arrangement fees at 2.0 % of the facility", DEBT * FEE_RATE, 840000)
    check("6.A fees plus development funded at close", DEBT * FEE_RATE + DEVCOST, 2640000)
    check("6.A gearing 70/30 reproduces the debt", GEAR * CAPEX, DEBT)
    check("6.A gearing 70/30 reproduces the equity", (1 - GEAR) * CAPEX, EQUITY)
    check("6.A 27 model periods = 2 construction + 25 operating", 2 + LIFE, 27)
    check("6.A DSCR at close", q(CFADS / INST, 4), D("1.2743"))
    check("6.A pre-working-capital CFADS (Domain 2's other definition)", CFADS + WC, 6984000)

    # amortisation on the exact instalment (the invariants) and on the printed one (the columns)
    INSTx = DEBT / AF612
    bal, INTx, PRINx = DEBT, {}, {}
    for t in range(1, TEN + 1):
        INTx[t] = bal * R6
        PRINx[t] = INSTx - INTx[t]
        bal -= PRINx[t]
    check("6.3.1 invariant 3 — closing balance nil at maturity", bal, 0)
    check("6.3.1 invariant 4 — sum of scheduled principal = drawings",
          sum(PRINx.values()), DEBT)
    check("6.3.1 invariant — debt service = interest + scheduled principal, every period",
          1 if all(q(INTx[t] + PRINx[t], 8) == q(INSTx, 8) for t in INTx) else 0, 1)
    check("6.3.1 invariant — interest ties to the opening balance at the rate, every period",
          1 if all(q(INTx[t], 8) == q((DEBT - sum(PRINx[k] for k in range(1, t))) * R6, 8)
                   for t in INTx) else 0, 1)
    # the printed schedule uses the printed (2 dp) instalment
    bal, INT, PRIN = DEBT, {}, {}
    for t in range(1, TEN + 1):
        INT[t] = bal * R6
        PRIN[t] = INST - INT[t]
        bal -= PRIN[t]
    check("6.3.1 principal in year one", Q0(PRIN[1]), 2489635)
    check("6.3.1 principal in year twelve", Q0(PRIN[12]), 4726071)
    check("6.3.1 interest in year twelve", Q0(INT[12]), 283564)
    check("6.3.1 year twelve carries the largest principal (MCQ 6.3-B distractor D is inverted)",
          1 if PRIN[12] == max(PRIN.values()) else 0, 1)
    check("6.2.3 interest year two", Q0(INT[2]), 2370622)
    check("6.2.3 interest year three", Q0(INT[3]), 2212281)
    check("6.2.3 interest year four", Q0(INT[4]), 2044440)
    check("6.2.3 interest year five", Q0(INT[5]), 1866528)
    check("MCQ 6.2-C year-eight interest implies the printed taxable profit",
          Q0(EBIT - INT[8]), 3833856)
    check("MCQ 6.2-C year-eight cash tax", Q0(TAX * (EBIT - INT[8])), 766771)
    check("MCQ 6.2-C effective rate year one", q(D(516000) / D(2580000) * 100, 1), D("20.0"))
    check("MCQ 6.2-C effective rate year eight",
          q(D(766771) / D(3833856) * 100, 1), D("20.0"))

    # ================= ENGINE 1 — the basis/horizon/case reconciliation (6.1.3) ========
    AF815, AF825, AF812 = af(R8, 15), af(R8, LIFE), af(R8, TEN)
    check("6.1.3 AF(0.08, 15)", q(AF815, 6), D("8.559479"))
    check("6.3.3 AF(0.08, 25)", q(AF825, 6), D("10.674776"))
    PV_D4 = D(8900000) * AF815
    check("6.1.3 PV of Domain 4's inflows", Q0(PV_D4), 76179360)
    check("6.1.3 NPV 1 — pre-tax operating cash, 15 years (Domain 4)", Q0(PV_D4 - CAPEX), NPV_D4)

    def ga(g, n, r=R8):                      # PV of (1+g)^(t-1) discounted at r, t = 1..n
        return sum((1 + g) ** (t - 1) / (1 + r) ** t for t in range(1, n + 1))

    lo, hi = D("0.01"), D("0.05")            # solve g so the escalating EBITDA stream ties to D4
    for _ in range(200):
        mid = (lo + hi) / 2
        if EBITDA * ga(mid, 15) < PV_D4:
            lo = mid
        else:
            hi = mid
    check("6.1.3 implied escalation solved (rounds to the printed 2.967 %)",
          q(mid * 100, 3), D("2.967"))
    G = D("0.02967")                         # the working value the manuscript states and uses
    PV_G15 = EBITDA * ga(G, 15)
    check("6.1.3 PV of the escalating EBITDA stream at g = 2.967 %", Q0(PV_G15), 76180021)
    check("6.1.3 gap to Domain 4's figure (printed-value decomposition)",
          Q0(PV_G15) - Q0(PV_D4), 661)
    check("6.1.3 gap as a percentage of Domain 4's PV",
          q((PV_G15 - PV_D4) / PV_D4 * 100, 3), D("0.001"))

    def npv(rev=REV, cost=COST, capex=CAPEX, tax=TAX, g=G, r=R8, n=LIFE):
        """post-tax unlevered FCF: (1-T)(R-C) less revenue-indexed working capital, plus the
        depreciation shield; revenue, cash cost and working capital all indexed at g."""
        dep = capex / LIFE
        return ((1 - tax) * (rev - cost) - WC / REV * rev) * ga(g, n, r) \
            + tax * dep * af(r, n) - capex

    check("6.1.3 FCF base term reduces to 5,400,000 (the stated closed form)",
          (1 - TAX) * (REV - COST) - WC, 5400000)
    check("6.1.3 FCF shield term is 0.20 x 2,400,000", TAX * DEP, 480000)
    N2, N3, N4, N5 = npv(n=15), npv(), npv(g=D(0)), npv(g=D(0), n=15)
    check("6.1.3 NPV 2 — post-tax unlevered, 15 years, g = 2.967 %", Q0(N2), -1041835)
    check("6.1.3 NPV 3 — post-tax unlevered, 25 years, g = 2.967 %", Q0(N3), 19875251)
    check("6.1.3 NPV 4 — post-tax unlevered, 25 years, flat (bank case)", Q0(N4), 2767684)
    check("6.1.3 NPV 5 — post-tax unlevered, 15 years, flat", Q0(N5), -9670265)
    check("6.1.3 flat 25-year NPV equals 5,880,000 x AF(0.08, 25) - 60,000,000",
          N4, D(5880000) * AF825 - CAPEX)
    check("6.1.3 widest defensible spread", Q0(N3) - Q0(N5), 29545516)
    check("6.1.3 spread from the exact values agrees", Q0(N3 - N5), 29545516)
    check("6.1.3 basis moves the 15-year answer (printed-value decomposition)",
          Q0(PV_D4 - CAPEX) - Q0(N2), 17221195)
    check("6.1.3 horizon moves the post-tax answer", Q0(N3) - Q0(N2), 20917086)
    check("6.1.3 case moves the 25-year answer", Q0(N3) - Q0(N4), 17107567)
    FIVE = [PV_D4 - CAPEX, N2, N3, N4, N5]
    check("6.1.3 the spread is the full range of the five members",
          Q0(max(FIVE)) - Q0(min(FIVE)), 29545516)
    check("6.1.3 the bridge reconciles the appraisal to the 25-year post-tax figure",
          Q0(PV_D4 - CAPEX) - (Q0(PV_D4 - CAPEX) - Q0(N2)) + (Q0(N3) - Q0(N2)), Q0(N3))
    check("MCQ 6.1-A distractor A (appraisal less financing figure)",
          Q0(PV_D4 - CAPEX) - Q0(N4), 13411676)
    check("6.1.3 invariant — the five order as 15y flat < 15y g < 25y flat < pre-tax < 25y g",
          1 if N5 < N2 < N4 < PV_D4 - CAPEX < N3 else 0, 1)

    # ================= ENGINE 2 — construction funding and IDC (6.2.1) ================
    CLOSE = DEBT * FEE_RATE + DEVCOST
    PROF_Q = [D(x) / 100 for x in (6, 9, 13, 16, 17, 15, 13, 11)]
    check("6.2.1 the quarterly spend profile sums to 100 %", sum(PROF_Q), 1)
    check("6.1.2 the quarterly construction model runs eight periods", len(PROF_Q), 8)

    def idc(prof, rate, base, average):
        """capitalised interest, cumulative debt and equity for one convention on a GIVEN
        certified-spend base; draws at period end, funded 70/30 against requirement."""
        bal, eq, ints, reqs = GEAR * CLOSE, (1 - GEAR) * CLOSE, [], []
        for p in prof:
            if average:                      # interest on (opening + closing)/2, solved in closed form
                i = (rate * bal + rate * (1 - GEAR) / 2 * GEAR / (1 - GEAR) * p * base) \
                    if False else (rate * bal + rate * GEAR / 2 * p * base) / (1 - rate * GEAR / 2)
            else:
                i = bal * rate
            r = p * base + i
            ints.append(i)
            reqs.append(r)
            bal += GEAR * r
            eq += (1 - GEAR) * r
        return sum(ints), ints, reqs, bal, eq

    S = CAPEX - CLOSE                        # first guess; solve the fixed point on IDC
    for _ in range(300):
        S_new = CAPEX - CLOSE - idc(PROF_Q, R6 / 4, S, False)[0]
        if abs(S_new - S) < D("1e-20"):
            S = S_new
            break
        S = S_new
    IDCQ, INTS_Q, REQS_Q, DBAL, EBAL = idc(PROF_Q, R6 / 4, S, False)
    check("6.2.1 certified-spend base solved from the fixed point", Q0(S), 55245403)
    check("6.2.1 capitalised interest, quarterly on opening balances", Q0(IDCQ), 2114597)
    CONT = S - EPC - OWNER
    check("6.2.1 balancing contingency", Q0(CONT), 3645403)
    check("6.2.1 uses total to the envelope",
          EPC + OWNER + DEVCOST + DEBT * FEE_RATE + CONT + IDCQ, CAPEX)
    check("6.2.1 invariant — cumulative debt drawn closes on exactly 42,000,000", DBAL, DEBT)
    check("6.2.1 invariant — cumulative equity drawn closes on exactly 18,000,000", EBAL, EQUITY)
    check("6.2.1 invariant — sources equal uses", DBAL + EBAL, CAPEX)
    check("6.2.1 the balancing line is not a round number",
          1 if Q0(CONT) % 1000 != 0 else 0, 1)
    check("6.2.1 IDC as a percentage of the envelope", q(IDCQ / CAPEX * 100, 2), D("3.52"))
    check("6.2.1 contingency as a percentage of the EPC price",
          q(CONT / EPC * 100, 2), D("7.59"))
    check("6.2.1 close funding requirement", CLOSE, 2640000)
    check("6.2.1 close debt draw", GEAR * CLOSE, 1848000)
    check("6.2.1 quarter-1 certified spend", Q0(PROF_Q[0] * S), 3314724)
    check("6.2.1 quarter-1 interest", Q0(INTS_Q[0]), 27720)
    check("6.2.1 quarter-1 funding requirement", Q0(REQS_Q[0]), 3342444)
    check("6.2.1 quarter-1 debt draw", Q0(GEAR * REQS_Q[0]), 2339711)
    check("6.2.1 quarter-1 closing debt balance", Q0(GEAR * CLOSE + GEAR * REQS_Q[0]), 4187711)
    check("6.2.1 total funding requirement equals the envelope", CLOSE + sum(REQS_Q), CAPEX)

    # the three alternative conventions, priced on the SAME uses
    IDC_QAVG = idc(PROF_Q, R6 / 4, S, True)[0]
    PROF_A = [sum(PROF_Q[:4]), sum(PROF_Q[4:])]
    check("6.1.2 annual profile: 44 % then 56 %", PROF_A[0] * 100, 44)
    IDC_AOPEN = idc(PROF_A, R6, S, False)[0]
    IDC_AAVG = idc(PROF_A, R6, S, True)[0]
    check("6.2.1 IDC on average balances, quarterly", Q0(IDC_QAVG), 2427554)
    check("6.2.1 average-balance premium", Q0(IDC_QAVG - IDCQ), 312957)
    check("6.2.1 IDC on an annual timeline, opening balances", Q0(IDC_AOPEN), 1247352)
    check("6.1.2 understatement from the annual timeline", Q0(IDCQ - IDC_AOPEN), 867245)
    check("6.1.2 understatement as a percentage",
          q((IDCQ - IDC_AOPEN) / IDCQ * 100, 2), D("41.01"))
    check("6.2.1 IDC on an annual timeline, average balances", Q0(IDC_AAVG), 2481619)
    check("6.2.1 annual/average lands within this of the quarterly answer",
          Q0(IDC_AAVG - IDCQ), 367022)
    check("6.2.1 invariant — the 41 % error needs BOTH mistakes: annual/average is far closer",
          1 if abs(IDC_AAVG - IDCQ) < abs(IDCQ - IDC_AOPEN) else 0, 1)
    check("6.2.1 invariant — average-balance alone costs less than the timeline alone",
          1 if abs(IDC_QAVG - IDCQ) < abs(IDCQ - IDC_AOPEN) else 0, 1)
    check("6.2.1 propagated understatement of annual depreciation",
          Q0((IDCQ - IDC_AOPEN) / LIFE), 34690)
    check("6.2.1 PV of the tax that understatement fails to shelter",
          Q0(TAX * (IDCQ - IDC_AOPEN) / LIFE * AF825), 74061)
    check("MCQ 6.1-B distractor D (quarterly figure multiplied by four)", Q0(IDCQ) * 4, 8458388)
    check("MCQ 6.2-A distractor C (IDC omitted from the deduction)",
          CAPEX - EPC - OWNER - DEVCOST - DEBT * FEE_RATE, 5760000)
    check("MCQ 6.2-A distractor D (10 % rule of thumb on the envelope)", CAPEX / 10, 6000000)
    # Fig 6.2.1 plotted series
    FIG_SPEND = [3314724, 4972086, 7181902, 8839264, 9391718, 8286810, 7181902, 6076994]
    FIG_INT = [27720, 62816, 115682, 192307, 287138, 388766, 479860, 560308]
    FIG_DEBT = [4187711, 7712142, 12820451, 19142551, 25917751, 31990655, 37353888, 42000000]
    FIG_EQ = [1794733, 3305204, 5494479, 8203951, 11107608, 13710281, 16008809, 18000000]
    cd, ce = GEAR * CLOSE, (1 - GEAR) * CLOSE
    for k in range(8):
        cd += GEAR * REQS_Q[k]
        ce += (1 - GEAR) * REQS_Q[k]
        check(f"Fig 6.2.1 quarter-{k + 1} certified spend", Q0(PROF_Q[k] * S), FIG_SPEND[k])
        check(f"Fig 6.2.1 quarter-{k + 1} capitalised interest", Q0(INTS_Q[k]), FIG_INT[k])
        check(f"Fig 6.2.1 quarter-{k + 1} cumulative debt drawn", Q0(cd), FIG_DEBT[k])
        check(f"Fig 6.2.1 quarter-{k + 1} cumulative equity drawn", Q0(ce), FIG_EQ[k])
    check("Fig 6.2.1 the largest quarterly requirement fits the 0-10m axis",
          1 if max(REQS_Q) < D(10000000) else 0, 1)

    # ================= ENGINE 3 — the first operating year (6.2.2) ====================
    RECV, PAY = D(900000), D(300000)
    check("6.2.2 net working-capital absorption", RECV - PAY, WC)
    TP = EBIT - INT1
    check("6.2.2 taxable profit", TP, 2580000)
    CTAX = TAX * TP
    check("6.2.2 cash tax", CTAX, 516000)
    NI = TP - CTAX
    check("6.2.2 net income", NI, 2064000)
    OCF = NI + DEP - RECV + PAY
    check("6.2.2 operating cash flow", OCF, 3864000)
    check("6.2.2 invariant 6 — CFADS = operating cash flow + interest paid", OCF + INT1, CFADS)
    DSRA = INST * D(6) / 12
    check("6.2.2 six-month DSRA", Q0(DSRA), 2504818)
    FUND = DSRA / 2
    check("6.2.2 annual DSRA funding instalment", Q0(FUND), 1252409)
    DIST1 = CFADS - INST - FUND
    check("6.2.2 distributable cash, operating year one", Q0(DIST1), 121956)
    check("6.2.2 distribution as a percentage of equity contributed",
          q(DIST1 / EQUITY * 100, 2), D("0.68"))
    check("6.2.2 the 1.20x distribution test passes in year one",
          1 if CFADS / INST > D("1.20") else 0, 1)
    check("6.2.2 closing plant, net", CAPEX - DEP, 57600000)
    check("6.2.2 closing receivables", RECV, 900000)
    check("6.2.2 closing restricted cash equals the DSRA balance funded",
          Q0(OCF - PRIN[1] - DIST1), Q0(FUND))
    check("6.2.2 closing senior debt", Q0(DEBT - PRIN[1]), 39510365)
    EQC = EQUITY + NI - DIST1
    check("6.2.2 closing equity", Q0(EQC), 19942044)
    ASSETS = (CAPEX - DEP) + RECV + FUND
    LIABS = PAY + (DEBT - PRIN[1]) + EQC
    check("6.2.2 total assets", Q0(ASSETS), 59752409)
    check("6.2.2 total liabilities and equity", Q0(LIABS), 59752409)
    check("6.2.2 invariant 1 — the balance sheet balances to the cent", ASSETS, LIABS)

    # CFADS as a function of revenue: derived, not typed
    def cfads_of_rev(r):
        ebitda = r - COST
        return ebitda - TAX * (ebitda - DEP - INT1) - WC / REV * r

    check("EX 6.4 CFADS slope on revenue is 0.75",
          (cfads_of_rev(REV + 1) - cfads_of_rev(REV)), D("0.75"))
    check("EX 6.4 CFADS intercept is -2,616,000", cfads_of_rev(D(0)), -2616000)
    check("EX 6.4 the stated CFADS line reproduces the base case", cfads_of_rev(REV), CFADS)
    EL_CFADS = D("0.75") * REV / CFADS
    check("6.2.2 CFADS elasticity to revenue", q(EL_CFADS, 4), D("1.4098"))
    EL_DIST = D("0.75") * REV / DIST1
    check("6.2.2 distribution elasticity to revenue", q(EL_DIST, 1), D("73.8"))
    check("6.2.2 the equity line is about 52x more revenue-sensitive",
          q(EL_DIST / EL_CFADS, 0), 52)
    check("6.2.2 revenue that erases the first-year distribution", Q0(DIST1 / D("0.75")), 162608)
    check("6.2.2 that miss as a percentage of revenue",
          q(DIST1 / (D("0.75") * REV) * 100, 2), D("1.36"))

    # ================= ENGINE 4 — tax, coverage and the checks (6.2.3, 6.4.1) =========
    wdv, loss, rows = CAPEX, D(0), []
    for t in range(1, 6):
        allow = wdv * D("0.15")
        wdv -= allow
        tp = EBITDA - allow - INT[t] - loss
        rows.append((allow, tp))
        loss = -tp if tp < 0 else D(0)
    ALLOW = [9000000, 7650000, 6502500, 5527125, 4698056]
    LOSSCF = [4020000, 6540622, 7755403, 7826968, 6891552]
    for t in range(5):
        check(f"6.2.3 declining-balance allowance year {t + 1}", Q0(rows[t][0]), ALLOW[t])
        check(f"6.2.3 loss carried forward year {t + 1}", Q0(-rows[t][1]), LOSSCF[t])
        check(f"6.2.3 cash tax is nil in year {t + 1}", 1 if rows[t][1] < 0 else 0, 1)
    CF_DB = EBITDA - D(0) - WC
    check("6.2.3 year-one CFADS with no cash tax", CF_DB, 6900000)
    check("6.2.3 DSCR with no cash tax", q(CF_DB / INST, 4), D("1.3773"))
    check("6.2.3 coverage gained from the tax assumption",
          q(CF_DB / INST - CFADS / INST, 4), D("0.1030"))
    CAP_BASE = CFADS / D("1.30") * AF612
    CAP_DB = CF_DB / D("1.30") * AF612
    check("6.2.3 debt capacity at 1.30x, base tax assumption", Q0(CAP_BASE), 41171123)
    check("6.2.3 debt capacity at 1.30x, allowance regime", Q0(CAP_DB), 44498864)
    check("6.2.3 capacity difference", Q0(CAP_DB - CAP_BASE), 3327741)
    check("6.2.3 Domain 10's sizing gap", Q0(DEBT - CAP_BASE), 828877)
    check("6.2.3 the difference is more than four times that gap",
          q((CAP_DB - CAP_BASE) / (DEBT - CAP_BASE), 2), D("4.01"))
    check("6.2.3 invariant — the allowance regime cannot reduce capacity",
          1 if CAP_DB > CAP_BASE else 0, 1)

    # WE 6.4.1 — taxing EBIT instead of taxable profit
    TAX_ERR = TAX * EBIT
    check("6.4.1 tax as modelled (20 % of EBIT)", TAX_ERR, 1020000)
    NI_ERR = TP - TAX_ERR
    check("6.4.1 net income as modelled", NI_ERR, 1560000)
    OCF_ERR = NI_ERR + DEP - WC
    check("6.4.1 operating cash flow as modelled", OCF_ERR, 3360000)
    CF_ERR = OCF_ERR + INT1
    check("6.4.1 CFADS as modelled", CF_ERR, 5880000)
    check("6.4.1 DSCR as modelled", q(CF_ERR / INST, 4), D("1.1737"))
    check("6.4.1 the modelled DSCR breaches the 1.20x covenant",
          1 if CF_ERR / INST < D("1.20") else 0, 1)
    check("6.4.1 distributable cash as modelled", Q0(CF_ERR - INST - FUND), -382044)
    check("6.4.1 invariant 5 catches it — distributable cash is negative",
          1 if CF_ERR - INST - FUND < 0 else 0, 1)
    CAP_ERR = CF_ERR / D("1.30") * AF612
    check("6.4.1 debt capacity as modelled", Q0(CAP_ERR), 37920771)
    check("6.4.1 debt-capacity shortfall", Q0(CAP_BASE - CAP_ERR), 3250352)
    check("6.4.1 implied effective tax rate", q(TAX_ERR / TP * 100, 2), D("39.53"))
    check("6.4.1 the effective-rate check sees it, the balance sheet does not",
          1 if abs(TAX_ERR / TP - TAX) > D("0.15") else 0, 1)
    SUB = CAP_BASE - CAP_ERR
    check("6.4.1 annual cost of substituting equity at a 6-point spread",
          Q0(SUB * D("0.06")), 195021)
    check("6.4.1 present value of that cost over twelve years at 8 %",
          Q0(SUB * D("0.06") * AF812), 1469694)
    check("6.4.1 AF(0.08, 12)", q(AF812, 6), D("7.536078"))

    # WE 6.4.1b — the DSCR profile the level line hides
    def cf_bank(t):
        return EBITDA - TAX * (EBIT - (INT[t] if t <= TEN else D(0))) - WC

    def cf_spon(t):
        e = EBITDA * (1 + G) ** (t - 1)
        return e - TAX * (e - DEP - (INT[t] if t <= TEN else D(0))) - WC / REV * REV * (1 + G) ** (t - 1)

    check("6.4.1b bank-case CFADS reduces to 5,880,000 + 0.2 x interest",
          cf_bank(1), D(5880000) + TAX * INT1)
    check("6.4.1b year-one CFADS reconciles to the documented figure", cf_bank(1), CFADS)
    check("6.4.1b year-twelve cash tax", Q0(TAX * (EBIT - INT[12])), 963287)
    check("6.4.1b year-twelve CFADS", Q0(cf_bank(12)), 5936713)
    DS_B = [cf_bank(t) / INST for t in range(1, TEN + 1)]
    check("6.4.1b DSCR year one", q(DS_B[0], 4), D("1.2743"))
    check("6.4.1b DSCR year twelve (the minimum)", q(DS_B[11], 4), D("1.1851"))
    check("6.4.1b invariant — the bank-case DSCR falls monotonically",
          1 if all(DS_B[i] > DS_B[i + 1] for i in range(TEN - 1)) else 0, 1)
    check("6.4.1b the minimum is the final loan year",
          1 if min(DS_B) == DS_B[11] else 0, 1)
    check("6.4.1b the minimum breaches the 1.20x covenant",
          1 if min(DS_B) < D("1.20") else 0, 1)
    check("6.4.1b average DSCR over the loan life", q(sum(DS_B) / TEN, 4), D("1.2340"))
    check("6.4.1b the average passes while the minimum breaches",
          1 if sum(DS_B) / TEN > D("1.20") > min(DS_B) else 0, 1)
    PV_MOD = sum(cf_bank(t) / (1 + R8) ** t for t in range(1, LIFE + 1))
    PV_LEV = CFADS * AF825
    check("6.4.1b PV of the modelled CFADS at 8 % over 25 years", Q0(PV_MOD), 65315883)
    check("6.4.1b PV on the level assumption", Q0(PV_LEV), 68147771)
    check("6.4.1b overstatement of value", Q0(PV_LEV - PV_MOD), 2831888)
    check("6.4.1b overstatement as a percentage",
          q((PV_LEV - PV_MOD) / PV_MOD * 100, 2), D("4.34"))
    LLCR_M = sum(cf_bank(t) / (1 + R6) ** t for t in range(1, TEN + 1)) / DEBT
    PLCR_M = sum(cf_bank(t) / (1 + R6) ** t for t in range(1, LIFE + 1)) / DEBT
    check("6.4.1b LLCR on the modelled profile", q(LLCR_M, 4), D("1.2395"))
    check("6.4.1b PLCR on the modelled profile", q(PLCR_M, 4), D("1.8555"))
    check("6.4.1b LLCR on level cash equals DSCR (Domain 10's identity)",
          q(CFADS * AF612 / DEBT, 4), q(CFADS / INST, 4))
    check("6.4.1b PLCR on level cash", q(CFADS * af(R6, LIFE) / DEBT, 4), D("1.9431"))
    check("6.4.1b invariant — the LLCR = DSCR identity BREAKS on non-level cash",
          1 if q(LLCR_M, 4) != q(DS_B[0], 4) else 0, 1)
    check("6.4.1b invariant — PLCR >= LLCR wherever a tail exists (modelled)",
          1 if PLCR_M >= LLCR_M else 0, 1)
    check("6.4.1b invariant — PLCR >= LLCR (level case)",
          1 if CFADS * af(R6, LIFE) >= CFADS * AF612 else 0, 1)
    DS_S = [cf_spon(t) / INST for t in range(1, TEN + 1)]
    check("6.4.1b sponsor-case DSCR year twelve", q(DS_S[11], 4), D("1.5940"))
    check("6.4.1b invariant — the sponsor-case DSCR rises monotonically",
          1 if all(DS_S[i] < DS_S[i + 1] for i in range(TEN - 1)) else 0, 1)
    check("6.4.1b the sponsor case puts the minimum in year one",
          1 if min(DS_S) == DS_S[0] else 0, 1)
    check("6.4.1b the flat case is the one that finds the defect",
          1 if min(DS_B) < D("1.20") <= min(DS_S) else 0, 1)
    # Fig 6.4.1 plotted series
    FB = ["1.2743", "1.2684", "1.2621", "1.2554", "1.2483", "1.2407",
          "1.2327", "1.2243", "1.2153", "1.2058", "1.1957", "1.1851"]
    FS = ["1.2743", "1.3004", "1.3270", "1.3542", "1.3820", "1.4104",
          "1.4394", "1.4691", "1.4994", "1.5303", "1.5618", "1.5940"]
    for t in range(TEN):
        check(f"Fig 6.4.1 bank-case DSCR year {t + 1}", q(DS_B[t], 4), D(FB[t]))
        check(f"Fig 6.4.1 sponsor-case DSCR year {t + 1}", q(DS_S[t], 4), D(FS[t]))
    check("Fig 6.4.1 every plotted value fits the 1.10-1.65 axis",
          1 if D("1.10") < min(DS_B) and max(DS_S) < D("1.65") else 0, 1)

    # sizing that removes the year-twelve breach
    I_SIZED = CFADS / D("1.30")
    check("6.4.1b instalment at the properly sized debt", Q0(I_SIZED), 4910769)
    check("6.4.1b properly sized debt", Q0(I_SIZED * AF612), 41171123)
    i12s = I_SIZED * (1 - 1 / (1 + R6))
    check("6.4.1b year-twelve DSCR at the sized debt",
          q((D(5880000) + TAX * i12s) / I_SIZED, 4), D("1.2087"))
    check("6.4.1b the sized structure stays inside the covenant",
          1 if (D(5880000) + TAX * i12s) / I_SIZED > D("1.20") else 0, 1)
    I_X = D(5880000) / (D("1.20") - TAX * (1 - 1 / (1 + R6)))
    check("6.4.1b debt level at which the year-twelve minimum is exactly 1.20x",
          Q0(I_X * AF612), 41472081)
    check("6.4.1b that structure needed this much less debt", Q0(DEBT - I_X * AF612), 527919)
    check("6.4.1b the year-one 1.30x test binds ahead of the year-twelve minimum",
          1 if CAP_BASE < I_X * AF612 else 0, 1)

    # ================= ENGINE 5 — the waterfall and returns (6.3.2, 6.3.3) ============
    check("6.3.2 lock-up CFADS at 1.15x", Q0(D("1.15") * INST), 5761081)
    check("6.3.2 covenant CFADS at 1.20x", Q0(D("1.20") * INST), 6011562)
    check("6.3.2 DSRA balance held for the rest of the loan", Q0(DSRA), 2504818)

    def dist(t, g):
        d = cf_spon(t) if g else cf_bank(t)
        if t <= TEN:
            d -= INST
        if t in (1, 2):
            d -= FUND
        if t == TEN:
            d += DSRA
        return d

    DB = [dist(t, False) for t in range(1, LIFE + 1)]
    DSP = [dist(t, True) for t in range(1, LIFE + 1)]
    check("6.3.3 bank-case distribution, operating year one", Q0(DB[0]), 121956)
    check("6.3.3 bank-case distribution, operating year two", Q0(DB[1]), 92080)
    check("6.3.3 bank-case distribution, operating year three", Q0(DB[2]), 1312821)
    check("6.3.2 bank-case distribution, year eleven", Q0(DB[10]), 980580)
    check("6.3.2 bank-case distribution, year twelve (reserve release)", Q0(DB[11]), 3431895)
    check("6.3.3 post-loan distribution", Q0(DB[12]), 5880000)
    check("6.3.2 invariant — CFADS falls between years eleven and twelve",
          1 if cf_bank(12) < cf_bank(11) else 0, 1)
    check("6.3.2 invariant — yet the distribution rises, by the reserve release",
          Q0(DB[11] - DB[10]), Q0(DSRA - (cf_bank(11) - cf_bank(12))))
    check("6.3.2 invariant — no waterfall tier is negative in the bank case",
          1 if min(DB) >= 0 else 0, 1)
    check("6.3.3 total bank-case distributions over 25 years", Q0(sum(DB)), 90507502)
    check("6.3.3 total sponsor-case distributions over 25 years", Q0(sum(DSP)), 151536729)
    check("6.3.3 bank-case money multiple", q(sum(DB) / EQUITY, 3), D("5.028"))
    check("6.3.3 sponsor-case money multiple", q(sum(DSP) / EQUITY, 3), D("8.419"))
    check("6.3.3 equity draw, construction year one", Q0((1 - GEAR) * (CLOSE + sum(REQS_Q[:4]))),
          8203951)
    check("6.3.3 equity draw, construction year two", Q0((1 - GEAR) * sum(REQS_Q[4:])), 9796049)
    check("6.3.3 the two draws are the whole equity",
          (1 - GEAR) * (CLOSE + sum(REQS_Q)), EQUITY)
    FCF_FLAT = EBITDA - TAX * EBIT - WC
    check("6.3.3 unlevered post-tax free cash flow, flat", FCF_FLAT, 5880000)
    check("6.3.3 project NPV, bank case", Q0(FCF_FLAT * AF825 - CAPEX), 2767684)

    def irr(flows, lo=D("-0.5"), hi=D(1)):
        for _ in range(400):
            m = (lo + hi) / 2
            if sum(f / (1 + m) ** t for t, f in flows) > 0:
                lo = m
            else:
                hi = m
        return m

    R_PROJ = irr([(0, -CAPEX)] + [(t, FCF_FLAT) for t in range(1, LIFE + 1)])
    check("6.3.3 project IRR, unlevered post-tax", q(R_PROJ * 100, 2), D("8.54"))
    check("6.3.3 the asset barely clears the 8 % hurdle",
          1 if D("0.08") < R_PROJ < D("0.09") else 0, 1)
    E1 = (1 - GEAR) * (CLOSE + sum(REQS_Q[:4]))
    E2 = (1 - GEAR) * sum(REQS_Q[4:])
    EQ_FLOWS = lambda ds: [(0, -E1), (1, -E2)] + [(3 + i, d) for i, d in enumerate(ds)]
    R_EQ_B, R_EQ_S = irr(EQ_FLOWS(DB)), irr(EQ_FLOWS(DSP))
    check("6.3.3 equity IRR, bank case", q(R_EQ_B * 100, 2), D("9.83"))
    check("6.3.3 equity IRR, sponsor case", q(R_EQ_S * 100, 2), D("13.52"))
    check("6.3.3 leverage lifts the equity return above the project return",
          1 if R_EQ_B > R_PROJ else 0, 1)
    check("6.3.3 the escalation case is worth this many points of equity return",
          q((R_EQ_S - R_EQ_B) * 100, 2), D("3.69"))
    check("MCQ 6.3-A distractor A (appraisal less project IRR, in points)",
          q(D("12.19") - R_PROJ * 100, 2), D("3.65"))
    cum, pay = D(0), None
    for t, d in enumerate(DB, 1):
        if cum + d >= EQUITY:
            pay = t - 1 + (EQUITY - cum) / d
            break
        cum += d
    check("6.3.3 equity payback from financial close, in years", q(pay + 2, 2), D("14.67"))
    check("6.3.3 equity payback falls after the loan is repaid",
          1 if pay > TEN else 0, 1)

    # ================= ENGINE 6 — sensitivity, breakevens, review economics ===========
    SP, BK = npv(), npv(g=D(0))
    check("6.4.2 sponsor base NPV", Q0(SP), 19875251)
    check("6.4.2 bank base NPV", Q0(BK), 2767684)
    GRID = [
        ("Revenue", dict(rev=D(10800000)), dict(rev=D(13200000)),
         (7416691, 32333810, "6.27"), (-6839615, 12374983, "34.71")),
        ("Cash operating cost", dict(cost=D(4050000)), dict(cost=D(4950000)),
         (24858674, 14891827, "-2.51"), (6610603, -1075235, "-13.88")),
        ("Capital cost", dict(capex=D(54000000)), dict(capex=D(66000000)),
         (25362861, 14387640, "-2.76"), (8255295, -2719927, "-19.83")),
        ("Tax rate", dict(tax=D("0.18")), dict(tax=D("0.22")),
         (21439288, 18311213, "-0.79"), (3856511, 1678857, "-3.93")),
    ]
    for nm, dn, up, (sa, sb, sel), (ba, bb, bel) in GRID:
        check(f"6.4.2 {nm} sponsor NPV at -10 %", Q0(npv(**dn)), sa)
        check(f"6.4.2 {nm} sponsor NPV at +10 %", Q0(npv(**up)), sb)
        check(f"6.4.2 {nm} sponsor elasticity", q((npv(**up) / SP - 1) / D("0.1"), 2), D(sel))
        check(f"6.4.2 {nm} bank NPV at -10 %", Q0(npv(g=D(0), **dn)), ba)
        check(f"6.4.2 {nm} bank NPV at +10 %", Q0(npv(g=D(0), **up)), bb)
        check(f"6.4.2 {nm} bank elasticity",
              q((npv(g=D(0), **up) / BK - 1) / D("0.1"), 2), D(bel))
    check("6.4.2 interest rate leaves the sponsor NPV unchanged (unlevered)", SP, npv())
    check("6.4.2 interest-rate elasticity of unlevered value is nil", (SP / SP - 1) / D("0.1"), 0)
    check("6.4.2 escalation -10 % (g = 2.6703 %)", Q0(npv(g=G * D("0.9"))), 17852671)
    check("6.4.2 escalation +10 % (g = 3.2637 %)", Q0(npv(g=G * D("1.1"))), 21978907)
    check("6.4.2 escalation elasticity, downward",
          q((npv(g=G * D("0.9")) / SP - 1) / D("-0.1"), 2), D("1.02"))
    check("6.4.2 escalation elasticity, upward",
          q((npv(g=G * D("1.1")) / SP - 1) / D("0.1"), 2), D("1.06"))
    check("6.4.2 discount rate 7.2 %, sponsor", Q0(npv(r=D("0.072"))), 26469750)
    check("6.4.2 discount rate 8.8 %, sponsor", Q0(npv(r=D("0.088"))), 14022003)
    check("6.4.2 discount elasticity, sponsor downward",
          q((npv(r=D("0.072")) / SP - 1) / D("-0.1"), 2), D("-3.32"))
    check("6.4.2 discount elasticity, sponsor upward",
          q((npv(r=D("0.088")) / SP - 1) / D("0.1"), 2), D("-2.94"))
    check("6.4.2 discount rate 7.2 %, bank", Q0(npv(g=D(0), r=D("0.072"))), 7305980)
    check("6.4.2 discount rate 8.8 %, bank", Q0(npv(g=D(0), r=D("0.088"))), -1294646)
    check("6.4.2 discount elasticity, bank downward",
          q((npv(g=D(0), r=D("0.072")) / BK - 1) / D("-0.1"), 2), D("-16.40"))
    check("6.4.2 discount elasticity, bank upward",
          q((npv(g=D(0), r=D("0.088")) / BK - 1) / D("0.1"), 2), D("-14.68"))
    check("6.4.2 invariant — the discount row is convex, not linear",
          1 if abs((npv(r=D("0.072")) / SP - 1) / D("-0.1"))
          > abs((npv(r=D("0.088")) / SP - 1) / D("0.1")) else 0, 1)
    check("6.4.2 invariant — the same revenue move is a bigger elasticity on the thinner case",
          1 if abs((npv(g=D(0), rev=D(13200000)) / BK - 1))
          > abs((npv(rev=D(13200000)) / SP - 1)) else 0, 1)

    def dscr(t, rev=REV, cost=COST, capex=CAPEX, tax=TAX, rate=R6):
        """year-t DSCR; a capital overrun is equity-funded, so the instalment is unchanged by capex."""
        inst = DEBT / af(rate, TEN)
        b, ints = DEBT, {}
        for k in range(1, TEN + 1):
            ints[k] = b * rate
            b -= inst - ints[k]
        ebitda = rev - cost
        return (ebitda - tax * (ebitda - capex / LIFE - ints[t]) - WC / REV * rev) / inst

    check("6.4.2 base year-one DSCR reproduces 1.2743", q(dscr(1), 4), D("1.2743"))
    check("6.4.2 base minimum DSCR reproduces 1.1851", q(dscr(12), 4), D("1.1851"))
    DGRID = [("Revenue", dict(rev=D(13200000)), "1.4540", "1.3647"),
             ("Cash operating cost", dict(cost=D(4950000)), "1.2025", "1.1132"),
             ("Capital cost", dict(capex=D(66000000)), "1.2839", "1.1946"),
             ("Interest rate", dict(rate=D("0.066")), "1.2432", "1.1485"),
             ("Tax rate", dict(tax=D("0.22")), "1.2640", "1.1658")]
    for nm, kw, y1, y12 in DGRID:
        check(f"6.4.2 {nm} +10 %: bank DSCR year 1", q(dscr(1, **kw), 4), D(y1))
        check(f"6.4.2 {nm} +10 %: bank minimum DSCR", q(dscr(12, **kw), 4), D(y12))
    check("6.4.2 escalation and discount rate leave the bank DSCR untouched",
          q(dscr(1), 4), D("1.2743"))
    check("6.4.2 invariant — a capital overrun RAISES year-one coverage",
          1 if dscr(1, capex=D(66000000)) > dscr(1) else 0, 1)
    check("6.4.2 invariant — a 10 % rate rise drives the minimum below the 1.15x lock-up",
          1 if dscr(12, rate=D("0.066")) < D("1.15") else 0, 1)
    check("6.4.2 joint move: revenue -10 % and cash cost -5 %",
          Q0(npv(g=D(0), rev=D(10800000), cost=D(4275000))), -4918155)
    check("6.4.2 invariant — one-at-a-time is EXACTLY additive on this model",
          npv(g=D(0), rev=D(10800000), cost=D(4275000)),
          BK + (npv(g=D(0), rev=D(10800000)) - BK) + (npv(g=D(0), cost=D(4275000)) - BK))
    check("6.4.2 revenue -10 %: year-one DSCR", q(dscr(1, rev=D(10800000)), 4), D("1.0947"))
    check("6.4.2 that breaches both the covenant and the lock-up",
          1 if dscr(1, rev=D(10800000)) < D("1.15") else 0, 1)

    # the breakeven translation
    rev_at = lambda c: (c + D(2616000)) / D("0.75")
    for lab, mult, cf_p, rev_p, fall in (("1.30x sizing target", D("1.30"), 6512526, 12171368, "1.43"),
                                         ("1.20x covenant", D("1.20"), 6011562, 11503416, "-4.14"),
                                         ("1.15x lock-up", D("1.15"), 5761081, 11169441, "-6.92"),
                                         ("1.00x payment fails", D(1), 5009635, 10167514, "-15.27")):
        c = mult * INST
        check(f"6.4.2 {lab}: CFADS", Q0(c), cf_p)
        check(f"6.4.2 {lab}: revenue", Q0(rev_at(c)), rev_p)
        check(f"6.4.2 {lab}: change from base revenue",
              q((rev_at(c) / REV - 1) * 100, 2), D(fall))
    HEAD = CFADS - D("1.20") * INST
    check("6.4.2 Domain 10's covenant headroom in cash", Q0(HEAD), 372438)
    check("6.4.2 headroom as a percentage of CFADS", q(HEAD / CFADS * 100, 2), D("5.83"))
    check("6.4.2 the same headroom in revenue terms",
          q(q(HEAD / CFADS * 100, 2) / EL_CFADS, 2), D("4.14"))
    check("6.4.2 quoting the CFADS percentage overstates the room by this much",
          q((EL_CFADS - 1) * 100, 0), 41)
    check("MCQ 6.4-D distractor C (multiplies rather than divides by the elasticity)",
          q(q(HEAD / CFADS * 100, 2) * EL_CFADS, 2), D("8.22"))
    check("6.4.2 cash operating cost at which the covenant breaks",
          Q0((D(9984000) - D("1.20") * INST) / D("0.8")), 4965547)
    check("6.4.2 that cost as a rise on base",
          q(((D(9984000) - D("1.20") * INST) / D("0.8") / COST - 1) * 100, 2), D("10.35"))
    check("6.4.2 CFADS on cash cost: 9,984,000 - 0.8 x cost reproduces the base case",
          D(9984000) - D("0.8") * COST, CFADS)
    lo, hi = R6, D("0.12")                   # interest rate at which year-one DSCR hits 1.20x
    for _ in range(200):
        m = (lo + hi) / 2
        if (D(5880000) + D(8400000) * m) / (DEBT / af(m, TEN)) > D("1.20"):
            lo = m
        else:
            hi = m
    check("6.4.2 interest rate at which the covenant breaks", q(m * 100, 2), D("7.48"))

    # review economics (6.4.3, 6.4.4)
    DAY_CFADS = CFADS / 360
    check("6.4.3 forgone CFADS per day on 30/360", q(DAY_CFADS, 2), D("17733.33"))
    check("6.4.3 interest on drawn debt per day", DEBT * R6 / 360, 7000)
    check("6.4.3 Domain 5's total daily cost of a slip",
          q(DAY_CFADS + DEBT * R6 / 360, 2), D("24733.33"))
    FEE, ELAPSED = D(180000), 14 * DAY_CFADS
    check("6.4.3 two weeks of elapsed cost", Q0(ELAPSED), 248267)
    KORR = D(60000) + 7 * DAY_CFADS
    check("6.4.3 pre-close correction cost (rework plus one week)", Q0(KORR), 184133)
    check("6.4.3 the delay costs more than the auditor", 1 if ELAPSED > FEE else 0, 1)
    CF_MISS = CFADS - D(500000) + TAX * D(500000)
    check("6.4.3 true CFADS with the omitted maintenance provision", CF_MISS, 5984000)
    check("6.4.3 DSCR on the true CFADS", q(CF_MISS / INST, 4), D("1.1945"))
    CAP_MISS = CF_MISS / D("1.30") * AF612
    check("6.4.3 debt capacity on the true CFADS", Q0(CAP_MISS), 38591479)
    RESIZE = DEBT - CAP_MISS
    check("6.4.3 the resize the error would have forced", Q0(RESIZE), 3408521)
    AMEND = D("0.003") * DEBT
    check("6.4.3 amendment fee at 0.30 % of the facility", AMEND, 126000)
    DURESS = RESIZE * D("0.02") * AF812
    check("6.4.3 duress premium on the resized equity", Q0(DURESS), 513738)
    REOPEN = 70 * (DAY_CFADS + DEBT * R6 / 360)
    check("6.4.3 ten-week reopening cost", Q0(REOPEN), 1731333)
    C = AMEND + D(320000) + DURESS + REOPEN
    check("6.4.3 cost if the error reaches close", Q0(C), 2691071)
    P, DET = D("0.35"), D("0.85")

    def ecost(fee, elapsed, p, d, korr):
        return fee + elapsed + p * (d * korr + (1 - d) * C)

    NOTHING = P * C
    check("6.4.3 expected cost without any control", Q0(NOTHING), 941875)
    LATE = ecost(FEE, ELAPSED, P, DET, KORR)
    check("6.4.3 expected cost with the late audit", Q0(LATE), 624328)
    check("6.4.3 net value of the late audit", Q0(NOTHING - LATE), 317547)
    check("6.4.3 breakeven fee", Q0(FEE + (NOTHING - LATE)), 497547)
    check("6.4.3 breakeven fee as a multiple of the fee charged",
          q((FEE + (NOTHING - LATE)) / FEE, 2), D("2.76"))
    check("6.4.3 breakeven error rate, late",
          q((FEE + ELAPSED) / (DET * (C - KORR)) * 100, 2), D("20.10"))
    check("6.4.3 breakeven detection rate, late",
          q((FEE + ELAPSED) / (P * (C - KORR)) * 100, 2), D("48.81"))
    EARLY = ecost(FEE, D(0), P, DET, D(60000))
    check("6.4.3 expected cost with the early audit", Q0(EARLY), 339131)
    check("6.4.3 net value of the early audit", Q0(NOTHING - EARLY), 602744)
    check("6.4.3 breakeven error rate, early",
          q(FEE / (DET * (C - D(60000))) * 100, 2), D("8.05"))
    check("6.4.3 invariant — moving the audit early nearly doubles its value",
          1 if (NOTHING - EARLY) > D("1.8") * (NOTHING - LATE) else 0, 1)
    PRE = ecost(D(8000), D(0), P, D("0.55"), D(60000))
    check("6.4.4 expected cost with the AI pre-check alone", Q0(PRE), 443394)
    check("6.4.4 net value of the AI pre-check alone", Q0(NOTHING - PRE), 498481)
    RESID = P * D("0.45")
    check("6.4.4 residual error probability after the pre-check", RESID, D("0.1575"))
    BOTH = D(8000) + P * D("0.55") * D(60000) + ecost(FEE, D(0), RESID, DET, D(60000))
    check("6.4.4 expected cost with pre-check plus early audit", Q0(BOTH), 271159)
    check("6.4.4 net value of pre-check plus early audit", Q0(NOTHING - BOTH), 670716)
    check("6.4.4 invariant — the combination ranks first on expected value",
          1 if BOTH < EARLY < PRE < LATE < NOTHING else 0, 1)
    check("6.4.4 invariant — expected value ranks the pre-check above the LATE audit",
          1 if (NOTHING - PRE) > (NOTHING - LATE) else 0, 1)

    # ================= case studies ===================================================
    check("Case A appraisal NPV carried into the financing paper", Q0(PV_D4 - CAPEX), NPV_D4)
    check("Case A same asset, post-tax, 25 years, flat", Q0(N4), 2767684)
    check("Case A equity cure to restore 1.20x in year eleven",
          Q0(D("1.20") * INST - cf_bank(11)), 21347)
    check("Case A equity cure to restore 1.20x in year twelve",
          Q0(D("1.20") * INST - cf_bank(12)), 74849)
    check("Case A total cure cost",
          Q0(sum(D("1.20") * INST - cf_bank(t) for t in (11, 12))), 96196)
    check("Case A the cure is small relative to the resize it substitutes for",
          1 if sum(D("1.20") * INST - cf_bank(t) for t in (11, 12)) < DEBT - CAP_BASE else 0, 1)
    # Case study B — toll road
    BCAP, BDEBT, BR, BN = D(420000000), D(294000000), D("0.065"), 18
    check("Case B gearing is 70 %", BDEBT / BCAP * 100, 70)
    AFB = af(BR, BN)
    check("Case B AF(0.065, 18)", q(AFB, 6), D("10.432466"))
    BDS = BDEBT / AFB
    check("Case B level debt service", Q0(BDS), 28181255)
    check("Case B DSCR on the original forecast", q(D(38000000) / BDS, 4), D("1.3484"))
    check("Case B DSCR comfortably inside the 1.35x sizing target",
          1 if D(38000000) / BDS < D("1.35") else 0, 1)
    BCF = D(38000000) * D("0.93")
    check("Case B CFADS on the forecast 7 % lower", BCF, 35340000)
    check("Case B DSCR on the revised forecast", q(BCF / BDS, 4), D("1.2540"))
    check("Case B the revised DSCR breaches the 1.30x covenant",
          1 if BCF / BDS < D("1.30") else 0, 1)
    check("Case B shortfall against the sizing target", q(D("1.35") - BCF / BDS, 3), D("0.096"))
    BCAPY = BCF / D("1.35") * AFB
    check("Case B debt capacity at 1.35x on the revised forecast", Q0(BCAPY), 273098787)
    check("Case B the facility had to fall by", Q0(BDEBT - BCAPY), 20901213)
    check("Case B gearing after the resize", q(BCAPY / BCAP * 100, 1), D("65.0"))
    check("Case B AF(0.08, 18)", q(af(R8, 18), 6), D("9.371887"))
    check("Case B present value of the equity substitution",
          Q0((BDEBT - BCAPY) * D("0.055") * af(R8, 18)), 10773610)
    BDAY = D(38000000) / 360
    check("Case B forgone CFADS per day", q(BDAY, 2), D("105555.56"))
    check("Case B six weeks of delay", Q0(42 * BDAY), 4433333)
    check("Case B total cost of finding it before close", Q0(42 * BDAY + D(900000)), 5333333)
    BDAYC = BDAY + BDEBT * BR / 360
    check("Case B daily cost with drawn debt", Q0(BDAYC), 158639)
    check("Case B amendment fee at 0.30 %", D("0.003") * BDEBT, 882000)
    BLATE = 84 * BDAYC + D("0.003") * BDEBT + D(1400000)
    check("Case B cost had the error reached close", Q0(BLATE), 15607667)
    check("Case B the audit's timing avoided", Q0(BLATE - (42 * BDAY + D(900000))), 10274333)
    check("Case B invariant — later is dearer",
          1 if BLATE > 42 * BDAY + D(900000) else 0, 1)

    # ================= exercises ======================================================
    # 6.1 — a 40,000,000 envelope at 7.0 % over five construction quarters
    E_ENV, E_DEBT, E_EQ = D(40000000), D(26000000), D(14000000)
    check("EX 6.1 gearing 65/35 reproduces the debt", D("0.65") * E_ENV, E_DEBT)
    check("EX 6.1 gearing 65/35 reproduces the equity", D("0.35") * E_ENV, E_EQ)
    E_FEES = E_DEBT * D("0.0175")
    check("EX 6.1 arrangement fees at 1.75 %", E_FEES, 455000)
    E_CLOSE = E_FEES + D(1200000)
    E_PROF = [D(x) / 100 for x in (10, 20, 25, 25, 20)]
    check("EX 6.1 the spend profile sums to 100 %", sum(E_PROF), 1)

    def e_idc(base):
        bal, ints, reqs = D("0.65") * E_CLOSE, [], []
        for p in E_PROF:
            i = bal * (D("0.07") / 4)
            r = p * base + i
            ints.append(i)
            reqs.append(r)
            bal += D("0.65") * r
        return sum(ints), bal, reqs

    ES = E_ENV - E_CLOSE
    for _ in range(300):
        nxt = E_ENV - E_CLOSE - e_idc(ES)[0]
        if abs(nxt - ES) < D("1e-20"):
            ES = nxt
            break
        ES = nxt
    E_IDC, E_BAL, E_REQS = e_idc(ES)
    check("EX 6.1 capitalised interest", Q0(E_IDC), 849752)
    E_CONT = E_ENV - D(33000000) - D(2500000) - D(1200000) - E_FEES - E_IDC
    check("EX 6.1 balancing contingency", Q0(E_CONT), 1995248)
    check("EX 6.1 contingency as a percentage of the EPC price",
          q(E_CONT / D(33000000) * 100, 2), D("6.05"))
    check("EX 6.1 that percentage is inside the stated 5-10 % band",
          1 if D(5) < E_CONT / D(33000000) * 100 < D(10) else 0, 1)
    check("EX 6.1 total uses equal the envelope",
          D(33000000) + D(2500000) + D(1200000) + E_FEES + E_CONT + E_IDC, E_ENV)
    check("EX 6.1 debt drawn closes on 26,000,000", E_BAL, E_DEBT)
    check("EX 6.1 equity drawn closes on 14,000,000",
          D("0.35") * (E_CLOSE + sum(E_REQS)), E_EQ)
    # 6.2 — operating cash flow to CFADS
    OCF2 = D(3680000) + D(2500000) - D(1100000) + D(400000)
    check("EX 6.2 operating cash flow", OCF2, 5480000)
    check("EX 6.2 CFADS", OCF2 + D(2100000), 7580000)
    check("EX 6.2 common error (interest deducted a second time)", OCF2 - D(2100000), 3380000)
    # 6.3 — a 30,000,000 facility at 5.5 % over 10 years
    AF3 = af(D("0.055"), 10)
    check("EX 6.3 AF(0.055, 10)", q(AF3, 6), D("7.537626"))
    I3 = D(30000000) / AF3
    check("EX 6.3 instalment", Q0(I3), 3980033)
    b3, DS3 = D(30000000), []
    for t in range(1, 11):
        i3 = b3 * D("0.055")
        tax3 = D("0.25") * (D(6000000) - D(1800000) - i3)
        cf3 = D(6000000) - tax3 - D(250000)
        DS3.append(cf3 / I3)
        if t == 1:
            check("EX 6.3 year-1 interest", i3, 1650000)
            check("EX 6.3 year-1 cash tax", tax3, 637500)
            check("EX 6.3 year-1 CFADS", cf3, 5112500)
            check("EX 6.3 year-1 DSCR", q(cf3 / I3, 4), D("1.2845"))
        if t == 5:
            check("EX 6.3 year-5 interest", Q0(i3), 1093531)
            check("EX 6.3 year-5 cash tax", Q0(tax3), 776617)
            check("EX 6.3 year-5 CFADS", Q0(cf3), 4973383)
            check("EX 6.3 year-5 DSCR", q(cf3 / I3, 4), D("1.2496"))
        if t == 10:
            check("EX 6.3 year-10 interest", Q0(i3), 207490)
            check("EX 6.3 year-10 cash tax", Q0(tax3), 998128)
            check("EX 6.3 year-10 CFADS", Q0(cf3), 4751872)
            check("EX 6.3 year-10 DSCR", q(cf3 / I3, 4), D("1.1939"))
        b3 -= I3 - i3
    check("EX 6.3 the schedule closes to zero", Q0(b3), 0)
    check("EX 6.3 minimum DSCR is the final year", 1 if min(DS3) == DS3[9] else 0, 1)
    check("EX 6.3 invariant — coverage falls every year on flat revenue",
          1 if all(DS3[i] > DS3[i + 1] for i in range(9)) else 0, 1)
    # 6.4 — the breakeven translation, reconciled to Domain 10's headroom
    check("EX 6.4 covenant cash trigger", Q0(D("1.20") * INST), 6011562)
    check("EX 6.4 revenue at the trigger", Q0(rev_at(D("1.20") * INST)), 11503416)
    check("EX 6.4 fall as a percentage of revenue",
          q((rev_at(D("1.20") * INST) / REV - 1) * 100, 2), D("-4.14"))
    check("EX 6.4 elasticity from 0.75 x 12,000,000 / 6,384,000", q(EL_CFADS, 4), D("1.4098"))
    check("EX 6.4 5.83 % divided by the elasticity reconciles",
          q(D("5.83") / EL_CFADS, 2), D("4.14"))
    # 6.5 — review economics on other parameters
    check("EX 6.5 expected cost without the audit", D("0.28") * D(3400000), 952000)
    W5 = D(210000) + D(300000) + D("0.28") * (D("0.8") * D(90000) + D("0.2") * D(3400000))
    check("EX 6.5 expected cost with the audit", W5, 720560)
    check("EX 6.5 net value", D("0.28") * D(3400000) - W5, 231440)
    check("EX 6.5 breakeven error rate",
          q(D(510000) / (D("0.8") * (D(3400000) - D(90000))) * 100, 2), D("19.26"))
    W5B = D(210000) + D("0.28") * (D("0.8") * D(90000) + D("0.2") * D(3400000))
    check("EX 6.5 common error (elapsed cost omitted): net value",
          D("0.28") * D(3400000) - W5B, 531440)
    check("EX 6.5 common error: breakeven error rate",
          q(D(210000) / (D("0.8") * (D(3400000) - D(90000))) * 100, 2), D("7.93"))
    check("EX 6.5 the omission flatters the control by more than twice",
          1 if (D("0.28") * D(3400000) - W5B) > 2 * (D("0.28") * D(3400000) - W5) else 0, 1)
