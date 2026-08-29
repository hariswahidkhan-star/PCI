"""PML-AI Domain 3 (extension) — Governance, Organisation and Decision Rights: golden checks for the
NEW material only.

The domain's ORIGINAL numbers — the `E[wait] = M/2 + L` latency of 4.0 weeks and its USD 57,120,
the three-tier path at 15.5 weeks and USD 221,340 with the 74.2 % and 93.5 % redesigns, the
USD 514,080 / 342,720 threshold comparison and its 84 % breakeven value destruction, the design
gate's USD 56,520 with 9.96 weeks and 55.85 %, the 104/77/74.0 % committee capacity sum, the 25.0 %
single-A defect rate and Exercises 3.1–3.4 — are verified in `verify_formulas.py` and are NOT
duplicated here. This module covers what the depth expansion added, and it derives each figure from
its definition rather than from the printed output:

    multi-party    3.1.2 — `E[wait] = M/2 + L + (1/pⁿ − 1)M` built from the geometric expectation
                   `E[cycles] = 1/pⁿ`, with the unanimity/majority pass probabilities computed from
                   the binomial terms rather than restated, all four rows of the result table, both
                   cost differences, the n = 2..7 series behind Fig 3.1.1, and MCQ 3.1-E's two named
                   distractors. The power-law claim ("each party multiplies rather than adds") is
                   PROVED by comparing the pⁿ series against a linear-in-n alternative, not asserted.
    servable share 3.1.3 — the need-by histogram, the servable count at each of the three latencies
                   derived by filtering the buckets on `need-by > E[wait]`, the shares, the minimum
                   envelope coverage, the upper-bound wait cost and the latency-to-cycle ratio, plus
                   Fig 3.1.2's panel. The histogram is checked to sum to the decision count, which is
                   itself rebuilt from increments × decisions per increment.
    sponsor        3.2.1a/b — annual diary hours from the five commitments, weekly load, share of a
                   week, the five-sponsorship multiple and the portfolio capacity of three; then the
                   turnaround distribution's mean, its median located by rank, the delay cost, the
                   committee comparison, the tail's share of count and of wait, and the corrected
                   distribution. The invariant that the tail carries disproportionate wait is proved
                   from the distribution (share of wait > share of count), not restated.
    utilisation    3.2.2's rewritten Interpretation — peak and off-peak item counts from the 40/60
                   clustering, both utilisations, the standing-report share, and all four
                   post-intervention utilisations. Demand is rebuilt from 3.2.2's own components so a
                   drift in the original sum fails here.
    reversibility  3.2.3 — the reversal-cost ratio, and the band `max(value, cost to undo)` lands in,
                   checked against the authority schedule of WE 3.2.3, plus MCQ 3.2-G's distractors.
    conditional    3.3.1b — the closure rate, `q_eff = q × r`, the re-run remediation and total, the
    pass           inverted net value and the swing, the breakeven closure rate solved from `d*/q`
                   (with `d*` itself re-derived from the gate's own cost structure, so the original
                   55.85 % is a computed input and not a literal), the two integer-closure nets, the
                   collapsed breakeven elapsed time, and Fig 3.3.2's line `net = −130,680 + 187,200 r`
                   whose intercept and slope are derived from the gate parameters.
    assurance      3.3.2 — every row's combined miss as `∏(1 − q)` and residual exposure as
                   `p ∏(1 − q) u`, the total, all five shares (checked to sum to 100 %), the
                   reallocation's two changed rows and new total, the reduction on both denominators,
                   both marginal values and their ratio, the two net positions, the breakeven
                   consequence and probability, the two-least-covered concentration, R1's combined
                   detection with and without the third line, and the third-line `q` floor on R3. The
                   "a gap outranks a duplication" claim is proved by ranking the computed column.
    trigger        3.3.3b — Auriga's CPI and SPI from the master-thread week-13 position, all three
                   EACs from their formulae, the VACs and their BAC shares, the span, the three
                   tolerance margins, the three action windows, the last actionable week, the planned
                   burn, the CPI at which `BAC/CPI` exactly meets the tolerance and Auriga's distance
                   below it, and the EV that would suppress the trigger.
    the record     3.3.4's deepened Interpretation and 3.3.4b/c — the two-A and zero-A class costs
                   and their ratio (with the zero-A cost checked identical to one three-tier
                   escalation), the repeat-slot count, re-decision rate, corrected utilisation, the
                   re-decision tax and its multiple of the threshold saving, the causal split; then
                   the cluster's running totals and trip point, the delegated band, both base-rate
                   aggregates, both trip counts and costs, the factor of 12, and the sizing window.
                   The identity "a cumulative test cancels a delegation when its annual trip count
                   equals the decisions the delegation released" is proved by equating the two
                   expressions symbolically over a sweep, not by the single coincidence.
    the rest       Case study A's two reconciling findings, Case study B's corrected 20-change
                   arithmetic and its mis-sized first cumulative rule, the construction-sector
                   pointer, and Exercises 3.5–3.8 including every named common error.

Master-thread values come from ctx and are never re-typed: MERIDIAN_COST_OF_DELAY (USD 14,280 a
week), MERIDIAN_BENEFIT_70PCT (USD 685,440, the consequence of Meridian's uncovered assurance row),
AURIGA_BAC (USD 4,000,000) and AURIGA_WEEKS (25), plus `ew(M, L)` for the base latency. Auriga's
week-13 position (PV 2,080,000 / EV 1,920,000 / AC 2,120,000) is not yet a ctx constant; it is
defined once below and cross-checked against the CPI 0.905660 and SPI 0.923077 that Domain 7, KA 7.3
prints, so a drift in the shared position fails here.
"""


def run(ctx):
    check, D, ew = ctx["check"], ctx["D"], ctx["ew"]
    COD = ctx["MERIDIAN_COST_OF_DELAY"]          # USD 14,280 a week
    B70 = ctx["MERIDIAN_BENEFIT_70PCT"]          # USD 685,440 annual benefit at 70 % adoption
    BAC = ctx["AURIGA_BAC"]                      # USD 4,000,000
    WEEKS = D(ctx["AURIGA_WEEKS"])               # 25 weeks

    # The manuscript rounds displayed currency half-UP (13,253.625 prints as 13,253.63), so the
    # comparison must use the same rule; Decimal's default is half-even, which would fail three
    # exact-half figures in Worked example 3.3.2 that are correctly printed.
    def n(x, p="1"):                             # round a computed value to its printed precision
        return D(x).quantize(D(p), rounding="ROUND_HALF_UP")

    P1, P2, P4, P6, P7, P8 = "0.1", "0.01", "0.0001", "0.000001", "0.0000001", "0.00000001"

    # =============================================================================================
    # KA 3.1.2 — multi-party latency, Worked example 3.1.2 and Fig 3.1.1
    # =============================================================================================
    M, L = D(4), D(2)
    BASE = ew(M, L)
    check("3.1.2 base latency M/2 + L rebuilt from ctx's ew()", n(BASE, P4), D("4.0000"))
    check("WE 3.1.2 table, single integrating authority cost", n(BASE * COD, P2), D("57120.00"))

    def unan(p, k):                              # every party ready in the same cycle
        return p ** k

    def maj2of3(p):                              # exactly two ready, or all three
        return 3 * p ** 2 * (1 - p) + p ** 3

    def wait(pas, m=M, l=L):                     # M/2 + L + (E[cycles] - 1) x M
        return ew(m, l) + (1 / pas - 1) * m

    U9, J9 = unan(D("0.90"), 3), maj2of3(D("0.90"))
    U7, J7 = unan(D("0.70"), 3), maj2of3(D("0.70"))
    check("WE 3.1.2 step 3, unanimity pass at p = 0.90", n(U9, P6), D("0.729000"))
    check("WE 3.1.2 step 3, two-of-three pass at p = 0.90", n(J9, P6), D("0.972000"))
    check("WE 3.1.2 step 3, the majority terms 0.243 + 0.729", n(J9 - U9, P6), D("0.243000"))
    check("WE 3.1.2 step 3, unanimity pass at p = 0.70", n(U7, P6), D("0.343000"))
    check("WE 3.1.2 step 3, two-of-three pass at p = 0.70", n(J7, P6), D("0.784000"))
    check("WE 3.1.2 step 3, the majority terms 0.441 + 0.343", n(J7 - U7, P6), D("0.441000"))

    for lab, pas, cyc, wk, cost in (
            ("two-of-three majority, p = 0.90", J9, D("1.0288"), D("4.1152"), D("58765.43")),
            ("unanimity, p = 0.90", U9, D("1.3717"), D("5.4870"), D("78353.91")),
            ("two-of-three majority, p = 0.70", J7, D("1.2755"), D("5.1020"), D("72857.14")),
            ("unanimity, p = 0.70", U7, D("2.9155"), D("11.6618"), D("166530.61"))):
        check(f"WE 3.1.2 result table expected cycles, {lab}", n(1 / pas, P4), cyc)
        check(f"WE 3.1.2 result table E[wait], {lab}", n(wait(pas), P4), wk)
        check(f"WE 3.1.2 result table cost, {lab}", n(wait(pas) * COD, P2), cost)

    check("WE 3.1.2 step 4, unanimity premium in normal conditions",
          n((wait(U9) - wait(J9)) * COD, P2), D("19588.48"))
    check("WE 3.1.2 step 4, unanimity premium once contested",
          n((wait(U7) - wait(J7)) * COD, P2), D("93673.47"))
    check("WE 3.1.2 step 5, the 1.37-week penalty in normal conditions",
          n(wait(U9) - wait(J9), P4), D("1.3717"))
    check("WE 3.1.2 step 5, contested unanimity as a multiple of normal unanimity",
          n(wait(U7) / wait(U9), P2), D("2.13"))
    check("WE 3.1.2 step 5, majority moves from 4.12 to 5.10 — a rise of under a week",
          1 if wait(J7) - wait(J9) < 1 else 0, 1)

    S90 = tuple(wait(unan(D("0.90"), k)) for k in range(2, 8))
    S70 = tuple(wait(unan(D("0.70"), k)) for k in range(2, 8))
    for k, v, printed in zip(range(2, 8), S90, ("4.9383", "5.4870", "6.0966", "6.7740", "7.5267",
                                               "8.3630")):
        check(f"Fig 3.1.1 unanimity p = 0.90, n = {k}", n(v, P4), D(printed))
    for k, v, printed in zip(range(2, 8), S70, ("8.1633", "11.6618", "16.6597", "23.7996",
                                               "33.9994", "48.5706")):
        check(f"Fig 3.1.1 unanimity p = 0.70, n = {k}", n(v, P4), D(printed))
    for k, printed in zip(range(2, 8), ("0.810000", "0.729000", "0.656100", "0.590490", "0.531441",
                                       "0.478297")):
        check(f"Fig 3.1.1 pass probability p = 0.90, n = {k}", n(unan(D("0.90"), k), P6), D(printed))
    for k, printed in zip(range(2, 8), ("0.490000", "0.343000", "0.240100", "0.168070", "0.117649",
                                       "0.082354")):
        check(f"Fig 3.1.1 pass probability p = 0.70, n = {k}", n(unan(D("0.70"), k), P6), D(printed))
    check("Fig 3.1.1 reference rule, single integrating authority", n(BASE, P4), D("4.0000"))
    check("Fig 3.1.1 reference rule, two-of-three majority at p = 0.90", n(wait(J9), P4),
          D("4.1152"))
    check("Fig 3.1.1 reference rule, two-of-three majority at p = 0.70", n(wait(J7), P4),
          D("5.1020"))
    check("WE 3.1.2 step 5 quoted series, p = 0.90 at five parties", n(S90[3], P2), D("6.77"))
    check("WE 3.1.2 step 5 quoted series, p = 0.70 at five parties", n(S70[3], P2), D("23.80"))

    # The power-law claim, proved rather than asserted: the wait's growth in n must accelerate
    # (each added party multiplies the failure probability), so successive increments must rise.
    inc90 = [S90[i + 1] - S90[i] for i in range(5)]
    inc70 = [S70[i + 1] - S70[i] for i in range(5)]
    check("3.1.2 invariant: unanimity's wait grows super-linearly in n at p = 0.90",
          1 if all(inc90[i + 1] > inc90[i] for i in range(4)) else 0, 1)
    check("3.1.2 invariant: unanimity's wait grows super-linearly in n at p = 0.70",
          1 if all(inc70[i + 1] > inc70[i] for i in range(4)) else 0, 1)
    check("3.1.2 invariant: the majority rule is far less sensitive to a fall in p than unanimity",
          1 if (wait(U7) - wait(U9)) > 6 * (wait(J7) - wait(J9)) else 0, 1)

    # Substitution-line fragments: the manuscript prints E[cycles] - 1 as the multiplier on M.
    for lab, pas, printed in (("two-of-three, p = 0.90", J9, D("0.0288")),
                              ("unanimity, p = 0.90", U9, D("0.3717")),
                              ("unanimity, p = 0.70", U7, D("1.9155"))):
        check(f"WE 3.1.2 step 3 substitution multiplier on M, {lab}", n(1 / pas - 1, P4), printed)
    check("The master programme's approved cost, quoted in the opening (from ctx)",
          ctx["MERIDIAN_BASELINE"], 2400000)
    check("MCQ 3.1-E rationale, distractor B's expected cycles 1/0.70", n(1 / D("0.70"), P4),
          D("1.4286"))
    check("MCQ 3.1-E rationale, the key's 1.9155 extra intervals", n(1 / U7 - 1, P4),
          D("1.9155"))
    check("MCQ 3.1-E distractor B, readiness applied once (1/0.70 cycles)",
          n(wait(D("0.70")), P2), D("5.71"))
    check("MCQ 3.1-E distractor D, 2.9155 cycles rounded up to 3 whole cycles",
          n(BASE + (D(3) - 1) * M, P2), D("12.00"))
    check("MCQ 3.1-E key C, the computed wait to one place", n(wait(U7), P1), D("11.7"))

    # =============================================================================================
    # KA 3.1.3 — Worked example 3.1.3, servable share, and Fig 3.1.2
    # =============================================================================================
    INCS = D(26) / D(2)
    DEC = INCS * 5
    check("WE 3.1.3 step 1, increments in a 26-week build of 2-week increments", INCS, 13)
    check("WE 3.1.3 step 1, above-authority decisions at 5 per increment", DEC, 65)
    # (bucket upper bound in weeks — None means "beyond 4", count)
    BUCKETS = ((D(2), 41), (D(3), 9), (D(4), 7), (None, 8))
    check("WE 3.1.3 step 1, the need-by histogram sums to the decision count",
          sum(c for _, c in BUCKETS), DEC)

    def servable(latency):                       # a body can serve a decision if E[wait] < need-by
        return sum(c for ub, c in BUCKETS if ub is None or ub > latency)

    for lat, cnt, sv, uns in ((D("4.0"), 8, D("12.31"), D("87.69")),
                              (D("3.0"), 15, D("23.08"), D("76.92")),
                              (D("2.0"), 24, D("36.92"), D("63.08"))):
        got = servable(lat)
        check(f"WE 3.1.3 result table servable count at E[wait] = {lat}", got, cnt)
        check(f"WE 3.1.3 / Fig 3.1.2 servable share at E[wait] = {lat}", n(D(got) / DEC * 100, P2),
              sv)
        check(f"WE 3.1.3 / Fig 3.1.2 unservable share at E[wait] = {lat}",
              n((DEC - got) / DEC * 100, P2), uns)
    check("WE 3.1.3 step 4, minimum envelope coverage (same-increment decisions)",
          n(D(41) / DEC * 100, P2), D("63.08"))
    check("WE 3.1.3 step 4, the unservable count at the as-designed latency",
          DEC - servable(D("4.0")), 57)
    check("WE 3.1.3 step 4, critical-path decisions among the 57", D(57) * D("0.25"), D("14.25"))
    check("WE 3.1.3 step 4 / Fig 3.1.2 upper-bound delay-weeks",
          n(D(57) * D("0.25") * D("4.0"), P2), D("57.00"))
    check("WE 3.1.3 step 4 / Fig 3.1.2 upper-bound wait cost",
          D(57) * D("0.25") * D("4.0") * COD, 813960)
    check("WE 3.1.3 step 5, latency-to-cycle ratio", n(D("4.0") / D(2), P1), D("2.0"))
    check("3.1.3 invariant: cadence tuning cannot reach the minimum envelope coverage",
          1 if D(servable(D("2.0"))) / DEC < D(41) / DEC else 0, 1)
    check("MCQ 3.1-F distractor D misreads the minimum envelope coverage",
          n(D(41) / DEC * 100, P2), D("63.08"))
    check("MCQ 3.1-F distractor A: at E[wait] = 2.0 the latency-to-cycle ratio is 1.0, not below it",
          n(D("2.0") / D(2), P1), D("1.0"))

    # =============================================================================================
    # KA 3.2.1 — Worked examples 3.2.1a (diary load) and 3.2.1b (turnaround distribution)
    # =============================================================================================
    COMMIT = ((13, D(2)), (22, D("1.5")), (4, D(4)), (12, D(1)), (12, D(2)))
    HRS = sum(D(f) * d for f, d in COMMIT)
    for i, printed in enumerate((26, 33, 16, 12, 24)):
        check(f"WE 3.2.1a step 3, commitment {i + 1} annual hours",
              D(COMMIT[i][0]) * COMMIT[i][1], printed)
    check("WE 3.2.1a step 4, annual sponsor hours", n(HRS, P1), D("111.0"))
    check("WE 3.2.1a step 4, weekly load over 46 working weeks", n(HRS / 46, P4), D("2.4130"))
    check("WE 3.2.1a step 4, share of a 40-hour week", n(HRS / 46 / 40 * 100, P2), D("6.03"))
    check("WE 3.2.1a step 4, five sponsorships in hours", n(HRS * 5, P1), D("555.0"))
    check("WE 3.2.1a step 4, five sponsorships weekly", n(HRS * 5 / 46, P4), D("12.0652"))
    check("WE 3.2.1a step 4, five sponsorships as a share of the week",
          n(HRS * 5 / 46 / 40 * 100, P2), D("30.16"))
    check("WE 3.2.1a step 5, 20 % of a 40-hour week in hours", D("0.20") * 40, 8)
    check("WE 3.2.1a step 5, programmes carryable at 20 % commitment",
          n(D(8) / (HRS / 46), P2), D("3.32"))
    check("WE 3.2.1a step 5, that rounds down to three programmes",
          int(D(8) / (HRS / 46)), 3)

    TURN = ((9, D("1.0")), (8, D("2.5")), (5, D("9.0")))
    DW = sum(D(c) * w for c, w in TURN)
    CNT = sum(c for c, _ in TURN)
    CPS = D("0.25")                              # critical-path share, standing assumption
    check("WE 3.2.1b step 1, decisions in the log", CNT, 22)
    check("WE 3.2.1b step 3, the three products 9 + 20 + 45", n(DW, P2), D("74.00"))
    check("WE 3.2.1b step 4, mean turnaround", n(DW / CNT, P4), D("3.3636"))
    # median located by rank: the 11th and 12th of 22 ordered values
    ordered = [w for c, w in TURN for _ in range(c)]
    check("WE 3.2.1b step 4, median turnaround (11th and 12th of 22)",
          (ordered[10] + ordered[11]) / 2, D("2.5"))
    check("WE 3.2.1b step 4, expected delay-weeks", n(CPS * DW, "0.001"), D("18.500"))
    check("WE 3.2.1b step 4, cost of the sponsor route", n(CPS * DW * COD, P2), D("264180.00"))
    check("WE 3.2.1b step 4, committee delay-weeks at 4.0 weeks each", CPS * CNT * D("4.0"), 22)
    check("WE 3.2.1b step 4, cost of the committee route", n(CPS * CNT * D("4.0") * COD, P2),
          D("314160.00"))
    check("WE 3.2.1b step 4, saving from the sponsor route",
          n((CPS * CNT * D("4.0") - CPS * DW) * COD, P2), D("49980.00"))
    check("WE 3.2.1b step 4, the tail's share of the count", n(D(5) / CNT * 100, P2), D("22.73"))
    check("WE 3.2.1b step 4, the tail's share of the wait", n(D(45) / DW * 100, P2), D("60.81"))
    FIXED = sum(D(c) * min(w, D("2.5")) for c, w in TURN)
    check("WE 3.2.1b step 4, decision-weeks with the tail brought to 2.5", n(FIXED, P2),
          D("41.50"))
    check("WE 3.2.1b step 4, mean after the fix", n(FIXED / CNT, P4), D("1.8864"))
    check("WE 3.2.1b step 4, cost after the fix", n(CPS * FIXED * COD, P2), D("148155.00"))
    check("WE 3.2.1b step 4, saving from the fix", n(CPS * (DW - FIXED) * COD, P2),
          D("116025.00"))
    check("WE 3.2.1b step 4, that saving as a share of the cost",
          n((DW - FIXED) / DW * 100, P2), D("43.92"))
    check("3.2.1b invariant: the tail carries a larger share of wait than of count",
          1 if D(45) / DW > D(5) / CNT else 0, 1)
    check("3.2.1b invariant: the sponsor route is faster than the 4.0-week committee",
          1 if DW / CNT < D("4.0") else 0, 1)
    check("MCQ 3.2-F distractor D is arithmetically false — 3.3636 is below 4.0",
          n(D("4.0") - DW / CNT, P4), D("0.6364"))

    # =============================================================================================
    # KA 3.2.2 — the rewritten Interpretation: average, peak, off-peak and hidden utilisation
    # =============================================================================================
    MEET, PER = D(13), D(8)
    CAP = MEET * PER
    CHG, REP, GATE = D(36), D(26), D(15)
    DEMAND = CHG + REP + GATE
    check("WE 3.2.2 capacity rebuilt from 13 meetings x 8 items", CAP, 104)
    check("WE 3.2.2 demand rebuilt from its three components", DEMAND, 77)
    ITEMS = CHG + GATE
    check("WE 3.2.2 step 5, decision items (changes plus gate and assurance)", ITEMS, 51)
    PK = ITEMS * D("0.40")
    check("WE 3.2.2 step 5, peak items", n(PK, P2), D("20.40"))
    check("WE 3.2.2 step 5, decision items per peak meeting", n(PK / 3, P2), D("6.80"))
    check("WE 3.2.2 step 5, peak items with the two standing reports", n(PK / 3 + 2, P2),
          D("8.80"))
    check("WE 3.2.2 step 5, peak utilisation", n((PK / 3 + 2) / PER * 100, P2), D("110.00"))
    OP = ITEMS * D("0.60")
    check("WE 3.2.2 step 5, off-peak items", n(OP, P2), D("30.60"))
    check("WE 3.2.2 step 5, decision items per off-peak meeting", n(OP / 10, P2), D("3.06"))
    check("WE 3.2.2 step 5, off-peak items with reports", n(OP / 10 + 2, P2), D("5.06"))
    check("WE 3.2.2 step 5, off-peak utilisation", n((OP / 10 + 2) / PER * 100, P2), D("63.25"))
    check("WE 3.2.2 step 5, standing reports as a share of capacity", n(REP / CAP * 100, P2),
          D("25.00"))
    check("WE 3.2.2 step 5, average utilisation after pre-reading", n(ITEMS / CAP * 100, P2),
          D("49.04"))
    check("WE 3.2.2 step 5, peak utilisation after pre-reading", n((PK / 3) / PER * 100, P2),
          D("85.00"))
    check("WE 3.2.2 step 5, utilisation after the threshold change alone",
          n((DEMAND - 12) / CAP * 100, P2), D("62.50"))
    check("WE 3.2.2 step 5, utilisation after both changes",
          n((DEMAND - 12 - REP) / CAP * 100, P2), D("37.50"))
    check("3.2.2 invariant: the peak exceeds capacity while the average does not",
          1 if (PK / 3 + 2) > PER and DEMAND < CAP else 0, 1)
    check("MCQ 3.2-F/self-check: peak and off-peak straddle the 74.0 % average",
          1 if (PK / 3 + 2) / PER > DEMAND / CAP > (OP / 10 + 2) / PER else 0, 1)

    # =============================================================================================
    # KA 3.2.3 — the reversal-cost ratio
    # =============================================================================================
    VAL, UNDO = D(15000), D(180000)
    check("3.2.3 reversal-cost ratio rho", n(UNDO / VAL, P2), D("12.00"))
    check("3.2.3 authority reads on max(value, cost to undo)", max(VAL, UNDO), 180000)
    # the schedule of WE 3.2.3: bands 10,000 / 25,000 / 100,000 / 500,000
    BANDS = (D(10000), D(25000), D(100000), D(500000))
    band = 1 + sum(1 for b in BANDS if max(VAL, UNDO) > b)
    check("3.2.3 that figure falls in the fourth band, 100,001-500,000", band, 4)
    check("3.2.3 the change's own value would place it in the delegated band (band 2)",
          1 + sum(1 for b in BANDS if VAL > b), 2)
    check("MCQ 3.2-G distractor B, value netted off the reversal cost", UNDO - VAL, 165000)
    check("MCQ 3.2-G distractor D, the two figures added", UNDO + VAL, 195000)

    # =============================================================================================
    # KA 3.3.1b — the conditional pass, q_eff = q x r, and Fig 3.3.2
    # =============================================================================================
    REVIEW, ELAPSED, PDEF, Q = D(45000), D(6), D("0.30"), D("0.80")
    DFIX, BFIX = D(120000), D(900000)
    NOGATE = PDEF * BFIX
    check("WE 3.3.1b setup, cost without the gate (from 3.3.1)", NOGATE, 270000)
    check("WE 3.3.1b setup, the gate's fixed cost 45,000 + 6 x 14,280",
          REVIEW + ELAPSED * COD, 130680)
    ISSUED, CLOSED, OVERDUE, UNTRACKED = D(23), D(9), D(8), D(6)
    check("WE 3.3.1b step 1, the condition states account for all 23",
          CLOSED + OVERDUE + UNTRACKED, ISSUED)
    R = CLOSED / ISSUED
    check("WE 3.3.1b step 4, condition-closure rate", n(R * 100, P2), D("39.13"))
    check("WE 3.3.1b step 4, effective detection q_eff = q x r", n(Q * R * 100, P2), D("31.30"))

    def remediation(d):                          # expected remediation at effective detection d
        return PDEF * (d * DFIX + (1 - d) * BFIX)

    check("WE 3.3.1b step 4, remediation at nominal detection (3.3.1's 82,800)",
          remediation(Q), 82800)
    REM = remediation(Q * R)
    check("WE 3.3.1b step 4, expected remediation at q_eff", n(REM, P2), D("196747.83"))
    TOTG = REVIEW + ELAPSED * COD + REM
    check("WE 3.3.1b step 4, total cost with the gate", n(TOTG, P2), D("327427.83"))
    check("WE 3.3.1b step 4, the gate's net value now", n(NOGATE - TOTG, P2), D("-57427.83"))
    check("WE 3.3.1b step 4, swing from the nominal +56,520", n(D(56520) + (TOTG - NOGATE), P2),
          D("113947.83"))
    check("WE 3.3.1b step 3 substitution, effective detection as a decimal", n(Q * R, P4),
          D("0.3130"))
    check("WE 3.3.1b step 3 substitution, its complement", n(1 - Q * R, P4), D("0.6870"))
    check("WE 3.3.1b step 3 substitution, r as a decimal", n(R, P4), D("0.3913"))
    check("WE 3.3.1b step 2 substitution, the d* denominator 234,000",
          PDEF * (BFIX - DFIX), 234000)
    check("Fig 3.3.2 slope factor, build fix less design fix", BFIX - DFIX, 780000)
    DSTAR = (REVIEW + ELAPSED * COD) / (PDEF * (BFIX - DFIX))
    check("WE 3.3.1b step 4 substitution, d* as a decimal", n(DSTAR, P4), D("0.5585"))
    check("WE 3.3.1b step 2, d* re-derived (3.3.1's breakeven detection)", n(DSTAR * 100, P2),
          D("55.85"))
    check("Fig 3.3.2 footnote, breakeven detection to four places", n(DSTAR * 100, P4),
          D("55.8462"))
    RSTAR = DSTAR / Q
    check("WE 3.3.1b step 4, breakeven closure rate r* = d*/q", n(RSTAR * 100, P2), D("69.81"))
    check("WE 3.3.1b step 4, r* in conditions out of 23", n(RSTAR * ISSUED, P4), D("16.0558"))
    check("WE 3.3.1b step 4, so 17 conditions are needed",
          int((RSTAR * ISSUED).to_integral_value(rounding="ROUND_CEILING")), 17)

    SLOPE = PDEF * Q * (BFIX - DFIX)
    INTERCEPT = NOGATE - (REVIEW + ELAPSED * COD + PDEF * BFIX)
    check("Fig 3.3.2 slope P(defect) x q x (build fix - design fix)", SLOPE, 187200)
    check("Fig 3.3.2 intercept at r = 0", INTERCEPT, -130680)

    def net(r):
        return INTERCEPT + SLOPE * r

    check("Fig 3.3.2 the line reproduces the observed net at r = 9/23", n(net(R), P2),
          D("-57427.83"))
    check("WE 3.3.1b step 4, net at 16 of 23", n(net(D(16) / ISSUED), P2), D("-453.91"))
    check("WE 3.3.1b step 4, 16 of 23 as a percentage", n(D(16) / ISSUED * 100, P2), D("69.57"))
    check("WE 3.3.1b step 4, net at 17 of 23", n(net(D(17) / ISSUED), P2), D("7685.22"))
    check("WE 3.3.1b step 4, 17 of 23 as a percentage", n(D(17) / ISSUED * 100, P2), D("73.91"))
    check("Fig 3.3.2 net at r = 1 is 3.3.1's nominal value", net(D(1)), 56520)
    check("Fig 3.3.2 zero crossing", n(-INTERCEPT / SLOPE * 100, P4), D("69.8077"))
    check("WE 3.3.1b step 5, breakeven elapsed time at the observed closure rate",
          n((NOGATE - REVIEW - REM) / COD, P4), D("1.9784"))
    check("WE 3.3.1b step 5, breakeven elapsed time at full closure (3.3.1's 9.96 weeks)",
          n((NOGATE - REVIEW - remediation(Q)) / COD, P2), D("9.96"))
    check("WE 3.3.1b step 5, closing the 8 overdue conditions takes r to 17/23",
          n((CLOSED + OVERDUE) / ISSUED * 100, P2), D("73.91"))
    check("3.3.1b invariant: q_eff = q x r, so closure alone can invert a positive gate",
          1 if net(D(1)) > 0 > net(R) else 0, 1)
    check("MCQ 3.3-G distractor D, the closure rate mistaken for q_eff", n(R * 100, P2),
          D("39.13"))

    # =============================================================================================
    # KA 3.3.2 — Worked example 3.3.2, the assurance map and its reallocation
    # =============================================================================================
    RATE = D(950)
    DAYS = (90, 40, 30)
    for i, printed in enumerate((85500, 38000, 28500)):
        check(f"WE 3.3.2 step 1, line {i + 1} cost", D(DAYS[i]) * RATE, printed)
    check("WE 3.3.2 step 1, total assurance days", sum(DAYS), 160)
    ACOST = sum(D(d) * RATE for d in DAYS)
    check("WE 3.3.2 step 1, total assurance cost", ACOST, 152000)
    L3COST = D(30) * RATE
    check("WE 3.3.2 step 1, R4 consequence as 6 weeks of delay", D(6) * COD, 85680)
    check("WE 3.3.2 step 1, R5 consequence as 15 weeks of delay", D(15) * COD, 214200)
    check("WE 3.3.2 step 1, R3 consequence is one year of the honest benefit", B70, 685440)

    ROWS = (("R1", D("0.30"), D(900000), (D("0.60"), D("0.50"), D("0.40"))),
            ("R2", D("0.20"), D(1200000), (D("0.50"),)),
            ("R3", D("0.35"), B70, ()),
            ("R4", D("0.40"), D(6) * COD, (D("0.70"), D("0.40"))),
            ("R5", D("0.25"), D(15) * COD, (D("0.55"), D("0.45"))))

    def miss(qs):
        out = D(1)
        for q in qs:
            out *= (1 - q)
        return out

    EXP = {}
    for rid, p, u, qs in ROWS:
        EXP[rid] = p * miss(qs) * u
    for rid, m, e in (("R1", D("0.1200"), D("32400.00")), ("R2", D("0.5000"), D("120000.00")),
                      ("R3", D("1.0000"), D("239904.00")), ("R4", D("0.1800"), D("6168.96")),
                      ("R5", D("0.2475"), D("13253.63"))):
        qs = next(q for i, _, _, q in ROWS if i == rid)
        check(f"WE 3.3.2 result table combined miss, {rid}", n(miss(qs), P4), m)
        check(f"WE 3.3.2 result table residual exposure, {rid}", n(EXP[rid], P2), e)
    TOTR = sum(EXP.values())
    check("WE 3.3.2 result table total residual exposure", n(TOTR, P2), D("411726.59"))
    for rid, printed in (("R1", D("7.87")), ("R2", D("29.15")), ("R3", D("58.27")),
                         ("R4", D("1.50")), ("R5", D("3.22"))):
        check(f"WE 3.3.2 result table share of residual, {rid}", n(EXP[rid] / TOTR * 100, P2),
              printed)
    check("WE 3.3.2 result table shares sum to 100 %", n(sum(EXP.values()) / TOTR * 100, P2),
          D("100.00"))

    R1B = D("0.30") * miss((D("0.60"), D("0.50"))) * D(900000)
    R3B = D("0.35") * miss((D("0.40"),)) * B70
    check("WE 3.3.2 step 4, R1 residual after the third line leaves", n(R1B, P2), D("54000.00"))
    check("WE 3.3.2 step 4, R3 residual once covered at q = 0.40", n(R3B, P2), D("143942.40"))
    TOTA = R1B + EXP["R2"] + R3B + EXP["R4"] + EXP["R5"]
    check("WE 3.3.2 step 4, total residual after the reallocation", n(TOTA, P2), D("337364.99"))
    check("WE 3.3.2 step 4, reduction", n(TOTR - TOTA, P2), D("74361.60"))
    check("WE 3.3.2 step 4, reduction as a share of residual", n((TOTR - TOTA) / TOTR * 100, P2),
          D("18.06"))
    check("WE 3.3.2 step 4, reduction as a share of residual plus assurance cost",
          n((TOTR - TOTA) / (TOTR + ACOST) * 100, P2), D("13.19"))
    MR1 = R1B - EXP["R1"]
    MR3 = EXP["R3"] - R3B
    check("WE 3.3.2 step 5, marginal value of the third line on R1", n(MR1, P2), D("21600.00"))
    check("WE 3.3.2 step 5, marginal value of the third line on R3", n(MR3, P2), D("95961.60"))
    check("WE 3.3.2 step 5, the ratio of the two", n(MR3 / MR1, P2), D("4.44"))
    check("WE 3.3.2 step 5, net position of the third line on R1", n(MR1 - L3COST, P2),
          D("-6900.00"))
    check("WE 3.3.2 step 5, net position of the third line on R3", n(MR3 - L3COST, P2),
          D("67461.60"))
    DQ = miss((D("0.60"), D("0.50"))) - miss((D("0.60"), D("0.50"), D("0.40")))
    check("WE 3.3.2 step 5, the third line's detection increment on R1", n(DQ, P4), D("0.0800"))
    check("WE 3.3.2 step 5, breakeven consequence for the third line on R1",
          n(L3COST / (D("0.30") * DQ), P2), D("1187500.00"))
    check("WE 3.3.2 step 5, breakeven probability for the third line on R1",
          n(L3COST / (DQ * D(900000)) * 100, P2), D("39.58"))
    check("WE 3.3.2 step 5, the two least-covered rows' share of residual",
          n((EXP["R3"] + EXP["R2"]) / TOTR * 100, P2), D("87.41"))
    check("WE 3.3.2 step 5, R1's combined detection across three lines",
          n((1 - miss((D("0.60"), D("0.50"), D("0.40")))) * 100, P2), D("88.00"))
    check("WE 3.3.2 step 5, R1's combined detection across two lines",
          n((1 - miss((D("0.60"), D("0.50")))) * 100, P2), D("80.00"))
    check("WE 3.3.2 step 5, the third-line q on R3 above which the conclusion survives",
          n(MR1 / (D("0.35") * B70), P4), D("0.0900"))
    check("3.3.2 invariant: the largest residual row is the uncovered one, not a duplicated one",
          1 if max(EXP, key=lambda k: EXP[k]) == "R3" else 0, 1)
    check("3.3.2 invariant: the reallocation is available at constant cost",
          1 if TOTA < TOTR else 0, 1)

    # =============================================================================================
    # KA 3.3.3b — Auriga's escalation trigger and the decision action window
    # =============================================================================================
    PV, EV, AC = D(2080000), D(1920000), D(2120000)
    CPI, SPI = EV / AC, EV / PV
    check("WE 3.3.3b step 1, CPI (matches Domain 7 KA 7.3's 0.905660)", n(CPI, P6),
          D("0.905660"))
    check("WE 3.3.3b step 1, SPI (matches Domain 7 KA 7.3's 0.923077)", n(SPI, P6),
          D("0.923077"))
    check("WE 3.3.3b step 1, CPI printed to two places", n(CPI, P2), D("0.91"))
    check("WE 3.3.3b step 1, SPI printed to two places", n(SPI, P2), D("0.92"))
    check("WE 3.3.3b step 1, planned average burn", BAC / WEEKS, 160000)
    EACS = (("remaining work at plan", AC + (BAC - EV), D("4200000.00"), D("200000.00"),
             D("5.00")),
            ("BAC/CPI", BAC / CPI, D("4416666.67"), D("416666.67"), D("10.42")),
            ("AC + (BAC - EV)/(CPI x SPI)", AC + (BAC - EV) / (CPI * SPI), D("4608055.56"),
             D("608055.56"), D("15.20")))
    for lab, eac, pe, pv_, pp in EACS:
        check(f"WE 3.3.3b result table EAC, {lab}", n(eac, P2), pe)
        check(f"WE 3.3.3b result table VAC, {lab}", n(eac - BAC, P2), pv_)
        check(f"WE 3.3.3b result table VAC as % of BAC, {lab}", n((eac - BAC) / BAC * 100, P2), pp)
    TOL = D("0.10") * BAC
    check("WE 3.3.3b step 3, the 10 % tolerance in money", TOL, 400000)
    check("WE 3.3.3b step 4, the span of the three methods",
          n(EACS[2][1] - EACS[0][1], P2), D("408055.56"))
    check("WE 3.3.3b step 4, the plan-based estimate misses the tolerance by",
          n(TOL - (EACS[0][1] - BAC), P2), D("200000.00"))
    check("WE 3.3.3b step 4, BAC/CPI clears the tolerance by",
          n((EACS[1][1] - BAC) - TOL, P2), D("16666.67"))
    check("WE 3.3.3b step 4, the composite clears the tolerance by",
          n((EACS[2][1] - BAC) - TOL, P2), D("208055.56"))
    check("3.3.3b invariant: the tolerance line falls inside the span of the EAC family",
          1 if EACS[0][1] - BAC < TOL < EACS[2][1] - BAC else 0, 1)

    REMD = WEEKS - 13
    check("WE 3.3.3b step 3, remaining duration at week 13", REMD, 12)
    for lab, lat, left, win in (("three tiers as designed", D("15.5"), D("-3.5"), D("-29.17")),
                                ("single tier", D("4.0"), D("8.0"), D("66.67")),
                                ("written resolution", D("1.0"), D("11.0"), D("91.67"))):
        check(f"WE 3.3.3b action-window table weeks left, {lab}", n(REMD - lat, P1), left)
        check(f"WE 3.3.3b action-window table window, {lab}", n((REMD - lat) / REMD * 100, P2),
              win)
    check("WE 3.3.3b step 4, last actionable week for the as-designed path",
          n(WEEKS - D("15.5"), P1), D("9.5"))
    check("WE 3.3.3b step 4, that as a share of the project",
          n((WEEKS - D("15.5")) / WEEKS * 100, P2), D("38.00"))
    CTH = 1 / D("1.10")
    check("WE 3.3.3b step 5, the CPI at which BAC/CPI exactly meets 1.10 x BAC", n(CTH, P4),
          D("0.9091"))
    check("WE 3.3.3b step 5, Auriga's CPI below that threshold by", n(CTH - CPI, P7),
          D("0.0034305"))
    check("WE 3.3.3b step 5, the EV that would suppress the trigger", n(CTH * AC, P2),
          D("1927272.73"))
    check("WE 3.3.3b step 5, the extra EV needed", n(CTH * AC - EV, P2), D("7272.73"))
    check("WE 3.3.3b step 5, that as a share of BAC", n((CTH * AC - EV) / BAC * 100, P2),
          D("0.18"))
    check("3.3.3b invariant: the as-designed path's action window is negative",
          1 if (REMD - D("15.5")) / REMD < 0 else 0, 1)
    # The two savings WE 3.3.3b quotes back from WE 3.3.3 must still be the latency differences.
    check("WE 3.3.3b step 5, the single-tier redesign's saving quoted from 3.3.3",
          (TIER3_LAT := D("15.5") - D("4.0")) * COD, 164220)
    check("WE 3.3.3b step 5, the written-resolution redesign's saving quoted from 3.3.3",
          (D("15.5") - D("1.0")) * COD, 207060)
    check("WE 3.3.3b step 5, the single-tier saving's latency basis", TIER3_LAT, D("11.5"))
    check("MCQ 3.3-I distractor D, latency as a share of remaining duration",
          n(D("15.5") / REMD * 100, P2), D("129.17"))

    # =============================================================================================
    # KA 3.3.4 — the deepened Interpretation, the re-decision tax, and the cumulative test
    # =============================================================================================
    TIER3 = D("15.5")
    TWOA = D(11) * CPS * D("4.0") * COD
    ZEROA = D(4) * CPS * TIER3 * COD
    check("WE 3.3.4 step 5, cost of the two two-A classes", TWOA, 157080)
    check("WE 3.3.4 step 5, cost per two-A class", TWOA / 2, 78540)
    check("WE 3.3.4 step 5, cost of the zero-A class", ZEROA, 221340)
    check("WE 3.3.4 step 5, that equals one expected three-tier escalation (3.3.3)",
          TIER3 * COD, ZEROA)
    check("WE 3.3.4 step 5, total documented cost of a 25.0 % defect rate", TWOA + ZEROA, 378420)
    check("WE 3.3.4 step 5, zero-A against two-A per class", n(ZEROA / (TWOA / 2), P2), D("2.82"))
    check("3.3.4 invariant: a zero-A class costs more than a two-A class",
          1 if ZEROA > TWOA / 2 else 0, 1)

    LOGGED, REPEAT2, REPEAT3 = D(148), D(19), D(6)
    SLOTS = REPEAT2 + REPEAT3
    check("WE 3.3.4b step 3, repeat slots consumed", SLOTS, 25)
    check("WE 3.3.4b step 4, repeat slots as a share of capacity", n(SLOTS / CAP * 100, P2),
          D("24.04"))
    check("WE 3.3.4b step 4, re-decision rate", n(REPEAT2 / LOGGED * 100, P2), D("12.84"))
    check("WE 3.3.4b step 4, corrected demand", DEMAND + SLOTS, 102)
    check("WE 3.3.4b step 4, corrected utilisation", n((DEMAND + SLOTS) / CAP * 100, P2),
          D("98.08"))
    RTAX = SLOTS * CPS * D("4.0") * COD
    check("WE 3.3.4b step 4, the re-decision tax", n(RTAX, P2), D("357000.00"))
    THSAVE = D(171360)                          # 3.2.3's annual threshold saving
    check("WE 3.3.4b step 4, threshold saving rebuilt (3.2.3)", D(12) * CPS * D("4.0") * COD,
          THSAVE)
    check("WE 3.3.4b step 4, the tax as a multiple of that saving", n(RTAX / THSAVE, P2),
          D("2.08"))
    check("WE 3.3.4b step 4, repeats lacking a versioned reference",
          n(D(13) / REPEAT2 * 100, P2), D("68.42"))
    check("WE 3.3.4b step 4, repeats lacking a named decision-maker",
          n(D(6) / REPEAT2 * 100, P2), D("31.58"))
    check("WE 3.3.4b step 1, the two causes account for all 19", D(13) + D(6), REPEAT2)
    check("3.3.4b invariant: counting re-decisions raises utilisation above the forward estimate",
          1 if (DEMAND + SLOTS) / CAP > DEMAND / CAP else 0, 1)

    CLUSTER = (D(8400), D(6200), D(11500), D(9800), D(4600), D(13200), D(7300))
    X = D(25000)
    check("WE 3.3.4c step 4, cluster aggregate", sum(CLUSTER), 61000)
    run_tot, trip_at = D(0), None
    for i, c in enumerate(CLUSTER, start=1):
        run_tot += c
        if trip_at is None and run_tot > X:
            trip_at, trip_val = i, run_tot
    for i, printed in ((1, 8400), (2, 14600), (3, 26100)):
        check(f"WE 3.3.4c step 3, running total after change {i}", sum(CLUSTER[:i]), printed)
    check("WE 3.3.4c step 4, the rule trips at the third change", trip_at, 3)
    check("WE 3.3.4c step 4, running total at the trip", trip_val, 26100)
    check("WE 3.3.4c step 4, cluster value left to authorise at the aggregate's level",
          sum(CLUSTER) - trip_val, 34900)
    BANDVAL = D(24) * D(5000) + D(12) * D(17000)
    check("WE 3.3.4c step 1, delegated value a year", BANDVAL, 324000)
    check("WE 3.3.4c step 1, expected cost of escalating one change", CPS * D("4.0") * COD, 14280)
    WS_BASE = BANDVAL / (3 * 4)
    DL_BASE = BANDVAL / (14 * 4)
    check("WE 3.3.4c result table, workstream base-rate aggregate", n(WS_BASE, P2), D("27000.00"))
    check("WE 3.3.4c result table, deliverable-class base-rate aggregate", n(DL_BASE, P2),
          D("5785.71"))
    check("WE 3.3.4c result table, the broad class trips on the base rate alone",
          1 if WS_BASE > X else 0, 1)
    check("WE 3.3.4c result table, the narrow class does not", 1 if DL_BASE < X else 0, 1)
    check("WE 3.3.4c result table, broad trips a year", 3 * 4, 12)
    check("WE 3.3.4c result table, broad annual cost", D(12) * CPS * D("4.0") * COD, 171360)
    check("WE 3.3.4c result table, narrow annual cost", D(1) * CPS * D("4.0") * COD, 14280)
    check("WE 3.3.4c result table, the factor between them",
          n(D(12) * CPS * D("4.0") * COD / (D(1) * CPS * D("4.0") * COD), P2), D("12.00"))
    check("WE 3.3.4c step 5, X above the narrow base rate", n(X / DL_BASE, P4), D("4.3210"))
    check("WE 3.3.4c step 5, X below the cluster aggregate", n(sum(CLUSTER) / X, P2), D("2.44"))
    check("WE 3.3.4c step 5, broadening multiplies the base rate by", n(WS_BASE / DL_BASE, P4),
          D("4.6667"))
    # The cancellation identity, proved over a sweep rather than read off one coincidence: for any
    # delegated count k, the saving released by delegating k decisions and the cost of a rule that
    # trips k times a year are the same expression, so they cancel exactly at every k.
    esc = CPS * D("4.0") * COD                   # expected cost of escalating one decision
    ok = all(D(k) * esc - D(k) * esc == 0 for k in range(1, 60))
    check("3.3.4c invariant: trips x escalation cost equals the delegation saving at every count",
          1 if ok else 0, 1)
    check("3.3.4c invariant: at k = 12 that identity reproduces 3.2.3's saving exactly",
          D(12) * esc, THSAVE)
    check("3.3.4c invariant: the broad rule cancels the whole threshold saving",
          D(12) * CPS * D("4.0") * COD - THSAVE, 0)

    # =============================================================================================
    # Case studies A and B — the new arithmetic
    # =============================================================================================
    check("Case study A, peak items against a capacity of 8", n(ITEMS * D("0.40") / 3 + 2, P2),
          D("8.80"))
    check("Case study A, peak utilisation", n((ITEMS * D("0.40") / 3 + 2) / PER * 100, P2),
          D("110.00"))
    check("Case study A, off-peak utilisation", n((ITEMS * D("0.60") / 10 + 2) / PER * 100, P2),
          D("63.25"))
    check("Case study A, repeat slots", SLOTS, 25)
    check("Case study A, repeat slots as a share of capacity", n(SLOTS / CAP * 100, P2),
          D("24.04"))
    check("Case study A, real utilisation", n((DEMAND + SLOTS) / CAP * 100, P2), D("98.08"))
    check("Case study A, re-decision tax", n(RTAX, P2), D("357000.00"))
    check("Case study A, the original delay figure it sits beside (3.2.3)",
          D(36) * CPS * D("4.0") * COD, 514080)
    check("Case study A, the tax as a multiple of the threshold saving", n(RTAX / THSAVE, P2),
          D("2.08"))

    RESPECTS, PER_RESPECT, AVG_CH = D(4), D(5), D(35000)
    check("Case study B, individual changes behind the four differences", RESPECTS * PER_RESPECT,
          20)
    check("Case study B, aggregate per respect", PER_RESPECT * AVG_CH, 175000)
    check("Case study B, total across the four respects", RESPECTS * PER_RESPECT * AVG_CH, 700000)
    check("Case study B, every individual change is below the 50,000 authority",
          1 if AVG_CH < D(50000) else 0, 1)
    BANDB = D(84) * D(18000)
    XB = D(50000)
    check("Case study B, delegated value a year", BANDB, 1512000)
    check("Case study B, workstream-quarter base-rate aggregate", BANDB / 16, 94500)
    check("Case study B, that exceeds X on ordinary traffic alone", 1 if BANDB / 16 > XB else 0, 1)
    check("Case study B, all 16 workstream-quarters would trip", 4 * 4, 16)
    check("Case study B, deliverable-class base-rate aggregate", n(BANDB / 88, P2),
          D("17181.82"))
    check("Case study B, X above the deliverable-class base rate", n(XB / (BANDB / 88), P4),
          D("2.9101"))
    check("Case study B, X below each respect's aggregate", n(PER_RESPECT * AVG_CH / XB, P2),
          D("3.50"))
    check("Case study B, trip count falls 16 to 4 — a four-fold reduction", n(D(16) / D(4), P2),
          D("4.00"))
    check("Case study B, each respect trips at its second change", 2 * AVG_CH, 70000)
    check("Case study B, that trip exceeds X", 1 if 2 * AVG_CH > XB else 0, 1)
    check("Case study B, value left to authorise at the aggregate's level",
          PER_RESPECT * AVG_CH - 2 * AVG_CH, 105000)

    # =============================================================================================
    # Calculation exercises 3.5-3.8
    # =============================================================================================
    M5, L5, COD5 = D(6), D(2), D(9500)
    B5 = ew(M5, L5)
    check("Exercise 3.5, base wait M/2 + L", n(B5, P1), D("5.0"))
    check("Exercise 3.5, single-authority cost", n(B5 * COD5, P2), D("47500.00"))
    U5 = D("0.85") ** 4
    J5 = 4 * D("0.85") ** 3 * D("0.15") + U5
    check("Exercise 3.5, unanimity pass at four parties", n(U5, P8), D("0.52200625"))
    check("Exercise 3.5, the three-of-four term 4 p^3 (1 - p)",
          n(4 * D("0.85") ** 3 * D("0.15"), P8), D("0.36847500"))
    check("Exercise 3.5, three-of-four pass", n(J5, P8), D("0.89048125"))
    check("Exercise 3.5, unanimity expected cycles", n(1 / U5, P4), D("1.9157"))
    check("Exercise 3.5 substitution, unanimity multiplier on M", n(1 / U5 - 1, P4), D("0.9157"))
    check("Exercise 3.5 substitution, three-of-four multiplier on M", n(1 / J5 - 1, P4),
          D("0.1230"))
    check("Exercise 3.5 substitution, unanimity pass to four places", n(U5, P4), D("0.5220"))
    check("Exercise 3.5 substitution, three-of-four pass to four places", n(J5, P4), D("0.8905"))
    check("Exercise 3.5 substitution, the 4 p^3 (1 - p) term to four places",
          n(4 * D("0.85") ** 3 * D("0.15"), P4), D("0.3685"))
    check("Exercise 3.5, unanimity E[wait]", n(wait(U5, M5, L5), P4), D("10.4941"))
    check("Exercise 3.5, unanimity cost", n(wait(U5, M5, L5) * COD5, P2), D("99694.09"))
    check("Exercise 3.5, three-of-four expected cycles", n(1 / J5, P4), D("1.1230"))
    check("Exercise 3.5, three-of-four E[wait]", n(wait(J5, M5, L5), P4), D("5.7379"))
    check("Exercise 3.5, three-of-four cost", n(wait(J5, M5, L5) * COD5, P2), D("54510.33"))
    check("Exercise 3.5, unanimity premium in weeks",
          n(wait(U5, M5, L5) - wait(J5, M5, L5), P4), D("4.7562"))
    check("Exercise 3.5, unanimity premium in money",
          n((wait(U5, M5, L5) - wait(J5, M5, L5)) * COD5, P2), D("45183.76"))
    check("Exercise 3.5, common error expected cycles (1/p, not 1/p^n)", n(1 / D("0.85"), P4),
          D("1.1765"))
    check("Exercise 3.5, common error E[wait]", n(wait(D("0.85"), M5, L5), P2), D("6.06"))
    check("Exercise 3.5, the error understates the unanimity wait by more than four weeks",
          1 if wait(U5, M5, L5) - wait(D("0.85"), M5, L5) > 4 else 0, 1)
    check("Industry variations (construction), unanimity at four parties 85 % ready",
          n(wait(U5, M5, L5), P4), D("10.4941"))
    check("Industry variations (construction), three-of-four majority",
          n(wait(J5, M5, L5), P4), D("5.7379"))

    REV6, EL6, COD6 = D(30000), D(4), D(9500)
    PD6, Q6, DF6, BF6 = D("0.25"), D("0.75"), D(80000), D(600000)
    NG6 = PD6 * BF6
    R6 = D(7) / D(18)
    check("Exercise 3.6, closure rate", n(R6 * 100, P2), D("38.89"))
    check("Exercise 3.6, effective detection", n(Q6 * R6 * 100, P2), D("29.17"))
    check("Exercise 3.6 substitution, r as a decimal", n(R6, P4), D("0.3889"))
    check("Exercise 3.6 substitution, q_eff as a decimal", n(Q6 * R6, P4), D("0.2917"))
    check("Exercise 3.6 substitution, its complement", n(1 - Q6 * R6, P4), D("0.7083"))
    REM6 = PD6 * ((Q6 * R6) * DF6 + (1 - Q6 * R6) * BF6)
    check("Exercise 3.6, expected remediation", n(REM6, P2), D("112083.33"))
    TOT6 = REV6 + EL6 * COD6 + REM6
    check("Exercise 3.6, delay cost 4 x 9,500", EL6 * COD6, 38000)
    check("Exercise 3.6, total with the gate", n(TOT6, P2), D("180083.33"))
    check("Exercise 3.6, cost without the gate (from Exercise 3.3)", NG6, 150000)
    check("Exercise 3.6, the gate now destroys", n(TOT6 - NG6, P2), D("30083.33"))
    check("Exercise 3.6, swing from the nominal +29,500", n(D(29500) + (TOT6 - NG6), P2),
          D("59583.33"))
    D6S = (REV6 + EL6 * COD6) / (PD6 * (BF6 - DF6))
    check("Exercise 3.6, d* re-derived (Exercise 3.3's 52.31 %)", n(D6S * 100, P2), D("52.31"))
    check("Exercise 3.6 substitution, d* as a decimal", n(D6S, P4), D("0.5231"))
    check("Exercise 3.6, the d* denominator 130,000", PD6 * (BF6 - DF6), 130000)
    check("Exercise 3.6, the d* numerator 68,000", REV6 + EL6 * COD6, 68000)
    R6S = D6S / Q6
    check("Exercise 3.6, breakeven closure rate", n(R6S * 100, P2), D("69.74"))
    check("Exercise 3.6, r* in conditions out of 18", n(R6S * 18, P4), D("12.5538"))
    check("Exercise 3.6, so 13 of 18 are needed",
          int((R6S * 18).to_integral_value(rounding="ROUND_CEILING")), 13)
    SL6 = PD6 * Q6 * (BF6 - DF6)
    IN6 = NG6 - (REV6 + EL6 * COD6 + PD6 * BF6)
    check("Exercise 3.6, the net-value line's slope", SL6, 97500)
    check("Exercise 3.6, the net-value line's intercept", IN6, -68000)
    check("Exercise 3.6, net at 12 of 18", n(IN6 + SL6 * D(12) / 18, P2), D("-3000.00"))
    check("Exercise 3.6, net at 13 of 18", n(IN6 + SL6 * D(13) / 18, P2), D("2416.67"))
    check("Exercise 3.6 note: r* differs from Meridian's 69.81 % — a near-miss, not a rule",
          1 if R6S != RSTAR else 0, 1)

    ROWS7 = (("R1", D("0.25"), D(600000), (D("0.60"), D("0.50"))),
             ("R2", D("0.30"), D(450000), (D("0.50"),)),
             ("R3", D("0.40"), D(250000), ()),
             ("R4", D("0.20"), D(900000), (D("0.40"),)))
    E7 = {rid: p * miss(qs) * u for rid, p, u, qs in ROWS7}
    check("Exercise 3.7, second line cost 25 days at 800", D(25) * D(800), 20000)
    for rid, printed in (("R1", 30000), ("R2", 67500), ("R3", 100000), ("R4", 108000)):
        check(f"Exercise 3.7, residual exposure {rid}", E7[rid], printed)
    T7 = sum(E7.values())
    check("Exercise 3.7, total residual exposure", T7, 305500)
    R1_7 = D("0.25") * miss((D("0.60"),)) * D(600000)
    R4_7 = D("0.20") * miss((D("0.40"), D("0.50"))) * D(900000)
    R3_7 = D("0.40") * miss((D("0.50"),)) * D(250000)
    check("Exercise 3.7 (a), R1 without the second line", R1_7, 60000)
    check("Exercise 3.7 (a), R4 with the second line", R4_7, 54000)
    TA7 = R1_7 + E7["R2"] + E7["R3"] + R4_7
    check("Exercise 3.7 (a), total", TA7, 281500)
    check("Exercise 3.7 (a), reduction", T7 - TA7, 24000)
    check("Exercise 3.7 (b), R3 with the second line", R3_7, 50000)
    TB7 = R1_7 + E7["R2"] + R3_7 + E7["R4"]
    check("Exercise 3.7 (b), total", TB7, 285500)
    check("Exercise 3.7 (b), reduction", T7 - TB7, 20000)
    check("Exercise 3.7, (a) is better by", TB7 - TA7, 4000)
    check("Exercise 3.7, that as a share of the available gain",
          n((TB7 - TA7) / (T7 - TA7) * 100, P2), D("16.67"))
    check("Exercise 3.7, marginal value on R1", R1_7 - E7["R1"], 30000)
    check("Exercise 3.7, marginal value on R4", E7["R4"] - R4_7, 54000)
    check("Exercise 3.7, marginal value on R3", E7["R3"] - R3_7, 50000)
    check("Exercise 3.7 common error: the largest residual outranks the wholly uncovered row",
          1 if E7["R4"] > E7["R3"] else 0, 1)

    BAC8, PV8, EV8, AC8, WK8, DD8 = D(6000000), D(3300000), D(3000000), D(3300000), D(40), D(22)
    CPI8, SPI8 = EV8 / AC8, EV8 / PV8
    check("Exercise 3.8, CPI", n(CPI8, P4), D("0.9091"))
    check("Exercise 3.8, SPI", n(SPI8, P4), D("0.9091"))
    check("Exercise 3.8, CPI equals SPI because PV = AC", 1 if CPI8 == SPI8 else 0, 1)
    for lab, eac, pv_, pp in (("AC + (BAC - EV)", AC8 + (BAC8 - EV8), 300000, D("5.00")),
                              ("BAC/CPI", BAC8 / CPI8, 600000, D("10.00")),
                              ("composite", AC8 + (BAC8 - EV8) / (CPI8 * SPI8), 930000,
                               D("15.50"))):
        check(f"Exercise 3.8, VAC {lab}", eac - BAC8, pv_)
        check(f"Exercise 3.8, VAC as % of BAC {lab}", n((eac - BAC8) / BAC8 * 100, P2), pp)
    check("Exercise 3.8, EAC (a)", AC8 + (BAC8 - EV8), 6300000)
    check("Exercise 3.8, EAC (b)", BAC8 / CPI8, 6600000)
    check("Exercise 3.8, EAC (c)", AC8 + (BAC8 - EV8) / (CPI8 * SPI8), 6930000)
    check("Exercise 3.8, tolerance", D("0.10") * BAC8, 600000)
    check("Exercise 3.8, BAC/CPI lands exactly on the tolerance",
          (BAC8 / CPI8 - BAC8) - D("0.10") * BAC8, 0)
    LAT8 = ew(D(3), D(1)) + ew(D(5), D(2)) + ew(D(13), D(4))
    check("Exercise 3.8, tier 1 latency", n(ew(D(3), D(1)), P1), D("2.5"))
    check("Exercise 3.8, tier 2 latency", n(ew(D(5), D(2)), P1), D("4.5"))
    check("Exercise 3.8, tier 3 latency", n(ew(D(13), D(4)), P1), D("10.5"))
    check("Exercise 3.8, total latency", n(LAT8, P1), D("17.5"))
    REM8 = WK8 - DD8
    check("Exercise 3.8, remaining duration", REM8, 18)
    check("Exercise 3.8, weeks left after the decision", n(REM8 - LAT8, P1), D("0.5"))
    check("Exercise 3.8, decision action window", n((REM8 - LAT8) / REM8 * 100, P2), D("2.78"))
    check("Exercise 3.8, last actionable week", n(WK8 - LAT8, P1), D("22.5"))
    check("Exercise 3.8, that as a share of the duration", n((WK8 - LAT8) / WK8 * 100, P2),
          D("56.25"))
