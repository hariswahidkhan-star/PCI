"""PML-AI Domain 1 (extension) — The Project Leadership Profession: golden checks for NEW material.

The domain's ORIGINAL numbers (Meridian's 979,200 / 685,440 / 14,280, the 30 % overstatement, the
4.20-week acceleration breakeven and the 50/70/90 % sensitivity) are verified in `verify_formulas.py`
and are NOT duplicated here. This module covers only what the depth expansion added, and it covers it
by re-deriving each figure from its definition rather than from its printed output:

    two verdicts   WE 1.1.1 — the capital underspend as a stock against the benefit shortfall as a
                   flow, the 4.08 scale ratio, both horizon conversions (undiscounted and at Domain
                   2's 7 % annuity factor, read from ctx via af()), and the reconciliation to Domain
                   16's capital outturn.
    escalation     WE 1.2.2 — the shortfall and recovered run rates, the timing value of 26 weeks,
                   the earliness that funds the remedy, and Fig 1.2.1's four plotted values. The
                   recovered-rate identity (5,712 is exactly 40 % of the cost of delay) is PROVED
                   from 11.2/28 rather than asserted.
    payback        WE 1.3.2b — payback on both benefit bases, the 1/a scaling proved by sweep, the
                   adoption a payback target silently demands, the hyperbolic marginal-slope ratio,
                   the run-cost effect, and every point on Fig 1.3.2's curve.
    trough         WE 1.3.3b — the ramp and post-ramp rates built from the parameters, both
                   durations, the specialist-week ledger, and the crossover horizon SOLVED from the
                   two cumulative-progress functions so that `R × (1 + d/g)` is verified as a
                   theorem rather than restated as a formula. The parameter-free trough condition is
                   proved by sweeping supervision against ramp productivity.
    verification   WE 1.4.3 — the three breakeven error probabilities from `C_v / [(1 − q) L]`, the
                   above-100 % result of a misapplied check, and the cost-versus-exposure asymmetry.
    exercises      1.4–1.7 and Case study B, each re-derived, including every named common error.

Master-thread values come from ctx and are never re-typed: MERIDIAN_BASELINE (USD 2,400,000),
MERIDIAN_BENEFIT_FULL (USD 979,200), MERIDIAN_BENEFIT_70PCT (USD 685,440), MERIDIAN_COST_OF_DELAY
(USD 14,280 a week) and af(r, n) for Domain 2's eight-year 7 % factor. Domain 2's breakeven adoption
(41.05 %), Domain 16's capital outturn (USD 2,514,000), run cost (USD 108,000) and measured hours
(5.4 against 6.0 at 67.50 % adoption) are declared once below as cited cross-domain figures, so a
drift in any of them fails here rather than passing silently.

Published MCQ distractors are checked too (the stock-minus-flow net, the full-gap upper bound, the
three wrong divisors in the breakeven-probability question, the output-based payback read as a
percentage), because the manuscript states each distractor's arithmetic and a wrong distractor is as
much a defect as a wrong marked answer.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    COST = ctx["MERIDIAN_BASELINE"]              # USD 2,400,000 approved
    POT = ctx["MERIDIAN_BENEFIT_FULL"]           # USD 979,200 output-based claim
    B70 = ctx["MERIDIAN_BENEFIT_70PCT"]          # USD 685,440 honest benefit at 70 %
    COD = ctx["MERIDIAN_COST_OF_DELAY"]          # USD 14,280 a week at 70 % adoption

    # ---- primitives the domain builds everything from -------------------------------------------
    CLIN, HRS, RATE, WKS = D(40), D(6), D(85), D(48)
    PER = HRS * RATE                             # value of one adopting clinic-week
    check("D1 primitives rebuild MERIDIAN_BENEFIT_FULL", CLIN * HRS * RATE * WKS, POT)
    check("D1 primitives rebuild MERIDIAN_BENEFIT_70PCT", POT * D("0.70"), B70)
    check("D1 primitives rebuild MERIDIAN_COST_OF_DELAY", CLIN * D("0.70") * HRS * RATE, COD)
    check("WE 1.2.2 value of one adopting clinic-week", PER, 510)

    # cited cross-domain figures — one place, so drift fails loudly
    D2_BREAKEVEN_ADOPTION = D("41.05")           # D2 KA 2.2.2, discounted breakeven adoption %
    D16_OUTTURN = D(2514000)                     # D16 closing capital account
    D16_RUN_COST = D(108000)                     # D16 KA 16.2.1 measured annual run cost
    D16_ADOPTION = D("0.675")                    # D16 registered clinic-count measure
    D16_HOURS, CASE_HOURS = D("5.4"), D("6.0")

    # =============================================================================================
    # KA 1.1.1 — Worked example 1.1.1, the two verdicts priced
    # =============================================================================================
    under = COST * D("0.03")
    check("WE 1.1.1 delivery verdict, 3 % underspend (step 3–4)", under, 72000)
    b40 = POT * D("0.40")
    check("WE 1.1.1 benefit at measured 40 % adoption (step 1)", b40, 391680)
    short = B70 - b40
    check("WE 1.1.1 annual benefit shortfall (step 3)", short, 293760)
    check("WE 1.1.1 shortfall via the adoption term alone (step 3)",
          POT * (D("0.70") - D("0.40")), short)
    check("WE 1.1.1 scale ratio shortfall/underspend (step 4)", short / under, D("4.08"))
    check("WE 1.1.1 ratio is exact, not rounded", (short / under) * 1000 % 1, 0)

    undisc = short * 8
    check("WE 1.1.1 shortfall over D2's eight-year life, undiscounted (step 4)", undisc, 2350080)
    check("WE 1.1.1 that total as % of approved cost (step 4)",
          (undisc / COST * 100).quantize(D("0.01")), D("97.92"))
    AF8 = af(D("0.07"), 8)
    check("WE 1.1.1 D2 annuity factor cited, 7 % over 8 years (step 4)",
          AF8.quantize(D("0.000001")), D("5.971299"))
    disc = short * D("5.971299")
    check("WE 1.1.1 discounted shortfall (step 4)", disc.quantize(D("0.01")), D("1754128.79"))
    check("WE 1.1.1 discounted shortfall as % of approved cost (step 4)",
          (disc / COST * 100).quantize(D("0.01")), D("73.09"))
    check("WE 1.1.1 overstatement of quoting the flow undiscounted (reading 1)",
          (undisc - disc).quantize(D("0.01")), D("595951.21"))
    check("WE 1.1.1 underspend needed to offset one year of shortfall (reading 2)",
          (short / COST * 100).quantize(D("0.01")), D("12.24"))
    weekly = short / WKS
    check("WE 1.1.1 weekly form of the shortfall (reading 3)", weekly, 6120)
    check("WE 1.1.1 weekly form identically 12 clinics x 6 h x 85 (reading 3)", D(12) * PER, weekly)
    implied = COST * D("0.97")
    check("WE 1.1.1 outturn implied by the 3 % underspend (reading 4)", implied, 2328000)
    check("WE 1.1.1 swing to D16's final capital outturn (reading 4)", D16_OUTTURN - implied, 186000)
    check("MCQ 1.1-D distractor C, the illegitimate stock-minus-flow net", short - under, 221760)

    # Invariant: the two verdicts are a stock and a flow, so the ratio is dimensionally a scale
    # factor per year — doubling the horizon doubles the flow side and nothing else.
    check("WE 1.1.1 invariant: flow side scales linearly with horizon", short * 16 / (short * 8), 2)

    # =============================================================================================
    # KA 1.2.2 — Worked example 1.2.2, the timing value of one escalation
    # =============================================================================================
    c40 = CLIN * D("0.40")
    check("WE 1.2.2 clinics in daily use at week 13 (step 1)", c40, 16)
    gap = CLIN * D("0.70") - c40
    check("WE 1.2.2 shortfall in clinics against plan (step 1)", gap, 12)
    c68 = CLIN * D("0.68")
    check("WE 1.2.2 clinics after the remedy, 68 % (step 1)", c68, D("27.2"))
    rec = c68 - c40
    check("WE 1.2.2 clinics the remedy recovers (step 1)", rec, D("11.2"))
    remedy = D(11) * D(12000)
    check("WE 1.2.2 remedy cost, 11 champions at 12,000 (step 1)", remedy, 132000)
    # The setup's caveat: D16's registered clinic-count measure reads 67.50 %, which recovers 11
    # clinics, so the 11.2 used here is the more generous of the two readings and every figure
    # derived from it is an upper estimate.
    check("WE 1.2.2 caveat: D16's registered measure in clinics", CLIN * D16_ADOPTION, 27)
    check("WE 1.2.2 caveat: 11.2 is the more generous reading",
          1 if rec > CLIN * D16_ADOPTION - c40 else 0, 1)

    srr = gap * PER
    check("WE 1.2.2 shortfall run rate (step 4)", srr, 6120)
    rrr = rec * PER
    check("WE 1.2.2 recovered run rate (step 4)", rrr, 5712)
    timing = D(26) * rrr
    check("WE 1.2.2 timing value of 26 weeks of earliness (step 4)", timing, 148512)
    check("WE 1.2.2 timing value as a multiple of the remedy (step 4)",
          (timing / remedy).quantize(D("0.01")), D("1.13"))
    fund = remedy / rrr
    check("WE 1.2.2 earliness that funds the whole remedy (step 4)",
          fund.quantize(D("0.01")), D("23.11"))
    check("WE 1.2.2 margin in the 26 weeks available (step 4)",
          (D(26) - fund).quantize(D("0.01")), D("2.89"))
    check("WE 1.2.2 overstatement from using the gap rate (reading 2)", D(26) * (srr - rrr), 10608)

    # Identity PROVED, not asserted: the recovered rate is that fraction of the cost of delay which
    # the recovered clinics are of the 28 adopting clinics the cost of delay is built on.
    check("WE 1.2.2 identity: recovered clinics as a fraction of the 28 (reading 2)",
          rec / (CLIN * D("0.70")), D("0.40"))
    check("WE 1.2.2 identity: recovered rate as % of the cost of delay (reading 2)",
          (rrr / COD * 100).quantize(D("0.01")), D("40.00"))
    check("MCQ 1.2-D distractor A, the full-gap upper bound", D(26) * srr, 159120)
    check("MCQ 1.2-D distractor C, deducting a remedy incurred either way", timing - remedy, 16512)

    # Fig 1.2.1 — every plotted value
    resid = (gap - rec) * PER
    check("Fig 1.2.1 residual slope after the remedy, 0.8 clinics", resid, 408)
    crim52 = D(26) * srr + D(13) * resid
    check("Fig 1.2.1 crimson line at week 52", crim52, 164424)
    blue52 = D(39) * resid
    check("Fig 1.2.1 blue line at week 52", blue52, 15912)
    check("Fig 1.2.1 gap at week 52 equals the timing value", crim52 - blue52, timing)
    check("Fig 1.2.1 remedy-cost crossing week", (13 + fund).quantize(D("0.01")), D("36.11"))
    # Invariant: whatever the remedy recovers, the week-52 gap collapses to 26 weeks x the RECOVERED
    # run rate — the residual slope cancels because both lines carry it beyond the audit.
    for rec_alt in (D("6"), D("11.2"), D("12")):
        resid_alt = srr - rec_alt * PER
        check("Fig 1.2.1 invariant: week-52 gap is 26 weeks x the recovered run rate",
              (D(26) * srr + D(13) * resid_alt) - D(39) * resid_alt, D(26) * rec_alt * PER)

    check("Case study A recovered run rate annualised", rrr * WKS, 274176)
    check("Case study A timing value against the celebrated underspend",
          (timing / under).quantize(D("0.01")), D("2.06"))

    # Invariant behind reading 1: timing value is linear in earliness while the message cost is
    # fixed, so the benefit-to-cost ratio is bounded only by how early the measure could have shown.
    check("WE 1.2.2 invariant: timing value linear in weeks of earliness",
          (D(52) * rrr) / (D(26) * rrr), 2)

    # =============================================================================================
    # KA 1.3.2 — new readings on the existing worked example
    # =============================================================================================
    check("WE 1.3.2 honest benefit as a ratio of the output-based claim (reading 1)", B70 / POT,
          D("0.70"))
    # Invariant: the overstatement is 1 - a whatever the hours, rate or weeks are.
    for h, r_, w in ((D(3), D(120), D(50)), (D("7.5"), D(60), D(44)), (D(6), D(85), D(48))):
        base = CLIN * h * r_ * w
        check("WE 1.3.2 invariant: overstatement is 1 - a, independent of h, rate and weeks",
              (base - base * D("0.70")) / base, D("0.30"))
    check("WE 1.3.2 full potential per clinic (reading 2)", POT / CLIN, 24480)
    check("WE 1.3.2 realistic benefit per clinic (reading 2)", POT / CLIN * D("0.70"), 17136)
    check("WE 1.3.2 realistic per clinic ties to the honest annual benefit", B70 / CLIN, 17136)
    check("WE 1.3.2 value of a 5-point shift in the adoption definition (reading 3)",
          D("0.05") * POT, 48960)
    U16 = CLIN * RATE * WKS
    check("WE 1.3.2 D16 benefit per unit of adoption-hours (reading 5)", U16, 163200)
    check("WE 1.3.2 hours effect at D16's measured 67.50 % adoption (reading 5)",
          U16 * D16_ADOPTION * (CASE_HOURS - D16_HOURS), 66096)

    # =============================================================================================
    # KA 1.3.2 — Worked example 1.3.2b, payback and the adoption a hurdle demands
    # =============================================================================================
    def payback(a):
        return COST / (POT * a)

    pb_out, pb_hon = COST / POT, COST / B70
    check("WE 1.3.2b payback on the output-based claim (step 4)",
          pb_out.quantize(D("0.0001")), D("2.4510"))
    check("WE 1.3.2b payback on the honest benefit (step 4)",
          pb_hon.quantize(D("0.0001")), D("3.5014"))
    check("WE 1.3.2b ratio of the two paybacks (step 4)",
          (pb_hon / pb_out).quantize(D("0.000001")), D("1.428571"))
    check("WE 1.3.2b that ratio is exactly 1/0.70 (reading 1)", pb_hon / pb_out, D(1) / D("0.70"))
    # Invariant: payback scales as 1/a exactly — proved by sweep, not by restating the formula.
    for a in (D("0.30"), D("0.45"), D("0.70"), D("0.95")):
        check("WE 1.3.2b invariant: payback x adoption is constant (reading 1)",
              payback(a) * a, COST / POT)

    req = COST / 3
    check("WE 1.3.2b annual benefit a three-year rule demands (step 4)", req, 800000)
    adopt3 = req / POT * 100
    check("WE 1.3.2b adoption a three-year payback rule requires (step 4)",
          adopt3.quantize(D("0.01")), D("81.70"))
    check("WE 1.3.2b that requirement in percentage points beyond the case (step 4)",
          (adopt3 - 70).quantize(D("0.01")), D("11.70"))
    check("WE 1.3.2b that requirement in clinics (step 4)",
          ((adopt3 / 100 - D("0.70")) * CLIN).quantize(D("0.01")), D("4.68"))
    check("WE 1.3.2b cross-check: the required adoption returns a three-year payback",
          payback(adopt3 / 100), 3)

    cut_low = payback(D("0.40")) - payback(D("0.50"))
    cut_high = payback(D("0.80")) - payback(D("0.90"))
    check("WE 1.3.2b payback cut from 40 to 50 % adoption (reading 3)",
          cut_low.quantize(D("0.0001")), D("1.2255"))
    check("WE 1.3.2b payback cut from 80 to 90 % adoption (reading 3)",
          cut_high.quantize(D("0.0001")), D("0.3404"))
    check("WE 1.3.2b the first ten points are worth this multiple of the last ten (reading 3)",
          (cut_low / cut_high).quantize(D("0.01")), D("3.60"))
    check("WE 1.3.2b that multiple is exact", cut_low / cut_high, D("3.6"))

    net_ben = B70 - D16_RUN_COST
    check("WE 1.3.2b net benefit after D16's measured run cost (reading 4)", net_ben, 577440)
    pb_net = COST / net_ben
    check("WE 1.3.2b payback including the run cost (reading 4)",
          pb_net.quantize(D("0.0001")), D("4.1563"))
    check("WE 1.3.2b years added by a line that was simply absent (reading 4)",
          (pb_net - pb_hon).quantize(D("0.0001")), D("0.6549"))
    check("WE 1.3.2b gap between the payback hurdle and D2's value breakeven (reading 4)",
          (adopt3 - D2_BREAKEVEN_ADOPTION).quantize(D("0.01")), D("40.65"))

    for a, printed in (("0.30", "8.1699"), ("0.40", "6.1275"), ("0.50", "4.9020"),
                       ("0.70", "3.5014"), ("0.80", "3.0637"), ("0.90", "2.7233"),
                       ("1.00", "2.4510")):
        check(f"Fig 1.3.2 payback at {a} adoption",
              payback(D(a)).quantize(D("0.0001")), D(printed))
    check("Case study A payback at measured adoption vs the output-based claim",
          (payback(D("0.40")) / pb_out).quantize(D("0.01")), D("2.50"))
    check("Case study A that multiple is exactly 1/0.40",
          payback(D("0.40")) / pb_out, D(1) / D("0.40"))
    check("MCQ 1.3-F distractor C, the output-based payback read as a percentage",
          (pb_out * 100).quantize(D("0.01")), D("245.10"))

    # =============================================================================================
    # KA 1.3.3 — new readings on the existing acceleration example
    # =============================================================================================
    SPEND, SAVED_WKS = D(60000), D(8)
    need_wk = SPEND / SAVED_WKS
    check("WE 1.3.3 weekly benefit the spend requires (reading 2)", need_wk, 7500)
    need_clin = need_wk / PER
    check("WE 1.3.3 clinics that weekly benefit requires (reading 2)",
          need_clin.quantize(D("0.0001")), D("14.7059"))
    be_adopt = need_clin / CLIN * 100
    check("WE 1.3.3 breakeven adoption for the acceleration (reading 2)",
          be_adopt.quantize(D("0.01")), D("36.76"))
    cod40 = c40 * PER
    check("WE 1.3.3 cost of delay at the measured 40 % adoption (reading 2)", cod40, 8160)
    check("WE 1.3.3 breakeven weeks at 40 % adoption (reading 2)",
          (SPEND / cod40).quantize(D("0.0001")), D("7.3529"))
    check("WE 1.3.3 margin left in the 8 weeks available (reading 2)",
          (SAVED_WKS - SPEND / cod40).quantize(D("0.0001")), D("0.6471"))
    net40 = SAVED_WKS * cod40 - SPEND
    net70 = SAVED_WKS * COD - SPEND
    check("WE 1.3.3 net at 40 % adoption (reading 2)", net40, 5280)
    check("WE 1.3.3 net at the case's 70 % adoption, for contrast", net70, 54240)
    check("WE 1.3.3 collapse in the net, as a percentage (reading 2)",
          ((1 - net40 / net70) * 100).quantize(D("0.1")), D("90.3"))
    check("WE 1.3.3 maximum justified spend at 40 % adoption (reading 3)",
          SAVED_WKS * cod40, 65280)
    check("WE 1.3.3 maximum justified spend at 70 % adoption (reading 3)",
          SAVED_WKS * COD, 114240)
    # Invariant: breakeven weeks = cost / cost of delay is scale-free in the programme's size.
    for k in (D(1), D(10), D(100)):
        check("WE 1.3.3 invariant: breakeven weeks scale-free (reading 1)",
              (SPEND * k) / (COD * k), SPEND / COD)

    # =============================================================================================
    # KA 1.3.3 — Worked example 1.3.3b, the reinforcement trough
    # =============================================================================================
    CREW, PERHEAD, NEW = D(6), D("0.1"), D(3)
    RAMP_WKS, RAMP_PROD, SUPERV = D(4), D("0.25"), D("0.50")
    REMAIN, SWRATE = D(8), D(4200)

    base = CREW * PERHEAD
    check("WE 1.3.3b base delivery rate, crew of 6 (step 1)", base, D("0.6"))
    sup_loss = NEW * SUPERV * PERHEAD
    check("WE 1.3.3b supervision loss during the ramp (step 3)", sup_loss, D("0.15"))
    new_out = NEW * RAMP_PROD * PERHEAD
    check("WE 1.3.3b newcomer output during the ramp (step 3)", new_out, D("0.075"))
    r_ramp = base - sup_loss + new_out
    check("WE 1.3.3b ramp-period rate (step 3–4)", r_ramp, D("0.525"))
    r_post = base + NEW * PERHEAD
    check("WE 1.3.3b post-ramp rate (step 3–4)", r_post, D("0.9"))

    dur6 = REMAIN / base
    check("WE 1.3.3b duration, crew of 6 (step 4 table)", dur6.quantize(D("0.0001")), D("13.3333"))
    cum4_9 = RAMP_WKS * r_ramp
    check("WE 1.3.3b cumulative at week 4, crew of 9 (step 4 table)", cum4_9, D("2.1"))
    check("WE 1.3.3b cumulative at week 4, crew of 6 (step 4 table)", RAMP_WKS * base, D("2.4"))
    dur9 = RAMP_WKS + (REMAIN - cum4_9) / r_post
    check("WE 1.3.3b duration, crew of 9 (step 4 table)", dur9.quantize(D("0.0001")), D("10.5556"))
    saved = dur6 - dur9
    check("WE 1.3.3b weeks saved (step 4)", saved.quantize(D("0.0001")), D("2.7778"))
    check("WE 1.3.3b weeks saved is exactly 25/9", saved, D(25) / 9)
    val = saved * COD
    check("WE 1.3.3b value of the weeks saved (step 4)", val.quantize(D("0.01")), D("39666.67"))
    sw6, sw9 = CREW * dur6, (CREW + NEW) * dur9
    check("WE 1.3.3b specialist-weeks, crew of 6 (step 4 table)", sw6, D("80.0"))
    check("WE 1.3.3b specialist-weeks, crew of 9 (step 4 table)", sw9, D("95.0"))
    inc = sw9 - sw6
    check("WE 1.3.3b incremental specialist-weeks (step 4)", inc, D("15.0"))
    inc_cost = inc * SWRATE
    check("WE 1.3.3b cost of the incremental effort (step 4)", inc_cost, 63000)
    check("WE 1.3.3b net result of the reinforcement (step 4)",
          (val - inc_cost).quantize(D("0.01")), D("-23333.33"))

    d, g = base - r_ramp, r_post - base
    check("WE 1.3.3b ramp-period deficit rate d (step 4)", d, D("0.075"))
    check("WE 1.3.3b post-ramp gain rate g (step 4)", g, D("0.3"))
    cross = RAMP_WKS * (1 + d / g)
    check("WE 1.3.3b crossover horizon R x (1 + d/g) (step 4)", cross, D("5.00"))

    # The crossover is verified as a THEOREM: solve for the week at which the two cumulative-progress
    # functions are equal, and for the remaining work at which the two durations are equal.
    t_star = (RAMP_WKS * r_post - cum4_9) / (r_post - base)
    check("WE 1.3.3b crossover solved from the cumulative-progress lines", t_star, cross)
    check("WE 1.3.3b cumulative progress is equal at the crossover", base * t_star,
          cum4_9 + r_post * (t_star - RAMP_WKS))
    w_star = base * cross                       # remaining work, in clinics, at the crossover
    check("WE 1.3.3b remaining work at the crossover, in clinics", w_star, 3)
    check("WE 1.3.3b at that work the two durations are equal",
          w_star / base, RAMP_WKS + (w_star - cum4_9) / r_post)

    behind = RAMP_WKS * base - cum4_9
    check("WE 1.3.3b clinics behind at week 4 (step 4)", behind, D("0.3"))
    check("WE 1.3.3b percentage behind at week 4 (step 4)",
          behind / (RAMP_WKS * base) * 100, D("12.5"))
    check("WE 1.3.3b crossover with the ramp halved to 2 weeks (reading 2)",
          D(2) * (1 + d / g), D("2.50"))
    d_low = base - (base - NEW * D("0.25") * PERHEAD + new_out)
    check("WE 1.3.3b deficit vanishes when supervision falls to 25 % (reading 2)", d_low, 0)
    check("WE 1.3.3b crossover with supervision halved (reading 2)",
          RAMP_WKS * (1 + d_low / g), D("4.00"))
    check("WE 1.3.3b shortening the ramp is worth this multiple of easing supervision (reading 2)",
          (cross - D("2.50")) / (cross - D("4.00")), D("2.5"))

    be_cod = inc_cost / saved
    check("WE 1.3.3b breakeven cost of delay (reading 3)", be_cod, 22680)
    max_cod = CLIN * HRS * RATE
    check("WE 1.3.3b maximum conceivable cost of delay, at 100 % adoption (reading 3)",
          max_cod, 20400)
    check("WE 1.3.3b breakeven exceeds that ceiling by (reading 3)",
          (be_cod / max_cod * 100 - 100).quantize(D("0.01")), D("11.18"))
    check("WE 1.3.3b breakeven specialist-week rate (reading 3)",
          (val / inc).quantize(D("0.01")), D("2644.44"))
    check("WE 1.3.3b no-trough reinforced rate, 25 % supervision / 50 % productivity (reading 4)",
          base - NEW * D("0.25") * PERHEAD + NEW * D("0.50") * PERHEAD, D("0.675"))

    # Invariant, parameter-free: a trough exists precisely when supervision load per newcomer
    # exceeds that newcomer's ramp productivity. Proved by sweep over both fractions.
    for s in (D("0.10"), D("0.25"), D("0.50"), D("0.75")):
        for p in (D("0.10"), D("0.25"), D("0.50"), D("0.75")):
            rate_r = base - NEW * s * PERHEAD + NEW * p * PERHEAD
            check("WE 1.3.3b invariant: trough exists iff supervision > ramp productivity",
                  1 if rate_r < base else 0, 1 if s > p else 0)

    check("MCQ 1.3-E stem and answer, breakeven cost of delay", inc_cost / saved, 22680)

    # =============================================================================================
    # KA 1.4.3 — Worked example 1.4.3, proportionate verification
    # =============================================================================================
    cv_sum, cv_board, cv_safe = D("0.5") * RATE, D(6) * RATE, D(12000)
    check("WE 1.4.3 cost of a glance, 0.5 h at 85 (step 1)", cv_sum, D("42.50"))
    check("WE 1.4.3 cost of a recompute and source trace, 6 h at 85 (step 1)", cv_board, 510)
    L_sum, L_board, L_safe = D(5000), short, D(4000000)
    check("WE 1.4.3 the board figure's loss is 1.3.2's overstatement (step 1)", L_board, 293760)
    e_sum = (1 - D("0.90")) * L_sum
    e_board = (1 - D("0.25")) * L_board
    e_safe = (1 - D("0.05")) * L_safe
    check("WE 1.4.3 exposed loss, weekly summary (step 4 table)", e_sum, D("500.00"))
    check("WE 1.4.3 exposed loss, board benefits figure (step 4 table)", e_board, 220320)
    check("WE 1.4.3 exposed loss, clinical-safety assessment (step 4 table)", e_safe, 3800000)
    p_sum, p_board, p_safe = cv_sum / e_sum * 100, cv_board / e_board * 100, cv_safe / e_safe * 100
    check("WE 1.4.3 breakeven P*, weekly summary (step 4 table)",
          p_sum.quantize(D("0.0001")), D("8.5000"))
    check("WE 1.4.3 breakeven P*, board benefits figure (step 4 table)",
          p_board.quantize(D("0.0001")), D("0.2315"))
    check("WE 1.4.3 breakeven P*, clinical-safety assessment (step 4 table)",
          p_safe.quantize(D("0.0001")), D("0.3158"))
    check("WE 1.4.3 spread between summary and board figure (reading 1)",
          (p_sum / p_board).quantize(D("0.01")), D("36.72"))
    check("WE 1.4.3 that spread is exact", p_sum / p_board, D("36.72"))
    check("WE 1.4.3 six-hour recompute misapplied to the summary (reading 2)",
          (cv_board / e_sum).quantize(D("0.01")), D("1.02"))
    check("WE 1.4.3 that misapplication as a percentage (reading 2)",
          (cv_board / e_sum * 100).quantize(D("0.01")), D("102.00"))
    check("WE 1.4.3 a P* above 100 % means no error rate justifies the check (reading 2)",
          1 if cv_board / e_sum > 1 else 0, 1)
    check("WE 1.4.3 the safety review costs this multiple of the recompute (reading 3)",
          (cv_safe / cv_board).quantize(D("0.01")), D("23.53"))
    check("WE 1.4.3 yet its breakeven is only this multiple higher (reading 3)",
          (p_safe / p_board).quantize(D("0.01")), D("1.36"))
    check("WE 1.4.3 breakeven with the downstream control removed (reading 4)",
          (cv_sum / L_sum * 100).quantize(D("0.0001")), D("0.8500"))
    check("WE 1.4.3 removing q = 0.90 is a tenfold change in the case for checking (reading 4)",
          (cv_sum / e_sum) / (cv_sum / L_sum), 10)
    # Invariant: P* is homogeneous of degree 0 in a common scaling of C_v and L, so the exposed
    # loss — not the cost of checking — is what discriminates between artefacts.
    for k in (D(2), D(10)):
        check("WE 1.4.3 invariant: P* unchanged when C_v and L scale together",
              (cv_board * k) / ((1 - D("0.25")) * L_board * k) * 100, p_board)

    check("MCQ 1.4-D distractor A, dividing by the full loss",
          (cv_board / L_board * 100).quantize(D("0.0001")), D("0.1736"))
    check("MCQ 1.4-D distractor C, catch probability used as escape probability",
          (cv_board / (D("0.25") * L_board) * 100).quantize(D("0.0001")), D("0.6944"))
    check("MCQ 1.4-D distractor D, escape fraction applied to the verification cost",
          (cv_board * D("0.75") / L_board * 100).quantize(D("0.0001")), D("0.1302"))

    # =============================================================================================
    # Case study B — the plan nobody could critique
    # =============================================================================================
    codB = D(12000) + D(26000)
    check("Case study B cost of delay a week", codB, 38000)
    lossB = D(7) * codB
    check("Case study B cost of the seven-week slip", lossB, 266000)
    chkB = D(2) * D(4) * D(145)
    check("Case study B cost of the verification not done", chkB, 1160)
    check("Case study B loss as a multiple of the check",
          (lossB / chkB).quantize(D("0.01")), D("229.31"))
    check("Case study B breakeven error probability",
          (chkB / lossB * 100).quantize(D("0.0001")), D("0.4361"))
    check("Case study B that breakeven per thousand, 'about four in a thousand'",
          (chkB / lossB * 1000).quantize(D("0.1")), D("4.4"))
    check("Case study B breakeven sits in the same band as 1.4.3's consequential rows",
          1 if p_board < chkB / lossB * 100 < 10 * p_safe else 0, 1)

    # =============================================================================================
    # Calculation exercises 1.4 – 1.7
    # =============================================================================================
    E4_COST, E4_POT = D(1800000), D(540000)
    check("Exercise 1.4 delivery verdict", E4_COST * D("0.02"), 36000)
    check("Exercise 1.4 benefit at planned 75 % adoption", E4_POT * D("0.75"), 405000)
    check("Exercise 1.4 benefit at measured 45 % adoption", E4_POT * D("0.45"), 243000)
    e4_short = E4_POT * (D("0.75") - D("0.45"))
    check("Exercise 1.4 programme verdict, annual shortfall", e4_short, 162000)
    check("Exercise 1.4 scale of the two verdicts",
          (e4_short / (E4_COST * D("0.02"))).quantize(D("0.01")), D("4.50"))
    check("Exercise 1.4 common error, the stock-minus-flow net",
          e4_short - E4_COST * D("0.02"), 126000)

    E5_COST, E5_POT = D(3150000), D(1050000)
    e5_ben = E5_POT * D("0.60")
    check("Exercise 1.5 benefit at the assumed 60 % adoption", e5_ben, 630000)
    check("Exercise 1.5 payback as assumed", (E5_COST / e5_ben).quantize(D("0.01")), D("5.00"))
    e5_req = E5_COST / 4
    check("Exercise 1.5 annual benefit a four-year rule demands", e5_req, 787500)
    check("Exercise 1.5 adoption the rule requires",
          (e5_req / E5_POT * 100).quantize(D("0.01")), D("75.00"))
    check("Exercise 1.5 that requirement in points above the assumption",
          e5_req / E5_POT * 100 - 60, 15)
    check("Exercise 1.5 common error, the output-based payback",
          (E5_COST / E5_POT).quantize(D("0.01")), D("3.00"))
    check("Exercise 1.5 common error, understatement factor 1/a",
          (D(1) / D("0.60")).quantize(D("0.0001")), D("1.6667"))

    E6_CREW, E6_PH, E6_NEW = D(5), D("0.08"), D(2)
    e6_sup = E6_NEW * D("0.6") * E6_PH
    e6_out = E6_NEW * D("0.2") * E6_PH
    check("Exercise 1.6 supervision loss", e6_sup, D("0.096"))
    check("Exercise 1.6 newcomer ramp output", e6_out, D("0.032"))
    e6_d, e6_g = e6_sup - e6_out, E6_NEW * E6_PH
    check("Exercise 1.6 deficit rate d", e6_d, D("0.064"))
    check("Exercise 1.6 post-ramp gain rate g", e6_g, D("0.16"))
    check("Exercise 1.6 crossover horizon", D(3) * (1 + e6_d / e6_g), D("4.20"))
    check("Exercise 1.6 a trough does occur, 60 % supervision against 20 % productivity",
          1 if D("0.6") > D("0.2") else 0, 1)
    check("Exercise 1.6 common error, capacity before", E6_CREW * E6_PH, D("0.40"))
    check("Exercise 1.6 common error, capacity after", (E6_CREW + E6_NEW) * E6_PH, D("0.56"))

    E7_CV, E7_L, E7_Q = D(720), D(250000), D("0.40")
    e7_exposed = (1 - E7_Q) * E7_L
    check("Exercise 1.7 exposed loss", e7_exposed, 150000)
    check("Exercise 1.7 breakeven error probability",
          (E7_CV / e7_exposed * 100).quantize(D("0.01")), D("0.48"))
    check("Exercise 1.7 breakeven with the control removed",
          (E7_CV / E7_L * 100).quantize(D("0.001")), D("0.288"))
    check("Exercise 1.7 breakeven with the check cost halved",
          (E7_CV / 2 / e7_exposed * 100).quantize(D("0.01")), D("0.24"))
    # The solution's claim is that evidence for q matters ABOUT AS MUCH as the price of checking:
    # removing the control moves P* by 0.192 points, halving the cost by 0.24 — comparable, and in
    # fact slightly the smaller of the two, which is why the wording is "about as much" and not "more".
    q_effect = E7_CV / e7_exposed * 100 - E7_CV / E7_L * 100
    cost_effect = E7_CV / e7_exposed * 100 - E7_CV / 2 / e7_exposed * 100
    check("Exercise 1.7 effect of removing the control, in points of P*", q_effect, D("0.192"))
    check("Exercise 1.7 effect of halving the check cost, in points of P*", cost_effect, D("0.24"))
    check("Exercise 1.7 the two effects are of the same order ('about as much')",
          1 if D("0.7") <= q_effect / cost_effect <= D("1.3") else 0, 1)

    # =============================================================================================
    # Domain 1 summary and Executive perspective — every figure carried forward
    # =============================================================================================
    check("Summary payback on the honest benefit", pb_hon.quantize(D("0.0001")), D("3.5014"))
    check("Summary adoption a three-year rule commits to", adopt3.quantize(D("0.01")), D("81.70"))
    check("Summary acceleration breakeven adoption", be_adopt.quantize(D("0.01")), D("36.76"))
    check("Summary cost of a measure left silent for 26 weeks", timing, 148512)
    check("Summary reinforcement, weeks earlier", saved.quantize(D("0.0001")), D("2.7778"))
    check("Summary reinforcement, value destroyed",
          (inc_cost - val).quantize(D("0.01")), D("23333.33"))
    check("Summary crossover horizon", cross, D("5.00"))
    check("Summary breakeven P*, weekly summary, at display precision",
          p_sum.quantize(D("0.01")), D("8.50"))
    check("Summary breakeven P*, board figure", p_board.quantize(D("0.0001")), D("0.2315"))
    check("Summary the 36.72 spread", (p_sum / p_board).quantize(D("0.01")), D("36.72"))
    check("Executive perspective timing value as a multiple of the remedy",
          (timing / remedy).quantize(D("0.01")), D("1.13"))
