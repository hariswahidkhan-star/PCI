"""PFL-AI Domain 1 (extension) — Foundations of project finance: golden checks for the NEW material.

The domain's original numbers (WE 1.2.2's cash bridge, WE 1.2.3's interest-only leverage table,
Exercises 1.1-1.3) are verified in `verify_formulas.py` and are NOT duplicated here. This module
covers only what the depth expansion added, and every value is recomputed from its definition rather
than read back from the manuscript. Master-thread values come from ctx and are never re-typed.

Five engines carry the new material:

  recourse      (WE 1.1.2, Fig 1.1.3, MCQ 1.1-D, Toolkit 1.T.4, Exercise 1.4, Case study A) the
                incremental cost of a limited-recourse route is PV(instalment differential) plus a
                largely FIXED close-cost premium, and the breakeven failure probability is that cost
                divided by the exposure the ring-fence removes. The exposure is built from the
                amortisation schedule itself — the year-three closing balance is recomputed by
                running the recursion, not asserted — less the enforcement recovery. Because the
                premium is fixed and the exposure scales with the facility, p*(D) is a hyperbola:
                its asymptote, its 100 % crossing and every plotted marker are solved here, and the
                monotonicity and the limit are asserted as invariants rather than as points.

  duration      (WE 1.1.4, Fig-free, MCQ 1.1-E, Exercise 1.6, 1.A.3) Macaulay duration computed BOTH
                ways — the closed form for a level stream and the Sigma t*CF/(1+r)^t definition — so
                a printed duration has to survive two independent routes. The three teaching
                invariants are proved, not stated: the (1+r)/r ceiling binds at every tenor, a
                deferral adds exactly the deferral, and escalation adds duration.

  retention     (WE 1.2.1, MCQ 1.2-F) a retained risk is paid for in the capital structure: the
                equity substituted for debt, priced at the equity-debt spread over the debt term.
                The margin-response alternative is priced too, because the manuscript's professional
                point is that the FORM of the lender's response changes the answer's sign.

  coverage      (WE 1.2.2 interpretation, WE 1.2.2b, MCQ 1.2-E, Exercise 1.7) covenant headroom
                translated into days of revenue, and the DSCR-to-days sensitivity. The coverage
                identity DSCR x debt service = CFADS is reversed to test the printed ratio, and the
                4-decimal rounding residual (221.83) is asserted as a rounding artefact.

  leverage      (WE 1.2.3 interpretation, WE 1.2.3b, Fig 1.2.2, MCQ 1.2-D, Exercise 1.5) the
                levered-return identity re-derives EVERY row of the interest-only table, the
                crossover is solved from the identity rather than read off the table, and both
                equity cliffs and the covenant threshold are solved as cash levels.

  ethics        (WE 1.3.2, WE 1.3.3, 1.A.2, MCQ 1.3-D/E, Case study B) three breakeven
                probabilities of the same shape — a spend divided by what it avoids or buys — plus
                the contingent-claim valuation of sponsor support against cap and expected draw.

Values borrowed from sibling domains are declared once, named, and checked against ctx where ctx
carries them: Domain 13's close-cost budget (2,709,000) and cost of delay (which is DERIVED here
from ctx's CFADS on Domain 5's 30/360 basis, so a drift in either book fails), Domain 9's cost of
equity (15.42 %), Domain 2's pre-working-capital CFADS (6,984,000) and revenue (12,000,000), and
Domain 5's cost-overrun support (10 % of capex).
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]

    DEBT = ctx["KESTREL_DEBT"]                     # 42,000,000
    EQUITY = ctx["KESTREL_EQUITY"]                 # 18,000,000
    CAPEX = ctx["KESTREL_CAPEX"]                   # 60,000,000
    RATE = ctx["KESTREL_RATE"]                     # 6.0 %
    TEN = ctx["KESTREL_TENOR"]                     # 12 years
    LIFE = ctx["KESTREL_LIFE"]                     # 25 years
    CFADS = ctx["KESTREL_CFADS"]                   # 6,384,000
    DISC = ctx["KESTREL_DISCOUNT"]                 # 8.0 %

    # ---- sibling-domain inputs, declared once ------------------------------------------------
    CLOSE_PF = D(2709000)          # Domain 13 KA 13.3.4 close-cost budget
    CLOSE_CORP = D(350000)         # WE 1.1.2's corporate-route close costs (this domain's own)
    CORP_RATE = D("0.046")         # WE 1.1.2's corporate rate, 140 bp inside the senior rate
    RECOVERY = D("0.40")           # WE 1.1.2's enforcement recovery on capital cost
    KE = D("0.1542")               # Domain 9 KA 9.1.3 cost of equity at 70 % gearing
    CFADS_PRE = D(6984000)         # Domain 2 KA 2.3 CFADS before working-capital movements
    REVENUE = D(12000000)          # Domain 2 first-year revenue
    STREAM = D(8900000)            # Domain 4 appraisal operating stream
    COVENANT = D("1.20")           # Domain 10 KA 10.2.1 financial covenant

    AF_PF = af(RATE, TEN)
    AF_CORP = af(CORP_RATE, TEN)
    AF_DISC = af(DISC, TEN)
    INST = DEBT / AF_PF

    # ============================================================================ recourse ====
    check("WE 1.1.2 step 3 AF(0.046,12)", AF_CORP.quantize(D("0.000001")), D("9.066641"))
    check("WE 1.1.2 step 3 AF(0.06,12)", AF_PF.quantize(D("0.000001")), D("8.383844"))
    check("WE 1.1.2 step 3 AF(0.08,12)", AF_DISC.quantize(D("0.000001")), D("7.536078"))
    check("WE 1.1.2 Result project instalment vs master thread",
          INST.quantize(D("0.01")), ctx["KESTREL_INSTALMENT"])
    check("WE 1.1.2 Result corporate instalment",
          (DEBT / AF_CORP).quantize(D("0.01")), D("4632366.30"))

    DIFFERENTIAL = INST - DEBT / AF_CORP
    check("WE 1.1.2 Result annual differential", DIFFERENTIAL.quantize(D("0.01")), D("377268.94"))
    check("WE 1.1.2 Result PV of project debt service at 8 %",
          (INST * AF_DISC).quantize(D("0.01")), D("37753001.96"))
    check("WE 1.1.2 Result PV of corporate debt service at 8 %",
          (DEBT / AF_CORP * AF_DISC).quantize(D("0.01")), D("34909873.80"))
    PV_MARGIN = DIFFERENTIAL * AF_DISC
    check("WE 1.1.2 Result PV of the margin differential",
          PV_MARGIN.quantize(D("0.01")), D("2843128.16"))
    PREMIUM = CLOSE_PF - CLOSE_CORP
    check("WE 1.1.2 Result close-cost premium", PREMIUM, D(2359000))
    INCREMENTAL = PV_MARGIN + PREMIUM
    check("WE 1.1.2 Result incremental cost of limited recourse",
          INCREMENTAL.quantize(D("0.01")), D("5202128.16"))

    # The exposure is built from the schedule, not asserted: run the recursion three years.
    bal = DEBT
    for _ in range(3):
        bal = bal * (1 + RATE) - INST
    check("WE 1.1.2 step 3 year-three closing balance (recursion, Domain 3's schedule)",
          bal.quantize(D("0.01")), D("34073997.27"))
    check("WE 1.1.2 step 1 enforcement recovery at 40 % of capital cost",
          RECOVERY * CAPEX, D(24000000))
    EXPOSURE = bal - RECOVERY * CAPEX
    check("WE 1.1.2 Result exposure removed", EXPOSURE.quantize(D("0.01")), D("10073997.27"))
    check("WE 1.1.2 Result breakeven failure probability p*",
          (INCREMENTAL / EXPOSURE * 100).quantize(D("0.0001")), D("51.6392"))
    check("WE 1.1.2 Result note incremental cost as a share of debt raised",
          (INCREMENTAL / DEBT * 100).quantize(D("0.0001")), D("12.3860"))
    check("WE 1.1.2 Result note fixed premium as a share of the incremental cost",
          (PREMIUM / INCREMENTAL * 100).quantize(D("0.0001")), D("45.3468"))

    # Recovery sensitivity: 40 % -> 50 % of capital cost.
    EXP50 = bal - D("0.50") * CAPEX
    check("WE 1.1.2 Interpretation exposure at 50 % recovery",
          EXP50.quantize(D("0.01")), D("4073997.27"))
    check("WE 1.1.2 Interpretation p* at 50 % recovery (impossible)",
          (INCREMENTAL / EXP50 * 100).quantize(D("0.0001")), D("127.6910"))

    # Fig 1.1.3: p*(D) with the two per-dollar coefficients solved, not copied.
    VAR = PV_MARGIN / DEBT                       # PV of margin differential per dollar of debt
    EXP_PD = EXPOSURE / DEBT                     # exposure removed per dollar of debt
    check("Fig 1.1.3 spec margin differential per dollar of debt",
          VAR.quantize(D("0.0000001")), D("0.0676935"))
    check("Fig 1.1.3 spec margin differential as a percentage of debt",
          (VAR * 100).quantize(D("0.0001")), D("6.7694"))
    check("Fig 1.1.3 spec exposure removed per dollar of debt",
          EXP_PD.quantize(D("0.0000001")), D("0.2398571"))

    def pstar(debt):
        return (PREMIUM + debt * VAR) / (debt * EXP_PD) * 100

    for size, printed in [(15, "93.7893"), (25, "67.5625"), (42, "51.6392"),
                          (75, "41.3358"), (150, "34.7791"), (300, "31.5008")]:
        check(f"Fig 1.1.3 spec p* at {size},000,000 of debt",
              pstar(D(size) * D(1000000)).quantize(D("0.0001")), D(printed))
    check("WE 1.1.2 Interpretation asymptote (margin differential alone)",
          (VAR / EXP_PD * 100).quantize(D("0.0001")), D("28.2224"))
    CROSS = PREMIUM / (EXP_PD - VAR)
    check("WE 1.1.2 Interpretation debt size at which p* reaches 100 %",
          CROSS.quantize(D("0.01")), D("13702087.32"))
    check("Fig 1.1.3 spec 100 % crossing, as printed in the summary",
          CROSS.quantize(D("1")), D(13702087))

    # Invariants the figure and the interpretation claim, proved rather than asserted.
    check("INVARIANT 1.1.2 p* is exactly 100 % at the crossing",
          pstar(CROSS).quantize(D("0.000001")), D(100))
    check("INVARIANT 1.1.2 p* exceeds 100 % just below the crossing (impossible configuration)",
          D(1) if pstar(CROSS - D(1000)) > 100 else D(0), D(1))
    falling = all(pstar(D(n) * D(1000000)) > pstar(D(n + 5) * D(1000000)) for n in range(15, 300, 5))
    check("INVARIANT 1.1.2 p* falls monotonically with facility size", D(1) if falling else D(0), D(1))
    check("INVARIANT 1.1.2 p* approaches the asymptote from above as the premium goes immaterial",
          pstar(D(1000000000000)).quantize(D("0.0001")), (VAR / EXP_PD * 100).quantize(D("0.0001")))
    check("INVARIANT 1.1.2 exposure per dollar reconciles to balance fraction less recovery/gearing",
          (bal / DEBT - RECOVERY / (DEBT / CAPEX)).quantize(D("0.0000001")),
          EXP_PD.quantize(D("0.0000001")))

    check("MCQ 1.1-D distractor B (fixed premium only, margin differential dropped)",
          (PREMIUM / EXPOSURE * 100).quantize(D("0.0001")), D("23.4167"))
    check("MCQ 1.1-D distractor B as printed to two decimals",
          (PREMIUM / EXPOSURE * 100).quantize(D("0.01")), D("23.42"))
    check("MCQ 1.1-D distractor A (incremental cost over the facility, a cost intensity)",
          (INCREMENTAL / DEBT * 100).quantize(D("0.01")), D("12.39"))
    check("MCQ 1.1-D key / WE 1.1.2 Interpretation p* as printed to two decimals",
          (INCREMENTAL / EXPOSURE * 100).quantize(D("0.01")), D("51.64"))
    check("Case study A incremental cost, as printed",
          INCREMENTAL.quantize(D("1")), D(5202128))
    check("Case study A margin differential, as printed",
          PV_MARGIN.quantize(D("1")), D(2843128))
    check("Case study A exposure removed, as printed",
          EXPOSURE.quantize(D("1")), D(10073997))
    check("Case study A NPV cited from the master thread", ctx["KESTREL_NPV"], D(16179360))

    # ============================================================================ duration ====
    def dur_closed(r, n):
        return (1 + r) / r - D(n) / ((1 + r) ** n - 1)

    def dur_sum(flows, r, offset=0):
        """Macaulay duration straight from the definition, on (t, CF) pairs."""
        num = sum(D(t + offset) * cf / (1 + r) ** (t + offset) for t, cf in flows)
        den = sum(cf / (1 + r) ** (t + offset) for t, cf in flows)
        return num / den, den

    level = [(t, STREAM) for t in range(1, 16)]
    D_LEVEL, PV_LEVEL = dur_sum(level, DISC)
    check("WE 1.1.4 step 3 duration of the 15-year level stream (closed form)",
          dur_closed(DISC, 15).quantize(D("0.000001")), D("6.594460"))
    check("WE 1.1.4 Result duration of the 15-year level stream (summation, second route)",
          D_LEVEL.quantize(D("0.000001")), D("6.594460"))
    check("WE 1.1.4 Result table (a) present value of the operating stream",
          PV_LEVEL.quantize(D("0.01")), D("76179360.32"))
    check("WE 1.1.4 Result table (a) duration as printed",
          D_LEVEL.quantize(D("0.0001")), D("6.5945"))
    check("WE 1.1.4 Result table ceiling (1+r)/r at 8 %",
          ((1 + DISC) / DISC).quantize(D("0.0001")), D("13.5000"))

    D_GREEN, PV_GREEN = dur_sum(level, DISC, offset=3)
    check("WE 1.1.4 Result table (b) present value of the greenfield stream",
          PV_GREEN.quantize(D("0.01")), D("60473632.32"))
    check("WE 1.1.4 Result table (b) duration of the greenfield stream",
          D_GREEN.quantize(D("0.0001")), D("9.5945"))
    check("INVARIANT 1.1.4 a three-year deferral adds exactly three years of duration",
          (D_GREEN - D_LEVEL).quantize(D("0.0000000001")), D(3))

    indexed = [(t, STREAM * D("1.025") ** (t - 1)) for t in range(1, 16)]
    D_IDX, PV_IDX = dur_sum(indexed, DISC)
    check("WE 1.1.4 Result table (c) present value of the indexed stream",
          PV_IDX.quantize(D("0.01")), D("87937828.16"))
    check("WE 1.1.4 Result table (c) duration of the indexed stream",
          D_IDX.quantize(D("0.0001")), D("7.0342"))
    check("INVARIANT 1.1.4 escalation adds duration (weight moves to later periods)",
          D(1) if D_IDX > D_LEVEL else D(0), D(1))
    check("WE 1.1.4 Interpretation duration added by 2.5 % escalation",
          (D_IDX - D_LEVEL).quantize(D("0.01")), D("0.44"))

    D_25 = dur_closed(DISC, LIFE)
    check("WE 1.1.4 Result table duration of a level stream over the 25-year asset life",
          D_25.quantize(D("0.0001")), D("9.2254"))
    for lab, dd, printed in [("(a)", D_LEVEL, "-7.4055"), ("(b)", D_GREEN, "-4.4055"),
                             ("(c)", D_IDX, "-6.9658"), ("25-year", D_25, "-4.7746")]:
        check(f"WE 1.1.4 Result table gap to a 14-year liability {lab}",
              (dd - 14).quantize(D("0.0001")), D(printed))
    check("WE 1.1.4 Interpretation duration bought by extending 15 years to 25",
          (D_25 - D_LEVEL).quantize(D("0.0001")), D("2.6309"))
    check("INVARIANT 1.1.4 the greenfield position is the closest duration match, on timing alone",
          D(1) if abs(D_GREEN - 14) < min(abs(D_LEVEL - 14), abs(D_IDX - 14), abs(D_25 - 14))
          else D(0), D(1))
    check("WE 1.1.4 Interpretation share of PV arriving in the first six of fifteen years",
          (af(DISC, 6) / af(DISC, 15) * 100).quantize(D("0.01")), D("54.01"))
    check("WE 1.1.4 Interpretation duration as a share of the asset's 15-year life",
          (D_LEVEL / 15 * 100).quantize(D("0.01")), D("43.96"))
    check("WE 1.1.4 Interpretation tenor needed to reach a 13.0-year duration",
          dur_closed(DISC, 63).quantize(D("0.0001")), D("13.0022"))
    check("WE 1.1.4 Interpretation the tenor below it falls short of 13.0",
          dur_closed(DISC, 62).quantize(D("0.0001")), D("12.9706"))
    check("INVARIANT 1.1.4 63 is the SMALLEST tenor reaching 13.0 years",
          D(1) if dur_closed(DISC, 62) < 13 <= dur_closed(DISC, 63) else D(0), D(1))
    check("WE 1.1.4 Interpretation duration of the same stream at 6 %",
          dur_closed(D("0.06"), 15).quantize(D("0.0001")), D("6.9260"))
    check("WE 1.1.4 Interpretation ceiling at 6 %",
          (D("1.06") / D("0.06")).quantize(D("0.0001")), D("17.6667"))
    capped = all(dur_closed(DISC, n) < (1 + DISC) / DISC for n in (1, 15, 25, 60, 200, 600))
    rising = all(dur_closed(DISC, n) < dur_closed(DISC, n + 1) for n in (1, 15, 25, 60, 200))
    check("INVARIANT 1.1.4 a level stream's duration is capped at (1+r)/r at every tenor",
          D(1) if capped else D(0), D(1))
    check("INVARIANT 1.1.4 duration rises monotonically with tenor toward that cap",
          D(1) if rising else D(0), D(1))
    check("WE 1.1.4 step 3 printed intermediate 1.08^15 - 1",
          ((1 + DISC) ** 15 - 1).quantize(D("0.000001")), D("2.172169"))
    check("MCQ 1.1-E distractor B (unweighted mean of dates 1..15 — undiscounted weights)",
          (sum(D(t) for t in range(1, 16)) / 15).quantize(D("0.01")), D("8.00"))
    check("MCQ 1.1-E distractor D (the stream's tenor, mistaken for its duration)",
          D(len(level)).quantize(D("0.01")), D("15.00"))

    # =========================================================================== retention ====
    EQ_NEW = CAPEX * D("0.38")
    check("WE 1.2.1 step 3 equity at 62 % gearing", EQ_NEW.quantize(D("0.01")), D(22800000))
    check("WE 1.2.1 step 3 base equity reconciles to the master thread",
          (CAPEX * D("0.30")).quantize(D("0.01")), EQUITY)
    SUBST = EQ_NEW - CAPEX * D("0.30")
    check("WE 1.2.1 step 3 debt replaced by equity", SUBST.quantize(D("0.01")), D(4800000))
    check("WE 1.2.1 step 3 substitution as a share of the envelope",
          (SUBST / CAPEX * 100).quantize(D("0.0001")), D("8.0000"))
    SPREAD = KE - RATE
    check("WE 1.2.1 step 3 equity-debt spread in basis points",
          (SPREAD * 10000).quantize(D("1")), D(942))
    ANNUAL = SUBST * SPREAD
    check("WE 1.2.1 step 3 annual cost of the substitution", ANNUAL.quantize(D("0.01")), D(452160))
    PV_RET = ANNUAL * AF_DISC
    check("WE 1.2.1 Result cost of retention in present value",
          PV_RET.quantize(D("0.01")), D("3407513.04"))
    check("WE 1.2.1 Result multiple of the risk's own expected cost",
          (PV_RET / D(900000)).quantize(D("0.0001")), D("3.7861"))
    check("WE 1.2.1 Result value created by transferring at 1,350,000",
          (PV_RET - D(1350000)).quantize(D("0.01")), D("2057513.04"))
    check("WE 1.2.1 Interpretation cost of retention at 10 %",
          (ANNUAL * af(D("0.10"), TEN)).quantize(D("0.01")), D("3080878.89"))
    check("WE 1.2.1 Interpretation cost of retention at 6 %",
          (ANNUAL * af(D("0.06"), TEN)).quantize(D("0.01")), D("3790838.88"))
    check("WE 1.2.1 Interpretation annual cost of a 35 bp margin response instead",
          (D("0.0035") * DEBT).quantize(D("0.01")), D(147000))
    PV_MARGIN_ALT = D("0.0035") * DEBT * AF_DISC
    check("WE 1.2.1 Interpretation PV of the margin response",
          PV_MARGIN_ALT.quantize(D("0.01")), D("1107803.47"))
    check("INVARIANT 1.2.1 the margin response is under a third of the de-gearing response, so the "
          "same 1,350,000 quote flips from accept to reject",
          D(1) if PV_MARGIN_ALT * 3 < PV_RET and PV_MARGIN_ALT < D(1350000) else D(0), D(1))

    # ============================================================================ coverage ====
    # WE 1.2.2 interpretation — the generic bridge's breakeven, in days.
    BREAKEVEN_AR = D(2000000) - D(1000000) + D(500000)
    check("WE 1.2.2 Interpretation receivables rise at which operating cash flow crosses zero",
          BREAKEVEN_AR, D(1500000))
    QDAYS = D(365) / 4
    check("WE 1.2.2 Interpretation quarter length used for the days translation",
          QDAYS.quantize(D("0.01")), D("91.25"))
    check("WE 1.2.2 Interpretation breakeven receivables in days of quarterly sales",
          (BREAKEVEN_AR / D(10000000) * QDAYS).quantize(D("0.0001")), D("13.6875"))
    check("WE 1.2.2 Interpretation actual receivables in days of quarterly sales",
          (D(3000000) / D(10000000) * QDAYS).quantize(D("0.0001")), D("27.3750"))
    check("INVARIANT 1.2.2 the actual receivables build is exactly twice the breakeven",
          (D(3000000) / BREAKEVEN_AR).quantize(D("0.000001")), D(2))
    check("WE 1.2.2 Interpretation absorption repeated for four quarters",
          BREAKEVEN_AR * 4, D(6000000))
    check("WE 1.2.2 Interpretation annualised profit at 2,000,000 a quarter",
          D(2000000) * 4, D(8000000))

    # WE 1.2.2b — Kestrel's covenant headroom in days.
    check("WE 1.2.2b Result CFADS before working capital, as DSCR",
          (CFADS_PRE / INST).quantize(D("0.0001")), D("1.3941"))
    WC = CFADS_PRE - CFADS
    check("WE 1.2.2b Result working capital absorbed", WC, D(600000))
    check("WE 1.2.2b Result working capital absorbed, in DSCR",
          (WC / INST).quantize(D("0.0001")), D("0.1198"))
    check("WE 1.2.2b Result working capital absorbed, in days of revenue",
          (WC / REVENUE * 365).quantize(D("0.0001")), D("18.2500"))
    check("WE 1.2.2b Result documented DSCR vs master thread",
          (CFADS / INST).quantize(D("0.0001")), D("1.2743"))
    COV_CASH = COVENANT * INST
    check("WE 1.2.2b step 3 covenant floor at 1.20x",
          COV_CASH.quantize(D("0.01")), D("6011562.28"))
    HEAD = CFADS - COV_CASH
    check("WE 1.2.2b Result remaining headroom", HEAD.quantize(D("0.01")), D("372437.72"))
    check("WE 1.2.2b Result remaining headroom, in DSCR",
          (HEAD / INST).quantize(D("0.0001")), D("0.0743"))
    check("WE 1.2.2b Result remaining headroom, in days of revenue",
          (HEAD / REVENUE * 365).quantize(D("0.0001")), D("11.3283"))
    TOL = CFADS_PRE - COV_CASH
    check("WE 1.2.2b Result total working-capital tolerance from 6,984,000",
          TOL.quantize(D("0.01")), D("972437.72"))
    check("WE 1.2.2b Result total tolerance, in DSCR",
          (TOL / INST).quantize(D("0.0001")), D("0.1941"))
    check("WE 1.2.2b Result total tolerance, in days of revenue",
          (TOL / REVENUE * 365).quantize(D("0.0001")), D("29.5783"))
    check("WE 1.2.2b Result note CFADS worth 0.01x of DSCR",
          (D("0.01") * INST).quantize(D("0.01")), D("50096.35"))
    check("WE 1.2.2b Result note days of revenue worth 0.01x of DSCR",
          (D("0.01") * INST / REVENUE * 365).quantize(D("0.0001")), D("1.5238"))
    check("WE 1.2.2b Interpretation share of the tolerance already consumed",
          (WC / TOL * 100).quantize(D("0.01")), D("61.70"))
    check("INVARIANT 1.2.2b headroom plus absorption equals the total tolerance",
          (HEAD + WC).quantize(D("0.01")), TOL.quantize(D("0.01")))
    check("INVARIANT 1.2.2b the days columns are the DSCR columns rescaled by the same factor",
          ((HEAD / REVENUE * 365) / (HEAD / INST)).quantize(D("0.0001")),
          ((TOL / REVENUE * 365) / (TOL / INST)).quantize(D("0.0001")))
    check("MCQ 1.2-E distractor B (absorption already spent, mistaken for headroom)",
          (WC / REVENUE * 365).quantize(D("0.01")), D("18.25"))
    check("MCQ 1.2-E distractor C (total tolerance, double-counting the 600,000 spent)",
          (TOL / REVENUE * 365).quantize(D("0.01")), D("29.58"))
    check("MCQ 1.2-E distractor D (the 0.01x sensitivity, mistaken for the headroom)",
          (D("0.01") * INST / REVENUE * 365).quantize(D("0.01")), D("1.52"))
    check("MCQ 1.2-E key / summary headroom in days as printed to two decimals",
          (HEAD / REVENUE * 365).quantize(D("0.01")), D("11.33"))
    check("MCQ 1.2-E rationale headroom in currency, as printed",
          HEAD.quantize(D("1")), D(372438))

    # 1.A.3 coverage identity, reversed, with its rounding residual.
    check("1.A.3 coverage identity reversed on the printed 4-decimal ratio",
          (D("1.2743") * ctx["KESTREL_INSTALMENT"]).quantize(D("0.01")), D("6383778.17"))
    check("1.A.3 rounding residual against CFADS",
          (CFADS - D("1.2743") * ctx["KESTREL_INSTALMENT"]).quantize(D("0.01")), D("221.83"))
    check("INVARIANT 1.A.3 that residual is smaller than half a 0.0001 tick of DSCR",
          D(1) if abs(CFADS - D("1.2743") * ctx["KESTREL_INSTALMENT"])
          < D("0.00005") * ctx["KESTREL_INSTALMENT"] else D(0), D(1))
    check("1.A.3 debt service reconciles to the annuity factor",
          INST.quantize(D("0.01")), ctx["KESTREL_INSTALMENT"])
    check("1.A.3 same test on the 6-decimal AF literal the manuscript prints "
          "(tolerance widened: display rounding of AF, 0.03)",
          (DEBT / D("8.383844")).quantize(D("0.01")), ctx["KESTREL_INSTALMENT"], D("0.05"))

    # ============================================================================ leverage ====
    PROJ = D(100000000)          # WE 1.2.3's generic project — deliberately NOT Kestrel
    CASH = D(12000000)
    D70, EQ30 = D(70000000), D(30000000)
    RU, RD = CASH / PROJ, RATE

    def identity(ru):
        return ru + (D70 / EQ30) * (ru - RD)

    check("WE 1.2.3 Interpretation gearing ratio D/E",
          (D70 / EQ30).quantize(D("0.000001")), D("2.333333"))
    check("WE 1.2.3 Interpretation levered-return identity on the base case",
          (identity(RU) * 100).quantize(D("0.0001")), D("26.0000"))
    for cash, printed in [(D(12000000), "26.0"), (D(9000000), "16.0"), (D(6000000), "6.0"),
                          (D(4200000), "0.0")]:
        check(f"INVARIANT 1.2.3 identity reproduces the interest-only table row at {cash}",
              (identity(cash / PROJ) * 100).quantize(D("0.1")), D(printed))
        check(f"INVARIANT 1.2.3 identity equals the direct calculation at {cash}",
              (identity(cash / PROJ) * 100).quantize(D("0.000001")),
              ((cash - D70 * RD) / EQ30 * 100).quantize(D("0.000001")))
    CROSSOVER = PROJ * RD
    check("WE 1.2.3 Interpretation crossover cash where the unlevered return equals the debt rate",
          CROSSOVER.quantize(D("0.01")), D(6000000))
    check("INVARIANT 1.2.3 at the crossover the levered and unlevered returns coincide at 6.0 %",
          (identity(CROSSOVER / PROJ) * 100).quantize(D("0.000001")),
          (CROSSOVER / PROJ * 100).quantize(D("0.000001")))
    check("INVARIANT 1.2.3 below the crossover gearing SUBTRACTS from the equity return",
          D(1) if identity(D(5000000) / PROJ) < D(5000000) / PROJ else D(0), D(1))

    # WE 1.2.3b — the same gearing, amortising.
    INST70 = D70 / AF_PF
    check("WE 1.2.3b step 3 amortising instalment on 70,000,000",
          INST70.quantize(D("0.01")), D("8349392.06"))
    check("WE 1.2.3b step 3 same instalment from the printed 6-decimal AF literal "
          "(tolerance widened: display rounding of AF, 0.05)",
          (D70 / D("8.383844")).quantize(D("0.01")), D("8349392.06"), D("0.10"))
    check("WE 1.2.3b Result year-one interest, identical in both columns",
          (D70 * RD).quantize(D("0.01")), D(4200000))
    check("WE 1.2.3b Result principal repaid in year one",
          (INST70 - D70 * RD).quantize(D("0.01")), D("4149392.06"))
    check("WE 1.2.3b Result equity cash at 12,000,000",
          (CASH - INST70).quantize(D("0.01")), D("3650607.94"))
    check("WE 1.2.3b Result cash-on-cash return on 30,000,000",
          ((CASH - INST70) / EQ30 * 100).quantize(D("0.0001")), D("12.1687"))
    check("WE 1.2.3b Result DSCR at base case, amortising",
          (CASH / INST70).quantize(D("0.0001")), D("1.4372"))
    check("WE 1.2.3b Result DSCR at base case, interest-only",
          (CASH / D(4200000)).quantize(D("0.0001")), D("2.8571"))
    check("WE 1.2.3b Result cash at which the 1.20x covenant fails, interest-only",
          (D(4200000) * COVENANT).quantize(D("0.01")), D(5040000))
    COV70 = INST70 * COVENANT
    check("WE 1.2.3b Result cash at which the 1.20x covenant fails, amortising",
          COV70.quantize(D("0.01")), D("10019270.47"))
    check("WE 1.2.3b Result decline to the amortising covenant",
          ((1 - COV70 / CASH) * 100).quantize(D("0.0001")), D("16.5061"))
    check("WE 1.2.3b Result decline to zero equity cash, amortising",
          ((1 - INST70 / CASH) * 100).quantize(D("0.0001")), D("30.4217"))
    check("WE 1.2.3b Result decline to zero equity cash, interest-only",
          ((1 - D(4200000) / CASH) * 100).quantize(D("0.0001")), D("65.0000"))
    check("MCQ 1.2-D rationale spread benefit remaining after amortisation, in points",
          ((CASH - INST70) / EQ30 * 100 - D(12)).quantize(D("0.0001")), D("0.1687"))
    check("MCQ 1.2-D distractor C (project cash over equity, lender never paid)",
          (CASH / EQ30 * 100).quantize(D("0.0001")), D("40.0000"))
    def solve_cliff(service):
        """Cash level at which equity cash reaches zero, found by bisection on equity cash —
        an independent route to 1.A.3's claim that the cliff is debt service, not debt."""
        lo, hi = D(0), D(30000000)
        for _ in range(200):
            mid = (lo + hi) / 2
            if (mid - service) / EQ30 < 0:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    check("INVARIANT 1.A.3 the equity cliff solved from equity cash equals debt service, amortising",
          solve_cliff(INST70).quantize(D("0.01")), INST70.quantize(D("0.01")))
    check("INVARIANT 1.A.3 the equity cliff solved from equity cash equals debt service, "
          "interest-only",
          solve_cliff(D(4200000)).quantize(D("0.01")), D(4200000))
    check("WE 1.2.3b Interpretation cushion overstatement factor of the interest-only reading",
          ((1 - D(4200000) / CASH) / (1 - COV70 / CASH)).quantize(D("0.01")), D("3.94"))
    check("INVARIANT 1.2.3b the interest-only cushion overstates the covenant threshold ~4x",
          D(1) if D("3.5") < (1 - D(4200000) / CASH) / (1 - COV70 / CASH) < D("4.5")
          else D(0), D(1))
    check("INVARIANT 1.2.3b amortisation more than halves the distance to zero equity cash",
          D(1) if (1 - INST70 / CASH) * 2 < (1 - D(4200000) / CASH) else D(0), D(1))
    check("INVARIANT 1.2.3b the covenant bites before the equity cliff, which bites before the "
          "interest-only cliff",
          D(1) if COV70 > INST70 > D(4200000) else D(0), D(1))
    check("Fig 1.2.2 spec all-equity return at the base case",
          (CASH / PROJ * 100).quantize(D("0.0001")), D("12.0000"))
    check("WE 1.2.3b Result table interest-only equity cash at 12,000,000",
          (CASH - D70 * RD).quantize(D("0.01")), D(7800000))
    check("WE 1.2.3b Interpretation / summary decline to the covenant, printed to two decimals",
          ((1 - COV70 / CASH) * 100).quantize(D("0.01")), D("16.51"))
    check("WE 1.2.3b Interpretation / summary decline to zero equity cash, two decimals",
          ((1 - INST70 / CASH) * 100).quantize(D("0.01")), D("30.42"))

    # ============================================================================== ethics ====
    FEE_AT_RISK = D(250000)
    FRANCHISE = D(3) * D(850000) * D(5)
    check("WE 1.3.2 step 3 jurisdictional franchise loss", FRANCHISE, D(12750000))
    LOSS = D(900000) + FRANCHISE + D(400000)
    check("WE 1.3.2 Result loss on discovery to the firm", LOSS, D(14050000))
    check("WE 1.3.2 Result breakeven discovery probability",
          (FEE_AT_RISK / LOSS * 100).quantize(D("0.0001")), D("1.7794"))
    ABORT = D(3) * D(1800000)
    check("Case study B abortive bid costs across three bidders", ABORT, D(5400000))
    OTHERS = D(1100000) + D(2100000) + ABORT
    check("WE 1.3.2 Result cost imposed on others", OTHERS, D(8600000))
    check("WE 1.3.2 Result total value destroyed", LOSS + OTHERS, D(22650000))
    check("WE 1.3.2 Result ratio of value destroyed to the saving",
          ((LOSS + OTHERS) / FEE_AT_RISK).quantize(D("0.0001")), D("90.6000"))
    for p, printed in [("0.10", "-1155000"), ("0.25", "-3262500"), ("0.50", "-6775000")]:
        check(f"WE 1.3.2 Result expected value of concealment at a {p} discovery probability",
              (FEE_AT_RISK - D(p) * LOSS).quantize(D("0.01")), D(printed))
    check("INVARIANT 1.3.2 the concealment EV is negative at every probability above the breakeven",
          D(1) if all(FEE_AT_RISK - D(p) * LOSS < 0 for p in ("0.02", "0.10", "0.25", "0.50"))
          else D(0), D(1))
    check("INVARIANT 1.3.2 what concealment buys is bounded by the fee at risk — no upside term",
          D(1) if FEE_AT_RISK < LOSS and FEE_AT_RISK < FRANCHISE else D(0), D(1))
    check("MCQ 1.3-D distractor B (forfeited fees only, franchise dropped)",
          (FEE_AT_RISK / D(900000) * 100).quantize(D("0.0001")), D("27.7778"))
    check("MCQ 1.3-D distractor C (franchise only, fees and legal cost dropped)",
          (FEE_AT_RISK / FRANCHISE * 100).quantize(D("0.0001")), D("1.9608"))

    # WE 1.3.3 — the cost of delay is DERIVED from ctx, not re-typed from Domain 13.
    COD_DAY = CFADS / 360                          # Domain 5 KA 5.4.2, 30/360 basis
    COD_WEEK = COD_DAY * 7
    check("WE 1.3.3 step 1 forgone CFADS per day on a 30/360 basis (Domain 5)",
          COD_DAY.quantize(D("0.01")), D("17733.33"))
    check("WE 1.3.3 step 1 cost of delay per calendar week (Domain 13 KA 13.1.3)",
          COD_WEEK.quantize(D("0.01")), D("124133.33"))
    COST9 = 9 * COD_WEEK
    check("WE 1.3.3 Result cost of the nine weeks", COST9.quantize(D("1")), D(1117200))
    MG = (D("0.0235") - D("0.0175")) * DEBT
    check("WE 1.3.3 step 3 annual value of the 60-basis-point difference",
          MG.quantize(D("0.01")), D(252000))
    PRIZE = MG * AF_PF
    check("WE 1.3.3 Result present value of the prize, had it been real",
          PRIZE.quantize(D("1")), D(2112729))
    check("WE 1.3.3 Result breakeven negotiating-success probability",
          (COST9 / PRIZE * 100).quantize(D("0.0001")), D("52.8795"))
    check("WE 1.3.3 Interpretation that breakeven printed to two decimals",
          (COST9 / PRIZE * 100).quantize(D("0.01")), D("52.88"))
    check("MCQ 1.3-E distractor C (one year of the margin difference, undiscounted)",
          MG.quantize(D("1")), D(252000))
    check("MCQ 1.3-E distractor D (a single week of delay)",
          COD_WEEK.quantize(D("1")), D(124133))

    # 1.A.2 — sponsor support as a contingent claim.
    SUPPORT = D("0.10") * CAPEX
    check("1.A.2 preamble cost-overrun support at 10 % of capital cost (Domain 5 KA 5.2.3)",
          SUPPORT, D(6000000))
    check("1.A.2 preamble effective recourse fraction (support / senior debt)",
          (SUPPORT / DEBT * 100).quantize(D("0.0001")), D("14.2857"))
    ANN_SUP = D("0.0040") * DEBT
    check("WE 1.A.2 step 3 annual margin saving at 40 basis points",
          ANN_SUP.quantize(D("0.01")), D(168000))
    VAL_SUP = ANN_SUP * AF_PF
    check("WE 1.A.2 Result present value of the margin saving",
          VAL_SUP.quantize(D("0.01")), D("1408485.78"))
    check("Case study A support valued, as printed", VAL_SUP.quantize(D("1")), D(1408486))
    check("WE 1.A.2 Result breakeven call probability against the full cap",
          (VAL_SUP / SUPPORT * 100).quantize(D("0.0001")), D("23.4748"))
    DRAW = SUPPORT * D("0.55")
    check("WE 1.A.2 step 3 expected draw at 55 % of the cap", DRAW.quantize(D("0.01")), D(3300000))
    check("WE 1.A.2 Result breakeven call probability against the expected draw",
          (VAL_SUP / DRAW * 100).quantize(D("0.0001")), D("42.6814"))
    check("INVARIANT 1.A.2 the two breakevens differ by exactly the inverse of the draw fraction",
          ((VAL_SUP / DRAW) / (VAL_SUP / SUPPORT)).quantize(D("0.000001")),
          (1 / D("0.55")).quantize(D("0.000001")))

    # =========================================================================== exercises ====
    # Exercise 1.2 (corrected by the expansion).
    IO80 = D(80000000) * D("0.065")
    check("Exercise 1.2 interest-only debt service at 80 % gearing", IO80.quantize(D("0.01")),
          D(5200000))
    check("Exercise 1.2 base-case levered return at 80 % gearing",
          ((CASH - IO80) / D(20000000) * 100).quantize(D("0.0001")), D("34.0000"))
    check("Exercise 1.2 cash decline at which equity income reaches zero",
          ((1 - IO80 / CASH) * 100).quantize(D("0.0001")), D("56.6667"))
    check("Exercise 1.2 points of extra base return over the 70/30 case",
          (D(34) - D(26)).quantize(D("0.0001")), D("8.0000"))

    # Exercise 1.4 — the recourse engine on a different facility.
    A58, A45, A810 = af(D("0.058"), 10), af(D("0.045"), 10), af(D("0.08"), 10)
    check("Exercise 1.4 AF(0.058,10)", A58.quantize(D("0.000001")), D("7.430333"))
    check("Exercise 1.4 AF(0.045,10)", A45.quantize(D("0.000001")), D("7.912718"))
    check("Exercise 1.4 AF(0.08,10)", A810.quantize(D("0.000001")), D("6.710081"))
    I_PF, I_C = D(30000000) / A58, D(30000000) / A45
    check("Exercise 1.4 project-route instalment", I_PF.quantize(D("0.01")), D("4037504.15"))
    check("Exercise 1.4 corporate-route instalment", I_C.quantize(D("0.01")), D("3791364.65"))
    check("Exercise 1.4 annual differential", (I_PF - I_C).quantize(D("0.01")), D("246139.50"))
    PV14 = (I_PF - I_C) * A810
    check("Exercise 1.4 PV of the differential", PV14.quantize(D("0.01")), D("1651616.08"))
    INC14 = PV14 + D(1700000)
    check("Exercise 1.4 incremental cost", INC14.quantize(D("0.01")), D("3351616.08"))
    check("Exercise 1.4 breakeven failure probability",
          (INC14 / D(7500000) * 100).quantize(D("0.0001")), D("44.6882"))
    check("Exercise 1.4 common error (cost intensity on the facility)",
          (INC14 / D(30000000) * 100).quantize(D("0.0001")), D("11.1721"))

    # Exercise 1.5 — the amortising cliff on the 80/20 structure.
    A65 = af(D("0.065"), 10)
    check("Exercise 1.5 AF(0.065,10)", A65.quantize(D("0.000001")), D("7.188830"))
    I15 = D(80000000) / A65
    check("Exercise 1.5 amortising instalment", I15.quantize(D("0.01")), D("11128375.20"))
    check("Exercise 1.5 equity cash", (CASH - I15).quantize(D("0.01")), D("871624.80"))
    check("Exercise 1.5 equity return on 20,000,000",
          ((CASH - I15) / D(20000000) * 100).quantize(D("0.0001")), D("4.3581"))
    check("Exercise 1.5 DSCR", (CASH / I15).quantize(D("0.0001")), D("1.0783"))
    check("Exercise 1.5 cash decline to zero equity cash",
          ((1 - I15 / CASH) * 100).quantize(D("0.0001")), D("7.2635"))

    # Exercise 1.6 — duration at 6 %.
    check("Exercise 1.6 ceiling at 6 %", (D("1.06") / D("0.06")).quantize(D("0.0001")), D("17.6667"))
    D20, D25_6 = dur_closed(D("0.06"), 20), dur_closed(D("0.06"), 25)
    check("Exercise 1.6 (a) duration of 20 level payments at 6 %",
          D20.quantize(D("0.0001")), D("8.6051"))
    check("Exercise 1.6 (b) duration of 25 level payments at 6 %",
          D25_6.quantize(D("0.0001")), D("10.0722"))
    check("Exercise 1.6 duration bought by five extra years of tenor",
          (D25_6 - D20).quantize(D("0.0001")), D("1.4671"))
    check("Exercise 1.6 common error (same 25-payment stream at 8 %)",
          dur_closed(DISC, 25).quantize(D("0.0001")), D("9.2254"))

    # Exercise 1.7 — coverage in days on a different project.
    check("Exercise 1.7 DSCR", (D(4800000) / D(4000000)).quantize(D("0.0001")), D("1.2000"))
    check("Exercise 1.7 covenant cash at 1.15x", (D("1.15") * D(4000000)).quantize(D("0.01")),
          D(4600000))
    HEAD17 = D(4800000) - D("1.15") * D(4000000)
    check("Exercise 1.7 headroom in currency", HEAD17.quantize(D("0.01")), D(200000))
    check("Exercise 1.7 headroom in days of revenue",
          (HEAD17 / D(9000000) * 365).quantize(D("0.0001")), D("8.1111"))
    check("Exercise 1.7 CFADS worth 0.01x of DSCR",
          (D("0.01") * D(4000000)).quantize(D("0.01")), D(40000))
    check("Exercise 1.7 days worth 0.01x of DSCR",
          (D("0.01") * D(4000000) / D(9000000) * 365).quantize(D("0.0001")), D("1.6222"))
    check("INVARIANT 1.7 headroom is measured from the covenant, not from the reported ratio",
          (D(4800000) - D(4000000) - HEAD17).quantize(D("0.01")), D(600000))

    # ==================================================================== shared structures ====
    # Every breakeven in the domain is a spend divided by what it avoids or buys. Re-deriving each
    # from that one shape is the invariant 1.A.3 claims.
    check("INVARIANT 1.A.3 recourse breakeven = incremental cost / exposure removed",
          (INCREMENTAL / EXPOSURE * 100).quantize(D("0.0001")), D("51.6392"))
    check("INVARIANT 1.A.3 disclosure breakeven = cost avoided / loss on discovery",
          (FEE_AT_RISK / LOSS * 100).quantize(D("0.0001")), D("1.7794"))
    check("INVARIANT 1.A.3 support breakeven = value bought / amount at risk",
          (VAL_SUP / SUPPORT * 100).quantize(D("0.0001")), D("23.4748"))
    check("INVARIANT 1.A.3 negotiating breakeven = cost incurred / value pursued",
          (COST9 / PRIZE * 100).quantize(D("0.0001")), D("52.8795"))
    check("INVARIANT 1.A.3 a breakeven above 100 % marks an impossible configuration, not a "
          "worthless idea (the 50 %-recovery case)",
          D(1) if INCREMENTAL / EXP50 > 1 else D(0), D(1))
    check("INVARIANT 1.A.3 cash-bridge signs: payables are a source, receivables a use",
          (D(2000000) - D(3000000) - D(1000000) + D(500000)), D(-1500000))
