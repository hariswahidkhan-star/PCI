"""PML-AI Domain 15 — Programmes, portfolios and enterprise delivery: golden checks.

Every number printed as a result in `domain-15-programmes-portfolios-enterprise.md` is recomputed
here from its definition rather than from its printed output. Master-thread values come from ctx and
are never re-typed: `MERIDIAN_COST_OF_DELAY` (USD 14,280/week, Domain 1), `MERIDIAN_BENEFIT_70PCT`
(USD 685,440), `MERIDIAN_BENEFIT_FULL` (USD 979,200), and the shared `ew(M, L)` helper Domain 3
registered for `E[wait] = M/2 + L`.

Five engines, each checked from its definition:

    dependency    the milestone product Pi p_i over Meridian's six regional predecessors and its
                  fourth power for the four-region programme milestone; the expected count of
                  on-time regions; the k at which a uniform p first falls below one half, found by
                  search rather than asserted; the required per-dependency reliability as the k-th
                  root of a target confidence, verified by re-exponentiation; every decoupling
                  result derived as a division by the removed factor; and the two rival
                  interventions priced per percentage point of programme-milestone gain, with the
                  ratio between them derived rather than typed.
    allocation    full enumeration over the 63 non-empty subsets of the six candidates, with
                  feasibility tested per quarter. Greedy, aggregate-only greedy and the optimum are
                  all PRODUCED by their procedures, not hard-coded, so the 1,010,000 shortfall and
                  the 390,000 aggregate illusion are derived. The same engine is re-run at a
                  reserved capacity of 5 for KA 15.3.3 and on Exercise 15.2's four candidates.
    benefits      the bridge from gross claimed benefit through four typed eliminations to net
                  realistic benefit; the same-pool elimination computed as the excess over the pool
                  cap; the adoption-invariance identity of the elimination rate checked at both
                  bases; simple payback both ways; and the breakeven elimination rate solved from
                  the investment rule rather than asserted.
    capacity      Little's Law T = W/C at both work-in-progress levels and the excess-concurrency
                  cost as benefit run rate x excess / throughput, cross-checked per initiative; plan
                  survival as a product over periods of P(loss <= slack) built from a cumulative
                  loss distribution; the breakeven breach cost solved from the value forgone and the
                  survival improvement; and the enterprise PMO's recurring test with the share of
                  the allocation gap it must capture derived from the deficit.
    enterprise    cumulative tier latency Sum(ew(M, L)) across five tiers, gross latency-weeks from
                  the decision-class distribution, the annual bill at the critical-path share, and
                  each of the three redesigns computed from its own mechanism (per-traversal for the
                  paper deadline, per-decision for the out-of-cycle route and the tier removal).
                  Portfolio EVM is aggregated from summed EV/AC/PV and contrasted with the
                  unweighted mean of component CPIs, with the variance swing derived from the two
                  EACs.

Probabilities are held as exact Decimals throughout; money is Decimal. Tolerances are the default
0.005 except where a display rounding boundary is named in the check.
"""

from itertools import combinations


def run(ctx):
    check, D, ew = ctx["check"], ctx["D"], ctx["ew"]
    COD = ctx["MERIDIAN_COST_OF_DELAY"]              # USD 14,280 a week (Domain 1)
    BEN70 = ctx["MERIDIAN_BENEFIT_70PCT"]            # USD 685,440 a year
    BENFULL = ctx["MERIDIAN_BENEFIT_FULL"]           # USD 979,200 a year
    ADOPT = D("0.70")

    # master-thread consistency: the cost of delay IS the 70 % benefit over a 48-week year
    check("D15 thread: cost of delay from the 70 % benefit", BEN70 / 48, COD)
    check("D15 thread: 70 % benefit from full potential", BENFULL * ADOPT, BEN70)
    check("D15 15.1.4 per-region weekly benefit (10 of 40 clinics)", BEN70 / 4 / 48, D(3570))
    check("D15 15.1.4 per-region annual benefit", BEN70 / 4, D(171360))

    # ================================================================ 1. dependency arithmetic
    DEP = (D("0.90"), D("0.88"), D("0.85"), D("0.92"), D("0.95"), D("0.90"))

    def prod(seq):
        r = D(1)
        for x in seq:
            r *= x
        return r

    pr = prod(DEP)
    check("D15 WE 15.1.3 Region A milestone product", pr, D("0.52953912"), D("0.0000001"))
    check("D15 WE 15.1.3 Region A as a percentage", pr * 100, D("52.9539"), D("0.0001"))
    check("D15 WE 15.1.3 four-region programme milestone %", pr ** 4 * 100,
          D("7.8631"), D("0.0001"))
    check("D15 WE 15.1.3 expected regions on time", D(4) * pr, D("2.1182"), D("0.0001"))
    check("D15 WE 15.1.3 expected regions late", D(4) - D(4) * pr, D("1.8818"), D("0.0001"))
    check("D15 case A: probability the committed date is missed %", (1 - pr ** 4) * 100,
          D("92.1369"), D("0.0001"))

    # required per-dependency reliability as the k-th root, verified by re-exponentiation
    def kth_root(target, k):
        lo, hi = D(0), D(1)
        for _ in range(200):
            mid = (lo + hi) / 2
            if mid ** k < target:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    r24 = kth_root(D("0.80"), 24)
    r6 = kth_root(D("0.80"), 6)
    r5 = kth_root(D("0.85"), 5)
    check("D15 WE 15.1.3 required reliability, 24 deps at 80 % %", r24 * 100,
          D("99.0745"), D("0.0001"))
    check("D15 WE 15.1.3 re-exponentiation check, 24 deps", r24 ** 24, D("0.80"), D("0.000001"))
    check("D15 WE 15.1.3 required reliability, 6 deps at 80 % %", r6 * 100,
          D("96.3492"), D("0.0001"))
    check("D15 Ex 15.1(d) required reliability, 5 deps at 85 % %", r5 * 100,
          D("96.8019"), D("0.0001"))
    check("D15 Ex 15.1(d) re-exponentiation check, 5 deps", r5 ** 5, D("0.85"), D("0.000001"))

    # the k at which a uniform p first falls below one half, found by search
    def first_k_below_half(p):
        k, v = 1, p
        while v >= D("0.5"):
            k += 1
            v *= p
        return k

    for p, k_exp in ((D("0.99"), 69), (D("0.95"), 14), (D("0.90"), 7), (D("0.85"), 5)):
        check(f"D15 Fig 15.1.1 first k below 0.50 at p={p}", D(first_k_below_half(p)), D(k_exp))
    check("D15 Ex 15.1(c) 0.93^9 %", D("0.93") ** 9 * 100, D("52.0411"), D("0.0001"))
    check("D15 Ex 15.1(c) 0.93^10 %", D("0.93") ** 10 * 100, D("48.3982"), D("0.0001"))
    check("D15 Ex 15.1(c) largest k at or above 0.50", D(first_k_below_half(D("0.93")) - 1), D(9))

    # +0.03 absolute sensitivity: best and worst single-dependency improvement
    gains = sorted((pr / d * (d + D("0.03")) - pr) * 100 for d in DEP)
    check("D15 WE 15.1.3 smallest +0.03 gain, pts", gains[0], D("1.6722"), D("0.0001"))
    check("D15 WE 15.1.3 largest +0.03 gain, pts", gains[-1], D("1.8690"), D("0.0001"))

    # decoupling: division by the removed factor
    d_mig = pr / D("0.85")
    d_both = d_mig / D("0.90")
    check("D15 15.1.4 decouple migration, region %", d_mig * 100, D("62.2987"), D("0.0001"))
    check("D15 15.1.4 decouple migration, gain pts", (d_mig - pr) * 100, D("9.3448"), D("0.0001"))
    check("D15 MCQ 15.1-D decoupled value", d_mig, D("0.6229872"), D("0.0000001"))
    check("D15 15.1.4 decouple migration + IG, region %", d_both * 100, D("69.2208"), D("0.0001"))
    check("D15 15.1.4 decouple both, gain pts", (d_both - pr) * 100, D("16.2669"), D("0.0001"))
    check("D15 15.1.4 four regions after decoupling %", d_both ** 4 * 100, D("22.9587"), D("0.0001"))
    check("D15 15.1.4 expected regions on time after", D(4) * d_both, D("2.7688"), D("0.0001"))
    check("D15 15.1.4 expected regions late after", D(4) - D(4) * d_both, D("1.2312"), D("0.0001"))

    # the money route, and the cost per percentage point of each
    rec = pr / D("0.95") * D("0.98") / D("0.92") * D("0.96")
    check("D15 15.1.4 recovery-spend region %", rec * 100, D("57.0012"), D("0.0001"))
    check("D15 15.1.4 recovery-spend four regions %", rec ** 4 * 100, D("10.5569"), D("0.0001"))
    g_rec = (rec ** 4 - pr ** 4) * 100
    g_str = (d_both ** 4 - pr ** 4) * 100
    check("D15 15.1.4 recovery gain, pts", g_rec, D("2.6938"), D("0.0001"))
    check("D15 15.1.4 structural gain, pts", g_str, D("15.0956"), D("0.0001"))
    check("D15 15.1.4 recovery cost per point", D(240000) / g_rec, D("89093.08"), D("0.01"))
    check("D15 15.1.4 structural cost per point", D(18000) / g_str, D("1192.40"), D("0.01"))
    check("D15 15.1.4 ratio of costs per point",
          (D(240000) / g_rec) / (D(18000) / g_str), D("74.72"), D("0.01"))
    check("D15 15.1.4 structural as % of recovery cost per point",
          (D(18000) / g_str) / (D(240000) / g_rec) * 100, D("1.34"), D("0.01"))

    # MCQ 15.1-A and its named distractors
    m1 = (D("0.95"), D("0.90"), D("0.90"), D("0.85"))
    check("D15 MCQ 15.1-A correct product", prod(m1), D("0.654075"), D("0.000001"))
    check("D15 MCQ 15.1-A distractor A (mean)", sum(m1) / 4, D("0.90"))
    check("D15 MCQ 15.1-A distractor D (1 - summed shortfalls)",
          1 - sum(1 - x for x in m1), D("0.60"))
    check("D15 MCQ 15.1-B distractor B (divide by four) %", pr / 4 * 100, D("13.2385"), D("0.0001"))
    check("D15 MCQ 15.1-B distractor D (square, not fourth power) %", pr ** 2 * 100,
          D("28.0412"), D("0.0001"))

    # ================================================================ 2. portfolio allocation
    # (key, quarterly demand, NPV)
    CAND = (("A", (2, 3, 3, 2), D(1700000)), ("B", (2, 2, 2, 2), D(1360000)),
            ("C", (5, 1, 0, 0), D(1140000)), ("D", (0, 1, 2, 3), D(1020000)),
            ("E", (1, 1, 1, 1), D(640000)), ("F", (2, 0, 1, 2), D(750000)))

    def units(c):
        return sum(c[1])

    def ratio(c):
        return c[2] / units(c)

    def demand(sel, nq):
        return [sum(c[1][q] for c in sel) for q in range(nq)]

    def greedy(cands, cap, nq):
        rem, sel = [cap] * nq, []
        for c in sorted(cands, key=lambda c: (-ratio(c), -c[2])):
            if all(c[1][q] <= rem[q] for q in range(nq)):
                sel.append(c)
                rem = [rem[q] - c[1][q] for q in range(nq)]
        return sel

    def aggregate_greedy(cands, cap, nq):
        rem, sel = cap * nq, []
        for c in sorted(cands, key=lambda c: (-ratio(c), -c[2])):
            if units(c) <= rem:
                sel.append(c)
                rem -= units(c)
        return sel

    def enumerate_best(cands, cap, nq):
        best, bv, nfeas = None, D(-1), 0
        for r in range(1, len(cands) + 1):
            for combo in combinations(cands, r):
                if all(d <= cap for d in demand(combo, r and nq)):
                    nfeas += 1
                    v = sum(c[2] for c in combo)
                    if v > bv:
                        best, bv = combo, v
        return best, bv, nfeas

    CAP, NQ = 6, 4
    check("D15 WE 15.2.3 aggregate capacity, units", D(CAP * NQ), D(24))
    for k, u in (("A", 10), ("B", 8), ("C", 6), ("D", 6), ("E", 4), ("F", 5)):
        c = next(x for x in CAND if x[0] == k)
        check(f"D15 WE 15.2.3 candidate {k} units", D(units(c)), D(u))
    for k, rr in (("A", 170000), ("B", 170000), ("C", 190000), ("D", 170000),
                  ("E", 160000), ("F", 150000)):
        c = next(x for x in CAND if x[0] == k)
        check(f"D15 WE 15.2.3 candidate {k} NPV per unit", ratio(c), D(rr))

    g = greedy(CAND, CAP, NQ)
    gv = sum(c[2] for c in g)
    check("D15 WE 15.2.3 greedy set is C+D+E", D(1) if [c[0] for c in g] == ["C", "D", "E"] else D(0), D(1))
    check("D15 WE 15.2.3 greedy NPV", gv, D(2800000))
    check("D15 WE 15.2.3 greedy units", D(sum(units(c) for c in g)), D(16))

    best, bv, nfeas = enumerate_best(CAND, CAP, NQ)
    check("D15 WE 15.2.3 subsets enumerated", D(2 ** len(CAND) - 1), D(63))
    check("D15 WE 15.2.3 feasible subsets", D(nfeas), D(26))
    check("D15 WE 15.2.3 optimum set is A+B+F",
          D(1) if [c[0] for c in best] == ["A", "B", "F"] else D(0), D(1))
    check("D15 WE 15.2.3 optimum NPV", bv, D(3810000))
    check("D15 WE 15.2.3 optimum units", D(sum(units(c) for c in best)), D(23))
    dopt = demand(best, NQ)
    for q, v in zip(range(4), (6, 5, 6, 6)):
        check(f"D15 WE 15.2.3 optimum Q{q+1} demand", D(dopt[q]), D(v))
    check("D15 WE 15.2.3 optimum utilisation %",
          D(sum(units(c) for c in best)) / D(CAP * NQ) * 100, D("95.83"), D("0.01"))
    check("D15 WE 15.2.3 greedy shortfall", bv - gv, D(1010000))
    check("D15 WE 15.2.3 greedy shortfall % of optimum", (bv - gv) / bv * 100,
          D("26.51"), D("0.01"))
    check("D15 WE 15.2.3 greedy captured % of optimum", gv / bv * 100, D("73.49"), D("0.01"))
    # runner-up
    runner = max((c for r in range(1, len(CAND) + 1) for c in combinations(CAND, r)
                  if all(d <= CAP for d in demand(c, NQ)) and sum(x[2] for x in c) != bv),
                 key=lambda c: sum(x[2] for x in c))
    check("D15 WE 15.2.3 runner-up NPV (A+B+E)", sum(c[2] for c in runner), D(3700000))
    check("D15 WE 15.2.3 gap to runner-up", bv - sum(c[2] for c in runner), D(110000))

    ag = aggregate_greedy(CAND, CAP, NQ)
    agv = sum(c[2] for c in ag)
    check("D15 WE 15.2.3 aggregate-only set is C+A+B",
          D(1) if sorted(c[0] for c in ag) == ["A", "B", "C"] else D(0), D(1))
    check("D15 WE 15.2.3 aggregate-only NPV", agv, D(4200000))
    check("D15 WE 15.2.3 aggregate-only units", D(sum(units(c) for c in ag)), D(24))
    check("D15 WE 15.2.3 aggregate-only Q1 demand", D(demand(ag, NQ)[0]), D(9))
    check("D15 WE 15.2.3 aggregate-only Q1 breach", D(demand(ag, NQ)[0] - CAP), D(3))
    check("D15 WE 15.2.3 aggregate illusion", agv - bv, D(390000))
    check("D15 WE 15.2.3 aggregate illusion % of optimum", (agv - bv) / bv * 100,
          D("10.24"), D("0.01"))
    check("D15 15.2.3 enumeration at 25 candidates", D(2 ** 25), D(33554432))

    # Exercise 15.2 — same engine, three periods
    EX2 = (("P", (2, 2, 2), D(1020000)), ("Q", (2, 2, 1), D(850000)),
           ("R", (4, 1, 0), D(950000)), ("S", (1, 1, 2), D(640000)))
    check("D15 Ex 15.2 R ratio", ratio(EX2[2]), D(190000))
    check("D15 Ex 15.2 P ratio", ratio(EX2[0]), D(170000))
    check("D15 Ex 15.2 Q ratio", ratio(EX2[1]), D(170000))
    check("D15 Ex 15.2 S ratio", ratio(EX2[3]), D(160000))
    g2 = greedy(EX2, 5, 3)
    check("D15 Ex 15.2 greedy set is R+S",
          D(1) if [c[0] for c in g2] == ["R", "S"] else D(0), D(1))
    check("D15 Ex 15.2 greedy NPV", sum(c[2] for c in g2), D(1590000))
    check("D15 Ex 15.2 greedy units", D(sum(units(c) for c in g2)), D(9))
    b2, bv2, _ = enumerate_best(EX2, 5, 3)
    check("D15 Ex 15.2 optimum set is P+Q+S",
          D(1) if [c[0] for c in b2] == ["P", "Q", "S"] else D(0), D(1))
    check("D15 Ex 15.2 optimum NPV", bv2, D(2510000))
    check("D15 Ex 15.2 optimum units", D(sum(units(c) for c in b2)), D(15))
    check("D15 Ex 15.2 optimum P1 demand", D(demand(b2, 3)[0]), D(5))
    check("D15 Ex 15.2 greedy shortfall", bv2 - sum(c[2] for c in g2), D(920000))
    check("D15 Ex 15.2 greedy shortfall % of optimum",
          (bv2 - sum(c[2] for c in g2)) / bv2 * 100, D("36.65"), D("0.01"))
    a2 = aggregate_greedy(EX2, 5, 3)
    check("D15 Ex 15.2 aggregate-only NPV", sum(c[2] for c in a2), D(2610000))
    check("D15 Ex 15.2 aggregate-only units", D(sum(units(c) for c in a2)), D(15))
    check("D15 Ex 15.2 aggregate-only P1 demand", D(demand(a2, 3)[0]), D(7))
    check("D15 Ex 15.2 aggregate-only P1 breach", D(demand(a2, 3)[0] - 5), D(2))
    check("D15 Ex 15.2 aggregate overstatement", sum(c[2] for c in a2) - bv2, D(100000))
    check("D15 Ex 15.2 subsets enumerated", D(2 ** 4 - 1), D(15))

    # ================================================================ 3. benefits bridge
    CLAIM = (BENFULL, D(264000), D(318000), D(180000), D(240000))
    gross = sum(CLAIM)
    check("D15 WE 15.2.1 gross claimed at full potential", gross, D(1981200))
    FTE, POOL, CLAIMED_FTE = D(42000), D("6.0"), D("8.4")
    e_pool = (CLAIMED_FTE - POOL) * FTE
    check("D15 WE 15.2.1 same-pool excess posts", CLAIMED_FTE - POOL, D("2.4"))
    check("D15 WE 15.2.1 same-pool elimination", e_pool, D(100800))
    check("D15 MCQ 15.2-D distractor A (gross claim)", CLAIMED_FTE * FTE, D(352800))
    check("D15 MCQ 15.2-D distractor B (pool value)", POOL * FTE, D(252000))
    elim = D(96000) + e_pool + D(108000) + D(60000)
    check("D15 WE 15.2.1 total eliminations", elim, D(364800))
    net_fp = gross - elim
    check("D15 WE 15.2.1 net full potential", net_fp, D(1616400))
    check("D15 WE 15.2.1 elimination rate %", elim / gross * 100, D("18.4131"), D("0.0001"))
    net_real = net_fp * ADOPT
    naive = gross * ADOPT
    check("D15 WE 15.2.1 net realistic benefit", net_real, D(1131480))
    check("D15 WE 15.2.1 unreconciled realistic benefit", naive, D(1386840))
    check("D15 WE 15.2.1 overstatement", naive - net_real, D(255360))
    check("D15 WE 15.2.1 overstatement % of honest figure",
          (naive - net_real) / net_real * 100, D("22.57"), D("0.01"))
    check("D15 WE 15.2.1 adoption-invariance of the elimination rate",
          (naive - net_real) / naive * 100, D("18.4131"), D("0.0001"))
    check("D15 MCQ 15.2-C distractor D (adoption applied twice)", net_real * ADOPT, D(792036))
    PCOST = D(2400000) + D(640000) + D(890000) + D(520000) + D(430000)
    check("D15 WE 15.2.1 portfolio approved cost", PCOST, D(4880000))
    check("D15 WE 15.2.1 payback, reconciled, years", PCOST / net_real, D("4.3129"), D("0.0001"))
    check("D15 WE 15.2.1 payback, unreconciled, years", PCOST / naive, D("3.5188"), D("0.0001"))
    check("D15 WE 15.2.1 payback difference, years", PCOST / net_real - PCOST / naive,
          D("0.7941"), D("0.0001"))
    check("D15 WE 15.2.1 payback difference, months",
          (PCOST / net_real - PCOST / naive) * 12, D("9.53"), D("0.01"))
    need = PCOST / 4
    be_elim = gross - need / ADOPT
    check("D15 WE 15.2.1 4-year rule requires net benefit", need, D(1220000))
    check("D15 WE 15.2.1 breakeven elimination rate %", be_elim / gross * 100,
          D("12.0302"), D("0.0001"))

    # Exercise 15.3 — the same bridge on independent numbers
    g3 = D(1240000)
    e3 = D(85000) + (D("4.8") - D("3.5")) * D(38000) + D(64000) + D(45000)
    check("D15 Ex 15.3 same-pool elimination", (D("4.8") - D("3.5")) * D(38000), D(49400))
    check("D15 Ex 15.3 total eliminations", e3, D(243400))
    check("D15 Ex 15.3 net full potential", g3 - e3, D(996600))
    check("D15 Ex 15.3 elimination rate %", e3 / g3 * 100, D("19.6290"), D("0.0001"))
    A3 = D("0.65")
    h3, n3 = (g3 - e3) * A3, g3 * A3
    check("D15 Ex 15.3 honest net benefit", h3, D(647790))
    check("D15 Ex 15.3 unreconciled net benefit", n3, D(806000))
    check("D15 Ex 15.3 overstatement", n3 - h3, D(158210))
    check("D15 Ex 15.3 overstatement % of honest", (n3 - h3) / h3 * 100, D("24.42"), D("0.01"))
    C3 = D(3150000)
    check("D15 Ex 15.3 payback honest, years", C3 / h3, D("4.8627"), D("0.0001"))
    check("D15 Ex 15.3 payback unreconciled, years", C3 / n3, D("3.9082"), D("0.0001"))
    need3 = C3 / 4
    check("D15 Ex 15.3 4-year rule requires", need3, D(787500))
    check("D15 Ex 15.3 required net full potential", need3 / A3, D("1211538.46"), D("0.01"))
    check("D15 Ex 15.3 max eliminations", g3 - need3 / A3, D("28461.54"), D("0.01"))
    check("D15 Ex 15.3 breakeven elimination rate %", (g3 - need3 / A3) / g3 * 100,
          D("2.2953"), D("0.0001"))
    check("D15 Ex 15.3 common error (pool value not excess)", D("3.5") * D(38000), D(133000))

    # ================================================================ 4. capacity, WIP, PMO
    C_TP, W_NOW, W_LIM = D(5), D(12), D(5)
    check("D15 WE 15.3.2 throughput from 15 in 3 years", D(15) / D(3), C_TP)
    check("D15 WE 15.3.2 cycle time at W=12, years", W_NOW / C_TP, D("2.40"), D("0.001"))
    check("D15 WE 15.3.2 cycle time at W=5, years", W_LIM / C_TP, D("1.00"), D("0.001"))
    excess_T = (W_NOW - W_LIM) / C_TP
    check("D15 WE 15.3.2 excess cycle time, years", excess_T, D("1.40"), D("0.001"))
    check("D15 WE 15.3.2 benefit per initiative", net_real / 5, D(226296))
    check("D15 WE 15.3.2 benefit forgone once", net_real * excess_T, D(1584072))
    check("D15 WE 15.3.2 cross-check per initiative",
          net_real / 5 * excess_T * 5, D(1584072))
    check("D15 15.3.2 W for a 1.5-year cycle time", C_TP * D("1.5"), D("7.50"), D("0.001"))
    check("D15 MCQ 15.3-A distractor A (inverted ratio), years", C_TP / W_NOW,
          D("0.42"), D("0.005"))

    # Exercise 15.5
    check("D15 Ex 15.5 cycle time at W=20, C=8, years", D(20) / D(8), D("2.50"), D("0.001"))
    check("D15 Ex 15.5 cycle time at W=8, years", D(8) / D(8), D("1.00"), D("0.001"))
    check("D15 Ex 15.5 excess cycle time, years", D(12) / D(8), D("1.50"), D("0.001"))
    check("D15 Ex 15.5 benefit forgone", D(2240000) * D(12) / D(8), D(3360000))
    check("D15 Ex 15.5 W for T=1.5", D(8) * D("1.5"), D("12.00"), D("0.001"))

    # plan survival: product over periods of P(loss <= slack)
    LOSS = {0: D("0.70"), 1: D("0.22"), 2: D("0.06")}
    CUM = {0: LOSS[0], 1: LOSS[0] + LOSS[1], 2: LOSS[0] + LOSS[1] + LOSS[2]}
    check("D15 WE 15.3.3 P(loss <= 1)", CUM[1], D("0.92"))
    check("D15 WE 15.3.3 P(loss <= 2)", CUM[2], D("0.98"))

    def survival(dem, cap):
        r = D(1)
        for d in dem:
            r *= CUM[min(cap - d, 2)]
        return r

    s_agg = survival(dopt, CAP)
    check("D15 WE 15.3.3 aggressive plan survival", s_agg, D("0.31556"), D("0.000001"))
    check("D15 WE 15.3.3 aggressive plan survival %", s_agg * 100, D("31.5560"), D("0.0001"))
    bp, bpv, _ = enumerate_best(CAND, 5, NQ)
    check("D15 WE 15.3.3 protective plan is A+E+F",
          D(1) if [c[0] for c in bp] == ["A", "E", "F"] else D(0), D(1))
    check("D15 WE 15.3.3 protective plan NPV", bpv, D(3090000))
    dprot = demand(bp, NQ)
    for q, v in zip(range(4), (5, 4, 5, 5)):
        check(f"D15 WE 15.3.3 protective Q{q+1} demand", D(dprot[q]), D(v))
    s_prot = survival(dprot, CAP)
    check("D15 WE 15.3.3 protective plan survival", s_prot, D("0.76311424"), D("0.00000001"))
    check("D15 WE 15.3.3 protective plan survival %", s_prot * 100, D("76.3114"), D("0.0001"))
    forgone = bv - bpv
    check("D15 WE 15.3.3 NPV forgone by reserving", forgone, D(720000))
    check("D15 WE 15.3.3 NPV forgone % of optimum", forgone / bv * 100, D("18.90"), D("0.01"))
    breach = D(13) * COD
    check("D15 WE 15.3.3 one quarter's breach cost", breach, D(185640))
    check("D15 WE 15.3.3 expected slippage, aggressive", (1 - s_agg) * breach,
          D("127059.44"), D("0.01"))
    check("D15 WE 15.3.3 expected slippage, protective", (1 - s_prot) * breach,
          D("43975.47"), D("0.01"))
    check("D15 WE 15.3.3 expected slippage saving",
          (1 - s_agg) * breach - (1 - s_prot) * breach, D("83083.97"), D("0.01"))
    be_breach = forgone / (s_prot - s_agg)
    check("D15 WE 15.3.3 breakeven breach cost", be_breach, D("1608743.56"), D("0.01"))
    check("D15 WE 15.3.3 breakeven in weeks of delay", be_breach / COD, D("112.66"), D("0.01"))
    check("D15 WE 15.3.3 breakeven in quarters", be_breach / breach, D("8.67"), D("0.01"))
    check("D15 MCQ 15.3-C distractor B (0.92 in two quarters)",
          CUM[0] * CUM[1] * CUM[1] * CUM[0] * 100, D("41.4736"), D("0.0001"))
    check("D15 MCQ 15.3-C distractor D (no slack tolerance)",
          CUM[0] ** 4 * 100, D("24.01"), D("0.01"))

    # enterprise PMO
    pmo = 8 * D(132000) + D(124000)
    check("D15 WE 15.3.4 PMO annual cost", pmo, D(1180000))
    check("D15 WE 15.3.4 PMO cost % of portfolio cost", pmo / PCOST * 100, D("24.18"), D("0.01"))
    LAT_SAVE = D(517650)                      # derived in the enterprise engine below
    recurring = LAT_SAVE + D(186000)
    oneoff = net_real * D("0.5")
    check("D15 WE 15.3.4 recurring value", recurring, D(703650))
    check("D15 WE 15.3.4 one-off value", oneoff, D(565740))
    check("D15 WE 15.3.4 year-one total", recurring + oneoff, D(1269390))
    check("D15 WE 15.3.4 year-one surplus", recurring + oneoff - pmo, D(89390))
    check("D15 WE 15.3.4 year-two deficit", pmo - recurring, D(476350))
    check("D15 WE 15.3.4 share of allocation gap needed %",
          (pmo - recurring) / (bv - gv) * 100, D("47.1634"), D("0.0001"))

    # ================================================================ 5. enterprise latency, EVM
    TIERS = ((2, 1), (4, 2), (6, 2), (12, 3), (13, 4))
    lat = [ew(M, L) for M, L in TIERS]
    for i, v in enumerate((D(2), D(4), D(5), D(9), D("10.5"))):
        check(f"D15 WE 15.4.1 tier {i+1} latency", lat[i], v)
    cum = []
    run_ = D(0)
    for e in lat:
        run_ += e
        cum.append(run_)
    for i, v in enumerate((D(2), D(6), D(11), D(20), D("30.5"))):
        check(f"D15 WE 15.4.1 cumulative latency through tier {i+1}", cum[i], v)
    check("D15 WE 15.4.1 five-tier path cost", cum[4] * COD, D(435540))
    check("D15 MCQ 15.4-A distractor A (M/2 only)", sum(D(M) / 2 for M, _ in TIERS), D("18.5"))
    check("D15 MCQ 15.4-A distractor C (intervals only)", sum(D(M) for M, _ in TIERS), D(37))
    check("D15 MCQ 15.4-A distractor D (omit top tier)", cum[3], D(20))

    VOL = ((1, 48), (2, 22), (3, 9), (4, 4), (5, 2))
    gross_w = sum(D(n) * cum[t - 1] for t, n in VOL)
    ndec = sum(n for _, n in VOL)
    trav = sum(n * t for t, n in VOL)
    check("D15 WE 15.4.1 decisions a year", D(ndec), D(85))
    check("D15 WE 15.4.1 gross latency-weeks", gross_w, D(468))
    check("D15 WE 15.4.1 tier traversals", D(trav), D(145))
    check("D15 WE 15.4.1 average latency per decision, weeks", gross_w / ndec,
          D("5.5059"), D("0.0001"))
    CP = D("0.25")
    check("D15 WE 15.4.1 delaying weeks", gross_w * CP, D("117.0"), D("0.001"))
    bill = gross_w * CP * COD
    check("D15 WE 15.4.1 annual latency bill", bill, D(1670760))
    save_L = D(trav) * CP * COD
    check("D15 WE 15.4.1 paper-deadline saving", save_L, LAT_SAVE)
    check("D15 WE 15.4.1 paper-deadline saving % of bill", save_L / bill * 100,
          D("30.9829"), D("0.0001"))
    check("D15 MCQ 15.4-B distractor C (per decision, not traversal)", D(ndec) * CP * COD,
          D(303450))
    check("D15 MCQ 15.4-B distractor D (omit critical-path share)", D(trav) * COD, D(2070600))
    top_w = D(4) * cum[3] + D(2) * cum[4]
    check("D15 WE 15.4.1 top two classes, gross weeks", top_w, D(141))
    check("D15 WE 15.4.1 top two classes, % of decisions", D(6) / D(ndec) * 100,
          D("7.0588"), D("0.0001"))
    check("D15 WE 15.4.1 top two classes, % of latency", top_w / gross_w * 100,
          D("30.1282"), D("0.0001"))
    wr = D(4) * (cum[3] - D("1.5")) + D(2) * (cum[4] - D("1.5"))
    check("D15 WE 15.4.1 written-resolution saving", wr * CP * COD, D(471240))
    check("D15 WE 15.4.1 written-resolution % of bill", wr * CP * COD / bill * 100,
          D("28.2051"), D("0.0001"))
    skip = (D(9) + D(4) + D(2)) * lat[0]
    check("D15 WE 15.4.1 tier-removal saving", skip * CP * COD, D(107100))
    check("D15 WE 15.4.1 tier-removal % of bill", skip * CP * COD / bill * 100,
          D("6.4103"), D("0.0001"))
    check("D15 15.A.1 cost of one inserted tier",
          D(22) * D(4) * CP * COD, D(314160))

    # Exercise 15.4
    T4 = ((2, 1), (4, 1), (8, 2), (12, 4))
    l4 = [ew(M, L) for M, L in T4]
    for i, v in enumerate((D(2), D(3), D(6), D(10))):
        check(f"D15 Ex 15.4 tier {i+1} latency", l4[i], v)
    c4, r4 = [], D(0)
    for e in l4:
        r4 += e
        c4.append(r4)
    for i, v in enumerate((D(2), D(5), D(11), D(21))):
        check(f"D15 Ex 15.4 cumulative through tier {i+1}", c4[i], v)
    V4 = ((1, 40), (2, 18), (3, 7), (4, 3))
    gw4 = sum(D(n) * c4[t - 1] for t, n in V4)
    tr4 = sum(n * t for t, n in V4)
    nd4 = sum(n for _, n in V4)
    CP4, COD4 = D("0.30"), D(11500)
    check("D15 Ex 15.4 gross latency-weeks", gw4, D(310))
    check("D15 Ex 15.4 tier traversals", D(tr4), D(109))
    check("D15 Ex 15.4 decisions", D(nd4), D(68))
    check("D15 Ex 15.4 delaying weeks", gw4 * CP4, D("93.0"), D("0.001"))
    check("D15 Ex 15.4 annual bill", gw4 * CP4 * COD4, D(1069500))
    check("D15 Ex 15.4 paper-deadline saving", D(tr4) * CP4 * COD4, D(376050))
    check("D15 Ex 15.4 paper-deadline % of bill",
          D(tr4) * CP4 * COD4 / (gw4 * CP4 * COD4) * 100, D("35.1613"), D("0.0001"))
    half4 = sum(D(n) * sum(D(M) / 2 for M, _ in T4[:t]) for t, n in V4)
    check("D15 Ex 15.4 M/2-only error, weeks", half4, D(182))
    check("D15 Ex 15.4 M/2-only understatement %", (gw4 - half4) / gw4 * 100,
          D("41.2903"), D("0.0001"))
    check("D15 Ex 15.4 top class % of decisions", D(3) / D(nd4) * 100, D("4.4118"), D("0.0001"))
    check("D15 Ex 15.4 top class % of latency", D(3) * c4[3] / gw4 * 100,
          D("20.3226"), D("0.0001"))

    # portfolio EVM aggregation
    EVM = ((D(1300000), D(1196000), D(1352000), D(2400000)),
           (D(380000), D(361000), D(372000), D(640000)),
           (D(250000), D(262500), D(245000), D(890000)),
           (D(300000), D(306000), D(291000), D(520000)),
           (D(220000), D(224400), D(213000), D(430000)))
    tPV = sum(r[0] for r in EVM)
    tEV = sum(r[1] for r in EVM)
    tAC = sum(r[2] for r in EVM)
    tBAC = sum(r[3] for r in EVM)
    check("D15 WE 15.4.2 portfolio PV", tPV, D(2450000))
    check("D15 WE 15.4.2 portfolio EV", tEV, D(2349900))
    check("D15 WE 15.4.2 portfolio AC", tAC, D(2473000))
    check("D15 WE 15.4.2 portfolio BAC equals approved cost", tBAC, PCOST)
    for i, v in enumerate((D("0.8846"), D("0.9704"), D("1.0714"), D("1.0515"), D("1.0535"))):
        check(f"D15 WE 15.4.2 component {i+1} CPI", EVM[i][1] / EVM[i][2], v, D("0.0001"))
    for i, v in enumerate((D("54.67"), D("15.04"), D("9.91"), D("11.77"), D("8.61"))):
        check(f"D15 WE 15.4.2 component {i+1} share of AC %", EVM[i][2] / tAC * 100, v, D("0.01"))
    cpi_p = tEV / tAC
    spi_p = tEV / tPV
    check("D15 WE 15.4.2 portfolio CPI", cpi_p, D("0.950222"), D("0.000001"))
    check("D15 WE 15.4.2 portfolio SPI", spi_p, D("0.959143"), D("0.000001"))
    mean_cpi = sum(r[1] / r[2] for r in EVM) / 5
    check("D15 WE 15.4.2 unweighted mean CPI", mean_cpi, D("1.006308"), D("0.000001"))
    eac_w, eac_s = tBAC / cpi_p, tBAC / mean_cpi
    check("D15 WE 15.4.2 EAC at portfolio CPI", eac_w, D("5135639.81"), D("0.01"))
    check("D15 WE 15.4.2 VAC at portfolio CPI", tBAC - eac_w, D("-255639.81"), D("0.01"))
    check("D15 WE 15.4.2 EAC at unweighted mean CPI", eac_s, D("4849408.40"), D("0.01"))
    check("D15 WE 15.4.2 VAC at unweighted mean CPI", tBAC - eac_s, D("30591.60"), D("0.01"))
    check("D15 WE 15.4.2 variance swing", (tBAC - eac_s) - (tBAC - eac_w),
          D("286231.41"), D("0.01"))
    check("D15 WE 15.4.2 concentration below CPI 0.95 %",
          sum(r[2] for r in EVM if r[1] / r[2] < D("0.95")) / tAC * 100, D("54.67"), D("0.01"))
    check("D15 WE 15.4.2 complement of the concentration %",
          100 - sum(r[2] for r in EVM if r[1] / r[2] < D("0.95")) / tAC * 100,
          D("45.33"), D("0.01"))
    check("D15 MCQ 15.4-D distractor D (inverted ratio)", tAC / tEV, D("1.05"), D("0.005"))

    # ================================================================ case study B
    CREW, MONTHS, DEM, PEAK, PEAKM = D(9), D(36), D(311), D(17), D(6)
    check("D15 case B aggregate capacity, crew-weeks", CREW * MONTHS, D(324))
    check("D15 case B demand % of aggregate capacity", DEM / (CREW * MONTHS) * 100,
          D("95.99"), D("0.01"))
    check("D15 case B peak demand % of capacity", PEAK / CREW * 100, D("188.89"), D("0.01"))
    unmet = (PEAK - CREW) * PEAKM
    check("D15 case B unmet demand, crew-weeks", unmet, D(48))
    catchup = unmet / CREW
    check("D15 case B structural catch-up, months", catchup, D("5.3333"), D("0.0001"))
    b_gross, b_elim = D(24600000), D(3100000) * 2 + D(1850000)
    check("D15 case B eliminations", b_elim, D(8050000))
    check("D15 case B net benefit", b_gross - b_elim, D(16550000))
    check("D15 case B elimination rate %", b_elim / b_gross * 100, D("32.7236"), D("0.0001"))
    slipped = D(7400000)
    check("D15 case B benefit forgone in the catch-up", slipped * catchup / 12,
          D("3288888.89"), D("0.01"))
    check("D15 case B on-time benefit, original", b_gross - b_elim - slipped, D(9150000))
    check("D15 case B on-time %, original",
          (b_gross - b_elim - slipped) / (b_gross - b_elim) * 100, D("55.29"), D("0.01"))
    check("D15 case B on-time %, re-sequenced", D(15000000) / (b_gross - b_elim) * 100,
          D("90.63"), D("0.01"))
    check("D15 case B deferred benefit", b_gross - b_elim - D(15000000), D(1550000))
    check("D15 case B improvement from re-sequencing",
          D(15000000) - (b_gross - b_elim - slipped), D(5850000))
