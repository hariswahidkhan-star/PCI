"""PFL-AI Domain 4 — Investment appraisal: EXTENSION checks (verification pass).

`pfl_d04.py` asserts the numbers of the depth expansion. This module deliberately does **not**
duplicate it. It covers only what the verification pass added or repaired, and it derives every
value from first principles — bisection for roots, closed forms tested against the numeric root —
before comparing it with the manuscript. Master-thread figures come from `ctx`, never re-typed.

Four groups of claims live here.

1. **The two presentation tables that were computed off a published two-decimal rate.** Both were
   corrected, and both have exact closed forms the manuscript now teaches. At either project's own
   IRR the annuity factor is pinned at that project's `I0 / A`, so the *other* project's NPV in a
   profile table is round by construction (1,666,667 and -5,000,000 for P and Q). At the master
   appraisal's IRR the present value of the base inflow equals `I0`, so scaling the inflow by `m`
   gives exactly `(m - 1) x I0` (-12,000,000 / -6,000,000 / 0 / +6,000,000). Each closed form is
   checked *against the numeric root* as well as against the printed cell, and the pre-correction
   values are asserted as what the published-rate route actually produces — so the defect signature
   is on the record rather than merely removed.

2. **The MIRR wedge decomposition, made additive.** The manuscript now moves one variable at a
   time: IRR 12.1921 % -> MIRR 9.7327 % (reinvestment fiction, 2.46 pts) -> MIRR 7.5273 %
   (reinvestment rate to 4 %, 2.21 pts) -> MIRR 7.0920 % (membrane outflow at the 6 % finance rate,
   0.44 pts). The parts must sum to the whole; that summation is the invariant, and the earlier
   text's 2.46 + 2.64 + "a further bite" could not satisfy it.

3. **The rationing enumeration, counted rather than asserted.** Five indivisible projects against a
   20,000,000 budget give 31 non-empty subsets of which **16** fit, in the split the manuscript now
   states (5 singles, 9 of 10 pairs, 2 of 10 triples, no set of four — the four cheapest total
   28,000,000).

4. **Case study B, which asserted a wedge and computed none.** A 3-year doubling earns
   `2^(1/3) - 1`; recycling it with an average idle interval `g` at 2.0 % cash multiplies capital by
   `2 x 1.02^g` per cycle of `3 + g` years, so the realised compound return is
   `(2 x 1.02^g)^(1/(3+g)) - 1`. Every figure in the case is a value of that function, a difference
   between two of its values, or the concession it is compared with, and the module tests the
   function's own invariants (wedge additivity across the comparison table, monotonicity in `g`, and
   the breakeven gap reproducing the concession IRR to ten figures).

Master appraisal: I0 = KESTREL_CAPEX; A = 8,900,000 for 15 years; r = KESTREL_DISCOUNT.
"""


def run(ctx):
    check, D, _af = ctx["check"], ctx["D"], ctx["af"]

    def af(r, n):                             # ctx's annuity factor, string-rate tolerant
        return _af(D(str(r)), n)

    I0 = ctx["KESTREL_CAPEX"]                 # 60,000,000
    R = ctx["KESTREL_DISCOUNT"]               # 8.0 %
    R_FIN = ctx["KESTREL_RATE"]               # 6.0 % senior facility — the MIRR finance rate
    A, N = D(8900000), 15

    def solve(f, lo, hi, it=200):
        """Deterministic bracketing bisection; no floats anywhere."""
        lo, hi = D(str(lo)), D(str(hi))
        for _ in range(it):
            m = (lo + hi) / 2
            if f(lo) * f(m) <= 0:
                hi = m
            else:
                lo = m
        return (lo + hi) / 2

    def nthroot(x, n):
        """x^(1/n) by bisection — an independent route to Decimal's fractional power."""
        return solve(lambda y: D(str(y)) ** D(str(n)) - D(str(x)), "0.000001", "1000")

    def fvaf(r, n):
        r = D(str(r))
        return ((1 + r) ** n - 1) / r

    AF15 = af(R, N)
    NPV = A * AF15 - I0
    IRR = solve(lambda r: A * af(r, N) - I0, "0.05", "0.25")
    check("master NPV agrees with the master-thread constant", NPV, ctx["KESTREL_NPV"], tol=D("0.5"))
    check("master IRR (retained root)", IRR * 100, D("12.192120"), tol=D("0.0000005"))

    # =====================================================================================
    # 1a. KA 4.3.1 — the profile table's cross-cells, corrected, and the invariant behind them
    # =====================================================================================
    iP, aP, iQ, aQ, nPQ = D(5000000), D(2000000), D(20000000), D(6000000), 5
    IRR_P = solve(lambda r: aP * af(r, nPQ) - iP, "0.05", "0.60")
    IRR_Q = solve(lambda r: aQ * af(r, nPQ) - iQ, "0.05", "0.60")
    check("4.3.1 IRR of P (retained)", IRR_P * 100, D("28.649290"), tol=D("0.0000005"))
    check("4.3.1 IRR of Q (retained)", IRR_Q * 100, D("15.238237"), tol=D("0.0000005"))

    # the invariant the new paragraph claims: at a project's own IRR the factor IS its I0/A
    check("4.3.1 INVARIANT AF at P's IRR is pinned at P's I0/A", af(IRR_P, nPQ), iP / aP,
          tol=D("0.0000000001"))
    check("4.3.1 INVARIANT AF at Q's IRR is pinned at Q's I0/A", af(IRR_Q, nPQ), iQ / aQ,
          tol=D("0.0000000001"))
    check("4.3.1 factor at P's IRR (printed 2.500000)", iP / aP, D("2.500000"),
          tol=D("0.0000005"))
    check("4.3.1 factor at Q's IRR (printed 3.333333...)", iQ / aQ, D(10) / 3,
          tol=D("0.0000000001"))

    # the corrected cells, by closed form and again at the numeric root
    check("4.3.1 CORRECTED cell NPV_P at Q's IRR, closed form", aP * (iQ / aQ) - iP,
          D("1666666.67"), tol=D("0.005"))
    check("4.3.1 CORRECTED cell NPV_P at Q's IRR, at the numeric root",
          aP * af(IRR_Q, nPQ) - iP, D("1666666.67"), tol=D("0.005"))
    check("4.3.1 CORRECTED cell NPV_Q at P's IRR, closed form", aQ * (iP / aP) - iQ,
          D(-5000000), tol=D("0.005"))
    check("4.3.1 CORRECTED cell NPV_Q at P's IRR, at the numeric root",
          aQ * af(IRR_P, nPQ) - iQ, D(-5000000), tol=D("0.005"))
    # and the other diagonal is zero by definition, which is what makes the cross-cells readable
    check("4.3.1 NPV_Q at Q's own IRR is zero", aQ * af(IRR_Q, nPQ) - iQ, D(0),
          tol=D("0.0000005"))
    check("4.3.1 NPV_P at P's own IRR is zero", aP * af(IRR_P, nPQ) - iP, D(0),
          tol=D("0.0000005"))

    # the defect signature: the published two-decimal rates produce the pre-correction values
    check("4.3.1 DEFECT SIGNATURE published 15.2382 % gave 1,666,672 (was printed 1,666,389)",
          aP * af("0.152382", nPQ) - iP, D("1666672.50"), tol=D("0.5"))
    check("4.3.1 DEFECT SIGNATURE published 28.6493 % gave -5,000,003 (was printed -5,000,208)",
          aQ * af("0.286493", nPQ) - iQ, D("-5000002.85"), tol=D("0.5"))
    check("4.3.1 rounding a published rate moves a cell by dollars, not cents",
          abs(aP * af("0.152382", nPQ) - iP - (aP * (iQ / aQ) - iP)), D("5.83"), tol=D("0.5"))

    # =====================================================================================
    # 1b. KA 4.3.3 — the two-way table's IRR column, corrected to the (m-1) x I0 closed form
    # =====================================================================================
    check("4.3.3 INVARIANT PV of the base inflow at the retained IRR equals I0",
          A * af(IRR, N), I0, tol=D("0.000001"))
    for label, m in (("80 %", D("0.80")), ("90 %", D("0.90")),
                     ("100 %", D(1)), ("110 %", D("1.10"))):
        want = (m - 1) * I0
        check(f"4.3.3 CORRECTED IRR-column cell at {label}, closed form (m-1) x I0",
              want, {"80 %": D(-12000000), "90 %": D(-6000000),
                     "100 %": D(0), "110 %": D(6000000)}[label])
        check(f"4.3.3 CORRECTED IRR-column cell at {label}, at the numeric root",
              A * m * af(IRR, N) - I0, want, tol=D("0.000005"))
    check("4.3.3 INVARIANT the IRR column is linear: 10 % of inflow is 10 % of I0",
          (D("1.10") - 1) * I0 - ((D(1) - 1) * I0), I0 / 10)
    # the defect signature again: the published 12.19 % shifts every cell by about six thousand
    check("4.3.3 DEFECT SIGNATURE 80 % cell at the published 12.19 % (was printed -11,994,599)",
          A * D("0.80") * af("0.1219", N) - I0, D("-11994599.43"), tol=D("0.5"))
    check("4.3.3 DEFECT SIGNATURE 90 % cell at the published 12.19 % (was printed -5,993,924)",
          A * D("0.90") * af("0.1219", N) - I0, D("-5993924.36"), tol=D("0.5"))
    check("4.3.3 DEFECT SIGNATURE 110 % cell at the published 12.19 % (was printed +6,007,426)",
          A * D("1.10") * af("0.1219", N) - I0, D("6007425.79"), tol=D("0.5"))
    check("4.3.3 DEFECT SIGNATURE the 100 % cell stops being zero at 12.19 %",
          A * af("0.1219", N) - I0, D("6750.72"), tol=D("0.5"))

    # MCQ 4.3-F's stem, now self-contained: singly survivable, jointly not
    check("MCQ 4.3-F 80 % inflow at the 8 % hurdle is still positive",
          A * D("0.80") * af(R, N) - I0, D(943488), tol=D("0.5"))
    check("MCQ 4.3-F base inflow at a 9 % rate is still positive",
          A * af("0.09", N) - I0, D(11740127), tol=D("0.5"))
    check("MCQ 4.3-F the joint cell destroys value", A * D("0.80") * af("0.09", N) - I0,
          D(-2607898), tol=D("0.5"))
    check("4.3.3 rate at which the 80 % case breaks even (about 8.2 %)",
          solve(lambda r: A * D("0.80") * af(r, N) - I0, "0.01", "0.30") * 100,
          D("8.2566"), tol=D("0.00005"))

    # MCQ 4.3-H's tightened key: on a 15 % tariff concession the rate headroom nearly vanishes
    R85 = solve(lambda r: A * D("0.85") * af(r, N) - I0, "0.01", "0.30")
    check("MCQ 4.3-H revenue headroom left after a 15 % concession (points)",
          (A - I0 / AF15) / A * 100 - 15, D("6.24"), tol=D("0.005"))
    check("MCQ 4.3-H breakeven rate on the reduced tariff", R85 * 100, D("9.2755"),
          tol=D("0.00005"))
    check("MCQ 4.3-H rate headroom remaining (stated as barely 1.3 points)",
          R85 * 100 - 8, D("1.3"), tol=D("0.03"))
    check("MCQ 4.3-H the reduced tariff is still positive at 9 %",
          A * D("0.85") * af("0.09", N) - I0, D(979108), tol=D("0.5"))
    check("MCQ 4.3-H and negative at 10 %", A * D("0.85") * af("0.10", N) - I0,
          D(-2460009), tol=D("0.5"))

    # =====================================================================================
    # 2. KA 4.1.3b — the wedge decomposition, one variable at a time, made additive
    # =====================================================================================
    MIRR_8 = (nthroot(A * fvaf(R, N) / I0, N) - 1) * 100
    MIRR_4 = (nthroot(A * fvaf("0.04", N) / I0, N) - 1) * 100
    PV_OUT = I0 + D(6000000) / (1 + R_FIN) ** 8
    MIRR_4M = (nthroot(A * fvaf("0.04", N) / PV_OUT, N) - 1) * 100
    check("4.1.3 MIRR at an 8 % reinvestment rate", MIRR_8, D("9.7327"), tol=D("0.00005"))
    check("4.1.3b MIRR at a 4 % reinvestment rate, original outflow only", MIRR_4,
          D("7.5273"), tol=D("0.00005"))
    check("4.1.3b PV of the membrane outflow at the 6 % finance rate",
          D(6000000) / (1 + R_FIN) ** 8, D(3764474), tol=D("0.5"))
    check("4.1.3b PV of outflows", PV_OUT, D(63764474), tol=D("0.5"))
    check("4.1.3b FV of inflows at 4 %", A * fvaf("0.04", N), D(178209930), tol=D("0.5"))
    check("4.1.3b MIRR with both honest rates and the membrane", MIRR_4M, D("7.0920"),
          tol=D("0.00005"))
    # a second, independent route to the same MIRR: Decimal's fractional power
    check("4.1.3b MIRR by fractional power agrees with the bisected root",
          ((A * fvaf("0.04", N) / PV_OUT) ** (D(1) / N) - 1) * 100, MIRR_4M,
          tol=D("0.0000001"))
    STEP1, STEP2, STEP3 = IRR * 100 - MIRR_8, MIRR_8 - MIRR_4, MIRR_4 - MIRR_4M
    check("4.1.3b step 1 — the reinvestment fiction at the opportunity cost (pts)", STEP1,
          D("2.46"), tol=D("0.005"))
    check("4.1.3b step 2 — CORRECTED: reinvestment rate 8 % to 4 % alone (pts)", STEP2,
          D("2.21"), tol=D("0.005"))
    check("4.1.3b step 3 — the membrane outflow at the finance rate (pts)", STEP3,
          D("0.44"), tol=D("0.005"))
    check("4.1.3b INVARIANT the three parts sum to the whole wedge (pts)",
          STEP1 + STEP2 + STEP3, D("5.10"), tol=D("0.005"))
    check("4.1.3b INVARIANT the wedge equals IRR minus the honest MIRR exactly",
          STEP1 + STEP2 + STEP3, IRR * 100 - MIRR_4M, tol=D("0.0000001"))
    check("4.1.3b DEFECT SIGNATURE the old 2.64 pts was steps 2 and 3 together",
          STEP2 + STEP3, D("2.64"), tol=D("0.005"))
    check("4.1.3b INVARIANT MIRR lies between the reinvestment rate and the IRR",
          D(1) if D(4) < MIRR_4M < IRR * 100 else D(0), D(1))
    # MIRR is not a hurdle test: the same flows are worth +12,937,747 at the 8 % project rate
    check("4.1.3b NPV of the same flows at the project rate",
          A * AF15 - I0 - D(6000000) / (1 + R) ** 8, D(12937747), tol=D("0.5"))
    check("4.1.3b IRR of the same flows",
          solve(lambda r: A * af(r, N) - I0 - D(6000000) / (1 + D(str(r))) ** 8,
                "0.05", "0.25") * 100, D("11.4250"), tol=D("0.00005"))

    # MCQ 4.1-F's stem, rewritten as an explanation of the 2.46-point gap
    check("MCQ 4.1-F the gap in the stem is IRR minus MIRR at a common 8 %", IRR * 100 - MIRR_8,
          D("2.46"), tol=D("0.005"))

    # =====================================================================================
    # 3. KA 4.3.2 — the enumeration, counted
    # =====================================================================================
    PROJ = (("W", D(8000000), D(2400000)), ("V", D(4000000), D(1160000)),
            ("X", D(12000000), D(3000000)), ("Y", D(10000000), D(2200000)),
            ("Z", D(6000000), D(1020000)))
    BUDGET = D(20000000)
    feasible, by_size = [], {1: 0, 2: 0, 3: 0, 4: 0, 5: 0}
    for mask in range(1, 1 << len(PROJ)):
        chosen = [PROJ[i] for i in range(len(PROJ)) if mask >> i & 1]
        if sum(p[1] for p in chosen) <= BUDGET:
            feasible.append(chosen)
            by_size[len(chosen)] += 1
    check("4.3.2 non-empty subsets of five projects", D((1 << len(PROJ)) - 1), D(31))
    check("4.3.2 CORRECTED count of feasible sets (manuscript said twelve)",
          D(len(feasible)), D(16))
    check("4.3.2 feasible singles", D(by_size[1]), D(5))
    check("4.3.2 feasible pairs (of ten)", D(by_size[2]), D(9))
    check("4.3.2 feasible triples (of ten)", D(by_size[3]), D(2))
    check("4.3.2 feasible sets of four", D(by_size[4]), D(0))
    check("4.3.2 the four cheapest projects already exceed the budget",
          sum(sorted(p[1] for p in PROJ)[:4]), D(28000000))
    best = max(feasible, key=lambda s: sum(p[2] for p in s))
    check("4.3.2 value-maximising set value", sum(p[2] for p in best), D(5400000))
    check("4.3.2 value-maximising set spends the budget exactly", sum(p[1] for p in best),
          BUDGET)
    # MCQ 4.3-G's rewritten options restate the greedy result, so it is re-derived here too
    spent, value = D(0), D(0)
    for name, cost, npv_p in sorted(PROJ, key=lambda p: -(p[2] / p[1])):
        if spent + cost <= BUDGET:
            spent, value = spent + cost, value + npv_p
    check("MCQ 4.3-G greedy PI spend", spent, D(18000000))
    check("MCQ 4.3-G greedy PI value", value, D(4580000))
    check("MCQ 4.3-G budget left unspent by the greedy pack", BUDGET - spent, D(2000000))
    check("MCQ 4.3-G value the greedy pack leaves unfunded",
          sum(p[2] for p in best) - value, D(820000))
    check("MCQ 4.3-G shortfall as a share of the achievable value (%, stated as 15)",
          D(820000) / D(5400000) * 100, D(15), tol=D("0.25"))

    # =====================================================================================
    # 4. MCQ 4.2-F — the payback gap, restated as the item's subject
    # =====================================================================================
    DPB = 10 + (I0 - A * af(R, 10)) / (A / (1 + R) ** 11)
    check("MCQ 4.2-F simple payback", I0 / A, D("6.74"), tol=D("0.005"))
    check("MCQ 4.2-F discounted payback", DPB, D("10.07"), tol=D("0.005"))
    check("MCQ 4.2-F the gap (stated as three and a third years)", DPB - I0 / A,
          D(10) / 3, tol=D("0.01"))
    check("MCQ 4.2-F INVARIANT discounted payback is the later of the two at any positive rate",
          D(1) if DPB > I0 / A else D(0), D(1))

    # =====================================================================================
    # 5. Case study B — the fund that bought percentages, now computed
    # =====================================================================================
    COMMIT, EXIT, DEAL_YRS = D(10000000), D(20000000), 3
    CASH = D("0.02")                       # idle balances held at 2.0 %
    MULT = EXIT / COMMIT
    check("Case B the representative deal doubles the money", MULT, D(2))
    DEAL = nthroot(MULT, DEAL_YRS) - 1
    check("Case B deal IRR (2^(1/3) - 1)", DEAL * 100, D("25.9921"), tol=D("0.00005"))
    check("Case B deal IRR by fractional power agrees",
          (MULT ** (D(1) / DEAL_YRS) - 1) * 100, DEAL * 100, tol=D("0.0000001"))
    check("Case B INVARIANT the deal IRR compounds back to the exit multiple",
          (1 + DEAL) ** DEAL_YRS, MULT, tol=D("0.0000001"))
    check("Case B deal IRR sits inside the case's stated 25-30 % band",
          D(1) if D(25) <= DEAL * 100 <= D(30) else D(0), D(1))

    def cycle(gap):
        """(multiple, realised compound return) for one recycling cycle with an idle gap."""
        gap = D(str(gap))
        mult = D(2) * (1 + CASH) ** gap
        return mult, nthroot(mult, DEAL_YRS + gap) - 1

    M6, R6 = cycle("0.5")
    M12, R12 = cycle(1)
    check("Case B cycle multiple at a six-month gap", M6, D("2.019901"), tol=D("0.0000005"))
    check("Case B cycle multiple at a twelve-month gap", M12, D("2.040000"), tol=D("0.0000005"))
    check("Case B twelve-month multiple is exactly 2 x 1.02", M12, D(2) * (1 + CASH),
          tol=D("0.0000000001"))
    check("Case B realised return at a six-month gap", R6 * 100, D("22.2467"), tol=D("0.00005"))
    check("Case B realised return at a twelve-month gap", R12 * 100, D("19.5109"),
          tol=D("0.00005"))
    check("Case B INVARIANT the realised return compounds back to its cycle multiple",
          (1 + R6) ** (D(DEAL_YRS) + D("0.5")), M6, tol=D("0.0000001"))
    check("Case B wedge at a six-month gap (bp)", (DEAL - R6) * 10000, D("374.54"),
          tol=D("0.005"))
    check("Case B the six-month wedge is the case's 'nearly four points'",
          (DEAL - R6) * 100, D("3.75"), tol=D("0.005"))
    check("Case B wedge at a twelve-month gap (bp)", (DEAL - R12) * 10000, D("648.12"),
          tol=D("0.005"))
    check("Case B INVARIANT a longer idle gap lowers the realised return",
          D(1) if R12 < R6 < DEAL else D(0), D(1))
    check("Case B spread between the six- and twelve-month gaps (bp)", (R6 - R12) * 10000,
          D("273.58"), tol=D("0.005"))
    check("Case B sensitivity per month of idle capital (bp, stated as roughly 46)",
          (R6 - R12) * 10000 / 6, D(46), tol=D("0.5"))
    check("Case B INVARIANT the two wedges differ by the same spread",
          (DEAL - R12) * 10000 - (DEAL - R6) * 10000, (R6 - R12) * 10000,
          tol=D("0.0000001"))

    # the declined concession
    CONC_A, CONC_N = D(1800000), 12
    check("Case B concession annuity factor required", COMMIT / CONC_A, D("5.555556"),
          tol=D("0.0000005"))
    CONC = solve(lambda r: CONC_A * af(r, CONC_N) - COMMIT, "0.01", "0.50")
    check("Case B concession IRR", CONC * 100, D("14.4284"), tol=D("0.00005"))
    check("Case B INVARIANT AF at the concession IRR is the required factor",
          af(CONC, CONC_N), COMMIT / CONC_A, tol=D("0.0000000001"))
    check("Case B concession IRR sits inside the case's stated 13-15 % band",
          D(1) if D(13) <= CONC * 100 <= D(15) else D(0), D(1))
    check("Case B AF at the fund's 9 % cost of capital", af("0.09", CONC_N),
          D("7.16072528"), tol=D("0.000000005"))
    check("Case B concession NPV at the fund's 9 %", CONC_A * af("0.09", CONC_N) - COMMIT,
          D(2889305), tol=D("0.5"))

    # the comparison table
    check("Case B advantage believed (bp) — CORRECTED, was printed 1,156.37",
          (DEAL - CONC) * 10000, D("1156.38"), tol=D("0.005"))
    check("Case B advantage at a six-month gap (bp)", (R6 - CONC) * 10000, D("781.84"),
          tol=D("0.005"))
    check("Case B advantage at a twelve-month gap (bp)", (R12 - CONC) * 10000, D("508.26"),
          tol=D("0.005"))
    check("Case B INVARIANT believed advantage less actual advantage is the wedge (six-month)",
          (DEAL - CONC) * 10000 - (R6 - CONC) * 10000, (DEAL - R6) * 10000,
          tol=D("0.0000001"))
    check("Case B INVARIANT believed advantage less actual advantage is the wedge (twelve-month)",
          (DEAL - CONC) * 10000 - (R12 - CONC) * 10000, (DEAL - R12) * 10000,
          tol=D("0.0000001"))
    check("Case B the short-deal book still leads at every gap the fund ran",
          D(1) if R12 > CONC else D(0), D(1))

    # the breakeven redeployment gap
    GAP = solve(lambda g: cycle(g)[1] - CONC, "0.5", "5", it=90)
    check("Case B breakeven redeployment gap (years)", GAP, D("2.5119"), tol=D("0.00005"))
    check("Case B INVARIANT at the breakeven gap the realised return IS the concession IRR",
          cycle(GAP)[1] * 100, CONC * 100, tol=D("0.0000001"))
    check("Case B the fund's actual gaps sat well inside the breakeven",
          D(1) if GAP > 1 else D(0), D(1))
