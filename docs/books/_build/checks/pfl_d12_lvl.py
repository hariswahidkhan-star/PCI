"""PFL-AI Domain 12 — golden checks for the Evaluation and Comprehension MCQs added to KA 12.1–12.4.

Scope: every number printed in the twelve items added to KA 12.1–12.4 — 12.1-E/F/G, 12.2-E/F/G,
12.3-D/E/F and 12.4-E/F/G — options and rationales. `pfl_d12.py` owns the domain's worked examples;
this module owns only the added items. Master-thread values come from ctx, never re-typed.

The first pass covered six of the twelve. 12.1-G, 12.2-F, 12.2-G, 12.3-E and 12.4-G printed a further
twenty-two figures that no module recomputed — the three make-good bases and the prepayment residue,
the deduction ceiling, the amortisation schedule behind the termination gap, the parent guarantee at
its assessed credit, and the prolongation asymmetry. Section 6 closes that.

Engines used:

  1. The cap stack (12.1.2) — cap-binding day = delay cap / daily rate; exposure = daily economic cost
     x days; recovery = min(rate x days, cap), then limited by the aggregate cap; residue is the
     difference, expressed against the EPC price and the equity contribution.
  2. The O&M asymmetry (12.1.4) — outage cost on the same 30/360 daily basis as a construction delay,
     against a cap expressed as a share of the annual fee.
  3. The volume floor (12.2.2) — `CFADS(x) = 9,060,000x - 2,676,000`, inverted for a target ratio as
     `x = (k x DS + 2,676,000) / 9,060,000`; the slope of coverage per point of volume; and the debt
     that a 90 % floor would support at the covenant, giving the additional equity required.
  4. Risk-adjusted cover (12.3.2) — sum(face x probability of payment), applied in order of certainty,
     with the equivalent face amount of a conditional instrument as certain cover / probability.
  5. The claim (12.4.3) — expected award discounted at 26 months, own costs at the 13-month midpoint,
     the settlement ceiling as the present value of fighting less the cost of settling, and the
     breakeven disputed sum solving `k D + PV(costs) = D/2 + negotiation cost`.
  6. The five items the first pass left unchecked. The three make-good bases built from the same
     output line — bare covenant by resizing the debt, sized coverage first-order on the shortfall
     ratio, value as 25 years of lost `CFADS` — and the prepayment residue that shows a 12-year relief
     cannot pay a 25-year loss (12.1.3). The deduction ceiling against covenant headroom (12.2.1).
     The amortisation schedule rolled to year five, and five years of distributions against the equity
     contributed, so the unreturned figure is a difference of computed values (12.2.3). The parent
     guarantee at its assessed credit against the bond it would replace, with the bond's own fee
     (12.3.2). And the two daily rates whose difference is the quantification argument (12.4.2).
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))

    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    DS = ctx["KESTREL_INSTALMENT"]             # 5,009,635.23
    EQUITY = ctx["KESTREL_EQUITY"]             # 18,000,000
    BOARD = ctx["KESTREL_DISCOUNT"]            # 0.08
    AF6_12 = D("8.383844")
    COV, LOCK = D("1.20"), D("1.15")
    EPC = D(48000000)
    DAILY = D(7000) + CF / 360                 # interest on drawn debt + forgone CFADS (30/360)
    HEAD = CF - DS * COV

    check("D12-lvl daily economic cost of delay", q(DAILY, 2), D("24733.33"))
    check("D12-lvl annual covenant headroom", q(HEAD, 0), 372438)

    # ================= MCQ 12.1-E — rate, cap and the cap-binding day ==============
    LDR, DELAY_CAP, PERF_CAP, AGG = D(20000), EPC * D("0.10"), EPC * D("0.10"), EPC * D("0.20")
    check("MCQ 12.1-E delay damages cap", DELAY_CAP, 4800000)
    check("MCQ 12.1-E aggregate liability cap", AGG, 9600000)
    check("MCQ 12.1-E the two sub-caps exhaust the aggregate exactly", DELAY_CAP + PERF_CAP, AGG)
    check("MCQ 12.1-E cap-binding day", DELAY_CAP / LDR, 240)
    for days, exposure, residue in ((240, 5936000, 1136000), (300, 7420000, 2620000),
                                    (360, 8904000, 4104000)):
        exp = DAILY * days
        rec = min(LDR * days, DELAY_CAP)
        check(f"MCQ 12.1-E {days}-day delay exposure", q(exp, 0), exposure)
        check(f"MCQ 12.1-E {days}-day uncovered residue", q(exp - rec, 0), residue)
    check("MCQ 12.1-E recovery as a share of daily cost to the cap-binding day %",
          q(LDR / DAILY * 100, 2), D("80.86"))
    check("MCQ 12.1-E beyond the cap the marginal recovery per further day is nil",
          min(LDR * 301, DELAY_CAP) - min(LDR * 300, DELAY_CAP), 0)

    # ================= MCQ 12.1-F — the O&M liability asymmetry ====================
    FEE = D(1200000)
    check("MCQ 12.1-F half-fee cap", FEE / 2, 600000)
    for days, cost in ((30, 742000), (60, 1484000)):
        check(f"MCQ 12.1-F {days}-day outage cost", q(DAILY * days, 0), cost)
    check("MCQ 12.1-F 30-day outage already exceeds a half-fee cap",
          q(DAILY * 30 - FEE / 2, 0), 142000)
    check("MCQ 12.1-F 60-day outage exceeds even a full-year-fee cap",
          q(DAILY * 60 - FEE, 0), 284000)

    # ================= MCQ 12.2-E — the contracted volume floor ====================
    SLOPE, INTERCEPT = D(9060000), D(2676000)  # CFADS(x) = 9,060,000x - 2,676,000
    check("MCQ 12.2-E the cash-flow line reproduces the master thread",
          SLOPE * 1 - INTERCEPT, CF)
    check("MCQ 12.2-E value of one percentage point of output", SLOPE / 100, 90600)
    check("MCQ 12.2-E coverage per percentage point of volume", q(SLOPE / 100 / DS, 4), D("0.0181"))
    check("MCQ 12.2-E points of volume the headroom buys", q(HEAD / (SLOPE / 100), 2), D("4.11"))

    def floor_for(k):
        return (k * DS + INTERCEPT) / SLOPE

    check("MCQ 12.2-E floor the 1.20 covenant requires %", q(floor_for(COV) * 100, 4), D("95.8892"))
    check("MCQ 12.2-E floor the 1.15 lock-up requires %", q(floor_for(LOCK) * 100, 4), D("93.1245"))
    check("MCQ 12.2-E floor at which cash equals debt service %",
          q(floor_for(D(1)) * 100, 4), D("84.8304"))
    cf90 = SLOPE * D("0.90") - INTERCEPT
    check("MCQ 12.2-E CFADS at the negotiated 90 % floor", cf90, 5478000)
    check("MCQ 12.2-E DSCR at the negotiated 90 % floor", q(cf90 / DS, 4), D("1.0935"))
    check("MCQ 12.2-E 90 % fails the covenant", 1 if cf90 / DS < COV else 0, 1)
    check("MCQ 12.2-E 90 % fails the lock-up as well", 1 if cf90 / DS < LOCK else 0, 1)
    debt90 = cf90 / COV * AF6_12
    check("MCQ 12.2-E debt a 90 % floor supports at the covenant", q(debt90, 0), 38272248)
    check("MCQ 12.2-E additional equity a 90 % floor requires",
          q(ctx["KESTREL_DEBT"] - debt90, 0), 3727752)

    # ================= MCQ 12.3-D — risk-adjusted cover ============================
    EXPOSURE = D("12255673.53")                 # the combined 300-day, 5 %-shortfall stress (12.1.2)
    BOND, PQ = D(4800000), D("0.70")
    cover = BOND + (AGG - BOND) * PQ
    check("MCQ 12.3-D nominal cover", AGG, 9600000)
    check("MCQ 12.3-D risk-adjusted cover", cover, 8160000)
    check("MCQ 12.3-D credit haircut", AGG - cover, 1440000)
    check("MCQ 12.3-D credit haircut as % of the nominal cap",
          q((AGG - cover) / AGG * 100, 2), D("15.00"))
    check("MCQ 12.3-D nominal residue", q(EXPOSURE - AGG, 0), 2655674)
    check("MCQ 12.3-D risk-adjusted residue", q(EXPOSURE - cover, 0), 4095674)
    check("MCQ 12.3-D nominal residue as % of equity",
          q((EXPOSURE - AGG) / EQUITY * 100, 2), D("14.75"))
    check("MCQ 12.3-D risk-adjusted residue as % of equity",
          q((EXPOSURE - cover) / EQUITY * 100, 2), D("22.75"))
    check("MCQ 12.3-D equivalent guarantee face amount", q(BOND / PQ, 2), D("6857142.86"))
    check("MCQ 12.3-D equivalence multiple", q(1 / PQ, 4), D("1.4286"))

    # ================= MCQ 12.4-E — the claims policy, priced =====================
    QUANTUM, OWN_ASSESS = D(1870000), D(1050000)
    DAYS_CLAIMED, DAYS_ALLOWED = 90, 55
    DSP = (QUANTUM - OWN_ASSESS) + LDR * (DAYS_CLAIMED - DAYS_ALLOWED)
    check("MCQ 12.4-E quantum gap", QUANTUM - OWN_ASSESS, 820000)
    check("MCQ 12.4-E time-impact gap", LDR * (DAYS_CLAIMED - DAYS_ALLOWED), 700000)
    check("MCQ 12.4-E disputed sum", DSP, 1520000)
    PF, PS = D("0.35"), D("0.25")
    emv = PF * DSP + PS * DSP / 2
    check("MCQ 12.4-E expected award", emv, 722000)
    df_award = (1 + BOARD) ** (D(26) / 12)
    df_costs = (1 + BOARD) ** (D(13) / 12)
    check("MCQ 12.4-E discount factor at 26 months", q(df_award, 6), D("1.181458"))
    check("MCQ 12.4-E discount factor at 13 months", q(df_costs, 6), D("1.086949"))
    COSTS = D(620000) + D(180000)
    check("MCQ 12.4-E own costs of the process", COSTS, 800000)
    pv_award, pv_costs = emv / df_award, COSTS / df_costs
    fight = pv_award + pv_costs
    check("MCQ 12.4-E PV of the expected award", q(pv_award, 2), D("611109.54"))
    check("MCQ 12.4-E PV of own costs", q(pv_costs, 2), D("736005.26"))
    check("MCQ 12.4-E PV of fighting", q(fight, 0), 1347115)
    NEG = D(60000)
    check("MCQ 12.4-E settlement ceiling", q(fight - NEG, 0), 1287115)
    check("MCQ 12.4-E ceiling as % of the disputed sum",
          q((fight - NEG) / DSP * 100, 2), D("84.68"))
    check("MCQ 12.4-E saving from settling at the midpoint",
          q(fight - (DSP / 2 + NEG), 0), 527115)
    check("MCQ 12.4-E cost of holding to the own assessment equals that saving",
          q(fight - (DSP / 2 + NEG), 0), 527115)
    k = (PF + PS / 2) / df_award
    d_star = (pv_costs - NEG) / (D("0.5") - k)
    check("MCQ 12.4-E breakeven disputed sum", q(d_star, 0), 6901234)
    check("MCQ 12.4-E own costs against annual covenant headroom",
          q(COSTS / HEAD, 3), D("2.148"))

    # ============ MCQ 12.1-G — three bases, and the application clause ============
    SHORT_PCT, LIFE = D("0.05"), ctx["KESTREL_LIFE"]
    SLOPE = D(9060000)
    annual_short = SLOPE * SHORT_PCT           # 453,000 of CFADS a year at a 5 % shortfall
    check("MCQ 12.1-G annual CFADS shortfall at 5 %", annual_short, 453000)
    AF8_25 = D("10.674776")                    # the registry literal 12.1.3 substitutes
    check("MCQ 12.1-G AF(0.08,25) literal is the correct rounding",
          q(af(BOARD, LIFE), 6), AF8_25)
    value_basis = annual_short * AF8_25
    check("MCQ 12.1-G value basis", q(value_basis, 2), D("4835673.53"))
    DEBT = ctx["KESTREL_DEBT"]
    sized_basis = DEBT * annual_short / CF     # first-order, per Domain 5 KA 5.4.3
    check("MCQ 12.1-G sized-coverage basis", q(sized_basis, 2), D("2980263.16"))
    cf95 = CF - annual_short
    bare_basis = DEBT - (cf95 / COV) * AF6_12
    check("MCQ 12.1-G stressed CFADS at 95 % output", cf95, 5931000)
    check("MCQ 12.1-G bare covenant basis", q(bare_basis, 2), D("562851.03"))
    check("MCQ 12.1-G spread between the extremes", q(value_basis / bare_basis, 3), D("8.591"))
    check("MCQ 12.1-G value the sized basis concedes to equity",
          q(value_basis - sized_basis, 0), 1855410)
    check("MCQ 12.1-G the value basis exceeds the sub-cap",
          1 if value_basis > PERF_CAP else 0, 1)
    new_debt = DEBT - PERF_CAP                 # the full cap applied wholly to prepayment
    relief = DS - new_debt / AF6_12
    check("MCQ 12.1-G debt after prepayment", new_debt, 37200000)
    check("MCQ 12.1-G instalment after prepayment", q(new_debt / AF6_12, 2), D("4437105.46"))
    check("MCQ 12.1-G annual relief", q(relief, 2), D("572529.77"))
    check("MCQ 12.1-G DSCR after prepayment", q(cf95 / (new_debt / AF6_12), 4), D("1.3367"))
    check("MCQ 12.1-G AF(0.08,12) literal is the correct rounding",
          q(af(BOARD, 12), 6), D("7.536078"))
    AF8_12 = D("7.536078")                     # 12.1.3 substitutes the six-decimal factor
    check("MCQ 12.1-G PV of twelve years of relief", q(relief * AF8_12, 2), D("4314628.99"))
    check("MCQ 12.1-G residual gap a 12-year relief leaves on a 25-year loss",
          q(value_basis - relief * AF8_12, 2), D("521044.53"))

    # ============ MCQ 12.2-F — the deduction regime as an uncapped cap ============
    check("MCQ 12.2-F annual deduction that breaches the covenant", q(HEAD, 0), 372438)
    check("MCQ 12.2-F that ceiling against the tariff, %",
          q(HEAD / D(12000000) * 100, 3), D("3.104"))
    check("MCQ 12.2-F the deduction bites before any of the caps, which are per-event not annual",
          1 if HEAD < DELAY_CAP else 0, 1)

    # ============ MCQ 12.2-G — a debt-outstanding formula pays equity nothing ============
    R = ctx["KESTREL_RATE"]
    bal, sched = DEBT, {}
    for t in range(1, ctx["KESTREL_TENOR"] + 1):
        interest = bal * R
        bal = bal - (DS - interest)
        sched[t] = bal
    check("MCQ 12.2-G debt outstanding at the end of year 1", q(sched[1], 2), D("39510364.77"))
    check("MCQ 12.2-G debt outstanding at the end of year 5", q(sched[5], 0), 27965695)
    # the cent-rounded instalment leaves 7 cents at maturity, which the schedule truncates
    check("MCQ 12.2-G the schedule retires the debt", q(sched[12], 0), 0)
    dist = CF - DS
    check("MCQ 12.2-G annual distribution", q(dist, 0), 1374365)
    check("MCQ 12.2-G five years of nominal distributions", q(dist * 5, 0), 6871824)
    check("MCQ 12.2-G unreturned equity on a year-5 force-majeure termination",
          q(EQUITY - dist * 5, 0), 11128176)
    check("MCQ 12.2-G an 85 % formula's gap at the end of year 5",
          q(sched[5] * D("0.15"), 0), 4194854)

    # ============ MCQ 12.3-E — a larger face amount at a worse credit ============
    PCG = D(5500000)
    check("MCQ 12.3-E face amount offered above the bond", PCG - BOND, 700000)
    check("MCQ 12.3-E the guarantee's worth at the assessed credit", PCG * PQ, 3850000)
    check("MCQ 12.3-E cover given up against the bond", BOND - PCG * PQ, 950000)
    check("MCQ 12.3-E face amount that would be equivalent", q(BOND / PQ, 0), 6857143)
    check("MCQ 12.3-E the offered face is short of that by", q(BOND / PQ - PCG, 0), 1357143)
    BOND_FEE_RATE, CONSTRUCTION_YEARS = D("0.012"), 3
    fee = BOND * BOND_FEE_RATE * CONSTRUCTION_YEARS
    check("MCQ 12.3-E bond fee over the construction period", q(fee, 0), 172800)
    check("MCQ 12.3-E that fee as a share of what is given up %",
          q(fee / (BOND - PCG * PQ) * 100, 2), D("18.19"))
    check("MCQ 12.3-E the fee saving is less than a fifth of the cover surrendered",
          1 if fee < (BOND - PCG * PQ) / 5 else 0, 1)
    check("MCQ 12.3-E demand form does not change the obligor's credit", PCG * PQ, 3850000)

    # ============ MCQ 12.4-G — the prolongation asymmetry ============
    PROLONG = D(12500)
    check("MCQ 12.4-G the contractor's rate", LDR, 20000)
    check("MCQ 12.4-G the project company's asserted rate", PROLONG, 12500)
    check("MCQ 12.4-G the asymmetry, per day", LDR - PROLONG, 7500)
    check("MCQ 12.4-G the contractor's rate as a share of the daily economic cost %",
          q(LDR / DAILY * 100, 2), D("80.86"))
    check("MCQ 12.4-G symmetry at 12,500 would recover only, %",
          q(PROLONG / DAILY * 100, 2), D("50.54"))
