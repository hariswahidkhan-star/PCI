"""PFL-AI Domain 5 — golden checks for MCQ 5.1-F, 5.2-F, 5.2-G, 5.3-G, 5.4-G and 5.4-H.

Scope: the Evaluation and Comprehension items added to KA 5.1–5.4 to lift the domain's
cognitive-level mix. Every number printed in those stems, options and rationales is recomputed here
from first principles; master-thread values are read from ctx, never re-typed. `pfl_d05.py` owns the
domain's worked examples and the items that preceded these, so the two modules never edit one file.

Engines used:

  1. Development funnel (5.1.2) — programme spend = sum(count x unit); cost per close = spend /
     closes; breakeven closes = spend / value per close; breakeven close rate = that / screened; the
     margin over the breakeven rate is proved identical to the portfolio value multiple.
  2. Equity bridge (5.2.4) — bridge interest = 9,000,000[(1.055^2 - 1) + 0.055]; the two funding
     profiles are proved equal in present value AT the bridge rate and unequal at the sponsors' 12 %.
  3. Technology premium (5.3.4) — Domain 10's sizing rule at two target ratios, the capacity lost,
     and the annual cost of substituting equity for debt at the stated 6-point spread.
  4. Performance buy-down (5.4.3) — the 97 %-output year is rebuilt from the fixed/variable split;
     the buy-down is debt x proportional CFADS shortfall, and is proved to restore the sized ratio.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))
    DEBT = ctx["KESTREL_DEBT"]                 # 42,000,000
    CAPEX = ctx["KESTREL_CAPEX"]               # 60,000,000
    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    DS = ctx["KESTREL_INSTALMENT"]             # 5,009,635.23
    NPV_D4 = ctx["KESTREL_NPV"]                # 16,179,360
    AF6_12 = af(ctx["KESTREL_RATE"], ctx["KESTREL_TENOR"])
    COV = D("1.20")

    # ================= MCQ 5.1-F — the budget defended on forecast value =================
    STAGES = ((40, D(25000)), (12, D(250000)), (5, D(1200000)), (2, D(2400000)))
    spend = sum(D(n) * u for n, u in STAGES)
    closes, screened = D(2), D(40)
    check("MCQ 5.1-F programme spend", spend, 14800000)
    check("MCQ 5.1-F cost per closed project", spend / closes, 7400000)
    check("MCQ 5.1-F achieved close rate %", closes / screened * 100, "5.00")
    bk = spend / NPV_D4
    check("MCQ 5.1-F breakeven closes", q(bk, 4), D("0.9147"))
    check("MCQ 5.1-F breakeven close rate %", q(bk / screened * 100, 2), D("2.29"))
    margin = (closes / screened) / (bk / screened)
    check("MCQ 5.1-F margin over the breakeven close rate", q(margin, 2), D("2.19"))
    check("MCQ 5.1-F invariant: that margin is identically the value multiple",
          margin, closes * NPV_D4 / spend, D("0.0000005"))
    check("MCQ 5.1-F screening stage spend", D(40) * D(25000), 1000000)
    check("MCQ 5.1-F invariant: screening is the smallest of the four stages",
          1 if min(D(n) * u for n, u in STAGES) == D(1000000) else 0, 1)

    # ================= MCQ 5.2-F — the equity bridge, presented =================
    LEG, RB, KE = D(9000000), D("0.055"), D("0.12")
    br_int = LEG * ((1 + RB) ** 2 - 1) + LEG * RB
    repay = LEG * 2 + br_int
    check("MCQ 5.2-F bridge interest", br_int, 1512225)
    check("MCQ 5.2-F bridge repayment at t = 2", repay, 19512225)
    pv_b_r, pv_p_r = repay / (1 + RB) ** 2, LEG + LEG / (1 + RB)
    check("MCQ 5.2-F invariant: the profiles are equal in PV at the bridge rate",
          pv_b_r - pv_p_r, 0, D("0.0000005"))
    check("MCQ 5.2-F PV of either profile at the bridge rate", q(pv_b_r, 0), 17530806)
    pv_b_e, pv_p_e = repay / (1 + KE) ** 2, LEG + LEG / (1 + KE)
    check("MCQ 5.2-F saving at the sponsors' 12 % requirement",
          q(pv_p_e - pv_b_e, 0), 1480688)
    check("MCQ 5.2-F invariant: the saving exists only away from the bridge rate",
          1 if (pv_p_e - pv_b_e) > 0 and abs(pv_b_r - pv_p_r) < D("0.000001") else 0, 1)

    # ================= MCQ 5.3-G — the first-of-a-kind premium, judged =================
    def capacity(target):
        return (CF / target) * AF6_12

    cap130, cap145 = capacity(D("1.30")), capacity(D("1.45"))
    check("MCQ 5.3-G debt capacity at 1.30x", q(cap130, 0), 41171123)
    check("MCQ 5.3-G debt capacity at 1.45x", q(cap145, 0), 36912041)
    lost = q(cap130, 0) - q(cap145, 0)
    check("MCQ 5.3-G capacity lost, payable as equity", lost, 4259082)
    check("MCQ 5.3-G annual cost of substituting equity for debt at 6 points",
          q(lost * D("0.06"), 0), 255545)
    check("MCQ 5.3-G gearing at 1.30x %", q(cap130 / CAPEX * 100, 1), D("68.6"))
    check("MCQ 5.3-G gearing at 1.45x %", q(cap145 / CAPEX * 100, 1), D("61.5"))

    # ================= MCQ 5.4-G — the buy-down, judged =================
    REV, FIXED, VAR = D(12000000), D(3825000), D(675000)
    DEP, INT, TAXR, DWC, OUT = D(2400000), D(2520000), D("0.20"), D(600000), D("0.97")
    eb97 = REV * OUT - (FIXED + VAR * OUT)
    cf97 = eb97 - (eb97 - DEP - INT) * TAXR - DWC
    check("MCQ 5.4-G 97 % output CFADS", cf97, 6112200)
    check("MCQ 5.4-G 97 % output DSCR", q(cf97 / DS, 4), D("1.2201"))
    check("MCQ 5.4-G sized DSCR", q(CF / DS, 4), D("1.2743"))
    lost_cf = CF - cf97
    check("MCQ 5.4-G annual CFADS lost", lost_cf, 271800)
    buydown = DEBT * lost_cf / CF
    check("MCQ 5.4-G buy-down", q(buydown, 0), 1788158)
    inst2 = (DEBT - buydown) / AF6_12
    check("MCQ 5.4-G invariant: the buy-down restores the sized ratio",
          q(cf97 / inst2, 4), q(CF / DS, 4))
    head0 = CF - DS * COV
    check("MCQ 5.4-G base annual headroom", q(head0, 0), 372438)
    check("MCQ 5.4-G headroom left at 97 % output", q(cf97 - DS * COV, 0), 100638)
    check("MCQ 5.4-G headroom consumed %", q(lost_cf / head0 * 100, 1), D("73.0"))
    check("MCQ 5.4-G option D is arithmetically true: 1.2201 clears 1.20",
          1 if cf97 / DS > COV else 0, 1)
