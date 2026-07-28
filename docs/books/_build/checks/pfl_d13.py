"""PFL-AI Domain 13 — Due diligence and financial close: golden checks.

Every number printed as a result in `domain-13-diligence-financial-close.md` is recomputed here
from first principles. Master-thread values are read from ctx, never re-typed.

The domain has five engines:

  1. Diligence stream economics (KA 13.1.3) — for each stream, expected cost without the stream
     is p*C; with it, fee + priced elapsed time + p*[d*F + (1-d)*C]. The breakeven detection rate
     is d* = (fee + priced elapsed time) / [p * (C - F)]: the diligence spend divided by the
     expected avoidable loss. Elapsed time is priced once for a parallel envelope and once per
     stream in series, which is the whole of the 5,213,600 difference.
  2. The cost of delay to financial close (KA 13.1, throughout) — Kestrel's concession expiry is
     fixed, so a day's slip destroys a day of operating life: annual CFADS / 360 per day on a
     30/360 basis (Domain 5, KA 5.4.2), and seven of those per calendar week.
  3. Model-audit Class 1 findings (KA 13.2.3) — a definitional finding moves CFADS and therefore
     DSCR and debt capacity (CFADS / target DSCR * AF); a convention finding replaces the
     in-arrears instalment with an annuity-due instalment, AF_due = AF * (1 + r), which is smaller
     and so flatters the ratio.
  4. The conditions-precedent conjunction (KA 13.3.2-13.3.3) — close is the maximum of the chains'
     realised durations, each chain being base duration plus a single independent slip. E[max] is
     enumerated over all 2^k outcomes; the conjunction premium is the excess of the expected slip
     cost over the cost of the critical chain's own expected slip.
  5. Close costs and syndication (KA 13.3.4, 13.4) — close cost = fixed component F + k * debt,
     so its share of debt is F/D + k; the all-in cost of debt solves net proceeds = instalment *
     AF(r, n); the syndication fee splits into a praecipium plus a pool rate on final allocations;
     an underwriter's carry is charged on the excess over target hold, not the residual hold; and
     market flex is valued as an annuity, facility * flex * AF.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DEBT = ctx["KESTREL_DEBT"]
    CAPEX = ctx["KESTREL_CAPEX"]
    DS = ctx["KESTREL_INSTALMENT"]
    CF = ctx["KESTREL_CFADS"]
    RATE = ctx["KESTREL_RATE"]
    TEN = ctx["KESTREL_TENOR"]
    AF6_12 = D("8.383844")            # the registry literal the manuscript multiplies by
    EPC = D(48000000)

    def solve_af(target, n, lo=D("0.0001"), hi=D("0.9")):
        """Rate r with AF(r, n) = target, by bisection on a monotone decreasing function."""
        for _ in range(400):
            mid = (lo + hi) / 2
            if af(mid, n) > target:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    # ---------- master-thread anchors this domain reads ----------
    check("D13 AF(0.06,12) registry literal", af(RATE, TEN).quantize(D("0.000001")), AF6_12)
    check("D13 DSCR at close", (CF / DS).quantize(D("0.0001")), D("1.2743"))
    check("D13 covenant cash trigger 1.20x", (DS * D("1.20")).quantize(D("0.01")),
          D("6011562.28"))
    check("D13 capacity at 1.30x on uncorrected CFADS (Domain 10)",
          (CF / D("1.30") * AF6_12).quantize(D("1")), 41171123)

    # ---------- engine 2: the cost of delay to financial close ----------
    COD_D = CF / 360
    COD_W = COD_D * 7
    check("D13 cost of delay per day (30/360, Domain 5)", COD_D.quantize(D("0.01")),
          D("17733.33"))
    check("D13 cost of delay per calendar week", COD_W.quantize(D("0.01")), D("124133.33"))
    check("D13 twelve-week parallel envelope delay", (12 * COD_W).quantize(D("0.01")), 1489600)
    check("D13 fifty-four-week serial delay", (54 * COD_W).quantize(D("0.01")), 6703200)
    check("D13 the 42-week scheduling difference", (42 * COD_W).quantize(D("0.01")), 5213600)
    check("D13 insurance stream four-week serial delay", (4 * COD_W).quantize(D("0.01")),
          D("496533.33"))

    # ---------- engine 1: the seven diligence streams ----------
    # (name, fee, weeks, p, C, F)
    streams = [
        ("technical", D(260000), 8, D("0.30"), D(6200000), D(400000)),
        ("financial", D(180000), 5, D("0.35"), D("2691071"), D(60000)),
        ("legal", D(420000), 10, D("0.25"), D(9000000), D(250000)),
        ("tax", D(150000), 6, D("0.20"), D(4800000), D(300000)),
        ("insurance", D(60000), 4, D("0.15"), D(1400000), D(120000)),
        ("E&S", D(240000), 12, D("0.22"), D(7500000), D(900000)),
        ("market", D(190000), 9, D("0.28"), D(5600000), D(500000)),
    ]
    printed_pc = {"technical": 1860000, "financial": D("941874.85"), "legal": 2250000,
                  "tax": 960000, "insurance": 210000, "E&S": 1650000, "market": 1568000}
    printed_avoid = {"technical": 1740000, "financial": D("920874.85"), "legal": 2187500,
                     "tax": 900000, "insurance": 192000, "E&S": 1452000, "market": 1428000}
    printed_env = {"technical": D("14.94"), "financial": D("19.55"), "legal": D("19.20"),
                   "tax": D("16.67"), "insurance": D("31.25"), "E&S": D("16.53"),
                   "market": D("13.31")}
    printed_ser = {"technical": D("72.02"), "financial": D("86.95"), "legal": D("75.95"),
                   "tax": D("99.42"), "insurance": D("289.86"), "E&S": D("119.12"),
                   "market": D("91.54")}
    d_use = D("0.80")
    tot_fee, tot_wk, sum_pc, sum_res = D(0), 0, D(0), D(0)
    for nm, fee, wk, p, C, F in streams:
        avoid = p * (C - F)
        check(f"WE 13.1.3 {nm} p*C", (p * C).quantize(D("0.01")), printed_pc[nm])
        check(f"WE 13.1.3 {nm} expected avoidable loss", avoid.quantize(D("0.01")),
              printed_avoid[nm])
        check(f"WE 13.1.3 {nm} d* inside the envelope %",
              (fee / avoid * 100).quantize(D("0.01")), printed_env[nm])
        check(f"WE 13.1.3 {nm} d* sequenced serially %",
              ((fee + wk * COD_W) / avoid * 100).quantize(D("0.01")), printed_ser[nm])
        tot_fee += fee
        tot_wk += wk
        sum_pc += p * C
        sum_res += p * (d_use * F + (1 - d_use) * C)
    check("WE 13.1.3 total diligence fees", tot_fee, 1500000)
    check("WE 13.1.3 total stream weeks", tot_wk, 54)
    check("WE 13.1.3 longest stream sets the envelope", max(s[2] for s in streams), 12)
    check("WE 13.1.3 fees as % of debt raised", (tot_fee / DEBT * 100).quantize(D("0.001")),
          D("3.571"))
    check("WE 13.1.3 expected cost with no diligence", sum_pc.quantize(D("0.01")),
          D("9439874.85"))
    check("WE 13.1.3 residual expected cost at d = 0.80", sum_res.quantize(D("0.01")),
          D("2383574.97"))
    par = tot_fee + 12 * COD_W + sum_res
    ser = tot_fee + tot_wk * COD_W + sum_res
    check("WE 13.1.3 expected cost, parallel envelope", par.quantize(D("0.01")),
          D("5373174.97"))
    check("WE 13.1.3 net value, parallel envelope", (sum_pc - par).quantize(D("0.01")),
          D("4066699.88"))
    check("WE 13.1.3 expected cost, serial sequencing", ser.quantize(D("0.01")),
          D("10586774.97"))
    check("WE 13.1.3 net value, serial sequencing", (sum_pc - ser).quantize(D("0.01")),
          D("-1146900.12"))
    check("WE 13.1.3 the difference is 42 weeks of scheduling",
          (ser - par).quantize(D("0.01")), 5213600)
    check("Case A scheduling difference as a multiple of the fee budget",
          ((ser - par) / tot_fee).quantize(D("0.001")), D("3.476"))
    check("MCQ 13.1-A distractor A: fee / (p*C)",
          (D(260000) / D(1860000) * 100).quantize(D("0.01")), D("13.98"))
    check("MCQ 13.1-A distractor C: fee / C",
          (D(260000) / D(6200000) * 100).quantize(D("0.01")), D("4.19"))

    # ---------- engine 3: model-audit Class 1 findings ----------
    CF_CORR = CF - D(260000)
    check("WE 13.2.3 F-01 corrected CFADS", CF_CORR, 6124000)
    check("WE 13.2.3 F-01 corrected DSCR", (CF_CORR / DS).quantize(D("0.0001")), D("1.2224"))
    AF_DUE = AF6_12 * (1 + RATE)
    check("WE 13.2.3 F-02 annuity-due factor", AF_DUE.quantize(D("0.000001")), D("8.886875"))
    INST_DUE = DEBT / AF_DUE
    check("WE 13.2.3 F-02 annuity-due instalment", INST_DUE.quantize(D("1")), 4726071)
    check("WE 13.2.3 F-02 instalment understatement", (DS - INST_DUE).quantize(D("1")),
          283564)
    check("WE 13.2.3 F-02 flattered DSCR on reported CFADS",
          (CF / INST_DUE).quantize(D("0.0001")), D("1.3508"))
    check("WE 13.2.3 F-01+F-02 reported DSCR on the wrong instalment",
          (CF_CORR / INST_DUE).quantize(D("0.0001")), D("1.2958"))
    check("WE 13.2.3 sustainable debt service at 1.30x",
          (CF_CORR / D("1.30")).quantize(D("0.01")), D("4710769.23"))
    CAP130 = CF_CORR / D("1.30") * AF6_12
    check("WE 13.2.3 debt capacity at 1.30x on corrected CFADS", CAP130.quantize(D("1")),
          39494354)
    check("WE 13.2.3 resize against the 42,000,000 mandate",
          (DEBT - CAP130).quantize(D("1")), 2505646)
    check("WE 13.2.3 F-03 six-month DSRA left unfunded", (DS * 6 / 12).quantize(D("0.01")),
          D("2504817.62"))            # printed as 2,504,818 (Domain 10, KA 10.3.2)
    check("MCQ 13.2-C distractor D: capacity with no coverage divisor",
          (CF_CORR * AF6_12).quantize(D("1")), 51342661)
    # finding classes: 3 / 7 / 24 = 34
    check("KA 13.2.2 finding total", 3 + 7 + 24, 34)
    check("KA 13.2.2 Class 3 share of the count %",
          (D(24) / D(34) * 100).quantize(D("0.1")), D("70.6"))
    check("KA 13.2.2 the 31-of-34 closure metric %",
          (D(31) / D(34) * 100).quantize(D("0.1")), D("91.2"))
    check("KA 13.2.2 materiality of 0.01x DSCR in annual cash",
          (DS * D("0.01")).quantize(D("0.01")), D("50096.35"))

    # ---------- engine 4: the conditions-precedent conjunction ----------
    def conjunction(chains):
        """chains = [(base weeks, slip probability, slip weeks)]; returns (E[max], {max: prob})."""
        total, dist = D(0), {}
        outcomes = [(D(1), [])]
        for base, p, slip in chains:
            nxt = []
            for pr, durs in outcomes:
                nxt.append((pr * (1 - p), durs + [D(base)]))
                nxt.append((pr * p, durs + [D(base + slip)]))
            outcomes = nxt
        for pr, durs in outcomes:
            m = max(durs)
            total += pr * m
            dist[m] = dist.get(m, D(0)) + pr
        return total, dist

    KCH = [(9, D("0.20"), 3), (18, D("0.35"), 6), (20, D("0.30"), 4),
           (17, D("0.25"), 5), (7, D("0.15"), 2)]
    base_close = max(c[0] for c in KCH)
    check("WE 13.3.2 base close date (weeks)", base_close, 20)
    check("WE 13.3.2 sum of the five chains (weeks)", sum(c[0] for c in KCH), 71)
    exp_dur = [(D(b) + p * s) for b, p, s in KCH]
    for got, want, nm in zip(exp_dur, ["9.60", "20.10", "21.20", "18.25", "7.30"], "ABCDE"):
        check(f"WE 13.3.2 chain {nm} expected duration", got.quantize(D("0.01")), D(want))
    check("WE 13.3.2 max of the expectations", max(exp_dur).quantize(D("0.0001")), D("21.2000"))
    E, dist = conjunction(KCH)
    check("WE 13.3.2 E[close] weeks", E.quantize(D("0.0001")), D("22.4075"))
    check("WE 13.3.2 P(close at 20 weeks) %",
          (dist[D(20)] * 100).quantize(D("0.001")), D("34.125"))
    check("WE 13.3.2 P(close at 22 weeks) %",
          (dist[D(22)] * 100).quantize(D("0.001")), D("11.375"))
    check("WE 13.3.2 P(close at 24 weeks) %",
          (dist[D(24)] * 100).quantize(D("0.001")), D("54.500"))
    check("WE 13.3.2 expected slip (weeks)", (E - base_close).quantize(D("0.0001")),
          D("2.4075"))
    check("WE 13.3.2 expected slip cost", ((E - base_close) * COD_W).quantize(D("0.01")),
          298851)
    check("WE 13.3.2 chain C's own expected slip cost",
          (D("1.2") * COD_W).quantize(D("0.01")), 148960)
    check("WE 13.3.2 conjunction premium",
          ((E - base_close) * COD_W - D("1.2") * COD_W).quantize(D("0.01")), 149891)
    check("WE 13.3.2 expected cost as a multiple of the critical chain's own slip",
          ((E - base_close) / D("1.2")).quantize(D("0.0001")), D("2.0062"))
    # float and criticality risk
    for (b, p, s), flt, expo, rw in zip(KCH, [11, 2, 0, 3, 13], [0, 4, 4, 2, 0],
                                        ["0.60", "2.10", "1.20", "1.25", "0.30"]):
        check(f"KA 13.3.3 chain float for a {b}-week chain", base_close - b, flt)
        check(f"KA 13.3.3 exposed slip for a {b}-week chain", max(0, b + s - base_close), expo)
        check(f"KA 13.3.3 criticality risk for a {b}-week chain",
              (p * s).quantize(D("0.01")), D(rw))
    KCH2 = [(9, D("0.20"), 3), (18, D("0.10"), 6), (20, D("0.30"), 4),
            (17, D("0.25"), 5), (7, D("0.15"), 2)]
    E2, dist2 = conjunction(KCH2)
    check("WE 13.3.3 E[close] with chain B de-risked", E2.quantize(D("0.0001")), D("21.7950"))
    check("WE 13.3.3 expected slip cost with chain B de-risked",
          ((E2 - base_close) * COD_W).quantize(D("0.01")), D("222819.33"))
    check("WE 13.3.3 value of de-risking chain B", ((E - E2) * COD_W).quantize(D("0.01")),
          D("76031.67"))
    check("WE 13.3.3 P(base date) with chain B de-risked %",
          (dist2[D(20)] * 100).quantize(D("0.001")), D("47.250"))
    check("WE 13.3.3 P(24 weeks) with chain B de-risked %",
          (dist2[D(24)] * 100).quantize(D("0.001")), D("37.000"))
    check("KA 13.3.5 weeks of margin to a 32-week long-stop",
          (D(32) - E).quantize(D("0.0001")), D("9.5925"))

    # ---------- engine 5a: the close-cost budget ----------
    close_items = [260000, 420000, 180000, 60000, 240000, 190000, 150000,   # diligence 1,500,000
                   504000, 40000,                                          # lender fees
                   380000, 189000,                                         # sponsor advisers
                   96000]                                                  # perfection
    TOT_CC = D(sum(close_items))
    check("WE 13.3.4 arrangement fee at 1.20 % of debt", DEBT * D("0.0120"), 504000)
    check("WE 13.3.4 success fee at 0.45 % of debt", DEBT * D("0.0045"), 189000)
    check("WE 13.3.4 total close costs", TOT_CC, 2709000)
    check("WE 13.3.4 close costs as % of debt raised",
          (TOT_CC / DEBT * 100).quantize(D("0.01")), D("6.45"))
    check("WE 13.3.4 close costs as % of the envelope",
          (TOT_CC / CAPEX * 100).quantize(D("0.001")), D("4.515"))
    LENDER_FEES = D(504000 + 40000)
    check("WE 13.3.4 fees payable to lenders", LENDER_FEES, 544000)
    check("WE 13.3.4 third-party and sponsor-side costs", TOT_CC - LENDER_FEES, 2165000)
    check("WE 13.3.4 lender fees as % of total close costs",
          (LENDER_FEES / TOT_CC * 100).quantize(D("0.01")), D("20.08"))
    PROVISION = D(840000) + D(1800000)      # Domain 6 KA 6.2.1 fee line + development line
    check("WE 13.3.4 the model's provision", PROVISION, 2640000)
    check("WE 13.3.4 shortfall against the provision", TOT_CC - PROVISION, 69000)
    check("WE 13.3.4 spare on the 2.0 % fee line", D(840000) - LENDER_FEES, 296000)
    check("WE 13.3.4 shortfall on the development-cost line",
          TOT_CC - LENDER_FEES - D(1800000), 365000)
    CONT_NEW = D(3645403) - D(69000)
    check("WE 13.3.4 contingency after absorbing the shortfall", CONT_NEW, 3576403)
    check("WE 13.3.4 contingency as % of the EPC price",
          (CONT_NEW / EPC * 100).quantize(D("0.01")), D("7.45"))
    K_PROP = D("0.0165")
    F_FIX = TOT_CC - K_PROP * DEBT
    check("WE 13.3.4 proportional component at 1.65 % of debt", K_PROP * DEBT, 693000)
    check("WE 13.3.4 fixed component", F_FIX, 2016000)
    check("WE 13.3.4 F/D + k reproduces the 6.45 % share",
          (F_FIX / DEBT + K_PROP).quantize(D("0.0001")) * 100, D("6.45"))
    check("WE 13.3.4 debt at which close costs reach 3.0 % of debt",
          (F_FIX / (D("0.030") - K_PROP)).quantize(D("0.01")), D("149333333.33"))
    check("WE 13.3.4 close-cost share at 150,000,000 of debt %",
          (F_FIX / D(150000000) * 100 + K_PROP * 100).quantize(D("0.001")), D("2.994"))
    check("WE 13.3.4 close-cost share at 420,000,000 of debt %",
          (F_FIX / D(420000000) * 100 + K_PROP * 100).quantize(D("0.001")), D("2.130"))
    r_lend = solve_af((DEBT - LENDER_FEES) / DS, TEN)
    check("WE 13.3.4 all-in cost on lender fees only %",
          (r_lend * 100).quantize(D("0.0001")), D("6.2386"))
    r_all = solve_af((DEBT - TOT_CC) / DS, TEN)
    check("WE 13.3.4 all-in cost on total close costs %",
          (r_all * 100).quantize(D("0.0001")), D("7.2376"))
    check("WE 13.3.4 all-in premium over the headline rate (bp)",
          ((r_all - RATE) * 10000).quantize(D("0.1")), D("123.8"))
    check("WE 13.3.4 close-day funds requirement, debt share",
          PROVISION * D("0.70"), 1848000)
    check("WE 13.3.4 close-day funds requirement, equity share",
          PROVISION * D("0.30"), 792000)

    # ---------- engine 5b: syndication, flex and the failed sell-down ----------
    FEE_TOT = DEBT * D("0.0120")
    PRAE = DEBT * D("0.0025")
    POOL_RATE = (FEE_TOT - PRAE) / DEBT
    HOLD, PART = D(15000000), D(13500000)
    check("WE 13.4.2 praecipium at 0.25 %", PRAE, 105000)
    check("WE 13.4.2 participation pool", FEE_TOT - PRAE, 399000)
    check("WE 13.4.2 pool rate %", (POOL_RATE * 100).quantize(D("0.01")), D("0.95"))
    ARR = PRAE + POOL_RATE * HOLD
    check("WE 13.4.2 arranger fee earned", ARR, 247500)
    check("WE 13.4.2 arranger fee yield %", (ARR / HOLD * 100).quantize(D("0.001")),
          D("1.650"))
    check("WE 13.4.2 participant fee earned", POOL_RATE * PART, 128250)
    check("WE 13.4.2 participant fee yield %",
          (POOL_RATE * PART / PART * 100).quantize(D("0.001")), D("0.950"))
    check("WE 13.4.2 fees reconcile to the total", ARR + 2 * POOL_RATE * PART, FEE_TOT)
    check("WE 13.4.2 arranger yield differential (bp)",
          ((ARR / HOLD - POOL_RATE) * 10000).quantize(D("0.1")), D("70.0"))
    check("MCQ 13.4-A distractor A: 1.20 % applied to the hold", HOLD * D("0.0120"), 180000)
    PLACED = D(20000000)
    RESID = DEBT - PLACED
    EXCESS = RESID - HOLD
    CHARGE = D("0.0090")
    CARRY = EXCESS * CHARGE * AF6_12
    check("WE 13.4.3 residual hold after a failed sell-down", RESID, 22000000)
    check("WE 13.4.3 excess over target hold", EXCESS, 7000000)
    check("WE 13.4.3 present value of the carry", CARRY.quantize(D("0.01")), D("528182.17"))
    check("WE 13.4.3 net outcome for the arranger", (ARR - CARRY).quantize(D("0.01")),
          D("-280682.17"))
    check("WE 13.4.3 excess hold that consumes the praecipium",
          (PRAE / (CHARGE * AF6_12)).quantize(D("1")), 1391565)
    check("WE 13.4.3 that excess as % of the facility",
          (PRAE / (CHARGE * AF6_12) / DEBT * 100).quantize(D("0.01")), D("3.31"))
    check("WE 13.4.3 excess hold that consumes the whole arranger fee",
          (ARR / (CHARGE * AF6_12)).quantize(D("1")), 3280118)
    check("MCQ 13.4-B distractor A: one year of carry, undiscounted", EXCESS * CHARGE, 63000)
    FLEX = D("0.0050")
    PV_FLEX = DEBT * FLEX * AF6_12
    check("WE 13.4.4 present value of 50 bp of flex", PV_FLEX.quantize(D("1")), 1760607)
    check("WE 13.4.4 flex as a multiple of the arrangement fee",
          (PV_FLEX / FEE_TOT).quantize(D("0.0001")), D("3.4933"))
    AF65 = af(D("0.065"), TEN)
    check("WE 13.4.4 AF(6.5 %, 12)", AF65.quantize(D("0.000001")), D("8.158725"))
    INST_FLEX = DEBT / AF65
    check("WE 13.4.4 flexed instalment", INST_FLEX.quantize(D("0.01")), D("5147862.98"))
    check("WE 13.4.4 instalment increase", (INST_FLEX - DS).quantize(D("1")), 138228)
    check("WE 13.4.4 flexed DSCR", (CF / INST_FLEX).quantize(D("0.0001")), D("1.2401"))
    DS_MAX = CF / D("1.20")
    check("WE 13.4.4 maximum instalment the 1.20x covenant allows", DS_MAX, 5320000)
    check("WE 13.4.4 AF required at that instalment",
          (DEBT / DS_MAX).quantize(D("0.000001")), D("7.894737"))
    R_STAR = solve_af(DEBT / DS_MAX, TEN)
    check("WE 13.4.4 rate at which the covenant binds %",
          (R_STAR * 100).quantize(D("0.0001")), D("7.1138"))
    check("WE 13.4.4 flex ceiling (bp)", ((R_STAR - RATE) * 10000).quantize(D("0.1")),
          D("111.4"))
    check("WE 13.4.4 share of the flex headroom consumed by 50 bp %",
          (FLEX / (R_STAR - RATE) * 100).quantize(D("0.1")), D("44.9"))
    check("KA 13.4.4 present value of 10 bp of flex",
          (DEBT * D("0.0010") * AF6_12).quantize(D("1")), 352121)
    check("MCQ 13.4-C distractor A: one year of flex", DEBT * FLEX, 210000)
    check("MCQ 13.4-C distractor C: twelve years undiscounted", DEBT * FLEX * 12, 2520000)

    # ---------- 13.A.1 liability caps ----------
    check("13.A.1 technical cap at 3x fees", D(260000) * 3, 780000)
    check("13.A.1 cap as % of the exposure it addresses",
          (D(780000) / D(6200000) * 100).quantize(D("0.01")), D("12.58"))
    check("13.A.1 all seven caps at 3x fees", D(1500000) * 3, 4500000)
    check("13.A.1 aggregate caps as % of aggregate expected exposure",
          (D(4500000) / sum_pc * 100).quantize(D("0.01")), D("47.67"))
    check("13.A.1 moving the technical cap from 3x to 5x", D(260000) * 5 - D(780000), 520000)

    # ---------- 13.A.2 own diligence against reliance ----------
    pA, CA, FA = D("0.65"), D(9000000), D(600000)
    own_res = pA * (D("0.90") * FA + D("0.10") * CA)
    rel_res = pA * (D("0.50") * FA + D("0.50") * CA)
    check("WE 13.A.2 own-diligence residual expected cost", own_res, 936000)
    check("WE 13.A.2 reliance residual expected cost", rel_res, 3120000)
    check("WE 13.A.2 detection lost by relying", rel_res - own_res, 2184000)
    check("WE 13.A.2 fees saved by relying", D(1500000) - D(240000), 1260000)
    check("WE 13.A.2 own-diligence advantage at zero cost of delay",
          (D(240000) + rel_res) - (D(1500000) + own_res), 924000)
    BE_COD = ((D(1500000) + own_res) - (D(240000) + rel_res)) / (3 - 12)
    check("WE 13.A.2 breakeven cost of delay per week", BE_COD.quantize(D("0.01")),
          D("102666.67"))
    own_tot = D(1500000) + 12 * COD_W + own_res
    rel_tot = D(240000) + 3 * COD_W + rel_res
    check("WE 13.A.2 own-diligence total at Kestrel's cost of delay",
          own_tot.quantize(D("0.01")), 3925600)
    check("WE 13.A.2 reliance total at Kestrel's cost of delay",
          rel_tot.quantize(D("0.01")), 3732400)
    check("WE 13.A.2 reliance advantage at Kestrel's cost of delay",
          (own_tot - rel_tot).quantize(D("0.01")), 193200)

    # ---------- Case study A: the resized facility ----------
    EQ_NEW = CAPEX - CAP130
    check("Case A equity after the resize", EQ_NEW.quantize(D("1")), 20505646)
    check("Case A gearing, debt share %", (CAP130 / CAPEX * 100).quantize(D("0.01")),
          D("65.82"))
    check("Case A gearing, equity share %", (EQ_NEW / CAPEX * 100).quantize(D("0.01")),
          D("34.18"))
    check("Case A debt-to-equity after the resize", (CAP130 / EQ_NEW).quantize(D("0.001")),
          D("1.926"))
    check("Case A instalment on the resized facility", (CAP130 / AF6_12).quantize(D("0.01")),
          D("4710769.23"))
    check("Case A DSCR on the resized facility",
          (CF_CORR / (CAP130 / AF6_12)).quantize(D("0.0001")), D("1.3000"))
    check("Case A proportional close-cost saving on the resize",
          ((DEBT - CAP130) * K_PROP).quantize(D("1")), 41343)
    CC_NEW = F_FIX + K_PROP * CAP130
    check("Case A close costs on the resized facility", CC_NEW.quantize(D("1")), 2667657)
    check("Case A close costs as % of the resized debt",
          (CC_NEW / CAP130 * 100).quantize(D("0.0001")), D("6.7545"))
    check("Case A close costs as % of the original debt",
          (TOT_CC / DEBT * 100).quantize(D("0.0001")), D("6.4500"))

    # ---------- Case study B: the two waivers ----------
    FAC_B = D(180000000)
    W_FEE = FAC_B * D("0.0025")
    STEP = FAC_B * D("0.0025") * D(120) / D(360)
    check("Case B waiver fee at 0.25 %", W_FEE, 450000)
    check("Case B 25 bp step-up over 120 days (30/360)", STEP, 150000)
    check("Case B total cost of the CP-14 waiver", W_FEE + STEP, 600000)
    check("Case B counterfactual: 210 days at 240,000 a day", D(240000) * 210, 50400000)
    check("Case B value of waiving", D(240000) * 210 - (W_FEE + STEP), 49800000)
    check("Case B benefit-to-cost ratio of the waiver",
          ((D(240000) * 210 - (W_FEE + STEP)) / (W_FEE + STEP)).quantize(D("0.01")),
          D("83.00"))
    check("Case B CP-22 cure as a multiple of the CP-14 waiver cost",
          (D(3400000) / (W_FEE + STEP)).quantize(D("0.001")), D("5.667"))

    # ---------- Exercise 13.1 ----------
    fe, pe, Ce, Fe, wke, code = D(320000), D("0.25"), D(7400000), D(380000), 7, D(96000)
    avoide = pe * (Ce - Fe)
    check("EX 13.1 expected avoidable loss", avoide, 1755000)
    check("EX 13.1 d* inside the envelope %", (fe / avoide * 100).quantize(D("0.01")),
          D("18.23"))
    check("EX 13.1 priced delay on the critical path", wke * code, 672000)
    check("EX 13.1 d* on the critical path %",
          ((fe + wke * code) / avoide * 100).quantize(D("0.01")), D("56.52"))
    check("EX 13.1 expected cost without the stream", pe * Ce, 1850000)
    withe = fe + pe * (D("0.75") * Fe + D("0.25") * Ce)
    check("EX 13.1 expected cost with the stream, in the envelope", withe, 853750)
    check("EX 13.1 net value in the envelope", pe * Ce - withe, 996250)
    check("EX 13.1 net value on the critical path", pe * Ce - withe - wke * code, 324250)
    check("EX 13.1 common error: fee / (p*C) %",
          (fe / (pe * Ce) * 100).quantize(D("0.01")), D("17.30"))

    # ---------- Exercise 13.2 ----------
    ECH = [(16, D("0.30"), 4), (14, D("0.40"), 5), (11, D("0.25"), 6)]
    b2 = max(c[0] for c in ECH)
    check("EX 13.2 base close (weeks)", b2, 16)
    E3, dist3 = conjunction(ECH)
    check("EX 13.2 E[close] weeks", E3.quantize(D("0.0001")), D("18.1450"))
    check("EX 13.2 max of the expectations",
          max(D(b) + p * s for b, p, s in ECH).quantize(D("0.001")), D("17.200"))
    for wk, want in ((16, "31.50"), (17, "10.50"), (19, "28.00"), (20, "30.00")):
        check(f"EX 13.2 P(close at {wk} weeks) %",
              (dist3[D(wk)] * 100).quantize(D("0.01")), D(want))
    check("EX 13.2 expected slip (weeks)", (E3 - b2).quantize(D("0.0001")), D("2.1450"))
    check("EX 13.2 expected slip cost", (E3 - b2) * D(80000), 171600)
    check("EX 13.2 chain A's own expected slip cost", D("1.2") * D(80000), 96000)
    check("EX 13.2 conjunction premium", (E3 - b2) * D(80000) - D("1.2") * D(80000), 75600)
    check("EX 13.2 understatement from using the max of expectations (weeks)",
          (E3 - D("17.200")).quantize(D("0.001")), D("0.945"))

    # ---------- Exercise 13.3 ----------
    F3, k3, D3 = D(1650000), D("0.0185"), D(55000000)
    check("EX 13.3 total close costs", F3 + k3 * D3, 2667500)
    check("EX 13.3 share of debt raised %",
          ((F3 + k3 * D3) / D3 * 100).quantize(D("0.001")), D("4.850"))
    check("EX 13.3 debt at which close costs reach 2.5 %",
          (F3 / (D("0.025") - k3)).quantize(D("0.01")), D("253846153.85"))

    # ---------- Exercise 13.4 ----------
    AF14 = af(D("0.065"), 14)
    check("EX 13.4 AF(6.5 %, 14)", AF14.quantize(D("0.000001")), D("9.013842"))
    INST4 = D(36000000) / AF14
    check("EX 13.4 instalment", INST4.quantize(D("0.01")), D("3993857.30"))
    check("EX 13.4 reported DSCR", (D(5600000) / INST4).quantize(D("0.0001")), D("1.4022"))
    check("EX 13.4 corrected CFADS", D(5600000) - D(320000), 5280000)
    check("EX 13.4 corrected DSCR", (D(5280000) / INST4).quantize(D("0.0001")), D("1.3220"))
    CAP4 = D(5280000) / D("1.25") * AF14
    check("EX 13.4 debt capacity at 1.25x", CAP4.quantize(D("0.01")), D("38074470.00"))
    check("EX 13.4 headroom above the drawn facility",
          (CAP4 - D(36000000)).quantize(D("0.01")), D("2074470.00"))

    # ---------- Exercise 13.5 ----------
    FAC5 = D(96000000)
    FEE5 = FAC5 * D("0.0135")
    PRAE5 = FAC5 * D("0.0030")
    POOL5 = (FEE5 - PRAE5) / FAC5
    check("EX 13.5 total arrangement fee", FEE5, 1296000)
    check("EX 13.5 praecipium", PRAE5, 288000)
    check("EX 13.5 participation pool", FEE5 - PRAE5, 1008000)
    check("EX 13.5 pool rate %", (POOL5 * 100).quantize(D("0.0001")), D("1.0500"))
    ARR5 = PRAE5 + POOL5 * D(30000000)
    check("EX 13.5 arranger fee earned", ARR5, 603000)
    check("EX 13.5 arranger fee yield %",
          (ARR5 / D(30000000) * 100).quantize(D("0.001")), D("2.010"))
    check("EX 13.5 participant fee earned", POOL5 * D(22000000), 231000)
    check("EX 13.5 participant fee yield %",
          (POOL5 * 100).quantize(D("0.001")), D("1.050"))
    check("EX 13.5 fees reconcile", ARR5 + 3 * POOL5 * D(22000000), FEE5)
    EXC5 = D(42000000) - D(30000000)
    check("EX 13.5 excess over target hold", EXC5, 12000000)
    CARRY5 = EXC5 * D("0.0085") * AF14
    check("EX 13.5 carry on the excess hold", CARRY5.quantize(D("0.01")), D("919411.92"))
    check("EX 13.5 net outcome", (ARR5 - CARRY5).quantize(D("0.01")), D("-316411.92"))
    check("EX 13.5 excess hold that consumes the praecipium",
          (PRAE5 / (D("0.0085") * AF14)).quantize(D("0.01")), D("3758924.52"))
    check("EX 13.5 that excess as % of the facility",
          (PRAE5 / (D("0.0085") * AF14) / FAC5 * 100).quantize(D("0.01")), D("3.92"))
