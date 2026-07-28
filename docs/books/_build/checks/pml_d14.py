"""PML-AI Domain 14 — Digital delivery, data and responsible AI: golden checks.

Every number printed as a result in `domain-14-digital-data-responsible-ai.md` is recomputed here
from its definition rather than from its printed output. Master-thread values come from ctx and are
never re-typed: `MERIDIAN_COST_OF_DELAY` (USD 14,280/week, Domain 1), `MERIDIAN_BENEFIT_70PCT`,
`AURIGA_BAC`, and the shared `mesh(n)` helper Domain 4 registered for `n(n - 1)/2`. Auriga's cost of
delay (USD 45,000/week, Domain 6/7) and blended engineering rate (USD 130.625/hour, Domain 7
KA 7.4.1) and Meridian's blended internal rate (USD 110/hour, Domain 11) are declared once and
reused.

Five engines, each checked from its definition:

    exposure      class defects n.d, class exposure n.d.u, total exposure, weighted mean defect
                  rate, exposure per record d.u — and the uniform-target comparison built from
                  Sum(n.u) rather than from the printed total, so the "fewer defects, more cost"
                  result is derived. Remediation net values and their breakeven cost per record are
                  solved, not asserted.
    staleness     the registered E[wait] identity ew(R, G) = R/2 + G applied to information, priced
                  per DAY from the weekly cost of delay, so the day rate is derived not typed. The
                  lever asymmetry is asserted against the general form at both levers.
    automation    naive breakeven F/(m - a) and quality-adjusted F/[(m + em.u) - (a + ea.u)] at
                  three escape-rate settings; the ratio between them is checked to be exactly the
                  ratio of unit savings; the negative-saving case (Case B) is checked to have NO
                  breakeven by sign, and the twin's breakeven fidelity is solved.
    verification  tier thresholds u* = dv/(p.dq) from the tier table; tier ASSIGNMENT recomputed by
                  comparison rather than hard-coded; total cost of four regimes as verification plus
                  Sum(n.p.(1-q).u); the p = 0.04 case re-derived end to end, including the
                  threshold-tripling identity and the reassignment it causes. The escape-cost
                  asymmetry is checked both as a ratio of expected costs and as the
                  escape-probability ratio divided by the consequence ratio.
    fairness+risk recall by group, aggregate recall, per-clinic burden and the threshold-change
                  economics; then EAL = P.u for four control options with the sub-additivity result
                  and the A-vs-B and B-vs-nothing crossover probabilities solved algebraically.
"""


def run(ctx):
    check, D, mesh, ew = ctx["check"], ctx["D"], ctx["mesh"], ctx["ew"]
    COD_MER = ctx["MERIDIAN_COST_OF_DELAY"]          # USD 14,280 per week (Domain 1)
    BEN70 = ctx["MERIDIAN_BENEFIT_70PCT"]            # USD 685,440 per year
    BAC = ctx["AURIGA_BAC"]                          # USD 4,000,000

    COD_AUR = D(45000)                               # Auriga cost of delay per week (D6/D7)
    RATE_AUR = D("130.625")                          # Auriga blended engineering rate (D7 KA 7.4.1)
    RATE_MER = D(110)                                # Meridian blended internal rate (D11)
    CLINICS = 40

    # ---------------- 14.1.1 systems estate: the registered interface count -----------------
    check("14.1.1 mesh interfaces, 11 systems", mesh(11), 55)

    # ---------------- WE 14.1.4 Meridian CDE: defect rate by class --------------------------
    # (label, records, defect rate, consequence per defect)
    CDE = (("accounts", 4800, D("0.015"), D(250)),
           ("devices", 1600, D("0.040"), D(180)),
           ("mappings", 900, D("0.060"), D(1200)),
           ("training", 3600, D("0.025"), D(90)),
           ("readiness", 1200, D("0.030"), D(400)),
           ("reconciliation", 2000, D("0.008"), D(3500)))
    PRINTED_DEFECTS = {"accounts": 72, "devices": 64, "mappings": 54,
                       "training": 90, "readiness": 36, "reconciliation": 16}
    PRINTED_EXP = {"accounts": 18000, "devices": 11520, "mappings": 64800,
                   "training": 8100, "readiness": 14400, "reconciliation": 56000}
    PRINTED_PER_REC = {"accounts": "3.75", "devices": "7.20", "mappings": "72.00",
                       "training": "2.25", "readiness": "12.00", "reconciliation": "28.00"}
    recs = defects = exposure = maxexp = D(0)
    for lab, n, d, u in CDE:
        recs += D(n)
        defects += D(n) * d
        exposure += D(n) * d * u
        maxexp += D(n) * u
        check(f"WE 14.1.4 {lab} defects", D(n) * d, PRINTED_DEFECTS[lab])
        check(f"WE 14.1.4 {lab} exposure", D(n) * d * u, PRINTED_EXP[lab])
        check(f"WE 14.1.4 {lab} exposure per record", d * u, D(PRINTED_PER_REC[lab]))
    check("WE 14.1.4 total records", recs, 14100)
    check("WE 14.1.4 total defects", defects, 332)
    check("WE 14.1.4 total exposure", exposure, 172820)
    check("WE 14.1.4 weighted mean defect rate %", defects / recs * 100, D("2.3546"),
          D("0.00005"))
    check("WE 14.1.4 exposure if all records defective", maxexp, 10372000)
    check("WE 14.1.4 total exposure, indicative SAR", exposure * D("3.75"), 648075)
    check("Case A reported conformance %", (1 - defects / recs) * 100, D("97.6"), D("0.05"))
    # the top two classes: 20.6 % of records carrying 70.0 % of exposure (case study)
    check("Case A top-two record share %", (D(900) + D(2000)) / recs * 100, D("20.57"),
          D("0.005"))
    check("Case A top-two record count", D(900) + D(2000), 2900)
    check("Case A top-two exposure share %", (D(64800) + D(56000)) / exposure * 100, D("69.90"),
          D("0.005"))
    # uniform 2 % target: fewer defects, more cost — both derived
    UNI = D("0.02")
    uni_defects, uni_exposure = UNI * recs, UNI * maxexp
    check("WE 14.1.4 uniform 2 % defects", uni_defects, 282)
    check("WE 14.1.4 uniform 2 % exposure", uni_exposure, 207440)
    check("WE 14.1.4 uniform 2 % cost delta", uni_exposure - exposure, 34620)
    check("WE 14.1.4 uniform 2 % cost delta %", (uni_exposure - exposure) / exposure * 100,
          D("20.03"), D("0.005"))
    check("WE 14.1.4 uniform 2 % defect reduction %", (defects - uni_defects) / defects * 100,
          D("15.06"), D("0.005"))
    # remediation: saved, cost, net, and the breakeven cost per record (solved)
    for lab, n, d0, d1, u, cpr, saved, cost, net, be in (
            ("mappings", 900, D("0.060"), D("0.010"), D(1200), D(22), 54000, 19800, 34200, "60.00"),
            ("reconciliation", 2000, D("0.008"), D("0.002"), D(3500), D(12), 42000, 24000, 18000,
             "21.00"),
            ("training", 3600, D("0.025"), D("0.005"), D(90), D(8), 6480, 28800, -22320, "1.80")):
        s = D(n) * (d0 - d1) * u
        c = D(n) * cpr
        check(f"WE 14.1.4 remediate {lab} exposure removed", s, saved)
        check(f"WE 14.1.4 remediate {lab} cost", c, cost)
        check(f"WE 14.1.4 remediate {lab} net", s - c, net)
        check(f"WE 14.1.4 remediate {lab} breakeven per record", s / D(n), D(be))
    # Case A: the consequence-proportional schedule
    NEW = {"mappings": D("0.010"), "reconciliation": D("0.002")}
    nd = ne = D(0)
    for lab, n, d, u in CDE:
        dd = NEW.get(lab, d)
        nd += D(n) * dd
        ne += D(n) * dd * u
    check("Case A new defect count", nd, 275)
    check("Case A new exposure", ne, 76820)
    check("Case A new weighted mean rate %", nd / recs * 100, D("1.9504"), D("0.00005"))
    check("Case A remediation spend", D(19800) + D(24000), 43800)
    check("Case A exposure removed", exposure - ne, 96000)
    check("Case A net", exposure - ne - D(43800), 52200)
    check("Case A vs uniform target saving %", (uni_exposure - ne) / uni_exposure * 100,
          D("62.97"), D("0.005"))

    # ---------------- WE 14.2.1 information age: the registered E[wait] identity ------------
    per_day = COD_MER / 7
    check("WE 14.2.1 cost of delay per day", per_day, 2040)
    age_month, age_week = ew(30, 9), ew(7, 4)        # R/2 + G, the D3 identity on information
    check("WE 14.2.1 monthly information age, days", age_month, 24)
    check("WE 14.2.1 weekly information age, days", age_week, D("7.5"))
    check("WE 14.2.1 monthly age in weeks", age_month / 7, D("3.4286"), D("0.00005"))
    check("WE 14.2.1 monthly age priced", age_month * per_day, 48960)
    check("WE 14.2.1 weekly age priced", age_week * per_day, 15300)
    check("WE 14.2.1 saving per detectable slip", (age_month - age_week) * per_day, 33660)
    # lever asymmetry asserted against the general form, not the example
    check("WE 14.2.1 lever: 3 days off G", ew(30, 9) - ew(30, 6), 3)
    check("WE 14.2.1 lever: 3 days off R", ew(30, 9) - ew(27, 9), D("1.5"))
    # production cost of a shorter cycle
    check("WE 14.2.1 report production cost", RATE_MER * 14, 1540)
    check("WE 14.2.1 extra productions a month", D(30) / 7 - 1, D("3.2857"), D("0.00005"))
    check("WE 14.2.1 added monthly production cost", RATE_MER * 14 * (D(30) / 7 - 1), 5060,
          D("0.005"))

    # ---------------- 14.2.3 digital twin: breakeven fidelity -------------------------------
    twin_cost = D(180000) + D(3500) * 9
    twin_gross = D(62000) * 3 + D(120000)
    check("14.2.3 twin cost", twin_cost, 211500)
    check("14.2.3 twin gross benefit at perfect fidelity", twin_gross, 306000)
    check("14.2.3 twin breakeven fidelity %", twin_cost / twin_gross * 100, D("69.12"),
          D("0.005"))
    check("14.2.3 twin measured fidelity %", D(34) / D(40) * 100, 85)
    check("14.2.3 twin expected net at 85 %", D("0.85") * twin_gross - twin_cost, 48600)
    check("14.2.3 twin pooled fidelity %", D(64) / D(80) * 100, 80)
    check("14.2.3 twin expected net at 80 %", D("0.80") * twin_gross - twin_cost, 33300)
    check("14.2.3 twin margin above breakeven, points",
          (D(34) / D(40) - twin_cost / twin_gross) * 100, D("15.88"), D("0.005"))

    # ---------------- WE 14.2.4 automation breakeven volume ---------------------------------
    m_unit = RATE_AUR * 24 / 60                      # 24 minutes of engineer time
    a_unit = D("4.25")
    F_build, F_all = D(46000), D(46000) + D(1400) * 24
    U_ESC = D(1800)
    check("WE 14.2.4 manual unit cost", m_unit, D("52.25"))
    check("WE 14.2.4 fixed cost with maintenance", F_all, 79600)
    check("WE 14.2.4 breakeven on build cost only", F_build / (m_unit - a_unit), D("958.33"),
          D("0.005"))
    check("WE 14.2.4 naive breakeven with maintenance", F_all / (m_unit - a_unit), D("1658.33"),
          D("0.005"))
    qm = m_unit + D("0.015") * U_ESC
    qa_worse = a_unit + D("0.025") * U_ESC
    qa_better = a_unit + D("0.005") * U_ESC
    check("WE 14.2.4 quality-adjusted manual unit", qm, D("79.25"))
    check("WE 14.2.4 quality-adjusted automated unit (2.5 %)", qa_worse, D("49.25"))
    check("WE 14.2.4 quality-adjusted automated unit (0.5 %)", qa_better, D("13.25"))
    check("WE 14.2.4 quality-adjusted breakeven (2.5 %)", F_all / (qm - qa_worse), D("2653.33"),
          D("0.005"))
    check("WE 14.2.4 ratio to naive breakeven",
          (F_all / (qm - qa_worse)) / (F_all / (m_unit - a_unit)), D("1.60"))
    check("WE 14.2.4 quality-adjusted breakeven (0.5 %)", F_all / (qm - qa_better), D("1206.06"),
          D("0.005"))
    vol_proj = D(60) * 12
    check("WE 14.2.4 Auriga project volume", vol_proj, 720)
    check("WE 14.2.4 portfolio volume per year", D(6) * vol_proj, 4320)
    check("WE 14.2.4 portfolio volume over 24 months", D(6) * vol_proj * 2, 8640)
    check("WE 14.2.4 quality-adjusted 24-month net",
          D(8640) * (qm - qa_worse) - F_all, 179600)
    check("WE 14.2.4 naive 24-month net claim", D(8640) * (m_unit - a_unit) - F_all, 335120)
    check("WE 14.2.4 net removed by one point of escape",
          D(8640) * ((m_unit - a_unit) - (qm - qa_worse)), 155520)
    check("WE 14.2.4 visible saving on Auriga alone", vol_proj * (m_unit - a_unit), 34560)

    # ---------------- WE 14.3.3 verification standard: thresholds and regimes ---------------
    TIERS = ((D(0), D(0)),
             (RATE_MER * 6 / 60, D("0.35")),
             (RATE_MER * 24 / 60, D("0.70")),
             (RATE_MER * 66 / 60, D("0.90")),
             (RATE_MER * 150 / 60, D("0.97")))
    for k, printed_v in enumerate(("0", "11.00", "44.00", "121.00", "275.00")):
        check(f"WE 14.3.3 tier {k} cost per item", TIERS[k][0], D(printed_v))

    def thresholds(p):
        return [(TIERS[k][0] - TIERS[k - 1][0]) / (p * (TIERS[k][1] - TIERS[k - 1][1]))
                for k in range(1, 5)]

    P12, P04 = D("0.12"), D("0.04")
    th12, th04 = thresholds(P12), thresholds(P04)
    for k, printed in enumerate(("261.90", "785.71", "3208.33", "18333.33")):
        check(f"WE 14.3.3 threshold {k}->{k+1} at p=0.12", th12[k], D(printed), D("0.005"))
    for k, printed in enumerate(("785.71", "2357.14", "9625.00", "55000.00")):
        check(f"WE 14.3.3 threshold {k}->{k+1} at p=0.04", th04[k], D(printed), D("0.005"))
        check(f"WE 14.3.3 threshold {k} triples as p thirds", th04[k] / th12[k], 3)
    CLASSES = (("summaries", 620, D(150)), ("readiness", 180, D(480)),
               ("mappings", 95, D(1200)), ("scripts", 40, D(3500)),
               ("impact", 6, D(28560)))
    check("WE 14.3.3 impact assessment consequence = 2 weeks of delay", D(2) * COD_MER, 28560)
    check("WE 14.3.3 monthly output count", sum(n for _, n, _ in CLASSES), 941)

    def assign(ths):
        out = []
        for _, _, u in CLASSES:
            t = 0
            for k, th in enumerate(ths, start=1):
                if u >= th:
                    t = k
            out.append(t)
        return out

    def regime(tiers, p):
        vc = esc = D(0)
        for (lab, n, u), t in zip(CLASSES, tiers):
            vc += D(n) * TIERS[t][0]
            esc += D(n) * p * (1 - TIERS[t][1]) * u
        return vc, esc, vc + esc

    a12 = assign(th12)
    check("WE 14.3.3 tier assignment at p=0.12", D(int("".join(str(t) for t in a12))), 1234)
    v_t, e_t, tot_t = regime(a12, P12)
    check("WE 14.3.3 tiered verification cost", v_t, 12650)
    check("WE 14.3.3 tiered expected escaped loss", e_t, D("24300.096"))
    check("WE 14.3.3 tiered total", tot_t, D("36950.096"))
    v_2, e_2, tot_2 = regime([2] * 5, P12)
    check("WE 14.3.3 uniform tier 2 verification cost", v_2, 41404)
    check("WE 14.3.3 uniform tier 2 escaped loss", e_2, D("21771.36"))
    check("WE 14.3.3 uniform tier 2 total", tot_2, D("63175.36"))
    v_4, e_4, tot_4 = regime([4] * 5, P12)
    check("WE 14.3.3 uniform tier 4 verification cost", v_4, 258775)
    check("WE 14.3.3 uniform tier 4 total", tot_4, D("260952.136"))
    _, e_0, tot_0 = regime([0] * 5, P12)
    check("WE 14.3.3 no-verification total", tot_0, D("72571.20"))
    check("WE 14.3.3 tiered saving vs uniform 2", tot_2 - tot_t, D("26225.264"))
    check("WE 14.3.3 tiered saving vs uniform 2 %", (tot_2 - tot_t) / tot_2 * 100, D("41.51"),
          D("0.005"))
    check("WE 14.3.3 uniform 2 verification cost ratio", v_2 / v_t, D("3.27"), D("0.005"))
    check("WE 14.3.3 escaped-loss reduction from tripling verification", e_t - e_2,
          D("2528.736"))
    check("WE 14.3.3 escaped-loss reduction %", (e_t - e_2) / e_t * 100, D("10.41"), D("0.005"))
    check("WE 14.3.3 uniform tier 4 multiple of tiered", tot_4 / tot_t, D("7.06"), D("0.005"))
    esc_t0 = D(620) * P12 * D(150)
    check("WE 14.3.3 uniform tier 4 escaped loss", e_4, D("2177.136"))
    check("WE 14.3.3 consequence spread across classes", D(28560) / D(150), D("190.4"))
    check("WE 14.3.3 review-cost spread across tiers", TIERS[4][0] / TIERS[1][0], 25)
    check("WE 14.3.3 escaped loss in the unverified class", esc_t0, 11160)
    check("WE 14.3.3 unverified class share of escaped loss %", esc_t0 / e_t * 100, D("45.93"),
          D("0.005"))
    s_cost = D(620) * TIERS[1][0]
    s_saved = esc_t0 - D(620) * P12 * (1 - TIERS[1][1]) * D(150)
    check("WE 14.3.3 tier 1 on summaries cost", s_cost, 6820)
    check("WE 14.3.3 tier 1 on summaries saving", s_saved, 3906)
    check("WE 14.3.3 tier 1 on summaries net", s_saved - s_cost, -2914)
    a04 = assign(th04)
    check("WE 14.3.3 tier assignment at p=0.04", D(int("".join(str(t) for t in a04))), 123)
    v_b, e_b, tot_b = regime(a04, P04)
    check("WE 14.3.3 p=0.04 verification cost", v_b, 3531)
    check("WE 14.3.3 p=0.04 escaped loss", e_b, D("12505.44"))
    check("WE 14.3.3 p=0.04 total", tot_b, D("16036.44"))
    check("WE 14.3.3 p=0.04 total fall %", (tot_t - tot_b) / tot_t * 100, D("56.60"), D("0.005"))
    check("WE 14.3.3 p=0.04 verification bill fall %", (v_t - v_b) / v_t * 100, D("72.09"),
          D("0.005"))

    # ---------------- WE 14.3.4 the plausible wrong number ---------------------------------
    PV, EV, AC = D(2080000), D(1920000), D(2120000)
    cpi, spi = EV / AC, EV / PV
    check("WE 14.3.4 CPI", cpi, D("0.91"), D("0.005"))
    check("WE 14.3.4 SPI", spi, D("0.92"), D("0.005"))
    eac = BAC / cpi
    check("WE 14.3.4 EAC from CPI", eac, D("4416666.67"), D("0.005"))
    check("WE 14.3.4 VAC", BAC - eac, D("-416666.67"), D("0.005"))
    eac_rounded = BAC / D("0.91")
    check("WE 14.3.4 EAC from rounded CPI", eac_rounded, D("4395604.40"), D("0.005"))
    check("WE 14.3.4 understatement from rounding", eac - eac_rounded, D("21062.27"), D("0.005"))
    check("WE 14.3.4 understatement as % of VAC",
          (eac - eac_rounded) / (eac - BAC) * 100, D("5.05"), D("0.005"))
    check("WE 14.3.4 plausible forecast overrun", D(4320000) - BAC, 320000)
    check("WE 14.3.4 plausible error forecast gap", eac - D(4320000), D("96666.67"), D("0.005"))
    check("WE 14.3.4 plausible gap as % of BAC", (eac - D(4320000)) / BAC * 100, D("2.42"),
          D("0.005"))
    cons_p = D(6) * COD_AUR
    check("WE 14.3.4 consequence of the missed funding request", cons_p, 270000)
    pe, qo, qp, cons_o = D("0.10"), D("0.98"), D("0.25"), D(900000)
    exp_o, exp_p = pe * (1 - qo) * cons_o, pe * (1 - qp) * cons_p
    check("WE 14.3.4 expected escaped cost, obvious error", exp_o, 1800)
    check("WE 14.3.4 expected escaped cost, plausible error", exp_p, 20250)
    check("WE 14.3.4 asymmetry ratio", exp_p / exp_o, D("11.25"))
    check("WE 14.3.4 escape-probability ratio", (1 - qp) / (1 - qo), D("37.50"))
    check("WE 14.3.4 consequence ratio", cons_o / cons_p, D("3.3333"), D("0.00005"))
    check("WE 14.3.4 asymmetry decomposition",
          ((1 - qp) / (1 - qo)) / (cons_o / cons_p), exp_p / exp_o)
    rep_cost = RATE_AUR * 40 / 60
    check("WE 14.3.4 reperformance cost", rep_cost, D("87.08"), D("0.005"))
    exp_p2 = pe * (1 - D("0.85")) * cons_p
    check("WE 14.3.4 expected escaped cost after reperformance", exp_p2, 4050)
    check("WE 14.3.4 reperformance saving", exp_p - exp_p2, 16200)
    check("WE 14.3.4 reperformance return, times cost", (exp_p - exp_p2) / rep_cost, D("186.02"),
          D("0.03"))
    check("WE 14.3.4 breakeven detection improvement %", rep_cost / (pe * cons_p) * 100,
          D("0.3225"), D("0.00005"))

    # ---------------- WE 14.4.2 differential error by clinic group -------------------------
    URB_N, URB_R, URB_F = 24, 9, 8
    RUR_N, RUR_R, RUR_F = 16, 11, 6
    check("WE 14.4.2 clinic total", URB_N + RUR_N, CLINICS)
    check("WE 14.4.2 urban recall %", D(URB_F) / D(URB_R) * 100, D("88.89"), D("0.005"))
    check("WE 14.4.2 rural recall %", D(RUR_F) / D(RUR_R) * 100, D("54.55"), D("0.005"))
    check("WE 14.4.2 aggregate recall %", D(URB_F + RUR_F) / D(URB_R + RUR_R) * 100, 70)
    check("WE 14.4.2 recall gap, points",
          (D(URB_F) / D(URB_R) - D(RUR_F) / D(RUR_R)) * 100, D("34.34"), D("0.005"))
    check("WE 14.4.2 urban base rate %", D(URB_R) / D(URB_N) * 100, D("37.50"))
    check("WE 14.4.2 rural base rate %", D(RUR_R) / D(RUR_N) * 100, D("68.75"))
    excess = D(6400) - D(1900)
    miss_u, miss_r = URB_R - URB_F, RUR_R - RUR_F
    check("WE 14.4.2 excess cost per missed clinic", excess, 4500)
    check("WE 14.4.2 total excess cost", D(miss_u + miss_r) * excess, 27000)
    check("WE 14.4.2 rural share of excess %", D(miss_r) / D(miss_u + miss_r) * 100, D("83.33"),
          D("0.005"))
    check("WE 14.4.2 rural share of clinics %", D(RUR_N) / D(CLINICS) * 100, 40)
    per_urb, per_rur = D(miss_u) * excess / D(URB_N), D(miss_r) * excess / D(RUR_N)
    check("WE 14.4.2 urban excess per clinic", per_urb, D("187.50"))
    check("WE 14.4.2 rural excess per clinic", per_rur, D("1406.25"))
    check("WE 14.4.2 per-clinic burden ratio", per_rur / per_urb, D("7.50"))
    check("WE 14.4.2 corrected rural recall %", D(10) / D(11) * 100, D("90.91"), D("0.005"))
    avoided, extra = D(4) * excess, D(2) * D(1900)
    check("WE 14.4.2 emergency cost avoided", avoided, 18000)
    check("WE 14.4.2 extra planned support", extra, 3800)
    check("WE 14.4.2 net of correcting the differential", avoided - extra, 14200)
    check("WE 14.4.2 return on correcting, times cost", avoided / extra, D("4.74"), D("0.005"))
    check("14.4.2 benefit per clinic-year", BEN70 / CLINICS, 17136)
    check("14.4.2 benefit per clinic-week", BEN70 / CLINICS / 52, D("329.54"), D("0.005"))

    # ---------------- WE 14.4.4 security control economics --------------------------------
    P0, IMP = D("0.08"), D(480000)
    eal0 = P0 * IMP
    check("WE 14.4.4 baseline EAL", eal0, 38400)
    A_C, A_P = D(26000), D("0.02")
    B_C, B_IMP = D(14000), D(260000)
    tot_A = A_C + A_P * IMP
    tot_B = B_C + P0 * B_IMP
    tot_AB = (A_C + B_C) + A_P * B_IMP
    check("WE 14.4.4 control A residual EAL", A_P * IMP, 9600)
    check("WE 14.4.4 control A total", tot_A, 35600)
    check("WE 14.4.4 control B residual EAL", P0 * B_IMP, 20800)
    check("WE 14.4.4 control B total", tot_B, 34800)
    check("WE 14.4.4 both controls residual EAL", A_P * B_IMP, 5200)
    check("WE 14.4.4 both controls total", tot_AB, 45200)
    check("WE 14.4.4 both worse than B alone by", tot_AB - tot_B, 10400)
    check("WE 14.4.4 A avoided loss", eal0 - A_P * IMP, 28800)
    check("WE 14.4.4 B avoided loss", eal0 - P0 * B_IMP, 17600)
    check("WE 14.4.4 naive sum of avoided losses",
          (eal0 - A_P * IMP) + (eal0 - P0 * B_IMP), 46400)
    check("WE 14.4.4 true combined avoided loss", eal0 - A_P * B_IMP, 33200)
    check("WE 14.4.4 overstatement of combined benefit",
          ((eal0 - A_P * IMP) + (eal0 - P0 * B_IMP)) - (eal0 - A_P * B_IMP), 13200)
    check("WE 14.4.4 A avoided per dollar", (eal0 - A_P * IMP) / A_C, D("1.11"), D("0.005"))
    check("WE 14.4.4 B avoided per dollar", (eal0 - P0 * B_IMP) / B_C, D("1.26"), D("0.005"))
    # crossovers solved: A total = A_C + (A_P/P)*P*IMP, so slope kA; B slope kB
    kA, kB = (A_P / P0) * IMP, B_IMP
    p_star = (A_C - B_C) / (kB - kA)
    check("WE 14.4.4 A-vs-B crossover probability %", p_star * 100, D("8.5714"), D("0.00005"))
    check("WE 14.4.4 crossover margin above assessed P, points", (p_star - P0) * 100, D("0.57"),
          D("0.005"))
    check("WE 14.4.4 B-vs-nothing crossover probability %", B_C / (IMP - B_IMP) * 100,
          D("6.3636"), D("0.00005"))
    check("WE 14.4.4 A-vs-nothing crossover probability %", A_C / (IMP - kA) * 100,
          D("7.2222"), D("0.00005"))

    # ---------------- Case study B: automated exception triage (financial services) --------
    RATE_FS = D("112.80")
    m_fs = RATE_FS * 5 / 60
    a_fs, U_FS = D("1.15"), D(640)
    F_fs = D(128000) + D(2600) * 18
    vol_fs = D(9600)
    check("Case B manual unit cost", m_fs, D("9.40"))
    check("Case B fixed cost over 18 months", F_fs, 174800)
    check("Case B visible unit saving", m_fs - a_fs, D("8.25"))
    check("Case B claimed monthly saving", (m_fs - a_fs) * vol_fs, 79200)
    check("Case B naive breakeven volume", F_fs / (m_fs - a_fs), D("21187.88"), D("0.005"))
    check("Case B naive breakeven, months", F_fs / (m_fs - a_fs) / vol_fs, D("2.21"), D("0.005"))
    qm_fs = m_fs + D("0.008") * U_FS
    qa_fs = a_fs + D("0.035") * U_FS
    check("Case B quality-adjusted manual unit", qm_fs, D("14.52"))
    check("Case B quality-adjusted automated unit", qa_fs, D("23.55"))
    check("Case B unit loss against automation", qa_fs - qm_fs, D("9.03"))
    check("Case B monthly value destroyed", (qa_fs - qm_fs) * vol_fs, 86688)
    check("Case B swing between claim and reality per month",
          (m_fs - a_fs) * vol_fs + (qa_fs - qm_fs) * vol_fs, 165888)
    check("Case B escaped-error cost over 18 months", (qa_fs - qm_fs) * vol_fs * 18, 1560384)
    check("Case B no breakeven exists (sign of unit saving)",
          D(1) if (qm_fs - qa_fs) > 0 else D(-1), -1)
    layer = D("0.12") * D("2.10")
    qa_fix = a_fs + layer + D("0.009") * U_FS
    check("Case B containment layer cost per unit averaged", layer, D("0.252"))
    check("Case B automated unit with containment", qa_fix, D("7.162"))
    check("Case B unit saving with containment", qm_fs - qa_fix, D("7.358"))
    check("Case B monthly saving with containment", (qm_fs - qa_fix) * vol_fs, D("70636.80"))
    check("Case B breakeven with containment, units", F_fs / (qm_fs - qa_fix), D("23756.46"),
          D("0.005"))
    check("Case B breakeven with containment, months",
          F_fs / (qm_fs - qa_fix) / vol_fs, D("2.47"), D("0.005"))
    check("Case B 18-month net with containment",
          (qm_fs - qa_fix) * vol_fs * 18 - F_fs, D("1096662.40"))

    # ---------------- Exercise 14.1 — CDE defect classes ----------------------------------
    EX1 = ((2500, D("0.050"), D(260), 125, 32500, "13.00"),
           (900, D("0.010"), D(4800), 9, 43200, "48.00"),
           (1800, D("0.030"), D(150), 54, 8100, "4.50"),
           (600, D("0.080"), D(90), 48, 4320, "7.20"))
    r1 = d1 = e1 = mx1 = D(0)
    for n, d, u, pdef, pexp, pper in EX1:
        r1 += D(n)
        d1 += D(n) * d
        e1 += D(n) * d * u
        mx1 += D(n) * u
        check(f"Ex 14.1 class n={n} defects", D(n) * d, pdef)
        check(f"Ex 14.1 class n={n} exposure", D(n) * d * u, pexp)
        check(f"Ex 14.1 class n={n} exposure per record", d * u, D(pper))
    check("Ex 14.1 records", r1, 5800)
    check("Ex 14.1 defects", d1, 236)
    check("Ex 14.1 exposure", e1, 88120)
    check("Ex 14.1 weighted mean rate %", d1 / r1 * 100, D("4.0690"), D("0.00005"))
    check("Ex 14.1 class B share of exposure %", D(43200) / e1 * 100, D("49.02"), D("0.005"))
    check("Ex 14.1 uniform 2 % defects", D("0.02") * r1, 116)
    check("Ex 14.1 uniform 2 % count reduction %", (d1 - D("0.02") * r1) / d1 * 100, D("50.8"),
          D("0.05"))
    check("Ex 14.1 uniform 2 % max exposure base", mx1, 5294000)
    check("Ex 14.1 uniform 2 % exposure", D("0.02") * mx1, 105880)
    check("Ex 14.1 uniform 2 % delta", D("0.02") * mx1 - e1, 17760)
    check("Ex 14.1 uniform 2 % delta %", (D("0.02") * mx1 - e1) / e1 * 100, D("20.15"),
          D("0.005"))

    # ---------------- Exercise 14.2 — automation breakeven --------------------------------
    F2 = D(62400) + D(900) * 24
    m2, a2, U2 = D("31.20"), D("3.60"), D(720)
    check("Ex 14.2 fixed cost", F2, 84000)
    check("Ex 14.2 naive breakeven", F2 / (m2 - a2), D("3043.48"), D("0.005"))
    qm2 = m2 + D("0.010") * U2
    qa2 = a2 + D("0.040") * U2
    check("Ex 14.2 quality-adjusted manual unit", qm2, D("38.40"))
    check("Ex 14.2 quality-adjusted automated unit", qa2, D("32.40"))
    check("Ex 14.2 quality-adjusted breakeven", F2 / (qm2 - qa2), 14000)
    check("Ex 14.2 ratio to naive", (F2 / (qm2 - qa2)) / (F2 / (m2 - a2)), D("4.60"), D("0.005"))
    check("Ex 14.2 volume over horizon", D(500) * 24, 12000)
    qa2f = a2 + D("1.80") + D("0.010") * U2
    check("Ex 14.2 automated unit with containment", qa2f, D("12.60"))
    check("Ex 14.2 unit saving with containment", qm2 - qa2f, D("25.80"))
    check("Ex 14.2 breakeven with containment", F2 / (qm2 - qa2f), D("3255.81"), D("0.005"))
    check("Ex 14.2 24-month net with containment", D(12000) * (qm2 - qa2f) - F2, 225600)

    # ---------------- Exercise 14.3 — verification tiers ----------------------------------
    T3 = ((D(0), D(0)), (D("14.00"), D("0.40")), (D("42.00"), D("0.75")),
          (D("126.00"), D("0.92")))

    def th3(p):
        return [(T3[k][0] - T3[k - 1][0]) / (p * (T3[k][1] - T3[k - 1][1])) for k in range(1, 4)]

    for k, printed in enumerate(("437.50", "1000.00", "6176.47")):
        check(f"Ex 14.3 threshold {k}->{k+1} at p=0.08", th3(D("0.08"))[k], D(printed), D("0.005"))
    for k, printed in enumerate(("218.75", "500.00", "3088.24")):
        check(f"Ex 14.3 threshold {k}->{k+1} at p=0.16", th3(D("0.16"))[k], D(printed), D("0.005"))
    for u, printed_tier in ((D(900), 1), (D(4000), 2), (D(20000), 3)):
        t = 0
        for k, th in enumerate(th3(D("0.08")), start=1):
            if u >= th:
                t = k
        check(f"Ex 14.3 tier for consequence {int(u)}", t, printed_tier)
    check("Ex 14.3 value of tier 3 over tier 2 at u=900",
          D("0.08") * D("0.17") * D(900), D("12.24"))

    # ---------------- Exercise 14.4 — plausible vs obvious --------------------------------
    pe4, qo4, qp4, uo4, up4 = D("0.15"), D("0.99"), D("0.30"), D(620000), D(180000)
    eo4, ep4 = pe4 * (1 - qo4) * uo4, pe4 * (1 - qp4) * up4
    check("Ex 14.4 obvious expected escaped cost", eo4, 930)
    check("Ex 14.4 plausible expected escaped cost", ep4, 18900)
    check("Ex 14.4 asymmetry ratio", ep4 / eo4, D("20.32"), D("0.005"))
    check("Ex 14.4 escape-probability ratio", (1 - qp4) / (1 - qo4), 70)
    check("Ex 14.4 consequence ratio", uo4 / up4, D("3.4444"), D("0.00005"))
    check("Ex 14.4 decomposition", ((1 - qp4) / (1 - qo4)) / (uo4 / up4), ep4 / eo4)
    check("Ex 14.4 reperformance saving", pe4 * (D("0.88") - qp4) * up4, 15660)
    check("Ex 14.4 reperformance return, times cost",
          pe4 * (D("0.88") - qp4) * up4 / D(145), 108)
    check("Ex 14.4 breakeven detection improvement %", D(145) / (pe4 * up4) * 100, D("0.5370"),
          D("0.00005"))

    # ---------------- Exercise 14.5 — security controls -----------------------------------
    P5, IMP5 = D("0.12"), D(750000)
    eal5 = P5 * IMP5
    X_C, X_P, Y_C, Y_IMP = D(34000), D("0.03"), D(21000), D(400000)
    check("Ex 14.5 baseline EAL", eal5, 90000)
    check("Ex 14.5 X total", X_C + X_P * IMP5, 56500)
    check("Ex 14.5 Y total", Y_C + P5 * Y_IMP, 69000)
    check("Ex 14.5 X and Y total", (X_C + Y_C) + X_P * Y_IMP, 67000)
    check("Ex 14.5 both worse than X alone by",
          ((X_C + Y_C) + X_P * Y_IMP) - (X_C + X_P * IMP5), 10500)
    check("Ex 14.5 combined avoided loss", eal5 - X_P * Y_IMP, 78000)
    check("Ex 14.5 naive sum of avoided losses",
          (eal5 - X_P * IMP5) + (eal5 - P5 * Y_IMP), 109500)
    check("Ex 14.5 naive overstatement",
          ((eal5 - X_P * IMP5) + (eal5 - P5 * Y_IMP)) - (eal5 - X_P * Y_IMP), 31500)
    check("Ex 14.5 X-vs-Y crossover probability %",
          (X_C - Y_C) / (Y_IMP - (X_P / P5) * IMP5) * 100, D("6.1176"), D("0.00005"))

    # ---------------- MCQ distractors that are computed, not invented ----------------------
    check("MCQ 14.1-A distractor: defect count", D(900) * D("0.06"), 54)
    check("MCQ 14.1-A distractor: exposure per record", D("0.06") * D(1200), 72)
    check("MCQ 14.1-A distractor: all records priced", D(900) * D(1200), 1080000)
    check("MCQ 14.2-A distractor: R + G", D(30) + D(9), 39)
    check("MCQ 14.2-A distractor: R/2 alone", D(30) / 2, 15)
    check("MCQ 14.2-C distractor: fixed over manual unit", F_all / m_unit, D("1523.44"),
          D("0.005"))
    check("MCQ 14.2-E distractor: build cost over benefit %", D(180000) / twin_gross * 100,
          D("58.82"), D("0.005"))
    check("MCQ 14.2-E distractor: cost over one stream %", twin_cost / D(186000) * 100,
          D("113.71"), D("0.005"))
    check("MCQ 14.3-A distractor: increment over total q",
          D(77) / (P12 * D("0.90")), D("712.96"), D("0.005"))
    check("MCQ 14.3-A distractor: total v over total q",
          D(121) / (P12 * D("0.90")), D("1120.37"), D("0.005"))
    check("MCQ 14.3-A distractor: total v over increment",
          D(121) / (P12 * D("0.20")), D("5041.67"), D("0.005"))
    check("MCQ 14.4-A distractor: flagged over all rural clinics",
          D(6) / D(16) * 100, D("37.50"))
