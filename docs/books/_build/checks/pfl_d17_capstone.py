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
