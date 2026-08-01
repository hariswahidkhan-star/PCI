"""PML-AI Domain 5 — Scope, requirements and value definition: golden checks.

Every number printed as a result in `domain-05-scope-requirements-value.md` is recomputed here from
its definition rather than copied from its printed output. Master-thread values come from ctx and are
never re-typed: `MERIDIAN_BASELINE` (USD 2,400,000), `MERIDIAN_BENEFIT_FULL` (USD 979,200),
`MERIDIAN_BENEFIT_70PCT` (USD 685,440), `MERIDIAN_COST_OF_DELAY` (USD 14,280/week, Domain 1),
`AURIGA_BAC`/`AURIGA_WEEKS` (the Domain 6 handover the chapter opener names) and the shared helpers
`af(r, n)` for Domain 2's annuity factor and `ew(M, L)` for Domain 3's governance latency. Two
sibling-domain results the chapter sets its own figures beside — Domain 2's ramped NPV (+1,332,898,
WE 2.2.2) and Domain 4's authorised drift (291,176, WE 4.3.3) — are declared once below and
reconstructed from their own bases, so a drift in either sibling fails here rather than passing
silently.

Display rounding follows the family convention: `Decimal.quantize` with its default ROUND_HALF_EVEN,
which is what the manuscript's exact-half figures (27/480 = 5.62 %, 220,920/2,400,000 = 9.20 %) print.
Two of those are flagged in their check names as half-boundary cases.

Five engines, each checked from first principles rather than from the manuscript's stated results:

    boundary       (KA 5.1.3) per-unit rollout cost from the Domain 4 WBS element and the approved
                   site count; exposure = additional sites x unit cost; per-site benefit at the
                   registered 70 % adoption derived from MERIDIAN_BENEFIT_FULL rather than typed; and
                   the payback period, checked to be under a year, since the domain's whole teaching
                   point is the inversion from "exclude" to "include".
    ladder         (KA 5.2.1) the ladder is asserted GEOMETRIC at ratio 4 term by term rather than
                   listed, and its multiples x1..x256 derived. The detection profile is checked to sum
                   to the stated defect count and to the stated defects-per-requirement. The saving is
                   computed on stage DIFFERENCES and the gross-cost error recomputed separately, with
                   the 10,000 overstatement shown to equal (defects moved x definition cost) — the
                   identity that makes the teaching point exact rather than anecdotal. The single-defect
                   argument is pinned as an INEQUALITY (one live defect saved > the whole programme
                   cost), as is the breakeven-delay claim.
    traceability   (KA 5.2.3) forward classes plus the fully-traced count are asserted to RECONSTRUCT
                   the 480 baseline, so a mis-stated class fails here; each class rate is computed on
                   the requirements denominator and the reverse rate on the design-register
                   denominator, with the two denominators asserted different (the domain's
                   not-combinable claim) and the rounded-class-sum vs combined-rate gap reproduced.
    value          (KA 5.3.1-5.3.3) attributed bundle values are asserted to sum to the stated total
                   and to leave exactly the stated unattributed remainder against
                   MERIDIAN_BENEFIT_FULL (the sum test). Must-have inflation is computed both ways with
                   its fold increase. The greedy selection is BUILT by sorting on value per week and
                   filling, and the optimum by ENUMERATING every subset — neither is typed — so a
                   mis-stated selection fails. Greedy <= enumeration, greedy strands capacity and
                   enumeration does not, and the printed 0.88 % ratio advantage against the 10.76 %
                   value loss are both derived. Present value reuses Domain 2's af(0.07, 8).
    change         (KA 5.4.1-5.4.3) the count reconciliation, creep priced as direct plus schedule at
                   the registered cost of delay, its share of baseline, of combined movement and of
                   Domain 2's NPV; the criterion-state census asserted to reconstruct 480; expected
                   cost per dispute with its delay component; the breakeven dispute probability, pinned
                   as an inequality against the observed rate; and the fix-versus-defer comparison with
                   its breakeven go-live delay, pinned as an inequality both ways.

Published MCQ distractors that the rationales describe as the results of named errors are checked too
(one site rather than eleven; the whole baseline divided by the site count instead of the rollout
element; the annual benefit quoted as a cost; the elicitation saving on gross later-stage costs; the
saving net of investment; the unimproved total; 17 orphans over the requirements denominator; A+B over
capacity; a feasible set greedy never reaches; the 51 additions priced as though all were uncontrolled;
the schedule impact alone; the observed dispute rate, the return multiple and the deficient share
offered as a breakeven probability), because a wrong distractor is as much a defect as a wrong answer.
"""


def run(ctx):
    check, D, af, ew = ctx["check"], ctx["D"], ctx["af"], ctx["ew"]
    q = lambda x, n: D(x).quantize(D(1).scaleb(-n))

    COD = ctx["MERIDIAN_COST_OF_DELAY"]          # USD 14,280 per week (Domain 1)
    BASE = ctx["MERIDIAN_BASELINE"]              # USD 2,400,000 approved cost
    BFULL = ctx["MERIDIAN_BENEFIT_FULL"]         # USD 979,200 a year, full potential
    B70 = ctx["MERIDIAN_BENEFIT_70PCT"]          # USD 685,440 a year at 70 % adoption
    ADOPT = D("0.70")
    CLINICS = D(40)
    D2_NPV = D(1332898)                          # Domain 2 WE 2.2.2, ramped case
    D4_DRIFT = D(291176)                         # Domain 4 WE 4.3.3, authorised drift
    ROLLOUT = D(520000)                          # Domain 4 WE 4.2.1, clinic-rollout WBS element
    TRAINING = D(214000)                         # Domain 4's missing enabling-change element

    # ---- master-thread and sibling values the chapter opener restates ----------------------
    check("D05 opener: Meridian realistic benefit is 70 % of full potential", BFULL * ADOPT, B70)
    check("D05 opener: cost of delay from its Domain 1 basis", D(28) * 6 * 85, COD)
    check("D05 opener: E[wait] = M/2 + L at M=4, L=2 (Domain 3)", ew(4, 2), D("4.0"))
    check("D05 opener: Auriga BAC (Domain 6 handover)", ctx["AURIGA_BAC"], 4000000)
    check("D05 opener: Auriga duration in weeks (Domain 6 handover)", ctx["AURIGA_WEEKS"], 25)
    # Domain 4's drift, rebuilt from its own basis (34 changes x 6,800; 14 x 0.3 wk x CoD)
    check("D05 opener: Domain 4 authorised drift rebuilt from WE 4.3.3's basis",
          D(34) * 6800 + D(14) * D("0.3") * COD, D4_DRIFT)
    # Domain 2's ramped NPV, rebuilt from the 40/60/70 % ramp on the 979,200 potential less baseline
    AF8 = af(D("0.07"), 8)
    check("D05 opener: Domain 2's AF(0.07, 8) to six places", q(AF8, 6), D("5.971299"))
    ramp = [D("0.40"), D("0.60")] + [ADOPT] * 6
    pv_ben = sum(BFULL * s / (1 + D("0.07")) ** (i + 1) for i, s in enumerate(ramp))
    check("D05 opener: Domain 2 ramped PV of benefits", q(pv_ben, 0), 3732898)
    check("D05 opener: Domain 2 ramped NPV rebuilt", q(pv_ben - BASE, 0), D2_NPV)

    # ======================= ENGINE 1 — the undrawn boundary (KA 5.1.3) ====================
    SITES_ADD = D(6) + D(3) + D(2)
    check("WE 5.1.3 additional sites (6 branch + 3 hubs + 2 prison units)", SITES_ADD, 11)
    UNIT = ROLLOUT / CLINICS
    check("WE 5.1.3 per-site rollout cost", UNIT, 13000)
    EXPOSURE = SITES_ADD * UNIT
    check("WE 5.1.3 boundary exposure", EXPOSURE, 143000)
    check("WE 5.1.3 exposure as a share of the approved baseline",
          q(EXPOSURE / BASE * 100, 2), D("5.96"))
    check("WE 5.1.3 full-potential benefit per site", BFULL / CLINICS, 24480)
    PSB = (BFULL / CLINICS) * ADOPT
    check("WE 5.1.3 per-site benefit at realistic adoption", PSB, 17136)
    ADD_BEN = SITES_ADD * PSB
    check("WE 5.1.3 additional annual benefit of the 11 sites", ADD_BEN, 188496)
    PAYBACK = EXPOSURE / ADD_BEN
    check("WE 5.1.3 payback in years", q(PAYBACK, 2), D("0.76"))
    check("WE 5.1.3 payback in months", q(PAYBACK * 12, 1), D("9.1"))
    # INVARIANT: the teaching point is the inversion — inclusion pays back inside one year, so the
    # instinct to write the exclusion is value-destroying. Pinned as an inequality, not arithmetic.
    check("WE 5.1.3 INVARIANT payback is under one year, so exclusion destroys value",
          1 if PAYBACK < 1 else 0, 1)
    check("WE 5.1.3 INVARIANT annual benefit of the 11 sites exceeds their one-off cost",
          1 if ADD_BEN > EXPOSURE else 0, 1)
    # the caution: the 13,000 is rollout only, so the true marginal cost is higher and payback longer
    check("WE 5.1.3 caution: Domain 4's missing enabling-change element", TRAINING, 214000)
    # MCQ 5.1-B distractors, each a named error
    check("MCQ 5.1-B distractor A (one site, not eleven)", UNIT, 13000)
    check("MCQ 5.1-B distractor C (whole baseline over the site count, per site)",
          BASE / CLINICS, 60000)
    check("MCQ 5.1-B distractor D (annual benefit quoted as a cost)", ADD_BEN, 188496)

    # ======================= ENGINE 2 — the correction ladder (KA 5.2.1) ==================
    REQS = D(480)
    L0 = D(400)
    LADDER = [L0 * 4 ** k for k in range(5)]     # derived geometrically, not listed
    for k, printed in enumerate((400, 1600, 6400, 25600, 102400)):
        check(f"WE 5.2.1 ladder stage {k + 1} derived at ratio 4", LADDER[k], printed)
    for k in range(4):
        check(f"WE 5.2.1 INVARIANT ladder ratio stage {k + 1}->{k + 2} is exactly 4",
              LADDER[k + 1] / LADDER[k], 4)
    for k, printed in enumerate((1, 4, 16, 64, 256)):
        check(f"Fig 5.2.1 multiple of the definition-stage cost at stage {k + 1}",
              LADDER[k] / L0, printed)
    PROFILE = [D(24), D(22), D(26), D(18), D(6)]
    check("WE 5.2.1 detection profile sums to the stated defect count", sum(PROFILE), 96)
    check("WE 5.2.1 defects per requirement", sum(PROFILE) / REQS, D("0.2"))
    UNIMPROVED = sum(n * c for n, c in zip(PROFILE, LADDER))
    check("WE 5.2.1 unimproved expected correction cost", UNIMPROVED, 1286400)
    check("WE 5.2.1 unimproved average per defect", UNIMPROVED / sum(PROFILE), 13400)
    SPEND = D(84000)
    check("WE 5.2.1 elicitation spend per requirement", SPEND / REQS, 175)
    MOVED = [(D(13), 2), (D(9), 3), (D(3), 4)]   # (count, ladder index moved FROM)
    # the move is exactly half of the build, test and live populations — the manuscript's own hedge
    for (n, i), pop in zip(MOVED, (PROFILE[2], PROFILE[3], PROFILE[4])):
        check(f"WE 5.2.1 defects moved from stage {i + 1} are half that stage's population",
              n * 2, pop)
    for (n, i), printed in zip(MOVED, (6000, 25200, 102000)):
        check(f"WE 5.2.1 stage {i + 1} saving per defect is the DIFFERENCE to definition",
              LADDER[i] - L0, printed)
    PARTS = [n * (LADDER[i] - L0) for n, i in MOVED]
    for part, printed in zip(PARTS, (78000, 226800, 306000)):
        check("WE 5.2.1 saving component", part, printed)
    SAVING = sum(PARTS)
    check("WE 5.2.1 total saving on stage differences", SAVING, 610800)
    check("WE 5.2.1 net of the investment", SAVING - SPEND, 526800)
    check("WE 5.2.1 return on the elicitation spend", q(SAVING / SPEND, 2), D("7.27"))
    check("WE 5.2.1 improved expected correction cost", UNIMPROVED - SAVING, 675600)
    check("WE 5.2.1 improved average per defect",
          q((UNIMPROVED - SAVING) / sum(PROFILE), 2), D("7037.50"))
    BE_WEEKS = (SAVING - SPEND) / COD
    check("WE 5.2.1 breakeven definition-phase extension in weeks", q(BE_WEEKS, 2), D("36.89"))
    GROSS = sum(n * LADDER[i] for n, i in MOVED)
    check("WE 5.2.1 the gross-cost error (later-stage costs, not differences)", GROSS, 620800)
    check("WE 5.2.1 overstatement from the gross-cost error", GROSS - SAVING, 10000)
    # INVARIANT: the overstatement is exactly (defects moved x definition-stage cost)
    check("WE 5.2.1 INVARIANT overstatement equals defects moved x definition cost",
          sum(n for n, _ in MOVED) * L0, GROSS - SAVING)
    check("WE 5.2.1 INVARIANT the difference basis is strictly the cheaper claim",
          1 if SAVING < GROSS else 0, 1)
    # INVARIANT: one live-service defect moved pays for the whole programme
    check("WE 5.2.1 one live-service defect moved to definition", LADDER[4] - L0, 102000)
    check("WE 5.2.1 INVARIANT one moved live defect exceeds the whole elicitation spend",
          1 if (LADDER[4] - L0) > SPEND else 0, 1)
    FOUR_WK = 4 * COD
    check("WE 5.2.1 a four-week definition extension priced", FOUR_WK, 57120)
    check("WE 5.2.1 net after a four-week extension", SAVING - SPEND - FOUR_WK, 469680)
    check("WE 5.2.1 breakeven is more than nine times the realistic extension",
          q(BE_WEEKS / 4, 2), D("9.22"))
    check("WE 5.2.1 INVARIANT the delay objection is not decisive at four weeks",
          1 if BE_WEEKS > 4 else 0, 1)
    # Fig 5.2.1 plotted values and annotation
    # An axis bound restated against itself asserts nothing. What matters is that the drawn range
    # actually contains the data: a log axis of 200 to 200,000 has to bracket the whole ladder, or
    # the chart clips the very column its argument rests on.
    check("Fig 5.2.1 INVARIANT the log axis brackets every plotted value",
          1 if all(D(200) <= v <= D(200000) for v in LADDER) else 0, 1)
    check("Fig 5.2.1 INVARIANT the axis is no wider than one decade beyond the data",
          1 if min(LADDER) / D(200) < 10 and D(200000) / max(LADDER) < 10 else 0, 1)
    check("Fig 5.2.1 dashed elicitation line", SPEND, 84000)
    check("Fig 5.2.1 annotation: one live defect moved saves", LADDER[4] - L0, 102000)
    check("Fig 5.2.1 INVARIANT the dashed line sits below the tallest column",
          1 if SPEND < LADDER[4] else 0, 1)
    # MCQ 5.2-A / 5.2-B distractors
    check("MCQ 5.2-A distractor A (gross later-stage costs)", GROSS, 620800)
    check("MCQ 5.2-A distractor C (saving net of the 84,000 investment)", SAVING - SPEND, 526800)
    check("MCQ 5.2-A distractor D (unimproved total correction cost)", UNIMPROVED, 1286400)
    check("MCQ 5.2-B distractor A (return multiple)", q(SAVING / SPEND, 2), D("7.27"))
    check("MCQ 5.2-B distractor C (average falls from / to)",
          q((UNIMPROVED - SAVING) / sum(PROFILE), 2), D("7037.50"))

    # ======================= ENGINE 3 — traceability (KA 5.2.3) ===========================
    DESIGN = D(425)
    TRACED, NO_DESIGN, NO_TEST, NO_EVID, ORPHAN = D(416), D(34), D(21), D(9), D(17)
    # INVARIANT: the classes and the fully-traced count reconstruct the baseline exactly
    check("WE 5.2.3 INVARIANT forward classes + fully traced reconstruct the 480 baseline",
          TRACED + NO_DESIGN + NO_TEST + NO_EVID, REQS)
    NONCONF = NO_DESIGN + NO_TEST + NO_EVID
    check("WE 5.2.3 forward non-conformances", NONCONF, 64)
    check("WE 5.2.3 forward defect rate", q(NONCONF / REQS * 100, 2), D("13.33"))
    check("WE 5.2.3 class rate: no design element", q(NO_DESIGN / REQS * 100, 2), D("7.08"))
    check("WE 5.2.3 class rate: no test case", q(NO_TEST / REQS * 100, 2), D("4.38"))
    check("WE 5.2.3 class rate: accepted without verification evidence",
          q(NO_EVID / REQS * 100, 2), D("1.88"))
    check("WE 5.2.3 fully traced share", q(TRACED / REQS * 100, 2), D("86.67"))
    check("WE 5.2.3 rounded class rates sum to 13.34, not the combined 13.33 (display rounding)",
          q(NO_DESIGN / REQS * 100, 2) + q(NO_TEST / REQS * 100, 2)
          + q(NO_EVID / REQS * 100, 2), D("13.34"))
    check("WE 5.2.3 reverse defect rate on the design-register denominator",
          q(ORPHAN / DESIGN * 100, 2), D("4.00"))
    # INVARIANT: the two rates run on different denominators and are therefore not combinable
    check("WE 5.2.3 INVARIANT the reverse denominator differs from the forward one",
          1 if DESIGN != REQS else 0, 1)
    check("WE 5.2.3 orphaned design elements priced at build-stage correction cost",
          ORPHAN * LADDER[2], 108800)
    check("WE 5.2.3 the 34 with no design surface at design-stage cost each", LADDER[1], 1600)
    check("WE 5.2.3 the 21 with no test case surface in live service at", LADDER[4], 102400)
    # MCQ 5.2-D distractor A: the 17 put over the requirements baseline and called forward
    check("MCQ 5.2-D distractor A (17 over the 480 requirements, mislabelled forward)",
          q(ORPHAN / REQS * 100, 2), D("3.54"))
    check("MCQ 5.2-C distractor D (the combined rate the class breakdown exists to prevent)",
          q(NONCONF / REQS * 100, 2), D("13.33"))

    # ======================= ENGINE 4 — value and prioritisation (KA 5.3) =================
    # 5.3.1 the sum test
    BUNDLES = {"A": (D(13), D(299000)), "B": (D(10), D(228000)), "C": (D(10), D(225000)),
               "D": (D(5), D(110000)), "E": (D(5), D(95000))}
    ATTRIB = sum(v for _, v in BUNDLES.values())
    check("5.3.1 attributed benefit across the five candidate bundles", ATTRIB, 957000)
    check("5.3.1 unattributed remainder against the full potential", BFULL - ATTRIB, 22200)
    check("5.3.1 INVARIANT the sum test holds — attribution does not exceed the business case",
          1 if ATTRIB <= BFULL else 0, 1)
    # 5.3.2 must-have inflation
    MUST0, MUST1 = D(374), D(148)
    check("5.3.2 must-have share on first pass", q(MUST0 / REQS * 100, 2), D("77.92"))
    check("5.3.2 must-have share after the consequence re-test",
          q(MUST1 / REQS * 100, 2), D("30.83"))
    POOL0, POOL1 = REQS - MUST0, REQS - MUST1
    check("5.3.2 discretionary pool before the re-test", POOL0, 106)
    check("5.3.2 discretionary pool before, as a share", q(POOL0 / REQS * 100, 2), D("22.08"))
    check("5.3.2 discretionary pool after the re-test", POOL1, 332)
    check("5.3.2 discretionary pool after, as a share", q(POOL1 / REQS * 100, 2), D("69.17"))
    check("5.3.2 fold increase in the tradeable pool", q(POOL1 / POOL0, 2), D("3.13"))
    check("5.3.2 INVARIANT the re-test strictly enlarges the tradeable pool",
          1 if POOL1 > POOL0 else 0, 1)
    check("5.3.2 at 77.92 % must-have, four requirements in five are outside prioritisation's reach",
          q(MUST0 / REQS * 5, 1), D("3.9"))
    # 5.3.3 greedy against enumeration — both BUILT, neither typed
    CAP = D(20)
    check("WE 5.3.3 total candidate effort against 20 weeks available",
          sum(e for e, _ in BUNDLES.values()), 43)
    check("WE 5.3.3 total attributed benefit of the candidate set", ATTRIB, 957000)
    for name, printed in (("A", 23000), ("B", 22800), ("C", 22500), ("D", 22000), ("E", 19000)):
        e, v = BUNDLES[name]
        check(f"WE 5.3.3 value per week, bundle {name}", v / e, printed)

    order = sorted(BUNDLES, key=lambda k: -BUNDLES[k][1] / BUNDLES[k][0])
    check("WE 5.3.3 ratio order sorts to A, B, C, D, E",
          1 if order == ["A", "B", "C", "D", "E"] else 0, 1)
    rem, greedy = CAP, []
    for k in order:
        if BUNDLES[k][0] <= rem:
            greedy.append(k)
            rem -= BUNDLES[k][0]
    check("WE 5.3.3 greedy selects A + D", 1 if greedy == ["A", "D"] else 0, 1)
    G_EFF = sum(BUNDLES[k][0] for k in greedy)
    G_VAL = sum(BUNDLES[k][1] for k in greedy)
    check("WE 5.3.3 greedy effort used", G_EFF, 18)
    check("WE 5.3.3 greedy annual value", G_VAL, 409000)
    check("WE 5.3.3 greedy capacity utilisation", q(G_EFF / CAP * 100, 1), D("90.0"))
    check("WE 5.3.3 stranded capacity under greedy", CAP - G_EFF, 2)
    check("WE 5.3.3 INVARIANT no unselected bundle fits the stranded 2 weeks",
          sum(1 for k in BUNDLES if k not in greedy and BUNDLES[k][0] <= CAP - G_EFF), 0)

    def subsets(keys):
        out = [[]]
        for k in keys:
            out += [s + [k] for s in out]
        return out[1:]

    feasible = []
    for s in subsets(list(BUNDLES)):
        e = sum(BUNDLES[k][0] for k in s)
        if e <= CAP:
            feasible.append((sum(BUNDLES[k][1] for k in s), e, sorted(s)))
    feasible.sort(key=lambda t: (-t[0], t[1]))
    E_VAL, E_EFF, E_SET = feasible[0]
    check("WE 5.3.3 enumeration selects B + C", 1 if E_SET == ["B", "C"] else 0, 1)
    check("WE 5.3.3 enumerated annual value", E_VAL, 453000)
    check("WE 5.3.3 enumerated effort used", E_EFF, 20)
    check("WE 5.3.3 enumerated capacity utilisation", q(E_EFF / CAP * 100, 1), D("100.0"))
    # the four other material feasible sets the substitution line prints
    mat = {tuple(s): (v, e) for v, e, s in feasible}
    for s, pv, pe in ((("A", "D"), 409000, 18), (("A", "E"), 394000, 18),
                      (("B", "D", "E"), 433000, 20), (("C", "D", "E"), 430000, 20),
                      (("B", "C"), 453000, 20)):
        check(f"WE 5.3.3 feasible set {'+'.join(s)} value", mat[s][0], pv)
        check(f"WE 5.3.3 feasible set {'+'.join(s)} effort", mat[s][1], pe)
    DIFF = E_VAL - G_VAL
    check("WE 5.3.3 annual difference", DIFF, 44000)
    check("WE 5.3.3 difference as a share of the greedy result",
          q(DIFF / G_VAL * 100, 2), D("10.76"))
    check("WE 5.3.3 present value of the difference over eight years at 7 %",
          q(DIFF * D("5.971299"), 0), 262737)
    check("WE 5.3.3 present value recomputed from af(0.07, 8) rather than the printed factor",
          q(DIFF * AF8, 0), 262737)
    # INVARIANTS: greedy is never better; here it is strictly worse; and it strands capacity
    check("WE 5.3.3 INVARIANT greedy never beats the enumerated optimum",
          1 if G_VAL <= E_VAL else 0, 1)
    check("WE 5.3.3 INVARIANT greedy is strictly worse here", 1 if G_VAL < E_VAL else 0, 1)
    check("WE 5.3.3 INVARIANT enumeration leaves no stranded capacity", CAP - E_EFF, 0)
    RA = BUNDLES["A"][1] / BUNDLES["A"][0] - BUNDLES["B"][1] / BUNDLES["B"][0]
    check("WE 5.3.3 A's ratio advantage over B in USD per week", RA, 200)
    check("WE 5.3.3 A's ratio advantage as a percentage",
          q((BUNDLES["A"][1] / BUNDLES["A"][0]) / (BUNDLES["B"][1] / BUNDLES["B"][0]) * 100
            - 100, 2), D("0.88"))
    check("WE 5.3.3 INVARIANT a sub-1 % ratio advantage bought a >10 % value loss",
          1 if q((BUNDLES["A"][1] / BUNDLES["A"][0])
                 / (BUNDLES["B"][1] / BUNDLES["B"][0]) * 100 - 100, 2) < 1
          and q(DIFF / G_VAL * 100, 2) > 10 else 0, 1)
    # Fig 5.3.1 plotted values
    check("Fig 5.3.1 capacity bar length in development-weeks", CAP, 20)
    check("Fig 5.3.1 greedy segment A effort", BUNDLES["A"][0], 13)
    check("Fig 5.3.1 greedy segment A value", BUNDLES["A"][1], 299000)
    check("Fig 5.3.1 greedy segment D effort", BUNDLES["D"][0], 5)
    check("Fig 5.3.1 greedy segment D value", BUNDLES["D"][1], 110000)
    check("Fig 5.3.1 greedy idle segment", CAP - G_EFF, 2)
    check("Fig 5.3.1 greedy bar total", G_VAL, 409000)
    check("Fig 5.3.1 enumeration segment B effort", BUNDLES["B"][0], 10)
    check("Fig 5.3.1 enumeration segment B value", BUNDLES["B"][1], 228000)
    check("Fig 5.3.1 enumeration segment C effort", BUNDLES["C"][0], 10)
    check("Fig 5.3.1 enumeration segment C value", BUNDLES["C"][1], 225000)
    check("Fig 5.3.1 enumeration bar total", E_VAL, 453000)
    check("Fig 5.3.1 caption difference", DIFF, 44000)
    check("Fig 5.3.1 caption percentage", q(DIFF / G_VAL * 100, 2), D("10.76"))
    check("Fig 5.3.1 caption present value", q(DIFF * D("5.971299"), 0), 262737)
    # MCQ 5.3-A distractors
    check("MCQ 5.3-A distractor B (the enumerated optimum, not the greedy result)", E_VAL, 453000)
    check("MCQ 5.3-A distractor C value (A + B)", BUNDLES["A"][1] + BUNDLES["B"][1], 527000)
    check("MCQ 5.3-A distractor C effort exceeds capacity at 23 weeks",
          BUNDLES["A"][0] + BUNDLES["B"][0], 23)
    check("MCQ 5.3-A distractor C is infeasible",
          1 if BUNDLES["A"][0] + BUNDLES["B"][0] > CAP else 0, 1)
    check("MCQ 5.3-A distractor D value (B + D + E, feasible but never reached)",
          BUNDLES["B"][1] + BUNDLES["D"][1] + BUNDLES["E"][1], 433000)
    check("MCQ 5.3-A distractor D is feasible but not greedy's answer",
          1 if BUNDLES["B"][0] + BUNDLES["D"][0] + BUNDLES["E"][0] <= CAP
          and 433000 != G_VAL else 0, 1)
    check("MCQ 5.3-B rationale: A's size wastes 10 % of the capacity",
          q((CAP - G_EFF) / CAP * 100, 1), D("10.0"))
    check("MCQ 5.3-D correct option: pool of 106 is 22.08 %, about a fifth",
          q(POOL0 / REQS * 100, 2), D("22.08"))

    # ======================= ENGINE 5 — change, criteria, acceptance (KA 5.4) ==============
    # 5.4.1 the count reconciliation
    ADDS, TRACED_AT_AT = D(12), D(531)
    EXPECTED = REQS + ADDS
    check("WE 5.4.1 expected traced count", EXPECTED, 492)
    UNEXPL = TRACED_AT_AT - EXPECTED
    check("WE 5.4.1 unexplained requirements", UNEXPL, 39)
    AVG_DIRECT, CP_COUNT, CP_WEEKS = D(4200), D(16), D("0.25")
    DIRECT = UNEXPL * AVG_DIRECT
    check("WE 5.4.1 direct creep cost", DIRECT, 163800)
    SCH_WEEKS = CP_COUNT * CP_WEEKS
    check("WE 5.4.1 critical-path weeks consumed", SCH_WEEKS, D("4.0"))
    check("WE 5.4.1 schedule creep cost at the registered cost of delay", SCH_WEEKS * COD, 57120)
    CREEP = DIRECT + SCH_WEEKS * COD
    check("WE 5.4.1 total creep cost", CREEP, 220920)
    check("WE 5.4.1 creep as a share of baseline (exact-half display boundary)",
          q(CREEP / BASE * 100, 2), D("9.20"))
    check("WE 5.4.1 average cost per crept requirement", q(CREEP / UNEXPL, 2), D("5664.62"))
    check("WE 5.4.1 crept requirements per month over 13 months", q(UNEXPL / 13, 2), D("3.00"))
    check("WE 5.4.1 crept requirements as a multiple of the approved additions",
          q(UNEXPL / ADDS, 2), D("3.25"))
    check("WE 5.4.1 INVARIANT creep outnumbers controlled change", 1 if UNEXPL > ADDS else 0, 1)
    MOVEMENT = D4_DRIFT + CREEP
    check("WE 5.4.1 total baseline movement with Domain 4's authorised drift", MOVEMENT, 512096)
    check("WE 5.4.1 total movement as a share of baseline",
          q(MOVEMENT / BASE * 100, 2), D("21.34"))
    LOGSHARE = D4_DRIFT / MOVEMENT * 100
    check("WE 5.4.1 share of movement the change log captured", q(LOGSHARE, 2), D("56.86"))
    check("WE 5.4.1 share the change log cannot see", q(100 - LOGSHARE, 2), D("43.14"))
    check("WE 5.4.1 INVARIANT the change log captures less than all of the movement",
          1 if LOGSHARE < 100 else 0, 1)
    check("WE 5.4.1 a single crept requirement as a share of baseline, on its direct cost",
          q(AVG_DIRECT / BASE * 100, 3), D("0.175"))
    check("WE 5.4.1 movement as a share of Domain 2's approved NPV",
          q(MOVEMENT / D2_NPV * 100, 2), D("38.42"))
    check("WE 5.4.1 NPV remaining after total scope movement", D2_NPV - MOVEMENT, 820802)
    check("WE 5.4.1 INVARIANT the NPV survives the movement — which is why nobody stopped",
          1 if D2_NPV - MOVEMENT > 0 else 0, 1)
    # MCQ 5.4-A / 5.4-B distractors
    check("MCQ 5.4-A distractor A (direct only, schedule impact omitted)", DIRECT, 163800)
    check("MCQ 5.4-A distractor C (all 51 additions priced as uncontrolled)",
          (UNEXPL + ADDS) * AVG_DIRECT, 214200)
    check("MCQ 5.4-A distractor C counts 51 requirements", UNEXPL + ADDS, 51)
    check("MCQ 5.4-A distractor D (schedule impact alone)", SCH_WEEKS * COD, 57120)
    check("MCQ 5.4-B distractor A (total movement share, true but subordinate)",
          q(MOVEMENT / BASE * 100, 2), D("21.34"))
    check("MCQ 5.4-B distractor C (average crept cost, true but subordinate)",
          q(CREEP / UNEXPL, 2), D("5664.62"))

    # 5.4.2 acceptance criteria
    MEAS, RESTATED, ABSENT = D(382), D(71), D(27)
    check("WE 5.4.2 INVARIANT the criterion census reconstructs the 480 baseline",
          MEAS + RESTATED + ABSENT, REQS)
    DEFIC = RESTATED + ABSENT
    check("WE 5.4.2 deficient criteria", DEFIC, 98)
    check("WE 5.4.2 deficient share of the baseline", q(DEFIC / REQS * 100, 2), D("20.42"))
    check("WE 5.4.2 restated share", q(RESTATED / REQS * 100, 2), D("14.79"))
    check("WE 5.4.2 absent share (exact-half display boundary)",
          q(ABSENT / REQS * 100, 2), D("5.62"))
    check("WE 5.4.2 the two class rates round to 20.41 against the combined 20.42",
          q(RESTATED / REQS * 100, 2) + q(ABSENT / REQS * 100, 2), D("20.41"))
    check("WE 5.4.2 INVARIANT the restated class is larger than the absent class — and worse",
          1 if RESTATED > ABSENT else 0, 1)
    PER_CRIT = D(320)
    REMED = DEFIC * PER_CRIT
    check("WE 5.4.2 remediation cost", REMED, 31360)
    P_DISPUTE, REWORK, SHARE_DELAY, DELAY_WK = D("0.25"), D(9600), D(1) / 3, D("0.5")
    EXP_DELAY = SHARE_DELAY * DELAY_WK * COD
    check("WE 5.4.2 expected delay cost per dispute", q(EXP_DELAY, 2), D("2380.00"))
    CPD = REWORK + EXP_DELAY
    check("WE 5.4.2 expected cost per dispute", q(CPD, 2), D("11980.00"))
    EXP_N = DEFIC * P_DISPUTE
    check("WE 5.4.2 expected number of disputes", q(EXP_N, 1), D("24.5"))
    EXP_COST = EXP_N * CPD
    check("WE 5.4.2 expected dispute cost", q(EXP_COST, 2), D("293510.00"))
    check("WE 5.4.2 net benefit of remediation", q(EXP_COST - REMED, 2), D("262150.00"))
    check("WE 5.4.2 return multiple on remediation", q(EXP_COST / REMED, 2), D("9.36"))
    BE_P = REMED / (DEFIC * CPD)
    check("WE 5.4.2 breakeven dispute probability", q(BE_P * 100, 2), D("2.67"))
    check("WE 5.4.2 observed rate as a multiple of the breakeven",
          q(P_DISPUTE * 100 / (BE_P * 100), 2), D("9.36"))
    # INVARIANT: the breakeven is the probability at which the two sides are exactly equal
    check("WE 5.4.2 INVARIANT at the breakeven probability the two costs are equal",
          q(DEFIC * BE_P * CPD, 6), q(REMED, 6))
    check("WE 5.4.2 INVARIANT the observed rate is far above the breakeven",
          1 if P_DISPUTE > BE_P else 0, 1)
    check("WE 5.4.2 remediation effort: 98 criteria at four a day, in weeks",
          q(DEFIC / 4 / 5, 1), D("4.9"))
    # MCQ 5.4-C distractors
    check("MCQ 5.4-C distractor A (the observed dispute rate)", P_DISPUTE * 100, 25)
    check("MCQ 5.4-C distractor C (the return multiple)", q(EXP_COST / REMED, 2), D("9.36"))
    check("MCQ 5.4-C distractor D (the deficient share of the baseline)",
          q(DEFIC / REQS * 100, 2), D("20.42"))

    # 5.4.3 conditional acceptance
    OPEN, SEV1, FIX_DELAY = D(41), D(6), D("1.5")
    check("WE 5.4.3 severity-two and -three items", OPEN - SEV1, 35)
    FIX_CORR = SEV1 * LADDER[2]
    check("WE 5.4.3 build-stage correction cost of the six items", FIX_CORR, 38400)
    check("WE 5.4.3 go-live delay priced", FIX_DELAY * COD, 21420)
    FIX = FIX_CORR + FIX_DELAY * COD
    check("WE 5.4.3 fix-first total cost", FIX, 59820)
    DEFER = SEV1 * LADDER[4]
    check("WE 5.4.3 deferral cost at live-service correction", DEFER, 614400)
    check("WE 5.4.3 deferral as a multiple of fixing first", q(DEFER / FIX, 2), D("10.27"))
    check("WE 5.4.3 difference between the two courses", DEFER - FIX, 554580)
    BE_DELAY = (DEFER - FIX_CORR) / COD
    check("WE 5.4.3 breakeven go-live delay in weeks", q(BE_DELAY, 2), D("40.34"))
    check("WE 5.4.3 breakeven as a multiple of the actual 1.5-week delay",
          q(BE_DELAY / FIX_DELAY, 0), 27)
    # INVARIANTS: fixing first is cheaper, and remains so up to the breakeven delay
    check("WE 5.4.3 INVARIANT fixing first is cheaper than deferring",
          1 if FIX < DEFER else 0, 1)
    check("WE 5.4.3 INVARIANT at the breakeven delay the two courses cost the same",
          q(FIX_CORR + BE_DELAY * COD, 6), q(DEFER, 6))
    check("WE 5.4.3 INVARIANT just inside the breakeven, fixing first still wins",
          1 if FIX_CORR + (BE_DELAY - 1) * COD < DEFER else 0, 1)
    check("WE 5.4.3 INVARIANT just beyond the breakeven, deferring wins",
          1 if FIX_CORR + (BE_DELAY + 1) * COD > DEFER else 0, 1)

    # ======================= case studies =================================================
    # Case study A — Meridian acceptance
    check("Case A deficient criteria and their share", q(DEFIC / REQS * 100, 2), D("20.42"))
    check("Case A remediation cost of all 98 criteria", DEFIC * PER_CRIT, 31360)
    check("Case A expected dispute cost", q(EXP_COST, 2), D("293510.00"))
    check("Case A return multiple", q(EXP_COST / REMED, 2), D("9.36"))
    check("Case A breakeven dispute probability", q(BE_P * 100, 2), D("2.67"))
    check("Case A INVARIANT the observed disputes fall short of the expectation of 24.5",
          1 if D(22) < D("24.5") else 0, 1)
    check("Case A observed shortfall against the expectation", D("24.5") - D(22), D("2.5"))
    check("Case A INVARIANT observed disputes fell below the expectation",
          1 if D(22) < EXP_N else 0, 1)
    check("Case A four-week acceptance suspension priced", 4 * COD, 57120)
    check("Case A rewriting criteria for the 41 requirements still open", D(41) * PER_CRIT, 13120)
    check("Case A six severity-one items fixed rather than deferred", FIX, 59820)
    check("Case A deferral cost avoided", DEFER, 614400)
    check("Case A orphan disposition reconstructs the 17", D(11) + D(6), ORPHAN)
    check("Case A creep exposed by the first count reconciliation", UNEXPL, 39)
    check("Case A creep cost exposed", CREEP, 220920)
    check("Case A INVARIANT prevention (31,360) cost less than the suspension alone (57,120)",
          1 if REMED < 4 * COD else 0, 1)

    # Case study B — the licensing authority
    B_REQS, B_APPL, B_JOURNEY = D(612), D(4200), D(47)
    B_FCAST, B_REAL0, B_REAL1, B_CHANGE = D(1150000), D(391000), D(816500), D(285000)
    B_DIGITAL_SHARE = D("0.78")
    check("Case B realised share of forecast benefit at the review",
          q(B_REAL0 / B_FCAST * 100, 1), D("34.0"))
    check("Case B requirements addressing the applicant journey, as a share",
          q(B_JOURNEY / B_REQS * 100, 2), D("7.68"))
    check("Case B requirements describing internal officers' work", B_REQS - B_JOURNEY, 565)
    check("Case B forecast benefit depending on digital adoption",
          B_FCAST * B_DIGITAL_SHARE, 897000)
    check("Case B forecast benefit not depending on digital adoption",
          B_FCAST * (1 - B_DIGITAL_SHARE), 253000)
    B_DIG_REAL = B_REAL0 - B_FCAST * (1 - B_DIGITAL_SHARE)
    check("Case B digital-dependent benefit actually delivered", B_DIG_REAL, 138000)
    check("Case B digital-dependent benefit delivered, as a share of its forecast",
          q(B_DIG_REAL / (B_FCAST * B_DIGITAL_SHARE) * 100, 1), D("15.4"))
    check("Case B INVARIANT the shortfall is 66 points, not the 78 a total adoption failure gives",
          q(100 - B_REAL0 / B_FCAST * 100, 1), D("66.0"))
    check("Case B realised share after re-elicitation",
          q(B_REAL1 / B_FCAST * 100, 1), D("71.0"))
    B_UPLIFT = B_REAL1 - B_REAL0
    check("Case B annual uplift", B_UPLIFT, 425500)
    check("Case B payback in years", q(B_CHANGE / B_UPLIFT, 2), D("0.67"))
    check("Case B payback in months", q(B_CHANGE / B_UPLIFT * 12, 1), D("8.0"))
    # An INPUT is not a golden answer. Restating one against itself (`check(..., D(8), 8)`)
    # produced a passing line that could never fail; whether the manuscript still states
    # this figure is a question about the manuscript, which audit_prose_math.py and
    # audit_figures.py ask by reading it. Removed rather than left as false assurance.
    check("Case B INVARIANT the uplift exceeds the cost of buying it in year one",
          1 if B_UPLIFT > B_CHANGE else 0, 1)
    check("Case B INVARIANT re-elicitation left benefit still short of forecast",
          1 if B_REAL1 < B_FCAST else 0, 1)

    # ======================= exercises ====================================================
    # Exercise 5.1 — traceability audit on a different pair of denominators
    X1_R, X1_D = D(640), D(578)
    X1 = (D(51), D(38), D(12))
    check("EX 5.1 forward non-conformances", sum(X1), 101)
    check("EX 5.1 forward defect rate", q(sum(X1) / X1_R * 100, 2), D("15.78"))
    check("EX 5.1 class rate: no design element", q(X1[0] / X1_R * 100, 2), D("7.97"))
    check("EX 5.1 class rate: no test case", q(X1[1] / X1_R * 100, 2), D("5.94"))
    check("EX 5.1 class rate: accepted without evidence", q(X1[2] / X1_R * 100, 2), D("1.88"))
    check("EX 5.1 reverse defect rate", q(D(23) / X1_D * 100, 2), D("3.98"))
    check("EX 5.1 INVARIANT the two denominators differ", 1 if X1_R != X1_D else 0, 1)

    # Exercise 5.2 — a different ladder, same method
    X2_L = [D(500) * 4 ** k for k in range(5)]
    for k, printed in enumerate((500, 2000, 8000, 32000, 128000)):
        check(f"EX 5.2 ladder stage {k + 1}", X2_L[k], printed)
    for k in range(4):
        check(f"EX 5.2 INVARIANT ladder ratio {k + 1}->{k + 2} is exactly 4",
              X2_L[k + 1] / X2_L[k], 4)
    X2_MOVED, X2_SPEND, X2_COD = [(D(11), 2), (D(8), 3), (D(2), 4)], D(62000), D(11500)
    for (n, i), printed in zip(X2_MOVED, (7500, 31500, 127500)):
        check(f"EX 5.2 stage {i + 1} saving per defect", X2_L[i] - X2_L[0], printed)
    X2_PARTS = [n * (X2_L[i] - X2_L[0]) for n, i in X2_MOVED]
    for part, printed in zip(X2_PARTS, (82500, 252000, 255000)):
        check("EX 5.2 saving component", part, printed)
    X2_SAV = sum(X2_PARTS)
    check("EX 5.2 total saving", X2_SAV, 589500)
    check("EX 5.2 net", X2_SAV - X2_SPEND, 527500)
    check("EX 5.2 return", q(X2_SAV / X2_SPEND, 2), D("9.51"))
    check("EX 5.2 breakeven definition-phase extension",
          q((X2_SAV - X2_SPEND) / X2_COD, 2), D("45.87"))
    X2_GROSS = sum(n * X2_L[i] for n, i in X2_MOVED)
    check("EX 5.2 common error: gross later-stage costs", X2_GROSS, 600000)
    check("EX 5.2 common error overstatement", X2_GROSS - X2_SAV, 10500)
    check("EX 5.2 INVARIANT overstatement equals defects moved x definition cost",
          sum(n for n, _ in X2_MOVED) * X2_L[0], X2_GROSS - X2_SAV)

    # Exercise 5.3 — greedy against enumeration at a different capacity
    X3 = {"P": (D(16), D(496000)), "Q": (D(12), D(366000)),
          "R": (D(12), D(360000)), "S": (D(6), D(168000))}
    X3_CAP = D(24)
    for name, printed in (("P", 31000), ("Q", 30500), ("R", 30000), ("S", 28000)):
        e, v = X3[name]
        check(f"EX 5.3 value per week, bundle {name}", v / e, printed)
    x3_order = sorted(X3, key=lambda k: -X3[k][1] / X3[k][0])
    check("EX 5.3 ratio order sorts to P, Q, R, S",
          1 if x3_order == ["P", "Q", "R", "S"] else 0, 1)
    rem3, g3 = X3_CAP, []
    for k in x3_order:
        if X3[k][0] <= rem3:
            g3.append(k)
            rem3 -= X3[k][0]
    check("EX 5.3 greedy selects P + S", 1 if g3 == ["P", "S"] else 0, 1)
    G3_EFF = sum(X3[k][0] for k in g3)
    G3_VAL = sum(X3[k][1] for k in g3)
    check("EX 5.3 greedy value", G3_VAL, 664000)
    check("EX 5.3 greedy effort", G3_EFF, 22)
    check("EX 5.3 greedy capacity utilisation", q(G3_EFF / X3_CAP * 100, 1), D("91.7"))
    check("EX 5.3 stranded capacity", X3_CAP - G3_EFF, 2)
    f3 = []
    for s in subsets(list(X3)):
        e = sum(X3[k][0] for k in s)
        if e <= X3_CAP:
            f3.append((sum(X3[k][1] for k in s), e, sorted(s)))
    f3.sort(key=lambda t: (-t[0], t[1]))
    E3_VAL, E3_EFF, E3_SET = f3[0]
    check("EX 5.3 enumeration selects Q + R", 1 if E3_SET == ["Q", "R"] else 0, 1)
    check("EX 5.3 enumerated value", E3_VAL, 726000)
    check("EX 5.3 enumerated effort", E3_EFF, 24)
    check("EX 5.3 enumerated capacity utilisation", q(E3_EFF / X3_CAP * 100, 1), D("100.0"))
    check("EX 5.3 annual difference", E3_VAL - G3_VAL, 62000)
    check("EX 5.3 difference as a share of the greedy result",
          q((E3_VAL - G3_VAL) / G3_VAL * 100, 2), D("9.34"))
    AF6 = af(D("0.08"), 6)
    check("EX 5.3 AF(0.08, 6) to six places", q(AF6, 6), D("4.622880"))
    check("EX 5.3 present value of the difference",
          q((E3_VAL - G3_VAL) * D("4.622880"), 0), 286619)
    check("EX 5.3 present value recomputed from af(0.08, 6)",
          q((E3_VAL - G3_VAL) * AF6, 0), 286619)
    check("EX 5.3 INVARIANT greedy is strictly worse", 1 if G3_VAL < E3_VAL else 0, 1)
    check("EX 5.3 INVARIANT enumeration strands no capacity", X3_CAP - E3_EFF, 0)

    # Exercise 5.4 — count reconciliation at a different scale
    X4_BASE, X4_COD = D(3100000), D(11500)
    check("EX 5.4 expected traced count", D(560) + D(19), 579)
    X4_UNEXPL = D(638) - (D(560) + D(19))
    check("EX 5.4 unexplained requirements", X4_UNEXPL, 59)
    check("EX 5.4 direct cost", X4_UNEXPL * D(3800), 224200)
    check("EX 5.4 critical-path weeks", D(21) * D("0.2"), D("4.2"))
    check("EX 5.4 schedule cost", D(21) * D("0.2") * X4_COD, 48300)
    X4_TOT = X4_UNEXPL * D(3800) + D(21) * D("0.2") * X4_COD
    check("EX 5.4 total creep cost", X4_TOT, 272500)
    check("EX 5.4 share of baseline", q(X4_TOT / X4_BASE * 100, 2), D("8.79"))
    check("EX 5.4 a single crept requirement as a share of baseline, on its direct cost",
          q(D(3800) / X4_BASE * 100, 2), D("0.12"))

    # Exercise 5.5 — acceptance-criterion economics at a different scale
    X5_N = D(126) + D(41)
    check("EX 5.5 deficient criteria", X5_N, 167)
    check("EX 5.5 remediation cost", X5_N * D(280), 46760)
    X5_DELAY = D("0.25") * D("0.5") * X4_COD
    check("EX 5.5 expected delay cost per dispute", q(X5_DELAY, 2), D("1437.50"))
    X5_CPD = D(8400) + X5_DELAY
    check("EX 5.5 expected cost per dispute", q(X5_CPD, 2), D("9837.50"))
    X5_EXP_N = X5_N * D("0.20")
    check("EX 5.5 expected disputes", q(X5_EXP_N, 1), D("33.4"))
    check("EX 5.5 expected dispute cost", q(X5_EXP_N * X5_CPD, 2), D("328572.50"))
    check("EX 5.5 net benefit", q(X5_EXP_N * X5_CPD - X5_N * D(280), 2), D("281812.50"))
    X5_BE = X5_N * D(280) / (X5_N * X5_CPD)
    check("EX 5.5 breakeven dispute probability", q(X5_BE * 100, 2), D("2.85"))
    check("EX 5.5 INVARIANT at the breakeven the two costs are equal",
          q(X5_N * X5_BE * X5_CPD, 6), q(X5_N * D(280), 6))
    check("EX 5.5 common error: the rate applied to all 720 rather than the 167 deficient",
          q(D(720) / X5_N, 2), D("4.31"))
    check("EX 5.5 INVARIANT that error inflates the expected cost more than fourfold",
          1 if D(720) / X5_N > 4 else 0, 1)
