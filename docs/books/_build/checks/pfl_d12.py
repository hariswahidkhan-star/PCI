"""PFL-AI Domain 12 — Contracts and transaction structure: golden checks.

Every number printed as a result in `domain-12-contracts-transaction-structure.md` is recomputed
here from first principles. Master-thread values are read from ctx, never re-typed.

The domain has five engines:

  1. The CFADS-vs-output line (KA 12.1.3, 12.2.2) — revenue and variable cost scale with the
     output share x, fixed cash cost does not, cash tax at 20 % on (EBITDA - depreciation -
     interest), working capital held at 600,000:  CFADS(x) = 9,060,000x - 2,676,000.  Every
     volume, shortfall and floor result in the domain is a point on this one line.
  2. Delay damages and the cap stack (KA 12.1.2) — daily economic cost = drawn debt * rate / 360
     + annual CFADS / 360; recovery = min(rate * days, cap); cap-binding day = cap / rate;
     residue = exposure - recovery, further limited by the aggregate cap.
  3. Performance damages on three bases (KA 12.1.3) — value = annual CFADS shortfall *
     AF(0.08, 25); sized coverage = debt * shortfall / base CFADS; bare covenant = debt -
     CFADS(new)/1.20 * AF(0.06, 12); plus the tenor-mismatch residue when a 25-year loss is paid
     by a 12-year debt prepayment.
  4. Guarantee sufficiency (KA 12.3.2) — risk-adjusted cover = sum(face * probability of
     payment), the weaker obligor counting only for the increment above the certain layer;
     equivalent face = certain cover / probability.
  5. Claim valuation (KA 12.4.3) — expected award = sum(probability * outcome), discounted to
     today alongside own costs at their timing midpoint; settlement ceiling = PV of fighting less
     the cost of settling; breakeven disputed sum solves k*D + PV(costs) = 0.5D + negotiation.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DEBT = ctx["KESTREL_DEBT"]
    EQUITY = ctx["KESTREL_EQUITY"]
    DS = ctx["KESTREL_INSTALMENT"]
    CF = ctx["KESTREL_CFADS"]
    RATE = ctx["KESTREL_RATE"]
    TEN = ctx["KESTREL_TENOR"]
    LIFE = ctx["KESTREL_LIFE"]
    R8 = ctx["KESTREL_DISCOUNT"]
    NPVP = ctx["KESTREL_NPV"]
    INT1 = ctx["KESTREL_INTEREST_Y1"]
    # The registry's published AF(0.06, 12) literal is the value the manuscript multiplies by.
    AF6_12 = D("8.383844")
    COV, LOCK = D("1.20"), D("1.15")
    EPC = D(48000000)

    # ---------- master-thread anchors this domain reads ----------
    check("D12 AF(0.06,12) registry literal", af(RATE, TEN).quantize(D("0.000001")), AF6_12)
    check("D12 year-one principal", DS - INT1, D("2489635.23"))
    check("D12 DSCR at close", (CF / DS).quantize(D("0.0001")), D("1.2743"))
    check("D12 covenant cash trigger", (DS * COV).quantize(D("0.01")), D("6011562.28"))
    check("D12 lock-up cash trigger", (DS * LOCK).quantize(D("0.01")), D("5761080.51"))
    HEAD = CF - DS * COV
    check("D12 annual covenant headroom", HEAD.quantize(D("0.01")), D("372437.72"))
    check("D12 EPC price is 80 % of capex", EPC / ctx["KESTREL_CAPEX"], D("0.80"))

    # ================= engine 1 — the CFADS-vs-output line =================
    REV, FIXOP, VAROP = D(12000000), D(3825000), D(675000)
    DEP, TAXR, WC = D(2400000), D("0.20"), D(600000)

    def ebitda(x):
        return REV * x - FIXOP - VAROP * x

    def cfads(x):
        e = ebitda(x)
        return e - (e - DEP - INT1) * TAXR - WC

    SLOPE = cfads(D(1)) - cfads(D("0.99"))
    ICPT = -cfads(D(0))
    check("12.1.3 CFADS(1.00) reproduces the documented CFADS", cfads(D(1)), CF)
    check("12.1.3 EBITDA(1.00) reproduces the thread EBITDA", ebitda(D(1)), ctx["KESTREL_EBITDA"])
    check("12.1.3 CFADS(0.97) reproduces Domain 5's figure", cfads(D("0.97")), D(6112200))
    check("12.1.3 CFADS line slope per output point", SLOPE, D(90600))
    check("12.1.3 CFADS line intercept", ICPT, D(2676000))
    check("12.1.3 line identity at 0.95", SLOPE * 100 * D("0.95") - ICPT, cfads(D("0.95")))

    # 5 % output shortfall
    CF95 = cfads(D("0.95"))
    SH5 = CF - CF95
    check("12.1.3 EBITDA at 95 % output", ebitda(D("0.95")), D(6933750))
    check("12.1.3 EBITDA fall on a 5 % shortfall", ebitda(D(1)) - ebitda(D("0.95")), D(566250))
    check("12.1.3 EBITDA fall as a percentage",
          ((ebitda(D(1)) - ebitda(D("0.95"))) / ebitda(D(1)) * 100).quantize(D("0.01")),
          D("7.55"))
    check("12.1.3 operating leverage at 5 %",
          (((ebitda(D(1)) - ebitda(D("0.95"))) / ebitda(D(1))) / D("0.05")).quantize(D("0.001")),
          D("1.510"))
    check("12.1.3 CFADS at 95 % output", CF95, D(5931000))
    check("12.1.3 annual CFADS shortfall at 5 %", SH5, D(453000))
    check("12.1.3 DSCR at 95 % output", (CF95 / DS).quantize(D("0.0001")), D("1.1839"))
    check("12.1.3 covenant breached by (cash)", (DS * COV - CF95).quantize(D("0.01")),
          D("80562.28"))
    check("12.1.3 output shortfall that exhausts headroom (%)",
          (HEAD / SLOPE).quantize(D("0.0001")), D("4.1108"))

    # ================= engine 2 — delay damages and the cap stack =================
    DAY_INT = DEBT * RATE / 360
    DAY_CF = CF / 360
    DAY = DAY_INT + DAY_CF
    LDR = D(20000)
    CAPD = EPC * D("0.10")
    CAPP = EPC * D("0.10")
    AGG = EPC * D("0.20")
    check("12.1.2 daily carrying cost of drawn debt (30/360)", DAY_INT, D(7000))
    check("12.1.2 daily forgone CFADS", DAY_CF.quantize(D("0.0001")), D("17733.3333"))
    check("12.1.2 daily economic cost of delay", DAY.quantize(D("0.0001")), D("24733.3333"))
    check("12.1.2 damages recover (% of daily cost)", (LDR / DAY * 100).quantize(D("0.01")),
          D("80.86"))
    check("12.1.2 revenue-only calibration recovers (%)",
          (DAY_CF / DAY * 100).quantize(D("0.01")), D("71.70"))
    check("12.1.2 delay cap 10 % of EPC price", CAPD, D(4800000))
    check("12.1.2 cap-binding day", CAPD / LDR, D(240))
    for days, econ, unc in ((180, D(4452000), D(852000)),
                            (300, D(7420000), D(2620000)),
                            (360, D(8904000), D(4104000))):
        got = DAY * days
        check(f"12.1.2 economic cost of a {days}-day delay", got, econ)
        check(f"12.1.2 uncovered residue at {days} days",
              got - min(LDR * days, CAPD), unc)
    check("12.1.1 sub-caps sum equals the aggregate cap exactly", CAPD + CAPP, AGG)
    check("12.1.2 aggregate cap 20 % of EPC price", AGG, D(9600000))

    # ================= engine 3 — performance damages on three bases =================
    # The manuscript substitutes the displayed six-decimal annuity factors, so the checks use the
    # same literals and separately assert that each is the correct rounding of af().
    AF8_25 = D("10.674776")
    check("12.1.3 AF(0.08,25) literal is the correct rounding",
          af(R8, LIFE).quantize(D("0.000001")), AF8_25)
    VALUE5 = SH5 * AF8_25
    PER_PT = SLOPE * AF8_25
    check("12.1.3 value of a 5 % permanent shortfall", VALUE5.quantize(D("0.01")),
          D("4835673.53"))
    check("12.1.3 value per output point", PER_PT.quantize(D("0.01")), D("967134.71"))
    check("12.1.3 shortfall the 10 % cap reaches (%)",
          (CAPP / PER_PT).quantize(D("0.0001")), D("4.9631"))
    SIZED = DEBT * SH5 / CF
    check("12.1.3 buy-down restoring the sized 1.2743 DSCR", SIZED.quantize(D("0.01")),
          D("2980263.16"))
    check("12.1.3 debt after the sized buy-down", (DEBT - SIZED).quantize(D("0.01")),
          D("39019736.84"))
    DS_COV = CF95 / COV
    D_COV = DS_COV * AF6_12
    check("12.1.3 debt service at the bare covenant", DS_COV, D(4942500))
    check("12.1.3 debt after the covenant-restoring prepayment", D_COV.quantize(D("0.01")),
          D("41437148.97"))
    check("12.1.3 covenant-restoring prepayment", (DEBT - D_COV).quantize(D("0.01")),
          D("562851.03"))
    check("12.1.3 spread value / bare covenant", (VALUE5 / (DEBT - D_COV)).quantize(D("0.001")),
          D("8.591"))
    check("12.1.3 spread value / sized coverage", (VALUE5 / SIZED).quantize(D("0.001")),
          D("1.623"))
    check("12.1.3 value less sized coverage", (VALUE5 - SIZED).quantize(D("0.01")),
          D("1855410.37"))

    # the application clause: 4,800,000 wholly to mandatory prepayment
    NEWDEBT = DEBT - CAPP
    NEWDS = NEWDEBT / AF6_12
    RELIEF = DS - NEWDS
    AF8_12 = D("7.536078")
    check("12.1.3 debt after applying the cap to prepayment", NEWDEBT, D(37200000))
    check("12.1.3 instalment after prepayment", NEWDS.quantize(D("0.01")), D("4437105.46"))
    check("12.1.3 annual debt-service relief", RELIEF.quantize(D("0.01")), D("572529.77"))
    check("12.1.3 DSCR after prepayment", (CF95 / NEWDS).quantize(D("0.0001")), D("1.3367"))
    check("12.1.3 distribution before the event", (CF - DS).quantize(D("0.01")),
          D("1374364.77"))
    check("12.1.3 distribution after prepayment", (CF95 - NEWDS).quantize(D("0.01")),
          D("1493894.54"))
    check("12.1.3 AF(0.08,12) literal is the correct rounding",
          af(R8, TEN).quantize(D("0.000001")), AF8_12)
    check("12.1.3 PV of 12 years of debt-service relief",
          (RELIEF * AF8_12).quantize(D("0.01")), D("4314628.99"))
    check("12.1.3 residual equity gap from the tenor mismatch",
          (VALUE5 - RELIEF * AF8_12).quantize(D("0.01")), D("521044.53"))

    # the combined stress: 300 days late and 95 % output
    EXPO = DAY * 300 + VALUE5
    check("12.1.2 combined stress exposure", EXPO.quantize(D("0.01")), D("12255673.53"))
    check("12.1.2 uncovered residue, nominal cover", (EXPO - AGG).quantize(D("0.01")),
          D("2655673.53"))
    check("12.1.2 residue as % of EPC price", ((EXPO - AGG) / EPC * 100).quantize(D("0.01")),
          D("5.53"))
    check("12.1.2 residue as % of equity", ((EXPO - AGG) / EQUITY * 100).quantize(D("0.01")),
          D("14.75"))
    check("12.1.2 residue as % of project NPV", ((EXPO - AGG) / NPVP * 100).quantize(D("0.01")),
          D("16.41"))
    check("12.1.2 performance head uncovered at the cap",
          (VALUE5 - CAPP).quantize(D("0.01")), D("35673.53"))

    # ---------- 12.1.4 the O&M liability asymmetry ----------
    check("12.1.4 30-day outage economic cost", (DAY * 30).quantize(D("0.01")), D(742000))
    check("12.1.4 uncovered against a half-fee cap", (DAY * 30 - D(600000)).quantize(D("0.01")),
          D(142000))
    check("12.1.4 60-day outage economic cost", (DAY * 60).quantize(D("0.01")), D(1484000))
    check("12.1.4 60-day uncovered against a half-fee cap",
          (DAY * 60 - D(600000)).quantize(D("0.01")), D(884000))
    check("12.1.4 60-day uncovered against a full-fee cap",
          (DAY * 60 - D(1200000)).quantize(D("0.01")), D(284000))
    check("12.1.4 O&M fee as a share of cash operating cost",
          (D(1200000) / D(4500000) * 100).quantize(D("1")), D(27))

    # ================= engine 1b — the contracted volume floor =================
    def floor_for(k):
        return (k * DS + ICPT) / SLOPE / 100

    check("12.2.2 DSCR at the negotiated 90 % floor",
          (cfads(D("0.90")) / DS).quantize(D("0.0001")), D("1.0935"))
    check("12.2.2 CFADS at 90 % output", cfads(D("0.90")), D(5478000))
    check("12.2.2 covenant floor (%)", (floor_for(COV) * 100).quantize(D("0.0001")),
          D("95.8892"))
    check("12.2.2 lock-up floor (%)", (floor_for(LOCK) * 100).quantize(D("0.0001")),
          D("93.1245"))
    check("12.2.2 DSCR 1.00 floor (%)", (floor_for(D(1)) * 100).quantize(D("0.0001")),
          D("84.8304"))
    check("12.2.2 CFADS exhausted at (%)", (ICPT / SLOPE).quantize(D("0.0001")), D("29.5364"))
    check("12.2.2 flexibility the covenant permits (points)",
          (D(100) - floor_for(COV) * 100).quantize(D("0.0001")), D("4.1108"))
    check("12.2.2 gap between negotiated and required floor (points)",
          (floor_for(COV) * 100 - D(90)).quantize(D("0.0001")), D("5.8892"))
    check("12.2.2 volume gap between covenant and lock-up floors (points)",
          (floor_for(COV) * 100 - floor_for(LOCK) * 100).quantize(D("0.01")), D("2.76"))
    check("12.2.2 coverage cost per output point", (SLOPE / DS).quantize(D("0.0001")),
          D("0.0181"))
    check("12.2.2 CFADS fall on a 10 % volume cut (%)",
          (SLOPE * 10 / CF * 100).quantize(D("0.01")), D("14.19"))
    check("12.2.2 debt that lets a 90 % floor clear the covenant",
          (cfads(D("0.90")) / COV * AF6_12).quantize(D("0.01")), D("38272247.86"))
    check("12.2.2 additional equity that would require",
          (DEBT - cfads(D("0.90")) / COV * AF6_12).quantize(D("0.01")), D("3727752.14"))

    # ================= 12.2.3 / 12.A.2 — termination against the amortisation profile =========
    bal = DEBT
    balances, interest, principal = {}, {}, {}
    for t in range(1, TEN + 1):
        i = bal * RATE
        p = DS - i
        bal = bal - p
        balances[t], interest[t], principal[t] = bal, i, p
    for t, ii, pp, cc in ((1, D("2520000.00"), D("2489635.23"), D("39510364.77")),
                          (3, D("2212281.09"), D("2797354.14"), D("34073997.28")),
                          (5, D("1866528.11"), D("3143107.12"), D("27965694.77")),
                          (8, D("1266144.36"), D("3743490.87"), D("17358915.21")),
                          (12, D("283564.26"), D("4726070.97"), D(0))):
        check(f"12.2.3 year {t} interest", interest[t].quantize(D("0.01")), ii)
        check(f"12.2.3 year {t} principal", principal[t].quantize(D("0.01")), pp)
        check(f"12.2.3 year {t} closing balance", balances[t].quantize(D("0.01")), cc,
              tol=D("0.10"))
    check("12.2.3 85 % formula gap at end of year 1",
          (balances[1] * D("0.15")).quantize(D("0.01")), D("5926554.72"))
    check("12.2.3 85 % formula gap at end of year 5",
          (balances[5] * D("0.15")).quantize(D("0.01")), D("4194854.22"))
    check("12.2.3 five years of nominal distributions",
          ((CF - DS) * 5).quantize(D("0.01")), D("6871823.85"))
    check("12.2.3 unreturned equity at end of year 5",
          (EQUITY - (CF - DS) * 5).quantize(D("0.01")), D("11128176.15"))

    # ================= 12.2.4 — the interface hole counterfactual =================
    check("12.2.4 60-day interface hole", (DAY * 60).quantize(D("0.01")), D(1484000))
    check("12.2.4 interface hole as % of EPC price",
          (DAY * 60 / EPC * 100).quantize(D("0.001")), D("3.092"))
    check("12.2.4 split packages sum to the EPC price", D(8000000) + D(40000000), EPC)

    # ================= engine 4 — guarantee sufficiency =================
    BOND = EPC * D("0.10")
    PQ = D("0.70")
    RACOV = BOND + (AGG - BOND) * PQ
    check("12.3.2 on-demand bond face", BOND, D(4800000))
    check("12.3.2 parent guarantee increment", AGG - BOND, D(4800000))
    check("12.3.2 risk-adjusted value of the guarantee increment", (AGG - BOND) * PQ, D(3360000))
    check("12.3.2 risk-adjusted cover", RACOV, D(8160000))
    check("12.3.2 credit haircut", AGG - RACOV, D(1440000))
    check("12.3.2 credit haircut (% of nominal cap)",
          ((AGG - RACOV) / AGG * 100).quantize(D("0.01")), D("15.00"))
    check("12.3.2 uncovered residue, risk-adjusted", (EXPO - RACOV).quantize(D("0.01")),
          D("4095673.53"))
    check("12.3.2 risk-adjusted residue as % of equity",
          ((EXPO - RACOV) / EQUITY * 100).quantize(D("0.01")), D("22.75"))
    check("12.3.2 equivalent parent-guarantee face for the bond",
          (BOND / PQ).quantize(D("0.01")), D("6857142.86"))
    check("12.3.2 equivalence multiple", (1 / PQ).quantize(D("0.0001")), D("1.4286"))
    check("12.3.2 three-year bond cost at 1.2 % p.a.", BOND * D("0.012") * 3, D(172800))
    check("12.3.2 bond cost as % of face", (BOND * D("0.012") * 3 / BOND * 100), D("3.60"))

    # case-study remediation: bond to 15 %, aggregate to 30 %
    check("CS-A aggregate cap at 30 % of EPC price", EPC * D("0.30"), D(14400000))
    check("CS-A room left for a defects head", EPC * D("0.30") - AGG, D(4800000))
    check("CS-A bond at 15 % of EPC price", EPC * D("0.15"), D(7200000))
    check("CS-A risk-adjusted uplift from the larger bond",
          (EPC * D("0.15") - BOND) * (1 - PQ), D(720000))
    check("CS-A cost of the incremental bonding",
          (EPC * D("0.15") - BOND) * D("0.012") * 3, D(86400))

    # ================= engine 5 — claim valuation =================
    PROL = D(12500) * 90
    QUANTUM = PROL + D(480000) + D(265000)
    LD_RELIEF = LDR * 90
    OWN_ECON = DAY * 90
    check("12.4.3 prolongation over 90 days", PROL, D(1125000))
    check("12.4.3 total quantum claimed", QUANTUM, D(1870000))
    check("12.4.3 value of the extension of time to the contractor", LD_RELIEF, D(1800000))
    check("12.4.3 project company's own economic cost of 90 days", OWN_ECON.quantize(D("0.01")),
          D(2226000))
    check("12.4.3 total exposure of the event", (QUANTUM + OWN_ECON).quantize(D("0.01")),
          D(4096000))
    GAP_MONEY = QUANTUM - D(1050000)
    GAP_LD = LDR * (90 - 55)
    DSP = GAP_MONEY + GAP_LD
    check("12.4.3 quantum gap", GAP_MONEY, D(820000))
    check("12.4.3 time-impact gap", GAP_LD, D(700000))
    check("12.4.3 disputed sum", DSP, D(1520000))
    PF, PS = D("0.35"), D("0.25")
    EMV = PF * DSP + PS * DSP / 2
    check("12.4.3 expected award", EMV, D(722000))
    DF_A = (1 + R8) ** (D(26) / 12)
    DF_C = (1 + R8) ** (D(13) / 12)
    check("12.4.3 discount factor at 26 months", DF_A.quantize(D("0.000001")), D("1.181458"))
    check("12.4.3 discount factor at 13 months", DF_C.quantize(D("0.000001")), D("1.086949"))
    PV_A = EMV / DF_A
    PV_C = (D(620000) + D(180000)) / DF_C
    check("12.4.3 PV of the expected award", PV_A.quantize(D("0.01")), D("611109.54"))
    check("12.4.3 PV of own costs", PV_C.quantize(D("0.01")), D("736005.26"))
    FIGHT = PV_A + PV_C
    check("12.4.3 PV of fighting", FIGHT.quantize(D("0.01")), D("1347114.80"))
    NEG = D(60000)
    check("12.4.3 cost of settling at the midpoint", DSP / 2 + NEG, D(820000))
    check("12.4.3 saving from settling at the midpoint", (FIGHT - (DSP / 2 + NEG)).quantize(
        D("0.01")), D("527114.80"))
    check("12.4.3 settlement ceiling", (FIGHT - NEG).quantize(D("0.01")), D("1287114.80"))
    check("12.4.3 ceiling as % of the disputed sum",
          ((FIGHT - NEG) / DSP * 100).quantize(D("0.01")), D("84.68"))
    K = (PF + PS / 2) / DF_A
    DSTAR = (PV_C - NEG) / (D("0.5") - K)
    check("12.4.3 breakeven disputed sum", DSTAR.quantize(D("0.01")), D("6901234.43"))
    check("12.4.3 breakeven as a multiple of the disputed sum",
          (DSTAR / DSP).quantize(D("0.01")), D("4.54"))
    check("12.4.3 breakeven as % of the EPC price",
          (DSTAR / EPC * 100).quantize(D("0.01")), D("14.38"))
    check("12.4.3 breakeven identity: fighting equals settling at D*",
          K * DSTAR + PV_C, DSTAR / 2 + NEG)
    check("12.4.4 dispute budget as a multiple of annual headroom",
          (D(800000) / HEAD).quantize(D("0.001")), D("2.148"))

    # ================= Case study B — the interface nobody owned (data centre) ============
    SHELL, FITOUT = D(62000000), D(48000000)
    TOT = SHELL + FITOUT
    DC_DEBT, DC_RATE, DC_CF = D(77000000), D("0.065"), D(14600000)
    DC_DAY = DC_DEBT * DC_RATE / 360 + DC_CF / 360
    HOLE = DC_DAY * 74
    PROLB = (D(9800) + D(7400)) * 74
    check("CS-B combined contract price", TOT, D(110000000))
    check("CS-B daily carrying cost", (DC_DEBT * DC_RATE / 360).quantize(D("0.01")),
          D("13902.78"))
    check("CS-B daily forgone CFADS", (DC_CF / 360).quantize(D("0.0001")), D("40555.5556"))
    check("CS-B daily economic cost of delay", DC_DAY.quantize(D("0.0001")), D("54458.3333"))
    check("CS-B 74-day interface hole", HOLE.quantize(D("0.01")), D("4029916.67"))
    check("CS-B prolongation claimed by both contractors", PROLB, D(1272800))
    check("CS-B total cost to the project company", (HOLE + PROLB).quantize(D("0.01")),
          D("5302716.67"))
    check("CS-B cost as % of the combined price",
          ((HOLE + PROLB) / TOT * 100).quantize(D("0.001")), D("4.821"))
    check("CS-B declined wrap premium at 3.5 %", TOT * D("0.035"), D(3850000))
    check("CS-B breakeven probability for the wrap (%)",
          (TOT * D("0.035") / (HOLE + PROLB) * 100).quantize(D("0.01")), D("72.60"))
    check("CS-B breakeven probability for the interface regime (%)",
          (D(640000) / (HOLE + PROLB) * 100).quantize(D("0.01")), D("12.07"))

    # ================= Case study A — the cap that covered 4.96 per cent =================
    check("CS-A cap-binding day", CAPD / LDR, D(240))
    check("CS-A shortfall the performance cap reaches (%)",
          (CAPP / PER_PT).quantize(D("0.0001")), D("4.9631"))
    check("CS-A margin between cap reach and covenant failure (points)",
          (CAPP / PER_PT - HEAD / SLOPE).quantize(D("0.01")), D("0.85"))

    # ================= Exercises =================
    # Exercise 12.1 — delay damages on a larger project
    E_DEBT, E_RATE, E_CF, E_EPC = D(96000000), D("0.072"), D(15300000), D(120000000)
    E_LDR, E_CAP = D(45000), E_EPC * D("0.08")
    E_DI, E_DC = E_DEBT * E_RATE / 360, E_CF / 360
    E_DAY = E_DI + E_DC
    check("Ex 12.1 daily interest", E_DI, D(19200))
    check("Ex 12.1 daily forgone CFADS", E_DC, D(42500))
    check("Ex 12.1 daily economic cost", E_DAY, D(61700))
    check("Ex 12.1 damages recovery (%)", (E_LDR / E_DAY * 100).quantize(D("0.01")), D("72.93"))
    check("Ex 12.1 cap 8 % of price", E_CAP, D(9600000))
    check("Ex 12.1 cap-binding day", (E_CAP / E_LDR).quantize(D("0.01")), D("213.33"))
    check("Ex 12.1 economic cost of 300 days", E_DAY * 300, D(18510000))
    check("Ex 12.1 uncovered at 300 days", E_DAY * 300 - min(E_LDR * 300, E_CAP), D(8910000))
    check("Ex 12.1 uncovered as % of price",
          ((E_DAY * 300 - E_CAP) / E_EPC * 100).quantize(D("0.001")), D("7.425"))
    check("Ex 12.1 revenue-only calibration recovery (%)",
          (E_DC / E_DAY * 100).quantize(D("0.01")), D("68.88"))

    # Exercise 12.2 — performance cap reach
    AF8_20 = D("9.818147")
    check("Ex 12.2 AF(0.08,20) literal is the correct rounding",
          af(R8, 20).quantize(D("0.000001")), AF8_20)
    E_PER_PT = D(210000) * AF8_20
    check("Ex 12.2 value per output point", E_PER_PT.quantize(D("0.01")), D("2061810.87"))
    check("Ex 12.2 shortfall the cap reaches (%)",
          (E_CAP / E_PER_PT).quantize(D("0.0001")), D("4.6561"))

    # Exercise 12.3 — the volume floor at a 1.25 covenant
    AF72_15 = D("8.993967")
    check("Ex 12.3 AF(0.072,15) literal is the correct rounding",
          af(D("0.072"), 15).quantize(D("0.000001")), AF72_15)
    E_INST = E_DEBT / AF72_15
    check("Ex 12.3 instalment", E_INST.quantize(D("0.01")), D("10673821.69"))
    EA, EB = D(16800000), D(3072000)
    check("Ex 12.3 CFADS at full contracted volume", EA - EB, D(13728000))
    check("Ex 12.3 DSCR at full volume", ((EA - EB) / E_INST).quantize(D("0.0001")),
          D("1.2861"))
    check("Ex 12.3 required CFADS at 1.25", (D("1.25") * E_INST).quantize(D("0.01")),
          D("13342277.11"))
    check("Ex 12.3 volume floor (%)",
          ((D("1.25") * E_INST + EB) / EA * 100).quantize(D("0.0001")), D("97.7040"))
    check("Ex 12.3 DSCR at a 90 % floor",
          ((EA * D("0.90") - EB) / E_INST).quantize(D("0.0001")), D("1.1287"))

    # Exercise 12.4 — guarantee sufficiency
    X_EXPO, X_BOND, X_PCG, X_PQ = D(14200000), D(6000000), D(12000000), D("0.65")
    X_RA = X_BOND + (X_PCG - X_BOND) * X_PQ
    check("Ex 12.4 risk-adjusted cover", X_RA, D(9900000))
    check("Ex 12.4 nominal uncovered", X_EXPO - X_PCG, D(2200000))
    check("Ex 12.4 risk-adjusted uncovered", X_EXPO - X_RA, D(4300000))
    check("Ex 12.4 equivalent guarantee face for the bond",
          (X_BOND / X_PQ).quantize(D("0.01")), D("9230769.23"))
    check("Ex 12.4 equivalence multiple", (1 / X_PQ).quantize(D("0.0001")), D("1.5385"))

    # Exercise 12.5 — claim valuation
    Y_D, Y_PF, Y_PS = D(2400000), D("0.30"), D("0.30")
    Y_EMV = Y_PF * Y_D + Y_PS * Y_D / 2
    Y_DFA = (1 + R8) ** D("2.5")
    Y_DFC = (1 + R8) ** (D(14) / 12)
    check("Ex 12.5 expected award", Y_EMV, D(1080000))
    check("Ex 12.5 discount factor at 30 months", Y_DFA.quantize(D("0.000001")), D("1.212158"))
    check("Ex 12.5 discount factor at 14 months", Y_DFC.quantize(D("0.000001")), D("1.093942"))
    Y_PVA, Y_PVC = Y_EMV / Y_DFA, D(700000) / Y_DFC
    check("Ex 12.5 PV of the expected award", Y_PVA.quantize(D("0.01")), D("890972.64"))
    check("Ex 12.5 PV of own costs", Y_PVC.quantize(D("0.01")), D("639887.55"))
    check("Ex 12.5 PV of fighting", (Y_PVA + Y_PVC).quantize(D("0.01")), D("1530860.19"))
    check("Ex 12.5 cost of settling at the midpoint", Y_D / 2 + D(50000), D(1250000))
    check("Ex 12.5 saving from settling",
          (Y_PVA + Y_PVC - (Y_D / 2 + D(50000))).quantize(D("0.01")), D("280860.19"))
    check("Ex 12.5 settlement ceiling", (Y_PVA + Y_PVC - D(50000)).quantize(D("0.01")),
          D("1480860.19"))
    check("Ex 12.5 ceiling as % of the disputed sum",
          ((Y_PVA + Y_PVC - D(50000)) / Y_D * 100).quantize(D("0.01")), D("61.70"))
    Y_K = (Y_PF + Y_PS / 2) / Y_DFA
    check("Ex 12.5 k coefficient", Y_K.quantize(D("0.000001")), D("0.371239"))
    check("Ex 12.5 breakeven disputed sum",
          ((Y_PVC - D(50000)) / (D("0.5") - Y_K)).quantize(D("0.01")), D("4581245.18"))
