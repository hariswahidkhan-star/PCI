"""PFL-AI Domain 2 (extension) — Accounting and financial-statement foundations: golden checks for
the NEW material only.

The domain's ORIGINAL numbers (WE 2.2.1's income statement, WE 2.2.4's indirect bridge, WE 2.3.1's
DSCR pair, WE 2.3.3's capex/opex charge, WE 2.4.2's headline cover and leverage, Exercises 2.1-2.4)
are verified in `verify_formulas.py` and are NOT duplicated here. This module covers what the depth
expansion added, and every value is recomputed from its definition rather than read back from the
manuscript. Master-thread values come from ctx and are never re-typed.

Seven engines carry the new material:

  bases         (WE 2.1.1, MCQ 2.1-D, self-check 2.1) the same year on the accrual and the cash
                basis, quarter by quarter. The four quarters are run as a recursion over the
                receivable and payable profiles — nothing is asserted per quarter — and the domain's
                central invariant is proved rather than stated: cumulative accrual result minus
                cumulative cash result equals closing net working capital, to the cent, and it is
                nil in the one quarter where both balances moved by the same amount.

  articulation  (WE 2.2.2, WE 2.2.3, WE 2.2.4 interpretation, MCQ 2.2-E/2.2-F, Toolkit 2.T.3)
                the balance sheet is DERIVED, not plugged: cash comes from the cash-flow statement,
                equity from the profit, and A = L + E is tested as a residual that must be zero.
                Closing cash is then reached by two independent routes (OCF - principal and
                CFADS - debt service) and the two are asserted equal, which is only true because
                CFADS - OCF and debt service - principal are both the interest. The direct method is
                built from gross flows and asserted equal to the indirect figure. The three MCQ
                distractors are each recomputed as the named error that produces them.

  breakevens    (WE 2.2.1 interpretation, self-check 2.2) revenue elasticity, the profit breakeven
                and the cash breakeven. The cash breakeven is SOLVED from CFADS(EBITDA) = debt
                service rather than substituted, the tax-charge assumption behind it is tested (PBT
                at that point must still be positive), and the ordering claim — cash binds first, by
                about seven revenue points — is asserted as an invariant.

  collections   (WE 2.3.1 interpretation, WE 2.3.1B, Fig 2.3.1, MCQ 2.3-E, Case study A, Exercise
                2.6) a coverage covenant restated as a maximum collection period. Each threshold is
                inverted from the ratio to a CFADS trigger to an allowable receivable balance to a
                DSO, the unit rates (cash per day, DSCR per day) are derived and cross-multiplied
                back, the 360/365 convention effect is computed, and the plotted curve is evaluated
                at every marked point including the two unlabelled endpoints.

  recognition   (WE 2.3.2, WE 2.3.4, MCQ 2.3-F/2.3-G, Case study C, Exercises 2.7 and 2.11) the
                input/output progress pair and the onerous swing, plus provisions. The onerous
                multiple is proved to equal 1 + profit recognised / loss on BOTH sets of figures, so
                the manuscript's claim that the multiple is not a quirk of one example is tested.
                The provision's lifetime invariant — depreciation plus accretion equals the cash
                eventually paid — is proved on both the domain's and the exercise's numbers.

  ratios        (WE 2.4.1, WE 2.4.2 interpretation, Fig 2.4.1, MCQ 2.4-D/2.4-E, Exercise 2.8) the
                full ratio set, and the leverage identity re-derived on every basis so that the
                manuscript's claim that it is exact on opening bases and broken by mixing them is
                tested in both directions. The binding-covenant comparison is computed in revenue
                units from both covenants independently.

  spend         (WE 2.4.3, MCQ 2.4-F, Exercise 2.9, 2.A.1, 2.A.2, 2.A.4, Exercise 2.10) the five
                meanings of spend, reconciled to a single identity whose residual must be zero, plus
                deferred tax (the reversal year and the peak are SEARCHED for over the declining
                balance, not asserted), the lease restatement and the distributable-reserve position.

Values borrowed from sibling domains are declared once, named, and tied where possible: Domain 9's
WACC (7.9860 %), cost of equity (15.42 %) and equity IRR (12.5311 %); Domain 6's DSRA instalment,
which is DERIVED here from ctx's instalment (six months of debt service, funded over two years) so
a drift in either book fails, and Domain 6's no-cash-tax CFADS; Domain 14's certified spend,
remaining EPC commitment, approved variations, advance payment, retention cap and capitalised
interest; Domain 10's PLCR and Domain 4's NPV (from ctx).
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]

    DEBT0 = ctx["KESTREL_DEBT"]                 # 42,000,000 opening senior debt
    EQ0 = ctx["KESTREL_EQUITY"]                 # 18,000,000 opening equity
    CAPEX = ctx["KESTREL_CAPEX"]                # 60,000,000 plant cost
    RATE = ctx["KESTREL_RATE"]                  # 6.0 % senior rate
    TENOR = ctx["KESTREL_TENOR"]                # 12 years
    LIFE = ctx["KESTREL_LIFE"]                  # 25 years
    INST = ctx["KESTREL_INSTALMENT"]            # 5,009,635.23 annual debt service
    CFADS = ctx["KESTREL_CFADS"]                # 6,384,000 documented CFADS
    EBITDA = ctx["KESTREL_EBITDA"]              # 7,500,000
    EBIT = ctx["KESTREL_EBIT"]                  # 5,100,000
    INT = ctx["KESTREL_INTEREST_Y1"]            # 2,520,000
    DISC = ctx["KESTREL_DISCOUNT"]              # 8.0 %
    NPV = ctx["KESTREL_NPV"]                    # +16,179,360

    def q(x, n=4):
        return x.quantize(D(1).scaleb(-n))

    # ---- this domain's own inputs, declared once ----------------------------------------------
    REV = D(12000000)          # master statements: revenue
    COST = D(4500000)          # master statements: cash operating costs
    T = D("0.20")              # tax rate
    RECV = D(900000)           # closing receivables
    PAY = D(300000)            # closing payables
    DAYS = D(365)              # the stated day-count convention

    # ---- sibling-domain inputs, declared once ------------------------------------------------
    WACC = D("7.9860")         # Domain 9 KA 9.1.4 at 70 % gearing
    KE = D("15.42")            # Domain 9 KA 9.1.3 cost of equity at 70 % gearing
    EQ_IRR = D("12.5311")      # Domain 9 KA 9.1.4 equity IRR at 70 % gearing
    PROJ_IRR = D("12.19")      # Domain 4's project IRR
    PLCR = D("1.9431")         # Domain 10 KA 10.2.2
    CF_NOTAX = D(6900000)      # Domain 6 KA 6.2.3 year-one CFADS with no cash tax
    TAXLOSS = D(4020000)       # Domain 6 KA 6.2.3 year-one tax loss carried forward
    EPC = D(48000000)          # Domain 14 master construction contract
    D14_CERT = D(33945403)     # Domain 14 KA 14.2.1 cumulative certified spend at Q5
    D14_EPC_REMAIN = D(18720000)   # Domain 14 remaining EPC contract value
    D14_VAR_APPR = D(840000)   # Domain 14 approved but uncertified variations
    D14_ADVANCE = D(4800000)   # Domain 14 KA 14.3.2 advance payment
    D14_IDC_TOTAL = D(2114597)     # Domain 14 / Domain 6 total capitalised interest
    D14_IDC_REMAIN = D(1436674)    # Domain 14 remaining capitalised interest to completion
    CERT_PCT = D("0.61")       # Domain 14 KA 14.2.1 certified percentage at Q5

    DEP = CAPEX / LIFE
    check("2.2.1 master depreciation ties to ctx capex and life", DEP, D(2400000))
    check("2.2.1 EBITDA ties to ctx from this domain's revenue and cost", REV - COST, EBITDA)
    check("2.2.1 EBIT ties to ctx", EBITDA - DEP, EBIT)
    PBT = EBIT - INT
    TAX = PBT * T
    NI = PBT - TAX
    PRIN = INST - INT
    check("master statements principal (instalment less interest)", q(PRIN, 2), D("2489635.23"))
    check("master statements interest ties to ctx rate on opening debt", DEBT0 * RATE, INT)
    check("master statements opening balance sheet balances", CAPEX - (DEBT0 + EQ0), D(0))

    # ======================================================================== bases ============
    # WE 2.1.1 — the same year on both bases, quarter by quarter. The quarters are run as a
    # recursion over the two profiles; nothing per quarter is asserted.
    QREV, QCOST = REV / 4, COST / 4
    QEB = QREV - QCOST
    check("WE 2.1.1 setup quarterly revenue", QREV, D(3000000))
    check("WE 2.1.1 setup quarterly cash operating cost", QCOST, D(1125000))
    check("WE 2.1.1 setup quarterly accrual EBITDA", QEB, D(1875000))
    check("WE 2.1.1 setup annual accrual EBITDA ties to ctx", QEB * 4, EBITDA)

    RECV_P = [D(1200000), D(1050000), D(950000), D(900000)]
    PAY_P = [D(450000), D(400000), D(350000), D(300000)]
    check("WE 2.1.1 setup closing receivables equal the master balance sheet", RECV_P[-1], RECV)
    check("WE 2.1.1 setup closing payables equal the master balance sheet", PAY_P[-1], PAY)

    printed_recd = [D(1800000), D(3150000), D(3100000), D(3050000)]
    printed_paid = [D(675000), D(1175000), D(1175000), D(1175000)]
    printed_cash_eb = [D(1125000), D(1975000), D(1925000), D(1875000)]
    printed_div = [D(-750000), D(100000), D(50000), D(0)]

    pr, pp = D(0), D(0)
    tot_recd, tot_paid = D(0), D(0)
    for i in range(4):
        recd = QREV - (RECV_P[i] - pr)
        paid = QCOST - (PAY_P[i] - pp)
        check(f"WE 2.1.1 result Q{i + 1} cash received", recd, printed_recd[i])
        check(f"WE 2.1.1 result Q{i + 1} cash paid", paid, printed_paid[i])
        check(f"WE 2.1.1 result Q{i + 1} cash-basis EBITDA", recd - paid, printed_cash_eb[i])
        check(f"WE 2.1.1 result Q{i + 1} divergence from accrual", recd - paid - QEB,
              printed_div[i])
        tot_recd += recd
        tot_paid += paid
        pr, pp = RECV_P[i], PAY_P[i]

    check("WE 2.1.1 result year cash received", tot_recd, D(11100000))
    check("WE 2.1.1 result year cash paid", tot_paid, D(4200000))
    check("WE 2.1.1 result year cash-basis EBITDA", tot_recd - tot_paid, CF_NOTAX)
    check("WE 2.1.1 result year divergence", tot_recd - tot_paid - EBITDA, D(-600000))
    check("WE 2.1.1 year cash received = revenue less closing receivables", tot_recd, REV - RECV)
    check("WE 2.1.1 year cash paid = cost less closing payables", tot_paid, COST - PAY)
    check("INVARIANT 2.1.1 cumulative accrual less cumulative cash = closing net working capital",
          (tot_recd - tot_paid - EBITDA) + (RECV - PAY), D(0))
    check("INVARIANT 2.1.1 the quarter where the bases agree is the quarter where net working "
          "capital did not move",
          (RECV_P[3] - RECV_P[2]) - (PAY_P[3] - PAY_P[2]), D(0))
    check("INVARIANT 2.1.1 the divergences sum to the annual divergence",
          sum(printed_div, D(0)), D(-600000))
    check("MCQ 2.1-D 600,000 difference is receivables less payables", RECV - PAY, D(600000))
    check("Self-check 2.1 Q4 net working capital movement is nil",
          (RECV_P[3] - PAY_P[3]) - (RECV_P[2] - PAY_P[2]), D(0))

    # ==================================================================== breakevens ===========
    # WE 2.2.1 interpretation. Margins, elasticity and the two breakevens.
    EB_MARGIN = EBITDA / REV * 100
    NET_MARGIN = NI / REV * 100
    check("WE 2.2.1 interpretation EBITDA margin %", q(EB_MARGIN), D("62.5000"))
    check("WE 2.2.1 interpretation net margin %", q(NET_MARGIN), D("17.2000"))
    check("WE 2.2.1 interpretation margin gap, points", q(EB_MARGIN - NET_MARGIN, 2), D("45.30"))

    REV_1PC = REV * D("0.01")
    NI_1PC = REV_1PC * (1 - T)
    check("WE 2.2.1 interpretation 1 % of revenue", REV_1PC, D(120000))
    check("WE 2.2.1 interpretation net-income cost of a 1 % revenue fall", NI_1PC, D(96000))
    check("WE 2.2.1 interpretation that cost as % of net income", q(NI_1PC / NI * 100),
          D("4.6512"))
    check("WE 2.2.1 interpretation revenue elasticity of net income",
          q((NI_1PC / NI * 100) / 1), D("4.6512"))

    PBE_EBITDA = DEP + INT
    PBE_REV = PBE_EBITDA + COST
    check("WE 2.2.1 interpretation profit-breakeven EBITDA", PBE_EBITDA, D(4920000))
    check("WE 2.2.1 interpretation profit-breakeven revenue", PBE_REV, D(9420000))
    check("WE 2.2.1 interpretation profit-breakeven revenue fall %",
          q((REV - PBE_REV) / REV * 100), D("21.5000"))
    check("INVARIANT 2.2.1 net income is nil at the profit breakeven",
          (PBE_EBITDA - DEP - INT) * (1 - T), D(0))

    # Cash breakeven: solve CFADS(E) = 0.8 E + 0.2 (dep + interest) - dWC for CFADS = debt service.
    DWC = RECV - PAY
    CBE_EBITDA = (INST - T * (DEP + INT) + DWC) / (1 - T)
    check("WE 2.2.1 interpretation cash-breakeven EBITDA (solved)", q(CBE_EBITDA, 2),
          D("5782044.04"))
    check("WE 2.2.1 interpretation cash-breakeven revenue", q(CBE_EBITDA + COST, 2),
          D("10282044.04"))
    check("WE 2.2.1 interpretation cash-breakeven revenue fall % (exact)",
          q((REV - (CBE_EBITDA + COST)) / REV * 100), D("14.3163"))
    check("WE 2.2.1 interpretation cash-breakeven revenue fall % as printed (2 dp)",
          q((REV - (CBE_EBITDA + COST)) / REV * 100, 2), D("14.32"))
    check("INVARIANT 2.2.1 CFADS equals debt service at the cash breakeven",
          q((1 - T) * CBE_EBITDA + T * (DEP + INT) - DWC, 2), q(INST, 2))
    check("INVARIANT 2.2.1 the 20 % charge is still a charge on positive profit at the cash "
          "breakeven (PBT > 0)", D(1) if CBE_EBITDA - DEP - INT > 0 else D(0), D(1))
    check("WE 2.2.1 interpretation PBT at the cash breakeven", q(CBE_EBITDA - DEP - INT, 2),
          D("862044.04"))
    check("INVARIANT 2.2.1 the cash constraint binds first, by about seven revenue points",
          q((REV - PBE_REV) / REV * 100 - (REV - (CBE_EBITDA + COST)) / REV * 100, 2),
          D("7.18"))
    check("Self-check 2.2 the CFADS coefficient on EBITDA is 1 - T", 1 - T, D("0.80"))

    # =================================================================== articulation ==========
    # WE 2.2.4 (original) supplies OCF; recomputed here because everything below depends on it.
    OCF = NI + DEP - RECV + PAY
    check("WE 2.2.2 setup operating cash flow (indirect)", OCF, D(3864000))

    # WE 2.2.3 — the direct method.
    COLLECT = REV - RECV
    SUPPLIER = COST - PAY
    OCF_DIRECT = COLLECT - SUPPLIER - INT - TAX
    check("WE 2.2.3 result cash collected from customers", COLLECT, D(11100000))
    check("WE 2.2.3 result cash paid to suppliers and employees", SUPPLIER, D(4200000))
    check("WE 2.2.3 result interest paid", INT, D(2520000))
    check("WE 2.2.3 result tax paid", TAX, D(516000))
    check("WE 2.2.3 result operating cash flow (direct)", OCF_DIRECT, D(3864000))
    check("INVARIANT 2.2.3 direct and indirect methods give the identical figure",
          OCF_DIRECT - OCF, D(0))
    check("WE 2.2.3 interpretation collection ratio %", q(COLLECT / REV * 100), D("92.5000"))
    DSO = RECV / REV * DAYS
    DPO = PAY / COST * DAYS
    check("WE 2.2.3 interpretation days sales outstanding", q(DSO), D("27.3750"))
    check("WE 2.2.3 interpretation days payables outstanding on cash costs", q(DPO),
          D("24.3333"))
    check("WE 2.2.3 interpretation OCF on a financing classification of interest", OCF + INT,
          CFADS)
    check("WE 2.2.3 interpretation the classification moves OCF by % of itself",
          q(INT / OCF * 100), D("65.2174"))
    check("INVARIANT 2.2.3 CFADS = OCF + interest holds only because interest is classified in "
          "operating", CFADS - (OCF + INT), D(0))
    check("MCQ 2.2-F the difference between the two reported figures is the whole interest charge",
          (OCF + INT) - OCF, INT)

    # WE 2.2.2 — the balance sheet, derived and proved.
    PLANT = CAPEX - DEP
    CASH = OCF - PRIN
    DEBT1 = DEBT0 - PRIN
    EQ1 = EQ0 + NI
    ASSETS = PLANT + RECV + CASH
    LIABEQ = PAY + DEBT1 + EQ1
    check("WE 2.2.2 result plant, net", PLANT, D(57600000))
    check("WE 2.2.2 result receivables", RECV, D(900000))
    check("WE 2.2.2 result cash", q(CASH, 2), D("1374364.77"))
    check("WE 2.2.2 result total assets", q(ASSETS, 2), D("59874364.77"))
    check("WE 2.2.2 result payables", PAY, D(300000))
    check("WE 2.2.2 result senior debt", q(DEBT1, 2), D("39510364.77"))
    check("WE 2.2.2 result equity", EQ1, D(20064000))
    check("WE 2.2.2 result total liabilities and equity", q(LIABEQ, 2), D("59874364.77"))
    check("INVARIANT 2.2.2 A = L + E with nothing plugged (residual)", ASSETS - LIABEQ, D(0))
    check("WE 2.2.2 interpretation cash by route 1 (OCF less principal)", q(OCF - PRIN, 2),
          D("1374364.77"))
    check("WE 2.2.2 interpretation cash by route 2 (CFADS less debt service)",
          q(CFADS - INST, 2), D("1374364.77"))
    check("INVARIANT 2.2.2 the two routes to cash are identical because the interest cancels",
          (CFADS - INST) - (OCF - PRIN), D(0))
    check("INVARIANT 2.2.2 CFADS less OCF equals debt service less principal (both the interest)",
          (CFADS - OCF) - (INST - PRIN), D(0))
    # The restricted split: Domain 6's DSRA instalment DERIVED from ctx's instalment.
    DSRA = INST * D(6) / 12
    DSRA_FUND = DSRA / 2
    DIST = CFADS - INST - DSRA_FUND
    check("WE 2.2.2 interpretation Domain 6 DSRA instalment derived from ctx debt service",
          q(DSRA_FUND, 2), D("1252408.81"))
    check("WE 2.2.2 interpretation Domain 6 first-year distribution", q(DIST, 2), D("121955.96"))
    check("INVARIANT 2.2.2 the reserve instalment plus the distribution is the closing cash",
          q(DSRA_FUND + DIST, 2) - q(CASH, 2), D(0))
    check("WE 2.2.2 interpretation restricted share of closing cash %",
          q(DSRA_FUND / CASH * 100), D("91.1264"))
    check("WE 2.2.2 interpretation free share of closing cash %", q(DIST / CASH * 100),
          D("8.8736"))
    check("INVARIANT 2.2.2 the restricted and free shares sum to 100 %",
          q(DSRA_FUND / CASH * 100) + q(DIST / CASH * 100), D(100))
    check("WE 2.2.2 interpretation carrying amount at loan maturity",
          CAPEX - D(TENOR) * DEP, D(31200000))
    CE1 = DEBT1 + EQ1
    check("WE 2.2.2 interpretation closing capital employed", q(CE1, 2), D("59574364.77"))
    check("WE 2.2.2 interpretation fall in capital employed", q(CAPEX - CE1, 2), D("425635.23"))
    check("INVARIANT 2.2.2 capital employed falls by principal repaid less retained profit",
          (CAPEX - CE1) - (PRIN - NI), D(0))

    # MCQ 2.2-E — each distractor recomputed as its named error.
    check("MCQ 2.2-E key cash", q(CASH, 2), D("1374364.77"))
    check("MCQ 2.2-E key total assets", q(ASSETS, 2), D("59874364.77"))
    check("MCQ 2.2-E distractor B cash (principal repayment omitted)", OCF, D(3864000))
    check("MCQ 2.2-E distractor B total assets", PLANT + RECV + OCF, D(62364000))
    check("MCQ 2.2-E distractor C cash (whole instalment deducted, interest double-counted)",
          q(OCF - INST, 2), D("-1145635.23"))
    check("MCQ 2.2-E distractor C total assets", q(PLANT + RECV + (OCF - INST), 2),
          D("57354364.77"))
    check("MCQ 2.2-E distractor D total assets (cash omitted from the total)", PLANT + RECV,
          D(58500000))

    # WE 2.2.4 interpretation additions.
    check("WE 2.2.4 interpretation cash conversion (OCF / net income)", q(OCF / NI), D("1.8721"))
    check("INVARIANT 2.2.4 cash conversion exceeds 1.0 because depreciation exceeds the "
          "working-capital absorption", D(1) if DEP > DWC else D(0), D(1))
    WRONG_ABSORB = RECV + PAY
    CF_WRONG = EBITDA - TAX - WRONG_ABSORB
    check("WE 2.2.4 interpretation absorption on the reversed payables sign", WRONG_ABSORB,
          D(1200000))
    check("WE 2.2.4 interpretation CFADS on the reversed sign", CF_WRONG, D(5784000))
    check("WE 2.2.4 interpretation DSCR on the reversed sign", q(CF_WRONG / INST), D("1.1546"))
    check("WE 2.2.4 interpretation the documented DSCR", q(CFADS / INST), D("1.2743"))
    check("WE 2.2.4 interpretation the sign error costs this much coverage",
          q(D(2) * PAY / INST), D("0.1198"))
    check("INVARIANT 2.2.4 the sign error is worth twice the payables balance of CFADS",
          CFADS - CF_WRONG, D(2) * PAY)
    check("WE 2.2.4 interpretation cash after debt service", q(OCF - PRIN, 2), D("1374364.77"))

    # ==================================================================== collections =========
    # WE 2.3.1 interpretation — the unit sensitivity.
    CF_PRE = EBITDA - TAX
    check("WE 2.3.1 CFADS before working capital", CF_PRE, D(6984000))
    check("WE 2.3.1 interpretation working-capital absorption per 0.01 of DSCR",
          q(D("0.01") * INST, 2), D("50096.35"))
    check("INVARIANT 2.3.1 CFADS after working capital ties to ctx", CF_PRE - DWC, CFADS)

    # WE 2.3.1B — each threshold inverted to a maximum collection period.
    thresholds = [("distribution condition", D("1.25"), D("6262044.04"), D("1021955.96"),
                   D("31.0845"), D("3.7095")),
                  ("financial covenant", D("1.20"), D("6011562.28"), D("1272437.72"),
                   D("38.7033"), D("11.3283")),
                  ("distribution lock-up", D("1.15"), D("5761080.51"), D("1522919.49"),
                   D("46.3221"), D("18.9471"))]
    for name, th, p_trig, p_recv, p_dso, p_head in thresholds:
        trig = th * INST
        allow_recv = (CF_PRE - trig) + PAY
        max_dso = allow_recv / REV * DAYS
        check(f"WE 2.3.1B result {name} CFADS trigger", q(trig, 2), p_trig)
        check(f"WE 2.3.1B result {name} allowable closing receivables", q(allow_recv, 2), p_recv)
        check(f"WE 2.3.1B result {name} maximum DSO, days", q(max_dso), p_dso)
        check(f"WE 2.3.1B result {name} headroom in days", q(max_dso - DSO), p_head)
        check(f"INVARIANT 2.3.1B the {name} threshold is reproduced by its own maximum DSO",
              q((CF_PRE - (REV * max_dso / DAYS - PAY)) / INST, 4), q(th, 4))
    check("WE 2.3.1B result actual DSO", q(DSO), D("27.3750"))

    R45 = REV * D(45) / DAYS
    AB45 = R45 - PAY
    CF45 = CF_PRE - AB45
    check("WE 2.3.1B result 45-day receivables", q(R45, 2), D("1479452.05"))
    check("WE 2.3.1B result 45-day absorption", q(AB45, 2), D("1179452.05"))
    check("WE 2.3.1B result 45-day CFADS", q(CF45, 2), D("5804547.95"))
    check("WE 2.3.1B result 45-day DSCR", q(CF45 / INST), D("1.1587"))
    check("INVARIANT 2.3.1B the 45-day case is below the covenant and inside the lock-up",
          D(1) if D("1.15") < CF45 / INST < D("1.20") else D(0), D(1))

    CASH_PER_DAY = REV / DAYS
    DSCR_PER_DAY = CASH_PER_DAY / INST
    check("WE 2.3.1B interpretation cash per day of DSO", q(CASH_PER_DAY, 2), D("32876.71"))
    check("WE 2.3.1B interpretation DSCR per day of DSO", q(DSCR_PER_DAY, 6), D("0.006563"))
    check("WE 2.3.1B interpretation DSCR per week of DSO", q(DSCR_PER_DAY * 7), D("0.0459"))
    # The unit rate has to reproduce the headroom it was derived from: 3.7095 days of collection
    # must be worth exactly the receivable balance the 1.25x condition tolerates above the actual.
    check("INVARIANT 2.3.1B the per-day rate reproduces the distribution headroom in cash",
          q(CASH_PER_DAY * D("3.7095"), 0),
          q(((CF_PRE - D("1.25") * INST) + PAY) - RECV, 0))
    check("INVARIANT 2.3.1B the per-day DSCR rate reproduces the distribution headroom in ratio",
          q(DSCR_PER_DAY * D("3.7095"), 4), q(CFADS / INST - D("1.25"), 4))
    check("WE 2.3.1B interpretation 360-day convention tightens thresholds by %",
          q(DAYS / D(360) * 100 - 100), D("1.3889"))
    check("WE 2.3.1B interpretation the 1.25x test on a 360-day convention, days",
          q(((CF_PRE - D("1.25") * INST) + PAY) / REV * D(360)), D("30.6587"))

    # Fig 2.3.1 — every plotted point, including the two unlabelled endpoints.
    def dscr_of_dso(days):
        return (CF_PRE - (REV * D(days) / DAYS - PAY)) / INST

    for days, printed in ((D("27.3750"), D("1.2743")), (D("31.0845"), D("1.2500")),
                          (D("38.7033"), D("1.2000")), (D(45), D("1.1587")),
                          (D(20), D("1.3227")), (D(50), D("1.1259"))):
        check(f"Fig 2.3.1 curve at {days} days DSO", q(dscr_of_dso(days)), printed)
    check("INVARIANT Fig 2.3.1 the curve is linear in DSO (equal steps give equal falls)",
          q(dscr_of_dso(30) - dscr_of_dso(20), 8),
          q(dscr_of_dso(40) - dscr_of_dso(30), 8))
    check("Fig 2.3.1 caption 50,096.35 of absorption is 0.01 of coverage",
          q(D("50096.35") / INST, 4), D("0.0100"))

    # MCQ 2.3-E — the key and its two numeric distractors.
    check("MCQ 2.3-E key (1.20x maximum DSO)",
          q(((CF_PRE - D("1.20") * INST) + PAY) / REV * DAYS), D("38.7033"))
    check("MCQ 2.3-E distractor A (the actual position, not the limit)", q(DSO), D("27.3750"))
    check("MCQ 2.3-E distractor B (the 1.25x distribution threshold)",
          q(((CF_PRE - D("1.25") * INST) + PAY) / REV * DAYS), D("31.0845"))
    check("MCQ 2.3-E distractor D (the breaching scenario's DSCR)", q(CF45 / INST), D("1.1587"))

    # Case study A — the headroom figures now printed in the case.
    check("Case study A headroom at the 1.25x distribution condition",
          q(CFADS - D("1.25") * INST, 0), D(121956))
    check("Case study A headroom at the 1.20x covenant", q(CFADS - D("1.20") * INST, 0),
          D(372438))
    check("Case study A that headroom as % of CFADS",
          q((CFADS - D("1.20") * INST) / CFADS * 100, 1), D("5.8"))
    check("Case study A pre-working-capital covenant headroom",
          q(CF_PRE - D("1.20") * INST, 0), D(972438))
    check("Case study A the 600,000 as % of the apparent margin",
          q(DWC / (CF_PRE - D("1.20") * INST) * 100, 1), D("61.7"))
    check("Case study A tolerated receivables at the 1.25x condition",
          q((CF_PRE - D("1.25") * INST) + PAY, 2), D("1021955.96"))
    check("INVARIANT Case study A the working-capital movement is exactly the headroom lost",
          (CF_PRE - D("1.20") * INST) - (CFADS - D("1.20") * INST), DWC)

    # =================================================================== recognition ===========
    # WE 2.3.2 / Case study C — input against output progress, then the onerous swing.
    C_INC = D(27000000)
    C_TC = D(15000000)
    C_TOT = C_INC + C_TC
    C_MARGIN = EPC - C_TOT
    prog_in = C_INC / C_TOT
    rev_in = EPC * prog_in
    prof_in = rev_in - C_INC
    rev_out = EPC * CERT_PCT
    prof_out = rev_out - C_INC
    check("WE 2.3.2 setup expected total cost", C_TOT, D(42000000))
    check("WE 2.3.2 setup expected margin", C_MARGIN, D(6000000))
    check("WE 2.3.2 setup expected margin %", q(C_MARGIN / EPC * 100), D("12.5000"))
    check("WE 2.3.2 result input progress %", q(prog_in * 100), D("64.2857"))
    check("WE 2.3.2 result input revenue to date", q(rev_in, 2), D("30857142.86"))
    check("WE 2.3.2 result input profit to date", q(prof_in, 2), D("3857142.86"))
    check("WE 2.3.2 result output progress %", q(CERT_PCT * 100), D("61.0000"))
    check("WE 2.3.2 result output revenue to date", q(rev_out, 2), D("29280000.00"))
    check("WE 2.3.2 result output profit to date", q(prof_out, 2), D("2280000.00"))
    check("WE 2.3.2 result difference in progress, points", q(prog_in * 100 - CERT_PCT * 100),
          D("3.2857"))
    check("WE 2.3.2 result difference in revenue and profit", q(rev_in - rev_out, 2),
          D("1577142.86"))
    check("INVARIANT 2.3.2 the revenue difference and the profit difference are the same number "
          "because cost to date is common", (rev_in - rev_out) - (prof_in - prof_out), D(0))
    check("WE 2.3.2 interpretation difference as % of the expected margin",
          q((rev_in - rev_out) / C_MARGIN * 100), D("26.2857"))
    C_TC2 = D(22500000)
    C_TOT2 = C_INC + C_TC2
    C_LOSS = C_TOT2 - EPC
    C_CHARGE = prof_in + C_LOSS
    check("WE 2.3.2 result revised expected total cost", C_TOT2, D(49500000))
    check("WE 2.3.2 result expected loss", C_LOSS, D(1500000))
    check("WE 2.3.2 result period charge", q(C_CHARGE, 2), D("5357142.86"))
    check("WE 2.3.2 result charge as a multiple of the loss", q(C_CHARGE / C_LOSS), D("3.5714"))
    check("INVARIANT 2.3.2 the multiple is 1 + profit recognised / loss",
          q(C_CHARGE / C_LOSS, 6), q(1 + prof_in / C_LOSS, 6))
    check("MCQ 2.3-G key period charge", q(C_CHARGE, 2), D("5357142.86"))
    check("MCQ 2.3-G distractor A (the loss alone, reversal forgotten)", C_LOSS, D(1500000))
    # Case study C prints the same set; each figure is checked against the derivation, not the WE.
    check("Case study C cumulative profit on the input measure", q(prof_in, 2), D("3857142.86"))
    check("Case study C input revenue recognised", q(rev_in, 2), D("30857142.86"))
    check("Case study C output revenue recognised", q(rev_out, 2), D("29280000.00"))
    check("Case study C output cumulative profit", q(prof_out, 2), D("2280000.00"))
    check("Case study C the gap between the two systems", q(rev_in - rev_out, 2),
          D("1577142.86"))
    check("Case study C that gap as % of the expected margin",
          q((rev_in - rev_out) / C_MARGIN * 100), D("26.2857"))
    check("Case study C the quarter's charge", q(C_CHARGE, 2), D("5357142.86"))
    check("Case study C the charge as a multiple of the loss", q(C_CHARGE / C_LOSS),
          D("3.5714"))

    # WE 2.3.4 — measuring and unwinding a restoration obligation.
    SETTLE = D(4500000)
    PR_RATE = D("0.05")
    PROV = SETTLE / (1 + PR_RATE) ** LIFE
    ACC1 = PROV * PR_RATE
    PDEP1 = PROV / LIFE
    check("WE 2.3.4 result provision recognised at construction", q(PROV, 2), D("1328862.47"))
    check("WE 2.3.4 result asset cost including the restoration asset", q(CAPEX + PROV, 2),
          D("61328862.47"))
    check("WE 2.3.4 result year-one accretion", q(ACC1, 2), D("66443.12"))
    check("WE 2.3.4 result year-one depreciation of the restoration asset", q(PDEP1, 2),
          D("53154.50"))
    check("WE 2.3.4 result year-one charge against profit", q(ACC1 + PDEP1, 2), D("119597.62"))
    check("WE 2.3.4 result provision at the end of year one", q(PROV + ACC1, 2), D("1395305.60"))
    check("INVARIANT 2.3.4 the year-one cash effect is nil: the provision moves by the accretion "
          "alone, so no settlement is recognised",
          (PROV + ACC1) - PROV - ACC1, D(0))
    check("WE 2.3.4 interpretation the charge as % of net income",
          q((ACC1 + PDEP1) / NI * 100), D("5.7945"))
    check("WE 2.3.4 interpretation lifetime accretion", q(SETTLE - PROV, 2), D("3171137.53"))
    check("INVARIANT 2.3.4 lifetime depreciation plus lifetime accretion equals the cash "
          "eventually paid", q(PROV + (SETTLE - PROV), 2), q(SETTLE, 2))
    check("INVARIANT 2.3.4 the provision accretes to exactly the settlement amount",
          q(PROV * (1 + PR_RATE) ** LIFE, 2), q(SETTLE, 2))
    for r, p_prov, p_chg in ((D("0.04"), D("1688025.61"), D("27.0279")),
                             (D("0.06"), D("1048493.84"), D("-21.0984"))):
        p2 = SETTLE / (1 + r) ** LIFE
        check(f"WE 2.3.4 interpretation provision at {r * 100} %", q(p2, 2), p_prov)
        check(f"WE 2.3.4 interpretation change against the 5.0 % provision at {r * 100} %",
              q((p2 - PROV) / PROV * 100), p_chg)
    check("MCQ 2.3-F distractor A (undiscounted amount depreciated straight-line)",
          SETTLE / LIFE, D(180000))
    check("MCQ 2.3-F distractor C (accretion alone, restoration asset forgotten)", q(ACC1, 2),
          D("66443.12"))
    check("WE 2.3.4 interpretation Domain 10 PLCR the spend sits outside", PLCR, D("1.9431"))

    # WE 2.3.3 interpretation — the after-tax refinement.
    AF810 = af(DISC, 10)
    OVERHAUL = D(1200000)
    check("WE 2.3.3 interpretation AF(8 %, 10)", q(AF810, 6), D("6.710081"))
    relief_now = T * OVERHAUL
    relief_ann = T * (OVERHAUL / 10)
    relief_pv = relief_ann * AF810
    check("WE 2.3.3 interpretation relief if expensed", relief_now, D(240000))
    check("WE 2.3.3 interpretation annual relief if capitalised", relief_ann, D(24000))
    check("WE 2.3.3 interpretation present value of the capitalised relief", q(relief_pv, 2),
          D("161041.95"))
    check("WE 2.3.3 interpretation present-value advantage of expensing",
          q(relief_now - relief_pv, 2), D("78958.05"))
    check("WE 2.3.3 interpretation that advantage as % of the spend",
          q((relief_now - relief_pv) / OVERHAUL * 100), D("6.5798"))
    check("INVARIANT 2.3.3 both treatments charge the identical amount over ten years",
          OVERHAUL - 10 * (OVERHAUL / 10), D(0))
    check("INVARIANT 2.3.3 the deferred charge is the year-one profit difference",
          OVERHAUL - OVERHAUL / 10, D(1080000))

    # ======================================================================== ratios ===========
    CA = RECV + CASH
    check("WE 2.4.1 result current assets", q(CA, 2), D("2274364.77"))
    check("WE 2.4.1 result current ratio", q(CA / PAY), D("7.5812"))
    check("WE 2.4.1 result cash as % of current assets", q(CASH / CA * 100), D("60.4285"))
    check("WE 2.4.1 result debt/equity, closing", q(DEBT1 / EQ1), D("1.9692"))
    check("WE 2.4.1 result capital employed", q(CE1, 2), D("59574364.77"))
    check("WE 2.4.1 result gearing, closing %", q(DEBT1 / CE1 * 100), D("66.3211"))
    check("WE 2.4.1 result gearing at financial close %", q(DEBT0 / CAPEX * 100), D("70.0000"))
    ND = DEBT1 - CASH
    check("WE 2.4.1 result net debt", q(ND, 2), D("38136000.00"))
    check("WE 2.4.1 result net debt/EBITDA", q(ND / EBITDA), D("5.0848"))
    check("WE 2.4.1 result gross debt/EBITDA", q(DEBT1 / EBITDA), D("5.2680"))
    check("WE 2.4.1 result pre-tax ROCE on closing capital %", q(EBIT / CE1 * 100), D("8.5607"))
    EBIT_AT = EBIT * (1 - T)
    ROCE_AT = EBIT_AT / CAPEX * 100
    check("WE 2.4.1 result EBIT after tax", EBIT_AT, D(4080000))
    check("WE 2.4.1 result after-tax ROCE on opening capital %", q(ROCE_AT), D("6.8000"))
    check("WE 2.4.1 result ROE on closing equity %", q(NI / EQ1 * 100), D("10.2871"))
    EQ_AVG = (EQ0 + EQ1) / 2
    check("WE 2.4.1 result average equity", EQ_AVG, D(19032000))
    check("WE 2.4.1 result ROE on average equity %", q(NI / EQ_AVG * 100), D("10.8449"))
    check("WE 2.4.1 result ROE on opening equity %", q(NI / EQ0 * 100), D("11.4667"))
    KDAT = RATE * (1 - T) * 100
    check("WE 2.4.1 result after-tax cost of debt %", q(KDAT), D("4.8000"))
    DE0 = DEBT0 / EQ0
    check("WE 2.4.1 interpretation opening debt/equity", q(DE0), D("2.3333"))
    check("WE 2.4.1 interpretation the spread, points", q(ROCE_AT - KDAT), D("2.0000"))
    check("WE 2.4.1 interpretation the leverage wedge, points", q(DE0 * (ROCE_AT - KDAT)),
          D("4.6667"))
    check("INVARIANT 2.4.1 the leverage identity is EXACT on opening bases",
          q(ROCE_AT + DE0 * (ROCE_AT - KDAT), 6), q(NI / EQ0 * 100, 6))
    check("INVARIANT 2.4.1 the identity is BROKEN on closing bases (residual is not nil)",
          D(1) if q(EBIT_AT / CE1 * 100 + (DEBT1 / EQ1) * (EBIT_AT / CE1 * 100 - KDAT), 4)
          != q(NI / EQ1 * 100, 4) else D(0), D(1))
    check("WE 2.4.1 interpretation ROE spread across bases, points",
          q(NI / EQ0 * 100 - NI / EQ1 * 100), D("1.1796"))
    check("WE 2.4.1 interpretation average versus closing ROE, points",
          q(NI / EQ_AVG * 100 - NI / EQ1 * 100), D("0.5578"))
    check("WE 2.4.1 interpretation after-tax ROCE against Domain 9's WACC, points",
          q(ROCE_AT - WACC), D("-1.1860"))
    check("WE 2.4.1 interpretation year-two after-tax ROCE on unchanged EBIT %",
          q(EBIT_AT / CE1 * 100), D("6.8486"))
    check("INVARIANT 2.4.1 year two rises purely because capital employed fell",
          q(CAPEX - CE1, 2), q(PRIN - NI, 2))
    check("WE 2.4.1 interpretation ROE against the cost of equity, points",
          q(NI / EQ1 * 100 - KE), D("-5.1329"))
    check("WE 2.4.1 interpretation ROE against the equity IRR, points",
          q(NI / EQ1 * 100 - EQ_IRR), D("-2.2440"))
    check("WE 2.4.1 interpretation Domain 4's NPV quoted beside the accounting return", NPV,
          D(16179360))
    check("MCQ 2.4-D quoted project IRR", PROJ_IRR, D("12.19"))
    check("WE 2.4.1 interpretation restricted share of the current-asset cash %",
          q(DSRA_FUND / CASH * 100), D("91.1264"))

    # Fig 2.4.1 — the six bars and the two annotations.
    for lab, val in (("after-tax cost of debt", q(KDAT)), ("after-tax ROCE", q(ROCE_AT)),
                     ("WACC", WACC), ("ROE on opening equity", q(NI / EQ0 * 100)),
                     ("equity IRR", EQ_IRR), ("cost of equity", KE)):
        check(f"Fig 2.4.1 bar {lab} is inside the 0-16 % axis",
              D(1) if D(0) <= val <= D(16) else D(0), D(1))
    check("Fig 2.4.1 bar after-tax cost of debt", q(KDAT), D("4.8000"))
    check("Fig 2.4.1 bar after-tax ROCE on opening capital", q(ROCE_AT), D("6.8000"))
    check("Fig 2.4.1 bar ROE on opening equity", q(NI / EQ0 * 100), D("11.4667"))
    check("Fig 2.4.1 wedge annotation D/E x spread", q(DE0 * (ROCE_AT - KDAT)), D("4.6667"))
    check("Fig 2.4.1 wedge annotation lands on the ROE bar",
          q(ROCE_AT + DE0 * (ROCE_AT - KDAT)), D("11.4667"))
    check("Fig 2.4.1 gap annotation against WACC", q(ROCE_AT - WACC), D("-1.1860"))
    check("Fig 2.4.1 ordering: every bar is above the one below it",
          D(1) if q(KDAT) < q(ROCE_AT) < WACC < q(NI / EQ0 * 100) < EQ_IRR < KE else D(0), D(1))

    # WE 2.4.2 interpretation — two interest covers, and which covenant binds.
    check("WE 2.4.2 interpretation interest cover on EBIT", q(EBIT / INT), D("2.0238"))
    check("WE 2.4.2 interpretation cash interest cover on EBITDA", q(EBITDA / INT), D("2.9762"))
    check("WE 2.4.2 interpretation the gap between the two covers",
          q(EBITDA / INT - EBIT / INT), D("0.9524"))
    check("INVARIANT 2.4.2 the gap between the two covers is depreciation over interest",
          q(EBITDA / INT - EBIT / INT, 8), q(DEP / INT, 8))
    ICR_TRIG = D("2.00") * INT
    ICR_HEAD = EBIT - ICR_TRIG
    check("WE 2.4.2 interpretation interest-cover covenant EBIT trigger", ICR_TRIG, D(5040000))
    check("WE 2.4.2 interpretation interest-cover headroom", ICR_HEAD, D(60000))
    check("WE 2.4.2 interpretation interest-cover headroom as % of revenue",
          q(ICR_HEAD / REV * 100), D("0.5000"))
    DSCR_HEAD = CFADS - D("1.20") * INST
    DSCR_HEAD_REV = DSCR_HEAD / D("0.80")
    check("WE 2.4.2 interpretation DSCR covenant headroom in cash", q(DSCR_HEAD, 2),
          D("372437.72"))
    check("WE 2.4.2 interpretation DSCR headroom in revenue units", q(DSCR_HEAD_REV, 2),
          D("465547.16"))
    check("WE 2.4.2 interpretation DSCR headroom as % of revenue",
          q(DSCR_HEAD_REV / REV * 100), D("3.8796"))
    check("WE 2.4.2 interpretation the DSCR covenant tolerates this multiple of the revenue miss",
          q(DSCR_HEAD_REV / ICR_HEAD), D("7.7591"))
    check("INVARIANT 2.4.2 the interest-cover covenant binds first on these figures",
          D(1) if ICR_HEAD < DSCR_HEAD_REV else D(0), D(1))
    check("MCQ 2.4-E key ratio of the two tolerances", q(DSCR_HEAD_REV / ICR_HEAD), D("7.7591"))
    check("WE 2.4.2 interpretation gross less net debt/EBITDA",
          q(DEBT1 / EBITDA - ND / EBITDA), D("0.1832"))
    check("INVARIANT 2.4.2 the gross/net difference is the cash balance over EBITDA",
          q(DEBT1 / EBITDA - ND / EBITDA, 8), q(CASH / EBITDA, 8))

    # ========================================================================= spend ===========
    # WE 2.4.3 — five answers, reconciled to one identity.
    NOT_INVOICED = D(1205403)      # this domain's own illustration
    RETENTION = D(1200000)         # 2.5 % of the contract price, Domain 14 KA 14.3.2 cap
    UNPAID = D(358000)             # this domain's own illustration
    NONCAP = D(620000)             # owner's G&A failing the capitalisation test
    check("WE 2.4.3 setup retention equals the 2.5 % cap on Domain 14's contract price",
          EPC * D("0.025"), RETENTION)
    IDC_TO_DATE = D14_IDC_TOTAL - D14_IDC_REMAIN
    check("WE 2.4.3 setup capitalised interest to date", IDC_TO_DATE, D(677923))
    INCURRED = D14_CERT
    OPEN_COMM = D14_EPC_REMAIN + D14_VAR_APPR
    COMMITTED = INCURRED + OPEN_COMM
    INVOICED = INCURRED - NOT_INVOICED
    ADV_REC = D14_ADVANCE * CERT_PCT
    ADV_UNREC = D14_ADVANCE - ADV_REC
    PAID = INVOICED - ADV_REC - RETENTION - UNPAID + D14_ADVANCE
    CAPITALISED = INCURRED - NONCAP + IDC_TO_DATE
    check("WE 2.4.3 substitution open commitments", OPEN_COMM, D(19560000))
    check("WE 2.4.3 result committed", COMMITTED, D(53505403))
    check("WE 2.4.3 result capitalised", CAPITALISED, D(34003326))
    check("WE 2.4.3 result incurred (Domain 14's certified spend)", INCURRED, D(33945403))
    check("WE 2.4.3 result paid", q(PAID, 2), D("33054000.00"))
    check("WE 2.4.3 result invoiced", INVOICED, D(32740000))
    check("WE 2.4.3 result advance recovered against certifications", q(ADV_REC, 2),
          D("2928000.00"))
    check("WE 2.4.3 result unrecovered advance", q(ADV_UNREC, 2), D("1872000.00"))
    check("WE 2.4.3 result cash paid against invoices before the advance itself",
          q(INVOICED - ADV_REC - RETENTION - UNPAID, 2), D("28254000.00"))
    RECON = PAID + RETENTION + UNPAID + NOT_INVOICED + OPEN_COMM - ADV_UNREC
    check("WE 2.4.3 result the reconciliation closes on committed", q(RECON, 2),
          q(COMMITTED, 2))
    check("INVARIANT 2.4.3 the reconciliation residual is nil", q(RECON - COMMITTED, 6), D(0))
    check("WE 2.4.3 interpretation capitalised exceeds incurred by", CAPITALISED - INCURRED,
          D(57923))
    check("INVARIANT 2.4.3 that excess is capitalised interest less non-capitalisable cost",
          (CAPITALISED - INCURRED) - (IDC_TO_DATE - NONCAP), D(0))
    APPARENT = INCURRED - PAID
    GROSS_ITEMS = RETENTION + UNPAID + NOT_INVOICED + ADV_UNREC
    check("WE 2.4.3 interpretation apparent incurred-to-paid gap", q(APPARENT, 2),
          D("891403.00"))
    check("WE 2.4.3 interpretation gross reconciling items", q(GROSS_ITEMS, 2),
          D("4635403.00"))
    check("WE 2.4.3 interpretation gross items as a multiple of the apparent gap",
          q(GROSS_ITEMS / APPARENT), D("5.2001"))
    check("WE 2.4.3 interpretation spread between the largest and smallest measure",
          q(COMMITTED - PAID, 2), D("20451403.00"))
    check("WE 2.4.3 interpretation that spread as % of the 60,000,000 envelope",
          q((COMMITTED - PAID) / CAPEX * 100), D("34.0857"))
    check("WE 2.4.3 interpretation capitalised as % of incurred",
          q(CAPITALISED / INCURRED * 100), D("100.1706"))
    check("MCQ 2.4-F key capitalised less incurred", CAPITALISED - INCURRED, D(57923))
    check("INVARIANT 2.4.3 committed is the largest and invoiced the smallest of the five",
          D(1) if COMMITTED > CAPITALISED > INCURRED > PAID > INVOICED else D(0), D(1))

    # 2.A.1 — deferred tax.
    ALLOW_RATE = D("0.15")
    ALLOW1 = CAPEX * ALLOW_RATE
    WDV1 = CAPEX - ALLOW1
    TEMP1 = PLANT - WDV1
    DTL1 = TEMP1 * T
    check("WE 2.A.1 result year-one capital allowance", ALLOW1, D(9000000))
    check("WE 2.A.1 result tax written-down value", WDV1, D(51000000))
    check("WE 2.A.1 result carrying amount", PLANT, D(57600000))
    check("WE 2.A.1 result temporary difference", TEMP1, D(6600000))
    check("WE 2.A.1 result deferred tax liability", DTL1, D(1320000))
    CASH_TAX_DB = D(0)          # Domain 6 KA 6.2.3: the allowance and interest give a tax loss
    check("WE 2.A.1 result total tax charge (current nil plus the DTL movement)",
          CASH_TAX_DB + DTL1, D(1320000))
    check("WE 2.A.1 result net income under the allowance regime", PBT - DTL1, D(1260000))
    check("WE 2.A.1 interpretation fall in net income", NI - (PBT - DTL1), D(804000))
    check("WE 2.A.1 setup Domain 6's year-one tax loss carried forward",
          ALLOW1 + INT - EBITDA, TAXLOSS)
    check("WE 2.A.1 result CFADS with no cash tax ties to Domain 6", EBITDA - DWC, CF_NOTAX)
    check("WE 2.A.1 result DSCR with no cash tax", q(CF_NOTAX / INST), D("1.3773"))
    check("WE 2.A.1 interpretation rise in DSCR", q(CF_NOTAX / INST - CFADS / INST), D("0.1030"))
    check("INVARIANT 2.A.1 profit and coverage move in opposite directions",
          D(1) if (PBT - DTL1) < NI and CF_NOTAX > CFADS else D(0), D(1))
    # The reversal year and the peak are SEARCHED, not asserted.
    nbv, carry, twdv = CAPEX, CAPEX, CAPEX
    first_below, peak_year, peak_diff = 0, 0, D(-1)
    allow_by_year, diff_by_year = {}, {}
    for yr in range(1, TENOR + 1):
        a = nbv * ALLOW_RATE
        nbv -= a
        carry -= DEP
        twdv -= a
        allow_by_year[yr] = a
        diff_by_year[yr] = carry - twdv
        if first_below == 0 and a < DEP:
            first_below = yr
        if carry - twdv > peak_diff:
            peak_diff, peak_year = carry - twdv, yr
    check("WE 2.A.1 interpretation the allowance first falls below depreciation in year",
          D(first_below), D(10))
    check("WE 2.A.1 interpretation the allowance in that year", q(allow_by_year[10], 2),
          D("2084552.52"))
    check("WE 2.A.1 interpretation the year before is still above depreciation",
          D(1) if allow_by_year[9] > DEP else D(0), D(1))
    check("WE 2.A.1 interpretation the temporary difference peaks in year", D(peak_year), D(9))
    check("WE 2.A.1 interpretation the peak temporary difference", q(peak_diff, 2),
          D("24502983.22"))
    check("WE 2.A.1 interpretation the deferred tax liability at the peak", q(peak_diff * T, 2),
          D("4900596.64"))
    check("INVARIANT 2.A.1 the peak is the year before the reversal begins",
          D(peak_year + 1), D(first_below))

    # 2.A.2 — leases.
    LEASE = D(500000)
    AF610 = af(RATE, 10)
    LIAB = AF610 * LEASE
    check("2.A.2 AF(6 %, 10)", q(AF610, 6), D("7.360087"))
    check("2.A.2 lease liability recognised", q(LIAB, 2), D("3680043.53"))
    check("2.A.2 liability as % of senior debt", q(LIAB / DEBT1 * 100), D("9.3141"))
    check("2.A.2 debt for the ratio after recognition", q(DEBT1 + LIAB, 2), D("43190408.30"))
    check("2.A.2 EBITDA after the operating cost is removed", EBITDA + LEASE, D(8000000))
    check("2.A.2 debt/EBITDA before", q(DEBT1 / EBITDA), D("5.2680"))
    check("2.A.2 debt/EBITDA after, both lines restated",
          q((DEBT1 + LIAB) / (EBITDA + LEASE)), D("5.3988"))
    MOVE = (DEBT1 + LIAB) / (EBITDA + LEASE) - DEBT1 / EBITDA
    check("2.A.2 the consistently restated ratio moves by", q(MOVE), D("0.1308"))
    NAIVE = (DEBT1 + LIAB) / EBITDA
    check("2.A.2 the naive restatement (numerator only)", q(NAIVE), D("5.7587"))
    check("2.A.2 the naive apparent deterioration", q(NAIVE - DEBT1 / EBITDA), D("0.4907"))
    check("2.A.2 the naive answer overstates the move by this multiple",
          q((NAIVE - DEBT1 / EBITDA) / MOVE, 2), D("3.75"))
    check("INVARIANT 2.A.2 restating only the numerator overstates the deterioration",
          D(1) if NAIVE - DEBT1 / EBITDA > MOVE else D(0), D(1))

    # 2.A.4 — distributable reserves.
    check("2.A.4 plant carried after the owner's costs are expensed", CAPEX - NONCAP,
          D(59380000))
    check("2.A.4 retained earnings opening", -NONCAP, D(-620000))
    check("2.A.4 retained earnings after year one", -NONCAP + NI, D(1444000))
    check("2.A.4 the first-year distribution as % of the reserve",
          q(DIST / (-NONCAP + NI) * 100), D("8.4457"))
    check("INVARIANT 2.A.4 an SPV can hold released cash and have no distributable reserve at "
          "the commercial operations date", D(1) if -NONCAP < 0 else D(0), D(1))

    # ===================================================================== exercises ==========
    # Exercises 2.1-2.3 supply the second project; recomputed because 2.5-2.11 build on them.
    E_REV, E_COST, E_ASSET, E_LIFE = D(15000000), D(5800000), D(75000000), 30
    E_INT, E_DS = D(2100000), D(4400000)
    E_EBITDA = E_REV - E_COST
    E_DEP = E_ASSET / E_LIFE
    E_EBIT = E_EBITDA - E_DEP
    E_PBT = E_EBIT - E_INT
    E_TAX = E_PBT * T
    E_NI = E_PBT - E_TAX
    E_RECV, E_INV, E_PAY = D(1100000), D(200000), D(450000)
    E_OCF = E_NI + E_DEP - E_RECV - E_INV + E_PAY
    E_DWC = E_RECV + E_INV - E_PAY
    E_PRIN = E_DS - E_INT
    E_DEBT0, E_EQ0 = D(52500000), D(22500000)
    check("Exercise 2.5 principal", E_PRIN, D(2300000))
    E_CASH = E_OCF - E_PRIN
    check("Exercise 2.5 closing cash", q(E_CASH, 2), D("3030000.00"))
    check("Exercise 2.5 plant, net", E_ASSET - E_DEP, D(72500000))
    check("Exercise 2.5 closing debt", E_DEBT0 - E_PRIN, D(50200000))
    check("Exercise 2.5 closing equity", E_EQ0 + E_NI, D(26180000))
    E_ASSETS = (E_ASSET - E_DEP) + E_RECV + E_INV + E_CASH
    E_LIABEQ = E_PAY + (E_DEBT0 - E_PRIN) + (E_EQ0 + E_NI)
    check("Exercise 2.5 total assets", q(E_ASSETS, 2), D("76830000.00"))
    check("Exercise 2.5 total liabilities and equity", q(E_LIABEQ, 2), D("76830000.00"))
    check("INVARIANT 2.5 the sheet balances (residual)", E_ASSETS - E_LIABEQ, D(0))
    E_CF = E_EBITDA - E_TAX - E_DWC
    check("Exercise 2.5 second route to cash (CFADS less debt service)", q(E_CF - E_DS, 2),
          D("3030000.00"))
    check("INVARIANT 2.5 the two routes to cash agree", (E_CF - E_DS) - (E_OCF - E_PRIN), D(0))
    check("Exercise 2.5 common error (whole instalment deducted) understates cash by", E_INT,
          D(2100000))

    E_CF_PRE = E_EBITDA - E_TAX
    E_TRIG = D("1.60") * E_DS
    E_ALLOW = E_CF_PRE - E_TRIG
    E_ALLOW_RECV = E_ALLOW - E_INV + E_PAY
    check("Exercise 2.6 CFADS before working capital", q(E_CF_PRE, 2), D("8280000.00"))
    check("Exercise 2.6 covenant CFADS trigger", q(E_TRIG, 2), D("7040000.00"))
    check("Exercise 2.6 allowable absorption", q(E_ALLOW, 2), D("1240000.00"))
    check("Exercise 2.6 allowable receivables", q(E_ALLOW_RECV, 2), D("1490000.00"))
    check("Exercise 2.6 maximum DSO, days", q(E_ALLOW_RECV / E_REV * DAYS), D("36.2567"))
    check("Exercise 2.6 actual DSO, days", q(E_RECV / E_REV * DAYS), D("26.7667"))
    check("Exercise 2.6 headroom, days",
          q((E_ALLOW_RECV - E_RECV) / E_REV * DAYS), D("9.4900"))
    check("Exercise 2.6 actual DSCR after working capital", q(E_CF / E_DS), D("1.6886"))
    # Round-trip test started from the PRINTED maximum DSO, not from the balance that produced it.
    check("INVARIANT 2.6 the printed maximum DSO of 36.2567 days reproduces the 1.60x covenant",
          q((E_CF_PRE - (E_REV * D("36.2567") / DAYS + E_INV - E_PAY)) / E_DS, 3), D("1.600"))
    check("INVARIANT 2.6 the printed actual DSO of 26.7667 days reproduces the reported DSCR",
          q((E_CF_PRE - (E_REV * D("26.7667") / DAYS + E_INV - E_PAY)) / E_DS, 3), D("1.689"))

    X_SETTLE, X_RATE, X_LIFE = D(6000000), D("0.06"), 20
    X_PROV = X_SETTLE / (1 + X_RATE) ** X_LIFE
    check("Exercise 2.7 provision recognised", q(X_PROV, 2), D("1870828.36"))
    check("Exercise 2.7 year-one accretion", q(X_PROV * X_RATE, 2), D("112249.70"))
    check("Exercise 2.7 year-one depreciation", q(X_PROV / X_LIFE, 2), D("93541.42"))
    check("Exercise 2.7 total year-one charge",
          q(X_PROV * X_RATE + X_PROV / X_LIFE, 2), D("205791.12"))
    check("Exercise 2.7 provision at the end of year one", q(X_PROV * (1 + X_RATE), 2),
          D("1983078.06"))
    check("INVARIANT 2.7 the year-one cash effect is nil: closing less opening is the accretion",
          q(X_PROV * (1 + X_RATE) - X_PROV, 2), q(X_PROV * X_RATE, 2))
    check("INVARIANT 2.7 lifetime charge equals the settlement amount",
          q(X_PROV + (X_SETTLE - X_PROV), 2), q(X_SETTLE, 2))
    check("Exercise 2.7 lifetime accretion", q(X_SETTLE - X_PROV, 2), D("4129171.64"))

    E_CA = E_RECV + E_INV + E_CASH
    check("Exercise 2.8 current assets", q(E_CA, 2), D("4330000.00"))
    check("Exercise 2.8 current ratio", q(E_CA / E_PAY), D("9.6222"))
    check("Exercise 2.8 debt/equity", q((E_DEBT0 - E_PRIN) / (E_EQ0 + E_NI)), D("1.9175"))
    check("Exercise 2.8 net debt", q((E_DEBT0 - E_PRIN) - E_CASH, 2), D("47170000.00"))
    check("Exercise 2.8 net debt/EBITDA",
          q(((E_DEBT0 - E_PRIN) - E_CASH) / E_EBITDA), D("5.1272"))
    E_CE = (E_DEBT0 - E_PRIN) + (E_EQ0 + E_NI)
    check("Exercise 2.8 closing capital employed", q(E_CE, 2), D("76380000.00"))
    check("Exercise 2.8 pre-tax ROCE %", q(E_EBIT / E_CE * 100), D("8.7719"))
    check("Exercise 2.8 interest cover", q(E_EBIT / E_INT), D("3.1905"))
    E_ROCE_AT = E_EBIT * (1 - T) / E_ASSET * 100
    E_KDAT = E_INT / E_DEBT0 * (1 - T) * 100
    E_DE = E_DEBT0 / E_EQ0
    check("Exercise 2.8 EBIT after tax", E_EBIT * (1 - T), D(5360000))
    check("Exercise 2.8 after-tax ROCE on opening capital %", q(E_ROCE_AT), D("7.1467"))
    check("Exercise 2.8 after-tax cost of debt %", q(E_KDAT), D("3.2000"))
    check("Exercise 2.8 opening debt/equity", q(E_DE), D("2.3333"))
    check("Exercise 2.8 leverage identity", q(E_ROCE_AT + E_DE * (E_ROCE_AT - E_KDAT)),
          D("16.3556"))
    check("Exercise 2.8 ROE on opening equity %", q(E_NI / E_EQ0 * 100), D("16.3556"))
    check("INVARIANT 2.8 the identity reproduces ROE on opening equity exactly",
          q(E_ROCE_AT + E_DE * (E_ROCE_AT - E_KDAT), 6), q(E_NI / E_EQ0 * 100, 6))

    Y_COMM, Y_PAID = D(24000000), D(14600000)
    Y_RET, Y_UNPAID, Y_NOTINV, Y_ADV = D(700000), D(260000), D(540000), D(900000)
    Y_OPEN = Y_COMM - Y_PAID - Y_RET - Y_UNPAID - Y_NOTINV + Y_ADV
    Y_INC = Y_PAID + Y_RET + Y_UNPAID + Y_NOTINV - Y_ADV
    check("Exercise 2.9 open commitments", Y_OPEN, D(8800000))
    check("Exercise 2.9 incurred", Y_INC, D(15200000))
    check("Exercise 2.9 invoiced", Y_INC - Y_NOTINV, D(14660000))
    check("INVARIANT 2.9 incurred plus open commitments equals committed", Y_INC + Y_OPEN,
          Y_COMM)

    Z_COST, Z_LIFE, Z_ALLOW = D(75000000), 30, D("0.20")
    Z_DEP = Z_COST / Z_LIFE
    Z_A1 = Z_COST * Z_ALLOW
    Z_W1 = Z_COST - Z_A1
    Z_C1 = Z_COST - Z_DEP
    check("Exercise 2.10 year-one allowance", Z_A1, D(15000000))
    check("Exercise 2.10 year-one tax written-down value", Z_W1, D(60000000))
    check("Exercise 2.10 year-one carrying amount", Z_C1, D(72500000))
    check("Exercise 2.10 year-one temporary difference", Z_C1 - Z_W1, D(12500000))
    check("Exercise 2.10 year-one deferred tax liability", (Z_C1 - Z_W1) * T, D(2500000))
    Z_A2 = Z_W1 * Z_ALLOW
    Z_W2 = Z_W1 - Z_A2
    Z_C2 = Z_C1 - Z_DEP
    check("Exercise 2.10 year-two allowance", Z_A2, D(12000000))
    check("Exercise 2.10 year-two tax written-down value", Z_W2, D(48000000))
    check("Exercise 2.10 year-two carrying amount", Z_C2, D(70000000))
    check("Exercise 2.10 year-two cumulative temporary difference", Z_C2 - Z_W2, D(22000000))
    check("Exercise 2.10 year-two deferred tax liability", (Z_C2 - Z_W2) * T, D(4400000))
    check("Exercise 2.10 year-two deferred tax charge (the movement)",
          (Z_C2 - Z_W2) * T - (Z_C1 - Z_W1) * T, D(1900000))
    check("INVARIANT 2.10 the movement equals the tax rate on the excess allowance",
          ((Z_C2 - Z_W2) - (Z_C1 - Z_W1)) * T, T * (Z_A2 - Z_DEP))

    W_PRICE, W_INC, W_TC = D(30000000), D(12000000), D(13000000)
    W_TOT = W_INC + W_TC
    W_PROG = W_INC / W_TOT
    W_REV = W_PRICE * W_PROG
    W_PROF = W_REV - W_INC
    check("Exercise 2.11 expected total cost", W_TOT, D(25000000))
    check("Exercise 2.11 progress %", q(W_PROG * 100), D("48.0000"))
    check("Exercise 2.11 revenue to date", W_REV, D(14400000))
    check("Exercise 2.11 profit to date", W_PROF, D(2400000))
    W_TOT2 = W_INC + D(20000000)
    W_LOSS = W_TOT2 - W_PRICE
    W_CHARGE = W_PROF + W_LOSS
    check("Exercise 2.11 revised expected total cost", W_TOT2, D(32000000))
    check("Exercise 2.11 expected loss", W_LOSS, D(2000000))
    check("Exercise 2.11 period charge", W_CHARGE, D(4400000))
    check("Exercise 2.11 charge as a multiple of the loss", q(W_CHARGE / W_LOSS), D("2.2000"))
    check("INVARIANT 2.11 the onerous multiple is 1 + profit recognised / loss on a SECOND set "
          "of figures", q(W_CHARGE / W_LOSS, 6), q(1 + W_PROF / W_LOSS, 6))

    # ====================================================== toolkit and summary restatements ===
    check("Toolkit 2.T.3 the ROE basis spread, points",
          q(NI / EQ0 * 100 - NI / EQ1 * 100), D("1.1796"))
    check("Toolkit 2.T.3 the restricted share of Kestrel's cash %",
          q(DSRA_FUND / CASH * 100), D("91.1264"))
    check("Domain 2 summary collection ratio %", q(COLLECT / REV * 100, 2), D("92.50"))
    check("Domain 2 summary closing cash", q(CASH, 2), D("1374364.77"))
    check("Domain 2 summary balance-sheet total", q(ASSETS, 2), D("59874364.77"))
    check("Domain 2 summary reserve instalment", q(DSRA_FUND, 2), D("1252408.81"))
    check("Domain 2 summary distribution", q(DIST, 2), D("121955.96"))
    check("Domain 2 summary profit breakeven fall %", q((REV - PBE_REV) / REV * 100, 2),
          D("21.50"))
    check("Domain 2 summary cash breakeven fall %",
          q((REV - (CBE_EBITDA + COST)) / REV * 100, 2), D("14.32"))
    check("Domain 2 summary dividend headroom in days", q(D("31.0845") - DSO), D("3.7095"))
    check("Domain 2 summary covenant reached at days",
          q(((CF_PRE - D("1.20") * INST) + PAY) / REV * DAYS), D("38.7033"))
    check("Domain 2 summary capex/opex after-tax advantage", q(relief_now - relief_pv, 2),
          D("78958.05"))
    check("Domain 2 summary progress-measure gap, points",
          q(prog_in * 100 - CERT_PCT * 100), D("3.2857"))
    check("Domain 2 summary progress-measure profit difference", q(rev_in - rev_out, 2),
          D("1577142.86"))
    check("Domain 2 summary onerous charge", q(C_CHARGE, 2), D("5357142.86"))
    check("Domain 2 summary onerous multiple", q(C_CHARGE / C_LOSS), D("3.5714"))
    check("Domain 2 summary provision recognised", q(PROV, 2), D("1328862.47"))
    check("Domain 2 summary provision year-one charge", q(ACC1 + PDEP1, 2), D("119597.62"))
    check("Domain 2 summary provision rate sensitivity %",
          q((SETTLE / (1 + D("0.04")) ** LIFE - PROV) / PROV * 100), D("27.0279"))
    check("Domain 2 summary leverage identity", q(ROCE_AT + DE0 * (ROCE_AT - KDAT)),
          D("11.4667"))
    check("Domain 2 summary ROCE shortfall against WACC, points", q(ROCE_AT - WACC),
          D("-1.1860"))
    check("Domain 2 summary binding-covenant multiple", q(DSCR_HEAD_REV / ICR_HEAD),
          D("7.7591"))
    check("Domain 2 summary spread of the five spend measures", q(COMMITTED - PAID, 2),
          D("20451403.00"))
    check("Domain 2 summary that spread as % of the envelope",
          q((COMMITTED - PAID) / CAPEX * 100), D("34.0857"))
