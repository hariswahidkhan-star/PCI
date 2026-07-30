"""PFL-AI Domain 12 — golden checks for the Evaluation and Comprehension MCQs added to KA 12.1–12.4.

Scope: every number printed in MCQ 12.1-E, 12.1-F, 12.2-E, 12.3-D, 12.4-E and 12.4-F — options and
rationales. `pfl_d12.py` owns the domain's worked examples; this module owns only the added items.
Master-thread values come from ctx, never re-typed.

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
