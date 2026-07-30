"""PFL-AI Domain 6 — golden checks for MCQ 6.1-F, 6.2-F, 6.2-G, 6.3-G, 6.4-G and 6.4-H.

Scope: the Evaluation and Comprehension items added to KA 6.1–6.4. Every number printed in those
stems, options and rationales is recomputed here from first principles; master-thread values are read
from ctx, never re-typed. `pfl_d06.py` owns the domain's worked examples and the items that preceded
these, so the two modules never edit one file.

Engines used:

  1. Convention pricing (6.1.2, 6.2.1) — the four IDC variants are taken as the manuscript's computed
     results and the DIFFERENCES between them are re-derived here, together with the propagation into
     the depreciation base (867,245 / 25) and the present value of the tax it shelters at 8 %.
  2. The first operating year (6.2.2) — CFADS less debt service against CFADS less debt service less
     reserve funding, the DSRA being funded in two equal instalments; the distribution as a share of
     equity contributed.
  3. Cash tax (6.2.3) — 20 % of (EBITDA - accounting depreciation - interest) on the base model's
     stated simplification.
  4. Returns (6.3.3) — money multiple = distributions / equity contributed; the case difference in
     equity IRR points.
  5. Review economics (6.4.3, 6.4.4) — one expected-cost function
     E(fee, elapsed, p, d, correction) = fee + elapsed + p[d x correction + (1-d) x C], from which the
     four controls, the breakeven error rate and the ranking all follow.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))
    CAPEX = ctx["KESTREL_CAPEX"]               # 60,000,000
    EQUITY = ctx["KESTREL_EQUITY"]             # 18,000,000
    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    DS = ctx["KESTREL_INSTALMENT"]             # 5,009,635.23
    EBITDA = ctx["KESTREL_EBITDA"]             # 7,500,000
    INT1 = ctx["KESTREL_INTEREST_Y1"]          # 2,520,000
    LIFE = ctx["KESTREL_LIFE"]                 # 25
    R8 = ctx["KESTREL_DISCOUNT"]               # 0.08
    TAXR = D("0.20")
    DEP = CAPEX / LIFE

    # ================= MCQ 6.1-F — the conventions an assistant chose =================
    IDC_QO, IDC_QA = D(2114597), D(2427554)    # quarterly, opening and average balance
    IDC_AO, IDC_AA = D(1247352), D(2481619)    # annual, opening and average balance
    under = IDC_QO - IDC_AO
    check("MCQ 6.1-F annual-timeline understatement", under, 867245)
    check("MCQ 6.1-F understatement %", q(under / IDC_QO * 100, 2), D("41.01"))
    check("MCQ 6.1-F annual/average lands within this of the quarterly answer",
          IDC_AA - IDC_QO, 367022)
    check("MCQ 6.1-F invariant: the 41 % error needs BOTH wrong choices",
          1 if abs(IDC_AA - IDC_QO) < under and abs(IDC_QA - IDC_QO) < under else 0, 1)
    check("MCQ 6.1-F quarterly IDC as % of the envelope",
          q(IDC_QO / CAPEX * 100, 2), D("3.52"))
    dep_lost = under / LIFE
    check("MCQ 6.1-F annual depreciation understated", q(dep_lost, 0), 34690)
    check("MCQ 6.1-F PV of the tax that depreciation shelters",
          q(dep_lost * TAXR * af(R8, LIFE), 0), 74061)

    # ================= MCQ 6.2-F — distributions struck before reserve funding =========
    DSRA = D("2504817.62")                     # six months of debt service
    fund = DSRA / 2
    check("MCQ 6.2-F six-month DSRA", q(DSRA, 0), 2504818)
    check("MCQ 6.2-F annual reserve funding, two equal instalments", q(fund, 0), 1252409)
    naive = CF - DS
    modelled = CF - DS - fund
    check("MCQ 6.2-F distribution before reserve funding", q(naive, 0), 1374365)
    check("MCQ 6.2-F distribution through the waterfall", q(modelled, 0), 121956)
    check("MCQ 6.2-F distribution as % of equity contributed",
          q(modelled / EQUITY * 100, 2), D("0.68"))
    check("MCQ 6.2-F invariant: the naive figure overstates by an order of magnitude",
          1 if naive / modelled > 10 else 0, 1)
    check("MCQ 6.2-F invariant: the 1.15x lock-up is not engaged at this coverage",
          1 if CF / DS > D("1.15") else 0, 1)

    # ================= MCQ 6.2-G — the three depreciation objects =================
    check("MCQ 6.2-G accounting depreciation, straight line over the life", DEP, 2400000)
    check("MCQ 6.2-G year-one taxable profit", EBITDA - DEP - INT1, 2580000)
    check("MCQ 6.2-G year-one cash tax", (EBITDA - DEP - INT1) * TAXR, 516000)

    # ================= MCQ 6.3-G — the multiple and the case =================
    DIST_BANK, DIST_SPON = D(90507502), D(151536729)
    check("MCQ 6.3-G money multiple, bank case", q(DIST_BANK / EQUITY, 3), D("5.028"))
    # The board paper pairs the BANK case's multiple with the SPONSOR case's IRR. The item now names
    # that, so the sponsor case's own multiple has to be printed and therefore checked.
    check("MCQ 6.3-G money multiple, sponsor case", q(DIST_SPON / EQUITY, 3), D("8.419"))
    check("MCQ 6.3-G case difference in equity IRR points",
          D("13.52") - D("9.83"), D("3.69"))
    check("MCQ 6.3-G the escalation assumption behind it %", D("0.02967") * 100, D("2.967"))
    check("MCQ 6.3-G invariant: the sponsor case returns the larger multiple",
          1 if DIST_SPON > DIST_BANK else 0, 1)

    # ================= MCQ 6.4-G — pre-check against audit =================
    C, P, FEE, ELAPSED = D(2691071), D("0.35"), D(180000), D(248267)
    D_AUD, CORR_LATE, CORR_EARLY = D("0.85"), D(184133), D(60000)
    SCAN_FEE, D_SCAN = D(8000), D("0.55")

    def expected(fee, elapsed, p, d, corr):
        return fee + elapsed + p * (d * corr + (1 - d) * C)

    nothing = P * C
    late = expected(FEE, ELAPSED, P, D_AUD, CORR_LATE)
    early = expected(FEE, D(0), P, D_AUD, CORR_EARLY)
    pre = expected(SCAN_FEE, D(0), P, D_SCAN, CORR_EARLY)
    both = (SCAN_FEE + P * D_SCAN * CORR_EARLY
            + expected(FEE, D(0), P * (1 - D_SCAN), D_AUD, CORR_EARLY))
    check("MCQ 6.4-G expected cost with no control", q(nothing, 0), 941875)
    check("MCQ 6.4-G net value, late audit", q(nothing - late, 0), 317547)
    check("MCQ 6.4-G net value, AI pre-check alone", q(nothing - pre, 0), 498481)
    check("MCQ 6.4-G net value, early audit", q(nothing - early, 0), 602744)
    check("MCQ 6.4-G net value, pre-check plus early audit", q(nothing - both, 0), 670716)
    check("MCQ 6.4-G breakeven error rate, late audit %",
          q((FEE + ELAPSED) / (D_AUD * (C - CORR_LATE)) * 100, 2), D("20.10"))
    check("MCQ 6.4-G invariant: the pre-check alone outranks the LATE audit",
          1 if (nothing - pre) > (nothing - late) else 0, 1)
    check("MCQ 6.4-G invariant: pre-check plus early audit is the best of the four",
          1 if both == min(late, early, pre, both) else 0, 1)
