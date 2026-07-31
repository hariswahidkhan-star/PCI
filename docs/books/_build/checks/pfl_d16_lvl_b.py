"""PFL-AI Domain 16 — Evaluation and Comprehension MCQ batch (D13-16 author): golden checks.

Every number appearing in an option or a rationale of the items added to
`domain-16-data-automation-responsible-ai.md` by this author — MCQ 16.1-G, 16.1-H, 16.2-F, 16.2-G,
16.3-G and 16.4-F — is recomputed here from first principles. Kestrel's covenant headroom, the
yardstick the domain measures every data-quality figure against, is derived from the master thread
rather than re-typed.

Four engines are exercised.

  1. Cost per reviewed item and the automation breakeven (KA 16.1.2), including the result the
     Evaluation item turns on: because the automated process carries the higher residual error rate,
     repricing an undetected error from 320 to 1,200 moves the breakeven from 14,800 to 50,000
     records a year. The error rate the estate case tolerates, and the breakeven computed with the
     error-cost term omitted, are both recomputed so the distractors are exact.
  2. The review that moves from build to review (KA 16.2.3): the real saving of a third against the
     apparent saving of two thirds, and the expected cost of the defect the omitted hours would
     have caught.
  3. The attribution residual (KA 16.3.1), expressed as a percentage of annual covenant headroom
     rather than of the forecast — the translation that makes an unreconciled explanation a
     disqualifying finding.
  4. Governance latency (KA 16.4.3) on the registered `E[wait] = M/2 + L` form, giving the price of
     an approval gate per change.

Domain 16 carries items from two authors in this batch; this module covers only the six items listed
above, which is why it is suffixed rather than named `pfl_d16_lvl.py`.
"""


def run(ctx):
    check, D, ew = ctx["check"], ctx["D"], ctx["ew"]
    DS = ctx["KESTREL_INSTALMENT"]                 # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                      # 6,384,000
    # Kestrel's annual covenant headroom (Domain 10): the yardstick this domain measures against.
    HEAD_COV = CF - DS * D("1.20")
    check("D16 covenant headroom read from the master thread",
          HEAD_COV.quantize(D("0.01")), D("372437.72"))


    # ---- MCQ 16.1-G: the breakeven that rises with consequence ----
    # The two processing costs are built from the setup's minutes and rates, not re-typed: manual is
    # 8.5 minutes at 1.60 a minute; the pipeline is 0.50 a record of platform and inference plus a
    # 5 % referral rate at 6.75 adjudication minutes.
    MAN_PROC = D("8.5") * D("1.60")
    AUT_PROC = D("0.50") + D("0.05") * (D("6.75") * D("1.60"))
    MAN_ERR, AUT_ERR = D("0.0180"), D("0.0260")
    check("MCQ 16.1-G manual processing cost a record", MAN_PROC, D("13.60"))
    check("MCQ 16.1-G automated processing cost a record", AUT_PROC, D("1.04"))
    check("MCQ 16.1-G the automated residual error rate is the higher of the two",
          1 if AUT_ERR > MAN_ERR else 0, 1)
    FIXED, KESTREL_RECS, ESTATE_RECS = D(148000), D(9400), D(56400)

    def per_item(cons):
        return MAN_PROC + MAN_ERR * cons, AUT_PROC + AUT_ERR * cons

    for cons, man, aut, adv, bev in (
            (D(320), "19.36", "9.36", "10.00", 14800),
            (D(1200), "35.20", "32.24", "2.96", 50000)):
        m, a = per_item(cons)
        check(f"MCQ 16.1-G manual all-in per record at {cons}", m, D(man))
        check(f"MCQ 16.1-G automated all-in per record at {cons}", a, D(aut))
        check(f"MCQ 16.1-G per-record advantage at {cons}", m - a, D(adv))
        check(f"MCQ 16.1-G breakeven volume at {cons}", (FIXED / (m - a)).quantize(D("1")), bev)
    m320, a320 = per_item(D(320))
    check("MCQ 16.1-G automation at Kestrel's own volume",
          (KESTREL_RECS * (m320 - a320) - FIXED).quantize(D("1")), -54000)
    check("MCQ 16.1-G automation across the six-asset estate",
          (ESTATE_RECS * (m320 - a320) - FIXED).quantize(D("1")), 416000)
    check("MCQ 16.1-G estate manual total", ESTATE_RECS * m320, 1091904)
    check("MCQ 16.1-G estate automated total all-in", FIXED + ESTATE_RECS * a320, 675904)
    check("MCQ 16.1-G automated error rate the estate case tolerates %",
          (((ESTATE_RECS * m320 - FIXED) / ESTATE_RECS - AUT_PROC) / D(320) * 100)
          .quantize(D("0.0001")), D("4.9050"))
    check("MCQ 16.1-G the tolerated rate is nearly double the modelled one",
          (D("4.9050") / (AUT_ERR * 100)).quantize(D("0.001")), D("1.887"))
    bev_no_err = FIXED / (MAN_PROC - AUT_PROC)
    check("MCQ 16.1-G breakeven with the error-cost term omitted",
          bev_no_err.quantize(D("1")), 11783)
    # Computed on the unrounded breakeven, as the manuscript does (11,783.4395, not 11,783).
    check("MCQ 16.1-G omission flatters automation by %",
          ((D(14800) - bev_no_err) / D(14800) * 100).quantize(D("0.0001")), D("20.3822"))
    check("MCQ 16.1-G estate case at 1,200 an error", (ESTATE_RECS * D("2.96") - FIXED), 18944)

    # ---- MCQ 16.2-F: the review that moves from build to review ----
    RATE_MOD = D("150.00")
    HAND_B, HAND_R = D(40), D(8)
    MACH_B, MACH_R = D(6), D(26)
    hand = (HAND_B + HAND_R) * RATE_MOD
    mach = (MACH_B + MACH_R) * RATE_MOD
    mach_habit = (MACH_B + HAND_R) * RATE_MOD
    check("MCQ 16.2-F hand-built cost", hand, 7200)
    check("MCQ 16.2-F machine-assisted cost with proper review", mach, 4800)
    check("MCQ 16.2-F real saving %", ((hand - mach) / hand * 100).quantize(D("0.0001")),
          D("33.3333"))
    check("MCQ 16.2-F cost at the habitual review", mach_habit, 2100)
    check("MCQ 16.2-F apparent saving %",
          ((hand - mach_habit) / hand * 100).quantize(D("0.0001")), D("70.8333"))
    check("MCQ 16.2-F omitted review hours", MACH_R - HAND_R, 18)
    check("MCQ 16.2-F value of the omitted review", (MACH_R - HAND_R) * RATE_MOD, 2700)
    EMV_DEF = D("0.35") * D(3250352)
    check("MCQ 16.2-F expected cost of the defect the review would catch",
          EMV_DEF.quantize(D("0.01")), D("1137623.20"))
    check("MCQ 16.2-F expected return on the omitted review",
          (EMV_DEF / ((MACH_R - HAND_R) * RATE_MOD)).quantize(D("0.0001")), D("421.3419"))
    check("MCQ 16.2-F review hours per assisted build hour",
          (MACH_R / MACH_B).quantize(D("0.0001")), D("4.3333"))

    # ---- MCQ 16.2-G: the load-bearing subset, and the failure no accuracy statistic sees ----
    # The item states no computed figure beyond the two counts and the verification price, so those
    # are the checks: 26 of 340 terms verified at half an hour of a 240.00 specialist hour.
    TERMS, LOAD = D(340), D(26)
    VERIFY = LOAD * D("0.5") * D("240.00")
    check("MCQ 16.2-G load-bearing terms as % of the agreement's defined terms",
          (LOAD / TERMS * 100).quantize(D("0.0001")), D("7.6471"))
    check("MCQ 16.2-G cost of verifying the load-bearing subset", VERIFY, 3120)
    check("MCQ 16.2-G verification against one definitional error",
          (D(600000) / VERIFY).quantize(D("0.0001")), D("192.3077"))

    # ---- MCQ 16.3-G: the attribution residual, in covenant headroom ----
    ATTRIB = D(6190000)
    RESID = CF - ATTRIB
    check("MCQ 16.3-G attribution residual", RESID, 194000)
    check("MCQ 16.3-G residual as % of the forecast",
          (RESID / CF * 100).quantize(D("0.0001")), D("3.0388"))
    check("MCQ 16.3-G residual as % of annual covenant headroom",
          (RESID / HEAD_COV * 100).quantize(D("0.0001")), D("52.0892"))
    check("MCQ 16.3-G the residual exceeds half the headroom",
          1 if RESID > HEAD_COV / 2 else 0, 1)

    # ---- MCQ 16.4-F: the price of an approval gate ----
    BENEFIT = D(416000)
    per_day = BENEFIT / 365
    wait_comm = ew(60, 10)
    wait_panel = ew(7, 2)
    check("MCQ 16.4-F daily value of the change", per_day.quantize(D("0.0001")), D("1139.7260"))
    check("MCQ 16.4-F committee expected wait, days", wait_comm, 40)
    check("MCQ 16.4-F delegated panel expected wait, days", wait_panel, D("5.5"))
    check("MCQ 16.4-F value forgone, committee",
          (per_day * wait_comm).quantize(D("0.01")), D("45589.04"))
    check("MCQ 16.4-F value forgone, delegated panel",
          (per_day * wait_panel).quantize(D("0.01")), D("6268.49"))
    check("MCQ 16.4-F price of the governance design per change",
          (per_day * (wait_comm - wait_panel)).quantize(D("0.01")), D("39320.55"))
    check("MCQ 16.4-F the gate's latency against the headroom it protects",
          1 if per_day * wait_comm < HEAD_COV else 0, 1)
    check("MCQ 16.4-F latency as % of the covenant headroom it protects",
          (per_day * wait_comm / HEAD_COV * 100).quantize(D("0.01")), D("12.24"))
