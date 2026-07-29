"""PFL-AI Domain 11 — Risk identification and allocation: golden checks.

Every number printed as a result in `domain-11-risk-identification-allocation.md` is recomputed
here from first principles. Master-thread values are read from ctx, never re-typed.

The domain has four engines:

  1. Allocation pricing (KA 11.1) — net value of transfer = own EMV - transferee EMV * (1+loading);
     breakeven loading = own EMV / transferee EMV - 1.
  2. Escalation and pass-through (KA 11.2) — residual per 1 % input move = cost * 0.01 * (1 - phi);
     CFADS(t) = revenue(t) - CPI cost(t) - power(t) - 1,116,000 (cash tax 516,000 + working
     capital 600,000 held flat, the manuscript's stated simplification).
  3. Rate and currency (KA 11.3) — principal is fixed by the amortisation schedule and only
     interest floats: DS = principal + debt * blended rate.  Currency: CFADS in USD =
     local numerator / x - USD costs, where x is host-currency units per USD.
  4. Register aggregation (KA 11.4) — the PML-AI D8 method: mean = sum EMV,
     variance = sum p(1-p)impact^2 independent, (1-rho)*sum sigma_i^2 + rho*(sum sigma_i)^2
     correlated, P80 = mean + 0.8416 sigma; then annualised over the loan life at AF(0.06, 12).
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DEBT = ctx["KESTREL_DEBT"]
    EQUITY = ctx["KESTREL_EQUITY"]
    CAPEX = ctx["KESTREL_CAPEX"]
    DS = ctx["KESTREL_INSTALMENT"]
    CF = ctx["KESTREL_CFADS"]
    INT1 = ctx["KESTREL_INTEREST_Y1"]
    RATE = ctx["KESTREL_RATE"]
    # The registry's published AF(0.06, 12) literal is the value the manuscript divides by,
    # so the checks use it and separately assert that it is the correct rounding of af().
    AF6_12 = D("8.383844")
    PRIN1 = DS - INT1
    COV, LOCK, Z80 = D("1.20"), D("1.15"), D("0.8416")
    HEAD = CF - DS * COV

    # ---------- master-thread anchors this domain reads ----------
    check("D11 AF(0.06,12) registry literal is the correct rounding",
          af(RATE, ctx["KESTREL_TENOR"]).quantize(D("0.000001")), AF6_12)
    check("D11 year-one principal", PRIN1, D("2489635.23"))
    check("D11 covenant cash trigger", DS * COV, D("6011562.276"))
    check("D11 lock-up cash trigger", (DS * LOCK).quantize(D("0.01")), D("5761080.51"))
    check("D11 annual headroom", HEAD.quantize(D("0.01")), D("372437.72"))
    check("D11 DSCR at close", (CF / DS).quantize(D("0.0001")), D("1.2743"))

    # ================= KA 11.1 — allocation pricing =================
    LOAD = D("1.40")
    # (id, own p, own impact, bidder p, bidder impact)
    ITEMS = [
        ("A1", D("0.30"), D(6000000), D("0.12"), D(4000000)),
        ("A2", D("0.35"), D(5200000), D("0.20"), D(3400000)),
        ("A3", D("0.25"), D(3200000), D("0.15"), D(2400000)),
        ("A4", D("0.40"), D(2400000), D("0.45"), D(2900000)),
        ("A5", D("0.30"), D(1800000), D("0.31"), D(2000000)),
        ("A6", D("0.35"), D(1400000), D("0.35"), D(1500000)),
        ("A7", D("0.50"), D(900000), D("0.50"), D(900000)),
        ("A8", D("0.20"), D(2000000), D("0.20"), D(2000000)),
    ]
    EXPECT = {                        # own EMV, bidder EMV, loaded premium, net value
        "A1": (1800000, 480000, 672000, 1128000),
        "A2": (1820000, 680000, 952000, 868000),
        "A3": (800000, 360000, 504000, 296000),
        "A4": (960000, 1305000, 1827000, -867000),
        "A5": (540000, 620000, 868000, -328000),
        "A6": (490000, 525000, 735000, -245000),
        "A7": (450000, 450000, 630000, -180000),
        "A8": (400000, 400000, 560000, -160000),
    }
    for idd, pa, ia, pb, ib in ITEMS:
        own, con, prem, net = EXPECT[idd]
        check(f"WE 11.1.3 {idd} own EMV", pa * ia, own)
        check(f"WE 11.1.3 {idd} bidder EMV", pb * ib, con)
        check(f"WE 11.1.3 {idd} loaded premium", pb * ib * LOAD, prem)
        check(f"WE 11.1.3 {idd} net value of transfer", pa * ia - pb * ib * LOAD, net)

    own_T = sum(p * i for _, p, i, _, _ in ITEMS[:3])
    con_T = sum(p * i for _, _, _, p, i in ITEMS[:3])
    own_R = sum(p * i for _, p, i, _, _ in ITEMS[3:])
    con_R = sum(p * i for _, _, _, p, i in ITEMS[3:])
    prem_T, prem_R = con_T * LOAD, con_R * LOAD
    OPP = D(-150000)
    check("WE 11.1.3 A1-A3 own EMV", own_T, 4420000)
    check("WE 11.1.3 A1-A3 bidder EMV", con_T, 1520000)
    check("WE 11.1.3 A1-A3 premium", prem_T, 2128000)
    check("WE 11.1.3 A1-A3 value created", own_T - prem_T, 2292000)
    check("WE 11.1.3 A4-A8 own EMV", own_R, 2840000)
    check("WE 11.1.3 A4-A8 bidder EMV", con_R, 3300000)
    check("WE 11.1.3 A4-A8 premium", prem_R, 4620000)
    check("WE 11.1.3 A4-A8 value destroyed", own_R - prem_R, -1780000)
    check("WE 11.1.3 A4-A8 destroyed at zero loading", own_R - con_R, -460000)
    check("WE 11.1.3 breakeven loading A1-A3 %",
          ((own_T / con_T - 1) * 100).quantize(D("0.01")), D("190.79"))
    check("WE 11.1.3 breakeven loading A4-A8 %",
          ((own_R / con_R - 1) * 100).quantize(D("0.01")), D("-13.94"))
    check("WE 11.1.3 A4-A8 acceptable as % of bidder expected cost",
          (own_R / con_R * 100).quantize(D("0.01")), D("86.06"))
    check("WE 11.1.3 retained residue = D8 register", own_R + OPP, 2690000)
    check("WE 11.1.3 gross register wholly retained", own_T + own_R + OPP, 7110000)
    check("WE 11.1.3 expected cost after allocation", prem_T + own_R + OPP, 4818000)
    check("WE 11.1.3 cost reduction per premium dollar",
          (own_T / prem_T).quantize(D("0.0001")), D("2.0771"))
    check("WE 11.1.3 un-risked EPC scope", D(48000000) - prem_T, 45872000)
    check("CS-A full-wrap EPC price", D(48000000) + prem_R, 52620000)

    # Case study A — the full wrap, financed
    capex_w = CAPEX + prem_R
    debt_w = capex_w * D("0.7")
    inst_w = debt_w / AF6_12
    check("CS-A full-wrap capex", capex_w, 64620000)
    check("CS-A full-wrap debt at 70/30", debt_w, 45234000)
    check("CS-A full-wrap instalment", inst_w.quantize(D("0.01")), D("5395377.11"))
    check("CS-A full-wrap DSCR", (CF / inst_w).quantize(D("0.0001")), D("1.1832"))
    check("CS-A full-wrap DSCR given up",
          (CF / DS - CF / inst_w).quantize(D("0.0001")), D("0.0911"))
    check("CS-A full-wrap margin above lock-up",
          (CF / inst_w - LOCK).quantize(D("0.0001")), D("0.0332"))
    check("CS-A equity-funded equity", EQUITY + prem_R, 22620000)
    check("CS-A equity-funded D/E", (DEBT / (EQUITY + prem_R)).quantize(D("0.0001")), D("1.8568"))
    check("CS-A equity uplift %", (prem_R / EQUITY * 100).quantize(D("0.01")), D("25.67"))

    # ================= KA 11.2 — pass-through and indexation =================
    POWER, OTHER, OPEX = D(1800000), D(2700000), D(4500000)
    check("11.2.2 opex decomposition", POWER + OTHER, OPEX)
    for phi, per1, tol in ((D(0), D(18000), D("20.6910")), (D("0.70"), D(5400), D("68.9699"))):
        check(f"WE 11.2.2 residual per 1 % at phi={phi}", POWER * D("0.01") * (1 - phi), per1)
        check(f"WE 11.2.2 tolerance % at phi={phi}", (HEAD / per1).quantize(D("0.0001")), tol)
    check("WE 11.2.2 multiplier", (D(1) / D("0.30")).quantize(D("0.0001")), D("3.3333"))
    check("WE 11.2.2 25 % rise residual at phi=0.70", POWER * D("0.25") * D("0.30"), 135000)
    check("WE 11.2.2 25 % rise CFADS at phi=0.70", CF - D(135000), 6249000)
    check("WE 11.2.2 25 % rise DSCR at phi=0.70",
          ((CF - D(135000)) / DS).quantize(D("0.0001")), D("1.2474"))
    check("WE 11.2.2 25 % rise residual unprotected", POWER * D("0.25"), 450000)
    check("WE 11.2.2 25 % rise CFADS unprotected", CF - D(450000), 5934000)
    check("WE 11.2.2 25 % rise DSCR unprotected",
          ((CF - D(450000)) / DS).quantize(D("0.0001")), D("1.1845"))

    FIXED = D(1116000)                # cash tax 516,000 + working-capital movement 600,000
    REV0, CPI, REVIDX, POWG = D(12000000), D("0.025"), D("0.80"), D("0.08")

    def cfads_year(t, g=POWG):
        rev = REV0 * (1 + REVIDX * CPI) ** (t - 1)
        oth = OTHER * (1 + CPI) ** (t - 1)
        pow_ = POWER * (1 + g) ** (t - 1)
        return rev, oth, pow_, rev - oth - pow_ - FIXED

    check("11.2.3 revenue-weighted escalation %", REVIDX * CPI * 100, 2)
    check("11.2.3 cost-weighted escalation %",
          ((OTHER * CPI + POWER * POWG) / OPEX * 100).quantize(D("0.01")), D("4.70"))
    TABLE = {                          # year: (revenue, CPI cost, power, CFADS, DSCR)
        1: (12000000, 2700000, 1800000, 6384000, "1.2743"),
        3: ("12484800", "2836687.50", 2099520, "6432592.50", "1.2840"),
        5: ("12989185.92", "2980294.80", "2448880.13", "6444010.99", "1.2863"),
        8: ("13784228.01", "3209451.53", "3084883.68", "6373892.79", "1.2723"),
        10: ("14341110.82", "3371930.02", "3598208.33", "6254972.48", "1.2486"),
        12: ("14920491.70", "3542633.98", "4196950.19", "6064907.53", "1.2106"),
        15: ("15833745.16", "3815029.32", "5286948.52", "5615767.32", "1.1210"),
    }
    for t, (rv, ot, pw, cf, ds) in TABLE.items():
        rev, oth, pow_, cfa = cfads_year(t)
        check(f"WE 11.2.3 yr{t} revenue", rev.quantize(D("0.01")), D(str(rv)))
        check(f"WE 11.2.3 yr{t} CPI-linked cost", oth.quantize(D("0.01")), D(str(ot)))
        check(f"WE 11.2.3 yr{t} power", pow_.quantize(D("0.01")), D(str(pw)))
        check(f"WE 11.2.3 yr{t} CFADS", cfa.quantize(D("0.01")), D(str(cf)))
        check(f"WE 11.2.3 yr{t} DSCR", (cfa / DS).quantize(D("0.0001")), D(ds))
    check("WE 11.2.3 no breach inside the loan life",
          1 if all(cfads_year(t)[3] > DS * COV for t in range(1, 13)) else 0, 1)
    check("WE 11.2.3 first breach year 13", cfads_year(13)[3].quantize(D("0.01")), D("5938995.50"))
    check("WE 11.2.3 yr13 DSCR", (cfads_year(13)[3] / DS).quantize(D("0.0001")), D("1.1855"))
    head12 = cfads_year(12)[3] - DS * COV
    check("WE 11.2.3 yr12 headroom", head12.quantize(D("0.01")), D("53345.25"))
    check("WE 11.2.3 headroom consumed %",
          ((HEAD - head12) / HEAD * 100).quantize(D("0.01")), D("85.68"))
    lo, hi = D(0), D("0.30")           # breakeven power escalation for a yr-12 covenant breach
    for _ in range(200):
        mid = (lo + hi) / 2
        if cfads_year(12, mid)[3] > DS * COV:
            lo = mid
        else:
            hi = mid
    check("WE 11.2.3 breakeven power escalation %",
          (lo * 100).quantize(D("0.0001")), D("8.1241"))
    p12, c12 = POWER * (1 + POWG) ** 11, POWER * (1 + CPI) ** 11
    check("WE 11.2.3 yr12 power at 8 %", p12.quantize(D("0.01")), D("4196950.19"))
    check("WE 11.2.3 yr12 power at CPI", c12.quantize(D("0.01")), D("2361755.98"))
    check("WE 11.2.3 escalation wedge", (p12 - c12).quantize(D("0.01")), D("1835194.21"))
    check("WE 11.2.3 wedge / headroom", ((p12 - c12) / HEAD).quantize(D("0.0001")), D("4.9275"))

    # ================= KA 11.3 — interest rate =================
    SWAP = D("0.062")                  # swap reference 4.20 % + 200 bp margin

    def dscr_rate(bp, h=D(0)):
        rate = SWAP * h + (RATE + D(bp) / 10000) * (1 - h)
        return rate, PRIN1 + DEBT * rate, CF / (PRIN1 + DEBT * rate)

    RATETAB = {                        # bp: (all-in %, interest, DS, DSCR h0, DSCR h75, h100)
        -200: ("4.00", 1680000, "4169635.23", "1.5311", "1.3129", "1.2533"),
        -100: ("5.00", 2100000, "4589635.23", "1.3910", "1.2851", "1.2533"),
        0: ("6.00", 2520000, "5009635.23", "1.2743", "1.2585", "1.2533"),
        50: ("6.50", 2730000, "5219635.23", "1.2231", "1.2456", "1.2533"),
        100: ("7.00", 2940000, "5429635.23", "1.1758", "1.2330", "1.2533"),
        200: ("8.00", 3360000, "5849635.23", "1.0914", "1.2085", "1.2533"),
    }
    for bp, (pct, itr, ds, d0, d75, d100) in RATETAB.items():
        rate, dsv, dsc = dscr_rate(bp)
        check(f"WE 11.3.1 {bp:+d}bp all-in %", (rate * 100).quantize(D("0.01")), D(pct))
        check(f"WE 11.3.1 {bp:+d}bp interest", DEBT * rate, itr)
        check(f"WE 11.3.1 {bp:+d}bp debt service", dsv.quantize(D("0.01")), D(ds))
        check(f"WE 11.3.1 {bp:+d}bp DSCR unhedged", dsc.quantize(D("0.0001")), D(d0))
        check(f"WE 11.3.1 {bp:+d}bp DSCR h=75 %",
              dscr_rate(bp, D("0.75"))[2].quantize(D("0.0001")), D(d75))
        check(f"WE 11.3.1 {bp:+d}bp DSCR h=100 %",
              dscr_rate(bp, D(1))[2].quantize(D("0.0001")), D(d100))
    for tgt, ds_x, int_x, rate_x, bp_x in (
            (COV, "5320000.00", "2830364.77", "6.7390", "73.9"),
            (LOCK, "5551304.35", "3061669.12", "7.2897", "129.0"),
            (D(1), "6384000.00", "3894364.77", "9.2723", "327.2")):
        ds_max = CF / tgt
        int_max = ds_max - PRIN1
        r_max = int_max / DEBT
        check(f"WE 11.3.1 max DS at {tgt}", ds_max.quantize(D("0.01")), D(ds_x))
        check(f"WE 11.3.1 max interest at {tgt}", int_max.quantize(D("0.01")), D(int_x))
        check(f"WE 11.3.1 breakeven all-in % at {tgt}",
              (r_max * 100).quantize(D("0.0001")), D(rate_x))
        check(f"WE 11.3.1 breakeven bp at {tgt}",
              ((r_max - RATE) * 10000).quantize(D("0.1")), D(bp_x))
    check("WE 11.3.1 swap cost year one", DEBT * (SWAP - RATE), 84000)
    check("WE 11.3.1 75 % hedge cost year one", DEBT * (SWAP - RATE) * D("0.75"), 63000)
    check("WE 11.3.1 floating left at h=75 %", DEBT * D("0.25"), 10500000)
    spread = dscr_rate(-200)[2] - dscr_rate(200)[2]
    given_up = CF / DS - dscr_rate(0, D(1))[2]
    check("WE 11.3.1 unhedged DSCR range", spread.quantize(D("0.0001")), D("0.4397"))
    check("WE 11.3.1 DSCR given up for a full hedge",
          given_up.quantize(D("0.0001")), D("0.0210"))
    check("WE 11.3.1 range removed per unit given up",
          (spread / given_up).quantize(D("0.01")), D("20.92"))
    r_cov = (CF / COV - PRIN1) / DEBT
    check("WE 11.3.1 minimum hedge ratio %",
          ((D("0.08") - r_cov) / (D("0.08") - SWAP) * 100).quantize(D("0.0001")), D("70.0576"))

    # ================= KA 11.3 — currency =================
    X0 = D(4)                          # host-currency units per USD at close
    HC_REV, HC_OPEX, HC_TAX, HC_WC = D(48000000), D(12600000), D(2064000), D(2400000)
    USD_OPEX = D(1350000)
    NUM = HC_REV - HC_OPEX - HC_TAX - HC_WC
    check("WE 11.3.2 local currency revenue", D(12000000) * X0, HC_REV)
    check("WE 11.3.2 local operating cost", (OPEX - USD_OPEX) * X0, HC_OPEX)
    check("WE 11.3.2 local numerator", NUM, 30936000)
    check("WE 11.3.2 decomposition reproduces the master thread", NUM / X0 - USD_OPEX, CF)
    # The three threshold rows are computed at the EXACT breakeven x, then displayed to 6 dp.
    x_cov = NUM / (DS * COV + USD_OPEX)
    x_lock = NUM / (DS * LOCK + USD_OPEX)
    x_pay = NUM / (DS + USD_OPEX)
    CCYTAB = {                          # x: (devaluation %, CFADS, DSCR)
        x_cov: ("5.06", "6011562.28", "1.2000"),
        x_lock: ("8.76", "5761080.51", "1.1500"),
        D("4.4"): ("10.00", "5680909.09", "1.1340"),
        x_pay: ("21.61", "5009635.23", "1.0000"),
        D(5): ("25.00", "4837200.00", "0.9656"),
    }
    for x, (dev, cfv, dsc) in CCYTAB.items():
        xs = str(x.quantize(D("0.000001")))
        c = NUM / x - USD_OPEX
        check(f"WE 11.3.2 x={xs} devaluation %",
              ((x / X0 - 1) * 100).quantize(D("0.01")), D(dev))
        check(f"WE 11.3.2 x={xs} CFADS", c.quantize(D("0.01")), D(cfv))
        check(f"WE 11.3.2 x={xs} DSCR", (c / DS).quantize(D("0.0001")), D(dsc))
    for tgt, x_x, dev_x in ((COV, "4.202369", "5.0592"),
                            (LOCK, "4.350394", "8.7598"),
                            (D(1), "4.864430", "21.6107")):
        x = NUM / (DS * tgt + USD_OPEX)
        check(f"WE 11.3.2 breakeven x at {tgt}", x.quantize(D("0.000001")), D(x_x))
        check(f"WE 11.3.2 breakeven devaluation % at {tgt}",
              ((x / X0 - 1) * 100).quantize(D("0.0001")), D(dev_x))
    usd_out = DS + USD_OPEX
    check("WE 11.3.2 USD outflow", usd_out.quantize(D("0.01")), D("6359635.23"))
    check("WE 11.3.2 debt-service-matching indexed share %",
          (usd_out / D(12000000) * 100).quantize(D("0.0001")), D("52.9970"))
    HC_LOCAL = HC_OPEX + HC_TAX + HC_WC

    def cfads_idx(s, x):
        return (D(12000000) * s + D(12000000) * (1 - s) * X0 / x
                - USD_OPEX - HC_LOCAL / x)

    check("WE 11.3.2 indexed model reproduces base", cfads_idx(D(0), X0), CF)
    A = D(12000000) - D(12000000) * X0 / D(5)
    B = D(12000000) * X0 / D(5) - USD_OPEX - HC_LOCAL / D(5)
    check("WE 11.3.2 minimum indexed share for 1.20 at 25 % %",
          ((DS * COV - B) / A * 100).quantize(D("0.0001")), D("48.9318"))
    s_match = D("0.52997")
    check("WE 11.3.2 CFADS at matching share, 25 % devaluation",
          cfads_idx(s_match, D(5)).quantize(D("0.01")), D("6109128.00"))
    check("WE 11.3.2 DSCR at matching share, 25 % devaluation",
          (cfads_idx(s_match, D(5)) / DS).quantize(D("0.0001")), D("1.2195"))
    check("WE 11.3.2 CFADS still lost at matching share",
          (CF - cfads_idx(s_match, D(5))).quantize(D("0.01")), D("274872.00"))
    lo, hi = D(1), D(4)
    for _ in range(300):
        mid = (lo + hi) / 2
        if cfads_idx(s_match, X0 * mid) > DS * COV:
            lo = mid
        else:
            hi = mid
    check("WE 11.3.2 breakeven devaluation at matching share %",
          ((lo - 1) * 100).quantize(D("0.01")), D("37.17"))

    # ================= KA 11.3 — counterparty and force majeure =================
    TRANSFERRED, LGD = D(4420000), D("0.70")
    el_u = D("0.015") * LGD * TRANSFERRED
    el_c = D("0.12") * LGD * TRANSFERRED
    check("11.3.3 expected credit loss, unconditional PD", el_u, 46410)
    check("11.3.3 unconditional EL as % of headroom",
          (el_u / HEAD * 100).quantize(D("0.01")), D("12.46"))
    check("11.3.3 expected credit loss, conditional PD", el_c, 371280)
    check("11.3.3 conditional EL as % of headroom",
          (el_c / HEAD * 100).quantize(D("0.01")), D("99.69"))
    day_cf = CF / 360
    check("WE 11.3.4 daily CFADS", day_cf.quantize(D("0.01")), D("17733.33"))
    check("WE 11.3.4 maximum survivable outage days",
          (HEAD / day_cf).quantize(D("0.0001")), D("21.0021"))
    for days, loss, cfv, dsc in ((21, 372400, 6011600, "1.2000"),
                                 (30, 532000, 5852000, "1.1681"),
                                 (60, 1064000, 5320000, "1.0620")):
        check(f"WE 11.3.4 {days}-day uninsured loss", D(days) * day_cf, loss)
        check(f"WE 11.3.4 {days}-day CFADS", CF - D(days) * day_cf, cfv)
        check(f"WE 11.3.4 {days}-day DSCR",
              ((CF - D(days) * day_cf) / DS).quantize(D("0.0001")), D(dsc))
    check("WE 11.3.4 90-day outage capped at the 60-day waiting period",
          CF - D(min(90, 60)) * day_cf, 5320000)

    # ================= KA 11.4 — the operating register =================
    REG = [("O1", D("0.15"), D(3200000)), ("O2", D("0.10"), D(1500000)),
           ("O3", D("0.20"), D(2600000)), ("O4", D("0.12"), D(4500000)),
           ("O5", D("0.25"), D(900000)), ("O6", D("0.30"), D(700000))]

    def aggregate(rows, rho=D(0), pmul=D(1), imul=D(1)):
        mean = var_i = sig_sum = D(0)
        for _, p, i in rows:
            p2 = min(D(1), p * pmul)
            i2 = i * imul
            mean += p2 * i2
            s = (p2 * (1 - p2)).sqrt() * i2
            var_i += s * s
            sig_sum += s
        var = (1 - rho) * var_i + rho * sig_sum * sig_sum
        sig = var.sqrt()
        return mean, var, sig, mean + Z80 * sig

    for idd, emv in (("O1", 480000), ("O2", 150000), ("O3", 520000),
                     ("O4", 540000), ("O5", 225000), ("O6", 210000)):
        p, i = next((p, i) for k, p, i in REG if k == idd)
        check(f"WE 11.4.1 {idd} EMV", p * i, emv)
    mean, var, sig, p80 = aggregate(REG)
    check("WE 11.4.1 register mean", mean, 2125000)
    check("WE 11.4.1 register variance", var, D("4982875000000"))
    check("WE 11.4.1 register sigma", sig.quantize(D("0.01")), D("2232235.43"))
    check("WE 11.4.1 register P80 independent", p80.quantize(D("0.01")), D("4003649.34"))
    _, _, sig_c, p80_c = aggregate(REG, D("0.30"))
    check("WE 11.4.1 register sigma at rho=0.30", sig_c.quantize(D("0.01")), D("3227337.81"))
    check("WE 11.4.1 register P80 at rho=0.30", p80_c.quantize(D("0.01")), D("4841127.50"))
    check("WE 11.4.1 correlation uplift", (p80_c - p80).quantize(D("0.01")), D("837478.16"))
    check("WE 11.4.1 correlation uplift %",
          ((p80_c - p80) / p80 * 100).quantize(D("0.01")), D("20.92"))
    check("WE 11.4.1 cyber + AI share of mean", D(540000) + D(225000) + D(210000), 975000)
    check("WE 11.4.1 cyber + AI share of mean %",
          (D(975000) / mean * 100).quantize(D("0.01")), D("45.88"))
    check("MCQ 11.4-A distractor mean + 1 sigma",
          (mean + sig).quantize(D("0.01")), D("4357235.43"))

    mean_L, _, sig_L, p80_L = aggregate(REG, D("0.30"), D("1.5"), D("1.4"))
    for idd, emv in (("O1", 1008000), ("O2", 315000), ("O3", 1092000),
                     ("O4", 1134000), ("O5", "472500"), ("O6", 441000)):
        p, i = next((p, i) for k, p, i in REG if k == idd)
        check(f"WE 11.4.2 {idd} lender EMV", p * D("1.5") * i * D("1.4"), D(str(emv)))
    check("WE 11.4.2 lender mean", mean_L, 4462500)
    check("WE 11.4.2 lender sigma", sig_L.quantize(D("0.01")), D("5253725.84"))
    check("WE 11.4.2 lender P80", p80_L.quantize(D("0.01")), D("8884035.66"))
    check("WE 11.4.2 lender mean less sponsor P80",
          (mean_L - p80).quantize(D("0.01")), D("458850.66"))
    check("WE 11.4.2 lender mean / sponsor P80",
          (mean_L / p80).quantize(D("0.0001")), D("1.1146"))
    check("WE 11.4.2 lender P80 / sponsor mean",
          (p80_L / mean).quantize(D("0.0001")), D("4.1807"))

    for lab, pv, ann, cfv, dsc in (
            ("sponsor mean", mean, "253463.69", "6130536.31", "1.2237"),
            ("sponsor P80", p80, "477543.40", "5906456.60", "1.1790"),
            ("sponsor P80 rho", p80_c, "577435.30", "5806564.70", "1.1591"),
            ("lender mean", mean_L, "532273.74", "5851726.26", "1.1681"),
            ("lender P80", p80_L, "1059661.38", "5324338.62", "1.0628")):
        a = pv / AF6_12
        check(f"WE 11.4.4 {lab} annual equivalent", a.quantize(D("0.01")), D(ann))
        check(f"WE 11.4.4 {lab} stressed CFADS", (CF - a).quantize(D("0.01")), D(cfv))
        check(f"WE 11.4.4 {lab} stressed DSCR", ((CF - a) / DS).quantize(D("0.0001")), D(dsc))
    ceiling = HEAD * AF6_12
    check("WE 11.4.4 covenant-preserving PV ceiling",
          ceiling.quantize(D("0.01")), D("3122459.78"))
    check("WE 11.4.4 gap to sponsor P80", (p80 - ceiling).quantize(D("0.01")), D("881189.56"))
    cap_L = ((CF - p80_L / AF6_12) / COV) * AF6_12
    check("WE 11.4.4 lender-stressed debt capacity",
          cap_L.quantize(D("0.01")), D("37198687.03"))
    check("WE 11.4.4 debt capacity removed",
          (DEBT - cap_L).quantize(D("0.01")), D("4801312.97"))
    check("MCQ 11.4-C distractor headroom / AF",
          (HEAD / AF6_12).quantize(D("0.01")), D("44423.27"))

    # ================= Case study B — Larkspur airport concession =================
    LK_DEBT, LK_RATE, LK_N = D(180000000), D("0.065"), 14
    AF_LK = af(LK_RATE, LK_N)
    LK_INST = LK_DEBT / AF_LK
    LK_REV, LK_OPL, LK_OPU, LK_TAX = D(60000000), D(17600000), D(6600000), D(4000000)
    LK_COV = D("1.30")
    check("CS-B AF(0.065,14)", AF_LK.quantize(D("0.000001")), D("9.013842"))
    check("CS-B instalment", LK_INST.quantize(D("0.01")), D("19969286.50"))
    check("CS-B base CFADS", LK_REV - LK_OPL - LK_OPU - LK_TAX, 31800000)
    check("CS-B base DSCR",
          ((LK_REV - LK_OPL - LK_OPU - LK_TAX) / LK_INST).quantize(D("0.0001")), D("1.5924"))
    check("CS-B covenant cash trigger", (LK_INST * LK_COV).quantize(D("0.01")), D("25960072.46"))
    LK_NUM = LK_REV - LK_OPL - LK_TAX
    check("CS-B local numerator", LK_NUM, 38400000)

    def cf_lk(mult, s=D(0)):
        return (LK_REV * s + LK_REV * (1 - s) / mult
                - (LK_OPL + LK_TAX) / mult - LK_OPU)

    check("CS-B model reproduces base", cf_lk(D(1)), 31800000)
    check("CS-B 30 % devaluation revenue",
          (LK_REV / D("1.3")).quantize(D("0.01")), D("46153846.15"))
    check("CS-B 30 % devaluation local opex",
          (LK_OPL / D("1.3")).quantize(D("0.01")), D("13538461.54"))
    check("CS-B 30 % devaluation tax", (LK_TAX / D("1.3")).quantize(D("0.01")), D("3076923.08"))
    check("CS-B 30 % devaluation CFADS",
          cf_lk(D("1.3")).quantize(D("0.01")), D("22938461.54"))
    check("CS-B 30 % devaluation DSCR",
          (cf_lk(D("1.3")) / LK_INST).quantize(D("0.0001")), D("1.1487"))
    for tgt, dev in ((LK_COV, "17.9359"), (D(1), "44.5278")):
        mult = LK_NUM / (LK_INST * tgt + LK_OPU)
        check(f"CS-B breakeven devaluation % at {tgt}",
              ((mult - 1) * 100).quantize(D("0.0001")), D(dev))
    check("CS-B s=40 % at 30 % devaluation CFADS",
          cf_lk(D("1.3"), D("0.40")).quantize(D("0.01")), D("28476923.08"))
    check("CS-B s=40 % at 30 % devaluation DSCR",
          (cf_lk(D("1.3"), D("0.40")) / LK_INST).quantize(D("0.0001")), D("1.4260"))
    check("CS-B s=40 % at 60 % devaluation CFADS", cf_lk(D("1.6"), D("0.40")), 26400000)
    check("CS-B s=40 % at 60 % devaluation DSCR",
          (cf_lk(D("1.6"), D("0.40")) / LK_INST).quantize(D("0.0001")), D("1.3220"))
    Aa = LK_REV - LK_REV / D("1.6")
    Bb = LK_REV / D("1.6") - (LK_OPL + LK_TAX) / D("1.6") - LK_OPU
    check("CS-B minimum indexed share at 60 % devaluation %",
          ((LK_INST * LK_COV - Bb) / Aa * 100).quantize(D("0.0001")), D("38.0448"))
    lo, hi = D(1), D(6)
    for _ in range(300):
        mid = (lo + hi) / 2
        if cf_lk(mid, D("0.40")) > LK_INST * LK_COV:
            lo = mid
        else:
            hi = mid
    check("CS-B s=40 % breakeven devaluation %",
          ((lo - 1) * 100).quantize(D("0.0001")), D("68.2229"))
    check("CS-B tolerance improvement ratio",
          ((lo - 1) / D("0.179359")).quantize(D("0.001")), D("3.804"))
    check("CS-B natural hedge (local costs + tax)", LK_OPL + LK_TAX, 21600000)
    check("CS-B natural hedge % of local revenue",
          ((LK_OPL + LK_TAX) / LK_REV * 100).quantize(D("0.01")), D("36.00"))
    check("CS-B USD debt service % of revenue",
          (LK_INST / LK_REV * 100).quantize(D("0.01")), D("33.28"))

    # ================= Calculation exercises =================
    LX = D("1.35")
    EX1 = [("X1", D("0.25"), D(4000000), D("0.10"), D(3000000), 1000000, 300000, 405000,
            595000, "233.33"),
           ("X2", D("0.40"), D(1500000), D("0.40"), D(1600000), 600000, 640000, 864000,
            -264000, "-6.25"),
           ("X3", D("0.30"), D(2000000), D("0.15"), D(1800000), 600000, 270000, "364500",
            "235500", "122.22")]
    for idd, pa, ia, pb, ib, own, con, prem, net, bkl in EX1:
        check(f"EX 11.1 {idd} own EMV", pa * ia, own)
        check(f"EX 11.1 {idd} transferee EMV", pb * ib, con)
        check(f"EX 11.1 {idd} premium", pb * ib * LX, D(str(prem)))
        check(f"EX 11.1 {idd} net value", pa * ia - pb * ib * LX, D(str(net)))
        check(f"EX 11.1 {idd} breakeven loading %",
              ((pa * ia / (pb * ib) - 1) * 100).quantize(D("0.01")), D(bkl))
    check("EX 11.1 X1+X3 premium", D(405000) + D("364500"), D("769500"))
    check("EX 11.1 X1+X3 retained cost avoided", D(1000000) + D(600000), 1600000)
    check("EX 11.1 X1+X3 net", D(1600000) - D("769500"), D("830500"))

    E2_DEBT, E2_PRIN, E2_CF, E2_COV, E2_SWAP = (D(120000000), D(6000000), D(17500000),
                                                D("1.25"), D("0.0575"))
    check("EX 11.2 interest at 5.50 %", E2_DEBT * D("0.055"), 6600000)
    check("EX 11.2 DS at 5.50 %", E2_PRIN + E2_DEBT * D("0.055"), 12600000)
    check("EX 11.2 DSCR at 5.50 %",
          (E2_CF / (E2_PRIN + E2_DEBT * D("0.055"))).quantize(D("0.0001")), D("1.3889"))
    check("EX 11.2 interest at 7.00 %", E2_DEBT * D("0.07"), 8400000)
    check("EX 11.2 DS at 7.00 %", E2_PRIN + E2_DEBT * D("0.07"), 14400000)
    check("EX 11.2 DSCR at 7.00 %",
          (E2_CF / (E2_PRIN + E2_DEBT * D("0.07"))).quantize(D("0.0001")), D("1.2153"))
    e2_dsmax = E2_CF / E2_COV
    e2_rmax = (e2_dsmax - E2_PRIN) / E2_DEBT
    check("EX 11.2 max debt service", e2_dsmax, 14000000)
    check("EX 11.2 max interest", e2_dsmax - E2_PRIN, 8000000)
    check("EX 11.2 breakeven all-in %", (e2_rmax * 100).quantize(D("0.0001")), D("6.6667"))
    check("EX 11.2 breakeven bp above 5.50 %",
          ((e2_rmax - D("0.055")) * 10000).quantize(D("0.1")), D("116.7"))
    check("EX 11.2 minimum hedge ratio %",
          ((D("0.07") - e2_rmax) / (D("0.07") - E2_SWAP) * 100).quantize(D("0.0001")),
          D("26.6667"))
    check("EX 11.2 swapped DS", E2_PRIN + E2_DEBT * E2_SWAP, 12900000)
    check("EX 11.2 swapped DSCR",
          (E2_CF / (E2_PRIN + E2_DEBT * E2_SWAP)).quantize(D("0.0001")), D("1.3566"))
    check("EX 11.2 swap cost per period", E2_DEBT * (E2_SWAP - D("0.055")), 300000)

    E3_REV, E3_OP, E3_DS = D(30000000), D(11000000), D(12400000)
    e3_loc = E3_REV * D("0.75") - E3_OP * D("0.60")
    e3_usd = E3_REV * D("0.25") - E3_OP * D("0.40")
    check("EX 11.3 local numerator", e3_loc, 15900000)
    check("EX 11.3 USD numerator", e3_usd, 3100000)
    check("EX 11.3 base CFADS", e3_loc + e3_usd, 19000000)
    check("EX 11.3 base DSCR", ((e3_loc + e3_usd) / E3_DS).quantize(D("0.0001")), D("1.5323"))
    check("EX 11.3 +20 % CFADS", e3_loc / D("1.20") + e3_usd, 16350000)
    check("EX 11.3 +20 % DSCR",
          ((e3_loc / D("1.20") + e3_usd) / E3_DS).quantize(D("0.0001")), D("1.3185"))
    check("EX 11.3 +35 % CFADS",
          (e3_loc / D("1.35") + e3_usd).quantize(D("0.01")), D("14877777.78"))
    check("EX 11.3 +35 % DSCR",
          ((e3_loc / D("1.35") + e3_usd) / E3_DS).quantize(D("0.0001")), D("1.1998"))
    check("EX 11.3 covenant CFADS", E3_DS * D("1.30"), 16120000)
    e3_mult = e3_loc / (E3_DS * D("1.30") - e3_usd)
    check("EX 11.3 breakeven devaluation %",
          ((e3_mult - 1) * 100).quantize(D("0.0001")), D("22.1198"))

    R4 = [("i1", D("0.20"), D(2500000)), ("i2", D("0.10"), D(4000000)),
          ("i3", D("0.30"), D(1200000)), ("i4", D("0.15"), D(2000000))]
    m4, v4, s4, p4 = aggregate(R4)
    check("EX 11.4 mean", m4, 1560000)
    check("EX 11.4 variance", v4, D("3252400000000"))
    check("EX 11.4 sigma", s4.quantize(D("0.01")), D("1803441.16"))
    check("EX 11.4 P80", p4.quantize(D("0.01")), D("3077776.08"))
    _, v4c, s4c, p4c = aggregate(R4, D("0.30"))
    check("EX 11.4 sum of item sigmas",
          sum((p * (1 - p)).sqrt() * i for _, p, i in R4).quantize(D("0.01")), D("3464051.93"))
    check("EX 11.4 variance at rho=0.30", v4c.quantize(D(1)), D("5876576724325"))
    check("EX 11.4 sigma at rho=0.30", s4c.quantize(D("0.01")), D("2424165.16"))
    check("EX 11.4 P80 at rho=0.30", p4c.quantize(D("0.01")), D("3600177.40"))
    check("EX 11.4 correlation uplift", (p4c - p4).quantize(D("0.01")), D("522401.32"))
    check("EX 11.4 exposure ceiling", D(900000) * D("7.360087"), D("6624078.30"))
    check("EX 11.4 AF(0.06,10) as used",
          af(D("0.06"), 10).quantize(D("0.000001")), D("7.360087"))

    E5_CF, E5_DS = D(9600000), D(7100000)
    check("EX 11.5 covenant trigger", E5_DS * COV, 8520000)
    check("EX 11.5 headroom", E5_CF - E5_DS * COV, 1080000)
    check("EX 11.5 daily CFADS", (E5_CF / 360).quantize(D("0.01")), D("26666.67"))
    check("EX 11.5 maximum survivable days",
          ((E5_CF - E5_DS * COV) / (E5_CF / 360)).quantize(D("0.0001")), D("40.5000"))
    for days, cfv, dsc in ((45, 8400000, "1.1831"), (30, 8800000, "1.2394"),
                           (15, 9200000, "1.2958")):
        check(f"EX 11.5 {days}-day CFADS", E5_CF - D(days) * E5_CF / 360, cfv)
        check(f"EX 11.5 {days}-day DSCR",
              ((E5_CF - D(days) * E5_CF / 360) / E5_DS).quantize(D("0.0001")), D(dsc))

    # ---------- structural invariants asserted in 11.A.3 ----------
    check("11.A.3 net value signs partition the register",
          1 if all((pa * ia - pb * ib * LOAD > 0) == (idd in ("A1", "A2", "A3"))
                   for idd, pa, ia, pb, ib in ITEMS) else 0, 1)
    check("11.A.3 register mean equals sum of EMV",
          mean - sum(p * i for _, p, i in REG), 0)
    check("11.A.3 correlated P80 exceeds independent P80", 1 if p80_c > p80 else 0, 1)
    check("11.A.3 register P80 exceeds the covenant ceiling", 1 if p80 > ceiling else 0, 1)
