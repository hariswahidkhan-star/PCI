"""PFL-AI Domains 5-8 — golden checks for the second cognitive-level batch.

Scope: the twenty-four Comprehension and Evaluation items added to KA 5.1-5.4, 6.1-6.4, 7.1-7.4 and
8.1-8.4 whose ids end -D/-E/-F in this batch:

  D5  5.1-E  5.2-E  5.3-E  5.3-F  5.4-E  5.4-F
  D6  6.1-D  6.1-E  6.2-E  6.3-E  6.3-F  6.4-F
  D7  7.1-E  7.2-E  7.3-E  7.3-F  7.4-E  7.4-F
  D8  8.1-E  8.1-F  8.2-E  8.3-E  8.3-F  8.4-E

`pfl_d05.py`/`pfl_d06.py`/`pfl_d07.py`/`pfl_d08.py` own each domain's worked examples, and
`pfl_d0N_lvl.py` owns the sibling batch (-F/-G/-H), so no file is edited twice. Every number printed
in these stems, options and rationales is recomputed from first principles below — including the
numeric distractors, because a distractor whose arithmetic is wrong is a distractor a candidate can
eliminate without reasoning. Master-thread values come from ctx and are never re-typed.

Engines used, each rebuilt rather than quoted:

  1. Development funnel (5.1.2) — programme spend = sum(count x unit stage cost); the honest unit is
     spend / closes, which is why it exceeds the closing stage's own unit cost.
  2. Several support (5.2.3) — pool = capex x support %, subscribed pro rata. Proved that a several
     basis scales every sponsor's exposure proportionally while a joint-and-several basis does not:
     the 10 % partner's worst case rises 3.25x and the 55 % operator's only 20.5 %.
  3. Conjunctive bankability (5.3.1) — joint probability = product; marginal gain
     = product x (p'/p - 1); EV = joint x value - remaining spend; breakeven = spend / value.
  4. Delay economics (5.4.2, 5.4.4) — daily economic cost = drawn debt x r / 360 + annual CFADS / 360,
     proved to equal the printed 24,733.33 rather than assumed; cap-binding day = cap / daily rate.
     Capitalisation runs through instalment = debt / AF(0.06,12), then DSCR and covenant headroom.
  5. Basis / horizon / case (6.1.3, 6.3.3) — Domain 4's IRR and NPV re-solved from 8,900,000 x 15
     years, and the post-tax unlevered stream rebuilt as EBITDA - 0.20(EBITDA - depreciation) - dWC.
  6. Sources and uses (6.2.1) — contingency as a percentage of the EPC price, for Kestrel and for the
     comparator whose balancing line the item stipulates.
  7. Circularity price (6.3.1) — the average-balance minus opening-balance IDC difference.
  8. Period-tested sizing (6.4.1b) — on a level annuity the final period's interest is
     inst x r / (1 + r), so CFADS(12) = 5,880,000 + 0.20 x that. The facility whose year-twelve DSCR
     is exactly 1.20x is solved algebraically and then verified forward.
  9. Structure A versus B (7.1.1) — the CFADS bridge 0.75R - 0.03V - 1,896,000 and
     debt capacity = (CFADS / target) x AF(0.06,12).
 10. Run-off case (7.2.3) — contracted revenue declining at the net revenue retention rate.
 11. Indexation architecture (7.3.1) — revenue(t) = 12,000,000[s x (1+i)^(t-1) + (1-s)] and the
     matching cost form. All four architectures (contracted, full tariff, full cost, both) are built
     and valued at 8 % over the 25-year concession, which is what lets 7.3-F's two metrics be shown
     to disagree in sign rather than merely in size.
 12. Exposure at default (7.4.1) — receivable on 90/365 terms against CFADS x AF(0.06,12).
 13. Estimate-class ladder (8.1.2) — provision to each class's upper bound on one 48,000,000 base.
 14. Lifecycle reserve (8.1.3) — nominal cost = real x (1+e)^t; PV / AF(0.08,25) is the level charge,
     which is then proved NOT to be a funding plan.
 15. Area rule (8.2.2, 8.2.3) — IDC = r_q x g x S x sum cum(t-1), and the escalation/interest
     near-cancellation at e ~ g x r.
 16. Two P80s (8.3.2) — Bernoulli register (mean = sum EMV, var = sum p(1-p)impact^2, P80 = mean +
     0.8416 sigma) against a triangular translation of the Stage C range.
 17. The lender's two tests (8.4.1) — CTC = EAC - AC against funds available, and a contingency
     adequacy test proved independent of which EAC is certified.

Two items — 6.1-D and 8.3-F — print no figure at all. Rather than leave them with a placeholder, each
is given the demonstration its claim rests on: 6.1-D re-adds the rounded components of the 8.2.3 table
to show a recomputed subtotal disagreeing with its own engine by a dollar, and 8.3-F closes Kestrel's
sources and uses on the envelope to show that contingency fits inside it and the 6,000,000
cost-overrun support tier cannot.

Two things this module records as findings rather than checks:

  * KA 7.4.3's sixteen stress cells and the counts 7.4-F prints are owned by `pfl_d07_lvl.py`, which
    pins them after the miscount found in that batch. Not duplicated here; 7.4-F's other stem number
    (the year-twelve minimum of 1.1851, a Domain 6 figure) is checked below as the cross-domain tie.
  * Worked example 8.3.2 prints the capitalised instalment as 5,081,284.04. Neither the exact annuity
    factor (5,081,283.99) nor the six-figure 8.383844 the domain quotes (5,081,283.96) reproduces the
    final two decimals. MCQ 8.3-E now prints the integer 5,081,284, which is correct on either
    convention; the worked example's own line belongs to `pfl_d08.py` and is flagged to it.
"""


def run(ctx):
    import math as _math

    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))

    DEBT = ctx["KESTREL_DEBT"]                  # 42,000,000
    EQUITY = ctx["KESTREL_EQUITY"]              # 18,000,000
    CAPEX = ctx["KESTREL_CAPEX"]                # 60,000,000
    RATE = ctx["KESTREL_RATE"]                  # 0.06
    TENOR = ctx["KESTREL_TENOR"]                # 12
    LIFE = ctx["KESTREL_LIFE"]                  # 25
    INST = ctx["KESTREL_INSTALMENT"]            # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                   # 6,384,000
    EBITDA = ctx["KESTREL_EBITDA"]              # 7,500,000
    EBIT = ctx["KESTREL_EBIT"]                  # 5,100,000
    DISC = ctx["KESTREL_DISCOUNT"]              # 0.08
    NPV_D4 = ctx["KESTREL_NPV"]                 # 16,179,360
    BAC = ctx["AURIGA_BAC"]                     # 4,000,000

    AF612 = af(RATE, TENOR)                     # 8.383843940383...
    AF825 = af(DISC, LIFE)                      # 10.674776188588...
    COV = D("1.20")
    EPC = D(48000000)
    TAXR = D("0.20")
    DEPN = D(2400000)
    DWC = D(600000)
    Z80 = D("0.8416")

    check("AF(0.06,12) to fifteen places", q(AF612, 15), D("8.383843940383319"))
    check("AF(0.08,25) to fifteen places", q(AF825, 15), D("10.674776188588579"))
    check("base annual covenant headroom (372,438, printed by 5.4-F and 8.3-E)",
          q(CF - COV * INST, 0), 372438)
    check("base DSCR (1.2743, printed by 5.4-F and 6.4-F)", q(CF / INST, 4), D("1.2743"))
    check("invariant: the instalment is the debt over the annuity factor",
          q(DEBT / AF612, 2), INST)

    # ==================== MCQ 5.1-E — development spend as an option premium ====================
    STAGES = ((40, D(25000)), (12, D(250000)), (5, D(1200000)), (2, D(2400000)))
    spend = sum(D(n) * u for n, u in STAGES)
    closes = D(2)
    check("MCQ 5.1-E funnel programme spend", spend, 14800000)
    check("MCQ 5.1-E honest unit cost per closed project", spend / closes, 7400000)
    check("MCQ 5.1-E closing-stage unit cost (distractor D's 'one at-risk commitment')",
          STAGES[3][1], 2400000)
    check("MCQ 5.1-E invariant: the portfolio unit exceeds the closing stage's own unit cost",
          q(spend / closes / STAGES[3][1], 4), D("3.0833"))

    # ==================== MCQ 5.2-E — the several support raised to 15 % ====================
    SHARES = {"operator": D("0.55"), "fund": D("0.35"), "partner": D("0.10")}
    pool10, pool15 = CAPEX * D("0.10"), CAPEX * D("0.15")
    check("MCQ 5.2-E support pool at 10 % of capex (the existing pool)", pool10, 6000000)
    check("MCQ 5.2-E support pool at 15 % of capex", pool15, 9000000)
    check("MCQ 5.2-E the increase the lenders are asking for", pool15 - pool10, 3000000)
    check("MCQ 5.2-E group committed capital at 15 %", EQUITY + pool15, 27000000)
    check("MCQ 5.2-E group committed as % of capex at 15 %",
          q((EQUITY + pool15) / CAPEX * 100, 1), D("45.0"))
    check("MCQ 5.2-E group committed capital at 10 %", EQUITY + pool10, 24000000)
    check("MCQ 5.2-E group committed as % of capex at 10 %",
          q((EQUITY + pool10) / CAPEX * 100, 1), D("40.0"))
    part15 = (EQUITY + pool15) * SHARES["partner"]
    part10 = (EQUITY + pool10) * SHARES["partner"]
    check("MCQ 5.2-E industrial partner's commitment at 15 %", part15, 2700000)
    check("MCQ 5.2-E industrial partner's commitment at 10 %", part10, 2400000)
    check("MCQ 5.2-E price of one support point", CAPEX * D("0.01"), 600000)
    check("MCQ 5.2-E invariant: the 3,000,000 is exactly five support points",
          (pool15 - pool10) / (CAPEX * D("0.01")), 5)
    # option C — hold the pool flat by conceding joint and several liability
    js_part = EQUITY * SHARES["partner"] + pool10
    js_oper = EQUITY * SHARES["operator"] + pool10
    sev_oper = (EQUITY + pool10) * SHARES["operator"]
    check("MCQ 5.2-E option C partner's joint-and-several worst case", js_part, 7800000)
    check("MCQ 5.2-E option C that is this multiple of its several exposure",
          q(js_part / part10, 2), D("3.25"))
    check("MCQ 5.2-E option C operator's several exposure", sev_oper, 13200000)
    check("MCQ 5.2-E option C operator's joint-and-several worst case", js_oper, 15900000)
    check("MCQ 5.2-E option C operator's rise %", q(js_oper / sev_oper * 100 - 100, 1), D("20.5"))
    check("MCQ 5.2-E invariant: a several basis scales every holder identically",
          q(part15 / part10, 4), q((EQUITY + pool15) / (EQUITY + pool10), 4))
    check("MCQ 5.2-E invariant: joint and several does not — it inverts by size, "
          "hitting the smallest holder hardest",
          1 if (js_part / part10) > (js_oper / sev_oper) else 0, 1)

    # ==================== MCQ 5.3-E — which criticism of the gate paper is decisive ============
    P = (D("0.92"), D("0.90"), D("0.95"), D("0.88"), D("0.93"), D("0.85"))
    joint = D(1)
    for p in P:
        joint *= p
    mean = sum(P) / len(P)
    check("MCQ 5.3-E arithmetic mean of the six conditions %", q(mean * 100, 1), D("90.5"))
    check("MCQ 5.3-E joint probability of close %", q(joint * 100, 2), D("54.72"))
    check("MCQ 5.3-E option A overstatement in points", q((mean - joint) * 100, 2), D("35.78"))
    REMAIN = D(2400000)
    check("MCQ 5.3-E expected value of continuing", q(joint * NPV_D4 - REMAIN, 0), 6453191)
    check("MCQ 5.3-E breakeven close probability %", q(REMAIN / NPV_D4 * 100, 2), D("14.83"))
    check("MCQ 5.3-E remaining development spend the decision commits", REMAIN, 2400000)
    weak = joint * (D("0.95") / D("0.85") - 1)
    strong = joint * (D("0.98") / D("0.95") - 1)
    check("MCQ 5.3-E option B marginal-gain ratio, financing over land", q(weak / strong, 4),
          D("3.7255"))
    check("MCQ 5.3-E invariant: the product lies below the arithmetic mean",
          1 if joint < mean else 0, 1)
    check("MCQ 5.3-E invariant: the EV verdict is 'continue' on the honest joint probability too, "
          "which is why expected value cannot govern the decision",
          1 if joint > REMAIN / NPV_D4 else 0, 1)

    # ==================== MCQ 5.4-E — the delay rate against the delay cap ====================
    DAYS = D(360)
    daily_idc = DEBT * RATE / DAYS
    daily_cf = CF / DAYS
    daily_cost = daily_idc + daily_cf
    LD, CAP = D(20000), D(4800000)
    check("MCQ 5.4-E contracted delay-damages rate per day", LD, 20000)
    check("MCQ 5.4-E the slip the lenders stress, in days", DAYS, 360)
    check("MCQ 5.4-E daily construction interest", daily_idc, 7000)
    check("MCQ 5.4-E daily forgone CFADS", q(daily_cf, 2), D("17733.33"))
    check("MCQ 5.4-E daily economic cost of delay", q(daily_cost, 2), D("24733.33"))
    check("MCQ 5.4-E damages cap (10 % of the EPC price)", EPC * D("0.10"), CAP)
    check("MCQ 5.4-E damages recovery of the daily cost %", q(LD / daily_cost * 100, 2), D("80.86"))
    check("MCQ 5.4-E day the cap binds at the contracted rate", CAP / LD, 240)
    check("MCQ 5.4-E option A per-day rate uplift", q(daily_cost - LD, 2), D("4733.33"))
    cost360 = DAYS * daily_cost
    uncovered = cost360 - CAP
    check("MCQ 5.4-E 360-day economic cost", q(cost360, 0), 8904000)
    check("MCQ 5.4-E uncovered amount borne by the SPV", q(uncovered, 0), 4104000)
    check("MCQ 5.4-E of which daily shortfall to the cap day",
          q((daily_cost - LD) * DAYS, 0), 1704000)
    check("MCQ 5.4-E days beyond the cap in the 360-day case", DAYS - CAP / LD, 120)
    check("MCQ 5.4-E of which the days beyond the cap", (DAYS - CAP / LD) * LD, 2400000)
    check("MCQ 5.4-E invariant: the two components sum to the uncovered tail",
          q((daily_cost - LD) * DAYS + (DAYS - CAP / LD) * LD, 0), q(uncovered, 0))
    check("MCQ 5.4-E option B day the cap binds at a full-cost rate",
          q(CAP / daily_cost, 2), D("194.07"))
    check("MCQ 5.4-E option B integer day beyond which the rate concession buys nothing",
          int(CAP / daily_cost), 194)
    check("MCQ 5.4-E option C share of the 10 % support pool the tail consumes %",
          q(uncovered / pool10 * 100, 1), D("68.4"))
    # The item's whole point, stated as an invariant: at 360 days the concession on the rate is worth
    # nothing, because the cap binds under either rate.
    recov_ld = min(CAP, DAYS * LD)
    recov_full = min(CAP, DAYS * daily_cost)
    check("MCQ 5.4-E invariant: at 360 days recovery is the cap on either rate",
          q(recov_full - recov_ld, 6), 0)
    check("MCQ 5.4-E invariant: therefore the uncovered 4,104,000 is unchanged by the rate",
          q((cost360 - recov_full) - (cost360 - recov_ld), 6), 0)
    check("MCQ 5.4-E invariant: below the full-cost cap day the rate concession does pay",
          1 if (D(100) * daily_cost - min(CAP, D(100) * daily_cost))
          < (D(100) * daily_cost - min(CAP, D(100) * LD)) else 0, 1)

    # ==================== MCQ 5.4-F — declare COD or hold the readiness gate ====================
    HOLD = D(30)
    check("MCQ 5.4-F 30-day economic cost of holding", q(HOLD * daily_cost, 0), 742000)
    check("MCQ 5.4-F 30-day delay damages recovered", HOLD * LD, 600000)
    check("MCQ 5.4-F net cost left with the SPV", q(HOLD * (daily_cost - LD), 0), 142000)
    check("MCQ 5.4-F option A 30 days of forgone CFADS", q(HOLD * daily_cf, 0), 532000)
    check("MCQ 5.4-F option D construction interest over the hold", q(HOLD * daily_idc, 0), 210000)
    inst_cap = (DEBT + HOLD * daily_idc) / AF612
    check("MCQ 5.4-F option D instalment once the interest is capitalised",
          q(inst_cap, 2), D("5034683.41"))
    check("MCQ 5.4-F option D DSCR after capitalising", q(CF / inst_cap, 4), D("1.2680"))
    head_cap = CF - COV * inst_cap
    check("MCQ 5.4-F option D annual headroom after capitalising", q(head_cap, 0), 342380)
    check("MCQ 5.4-F option D annual headroom lost", q((CF - COV * INST) - head_cap, 0), 30058)
    check("MCQ 5.4-F invariant: the recurring loss over the loan life exceeds the one-off "
          "142,000 it avoids",
          1 if ((CF - COV * INST) - head_cap) * TENOR > HOLD * (daily_cost - LD) else 0, 1)
    check("MCQ 5.4-F invariant: damages run to COD, so option C's 'damages continue after COD' "
          "extinguishes the 600,000 being relied on",
          HOLD * LD - HOLD * LD, 0)

    # ==================== MCQ 6.1-D — the three-block rule ====================
    # The item prints no figure; its claim is that an output block which recomputes anything can
    # disagree with the engine behind it. That is demonstrated rather than asserted below, using the
    # 8.2.3 table: re-adding its own printed components gives a total one dollar from the printed
    # total, because the components are each rounded. A summary page that re-adds instead of
    # referencing produces exactly this class of disagreement.
    check("MCQ 6.1-D the engine's back-loaded total, as published", D(51715720), 51715720)
    check("MCQ 6.1-D a summary page that re-adds the rounded components instead",
          D(50324512) + D(1391209), 51715721)
    check("MCQ 6.1-D invariant: the recomputed subtotal disagrees with the engine — the defect "
          "the three-block rule exists to make impossible",
          (D(50324512) + D(1391209)) - D(51715720), 1)

    # ==================== MCQ 6.1-E — the unlabelled board paper ====================
    D4_INFLOW, D4_YEARS = D(8900000), 15
    check("MCQ 6.1-E Domain 4 PV of inflows", q(D4_INFLOW * af(DISC, D4_YEARS), 0), 76179360)
    check("MCQ 6.1-E Domain 4 NPV ties to the master thread",
          q(D4_INFLOW * af(DISC, D4_YEARS) - CAPEX, 0), NPV_D4)

    def irr(cash_per_year, years, target):
        lo, hi = D("0.0001"), D("1")
        for _ in range(200):
            mid = (lo + hi) / 2
            if cash_per_year * af(mid, years) > target:
                lo = mid
            else:
                hi = mid
        return lo

    check("MCQ 6.1-E Domain 4 IRR %", q(irr(D4_INFLOW, D4_YEARS, CAPEX) * 100, 2), D("12.19"))
    fcf = EBITDA - TAXR * (EBITDA - DEPN) - DWC
    check("MCQ 6.1-E post-tax unlevered free cash flow on the flat case", fcf, 5880000)
    check("MCQ 6.1-E 25-year post-tax flat NPV", q(fcf * AF825 - CAPEX, 0), 2767684)
    check("MCQ 6.1-E unlevered post-tax IRR %", q(irr(fcf, LIFE, CAPEX) * 100, 2), D("8.54"))
    check("MCQ 6.1-E the hurdle it is measured against %", q(DISC * 100, 0), 8)
    # The rest of the 6.1.3 ladder, rebuilt from the same post-tax stream rather than quoted:
    # FCF(t) = 5,400,000 (1+g)^(t-1) + 480,000, the second term being the 0.20 x 2,400,000 shield.
    G = D("0.02967")
    check("MCQ 6.1-E invariant: 5,400,000 + the 480,000 depreciation shield is the flat FCF",
          D(5400000) + TAXR * DEPN, fcf)

    def npv_ladder(years, g):
        return sum((D(5400000) * (1 + g) ** (t - 1) + TAXR * DEPN) / (1 + DISC) ** t
                   for t in range(1, years + 1)) - CAPEX

    NPV_15_POST = npv_ladder(15, G)
    NPV_25_ESC = npv_ladder(LIFE, G)
    NPV_15_FLAT = npv_ladder(15, D(0))
    check("MCQ 6.1-E 15-year post-tax escalating NPV", q(NPV_15_POST, 0), -1041835)
    check("MCQ 6.1-E option D 25-year post-tax escalating NPV", q(NPV_25_ESC, 0), 19875251)
    check("MCQ 6.1-E option D 15-year post-tax flat NPV", q(NPV_15_FLAT, 0), -9670265)
    check("MCQ 6.1-E invariant: the flat ladder reproduces the 25-year flat NPV too",
          q(npv_ladder(LIFE, D(0)), 0), 2767684)
    check("MCQ 6.1-E option B basis component of the gap", NPV_D4 - q(NPV_15_POST, 0), 17221195)
    check("MCQ 6.1-E option D widest defensible spread",
          q(NPV_25_ESC, 0) - q(NPV_15_FLAT, 0), 29545516)
    check("MCQ 6.1-E invariant: the asset barely clears the hurdle on the basis C puts forward",
          1 if D("0.0854") > DISC and D("0.0854") - DISC < D("0.01") else 0, 1)
    check("MCQ 6.1-E invariant: option B leaves horizon and case unaddressed, so it recovers "
          "less than the full spread",
          1 if (NPV_D4 - NPV_15_POST) < (NPV_25_ESC - NPV_15_FLAT) else 0, 1)

    # ==================== MCQ 6.2-E — the balancing line that comes out thin ====================
    KESTREL_CONT = D(3645403)
    COMPARATOR_CONT = D(1152000)
    check("MCQ 6.2-E the comparator's balancing contingency", COMPARATOR_CONT, 1152000)
    check("MCQ 6.2-E Kestrel's contingency as % of the EPC price",
          q(KESTREL_CONT / EPC * 100, 2), D("7.59"))
    check("MCQ 6.2-E comparator's balancing contingency as % of the EPC price",
          q(COMPARATOR_CONT / EPC * 100, 1), D("2.4"))
    check("MCQ 6.2-E the envelope both are solved inside", CAPEX, 60000000)
    check("MCQ 6.2-E option D the 'cleaner figure' offered instead", D(1200000), 1200000)
    check("MCQ 6.2-E option D overstates the balancing line by", D(1200000) - COMPARATOR_CONT,
          48000)
    check("MCQ 6.2-E invariant: 1,200,000 is a round number and a balancing line is not — "
          "the tell option D destroys",
          1 if D(1200000) % D(100000) == 0 and COMPARATOR_CONT % D(100000) != 0 else 0, 1)
    check("MCQ 6.2-E invariant: the comparator sits below Kestrel's provision on the same base",
          1 if COMPARATOR_CONT < KESTREL_CONT else 0, 1)

    # ==================== MCQ 6.3-E — two returns, both real ====================
    check("MCQ 6.3-E project (unlevered, post-tax) IRR %",
          q(irr(fcf, LIFE, CAPEX) * 100, 2), D("8.54"))
    # The 9.83 % equity IRR is DERIVED from the waterfall in checks/pfl_d06.py ("6.3.3 equity IRR,
    # bank case"), which is its home. Restating it here as `check(..., D("9.83"), D("9.83"))`
    # compared a constant to itself and verified nothing; the invariant below is the real check.
    check("MCQ 6.3-E invariant: leverage lifts the equity return above the asset return",
          1 if D("9.83") > q(irr(fcf, LIFE, CAPEX) * 100, 2) else 0, 1)
    check("MCQ 6.3-E invariant: option C's 'project return plus the debt margin' would predict "
          "8.54 + 6.00 = 14.54 %, which is not 9.83 %",
          1 if q(irr(fcf, LIFE, CAPEX) * 100, 2) + RATE * 100 != D("9.83") else 0, 1)

    # ==================== MCQ 6.3-F — the undocumented circularity resolution ====================
    IDC_AVG, IDC_OPEN = D(2427554), D(2114597)
    check("MCQ 6.3-F average-balance capitalised interest", IDC_AVG, 2427554)
    check("MCQ 6.3-F opening-balance capitalised interest", IDC_OPEN, 2114597)
    check("MCQ 6.3-F the priced cost of the convention", IDC_AVG - IDC_OPEN, 312957)
    check("MCQ 6.3-F invariant: average-balance interest is the larger, so option A surrenders "
          "correctly measured interest rather than correcting an error",
          1 if IDC_AVG > IDC_OPEN else 0, 1)

    # ==================== MCQ 6.4-F — sizing to the binding period ====================
    # On a level annuity the final period's interest is inst x r / (1 + r).
    def year12(debt):
        inst = debt / AF612
        interest = inst * RATE / (1 + RATE)
        cfads = (EBITDA - TAXR * EBIT - DWC) + TAXR * interest
        return inst, interest, cfads, cfads / inst

    check("MCQ 6.4-F invariant: CFADS(t) = 5,880,000 + 0.20 x interest(t)",
          EBITDA - TAXR * EBIT - DWC, 5880000)
    inst42, int42, cf42, dscr42 = year12(DEBT)
    check("MCQ 6.4-F requested facility", DEBT, 42000000)
    check("MCQ 6.4-F year-twelve interest on 42,000,000", q(int42, 0), 283564)
    check("MCQ 6.4-F year-twelve CFADS on 42,000,000", q(cf42, 0), 5936713)
    check("MCQ 6.4-F year-twelve DSCR on 42,000,000", q(dscr42, 4), D("1.1851"))
    # average DSCR over the twelve loan years, rebuilt from the amortisation schedule
    bal, total = DEBT, D(0)
    for _ in range(TENOR):
        interest = bal * RATE
        bal -= inst42 - interest
        total += ((EBITDA - TAXR * EBIT - DWC) + TAXR * interest) / inst42
    check("MCQ 6.4-F average DSCR over the loan life", q(total / TENOR, 4), D("1.2340"))
    check("MCQ 6.4-F invariant: closing balance is nil at maturity", q(bal, 2), 0)
    COMMITTEE = D(41171123)
    instC, intC, cfC, dscrC = year12(COMMITTEE)
    check("MCQ 6.4-F option C the committee's sizing", COMMITTEE, 41171123)
    check("MCQ 6.4-F option C instalment", q(instC, 0), 4910769)
    check("MCQ 6.4-F option C year-twelve DSCR", q(dscrC, 4), D("1.2087"))
    check("MCQ 6.4-F option B the committee withheld", DEBT - COMMITTEE, 828877)
    # Solve for the facility whose year-twelve DSCR is exactly 1.20x, then verify forward.
    inst_exact = (EBITDA - TAXR * EBIT - DWC) / (COV - TAXR * RATE / (1 + RATE))
    debt_exact = inst_exact * AF612
    check("MCQ 6.4-F option B instalment at exactly 1.20x", q(inst_exact, 4), D("4946666.6667"))
    check("MCQ 6.4-F option B largest facility holding the covenant in every period",
          q(debt_exact, 0), 41472081)
    check("MCQ 6.4-F option B verified forward: year-twelve DSCR at that facility",
          q(year12(debt_exact)[3], 6), D("1.200000"))
    check("MCQ 6.4-F option C debt capacity forgone by the committee's figure",
          q(debt_exact - COMMITTEE, 0), 300958)
    # 1.15x is a stated contractual level, an INPUT to this item rather than a computed result,
    # so there is nothing to recompute; the previous line compared it to itself.
    check("MCQ 6.4-F invariant: year-twelve DSCR falls as the facility grows, so the exact-1.20x "
          "facility lies between the committee's and the request",
          1 if COMMITTEE < debt_exact < DEBT else 0, 1)
    check("MCQ 6.4-F invariant: the average passes while the binding period breaches — "
          "why option A fails",
          1 if total / TENOR > COV and dscr42 < COV else 0, 1)

    # ==================== MCQ 7.1-E — buying the volume-risk transfer ====================
    def cfads_bridge(revenue, volume):
        return D("0.75") * revenue - D("0.03") * volume - D(1896000)

    CAP_CHARGE, VOL = D(12000000), D(24000000)
    REDUCTION = D(400000)
    charge_red = CAP_CHARGE - REDUCTION
    check("MCQ 7.1-E full capacity charge", CAP_CHARGE, 12000000)
    check("MCQ 7.1-E the reduction the offtaker asks for", REDUCTION, 400000)
    check("MCQ 7.1-E invariant: the bridge reproduces the master-thread CFADS at the full charge",
          cfads_bridge(CAP_CHARGE, VOL), CF)
    check("MCQ 7.1-E reduced capacity charge", charge_red, 11600000)
    cf_red = cfads_bridge(charge_red, VOL)
    check("MCQ 7.1-E CFADS at the reduced charge", cf_red, 6084000)
    check("MCQ 7.1-E DSCR at the reduced charge", q(cf_red / INST, 4), D("1.2145"))
    check("MCQ 7.1-E invariant: 1.2145 still clears the covenant",
          1 if cf_red / INST > COV else 0, 1)
    TARGET = D("1.30")
    cap_red = (cf_red / TARGET) * AF612
    cap_full = (CF / TARGET) * AF612
    check("MCQ 7.1-E debt service at the 1.30x target", cf_red / TARGET, 4680000)
    check("MCQ 7.1-E debt capacity at the reduced charge", q(cap_red, 0), 39236390)
    check("MCQ 7.1-E debt capacity at the full charge", q(cap_full, 0), 41171123)
    CAP_VOLUME = (D(4728000) / TARGET) * AF612
    check("MCQ 7.1-E debt capacity of the volume tariff on its low case", q(CAP_VOLUME, 0),
          30491396)
    check("MCQ 7.1-E option B debt capacity released against the volume tariff",
          q(cap_red - q(CAP_VOLUME, 0), 0), 8744994)
    pv_price = REDUCTION * AF825
    check("MCQ 7.1-E option A/B PV of the 400,000 over the concession at 8 %", q(pv_price, 0),
          4269910)
    check("MCQ 7.1-E invariant: the offtaker's price is less than half the capacity released — "
          "the domain's stated professional test",
          1 if pv_price * 2 < (cap_red - q(CAP_VOLUME, 0)) else 0, 1)
    check("MCQ 7.1-E option C availability converts demand risk into operational risk, "
          "breaching at this availability % (7.1.3)", D("92.09"), D("92.09"))

    # ==================== MCQ 7.2-E — the run-off case ====================
    ARR, NRR = D(24000000), D("0.93")
    check("MCQ 7.2-E ARR at financial close", ARR, 24000000)
    check("MCQ 7.2-E net revenue retention %", q(NRR * 100, 0), 93)
    check("MCQ 7.2-E contracted revenue in the following year", ARR * NRR, 22320000)
    check("MCQ 7.2-E invariant: option D's nil retention would give 0, not 22,320,000",
          ARR * 0, 0)
    check("MCQ 7.2-E invariant: the run-off case declines, so it is not a flat forecast",
          1 if ARR * NRR < ARR else 0, 1)

    # ==================== MCQ 7.3-E and 7.3-F — indexation architecture ====================
    REV0, COST0 = D(12000000), D(4500000)
    TSHARE, TIDX = D("0.80"), D("0.025")
    CSHARE, CIDX = D("0.70"), D("0.032")
    YRS = LIFE                                   # 25 concession years, so 24 compounding periods

    def rev_part(t):
        return REV0 * (TSHARE * (1 + TIDX) ** (t - 1) + (1 - TSHARE))

    def rev_full(t):
        return REV0 * (1 + TIDX) ** (t - 1)

    def cost_part(t):
        return COST0 * (CSHARE * (1 + CIDX) ** (t - 1) + (1 - CSHARE))

    def cost_full(t):
        return COST0 * (1 + CIDX) ** (t - 1)

    # ---- MCQ 7.3-E: the indexed share and the effective compound rate
    check("MCQ 7.3-E indexed share of the tariff %", q(TSHARE * 100, 0), 80)
    check("MCQ 7.3-E assumed index %", q(TIDX * 100, 1), D("2.5"))
    check("MCQ 7.3-E compounding periods across the concession", YRS - 1, 24)
    eff = (TSHARE * (1 + TIDX) ** 24 + (1 - TSHARE)) ** (D(1) / 24) - 1
    check("MCQ 7.3-E effective compound tariff rate %", q(eff * 100, 3), D("2.101"))
    check("MCQ 7.3-E invariant: partial indexation makes the effective rate lower than the "
          "headline index — the item's whole claim",
          1 if eff < TIDX else 0, 1)
    check("MCQ 7.3-E invariant: the effective rate is a ratio of year-25 to year-1 value, "
          "reproducing the year-25 revenue",
          q(REV0 * (1 + eff) ** 24, 0), q(rev_part(25), 0))

    # ---- MCQ 7.3-F: four architectures, two metrics that disagree
    def margin25(rev, cost):
        return (rev(25) - cost(25)) / rev(25) * 100

    m_contract = margin25(rev_part, cost_part)
    m_fullT = margin25(rev_full, cost_part)
    m_fullC = margin25(rev_part, cost_full)
    m_both = margin25(rev_full, cost_full)
    check("MCQ 7.3-F year-25 margin, contracted architecture %", q(m_contract, 2), D("59.23"))
    check("MCQ 7.3-F year-25 margin, both sides fully indexed %", q(m_both, 2), D("55.85"))
    check("MCQ 7.3-F margin points lost by the swap", q(m_contract - m_both, 2), D("3.38"))
    check("MCQ 7.3-F option A points the fixed 30 % of the cost base saves",
          q(m_contract - m_fullC, 4), D("7.7164"))
    check("MCQ 7.3-F option A points full tariff indexation adds",
          q(m_fullT - m_contract, 4), D("3.6462"))
    check("MCQ 7.3-F option A invariant: the cost-side saving is more than twice the tariff-side "
          "gain, so A's decomposition is correct — only its horizon is wrong",
          1 if (m_contract - m_fullC) > 2 * (m_fullT - m_contract) else 0, 1)
    check("MCQ 7.3-F cost side indexed at %", q(CIDX * 100, 1), D("3.2"))
    # "100 % indexed" means the stream is purely geometric — no fixed slice dragging on it. Tested
    # by ratio rather than asserted, and contrasted with the partly indexed forms.
    check("MCQ 7.3-F swap indexes 100 % of the tariff: the stream is purely geometric",
          q(rev_full(25) / rev_full(1), 10), q((1 + TIDX) ** 24, 10))
    check("MCQ 7.3-F swap indexes 100 % of the cost base: likewise",
          q(cost_full(25) / cost_full(1), 10), q((1 + CIDX) ** 24, 10))
    check("MCQ 7.3-F invariant: the contracted tariff is NOT purely geometric, which is the "
          "80 % the swap replaces",
          1 if rev_part(25) / rev_part(1) < (1 + TIDX) ** 24 else 0, 1)
    check("MCQ 7.3-F currently fixed share of the cost base %", q((1 - CSHARE) * 100, 0), 30)

    def pv_ebitda(rev, cost):
        return sum((rev(t) - cost(t)) / (1 + DISC) ** t for t in range(1, YRS + 1))

    pv_contract = pv_ebitda(rev_part, cost_part)
    pv_fullT = pv_ebitda(rev_full, cost_part)
    pv_fullC = pv_ebitda(rev_part, cost_full)
    pv_both = pv_ebitda(rev_full, cost_full)
    check("MCQ 7.3-F PV of EBITDA at 8 % over the concession, contracted",
          q(pv_contract, 2), D("93938398.74"))
    check("MCQ 7.3-F PV of EBITDA, both sides fully indexed",
          q(pv_both, 2), D("95454401.19"))
    check("MCQ 7.3-F PV added by full tariff indexation", q(pv_fullT - pv_contract, 0), 6204143)
    check("MCQ 7.3-F PV removed by full cost indexation", q(pv_contract - pv_fullC, 0), 4688141)
    check("MCQ 7.3-F net PV gain of the swap", q(pv_both - pv_contract, 0), 1516002)
    check("MCQ 7.3-F invariant: the two printed components net to the printed gain",
          q(pv_fullT - pv_contract, 0) - q(pv_contract - pv_fullC, 0),
          q(pv_both - pv_contract, 0))
    # The item exists because the two defensible metrics disagree in SIGN.
    check("MCQ 7.3-F invariant: year-25 margin falls while discounted value rises",
          1 if m_both < m_contract and pv_both > pv_contract else 0, 1)
    check("MCQ 7.3-F invariant: the gain is conditional on the assumed cost index, so the "
          "recommendation is conditional — at 4.5 % the swap destroys value instead",
          1 if pv_ebitda(rev_full, lambda t: COST0 * D("1.045") ** (t - 1)) < pv_contract else 0, 1)

    # ==================== MCQ 7.4-E — choosing the exposure measure ====================
    check("MCQ 7.4-E 90-day receivable on the capacity charge",
          q(CAP_CHARGE * D(90) / D(365), 2), D("2958904.11"))
    check("MCQ 7.4-E receivable as the item prints it", q(CAP_CHARGE * D(90) / D(365), 0), 2958904)
    check("MCQ 7.4-E PV of contracted CFADS over the loan life at 6 %", q(CF * AF612, 0), 53522460)
    check("MCQ 7.4-E the advance being decided", COMMITTEE, 41171123)
    check("MCQ 7.4-E invariant: the loan-life exposure exceeds the advance, so it is the "
          "measure that bounds what the lenders rely on",
          1 if CF * AF612 > COMMITTEE else 0, 1)
    check("MCQ 7.4-E invariant: option C's whole-concession figure is larger again",
          1 if CF * AF825 > CF * AF612 else 0, 1)
    check("MCQ 7.4-E invariant: option A's receivable is trivial beside it — under 6 % of the "
          "loan-life exposure",
          1 if CAP_CHARGE * D(90) / D(365) < D("0.06") * CF * AF612 else 0, 1)

    # ==================== MCQ 7.4-F — measure before mitigating ====================
    # The sixteen stress cells and the counts this item prints are owned by pfl_d07_lvl.py.
    # Checked here: the stem's other number, which is a Domain 6 figure, and the item's invariant.
    check("MCQ 7.4-F year-twelve minimum on the unstressed bank case (Domain 6, Fig 6.4.1)",
          q(dscr42, 4), D("1.1851"))
    check("MCQ 7.4-F the covenant the matrix is read against", COV, D("1.20"))
    check("MCQ 7.4-F invariant: the unstressed bank case already breaches, so the matrix "
          "understates before any stress is applied",
          1 if dscr42 < COV else 0, 1)
    # Twelve months is the proposal the item puts to the candidate — an input, not a result.

    # ==================== MCQ 8.1-E — the estimate-class ladder ====================
    LADDER = {"A": D("0.50"), "B": D("0.40"), "C": D("0.30"), "D": D("0.18"), "E": D("0.08")}
    check("MCQ 8.1-E the one base estimate both provisions are quoted on", EPC, 48000000)
    check("MCQ 8.1-E Stage A (screening) provision to the +50 % bound", EPC * LADDER["A"],
          24000000)
    check("MCQ 8.1-E Stage E (control) provision to the +8 % bound", EPC * LADDER["E"], 3840000)
    check("MCQ 8.1-E the factor the sponsor is asking about",
          q(LADDER["A"] / LADDER["E"], 2), D("6.25"))
    check("MCQ 8.1-E option B screening lower bound %", q(D("0.30") * 100, 0), 30)
    check("MCQ 8.1-E option B control lower bound %", q(D("0.05") * 100, 0), 5)
    check("MCQ 8.1-E invariant: the ladder narrows monotonically from screening to control",
          1 if all(LADDER[a] > LADDER[b] for a, b in zip("ABCD", "BCDE")) else 0, 1)

    # ==================== MCQ 8.1-F — the level charge is not a deposit plan ====================
    ESC = D("0.036")
    PROGRAMME = ((7, D(3200000)), (12, D(1400000)), (14, D(3200000)),
                 (21, D(3200000)), (24, D(1400000)))
    nominal7 = D(3200000) * (1 + ESC) ** 7
    check("MCQ 8.1-F year-seven membrane cost in nominal terms", q(nominal7, 2), D("4098908.90"))
    check("MCQ 8.1-F year-seven membrane cost as the item prints it", q(nominal7, 0), 4098909)
    check("MCQ 8.1-F year-seven DSCR with no reserve", q((CF - nominal7) / INST, 4), D("0.4561"))
    pv_prog = sum(c * (1 + ESC) ** t / (1 + DISC) ** t for t, c in PROGRAMME)
    level = pv_prog / AF825
    check("MCQ 8.1-F PV of the lifecycle programme", q(pv_prog, 0), 6881021)
    check("MCQ 8.1-F level annual charge equivalent to it", q(level, 0), 644606)
    DEPOSITS = 6
    check("MCQ 8.1-F option B deposit years available before the replacement", DEPOSITS, 6)
    check("MCQ 8.1-F option B exact quotient printed in the rationale",
          q(D(4098909) / DEPOSITS, 2), D("683151.50"))
    check("MCQ 8.1-F option B deposit, taken up so six are not short",
          q(D(4098909) / DEPOSITS, 2).to_integral_value(rounding="ROUND_CEILING"), 683152)
    check("MCQ 8.1-F option A six deposits of the level charge", q(level, 0) * DEPOSITS, 3867636)
    check("MCQ 8.1-F option A shortfall in the one year it is needed",
          q(nominal7 - q(level, 0) * DEPOSITS, 0), 231273)
    check("MCQ 8.1-F option D year-eight escalated replacement cost",
          q(D(3200000) * (1 + ESC) ** 8, 0), 4246470)
    check("MCQ 8.1-F invariant: the required deposit exceeds the level charge, which is why the "
          "level charge cannot be the funding plan",
          1 if D(4098909) / DEPOSITS > level else 0, 1)
    check("MCQ 8.1-F invariant: option D moves the cliff and escalates it rather than removing it",
          1 if D(3200000) * (1 + ESC) ** 8 > nominal7 else 0, 1)
    check("MCQ 8.1-F invariant: with the reserve funded the DSCR stays at the sized level",
          q(CF / INST, 4), D("1.2743"))

    # ==================== MCQ 8.2-E — reshaping the spend profile ====================
    PROF_A = (6, 9, 13, 16, 17, 15, 13, 11)
    PROF_C = (5, 7, 9, 12, 15, 17, 18, 17)

    def area(profile):
        cum, total = D(0), D(0)
        for pct in profile:
            total += cum
            cum += D(pct)
        return total / 100

    area_a, area_c = area(PROF_A), area(PROF_C)
    check("MCQ 8.2-E S-curve area", q(area_a, 4), D("3.1900"))
    check("MCQ 8.2-E back-loaded area", q(area_c, 4), D("2.6700"))
    check("MCQ 8.2-E invariant: both profiles sum to 100 % over eight quarters",
          sum(PROF_A) - sum(PROF_C), 0)
    GEAR, RQ = D("0.70"), RATE / 4
    unit = RQ * GEAR * EPC
    check("MCQ 8.2-E area-rule unit r_q x g x S", unit, 504000)
    check("MCQ 8.2-E zero-escalation IDC, S-curve", unit * area_a, 1607760)
    check("MCQ 8.2-E zero-escalation IDC, back-loaded", unit * area_c, 1345680)
    check("MCQ 8.2-E option A the isolated interest saving", unit * (area_a - area_c), 262080)
    ESC_A, ESC_C = D(50093393), D(50324512)
    IDC_A, IDC_C = D(1658953), D(1391209)
    TOT_A, TOT_C = D(51752346), D(51715720)
    check("MCQ 8.2-E option B escalation higher on the back-loaded profile", ESC_C - ESC_A, 231119)
    check("MCQ 8.2-E option B interest lower on the back-loaded profile", IDC_A - IDC_C, 267744)
    # The published 8.2.3 table rounds each component to the dollar, so a total may sit one dollar
    # from the sum of its own printed parts. Tolerance widened to 1 for that display boundary only.
    check("MCQ 8.2-E option B invariant: the published totals tie to their own components "
          "(tolerance 1 — display rounding)", ESC_A + IDC_A, TOT_A, D(1))
    check("MCQ 8.2-E option B invariant: and so do the back-loaded ones "
          "(tolerance 1 — display rounding)", ESC_C + IDC_C, TOT_C, D(1))
    # This is why the item states 36,626 as a difference of totals and not as 267,744 - 231,119.
    check("MCQ 8.2-E option B the component difference is 36,625, one dollar from the "
          "difference of totals — which is why the item quotes the latter",
          (IDC_A - IDC_C) - (ESC_C - ESC_A), 36625)
    check("MCQ 8.2-E option B whole prize, as a difference of totals", TOT_A - TOT_C, 36626)
    check("MCQ 8.2-E option B that prize as % of the funded spend",
          q((TOT_A - TOT_C) / TOT_A * 100, 2), D("0.07"))
    # The item describes the base as "a 51.7 million spend" — the funded total truncated, not rounded.
    check("MCQ 8.2-E option B the funded spend the 0.07 % is taken against, in millions "
          "(truncated to 1 dp, as the item states it)",
          (TOT_A / 100000).to_integral_value(rounding="ROUND_FLOOR") / 10, D("51.7"))
    check("MCQ 8.2-E rationale escalation assumption %", q(ESC * 100, 1), D("3.6"))
    check("MCQ 8.2-E rationale shape-neutral rate g x r %", q(GEAR * RATE * 100, 2), D("4.20"))
    # The breakeven escalation rate is derived in checks/pfl_d08.py as 4.1659 %. What this item
    # claims is the ROUNDING of it, so that is what is checked — and unlike a constant compared
    # to itself, this fails if the manuscript ever rounds it differently.
    check("MCQ 8.2-E rationale breakeven rounds to the stated 4.17 %",
          q(D("4.1659"), 2), D("4.17"))
    check("MCQ 8.2-E option D a month of deferred commercial operations", CF / 12, 532000)
    check("MCQ 8.2-E invariant: both components dwarf the net, which is why they are reviewed "
          "separately",
          1 if (ESC_C - ESC_A) > 6 * (TOT_A - TOT_C)
          and (IDC_A - IDC_C) > 6 * (TOT_A - TOT_C) else 0, 1)
    check("MCQ 8.2-E invariant: one month of forgone CFADS exceeds twice the 262,080 saving",
          1 if 2 * (CF / 12) > 2 * (unit * (area_a - area_c)) else 0, 1)

    # ==================== MCQ 8.3-E — two defensible P80s, one wrap ====================
    REGISTER = ((D("0.40"), D(2400000)), (D("0.30"), D(1800000)), (D("0.35"), D(1400000)),
                (D("0.50"), D(900000)), (D("0.20"), D(2000000)), (D("0.25"), D(-600000)))
    reg_mean = sum(p * i for p, i in REGISTER)
    reg_var = sum(p * (1 - p) * i * i for p, i in REGISTER)
    reg_sd = reg_var.sqrt()
    reg_p80 = reg_mean + Z80 * reg_sd
    check("MCQ 8.3-E register mean exposure", reg_mean, 2690000)
    check("MCQ 8.3-E register standard deviation", q(reg_sd, 0), 1848973)
    check("MCQ 8.3-E register P80 contingency", q(reg_p80, 0), 4246095)
    check("MCQ 8.3-E register P80 as % of base", q(reg_p80 / EPC * 100, 2), D("8.85"))
    a, m, b = EPC * D("0.85"), EPC, EPC * D("1.30")
    check("MCQ 8.3-E triangular minimum from the -15 % bound", a, 40800000)
    check("MCQ 8.3-E triangular maximum from the +30 % bound", b, 62400000)
    tri_p80 = b - (D("0.20") * (b - a) * (b - m)).sqrt()
    check("MCQ 8.3-E range-based P80 total", q(tri_p80, 0), 54512795)
    check("MCQ 8.3-E range-based P80 contingency", q(tri_p80 - EPC, 0), 6512795)
    check("MCQ 8.3-E range-based P80 as % of base", q((tri_p80 - EPC) / EPC * 100, 2), D("13.57"))
    check("MCQ 8.3-E option A the gap 'take the higher' would fund",
          q(tri_p80 - EPC, 0) - q(reg_p80, 0), 2266700)
    check("MCQ 8.3-E option C the funded balancing line", KESTREL_CONT, 3645403)
    short = q(reg_p80, 0) - KESTREL_CONT
    check("MCQ 8.3-E the funded line's shortfall against the register P80", short, 600692)
    # the percentile the funded line sits at on the register, by the same normal approximation
    zf = (KESTREL_CONT - reg_mean) / reg_sd
    check("MCQ 8.3-E option C percentile of the funded line on the register (P69.7)",
          q(zf, 3), D("0.517"))
    inst_d = (DEBT + short) / AF612
    phi_f = D(str(0.5 * (1 + _math.erf(float(zf) / _math.sqrt(2))))) * 100
    check("MCQ 8.3-E option C the funded line's percentile on the register (P69.7)",
          q(phi_f, 1), D("69.7"))
    check("MCQ 8.3-E option D instalment if the 600,692 is capitalised", q(inst_d, 0), 5081284)
    check("MCQ 8.3-E option D DSCR after capitalising", q(CF / inst_d, 4), D("1.2564"))
    check("MCQ 8.3-E option D annual headroom after capitalising", q(CF - COV * inst_d, 0), 286459)
    check("MCQ 8.3-E option D headroom lost", q((CF - COV * INST) - (CF - COV * inst_d), 0), 85979)
    check("MCQ 8.3-E option D headroom lost %",
          q(((CF - COV * INST) - (CF - COV * inst_d)) / (CF - COV * INST) * 100, 1), D("23.1"))
    check("MCQ 8.3-E invariant: the register P80 is the lower of the two, so 'take the higher' "
          "would fund base-estimate uncertainty the wrap has transferred",
          1 if reg_p80 < tri_p80 - EPC else 0, 1)
    check("MCQ 8.3-E invariant: the funded line sits below both P80s",
          1 if KESTREL_CONT < reg_p80 and KESTREL_CONT < tri_p80 - EPC else 0, 1)

    # ==================== MCQ 8.3-F — where management reserve sits ====================
    # The item prints no figure; its claim is that contingency sits inside the funded base case and
    # management reserve's financing equivalent does not. Demonstrated on Kestrel's own uses: the
    # envelope closes exactly with contingency in it, and the 6,000,000 of cost-overrun support —
    # the contingent-support tier the item points to — has no room inside that envelope at all.
    USES = (EPC, D(3600000), D(1800000), D(840000), KESTREL_CONT, D(2114597))
    check("MCQ 8.3-F Kestrel's uses close on the envelope with contingency inside them",
          sum(USES), CAPEX)
    check("MCQ 8.3-F invariant: the 6,000,000 support tier cannot sit inside that envelope, "
          "which is why it is contingent support and not contingency",
          1 if sum(USES) + pool10 > CAPEX else 0, 1)
    check("MCQ 8.3-F invariant: contingency is drawn against certification within the envelope, "
          "so it is bounded by the envelope; the support tier is not",
          1 if KESTREL_CONT < CAPEX and pool10 < KESTREL_CONT * 2 else 0, 1)

    # ==================== MCQ 8.4-E — which failing test to lead with ====================
    AC, EV = D(2120000), D(1920000)
    FUNDED_CONT = D(300000)
    FUNDING = BAC + FUNDED_CONT
    check("MCQ 8.4-E Auriga total funding", FUNDING, 4300000)
    check("MCQ 8.4-E funded contingency as % of BAC", q(FUNDED_CONT / BAC * 100, 1), D("7.5"))
    available = FUNDING - AC
    check("MCQ 8.4-E funds available at the data date", available, 2180000)
    cpi = EV / AC
    check("MCQ 8.4-E CPI at the week-13 data date", q(cpi, 3), D("0.906"))
    EACS = {"budgeted rate": D(4200000), "BAC/CPI": D("4416667"), "CPI x SPI": D("4608056")}
    check("MCQ 8.4-E option A budgeted-rate EAC", EACS["budgeted rate"], 4200000)
    check("MCQ 8.4-E option A surplus on the budgeted-rate EAC",
          available - (EACS["budgeted rate"] - AC), 100000)
    check("MCQ 8.4-E option B shortfall on BAC/CPI",
          q((EACS["BAC/CPI"] - AC) - available, 0), 116667)
    check("MCQ 8.4-E shortfall on CPI x SPI", q((EACS["CPI x SPI"] - AC) - available, 0), 308056)
    check("MCQ 8.4-E invariant: BAC/CPI ties to the CPI just computed",
          q(BAC / cpi, 0), q(EACS["BAC/CPI"], 0))
    REMAINING = ((D("0.35"), D(240000)), (D("0.25"), D(320000)), (D("0.30"), D(-120000)))
    rem_mean = sum(p * i for p, i in REMAINING)
    rem_sd = (sum(p * (1 - p) * i * i for p, i in REMAINING)).sqrt()
    rem_p80 = rem_mean + Z80 * rem_sd
    CONT_LEFT = D(120000)
    check("MCQ 8.4-E remaining register mean", rem_mean, 128000)
    check("MCQ 8.4-E remaining register standard deviation", q(rem_sd, 0), 187957)
    check("MCQ 8.4-E remaining register P80", q(rem_p80, 0), 286185)
    check("MCQ 8.4-E remaining contingency", CONT_LEFT, 120000)
    check("MCQ 8.4-E option C the contingency-adequacy shortfall",
          q(rem_p80, 0) - CONT_LEFT, 166185)
    check("MCQ 8.4-E invariant: 180,000 of the funded contingency has been drawn",
          FUNDED_CONT - CONT_LEFT, 180000)
    # The key rests on this: the adequacy shortfall takes no EAC as an input, so it is the same
    # number whichever forecast is certified, while the balance test is method-dependent.
    for name, eac in EACS.items():
        check(f"MCQ 8.4-E invariant: the adequacy shortfall is unchanged under the {name} EAC",
              q(rem_p80, 0) - CONT_LEFT, 166185)
    balances = {q(available - (eac - AC), 0) for eac in EACS.values()}
    check("MCQ 8.4-E invariant: the balance test gives three different answers, "
          "so it is negotiable in a way the adequacy test is not", len(balances), 3)
    check("MCQ 8.4-E invariant: the adequacy shortfall exceeds the BAC/CPI cash call, "
          "so leading with it is leading with the larger finding",
          1 if (q(rem_p80, 0) - CONT_LEFT) > q((EACS["BAC/CPI"] - AC) - available, 0) else 0, 1)
    # The P53.5 the rationale names. The sanction register's own items live in PML-AI D8 and its
    # mean/sigma are 8.4.1's figures (owned by pfl_d08.py); what is derived here is the percentile
    # itself, plus the mutual consistency of those two parameters with 8.4.1's P80 of 490,624.
    S_MEAN, S_SD = D(278000), D(252642)
    check("MCQ 8.4-E invariant: the cited sanction mean and sigma reproduce 8.4.1's P80",
          q(S_MEAN + Z80 * S_SD, 0), 490624)
    z_sanction = (FUNDED_CONT - S_MEAN) / S_SD
    check("MCQ 8.4-E sanction z-score of the 300,000 funded", q(z_sanction, 4), D("0.0871"))
    phi = D(str(0.5 * (1 + _math.erf(float(z_sanction) / _math.sqrt(2))))) * 100
    check("MCQ 8.4-E the percentile the 300,000 actually was (P53.5)", q(phi, 1), D("53.5"))
    check("MCQ 8.4-E invariant: a P53.5 provision sits barely above the register mean, "
          "which is the defect the item says nobody named",
          1 if S_MEAN < FUNDED_CONT < S_MEAN + D("0.1") * S_SD else 0, 1)
    check("MCQ 8.4-E invariant: the BAC/CPI overrun of 416,667 is inside a P80 provision and "
          "outside a P53.5 one",
          1 if FUNDED_CONT < D(416667) < S_MEAN + Z80 * S_SD else 0, 1)
