"""PFL-AI Domain 8 — golden checks for MCQ 8.1-G, 8.1-H, 8.2-F, 8.2-G, 8.3-G, 8.4-F and 8.4-G.

Scope: the Evaluation and Comprehension items added to KA 8.1–8.4. Every number printed in those
stems, options and rationales is recomputed here from first principles; master-thread values are read
from ctx, never re-typed. `pfl_d08.py` owns the domain's worked examples and the items that preceded
these, so the two modules never edit one file.

Engines used:

  1. Estimate class (8.1.2) — contingency to the upper bound = base x upper-bound percentage; the
     Stage C P80 solves (b - x)^2 = 0.20 (b - a)(b - m) on a triangular distribution.
  2. The area rule (8.2.2) — IDC = r_q x g x S x sum cum(t-1), computed both closed-form and by the
     full recursion so the two must agree; then the escalation pitfall of 8.2.3, escalating a total
     against escalating the profile.
  3. Register aggregation and release (8.3.2, 8.3.3) — mean = sum EMV, variance = sum p(1-p)impact^2,
     P80 = mean + 0.8416 sigma; the register is then re-aggregated with the ground-conditions item
     removed, and the excess over the percentage rule is carried through gearing, the annuity factor,
     the instalment, the covenant trigger and the headroom.
  4. Delay during construction (8.4.2) — escalation on remaining owner-retained scope at the monthly
     factor, interest on the balance DRAWN at the date of the slip, deferred CFADS, and the coverage
     consequence of capitalising the first two.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))
    DEBT = ctx["KESTREL_DEBT"]                 # 42,000,000
    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    DS = ctx["KESTREL_INSTALMENT"]             # 5,009,635.23
    RATE = ctx["KESTREL_RATE"]                 # 0.06
    AF6_12 = af(RATE, ctx["KESTREL_TENOR"])
    EPC, CONT, COV = D(48000000), D(3645403), D("1.20")
    ESCAL, G = D("0.036"), D("0.70")
    head0 = CF - DS * COV
    check("D8-lvl annual covenant headroom", q(head0, 0), 372438)

    # ================= MCQ 8.1-G — the wrap declined, the provision retained ===========
    check("MCQ 8.1-G funded contingency as % of base", q(CONT / EPC * 100, 2), D("7.59"))
    check("MCQ 8.1-G coverage of the Stage E (+8 %) band %",
          q(CONT / (EPC * D("0.08")) * 100, 2), D("94.93"))
    check("MCQ 8.1-G coverage of the Stage C (+30 %) band %",
          q(CONT / (EPC * D("0.30")) * 100, 2), D("25.32"))
    check("MCQ 8.1-G Stage C upper-bound provision", EPC * D("0.30"), 14400000)
    a, m, b = EPC * D("0.85"), EPC, EPC * D("1.30")
    p80_total = b - ((D("0.20") * (b - a) * (b - m)).sqrt())
    check("MCQ 8.1-G Stage C P80 contingency", q(p80_total - EPC, 0), 6512795)
    check("MCQ 8.1-G the 10 % rule", EPC * D("0.10"), 4800000)
    check("MCQ 8.1-G the 10 % rule as a share of the Stage C upper bound",
          q(EPC * D("0.10") / (EPC * D("0.30")), 4), D("0.3333"))

    # ================= MCQ 8.1-H — operating cost is a coverage driver =================
    check("MCQ 8.1-H a 500,000 opex error against annual headroom",
          q(D(500000) / head0, 2), D("1.34"))

    # ================= MCQ 8.2-F / 8.2-G — escalation and the area rule ================
    A = [D(w) for w in (6, 9, 13, 16, 17, 15, 13, 11)]
    B = [D(w) for w in (14, 16, 16, 14, 12, 11, 9, 8)]
    C = [D(w) for w in (5, 7, 9, 12, 15, 17, 18, 17)]
    RQ, S = RATE / 4, EPC
    COEF = RQ * G * S
    check("MCQ 8.2-G area-rule coefficient r_q x g x S", COEF, 504000)
    check("MCQ 8.2-F/G profiles each sum to 100 per cent",
          1 if all(sum(p) == 100 for p in (A, B, C)) else 0, 1)

    def area(p):
        cum = tot = D(0)
        for w in p:
            tot += cum / 100
            cum += w
        return tot

    def idc(p, factor=None):
        bal = tot = D(0)
        for t, w in enumerate(p, start=1):
            tot += bal * RQ
            spend = S * w / 100
            if factor is not None:
                spend *= factor ** t
            bal += G * spend
        return tot

    for lab, p, want in (("A (S-curve)", A, 1607760), ("B (front-loaded)", B, 2000880),
                         ("C (back-loaded)", C, 1345680)):
        check(f"MCQ 8.2-G profile {lab} IDC from the area rule", COEF * area(p), want)
        check(f"MCQ 8.2-G profile {lab} closed form equals the full recursion",
              idc(p), COEF * area(p), D("0.005"))
    idcA, idcB, idcC = (COEF * area(p) for p in (A, B, C))
    check("MCQ 8.2-G spread between the extreme profiles", idcB - idcC, 655200)
    check("MCQ 8.2-G that spread as % of the S-curve's own IDC",
          q((idcB - idcC) / idcA * 100, 2), D("40.75"))
    check("MCQ 8.2-G invariant: identical total spend and duration on all three profiles",
          1 if all(sum(S * w / 100 for w in p) == S and len(p) == 8 for p in (A, B, C)) else 0, 1)

    FQ = (1 + ESCAL) ** (D(1) / 4)

    def esc_spend(p, factor):
        return sum(S * w / 100 * factor ** t for t, w in enumerate(p, start=1))

    check("MCQ 8.2-F profile-correct escalated spend, S-curve", q(esc_spend(A, FQ), 0), 50093393)
    check("MCQ 8.2-F the pitfall: the total escalated by 1.036^2", S * (1 + ESCAL) ** 2, 51518208)
    check("MCQ 8.2-F size of the overstatement",
          q(S * (1 + ESCAL) ** 2, 0) - q(esc_spend(A, FQ), 0), 1424815)
    check("MCQ 8.2-F construction escalation is a distinct assumption from revenue's 2.967 %",
          1 if ESCAL != D("0.02967") else 0, 1)

    # ================= MCQ 8.3-G — the release, and what it costs if drawn ============
    REG = ((D("0.40"), D(2400000)), (D("0.30"), D(1800000)), (D("0.35"), D(1400000)),
           (D("0.50"), D(900000)), (D("0.20"), D(2000000)), (D("0.25"), D(-600000)))

    def aggregate(rows):
        mean = sum(p * i for p, i in rows)
        sd = (sum(p * (1 - p) * i * i for p, i in rows)).sqrt()
        return mean, sd, mean + D("0.8416") * sd

    mean, sd, p80 = aggregate(REG)
    check("MCQ 8.3-G register mean exposure", mean, 2690000)
    check("MCQ 8.3-G register sigma", q(sd, 0), 1848973)
    check("MCQ 8.3-G register P80", q(p80, 0), 4246095)
    mean2, sd2, p80_2 = aggregate(REG[1:])     # ground conditions (p 0.40, 2,400,000) retires
    check("MCQ 8.3-G register mean after the retirement", mean2, 1730000)
    check("MCQ 8.3-G register sigma after the retirement", q(sd2, 0), 1426990)
    check("MCQ 8.3-G register P80 after the retirement", q(p80_2, 0), 2930955)
    RULE = EPC * D("0.10")
    excess = RULE - q(p80_2, 0)
    check("MCQ 8.3-G excess provision the percentage rule still holds", excess, 1869045)
    check("MCQ 8.3-G commitment fee on the excess at 0.60 %",
          q(excess * D("0.006"), 0), 11214)
    added_debt = excess * G
    check("MCQ 8.3-G senior debt added if the excess is drawn", q(added_debt, 0), 1308332)
    add_inst = added_debt / AF6_12
    check("MCQ 8.3-G addition to the annual instalment", q(add_inst, 0), 156054)
    inst2 = DS + add_inst
    check("MCQ 8.3-G instalment after the draw", q(inst2, 2), D("5165689.12"))
    check("MCQ 8.3-G DSCR after the draw", q(CF / inst2, 4), D("1.2358"))
    head2 = CF - inst2 * COV
    check("MCQ 8.3-G headroom after the draw", q(head2, 0), 185173)
    check("MCQ 8.3-G headroom lost", q(head0 - head2, 0), 187265)
    check("MCQ 8.3-G headroom lost %", q((head0 - head2) / head0 * 100, 1), D("50.3"))
    # INVARIANT: the asymmetry the item turns on
    check("MCQ 8.3-G invariant: the annual coverage loss dwarfs the undrawn fee",
          1 if (head0 - head2) > excess * D("0.006") * 16 else 0, 1)

    # ================= MCQ 8.4-F — capitalise, or fund with equity =================
    DRAWN, REM_OWN = D("31990655"), D(922906)
    FM = (1 + ESCAL) ** (D(1) / 12)
    esc_row, int_row, rev_row = REM_OWN * (FM - 1), DRAWN * RATE / 12, CF / 12
    check("MCQ 8.4-F escalation row per month", q(esc_row, 0), 2724)
    check("MCQ 8.4-F interest row per month", q(int_row, 0), 159953)
    check("MCQ 8.4-F deferred CFADS row per month", rev_row, 532000)
    cap3 = (esc_row + int_row) * 3
    check("MCQ 8.4-F capitalised cost of three months", q(cap3, 0), 488032)
    inst3 = (DEBT + cap3) / AF6_12
    check("MCQ 8.4-F debt after capitalisation", q(DEBT + cap3, 0), 42488032)
    check("MCQ 8.4-F instalment after capitalisation", q(inst3, 2), D("5067846.24"))
    check("MCQ 8.4-F sized DSCR", q(CF / DS, 4), D("1.2743"))
    check("MCQ 8.4-F DSCR after capitalisation", q(CF / inst3, 4), D("1.2597"))
    head3 = CF - inst3 * COV
    check("MCQ 8.4-F headroom after capitalisation", q(head3, 0), 302585)
    check("MCQ 8.4-F headroom lost %", q((head0 - head3) / head0 * 100, 1), D("18.8"))
    check("MCQ 8.4-F invariant: 488,032 is under 1 % of the envelope yet costs 18.8 % of headroom",
          1 if cap3 / ctx["KESTREL_CAPEX"] < D("0.01") else 0, 1)
