"""PFL-AI Domain 9 — golden checks for the Evaluation and Comprehension MCQs added to KA 9.1–9.4.

Scope: every number printed in the twelve items added to KA 9.1–9.4 — 9.1-E/F/G, 9.2-E/F, 9.3-E/F/G/H
and 9.4-E/F/G — in the options and in the rationales. `pfl_d09.py` owns the domain's worked examples;
this module owns only the added items, so the two never touch the same file. Master-thread values come
from ctx, never re-typed.

The verifier found the first six covered and the second six not: 9.1-G, 9.2-F, 9.3-G, 9.3-H, 9.4-F and
9.4-G printed twenty-nine figures that no module recomputed. Section 5 closes that.

Engines used:

  1. The shareholder-loan shield (9.1.2) — interest x tax rate, discounted at the board's 8 % over the
     25-year operating life, and expressed as a share of the tranche.
  2. The `WACC` ladder and its closed form (9.1.3, 9.2.3) — `k_e(g) = F + ERP x beta_a(1 + (1-T)g/(1-g))`
     with `F = r_f + CRP + SP`, so `WACC(g) = (F + ERP beta_a) - g[(F + ERP beta_a) - k_d(1-T)
     - ERP beta_a (1-T)]`. The module derives the ladder from weights AND from the closed form and
     asserts the two agree, then evaluates it at the coverage-binding gearing.
  3. All-in effective cost (9.3.3) — the rate solving `net proceeds = (instalment + annual compliance
     cost) x AF(r, n)`, by deterministic bisection; separately the capacity effect of tenor at a fixed
     coverage target, so cost and capacity are never collapsed into one figure.
  4. Sustainability-linked breakeven (9.4.3) — ratchet and compliance cost as level annuities at the
     loan rate over the facility, plus the base-margin increment that turns the package positive.
  5. The six items the first pass left unchecked: the covenant- and lock-up-binding gearings and the
     price of the coverage constraint (9.1.4); negative arbitrage against commitment fees, levelised
     (9.2.4); the capitalised ECA premium solved from proceeds (9.3.2); and the concessional loan's
     grant element built period by period through grace and amortisation rather than quoted (9.4.2).
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))

    DEBT = ctx["KESTREL_DEBT"]                 # 42,000,000
    CAPEX = ctx["KESTREL_CAPEX"]               # 60,000,000
    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    RATE = ctx["KESTREL_RATE"]                 # 0.06
    N = ctx["KESTREL_TENOR"]                   # 12
    LIFE = ctx["KESTREL_LIFE"]                 # 25
    BOARD = ctx["KESTREL_DISCOUNT"]            # 0.08
    AF6_12 = D("8.383844")                     # the registry literal the manuscript divides by
    T = D("0.20")
    COV = D("1.20")

    check("D9-lvl AF(0.06,12) registry literal is the correct rounding",
          q(af(RATE, N), 6), AF6_12)

    # ================= MCQ 9.1-E — the shareholder-loan shield =================
    SL, SL_RATE = D(6000000), D("0.12")
    interest = SL * SL_RATE
    shield = interest * T
    AF8_25 = af(BOARD, LIFE)
    pv_shield = shield * AF8_25
    check("MCQ 9.1-E AF(0.08,25)", q(AF8_25, 6), D("10.674776"))
    check("MCQ 9.1-E shareholder-loan interest", interest, 720000)
    check("MCQ 9.1-E annual shield", shield, 144000)
    check("MCQ 9.1-E present value of the shield", q(pv_shield, 0), 1537168)
    check("MCQ 9.1-E shield as % of the tranche", q(pv_shield / SL * 100, 2), D("25.62"))

    # ================= MCQ 9.2-E — which structure's WACC =================
    RF, ERP, BETA_A, CRP, SP = D("4.10"), D("6.00"), D("0.60"), D("0.50"), D("0.50")
    KD = RATE * 100                            # 6.00 % pre-tax senior
    F = RF + CRP + SP
    check("MCQ 9.2-E non-systematic build-up F", F, D("5.10"))
    check("MCQ 9.2-E after-tax cost of senior debt %", KD * (1 - T), D("4.80"))

    def k_e(g):                                # re-levered per 9.1.3
        de = g / (1 - g)
        return F + ERP * (BETA_A * (1 + (1 - T) * de))

    def wacc_weights(g):                       # from the weights, as the definition requires
        return g * KD * (1 - T) + (1 - g) * k_e(g)

    def wacc_closed(g):                        # the 9.2.3 closed form
        return (F + ERP * BETA_A) - g * ((F + ERP * BETA_A) - KD * (1 - T) - ERP * BETA_A * (1 - T))

    check("MCQ 9.2-E closed-form intercept %", F + ERP * BETA_A, D("8.70"))
    check("MCQ 9.2-E closed-form slope % per unit of gearing",
          (F + ERP * BETA_A) - KD * (1 - T) - ERP * BETA_A * (1 - T), D("1.02"))
    for g, w in ((D("0.60"), "8.0880"), (D("0.70"), "7.9860"), (D("0.80"), "7.8840")):
        check(f"MCQ 9.2-E WACC at g={g} from weights", q(wacc_weights(g), 4), D(w))
        check(f"MCQ 9.2-E WACC at g={g} closed form", q(wacc_closed(g), 4), D(w))

    cap130 = CF / D("1.30") * AF6_12           # Domain 10's 1.30x capacity
    g_bind = cap130 / CAPEX
    check("MCQ 9.2-E 1.30x debt capacity", q(cap130, 0), 41171123)
    check("MCQ 9.2-E coverage-binding gearing %", q(g_bind * 100, 4), D("68.6185"))
    check("MCQ 9.2-E WACC at the coverage-binding gearing %",
          q(wacc_closed(g_bind), 4), D("8.0001"))
    check("MCQ 9.2-E range across the three quoted rates, basis points",
          q((wacc_closed(g_bind) - wacc_closed(D("0.80"))) * 100, 2), D("11.61"))
    # why the 80 % structure is not a candidate: the covenant fails on the base case
    ds80 = CAPEX * D("0.80") / af(RATE, N)     # 9.1.4's table is computed on the exact factor
    check("MCQ 9.2-E debt service at 80 % gearing", q(ds80, 2), D("5725297.41"))
    check("MCQ 9.2-E DSCR at 80 % gearing", q(CF / ds80, 4), D("1.1151"))
    check("MCQ 9.2-E CFADS the 1.20 covenant needs at 80 % gearing", q(ds80 * COV, 0), 6870357)
    check("MCQ 9.2-E 80 % structure fails on the base case",
          1 if ds80 * COV > CF else 0, 1)

    # ================= MCQ 9.3-E — the DFI tranche, cost and capacity =================
    DFI, DFI_RATE, DFI_N = D(12000000), D("0.0525"), 18
    FEE, ADVISORY, MONITOR = D("0.01"), D(350000), D(120000)
    AF_DFI = af(DFI_RATE, DFI_N)
    inst = DFI / AF_DFI
    proceeds = DFI - DFI * FEE - ADVISORY
    outflow = inst + MONITOR
    check("MCQ 9.3-E AF(0.0525,18)", q(AF_DFI, 6), D("11.464588"))
    check("MCQ 9.3-E DFI instalment", q(inst, 2), D("1046701.34"))
    check("MCQ 9.3-E front-end fee", DFI * FEE, 120000)
    check("MCQ 9.3-E net proceeds", proceeds, 11530000)
    check("MCQ 9.3-E annual outflow including monitoring", q(outflow, 2), D("1166701.34"))
    lo, hi = D("0.001"), D("0.30")             # bisection: outflow x AF(r,18) = proceeds
    for _ in range(300):
        mid = (lo + hi) / 2
        if outflow * af(mid, DFI_N) > proceeds:
            lo = mid
        else:
            hi = mid
    check("MCQ 9.3-E all-in economic cost %", q(lo * 100, 4), D("7.2465"))
    check("MCQ 9.3-E all-in above the 5.25 % headline, basis points",
          q((lo * 100 - DFI_RATE * 100) * 100, 0), 200)
    cap_dfi = CF / D("1.30") * AF_DFI
    check("MCQ 9.3-E capacity at 18 years, 5.25 %", q(cap_dfi, 0), 56299948)
    check("MCQ 9.3-E capacity uplift over the commercial tranche",   # difference of the two reported
          q(cap_dfi, 0) - q(cap130, 0), 15128825)                    # whole-unit figures

    # ================= MCQ 9.4-E — does the ratchet pay for itself? =================
    RATCHET, VERIFY = D("0.0015"), D(85000)
    benefit = DEBT * RATCHET
    pv_ben, pv_cost = benefit * AF6_12, VERIFY * AF6_12
    check("MCQ 9.4-E annual margin saving", benefit, 63000)
    check("MCQ 9.4-E PV of the margin saving", q(pv_ben, 0), 528182)
    check("MCQ 9.4-E PV of the verification cost", q(pv_cost, 0), 712627)
    check("MCQ 9.4-E net position on margin alone", q(pv_ben - pv_cost, 0), -184445)
    check("MCQ 9.4-E breakeven ratchet, basis points",
          q(VERIFY / DEBT * 10000, 2), D("20.24"))
    base10 = DEBT * D("0.0010")
    check("MCQ 9.4-E annual saving from 10 bp of base margin", base10, 42000)
    check("MCQ 9.4-E PV of a 10 bp base-margin reduction", q(base10 * AF6_12, 0), 352121)
    check("MCQ 9.4-E combined position with the base-margin reduction",
          q(base10 * AF6_12 + pv_ben - pv_cost, 0), 167677)

    # ============ MCQ 9.1-G — the marginal exchange rate, and where it stops applying ============
    # 9.1.4's table is computed on the exact annuity factor, so debt service is too.
    AF_EX = af(RATE, N)
    ds70, ds75 = DEBT / AF_EX, CAPEX * D("0.75") / AF_EX
    check("MCQ 9.1-G debt service at 70 % gearing", q(ds70, 2), D("5009635.23"))
    check("MCQ 9.1-G debt service at 75 % gearing", q(ds75, 2), D("5367466.32"))
    check("MCQ 9.1-G DSCR at 70 %", q(CF / ds70, 4), D("1.2743"))
    check("MCQ 9.1-G DSCR at 75 %", q(CF / ds75, 4), D("1.1894"))
    coverage_sold = (CF / ds70 - CF / ds75) * 100          # in hundredths of a ratio point
    check("MCQ 9.1-G coverage surrendered, hundredths", q(coverage_sold, 2), D("8.50"))
    irr_gain = (D("13.0193") - D("12.5311")) * 100         # from 9.1.4's equity IRR column
    check("MCQ 9.1-G equity IRR gain 70->75 %, basis points", q(irr_gain, 2), D("48.82"))
    check("MCQ 9.1-G basis points of return per hundredth of coverage",
          q(irr_gain / coverage_sold, 2), D("5.75"))
    check("MCQ 9.1-G CFADS the 1.20 covenant needs at 75 %", q(ds75 * COV, 0), 6440960)
    check("MCQ 9.1-G shortfall at 75 % before any stress", q(ds75 * COV - CF, 0), 56960)
    cap_cov = CF / COV * AF_EX
    g_cov = cap_cov / CAPEX
    check("MCQ 9.1-G debt the 1.20 covenant binds at", q(cap_cov, 0), 44602050)
    check("MCQ 9.1-G gearing the covenant binds at %", q(g_cov * 100, 4), D("74.3367"))
    check("MCQ 9.1-G WACC at the covenant-binding gearing %", q(wacc_closed(g_cov), 4), D("7.9418"))
    check("MCQ 9.1-G price of the coverage constraint against the 75 % ask, basis points",
          q((wacc_closed(g_bind) - wacc_closed(D("0.75"))) * 100, 2), D("6.51"))
    # the lock-up figures 9.1.4 quotes alongside, so the three binding points are one computation
    cap_lock = CF / D("1.15") * AF_EX
    check("MCQ 9.1-G debt the 1.15 lock-up binds at", q(cap_lock, 0), 46541269)
    check("MCQ 9.1-G gearing the lock-up binds at %", q(cap_lock / CAPEX * 100, 4), D("77.5688"))

    # ============ MCQ 9.2-F — drawing early against drawing progressively ============
    IDLE = DEBT / 2                            # even two-year spend, so half the issue is idle
    COUPON, DEPOSIT, COMMIT, YEARS = D("0.06"), D("0.03"), D("0.006"), 2
    neg_arb = IDLE * (COUPON - DEPOSIT) * YEARS
    commit = IDLE * COMMIT * YEARS
    check("MCQ 9.2-F negative arbitrage", neg_arb, 1260000)
    check("MCQ 9.2-F commitment fees on the bank alternative", commit, 252000)
    check("MCQ 9.2-F the bank route is cheaper by", neg_arb - commit, 1008000)
    check("MCQ 9.2-F negative arbitrage levelised over the facility, basis points",
          q(neg_arb / AF6_12 / DEBT * 10000, 2), D("35.78"))
    # the comparison the rationale draws: this one dimension against the whole WACC benefit of gearing
    check("MCQ 9.2-F WACC benefit of twenty points of gearing, basis points",
          q((wacc_closed(D("0.60")) - wacc_closed(D("0.80"))) * 100, 2), D("20.40"))

    # ============ MCQ 9.3-G — the cheap tranche that creates the exposure ============
    # The devaluation tolerance is Domain 11's (KA 11.3.2) and is rebuilt here from its own
    # decomposition rather than quoted, because 9.3-G's rationale prints it.
    HC_REV, HC_LOCAL_COST, HC_TAX, HC_WC = D(48000000), D(12600000), D(2064000), D(2400000)
    USD_COST, X0 = D(1350000), D(4)
    hc_num = HC_REV - HC_LOCAL_COST - HC_TAX - HC_WC
    check("MCQ 9.3-G local numerator in HC", hc_num, 30936000)
    check("MCQ 9.3-G the decomposition reproduces the master thread",
          q(hc_num / X0 - USD_COST, 2), CF)
    x_cov = hc_num / (COV * (DEBT / AF_EX) + USD_COST)
    check("MCQ 9.3-G unindexed covenant breakeven rate", q(x_cov, 6), D("4.202369"))
    check("MCQ 9.3-G devaluation that breaches with the whole local cost base offsetting %",
          q((x_cov / X0 - 1) * 100, 2), D("5.06"))

    # ============ MCQ 9.3-H — a premium capitalised into the loan ============
    ECA, ECA_RATE, ECA_N, PREMIUM = D(15000000), D("0.038"), 14, D("0.06")
    prem = ECA * PREMIUM
    gross = ECA + prem
    AF_ECA = af(ECA_RATE, ECA_N)
    inst_eca = gross / AF_ECA
    check("MCQ 9.3-H exposure premium", prem, 900000)
    check("MCQ 9.3-H grossed-up loan", gross, 15900000)
    check("MCQ 9.3-H AF(0.038,14)", q(AF_ECA, 6), D("10.703972"))
    check("MCQ 9.3-H instalment on the grossed-up loan", q(inst_eca, 2), D("1485429.84"))
    lo, hi = D("0.001"), D("0.30")             # proceeds are the 15,000,000, not the loan
    for _ in range(300):
        mid = (lo + hi) / 2
        if inst_eca * af(mid, ECA_N) > ECA:
            lo = mid
        else:
            hi = mid
    check("MCQ 9.3-H all-in cost solved from proceeds %", q(lo * 100, 4), D("4.6895"))
    check("MCQ 9.3-H above the 3.80 % headline, basis points",
          q((lo * 100 - ECA_RATE * 100) * 100, 2), D("88.95"))
    check("MCQ 9.3-H an arrangement fee deducted instead would cut proceeds, not raise the loan",
          1 if gross > ECA else 0, 1)

    # ============ MCQ 9.4-F — the grant element of a concessional tranche ============
    CONC, CONC_RATE, CONC_N, GRACE = D(12000000), D("0.02"), 25, 7
    grace_interest = CONC * CONC_RATE
    AF_CONC = af(CONC_RATE, CONC_N - GRACE)
    conc_inst = CONC / AF_CONC
    check("MCQ 9.4-F interest during grace", grace_interest, 240000)
    check("MCQ 9.4-F AF(0.02,18)", q(AF_CONC, 6), D("14.992031"))
    check("MCQ 9.4-F amortising instalment after grace", q(conc_inst, 2), D("800425.23"))
    pv_service = sum(grace_interest / (1 + RATE) ** t for t in range(1, GRACE + 1)) \
        + sum(conc_inst / (1 + RATE) ** t for t in range(GRACE + 1, CONC_N + 1))
    check("MCQ 9.4-F PV of debt service at the 6 % market rate", q(pv_service, 0), 7103613)
    check("MCQ 9.4-F grant element %", q((1 - pv_service / CONC) * 100, 2), D("40.80"))
    check("MCQ 9.4-F subsidy in present-value terms", q(CONC - pv_service, 0), 4896387)
    check("MCQ 9.4-F borrowing content, millions to one decimal",
          q(pv_service / 1000000, 1), D("7.1"))
    check("MCQ 9.4-F support content, millions to one decimal",
          q((CONC - pv_service) / 1000000, 1), D("4.9"))
    # option C's error: the undiscounted interest saved is a different and larger number
    undisc = sum(grace_interest for _ in range(GRACE)) \
        + sum(conc_inst for _ in range(CONC_N - GRACE)) - CONC
    check("MCQ 9.4-F undiscounted service above principal is not the grant element",
          1 if undisc != q(CONC - pv_service, 0) else 0, 1)

    # ============ MCQ 9.4-G — which face the support should strengthen ============
    # 9.4.2's G1 case: the grant displaces equity, so DSCR is untouched and equity IRR jumps.
    check("MCQ 9.4-G equity IRR uplift on the grant-displaces-equity case, basis points",
          q((D("16.8231") - D("12.5311")) * 100, 2), D("429.20"))
    check("MCQ 9.4-G DSCR is untouched on that case", q(CF / ds70, 4), D("1.2743"))
