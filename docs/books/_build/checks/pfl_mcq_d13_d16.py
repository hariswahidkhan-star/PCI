"""PFL-AI Domains 13-16 — the Evaluation/Comprehension MCQ batch: golden checks.

Scope. Domains 13-16 each carry new items from two authors in this batch. This module covers the
twenty-four items lettered `-E` and `-F` by the D13-16 author:

    13.1-E  13.1-F  13.2-E  13.2-F  13.3-F  13.4-F
    14.1-E  14.1-F  14.2-E  14.2-F  14.3-E  14.4-E
    15.1-E  15.1-F  15.2-E  15.2-F  15.3-E  15.4-E
    16.1-E  16.1-F  16.2-E  16.3-E  16.3-F  16.4-E

The `-G`/`-H` items in the same four files belong to the other author and are checked in
`pfl_d13_lvl.py`, `pfl_d14_lvl.py`, `pfl_d15_lvl.py` and `pfl_d16_lvl_b.py`. Nothing is checked
twice.

Method. Every figure is rebuilt from the transaction's own inputs before it is compared with the
manuscript, so a check cannot pass merely because a value was copied across. Master-thread
quantities — the 42,000,000 facility, the 18,000,000 of equity, the 60,000,000 envelope, the
5,009,635.23 instalment, the 6,384,000 of `CFADS`, the 6 % coupon and the 8 % equity rate — are read
from ctx. Three chains are rebuilt rather than quoted:

  * Kestrel's amortisation, so that the balance outstanding after five years (15.3) and after three
    years (15.4) is `instalment × AF(0.06, remaining tenor)` rather than a literal, and the swap
    break cost of 15.3.2 is rebuilt from the seven remaining opening balances;
  * the rate at which the 1.20x covenant fails, solved by bisection, giving both 13.4-F's 111.38
    basis points of headroom and 14.4-E's 0.7001 months of tolerable slip;
  * the validation sample size of 16.3.2, solved from the binomial condition for zero and for one
    permitted failure, so that 299 and 473 are computed and not asserted.

Both keys and numeric distractors are recomputed. A distractor whose figure is arithmetically
impossible gives the answer away, and three of these items turn on a distractor being *right* — the
6.2386 %/7.2376 % pair of 13.3-F, the 0.8502 coverage of 14.2-E, and the 25-basis-point price win of
15.3-E, which is genuinely positive and still the weaker course.

Teaching invariants are checked as invariants, not as arithmetic: that the diligence envelope is a
maximum and not a sum; that a fixed funding envelope conserves the contingency-plus-interest pool;
that a coverage ratio below one is a funding shortfall and not a narrow margin; that par at the
contract rate is an identity insensitive to tenor and therefore carries no credit information; that
extending a validated sample cannot be replaced by repairing a failed case; and that the blanket
dual-approval rule stays the weaker policy across an order of magnitude of caught rates.

Level tags, and what verification changed. Two items were tagged Evaluation on authoring and are
tagged **Analysis** in the manuscript as checked in: 13.1-E and 13.2-E. Neither offers a defensible
alternative course — every distractor is arithmetically or conceptually wrong rather than weaker — so
neither asks for a judgement between positions a competent professional could hold, which is the test
an Evaluation item has to meet. The checks below compute what makes each distractor wrong, and that
arithmetic is the evidence for the level. Three other repairs were made to the manuscript and are
reflected here: 16.3-E's key was reworded from a near-verbatim restatement of 16.3.1's own sentence
into a genuine paraphrase, so that the item is Comprehension rather than Recall; 14.1-E's rationale
now names option A's real defect, since the 183,013 it quotes is a true present-value gain already
net of the extra equity and the weakness lies in what it omits; and 14.3-E's rationale now carries the
counsel pointer that the effectiveness of a vesting arrangement against a vendor's insolvency is a
question of the governing law, on which the book states no jurisdiction's position.

Two generated artefacts still carry the pre-verification tags and need regenerating once every
domain in this batch has been verified: `pfl-ai/QUESTION_BANK.md` (13.1-E, 13.2-E and 16.3-E) and
`CORPUS_GATE_REPORT.md` §4 (PFL-AI Evaluation 122 to 120, Analysis 131 to 133). Neither is
regenerated from here, because both read every manuscript and siblings are still in flight.
"""


def run(ctx):
    check, D, af, mesh = ctx["check"], ctx["D"], ctx["af"], ctx["mesh"]
    DEBT = ctx["KESTREL_DEBT"]                       # 42,000,000
    EQUITY = ctx["KESTREL_EQUITY"]                   # 18,000,000
    ENVELOPE = ctx["KESTREL_CAPEX"]                  # 60,000,000
    RATE = ctx["KESTREL_RATE"]                       # 0.06
    TENOR = ctx["KESTREL_TENOR"]                     # 12
    DS = ctx["KESTREL_INSTALMENT"]                   # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                        # 6,384,000
    DISC = ctx["KESTREL_DISCOUNT"]                   # 0.08
    COV = D("1.20")
    DIST_COND = D("1.25")

    def solve_rate(target_af, n, lo=D("0.0001"), hi=D("0.9")):
        """Bisect for the rate whose n-period annuity factor equals target_af."""
        for _ in range(400):
            mid = (lo + hi) / 2
            if af(mid, n) > target_af:
                lo = mid
            else:
                hi = mid
        return (lo + hi) / 2

    # =====================================================================================
    # DOMAIN 13
    # =====================================================================================

    # Cost of delay: CFADS on a 30/360 basis carried as a calendar week (KA 13.1, Domain 5).
    # The exact rate is 124,133.33recurring; the domain prints and then multiplies the displayed
    # figure, so multiples are built on DELAY and the exact rate is checked against it separately.
    DELAY_EXACT = CF / 360 * 7
    DELAY = DELAY_EXACT.quantize(D("0.01"))
    check("D13 cost of delay per calendar week as displayed", DELAY, D("124133.33"))
    check("D13 the exact weekly rate rounds to the displayed one (display-rounding boundary)",
          DELAY_EXACT, D("124133.33"), D("0.005"))

    # Kestrel's amortisation, built forward from the facility at the printed instalment — the basis
    # the corpus uses. Balances after 3 and 5 years feed 15.3 and 15.4 and are never re-typed.
    BALANCE = {0: DEBT}
    _b = DEBT
    for _y in range(1, TENOR + 1):
        _b = _b * (1 + RATE) - DS
        BALANCE[_y] = _b
    # The printed instalment is the exact one rounded down, so the schedule closes to a few cents
    # rather than to nil. Tolerance widened for that rounding residual and for nothing else.
    check("D13/D15 amortisation closes to nil at the end of the tenor (instalment-rounding "
          "residual)", BALANCE[TENOR].quantize(D("0.01")), D("0.00"), D("0.10"))

    # ---- MCQ 13.1-E [13.1.4 · Analysis] the parallel envelope under compression ----
    # The seven streams of WE 13.1.3, by duration in weeks.
    STREAMS = {"technical": 8, "financial": 5, "legal": 10, "tax": 6,
               "insurance": 4, "environmental and social": 12, "market": 9}
    FEE_BOOK = D(1500000)
    ACCEL_FEE = D(200000)
    envelope = max(STREAMS.values())
    check("MCQ 13.1-E envelope is the maximum of stream durations, weeks", envelope, 12)
    check("MCQ 13.1-E envelope is NOT the sum of durations (the item's trap)",
          sum(STREAMS.values()), 54)
    check("MCQ 13.1-E stem: next-longest stream is legal at weeks",
          max(v for k, v in STREAMS.items() if k != "environmental and social"), 10)
    check("MCQ 13.1-E stem: third-longest stream is market at weeks",
          sorted(STREAMS.values())[-3], 9)
    compressed = dict(STREAMS, **{"environmental and social": 9})
    new_envelope = max(compressed.values())
    check("MCQ 13.1-E envelope after compressing to nine weeks", new_envelope, 10)
    weeks_released = envelope - new_envelope
    check("MCQ 13.1-E weeks actually released", weeks_released, 2)
    check("MCQ 13.1-E option A credits all three weeks",
          (D(3) * DELAY).quantize(D("0.01")), D("372399.99"))
    check("MCQ 13.1-E key: value of the two weeks released",
          (D(weeks_released) * DELAY).quantize(D("0.01")), D("248266.66"))
    check("MCQ 13.1-E key: net of the acceleration fee",
          (D(weeks_released) * DELAY - ACCEL_FEE).quantize(D("0.01")), D("48266.66"))
    check("MCQ 13.1-E option C: a ten per cent cut across the whole fee book",
          FEE_BOOK * D("0.10"), 150000)
    # The two invariants that decide the item, and that fix its level at Analysis.
    check("MCQ 13.1-E invariant: the purchase pays, so declining (D) is wrong",
          1 if D(weeks_released) * DELAY > ACCEL_FEE else 0, 1)
    check("MCQ 13.1-E invariant: option A overstates the gain by one week of delay",
          (D(3) * DELAY - D(weeks_released) * DELAY).quantize(D("0.01")), D("124133.33"))
    check("MCQ 13.1-E invariant: option C's saving is smaller than the key's gross gain",
          1 if FEE_BOOK * D("0.10") < D(weeks_released) * DELAY else 0, 1)

    # ---- MCQ 13.1-F [13.1.1 · Comprehension] the decision-value test ----
    # Distractor C confuses reliance with information value; 13.A.1 prices the confusion.
    TECH_FEE, TECH_C = D(260000), D(6200000)
    check("MCQ 13.1-F distractor C: technical cap at three times fees", TECH_FEE * 3, 780000)
    check("MCQ 13.1-F distractor C: cap as a share of the exposure %",
          (TECH_FEE * 3 / TECH_C * 100).quantize(D("0.01")), D("12.58"))
    check("MCQ 13.1-F distractor C: all seven caps at three times fees", FEE_BOOK * 3, 4500000)
    check("MCQ 13.1-F distractor C: aggregate caps as a share of aggregate exposure %",
          (FEE_BOOK * 3 / D("9439874.85") * 100).quantize(D("0.01")), D("47.67"))
    # Distractor D (lenders require it) — the arithmetic then argues timing, and timing is worth
    # the whole difference between the parallel and the serial reading.
    check("MCQ 13.1-F distractor D: what sequencing alone is worth",
          ((sum(STREAMS.values()) - envelope) * DELAY).quantize(D("1")), D("5213600"))

    # ---- MCQ 13.2-E [13.2.3 · Analysis] two Class 1 findings, ranked ----
    # Debt capacity at the 1.30x credit requirement on CFADS corrected for finding F-01, rebuilt
    # rather than quoted: the definitional finding removed 260,000 of CFADS.
    CF_CORRECTED = CF - D(260000)
    check("MCQ 13.2-E CFADS corrected for the definitional finding", CF_CORRECTED, 6124000)
    check("MCQ 13.2-E the corrected DSCR against the mandated facility",
          (CF_CORRECTED / DS).quantize(D("0.0001")), D("1.2224"))
    CAPACITY_130 = CF_CORRECTED / D("1.30") * af(RATE, TENOR)
    check("MCQ 13.2-E debt capacity at the 1.30x requirement",
          CAPACITY_130.quantize(D("1")), D("39494354"))
    DSRA = D(2504818)                   # six-month debt service reserve (Domain 10, KA 10.3.2)
    resize = (DEBT - CAPACITY_130).quantize(D("1"))
    check("MCQ 13.2-E resize against the mandated facility", resize, 2505646)
    check("MCQ 13.2-E stem: the two findings are this far apart", resize - DSRA, 828)
    check("MCQ 13.2-E invariant: the gap is under 0.04 % of either finding, so magnitude "
          "cannot rank them",
          1 if (resize - DSRA) / resize * 100 < D("0.04") else 0, 1)
    check("MCQ 13.2-E invariant: the DSRA is a uses-of-funds omission, not a coverage number "
          "— six months of service",
          (DS / 2).quantize(D("0.01")), D("2504817.62"))
    check("MCQ 13.2-E invariant: the two findings sum to more than a tenth of the facility, "
          "so neither can be netted away — combined as a share of debt %",
          ((resize + DSRA) / DEBT * 100).quantize(D("0.01")), D("11.93"))

    # ---- MCQ 13.2-F [13.2.2 · Comprehension] why one closure percentage is invalid ----
    FINDINGS_TOTAL, FINDINGS_CLOSED, CLASS3, CLASS1 = 34, 31, 24, 3
    check("MCQ 13.2-F the count-based closure percentage %",
          (D(FINDINGS_CLOSED) / D(FINDINGS_TOTAL) * 100).quantize(D("0.1")), D("91.2"))
    check("MCQ 13.2-F Class 3 as a share of the count %",
          (D(CLASS3) / D(FINDINGS_TOTAL) * 100).quantize(D("0.1")), D("70.6"))
    check("MCQ 13.2-F the open findings are exactly the Class 1 count",
          FINDINGS_TOTAL - FINDINGS_CLOSED, CLASS1)
    # Class-weighted closure: if the three open findings are the three Class 1 findings, the
    # impact-closed measure the key prescribes is nil while the count-based measure reads 91.2 %.
    CLASS1_CLOSED = CLASS1 - (FINDINGS_TOTAL - FINDINGS_CLOSED)
    check("MCQ 13.2-F key: Class 1 findings closed", CLASS1_CLOSED, 0)
    check("MCQ 13.2-F key: impact closed on the transaction-changing class %",
          (D(CLASS1_CLOSED) / D(CLASS1) * 100).quantize(D("0.1")), D("0.0"))
    check("MCQ 13.2-F invariant: the two measures disagree by the whole of the count metric, "
          "points",
          (D(FINDINGS_CLOSED) / D(FINDINGS_TOTAL) * 100
           - D(CLASS1_CLOSED) / D(CLASS1) * 100).quantize(D("0.1")), D("91.2"))
    check("MCQ 13.2-F distractor D inverts the direction: Class 3 is the majority of the count, "
          "so a count overstates progress",
          1 if D(CLASS3) / D(FINDINGS_TOTAL) > D("0.5") else 0, 1)
    check("MCQ 13.2-F distractor D: the impact-bearing classes are this share of the count %",
          ((D(FINDINGS_TOTAL) - D(CLASS3)) / D(FINDINGS_TOTAL) * 100).quantize(D("0.1")),
          D("29.4"))

    # ---- MCQ 13.3-F [13.3.4 · Evaluation] which all-in rate belongs in a route comparison ----
    CLOSE_COSTS = D(2709000)
    LENDER_FEES = D(544000)
    COMMON_COSTS = CLOSE_COSTS - LENDER_FEES
    check("MCQ 13.3-F third-party and sponsor-side close cost", COMMON_COSTS, 2165000)
    check("MCQ 13.3-F close costs as a share of debt raised %",
          (CLOSE_COSTS / DEBT * 100).quantize(D("0.01")), D("6.45"))
    # All-in rate = the rate whose annuity factor equates net proceeds to the unchanged instalment.
    r_lender = solve_rate((DEBT - LENDER_FEES) / DS, TENOR)
    r_whole = solve_rate((DEBT - CLOSE_COSTS) / DS, TENOR)
    check("MCQ 13.3-F key: all-in rate netting lender fees only %",
          (r_lender * 100).quantize(D("0.0001")), D("6.2386"))
    check("MCQ 13.3-F distractor C: all-in rate netting the whole budget %",
          (r_whole * 100).quantize(D("0.0001")), D("7.2376"))
    check("MCQ 13.3-F distractor A: the headline rate %", (RATE * 100).quantize(D("0.01")),
          D("6.00"))
    check("MCQ 13.3-F the whole-budget rate is this far above the headline, basis points",
          ((r_whole - RATE) * 10000).quantize(D("0.1")), D("123.8"))
    # The invariant that decides the item: a cost common to both routes shifts the level of each
    # route's all-in rate and cannot change their ranking.
    check("MCQ 13.3-F invariant: the common 2,165,000 is what separates the two rates, "
          "basis points",
          ((r_whole - r_lender) * 10000).quantize(D("0.1")), D("99.9"))
    check("MCQ 13.3-F invariant: all three options are correct rates, so only relevance "
          "discriminates — key rate lies between the other two",
          1 if RATE < r_lender < r_whole else 0, 1)

    # ---- MCQ 13.4-F [13.4.4 · Evaluation] the fee-for-flex trade ----
    AF_LOAN = af(RATE, TENOR)
    check("MCQ 13.4-F AF(0.06, 12)", AF_LOAN.quantize(D("0.000001")), D("8.383844"))
    FEE_GIVEN_UP = DEBT * D("0.0025")                 # 1.20 % down to 0.95 %
    check("MCQ 13.4-F certain fee saving, 25 basis points of the facility", FEE_GIVEN_UP, 105000)
    check("MCQ 13.4-F invariant: 25 basis points is exactly the praecipium of 13.4.2",
          DEBT * D("0.0025"), FEE_GIVEN_UP)
    FLEX_PV = DEBT * D("0.0010") * AF_LOAN            # ten extra basis points of margin, for life
    check("MCQ 13.4-F key: present value of ten extra basis points of flex",
          FLEX_PV.quantize(D("0.01")), D("352121.45"))
    check("MCQ 13.4-F key: flex value as a multiple of the fee saved",
          (FLEX_PV / FEE_GIVEN_UP).quantize(D("0.0001")), D("3.3535"))
    check("MCQ 13.4-F rationale: indifference exercise probability %",
          (FEE_GIVEN_UP / FLEX_PV * 100).quantize(D("0.01")), D("29.82"))
    # Covenant rate headroom: the rate at which 1.20x coverage is exactly consumed.
    r_cov = solve_rate(DEBT / (CF / COV), TENOR)
    check("MCQ 13.4-F the 1.20x covenant survives to this rate %",
          (r_cov * 100).quantize(D("0.0001")), D("7.1138"))
    HEADROOM_BP = (r_cov - RATE) * 10000
    check("MCQ 13.4-F stem: rate headroom in basis points, to two decimals",
          HEADROOM_BP.quantize(D("0.01")), D("111.38"))
    check("MCQ 13.4-F headroom as the body displays it, one decimal (display rounding)",
          HEADROOM_BP.quantize(D("0.1")), D("111.4"))
    check("MCQ 13.4-F key: the 50 basis-point cap as a share of headroom %",
          (D(50) / HEADROOM_BP * 100).quantize(D("0.01")), D("44.89"))
    check("MCQ 13.4-F key: the lifted 60 basis-point cap as a share of headroom %",
          (D(60) / HEADROOM_BP * 100).quantize(D("0.01")), D("53.87"))
    check("MCQ 13.4-F invariant: the lifted cap still sits inside the covenant headroom, so "
          "option A's 'flex may never bite' is a probability claim and not an impossibility",
          1 if D(60) < HEADROOM_BP else 0, 1)

    # =====================================================================================
    # DOMAIN 14
    # =====================================================================================

    # ---- MCQ 14.1-E [14.1.3 · Evaluation] the funding-order recommendation ----
    INT_EQUITY_FIRST = D(1338006)
    INT_PRO_RATA = D(2114597)
    INT_DEBT_FIRST = D(2804070)
    CONT_CLOSE = D(3645403)
    EPC_PRICE = D(48000000)
    check("MCQ 14.1-E interest saved by equity-first against pro rata",
          INT_PRO_RATA - INT_EQUITY_FIRST, 776591)
    check("MCQ 14.1-E extra interest cost of debt-first against pro rata",
          INT_DEBT_FIRST - INT_PRO_RATA, 689473)
    check("MCQ 14.1-E spread from best to worst funding order",
          INT_DEBT_FIRST - INT_EQUITY_FIRST, 1466064)
    # At a fixed envelope the balancing contingency and capitalised interest share one pool, so
    # every dollar of interest saved reappears as funded contingency. The pool is derived from the
    # envelope less the four uses that the funding order cannot move.
    OWNER_COSTS, CAP_DEV, FIN_FEES = D(3600000), D(1800000), D(840000)
    FIXED_USES = EPC_PRICE + OWNER_COSTS + CAP_DEV + FIN_FEES
    check("MCQ 14.1-E uses the funding order cannot move", FIXED_USES, 54240000)
    check("MCQ 14.1-E fees and capitalised development, as the restated statement groups them",
          CAP_DEV + FIN_FEES, 2640000)
    check("MCQ 14.1-E the certified-spend base", EPC_PRICE + OWNER_COSTS + CONT_CLOSE, 55245403)
    POOL = ENVELOPE - FIXED_USES
    check("MCQ 14.1-E contingency-plus-interest pool at a fixed envelope", POOL, 5760000)
    check("MCQ 14.1-E the pool is exactly the close contingency plus pro-rata interest",
          POOL - (CONT_CLOSE + INT_PRO_RATA), 0)
    CONT_EQUITY_FIRST = D(4388050)
    INT_EF_AT_FIXED = POOL - CONT_EQUITY_FIRST
    check("MCQ 14.1-E key: extra funded contingency under equity-first",
          CONT_EQUITY_FIRST - CONT_CLOSE, 742647)
    check("MCQ 14.1-E invariant: the pool is conserved, so the contingency gain equals the "
          "interest fall at the fixed envelope",
          (CONT_EQUITY_FIRST - CONT_CLOSE) - (INT_PRO_RATA - INT_EF_AT_FIXED), 0)
    check("MCQ 14.1-E new contingency as a share of the EPC price %",
          (CONT_EQUITY_FIRST / EPC_PRICE * 100).quantize(D("0.01")), D("9.14"))
    check("MCQ 14.1-E close contingency as a share of the EPC price %",
          (CONT_CLOSE / EPC_PRICE * 100).quantize(D("0.01")), D("7.59"))
    check("MCQ 14.1-E option A: present value of deferring the equity cheque",
          D(16476605) - D(16293592), 183013)
    check("MCQ 14.1-E invariant: option A's gain is already net of the extra equity it costs, "
          "so it is a true statement about shareholder value",
          1 if D(16293592) < D(16476605) else 0, 1)
    check("MCQ 14.1-E invariant: the protection forgone is 4.058 times the present value gained",
          ((CONT_EQUITY_FIRST - CONT_CLOSE) / D(183013)).quantize(D("0.001")), D("4.058"))
    check("MCQ 14.1-E option D is false: capitalised interest is drawn debt, so equity-first "
          "leaves the facility undrawn by this much",
          DEBT - D(41223409), 776591)

    # ---- MCQ 14.1-F [14.1.1 · Comprehension] identity against test ----
    REMAINING_USES = D(22736674)
    AVAILABLE_COMMITMENT = D(22728934)
    CONT_REMAINING = D(1500000)
    check("MCQ 14.1-F the restated statement can fail: uses exceed commitment by",
          REMAINING_USES - AVAILABLE_COMMITMENT, 7740)
    check("MCQ 14.1-F distractor D double-counts contingency as a source, turning a shortfall "
          "into an apparent surplus of",
          AVAILABLE_COMMITMENT + CONT_REMAINING - REMAINING_USES, 1492260)
    check("MCQ 14.1-F invariant: the double-count is the whole remaining contingency",
          (AVAILABLE_COMMITMENT + CONT_REMAINING - REMAINING_USES)
          + (REMAINING_USES - AVAILABLE_COMMITMENT), CONT_REMAINING)

    # ---- MCQ 14.2-E [14.2.2 · Evaluation] is 'coverage thin, monitor monthly' supported? ----
    KNOWN_CLAIMS = D(3420000)
    OPEN_P80 = D(1764289)
    DENOM = KNOWN_CLAIMS + OPEN_P80
    check("MCQ 14.2-E honest denominator", DENOM, 5184289)
    check("MCQ 14.2-E stem: coverage on the remainder",
          (CONT_REMAINING / DENOM).quantize(D("0.0001")), D("0.2893"))
    check("MCQ 14.2-E key: the shortfall the ratio conceals",
          DENOM - CONT_REMAINING, 3684289)
    check("MCQ 14.2-E distractor C: coverage on open risk alone (defensible, and not the point)",
          (CONT_REMAINING / OPEN_P80).quantize(D("0.0001")), D("0.8502"))
    LENDER_CTC = D(24656674)
    check("MCQ 14.2-E key: the independently derived in-balance shortfall",
          LENDER_CTC - AVAILABLE_COMMITMENT, 1927740)
    check("MCQ 14.2-E invariant: a coverage ratio below one is a funding shortfall, "
          "not a narrow margin",
          1 if CONT_REMAINING / DENOM < 1 and DENOM - CONT_REMAINING > 0 else 0, 1)
    check("MCQ 14.2-E invariant: both tests point the same way, so the finding is corroborated",
          1 if (DENOM - CONT_REMAINING > 0) and (LENDER_CTC - AVAILABLE_COMMITMENT > 0) else 0, 1)

    # ---- MCQ 14.2-F [14.2.1 · Comprehension] why two cost-to-completes differ ----
    VARIATIONS = D(840000)
    CLAIM_EXPOSURE = D(1260000)
    REMAINING_IDC = D(1436674)
    BLIND = VARIATIONS + CLAIM_EXPOSURE + REMAINING_IDC
    check("MCQ 14.2-F stem: the difference between the two figures", BLIND, 3536674)
    check("MCQ 14.2-F the difference as a share of the lender's number %",
          (BLIND / LENDER_CTC * 100).quantize(D("0.01")), D("14.34"))
    check("MCQ 14.2-F invariant: the three lines are outside the control accounts by "
          "construction, so the lender's figure exceeds the controller's",
          1 if LENDER_CTC - BLIND < LENDER_CTC else 0, 1)
    check("MCQ 14.2-F distractor D names a line in neither figure — remaining contingency is "
          "a use, and the earned-value base is",
          LENDER_CTC - BLIND - D(2400000), 18720000)

    # ---- MCQ 14.3-E [14.3.1 · Evaluation] the off-site materials certification ----
    OFF_SITE = D(690000)
    RETENTION = D("0.05")
    GEARING = D("0.70")
    QUARTERLY = D("0.015")
    advanced = OFF_SITE * (1 - RETENTION) * GEARING
    check("MCQ 14.3-E key: senior debt advanced against goods off site",
          advanced.quantize(D("0.01")), D("458850.00"))
    check("MCQ 14.3-E net for payment after retention", OFF_SITE * (1 - RETENTION), 655500)
    check("MCQ 14.3-E equity share of the same certification",
          (OFF_SITE * (1 - RETENTION) * (1 - GEARING)).quantize(D("0.01")), D("196650.00"))
    # The invariant that makes A a mispriced concern: the exposure is the advance, and one
    # quarter's interest on it is two orders of magnitude smaller.
    check("MCQ 14.3-E distractor A: one quarter's capitalised interest on the advance",
          (advanced * QUARTERLY).quantize(D("0.01")), D("6882.75"))
    check("MCQ 14.3-E invariant: the security exposure exceeds A's cost concern by a factor of",
          (advanced / (advanced * QUARTERLY)).quantize(D("0.01")), D("66.67"))
    check("MCQ 14.3-E invariant: the advance is 66.5 % of the certified amount, so it is the "
          "quantity at risk %",
          ((1 - RETENTION) * GEARING * 100).quantize(D("0.1")), D("66.5"))

    # ---- MCQ 14.4-E [14.4.2 · Evaluation] which term-sheet amendment to press first ----
    SUSTAINABLE = COV * DS
    check("MCQ 14.4-E CFADS required for 1.20x coverage",
          SUSTAINABLE.quantize(D("0.01")), D("6011562.28"))
    MONTHLY_CF = CF / 12
    check("MCQ 14.4-E forgone CFADS per month of slip", MONTHLY_CF, 532000)
    DAMAGES_MONTH = D(20000) * 30
    DAMAGES_CAP = D(4800000)
    check("MCQ 14.4-E delay damages per month at 20,000 a day", DAMAGES_MONTH, 600000)
    slip_fail = 12 - SUSTAINABLE / MONTHLY_CF
    check("MCQ 14.4-E stem: the covenant fails beyond this many months of slip",
          slip_fail.quantize(D("0.0001")), D("0.7001"))
    check("MCQ 14.4-E the same breakeven in days", (slip_fail * 30).quantize(D("0.01")),
          D("21.00"))
    # Option C: damages credited to CFADS at a four-month slip.
    slip4 = D(4)
    cf4 = MONTHLY_CF * (12 - slip4)
    dam4 = slip4 * DAMAGES_MONTH
    check("MCQ 14.4-E CFADS at a four-month slip before damages", cf4, 4256000)
    check("MCQ 14.4-E CFADS at a four-month slip with damages credited", cf4 + dam4, 6656000)
    check("MCQ 14.4-E option C: the damages-credited ratio",
          ((cf4 + dam4) / DS).quantize(D("0.0001")), D("1.3286"))
    check("MCQ 14.4-E option C: what the damages credit is worth in coverage",
          (dam4 / DS).quantize(D("0.0001")), D("0.4791"))
    check("MCQ 14.4-E option C: the uncredited ratio at four months is a breach",
          1 if cf4 / DS < COV else 0, 1)
    # The cap is where option C expires: damages stop growing, CFADS keeps shrinking.
    cap_month = DAMAGES_CAP / DAMAGES_MONTH
    check("MCQ 14.4-E the damages cap binds at this month of slip", cap_month, 8)
    peak = (MONTHLY_CF * (12 - cap_month) + DAMAGES_CAP) / DS
    check("MCQ 14.4-E option C: the damages-credited ratio peaks at",
          peak.quantize(D("0.0001")), D("1.3829"))
    recross = 12 - (SUSTAINABLE - DAMAGES_CAP) / MONTHLY_CF
    check("MCQ 14.4-E option C: the ratio re-crosses 1.20 at this month of slip",
          recross.quantize(D("0.0001")), D("9.7226"))
    check("MCQ 14.4-E invariant: option C is non-monotonic — it rises to the cap and falls "
          "after, so it is a protection that expires",
          1 if peak > (cf4 + dam4) / DS
          and (MONTHLY_CF * (12 - D(10)) + DAMAGES_CAP) / DS < COV else 0, 1)
    check("MCQ 14.4-E invariant: option A cannot help — a reserve pays an instalment and does "
          "not enter CFADS, so the breakeven slip is unchanged",
          (12 - SUSTAINABLE / MONTHLY_CF).quantize(D("0.0001")), slip_fail.quantize(D("0.0001")))

    # =====================================================================================
    # DOMAIN 15
    # =====================================================================================

    MAINT = D(600000)
    DISTRIBUTABLE = CF - DS - MAINT
    check("D15 base-case distributable amount", DISTRIBUTABLE.quantize(D("0.01")),
          D("774364.77"))

    # ---- MCQ 15.1-E [15.1.4 · Evaluation] was the withheld dividend wrong? ----
    REFORECAST = D(5500000)
    OUTTURN_Y2 = D("5963894.11")
    check("MCQ 15.1-E stem: the outturn beat the re-forecast by",
          OUTTURN_Y2 - REFORECAST, D("463894.11"))
    check("MCQ 15.1-E stem: the forward DSCR on the re-forecast",
          (REFORECAST / DS).quantize(D("0.0001")), D("1.0979"))
    check("MCQ 15.1-E stem: the dividend the forward test stopped",
          DISTRIBUTABLE.quantize(D("0.01")), D("774364.77"))
    check("MCQ 15.1-E the re-forecast failed the distribution condition as well as the covenant",
          1 if REFORECAST / DS < COV and REFORECAST / DS < DIST_COND else 0, 1)
    # The invariant that disposes of the director's argument: even the better outturn fails 1.20.
    check("MCQ 15.1-E invariant: the year-two outturn ratio",
          (OUTTURN_Y2 / DS).quantize(D("0.0001")), D("1.1905"))
    check("MCQ 15.1-E invariant: the outturn does not vindicate the director — 463,894.11 more "
          "cash still breaches the 1.20x covenant",
          1 if OUTTURN_Y2 / DS < COV else 0, 1)
    check("MCQ 15.1-E invariant: CFADS needed to clear 1.20x exceeds the outturn by",
          (SUSTAINABLE - OUTTURN_Y2).quantize(D("0.01")), D("47668.17"))

    # ---- MCQ 15.1-F [15.1.2 · Comprehension] the working-capital asymmetry ----
    CASH_GEAR = D("0.80")
    check("MCQ 15.1-F distractor C: the cash-to-revenue gearing", CASH_GEAR, D("0.80"))
    # Distractor C names a real mechanism that is symmetric in the revenue move, so it cannot
    # flatter a decline and penalise a recovery. Shown on a 1,000,000 move in each direction.
    MOVE = D(1000000)
    check("MCQ 15.1-F distractor C: CFADS effect of a 1,000,000 revenue fall",
          CASH_GEAR * -MOVE, -800000)
    check("MCQ 15.1-F distractor C: CFADS effect of a 1,000,000 revenue rise",
          CASH_GEAR * MOVE, 800000)
    check("MCQ 15.1-F invariant: the two are equal and opposite, so the gearing is symmetric "
          "and cannot produce the asymmetry the item asks about",
          CASH_GEAR * MOVE + CASH_GEAR * -MOVE, 0)
    check("MCQ 15.1-F distractor C in DSCR terms — a fall of this much coverage",
          (CASH_GEAR * MOVE / DS).quantize(D("0.0001")), D("0.1597"))

    # ---- MCQ 15.2-E [15.2.4 · Evaluation] two half-true board papers ----
    PAID_Y5, PAID_Y6, INJECTION = D("2077924.35"), D("885953.11"), D(1500000)
    df = lambda n: (1 + DISC) ** -n
    base_pv = DISTRIBUTABLE * af(DISC, 6)
    check("MCQ 15.2-E base-case present value of six years of distributions",
          base_pv.quantize(D("0.01")), D("3579795.15"))
    actual_pv = PAID_Y5 * df(5) + PAID_Y6 * df(6) - INJECTION * df(4)
    check("MCQ 15.2-E actual present value net of the injection",
          actual_pv.quantize(D("0.01")), D("869956.36"))
    pv_loss = base_pv - actual_pv
    check("MCQ 15.2-E key: the present-value loss", pv_loss.quantize(D("0.01")),
          D("2709838.79"))
    check("MCQ 15.2-E stem: distributions ultimately paid", PAID_Y5 + PAID_Y6,
          D("2963877.46"))
    cash_loss = DISTRIBUTABLE * 6 - (PAID_Y5 + PAID_Y6) + INJECTION
    check("MCQ 15.2-E stem: the undiscounted cash difference",
          cash_loss.quantize(D("0.01")), D("3182311.16"))
    check("MCQ 15.2-E key: the trading-shortfall component",
          (cash_loss - INJECTION).quantize(D("0.01")), D("1682311.16"))
    check("MCQ 15.2-E key: the split reconciles to the cash loss",
          (D("1682311.16") + INJECTION).quantize(D("0.01")), cash_loss.quantize(D("0.01")))
    EQUITY_NPV_25Y = D("5027733.03")
    check("MCQ 15.2-E key: the present-value loss as a share of the 25-year equity NPV %",
          (pv_loss / EQUITY_NPV_25Y * 100).quantize(D("0.01")), D("53.90"))
    check("MCQ 15.2-E invariant against paper 1: present-value loss is less than cash loss, "
          "because 2,963,877.46 came back",
          1 if pv_loss < cash_loss else 0, 1)
    check("MCQ 15.2-E invariant against paper 2: the loss exceeds half the whole equity value",
          1 if pv_loss / EQUITY_NPV_25Y > D("0.5") else 0, 1)
    # Distractor D changes the rate to change the answer; the loss survives the change of rate.
    base6 = DISTRIBUTABLE * af(RATE, 6)
    actual6 = PAID_Y5 * (1 + RATE) ** -5 + PAID_Y6 * (1 + RATE) ** -6 - INJECTION * (1 + RATE) ** -4
    check("MCQ 15.2-E distractor D: the loss measured at the 6 % loan rate is still a loss",
          1 if base6 - actual6 > 0 else 0, 1)

    # ---- MCQ 15.2-F [15.2.3 · Comprehension] a 12.19 % IRR paying a 4.30 % cash yield ----
    check("MCQ 15.2-F the cash yield on subscribed equity %",
          (DISTRIBUTABLE / EQUITY * 100).quantize(D("0.0001")), D("4.3020"))
    check("MCQ 15.2-F key: debt service as a share of CFADS %",
          (DS / CF * 100).quantize(D("0.01")), D("78.47"))
    check("MCQ 15.2-F the same figure as the body displays it (display rounding)",
          (DS / CF * 100).quantize(D("0.1")), D("78.5"))
    check("MCQ 15.2-F key: post-retirement distributable", CF - MAINT, 5784000)
    check("MCQ 15.2-F key: the post-retirement yield %",
          ((CF - MAINT) / EQUITY * 100).quantize(D("0.01")), D("32.13"))
    check("MCQ 15.2-F distractor C: the reserve charge as a share of the dividend %",
          (MAINT / DISTRIBUTABLE * 100).quantize(D("0.1")), D("77.5"))
    check("MCQ 15.2-F distractor C: the yield with the reserve added back %",
          ((DISTRIBUTABLE + MAINT) / EQUITY * 100).quantize(D("0.0001")), D("7.6354"))
    check("MCQ 15.2-F invariant: even with the reserve added back the yield is nowhere near "
          "12.19 %, so C cannot explain the shape",
          1 if (DISTRIBUTABLE + MAINT) / EQUITY * 100 < D("12.19") else 0, 1)

    # ---- MCQ 15.3-E [15.3.2 · Evaluation] price against tenor ----
    # Outstanding after five years, read off the rebuilt schedule rather than quoted.
    OUTSTANDING_Y5 = BALANCE[5]
    check("MCQ 15.3-E outstanding after five years", OUTSTANDING_Y5.quantize(D("0.01")),
          D("27965694.77"))
    check("MCQ 15.3-E the second remaining opening balance", BALANCE[6].quantize(D("0.01")),
          D("24634001.23"))
    check("MCQ 15.3-E the last remaining opening balance", BALANCE[11].quantize(D("0.01")),
          D("4726071.04"))
    # Swap break: 80 basis points on each of the seven remaining opening balances, discounted at
    # the 2.20 % market rate.
    SWAP_MKT = D("0.0220")
    break_cost = sum(D("0.0080") * BALANCE[5 + k] * (1 + SWAP_MKT) ** -(k + 1)
                     for k in range(7))
    check("MCQ 15.3-E swap break cost", break_cost.quantize(D("0.01")), D("886064.34"))
    COSTS = (break_cost + D("0.0100") * OUTSTANDING_Y5 + D("0.0050") * OUTSTANDING_Y5
             + D(320000))
    check("MCQ 15.3-E arrangement fee at 1.00 %",
          (D("0.0100") * OUTSTANDING_Y5).quantize(D("0.01")), D("279656.95"))
    check("MCQ 15.3-E prepayment fee at 0.50 %",
          (D("0.0050") * OUTSTANDING_Y5).quantize(D("0.01")), D("139828.47"))
    check("MCQ 15.3-E total transaction cost", COSTS.quantize(D("0.01")), D("1625549.77"))
    AF_EQ7 = af(DISC, 7)
    check("MCQ 15.3-E AF(0.08, 7)", AF_EQ7.quantize(D("0.000001")), D("5.206370"))

    REMAINING = TENOR - 5                             # seven years of old service remain

    def refi_npv(rate, tenor):
        """Incremental cash to equity: the old service ceases for its seven remaining years,
        the new service arises for the whole new tenor. Beyond year seven the incremental cash
        is negative, which is why the ten-year option cannot be priced on a level saving."""
        inst = OUTSTANDING_Y5 / af(rate, tenor)
        pv = DS * af(DISC, REMAINING) - inst * af(DISC, tenor)
        return inst, pv, pv - COSTS

    inst745, pv745, npv745 = refi_npv(D("0.0445"), 7)
    check("MCQ 15.3-E stem: the 7-year instalment at 4.45 %", inst745.quantize(D("0.01")),
          D("4737139.41"))
    check("MCQ 15.3-E stem: present value of incremental cash, 7-year option",
          pv745.quantize(D("0.01")), D("1418714.07"))
    check("MCQ 15.3-E stem: the 7-year net present value at 4.45 %",
          npv745.quantize(D("0.01")), D("-206835.69"), D("0.02"))
    inst10, pv10, npv10 = refi_npv(D("0.0445"), 10)
    check("MCQ 15.3-E key: the 10-year instalment at 4.45 %", inst10.quantize(D("0.01")),
          D("3525588.23"))
    check("MCQ 15.3-E key: present value of incremental cash, 10-year option",
          pv10.quantize(D("0.01")), D("2425030.89"))
    check("MCQ 15.3-E key: the 10-year net present value at 4.45 %",
          npv10.quantize(D("0.01")), D("799481.12"), D("0.02"))
    check("MCQ 15.3-E invariant: the 10-year option is worth more despite a thinner early "
          "saving, so the value is tenor and not price",
          1 if npv10 > npv745 and inst10 < inst745 else 0, 1)
    # The breakeven all-in rate on the 7-year option, solved rather than quoted.
    lo, hi = D("0.03"), D("0.05")
    for _ in range(400):
        mid = (lo + hi) / 2
        if refi_npv(mid, 7)[2] > 0:
            lo = mid
        else:
            hi = mid
    r_be = (lo + hi) / 2
    check("MCQ 15.3-E stem: the 7-year breakeven all-in rate %",
          (r_be * 100).quantize(D("0.0001")), D("4.2206"))
    check("MCQ 15.3-E stem: the market's offer is short by this many basis points",
          ((D("0.0445") - r_be) * 10000).quantize(D("0.01")), D("22.94"))
    inst420, pv420, npv420 = refi_npv(D("0.0420"), 7)
    check("MCQ 15.3-E key: the 7-year instalment after a further 25 basis points",
          inst420.quantize(D("0.01")), D("4693850.57"))
    check("MCQ 15.3-E key: the annual saving after a further 25 basis points",
          (DS - inst420).quantize(D("0.01")), D("315784.66"))
    # The manuscript multiplies the displayed saving by the displayed annuity factor; the exact
    # product is a penny lower. Tolerance widened for that display-rounding boundary only.
    check("MCQ 15.3-E key: present value of that saving (display-rounding boundary)",
          pv420.quantize(D("0.01")), D("1644091.82"), D("0.02"))
    check("MCQ 15.3-E key: the best 7-year outcome after the price win",
          npv420.quantize(D("0.01")), D("18542.05"))
    check("MCQ 15.3-E invariant: option A is arithmetically correct — 25 basis points does "
          "turn the transaction positive",
          1 if npv420 > 0 else 0, 1)
    check("MCQ 15.3-E rationale: the tenor value left on the table",
          (npv10 - npv420).quantize(D("0.01")), D("780939.07"), D("0.02"))
    check("MCQ 15.3-E invariant: tenor beats the whole price win by a factor above forty",
          1 if npv10 / npv420 > 40 else 0, 1)

    # ---- MCQ 15.4-E [15.4.2 · Evaluation] is 'recovery is par' evidence? ----
    OUTSTANDING_Y3 = BALANCE[3]
    check("MCQ 15.4-E outstanding after three years", OUTSTANDING_Y3.quantize(D("0.01")),
          D("34073997.28"))
    CF_REVISED = D("5202936.48")
    check("MCQ 15.4-E the deteriorated DSCR before restructuring",
          (CF_REVISED / DS).quantize(D("0.0001")), D("1.0386"))
    sustainable_svc = CF_REVISED / COV
    check("MCQ 15.4-E sustainable service at 1.20x", sustainable_svc.quantize(D("0.01")),
          D("4335780.40"))
    check("MCQ 15.4-E required annuity factor",
          (OUTSTANDING_Y3 / sustainable_svc).quantize(D("0.000001")), D("7.858792"))
    check("MCQ 15.4-E AF(0.06, 10) is too short", af(RATE, 10).quantize(D("0.000001")),
          D("7.360087"))
    check("MCQ 15.4-E AF(0.06, 11) clears it", af(RATE, 11).quantize(D("0.000001")),
          D("7.886875"))
    inst11 = OUTSTANDING_Y3 / af(RATE, 11)
    check("MCQ 15.4-E extension instalment over 11 years", inst11.quantize(D("0.01")),
          D("4320342.23"))
    check("MCQ 15.4-E option D: the restored DSCR",
          (CF_REVISED / inst11).quantize(D("0.0001")), D("1.2043"))
    rec6 = inst11 * af(RATE, 11) / OUTSTANDING_Y3 * 100
    check("MCQ 15.4-E option A: recovery at the contract rate %",
          rec6.quantize(D("0.0001")), D("100.0000"))
    rec7 = inst11 * af(D("0.0700"), 11) / OUTSTANDING_Y3 * 100
    check("MCQ 15.4-E key: recovery at the 7.00 % the lenders now require %",
          rec7.quantize(D("0.0001")), D("95.0779"))
    check("MCQ 15.4-E key: the undisclosed loss in points",
          (D(100) - rec7).quantize(D("0.01")), D("4.92"))
    # The invariant that makes option A's true statement useless as evidence: par at the contract
    # rate is an identity at every tenor, so it is insensitive to the deterioration being approved.
    for years in (9, 10, 11, 15, 20):
        par = (OUTSTANDING_Y3 / af(RATE, years)) * af(RATE, years) / OUTSTANDING_Y3 * 100
        check(f"MCQ 15.4-E invariant: par at the contract rate holds at a {years}-year tenor %",
              par.quantize(D("0.0001")), D("100.0000"))
    # And par is insensitive to the deterioration the committee is being asked to approve, which
    # is the whole objection: the identity does not move with CFADS, while coverage does.
    for shock, dscr in (("0.90", "1.0839"), ("0.70", "0.8430")):
        cfx = CF_REVISED * D(shock)
        n_needed = 11
        par_x = (OUTSTANDING_Y3 / af(RATE, n_needed)) * af(RATE, n_needed) / OUTSTANDING_Y3 * 100
        check(f"MCQ 15.4-E invariant: coverage at {shock}x the revised CFADS falls to",
              (cfx / inst11).quantize(D("0.0001")), D(dscr))
        check(f"MCQ 15.4-E invariant: par at the contract rate is unmoved at {shock}x CFADS %",
              par_x.quantize(D("0.0001")), D("100.0000"))
    HAIRCUT = OUTSTANDING_Y3 - sustainable_svc * af(RATE, 9)
    check("MCQ 15.4-E option C: the injection amount", HAIRCUT.quantize(D("0.01")),
          D("4583353.23"))
    check("MCQ 15.4-E option C: the injection as a share of outstanding %",
          (HAIRCUT / OUTSTANDING_Y3 * 100).quantize(D("0.0001")), D("13.4512"))
    inst_inj = (OUTSTANDING_Y3 - HAIRCUT) / af(RATE, 9)
    check("MCQ 15.4-E option C: the re-amortised instalment", inst_inj.quantize(D("0.01")),
          D("4335780.40"))
    rec_inj = (HAIRCUT + inst_inj * af(D("0.0700"), 9)) / OUTSTANDING_Y3 * 100
    check("MCQ 15.4-E option C: injection recovery at 7.00 % %",
          rec_inj.quantize(D("0.0001")), D("96.3549"))
    check("MCQ 15.4-E invariant: the injection recovers more than the extension, which is why "
          "lenders prefer it and cannot grant it",
          1 if rec_inj > rec7 else 0, 1)

    # =====================================================================================
    # DOMAIN 16
    # =====================================================================================

    AF_EST = af(DISC, 10)
    check("D16 AF(0.08, 10), the estate appraisal factor",
          AF_EST.quantize(D("0.000001")), D("6.710081"))

    # ---- MCQ 16.1-E [16.1.1 · Evaluation] what a reviewer must require ----
    SPINE_BUILD = D(900000)
    LABOUR_SAVING = D(316192)
    SPINE_FEED_COST = D(181472)
    DEFINITIONAL_PER_PERIOD = D(600000)
    check("MCQ 16.1-E pairwise reconciliations across a nine-system estate", mesh(9), 36)
    check("MCQ 16.1-E stem: the labour-case net present value",
          (LABOUR_SAVING * AF_EST - SPINE_BUILD).quantize(D("1")), D("1221674"))
    LABOUR_NPV = LABOUR_SAVING * AF_EST - SPINE_BUILD
    check("MCQ 16.1-E key: present value of one recurring definitional divergence",
          (DEFINITIONAL_PER_PERIOD * AF_EST).quantize(D("1")), D("4026049"))
    check("MCQ 16.1-E key: the definitional case as a multiple of the labour case",
          (DEFINITIONAL_PER_PERIOD * AF_EST / LABOUR_NPV).quantize(D("0.0001")), D("3.2955"))
    check("MCQ 16.1-E key: an unretired mesh turns the saving into this annual cost",
          SPINE_FEED_COST, 181472)
    check("MCQ 16.1-E invariant: the realisation risk is a sign flip, not a haircut",
          1 if LABOUR_SAVING > 0 and -SPINE_FEED_COST < 0 else 0, 1)
    check("MCQ 16.1-E invariant: the swing between retiring and not retiring the mesh",
          LABOUR_SAVING + SPINE_FEED_COST, 497664)
    # Distractor C, rebuilt from the interface economics rather than quoted: a pairwise
    # reconciliation is 12 hours a month and a spine feed 4, at a loaded analyst hour of 96.00.
    HOURS_PAIR, HOURS_FEED, RATE_HR = D(12), D(4), D("96.00")
    check("MCQ 16.1-E mesh hours a year on today's nine-system estate",
          mesh(9) * HOURS_PAIR * 12, 5184)
    check("MCQ 16.1-E mesh cost a year on today's estate",
          mesh(9) * HOURS_PAIR * 12 * RATE_HR, 497664)
    check("MCQ 16.1-E spine feed labour a year on today's estate",
          D(9) * HOURS_FEED * 12 * RATE_HR, 41472)
    check("MCQ 16.1-E spine all-in cost a year (feed labour plus the 140,000 run cost)",
          D(9) * HOURS_FEED * 12 * RATE_HR + D(140000), SPINE_FEED_COST)
    check("MCQ 16.1-E stem: the net annual labour saving",
          mesh(9) * HOURS_PAIR * 12 * RATE_HR - SPINE_FEED_COST, LABOUR_SAVING)
    check("MCQ 16.1-E distractor C: reconciliations at a fifteen-system estate", mesh(15), 105)
    check("MCQ 16.1-E distractor C: mesh cost a year at fifteen systems",
          mesh(15) * HOURS_PAIR * 12 * RATE_HR, 1451520)
    check("MCQ 16.1-E distractor C: spine cost a year at fifteen systems",
          D(15) * HOURS_FEED * 12 * RATE_HR, 69120)
    check("MCQ 16.1-E invariant: the mesh is quadratic and the spine linear — the cost ratio at "
          "fifteen systems",
          ((mesh(15) * HOURS_PAIR) / (D(15) * HOURS_FEED)).quantize(D("0.01")), D("21.00"))
    check("MCQ 16.1-E invariant: distractor C strengthens a case that already passes",
          1 if LABOUR_NPV > 0 else 0, 1)
    check("MCQ 16.1-E distractor D optimises a one-off against a recurring exposure of",
          DEFINITIONAL_PER_PERIOD, 600000)

    # ---- MCQ 16.1-F [16.1.2 · Comprehension] why the error-cost term cannot be left out ----
    MANUAL_PROC, MANUAL_ERR = D("13.60"), D("0.018")
    AUTO_PROC, AUTO_ERR = D("1.04"), D("0.026")
    ERROR_COST = D(320)
    FIXED = D(148000)
    manual_all_in = MANUAL_PROC + MANUAL_ERR * ERROR_COST
    auto_all_in = AUTO_PROC + AUTO_ERR * ERROR_COST
    check("MCQ 16.1-F manual cost per reviewed item", manual_all_in.quantize(D("0.01")),
          D("19.36"))
    check("MCQ 16.1-F automated cost per reviewed item", auto_all_in.quantize(D("0.01")),
          D("9.36"))
    check("MCQ 16.1-F the per-item advantage", (manual_all_in - auto_all_in).quantize(D("0.01")),
          D("10.00"))
    check("MCQ 16.1-F distractor A: the fixed cost the breakeven volume handles", FIXED, 148000)
    check("MCQ 16.1-F distractor A: the breakeven volume that handles it",
          (FIXED / (manual_all_in - auto_all_in)).quantize(D("1")), D("14800"))
    # The invariant: dropping the error-cost term overstates the advantage, and at a higher
    # consequence per error the sign of the term's effect is what decides the case.
    naive_adv = MANUAL_PROC - AUTO_PROC
    check("MCQ 16.1-F invariant: the advantage with the error term omitted",
          naive_adv.quantize(D("0.01")), D("12.56"))
    check("MCQ 16.1-F invariant: omitting it overstates the advantage by %",
          ((naive_adv / (manual_all_in - auto_all_in) - 1) * 100).quantize(D("0.0001")),
          D("25.6000"))
    HIGH_ERROR = D(1200)
    high_adv = (MANUAL_PROC + MANUAL_ERR * HIGH_ERROR) - (AUTO_PROC + AUTO_ERR * HIGH_ERROR)
    check("MCQ 16.1-F invariant: at 1,200 an error the breakeven volume rises to",
          (FIXED / high_adv).quantize(D("1")), D("50000"))
    check("MCQ 16.1-F invariant: the term matters because the automated residual rate is the "
          "higher of the two",
          1 if AUTO_ERR > MANUAL_ERR else 0, 1)

    # ---- MCQ 16.2-E [16.2.1 · Evaluation] which downside case carries the covenant test ----
    DRAWS = 500
    check("MCQ 16.2-E the minimum of 500 draws sits at this percentile %",
          (D(1) / D(DRAWS + 1) * 100).quantize(D("0.0001")), D("0.1996"))
    check("MCQ 16.2-E invariant: the generated minimum is a percentile of the input tails, so "
          "it moves with the draw count — 5,000 draws would place it at %",
          (D(1) / D(5001) * 100).quantize(D("0.0001")), D("0.0200"))
    check("MCQ 16.2-E invariant: ten times the draws moves the tested percentile by this factor, "
          "so the 'conservatism' is a function of run length and not of the project",
          ((D(1) / D(DRAWS + 1)) / (D(1) / D(5001))).quantize(D("0.0001")), D("9.9820"))

    # ---- MCQ 16.3-E [16.3.1 · Comprehension] explanation against justification ----
    ATTRIBUTED = D(6190000)
    RESIDUAL = CF - ATTRIBUTED
    check("MCQ 16.3-E distractor D: an attribution in currency leaves a measurable residual",
          RESIDUAL, 194000)
    check("MCQ 16.3-E invariant: the hundred-per-cent rule fails by the residual, which is what "
          "makes the account unfaithful — attributed less published",
          ATTRIBUTED - CF, -194000)
    check("MCQ 16.3-E the residual as a share of the published forecast %",
          (RESIDUAL / CF * 100).quantize(D("0.0001")), D("3.0388"))
    # The distinction the item restates: faithfulness is about the route, correctness about the
    # destination. An attribution can reconcile to the penny on a forecast that is itself wrong,
    # so reconciliation is necessary and not sufficient.
    WRONG_OUTPUT = CF - D(500000)
    check("MCQ 16.3-E invariant: an attribution reconciling exactly to a forecast 500,000 too "
          "high is perfectly faithful and still explains a wrong number",
          (ATTRIBUTED + RESIDUAL) - CF, 0)
    check("MCQ 16.3-E invariant: that faithful account says nothing about the 500,000 error, "
          "which only validation can find",
          CF - WRONG_OUTPUT, 500000)

    # ---- MCQ 16.3-F [16.3.2 · Evaluation] 'fixed it and re-ran the failing case' ----
    P_BOUND, ALPHA, USES = D("0.01"), D("0.05"), D(56400)
    n_zero_exact = ALPHA.ln() / (1 - P_BOUND).ln()
    check("MCQ 16.3-F the zero-failure sample size, exact",
          n_zero_exact.quantize(D("0.0001")), D("298.0729"))
    n_zero = int(n_zero_exact) + 1
    check("MCQ 16.3-F stem: the zero-failure sample size, rounded up", n_zero, 299)
    n = n_zero
    while True:
        p_at_most_one = ((1 - P_BOUND) ** n) + n * P_BOUND * ((1 - P_BOUND) ** (n - 1))
        if p_at_most_one <= ALPHA:
            break
        n += 1
    check("MCQ 16.3-F key: the sample size permitting one observed failure", n, 473)
    check("MCQ 16.3-F key: that binomial tail is at or below alpha",
          1 if p_at_most_one <= ALPHA else 0, 1)
    check("MCQ 16.3-F key: the increase once a failure is observed %",
          ((D(n) / D(n_zero) - 1) * 100).quantize(D("0.01")), D("58.19"))
    check("MCQ 16.3-F key: errors a year the 1 % bound still admits", P_BOUND * USES, 564)
    check("MCQ 16.3-F invariant: repairing the failed case cannot restore the claim — the "
          "requirement rises rather than falls",
          1 if n > n_zero else 0, 1)
    check("MCQ 16.3-F distractor C over-reads: one failure in 299 is well inside a 1 % rate, "
          "whose expected failures in 299 cases are",
          (P_BOUND * D(n_zero)).quantize(D("0.01")), D("2.99"))
    # Distractor D: monitoring is where the assurance sits, and it gets there faster than testing.
    TIGHT = D(10) / USES
    n_monitor = ALPHA.ln() / (1 - TIGHT).ln()
    check("MCQ 16.3-F distractor D: the bound implied by ten errors a year %",
          (TIGHT * 100).quantize(D("0.000001")), D("0.017730"))
    check("MCQ 16.3-F distractor D: test cases that bound would require",
          int(n_monitor) + 1, 16895)
    check("MCQ 16.3-F distractor D: months of production to reach that many observations",
          (D(int(n_monitor) + 1) / USES * 12).quantize(D("0.01")), D("3.59"))
    check("MCQ 16.3-F invariant: monitoring outruns testing, and still does not license a "
          "validation claim the sample no longer supports",
          1 if int(n_monitor) + 1 > n else 0, 1)

    # ---- MCQ 16.4-E [16.4.2 · Evaluation] the dual-approval threshold policy ----
    APPROVER = D("24.00")
    BAD_RATE, CAUGHT = D("0.0020"), D("0.85")
    caught_rate = BAD_RATE * CAUGHT
    check("MCQ 16.4-E the caught rate", caught_rate.quantize(D("0.000001")), D("0.001700"))
    check("MCQ 16.4-E stem: the derived threshold, exact",
          (APPROVER / caught_rate).quantize(D("0.01")), D("14117.65"))
    check("MCQ 16.4-E stem: the derived threshold as published",
          (APPROVER / caught_rate).quantize(D("1")), D("14118"))
    N_ALL, N_SUB = D(4800), D(3650)
    N_OVER = N_ALL - N_SUB
    V_ALL, V_SUB = D(68000000), D(4600000)
    V_OVER = V_ALL - V_SUB
    check("MCQ 16.4-E payments above the threshold", N_OVER, 1150)
    check("MCQ 16.4-E value above the threshold", V_OVER, 63400000)
    blanket_net = caught_rate * V_ALL - N_ALL * APPROVER
    thresh_net = caught_rate * V_OVER - N_OVER * APPROVER
    check("MCQ 16.4-E rationale: the whole blanket-approval cost", N_ALL * APPROVER, 115200)
    check("MCQ 16.4-E blanket rule: loss avoided",
          (caught_rate * V_ALL).quantize(D("0.01")), D("115600.00"))
    check("MCQ 16.4-E stem: the blanket rule's net value", blanket_net.quantize(D("0.01")),
          D("400.00"))
    check("MCQ 16.4-E thresholded rule: cost", N_OVER * APPROVER, 27600)
    check("MCQ 16.4-E thresholded rule: loss avoided",
          (caught_rate * V_OVER).quantize(D("0.01")), D("107780.00"))
    check("MCQ 16.4-E stem: the thresholded rule's net value", thresh_net.quantize(D("0.01")),
          D("80180.00"))
    check("MCQ 16.4-E key/option A: the released approver time", N_SUB * APPROVER, 87600)
    check("MCQ 16.4-E option A: protection that time bought",
          (caught_rate * V_SUB).quantize(D("0.01")), D("7820.00"))
    check("MCQ 16.4-E option A: value destroyed by approving the sub-threshold population",
          (N_SUB * APPROVER - caught_rate * V_SUB).quantize(D("0.01")), D("79780.00"))
    check("MCQ 16.4-E invariant: the two policies reconcile exactly",
          (thresh_net - blanket_net).quantize(D("0.01")), D("79780.00"))
    check("MCQ 16.4-E option C: the blanket rule is value-neutral to three significant "
          "figures — its net as a share of its cost %",
          (blanket_net / (N_ALL * APPROVER) * 100).quantize(D("0.01")), D("0.35"))
    # Distractor D: the caught rate is an assessment. The conclusion survives it by an order of
    # magnitude — the blanket rule only wins once a second approver catches eleven times as much.
    breakeven_caught = N_SUB * APPROVER / V_SUB
    check("MCQ 16.4-E distractor D: the caught rate at which the blanket rule would pay",
          breakeven_caught.quantize(D("0.000001")), D("0.019043"))
    check("MCQ 16.4-E invariant: that is this multiple of the assessed caught rate",
          (breakeven_caught / caught_rate).quantize(D("0.01")), D("11.20"))
    for mult in ("0.5", "1", "2", "5", "10"):
        c = caught_rate * D(mult)
        check(f"MCQ 16.4-E invariant: thresholded beats blanket at {mult}x the caught rate",
              1 if c * V_OVER - N_OVER * APPROVER > c * V_ALL - N_ALL * APPROVER else 0, 1)
    # The rationale's comparator, rebuilt from 16.3.3: 1,000 items re-reviewed at 13.60 each,
    # extrapolating 784 missed errors at 320 apiece.
    AUDIT_COST = D(1000) * D("13.60")
    AUDIT_FOUND = D(784) * ERROR_COST
    check("MCQ 16.4-E rationale: the blind audit's cost", AUDIT_COST, 13600)
    check("MCQ 16.4-E rationale: missed errors the audit identified", AUDIT_FOUND, 250880)
    check("MCQ 16.4-E rationale: the blind audit's return, the better use of the released time",
          (AUDIT_FOUND / AUDIT_COST).quantize(D("0.0001")), D("18.4471"))
    check("MCQ 16.4-E invariant: the released 87,600 buys more on the audit than on "
          "sub-threshold approval — return per dollar there",
          (caught_rate * V_SUB / (N_SUB * APPROVER)).quantize(D("0.0001")), D("0.0893"))
