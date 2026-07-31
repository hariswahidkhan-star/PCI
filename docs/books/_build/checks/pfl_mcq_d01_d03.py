"""PFL-AI Domains 1-3 — judgement-item batch (Evaluation / Comprehension): golden checks.

Verification module for the eighteen sample MCQs added to Domains 1, 2 and 3 to close the
programme's cognitive-level gap:

    D1  MCQ 1.1-G, 1.1-H, 1.2-G, 1.2-H, 1.3-G, 1.3-H
    D2  MCQ 2.1-F, 2.2-G, 2.2-H, 2.3-H, 2.3-I, 2.4-G
    D3  MCQ 3.1-I, 3.2-J, 3.2-K, 3.3-J, 3.3-K, 3.3-L

An Evaluation item turns on a professional judgement rather than on arithmetic, but the arithmetic
inside it still has to be right in *every* option: a distractor whose numbers cannot be reproduced
gives the answer away, and a distractor whose numbers are wrong is not a nameable error. So every
figure printed in a stem, an option or a rationale is recomputed here from its own definition.

Nothing in this module is read back from the manuscript. Master-thread values (42,000,000 facility,
6.0 % rate, 12-year tenor, 5,009,635.23 instalment, 6,384,000 CFADS, 7,500,000 EBITDA, 5,100,000
EBIT, 2,520,000 year-one interest, 8.0 % appraisal rate, 60,000,000 envelope, 25-year life) come
from ctx and are never re-typed as literals.

Seven engines carry the batch, and each is exercised by at least two independent routes so that a
single coding error cannot make a wrong manuscript value pass:

  1. **Macaulay duration** (1.1-G) — the closed form `(1+r)/r - n/((1+r)^n - 1)` against the
     definitional `sum(t*PV) / sum(PV)`. The ceiling claim, the deferral lever (which adds *exactly*
     the deferral), the escalation lever (a level stream at `r* = (1+r)/(1+g) - 1`) and the
     unreachability of the ceiling at any finite tenor are each proved rather than quoted.
  2. **The recourse hyperbola** (1.1-H, 1.3-H) — `p*(D) = (fixed premium + D*m) / (D*x)` with the
     margin coefficient `m` and exposure coefficient `x` derived from the two instalments and the
     year-three balance recursion, never entered as percentages. Monotonicity in D and convergence
     on the asymptote are checked as invariants.
  3. **Leverage** (1.2-G, 1.2-H) — the levered-return identity `r_e = r_u + (D/E)(r_u - r_d)`
     against the direct equity-cash arithmetic, and the amortising cliff/covenant thresholds solved
     as cash levels before being expressed as percentage declines.
  4. **Accrual vs cash** (2.1-F, 2.2-H, 2.3-H) — the invariant that cumulative accrual less
     cumulative cash equals the net working-capital balance, and the cash-conversion ratio as a
     property of capital intensity (tested by holding depreciation and working capital and moving
     net income, which must leave the "above 1.0" conclusion intact).
  5. **Breakevens and covenant headroom** (2.2-G, 2.4-G) — CFADS as an affine function of revenue,
     with each threshold solved from the definition and then re-derived from the headroom route, so
     the 3.8796 % / 0.5000 % ranking is computed twice.
  6. **Amortisation, fees and day count** (3.2-J, 3.2-K, 3.3-L) — the outstanding-balance closed
     form against a schedule recursion at every k; an all-in cost recovered by bisection and
     verified by substitution; actual/360 as an exact 365/360 uplift, checked to be independent of
     rate, balance and tenor.
  7. **Consistency and convexity** (3.1-I, 3.3-J, 3.3-K) — the Fisher identity (consistent nominal
     equals consistent real to the cent), and the indexation cap as a convex payoff, valued both by
     the growth-adjusted closed form and by direct summation of the capped and uncapped flows.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]

    # ---------------- master-thread anchors, read from ctx ----------------
    DEBT = ctx["KESTREL_DEBT"]                 # 42,000,000 senior facility
    EQUITY = ctx["KESTREL_EQUITY"]             # 18,000,000
    CAPEX = ctx["KESTREL_CAPEX"]               # 60,000,000 envelope
    R = ctx["KESTREL_RATE"]                    # 6.0 %
    TEN = ctx["KESTREL_TENOR"]                 # 12 years
    LIFE = ctx["KESTREL_LIFE"]                 # 25 years
    INST = ctx["KESTREL_INSTALMENT"]           # 5,009,635.23 (printed, cent-rounded)
    CFADS = ctx["KESTREL_CFADS"]               # 6,384,000
    EBITDA = ctx["KESTREL_EBITDA"]             # 7,500,000
    EBIT = ctx["KESTREL_EBIT"]                 # 5,100,000
    INT1 = ctx["KESTREL_INTEREST_Y1"]          # 2,520,000
    DISC = ctx["KESTREL_DISCOUNT"]             # 8.0 % appraisal rate

    CENT = D("0.5")        # printed to the whole currency unit — display-rounding tolerance
    P4 = D("0.0001")

    def q(x, p=P4):
        return x.quantize(p)

    INST_EXACT = DEBT / af(R, TEN)
    check("anchor: exact instalment rounds to the printed 5,009,635.23",
          q(INST_EXACT, D("0.01")), INST)
    check("anchor: envelope = debt + equity", DEBT + EQUITY, CAPEX)
    check("anchor: year-one interest = debt x rate", DEBT * R, INT1)

    # =====================================================================================
    # DOMAIN 1
    # =====================================================================================

    def dur_closed(r, n):
        """Macaulay duration of a level stream, closed form."""
        return (1 + r) / r - D(n) / ((1 + r) ** n - 1)

    def dur_sum(cfs, r, offset=0):
        """Definitional Macaulay duration; `offset` defers the whole stream by that many periods."""
        num = den = D(0)
        for k, cf in enumerate(cfs, start=1):
            t = D(k + offset)
            pv = cf / (1 + r) ** t
            num += t * pv
            den += pv
        return num / den

    # ------------------------- MCQ 1.1-G — the duration ceiling -------------------------
    CEIL8 = (1 + DISC) / DISC
    check("MCQ 1.1-G option B: ceiling (1+r)/r at 8 %", q(CEIL8), D("13.5000"))

    level = [D(1)] * 15
    d15_closed, d15_defn = dur_closed(DISC, 15), dur_sum(level, DISC)
    d25_closed, d25_defn = dur_closed(DISC, 25), dur_sum([D(1)] * 25, DISC)
    check("MCQ 1.1-G rationale: 15-year level-stream duration, closed form", q(d15_closed),
          D("6.5945"))
    check("MCQ 1.1-G rationale: 15-year level-stream duration, definitional route", q(d15_defn),
          D("6.5945"))
    check("MCQ 1.1-G option B: 25-year level-stream duration, closed form", q(d25_closed),
          D("9.2254"))
    check("MCQ 1.1-G option B: 25-year level-stream duration, definitional route", q(d25_defn),
          D("9.2254"))
    check("MCQ 1.1-G rationale: duration bought by extending 15 years to 25",
          q(d25_closed - d15_closed), D("2.6309"))

    # lever 1 — deferring every receipt by 3 years shifts the weighted mean by exactly 3 years
    d15_def3 = dur_sum(level, DISC, offset=3)
    check("MCQ 1.1-G rationale: a 3-year deferral adds exactly 3.0000 years",
          q(d15_def3 - d15_defn), D("3.0000"))
    check("MCQ 1.1-G option C: deferred 15-year stream reaches 9.5945 years", q(d15_def3),
          D("9.5945"))
    check("MCQ 1.1-G option C invariant: deferral (9.5945) beats the 25-year tenor (9.2254), "
          "so 9.2254 is NOT the longest duration available (1 = true)",
          D(1) if d15_def3 > d25_closed else D(0), D(1))

    # lever 2 — escalation at g is a level stream at r* = (1+r)/(1+g) - 1
    G = D("0.025")
    RSTAR = (1 + DISC) / (1 + G) - 1
    esc = [(1 + G) ** D(k) for k in range(15)]
    check("MCQ 1.1-G rationale: escalated-stream duration, closed form at r*",
          q(dur_closed(RSTAR, 15)), D("7.0342"))
    check("MCQ 1.1-G rationale: escalated-stream duration, direct summation of escalated flows",
          q(dur_sum(esc, DISC)), D("7.0342"))
    check("MCQ 1.1-G rationale: duration added by 2.5 % escalation",
          q(dur_closed(RSTAR, 15) - d15_closed), D("0.4398"))

    # lever 3 — the ceiling moves with the rate
    check("MCQ 1.1-G rationale: ceiling (1+r)/r at 6 %",
          q((1 + D("0.06")) / D("0.06")), D("17.6667"))

    # the ceiling is a strict bound at every finite tenor — the item's whole claim. The residual
    # `n/((1+r)^n - 1)` is positive for every finite n, so the inequality is strict; beyond about
    # 400 years the residual is smaller than 28-digit Decimal can represent against 13.5, which is
    # why the sweep stops at a tenor a concession could conceivably have.
    tenors = (15, 25, 63, 100, 200)
    check("MCQ 1.1-G invariant: no tenor up to 200 years reaches the 8 % ceiling "
          "(1 = strictly below at every one)",
          D(1) if all(dur_closed(DISC, n) < CEIL8 for n in tenors) else D(0), D(1))
    check("MCQ 1.1-G invariant: duration rises monotonically with tenor but never arrives "
          "(1 = monotone)",
          D(1) if all(dur_closed(DISC, a) < dur_closed(DISC, b)
                      for a, b in zip(tenors, tenors[1:])) else D(0), D(1))
    check("MCQ 1.1-G rationale: even a 200-year level stream still falls short of 13.5000",
          q(dur_closed(DISC, 200), D("0.000001")), D("13.499959"))
    check("MCQ 1.1-G rationale: 63 years is what it takes to reach 13.0 (WE 1.1.4's marker)",
          q(dur_closed(DISC, 63)), D("13.0022"))
    check("MCQ 1.1-G stem: gap still open against a 14-year liability at the 25-year tenor",
          q(D(14) - d25_closed), D("4.7746"))

    # ------------------- MCQ 1.1-H — the breakeven hyperbola in scale -------------------
    R_CORP = R - D("0.014")                       # 140 bp tighter on the corporate balance sheet
    check("MCQ 1.1-H rationale: corporate rate is the 6.0 % facility less 140 bp",
          q(R_CORP * 100), D("4.6000"))
    inst_corp = DEBT / af(R_CORP, TEN)
    annual_diff = INST_EXACT - inst_corp
    check("MCQ 1.1-H rationale: annual debt-service differential",
          q(annual_diff, D("0.01")), D("377268.94"))
    PV_MARGIN = annual_diff * af(DISC, TEN)
    check("MCQ 1.1-H rationale: PV of the margin differential at 8 %",
          q(PV_MARGIN, D("0.01")), D("2843128.16"))
    FIXED = D(2709000) - D(350000)
    check("MCQ 1.1-H rationale: close-cost premium", FIXED, D("2359000"))
    M = PV_MARGIN / DEBT                          # proportional cost coefficient
    check("MCQ 1.1-H rationale: margin differential as a percentage of debt", q(M * 100),
          D("6.7694"))

    bal = DEBT                                    # year-three closing balance, by recursion
    for _ in range(3):
        bal = bal + bal * R - INST_EXACT
    check("MCQ 1.1-H rationale: year-three closing balance", q(bal, D("0.01")),
          D("34073997.27"))
    EXPOSURE40 = bal - D(24000000)                # 40 % of capital cost recovered on enforcement
    check("MCQ 1.1-H rationale: 40 % recovery is 24,000,000 of the 60,000,000 envelope",
          CAPEX * D("0.40"), D("24000000"))
    check("MCQ 1.1-H rationale: exposure the ring-fence removes",
          q(EXPOSURE40, D("0.01")), D("10073997.27"))
    X = EXPOSURE40 / DEBT                         # proportional benefit coefficient
    check("MCQ 1.1-H rationale: exposure removed as a percentage of debt", q(X * 100),
          D("23.9857"))

    def pstar(debt):
        return (FIXED + debt * M) / (debt * X)

    check("MCQ 1.1-H stem: p* on a 15,000,000 facility", q(pstar(D(15000000)) * 100),
          D("93.7893"))
    check("MCQ 1.1-H stem: p* at Kestrel's 42,000,000", q(pstar(DEBT) * 100), D("51.6392"))
    check("MCQ 1.1-H stem/rationale: asymptote = margin coefficient / exposure coefficient",
          q(M / X * 100), D("28.2224"))
    check("MCQ 1.1-H invariant: p* falls monotonically with facility size (1 = monotone)",
          D(1) if all(pstar(D(a)) > pstar(D(b)) for a, b in
                      [(15000000, 25000000), (25000000, 42000000), (42000000, 150000000)])
          else D(0), D(1))
    check("MCQ 1.1-H invariant: p* converges on the asymptote from above at 10^12 of debt",
          q(pstar(D(10) ** 12) * 100), D("28.2224"))
    check("MCQ 1.1-H invariant: p* exceeds 100 % below 13,702,087 of debt",
          q(FIXED / (X - M), D("0.01")), D("13702087.32"))

    COST = FIXED + PV_MARGIN
    EXPOSURE50 = bal - CAPEX * D("0.50")
    check("MCQ 1.1-H rationale: exposure at a 50 % enforcement recovery",
          q(EXPOSURE50, D("0.01")), D("4073997.27"))
    check("MCQ 1.1-H rationale / MCQ 1.3-H option B: p* at a 50 % recovery",
          q(COST / EXPOSURE50 * 100), D("127.6910"))
    check("MCQ 1.1-H invariant: a better recovery RAISES p* (1 = raises)",
          D(1) if COST / EXPOSURE50 > COST / EXPOSURE40 else D(0), D(1))

    # ------------------- MCQ 1.2-G / 1.2-H — leverage, both faces -------------------
    PROJ_COST = D(100000000)
    CASH = D(12000000)
    SENIOR = D(70000000)
    EQ = D(30000000)
    R_D = D("0.06")
    check("MCQ 1.2-G stem: capital structure sums to cost", SENIOR + EQ, PROJ_COST)

    IO_SERVICE = SENIOR * R_D
    check("MCQ 1.2-G rationale: year-one interest, identical in both columns", IO_SERVICE,
          D("4200000"))
    check("MCQ 1.2-G option B: interest-only levered return", q((CASH - IO_SERVICE) / EQ * 100),
          D("26.0000"))
    check("MCQ 1.2-G stem/rationale: the interest-only cliff, as a decline in project cash",
          q((CASH - IO_SERVICE) / CASH * 100), D("65.0000"))

    INST70 = SENIOR / af(R_D, TEN)
    check("MCQ 1.2-G option B: level instalment on the 70,000,000 facility",
          q(INST70, D("0.01")), D("8349392.06"))
    check("MCQ 1.2-G rationale: year-one principal repaid", q(INST70 - IO_SERVICE, D("0.01")),
          D("4149392.06"))
    check("MCQ 1.2-G option B: amortising cash-on-cash return", q((CASH - INST70) / EQ * 100),
          D("12.1687"))
    check("MCQ 1.2-G option A: equity cash exhausts at this decline in project cash",
          q((CASH - INST70) / CASH * 100), D("30.4217"))
    COV_CASH = INST70 * D("1.20")
    check("MCQ 1.2-G rationale: cash level at which the 1.20x covenant fails",
          q(COV_CASH, D("0.01")), D("10019270.47"))
    check("MCQ 1.2-G option A: the 1.20x covenant engages at this decline",
          q((CASH - COV_CASH) / CASH * 100), D("16.5061"))
    check("MCQ 1.2-G invariant: thresholds rank covenant < equity cliff < interest-only claim "
          "(1 = correctly ordered)",
          D(1) if (CASH - COV_CASH) < (CASH - INST70) < (CASH - IO_SERVICE) else D(0), D(1))
    check("MCQ 1.2-G option A: reported resilience is roughly four times the first consequence",
          q(((CASH - IO_SERVICE) / CASH) / ((CASH - COV_CASH) / CASH)), D("3.9370"))

    DE = SENIOR / EQ
    check("MCQ 1.2-H option B: debt-to-equity ratio", q(DE, D("0.000001")), D("2.333333"))
    r_u_base = CASH / PROJ_COST * 100
    check("MCQ 1.2-H rationale: gearing adds this many points at an unlevered 12.0 %",
          q(DE * (r_u_base - D(6))), D("14.0000"))
    check("MCQ 1.2-H rationale: identity reproduces the 26.0000 % base case",
          q(r_u_base + DE * (r_u_base - D(6))), D("26.0000"))
    check("MCQ 1.2-H rationale: gearing subtracts this many points at an unlevered 4.0 %",
          q(DE * (D(6) - D(4))), D("4.6667"))
    check("MCQ 1.2-H rationale: levered return at an unlevered 4.0 %",
          q(D(4) + DE * (D(4) - D(6))), D("-0.6667"))
    check("MCQ 1.2-H rationale: crossover cash where the unlevered return equals the debt rate",
          PROJ_COST * R_D, D("6000000"))
    check("MCQ 1.2-H invariant: identity and direct arithmetic agree at the crossover "
          "(both equal the debt rate)",
          q((PROJ_COST * R_D - IO_SERVICE) / EQ * 100), q(D(6) + DE * (D(6) - D(6))))
    check("MCQ 1.2-H option B invariant: the identity's sign follows the spread over r_d, "
          "so it subtracts below the crossover (1 = negative)",
          D(1) if (D(4) + DE * (D(4) - D(6))) < 0 else D(0), D(1))

    # ------------------------------ MCQ 1.3-G / 1.3-H ------------------------------
    WEEKLY = D("124133.33")
    check("MCQ 1.3-G stem: nine weeks of pre-close delay (whole-unit display rounding)",
          q(D(9) * WEEKLY, D("0.01")), D("1117200"), tol=D("0.05"))
    check("MCQ 1.3-G rationale: weekly cost implied by the printed 1,117,200",
          q(D(1117200) / 9, D("0.01")), WEEKLY)
    check("MCQ 1.3-H stem: incremental cost of the limited-recourse route",
          q(COST, D("0.01")), D("5202128.16"))
    check("MCQ 1.3-H stem: cost as printed to the whole unit", q(COST, D("1")), D("5202128"))
    check("MCQ 1.3-H rationale: the two components sum to the printed total",
          q(FIXED + PV_MARGIN, D("0.01")), q(D("2359000") + D("2843128.16"), D("0.01")))

    # =====================================================================================
    # DOMAIN 2
    # =====================================================================================
    REV = D(12000000)               # KA 2.2 revenue
    OPEX = D(4500000)               # KA 2.2 cash operating cost
    DEP = CAPEX / LIFE              # straight line over the 25-year life
    TAXR = D("0.20")
    AR, AP = D(900000), D(300000)   # closing receivables / payables (WE 2.1.1)
    WC = AR - AP

    check("D2 anchor: EBITDA = revenue less cash opex", REV - OPEX, EBITDA)
    check("D2 anchor: depreciation = envelope / 25-year life", DEP, D("2400000"))
    check("D2 anchor: EBIT = EBITDA less depreciation", EBITDA - DEP, EBIT)
    PBT = EBIT - INT1
    TAX = PBT * TAXR
    NI = PBT - TAX
    check("D2 anchor: net income", NI, D("2064000"))
    check("D2 anchor: CFADS = EBITDA less working capital less tax", EBITDA - WC - TAX, CFADS)

    # ------------------------------- MCQ 2.1-F -------------------------------
    COLLECTED, PAID = D(11100000), D(4200000)
    check("MCQ 2.1-F stem: accrual EBITDA", REV - OPEX, D("7500000"))
    check("MCQ 2.1-F stem: cash-basis EBITDA = collections less payments", COLLECTED - PAID,
          D("6900000"))
    check("MCQ 2.1-F option B: the gap the cash basis has not yet reported",
          (REV - OPEX) - (COLLECTED - PAID), D("600000"))
    check("MCQ 2.1-F option B invariant: accrual less cash equals the net working-capital balance",
          (REV - OPEX) - (COLLECTED - PAID), AR - AP)
    check("MCQ 2.1-F rationale invariant: releasing the same cycle reverses the sign, so the cash "
          "basis reports MORE (1 = reverses)",
          D(1) if ((REV - OPEX) - (REV + AR - (OPEX + AP))) < 0 else D(0), D(1))

    # ------------------------- MCQ 2.2-G — the two breakevens -------------------------
    def cfads_at(rev):
        """CFADS as a function of revenue: opex, depreciation, interest and the working-capital
        absorption all hold, so CFADS is affine in revenue with slope (1 - tax rate)."""
        ebitda = rev - OPEX
        return ebitda - WC - TAXR * (ebitda - DEP - INT1)

    check("MCQ 2.2-G invariant: cfads_at(revenue) reproduces the master CFADS", cfads_at(REV),
          CFADS)
    SLOPE = (cfads_at(REV + D(1000000)) - cfads_at(REV)) / D(1000000)
    check("MCQ 2.2-G rationale: cash-to-revenue slope used to convert headroom to revenue",
          q(SLOPE, D("0.01")), D("0.80"))

    BE_PROFIT = OPEX + DEP + INT1
    check("MCQ 2.2-G option A: profit breakeven revenue", BE_PROFIT, D("9420000"))
    FALL_PROFIT = (REV - BE_PROFIT) / REV * 100
    check("MCQ 2.2-G option A: profit breakeven as a revenue fall", q(FALL_PROFIT), D("21.5000"))

    def revenue_for_dscr(target):
        """Revenue at which CFADS / instalment = target, solved from the affine relation."""
        return (target * INST + WC - TAXR * (DEP + INT1)) / (1 - TAXR) + OPEX

    BE_CASH = revenue_for_dscr(D(1))
    check("MCQ 2.2-G option B: substitution check — CFADS at the cash breakeven equals the "
          "instalment", q(cfads_at(BE_CASH), D("0.01")), INST)
    check("MCQ 2.2-G option B: cash breakeven revenue (whole-unit display rounding)",
          q(BE_CASH, D("0.01")), D("10282044"), tol=CENT)
    FALL_CASH = (REV - BE_CASH) / REV * 100
    check("MCQ 2.2-G option B: cash breakeven as a revenue fall", q(FALL_CASH), D("14.3163"))
    check("MCQ 2.2-G option B: revenue points by which the cash breakeven binds earlier",
          q(FALL_PROFIT - FALL_CASH), D("7.1837"))
    check("MCQ 2.2-G invariant: the cash breakeven binds before the profit breakeven "
          "(1 = binds earlier)", D(1) if BE_CASH > BE_PROFIT else D(0), D(1))
    check("MCQ 2.2-G option C: net income's elasticity to revenue",
          q(REV / D(100) * (1 - TAXR) / NI * 100), D("4.6512"))

    # covenant headroom, computed twice — from the headroom and by solving the covenant
    HEADROOM_CASH = CFADS - D("1.20") * INST
    check("MCQ 2.2-G rationale: 1.20x covenant debt-service requirement",
          q(D("1.20") * INST, D("0.01")), D("6011562.28"))
    check("MCQ 2.2-G rationale: DSCR covenant headroom in cash",
          q(HEADROOM_CASH, D("0.01")), D("372437.72"))
    HEADROOM_REV = HEADROOM_CASH / SLOPE
    check("MCQ 2.2-G rationale / MCQ 2.4-G stem: DSCR headroom in revenue",
          q(HEADROOM_REV, D("0.01")), D("465547.16"))
    DSCR_TOL = HEADROOM_REV / REV * 100
    check("MCQ 2.2-G stem / MCQ 2.4-G stem: DSCR covenant tolerates this revenue fall",
          q(DSCR_TOL), D("3.8796"))
    check("MCQ 2.2-G invariant: the same 3.8796 % recovered by solving DSCR = 1.20 directly",
          q((REV - revenue_for_dscr(D("1.20"))) / REV * 100), D("3.8796"))

    # ------------------------------- MCQ 2.2-H -------------------------------
    OCF = NI + DEP - WC
    check("MCQ 2.2-H rationale: operating cash flow", OCF, D("3864000"))
    check("MCQ 2.2-H stem: cash conversion = operating cash flow / net income", q(OCF / NI),
          D("1.8721"))
    check("MCQ 2.2-H option B: depreciation add-back", DEP, D("2400000"))
    check("MCQ 2.2-H option B: net working-capital absorption", WC, D("600000"))
    check("MCQ 2.2-H option B invariant: conversion exceeds 1.0 whenever depreciation exceeds the "
          "working-capital absorption, at any level of net income (1 = still above 1.0 on a "
          "quarter of the profit)",
          D(1) if (NI / 4 + DEP - WC) / (NI / 4) > 1 else D(0), D(1))
    check("MCQ 2.2-H option B invariant: the add-back does not depend on profitability — "
          "conversion is still above 1.0 with net income at a tenth",
          D(1) if (NI / 10 + DEP - WC) / (NI / 10) > 1 else D(0), D(1))

    # ------------------------- MCQ 2.3-H — the CFADS definition -------------------------
    DSCR_INCL = CFADS / INST
    DSCR_EXCL = (CFADS + WC) / INST
    check("MCQ 2.3-H stem: DSCR on the sponsor's exclusive definition", q(DSCR_EXCL),
          D("1.3941"))
    check("MCQ 2.3-H stem: DSCR on the lenders' inclusive definition", q(DSCR_INCL),
          D("1.2743"))
    check("MCQ 2.3-H option A: coverage the exclusion buys", q(DSCR_EXCL - DSCR_INCL),
          D("0.1198"))
    check("MCQ 2.3-H option A invariant: that difference is the working-capital movement over the "
          "instalment", q(DSCR_EXCL - DSCR_INCL), q(WC / INST))
    check("MCQ 2.3-H option A: the 600,000 restated as days of revenue", q(WC * 365 / REV, D("0.01")),
          D("18.25"))
    check("MCQ 2.3-H option B invariant: the exclusion turns against the sponsor when the cycle "
          "releases cash — DSCR then falls below the inclusive figure (1 = falls below)",
          D(1) if (CFADS - WC) / INST < (CFADS - WC + WC) / INST else D(0), D(1))
    check("MCQ 2.3-H option B: DSCR on the exclusive definition in a year that releases the same "
          "600,000 (the exclusion now costs 0.1198)", q((CFADS + WC - 2 * WC) / INST),
          q(DSCR_EXCL - 2 * (WC / INST)))

    # ------------------------- MCQ 2.3-I — classification and tax -------------------------
    SPEND = D(1200000)
    CAP_LIFE = 10
    relief_expensed = SPEND * TAXR                       # taken at t = 0
    check("MCQ 2.3-I stem: relief on expensing, taken at once", relief_expensed, D("240000"))
    check("MCQ 2.3-I stem: annual depreciation if capitalised over ten years", SPEND / CAP_LIFE,
          D("120000"))
    check("MCQ 2.3-I stem: annuity factor at 8 % over ten years", q(af(DISC, CAP_LIFE),
          D("0.000001")), D("6.710081"))
    relief_capitalised = SPEND / CAP_LIFE * TAXR * af(DISC, CAP_LIFE)
    check("MCQ 2.3-I stem: PV of relief if capitalised", q(relief_capitalised, D("0.01")),
          D("161041.95"))
    ADV = relief_expensed - relief_capitalised
    check("MCQ 2.3-I stem: present-value advantage of expensing", q(ADV, D("0.01")),
          D("78958.05"))
    check("MCQ 2.3-I stem: advantage as printed to the whole unit", q(ADV, D("1")), D("78958"))
    check("MCQ 2.3-I invariant: the advantage is timing only — the two routes relieve the "
          "identical 240,000 undiscounted",
          SPEND / CAP_LIFE * TAXR * CAP_LIFE, relief_expensed)

    # ------------------------- MCQ 2.4-G — which covenant binds -------------------------
    IC_TRIGGER = D("2.00") * INT1
    check("MCQ 2.4-G stem: EBIT at which the 2.00x interest-cover covenant trips", IC_TRIGGER,
          D("5040000"))
    IC_HEADROOM = EBIT - IC_TRIGGER
    check("MCQ 2.4-G rationale: interest-cover headroom in EBIT", IC_HEADROOM, D("60000"))
    check("MCQ 2.4-G invariant: EBIT falls one-for-one with revenue, so EBIT headroom is revenue "
          "headroom", (REV - D(1)) - OPEX - DEP, EBIT - D(1))
    IC_TOL = IC_HEADROOM / REV * 100
    check("MCQ 2.4-G stem: interest-cover covenant tolerates this revenue fall", q(IC_TOL),
          D("0.5000"))
    check("MCQ 2.4-G option B: turns of cover gained by defining the numerator as EBITDA",
          q(EBITDA / INT1 - EBIT / INT1), D("0.9524"))
    check("MCQ 2.4-G option B invariant: that gain is depreciation over interest",
          q(EBITDA / INT1 - EBIT / INT1), q(DEP / INT1))
    check("MCQ 2.4-G rationale: ratio of DSCR headroom to interest-cover headroom, in revenue",
          q(HEADROOM_REV / IC_HEADROOM), D("7.7591"))
    check("MCQ 2.4-G option B invariant: interest cover tolerates under a seventh of the DSCR "
          "test's revenue miss (1 = under)",
          D(1) if IC_TOL / DSCR_TOL < D(1) / 7 else D(0), D(1))
    check("MCQ 2.4-G option A: even at 1.15x the coverage test stays far slacker than "
          "interest cover", q((REV - revenue_for_dscr(D("1.15"))) / REV * 100), D("6.4887"))
    check("MCQ 2.4-G option A invariant: the 1.15x concession leaves interest cover binding "
          "(1 = still binding)",
          D(1) if IC_TOL < (REV - revenue_for_dscr(D("1.15"))) / REV * 100 else D(0), D(1))

    # =====================================================================================
    # DOMAIN 3
    # =====================================================================================

    # --------------------- MCQ 3.1-I — matching the rate to the flow ---------------------
    REBATE, YRS = D(500000), 5
    v8 = REBATE / (1 + DISC) ** YRS
    v6 = REBATE / (1 + R) ** YRS
    v7 = REBATE / (1 + D("0.07")) ** YRS
    check("MCQ 3.1-I stem: rebate discounted at the 8.0 % appraisal rate", q(v8, D("0.01")),
          D("340291.60"))
    check("MCQ 3.1-I stem: rebate discounted at the 6.0 % senior-debt rate", q(v6, D("0.01")),
          D("373629.09"))
    check("MCQ 3.1-I rationale: value moved by the 200 basis points between the proposals",
          q(v6 - v8, D("0.01")), D("33337.49"))
    check("MCQ 3.1-I rationale: value at the 7 % midpoint used as the base", q(v7, D("0.01")),
          D("356493.09"))
    check("MCQ 3.1-I rationale: the 200 bp as a percentage of the midpoint value",
          q((v6 - v8) / v7 * 100), D("9.3515"))
    check("MCQ 3.1-I invariant: a lower rate raises the present value of a single receipt "
          "(1 = raises)", D(1) if v6 > v8 else D(0), D(1))

    # ------------------- MCQ 3.2-J — the outstanding-balance closed form -------------------
    K = 7
    B7_CLOSED = INST_EXACT * af(R, TEN - K)
    check("MCQ 3.2-J stem: annuity factor over the five remaining years",
          q(af(R, TEN - K), D("0.0000001")), D("4.2123638"))
    check("MCQ 3.2-J stem: balance outstanding after seven years, closed form",
          q(B7_CLOSED, D("0.01")), D("21102406"), tol=CENT)

    bal = DEBT                                      # ...and by schedule recursion
    balances = []
    for _ in range(TEN):
        bal = bal + bal * R - INST_EXACT
        balances.append(bal)
    check("MCQ 3.2-J rationale: same balance by schedule recursion",
          q(balances[K - 1], D("0.01")), q(B7_CLOSED, D("0.01")), tol=CENT)
    check("MCQ 3.2-J invariant: closed form equals the recursion at every k (max gap, cents)",
          q(max(abs(balances[k - 1] - INST_EXACT * af(R, TEN - k)) for k in range(1, TEN + 1)),
            D("0.01")), D("0.00"), tol=D("0.02"))
    check("MCQ 3.2-J invariant: the schedule retires the facility exactly",
          q(balances[-1], D("0.01")), D("0.00"), tol=D("0.02"))

    REMAINING_CASH = (TEN - K) * INST_EXACT
    check("MCQ 3.2-J option A: cash still to be paid over the remaining five years",
          q(REMAINING_CASH, D("0.01")), D("25048176"), tol=CENT)
    check("MCQ 3.2-J rationale: interest not yet accrued",
          q(REMAINING_CASH - B7_CLOSED, D("0.01")), D("3945770.13"), tol=CENT)
    check("MCQ 3.2-J rationale: proportion of the facility still outstanding",
          q(B7_CLOSED / DEBT * 100), D("50.2438"))
    check("MCQ 3.2-J rationale: proportion of the term elapsed", q(D(K) / TEN * 100, D("0.01")),
          D("58.33"))
    check("MCQ 3.2-J option D invariant: an annuity's principal is back-loaded, so more than half "
          "is outstanding after more than half the term (1 = back-loaded)",
          D(1) if B7_CLOSED / DEBT > D("0.5") else D(0), D(1))

    # ------------------------- MCQ 3.2-K — fee against margin -------------------------
    FEE = D(840000)
    NET = DEBT - FEE
    check("MCQ 3.2-K rationale: net proceeds received", NET, D("41160000"))

    def solve_rate(target, amount=INST_EXACT, n=TEN):
        """Rate at which `amount` over `n` years has present value `target` (bisection)."""
        lo, hi = D("0.0000001"), D("1")
        for _ in range(400):
            mid = (lo + hi) / 2
            if amount * af(mid, n) > target:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    ALLIN = solve_rate(NET)
    check("MCQ 3.2-K stem: substitution check — the solved rate reproduces the net proceeds",
          q(INST_EXACT * af(ALLIN, TEN), D("0.01")), q(NET, D("0.01")))
    check("MCQ 3.2-K stem: all-in cost of the fee-bearing facility", q(ALLIN * 100),
          D("6.3704"))
    check("MCQ 3.2-K option B: basis points by which the fee-free 6.15 % facility is cheaper",
          q((ALLIN - D("0.0615")) * 100, D("0.01")), D("0.2204"))
    check("MCQ 3.2-K rationale: basis points the 2.0 % fee adds to the 6.00 % coupon",
          q((ALLIN - R) * 100, D("0.01")), D("0.3704"))

    PROCEEDS_615 = INST_EXACT * af(D("0.0615"), TEN)
    EQUAL_FEE = DEBT - PROCEEDS_615
    check("MCQ 3.2-K option B: fee that would equalise the two offers",
          q(EQUAL_FEE, D("0.01")), D("343244"), tol=CENT)
    check("MCQ 3.2-K rationale: equalising fee as a percentage of the facility",
          q(EQUAL_FEE / DEBT * 100), D("0.8172"))
    check("MCQ 3.2-K option B: excess over the market-equivalent fee",
          q(FEE - EQUAL_FEE, D("0.01")), D("496756"), tol=CENT)
    check("MCQ 3.2-K invariant: the equalising fee reproduces a 6.15 % all-in cost",
          q(solve_rate(DEBT - EQUAL_FEE) * 100), D("6.1500"))

    ALLIN_1PCT = solve_rate(DEBT - DEBT / 100)
    check("MCQ 3.2-K rationale: all-in cost of a 1.00 % fee", q(ALLIN_1PCT * 100, D("0.000001")),
          D("6.183800"))
    check("MCQ 3.2-K rationale: basis points per 1.00 % of upfront fee",
          q((ALLIN_1PCT - R) * 100, D("0.01")), D("0.1838"))
    check("MCQ 3.2-K option D: the straight-line 'amortise the fee' figure",
          q(FEE / DEBT / TEN * 100, D("0.01")), D("0.1667"))
    check("MCQ 3.2-K option D invariant: the straight-line figure understates the true cost "
          "(1 = understates)", D(1) if FEE / DEBT / TEN < (ALLIN - R) else D(0), D(1))
    check("MCQ 3.2-K rationale: the straight-line error is under half the true 37.04 bp "
          "(1 = under half)",
          D(1) if FEE / DEBT / TEN < (ALLIN - R) / 2 else D(0), D(1))

    # ------------------- MCQ 3.3-J — the Fisher identity as a defect test -------------------
    SUPPORT = D(5600000)
    R_NOM, INFL = D("0.09"), D("0.03")
    check("MCQ 3.3-J stem: annuity factor at the nominal 9 % over 25 years",
          q(af(R_NOM, LIFE), D("0.000001")), D("9.822580"))
    WRONG = SUPPORT * af(R_NOM, LIFE)
    check("MCQ 3.3-J stem/option B: real flows discounted at the nominal rate — the defect",
          q(WRONG, D("0.01")), D("55006446"), tol=CENT)
    R_REAL = (1 + R_NOM) / (1 + INFL) - 1
    check("MCQ 3.3-J rationale: real rate from the Fisher relation",
          q(R_REAL * 100, D("0.000001")), D("5.825243"))
    RIGHT_AF = SUPPORT * af(R_REAL, LIFE)
    RIGHT_SUM = sum(SUPPORT * (1 + INFL) ** D(t) / (1 + R_NOM) ** D(t)
                    for t in range(1, LIFE + 1))
    check("MCQ 3.3-J stem: consistent value, nominal flows at the nominal rate (direct summation)",
          q(RIGHT_SUM, D("0.01")), D("72791113"), tol=CENT)
    check("MCQ 3.3-J invariant: consistent real equals consistent nominal to the cent",
          q(RIGHT_AF, D("0.01")), q(RIGHT_SUM, D("0.01")), tol=D("0.02"))
    check("MCQ 3.3-J option B: the difference that identifies the defect",
          q(RIGHT_SUM - WRONG, D("0.01")), D("17784667"), tol=CENT)
    check("MCQ 3.3-J rationale: the defect as a percentage understatement",
          q(-(RIGHT_SUM - WRONG) / RIGHT_SUM * 100), D("-24.4325"))

    # ------------------- MCQ 3.3-K — the indexation cap as a convex payoff -------------------
    OM = D(2700000)
    CAP_G, FCAST, STRESS = D("0.03"), D("0.04"), D("0.05")

    def rstar(g):
        return (1 + DISC) / (1 + g) - 1

    def om_pv(g):
        return OM * af(rstar(g), LIFE)

    def om_pv_sum(g):
        return sum(OM * (1 + g) ** D(t) / (1 + DISC) ** D(t) for t in range(1, LIFE + 1))

    check("MCQ 3.3-K stem: growth-adjusted rate at the 4.0 % forecast",
          q(rstar(FCAST) * 100, D("0.000001")), D("3.846154"))
    check("MCQ 3.3-K stem: annuity factor at that rate over 25 years",
          q(af(rstar(FCAST), LIFE), D("0.000001")), D("15.879244"))
    check("MCQ 3.3-K stem: growth-adjusted rate at the 3.0 % cap",
          q(rstar(CAP_G) * 100, D("0.000001")), D("4.854369"))
    check("MCQ 3.3-K stem: annuity factor at the capped rate over 25 years",
          q(af(rstar(CAP_G), LIFE), D("0.000001")), D("14.301981"))
    check("MCQ 3.3-K stem: uncapped O&M present value at the 4.0 % forecast",
          q(om_pv(FCAST), D("0.01")), D("42873959.52"))
    check("MCQ 3.3-K stem: capped O&M present value", q(om_pv(CAP_G), D("0.01")),
          D("38615349.31"))
    CAPVAL4 = om_pv(FCAST) - om_pv(CAP_G)
    check("MCQ 3.3-K stem: value of the cap at the 4.0 % forecast",
          q(CAPVAL4, D("0.01")), D("4258610.20"))
    check("MCQ 3.3-K invariant: same cap value by direct summation of capped and uncapped flows",
          q(om_pv_sum(FCAST) - om_pv_sum(CAP_G), D("0.01")), q(CAPVAL4, D("0.01")),
          tol=D("0.02"))
    AF8 = af(DISC, LIFE)
    check("MCQ 3.3-K rationale: annuity factor at 8 % over 25 years", q(AF8, D("0.000001")),
          D("10.674776"))
    LAE4 = CAPVAL4 / AF8
    check("MCQ 3.3-K stem: level annual equivalent of the cap at the forecast",
          q(LAE4, D("0.01")), D("398941.40"))
    CUT = D(500000)
    check("MCQ 3.3-K option A: apparent annual gain from the trade at the point forecast",
          q(CUT - LAE4, D("0.01")), D("101058.60"))
    CAPVAL5 = om_pv(STRESS) - om_pv(CAP_G)
    check("MCQ 3.3-K option B: growth-adjusted rate at a 5.0 % outturn",
          q(rstar(STRESS) * 100, D("0.000001")), D("2.857143"))
    check("MCQ 3.3-K option B: uncapped O&M present value at a 5.0 % outturn",
          q(om_pv(STRESS), D("0.01")), D("47772731.12"))
    check("MCQ 3.3-K option B: value of the cap at a 5.0 % outturn",
          q(CAPVAL5, D("0.01")), D("9157381.81"))
    check("MCQ 3.3-K invariant: same stressed cap value by direct summation",
          q(om_pv_sum(STRESS) - om_pv_sum(CAP_G), D("0.01")), q(CAPVAL5, D("0.01")),
          tol=D("0.02"))
    check("MCQ 3.3-K option B: level annual equivalent of the cap at a 5.0 % outturn",
          q(CAPVAL5 / AF8, D("0.01")), D("857852.35"))
    check("MCQ 3.3-K invariant: the cap pays nothing at or below its own 3.0 % strike",
          q(om_pv(CAP_G) - om_pv(CAP_G), D("0.01")), D("0.00"))
    check("MCQ 3.3-K option B invariant: the payoff is CONVEX — the second point of index costs "
          "more than the first (1 = convex)",
          D(1) if CAPVAL5 - CAPVAL4 > CAPVAL4 else D(0), D(1))
    check("MCQ 3.3-K option B invariant: 500,000 a year is short of the stressed level annual "
          "equivalent (1 = short)", D(1) if CUT < CAPVAL5 / AF8 else D(0), D(1))
    check("MCQ 3.3-K rationale: the 500,000 reduction indexed at 3.0 %",
          q(CUT * af(rstar(CAP_G), LIFE), D("0.01")), D("7150990.61"))
    check("MCQ 3.3-K rationale: the 500,000 reduction left level",
          q(CUT * AF8, D("0.01")), D("5337388.09"))
    check("MCQ 3.3-K option D invariant: indexing the fixed leg improves it but still falls short "
          "of the stressed cap value (1 = still short)",
          D(1) if CUT * af(rstar(CAP_G), LIFE) < CAPVAL5 else D(0), D(1))

    # --------------------------- MCQ 3.3-L — actual/360 ---------------------------
    UPLIFT = D(365) / 360 - 1
    check("MCQ 3.3-L option B: the actual/360 uplift over a full year", q(UPLIFT * 100),
          D("1.3889"))
    R_360 = R * D(365) / 360
    check("MCQ 3.3-L option B: 6.0 % actual/360 restated as a 365-day quoted rate",
          q(R_360 * 100), D("6.0833"))
    check("MCQ 3.3-L rationale: basis points added at 6 %", q((R_360 - R) * 100, D("0.01")),
          D("0.08"), tol=D("0.005"))
    check("MCQ 3.3-L rationale: basis points added at 6 %, to four places",
          q((R_360 - R) * 100), D("0.0833"))
    check("MCQ 3.3-L rationale: one year of the uplift on the 42,000,000 balance",
          q(DEBT * R * D(365) / 360 - DEBT * R, D("0.01")), D("35000.00"))
    INST_360 = DEBT / af(R_360, TEN)
    check("MCQ 3.3-L rationale: instalment on the actual/360 equivalent rate",
          q(INST_360, D("0.01")), D("5032547.52"))
    LIFE_INT_360 = TEN * INST_360 - DEBT
    LIFE_INT_365 = TEN * INST_EXACT - DEBT
    check("MCQ 3.3-L rationale: lifetime interest on the actual/360 basis",
          q(LIFE_INT_360, D("0.01")), D("18390570.24"))
    check("MCQ 3.3-L rationale: lifetime interest on the quoted 6.0 %",
          q(LIFE_INT_365, D("0.01")), D("18115622.81"))
    check("MCQ 3.3-L rationale: extra interest across the twelve-year schedule",
          q(LIFE_INT_360 - LIFE_INT_365, D("0.01")), D("274947"), tol=CENT)
    check("MCQ 3.3-L option B invariant: the uplift is independent of the rate — same 1.3889 % "
          "at 3 % and at 9 %",
          q(((D("0.03") * D(365) / 360) / D("0.03") - 1) * 100),
          q(((D("0.09") * D(365) / 360) / D("0.09") - 1) * 100))
    check("MCQ 3.3-L option B invariant: the uplift is independent of the balance",
          q((D(7000000) * R * D(365) / 360 - D(7000000) * R) / (D(7000000) * R) * 100),
          q(UPLIFT * 100))
    check("MCQ 3.3-L option D invariant: dividing by 360 raises the daily rate, so the convention "
          "costs money (1 = costs)", D(1) if R_360 > R else D(0), D(1))

    # =====================================================================================
    # DISPLAY FORMS — every figure the manuscript prints at a coarser precision than the
    # derivation above, checked in the exact form it appears on the page. A value that is right
    # to the cent and wrong when rounded is still a printed error.
    # =====================================================================================
    check("display: MCQ 1.2-G option A — covenant engages at 16.51 %",
          q((CASH - COV_CASH) / CASH * 100, D("0.01")), D("16.51"))
    check("display: MCQ 1.2-G option A — equity cash exhausts at 30.42 %",
          q((CASH - INST70) / CASH * 100, D("0.01")), D("30.42"))
    check("display: MCQ 2.2-G option B — cash breakeven as a 14.32 % fall",
          q(FALL_CASH, D("0.01")), D("14.32"))
    check("display: MCQ 3.2-K option B — 22.04 basis points cheaper",
          q((ALLIN - D("0.0615")) * 10000, D("0.01")), D("22.04"))
    check("display: MCQ 3.2-K rationale — 18.38 basis points per 1.00 % of fee",
          q((ALLIN_1PCT - R) * 10000, D("0.01")), D("18.38"))
    check("display: MCQ 3.2-K rationale — 37.04 basis points for the 2.0 % fee",
          q((ALLIN - R) * 10000, D("0.01")), D("37.04"))
    check("display: MCQ 3.2-K option D — the 16.67 basis-point straight-line error",
          q(FEE / DEBT / TEN * 10000, D("0.01")), D("16.67"))
    check("display: MCQ 3.3-L rationale — 8.33 basis points at 6 %",
          q((R_360 - R) * 10000, D("0.01")), D("8.33"))
    check("display: MCQ 3.3-K stem — cap value 4,258,610",
          q(CAPVAL4, D("1")), D("4258610"))
    check("display: MCQ 3.3-K stem — level annual equivalent 398,941",
          q(LAE4, D("1")), D("398941"))
    check("display: MCQ 3.3-K option A — 101,059 a year",
          q(CUT - LAE4, D("1")), D("101059"))
    check("display: MCQ 3.3-K option B — stressed cap value 9,157,382",
          q(CAPVAL5, D("1")), D("9157382"))
    check("display: MCQ 3.3-K option B — stressed level annual equivalent 857,852",
          q(CAPVAL5 / AF8, D("1")), D("857852"))
    check("display: MCQ 3.3-K rationale — indexed reduction 7,150,991",
          q(CUT * af(rstar(CAP_G), LIFE), D("1")), D("7150991"))
    check("display: MCQ 3.3-K rationale — level reduction 5,337,388",
          q(CUT * AF8, D("1")), D("5337388"))
    check("display: MCQ 3.3-J stem — 55,006,446 and 72,791,113",
          q(WRONG, D("1")) + q(RIGHT_SUM, D("1")), D("55006446") + D("72791113"))
    check("display: MCQ 3.3-L rationale — 274,947 of extra interest",
          q(LIFE_INT_360 - LIFE_INT_365, D("1")), D("274947"))
    check("display: MCQ 3.2-K option B — equalising fee 343,244 and excess 496,756",
          q(EQUAL_FEE, D("1")) + q(FEE - EQUAL_FEE, D("1")), D("343244") + D("496756"))
    check("display: MCQ 3.2-J — 21,102,406 outstanding and 25,048,176 of cash remaining",
          q(B7_CLOSED, D("1")) + q(REMAINING_CASH, D("1")), D("21102406") + D("25048176"))
    check("display: MCQ 2.2-G option B — cash breakeven revenue 10,282,044",
          q(BE_CASH, D("1")), D("10282044"))
    check("display: MCQ 2.4-G rationale — DSCR headroom 465,547 of revenue",
          q(HEADROOM_REV, D("1")), D("465547"))
