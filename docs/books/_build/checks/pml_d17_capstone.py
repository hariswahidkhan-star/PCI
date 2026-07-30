"""PML-AI Appendix G — integrated capstones. Golden-answer checks.

The capstone asserts that the sixteen domains' Meridian results agree, and its own arithmetic is the
arithmetic of putting them side by side. Those cross-domain claims are the ones most likely to be
wrong, because no single chapter's author owns them, so each is recomputed here from the domain
results it combines — including the two conjunction claims, which are the capstone's spine.

Attributed to domain 17 so that Appendix C reports the capstone's record separately from the sixteen
domains' rather than inflating one of them.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]

    COST = D(2400000)               # D1 approved programme cost
    COD = D(14280)                  # D1, D3 cost of delay per week
    BEN_MAX = D(979200)             # D1 theoretical maximum annual benefit
    ADOPT_PLAN, ADOPT_ACTUAL = D("0.70"), D("0.40")
    AF8 = D("5.971299")             # D2 eight-year annuity factor at 7 %

    # ---- G.1.1 the two verdicts ----------------------------------------------------------
    check("G.1.1 planned benefit at 70 % adoption", BEN_MAX * ADOPT_PLAN, D(685440))
    check("G.1.1 measured benefit at 40 % adoption", BEN_MAX * ADOPT_ACTUAL, D(391680))
    SHORTFALL = BEN_MAX * (ADOPT_PLAN - ADOPT_ACTUAL)
    check("G.1.1 annual benefit shortfall", SHORTFALL, D(293760))
    check("G.1.1 shortfall two ways agree", SHORTFALL,
          BEN_MAX * ADOPT_PLAN - BEN_MAX * ADOPT_ACTUAL)
    UNDERSPEND = COST * D("0.03")
    check("G.1.1 capital underspend", UNDERSPEND, D(72000))
    check("G.1.1 first-year ratio", SHORTFALL / UNDERSPEND, D("4.08"), tol=D("0.005"))
    check("G.1.1 undiscounted eight-year shortfall", SHORTFALL * 8, D(2350080))
    check("G.1.1 undiscounted shortfall as share of programme cost (%)",
          SHORTFALL * 8 / COST * 100, D("97.92"), tol=D("0.005"))
    check("G.1.1 discounted shortfall at 7 %", SHORTFALL * AF8, D("1754128.79"))
    check("G.1.1 discounted shortfall as share of programme cost (%)",
          SHORTFALL * AF8 / COST * 100, D("73.09"), tol=D("0.005"))
    # the annuity factor itself, so the capstone does not depend on a transcribed constant
    check("G.1.1 AF(0.07,8)", (1 - D("1.07") ** -8) / D("0.07"), AF8, tol=D("0.0000005"))
    # the two verdicts are a stock and a flow: the ratio is a scale comparison, not a net
    check("G.1.1 INVARIANT the flow exceeds the stock in year one alone",
          D(1) if SHORTFALL > UNDERSPEND else D(0), D(1))

    # ---- G.1.2 the conjunction error, twice ----------------------------------------------
    # D16 readiness: seven conditions averaging 90.71 % give a product of 49.79 %
    # Domain 16's own seven conditions, not a set reconstructed to fit the summary figures:
    # training 0.96, data migration 0.90, interfaces 0.94, network and devices 0.98,
    # workflow redesign 0.85, clinical champion 0.80, rehearsed fallback 0.92.
    READY = [D("0.96"), D("0.90"), D("0.94"), D("0.98"), D("0.85"), D("0.80"), D("0.92")]
    avg = sum(READY) / 7
    prod = D(1)
    for c in READY:
        prod *= c
    check("G.1.2 readiness conditions sum", sum(READY), D("6.35"))
    check("G.1.2 readiness conditions, mean (%)", avg * 100, D("90.71"), tol=D("0.005"))
    check("G.1.2 readiness conditions, product (%)", prod * 100, D("49.79"), tol=D("0.005"))
    check("G.1.2 reported-versus-actual gap (points)", D("90.71") - D("49.79"), D("40.92"),
          tol=D("0.005"))
    check("G.1.2 INVARIANT the mean overstates the conjunction",
          D(1) if avg > prod else D(0), D(1))
    check("G.1.2 all seven conditions at 0.98 still leaves failures (%)",
          100 - D("0.98") ** 7 * 100, D("13.19"), tol=D("0.005"))
    # the remediation ranking: p'/p favours the worst condition by more than tenfold
    # Both conditions are remediated to the SAME target, 0.98, which is what makes the comparison
    # fair. Lifting the champion to 0.95 instead gives 9.34 points -- a different question.
    check("G.1.2 uplift from lifting the clinical champion 0.80 -> 0.98 (points)",
          (prod / D("0.80") * D("0.98") - prod) * 100, D("11.20"), tol=D("0.005"))
    check("G.1.2 uplift from lifting training 0.96 -> 0.98 (points)",
          (prod / D("0.96") * D("0.98") - prod) * 100, D("1.04"), tol=D("0.005"))
    check("G.1.2 remediation ratio", D("11.20") / D("1.04"), D("10.7692"), tol=D("0.00005"))
    # D15 commitments: one region at 52.95 %, four on one date at 7.86 %
    REGION = D("0.5295")
    check("G.1.2 four regions on one date (%)", REGION ** 4 * 100, D("7.86"), tol=D("0.005"))
    check("G.1.2 probability the four-region date is missed (%)", 100 - REGION ** 4 * 100,
          D("92.14"), tol=D("0.005"))
    check("G.1.2 a six-predecessor floor at 0.85 each (%)", D("0.85") ** 6 * 100, D("37.7150"),
          tol=D("0.00005"))
    check("G.1.2 INVARIANT Region A's own probability exceeds the four-region commitment",
          D(1) if REGION > REGION ** 4 else D(0), D(1))
    check("G.1.2 components reporting green", D(4) * D(6), D(24))

    # ---- G.1.3 the breakevens ------------------------------------------------------------
    OBJECTION, PRECONSULT = D(224520), D(3300)
    check("G.1.3 nine weeks of critical path at the cost of delay", D(9) * COD, D(128520))
    check("G.1.3 residue is re-verification and rework", OBJECTION - D(9) * COD, D(96000))
    check("G.1.3 engagement breakeven probability (%)", PRECONSULT / OBJECTION * 100,
          D("1.4698"), tol=D("0.00005"))
    check("G.1.3 prevention-to-consequence ratio", OBJECTION / PRECONSULT, D("68.04"),
          tol=D("0.005"))
    check("G.1.3 governance latency's share of the nine weeks (%)", D(4) / D(9) * 100,
          D("44.44"), tol=D("0.005"))
    check("G.1.3 governance latency priced", D(4) * COD, D(57120))
    # D13 sequencing
    check("G.1.3 sequencing swing, worst order to delay-cost density", D(386920) - D(231880),
          D(155040))
    check("G.1.3 sequencing swing in weeks of delay cost", (D(386920) - D(231880)) / COD,
          D("10.8571"), tol=D("0.00005"))
    check("G.1.3 density beats ranking by cost of delay alone", D(276080) - D(231880), D(44200))
    # D16's hold decision, in the same units as the objection's rework line. Written as a comparison
    # of the DERIVED residue against D16's figure — `check(..., D(96000), D(96000))` stood here and
    # compared a constant to itself, which can never fail and therefore establishes nothing.
    HOLD_COST = D(96000)                    # D16: three weeks held, 96,000 spent
    check("G.1.3 the objection's rework residue equals D16's hold cost",
          OBJECTION - D(9) * COD, HOLD_COST)
    check("G.1.3 three weeks held, priced at the cost of delay", D(3) * COD, D(42840))

    # ---- G.1.4 the benefit was decided earlier -------------------------------------------
    check("G.1.4 reallocation distance in Meridians", D(4200000) / COST, D("1.75"),
          tol=D("0.005"))
    check("G.1.4 alignment gap between mapped and total spend (points)",
          D("76.6667") - D("59.0000"), D("17.6667"), tol=D("0.00005"))
    check("G.1.4 Case study B: requirements addressing the external user (%)",
          D(47) / D(612) * 100, D("7.6797"), tol=D("0.00005"))
    check("G.1.4 Case study B: correction payback (months)", D(285000) / D(425500) * 12,
          D("8.04"), tol=D("0.005"))
    check("G.1.4 Case study B: annual return per dollar of correction",
          D(425500) / D(285000), D("1.4930"), tol=D("0.00005"))

    # ---- G.1.5 what could be seen --------------------------------------------------------
    check("G.1.5 data estate weighted mean defect rate (%)", D(332) / D(14100) * 100,
          D("2.3546"), tol=D("0.00005"))
    check("G.1.5 exposure per defective record", D(172820) / D(332), D("520.54"), tol=D("0.005"))
    check("G.1.5 exposure per record across the estate", D(172820) / D(14100), D("12.2567"),
          tol=D("0.00005"))
    check("G.1.5 INVARIANT the low-rate class exceeds the estate average exposure per record",
          D(1) if D("28.00") > D(172820) / D(14100) else D(0), D(1))
    check("G.1.5 risk-register coverage implies unidentified risks",
          D("11.43") / (D("11.43") / (1 - D("0.7861"))) * 100, D("21.39"), tol=D("0.005"))
    check("G.1.5 leader's recurring commitments as share of the week (%)",
          D("37.0") / D("45.0") * 100, D("82.2"), tol=D("0.05"))
    check("G.1.5 discretionary attention (hours)", D("45.0") - D("37.0"), D("8.0"))
    check("G.1.5 discretionary attention as share of the week (%)",
          (D("45.0") - D("37.0")) / D("45.0") * 100, D("17.8"), tol=D("0.05"))
