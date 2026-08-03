"""PML-AI Domain 16 — Transition, Closeout and Benefits Realisation: golden checks.

Every number printed as a result in `domain-16-transition-closeout-benefits.md` is recomputed here
from its definition rather than from its printed output, and every teaching invariant the domain
asserts is pinned as an inequality or an identity so that the *claim* cannot silently stop being
true. Master-thread values come from ctx and are never re-typed: `MERIDIAN_BASELINE` (approved cost
USD 2,400,000), `MERIDIAN_BENEFIT_FULL` (USD 979,200/yr), `MERIDIAN_BENEFIT_70PCT`
(USD 685,440/yr), `MERIDIAN_COST_OF_DELAY` (USD 14,280/week) and `AURIGA_BAC` (USD 4,000,000).
Domain 2's appraisal (8 years at 7 %, `AF = 5.971299`) is rebuilt from `af(r, n)` and its ramped
profile rather than asserted, so the closing account is measured against a recomputed promise.
Values owned by other domains and cited here — Auriga's week-13 `EV`/`AC` (Domain 7), Domain 3's
delegation saving and its 204,000 delegated band — are declared once below with their home named.

Five engines, each checked from first principles:

    readiness      the conjunction P = prod(p) against the equal-weight dashboard (Sigma p)/k, the
                   running product at each of the seven conditions, the proportional-shortfall gain
                   prod x (p'/p - 1) for every single-condition lift printed in the text and in the
                   figure panel, the uniform bound 0.98^7, the per-condition probability that a 95 %
                   conjunction demands, and min(p) as the perfectly correlated bound. The three
                   structural claims are checked as claims, not as arithmetic: the product never
                   exceeds the average, min(p) never falls below the product, and a mean-preserving
                   spread lowers the product while leaving the dashboard reading untouched. Both
                   printed MCQ distractors are derived from the named error.
    hold-or-go     total cost = weeks x cost of delay + uplift + 40 x (1 - P) x cost per failed
                   go-live, evaluated for all three options under BOTH correlation bounds, so the
                   dominance claim ("best under both") is checked as six inequalities rather than
                   asserted. The breakeven uplift spend is solved from the equality and then
                   re-tested either side of it, and the residual failed-clinic count after a
                   blanket uplift is computed rather than quoted.
    commercial     the final account as sum + variations + settlement - recoveries, with contract
                   growth decomposed into its three percentage components and asserted to sum to
                   the printed 9.01 %; the final payment recomputed BOTH ways (account less paid to
                   date, and the movements since the last certificate) and the two asserted equal,
                   since that reconciliation is the example's whole point. Carrying cost from its
                   three components, EMV of determination, the breakeven settlement price, and the
                   breakeven-premium identity c x delta-months + (EMV - assessed) checked against
                   the price-difference route.
    knowledge      reconstruction-to-record ratio, key-person concentration, expected recovery at
                   each retrieval rate, the breakeven retrieval rate and its post-index counterpart,
                   and the indexing return. The claim that retrieval beats capture is checked as an
                   inequality against the review's own cost-recovery multiple.
    benefits       U = clinics x rate x weeks; annual benefit U x a x h; the two-factor variance
                   decomposition in both orders, both asserted to sum to the same total, with the
                   interaction term checked as the difference between them and as the amount by
                   which valuing both factors at plan overstates the shortfall. Then the full
                   present-value bridge: the flat claim, Domain 2's adoption correction, the level
                   and timing variances (each rebuilt from its counterfactual profile), the omitted
                   operating cost, the cost outturn, the realised NPV and the improvement plan, with
                   the bridge asserted to close on the independently computed realised NPV. The
                   attribution sensitivity, the re-tested cost of delay, retention economics,
                   benefits decay and both case studies follow the same rule.

The exercises and every numeric MCQ option are checked on the same terms, because a wrong distractor
is as much a defect as a wrong marked answer.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))
    ge = lambda a, b: 1 if a >= b else 0          # inequality claims asserted as 1/0
    gt = lambda a, b: 1 if a > b else 0

    COD = ctx["MERIDIAN_COST_OF_DELAY"]           # USD 14,280/week (Domain 1)
    APPROVED = ctx["MERIDIAN_BASELINE"]           # USD 2,400,000 approved cost
    FULL = ctx["MERIDIAN_BENEFIT_FULL"]           # USD 979,200/yr full potential
    HONEST = ctx["MERIDIAN_BENEFIT_70PCT"]        # USD 685,440/yr at 70 % adoption
    R = D("0.07")                                 # Domain 2's appraisal discount rate
    AF8 = af(R, 8)
    v = lambda n: (1 + R) ** -n

    # values owned elsewhere, cited by this domain
    AURIGA_EV, AURIGA_AC = D(1920000), D(2120000)          # Domain 7, week 13
    D3_SAVING, D3_BAND = D(171360), D(204000)              # Domain 3 KA 3.2 delegation analysis

    # ---- master-thread anchors the domain restates ----------------------------------------
    check("D16 cost of delay from its Domain 1 basis (28 clinics x 6 h x 85)", D(28) * 6 * 85, COD)
    check("D16 AF(7 %, 8) as printed", q(AF8, 6), D("5.971299"))
    check("D16 honest annual benefit from 40 x 0.70 x 6 x 85 x 48",
          D(40) * D("0.70") * 6 * 85 * 48, HONEST)
    check("D16 full-potential annual benefit from 40 x 6 x 85 x 48", D(40) * 6 * 85 * 48, FULL)
    check("D16 Domain 2 honest NPV (recomputed ramped profile)",
          q(FULL * (D("0.40") * v(1) + D("0.60") * v(2) + D("0.70") * (AF8 - af(R, 2)))
            - APPROVED, 0), D(1332898))
    check("D16 Domain 2 honest PV of benefits",
          q(FULL * (D("0.40") * v(1) + D("0.60") * v(2) + D("0.70") * (AF8 - af(R, 2))), 0),
          D(3732898))
    check("D16 SAR indication of the measured benefit at 1 USD = 3.75 SAR (millions)",
          q(D(594864) * D("3.75") / 1000000, 2), D("2.23"))

    # ============================ ENGINE 1 — readiness as a conjunction (16.1.3) ===========
    NAMES = ("training", "data migration", "interfaces", "network", "workflow",
             "champion", "fallback")
    P = [D(s) for s in ("0.96", "0.90", "0.94", "0.98", "0.85", "0.80", "0.92")]
    K = len(P)
    check("16.1.3 seven conditions", K, 7)
    check("16.1.3 sum of the seven probabilities (the printed 6.35)", sum(P), D("6.35"))
    AVG = sum(P) / K
    check("16.1.3 dashboard reading (Sigma p)/k", q(AVG * 100, 2), D("90.71"))

    RUN, prod = [], D(1)
    for p in P:
        prod *= p
        RUN.append(prod)
    CONJ = RUN[-1]
    for i, printed in enumerate(("96.00", "86.40", "81.22", "79.59", "67.65", "54.12", "49.79")):
        check(f"16.1.3 running conjunction after {NAMES[i]}", q(RUN[i] * 100, 2), D(printed))
    check("16.1.3 P(clean go-live) = prod p", q(CONJ * 100, 2), D("49.79"))
    check("16.1.3 gap between dashboard and conjunction, pp", q((AVG - CONJ) * 100, 2), D("40.92"))
    check("16.1.3 workflow and champion together remove, pp", q((RUN[3] - RUN[5]) * 100, 2),
          D("25.47"))
    # INVARIANT: a product of probabilities never exceeds their average (dashboard error is signed)
    check("16.1.3 INVARIANT product <= average for the printed set", ge(AVG, CONJ), 1)
    check("16.1.3 INVARIANT product <= average holds for a second, flatter set",
          ge(sum([D("0.99")] * 7) / 7, D("0.99") ** 7), 1)
    # INVARIANT: min(p) is an upper bound on the conjunction — the perfectly correlated case is
    # the OPTIMISTIC one, which is the claim 16.1.3 and MCQ 16.1-A both rest on
    check("16.1.3 INVARIANT min(p) >= prod p", ge(min(P), CONJ), 1)
    check("16.1.3 perfectly correlated bound min(p)", q(min(P) * 100, 2), D("80.00"))
    # INVARIANT: a mean-preserving spread lowers the product and leaves the dashboard unchanged
    SPREAD = [P[0], D("0.86"), D("0.98"), P[3], P[4], P[5], P[6]]   # 0.90/0.94 -> 0.86/0.98
    check("16.1.3 INVARIANT mean-preserving spread leaves the dashboard reading identical",
          sum(SPREAD) / K, AVG)
    check("16.1.3 INVARIANT mean-preserving spread lowers the conjunction",
          gt(CONJ, SPREAD[0] * SPREAD[1] * SPREAD[2] * SPREAD[3] * SPREAD[4] * SPREAD[5]
             * SPREAD[6]), 1)
    # INVARIANT: adding a further condition below 1 lowers the conjunction (gap grows with k)
    check("16.1.3 INVARIANT an eighth condition at 0.95 lowers the conjunction",
          gt(CONJ, CONJ * D("0.95")), 1)

    # proportional shortfall: lifting p -> p' multiplies the conjunction by p'/p
    TARGET = D("0.98")
    for nm, p, fac, gain in (("champion", D("0.80"), "1.2250", "11.20"),
                             ("workflow", D("0.85"), "1.1529", "7.62"),
                             ("data migration", D("0.90"), "1.0889", "4.43"),
                             ("fallback", D("0.92"), "1.0652", "3.25"),
                             ("interfaces", D("0.94"), "1.0426", "2.12"),
                             ("training", D("0.96"), "1.0208", "1.04")):
        check(f"16.1.3 / Fig 16.1.1 multiplier from lifting {nm} to 0.98", q(TARGET / p, 4),
              D(fac))
        check(f"16.1.3 / Fig 16.1.1 gain from lifting {nm} to 0.98, pp",
              q(CONJ * (TARGET / p - 1) * 100, 2), D(gain))
    # INVARIANT: the lift identity itself — replacing p by p' multiplies the product by p'/p
    check("16.1.3 INVARIANT lifting champion to 0.98 reproduces the recomputed product",
          CONJ * TARGET / D("0.80"),
          P[0] * P[1] * P[2] * P[3] * P[4] * TARGET * P[6])
    check("16.1.3 the weakest link is worth nearly eleven times the strongest",
          q((TARGET / D("0.80") - 1) / (TARGET / D("0.96") - 1), 2), D("10.80"))
    # INVARIANT: gain per point of gap closed is proportional to 1/p, so ranking follows p'/p and
    # NOT the absolute gap — the claim MCQ 16.1-B turns on
    g_champ = CONJ * (D("0.82") / D("0.80") - 1) * 100      # two points closed at 0.80
    g_train = CONJ * (D("0.98") / D("0.96") - 1) * 100      # the same two points at 0.96
    check("MCQ 16.1-B two points closed at 0.80, pp", q(g_champ, 2), D("1.24"))
    check("MCQ 16.1-B the same two points closed at 0.96, pp", q(g_train, 2), D("1.04"))
    check("MCQ 16.1-B INVARIANT an equal gap closed at a lower p yields a larger gain",
          gt(g_champ, g_train), 1)
    UNIFORM = TARGET ** K
    check("16.1.3 all seven at 0.98", q(UNIFORM * 100, 2), D("86.81"))
    check("16.1.3 residual failure rate at 0.98 across seven", q((1 - UNIFORM) * 100, 2),
          D("13.19"))
    P95 = D("0.95") ** (D(1) / K)
    check("16.1.3 per-condition probability for a 95 % conjunction", q(P95 * 100, 2), D("99.27"))
    check("16.1.3 INVARIANT that per-condition figure reproduces 95 %", q(P95 ** K * 100, 2),
          D("95.00"))
    # MCQ 16.1-A distractors
    check("MCQ 16.1-A distractor A (the dashboard average)", q(AVG * 100, 2), D("90.71"))
    check("MCQ 16.1-A distractor B (min p, the correlated bound)", q(min(P) * 100, 2), D("80.00"))
    check("MCQ 16.1-A distractor D (dashboard rounded to 0.90 and raised to the seventh)",
          q(D("0.90") ** K * 100, 2), D("47.83"))
    check("MCQ 16.1-C distractor C (0.98 on every condition)", q(UNIFORM * 100, 2), D("86.81"))
    check("MCQ 16.1-C distractor D INVARIANT 99.90 % is stricter than the target requires",
          gt(D("0.999") ** K, D("0.95")), 1)
    # Domain 15 KA 15.1.3's conjunction, cited in 16.1.3 — checked so the citation cannot drift
    D15 = D(1)
    for x in ("0.90", "0.88", "0.85", "0.92", "0.95", "0.90"):
        D15 *= D(x)
    check("16.1.3 cited Domain 15 six-predecessor conjunction", q(D15 * 100, 2), D("52.95"))

    # ============================ ENGINE 2 — hold or go (16.1.4) ===========================
    CLINICS = D(40)
    REMED, LOST_WEEKS, HOURS_PLAN, RATE = D(24000), D(6), D(6), D(85)
    check("16.1.4 benefit lost per week by a reverted clinic", HOURS_PLAN * RATE, D(510))
    FAILCOST = REMED + LOST_WEEKS * HOURS_PLAN * RATE
    check("16.1.4 cost of one failed go-live", FAILCOST, D(27060))
    P_TARGETED = CONJ * (TARGET / D("0.80")) * (TARGET / D("0.85"))
    check("16.1.4 conjunction after the targeted uplift", q(P_TARGETED * 100, 2), D("70.32"))

    def opt_cost(weeks, spend, pclean):
        return weeks * COD + spend + CLINICS * (1 - pclean) * FAILCOST

    OPTS = (("1 go now", D(0), D(0), CONJ, min(P), "543445", "216480", "20.08", "8.00"),
            ("2 targeted uplift", D(2), D(38000), P_TARGETED, D("0.90"),
             "387766", "174800", "11.87", "4.00"),
            ("3 blanket uplift", D(3), D(96000), UNIFORM, TARGET,
             "281581", "160488", "5.27", "0.80"))
    for nm, w, s, pi, pc, ti, tc, fi, fc in OPTS:
        check(f"16.1.4 option {nm} — independent total", q(opt_cost(w, s, pi), 0), D(ti))
        check(f"16.1.4 option {nm} — correlated total", q(opt_cost(w, s, pc), 0), D(tc))
        check(f"16.1.4 option {nm} — expected failed clinics, independent",
              q(CLINICS * (1 - pi), 2), D(fi))
        check(f"16.1.4 option {nm} — expected failed clinics, correlated",
              q(CLINICS * (1 - pc), 2), D(fc))
    T1I, T1C = opt_cost(D(0), D(0), CONJ), opt_cost(D(0), D(0), min(P))
    T2I, T2C = opt_cost(D(2), D(38000), P_TARGETED), opt_cost(D(2), D(38000), D("0.90"))
    T3I, T3C = opt_cost(D(3), D(96000), UNIFORM), opt_cost(D(3), D(96000), TARGET)
    # INVARIANT: option 3 DOMINATES — cheapest under both correlation assumptions
    check("16.1.4 INVARIANT blanket uplift beats going now, independent", gt(T1I, T3I), 1)
    check("16.1.4 INVARIANT blanket uplift beats the targeted uplift, independent",
          gt(T2I, T3I), 1)
    check("16.1.4 INVARIANT blanket uplift beats going now, correlated", gt(T1C, T3C), 1)
    check("16.1.4 INVARIANT blanket uplift beats the targeted uplift, correlated",
          gt(T2C, T3C), 1)
    check("16.1.4 saving of the blanket uplift, independent", q(T1I - T3I, 0), D(261864))
    check("16.1.4 saving of the blanket uplift, correlated", q(T1C - T3C, 0), D(55992))
    check("16.1.4 three weeks of deferred benefit", 3 * COD, D(42840))
    check("16.1.4 total commitment of the blanket uplift", 3 * COD + D(96000), D(138840))
    BE_UPLIFT = T1I - 3 * COD - CLINICS * (1 - UNIFORM) * FAILCOST
    check("16.1.4 breakeven uplift spend, independent", q(BE_UPLIFT, 0), D(357864))
    check("16.1.4 breakeven uplift spend as a multiple of the 96,000 actually spent",
          q(BE_UPLIFT / D(96000), 1), D("3.7"))
    # the breakeven is a breakeven: equal at it, worse either side
    check("16.1.4 INVARIANT at the breakeven spend holding exactly ties with going now",
          q(opt_cost(D(3), BE_UPLIFT, UNIFORM), 2), q(T1I, 2))
    check("16.1.4 INVARIANT one dollar more than the breakeven makes holding worse",
          gt(opt_cost(D(3), BE_UPLIFT + 1, UNIFORM), T1I), 1)
    check("16.1.4 residual expected failed clinics after the blanket uplift",
          q(CLINICS * (1 - UNIFORM), 2), D("5.27"))
    # MCQ 16.1-D
    check("MCQ 16.1-D marked answer (expected saving from holding)", q(T1I - T3I, 0), D(261864))
    check("MCQ 16.1-D distractor A (the cost of holding, not the saving)",
          q(3 * COD + D(96000), 0), D(138840))
    check("MCQ 16.1-D distractor C (residual remediation after the uplift omitted)",
          q(T1I - (3 * COD + D(96000)), 0), D(404605))
    check("MCQ 16.1-D distractor D (cost of going now, no credit for the hold)", q(T1I, 0),
          D(543445))
    # hypercare exit series: threshold 0.8 sustained two consecutive weeks, weeks 4-7
    SERIES = [D("1.4"), D("0.9"), D("0.6"), D("0.5")]
    below = [1 if x < D("0.8") else 0 for x in SERIES]
    first = next(i for i in range(1, len(SERIES)) if below[i] and below[i - 1])
    check("16.1.4 hypercare exit week (first two consecutive weeks below 0.8, series from week 4)",
          4 + first, 7)
    check("16.1.4 hypercare exit is one week later than the planned week 6", 4 + first - 6, 1)

    # ============================ ENGINE 3 — commercial closeout (16.2) ====================
    check("16.2.1 measured annual run cost from its three components",
          D(42000) + D(39000) + D(27000), D(108000))
    OPEX_PV = D(108000) * AF8
    check("16.2.1 present value of the omitted operating cost", q(OPEX_PV, 0), D(644900))
    HONEST_NPV = FULL * (D("0.40") * v(1) + D("0.60") * v(2)
                         + D("0.70") * (AF8 - af(R, 2))) - APPROVED
    check("16.2.1 omitted operating cost as a share of the honest NPV",
          q(OPEX_PV / HONEST_NPV * 100, 1), D("48.4"))
    check("MCQ 16.2-D distractor A (undiscounted eight-year total)", D(108000) * 8, D(864000))
    check("MCQ 16.2-D overstatement of the undiscounted total",
          q((D(108000) * 8 / OPEX_PV - 1) * 100, 1), D("34.0"))
    check("MCQ 16.2-D distractor D (five years, undiscounted)", D(108000) * 5, D(540000))

    SUM, VARS, SETT, RECOV = D(1680000), D(96000), D(74000), D(18600)
    RET = SUM * D("0.05")
    check("16.2.3 retention at 5 % of the contract sum", RET, D(84000))
    CERT = SUM + VARS
    check("16.2.3 certified works value", CERT, D(1776000))
    check("16.2.3 paid on certification net of retention", CERT - RET, D(1692000))
    check("16.2.3 takeover release (half the retention)", RET / 2, D(42000))
    PAID = CERT - RET + RET / 2
    check("16.2.3 amounts paid to date", PAID, D(1734000))
    FINAL = SUM + VARS + SETT - RECOV
    check("16.2.3 final account", FINAL, D(1831400))
    check("16.2.3 contract growth over the original sum", q((FINAL / SUM - 1) * 100, 2),
          D("9.01"))
    check("16.2.3 final retention release", RET / 2 - RECOV, D(23400))
    check("16.2.3 final payment (account less amounts paid to date)", FINAL - PAID, D(97400))
    # INVARIANT: the final payment must equal the movements since the last certificate
    check("16.2.3 INVARIANT final payment reconciles to the movements since the certificate",
          RET / 2 + SETT - RECOV, FINAL - PAID)
    check("16.2.3 variations as a share of the original sum", q(VARS / SUM * 100, 2), D("5.71"))
    check("16.2.3 claim settlement as a share of the original sum", q(SETT / SUM * 100, 2),
          D("4.40"))
    check("16.2.3 defect recovery as a share of the original sum", q(RECOV / SUM * 100, 2),
          D("1.11"))
    check("16.2.3 INVARIANT the three components net to the printed growth",
          q((VARS + SETT - RECOV) / SUM * 100, 2), D("9.01"))
    check("MCQ 16.2-A distractor A (certified works value)", CERT, D(1776000))
    check("MCQ 16.2-A distractor C (recovery omitted)", SUM + VARS + SETT, D(1850000))
    check("MCQ 16.2-A distractor D (settlement omitted)", SUM + VARS - RECOV, D(1757400))

    CLAIMED, ASSESSED, PROVISION = D(148000), D(62000), D(148000)
    LOCKED = RET / 2 + PROVISION
    check("16.2.4 cash locked up (retention still held plus the provision)", LOCKED, D(190000))
    C = D(3250) + D("1.5") * D(700) + LOCKED * D("0.06") / 12
    check("16.2.4 internal management component", D("1.5") * D(700), D(1050))
    check("16.2.4 opportunity-cost component", LOCKED * D("0.06") / 12, D(950))
    check("16.2.4 monthly carrying cost", C, D(5250))
    check("16.2.4 fourteen months of carrying cost", C * 14, D(73500))
    check("16.2.4 INVARIANT fourteen months of carrying exceeds the claim's assessed value",
          gt(C * 14, ASSESSED), 1)
    EMV = D("0.25") * D(40000) + D("0.50") * ASSESSED + D("0.25") * D(110000)
    check("16.2.4 probabilities sum to one", D("0.25") + D("0.50") + D("0.25"), 1)
    check("16.2.4 EMV of determination", EMV, D(68500))
    check("16.2.4 expected cost of fighting to determination", EMV + C * 14, D(142000))
    BE_PRICE = EMV + C * 14 - C * 2
    check("16.2.4 breakeven settlement price at month 2", BE_PRICE, D(131500))
    check("16.2.4 breakeven premium over the assessed value", BE_PRICE - ASSESSED, D(69500))
    # INVARIANT: the transferable identity c x months saved + (EMV - assessed)
    check("16.2.4 INVARIANT breakeven premium identity reproduces the price route",
          C * 12 + (EMV - ASSESSED), BE_PRICE - ASSESSED)
    check("16.2.4 identity's first term (twelve months of carrying)", C * 12, D(63000))
    check("16.2.4 identity's second term (EMV less own assessment)", EMV - ASSESSED, D(6500))
    check("16.2.4 premium actually paid at the 74,000 settlement", SETT - ASSESSED, D(12000))
    check("16.2.4 premium paid as a share of the breakeven premium",
          q((SETT - ASSESSED) / (BE_PRICE - ASSESSED) * 100, 2), D("17.27"))
    check("16.2.4 saving against the expected cost of determination",
          (EMV + C * 14) - (SETT + C * 2), D(57500))
    check("16.2.4 INVARIANT settling at 74,000 beats fighting to determination",
          gt(EMV + C * 14, SETT + C * 2), 1)
    check("16.2.4 attention-driven share of the carrying cost", D(3250) + D("1.5") * D(700),
          D(4300))
    check("16.2.4 twelve months of carrying against the claim's assessed value (near-identity)",
          q(C * 12 / ASSESSED, 2), D("1.02"))
    check("MCQ 16.2-B distractor A (carrying cost ignored)", EMV, D(68500))
    check("MCQ 16.2-B distractor C (two carried months not credited)", EMV + C * 14, D(142000))
    check("MCQ 16.2-B distractor D (fourteen months of carrying alone)", C * 14, D(73500))
    check("MCQ 16.2-C rationale: winning at the assessed value after fourteen months",
          ASSESSED + C * 14, D(135500))

    # ============================ ENGINE 4 — knowledge and lessons (16.3) ==================
    PROCS, SINGLE = D(34), D(11)
    check("16.3.1 key-person concentration", q(SINGLE / PROCS * 100, 2), D("32.35"))
    RECON, RECORD, ITEMS = D(8400), D(1200), D(3)
    check("16.3.1 total reconstruction cost", RECON * ITEMS, D(25200))
    check("16.3.1 total cost of recording at the time", RECORD * ITEMS, D(3600))
    check("16.3.1 reconstruct-to-record ratio", q(RECON * ITEMS / (RECORD * ITEMS), 2), D("7.00"))
    check("16.3.1 INVARIANT the ratio is the per-item ratio", RECON / RECORD,
          RECON * ITEMS / (RECORD * ITEMS))
    check("MCQ 16.3-C distractor B (the item count, not the cost ratio)", ITEMS, 3)
    check("MCQ 16.3-C distractor C (absolute saving, mislabelled a ratio)",
          RECON * ITEMS - RECORD * ITEMS, D(21600))
    check("MCQ 16.3-C distractor D (per-item reconstruction over the recording total)",
          q(RECON / (RECORD * ITEMS), 2), D("2.33"))

    REVIEW, LESSONS, AVOIDED = D(46000), D(34), D(26000)
    ADDRESSABLE = LESSONS * AVOIDED
    check("16.3.3 addressable value of the lessons", ADDRESSABLE, D(884000))
    REC12 = LESSONS * D("0.12") * AVOIDED
    REC35 = LESSONS * D("0.35") * AVOIDED
    check("16.3.3 expected recovery at the current 12 % retrieval rate", REC12, D(106080))
    check("16.3.3 expected recovery at 35 % retrieval", REC35, D(309400))
    check("16.3.3 increment from indexing", REC35 - REC12, D(203320))
    check("16.3.3 return on the 18,000 index", q((REC35 - REC12) / D(18000), 2), D("11.30"))
    check("16.3.3 breakeven retrieval rate", q(REVIEW / ADDRESSABLE * 100, 2), D("5.20"))
    check("16.3.3 breakeven retrieval rate with the index",
          q((REVIEW + D(18000)) / ADDRESSABLE * 100, 2), D("7.24"))
    check("16.3.3 net value of review plus index at 35 % retrieval",
          REC35 - REVIEW - D(18000), D(245400))
    check("16.3.3 the review's cost-recovery multiple at 12 %", q(REC12 / REVIEW, 2), D("2.31"))
    # INVARIANT: the current retrieval rate is above breakeven, so the review pays
    check("16.3.3 INVARIANT 12 % retrieval exceeds the breakeven rate",
          gt(D("0.12"), REVIEW / ADDRESSABLE), 1)
    # INVARIANT: retrieval beats capture — indexing returns more per dollar than the review itself
    check("16.3.3 INVARIANT indexing returns more per dollar than the review does",
          gt((REC35 - REC12) / D(18000), REC12 / REVIEW), 1)
    check("MCQ 16.3-A distractor C (review cost over the expected recovery)",
          q(REVIEW / REC12 * 100, 2), D("43.36"))
    check("MCQ 16.3-A distractor D (breakeven rate with the decimal misplaced)",
          q(REVIEW / ADDRESSABLE * 1000, 2), D("52.04"))
    check("MCQ 16.3-B increment from raising retrieval by 23 points",
          LESSONS * D("0.23") * AVOIDED, D(203320))

    # ============================ ENGINE 5 — the closing account (16.4) ===================
    U = D(40) * RATE * 48
    check("16.4.2 U = clinics x rate x weeks", U, D(163200))
    A_PLAN, H_PLAN = D("0.700"), D("6.0")
    A_ACT, H_ACT = D("0.675"), D("5.4")
    check("16.4.1 adoption on the registered measure (27 of 40)", q(D(27) / D(40) * 100, 2),
          D("67.50"))
    check("16.4.2 non-adopting clinics", D(40) - 27, 13)
    check("16.4.2 case annual benefit U x a x h", U * A_PLAN * H_PLAN, HONEST)
    MEASURED = U * A_ACT * H_ACT
    check("16.4.2 measured annual benefit", MEASURED, D(594864))
    check("16.4.2 measured annual benefit the direct way (27 x 5.4 x 85 x 48)",
          D(27) * H_ACT * RATE * 48, MEASURED)
    check("16.4.2 shortfall against the honest case", HONEST - MEASURED, D(90576))
    check("16.4.2 shortfall as a percentage of the honest case",
          q((HONEST - MEASURED) / HONEST * 100, 2), D("13.21"))
    check("16.4.2 realised share of the honest case", q(MEASURED / HONEST * 100, 2), D("86.79"))
    check("16.4.2 realised share of the flat claim", q(MEASURED / FULL * 100, 2), D("60.75"))
    check("16.4.2 shortfall against the flat claim the case made", FULL - MEASURED, D(384336))
    ADOPT_EFF = U * H_PLAN * (A_ACT - A_PLAN)
    HOURS_EFF = U * A_ACT * (H_ACT - H_PLAN)
    check("16.4.2 adoption effect (adoption first)", ADOPT_EFF, D(-24480))
    check("16.4.2 hours effect (adoption first)", HOURS_EFF, D(-66096))
    check("16.4.2 INVARIANT the two effects sum to the total variance",
          ADOPT_EFF + HOURS_EFF, MEASURED - HONEST)
    HOURS_FIRST = U * A_PLAN * (H_ACT - H_PLAN)
    ADOPT_SECOND = U * H_ACT * (A_ACT - A_PLAN)
    check("16.4.2 hours effect (hours first)", HOURS_FIRST, D(-68544))
    check("16.4.2 adoption effect (hours first)", ADOPT_SECOND, D(-22032))
    check("16.4.2 INVARIANT the reversed order sums to the same total",
          HOURS_FIRST + ADOPT_SECOND, MEASURED - HONEST)
    INTERACT = U * (A_ACT - A_PLAN) * (H_ACT - H_PLAN)
    check("16.4.2 interaction term", INTERACT, D(2448))
    check("16.4.2 INVARIANT the interaction is what moves between the orders",
          HOURS_EFF - HOURS_FIRST, INTERACT)
    check("16.4.2 INVARIANT both factors at plan overstates the shortfall by the interaction",
          (ADOPT_EFF + HOURS_FIRST) - (MEASURED - HONEST), -INTERACT)
    check("16.4.2 hours error against adoption error", q(HOURS_EFF / ADOPT_EFF, 2), D("2.70"))
    check("16.4.2 benefit-per-unit assumption wrong by",
          q((D(1) - H_ACT / H_PLAN) * 100, 0), D(10))
    check("MCQ 16.4-A distractor C (all 40 clinics at the measured 5.4 hours)",
          D(40) * H_ACT * RATE * 48, D(881280))
    check("MCQ 16.4-A distractor D (27 clinics but the case's 6.0 hours)",
          D(27) * H_PLAN * RATE * 48, D(660960))
    check("MCQ 16.4-B distractor C (orders mixed) overstates the total by the interaction",
          -(ADOPT_EFF + HOURS_FIRST), D(93024))

    # ---- the present-value bridge -------------------------------------------------------
    PV_FLAT = FULL * AF8
    # Display-rounding boundary, inherited from Domain 2: at full precision the flat claim's PV is
    # 5,847,095.4973 and its NPV 3,447,095.4973 — half a dollar below the tie — while the printed
    # figure is the one the display-rounded AF = 5.971299 gives. Both are checked, and the
    # full-precision comparison carries a widened tolerance for exactly that half-dollar.
    check("16.4.2 bridge — flat claim PV of benefits on the printed AF 5.971299",
          q(FULL * D("5.971299"), 0), D(5847096))
    check("16.4.2 bridge — flat claim, as approved, on the printed AF 5.971299",
          q(FULL * D("5.971299") - APPROVED, 0), D(3447096))
    check("16.4.2 bridge — flat claim NPV at full precision (display-rounding boundary, tol 0.51)",
          PV_FLAT - APPROVED, D(3447096), D("0.51"))
    PV_HONEST = FULL * (D("0.40") * v(1) + D("0.60") * v(2) + D("0.70") * (AF8 - af(R, 2)))
    check("16.4.2 bridge — adoption term corrected (Domain 2)",
          q(PV_HONEST - PV_FLAT, 0), D(-2114198))
    check("16.4.2 bridge — honest ramped case (Domain 2)", q(PV_HONEST - APPROVED, 0),
          D(1332898))
    # level counterfactual: measured level (5.4 h, 67.50 % steady) on the CASE's timing
    PV_LEVEL_CF = U * H_ACT * (D("0.40") * v(1) + D("0.60") * v(2) + A_ACT * (AF8 - af(R, 2)))
    # realised: measured level on the MEASURED ramp (15 / 40 / 60 %, steady from year 4)
    PV_ACT = U * H_ACT * (D("0.15") * v(1) + D("0.40") * v(2) + D("0.60") * v(3)
                          + A_ACT * (AF8 - af(R, 3)))
    check("16.4.2 bridge — level variance", q(PV_LEVEL_CF - PV_HONEST, 0), D(-465015))
    check("16.4.2 bridge — timing variance", q(PV_ACT - PV_LEVEL_CF, 0), D(-413809))
    check("16.4.2 INVARIANT level and timing sum to the whole benefit variance",
          q(PV_ACT - PV_HONEST, 2), q((PV_LEVEL_CF - PV_HONEST) + (PV_ACT - PV_LEVEL_CF), 2))
    check("16.4.2 realised present value of benefits", q(PV_ACT, 0), D(2854073))
    check("16.4.2 bridge — operating cost omitted from the case", q(-OPEX_PV, 0), D(-644900))
    OUTTURN = D(2514000)
    check("16.4.2 bridge — cost outturn variance", -(OUTTURN - APPROVED), D(-114000))
    check("16.4.2 cost outturn as an overspend on approved cost",
          q((OUTTURN / APPROVED - 1) * 100, 2), D("4.75"))
    REALISED = PV_ACT - OUTTURN - OPEX_PV
    check("16.4.2 realised NPV (computed directly)", q(REALISED, 0), D(-304827))
    # INVARIANT: the bridge closes — every line sums to the realised NPV
    BRIDGE = (PV_FLAT - APPROVED) + (PV_HONEST - PV_FLAT) + (PV_LEVEL_CF - PV_HONEST) \
        + (PV_ACT - PV_LEVEL_CF) - OPEX_PV - (OUTTURN - APPROVED)
    check("16.4.2 INVARIANT the bridge lines close on the realised NPV", q(BRIDGE, 2),
          q(REALISED, 2))
    check("16.4.2 realised position with the operating cost stripped out",
          q(REALISED + OPEX_PV, 0), D(340073))
    # reversed bridge convention, stated in the note
    PV_TIMING_CF = U * H_PLAN * (D("0.15") * v(1) + D("0.40") * v(2) + D("0.60") * v(3)
                                 + A_PLAN * (AF8 - af(R, 3)))
    check("16.4.2 bridge reversed — timing variance first", q(PV_TIMING_CF - PV_HONEST, 0),
          D(-479771))
    check("16.4.2 bridge reversed — level variance second", q(PV_ACT - PV_TIMING_CF, 0),
          D(-399053))
    check("16.4.2 INVARIANT the bridge interaction is what moves between the two orders",
          q((PV_LEVEL_CF - PV_HONEST) - (PV_ACT - PV_TIMING_CF), 0), D(-65962))
    # improvement plan
    UPLIFT = FULL * D("0.80") - MEASURED
    check("16.4.2 improvement plan annual increment", UPLIFT, D(188496))
    AF58 = AF8 - af(R, 4)
    check("16.4.2 years 5-8 annuity factor", q(AF58, 6), D("2.584087"))
    PLAN_PV, PLAN_COST = UPLIFT * AF58, D(180000) * v(3)
    check("16.4.2 improvement plan present value of benefits", q(PLAN_PV, 0), D(487090))
    check("16.4.2 improvement plan present cost", q(PLAN_COST, 0), D(146934))
    check("16.4.2 improvement plan NPV", q(PLAN_PV - PLAN_COST, 0), D(340156))
    check("16.4.2 improvement plan benefit-cost ratio", q(PLAN_PV / PLAN_COST, 2), D("3.32"))
    check("16.4.2 post-plan NPV", q(REALISED + PLAN_PV - PLAN_COST, 0), D(35329))
    check("16.4.2 INVARIANT the plan turns the account positive",
          gt(REALISED + PLAN_PV - PLAN_COST, 0), 1)
    BE_LEVEL = ((-REALISED + PLAN_COST) / AF58 + MEASURED) / FULL
    check("16.4.2 breakeven realisation level for years 5-8, with the plan spent",
          q(BE_LEVEL * 100, 2), D("78.60"))
    check("16.4.2 INVARIANT at that level the account is exactly zero",
          q((FULL * BE_LEVEL - MEASURED) * AF58 - PLAN_COST + REALISED, 2), D("0.00"))
    check("16.4.2 current trajectory as a share of full potential",
          q(MEASURED / FULL * 100, 2), D("60.75"))
    check("16.4.2 the plan's designed level (80 % adoption at the full 6.0 hours)",
          q(FULL * D("0.80") / FULL * 100, 2), D("80.00"))
    check("16.4.2 INVARIANT the plan's 80 % clears the 78.60 % breakeven, which is why it pays",
          gt(D("0.80"), BE_LEVEL), 1)

    # attribution (16.4.3)
    check("16.4.3 comparison cohort size (the final wave)", D(8), 8)
    ATTR = D("4.8")
    check("16.4.3 steady benefit at 4.8 attributable hours", D(27) * ATTR * RATE * 48,
          D(528768))
    PV_ATTR = PV_ACT * ATTR / H_ACT
    check("16.4.3 realised PV of benefits at 4.8 hours", q(PV_ATTR, 0), D(2536954))
    check("16.4.3 realised NPV at 4.8 hours", q(PV_ATTR - OUTTURN - OPEX_PV, 0), D(-621946))
    check("16.4.3 the unfalsifiable claim more than doubles the measured loss",
          q((PV_ATTR - OUTTURN - OPEX_PV) / REALISED, 2), D("2.04"))
    # the cost of delay, re-tested at the realised rate
    COD_REAL = D(27) * H_ACT * RATE
    check("16.4.3 realised cost of delay per week", COD_REAL, D(12393))
    check("16.4.3 realised rate as a share of the figure used throughout",
          q(COD_REAL / COD * 100, 2), D("86.79"))
    check("16.4.3 overstatement of the figure used throughout",
          q((COD / COD_REAL - 1) * 100, 2), D("15.23"))
    check("16.4.3 Domain 3's delegation saving is 12 weeks of delay at 14,280",
          D3_SAVING / COD, 12)
    check("16.4.3 Domain 3's saving re-priced at the realised rate",
          (D3_SAVING / COD) * COD_REAL, D(148716))
    check("16.4.3 Domain 3's breakeven value-destruction rate at 14,280",
          q(D3_SAVING / D3_BAND * 100, 0), D(84))
    check("16.4.3 Domain 3's breakeven rate re-priced at the realised rate",
          q((D3_SAVING / COD) * COD_REAL / D3_BAND * 100, 2), D("72.90"))
    check("16.4.3 INVARIANT the conclusion survives the 15 % error (still far above any delegate)",
          gt(q((D3_SAVING / COD) * COD_REAL / D3_BAND * 100, 2), D(50)), 1)

    # retention economics (16.4.4)
    CLASSES = [D(s) for s in ("1.9", "0.4", "1.6", "1.4", "1.1")]
    check("16.4.4 archive classes sum to the stated volume", sum(CLASSES), D("6.4"))
    TB, UNIT, MONTHS = sum(CLASSES), D(21), D(12) * 7
    check("16.4.4 monthly storage cost", TB * UNIT, D("134.40"))
    STORE7 = TB * UNIT * MONTHS
    check("16.4.4 seven years of retention", q(STORE7, 0), D(11290))
    NEED_P, CONSEQ = D("0.18"), D(310000)
    check("16.4.4 expected value of holding the evidence", NEED_P * CONSEQ, D(55800))
    check("16.4.4 value-to-cost ratio of retention", q(NEED_P * CONSEQ / STORE7, 2), D("4.94"))
    BE_NEED = STORE7 / CONSEQ
    check("16.4.4 breakeven need probability", q(BE_NEED * 100, 2), D("3.64"))
    check("16.4.4 breakeven need probability as one chance in", q(1 / BE_NEED, 0), D(27))
    check("16.4.4 INVARIANT the assessed need probability exceeds the breakeven",
          gt(NEED_P, BE_NEED), 1)
    PD_TB, DEID = D("1.1"), D(34000)
    EXPOSURE = D("0.04") * D(1250000)
    PD_STORE = PD_TB * UNIT * MONTHS
    check("16.4.4 exposure removed by de-identification", EXPOSURE, D(50000))
    check("16.4.4 storage avoided by de-identification", q(PD_STORE, 0), D(1940))
    check("16.4.4 net gain from de-identification", q(EXPOSURE + PD_STORE - DEID, 0), D(17940))
    check("16.4.4 INVARIANT de-identification pays", gt(EXPOSURE + PD_STORE - DEID, 0), 1)
    check("16.4.4 exposure against storage cost on the same volume",
          q(EXPOSURE / PD_STORE, 0), D(26))
    check("MCQ 16.4-E distractor C (one year of storage)",
          q(TB * UNIT * 12 / CONSEQ * 100, 2), D("0.52"))
    check("MCQ 16.4-E distractor D (breakeven with the decimal misplaced)",
          q(BE_NEED * 1000, 2), D("36.42"))

    # model retention (16.4.5)
    OUTPUTS, UNREPRO, REDERIVE = D(214), D(61), D(1850)
    check("16.4.5 unexplainable share of the AI-informed decision base",
          q(UNREPRO / OUTPUTS * 100, 2), D("28.50"))
    check("16.4.5 cost of re-deriving them by hand", UNREPRO * REDERIVE, D(112850))

    # ============================ advanced topics (16.A.1) ================================
    BASE_LEVEL, DECAY = D("0.6075"), D("0.96")
    check("16.A.1 base effective realisation level equals the measured share",
          q(MEASURED / FULL, 4), BASE_LEVEL)
    LEVELS = [BASE_LEVEL * DECAY ** k for k in range(1, 6)]
    for k, printed in enumerate(("0.5832", "0.5599", "0.5375", "0.5160", "0.4953")):
        check(f"16.A.1 decayed level, year {4 + k}", q(LEVELS[k], 4), D(printed))
    DECAY_PV = sum((BASE_LEVEL - LEVELS[k]) * FULL * v(4 + k) for k in range(5))
    check("16.A.1 present value of the decay", q(DECAY_PV, 0), D(216824))
    SUST_PV = D(22000) * (AF8 - af(R, 3))
    check("16.A.1 present value of the sustainment programme", q(SUST_PV, 0), D(73634))
    check("16.A.1 net value of sustainment", q(DECAY_PV - SUST_PV, 0), D(143190))
    check("16.A.1 sustainment value ratio", q(DECAY_PV / SUST_PV, 2), D("2.94"))
    check("16.A.1 breakeven sustainment spend a year", q(DECAY_PV / (AF8 - af(R, 3)), 0),
          D(64782))
    check("16.A.1 INVARIANT the breakeven spend is nearly three times what was proposed",
          gt(DECAY_PV / (AF8 - af(R, 3)) / D(22000), D("2.9")), 1)

    # ============================ case study B — Auriga (16.CSB) ==========================
    CPI = AURIGA_EV / AURIGA_AC
    check("Case B Auriga CPI at week 13 (Domain 7)", q(CPI, 4), D("0.9057"))
    EAC = ctx["AURIGA_BAC"] / CPI
    check("Case B EAC = BAC/CPI", q(EAC, 0), D(4416667))
    ACTUAL = D(4412000)
    check("Case B outturn below the CPI forecast", q((EAC - ACTUAL) / EAC * 100, 2), D("0.11"))
    check("Case B outturn above budget", q((ACTUAL / ctx["AURIGA_BAC"] - 1) * 100, 2),
          D("10.30"))
    check("Case B INVARIANT the CPI forecast was accurate to within five thousand dollars",
          gt(D(5000), EAC - ACTUAL), 1)
    SHORT, HOURS_YR, RESTRICT = D("0.008"), D(8760), D(1900)
    check("Case B availability shortfall in hours a year", SHORT * HOURS_YR, D("70.08"))
    ANNUAL_LOSS = SHORT * HOURS_YR * RESTRICT
    check("Case B annual cost of the shortfall", ANNUAL_LOSS, D(133152))
    AF12 = af(R, 12)
    check("Case B AF(7 %, 12) as printed", q(AF12, 6), D("7.942686"))
    LOSS_PV = ANNUAL_LOSS * AF12
    check("Case B present value of the shortfall over the asset life", q(LOSS_PV, 0),
          D(1057585))
    RETEST = D(74000) + 3 * D(18000)
    check("Case B cost of remediation and retest", RETEST, D(128000))
    DEDUCT = D(240000)
    check("Case B value of accepting the deduction", q(DEDUCT - LOSS_PV, 0), D(-817585))
    check("Case B retesting is worth this much more than accepting",
          q((DEDUCT - LOSS_PV) * -1 - RETEST, 0), D(689585))
    check("Case B INVARIANT retesting beats accepting the deduction",
          gt(LOSS_PV - DEDUCT, RETEST), 1)
    check("Case B breakeven deduction", q(LOSS_PV + RETEST, 0), D(1185585))
    check("Case B breakeven deduction as a multiple of the offer",
          q((LOSS_PV + RETEST) / DEDUCT, 2), D("4.94"))
    BE_HOURLY = (DEDUCT + RETEST) / AF12 / (SHORT * HOURS_YR)
    check("Case B breakeven restriction cost per hour", q(BE_HOURLY, 0), D(661))
    check("Case B INVARIANT at that hourly rate acceptance exactly ties with retesting",
          q(DEDUCT - BE_HOURLY * SHORT * HOURS_YR * AF12, 2), q(-RETEST, 2))
    check("Case B retention held on the control-systems subcontract",
          D(3200000) * D("0.05"), D(160000))
    PC = [D(s) for s in ("0.97", "0.93", "0.99", "0.88", "0.96")]
    pc = D(1)
    for x in PC:
        pc *= x
    check("Case B cutover conjunction on five conditions", q(pc * 100, 2), D("75.45"))
    check("Case B cutover dashboard average", q(sum(PC) / len(PC) * 100, 2), D("94.60"))
    check("Case B INVARIANT the cutover product is below its dashboard average",
          ge(sum(PC) / len(PC), pc), 1)

    # ============================ calculation exercises ===================================
    # 16.1 readiness on six conditions
    E1 = [D(s) for s in ("0.95", "0.88", "0.92", "0.99", "0.86", "0.90")]
    check("EX 16.1 sum of the six probabilities", sum(E1), D("5.50"))
    check("EX 16.1 dashboard average", q(sum(E1) / 6 * 100, 2), D("91.67"))
    e1 = D(1)
    for x in E1:
        e1 *= x
    check("EX 16.1 conjunction", q(e1 * 100, 2), D("58.93"))
    check("EX 16.1 gap in percentage points", q((sum(E1) / 6 - e1) * 100, 2), D("32.73"))
    check("EX 16.1 multiplier from lifting 0.86 to 0.99", q(D("0.99") / D("0.86"), 4),
          D("1.1512"))
    check("EX 16.1 conjunction after lifting 0.86 to 0.99",
          q(e1 * D("0.99") / D("0.86") * 100, 2), D("67.84"))
    for p, printed in ((D("0.86"), "8.91"), (D("0.95"), "2.48"), (D("0.88"), "7.37"),
                       (D("0.92"), "4.48"), (D("0.90"), "5.89")):
        check(f"EX 16.1 gain from lifting {p} to 0.99, pp",
              q(e1 * (D("0.99") / p - 1) * 100, 2), D(printed))
    check("EX 16.1 INVARIANT the 0.86 condition is the largest single gain",
          gt(e1 * (D("0.99") / D("0.86") - 1), e1 * (D("0.99") / D("0.88") - 1)), 1)
    check("EX 16.1 per-condition probability for a 90 % conjunction",
          q(D("0.90") ** (D(1) / 6) * 100, 2), D("98.26"))
    check("EX 16.1 a point closed at 0.86 against a point closed at 0.99",
          q(D("0.99") / D("0.86"), 2), D("1.15"))

    # 16.2 final account
    S2, V2, ST2, RC2 = D(2450000), D(138000), D(55000), D(26400)
    R2 = S2 * D("0.05")
    check("EX 16.2 retention", R2, D(122500))
    check("EX 16.2 certified works", S2 + V2, D(2588000))
    check("EX 16.2 paid on certification", S2 + V2 - R2, D(2465500))
    check("EX 16.2 takeover release", R2 / 2, D(61250))
    PAID2 = S2 + V2 - R2 + R2 / 2
    check("EX 16.2 amounts paid to date", PAID2, D(2526750))
    FIN2 = S2 + V2 + ST2 - RC2
    check("EX 16.2 final account", FIN2, D(2616600))
    check("EX 16.2 final retention release", R2 / 2 - RC2, D(34850))
    check("EX 16.2 final payment", FIN2 - PAID2, D(89850))
    check("EX 16.2 INVARIANT final payment reconciles to the movements",
          R2 / 2 + ST2 - RC2, FIN2 - PAID2)
    check("EX 16.2 common error understates growth by", FIN2 - (S2 + V2), D(28600))
    check("EX 16.2 that understatement as a share of the original sum",
          q((FIN2 - (S2 + V2)) / S2 * 100, 2), D("1.17"))

    # 16.3 carrying cost
    C3 = D(2900) + D("1.2") * D(750) + D(240000) * D("0.06") / 12
    check("EX 16.3 internal management component", D("1.2") * D(750), D(900))
    check("EX 16.3 opportunity-cost component", D(240000) * D("0.06") / 12, D(1200))
    check("EX 16.3 monthly carrying cost", C3, D(5000))
    check("EX 16.3 probabilities sum to one", D("0.30") + D("0.45") + D("0.25"), 1)
    EMV3 = D("0.30") * D(30000) + D("0.45") * D(55000) + D("0.25") * D(96000)
    check("EX 16.3 EMV component 0.30 x 30,000", D("0.30") * D(30000), D(9000))
    check("EX 16.3 EMV component 0.45 x 55,000", D("0.45") * D(55000), D(24750))
    check("EX 16.3 EMV component 0.25 x 96,000", D("0.25") * D(96000), D(24000))
    check("EX 16.3 EMV of determination", EMV3, D(57750))
    check("EX 16.3 expected cost of determination", EMV3 + 11 * C3, D(112750))
    check("EX 16.3 breakeven settlement price at month 1", EMV3 + 11 * C3 - C3, D(107750))
    check("EX 16.3 breakeven premium over the client's assessment",
          EMV3 + 10 * C3 - D(55000), D(52750))
    check("EX 16.3 INVARIANT the premium identity reproduces the price route",
          10 * C3 + (EMV3 - D(55000)), EMV3 + 11 * C3 - C3 - D(55000))
    check("EX 16.3 internal management as a share of the carrying cost",
          q(D("1.2") * D(750) / C3 * 100, 0), D(18))
    check("EX 16.3 common error understates the premium by", 10 * D("1.2") * D(750), D(9000))

    # 16.4 benefit decomposition
    U4 = D(60) * D(92) * D(46)
    check("EX 16.4 U", U4, D(253920))
    check("EX 16.4 planned annual benefit", U4 * D("0.75") * D("4.0"), D(761760))
    M4 = U4 * D("0.66") * D("3.6")
    check("EX 16.4 measured annual benefit", M4, D("603313.92"))
    check("EX 16.4 measured as a share of plan",
          q(M4 / (U4 * D("0.75") * D("4.0")) * 100, 2), D("79.20"))
    check("EX 16.4 total variance", M4 - U4 * D("0.75") * D("4.0"), D("-158446.08"))
    A4F = U4 * D("4.0") * (D("0.66") - D("0.75"))
    H4S = U4 * D("0.66") * (D("3.6") - D("4.0"))
    check("EX 16.4 adoption effect, adoption first", A4F, D("-91411.20"))
    check("EX 16.4 hours effect at measured adoption", H4S, D("-67034.88"))
    check("EX 16.4 INVARIANT adoption-first order sums to the total variance",
          A4F + H4S, M4 - U4 * D("0.75") * D("4.0"))
    H4F = U4 * D("0.75") * (D("3.6") - D("4.0"))
    A4S = U4 * D("3.6") * (D("0.66") - D("0.75"))
    check("EX 16.4 hours effect, hours first", H4F, D("-76176.00"))
    check("EX 16.4 adoption effect at measured hours", A4S, D("-82270.08"))
    check("EX 16.4 INVARIANT hours-first order sums to the same total",
          H4F + A4S, M4 - U4 * D("0.75") * D("4.0"))
    I4 = U4 * (D("0.66") - D("0.75")) * (D("3.6") - D("4.0"))
    check("EX 16.4 interaction term", I4, D("9141.12"))
    check("EX 16.4 INVARIANT the interaction is what moves between the orders", H4S - H4F, I4)
    check("EX 16.4 common error (both variances at plan)", A4F + H4F, D("-167587.20"))
    check("EX 16.4 INVARIANT that error overstates by exactly the interaction",
          (A4F + H4F) - (M4 - U4 * D("0.75") * D("4.0")), -I4)

    # 16.5 review economics
    REV5, L5, AV5 = D(62000), D(41), D(19500)
    ADDR5 = L5 * AV5
    check("EX 16.5 addressable value", ADDR5, D(799500))
    check("EX 16.5 expected recovery at 9 %", ADDR5 * D("0.09"), D(71955))
    check("EX 16.5 cost-recovery multiple at 9 %", q(ADDR5 * D("0.09") / REV5, 2), D("1.16"))
    check("EX 16.5 breakeven retrieval rate", q(REV5 / ADDR5 * 100, 2), D("7.75"))
    check("EX 16.5 expected recovery at 30 %", ADDR5 * D("0.30"), D(239850))
    check("EX 16.5 increment from raising retrieval to 30 %", ADDR5 * D("0.21"), D(167895))
    check("EX 16.5 return on the 24,000 index", q(ADDR5 * D("0.21") / D(24000), 2), D("7.00"))
    check("EX 16.5 combined breakeven retrieval rate",
          q((REV5 + D(24000)) / ADDR5 * 100, 2), D("10.76"))
    check("EX 16.5 net value at 30 % retrieval", ADDR5 * D("0.30") - REV5 - D(24000),
          D(153850))
    check("EX 16.5 INVARIANT 9 % retrieval already exceeds the breakeven rate",
          gt(D("0.09"), REV5 / ADDR5), 1)
    check("EX 16.5 INVARIANT indexing returns more per dollar than the review itself",
          gt(ADDR5 * D("0.21") / D(24000), ADDR5 * D("0.09") / REV5), 1)
