"""PFL-AI Domain 1 — judgement-item batch (Evaluation / Comprehension): golden checks.

Recomputes every number printed in the six items added to Domain 1's sample-MCQ blocks to close the
book's cognitive-level gap — **MCQ 1.1-G, 1.1-H, 1.2-G, 1.2-H, 1.3-G, 1.3-H**. An Evaluation item
turns on a professional judgement rather than on arithmetic, but the arithmetic in it still has to be
right in *every* option: a distractor whose numbers cannot be reproduced gives the answer away.

Levels as verified, not as drafted. 1.2-G, 1.3-G and 1.3-H are Evaluation: every option is defensible
on its own terms and the key wins on a stated criterion, so the arithmetic cannot settle them.
1.1-H and 1.2-H are Comprehension: they restate a mechanism the candidate is not asked to compute.
**1.1-G was drafted as Evaluation and is tagged Analysis**, because its key is an impossibility
proof — the `(1 + r)/r` ceiling falsifies the paper's mechanism outright, and no distractor survives
that arithmetic — which is error detection rather than a choice between defensible positions.

Four engines carry the batch, and each printed figure is recomputed from its definition rather than
read back from the manuscript:

  duration   (MCQ 1.1-G) Macaulay duration of a level stream is computed by the closed form
             `(1+r)/r - n/((1+r)^n - 1)` AND by the definitional `Sigma t*PV / Sigma PV`, so the
             ceiling claim survives two routes. The three levers the item names are proved rather
             than asserted: a deferral adds *exactly* the deferral, escalation at g is the level
             duration at `r* = (1+r)/(1+g) - 1`, and no finite tenor reaches the ceiling.

  recourse   (MCQ 1.1-H, 1.3-H) `p*(D) = (fixed close-cost premium + D*margin coefficient) /
             (D*exposure coefficient)`. The two coefficients are derived from the worked example's
             own PV figures, never re-typed as percentages, so the hyperbola's markers and its
             asymptote are consequences. The 50 % recovery variant is solved from the year-three
             balance, which is where the exposure comes from.

  leverage   (MCQ 1.2-G, 1.2-H) the levered-return identity `r_e = r_u + (D/E)(r_u - r_d)` and the
             amortising equity cliff. Every threshold in 1.2-G is solved as a cash level and then
             expressed as a percentage decline, so the item's ranking of the objections (the
             covenant at -16.51 %, the equity cliff at -30.42 %, the paper's claim at -65.00 %) is
             computed, not quoted.

  delay      (MCQ 1.3-G) the nine-week cost of the unverified benchmark, from the weekly rate.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DISC = ctx["KESTREL_DISCOUNT"]                     # 8.0 % appraisal rate
    DEBT = ctx["KESTREL_DEBT"]                         # 42,000,000 senior facility
    TENOR = ctx["KESTREL_TENOR"]                       # 12 years

    def dur_closed(r, n):
        return (1 + r) / r - D(n) / ((1 + r) ** n - 1)

    def dur_sum(cfs, r, offset=0):
        """Definitional Macaulay duration; offset defers the whole stream by `offset` periods."""
        num = den = D(0)
        for k, cf in enumerate(cfs, start=1):
            t = D(k + offset)
            pv = cf / (1 + r) ** t
            num += t * pv
            den += pv
        return num / den

    # ===================== MCQ 1.1-G — the duration ceiling as a judgement =====================
    CEIL8 = (1 + DISC) / DISC
    D15, D25 = dur_closed(DISC, 15), dur_closed(DISC, 25)
    level15 = [D(8900000)] * 15
    check("MCQ 1.1-G ceiling (1+r)/r at 8 %", CEIL8.quantize(D("0.0001")), D("13.5000"))
    check("MCQ 1.1-G duration of the 15-year level stream, closed form",
          D15.quantize(D("0.0001")), D("6.5945"))
    check("MCQ 1.1-G duration of the 15-year level stream, definitional route",
          dur_sum(level15, DISC).quantize(D("0.0001")), D("6.5945"))
    check("MCQ 1.1-G duration at the 25-year tenor the paper proposes",
          D25.quantize(D("0.0001")), D("9.2254"))
    check("MCQ 1.1-G duration bought by extending 15 years to 25",
          (D25 - D15).quantize(D("0.0001")), D("2.6309"))
    check("MCQ 1.1-G gap still open against a 14-year liability at 25 years",
          (D(14) - D25).quantize(D("0.0001")), D("4.7746"))

    # lever 1 — a three-year deferral adds exactly three years
    check("MCQ 1.1-G rationale: a 3-year deferral adds exactly 3.0000 years",
          (dur_sum(level15, DISC, offset=3) - dur_sum(level15, DISC)).quantize(D("0.0001")),
          D("3.0000"))
    # lever 2 — escalation at g is the level duration at r* = (1+r)/(1+g) - 1
    G = D("0.025")
    RSTAR = (1 + DISC) / (1 + G) - 1
    indexed15 = [D(8900000) * (1 + G) ** D(k) for k in range(15)]
    check("MCQ 1.1-G escalation-equivalent rate r* at g = 2.5 %",
          (RSTAR * 100).quantize(D("0.0001")), D("5.3659"))
    check("MCQ 1.1-G duration of the 2.5 %-indexed stream, closed form at r*",
          dur_closed(RSTAR, 15).quantize(D("0.0001")), D("7.0342"))
    check("MCQ 1.1-G duration of the 2.5 %-indexed stream, definitional route",
          dur_sum(indexed15, DISC).quantize(D("0.0001")), D("7.0342"))
    check("MCQ 1.1-G rationale: duration added by 2.5 % escalation",
          (dur_closed(RSTAR, 15) - D15).quantize(D("0.0001")), D("0.4398"))
    # lever 3 — a lower rate lifts the ceiling
    check("MCQ 1.1-G rationale: the ceiling at 6 %",
          (D("1.06") / D("0.06")).quantize(D("0.0001")), D("17.6667"))
    # The shortfall from the ceiling is n/((1+r)^n - 1) > 0 for every finite n; the tenors tested
    # stop at 400 because beyond that the shortfall falls below 28-digit working precision.
    check("INVARIANT MCQ 1.1-G no finite tenor reaches the 8 % ceiling",
          D(1) if all(dur_closed(DISC, n) < CEIL8 for n in (15, 25, 40, 99, 400)) else D(0), D(1))
    check("INVARIANT MCQ 1.1-G duration rises monotonically toward the cap",
          D(1) if all(dur_closed(DISC, n) < dur_closed(DISC, n + 1)
                      for n in (15, 25, 40, 99)) else D(0), D(1))
    check("MCQ 1.1-G option B's claim is a cap, not a slow approach: 400 years",
          dur_closed(DISC, 400).quantize(D("0.0001")), D("13.5000"))

    # ===================== MCQ 1.1-H — why p* falls with facility size =====================
    FIXED = D(2359000)                      # close-cost premium, largely fixed
    MARGIN_PV = D(2843128)                   # PV of the 140 bp differential on 42,000,000
    EXPOSURE = D("10073997.27")              # year-three balance 34,073,997.27 less 24,000,000
    Y3_BALANCE = D("34073997.27")
    INCREMENTAL = D("5202128.16")
    m_coeff = MARGIN_PV / DEBT               # proportional cost per unit of debt
    e_coeff = EXPOSURE / DEBT                # proportional benefit per unit of debt

    def p_star(debt):
        return (FIXED + debt * m_coeff) / (debt * e_coeff)

    check("MCQ 1.1-H incremental cost is fixed premium plus margin PV",
          FIXED + MARGIN_PV, INCREMENTAL.quantize(D("1")))
    check("MCQ 1.1-H margin differential as a share of debt, %",
          (m_coeff * 100).quantize(D("0.0001")), D("6.7694"))
    check("MCQ 1.1-H exposure removed as a share of debt, %",
          (e_coeff * 100).quantize(D("0.0001")), D("23.9857"))
    check("MCQ 1.1-H p* on a 15,000,000 facility, %",
          (p_star(D(15000000)) * 100).quantize(D("0.0001")), D("93.7893"))
    check("MCQ 1.1-H p* on the 42,000,000 facility, %",
          (p_star(DEBT) * 100).quantize(D("0.0001")), D("51.6392"))
    check("MCQ 1.1-H asymptote — the margin differential alone, %",
          (m_coeff / e_coeff * 100).quantize(D("0.0001")), D("28.2224"))
    check("INVARIANT MCQ 1.1-H p* falls monotonically with facility size (option B's mechanism)",
          D(1) if all(p_star(D(a)) > p_star(D(b)) for a, b in
                      ((5000000, 15000000), (15000000, 42000000),
                       (42000000, 200000000), (200000000, 5000000000))) else D(0), D(1))
    check("INVARIANT MCQ 1.1-H p* never reaches the asymptote at any finite size",
          D(1) if all(p_star(D(s)) > m_coeff / e_coeff
                      for s in (15000000, 42000000, 10 ** 10)) else D(0), D(1))
    # option D reversed: a better recovery SHRINKS the exposure and so RAISES p*
    p50 = INCREMENTAL / (Y3_BALANCE - D(30000000))
    check("MCQ 1.1-H / 1.3-H exposure removed at a 50 % enforcement recovery",
          (Y3_BALANCE - D(30000000)).quantize(D("0.01")), D("4073997.27"))
    check("MCQ 1.1-H / 1.3-H p* at a 50 % recovery, %",
          (p50 * 100).quantize(D("0.0001")), D("127.6910"))
    check("INVARIANT MCQ 1.1-H a better recovery raises p* (option D reverses the direction)",
          D(1) if p50 > p_star(DEBT) else D(0), D(1))
    check("INVARIANT MCQ 1.1-H at a 50 % recovery the route cannot pay at any probability",
          D(1) if p50 > 1 else D(0), D(1))

    # ===================== MCQ 1.2-G — which objection is decisive =====================
    COST, PROJECT_CASH = D(100000000), D(12000000)
    LDEBT, LEQUITY, LRATE = D(70000000), D(30000000), D("0.06")
    COV = D("1.20")
    AF612 = af(LRATE, TENOR)
    INTEREST_ONLY = LDEBT * LRATE
    AMORTISING = LDEBT / AF612
    check("MCQ 1.2-G funding sums to the cost", LDEBT + LEQUITY, COST)
    check("MCQ 1.2-G AF(0.06,12)", AF612.quantize(D("0.000001")), D("8.383844"))
    check("MCQ 1.2-G interest-only debt service", INTEREST_ONLY, 4200000)
    check("MCQ 1.2-G amortising instalment", AMORTISING.quantize(D("0.01")), D("8349392.06"))
    check("MCQ 1.2-G year-one principal inside the instalment",
          (AMORTISING - INTEREST_ONLY).quantize(D("0.01")), D("4149392.06"))
    check("MCQ 1.2-G the paper's claimed cushion (interest-only equity cliff), %",
          ((PROJECT_CASH - INTEREST_ONLY) / PROJECT_CASH * 100).quantize(D("0.01")), D("65.00"))
    check("MCQ 1.2-G equity cliff on the amortising facility, %",
          ((PROJECT_CASH - AMORTISING) / PROJECT_CASH * 100).quantize(D("0.0001")), D("30.4217"))
    check("MCQ 1.2-G decline at which the 1.20x covenant engages, %",
          ((PROJECT_CASH - AMORTISING * COV) / PROJECT_CASH * 100).quantize(D("0.0001")),
          D("16.5061"))
    check("MCQ 1.2-G option B interest-only cash-on-cash return, %",
          ((PROJECT_CASH - INTEREST_ONLY) / LEQUITY * 100).quantize(D("0.0001")), D("26.0000"))
    check("MCQ 1.2-G option B amortising cash-on-cash return, %",
          ((PROJECT_CASH - AMORTISING) / LEQUITY * 100).quantize(D("0.0001")), D("12.1687"))
    check("MCQ 1.2-G rationale: reported resilience over the distance to the first consequence",
          (D("65.00") / D("16.5061")).quantize(D("0.01")), D("3.94"))
    check("INVARIANT MCQ 1.2-G the covenant is the FIRST consequence, before the equity cliff",
          D(1) if AMORTISING * COV > AMORTISING > INTEREST_ONLY else D(0), D(1))
    check("MCQ 1.2-G option D year-one interest is identical under both shapes",
          AMORTISING - (AMORTISING - INTEREST_ONLY), INTEREST_ONLY)

    # ===================== MCQ 1.2-H — reading the levered-return identity =====================
    DE = LDEBT / LEQUITY

    def r_e(r_u):
        return r_u + DE * (r_u - LRATE)

    check("MCQ 1.2-H debt-to-equity multiplier at 70/30", DE.quantize(D("0.000001")),
          D("2.333333"))
    check("MCQ 1.2-H spread term added at an unlevered 12.0 %",
          (DE * (D("0.12") - LRATE) * 100).quantize(D("0.0001")), D("14.0000"))
    check("MCQ 1.2-H levered return at an unlevered 12.0 %, %",
          (r_e(D("0.12")) * 100).quantize(D("0.0001")), D("26.0000"))
    check("MCQ 1.2-H spread term subtracted at an unlevered 4.0 %",
          (DE * (D("0.04") - LRATE) * 100).quantize(D("0.0001")), D("-4.6667"))
    check("MCQ 1.2-H levered return at an unlevered 4.0 %, %",
          (r_e(D("0.04")) * 100).quantize(D("0.0001")), D("-0.6667"))
    check("MCQ 1.2-H the crossover cash level (r_u = r_d)", COST * LRATE, 6000000)
    check("MCQ 1.2-H at the crossover gearing is exactly neutral, %",
          (r_e(LRATE) * 100).quantize(D("0.0001")), D("6.0000"))
    check("INVARIANT MCQ 1.2-H the identity reproduces the direct calculation at 12 %",
          r_e(D("0.12")), (PROJECT_CASH - INTEREST_ONLY) / LEQUITY)
    check("INVARIANT MCQ 1.2-H option A fails: below the crossover gearing subtracts",
          D(1) if r_e(D("0.04")) < D("0.04") and r_e(D("0.055")) < D("0.055") else D(0), D(1))

    # ===================== MCQ 1.3-G / 1.3-H — the cost of the unverified benchmark ==========
    COD_WEEK, WEEKS = D("124133.33"), 9
    check("MCQ 1.3-G cost of the nine-week delay",
          (COD_WEEK * WEEKS).quantize(D("1")), D(1117200))
    check("MCQ 1.3-H the single figure the board was shown",
          INCREMENTAL.quantize(D("1")), D(5202128))
    check("MCQ 1.3-H the base case rests on a 40 % recovery",
          (D(24000000) / D(60000000) * 100).quantize(D("0.01")), D("40.00"))
