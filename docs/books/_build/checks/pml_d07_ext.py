"""PML-AI Domain 7 (extension) — Cost, resources and commercial awareness: golden checks.

Every number printed as a *result* by the depth expansion of
`domain-07-cost-resources-commercial.md` is recomputed here from its definition rather than from its
printed output. The domain's ORIGINAL core arithmetic (the week-13 earned-value set, the three
`EAC`s, `VAC`, `TCPI` to `BAC`, the PERT package, the blended rate, the incentive fee and `PTA`,
the 35 % payables figure) lives in `verify_formulas.py` and is not duplicated. What is added here
are the new engines:

    class range     an accuracy class read as a triangular distribution — bounds, mean, median,
                    quantile, sigma and the percentile the approved number occupies, all from the
                    closed forms, so a mis-stated percentile fails here rather than being copied.
    baseline build  control accounts summed to `BAC`, the funding requirement, and the contingency
                    draw index / projected draw / required draw efficiency, plus the reconciliation
                    invariant `|CV| − draws = unattributed variance`.
    accruals        the reported-versus-true report built twice from the same physical facts, and
                    the timing invariant proved by showing the cumulative index is identical on
                    both paths after the backlog clears.
    commitments     coverage, the uncommitted balances of baseline and forecast, and the identity
                    that their difference IS |`VAC`| — asserted against `EAC(b) − BAC`, not typed.
    earning rules   one physical state earned three ways, the bias of each rule against the
                    objective figure, the 50 % unbiasedness breakeven proved by re-earning at 50 %
                    (including the fact that 0/100 does NOT join them there), and the
                    level-of-effort blend `1 − (1 − s)(1 − SPI_d)` with its cap solved rather than
                    asserted.
    forecasts       each `EAC`'s implied `ETC` efficiency derived as `TCPI` to that `EAC` and
                    checked against 1, `CPI` and `CPI × SPI`; the funding run-out found by spending
                    each authorised `ETC` at the demonstrated efficiency; the descope solved from
                    `TCPI = CPI` and its `1/CPI` exchange rate.
    resources       the mix-shift ranking recomputed from the pool, burdened cost per productive
                    hour, the utilisation sensitivity, and the four priced rungs of Domain 6's
                    capacity ladder with both productivity breakevens solved.
    contracting     the fixed-price / T&M crossing point, the premium-equals-tolerance identity,
                    the triangular exceedance probability, and the incentive structure swept across
                    six share ratios with the ceiling-binds-first condition derived.
    cash            the week-13 cash bridge, the working-capital identity checked as an identity,
                    days of spend, and the payment-terms lever recomputed from the week-8 valuation.
    earned schedule `ES` interpolated off the `PV` curve at two data dates, the mid-project
                    coincidence of `SPI` and `SPI(t)` PROVED from the local slope, and the
                    late-project divergence.
    reserve         Meridian's reserve architecture, the payback, and the latency priced with the
                    decision's own benefit rather than the programme's headline rate.

Master-thread values come from ctx and are never re-typed: AURIGA_BAC, AURIGA_WEEKS,
MERIDIAN_BASELINE, MERIDIAN_COST_OF_DELAY, MERIDIAN_BENEFIT_70PCT (from which the per-clinic weekly
benefit and the 48-week operating year are recovered) and ew(M, L) for Domain 3's governance
latency. Auriga's week-13 `PV`/`EV`/`AC`, its cost of delay (USD 45,000/week, Domain 6) and its
contract price are declared once below.

Published MCQ distractors that are the results of named errors are checked too, because the
manuscript states each distractor's arithmetic and a wrong distractor is as much a defect as a
wrong answer.
"""

from decimal import ROUND_HALF_UP


def run(ctx):
    check, D = ctx["check"], ctx["D"]
    ew = ctx["ew"]

    BAC = ctx["AURIGA_BAC"]                 # USD 4,000,000
    PD = D(ctx["AURIGA_WEEKS"])             # 25 weeks
    PV, EV, AC = D(2080000), D(1920000), D(2120000)   # week-13 data date (verified in the core file)
    COD_A = D(45000)                        # Auriga cost of delay, USD/week (Domain 6)
    PRICE = D(4400000)                      # fixed price to the utility (KA 7.4.4)

    def q(x, n):
        """Round as the manuscript prints — half-up, not banker's."""
        return D(x).quantize(D(1).scaleb(-n), rounding=ROUND_HALF_UP)

    def rt(x):
        return D(x).sqrt()

    CPI, SPI = EV / AC, EV / PV

    # =====================================================================================
    # KA 7.1.1 — WE 7.1.1 / Fig 7.1.1 / MCQ 7.1-D / Exercise 7.6: the class range as a
    # triangular distribution. a, m, b come from the declared -15 %/+30 % class, not literals.
    # =====================================================================================
    a, m, b = BAC * D("0.85"), BAC, BAC * D("1.30")
    check("WE 7.1.1 class lower bound", a, 3400000)
    check("WE 7.1.1 class upper bound", b, 5200000)
    band = b - a
    check("WE 7.1.1 band width", band, 1800000)
    check("WE 7.1.1 band as % of the point estimate", q(band / BAC * 100, 2), D("45.00"))

    tri_mean = (a + m + b) / 3
    check("WE 7.1.1 triangular mean (MCQ 7.1-D key)", tri_mean, 4200000)
    check("WE 7.1.1 mean over the point estimate", tri_mean - BAC, 200000)
    check("WE 7.1.1 mean gap as % of the point", q((tri_mean - BAC) / BAC * 100, 2), D("5.00"))

    pert3 = (a + 4 * m + b) / 6
    check("WE 7.1.1 PERT weighting of the same points (MCQ 7.1-D distractor B)", pert3, 4100000)
    check("WE 7.1.1 PERT gap over the point", pert3 - BAC, 100000)
    check("WE 7.1.1 PERT gap as % of the point", q((pert3 - BAC) / BAC * 100, 2), D("2.50"))
    check("MCQ 7.1-D distractor D (endpoint average, mode ignored)", (a + b) / 2, 4300000)

    Fm = (m - a) / (b - a)
    check("WE 7.1.1 approval percentile F(mode)", q(Fm * 100, 2), D("33.33"))
    check("Self-check 7.1 Q4 exceedance probability", q((1 - Fm) * 100, 2), D("66.67"))

    sigma3 = rt((a * a + m * m + b * b - a * b - a * m - b * m) / 18)
    check("Fig 7.1.1 triangular sigma", q(sigma3, 0), 374166)

    median3 = b - rt((b - a) * (b - m) / 2)
    check("WE 7.1.1 triangular median", q(median3, 0), 4160770)
    check("WE 7.1.1 median lies above the mode (asymmetry direction)",
          1 if median3 > m else 0, 1, tol=D(0))

    p80_3 = b - rt(D("0.20") * (b - a) * (b - m))
    check("WE 7.1.1 P80 of the class range", q(p80_3, 0), 4542733)
    check("WE 7.1.1 P80 premium on the point", q(p80_3 - BAC, 0), 542733)
    check("Fig 7.1.1 P80 premium as %", q((p80_3 - BAC) / BAC * 100, 2), D("13.57"))
    check("WE 7.1.1 P80 above the mean", q(p80_3 - tri_mean, 0), 342733)
    # the coincidence the interpretation rests on
    check("WE 7.1.1 range mean == week-13 EAC(a)", tri_mean, AC + (BAC - EV), tol=D(0))

    # =====================================================================================
    # KA 7.1.2 — the deepened interpretation of the control-hardware package
    # =====================================================================================
    o7, m7, p7 = D(680000), D(750000), D(1000000)
    Ce = (o7 + 4 * m7 + p7) / 6
    sd = (p7 - o7) / 6
    check("7.1.2 mode-to-mean gap in sigma", q((Ce - m7) / sd, 4), D("0.5625"))
    check("7.1.2 mode-to-mean gap as % of the mode", q((Ce - m7) / m7 * 100, 2), D("4.00"))
    check("7.1.2 pessimistic value in sigma above the mean", q((p7 - Ce) / sd, 3), D("4.125"))
    check("7.1.2 optimistic value in sigma below the mean", q((Ce - o7) / sd, 3), D("1.875"))
    p80_pkg = Ce + D("0.8416") * sd
    check("7.1.2 package P80 (normal approximation)", q(p80_pkg, 0), 824885)
    check("7.1.2 contingency above the mean", q(p80_pkg - Ce, 0), 44885)
    check("7.1.2 contingency above the mode", q(p80_pkg - m7, 0), 74885)

    # =====================================================================================
    # KA 7.1.3 — WE 7.1.3 / MCQ 7.1-E / MCQ 7.1-F / Exercise 7.7 / Self-check 7.1 Q5-Q6
    # =====================================================================================
    cas = [D(620000), D(780000), D(540000), D(1020000), D(480000), D(200000)]
    ca_total = sum(cas)
    check("WE 7.1.3 control-account total", ca_total, 3640000)
    check("WE 7.1.3 CA-20 equals WE 7.1.2's Ce", cas[1], Ce, tol=D(0))
    cont, mr = D(360000), D(240000)
    check("WE 7.1.3 BAC = control accounts + contingency (MCQ 7.1-E key)", ca_total + cont, BAC)
    check("WE 7.1.3 total funding requirement", BAC + mr, 4240000)
    check("MCQ 7.1-E distractor C (MR inside the baseline)", ca_total + cont + mr, 4240000)
    check("WE 7.1.3 contingency as % of control accounts", q(cont / ca_total * 100, 2), D("9.89"))
    check("WE 7.1.3 contingency as % of BAC", q(cont / BAC * 100, 2), D("9.00"))
    check("WE 7.1.3 management reserve as % of BAC", q(mr / BAC * 100, 2), D("6.00"))

    drawn = D(150000) + D(40000)
    check("WE 7.1.3 contingency drawn", drawn, 190000)
    pct = EV / BAC                                    # 48.00 % complete, derived not typed
    check("WE 7.1.3 percent complete from EV/BAC", q(pct * 100, 2), D("48.00"))
    check("WE 7.1.3 contingency drawn as %", q(drawn / cont * 100, 2), D("52.78"))
    draw_index = (drawn / cont) / pct
    check("WE 7.1.3 contingency draw index (MCQ 7.1-F distractor D)", q(draw_index, 4), D("1.0995"))
    check("WE 7.1.3 draw index above 1.00 (depleting faster than progress)",
          1 if draw_index > 1 else 0, 1, tol=D(0))
    rate = drawn / (pct * 100)
    check("WE 7.1.3 draw rate per point of progress", q(rate, 2), D("3958.33"))
    projected = rate * 100
    check("WE 7.1.3 projected total draw", q(projected, 0), 395833)
    shortfall = projected - cont
    check("WE 7.1.3 projected shortfall", q(shortfall, 0), 35833)
    check("WE 7.1.3 shortfall as % of contingency", q(shortfall / cont * 100, 2), D("9.95"))
    check("WE 7.1.3 shortfall as % of management reserve", q(shortfall / mr * 100, 2), D("14.93"))
    remain = cont - drawn
    check("WE 7.1.3 remaining contingency", remain, 170000)
    pts_left = (1 - pct) * 100
    check("WE 7.1.3 points of progress remaining", pts_left, 52)
    check("WE 7.1.3 remaining allowance per point", q(remain / pts_left, 2), D("3269.23"))
    dre = remain / (rate * pts_left)
    check("WE 7.1.3 required draw efficiency (MCQ 7.1-F key)", q(dre, 4), D("0.8259"))
    check("WE 7.1.3 required slow-down", q((1 - dre) * 100, 2), D("17.41"))
    check("MCQ 7.1-F distractor A (balance read as an index)", q(remain / cont, 4), D("0.4722"))
    check("MCQ 7.1-F distractor C (shares inverted)", q(pct / (drawn / cont), 4), D("0.9095"))
    # the reconciliation invariant
    CV = EV - AC
    check("WE 7.1.3 unattributed variance = |CV| - draws", abs(CV) - drawn, 10000)
    check("7.A.3 invariant: draws + unattributed == |CV|", drawn + D(10000), abs(CV), tol=D(0))

    # =====================================================================================
    # KA 7.2.1 — WE 7.2.1 / MCQ 7.2-C / MCQ 7.2-D / Exercise 7.8: the unrecognised accrual
    # =====================================================================================
    accrual = D(200000)
    rAC = AC - accrual
    check("WE 7.2.1 reported AC", rAC, 1920000)
    check("WE 7.2.1 reported CPI (MCQ 7.2-C first value)", EV / rAC, 1)
    check("WE 7.2.1 reported CV", EV - rAC, 0)
    check("WE 7.2.1 true CPI (MCQ 7.2-C second value)", q(CPI, 4), D("0.9057"))
    rEACb = BAC / (EV / rAC)
    tEACb = BAC / CPI
    check("WE 7.2.1 reported EAC(b)", rEACb, BAC, tol=D(0))
    check("WE 7.2.1 true EAC(b)", q(tEACb, 0), 4416667)
    check("WE 7.2.1 forecast understatement", q(tEACb - rEACb, 0), 416667)
    check("WE 7.2.1 understatement as % of BAC", q((tEACb - rEACb) / BAC * 100, 2), D("10.42"))
    check("WE 7.2.1 reported TCPI to BAC", (BAC - EV) / (BAC - rAC), 1)
    check("WE 7.2.1 true TCPI to BAC", q((BAC - EV) / (BAC - AC), 4), D("1.1064"))
    pEV, pACt = D(480000), D(520000)
    pACr = pACt + accrual
    check("WE 7.2.1 reported period AC", pACr, 720000)
    check("WE 7.2.1 reported period CPI", q(pEV / pACr, 4), D("0.6667"))
    check("WE 7.2.1 true period CPI", q(pEV / pACt, 4), D("0.9231"))
    check("WE 7.2.1 period index error", q(pEV / pACt - pEV / pACr, 4), D("0.2564"))
    check("WE 7.2.1 reported period implies % over budget",
          q((1 / (pEV / pACr) - 1) * 100, 1), D("50.0"))
    check("WE 7.2.1 true period performance IMPROVED on cumulative",
          1 if pEV / pACt > CPI else 0, 1, tol=D(0))
    cEV, cAC = EV + pEV, AC + pACt
    check("WE 7.2.1 cumulative EV after week 17", cEV, 2400000)
    check("WE 7.2.1 cumulative AC after week 17", cAC, 2640000)
    check("WE 7.2.1 cumulative CPI after week 17", q(cEV / cAC, 4), D("0.9091"))
    # the timing invariant: the reported path reaches the same cumulative AC
    check("WE 7.2.1 accrual omission cancels cumulatively", rAC + pACr, cAC, tol=D(0))

    # =====================================================================================
    # KA 7.2.1b — WE 7.2.1b / MCQ 7.2-E: commitments, coverage and the uncommitted exposure
    # =====================================================================================
    oc, stale = D(1180000), D(120000)
    EACa = AC + (BAC - EV)
    EACb = BAC / CPI
    base_unc = BAC - AC - oc
    check("WE 7.2.1b baseline uncommitted balance (MCQ 7.2-E distractor A)", base_unc, 700000)
    fc_unc = EACb - AC - oc
    check("WE 7.2.1b forecast uncommitted balance (MCQ 7.2-E key)", q(fc_unc, 0), 1116667)
    check("WE 7.2.1b identity: forecast less baseline uncommitted == |VAC(b)|",
          fc_unc - base_unc, EACb - BAC, tol=D("0.000001"))
    check("WE 7.2.1b that difference", q(fc_unc - base_unc, 0), 416667)
    cov_b = (AC + oc) / EACb * 100
    check("WE 7.2.1b commitment coverage of EAC(b)", q(cov_b, 2), D("74.72"))
    check("WE 7.2.1b commitment coverage of EAC(a)", q((AC + oc) / EACa * 100, 2), D("78.57"))
    cov_clean = (AC + oc - stale) / EACb * 100
    check("WE 7.2.1b coverage after cleansing stale lines", q(cov_clean, 2), D("72.00"))
    check("WE 7.2.1b fall in coverage points", q(cov_b - cov_clean, 2), D("2.72"))
    check("WE 7.2.1b baseline uncommitted after cleansing", BAC - AC - (oc - stale), 820000)
    check("WE 7.2.1b weekly rate of new commitment", q(fc_unc / 12, 0), 93056)
    esc = fc_unc * D("0.05")
    check("WE 7.2.1b 5 % movement on the uncommitted remainder", q(esc, 0), 55833)
    check("WE 7.2.1b that exposure as % of the cost variance", q(esc / abs(CV) * 100, 2), D("27.92"))
    check("MCQ 7.2-E distractor C (EV in place of AC)", q(EACb - EV - oc, 0), 1316667)
    check("MCQ 7.2-E distractor D (AC omitted)", q(EACb - oc, 0), 3236667)

    # =====================================================================================
    # KA 7.3.1 — WE 7.3.1 / MCQ 7.3-E / MCQ 7.3-F / Exercise 7.9: earning rules and LOE
    # =====================================================================================
    n_pkg, pkg = 10, D(40000)
    grp = n_pkg * pkg
    check("WE 7.3.1 group budget", grp, 400000)
    PVg = 6 * pkg
    check("WE 7.3.1 group PV (6 packages expected)", PVg, 240000)
    ACg = D(210000)
    done, wip, frac = 4, 3, D("0.30")
    ev_0100 = done * pkg
    ev_5050 = done * pkg + wip * pkg / 2
    ev_pc = done * pkg + wip * frac * pkg
    check("WE 7.3.1 EV under 0/100", ev_0100, 160000)
    check("WE 7.3.1 EV under 50/50 (MCQ 7.3-E first value)", ev_5050, 220000)
    check("WE 7.3.1 EV under objective percent complete (MCQ 7.3-E second)", ev_pc, 196000)
    check("WE 7.3.1 SPI under 0/100", q(ev_0100 / PVg, 4), D("0.6667"))
    check("WE 7.3.1 SPI under 50/50", q(ev_5050 / PVg, 4), D("0.9167"))
    check("WE 7.3.1 SPI under percent complete", q(ev_pc / PVg, 4), D("0.8167"))
    check("WE 7.3.1 CPI under 0/100", q(ev_0100 / ACg, 4), D("0.7619"))
    check("WE 7.3.1 CPI under 50/50", q(ev_5050 / ACg, 4), D("1.0476"))
    check("WE 7.3.1 CPI under percent complete", q(ev_pc / ACg, 4), D("0.9333"))
    check("WE 7.3.1 the earning rule alone flips the account across 1.00",
          1 if (ev_5050 / ACg > 1 > ev_pc / ACg) else 0, 1, tol=D(0))
    check("WE 7.3.1 EV spread", ev_5050 - ev_0100, 60000)
    check("WE 7.3.1 EV spread as % of budget", q((ev_5050 - ev_0100) / grp * 100, 2), D("15.00"))
    check("WE 7.3.1 SPI spread", q(ev_5050 / PVg - ev_0100 / PVg, 4), D("0.2500"))
    check("WE 7.3.1 50/50 overstatement", ev_5050 - ev_pc, 24000)
    check("WE 7.3.1 50/50 overstatement as %", q((ev_5050 - ev_pc) / ev_pc * 100, 2), D("12.24"))
    check("WE 7.3.1 0/100 understatement", ev_pc - ev_0100, 36000)
    check("WE 7.3.1 0/100 understatement as %", q((ev_pc - ev_0100) / ev_pc * 100, 2), D("18.37"))
    # the 50 % unbiasedness breakeven, re-earned rather than asserted
    ev_pc_at50 = done * pkg + wip * D("0.50") * pkg
    check("WE 7.3.1 at genuine 50 %: percent complete meets 50/50", ev_pc_at50, ev_5050, tol=D(0))
    check("WE 7.3.1 at genuine 50 %: 0/100 still reads 160,000", done * pkg, 160000)
    check("WE 7.3.1 0/100 does NOT join them at 50 %",
          1 if done * pkg != ev_5050 else 0, 1, tol=D(0))

    def loe_report(s, spid):
        return 1 - (1 - s) * (1 - spid)

    s70, spid = D("0.70"), D("0.60")
    check("7.3.1 LOE blend at a 70 % share (MCQ 7.3-F key)", loe_report(s70, spid), D("0.88"))
    check("7.3.1 LOE distortion s(1 - SPId) at 70 %", s70 * (1 - spid), D("0.28"))
    check("7.3.1 blend identity == s + (1-s)SPId",
          loe_report(s70, spid), s70 * 1 + (1 - s70) * spid, tol=D(0))
    s20 = D("0.20")
    check("7.3.1 LOE blend under a 20 % cap", loe_report(s20, spid), D("0.68"))
    check("7.3.1 LOE distortion under a 20 % cap", s20 * (1 - spid), D("0.08"))
    # solve 1 - (1 - s)(1 - SPId) >= 0.95  ->  s >= 1 - (1 - 0.95)/(1 - SPId)
    s_needed = 1 - (1 - D("0.95")) / (1 - spid)
    check("7.3.1 LOE share needed to report 0.95", q(s_needed, 3), D("0.875"))
    check("7.3.1 that share does report 0.95", loe_report(s_needed, spid), D("0.95"))
    check("MCQ 7.3-F distractor D (share times index, not blended)", s70 * spid, D("0.42"))

    # =====================================================================================
    # KA 7.3.2 — deepened interpretation
    # =====================================================================================
    check("7.3.2 percent-spent minus percent-complete", q((AC / BAC - EV / BAC) * 100, 2), D("5.00"))
    check("7.3.2 identity: that gap == CV/BAC",
          (AC / BAC - EV / BAC), abs(CV) / BAC, tol=D(0))
    check("7.3.2 overrun rate 1/CPI - 1", q((1 / CPI - 1) * 100, 2), D("10.42"))
    slope = PV - D(1920000)                     # PV(13) - PV(12), from 7.A.1's curve
    check("7.3.2 local PV slope per week", slope, 160000)
    check("7.3.2 SV expressed in weeks", (PV - EV) / slope, 1)

    # =====================================================================================
    # KA 7.3.3 / 7.3.3b — the EAC family, its implied efficiencies and its funding run-out
    # =====================================================================================
    EACc = AC + (BAC - EV) / (CPI * SPI)
    spread = EACc - EACa
    check("7.3.3 EAC spread as % of BAC", q(spread / BAC * 100, 2), D("10.20"))
    check("7.3.3 EAC spread as % of the remaining work", q(spread / (BAC - EV) * 100, 2),
          D("19.62"))
    check("7.3.3 implied ETC efficiency of method (a)", (BAC - EV) / (EACa - AC), 1)
    check("7.3.3 implied ETC efficiency of method (b) == CPI",
          (BAC - EV) / (EACb - AC), CPI, tol=D("0.000001"))
    check("7.3.3 implied ETC efficiency of method (c) == CPI x SPI",
          (BAC - EV) / (EACc - AC), CPI * SPI, tol=D("0.000001"))
    check("7.3.3 method (c) implied efficiency printed", q(CPI * SPI, 4), D("0.8360"))
    check("7.3.3 price of schedule pressure, (c) - (b)", q(EACc - EACb, 0), 191389)
    check("7.3.3 efficiency (c) demands beyond budget", q((1 - CPI * SPI) * 100, 2), D("16.40"))
    check("7.3.3 efficiency already lost", q((1 - CPI) * 100, 2), D("9.43"))
    check("7.3.3b (b) - (a) forecast difference", q(EACb - EACa, 0), 216667)

    for nm, E, etc_p in (("a", EACa, 2080000), ("b", EACb, 2296667), ("c", EACc, 2488056)):
        check(f"7.3.3b funded ETC, method ({nm})", q(E - AC, 0), etc_p)
    buys_a = (EACa - AC) * CPI
    check("7.3.3b budgeted work method (a) buys at CPI", q(buys_a, 0), 1883774)
    check("7.3.3b EV method (a) reaches", q(EV + buys_a, 0), 3803774)
    check("7.3.3b completion method (a) funds (MCQ 7.3-H key)",
          q((EV + buys_a) / BAC * 100, 2), D("95.09"))
    unf = BAC - (EV + buys_a)
    check("7.3.3b unfunded budgeted work", q(unf, 0), 196226)
    check("7.3.3b unfunded as % of BAC", q(unf / BAC * 100, 2), D("4.91"))
    rate_wk = (BAC - EV) / 12
    check("7.3.3b baseline remaining rate per week", q(rate_wk, 0), 173333)
    check("7.3.3b weeks short", q(unf / rate_wk, 2), D("1.13"))
    check("7.3.3b method (b) funds the scope exactly (the BAC/CPI identity)",
          EV + (EACb - AC) * CPI, BAC, tol=D("0.000001"))
    buys_c = (EACc - AC) * CPI
    check("7.3.3b budgeted work method (c) buys at CPI", q(buys_c, 0), 2253333)
    check("7.3.3b EV method (c) would reach at CPI", q(EV + buys_c, 0), 4173333)
    check("7.3.3b method (c) apparent surplus", q(EV + buys_c - BAC, 0), 173333)
    check("7.3.3b method (c) completes exactly at CPI x SPI",
          EV + (EACc - AC) * CPI * SPI, BAC, tol=D("0.000001"))
    check("MCQ 7.3-H distractor C (CPI applied to all of BAC)", q(CPI * 100, 2), D("90.57"))
    check("MCQ 7.3-H distractor D (share of work remaining)",
          q((BAC - EV) / BAC * 100, 2), D("52.00"))

    # =====================================================================================
    # KA 7.3.4 / 7.3.4b — the TCPI identity family, the required improvement, the descope
    # =====================================================================================
    def tcpi(target):
        return (BAC - EV) / (target - AC)

    check("7.3.4 TCPI to BAC", q(tcpi(BAC), 4), D("1.1064"))
    check("7.3.4 TCPI to EAC(a) (MCQ 7.3-G key)", tcpi(EACa), 1)
    check("7.3.4 TCPI to EAC(b)", q(tcpi(EACb), 4), D("0.9057"))
    check("7.3.4 TCPI to EAC(c)", q(tcpi(EACc), 4), D("0.8360"))
    req = tcpi(BAC) / CPI
    check("7.3.4 required improvement ratio", q(req, 4), D("1.2216"))
    check("7.3.4 required improvement as %", q((req - 1) * 100, 2), D("22.16"))
    Ddesc = (BAC - EV) - CPI * (BAC - AC)
    check("7.3.4b descope required (Self-check 7.3 Q5)", q(Ddesc, 0), 377358)
    check("7.3.4b descope is exactly 20,000,000/53", Ddesc, D(20000000) / 53, tol=D("0.000001"))
    check("7.3.4b descope as % of BAC", q(Ddesc / BAC * 100, 2), D("9.43"))
    check("7.3.4b descope as % of remaining work", q(Ddesc / (BAC - EV) * 100, 2), D("18.14"))
    check("7.3.4b after the descope TCPI equals CPI",
          (BAC - Ddesc - EV) / (BAC - AC), CPI, tol=D("0.000001"))
    extra = EACb - BAC
    check("7.3.4b identity: descope share of BAC == 1 - CPI", Ddesc / BAC, 1 - CPI,
          tol=D("0.000000001"))
    check("7.3.4b extra funding for the full scope", q(extra, 0), 416667)
    check("7.3.4b descope exchange rate", q(extra / Ddesc, 6), D("1.104167"))
    check("7.3.4b exchange rate identity == 1/CPI", extra / Ddesc, 1 / CPI, tol=D("0.000001"))

    # =====================================================================================
    # KA 7.4.1 — the mix-shift ranking and the engineer-week
    # =====================================================================================
    def blend(counts_rates):
        return sum(D(c) * D(r) for c, r in counts_rates) / sum(D(c) for c, _ in counts_rates)

    base_pool = [(40, 95), (25, 140), (15, 210)]
    RATE = blend(base_pool)
    check("7.4.1 blended rate", RATE, D("130.625"))
    b_regrade = blend([(35, 95), (25, 140), (20, 210)])
    check("7.4.1 blend after re-grading five technicians", b_regrade, D("137.8125"))
    check("7.4.1 re-grade movement %", q((b_regrade / RATE - 1) * 100, 2), D("5.50"))
    b_addtech = blend([(45, 95), (25, 140), (15, 210)])
    check("7.4.1 blend adding five technicians", q(b_addtech, 4), D("128.5294"))
    check("7.4.1 add-technicians movement %", q((b_addtech / RATE - 1) * 100, 2), D("-1.60"))
    b_addspec = blend([(40, 95), (25, 140), (20, 210)])
    check("7.4.1 blend adding five specialists", q(b_addspec, 4), D("135.2941"))
    check("7.4.1 add-specialists movement %", q((b_addspec / RATE - 1) * 100, 2), D("3.57"))
    check("7.4.1 mix ranking: re-grade > add top > add bottom",
          1 if b_regrade > b_addspec > RATE > b_addtech else 0, 1, tol=D(0))
    EW = RATE * 40
    check("7.4.1 engineer-week", EW, 5225)
    check("7.4.1 engineer-week in SAR at 3.75", q(EW * D("3.75"), 0), 19594)
    check("7.4.1 specialist premium over the blend", D(210) - RATE, D("79.375"))
    check("7.4.1 specialist premium as %", q((D(210) - RATE) / RATE * 100, 2), D("60.77"))
    check("MCQ 7.4-D distractor A (unweighted average)", q(blend([(1, 95), (1, 140), (1, 210)]), 2),
          D("148.33"))
    check("MCQ 7.4-D distractor C (two cheapest grades only)",
          q(blend([(40, 95), (25, 140)]), 2), D("112.31"))

    # =====================================================================================
    # KA 7.4.1b — burdened cost per productive hour
    # =====================================================================================
    basepay, oncost, util, ovh = D("55.00"), D("1.32"), D("0.85"), D("1.18")
    paid = basepay * oncost
    check("7.4.1b cost per paid hour (MCQ 7.4-E distractor D)", paid, D("72.60"))
    prod_pre = paid / util
    check("7.4.1b per productive hour before overhead (MCQ 7.4-E distractor A)",
          q(prod_pre, 4), D("85.4118"))
    full = prod_pre * ovh
    check("7.4.1b full cost per productive hour (MCQ 7.4-E key)", q(full, 4), D("100.7859"))
    check("7.4.1b paid-to-productive multiplier", q(1 / util, 5), D("1.17647"))
    check("7.4.1b that multiplier as an uplift %", q((1 / util - 1) * 100, 2), D("17.65"))
    check("7.4.1b under-recovery against the 95.00 charge-out", q(full - 95, 4), D("5.7859"))
    check("7.4.1b under-recovery as % of cost", q((full - 95) / full * 100, 2), D("5.74"))
    full80 = paid / D("0.80") * ovh
    check("7.4.1b full cost at 80 % utilisation", q(full80, 4), D("107.0850"))
    check("7.4.1b utilisation effect %", q((full80 / full - 1) * 100, 2), D("6.25"))
    check("7.4.1b cost scales as 1/utilisation", full80 / full, util / D("0.80"),
          tol=D("0.000001"))
    labour = BAC * D("0.62")
    check("7.4.1b Auriga labour content", labour, 2480000)
    check("7.4.1b five points of utilisation in money", labour * D("0.0625"), 155000)
    check("7.4.1b that as % of the cost variance",
          q(labour * D("0.0625") / abs(CV) * 100, 2), D("77.50"))
    check("MCQ 7.4-E distractor C (15 % uplift instead of dividing)",
          q(paid * D("1.15") * ovh, 2), D("98.52"))

    # =====================================================================================
    # KA 7.4.1c — the price of a crew-week, four ways
    # =====================================================================================
    crew_h = D(4) * 40
    straight = crew_h * 95
    check("7.4.1c straight-time crew-week", straight, 15200)
    overtime = straight * D("1.5")
    check("7.4.1c overtime crew-week", overtime, 22800)
    check("7.4.1c overtime multiple of straight time", overtime / straight, D("1.5000"))
    agency_nom = crew_h * 128
    check("7.4.1c agency nominal crew-week", agency_nom, 20480)
    agency = agency_nom / D("0.85")
    check("7.4.1c agency cost per productive crew-week", q(agency, 0), 24094)
    check("7.4.1c agency multiple of straight time", q(agency / straight, 4), D("1.5851"))
    check("7.4.1c delay multiple of straight time", q(COD_A / straight, 4), D("2.9605"))
    be_ag = agency_nom / overtime
    check("7.4.1c agency-beats-overtime breakeven (MCQ 7.4-F key)", q(be_ag, 6), D("0.898246"))
    check("7.4.1c that breakeven == rate ratio 128/142.50", be_ag, D(128) / D("142.50"),
          tol=D("0.000001"))
    be_ot = overtime / agency
    check("7.4.1c overtime-beats-agency breakeven", q(be_ot, 6), D("0.946289"))
    check("7.4.1c overtime at 90 % productivity", q(overtime / D("0.90"), 0), 25333)
    check("7.4.1c at 90 % overtime has lost to the agency crew",
          1 if overtime / D("0.90") > agency else 0, 1, tol=D(0))
    check("7.4.1c dearest purchased option as % of the delay", q(agency / COD_A * 100, 2),
          D("53.54"))
    check("7.4.1c cheapest purchased option as % of the delay", q(straight / COD_A * 100, 2),
          D("33.78"))
    check("7.4.1c crew-weeks the delay cost buys, straight", q(COD_A / straight, 2), D("2.96"))
    check("7.4.1c crew-weeks the delay cost buys, overtime", q(COD_A / overtime, 2), D("1.97"))
    check("7.4.1c crew-weeks the delay cost buys, agency", q(COD_A / agency, 2), D("1.87"))
    check("MCQ 7.4-F distractor A (agency against straight time)", q(D(95) / D(128), 4),
          D("0.7422"))
    check("MCQ 7.4-F distractor D (premium inverted)", q(1 / D("1.5"), 4), D("0.6667"))

    # =====================================================================================
    # KA 7.4.2 — fixed price against time and materials
    # =====================================================================================
    FP = D(480000)
    be_h = FP / RATE
    check("7.4.2 breakeven hours (MCQ 7.4-G key)", q(be_h, 2), D("3674.64"))
    h_o, h_m, h_p = D(2900), D(3200), D(4100)
    tm_m = h_m * RATE
    check("7.4.2 T&M at the most likely hours", tm_m, 418000)
    prem = FP - tm_m
    check("7.4.2 fixed-price premium", prem, 62000)
    check("7.4.2 premium as % of the estimate", q(prem / tm_m * 100, 2), D("14.83"))
    check("7.4.2 premium as % of the price (MCQ 7.4-G distractor B)",
          q(prem / FP * 100, 2), D("12.92"))
    check("7.4.2 identity: premium % == hours overrun at breakeven",
          prem / tm_m, be_h / h_m - 1, tol=D("0.000001"))
    check("7.4.2 T&M at the optimistic hours", q(h_o * RATE, 0), 378813)
    check("7.4.2 optimistic advantage over the fixed price", q(FP - h_o * RATE, 0), 101188)
    check("7.4.2 T&M at the pessimistic hours", q(h_p * RATE, 0), 535563)
    check("7.4.2 pessimistic disadvantage", q(h_p * RATE - FP, 0), 55563)
    pert_h = (h_o + 4 * h_m + h_p) / 6
    check("7.4.2 PERT expected hours", pert_h, 3300)
    check("7.4.2 T&M at PERT hours", q(pert_h * RATE, 0), 431063)
    check("7.4.2 premium against PERT expectation", q((FP - pert_h * RATE) / (pert_h * RATE) * 100,
                                                     2), D("11.35"))
    tri_h = (h_o + h_m + h_p) / 3
    check("7.4.2 triangular mean hours", tri_h, 3400)
    check("7.4.2 T&M at the triangular mean", tri_h * RATE, 444125)
    check("7.4.2 premium against the triangular mean",
          q((FP - tri_h * RATE) / (tri_h * RATE) * 100, 2), D("8.08"))
    check("7.4.2 premium range, low", FP - tri_h * RATE, 35875)
    check("7.4.2 premium range, high", q(FP - pert_h * RATE, 0), 48938)
    exceed = (h_p - be_h) ** 2 / ((h_p - h_o) * (h_p - h_m))
    check("7.4.2 triangular probability of exceeding the breakeven",
          q(exceed * 100, 2), D("16.75"))
    check("MCQ 7.4-G distractor C (mid-grade rate instead of the blend)",
          q(FP / 140, 2), D("3428.57"))

    # =====================================================================================
    # KA 7.4.3 — incentive fee, PTA, the zero-fee boundary and the share sweep
    # =====================================================================================
    tc, tf, ceiling = D(2000000), D(150000), D(2450000)
    tp = tc + tf

    def pta_at(s):
        return tc + (ceiling - tp) / s

    def fee_at(cost, s):
        return tf - (cost - tc) * (1 - s)

    def zerofee(s):
        return tc + tf / (1 - s)

    s70 = D("0.70")
    PTA = pta_at(s70)
    check("7.4.3 overrun at the PTA", q(PTA - tc, 2), D("428571.43"))
    check("7.4.3 fee at the PTA", q(fee_at(PTA, s70), 2), D("21428.57"))
    check("7.4.3 buyer outlay at the PTA == the ceiling", PTA + fee_at(PTA, s70), ceiling,
          tol=D("0.000001"))
    check("7.4.3 zero-fee cost", zerofee(s70), 2500000)
    check("7.4.3 ceiling-binds-first threshold", tp + tf * (s70 / (1 - s70)), 2500000)
    check("7.4.3 at that ceiling PTA and zero-fee coincide",
          tc + (D(2500000) - tp) / s70, zerofee(s70), tol=D("0.000001"))
    sweep = [(D("0.50"), 2600000, -150000, 2300000),
             (D("0.60"), 2500000, -50000, 2375000),
             (D(2) / 3, 2450000, 0, 2450000),
             (D("0.70"), 2428571, 21429, 2500000),
             (D("0.80"), 2375000, 75000, 2750000),
             (D("0.90"), 2333333, 116667, 3500000)]
    for s, pp, fp_, zp in sweep:
        check(f"7.4.3 PTA at buyer share {s:.4f}", q(pta_at(s), 0), pp)
        check(f"7.4.3 fee at the PTA, share {s:.4f}", q(fee_at(pta_at(s), s), 0), fp_)
        check(f"7.4.3 zero-fee cost at share {s:.4f}", q(zerofee(s), 0), zp)
    check("7.4.3 PTA arrives earlier as the buyer share rises",
          1 if pta_at(D("0.90")) < pta_at(s70) else 0, 1, tol=D(0))
    check("7.4.3 PTA movement 70/30 to 90/10", q(pta_at(s70) - pta_at(D("0.90")), 2),
          D("95238.10"))
    check("7.4.3 sensitivity of the PTA to the ceiling", q(1 / s70, 4), D("1.4286"))
    check("7.4.3 66.67 % is the share below which the ceiling never binds first",
          1 if pta_at(D("0.60")) > zerofee(D("0.60")) and
          abs(pta_at(D(2) / 3) - zerofee(D(2) / 3)) < D("0.01") else 0, 1, tol=D(0))
    fcast = D(2300000)
    check("7.4.3 PTA headroom on the cost forecast", q(PTA - fcast, 0), 128571)
    check("7.4.3 headroom as % of the forecast", q((PTA - fcast) / fcast * 100, 2), D("5.59"))
    check("7.4.3 a 6 % cost movement passes the inflection",
          1 if fcast * D("1.06") > PTA else 0, 1, tol=D(0))
    check("7.4.3 PTA as % of target cost", q(PTA / tc * 100, 2), D("121.43"))
    check("7.4.3 ceiling as % of target price", q(ceiling / tp * 100, 2), D("113.95"))

    # =====================================================================================
    # KA 7.4.4 / Fig 7.4.1 / MCQ 7.4-H / Exercise 7.14 — cash, profit and the funding gap
    # =====================================================================================
    check("7.4.4 bid margin", PRICE - BAC, 400000)
    check("7.4.4 bid margin as % of price", q((PRICE - BAC) / PRICE * 100, 2), D("9.09"))
    f = PRICE / BAC
    check("7.4.4 price factor", f, D("1.10"))
    ev4, ev8, ev12 = D(320000), D(880000), D(1760000)
    ret_pct = D("0.05")
    cert12 = f * ev12
    check("7.4.4 value certified to week 12", cert12, 1936000)
    ret12 = cert12 * ret_pct
    check("7.4.4 retention withheld to date", ret12, 96800)
    check("7.4.4 net certified to date", cert12 - ret12, 1839200)
    pay4 = f * ev4 * (1 - ret_pct)
    check("7.4.4 week-4 payment received", pay4, 334400)
    payables = AC * D("0.35")
    check("7.4.4 payables as % of AC", q(payables / AC * 100, 2), D("35.00"))
    cashout = AC - payables
    check("7.4.4 cash paid out", cashout, 1378000)
    funding = cashout - pay4
    check("7.4.4 funding absorbed", funding, 1043600)
    check("7.4.4 funding absorbed as % of BAC", q(funding / BAC * 100, 2), D("26.09"))
    revenue = f * EV
    check("7.4.4 revenue recognised", revenue, 2112000)
    check("7.4.4 margin to date (MCQ 7.4-H key)", revenue - AC, -8000)
    recv = (cert12 - ret12) - pay4
    check("7.4.4 receivables outstanding", recv, 1504800)
    unbilled = f * (EV - ev12)
    check("7.4.4 unbilled work in progress", unbilled, 176000)
    exposure = recv + ret12 + unbilled
    check("7.4.4 total client exposure", exposure, 1777600)
    check("7.4.4 working-capital identity: exposure - payables + loss == funding absorbed",
          exposure - payables + (AC - revenue), funding, tol=D(0))
    daily = AC / 91
    check("7.4.4 daily spend", q(daily, 0), 23297)
    check("7.4.4 funding absorbed in days of spend", q(funding / daily, 2), D("44.80"))
    check("7.4.4 cash headline as a multiple of the variance", q(funding / abs(CV), 2), D("5.22"))
    pay8 = f * (ev8 - ev4) * (1 - ret_pct)
    check("7.4.4 week-8 payment brought inside by 30-day terms", pay8, 585200)
    f30 = cashout - pay4 - pay8
    check("7.4.4 funding absorbed under 30-day terms", f30, 458400)
    check("7.4.4 reduction from one payment term", q((funding - f30) / funding * 100, 2),
          D("56.08"))
    ret_end = ret_pct * PRICE
    check("7.4.4 retention at completion", ret_end, 220000)
    check("7.4.4 retention as % of the bid margin", q(ret_end / (PRICE - BAC) * 100, 2), D("55.00"))
    check("7.4.4 retention held beyond completion", ret_end / 2, 110000)
    check("7.4.4 that as % of the bid margin", q(ret_end / 2 / (PRICE - BAC) * 100, 2), D("27.50"))
    for nm, E, mp, pp in (("a", EACa, 200000, D("4.55")),
                          ("b", EACb, -16667, D("-0.38")),
                          ("c", EACc, -208056, D("-4.73"))):
        check(f"7.4.4 margin at completion, method ({nm})", q(PRICE - E, 0), mp)
        check(f"7.4.4 margin at completion as % of price, method ({nm})",
              q((PRICE - E) / PRICE * 100, 2), pp)
    eac_for_margin = PRICE - (PRICE - BAC)      # solve PRICE - EAC = the bid margin
    check("7.4.4 the EAC that preserves the bid margin", eac_for_margin, BAC, tol=D(0))
    check("7.4.4 the TCPI that EAC demands", q(tcpi(eac_for_margin), 4), D("1.1064"))
    check("MCQ 7.4-H distractor A (the bid margin)", PRICE - BAC, 400000)
    check("MCQ 7.4-H distractors C/D (the cost variance read as margin)", abs(CV), 200000)

    # =====================================================================================
    # 7.A.1 / Exercise 7.16 — earned schedule at two data dates
    # =====================================================================================
    pv12 = D(1920000)
    ES13 = D(12) + (EV - pv12) / (PV - pv12)
    check("7.A.1 ES at week 13", ES13, D("12.0000"))
    spit13 = ES13 / 13
    check("7.A.1 SPI(t) at week 13", q(spit13, 4), D("0.9231"))
    check("7.A.1 SPI at week 13", q(SPI, 4), D("0.9231"))
    check("7.A.1 SPI and SPI(t) coincide on a locally linear PV curve", spit13, SPI, tol=D(0))
    ie13 = PD / spit13
    check("7.A.1 time forecast at week 13", q(ie13, 4), D("27.0833"))
    check("7.A.1 weeks late at week 13", q(ie13 - PD, 4), D("2.0833"))
    check("7.A.1 delay cost at Domain 6's rate", q((ie13 - PD) * COD_A, 0), 93750)
    pv24, ev27 = D(3880000), D(3920000)
    ES27 = D(24) + (ev27 - pv24) / (BAC - pv24)
    check("7.A.1 ES at week 27", q(ES27, 4), D("24.3333"))
    spi27 = ev27 / BAC                      # PV is pinned at BAC past the planned finish
    check("7.A.1 SPI at week 27", q(spi27, 4), D("0.9800"))
    spit27 = ES27 / 27
    check("7.A.1 SPI(t) at week 27", q(spit27, 4), D("0.9012"))
    check("7.A.1 divergence at week 27", q(spi27 - spit27, 4), D("0.0788"))
    ie27 = PD / spit27
    check("7.A.1 time forecast at week 27", q(ie27, 4), D("27.7397"))
    check("7.A.1 weeks late at week 27", q(ie27 - PD, 4), D("2.7397"))
    check("7.A.1 lateness as % of planned duration", q((ie27 - PD) / PD * 100, 2), D("10.96"))
    check("7.A.1 remaining baseline weeks at week 27", q(PD - ES27, 4), D("0.6667"))
    check("7.A.1 those weeks at the demonstrated time efficiency",
          q((PD - ES27) / spit27, 4), D("0.7397"))
    check("7.A.1 consistency: 27 + that == IEAC(t)", 27 + (PD - ES27) / spit27, ie27, tol=D(0))

    # =====================================================================================
    # Case study C — Meridian's reserve architecture and the priced latency
    # =====================================================================================
    ca_m, cont_m, mr_m = D(2160000), D(120000), D(120000)
    base_m = ca_m + cont_m
    check("Case C Meridian baseline", base_m, 2280000)
    check("Case C total approved cost == the master-thread figure",
          base_m + mr_m, ctx["MERIDIAN_BASELINE"], tol=D(0))
    check("Case C contingency as % of control accounts", q(cont_m / ca_m * 100, 2), D("5.56"))
    check("Case C management reserve as % of the baseline", q(mr_m / base_m * 100, 2), D("5.26"))
    check("Case C remaining contingency", cont_m - D(74000), 46000)
    pkg_m = D(168000)
    short_m = pkg_m - (cont_m - D(74000))
    check("Case C funding shortfall", short_m, 122000)
    check("Case C shortfall exceeds the whole management reserve", short_m - mr_m, 2000)
    # per-clinic weekly benefit and the operating year recovered from the master-thread constant
    WKS = D(48)
    per_clinic_wk = ctx["MERIDIAN_BENEFIT_70PCT"] / (D(28) * WKS)
    check("Case C per-clinic weekly benefit (from MERIDIAN_BENEFIT_70PCT)", per_clinic_wk, 510)
    clinics = D(28) - D(16)
    check("Case C clinics the decision unlocks", clinics, 12)
    wk_ben = clinics * per_clinic_wk
    check("Case C weekly benefit of the decision", wk_ben, 6120)
    yr_ben = wk_ben * WKS
    check("Case C annual benefit of the decision", yr_ben, 293760)
    check("Case C payback in weeks", q(pkg_m / wk_ben, 4), D("27.4510"))
    check("Case C unreserved gap as a multiple of contingency", q(yr_ben / cont_m, 4), D("2.4480"))
    Ewait = ew(D(4), D(2))
    check("Case C E[wait] = M/2 + L", Ewait, 4)
    check("Case C cost of the wait", Ewait * wk_ben, 24480)
    check("Case C wait as % of the package", q(Ewait * wk_ben / pkg_m * 100, 2), D("14.57"))
    check("Case C headline rate overstates by this factor",
          q(ctx["MERIDIAN_COST_OF_DELAY"] / wk_ben, 4), D("2.3333"))
    check("Case C the wrong figure the headline rate would give",
          Ewait * ctx["MERIDIAN_COST_OF_DELAY"], 57120)
    check("Case C delegated latency cost at E[wait] 0.5", D("0.5") * wk_ben, 3060)
    check("Case C saving from the delegation", Ewait * wk_ben - D("0.5") * wk_ben, 21420)
    check("Case C a 150,000 band clears the 122,000 decision",
          1 if D(150000) > short_m else 0, 1, tol=D(0))

    # =====================================================================================
    # Industry variations — values carried, asserted here against their sources
    # =====================================================================================
    check("Industry (defence) LOE distortion under a 20 % cap", s20 * (1 - spid), D("0.08"))
    check("Industry (technology) utilisation effect in money", labour * D("0.0625"), 155000)
    check("Industry (energy) escalation exposure as % of the variance",
          q(esc / abs(CV) * 100, 2), D("27.92"))
    check("Industry (construction) retention as % of the bid margin",
          q(ret_end / (PRICE - BAC) * 100, 2), D("55.00"))
    check("Industry (public services) unreserved gap over contingency",
          q(yr_ben / cont_m, 4), D("2.4480"))

    # =====================================================================================
    # Exercise 7.2's corrected rounding-error figure (indices are display, not arithmetic)
    # =====================================================================================
    check("EX 7.2 rounded-CPI error, adrift by", q(EACb - BAC / D("0.91"), 0), 21062)
    check("MCQ 7.3-C distractor D (PV in the TCPI denominator)",
          q((BAC - EV) / (BAC - PV), 2), D("1.08"))
