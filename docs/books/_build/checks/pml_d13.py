"""PML-AI Domain 13 — Agile, adaptive and hybrid delivery: golden checks.

Every number printed as a result in `domain-13-agile-adaptive-hybrid.md` is recomputed here from its
definition rather than from its printed output. Master-thread values come from ctx and are never
re-typed: `MERIDIAN_COST_OF_DELAY` (USD 14,280/week, Domain 1), `MERIDIAN_BENEFIT_70PCT`,
`MERIDIAN_BENEFIT_FULL`, and the shared helpers `ew(M, L)` for Domain 3's governance latency and
`mesh(n)` for Domain 4's pairwise interface count. Auriga's schedule value (USD 45,000/week, Domain 6
KA 6.4.2) is declared once below.

The domain has five engines, each checked from first principles:

    flow           Little's Law in all three forms, on a two-regime throughput model: T = 6W/18 below
                   the capacity point W* = 18 and T = 6(1 - 0.02(W - 18)) above it. Cycle time is
                   derived as W/T at every printed W, and the reciprocal reading T = W/C is asserted
                   to agree, since the manuscript relies on the two being the same operation. Flow
                   efficiency, the backlog duration at each throughput and the priced difference are
                   computed, and the model's zero-throughput WIP is solved for (68) rather than
                   asserted, because the manuscript prints it as a calibration warning.
    sequencing     delay-cost density = CoD / effort, and the total delay cost of a sequence as
                   sum(CoD_i x completion week_i). All four printed orderings are built by SORTING on
                   their stated key rather than by re-typing the order, so a mis-stated order in the
                   manuscript fails here. The epic CoDs are asserted to sum EXACTLY to Meridian's
                   programme cost of delay (the apportionment claim), the big-bang total is computed
                   as CoD x 34, and the E2 breakeven CoD is solved from the density that would tie it
                   with the second-ranked epic.
    forecasting    four-week rolling totals built from the printed history (whose sum and mean are
                   themselves checked), the min/median/max sustained rate taken from the sorted
                   windows, net drain = gross - arrivals, and the range, the naive figure, the
                   arrival understatement and the required gross rate for a target date. The
                   percentage gaps to the best observed rate and to the mean are computed, not typed.
    governance     blocked WIP = share x throughput x ew(M, L) (Little's Law on the blocked
                   sub-population), the cycle-time decomposition Cu = C - s x E[wait] with its blocked
                   counterpart and their ratio, the post-remedy average and the throughput it implies
                   at unchanged WIP, and the arrival ceiling that a 40-week forecast requires. The
                   identity 18/2.55 == 240/34 is asserted as an equality, since the manuscript draws
                   attention to it. The count-view/size-view divergence is a sixth, smaller check set.
    commercial     both models priced to equality at plan, the asymmetric change (additions at the
                   change rate less omissions at the credit rate) plus change-board latency, the
                   capacity model's quantity exposure over the honest duration range, and the
                   breakeven change count solved and then re-checked by evaluating the two totals
                   either side of it. Sizing inflation is checked to move reported velocity while
                   leaving item throughput untouched.

Published MCQ distractors that are the results of named errors are checked too (holding throughput
constant when WIP rises; ignoring arrivals; blocked WIP from the iteration length instead of E[wait];
adding E[wait] to the average rather than the unblocked cycle time; treating a reciprocal as a
proportional change; rounding the cycle time before dividing), because the manuscript states each
distractor's arithmetic and a wrong distractor is as much a defect as a wrong answer.
"""


def run(ctx):
    check, D, ew, mesh = ctx["check"], ctx["D"], ctx["ew"], ctx["mesh"]
    # the shared check helper compares single Decimals; sequence identities are asserted as 1/0
    same = lambda a, b: 1 if list(a) == list(b) else 0
    COD = ctx["MERIDIAN_COST_OF_DELAY"]                     # USD 14,280 per week (Domain 1)
    AURIGA_COD = D(45000)                                   # USD/week, Domain 6 KA 6.4.2
    q = lambda x, n: x.quantize(D(1).scaleb(-n))

    # ---- master-thread values the domain restates -------------------------------------
    check("D13 Meridian cost of delay from its Domain 1 basis", D(28) * 6 * 85, COD)
    check("D13 Meridian 70 % benefit share",
          q(ctx["MERIDIAN_BENEFIT_70PCT"] / ctx["MERIDIAN_BENEFIT_FULL"] * 100, 1), D("70.0"))
    check("D13 Meridian E[wait] from M=4, L=2 (Domain 3)", ew(4, 2), D(4))

    # =============================== ENGINE 1 — flow (13.2.3) =========================
    T0, WSTAR, S, BACK, ARR, ITER = D(6), D(18), D("0.02"), D(240), D("1.5"), D(2)

    def thr(w):
        w = D(w)
        return T0 * w / WSTAR if w <= WSTAR else T0 * (D(1) - S * (w - WSTAR))

    def cyc(w):
        return D(w) / thr(w)

    check("13.2.3 capacity per 2-week iteration", T0 * ITER, 12)
    check("13.2.3 CT at WIP 18 (W/T)", cyc(18), 3)
    check("13.2.3 Little's Law reciprocal reading agrees at WIP 18", WSTAR / cyc(18), T0)
    check("13.2.3 W = T x C recovers the WIP at 18", thr(18) * cyc(18), WSTAR)
    check("13.2.3 throughput at WIP 30", thr(30), D("4.56"))
    check("13.2.3 CT at WIP 30", q(cyc(30), 2), D("6.58"))
    check("13.2.3 Little's Law reciprocal reading agrees at WIP 30", D(30) / cyc(30), thr(30))
    check("13.2.3 WIP change", q((D(30) / WSTAR - 1) * 100, 1), D("66.7"))
    check("13.2.3 throughput change", q((thr(30) / T0 - 1) * 100, 1), D("-24.0"))
    check("13.2.3 cycle-time change", q((cyc(30) / cyc(18) - 1) * 100, 1), D("119.3"))
    check("13.2.3 cycle-time multiple", q(cyc(30) / cyc(18), 3), D("2.193"))
    TOUCH = D("0.9")
    check("13.2.3 flow efficiency at WIP 18", q(TOUCH / cyc(18) * 100, 1), D("30.0"))
    check("13.2.3 flow efficiency at WIP 30", q(TOUCH / cyc(30) * 100, 1), D("13.7"))
    check("13.2.3 zero-throughput WIP solved from the model", WSTAR + D(1) / S, 68)
    check("13.2.3 backlog 240 at 6.00/week", BACK / T0, 40)
    check("13.2.3 backlog 240 at 4.56/week", q(BACK / thr(30), 4), D("52.6316"))
    check("13.2.3 extra weeks", q(BACK / thr(30) - BACK / T0, 4), D("12.6316"))
    check("13.2.3 cost of the WIP decision",
          q((BACK / thr(30) - BACK / T0) * COD, 2), D("180378.95"))
    # figure 13.2.1 plotted values
    check("Fig 13.2.1 CT at WIP 42", q(cyc(42), 2), D("13.46"))
    check("Fig 13.2.1 throughput at WIP 42", thr(42), D("3.12"))
    # MCQ 13.2-A/B distractors, each a named error
    check("MCQ 13.2-A distractor A (inverted ratio T/W)", q(T0 / WSTAR, 2), D("0.33"))
    check("MCQ 13.2-A distractor D (multiplied, not divided)", T0 * WSTAR, 108)
    check("MCQ 13.2-B distractor A (throughput held constant)", D(30) / T0, 5)
    check("MCQ 13.2-B distractor D (WIP assumed to raise throughput)",
          q(D(30) / D("7.20"), 2), D("4.17"))
    check("MCQ 13.2-F distractor C (efficiency inverted)", q(cyc(18) / TOUCH, 2), D("3.33"))

    # 13.2.2 commitment fallacy
    HIST = [D(x) for x in (5, 7, 4, 6, 8, 5, 7, 6, 5, 8, 6, 5)]
    check("13.2.2 12-week total", sum(HIST), 72)
    check("13.2.2 12-week mean", sum(HIST) / 12, T0)
    PAIRS = [HIST[i] + HIST[i + 1] for i in range(0, 12, 2)]
    check("13.2.2 six 2-week totals sum to the 12-week total", sum(PAIRS), sum(HIST))
    check("13.2.2 mean 2-week total", sum(PAIRS) / 6, T0 * ITER)
    below = sum(1 for x in PAIRS if x < T0 * ITER)
    check("13.2.2 iterations below the mean commitment", below, 2)
    check("13.2.2 share of iterations that miss", q(D(below) / 6 * 100, 1), D("33.3"))
    check("13.2.2 lowest observed 2-week total", min(PAIRS), 10)
    check("13.2.2 a commitment at the low end is met in every observed iteration",
          sum(1 for x in PAIRS if x < min(PAIRS)), 0)

    # =============================== ENGINE 2 — sequencing (13.1.3) ===================
    EPICS = {"E1": (D(4080), D(6)), "E2": (D(6120), D(14)),
             "E3": (D(2720), D(3)), "E4": (D(1360), D(11))}
    check("13.1.3 epic CoDs sum exactly to the programme cost of delay",
          sum(c for c, _ in EPICS.values()), COD)
    check("13.1.3 total effort", sum(e for _, e in EPICS.values()), 34)
    for name, dens in (("E3", D("906.67")), ("E1", D("680.00")),
                       ("E2", D("437.14")), ("E4", D("123.64"))):
        c, e = EPICS[name]
        check(f"13.1.3 density {name}", q(c / e, 2), dens)
    for name, share in (("E2", D("42.9")), ("E1", D("28.6")),
                        ("E3", D("19.0")), ("E4", D("9.5"))):
        check(f"13.1.3 {name} share of programme CoD",
              q(EPICS[name][0] / COD * 100, 1), share)

    def seq_cost(order):
        t, total = D(0), D(0)
        weeks = []
        for n in order:
            c, e = EPICS[n]
            t += e
            total += c * t
            weeks.append(int(t))
        return total, weeks

    names = list(EPICS)
    by_density = sorted(names, key=lambda n: -EPICS[n][0] / EPICS[n][1])
    by_cod = sorted(names, key=lambda n: -EPICS[n][0])
    by_effort = sorted(names, key=lambda n: EPICS[n][1])
    worst = sorted(names, key=lambda n: EPICS[n][0] / EPICS[n][1])
    check("13.1.3 density order sorts to E3, E1, E2, E4",
          same(by_density, ["E3", "E1", "E2", "E4"]), 1)
    dens_total, dens_weeks = seq_cost(by_density)
    check("13.1.3 density-order completion weeks", same(dens_weeks, [3, 9, 23, 34]), 1)
    check("13.1.3 density-order total delay cost", dens_total, 231880)
    cod_total, cod_weeks = seq_cost(by_cod)
    check("13.1.3 CoD-first order sorts to E2, E1, E3, E4",
          same(by_cod, ["E2", "E1", "E3", "E4"]), 1)
    check("13.1.3 CoD-first completion weeks", same(cod_weeks, [14, 20, 23, 34]), 1)
    check("13.1.3 CoD-first total", cod_total, 276080)
    check("13.1.3 saving of the density order", cod_total - dens_total, 44200)
    worst_total, worst_weeks = seq_cost(worst)
    check("13.1.3 worst order sorts to E4, E2, E1, E3",
          same(worst, ["E4", "E2", "E1", "E3"]), 1)
    check("13.1.3 worst-order completion weeks", same(worst_weeks, [11, 25, 31, 34]), 1)
    check("13.1.3 worst-order total", worst_total, 386920)
    check("13.1.3 best-to-worst spread", worst_total - dens_total, 155040)
    check("MCQ 13.1-A distractor C sorts to E3, E1, E4, E2 on effort alone",
          same(by_effort, ["E3", "E1", "E4", "E2"]), 1)
    check("MCQ 13.1-A distractor C total", seq_cost(by_effort)[0], 280160)
    # E2's breakeven CoD: the density that ties it with the second-ranked epic
    second = D(680)
    check("13.1.3 E2 breakeven CoD", second * EPICS["E2"][1], 9520)
    check("13.1.3 E2 breakeven uplift",
          q((second * EPICS["E2"][1] / EPICS["E2"][0] - 1) * 100, 1), D("55.6"))
    check("13.1.3 big-bang release at week 34", COD * 34, 485520)
    check("13.1.3 value created by releasability", COD * 34 - dens_total, 253640)

    # =============================== ENGINE 3 — forecasting (13.2.4) ==================
    WINS = [sum(HIST[i:i + 4]) for i in range(len(HIST) - 3)]
    check("13.2.4 nine four-week rolling totals match the printed series",
          same([int(w) for w in WINS], [22, 25, 23, 26, 26, 23, 26, 25, 24]), 1)
    check("13.2.4 nine windows from a twelve-week history", len(WINS), 9)
    RATES = sorted(w / 4 for w in WINS)
    lo, hi, med = RATES[0], RATES[-1], RATES[len(RATES) // 2]
    check("13.2.4 lowest sustained rate", lo, D("5.5"))
    check("13.2.4 highest sustained rate", hi, D("6.5"))
    check("13.2.4 median sustained rate", med, D("6.25"))
    check("13.2.4 naive forecast (mean rate, arrivals ignored)", BACK / (sum(HIST) / 12), 40)
    check("13.2.4 net drain at the mean rate", sum(HIST) / 12 - ARR, D("4.5"))
    check("13.2.4 forecast at the mean net drain", q(BACK / (sum(HIST) / 12 - ARR), 4),
          D("53.3333"))
    check("13.2.4 arrival understatement in weeks",
          q(BACK / (sum(HIST) / 12 - ARR) - BACK / (sum(HIST) / 12), 4), D("13.3333"))
    check("13.2.4 arrival understatement priced",
          (BACK / (sum(HIST) / 12 - ARR) - BACK / (sum(HIST) / 12)) * COD, 190400)
    check("13.2.4 fast end of the range", BACK / (hi - ARR), 48)
    check("13.2.4 slow end of the range", BACK / (lo - ARR), 60)
    PLAN = D(34)
    check("13.2.4 required net drain", q(BACK / PLAN, 4), D("7.0588"))
    check("13.2.4 required gross throughput", q(BACK / PLAN + ARR, 4), D("8.5588"))
    check("13.2.4 required rate above the best sustained rate",
          q(((BACK / PLAN + ARR) / hi - 1) * 100, 1), D("31.7"))
    check("13.2.4 required rate above the 12-week mean",
          q(((BACK / PLAN + ARR) / (sum(HIST) / 12) - 1) * 100, 1), D("42.6"))
    check("13.2.4 plan vs fast end, weeks", BACK / (hi - ARR) - PLAN, 14)
    check("13.2.4 plan vs fast end, priced", (BACK / (hi - ARR) - PLAN) * COD, 199920)
    check("13.2.4 plan vs slow end, weeks", BACK / (lo - ARR) - PLAN, 26)
    check("13.2.4 plan vs slow end, priced", (BACK / (lo - ARR) - PLAN) * COD, 371280)
    check("MCQ 13.2-C distractor A (arrivals ignored, fast)", q(BACK / hi, 2), D("36.92"))
    check("MCQ 13.2-C distractor A (arrivals ignored, slow)", q(BACK / lo, 2), D("43.64"))
    check("MCQ 13.2-C distractor D, fast (range in iterations, mislabelled weeks)",
          BACK / (hi - ARR) / ITER, 24)
    check("MCQ 13.2-C distractor D, slow (range in iterations, mislabelled weeks)",
          BACK / (lo - ARR) / ITER, 30)

    # =============================== ENGINE 4 — governance (13.3.2, 13.3.4) ==========
    EW, SH, WIP = ew(4, 2), D("0.15"), D(18)
    check("13.3.2 E[wait] in iterations", EW / ITER, 2)
    check("13.3.2 blocked items per iteration", SH * T0 * ITER, D("1.8"))
    check("13.3.2 blocked arrival rate", SH * T0, D("0.9"))
    check("13.3.2 blocked WIP (Little's Law on the blocked population)", SH * T0 * EW, D("3.6"))
    check("13.3.2 blocked share of WIP", q(SH * T0 * EW / WIP * 100, 1), D("20.0"))
    CU = cyc(18) - SH * EW
    check("13.3.2 unblocked cycle time", CU, D("2.4"))
    check("13.3.2 blocked cycle time", CU + EW, D("6.4"))
    check("13.3.2 the decomposition reconstructs the average",
          (D(1) - SH) * CU + SH * (CU + EW), cyc(18))
    check("13.3.2 blocked/unblocked ratio", q((CU + EW) / CU, 3), D("2.667"))
    EW2 = D(1)
    CT_NEW = (D(1) - SH) * CU + SH * (CU + EW2)
    check("13.3.2 blocked WIP with a 1-week route", SH * T0 * EW2, D("0.9"))
    check("13.3.2 blocked share with a 1-week route",
          q(SH * T0 * EW2 / WIP * 100, 1), D("5.0"))
    check("13.3.2 average cycle time with a 1-week route", q(CT_NEW, 3), D("2.550"))
    check("13.3.2 cycle-time reduction", q((D(1) - CT_NEW / cyc(18)) * 100, 1), D("15.0"))
    check("13.3.2 throughput at unchanged WIP", q(WIP / CT_NEW, 4), D("7.0588"))
    check("13.3.2 throughput uplift", q(((WIP / CT_NEW) / T0 - 1) * 100, 1), D("17.6"))
    check("13.3.2 identity 18/2.55 equals 240/34", WIP / CT_NEW, BACK / PLAN)
    check("13.3.2 240 at the improved gross rate", BACK / (WIP / CT_NEW), PLAN)
    check("13.3.2 240 net of arrivals at the improved rate",
          q(BACK / (WIP / CT_NEW - ARR), 2), D("43.17"))
    check("13.3.2 arrival ceiling for a 40-week forecast",
          q(WIP / CT_NEW - D(6), 4), D("1.0588"))
    check("13.3.2 required cut in discovery",
          q((D(1) - (WIP / CT_NEW - D(6)) / ARR) * 100, 1), D("29.4"))
    check("MCQ 13.3-A distractor D (share applied to WIP, not the arrival rate)",
          SH * WIP, D("2.7"))
    check("MCQ 13.3-B distractor C (E[wait] added to the average, not the unblocked CT)",
          cyc(18) + EW, 7)
    check("MCQ 13.3-B distractor C double-counts", cyc(18) + EW - (CU + EW), SH * EW)
    check("MCQ 13.3-C distractor C (reciprocal treated as proportional)",
          q(D(1) / (D(1) - D("0.15")), 3), D("1.176"))
    check("MCQ 13.3-C distractor D (CT rounded before dividing)", WIP / D("2.50"), D("7.2"))
    check("MCQ 13.3-C distractor D error size",
          q(WIP / D("2.50") - WIP / CT_NEW, 4), D("0.1412"))
    # 13.3.1 scaling, cited from Domains 12 and 4
    check("13.3.1 five streams meshed (Domain 4)", mesh(5), 10)
    check("13.3.1 nine streams meshed (Domain 4)", mesh(9), 36)
    check("13.3.1 links saved by an integration layer at nine streams", mesh(9) - 9, 27)
    # 13.3.4 count view against size view
    DONE, TOTAL, SZ_D, SZ_R, WKS = D(96), D(240), D("1.2"), D(2), D(16)
    REM = TOTAL - DONE
    size_d, size_r = DONE * SZ_D, REM * SZ_R
    check("13.3.4 percentage complete by count", q(DONE / TOTAL * 100, 1), D("40.0"))
    check("13.3.4 delivered size", size_d, D("115.2"))
    check("13.3.4 remaining size", size_r, D("288.0"))
    check("13.3.4 total size", size_d + size_r, D("403.2"))
    check("13.3.4 percentage complete by size",
          q(size_d / (size_d + size_r) * 100, 1), D("28.6"))
    check("13.3.4 overstatement in percentage points",
          q(DONE / TOTAL * 100 - size_d / (size_d + size_r) * 100, 1), D("11.4"))
    check("13.3.4 remaining duration on the count view", REM / T0, 24)
    check("13.3.4 delivered size rate", size_d / WKS, D("7.2"))
    check("13.3.4 remaining duration on the size view", size_r / (size_d / WKS), 40)
    check("13.3.4 understatement in weeks", size_r / (size_d / WKS) - REM / T0, 16)
    check("13.3.4 understatement priced", (size_r / (size_d / WKS) - REM / T0) * COD, 228480)

    # =============================== ENGINE 5 — commercial (13.4.1, 13.4.2) ==========
    ITEMS, SCOPE_PRICE, ITER_PRICE, PLAN_ITERS = D(240), D(1200000), D(60000), D(20)
    check("13.4.1 scope price per item", SCOPE_PRICE / ITEMS, 5000)
    check("13.4.1 capacity price at plan equals the scope price",
          ITER_PRICE * PLAN_ITERS, SCOPE_PRICE)
    check("13.4.1 capacity price per item at 12 items an iteration",
          ITER_PRICE / (T0 * ITER), 5000)
    ADD, CHG, CRED = D(30), D(7500), D(3000)
    check("13.4.1 additions priced", ADD * CHG, 225000)
    check("13.4.1 omissions credited", ADD * CRED, 90000)
    check("13.4.1 net price movement on a net-zero change", ADD * CHG - ADD * CRED, 135000)
    check("13.4.1 change-board latency priced", EW * COD, 57120)
    SCOPE_EXP = ADD * CHG - ADD * CRED + EW * COD
    CAP_EXP = (ITER / 2) * COD
    check("13.4.1 scope-based exposure per change", SCOPE_EXP, 192120)
    check("13.4.1 capacity-based exposure per change", CAP_EXP, 14280)
    check("13.4.1 difference per change", SCOPE_EXP - CAP_EXP, 177840)
    IT_LO, IT_HI = D(48) / ITER, D(60) / ITER
    check("13.4.1 honest duration range, fast end in iterations", IT_LO, 24)
    check("13.4.1 honest duration range, slow end in iterations", IT_HI, 30)
    check("13.4.1 capacity cost, fast end", IT_LO * ITER_PRICE, 1440000)
    check("13.4.1 capacity cost, slow end", IT_HI * ITER_PRICE, 1800000)
    QTY = IT_HI * ITER_PRICE - SCOPE_PRICE
    check("13.4.1 worst-case quantity exposure", QTY, 600000)
    check("13.4.1 breakeven change count", q(QTY / (SCOPE_EXP - CAP_EXP), 4), D("3.3738"))
    check("13.4.1 three changes leave the scope model cheaper",
          1 if SCOPE_PRICE + 3 * SCOPE_EXP < IT_HI * ITER_PRICE else 0, 1)
    check("13.4.1 three changes, scope total", SCOPE_PRICE + 3 * SCOPE_EXP, 1776360)
    check("13.4.1 four changes tip it", SCOPE_PRICE + 4 * SCOPE_EXP, 1968480)
    check("13.4.1 four changes exceed the capacity worst case",
          1 if SCOPE_PRICE + 4 * SCOPE_EXP > IT_HI * ITER_PRICE else 0, 1)
    IT_MEAN = (BACK / D("4.5")) / ITER
    check("13.4.1 expected duration in iterations", q(IT_MEAN, 4), D("26.6667"))
    check("13.4.1 capacity cost at the expected duration", IT_MEAN * ITER_PRICE, 1600000)
    check("13.4.1 breakeven against the expected capacity cost",
          q((IT_MEAN * ITER_PRICE - SCOPE_PRICE) / (SCOPE_EXP - CAP_EXP), 4), D("2.2492"))
    check("13.4.2 bounded funding envelope of four iterations", 4 * ITER_PRICE, 240000)
    check("13.4.2 velocity at average size 1.2", T0 * ITER * SZ_D, D("14.4"))
    check("13.4.2 velocity after 25 % sizing inflation",
          T0 * ITER * SZ_D * D("1.25"), D("18.0"))
    check("13.4.2 item throughput unchanged by sizing inflation", T0 * ITER, 12)

    # =============================== case studies ====================================
    check("Case A bad-path gross throughput", thr(30), D("4.56"))
    check("Case A bad-path net drain", thr(30) - ARR, D("3.06"))
    check("Case A bad-path forecast", q(BACK / (thr(30) - ARR), 4), D("78.4314"))
    check("Case A difference in weeks", q(BACK / (thr(30) - ARR) - D(40), 4), D("38.4314"))
    check("Case A difference priced", (BACK / (thr(30) - ARR) - D(40)) * COD, 548800)
    SQ_ITER, PLANNED, ACTUAL, STOP = D(120000), D(26), D(44), D(16)
    check("Case B iteration price is two squads at 60,000", SQ_ITER, 2 * ITER_PRICE)
    check("Case B planned cost", SQ_ITER * PLANNED, 3120000)
    check("Case B actual cost", SQ_ITER * ACTUAL, 5280000)
    check("Case B overrun", SQ_ITER * (ACTUAL - PLANNED), 2160000)
    check("Case B cost ratio", q(ACTUAL / PLANNED * 100, 1), D("169.2"))
    check("Case B overrun percentage", q((ACTUAL / PLANNED - 1) * 100, 1), D("69.2"))
    check("Case B benefit share attributed", q(D(758500) / D(1850000) * 100, 1), D("41.0"))
    check("Case B spend at the natural stop point", SQ_ITER * STOP, 1920000)
    check("Case B value of the two controls", SQ_ITER * (ACTUAL - STOP), 3360000)
    check("Case B control cost", D(4500) * 8, 36000)
    check("Case B protection ratio",
          q(SQ_ITER * (ACTUAL - STOP) / (D(4500) * 8), 1), D("93.3"))
    check("Case B envelope of four iterations", 4 * SQ_ITER, 480000)

    # =============================== exercises =======================================
    # 13.1 Auriga commissioning queue
    T1, W1, W1B, S1, Q1 = D(5), D(20), D(30), D("0.015"), D(140)
    T1B = T1 * (D(1) - S1 * (W1B - W1))
    check("EX 13.1 CT before", W1 / T1, 4)
    check("EX 13.1 throughput after", T1B, D("4.25"))
    check("EX 13.1 CT after", q(W1B / T1B, 2), D("7.06"))
    check("EX 13.1 queue at 5/week", Q1 / T1, 28)
    check("EX 13.1 queue at 4.25/week", q(Q1 / T1B, 2), D("32.94"))
    check("EX 13.1 delay", q(Q1 / T1B - Q1 / T1, 4), D("4.9412"))
    check("EX 13.1 delay priced at Auriga's 45,000/week",
          q((Q1 / T1B - Q1 / T1) * AURIGA_COD, 2), D("222352.94"))
    check("EX 13.1 common error (throughput held constant)", W1B / T1, 6)
    # 13.2 range forecast
    H2 = [D(x) for x in (7, 5, 8, 6, 9, 6, 7, 5, 8, 9)]
    check("EX 13.2 total", sum(H2), 70)
    check("EX 13.2 mean", sum(H2) / 10, 7)
    W2 = [sum(H2[i:i + 4]) for i in range(len(H2) - 3)]
    check("EX 13.2 rolling totals match the printed series",
          same([int(x) for x in W2], [26, 28, 29, 28, 27, 26, 29]), 1)
    lo2, hi2 = min(W2) / 4, max(W2) / 4
    check("EX 13.2 lowest sustained rate", lo2, D("6.5"))
    check("EX 13.2 highest sustained rate", hi2, D("7.25"))
    B2, A2, C2 = D(180), D("1.25"), D(9500)
    check("EX 13.2 fast end", B2 / (hi2 - A2), 30)
    check("EX 13.2 slow end", q(B2 / (lo2 - A2), 2), D("34.29"))
    check("EX 13.2 mean net drain", sum(H2) / 10 - A2, D("5.75"))
    check("EX 13.2 forecast at the mean net drain", q(B2 / (sum(H2) / 10 - A2), 2), D("31.30"))
    check("EX 13.2 naive forecast", q(B2 / (sum(H2) / 10), 2), D("25.71"))
    check("EX 13.2 understatement", q(B2 / (sum(H2) / 10 - A2) - B2 / (sum(H2) / 10), 4),
          D("5.5901"))
    check("EX 13.2 understatement priced",
          q((B2 / (sum(H2) / 10 - A2) - B2 / (sum(H2) / 10)) * C2, 2), D("53105.59"))
    check("EX 13.2 common error (single best week)", B2 / max(H2), 20)
    # 13.3 governance latency against iteration
    EW3, SH3, T3, W3, IT3 = ew(6, 2), D("0.12"), D(10), D(40), D(3)
    check("EX 13.3 E[wait]", EW3, 5)
    check("EX 13.3 E[wait] in iterations", q(EW3 / IT3, 4), D("1.6667"))
    check("EX 13.3 cycle time", W3 / T3, 4)
    check("EX 13.3 blocked arrival rate", SH3 * T3, D("1.2"))
    check("EX 13.3 blocked WIP", SH3 * T3 * EW3, 6)
    check("EX 13.3 blocked share", q(SH3 * T3 * EW3 / W3 * 100, 1), D("15.0"))
    CU3 = W3 / T3 - SH3 * EW3
    check("EX 13.3 unblocked cycle time", CU3, D("3.4"))
    check("EX 13.3 blocked cycle time", CU3 + EW3, D("8.4"))
    check("EX 13.3 ratio", q((CU3 + EW3) / CU3, 3), D("2.471"))
    CT3 = (D(1) - SH3) * CU3 + SH3 * (CU3 + D(1))
    check("EX 13.3 cycle time with a 1-week route", q(CT3, 3), D("3.520"))
    check("EX 13.3 reduction", q((D(1) - CT3 / (W3 / T3)) * 100, 1), D("12.0"))
    check("EX 13.3 throughput at unchanged WIP", q(W3 / CT3, 2), D("11.36"))
    check("EX 13.3 uplift", q(((W3 / CT3) / T3 - 1) * 100, 1), D("13.6"))
    check("EX 13.3 common error (iteration length for E[wait])", SH3 * T3 * IT3, D("3.6"))
    check("EX 13.3 common error share", q(SH3 * T3 * IT3 / W3 * 100, 1), D("9.0"))
    check("EX 13.3 common error understatement",
          q((D(1) - (SH3 * T3 * IT3) / (SH3 * T3 * EW3)) * 100, 1), D("40.0"))
    # 13.4 delay-cost density
    F = {"F1": (D(3600), D(4)), "F2": (D(5400), D(9)), "F3": (D(1800), D(6))}
    for n, d in (("F1", D(900)), ("F2", D(600)), ("F3", D(300))):
        check(f"EX 13.4 density {n}", F[n][0] / F[n][1], d)

    def fcost(order):
        t, s = D(0), D(0)
        for n in order:
            c, e = F[n]
            t += e
            s += c * t
        return s

    fnames = list(F)
    check("EX 13.4 density order sorts to F1, F2, F3",
          same(sorted(fnames, key=lambda n: -F[n][0] / F[n][1]), ["F1", "F2", "F3"]), 1)
    check("EX 13.4 density-order total", fcost(["F1", "F2", "F3"]), 118800)
    check("EX 13.4 CoD-first total", fcost(["F2", "F1", "F3"]), 129600)
    check("EX 13.4 shortest-first total", fcost(["F1", "F3", "F2"]), 135000)
    check("EX 13.4 worst-order total", fcost(["F3", "F2", "F1"]), 160200)
    check("EX 13.4 saving vs CoD-first",
          fcost(["F2", "F1", "F3"]) - fcost(["F1", "F2", "F3"]), 10800)
    check("EX 13.4 saving vs shortest-first",
          fcost(["F1", "F3", "F2"]) - fcost(["F1", "F2", "F3"]), 16200)
    check("EX 13.4 spread", fcost(["F3", "F2", "F1"]) - fcost(["F1", "F2", "F3"]), 41400)
    check("EX 13.4 F2 breakeven CoD", D(900) * F["F2"][1], 8100)
    check("EX 13.4 F2 breakeven uplift",
          q((D(8100) / F["F2"][0] - 1) * 100, 1), D("50.0"))
    # 13.5 contract exposure
    I5, SP5, IP5, C5 = D(180), D(900000), D(45000), D(9500)
    check("EX 13.5 price per item", SP5 / I5, 5000)
    check("EX 13.5 capacity total at plan", IP5 * 20, SP5)
    A5 = D(24)
    check("EX 13.5 additions", A5 * CHG, 180000)
    check("EX 13.5 credit", A5 * CRED, 72000)
    check("EX 13.5 net movement", A5 * CHG - A5 * CRED, 108000)
    check("EX 13.5 change-board latency", D(4) * C5, 38000)
    SE5 = A5 * CHG - A5 * CRED + D(4) * C5
    check("EX 13.5 scope exposure per change", SE5, 146000)
    check("EX 13.5 capacity exposure per change", (ITER / 2) * C5, 9500)
    check("EX 13.5 difference per change", SE5 - (ITER / 2) * C5, 136500)
    check("EX 13.5 capacity cost, fast end", 22 * IP5, 990000)
    check("EX 13.5 capacity cost, slow end", 27 * IP5, 1215000)
    check("EX 13.5 quantity exposure", 27 * IP5 - SP5, 315000)
    check("EX 13.5 breakeven change count",
          q((27 * IP5 - SP5) / (SE5 - (ITER / 2) * C5), 4), D("2.3077"))
    check("EX 13.5 three changes, scope total", SP5 + 3 * SE5, 1338000)
    check("EX 13.5 three changes exceed the capacity worst case",
          1 if SP5 + 3 * SE5 > 27 * IP5 else 0, 1)
