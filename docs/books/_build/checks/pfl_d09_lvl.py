"""PFL-AI Domain 9 — golden checks for the Evaluation and Comprehension MCQs added to KA 9.1–9.4.

Scope: every number printed in MCQ 9.1-E, 9.1-F, 9.2-E, 9.3-E, 9.3-F and 9.4-E — in the options and
in the rationales. `pfl_d09.py` owns the domain's worked examples; this module owns only the added
items, so the two never touch the same file. Master-thread values come from ctx, never re-typed.

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
