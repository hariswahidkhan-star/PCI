"""PFL-AI Domain 7 — Revenue, demand and commercial models: golden checks.

Every number printed as a result in `domain-07-revenue-demand-commercial.md` is recomputed here
from first principles. Master-thread values are read from ctx, never re-typed.

The domain's engine is the CFADS bridge of KA 6.2.2 expressed in the two commercial variables:

    CFADS = 0.75 * revenue - 0.03 * volume(m3) - 1,896,000

derived from: EBITDA = revenue - 3,600,000 - 0.0375*volume; cash tax = 20% of
(EBITDA - depreciation 2,400,000 - interest 2,520,000); working capital = 5% of revenue.
The checks below rebuild it from those primitives rather than trusting the reduced form.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DS = ctx["KESTREL_INSTALMENT"]
    CF0 = ctx["KESTREL_CFADS"]
    R0, V0 = D(12000000), D(24000000)
    P, VC, FC = D("0.50"), D("0.0375"), D(3600000)
    DEP, INT, TAXR, WCR = D(2400000), D(2520000), D("0.20"), D("0.05")
    AF6_12 = af(D("0.06"), 12)

    def bridge(rev, vol):
        """CFADS from primitives; returns (EBITDA, cash tax, dWC, CFADS)."""
        eb = rev - (FC + VC * vol)
        taxable = eb - DEP - INT
        tax = taxable * TAXR if taxable > 0 else D(0)
        dwc = WCR * rev
        return eb, tax, dwc, eb - tax - dwc

    # --- master-thread reproduction and the reduced form -------------------------------
    eb, tax, dwc, cf = bridge(R0, V0)
    check("D7 anchor EBITDA", eb, ctx["KESTREL_EBITDA"])
    check("D7 anchor cash tax", tax, 516000)
    check("D7 anchor working capital", dwc, 600000)
    check("D7 anchor CFADS ties to master thread", cf, CF0)
    check("D7 reduced form equals primitives",
          D("0.75") * R0 - D("0.03") * V0 - D(1896000), CF0)
    check("D7 AF(6%,12)", AF6_12, "8.383844", D("0.0000005"))

    # --- KA 7.1.1 three demand states, flat 0.50 tariff -------------------------------
    for lab, vol, e_, t_, w_, c_, ds_ in [
        ("high", D(28800000), 9720000, 960000, 720000, 8040000, "1.6049"),
        ("base", D(24000000), 7500000, 516000, 600000, 6384000, "1.2743"),
        ("low", D(19200000), 5280000, 72000, 480000, 4728000, "0.9438"),
    ]:
        e, t, w, c = bridge(P * vol, vol)
        check(f"WE 7.1.1 {lab} EBITDA", e, e_)
        check(f"WE 7.1.1 {lab} cash tax", t, t_)
        check(f"WE 7.1.1 {lab} working capital", w, w_)
        check(f"WE 7.1.1 {lab} CFADS", c, c_)
        check(f"WE 7.1.1 {lab} DSCR", c / DS, ds_, D("0.00005"))
    probs = [(D("0.25"), D(28800000)), (D("0.50"), D(24000000)), (D("0.25"), D(19200000))]
    ev = sum(p * v for p, v in probs)
    ecf = sum(p * bridge(P * v, v)[3] for p, v in probs)
    check("WE 7.1.1 expected volume", ev, 24000000)
    check("WE 7.1.1 expected revenue", P * ev, 12000000)
    check("WE 7.1.1 expected CFADS equals base", ecf, CF0)
    check("WE 7.1.1 expected DSCR", ecf / DS, "1.2743", D("0.00005"))

    # debt capacity comparison
    cap_avail = (CF0 / D("1.30")) * AF6_12
    cap_vol_low = (D(4728000) / D("1.30")) * AF6_12
    cap_vol_150 = (CF0 / D("1.50")) * AF6_12
    check("WE 7.1.1 availability capacity at 1.30x", cap_avail, 41171123, D("0.6"))
    check("WE 7.1.1 low-case debt service at 1.30x", D(4728000) / D("1.30"), 3636923, D("0.6"))
    check("WE 7.1.1 volume capacity on low case", cap_vol_low, 30491396, D("0.6"))
    check("WE 7.1.1 volume capacity at 1.50x on expected", cap_vol_150, 35681640, D("0.6"))
    check("WE 7.1.1 capacity gap", cap_avail - cap_vol_low, 10679727, D("0.6"))
    check("WE 7.1.1 gap as % of availability capacity",
          (cap_avail - cap_vol_low) / cap_avail * 100, "25.94", D("0.005"))
    check("WE 7.1.1 shortfall against the 42m request", D(42000000) - cap_vol_low, 11508604, D("0.6"))

    # revenue and nameplate figures printed in the KA 7.1.1 table and the master thread
    check("D7 nameplate capacity per day", D(30000000) / 365, 82192, D("0.5"))
    for lab, vol, rev_ in [("high", D(28800000), 14400000), ("base", D(24000000), 12000000),
                           ("low", D(19200000), 9600000)]:
        check(f"WE 7.1.1 {lab} revenue", P * vol, rev_)

    # --- KA 7.1.3 availability deductions ---------------------------------------------
    GUAR, MULT = D("0.95"), D("1.5")

    def avail(actual):
        s = GUAR - D(actual)
        ded = R0 * MULT * s
        return s, ded, R0 - ded, bridge(R0 - ded, V0 * (D(1) - s))[3]

    slope = D(9000000) * MULT - D(720000)
    check("WE 7.1.3 slope of CFADS per unit shortfall", slope, 12780000)
    check("WE 7.1.3 cost of one availability point", slope / 100, 127800)
    check("WE 7.1.3 revenue lost per point", R0 * MULT / 100, 180000)
    check("WE 7.1.3 recovered per point", R0 * MULT / 100 - slope / 100, 52200)
    for a, ded_, rev_, cf_, ds_ in [
        ("0.93", 360000, 11640000, 6128400, "1.2233"),
        ("0.91", 720000, 11280000, 5872800, "1.1723"),
    ]:
        s, ded, rev, c = avail(a)
        check(f"WE 7.1.3 deduction at {a}", ded, ded_)
        check(f"WE 7.1.3 revenue at {a}", rev, rev_)
        check(f"WE 7.1.3 CFADS at {a}", c, cf_)
        check(f"WE 7.1.3 DSCR at {a}", c / DS, ds_, D("0.00005"))
    for lab, thr, avail_, ded_, rev_, cf_ in [
        ("covenant 1.20x", "1.20", "92.086", 524560, 11475440, "6011562.28"),
        ("lock-up 1.15x", "1.15", "90.126", 877351, 11122649, "5761080.51"),
        ("payment 1.00x", "1.00", "84.246", 1935725, 10064275, "5009635.23"),
    ]:
        s = (CF0 - DS * D(thr)) / slope
        check(f"WE 7.1.3 {lab} availability", (GUAR - s) * 100, avail_, D("0.0005"))
        check(f"WE 7.1.3 {lab} deduction", R0 * MULT * s, ded_, D("0.6"))
        check(f"WE 7.1.3 {lab} revenue", R0 - R0 * MULT * s, rev_, D("0.6"))
        check(f"WE 7.1.3 {lab} CFADS", CF0 - slope * s, cf_)
    slope1 = D(9000000) * D("1.0") - D(720000)
    check("7.1.3 slope at multiplier 1.0", slope1, 8280000)
    s15 = (CF0 - DS * D("1.20")) / slope
    s10 = (CF0 - DS * D("1.20")) / slope1
    check("7.1.3 covenant availability at multiplier 1.0", (GUAR - s10) * 100, "90.502", D("0.0005"))
    check("7.1.3 tolerance cost of the 1.5x multiplier", (s10 - s15) * 100, "1.584", D("0.0005"))

    # --- KA 7.2.2 banded tariff --------------------------------------------------------
    def banded(v):
        r, rem = D(0), v
        take = min(rem, D(18000000)); r += take * D("0.55"); rem -= take
        take = min(rem, D(6000000)); r += take * D("0.35"); rem -= take
        return r + rem * D("0.10")

    for lab, vol, rev_, cf_, ds_ in [
        ("high", D(28800000), 12480000, 6600000, "1.3175"),
        ("base", D(24000000), 12000000, 6384000, "1.2743"),
        ("low", D(19200000), 10320000, 5268000, "1.0516"),
    ]:
        rev = banded(vol)
        c = bridge(rev, vol)[3]
        check(f"WE 7.2.2 {lab} banded revenue", rev, rev_)
        check(f"WE 7.2.2 {lab} banded CFADS", c, cf_)
        check(f"WE 7.2.2 {lab} banded DSCR", c / DS, ds_, D("0.00005"))
    erev_b = sum(p * banded(v) for p, v in probs)
    ecf_b = sum(p * bridge(banded(v), v)[3] for p, v in probs)
    check("WE 7.2.2 expected banded revenue", erev_b, 11700000)
    check("WE 7.2.2 expected revenue given up", R0 - erev_b, 300000)
    check("WE 7.2.2 give-up as % of base revenue", (R0 - erev_b) / R0 * 100, "2.50")
    check("WE 7.2.2 expected banded CFADS", ecf_b, 6159000)
    cap_b = (D(5268000) / D("1.30")) * AF6_12
    check("WE 7.2.2 banded capacity on low case", cap_b, 33973915, D("0.6"))
    check("WE 7.2.2 capacity gain over flat", cap_b - cap_vol_low, 3482520, D("0.6"))
    check("WE 7.2.2 PV of revenue given up", (R0 - erev_b) * AF6_12, 2515153, D("0.6"))
    check("WE 7.2.2 net gain", cap_b - cap_vol_low - (R0 - erev_b) * AF6_12, 967367, D("1.2"))
    check("WE 7.2.2 low-tail coverage improvement",
          D(5268000) / DS - D(4728000) / DS, "0.1078", D("0.00005"))

    # --- KA 7.2.3 Halyard Connect run-off ---------------------------------------------
    ARR, NRR, FIXH, MCAP = D(24000000), D("0.93"), D(13200000), D(1800000)
    AF7_7 = af(D("0.07"), 7)
    check("WE 7.2.3 AF(7%,7)", AF7_7, "5.389289", D("0.0000005"))
    inst = D(40000000) / AF7_7
    check("WE 7.2.3 level instalment on 40m", inst, "7422128.79", D("0.005"))
    expect = [
        (24000000, 10800000, 9000000, "1.2126"), (22320000, 9120000, 7320000, "0.9862"),
        (20757600, 7557600, 5757600, "0.7757"), ("19304568", "6104568", "4304568", "0.5800"),
        ("17953248.24", "4753248.24", "2953248.24", "0.3979"),
        ("16696520.8632", "3496520.8632", "1696520.8632", "0.2286"),
        ("15527764.402776", "2327764.402776", "527764.402776", "0.0711"),
    ]
    pv_sculpt = D(0)
    for t, (rv, ev_, cv, dv) in enumerate(expect, start=1):
        rev = ARR * NRR ** (t - 1)
        ebt = rev - FIXH
        cfh = ebt - MCAP
        check(f"WE 7.2.3 yr{t} revenue", rev, rv, D("0.6"))
        check(f"WE 7.2.3 yr{t} EBITDA", ebt, ev_, D("0.6"))
        check(f"WE 7.2.3 yr{t} CFADS", cfh, cv, D("0.6"))
        check(f"WE 7.2.3 yr{t} DSCR on level service", cfh / inst, dv, D("0.00005"))
        pv_sculpt += (cfh / D("1.30")) / (D("1.07") ** t)
    check("WE 7.2.3 sculpted capacity at 1.30x", pv_sculpt, 20271839, D("0.6"))
    check("WE 7.2.3 capacity as % of 40m sought", pv_sculpt / D(40000000) * 100, "50.68", D("0.005"))
    warct = D("0.40") * 5 + D("0.35") * 3 + D("0.25") * 1
    check("WE 7.2.3 WARCT", warct, "3.30")
    check("WE 7.2.3 WARCT as % of a 7-year tenor", warct / 7 * 100, "47.14", D("0.005"))
    check("7.2.3 EBITDA fall yr1->2", (D(10800000) - D(9120000)) / D(10800000) * 100,
          "15.56", D("0.005"))
    check("7.2.3 EBITDA fall yr2->3", (D(9120000) - D("7557600")) / D(9120000) * 100,
          "17.13", D("0.005"))
    check("7.2.3 CFADS fall yr1->2", (D(9000000) - D(7320000)) / D(9000000) * 100,
          "18.67", D("0.005"))
    check("7.2.3 CFADS fall yr2->3", (D(7320000) - D("5757600")) / D(7320000) * 100,
          "21.34", D("0.005"))

    # --- KA 7.3.1 indexation architecture ---------------------------------------------
    OPEX0 = D(4500000)

    def rev_t(t, share, g):
        return R0 * (D(share) * (D(1) + D(g)) ** (t - 1) + (D(1) - D(share)))

    def opex_t(t, share, g):
        return OPEX0 * D(share) * (D(1) + D(g)) ** (t - 1) + OPEX0 * (D(1) - D(share))

    for t, rv, ov, ebv, mg in [
        (1, 12000000, 4500000, 7500000, "62.50"), (5, 12996604, 4922970, 8073634, "62.12"),
        (12, 14996032, 5804380, 9191652, "61.29"), (25, 19763769, 8058467, 11705302, "59.23"),
    ]:
        r, o = rev_t(t, "0.80", "0.025"), opex_t(t, "0.70", "0.032")
        check(f"WE 7.3.1 yr{t} revenue", r, rv, D("0.6"))
        check(f"WE 7.3.1 yr{t} opex", o, ov, D("0.6"))
        check(f"WE 7.3.1 yr{t} EBITDA", r - o, ebv, D("1.2"))
        check(f"WE 7.3.1 yr{t} margin", (r - o) / r * 100, mg, D("0.005"))
    r25 = rev_t(25, "0.80", "0.025")
    o25 = opex_t(25, "0.70", "0.032")
    rcagr = (r25 / R0) ** (D(1) / 24) - 1
    ocagr = (o25 / OPEX0) ** (D(1) / 24) - 1
    check("WE 7.3.1 effective revenue rate", rcagr * 100, "2.101", D("0.0005"))
    check("WE 7.3.1 effective cost rate", ocagr * 100, "2.457", D("0.0005"))
    check("WE 7.3.1 effective gap in basis points", (ocagr - rcagr) * 10000, "35.7", D("0.05"))
    check("WE 7.3.1 margin drift to yr25", D("62.50") - (r25 - o25) / r25 * 100, "3.27", D("0.005"))
    rf25 = rev_t(25, "1.00", "0.025")
    check("WE 7.3.1 full-indexation yr25 margin", (rf25 - o25) / rf25 * 100, "62.87", D("0.005"))
    check("WE 7.3.1 full-indexation yr25 EBITDA", rf25 - o25, 13646244, D("1.2"))
    oc25 = opex_t(25, "1.00", "0.032")
    check("WE 7.3.1 fully indexed cost yr25 margin",
          (r25 - oc25) / r25 * 100, "51.51", D("0.005"))
    check("WE 7.3.1 unindexed tariff slice in margin points",
          (rf25 - o25) / rf25 * 100 - (r25 - o25) / r25 * 100, "3.65", D("0.005"))
    check("WE 7.3.1 fixed cost slice saves margin points",
          (r25 - o25) / r25 * 100 - (r25 - oc25) / r25 * 100, "7.72", D("0.005"))
    pv8 = sum((rev_t(t, "1.00", "0.025") - rev_t(t, "0.80", "0.025")) / (D("1.08") ** t)
              for t in range(1, 26))
    check("WE 7.3.1 PV at 8% of the unindexed slice", pv8, 6204143, D("0.6"))
    pv6 = sum((rev_t(t, "1.00", "0.025") - rev_t(t, "0.80", "0.025")) / (D("1.06") ** t)
              for t in range(1, 13))
    check("WE 7.3.1 PV at 6% over the loan years", pv6, 2619217, D("0.6"))
    # year-12 coverage on the contracted indexation case
    bal, int12 = ctx["KESTREL_DEBT"], None
    for t in range(1, 13):
        i = bal * D("0.06")
        bal -= DS - i
        if t == 12:
            int12 = i
    check("7.3.1 yr12 interest", int12, "283564.26", D("0.005"))
    r12, o12 = rev_t(12, "0.80", "0.025"), opex_t(12, "0.70", "0.032")
    eb12 = r12 - o12
    tax12 = (eb12 - DEP - int12) * TAXR
    cf12 = eb12 - tax12 - WCR * r12
    check("7.3.1 yr12 cash tax", tax12, 1301618, D("0.6"))
    check("7.3.1 yr12 CFADS", cf12, 7140233, D("1.2"))
    check("7.3.1 yr12 DSCR", cf12 / DS, "1.4253", D("0.00005"))

    # --- KA 7.3.2 operating leverage ---------------------------------------------------
    contrib = (P - VC) * V0
    check("WE 7.3.2 contribution", contrib, 11100000)
    check("WE 7.3.2 DOL", contrib / ctx["KESTREL_EBITDA"], "1.4800", D("0.00005"))
    el_price = (D("0.75") * R0) / CF0
    el_vol = ((D("0.75") * P - D("0.03")) * V0) / CF0
    check("WE 7.3.2 CFADS elasticity to price (ties to D6 1.4098)",
          el_price, "1.4098", D("0.00005"))
    check("WE 7.3.2 CFADS elasticity to volume", el_vol, "1.2970", D("0.00005"))
    cf_v80 = D("0.75") * P * D(19200000) - D("0.03") * D(19200000) - D(1896000)
    check("WE 7.3.2 CFADS at volume -20%", cf_v80, 4728000)
    check("WE 7.3.2 CFADS fall for a 20% volume fall", (CF0 - cf_v80) / CF0 * 100,
          "25.94", D("0.005"))
    cf_p90 = D("0.75") * (R0 * D("0.9")) - D("0.03") * V0 - D(1896000)
    check("WE 7.3.2 CFADS at price -10%", cf_p90, 5484000)
    check("WE 7.3.2 CFADS fall for a 10% price fall", (CF0 - cf_p90) / CF0 * 100,
          "14.10", D("0.005"))
    # D10 Case study B generalisation
    d10_fall = (D(24000000) - D(19700000)) / D(24000000)
    check("7.3.2 D10 Case B CFADS fall", d10_fall * 100, "17.9167", D("0.00005"))
    check("7.3.2 D10 Case B implied elasticity", d10_fall / D("0.12"), "1.4931", D("0.00005"))
    check("7.3.2 D10 Case B implied revenue", (d10_fall / D("0.12")) * D(24000000),
          35833333, D("0.6"))
    check("7.3.2 D10 Case B implied fixed costs and tax",
          (d10_fall / D("0.12")) * D(24000000) - D(24000000), 11833333, D("0.6"))
    need = D(1) - D("1.20") / D("1.30")
    check("7.3.2 CFADS fall from 1.30x to 1.20x", need * 100, "7.6923", D("0.00005"))
    for margin, elas, tol in [("0.80", "1.2500", "6.15"), ("0.67", "1.4925", "5.15"),
                              ("0.60", "1.6667", "4.62"), ("0.40", "2.5000", "3.08"),
                              ("0.20", "5.0000", "1.54")]:
        e = D(1) / D(margin)
        check(f"7.3.2 elasticity at CFADS margin {margin}", e, elas, D("0.00005"))
        check(f"7.3.2 volume tolerance at CFADS margin {margin}", need / e * 100, tol, D("0.005"))

    # --- KA 7.3.3 tolerance set --------------------------------------------------------
    COV, LOCK = DS * D("1.20"), DS * D("1.15")
    fall = (CF0 - COV) / CF0
    check("7.3.3 covenant CFADS", COV, "6011562.276")
    check("7.3.3 cash headroom", CF0 - COV, "372437.724")
    check("7.3.3 CFADS tolerance", fall * 100, "5.8339", D("0.00005"))
    check("7.3.3 price tolerance (ties to D6 4.14%)", fall / el_price * 100, "4.1382", D("0.00005"))
    check("7.3.3 volume tolerance", fall / el_vol * 100, "4.4980", D("0.00005"))
    Rc = (COV + D(1896000) + D("0.03") * V0) / D("0.75")
    check("7.3.3 revenue at covenant (ties to D6)", Rc, "11503416.368", D("0.005"))
    Vc = (COV + D(1896000)) / (D("0.75") * P - D("0.03"))
    check("7.3.3 volume at covenant", Vc, "22920470.37", D("0.005"))
    check("7.3.3 volume lost at covenant", V0 - Vc, "1079529.63", D("0.005"))
    Vl = (LOCK + D(1896000)) / (D("0.75") * P - D("0.03"))
    check("7.3.3 volume at lock-up", Vl, "22194436.27", D("0.005"))
    check("7.3.3 volume tolerance to lock-up", (V0 - Vl) / V0 * 100, "7.5232", D("0.00005"))
    check("7.3.3 CFADS tolerance overstates volume room by",
          (fall / (fall / el_vol) - 1) * 100, "29.70", D("0.005"))

    # --- KA 7.4.1 expected loss --------------------------------------------------------
    PD, LGD = D("0.006"), D("0.45")
    cum = D(1) - (D(1) - PD) ** 12
    EAD = CF0 * AF6_12
    EL = cum * EAD * LGD
    check("WE 7.4.1 12-year cumulative PD", cum * 100, "6.9671", D("0.00005"))
    check("WE 7.4.1 exposure at default", EAD, "53522459.72", D("0.005"))
    check("WE 7.4.1 expected loss", EL, "1678030.70", D("0.005"))
    check("WE 7.4.1 EL as % of exposure", EL / EAD * 100, "3.135", D("0.0005"))
    check("WE 7.4.1 annualised credit charge", EL / AF6_12, "200150.52", D("0.005"))
    check("WE 7.4.1 CFADS net of credit charge", CF0 - EL / AF6_12, "6183849.48", D("0.005"))
    check("WE 7.4.1 DSCR net of credit charge", (CF0 - EL / AF6_12) / DS, "1.2344", D("0.00005"))
    check("WE 7.4.1 coverage cost of the credit charge",
          CF0 / DS - (CF0 - EL / AF6_12) / DS, "0.0400", D("0.00005"))
    check("WE 7.4.1 credit charge as % of covenant headroom",
          (CF0 / DS - (CF0 - EL / AF6_12) / DS) / (CF0 / DS - D("1.20")) * 100, "53.7", D("0.05"))
    check("WE 7.4.1 90-day receivable exposure", R0 * 90 / 365, "2958904.11", D("0.005"))

    # --- KA 7.4.2 concentration and enhancement ---------------------------------------
    p = cum
    k = [(D(1) - p) ** 4, 4 * p * (D(1) - p) ** 3, 6 * p ** 2 * (D(1) - p) ** 2,
         4 * p ** 3 * (D(1) - p), p ** 4]
    for i, want in enumerate(["74.9111", "22.4399", "2.5207", "0.1258", "0.002356"]):
        check(f"WE 7.4.2 P(exactly {i} of 4 default)", k[i] * 100, want, D("0.00005"))
    check("WE 7.4.2 probabilities sum to one", sum(k), 1, D("0.0000005"))
    check("WE 7.4.2 P(>=50% revenue lost), four-way", (k[2] + k[3] + k[4]) * 100,
          "2.6489", D("0.00005"))
    check("WE 7.4.2 P(>=50% revenue lost), single", p * 100, "6.9671", D("0.00005"))
    check("WE 7.4.2 reduction in that probability",
          (p - (k[2] + k[3] + k[4])) / p * 100, "61.98", D("0.005"))
    check("WE 7.4.2 P(total loss), four-way", k[4] * 100, "0.0023562", D("0.0000005"))
    check("WE 7.4.2 total-loss reduction factor", p / k[4], 2957, D("0.6"))
    check("WE 7.4.2 P(some default), four-way", (D(1) - k[0]) * 100, "25.09", D("0.005"))
    check("WE 7.4.2 expected loss four-way is identical",
          cum * (D("0.25") * EAD) * LGD * 4, EL, D("0.005"))
    cumg = D(1) - (D(1) - D("0.002")) ** 12
    ELg = cumg * EAD * LGD
    check("WE 7.4.2 guaranteed cumulative PD", cumg * 100, "2.3738", D("0.00005"))
    check("WE 7.4.2 guaranteed expected loss", ELg, "571726.30", D("0.005"))
    check("WE 7.4.2 expected-loss reduction", EL - ELg, "1106304.40", D("0.005"))
    bal, totfee, pvfee = ctx["KESTREL_DEBT"], D(0), D(0)
    for t in range(1, 13):
        f = bal * D("0.0035")
        totfee += f
        pvfee += f / (D("1.06") ** t)
        bal -= DS - bal * D("0.06")
    check("WE 7.4.2 total guarantee fee", totfee, "1056744.66", D("0.005"))
    check("WE 7.4.2 PV of guarantee fee", pvfee, "805901.26", D("0.005"))
    check("WE 7.4.2 net expected-loss gain", EL - ELg - pvfee, "300403.14", D("0.005"))
    cap120 = (CF0 / D("1.20")) * AF6_12
    check("WE 7.4.2 capacity at 1.20x", cap120, 44602050, D("0.6"))
    check("WE 7.4.2 capacity gain from enhancement", cap120 - cap_avail, 3430927, D("1.2"))
    check("WE 7.4.2 capacity gain net of fee PV", cap120 - cap_avail - pvfee, 2625026, D("1.2"))

    # --- KA 7.4.3 stress matrix and reverse stress ------------------------------------
    matrix = {
        ("0.05", "-0.20"): "1.0156", ("0.05", "-0.10"): "1.1899",
        ("0.05", "0.00"): "1.3642", ("0.05", "0.10"): "1.5384",
        ("0.00", "-0.20"): "0.9438", ("0.00", "-0.10"): "1.1091",
        ("0.00", "0.00"): "1.2743", ("0.00", "0.10"): "1.4396",
        ("-0.05", "-0.20"): "0.8719", ("-0.05", "-0.10"): "1.0282",
        ("-0.05", "0.00"): "1.1845", ("-0.05", "0.10"): "1.3408",
        ("-0.10", "-0.20"): "0.8001", ("-0.10", "-0.10"): "0.9474",
        ("-0.10", "0.00"): "1.0947", ("-0.10", "0.10"): "1.2420",
    }
    for (pm, vm), want in sorted(matrix.items()):
        v = V0 * (D(1) + D(vm))
        r = P * (D(1) + D(pm)) * v
        c = D("0.75") * r - D("0.03") * v - D(1896000)
        check(f"WE 7.4.3 DSCR price {pm} volume {vm}", c / DS, want, D("0.00005"))
    check("WE 7.4.3 D6 cross-check: DSCR at revenue +10%",
          (D("0.75") * (R0 * D("1.10")) - D("0.03") * V0 - D(1896000)) / DS,
          "1.4540", D("0.00005"))
    Vr = (COV + D(1896000)) / (D("0.75") * P * D("0.95") - D("0.03"))
    check("WE 7.4.3 volume to hold 1.20x at price -5%", Vr, "24237738.78", D("0.005"))
    check("WE 7.4.3 volume increase required", (Vr - V0) / V0 * 100, "0.9906", D("0.00005"))
    check("WE 7.4.3 CFADS at price -5%, base volume",
          D("0.75") * (P * D("0.95") * V0) - D("0.03") * V0 - D(1896000), 5934000)

    # --- 7.A.1 the value-neutral collar ------------------------------------------------
    MRG, CAPR = D(10800000), D(13200000)
    tot, ecoll = D(0), D(0)
    for pr, v in probs:
        raw = P * v
        adj = min(max(raw, MRG), CAPR)
        tot += pr * (adj - raw)
        ecoll += pr * (D("0.75") * adj - D("0.03") * v - D(1896000))
    check("7.A.1 expected net transfer is nil", tot, 0)
    check("7.A.1 expected CFADS unchanged", ecoll, CF0)
    cf_lo = D("0.75") * MRG - D("0.03") * D(19200000) - D(1896000)
    cf_hi = D("0.75") * CAPR - D("0.03") * D(28800000) - D(1896000)
    check("7.A.1 collared low CFADS", cf_lo, 5628000)
    check("7.A.1 collared high CFADS", cf_hi, 7140000)
    check("7.A.1 collared low DSCR", cf_lo / DS, "1.1234", D("0.00005"))
    check("7.A.1 collared high DSCR", cf_hi / DS, "1.4253", D("0.00005"))
    cap_coll = (cf_lo / D("1.30")) * AF6_12
    check("7.A.1 collared capacity on low case", cap_coll, 36295595, D("0.6"))
    check("7.A.1 capacity gain from the collar", cap_coll - cap_vol_low, 5804200, D("0.6"))

    # --- 7.A.2 the merchant tail -------------------------------------------------------
    rev_m = D("0.70") * R0
    cf_m = D("0.75") * rev_m - D("0.03") * V0 - D(1896000)
    check("7.A.2 merchant revenue", rev_m, 8400000)
    check("7.A.2 merchant CFADS", cf_m, 3684000)
    check("7.A.2 merchant DSCR", cf_m / DS, "0.7354", D("0.00005"))
    AF6_8 = af(D("0.06"), 8)
    check("7.A.2 AF(6%,8)", AF6_8, "6.209794", D("0.0000005"))
    check("7.A.2 eight-year capacity at 1.30x", (CF0 / D("1.30")) * AF6_8, 30494864, D("0.6"))
    check("7.A.2 shortfall against the twelve-year structure",
          cap_avail - (CF0 / D("1.30")) * AF6_8, 10676259, D("1.2"))
    check("7.A.2 level service constrained by the weakest period", cf_m / D("1.30"),
          2833846, D("0.6"))
    check("7.A.2 level-service capacity", (cf_m / D("1.30")) * AF6_12, 23758524, D("0.6"))
    pvs = sum(((CF0 if t <= 8 else cf_m) / (D("1.30") if t <= 8 else D("1.80")))
              / (D("1.06") ** t) for t in range(1, 13))
    check("7.A.2 sculpted capacity", pvs, 34944420, D("0.6"))
    check("7.A.2 capacity destroyed by level service",
          pvs - (cf_m / D("1.30")) * AF6_12, 11185896, D("1.2"))

    # --- Case study A: the indexation choice ------------------------------------------
    pvA = sum(rev_t(t, "1.00", "0.020") / (D("1.08") ** t) for t in range(1, 26))
    pvB = sum(rev_t(t, "0.80", "0.025") / (D("1.08") ** t) for t in range(1, 26))
    check("Case A PV of revenue, 100% at 2.0%", pvA, 152088430, D("0.6"))
    check("Case A PV of revenue, 80% at 2.5%", pvB, 152913886, D("0.6"))
    check("Case A value destroyed by the offer", pvB - pvA, 825456, D("1.2"))
    pvA6 = sum(rev_t(t, "1.00", "0.020") / (D("1.06") ** t) for t in range(1, 13))
    pvB6 = sum(rev_t(t, "0.80", "0.025") / (D("1.06") ** t) for t in range(1, 13))
    check("Case A value destroyed over the loan years", pvB6 - pvA6, 166192, D("1.2"))
    check("Case A yr25 revenue, 100% at 2.0%", rev_t(25, "1.00", "0.020"), 19301247, D("0.6"))
    rA12, oA12 = rev_t(12, "1.00", "0.020"), opex_t(12, "0.70", "0.032")
    ebA12 = rA12 - oA12
    cfA12 = ebA12 - (ebA12 - DEP - int12) * TAXR - WCR * rA12
    check("Case A yr12 DSCR, 100% at 2.0%", cfA12 / DS, "1.4140", D("0.00005"))

    def pv_ebitda(rs, rg, cs, cg):
        return sum((rev_t(t, rs, rg) - opex_t(t, cs, cg)) / (D("1.08") ** t)
                   for t in range(1, 26))

    base_pv = pv_ebitda("0.80", "0.025", "0.70", "0.032")
    settle = pv_ebitda("0.90", "0.025", "0.70", "0.025")
    check("Case A PV of EBITDA, opening position", base_pv, 93938399, D("0.6"))
    check("Case A PV of EBITDA, settlement", settle, 99836527, D("0.6"))
    check("Case A total gain", settle - base_pv, 5898128, D("1.2"))
    check("Case A gain from ten more points of tariff indexation",
          pv_ebitda("0.90", "0.025", "0.70", "0.032") - base_pv, 3102071, D("1.2"))
    check("Case A gain from index matching",
          pv_ebitda("0.80", "0.025", "0.70", "0.025") - base_pv, 2796057, D("1.2"))
    r25s, o25s = rev_t(25, "0.90", "0.025"), opex_t(25, "0.70", "0.025")
    check("Case A settlement yr25 margin", (r25s - o25s) / r25s * 100, "66.01", D("0.005"))

    # --- Case study B: Northwind Data Campus ------------------------------------------
    AF65_10 = af(D("0.065"), 10)
    check("Case B AF(6.5%,10)", AF65_10, "7.188830", D("0.0000005"))
    rev1, opex1, mc = D(34000000), D(12000000), D(1500000)
    cf1 = rev1 - opex1 - mc
    check("Case B single-anchor CFADS", cf1, 20500000)
    d1 = (cf1 / D("1.60")) * AF65_10
    check("Case B single-anchor debt service", cf1 / D("1.60"), 12812500)
    check("Case B single-anchor capacity", d1, 92106887, D("0.6"))
    check("Case B single-anchor equity", D(200000000) - d1, 107893113, D("0.6"))
    check("Case B single-anchor equity share",
          (D(200000000) - d1) / D(200000000) * 100, "53.95", D("0.005"))
    rev2 = D("0.75") * rev1 + D("0.25") * rev1 * D("0.94")
    cf2 = rev2 - (opex1 + D(400000)) - mc
    check("Case B diversified revenue", rev2, 33490000)
    check("Case B revenue reduction", rev1 - rev2, 510000)
    check("Case B revenue reduction %", (rev1 - rev2) / rev1 * 100, "1.50")
    check("Case B diversified CFADS", cf2, 19590000)
    check("Case B CFADS reduction", cf1 - cf2, 910000)
    check("Case B CFADS reduction %", (cf1 - cf2) / cf1 * 100, "4.44", D("0.005"))
    d2 = (cf2 / D("1.35")) * AF65_10
    check("Case B diversified debt service", cf2 / D("1.35"), 14511111, D("0.6"))
    check("Case B diversified capacity", d2, 104317914, D("0.6"))
    check("Case B capacity gain", d2 - d1, 12211027, D("1.2"))
    check("Case B capacity gain net of reserve", d2 - d1 - D(2000000), 10211027, D("1.2"))
    check("Case B diversified equity", D(200000000) - d2, 95682086, D("0.6"))
    check("Case B diversified equity share",
          (D(200000000) - d2) / D(200000000) * 100, "47.84", D("0.005"))
    check("Case B WARCT diversified", D("0.75") * 10 + D("0.25") * 3, "8.25")

    # --- Calculation exercises ---------------------------------------------------------
    check("Ex 7.1 deduction", D(20000000) * D("1.25") * D("0.03"), 750000)
    check("Ex 7.1 deduction as % of charge",
          D(20000000) * D("1.25") * D("0.03") / D(20000000) * 100, "3.75")
    check("Ex 7.1 revenue received", D(20000000) - 750000, 19250000)
    check("Ex 7.1 error: no multiplier", D(20000000) * D("0.03"), 600000)
    check("Ex 7.1 error: on total unavailability",
          D(20000000) * D("1.25") * D("0.06"), 1500000)
    T2, FC2, VC2, DS2 = D("0.80"), D(4000000), D("0.10"), D(6000000)
    hi = T2 * D(18000000) - (FC2 + VC2 * D(18000000))
    lo = T2 * D(10500000) - (FC2 + VC2 * D(10500000))
    check("Ex 7.2 high CFADS", hi, 8600000)
    check("Ex 7.2 high DSCR", hi / DS2, "1.4333", D("0.00005"))
    check("Ex 7.2 low CFADS", lo, 3350000)
    check("Ex 7.2 low DSCR", lo / DS2, "0.5583", D("0.00005"))
    ecf2 = D("0.60") * hi + D("0.40") * lo
    check("Ex 7.2 expected volume", D("0.60") * D(18000000) + D("0.40") * D(10500000), 15000000)
    check("Ex 7.2 expected CFADS", ecf2, 6500000)
    check("Ex 7.2 expected DSCR", ecf2 / DS2, "1.0833", D("0.00005"))
    AF7_10 = af(D("0.07"), 10)
    check("Ex 7.2 AF(7%,10)", AF7_10, "7.023582", D("0.0000005"))
    check("Ex 7.2 capacity on expected CFADS", (ecf2 / D("1.25")) * AF7_10, 36522624, D("0.6"))
    check("Ex 7.2 capacity on low case", (lo / D("1.25")) * AF7_10, 18823199, D("0.6"))
    check("Ex 7.2 capacity difference",
          (ecf2 / D("1.25")) * AF7_10 - (lo / D("1.25")) * AF7_10, 17699425, D("1.2"))
    R3, O3 = D(30000000), D(18000000)
    r3 = R3 * (D("0.90") * D("1.022") ** 19 + D("0.10"))
    o3 = O3 * D("0.60") * D("1.035") ** 19 + O3 * D("0.40")
    check("Ex 7.3 yr1 EBITDA", R3 - O3, 12000000)
    check("Ex 7.3 yr1 margin", (R3 - O3) / R3 * 100, "40.00")
    check("Ex 7.3 yr20 revenue", r3, 43825432, D("0.6"))
    check("Ex 7.3 yr20 opex", o3, 27963014, D("0.6"))
    check("Ex 7.3 yr20 EBITDA", r3 - o3, 15862417, D("1.2"))
    check("Ex 7.3 yr20 margin", (r3 - o3) / r3 * 100, "36.19", D("0.005"))
    check("Ex 7.3 margin drift", D("40.00") - (r3 - o3) / r3 * 100, "3.81", D("0.005"))
    check("Ex 7.3 effective revenue rate", ((r3 / R3) ** (D(1) / 19) - 1) * 100,
          "2.015", D("0.0005"))
    check("Ex 7.3 effective cost rate", ((o3 / O3) ** (D(1) / 19) - 1) * 100,
          "2.346", D("0.0005"))
    check("Ex 7.3 effective gap in basis points",
          (((o3 / O3) ** (D(1) / 19)) - ((r3 / R3) ** (D(1) / 19))) * 10000, "33.1", D("0.05"))
    c4, e4 = D(50000000) * D("0.70"), D(50000000) * D("0.70") - D(18000000)
    check("Ex 7.4 contribution", c4, 35000000)
    check("Ex 7.4 EBITDA", e4, 17000000)
    check("Ex 7.4 DOL", c4 / e4, "2.0588", D("0.00005"))
    need4 = D(1) - D("1.25") / D("1.40")
    check("Ex 7.4 required earnings fall", need4 * 100, "10.7143", D("0.00005"))
    check("Ex 7.4 volume fall", need4 / (c4 / e4) * 100, "5.2041", D("0.00005"))
    v4 = need4 / (c4 / e4)
    check("Ex 7.4 EBITDA after the volume fall", c4 * (D(1) - v4) - D(18000000),
          15178571, D("0.6"))
    check("Ex 7.4 EBITDA as % of base",
          (c4 * (D(1) - v4) - D(18000000)) / e4 * 100, "89.29", D("0.005"))
    cum5 = D(1) - (D(1) - D("0.01")) ** 10
    check("Ex 7.5 10-year cumulative PD", cum5 * 100, "9.5618", D("0.00005"))
    check("Ex 7.5 expected loss", cum5 * D(80000000) * D("0.50"), "3824717.00", D("0.6"))
    check("Ex 7.5 two-way P(>=50% loss)", (D(1) - (D(1) - cum5) ** 2) * 100,
          "18.2093", D("0.00005"))
    check("Ex 7.5 two-way P(total loss)", cum5 * cum5 * 100, "0.9143", D("0.00005"))

    # --- MCQ and rationale figures not otherwise printed -------------------------------
    check("MCQ 7.1-B distractor C: multiplier on total unavailability",
          R0 * MULT * D("0.09"), 1620000)
    check("WE 7.1.3 volume at 93% availability", V0 * (D(1) - D("0.02")), 23520000)
    check("WE 7.1.3 volume at 91% availability", V0 * (D(1) - D("0.04")), 23040000)
    check("MCQ 7.2-A band 1 revenue", D(18000000) * D("0.55"), 9900000)
    check("MCQ 7.2-A distractor C: 0.55 applied to the whole volume",
          D(19200000) * D("0.55"), 10560000)
    check("WE 7.2.2 expected banded DSCR", D(6159000) / DS, "1.2294", D("0.00005"))
    check("MCQ 7.3-A distractor D: index divided by the indexed share",
          D("0.025") / D("0.80") * 100, "3.125", D("0.00005"))
    check("MCQ 7.3-B distractor B: revenue over EBITDA", R0 / D(7500000), "1.6000", D("0.00005"))
    check("MCQ 7.3-B distractor D: inverted DOL", D(7500000) / contrib, "0.6757", D("0.00005"))
    check("7.3.2 CFADS fall for a 10% volume fall",
          (CF0 - (D("0.75") * P * D(21600000) - D("0.03") * D(21600000) - D(1896000)))
          / CF0 * 100, "12.97", D("0.005"))
    check("7.3.2 price-versus-volume gap in points at a 10% revenue move",
          (CF0 - cf_p90) / CF0 * 100
          - (CF0 - (D("0.75") * P * D(21600000) - D("0.03") * D(21600000) - D(1896000)))
          / CF0 * 100, "1.13", D("0.005"))
    check("7.4.2 P(no default), single offtaker", (D(1) - p) * 100, "93.0329", D("0.00005"))
    check("MCQ 7.4-A distractor A: single-year PD", PD * EAD * LGD, 144511, D("0.6"))
    check("MCQ 7.4-A distractor C: LGD omitted", cum * EAD, 3728957, D("0.6"))
    check("MCQ 7.4-A distractor D: PD omitted", EAD * LGD, 24085107, D("0.6"))
    check("Ex 7.3 fixed portion of opex", O3 * D("0.40"), 7200000)
    check("Ex 7.4 common error: coverage at a 10.7143% volume fall",
          (c4 * (D(1) - need4) - D(18000000)) / e4 * D("1.40"), "1.09", D("0.005"))
    check("Ex 7.2 residual sought after the run-off answer",
          D(40000000) - pv_sculpt, 19728161, D("0.6"))
