"""PFL-AI Domain 3 extension — time value of money: golden checks for the NEW material.

The domain's original numbers are verified in `verify_formulas.py`; this module owns only the
material added by the depth expansion — twelve new worked examples (3.1.2b, 3.1.3b, 3.2.1d,
3.2.2c, 3.2.2d, 3.2.3b, 3.2.3c, 3.3.1b, 3.3.2b, 3.3.3b, 3.3.4b, 3.A.3b), the two new figures
(3.2.3, 3.3.3), eight new MCQ rationales, six new exercises, and the arithmetic added to the
Interpretation steps, the case studies, the toolkits and the summary.

Every value is recomputed from first principles at 28-digit Decimal precision. Master-thread
figures (the 42,000,000 facility, 6.0 % rate, 12-year tenor, 5,009,635.23 instalment, 6,384,000
CFADS, 8.0 % board discount rate, 60,000,000 envelope, 18,000,000 equity, 25-year life) are read
from ctx and never re-typed.

Six engines carry the new material:

  1. **The factor identities** (3.1.3b) — the annuity factor is the sum of the discount-factor row,
     so `AF(r,n) x r = 1 - DF(n)`, whence `AF(r,n) < 1/r` for every finite n and
     `AF(r,n) / (1/r) = 1 - DF(n)`. These are the reviewer's tie between a column and its summary
     cell, and they are what condemns an annuity factor above the reciprocal of the rate.
  2. **The inverse solves** (3.1.2b, 3.2.3b, 3.A.3b, Case study A/B) — a rate recovered from a price
     on a single flow in closed form, `r = (x/PV)^(1/n) - 1`; and a rate recovered from a *stream*
     by bisection, which is how an all-in cost, a PV-equivalent flat rate, a breakeven rate and an
     indifference rate are all obtained. Each solve is verified by substitution, not just reported.
  3. **The growth-adjusted rate** (3.2.1d, 3.3.2b, exercises 3.6/3.11) — an indexed stream is a level
     stream at `r* = (1+r)/(1+g) - 1`. Every closed form here is checked against direct summation of
     the escalated flows, and the double-count defect (escalate *and* discount at `r*`) is priced.
  4. **Schedule shapes and the balance path** (3.2.2c, 3.2.2d, Fig 3.2.3) — a fourth shape (the
     repayment holiday) and the outstanding-balance closed form `B_k = A x AF(r, n-k)`, each proved
     against a cent-rounded recursion, plus the half-life of principal under each shape.
  5. **Frequency, fees and day count** (3.2.3b, 3.2.3c, 3.3.4b, 3.A.1) — the EAR ladder and its
     shrinking increments; payment frequency (which lowers total interest) against compounding
     frequency (which raises effective cost); and actual/360 as an exact `365/360` uplift.
  6. **Consistency** (3.3.1b, 3.3.3b, 3.A.2) — the nominal and real treatments of one stream are
     equal to the cent; the two routes to a hedged FX value are equal to the cent (covered interest
     parity); and a mid-period reading is exactly `(1+r)^0.5` times a period-end reading.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]

    # ---------- master-thread anchors, read from ctx ----------
    DEBT = ctx["KESTREL_DEBT"]                    # 42,000,000
    EQUITY = ctx["KESTREL_EQUITY"]                # 18,000,000
    CAPEX = ctx["KESTREL_CAPEX"]                  # 60,000,000
    R = ctx["KESTREL_RATE"]                       # 0.06
    TEN = ctx["KESTREL_TENOR"]                    # 12
    LIFE = ctx["KESTREL_LIFE"]                    # 25
    INST = ctx["KESTREL_INSTALMENT"]              # 5,009,635.23 (printed)
    CF = ctx["KESTREL_CFADS"]                     # 6,384,000
    INT1 = ctx["KESTREL_INTEREST_Y1"]             # 2,520,000
    DISC = ctx["KESTREL_DISCOUNT"]                # 0.08

    # Domain-3 inputs the expanded intro now cites; asserted against ctx where derivable.
    AVAIL = D(5600000)          # 25-year availability payment (this domain's own master input)
    FEE = D(840000)             # arrangement fee inside the funding envelope (Domain 6, KA 6.2.1)
    OPEX = D(4500000)           # documented cash operating cost (Domain 2, KA 2.2)
    POWER = D(1800000)          # electricity element of OPEX (Domain 11, KA 11.2.2)

    check("D3ext intro: envelope = debt + equity", DEBT + EQUITY, CAPEX)
    check("D3ext intro: gearing 70 % debt", (DEBT / CAPEX * 100).quantize(D("0.01")), 70)
    check("D3ext intro: arrangement fee is 2.0 % of the facility",
          (FEE / DEBT * 100).quantize(D("0.01")), D("2.00"))
    check("D3ext WE 3.3.2b: O&M base = opex less power", OPEX - POWER, 2700000)
    check("D3ext master instalment recomputes from ctx rate/tenor",
          (DEBT * R / (1 - (1 + R) ** -TEN)).quantize(D("0.01")), INST)
    check("D3ext master DSCR at close", (CF / INST).quantize(D("0.0001")), D("1.2743"))

    # ---------- shared solvers ----------
    def bisect(f, lo, hi, iters=400):
        """Root of a continuous f that changes sign once on [lo, hi], with f(lo) < 0 < f(hi)."""
        lo, hi = D(lo), D(hi)
        for _ in range(iters):
            mid = (lo + hi) / 2
            if f(mid) < 0:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    def rate_from_af(target, n, lo="0.0001", hi="0.9"):
        """r with AF(r, n) = target. AF is decreasing in r, so bracket on (target - AF)."""
        return bisect(lambda x: target - af(x, n), lo, hi)

    def rate_from_stream(pv, flow, n, lo="0.0001", hi="0.9"):
        """r with sum(flow/(1+r)^t, t=1..n) = pv. Decreasing in r, so bracket on (pv - PV(r))."""
        return bisect(lambda x: pv - sum(flow / (1 + x) ** t for t in range(1, n + 1)), lo, hi)

    def pv_indexed(a, g, r, n):
        """Direct summation of an indexed stream — the independent route to A x AF(r*, n)."""
        return sum(a * (1 + g) ** t / (1 + r) ** t for t in range(1, n + 1))

    # =====================================================================================
    # 1. KA 3.1 — interpretation arithmetic, the inverse solve, the factor identities
    # =====================================================================================

    # ---- WE 3.1.1 Interpretation: the compound/simple ratio curve --------------------
    def ratio(n):
        return D("1.08") ** n / (1 + D("0.08") * n)

    for n, printed in ((3, "1.0159"), (5, "1.0495"), (7, "1.0986"), (8, "1.1286"),
                       (10, "1.1994"), (25, "2.2828")):
        check(f"WE 3.1.1 compound/simple ratio at year {n}",
              ratio(n).quantize(D("0.0001")), printed)
    check("WE 3.1.1 the ratio first passes 1.10 in year",
          bisect(lambda n: ratio(n) - D("1.10"), "1", "30").quantize(D("0.0001")), D("7.0493"))
    check("WE 3.1.1 1.08^25 multiple", (D("1.08") ** 25).quantize(D("0.000001")), D("6.848475"))
    check("WE 3.1.1 simple-interest 25-year multiple", 1 + D("0.08") * 25, 3)

    def ioi(n):                      # interest on interest, per unit of principal
        return D("1.08") ** n - 1 - D("0.08") * n

    check("WE 3.1.1 interest-on-interest at year 3", ioi(3).quantize(D("0.0001")), D("0.0197"))
    check("WE 3.1.1 interest-on-interest at year 8", ioi(8).quantize(D("0.0001")), D("0.2109"))
    check("WE 3.1.1 interest-on-interest passes 1.0 in year",
          bisect(lambda n: ioi(n) - 1, "1", "40").quantize(D("0.0001")), D("15.1679"))
    check("WE 3.1.1 interest-on-interest on 100,000 in the sixteenth year",
          (D(100000) * ioi(16)).quantize(D("1")), 114594)

    # ---- WE 3.1.2 Interpretation: the indifference price and its rate sensitivity ----
    REBATE = D(500000)
    pv7 = REBATE / D("1.07") ** 5
    pv6 = REBATE / D("1.06") ** 5
    pv8 = REBATE / D("1.08") ** 5
    check("WE 3.1.2 indifference price at 7 %", pv7.quantize(D("0.01")), D("356493.09"))
    check("WE 3.1.2 indifference price at 6 %", pv6.quantize(D("0.01")), D("373629.09"))
    check("WE 3.1.2 indifference price at 8 %", pv8.quantize(D("0.01")), D("340291.60"))
    check("WE 3.1.2 the 200 bp spread", (pv6 - pv8).quantize(D("0.01")), D("33337.49"))
    check("WE 3.1.2 spread as a share of the 7 % value",
          ((pv6 - pv8) / pv7 * 100).quantize(D("0.0001")), D("9.3515"))

    # ---- WE 3.1.2b — reading an offer as a rate --------------------------------------
    check("WE 3.1.2b substitution 500,000/380,000",
          (REBATE / D(380000)).quantize(D("0.000001")), D("1.315789"))
    check("WE 3.1.2b substitution 500,000/330,000",
          (REBATE / D(330000)).quantize(D("0.000001")), D("1.515152"))
    r380 = (REBATE / D(380000)) ** (D(1) / 5) - 1
    r330 = (REBATE / D(330000)) ** (D(1) / 5) - 1
    check("WE 3.1.2b rate implied by the 380,000 offer",
          (r380 * 100).quantize(D("0.0001")), D("5.6422"))
    check("WE 3.1.2b rate implied by the 330,000 offer",
          (r330 * 100).quantize(D("0.0001")), D("8.6654"))
    check("WE 3.1.2b identity check — the exact PV returns 7.0000 %",
          ((REBATE / pv7) ** (D(1) / 5) - 1) * 100, D("7.0000"))
    check("WE 3.1.2b invariant: the 330,000 offer beats the 7 % hurdle while 380,000 does not",
          1 if (r330 > D("0.07") > r380) else 0, 1)
    # MCQ 3.1-G distractors
    check("MCQ 3.1-G distractor A (premium over the future amount, then / 5)",
          (D(120000) / REBATE / 5 * 100).quantize(D("0.0001")), D("4.8000"))
    check("MCQ 3.1-G distractor C (total premium divided by five)",
          ((REBATE / D(380000) - 1) / 5 * 100).quantize(D("0.0001")), D("6.3158"))
    check("MCQ 3.1-G total premium quoted in the rationale",
          ((REBATE / D(380000) - 1) * 100).quantize(D("0.0001")), D("31.5789"))

    # ---- WE 3.1.3b — the factor row that checks itself -------------------------------
    dfs = [1 / (1 + R) ** t for t in range(1, TEN + 1)]
    check("WE 3.1.3b DF(1) at 6 %", dfs[0].quantize(D("0.000001")), D("0.943396"))
    check("WE 3.1.3b DF(2) at 6 %", dfs[1].quantize(D("0.000001")), D("0.889996"))
    check("WE 3.1.3b DF(12) at 6 %", dfs[11].quantize(D("0.000001")), D("0.496969"))
    row_sum = sum(dfs)
    check("WE 3.1.3b sum of the twelve factors", row_sum.quantize(D("0.000001")), D("8.383844"))
    check("WE 3.1.3b the sum equals the closed form AF(0.06,12)",
          row_sum.quantize(D("0.0000000001")), af(R, TEN).quantize(D("0.0000000001")))
    check("WE 3.1.3b full-precision AF(0.06,12)",
          af(R, TEN).quantize(D("0.0000000001")), D("8.3838439404"))
    check("WE 3.1.3b identity LHS  AF x r", (af(R, TEN) * R).quantize(D("0.0000000001")),
          D("0.5030306364"))
    check("WE 3.1.3b identity RHS  1 - DF(12)", (1 - dfs[11]).quantize(D("0.0000000001")),
          D("0.5030306364"))
    check("WE 3.1.3b DF(25) at 8 %", (1 / (1 + DISC) ** LIFE).quantize(D("0.0000000001")),
          D("0.1460179049"))
    af25 = af(DISC, LIFE)
    check("WE 3.1.3b AF(0.08,25)", af25.quantize(D("0.0000000001")), D("10.6747761886"))
    check("WE 3.1.3b identity at 8 %/25y  AF x r", (af25 * DISC).quantize(D("0.0000000001")),
          D("0.8539820951"))
    check("WE 3.1.3b identity at 8 %/25y  1 - DF(25)",
          (1 - 1 / (1 + DISC) ** LIFE).quantize(D("0.0000000001")), D("0.8539820951"))
    check("WE 3.1.3b perpetuity ceiling 1/r at 6 %", (1 / R).quantize(D("0.0001")), D("16.6667"))
    check("WE 3.1.3b perpetuity ceiling 1/r at 8 %", (1 / DISC).quantize(D("0.0001")), D("12.5000"))
    check("WE 3.1.3b AF(0.08,25) as a share of the perpetuity",
          (af25 / (1 / DISC) * 100).quantize(D("0.0001")), D("85.3982"))
    check("WE 3.1.3b AF(0.06,12) as a share of the perpetuity",
          (af(R, TEN) / (1 / R) * 100).quantize(D("0.0001")), D("50.3031"))
    check("WE 3.1.3b invariant: AF(r,n) is strictly below 1/r at both settings",
          1 if (af(R, TEN) < 1 / R and af25 < 1 / DISC) else 0, 1)
    af_rounded = (1 - D("0.4969")) / R
    check("WE 3.1.3b AF shift from a four-decimal DF(12)",
          (af_rounded - af(R, TEN)).quantize(D("0.0000001")), D("0.0011561"))
    check("WE 3.1.3b instalment cost of that rounding",
          (DEBT / af(R, TEN) - DEBT / af_rounded).quantize(D("1")), 691)
    # MCQ 3.1-H distractors
    check("MCQ 3.1-H distractor B  n x DF(n)", (12 * dfs[11]).quantize(D("0.000001")),
          D("5.963632"))
    check("MCQ 3.1-H distractor C  AF x DF(n)", (af(R, TEN) * dfs[11]).quantize(D("0.000001")),
          D("4.166514"))
    check("MCQ 3.1-H distractor D  1/r - DF(n)", (1 / R - dfs[11]).quantize(D("0.000001")),
          D("16.169697"))
    # self-check 4
    check("Self-check 3.1-4 largest annuity factor at 8 %", (1 / DISC).quantize(D("0.1")),
          D("12.5"))

    # =====================================================================================
    # 2. KA 3.2 — annuity conventions, the indexed stream, four schedule shapes, balances
    # =====================================================================================

    # ---- WE 3.2.1b Interpretation: the annuity-due invariant --------------------------
    check("WE 3.2.1b annuity-due / ordinary = (1 + r) exactly",
          (D("2156063.42") / D("1996355.02")).quantize(D("0.000001")), D("1.080000"))
    check("WE 3.2.1b invariant holds for arbitrary A and n (2.5m, 17y, 6 %)",
          ((D(2500000) * af(R, 17) * (1 + R)) / (D(2500000) * af(R, 17))).quantize(D("0.00000001")),
          (1 + R))
    check("WE 3.2.1b quarterly-in-advance factor is 1 + r/4, not 1 + r",
          (1 + DISC / 4).quantize(D("0.01")), D("1.02"))

    # ---- WE 3.2.1d — the indexed availability payment --------------------------------
    G = D("0.025")
    check("WE 3.2.1d first indexed payment", (AVAIL * (1 + G)).quantize(D("0.01")), 5740000)
    rstar = (1 + DISC) / (1 + G) - 1
    check("WE 3.2.1d growth-adjusted rate r*", (rstar * 100).quantize(D("0.000001")),
          D("5.365854"))
    check("WE 3.2.1d r* quoted to four places in MCQ 3.2-G", (rstar * 100).quantize(D("0.0001")),
          D("5.3659"))
    af_star = af(rstar, LIFE)
    check("WE 3.2.1d AF(r*,25)", af_star.quantize(D("0.000001")), D("13.591332"))
    pv_ix = AVAIL * af_star
    check("WE 3.2.1d indexed PV via the identity", pv_ix.quantize(D("0.01")), D("76111457.28"))
    check("WE 3.2.1d indexed PV by direct summation — the two routes agree to the cent",
          pv_indexed(AVAIL, G, DISC, LIFE).quantize(D("0.01")), D("76111457.28"))
    pv_level = AVAIL * af25
    check("WE 3.2.1d level PV of the same stream", pv_level.quantize(D("0.01")), D("59778746.66"))
    check("WE 3.2.1d value of indexation", (pv_ix - pv_level).quantize(D("1")), 16332711)
    check("WE 3.2.1d indexation uplift %", ((pv_ix / pv_level - 1) * 100).quantize(D("0.0001")),
          D("27.3219"))
    a_neutral = pv_level / af_star
    check("WE 3.2.1d value-neutral indexed base payment", a_neutral.quantize(D("0.01")),
          D("4398299.46"))
    check("WE 3.2.1d reshaping is value-neutral by construction",
          (a_neutral * af_star).quantize(D("0.01")), pv_level.quantize(D("0.01")))
    check("WE 3.2.1d reshaped first payment", (a_neutral * (1 + G)).quantize(D("1")), 4508257)
    check("WE 3.2.1d reshaped year-25 payment", (a_neutral * (1 + G) ** LIFE).quantize(D("1")),
          8154201)
    crossings = [t for t in range(1, LIFE + 1) if a_neutral * (1 + G) ** t > AVAIL]
    check("WE 3.2.1d reshaped stream first exceeds 5,600,000 in year", crossings[0], 10)
    check("WE 3.2.1d reshaped year-10 payment", (a_neutral * (1 + G) ** 10).quantize(D("1")),
          5630195)
    double = pv_indexed(AVAIL, G, rstar, LIFE)
    check("WE 3.2.1d double-count defect PV", double.quantize(D("1")), 99768254)
    check("WE 3.2.1d double-count overstatement", (double - pv_ix).quantize(D("1")), 23656797)
    check("WE 3.2.1d double-count overstatement %",
          ((double / pv_ix - 1) * 100).quantize(D("0.0001")), D("31.0818"))
    check("WE 3.2.1d reshaped year-1 payment is this much below the level offer",
          ((1 - a_neutral * (1 + G) / AVAIL) * 100).quantize(D("0.0001")), D("19.4954"))
    check("WE 3.2.1d nominal total of the reshaped stream",
          sum(a_neutral * (1 + G) ** t for t in range(1, LIFE + 1)).quantize(D("1")), 153991976)
    check("WE 3.2.1d nominal total of the level stream", AVAIL * LIFE, 140000000)
    check("MCQ 3.2-G distractor D  growing perpetuity A(1+g)/(r-g)",
          (AVAIL * (1 + G) / (DISC - G)).quantize(D("1")), 104363636)

    # ---- WE 3.2.2 Interpretation: the schedule's own invariants -----------------------
    A_full = DEBT * R / (1 - (1 + R) ** -TEN)
    check("WE 3.2.2 instalment at 28-digit precision (first 12 places)",
          A_full.quantize(D("0.000000000001")), D("5009635.233987"))
    # the cent-rounded schedule, run on the printed instalment
    bal, rows = DEBT, []
    for _ in range(TEN):
        interest = (bal * R).quantize(D("0.01"))
        principal = INST - interest
        bal -= principal
        rows.append((interest, principal, bal))
    check("WE 3.2.2 closing balance on the printed instalment is 0.06, not zero",
          rows[-1][2], D("0.06"))
    check("WE 3.2.2 year-1 interest", rows[0][0], INT1)
    check("WE 3.2.2 year-12 interest", rows[-1][0], D("283564.26"))
    check("WE 3.2.2 interest falls by a factor of",
          (rows[0][0] / rows[-1][0]).quantize(D("0.0001")), D("8.8869"))
    check("WE 3.2.2 year-1 principal", rows[0][1], D("2489635.23"))
    check("WE 3.2.2 year-12 principal", rows[-1][1], D("4726070.97"))
    check("WE 3.2.2 principal-column invariant  final/first = (1+r)^(n-1)",
          (rows[-1][1] / rows[0][1]).quantize(D("0.0001")),
          ((1 + R) ** (TEN - 1)).quantize(D("0.0001")))
    check("WE 3.2.2 (1+r)^11", ((1 + R) ** 11).quantize(D("0.0001")), D("1.8983"))
    check("WE 3.2.2 interest share of the first instalment",
          (INT1 / INST * 100).quantize(D("0.0001")), D("50.3031"))
    check("WE 3.2.2 that share equals 1 - DF(n) from WE 3.1.3b",
          ((1 - dfs[11]) * 100).quantize(D("0.0001")), D("50.3031"))
    plain_interest = TEN * A_full - DEBT
    check("WE 3.2.2 lifetime interest, annuity", plain_interest.quantize(D("0.01")),
          D("18115622.81"))

    # ---- WE 3.2.2b Interpretation: the two level-principal crossovers -----------------
    LP1 = DEBT / TEN + DEBT * R
    check("WE 3.2.2b level-principal year-1 service", LP1.quantize(D("0.01")), 6020000)
    check("WE 3.2.2b heavier than the annuity by", (LP1 - INST).quantize(D("0.01")),
          D("1010364.77"))
    check("WE 3.2.2b annual step down in level-principal service", DEBT / TEN * R, 210000)
    check("WE 3.2.2b exact payment parity year",
          (1 + (LP1 - INST) / (DEBT / TEN * R)).quantize(D("0.0001")), D("5.8113"))
    check("WE 3.2.2b payment crossover falls in year 6",
          6 if D("5.8113") < 6 else 0, 6)
    cum, cum10, cum12 = D(0), None, None
    for k in range(1, TEN + 1):
        svc = DEBT / TEN + (DEBT - DEBT / TEN * (k - 1)) * R
        cum += svc - A_full
        if k == 10:
            cum10 = cum
        if k == 12:
            cum12 = cum
    check("WE 3.2.2b cumulative cash still ahead at year 10", cum10.quantize(D("1")), 653648)
    check("WE 3.2.2b lifetime cash advantage of level principal",
          (-cum12).quantize(D("1")), 1735623)
    check("WE 3.2.2b that advantage equals the interest saving",
          (-cum12).quantize(D("0.01")),
          (plain_interest - D(16380000)).quantize(D("0.01")))
    check("WE 3.2.2b level-principal year-1 DSCR", (CF / LP1).quantize(D("0.0001")), D("1.0605"))
    check("WE 3.2.2b level-principal lifetime interest",
          sum((DEBT - DEBT / TEN * (k - 1)) * R for k in range(1, TEN + 1)).quantize(D("0.01")),
          16380000)

    # ---- WE 3.2.2c — the repayment holiday ------------------------------------------
    af9 = af(R, 9)
    check("WE 3.2.2c AF(0.06,9)", af9.quantize(D("0.000001")), D("6.801692"))
    A_hol = DEBT / af9
    check("WE 3.2.2c instalment for years 4-12", A_hol.quantize(D("0.01")), D("6174933.87"))
    hol_interest = 3 * DEBT * R + (9 * A_hol - DEBT)
    check("WE 3.2.2c lifetime interest with the holiday", hol_interest.quantize(D("0.01")),
          D("21134404.83"))
    check("WE 3.2.2c extra interest the holiday costs",
          (hol_interest - plain_interest).quantize(D("0.01")), D("3018782.02"))
    check("WE 3.2.2c year-1 DSCR (interest only)", (CF / (DEBT * R)).quantize(D("0.0001")),
          D("2.5333"))
    check("WE 3.2.2c year-4 DSCR (first amortising period)",
          (CF / A_hol).quantize(D("0.0001")), D("1.0339"))
    check("WE 3.2.2c the step up at year 4", ((A_hol / (DEBT * R) - 1) * 100).quantize(D("0.0001")),
          D("145.0371"))
    check("WE 3.2.2c CFADS needed for 1.30x at year 4", (A_hol * D("1.30")).quantize(D("0.01")),
          D("8027414.03"))
    check("WE 3.2.2c shortfall against 1.30x", (A_hol * D("1.30") - CF).quantize(D("0.01")),
          D("1643414.03"))
    check("WE 3.2.2c invariant: the binding period is the first amortising one, not the first",
          1 if (CF / A_hol) < (CF / (DEBT * R)) else 0, 1)

    # ---- WE 3.2.2d — outstanding balance, two routes --------------------------------
    af5 = af(R, 5)
    check("WE 3.2.2d AF(0.06,5)", af5.quantize(D("0.000001")), D("4.212364"))
    check("WE 3.2.2d B_7 by closed form", (INST * af5).quantize(D("0.01")), D("21102406.02"))
    check("WE 3.2.2d B_7 by cent-rounded recursion", rows[6][2], D("21102406.08"))
    check("WE 3.2.2d the two routes agree to six cents",
          (rows[6][2] - (INST * af5)).quantize(D("0.01")), D("0.06"))
    check("WE 3.2.2d B_3 by closed form", (INST * af(R, 9)).quantize(D("0.01")),
          D("34073997.24"))
    check("WE 3.2.2d B_3 by recursion", rows[2][2], D("34073997.29"))
    check("WE 3.2.2d B_5 by closed form", (INST * af(R, 7)).quantize(D("0.01")),
          D("27965694.73"))
    check("WE 3.2.2d B_5 by recursion", rows[4][2], D("27965694.78"))
    check("WE 3.2.2d B_7 as a share of the original principal",
          (INST * af5 / DEBT * 100).quantize(D("0.0001")), D("50.2438"))
    check("WE 3.2.2d 7 of 12 years elapsed, %", (D(7) / TEN * 100).quantize(D("0.01")),
          D("58.33"))
    check("WE 3.2.2d invariant: more than half the loan outstanding past mid-term",
          1 if (INST * af5 / DEBT) > D("0.5") else 0, 1)
    check("WE 3.2.2d remaining cash debt service after year 7", (5 * INST).quantize(D("0.01")),
          D("25048176.15"))
    check("WE 3.2.2d interest not yet accrued", (5 * INST - INST * af5).quantize(D("0.01")),
          D("3945770.13"))

    # ---- Fig 3.2.3 — the four balance paths ----------------------------------------
    bal, ann_path = DEBT, []
    for _ in range(TEN):
        bal -= A_full - bal * R
        ann_path.append(bal)
    printed_ann = [39510365, 36871351, 34073997, 31108802, 27965695, 24634001,
                   21102406, 17358915, 13390815, 9184628, 4726071, 0]
    for k, (computed, printed) in enumerate(zip(ann_path, printed_ann), start=1):
        check(f"Fig 3.2.3 annuity balance after year {k}", computed.quantize(D("1")), printed)
    check("Fig 3.2.3 annuity balance path ends at zero",
          ann_path[-1].quantize(D("0.01")), 0)
    bal, hol_path = DEBT, []
    for _ in range(9):
        bal -= A_hol - bal * R
        hol_path.append(bal)
    printed_hol = [38345066, 34470836, 30364153, 26011068, 21396798, 16505672,
                   11321078, 5825409, 0]
    for k, (computed, printed) in enumerate(zip(hol_path, printed_hol), start=4):
        check(f"Fig 3.2.3 holiday balance after year {k}", computed.quantize(D("1")), printed)
    check("Fig 3.2.3 level-principal balance at year 6", DEBT - 6 * DEBT / TEN, 21000000)
    check("Fig 3.2.3 header: level-principal lifetime interest", D(16380000), 16380000)
    check("Fig 3.2.3 header: bullet lifetime interest", (DEBT * R * TEN).quantize(D("0.01")),
          30240000)

    # ---- the balance-path reading passage ------------------------------------------
    check("Fig 3.2.3 passage: level-principal balance-years = interest / r",
          (D(16380000) / R).quantize(D("1")), 273000000)
    check("Fig 3.2.3 passage: half-life, level principal",
          (D(21000000) / (DEBT / TEN)).quantize(D("0.0001")), D("6.0000"))
    check("Fig 3.2.3 passage: half-life, annuity",
          bisect(lambda k: D(21000000) - A_full * af(R, TEN - k), "0", "12").quantize(D("0.0001")),
          D("7.0281"))
    check("Fig 3.2.3 passage: half-life, holiday",
          bisect(lambda k: D(21000000) - A_hol * af(R, TEN - k), "3", "12").quantize(D("0.0001")),
          D("8.0833"))
    check("Fig 3.2.3 passage: interest ordering matches the balance-area ordering",
          1 if D(16380000) < plain_interest < hol_interest < DEBT * R * TEN else 0, 1)

    # ---- WE 3.2.3 Interpretation: the shrinking compounding increments ---------------
    ears = {m: (1 + R / m) ** m - 1 for m in (1, 2, 4, 12, 365)}
    cont = R.exp() - 1
    check("WE 3.2.3 EAR, annual", (ears[1] * 100).quantize(D("0.0001")), D("6.0000"))
    check("WE 3.2.3 EAR, semi-annual", (ears[2] * 100).quantize(D("0.0001")), D("6.0900"))
    check("WE 3.2.3 EAR, quarterly", (ears[4] * 100).quantize(D("0.0001")), D("6.1364"))
    check("WE 3.2.3 EAR, monthly", (ears[12] * 100).quantize(D("0.0001")), D("6.1678"))
    check("WE 3.2.3 EAR, daily", (ears[365] * 100).quantize(D("0.0001")), D("6.1831"))
    check("WE 3.2.3 EAR, continuous", (cont * 100).quantize(D("0.0001")), D("6.1837"))
    check("WE 3.2.3 increment annual -> semi-annual, bp",
          ((ears[2] - ears[1]) * 10000).quantize(D("0.0001")), D("9.0000"))
    check("WE 3.2.3 increment semi-annual -> quarterly, bp",
          ((ears[4] - ears[2]) * 10000).quantize(D("0.0001")), D("4.6355"))
    check("WE 3.2.3 increment quarterly -> monthly, bp",
          ((ears[12] - ears[4]) * 10000).quantize(D("0.0001")), D("3.1426"))
    check("WE 3.2.3 increment monthly -> daily, bp",
          ((ears[365] - ears[12]) * 10000).quantize(D("0.0001")), D("1.5350"))
    check("WE 3.2.3 whole range annual -> continuous, bp",
          ((cont - R) * 10000).quantize(D("0.0001")), D("18.3655"))
    check("WE 3.2.3 the manuscript's claim that the first step alone is about half the range",
          1 if D(45) < (ears[2] - ears[1]) / (cont - R) * 100 < D(55) else 0, 1)
    check("WE 3.2.3 invariant: EAR >= i_nom, equality only at m = 1",
          1 if (ears[1] == R and ears[2] > R and ears[4] > R and ears[12] > R
                and ears[365] > R and cont > R) else 0, 1)

    # ---- WE 3.2.3b — the fee-inclusive all-in cost ---------------------------------
    net = DEBT - FEE
    check("WE 3.2.3b net proceeds", net, 41160000)
    r_all = rate_from_stream(net, INST, TEN)
    check("WE 3.2.3b all-in effective cost", (r_all * 100).quantize(D("0.000001")),
          D("6.370442"))
    check("WE 3.2.3b verified by substitution — the solved rate returns net proceeds",
          sum(INST / (1 + r_all) ** t for t in range(1, TEN + 1)).quantize(D("0.01")),
          net.quantize(D("0.01")))
    check("WE 3.2.3b premium over the 6.00 % headline, bp",
          ((r_all - R) * 10000).quantize(D("0.0001")), D("37.0442"))
    A615 = DEBT * D("0.0615") / (1 - D("1.0615") ** -TEN)
    check("WE 3.2.3b rival instalment at 6.15 %", A615.quantize(D("0.01")), D("5050913.67"))
    check("WE 3.2.3b rival instalment premium a year", (A615 - INST).quantize(D("0.01")),
          D("41278.44"))
    check("WE 3.2.3b the 6.15 % facility is cheaper by, bp",
          ((r_all - D("0.0615")) * 10000).quantize(D("0.01")), D("22.04"))
    r_1pct = rate_from_stream(DEBT - DEBT / 100, INST, TEN)
    check("WE 3.2.3b all-in cost of a 1.00 % fee", (r_1pct * 100).quantize(D("0.000001")),
          D("6.183800"))
    check("WE 3.2.3b fee-to-margin exchange rate, bp per 1.00 % of fee",
          ((r_1pct - R) * 10000).quantize(D("0.0001")), D("18.3800"))
    fee_eq = DEBT - INST * af(D("0.0615"), TEN)
    check("WE 3.2.3b fee that equalises the two offers", fee_eq.quantize(D("0.01")),
          D("343243.74"))
    check("WE 3.2.3b that fee as a % of the facility",
          (fee_eq / DEBT * 100).quantize(D("0.0001")), D("0.8172"))
    check("WE 3.2.3b excess of the arranger's fee over the market-equivalent",
          (FEE - fee_eq).quantize(D("0.01")), D("496756.26"))
    check("WE 3.2.3b invariant: all-in cost exceeds the contract rate whenever a fee is deducted",
          1 if r_all > R else 0, 1)
    check("MCQ 3.2-I distractor B (fee spread straight-line over the tenor)",
          ((R + D("0.02") / TEN) * 100).quantize(D("0.0001")), D("6.1667"))
    check("MCQ 3.2-I distractor D error size, bp",
          ((D("0.08") - r_all) * 10000).quantize(D("0.01")), D("162.96"))

    # ---- WE 3.2.3c — payment frequency against compounding frequency ---------------
    af24 = af(D("0.03"), 24)
    check("WE 3.2.3c AF(0.03,24)", af24.quantize(D("0.000001")), D("16.935542"))
    S = DEBT / af24
    check("WE 3.2.3c semi-annual instalment", S.quantize(D("0.01")), D("2479991.47"))
    check("WE 3.2.3c annual cash under semi-annual service", (2 * S).quantize(D("0.01")),
          D("4959982.94"))
    semi_interest = 24 * S - DEBT
    check("WE 3.2.3c lifetime interest, semi-annual", semi_interest.quantize(D("0.01")),
          D("17519795.28"))
    EAR2 = D("1.03") ** 2 - 1
    check("WE 3.2.3c EAR on 6 % semi-annual", (EAR2 * 100).quantize(D("0.01")), D("6.09"))
    A_wrong = DEBT * EAR2 / (1 - (1 + EAR2) ** -TEN)
    check("WE 3.2.3c the EAR-with-annual-periods instalment", A_wrong.quantize(D("0.01")),
          D("5034382.68"))
    check("WE 3.2.3c its lifetime interest", (TEN * A_wrong - DEBT).quantize(D("0.01")),
          D("18412592.21"))
    check("WE 3.2.3c overstatement of annual debt service",
          (A_wrong - 2 * S).quantize(D("0.01")), D("74399.74"))
    check("WE 3.2.3c overstatement of lifetime interest",
          ((TEN * A_wrong - DEBT) - semi_interest).quantize(D("0.01")), D("892796.93"))
    check("WE 3.2.3c semi-annual amortisation saves this much interest against annual",
          (plain_interest - semi_interest).quantize(D("0.01")), D("595827.53"))
    check("WE 3.2.3c invariant: more frequent amortisation lowers total interest",
          1 if semi_interest < plain_interest else 0, 1)
    check("WE 3.2.3c invariant: more frequent compounding raises effective cost",
          1 if ears[2] > ears[1] else 0, 1)
    r_equiv = rate_from_stream(DEBT, 2 * S, TEN)
    check("WE 3.2.3c annual-equivalent cost of the semi-annual shape",
          (r_equiv * 100).quantize(D("0.0001")), D("5.8188"))
    check("WE 3.2.3c that is this far below 6.00 %, bp",
          ((R - r_equiv) * 10000).quantize(D("0.0001")), D("18.1224"))
    check("WE 3.2.3c DSCR on semi-annual service", (CF / (2 * S)).quantize(D("0.0001")),
          D("1.2871"))
    check("WE 3.2.3c DSCR on the mis-specified schedule", (CF / A_wrong).quantize(D("0.0001")),
          D("1.2681"))

    # ---- KA 3.2 self-check additions ------------------------------------------------
    check("Self-check 3.2-5: a 1.00 % fee divided by the tenor would give, bp",
          (D("0.01") / TEN * 10000).quantize(D("0.01")), D("8.33"))
    check("Self-check 3.2-5: the true figure is 18.38 bp, not 8.33 — better than double",
          1 if (r_1pct - R) * 10000 > 2 * D("0.01") / TEN * 10000 else 0, 1)

    # =====================================================================================
    # 3. KA 3.3 — Fisher consistency, indexation mechanics, hedged FX, day count
    # =====================================================================================

    # ---- WE 3.3.1 Interpretation ----------------------------------------------------
    i_real = D("1.09") / D("1.03") - 1
    check("WE 3.3.1 exact real rate", (i_real * 100).quantize(D("0.0001")), D("5.8252"))
    check("WE 3.3.1 subtraction-shortcut error, bp",
          ((D("0.06") - i_real) * 10000).quantize(D("0.0001")), D("17.4757"))
    check("WE 3.3.1 exact real rate at 9 % nominal / 10 % inflation",
          ((D("1.09") / D("1.10") - 1) * 100).quantize(D("0.0001")), D("-0.9091"))
    check("WE 3.3.1 invariant: the shortcut always overstates i_real (omits i_real x pi)",
          1 if D("0.06") > i_real else 0, 1)

    # ---- WE 3.3.1b — the consistency rule priced four ways -------------------------
    af_real = af(i_real, LIFE)
    af_nom = af(D("0.09"), LIFE)
    check("WE 3.3.1b AF(i_real,25)", af_real.quantize(D("0.000001")), D("12.998413"))
    check("WE 3.3.1b AF(0.09,25)", af_nom.quantize(D("0.000001")), D("9.822580"))
    truth = pv_indexed(AVAIL, D("0.03"), D("0.09"), LIFE)
    check("WE 3.3.1b nominal flows at 9.0 % (direct summation)", truth.quantize(D("0.01")),
          D("72791112.66"))
    check("WE 3.3.1b real flows at 5.8252 % — identical to the cent",
          (AVAIL * af_real).quantize(D("0.01")), D("72791112.66"))
    check("WE 3.3.1b the consistency identity holds exactly",
          truth.quantize(D("0.000001")), (AVAIL * af_real).quantize(D("0.000001")))
    d1 = AVAIL * af_nom
    check("WE 3.3.1b defect 1 — real flows at the nominal rate", d1.quantize(D("0.01")),
          D("55006445.79"))
    check("WE 3.3.1b defect 1 error", (d1 - truth).quantize(D("1")), -17784667)
    check("WE 3.3.1b defect 1 error %", ((d1 / truth - 1) * 100).quantize(D("0.0001")),
          D("-24.4325"))
    check("WE 3.3.1b defect 1 in years of payment", ((truth - d1) / AVAIL).quantize(D("0.0001")),
          D("3.1758"))
    d2 = pv_indexed(AVAIL, D("0.03"), i_real, LIFE)
    check("WE 3.3.1b defect 2 — nominal flows at the real rate", d2.quantize(D("0.01")),
          D("100366400.14"))
    check("WE 3.3.1b defect 2 error", (d2 - truth).quantize(D("1")), 27575287)
    check("WE 3.3.1b defect 2 error %", ((d2 / truth - 1) * 100).quantize(D("0.0001")),
          D("37.8828"))
    check("WE 3.3.1b invariant: the two errors are not equal and opposite",
          1 if abs(d2 - truth) > abs(d1 - truth) else 0, 1)
    sc = AVAIL * af(D("0.06"), LIFE)
    check("WE 3.3.1b Fisher shortcut at 6.00 %", sc.quantize(D("0.01")), D("71586794.49"))
    check("WE 3.3.1b shortcut error", (sc - truth).quantize(D("1")), -1204318)
    check("WE 3.3.1b shortcut error %", ((sc / truth - 1) * 100).quantize(D("0.0001")),
          D("-1.6545"))
    check("WE 3.3.1b shortcut error in years of payment",
          ((truth - sc) / AVAIL).quantize(D("0.0001")), D("0.2151"))
    # Fig 3.3.3 prints the same five figures
    for label, value, printed in (("correct (nominal)", truth, 72791113),
                                  ("correct (real)", AVAIL * af_real, 72791113),
                                  ("defect 1", d1, 55006446),
                                  ("defect 2", d2, 100366400),
                                  ("shortcut", sc, 71586794)):
        check(f"Fig 3.3.3 bar — {label}", value.quantize(D("1")), printed)

    # ---- WE 3.3.2b — the cap, the lag and what each is worth -----------------------
    OM = OPEX - POWER                            # 2,700,000, tied to the master thread
    r_g4 = (1 + DISC) / D("1.04") - 1
    r_g3 = (1 + DISC) / D("1.03") - 1
    r_g5 = (1 + DISC) / D("1.05") - 1
    check("WE 3.3.2b r* at g = 4.0 %", (r_g4 * 100).quantize(D("0.000001")), D("3.846154"))
    check("WE 3.3.2b AF(r*,25) at g = 4.0 %", af(r_g4, LIFE).quantize(D("0.000001")),
          D("15.879244"))
    check("WE 3.3.2b r* at g = 3.0 %", (r_g3 * 100).quantize(D("0.000001")), D("4.854369"))
    check("WE 3.3.2b AF(r*,25) at g = 3.0 %", af(r_g3, LIFE).quantize(D("0.000001")),
          D("14.301981"))
    uncapped = OM * af(r_g4, LIFE)
    capped = OM * af(r_g3, LIFE)
    lagged = capped / D("1.03")
    check("WE 3.3.2b uncapped obligation", uncapped.quantize(D("1")), 42873960)
    check("WE 3.3.2b uncapped by direct summation — routes agree to the cent",
          pv_indexed(OM, D("0.04"), DISC, LIFE).quantize(D("0.01")),
          uncapped.quantize(D("0.01")))
    check("WE 3.3.2b capped obligation", capped.quantize(D("1")), 38615349)
    check("WE 3.3.2b capped and lagged obligation", lagged.quantize(D("1")), 37490630)
    check("WE 3.3.2b value of the cap", (uncapped - capped).quantize(D("0.01")),
          D("4258610.20"))
    check("WE 3.3.2b cap as a % of the uncapped obligation",
          ((uncapped - capped) / uncapped * 100).quantize(D("0.0001")), D("9.9329"))
    check("WE 3.3.2b value of the lag", (capped - lagged).quantize(D("0.01")), D("1124718.91"))
    check("WE 3.3.2b cap and lag together", (uncapped - lagged).quantize(D("1")), 5383329)
    check("WE 3.3.2b year-25 capped price", (OM * D("1.03") ** LIFE).quantize(D("1")), 5653200)
    check("WE 3.3.2b year-25 uncapped price", (OM * D("1.04") ** LIFE).quantize(D("1")), 7197758)
    check("WE 3.3.2b year-25 single-period saving",
          (OM * D("1.04") ** LIFE - OM * D("1.03") ** LIFE).quantize(D("1")), 1544558)
    check("WE 3.3.2b cap as a % of the capital envelope",
          ((uncapped - capped) / CAPEX * 100).quantize(D("0.01")), D("7.10"))
    check("WE 3.3.2b cap as a level annual equivalent",
          ((uncapped - capped) / af25).quantize(D("0.01")), D("398941.40"))
    check("WE 3.3.2b r* at g = 5.0 %", (r_g5 * 100).quantize(D("0.000001")), D("2.857143"))
    cap5 = OM * (af(r_g5, LIFE) - af(r_g3, LIFE))
    check("WE 3.3.2b cap value at a 5.0 % outturn", cap5.quantize(D("0.01")), D("9157381.81"))
    check("WE 3.3.2b cap value is convex in the outturn — more than double at 5 %",
          1 if cap5 > 2 * (uncapped - capped) else 0, 1)
    check("WE 3.3.2b the lag identity  1 - 1/(1+g)",
          ((1 - 1 / D("1.03")) * 100).quantize(D("0.0001")), D("2.9126"))
    check("WE 3.3.2b lag identity reproduces the lag value",
          (capped * (1 - 1 / D("1.03"))).quantize(D("0.01")), (capped - lagged).quantize(D("0.01")))
    # MCQ 3.3-H distractors
    check("MCQ 3.3-H distractor B (the cap as a level annual equivalent)",
          ((uncapped - capped) / af25).quantize(D("1")), 398941)
    check("MCQ 3.3-H distractor C (the year-25 saving, undiscounted)",
          (OM * D("1.04") ** LIFE - OM * D("1.03") ** LIFE).quantize(D("1")), 1544558)
    check("MCQ 3.3-H marked answer", (uncapped - capped).quantize(D("1")), 4258610)

    # ---- WE 3.3.3b — two routes to one hedged FX value ----------------------------
    SPOT = D("3.7500")
    I_USD, I_SAR = D("0.05"), D("0.055")
    C_SAR = D(20000000)
    af_sar = af(I_SAR, 5)
    check("WE 3.3.3b AF(0.055,5)", af_sar.quantize(D("0.000001")), D("4.270284"))
    pv_sar = C_SAR * af_sar
    check("WE 3.3.3b PV of the stream in SAR", pv_sar.quantize(D("0.01")), D("85405689.51"))
    route1 = pv_sar / SPOT
    check("WE 3.3.3b route 1 — discount then convert", route1.quantize(D("0.01")),
          D("22774850.54"))
    fwd_printed = ["3.7679", "3.7858", "3.8038", "3.8219", "3.8401"]
    usd_printed = [5308057, 5282900, 5257863, 5232944, 5208143]
    disc_printed = [5055292, 4791746, 4541940, 4305156, 4080717]
    route2 = D(0)
    for t in range(1, 6):
        fwd = SPOT * ((1 + I_SAR) / (1 + I_USD)) ** t
        usd = C_SAR / fwd
        dis = usd / (1 + I_USD) ** t
        route2 += dis
        check(f"WE 3.3.3b forward at year {t}", fwd.quantize(D("0.0001")), fwd_printed[t - 1])
        check(f"WE 3.3.3b USD receipt at year {t}", usd.quantize(D("1")), usd_printed[t - 1])
        check(f"WE 3.3.3b discounted USD at year {t}", dis.quantize(D("1")),
              disc_printed[t - 1])
    check("WE 3.3.3b route 2 — convert then discount", route2.quantize(D("0.01")),
          D("22774850.54"))
    check("WE 3.3.3b covered interest parity: the two routes agree to the cent",
          route1.quantize(D("0.000001")), route2.quantize(D("0.000001")))
    check("WE 3.3.3b invariant: the higher-rate currency trades at a forward discount",
          1 if SPOT * ((1 + I_SAR) / (1 + I_USD)) > SPOT else 0, 1)

    # ---- WE 3.3.4b — actual/360 over a year and across the facility ---------------
    UPLIFT = D(365) / 360
    year_a360 = DEBT * R * UPLIFT
    check("WE 3.3.4b one year of interest, actual/360", year_a360.quantize(D("0.01")), 2555000)
    check("WE 3.3.4b uplift over 30/360", (year_a360 - INT1).quantize(D("0.01")), 35000)
    check("WE 3.3.4b uplift as a % of all interest", ((UPLIFT - 1) * 100).quantize(D("0.0001")),
          D("1.3889"))
    r_a360 = R * UPLIFT
    check("WE 3.3.4b equivalent quoted rate", (r_a360 * 100).quantize(D("0.0001")), D("6.0833"))
    check("WE 3.3.4b equivalent rate premium, bp", ((r_a360 - R) * 10000).quantize(D("0.01")),
          D("8.33"))
    A_a360 = DEBT * r_a360 / (1 - (1 + r_a360) ** -TEN)
    check("WE 3.3.4b instalment on the actual/360 rate", A_a360.quantize(D("0.01")),
          D("5032547.52"))
    check("WE 3.3.4b instalment increase a year", (A_a360 - INST).quantize(D("0.01")),
          D("22912.29"))
    check("WE 3.3.4b lifetime interest on the actual/360 rate",
          (TEN * A_a360 - DEBT).quantize(D("0.01")), D("18390570.24"))
    extra_a360 = (TEN * A_a360 - DEBT) - plain_interest
    check("WE 3.3.4b extra lifetime interest", extra_a360.quantize(D("0.01")), D("274947.44"))
    check("WE 3.3.4b DSCR on the actual/360 instalment", (CF / A_a360).quantize(D("0.0001")),
          D("1.2685"))
    check("WE 3.3.4b DSCR reduction", (CF / INST - CF / A_a360).quantize(D("0.0001")),
          D("0.0058"))
    check("WE 3.3.4b one leap year of interest",
          (DEBT * R * D(366) / 360).quantize(D("0.01")), 2562000)
    headroom = CF - D("1.20") * INST
    check("WE 3.3.4b annual covenant headroom at 1.20x", headroom.quantize(D("0.01")),
          D("372437.72"))
    check("WE 3.3.4b extra interest as a % of that headroom",
          (extra_a360 / headroom * 100).quantize(D("0.01")), D("73.82"))
    check("WE 3.3.4b 366/360 uplift, %", ((D(366) / 360 - 1) * 100).quantize(D("0.0001")),
          D("1.6667"))
    check("WE 3.3.4b invariant: actual/360 interest = 30/360 interest x 365/360 over a full year",
          (INT1 * UPLIFT).quantize(D("0.01")), year_a360.quantize(D("0.01")))
    # MCQ 3.3-I distractors
    check("MCQ 3.3-I distractor C interest (366-day leap year)",
          (DEBT * R * D(366) / 360).quantize(D("0.01")), 2562000)
    check("MCQ 3.3-I distractor C rate", (R * D(366) / 360 * 100).quantize(D("0.0001")),
          D("6.1000"))
    check("MCQ 3.3-I distractor D interest (fraction inverted, 360/365)",
          (DEBT * R * D(360) / 365).quantize(D("0.01")), D("2485479.45"))
    check("MCQ 3.3-I distractor D rate", (R * D(360) / 365 * 100).quantize(D("0.0001")),
          D("5.9178"))

    # =====================================================================================
    # 4. Advanced topics — continuous equivalence, timing conventions, term structure
    # =====================================================================================

    check("3.A.1 100,000 continuously compounded for 3 years at 8 %",
          (D(100000) * D("0.24").exp()).quantize(D("0.01")), D("127124.92"))
    check("3.A.1 100,000 annually compounded for 3 years at 8 %",
          (D(100000) * D("1.08") ** 3).quantize(D("0.01")), D("125971.20"))
    check("3.A.1 continuous equivalent of 6 % annual  ln(1.06)",
          D("1.06").ln().quantize(D("0.0000000001")), D("0.0582689081"))
    check("3.A.1 that rate as a percentage", (D("1.06").ln() * 100).quantize(D("0.0001")),
          D("5.8269"))
    check("3.A.1 continuous equivalent of 8 % annual", (D("1.08").ln() * 100).quantize(D("0.0001")),
          D("7.6961"))
    check("3.A.1 monthly-to-continuous gap, bp",
          ((cont - ears[12]) * 10000).quantize(D("0.0001")), D("1.5873"))
    check("3.A.1 the confusable pair sit this far apart, bp",
          ((D("1.06").ln() - i_real) * 10000).quantize(D("0.0001")), D("0.1662"))
    check("3.A.1 both round to 5.83 % at two decimals",
          1 if (D("1.06").ln() * 100).quantize(D("0.01")) == (i_real * 100).quantize(D("0.01"))
          else 0, 1)

    check("3.A.2 the mid-period multiplier (1.08)^0.5",
          (D("1.08") ** D("0.5")).quantize(D("0.0000001")), D("1.0392305"))
    check("3.A.2 that multiplier as a percentage",
          ((D("1.08") ** D("0.5") - 1) * 100).quantize(D("0.0001")), D("3.9230"))
    check("3.A.2 the master stream on the period-end convention",
          pv_level.quantize(D("0.01")), D("59778746.66"))
    mid = pv_level * D("1.08") ** D("0.5")
    check("3.A.2 the same stream on the mid-period convention", mid.quantize(D("0.01")),
          D("62123895.85"))
    check("3.A.2 the convention is worth", (mid - pv_level).quantize(D("0.01")),
          D("2345149.20"))
    due = pv_level * (1 + DISC)
    check("3.A.2 the same stream read as an annuity-due", due.quantize(D("0.01")),
          D("64561046.39"))
    check("3.A.2 annuity-due / mid-period is the same (1.08)^0.5 step",
          (due / mid).quantize(D("0.0000001")), D("1.0392305"))
    check("3.A.2 invariant: mid-period / period-end = (1+r)^0.5 whatever the stream (17y, 6 %)",
          ((D(3300000) * af(R, 17) * (1 + R) ** D("0.5")) / (D(3300000) * af(R, 17)))
          .quantize(D("0.0000001")), ((1 + R) ** D("0.5")).quantize(D("0.0000001")))

    # ---- WE 3.A.3b — the flat rate against a curve, at concession scale -----------
    def curve_rate(t):
        return D("0.060") + D("0.0010") * (t - 1)

    check("3.A.3 curve one-year rate", (curve_rate(1) * 100).quantize(D("0.0001")), D("6.0000"))
    check("3.A.3 curve 25-year rate", (curve_rate(25) * 100).quantize(D("0.0001")), D("8.4000"))
    check("3.A.3 curve arithmetic mean (t = 13)", (curve_rate(13) * 100).quantize(D("0.0001")),
          D("7.2000"))
    pv_curve = sum(AVAIL / (1 + curve_rate(t)) ** t for t in range(1, LIFE + 1))
    check("WE 3.A.3b PV against the curve", pv_curve.quantize(D("0.01")), D("63564261.78"))
    check("WE 3.A.3b PV at the flat 8.0 %", pv_level.quantize(D("0.01")), D("59778746.66"))
    check("WE 3.A.3b the flat rate understates by", (pv_curve - pv_level).quantize(D("0.01")),
          D("3785515.13"))
    check("WE 3.A.3b understatement %", ((pv_curve / pv_level - 1) * 100).quantize(D("0.0001")),
          D("6.3325"))
    af_target = pv_curve / AVAIL
    check("WE 3.A.3b annuity-factor target", af_target.quantize(D("0.000001")), D("11.350761"))
    r_flat = rate_from_af(af_target, LIFE)
    check("WE 3.A.3b PV-equivalent flat rate", (r_flat * 100).quantize(D("0.0001")), D("7.2946"))
    check("WE 3.A.3b verified by substitution — it reproduces the curve's PV",
          (AVAIL * af(r_flat, LIFE)).quantize(D("0.01")), pv_curve.quantize(D("0.01")))
    check("WE 3.A.3b it sits this far below the rate the board used, bp",
          ((DISC - r_flat) * 10000).quantize(D("0.01")), D("70.54"))
    check("WE 3.A.3b invariant: the equivalent flat rate is not the curve's arithmetic mean",
          1 if r_flat > curve_rate(13) else 0, 1)
    check("WE 3.A.3b the drift exceeds twice Case study A's 1,778,747",
          1 if (pv_curve - pv_level) > 2 * D("1778746.66") else 0, 1)

    # =====================================================================================
    # 5. Case studies
    # =====================================================================================

    GRANT = D(58000000)
    r_be = rate_from_af(GRANT / AVAIL, LIFE)
    check("Case A breakeven annuity factor", (GRANT / AVAIL).quantize(D("0.000001")),
          D("10.357143"))
    check("Case A breakeven discount rate", (r_be * 100).quantize(D("0.0001")), D("8.3570"))
    check("Case A headroom above the 8.0 % used, bp",
          ((r_be - DISC) * 10000).quantize(D("0.01")), D("35.70"))
    af10 = af(D("0.10"), LIFE)
    check("Case A AF(0.10,25)", af10.quantize(D("0.000001")), D("9.077040"))
    k_ind = GRANT / af25
    g_ind = GRANT / af10
    check("Case A Kestrel's indifference payment", k_ind.quantize(D("0.01")), D("5433369.19"))
    check("Case A the grantor's indifference payment", g_ind.quantize(D("0.01")),
          D("6389748.19"))
    check("Case A the negotiating band", (g_ind - k_ind).quantize(D("0.01")), D("956379.00"))
    check("Case A where the 5,600,000 offer sits in the band, %",
          ((AVAIL - k_ind) / (g_ind - k_ind) * 100).quantize(D("0.0001")), D("17.4231"))
    joint = AVAIL * (af25 - af10)
    check("Case A joint gain from choosing the stream", joint.quantize(D("0.01")),
          D("8947322.55"))
    kestrel_gain = AVAIL * af25 - GRANT
    grantor_gain = GRANT - AVAIL * af10
    check("Case A Kestrel's share of the joint gain", kestrel_gain.quantize(D("0.01")),
          D("1778746.66"))
    check("Case A grantor's share", grantor_gain.quantize(D("0.01")), D("7168575.90"))
    check("Case A the two shares exhaust the joint gain",
          (kestrel_gain + grantor_gain).quantize(D("0.01")), joint.quantize(D("0.01")))
    check("Case A Kestrel's share, %", (kestrel_gain / joint * 100).quantize(D("0.0001")),
          D("19.8802"))
    check("Case A grantor's share, %", (grantor_gain / joint * 100).quantize(D("0.0001")),
          D("80.1198"))
    hybrid_stream = D(29000000) / af10
    check("Case A hybrid stream at the grantor's indifference price",
          hybrid_stream.quantize(D("0.01")), D("3194874.09"))
    hybrid_value = D(29000000) + hybrid_stream * af25
    check("Case A value of the hybrid package to Kestrel", hybrid_value.quantize(D("1")),
          63104566)
    check("Case A hybrid surplus", (hybrid_value - GRANT).quantize(D("1")), 5104566)
    check("Case A hybrid surplus as a multiple of the offer's",
          ((hybrid_value - GRANT) / kestrel_gain).quantize(D("0.0001")), D("2.8698"))

    # ---- Case study B ---------------------------------------------------------------
    B_P, B_R, B_N = D(30000000), D("0.075"), 7
    B_A = B_P * B_R / (1 - (1 + B_R) ** -B_N)
    check("Case B annuity instalment", B_A.quantize(D("0.01")), D("5664009.46"))
    check("Case B bullet interest less annuity interest",
          (B_P * B_R * B_N - (B_N * B_A - B_P)).quantize(D("0.01")), D("6101933.77"))
    adv = B_A - B_P * B_R
    dis = B_P + B_P * B_R - B_A
    check("Case B annual cash advantage of the bullet, years 1-6", adv.quantize(D("0.01")),
          D("3414009.46"))
    check("Case B year-7 cash disadvantage", dis.quantize(D("0.01")), D("26585990.54"))

    def diff_npv(x):
        return sum(adv / (1 + x) ** t for t in range(1, 7)) - dis / (1 + x) ** 7

    check("Case B the deferral's indifference rate is exactly the loan rate — NPV at 7.5 % = 0",
          diff_npv(B_R).quantize(D("0.01")), 0)
    check("Case B indifference rate, %",
          (bisect(lambda x: diff_npv(x), "0.001", "0.5") * 100).quantize(D("0.0001")),
          D("7.5000"))
    for rate, printed in (("0.10", 1226084), ("0.12", 2010232), ("0.15", 2925601),
                          ("0.20", 3933661)):
        check(f"Case B value of the deferral at {rate}", diff_npv(D(rate)).quantize(D("1")),
              printed)
    refi = B_P * D("0.098") / (1 - D("1.098") ** -7)
    check("Case B refinancing instalment at 9.8 %", refi.quantize(D("0.01")), D("6121645.60"))
    check("Case B annual increase on refinancing", (refi - B_A).quantize(D("0.01")),
          D("457636.14"))
    b6 = B_A * af(B_R, 1)
    check("Case B annuity balance at the end of year 6", b6.quantize(D("0.01")),
          D("5268846.01"))
    check("Case B that is this share of the bullet's exposure, %",
          (b6 / B_P * 100).quantize(D("0.0001")), D("17.5628"))

    # =====================================================================================
    # 6. New calculation exercises 3.6 - 3.11
    # =====================================================================================

    # Exercise 3.6 — indexed concession tariff
    e6_rs = D("1.09") / D("1.03") - 1
    check("Ex 3.6 r*", (e6_rs * 100).quantize(D("0.0001")), D("5.8252"))
    check("Ex 3.6 AF(r*,15)", af(e6_rs, 15).quantize(D("0.000001")), D("9.824117"))
    e6_pv = D(4000000) * af(e6_rs, 15)
    check("Ex 3.6 PV", e6_pv.quantize(D("0.01")), D("39296469.16"))
    check("Ex 3.6 PV by direct summation",
          pv_indexed(D(4000000), D("0.03"), D("0.09"), 15).quantize(D("0.01")),
          e6_pv.quantize(D("0.01")))
    check("Ex 3.6 AF(0.09,15)", af(D("0.09"), 15).quantize(D("0.000001")), D("8.060688"))
    e6_e1 = D(4000000) * af(D("0.09"), 15)
    check("Ex 3.6 error 1 (level annuity at 9 %)", e6_e1.quantize(D("0.01")), D("32242753.72"))
    check("Ex 3.6 error 1 understatement", (e6_pv - e6_e1).quantize(D("1")), 7053715)
    check("Ex 3.6 error 1 understatement %", ((1 - e6_e1 / e6_pv) * 100).quantize(D("0.01")),
          D("17.95"))
    e6_e2 = pv_indexed(D(4000000), D("0.03"), e6_rs, 15)
    check("Ex 3.6 error 2 (double-count)", e6_e2.quantize(D("1")), 48651797)
    check("Ex 3.6 error 2 overstatement", (e6_e2 - e6_pv).quantize(D("1")), 9355328)
    check("Ex 3.6 error 2 overstatement %", ((e6_e2 / e6_pv - 1) * 100).quantize(D("0.01")),
          D("23.81"))

    # Exercise 3.7 — outstanding balance after year 9
    check("Ex 3.7 AF(0.06,3)", af(R, 3).quantize(D("0.000001")), D("2.673012"))
    check("Ex 3.7 B_9 by formula", (INST * af(R, 3)).quantize(D("0.01")), D("13390814.83"))
    check("Ex 3.7 B_9 by cent-rounded recursion", rows[8][2], D("13390814.89"))
    e7_err = DEBT - 9 * D(2489635)
    check("Ex 3.7 the even-retirement error", e7_err.quantize(D("0.01")), 19593285)
    check("Ex 3.7 that error overstates the balance by",
          (e7_err - INST * af(R, 3)).quantize(D("0.01")), D("6202470.17"))

    # Exercise 3.8 — all-in cost on a short tenor
    e8_A = D(25000000) * D("0.055") / (1 - D("1.055") ** -8)
    check("Ex 3.8 instalment", e8_A.quantize(D("0.01")), D("3946600.30"))
    e8_net = D(25000000) * D("0.985")
    check("Ex 3.8 net proceeds", e8_net.quantize(D("0.01")), 24625000)
    e8_r = rate_from_stream(e8_net, e8_A, 8)
    check("Ex 3.8 all-in effective cost", (e8_r * 100).quantize(D("0.0001")), D("5.8794"))
    check("Ex 3.8 verified by substitution",
          sum(e8_A / (1 + e8_r) ** t for t in range(1, 9)).quantize(D("0.01")),
          e8_net.quantize(D("0.01")))
    check("Ex 3.8 premium over the 5.5 % coupon, bp",
          ((e8_r - D("0.055")) * 10000).quantize(D("0.01")), D("37.94"))
    check("Ex 3.8 named error: fee / tenor, bp", (D("0.015") / 8 * 10000).quantize(D("0.01")),
          D("18.75"))
    check("Ex 3.8 named error: the rate it produces",
          ((D("0.055") + D("0.015") / 8) * 100).quantize(D("0.0001")), D("5.6875"))
    check("Ex 3.8 invariant: the shorter tenor makes the fee dearer per year than on Kestrel",
          1 if (e8_r - D("0.055")) > (r_all - R) else 0, 1)

    # Exercise 3.9 — a two-year interest-only holiday
    check("Ex 3.9 AF(0.07,8)", af(D("0.07"), 8).quantize(D("0.000001")), D("5.971299"))
    e9_Ah = D(20000000) / af(D("0.07"), 8)
    check("Ex 3.9 instalment for years 3-10", e9_Ah.quantize(D("0.01")), D("3349355.25"))
    e9_plain = D(20000000) * D("0.07") / (1 - D("1.07") ** -10)
    check("Ex 3.9 plain ten-year instalment", e9_plain.quantize(D("0.01")), D("2847550.05"))
    e9_hi = 2 * D(1400000) + (8 * e9_Ah - D(20000000))
    check("Ex 3.9 lifetime interest with the holiday", e9_hi.quantize(D("0.01")),
          D("9594842.00"))
    check("Ex 3.9 lifetime interest without", (10 * e9_plain - D(20000000)).quantize(D("0.01")),
          D("8475500.55"))
    check("Ex 3.9 extra interest the holiday costs",
          (e9_hi - (10 * e9_plain - D(20000000))).quantize(D("1")), 1119341)
    check("Ex 3.9 instalment uplift %", ((e9_Ah / e9_plain - 1) * 100).quantize(D("0.01")),
          D("17.62"))

    # Exercise 3.10 — three-year forward
    e10_F = SPOT * ((1 + I_SAR) / (1 + I_USD)) ** 3
    check("Ex 3.10 three-year forward", e10_F.quantize(D("0.0001")), D("3.8038"))
    check("Ex 3.10 USD receivable at the forward",
          (D(10000000) / e10_F).quantize(D("0.01")), D("2628931.38"))
    check("Ex 3.10 USD at spot (the named error)",
          (D(10000000) / SPOT).quantize(D("0.01")), D("2666666.67"))
    check("Ex 3.10 the error overstates the receipt by",
          (D(10000000) / SPOT - D(10000000) / e10_F).quantize(D("0.01")), D("37735.29"))

    # Exercise 3.11 — valuing an indexation cap
    e11_r5 = D("1.08") / D("1.05") - 1
    e11_r3 = D("1.08") / D("1.03") - 1
    check("Ex 3.11 uncapped r*", (e11_r5 * 100).quantize(D("0.0001")), D("2.8571"))
    check("Ex 3.11 uncapped AF(.,10)", af(e11_r5, 10).quantize(D("0.000001")), D("8.592732"))
    e11_un = D(5000000) * af(e11_r5, 10)
    check("Ex 3.11 uncapped PV", e11_un.quantize(D("1")), 42963658)
    check("Ex 3.11 capped r*", (e11_r3 * 100).quantize(D("0.0001")), D("4.8544"))
    check("Ex 3.11 capped AF(.,10)", af(e11_r3, 10).quantize(D("0.000001")), D("7.776638"))
    e11_cap = D(5000000) * af(e11_r3, 10)
    check("Ex 3.11 capped PV", e11_cap.quantize(D("1")), 38883189)
    check("Ex 3.11 value of the cap", (e11_un - e11_cap).quantize(D("0.01")), D("4080469.31"))
    check("Ex 3.11 cap as a % of the uncapped obligation",
          ((e11_un - e11_cap) / e11_un * 100).quantize(D("0.01")), D("9.50"))
    check("Ex 3.11 named error (final-year price saving, undiscounted)",
          (D(5000000) * (D("1.05") ** 10 - D("1.03") ** 10)).quantize(D("0.01")),
          D("1424891.24"))

    # =====================================================================================
    # 7. Toolkits, Executive perspective, exam preparation, summary — restated values
    # =====================================================================================

    check("3.T.4 / exam prep: fee-to-margin equivalence on Kestrel, bp per 1 %",
          ((r_1pct - R) * 10000).quantize(D("0.01")), D("18.38"))
    check("3.T.4 / 3.T.1: day-count basis worth, bp", ((r_a360 - R) * 10000).quantize(D("0.01")),
          D("8.33"))
    check("Executive perspective: negotiating band", (g_ind - k_ind).quantize(D("0.01")),
          D("956379.00"))
    check("Executive perspective: offer position in the band, %",
          ((AVAIL - k_ind) / (g_ind - k_ind) * 100).quantize(D("0.0001")), D("17.4231"))
    check("Executive perspective: share of the joint gain captured, %",
          (kestrel_gain / joint * 100).quantize(D("0.0001")), D("19.8802"))
    check("Executive perspective: joint gain", joint.quantize(D("1")), 8947323)
    check("Executive perspective / exam prep: cap value", (uncapped - capped).quantize(D("1")),
          4258610)
    check("Executive perspective / exam prep: holiday cost",
          (hol_interest - plain_interest).quantize(D("1")), 3018782)
    check("Executive perspective / exam prep: day-count interest",
          extra_a360.quantize(D("1")), 274947)
    check("Executive perspective / exam prep: arrangement fee, bp",
          ((r_all - R) * 10000).quantize(D("0.01")), D("37.04"))
    check("Summary: mixing worlds moves value by -24.4325 %",
          ((d1 / truth - 1) * 100).quantize(D("0.0001")), D("-24.4325"))
    check("Summary: mixing worlds moves value by +37.8828 %",
          ((d2 / truth - 1) * 100).quantize(D("0.0001")), D("37.8828"))
    check("Summary: double-counted indexation overstates by 31.0818 %",
          ((double / pv_ix - 1) * 100).quantize(D("0.0001")), D("31.0818"))
    check("Summary: interest range across the four shapes, low",
          D(16380000).quantize(D("1")), 16380000)
    check("Summary: interest range across the four shapes, high",
          (DEBT * R * TEN).quantize(D("1")), 30240000)
    check("Summary: first-year coverage range, low", (CF / LP1).quantize(D("0.0001")),
          D("1.0605"))
    check("Summary: first-year coverage range, high", (CF / (DEBT * R)).quantize(D("0.0001")),
          D("2.5333"))
    # A row count is a structural claim, not an arithmetic one, and it stood here as
    # `check(..., 16, 16)` — a constant compared to itself, which can never fail. Structural
    # audits belong to the document generators (make_glossary / make_question_bank /
    # make_appendices), which do fail on defects; this suite checks arithmetic.
