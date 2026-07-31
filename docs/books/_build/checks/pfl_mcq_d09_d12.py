"""PFL-AI Domains 9-12 — golden checks for the third cognitive-level batch.

Scope: the twenty-three sample MCQs added to KA 9.1-9.4, 10.1/10.3/10.4, 11.1-11.4 and 12.1-12.4 in
this batch:

  D9   9.1-G  9.2-F  9.3-G  9.3-H  9.4-F  9.4-G
  D10  10.1-H  10.3-G  10.3-H  10.4-G  10.4-H
  D11  11.1-E  11.1-F  11.2-F  11.3-G  11.4-F  11.4-G
  D12  12.1-G  12.2-F  12.2-G  12.3-E  12.3-F  12.4-G

`pfl_d09.py`/`pfl_d10_ext.py`/`pfl_d11.py`/`pfl_d12.py` own each domain's worked examples and
`pfl_d0N_lvl.py`/`pfl_d1N_lvl.py` own the earlier level batches, so no file is edited twice. Every
number printed in these stems, options and rationales is rebuilt from first principles below —
distractors included, because a numeric distractor whose arithmetic is wrong is one a candidate
eliminates without reasoning. Master-thread values come from ctx and are never re-typed.

Engines rebuilt here rather than quoted:

  1. **The `WACC` line (9.1.3/9.1.4/9.2.3).** `WACC(g)` is built from the re-levered beta —
     `β_e = β_a(1 + (1 − T)g/(1 − g))`, `k_e = r_f + β_e·ERP + CRP + SP`, `WACC = g·k_d(1 − T) +
     (1 − g)k_e` — and then *proved* to collapse to the closed form `8.70 − 1.02g` that 9.1-G's
     three gearing/`WACC` pairs and its 6.51 bp constraint price all depend on. The collapse is the
     reason a marginal exchange rate can be quoted at all, so it is checked as an identity, not
     assumed.
  2. **Coverage-bounded gearing (9.1.4).** Debt capacity `= (CFADS/λ)·AF(r, n)` at λ = 1.30 and
     1.20, converted to gearing on capex; and the reverse test at 75 % gearing, where the 1.20×
     requirement of 6,440,960 is shown to exceed the 6,384,000 available by 56,960.
  3. **Negative arbitrage levelised (9.2.4).** 1,260,000 spread over the facility by the loan-rate
     annuity factor, expressed on the drawn amount — the 35.78 bp of 9.2-F.
  4. **Capitalised premium (9.3.2).** The grossed-up loan, its instalment on `AF(0.038, 14)`, and
     the all-in rate solved (bisection) from *proceeds* against that instalment: 4.6895 %, 88.95 bp
     above the headline. This is the arithmetic 9.3-H's key describes in words.
  5. **Grant element (9.4.2).** The concessional stream built period by period — seven years of
     interest-only at 2.0 %, eighteen level instalments on `AF(0.02, 18)` — discounted at the 6.0 %
     market rate, giving 9.4-F's 40.80 % / 7.1m / 4.9m split.
  6. **Currency decomposition (11.3.2).** The `HC` numerator 30,936,000 is re-derived from the
     tariff, local costs, tax and working capital, checked against the master thread at the closing
     rate, and then solved for the devaluation that breaches the covenant (5.06 %) and for the
     indexed share that lifts that tolerance to 37.17 % — the two figures 9.3-G's rationale turns
     on.
  7. **Tenor as a capacity lever (10.1.2).** Capacity at 13 years against 12, and the three rival
     resolutions 10.1-H prices: the `CFADS` uplift (128,526 / 2.0132 %), the ratio the request
     delivers (1.2743) and the year-one coverage the thirteenth year produces (1.3456).
  8. **Reserve tolerance ladder (10.3.2).** `tolerance(m) = 1 − DS(1 − m/12)/CFADS`, proved linear
     in `m` so that "6.5393 points and 417,470 per month" is an identity rather than a fitted
     figure; then the m = 6/9/12 endpoints and the 63,840-per-point exchange rate of 10.3-H.
  9. **Reserve funding routes (10.3.2b).** LC fee against the equity carry on the *increment*, and
     the 12.42 % breakeven — 10.3-H's option C.
 10. **Graduated thresholds (10.4.2).** The instalment times 1.25/1.20/1.15, which is 10.4-H's
     three-consequence ladder in cash.
 11. **Register pricing (11.1.3).** The eight-item register rebuilt from its own `p` and impact
     columns at the stated 40 % loading, so that 11.1-E's bundle premium (6,748,000), bundle cost
     (6,598,000), full-retention cost (7,110,000) and unbundled cost (4,818,000) are derived, not
     copied — and the identity that the 512,000 by which the bundle beats retention is exactly
     2,292,000 − 1,780,000 is checked, because it is what makes option A true and still wrong.
 12. **Hedge ratio (11.3.1).** Debt service = fixed principal + outstanding × all-in rate, so
     `DSCR` across the ±200 bp range, the covenant and payment-failure breakevens in *reference
     rate* terms (+73.90 / +327.23 bp — the base-crossing that 11.3-G's option A had to be
     corrected for), the minimum hedge ratio and the 20.92 range-per-coverage exchange rate.
 13. **Operating register (11.4.1/11.4.2/11.4.4).** Bernoulli aggregation (mean = ΣEMV, variance =
     Σp(1 − p)impact², P80 = mean + 0.8416σ), the uniform-correlation variance form, the lender's
     1.5×/1.4× re-cut, and the covenant-preserving ceiling headroom × `AF(0.06, 12)` — 11.4-F's
     881,189 gap and 4,801,313 of surrendered capacity.
 14. **Three make-good bases (12.1.3).** Value, sized-coverage and bare-covenant buy-downs, plus
     the application-clause arithmetic (relief × `AF(0.08, 12)`) that produces 12.1-G's 521,045.
 15. **Amortisation to year five (12.2.3).** The balance rolled forward period by period, and the
     unreturned equity 18,000,000 − 5 × distribution — 12.2-G's 11,128,176.
 16. **Cover = face × credit (12.3.2).** The 5,500,000 offer risk-adjusted at 0.70 against the
     4,800,000 bond, the 950,000 shortfall, the equivalent face and the bond fee — and the
     breakeven probability (0.8727) that shows why a 700,000 higher face does not close the gap.

Two things this module records rather than checks:

  * Worked example 12.1.3 prints the bare-covenant buy-down as 562,851.03. Rebuilt from
     `(CFADS(0.95)/1.20) × AF(0.06, 12)` it is 562,851.32. The 29-cent difference is the worked
     example's own rounding of 5,931,000 and belongs to `pfl_d12.py`; MCQ 12.1-G prints the integer
     562,851, which is right on either convention, and the 8.591× spread is checked on the
     recomputed figure.
  * Worked example 11.4.1 prints the ρ = 0.30 P80 as 4,841,128 against a rebuilt 4,841,127.6.
     MCQ 11.4-F prints only the *uplift* (837,478), which is checked here; the level itself belongs
     to `pfl_d11.py`.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: D(str(x)).quantize(D(1).scaleb(-n))

    DEBT = ctx["KESTREL_DEBT"]                  # 42,000,000
    EQUITY = ctx["KESTREL_EQUITY"]              # 18,000,000
    CAPEX = ctx["KESTREL_CAPEX"]                # 60,000,000
    RATE = ctx["KESTREL_RATE"]                  # 0.06
    TENOR = ctx["KESTREL_TENOR"]                # 12
    LIFE = ctx["KESTREL_LIFE"]                  # 25
    INST = ctx["KESTREL_INSTALMENT"]            # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                   # 6,384,000
    DISC = ctx["KESTREL_DISCOUNT"]              # 0.08

    AF612 = af(RATE, TENOR)
    AF613 = af(RATE, 13)
    AF825 = af(DISC, LIFE)
    AF812 = af(DISC, TENOR)
    COV = D("1.20")
    SIZING = D("1.30")
    LOCKUP = D("1.15")
    DISTCOND = D("1.25")
    TAXR = D("0.20")
    Z80 = D("0.8416")

    # ---- master-thread ties every item below leans on -------------------------------------------
    check("AF(0.06,12) to fifteen places", q(AF612, 15), D("8.383843940383319"))
    check("AF(0.06,13) to fifteen places", q(AF613, 15), D("8.852682962625773"))
    check("AF(0.08,25) to fifteen places", q(AF825, 15), D("10.674776188588579"))
    check("invariant: instalment is debt over the annuity factor", q(DEBT / AF612, 2), INST)
    check("base DSCR 1.2743 (printed by 9.1-G, 10.1-H, 11.3-G, 12.1-G)",
          q(CF / INST, 4), D("1.2743"))
    HEADROOM = CF - COV * INST
    check("annual covenant headroom 372,437.72 (printed by 11.4-F and 12.2-F)",
          q(HEADROOM, 2), D("372437.72"))
    check("MCQ 12.2-F headroom rounded as printed in option D", q(HEADROOM, 0), 372438)

    # ============================== D9 KA 9.1 — MCQ 9.1-G ========================================
    # The WACC line, built from the re-levered beta rather than quoted.
    BETA_A, RF, ERP, CRP, SP = D("0.60"), D("4.10"), D(6), D("0.50"), D("0.50")
    KD = D("6.00")

    def ke(g):
        beta_e = BETA_A * (1 + (1 - TAXR) * g / (1 - g))
        return RF + beta_e * ERP + CRP + SP

    def wacc(g):
        return g * KD * (1 - TAXR) + (1 - g) * ke(g)

    check("9.1.3 k_e at 70 % gearing (the 15.42 % the whole domain uses)",
          q(ke(D("0.70")), 2), D("15.42"))
    # The closed form is what makes a marginal exchange rate quotable — prove it, do not assume it.
    for g in ("0.60", "0.686185", "0.70", "0.743367", "0.75", "0.80"):
        check(f"MCQ 9.1-G invariant: WACC({g}) collapses to 8.70 - 1.02g",
              q(wacc(D(g)), 10), q(D("8.70") - D("1.02") * D(g), 10))
    check("MCQ 9.1-G WACC at the 1.30x sizing gearing (8.0001 %)",
          q(wacc(D("0.686185")), 4), D("8.0001"))
    check("MCQ 9.1-G WACC at the 1.20x covenant gearing (7.9418 %)",
          q(wacc(D("0.743367")), 4), D("7.9418"))
    check("MCQ 9.1-G WACC at the sponsors' 75 % (7.9350 %, corpus 9.1.4)",
          q(wacc(D("0.75")), 4), D("7.9350"))
    check("MCQ 9.1-G option C: price of the coverage constraint, 6.51 bp of WACC",
          q((wacc(D("0.686185")) - wacc(D("0.75"))) * 100, 2), D("6.51"))

    CAP130 = (CF / SIZING) * AF612
    CAP120 = (CF / COV) * AF612
    check("MCQ 9.1-G / 10.1-H 1.30x debt capacity", q(CAP130, 0), 41171123)
    check("MCQ 9.1-G 1.30x gearing on capex (68.6185 %)", q(CAP130 / CAPEX * 100, 4), D("68.6185"))
    check("MCQ 9.1-G option A/D: 1.20x covenant binds at", q(CAP120, 0), 44602050)
    check("MCQ 9.1-G option A/D: that gearing (74.3367 %)", q(CAP120 / CAPEX * 100, 4), D("74.3367"))
    DS75 = (CAPEX * D("0.75")) / AF612
    check("MCQ 9.1-G debt service at 75 % gearing", q(DS75, 2), D("5367466.32"))
    check("MCQ 9.1-G CFADS the 1.20x covenant then requires", q(DS75 * COV, 0), 6440960)
    check("MCQ 9.1-G shortfall before any stress (56,960)", q(DS75 * COV - CF, 0), 56960)
    check("MCQ 9.1-G invariant: 75 % gearing exceeds the covenant-binding gearing",
          1 if D("0.75") > CAP120 / CAPEX else 0, 1)
    # the sponsors' marginal exchange rate, rebuilt
    DSCR70, DSCR75 = CF / INST, CF / DS75
    check("MCQ 9.1-G equity IRR bought, 70 -> 75 % (48.82 bp, corpus 9.1.4)",
          q((D("13.0193") - D("12.5311")) * 100, 2), D("48.82"))
    check("MCQ 9.1-G coverage surrendered, 70 -> 75 % (8.50 hundredths)",
          q((DSCR70 - DSCR75) * 100, 2), D("8.50"))
    check("MCQ 9.1-G option B: 5.75 bp of return per hundredth of coverage",
          q((D("13.0193") - D("12.5311")) * 100 / ((DSCR70 - DSCR75) * 100), 2), D("5.75"))

    # ============================== D9 KA 9.2 — MCQ 9.2-F ========================================
    IDLE, COUPON, DEPOSIT, CONSTR = DEBT / 2, D("0.06"), D("0.03"), 2
    NEGARB = IDLE * (COUPON - DEPOSIT) * CONSTR
    COMMIT = IDLE * D("0.006") * CONSTR
    check("MCQ 9.2-F negative arbitrage on a fully drawn bond", NEGARB, 1260000)
    check("MCQ 9.2-F commitment fees on a progressively drawn facility", COMMIT, 252000)
    check("MCQ 9.2-F avoidable cost on this dimension alone", NEGARB - COMMIT, 1008000)
    check("MCQ 9.2-F option A: levelised over the facility, 35.78 bp a year on 42,000,000",
          q(NEGARB / AF612 / DEBT * 10000, 2), D("35.78"))
    check("MCQ 9.2-F invariant: the negative arbitrage is five times the commitment fee",
          q(NEGARB / COMMIT, 4), 5)

    # ============================== D9 KA 9.3 — MCQ 9.3-G, 9.3-H =================================
    # 11.3.2's currency decomposition, rebuilt so 9.3-G's cross-references are earned.
    FX0 = D(4)
    HC_REV = D(12000000) * FX0
    HC_LOCAL_OPEX = D(3150000) * FX0
    HC_TAX, HC_WC = D(2064000), D(2400000)
    USD_OPEX = D(1350000)
    HC_NUM = HC_REV - HC_LOCAL_OPEX - HC_TAX - HC_WC
    check("9.3-G cross-ref: HC local numerator", HC_NUM, 30936000)
    check("9.3-G cross-ref invariant: at the closing rate the master thread reappears",
          q(HC_NUM / FX0 - USD_OPEX, 0), CF)
    X_COV = HC_NUM / (COV * INST + USD_OPEX)
    check("MCQ 9.3-G rationale: devaluation that breaches the covenant (5.06 %)",
          q((X_COV / FX0 - 1) * 100, 2), D("5.06"))
    # the indexation remedy, which is what makes option C over-strong rather than wrong
    IDX = (INST + USD_OPEX) / D(12000000)
    check("9.3-G rationale: debt-service-matching indexed share (52.997 %)",
          q(IDX * 100, 3), D("52.997"))
    # with share s indexed to FX, USD CFADS(x) = s*R + (1-s)*R*FX0/x - USD costs ... local costs and
    # tax/WC stay in HC, so rebuild the whole line and solve for the covenant-breaching rate.
    HC_NONREV = HC_LOCAL_OPEX + HC_TAX + HC_WC

    def cfads_indexed(x, s):
        rev_usd = D(12000000) * s + (HC_REV * (1 - s)) / x
        return rev_usd - HC_NONREV / x - USD_OPEX

    lo, hi = FX0, D(20)
    for _ in range(300):
        mid = (lo + hi) / 2
        if cfads_indexed(mid, IDX) > COV * INST:
            lo = mid
        else:
            hi = mid
    check("MCQ 9.3-G rationale: indexation lifts tolerable devaluation to +37.17 %",
          q(((lo + hi) / 2 / FX0 - 1) * 100, 2), D("37.17"))

    # MCQ 9.3-H — the capitalised exposure premium
    ELIG, PREMIUM, ECA_RATE, ECA_N = D(15000000), D("0.06"), D("0.038"), 14
    LOAN = ELIG * (1 + PREMIUM)
    check("MCQ 9.3-H grossed-up loan", LOAN, 15900000)
    ECA_INST = LOAN / af(ECA_RATE, ECA_N)
    check("MCQ 9.3-H instalment on the grossed-up loan", q(ECA_INST, 2), D("1485429.84"))
    lo, hi = D("0.01"), D("0.20")
    for _ in range(300):
        mid = (lo + hi) / 2
        if ECA_INST * af(mid, ECA_N) > ELIG:
            lo = mid
        else:
            hi = mid
    ALLIN = (lo + hi) / 2
    check("MCQ 9.3-H all-in cost solved from proceeds against instalments (4.6895 %)",
          q(ALLIN * 100, 4), D("4.6895"))
    check("MCQ 9.3-H basis points the headline rate understates (88.95)",
          q((ALLIN * 100 - ECA_RATE * 100) * 100, 2), D("88.95"))
    check("MCQ 9.3-H invariant: capitalisation leaves proceeds unchanged (option B's error)",
          ELIG, 15000000)

    # ============================== D9 KA 9.4 — MCQ 9.4-F, 9.4-G =================================
    FACE, CONC_RATE, GRACE, AMORT, MKT = D(12000000), D("0.02"), 7, 18, D("0.06")
    GRACE_INT = FACE * CONC_RATE
    CONC_INST = FACE / af(CONC_RATE, AMORT)
    check("MCQ 9.4-F interest during grace", GRACE_INT, 240000)
    check("MCQ 9.4-F amortising instalment after grace", q(CONC_INST, 2), D("800425.23"))
    pv = sum(GRACE_INT / (1 + MKT) ** t for t in range(1, GRACE + 1))
    pv += sum(CONC_INST / (1 + MKT) ** t for t in range(GRACE + 1, GRACE + AMORT + 1))
    check("MCQ 9.4-F present value of the whole service stream (7,103,613)", q(pv, 0), 7103613)
    check("MCQ 9.4-F option B: grant element 40.80 %", q((1 - pv / FACE) * 100, 2), D("40.80"))
    check("MCQ 9.4-F option B: debt content, about 7.1 million", q(pv / D(1000000), 1), D("7.1"))
    check("MCQ 9.4-F option B: subsidy, about 4.9 million",
          q((FACE - pv) / D(1000000), 1), D("4.9"))
    check("MCQ 9.4-F invariant: debt content plus subsidy is the face value",
          q(pv + (FACE - pv), 2), FACE)
    check("MCQ 9.4-G rationale: G1 lifts equity IRR by 429.20 bp (corpus 9.4.2)",
          q((D("16.8231") - D("12.5311")) * 100, 2), D("429.20"))

    # ============================== D10 KA 10.1 — MCQ 10.1-H ====================================
    GAP = DEBT - CAP130
    check("MCQ 10.1-H the gap the item is set against (828,877)", q(GAP, 0), 828877)
    check("MCQ 10.1-H option B: the ratio 42,000,000 actually delivers", q(CF / INST, 4), D("1.2743"))
    CAP13 = (CF / SIZING) * AF613
    check("MCQ 10.1-H option C: thirteen years at 1.30x supports", q(CAP13, 0), 43473483)
    check("MCQ 10.1-H option C: which clears the request with", q(CAP13 - DEBT, 0), 1473483)
    check("MCQ 10.1-H invariant: a thirteenth year more than closes an 828,877 gap",
          1 if CAP13 - DEBT > GAP else 0, 1)
    CF_NEEDED = (DEBT / AF612) * SIZING
    check("MCQ 10.1-H option D: CFADS uplift the request would need", q(CF_NEEDED - CF, 0), 128526)
    check("MCQ 10.1-H option D: that uplift as a share of CFADS (2.0132 %)",
          q((CF_NEEDED - CF) / CF * 100, 4), D("2.0132"))
    INST13 = DEBT / AF613
    check("MCQ 10.1-H year-one DSCR at 42,000,000 over thirteen years (1.3456)",
          q(CF / INST13, 4), D("1.3456"))
    check("MCQ 10.1-H invariant: thirteen-year coverage clears the 1.30x target",
          1 if CF / INST13 > SIZING else 0, 1)

    # ============================== D10 KA 10.3 — MCQ 10.3-G, 10.3-H ============================
    DSRA6 = INST * D(6) / D(12)
    check("MCQ 10.3-G six-month debt service reserve", q(DSRA6, 0), 2504818)
    check("MCQ 10.3-G the CFADS collapse it survives, as a share of base case (39 %)",
          q(DSRA6 / CF * 100, 0), 39)
    check("MCQ 10.3-G invariant: coverage at that CFADS is far below the covenant",
          1 if DSRA6 / INST < COV else 0, 1)

    def tol(m):
        return 1 - INST * (1 - D(m) / 12) / CF

    check("MCQ 10.3-H tolerance at six months funded (60.7641 %)", q(tol(6) * 100, 4), D("60.7641"))
    check("MCQ 10.3-H tolerance at nine months (option D's 80.3821 %)",
          q(tol(9) * 100, 4), D("80.3821"))
    check("MCQ 10.3-H tolerance at twelve months (100 %)", q(tol(12) * 100, 4), 100)
    check("MCQ 10.3-H the increment six to twelve months (39.2359 points)",
          q((tol(12) - tol(6)) * 100, 4), D("39.2359"))
    check("MCQ 10.3-H points bought per month (6.5393)", q((tol(1) - tol(0)) * 100, 4), D("6.5393"))
    check("MCQ 10.3-H invariant: tolerance is linear in m, so every month buys the same",
          q((tol(9) - tol(8)) * 100, 10), q((tol(1) - tol(0)) * 100, 10))
    check("MCQ 10.3-H cash cost of one month (417,470)", q(INST / 12, 0), 417470)
    check("MCQ 10.3-H cash per percentage point of tolerance (63,840)",
          q((INST / 12) / ((tol(1) - tol(0)) * 100), 2), D("63840.00"))
    check("MCQ 10.3-H invariant: the increment is a second six months of service",
          q(INST * D(6) / D(12), 2), q(DSRA6, 2))
    KE, DEP, LCFEE = D("0.1542"), D("0.0300"), D("0.0125")
    check("MCQ 10.3-H option C: LC fee on the incremental cover (31,310)",
          q(DSRA6 * LCFEE, 0), 31310)
    check("MCQ 10.3-H option C: equity carry on the same cash (311,098)",
          q(DSRA6 * (KE - DEP), 0), 311098)
    check("MCQ 10.3-H option C: breakeven LC fee (12.42 %)", q((KE - DEP) * 100, 2), D("12.42"))
    check("MCQ 10.3-H option B: after-tax cost of senior debt (4.80 %)",
          q(D("6.00") * (1 - TAXR), 2), D("4.80"))
    check("MCQ 10.3-H invariant: the LC is an order of magnitude below the breakeven",
          1 if LCFEE * 100 * 9 < (KE - DEP) * 100 else 0, 1)

    # ============================== D10 KA 10.4 — MCQ 10.4-G, 10.4-H ============================
    CURE_A, CURE_B = D(96196), D(61485)
    check("MCQ 10.4-G option A: the saving the prepayment treatment buys", CURE_A - CURE_B, 34711)
    check("MCQ 10.4-G option A: that saving as a percentage (36.08 %)",
          q((CURE_A - CURE_B) / CURE_A * 100, 2), D("36.08"))
    check("MCQ 10.4-G key: the year-twelve shortfall (74,849, corpus 10.4.3)", D(74849), 74849)
    check("MCQ 10.4-G key invariant: the base-case cure is trivial beside the 828,877 resizing",
          1 if CURE_A * 8 < GAP else 0, 1)
    check("MCQ 10.4-H distribution condition in cash (1.25x)", q(INST * DISTCOND, 0), 6262044)
    check("MCQ 10.4-H financial covenant in cash (1.20x)", q(INST * COV, 0), 6011562)
    check("MCQ 10.4-H lock-up trigger in cash (1.15x)", q(INST * LOCKUP, 0), 5761081)
    check("MCQ 10.4-H invariant: the three thresholds engage in descending cash order",
          1 if INST * DISTCOND > INST * COV > INST * LOCKUP else 0, 1)
    check("MCQ 10.4-H rationale: the 1.15x trigger is never reached on the bank case minimum",
          1 if D(5936713) > INST * LOCKUP else 0, 1)

    # ============================== D11 KA 11.1 — MCQ 11.1-E, 11.1-F ============================
    # Rebuild the register from its own p and impact columns; nothing is copied from the table.
    REGISTER = (
        ("A1", D("0.30"), D(6000000), D("0.12"), D(4000000)),
        ("A2", D("0.35"), D(5200000), D("0.20"), D(3400000)),
        ("A3", D("0.25"), D(3200000), D("0.15"), D(2400000)),
        ("A4", D("0.40"), D(2400000), D("0.45"), D(2900000)),
        ("A5", D("0.30"), D(1800000), D("0.31"), D(2000000)),
        ("A6", D("0.35"), D(1400000), D("0.35"), D(1500000)),
        ("A7", D("0.50"), D(900000), D("0.50"), D(900000)),
        ("A8", D("0.20"), D(2000000), D("0.20"), D(2000000)),
    )
    LOADING = D("1.40")
    OPPORTUNITY = D(150000)                       # A9, retained
    CONTROLLED = {"A1", "A2", "A3"}
    own13 = sum(p * i for k, p, i, _, _ in REGISTER if k in CONTROLLED)
    own48 = sum(p * i for k, p, i, _, _ in REGISTER if k not in CONTROLLED)
    prem13 = sum(bp * bi * LOADING for k, _, _, bp, bi in REGISTER if k in CONTROLLED)
    prem48 = sum(bp * bi * LOADING for k, _, _, bp, bi in REGISTER if k not in CONTROLLED)
    bid48 = sum(bp * bi for k, _, _, bp, bi in REGISTER if k not in CONTROLLED)
    check("MCQ 11.1-E owner EMV on the controlled items", own13, 4420000)
    check("MCQ 11.1-E loaded premium on the controlled items", prem13, 2128000)
    check("MCQ 11.1-E owner EMV on the uncontrolled items", own48, 2840000)
    check("MCQ 11.1-E loaded premium on the uncontrolled items", prem48, 4620000)
    check("MCQ 11.1-E stem: the single-wrap premium", prem13 + prem48, 6748000)
    RETAIN_ALL = own13 + own48 - OPPORTUNITY
    BUNDLE = prem13 + prem48 - OPPORTUNITY
    UNBUNDLED = prem13 + own48 - OPPORTUNITY
    check("MCQ 11.1-E stem: expected cost of retaining the whole register", RETAIN_ALL, 7110000)
    check("MCQ 11.1-E option A: expected cost with the bundle", BUNDLE, 6598000)
    check("MCQ 11.1-E option A: by which the bundle beats full retention", RETAIN_ALL - BUNDLE,
          512000)
    check("MCQ 11.1-E stem/key: expected cost of the unbundled allocation", UNBUNDLED, 4818000)
    check("MCQ 11.1-E key: by which unbundling beats the bundle", BUNDLE - UNBUNDLED, 1780000)
    check("MCQ 11.1-E option C: value the controlled transfers create", own13 - prem13, 2292000)
    check("MCQ 11.1-E option B: destruction remaining at a zero loading", bid48 - own48, 460000)
    check("MCQ 11.1-E option B: the bidder's own expected cost on those items", bid48, 3300000)
    check("MCQ 11.1-E invariant: option A is true (the bundle does beat retention) and still wrong",
          1 if 0 < RETAIN_ALL - BUNDLE < BUNDLE - UNBUNDLED else 0, 1)
    check("MCQ 11.1-E invariant: 512,000 is 2,292,000 of creation less 1,780,000 of destruction",
          (own13 - prem13) - (prem48 - own48), RETAIN_ALL - BUNDLE)
    check("MCQ 11.1-F invariant: the capacity ground leaves the distribution unchanged",
          sum(bp * bi for k, _, _, bp, bi in REGISTER if k in {"A7", "A8"}),
          sum(p * i for k, p, i, _, _ in REGISTER if k in {"A7", "A8"}))

    # ============================== D11 KA 11.2 — MCQ 11.2-F ====================================
    DAILY_DELAY = D("24733.33")
    check("MCQ 11.2-F rationale: a 30-day outage costs", q(DAILY_DELAY * 30, 0), 742000)
    check("MCQ 11.2-F rationale: a cap at 50 % of a 1,200,000 fee", D(1200000) * D("0.50"), 600000)
    check("MCQ 11.2-F invariant: the outage cost already exceeds the cap",
          1 if DAILY_DELAY * 30 > D(1200000) * D("0.50") else 0, 1)
    check("MCQ 11.2-F stem: points of availability between compliance and breach (2.9)",
          q(D(95) - D("92.086"), 1), D("2.9"))

    # ============================== D11 KA 11.3 — MCQ 11.3-G ====================================
    PRINCIPAL_Y1 = D("2489635.23")
    MARGIN, REF0, SWAP_REF = D("0.02"), D("0.04"), D("0.042")
    check("11.3.1 invariant: year-one principal is the instalment less year-one interest",
          q(INST - DEBT * (REF0 + MARGIN), 2), PRINCIPAL_Y1)

    def dscr(allin):
        return CF / (PRINCIPAL_Y1 + DEBT * allin)

    check("MCQ 11.3-G unhedged DSCR at -200 bp (1.5311)", q(dscr(D("0.04")), 4), D("1.5311"))
    check("MCQ 11.3-G unhedged DSCR at +200 bp (1.0914)", q(dscr(D("0.08")), 4), D("1.0914"))
    check("MCQ 11.3-G full-hedge DSCR (1.2533)", q(dscr(SWAP_REF + MARGIN), 4), D("1.2533"))
    H75 = (SWAP_REF + MARGIN) * D("0.75") + D("0.08") * D("0.25")
    check("MCQ 11.3-G 75 % hedge holds at +200 bp (1.2085)", q(dscr(H75), 4), D("1.2085"))
    BE_COV = ((CF / COV) - PRINCIPAL_Y1) / DEBT
    BE_PAY = (CF - PRINCIPAL_Y1) / DEBT
    check("MCQ 11.3-G covenant breakeven all-in rate (6.7390 %)", q(BE_COV * 100, 4), D("6.7390"))
    check("MCQ 11.3-G option A: covenant binds at +73.9 bp of REFERENCE rate",
          q((BE_COV * 100 - MARGIN * 100 - REF0 * 100) * 100, 1), D("73.9"))
    check("MCQ 11.3-G stem: payment failure at an all-in 9.2723 %", q(BE_PAY * 100, 4), D("9.2723"))
    check("MCQ 11.3-G option A/B: payment failure at +327.2 bp of REFERENCE rate",
          q((BE_PAY * 100 - MARGIN * 100 - REF0 * 100) * 100, 1), D("327.2"))
    check("MCQ 11.3-G option B rounded as printed (327 bp of reference-rate headroom)",
          q((BE_PAY * 100 - MARGIN * 100 - REF0 * 100) * 100, 0), 327)
    check("MCQ 11.3-G invariant: measuring the all-in rate against the reference rate would give "
          "the 528 bp the draft printed, which is the error corrected",
          q((BE_PAY * 100 - REF0 * 100) * 100, 0), 527)
    HMIN = (D("0.08") - BE_COV) / (D("0.08") - (SWAP_REF + MARGIN))
    check("MCQ 11.3-G stem: minimum hedge ratio surviving +200 bp (70.0576 %)",
          q(HMIN * 100, 4), D("70.0576"))
    COST = dscr(D("0.06")) - dscr(SWAP_REF + MARGIN)
    RANGE = dscr(D("0.04")) - dscr(D("0.08"))
    check("MCQ 11.3-G option A: coverage given up (0.0210)", q(COST, 4), D("0.0210"))
    check("MCQ 11.3-G option A: coverage range removed (0.4397)", q(RANGE, 4), D("0.4397"))
    check("MCQ 11.3-G option A: units of range per unit surrendered (20.92)",
          q(RANGE / COST, 2), D("20.92"))
    check("MCQ 11.3-G stem: full-hedge year-one cash cost (84,000)",
          q(DEBT * (SWAP_REF - REF0), 0), 84000)
    check("MCQ 11.3-G stem: 75 % hedge year-one cash cost (63,000)",
          q(DEBT * D("0.75") * (SWAP_REF - REF0), 0), 63000)
    check("MCQ 11.3-G invariant: the covenant binds far before payment does",
          1 if BE_COV < BE_PAY else 0, 1)

    # ============================== D11 KA 11.4 — MCQ 11.4-F, 11.4-G ============================
    OPREG = (
        (D("0.15"), D(3200000)), (D("0.10"), D(1500000)), (D("0.20"), D(2600000)),
        (D("0.12"), D(4500000)), (D("0.25"), D(900000)), (D("0.30"), D(700000)),
    )
    RHO = D("0.30")
    mean = sum(p * i for p, i in OPREG)
    sigmas = [(p * (1 - p) * i * i).sqrt() for p, i in OPREG]
    sig_ind = sum(s * s for s in sigmas).sqrt()
    p80_ind = mean + Z80 * sig_ind
    var_rho = (1 - RHO) * sum(s * s for s in sigmas) + RHO * (sum(sigmas)) ** 2
    p80_rho = mean + Z80 * var_rho.sqrt()
    check("MCQ 11.4-F stem: register mean (2,125,000)", q(mean, 0), 2125000)
    check("MCQ 11.4-F stem: P80 on independence (4,003,649)", q(p80_ind, 0), 4003649)
    check("MCQ 11.4-F key: the rho = 0.30 uplift (837,478)", q(p80_rho - p80_ind, 0), 837478)
    CEILING = HEADROOM * AF612
    check("MCQ 11.4-F stem: covenant-preserving exposure ceiling (3,122,460)", q(CEILING, 0),
          3122460)
    # 881,189 is the difference of the two figures AS PRINTED (4,003,649 − 3,122,460); the exact
    # difference is 881,189.62, so the manuscript's integer follows the display convention. Both
    # are pinned so a later edit cannot drift to 881,190 without this check saying so.
    check("MCQ 11.4-F key: the gap to be closed, exact on the ctx instalment convention",
          q(p80_ind - CEILING, 2), D("881189.58"))
    check("MCQ 11.4-F key: the gap as printed — difference of the two printed roundings",
          q(p80_ind, 0) - q(CEILING, 0), 881189)
    check("MCQ 11.4-F invariant: both quantities are present values over the same annuity factor",
          q(CEILING / AF612, 2), q(HEADROOM, 2))
    lender = [(p * D("1.5"), i * D("1.4")) for p, i in OPREG]
    lsig = [(p * (1 - p) * i * i).sqrt() for p, i in lender]
    lvar = (1 - RHO) * sum(s * s for s in lsig) + RHO * (sum(lsig)) ** 2
    lp80 = sum(p * i for p, i in lender) + Z80 * lvar.sqrt()
    check("MCQ 11.4-F option C: the lender's P80 (8,884,036)", q(lp80, 0), 8884036)
    STRESSED = CF - lp80 / AF612
    RESIZED = (STRESSED / COV) * AF612
    check("MCQ 11.4-F option C: facility resized on the lender's P80 (37,198,687)",
          q(RESIZED, 0), 37198687)
    check("MCQ 11.4-F rationale: debt capacity surrendered (4,801,313)", q(DEBT - RESIZED, 0),
          4801313)
    check("MCQ 11.4-F option A invariant: provisioning the mean holds the covenant, "
          "which is why it is the tempting answer",
          1 if (CF - mean / AF612) / INST > COV else 0, 1)
    check("MCQ 11.4-G invariant: the two model-risk lines are 435,000 of the mean",
          sum(p * i for p, i in OPREG[4:]), 435000)

    # ============================== D12 KA 12.1 — MCQ 12.1-G ====================================
    SHORTFALL_ANNUAL = D(453000)
    VALUE_BASIS = SHORTFALL_ANNUAL * AF825
    SIZED_BASIS = DEBT * (SHORTFALL_ANNUAL / CF)
    SUBCAP = D(4800000)
    CF95 = D(5931000)
    BARE_BASIS = DEBT - (CF95 / COV) * AF612
    check("MCQ 12.1-G stem: value of the sponsors' loss (4,835,674)", q(VALUE_BASIS, 0), 4835674)
    check("MCQ 12.1-G stem: the sized-coverage basis (2,980,263)", q(SIZED_BASIS, 0), 2980263)
    check("MCQ 12.1-G stem: the bare covenant-restoring figure (562,851)", q(BARE_BASIS, 0), 562851)
    check("MCQ 12.1-G key: value loss the sized basis gives up (1,855,410)",
          q(VALUE_BASIS - SIZED_BASIS, 0), 1855410)
    check("MCQ 12.1-G option C: the spread between the outer bases (8.591x)",
          q(VALUE_BASIS / BARE_BASIS, 3), D("8.591"))
    RELIEF = INST - (DEBT - SUBCAP) / AF612
    check("MCQ 12.1-G key: annual relief from applying the cap to prepayment (572,530)",
          q(RELIEF, 0), 572530)
    check("MCQ 12.1-G key: present value of that relief over twelve years (4,314,629)",
          q(RELIEF * AF812, 0), 4314629)
    check("MCQ 12.1-G key: residual under-compensation (521,045)",
          q(VALUE_BASIS - RELIEF * AF812, 0), 521045)
    check("MCQ 12.1-G option D invariant: the value basis exceeds the sub-cap, so it is not "
          "deliverable without moving the cap",
          1 if VALUE_BASIS > SUBCAP else 0, 1)
    check("MCQ 12.1-G invariant: the three bases rank bare < sized < value",
          1 if BARE_BASIS < SIZED_BASIS < VALUE_BASIS else 0, 1)

    # ============================== D12 KA 12.2 — MCQ 12.2-G ====================================
    bal = DEBT
    for _ in range(5):
        bal -= INST - bal * RATE
    check("MCQ 12.2-G rationale: debt outstanding at the end of year five (27,965,695)",
          q(bal, 0), 27965695)
    DIST = CF - INST
    check("12.2-G cross-ref: annual nominal distribution (1,374,365)", q(DIST, 0), 1374365)
    check("MCQ 12.2-G rationale: five years of nominal distributions (6,871,824)",
          q(DIST * 5, 0), 6871824)
    check("MCQ 12.2-G key: the sponsors' unreturned equity (11,128,176)",
          q(EQUITY - DIST * 5, 0), 11128176)
    check("MCQ 12.2-G option D invariant: the formula tracks what is owed, so the lenders are whole",
          1 if bal > 0 else 0, 1)

    # ============================== D12 KA 12.3 — MCQ 12.3-E, 12.3-F ============================
    BOND, PCG_OFFER, PAYPROB = D(4800000), D(5500000), D("0.70")
    check("MCQ 12.3-E stem: face amount by which the offer is higher", PCG_OFFER - BOND, 700000)
    check("MCQ 12.3-E option C: risk-adjusted cover of the offer", PCG_OFFER * PAYPROB, 3850000)
    check("MCQ 12.3-E option C: cover given up against the bond", BOND - PCG_OFFER * PAYPROB,
          950000)
    check("MCQ 12.3-E option C: the equivalent face (6,857,143)", q(BOND / PAYPROB, 0), 6857143)
    BONDFEE = BOND * D("0.012") * 3
    check("MCQ 12.3-E stem: bond fee over the construction period (172,800)", BONDFEE, 172800)
    check("MCQ 12.3-E rationale: the fee saving is less than a fifth of the cover given up",
          1 if BONDFEE * 5 < BOND - PCG_OFFER * PAYPROB else 0, 1)
    check("MCQ 12.3-E invariant: the offer is nominally higher and risk-adjusted lower",
          1 if PCG_OFFER > BOND and PCG_OFFER * PAYPROB < BOND else 0, 1)
    check("MCQ 12.3-E option B invariant: demand form does not change the obligor's credit, so an "
          "on-demand 5,500,000 parent guarantee is still worth 3,850,000",
          PCG_OFFER * PAYPROB, 3850000)
    check("MCQ 12.3-E invariant: the payment probability that would make the offer adequate",
          q(BOND / PCG_OFFER, 4), D("0.8727"))
    check("MCQ 12.3-F invariant: two instruments of equal face differ only on certainty and "
          "duration — equal face, unequal cover",
          q(BOND * 1 - BOND * PAYPROB, 0), 1440000)

    # ============================== D12 KA 12.4 — MCQ 12.4-G ====================================
    LD_RATE, PROLONG_RATE = D(20000), D(12500)
    check("MCQ 12.4-G key: the asymmetry in the two daily rates", LD_RATE - PROLONG_RATE, 7500)
    check("MCQ 12.4-G rationale: the damages rate recovers 80.86 % of the daily economic cost",
          q(LD_RATE / DAILY_DELAY * 100, 2), D("80.86"))
    check("MCQ 12.4-G option B invariant: equalising downward would recover only 50.54 %",
          q(PROLONG_RATE / DAILY_DELAY * 100, 2), D("50.54"))
