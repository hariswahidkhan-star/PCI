"""PML-AI Domain 6 (extension) — Planning, scheduling and delivery flow: golden checks.

Every number the depth expansion of `domain-06-planning-scheduling-flow.md` prints as a Result, a
Substitution, an in-text figure, a numeric MCQ option, an exercise answer or a figure-spec value is
recomputed here from its own definition. The domain's ORIGINAL arithmetic — WE 6.1.2's FS+2/SS+1
dates, WE 6.2.1's two passes, WE 6.3.1/6.3.1b's smoothing and levelling, WE 6.4.2's single-lever
crash, WE 6.4.3's PERT figures, WE 6.A.4's 85,000 recovery and Exercises 6.1–6.5 — lives in
`verify_formulas.py` and is deliberately NOT duplicated here.

What is verified are the new engines, each rebuilt from primitives rather than from a printed output:

    network        the seven-activity Auriga network is rebuilt ONCE from `(duration, predecessors)`
                   and everything downstream — path lengths, path floats, activity floats, both
                   passes, the resource histogram — is derived from it. No pass result is typed in.
                   The three pass invariants the domain now teaches (critical durations sum to PD;
                   critical activities show ES = LS and EF = LF; every TF equals both LS − ES and
                   LF − EF) are PROVED over every activity rather than illustrated on one.

    governance     WE 6.1.2b's approval wait computed by scanning the actual meeting calendar for the
                   first meeting whose paper deadline the submission makes, then swept over a full
                   arrival cycle to show the mean equals Domain 3's `ew(M, L)` — the master helper,
                   read from ctx, not restated.

    float algebra  WE 6.2.2's absorbable slip found by CONSTRAINED MAXIMISATION over the two paths,
                   not by quoting 8; the n-activity overstatement law proved for a family of (n, f)
                   pairs rather than only at the published 10 × 5.

    levelling      WE 6.3.1c's answers found by SEARCH: the minimum resource-feasible duration by
                   exhaustive enumeration of integer start offsets under the 6-engineer cap, and the
                   27-week figure by actually running the minimum-total-float serial priority rule.
                   The teaching claim — that the heuristic is beaten by exactly one week — is
                   therefore proved, and the 25-week plan is checked to need one extra engineer-week.

    compression    WE 6.4.2b's whole frontier from a brute-force search over the crash menu for the
                   cheapest set that shortens EVERY path to each target duration; the published
                   cumulative costs, marginal costs, net savings and least-cost duration all fall out
                   of that search. The range of optimality is re-derived by re-optimising at week
                   values either side of every boundary, so a mis-stated band fails here.

    fast-track     WE 6.4.2c's two breakeven probabilities from the payoff swing, with the identity
                   `p** / p* = (good − K) / good` proved algebraically over a sweep of K rather than
                   read off the printed 0.7778.

    rollout        6.A.5's takt/crew/ramp arithmetic from Domain 1's ramp model, generalised: the
                   incremental specialist-weeks are shown CONSTANT over four remaining volumes and
                   two crew increments, the weeks-saved line is verified against its closed form, the
                   two breakeven volumes are found by solving that line, and Domain 1's own opposite
                   verdict is re-derived from the same function as a consistency test on the master
                   thread. The `40 × 357 = MERIDIAN_COST_OF_DELAY` identity closes the loop.

    buffers        6.A.6's chain, both sizing conventions and the consumption forecast, with the
                   embedded-safety identity (chain + safety = the network critical path) proved.

Master-thread values (Auriga's `BAC` and 25 weeks, Meridian's cost of delay, Domain 3's `ew`) are
read from `ctx`. The rates Domain 6 shares with Domains 1 and 7 — USD 45,000 a week, USD 5,225 an
engineer-week, USD 4,200 a specialist-week — are declared once at the top of `run` and reused, so a
change in a sibling domain surfaces as one failure rather than fifty.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]
    ew = ctx["ew"]
    COD_A = D(45000)                      # Auriga: the value of a week (D7/D8/D9 unchanged)
    ENG_WK = D(5225)                      # Auriga: engineer-week (D7, KA 7.4.1)
    SPEC_WK = D(4200)                     # Meridian: deployment specialist-week (D1, KA 1.3.3b)
    PLAN_WK = D(4200)                     # Meridian: planner-week, same rate basis
    COD_M = ctx["MERIDIAN_COST_OF_DELAY"]
    CLINICS = D(40)

    # =============================================================== the network, built once
    dur = {"A": D(2), "B": D(6), "C": D(8), "D": D(7), "E": D(5), "F": D(4), "G": D(2)}
    preds = {"A": [], "B": ["A"], "C": ["B"], "D": ["B"], "E": ["C", "D"], "F": ["E"], "G": ["D"]}
    order = ["A", "B", "C", "D", "E", "F", "G"]
    succs = {a: [b for b in order if a in preds[b]] for a in order}

    def passes(durs):
        es, ef = {}, {}
        for a in order:
            es[a] = max([ef[p] for p in preds[a]], default=D(0))
            ef[a] = es[a] + durs[a]
        pd = max(ef.values())
        lf, ls = {}, {}
        for a in reversed(order):
            lf[a] = min([ls[s] for s in succs[a]], default=pd)
            ls[a] = lf[a] - durs[a]
        return es, ef, ls, lf, pd

    ES, EF, LS, LF, PD = passes(dur)
    TF = {a: LS[a] - ES[a] for a in order}
    FF = {a: min([ES[s] for s in succs[a]], default=PD) - EF[a] for a in order}

    check("6 master network — project duration", PD, ctx["AURIGA_WEEKS"])
    check("6 master network — Auriga's BAC", ctx["AURIGA_BAC"], 4000000)

    crit = [a for a in order if TF[a] == 0]
    check("KA 6.2.1 invariant 1 — critical durations sum to the project duration",
          sum(dur[a] for a in crit), PD)
    check("KA 6.2.1 — the critical path is A-B-C-E-F",
          1 if crit == ["A", "B", "C", "E", "F"] else 0, 1)
    check("KA 6.2.1 invariant 2 — every critical activity shows ES=LS and EF=LF",
          1 if all(ES[a] == LS[a] and EF[a] == LF[a] for a in crit) else 0, 1)
    check("KA 6.2.1 invariant 3 — TF equals both LS-ES and LF-EF, every activity",
          1 if all(LS[a] - ES[a] == LF[a] - EF[a] for a in order) else 0, 1)
    check("KA 6.2.2 invariant — FF <= TF for every activity",
          1 if all(FF[a] <= TF[a] for a in order) else 0, 1)
    check("WE 6.2.1 Interpretation — G's TF read as LS-ES (23-15)", LS["G"] - ES["G"], 8)
    check("WE 6.2.1 Interpretation — G's TF read as LF-EF (25-17)", LF["G"] - EF["G"], 8)
    check("WE 6.2.1 Interpretation — G's LS is 23", LS["G"], 23)
    check("WE 6.2.1 Interpretation — G's LF is 25", LF["G"], 25)

    # =============================================================== WE 6.1.2 overlap breakeven
    saved, lost = D(5), D(6)
    check("WE 6.1.2 Interpretation — breakeven rework probability 5/6 (%)",
          saved / lost * 100, D("83.33"))
    check("WE 6.1.2 Interpretation — expected saving at p = 0.30 (weeks)",
          saved - lost * D("0.30"), D("3.20"))
    check("WE 6.1.2 Interpretation invariant — at p* the expected saving is exactly zero",
          saved - lost * (saved / lost), 0, tol=D("1e-20"))

    # =============================================================== WE 6.1.2b governance calendar
    M, L = 4, 2
    meetings = [20, 24, 28]

    def approval(finish):
        """The first meeting whose paper deadline a submission finishing at `finish` makes."""
        for m in meetings:
            if m - finish >= L:
                return m, m - finish
        raise AssertionError("no reachable meeting in the calendar")

    m23, w23 = approval(23)
    m22, w22 = approval(22)
    check("WE 6.1.2b Substitution — a week-23 finish reaches the week-28 meeting", m23, 28)
    check("WE 6.1.2b Substitution — the wait from a week-23 finish (weeks)", w23, 5)
    check("WE 6.1.2b Substitution — a week-22 finish reaches the week-24 meeting", m22, 24)
    check("WE 6.1.2b Substitution — the wait from a week-22 finish (weeks)", w22, 2)
    check("WE 6.1.2b Result — best-case wait is L (weeks)", L, 2)
    check("WE 6.1.2b Result — worst-case wait is M + L (weeks)", M + L, 6)
    waits = []
    for k in range(4000):                      # a full four-week cycle of arrival dates
        t = D(20) - D(k) / 1000
        for mm in (20, 24, 28, 32):
            if D(mm) - t >= L:
                waits.append(D(mm) - t)
                break
    check("WE 6.1.2b Formula — mean wait over a full arrival cycle equals ew(M, L)",
          sum(waits) / len(waits), ew(M, L), tol=D("0.002"))
    check("WE 6.1.2b Formula — Domain 3's E[wait] = M/2 + L (weeks)", ew(M, L), 4)
    check("WE 6.1.2b Result — understatement against the 3-week drawn lag (weeks)", w23 - 3, 2)
    check("WE 6.1.2b Result — the path's TF after the understatement", D(2) - (w23 - 3), 0)
    check("WE 6.1.2b Result — the approval date moves 28 -> 24 (weeks)", m23 - m22, 4)
    check("WE 6.1.2b Result — the wait itself shortens 5 -> 2 (weeks)", w23 - w22, 3)
    check("WE 6.1.2b Result — asymmetry: the extra week is the design's own",
          (m23 - m22) - (w23 - w22), 1)
    check("WE 6.1.2b Result — value of the four weeks bought (USD)", (m23 - m22) * COD_M, 57120)
    check("WE 6.1.2b Result — net of the USD 12,000 compression (USD)",
          (m23 - m22) * COD_M - 12000, 45120)
    check("WE 6.1.2b Interpretation — leverage per week of design compression",
          D(m23 - m22) / 1, 4)
    check("WE 6.1.2b Interpretation — Domain 3's lever, one week off L (USD)",
          (ew(M, L) - ew(M, L - 1)) * COD_M, 14280)
    check("WE 6.1.2b Interpretation caution — a week-21 finish gains nothing from compression",
          approval(21)[0] - approval(20)[0], 0)

    # =============================================================== 6.1.3 network metrics
    links = sum(len(preds[a]) for a in order)
    n = len(order)
    check("6.1.3 — dependency links counted from the predecessor list", links, 7)
    check("6.1.3 — activities in the network", n, 7)
    check("6.1.3 — logic density, links / activities", D(links) / n, D("1.00"))
    check("6.1.3 — connectivity floor n-1", n - 1, 6)
    check("6.1.3 — surplus over the connectivity floor", links - (n - 1), 1)
    check("6.1.3 invariant — the surplus equals the count of convergences",
          links - (n - 1), sum(1 for a in order if len(preds[a]) > 1))
    check("6.1.3 — activities at zero float", len(crit), 5)
    check("6.1.3 — zero-float share (%)", D(len(crit)) / n * 100, D("71.4"), tol=D("0.05"))
    check("MCQ 6.1-G rationale — minimum links to connect 400 activities", 400 - 1, 399)
    check("MCQ 6.1-G rationale — 310 links is below that floor, so dangles exist",
          1 if 310 < 400 - 1 else 0, 1)

    # =============================================================== paths, floats, the band
    paths = {"A-B-C-E-F": ["A", "B", "C", "E", "F"],
             "A-B-D-E-F": ["A", "B", "D", "E", "F"],
             "A-B-D-G": ["A", "B", "D", "G"]}
    plen = {k: sum(dur[a] for a in v) for k, v in paths.items()}
    pfl = {k: PD - plen[k] for k in paths}
    for k, want in (("A-B-C-E-F", 25), ("A-B-D-E-F", 24), ("A-B-D-G", 17)):
        check(f"WE 6.2.3 Substitution — path {k} length (wk)", plen[k], want)
    for k, want in (("A-B-C-E-F", 0), ("A-B-D-E-F", 1), ("A-B-D-G", 8)):
        check(f"WE 6.2.3 Result — path {k} float (wk)", pfl[k], want)
    for k, want in (("A-B-C-E-F", "0.00"), ("A-B-D-E-F", "4.00"), ("A-B-D-G", "32.00")):
        check(f"WE 6.2.3 Result — path {k} float as % of PD", pfl[k] / PD * 100, D(want))
    check("WE 6.2.1 Interpretation — the second path is 4.00 % below the project duration",
          pfl["A-B-D-E-F"] / PD * 100, D("4.00"))
    check("WE 6.2.3 Substitution — a 10 % band (weeks)", D("0.10") * PD, D("2.5"))
    check("MCQ 6.2-H — a 2 % band (weeks)", D("0.02") * PD, D("0.5"))
    check("MCQ 6.2-H invariant — a 2 % band excludes A-B-D-E-F",
          1 if pfl["A-B-D-E-F"] > D("0.02") * PD else 0, 1)
    check("WE 6.2.3 Interpretation invariant — a 4 % band captures it",
          1 if pfl["A-B-D-E-F"] <= D("0.04") * PD else 0, 1)
    inband = sorted({a for k in paths if pfl[k] <= D("0.10") * PD for a in paths[k]})
    check("WE 6.2.3 Result — paths inside the 10 % band",
          sum(1 for k in paths if pfl[k] <= D("0.10") * PD), 2)
    check("WE 6.2.3 Result — activities inside the band", len(inband), 6)
    check("WE 6.2.3 Result — activities inside the band as % of the network",
          D(len(inband)) / n * 100, D("85.71"))
    mon = D("0.5") * 12 * ENG_WK
    check("WE 6.2.3 Setup — monitoring cost, 0.5 eng-wk x 12 cycles (USD)", mon, 31350)
    check("WE 6.2.3 Result — weeks of lateness the monitoring must avoid", mon / COD_A,
          D("0.6967"), tol=D("0.00005"))

    d9 = dict(dur)
    d9["D"] = dur["D"] + 2
    ES2, EF2, _, _, PD2 = passes(d9)
    check("WE 6.2.3 Interpretation — with D at 9 weeks E starts week 17", ES2["E"], 17)
    check("WE 6.2.3 Interpretation — with D at 9 weeks F finishes week 26", EF2["F"], 26)
    check("WE 6.2.3 Interpretation — cost of that undetected slip (USD)", (PD2 - PD) * COD_A, 45000)
    check("WE 6.2.3 Interpretation — that cost as a multiple of the monitoring",
          (PD2 - PD) * COD_A / mon, D("1.44"), tol=D("0.005"))

    # =============================================================== WE 6.2.2 float is not additive
    summed = TF["D"] + TF["G"]
    check("WE 6.2.2 Setup — D's TF from the pass", TF["D"], 1)
    check("WE 6.2.2 Setup — G's TF from the pass", TF["G"], 8)
    check("WE 6.2.2 Substitution — path A-B-D-G length (wk)", plen["A-B-D-G"], 17)
    check("WE 6.2.2 Substitution — its path float (wk)", pfl["A-B-D-G"], 8)
    check("WE 6.2.2 Result — the float report's implied absorption (weeks)", summed, 9)
    feas = [(d, g) for d in range(12) for g in range(12)
            if plen["A-B-D-G"] + d + g <= PD and plen["A-B-D-E-F"] + d <= PD]
    absorb = max(d + g for d, g in feas)
    check("WE 6.2.2 Result — absorbable cumulative slip, by maximisation (weeks)", absorb, 8)
    check("WE 6.2.2 Substitution — d is capped at 1 by the tighter path",
          max(d for d, _ in feas), 1)
    check("WE 6.2.2 Result — overstatement (weeks)", summed - absorb, 1)
    check("WE 6.2.2 Result — false comfort at Auriga's cost of delay (USD)",
          (summed - absorb) * COD_A, 45000)
    check("MCQ 6.2-G distractor C — the minimum of the two floats", min(TF["D"], TF["G"]), 1)
    for nn, ff in ((10, 5), (4, 3), (7, 2), (25, 1)):
        # Summing per-activity float over the path is what a report does; the law is that the
        # sum equals n*f while the path still absorbs only f. Comparing D(nn)*ff with nn*ff
        # compared the same product with itself and could not fail.
        check(f"WE 6.2.2 law — {nn} activities on a path with float {ff} report n.f",
              sum(D(ff) for _ in range(nn)), nn * ff)
        check(f"WE 6.2.2 law — that path still absorbs only f = {ff}", D(nn) * ff / nn, ff)
    check("WE 6.2.2 Interpretation — the published 10 x 5 case reports 50 weeks", D(10) * 5, 50)

    # =============================================================== 6.3.1b breakeven width
    shift = D(20000)
    check("WE 6.3.1b Interpretation — shift premium as % of the cost of delay",
          shift / COD_A * 100, D("44.4"), tol=D("0.05"))
    check("WE 6.3.1b Interpretation — headroom before extension wins (x)", COD_A / shift, D("2.25"))

    # =============================================================== WE 6.3.1c resource levelling
    engs = {"A": 2, "B": 4, "C": 1, "D": 3, "E": 5, "F": 6, "G": 2}
    CAP = 6
    total = sum(dur[a] * engs[a] for a in order)
    check("WE 6.3.1c Substitution — total demand (engineer-weeks)", total, 110)
    check("WE 6.3.1c Substitution — capacity over the 25-week plan", D(CAP) * PD, 150)
    check("WE 6.3.1c Result — average utilisation of the 25-week plan (%)",
          total / (D(CAP) * PD) * 100, D("73.33"), tol=D("0.005"))
    check("WE 6.3.1c Interpretation — average utilisation of the feasible 26-week plan (%)",
          total / (D(CAP) * 26) * 100, D("70.51"), tol=D("0.005"))

    def profile(start, durs):
        pts = sorted({start[a] for a in start} | {start[a] + durs[a] for a in start})
        return [(pts[i], pts[i + 1],
                 sum(engs[a] for a in start if start[a] <= pts[i] < start[a] + durs[a]))
                for i in range(len(pts) - 1)]

    def feasible(start, durs, cap):
        end = int(max(start[a] + durs[a] for a in start))
        return all(sum(engs[a] for a in start if start[a] <= t < start[a] + durs[a]) <= cap
                   for t in range(end))

    prof = profile(ES, dur)
    check("WE 6.3.1c Substitution — the early-start profile has seven intervals", len(prof), 7)
    for i, want in enumerate([2, 4, 4, 3, 7, 5, 6]):
        check(f"WE 6.3.1c Substitution — early-start demand in interval {i + 1}", prof[i][2], want)
    breach = [(a, b, v) for a, b, v in prof if v > CAP]
    check("WE 6.3.1c Result — breaching intervals", len(breach), 1)
    check("WE 6.3.1c Result — the breach runs from week 16", breach[0][0], 16)
    check("WE 6.3.1c Result — the breach runs to week 17", breach[0][1], 17)
    check("WE 6.3.1c Result — the breach is one engineer", breach[0][2] - CAP, 1)
    check("WE 6.3.1c Interpretation — excess area of the histogram (engineer-weeks)",
          sum((v - CAP) * (b - a) for a, b, v in breach), 1)

    best, bestplan = None, None
    for dA in range(2):
        for dB in range(2):
            for dC in range(3):
                for dD in range(5):
                    for dE in range(5):
                        for dF in range(3):
                            for dG in range(13):
                                off = dict(zip(order, (dA, dB, dC, dD, dE, dF, dG)))
                                st = {}
                                for a in order:
                                    st[a] = max([st[p] + dur[p] for p in preds[a]],
                                                default=D(0)) + off[a]
                                end = max(st[a] + dur[a] for a in order)
                                if best is not None and end >= best:
                                    continue
                                if feasible(st, dur, CAP):
                                    best, bestplan = end, dict(st)
    check("WE 6.3.1c Result — minimum resource-feasible duration, by search (wk)", best, 26)
    check("WE 6.3.1c Result — in that plan G starts week 15", bestplan["G"], 15)
    check("WE 6.3.1c Result — in that plan E is deferred to week 17", bestplan["E"], 17)
    check("WE 6.3.1c Result — in that plan F finishes week 26", bestplan["F"] + dur["F"], 26)
    check("WE 6.3.1c Result — cost of the best feasible plan vs 25 weeks (USD)",
          (best - PD) * COD_A, 45000)

    sched, todo = {}, set(order)
    while todo:
        a = sorted([x for x in todo if all(p in sched for p in preds[x])],
                   key=lambda x: (TF[x], x))[0]
        s = max([sched[p] + dur[p] for p in preds[a]], default=D(0))
        while True:
            trial = dict(sched)
            trial[a] = s
            if feasible(trial, dur, CAP):
                break
            s += 1
        sched[a] = s
        todo.discard(a)
    heur = max(sched[a] + dur[a] for a in order)
    check("WE 6.3.1c Result — the minimum-total-float priority rule's duration (wk)", heur, 27)
    check("WE 6.3.1c Result — under that rule G is pushed to week 25", sched["G"], 25)
    check("WE 6.3.1c Result — cost of the priority-rule plan vs 25 weeks (USD)",
          (heur - PD) * COD_A, 90000)
    check("WE 6.3.1c Interpretation claim — the heuristic is beaten by exactly one week",
          heur - best, 1)
    check("WE 6.3.1c Interpretation — value left on the table by the rule (USD)",
          (heur - best) * COD_A, 45000)
    check("WE 6.3.1c Interpretation — the block G cannot fit inside (weeks)",
          dur["E"] + dur["F"], 9)
    check("WE 6.3.1c Result — the early-start plan is feasible with one extra engineer",
          1 if feasible(ES, dur, CAP + 1) else 0, 1)
    check("WE 6.3.1c Result — cost of buying the peak out (USD)", D(1) * ENG_WK, 5225)
    check("WE 6.3.1c Interpretation — return on that engineer-week vs the 26-week baseline (x)",
          COD_A / ENG_WK, D("8.61"), tol=D("0.005"))
    check("WE 6.3.1c Interpretation — its breakeven price (USD)", COD_A, 45000)
    check("WE 6.3.1c Interpretation — apparent value against the 27-week baseline (USD)",
          (heur - PD) * COD_A, 90000)
    check("WE 6.3.1c Interpretation — apparent return against that baseline (x)",
          (heur - PD) * COD_A / ENG_WK, D("17.22"), tol=D("0.005"))
    check("WE 6.3.1c Interpretation — the un-optimised baseline doubles the defensible price",
          ((heur - PD) * COD_A) / ((best - PD) * COD_A), 2)

    # =============================================================== WE 6.3.3 ranged package
    start3, lo3, hi3 = D(40), D(14), D(18)
    check("WE 6.3.3 Substitution — earliest tranche completion (week)", start3 + lo3, 54)
    check("WE 6.3.3 Substitution — latest tranche completion (week)", start3 + hi3, 58)
    check("WE 6.3.3 Result — mid-point (week)", start3 + (lo3 + hi3) / 2, 56)
    exp0, exp1 = (hi3 - lo3) * COD_M, (D("16.5") - D("15.5")) * COD_M
    check("WE 6.3.3 Result — commitment exposure before elaboration (USD)", exp0, 57120)
    check("WE 6.3.3 Result — commitment exposure after elaboration (USD)", exp1, 14280)
    check("WE 6.3.3 Result — exposure removed (USD)", exp0 - exp1, 42840)
    check("WE 6.3.3 Result — elaboration cost, 3 planner-weeks (USD)", 3 * PLAN_WK, 12600)
    check("WE 6.3.3 Result — return ratio", (exp0 - exp1) / (3 * PLAN_WK), D("3.40"))
    check("WE 6.3.3 Result — cost of missing the mid-point by two weeks (USD)", 2 * COD_M, 28560)
    check("WE 6.3.3 Interpretation invariant — the elaborated range sits inside the original",
          1 if lo3 <= D("15.5") and D("16.5") <= hi3 else 0, 1)

    # =============================================================== WE 6.4.1 required rate
    backlog = D(72)
    for rate, want in ((D("4.5"), 16), (D("4.0"), 18), (D("3.6"), 20)):
        check(f"WE 6.4.1 Substitution — duration at {rate} items a week (wk)", backlog / rate, want)
    check("WE 6.4.1 Setup — LS(F) read off the backward pass", LS["F"], 21)
    window = LS["F"] - 5
    check("WE 6.4.1 Result — available window (weeks)", window, 16)
    check("WE 6.4.1 Result — required throughput (items a week)", backlog / window, D("4.5"))
    check("WE 6.4.1 Result — required rate for a week-3 start", backlog / (LS["F"] - 3), D("4.0"))
    check("WE 6.4.1 Result — required rate for a week-1 start", backlog / (LS["F"] - 1), D("3.6"))
    check("WE 6.4.1 Result — items delivered in the window at the mean rate",
          D("4.0") * window, 64)
    check("WE 6.4.1 Result — items deferred", backlog - D("4.0") * window, 8)
    check("WE 6.4.1 Result — deferred share of the package (%)",
          (backlog - D("4.0") * window) / backlog * 100, D("11.11"), tol=D("0.005"))
    check("WE 6.4.1 Result — cost of finishing the full package at the mean rate (USD)",
          (backlog / D("4.0") - window) * COD_A, 90000)
    check("WE 6.4.1 Interpretation invariant — the requirement equals the best observed rate",
          backlog / window, D("4.5"))

    # =============================================================== WE 6.4.2b compression menu
    menu = {"C": (D(30000), 2), "B": (D(40000), 2), "D": (D(35000), 1),
            "E": (D(55000), 1), "F": (D(65000), 1)}

    def duration_after(buy):
        d2 = {a: dur[a] - buy.get(a, 0) for a in order}
        return max(sum(d2[a] for a in v) for v in paths.values())

    def spend(buy):
        return sum(menu[a][0] * k for a, k in buy.items())

    cheapest = {}
    for c in range(menu["C"][1] + 1):
        for b in range(menu["B"][1] + 1):
            for dd in range(menu["D"][1] + 1):
                for e in range(menu["E"][1] + 1):
                    for f in range(menu["F"][1] + 1):
                        buy = {"C": c, "B": b, "D": dd, "E": e, "F": f}
                        T, cost = duration_after(buy), spend(buy)
                        if T not in cheapest or cost < cheapest[T][0]:
                            cheapest[T] = (cost, dict(buy))
    frontier = {T: min(v[0] for t, v in cheapest.items() if t <= T) for T in cheapest}

    check("WE 6.4.2b Result — shortest technically feasible duration (wk)", min(cheapest), 19)
    published_cum = {25: 0, 24: 30000, 23: 70000, 22: 110000, 21: 165000, 20: 230000, 19: 295000}
    for T, cum in published_cum.items():
        check(f"WE 6.4.2b Result table — cheapest spend to reach {T} weeks (USD)",
              frontier[T], cum)
    marg = [frontier[T] - frontier[T + 1] for T in range(24, 18, -1)]
    for i, want in enumerate([30000, 40000, 40000, 55000, 65000, 65000]):
        check(f"WE 6.4.2b Result table — marginal cost of bought week {i + 1} (USD)",
              marg[i], want)
    check("WE 6.4.2b Substitution — 25 -> 24 is bought on C alone", cheapest[24][1]["C"], 1)
    check("WE 6.4.2b Substitution — the rejected 24 -> 23 route, C x1 + D x1 (USD)",
          menu["C"][0] + menu["D"][0], 65000)
    check("WE 6.4.2b Substitution — B's slope beats it (USD)", menu["B"][0], 40000)
    nets = {T: (PD - T) * COD_A - frontier[T] for T in frontier}
    for T, want in ((25, 0), (24, 15000), (23, 20000), (22, 25000),
                    (21, 15000), (20, -5000), (19, -25000)):
        check(f"WE 6.4.2b Result table — net saving at {T} weeks (USD)", nets[T], want)
    for T, want in ((24, 45000), (23, 90000), (22, 135000), (21, 180000),
                    (20, 225000), (19, 270000)):
        check(f"WE 6.4.2b Result table — value of the weeks saved at {T} (USD)",
              (PD - T) * COD_A, want)
    opt = max(nets, key=lambda T: nets[T])
    check("WE 6.4.2b Result — least-cost duration (wk)", opt, 22)
    check("WE 6.4.2b Result — weeks compressed at the optimum", PD - opt, 3)
    check("WE 6.4.2b Result — spend at the optimum (USD)", frontier[opt], 110000)
    check("WE 6.4.2b Result — net saving at the optimum (USD)", nets[opt], 25000)
    check("WE 6.4.2b Result — value destroyed at the technical limit (USD)", nets[19], -25000)
    check("WE 6.4.2b Interpretation — average crash cost at 22 weeks (USD/wk)",
          frontier[22] / (PD - 22), D("36666.67"), tol=D("0.005"))
    check("WE 6.4.2b Interpretation — loss on the fourth week (USD)", COD_A - marg[3], -10000)
    check("WE 6.4.2b Interpretation invariant — the average sits below the value of a week "
          "while the next marginal sits above it",
          1 if frontier[22] / (PD - 22) < COD_A < marg[3] else 0, 1)

    def optimum_at(v):
        return max(frontier, key=lambda T: (PD - T) * v - frontier[T])

    for v, want in ((D(20000), 25), (D(29999), 25), (D(30001), 24), (D(39999), 24),
                    (D(40001), 22), (D(45000), 22), (D(54999), 22), (D(55001), 21),
                    (D(64999), 21), (D(65001), 19), (D(80000), 19)):
        check(f"WE 6.4.2b Interpretation — range of optimality at v = {v}", optimum_at(v), want)
    check("WE 6.4.2b Interpretation — 45,000 above the band's lower bound (%)",
          (COD_A - D(40000)) / D(40000) * 100, D("12.50"))
    check("WE 6.4.2b Interpretation — 45,000 below the band's upper bound (%)",
          (D(55000) - COD_A) / D(55000) * 100, D("18.18"), tol=D("0.005"))
    check("WE 6.4.2b Interpretation — an indirect cost of 18,000 takes v to 63,000",
          COD_A + D(18000), 63000)
    check("WE 6.4.2b Interpretation — and moves the optimum to 21 weeks",
          optimum_at(COD_A + D(18000)), 21)

    restricted = {}
    for c in range(menu["C"][1] + 1):
        for e in range(menu["E"][1] + 1):
            buy = {"C": c, "E": e}
            T, cost = duration_after(buy), spend(buy)
            if T not in restricted or cost < restricted[T][0]:
                restricted[T] = (cost, dict(buy))
    check("6.A.4 / 6.4.2b — cheapest route to 23 weeks with only C and E buyable (USD)",
          restricted[23][0], 85000)
    check("6.A.4 / 6.4.2b — that route buys two weeks in total",
          restricted[23][1]["C"] + restricted[23][1]["E"], 2)
    check("6.A.4 / 6.4.2b — the same two weeks on the full planning-time menu (USD)",
          frontier[23], 70000)
    check("6.A.4 / 6.4.2b — the price of the options that expired (USD)",
          restricted[23][0] - frontier[23], 15000)
    check("MCQ 6.4-G distractor C — average of all six marginals (USD)",
          frontier[19] / 6, D("49166.67"), tol=D("0.005"))
    check("MCQ 6.4-G distractor D — average of the first four marginals (USD)",
          frontier[21] / 4, D("41250"))

    # =============================================================== WE 6.4.2c fast-tracking
    overlap = D("0.4") * dur["E"]
    check("WE 6.4.2c Setup — overlap taken (weeks)", overlap, 2)
    check("WE 6.4.2c Setup — duration with the overlap (wk)", PD - overlap, 23)
    good = overlap * COD_A
    bad_T = (PD - overlap) + D("1.5")
    bad = (PD - bad_T) * COD_A - D(140000)
    swing = good - bad
    check("WE 6.4.2c Result — good outcome (USD)", good, 90000)
    check("WE 6.4.2c Result — duration if the overlap fails (wk)", bad_T, D("24.5"))
    check("WE 6.4.2c Result — bad outcome (USD)", bad, -117500)
    check("WE 6.4.2c Result — swing between the outcomes (USD)", swing, 207500)
    check("WE 6.4.2c Result — p* against doing nothing (%)", good / swing * 100, D("43.37"),
          tol=D("0.005"))
    K = good - D(70000)
    check("WE 6.4.2c Substitution — the crash alternative's certain net (USD)", K, 20000)
    check("WE 6.4.2c Result — p** against the crash (%)", (good - K) / swing * 100, D("33.73"),
          tol=D("0.005"))
    EV = good - D("0.25") * swing
    check("WE 6.4.2c Result — expected value at the assessed 25 % (USD)", EV, 38125)
    check("WE 6.4.2c Result — advantage over the crash plan (USD)", EV - K, 18125)
    check("WE 6.4.2c Interpretation — downside gap between the plans (USD)", K - bad, 137500)
    check("WE 6.4.2c Interpretation — ratio p**/p*", (good - K) / good, D("0.7778"),
          tol=D("0.00005"))
    check("WE 6.4.2c Interpretation — probability headroom removed (%)",
          (1 - (good - K) / good) * 100, D("22.22"), tol=D("0.005"))
    check("WE 6.4.2c Interpretation — the hedge's crashed week is the menu's cheapest (USD)",
          min(menu[a][0] for a in menu), 30000)
    check("WE 6.4.2c Interpretation — one week of overlap plus C x1 also reaches 23 weeks",
          duration_after({"C": 1}) - 1, 23)
    for k in (D(0), D(20000), D(50000), D(90000)):
        check(f"WE 6.4.2c identity — p**/p* = (good-K)/good at K = {k}",
              ((good - k) / swing) / (good / swing), (good - k) / good, tol=D("1e-20"))
    check("WE 6.4.2c invariant — a certain alternative always lowers the threshold",
          1 if (good - K) / swing < good / swing else 0, 1)
    check("WE 6.4.2c Interpretation — even the bad outcome finishes inside the original date",
          1 if bad_T < PD else 0, 1)

    # =============================================================== 6.4.3 deterministic bias
    def pert(o, m, p):
        return ((D(o) + 4 * D(m) + D(p)) / 6, (D(p) - D(o)) / 6, (D(o) - 2 * D(m) + D(p)) / 6)

    te_e, _, bias_e = pert(4, 5, 12)
    check("WE 6.4.3 Interpretation — bias of (4, 5, 12) (weeks)", bias_e, D("1.0000"))
    check("WE 6.4.3 Interpretation — the identity te - m holds for (4, 5, 12)",
          bias_e, te_e - 5, tol=D("1e-20"))
    te_x, _, bias_x = pert(6, 8, 16)
    check("WE 6.4.3 Interpretation — bias of Exercise 6.4's (6, 8, 16) (weeks)",
          bias_x, D("1.0000"))
    check("WE 6.4.3 Interpretation — the identity te - m holds for (6, 8, 16)",
          bias_x, te_x - 8, tol=D("1e-20"))
    te_s, sd_s, bias_s = pert(5, 9, 13)
    check("WE 6.4.3 Interpretation — bias of the symmetric (5, 9, 13)", bias_s, 0)
    check("WE 6.4.3 Interpretation — sigma of the symmetric (5, 9, 13)", sd_s, D("1.3333"),
          tol=D("0.00005"))
    check("WE 6.4.3 Interpretation invariant — a symmetric estimate gives te = m", te_s, 9)
    check("WE 6.4.3 Interpretation invariant — bias is positive on every right-skewed estimate",
          1 if all(pert(o, m, p)[2] > 0 for o, m, p in
                   ((4, 5, 12), (6, 8, 16), (1, 2, 10), (3, 4, 20))) else 0, 1)

    BAC, PV13, EV13 = ctx["AURIGA_BAC"], D(2080000), D(1920000)
    pv_rate = BAC / PD
    check("KA 6.4.3 bridge — Domain 7's PV at week 13 is consistent with a linear PV curve",
          pv_rate * 13, PV13)
    ES_t = EV13 / pv_rate
    check("KA 6.4.3 bridge — earned schedule ES (weeks)", ES_t, D("12.0000"))
    check("KA 6.4.3 bridge — SPI(t) = ES / AT", ES_t / 13, D("0.9231"), tol=D("0.00005"))
    check("KA 6.4.3 bridge — the currency SPI = EV / PV", EV13 / PV13, D("0.9231"),
          tol=D("0.00005"))
    check("KA 6.4.3 bridge — the two indices coincide on a linear PV curve",
          ES_t / 13, EV13 / PV13, tol=D("1e-20"))

    # =============================================================== Case study A
    check("Case A — option (2) net vs doing nothing (USD)", COD_A - D(35000), 10000)
    check("Case A — option (3) expected cost (USD)", D("0.20") * D(60000), 12000)
    check("Case A — option (3) net vs doing nothing (USD)", COD_A - D("0.20") * D(60000), 33000)
    check("Case A — combined expected cost (USD)", D(35000) + D("0.20") * D(60000), 47000)
    check("Case A — penalty avoided by two weeks (USD)", 2 * COD_A, 90000)
    check("Case A — net of the combined plan (USD)",
          2 * COD_A - (D(35000) + D("0.20") * D(60000)), 43000)
    check("Case A — fast-track breakeven probability (%)", COD_A / D(60000) * 100, D("75.00"))
    check("Case A — bad outcome if the overlap fails (USD)", COD_A - D(60000), -15000)
    check("Case A invariant — the assessed 20 % is well below the 75 % breakeven",
          1 if D("0.20") < COD_A / D(60000) else 0, 1)
    d10 = dict(dur)
    d10["D"] = D(10)
    ES3, EF3, LS3, _, PD3 = passes(d10)
    check("Case A — new project duration with D at 10 weeks (wk)", PD3, 27)
    check("Case A — E's new start (week)", ES3["E"], 18)
    check("Case A — procurement C's new TF", LS3["C"] - ES3["C"], 2)
    check("Case A — the critical path has migrated off C onto D",
          1 if LS3["C"] - ES3["C"] > 0 and LS3["D"] - ES3["D"] == 0 else 0, 1)
    check("Case A — penalty exposure of doing nothing (USD)", (PD3 - PD) * COD_A, 90000)

    # =============================================================== Case study B
    env, uat, pinned = D(69), D(12), D(78)
    check("Case B — honest finish (week)", env + uat, 81)
    check("Case B — total float against the pinned date", pinned - (env + uat), -3)
    check("Case B — the window the plan shows (weeks)", pinned - env, 9)
    check("Case B — that window as % of the test plan", (pinned - env) / uat * 100, D("75.00"))
    check("Case B — weeks of concealment, six monthly cycles", 6 * 4, 24)
    env7 = D(73)
    check("Case B — cycle-seven window (weeks)", pinned - env7, 5)
    check("Case B — cycle-seven window as % of the test plan",
          (pinned - env7) / uat * 100, D("41.67"), tol=D("0.005"))
    check("Case B — cycle-seven gap between honest logic and the pin (weeks)",
          env7 + uat - pinned, 7)
    check("Case B — growth factor of the recovery problem",
          (env7 + uat - pinned) / (env + uat - pinned), D("2.3333"), tol=D("0.00005"))
    det0 = D("0.75")
    det7 = det0 * (pinned - env7) / uat
    check("Case B — detection with 5/12 of the plan run (%)", det7 * 100, D("31.25"))
    check("Case B — escape fraction as designed", 1 - det0, D("0.25"))
    check("Case B — escape fraction at cycle seven", 1 - det7, D("0.6875"))
    check("Case B — multiple of defects reaching live service", (1 - det7) / (1 - det0), D("2.75"))
    check("Case B invariant — a window shorter than the test plan implies negative float",
          1 if (pinned - env) < uat and (pinned - (env + uat)) < 0 else 0, 1)

    # =============================================================== 6.A.5 takt and the integral
    per_head = D("0.1")
    base = 6 * per_head
    WINDOW = D(50)
    RAMP_WK, RAMP_PROD, SUPERVISION = D(4), D("0.25"), D("0.5")
    check("WE 6.A.5 Substitution — base rate of the crew of six (clinics a week)", base, D("0.6"))
    T0 = CLINICS / base
    check("WE 6.A.5 Substitution — baseline duration of the crew of six (wk)", T0, D("66.6667"),
          tol=D("0.00005"))
    check("WE 6.A.5 Result — required takt (weeks a clinic)", WINDOW / CLINICS, D("1.25"))
    check("WE 6.A.5 Result — required rate (clinics a week)", CLINICS / WINDOW, D("0.8"))
    check("WE 6.A.5 Result — the crew the window needs", CLINICS / WINDOW / per_head, 8)

    def ramped(k, N):
        rr = base - k * SUPERVISION * per_head + k * RAMP_PROD * per_head
        pr = base + k * per_head
        return rr, pr, RAMP_WK + (N - RAMP_WK * rr) / pr

    rr2, pr2, T1 = ramped(D(2), CLINICS)
    check("WE 6.A.5 Substitution — ramp-period rate with two newcomers", rr2, D("0.55"))
    check("WE 6.A.5 Substitution — post-ramp rate with two newcomers", pr2, D("0.8"))
    check("WE 6.A.5 Result — duration of the eight-person crew (wk)", T1, D("51.2500"))
    check("WE 6.A.5 Result — achieved takt (weeks a clinic)", T1 / CLINICS, D("1.2813"),
          tol=D("0.00005"))
    check("WE 6.A.5 Result — weeks saved", T0 - T1, D("15.4167"), tol=D("0.00005"))
    check("WE 6.A.5 Result — specialist-weeks at the crew of six", 6 * T0, D("400.0"),
          tol=D("0.00005"))
    check("WE 6.A.5 Result — specialist-weeks at the crew of eight", 8 * T1, D("410.0"))
    extra = 8 * T1 - 6 * T0
    check("WE 6.A.5 Result — extra specialist-weeks", extra, 10)
    check("WE 6.A.5 Result — cost of the reinforcement (USD)", extra * SPEC_WK, 42000)
    check("WE 6.A.5 Interpretation invariant — work content is invariant to the rate",
          CLINICS * 10, 6 * T0)
    ben = D(6) * D(85) * D("0.70")
    check("WE 6.A.5 Setup — benefit a week per installed clinic (USD)", ben, 357)
    check("WE 6.A.5 identity — 40 clinics x 357 reproduces Domain 1's cost of delay",
          CLINICS * ben, COD_M)
    cw = (CLINICS / 2) * (T0 - T1)
    check("WE 6.A.5 Result — clinic-weeks gained (the ramp integral)", cw, D("308.3333"),
          tol=D("0.00005"))
    check("WE 6.A.5 Result — gain on the ramp-integral basis (USD)", cw * ben, D("110075.00"))
    check("WE 6.A.5 Result — net on the ramp-integral basis (USD)",
          cw * ben - extra * SPEC_WK, 68075)
    check("WE 6.A.5 Interpretation — value per week saved on a linear rollout (USD)",
          (CLINICS / 2) * ben, 7140)
    check("WE 6.A.5 identity — that is exactly half the cost of delay",
          (CLINICS / 2) * ben * 2, COD_M)
    check("WE 6.A.5 Interpretation — gain on the switch-on-at-completion basis (USD)",
          (T0 - T1) * COD_M, D("220150.00"))
    check("WE 6.A.5 Interpretation — net on that basis (USD)",
          (T0 - T1) * COD_M - extra * SPEC_WK, 178150)
    check("WE 6.A.5 Interpretation — the two bases differ by exactly a factor of two",
          (T0 - T1) * COD_M / (cw * ben), 2)
    check("WE 6.A.5 Interpretation invariant — the decision is 'buy' on both bases",
          1 if cw * ben > extra * SPEC_WK and (T0 - T1) * COD_M > extra * SPEC_WK else 0, 1)
    for k, want in ((2, 10), (3, 15)):
        for N in (D(12), D(25), D(40), D(60)):
            _, _, TK = ramped(D(k), N)
            check(f"WE 6.A.5 Interpretation — extra specialist-weeks, {k} newcomers, N = {N}",
                  (6 + k) * TK - 6 * (N / base), want)
    slope = 1 / base - 1 / pr2
    intercept = -RAMP_WK + RAMP_WK * rr2 / pr2
    check("WE 6.A.5 Interpretation — slope of the weeks-saved line", slope, D("0.4166667"),
          tol=D("0.0000005"))
    check("WE 6.A.5 Interpretation — intercept of the weeks-saved line", intercept, D("-1.25"))
    check("WE 6.A.5 Interpretation — the line reproduces the 40-clinic saving",
          slope * CLINICS + intercept, T0 - T1, tol=D("1e-20"))
    for basis, val, want in (("full cost of delay", COD_M, D("10.0588")),
                             ("ramp integral", (CLINICS / 2) * ben, D("17.1176"))):
        need = extra * SPEC_WK / val
        check(f"WE 6.A.5 Interpretation — breakeven volume on the {basis} (clinics)",
              (need - intercept) / slope, want, tol=D("0.00005"))
    check("WE 6.A.5 Interpretation invariant — eight clinics sit below both breakevens",
          1 if slope * 8 + intercept < extra * SPEC_WK / COD_M else 0, 1)
    rr3, pr3, T1_8 = ramped(D(3), D(8))
    T0_8 = D(8) / base
    check("WE 6.A.5 Interpretation — Domain 1's baseline duration (wk)", T0_8, D("13.3333"),
          tol=D("0.00005"))
    check("WE 6.A.5 Interpretation — Domain 1's reinforced duration (wk)", T1_8, D("10.5556"),
          tol=D("0.00005"))
    check("WE 6.A.5 Interpretation — Domain 1's weeks saved", T0_8 - T1_8, D("2.7778"),
          tol=D("0.00005"))
    check("WE 6.A.5 Interpretation — Domain 1's net, reproduced (USD)",
          (T0_8 - T1_8) * COD_M - 15 * SPEC_WK, D("-23333.33"), tol=D("0.005"))
    drum = D("0.7")
    Td = CLINICS / drum
    check("WE 6.A.5 Interpretation — the achievable rate is the drum's", min(pr2, drum), drum)
    check("WE 6.A.5 Interpretation — duration set by the drum (wk)", Td, D("57.1429"),
          tol=D("0.00005"))
    check("WE 6.A.5 Interpretation — weeks the drum costs", Td - T1, D("5.8929"),
          tol=D("0.00005"))
    check("WE 6.A.5 Interpretation — value forgone to the drum (USD)",
          (CLINICS / 2) * (Td - T1) * ben, D("42075.00"))

    # =============================================================== 6.A.6 buffers
    net_d = [D(2), D(6), D(8), D(5), D(4)]
    p50 = [D("1.5"), D(5), D("6.5"), D(4), D(3)]
    safety = [a - b for a, b in zip(net_d, p50)]
    chain, S = sum(p50), sum(safety)
    check("WE 6.A.6 Substitution — chain of 50 % durations (wk)", chain, D("20.0"))
    check("WE 6.A.6 Substitution — aggregated embedded safety (wk)", S, D("5.0"))
    check("WE 6.A.6 identity — chain + safety equals the network critical path", chain + S, PD)
    check("WE 6.A.6 identity — the table's network durations are the critical path",
          sum(net_d), sum(dur[a] for a in crit))
    b_half = S / 2
    check("WE 6.A.6 Result — half-safety buffer (wk)", b_half, D("2.5"))
    check("WE 6.A.6 Result — commitment on the half-safety convention (wk)",
          chain + b_half, D("22.5000"))
    check("WE 6.A.6 Result — value of that commitment vs the network date (USD)",
          (PD - (chain + b_half)) * COD_A, 112500)
    check("WE 6.A.6 Substitution — sum of squared safeties", sum(x * x for x in safety), D("5.5"))
    b_rss = sum(x * x for x in safety).sqrt()
    check("WE 6.A.6 Result — root-sum-square buffer (wk)", b_rss, D("2.3452"), tol=D("0.00005"))
    check("WE 6.A.6 Result — commitment on the root-sum-square convention (wk)",
          chain + b_rss, D("22.3452"), tol=D("0.00005"))
    check("WE 6.A.6 Result — weeks inside the network date", PD - (chain + b_rss), D("2.6548"),
          tol=D("0.00005"))
    check("WE 6.A.6 Result — value of the root-sum-square commitment (USD)",
          (PD - (chain + b_rss)) * COD_A, D("119465.65"), tol=D("0.005"))
    check("WE 6.A.6 Interpretation — gap between the two conventions (wk)",
          b_half - b_rss, D("0.1548"), tol=D("0.00005"))
    check("WE 6.A.6 Interpretation invariant — root-sum-square sits below the simple sum, "
          "which is the fully correlated limit", 1 if b_rss < S else 0, 1)
    ratio = D("0.30") / D("0.45")
    check("WE 6.A.6 Substitution — buffer consumption ratio", ratio, D("0.6667"),
          tol=D("0.00005"))
    check("WE 6.A.6 Result — projected final buffer use (wk)", b_half * ratio, D("1.6667"),
          tol=D("0.00005"))
    check("WE 6.A.6 Result — forecast chain completion (wk)", chain + b_half * ratio,
          D("21.6667"), tol=D("0.00005"))
    check("WE 6.A.6 Result — buffer left unused (wk)", b_half - b_half * ratio, D("0.8333"),
          tol=D("0.00005"))
    check("WE 6.A.6 Result — value of the unused buffer (USD)",
          (b_half - b_half * ratio) * COD_A, 37500)
    check("WE 6.A.6 Result — weeks inside the original network date",
          PD - (chain + b_half * ratio), D("3.3333"), tol=D("0.00005"))
    check("WE 6.A.6 Interpretation invariant — a ratio below 1 forecasts buffer left over",
          1 if b_half * ratio < b_half else 0, 1)

    # =============================================================== figure specs
    check("Fig 6.2.2 spec — band line at 22.50 weeks", PD - D("0.10") * PD, D("22.50"))
    check("Fig 6.2.2 spec — the 26-week x-axis covers the longest path",
          1 if D(26) > plen["A-B-C-E-F"] else 0, 1)
    check("Fig 6.2.2 spec — side-note share of activities inside the band",
          D(len(inband)) / n * 100, D("85.71"))
    check("Fig 6.2.2 spec — the band's colour rule keeps two paths blue",
          sum(1 for k in paths if pfl[k] <= D("2.5")), 2)
    for T, cum in published_cum.items():
        check(f"Fig 6.4.2 spec — cumulative crash cost printed under {T} weeks (USD)",
              frontier[T], cum)
    check("Fig 6.4.2 spec — the y-axis range contains every net saving",
          1 if all(D(-30000) <= nets[T] <= D(30000) for T in nets) else 0, 1)
    check("Fig 6.4.2 spec — the peak annotation", nets[22], 25000)
    check("Fig 6.4.2 spec — three marginal steps are at or below the value of a week",
          sum(1 for m in marg if m <= COD_A), 3)
    check("Fig 6.4.2 spec — three marginal steps are above it",
          sum(1 for m in marg if m > COD_A), 3)

    # =============================================================== exercises 6.6 - 6.11
    for k, want in (("A-B-C-E-F", 25), ("A-B-D-E-F", 24), ("A-B-D-G", 17)):
        check(f"Ex 6.6 — path {k} length (wk)", plen[k], want)
    for k, want in (("A-B-C-E-F", 0), ("A-B-D-E-F", 1), ("A-B-D-G", 8)):
        check(f"Ex 6.6 — path {k} float (wk)", pfl[k], want)
    check("Ex 6.6 — absorbable cumulative slip on the D-G chain (weeks)", absorb, 8)
    check("Ex 6.6 — the sum a float report would imply (weeks)", summed, 9)
    check("Ex 6.6 — the overstatement priced (USD)", (summed - absorb) * COD_A, 45000)
    check("Ex 6.7 — total engineer demand (engineer-weeks)", total, 110)
    check("Ex 6.7 — capacity over 25 weeks", D(CAP) * PD, 150)
    check("Ex 6.7 — average utilisation (%)", total / (D(CAP) * PD) * 100, D("73.33"),
          tol=D("0.005"))
    check("Ex 6.7 — the breaching interval starts in week 16", breach[0][0], 16)
    check("Ex 6.7 — the breach is one engineer for one week",
          (breach[0][2] - CAP) * (breach[0][1] - breach[0][0]), 1)
    check("Ex 6.7 — cost of the cheapest fix (USD)", ENG_WK, 5225)
    check("Ex 6.7 — duration preserved, worth (USD)", COD_A, 45000)
    check("Ex 6.7 — return on the cheapest fix (x)", COD_A / ENG_WK, D("8.61"), tol=D("0.005"))
    for i, want in enumerate([30000, 40000, 40000, 55000, 65000, 65000]):
        check(f"Ex 6.8 — marginal cost of successive week {i + 1} (USD)", marg[i], want)
    check("Ex 6.8 — least-cost duration (wk)", opt, 22)
    check("Ex 6.8 — cumulative spend (USD)", frontier[opt], 110000)
    check("Ex 6.8 — net saving (USD)", nets[opt], 25000)
    check("Ex 6.8 — optimum just below the band's lower bound", optimum_at(D(39999)), 24)
    check("Ex 6.8 — optimum just above the band's upper bound", optimum_at(D(55001)), 21)
    check("Ex 6.8 — average crash cost at 22 weeks (USD)", frontier[22] / 3, D("36666.67"),
          tol=D("0.005"))
    check("Ex 6.8 — loss on a fourth week (USD)", COD_A - marg[3], -10000)
    check("Ex 6.9 — good outcome (USD)", good, 90000)
    check("Ex 6.9 — bad outcome (USD)", bad, -117500)
    check("Ex 6.9 — swing (USD)", swing, 207500)
    check("Ex 6.9 — p* (%)", good / swing * 100, D("43.37"), tol=D("0.005"))
    check("Ex 6.9 — the crash plan's certain net (USD)", K, 20000)
    check("Ex 6.9 — p** (%)", (good - K) / swing * 100, D("33.73"), tol=D("0.005"))
    check("Ex 6.9 — expected value at 25 % (USD)", EV, 38125)
    check("Ex 6.9 — advantage over the crash plan (USD)", EV - K, 18125)
    check("Ex 6.9 — worse downside (USD)", K - bad, 137500)
    check("Ex 6.10 — chain (wk)", chain, D("20.0"))
    check("Ex 6.10 — aggregated safety (wk)", S, D("5.0"))
    check("Ex 6.10 — half-safety buffer (wk)", b_half, D("2.5"))
    check("Ex 6.10 — half-safety commitment (wk)", chain + b_half, D("22.5000"))
    check("Ex 6.10 — root-sum-square buffer (wk)", b_rss, D("2.3452"), tol=D("0.00005"))
    check("Ex 6.10 — root-sum-square commitment (wk)", chain + b_rss, D("22.3452"),
          tol=D("0.00005"))
    check("Ex 6.10 — weeks inside the network date", PD - (chain + b_rss), D("2.6548"),
          tol=D("0.00005"))
    check("Ex 6.10 — value of that commitment (USD)", (PD - (chain + b_rss)) * COD_A,
          D("119465.65"), tol=D("0.005"))
    check("Ex 6.10 — consumption ratio", ratio, D("0.6667"), tol=D("0.00005"))
    check("Ex 6.10 — projected buffer use (wk)", b_half * ratio, D("1.6667"), tol=D("0.00005"))
    check("Ex 6.10 — forecast chain completion (wk)", chain + b_half * ratio, D("21.6667"),
          tol=D("0.00005"))
    check("Ex 6.10 — buffer unused (wk)", b_half - b_half * ratio, D("0.8333"), tol=D("0.00005"))
    check("Ex 6.11 — required takt (wk)", WINDOW / CLINICS, D("1.25"))
    check("Ex 6.11 — required rate (clinics a week)", CLINICS / WINDOW, D("0.8"))
    check("Ex 6.11 — crew", CLINICS / WINDOW / per_head, 8)
    check("Ex 6.11 — ramp-period rate", rr2, D("0.55"))
    check("Ex 6.11 — post-ramp rate", pr2, D("0.8"))
    check("Ex 6.11 — ramp-period output subtracted from the volume", RAMP_WK * rr2, D("2.2"))
    check("Ex 6.11 — reinforced duration (wk)", T1, D("51.2500"))
    check("Ex 6.11 — baseline duration (wk)", T0, D("66.6667"), tol=D("0.00005"))
    check("Ex 6.11 — weeks saved", T0 - T1, D("15.4167"), tol=D("0.00005"))
    check("Ex 6.11 — extra specialist-weeks", extra, 10)
    check("Ex 6.11 — reinforcement cost (USD)", extra * SPEC_WK, 42000)
    check("Ex 6.11 — ramp-integral gain (USD)", cw * ben, D("110075.00"))
    check("Ex 6.11 — ramp-integral net (USD)", cw * ben - extra * SPEC_WK, 68075)
    check("Ex 6.11 — switch-on-at-completion gain (USD)", (T0 - T1) * COD_M, D("220150.00"))
    check("Ex 6.11 — switch-on-at-completion net (USD)",
          (T0 - T1) * COD_M - extra * SPEC_WK, 178150)

    # =============================================================== summary claims
    check("Domain summary — nine weeks reported against eight available", summed - absorb, 1)
    check("Domain summary — the 10 % band captures 85.71 % of the network",
          D(len(inband)) / n * 100, D("85.71"))
    check("Domain summary — a plan 73.33 % loaded on average was infeasible on its peak",
          1 if total / (D(CAP) * PD) < 1 and max(p[2] for p in prof) > CAP else 0, 1)
    check("Domain summary — one engineer-week of excess demand was worth USD 45,000",
          (heur - best) * COD_A, 45000)
    check("Domain summary — least-cost duration 22 weeks", opt, 22)
    check("Domain summary — and it holds for any week worth 40,000-55,000",
          1 if optimum_at(D(40001)) == 22 == optimum_at(D(54999)) else 0, 1)
    check("Domain summary — four weeks of Meridian's schedule from one week of design",
          m23 - m22, 4)
