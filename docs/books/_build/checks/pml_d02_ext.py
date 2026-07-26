"""PML-AI Domain 2 (extension) — Strategy, Selection and Business Alignment: golden checks for the
NEW material only.

The domain's ORIGINAL numbers (Meridian's 979,200 / 685,440 potential and honest benefit, the flat and
ramped PVs of 5,847,096 and 3,732,898, the 2,114,198 overstatement at 158.6 %, the 41.05 % headline
breakeven, the four candidates' weighted scores, the 3-unit constrained selection, the forward NPV of
(120,000) at the reset point and Exercises 2.1–2.4) are verified in `verify_formulas.py` and are NOT
duplicated here. This module covers what the depth expansion added, and it derives each figure from its
definition rather than from its printed output:

    alignment      WE 2.1.1 — funded shares on both denominators, the index as a sum of minima, the
                   four gaps in points and money, the reallocation distance, and Fig 2.1.1's panel.
                   The identity `reallocation = (1 − index) × denominator` and `deficits = surpluses`
                   is PROVED by sweeping alternative portfolios, not restated. The intake-cadence
                   passage is rebuilt from ctx's ew(M, L) and MERIDIAN_COST_OF_DELAY.
    decay          WE 2.1.3 — the survival curve, the half-life SOLVED and then verified by
                   substitution (S(t½) = 0.5), the expected misaligned spend with and without a
                   re-test built from the detection tree, the per-gate breakeven, and the three
                   named errors behind MCQ 2.1-D's distractors.
    two bases      WE 2.2.2's rewritten Interpretation — the ramp fraction 0.912027 and its
                   independence of the steady-state level (proved by sweep), the ramped-basis
                   breakeven, and the conversion `flat-equivalent = 0.912027 × ramped-basis`.
    counterfactual WE 2.2.2b — both avoided-cost present values from discount factors and annuity
                   differences, the restated NPV and breakeven, Meridian's NPV at 40 % on both
                   baselines, and MCQ 2.2-E's undiscounted and sign-error distractors.
    constraint     WE 2.2.3b — V(c) for c = 1..6 solved by ENUMERATION over subsets (not read from
                   the manuscript table), the marginal values, the three block decisions from a base
                   of 3 units, and the non-monotonicity that makes the block decision necessary.
    weights        WE 2.2.3c — the flip point solved from the score differences and verified by
                   re-scoring at the flipped weights, criterion influence, the compression ratio, and
                   the fit-to-deliverability invariance proved across a sweep of shifts.
    attribution    WE 2.3.2 — the difference-in-differences, all three claim bases, the over-claim
                   share proved invariant to rate/weeks/clinics, the corrected potential and all four
                   breakevens that Fig 2.3.2 plots.
    assumptions    WE 2.3.4 — every impact rebuilt from its own driver, every EMV, the totals, the
                   exposure ratio, the test-order ranking and the free-test subtotal.
    kill criterion WE 2.4.3 — the forward breakeven, the three state NPVs, both policy expectations,
                   the saved/destroyed decomposition, the net identity `A − B = saved − destroyed`,
                   the breakeven signal quality and the flagged-population mix, plus Fig 2.4.1.
    real options   WE 2.A.1 — all six branch NPVs from their cash flows, the three expectations, the
                   price/option decomposition, the breakeven P(Low) solved from the two expectation
                   lines, and the premium ceiling derived from the one-for-one dependence of E[S] on
                   the phase-1 cost.
    the rest       Industry variations, both case studies' new arithmetic, and Exercises 2.5–2.9
                   including every named common error.

Master-thread values come from ctx and are never re-typed: MERIDIAN_BASELINE (USD 2,400,000),
MERIDIAN_BENEFIT_FULL (USD 979,200), MERIDIAN_BENEFIT_70PCT (USD 685,440), MERIDIAN_COST_OF_DELAY
(USD 14,280 a week), af(r, n) for the annuity factors and ew(M, L) for Domain 3's governance latency.
The domain's own established figures that the new material leans on — the 7 % rate, the eight-year
horizon, the 40/60/70 ramp, the flat and ramped PVs and the 41.0460 % flat-equivalent breakeven — are
rebuilt from the primitives below, so a drift in any of them fails here rather than passing silently.
"""


def run(ctx):
    check, D, af, ew = ctx["check"], ctx["D"], ctx["af"], ctx["ew"]
    COST = ctx["MERIDIAN_BASELINE"]              # USD 2,400,000 approved
    POT = ctx["MERIDIAN_BENEFIT_FULL"]           # USD 979,200 full-potential annual benefit
    B70 = ctx["MERIDIAN_BENEFIT_70PCT"]          # USD 685,440 at the realistic 70 % adoption
    COD = ctx["MERIDIAN_COST_OF_DELAY"]          # USD 14,280 a week at 70 % adoption

    def n(x, p="1"):                             # round a computed value to its printed precision
        return D(x).quantize(D(p))

    P2, P4, P6 = "0.01", "0.0001", "0.000001"

    # ---- the domain's own primitives, rebuilt ----------------------------------------------------
    R = D("0.07")
    AF8, AF4 = af(R, 8), af(R, 4)
    check("D2 primitives AF(0.07,8)", n(AF8, P6), D("5.971299"))
    check("D2 primitives AF(0.07,4)", n(AF4, P6), D("3.387211"))
    FLAT = POT * AF8                                       # 5,847,096 flat full-potential PV
    # Display-rounding boundary: the exact product is 5,847,095.4973, which rounds down, while the
    # manuscript's own basis (the printed AF of 5.971299) gives 5,847,095.98 and so prints 5,847,096.
    # Tolerance widened to 1 for that one figure; every value derived from FLAT below uses the exact
    # annuity factor and is checked at its own printed precision.
    check("D2 primitives flat full-potential PV (display-rounding boundary, tol 1)", n(FLAT),
          5847096, D(1))
    check("D2 primitives flat PV on the manuscript's stated AF of 5.971299",
          n(POT * D("5.971299")), 5847096)
    RAMP_A = [D("0.40"), D("0.60")] + [D("0.70")] * 6
    RAMPED = sum(POT * a / (1 + R) ** (i + 1) for i, a in enumerate(RAMP_A))
    check("D2 primitives ramped PV of benefits", n(RAMPED), 3732898)
    check("D2 primitives ramped NPV", n(RAMPED - COST), 1332898)
    PER = RAMPED / D("0.70")                               # PV per unit of sustained adoption
    check("D2 primitives PV per unit of sustained adoption (2.2.2, used by 2.3.4/2.4.3/2.A.1)",
          n(PER), 5332711)
    BE_FLAT = COST / FLAT * 100                            # 41.0460 % flat-equivalent breakeven
    check("D2 primitives flat-equivalent breakeven adoption", n(BE_FLAT, P4), D("41.0460"))
    check("D2 primitives ramp profile rebuilds Domain 1's honest annual benefit", POT * D("0.70"),
          B70)
    check("D2 primitives flat NPV as advertised (display-rounding boundary, tol 1)",
          n(FLAT - COST), 3447096, D(1))
    check("D2 primitives annual benefit at the achieved 40 % adoption (case study A)",
          POT * D("0.40"), 391680)

    # =============================================================================================
    # KA 2.1.1 — Worked example 2.1.1, the alignment index, and Fig 2.1.1
    # =============================================================================================
    MAP, TOT = D(18000000), D(30000000)
    SPEND = [D(4800000), D(6600000), D(1800000), D(4800000)]
    SUST = D(12000000)
    DECL = [D("0.40"), D("0.25"), D("0.20"), D("0.15")]
    check("WE 2.1.1 setup: mapped spend plus sustainment is the whole budget", sum(SPEND) + SUST,
          TOT)
    check("WE 2.1.1 setup: declared weights sum to one", sum(DECL), 1)

    for name, w, printed in (("access", DECL[0], D("40.0000")), ("safety", DECL[1], D("25.0000")),
                             ("digital", DECL[2], D("20.0000")), ("cost efficiency", DECL[3],
                                                                 D("15.0000"))):
        check(f"WE 2.1.1 result table declared weight, {name}", n(w * 100, P4), printed)
    sh = [s / MAP for s in SPEND]
    for name, v, printed in (("access", sh[0], D("26.6667")), ("safety", sh[1], D("36.6667")),
                             ("digital", sh[2], D("10.0000")), ("cost efficiency", sh[3],
                                                                D("26.6667"))):
        check(f"WE 2.1.1 funded share of mapped spend, {name} (step 3)", n(v * 100, P4), printed)
    IDX = sum(min(d, f) for d, f in zip(DECL, sh))
    check("WE 2.1.1 alignment index on mapped spend (step 4)", n(IDX * 100, P4), D("76.6667"))

    sh2 = [s / TOT for s in SPEND]
    for name, v, printed in (("access", sh2[0], 16), ("safety", sh2[1], 22),
                             ("digital", sh2[2], 6), ("cost efficiency", sh2[3], 16)):
        check(f"WE 2.1.1 funded share of total discretionary spend, {name} (step 3)", v * 100,
              printed)
    IDX2 = sum(min(d, f) for d, f in zip(DECL, sh2))
    check("WE 2.1.1 alignment index on total discretionary spend (step 4)", n(IDX2 * 100, P4),
          D("59.0000"))
    check("WE 2.1.1 the two indices differ by (interpretation 1)", n((IDX - IDX2) * 100, P4),
          D("17.6667"))
    check("WE 2.1.1 unmapped sustainment share (interpretation 1, Fig 2.1.1)",
          n(SUST / TOT * 100, P4), D("40.0000"))
    check("WE 2.1.1 apparent misalignment on total spend, the unfair reading (interpretation 1)",
          n(100 - IDX2 * 100, P4), D("41.0000"))

    gaps = [d - f for d, f in zip(DECL, sh)]
    for name, g, pts, money in (("access", gaps[0], D("-13.3333"), D(-2400000)),
                                ("safety", gaps[1], D("11.6667"), D(2100000)),
                                ("digital", gaps[2], D("-10.0000"), D(-1800000)),
                                ("cost efficiency", gaps[3], D("11.6667"), D(2100000))):
        check(f"WE 2.1.1 result table gap in points, {name}", n(-g * 100, P4), pts)
        check(f"WE 2.1.1 result table gap in money, {name}", n(-g * MAP), money)
    REALLOC = sum(g * MAP for g in gaps if g > 0)
    check("WE 2.1.1 reallocation distance (step 4, Fig 2.1.1)", REALLOC, 4200000)
    check("WE 2.1.1 deficits equal surpluses exactly (step 4)",
          sum(-g * MAP for g in gaps if g < 0), REALLOC)
    check("WE 2.1.1 reallocation via the identity (1 - index) x denominator (interpretation 3)",
          (1 - IDX) * MAP, REALLOC)
    check("WE 2.1.1 the identity's coefficient, 0.233333 (interpretation 3)", n(1 - IDX, P6),
          D("0.233333"))
    check("WE 2.1.1 the index as a decimal, 0.766667 (interpretation 3)", n(IDX, P6),
          D("0.766667"))
    check("WE 2.1.1 the access deficit as a decimal, 0.133333 (MCQ 2.1-C rationale)",
          n(gaps[0], P6), D("0.133333"))
    check("WE 2.1.1 reallocation distance as a multiple of Meridian's cost (interpretation 2)",
          REALLOC / COST, D("1.75"))
    check("WE 2.1.1 access deficit is exactly one Meridian (interpretation 2)", gaps[0] * MAP, COST)
    check("WE 2.1.1 digital funded at this share of its declared weight (interpretation 2)",
          n(sh[2] / DECL[2] * 100, P4), D("50.0000"))
    check("MCQ 2.1-C distractor A, deficits added to surpluses", REALLOC * 2, 8400000)
    check("MCQ 2.1-C distractor C, the access deficit alone", gaps[0] * MAP, 2400000)
    check("MCQ 2.1-C distractor D, the unmapped sustainment line", SUST, 12000000)

    # Invariant: whenever declared weights and funded shares each sum to one, the positive gaps equal
    # the negative gaps and the reallocation distance is (1 - index) x denominator with no residue.
    for alt in ([D(9000000), D(4500000), D(3000000), D(1500000)],
                [D(1000000), D(1000000), D(8000000), D(8000000)],
                [D(7200000), D(4500000), D(3600000), D(2700000)]):
        a_sh = [s / sum(alt) for s in alt]
        a_idx = sum(min(d, f) for d, f in zip(DECL, a_sh))
        a_def = sum((d - f) for d, f in zip(DECL, a_sh) if d > f)
        a_sur = sum((f - d) for d, f in zip(DECL, a_sh) if f > d)
        check("WE 2.1.1 invariant: deficits equal surpluses for any portfolio", a_def, a_sur)
        check("WE 2.1.1 invariant: reallocation distance is (1 - index) x denominator",
              a_def * sum(alt), (1 - a_idx) * sum(alt))
    check("WE 2.1.1 invariant: funded shares match declared weights gives an index of one",
          sum(min(d, d) for d in DECL), 1)

    # Intake cadence — Domain 3's latency formula, read from ctx, priced at Domain 1's cost of delay.
    W_Q, W_M = ew(13, 3), ew(4, 3)
    check("KA 2.1.1 E[wait] for a quarterly board, three-week deadline", W_Q, D("9.5"))
    check("KA 2.1.1 E[wait] for a monthly board, three-week deadline", W_M, D("5.0"))
    check("KA 2.1.1 cadence cost per candidate at Meridian's cost of delay",
          (W_Q - W_M) * COD, 64260)
    check("KA 2.1.1 the weeks saved by the monthly board", W_Q - W_M, D("4.5"))

    # =============================================================================================
    # KA 2.1.3 — Worked example 2.1.3, the alignment half-life and the value of a re-test
    # =============================================================================================
    H = D("0.15")
    SURV = [(1 - H) ** t for t in range(1, 6)]
    for t, printed in zip(range(1, 6), (D("85.00"), D("72.25"), D("61.41"), D("52.20"),
                                       D("44.37"))):
        check(f"WE 2.1.3 survival of alignment S({t}) (step 4)", n(SURV[t - 1] * 100, P2), printed)
    for t, printed in ((1, D("0.85")), (2, D("0.7225")), (3, D("0.614125"))):
        check(f"WE 2.1.3 survival as a decimal, S({t}) (step 3)", SURV[t - 1], printed)
    HALF = D("0.5").ln() / (1 - H).ln()
    check("WE 2.1.3 alignment half-life (step 4)", n(HALF, P4), D("4.2650"))
    check("WE 2.1.3 half-life verified by substitution: S(t) = 0.5 at t = t-half",
          n((1 - H) ** HALF, P6), D("0.500000"))
    check("WE 2.1.3 four-year survival is above a half (interpretation 1)",
          n(SURV[3] * 100, P2), D("52.20"))
    check("WE 2.1.3 five-year survival is below a half (interpretation 1)",
          n(SURV[4] * 100, P2), D("44.37"))

    ANN, LIFE = D(800000), 3
    check("WE 2.1.3 setup: three years at 800,000 is Meridian's approved cost", ANN * LIFE, COST)
    p1, p2 = H, (1 - H) * H
    check("WE 2.1.3 probability the driver is superseded during year 1 (step 2)", p1, D("0.15"))
    check("WE 2.1.3 probability the driver is superseded during year 2 (step 2)", p2, D("0.1275"))
    NO_TEST = p1 * (2 * ANN) + p2 * ANN
    check("WE 2.1.3 year-1 supersession component of the loss (step 3)", p1 * 2 * ANN, 240000)
    check("WE 2.1.3 year-2 supersession component of the loss (step 3)", p2 * ANN, 102000)
    check("WE 2.1.3 expected misaligned spend with no re-test (step 4)", NO_TEST, 342000)
    check("WE 2.1.3 that loss as a share of the programme cost (step 4)",
          n(NO_TEST / COST * 100, P4), D("14.2500"))

    def misaligned(d):
        """Expected misaligned spend with detection probability d at each subsequent annual gate."""
        y1 = (1 - d) * d * ANN + (1 - d) ** 2 * (2 * ANN)
        y2 = (1 - d) * ANN
        return p1 * y1 + p2 * y2, y1, y2

    W80, y1_80, y2_80 = misaligned(D("0.8"))
    check("WE 2.1.3 conditional loss given year-1 supersession at d = 0.8 (step 3)", y1_80, 192000)
    check("WE 2.1.3 conditional loss given year-2 supersession at d = 0.8 (step 3)", y2_80, 160000)
    check("WE 2.1.3 expected misaligned spend with the re-test (step 4)", W80, 49200)
    check("WE 2.1.3 value of the re-test (step 4)", NO_TEST - W80, 292800)
    check("WE 2.1.3 breakeven cost per gate across three gates (step 4)",
          (NO_TEST - W80) / 3, 97600)
    check("WE 2.1.3 year-1 share of the expected loss (interpretation 2)",
          n(p1 * 2 * ANN / NO_TEST * 100, P2), D("70.18"))
    check("WE 2.1.3 asking the question beats asking it well, by this multiple (interpretation 3)",
          n((NO_TEST - W80) / W80, P2), D("5.95"))
    W50, y1_50, y2_50 = misaligned(D("0.5"))
    check("WE 2.1.3 conditional losses at d = 0.5 (interpretation 3)", y1_50, 600000)
    check("WE 2.1.3 year-2 conditional loss at d = 0.5 (interpretation 3)", y2_50, 400000)
    check("WE 2.1.3 residual expected loss at d = 0.5 (interpretation 3)", W50, 141000)
    check("WE 2.1.3 value of a crude re-test at d = 0.5 (interpretation 3)", NO_TEST - W50, 201000)
    check("WE 2.1.3 that value as a share of a perfect re-test (interpretation 3)",
          n((NO_TEST - W50) / NO_TEST * 100, P2), D("58.77"))
    check("WE 2.1.3 a perfect re-test saves the whole 342,000 (interpretation 3)",
          misaligned(D(1))[0], 0)
    check("MCQ 2.1-D distractor A, the reciprocal hazard 1/h", n(1 / H, P4), D("6.6667"))
    check("MCQ 2.1-D distractor C, linear halving 0.5/h", n(D("0.5") / H, P4), D("3.3333"))
    check("MCQ 2.1-D distractor D, the annual hazard read as a continuous rate |ln 0.5|/h",
          n(-D("0.5").ln() / H, P4), D("4.6210"))
    check("MCQ 2.1-D distractor D is systematically too long, not too short",
          1 if -D("0.5").ln() / H > HALF else 0, 1)

    # =============================================================================================
    # KA 2.2.2 — the rewritten Interpretation: two breakeven bases
    # =============================================================================================
    FLAT70 = D("0.70") * FLAT
    check("WE 2.2.2 flat PV at the same 70 % steady state (interpretation 2)", n(FLAT70), 4092967)
    check("WE 2.2.2 the ramp keeps this fraction of the discounted value (interpretation 2)",
          n(RAMPED / FLAT70 * 100, P4), D("91.2027"))
    check("WE 2.2.2 the two ramp years give up this fraction (interpretation 2)",
          n(100 - RAMPED / FLAT70 * 100, P4), D("8.7973"))
    BE_RAMP = COST / PER * 100
    check("WE 2.2.2 ramped-basis breakeven adoption (interpretation 3)", n(BE_RAMP, P4),
          D("45.0053"))
    check("WE 2.2.2 ramped-basis breakeven as printed in the key terms table", n(BE_RAMP, P2),
          D("45.01"))
    check("WE 2.2.2 the two bases differ by (interpretation 3, 2.A.3, exam traps)",
          n(BE_RAMP - BE_FLAT, P4), D("3.9592"))
    check("Self-check 2.2-4 conversion: flat-equivalent = 0.912027 x ramped-basis",
          n(D("0.912027") * BE_RAMP, P4), n(BE_FLAT, P4))
    # Invariant: the 40/60/70 profile is the steady state scaled by 4/7, 6/7, 1, so the ramp fraction
    # is independent of the steady-state level — proved by sweep, not asserted.
    for ss in (D("0.35"), D("0.50"), D("0.70"), D("1.00")):
        prof = [ss * D(4) / D(7), ss * D(6) / D(7)] + [ss] * 6
        pv = sum(POT * a / (1 + R) ** (i + 1) for i, a in enumerate(prof))
        check("WE 2.2.2 invariant: the ramp fraction 0.912027 is independent of the steady state",
              n(pv / (ss * FLAT), P6), D("0.912027"))
        check("WE 2.2.2 invariant: PV per unit of sustained adoption is the same on any level",
              n(pv / ss), n(PER))

    # =============================================================================================
    # KA 2.2.2 — Worked example 2.2.2b, the do-nothing baseline that was not zero
    # =============================================================================================
    DF4 = 1 / (1 + R) ** 4
    check("WE 2.2.2b year-4 discount factor (step 3)", n(DF4, P6), D("0.762895"))
    AV_LEG = D(600000) * DF4
    check("WE 2.2.2b legacy re-platforming avoided, PV (step 4)", n(AV_LEG), 457737)
    check("WE 2.2.2b years 5-8 annuity-factor difference (step 3)", n(AF8 - AF4, P6),
          D("2.584087"))
    AV_REC = D(40000) * (AF8 - AF4)
    check("WE 2.2.2b manual reconciliation avoided, PV (step 4)", n(AV_REC), 103363)
    check("WE 2.2.2b manual reconciliation avoided, undiscounted (step 4)", D(40000) * 4, 160000)
    AV = AV_LEG + AV_REC
    check("WE 2.2.2b do-nothing cost undiscounted (step 4)", D(600000) + D(40000) * 4, 760000)
    check("WE 2.2.2b do-nothing cost, present value (step 4)", n(AV), 561101)
    NPV_B = RAMPED - COST + AV
    check("WE 2.2.2b incremental NPV with the counterfactual priced (step 4)", n(NPV_B), 1893998)
    check("WE 2.2.2b uplift as a share of the stated NPV (step 4)",
          n(AV / (RAMPED - COST) * 100, P2), D("42.10"))
    BE_CF = (COST - AV) / FLAT * 100
    check("WE 2.2.2b flat-equivalent breakeven with the counterfactual (step 4, Fig 2.3.2)",
          n(BE_CF, P4), D("31.4498"))
    check("WE 2.2.2b breakeven improvement in points (step 4)", n(BE_FLAT - BE_CF, P4),
          D("9.5962"))
    NPV40_APPROVED = D("0.40") * FLAT - COST
    check("WE 2.2.2b NPV at the achieved 40 % on the case as written (interpretation 3)",
          n(NPV40_APPROVED), -61162)
    check("WE 2.2.2b NPV at 40 % on the honest baseline (interpretation 3)",
          n(NPV40_APPROVED + AV), 499939)
    check("WE 2.2.2b honest NPV as a share of the advertised NPV (interpretation 2)",
          n(NPV_B / (FLAT - COST) * 100, P2), D("54.94"))
    check("WE 2.2.2b the same obligation dated at year 1 (interpretation 5)",
          n(D(600000) / (1 + R)), 560748)
    check("MCQ 2.2-E distractor C, undiscounted avoided costs added",
          n(RAMPED - COST + D(760000)), 2092898)
    check("MCQ 2.2-E distractor D, the avoided cost subtracted instead of added",
          n(RAMPED - COST - AV), 771797)
    # Invariant: the counterfactual moves the breakeven by exactly PV(avoided) / PV(full potential),
    # which is why it is a breakeven change and not a presentational one.
    check("WE 2.2.2b invariant: breakeven shift equals PV(avoided) over PV(full potential)",
          n(AV / FLAT * 100, P4), n(BE_FLAT - BE_CF, P4))

    # =============================================================================================
    # KA 2.2.3 — the rewritten Interpretation, and Worked examples 2.2.3b and 2.2.3c
    # =============================================================================================
    WTS = [D("0.35"), D("0.30"), D("0.20"), D("0.15")]
    SCORES = {"Meridian": [4, 5, 3, 3], "Beta": [5, 3, 4, 4], "Gamma": [3, 4, 5, 4],
              "Delta": [2, 2, 5, 5]}

    def total(w, s):
        return sum(a * D(b) for a, b in zip(w, s))

    TOTALS = {k: total(WTS, v) for k, v in SCORES.items()}
    check("WE 2.2.3 weighted score, Meridian", TOTALS["Meridian"], D("3.95"))
    check("WE 2.2.3 weighted score, Beta", TOTALS["Beta"], D("4.05"))
    check("WE 2.2.3 weighted score, Gamma", TOTALS["Gamma"], D("3.85"))
    check("WE 2.2.3 weighted score, Delta", TOTALS["Delta"], D("3.05"))

    ITEMS = {"Meridian": (3, D(1693072)), "Beta": (2, D(1200000)), "Gamma": (1, D(900000))}
    check("WE 2.2.3 NPV per unit, Meridian", n(ITEMS["Meridian"][1] / 3), 564357)
    check("WE 2.2.3 NPV per unit, Beta", ITEMS["Beta"][1] / 2, 600000)
    check("WE 2.2.3 NPV per unit, Gamma", ITEMS["Gamma"][1] / 1, 900000)

    def enumerate_v(items, cap):
        """max total NPV over subsets fitting the capacity — the whole point of the example."""
        keys = list(items)
        best, bestset = D(0), ()
        for mask in range(1 << len(keys)):
            pick = [keys[i] for i in range(len(keys)) if mask >> i & 1]
            if sum(items[k][0] for k in pick) <= cap:
                v = sum(items[k][1] for k in pick)
                if v > best:
                    best, bestset = v, tuple(pick)
        return best, bestset

    for cap, printed in ((3, D(2100000)),):
        check("WE 2.2.3 feasible optimum at 3 units, by enumeration (interpretation 1)",
              enumerate_v(ITEMS, cap)[0], printed)
    check("WE 2.2.3 enumerated set {Gamma} (interpretation 2)", ITEMS["Gamma"][1], 900000)
    check("WE 2.2.3 enumerated set {Beta} (interpretation 2)", ITEMS["Beta"][1], 1200000)
    check("WE 2.2.3 enumerated set {Meridian} (interpretation 2)", ITEMS["Meridian"][1], 1693072)
    check("WE 2.2.3 {Meridian, Beta} needs this many units — infeasible (interpretation 2)", 3 + 2,
          5)
    check("WE 2.2.3 {Meridian, Gamma} needs this many units — infeasible (interpretation 2)",
          3 + 1, 4)
    check("WE 2.2.3 Epsilon's NPV per unit (interpretation 2)", n(D(2200000) / 3), 733333)
    check("WE 2.2.3 greedy shortfall against Epsilon (interpretation 2)",
          D(2200000) - D(2100000), 100000)

    UNIT = D(400000)
    V = {c: enumerate_v(ITEMS, c) for c in range(0, 7)}
    for c, printed in ((1, D(900000)), (2, D(1200000)), (3, D(2100000)), (4, D(2593072)),
                       (5, D(2893072)), (6, D(3793072))):
        check(f"WE 2.2.3b V({c}) by enumeration (step 4)", V[c][0], printed)
    for c, printed in ((1, D(900000)), (2, D(300000)), (3, D(900000)), (4, D(493072)),
                       (5, D(300000)), (6, D(900000))):
        check(f"WE 2.2.3b marginal value of unit {c} (step 4)", V[c][0] - V[c - 1][0], printed)
    for k, gain, printed in ((1, D(493072), D(93072)), (2, D(793072), D(-6928)),
                             (3, D(1693072), D(493072))):
        check(f"WE 2.2.3b gain from buying {k} unit(s) from a base of 3 (step 4)",
              V[3 + k][0] - V[3][0], gain)
        check(f"WE 2.2.3b net of buying {k} unit(s) from a base of 3 (step 4)",
              V[3 + k][0] - V[3][0] - k * UNIT, printed)
    check("WE 2.2.3b the second unit judged on its own (interpretation 2)",
          V[5][0] - V[4][0] - UNIT, -100000)
    check("WE 2.2.3b the block of three beats the incremental answer by this multiple "
          "(interpretation 2)", n((V[6][0] - V[3][0] - 3 * UNIT) / (V[4][0] - V[3][0] - UNIT), P2),
          D("5.30"))
    check("WE 2.2.3b the block cost whose best use is three engineers (interpretation 3)",
          3 * UNIT, 1200000)
    check("WE 2.2.3b at c = 2 the marginal unit is below its cost (interpretation 3)",
          V[2][0] - V[1][0], 300000)
    check("WE 2.2.3b so shrinking from 2 units to 1 is worth (interpretation 3)",
          UNIT - (V[2][0] - V[1][0]), 100000)
    check("MCQ 2.2-G marked answer, marginal value of the fourth unit", V[4][0] - V[3][0], 493072)
    check("MCQ 2.2-G distractor A, Gamma's best-in-set per-unit ratio", ITEMS["Gamma"][1], 900000)
    check("MCQ 2.2-G distractor B, the whole NPV of the candidate newly admitted",
          ITEMS["Meridian"][1], 1693072)
    check("MCQ 2.2-G distractor D, Meridian's own average NPV per unit",
          n(ITEMS["Meridian"][1] / 3), 564357)
    # Invariant: the marginal value is non-monotone, which is what makes the block decision necessary.
    check("WE 2.2.3b invariant: marginal value is non-monotone (unit 3 beats unit 2)",
          1 if V[3][0] - V[2][0] > V[2][0] - V[1][0] else 0, 1)
    check("WE 2.2.3b invariant: marginal value is non-monotone again (unit 6 beats unit 5)",
          1 if V[6][0] - V[5][0] > V[5][0] - V[4][0] else 0, 1)

    MARGIN = TOTALS["Beta"] - TOTALS["Meridian"]
    check("WE 2.2.3c the margin to be closed (step 3)", MARGIN, D("0.10"))
    DELTA = MARGIN / 3
    check("WE 2.2.3c flip point as a weight shift (step 4)", n(DELTA, P6), D("0.033333"))
    check("WE 2.2.3c flip point in percentage points (step 4, MCQ 2.2-F, summary)",
          n(DELTA * 100, P4), D("3.3333"))
    WFLIP = [WTS[0] - DELTA, WTS[1] + DELTA, WTS[2], WTS[3]]
    check("WE 2.2.3c strategic-fit weight at the flip point (step 4)", n(WFLIP[0], P6),
          D("0.316667"))
    check("WE 2.2.3c benefit-value weight at the flip point (step 4)", n(WFLIP[1], P6),
          D("0.333333"))
    check("WE 2.2.3c weights still sum to one at the flip point", sum(WFLIP), 1)
    check("WE 2.2.3c Meridian's total at the flip point (step 4)",
          n(total(WFLIP, SCORES["Meridian"]), P6), D("3.983333"))
    check("WE 2.2.3c Beta's total at the flip point (step 4)",
          n(total(WFLIP, SCORES["Beta"]), P6), D("3.983333"))
    check("WE 2.2.3c the two are exactly level at the flip point",
          total(WFLIP, SCORES["Meridian"]), total(WFLIP, SCORES["Beta"]))
    for i, (name, printed) in enumerate((("strategic fit", D("1.40")), ("benefit value",
                                                                       D("1.20")),
                                         ("deliverability", D("0.80")), ("risk", D("0.60")))):
        check(f"WE 2.2.3c criterion influence, {name} (step 4)", (D(5) - D(1)) * WTS[i], printed)
    check("WE 2.2.3c criterion influences sum to the full score range (step 4)",
          sum((D(5) - D(1)) * w for w in WTS), 4)
    SPREAD = max(TOTALS.values()) - min(TOTALS.values())
    check("WE 2.2.3c the four candidates' totals span (step 4)", SPREAD, 1)
    check("WE 2.2.3c that span as a share of the maximum swing (step 4)",
          n(SPREAD / 4 * 100, P4), D("25.0000"))
    check("WE 2.2.3c score move needed on deliverability (interpretation 2)",
          n(MARGIN / WTS[2], P4), D("0.5000"))
    check("WE 2.2.3c score move needed on risk (interpretation 2)", n(MARGIN / WTS[3], P4),
          D("0.6667"))
    check("MCQ 2.2-F distractor C, the margin divided by the 4-point score range",
          n(MARGIN / 4 * 100, P4), D("2.5000"))
    # MCQ 2.2-F distractor A: a one-sided removal of 0.10 from strategic fit also levels the two,
    # but leaves the weights summing to 0.90 — the named error.
    WBAD = [WTS[0] - D("0.10"), WTS[1], WTS[2], WTS[3]]
    check("MCQ 2.2-F distractor A, the one-sided removal in percentage points",
          n(D("0.10") * 100, P4), D("10.0000"))
    check("MCQ 2.2-F distractor A leaves the weights summing to", sum(WBAD), D("0.90"))
    check("MCQ 2.2-F distractor A levels the two candidates, which is why it tempts",
          total(WBAD, SCORES["Meridian"]), total(WBAD, SCORES["Beta"]))
    # Invariant: a fit-to-deliverability shift moves both candidates identically, so the margin is
    # invariant to it for any shift — proved by sweep (MCQ 2.2-F distractor D is true of that pair).
    for dd in (D("0.01"), D("0.10"), D("0.19"), D("0.35")):
        WD = [WTS[0] - dd, WTS[1], WTS[2] + dd, WTS[3]]
        check("WE 2.2.3c invariant: the margin survives any fit-to-deliverability shift",
              total(WD, SCORES["Beta"]) - total(WD, SCORES["Meridian"]), MARGIN)

    # =============================================================================================
    # KA 2.3.2 — Worked example 2.3.2, attribution, and Fig 2.3.2
    # =============================================================================================
    BASE_H, POST_H, COHORT_H = D("22.0"), D("15.6"), D("20.9")
    ADOPTERS, RATE, WKS, CLIN = D(28), D(85), D(48), D(40)
    CASE_H = D("6.0")
    check("WE 2.3.2 setup: adopters plus the comparison cohort are the whole estate",
          ADOPTERS + D(12), CLIN)
    check("WE 2.3.2 setup: the valuation basis rebuilds Domain 1's full potential",
          CLIN * CASE_H * RATE * WKS, POT)
    RAW = BASE_H - POST_H
    CF = BASE_H - COHORT_H
    ATT = RAW - CF
    check("WE 2.3.2 raw improvement (step 3)", RAW, D("6.4"))
    check("WE 2.3.2 counterfactual improvement (step 3)", CF, D("1.1"))
    check("WE 2.3.2 attributable improvement (step 3)", ATT, D("5.3"))
    check("WE 2.3.2 raw claim across the adopters (step 4)", ADOPTERS * RAW * RATE * WKS, 731136)
    check("WE 2.3.2 business-case assumption on the same basis (step 4)",
          ADOPTERS * CASE_H * RATE * WKS, 685440)
    check("WE 2.3.2 attributable claim (step 4, MCQ 2.3-E)", ADOPTERS * ATT * RATE * WKS, 605472)
    check("WE 2.3.2 the over-claim in money (step 4, MCQ 2.3-E distractor D)",
          ADOPTERS * CF * RATE * WKS, 125664)
    check("WE 2.3.2 over-claim share of the raw claim (step 4, MCQ 2.3-F, summary)",
          n(CF / RAW * 100, P4), D("17.1875"))
    check("WE 2.3.2 attributable below the case, per cent (interpretation 1)",
          n((1 - ATT / CASE_H) * 100, P2), D("11.67"))
    check("WE 2.3.2 the case above what can be claimed, per cent (interpretation 1)",
          n((CASE_H / ATT - 1) * 100, P2), D("13.21"))
    POT_ATT = CLIN * ATT * RATE * WKS
    check("WE 2.3.2 corrected full potential (interpretation 3)", POT_ATT, 864960)
    check("WE 2.3.2 corrected potential as a share of the original (interpretation 3)",
          n(POT_ATT / POT * 100, P4), D("88.3333"))
    check("WE 2.3.2 that share is exactly 5.3/6.0 (interpretation 3)", POT_ATT / POT, ATT / CASE_H)
    FLAT_ATT = POT_ATT * AF8
    check("WE 2.3.2 corrected flat PV of full potential (interpretation 3)", n(FLAT_ATT, P2),
          D("5164934.36"))
    BE_ATT = COST / FLAT_ATT * 100
    check("WE 2.3.2 breakeven with attribution corrected only (interpretation 3, Fig 2.3.2)",
          n(BE_ATT, P4), D("46.4672"))
    check("WE 2.3.2 deterioration in points from the attribution correction (interpretation 3)",
          n(BE_ATT - BE_FLAT, P4), D("5.4212"))
    BE_BOTH = (COST - AV) / FLAT_ATT * 100
    check("WE 2.3.2 fully corrected breakeven (interpretation 3, Fig 2.3.2, self-check 2.3-5)",
          n(BE_BOTH, P4), D("35.6035"))
    check("WE 2.3.2 headroom at the achieved 40 % adoption (interpretation 3)",
          n(D(40) - BE_BOTH, P4), D("4.3965"))
    check("WE 2.3.2 NPV at 40 % on the fully corrected case (interpretation 3)",
          n(D("0.40") * FLAT_ATT - COST + AV), 227074)
    check("MCQ 2.3-E distractor A, the raw claim with no counterfactual deducted",
          ADOPTERS * RAW * RATE * WKS, 731136)
    check("MCQ 2.3-E distractor C, the business case's 6.0-hour assumption",
          ADOPTERS * CASE_H * RATE * WKS, B70)
    # Invariant: the over-claim share is counterfactual / raw whatever the rate, the operating year
    # or the number of adopters — the structural twin of Domain 1's adoption identity.
    for cl, rt, wk in ((D(28), D(85), D(48)), (D(9), D(210), D(40)), (D(140), D("62.5"), D(52))):
        raw_c, att_c = cl * RAW * rt * wk, cl * ATT * rt * wk
        check("WE 2.3.2 invariant: over-claim share is counterfactual/raw, independent of "
              "rate, weeks and adopters", n((raw_c - att_c) / raw_c * 100, P4), D("17.1875"))
    # Fig 2.3.2 plots four thresholds against one actual adoption; the ordering is the whole point.
    check("Fig 2.3.2 the four thresholds are in the plotted order (31.45 < 35.60 < 41.05 < 46.47)",
          1 if BE_CF < BE_BOTH < BE_FLAT < BE_ATT else 0, 1)
    check("Fig 2.3.2 the achieved 40 % clears exactly two of the four thresholds",
          sum(1 for b in (BE_CF, BE_BOTH, BE_FLAT, BE_ATT) if D(40) > b), 2)

    # =============================================================================================
    # KA 2.3.4 — Worked example 2.3.4, pricing the assumption register
    # =============================================================================================
    A1_I = (D("0.70") - D("0.45")) * PER
    A2_I = RAMPED * (1 - ATT / CASE_H)
    A3_I = D(600000) * DF4
    A4_I = D(320000)
    A5_I = RAMPED * (1 - D(70) / D(85))
    check("WE 2.3.4 A1 impact, adoption 70 % to 45 % (step 3)", n(A1_I), 1333178)
    check("WE 2.3.4 A2 impact, 6.0 to 5.3 attributable hours (step 3)", n(A2_I), 435505)
    check("WE 2.3.4 A3 impact, legacy remediation not avoided (step 3)", n(A3_I), 457737)
    check("WE 2.3.4 A4 impact, enabling change programme-funded (step 3)", A4_I, 320000)
    check("WE 2.3.4 A5 impact, clinician time valued at 70 not 85 (step 3)", n(A5_I), 658747)
    PROBS = [D("0.35"), D("0.50"), D("0.25"), D("0.45"), D("0.20")]
    IMPACTS = [A1_I, A2_I, A3_I, A4_I, A5_I]
    COSTS = [D(40000), D(15000), D(0), D(0), D(0)]
    EMVS = [i * p for i, p in zip(IMPACTS, PROBS)]
    for tag, e, printed in (("A1", EMVS[0], 466612), ("A2", EMVS[1], 217752),
                            ("A3", EMVS[2], 114434), ("A4", EMVS[3], 144000),
                            ("A5", EMVS[4], 131749)):
        check(f"WE 2.3.4 {tag} EMV (step 4)", n(e), printed)
    check("WE 2.3.4 total impacts (step 4)", n(sum(IMPACTS)), 3205166)
    TOT_EMV = sum(EMVS)
    check("WE 2.3.4 total EMV (step 4, case study A, summary)", n(TOT_EMV), 1074548)
    check("WE 2.3.4 total cost to test (step 4)", sum(COSTS), 55000)
    check("WE 2.3.4 assumption exposure ratio, per cent (step 4)",
          n(TOT_EMV / (RAMPED - COST) * 100, P2), D("80.62"))
    check("WE 2.3.4 exposure ratio as a ratio (interpretation 1, 2.A.3)",
          n(TOT_EMV / (RAMPED - COST), P4), D("0.8062"))
    check("WE 2.3.4 A1 alone as a share of the NPV (step 4)",
          n(EMVS[0] / (RAMPED - COST) * 100, P2), D("35.01"))
    check("WE 2.3.4 A1 EMV per unit of test cost (step 4)", n(EMVS[0] / COSTS[0], P2), D("11.67"))
    check("WE 2.3.4 A2 EMV per unit of test cost (step 4)", n(EMVS[1] / COSTS[1], P2), D("14.52"))
    check("WE 2.3.4 A2 outranks A1 on exposure per unit of test cost (test-order rule)",
          1 if EMVS[1] / COSTS[1] > EMVS[0] / COSTS[0] else 0, 1)
    FREE = sum(e for e, c in zip(EMVS, COSTS) if c == 0)
    check("WE 2.3.4 exposure carried by the free tests (step 4, MCQ 2.3-G, toolkit 2.T.4)",
          n(FREE), 390184)
    check("WE 2.3.4 free tests as a share of the register's exposure (step 4)",
          n(FREE / TOT_EMV * 100, P2), D("36.31"))
    check("WE 2.3.4 residual exposure after the free rows are cleared (interpretation 3)",
          n(TOT_EMV - FREE), 684365)
    check("MCQ 2.3-G distractor C, the double-counted 'risk-adjusted' NPV it implies "
          "(figure not printed, checked for internal consistency)",
          n(RAMPED - COST - TOT_EMV), 258349)

    # =============================================================================================
    # KA 2.4.2 — the rewritten Interpretation on the reset point
    # =============================================================================================
    SPENT, REM_COST, REM_BEN = D(1800000), D(900000), D(780000)
    check("WE 2.4.2 setup: spend plus remaining cost against the approved 2,400,000",
          SPENT + D(600000), COST)
    check("WE 2.4.2 forward NPV (step 4)", REM_BEN - REM_COST, -120000)
    check("WE 2.4.2 the sunk-cost frame multiplies the loss by (interpretation 1)",
          n(REM_COST / (REM_COST - REM_BEN), P4), D("7.5000"))
    check("WE 2.4.2 the 900,000 estimate exceeds the justified ceiling by, per cent "
          "(interpretation 2)", n((REM_COST - REM_BEN) / REM_BEN * 100, P2), D("15.38"))
    check("WE 2.4.2 continuation needs this multiple of the re-based benefit (interpretation 2)",
          n(REM_COST / REM_BEN, P6), D("1.153846"))
    check("WE 2.4.2 exit cost of 150,000 exceeds the 120,000 cost of completing (interpretation 4)",
          1 if D(150000) > REM_COST - REM_BEN else 0, 1)
    check("MCQ 2.4-F: with a 50,000 exit cost the sunk-cost frame reaches the wrong answer",
          1 if D(50000) < REM_COST - REM_BEN else 0, 1)
    check("WE 2.4.2 an inherited approval-time benefit would give a spurious continuation "
          "(interpretation 5)", 1 if RAMPED - REM_COST > 0 else 0, 1)

    # =============================================================================================
    # KA 2.4.3 — Worked example 2.4.3, calibrating the kill criterion, and Fig 2.4.1
    # =============================================================================================
    COMMITTED, REMAIN = D(900000), D(1500000)
    check("WE 2.4.3 setup: committed plus remaining is the approved cost", COMMITTED + REMAIN,
          COST)
    BE_FWD = REMAIN / PER * 100
    check("WE 2.4.3 forward breakeven adoption at the gate (step 4, MCQ 2.4-D, summary)",
          n(BE_FWD, P4), D("28.1283"))
    check("WE 2.4.3 band condemned by a criterion at the investment breakeven (interpretation 1)",
          n(BE_RAMP - BE_FWD, P4), D("16.8770"))
    check("WE 2.4.3 that band is exactly the sunk 900,000 expressed in adoption points",
          n(COMMITTED / PER * 100, P4), n(BE_RAMP - BE_FWD, P4))
    check("MCQ 2.4-D distractor D, remaining cost over total cost", n(REMAIN / COST * 100, P4),
          D("62.5000"))

    STATES = ((D("0.25"), D("0.25"), D("0.80")), (D("0.45"), D("0.45"), D("0.35")),
              (D("0.70"), D("0.30"), D("0.10")))
    check("WE 2.4.3 setup: the three priors sum to one", sum(p for _, p, _ in STATES), 1)
    NPVS = [PER * s - REMAIN for s, _, _ in STATES]
    check("WE 2.4.3 forward NPV in the Low state (step 3)", n(NPVS[0]), -166822)
    check("WE 2.4.3 forward NPV in the Mid state (step 3)", n(NPVS[1]), 899720)
    check("WE 2.4.3 forward NPV in the High state (step 3)", n(NPVS[2]), 2232898)
    check("WE 2.4.3 remaining benefit PV in the Mid state (step 3)", n(PER * D("0.45")), 2399720)
    for tag, (_, _, w), printed in (("Low", STATES[0], D("0.20")), ("Mid", STATES[1], D("0.65")),
                                    ("High", STATES[2], D("0.90"))):
        check(f"WE 2.4.3 P(Strong | {tag}) used in the Policy B substitution (step 3)", 1 - w,
              printed)
    POL_A = sum(p * v for (_, p, _), v in zip(STATES, NPVS))
    POL_B = sum(p * (1 - w) * v for (_, p, w), v in zip(STATES, NPVS))
    check("WE 2.4.3 Policy A, continue everything (step 4, Fig 2.4.1)", n(POL_A), 1033038)
    check("WE 2.4.3 Policy B, stop on Weak (step 4, Fig 2.4.1)", n(POL_B), 857709)
    SAVED = sum(p * w * -v for (_, p, w), v in zip(STATES, NPVS) if v < 0)
    D_MID = STATES[1][1] * STATES[1][2] * NPVS[1]
    D_HIGH = STATES[2][1] * STATES[2][2] * NPVS[2]
    check("WE 2.4.3 loss avoided on correctly stopped Low programmes (step 4, Fig 2.4.1)",
          n(SAVED, P2), D("33364.45"))
    check("WE 2.4.3 value destroyed by stopping Mid programmes (step 4, Fig 2.4.1)", n(D_MID, P2),
          D("141705.89"))
    check("WE 2.4.3 value destroyed by stopping High programmes (step 4, Fig 2.4.1)",
          n(D_HIGH, P2), D("66986.93"))
    check("WE 2.4.3 total value destroyed (step 4, Fig 2.4.1)", n(D_MID + D_HIGH, P2),
          D("208692.82"))
    check("WE 2.4.3 result table, loss avoided as printed", n(SAVED), 33364)
    check("WE 2.4.3 result table, Mid value destroyed as printed", n(D_MID), 141706)
    check("WE 2.4.3 result table, High value destroyed as printed", n(D_HIGH), 66987)
    check("WE 2.4.3 result, total destroyed as printed", n(D_MID + D_HIGH), 208693)
    check("WE 2.4.3 net effect of the criterion (step 4, Fig 2.4.1, MCQ 2.4-E, summary)",
          n(SAVED - D_MID - D_HIGH), -175328)
    check("WE 2.4.3 identity: Policy A less Policy B equals destroyed less saved",
          POL_A - POL_B, D_MID + D_HIGH - SAVED)
    FF = STATES[0][1] * -NPVS[0] / (STATES[1][1] * NPVS[1]) * 100
    check("WE 2.4.3 breakeven false-flag rate on Mid, perfect Low detection (interpretation 2)",
          n(FF, P4), D("10.3009"))
    check("WE 2.4.3 numerator of that rate, the whole Low loss (interpretation 2)",
          n(STATES[0][1] * -NPVS[0]), 41706)
    check("WE 2.4.3 denominator of that rate, the Mid upside at risk (interpretation 2)",
          n(STATES[1][1] * NPVS[1]), 404874)
    MIX = [p * w for _, p, w in STATES]
    check("WE 2.4.3 flagged population, Low share (interpretation 2)", n(MIX[0], P6),
          D("0.200000"))
    check("WE 2.4.3 flagged population, Mid share (interpretation 2)", n(MIX[1], P6),
          D("0.157500"))
    check("WE 2.4.3 flagged population, High share (interpretation 2)", n(MIX[2], P6),
          D("0.030000"))
    check("WE 2.4.3 share of everything stopped that would have paid (interpretation 2)",
          n((MIX[1] + MIX[2]) / sum(MIX) * 100, P2), D("48.39"))
    # Invariant: at the breakeven false-flag rate the criterion is exactly value-neutral.
    POL_B_BE = (STATES[0][1] * (1 - 1) * NPVS[0] + STATES[1][1] * (1 - FF / 100) * NPVS[1]
                + STATES[2][1] * NPVS[2])
    check("WE 2.4.3 invariant: at 10.3009 % false flags on Mid the criterion is value-neutral",
          n(POL_B_BE, P2), n(POL_A, P2))

    # =============================================================================================
    # 2.A.1 — Worked example 2.A.1, staging Meridian
    # =============================================================================================
    P_HIGH, P_LOW = D("0.60"), D("0.40")
    B_HIGH, B_LOW = PER * D("0.70"), PER * D("0.35")
    check("WE 2.A.1 High-state benefit PV (step 1)", n(B_HIGH), 3732898)
    check("WE 2.A.1 Low-state benefit PV (step 1)", n(B_LOW), 1866449)
    check("WE 2.A.1 setup: full commitment at 60,000 a clinic", COST / 40, 60000)
    PH1, PH2 = D(840000), D(1680000)
    check("WE 2.A.1 setup: phase 1 at 70,000 a clinic for 12 clinics", PH1 / 12, 70000)
    check("WE 2.A.1 setup: phase 2 at the original unit rate for 28 clinics", PH2 / 28, 60000)
    check("WE 2.A.1 setup: the 12 installed clinics are this share of the estate",
          n(D(12) / D(40) * 100, P2), D("30.00"))
    F_H, F_L = B_HIGH - COST, B_LOW - COST
    E_F = P_HIGH * F_H + P_LOW * F_L
    check("WE 2.A.1 F, High branch (step 4)", n(F_H), 1332898)
    check("WE 2.A.1 F, Low branch (step 4)", n(F_L), -533551)
    check("WE 2.A.1 F, expected NPV (step 4)", n(E_F), 586318)
    check("WE 2.A.1 phase-2 commitment discounted one year (step 3)", n(PH2 / (1 + R), P2),
          D("1570093.46"))
    check("WE 2.A.1 High benefit stream shifted one year (step 3)", n(B_HIGH / (1 + R), P2),
          D("3488689.41"))
    S_H = -PH1 - PH2 / (1 + R) + B_HIGH / (1 + R)
    check("WE 2.A.1 S, High branch (step 4)", n(S_H), 1078596)
    check("WE 2.A.1 residual 12-clinic value on abandonment (step 3)",
          n(D("0.30") * B_LOW / (1 + R), P2), D("523303.41"))
    S_LA = -PH1 + D("0.30") * B_LOW / (1 + R)
    check("WE 2.A.1 S, Low branch with abandonment (step 4)", n(S_LA), -316697)
    check("WE 2.A.1 Low benefit stream shifted one year (step 3)", n(B_LOW / (1 + R), P2),
          D("1744344.71"))
    S_LC = -PH1 - PH2 / (1 + R) + B_LOW / (1 + R)
    check("WE 2.A.1 S, Low branch if committed anyway (step 4)", n(S_LC), -665749)
    E_S = P_HIGH * S_H + P_LOW * S_LA
    E_C = P_HIGH * S_H + P_LOW * S_LC
    check("WE 2.A.1 S, expected NPV with the abandonment option (step 4)", n(E_S), 520479)
    check("WE 2.A.1 S, expected NPV committed to complete (step 4)", n(E_C), 380858)
    check("WE 2.A.1 price of staging (step 4)", n(E_F - E_C), 205460)
    check("WE 2.A.1 value of the option to abandon (step 4)", n(E_S - E_C), 139621)
    check("WE 2.A.1 net verdict (step 4)", n(E_S - E_F), -65839)
    check("WE 2.A.1 identity: option value less price of staging equals the net verdict",
          (E_S - E_C) - (E_F - E_C), E_S - E_F)
    check("WE 2.A.1 avoided loss in the Low state (interpretation 1)", n(S_LA - S_LC), 349052)
    check("WE 2.A.1 the 120,000 scale premium on phase 1 (interpretation 3)", PH1 - D(720000),
          120000)
    check("WE 2.A.1 cost of shifting the High benefit stream a year (interpretation 4)",
          n(B_HIGH * (1 - 1 / (1 + R))), 244208)
    # Breakeven P(Low), solved from the two expectation lines rather than restated.
    NUM = F_H - S_H
    DEN = (F_H - F_L) - (S_H - S_LA)
    P_BE = NUM / DEN
    check("WE 2.A.1 breakeven P(Low), numerator (step 4)", n(NUM, P2), D("254301.72"))
    check("WE 2.A.1 breakeven P(Low), denominator (step 4)", n(DEN, P2), D("471156.29"))
    check("WE 2.A.1 breakeven P(Low) (step 4, interpretation 5, summary)", n(P_BE * 100, P4),
          D("53.9740"))
    check("WE 2.A.1 breakeven P(Low) verified: the two expectations are equal there",
          n((1 - P_BE) * F_H + P_BE * F_L, P2),
          n((1 - P_BE) * S_H + P_BE * S_LA, P2))
    # No-premium staging, and the premium ceiling: E[S] falls one-for-one with the phase-1 cost.
    NOPREM = D(720000)
    E_S_NP = E_S + (PH1 - NOPREM)
    check("WE 2.A.1 staged expected NPV at rollout unit rates (step 4)", n(E_S_NP), 640479)
    check("WE 2.A.1 that beats full commitment by (step 4)", n(E_S_NP - E_F), 54161)
    C_MAX = PH1 - (E_F - E_S)
    check("WE 2.A.1 maximum justifiable first-phase cost (step 4)", n(C_MAX), 774161)
    check("WE 2.A.1 at that cost the staged option exactly ties with full commitment",
          n(E_S + (PH1 - C_MAX), P2), n(E_F, P2))
    check("WE 2.A.1 premium ceiling over rollout unit rates (step 4, interpretation 3)",
          n((C_MAX - NOPREM) / NOPREM * 100, P4), D("7.5223"))
    check("WE 2.A.1 premium actually assumed (step 4)", n((PH1 - NOPREM) / NOPREM * 100, P4),
          D("16.6667"))
    check("WE 2.A.1 the assumed premium is above the ceiling, hence the negative verdict",
          1 if PH1 > C_MAX else 0, 1)

    # =============================================================================================
    # Industry variations
    # =============================================================================================
    UPLIFT = COST * D("1.20")
    check("Industry variations, public sector: cost after a 20 % uplift", UPLIFT, 2880000)
    check("Industry variations, public sector: breakeven after the uplift",
          n(UPLIFT / FLAT * 100, P4), D("49.2552"))
    check("Industry variations, public sector: rise in points",
          n(UPLIFT / FLAT * 100 - BE_FLAT, P4), D("8.2092"))
    check("Industry variations, public sector: the achieved 40 % is now out of reach",
          1 if UPLIFT / FLAT * 100 > 40 else 0, 1)
    AF8_8 = af(D("0.08"), 8)
    check("Industry variations, utilities: AF(0.08,8)", n(AF8_8, P6), D("5.746639"))
    check("Industry variations, utilities: breakeven at 8 %", n(COST / (POT * AF8_8) * 100, P4),
          D("42.6507"))
    check("Industry variations, utilities: rise in points from one point of rate",
          n(COST / (POT * AF8_8) * 100 - BE_FLAT, P4), D("1.6047"))
    check("Industry variations, health: the cohort removed this share of the claim",
          n(CF / RAW * 100, P4), D("17.1875"))
    check("Industry variations, technology: the premium ceiling carried across",
          n((C_MAX - NOPREM) / NOPREM * 100, P4), D("7.5223"))

    # =============================================================================================
    # Case study A — the four-breakeven reconciliation; Case study B — the descope arithmetic
    # =============================================================================================
    check("Case study A row 1, the case as approved: breakeven", n(BE_FLAT, P4), D("41.0460"))
    check("Case study A row 1, NPV at 40 %", n(NPV40_APPROVED), -61162)
    check("Case study A row 2, attribution corrected only: breakeven", n(BE_ATT, P4),
          D("46.4672"))
    check("Case study A row 2, NPV at 40 %", n(D("0.40") * FLAT_ATT - COST), -334026)
    check("Case study A row 3, counterfactual priced only: breakeven", n(BE_CF, P4), D("31.4498"))
    check("Case study A row 3, NPV at 40 %", n(NPV40_APPROVED + AV), 499939)
    check("Case study A row 4, both corrections: breakeven", n(BE_BOTH, P4), D("35.6035"))
    check("Case study A row 4, NPV at 40 %", n(D("0.40") * FLAT_ATT - COST + AV), 227074)
    check("Case study A points short of the breakeven it was judged against",
          n(BE_FLAT - 40, P2), D("1.05"))
    check("Case study A points clear of the breakeven that was true", n(D(40) - BE_BOTH, P4),
          D("4.3965"))
    check("Case study A exposure resolvable for nothing before approval", n(FREE), 390184)

    B_CONT, B_DESC = D(14) - D(21), D(9) - D(6)
    check("Case study B forward NPV of continuing as planned", B_CONT, -7)
    check("Case study B forward NPV of the descoped variant", B_DESC, 3)
    check("Case study B swing between the two decisions", B_DESC - B_CONT, 10)
    check("Case study B swing as a share of the original 40m approval",
          n((B_DESC - B_CONT) / D(40) * 100, P2), D("25.00"))
    check("Case study B swing as a share of the 62m spent",
          n((B_DESC - B_CONT) / D(62) * 100, P2), D("16.13"))
    check("Case study B value per unit of spend on the discarded scope",
          n((D(14) - D(9)) / (D(21) - D(6)), P4), D("0.3333"))
    check("Case study B value per unit of spend on the retained scope", n(D(9) / D(6), P4),
          D("1.5000"))
    check("Case study B the retained ratio is 4.5 times the discarded one",
          n((D(9) / D(6)) / ((D(14) - D(9)) / (D(21) - D(6))), P2), D("4.50"))

    # =============================================================================================
    # Calculation exercises 2.5–2.9
    # =============================================================================================
    MAP5 = D(12000000)
    F5 = [D(3600000), D(5400000), D(3000000)]
    D5 = [D("0.45"), D("0.30"), D("0.25")]
    check("Exercise 2.5 setup: the three declared weights sum to one", sum(D5), 1)
    check("Exercise 2.5 setup: the three funded lines sum to the mapped spend", sum(F5), MAP5)
    sh5 = [x / MAP5 for x in F5]
    for i, printed in enumerate((D("30.0000"), D("45.0000"), D("25.0000"))):
        check(f"Exercise 2.5 funded share {i + 1}", n(sh5[i] * 100, P4), printed)
    IDX5 = sum(min(a, b) for a, b in zip(D5, sh5))
    check("Exercise 2.5 alignment index", n(IDX5 * 100, P4), D("85.0000"))
    check("Exercise 2.5 reallocation distance", (1 - IDX5) * MAP5, 1800000)
    check("Exercise 2.5 the single deficit on objective one", (D5[0] - sh5[0]) * MAP5, 1800000)
    check("Exercise 2.5 the single surplus on objective two", (sh5[1] - D5[1]) * MAP5, 1800000)
    check("Exercise 2.5 common error, deficit plus surplus", (1 - IDX5) * MAP5 * 2, 3600000)

    R6 = D("0.06")
    AF6_8, AF6_3 = af(R6, 8), af(R6, 3)
    check("Exercise 2.6 AF(0.06,8)", n(AF6_8, P6), D("6.209794"))
    PV6 = D(540000) * AF6_8
    check("Exercise 2.6 full-potential PV", n(PV6), 3353289)
    check("Exercise 2.6 breakeven without the counterfactual", n(D(1500000) / PV6 * 100, P4),
          D("44.7322"))
    A6_1 = D(450000) / (1 + R6) ** 3
    check("Exercise 2.6 mandatory replacement avoided, PV", n(A6_1, P2), D("377828.68"))
    check("Exercise 2.6 years 4-8 discount-factor sum", n(AF6_8 - AF6_3, P6), D("3.536782"))
    A6_2 = D(25000) * (AF6_8 - AF6_3)
    check("Exercise 2.6 workaround cost avoided, PV", n(A6_2, P2), D("88419.55"))
    check("Exercise 2.6 total avoided cost, PV", n(A6_1 + A6_2), 466248)
    check("Exercise 2.6 breakeven with the counterfactual",
          n((D(1500000) - A6_1 - A6_2) / PV6 * 100, P4), D("30.8280"))
    check("Exercise 2.6 improvement in points",
          n(D(1500000) / PV6 * 100 - (D(1500000) - A6_1 - A6_2) / PV6 * 100, P4), D("13.9042"))

    W7 = [D("0.40"), D("0.30"), D("0.20"), D("0.10")]
    S7 = {"P": [4, 5, 2, 2], "Q": [5, 3, 4, 3]}
    check("Exercise 2.7 setup: weights sum to one", sum(W7), 1)
    T7 = {k: total(W7, v) for k, v in S7.items()}
    check("Exercise 2.7 P's weighted total", T7["P"], D("3.70"))
    check("Exercise 2.7 Q's weighted total", T7["Q"], D("4.00"))
    M7 = T7["Q"] - T7["P"]
    check("Exercise 2.7 margin", M7, D("0.30"))
    DELTA7 = M7 / 3
    check("Exercise 2.7 flip point as a weight shift", n(DELTA7, P6), D("0.100000"))
    check("Exercise 2.7 flip point in percentage points", n(DELTA7 * 100, P4), D("10.0000"))
    W7F = [W7[0] - DELTA7, W7[1] + DELTA7, W7[2], W7[3]]
    check("Exercise 2.7 strategic-fit weight at the flip point", n(W7F[0], P4), D("0.3000"))
    check("Exercise 2.7 benefit weight at the flip point", n(W7F[1], P4), D("0.4000"))
    check("Exercise 2.7 P's total at the flip point", n(total(W7F, S7["P"]), P4), D("3.8000"))
    check("Exercise 2.7 Q's total at the flip point", n(total(W7F, S7["Q"]), P4), D("3.8000"))
    for i, printed in enumerate((D("1.60"), D("1.20"), D("0.80"), D("0.40"))):
        check(f"Exercise 2.7 criterion influence {i + 1}", (D(5) - D(1)) * W7[i], printed)
    check("Exercise 2.7 risk-score swing needed to close the margin", n(M7 / W7[3], P4),
          D("3.0000"))
    check("Exercise 2.7 that swing is three-quarters of the whole 4-point scale",
          n(M7 / W7[3] / 4, P4), D("0.7500"))

    IMP8 = [(D(900000), D("0.30")), (D(400000), D("0.45")), (D(250000), D("0.20")),
            (D(150000), D("0.60"))]
    EMV8 = [i * p for i, p in IMP8]
    for i, printed in enumerate((270000, 180000, 50000, 90000)):
        check(f"Exercise 2.8 EMV {i + 1}", EMV8[i], printed)
    check("Exercise 2.8 total EMV", sum(EMV8), 590000)
    check("Exercise 2.8 exposure ratio", n(sum(EMV8) / D(1100000) * 100, P2), D("53.64"))
    check("Exercise 2.8 that ratio against Meridian's 0.8062 — about two-thirds",
          n(sum(EMV8) / D(1100000) / (TOT_EMV / (RAMPED - COST)), P4), D("0.6653"))
    check("Exercise 2.8 common error, the EMVs deducted from the NPV",
          D(1100000) - sum(EMV8), 510000)

    IT9 = {"R": (3, D(2100000)), "S": (2, D(1500000)), "T": (2, D(1350000)), "U": (1, D(800000))}
    for k, printed in (("U", 800000), ("S", 750000), ("R", 700000), ("T", 675000)):
        check(f"Exercise 2.9 NPV per unit, {k}", IT9[k][1] / IT9[k][0], printed)
    check("Exercise 2.9 greedy takes U then S", IT9["U"][1] + IT9["S"][1], 2300000)
    check("Exercise 2.9 greedy leaves this many units idle", 4 - (IT9["U"][0] + IT9["S"][0]), 1)
    V9_4, SET9 = enumerate_v(IT9, 4)
    check("Exercise 2.9 optimum at 4 units, by enumeration", V9_4, 2900000)
    check("Exercise 2.9 the optimum is {R, U}", 1 if set(SET9) == {"R", "U"} else 0, 1)
    check("Exercise 2.9 enumerated set {S,T}", IT9["S"][1] + IT9["T"][1], 2850000)
    check("Exercise 2.9 enumerated set {S,U}", IT9["S"][1] + IT9["U"][1], 2300000)
    check("Exercise 2.9 enumerated set {T,U}", IT9["T"][1] + IT9["U"][1], 2150000)
    check("Exercise 2.9 enumerated set {R}", IT9["R"][1], 2100000)
    check("Exercise 2.9 greedy shortfall", V9_4 - D(2300000), 600000)
    check("Exercise 2.9 greedy shortfall as a share of the available value",
          n((V9_4 - D(2300000)) / V9_4 * 100, P2), D("20.69"))
    V9_5 = enumerate_v(IT9, 5)[0]
    check("Exercise 2.9 optimum at 5 units", V9_5, 3650000)
    check("Exercise 2.9 marginal value of the fifth unit", V9_5 - V9_4, 750000)
