"""PFL-AI Domain 10 — golden checks for the Evaluation and Comprehension MCQs added to KA 10.1–10.4.

Scope: every number printed in MCQ 10.1-F, 10.1-G, 10.2-G, 10.2-H, 10.3-F and 10.4-F — options and
rationales. `verify_formulas.py` owns the domain's original worked examples and `pfl_d10_ext.py` owns
the depth expansion; this module owns only the added items. Master-thread values come from ctx.

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
