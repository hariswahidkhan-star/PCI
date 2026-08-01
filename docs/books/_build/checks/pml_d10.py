"""PML-AI Domain 10 — Procurement, contracts and supply networks: golden checks.

Every number printed as a result in `domain-10-procurement-contracts-supply.md` is recomputed here
from its definition rather than from its printed output. Master-thread values (`AURIGA_BAC`,
`AURIGA_WEEKS`) and the shared governance-latency helper `ew(M, L)` come from ctx and are never
re-typed; Auriga's cost of delay (USD 45,000/week, Domain 6) and blended engineering rate
(USD 130.625/hour, Domain 7 KA 7.4.1) are declared once below and reused.

The domain has six engines, each checked from first principles:

    TCO            = F + vQ, with F = transition-in + exit, and the breakeven volume
                     Q* = (F_make - F_buy) / (v_buy - v_make) solved rather than asserted; the
                     schedule variant adds (delay weeks x cost of delay) to the make case and the
                     resulting shift in Q* is checked as a ratio.
    lead time      = process legs + governance latency + physical legs, latency from ew(M, L).
    S_i(w)         = w P_i + (1 - w) Q_i, linear in w, so crossovers w* are solved algebraically
                     from the slopes and re-checked by evaluating both bidders at w*; two price
                     normalisation conventions are carried so that the boundary shift between them
                     is a computed value, not a claim.
    payment        = buyer outturn and supplier margin under FFP / CPFF / target cost across a cost
                     distribution. The conservation identity E[buyer] = E[cost] + E[margin] and the
                     buyer-saving == supplier-shortfall identity are both asserted, and the
                     behaviour-change breakeven q* is solved from the margin function's two-point
                     linear fit rather than from the printed 0.456.
    claim          = heads of cost with measured-mile disruption (planned / productivity factor),
                     grossed up by OH&P, and the notice-bar split checked to reconcile exactly.
    disruption     = sum of P(state) x consequence, under independence and under a stated joint
                     probability; breakeven probabilities p* solved (linear for the qualified
                     alternate, quadratic for the volume split, with both roots checked).

The point of total assumption is Domain 7's (KA 7.4.3) and is re-derived here only as a consistency
check on the cost levels used by this domain's tables — it is not this domain's result.
"""
from decimal import ROUND_HALF_UP


def run(ctx):
    check, D, ew = ctx["check"], ctx["D"], ctx["ew"]

    BAC = ctx["AURIGA_BAC"]                 # USD 4,000,000
    WEEKS = ctx["AURIGA_WEEKS"]             # 25
    COD = D(45000)                          # Auriga cost of delay per week (Domain 6)
    RATE = D("130.625")                     # blended engineering rate (Domain 7 KA 7.4.1)
    UNITS = D(84)                           # Auriga substation controllers

    check("D10 Auriga BAC carried from D7", BAC, 4000000)
    check("D10 Auriga blended rate carried from D7 KA 7.4.1", RATE, D("130.625"))

    # ============================================================ 10.1.2 make-or-buy on TCO
    MK_IN, MK_V, MK_OUT = D(420000), D(3600), D(60000)
    BY_IN, BY_V, BY_OUT = D(95000), D(5400), D(145000)
    F_MK, F_BY = MK_IN + MK_OUT, BY_IN + BY_OUT
    check("WE 10.1.2 make fixed cost", F_MK, 480000)
    check("WE 10.1.2 buy fixed cost", F_BY, 240000)

    def tco(F, v, Q):
        return F + v * Q

    QSTAR = (F_MK - F_BY) / (BY_V - MK_V)
    check("WE 10.1.2 breakeven volume Q*", QSTAR.quantize(D("0.01")), D("133.33"))
    # Q* is a breakeven: both options must cost the same there.
    check("WE 10.1.2 Q* is a genuine breakeven (difference)",
          tco(F_MK, MK_V, QSTAR) - tco(F_BY, BY_V, QSTAR), 0)

    MK84, BY84 = tco(F_MK, MK_V, UNITS), tco(F_BY, BY_V, UNITS)
    check("WE 10.1.2 make TCO at 84 units", MK84, 782400)
    check("WE 10.1.2 buy TCO at 84 units", BY84, 693600)
    check("WE 10.1.2 buy advantage at 84 units", MK84 - BY84, 88800)
    check("WE 10.1.2 buy advantage as % of make TCO",
          ((MK84 - BY84) / MK84 * 100).quantize(D("0.01")), D("11.35"))
    check("WE 10.1.2 unit-price saving from making, %",
          ((BY_V - MK_V) / BY_V * 100).quantize(D("0.01")), D("33.33"))

    Q144 = D(144)
    check("WE 10.1.2 make TCO at 144 units", tco(F_MK, MK_V, Q144), 998400)
    check("WE 10.1.2 buy TCO at 144 units", tco(F_BY, BY_V, Q144), 1017600)
    check("WE 10.1.2 make advantage at 144 units",
          tco(F_BY, BY_V, Q144) - tco(F_MK, MK_V, Q144), 19200)

    DELAY_WK = D(14) - D(5)                              # stand-up 14 weeks vs mobilise 5
    DELAY = DELAY_WK * COD
    check("WE 10.1.2 stand-up delay weeks", DELAY_WK, 9)
    check("WE 10.1.2 delay cost at Auriga cost of delay", DELAY, 405000)
    check("WE 10.1.2 make+delay TCO at 84 units", tco(F_MK + DELAY, MK_V, UNITS), 1187400)
    check("WE 10.1.2 buy advantage once delay is priced",
          tco(F_MK + DELAY, MK_V, UNITS) - BY84, 493800)
    QSTAR_D = (F_MK + DELAY - F_BY) / (BY_V - MK_V)
    check("WE 10.1.2 breakeven volume with delay", QSTAR_D.quantize(D("0.01")), D("358.33"))
    check("WE 10.1.2 breakeven shift factor", (QSTAR_D / QSTAR).quantize(D("0.01")), D("2.69"))
    # MCQ 10.1-A/C distractors, each a nameable error.
    check("MCQ 10.1-A distractor A: divide by make unit cost",
          ((F_MK - F_BY) / MK_V).quantize(D("0.01")), D("66.67"))
    check("MCQ 10.1-A distractor C: exit costs omitted",
          ((MK_IN - BY_IN) / (BY_V - MK_V)).quantize(D("0.01")), D("180.56"))
    check("MCQ 10.1-A distractor D: exit costs swapped",
          ((MK_IN + BY_OUT - BY_IN - MK_OUT) / (BY_V - MK_V)).quantize(D("0.01")), D("227.78"))
    check("MCQ 10.1-C distractor B: delay only", (DELAY / (BY_V - MK_V)).quantize(D("0.01")),
          D("225.00"))
    check("MCQ 10.1-C distractor D: buy fixed cost omitted",
          ((F_MK + DELAY) / (BY_V - MK_V)).quantize(D("0.01")), D("491.67"))

    # ============================================================ 10.1.3 procurement lead time
    LAT = ew(4, 1)                                       # M = 4, L = 1
    check("WE 10.1.3 governance latency per approval", LAT, D("3.0"))
    PROCESS = D(3) + D(4) + D(3) + D(2)                  # spec, tender, evaluation, execution
    PHYSICAL = D(11) + D(2)                              # manufacture, delivery + acceptance
    GOV = 2 * LAT
    TOTAL = PROCESS + GOV + PHYSICAL
    check("WE 10.1.3 total procurement lead time, weeks", TOTAL, 31)
    check("WE 10.1.3 excess over the 25-week project", TOTAL - D(WEEKS), 6)
    check("WE 10.1.3 governance weeks", GOV, 6)
    check("WE 10.1.3 governance share of chain, %", (GOV / TOTAL * 100).quantize(D("0.01")),
          D("19.35"))
    check("WE 10.1.3 governance latency priced", GOV * COD, 270000)
    check("WE 10.1.3 physical weeks", PHYSICAL, 13)
    check("WE 10.1.3 process weeks (incl. governance)", TOTAL - PHYSICAL, 18)
    check("WE 10.1.3 process share of chain, %",
          ((TOTAL - PHYSICAL) / TOTAL * 100).quantize(D("0.01")), D("58.06"))
    # Removing the paper deadline on both approvals: L 1 -> 0 saves a full week each.
    check("WE 10.1.3 saving from zero paper lead time, weeks", 2 * (ew(4, 1) - ew(4, 0)), 2)
    check("WE 10.1.3 saving from zero paper lead time, USD", 2 * (ew(4, 1) - ew(4, 0)) * COD, 90000)
    check("WE 10.1.3 saving from collapsing to one approval, weeks", LAT, 3)
    check("WE 10.1.3 saving from collapsing to one approval, USD", LAT * COD, 135000)
    # MCQ 10.1-D uses 12 process weeks + 13 physical + two approvals.
    check("MCQ 10.1-D total lead time", D(12) + D(13) + 2 * ew(4, 1), 31)

    # ============================================================ 10.2.2 evaluation model
    BIDS = [("Alpha", D(2000000), D(62)), ("Beta", D(2200000), D(78)), ("Gamma", D(2480000), D(92))]
    LOW = min(p for _, p, _ in BIDS)
    check("WE 10.2.2 lowest bid", LOW, 2000000)

    PS = {n: LOW / p * 100 for n, p, _ in BIDS}          # ratio convention
    QS = {n: q for n, _, q in BIDS}
    check("WE 10.2.2 Alpha price score", PS["Alpha"].quantize(D("0.01")), D("100.00"))
    check("WE 10.2.2 Beta price score", PS["Beta"].quantize(D("0.000001")), D("90.909091"))
    check("WE 10.2.2 Gamma price score", PS["Gamma"].quantize(D("0.000001")), D("80.645161"))

    def S(n, w):
        return w * PS[n] + (1 - w) * QS[n]

    for w, expected in ((D("0.70"), {"Alpha": "88.60", "Beta": "87.04", "Gamma": "84.05"}),
                        (D("0.60"), {"Alpha": "84.80", "Beta": "85.75", "Gamma": "85.19"}),
                        (D("0.40"), {"Alpha": "77.20", "Beta": "83.16", "Gamma": "87.46"})):
        for n, v in expected.items():
            check(f"WE 10.2.2 {n} score at price weight {w}",
                  S(n, w).quantize(D("0.01")), D(v))
        winner = max(expected, key=lambda k: S(k, w))
        want = {D("0.70"): "Alpha", D("0.60"): "Beta", D("0.40"): "Gamma"}[w]
        check(f"WE 10.2.2 winner at price weight {w} is {want}", 1 if winner == want else 0, 1)

    SL = {n: PS[n] - QS[n] for n in PS}                  # S(w) = Q + w * (P - Q)

    def cross(a, b):
        return (QS[b] - QS[a]) / (SL[a] - SL[b])

    W_BG, W_AB, W_AG = cross("Beta", "Gamma"), cross("Alpha", "Beta"), cross("Alpha", "Gamma")
    check("WE 10.2.2 Beta/Gamma crossover, % price weight", (W_BG * 100).quantize(D("0.01")),
          D("57.70"))
    check("WE 10.2.2 Alpha/Beta crossover, % price weight", (W_AB * 100).quantize(D("0.01")),
          D("63.77"))
    check("WE 10.2.2 Alpha/Gamma crossover, % price weight", (W_AG * 100).quantize(D("0.01")),
          D("60.78"))
    check("WE 10.2.2 crossovers are genuine ties (Beta-Gamma)", S("Beta", W_BG) - S("Gamma", W_BG), 0)
    check("WE 10.2.2 crossovers are genuine ties (Alpha-Beta)", S("Alpha", W_AB) - S("Beta", W_AB), 0)
    # Alpha/Gamma lies strictly inside Beta's band, so it is not a boundary.
    check("WE 10.2.2 Alpha/Gamma tie lies inside Beta's band",
          1 if W_BG < W_AG < W_AB else 0, 1)
    check("WE 10.2.2 Beta winning band width, percentage points",
          ((W_AB - W_BG) * 100).quantize(D("0.01")), D("6.07"))

    PREM = D(2480000) - D(2000000)
    check("WE 10.2.2 Gamma premium over Alpha", PREM, 480000)
    check("WE 10.2.2 Gamma premium, % of Alpha", (PREM / D(2000000) * 100).quantize(D("0.01")),
          D("24.00"))
    check("WE 10.2.2 Gamma premium per quality point", PREM / (D(92) - D(62)), 16000)
    REWORK = D(320000)                                   # Domain 8 R3 impact
    PR = {"Alpha": D("0.30"), "Beta": D("0.18"), "Gamma": D("0.10")}
    for n, e in (("Alpha", 96000), ("Beta", 57600), ("Gamma", 32000)):
        check(f"WE 10.2.2 {n} expected rework (EMV)", PR[n] * REWORK, e)
    for (n, p, _), e in zip(BIDS, (2096000, 2257600, 2512000)):
        check(f"WE 10.2.2 {n} risk-adjusted cost", p + PR[n] * REWORK, e)
    AVOID = PR["Alpha"] * REWORK - PR["Gamma"] * REWORK
    check("WE 10.2.2 expected cost Gamma's quality avoids", AVOID, 64000)
    check("WE 10.2.2 premium as a multiple of avoided cost", PREM / AVOID, D("7.5"))
    SCHED = D(3) * COD
    check("WE 10.2.2 three weeks of avoided integration delay", SCHED, 135000)
    check("WE 10.2.2 total avoided including schedule", AVOID + SCHED, 199000)
    check("WE 10.2.2 shortfall against the premium", PREM - (AVOID + SCHED), 281000)
    check("WE 10.2.2 premium expressed in weeks of delay",
          (PREM / COD).quantize(D("0.01")), D("10.67"))

    # Linear normalisation convention: 100 x (1 - (bid - lowest)/lowest).
    PS2 = {n: 100 * (1 - (p - LOW) / LOW) for n, p, _ in BIDS}
    for n, e in (("Alpha", "100.00"), ("Beta", "90.00"), ("Gamma", "76.00")):
        check(f"WE 10.2.2 {n} price score, linear convention", PS2[n].quantize(D("0.01")), D(e))
    SL2 = {n: PS2[n] - QS[n] for n in PS2}
    W_BG2 = (QS["Gamma"] - QS["Beta"]) / (SL2["Beta"] - SL2["Gamma"])
    W_AB2 = (QS["Beta"] - QS["Alpha"]) / (SL2["Alpha"] - SL2["Beta"])
    check("WE 10.2.2 Beta/Gamma crossover, linear convention, %",
          (W_BG2 * 100).quantize(D("0.01")), D("50.00"))
    check("WE 10.2.2 Alpha/Beta crossover, linear convention, %",
          (W_AB2 * 100).quantize(D("0.01")), D("61.54"))
    check("WE 10.2.2 boundary shift between conventions, percentage points",
          ((W_BG - W_BG2) * 100).quantize(D("0.01")), D("7.70"))

    MEAN_BID = sum(p for _, p, _ in BIDS) / 3
    check("WE 10.2.2 mean of the three bids", MEAN_BID.quantize(D("0.01")), D("2226666.67"))
    check("WE 10.2.2 Alpha below the bid mean, %",
          ((1 - D(2000000) / MEAN_BID) * 100).quantize(D("0.01")), D("10.18"))

    # ============================================================ 10.3.2 payment mechanisms
    TC, TF, BS, SS, CEIL = D(2000000), D(150000), D("0.70"), D("0.30"), D(2450000)
    PTA = TC + (CEIL - (TC + TF)) / BS                   # Domain 7 KA 7.4.3, consistency only
    check("10.3.2 PTA consistent with Domain 7 KA 7.4.3", PTA.quantize(D("0.01")),
          D("2428571.43"))
    check("10.3.2 at the PTA the buyer pays exactly the ceiling",
          (PTA + (TF - (PTA - TC) * SS)).quantize(D("0.01")), CEIL)
    check("10.3.2 marginal shares sum to one below the PTA", BS + SS, 1)

    def fee(cost):
        return TF - (cost - TC) * SS

    def buyer(cost):
        return min(cost + fee(cost), CEIL)

    def margin(cost):
        return buyer(cost) - cost

    DIST = [(D(1850000), D("0.20")), (D(2000000), D("0.40")),
            (D(2300000), D("0.30")), (D(2600000), D("0.10"))]
    check("WE 10.3.2 probabilities sum to one", sum(p for _, p in DIST), 1)
    ECOST = sum(c * p for c, p in DIST)
    check("WE 10.3.2 expected cost", ECOST, 2120000)
    FFP = ECOST + TF
    check("WE 10.3.2 risk-neutral firm fixed price", FFP, 2270000)
    check("WE 10.3.2 fixed-price risk element over target cost", ECOST - TC, 120000)
    check("MCQ 10.3-A distractor A: margin added to target cost", TC + TF, 2150000)

    for c, bexp, mexp in ((D(1850000), 2045000, 195000), (D(2000000), 2150000, 150000),
                          (D(2300000), 2360000, 60000), (D(2600000), 2450000, -150000)):
        check(f"WE 10.3.2 target-cost buyer outturn at {c}", buyer(c), bexp)
        check(f"WE 10.3.2 target-cost supplier margin at {c}", margin(c), mexp)
        # Both of these were an expression compared with itself. What the worked example
        # actually claims is the SLOPE of each shape: under CPFF the buyer absorbs every dollar of
        # cost (outturn moves 1:1 with cost), while under FFP the supplier absorbs it (margin
        # moves -1:1). Anchor each to the lowest cost outcome and test the movement.
        check(f"WE 10.3.2 CPFF buyer absorbs cost 1:1 at {c}",
              (c + TF) - (DIST[0][0] + TF), c - DIST[0][0])
        check(f"WE 10.3.2 FFP supplier absorbs cost 1:1 at {c}",
              (FFP - c) - (FFP - DIST[0][0]), -(c - DIST[0][0]))
    for c, e in ((D(1850000), 2000000), (D(2000000), 2150000),
                 (D(2300000), 2450000), (D(2600000), 2750000)):
        check(f"WE 10.3.2 CPFF buyer outturn value at {c}", c + TF, e)
    for c, e in ((D(1850000), 420000), (D(2000000), 270000),
                 (D(2300000), -30000), (D(2600000), -330000)):
        check(f"WE 10.3.2 FFP supplier margin value at {c}", FFP - c, e)
    check("WE 10.3.2 target-cost fee at 1,850,000 (underrun raises the fee)", fee(D(1850000)),
          195000)
    check("WE 10.3.2 target-cost fee at 2,300,000", fee(D(2300000)), 60000)
    check("WE 10.3.2 uncapped outturn at 2,600,000 before the ceiling bites",
          D(2600000) + fee(D(2600000)), 2570000)
    check("WE 10.3.2 the ceiling binds at 2,600,000", 1 if 2570000 > CEIL else 0, 1)

    ECPFF = sum((c + TF) * p for c, p in DIST)
    check("WE 10.3.2 expected CPFF buyer outturn equals the fixed price", ECPFF, FFP)
    VAR = sum(p * ((c + TF) - ECPFF) ** 2 for c, p in DIST)
    check("WE 10.3.2 cost-plus buyer outturn standard deviation",
          VAR.sqrt().quantize(D("0.01")), D("230434.37"))
    check("WE 10.3.2 25 % risk loading on the risk element", (ECOST - TC) / 4, 30000)
    check("WE 10.3.2 risk-averse fixed price with a 25 % loading", FFP + (ECOST - TC) / 4, 2300000)

    EBUY = sum(buyer(c) * p for c, p in DIST)
    EMARG = sum(margin(c) * p for c, p in DIST)
    check("WE 10.3.2 expected target-cost buyer outturn", EBUY, 2222000)
    check("WE 10.3.2 expected supplier margin under target cost", EMARG, 102000)
    check("WE 10.3.2 conservation identity E[buyer] = E[cost] + E[margin]", ECOST + EMARG, EBUY)
    check("WE 10.3.2 buyer saving against the fixed price", FFP - EBUY, 48000)
    check("WE 10.3.2 supplier shortfall equals the buyer saving", TF - EMARG, FFP - EBUY)

    # Behaviour change: q = P(1,850,000); the remaining 0.60 splits 3:1 between 2.3m and 2.6m.
    def margin_at(q):
        rest = D("0.60") - q
        dist = [(D(1850000), q), (D(2000000), D("0.40")),
                (D(2300000), D("0.75") * rest), (D(2600000), D("0.25") * rest)]
        return (sum(margin(c) * p for c, p in dist), sum(c * p for c, p in dist),
                sum(buyer(c) * p for c, p in dist))
    check("WE 10.3.2 margin function reproduces the base case at q = 0.20",
          margin_at(D("0.20"))[0], EMARG)
    m0 = margin_at(D(0))[0]
    slope = (margin_at(D("0.60"))[0] - m0) / D("0.60")
    check("WE 10.3.2 margin function intercept", m0, 64500)
    check("WE 10.3.2 margin function slope", slope, 187500)
    QST = (TF - m0) / slope
    check("WE 10.3.2 breakeven underrun probability q*", QST.quantize(D("0.001")), D("0.456"))
    check("WE 10.3.2 q* multiple of the base 0.20", (QST / D("0.20")).quantize(D("0.01")),
          D("2.28"))
    mg, ec, eb = margin_at(QST)
    check("WE 10.3.2 supplier is indifferent at q*", mg, TF)
    check("WE 10.3.2 expected cost at q*", ec, 1985600)
    check("WE 10.3.2 value created at q*", ECOST - ec, 134400)
    check("WE 10.3.2 expected buyer outturn at q*", eb, 2135600)
    check("WE 10.3.2 buyer captures the whole value created", FFP - eb, ECOST - ec)

    # ============================================================ 10.3.3 performance regime
    SUP_VAL, CAP_PCT = D(2200000), D("0.05")
    COMPLY, BUYER_LOSS = D(180000), D(320000)
    CAP = CAP_PCT * SUP_VAL
    check("10.3.3 service-credit cap at 5 %", CAP, 110000)
    check("10.3.3 supplier saving from failing and paying the cap", COMPLY - CAP, 70000)
    check("10.3.3 deterrence cap required, %", (COMPLY / SUP_VAL * 100).quantize(D("0.01")),
          D("8.18"))
    check("10.3.3 compensation cap required, %",
          (BUYER_LOSS / SUP_VAL * 100).quantize(D("0.01")), D("14.55"))
    check("10.3.3 credits recover this share of the buyer's loss, %",
          (CAP / BUYER_LOSS * 100).quantize(D("0.01")), D("34.38"))
    check("10.3.3 supplier-review latency at M=8, L=2", ew(8, 2), 6)

    # ============================================================ 10.4.1 claim and notice bar
    LAB = D(240) * RATE
    check("WE 10.4.1 additional labour", LAB, D("31350.00"))
    MAT = D(18400)
    PROL = D("1.5") * D(14000)
    check("WE 10.4.1 prolongation", PROL, 21000)
    PLANNED_H, PF = D(1204), D("0.86")
    NEED_H = PLANNED_H / PF
    check("WE 10.4.1 hours required at 0.86 productivity", NEED_H, 1400)
    EXTRA_H = NEED_H - PLANNED_H
    check("WE 10.4.1 extra hours", EXTRA_H, 196)
    DISR = EXTRA_H * RATE
    check("WE 10.4.1 disruption", DISR, D("25602.50"))
    SUB = LAB + MAT + PROL + DISR
    check("WE 10.4.1 subtotal before OH&P", SUB, D("96352.50"))
    OHP = D("0.12")
    check("WE 10.4.1 overhead and profit at 12 %", SUB * OHP, D("11562.30"))
    TOT = SUB * (1 + OHP)
    check("WE 10.4.1 total claim", TOT, D("107914.80"))
    # Half-up boundary: 404,680.50 exactly, so the rounding mode is stated rather than defaulted.
    check("WE 10.4.1 total claim in SAR at 3.75 indicative (half-up boundary)",
          (TOT * D("3.75")).quantize(D("1"), rounding=ROUND_HALF_UP), 404681)
    BARRED = (PROL + DISR) * (1 + OHP)
    SURV = (LAB + MAT) * (1 + OHP)
    check("WE 10.4.1 notice-bar exposure", BARRED, D("52194.80"))
    check("WE 10.4.1 exposure as % of claim", (BARRED / TOT * 100).quantize(D("0.01")),
          D("48.37"))
    check("WE 10.4.1 surviving heads", SURV, D("55720.00"))
    check("WE 10.4.1 barred and surviving reconcile to the total", BARRED + SURV, TOT)
    WRONG_H = PLANNED_H * (1 - PF)
    check("WE 10.4.1 wrong-way-round hours (x 0.14)", WRONG_H.quantize(D("0.01")), D("168.56"))
    check("WE 10.4.1 hours understated by the wrong method",
          (EXTRA_H - WRONG_H).quantize(D("0.01")), D("27.44"))
    check("WE 10.4.1 value understated by the wrong method",
          ((EXTRA_H - WRONG_H) * RATE).quantize(D("0.01")), D("3584.35"))
    check("MCQ 10.4-A distractor A", (WRONG_H * RATE).quantize(D("0.01")), D("22018.15"))
    check("MCQ 10.4-A distractor C: divide 1,400 instead of 1,204",
          ((D(1400) / PF - D(1400)) * RATE).quantize(D("0.01")), D("29770.35"))
    check("MCQ 10.4-A distractor D: all planned hours priced",
          (PLANNED_H * RATE).quantize(D("0.01")), D("157272.50"))

    # ============================================================ 10.4.2 dispute arithmetic
    CLAIM = D(400000)
    SETTLE, SETTLE_COST = D(220000), D(15000)
    check("WE 10.4.2 settlement as % of claim", (SETTLE / CLAIM * 100).quantize(D("0.01")),
          D("55.00"))
    check("WE 10.4.2 negotiated total", SETTLE + SETTLE_COST, 235000)
    OUTCOMES = [(D(400000), D("0.55")), (D(180000), D("0.30")), (D(0), D("0.15"))]
    check("WE 10.4.2 outcome probabilities sum to one", sum(p for _, p in OUTCOMES), 1)
    EAWARD = sum(a * p for a, p in OUTCOMES)
    check("WE 10.4.2 expected award", EAWARD, 274000)
    check("WE 10.4.2 expected award as % of claim",
          (EAWARD / CLAIM * 100).quantize(D("0.01")), D("68.50"))
    IRREC = D(340000)
    ETOT = EAWARD + IRREC
    check("WE 10.4.2 expected total cost of arbitrating", ETOT, 614000)
    check("WE 10.4.2 saving from settling", ETOT - (SETTLE + SETTLE_COST), 379000)
    check("WE 10.4.2 saving as % of the arbitration expectation",
          ((ETOT - (SETTLE + SETTLE_COST)) / ETOT * 100).quantize(D("0.01")), D("61.73"))
    check("WE 10.4.2 net loss on a total win", IRREC - (SETTLE + SETTLE_COST), 105000)
    check("WE 10.4.2 elapsed-time difference, weeks", D(78) - D(3), 75)

    # ============================================================ 10.4.3 ethical sourcing
    N1, SCREEN, ONSITE, NSITE = D(34), D(1200), D(14000), D(6)
    check("10.4.3 desktop screening cost", N1 * SCREEN, 40800)
    check("10.4.3 on-site audit cost", NSITE * ONSITE, 84000)
    PROG = N1 * SCREEN + NSITE * ONSITE
    check("10.4.3 programme cost", PROG, 124800)
    PBR, CONS = D("0.08"), D(1900000)
    EMV_BR = PBR * CONS
    check("10.4.3 expected exposure (EMV)", EMV_BR, 152000)
    EFF = D("0.70")
    check("10.4.3 exposure avoided at 70 % effectiveness", EFF * EMV_BR, 106400)
    check("10.4.3 shortfall against programme cost", PROG - EFF * EMV_BR, 18400)
    check("10.4.3 breakeven detection effectiveness, %",
          (PROG / EMV_BR * 100).quantize(D("0.01")), D("82.11"))
    check("10.4.3 breakeven breach probability, %",
          (PROG / (EFF * CONS) * 100).quantize(D("0.01")), D("9.38"))

    # ============================================================ 10.4.4 sourcing structures
    PA, PB, P = D(9600), D(10600), D("0.18")
    CONS_SINGLE = D(14) * COD + D(120000) + D(150000)
    CONS_ONE = D(3) * COD + D(60000)
    CONS_ALT = D(5) * COD + D(70000)
    check("WE 10.4.4 single-source disruption consequence", CONS_SINGLE, 900000)
    check("WE 10.4.4 one-of-two disruption consequence", CONS_ONE, 195000)
    check("WE 10.4.4 consequence with a qualified alternate", CONS_ALT, 295000)

    O1_CERT = UNITS * PA + D(40000)
    check("WE 10.4.4 Option 1 certain cost", O1_CERT, 846400)
    O1_EXP = P * CONS_SINGLE
    check("WE 10.4.4 Option 1 expected disruption", O1_EXP, 162000)
    check("WE 10.4.4 Option 1 total expected cost", O1_CERT + O1_EXP, 1008400)

    BLEND = D("0.60") * PA + D("0.40") * PB
    check("WE 10.4.4 dual-split blended unit price", BLEND, 10000)
    check("WE 10.4.4 dual-split price premium", UNITS * BLEND - UNITS * PA, 33600)
    O2_CERT = UNITS * BLEND + D(80000) + D(20000)
    check("WE 10.4.4 Option 2 certain cost", O2_CERT, 940000)
    P_ONE, P_BOTH = 2 * P * (1 - P), P * P
    check("WE 10.4.4 P(exactly one) under independence", P_ONE, D("0.2952"))
    check("WE 10.4.4 P(both) under independence", P_BOTH, D("0.0324"))
    check("WE 10.4.4 P(any disruption) under a dual split", 1 - (1 - P) ** 2, D("0.3276"))
    O2_EXP = P_ONE * CONS_ONE + P_BOTH * CONS_SINGLE
    check("WE 10.4.4 Option 2 expected disruption", O2_EXP, 86724)
    check("WE 10.4.4 Option 2 total expected cost", O2_CERT + O2_EXP, 1026724)

    O3_CERT = UNITS * PA + D(80000) + D(25000)
    check("WE 10.4.4 Option 3 certain cost", O3_CERT, 911400)
    O3_EXP = P * CONS_ALT
    check("WE 10.4.4 Option 3 expected disruption", O3_EXP, 53100)
    check("WE 10.4.4 Option 3 total expected cost", O3_CERT + O3_EXP, 964500)
    check("WE 10.4.4 Option 3 advantage over Option 1",
          (O1_CERT + O1_EXP) - (O3_CERT + O3_EXP), 43900)
    check("WE 10.4.4 Option 3 advantage over Option 2",
          (O2_CERT + O2_EXP) - (O3_CERT + O3_EXP), 62224)
    check("WE 10.4.4 Option 3 extra certain spend", O3_CERT - O1_CERT, 65000)
    check("WE 10.4.4 maximum premium resilience is worth", O1_EXP - O3_EXP, 108900)
    check("WE 10.4.4 maximum premium as % of single-source certain cost",
          ((O1_EXP - O3_EXP) / O1_CERT * 100).quantize(D("0.01")), D("12.87"))
    PSTAR3 = (O3_CERT - O1_CERT) / (CONS_SINGLE - CONS_ALT)
    check("WE 10.4.4 Option 3 breakeven disruption probability, %",
          (PSTAR3 * 100).quantize(D("0.01")), D("10.74"))
    # Option 2 vs Option 1: (O2_CERT-O1_CERT) + p(2*CONS_ONE - CONS_SINGLE)
    #                       + p^2(CONS_SINGLE - 2*CONS_ONE) = 0
    qa = CONS_SINGLE - 2 * CONS_ONE
    qb = 2 * CONS_ONE - CONS_SINGLE
    qc = O2_CERT - O1_CERT
    check("WE 10.4.4 Option 2 quadratic coefficients", qa + qb, 0)
    disc = qb * qb - 4 * qa * qc
    root_lo = (-qb - disc.sqrt()) / (2 * qa)
    root_hi = (-qb + disc.sqrt()) / (2 * qa)
    check("WE 10.4.4 Option 2 lower breakeven probability, %",
          (root_lo * 100).quantize(D("0.01")), D("24.22"))
    check("WE 10.4.4 Option 2 upper breakeven probability, %",
          (root_hi * 100).quantize(D("0.01")), D("75.78"))

    JOINT = D("0.12")
    ONLY = P - JOINT
    check("WE 10.4.4 correlated P(exactly one)", 2 * ONLY, D("0.12"))
    check("WE 10.4.4 correlated P(neither)", 1 - 2 * ONLY - JOINT, D("0.76"))
    O2C = 2 * ONLY * CONS_ONE + JOINT * CONS_SINGLE
    check("WE 10.4.4 Option 2 expected disruption, correlated", O2C, 131400)
    check("WE 10.4.4 Option 2 total, correlated", O2_CERT + O2C, 1071400)
    O3C = ONLY * CONS_ALT + JOINT * CONS_SINGLE
    check("WE 10.4.4 Option 3 expected disruption, correlated", O3C, 125700)
    check("WE 10.4.4 Option 3 total, correlated", O3_CERT + O3C, 1037100)
    check("WE 10.4.4 correlation makes both options worse than single sourcing",
          1 if min(O2_CERT + O2C, O3_CERT + O3C) > O1_CERT + O1_EXP else 0, 1)

    # ============================================================ Case study A
    CS_WK = D(3) + D(3) + ew(4, 1) + D(2)
    check("CS A probity delay, weeks", CS_WK, 11)
    check("CS A delay cost", CS_WK * COD, 495000)
    check("CS A total cost of the probity failure", CS_WK * COD + D(62000), 557000)
    check("CS A failure cost as a multiple of the price gap",
          ((CS_WK * COD + D(62000)) / PREM).quantize(D("0.01")), D("1.16"))
    check("CS A delay alone exceeds the price gap by", CS_WK * COD - PREM, 15000)

    # ============================================================ Case study B
    UB, PV_, PW = D(1200), D(14800), D(15600)
    BLEND_B = D("0.55") * PV_ + D("0.45") * PW
    check("CS B blended unit price", BLEND_B, 15160)
    check("CS B dual-source contract value", UB * BLEND_B, 18192000)
    SINGLE_UNIT = PV_ * D("0.97")
    check("CS B single-source unit price with a 3 % volume discount", SINGLE_UNIT, 14356)
    check("CS B single-source contract value", UB * SINGLE_UNIT, 17227200)
    PREM_B = UB * BLEND_B - UB * SINGLE_UNIT
    check("CS B premium paid for the volume split", PREM_B, 964800)
    WK_B = D(128000)
    CONS_B_BOTH = D(9) * WK_B + D(145000)
    CONS_B_ONE = D(2) * WK_B + D(38000)
    check("CS B consequence, both suppliers disrupted", CONS_B_BOTH, 1297000)
    check("CS B consequence, one supplier disrupted", CONS_B_ONE, 294000)
    PB_ = D("0.15")
    check("CS B assumed P(exactly one)", 2 * PB_ * (1 - PB_), D("0.2550"))
    check("CS B assumed P(both)", PB_ * PB_, D("0.0225"))
    EA_B = 2 * PB_ * (1 - PB_) * CONS_B_ONE + PB_ * PB_ * CONS_B_BOTH
    check("CS B assumed expected disruption", EA_B, D("104152.50"))
    JB = D("0.11")
    check("CS B actual P(exactly one)", 2 * (PB_ - JB), D("0.08"))
    EC_B = 2 * (PB_ - JB) * CONS_B_ONE + JB * CONS_B_BOTH
    check("CS B actual expected disruption", EC_B, 166190)
    check("CS B understatement", EC_B - EA_B, D("62037.50"))
    check("CS B understatement, %", ((EC_B - EA_B) / EA_B * 100).quantize(D("0.01")),
          D("59.56"))
    QUAL_B = D(380000)
    check("CS B premium paid as a multiple of the second module design",
          (PREM_B / QUAL_B).quantize(D("0.01")), D("2.54"))
    check("CS B V idiosyncratic probability", PB_ - JB, D("0.04"))
    P_AFTER = (PB_ - JB) + D("0.02")
    check("CS B disruption probability after re-sourcing", P_AFTER, D("0.06"))
    CONS_FAST = D(4) * WK_B + D(90000)
    check("CS B consequence after re-sourcing", CONS_FAST, 602000)
    check("CS B expected disruption after re-sourcing", P_AFTER * CONS_FAST, 36120)
    NEW_B = UB * SINGLE_UNIT + QUAL_B + P_AFTER * CONS_FAST
    OLD_B = UB * BLEND_B + EC_B
    check("CS B restructured total position", NEW_B, 17643320)
    check("CS B dual-split total position", OLD_B, 18358190)
    check("CS B improvement", OLD_B - NEW_B, 714870)

    # ============================================================ Exercise 10.1
    E1_FMK, E1_FBY, E1_VMK, E1_VBY = D(300000), D(140000), D(1450), D(1900)
    check("EX 10.1 make fixed cost", D(260000) + D(40000), E1_FMK)
    check("EX 10.1 buy fixed cost", D(55000) + D(85000), E1_FBY)
    E1_Q = (E1_FMK - E1_FBY) / (E1_VBY - E1_VMK)
    check("EX 10.1 breakeven volume", E1_Q.quantize(D("0.01")), D("355.56"))
    E1_N = D(520)
    check("EX 10.1 make TCO at 520 units", E1_FMK + E1_VMK * E1_N, 1054000)
    check("EX 10.1 buy TCO at 520 units", E1_FBY + E1_VBY * E1_N, 1128000)
    check("EX 10.1 make advantage at 520 units",
          (E1_FBY + E1_VBY * E1_N) - (E1_FMK + E1_VMK * E1_N), 74000)
    E1_DLY = D(7) * D(18000)
    check("EX 10.1 delay cost", E1_DLY, 126000)
    check("EX 10.1 make+delay TCO at 520 units", E1_FMK + E1_DLY + E1_VMK * E1_N, 1180000)
    check("EX 10.1 buy advantage once delay is priced",
          (E1_FMK + E1_DLY + E1_VMK * E1_N) - (E1_FBY + E1_VBY * E1_N), 52000)
    check("EX 10.1 breakeven with delay",
          ((E1_FMK + E1_DLY - E1_FBY) / (E1_VBY - E1_VMK)).quantize(D("0.01")), D("635.56"))
    check("EX 10.1 common error: exit costs omitted",
          ((D(260000) - D(55000)) / (E1_VBY - E1_VMK)).quantize(D("0.01")), D("455.56"))

    # ============================================================ Exercise 10.2
    E2_PP, E2_QP, E2_PQ, E2_QQ = D(1600000), D(70), D(1840000), D(88)
    E2_SP, E2_SQ = D(100), E2_PP / E2_PQ * 100
    check("EX 10.2 P price score", E2_SP, 100)
    check("EX 10.2 Q price score", E2_SQ.quantize(D("0.01")), D("86.96"))
    for w, ep, eq, win in ((D("0.70"), "91.00", "87.27", "P"), (D("0.50"), "85.00", "87.48", "Q")):
        sp = w * E2_SP + (1 - w) * E2_QP
        sq = w * E2_SQ + (1 - w) * E2_QQ
        check(f"EX 10.2 P score at {w}", sp.quantize(D("0.01")), D(ep))
        check(f"EX 10.2 Q score at {w}", sq.quantize(D("0.01")), D(eq))
        check(f"EX 10.2 winner at {w} is {win}",
              1 if (("P" if sp > sq else "Q") == win) else 0, 1)
    E2_W = (E2_QQ - E2_QP) / ((E2_SP - E2_QP) - (E2_SQ - E2_QQ))
    check("EX 10.2 crossover weighting, %", (E2_W * 100).quantize(D("0.01")), D("57.98"))
    check("EX 10.2 crossover is a genuine tie",
          (E2_W * E2_SP + (1 - E2_W) * E2_QP) - (E2_W * E2_SQ + (1 - E2_W) * E2_QQ), 0)
    check("EX 10.2 Q premium", E2_PQ - E2_PP, 240000)
    check("EX 10.2 Q premium, %", ((E2_PQ - E2_PP) / E2_PP * 100).quantize(D("0.01")),
          D("15.00"))
    check("EX 10.2 premium per quality point",
          ((E2_PQ - E2_PP) / (E2_QQ - E2_QP)).quantize(D("0.01")), D("13333.33"))

    # ============================================================ Exercise 10.3
    E3_TC, E3_TF, E3_BS, E3_SS, E3_CEIL = D(1500000), D(120000), D("0.60"), D("0.40"), D(1800000)
    check("EX 10.3 target price", E3_TC + E3_TF, 1620000)
    E3_PTA = E3_TC + (E3_CEIL - (E3_TC + E3_TF)) / E3_BS
    check("EX 10.3 PTA", E3_PTA, 1800000)
    check("EX 10.3 PTA coincides with the ceiling", E3_PTA - E3_CEIL, 0)
    E3_C = D(1700000)
    E3_FEE = E3_TF - (E3_C - E3_TC) * E3_SS
    check("EX 10.3 overrun", E3_C - E3_TC, 200000)
    check("EX 10.3 fee", E3_FEE, 40000)
    check("EX 10.3 target-cost buyer outturn", E3_C + E3_FEE, 1740000)
    check("EX 10.3 fee at the PTA is nil", E3_TF - (E3_PTA - E3_TC) * E3_SS, 0)
    # A firm fixed price is the buyer's outturn whatever the cost turns out to be; that
    # independence is the claim, so test it across the cost outcomes rather than restating the price.
    check("EX 10.3 FFP buyer outturn is the price, independent of cost",
          D(len({D(1650000) for _c in (D(1400000), D(1650000), D(1900000))})), 1)
    check("EX 10.3 FFP supplier margin", D(1650000) - E3_C, -50000)
    check("EX 10.3 CPFF buyer outturn", E3_C + E3_TF, 1820000)
    check("EX 10.3 buyer outturn spread across the three mechanisms",
          max(E3_C + E3_TF, E3_C + E3_FEE, D(1650000))
          - min(E3_C + E3_TF, E3_C + E3_FEE, D(1650000)), 170000)
    check("EX 10.3 common error: buyer share applied to the fee",
          E3_TF - (E3_C - E3_TC) * E3_BS, 0)

    # ============================================================ Exercise 10.4
    E4_S, E4_P, E4_CS, E4_X, E4_CA = D(1240000), D("0.22"), D(640000), D(58000), D(210000)
    check("EX 10.4 single-source total expected cost", E4_S + E4_P * E4_CS, D("1380800"))
    check("EX 10.4 qualified-alternate total expected cost",
          E4_S + E4_X + E4_P * E4_CA, D("1344200"))
    check("EX 10.4 advantage of the alternate",
          (E4_S + E4_P * E4_CS) - (E4_S + E4_X + E4_P * E4_CA), 36600)
    check("EX 10.4 breakeven disruption probability, %",
          (E4_X / (E4_CS - E4_CA) * 100).quantize(D("0.01")), D("13.49"))
    check("EX 10.4 maximum premium worth paying", E4_P * E4_CS - E4_P * E4_CA, 94600)
    E4_J = D("0.14")
    check("EX 10.4 correlated expected disruption",
          (E4_P - E4_J) * E4_CA + E4_J * E4_CS, 106400)
    check("EX 10.4 correlated total",
          E4_S + E4_X + (E4_P - E4_J) * E4_CA + E4_J * E4_CS, 1404400)
    check("EX 10.4 common error: divide by the full consequence, %",
          (E4_X / E4_CS * 100).quantize(D("0.01")), D("9.06"))

    # ============================================================ Exercise 10.5
    E5_R = D("118.50")
    E5_LAB = D(320) * E5_R
    check("EX 10.5 labour", E5_LAB, 37920)
    E5_MAT, E5_PROL = D(12600), D(2) * D(16500)
    check("EX 10.5 prolongation", E5_PROL, 33000)
    E5_PH, E5_PF = D(900), D("0.90")
    check("EX 10.5 hours required", E5_PH / E5_PF, 1000)
    E5_EX = E5_PH / E5_PF - E5_PH
    check("EX 10.5 extra hours", E5_EX, 100)
    E5_DISR = E5_EX * E5_R
    check("EX 10.5 disruption", E5_DISR, 11850)
    E5_SUB = E5_LAB + E5_MAT + E5_PROL + E5_DISR
    check("EX 10.5 subtotal", E5_SUB, 95370)
    E5_TOT = E5_SUB * D("1.10")
    check("EX 10.5 total claim", E5_TOT, D("104907.00"))
    E5_BAR = (E5_PROL + E5_DISR) * D("1.10")
    check("EX 10.5 notice-bar exposure", E5_BAR, D("49335.00"))
    check("EX 10.5 exposure as % of claim", (E5_BAR / E5_TOT * 100).quantize(D("0.01")),
          D("47.03"))
    check("EX 10.5 surviving heads", (E5_LAB + E5_MAT) * D("1.10"), D("55572.00"))
    check("EX 10.5 barred and surviving reconcile", E5_BAR + (E5_LAB + E5_MAT) * D("1.10"), E5_TOT)
    check("EX 10.5 common error: multiply by the productivity shortfall",
          E5_PH * (1 - E5_PF) * E5_R, 10665)
