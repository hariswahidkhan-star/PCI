"""PFL-AI Domain 15 — Operations, performance and restructuring: golden checks.

Every number printed as a result in `domain-15-operations-performance-restructuring.md` is
recomputed here from first principles. Master-thread values are read from ctx, never re-typed.

The domain has six engines:

  1. The operating bridge (KA 15.1.2) — physical drivers to defined CFADS. Revenue is a capacity
     payment abated pro rata below the guaranteed availability plus a volume payment; cash opex is
     a fixed component plus per-cubic-metre energy and chemicals plus insurance; cash tax is the
     rate applied to EBITDA less depreciation less interest, with losses carried forward; working
     capital is receivables (45/360 of revenue) less payables (90/360 of cash opex) plus inventory,
     and its movement is the last line of CFADS. The bridge must reproduce the master thread's
     7,500,000 / 5,100,000 / 6,984,000 / 6,384,000 exactly.
  2. Triggers (KA 15.1.3) — debt service times each threshold gives the CFADS trigger; the headroom
     divided by the cash-to-revenue gearing (1 - tax rate) gives the revenue tolerance; divided
     again by each driver's own unit economics gives availability points, cubic metres or unit cost.
  3. The waterfall (KA 15.2) — residual = CFADS - debt service - reserve top-up; a top-up the
     residual cannot fund is drawn from the distribution-block account; distributions are nil
     unless both the backward and forward legs of the distribution condition pass, in which case
     the year's residual plus the whole block balance is released.
  4. Refinancing (KA 15.3.2-15.3.4) — incremental cash is the old instalment ceasing against the
     new instalment arising over its own tenor; the swap break cost is the rate dislocation applied
     to each remaining opening balance and discounted at the market rate; the breakeven rate solves
     PV(incremental) = costs; the value decomposes into margin, tenor and their interaction; a cash
     sweep is priced by re-running the equity profile with prepayment.
  5. Restructuring (KA 15.4.2-15.4.3) — sustainable service = revised CFADS / target coverage; the
     extension tenor is the smallest n with outstanding/AF(r,n) <= that; the haircut is outstanding
     less sustainable service * AF(r, remaining n); lender recovery is the PV of what is received at
     the contract rate (par, by the extension identity) and at a risk-adjusted rate; the enforcement
     floor is a distressed buyer's enterprise value net of costs and delay, and it bounds the set.
  6. Handback (KA 15.4.5) — a sinking fund whose contribution is the obligation / FVAF(reserve
     rate, years); its present cost to equity is contribution * AF(discount, years) * DF(discount,
     years deferred), and when the reserve rate equals the discount rate every profile costs the
     discounted obligation, which is the invariant the topic turns on.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DEBT = ctx["KESTREL_DEBT"]
    CAPEX = ctx["KESTREL_CAPEX"]
    DS = ctx["KESTREL_INSTALMENT"]
    CF = ctx["KESTREL_CFADS"]
    RATE = ctx["KESTREL_RATE"]
    TEN = ctx["KESTREL_TENOR"]
    LIFE = ctx["KESTREL_LIFE"]
    EQ = ctx["KESTREL_EQUITY"]
    DISC = ctx["KESTREL_DISCOUNT"]

    def df(r, t):
        return (D(1) + D(r)) ** -int(t)

    def fvaf(r, n):
        r = D(r)
        return ((D(1) + r) ** int(n) - 1) / r

    def npv(flows, r, t0=1):
        """flows[0] sits at period t0."""
        r = D(r)
        return sum(f / (D(1) + r) ** (i + t0) for i, f in enumerate(flows))

    def irr(flows):
        """flows[0] at t=0; bisection on a monotone-decreasing NPV over the relevant range."""
        lo, hi = D("-0.95"), D(3)
        for _ in range(400):
            mid = (lo + hi) / 2
            if sum(f / (D(1) + mid) ** i for i, f in enumerate(flows)) > 0:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    q2 = lambda x: x.quantize(D("0.01"))
    q4 = lambda x: x.quantize(D("0.0001"))
    pct = lambda x: (x * 100).quantize(D("0.0001"))

    # ================================================================ 1. amortisation anchors
    sched, bal = [], DEBT
    for _ in range(TEN):
        i = bal * RATE
        p = DS - i
        sched.append({"open": bal, "interest": i, "principal": p})
        bal -= p
    check("D15 Y1 interest ties to the master thread", q2(sched[0]["interest"]),
          ctx["KESTREL_INTEREST_Y1"])
    check("D15 balance after Y2 (B2)", q2(sched[2]["open"]), D("36871351.43"))
    check("D15 balance after Y3 (B3)", q2(sched[3]["open"]), D("34073997.28"))
    check("D15 balance after Y5 (B5)", q2(sched[5]["open"]), D("27965694.77"))
    check("D15 Y2 interest", q2(sched[1]["interest"]), D("2370621.89"))
    check("D15 Y3 interest", q2(sched[2]["interest"]), D("2212281.09"))
    check("D15 Y4 interest", q2(sched[3]["interest"]), D("2044439.84"))
    check("D15 Y5 interest", q2(sched[4]["interest"]), D("1866528.11"))
    check("D15 Y6 interest", q2(sched[5]["interest"]), D("1677941.69"))
    check("D15 Y2 principal", q2(sched[1]["principal"]), D("2639013.34"))
    check("D15 Y3 principal", q2(sched[2]["principal"]), D("2797354.14"))
    check("D15 Y4 principal", q2(sched[3]["principal"]), D("2965195.39"))
    check("D15 Y5 principal", q2(sched[4]["principal"]), D("3143107.12"))
    check("D15 Y6 principal", q2(sched[5]["principal"]), D("3331693.54"))

    # ================================================================ 2. the operating bridge
    CAPPAY, AGUAR, PVOL = D(7300000), D("0.95"), D("0.55")
    FOM, INS, CHEM, EBASE = D(2100000), D(200000), D("0.04"), D("0.26")
    DEP = CAPEX / D(LIFE)
    TAXR = D("0.20")
    check("D15 depreciation", q2(DEP), D("2400000.00"))

    def bridge(avail, vol, energy, interest, prev_wc, loss_cf=D(0)):
        avail, vol, energy = D(str(avail)), D(vol), D(str(energy))
        cappay = CAPPAY * min(D(1), avail / AGUAR)
        rev = cappay + PVOL * vol
        opex = FOM + energy * vol + CHEM * vol + INS
        ebitda = rev - opex
        pbt = ebitda - DEP - interest
        taxable = pbt - loss_cf
        tax = TAXR * taxable if taxable > 0 else D(0)
        new_cf = D(0) if taxable > 0 else -taxable
        recv = rev * D(45) / D(360)
        pay = opex * D(90) / D(360)
        wc = recv - pay + D(325000)
        dwc = -(wc - prev_wc)
        return {"cappay": cappay, "rev": rev, "opex": opex, "ebitda": ebitda,
                "ebit": ebitda - DEP, "pbt": pbt, "tax": tax, "pre": ebitda - tax,
                "recv": recv, "pay": pay, "wc": wc, "dwc": dwc,
                "cfads": ebitda - tax + dwc, "loss_cf": new_cf}

    y1 = bridge(0.95, 10000000, 0.26, sched[0]["interest"], D(0))
    check("D15 Y1 capacity payment", q2(y1["cappay"]), D("7300000.00"))
    check("D15 Y1 volume payment", q2(PVOL * D(10000000)), D("5500000.00"))
    check("D15 Y1 revenue", q2(y1["rev"]), D("12800000.00"))
    check("D15 Y1 cash operating cost", q2(y1["opex"]), D("5300000.00"))
    check("D15 Y1 EBITDA ties to master thread", q2(y1["ebitda"]), ctx["KESTREL_EBITDA"])
    check("D15 Y1 EBIT ties to master thread", q2(y1["ebit"]), ctx["KESTREL_EBIT"])
    check("D15 Y1 profit before tax", q2(y1["pbt"]), D("2580000.00"))
    check("D15 Y1 cash tax", q2(y1["tax"]), D("516000.00"))
    check("D15 Y1 CFADS before working capital", q2(y1["pre"]), D("6984000.00"))
    check("D15 Y1 receivables 45/360", q2(y1["recv"]), D("1600000.00"))
    check("D15 Y1 payables 90/360", q2(y1["pay"]), D("1325000.00"))
    check("D15 Y1 net working capital", q2(y1["wc"]), D("600000.00"))
    check("D15 Y1 working-capital movement", q2(y1["dwc"]), D("-600000.00"))
    check("D15 Y1 CFADS as defined ties to master thread", q2(y1["cfads"]), CF)
    check("D15 Y1 DSCR", q4(y1["cfads"] / DS), D("1.2743"))

    # ================================================================ 3. triggers in driver units
    COV, LOCK, DIST = D("1.20"), D("1.15"), D("1.25")
    GEAR = D(1) - TAXR
    check("D15 cash-to-revenue gearing", GEAR, D("0.80"))
    cov_cash, lock_cash, dist_cash = DS * COV, DS * LOCK, DS * DIST
    check("D15 covenant trigger 1.20x", q2(cov_cash), D("6011562.28"))
    check("D15 lock-up trigger 1.15x", q2(lock_cash), D("5761080.51"))
    check("D15 distribution trigger 1.25x", q2(dist_cash), D("6262044.04"))
    check("D15 covenant headroom", q2(CF - cov_cash), D("372437.72"))
    check("D15 covenant headroom %", pct((CF - cov_cash) / CF), D("5.8339"))
    check("D15 distribution headroom", q2(CF - dist_cash), D("121955.96"))
    check("D15 distribution headroom %", pct((CF - dist_cash) / CF), D("1.9103"))
    check("D15 lock-up headroom", q2(CF - lock_cash), D("622919.49"))
    check("D15 lock-up headroom %", pct((CF - lock_cash) / CF), D("9.7575"))
    rev_cov, rev_dist = (CF - cov_cash) / GEAR, (CF - dist_cash) / GEAR
    check("D15 revenue headroom to covenant", q2(rev_cov), D("465547.16"))
    check("D15 revenue headroom to distribution test", q2(rev_dist), D("152444.95"))
    abate = CAPPAY / D(95)
    check("D15 abatement per availability point", q2(abate), D("76842.11"))
    check("D15 availability points to covenant", q4(rev_cov / abate), D("6.0585"))
    check("D15 availability floor before covenant", q4(D(95) - rev_cov / abate), D("88.9415"))
    check("D15 availability points to distribution test", q4(rev_dist / abate), D("1.9839"))
    check("D15 availability floor before distribution test",
          q4(D(95) - rev_dist / abate), D("93.0161"))
    contrib = PVOL - EBASE - CHEM
    check("D15 volume cash contribution per m3", contrib, D("0.25"))
    check("D15 volume m3 to covenant", q2(rev_cov / contrib), D("1862188.62"))
    check("D15 volume % to covenant", pct(rev_cov / contrib / D(10000000)), D("18.6219"))
    check("D15 volume m3 to distribution test", q2(rev_dist / contrib), D("609779.81"))
    check("D15 volume % to distribution test",
          pct(rev_dist / contrib / D(10000000)), D("6.0978"))
    check("D15 energy rise to covenant per m3",
          (rev_cov / D(10000000)).quantize(D("0.000001")), D("0.046555"))
    check("D15 energy rise as % of 0.26", pct(rev_cov / D(10000000) / EBASE), D("17.9058"))
    check("D15 energy rise to distribution test per m3",
          (rev_dist / D(10000000)).quantize(D("0.000001")), D("0.015244"))
    check("D15 MCQ 15.1-B: 500,000 revenue fall costs", q2(D(500000) * GEAR), D("400000.00"))
    check("D15 MCQ 15.1-D distractor: gearing omitted",
          q4(D(95) - (CF - cov_cash) / abate), D("90.1532"))
    check("D15 MCQ 15.1-D distractor: gearing applied twice",
          q4(D(95) - rev_cov / GEAR / abate), D("87.4269"))

    # ================================================================ 4. the actual six years
    drivers = [(0.95, 10000000, 0.26), (0.92, 9500000, 0.40), (0.88, 9000000, 0.42),
               (0.945, 9900000, 0.32), (0.95, 10000000, 0.30), (0.95, 10000000, 0.30)]
    rows, prev_wc, loss = [], D(0), D(0)
    for t, (a, v, e) in enumerate(drivers):
        r = bridge(a, v, e, sched[t]["interest"], prev_wc, loss)
        prev_wc, loss = r["wc"], r["loss_cf"]
        rows.append(r)
    expected_cfads = [D("6384000.00"), D("5963894.11"), D("5202936.48"),
                      D("6094641.91"), D("6480627.99"), D("6495588.34")]
    expected_dscr = [D("1.2743"), D("1.1905"), D("1.0386"),
                     D("1.2166"), D("1.2936"), D("1.2966")]
    for t in range(6):
        check(f"D15 actual Y{t + 1} CFADS", q2(rows[t]["cfads"]), expected_cfads[t])
        check(f"D15 actual Y{t + 1} DSCR", q4(rows[t]["cfads"] / DS), expected_dscr[t])
    check("D15 actual Y2 revenue", q2(rows[1]["rev"]), D("12294473.68"))
    check("D15 actual Y2 EBITDA", q2(rows[1]["ebitda"]), D("5814473.68"))
    check("D15 actual Y2 working-capital release", q2(rows[1]["dwc"]), D("358190.79"))
    check("D15 actual Y3 EBITDA", q2(rows[2]["ebitda"]), D("5272105.26"))
    total_actual = sum(r["cfads"] for r in rows)
    check("D15 six-year actual CFADS", q2(total_actual), D("36621688.84"))
    check("D15 six-year base CFADS", q2(CF * 6), D("38304000.00"))
    check("D15 six-year CFADS shortfall", q2(CF * 6 - total_actual), D("1682311.16"))

    # rolling four-quarter tests
    quarters = [D(1700000), D(1640000), D(1560000), D(1484000),
                D(1610000), D(1520000), D(1440000)]
    quarters.append(rows[1]["cfads"] - sum(quarters[4:]))
    check("D15 Y1 quarters sum to the documented CFADS", q2(sum(quarters[:4])), CF)
    check("D15 Y2 residual quarter", q2(quarters[7]), D("1393894.11"))
    rolling = [D("6384000.00"), D("6294000.00"), D("6174000.00"),
               D("6054000.00"), D("5963894.11")]
    rolling_dscr = [D("1.2743"), D("1.2564"), D("1.2324"), D("1.2085"), D("1.1905")]
    for k in range(5):
        w = sum(quarters[k:k + 4])
        check(f"D15 rolling window {k + 1} CFADS", q2(w), rolling[k])
        check(f"D15 rolling window {k + 1} DSCR", q4(w / DS), rolling_dscr[k])
    FWD = D(5500000)
    check("D15 forward DSCR at the Y1 test date", q4(FWD / DS), D("1.0979"))
    check("D15 Y2 outturn above the re-forecast", q2(rows[1]["cfads"] - FWD), D("463894.11"))

    # ================================================================ 5. the waterfall
    MRA, DSRA = D(600000), DS / 2
    check("D15 DSRA six months of debt service", q2(DSRA), D("2504817.62"))
    check("D15 base-case distributable", q2(CF - DS - MRA), D("774364.77"))
    check("D15 base-case cash yield %", pct((CF - DS - MRA) / EQ), D("4.3020"))
    check("D15 debt service as % of CFADS", pct(DS / CF), D("78.4717"))
    check("D15 reserve charge as % of the base dividend", pct(MRA / (CF - DS - MRA)),
          D("77.4829"))
    check("D15 DSRA untouched: worst-year surplus over debt service",
          q2(rows[2]["cfads"] - DS), D("193301.25"))
    INJ = D(1500000)
    mra_bal, block, dist = D(0), D(0), []
    fwd_leg = [FWD] + [r["cfads"] for r in rows[1:]] + [rows[5]["cfads"]]
    for t in range(6):
        after = rows[t]["cfads"] - DS
        if t == 3:                                   # replacement brought forward, funded 1,200,000
            gap = D(3000000) - mra_bal
            check("D15 Y4 membrane funding gap", q2(gap), D("1200000.00"))
            check("D15 Y4 injection restoring the reserve", q2(INJ - gap), D("300000.00"))
            mra_bal = INJ - gap
        from_cash = MRA if after >= MRA else max(D(0), after)
        from_block = MRA - from_cash
        mra_bal += MRA
        block -= from_block
        avail = after - from_cash
        ok = (rows[t]["cfads"] / DS >= DIST) and (fwd_leg[t + 1] / DS >= DIST)
        if ok:
            paid, block = avail + block, D(0)
        else:
            paid, block = D(0), block + avail
        dist.append(paid)
        if t == 2:
            check("D15 Y3 reserve top-up from the block account", q2(from_block),
                  D("406698.75"))
        check(f"D15 waterfall Y{t + 1} block-account balance", q2(block),
              [D("774364.77"), D("1128623.65"), D("721924.90"),
               D("1206931.59"), D("0.00"), D("0.00")][t])
    check("D15 Y5 distribution including the block release", q2(dist[4]), D("2077924.35"))
    check("D15 Y6 distribution", q2(dist[5]), D("885953.11"))
    check("D15 Y1-Y4 distributions are nil", q2(sum(dist[:4])), D("0.00"))

    # cost of the drought
    HB = D(8000000) / fvaf("0.03", 10)
    check("D15 handback charge, 10-year profile", q2(HB), D("697844.05"))
    pv_base6 = npv([CF - DS - MRA] * 6, DISC)
    pv_act6 = npv(dist, DISC)
    pv_inj = INJ * df(DISC, 4)
    check("D15 PV of base-case distributions Y1-Y6", q2(pv_base6), D("3579795.15"))
    check("D15 PV of actual distributions Y1-Y6", q2(pv_act6), D("1972501.14"))
    check("D15 PV of the injection", q2(pv_inj), D("1102544.78"))
    check("D15 PV of actual net of the injection", q2(pv_act6 - pv_inj), D("869956.36"))
    check("D15 loss of present value", q2(pv_base6 - (pv_act6 - pv_inj)), D("2709838.79"))
    check("D15 total cash cost to equity", q2(CF * 6 - total_actual + INJ), D("3182311.16"))
    check("D15 trading share of the cash cost",
          pct((CF * 6 - total_actual) / (CF * 6 - total_actual + INJ)), D("52.8644"))
    check("D15 capital-call share of the cash cost",
          pct(INJ / (CF * 6 - total_actual + INJ)), D("47.1356"))
    check("D15 distributions ultimately paid Y1-Y6", q2(sum(dist)), D("2963877.46"))

    # base-case equity profile, IRR and NPV
    # The 12 instalments of 5,009,635.23 amortise the loan to a 0.07 rounding residue that the
    # final instalment absorbs (Domain 3), so debt service runs years 1-12 and no further.
    eq_flows = [-EQ]
    for y in range(1, LIFE + 1):
        ds = DS if y <= TEN else D(0)
        eq_flows.append(CF - ds - (MRA if y <= 20 else D(0)) - (HB if y >= 16 else D(0)))
    check("D15 base equity flow Y1-Y12", q2(eq_flows[1]), D("774364.77"))
    check("D15 base equity flow Y13-Y15", q2(eq_flows[13]), D("5784000.00"))
    check("D15 base equity flow Y16-Y20", q2(eq_flows[16]), D("5086155.95"))
    check("D15 base equity flow Y21-Y25", q2(eq_flows[21]), D("5686155.95"))
    check("D15 Y13-Y25 cash yield %", pct((CF - MRA) / EQ), D("32.1333"))
    base_npv = sum(eq_flows[t] / (D(1) + DISC) ** t for t in range(len(eq_flows)))
    check("D15 base equity NPV at 8 %", q2(base_npv), D("5027733.03"))
    check("D15 base equity IRR %", pct(irr(eq_flows)), D("9.8591"))
    check("D15 total base distributions", q2(sum(eq_flows[1:])), D("80505936.71"))
    check("D15 base money multiple", q4(sum(eq_flows[1:]) / EQ), D("4.4726"))
    tail13 = sum(eq_flows[13:])
    check("D15 distributions in the last 13 years", q2(tail13), D("71213559.47"))
    check("D15 back-end share %", (tail13 / sum(eq_flows[1:]) * 100).quantize(D("0.01")),
          D("88.46"))
    check("D15 PV loss as % of base equity NPV",
          ((pv_base6 - (pv_act6 - pv_inj)) / base_npv * 100).quantize(D("0.01")), D("53.90"))
    check("D15 distributions received Y1-Y8", q2(sum(eq_flows[1:9])), D("6194918.16"))

    # ================================================================ 6. refinancing
    B5 = sched[5]["open"]
    SWAP_FIX, SWAP_MKT = D("0.0300"), D("0.0220")
    break_pv = D(0)
    for k in range(5, 12):
        break_pv += (SWAP_FIX - SWAP_MKT) * sched[k]["open"] / (D(1) + SWAP_MKT) ** (k - 4)
    check("D15 swap break cost", q2(break_pv), D("886064.34"))
    check("D15 break cost Y6 component",
          q2((SWAP_FIX - SWAP_MKT) * sched[5]["open"] / (D(1) + SWAP_MKT)), D("218909.55"))
    arr, ppf, adv = D("0.0100") * B5, D("0.0050") * B5, D(320000)
    check("D15 arrangement fee 1.00 %", q2(arr), D("279656.95"))
    check("D15 prepayment fee 0.50 %", q2(ppf), D("139828.47"))
    costs = break_pv + arr + ppf + adv
    check("D15 total transaction cost", q2(costs), D("1625549.77"))

    def refi(rate, tenor):
        inst = B5 / af(D(str(rate)), tenor)
        inc = [(DS if t <= 7 else D(0)) - (inst if t <= tenor else D(0))
               for t in range(1, max(7, tenor) + 1)]
        return inst, npv(inc, DISC)

    i7, pv7 = refi("0.0445", 7)
    i10, pv10 = refi("0.0445", 10)
    iT, pvT = refi("0.06", 10)
    check("D15 7-year refi instalment", q2(i7), D("4737139.41"))
    check("D15 7-year refi annual saving", q2(DS - i7), D("272495.82"))
    check("D15 7-year refi PV of incremental cash", q2(pv7), D("1418714.07"))
    check("D15 7-year refi net NPV", q2(pv7 - costs), D("-206835.69"))
    check("D15 7-year refi DSCR on base CFADS", q4(CF / i7), D("1.3476"))
    check("D15 10-year refi instalment", q2(i10), D("3525588.23"))
    check("D15 10-year refi saving years 6-12", q2(DS - i10), D("1484047.00"))
    check("D15 10-year refi PV of incremental cash", q2(pv10), D("2425030.89"))
    check("D15 10-year refi net NPV", q2(pv10 - costs), D("799481.12"))
    check("D15 10-year refi DSCR on base CFADS", q4(CF / i10), D("1.8108"))
    check("D15 tenor component measured alone", q2(pvT), D("586108.78"))
    check("D15 margin-tenor interaction", q2(pv10 - pvT - pv7), D("420208.04"))
    check("D15 decomposition sums to the total", q2(pv7 + pvT + (pv10 - pvT - pv7)), q2(pv10))
    check("D15 LLCR after the 10-year refinance", q4(CF * af(D("0.0445"), 10) / B5), D("1.8108"))
    check("D15 PLCR after the 10-year refinance", q4(CF * af(D("0.0445"), 20) / B5), D("2.9824"))
    check("D15 LLCR before refinancing (identity with DSCR)",
          q4(CF * af(RATE, 7) / B5), D("1.2743"))
    check("D15 PLCR before refinancing", q4(CF * af(RATE, 20) / B5), D("2.6184"))

    def refi_npv(rate, tenor=7):
        return refi(rate, tenor)[1] - costs

    def solve_rate(tenor, lo=D("0.0100"), hi=D("0.1500")):
        for _ in range(300):
            mid = (lo + hi) / 2
            if refi_npv(mid, tenor) > 0:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    be7 = solve_rate(7)
    check("D15 breakeven all-in rate, 7-year %", pct(be7), D("4.2206"))
    check("D15 breakeven margin over a 3.00 % base, bp",
          ((be7 - D("0.03")) * 10000).quantize(D("0.01")), D("122.06"))
    check("D15 required margin reduction, bp",
          ((D("0.06") - be7) * 10000).quantize(D("0.01")), D("177.94"))
    check("D15 shortfall against a 145 bp offer, bp",
          ((be7 - D("0.0445")) * -10000).quantize(D("0.01")), D("22.94"))
    check("D15 breakeven all-in rate, 10-year %", pct(solve_rate(10)), D("5.1308"))
    check("D15 post-refinance distributable", q2(CF - i10 - MRA), D("2258411.77"))
    check("D15 post-refinance cash yield %", pct((CF - i10 - MRA) / EQ), D("12.5467"))

    # cash sweep on the refinanced structure
    def refi_profile(sweep, charge_costs=True):
        fl, bal3, retire = [-EQ], DEBT, None
        for y in range(1, LIFE + 1):
            ds = D(0)
            if y <= 5:
                i = bal3 * RATE
                ds, bal3 = DS, bal3 - (DS - i)
            elif bal3 > D("0.005"):
                i = bal3 * D("0.0445")
                p = i10 - i
                if p >= bal3 - D("0.005"):
                    p = bal3
                ds, bal3 = i + p, bal3 - p
            avail = CF - ds - (MRA if y <= 20 else D(0)) - (HB if y >= 16 else D(0))
            pre = D(0)
            if bal3 > D("0.005") and y >= 6 and sweep > 0:
                pre = min(sweep * avail, bal3)
                bal3 -= pre
            if bal3 <= D("0.005") and retire is None:
                retire = y
            if y == 5 and charge_costs:
                avail -= costs
            fl.append(avail - pre)
        return fl, retire

    f0, r0 = refi_profile(D(0), False)
    f5, r5 = refi_profile(D("0.50"), False)
    n0 = sum(f0[t] / (D(1) + DISC) ** t for t in range(len(f0)))
    n5 = sum(f5[t] / (D(1) + DISC) ** t for t in range(len(f5)))
    check("D15 refinanced loan retires (no sweep)", r0, 15)
    check("D15 refinanced loan retires (50 % sweep)", r5, 13)
    check("D15 sweep cost to equity, PV", q2(n0 - n5), D("646327.86"))
    check("D15 sweep cost to equity, bp of IRR",
          ((irr(f0) - irr(f5)) * 10000).quantize(D("0.01")), D("39.76"))
    check("D15 swept annual distribution years 6-12", q2(f5[6]), D("1129205.89"))
    fc0, _ = refi_profile(D(0))
    fc5, _ = refi_profile(D("0.50"))
    nc0 = sum(fc0[t] / (D(1) + DISC) ** t for t in range(len(fc0)))
    nc5 = sum(fc5[t] / (D(1) + DISC) ** t for t in range(len(fc5)))
    check("D15 refi net of costs, no sweep, equity NPV", q2(nc0), D("5571846.45"))
    check("D15 refi net of costs, 50 % sweep, equity NPV", q2(nc5), D("4925518.59"))
    check("D15 refinancing gain at t0", q2(nc0 - base_npv), D("544113.42"))
    check("D15 value destroyed once the sweep is priced", q2(base_npv - nc5), D("102214.44"))
    lo, hi = D(0), D(1)
    for _ in range(200):
        mid = (lo + hi) / 2
        fm, _ = refi_profile(mid)
        if sum(fm[t] / (D(1) + DISC) ** t for t in range(len(fm))) > base_npv:
            lo = mid
        else:
            hi = mid
    check("D15 breakeven sweep share %", pct((lo + hi) / 2), D("40.3334"))

    # ================================================================ 7. waiver, cure, amendment
    cure2 = cov_cash - rows[1]["cfads"]
    cure3 = cov_cash - rows[2]["cfads"]
    check("D15 Y2 equity cure required", q2(cure2), D("47668.16"))
    check("D15 Y3 equity cure required", q2(cure3), D("808625.80"))
    check("D15 Y2 cure as % of the Y3 cure", pct(cure2 / cure3), D("5.8950"))
    check("D15 Y3 cure as a multiple of the Y2 cure", (cure3 / cure2).quantize(D("0.1")),
          D("17.0"))
    B2 = sched[2]["open"]
    check("D15 Y2 waiver fee at 15 bp", q2(D("0.0015") * B2), D("55307.03"))
    check("D15 Y2 amendment fee at 25 bp", q2(D("0.0025") * B2), D("92178.38"))
    uplift = sum(D("0.0040") * sched[k]["open"] / (D(1) + DISC) ** (k - 1) for k in range(2, 12))
    check("D15 PV of a 40 bp margin uplift, years 3-12", q2(uplift), D("651258.24"))
    check("D15 first-year uplift", q2(D("0.0040") * sched[2]["open"]), D("147485.41"))
    check("D15 last-year uplift", q2(D("0.0040") * sched[11]["open"]), D("18904.28"))
    amend = D("0.0025") * B2 + uplift
    check("D15 total amendment cost", q2(amend), D("743436.62"))
    check("D15 PV at end Y2 of the Y3 cure", q2(cure3 / (D(1) + DISC)), D("748727.59"))
    check("D15 amendment less the cure it replaces",
          q2(amend - cure3 / (D(1) + DISC)), D("-5290.97"))
    check("D15 waiver premium over the cure", q2(D("0.0015") * B2 - cure2), D("7638.87"))
    check("D15 waiver premium as % of the option preserved",
          pct((D("0.0015") * B2 - cure2) / cure3), D("0.9447"))

    # ================================================================ 8. restructuring
    B3, CREV = sched[3]["open"], rows[2]["cfads"]
    check("D15 restructuring DSCR on the existing schedule", q4(CREV / DS), D("1.0386"))
    sust = CREV / COV
    check("D15 sustainable service at 1.20x", q2(sust), D("4335780.40"))
    check("D15 annuity factor required", (B3 / sust).quantize(D("0.000001")), D("7.858792"))
    check("D15 AF(0.06,10) insufficient", 1 if af(RATE, 10) < B3 / sust else 0, 1)
    check("D15 AF(0.06,11) sufficient", 1 if af(RATE, 11) >= B3 / sust else 0, 1)

    def equity_pv(service, n_service, injection=D(0)):
        fl = [CREV - (service if 4 <= y < 4 + n_service else D(0))
              - (MRA if y <= 20 else D(0)) - (HB if y >= 16 else D(0))
              for y in range(4, LIFE + 1)]
        return npv(fl, DISC) - injection

    instA = B3 / af(RATE, 11)
    check("D15 option A instalment", q2(instA), D("4320342.23"))
    check("D15 option A restored DSCR", q4(CREV / instA), D("1.2043"))
    check("D15 option A recovery at 6.00 % is par", q2(npv([instA] * 11, RATE)), q2(B3))
    recA7 = npv([instA] * 11, "0.07")
    check("D15 option A recovery at 7.00 %", q2(recA7), D("32396839.39"))
    check("D15 option A recovery %", pct(recA7 / B3), D("95.0779"))
    eqA = equity_pv(instA, 11)
    check("D15 option A equity value", q2(eqA), D("14898548.64"))
    check("D15 option A annual equity cash", q2(CREV - instA - MRA), D("282594.25"))

    suppB = sust * af(RATE, 9)
    check("D15 option B supported debt", q2(suppB), D("29490644.05"))
    check("D15 option B haircut", q2(B3 - suppB), D("4583353.23"))
    check("D15 option B haircut %", pct((B3 - suppB) / B3), D("13.4512"))
    check("D15 option B recovery at 6.00 %", pct(npv([sust] * 9, RATE) / B3), D("86.5488"))
    recB7 = npv([sust] * 9, "0.07")
    check("D15 option B recovery at 7.00 %", q2(recB7), D("28248616.29"))
    check("D15 option B recovery %", pct(recB7 / B3), D("82.9037"))
    eqB = equity_pv(sust, 9)
    check("D15 option B equity value", q2(eqB), D("18656183.22"))
    check("D15 option B annual equity cash", q2(CREV - sust - MRA), D("267156.08"))

    injC = B3 - suppB
    instC = suppB / af(RATE, 9)
    check("D15 option C instalment equals the sustainable service", q2(instC), q2(sust))
    recC7 = injC + npv([instC] * 9, "0.07")
    check("D15 option C recovery at 7.00 %", q2(recC7), D("32831969.52"))
    check("D15 option C recovery %", pct(recC7 / B3), D("96.3549"))
    eqC = equity_pv(instC, 9, injC)
    check("D15 option C equity value net of the injection", q2(eqC), D("14072829.99"))
    check("D15 equity prefers extension over injection by", q2(eqA - eqC), D("825718.65"))
    check("D15 equity prefers the haircut over extension by", q2(eqB - eqA), D("3757634.58"))

    gross = D(4900000) * af(D("0.13"), 22)
    check("D15 enforcement gross enterprise value", q2(gross), D("35130615.34"))
    check("D15 enforcement costs at 6 %", q2(gross * D("0.06")), D("2107836.92"))
    net_e = gross * D("0.94")
    check("D15 enforcement net proceeds", q2(net_e), D("33022778.42"))
    floor = net_e / (D(1) + RATE)
    check("D15 enforcement floor value", q2(floor), D("31153564.55"))
    check("D15 enforcement floor recovery %", pct(floor / B3), D("91.4291"))
    check("D15 haircut shortfall against the floor, points",
          (pct(floor / B3) - pct(recB7 / B3)).quantize(D("0.0001")), D("8.5254"))
    check("D15 maximum acceptable haircut", q2(B3 - floor), D("2920432.73"))
    check("D15 maximum acceptable haircut %", pct((B3 - floor) / B3), D("8.5709"))
    check("D15 instalment the maximum haircut supports",
          q2(floor / af(RATE, 9)), D("4580266.69"))
    check("D15 DSCR the maximum haircut restores",
          q4(CREV / (floor / af(RATE, 9))), D("1.1359"))

    # ================================================================ 9. exit
    rem = eq_flows[9:]
    price = lambda r: npv(rem, r)
    for r, pr_x, ir, nv in (("0.065", D("39260418.77"), D("13.4196"), D("7661177.40")),
                            ("0.070", D("37541787.43"), D("12.8568"), D("6732654.36")),
                            ("0.075", D("35919098.27"), D("12.3059"), D("5855965.90")),
                            ("0.080", D("34386097.04"), D("11.7666"), D("5027733.03"))):
        pr = price(r)
        check(f"D15 exit price at {r}", q2(pr), pr_x)
        fl = eq_flows[:9][:]
        fl[8] = fl[8] + pr
        check(f"D15 seller IRR at {r} %", pct(irr(fl)), ir)
        check(f"D15 seller NPV at 8 % on a sale at {r}",
              q2(sum(fl[t] / (D(1) + DISC) ** t for t in range(9))), nv)
    fl80 = eq_flows[:9][:]
    fl80[8] = fl80[8] + price("0.080")
    check("D15 exit at the sponsor's own rate leaves NPV unchanged",
          q2(sum(fl80[t] / (D(1) + DISC) ** t for t in range(9))), q2(base_npv))
    check("D15 yield-compression value at end Y8",
          q2(price("0.075") - price("0.080")), D("1533001.23"))
    check("D15 yield-compression value at t0",
          q2((price("0.075") - price("0.080")) * df(DISC, 8)), D("828232.87"))
    check("D15 IRR uplift from crystallisation, points",
          (pct(irr(fl80)) - pct(irr(eq_flows))).quantize(D("0.0001")), D("1.9075"))
    check("D15 exit price as a multiple of equity subscribed",
          q4(price("0.075") / EQ), D("1.9955"))

    # ================================================================ 10. handback
    OBL = D(8000000)
    c_late, c_early = OBL / fvaf("0.03", 10), OBL / fvaf("0.03", 20)
    check("D15 handback charge, years 16-25", q2(c_late), D("697844.05"))
    check("D15 handback charge, years 6-25", q2(c_early), D("297725.66"))
    pv_late = c_late * af(DISC, 10) * df(DISC, 15)
    pv_early = c_early * af(DISC, 20) * df(DISC, 5)
    check("D15 present cost, 10-year profile", q2(pv_late), D("1476147.78"))
    check("D15 present cost, 20-year profile", q2(pv_early), D("1989422.56"))
    check("D15 early funding costs more by", q2(pv_early - pv_late), D("513274.78"))
    check("D15 early funding costs more, %", pct((pv_early - pv_late) / pv_late), D("34.7712"))
    check("D15 total contributed, 10-year", q2(c_late * 10), D("6978440.53"))
    check("D15 total contributed, 20-year", q2(c_early * 20), D("5954513.22"))
    check("D15 indifference cost when the reserve earns the discount rate",
          q2(OBL * df(DISC, 25)), D("1168143.24"))
    lo, hi = D("0.0001"), D("0.30")
    for _ in range(300):
        mid = (lo + hi) / 2
        d = (OBL / fvaf(mid, 20)) * af(DISC, 20) * df(DISC, 5) \
            - (OBL / fvaf(mid, 10)) * af(DISC, 10) * df(DISC, 15)
        lo, hi = (mid, hi) if d > 0 else (lo, mid)
    check("D15 indifference reserve rate equals the discount rate %",
          pct((lo + hi) / 2), pct(DISC))

    # ================================================================ 11. exercises
    DS1, CF1, T1 = D(7200000), D(9100000), D("0.75")
    check("EX 15.1 DSCR", q4(CF1 / DS1), D("1.2639"))
    check("EX 15.1 covenant trigger", q2(DS1 * D("1.20")), D("8640000.00"))
    check("EX 15.1 covenant headroom", q2(CF1 - DS1 * D("1.20")), D("460000.00"))
    check("EX 15.1 covenant headroom %", (((CF1 - DS1 * D("1.20")) / CF1) * 100)
          .quantize(D("0.0001")), D("5.0549"))
    check("EX 15.1 lock-up trigger", q2(DS1 * D("1.10")), D("7920000.00"))
    check("EX 15.1 lock-up headroom", q2(CF1 - DS1 * D("1.10")), D("1180000.00"))
    check("EX 15.1 lock-up headroom %", pct((CF1 - DS1 * D("1.10")) / CF1), D("12.9670"))
    check("EX 15.1 distribution trigger", q2(DS1 * D("1.30")), D("9360000.00"))
    check("EX 15.1 distribution shortfall", q2(CF1 - DS1 * D("1.30")), D("-260000.00"))
    check("EX 15.1 distribution shortfall %", pct((CF1 - DS1 * D("1.30")) / CF1), D("-2.8571"))
    check("EX 15.1 revenue headroom to covenant",
          q2((CF1 - DS1 * D("1.20")) / T1), D("613333.33"))
    check("EX 15.1 revenue shortfall on the distribution test",
          q2((DS1 * D("1.30") - CF1) / T1), D("346666.67"))
    check("EX 15.1 availability points to covenant",
          q4((CF1 - DS1 * D("1.20")) / T1 / D(120000)), D("5.1111"))
    check("EX 15.1 availability points to recover the distribution test",
          q4((DS1 * D("1.30") - CF1) / T1 / D(120000)), D("2.8889"))

    CF2, DS2, MRA2, FWD2 = D(8450000), D(7200000), D(450000), D(7650000)
    check("EX 15.2 after debt service", q2(CF2 - DS2), D("1250000.00"))
    check("EX 15.2 after the reserve top-up", q2(CF2 - DS2 - MRA2), D("800000.00"))
    check("EX 15.2 backward DSCR", q4(CF2 / DS2), D("1.1736"))
    check("EX 15.2 forward DSCR", q4(FWD2 / DS2), D("1.0625"))
    check("EX 15.2 distribution requirement", q2(DS2 * D("1.30")), D("9360000.00"))

    OUT3 = D(31500000)
    old3, new3 = OUT3 / af(D("0.0575"), 8), OUT3 / af(D("0.0455"), 8)
    check("EX 15.3 existing instalment", q2(old3), D("5022557.82"))
    check("EX 15.3 new instalment", q2(new3), D("4785464.53"))
    check("EX 15.3 annual saving", q2(old3 - new3), D("237093.28"))
    pvs3 = (old3 - new3) * af(DISC, 8)
    check("EX 15.3 PV of savings", q2(pvs3), D("1362489.49"))
    cst3 = D("0.0100") * OUT3 + D("0.0025") * OUT3 + D(260000) + D(640000)
    check("EX 15.3 arrangement fee", q2(D("0.0100") * OUT3), D("315000.00"))
    check("EX 15.3 prepayment fee", q2(D("0.0025") * OUT3), D("78750.00"))
    check("EX 15.3 total costs", q2(cst3), D("1293750.00"))
    check("EX 15.3 net NPV", q2(pvs3 - cst3), D("68739.49"))
    lo, hi = D("0.0100"), D("0.0575")
    for _ in range(300):
        mid = (lo + hi) / 2
        v = (old3 - OUT3 / af(mid, 8)) * af(DISC, 8) - cst3
        lo, hi = (mid, hi) if v > 0 else (lo, mid)
    be3 = (lo + hi) / 2
    check("EX 15.3 breakeven all-in rate %", pct(be3), D("4.6112"))
    check("EX 15.3 required margin reduction, bp",
          ((D("0.0575") - be3) * 10000).quantize(D("0.01")), D("113.88"))
    check("EX 15.3 cushion against a 120 bp offer, bp",
          (D(120) - (D("0.0575") - be3) * 10000).quantize(D("0.01")), D("6.12"))

    B4, CF4, R4 = D(24000000), D(3600000), D("0.055")
    sust4 = CF4 / D("1.25")
    check("EX 15.4 sustainable service", q2(sust4), D("2880000.00"))
    check("EX 15.4 annuity factor required", (B4 / sust4).quantize(D("0.000001")),
          D("8.333333"))
    check("EX 15.4 existing instalment", q2(B4 / af(R4, 7)), D("4223146.03"))
    check("EX 15.4 existing DSCR", q4(CF4 / (B4 / af(R4, 7))), D("0.8524"))
    check("EX 15.4 11-year instalment", q2(B4 / af(R4, 11)), D("2965695.68"))
    check("EX 15.4 11-year DSCR insufficient", q4(CF4 / (B4 / af(R4, 11))), D("1.2139"))
    check("EX 15.4 12-year instalment", q2(B4 / af(R4, 12)), D("2784701.55"))
    check("EX 15.4 12-year DSCR", q4(CF4 / (B4 / af(R4, 12))), D("1.2928"))
    supp4 = sust4 * af(R4, 7)
    check("EX 15.4 haircut supported debt", q2(supp4), D("16366945.30"))
    check("EX 15.4 haircut", q2(B4 - supp4), D("7633054.70"))
    check("EX 15.4 haircut %", pct((B4 - supp4) / B4), D("31.8044"))
    check("EX 15.4 haircut recovery %", pct(supp4 / B4), D("68.1956"))
    check("EX 15.4 maximum feasible haircut", q2(B4 * D("0.10")), D("2400000.00"))
    check("EX 15.4 instalment the maximum haircut supports",
          q2(B4 * D("0.90") / af(R4, 7)), D("3800831.42"))
    check("EX 15.4 DSCR the maximum haircut restores",
          q4(CF4 / (B4 * D("0.90") / af(R4, 7))), D("0.9472"))

    O5 = D(12000000)
    for years, start, charge, cost in ((10, 16, D("1071105.16"), D("2265706.06")),
                                       (15, 11, D("669197.47"), D("2653163.73")),
                                       (20, 6, D("469765.54"), D("3139004.45"))):
        ch = O5 / fvaf("0.025", years)
        check(f"EX 15.5 charge over {years} years", q2(ch), charge)
        check(f"EX 15.5 present cost over {years} years",
              q2(ch * af(DISC, years) * df(DISC, start - 1)), cost)
    check("EX 15.5 PV of the obligation at 8 %", q2(O5 * df(DISC, 25)), D("1752214.86"))
    check("EX 15.5 20-year total contributions", q2(O5 / fvaf("0.025", 20) * 20),
          D("9395310.90"))
    check("EX 15.5 10-year total contributions", q2(O5 / fvaf("0.025", 10) * 10),
          D("10711051.58"))
    check("EX 15.5 excess cost of the earliest profile",
          q2(D("3139004.45") - D("2265706.06")), D("873298.39"))

    # ================================================================ 12. Case study B
    OB, RES = D(42000000), D("0.03")
    ch_b = OB / fvaf(RES, 5)
    check("CASE B year-25 annual charge", q2(ch_b), D("7910892.00"))
    bal28 = ch_b * fvaf(RES, 3)
    check("CASE B reserve balance at year 28", q2(bal28), D("24451776.08"))
    grown = bal28 * (D(1) + RES) ** 2
    check("CASE B balance grown to year 30", q2(grown), D("25940889.24"))
    TRUE = D(58000000)
    check("CASE B understatement", q2(TRUE - OB), D("16000000.00"))
    check("CASE B understatement %", (((TRUE - OB) / OB) * 100).quantize(D("0.0001")),
          D("38.0952"))
    gap_b = TRUE - grown
    check("CASE B gap at year 30", q2(gap_b), D("32059110.76"))
    extra = gap_b / fvaf(RES, 2)
    check("CASE B extra annual contribution", q2(extra), D("15792665.40"))
    check("CASE B revised annual contribution", q2(ch_b + extra), D("23703557.40"))
    DISTB = D(21000000)
    check("CASE B distributable after the original charge", q2(DISTB - ch_b),
          D("13089108.00"))
    check("CASE B revised charge as % of distributable cash",
          pct((ch_b + extra) / DISTB), D("112.8741"))
    check("CASE B annual shortfall", q2(DISTB - ch_b - extra), D("-2703557.40"))
    check("CASE B sponsor injection over two years",
          q2((ch_b + extra - DISTB) * 2), D("5407114.79"))
    check("CASE B dividends forgone", q2((DISTB - ch_b) * 2), D("26178216.00"))
    check("CASE B additional contribution over two years", q2(extra * 2), D("31585330.80"))
    ch_b10 = TRUE / fvaf(RES, 10)
    check("CASE B charge had it been known at year 20", q2(ch_b10), D("5059369.38"))
    check("CASE B that charge as % of distributable", pct(ch_b10 / DISTB), D("24.0922"))
    check("CASE B retention at 5 %", q2(TRUE * D("0.05")), D("2900000.00"))
    check("D15 amendment less the Y2 cure", q2(amend - cure2), D("695768.46"))
    check("D15 exit price at 7.50 % as an equity multiple", q4(price("0.075") / EQ),
          D("1.9955"))
