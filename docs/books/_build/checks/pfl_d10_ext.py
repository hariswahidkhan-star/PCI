"""PFL-AI Domain 10 — Debt sizing, covenants and credit metrics: EXTENSION checks.

The domain's original numbers are asserted in `verify_formulas.py` and are deliberately not
duplicated here. This module covers only the material added by the depth expansion, and it derives
every value from first principles before comparing it with the manuscript — master-thread figures
come from `ctx`, never from a re-typed literal.

Three engines carry almost all of the new arithmetic, so each is derived once and reused.

1. **`CFADS` as a function.** Domains 2 and 6 fix Kestrel's bank case: revenue flat at 12,000,000,
   cash operating cost 4,500,000 of which 750,000 is variable, depreciation 2,400,000, cash tax at
   `T`, a 600,000 working-capital movement. That gives

       CFADS(R, I) = 0.75 R - 3,120,000 + T x I        (T derived, not assumed)
                   = A + T x I  with A = 5,880,000 at R = 12,000,000

   The relation is checked against the master-thread `CFADS` of 6,384,000 at the year-one interest
   *before* anything is built on it, and `A` is obtained as `CFADS - T x interest`, from ctx.

2. **The sculpting closed form.** With `DS(t) = (A + T r B(t-1))/lambda` the balance rolls forward as
   `B(t) = B(t-1)(1 + r - T r/lambda) - A/lambda`, so `B(n) = 0` gives exactly

       max sculpted debt = (A/lambda) x AF(r*, n)      r* = r (1 - T/lambda)

   Both the closed form and an explicit twelve-row sculpted schedule are built here, and the module
   asserts that the schedule closes at zero, that every period's coverage is exactly `lambda`, and
   that its final payment equals the level payment a minimum-period test would impose — the
   manuscript's three structural claims, each tested as two independent calculations agreeing.

3. **The level-schedule coverage profile.** `profile(debt, instalment)` returns each period's
   interest, `CFADS` and `DSCR` for a level annuity against engine 1. Kestrel's declining bank-case
   profile (Domain 6, WE 6.4.1b), the three sizing bases of WE 10.1.3, the four-covenant stress of
   WE 10.2.4, the historic/forward tests of WE 10.4.1, the lock-up years of WE 10.4.2 and the cure
   sizing of WE 10.4.3 are all read off it, so a single schedule error would fail many checks.

Local helpers: `solve()` is a deterministic bracketing bisection (used only for the sculpted-lambda
identity of 10.2.2); `npv()` discounts a dated flow. ctx carries `af()` only.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]

    DEBT = ctx["KESTREL_DEBT"]
    CF1 = ctx["KESTREL_CFADS"]
    INST = ctx["KESTREL_INSTALMENT"]
    EBITDA = ctx["KESTREL_EBITDA"]
    EBIT = ctx["KESTREL_EBIT"]
    INT1 = ctx["KESTREL_INTEREST_Y1"]
    R = ctx["KESTREL_RATE"]
    N = ctx["KESTREL_TENOR"]
    LIFE = ctx["KESTREL_LIFE"]
    BOARD = ctx["KESTREL_DISCOUNT"]

    TOL6 = D("0.0000005")           # six-decimal factors
    MONEY = D("0.5")                # money printed to whole units

    def solve(fn, lo, hi, iters=300):
        lo, hi = D(lo), D(hi)
        sign_lo = fn(lo) > 0
        for _ in range(iters):
            mid = (lo + hi) / 2
            if (fn(mid) > 0) == sign_lo:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    def npv(pairs, rate):
        """pairs = [(t, cash), ...]; discounted to t = 0."""
        return sum(c / (1 + rate) ** t for t, c in pairs)

    # ================================================================= engine 1
    # Cash tax rate DERIVED from the master thread (Domain 2's CFADS before working capital).
    CFADS_PRE_WC = D(6984000)                      # Domain 2, KA 2.3.1
    WC = CFADS_PRE_WC - CF1
    T = (EBITDA - CFADS_PRE_WC) / (EBIT - INT1)
    check("D10ext derived working-capital movement", WC, 600000)
    check("D10ext derived cash tax rate %", T * 100, "20.0")

    REV = D(12000000)                              # Domain 6 bank case, revenue flat
    OPEX, OPEX_VAR, DEP = D(4500000), D(750000), D(2400000)

    def cfads(rev, interest):
        """Built from the components, not from the reduced form."""
        ebitda = rev - (OPEX - OPEX_VAR) - OPEX_VAR / REV * rev
        return ebitda - T * (ebitda - DEP - interest) - WC

    # header — "The CFADS relation this domain works with"
    check("Why-this-domain: CFADS relation returns the master-thread CFADS",
          cfads(REV, INT1), CF1)
    check("Why-this-domain: revenue coefficient 0.75",
          (cfads(REV + 1000000, INT1) - cfads(REV, INT1)) / 1000000, "0.75")
    check("Why-this-domain: relation constant 3,120,000",
          D("0.75") * REV - cfads(REV, D(0)), 3120000)
    check("Why-this-domain: interest coefficient = T", T, "0.20")
    check("Why-this-domain: one-variable collapse constant 2,616,000",
          D(3120000) - T * INT1, 2616000)

    A = CF1 - T * INT1                             # CFADS before the interest tax shield
    check("Why-this-domain: A (CFADS before the shield)", A, 5880000)
    check("Why-this-domain: A == 0.75R - 3,120,000", D("0.75") * REV - D(3120000), A)

    # header — Kestrel's three coverage thresholds, in cash
    LAM_DIST, LAM_COV, LAM_LOCK = D("1.25"), D("1.20"), D("1.15")
    TH_DIST, TH_COV, TH_LOCK = INST * LAM_DIST, INST * LAM_COV, INST * LAM_LOCK
    check("Why-this-domain: 1.25x distribution condition in cash", TH_DIST, 6262044, MONEY)
    check("Why-this-domain: 1.20x covenant in cash", TH_COV, 6011562, MONEY)
    check("Why-this-domain: 1.15x lock-up trigger in cash", TH_LOCK, 5761081, MONEY)

    # ================================================================= engine 3
    AF = af(R, N)
    AF25 = af(R, LIFE)
    DF12 = (1 + R) ** -N
    check("D10ext AF(6%,12)", AF, "8.383844", TOL6)
    check("D10ext AF(6%,25)", AF25, "12.783356", TOL6)
    check("WE 10.2.3 Substitution 1.06^-12", DF12, "0.496969", TOL6)

    def profile(debt, instalment, n=None):
        """Level-annuity coverage profile against engine 1. Returns rows and the closing balance."""
        n = n or N
        bal, rows = D(debt), []
        for t in range(1, n + 1):
            interest = bal * R
            c = A + T * interest
            rows.append({"t": t, "open": bal, "int": interest, "cfads": c,
                         "dscr": c / instalment})
            bal = bal * (1 + R) - instalment
        return rows, bal

    MAND, MAND_END = profile(DEBT, INST)           # the mandated 42,000,000 facility
    check("D10ext mandated schedule closes at zero", MAND_END, 0, D("0.1"))
    check("D10ext mandated y1 CFADS ties to master thread", MAND[0]["cfads"], CF1)
    check("D10ext mandated y12 CFADS ties to Domain 6", MAND[11]["cfads"], 5936713, MONEY)
    check("D10ext mandated y12 DSCR ties to Domain 6", MAND[11]["dscr"], "1.1851")

    # ---------------------------------------------------------- WE 10.1.2b
    LAM1, LAM2 = D("1.30"), D("1.25")
    s_star = 1 - LAM2 / LAM1
    check("WE 10.1.2b Result indifference stress s* %", s_star * 100, "3.8462")
    check("WE 10.1.2b Result CFADS at indifference", CF1 * (1 - s_star), 6138462, MONEY)
    cap130 = CF1 / LAM1 * AF
    cap125 = CF1 * D("0.95") / LAM2 * AF
    check("WE 10.1.2b Substitution max debt service at 1.30x", CF1 / LAM1, "4910769.23")
    check("WE 10.1.2b Substitution stressed CFADS 6,064,800", CF1 * D("0.95"), 6064800)
    check("WE 10.1.2b Result capacity at 1.30x on base cash", cap130, 41171123, MONEY)
    check("WE 10.1.2b Result capacity at 1.25x on a 5% stress", cap125, 40677069, MONEY)
    check("WE 10.1.2b Result capacity destroyed", cap130 - cap125, 494053, MONEY)
    check("WE 10.1.2b Interpretation s* for 1.30x -> 1.20x %",
          (1 - D("1.20") / LAM1) * 100, "7.6923")
    check("WE 10.1.2b Interpretation neutral ratio on a 5% stress", LAM1 * D("0.95"), "1.2350")
    # the rule is independent of CFADS, rate and tenor — the claim, tested
    for cf_alt, r_alt, n_alt in ((D(9000000), D("0.08"), 15), (D(3000000), D("0.04"), 7)):
        a1 = cf_alt / LAM1 * af(r_alt, n_alt)
        a2 = cf_alt * (1 - s_star) / LAM2 * af(r_alt, n_alt)
        check(f"WE 10.1.2b s* is invariant to CFADS/rate/tenor ({n_alt}yr)", a1 - a2, 0, D("0.01"))

    # ---------------------------------------------------------- 10.1.3 closed form
    LAM = D("1.30")
    RSTAR = R * (1 - T / LAM)
    AFS = af(RSTAR, N)
    check("10.1.3 effective sculpting rate r* %", RSTAR * 100, "5.076923", TOL6)
    check("WE 10.1.3 Substitution AF(r*,12)", AFS, "8.824924", TOL6)
    check("WE 10.1.3 Substitution sizing basis A/lambda", A / LAM, "4523076.92")
    sculpt = A / LAM * AFS
    check("WE 10.1.3 Result sculpted capacity", sculpt, 39915812, MONEY)

    # explicit sculpted schedule — the closed form's independent confirmation
    bal, tot_ds, sc = sculpt, D(0), []
    for t in range(1, N + 1):
        interest = bal * R
        c = A + T * interest
        ds = c / LAM
        sc.append({"t": t, "open": bal, "int": interest, "cfads": c, "ds": ds,
                   "prin": ds - interest, "dscr": c / ds})
        tot_ds += ds
        bal = bal * (1 + R) - ds
    check("10.A.3 sculpted schedule closes at zero", bal, 0, D("0.01"))
    for row in sc:
        check(f"10.A.3 sculpted coverage is exactly 1.3000 in period {row['t']}",
              row["dscr"], "1.3000")
    check("WE 10.1.3 Result y1 interest", sc[0]["int"], 2394949, MONEY)
    check("WE 10.1.3 Result y1 CFADS (resized debt, not the request)", sc[0]["cfads"],
          6358990, MONEY)
    check("WE 10.1.3 Result y1 debt service", sc[0]["ds"], 4891531, MONEY)
    check("WE 10.1.3 Result table y1 debt service to cents", sc[0]["ds"], "4891530.57")
    check("WE 10.1.3 Result y1 principal", sc[0]["prin"], 2496582, MONEY)
    check("WE 10.1.3 Result y6 debt service", sc[5]["ds"], 4763995, MONEY)
    check("WE 10.1.3 Result y6 principal", sc[5]["prin"], 3198030, MONEY)
    check("WE 10.1.3 Result y12 debt service", sc[11]["ds"], 4562811, MONEY)
    check("WE 10.1.3 Result y11 closing balance retired at y12", sc[11]["open"], 4304539, MONEY)
    check("WE 10.1.3 Result y12 service retires the closing balance",
          sc[11]["open"] * (1 + R), sc[11]["ds"], D("0.01"))
    check("WE 10.1.3 Result total debt service", tot_ds, 56888034, MONEY)
    check("WE 10.1.3 Result total interest", tot_ds - sculpt, 16972222, MONEY)

    # minimum-period level sizing: the binding period is the last
    DSMIN = A / (LAM - T * R / (1 + R))
    check("WE 10.1.3 Substitution minimum-period divisor",
          LAM - T * R / (1 + R), "1.28867925", D("0.000000005"))   # 8-dp display
    check("WE 10.1.3 Result minimum-period debt service", DSMIN, "4562811.13")
    debtmin = DSMIN * AF
    check("WE 10.1.3 Result minimum-period debt", debtmin, 38253896, MONEY)
    check("10.A.3 sculpted final payment == minimum-period level payment",
          sc[11]["ds"], DSMIN, D("0.00001"))

    check("WE 10.1.3 Interpretation cost of the every-period test", cap130 - debtmin,
          2917226, MONEY)
    check("WE 10.1.3 Interpretation sculpting recovers", sculpt - debtmin, 1661916, MONEY)
    check("WE 10.1.3 Interpretation recovery %",
          (sculpt - debtmin) / (cap130 - debtmin) * 100, "56.97")
    check("WE 10.1.3 Interpretation r* at a 30% tax rate %",
          R * (1 - D("0.30") / LAM) * 100, "4.6154")

    base_rows, base_end = profile(cap130, CF1 / LAM)
    check("WE 10.1.3 Result base-case-sized schedule closes", base_end, 0, D("0.1"))
    check("WE 10.1.3 Result base-case-sized DSCR y1", base_rows[0]["dscr"], "1.2980")
    check("WE 10.1.3 Result base-case-sized DSCR y12 (ties to Domain 6)",
          base_rows[11]["dscr"], "1.2087")
    check("WE 10.1.3 Result base-case-sized y1 tax shield 494,053",
          T * base_rows[0]["int"], 494053, MONEY)
    min_rows, min_end = profile(debtmin, DSMIN)
    check("WE 10.1.3 Result minimum-period-sized schedule closes", min_end, 0, D("0.1"))
    check("WE 10.1.3 Result minimum-period-sized DSCR y1", min_rows[0]["dscr"], "1.3893")
    check("WE 10.1.3 Result minimum-period-sized DSCR y12", min_rows[11]["dscr"], "1.3000")

    # ---------------------------------------------------------- 10.1.3 balloon
    gap = DEBT - cap130
    balloon = gap / DF12
    check("10.1.3 closing para sizing gap", gap, 828877, MONEY)
    check("10.1.3 closing para gap-closing balloon", balloon, 1667864, MONEY)
    check("10.1.3 closing para balloon as % of facility", balloon / DEBT * 100, "3.97")
    check("10.1.3 closing para balloon structure reproduces 42,000,000",
          CF1 / LAM * AF + balloon * DF12, DEBT, D("0.01"))

    # ---------------------------------------------------------- MCQ 10.1-D / self-check
    check("MCQ 10.1-D distractor C (shield feedback ignored)", A / LAM * AF, 37920771, MONEY)

    # ---------------------------------------------------------- 10.2.2 sculpting generalisation
    lam_sculpt = solve(lambda l: A / l * af(R * (1 - T / l), N) - DEBT, "1.0", "1.6")
    check("10.2.2 Interpretation sculpted constant DSCR at 42,000,000", lam_sculpt, "1.2387")
    LLCR_D6 = D("1.2395")                          # Domain 6, WE 6.4.1b
    check("10.2.2 Interpretation tax-shield qualification is 8 bp",
          (LLCR_D6 - lam_sculpt) * 10000, 8, D("0.5"))
    check("10.2.2 Interpretation LLCR - minimum DSCR gap", LLCR_D6 - D("1.1851"), "0.0544")
    # the identity itself: with cash independent of the schedule, sculpted lambda == LLCR
    flat = D(6000000)
    lam_flat = flat / DEBT * AF
    check("10.2.2 Interpretation identity holds where CFADS is schedule-independent",
          flat / lam_flat * AF, DEBT, D("0.01"))

    # ---------------------------------------------------------- WE 10.2.3
    BALLOON_B = DEBT * D("0.25")
    payB = (DEBT - BALLOON_B * DF12) / AF
    payC = DEBT * R
    LLCR = CF1 * AF / DEBT
    PLCR = CF1 * AF25 / DEBT
    ICR = EBITDA / INT1
    check("WE 10.2.3 Substitution 25% balloon amount", BALLOON_B, 10500000)
    check("WE 10.2.3 Result B debt service", payB, "4387226.43")
    check("WE 10.2.3 Result B DSCR", CF1 / payB, "1.4551")
    check("WE 10.2.3 Result C debt service", payC, "2520000.00")
    check("WE 10.2.3 Result C DSCR", CF1 / payC, "2.5333")
    check("WE 10.2.3 Result LLCR (all three structures)", LLCR, "1.2743")
    check("WE 10.2.3 Result PLCR (all three structures)", PLCR, "1.9431")
    check("WE 10.2.3 Result ICR on EBITDA", ICR, "2.9762")
    check("WE 10.2.3 Result DSCR/LLCR amortising", (CF1 / INST) / LLCR, "1.0000")
    check("WE 10.2.3 Result DSCR/LLCR 25% balloon", (CF1 / payB) / LLCR, "1.1419")
    check("WE 10.2.3 Result DSCR/LLCR bullet", (CF1 / payC) / LLCR, "1.9880")
    check("WE 10.2.3 Result maturity payment A", INST, "5009635.23")
    check("WE 10.2.3 Result maturity payment B", payB + BALLOON_B, "14887226.43")
    check("WE 10.2.3 Result maturity payment C", DEBT + payC, "44520000.00")
    check("WE 10.2.3 Interpretation B year-twelve coverage", CF1 / (payB + BALLOON_B), "0.4288")

    # ---------------------------------------------------------- WE 10.2.4
    PV_LOAN = npv([(r["t"], r["cfads"]) for r in MAND], R)
    check("WE 10.2.4 Setup PV of CFADS over the loan life at 6%", PV_LOAN, 52060092, MONEY)
    check("WE 10.2.4 Result PV/debt reproduces Domain 6's LLCR", PV_LOAN / DEBT, LLCR_D6)
    dCF = D("0.75") * REV
    dEB = D("0.9375") * REV
    check("WE 10.2.4 Formula CFADS falls per unit stress", dCF, 9000000)
    check("WE 10.2.4 Formula EBITDA falls per unit stress", dEB, 11250000)

    x_d1 = (MAND[0]["cfads"] - TH_COV) / dCF
    check("WE 10.2.4 Result DSCR year one breaks at %", x_d1 * 100, "4.1382")
    check("WE 10.2.4 Result DSCR year one revenue at trigger", REV * (1 - x_d1),
          11503416, MONEY)
    x_d12 = (MAND[11]["cfads"] - TH_COV) / dCF
    check("WE 10.2.4 Result DSCR year twelve shortfall", TH_COV - MAND[11]["cfads"],
          74849, MONEY)
    check("WE 10.2.4 Result DSCR year twelve stress %", x_d12 * 100, "-0.8317")
    check("WE 10.2.4 Result DSCR year twelve revenue needed", REV * (1 - x_d12),
          12099799, MONEY)
    lev_floor = DEBT / 6
    x_lev = (EBITDA - lev_floor) / dEB
    check("WE 10.2.4 Result debt/EBITDA floor", lev_floor, 7000000)
    check("WE 10.2.4 Result debt/EBITDA breaks at %", x_lev * 100, "4.4444")
    check("WE 10.2.4 Result debt/EBITDA revenue at trigger", REV * (1 - x_lev),
          11466667, MONEY)
    ll_floor = LAM_LOCK * DEBT
    x_ll = (PV_LOAN - ll_floor) / (dCF * AF)
    check("WE 10.2.4 Result LLCR PV floor", ll_floor, 48300000)
    check("WE 10.2.4 Result LLCR breaks at %", x_ll * 100, "4.9833")
    check("WE 10.2.4 Result LLCR revenue at trigger", REV * (1 - x_ll), 11402010, MONEY)
    icr_floor = D("2.50") * INT1
    x_icr = (EBITDA - icr_floor) / dEB
    check("WE 10.2.4 Result ICR 2.50x EBITDA floor", icr_floor, 6300000)
    check("WE 10.2.4 Result ICR 2.50x breaks at %", x_icr * 100, "10.6667")
    check("WE 10.2.4 Result ICR 2.50x revenue at trigger", REV * (1 - x_icr), 10720000, MONEY)
    x_icr29 = (EBITDA - D("2.90") * INT1) / dEB
    check("WE 10.2.4 Interpretation ICR at 2.90x numerator 192,000",
          EBITDA - D("2.90") * INT1, 192000)
    check("WE 10.2.4 Interpretation ICR at 2.90x binds first at %", x_icr29 * 100, "1.7067")
    check("MCQ 10.2-F gap DSCR -> leverage pp", (x_lev - x_d1) * 100, "0.31")
    check("WE 10.2.4 Interpretation the period test binds first",
          1 if x_d1 < min(x_lev, x_ll, x_icr) else 0, 1)
    check("WE 10.2.4 Interpretation ICR at 2.50x is 2.5x the DSCR tolerance",
          x_icr / x_d1, "2.578", D("0.01"))

    # ---------------------------------------------------------- 10.3.1 (cited, reconciled)
    DIV = D("774364.77")                           # Domain 15, KA 15.2.3
    MRA = D(600000)
    check("10.3.1 Domain 15's distributable amount reconciles", CF1 - INST - MRA, DIV)
    check("10.3.1 MRA charge as % of the base-case dividend (D15's 77.5%)",
          MRA / DIV * 100, "77.5", D("0.02"))      # display rounding of 77.4829

    # ---------------------------------------------------------- WE 10.3.2 tolerance
    DSRA6 = INST / 2
    check("WE 10.3.2 six-month DSRA", DSRA6, "2504817.62", D("0.006"))

    def tolerance(m):
        return 1 - INST * (1 - D(m) / 12) / CF1

    check("WE 10.3.2 Interpretation tolerance at 0 months %", tolerance(0) * 100, "21.5283")
    check("WE 10.3.2 Interpretation tolerance at 3 months %", tolerance(3) * 100, "41.1462")
    check("WE 10.3.2 Interpretation tolerance at 6 months %", tolerance(6) * 100, "60.7641")
    per_month = (tolerance(1) - tolerance(0)) * 100
    check("WE 10.3.2 Interpretation points of tolerance per month", per_month, "6.5393")
    check("WE 10.3.2 Interpretation funded cash per month", INST / 12, 417470, MONEY)
    check("WE 10.3.2 Interpretation cash per point of tolerance",
          (INST / 12) / per_month, 63840, D("0.01"))
    check("WE 10.3.2 Interpretation linearity: 6 months == 6 x one month",
          tolerance(6) - tolerance(0), 6 * (tolerance(1) - tolerance(0)), D("0.0000001"))

    # ---------------------------------------------------------- WE 10.3.2b funding routes
    KE = D("0.1542")                               # Domain 9, KA 9.1.3
    DEPOSIT = D("0.0300")                          # Domain 15, KA 15.4
    LC_FEE = D("0.0125")
    carry = DSRA6 * (KE - DEPOSIT)
    check("WE 10.3.2b Result breakeven LC fee %", (KE - DEPOSIT) * 100, "12.42")
    check("WE 10.3.2b Result equity carry a year", carry, 311098, MONEY)
    lc = DSRA6 * LC_FEE
    check("WE 10.3.2b Result LC at 1.25% a year", lc, 31310, MONEY)
    check("WE 10.3.2b Result LC saving a year", carry - lc, 279788, MONEY)
    AF8 = af(BOARD, N)
    check("WE 10.3.2b Substitution AF(8%,12)", AF8, "7.536078", TOL6)
    check("WE 10.3.2b Result PV of the LC saving at 8%", (carry - lc) * AF8, 2108505, MONEY)
    check("WE 10.3.2b Result after-tax debt cost a year", DSRA6 * R * (1 - T), 120231, MONEY)
    check("WE 10.3.2b Result capex capacity left", cap130 - DSRA6, 38666305, MONEY)
    check("WE 10.3.2b Result widened equity gap", DEBT - (cap130 - DSRA6), 3333695, MONEY)
    check("WE 10.3.2b Interpretation the debt route displaces capex one for one",
          (DEBT - (cap130 - DSRA6)) - gap, DSRA6, D("0.01"))

    # ---------------------------------------------------------- 10.3.3 distribution drought
    DRAW = INST - D(3000000)                       # MCQ 10.3-B bad-year gap
    check("10.3.3 MCQ 10.3-B reserve drawdown", DRAW, "2009635.23")
    check("10.3.3 years of dividend to replace the drawdown", DRAW / DIV, "2.5952")

    # ---------------------------------------------------------- WE 10.3.3 cash sweep
    SWEEP = DIV / 2
    APPLY = INST + SWEEP
    check("WE 10.3.3 Substitution sweep per year", SWEEP, "387182.39", D("0.006"))
    check("WE 10.3.3 Substitution total annual application", APPLY, "5396817.62", D("0.006"))

    def roll(total):
        """Repay from 42,000,000 at 6% applying `total` a year; final payment truncated."""
        bal, rows, paid = DEBT, [], D(0)
        while bal > 0:
            due = bal * (1 + R)
            pay = min(total, due)
            rows.append({"t": len(rows) + 1, "open": bal, "pay": pay, "prin": pay - bal * R})
            paid += pay
            bal = due - pay
        return rows, paid

    swept, swept_paid = roll(APPLY)
    check("WE 10.3.3 Result retirement year", len(swept), 11)
    check("WE 10.3.3 Result year-11 opening balance", swept[-1]["open"], "4081256.93")
    check("WE 10.3.3 Result final payment", swept[-1]["pay"], "4326132.35")
    # the base comparator is Domain 3's twelve-row schedule, not a truncating roll: the level
    # instalment is quoted to the cent, so a roll leaves a sub-cent residue and a phantom period.
    base_sched, bb = [], DEBT
    for t in range(1, N + 1):
        base_sched.append({"t": t, "open": bb, "pay": INST, "prin": INST - bb * R})
        bb -= INST - bb * R
    base_paid = INST * N
    check("WE 10.3.3 base schedule closes at zero", bb, 0, D("0.1"))
    check("WE 10.3.3 base schedule runs the full 12 years", len(base_sched), N)

    def wal(rows):
        return sum(D(r["t"]) * r["prin"] for r in rows) / DEBT

    check("WE 10.3.3 Result weighted average life, base", wal(base_sched), "7.1887")
    check("WE 10.3.3 Result weighted average life, swept", wal(swept), "6.4660")
    check("WE 10.3.3 Result reduction in average life",
          wal(base_sched) - wal(swept), "0.7227")
    check("WE 10.3.3 Interpretation reduction as % of the exposure period",
          (wal(base_sched) - wal(swept)) / wal(base_sched) * 100, "10.05")
    check("WE 10.3.3 Result total interest, base", base_paid - DEBT, "18115622.76")
    check("WE 10.3.3 Result total interest, swept", swept_paid - DEBT, "16294308.50")
    check("WE 10.3.3 Result interest saved", base_paid - swept_paid, 1821314, MONEY)
    check("WE 10.3.3 Result total cash swept", SWEEP * (len(swept) - 1), 3871824, MONEY)
    eq11 = CF1 - swept[-1]["pay"] - MRA
    check("WE 10.3.3 Result year-11 equity cash", eq11, 1457868, MONEY)
    check("WE 10.3.3 Result year-11 uplift over base", eq11 - DIV, "683502.88")
    pv_sweep = (-SWEEP * af(BOARD, len(swept) - 1)
                + (eq11 - DIV) / (1 + BOARD) ** len(swept)
                + INST / (1 + BOARD) ** N)
    check("WE 10.3.3 Result PV cost to equity at 8%", pv_sweep, -315488, MONEY)

    # ---------------------------------------------------------- WE 10.4.1 historic vs forward
    check("WE 10.4.1 Setup CFADS year 10", MAND[9]["cfads"], 6040690, MONEY)
    check("WE 10.4.1 Setup CFADS year 11", MAND[10]["cfads"], 5990216, MONEY)
    check("WE 10.4.1 Setup CFADS year 12", MAND[11]["cfads"], 5936713, MONEY)
    check("WE 10.4.1 Formula threshold in cash", TH_COV, 6011562, MONEY)
    for t, want in ((9, "1.2153"), (10, "1.2058"), (11, "1.1957"), (12, "1.1851")):
        check(f"WE 10.4.1 Result historic DSCR at end of year {t}",
              MAND[t - 1]["dscr"], want)
    first_hist = min(r["t"] for r in MAND if r["cfads"] < TH_COV)
    check("WE 10.4.1 Result historic test first fails at end of year 11", first_hist, 11)
    check("WE 10.4.1 Result forward test first fails at end of year 10", first_hist - 1, 10)

    # ---------------------------------------------------------- WE 10.4.2 lock-up
    dist = [r["cfads"] - INST - MRA for r in MAND]
    total_dist = sum(dist)
    check("WE 10.4.2 Result total distributable over the loan", total_dist, 6867502, MONEY)

    def caught(threshold):
        idx = [k for k in range(N) if MAND[k]["cfads"] < threshold]
        cash = sum(dist[k] for k in idx)
        pv = npv([(k + 1, dist[k]) for k in idx], BOARD)
        return [k + 1 for k in idx], cash, pv

    yrs, cash125, pv125 = caught(TH_DIST)
    check("WE 10.4.2 Result 1.25x first caught year", yrs[0], 5)
    check("WE 10.4.2 Result 1.25x number of caught years", len(yrs), 8)
    check("WE 10.4.2 Result 1.25x CFADS in the first caught year",
          MAND[4]["cfads"], 6253306, MONEY)
    check("WE 10.4.2 Result 1.25x cash affected", cash125, 3956574, MONEY)
    check("WE 10.4.2 Result 1.25x present value at 8%", pv125, 2165274, MONEY)
    check("WE 10.4.2 Result 1.25x share of distributable cash %",
          cash125 / total_dist * 100, "57.61")
    yrs120, cash120, _ = caught(TH_COV)
    check("WE 10.4.2 Result 1.20x caught years", len(yrs120), 2)
    check("WE 10.4.2 Result 1.20x first caught year", yrs120[0], 11)
    check("WE 10.4.2 Result 1.20x cash affected", cash120, 707658, MONEY)
    check("WE 10.4.2 Result 1.20x year-11 component", dist[10], "380580.31")
    check("WE 10.4.2 Result 1.20x year-12 component", dist[11], "327077.62")
    yrs115, _, _ = caught(TH_LOCK)
    check("WE 10.4.2 Result 1.15x catches no year", len(yrs115), 0)
    check("WE 10.4.2 Result minimum CFADS clears the 1.15x trigger",
          min(r["cfads"] for r in MAND) - TH_LOCK, 175632, MONEY)

    # ---------------------------------------------------------- WE 10.4.3 cures
    LAMC = D("1.20")
    cf11, cf12 = MAND[10]["cfads"], MAND[11]["cfads"]
    C11 = LAMC * INST - cf11
    P11 = INST - cf11 / LAMC
    check("WE 10.4.3 Result / MCQ 10.4-D year-11 cure, deemed CFADS", C11, "21346.73")
    check("WE 10.4.3 Result / MCQ 10.4-D year-11 cure, prepayment", P11, "17788.94")
    check("WE 10.4.3 Formula identity P = C / lambda", P11, C11 / LAMC, D("0.000001"))
    bal11 = MAND[11]["int"] / R                    # opening balance of the final period
    check("WE 10.4.3 Substitution balance after year 11", bal11, 4726071, MONEY)
    nb = bal11 - P11
    ni = nb * R
    nds = nb * (1 + R)
    ncf = A + T * ni
    check("WE 10.4.3 Substitution reduced balance", nb, 4708282, MONEY)
    check("WE 10.4.3 Substitution reduced year-12 interest", ni, 282497, MONEY)
    check("WE 10.4.3 Result year-12 debt service after the year-11 prepayment",
          nds, "4990779", MONEY)
    check("WE 10.4.3 Result year-12 DSCR under treatment (b)", ncf / nds, "1.1895")
    check("WE 10.4.3 Result year-12 DSCR under treatment (a)", cf12 / INST, "1.1851")
    C12 = LAMC * INST - cf12
    P12 = nds - ncf / LAMC
    check("WE 10.4.3 Result year-12 cure, deemed CFADS", C12, 74849, MONEY)
    check("WE 10.4.3 Result year-12 cure, prepayment", P12, 43696, MONEY)
    check("WE 10.4.3 Result total under treatment (a)", C11 + C12, 96196, MONEY)
    check("WE 10.4.3 Result total under treatment (b)", P11 + P12, 61485, MONEY)
    check("WE 10.4.3 Result saving", (C11 + C12) - (P11 + P12), 34711, MONEY)
    check("WE 10.4.3 Result saving %",
          ((C11 + C12) - (P11 + P12)) / (C11 + C12) * 100, "36.08")
    check("WE 10.4.3 Result Domain 6's cure total reconciles", C11 + C12, 96196, MONEY)
    check("WE 10.4.3 Interpretation 1 - 1/lambda at 1.20x %", (1 - 1 / LAMC) * 100, "16.6667")
    check("WE 10.4.3 Interpretation 1 - 1/lambda at 1.30x %", (1 - 1 / LAM) * 100, "23.0769")
    check("WE 10.4.3 Interpretation treatment (a) restores exactly 1.2000",
          (cf12 + C12) / INST, "1.2000")
    check("MCQ 10.4-D distractor C (multiplying by the covenant)", C11 * LAMC, "25616.08")

    # ---------------------------------------------------------- Fig 10.1.2 spec
    check("Fig 10.1.2 label 41.17m", cap130 / 1000000, "41.17", D("0.005"))
    check("Fig 10.1.2 label 38.25m", debtmin / 1000000, "38.25", D("0.005"))
    check("Fig 10.1.2 label 39.92m", sculpt / 1000000, "39.92", D("0.005"))
    check("Fig 10.1.2 header 2,917,226", cap130 - debtmin, 2917226, MONEY)
    check("Fig 10.1.2 header 1,661,916", sculpt - debtmin, 1661916, MONEY)
    check("Fig 10.1.2 axis floor 1.18 below every series",
          1 if min(r["dscr"] for r in base_rows) > D("1.18") else 0, 1)

    # ---------------------------------------------------------- Fig 10.2.2 spec
    for name, val, want in (("DSCR y1", x_d1, "4.1382"), ("debt/EBITDA", x_lev, "4.4444"),
                            ("LLCR", x_ll, "4.9833"), ("ICR 2.50x", x_icr, "10.6667"),
                            ("DSCR y12", x_d12, "-0.8317"),
                            ("ICR 2.90x marker", x_icr29, "1.7067")):
        check(f"Fig 10.2.2 bar {name} %", val * 100, want)
    check("Fig 10.2.2 bar label CFADS 6,011,562", TH_COV, 6011562, MONEY)
    check("Fig 10.2.2 bar label EBITDA 7,000,000", lev_floor, 7000000)
    check("Fig 10.2.2 bar label PV(CFADS) 48,300,000", ll_floor, 48300000)
    check("Fig 10.2.2 bar label EBITDA 6,300,000", icr_floor, 6300000)
    check("Fig 10.2.2 annotation 74,849 short", TH_COV - cf12, 74849, MONEY)

    # ---------------------------------------------------------- Exercise 10.5
    A5, R5, N5, T5, L5 = D(8400000), D("0.07"), 10, D("0.25"), D("1.35")
    rs5 = R5 * (1 - T5 / L5)
    check("Ex 10.5 effective sculpting rate %", rs5 * 100, "5.703704", TOL6)
    check("Ex 10.5 AF(r*,10)", af(rs5, N5), "7.464519", TOL6)
    check("Ex 10.5 sizing basis A/lambda", A5 / L5, 6222222, MONEY)
    debt5 = A5 / L5 * af(rs5, N5)
    check("Ex 10.5 maximum sculpted debt", debt5, 46445895, MONEY)
    check("Ex 10.5 AF(0.07,10)", af(R5, N5), "7.023582", TOL6)
    err5 = A5 / L5 * af(R5, N5)
    check("Ex 10.5 error at the loan rate", err5, 43702285, MONEY)
    check("Ex 10.5 understatement", debt5 - err5, 2743609, MONEY)
    bal5, first5, last5 = debt5, None, None
    for t in range(1, N5 + 1):
        i5 = bal5 * R5
        ds5 = (A5 + T5 * i5) / L5
        if t == 1:
            first5 = ds5
        last5 = ds5
        bal5 = bal5 * (1 + R5) - ds5
    check("Ex 10.5 schedule closes at zero", bal5, 0, D("0.01"))
    check("Ex 10.5 first-period debt service (not A/lambda)", first5, 6824299, MONEY)
    check("Ex 10.5 tenth-period debt service (the profile declines)", last5, 6298528, MONEY)

    # ---------------------------------------------------------- Exercise 10.6
    D6_, R6, N6, CF6 = D(40000000), D("0.065"), 10, D(7000000)
    AF6 = af(R6, N6)
    DF6 = (1 + R6) ** -N6
    check("Ex 10.6 AF(6.5%,10)", AF6, "7.188830", TOL6)
    check("Ex 10.6 1.065^-10", DF6, "0.532726", TOL6)
    pay6 = D6_ / AF6
    llcr6 = CF6 * AF6 / D6_
    check("Ex 10.6 full amortisation payment", pay6, 5564188, MONEY)
    check("Ex 10.6 full amortisation DSCR", CF6 / pay6, "1.2580")
    check("Ex 10.6 full amortisation LLCR", llcr6, "1.2580")
    check("Ex 10.6 full amortisation DSCR/LLCR", (CF6 / pay6) / llcr6, "1.0000")
    bal6 = D6_ * D("0.30")
    pay6b = (D6_ - bal6 * DF6) / AF6
    check("Ex 10.6 30% balloon amount", bal6, 12000000)
    check("Ex 10.6 balloon payment", pay6b, 4674931, MONEY)
    check("Ex 10.6 balloon DSCR", CF6 / pay6b, "1.4973")
    check("Ex 10.6 balloon DSCR/LLCR", (CF6 / pay6b) / llcr6, "1.1902")
    check("Ex 10.6 year-ten obligation", pay6b + bal6, 16674931, MONEY)
    check("Ex 10.6 year-ten coverage", CF6 / (pay6b + bal6), "0.4198")

    # ---------------------------------------------------------- Exercise 10.7
    REV7, EB7, DEP7, INT7, T7 = D(20000000), D(9000000), D(3000000), D(2600000), D("0.25")
    DS7, DEBT7 = D(6500000), D(40000000)
    CF7 = EB7 - T7 * (EB7 - DEP7 - INT7)
    check("Ex 10.7 CFADS", CF7, 8150000)
    check("Ex 10.7 DSCR", CF7 / DS7, "1.2538")
    dEB7 = D("0.90") * REV7
    dCF7 = (1 - T7) * dEB7
    check("Ex 10.7 EBITDA falls per unit stress", dEB7, 18000000)
    check("Ex 10.7 CFADS falls per unit stress", dCF7, 13500000)
    f_ds = D("1.20") * DS7
    f_icr = D("3.00") * INT7
    f_lev = DEBT7 / D("5.5")
    check("Ex 10.7 CFADS floor", f_ds, 7800000)
    check("Ex 10.7 ICR EBITDA floor", f_icr, 7800000)
    check("Ex 10.7 leverage EBITDA floor", f_lev, 7272727, MONEY)
    x7d = (CF7 - f_ds) / dCF7
    x7i = (EB7 - f_icr) / dEB7
    x7l = (EB7 - f_lev) / dEB7
    check("Ex 10.7 DSCR binds at %", x7d * 100, "2.5926")
    check("Ex 10.7 ICR binds at %", x7i * 100, "6.6667")
    check("Ex 10.7 leverage binds at %", x7l * 100, "9.5960")
    check("Ex 10.7 DSCR binds first", 1 if x7d < min(x7i, x7l) else 0, 1)
    check("Ex 10.7 common error: DSCR headroom in its own units", CF7 / DS7 - D("1.20"),
          "0.0538")
    check("Ex 10.7 common error: ICR headroom in its own units", EB7 / INT7 - D("3.00"),
          "0.4615")
    check("Ex 10.7 common error: leverage headroom in its own units",
          D("5.5") - DEBT7 / EB7, "1.0556")

    # ---------------------------------------------------------- Exercise 10.8
    L8, CF8, DS8 = D("1.25"), D(7600000), D(6500000)
    C8 = L8 * DS8 - CF8
    P8 = DS8 - CF8 / L8
    check("Ex 10.8 deemed-CFADS cure", C8, 525000)
    check("Ex 10.8 prepayment cure", P8, 420000)
    check("Ex 10.8 identity P = C / lambda", P8, C8 / L8, D("0.000001"))
    check("Ex 10.8 saving", C8 - P8, 105000)
    check("Ex 10.8 saving %", (1 - 1 / L8) * 100, "20.0000")
    check("Ex 10.8 common error: multiplying by the covenant", C8 * L8, 656250)

    # ---------------------------------------------------------- Exercise 10.9
    DS9, CF9 = D(6500000), D(8150000)

    def tol9(m):
        return 1 - DS9 * (1 - D(m) / 12) / CF9

    check("Ex 10.9 tolerance at 0 months %", tol9(0) * 100, "20.2454")
    check("Ex 10.9 tolerance at 3 months %", tol9(3) * 100, "40.1840")
    check("Ex 10.9 tolerance at 6 months %", tol9(6) * 100, "60.1227")
    check("Ex 10.9 points per month", (tol9(1) - tol9(0)) * 100, "6.6462")
    check("Ex 10.9 funded cash per month", DS9 / 12, 541667, MONEY)
    check("Ex 10.9 three-month reserve", DS9 * 3 / 12, 1625000)
    check("Ex 10.9 breakeven LC fee %", D("14.0") - D("2.5"), "11.50")
    check("Ex 10.9 equity carry on a three-month reserve", D(1625000) * D("0.115"), 186875)
    check("Ex 10.9 LC at 1.0%", D(1625000) * D("0.01"), 16250)
    check("Ex 10.9 saving", D(1625000) * (D("0.115") - D("0.01")), 170625)
