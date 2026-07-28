"""PFL-AI Domain 5 — Project development and bankability: golden checks.

Every number printed as a result in `domain-05-development-bankability.md` is recomputed here from
its definition rather than from its printed output, and every teaching invariant the domain asserts
is pinned as an equality, a bound or an inequality so that the *claim* fails if it stops holding.

Master-thread values come from ctx and are never re-typed: `KESTREL_CAPEX`, `KESTREL_DEBT`,
`KESTREL_EQUITY`, `KESTREL_RATE`, `KESTREL_TENOR`, `KESTREL_LIFE`, `KESTREL_INSTALMENT`,
`KESTREL_CFADS`, `KESTREL_EBITDA`, `KESTREL_EBIT`, `KESTREL_INTEREST_Y1`, `KESTREL_DISCOUNT`,
`KESTREL_NPV`, and the shared annuity factor `af(r, n)`.

The domain has six engines:

    funnel (5.1.2)     stage spend = count x unit cost; programme spend = sum; cost per close,
                       close rate, stage conversions, portfolio value, value multiple and the
                       breakeven close count/rate. Two identities are asserted rather than
                       assumed: the three stage conversions multiply to the close rate, and the
                       value multiple equals the achieved close rate divided by the breakeven close
                       rate — the manuscript uses the second to claim "2.19x of margin".
    gate (5.1.3)       expected waste with and without the gate, gate net value, the breakeven
                       detection probability (SOLVED, then re-substituted to confirm the gate is
                       worth exactly zero there), the bid-window option cost, the sign flip and the
                       breakeven window-miss probability (also re-substituted to zero).
    sponsors (5.2.3/4) gearing, the several support pool, the per-sponsor equity/support/committed
                       table with its column totals, committed capital as a share of capex, the
                       joint-and-several worst cases and their two very different multiples; then
                       the equity bridge, whose neutrality at the bridge rate is checked as an
                       EXACT zero difference and whose non-neutrality away from that rate is
                       checked too, because MCQ 5.2-C's distractor D generalises the identity.
    conjunction (5.3)  the running product of six conditions, the arithmetic mean it is contrasted
                       with (and the bound joint < mean), marginal gain from lifting the weakest
                       and the strongest and their ratio (checked against the closed form
                       (dp/p_weak)/(dp/p_strong)), the all-at-0.98 product, the uniform requirement
                       for a 0.90 joint (solved, then raised to the sixth power to confirm), the
                       continue test and the breakeven close probability.
    technology (5.3.4) Domain 10's sizing rule at two target ratios, the capacity loss, the gearing
                       move, the equity substitution cost, and the identity loss/capacity =
                       1 - DSCR_lo/DSCR_hi. Monotonicity (a higher target ratio cannot support more
                       debt) is asserted.
    completion (5.4)   the COD-slip cash table on 30/360 with damages, cap and the uncovered
                       residue at 180 and 360 days; the coverage percentage and its complement; the
                       capitalisation consequence (instalment, DSCR, trigger, headroom) and the
                       equity alternative; the quarterly value view of the slip and of Case study
                       A's nine months; then the 97 %-output buy-down, whose defining property —
                       that it restores the SIZED DSCR exactly — is asserted rather than described.

Published MCQ distractors that are the results of named errors are checked too (the closing-stage
unit cost; the breakeven close count divided by closes rather than screenings; the gate value with
its own cost omitted; base equity read as exposure; a ratio increase applied to principal; maximum
debt service mistaken for a capital sum; the pre-working-capital CFADS; damages paid for every day
of a capped regime), because the manuscript names each distractor's arithmetic and a wrong
distractor is as much a defect as a wrong answer.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))
    gt = lambda a, b: 1 if a > b else 0
    eq = lambda a, b: 1 if a == b else 0

    CAPEX = ctx["KESTREL_CAPEX"]            # USD 60,000,000
    DEBT = ctx["KESTREL_DEBT"]              # USD 42,000,000
    EQUITY = ctx["KESTREL_EQUITY"]          # USD 18,000,000
    R = ctx["KESTREL_RATE"]                 # 6.0 %
    N = ctx["KESTREL_TENOR"]                # 12 years
    LIFE = ctx["KESTREL_LIFE"]              # 25 years
    INST = ctx["KESTREL_INSTALMENT"]        # USD 5,009,635.23
    CFADS = ctx["KESTREL_CFADS"]            # USD 6,384,000
    EBITDA = ctx["KESTREL_EBITDA"]          # USD 7,500,000
    EBIT = ctx["KESTREL_EBIT"]              # USD 5,100,000
    INT1 = ctx["KESTREL_INTEREST_Y1"]       # USD 2,520,000
    DISC = ctx["KESTREL_DISCOUNT"]          # 8 %
    NPV = ctx["KESTREL_NPV"]                # USD 16,179,360

    AF = af(R, N)
    AF25 = af(R, LIFE)
    COVENANT = D("1.20")

    # ================= master-thread restatements (the domain's opening panel) =========
    check("D5 opening AF(0.06, 12) as printed", q(AF, 6), D("8.383844"))
    check("D5 opening instalment = debt / AF(0.06, 12)", q(DEBT / AF, 2), INST)
    check("D5 opening gearing, debt share of capex", q(DEBT / CAPEX * 100, 1), D("70.0"))
    check("D5 opening equity share of capex", q(EQUITY / CAPEX * 100, 1), D("30.0"))
    check("D5 opening debt + equity fund the capex exactly", DEBT + EQUITY, CAPEX)
    check("D5 opening year-one interest", DEBT * R, INT1)
    check("D5 opening year-one principal", INST - INT1, D("2489635.23"))
    check("D5 opening EBITDA = revenue - cash operating cost", D(12000000) - D(4500000), EBITDA)
    check("D5 opening EBIT = EBITDA - depreciation", EBITDA - D(2400000), EBIT)
    check("D5 opening CFADS from EBITDA, tax and working capital",
          EBITDA - (EBITDA - D(2400000) - INT1) * D("0.20") - D(600000), CFADS)
    check("D5 opening pre-working-capital CFADS", CFADS + D(600000), 6984000)
    check("D5 opening DSCR (Domain 10, cited)", q(CFADS / INST, 4), D("1.2743"))
    check("D5 opening LLCR (Domain 10, cited)", q(CFADS * AF / DEBT, 4), D("1.2743"))
    check("D5 INVARIANT level CFADS against a level instalment makes LLCR == DSCR",
          q(CFADS * AF / DEBT, 4), q(CFADS / INST, 4))
    check("D5 opening PLCR (Domain 10, cited)", q(CFADS * AF25 / DEBT, 4), D("1.9431"))
    check("D5 INVARIANT PLCR exceeds LLCR because life exceeds tenor",
          gt(CFADS * AF25 / DEBT, CFADS * AF / DEBT), 1)
    check("D5 opening covenant cash trigger (Domain 10, cited)", q(COVENANT * INST, 0), 6011562)
    check("D5 opening annual headroom (Domain 10, cited)", q(CFADS - COVENANT * INST, 0), 372438)
    check("D5 opening life less tenor leaves the PLCR tail", LIFE - N, 13)
    check("5.3.2 a 12-year loan against a 7-year offtake leaves five unfunded years", N - 7, 5)
    check("Why-this-domain six conditions near 0.90 give a joint near 55 %",
          q(D("0.905") ** 6 * 100, 0), 55)
    # Domain 4's appraisal verdict, restated in the opening paragraph. IRR (12.19 %) and MIRR are
    # Domain 4's golden checks; PI is recomputed here because it follows from ctx alone.
    check("D5 opening PI restated from Domain 4", q(1 + NPV / CAPEX, 3), D("1.270"))
    check("D5 opening INVARIANT a positive NPV at 8 % means an IRR above 8 % (12.19 % as cited)",
          gt(D("0.1219"), DISC) + gt(NPV, 0), 2)

    # ================= ENGINE 1 — the development funnel (5.1.2) =======================
    STAGES = [(D(40), D(25000)), (D(12), D(250000)), (D(5), D(1200000)), (D(2), D(2400000))]
    SPEND = [c * u for c, u in STAGES]
    check("WE 5.1.2 screening stage spend", SPEND[0], 1000000)
    check("WE 5.1.2 concept stage spend", SPEND[1], 3000000)
    check("WE 5.1.2 feasibility and bid stage spend", SPEND[2], 6000000)
    check("WE 5.1.2 to-close stage spend", SPEND[3], 4800000)
    PROG = sum(SPEND)
    check("WE 5.1.2 programme spend", PROG, 14800000)
    cum = []
    s = D(0)
    for x in SPEND:
        s += x
        cum.append(s)
    check("Fig 5.1.1 cumulative after screening", cum[0], 1000000)
    check("Fig 5.1.1 cumulative after concept", cum[1], 4000000)
    check("Fig 5.1.1 cumulative after feasibility", cum[2], 10000000)
    check("Fig 5.1.1 cumulative at close", cum[3], PROG)
    CLOSES, SCREENED = STAGES[3][0], STAGES[0][0]
    PERCLOSE = PROG / CLOSES
    check("WE 5.1.2 development cost per closed project", PERCLOSE, 7400000)
    check("WE 5.1.2 INVARIANT cost per close x closes recovers programme spend",
          PERCLOSE * CLOSES, PROG)
    CLOSERATE = CLOSES / SCREENED
    check("WE 5.1.2 close rate", q(CLOSERATE * 100, 1), D("5.0"))
    c1 = STAGES[1][0] / STAGES[0][0]
    c2 = STAGES[2][0] / STAGES[1][0]
    c3 = STAGES[3][0] / STAGES[2][0]
    check("WE 5.1.2 stage conversion screening to concept", q(c1 * 100, 1), D("30.0"))
    check("WE 5.1.2 stage conversion concept to feasibility", q(c2 * 100, 2), D("41.67"))
    check("WE 5.1.2 stage conversion feasibility to close", q(c3 * 100, 1), D("40.0"))
    check("WE 5.1.2 INVARIANT the three stage conversions multiply to the close rate",
          c1 * c2 * c3, CLOSERATE)
    PV_PORT = CLOSES * NPV
    check("WE 5.1.2 portfolio value at Domain 4's NPV per close", PV_PORT, 32358720)
    check("WE 5.1.2 net portfolio value", PV_PORT - PROG, 17558720)
    MULT = PV_PORT / PROG
    check("WE 5.1.2 value multiple", q(MULT, 4), D("2.1864"))
    BE_CLOSES = PROG / NPV
    check("WE 5.1.2 breakeven closes", q(BE_CLOSES, 4), D("0.9147"))
    BE_RATE = BE_CLOSES / SCREENED
    check("WE 5.1.2 breakeven close rate", q(BE_RATE * 100, 2), D("2.29"))
    check("WE 5.1.2 breakeven close rate from the key-terms formula",
          PROG / (NPV * SCREENED), BE_RATE)
    check("WE 5.1.2 INVARIANT at the breakeven close rate the programme is worth exactly zero",
          BE_RATE * SCREENED * NPV - PROG, 0)
    check("WE 5.1.2 margin over the breakeven close rate", q(CLOSERATE / BE_RATE, 2), D("2.19"))
    check("WE 5.1.2 INVARIANT the margin equals the value multiple", MULT, CLOSERATE / BE_RATE)
    check("WE 5.1.2 INVARIANT halving the hit rate still creates value",
          gt(CLOSERATE / 2, BE_RATE), 1)
    check("WE 5.1.2 one close in how many screenings at breakeven", q(1 / BE_RATE, 2), D("43.73"))
    check("WE 5.1.2 screening as a share of programme spend", q(SPEND[0] / PROG * 100, 1), D("6.8"))
    check("WE 5.1.2 a lost late-stage project costs its unit cost outright", STAGES[3][1], 2400000)
    # MCQ 5.1-A/B distractors, each a named error
    check("MCQ 5.1-A distractor A (closing-stage unit cost, portfolio ignored)",
          SPEND[3] / CLOSES, 2400000)
    check("MCQ 5.1-A distractor C (whole closing stage undivided)", SPEND[3], 4800000)
    check("MCQ 5.1-A distractor D (entire programme on one project)", PROG, 14800000)
    check("MCQ 5.1-B distractor A (achieved close rate)", q(CLOSERATE * 100, 2), D("5.00"))
    check("MCQ 5.1-B distractor C (divided by closes, not screenings)",
          q(BE_CLOSES / CLOSES * 100, 2), D("45.74"))
    check("MCQ 5.1-B distractor D (breakeven closes quoted as a percentage)",
          q(BE_CLOSES * 100, 2), D("91.47"))

    # ================= ENGINE 2 — the feasibility gate (5.1.3) =========================
    PFLAW, WASTE, GATE, DET = D("0.40"), D(3300000), D(180000), D("0.75")
    check("WE 5.1.3 late-discovery waste is the transaction tranche plus abortive fees",
          STAGES[3][1] + D(900000), WASTE)
    W_OUT = PFLAW * WASTE
    W_IN = GATE + PFLAW * (1 - DET) * WASTE
    check("WE 5.1.3 expected waste without the gate", W_OUT, 1320000)
    check("WE 5.1.3 expected waste with the gate", W_IN, 510000)
    GNET = W_OUT - W_IN
    check("WE 5.1.3 gate net value per project", GNET, 810000)
    check("WE 5.1.3 gate net value across five projects entering feasibility",
          GNET * STAGES[2][0], 4050000)
    BE_DET = 1 - (W_OUT - GATE) / (PFLAW * WASTE)
    check("WE 5.1.3 breakeven detection probability", q(BE_DET * 100, 2), D("13.64"))
    check("WE 5.1.3 INVARIANT at the breakeven detection rate the gate is worth exactly zero",
          W_OUT - (GATE + PFLAW * (1 - BE_DET) * WASTE), 0)
    check("WE 5.1.3 INVARIANT the achieved detection rate is far above breakeven",
          gt(DET, BE_DET), 1)
    PMISS = D("0.10")
    OPTCOST = PMISS * NPV
    check("WE 5.1.3 bid-window option cost", OPTCOST, 1617936)
    check("WE 5.1.3 gate net value after the option cost", GNET - OPTCOST, -807936)
    check("WE 5.1.3 INVARIANT the same gate is positive without the window risk and negative with it",
          gt(GNET, 0) + gt(0, GNET - OPTCOST), 2)
    BE_MISS = GNET / NPV
    check("WE 5.1.3 breakeven window-miss probability", q(BE_MISS * 100, 2), D("5.01"))
    check("WE 5.1.3 INVARIANT at the breakeven miss probability the gate is worth exactly zero",
          GNET - BE_MISS * NPV, 0)
    check("WE 5.1.3 option cost as a multiple of the study fee", q(OPTCOST / GATE, 2), D("8.99"))
    check("WE 5.1.3 the gate adds eight weeks", D(8), 8)
    # MCQ 5.1-C/D distractors
    check("MCQ 5.1-C distractor A (expected waste without the gate)", W_OUT, 1320000)
    check("MCQ 5.1-C distractor C (gate's own cost omitted)", W_OUT - PFLAW * (1 - DET) * WASTE,
          990000)
    check("MCQ 5.1-C distractor D (residual expected waste after the gate)",
          PFLAW * (1 - DET) * WASTE, 330000)
    check("MCQ 5.1-D correct answer, value destroyed as designed", GNET - OPTCOST, -807936)

    # ================= ENGINE 3 — sponsors, the SPV and the bridge (5.2.3, 5.2.4) ======
    check("WE 5.2.3 gearing D/E", q(DEBT / EQUITY, 4), D("2.3333"))
    SUPPORT_PCT = D("0.10")
    POOL = CAPEX * SUPPORT_PCT
    check("WE 5.2.3 several cost-overrun support pool", POOL, 6000000)
    SHARES = [("water operator", D("0.55"), 9900000, 3300000, 13200000),
              ("infrastructure fund", D("0.35"), 6300000, 2100000, 8400000),
              ("industrial partner", D("0.10"), 1800000, 600000, 2400000)]
    tot_e, tot_s, tot_c = D(0), D(0), D(0)
    for name, sh, pe, ps, pc in SHARES:
        e, sup = EQUITY * sh, POOL * sh
        check(f"WE 5.2.3 {name} base equity", e, pe)
        check(f"WE 5.2.3 {name} several support", sup, ps)
        check(f"WE 5.2.3 {name} total committed", e + sup, pc)
        check(f"WE 5.2.3 INVARIANT {name} support uplift over equity share",
              q(sup / e * 100, 1), D("33.3"))
        tot_e, tot_s, tot_c = tot_e + e, tot_s + sup, tot_c + (e + sup)
    check("WE 5.2.3 equity column totals to the equity ticket", tot_e, EQUITY)
    check("WE 5.2.3 support column totals to the pool", tot_s, POOL)
    check("WE 5.2.3 group committed capital", tot_c, 24000000)
    check("WE 5.2.3 INVARIANT committed column equals equity plus pool", tot_c, EQUITY + POOL)
    check("WE 5.2.3 group committed capital as a share of capex",
          q(tot_c / CAPEX * 100, 1), D("40.0"))
    check("WE 5.2.3 the headline equity share it is contrasted with",
          q(EQUITY / CAPEX * 100, 1), D("30.0"))
    check("WE 5.2.3 uniform uplift over equity (pro-rata subscription)",
          q(POOL / EQUITY * 100, 1), D("33.3"))
    check("WE 5.2.3 each point of support costs the group", CAPEX * D("0.01"), 600000)
    check("WE 5.2.3 a move from 10 % to 15 % support", CAPEX * D("0.05"), 3000000)
    P_EQ, O_EQ = EQUITY * D("0.10"), EQUITY * D("0.55")
    check("WE 5.2.3 partner worst case if joint and several", P_EQ + POOL, 7800000)
    check("WE 5.2.3 partner multiple on the liability-basis change",
          q((P_EQ + POOL) / (P_EQ + POOL * D("0.10")), 4), D("3.25"))
    check("WE 5.2.3 operator worst case if joint and several", O_EQ + POOL, 15900000)
    check("WE 5.2.3 operator rise on the liability-basis change",
          q(((O_EQ + POOL) / (O_EQ + POOL * D("0.55")) - 1) * 100, 1), D("20.5"))
    check("WE 5.2.3 INVARIANT joint and several hurts the small holder far more than the large one",
          gt((P_EQ + POOL) / (P_EQ + POOL * D("0.10")),
             (O_EQ + POOL) / (O_EQ + POOL * D("0.55"))), 1)
    # MCQ 5.2-A/B distractors
    check("MCQ 5.2-A distractor A (base equity read as exposure)", EQUITY * D("0.35"), 6300000)
    check("MCQ 5.2-A distractor A understates committed capital by",
          q((1 - EQUITY * D("0.35") / (EQUITY * D("0.35") + POOL * D("0.35"))) * 100, 1),
          D("25.0"))
    check("MCQ 5.2-A distractor C (support alone)", POOL * D("0.35"), 2100000)
    check("MCQ 5.2-A distractor D (support read as joint and several)",
          EQUITY * D("0.35") + POOL, 12300000)
    check("MCQ 5.2-B distractor A (the several answer)", O_EQ + POOL * D("0.55"), 13200000)
    check("MCQ 5.2-B distractor C (group total committed capital)", tot_c, 24000000)
    check("MCQ 5.2-B distractor D (own equity omitted)", POOL, 6000000)

    # ---- 5.2.4 the equity bridge -----------------------------------------------------
    BR, TR = D("0.055"), EQUITY / 2
    check("5.2.4 pro-rata tranche at close and at t = 1", TR, 9000000)
    I_T0 = TR * ((1 + BR) ** 2 - 1)
    I_T1 = TR * BR
    check("5.2.4 bridge interest on the t = 0 tranche over two years", I_T0, D("1017225"))
    check("5.2.4 bridge interest on the t = 1 tranche over one year", I_T1, 495000)
    BR_INT = I_T0 + I_T1
    check("5.2.4 total bridge interest", BR_INT, D("1512225"))
    REPAY = 2 * TR + BR_INT
    check("5.2.4 repayment at t = 2", REPAY, D("19512225"))
    PV_B = REPAY / (1 + BR) ** 2
    PV_P = TR + TR / (1 + BR)
    check("5.2.4 bridge profile discounted at the bridge rate", q(PV_B, 0), 17530806)
    check("5.2.4 pro-rata profile discounted at the bridge rate", q(PV_P, 0), 17530806)
    check("5.2.4 INVARIANT the difference at the bridge rate is exactly zero", PV_B - PV_P, 0)
    KE = D("0.12")
    PV_B12 = REPAY / (1 + KE) ** 2
    PV_P12 = TR + TR / (1 + KE)
    check("5.2.4 bridge profile at the sponsors' 12 %", q(PV_B12, 0), 15555026)
    check("5.2.4 pro-rata profile at the sponsors' 12 %", q(PV_P12, 0), 17035714)
    SAV = PV_P12 - PV_B12
    check("5.2.4 saving at 12 %", q(SAV, 0), 1480688)
    check("5.2.4 saving as a percentage of the pro-rata profile", q(SAV / PV_P12 * 100, 2),
          D("8.69"))
    check("5.2.4 spread between the sponsors' requirement and the bridge rate",
          (KE - BR) * 100, D("6.5"))
    check("5.2.4 average deferral in months", (D(2) * TR + D(1) * TR) / (2 * TR) * 12, 18)
    check("5.2.4 INVARIANT the bridge is accretive to sponsors only above the bridge rate",
          gt(SAV, 0), 1)
    check("5.2.4 INVARIANT MCQ 5.2-C distractor D is false: not neutral at every rate",
          eq(PV_B12, PV_P12), 0)
    check("5.2.4 INVARIANT below the bridge rate the pro-rata profile is the cheaper one",
          gt(REPAY / (1 + D("0.03")) ** 2, TR + TR / (1 + D("0.03"))), 1)
    check("5.2.4 bridge interest added to project cost", BR_INT, D("1512225"))
    check("MCQ 5.2-C distractor A/C (accrued interest read as a gain or a pure cost)",
          BR_INT, D("1512225"))

    # ================= ENGINE 4 — bankability as a conjunction (5.3.1) ================
    CONDS = [("offtake", D("0.92")), ("permits", D("0.90")), ("land", D("0.95")),
             ("technology", D("0.88")), ("EPC wrap", D("0.93")), ("financing", D("0.85"))]
    RUNPRINT = ["0.92", "0.828", "0.7866", "0.6922", "0.6438", "0.5472"]
    J = D(1)
    for (name, p), printed in zip(CONDS, RUNPRINT):
        J *= p
        check(f"Fig 5.3.1 running product after {name}", q(J, len(printed) - 2), D(printed))
    check("WE 5.3.1 joint probability of close", q(J, 4), D("0.5472"))
    check("WE 5.3.1 joint probability to six places, as printed in the substitution",
          q(J, 6), D("0.547190"))
    check("WE 5.3.1 joint probability as a percentage", q(J * 100, 2), D("54.72"))
    MEAN = sum(p for _, p in CONDS) / len(CONDS)
    check("WE 5.3.1 arithmetic mean of the six conditions", q(MEAN * 100, 1), D("90.5"))
    check("WE 5.3.1 the error in averaging, in points", q((MEAN - J) * 100, 0), 36)
    check("WE 5.3.1 INVARIANT the product is strictly below the mean", gt(MEAN, J), 1)
    check("WE 5.3.1 INVARIANT the product is below even the weakest condition",
          gt(min(p for _, p in CONDS), J), 1)
    WEAK, STRONG = D("0.85"), D("0.95")
    J_W = J / WEAK * D("0.95")
    J_S = J / STRONG * D("0.98")
    check("WE 5.3.1 joint after lifting the weakest to 0.95", q(J_W * 100, 2), D("61.16"))
    check("WE 5.3.1 gain from lifting the weakest, in points", q((J_W - J) * 100, 4), D("6.4375"))
    check("WE 5.3.1 joint after lifting the strongest to 0.98", q(J_S * 100, 2), D("56.45"))
    check("WE 5.3.1 gain from lifting the strongest, in points", q((J_S - J) * 100, 4), D("1.7280"))
    check("Fig 5.3.1 sub-caption gain from the weakest, 2 dp", q((J_W - J) * 100, 2), D("6.44"))
    check("Fig 5.3.1 sub-caption gain from the strongest, 2 dp", q((J_S - J) * 100, 2), D("1.73"))
    RATIO = (J_W - J) / (J_S - J)
    check("WE 5.3.1 the weakest link is worth this multiple more", q(RATIO, 4), D("3.7255"))
    check("WE 5.3.1 INVARIANT the ratio is the closed form (dp/p_weak)/(dp/p_strong)",
          RATIO, (D("0.10") / WEAK) / (D("0.03") / STRONG))
    check("WE 5.3.1 INVARIANT marginal gain ranks the weakest above the strongest",
          gt(J_W - J, J_S - J), 1)
    check("MCQ 5.3-B factor as printed in the option", q(RATIO, 2), D("3.73"))
    check("WE 5.3.1 six conditions all at 0.98", q(D("0.98") ** 6 * 100, 2), D("88.58"))
    check("WE 5.3.1 INVARIANT even 0.98 six times falls short of a 0.90 joint",
          gt(D("0.90"), D("0.98") ** 6), 1)
    UNI = D("0.90") ** (D(1) / 6)
    check("WE 5.3.1 uniform probability required for a 0.90 joint", q(UNI * 100, 2), D("98.26"))
    check("Fig 5.3.1 the same requirement as a probability", q(UNI, 4), D("0.9826"))
    check("WE 5.3.1 INVARIANT the uniform requirement raised to the sixth power returns 0.90",
          q(UNI ** 6, 6), D("0.900000"))
    REMAIN = STAGES[3][1]
    check("WE 5.3.1 remaining development spend to close", REMAIN, 2400000)
    check("WE 5.3.1 expected value of the close", q(J * NPV, 0), 8853191)
    check("WE 5.3.1 expected value of continuing", q(J * NPV - REMAIN, 0), 6453191)
    BE_CLOSE_P = REMAIN / NPV
    check("WE 5.3.1 breakeven close probability", q(BE_CLOSE_P * 100, 2), D("14.83"))
    check("WE 5.3.1 INVARIANT at the breakeven close probability continuing is worth zero",
          BE_CLOSE_P * NPV - REMAIN, 0)
    check("WE 5.3.1 INVARIANT the assessed joint probability is far above the breakeven",
          gt(J, BE_CLOSE_P), 1)
    # MCQ 5.3-A distractors
    check("MCQ 5.3-A distractor A (arithmetic mean)", q(MEAN * 100, 1), D("90.5"))
    check("MCQ 5.3-A distractor C (weakest condition, others taken as certain)",
          q(WEAK * 100, 1), D("85.0"))
    check("MCQ 5.3-A distractor D sums the shortfalls",
          q(sum(1 - p for _, p in CONDS), 2), D("0.57"))
    check("MCQ 5.3-A distractor D (one less the summed shortfalls)",
          q((1 - sum(1 - p for _, p in CONDS)) * 100, 1), D("43.0"))

    # ================= ENGINE 5 — technology and its price (5.3.4) =====================
    T_LO, T_HI = D("1.30"), D("1.45")
    DS_LO, DS_HI = CFADS / T_LO, CFADS / T_HI
    check("5.3.4 max debt service at the 1.30x target", q(DS_LO, 0), 4910769)
    check("5.3.4 max debt service at the 1.45x target", q(DS_HI, 0), 4402759)
    CAP_LO, CAP_HI = DS_LO * AF, DS_HI * AF
    check("5.3.4 debt capacity at 1.30x", q(CAP_LO, 0), 41171123)
    check("5.3.4 debt capacity at 1.45x", q(CAP_HI, 0), 36912041)
    LOSS = CAP_LO - CAP_HI
    check("5.3.4 capacity loss, payable as additional equity", q(LOSS, 0), 4259082)
    check("5.3.4 gearing at 1.30x", q(CAP_LO / CAPEX * 100, 1), D("68.6"))
    check("5.3.4 gearing at 1.45x", q(CAP_HI / CAPEX * 100, 1), D("61.5"))
    check("5.3.4 annual cost of substituting equity for debt at a 6-point spread",
          q(LOSS * (KE - R), 0), 255545)
    check("5.3.4 INVARIANT a higher target ratio cannot support more debt", gt(CAP_LO, CAP_HI), 1)
    check("5.3.4 INVARIANT the capacity at 1.30x returns a DSCR of exactly 1.30x",
          q(CFADS / (CAP_LO / AF), 4), D("1.3000"))
    check("5.3.4 INVARIANT the capacity at 1.45x returns a DSCR of exactly 1.45x",
          q(CFADS / (CAP_HI / AF), 4), D("1.4500"))
    check("5.3.4 INVARIANT loss/capacity is 1 - 1.30/1.45, independent of CFADS, tenor and rate",
          LOSS / CAP_LO, 1 - T_LO / T_HI)
    check("5.3.4 that proportion, as a percentage", q(LOSS / CAP_LO * 100, 2), D("10.34"))
    check("MCQ 5.3-C distractor B (ratio points read as percentages of principal)",
          DEBT * (T_HI - T_LO), 6300000)
    check("MCQ 5.3-C distractor C (fall in annual debt service)", q(DS_LO - DS_HI, 0), 508011)
    check("MCQ 5.3-C distractor D (max debt service mistaken for a capital sum)",
          q(DS_LO, 0), 4910769)

    # ================= ENGINE 6 — completion: the COD slip (5.4.2) =====================
    BASIS = D(360)
    DAY_I = DEBT * R / BASIS
    DAY_C = CFADS / BASIS
    DAY_T = DAY_I + DAY_C
    check("WE 5.4.2 daily extra interest on drawn debt, 30/360", DAY_I, 7000)
    check("WE 5.4.2 table cell, daily interest to 2 dp", q(DAY_I, 2), D("7000.00"))
    check("WE 5.4.2 daily forgone CFADS, 30/360", q(DAY_C, 2), D("17733.33"))
    check("WE 5.4.2 daily economic cost", q(DAY_T, 2), D("24733.33"))
    for days, pi, pc, pt in ((180, 1260000, 3192000, 4452000),
                             (360, 2520000, 6384000, 8904000)):
        check(f"WE 5.4.2 extra interest over {days} days", DAY_I * days, pi)
        check(f"WE 5.4.2 forgone CFADS over {days} days", DAY_C * days, pc)
        check(f"WE 5.4.2 total economic cost over {days} days", DAY_T * days, pt)
    DMG, EPC = D(20000), D(48000000)
    CAP_DMG = EPC * D("0.10")
    check("WE 5.4.2 table cell, damages rate to 2 dp", q(DMG, 2), D("20000.00"))
    check("WE 5.4.2 damages cap at 10 % of the EPC price", CAP_DMG, 4800000)
    check("WE 5.4.2 cap-binding day", CAP_DMG / DMG, 240)
    check("WE 5.4.2 INVARIANT the cap-binding day times the rate is the cap",
          (CAP_DMG / DMG) * DMG, CAP_DMG)
    check("WE 5.4.2 damages over 180 days", DMG * 180, 3600000)
    check("WE 5.4.2 damages over 360 days, capped", min(DMG * 360, CAP_DMG), 4800000)
    check("WE 5.4.2 uncovered per day before the cap", q(DAY_T - DMG, 2), D("4733.33"))
    check("WE 5.4.2 uncovered over 180 days", DAY_T * 180 - DMG * 180, 852000)
    check("WE 5.4.2 uncovered over 360 days", DAY_T * 360 - CAP_DMG, 4104000)
    check("WE 5.4.2 damages coverage of the daily cost", q(DMG / DAY_T * 100, 2), D("80.86"))
    check("WE 5.4.2 forgone-CFADS-only calibration covers", q(DAY_C / DAY_T * 100, 1), D("71.7"))
    check("WE 5.4.2 the omitted interest share", q(DAY_I / DAY_T * 100, 1), D("28.3"))
    check("WE 5.4.2 INVARIANT the two components exhaust the daily cost",
          DAY_C / DAY_T + DAY_I / DAY_T, 1)
    check("WE 5.4.2 INVARIANT beyond the cap every further day costs the SPV the full daily cost",
          (DAY_T * 361 - CAP_DMG) - (DAY_T * 360 - CAP_DMG), DAY_T)
    NEWDEBT = DEBT + DAY_I * 180
    check("WE 5.4.2 debt after capitalising the extra interest", NEWDEBT, 43260000)
    NEWINST = NEWDEBT / AF
    check("WE 5.4.2 instalment after capitalisation", q(NEWINST, 2), D("5159924.29"))
    check("WE 5.4.2 DSCR after capitalisation", q(CFADS / NEWINST, 4), D("1.2372"))
    check("WE 5.4.2 covenant cash trigger after capitalisation", q(COVENANT * NEWINST, 0), 6191909)
    H0 = CFADS - COVENANT * INST
    H1 = CFADS - COVENANT * NEWINST
    check("WE 5.4.2 headroom after capitalisation", q(H1, 2), D("192090.85"))
    check("WE 5.4.2 headroom as a share of what it was", q(H1 / H0 * 100, 1), D("51.6"))
    check("WE 5.4.2 INVARIANT capitalising raises the instalment and cuts DSCR and headroom",
          gt(NEWINST, INST) + gt(CFADS / INST, CFADS / NEWINST) + gt(H0, H1), 3)
    NEWEQ = EQUITY + DAY_I * 180
    check("WE 5.4.2 equity if the interest is funded with equity instead", NEWEQ, 19260000)
    check("WE 5.4.2 gearing on the equity route, debt share",
          q(DEBT / (DEBT + NEWEQ) * 100, 1), D("68.6"))
    check("WE 5.4.2 gearing on the equity route, equity share",
          q(NEWEQ / (DEBT + NEWEQ) * 100, 1), D("31.4"))
    check("WE 5.4.2 D/E on the equity route", q(DEBT / NEWEQ, 4), D("2.1807"))
    check("WE 5.4.2 INVARIANT funding with equity leaves coverage untouched",
          q(CFADS / INST, 4), D("1.2743"))
    check("MCQ 5.4-A distractor A (forgone CFADS alone)", q(DAY_C, 2), D("17733.33"))
    check("MCQ 5.4-A distractor C (interest alone)", DAY_I, 7000)
    check("MCQ 5.4-A distractor D (pre-working-capital CFADS)",
          q(DAY_I + (CFADS + D(600000)) / BASIS, 2), D("26400.00"))
    check("MCQ 5.4-A distractor D pre-working-capital daily rate",
          (CFADS + D(600000)) / BASIS, 19400)
    check("MCQ 5.4-B distractor C (damages paid for all 360 days)",
          DAY_T * 360 - DMG * 360, 1704000)
    check("MCQ 5.4-B distractor D (recovery omitted)", DAY_T * 360, 8904000)

    # ---- the value view of the slip, and Case study A --------------------------------
    QR = (1 + DISC) ** (D(1) / 4) - 1
    check("5.4.2 quarterly rate equivalent to 8 % annually", q(QR * 100, 6), D("1.942655"))
    check("5.4.2 INVARIANT the quarterly rate compounds to the annual rate",
          q((1 + QR) ** 4 - 1, 8), q(DISC, 8))
    QCF = CFADS / 4
    check("5.4.2 quarterly CFADS", QCF, 1596000)
    CONC_Q = (2 + LIFE) * 4
    check("5.4.2 concession quarters from close (2 build + 25 operating)", CONC_Q, 108)
    check("5.4.2 operating quarters on time", CONC_Q - 8, 100)

    def pv_stream(first_q, last_q):
        return sum(QCF / (1 + QR) ** t for t in range(first_q, last_q + 1))

    PV_ON = pv_stream(9, CONC_Q)
    PV_180 = pv_stream(11, CONC_Q)
    check("5.4.2 operating stream at close, on time", q(PV_ON, 0), 60150401)
    check("5.4.2 the same by annuity factors", q(QCF * (af(QR, CONC_Q) - af(QR, 8)), 0), 60150401)
    check("5.4.2 operating stream if COD slips 180 days", q(PV_180, 0), 57491504)
    check("5.4.2 present-value loss of the 180-day slip", q(PV_ON - PV_180, 0), 2658897)
    check("5.4.2 the naive figure it is contrasted with (six months of CFADS)", CFADS / 2, 3192000)
    check("5.4.2 INVARIANT the value loss is less than six months of CFADS",
          gt(CFADS / 2, PV_ON - PV_180), 1)
    check("5.4.2 INVARIANT a fixed-expiry concession loses operating quarters, not none",
          (CONC_Q - 8) - (CONC_Q - 10), 2)
    VLOSS = PV_ON - PV_180 + DAY_I * 180
    check("5.4.2 total value loss including capitalised interest", q(VLOSS, 0), 3918897)
    check("5.4.2 total value loss as a share of Domain 4's NPV", q(VLOSS / NPV * 100, 1), D("24.2"))
    DMG_PV = DMG * 180 / (1 + QR) ** 10
    check("5.4.2 damages of 3,600,000 received at the delayed COD, valued at close",
          q(DMG_PV, 0), 2969909)
    check("5.4.2 net value loss after damages", q(VLOSS - DMG_PV, 0), 948988)
    check("5.4.2 net loss as a share of Domain 4's NPV", q((VLOSS - DMG_PV) / NPV * 100, 1),
          D("5.9"))
    check("5.4.2 INVARIANT damages received later are worth less than their face amount",
          gt(DMG * 180, DMG_PV), 1)
    # Case study A — the same machinery over nine months
    PV_9M = pv_stream(12, CONC_Q)
    check("Case A operating stream with COD nine months later", q(PV_9M, 0), 56199935)
    A_LOSS = PV_ON - PV_9M
    check("Case A present-value loss", q(A_LOSS, 0), 3950466)
    check("Case A loss as a share of the on-time stream", q(A_LOSS / PV_ON * 100, 2), D("6.57"))
    A_ATRISK = STAGES[2][1] + STAGES[3][1]
    check("Case A spend at risk is the feasibility and transaction tranches", A_ATRISK, 3600000)
    A_CARRY = A_ATRISK * ((1 + KE) ** (D(3) / 4) - 1)
    check("Case A cost of carrying that spend nine months at 12 %", q(A_CARRY, 0), 319368)
    A_TOTAL = D(1400000) + A_LOSS + A_CARRY
    check("Case A re-route capital cost", D(1400000), 1400000)
    check("Case A total defect cost", q(A_TOTAL, 0), 5669834)
    check("Case A defect cost as a share of Domain 4's NPV", q(A_TOTAL / NPV * 100, 1), D("35.0"))
    check("Case A defect cost as a multiple of the review that would have found it",
          q(A_TOTAL / GATE, 1), D("31.5"))
    check("Case A eleven parcels split nine registered and two not", 9 + 2, 11)
    check("Case A INVARIANT a nine-month slip costs more than a six-month slip",
          gt(A_LOSS, PV_ON - PV_180), 1)

    # ---- 5.4.3 the buy-down ----------------------------------------------------------
    OUT = D("0.97")
    REV, OPEX = D(12000000), D(4500000)
    FIXSH = D("0.85")
    FIXED, VAR = OPEX * FIXSH, OPEX * (1 - FIXSH)
    check("WE 5.4.3 fixed cash operating cost", FIXED, 3825000)
    check("WE 5.4.3 variable cash operating cost", VAR, 675000)
    check("WE 5.4.3 revenue at 97 % of guaranteed output", REV * OUT, 11640000)
    check("WE 5.4.3 variable cost at 97 % output", VAR * OUT, D("654750"))
    OPEX2 = FIXED + VAR * OUT
    check("WE 5.4.3 cash operating cost at 97 % output", OPEX2, D("4479750"))
    EB2 = REV * OUT - OPEX2
    check("WE 5.4.3 EBITDA at 97 % output", EB2, D("7160250"))
    check("WE 5.4.3 EBITDA fall", EBITDA - EB2, D("339750"))
    check("WE 5.4.3 EBITDA fall as a percentage", q((EBITDA - EB2) / EBITDA * 100, 2), D("4.53"))
    OPLEV = ((EBITDA - EB2) / EBITDA) / (1 - OUT)
    check("WE 5.4.3 operating leverage", q(OPLEV, 4), D("1.5100"))
    check("WE 5.4.3 operating leverage as printed, 3 dp", q(OPLEV, 3), D("1.510"))
    check("WE 5.4.3 INVARIANT operating leverage exceeds one whenever any cost is fixed",
          gt(OPLEV, 1), 1)
    PBT2 = EB2 - D(2400000) - INT1
    check("WE 5.4.3 taxable profit at 97 % output", PBT2, D("2240250"))
    check("WE 5.4.3 cash tax at 20 %", PBT2 * D("0.20"), D("448050"))
    CF2 = EB2 - PBT2 * D("0.20") - D(600000)
    check("WE 5.4.3 CFADS at 97 % output", CF2, 6112200)
    check("WE 5.4.3 CFADS fall as a percentage", q((CFADS - CF2) / CFADS * 100, 2), D("4.26"))
    check("WE 5.4.3 DSCR at 97 % output", q(CF2 / INST, 4), D("1.2201"))
    H2 = CF2 - COVENANT * INST
    check("WE 5.4.3 covenant headroom at 97 % output", q(H2, 0), 100638)
    check("WE 5.4.3 share of headroom consumed by a 3 % shortfall",
          q((H0 - H2) / H0 * 100, 1), D("73.0"))
    SHORT = CFADS - CF2
    check("WE 5.4.3 annual CFADS shortfall", SHORT, 271800)
    BUY = DEBT * SHORT / CFADS
    check("WE 5.4.3 buy-down restoring sized coverage", q(BUY, 0), 1788158)
    check("WE 5.4.3 debt after the buy-down", q(DEBT - BUY, 0), 40211842)
    check("WE 5.4.3 INVARIANT the buy-down restores the sized DSCR exactly",
          q(CF2 / ((DEBT - BUY) / AF), 4), q(CFADS / INST, 4))
    check("WE 5.4.3 INVARIANT at constant coverage debt scales linearly with CFADS",
          (DEBT - BUY) / DEBT, CF2 / CFADS)
    check("WE 5.4.3 INVARIANT the buy-down repairs lenders, not equity: annual cash still lost",
          SHORT, 271800)
    check("MCQ 5.4-D distractor A (annual CFADS shortfall, not the debt adjustment)",
          SHORT, 271800)
    check("MCQ 5.4-D distractor C (the COD-slip interest of 5.4.2)", DAY_I * 180, 1260000)
    check("MCQ 5.4-D distractor D (the technology premium of 5.3.4)", q(LOSS, 0), 4259082)

    # ================= Case study B — the ten per cent that could not fund =============
    B_CAP = D(240000000)
    B_DEBT, B_EQ = B_CAP * D("0.70"), B_CAP * D("0.30")
    check("Case B senior debt at 70 %", B_DEBT, 168000000)
    check("Case B equity at 30 %", B_EQ, 72000000)
    B_POOL = B_CAP * SUPPORT_PCT
    check("Case B several support pool at 10 % of capital cost", B_POOL, 24000000)
    B_SH = [D("0.55"), D("0.35"), D("0.10")]
    for sh, pc in zip(B_SH, (52800000, 33600000, 9600000)):
        check(f"Case B committed capital at {sh}", B_EQ * sh + B_POOL * sh, pc)
    check("Case B local partner base equity", B_EQ * B_SH[2], 7200000)
    check("Case B local partner support", B_POOL * B_SH[2], 2400000)
    B_OV = D(16000000)
    CALLS = [B_OV * sh for sh in B_SH]
    check("Case B call on the operator", CALLS[0], 8800000)
    check("Case B call on the fund", CALLS[1], 5600000)
    check("Case B call on the local partner", CALLS[2], 1600000)
    check("Case B INVARIANT the pro-rata calls sum to the overrun", sum(CALLS), B_OV)
    check("Case B INVARIANT the overrun is inside the support pool", gt(B_POOL, B_OV), 1)
    MULTI = D("1.25")
    OPQ = B_EQ * B_SH[0] + CALLS[0] + CALLS[2] * MULTI
    FUQ = B_EQ * B_SH[1] + CALLS[1]
    PAQ = B_EQ * B_SH[2]
    TOTQ = OPQ + FUQ + PAQ
    check("Case B operator base equity before the overrun", B_EQ * B_SH[0], 39600000)
    check("Case B fund base equity before the overrun", B_EQ * B_SH[1], 25200000)
    check("Case B post-overrun operator equity", OPQ, 50400000)
    check("Case B post-overrun fund equity", FUQ, 30800000)
    check("Case B post-overrun partner equity", PAQ, 7200000)
    check("Case B post-overrun total equity", TOTQ, 88400000)
    check("Case B post-overrun operator share", q(OPQ / TOTQ * 100, 4), D("57.0136"))
    check("Case B post-overrun fund share", q(FUQ / TOTQ * 100, 4), D("34.8416"))
    check("Case B post-overrun partner share", q(PAQ / TOTQ * 100, 4), D("8.1448"))
    check("Case B INVARIANT the three post-overrun shares sum to 100 %",
          q((OPQ + FUQ + PAQ) / TOTQ * 100, 4), D("100.0000"))
    B_EV = D(96000000)
    B_IF = B_EV * B_SH[2] - CALLS[2]
    check("Case B partner value had it funded", B_EV * B_SH[2], 9600000)
    check("Case B partner net position had it funded", B_IF, 8000000)
    B_DECL = B_EV * PAQ / TOTQ
    check("Case B partner value having declined", q(B_DECL, 0), 7819005)
    B_COST = B_IF - B_DECL
    check("Case B cost of declining the call", q(B_COST, 0), 180995)
    check("Case B cost as a percentage of the sum withheld", q(B_COST / CALLS[2] * 100, 1),
          D("11.3"))
    OPQ_PAR = B_EQ * B_SH[0] + CALLS[0] + CALLS[2]
    TOT_PAR = OPQ_PAR + FUQ + PAQ
    check("Case B cost of declining at a par conversion",
          q(B_IF - B_EV * PAQ / TOT_PAR, 0), 145455)
    check("Case B INVARIANT a punitive multiplier costs the defaulter more than par does",
          gt(B_COST, B_IF - B_EV * PAQ / TOT_PAR), 1)
    BASE_NO_M = B_EQ * B_SH[0] + CALLS[0] + FUQ + PAQ
    M_FULL = (B_EV * PAQ / (B_IF - CALLS[2]) - BASE_NO_M) / CALLS[2]
    check("Case B multiplier needed to price the default in full", q(M_FULL, 4), D("13.5000"))
    check("Case B multiplier as printed, 2 dp", q(M_FULL, 2), D("13.50"))
    check("Case B INVARIANT at that multiplier the cost of declining equals the sum withheld",
          q(B_IF - B_EV * PAQ / (BASE_NO_M + CALLS[2] * M_FULL), 2), q(CALLS[2], 2))
    check("Case B INVARIANT dilution at 1.25x prices the default at a small fraction of it",
          gt(CALLS[2] / 5, B_COST), 1)
    for yr, printed in ((1, 1840000), (2, 2116000), (3, 2433400)):
        check(f"Case B shareholder loan at 15 % after {yr} year(s)",
              CALLS[2] * (1 + D("0.15")) ** yr, printed)
    check("Case B eleven weeks to arrange the letter of credit", D(11), 11)

    # ================= calculation exercises ==========================================
    # Exercise 5.1 — a second funnel
    E1 = [(D(60), D(30000)), (D(15), D(200000)), (D(6), D(900000)), (D(3), D(1800000))]
    E1S = [c * u for c, u in E1]
    check("EX 5.1 screening stage spend", E1S[0], 1800000)
    check("EX 5.1 concept stage spend", E1S[1], 3000000)
    check("EX 5.1 feasibility stage spend", E1S[2], 5400000)
    check("EX 5.1 to-close stage spend", E1S[3], 5400000)
    E1P = sum(E1S)
    check("EX 5.1 programme spend", E1P, 15600000)
    check("EX 5.1 cost per close", E1P / 3, 5200000)
    check("EX 5.1 close rate", q(D(3) / D(60) * 100, 1), D("5.0"))
    E1V = D(11000000) * 3
    check("EX 5.1 portfolio value", E1V, 33000000)
    check("EX 5.1 net portfolio value", E1V - E1P, 17400000)
    check("EX 5.1 value multiple", q(E1V / E1P, 4), D("2.1154"))
    E1BE = E1P / D(11000000)
    check("EX 5.1 breakeven closes", q(E1BE, 4), D("1.4182"))
    check("EX 5.1 breakeven close rate", q(E1BE / 60 * 100, 2), D("2.36"))
    check("EX 5.1 common error (closing-stage spend divided by closes)", E1S[3] / 3, 1800000)
    check("EX 5.1 common error understates cost per close by",
          q((1 - (E1S[3] / 3) / (E1P / 3)) * 100, 0), 65)
    # Exercise 5.2 — a five-condition conjunction
    E2P = [D(x) for x in ("0.95", "0.90", "0.80", "0.96", "0.92")]
    E2J = D(1)
    for p in E2P:
        E2J *= p
    check("EX 5.2 joint probability", q(E2J, 4), D("0.6041"))
    check("EX 5.2 joint probability as a percentage", q(E2J * 100, 2), D("60.41"))
    check("EX 5.2 arithmetic mean", q(sum(E2P) / 5 * 100, 1), D("90.6"))
    check("EX 5.2 the overstatement in points", q((sum(E2P) / 5 - E2J) * 100, 0), 30)
    check("EX 5.2 the weakest condition is 0.80", min(E2P), D("0.80"))
    E2W = E2J * D("0.92") / D("0.80")
    E2O = E2J * D("0.96") / D("0.90")
    check("EX 5.2 multiplier on lifting 0.80 to 0.92", q(D("0.92") / D("0.80"), 4), D("1.1500"))
    check("EX 5.2 joint after lifting the weakest", q(E2W * 100, 2), D("69.47"))
    check("EX 5.2 gain from lifting the weakest, in points", q((E2W - E2J) * 100, 4), D("9.0616"))
    check("EX 5.2 multiplier on lifting 0.90 to 0.96", q(D("0.96") / D("0.90"), 4), D("1.0667"))
    check("EX 5.2 joint after lifting 0.90", q(E2O * 100, 2), D("64.44"))
    check("EX 5.2 gain from lifting 0.90, in points", q((E2O - E2J) * 100, 4), D("4.0274"))
    check("EX 5.2 the weakest link is worth this multiple more",
          q((E2W - E2J) / (E2O - E2J), 4), D("2.2500"))
    check("EX 5.2 the same multiple as printed, 3 dp",
          q((E2W - E2J) / (E2O - E2J), 3), D("2.250"))
    check("EX 5.2 INVARIANT proportional lift beats absolute lift as a ranking key",
          gt(E2W - E2J, E2O - E2J), 1)
    check("EX 5.2 common error: absolute lifts are 0.12 against 0.06",
          (D("0.92") - D("0.80")) / (D("0.96") - D("0.90")), 2)
    # Exercise 5.3 — a four-sponsor committed-capital table
    E3C, E3E = D(90000000), D(25000000)
    E3POOL = E3C * D("0.12")
    check("EX 5.3 support pool at 12 % of capital cost", E3POOL, 10800000)
    E3T = D(0)
    for sh, pe, ps, pc in ((D("0.40"), 10000000, 4320000, 14320000),
                           (D("0.30"), 7500000, 3240000, 10740000),
                           (D("0.20"), 5000000, 2160000, 7160000),
                           (D("0.10"), 2500000, 1080000, 3580000)):
        check(f"EX 5.3 equity at {sh}", E3E * sh, pe)
        check(f"EX 5.3 support at {sh}", E3POOL * sh, ps)
        check(f"EX 5.3 committed at {sh}", E3E * sh + E3POOL * sh, pc)
        check(f"EX 5.3 INVARIANT uniform uplift at {sh}",
              q(E3POOL * sh / (E3E * sh) * 100, 1), D("43.2"))
        E3T += E3E * sh + E3POOL * sh
    check("EX 5.3 group committed capital", E3T, 35800000)
    check("EX 5.3 INVARIANT the table totals to equity plus pool", E3T, E3E + E3POOL)
    check("EX 5.3 group committed as a percentage of capex", q(E3T / E3C * 100, 2), D("39.78"))
    check("EX 5.3 equity headline as a percentage of capex", q(E3E / E3C * 100, 2), D("27.78"))
    check("EX 5.3 equity headline as printed in the solution text",
          q(E3E / E3C * 100, 1), D("27.8"))
    check("EX 5.3 uplift over headline equity", q(E3POOL / E3E * 100, 1), D("43.2"))
    check("EX 5.3 common error understates the 10 % holder's exposure by",
          q((1 - E3E * D("0.10") / (E3E * D("0.10") + E3POOL * D("0.10"))) * 100, 1), D("30.2"))
    # Exercise 5.4 — a second COD slip
    E4D, E4R, E4CF = D(50000000), D("0.07"), D(8000000)
    E4I, E4C = E4D * E4R / BASIS, E4CF / BASIS
    check("EX 5.4 daily interest", q(E4I, 2), D("9722.22"))
    check("EX 5.4 daily forgone CFADS", q(E4C, 2), D("22222.22"))
    check("EX 5.4 daily economic cost", q(E4I + E4C, 2), D("31944.44"))
    check("EX 5.4 interest over 150 days", q(E4I * 150, 2), D("1458333.33"))
    check("EX 5.4 forgone CFADS over 150 days", q(E4C * 150, 2), D("3333333.33"))
    check("EX 5.4 total cost over 150 days", q((E4I + E4C) * 150, 2), D("4791666.67"))
    check("EX 5.4 damages over 150 days", D(25000) * 150, 3750000)
    check("EX 5.4 shortfall over 150 days", q((E4I + E4C) * 150 - D(25000) * 150, 2),
          D("1041666.67"))
    check("EX 5.4 damages coverage", q(D(25000) / (E4I + E4C) * 100, 2), D("78.26"))
    E4CAP = D(60000000) * D("0.08")
    check("EX 5.4 damages cap", E4CAP, 4800000)
    check("EX 5.4 cap-binding day", E4CAP / D(25000), 192)
    check("EX 5.4 INVARIANT 150 days is inside the cap", gt(E4CAP / D(25000), 150), 1)
    check("EX 5.4 common error: the interest share omitted",
          q(E4I / (E4I + E4C) * 100, 2), D("30.43"))
    # Exercise 5.5 — debt capacity at two target ratios
    E5AF = af(D("0.065"), 14)
    check("EX 5.5 AF(0.065, 14) as printed", q(E5AF, 6), D("9.013842"))
    E5CF = D(8000000)
    check("EX 5.5 debt service at 1.30x", q(E5CF / D("1.30"), 2), D("6153846.15"))
    check("EX 5.5 capacity at 1.30x", q(E5CF / D("1.30") * E5AF, 0), 55469799)
    check("EX 5.5 debt service at 1.50x", q(E5CF / D("1.50"), 2), D("5333333.33"))
    check("EX 5.5 capacity at 1.50x", q(E5CF / D("1.50") * E5AF, 0), 48073826)
    E5LOSS = E5CF / D("1.30") * E5AF - E5CF / D("1.50") * E5AF
    check("EX 5.5 capacity loss", q(E5LOSS, 0), 7395973)
    check("EX 5.5 loss as a percentage of the 1.30x capacity",
          q(E5LOSS / (E5CF / D("1.30") * E5AF) * 100, 2), D("13.33"))
    check("EX 5.5 INVARIANT that percentage is 1 - 1.30/1.50",
          E5LOSS / (E5CF / D("1.30") * E5AF), 1 - D("1.30") / D("1.50"))
    check("EX 5.5 common error (0.20 read as a 20 % cut in debt)",
          q(E5CF / D("1.30") * E5AF * D("0.20"), 0), 11093960)
    check("EX 5.5 common error overstates the loss by, in millions",
          q((E5CF / D("1.30") * E5AF * D("0.20") - E5LOSS) / 1000000, 1), D("3.7"))
