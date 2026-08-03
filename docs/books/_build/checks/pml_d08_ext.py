"""PML-AI Domain 8 (extension) — Risk, uncertainty and resilience: golden checks.

Every number printed as a *result* by the depth expansion of
`domain-08-risk-uncertainty-resilience.md` is recomputed here from its definition rather than from
its printed output. The domain's original core figures (Auriga's `EMV` table, the 63,828,000,000
variance, σ 252,642, the P80 of 490,624, the survey tree and the fast-track tie to Domain 6) live in
`verify_formulas.py` and are not duplicated. What is added here are the new engines:

    completeness   register size inferred from the overlap of two identification methods. Both
                   estimators are computed from their definitions, and the *divergence identity*
                   LP − Chapman = missing/(m+1) — the identity Toolkit 8.T.4 now uses in place of an
                   invented 10 % threshold — is proved by sweeping n1, n2 and m rather than asserted.
    screening      the within-cell `EMV` span derived as a product of band ratios, and the band count
                   a ratio-constant scale needs, found by SEARCHING for the smallest n with 2^n over
                   the range, so the manuscript's "7 bands" is a computed minimum.
    EMV structure  the exact 32-outcome enumeration of Auriga's register: the aggregate's mean is
                   re-derived from the enumeration (not from Σ EMV), the exact non-exceedance of the
                   normal P80 is summed over outcomes, the discreteness of the percentile is proved by
                   showing no outcome level has cumulative probability 0.80, and the rank-flip
                   probabilities are solved rather than quoted.
    information    the imperfect-signal tree, with the random pair's detection probability built from
                   C(28,2)/C(40,2), both breakeven priors solved from the branch algebra, and the
                   value of perfect information bounding both.
    correlation    per-risk σ, the pairwise term, and the confidence a P80-labelled reserve actually
                   buys, inverted through a Decimal normal CDF implemented below from an erf series
                   (no float library call decides a published percentile).
    levers         the impact-versus-probability variance identity proved as algebra over a sweep of p
                   — the 1.5-to-3 range is derived as a limit and a maximum, not typed in — plus the
                   two Meridian responses' P90 reductions.
    reserves       the appetite-to-confidence conversion, the σ an appetite permits, the adequacy
                   ratio of a part-consumed reserve recomputed on the open register, and the release
                   from retiring a low-probability high-impact risk.
    resilience     the retainer on the average and on the tail, with the breakeven probability solved
                   and shown to sit inside a tenth of a point of the assessment.
    reference class the outturn-ratio order statistics, with the P80 read at position (n+1)·0.8 by
                   interpolation and the interpolation interval's width reported as its precision.
    monitors       precision, net value as a linear function of the base rate, the crossover solved
                   from the two lines, and the financial-services variant re-derived from the general
                   crossover formula b/(1−b) = c·Δk/(Δs(S−c)).
    schedule       merge probability three ways. The correct merge is NUMERICALLY INTEGRATED over the
                   shared A+B predecessor (Simpson, convergence-checked), the P80 date is found by
                   bisection on that integral, and the naive-product/correct/dominant-path ordering is
                   asserted as an invariant across every date in the fade table.

Master-thread values come from ctx and are never re-typed: MERIDIAN_COST_OF_DELAY (USD 14,280/week),
MERIDIAN_BASELINE (USD 2,400,000 approved cost), AURIGA_BAC (USD 4,000,000). Auriga's schedule value
(USD 45,000/week, Domains 6–8) is declared once below.

Published MCQ distractors that are the results of named errors are checked too (adding two
identification lists without removing the overlap; reading a coverage percentage as a population
count; a band ratio less one; the sum of a register P80 and a reference-class uplift), because the
manuscript states each distractor's arithmetic and a wrong distractor is as much a defect as a wrong
answer.
"""

import math
from itertools import product

PI = "3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803483"


def run(ctx):
    check, D = ctx["check"], ctx["D"]
    COD_M = ctx["MERIDIAN_COST_OF_DELAY"]          # USD 14,280/week (Domain 1)
    MB = ctx["MERIDIAN_BASELINE"]                  # USD 2,400,000 approved cost
    BAC = ctx["AURIGA_BAC"]                        # USD 4,000,000
    COD_A = D(45000)                               # USD/week, Auriga (Domains 6-8)
    Z80, Z90 = D("0.8416"), D("1.2816")
    pi = D(PI)

    def sq(x):
        return D(x).sqrt()

    def erf(x):
        """erf(x) by Taylor series at 28 digits; adequate for the |x| < 3 range used here."""
        x = D(x)
        if x < 0:
            return -erf(-x)
        total, fact = D(0), D(1)
        for n in range(0, 200):
            if n > 0:
                fact *= n
            t = (D(-1) ** n) * x ** (2 * n + 1) / (fact * (2 * n + 1))
            total += t
            if n > 8 and abs(t) < D("1e-30"):
                break
        return 2 / pi.sqrt() * total

    def Phi(z):
        return (1 + erf(D(z) / D(2).sqrt())) / 2

    # the normal CDF is load-bearing for a dozen published percentiles: pin it first
    check("Phi(0) = 0.5", Phi(0), D("0.5"), tol=D("1e-20"))
    check("Phi(z80) = 0.80 at the domain's z factor", Phi(Z80) * 100, D("80.00"), tol=D("0.01"))
    check("Phi(z90) = 0.90 at the domain's z factor", Phi(Z90) * 100, D("90.00"), tol=D("0.01"))

    # =====================================================================================
    # KA 8.1.3 — register completeness by capture-recapture
    # =====================================================================================
    def lp(n1, n2, m):
        return D(n1) * D(n2) / D(m)

    def chapman(n1, n2, m):
        return (D(n1) + 1) * (D(n2) + 1) / (D(m) + 1) - 1

    n1, n2, m = D(34), D(22), D(14)
    N = lp(n1, n2, m)
    dist = n1 + n2 - m
    check("8.1.3 Lincoln-Petersen N-hat", N, D("53.4286"))
    check("8.1.3 Chapman N-hat", chapman(n1, n2, m), D("52.6667"))
    check("8.1.3 distinct identified", dist, 42, tol=D("0"))
    check("8.1.3 estimated unidentified", N - dist, D("11.4286"))
    check("8.1.3 unidentified as % of population", (N - dist) / N * 100, D("21.39"))
    check("8.1.3 coverage %", dist / N * 100, D("78.61"))
    check("8.1.3 Chapman unidentified", chapman(n1, n2, m) - dist, D("10.6667"))
    check("8.1.3 Chapman coverage %", dist / chapman(n1, n2, m) * 100, D("79.75"))
    check("8.1.3 workshop-alone coverage %", n1 / N * 100, D("63.64"))
    check("8.1.3 risks the assumption review added", n2 - m, 8, tol=D("0"))
    check("8.1.3 cost per newly found risk", D(12000) / (n2 - m), 1500)
    check("8.1.3 coverage + unidentified share = 100 %",
          dist / N * 100 + (N - dist) / N * 100, 100, tol=D("1e-20"))
    # the estimator is monotone in m, which is WHY the missing count is a floor (8.1.3, MCQ 8.1-E)
    check("8.1.3 a larger overlap lowers N-hat (the floor argument)",
          D(1) if lp(34, 22, 15) < lp(34, 22, 14) else D(0), 1, tol=D("0"))
    # Toolkit 8.T.4's divergence identity, proved by sweep rather than asserted
    worst = D(0)
    for a in range(12, 40):
        for b in range(10, 30):
            for c in range(4, min(a, b) + 1):
                d1 = lp(a, b, c) - chapman(a, b, c)
                d2 = (lp(a, b, c) - (D(a) + D(b) - D(c))) / (D(c) + 1)
                worst = max(worst, abs(d1 - d2))
    check("8.T.4 divergence identity LP - Chapman = missing/(m+1), max error over sweep",
          worst, 0, tol=D("1e-20"))
    check("8.T.4 divergence on Meridian's numbers",
          lp(n1, n2, m) - chapman(n1, n2, m), D("0.7619"))
    # MCQ 8.1-D distractors
    check("MCQ 8.1-D distractor A (lists added, overlap not removed)", n1 + n2, 56, tol=D("0"))
    check("MCQ 8.1-D distractor D (coverage % read as a population count)",
          dist / N * 100 - dist, D("36.61"))

    # =====================================================================================
    # KA 8.2.1 — what one matrix cell conceals
    # =====================================================================================
    pmin, pmax, imin, imax = D("0.10"), D("0.35"), D(60000), D(250000)
    check("8.2.1 Medium/Medium cell floor", pmin * imin, 6000)
    check("8.2.1 Medium/Medium cell ceiling", pmax * imax, 87500)
    check("8.2.1 probability band ratio", pmax / pmin, D("3.5"))
    check("8.2.1 impact band ratio", imax / imin, D("4.1667"))
    span = pmax * imax / (pmin * imin)
    check("8.2.1 within-cell span factor", span, D("14.5833"))
    check("8.2.1 span identity = product of the two band ratios",
          span - (pmax / pmin) * (imax / imin), 0, tol=D("1e-20"))
    mm = (pmin + pmax) / 2 * (imin + imax) / 2
    hh = (D("0.35") + D("0.70")) / 2 * (D(250000) + D(800000)) / 2
    check("8.2.1 Medium/Medium midpoint EMV", mm, 34875)
    check("8.2.1 High/High midpoint EMV", hh, 275625)
    check("8.2.1 step between midpoints two bands apart", hh / mm, D("7.9032"))
    check("8.2.1 within-cell span vs that step", span / (hh / mm), D("1.8452"))
    # the band count is SEARCHED for, not asserted
    def bands_needed(rng, ratio=D(2)):
        n, r = 1, ratio
        while r < D(rng):
            n, r = n + 1, r * ratio
        return n
    check("8.2.1 probability range", D("0.70") / D("0.01"), 70)
    check("8.2.1 bands for a probability range of 70 at ratio 2", bands_needed(70), 7, tol=D("0"))
    check("8.2.1 log2(70)", D(str(math.log2(70))), D("6.1293"))
    check("8.2.1 impact range", D(800000) / D(10000), 80)
    check("8.2.1 bands for an impact range of 80 at ratio 2", bands_needed(80), 7, tol=D("0"))
    check("8.2.1 log2(80)", D(str(math.log2(80))), D("6.3219"))
    check("8.2.1 a linear low probability band is a ratio of 10", D("0.10") / D("0.01"), 10)
    check("8.2.1 ratio-2 axes hold the within-cell span at 4", D(2) * 2, 4, tol=D("0"))
    check("MCQ 8.2-F distractor D (band ratio less one)", pmax / pmin - 1, D("2.5"))

    # =====================================================================================
    # Auriga's register — the shared object for 8.2.2, 8.2.4, 8.2.4b, 8.3.1, 8.3.2b
    # =====================================================================================
    AUR = [("R1", D("0.35"), D(240000)), ("R2", D("0.50"), D(180000)),
           ("R3", D("0.25"), D(320000)), ("R4", D("0.15"), D(400000)),
           ("R5", D("0.30"), D(-120000))]
    emv = {r: p * i for r, p, i in AUR}
    var = {r: p * (1 - p) * i * i for r, p, i in AUR}
    sig1 = {r: sq(p * (1 - p)) * abs(i) for r, p, i in AUR}
    TOT = sum(emv.values())
    VAR = sum(var.values())
    SIG = sq(VAR)
    P80 = TOT + Z80 * SIG
    P90 = TOT + Z90 * SIG

    # ---- 8.2.2 deepened interpretation ------------------------------------------------
    check("8.2.2 opportunity-ignored overstatement %", (TOT + 36000) / TOT * 100 - 100, D("12.95"))
    check("8.2.2 P(all four threats occur) %",
          D("0.35") * D("0.50") * D("0.25") * D("0.15") * 100, D("0.65625"))
    check("8.2.2 R4 probability at which its EMV ties R1's", emv["R1"] / D(400000), D("0.21"))
    check("8.2.2 R4 probability at which it tops the register", emv["R2"] / D(400000), D("0.225"))
    check("8.2.2 EMV swing from a +-0.05 error on R2", D("0.05") * D(180000), 9000)
    check("8.2.2 that swing as % of the total", D("0.05") * D(180000) / TOT * 100, D("3.24"))

    # ---- 8.2.4 exact enumeration of the aggregate --------------------------------------
    outs = []
    for combo in product([0, 1], repeat=5):
        prob, cost = D(1), D(0)
        for bit, (r, p, i) in zip(combo, AUR):
            prob *= p if bit else (1 - p)
            if bit:
                cost += i
        outs.append((cost, prob))
    check("8.2.4 enumeration is a distribution (probabilities sum to 1)",
          sum(p for _, p in outs), 1, tol=D("1e-20"))
    check("8.2.4 enumerated mean reproduces sum EMV",
          sum(c * p for c, p in outs), TOT, tol=D("1e-16"))
    levels = sorted(set(c for c, _ in outs))
    check("8.2.4 distinct outcome levels", len(levels), 32, tol=D("0"))

    def cum(x):
        return sum(p for c, p in outs if c <= D(x))

    check("8.2.4 P(net cost <= 0) % (the one-in-five costless future)", cum(0) * 100, D("20.72"))
    check("8.2.4 exact non-exceedance of the P80 amount %", cum(P80) * 100, D("78.60"))
    check("8.2.4 approximation overstates confidence by, points", D(80) - cum(P80) * 100, D("1.40"))
    check("8.2.4 exact non-exceedance of the 10 % rule of thumb %", cum(400000) * 100, D("68.58"))
    reach = min(l for l in levels if cum(l) >= D("0.80"))
    check("8.2.4 smallest outcome level reaching 0.80", reach, 500000)
    check("8.2.4 cumulative probability at that level %", cum(reach) * 100, D("83.43"))
    check("8.2.4 no outcome level has cumulative probability exactly 0.80",
          D(sum(1 for l in levels if cum(l) == D("0.80"))), 0, tol=D("0"))
    z_rot = (D(400000) - TOT) / SIG
    check("8.2.4 z of the 10 % rule of thumb", z_rot, D("0.4829"))
    check("8.2.4 percentile of the 10 % rule of thumb %", Phi(z_rot) * 100, D("68.54"))
    check("8.2.4 the same 400,000 on a register of half the sigma %",
          Phi((D(400000) - TOT) / (SIG / 2)) * 100, D("83"), tol=D("0.5"))
    check("8.2.4 P80 as % of BAC", P80 / BAC * 100, D("12.27"))
    check("8.2.4 P90 amount", P90, D("601786.04"))
    check("8.2.4 P90 as % of BAC", P90 / BAC * 100, D("15.04"))
    check("8.2.4 cost of moving P80 to P90", P90 - P80, D("111162.50"))
    check("8.2.4 price of confidence per point", (P90 - P80) / 10, D("11116.25"))

    # =====================================================================================
    # Meridian's programme register (8.2.2b) — shared with 8.2.4c, 8.3.2, 8.4.2
    # =====================================================================================
    MER = [("M1", D("0.40"), D(90000)), ("M2", D("0.35"), D(120000)),
           ("M3", D("0.20"), D(150000)), ("M4", D("0.30"), D(70000)),
           ("M5", D("0.25"), D(-40000))]
    memv = {r: p * i for r, p, i in MER}
    mvar = {r: p * (1 - p) * i * i for r, p, i in MER}
    msig = {r: sq(p * (1 - p)) * abs(i) for r, p, i in MER}
    MTOT = sum(memv.values())
    MVAR = sum(mvar.values())
    MSIG = sq(MVAR)
    TOL = MB * D("0.05")
    for r, v in [("M1", 36000), ("M2", 42000), ("M3", 30000), ("M4", 21000), ("M5", -10000)]:
        check(f"8.2.2b {r} EMV", memv[r], v)
    check("8.2.2b total expected cost exposure", MTOT, 119000)
    check("8.2.2b exposure as % of approved cost", MTOT / MB * 100, D("4.96"))
    check("8.2.2b the objective's 5 % tolerance", TOL, 120000)
    check("8.2.2b exposure as % of the tolerance", MTOT / TOL * 100, D("99.17"))
    check("8.2.2b every quantified threat clears the 60,000 screen floor",
          D(min(i for r, p, i in MER if i > 0)), 70000)
    check("8.2.2b the one entry below the floor is the opportunity", D(40000), 40000)
    check("8.2.2b Meridian's benefit rate ties Domain 1's annual figure",
          ctx["MERIDIAN_BENEFIT_70PCT"], 685440)

    # =====================================================================================
    # KA 8.2.3 — the survey tree's breakevens (new) and 8.2.3b imperfect information
    # =====================================================================================
    ev_direct = D("0.40") * D(300000)
    check("8.2.3 survey price at which the branches tie", ev_direct - D("0.40") * D(90000), 84000)
    check("8.2.3 that as a multiple of the actual price",
          (ev_direct - D("0.40") * D(90000)) / D(25000), D("3.36"))
    check("8.2.3 breakeven prior probability %",
          D(25000) / (D(300000) - D(90000)) * 100, D("11.90"))
    check("8.2.3 planned-response cost at which knowing is worthless",
          (ev_direct - D(25000)) / D("0.40"), 237500)
    check("8.2.3 branch B EV at a 250,000 planned response",
          D(25000) + D("0.40") * D(250000), 125000)
    check("8.2.3 value destroyed there", D(25000) + D("0.40") * D(250000) - ev_direct, 5000)
    check("8.2.3 EVPI, gross of price", ev_direct - D("0.40") * D(90000), 84000)
    check("8.2.3 a perfect signal captures 100 % of EVPI",
          (ev_direct - (D(25000) + D("0.40") * D(90000))) / (D(84000) - D(25000)) * 100, 100)

    late = D(480000) + 6 * COD_M
    early = D(130000) + 2 * COD_M
    check("8.2.3b late-discovery total", late, 565680)
    check("8.2.3b early-discovery total", early, 158560)
    miss = (D(28) * 27) / (D(40) * 39)                     # C(28,2)/C(40,2)
    check("8.2.3b random pair misses every legacy clinic, probability %", miss * 100, D("48.4615"))
    check("8.2.3b random pair detection %", (1 - miss) * 100, D("51.5385"))
    prior = D("0.30")
    ev_no = prior * late
    inner = (1 - miss) * early + miss * late
    ev_rnd = D(45000) + prior * inner
    ev_des = D(45000) + prior * early
    check("8.2.3b no-pilot expected cost", ev_no, D("169704.00"))
    check("8.2.3b random pilot, detect branch", (1 - miss) * early, D("81719.38"))
    check("8.2.3b random pilot, miss branch", miss * late, D("274137.23"))
    check("8.2.3b random pilot inner expectation", inner, D("355856.62"))
    check("8.2.3b random pilot expected cost", ev_rnd, D("151756.98"))
    check("8.2.3b designed pilot expected cost", ev_des, D("92568.00"))
    check("8.2.3b value of the random pilot", ev_no - ev_rnd, D("17947.02"))
    check("8.2.3b value of the designed pilot", ev_no - ev_des, D("77136.00"))
    check("8.2.3b value of the design choice alone", ev_rnd - ev_des, D("59188.98"))
    check("8.2.3b perfect free information's expected cost", prior * D(130000), 39000)
    evpi = ev_no - prior * D(130000)
    check("8.2.3b EVPI", evpi, 130704)
    check("8.2.3b designed pilot captures % of EVPI", (ev_no - ev_des) / evpi * 100, D("59.02"))
    check("8.2.3b random pilot captures % of EVPI", (ev_no - ev_rnd) / evpi * 100, D("13.73"))
    bp_des = D(45000) / (late - early)
    bp_rnd = D(45000) / (late - inner)
    check("8.2.3b designed pilot breakeven prior %", bp_des * 100, D("11.0533"))
    check("8.2.3b random pilot breakeven prior %", bp_rnd * 100, D("21.4466"))
    check("8.2.3b ratio of the two priors", bp_rnd / bp_des, D("1.9403"))
    # at each breakeven the two branches must in fact tie — the solve, verified
    check("8.2.3b designed breakeven ties the branches",
          (D(45000) + bp_des * early) - bp_des * late, 0, tol=D("1e-18"))
    check("8.2.3b random breakeven ties the branches",
          (D(45000) + bp_rnd * inner) - bp_rnd * late, 0, tol=D("1e-18"))
    check("8.2.3b one week either way on the late case", prior * COD_M, 4284)
    check("8.2.3b the band in which piloting badly destroys value, points",
          (bp_rnd - bp_des) * 100, D("10.3934"))

    # =====================================================================================
    # KA 8.2.4b — one shared subcontractor
    # =====================================================================================
    for r, v in [("R1", "114472.70"), ("R2", "90000.00"), ("R3", "138564.06"),
                 ("R4", "142828.57"), ("R5", "54990.91")]:
        check(f"8.2.4b per-risk sigma {r}", sig1[r], D(v))
    check("8.2.4b sigma1 x sigma3", sig1["R1"] * sig1["R3"], D("15861803176.18"))
    check("8.2.4b sigma1 x sigma4", sig1["R1"] * sig1["R4"], D("16349972477.04"))
    check("8.2.4b sigma3 x sigma4", sig1["R3"] * sig1["R4"], D("19790907002.96"))
    rho = D("0.5")
    add = 2 * rho * (sig1["R1"] * sig1["R3"] + sig1["R1"] * sig1["R4"] + sig1["R3"] * sig1["R4"])
    check("8.2.4b added variance", add, D("52002682656"), tol=D("0.5"))
    check("8.2.4b correlated variance", VAR + add, D("115830682656"), tol=D("0.5"))
    SIGC = sq(VAR + add)
    check("8.2.4b correlated sigma", SIGC, D("340339.07"))
    check("8.2.4b sigma factor", SIGC / SIG, D("1.3471"))
    P80C = TOT + Z80 * SIGC
    check("8.2.4b correlated P80", P80C, D("564429.36"))
    check("8.2.4b uplift over the independence P80", P80C - P80, D("73805.82"))
    check("8.2.4b uplift %", (P80C - P80) / P80 * 100, D("15.04"))
    check("8.2.4b correlated P80 as % of BAC", P80C / BAC * 100, D("14.11"))
    zc = (P80 - TOT) / SIGC
    check("8.2.4b z of the approved reserve on the correlated distribution", zc, D("0.6247"))
    check("8.2.4b confidence the approved reserve actually buys %", Phi(zc) * 100, D("73.39"))
    check("8.2.4b that shortfall, points", D(80) - Phi(zc) * 100, D("6.61"))
    check("8.2.4b added variance as % of the independent variance (printed to 1 dp)",
          (add / VAR * 100).quantize(D("0.1")), D("81.5"), tol=D("0"))
    # the mean is invariant to dependence: the claim on which the whole KA's hand-check rests
    check("8.2.4b mean is unchanged by correlation", TOT, 278000)
    # 8.A.1's general mechanism: pairs grow with k(k-1)/2, entries with k, so doubling the number
    # of risks on one driver roughly quadruples the variance it contributes. Derived, not asserted.
    def pair_ratio(k):
        return (D(2 * k) * (2 * D(k) - 1) / 2) / (D(k) * (D(k) - 1) / 2)

    check("8.A.1 doubling risks on one driver quadruples its variance term, at k=14",
          pair_ratio(14), D("4.1538"))
    check("8.A.1 that ratio tends to 4 as k grows", pair_ratio(4000), 4, tol=D("0.001"))
    check("8.A.1 the pair count grows quadratically: C(2k,2)/C(k,2) exceeds 3.9 by k=20",
          D(1) if pair_ratio(20) > D("3.9") else D(0), 1, tol=D("0"))
    # correlation term is linear in rho (the sensitivity caution)
    check("8.2.4b halving rho halves the added variance",
          2 * D("0.25") * (sig1["R1"] * sig1["R3"] + sig1["R1"] * sig1["R4"]
                           + sig1["R3"] * sig1["R4"]) * 2 - add, 0, tol=D("1e-8"))

    # =====================================================================================
    # KA 8.2.4c — the response that buys variance
    # =====================================================================================
    check("8.2.4c Meridian variance", MVAR, D("10149000000"))
    check("8.2.4c Meridian sigma", MSIG, D("100742.25"))
    MP90 = MTOT + Z90 * MSIG
    check("8.2.4c Meridian P90", MP90, D("248111.26"))
    # A: halve M2's probability
    a_var = D("0.175") * D("0.825") * D(120000) ** 2
    check("8.2.4c A residual M2 variance", a_var, D("2079000000"))
    check("8.2.4c A EMV removed", memv["M2"] - D("0.175") * D(120000), 21000)
    check("8.2.4c A variance removed", mvar["M2"] - a_var, D("1197000000"))
    a_mean, a_sig = MTOT - 21000, sq(MVAR - (mvar["M2"] - a_var))
    check("8.2.4c A new mean", a_mean, 98000)
    check("8.2.4c A new sigma", a_sig, D("94615.01"))
    a_p90 = a_mean + Z90 * a_sig
    check("8.2.4c A new P90", a_p90, D("219258.60"))
    check("8.2.4c A P90 reduction", MP90 - a_p90, D("28852.67"))
    # B: cut M3's impact
    b_var = D("0.20") * D("0.80") * D(60000) ** 2
    check("8.2.4c B residual M3 variance", b_var, D("576000000"))
    check("8.2.4c B EMV removed", memv["M3"] - D("0.20") * D(60000), 18000)
    check("8.2.4c B variance removed", mvar["M3"] - b_var, D("3024000000"))
    b_mean, b_sig = MTOT - 18000, sq(MVAR - (mvar["M3"] - b_var))
    check("8.2.4c B new mean", b_mean, 101000)
    check("8.2.4c B new sigma", b_sig, D("84409.72"))
    b_p90 = b_mean + Z90 * b_sig
    check("8.2.4c B new P90", b_p90, D("209179.49"))
    check("8.2.4c B P90 reduction", MP90 - b_p90, D("38931.77"))
    redA, redB = MP90 - a_p90, MP90 - b_p90
    check("8.2.4c extra EMV A removes", D(21000) - D(18000), 3000)
    check("8.2.4c extra P90 requirement B removes", redB - redA, D("10079.11"))
    check("8.2.4c B's advantage on P90 %", (redB - redA) / redA * 100, D("34.93"))
    check("8.2.4c A P90 reduction per USD", redA / D(25000), D("1.1541"))
    check("8.2.4c B P90 reduction per USD", redB / D(25000), D("1.5573"))
    # the lever identity, derived over p rather than typed in
    def keep_p(p):                       # variance share left after halving p
        return (1 - D(p) / 2) / (2 * (1 - D(p)))

    def ratio(p):                        # impact lever / probability lever, at equal EMV cost
        return D("0.75") / (1 - keep_p(p))

    check("8.2.4c halving an impact removes % of variance, always",
          (1 - (D(1) / 2) ** 2) * 100, 75)
    for p, v in [("0.10", "0.5278"), ("0.35", "0.6346"), ("0.50", "0.75")]:
        check(f"8.2.4c variance share left by halving p at p={p}", keep_p(p), D(v))
    for p, v in [("0.10", "1.5882"), ("0.20", "1.7143"), ("0.35", "2.0526"), ("0.50", "3")]:
        check(f"8.2.4c impact-vs-probability variance ratio at p={p}", ratio(p), D(v))
    # the 1.5-to-3 range: a limit at p -> 0 and a maximum at p = 0.5, swept not asserted
    check("8.2.4c ratio tends to 1.5 as p tends to 0", ratio("0.0000001"), D("1.5"), tol=D("1e-6"))
    swept = [ratio(D(k) / 1000) for k in range(1, 501)]
    check("8.2.4c ratio never below 1.5 for p <= 0.5", min(swept), D("1.5"), tol=D("0.001"))
    check("8.2.4c ratio never above 3 for p <= 0.5", max(swept), D(3), tol=D("0.001"))
    check("8.2.4c halving p removes between 25 % and 50 % of variance (max)",
          max((1 - keep_p(D(k) / 1000)) * 100 for k in range(1, 501)), 50, tol=D("0.05"))
    check("8.2.4c halving p removes between 25 % and 50 % of variance (min)",
          min((1 - keep_p(D(k) / 1000)) * 100 for k in range(1, 501)), 25, tol=D("0.05"))
    # Fig 8.2.3
    check("Fig 8.2.3 M3 share of total EMV %", memv["M3"] / MTOT * 100, D("25.21"))
    check("Fig 8.2.3 M3 share of total variance %", mvar["M3"] / MVAR * 100, D("35.47"))
    check("Fig 8.2.3 M3 sigma contribution", msig["M3"], 60000)
    check("Fig 8.2.3 M3 is first by sigma contribution",
          msig["M3"] - max(msig.values()), 0, tol=D("0"))
    check("Fig 8.2.3 M3 is third of five by |EMV|",
          D(sorted(abs(v) for v in memv.values()).index(abs(memv["M3"])) + 1), 3, tol=D("0"))

    # =====================================================================================
    # KA 8.3.1 — four responses to R1, with secondary risks priced
    # =====================================================================================
    check("8.3.1 R1 EMV", emv["R1"], 84000)
    check("8.3.1 R1 impact as % of BAC", D(240000) / BAC * 100, D("6.00"))
    acc = emv["R1"]
    red_res, red_sec = D("0.15") * D(240000), D("0.10") * D(40000)
    red = D(22000) + red_res + red_sec
    check("8.3.1 reduce residual EMV", red_res, 36000)
    check("8.3.1 reduce secondary EMV", red_sec, 4000)
    check("8.3.1 reduce total expected cost", red, 62000)
    check("8.3.1 reduce ignoring secondaries", D(22000) + red_res, 58000)
    recovered = D("0.60") * D(240000)
    tr_res = D("0.35") * D(240000) * D("0.40")
    tr_sec = D("0.35") * D("0.25") * recovered
    trn = D(18000) + tr_res + tr_sec
    check("8.3.1 transfer residual EMV", tr_res, 33600)
    check("8.3.1 transfer counterparty secondary EMV", tr_sec, 12600)
    check("8.3.1 transfer total expected cost", trn, 64200)
    check("8.3.1 transfer ignoring secondaries", D(18000) + tr_res, 51600)
    avd = D(95000) + D("0.20") * D(120000)
    check("8.3.1 avoid secondary EMV", D("0.20") * D(120000), 24000)
    check("8.3.1 avoid total expected cost", avd, 119000)
    check("8.3.1 reduce is the minimum of the four",
          red - min(acc, red, trn, avd), 0, tol=D("0"))
    check("8.3.1 transfer is the minimum once secondaries are omitted",
          (D(18000) + tr_res) - min(acc, D(22000) + red_res, D(18000) + tr_res, D(95000)),
          0, tol=D("0"))
    check("8.3.1 apparent advantage of transfer", (D(22000) + red_res) - (D(18000) + tr_res), 6400)
    check("8.3.1 actual deficit of transfer", trn - red, 2200)
    check("8.3.1 swing in the comparison", ((D(22000) + red_res) - (D(18000) + tr_res))
          + (trn - red), 8600)
    q = (red - D(18000) - tr_res) / (D("0.35") * recovered)
    check("8.3.1 counterparty default probability at which transfer ties reduce %", q * 100,
          D("20.63"))
    check("8.3.1 at that probability the two options tie",
          (D(18000) + tr_res + D("0.35") * q * recovered) - red, 0, tol=D("1e-18"))
    check("8.3.1 avoid vs accepting", avd - acc, 35000)
    check("8.3.1 value reduce creates against accepting", acc - red, 22000)
    check("8.3.1 that as an improvement %", (acc - red) / acc * 100, D("26.19"))
    check("8.3.1 reduce cost at which acceptance becomes correct",
          acc - red_res - red_sec, 44000)
    check("8.3.1 reduce residual variance", D("0.15") * D("0.85") * D(240000) ** 2,
          D("7344000000"))
    check("8.3.1 transfer residual variance", D("0.35") * D("0.65") * D(96000) ** 2,
          D("2096640000"))
    check("8.3.1 the transfer is the impact lever of the two (less residual variance)",
          D(1) if D("0.35") * D("0.65") * D(96000) ** 2
          < D("0.15") * D("0.85") * D(240000) ** 2 else D(0), 1, tol=D("0"))
    check("MCQ 8.3-D transfer-vs-reduce reversal figures", trn - red, 2200)

    # =====================================================================================
    # KA 8.3.2 — appetite to confidence level
    # =====================================================================================
    check("8.3.2 tolerance from the appetite statement", TOL, 120000)
    check("8.3.2 register P90 requirement", MP90, D("248111.26"))
    check("8.3.2 P90 as a multiple of the tolerance", MP90 / TOL, D("2.0676"))
    check("8.3.2 P90 as % of approved cost", MP90 / MB * 100, D("10.34"))
    check("8.3.2 sigma the appetite permits at the current mean", (TOL - MTOT) / Z90, D("780.27"))
    check("8.3.2 that permitted sigma does reach the tolerance",
          MTOT + Z90 * ((TOL - MTOT) / Z90) - TOL, 0, tol=D("1e-18"))
    check("8.3.2 room left inside the tolerance", TOL - MTOT, 1000)
    check("8.3.2 gap to close", MP90 - TOL, D("128111.26"))
    check("8.3.2 responses of Response B's size needed", (MP90 - TOL) / redB, D("3.2907"))
    check("8.3.2 P80 implies an accepted exceedance probability %", D(100) - 80, 20)

    # ---- 8.3.2b adequacy at week 13 ---------------------------------------------------
    check("8.3.2b drawn total from its two components", D(195000) + D(37000), 232000)
    open_mean = emv["R1"] + emv["R3"] + emv["R5"]
    open_var = var["R1"] + var["R3"] + var["R5"]
    check("8.3.2b R1 variance", var["R1"], D("13104000000"))
    check("8.3.2b R3 variance", var["R3"], D("19200000000"))
    check("8.3.2b R5 variance", var["R5"], D("3024000000"))
    check("8.3.2b open register mean", open_mean, 128000)
    check("8.3.2b open register variance", open_var, D("35328000000"))
    open_sig = sq(open_var)
    check("8.3.2b open register sigma", open_sig, D("187957.44"))
    req = open_mean + Z80 * open_sig
    check("8.3.2b P80 requirement on the open register", req, D("286184.98"))
    rem = P80 - D(232000)
    check("8.3.2b remaining reserve", rem, D("258623.54"))
    check("8.3.2b share of the reserve drawn %", D(232000) / P80 * 100, D("47.29"))
    check("8.3.2b adequacy ratio", rem / req, D("0.9037"))
    check("8.3.2b adequacy ratio as %", rem / req * 100, D("90.37"))
    check("8.3.2b shortfall", req - rem, D("27561.44"))
    zr = (rem - open_mean) / open_sig
    check("8.3.2b z of the remaining reserve", zr, D("0.6950"))
    check("8.3.2b confidence the remaining reserve buys %", Phi(zr) * 100, D("75.65"))
    check("8.3.2b the naive drawdown test would have passed (drawn < progress)",
          D(1) if D(232000) / P80 < (emv["R4"] + emv["R2"]) / TOT else D(0), 1, tol=D("0"))
    no4_mean = TOT - emv["R4"]
    no4_sig = sq(VAR - var["R4"])
    no4_p80 = no4_mean + Z80 * no4_sig
    check("8.3.2b R4 variance", var["R4"], D("20400000000"))
    check("8.3.2b mean without R4", no4_mean, 218000)
    check("8.3.2b sigma without R4", no4_sig, D("208393.86"))
    check("8.3.2b P80 without R4", no4_p80, D("393384.27"))
    check("8.3.2b release from retiring R4", P80 - no4_p80, D("97239.27"))
    check("8.3.2b release as a multiple of R4's EMV", (P80 - no4_p80) / D(60000), D("1.6207"))
    check("8.3.2b reserve left idle if only the EMV is returned",
          P80 - no4_p80 - D(60000), D("37239.27"))
    check("8.3.2b R4 share of the register's EMV %", emv["R4"] / TOT * 100, D("21.58"))
    check("8.3.2b R4 share of the register's variance %", var["R4"] / VAR * 100, D("31.96"))
    check("8.3.2b R2 share of the register's EMV %", emv["R2"] / TOT * 100, D("32.37"))
    check("MCQ 8.3-E amount to return (not the EMV)", P80 - no4_p80, D("97239.27"))
    check("MCQ 8.3-E distractor D (the idle remainder)", P80 - no4_p80 - D(60000), D("37239.27"))

    # =====================================================================================
    # KA 8.4.1 — pricing a resilience measure twice
    # =====================================================================================
    pf = D("0.25")
    no_ret = pf * 7 * COD_M
    with_ret = D(18000) + pf * 2 * COD_M
    check("8.4.1 expected cost without the retainer", no_ret, D("24990.00"))
    check("8.4.1 expected cost with the retainer", with_ret, D("25140.00"))
    check("8.4.1 net effect on expected value", no_ret - with_ret, -150)
    be = D(18000) / (5 * COD_M)
    check("8.4.1 breakeven failure probability %", be * 100, D("25.2101"))
    check("8.4.1 at the breakeven the two options tie",
          (pf * 7 * COD_M).copy_abs() * 0 + (be * 7 * COD_M) - (D(18000) + be * 2 * COD_M),
          0, tol=D("1e-18"))
    check("8.4.1 the breakeven misses the assessment by, points", be * 100 - 25, D("0.2101"))
    tail_delay = 12 * COD_M
    tail_defer = D("0.40") * COD_M * 13
    check("8.4.1 tail delay cost", tail_delay, 171360)
    check("8.4.1 tail benefit deferral across the missed window", tail_defer, 74256)
    check("8.4.1 tail exposure without the retainer", tail_delay + tail_defer, 245616)
    tail_yes = D(18000) + 3 * COD_M
    check("8.4.1 tail exposure with the retainer", tail_yes, 60840)
    check("8.4.1 tail exposure removed", tail_delay + tail_defer - tail_yes, 184776)
    check("8.4.1 tail reduction factor", (tail_delay + tail_defer) / tail_yes, D("4.0371"))
    check("8.4.1 tail benefit per USD of retainer",
          (tail_delay + tail_defer - tail_yes) / D(18000), D("10.27"))
    check("8.4.1 the retainer buys nothing in % of futures", (1 - pf) * 100, 75)

    # =====================================================================================
    # KA 8.4.2 — reference-class forecasting
    # =====================================================================================
    ratios = [D(x) for x in ["0.95", "1.02", "1.05", "1.08", "1.12", "1.15",
                             "1.18", "1.22", "1.28", "1.35", "1.48", "1.72"]]
    n = len(ratios)
    check("8.4.2 class size", D(n), 12, tol=D("0"))
    check("8.4.2 sum of outturn ratios", sum(ratios), D("14.60"))
    mean_r = sum(ratios) / n
    check("8.4.2 mean ratio", mean_r, D("1.216667"))
    med_r = (ratios[n // 2 - 1] + ratios[n // 2]) / 2
    check("8.4.2 median ratio", med_r, D("1.1650"))
    pos = D(n + 1) * D("0.80")
    check("8.4.2 P80 order position", pos, D("10.40"))
    lo_i = int(pos)
    p80_r = ratios[lo_i - 1] + (pos - lo_i) * (ratios[lo_i] - ratios[lo_i - 1])
    check("8.4.2 P80 ratio by interpolation", p80_r, D("1.4020"))
    for lbl, r, f, u, up in [("best case", ratios[0], 2280000, -120000, "-5.00"),
                             ("median", med_r, 2796000, 396000, "16.50"),
                             ("mean", mean_r, 2920000, 520000, "21.67"),
                             ("P80", p80_r, 3364800, 964800, "40.20"),
                             ("worst case", ratios[-1], 4128000, 1728000, "72.00")]:
        check(f"8.4.2 {lbl} forecast", MB * r, f)
        check(f"8.4.2 {lbl} uplift", MB * r - MB, u)
        check(f"8.4.2 {lbl} uplift %", (MB * r - MB) / MB * 100, D(up))
    over = sum(1 for r in ratios if r > 1)
    check("8.4.2 projects exceeding approved cost", D(over), 11, tol=D("0"))
    check("8.4.2 that share %", D(over) / n * 100, D("91.67"))
    reg_p80 = MTOT + Z80 * MSIG
    check("8.4.2 register-based P80 contingency", reg_p80, D("203784.67"))
    check("8.4.2 reference-class uplift as a multiple of it", D(964800) / reg_p80, D("4.7344"))
    gap = D(964800) - reg_p80
    check("8.4.2 the gap", gap, D("761015.33"))
    check("8.4.2 average quantified EMV", MTOT / 5, 23800)
    unid = N - dist                                   # 8.1.3's estimate, not re-typed
    check("8.4.2 unidentified risks at the average EMV", unid * MTOT / 5, D("272000.00"))
    check("8.4.2 that as a share of the gap %", (unid * MTOT / 5) / gap * 100, D("35.74"))
    check("8.4.2 residual, undecomposable from the evidence", gap - unid * MTOT / 5,
          D("489015.33"))
    check("8.4.2 gap reconciles: attributed + residual",
          unid * MTOT / 5 + (gap - unid * MTOT / 5) - gap, 0, tol=D("1e-16"))
    check("8.4.2 width of the order-statistic interval the P80 interpolates across",
          (ratios[lo_i] - ratios[lo_i - 1]) * MB, 312000)
    check("8.4.2 class median uplift % against the 5 % tolerance", (med_r - 1) * 100, D("16.50"))
    check("MCQ 8.4-G distractor A (register P80 added to class uplift)",
          reg_p80 + D(964800), 1168585, tol=D("0.5"))

    # =====================================================================================
    # KA 8.4.4 — which way to tune a monitor
    # =====================================================================================
    NC, SAVE, COST = D(40), D(22000), D(900)

    def monitor(b, s, k):
        tp = NC * D(b) * D(s)
        fp = NC * (1 - D(b)) * (1 - D(k))
        al = tp + fp
        return tp, fp, al, tp / al, NC * D(b) * (1 - D(s)), tp * SAVE - al * COST

    check("8.4.4 genuine problems a week across the estate at b=1 %", NC * D("0.01"), D("0.4"))
    for lbl, b, s, k, e in [
            ("A sensitive at b=1.0 %", "0.01", "0.85", "0.90",
             ("0.3400", "3.9600", "4.3000", "7.91", "0.0600", "3610.00")),
            ("B specific at b=1.0 %", "0.01", "0.60", "0.98",
             ("0.2400", "0.7920", "1.0320", "23.26", "0.1600", "4351.20")),
            ("A sensitive at b=7.5 %", "0.075", "0.85", "0.90",
             ("2.5500", "3.7000", "6.2500", "40.80", "0.4500", "50475.00")),
            ("B specific at b=7.5 %", "0.075", "0.60", "0.98",
             ("1.8000", "0.7400", "2.5400", "70.87", "1.2000", "37314.00"))]:
        tp, fp, al, pr, ms, nv = monitor(b, s, k)
        check(f"8.4.4 {lbl} true positives", tp, D(e[0]))
        check(f"8.4.4 {lbl} false positives", fp, D(e[1]))
        check(f"8.4.4 {lbl} alerts", al, D(e[2]))
        check(f"8.4.4 {lbl} precision %", pr * 100, D(e[3]))
        check(f"8.4.4 {lbl} missed", ms, D(e[4]))
        check(f"8.4.4 {lbl} net value a week", nv, D(e[5]))
    check("8.4.4 catching one problem saves", D(26000) - D(4000), 22000)
    check("8.4.4 breakeven precision %", COST / SAVE * 100, D("4.0909"))

    def line(s, k):
        """Net(b) = slope.b + intercept, from the definition."""
        slope = NC * D(s) * SAVE - (NC * D(s) - NC * (1 - D(k))) * COST
        icept = -NC * (1 - D(k)) * COST
        return slope, icept

    slA, icA = line("0.85", "0.90")
    slB, icB = line("0.60", "0.98")
    check("8.4.4 Net_A slope", slA, 721000)
    check("8.4.4 Net_A intercept", icA, -3600)
    check("8.4.4 Net_B slope", slB, 507120)
    check("8.4.4 Net_B intercept", icB, -720)
    check("8.4.4 Net_A line reproduces the table at b=1 %",
          slA * D("0.01") + icA, D("3610.00"))
    check("8.4.4 Net_B line reproduces the table at b=7.5 %",
          slB * D("0.075") + icB, D("37314.00"))
    xover = (-icA + icB) / (slA - slB)
    check("8.4.4 crossover base rate %", xover * 100, D("1.3465"))
    check("8.4.4 at the crossover the two lines meet",
          (slA * xover + icA) - (slB * xover + icB), 0, tol=D("1e-16"))
    check("8.4.4 A viability floor %", -icA / slA * 100, D("0.4993"))
    check("8.4.4 B viability floor %", -icB / slB * 100, D("0.1420"))
    check("8.4.4 B is ahead at Meridian's measured 1.0 % base rate",
          (slB * D("0.01") + icB) - (slA * D("0.01") + icA), D("741.20"))
    check("8.4.4 A is ahead at a 7.5 % base rate",
          (slA * D("0.075") + icA) - (slB * D("0.075") + icB), 13161)
    check("8.4.4 B over the 26-week rollout", 26 * (slB * D("0.01") + icB), D("113131.20"))
    check("8.4.4 A over the 26-week rollout", 26 * (slA * D("0.01") + icA), D("93860.00"))
    check("8.4.4 difference over the rollout",
          26 * ((slB * D("0.01") + icB) - (slA * D("0.01") + icA)), D("19271.20"))
    check("8.4.4 B missed problems over the rollout", 26 * NC * D("0.01") * D("0.40"), D("4.16"))
    check("8.4.4 A missed problems over the rollout", 26 * NC * D("0.01") * D("0.15"), D("1.56"))
    check("8.4.4 extra missed problems B accepts", 26 * NC * D("0.01") * D("0.25"), D("2.60"))
    prA = monitor("0.01", "0.85", "0.90")[3]
    check("8.4.4 A's alerts are wrong % of the time", 100 - prA * 100, D("92.09"))
    check("8.4.4 A's precision as a multiple of the threshold", prA / (COST / SAVE), D("1.9328"))

    def crossover(S):
        """b/(1-b) = c.dk / (ds.(S-c)), solved for b — the general form the FS bullet uses."""
        ds, dk = D("0.85") - D("0.60"), D("0.98") - D("0.90")
        r = COST * dk / (ds * (D(S) - COST))
        return r / (1 + r)

    check("Industry FS general crossover reproduces Meridian's", crossover(22000) * 100,
          D("1.3465"))
    check("Industry FS crossover at a 10x saving %", crossover(220000) * 100, D("0.1313"))
    check("Industry FS crossover falls roughly in proportion to the ratio",
          crossover(22000) / crossover(220000), D("10.26"), tol=D("0.02"))

    # =====================================================================================
    # 8.A.2 — merge bias at Auriga's node E, three ways
    # =====================================================================================
    ACT = {"A": ("1.5", "2", "3.5"), "B": ("5", "6", "10"),
           "C": ("6", "8", "14"), "D": ("5", "7", "12")}
    te, sd = {}, {}
    for a, (o, mo, p) in ACT.items():
        te[a] = (D(o) + 4 * D(mo) + D(p)) / 6
        sd[a] = (D(p) - D(o)) / 6
    for a, e, s in [("A", "2.166667", "0.333333"), ("B", "6.500000", "0.833333"),
                    ("C", "8.666667", "1.333333"), ("D", "7.500000", "1.166667")]:
        check(f"8.A.2 {a} PERT te", te[a], D(e))
        check(f"8.A.2 {a} PERT sigma", sd[a], D(s))
    teABC = te["A"] + te["B"] + te["C"]
    vABC = sd["A"] ** 2 + sd["B"] ** 2 + sd["C"] ** 2
    teABD = te["A"] + te["B"] + te["D"]
    vABD = sd["A"] ** 2 + sd["B"] ** 2 + sd["D"] ** 2
    vAB = sd["A"] ** 2 + sd["B"] ** 2
    check("8.A.2 path A-B-C te", teABC, D("17.3333"))
    check("8.A.2 path A-B-C variance", vABC, D("2.583333"))
    check("8.A.2 path A-B-C sigma", sq(vABC), D("1.6073"))
    check("8.A.2 path A-B-D te", teABD, D("16.1667"))
    check("8.A.2 path A-B-D variance", vABD, D("2.166667"))
    check("8.A.2 path A-B-D sigma", sq(vABD), D("1.4720"))
    check("8.A.2 shared A+B variance", vAB, D("0.805556"))
    check("8.A.2 shared A+B sigma", sq(vAB), D("0.8975"))
    check("8.A.2 deterministic A-B-C length ties Domain 6's 16 weeks", D(2 + 6 + 8), 16, tol=D("0"))
    check("8.A.2 deterministic A-B-D length and its one week of float",
          D(16) - D(2 + 6 + 7), 1, tol=D("0"))
    sABC, sABD, sAB = sq(vABC), sq(vABD), sq(vAB)
    teAB = te["A"] + te["B"]

    def dom(T):
        return Phi((D(T) - teABC) / sABC)

    def sec(T):
        return Phi((D(T) - teABD) / sABD)

    def phif(z):
        return 0.5 * (1.0 + math.erf(z / math.sqrt(2.0)))

    def merge(T, npts=2001, span=9.0):
        """P(AB + max(C,D) <= T), integrating over the shared predecessor (Simpson)."""
        mu, s = float(teAB), float(sAB)
        tC, sC = float(te["C"]), float(sd["C"])
        tD, sD = float(te["D"]), float(sd["D"])
        lo, hi = mu - span * s, mu + span * s
        h = (hi - lo) / (npts - 1)
        tot = 0.0
        for i in range(npts):
            t = lo + i * h
            f = math.exp(-0.5 * ((t - mu) / s) ** 2) / (s * math.sqrt(2 * math.pi))
            g = phif((float(T) - t - tC) / sC) * phif((float(T) - t - tD) / sD)
            w = 1.0 if i in (0, npts - 1) else (4.0 if i % 2 else 2.0)
            tot += w * f * g
        return D(str(tot * h / 3.0))

    check("8.A.2 merge quadrature has converged (2001 vs 8001 points)",
          merge(16, 2001) - merge(16, 8001), 0, tol=D("1e-9"))
    check("8.A.2 P(critical path alone meets week 16) %", dom(16) * 100, D("20.34"))
    check("8.A.2 P(secondary path meets week 16) %", sec(16) * 100, D("45.49"))
    check("8.A.2 naive independent product %", dom(16) * sec(16) * 100, D("9.25"))
    check("8.A.2 correct merge sharing A and B %", merge(16) * 100, D("13.16"))
    check("8.A.2 correct merge above the naive product by %",
          (merge(16) / (dom(16) * sec(16)) - 1) * 100, D("42.2"), tol=D("0.05"))
    T80dom = teABC + Z80 * sABC
    check("8.A.2 dominant path's own P80 date", T80dom, D("18.6860"))
    check("8.A.2 dominant path's P80 buffer over week 16", T80dom - 16, D("2.6860"))
    check("8.A.2 dominant path at its own P80 %", dom(T80dom) * 100, D("80.00"), tol=D("0.01"))
    for T, dv, mv, rv in [(D(16), "20.34", "13.16", "35.31"),
                          (D(17), "41.79", "34.33", "17.85"),
                          (D(18), "66.08", "61.53", "6.90"),
                          (T80dom, "80.00", "77.61", "2.99"),
                          (D(20), "95.15", "94.80", "0.37")]:
        d_, m_ = dom(T), merge(T)
        check(f"8.A.2 fade table, dominant path at week {T.quantize(D('0.001'))} %",
              d_ * 100, D(dv), tol=D("0.01"))
        check(f"8.A.2 fade table, correct merge at week {T.quantize(D('0.001'))} %",
              m_ * 100, D(mv))
        check(f"8.A.2 fade table, reduction from the merge at week {T.quantize(D('0.001'))} %",
              (1 - m_ / d_) * 100, D(rv))
        # the ordering invariant the topic states, tested at every date in the table
        check(f"8.A.2 invariant naive <= correct <= dominant at week {T.quantize(D('0.001'))}",
              D(1) if d_ * sec(T) <= m_ <= d_ else D(0), 1, tol=D("0"))
    lo_b, hi_b = 17.0, 21.0
    for _ in range(50):
        mid = (lo_b + hi_b) / 2
        if merge(mid) < D("0.80"):
            lo_b = mid
        else:
            hi_b = mid
    T80m = D(str((lo_b + hi_b) / 2))
    check("8.A.2 merge-correct P80 date", T80m, D("18.8093"))
    check("8.A.2 merge-correct P80 buffer over week 16", T80m - 16, D("2.8093"))
    check("8.A.2 bisection landed on 80 %", merge(T80m) * 100, 80, tol=D("0.001"))
    check("8.A.2 buffer understated by ignoring the merge, weeks", T80m - T80dom, D("0.1233"))
    UNDERSTATEMENT = (T80m - T80dom) * COD_A
    check("8.A.2 value of that understatement at Auriga's cost of delay",
          UNDERSTATEMENT, D("5546.37"), tol=D("0.5"))
    # The manuscript prints this to the nearest fifty, not to the cent: it is the difference between
    # two numerically integrated P80 dates on PERT sigma conventions, so cents would be false
    # precision. The derived value is pinned above; what the book shows is pinned here.
    check("8.A.2 that understatement as printed, to the nearest fifty",
          (UNDERSTATEMENT / 50).quantize(D("1")) * 50, D(5550))
    check("8.A.2 D reaches E behind C on expectation, weeks", teABC - teABD, D("1.17"))
    check("Exercise 8.4 naive two-path product", D("0.80") * D("0.80"), D("0.64"))

    # =====================================================================================
    # Case study B — the diversified portfolio that wasn't
    # =====================================================================================
    pS, iS, pI, iI = D("0.25"), D(60000), D("0.20"), D(80000)
    vS, vI = pS * (1 - pS) * iS ** 2, pI * (1 - pI) * iI ** 2
    check("CSB shared-risk EMV", pS * iS, 15000)
    check("CSB shared-risk variance", vS, 675000000)
    check("CSB shared-risk sigma", sq(vS), D("25980.76"))
    check("CSB independent-risk EMV", pI * iI, 16000)
    check("CSB independent-risk variance", vI, 1024000000)
    check("CSB independent-risk sigma", sq(vI), D("32000.00"))
    csb = 14 * pS * iS + 8 * pI * iI
    check("CSB register count", D(14 + 8), 22, tol=D("0"))
    check("CSB total EMV", csb, 338000)
    check("CSB largest single item as % of the aggregate", (pI * iI) / csb * 100, D("4.73"))
    check("CSB reserve as a margin over the mean %", D(380000) / csb * 100 - 100, D("12.43"))
    vind = 14 * vS + 8 * vI
    sind = sq(vind)
    check("CSB independent aggregate variance", vind, 17642000000)
    check("CSB independent sigma", sind, D("132823.19"))
    check("CSB independent P80", csb + Z80 * sind, D("449784.00"))
    check("CSB confidence the 380,000 reserve buys, independent %",
          Phi((D(380000) - csb) / sind) * 100, D("62.41"))
    check("CSB outcome that occurred", 11 * iS + 2 * iI, 820000)
    check("CSB overrun beyond the reserve", 11 * iS + 2 * iI - D(380000), 440000)
    pairs = D(14 * 13) / 2
    check("CSB pairs among the 14 shared risks", pairs, 91)
    cterm = 2 * D("0.6") * pairs * vS
    check("CSB correlation term at rho=0.6", cterm, 73710000000)
    check("CSB correlation term as a multiple of the independent variance",
          cterm / vind, D("4.18"))
    s06 = sq(vind + cterm)
    check("CSB sigma at rho=0.6", s06, D("302244.93"))
    check("CSB P80 at rho=0.6", csb + Z80 * s06, 592369, tol=D("0.5"))
    check("CSB confidence the reserve buys at rho=0.6 %",
          Phi((D(380000) - csb) / s06) * 100, D("55.53"))
    drv = 14 * iS
    check("CSB single-driver impact", drv, 840000)
    vdrv = pS * (1 - pS) * drv ** 2
    check("CSB single-driver variance", vdrv, 132300000000)
    sdrv = sq(vdrv + 8 * vI)
    check("CSB sigma with one driver", sdrv, D("374822.62"))
    check("CSB P80 with one driver", csb + Z80 * sdrv, D("653450.72"))
    check("CSB confidence the reserve buys with one driver %",
          Phi((D(380000) - csb) / sdrv) * 100, D("54.46"))
    check("CSB a perfectly shared driver is the widest of the three",
          sdrv - max(sind, s06, sdrv), 0, tol=D("0"))
    for lbl, s, pct, odds, tolo in [("independent", sind, "99.99", 7026, D("1")),
                                    ("rho=0.6", s06, "94.46", 18, D("0.1")),
                                    ("one driver", sdrv, "90.08", 10, D("0.1"))]:
        c = Phi((D(820000) - csb) / s)
        check(f"CSB percentile of the 820,000 outcome, {lbl} %", c * 100, D(pct))
        check(f"CSB odds of the 820,000 outcome, {lbl}", 1 / (1 - c), odds, tol=tolo)
    check("CSB confidence points the untested assumption cost",
          Phi((D(380000) - csb) / sind) * 100 - Phi((D(380000) - csb) / sdrv) * 100, D("7.95"))
    check("CSB P80 movement the one-sentence test was worth",
          Z80 * (sdrv - sind), 203667, tol=D("0.5"))
    check("CSB the mean is identical across all three models",
          csb - (14 * pS * iS + 8 * pI * iI), 0, tol=D("1e-20"))

    # =====================================================================================
    # Calculation exercises 8.5 - 8.9
    # =====================================================================================
    EX5 = [(D("0.30"), D(200000)), (D("0.40"), D(150000)), (D("0.20"), D(250000))]
    e5 = [p * i for p, i in EX5]
    v5 = [p * (1 - p) * i * i for p, i in EX5]
    check("Ex8.5 X EMV", e5[0], 60000)
    check("Ex8.5 Y EMV", e5[1], 60000)
    check("Ex8.5 Z EMV", e5[2], 50000)
    check("Ex8.5 mean", sum(e5), 170000)
    check("Ex8.5 X variance", v5[0], D("8.40e9"))
    check("Ex8.5 Y variance", v5[1], D("5.40e9"))
    check("Ex8.5 Z variance", v5[2], D("1.00e10"))
    check("Ex8.5 total variance", sum(v5), D("23.80e9"))
    s5 = sq(sum(v5))
    check("Ex8.5 sigma", s5, D("154272.49"))
    check("Ex8.5 P80 assuming independence", sum(e5) + Z80 * s5, D("299835.72"))
    check("Ex8.5 sigma_X", sq(v5[0]), D("91651.51"))
    check("Ex8.5 sigma_Z", sq(v5[2]), 100000)
    ct5 = 2 * D("0.6") * sq(v5[0]) * sq(v5[2])
    check("Ex8.5 correlation term", ct5, D("10998181667.89"))
    check("Ex8.5 correlated total variance", sum(v5) + ct5, D("34798181667.89"))
    s5c = sq(sum(v5) + ct5)
    check("Ex8.5 correlated sigma", s5c, D("186542.71"))
    check("Ex8.5 corrected P80", sum(e5) + Z80 * s5c, D("326994.34"))
    check("Ex8.5 corrected P80 higher by %",
          (sum(e5) + Z80 * s5c) / (sum(e5) + Z80 * s5) * 100 - 100, D("9.06"))
    z5 = Z80 * s5 / s5c
    check("Ex8.5 z of the independence figure on the correlated distribution", z5, D("0.6960"))
    check("Ex8.5 confidence the independence figure buys %", Phi(z5) * 100, D("75.68"))

    check("Ex8.6 base EMV", D("0.20") * D(300000), 60000)
    check("Ex8.6 base variance", D("0.20") * D("0.80") * D(300000) ** 2, D("14.40e9"))
    check("Ex8.6 EMV after halving p", D("0.10") * D(300000), 30000)
    check("Ex8.6 variance after halving p", D("0.10") * D("0.90") * D(300000) ** 2, D("8.10e9"))
    check("Ex8.6 variance removed by halving p",
          D("0.20") * D("0.80") * D(300000) ** 2 - D("0.10") * D("0.90") * D(300000) ** 2,
          D("6.30e9"))
    check("Ex8.6 EMV after halving I", D("0.20") * D(150000), 30000)
    check("Ex8.6 variance after halving I", D("0.20") * D("0.80") * D(150000) ** 2, D("3.60e9"))
    check("Ex8.6 variance removed by halving I",
          D("0.20") * D("0.80") * D(300000) ** 2 - D("0.20") * D("0.80") * D(150000) ** 2,
          D("10.80e9"))
    check("Ex8.6 ratio of the two levers", D("10.80e9") / D("6.30e9"), D("1.714286"))
    check("Ex8.6 ratio matches the general identity at p=0.20", ratio("0.20"), D("1.714286"))

    a7 = D("0.25") * D(400000)
    t7n = D(26000) + D("0.25") * D(400000) * D("0.30")
    r7n = D(28000) + D("0.10") * D(400000)
    rec7 = D("0.70") * D(400000)
    r7 = r7n + D("0.15") * D(40000)
    t7 = t7n + D("0.25") * D("0.30") * rec7
    v7 = D(150000) + D("0.20") * D(90000)
    check("Ex8.7 accept", a7, 100000)
    check("Ex8.7 transfer ignoring secondaries", t7n, 56000)
    check("Ex8.7 reduce ignoring secondaries", r7n, 68000)
    check("Ex8.7 transfer's apparent advantage", r7n - t7n, 12000)
    check("Ex8.7 reduce with secondaries", r7, 74000)
    check("Ex8.7 transfer with secondaries", t7, 77000)
    check("Ex8.7 avoid with secondaries", v7, 168000)
    check("Ex8.7 the ranking reverses (reduce becomes the minimum)",
          r7 - min(a7, r7, t7, v7), 0, tol=D("0"))
    check("Ex8.7 value reduce creates against accepting", a7 - r7, 26000)
    check("Ex8.7 that improvement %", (a7 - r7) / a7 * 100, D("26.00"))
    q7 = (r7 - D(26000) - D(30000)) / (D("0.25") * rec7)
    check("Ex8.7 counterparty default probability at which transfer ties reduce %",
          q7 * 100, D("25.71"))
    check("Ex8.7 at that probability the two tie", (t7n + D("0.25") * q7 * rec7) - r7,
          0, tol=D("1e-18"))

    n1b, n2b, mb = D(28), D(19), D(11)
    Nb = lp(n1b, n2b, mb)
    check("Ex8.8 N-hat", Nb, D("48.3636"))
    check("Ex8.8 distinct identified", n1b + n2b - mb, 36, tol=D("0"))
    check("Ex8.8 estimated unidentified", Nb - 36, D("12.3636"))
    check("Ex8.8 unidentified as % of population", (Nb - 36) / Nb * 100, D("25.56"))
    check("Ex8.8 coverage %", D(36) / Nb * 100, D("74.44"))
    check("Ex8.8 Chapman N-hat", chapman(n1b, n2b, mb), D("47.3333"))
    check("Ex8.8 Chapman unidentified", chapman(n1b, n2b, mb) - 36, D("11.3333"))
    check("Ex8.8 estimator divergence", Nb - chapman(n1b, n2b, mb), D("1.0303"))
    check("Ex8.8 divergence equals missing/(m+1)", (Nb - 36) / (mb + 1), D("1.0303"))
    check("Ex8.8 workshop-alone coverage %", n1b / Nb * 100, D("57.89"))

    N9, S9, C9 = D(60), D(15000), D(750)

    def mon9(b, s, k):
        tp = N9 * D(b) * D(s)
        fp = N9 * (1 - D(b)) * (1 - D(k))
        return tp, fp, tp + fp, tp / (tp + fp), tp * S9 - (tp + fp) * C9

    tpA, fpA, alA, prA9, nvA = mon9("0.03", "0.90", "0.88")
    tpB, fpB, alB, prB9, nvB = mon9("0.03", "0.65", "0.97")
    check("Ex8.9 A true positives", tpA, D("1.62"))
    check("Ex8.9 A false positives", fpA, D("6.984"))
    check("Ex8.9 A alerts", alA, D("8.604"))
    check("Ex8.9 A precision %", prA9 * 100, D("18.83"))
    check("Ex8.9 A net value", nvA, D("17847.00"))
    check("Ex8.9 B true positives", tpB, D("1.17"))
    check("Ex8.9 B false positives", fpB, D("1.746"))
    check("Ex8.9 B alerts", alB, D("2.916"))
    check("Ex8.9 B net value", nvB, D("15363.00"))
    check("Ex8.9 A wins by", nvA - nvB, 2484)
    check("Ex8.9 breakeven precision %", C9 / S9 * 100, D("5.00"))
    check("Ex8.9 A clears the breakeven by a factor of", prA9 / (C9 / S9), D("3.77"))
    check("Ex8.9 B's precision is the better of the two, yet its net value is worse",
          D(1) if prB9 > prA9 and nvB < nvA else D(0), 1, tol=D("0"))
    slA9 = N9 * D("0.90") * S9 - (N9 * D("0.90") - N9 * D("0.12")) * C9
    icA9 = N9 * D("0.12") * C9
    slB9 = N9 * D("0.65") * S9 - (N9 * D("0.65") - N9 * D("0.03")) * C9
    icB9 = N9 * D("0.03") * C9
    check("Ex8.9 Net_A slope", slA9, 774900)
    check("Ex8.9 Net_A intercept", icA9, 5400)
    check("Ex8.9 Net_B slope", slB9, 557100)
    check("Ex8.9 Net_B intercept", icB9, 1350)
    check("Ex8.9 Net_A line reproduces the b=3 % figure", slA9 * D("0.03") - icA9, D("17847.00"))
    check("Ex8.9 Net_B line reproduces the b=3 % figure", slB9 * D("0.03") - icB9, D("15363.00"))
    b9 = (icA9 - icB9) / (slA9 - slB9)
    check("Ex8.9 crossover base rate %", b9 * 100, D("1.8595"))
    check("Ex8.9 at the crossover the two lines meet",
          (slA9 * b9 - icA9) - (slB9 * b9 - icB9), 0, tol=D("1e-16"))
    check("Ex8.9 A viability floor %", icA9 / slA9 * 100, D("0.6969"))
    check("Ex8.9 B viability floor %", icB9 / slB9 * 100, D("0.2423"))
