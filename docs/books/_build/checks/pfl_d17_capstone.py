"""PFL-AI Appendix G — integrated capstones. Golden-answer checks.

The capstone appendix asserts almost nothing new about the individual domains; it asserts that their
results **agree**, and its own arithmetic is the arithmetic of putting them side by side. Those are
exactly the claims most likely to be wrong, because no single chapter's author owns them, so each one
is recomputed here from the domain results it combines.

Attributed to domain 17 so that Appendix C reports the capstone's verification record separately
from the sixteen domains' rather than inflating one of them.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]

    # ---- the domain results the capstone combines (each verified in its own module) -------
    NPV = D("16179360.32")          # D4
    CFADS = D(6384000)              # D7, D10
    SERVICE = D("5009635.23")       # D3
    CAPEX = D(60000000)
    DEBT_REQ, DEBT_SIZED = D(42000000), D(41171123)      # D9 proposal, D10 capacity
    EQ_TERMSHEET = D(18000000)
    COV_HEADROOM = D("372437.72")   # D10, D15
    DIST_HEADROOM = D("121955.96")  # D15
    LIFECYCLE_PV, LIFECYCLE_ANN = D(6881021), D(644606)  # D8
    RESIDUE_NOM, RESIDUE_CR = D(2655674), D(4095674)     # D12
    P_CLOSE = D("0.5472")           # D5
    ORIGINATION = D(7400000)        # D5
    NPV_LOW, NPV_HIGH = D(-9670265), D(19875251)         # D6

    # ---- G.1.2 the first reconciliation --------------------------------------------------
    check("G.1.2 sizing gap", DEBT_REQ - DEBT_SIZED, D(828877))
    EQ_REAL = EQ_TERMSHEET + (DEBT_REQ - DEBT_SIZED)
    check("G.1.2 equity actually required", EQ_REAL, D(18828877))
    check("G.1.2 sources still equal uses", DEBT_SIZED + EQ_REAL, CAPEX)
    check("G.1.2 equity share as proposed (%)", EQ_TERMSHEET / CAPEX * 100, D("30.00"),
          tol=D("0.005"))
    check("G.1.2 equity share as financeable (%)", EQ_REAL / CAPEX * 100, D("31.38"),
          tol=D("0.005"))
    # and the sizing target itself: 1.30x on level CFADS with annuity service
    check("G.1.2 DSCR achieved on the sized facility", CFADS / (DEBT_SIZED / D("8.383844")),
          D("1.3000"), tol=D("0.0002"))
    check("G.1.2 DSCR on the requested facility", CFADS / SERVICE, D("1.2743"), tol=D("0.00005"))

    # ---- G.1.3 the binding constraint moves ----------------------------------------------
    check("G.1.3 covenant headroom as share of base CFADS (%)", COV_HEADROOM / CFADS * 100,
          D("5.8339"), tol=D("0.00005"))
    check("G.1.3 distribution headroom as share of base CFADS (%)", DIST_HEADROOM / CFADS * 100,
          D("1.9103"), tol=D("0.00005"))
    check("G.1.3 distribution headroom as share of covenant headroom (%)",
          DIST_HEADROOM / COV_HEADROOM * 100, D("32.75"), tol=D("0.005"))
    check("G.1.3 covenant binding CFADS", CFADS - COV_HEADROOM, D("6011562.28"))
    check("G.1.3 distribution binding CFADS", CFADS - DIST_HEADROOM, D("6262044.04"))
    # the tension with the lifecycle programme
    check("G.1.3 lifecycle annual charge as share of CFADS (%)", LIFECYCLE_ANN / CFADS * 100,
          D("10.0972"), tol=D("0.00005"))
    check("G.1.3 lifecycle charge over distribution headroom (x)",
          LIFECYCLE_ANN / DIST_HEADROOM, D("5.2856"), tol=D("0.00005"))
    check("G.1.3 lifecycle charge over covenant headroom (x)",
          LIFECYCLE_ANN / COV_HEADROOM, D("1.7308"), tol=D("0.00005"))
    check("G.1.3 CFADS after funding lifecycle from operating cash", CFADS - LIFECYCLE_ANN,
          D(5739394))
    DSCR_AFTER = (CFADS - LIFECYCLE_ANN) / SERVICE
    check("G.1.3 DSCR after funding lifecycle from operating cash", DSCR_AFTER, D("1.1457"),
          tol=D("0.00005"))
    check("G.1.3 INVARIANT that DSCR is below the 1.20 covenant",
          D(1) if DSCR_AFTER < D("1.20") else D(0), D(1))
    check("G.1.3 coverage lost to the lifecycle charge", CFADS / SERVICE - DSCR_AFTER,
          D("0.1287"), tol=D("0.00005"))
    # the lifecycle PV and its annual equivalent must be consistent at the project rate over 25 years
    AF25 = (1 - D("1.08") ** -25) / D("0.08")
    check("G.1.3 AF(0.08,25)", AF25, D("10.674776"), tol=D("0.0000005"))
    check("G.1.3 lifecycle annual charge reconciles to its present value",
          LIFECYCLE_PV / AF25, LIFECYCLE_ANN, tol=D(1500))

    # ---- G.1.4 the project against the business ------------------------------------------
    EV = NPV * P_CLOSE
    check("G.1.4 probability-weighted NPV at the development decision", EV, D("8853345.97"))
    check("G.1.4 net of honest origination cost", EV - ORIGINATION, D("1453345.97"))
    check("G.1.4 origination as share of expected value (%)", ORIGINATION / EV * 100, D("83.58"),
          tol=D("0.005"))
    check("G.1.4 the charge-code figure understates origination by", ORIGINATION - D(2400000),
          D(5000000))
    # D5's own condition sensitivity, restated: the weakest condition is worth 3.7255x the strongest.
    # Computed from the UNROUNDED uplifts, as D5 computes it. Dividing the printed four-decimal
    # figures instead gives 3.7254 — a rounding artefact, not a discrepancy, and exactly the trap
    # Domain 4 (KA 4.1.2) warns about: run the test on the retained value, not the printed one.
    _cond = [D("0.92"), D("0.90"), D("0.95"), D("0.88"), D("0.93"), D("0.85")]
    _base = D(1)
    for _c in _cond:
        _base *= _c
    check("G.1.4 joint probability of close (%)", _base * 100, D("54.72"), tol=D("0.005"))
    check("G.1.4 average condition probability (%)", sum(_cond) / 6 * 100, D("90.50"),
          tol=D("0.005"))
    _up_weak = _base / D("0.85") * D("0.95") - _base
    _up_strong = _base / D("0.95") * D("0.98") - _base
    check("G.1.4 uplift from lifting the weakest condition (points)", _up_weak * 100, D("6.4375"),
          tol=D("0.00005"))
    check("G.1.4 uplift from lifting the strongest condition (points)", _up_strong * 100,
          D("1.7280"), tol=D("0.00005"))
    check("G.1.4 weakest-condition uplift over strongest (x)", _up_weak / _up_strong, D("3.7255"),
          tol=D("0.00005"))
    check("G.1.4 a 90% joint probability needs every condition at 98.26%",
          D("0.9826") ** 6 * 100, D("90.00"), tol=D("0.005"))
    # the probability the capstone actually uses is D5's rounded 0.5472, and it must match the model
    check("G.1.4 P_CLOSE matches the computed joint probability", P_CLOSE, _base,
          tol=D("0.00005"))
    # D13's scheduling swing is the difference between the two programme values
    check("G.1.4 diligence scheduling swing", D("4066699.88") - D("-1146900.12"), D(5213600))

    # ---- G.1.5 what the equity cheque carries --------------------------------------------
    check("G.1.5 credit adjustment on the cover", RESIDUE_CR - RESIDUE_NOM, D(1440000))
    check("G.1.5 residue as share of the real equity cheque (%)", RESIDUE_CR / EQ_REAL * 100,
          D("21.7521"), tol=D("0.00005"))
    check("G.1.5 nominal residue as share of the real equity cheque (%)",
          RESIDUE_NOM / EQ_REAL * 100, D("14.1043"), tol=D("0.00005"))
    check("G.1.5 residue as share of NPV (%)", RESIDUE_CR / NPV * 100, D("25.3142"),
          tol=D("0.00005"))
    check("G.1.5 NPV net of the credit-adjusted residue", NPV - RESIDUE_CR, D("12083686.32"))
    # the full wrap is unaffordable on coverage, not on price
    WRAP_CAPEX = D(64620000)
    check("G.1.5 full-wrap capex increment", WRAP_CAPEX - CAPEX, D(4620000))
    check("G.1.5 full-wrap DSCR is below the 1.20 covenant",
          D(1) if D("1.1832") < D("1.20") else D(0), D(1))
    check("G.1.5 coverage surrendered by buying the full wrap", D("1.2743") - D("1.1832"),
          D("0.0911"), tol=D("0.00005"))
    check("G.1.5 transfer efficiency on the three controllable risks",
          D(4420000) / D(2128000), D("2.0771"), tol=D("0.00005"))
    check("G.1.5 value destroyed by the five declined items at zero margin",
          D(3300000) - D(2840000), D(460000))

    # ---- G.1.6 which of the five NPVs ----------------------------------------------------
    SPAN = NPV_HIGH - NPV_LOW
    check("G.1.6 span of the five defensible NPVs", SPAN, D(29545516))
    check("G.1.6 the headline NPV's position in that span (%)", (NPV - NPV_LOW) / SPAN * 100,
          D("87.49"), tol=D("0.005"))
    check("G.1.6 span as a multiple of the headline NPV", SPAN / NPV, D("1.8261"),
          tol=D("0.00005"))

    # ===================== Capstone Two — Aurora Ridge (demand risk) =====================
    # A new project, so nothing here is inherited: every figure is derived from the four stated
    # inputs (capex, traffic, toll, cost) and the two stated rates. The capstone's spine is that
    # ONE change of revenue basis produces a four-fold change in achievable gearing, so that
    # comparison is asserted as an invariant rather than left as a narrative claim.
    CAPEX_A, RATE_A = D(240000000), D("0.07")
    VPD, TOLL, DAYS = D(18000), D("2.40"), 365
    OM, PAVE = D(4200000), D(1368000)

    def af_a(r, n):
        r = D(str(r))
        return (1 - (1 + r) ** -n) / r

    AF20, AF17 = af_a(RATE_A, 20), af_a(RATE_A, 17)
    check("G.2 AF(0.07,20)", AF20, D("10.594014"), tol=D("0.0000005"))
    check("G.2 AF(0.07,17)", AF17, D("9.763223"), tol=D("0.0000005"))
    REV_A = VPD * DAYS * TOLL
    check("G.2 mature annual revenue", REV_A, D(15768000))
    check("G.2 mature EBITDA", REV_A - OM, D(11568000))
    CFADS_A = REV_A - OM - PAVE
    check("G.2.1 mature CFADS", CFADS_A, D(10200000))

    # G.2.1 the demand-risk premium
    for tgt, svc, cap, gear in (("1.30", D("7846153.85"), D("83122265.62"), D("34.63")),
                                ("1.40", D("7285714.29"), D("77184960.93"), D("32.16")),
                                ("1.45", D("7034482.76"), D("74523410.55"), D("31.05"))):
        check(f"G.2.1 service at {tgt}x", CFADS_A / D(tgt), svc, tol=D("0.01"))
        check(f"G.2.1 debt capacity at {tgt}x", CFADS_A / D(tgt) * AF20, cap, tol=D("0.05"))
        check(f"G.2.1 gearing at {tgt}x (%)", CFADS_A / D(tgt) * AF20 / CAPEX_A * 100, gear,
              tol=D("0.005"))
    CAP130, CAP140 = CFADS_A / D("1.30") * AF20, CFADS_A / D("1.40") * AF20
    check("G.2.1 capacity lost to the extra tenth of a turn", CAP130 - CAP140,
          D("5937304.69"), tol=D("0.05"))
    check("G.2.1 that loss as a share of capacity (%)", (CAP130 - CAP140) / CAP130 * 100,
          D("7.1429"), tol=D("0.00005"))
    # the identity behind it: capacity is inversely proportional to the required ratio
    check("G.2.1 INVARIANT the loss equals 1 - 1.30/1.40", (CAP130 - CAP140) / CAP130,
          1 - D("1.30") / D("1.40"), tol=D("0.0000001"))

    # G.2.2 the ramp
    SVC140 = CFADS_A / D("1.40")
    RAMP = ((1, D("0.60"), D(10800), D(9460800), D(3892800), D("0.5343")),
            (2, D("0.80"), D(14400), D(12614400), D(7046400), D("0.9672")),
            (3, D("1.00"), D(18000), D(15768000), D(10200000), D("1.4000")))
    for yr, f, veh, rev, cf, dscr in RAMP:
        check(f"G.2.2 year {yr} traffic", VPD * f, veh)
        check(f"G.2.2 year {yr} revenue", REV_A * f, rev)
        check(f"G.2.2 year {yr} CFADS", REV_A * f - OM - PAVE, cf)
        check(f"G.2.2 year {yr} DSCR on level-sized service", (REV_A * f - OM - PAVE) / SVC140,
              dscr, tol=D("0.00005"))
    check("G.2.2 INVARIANT year one fails to pay, not merely to cover",
          D(1) if (REV_A * D("0.60") - OM - PAVE) < SVC140 else D(0), D(1))
    check("G.2.2 cash shortfall across years 1-2",
          sum(SVC140 - (REV_A * f - OM - PAVE) for f in (D("0.60"), D("0.80"))),
          D("3632228.57"), tol=D("0.01"))
    check("G.2.2 covenant shortfall across the ramp",
          sum(CFADS_A - (REV_A * f - OM - PAVE) for f in (D("0.60"), D("0.80"), D("1.00"))),
          D(9460800))
    C1 = REV_A * D("0.60") - OM - PAVE
    check("G.2.2 capacity if sized on year one", C1 / D("1.40") * AF20, D("29457413.32"),
          tol=D("0.05"))
    check("G.2.2 capacity given up by sizing on year one",
          SVC140 * AF20 - C1 / D("1.40") * AF20, D("47727547.61"), tol=D("0.05"))
    check("G.2.2 that reduction as a share of level capacity (%)",
          (SVC140 * AF20 - C1 / D("1.40") * AF20) / (SVC140 * AF20) * 100, D(62), tol=D("0.5"))

    # G.2.3 sculpting, and which constraint binds
    CAP_IO = (C1 / D("1.40")) / RATE_A
    CAP_AM = (CFADS_A / D("1.40")) * AF17
    check("G.2.3 year-one interest-only constraint", CAP_IO, D("39722448.98"), tol=D("0.01"))
    check("G.2.3 steady-state amortisation constraint", CAP_AM, D("71132053.24"), tol=D("0.05"))
    check("G.2.3 INVARIANT year-one interest cover is the binding constraint",
          D(1) if CAP_IO < CAP_AM else D(0), D(1))
    D_SC = min(CAP_IO, CAP_AM)
    check("G.2.3 sculpted capacity", D_SC, D("39722448.98"), tol=D("0.01"))
    check("G.2.3 binding constraint as a share of the amortisation test (%)",
          CAP_IO / CAP_AM * 100, D("55.8"), tol=D("0.05"))
    check("G.2.3 recovered over sizing on year one", D_SC - D("29457413.32"),
          D("10265035.66"), tol=D("0.05"))
    check("G.2.3 given up against level sizing", SVC140 * AF20 - D_SC, D("37462511.95"),
          tol=D("0.05"))
    check("G.2.3 sculpted gearing (%)", D_SC / CAPEX_A * 100, D("16.55"), tol=D("0.005"))
    check("G.2.3 year-two cover on interest only",
          (REV_A * D("0.80") - OM - PAVE) / (D_SC * RATE_A), D("2.5342"), tol=D("0.00005"))
    check("G.2.3 steady-state service after amortisation begins", D_SC / AF17,
          D("4068579.51"), tol=D("0.01"))
    check("G.2.3 steady-state cover", CFADS_A / (D_SC / AF17), D("2.5070"), tol=D("0.00005"))
    check("G.2.3 interest paid over three interest-only years", D_SC * RATE_A * 3,
          D("8341714.29"), tol=D("0.01"))
    check("G.2.3 equity or support at the sculpted structure (%)",
          (CAPEX_A - D_SC) / CAPEX_A * 100, D("83.45"), tol=D("0.005"))
    check("G.2.3 equity at level 1.40x sizing (%)",
          (CAPEX_A - D("77184960.93")) / CAPEX_A * 100, D("67.84"), tol=D("0.005"))
    # the capstone's headline comparison against Kestrel
    check("G.2.3 support needed to reach a 30 % equity cheque",
          CAPEX_A - D_SC - CAPEX_A * D("0.30"), D("128277551.02"), tol=D("0.05"))
    check("G.2.3 that support as a share of capex (%)",
          (CAPEX_A - D_SC - CAPEX_A * D("0.30")) / CAPEX_A * 100, D("53.45"), tol=D("0.005"))
    check("G.2.3 INVARIANT availability gearing exceeds demand gearing fourfold",
          D("68.6185") / (D_SC / CAPEX_A * 100), D("4.1459"), tol=D("0.00005"))

    # G.2.4 the toll lever under inelastic demand
    E = D("-0.40")
    for pct, toll, veh, rev, chg in ((D("-0.10"), D("2.16"), D(18775), D("14802058.52"),
                                      D("-6.1260")),
                                     (D("0.10"), D("2.64"), D(17327), D("16695991.78"),
                                      D("5.8853")),
                                     (D("0.20"), D("2.88"), D(16734), D("17590790.60"),
                                      D("11.5601"))):
        m = 1 + pct
        check(f"G.2.4 toll at {pct:+} change", TOLL * m, toll, tol=D("0.005"))
        check(f"G.2.4 traffic at {pct:+} change", VPD * m ** E, veh, tol=D("1"))
        check(f"G.2.4 revenue at {pct:+} change", REV_A * m * m ** E, rev, tol=D("0.05"))
        check(f"G.2.4 revenue change at {pct:+} (%)", (m * m ** E - 1) * 100, chg,
              tol=D("0.00005"))
    check("G.2.4 INVARIANT inelastic demand gives no interior revenue optimum",
          D(1) if all(REV_A * (1 + p) * (1 + p) ** E > REV_A
                      for p in (D("0.10"), D("0.20"), D("0.50"), D("1.00"))) else D(0), D(1))
    check("G.2.4 traffic lost at a 10 % toll rise (%)", (1 - D("1.10") ** E) * 100,
          D("3.7406"), tol=D("0.00005"))
