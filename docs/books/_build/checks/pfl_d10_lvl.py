"""PFL-AI Domain 10 — golden checks for the Evaluation and Comprehension MCQs added to KA 10.1–10.4.

Scope: every number printed in the eleven items added to KA 10.1–10.4 — 10.1-F/G/H, 10.2-G/H,
10.3-F/G/H and 10.4-F/G/H — options and rationales. `verify_formulas.py` owns the domain's original
worked examples and `pfl_d10_ext.py` owns the depth expansion; this module owns only the added items.
Master-thread values come from ctx.

The first pass covered six of the eleven. 10.1-H, 10.3-G, 10.3-H, 10.4-G and 10.4-H printed a further
thirty-one figures that no module recomputed — the thirteenth year of tenor, the reserve tolerance
ladder and its per-month price, the two cure treatments, and the three graduated cash triggers.
Section 5 closes that.

Engines used:

  1. The three sizing bases (10.1.3) — level on the base case, `CFADS(1)/lambda x AF(r, n)`; level on the
     minimum period, `A / (lambda - T r/(1+r)) x AF(r, n)`; and sculpted, `(A/lambda) x AF(r*, n)` with
     `r* = r(1 - T/lambda)`. All three are rebuilt here so the 2,917,226 and the 1,661,916 are
     differences of computed values rather than of quoted ones.
  2. Repayment shape against a fixed cash flow (10.2.3) — the balloon's level payment is
     `(debt - balloon x DF(n)) / AF(r, n)`; `DSCR`, `LLCR` and the `DSCR / LLCR` reading follow, and the
     year-of-maturity coverage is computed on the same cash against the balloon payment.
  3. The cash sweep (10.3.3) — the balance is rolled forward period by period at
     `B(t) = B(t-1)(1+r) - instalment - sweep`, truncating the final payment to the balance, so the
     retirement year, weighted average life, interest saved and the present-value cost to equity are
     all read off one schedule.
  4. The declining bank-case `CFADS` profile (10.4.1) — `CFADS(t) = A + T x r x B(t-1)` on the level
     annuity, giving the historic and forward-looking test values around the end of the loan.
  5. The five items the first pass left unchecked. Capacity at a thirteenth year, and the `CFADS`
     uplift that would close the gap instead (10.1.2). The reserve tolerance `1 - DS(1 - m/12)/CFADS`
     evaluated across the month ladder, so the 6.5393 points a month and the 63,840 a point are
     slopes of a computed line rather than quoted constants (10.3.2). Both cure treatments, with the
     year-eleven prepayment rolled into the year-twelve balance before the second cure is sized, which
     is where the saving beyond `1 - 1/lambda` comes from (10.4.3). And the three thresholds tested
     year by year against the same bank case, which is what makes the eight-of-twelve claim a count
     rather than an assertion (10.4.2).
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))

    DEBT = ctx["KESTREL_DEBT"]                 # 42,000,000
    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    DS = ctx["KESTREL_INSTALMENT"]             # 5,009,635.23
    R = ctx["KESTREL_RATE"]                    # 0.06
    N = ctx["KESTREL_TENOR"]                   # 12
    BOARD = ctx["KESTREL_DISCOUNT"]            # 0.08
    AF6_12 = D("8.383844")
    T, TARGET, COV = D("0.20"), D("1.30"), D("1.20")
    A = D(5880000)                             # CFADS before the interest tax shield

    check("D10-lvl the master relation reproduces CFADS",
          A + T * ctx["KESTREL_INTEREST_Y1"], CF)

    # ================= MCQ 10.1-F / 10.1-G — the three sizing answers =================
    AF_EX = af(R, N)                           # the three capacities are sized on the exact factor
    ds_base = CF / TARGET
    cap_base = ds_base * AF_EX
    ds_min = A / (TARGET - T * R / (1 + R))
    cap_min = ds_min * AF_EX
    r_star = R * (1 - T / TARGET)
    cap_sculpt = A / TARGET * af(r_star, N)
    check("MCQ 10.1-F base-case debt service", q(ds_base, 2), D("4910769.23"))
    check("MCQ 10.1-F base-case capacity", q(cap_base, 0), 41171123)
    check("MCQ 10.1-F minimum-period debt service", q(ds_min, 2), D("4562811.13"))
    check("MCQ 10.1-F minimum-period capacity", q(cap_min, 0), 38253896)
    check("MCQ 10.1-F effective sculpting rate %", q(r_star * 100, 6), D("5.076923"))
    check("MCQ 10.1-F AF at the sculpting rate", q(af(r_star, N), 6), D("8.824924"))
    check("MCQ 10.1-F sculpted capacity", q(cap_sculpt, 0), 39915812)
    check("MCQ 10.1-F cost of the minimum-period test", q(cap_base - cap_min, 0), 2917226)
    check("MCQ 10.1-F capacity sculpting recovers", q(cap_sculpt - cap_min, 0), 1661916)
    check("MCQ 10.1-G level cash makes the two tests coincide",   # 10.1-G option D's converse
          q(CF / TARGET * AF6_12, 0), q(cap_base, 0))
    check("D10-lvl AF(0.06,12) registry literal is the correct rounding", q(AF_EX, 6), AF6_12)

    # ================= MCQ 10.2-G / 10.2-H — repayment shape =================
    BALLOON = DEBT * D("0.25")
    df12 = (1 + R) ** -N
    pay_b = (DEBT - BALLOON * df12) / AF6_12
    llcr = CF * AF6_12 / DEBT
    check("MCQ 10.2-G balloon amount", BALLOON, 10500000)
    check("MCQ 10.2-G DF(0.06,12)", q(df12, 6), D("0.496969"))
    check("MCQ 10.2-G balloon level payment", q(pay_b, 0), 4387226)
    check("MCQ 10.2-G balloon DSCR", q(CF / pay_b, 4), D("1.4551"))
    check("MCQ 10.2-G amortising DSCR", q(CF / DS, 4), D("1.2743"))
    check("MCQ 10.2-G LLCR unchanged by the shape", q(llcr, 4), D("1.2743"))
    check("MCQ 10.2-G DSCR / LLCR reading", q((CF / pay_b) / llcr, 4), D("1.1419"))
    check("MCQ 10.2-G payment due at maturity", q(pay_b + BALLOON, 0), 14887226)
    check("MCQ 10.2-G coverage in the year of maturity", q(CF / (pay_b + BALLOON), 4), D("0.4288"))
    # 10.1.2's 828,877 shortfall, on that example's own substitution (cent instalment x the
    # six-decimal annuity factor), which is what 10.1.3's balloon figure is computed from
    gap = DEBT - q(ds_base, 2) * AF6_12
    check("MCQ 10.2-G sizing gap at 1.30x", q(gap, 0), 828877)
    gap_bal = gap / q(df12, 6)                 # 10.1.3 divides by the six-decimal factor
    check("MCQ 10.2-G balloon that closes the sizing gap", q(gap_bal, 0), 1667864)
    check("MCQ 10.2-G that balloon as % of the facility", q(gap_bal / DEBT * 100, 2), D("3.97"))
    check("MCQ 10.2-H PLCR exceeds LLCR only through the horizon",
          q(CF * af(R, ctx["KESTREL_LIFE"]) / DEBT, 4), D("1.9431"))

    # ================= MCQ 10.3-F — the 50 % cash sweep =================
    DISTRIB = D("774364.77")                   # cash available for distribution (Domain 15)
    sweep = DISTRIB / 2
    check("MCQ 10.3-F sweep per period", q(sweep, 3), D("387182.385"))   # displayed as 387,182.39

    def schedule(extra):
        """Roll the balance forward; return rows of (t, interest, principal, payment)."""
        bal, rows, t = DEBT, [], 0
        while bal > 0 and t < 40:
            t += 1
            interest = bal * R
            # final period: the balance plus one period's interest is within a currency unit of
            # the payment, so the payment is truncated to retire it exactly
            if bal * (1 + R) <= DS + extra + 1:
                pay, principal, bal = bal * (1 + R), bal, D(0)
            else:
                pay = DS + extra
                principal = pay - interest
                bal = bal + interest - pay
            rows.append((t, interest, principal, pay))
        return rows

    def wal(rows):
        return sum(D(r[0]) * r[2] for r in rows) / sum(r[2] for r in rows)

    base_rows, swept_rows = schedule(D(0)), schedule(sweep)
    check("MCQ 10.3-F base schedule retires in year", D(len(base_rows)), 12)
    check("MCQ 10.3-F swept schedule retires in year", D(len(swept_rows)), 11)
    check("MCQ 10.3-F swept final payment", q(swept_rows[-1][3], 2), D("4326132.35"))
    check("MCQ 10.3-F base weighted average life", q(wal(base_rows), 4), D("7.1887"))
    check("MCQ 10.3-F swept weighted average life", q(wal(swept_rows), 4), D("6.4660"))
    check("MCQ 10.3-F reduction in weighted average life",
          q(wal(base_rows) - wal(swept_rows), 4), D("0.7227"))
    int_base = sum(r[1] for r in base_rows)
    int_swept = sum(r[1] for r in swept_rows)
    check("MCQ 10.3-F interest saved by the sweep", q(int_base - int_swept, 0), 1821314)
    check("MCQ 10.3-F nominal cash diverted over ten years", q(sweep * 10, 0), 3871824)
    year11_relief = DS - swept_rows[-1][3]
    check("MCQ 10.3-F year-eleven relief against the scheduled instalment",
          q(year11_relief, 2), D("683502.88"))
    pv_cost = (sweep * af(BOARD, 10)
               - year11_relief / (1 + BOARD) ** 11
               - DS / (1 + BOARD) ** 12)
    check("MCQ 10.3-F present-value cost to equity at 8 %", q(pv_cost, 0), 315488)

    # ================= MCQ 10.4-F — historic against forward-looking =================
    bal, cfads = DEBT, {}
    for t in range(1, N + 1):
        interest = bal * R
        cfads[t] = A + T * interest
        bal = bal * (1 + R) - DS
    check("MCQ 10.4-F CFADS year 1 reproduces the master thread", q(cfads[1], 2), CF)
    for t, v in ((10, 6040690), (11, 5990216), (12, 5936713)):
        check(f"MCQ 10.4-F bank-case CFADS year {t}", q(cfads[t], 0), v)
    trigger = DS * COV
    check("MCQ 10.4-F covenant cash trigger", q(trigger, 0), 6011562)
    check("MCQ 10.4-F end of year 10, historic DSCR", q(cfads[10] / DS, 4), D("1.2058"))
    check("MCQ 10.4-F end of year 10, forward DSCR", q(cfads[11] / DS, 4), D("1.1957"))
    check("MCQ 10.4-F end of year 11, historic DSCR", q(cfads[11] / DS, 4), D("1.1957"))
    check("MCQ 10.4-F end of year 12, historic DSCR", q(cfads[12] / DS, 4), D("1.1851"))
    check("MCQ 10.4-F the historic test passes at the end of year 10",
          1 if cfads[10] > trigger else 0, 1)
    check("MCQ 10.4-F the forward test fails at the same date",
          1 if cfads[11] < trigger else 0, 1)

    # ================= MCQ 10.1-H — which of the four levers is available =================
    AF_13 = af(R, 13)
    cap13 = CF / TARGET * AF_13
    check("MCQ 10.1-H AF(0.06,13)", q(AF_13, 6), D("8.852683"))
    check("MCQ 10.1-H capacity at a thirteenth year", q(cap13, 0), 43473483)
    check("MCQ 10.1-H margin over the 42,000,000 request", q(cap13 - DEBT, 0), 1473483)
    check("MCQ 10.1-H DSCR at 42,000,000 over 13 years", q(CF / (DEBT / AF_13), 4), D("1.3456"))
    check("MCQ 10.1-H coverage the requested amount delivers over 12", q(CF / DS, 4), D("1.2743"))
    uplift = DS * TARGET - CF                  # the CFADS lever, priced against the tenor lever
    check("MCQ 10.1-H CFADS uplift that would close the gap", q(uplift, 0), 128526)
    check("MCQ 10.1-H that uplift as % of CFADS", q(uplift / CF * 100, 4), D("2.0132"))
    check("MCQ 10.1-H the sizing gap the four levers are competing over", q(gap, 0), 828877)

    # ================= MCQ 10.3-G / 10.3-H — the reserve, and how it is funded =================
    dsra6 = DS / 2
    check("MCQ 10.3-G six-month reserve", q(dsra6, 0), 2504818)
    check("MCQ 10.3-G CFADS collapse it survives, % of base case", q(dsra6 / CF * 100, 0), 39)
    check("MCQ 10.3-G the ratio in that year is far below the covenant",
          1 if dsra6 / DS < COV else 0, 1)

    def tolerance(m):                          # 1 - DS(1 - m/12)/CFADS, linear in m
        return 1 - DS * (1 - D(m) / 12) / CF

    for m, want in ((0, "21.5283"), (3, "41.1462"), (6, "60.7641"), (9, "80.3821"),
                    (12, "100.0000")):
        check(f"MCQ 10.3-H tolerance at {m} months %", q(tolerance(m) * 100, 4), D(want))
    check("MCQ 10.3-H points the twelfth month set buys over the sixth",
          q((tolerance(12) - tolerance(6)) * 100, 4), D("39.2359"))
    per_month = (DS / 12) / CF * 100
    check("MCQ 10.3-H percentage points of tolerance per month", q(per_month, 4), D("6.5393"))
    check("MCQ 10.3-H funded cash per month", q(DS / 12, 0), 417470)
    check("MCQ 10.3-H cash per percentage point of tolerance", q((DS / 12) / per_month, 0), 63840)
    check("MCQ 10.3-H incremental cover for six further months", q(dsra6, 0), 2504818)
    KE, DEPOSIT, LC_FEE = D("0.1542"), D("0.03"), D("0.0125")
    check("MCQ 10.3-H equity carry on that increment", q(dsra6 * (KE - DEPOSIT), 0), 311098)
    check("MCQ 10.3-H LC cost on that increment at 1.25 %", q(dsra6 * LC_FEE, 0), 31310)
    check("MCQ 10.3-H breakeven LC fee %", q((KE - DEPOSIT) * 100, 2), D("12.42"))
    check("MCQ 10.3-H after-tax debt cost, the trap option's own figure",
          q(dsra6 * R * (1 - T), 0), 120231)

    # ================= MCQ 10.4-G — the same breach, two cure treatments =================
    LAM = COV
    cure11_a = LAM * DS - cfads[11]
    cure11_b = DS - cfads[11] / LAM
    check("MCQ 10.4-G year-11 cure, deemed CFADS", q(cure11_a, 0), 21347)
    check("MCQ 10.4-G year-11 cure, applied to prepayment", q(cure11_b, 0), 17789)
    check("MCQ 10.4-G the identity P = C / lambda", q(cure11_a / LAM, 0), q(cure11_b, 0))
    cure12_a = LAM * DS - cfads[12]
    check("MCQ 10.4-G year-12 cure, deemed CFADS", q(cure12_a, 0), 74849)
    check("MCQ 10.4-G total under treatment (a)", q(cure11_a + cure12_a, 0), 96196)
    # treatment (b): the year-11 prepayment reduces the closing balance, so year 12 is re-cut
    bal11 = DEBT
    for t in range(1, 12):
        bal11 = bal11 * (1 + R) - DS
    check("MCQ 10.4-G closing balance after year 11", q(bal11, 0), 4726071)
    bal11_b = bal11 - q(cure11_b, 0)
    ds12_b = bal11_b * (1 + R)
    cfads12_b = A + T * bal11_b * R
    cure12_b = ds12_b - cfads12_b / LAM
    check("MCQ 10.4-G year-12 debt service after the year-11 prepayment", q(ds12_b, 0), 4990779)
    check("MCQ 10.4-G year-12 DSCR before curing, treatment (b)",
          q(cfads12_b / ds12_b, 4), D("1.1895"))
    check("MCQ 10.4-G year-12 cure, treatment (b)", q(cure12_b, 0), 43696)
    total_b = q(cure11_b, 0) + q(cure12_b, 0)
    check("MCQ 10.4-G total under treatment (b)", total_b, 61485)
    check("MCQ 10.4-G what the drafting saves", q(cure11_a + cure12_a, 0) - total_b, 34711)
    check("MCQ 10.4-G that saving as %",
          q((q(cure11_a + cure12_a, 0) - total_b) / q(cure11_a + cure12_a, 0) * 100, 2), D("36.08"))
    check("MCQ 10.4-G the year-12 shortfall the item calls the resizing question in another form",
          q(cure12_a, 0), 74849)

    # ================= MCQ 10.4-H — three thresholds, three consequences =================
    DIST, LOCK = D("1.25"), D("1.15")
    for lvl, want in ((DIST, 6262044), (COV, 6011562), (LOCK, 5761081)):
        check(f"MCQ 10.4-H cash trigger at {lvl}x", q(DS * lvl, 0), want)
    caught_dist = [t for t in cfads if cfads[t] < DS * DIST]
    caught_cov = [t for t in cfads if cfads[t] < DS * COV]
    caught_lock = [t for t in cfads if cfads[t] < DS * LOCK]
    check("MCQ 10.4-H years the distribution condition catches", D(len(caught_dist)), 8)
    check("MCQ 10.4-H the caught years run 5 to 12", D(min(caught_dist)), 5)
    check("MCQ 10.4-H years the covenant catches", D(len(caught_cov)), 2)
    check("MCQ 10.4-H years the lock-up trigger catches", D(len(caught_lock)), 0)
    check("MCQ 10.4-H the minimum bank-case CFADS never reaches the lock-up",
          1 if min(cfads.values()) > DS * LOCK else 0, 1)
