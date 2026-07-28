"""PFL-AI Domain 9 — Funding structure and sources of capital: golden checks.

Every number printed as a result in `domain-09-funding-structure-capital.md` is recomputed here
from first principles. Master-thread values are read from ctx, never re-typed.

The domain's engine is the cost of capital, built up rather than assumed:

    k_d(after tax) = k_d (1 - T)                                       T derived, not assumed
    beta_e         = beta_a (1 + (1 - T) D/E)
    k_e            = F + beta_e * ERP           with F = r_f + CRP + SP
    WACC(g)        = g k_d(1-T) + (1-g) k_e  ==  a - b g
                     a = F + ERP*beta_a,  b = F + ERP*beta_a*T - k_d(1-T)

The closed form is itself checked against the point-by-point weighted average at five gearings,
because the manuscript's central claim rests on its slope. The cash tax rate of 20 % is *derived*
from the master thread (EBITDA - CFADS_before_WC = cash tax, on EBIT - interest) rather than
introduced, so it is checked before anything that depends on it.

All-in tranche costs are solved from the tranche's own cash-flow stream (never rate + fees), and
equity returns are solved as internal rates of return on the level-CFADS basis the manuscript
labels. Two local helpers — npv() and a bracketing bisection solve() — are defined here rather
than taken from ctx, which carries only af().
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]

    CAPEX = ctx["KESTREL_CAPEX"]
    DEBT = ctx["KESTREL_DEBT"]
    EQTY = ctx["KESTREL_EQUITY"]
    CF = ctx["KESTREL_CFADS"]
    INST = ctx["KESTREL_INSTALMENT"]
    EBITDA = ctx["KESTREL_EBITDA"]
    EBIT = ctx["KESTREL_EBIT"]
    INT1 = ctx["KESTREL_INTEREST_Y1"]
    LIFE = ctx["KESTREL_LIFE"]
    R = ctx["KESTREL_RATE"]
    N = ctx["KESTREL_TENOR"]
    BOARD = ctx["KESTREL_DISCOUNT"]
    NPV_BOARD = ctx["KESTREL_NPV"]
    APPRAISAL = D(8900000)          # D4 appraisal stream, 15 years (labelled in the manuscript)

    def npv(flows, r):
        return sum(c / (1 + r) ** t for t, c in enumerate(flows))

    def solve(fn, lo, hi, iters=200):
        """Bisection on a bracketed sign change; deterministic, no external solver."""
        lo, hi = D(lo), D(hi)
        sign_lo = fn(lo) > 0
        for _ in range(iters):
            mid = (lo + hi) / 2
            if (fn(mid) > 0) == sign_lo:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    def irr(flows):
        return solve(lambda r: npv(flows, r), "-0.9", "3")

    def allin(proceeds, payment, periods):
        """Effective rate solving  proceeds = payment * AF(r, periods)."""
        return solve(lambda r: D(proceeds) - D(payment) * af(r, periods), "0.0001", "0.60")

    # ---- annuity factors quoted in the manuscript ------------------------------------
    AF6_12 = af(R, N)
    TOL6 = D("0.0000005")
    check("D9 AF(6%,12)", AF6_12, "8.383844", TOL6)
    check("D9 AF(6%,6)", af(R, 6), "4.917324", TOL6)
    check("D9 AF(8%,25)", af(BOARD, LIFE), "10.674776", TOL6)
    check("D9 AF(4.75%,12)", af(D("0.0475"), 12), "8.989557", TOL6)
    check("D9 AF(11.5%,15)", af(D("0.115"), 15), "6.996708", TOL6)
    check("D9 AF(3.8%,14)", af(D("0.038"), 14), "10.703972", TOL6)
    check("D9 AF(5.25%,18)", af(D("0.0525"), 18), "11.464588", TOL6)
    check("D9 AF(5.7%,12)", af(D("0.057"), 12), "8.523470", TOL6)
    check("D9 AF(6.15%,12)", af(D("0.0615"), 12), "8.315327", TOL6)
    check("D9 AF(3.9%,14)", af(D("0.039"), 14), "10.633202", TOL6)
    check("D9 AF(6.3%,12)", af(D("0.063"), 12), "8.247657", TOL6)
    check("D9 AF(2%,18)", af(D("0.02"), 18), "14.992031", TOL6)
    check("D9 AF(6.5%,15)", af(D("0.065"), 15), "9.402669", TOL6)
    check("D9 AF(4.1%,12)", af(D("0.041"), 12), "9.330854", TOL6)
    check("D9 AF(6.2%,13)", af(D("0.062"), 13), "8.750167", TOL6)

    # ---- master thread: the cash tax rate is DERIVED (preamble) -----------------------
    CFADS_PRE_WC = D(6984000)
    cash_tax = EBITDA - CFADS_PRE_WC
    taxable = EBIT - INT1
    check("D9 derived cash tax", cash_tax, 516000)
    check("D9 derived taxable profit", taxable, 2580000)
    T = cash_tax / taxable
    check("D9 derived cash tax rate %", T * 100, "20.0")
    check("D9 depreciation reconciles", EBITDA - EBIT, 2400000)
    check("D9 year-one interest tax shield", INT1 * T, 504000)
    KDAT = R * 100 * (1 - T)
    check("D9 k_d after tax %", KDAT, "4.80")

    # ---- KA 9.1.3 cost of equity build-up --------------------------------------------
    RF, CRP, SP, ERP, BA = D("4.10"), D("0.50"), D("0.50"), D("6.00"), D("0.60")
    F = RF + CRP + SP
    check("WE 9.1.3 F = r_f + CRP + SP", F, "5.10")

    def beta_e(debt, equity):
        return BA * (1 + (1 - T) * (D(debt) / D(equity)))

    def k_e(debt, equity):
        return F + beta_e(debt, equity) * ERP

    check("WE 9.1.3 (1-T)D/E", (1 - T) * (DEBT / EQTY), "1.866667", D("0.0000005"))
    check("WE 9.1.3 beta_e", beta_e(DEBT, EQTY), "1.72")
    check("WE 9.1.3 beta_e * ERP", beta_e(DEBT, EQTY) * ERP, "10.32")
    check("WE 9.1.3 k_e %", k_e(DEBT, EQTY), "15.42")

    def wacc(debt, equity, grant=0, extra=()):
        """Weighted average over senior debt, equity, a zero-cost grant and extra tranches."""
        num = D(debt) * KDAT + D(equity) * k_e(D(debt) + sum(D(a) for a, _ in extra),
                                               D(equity)) + D(grant) * 0
        for amt, cost in extra:
            num += D(amt) * D(cost) * (1 - T)
        return num / CAPEX

    WACC70 = DEBT / CAPEX * KDAT + EQTY / CAPEX * k_e(DEBT, EQTY)
    check("WE 9.1.4 WACC at 70/30 %", WACC70, "7.9860")

    # ---- KA 9.2.3 closed form --------------------------------------------------------
    a = F + ERP * BA
    b = F + ERP * BA * T - KDAT
    check("9.2.3 closed-form intercept a %", a, "8.70")
    check("9.2.3 closed-form slope b %", b, "1.02")
    check("9.2.3 slope part: F - k_d(1-T)", F - KDAT, "0.30")
    check("9.2.3 slope part: ERP*beta_a*T", ERP * BA * T, "0.72")

    # ---- WE 9.1.4 the leverage ladder ------------------------------------------------
    ladder = [
        ("60", "13.02", "4293973.06", "1.4867", "2090026.94", "11.7685", "8.0880"),
        ("65", "14.0486", "4651804.15", "1.3724", "1732195.85", "12.1206", "8.0370"),
        ("70", "15.42", "5009635.23", "1.2743", "1374364.77", "12.5311", "7.9860"),
        ("75", "17.34", "5367466.32", "1.1894", "1016533.68", "13.0193", "7.9350"),
        ("80", "20.22", "5725297.41", "1.1151", "658702.59", "13.6146", "7.8840"),
    ]
    for g_pc, ke_p, ds_p, dscr_p, eqcf_p, irr_p, wacc_p in ladder:
        g = D(g_pc) / 100
        debt, equity = CAPEX * g, CAPEX * (1 - g)
        ds = debt / AF6_12
        eqcf = CF - ds
        w = g * KDAT + (1 - g) * k_e(debt, equity)
        check(f"WE 9.1.4 k_e at {g_pc}% gearing", k_e(debt, equity), ke_p)
        check(f"WE 9.1.4 debt service at {g_pc}%", ds, ds_p)
        check(f"WE 9.1.4 DSCR at {g_pc}%", CF / ds, dscr_p)
        check(f"WE 9.1.4 distributable at {g_pc}%", eqcf, eqcf_p)
        check(f"WE 9.1.4 equity IRR at {g_pc}%",
              irr([-equity] + [eqcf] * N + [CF] * (LIFE - N)) * 100, irr_p)
        check(f"WE 9.1.4 WACC at {g_pc}%", w, wacc_p)
        check(f"WE 9.1.4 closed form reproduces WACC at {g_pc}%", a - b * g, wacc_p)

    check("WE 9.1.4 equity IRR range 60->80 pp", D("13.6146") - D("11.7685"), "1.8461")
    check("WE 9.1.4 WACC range 60->80 pp", D("8.0880") - D("7.8840"), "0.2040")
    check("WE 9.1.4 DSCR range 60->80", D("1.4867") - D("1.1151"), "0.3716")
    check("WE 9.1.4 equity gain / WACC gain", D("1.8461") / D("0.2040"), "9.05")
    check("WE 9.1.4 marginal 70->75 equity IRR pp", D("13.0193") - D("12.5311"), "0.4882")
    check("WE 9.1.4 marginal 70->75 DSCR", D("1.2743") - D("1.1894"), "0.0850")
    check("WE 9.1.4 marginal 70->75 WACC pp", D("7.9860") - D("7.9350"), "0.0510")
    check("WE 9.1.4 bp equity IRR per 0.01 DSCR",
          (D("13.0193") - D("12.5311")) * 100 / ((D("1.2743") - D("1.1894")) * 100), "5.75")

    # ---- WE 9.1.4 the gearing at which each threshold binds --------------------------
    for thr, debt_p, gear_p in (("1.30", "41171122.86", "68.6185"),
                                ("1.20", "44602049.76", "74.3367"),
                                ("1.15", "46541269.32", "77.5688")):
        max_ds = CF / D(thr)
        max_debt = max_ds * AF6_12
        check(f"9.1.4 max debt at {thr}x", max_debt, debt_p)
        check(f"9.1.4 binding gearing at {thr}x %", max_debt / CAPEX * 100, gear_p)
    check("9.1.4 max debt service at 1.30x", CF / D("1.30"), "4910769.23")
    check("9.1.4 covenant cash needed at 75% gearing",
          (CAPEX * D("0.75") / AF6_12) * D("1.20"), "6440959.59")
    check("9.1.4 lock-up cash needed at 80% gearing",
          (CAPEX * D("0.80") / AF6_12) * D("1.15"), "6584092.02")

    # ---- 9.2.3 the reconciliation of Domain 4's 8.0 % --------------------------------
    bind_debt = (CF / D("1.30")) * AF6_12
    bind_equity = CAPEX - bind_debt
    g_bind = bind_debt / CAPEX
    check("9.2.3 equity at binding gearing", bind_equity, "18828877.14")
    check("9.2.3 beta_e at binding gearing", beta_e(bind_debt, bind_equity), "1.649565",
          D("0.0000005"))
    check("9.2.3 k_e at binding gearing %", k_e(bind_debt, bind_equity), "14.9974")
    check("9.2.3 WACC at binding gearing % (closed form)", a - b * g_bind, "8.0001")
    check("9.2.3 WACC at binding gearing % (weighted average)",
          g_bind * KDAT + (1 - g_bind) * k_e(bind_debt, bind_equity), "8.0001")
    check("9.2.3 WACC at 80% gearing %", a - b * D("0.80"), "7.8840")
    check("9.2.3 price of the coverage constraint pp",
          (a - b * g_bind) - (a - b * D("0.80")), "0.1161")

    # ---- 9.2.3 / executive perspective: the sponsor's corporate WACC -----------------
    BA_CORP = D("0.45")
    be_corp = BA_CORP * (1 + (1 - T) * (D(40) / D(60)))
    ke_corp = RF + be_corp * ERP
    kdat_corp = D("5.00") * (1 - T)
    wacc_corp = D("0.40") * kdat_corp + D("0.60") * ke_corp
    check("9.2.3 corporate beta_e", be_corp, "0.69")
    check("9.2.3 corporate k_d after tax %", kdat_corp, "4.00")
    check("9.2.3 corporate k_e %", ke_corp, "8.24")
    check("9.2.3 sponsor corporate WACC %", wacc_corp, "6.544")

    npv_board = APPRAISAL * af(BOARD, 15) - CAPEX
    npv_7986 = APPRAISAL * af(D("7.9860") / 100, 15) - CAPEX
    npv_corp = APPRAISAL * af(wacc_corp / 100, 15) - CAPEX
    check("9.2.3 NPV at the board's 8.00% ties to D4", npv_board, NPV_BOARD, D("0.5"))
    check("9.2.3 NPV at 7.9860%", npv_7986, "16244524.56")
    check("9.2.3 NPV uplift 8.00% -> 7.9860%", npv_7986 - npv_board, "65164.24")
    check("Exec NPV at corporate 6.544%", npv_corp, "23447721.82")
    check("Exec overstatement at corporate WACC", npv_corp - npv_board, "7268361.50")
    check("Exec overstatement %", (npv_corp - npv_board) / npv_board * 100, "44.92")

    # ---- WE 9.1.1 equity bridge loan (2-year construction, stated convention) --------
    base_stream = [-EQTY, D(0), D(0)] + [CF - INST] * N + [CF] * (LIFE - N)
    irr_base = irr(base_stream)
    check("WE 9.1.1 base equity IRR % (2-yr build)", irr_base * 100, "10.6696")
    ebl_cost = EQTY * ((1 + D("0.05")) ** 2 - 1)
    check("WE 9.1.1 EBL capitalised interest at 5.0%", ebl_cost, "1845000.00")
    check("WE 9.1.1 equity injection at t=2", EQTY + ebl_cost, "19845000.00")
    ebl_stream = [D(0), D(0), -(EQTY + ebl_cost)] + [CF - INST] * N + [CF] * (LIFE - N)
    irr_ebl = irr(ebl_stream)
    check("WE 9.1.1 equity IRR with EBL %", irr_ebl * 100, "11.6157")
    check("WE 9.1.1 EBL uplift pp", (irr_ebl - irr_base) * 100, "0.9461")

    def irr_at_bridge(rate):
        c = EQTY * ((1 + D(rate)) ** 2 - 1)
        return irr([D(0), D(0), -(EQTY + c)] + [CF - INST] * N + [CF] * (LIFE - N))
    indiff = solve(lambda x: irr_at_bridge(x) - irr_base, "0.01", "0.40")
    check("WE 9.1.1 EBL indifference rate = base equity IRR %", indiff * 100, "10.6696")

    # ---- WE 9.1.2 shareholder loan ---------------------------------------------------
    SHL, SHL_RATE = D(6000000), D("0.12")
    shl_int = SHL * SHL_RATE
    shl_shield = shl_int * T
    shl_pv = shl_shield * af(BOARD, LIFE)
    check("WE 9.1.2 shareholder-loan interest", shl_int, "720000.00")
    check("WE 9.1.2 annual tax shield", shl_shield, "144000.00")
    check("WE 9.1.2 PV of shield at 8% over 25 yr", shl_pv, "1537167.77")
    check("WE 9.1.2 PV as % of tranche", shl_pv / SHL * 100, "25.62")
    check("WE 9.1.2 DSCR if interest mis-ranked above CFADS", CF / (INST + shl_int), "1.1142")
    check("WE 9.1.2 cover from distributions", (CF - INST) / shl_int, "1.9088")

    # ---- 9.1.4 the sponsor-case escalation breakeven ---------------------------------
    def equity_irr_escalating(gr):
        gr = D(gr)
        flows = [-EQTY]
        for t in range(1, LIFE + 1):
            c = CF * (1 + gr) ** (t - 1)
            flows.append(c - INST if t <= N else c)
        return irr(flows)
    esc = solve(lambda x: equity_irr_escalating(x) - k_e(DEBT, EQTY) / 100, "0", "0.10")
    check("9.1.4 escalation breakeven for k_e 15.42% %", esc * 100, "1.7403")
    check("9.1.4 CFADS year 2 at breakeven escalation", CF * (1 + esc), "6495101.27")
    check("9.1.4 CFADS year 3 at breakeven escalation", CF * (1 + esc) ** 2, "6608136.04")
    check("9.1.4 DSCR year 12 at breakeven escalation", CF * (1 + esc) ** 11 / INST, "1.5407")

    # ---- WE 9.2.3 structures B and C -------------------------------------------------
    ds_b_sen = D(36000000) / af(D("0.057"), 12)
    ds_b_mez = D(9000000) / af(D("0.115"), 15)
    check("WE 9.2.3 B senior debt service", ds_b_sen, "4223631.91")
    check("WE 9.2.3 B mezzanine debt service", ds_b_mez, "1286319.25")
    check("WE 9.2.3 B senior-only DSCR", CF / ds_b_sen, "1.5115")
    check("WE 9.2.3 B total DSCR", CF / (ds_b_sen + ds_b_mez), "1.1586")
    check("WE 9.2.3 B k_e (D/E = 3.0) %", k_e(45000000, 15000000), "17.34")
    check("WE 9.2.3 B after-tax senior cost %", D("5.70") * (1 - T), "4.56")
    check("WE 9.2.3 B after-tax mezzanine cost %", D("11.50") * (1 - T), "9.20")
    wacc_b = (D(36000000) * D("5.70") * (1 - T) + D(9000000) * D("11.50") * (1 - T)
              + D(15000000) * k_e(45000000, 15000000)) / CAPEX
    check("WE 9.2.3 B blended WACC %", wacc_b, "8.4510")
    check("WE 9.2.3 B WACC increase over base pp", wacc_b - WACC70, "0.4650")
    check("WE 9.2.3 B equity CF years 1-12", CF - ds_b_sen - ds_b_mez, "874048.84")
    check("WE 9.2.3 B equity CF years 13-15", CF - ds_b_mez, "5097680.75")
    check("WE 9.2.3 B equity IRR %",
          irr([-D(15000000)] + [CF - ds_b_sen - ds_b_mez] * 12 + [CF - ds_b_mez] * 3
              + [CF] * 10) * 100, "12.0824")
    check("WE 9.2.3 B equity IRR loss vs base pp", D("12.5311") - D("12.0824"), "0.4487")

    ds_c_mez = D(6000000) / af(D("0.115"), 15)
    check("WE 9.2.3 C mezzanine debt service", ds_c_mez, "857546.17")
    check("WE 9.2.3 C total DSCR", CF / (INST + ds_c_mez), "1.0881")
    check("WE 9.2.3 C k_e (D/E = 4.0) %", k_e(48000000, 12000000), "20.22")
    wacc_c = (DEBT * KDAT + D(6000000) * D("11.50") * (1 - T)
              + D(12000000) * k_e(48000000, 12000000)) / CAPEX
    check("WE 9.2.3 C blended WACC %", wacc_c, "8.3240")
    check("WE 9.2.3 C WACC increase over base pp", wacc_c - WACC70, "0.3380")
    check("WE 9.2.3 C equity CF years 1-12", CF - INST - ds_c_mez, "516818.60")
    check("WE 9.2.3 C equity CF years 13-15", CF - ds_c_mez, "5526453.83")
    check("WE 9.2.3 C equity IRR %",
          irr([-D(12000000)] + [CF - INST - ds_c_mez] * 12 + [CF - ds_c_mez] * 3
              + [CF] * 10) * 100, "12.7378")
    check("WE 9.2.3 C equity IRR gain vs base pp", D("12.7378") - D("12.5311"), "0.2067")
    check("WE 9.2.3 B senior coverage gain vs base", D("1.5115") - D("1.2743"), "0.2372")

    # ---- WE 9.2.4 bond negative arbitrage --------------------------------------------
    idle = DEBT / 2
    neg_arb = idle * (D("0.06") - D("0.03")) * 2
    check("WE 9.2.4 average idle balance", idle, "21000000")
    check("WE 9.2.4 negative arbitrage over 2 years", neg_arb, "1260000.00")
    check("WE 9.2.4 negative arbitrage as bp per year", neg_arb / AF6_12 / DEBT * 10000, "35.78")
    check("WE 9.2.4 commitment fee equivalent", idle * D("0.006") * 2, "252000.00")
    check("WE 9.2.4 bank-route advantage", neg_arb - idle * D("0.006") * 2, "1008000.00")

    # ---- WE 9.3.1 ijara versus conventional, all-in ----------------------------------
    TR = D(21000000)
    conv_pay = TR / AF6_12
    conv_net = TR * (1 - D("0.012"))
    conv_allin = allin(conv_net, conv_pay, 12)
    check("WE 9.3.1 conventional instalment", conv_pay, "2504817.62")
    check("WE 9.3.1 conventional fee 1.20%", TR * D("0.012"), "252000.00")
    check("WE 9.3.1 conventional net proceeds", conv_net, "20748000.00")
    check("WE 9.3.1 conventional all-in %", conv_allin * 100, "6.2209")
    ij_pay = TR / af(D("0.0615"), 12)
    ij_net = TR * (1 - D("0.006"))
    ij_allin = allin(ij_net, ij_pay, 12)
    check("WE 9.3.1 ijara rental", ij_pay, "2525456.84")
    check("WE 9.3.1 ijara fee 0.60%", TR * D("0.006"), "126000.00")
    check("WE 9.3.1 ijara net proceeds", ij_net, "20874000.00")
    check("WE 9.3.1 ijara all-in %", ij_allin * 100, "6.2604")
    check("WE 9.3.1 all-in difference bp", (ij_allin - conv_allin) * 10000, "3.95")
    check("WE 9.3.1 annual payment difference", ij_pay - conv_pay, "20639.22")
    check("WE 9.3.1 PV of payment difference at 6%", (ij_pay - conv_pay) * AF6_12, "173035.98")

    # ---- WE 9.3.2 export credit ------------------------------------------------------
    ECA = D(15000000)
    eca_prem = ECA * D("0.06")
    eca_loan = ECA + eca_prem
    eca_inst = eca_loan / af(D("0.038"), 14)
    eca_allin = allin(ECA, eca_inst, 14)
    check("WE 9.3.2 exposure premium 6.0%", eca_prem, "900000.00")
    check("WE 9.3.2 grossed-up loan", eca_loan, "15900000.00")
    check("WE 9.3.2 ECA instalment", eca_inst, "1485429.84")
    check("WE 9.3.2 ECA all-in %", eca_allin * 100, "4.6895")
    check("WE 9.3.2 all-in uplift over headline pp", eca_allin * 100 - D("3.80"), "0.8895")
    comm_inst = ECA / AF6_12
    check("WE 9.3.2 commercial comparator instalment", comm_inst, "1789155.44")
    check("WE 9.3.2 annual debt-service saving", comm_inst - eca_inst, "303725.60")
    subst_ds = D(27000000) / AF6_12 + eca_inst
    check("WE 9.3.2 total debt service with ECA slice", subst_ds, "4705909.63")
    check("WE 9.3.2 DSCR with ECA slice", CF / subst_ds, "1.3566")
    check("WE 9.3.2 DSCR gain", CF / subst_ds - CF / INST, "0.0823")

    # ---- WE 9.3.3 development finance ------------------------------------------------
    DFI = D(12000000)
    dfi_inst = DFI / af(D("0.0525"), 18)
    dfi_net = DFI - DFI * D("0.01") - D(350000)
    dfi_outflow = dfi_inst + D(120000)
    dfi_allin = allin(dfi_net, dfi_outflow, 18)
    check("WE 9.3.3 DFI instalment", dfi_inst, "1046701.34")
    check("WE 9.3.3 DFI net proceeds", dfi_net, "11530000.00")
    check("WE 9.3.3 DFI annual outflow", dfi_outflow, "1166701.34")
    check("WE 9.3.3 DFI all-in economic cost %", dfi_allin * 100, "7.2465")
    check("WE 9.3.3 all-in uplift over headline pp", dfi_allin * 100 - D("5.25"), "1.9965")
    cap_dfi = (CF / D("1.30")) * af(D("0.0525"), 18)
    check("WE 9.3.3 capacity at 5.25%/18yr", cap_dfi, "56299947.60")
    check("WE 9.3.3 capacity uplift over 6%/12yr", cap_dfi - bind_debt, "15128824.74")
    check("WE 9.3.3 capacity uplift %", cap_dfi / bind_debt * 100 - 100, "36.75")

    # ---- WE 9.4.2 the grant, two ways ------------------------------------------------
    GRANT = D(6000000)
    wacc_g1 = (DEBT * KDAT + D(12000000) * k_e(DEBT, EQTY) + GRANT * 0) / CAPEX
    check("WE 9.4.2 G1 WACC % (grant in risk base)", wacc_g1, "6.4440")
    check("WE 9.4.2 G1 DSCR unchanged", CF / INST, "1.2743")
    irr_g1 = irr([-D(12000000)] + [CF - INST] * N + [CF] * (LIFE - N))
    check("WE 9.4.2 G1 equity IRR %", irr_g1 * 100, "16.8231")
    check("WE 9.4.2 G1 equity IRR gain bp", (irr_g1 * 100 - D("12.5311")) * 100, "429.20")
    check("WE 9.4.2 G1 alternative k_e (grant outside base) %", k_e(42, 12), "18.78")
    wacc_g1_alt = (DEBT * KDAT + D(12000000) * k_e(42, 12)) / CAPEX
    check("WE 9.4.2 G1 alternative WACC %", wacc_g1_alt, "7.1160")
    check("WE 9.4.2 treatment difference bp", (wacc_g1_alt - wacc_g1) * 100, "67.20")
    ds_g2 = D(36000000) / AF6_12
    check("WE 9.4.2 G2 senior debt service", ds_g2, "4293973.06")
    check("WE 9.4.2 G2 DSCR", CF / ds_g2, "1.4867")
    check("WE 9.4.2 G2 k_e %", k_e(36, 24), "13.02")
    wacc_g2 = (D(36000000) * KDAT + EQTY * k_e(36, 24)) / CAPEX
    check("WE 9.4.2 G2 WACC %", wacc_g2, "6.7860")
    irr_g2 = irr([-EQTY] + [CF - ds_g2] * N + [CF] * (LIFE - N))
    check("WE 9.4.2 G2 equity IRR %", irr_g2 * 100, "14.9940")
    check("WE 9.4.2 G2 equity IRR gain bp", (irr_g2 * 100 - D("12.5311")) * 100, "246.29")
    check("WE 9.4.2 G2 DSCR gain", CF / ds_g2 - CF / INST, "0.2124")

    # ---- 9.4.2 / 9.A.2 grant element of a concessional loan --------------------------
    CONC = D(12000000)
    grace_int = CONC * D("0.02")
    conc_inst = CONC / af(D("0.02"), 18)
    conc_pv = npv([D(0)] + [grace_int] * 7 + [conc_inst] * 18, D("0.06"))
    check("9.4.2 concessional interest during grace", grace_int, "240000.00")
    check("9.4.2 concessional instalment", conc_inst, "800425.23")
    check("9.4.2 PV of concessional service at 6%", conc_pv, "7103613.36")
    check("9.4.2 grant element %", (1 - conc_pv / CONC) * 100, "40.8032")
    check("9.4.2 grant-equivalent value", CONC - conc_pv, "4896386.64")

    # ---- WE 9.4.3 sustainability-linked ratchet --------------------------------------
    sll_sav = DEBT * D("0.0015")
    sll_cost = D(85000)
    sll_base = DEBT * D("0.0010")
    check("WE 9.4.3 ratchet value per year", sll_sav, "63000.00")
    check("WE 9.4.3 PV of margin saving", sll_sav * AF6_12, "528182.17")
    check("WE 9.4.3 PV of compliance cost", sll_cost * AF6_12, "712626.73")
    check("WE 9.4.3 net position", (sll_sav - sll_cost) * AF6_12, "-184444.57")
    check("WE 9.4.3 breakeven ratchet bp", sll_cost / DEBT * 10000, "20.24")
    check("WE 9.4.3 breakeven base-margin addition bp",
          (sll_cost - sll_sav) / DEBT * 10000, "5.24")
    check("WE 9.4.3 base-margin saving per year", sll_base, "42000.00")
    check("WE 9.4.3 PV of base-margin saving", sll_base * AF6_12, "352121.45")
    check("WE 9.4.3 combined net position",
          (sll_sav + sll_base - sll_cost) * AF6_12, "167676.88")

    # ---- WE 9.4.4 refinancing at the end of year 6 -----------------------------------
    bal = INST * af(R, 6)
    check("WE 9.4.4 outstanding balance after 6 payments", bal, "24634001.18")
    check("WE 9.4.4 principal repaid to date", DEBT - bal, "17365998.82")
    refi_cost = bal * D("0.0175")
    check("WE 9.4.4 transaction cost 1.75%", refi_cost, "431095.02")
    inst_rate_only = bal / af(D("0.0475"), 6)
    inst_extend = bal / af(D("0.0475"), 12)
    check("WE 9.4.4 rate-only instalment", inst_rate_only, "4814595.21")
    check("WE 9.4.4 rate-only annual saving", INST - inst_rate_only, "195040.02")
    check("WE 9.4.4 rate-only DSCR", CF / inst_rate_only, "1.3260")
    check("WE 9.4.4 extended instalment", inst_extend, "2740290.88")
    check("WE 9.4.4 extended annual saving years 7-12", INST - inst_extend, "2269344.35")
    check("WE 9.4.4 extended DSCR", CF / inst_extend, "2.3297")
    check("WE 9.4.4 new LLCR", CF * af(D("0.0475"), 12) / bal, "2.3297")
    check("WE 9.4.4 new PLCR (19 yr at 4.75%)", CF * af(D("0.0475"), 19) / bal, "3.1968")
    check("WE 9.4.4 old PLCR at t=6 (19 yr at 6%)", CF * af(R, 19) / bal, "2.8917")
    fl_rate = [-refi_cost] + [INST - inst_rate_only] * 6
    fl_ext = [-refi_cost] + [INST - inst_extend] * 6 + [-inst_extend] * 6
    KE = k_e(DEBT, EQTY) / 100
    check("WE 9.4.4 rate-only gain at k_e", npv(fl_rate, KE), "298756.99")
    check("WE 9.4.4 rate-only gain at 8%", npv(fl_rate, BOARD), "470551.52")
    check("WE 9.4.4 extended gain at k_e", npv(fl_ext, KE), "3723615.59")
    check("WE 9.4.4 extended gain at 8%", npv(fl_ext, BOARD), "2076799.98")
    check("WE 9.4.4 extended gain at the new loan rate", npv(fl_ext, D("0.0475")), "566832.30")
    check("WE 9.4.4 extension component at k_e",
          npv(fl_ext, KE) - npv(fl_rate, KE), "3424858.60")
    check("WE 9.4.4 extension component at 8%",
          npv(fl_ext, BOARD) - npv(fl_rate, BOARD), "1606248.46")
    check("WE 9.4.4 50% grantor gain share", npv(fl_ext, KE) / 2, "1861807.80")
    check("WE 9.4.4 gain shrinkage at loan rate %",
          100 - npv(fl_ext, D("0.0475")) / npv(fl_ext, KE) * 100, "84.78")
    check("WE 9.4.4 costs as % of gross rate-only benefit",
          refi_cost / (refi_cost + npv(fl_rate, KE)) * 100, "59.07")

    # ---- Case study A: closing the 828,877 gap ---------------------------------------
    check("Case A gap to the requested 42,000,000", DEBT - bind_debt, "828877.14")
    ds41 = D(41000000) / AF6_12
    check("Case A senior 41m debt service", ds41, "4890358.20")
    check("Case A senior 41m DSCR", CF / ds41, "1.3054")
    dist = CF - ds41
    check("Case A distributable", dist, "1493641.80")
    check("Case A option (i) k_e %", k_e(41, 19), "14.9147")
    check("Case A option (i) WACC %", (D(41000000) * KDAT + D(19000000) * k_e(41, 19)) / CAPEX,
          "8.0030")
    check("Case A option (i) equity IRR %",
          irr([-D(19000000)] + [dist] * N + [CF] * (LIFE - N)) * 100, "12.3868")
    mez4 = D(4000000) / af(D("0.115"), 15)
    check("Case A mezzanine debt service", mez4, "571697.45")
    check("Case A HoldCo cover in base case", dist / mez4, "2.6126")
    check("Case A option (ii) SPV total debt service", ds41 + mez4, "5462055.65")
    check("Case A option (ii) SPV total DSCR", CF / (ds41 + mez4), "1.1688")
    check("Case A option (iii) equity CF years 1-12", dist - mez4, "921944.35")
    check("Case A option (iii) equity CF years 13-15", CF - mez4, "5812302.55")
    check("Case A option (iii) equity IRR %",
          irr([-D(15000000)] + [dist - mez4] * 12 + [CF - mez4] * 3 + [CF] * 10) * 100,
          "12.4934")
    check("Case A option (iii) equity IRR gain over (i) bp",
          (D("12.4934") - D("12.3868")) * 100, "10.66")
    check("Case A option (iii) WACC %",
          (D(41000000) * KDAT + D(4000000) * D("11.50") * (1 - T)
           + D(15000000) * k_e(45, 15)) / CAPEX, "8.2283")
    lock = ds41 * D("1.15")
    check("Case A lock-up cash trigger", lock, "5623911.94")
    check("Case A lock-up shortfall %", (1 - lock / CF) * 100, "11.9061")
    cf115 = CF * D("0.885")
    check("Case A CFADS at 11.5% shortfall", cf115, "5649840.00")
    check("Case A DSCR at 11.5% shortfall", cf115 / ds41, "1.1553")
    check("Case A distributions at 11.5% shortfall", cf115 - ds41, "759481.80")
    check("Case A HoldCo cover at 11.5% shortfall", (cf115 - ds41) / mez4, "1.3285")
    cf12 = CF * D("0.88")
    check("Case A CFADS at 12% shortfall", cf12, "5617920.00")
    check("Case A headroom of the 11.5% case above the trigger", cf115 - lock, "25928.06")
    check("Case A cash lost between the two stresses", cf115 - cf12, "31920.00")

    # ---- Case study B: ECA sourcing versus open sourcing -----------------------------
    pN = D(120000000)
    pS = pN / D("1.08")
    afN, afS = af(D("0.039"), 14), af(D("0.063"), 12)
    check("Case B Supplier S price", pS, "111111111.11")
    check("Case B equipment premium", pN - pS, "8888888.89")
    ecaB = pN * D("0.85")
    premB = ecaB * D("0.065")
    instN = (ecaB + premB) / afN
    check("Case B ECA loan 85%", ecaB, "102000000.00")
    check("Case B exposure premium 6.5%", premB, "6630000.00")
    check("Case B financed loan", ecaB + premB, "108630000.00")
    check("Case B route N instalment", instN, "10216113.96")
    check("Case B route N all-in %", allin(ecaB, instN, 14) * 100, "4.8656")
    comB = pS * D("0.85")
    feeB = comB * D("0.011")
    instS = comB / afS
    check("Case B commercial loan 85%", comB, "94444444.44")
    check("Case B arrangement fee 1.10%", feeB, "1038888.89")
    check("Case B commercial proceeds", comB - feeB, "93405555.56")
    check("Case B route S instalment", instS, "11451063.27")
    check("Case B route S all-in %", allin(comB - feeB, instS, 12) * 100, "6.5041")
    check("Case B all-in difference bp (manuscript rounds to 164)",
          (allin(comB - feeB, instS, 12) - allin(ecaB, instN, 14)) * 10000, "163.84")
    check("Case B annual debt-service difference", instS - instN, "1234949.31")
    pvN = npv([pN - ecaB] + [instN] * 14, BOARD)
    pvS = npv([pS - (comB - feeB)] + [instS] * 12, BOARD)
    check("Case B route N own funds at close", pN - ecaB, "18000000.00")
    check("Case B route S own funds at close", pS - (comB - feeB), "17705555.56")
    check("Case B PV route N at 8%", pvN, "102224064.52")
    check("Case B PV route S at 8%", pvS, "104001661.74")
    check("Case B route N advantage", pvS - pvN, "1777597.22")

    def pv_pair(prem_pc, disc):
        prem_pc, disc = D(prem_pc), D(disc)
        s = pN / (1 + prem_pc)
        c = s * D("0.85")
        f = c * D("0.011")
        return (npv([pN - ecaB] + [instN] * 14, disc),
                npv([s - (c - f)] + [c / afS] * 12, disc))
    for disc, want in (("0.08", "9.8780"), ("0.06", "8.7179"), ("0.10", "10.8612")):
        be = solve(lambda p, d=disc: pv_pair(p, d)[0] - pv_pair(p, d)[1], "0.001", "0.30")
        check(f"Case B breakeven equipment premium at {disc} %", be * 100, want)
    inst9 = (ecaB * D("1.09")) / afN
    pv9 = npv([pN - ecaB] + [inst9] * 14, BOARD)
    check("Case B loan at 9.0% exposure premium", ecaB * D("1.09"), "111180000.00")
    check("Case B instalment at 9.0% premium", inst9, "10455928.84")
    check("Case B all-in at 9.0% premium %", allin(ecaB, inst9, 14) * 100, "5.2291")
    check("Case B PV route N at 9.0% premium", pv9, "104201155.24")
    check("Case B route N now worse by", pv9 - pvS, "199493.50")
    flip = solve(lambda x: npv([pN - ecaB] + [(ecaB * (1 + D(x))) / afN] * 14, BOARD) - pvS,
                 "0.001", "0.40")
    check("Case B flip exposure premium %", flip * 100, "8.7477")

    # ---- Calculation exercises 9.1 - 9.5 ---------------------------------------------
    be1 = D("0.55") * (1 + D("0.75") * (D(105) / D(45)))
    ke1 = D("3.80") + be1 * D("5.5") + D("1.20") + D("0.40")
    check("Ex 9.1 (1-T)D/E", D("0.75") * (D(105) / D(45)), "1.75")
    check("Ex 9.1 beta_e", be1, "1.5125")
    check("Ex 9.1 beta_e * ERP", be1 * D("5.5"), "8.31875")
    check("Ex 9.1 k_e %", ke1, "13.71875")
    check("Ex 9.1 k_d after tax %", D("5.5") * D("0.75"), "4.125")
    check("Ex 9.1 WACC %", D("0.70") * D("4.125") + D("0.30") * ke1, "7.0031")
    check("Ex 9.1 error: pre-tax k_d %", D("0.70") * D("5.5") + D("0.30") * ke1, "7.9656")
    ke1_err = D("3.80") + D("0.55") * D("5.5") + D("1.60")
    check("Ex 9.1 error: unlevered beta k_e %", ke1_err, "8.4250")
    check("Ex 9.1 error: unlevered beta WACC %",
          D("0.70") * D("4.125") + D("0.30") * ke1_err, "5.4150")
    af65 = af(D("0.065"), 15)
    for g_pc, ds_p, dscr_p, eq_p, irr_p in (("65", "6912930.89", "2.0252", "7087069.11",
                                             "20.7836"),
                                            ("75", "7976458.72", "1.7552", "6023541.28",
                                             "24.8151")):
        g = D(g_pc) / 100
        debt = D(100000000) * g
        ds = debt / af65
        check(f"Ex 9.2 debt service at {g_pc}%", ds, ds_p)
        check(f"Ex 9.2 DSCR at {g_pc}%", D(14000000) / ds, dscr_p)
        check(f"Ex 9.2 distributable at {g_pc}%", D(14000000) - ds, eq_p)
        check(f"Ex 9.2 equity IRR at {g_pc}%",
              irr([-(D(100000000) - debt)] + [D(14000000) - ds] * 15
                  + [D(14000000)] * 7) * 100, irr_p)
    check("Ex 9.2 equity IRR gain bp", (D("24.8151") - D("20.7836")) * 100, "403.15")
    check("Ex 9.2 DSCR surrendered", D("2.0252") - D("1.7552"), "0.2700")
    check("Ex 9.3 after-tax senior %", D("5.2") * D("0.7"), "3.64")
    check("Ex 9.3 after-tax mezzanine %", D("10.8") * D("0.7"), "7.56")
    check("Ex 9.3 WACC %",
          (D(80) * D("3.64") + D(20) * D("7.56") + D(50) * D("14.5")) / D(150), "7.7827")
    check("Ex 9.3 error: no tax shield %",
          (D(80) * D("5.2") + D(20) * D("10.8") + D(50) * D("14.5")) / D(150), "9.0467")
    check("Ex 9.3 error: taxing equity %",
          (D(80) * D("3.64") + D(20) * D("7.56")
           + D(50) * D("14.5") * D("0.7")) / D(150), "6.3327")
    check("Ex 9.3 error: simple average %",
          (D("3.64") + D("7.56") + D("14.5")) / 3, "8.5667")
    ex4_prem = D(40000000) * D("0.055")
    ex4_inst = (D(40000000) + ex4_prem) / af(D("0.041"), 12)
    ex4_allin = allin(D(40000000), ex4_inst, 12)
    check("Ex 9.4 premium", ex4_prem, "2200000.00")
    check("Ex 9.4 grossed-up loan", D(40000000) + ex4_prem, "42200000.00")
    check("Ex 9.4 instalment", ex4_inst, "4522630.17")
    check("Ex 9.4 all-in %", ex4_allin * 100, "5.0378")
    check("Ex 9.4 uplift bp", (ex4_allin * 100 - D("4.10")) * 100, "93.78")
    check("Ex 9.4 error: premium spread straight-line %", D("4.10") + D("5.5") / 12, "4.5583")
    af62 = af(D("0.062"), 13)
    check("Ex 9.5 k_d after tax %", D("6.2") * D("0.75"), "4.650")
    check("Ex 9.5 (a) debt service", D(84000000) / af62, "9599817.02")
    check("Ex 9.5 (a) DSCR", D(13000000) / (D(84000000) / af62), "1.3542")
    check("Ex 9.5 (a) WACC %",
          (D(84000000) * D("4.65") + D(18000000) * D("16.50")) / D(120000000), "5.7300")
    check("Ex 9.5 (b) debt service", D(66000000) / af62, "7542713.37")
    check("Ex 9.5 (b) DSCR", D(13000000) / (D(66000000) / af62), "1.7235")
    check("Ex 9.5 (b) WACC %",
          (D(66000000) * D("4.65") + D(36000000) * D("13.50")) / D(120000000), "6.6075")
