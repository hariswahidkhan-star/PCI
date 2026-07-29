"""PFL-AI Domain 14 — Construction monitoring and drawdown: golden checks.

Every number printed as a result in `domain-14-construction-monitoring-drawdown.md` is recomputed
here from first principles. Master-thread values are read from ctx, never re-typed.

The domain has six engines:

  1. The quarterly construction funding model (KA 14.1) — for each of financial close plus eight
     construction quarters: interest = opening drawn debt * r/4; requirement = certified spend +
     interest; the split follows the funding order. The certified-spend base is EPC + owner's
     costs + the balancing contingency, and the contingency is solved so that
     fees + development + EPC + owner + contingency + IDC(contingency) = the 60,000,000 envelope.
     Solving reproduces Domain 6's contingency of 3,645,402.76 and IDC of 2,114,597.24. Re-running
     the same model under equity-first and debt-first orders gives 1,338,006 and 2,804,070, a
     spread of 1,466,064; the same model with the 60,000,000 envelope held and the order changed
     re-solves the balancing contingency to 4,388,050.
  2. The restated sources and uses and the in-balance test (KA 14.1.1, 14.2.1) — drawn, remaining
     and available columns; available commitment is undrawn debt + uncalled equity and EXCLUDES
     contingency, which is a use. The lender's cost-to-complete is built on a commitment basis and
     reconciles to the bottom-up EVM CTC through three bridge lines (approved variations, assessed
     claim exposure, remaining IDC) that earned value cannot see.
  3. Disaggregated earned value (KA 14.2.1) — under a milestone-certified fixed lump sum EV = AC on
     that scope, so its CPI is identically 1.000; a blended CPI applied through BAC/CPI therefore
     spreads an owner-scope overrun across a scope that cannot overrun, and the misattribution is
     EPC * (1/CPI - 1) against OWN * (1/CPI - 1).
  4. Contingency coverage on the remainder (KA 14.2.2) — register mean = sum(p*impact), variance =
     sum(p(1-p)impact^2), P80 = mean + 0.8416 sigma (the normal approximation of Domain 8, KA 8.3),
     computed for the sanction register and for the open register, with the coverage ratio taken on
     four different denominators to show the 1.3825 / 0.8502 / 0.2893 spread.
  5. Certification, retention and advance payment (KA 14.3) — the same quarter's work valued on
     milestone, measured (PoC) and cost-incurred bases, each net of retention and split by gearing;
     and the funding model re-run with a 10 % advance recovered pro rata and 5 % retention capped at
     2.5 %, which prices the advance at 254,597 of capitalised interest.
  6. Delay (KA 14.4) — the funded cost of a month of slip (interest on drawn debt + prolongation)
     against the economic cost (which adds forgone CFADS/12), each measured against 20,000/day of
     damages; and coverage at a calendar-fixed first repayment date, where m months of slip leave
     (12 - m)/12 of a year's CFADS against a full year's debt service, inverted to give the
     covenant, payment and reserve breakevens.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DEBT = ctx["KESTREL_DEBT"]                       # 42,000,000
    EQUITY = ctx["KESTREL_EQUITY"]                   # 18,000,000
    ENV = ctx["KESTREL_CAPEX"]                       # 60,000,000
    RATE = ctx["KESTREL_RATE"]                       # 0.06
    TEN = ctx["KESTREL_TENOR"]                       # 12
    DS = ctx["KESTREL_INSTALMENT"]                   # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                        # 6,384,000
    NPV4 = ctx["KESTREL_NPV"]                        # 16,179,360
    RDISC = ctx["KESTREL_DISCOUNT"]                  # 0.08
    LIFE = ctx["KESTREL_LIFE"]                       # 25

    EPC = D(48000000)
    OWN = D(3600000)
    DEV = D(1800000)
    FEES = D(840000)
    RQ = RATE / 4                                    # 0.015 quarterly
    PROFILE = [D(p) / 100 for p in ("6", "9", "13", "16", "17", "15", "13", "11")]
    AF6_12 = af(RATE, TEN)

    check("D14 registry AF(0.06,12)", AF6_12.quantize(D("0.000001")), D("8.383844"))
    check("D14 profile sums to unity", sum(PROFILE), 1)
    check("D14 base DSCR", (CF / DS).quantize(D("0.0001")), D("1.2743"))

    # ---------- Engine 1: the quarterly funding model under three funding orders ----------
    def model(cont, order):
        """Return (total IDC, cumulative debt, cumulative equity, per-period rows)."""
        spend_base = EPC + OWN + cont
        close_req = FEES + DEV
        if order == "pro":
            d, e = close_req * D("0.7"), close_req * D("0.3")
        elif order == "eq":
            e = min(close_req, EQUITY)
            d = close_req - e
        else:
            d = min(close_req, DEBT)
            e = close_req - d
        cum_d, cum_e, tot_i = d, e, D(0)
        rows = [(D(0), D(0), close_req, d, e, cum_d, cum_e)]
        for p in PROFILE:
            interest = cum_d * RQ
            tot_i += interest
            spend = spend_base * p
            req = spend + interest
            if order == "pro":
                dd, ee = req * D("0.7"), req * D("0.3")
            elif order == "eq":
                ee = min(req, EQUITY - cum_e)
                dd = req - ee
            else:
                dd = min(req, DEBT - cum_d)
                ee = req - dd
            cum_d += dd
            cum_e += ee
            rows.append((interest, spend, req, dd, ee, cum_d, cum_e))
        return tot_i, cum_d, cum_e, rows

    def solve_contingency(order):
        """Balancing contingency such that total uses equal the 60,000,000 envelope."""
        lo, hi = D(0), D(9000000)
        for _ in range(220):
            mid = (lo + hi) / 2
            idc, _, _, _ = model(mid, order)
            if FEES + DEV + EPC + OWN + mid + idc < ENV:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    CONT = solve_contingency("pro")
    IDC_PRO, CD_PRO, CE_PRO, ROWS = model(CONT, "pro")
    check("D14 D6 contingency reproduced", CONT.quantize(D("0.01")), D("3645402.76"))
    check("D14 D6 IDC pro rata reproduced", IDC_PRO.quantize(D("0.01")), D("2114597.24"))
    check("D14 sources = uses at close", FEES + DEV + EPC + OWN + CONT + IDC_PRO, ENV)
    SPEND_BASE = EPC + OWN + CONT
    check("D14 certified-spend base", SPEND_BASE.quantize(D("1")), D(55245403))
    check("D14 pro-rata debt closes on commitment", CD_PRO.quantize(D("0.01")), DEBT)
    check("D14 pro-rata equity closes on commitment", CE_PRO.quantize(D("0.01")), EQUITY)

    # per-quarter figures the manuscript prints
    check("D14 Q1 interest", ROWS[1][0].quantize(D("1")), D(27720))
    check("D14 Q2 interest", ROWS[2][0].quantize(D("1")), D(62816))
    check("D14 Q3 interest", ROWS[3][0].quantize(D("1")), D(115682))
    check("D14 Q4 interest", ROWS[4][0].quantize(D("1")), D(192307))
    check("D14 Q5 interest", ROWS[5][0].quantize(D("1")), D(287138))
    check("D14 Q8 interest", ROWS[8][0].quantize(D("1")), D(560308))
    check("D14 Q5 planned certified spend", ROWS[5][1].quantize(D("1")), D(9391718))
    check("D14 Q8 certified spend", ROWS[8][1].quantize(D("1")), D(6076994))
    check("D14 IDC to Q5", sum(r[0] for r in ROWS[1:6]).quantize(D("1")), D(685663))
    check("D14 IDC Q6-Q8 planned", sum(r[0] for r in ROWS[6:9]).quantize(D("1")), D(1428934))
    check("D14 cum debt end Q6 ties to D8's 31,990,655",
          ROWS[6][5].quantize(D("1")), D(31990655))
    check("D14 cum debt end Q5 plan", ROWS[5][5].quantize(D("1")), D(25917751))
    check("D14 cum equity end Q5 plan", ROWS[5][6].quantize(D("1")), D(11107608))
    check("D14 opening debt balance Q5", ROWS[4][5].quantize(D("1")), D(19142551))
    check("D14 Q1 interest share of requirement",
          (ROWS[1][0] / ROWS[1][2] * 100).quantize(D("0.01")), D("0.83"))
    check("D14 Q8 interest share of requirement",
          (ROWS[8][0] / ROWS[8][2] * 100).quantize(D("0.01")), D("8.44"))
    check("D14 Q8 interest share of spend (MCQ 14.1-C distractor C)",
          (ROWS[8][0] / ROWS[8][1] * 100).quantize(D("0.01")), D("9.22"))
    check("D14 IDC share of envelope (D6, MCQ distractor D)",
          (IDC_PRO / ENV * 100).quantize(D("0.01")), D("3.52"))

    IDC_EQ, CD_EQ, CE_EQ, ROWS_EQ = model(CONT, "eq")
    IDC_DB, CD_DB, CE_DB, ROWS_DB = model(CONT, "dbt")
    check("D14 IDC equity-first", IDC_EQ.quantize(D("1")), D(1338006))
    check("D14 IDC debt-first", IDC_DB.quantize(D("1")), D(2804070))
    check("D14 equity-first saving", (IDC_PRO - IDC_EQ).quantize(D("1")), D(776591))
    check("D14 equity-first saving %",
          ((IDC_PRO - IDC_EQ) / IDC_PRO * 100).quantize(D("0.01")), D("36.73"))
    check("D14 debt-first cost", (IDC_DB - IDC_PRO).quantize(D("1")), D(689473))
    check("D14 debt-first cost %",
          ((IDC_DB - IDC_PRO) / IDC_PRO * 100).quantize(D("0.01")), D("32.61"))
    check("D14 funding-order spread", (IDC_DB - IDC_EQ).quantize(D("1")), D(1466064))
    check("D14 spread as % of envelope",
          ((IDC_DB - IDC_EQ) / ENV * 100).quantize(D("0.0001")), D("2.4434"))
    check("D14 equity-first total uses",
          (FEES + DEV + EPC + OWN + CONT + IDC_EQ).quantize(D("1")), D(59223409))
    check("D14 debt-first total uses",
          (FEES + DEV + EPC + OWN + CONT + IDC_DB).quantize(D("1")), D(60689473))
    check("D14 equity-first debt drawn", CD_EQ.quantize(D("1")), D(41223409))
    check("D14 debt-first equity drawn", CE_DB.quantize(D("1")), D(18689473))
    check("D14 equity-first D/E", (CD_EQ / CE_EQ).quantize(D("0.0001")), D("2.2902"))
    check("D14 pro-rata D/E", (CD_PRO / CE_PRO).quantize(D("0.0001")), D("2.3333"))
    check("D14 debt-first D/E", (CD_DB / CE_DB).quantize(D("0.0001")), D("2.2473"))

    # sponsor share of cumulative funding drawn (Fig 14.1.1 left panel)
    def share(rows, i):
        d, e = rows[i][5], rows[i][6]
        return (e / (d + e) * 100).quantize(D("0.01"))

    check("D14 fig pro-rata share flat", share(ROWS, 0), D("30.00"))
    check("D14 fig pro-rata share Q8", share(ROWS, 8), D("30.00"))
    check("D14 fig equity-first share close", share(ROWS_EQ, 0), D("100.00"))
    check("D14 fig equity-first share Q3", share(ROWS_EQ, 3), D("99.40"))
    check("D14 fig equity-first share Q4", share(ROWS_EQ, 4), D("66.79"))
    check("D14 fig equity-first share Q5", share(ROWS_EQ, 5), D("49.35"))
    check("D14 fig equity-first share Q6", share(ROWS_EQ, 6), D("39.96"))
    check("D14 fig equity-first share Q7", share(ROWS_EQ, 7), D("34.20"))
    check("D14 fig equity-first share Q8", share(ROWS_EQ, 8), D("30.39"))
    check("D14 fig debt-first share Q5", share(ROWS_DB, 5), D("0.00"))
    check("D14 fig debt-first share Q6", share(ROWS_DB, 6), D("9.03"))
    check("D14 fig debt-first share Q7", share(ROWS_DB, 7), D("22.20"))
    check("D14 fig debt-first share Q8", share(ROWS_DB, 8), D("30.80"))
    check("D14 debt-first exposure at Q5", ROWS_DB[5][5].quantize(D("1")), D(37323907))
    check("D14 exposure difference at Q5",
          (ROWS_DB[5][5] - ROWS[5][5]).quantize(D("1")), D(11406157))

    # the ten-basis-point comparison
    SUM_OPENING = IDC_PRO / RQ                       # quarter-dollars of drawn balance
    TENBP = SUM_OPENING * (D("0.001") / 4)
    check("D14 sum of opening balances", SUM_OPENING.quantize(D("1")), D(140973150))
    check("D14 10bp cost over the build", TENBP.quantize(D("1")), D(35243))
    check("D14 spread as multiple of 10bp",
          ((IDC_DB - IDC_EQ) / TENBP).quantize(D("0.1")), D("41.6"))

    # fixed envelope, equity-first: the balancing contingency re-solves upward
    CONT_EQ = solve_contingency("eq")
    IDC_EQ2, CD_EQ2, CE_EQ2, _ = model(CONT_EQ, "eq")
    check("D14 equity-first contingency at fixed envelope",
          CONT_EQ.quantize(D("1")), D(4388050))
    check("D14 equity-first contingency as % of EPC",
          (CONT_EQ / EPC * 100).quantize(D("0.01")), D("9.14"))
    check("D14 extra contingency bought by the clause",
          (CONT_EQ - CONT).quantize(D("1")), D(742647))
    check("D14 pro-rata contingency as % of EPC (D6)",
          (CONT / EPC * 100).quantize(D("0.01")), D("7.59"))

    # present value of the equity draw stream, quarterly at the 8 % board rate
    RQE = (1 + RDISC) ** (D(1) / 4) - 1
    check("D14 quarterly equity discount rate",
          (RQE * 100).quantize(D("0.0001")), D("1.9427"))

    def pv_equity(rows):
        return sum(rows[t][4] / (1 + RQE) ** t for t in range(len(rows)))

    check("D14 PV of pro-rata equity draws", pv_equity(ROWS).quantize(D("1")), D(16476605))
    check("D14 PV of debt-first equity draws", pv_equity(ROWS_DB).quantize(D("1")), D(16293592))
    check("D14 debt-first PV advantage to the sponsor",
          (pv_equity(ROWS) - pv_equity(ROWS_DB)).quantize(D("1")), D(183013))

    # ---------- Engine 2: the quarter-five data date, restated ----------
    PLAN_SPEND_Q5 = SPEND_BASE * D("0.61")
    EPC_CERT = EPC * D("0.61")
    AC_OWN = D(4200000)
    EV_OWN = OWN * D("0.70")
    VAR_CERT = D("465402.76")
    ACT_SPEND_Q5 = EPC_CERT + AC_OWN + VAR_CERT
    EXTRA = ACT_SPEND_Q5 - PLAN_SPEND_Q5
    check("D14 planned cum certified spend Q5", PLAN_SPEND_Q5.quantize(D("1")), D(33699696))
    check("D14 EPC certified at 61 %", EPC_CERT, D(29280000))
    check("D14 owner EV at 70 %", EV_OWN, D(2520000))
    check("D14 actual cum certified spend Q5", ACT_SPEND_Q5.quantize(D("1")), D(33945403))
    check("D14 extra over plan drawn in Q5", EXTRA.quantize(D("1")), D(245707))
    check("D14 extra debt share", (EXTRA * D("0.7")).quantize(D("1")), D(171995))
    check("D14 extra equity share", (EXTRA * D("0.3")).quantize(D("1")), D(73712))

    CONT_DRAWN = (AC_OWN - EV_OWN) + VAR_CERT
    CONT_REM = CONT - CONT_DRAWN
    check("D14 owner-scope overrun to date", AC_OWN - EV_OWN, D(1680000))
    check("D14 contingency drawn", CONT_DRAWN.quantize(D("0.01")), D("2145402.76"))
    check("D14 contingency remaining is exactly 1,500,000", CONT_REM.quantize(D("0.01")),
          D("1500000.00"))
    check("D14 draw-rate check (the misleading one)",
          (CONT_DRAWN / CONT * 100).quantize(D("0.01")), D("58.85"))

    CUM_D = ROWS[5][5] + EXTRA * D("0.7")
    CUM_E = ROWS[5][6] + EXTRA * D("0.3")
    UNDRAWN_D = DEBT - CUM_D
    UNCALLED_E = EQUITY - CUM_E
    AVAIL = UNDRAWN_D + UNCALLED_E
    check("D14 cum debt drawn restated", CUM_D.quantize(D("1")), D(26089746))
    check("D14 cum equity restated", CUM_E.quantize(D("1")), D(11181320))
    check("D14 undrawn debt", UNDRAWN_D.quantize(D("1")), D(15910254))
    check("D14 uncalled equity", UNCALLED_E.quantize(D("1")), D(6818680))
    check("D14 available commitment", AVAIL.quantize(D("1")), D(22728934))
    check("D14 drawn share of debt commitment",
          (CUM_D / DEBT * 100).quantize(D("0.01")), D("62.12"))
    check("D14 drawn share of equity commitment",
          (CUM_E / EQUITY * 100).quantize(D("0.01")), D("62.12"))

    Q5_REQ = ROWS[5][1] + EXTRA + ROWS[5][0]
    check("D14 quarter-five funding requirement", Q5_REQ.quantize(D("0.01")), D("9924563.82"))
    check("D14 quarter-five funding requirement, displayed", Q5_REQ.quantize(D("1")), D(9924564))
    check("D14 quarter-five debt draw", (Q5_REQ * D("0.7")).quantize(D("1")), D(6947195))
    check("D14 quarter-five equity draw", (Q5_REQ * D("0.3")).quantize(D("1")), D(2977369))

    EXTRA_INT = EXTRA * D("0.7") * RQ * 3
    IDC_REM = sum(r[0] for r in ROWS[6:9]) + EXTRA_INT
    check("D14 extra interest on the early draw", EXTRA_INT.quantize(D("1")), D(7740))
    check("D14 remaining IDC restated", IDC_REM.quantize(D("1")), D(1436674))
    check("D14 total IDC restated",
          (sum(r[0] for r in ROWS[1:6]) + IDC_REM).quantize(D("1")), D(2122337))

    DRAWN_COL = FEES + DEV + EPC_CERT + EV_OWN + CONT_DRAWN + sum(r[0] for r in ROWS[1:6])
    check("D14 restated drawn column", DRAWN_COL.quantize(D("1")), D(37271066))
    check("D14 restated drawn column ties to sources",
          DRAWN_COL.quantize(D("0.01")), (CUM_D + CUM_E).quantize(D("0.01")))
    REM_BASE = SPEND_BASE * D("0.39") - EXTRA
    check("D14 remaining planned base scope is exactly 21,300,000",
          REM_BASE.quantize(D("0.01")), D("21300000.00"))
    check("D14 remaining base scope decomposes",
          (EPC - EPC_CERT) + OWN * D("0.30") + CONT_REM, REM_BASE)
    check("D14 remaining EPC contract value", EPC - EPC_CERT, D(18720000))
    check("D14 remaining owner budget", OWN * D("0.30"), D(1080000))
    check("D14 restated remaining uses", (REM_BASE + IDC_REM).quantize(D("1")), D(22736674))
    check("D14 out of balance on the plan by the early-draw interest",
          (AVAIL - (REM_BASE + IDC_REM)).quantize(D("1")), D(-7740))
    check("D14 restated total uses column",
          (FEES + DEV + EPC + OWN + CONT + sum(r[0] for r in ROWS[1:6]) + IDC_REM)
          .quantize(D("1")), D(60007740))

    # ---------- Engine 3: disaggregated earned value, and the lender's cost-to-complete ----------
    BAC = EPC + OWN
    EV = EPC_CERT + EV_OWN
    AC = EPC_CERT + AC_OWN
    CPI = EV / AC
    check("D14 BAC", BAC, D(51600000))
    check("D14 EV", EV, D(31800000))
    check("D14 AC", AC, D(33480000))
    check("D14 blended CPI", CPI.quantize(D("0.000001")), D("0.949821"))
    check("D14 CPI on milestone-certified fixed-price scope is identically 1",
          (EPC_CERT / EPC_CERT).quantize(D("0.0001")), D("1.0000"))
    check("D14 owner-scope CPI", (EV_OWN / AC_OWN).quantize(D("0.0001")), D("0.6000"))

    EAC_A = AC + (BAC - EV)
    EAC_B = BAC / CPI
    OWN_REM_BU = D(2400000)
    EAC_D = EPC + AC_OWN + OWN_REM_BU
    check("D14 EAC(a) budgeted rate", EAC_A, D(53280000))
    check("D14 EAC(b) BAC/CPI", EAC_B.quantize(D("1")), D(54326038))
    check("D14 EAC(d) bottom-up", EAC_D, D(54600000))
    check("D14 CTC(a)", EAC_A - AC, D(19800000))
    check("D14 CTC(b)", (EAC_B - AC).quantize(D("1")), D(20846038))
    check("D14 CTC(d)", EAC_D - AC, D(21120000))
    check("D14 CTC(d) decomposes to remaining EPC + bottom-up owner scope",
          (EPC - EPC_CERT) + OWN_REM_BU, EAC_D - AC)
    check("D14 EAC(b) uplift over BAC", (EAC_B - BAC).quantize(D("1")), D(2726038))
    check("D14 uplift misattributed to fixed-price scope",
          (EPC * (1 / CPI - 1)).quantize(D("1")), D(2535849))
    check("D14 uplift attributed to owner scope",
          (OWN * (1 / CPI - 1)).quantize(D("1")), D(190189))
    check("D14 misattribution sums to the uplift",
          ((EPC + OWN) * (1 / CPI - 1)).quantize(D("1")), (EAC_B - BAC).quantize(D("1")))
    check("D14 true owner overrun at CPI 0.60", OWN / D("0.60") - OWN, D(2400000))
    check("D14 bottom-up owner overrun", (AC_OWN + OWN_REM_BU) - OWN, D(3000000))

    VAR_APPR = D(840000)
    CLAIM = D(1260000)
    LCTC = (EPC - EPC_CERT) + VAR_APPR + CLAIM + OWN_REM_BU + IDC_REM
    check("D14 lender cost-to-complete", LCTC.quantize(D("1")), D(24656674))
    check("D14 in-balance shortfall", (AVAIL - LCTC).quantize(D("1")), D(-1927740))
    check("D14 bridge from CTC(d) to the lender's number",
          ((EAC_D - AC) + VAR_APPR + CLAIM + IDC_REM).quantize(D("1")), LCTC.quantize(D("1")))
    INVISIBLE = VAR_APPR + CLAIM + IDC_REM
    check("D14 lines invisible to earned value", INVISIBLE.quantize(D("1")), D(3536674))
    check("D14 invisible lines as % of the lender's number",
          (INVISIBLE / LCTC * 100).quantize(D("0.01")), D("14.34"))
    check("D14 MCQ 14.2-A distractor C (omits remaining IDC)",
          (LCTC - IDC_REM).quantize(D("1")), D(23220000))
    check("D14 MCQ 14.1-A distractor A (double-counts contingency)",
          (AVAIL + CONT_REM).quantize(D("1")), D(24228934))
    check("D14 MCQ 14.1-A distractor D (deducts contingency)",
          (AVAIL - CONT_REM).quantize(D("1")), D(21228934))

    KNOWN = VAR_APPR + CLAIM + (OWN_REM_BU - OWN * D("0.30"))
    check("D14 bottom-up uplift above remaining budget", OWN_REM_BU - OWN * D("0.30"), D(1320000))
    check("D14 known committed claims on contingency", KNOWN, D(3420000))
    check("D14 contingency committed beyond its balance",
          (KNOWN - CONT_REM).quantize(D("1")), D(1920000))
    check("D14 two routes agree: in-balance = contingency excess + early-draw interest",
          (KNOWN - CONT_REM + EXTRA_INT).quantize(D("1")), (LCTC - AVAIL).quantize(D("1")))

    # ---------- Engine 4: risk registers and contingency coverage ----------
    Z80 = D("0.8416")

    def register(items):
        mean = sum(p * i for p, i in items)
        var = sum(p * (1 - p) * i * i for p, i in items)
        sd = var.sqrt()
        return mean, sd, mean + Z80 * sd

    SANCTION = [(D("0.30"), D(2600000)), (D("0.25"), D(1200000)), (D("0.35"), D(900000)),
                (D("0.35"), D(700000)), (D("0.30"), D(600000)), (D("0.40"), D(800000))]
    M_S, SD_S, P80_S = register(SANCTION)
    check("D14 sanction register mean", M_S, D(2140000))
    check("D14 sanction register sigma", SD_S.quantize(D("1")), D(1488136))
    check("D14 sanction register P80", P80_S.quantize(D("1")), D(3392416))
    check("D14 coverage at close", (CONT / P80_S).quantize(D("0.0001")), D("1.0746"))
    check("D14 z of the funded contingency (about P84)",
          ((CONT - M_S) / SD_S).quantize(D("0.0001")), D("1.0116"))

    OPEN = [(D("0.40"), D(900000)), (D("0.25"), D(1200000)),
            (D("0.35"), D(700000)), (D("0.30"), D(600000))]
    M_O, SD_O, P80_O = register(OPEN)
    check("D14 open register mean", M_O, D(1085000))
    check("D14 open register sigma", SD_O.quantize(D("1")), D(807140))
    check("D14 open register P80", P80_O.quantize(D("1")), D(1764289))
    check("D14 coverage on the mean only (the comfortable answer)",
          (CONT_REM / M_O).quantize(D("0.0001")), D("1.3825"))
    check("D14 coverage on open risk at P80 (still the wrong denominator)",
          (CONT_REM / P80_O).quantize(D("0.0001")), D("0.8502"))
    check("D14 honest coverage on the remainder",
          (CONT_REM / (KNOWN + P80_O)).quantize(D("0.0001")), D("0.2893"))
    check("D14 honest denominator", (KNOWN + P80_O).quantize(D("1")), D(5184289))
    check("D14 coverage shortfall", (KNOWN + P80_O - CONT_REM).quantize(D("1")), D(3684289))
    check("D14 MCQ 14.2-C distractor D (known claims only)",
          (CONT_REM / KNOWN).quantize(D("0.0001")), D("0.4386"))

    # ---------- Engine 5: certification, retention and the advance payment ----------
    MILE = D(5760000)
    PART, PC = D(1440000), D("0.92")
    OFFSITE = D(690000)
    MEAS = MILE + PART * PC
    COSTB = MEAS + OFFSITE
    RET = D("0.05")
    check("D14 measured/PoC certified value", MEAS, D(7084800))
    check("D14 cost-incurred certified value", COSTB, D(7774800))
    check("D14 certification spread", COSTB - MILE, D(2014800))
    check("D14 spread as % of the milestone figure",
          ((COSTB - MILE) / MILE * 100).quantize(D("0.01")), D("34.98"))
    check("D14 milestone net of retention", MILE * (1 - RET), D(5472000))
    check("D14 measured net of retention", MEAS * (1 - RET), D(6730560))
    check("D14 cost-incurred net of retention", COSTB * (1 - RET), D(7386060))
    check("D14 milestone debt advanced", MILE * (1 - RET) * D("0.7"), D(3830400))
    check("D14 measured debt advanced", (MEAS * (1 - RET) * D("0.7")).quantize(D("1")),
          D(4711392))
    check("D14 cost-incurred debt advanced", (COSTB * (1 - RET) * D("0.7")).quantize(D("1")),
          D(5170242))
    OVER = (COSTB - MILE) * (1 - RET)
    check("D14 over-certification net of retention", OVER, D(1914060))
    check("D14 debt advanced against work not in place",
          (OVER * D("0.7")).quantize(D("1")), D(1339842))
    check("D14 as % of the 10 % performance bond",
          (OVER * D("0.7") / D(4800000) * 100).quantize(D("0.01")), D("27.91"))
    check("D14 extra IDC on early certification",
          (OVER * D("0.7") * RQ * 2).quantize(D("1")), D(40195))

    def idc_of(cash):
        """Capitalised interest for a period-end draw schedule at 70 % gearing."""
        cum_d, tot = D(0), D(0)
        for t, c in enumerate(cash):
            i = cum_d * RQ if t > 0 else D(0)
            tot += i
            cum_d += (c + i) * D("0.7")
        return tot

    BASE_CASH = [FEES + DEV] + [SPEND_BASE * p for p in PROFILE]
    check("D14 idc_of reproduces the base model", idc_of(BASE_CASH).quantize(D("0.01")),
          IDC_PRO.quantize(D("0.01")))
    ADV = EPC * D("0.10")
    RET_CAP = EPC * D("0.025")
    check("D14 advance payment", ADV, D(4800000))
    check("D14 retention cap", RET_CAP, D(1200000))
    ADV_CASH = [FEES + DEV + ADV] + [SPEND_BASE * p - ADV * p for p in PROFILE]
    RET_CASH, held = [FEES + DEV], D(0)
    for p in PROFILE:
        r = min(EPC * p * RET, max(RET_CAP - held, D(0)))
        held += r
        RET_CASH.append(SPEND_BASE * p - r)
    check("D14 retention held at completion reaches the cap", held, RET_CAP)
    BOTH_CASH, held2 = [FEES + DEV + ADV], D(0)
    for p in PROFILE:
        r = min(EPC * p * RET, max(RET_CAP - held2, D(0)))
        held2 += r
        BOTH_CASH.append(SPEND_BASE * p - ADV * p - r)
    check("D14 IDC with the advance only", idc_of(ADV_CASH).quantize(D("1")), D(2369194))
    check("D14 IDC with retention only", idc_of(RET_CASH).quantize(D("1")), D(2052008))
    check("D14 IDC with retention only, full precision",
          idc_of(RET_CASH).quantize(D("0.0001")), D("2052008.4970"))
    check("D14 IDC with both terms", idc_of(BOTH_CASH).quantize(D("1")), D(2306605))
    check("D14 cost of the advance in capitalised interest",
          (idc_of(ADV_CASH) - IDC_PRO).quantize(D("1")), D(254597))
    check("D14 saving from retention",
          (idc_of(RET_CASH) - IDC_PRO).quantize(D("1")), D(-62589))
    check("D14 net effect of both terms",
          (idc_of(BOTH_CASH) - IDC_PRO).quantize(D("1")), D(192008))
    check("D14 advance cost as % of the advance",
          ((idc_of(ADV_CASH) - IDC_PRO) / ADV * 100).quantize(D("0.01")), D("5.30"))
    check("D14 advance cost as % of the EPC price",
          ((idc_of(ADV_CASH) - IDC_PRO) / EPC * 100).quantize(D("0.01")), D("0.53"))

    # the retention release that lands in the first operating year
    RELEASE = RET_CAP / 2
    TRIGGER = DS * D("1.20")
    HEADROOM = CF - TRIGGER
    check("D14 covenant cash trigger (D10)", TRIGGER.quantize(D("0.01")), D("6011562.28"))
    check("D14 annual headroom (D10)", HEADROOM.quantize(D("0.01")), D("372437.72"))
    check("D14 retention release tranche", RELEASE, D(600000))
    check("D14 CFADS net of the release", CF - RELEASE, D(5784000))
    check("D14 DSCR after the release breaches the covenant",
          ((CF - RELEASE) / DS).quantize(D("0.0001")), D("1.1546"))
    check("D14 release exceeds headroom by",
          (RELEASE - HEADROOM).quantize(D("0.01")), D("227562.28"))

    # ---------- Engine 5b: the marginal coverage of a variation ----------
    CVAR, DCF = D(1850000), D(240000)
    AF8_25 = af(RDISC, LIFE)
    check("D14 AF(0.08,25)", AF8_25.quantize(D("0.000001")), D("10.674776"))
    check("D14 variation PV of inflows", (DCF * AF8_25).quantize(D("1")), D(2561946))
    check("D14 variation NPV", (DCF * AF8_25 - CVAR).quantize(D("1")), D(711946))
    check("D14 variation PI", (DCF * AF8_25 / CVAR).quantize(D("0.0001")), D("1.3848"))
    DDS_FULL = CVAR / AF6_12
    check("D14 variation delta debt service, fully debt funded",
          DDS_FULL.quantize(D("1")), D(220663))
    check("D14 marginal DSCR, fully debt funded",
          (DCF / DDS_FULL).quantize(D("0.0001")), D("1.0876"))
    DDS_70 = CVAR * D("0.7") / AF6_12
    check("D14 variation debt share at 70 %", CVAR * D("0.7"), D(1295000))
    check("D14 variation delta debt service at 70 %", DDS_70.quantize(D("1")), D(154464))
    check("D14 marginal DSCR at 70 % gearing",
          (DCF / DDS_70).quantize(D("0.0001")), D("1.5538"))
    MAXDEBT = (DCF / (CF / DS)) * AF6_12
    check("D14 coverage-neutral maximum debt funding", MAXDEBT.quantize(D("1")), D(1578947))
    check("D14 coverage-neutral share of cost",
          (MAXDEBT / CVAR * 100).quantize(D("0.01")), D("85.35"))
    check("D14 residual from equity or contingency",
          (CVAR - MAXDEBT).quantize(D("1")), D(271053))
    check("D14 CFADS needed for coverage neutrality at full debt funding",
          (DDS_FULL * (CF / DS)).quantize(D("1")), D(281200))
    check("D14 CFADS shortfall against 240,000",
          (DDS_FULL * (CF / DS) - DCF).quantize(D("1")), D(41200))
    check("D14 MCQ 14.3-C distractor D (no coverage divisor)",
          (DCF * AF6_12).quantize(D("1")), D(2012123))
    NEWD = DEBT + CVAR
    NEWDS = NEWD / AF6_12
    check("D14 blended debt if fully debt funded", NEWD, D(43850000))
    check("D14 blended instalment", NEWDS.quantize(D("0.01")), D("5230297.74"))
    check("D14 blended DSCR", ((CF + DCF) / NEWDS).quantize(D("0.0001")), D("1.2665"))
    check("D14 blended covenant trigger",
          (NEWDS * D("1.20")).quantize(D("1")), D(6276357))
    check("D14 blended headroom",
          (CF + DCF - NEWDS * D("1.20")).quantize(D("1")), D(347643))
    check("D14 headroom lost",
          (HEADROOM - (CF + DCF - NEWDS * D("1.20"))).quantize(D("1")), D(24795))
    check("D14 headroom lost as %",
          ((HEADROOM - (CF + DCF - NEWDS * D("1.20"))) / HEADROOM * 100).quantize(D("0.01")),
          D("6.66"))

    # ---------- Engine 6: delay ----------
    INT_M = DEBT * RATE / 12
    TEAM, INS, ADVFEE = D(138000), D(46000), D(21000)
    FUNDED = INT_M + TEAM + INS + ADVFEE
    FORGONE = CF / 12
    ECON_D5 = INT_M + FORGONE
    ECON_FULL = FUNDED + FORGONE
    LD_M = D(20000) * 30
    LD_CAP = D(4800000)
    check("D14 monthly interest on drawn debt", INT_M, D(210000))
    check("D14 funded cost of a month of slip", FUNDED, D(415000))
    check("D14 prolongation share of the funded cost",
          ((TEAM + INS + ADVFEE) / FUNDED * 100).quantize(D("0.1")), D("49.4"))
    check("D14 forgone CFADS per month", FORGONE, D(532000))
    check("D14 D5's basis reconciles (7,000 + 17,733.33 per day)",
          ECON_D5, D(742000))
    check("D14 full economic cost per month", ECON_FULL, D(947000))
    check("D14 full economic cost per day",
          (ECON_FULL / 30).quantize(D("0.01")), D("31566.67"))
    check("D14 damages recovery of the funded cost",
          (LD_M / FUNDED * 100).quantize(D("0.01")), D("144.58"))
    check("D14 damages recovery of D5's basis",
          (LD_M / ECON_D5 * 100).quantize(D("0.01")), D("80.86"))
    check("D14 damages recovery of the full economic cost",
          (LD_M / ECON_FULL * 100).quantize(D("0.01")), D("63.36"))
    check("D14 cap binds at day 240", LD_CAP / D(20000), 240)
    check("D14 cap binds at month 8", (LD_CAP / LD_M).quantize(D("0.01")), D("8.00"))
    check("D14 months of funded cost the cap covers",
          (LD_CAP / FUNDED).quantize(D("0.0001")), D("11.5663"))
    check("D14 months of economic cost the cap covers",
          (LD_CAP / ECON_FULL).quantize(D("0.0001")), D("5.0686"))
    check("D14 eight-month funded cost", FUNDED * 8, D(3320000))
    check("D14 eight-month damages surplus over funded cost", LD_CAP - FUNDED * 8, D(1480000))
    check("D14 eight-month economic cost", ECON_FULL * 8, D(7576000))
    check("D14 eight-month economic cost uncovered", ECON_FULL * 8 - LD_CAP, D(2776000))
    check("D14 availability cushion funds six months", FUNDED * 6, D(2490000))
    check("D14 timing exposure beyond the availability period",
          ((LD_CAP / FUNDED - 6) * FUNDED).quantize(D("1")), D(2310000))
    check("D14 two months of economic cost (14.A.2)", ECON_FULL * 2, D(1894000))

    def dscr_at(m):
        return CF * (12 - m) / 12 / DS

    for months, want in ((D(0), "1.2743"), (D(1), "1.1681"), (D(2), "1.0620"),
                         (D(3), "0.9558"), (D(4), "0.8496"), (D(6), "0.6372"),
                         (D(8), "0.4248")):
        check(f"D14 DSCR at first test, slip {months} months",
              dscr_at(months).quantize(D("0.0001")), D(want))
    check("D14 CFADS at first test, 4-month slip", CF * 8 / 12, D(4256000))
    check("D14 shortfall at 3 months", (DS - CF * 9 / 12).quantize(D("0.01")), D("221635.23"))
    check("D14 shortfall at 4 months", (DS - CF * 8 / 12).quantize(D("0.01")), D("753635.23"))
    check("D14 shortfall at 6 months", (DS - CF * 6 / 12).quantize(D("0.01")), D("1817635.23"))
    check("D14 shortfall at 8 months", (DS - CF * 4 / 12).quantize(D("0.01")), D("2881635.23"))
    DSRA = DS / 2
    check("D14 six-month DSRA (D10)", DSRA.quantize(D("0.01")), D("2504817.62"))
    check("D14 DSRA consumed at 3 months",
          ((DS - CF * 9 / 12) / DSRA * 100).quantize(D("0.01")), D("8.85"))
    check("D14 DSRA consumed at 4 months",
          ((DS - CF * 8 / 12) / DSRA * 100).quantize(D("0.01")), D("30.09"))
    check("D14 DSRA consumed at 6 months",
          ((DS - CF * 6 / 12) / DSRA * 100).quantize(D("0.01")), D("72.57"))
    check("D14 DSRA consumed at 8 months",
          ((DS - CF * 4 / 12) / DSRA * 100).quantize(D("0.01")), D("115.04"))

    M_COV = 12 * (1 - TRIGGER / CF)
    M_PAY = 12 * (1 - DS / CF)
    M_DSRA = 12 * (1 - (DS - DSRA) / CF)
    check("D14 covenant breakeven slip, months", M_COV.quantize(D("0.0001")), D("0.7001"))
    check("D14 covenant breakeven slip, days", (M_COV * 30).quantize(D("0.01")), D("21.00"))
    check("D14 payment breakeven slip, months", M_PAY.quantize(D("0.0001")), D("2.5834"))
    check("D14 payment breakeven slip, days", (M_PAY * 30).quantize(D("0.01")), D("77.50"))
    check("D14 cash-plus-DSRA breakeven slip, months",
          M_DSRA.quantize(D("0.0001")), D("7.2917"))
    check("D14 cash-plus-DSRA breakeven slip, days",
          (M_DSRA * 30).quantize(D("0.01")), D("218.75"))

    LD4 = D(20000) * 120
    check("D14 damages over a four-month slip", LD4, D(2400000))
    check("D14 CFADS with damages credited", CF * 8 / 12 + LD4, D(6656000))
    check("D14 DSCR with damages credited",
          ((CF * 8 / 12 + LD4) / DS).quantize(D("0.0001")), D("1.3286"))
    check("D14 value of the CFADS definition at the first test",
          ((CF * 8 / 12 + LD4) / DS - CF * 8 / 12 / DS).quantize(D("0.0001")), D("0.4791"))

    def dscr_ld(m):
        return (CF * (12 - m) / 12 + min(D(20000) * 30 * m, LD_CAP)) / DS

    check("D14 damages-credited peak at the cap", dscr_ld(D(8)).quantize(D("0.0001")),
          D("1.3829"))
    check("D14 damages-credited at 12 months", dscr_ld(D(12)).quantize(D("0.0001")),
          D("0.9582"))
    check("D14 damages-credited re-crosses 1.20",
          (12 * (1 - (D("1.20") * DS - LD_CAP) / CF)).quantize(D("0.0001")), D("9.7226"))
    check("D14 damages-credited re-crosses 1.00",
          (12 * (1 - (DS - LD_CAP) / CF)).quantize(D("0.0001")), D("11.6059"))

    # buy-down at a failed performance test
    CF_LOW = D(5900000)
    check("D14 DSCR as completed at CFADS 5,900,000",
          (CF_LOW / DS).quantize(D("0.0001")), D("1.1777"))
    D_BASE = (CF_LOW / (CF / DS)) * AF6_12
    D_COV = (CF_LOW / D("1.20")) * AF6_12
    check("D14 debt supportable at base coverage", D_BASE.quantize(D("1")), D(38815789))
    check("D14 buy-down to restore base coverage",
          (DEBT - D_BASE).quantize(D("1")), D(3184211))
    check("D14 debt supportable at the covenant", D_COV.quantize(D("1")), D(41220566))
    check("D14 buy-down to restore the covenant only",
          (DEBT - D_COV).quantize(D("1")), D(779434))
    check("D14 the drafting difference between the two buy-downs",
          ((DEBT - D_BASE) - (DEBT - D_COV)).quantize(D("1")), D(2404777))
    check("D14 both buy-downs fit inside the performance cap",
          1 if DEBT - D_BASE < LD_CAP else 0, 1)
    check("D14 debt after the full cap", DEBT - LD_CAP, D(37200000))
    check("D14 instalment after the full cap",
          ((DEBT - LD_CAP) / AF6_12).quantize(D("0.01")), D("4437105.49"))
    check("D14 coverage after the full cap",
          (CF_LOW / ((DEBT - LD_CAP) / AF6_12)).quantize(D("0.0001")), D("1.3297"))
    check("D14 output shortfall as % of base CFADS",
          ((CF - CF_LOW) / CF * 100).quantize(D("0.01")), D("7.58"))

    # ---------- Case study A: the resolution ----------
    INJ = LCTC - AVAIL
    check("D14 case A injection", INJ.quantize(D("1")), D(1927740))
    check("D14 case A equity after injection", (EQUITY + INJ).quantize(D("1")), D(19927740))
    check("D14 case A total funding", (ENV + INJ).quantize(D("1")), D(61927740))
    check("D14 case A gearing debt %",
          (DEBT / (ENV + INJ) * 100).quantize(D("0.01")), D("67.82"))
    check("D14 case A gearing equity %",
          ((EQUITY + INJ) / (ENV + INJ) * 100).quantize(D("0.01")), D("32.18"))
    check("D14 case A D/E", (DEBT / (EQUITY + INJ)).quantize(D("0.0001")), D("2.1076"))
    check("D14 case A equity increase %", (INJ / EQUITY * 100).quantize(D("0.01")), D("10.71"))
    check("D14 case A NPV effect", (NPV4 - INJ).quantize(D("1")), D(14251620))
    check("D14 case A coverage ratios untouched (debt unchanged)",
          (CF / DS).quantize(D("0.0001")), D("1.2743"))

    # ---------- Case study B: the unvested off-site certification ----------
    ENV_B, DEBT_B, EQ_B, CONT_B = D(420000000), D(294000000), D(126000000), D(12600000)
    UNVESTED = D(16400000)
    check("D14 case B envelope balances", DEBT_B + EQ_B, ENV_B)
    check("D14 case B unvested as % of envelope",
          (UNVESTED / ENV_B * 100).quantize(D("0.0001")), D("3.9048"))
    check("D14 case B unvested as % of funded contingency",
          (UNVESTED / CONT_B * 100).quantize(D("0.01")), D("130.16"))
    DSHARE_B = UNVESTED * D("0.7")
    LOSS_B = UNVESTED * D("0.75")
    CARRY_B = DSHARE_B * D("0.055") * D(14) / 12
    check("D14 case B debt share advanced", DSHARE_B, D(11480000))
    check("D14 case B recovery at 25 %", UNVESTED * D("0.25"), D(4100000))
    check("D14 case B loss", LOSS_B, D(12300000))
    check("D14 case B carry interest", CARRY_B.quantize(D("1")), D(736633))
    check("D14 case B total cost", (LOSS_B + CARRY_B).quantize(D("1")), D(13036633))
    check("D14 case B cost as % of equity",
          ((LOSS_B + CARRY_B) / EQ_B * 100).quantize(D("0.01")), D("10.35"))
    check("D14 case B equity call after contingency",
          LOSS_B - D(5600000), D(6700000))
    check("D14 case B gearing debt % after the call",
          (DEBT_B / (DEBT_B + EQ_B + D(6700000)) * 100).quantize(D("0.01")), D("68.90"))
    check("D14 case B gearing equity % after the call",
          ((EQ_B + D(6700000)) / (DEBT_B + EQ_B + D(6700000)) * 100).quantize(D("0.01")),
          D("31.10"))

    # ---------- Calculation exercises ----------
    # 14.1 in-balance test
    AVAIL_E1 = D(180000000) - D(96000000)
    CTC_E1 = D(74000000) + D(3200000) + D(2600000) + D(4100000) + D(2900000)
    check("EX 14.1 available commitment", AVAIL_E1, D(84000000))
    check("EX 14.1 cost to complete", CTC_E1, D(86800000))
    check("EX 14.1 shortfall", AVAIL_E1 - CTC_E1, D(-2800000))
    check("EX 14.1 common error double-counts contingency",
          AVAIL_E1 + D(5000000), D(89000000))
    check("EX 14.1 common error surplus", AVAIL_E1 + D(5000000) - CTC_E1, D(2200000))

    # 14.2 IDC under two funding orders
    def ex2(order):
        rq2, g2, eqc = D("0.08") / 4, D("0.75"), D(20000000)
        cum_d, cum_e, tot = D(0), D(0), D(0)
        for _ in range(4):
            i = cum_d * rq2
            tot += i
            req = D(20000000) + i
            if order == "pro":
                dd, ee = req * g2, req * (1 - g2)
            else:
                ee = min(req, eqc - cum_e)
                dd = req - ee
            cum_d += dd
            cum_e += ee
        return tot, cum_d, cum_e

    T_PRO, CD2, CE2 = ex2("pro")
    T_EQ, CD3, CE3 = ex2("eq")
    check("EX 14.2 pro-rata IDC", T_PRO.quantize(D("0.01")), D("1818067.50"))
    check("EX 14.2 pro-rata total uses", (D(80000000) + T_PRO).quantize(D("1")), D(81818068))
    check("EX 14.2 pro-rata debt", CD2.quantize(D("1")), D(61363551))
    check("EX 14.2 pro-rata equity", CE2.quantize(D("1")), D(20454517))
    check("EX 14.2 equity-first IDC", T_EQ, D(1208000))
    check("EX 14.2 equity-first total uses", D(80000000) + T_EQ, D(81208000))
    check("EX 14.2 saving", (T_PRO - T_EQ).quantize(D("1")), D(610068))
    check("EX 14.2 saving %", ((T_PRO - T_EQ) / T_PRO * 100).quantize(D("0.01")), D("33.56"))

    # 14.3 advance recovery and retention
    P3, CERT3, PREV3 = D(90000000), D(7200000), D(36000000)
    ADV3, CAP3 = P3 * D("0.15"), P3 * D("0.05")
    REC3 = CERT3 * D("0.15")
    HELD3 = PREV3 * D("0.10")
    RET3 = min(CERT3 * D("0.10"), CAP3 - HELD3)
    check("EX 14.3 advance", ADV3, D(13500000))
    check("EX 14.3 retention cap", CAP3, D(4500000))
    check("EX 14.3 advance recovery this month", REC3, D(1080000))
    check("EX 14.3 retention held before", HELD3, D(3600000))
    check("EX 14.3 retention this month", RET3, D(720000))
    check("EX 14.3 net payment", CERT3 - REC3 - RET3, D(5400000))
    check("EX 14.3 cumulative retention held", HELD3 + RET3, D(4320000))
    check("EX 14.3 common error retention on net", (CERT3 - REC3) * D("0.10"), D(612000))
    check("EX 14.3 common error net payment",
          CERT3 - REC3 - (CERT3 - REC3) * D("0.10"), D(5508000))
    check("EX 14.3 over-payment", RET3 - (CERT3 - REC3) * D("0.10"), D(108000))

    # 14.4 marginal coverage of a variation
    AF65_15 = af(D("0.065"), 15)
    check("EX 14.4 AF(0.065,15)", AF65_15.quantize(D("0.000001")), D("9.402669"))
    MAX4 = (D(600000) / D("1.25")) * AF65_15
    check("EX 14.4 maximum coverage-neutral debt", MAX4.quantize(D("1")), D(4513281))
    check("EX 14.4 as % of cost", (MAX4 / D(6200000) * 100).quantize(D("0.01")), D("72.79"))
    check("EX 14.4 equity or contingency required",
          (D(6200000) - MAX4).quantize(D("1")), D(1686719))
    check("EX 14.4 delta debt service, fully debt funded",
          (D(6200000) / AF65_15).quantize(D("1")), D(659387))
    check("EX 14.4 marginal DSCR, fully debt funded",
          (D(600000) / (D(6200000) / AF65_15)).quantize(D("0.0001")), D("0.9099"))
    check("EX 14.4 common error, no coverage divisor",
          (D(600000) * AF65_15).quantize(D("1")), D(5641601))
    check("EX 14.4 over-levered by",
          (D(600000) * AF65_15 - MAX4).quantize(D("1")), D(1128320))

    # 14.5 coverage at a calendar-fixed first repayment date
    CF5, DS5, COV5, DSRA5 = D(14400000), D(11200000), D("1.15"), D(5600000)
    check("EX 14.5 undelayed DSCR", (CF5 / DS5).quantize(D("0.0001")), D("1.2857"))
    check("EX 14.5 CFADS at the first test", CF5 * 9 / 12, D(10800000))
    check("EX 14.5 DSCR at the first test",
          (CF5 * 9 / 12 / DS5).quantize(D("0.0001")), D("0.9643"))
    check("EX 14.5 shortfall", DS5 - CF5 * 9 / 12, D(400000))
    check("EX 14.5 DSRA consumed %",
          ((DS5 - CF5 * 9 / 12) / DSRA5 * 100).quantize(D("0.01")), D("7.14"))
    check("EX 14.5 covenant cash trigger", COV5 * DS5, D(12880000))
    check("EX 14.5 covenant breakeven months",
          (12 * (1 - COV5 * DS5 / CF5)).quantize(D("0.0001")), D("1.2667"))
    check("EX 14.5 payment breakeven months",
          (12 * (1 - DS5 / CF5)).quantize(D("0.0001")), D("2.6667"))
    check("EX 14.5 cash-plus-DSRA breakeven months",
          (12 * (1 - (DS5 - DSRA5) / CF5)).quantize(D("0.0001")), D("7.3333"))
