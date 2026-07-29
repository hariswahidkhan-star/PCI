"""PFL-AI Domain 16 — Data, automation and responsible AI in finance: golden checks.

Every number printed as a result in `domain-16-data-automation-responsible-ai.md` is recomputed
here from its definition rather than from its printed output. Master-thread values are read from
ctx and never re-typed: `KESTREL_CFADS` (6,384,000), `KESTREL_INSTALMENT` (5,009,635.23),
`KESTREL_DISCOUNT` (8 %). Domain 10's covenant trigger (6,011,562) and annual headroom (372,438)
are *derived* from those two here rather than taken on trust, because every data-quality figure in
this domain is expressed as a share of that headroom. The shared helpers `af(r, n)`, `ew(M, L)`
(PML-AI Domain 3 governance latency) and `mesh(n)` (PML-AI Domain 4 pairwise interface count) are
the registered forms the domain composes, so they are used rather than re-implemented.

The domain has nine engines, each checked from first principles:

  1. architecture (16.1.1)   mesh(9) pairwise reconciliations at 12 h a month against 9 spine feeds
                             at 4 h, both priced at the loaded analyst hour, then appraised as
                             `NPV = saving x af(0.08, 10) - build`. The convexity claim is pinned by
                             computing the tenth-system and fifteen-system estates and asserting the
                             27:1 marginal ratio, and the definitional exposure is expressed as a
                             multiple of headroom.
  2. automation economics    cost per item = minutes x rate + rate of error x consequence, on both
     (16.1.2)                processes; breakeven = fixed / advantage. The three sensitivities are
                             SOLVED (the automated error rate and the consequence cost that tie the
                             two processes at 56,400 records, and the breakeven with the error term
                             deleted), not re-typed, and the monotonicity claim — that the breakeven
                             RISES with the consequence of an error, because the automated residual
                             rate is the higher of the two — is pinned as an inequality and as its
                             own counterfactual (equal error rates make the consequence irrelevant).
  3. classifier (16.1.3)     the five-row confusion table is rebuilt from TP/FP/FN alone: TN, recall,
                             precision, accuracy, alert count and total cost are all derived, and the
                             row invariants (TP + FN = 1,200; TN = 46,800 - FP; row total = 48,000)
                             are asserted. Every adjacent step's marginal benefit less marginal cost
                             is checked to reconcile EXACTLY to the fall in the cost column — the
                             arithmetic identity the manuscript tells a reviewer to run — and the two
                             optima are located by argmin/argmax rather than asserted, so a
                             mis-stated "T4 not T2" would fail. The break-even posterior brackets the
                             marginal precisions at the optimum, and the 1:4 and 1:20 cost ratios are
                             re-optimised to confirm the stated tightening and loosening.
  4. coverage (16.2.1)       1 - (1 - p)^k, k >= ln(alpha)/ln(1 - p) (checked by substituting the
                             solved k back into the coverage form), and the expected percentile of a
                             minimum of k draws.
  5. sweep (16.2.2)          a^m and the inverse c^(1/m), checked by substitution; the load-bearing
                             verification cost and its multiple against Domain 2's 600,000.
  6. model assistance        hours x rate on three review regimes, the real and apparent savings, and
     (16.2.3)                EMV = p x consequence on Domain 6's 3,250,352 defect.
  7. assurance (16.3.1-4)    the attribution residual as a share of the forecast and of headroom;
                             the zero-failure sample-size ladder and the one-failure adjustment
                             (473 is verified as the SMALLEST n satisfying the two-term condition,
                             and 472 is verified to fail it); the blind-audit extrapolation, restated
                             recall and cost, and the label-completeness identity
                             true recall = labelled recall x completeness; and the inventory's
                             annual EMV and revalidation interval per model, with the volume-inversion
                             claim (M2 has the most uses and the smallest EMV) asserted.
  8. governance (16.4.1-3)   EMV avoided against the cost differential and the breakeven consequence
                             that divides them; the dual-approval threshold approver cost / caught
                             rate, with blanket, thresholded and sub-threshold policies priced and
                             the reconciliation blanket = thresholded - destroyed asserted; and
                             ew(M, L) x annual benefit / 365 on both approval designs.
  9. advanced + cases        the agentic inversion (removing a control destroys exactly the control's
                             net value), the fallback FTE count, the suspension increment (which is
                             identically quarter volume x per-record advantage), and both case
                             studies — including Case B's DSCR identities, cure amount, lock-up
                             crossing and waiver fee, all derived from the debt service.

Published MCQ distractors that the rationales describe as the results of named errors are checked
too (dividing the fixed cost by an absorption cost rather than by the differential; omitting the
error-cost term; omitting the catch rate; using the missed share instead of the caught share;
misplacing a decimal in the bad-payment rate; pricing a gate at the full meeting interval instead of
M/2 + L; the reciprocal-of-the-bound sample size and its 63 % confidence), because a distractor that
is not the result of the error its rationale names is as much a defect as a wrong answer.
"""


def run(ctx):
    check, D, af, ew, mesh = ctx["check"], ctx["D"], ctx["af"], ctx["ew"], ctx["mesh"]
    CF = ctx["KESTREL_CFADS"]                       # 6,384,000 (Domain 2 / Domain 10)
    DS = ctx["KESTREL_INSTALMENT"]                  # 5,009,635.23 (Domain 3)
    DISC = ctx["KESTREL_DISCOUNT"]                  # 8 %

    q = lambda x, n: D(x).quantize(D(1).scaleb(-n))
    pc = lambda x, n=4: q(D(x) * 100, n)            # a ratio printed as a percentage
    ln = lambda x: D(x).ln()
    ceil = lambda x: int(x) if D(x) == int(D(x)) else int(x) + 1
    ge = lambda a, b: 1 if D(a) >= D(b) else 0      # inequality claims, asserted as 1
    gt = lambda a, b: 1 if D(a) > D(b) else 0

    # ============================ 0 — the master thread this domain measures against ============
    check("Front matter Kestrel DSCR (Domain 10)", q(CF / DS, 4), D("1.2743"))
    COV = DS * D("1.2")
    check("Front matter 1.20x covenant CFADS trigger (Domain 10, display rounding)",
          COV, D(6011562), D("0.5"))
    check("Front matter annual covenant headroom (Domain 10, display rounding)",
          CF - COV, D(372438), D("0.5"))
    HEAD = D(372438)                # Domain 10's printed headroom; the yardstick of this domain
    RATE_H, RATE_MIN = D("96.00"), D("1.60")
    check("Front matter loaded analyst minute from the hour", RATE_H / 60, RATE_MIN)
    RATE_SR, RATE_LEG = D("150.00"), D("240.00")
    check("16.1.1 the two Domain 2 DSCR readings (pre/post working capital)",
          q(D(6984000) / DS, 2), D("1.39"))
    check("16.1.1 the two Domain 2 DSCR readings (post working capital)", q(CF / DS, 2), D("1.27"))

    # the estate
    KESTREL_RECS, ESTATE_RECS, ASSETS = D(9400), D(56400), 6
    check("Master estate six assets x 9,400 = 56,400 records", KESTREL_RECS * ASSETS, ESTATE_RECS)
    ITEMS, PREV = D(48000), D("0.025")
    check("Master estate genuinely erroneous payments (2.5 % of 48,000)", ITEMS * PREV, 1200)
    check("Master estate clean payments", ITEMS * (1 - PREV), 46800)

    # ==================== 1 — WE 16.1.1 the financial-data spine ================================
    N_SYS = 9
    MESH_H_MO, FEED_H_MO = D(12), D(4)
    BUILD, RUN = D(900000), D(140000)
    check("WE 16.1.1 mesh interfaces among 9 systems", mesh(N_SYS), 36)
    check("WE 16.1.1 spine feeds among 9 systems", N_SYS, 9)
    mesh_h = D(mesh(N_SYS)) * MESH_H_MO * 12
    feed_h = D(N_SYS) * FEED_H_MO * 12
    check("WE 16.1.1 mesh hours a year", mesh_h, 5184)
    check("WE 16.1.1 mesh cost a year", mesh_h * RATE_H, 497664)
    check("WE 16.1.1 spine hours a year", feed_h, 432)
    check("WE 16.1.1 spine labour a year", feed_h * RATE_H, 41472)
    spine_all_in = feed_h * RATE_H + RUN
    check("WE 16.1.1 spine all-in a year", spine_all_in, 181472)
    SAVE = mesh_h * RATE_H - spine_all_in
    check("WE 16.1.1 net annual saving", SAVE, 316192)
    check("WE 16.1.1 simple payback (years)", q(BUILD / SAVE, 4), D("2.8464"))
    check("WE 16.1.1 AF(0.08, 10)", q(af(DISC, 10), 6), D("6.710081"))
    check("WE 16.1.1 NPV of the spine", q(SAVE * af(DISC, 10) - BUILD, 0), D(1221674))
    # convexity: the tenth system, and the fifteen-system estate
    add_mesh = D(mesh(10) - mesh(N_SYS))
    check("WE 16.1.1 pairwise reconciliations added by a tenth system", add_mesh, 9)
    check("WE 16.1.1 hours added by a tenth system (mesh)", add_mesh * MESH_H_MO * 12, 1296)
    check("WE 16.1.1 cost added by a tenth system (mesh)", add_mesh * MESH_H_MO * 12 * RATE_H,
          124416)
    check("WE 16.1.1 hours added by one more spine feed", FEED_H_MO * 12, 48)
    check("WE 16.1.1 cost added by one more spine feed", FEED_H_MO * 12 * RATE_H, 4608)
    check("WE 16.1.1 marginal mesh-to-spine ratio",
          (add_mesh * MESH_H_MO * 12) / (FEED_H_MO * 12), 27)
    check("WE 16.1.1 mesh reconciliations at 15 systems", mesh(15), 105)
    check("WE 16.1.1 mesh cost at 15 systems", D(mesh(15)) * MESH_H_MO * 12 * RATE_H, 1451520)
    check("WE 16.1.1 spine cost at 15 systems (labour)", D(15) * FEED_H_MO * 12 * RATE_H, 69120)
    check("WE 16.1.1 cost of keeping the reconciliations (spine labour + run)", spine_all_in,
          181472)
    DEFN = D(600000)                                        # Domain 2 KA 2.3.1
    check("WE 16.1.1 definitional divergence as a multiple of headroom", q(DEFN / HEAD, 4),
          D("1.6110"))
    DEFN_PV = DEFN * af(DISC, 10)
    check("WE 16.1.1 a recurring definitional divergence in present value over the appraisal",
          q(DEFN_PV, 0), 4026049)
    check("WE 16.1.1 that exposure as a multiple of the spine's labour NPV",
          q(DEFN_PV / (SAVE * af(DISC, 10) - BUILD), 4), D("3.2955"))
    check("INV 16.1.1 the definitional case exceeds the labour case",
          gt(DEFN_PV, SAVE * af(DISC, 10) - BUILD), 1)
    check("INV 16.1.1 mesh is convex, layer is linear (marginal cost ratio > 1)",
          gt((add_mesh * MESH_H_MO * 12), (FEED_H_MO * 12)), 1)

    # ==================== 2 — WE 16.1.2 automation breakeven ====================================
    MAN_MIN, MAN_ERR = D("8.5"), D("0.0180")
    CONS = D(320)
    FIXED = D(148000)
    PLAT, REFER, ADJ_MIN, AUTO_ERR = D("0.50"), D("0.05"), D("6.75"), D("0.0260")
    man_proc = MAN_MIN * RATE_MIN
    check("WE 16.1.2 manual processing cost a record", man_proc, D("13.60"))
    check("WE 16.1.2 manual error cost a record", MAN_ERR * CONS, D("5.76"))
    MAN_ALL = man_proc + MAN_ERR * CONS
    check("WE 16.1.2 manual all-in cost a record", MAN_ALL, D("19.36"))
    check("WE 16.1.2 automated adjudication cost a record", REFER * (ADJ_MIN * RATE_MIN),
          D("0.54"))
    auto_proc = PLAT + REFER * (ADJ_MIN * RATE_MIN)
    check("WE 16.1.2 automated processing cost a record", auto_proc, D("1.04"))
    check("WE 16.1.2 automated error cost a record", AUTO_ERR * CONS, D("8.32"))
    AUTO_ALL = auto_proc + AUTO_ERR * CONS
    check("WE 16.1.2 automated all-in cost a record", AUTO_ALL, D("9.36"))
    ADV = MAN_ALL - AUTO_ALL
    check("WE 16.1.2 per-record advantage", ADV, D("10.00"))
    check("WE 16.1.2 breakeven volume", FIXED / ADV, 14800)
    net = lambda v, adv=None: D(v) * (ADV if adv is None else adv) - FIXED
    check("WE 16.1.2 net annual value at Kestrel's 9,400 records", net(KESTREL_RECS), -54000)
    check("WE 16.1.2 net annual value at the estate's 56,400 records", net(ESTATE_RECS), 416000)
    check("WE 16.1.2 manual total at 56,400", ESTATE_RECS * MAN_ALL, 1091904)
    check("WE 16.1.2 automated total at 56,400", FIXED + ESTATE_RECS * AUTO_ALL, 675904)
    check("WE 16.1.2 automated all-in a record at 56,400",
          q((FIXED + ESTATE_RECS * AUTO_ALL) / ESTATE_RECS, 4), D("11.9841"))
    check("WE 16.1.2 automated total at 9,400", FIXED + KESTREL_RECS * AUTO_ALL, 235984)
    check("WE 16.1.2 manual total at 9,400", KESTREL_RECS * MAN_ALL, 181984)
    # error term deleted — the specific nameable defect
    ADV_NOERR = man_proc - auto_proc
    check("WE 16.1.2 advantage with the error term deleted", ADV_NOERR, D("12.56"))
    BE_NOERR = FIXED / ADV_NOERR
    check("WE 16.1.2 apparent breakeven with the error term deleted", q(BE_NOERR, 0), 11783)
    check("WE 16.1.2 records by which the apparent breakeven understates", q(FIXED / ADV - BE_NOERR, 0),
          3017)
    check("WE 16.1.2 percentage by which omitting the error term flatters automation",
          pc((FIXED / ADV - BE_NOERR) / (FIXED / ADV)), D("20.3822"))
    # sensitivities SOLVED at the estate volume
    e_star = ((ESTATE_RECS * MAN_ALL - FIXED) / ESTATE_RECS - auto_proc) / CONS
    check("WE 16.1.2 automated error rate at which the case ties at 56,400", pc(e_star), D("4.9050"))
    check("INV 16.1.2 the tolerated error rate is nearly double the modelled 2.60 %",
          gt(e_star, AUTO_ERR * D("1.8")), 1)
    # consequence cost that ties the two processes at 56,400:
    #   fixed + V(auto_proc + e_a c) = V(man_proc + e_m c)  =>  c = (V(man_proc-auto_proc)-fixed)
    #                                                            / (V(e_a - e_m))
    c_star = (ESTATE_RECS * ADV_NOERR - FIXED) / (ESTATE_RECS * (AUTO_ERR - MAN_ERR))
    check("WE 16.1.2 consequence cost at which the case ties at 56,400", q(c_star, 2),
          D("1241.99"))
    # repriced consequence
    CONS2 = D(1200)
    man2, auto2 = man_proc + MAN_ERR * CONS2, auto_proc + AUTO_ERR * CONS2
    check("WE 16.1.2 manual all-in at USD 1,200 an error", man2, D("35.20"))
    check("WE 16.1.2 automated all-in at USD 1,200 an error", auto2, D("32.24"))
    check("WE 16.1.2 advantage at USD 1,200 an error", man2 - auto2, D("2.96"))
    check("WE 16.1.2 breakeven at USD 1,200 an error", FIXED / (man2 - auto2), 50000)
    check("WE 16.1.2 net annual value at 56,400 and USD 1,200 an error",
          net(ESTATE_RECS, man2 - auto2), 18944)
    # the teaching invariant: the breakeven RISES with consequence, because e_auto > e_man
    be = lambda c: FIXED / ((man_proc + MAN_ERR * D(c)) - (auto_proc + AUTO_ERR * D(c)))
    check("INV 16.1.2 automated residual error rate exceeds the manual one",
          gt(AUTO_ERR, MAN_ERR), 1)
    check("INV 16.1.2 breakeven is monotonically rising in the consequence per error",
          1 if all(be(c) < be(c + 100) for c in (0, 100, 200, 320, 600, 900, 1200)) else 0, 1)
    check("INV 16.1.2 breakeven at 320 is below the breakeven at 1,200", gt(be(1200), be(320)), 1)
    check("INV 16.1.2 with equal error rates the consequence leaves the breakeven untouched",
          FIXED / (man_proc + MAN_ERR * D(1200) - (auto_proc + MAN_ERR * D(1200))), BE_NOERR)
    check("INV 16.1.2 breakeven identity — the two totals are equal at V*",
          (FIXED / ADV) * MAN_ALL - (FIXED + (FIXED / ADV) * AUTO_ALL), 0)
    check("INV 16.1.2 Kestrel's volume is below the breakeven and the estate's is above",
          1 if KESTREL_RECS < FIXED / ADV < ESTATE_RECS else 0, 1)

    # Fig 16.1.2 — plotted values
    check("Fig 16.1.2 all three lines start at minus the committed fixed cost", -FIXED, -148000)
    check("Fig 16.1.2 slate slope (error cost ignored)", ADV_NOERR, D("12.56"))
    check("Fig 16.1.2 slate zero crossing", q(BE_NOERR, 0), 11783)
    check("Fig 16.1.2 blue slope (USD 320)", ADV, D("10.00"))
    check("Fig 16.1.2 blue zero crossing", FIXED / ADV, 14800)
    check("Fig 16.1.2 crimson slope (USD 1,200)", man2 - auto2, D("2.96"))
    check("Fig 16.1.2 crimson zero crossing", FIXED / (man2 - auto2), 50000)
    check("Fig 16.1.2 blue marker at Kestrel 9,400", net(KESTREL_RECS), -54000)
    check("Fig 16.1.2 blue marker at the portfolio 56,400", net(ESTATE_RECS), 416000)
    check("Fig 16.1.2 crimson marker at the portfolio 56,400", net(ESTATE_RECS, man2 - auto2),
          18944)
    check("Fig 16.1.2 the y-axis top (780k) contains the steepest line at 70,000",
          ge(D(780000), net(70000, ADV_NOERR)), 1)
    check("Fig 16.1.2 the y-axis floor (-220k) contains the common intercept",
          ge(-FIXED, D(-220000)), 1)

    # forecast-quality translation
    check("16.1.2 manual forecast error band on CFADS (4.8 % MAPE)", CF * D("0.048"), 306432)
    check("16.1.2 automated forecast error band on CFADS (3.1 % MAPE)", CF * D("0.031"), 197904)
    check("16.1.2 manual band as a share of covenant headroom",
          pc(CF * D("0.048") / HEAD), D("82.2773"))
    check("16.1.2 automated band as a share of covenant headroom",
          pc(CF * D("0.031") / HEAD), D("53.1374"))
    check("16.1.2 the accuracy improvement in points", D("4.8") - D("3.1"), D("1.7"))
    check("INV 16.1.2 the manual band nearly consumes headroom, the automated one about half",
          1 if D("0.80") < CF * D("0.048") / HEAD < 1 and
             D("0.50") < CF * D("0.031") / HEAD < D("0.60") else 0, 1)

    # ==================== 3 — WE 16.1.3 the detector as a classifier ============================
    FP_C, FN_C = D(40), D(320)
    check("WE 16.1.3 false-positive cost is 25 analyst minutes", D(25) * RATE_MIN, FP_C)
    ROWS = [("T1", "0.90", 600, 110, 600), ("T2", "0.80", 720, 205, 480),
            ("T3", "0.70", 840, 400, 360), ("T4", "0.60", 960, 950, 240),
            ("T5", "0.45", 1080, 2900, 120)]
    PRINTED = {   # recall %, precision %, accuracy %, alerts, total cost
        "T1": (D("50.0"), D("84.5070"), D("98.5208"), 710, D(196400)),
        "T2": (D("60.0"), D("77.8378"), D("98.5729"), 925, D(161800)),
        "T3": (D("70.0"), D("67.7419"), D("98.4167"), 1240, D(131200)),
        "T4": (D("80.0"), D("50.2618"), D("97.5208"), 1910, D(114800)),
        "T5": (D("90.0"), D("27.1357"), D("93.7083"), 3980, D(154400)),
    }
    costs, accs = [], []
    for lab, cut, tp, fp, fn in ROWS:
        tp, fp, fn = D(tp), D(fp), D(fn)
        tn = ITEMS * (1 - PREV) - fp
        rec, prec = tp / (tp + fn), tp / (tp + fp)
        acc = (tp + tn) / ITEMS
        cost = fp * FP_C + fn * FN_C
        r = PRINTED[lab]
        check(f"WE 16.1.3 {lab} real errors reconcile (TP + FN = 1,200)", tp + fn, 1200)
        check(f"WE 16.1.3 {lab} row totals to the population", tp + fp + fn + tn, ITEMS)
        check(f"WE 16.1.3 {lab} recall", pc(rec, 1), r[0])
        check(f"WE 16.1.3 {lab} precision", pc(prec), r[1])
        check(f"WE 16.1.3 {lab} accuracy", pc(acc), r[2])
        check(f"WE 16.1.3 {lab} alert count", tp + fp, r[3])
        check(f"WE 16.1.3 {lab} total misclassification cost", cost, r[4])
        costs.append(cost)
        accs.append(acc)
    imin, imax = costs.index(min(costs)), accs.index(max(accs))
    check("WE 16.1.3 cost-minimising threshold is the fourth (T4)", imin, 3)
    check("WE 16.1.3 accuracy-maximising threshold is the second (T2)", imax, 1)
    check("INV 16.1.3 the two optima are different thresholds", 1 if imin != imax else 0, 1)
    check("WE 16.1.3 minimum total cost", min(costs), 114800)
    check("WE 16.1.3 cost at the accuracy-maximising threshold", costs[imax], 161800)
    check("WE 16.1.3 annual cost of choosing accuracy", costs[imax] - min(costs), 47000)
    check("WE 16.1.3 the two error costs differ eight-fold", FN_C / FP_C, 8)
    POST = FP_C / FN_C
    check("WE 16.1.3 break-even posterior", pc(POST, 1), D("12.5"))
    check("WE 16.1.3 break-even posterior is one in eight", 1 / POST, 8)
    check("WE 16.1.3 adjudication hours for T4's alert queue (25 minutes an alert)",
          q(D(1910) * D(25) / 60, 2), D("795.83"))
    check("WE 16.1.3 analyst cost of T4's alert queue", D(1910) * FP_C, 76400)
    # marginal steps — the identity the manuscript tells a reviewer to run
    for i in range(4):
        a, b = ROWS[i], ROWS[i + 1]
        dtp, dfp = D(b[2] - a[2]), D(b[3] - a[3])
        ben, cst = dtp * FN_C, dfp * FP_C
        check(f"INV 16.1.3 step {a[0]}->{b[0]} net reconciles to the fall in total cost",
              ben - cst, costs[i] - costs[i + 1])
    check("WE 16.1.3 T3->T4 additional true positives", D(960 - 840), 120)
    check("WE 16.1.3 T3->T4 additional false positives", D(950 - 400), 550)
    mp34 = D(120) / D(120 + 550)
    check("WE 16.1.3 T3->T4 marginal precision", pc(mp34), D("17.9104"))
    check("WE 16.1.3 T3->T4 marginal benefit", D(120) * FN_C, 38400)
    check("WE 16.1.3 T3->T4 marginal cost", D(550) * FP_C, 22000)
    check("WE 16.1.3 T3->T4 net", D(120) * FN_C - D(550) * FP_C, 16400)
    check("WE 16.1.3 T4->T5 additional false positives", D(2900 - 950), 1950)
    mp45 = D(120) / D(120 + 1950)
    check("WE 16.1.3 T4->T5 marginal precision", pc(mp45), D("5.7971"))
    check("WE 16.1.3 T4->T5 marginal cost", D(1950) * FP_C, 78000)
    check("WE 16.1.3 T4->T5 net", D(120) * FN_C - D(1950) * FP_C, -39600)
    check("INV 16.1.3 the optimum is where marginal precision crosses the break-even posterior",
          1 if mp34 > POST > mp45 else 0, 1)
    check("INV 16.1.3 average precision exceeds the posterior at every threshold, including T5",
          1 if all(D(tp) / D(tp + fp) > POST for _, _, tp, fp, _ in ROWS) else 0, 1)
    # cost-ratio re-optimisation: 1:4 tightens to T3, 1:20 loosens to T5
    for fnc, want, label in ((D(160), 2, "1:4 tightens to T3"), (D(800), 4, "1:20 loosens to T5")):
        cs = [D(fp) * FP_C + D(fn) * fnc for _, _, _, fp, fn in ROWS]
        check(f"INV 16.1.3 cost ratio {label}", cs.index(min(cs)), want)

    # Fig 16.1.3 — plotted values
    for i, (lab, val, acc_val) in enumerate((("T1", 196400, D("98.5208")),
                                             ("T2", 161800, D("98.5729")),
                                             ("T3", 131200, D("98.4167")),
                                             ("T4", 114800, D("97.5208")),
                                             ("T5", 154400, D("93.7083")))):
        check(f"Fig 16.1.3 bar {lab} total annual cost", costs[i], val)
        check(f"Fig 16.1.3 accuracy point {lab}", pc(accs[i]), acc_val)
    check("Fig 16.1.3 left axis top (220k) contains the tallest bar", ge(D(220000), max(costs)), 1)
    check("Fig 16.1.3 right axis (92-99.6 %) contains every accuracy",
          1 if all(D(92) <= a * 100 <= D("99.6") for a in accs) else 0, 1)
    check("Fig 16.1.3 the annotated gap between the two optima", costs[imax] - min(costs), 47000)

    # ==================== MCQs — KA 16.1 =======================================================
    check("MCQ 16.1-A answer B (fixed / advantage)", FIXED / ADV, 14800)
    check("MCQ 16.1-A distractor A (error term omitted both sides)", q(BE_NOERR, 0), 11783)
    check("MCQ 16.1-A distractor C (fixed / automated all-in, an absorption reading)",
          q(FIXED / AUTO_ALL, 0), 15812)
    check("MCQ 16.1-A distractor D (fixed / automated processing cost alone)",
          q(FIXED / auto_proc, 0), 142308)
    check("MCQ 16.1-B answer C (breakeven rises from 14,800 to 50,000)",
          FIXED / (man2 - auto2), 50000)
    check("MCQ 16.1-B rationale net at 56,400 on the repriced consequence",
          net(ESTATE_RECS, man2 - auto2), 18944)
    check("MCQ 16.1-C answer C (the fourth threshold minimises total cost)", imin, 3)
    check("MCQ 16.1-C distractor A cost penalty of the accuracy choice",
          costs[imax] - min(costs), 47000)
    check("MCQ 16.1-C distractor B (T5's extra catches cost 78,000 to save 38,400)",
          D(1950) * FP_C - D(120) * FN_C, 39600)
    check("MCQ 16.1-C distractor D quoted precision at T1", PRINTED["T1"][1], D("84.5070"))
    check("MCQ 16.1-D answer B marginal precision", pc(mp45), D("5.7971"))
    check("MCQ 16.1-D answer B net", D(120) * FN_C - D(1950) * FP_C, -39600)
    check("MCQ 16.1-D distractor A quoted average precision at T5", PRINTED["T5"][1],
          D("27.1357"))

    # ==================== 4 — WE 16.2.1 scenario coverage ======================================
    P_MODE, K1 = D("0.05"), 40
    cover = 1 - (1 - P_MODE) ** K1
    check("WE 16.2.1 coverage of a 5 % mode in 40 scenarios", q(cover, 6), D("0.871488"))
    check("WE 16.2.1 expected breaches in 40 scenarios at 5 %", P_MODE * K1, 2)
    k95 = ln(D("0.05")) / ln(1 - P_MODE)
    check("WE 16.2.1 k for 95 % confidence on a 5 % mode", ceil(k95), 59)
    check("INV 16.2.1 the solved k restores the stated confidence",
          q(1 - (1 - P_MODE) ** k95, 6), D("0.950000"))
    k90 = ln(D("0.10")) / ln(1 - D("0.01"))
    check("WE 16.2.1 k for 90 % confidence on a 1 % mode", ceil(k90), 230)
    check("INV 16.2.1 the solved k restores the stated confidence (1 % mode)",
          q(1 - (1 - D("0.01")) ** k90, 6), D("0.900000"))
    check("WE 16.2.1 expected percentile of the minimum of 500 draws", pc(D(1) / 501), D("0.1996"))
    check("INV 16.2.1 a larger k pushes the generated minimum further into the tail",
          1 if D(1) / 501 < D(1) / 41 else 0, 1)

    # ==================== 5 — WE 16.2.2 the load-bearing subset ================================
    TERMS, LB, ACC_ITEM = D(340), 26, D("0.92")
    sweep = ACC_ITEM ** LB
    check("WE 16.2.2 probability all 26 load-bearing terms are correct", q(sweep, 6),
          D("0.114415"))
    check("WE 16.2.2 probability at least one is wrong", pc(1 - sweep), D("88.5585"))
    check("WE 16.2.2 expected errors across all 340 terms", TERMS * (1 - ACC_ITEM), D("27.2"))
    check("WE 16.2.2 a correct 26-item summary about one time in nine", q(1 / sweep, 0), 9)
    req = (ln(D("0.95")) / LB).exp()
    check("WE 16.2.2 per-item accuracy required for a 95 % clean sweep of 26", pc(req),
          D("99.8029"))
    check("INV 16.2.2 the required per-item accuracy reproduces the sweep confidence",
          q(req ** LB, 4), D("0.9500"))
    check("INV 16.2.2 sweep probability falls as the subset grows",
          1 if ACC_ITEM ** 26 < ACC_ITEM ** 18 < ACC_ITEM ** 1 else 0, 1)
    VERIFY = D(LB) * D("0.5") * RATE_LEG
    check("WE 16.2.2 cost of verifying the 26 load-bearing definitions", VERIFY, 3120)
    check("WE 16.2.2 definitional exposure as a multiple of headroom", q(DEFN / HEAD, 4),
          D("1.6110"))
    check("WE 16.2.2 verification is cheaper than one definitional error by", q(DEFN / VERIFY, 4),
          D("192.3077"))
    check("WE 16.2.2 terms treated as indicative", TERMS - LB, 314)

    # ==================== 6 — WE 16.2.3 model assistance ======================================
    H_BUILD, H_REV, M_BUILD, M_REV, M_REV_HABIT = D(40), D(8), D(6), D(26), D(8)
    hand = (H_BUILD + H_REV) * RATE_SR
    mach = (M_BUILD + M_REV) * RATE_SR
    habit = (M_BUILD + M_REV_HABIT) * RATE_SR
    check("WE 16.2.3 hand-built total cost", hand, 7200)
    check("WE 16.2.3 machine-assisted cost with proper review", mach, 4800)
    check("WE 16.2.3 real saving", hand - mach, 2400)
    check("WE 16.2.3 real saving as a percentage", pc((hand - mach) / hand), D("33.3333"))
    check("WE 16.2.3 cost at the habitual 8-hour review", habit, 2100)
    check("WE 16.2.3 apparent saving as a percentage", pc((hand - habit) / hand), D("70.8333"))
    check("WE 16.2.3 omitted review hours", M_REV - M_REV_HABIT, 18)
    check("WE 16.2.3 value of the omitted review", (M_REV - M_REV_HABIT) * RATE_SR, 2700)
    P_DEF, DEF_CONS = D("0.35"), D(3250352)             # Domain 6 KA 6.4.1
    EMV_DEF = P_DEF * DEF_CONS
    check("WE 16.2.3 EMV of the defect the omitted review would catch", q(EMV_DEF, 2),
          D("1137623.20"))
    check("WE 16.2.3 EMV as a multiple of the omitted review",
          q(EMV_DEF / ((M_REV - M_REV_HABIT) * RATE_SR), 4), D("421.3419"))
    check("WE 16.2.3 total hours saved", (H_BUILD + H_REV) - (M_BUILD + M_REV), 16)
    check("WE 16.2.3 review hours per assisted build hour", q(M_REV / M_BUILD, 4), D("4.3333"))
    check("WE 16.2.3 review hours per hand-built hour (one per five)", H_REV / H_BUILD, D("0.2"))
    check("INV 16.2.3 the real saving is about a third, not two thirds",
          1 if (hand - mach) / hand < (hand - habit) / hand else 0, 1)
    check("INV 16.2.3 the omitted review is trivial against the exposure it removes",
          gt(EMV_DEF, (M_REV - M_REV_HABIT) * RATE_SR * 100), 1)

    # ==================== MCQs — KA 16.2 ======================================================
    check("MCQ 16.2-A answer B (0.92^26)", pc(sweep), D("11.4415"))
    check("MCQ 16.2-A distractor A (per-item accuracy read as deliverable accuracy)",
          pc(ACC_ITEM, 2), D("92.00"))
    check("MCQ 16.2-A distractor C (the complement)", pc(1 - sweep, 2), D("88.56"))
    check("MCQ 16.2-A distractor D (the accuracy a 95 % sweep would need)", pc(req, 2),
          D("99.80"))
    check("MCQ 16.2-B answer C verification cost", VERIFY, 3120)
    check("MCQ 16.2-B answer C multiple against the 600,000 exposure", q(DEFN / VERIFY, 4),
          D("192.3077"))
    check("MCQ 16.2-B distractor B unachievable target", pc(req), D("99.8029"))
    check("MCQ 16.2-C answer A required k on a 1 % mode at 90 %", ceil(k90), 230)
    check("MCQ 16.2-C answer A generated minimum percentile", pc(D(1) / 501), D("0.1996"))
    check("MCQ 16.2-C distractor B (sample frequency read as a probability)",
          pc(D(3) / 40, 1), D("7.5"))
    check("MCQ 16.2-D answer B", pc((hand - mach) / hand), D("33.3333"))
    check("MCQ 16.2-D distractor A (review left at the hand-built hours)",
          pc((hand - habit) / hand), D("70.8333"))
    check("MCQ 16.2-D distractor C (build hours only, 6 against 40)",
          pc((H_BUILD - M_BUILD) / H_BUILD), D("85.0000"))
    check("MCQ 16.2-D distractor D total hours are NOT similar (16 hours apart)",
          (H_BUILD + H_REV) - (M_BUILD + M_REV), 16)

    # ==================== 7a — 16.3.1 attribution residual ====================================
    ATTR = D(6190000)
    RESID = CF - ATTR
    check("16.3.1 attribution residual", RESID, 194000)
    check("16.3.1 residual as a share of the forecast", pc(RESID / CF), D("3.0388"))
    check("16.3.1 residual as a share of covenant headroom", pc(RESID / HEAD), D("52.0892"))
    check("INV 16.3.1 the residual exceeds half the covenant headroom",
          gt(RESID / HEAD, D("0.5")), 1)
    check("INV 16.3.1 the hundred-per-cent rule: attributions + residual = the output",
          ATTR + RESID, CF)

    # ==================== 7b — WE 16.3.2 validation sample size ===============================
    ALPHA95, ALPHA99 = D("0.05"), D("0.01")
    n_1pc = ln(ALPHA95) / ln(1 - D("0.01"))
    check("WE 16.3.2 exact n for a 1 % bound at 95 %", q(n_1pc, 4), D("298.0729"))
    check("WE 16.3.2 test cases for a 1 % bound at 95 %", ceil(n_1pc), 299)
    check("INV 16.3.2 the solved n restores the stated confidence",
          q((1 - D("0.01")) ** n_1pc, 4), ALPHA95)
    LADDER = (("5 per cent", "0.05", 59), ("2 per cent", "0.02", 149), ("1 per cent", "0.01", 299),
              ("0.5 per cent", "0.005", 598), ("0.1 per cent", "0.001", 2995))
    prev_n = 0
    for label, p, want in LADDER:
        n = ceil(ln(ALPHA95) / ln(1 - D(p)))
        check(f"WE 16.3.2 ladder — {label} bound at 95 % confidence", n, want)
        check(f"INV 16.3.2 ladder is monotone at the {label} bound", gt(n, prev_n), 1)
        prev_n = n
    check("WE 16.3.2 n for a 1 % bound at 99 % confidence",
          ceil(ln(ALPHA99) / ln(1 - D("0.01"))), 459)
    check("INV 16.3.2 raising confidence raises the requirement",
          gt(ceil(ln(ALPHA99) / ln(1 - D("0.01"))), ceil(n_1pc)), 1)
    # one permitted failure — solved, not asserted
    two_term = lambda n: (1 - D("0.01")) ** n + D(n) * D("0.01") * (1 - D("0.01")) ** (n - 1)
    n_one = next(n for n in range(300, 900) if two_term(n) <= ALPHA95)
    check("WE 16.3.2 n with one permitted failure at a 1 % bound and 95 %", n_one, 473)
    check("INV 16.3.2 473 satisfies the two-term condition", ge(ALPHA95, two_term(473)), 1)
    check("INV 16.3.2 472 does not satisfy it (473 is the smallest)", gt(two_term(472), ALPHA95), 1)
    check("WE 16.3.2 increase in the suite from one observed failure",
          q((D(473) - 299) / 299 * 100, 2), D("58.19"))
    check("WE 16.3.2 errors a 1 % bound admits at 56,400 uses", D("0.01") * ESTATE_RECS, 564)
    p_for_10 = D(10) / ESTATE_RECS
    check("WE 16.3.2 defect rate needed to bound production errors at ten", pc(p_for_10, 6),
          D("0.017730"))
    n_10 = ln(ALPHA95) / ln(1 - p_for_10)
    check("WE 16.3.2 test cases that bound would require", ceil(n_10), 16895)
    check("WE 16.3.2 months of production to accumulate 16,895 observations (about four)",
          q(D(16895) / ESTATE_RECS * 12, 1), D("3.6"))
    check("WE 16.3.2 days of production to accumulate 16,895 observations",
          q(D(16895) / ESTATE_RECS * 365, 0), 109)
    check("INV 16.3.2 monitoring reaches the suite size production testing cannot",
          gt(ESTATE_RECS, D(16895)), 1)
    check("INV 16.3.2 an affordable suite still admits errors at production volume",
          gt(D("0.01") * ESTATE_RECS, 10), 1)

    # ==================== 7c — WE 16.3.3 label bias ===========================================
    UNFLAGGED = ITEMS - D(1910)
    check("WE 16.3.3 unflagged population at T4", UNFLAGGED, 46090)
    SAMP, FOUND = D(1000), D(17)
    rate = FOUND / SAMP
    check("WE 16.3.3 blind-audit error rate", pc(rate, 2), D("1.70"))
    missed = q(UNFLAGGED * rate, 0)
    check("WE 16.3.3 extrapolated missed errors", missed, 784)
    TRUE_POP = D(960) + missed
    check("WE 16.3.3 true anomaly population", TRUE_POP, 1744)
    check("WE 16.3.3 assumed population understated by", pc(TRUE_POP / D(1200) - 1), D("45.3333"))
    check("WE 16.3.3 true recall", pc(D(960) / TRUE_POP), D("55.0459"))
    restated = D(950) * FP_C + missed * FN_C
    check("WE 16.3.3 restated total cost", restated, 288880)
    check("WE 16.3.3 restatement factor", q(restated / D(114800), 4), D("2.5164"))
    check("WE 16.3.3 false-negative cost understated by", (missed - D(240)) * FN_C, 174080)
    AUDIT_COST = SAMP * man_proc
    check("WE 16.3.3 cost of the blind audit (1,000 items at 8.5 minutes)", AUDIT_COST, 13600)
    check("WE 16.3.3 value of the missed errors it identified", missed * FN_C, 250880)
    check("WE 16.3.3 return on the blind audit", q(missed * FN_C / AUDIT_COST, 4), D("18.4471"))
    z = D("1.96")
    n_aud = z * z * PREV * (1 - PREV) / (D("0.01") ** 2)
    check("WE 16.3.3 audit sample for a +/-1 point bound at 95 % on a 2.5 % rate", ceil(n_aud),
          937)
    check("WE 16.3.3 illustrative bound on true recall at 60 % label completeness",
          pc(D("0.80") * D("0.60"), 0), 48)
    check("INV 16.3.3 true recall = labelled recall x label completeness",
          pc(D("0.80") * (D(1200) / TRUE_POP)), pc(D(960) / TRUE_POP))
    check("INV 16.3.3 the measured recall sits between the 48 % bound and the 80 % claim",
          1 if D("0.48") < D(960) / TRUE_POP < D("0.80") else 0, 1)
    check("INV 16.3.3 restating the population raises, never lowers, the cost",
          gt(restated, D(114800)), 1)
    POP_FACTOR = TRUE_POP / D(1200)
    check("WE 16.3.3 population factor", q(POP_FACTOR, 4), D("1.4533"))
    check("WE 16.3.3 T4->T5 marginal benefit scaled by the population factor",
          D(120) * FN_C * POP_FACTOR, 55808)
    check("INV 16.3.3 T4 remains optimal after scaling — the answer is a model change",
          gt(D(1950) * FP_C, D(120) * FN_C * POP_FACTOR), 1)
    check("WE 16.3.3 the audit sample actually run exceeds the sample the bound required",
          gt(SAMP, ceil(n_aud)), 1)

    # ==================== 7d — WE 16.3.4 inventory, tiering, revalidation =====================
    TOL = D(50000)
    p_m4 = q(1 - ACC_ITEM ** LB, 6)
    check("WE 16.3.4 M4's defect probability is 16.2.2's sweep complement", p_m4, D("0.885585"))
    INV = (("M1 Covenant-forecast model", 12, D("0.030"), HEAD, D("134077.68"),
            D("0.372918"), D("136.12")),
           ("M2 Payment anomaly detector", 48000, D("0.001"), FN_C, D("15360.00"),
            D("3.255208"), D("1188.15")),
           ("M3 Consumption forecaster", 12, D("0.050"), D(306432), D("183859.20"),
            D("0.271947"), D("99.26")),
           ("M4 Defined-term extractor", 4, p_m4, DEFN, D("2125404.00"),
            D("0.023525"), D("8.59")),
           ("M5 Covenant-certificate assembler", 4, D("0.020"), D(325000), D("26000.00"),
            D("1.923077"), D("701.92")))
    emvs = []
    for name, uses, p, cons, emv_want, yr_want, d_want in INV:
        emv = D(uses) * p * cons
        check(f"WE 16.3.4 {name} annual EMV", q(emv, 2), emv_want)
        check(f"WE 16.3.4 {name} revalidation interval (years)", q(TOL / emv, 6), yr_want)
        check(f"WE 16.3.4 {name} revalidation interval (days)", q(TOL / emv * 365, 2), d_want)
        emvs.append(emv)
    check("WE 16.3.4 M4's interval is shorter than the 91-day gap between its uses",
          gt(D(365) / 4, TOL / emvs[3] * 365), 1)
    check("WE 16.3.4 the interval between M4's four uses (days)", q(D(365) / 4, 2), D("91.25"))
    check("INV 16.3.4 the high-volume model carries the smallest annual EMV",
          1 if emvs[1] == min(emvs) else 0, 1)
    check("INV 16.3.4 M2 has the most uses of any model in the inventory",
          1 if max(u for _, u, _, _, _, _, _ in INV) == 48000 else 0, 1)
    check("INV 16.3.4 M1 and M3 need quarterly attention (interval under 182.5 days)",
          1 if all(TOL / e * 365 < D("182.5") for e in (emvs[0], emvs[2])) else 0, 1)
    check("INV 16.3.4 M2 and M5 compute to intervals above a year, so the policy floor binds",
          1 if all(TOL / e > 1 for e in (emvs[1], emvs[4])) else 0, 1)
    check("INV 16.3.4 halving the tolerance halves every interval",
          (TOL / 2) / emvs[0], (TOL / emvs[0]) / 2)
    check("INV 16.3.4 the two derivations agree — verify every use",
          1 if (TOL / emvs[3] * 365 < D(365) / 4) and (sweep < D("0.5")) else 0, 1)

    # ==================== MCQs — KA 16.3 ======================================================
    check("MCQ 16.3-A answer B", ceil(n_1pc), 299)
    check("MCQ 16.3-A rationale rule of three at 3/p", D(3) / D("0.01"), 300)
    check("MCQ 16.3-A distractor A (reciprocal of the bound)", D(1) / D("0.01"), 100)
    check("MCQ 16.3-A distractor A confidence at n = 100", pc(1 - (1 - D("0.01")) ** 100, 0), 63)
    check("MCQ 16.3-A distractor D (99 % confidence)",
          ceil(ln(ALPHA99) / ln(1 - D("0.01"))), 459)
    check("MCQ 16.3-B answer B errors admitted", D("0.01") * ESTATE_RECS, 564)
    check("MCQ 16.3-B answer B cases a ten-error bound would need", ceil(n_10), 16895)
    check("MCQ 16.3-C answer B true population", TRUE_POP, 1744)
    check("MCQ 16.3-C answer B true recall", pc(D(960) / TRUE_POP), D("55.0459"))
    check("MCQ 16.3-C answer B restated cost", restated, 288880)
    check("MCQ 16.3-C rationale scaled T4->T5 benefit", D(120) * FN_C * POP_FACTOR, 55808)
    check("MCQ 16.3-C distractor C rebuttal — 937 sufficed", ceil(n_aud), 937)
    check("MCQ 16.3-D answer B EMV", q(D(4) * p_m4 * DEFN, 0), 2125404)
    check("MCQ 16.3-D answer B interval (years)", q(TOL / (D(4) * p_m4 * DEFN), 6), D("0.023525"))
    check("MCQ 16.3-D answer B interval (days)", q(TOL / (D(4) * p_m4 * DEFN) * 365, 2),
          D("8.59"))
    check("MCQ 16.3-D distractor D (the payment detector's interval)", q(TOL / emvs[1], 2),
          D("3.26"))
    check("MCQ 16.3-D distractor D quoted EMV", emvs[1], 15360)

    # ==================== 8a — WE 16.4.1 the deployment choice ================================
    MANAGED, PRIVATE = FIXED, D(410000)
    P_MAN, P_PRIV, BREACH = D("0.0080"), D("0.0015"), D(6500000)
    dP = P_MAN - P_PRIV
    check("WE 16.4.1 probability differential", pc(dP, 2), D("0.65"))
    check("WE 16.4.1 EMV avoided by the private deployment", dP * BREACH, 42250)
    dC = PRIVATE - MANAGED
    check("WE 16.4.1 cost differential", dC, 262000)
    check("WE 16.4.1 the private deployment is unjustified by", dC - dP * BREACH, 219750)
    check("WE 16.4.1 breakeven breach consequence", q(dC / dP, 0), 40307692)
    check("INV 16.4.1 EMV avoided falls short of the cost differential",
          gt(dC, dP * BREACH), 1)
    check("INV 16.4.1 the breakeven consequence x the probability differential = the cost gap",
          q((dC / dP) * dP, 0), dC)
    check("INV 16.4.1 the assessed consequence is well under the breakeven",
          1 if BREACH < dC / dP else 0, 1)

    # ==================== 8b — WE 16.4.2 the dual-approval threshold ==========================
    PAYS, PAY_VAL = D(4800), D(68000000)
    APPROVER = D(15) * RATE_MIN
    check("WE 16.4.2 second approver's cost a payment", APPROVER, 24)
    BAD, CATCH = D("0.0020"), D("0.85")
    CAUGHT = BAD * CATCH
    check("WE 16.4.2 caught rate (share of payment value protected)", pc(CAUGHT, 2), D("0.17"))
    VSTAR = APPROVER / CAUGHT
    check("WE 16.4.2 dual-approval threshold", q(VSTAR, 2), D("14117.65"))
    check("WE 16.4.2 threshold rounded for the policy", q(VSTAR, 0), 14118)
    ABOVE_N, ABOVE_V = D(1150), D(63400000)
    BELOW_N, BELOW_V = PAYS - ABOVE_N, PAY_VAL - ABOVE_V
    check("WE 16.4.2 sub-threshold payment count", BELOW_N, 3650)
    check("WE 16.4.2 sub-threshold payment value", BELOW_V, 4600000)
    check("WE 16.4.2 blanket policy cost", PAYS * APPROVER, 115200)
    check("WE 16.4.2 blanket policy loss avoided", CAUGHT * PAY_VAL, 115600)
    NET_BLANKET = CAUGHT * PAY_VAL - PAYS * APPROVER
    check("WE 16.4.2 blanket policy net value", NET_BLANKET, 400)
    check("WE 16.4.2 thresholded policy cost", ABOVE_N * APPROVER, 27600)
    check("WE 16.4.2 thresholded policy loss avoided", CAUGHT * ABOVE_V, 107780)
    NET_THRESH = CAUGHT * ABOVE_V - ABOVE_N * APPROVER
    check("WE 16.4.2 thresholded policy net value", NET_THRESH, 80180)
    check("WE 16.4.2 sub-threshold approval cost", BELOW_N * APPROVER, 87600)
    check("WE 16.4.2 sub-threshold loss avoided", CAUGHT * BELOW_V, 7820)
    DESTROYED = BELOW_N * APPROVER - CAUGHT * BELOW_V
    check("WE 16.4.2 value destroyed below the threshold", DESTROYED, 79780)
    check("INV 16.4.2 blanket = thresholded less the value destroyed below the threshold",
          NET_THRESH - DESTROYED, NET_BLANKET)
    check("INV 16.4.2 the targeted control beats the blanket control",
          gt(NET_THRESH, NET_BLANKET), 1)
    check("INV 16.4.2 at V* the loss avoided on one payment equals the approver's cost",
          CAUGHT * VSTAR, APPROVER)
    check("INV 16.4.2 the sub-threshold population averages below the threshold",
          1 if BELOW_V / BELOW_N < VSTAR else 0, 1)
    check("INV 16.4.2 the above-threshold population averages above the threshold",
          1 if ABOVE_V / ABOVE_N > VSTAR else 0, 1)
    check("WE 16.4.2 blanket benefit-to-cost ratio is 1.00 to three significant figures",
          q(CAUGHT * PAY_VAL / (PAYS * APPROVER), 2), D("1.00"))
    check("WE 16.4.2 a caught rate near nil makes the whole blanket cost waste",
          PAYS * APPROVER, 115200)

    # ==================== 8c — WE 16.4.3 approval latency =====================================
    BENEFIT = D(416000)
    check("WE 16.4.3 committee expected wait (days)", ew(60, 10), 40)
    check("WE 16.4.3 delegated panel expected wait (days)", ew(7, 2), D("5.5"))
    DAILY = BENEFIT / 365
    check("WE 16.4.3 daily value of the change", q(DAILY, 4), D("1139.7260"))
    check("WE 16.4.3 value forgone through the committee", q(ew(60, 10) * DAILY, 2),
          D("45589.04"))
    check("WE 16.4.3 value forgone through the delegated panel", q(ew(7, 2) * DAILY, 2),
          D("6268.49"))
    check("WE 16.4.3 price of the governance design per change",
          q((ew(60, 10) - ew(7, 2)) * DAILY, 2), D("39320.55"))
    check("INV 16.4.3 the committee's wait, and its cost, exceed the panel's",
          1 if ew(60, 10) > ew(7, 2) and ew(60, 10) * DAILY > ew(7, 2) * DAILY else 0, 1)
    check("INV 16.4.3 forgone value is linear in the expected wait",
          (ew(60, 10) * DAILY) / (ew(7, 2) * DAILY), ew(60, 10) / ew(7, 2))
    check("INV 16.4.3 the top-tier exposure justifies the committee's latency cost",
          gt(HEAD, ew(60, 10) * DAILY), 1)

    # ==================== MCQs — KA 16.4 ======================================================
    check("MCQ 16.4-A answer B", q(VSTAR, 0), 14118)
    check("MCQ 16.4-A distractor A (catch rate omitted)", APPROVER / BAD, 12000)
    check("MCQ 16.4-A distractor C (bad-payment rate decimal misplaced to 0.02 %)",
          APPROVER / D("0.0002"), 120000)
    check("MCQ 16.4-A distractor D (the 15 % missed instead of the 85 % caught)",
          APPROVER / (BAD * (1 - CATCH)), 80000)
    check("MCQ 16.4-B answer C thresholded net", NET_THRESH, 80180)
    check("MCQ 16.4-B answer C blanket net", NET_BLANKET, 400)
    check("MCQ 16.4-B answer C sub-threshold cost and protection",
          BELOW_N * APPROVER - CAUGHT * BELOW_V, 79780)
    check("MCQ 16.4-C answer B breakeven consequence", q(dC / dP, 0), 40307692)
    check("MCQ 16.4-C distractor A quoted EMV avoided", dP * BREACH, 42250)
    check("MCQ 16.4-C distractor D quoted probability differential", pc(dP, 2), D("0.65"))
    check("MCQ 16.4-D answer B", q(ew(60, 10) * DAILY, 0), 45589)
    check("MCQ 16.4-D distractor A (the panel's 5.5-day wait)", q(ew(7, 2) * DAILY, 0), 6268)
    check("MCQ 16.4-D distractor C (full 60-day interval, no M/2 and no L)",
          q(D(60) * DAILY, 0), 68384)

    # ==================== 9a — 16.A.1 the authorisation boundary ==============================
    check("16.A.1 approver time saved by removing the thresholded control",
          ABOVE_N * APPROVER, 27600)
    check("16.A.1 expected loss added by removing it", CAUGHT * ABOVE_V, 107780)
    check("16.A.1 net destruction", CAUGHT * ABOVE_V - ABOVE_N * APPROVER, 80180)
    check("INV 16.A.1 removing the control destroys exactly the control's net value",
          CAUGHT * ABOVE_V - ABOVE_N * APPROVER, NET_THRESH)
    check("16.A.1 expected loss with blanket approval in place", BAD * PAY_VAL * (1 - CATCH),
          20400)
    check("16.A.1 expected loss with no approval at all", BAD * PAY_VAL, 136000)
    check("INV 16.A.1 removing approval multiplies expected loss by 1/(1 - catch rate)",
          (BAD * PAY_VAL) / (BAD * PAY_VAL * (1 - CATCH)), 1 / (1 - CATCH))
    check("16.A.1 an agent may release below the derived threshold", q(VSTAR, 0), 14118)

    # ==================== 9b — 16.A.2 the automation you cannot switch off ====================
    fb_hours = ESTATE_RECS * MAN_MIN / 60
    check("16.A.2 hours a year to restore manual review of 56,400 records", fb_hours, 7990)
    check("16.A.2 full-time equivalents at 1,600 productive hours", fb_hours / 1600, D("4.99375"))
    QVOL = ESTATE_RECS / 4
    check("16.A.2 a quarter's record volume", QVOL, 14100)
    check("16.A.2 a quarter's committed fixed cost", FIXED / 4, 37000)
    check("16.A.2 a quarter's manual review at 19.36 all-in", QVOL * MAN_ALL, 272976)
    susp = FIXED / 4 + QVOL * MAN_ALL
    check("16.A.2 total cost of a suspended quarter", susp, 309976)
    auto_q = FIXED / 4 + QVOL * AUTO_ALL
    check("16.A.2 cost of an automated quarter", auto_q, 168976)
    check("16.A.2 incremental cost of one quarter's suspension", susp - auto_q, 141000)
    check("INV 16.A.2 the suspension increment is the quarter's volume x the per-record advantage",
          susp - auto_q, QVOL * ADV)
    check("16.4.4 the rollback figure quoted in the governance frame", susp - auto_q, 141000)

    # ==================== 9c — Case study A ===================================================
    check("Case A platform cost at Kestrel's 9,400", FIXED + KESTREL_RECS * AUTO_ALL, 235984)
    check("Case A manual cost at Kestrel's 9,400", KESTREL_RECS * MAN_ALL, 181984)
    check("Case A annual loss at the sponsoring asset",
          (FIXED + KESTREL_RECS * AUTO_ALL) - KESTREL_RECS * MAN_ALL, 54000)
    check("Case A restated breakeven", FIXED / ADV, 14800)
    check("Case A the original case's implied breakeven", q(BE_NOERR, 0), 11783)
    check("Case A flattering percentage", pc((FIXED / ADV - BE_NOERR) / (FIXED / ADV)),
          D("20.3822"))
    check("INV Case A even the flattered breakeven exceeded Kestrel's volume",
          gt(BE_NOERR, KESTREL_RECS), 1)
    check("Case A saving at the six-asset estate", net(ESTATE_RECS), 416000)
    check("Case A vendor threshold T2 accuracy", PRINTED["T2"][2], D("98.5729"))
    check("Case A vendor threshold T2 alerts", PRINTED["T2"][3], 925)
    check("Case A vendor threshold T2 cost", PRINTED["T2"][4], 161800)
    check("Case A cost-minimising threshold cost", min(costs), 114800)
    check("Case A cost-minimising threshold alerts", PRINTED["T4"][3], 1910)
    check("Case A cost-minimising threshold precision", PRINTED["T4"][1], D("50.2618"))
    check("Case A saving from moving the threshold", costs[imax] - min(costs), 47000)
    check("Case A break-even posterior", pc(POST, 1), D("12.5"))
    check("Case A audit cost", AUDIT_COST, 13600)
    check("Case A audit findings extrapolated", missed, 784)
    check("Case A the missed errors the detector's own arithmetic assumed (FN at T4)",
          D(1200) - D(960), 240)
    check("Case A true population", TRUE_POP, 1744)
    check("Case A population understatement", pc(TRUE_POP / D(1200) - 1), D("45.3333"))
    check("Case A restated recall", pc(D(960) / TRUE_POP), D("55.0459"))
    check("Case A restated cost", restated, 288880)
    check("Case A restatement factor", q(restated / D(114800), 4), D("2.5164"))
    check("Case A false-negative understatement", (missed - D(240)) * FN_C, 174080)
    check("Case A audit sample floor", ceil(n_aud), 937)
    check("Case A audit return", q(missed * FN_C / AUDIT_COST, 4), D("18.4471"))
    check("Case A scaled marginal benefit", D(120) * FN_C * POP_FACTOR, 55808)
    check("Case A unchanged marginal investigation cost", D(1950) * FP_C, 78000)
    check("Case A total annual improvement from rescoping",
          net(ESTATE_RECS) - net(KESTREL_RECS), 470000)
    check("Case A total annual improvement from retuning", costs[imax] - min(costs), 47000)

    # ==================== 9d — Case study B ===================================================
    B_DEBT, B_DS = D(130000000), D(17000000)
    B_REP, B_EXEC = D(22780000), D(20230000)
    check("Case B reported DSCR", q(B_REP / B_DS, 4), D("1.3400"))
    check("Case B DSCR on the executed definition", q(B_EXEC / B_DS, 4), D("1.1900"))
    check("Case B CFADS difference between the two definitions", B_REP - B_EXEC, 2550000)
    check("Case B CFADS the 1.25x covenant requires", D("1.25") * B_DS, 21250000)
    CURE = D("1.25") * B_DS - B_EXEC
    check("Case B shortfall against the covenant", CURE, 1020000)
    check("Case B CFADS at the 1.20x lock-up trigger", D("1.20") * B_DS, 20400000)
    check("Case B amount by which the lock-up was crossed", D("1.20") * B_DS - B_EXEC, 170000)
    check("INV Case B the reported figure passes and the executed figure fails the covenant",
          1 if B_REP / B_DS > D("1.25") > B_EXEC / B_DS else 0, 1)
    check("INV Case B the executed figure also breaches the lock-up",
          1 if B_EXEC / B_DS < D("1.20") else 0, 1)
    check("Case B sweep probability on 26 load-bearing terms", q(sweep, 6), D("0.114415"))
    check("Case B chance at least one load-bearing term is wrong", pc(1 - sweep), D("88.5585"))
    WAIVER = D("0.0025") * B_DEBT
    check("Case B waiver fee at 25 basis points", WAIVER, 325000)
    DIRECT = CURE + WAIVER
    check("Case B total direct cost", DIRECT, 1345000)
    check("Case B cost of the verification that would have prevented it", VERIFY, 3120)
    check("Case B direct cost as a multiple of the verification", q(DIRECT / VERIFY, 4),
          D("431.0897"))
    check("Case B M4 annual EMV", q(D(4) * p_m4 * DEFN, 0), 2125404)
    check("Case B M4 revalidation interval (days)", q(TOL / (D(4) * p_m4 * DEFN) * 365, 2),
          D("8.59"))
    check("Case B the waiver fee is the M5 consequence used in the inventory", WAIVER, 325000)

    # ==================== 9e — Executive perspective and summary ==============================
    check("Executive perspective committee latency cost", q(ew(60, 10) * DAILY, 0), 45589)
    check("Executive perspective panel latency cost", q(ew(7, 2) * DAILY, 0), 6268)
    check("Executive perspective rollback cost", susp - auto_q, 141000)
    check("Executive perspective fallback FTE", fb_hours / 1600, D("4.99375"))
    check("Summary sweep probability as a percentage", pc(sweep), D("11.4415"))
    check("Summary spine NPV", q(SAVE * af(DISC, 10) - BUILD, 0), 1221674)
    RETURNS = [DEFN / VERIFY, EMV_DEF / ((M_REV - M_REV_HABIT) * RATE_SR),
               missed * FN_C / AUDIT_COST, DIRECT / VERIFY]
    check("Summary the smallest verification return is about eighteen times", q(min(RETURNS), 4),
          D("18.4471"))
    check("Summary the largest verification return is about four hundred and thirty times",
          q(max(RETURNS), 4), D("431.0897"))
    check("INV Summary every verification return lies between 18 and 430 times its cost",
          1 if all(D(18) < r < D(432) for r in RETURNS) else 0, 1)

    # ==================== 10 — Calculation exercises ==========================================
    # Exercise 16.1
    e1_man = D(7) * RATE_MIN + D("0.022") * D(500)
    check("Ex 16.1 manual processing cost", D(7) * RATE_MIN, D("11.20"))
    check("Ex 16.1 manual error cost", D("0.022") * D(500), D("11.00"))
    check("Ex 16.1 manual all-in", e1_man, D("22.20"))
    e1_auto = D("0.95") + D("0.035") * D(500)
    check("Ex 16.1 automated error cost", D("0.035") * D(500), D("17.50"))
    check("Ex 16.1 automated all-in", e1_auto, D("18.45"))
    check("Ex 16.1 advantage", e1_man - e1_auto, D("3.75"))
    check("Ex 16.1 breakeven volume", D(96000) / (e1_man - e1_auto), 25600)
    e1_bad = D(96000) / (D("11.20") - D("0.95"))
    check("Ex 16.1 advantage with the error terms omitted", D("11.20") - D("0.95"), D("10.25"))
    check("Ex 16.1 apparent breakeven with the error terms omitted", q(e1_bad, 0), 9366)
    check("Ex 16.1 how far too low the apparent breakeven is",
          pc((D(25600) - e1_bad) / D(25600), 1), D("63.4"))
    check("INV Ex 16.1 the automated error rate is again the higher of the two",
          gt(D("0.035"), D("0.022")), 1)

    # Exercise 16.2
    E2_N, E2_BAD, E2_FP, E2_FN = D(30000), D(900), D(60), D(900)
    E2 = (("A", 450, 90, D("98.2000"), D(410400)), ("B", 675, 300, D("98.2500"), D(220500)),
          ("C", 855, 1500, D("94.8500"), D(130500)))
    e2_costs, e2_accs = [], []
    for lab, tp, fp, acc_want, cost_want in E2:
        tp, fp = D(tp), D(fp)
        fn, tn = E2_BAD - tp, (E2_N - E2_BAD) - fp
        check(f"Ex 16.2 {lab} accuracy", pc((tp + tn) / E2_N), acc_want)
        check(f"Ex 16.2 {lab} total cost", fp * E2_FP + fn * E2_FN, cost_want)
        e2_costs.append(fp * E2_FP + fn * E2_FN)
        e2_accs.append((tp + tn) / E2_N)
    check("Ex 16.2 the cost-minimising threshold is C", e2_costs.index(min(e2_costs)), 2)
    check("Ex 16.2 the accuracy-maximising threshold is B", e2_accs.index(max(e2_accs)), 1)
    check("Ex 16.2 annual cost of choosing B on accuracy", e2_costs[1] - min(e2_costs), 90000)
    check("Ex 16.2 B->C additional true positives", D(855 - 675), 180)
    check("Ex 16.2 B->C additional false positives", D(1500 - 300), 1200)
    check("Ex 16.2 B->C marginal precision", pc(D(180) / D(180 + 1200)), D("13.0435"))
    check("Ex 16.2 break-even posterior", pc(E2_FP / E2_FN), D("6.6667"))
    check("Ex 16.2 break-even posterior is one in fifteen", E2_FN / E2_FP, 15)
    check("Ex 16.2 B->C marginal benefit", D(180) * E2_FN, 162000)
    check("Ex 16.2 B->C marginal cost", D(1200) * E2_FP, 72000)
    check("INV Ex 16.2 B->C net reconciles to the fall in total cost",
          D(180) * E2_FN - D(1200) * E2_FP, e2_costs[1] - e2_costs[2])
    check("INV Ex 16.2 A->B net reconciles to the fall in total cost",
          D(675 - 450) * E2_FN - D(300 - 90) * E2_FP, e2_costs[0] - e2_costs[1])
    check("INV Ex 16.2 the marginal precision of the accepted step exceeds the posterior",
          gt(D(180) / D(1380), E2_FP / E2_FN), 1)

    # Exercise 16.3
    check("Ex 16.3 exact n for a 2 % bound at 95 %", q(ln(ALPHA95) / ln(D("0.98")), 4),
          D("148.2837"))
    check("Ex 16.3 test cases for a 2 % bound at 95 %", ceil(ln(ALPHA95) / ln(D("0.98"))), 149)
    check("Ex 16.3 exact n for a 2 % bound at 99 %", q(ln(ALPHA99) / ln(D("0.98")), 4),
          D("227.9482"))
    check("Ex 16.3 test cases for a 2 % bound at 99 %", ceil(ln(ALPHA99) / ln(D("0.98"))), 228)
    check("Ex 16.3 the rule-of-three approximation at a 2 % bound", D(3) / D("0.02"), 150)
    check("Ex 16.3 the 1/p answer the common error gives", D(1) / D("0.02"), 50)

    # Exercise 16.4
    e4 = D("0.95") ** 18
    check("Ex 16.4 probability all eighteen items are correct", q(e4, 6), D("0.397214"))
    check("Ex 16.4 as a percentage", pc(e4), D("39.7214"))
    check("Ex 16.4 probability at least one is wrong", pc(1 - e4), D("60.2786"))
    e4_req = (ln(D("0.99")) / 18).exp()
    check("Ex 16.4 per-item accuracy a 99 % sweep of eighteen requires", pc(e4_req), D("99.9442"))
    check("INV Ex 16.4 the required accuracy reproduces the sweep confidence",
          q(e4_req ** 18, 4), D("0.9900"))

    # Exercise 16.5
    E5_N, E5_VAL = D(3600), D(52000000)
    e5_appr = D("12.5") * RATE_MIN
    check("Ex 16.5 approver cost a payment", e5_appr, 20)
    e5_caught = D("0.0025") * D("0.80")
    check("Ex 16.5 caught rate", e5_caught, D("0.0020"))
    check("Ex 16.5 dual-approval threshold", e5_appr / e5_caught, 10000)
    check("Ex 16.5 blanket cost", E5_N * e5_appr, 72000)
    check("Ex 16.5 blanket loss avoided", e5_caught * E5_VAL, 104000)
    e5_blanket = e5_caught * E5_VAL - E5_N * e5_appr
    check("Ex 16.5 blanket net value", e5_blanket, 32000)
    E5_AN, E5_AV = D(820), D(48900000)
    check("Ex 16.5 thresholded cost", E5_AN * e5_appr, 16400)
    check("Ex 16.5 thresholded loss avoided", e5_caught * E5_AV, 97800)
    e5_thresh = e5_caught * E5_AV - E5_AN * e5_appr
    check("Ex 16.5 thresholded net value", e5_thresh, 81400)
    check("Ex 16.5 sub-threshold payment count", E5_N - E5_AN, 2780)
    check("Ex 16.5 sub-threshold payment value", E5_VAL - E5_AV, 3100000)
    check("Ex 16.5 sub-threshold cost", (E5_N - E5_AN) * e5_appr, 55600)
    check("Ex 16.5 sub-threshold loss avoided", e5_caught * (E5_VAL - E5_AV), 6200)
    e5_dest = (E5_N - E5_AN) * e5_appr - e5_caught * (E5_VAL - E5_AV)
    check("Ex 16.5 value destroyed below the threshold", e5_dest, 49400)
    check("INV Ex 16.5 blanket = thresholded less the value destroyed", e5_thresh - e5_dest,
          e5_blanket)
    check("INV Ex 16.5 at the threshold the avoided loss equals the approver's cost",
          e5_caught * (e5_appr / e5_caught), e5_appr)
    check("INV Ex 16.5 the sub-threshold population averages below the threshold",
          1 if (E5_VAL - E5_AV) / (E5_N - E5_AN) < e5_appr / e5_caught else 0, 1)
    check("INV Ex 16.5 the above-threshold population averages above the threshold",
          1 if E5_AV / E5_AN > e5_appr / e5_caught else 0, 1)

    # ==================== 11 — cross-domain invariants the chapter claims ====================
    check("INV domain every stated headroom comparison uses Domain 10's 372,438", HEAD, 372438)
    check("INV domain the automation, threshold and control results are all marginal quantities",
          1 if (FIXED / ADV > 0 and mp34 > POST > mp45 and NET_THRESH > NET_BLANKET) else 0, 1)
