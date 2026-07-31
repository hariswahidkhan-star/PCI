"""PFL-AI Domain 14 — Evaluation and Comprehension MCQ batch: golden checks.

Every number appearing in an option or a rationale of the items added to
`domain-14-construction-monitoring-drawdown.md` in this batch — MCQ 14.1-G, 14.2-G, 14.3-F, 14.3-G,
14.4-F and 14.4-G — is recomputed here from first principles. Master-thread values are read from
ctx and never re-typed.

Three engines are exercised.

  1. The two cost-to-completes (KA 14.1.1, 14.2.1). The lender's commitment-basis number is built
     line by line and the three lines earned value is structurally blind to — approved but
     unbaselined variations, assessed claim exposure and remaining capitalised interest — are
     isolated and expressed as a share of it; the in-balance shortfall is the difference against
     available commitment, and the sponsor's argument for excluding the claim exposure is priced
     rather than asserted (it closes 65.36 % of the gap and still leaves the project out of
     balance).
  2. The advance payment as a loan from the project to the contractor (KA 14.3.2), priced as a
     percentage of the advance and of the EPC price so that it can be negotiated against.
  3. The funded and economic costs of a month of slip (KA 14.4.1), and the three damages-recovery
     percentages that one damages rate produces on three cost bases — the arithmetic that makes the
     choice of basis a professional judgement rather than a presentation choice.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]
    DEBT = ctx["KESTREL_DEBT"]                     # 42,000,000
    CF = ctx["KESTREL_CFADS"]                      # 6,384,000
    RATE = ctx["KESTREL_RATE"]                     # 0.06


    # ---- MCQ 14.1-G and 14.2-G: the two cost-to-completes and the shortfall ----
    REM_EPC, VAR_APPR, CLAIMS = D(18720000), D(840000), D(1260000)
    OWNER_BU, REM_IDC = D(2400000), D(1436674)
    AVAIL = D(15910254) + D(6818680)                # undrawn debt + uncalled equity
    CTC_LENDER = REM_EPC + VAR_APPR + CLAIMS + OWNER_BU + REM_IDC
    CTC_BOTTOM = REM_EPC + OWNER_BU
    BLIND = VAR_APPR + CLAIMS + REM_IDC             # the lines earned value cannot see
    check("MCQ 14.2-G available commitment", AVAIL, 22728934)
    check("MCQ 14.2-G lender's cost-to-complete", CTC_LENDER, 24656674)
    check("MCQ 14.2-G bottom-up CTC(d)", CTC_BOTTOM, 21120000)
    check("MCQ 14.1-G / 14.2-G in-balance shortfall", CTC_LENDER - AVAIL, 1927740)
    check("MCQ 14.2-G lines invisible to earned value", BLIND, 3536674)
    check("MCQ 14.2-G invisible lines as % of the lender's number",
          (BLIND / CTC_LENDER * 100).quantize(D("0.01")), D("14.34"))
    # The sponsor's argument, priced: excluding the claim exposure alone would close 65.36 % of the
    # gap and still leave the project out of balance — the point the rationale of 14.2-G makes.
    check("MCQ 14.2-G excluding claim exposure still leaves a shortfall",
          CTC_LENDER - CLAIMS - AVAIL, 667740)
    check("MCQ 14.2-G claim exposure as % of the shortfall",
          (CLAIMS / (CTC_LENDER - AVAIL) * 100).quantize(D("0.01")), D("65.36"))
    check("MCQ 14.1-G shortfall as months of funded delay cost at 415,000",
          (D(1927740) / D(415000)).quantize(D("0.0001")), D("4.6452"))

    # ---- MCQ 14.3-F: the advance payment, priced ----
    EPC, ADV_PCT = D(48000000), D("0.10")
    ADV = EPC * ADV_PCT
    ADV_IDC = D(254597)                             # from WE 14.3.2, the re-run funding model
    check("MCQ 14.3-F advance payment", ADV, 4800000)
    check("MCQ 14.3-F advance carry as % of the advance",
          (ADV_IDC / ADV * 100).quantize(D("0.01")), D("5.30"))
    check("MCQ 14.3-F advance carry as % of the EPC price",
          (ADV_IDC / EPC * 100).quantize(D("0.01")), D("0.53"))
    check("MCQ 14.3-F retention saving is a quarter of what the advance costs",
          (ADV_IDC / D(62589)).quantize(D("0.001")), D("4.068"))
    check("MCQ 14.3-F both terms together", ADV_IDC - D(62589), 192008)

    # ---- MCQ 14.4-F: one damages rate, three recovery percentages ----
    INT_M = DEBT * RATE / 12
    PROL = D(138000) + D(46000) + D(21000)
    FUNDED = INT_M + PROL
    FORGONE = CF / 12
    ECONOMIC = FUNDED + FORGONE
    D5_BASIS = INT_M + FORGONE                      # Domain 5's narrower calibration basis
    LD_MONTH = D(20000) * 30
    check("MCQ 14.4-F interest on drawn debt per month", INT_M, 210000)
    check("MCQ 14.4-F prolongation the SPV bears directly", PROL, 205000)
    check("MCQ 14.4-F funded cost of a month of slip", FUNDED, 415000)
    check("MCQ 14.4-F forgone CFADS per month", FORGONE, 532000)
    check("MCQ 14.4-F full economic cost of a month of slip", ECONOMIC, 947000)
    check("MCQ 14.4-F Domain 5's narrower basis", D5_BASIS, 742000)
    check("MCQ 14.4-F damages per month at 20,000 a day", LD_MONTH, 600000)
    check("MCQ 14.4-F recovery of the funded cost %",
          (LD_MONTH / FUNDED * 100).quantize(D("0.01")), D("144.58"))
    check("MCQ 14.4-F recovery of the economic cost %",
          (LD_MONTH / ECONOMIC * 100).quantize(D("0.01")), D("63.36"))
    check("MCQ 14.4-F recovery on Domain 5's basis %",
          (LD_MONTH / D5_BASIS * 100).quantize(D("0.01")), D("80.86"))
    check("MCQ 14.4-F full economic cost per day (30/360)",
          (ECONOMIC / 30).quantize(D("0.01")), D("31566.67"))
    check("MCQ 14.4-F prolongation as % of the funded cost",
          (PROL / FUNDED * 100).quantize(D("0.1")), D("49.4"))
    check("MCQ 14.4-F understatement per month on Domain 5's basis", ECONOMIC - D5_BASIS, 205000)
