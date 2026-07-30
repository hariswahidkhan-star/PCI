"""PFL-AI Domain 15 — Evaluation and Comprehension MCQ batch: golden checks.

Every number appearing in an option or a rationale of the items added to
`domain-15-operations-performance-restructuring.md` in this batch — MCQ 15.1-G, 15.2-G, 15.3-F,
15.3-G, 15.4-F and 15.4-G — is recomputed here from first principles. Master-thread values are
read from ctx and never re-typed.

Four engines are exercised.

  1. The double translation (KA 15.1.3): ratio to cash trigger, cash to pre-tax revenue through the
     0.80 cash-to-revenue gearing, and revenue to each driver's own units. The two characteristic
     errors — omitting the gearing and applying it twice — are computed so that the distractors are
     independently correct answers to the wrong question.
  2. The waterfall residual and the block account (KA 15.2.2-15.2.3): the distributable amount, the
     year-three reserve shortfall it cannot fund, and the proof that the trapped year-one dividend
     covers that shortfall — which is what makes a weaker distribution test a call for new equity
     rather than a win.
  3. Cure, waiver and amendment (KA 15.3.3), with the demonstration that a covenant reset sized on
     the current ratio is breached again at the next test date.
  4. The exit decomposition (KA 15.4.4): a sale at the seller's own discount rate creates exactly
     nil, and the only real gain is yield compression, which is a market view rather than
     performance.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]
    EQUITY = ctx["KESTREL_EQUITY"]                 # 18,000,000
    DS = ctx["KESTREL_INSTALMENT"]                 # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                      # 6,384,000
    COV, LOCK, DIST = D("1.20"), D("1.15"), D("1.25")


    # ---- MCQ 15.1-G: ratio -> cash -> driver units, twice ----
    GEAR = D("0.80")                                # cash-to-revenue gearing after 20 % cash tax
    CAP_PAY, GUAR = D(7300000), D(95)
    PER_POINT = CAP_PAY / GUAR
    VOL, VOL_MARGIN = D(10000000), D("0.25")
    check("MCQ 15.1-G capacity payment per availability point",
          PER_POINT.quantize(D("0.01")), D("76842.11"))
    TRIG = {}
    for lab, ratio, cash, head, pct in (
            ("distribution", DIST, "6262044.04", "121955.96", "1.9103"),
            ("covenant", COV, "6011562.28", "372437.72", "5.8339"),
            ("lock-up", LOCK, "5761080.51", "622919.49", "9.7575")):
        trig = DS * ratio
        h = CF - trig
        TRIG[lab] = h
        check(f"MCQ 15.1-G {lab} cash trigger", trig.quantize(D("0.01")), D(cash))
        check(f"MCQ 15.1-G {lab} headroom", h.quantize(D("0.01")), D(head))
        check(f"MCQ 15.1-G {lab} headroom as % of CFADS",
              (h / CF * 100).quantize(D("0.0001")), D(pct))
    check("MCQ 15.1-G the distribution condition binds first",
          1 if TRIG["distribution"] < TRIG["covenant"] < TRIG["lock-up"] else 0, 1)
    check("MCQ 15.1-G covenant headroom in pre-tax revenue",
          (TRIG["covenant"] / GEAR).quantize(D("0.01")), D("465547.16"))
    check("MCQ 15.1-G availability floor for the covenant %",
          (GUAR - TRIG["covenant"] / GEAR / PER_POINT).quantize(D("0.0001")), D("88.9415"))
    check("MCQ 15.1-G availability floor for the distribution condition %",
          (GUAR - TRIG["distribution"] / GEAR / PER_POINT).quantize(D("0.0001")), D("93.0161"))
    check("MCQ 15.1-G volume tolerance to the covenant m3",
          (TRIG["covenant"] / GEAR / VOL_MARGIN).quantize(D("0.01")), D("1862188.62"))
    check("MCQ 15.1-G volume tolerance as % of output",
          (TRIG["covenant"] / GEAR / VOL_MARGIN / VOL * 100).quantize(D("0.0001")), D("18.6219"))
    # The distractor that omits the 0.80 gearing, and the one that applies it twice.
    check("MCQ 15.1-G distractor: points omitting the cash-to-revenue gearing",
          (TRIG["covenant"] / PER_POINT).quantize(D("0.0001")), D("4.8468"))
    check("MCQ 15.1-G distractor: points applying the gearing twice",
          (TRIG["covenant"] / GEAR / GEAR / PER_POINT).quantize(D("0.0001")), D("7.5731"))

    # ---- MCQ 15.2-G: the block account as pre-funding ----
    MRA = D(600000)                                 # levelised maintenance-reserve charge
    DISTRIBUTABLE = CF - DS - MRA
    CF_Y3 = D("5202936.48")
    RESIDUAL_Y3 = CF_Y3 - DS
    SHORT_Y3 = MRA - RESIDUAL_Y3
    check("MCQ 15.2-G base-case distributable amount",
          DISTRIBUTABLE.quantize(D("0.01")), D("774364.77"))
    check("MCQ 15.2-G base-case cash yield on subscribed equity %",
          (DISTRIBUTABLE / EQUITY * 100).quantize(D("0.0001")), D("4.3020"))
    check("MCQ 15.2-G year-three residual after debt service",
          RESIDUAL_Y3.quantize(D("0.01")), D("193301.25"))
    check("MCQ 15.2-G year-three reserve shortfall drawn from the block account",
          SHORT_Y3.quantize(D("0.01")), D("406698.75"))
    check("MCQ 15.2-G the trapped year-one dividend covers the shortfall",
          1 if DISTRIBUTABLE > SHORT_Y3 else 0, 1)
    check("MCQ 15.2-G shortfall as % of the trapped year-one dividend",
          (SHORT_Y3 / DISTRIBUTABLE * 100).quantize(D("0.01")), D("52.52"))
    check("MCQ 15.2-G reserve charge as % of the base-case dividend",
          (MRA / DISTRIBUTABLE * 100).quantize(D("0.1")), D("77.5"))
    check("MCQ 15.2-G debt service as % of CFADS", (DS / CF * 100).quantize(D("0.01")),
          D("78.47"))

    # ---- MCQ 15.3-F: sizing an amendment against the stress, not the current ratio ----
    CF_Y2 = D("5963894.11")
    OUT_Y2 = D("36871351.43")
    COV_CASH = DS * COV
    CURE_Y2, CURE_Y3 = COV_CASH - CF_Y2, COV_CASH - CF_Y3
    WAIVER = OUT_Y2 * D("0.0015")
    AMEND_FEE = OUT_Y2 * D("0.0025")
    UPLIFT_PV = D("651258.24")
    check("MCQ 15.3-F year-two backward DSCR", (CF_Y2 / DS).quantize(D("0.0001")), D("1.1905"))
    check("MCQ 15.3-F year-three backward DSCR", (CF_Y3 / DS).quantize(D("0.0001")), D("1.0386"))
    check("MCQ 15.3-F covenant cash requirement", COV_CASH.quantize(D("0.01")), D("6011562.28"))
    # Display rounding: the cure is 47,668.166, printed truncated in the manuscript table.
    check("MCQ 15.3-F year-two equity cure (display rounding)",
          CURE_Y2.quantize(D("0.01")), D("47668.16"), D("0.011"))
    check("MCQ 15.3-F year-three equity cure", CURE_Y3.quantize(D("0.01")), D("808625.80"))
    check("MCQ 15.3-F waiver fee at 15 bp", WAIVER.quantize(D("0.01")), D("55307.03"))
    check("MCQ 15.3-F amendment fee at 25 bp", AMEND_FEE.quantize(D("0.01")), D("92178.38"))
    check("MCQ 15.3-F total amendment cost",
          (AMEND_FEE + UPLIFT_PV).quantize(D("0.01")), D("743436.62"))
    check("MCQ 15.3-F the year-three breach is 17.0 times the year-two breach",
          (CURE_Y3 / CURE_Y2).quantize(D("0.1")), D("17.0"))
    # A reset to 1.10x sized on the year-two ratio is breached again at year three.
    RESET = D("1.10")
    check("MCQ 15.3-F a 1.10x reset clears the year-two ratio",
          1 if CF_Y2 / DS > RESET else 0, 1)
    check("MCQ 15.3-F the same reset is breached at year three",
          1 if CF_Y3 / DS < RESET else 0, 1)
    check("MCQ 15.3-F cash shortfall against a 1.10x reset at year three",
          (DS * RESET - CF_Y3).quantize(D("0.01")), D("307662.27"))
    check("MCQ 15.3-F a 1.00x reset survives year three",
          1 if CF_Y3 / DS > D(1) else 0, 1)

    # ---- MCQ 15.4-F: the exit, decomposed ----
    HOLD_NPV = D("5027733.03")
    NPV_AT = {"6.50": D("7661177.40"), "7.00": D("6732654.36"),
              "7.50": D("5855965.90"), "8.00": D("5027733.03")}
    check("MCQ 15.4-F selling at the sponsors' own rate creates nothing",
          NPV_AT["8.00"] - HOLD_NPV, 0)
    check("MCQ 15.4-F yield compression from 8.00 % to 7.50 %",
          NPV_AT["7.50"] - HOLD_NPV, D("828232.87"))
    check("MCQ 15.4-F IRR uplift at a sale on the sponsors' own rate",
          D("11.7666") - D("9.8591"), D("1.9075"))
    check("MCQ 15.4-F yield compression at end of year eight",
          (D("828232.87") * (1 + D("0.08")) ** 8).quantize(D("1")), D("1533001"))
    check("MCQ 15.4-F price difference at 7.50 % against 8.00 %",
          D("35919098.27") - D("34386097.04"), D("1533001.23"))
