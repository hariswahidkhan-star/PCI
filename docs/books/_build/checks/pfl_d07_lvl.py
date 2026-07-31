"""PFL-AI Domain 7 — golden checks for MCQ 7.1-F, 7.1-G, 7.2-F, 7.3-G, 7.4-G and 7.4-H.

Scope: the Evaluation and Comprehension items added to KA 7.1–7.4. Every number printed in those
stems, options and rationales is recomputed here from first principles; master-thread values are read
from ctx, never re-typed. `pfl_d07.py` owns the domain's worked examples and the items that preceded
these, so the two modules never edit one file.

Engines used:

  1. The CFADS bridge, rebuilt from primitives rather than trusting the reduced form:
     EBITDA = revenue - 3,600,000 - 0.0375 x volume; cash tax = 20 % of (EBITDA - 2,400,000 -
     2,520,000); working capital = 5 % of revenue.
  2. The availability line (7.1.3) — CFADS falls with the shortfall s at a slope of
     9,000,000 x multiplier - 720,000, so the covenant breakpoint is 0.95 - headroom / slope; the
     paraphrase error is the ratio of the two slopes.
  3. Generalised demand tolerance (7.3.2) — with wholly fixed cash costs the CFADS elasticity to
     volume is revenue / CFADS, so the tolerance is (1 - covenant/base) / elasticity.
  4. Credit (7.4.1, 7.4.2) — cumulative PD = 1 - (1-PD)^n; EL = cumulative PD x EAD x LGD; the
     four-payer loss distribution is binomial, so P(losing >= half) = P(at least two defaults) and
     P(losing all) = P(four defaults).
  5. The KA 7.4.3 stress matrix, recounted. The verifier found that 7.4.3's interpretation and two
     items (7.4-D, 7.4-F) miscounted a matrix whose sixteen printed cells are all correct: six cells
     clear 1.20x and four fall below 1.00x, not three and five. The manuscript is corrected and the
     counts are pinned here, because a count stated in prose is exactly the kind of number no
     arithmetic check was watching. The worked example's reduced form is first PROVED identical to
     the CFADS bridge above, so the cells are not taken on trust either.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))
    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    DS = ctx["KESTREL_INSTALMENT"]             # 5,009,635.23
    AF6_12 = af(ctx["KESTREL_RATE"], ctx["KESTREL_TENOR"])
    R0, V0 = D(12000000), D(24000000)
    P_UNIT, VC, FC = D("0.50"), D("0.0375"), D(3600000)
    DEP, INT, TAXR, WCR = D(2400000), D(2520000), D("0.20"), D("0.05")
    COV, CHARGE, GUAR = D("1.20"), D(12000000), D("0.95")

    def cfads(rev, vol):
        eb = rev - (FC + VC * vol)
        taxable = eb - DEP - INT
        tax = taxable * TAXR if taxable > 0 else D(0)
        return eb - tax - WCR * rev

    check("D7-lvl bridge reproduces the master thread", cfads(R0, V0), CF)
    head = CF - DS * COV
    check("D7-lvl annual covenant headroom", q(head, 0), 372438)

    # ================= MCQ 7.1-F — the paraphrased deduction clause =================
    # The slope is DERIVED from the bridge at two availability points, then cross-checked against
    # the manuscript's closed form, so neither is taken on trust.
    def cfads_at(s, mult):
        rev = CHARGE - CHARGE * mult * s
        vol = V0 * (1 - s)
        return cfads(rev, vol)

    for mult, want in ((D("1.5"), 12780000), (D("1.0"), 8280000)):
        derived = (cfads_at(D(0), mult) - cfads_at(D("0.01"), mult)) * 100
        check(f"MCQ 7.1-F slope of CFADS against availability at multiplier {mult}",
              derived, want, D("0.5"))
        check(f"MCQ 7.1-F closed form 9,000,000 x {mult} - 720,000 agrees",
              D(9000000) * mult - D(720000), want)
    s15 = head / D(12780000)
    s10 = head / D(8280000)
    check("MCQ 7.1-F covenant breakpoint at the 1.5x multiplier %",
          q((GUAR - s15) * 100, 3), D("92.086"))
    check("MCQ 7.1-F covenant breakpoint at a 1.0x multiplier %",
          q((GUAR - s10) * 100, 3), D("90.502"))
    check("MCQ 7.1-F operating tolerance the multiplier costs, in points",
          q((s10 - s15) * 100, 3), D("1.584"))
    check("MCQ 7.1-F CFADS lost per point of availability at 1.5x", D(12780000) / 100, 127800)
    check("MCQ 7.1-F CFADS lost per point of availability at 1.0x", D(8280000) / 100, 82800)
    check("MCQ 7.1-F the paraphrase understates the slope by %",
          q((1 - D(8280000) / D(12780000)) * 100, 2), D("35.21"))
    # INVARIANT: the paraphrase moves the breakpoint AWAY from the guarantee, i.e. flatters
    check("MCQ 7.1-F invariant: the paraphrase reports more tolerance than the clause gives",
          1 if s10 > s15 else 0, 1)

    # ================= MCQ 7.2-F — the bank case as a negotiating position ============
    check("MCQ 7.2-F the case difference in NPV (Domain 6's escalation case)",
          D(19875251) - D(2767684), 17107567)

    # ================= MCQ 7.3-G — cost structure behind an identical ratio ============
    BASE, TARGET = D("1.30"), COV
    fall = 1 - TARGET / BASE
    check("MCQ 7.3-G CFADS fall from a 1.30x base to a 1.20x covenant %",
          q(fall * 100, 4), D("7.6923"))
    for share, el, tol in ((D("0.80"), D("1.2500"), D("6.15")),
                           (D("0.40"), D("2.5000"), D("3.08"))):
        check(f"MCQ 7.3-G elasticity where CFADS is {share} of revenue",
              q(1 / share, 4), el)
        check(f"MCQ 7.3-G demand tolerance where CFADS is {share} of revenue %",
              q(fall / (1 / share) * 100, 2), tol)
    check("MCQ 7.3-G invariant: tolerance is independent of scale, so the ratio hides it",
          q(fall / (1 / D("0.80")) * 100, 2) / q(fall / (1 / D("0.40")) * 100, 2),
          2, D("0.005"))

    # ================= MCQ 7.4-G — concentration against expected loss ================
    PD, LGD, N = D("0.006"), D("0.45"), 12
    cum = 1 - (1 - PD) ** N
    ead = CF * AF6_12
    el = cum * ead * LGD
    check("MCQ 7.4-G twelve-year cumulative PD %", q(cum * 100, 4), D("6.9671"))
    check("MCQ 7.4-G exposure at default", q(ead, 0), 53522460)
    check("MCQ 7.4-G expected loss", q(el, 0), 1678031)
    p_half = 1 - ((1 - cum) ** 4 + 4 * cum * (1 - cum) ** 3)
    p_all = cum ** 4
    check("MCQ 7.4-G P(losing >= half the revenue), four payers %",
          q(p_half * 100, 4), D("2.6489"))
    check("MCQ 7.4-G P(losing all the revenue), four payers %",
          q(p_all * 100, 4), D("0.0024"))
    el_four = sum(cum * (ead / 4) * LGD for _ in range(4))
    check("MCQ 7.4-G invariant: expected loss is blind to concentration", el_four, el)
    check("MCQ 7.4-G invariant: the loss distribution is not",
          1 if p_half < cum and p_all < cum else 0, 1)

    # ================= MCQ 7.4-H — what LGD embeds =================
    check("MCQ 7.4-H LGD is the complement of the assumed recovery", 1 - D("0.55"), LGD)

    # ======== KA 7.4.3 stress matrix — the counts 7.4.3, 7.4-D and 7.4-F now print ==========
    def cell(pf, vf):
        vol, rev = V0 * vf, P_UNIT * pf * V0 * vf
        return (D("0.75") * rev - D("0.03") * vol - D(1896000)) / DS

    # The reduced form is the bridge with symmetric tax: 0.8(R - FC - VC.V) + 0.2 x 4,920,000 - 0.05R
    # collapses to 0.75R - 0.03V - 1,896,000. Proved at three cells with positive taxable profit.
    for pf, vf in ((D("1.05"), D("0.80")), (D("1.00"), D("1.00")), (D("0.90"), D("1.10"))):
        vol, rev = V0 * vf, P_UNIT * pf * V0 * vf
        check(f"KA 7.4.3 reduced form equals the CFADS bridge at tariff {pf} despatch {vf}",
              D("0.75") * rev - D("0.03") * vol - D(1896000), cfads(rev, vol), D("0.0000005"))

    PRINTED = {
        (D("1.05"), D("0.80")): D("1.0156"), (D("1.05"), D("0.90")): D("1.1899"),
        (D("1.05"), D("1.00")): D("1.3642"), (D("1.05"), D("1.10")): D("1.5384"),
        (D("1.00"), D("0.80")): D("0.9438"), (D("1.00"), D("0.90")): D("1.1091"),
        (D("1.00"), D("1.00")): D("1.2743"), (D("1.00"), D("1.10")): D("1.4396"),
        (D("0.95"), D("0.80")): D("0.8719"), (D("0.95"), D("0.90")): D("1.0282"),
        (D("0.95"), D("1.00")): D("1.1845"), (D("0.95"), D("1.10")): D("1.3408"),
        (D("0.90"), D("0.80")): D("0.8001"), (D("0.90"), D("0.90")): D("0.9474"),
        (D("0.90"), D("1.00")): D("1.0947"), (D("0.90"), D("1.10")): D("1.2420"),
    }
    got = {k: q(cell(*k), 4) for k in PRINTED}
    check("KA 7.4.3 all sixteen printed cells reproduce", 1 if got == PRINTED else 0, 1)
    clear = sum(1 for v in got.values() if v >= COV)
    below = sum(1 for v in got.values() if v < 1)
    check("KA 7.4.3 cells clearing the 1.20x covenant (7.4.3, 7.4-D, 7.4-F)", clear, 6)
    check("KA 7.4.3 cells below 1.00x (7.4.3, 7.4-F option D)", below, 4)
    check("MCQ 7.4-F failing cells stated in the rationale", 16 - clear, 10)
    check("KA 7.4.3 invariant: no clearing cell sits below forecast despatch",
          1 if all(vf >= 1 for (pf, vf), v in got.items() if v >= COV) else 0, 1)
    check("KA 7.4.3 invariant: three of the four sub-1.00x cells are in the -20 % column",
          sum(1 for (pf, vf), v in got.items() if v < 1 and vf == D("0.80")), 3)
    check("KA 7.4.3 invariant: base despatch clears only at base tariff and above",
          sum(1 for (pf, vf), v in got.items() if vf == 1 and v >= COV), 2)
