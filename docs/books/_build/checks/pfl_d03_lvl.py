"""PFL-AI Domain 3 — judgement-item batch (Evaluation / Comprehension): golden checks.

Recomputes every number printed in the six items added to Domain 3's sample-MCQ blocks —
**MCQ 3.1-I, 3.2-J, 3.2-K, 3.3-J, 3.3-K, 3.3-L**. The items turn on judgement (which rate belongs to
which risk; what a closed form is and is not; whether a fee or a margin is dearer; whether a
real-versus-nominal difference is a range or a defect; whether an option may be priced at the mean;
what a day-count convention actually charges), but every figure in every option is reproduced here.

Levels as verified, not as drafted. 3.1-I and 3.3-K are Evaluation: the strongest distractor is
correct arithmetic serving the wrong objective (a rate chosen from the *use* of the money; a convex
payoff priced at its mean forecast), so the key turns on a criterion rather than a computation.
3.2-J and 3.3-L are Comprehension. **3.2-K and 3.3-J were drafted as Evaluation and are tagged
Analysis**: in 3.2-K the stem hands over the all-in cost, after which the cheaper facility is a
subtraction; in 3.3-J the Fisher identity makes the two consistent treatments equal, so the verdict
"defect, not a range" is forced. Both are sound items — they are error detection, not judgement
between defensible positions, and the invariants below are what make that forcing explicit.

Five engines:

  rate choice   (MCQ 3.1-I) one receipt discounted at three rates, and the 200-basis-point spread
                expressed against the mid value — the size of the judgement, in money.

  closed form   (MCQ 3.2-J) principal outstanding by `B_k = A x AF(r, n-k)` AND by running the
                schedule recursion on the printed instalment, so the item's claim that the two agree
                is demonstrated; the cash-still-to-pay distractor and the back-loading share follow.

  all-in cost   (MCQ 3.2-K) the fee facility's all-in rate is SOLVED (bisection on the actual
                stream against net proceeds), never quoted, and inverted to find the fee that
                equalises the two offers. The "amortise the fee" distractor is computed too, so the
                error's size is a number.

  Fisher        (MCQ 3.3-J) the two consistent treatments are shown equal to the cent, and the
                defect the audit found is reproduced as real flows discounted at the nominal rate.

  convexity     (MCQ 3.3-K) the indexation cap valued at the 4.0 % forecast and at a 5.0 % outturn,
                with level annual equivalents, plus the indexed-versus-level fixed leg of option D.

  day count     (MCQ 3.3-L) the 365/360 uplift, the equivalent quoted rate, one year of it on the
                facility balance, and the whole-schedule figure from a re-amortisation at the
                uplifted rate.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DEBT = ctx["KESTREL_DEBT"]                          # 42,000,000
    RATE = ctx["KESTREL_RATE"]                          # 6.0 %
    TENOR = ctx["KESTREL_TENOR"]                        # 12 years
    INST = ctx["KESTREL_INSTALMENT"]                    # 5,009,635.23 (printed, cent-rounded)
    DISC = ctx["KESTREL_DISCOUNT"]                      # 8.0 %
    LIFE = ctx["KESTREL_LIFE"]                          # 25 years

    # ===================== MCQ 3.1-I — the rate must match the flow's risk =====================
    REBATE, YRS = D(500000), 5
    pv8 = REBATE / (1 + DISC) ** YRS
    pv7 = REBATE / D("1.07") ** YRS
    pv6 = REBATE / (1 + RATE) ** YRS
    check("MCQ 3.1-I value at the 8.0 % appraisal rate", pv8.quantize(D("0.01")), D("340291.60"))
    check("MCQ 3.1-I value at the 6.0 % senior-debt rate", pv6.quantize(D("0.01")), D("373629.09"))
    check("MCQ 3.1-I value at the 7.0 % midpoint", pv7.quantize(D("0.01")), D("356493.09"))
    check("MCQ 3.1-I the 200 bp between the two proposals, in money",
          (pv6 - pv8).quantize(D("0.01")), D("33337.49"))
    check("MCQ 3.1-I that spread as a share of the mid value, %",
          ((pv6 - pv8) / pv7 * 100).quantize(D("0.0001")), D("9.3515"))
    check("INVARIANT MCQ 3.1-I the higher rate gives the lower value",
          D(1) if pv6 > pv7 > pv8 else D(0), D(1))
    # option D charges the same risk twice: an explicit haircut ON TOP of the riskier rate
    check("MCQ 3.1-I option D double-counts: a 5 % haircut at 8 % lands below every proposal",
          (REBATE * D("0.95") / (1 + DISC) ** YRS).quantize(D("0.01")), D("323277.02"))

    # ===================== MCQ 3.2-J — what the closed form is, and is not =====================
    K = 7
    af5 = af(RATE, TENOR - K)
    B7_CLOSED = INST * af5
    bal = DEBT                                          # schedule recursion on the printed instalment
    for _ in range(K):
        interest = (bal * RATE).quantize(D("0.01"))
        bal = (bal + interest - INST).quantize(D("0.01"))
    check("MCQ 3.2-J AF(0.06,5)", af5.quantize(D("0.000001")), D("4.212364"))
    check("MCQ 3.2-J B_7 by the closed form", B7_CLOSED.quantize(D("0.01")), D("21102406.02"))
    check("MCQ 3.2-J B_7 by the schedule recursion", bal, D("21102406.08"))
    check("MCQ 3.2-J the two routes agree to six cents",
          (bal - B7_CLOSED).quantize(D("0.01")), D("0.06"))
    check("MCQ 3.2-J as printed in the stem", B7_CLOSED.quantize(D("1")), D(21102406))
    check("MCQ 3.2-J option A cash still to be paid (5 remaining instalments)",
          (INST * 5).quantize(D("0.01")), D("25048176.15"))
    check("MCQ 3.2-J rationale: interest not yet accrued, which is not a liability",
          (INST * 5 - B7_CLOSED).quantize(D("0.01")), D("3945770.13"))
    check("MCQ 3.2-J option D principal still outstanding after seven of twelve years, %",
          (B7_CLOSED / DEBT * 100).quantize(D("0.0001")), D("50.2438"))
    check("MCQ 3.2-J option D share of the term elapsed, %",
          (D(K) / D(TENOR) * 100).quantize(D("0.01")), D("58.33"))
    check("INVARIANT MCQ 3.2-J an annuity is back-loaded: more than half remains past halfway",
          D(1) if B7_CLOSED / DEBT > D("0.5") else D(0), D(1))
    # option D's own model — level-principal retirement — would leave 5/12 outstanding
    check("MCQ 3.2-J option D describes level principal, which would leave, %",
          (D(5) / D(12) * 100).quantize(D("0.0001")), D("41.6667"))

    # ===================== MCQ 3.2-K — a fee is not a margin =====================
    FEE, RIVAL = D(840000), D("0.0615")
    PROCEEDS = DEBT - FEE

    def all_in(payment, proceeds, n=TENOR):
        """Solve Sigma payment/(1+r)^t = proceeds for r."""
        lo, hi = D("0.0001"), D("0.5")
        for _ in range(300):
            mid = (lo + hi) / 2
            if payment * af(mid, n) > proceeds:
                lo = mid
            else:
                hi = mid
        return lo

    check("MCQ 3.2-K instalment on the 6.00 % facility",
          (DEBT / af(RATE, TENOR)).quantize(D("0.01")), D("5009635.23"))
    check("MCQ 3.2-K net proceeds after the 2.0 % fee", PROCEEDS, 41160000)
    r_all = all_in(INST, PROCEEDS)
    check("MCQ 3.2-K all-in cost of the fee facility, %",
          (r_all * 100).quantize(D("0.0001")), D("6.3704"))
    check("MCQ 3.2-K solve verified by substitution (PV of the stream at r_all)",
          (INST * af(r_all, TENOR)).quantize(D("0.01")), PROCEEDS.quantize(D("0.01")))
    check("MCQ 3.2-K premium over the headline, bp",
          ((r_all - RATE) * 10000).quantize(D("0.01")), D("37.04"))
    check("MCQ 3.2-K the rival's all-in cost is exactly its coupon, %",
          (RIVAL * 100).quantize(D("0.0001")), D("6.1500"))
    check("MCQ 3.2-K option B the rival is cheaper by, bp",
          ((r_all - RIVAL) * 10000).quantize(D("0.01")), D("22.04"))
    fee_eq = DEBT - INST * af(RIVAL, TENOR)
    check("MCQ 3.2-K option B the fee that would equalise the two offers",
          fee_eq.quantize(D("0.01")), D("343243.74"))
    check("MCQ 3.2-K option B as printed", fee_eq.quantize(D("1")), D(343244))
    check("MCQ 3.2-K option B the excess the arranger is asking",
          (FEE - fee_eq).quantize(D("1")), D(496756))
    check("MCQ 3.2-K equalising fee as a share of the facility, %",
          (fee_eq / DEBT * 100).quantize(D("0.0001")), D("0.8172"))
    check("INVARIANT MCQ 3.2-K at the equalising fee the two offers price identically, bp",
          ((all_in(INST, DEBT - fee_eq) - RIVAL) * 10000).quantize(D("0.01")), D("0.00"))
    # the exchange rate the rationale carries: 1.00 % of fee is worth 18.38 bp on this facility
    r_420 = all_in(INST, DEBT - DEBT * D("0.01"))
    check("MCQ 3.2-K a 1.00 % (420,000) fee prices at, %",
          (r_420 * 100).quantize(D("0.000001")), D("6.183800"))
    check("MCQ 3.2-K exchange rate: bp of margin per 1.00 % of upfront fee",
          ((r_420 - RATE) * 10000).quantize(D("0.01")), D("18.38"))
    # option D — the "amortise the fee" error
    check("MCQ 3.2-K option D the fee divided by the tenor, bp",
          (FEE / DEBT / TENOR * 10000).quantize(D("0.01")), D("16.67"))
    check("MCQ 3.2-K option D understates the true cost by, bp",
          ((r_all - RATE) * 10000 - FEE / DEBT / TENOR * 10000).quantize(D("0.01")), D("20.38"))
    check("INVARIANT MCQ 3.2-K option D is under half the stream's own answer",
          D(1) if FEE / DEBT / TENOR * 2 < (r_all - RATE) else D(0), D(1))

    # ===================== MCQ 3.3-J — a difference is a defect, not a range =================
    SUPPORT, NOM_RATE, INFL = D(5600000), D("0.09"), D("0.03")
    I_REAL = (1 + NOM_RATE) / (1 + INFL) - 1
    af25_disc = af(DISC, LIFE)
    consistent = SUPPORT * af(I_REAL, LIFE)
    defect = SUPPORT * af(NOM_RATE, LIFE)
    check("MCQ 3.3-J the exact real rate, %", (I_REAL * 100).quantize(D("0.0001")), D("5.8252"))
    check("MCQ 3.3-J the consistent valuation (level real flows at the real rate)",
          consistent.quantize(D("1")), D(72791113))
    check("MCQ 3.3-J the figure the real model reported (real flows at the nominal rate)",
          defect.quantize(D("1")), D(55006446))
    check("MCQ 3.3-J the unexplained difference the modeller called a range",
          (consistent - defect).quantize(D("1")), D(17784667))
    check("MCQ 3.3-J that difference as a share of the correct value, %",
          ((defect / consistent - 1) * 100).quantize(D("0.0001")), D("-24.4325"))
    # the identity the item rests on: escalated nominal flows at the nominal rate give the same value
    # A payment worth SUPPORT in year-0 purchasing power is SUPPORT*(1+pi)^t in year-t money.
    nominal_route = sum(SUPPORT * (1 + INFL) ** D(t) / (1 + NOM_RATE) ** D(t)
                        for t in range(1, LIFE + 1))
    check("INVARIANT MCQ 3.3-J the two consistent treatments agree to the cent",
          (nominal_route - consistent).quantize(D("0.01")), D("0.00"))
    check("MCQ 3.3-J option D averaging a right number with a wrong one",
          ((consistent + defect) / 2).quantize(D("1")), D(63898779))

    # ===================== MCQ 3.3-K — an option may not be priced at the mean ================
    OM_BASE, CAP, FORECAST, STRESS = D(2700000), D("0.03"), D("0.04"), D("0.05")
    CONCESSION = D(500000)
    r_cap = (1 + DISC) / (1 + CAP) - 1

    def cap_value(outturn):
        r_out = (1 + DISC) / (1 + outturn) - 1
        return OM_BASE * (af(r_out, LIFE) - af(r_cap, LIFE))

    check("MCQ 3.3-K AF(0.08,25)", af25_disc.quantize(D("0.000001")), D("10.674776"))
    check("MCQ 3.3-K cap-equivalent discount rate at 3.0 %, %",
          (r_cap * 100).quantize(D("0.0001")), D("4.8544"))
    v4 = cap_value(FORECAST)
    v5 = cap_value(STRESS)
    check("MCQ 3.3-K cap value at the 4.0 % forecast", v4.quantize(D("1")), D(4258610))
    check("MCQ 3.3-K cap value as a level annual equivalent",
          (v4 / af25_disc).quantize(D("1")), D(398941))
    check("MCQ 3.3-K option A the apparent gain from the trade, per year",
          (CONCESSION - v4 / af25_disc).quantize(D("1")), D(101059))
    check("MCQ 3.3-K option B cap value at a 5.0 % outturn", v5.quantize(D("1")), D(9157382))
    check("MCQ 3.3-K option B its level annual equivalent",
          (v5 / af25_disc).quantize(D("1")), D(857852))
    check("INVARIANT MCQ 3.3-K the payoff is convex: 5 % is worth more than twice 4 %",
          D(1) if v5 > 2 * v4 else D(0), D(1))
    check("INVARIANT MCQ 3.3-K the cap pays nothing at or below the capped rate",
          cap_value(CAP).quantize(D("0.01")), D("0.00"))
    check("INVARIANT MCQ 3.3-K option A's trade fails in the stressed state",
          D(1) if CONCESSION < v5 / af25_disc else D(0), D(1))
    check("MCQ 3.3-K option D an indexed 500,000 concession, capped at 3.0 %",
          (CONCESSION * af(r_cap, LIFE)).quantize(D("1")), D(7150991))
    check("MCQ 3.3-K option D the same concession left level",
          (CONCESSION * af25_disc).quantize(D("1")), D(5337388))
    check("INVARIANT MCQ 3.3-K option D still leaves the payer short in the stressed state",
          D(1) if CONCESSION * af(r_cap, LIFE) < v5 else D(0), D(1))

    # ===================== MCQ 3.3-L — what actual/360 charges =====================
    UPLIFT = D(365) / D(360)
    r_360 = RATE * UPLIFT
    check("MCQ 3.3-L the uplift over a full year, %",
          ((UPLIFT - 1) * 100).quantize(D("0.0001")), D("1.3889"))
    check("MCQ 3.3-L the equivalent quoted rate on a 6.0 % facility, %",
          (r_360 * 100).quantize(D("0.0001")), D("6.0833"))
    check("MCQ 3.3-L the uplift in basis points at 6.0 %",
          ((r_360 - RATE) * 10000).quantize(D("0.01")), D("8.33"))
    check("MCQ 3.3-L one year of it on the 42,000,000 balance",
          (DEBT * (r_360 - RATE)).quantize(D("0.01")), D("35000.00"))
    check("INVARIANT MCQ 3.3-L the uplift is independent of rate, balance and tenor",
          ((D("0.0925") * UPLIFT / D("0.0925")) - (RATE * UPLIFT / RATE)).quantize(D("0.000001")),
          D("0.000000"))
    base_interest = INST * TENOR - DEBT
    inst_360 = DEBT / af(r_360, TENOR)
    interest_360 = inst_360 * TENOR - DEBT
    check("MCQ 3.3-L lifetime interest on the 6.0000 % annuity",
          base_interest.quantize(D("0.01")), D("18115622.76"))
    check("MCQ 3.3-L instalment re-amortised at the uplifted rate",
          inst_360.quantize(D("0.01")), D("5032547.52"))
    check("MCQ 3.3-L lifetime interest on the actual/360 basis",
          interest_360.quantize(D("0.01")), D("18390570.24"))
    check("MCQ 3.3-L extra interest across the twelve-year schedule",
          (interest_360 - base_interest).quantize(D("1")), D(274947))
    check("INVARIANT MCQ 3.3-L option D is inverted: the smaller denominator costs MORE",
          D(1) if interest_360 > base_interest else D(0), D(1))
    check("MCQ 3.3-L option A the numerator effect does reverse: a 92-day quarter at actual/360",
          (DEBT * RATE * D(92) / D(360)).quantize(D("0.01")), D("644000.00"))
    check("MCQ 3.3-L option A a 90-day quarter on the same basis",
          (DEBT * RATE * D(90) / D(360)).quantize(D("0.01")), D("630000.00"))
