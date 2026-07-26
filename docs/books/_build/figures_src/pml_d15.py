"""PML-AI Domain 15 — Programmes, Portfolios and Enterprise Delivery. PCI original artwork.

Fig 15.1.1  Why a programme milestone becomes improbable (KA 15.1.3). Probability that a merged
            milestone is met against the number of independent predecessors k = 1&#8230;24, for four
            uniform per-predecessor probabilities p = 0.99 / 0.95 / 0.90 / 0.85. Each curve is the
            product p^k. Meridian&#8217;s Region A go&#8209;live (six heterogeneous dependencies,
            0.90 &#183; 0.88 &#183; 0.85 &#183; 0.92 &#183; 0.95 &#183; 0.90 = 52.9539 %) and the
            four-region programme milestone (24 dependencies, 52.9539 %^4 = 7.8631 %) are marked.
            A dashed rule at 80 % carries the design consequence: 80 % across 24 independent
            dependencies requires 99.0745 % on every one of them. The 50 % crossings are annotated
            at k = 5 (p = 0.85), k = 7 (p = 0.90), k = 14 (p = 0.95) and k = 69 (p = 0.99,
            off-scale).

Fig 15.2.1  Aggregate capacity feasible, period capacity not (KA 15.2.3). Six portfolio candidates
            compete for one scarce integration team of 6 units per quarter over four quarters
            (aggregate 24 units). Three plans are drawn as grouped per-quarter bars against the
            capacity rule: the aggregate-only ranking C+A+B (24 of 24 units, NPV 4,200,000 &#8212;
            and infeasible, demanding 9 units in Q1); the per-unit greedy C+D+E (feasible,
            NPV 2,800,000, 73.49 % of the optimum); and the enumerated optimum A+B+F (feasible,
            NPV 3,810,000, 23 of 24 units, slack 0/1/0/0). A side panel prints the three NPVs, the
            390,000 aggregate illusion and the 1,010,000 greedy shortfall.

All values are those computed in the manuscript with decimal arithmetic (probabilities held as exact
rationals). Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""

from fractions import Fraction as F


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # =========================================== Fig 15.1.1 — the dependency product collapse
    KMAX = 24
    W, H = 700, 460
    L, R, T, B = 74, 236, 34, 66

    def X(k):
        return L + (k - 1) / (KMAX - 1) * (W - R - L)

    def Y(v):                                # v as a fraction of 1
        return (H - B) - float(v) * (H - T - B)

    body = ""
    for pct in range(0, 101, 10):
        y = Y(pct / 100.0)
        body += (f'<line x1="{L}" y1="{y:.1f}" x2="{W-R}" y2="{y:.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{y+4:.1f}" font-size="9.8" fill="{SLATE}" '
                 f'text-anchor="end">{pct} %</text>')
    for k in (1, 4, 6, 8, 12, 16, 20, 24):
        body += (f'<text x="{X(k):.1f}" y="{H-B+17}" font-size="9.8" fill="{SLATE}" '
                 f'text-anchor="middle">{k}</text>')

    # the 80 % design rule
    body += (f'<line x1="{L}" y1="{Y(0.80):.1f}" x2="{W-R}" y2="{Y(0.80):.1f}" stroke="{INK}" '
             f'stroke-width="1" stroke-dasharray="5 4"/>'
             f'<text x="{W-R-4}" y="{Y(0.80)+15:.1f}" font-size="9.8" fill="{INK}" '
             f'text-anchor="end">80 % target &#8212; at k = 24 it needs 99.0745 % on every '
             f'dependency</text>')

    CURVES = ((F(99, 100), "p = 0.99", 69), (F(95, 100), "p = 0.95", 14),
              (F(90, 100), "p = 0.90", 7), (F(85, 100), "p = 0.85", 5))
    for idx, (p, lab, k50) in enumerate(CURVES):
        pts = " ".join(f"{X(k):.1f},{Y(p ** k):.1f}" for k in range(1, KMAX + 1))
        wide = 2.6 if idx else 2.0
        dash = "" if idx in (1, 2) else ' stroke-dasharray="7 4"'
        col = BLUE if idx in (1, 2) else SLATE
        body += (f'<polyline points="{pts}" fill="none" stroke="{col}" '
                 f'stroke-width="{wide}"{dash}/>'
                 f'<text x="{W-R+8}" y="{Y(p ** KMAX)+4:.1f}" font-size="10.2" fill="{col}" '
                 f'font-weight="600">{lab}</text>')
        note = (f"50 % at k = {k50}" if k50 <= KMAX
                else f"50 % only at k = {k50}")
        body += (f'<text x="{W-R+8}" y="{Y(p ** KMAX)+16:.1f}" font-size="9.2" fill="{SLATE}">'
                 f'{note}</text>')

    # Meridian markers
    PR = F(90, 100) * F(88, 100) * F(85, 100) * F(92, 100) * F(95, 100) * F(90, 100)
    body += (f'<circle cx="{X(6):.1f}" cy="{Y(PR):.1f}" r="4.6" fill="{CRIMSON}"/>'
             f'<text x="{X(6)+9:.1f}" y="{Y(PR)-10:.1f}" font-size="10.2" fill="{CRIMSON}" '
             f'font-weight="600">Region A go&#8209;live: k = 6, 52.9539 %</text>'
             f'<text x="{X(6)+9:.1f}" y="{Y(PR)+3:.1f}" font-size="9.2" fill="{SLATE}">'
             f'0.90 &#183; 0.88 &#183; 0.85 &#183; 0.92 &#183; 0.95 &#183; 0.90</text>')
    body += (f'<circle cx="{X(24):.1f}" cy="{Y(PR ** 4):.1f}" r="4.6" fill="{CRIMSON}"/>'
             f'<text x="{X(24)-6:.1f}" y="{Y(PR ** 4)-10:.1f}" font-size="10.2" fill="{CRIMSON}" '
             f'font-weight="600" text-anchor="end">four regions, 24 dependencies: '
             f'7.8631 %</text>')

    body += axes(L, T, W - R, H - B,
                 "Number of independent predecessors k feeding the milestone",
                 "Probability the milestone is met")
    (PML / "fig_15_1_1.svg").write_text(svg(W, H, body))

    # ================================ Fig 15.2.1 — aggregate feasible, period infeasible
    CAND = {"A": (2, 3, 3, 2), "B": (2, 2, 2, 2), "C": (5, 1, 0, 0),
            "D": (0, 1, 2, 3), "E": (1, 1, 1, 1), "F": (2, 0, 1, 2)}
    PLANS = (("aggregate-only ranking  C + A + B", ("C", "A", "B"), CRIMSON, 4200000, False),
             ("per-unit greedy  C + D + E", ("C", "D", "E"), SLATE, 2800000, True),
             ("enumerated optimum  A + B + F", ("A", "B", "F"), BLUE, 3810000, True))
    CAP = 6

    W2, H2 = 700, 430
    L2, R2, T2, B2 = 70, 246, 44, 74
    GW = (W2 - R2 - L2) / 4.0            # per-quarter group width
    BW = GW * 0.24

    def Y2(u):
        return (H2 - B2) - u / 10.0 * (H2 - T2 - B2)

    b2 = ""
    for u in range(0, 11, 2):
        y = Y2(u)
        b2 += (f'<line x1="{L2}" y1="{y:.1f}" x2="{W2-R2}" y2="{y:.1f}" stroke="{GRID}"/>'
               f'<text x="{L2-8}" y="{y+4:.1f}" font-size="9.8" fill="{SLATE}" '
               f'text-anchor="end">{u}</text>')
    # capacity rule
    b2 += (f'<line x1="{L2}" y1="{Y2(CAP):.1f}" x2="{W2-R2}" y2="{Y2(CAP):.1f}" '
           f'stroke="{INK}" stroke-width="1.6" stroke-dasharray="6 3"/>'
           f'<text x="{L2+6}" y="{Y2(CAP)-6:.1f}" font-size="10.2" fill="{INK}" '
           f'font-weight="600">capacity 6 units a quarter</text>')

    for q in range(4):
        gx = L2 + q * GW
        b2 += (f'<text x="{gx+GW/2:.1f}" y="{H2-B2+18}" font-size="10.4" fill="{INK}" '
               f'text-anchor="middle">Q{q+1}</text>')
        for pi, (lab, sel, col, npv, feas) in enumerate(PLANS):
            d = sum(CAND[s][q] for s in sel)
            x = gx + GW * 0.14 + pi * BW * 1.12
            y = Y2(d)
            b2 += (f'<rect x="{x:.1f}" y="{y:.1f}" width="{BW:.1f}" '
                   f'height="{(H2-B2)-y:.1f}" fill="{col}" '
                   f'opacity="{0.95 if feas else 0.55}"/>'
                   f'<text x="{x+BW/2:.1f}" y="{y-4:.1f}" font-size="9.6" fill="{col}" '
                   f'text-anchor="middle" font-weight="600">{d}</text>')
            if d > CAP:
                b2 += (f'<text x="{x+BW/2:.1f}" y="{y-16:.1f}" font-size="10.6" '
                       f'fill="{CRIMSON}" text-anchor="middle" font-weight="700">&#215;</text>')

    notes = (("plan", "NPV, USD", "units", "verdict"),
             ("C + A + B (aggregate)", "4,200,000", "24 of 24", "INFEASIBLE &#8212; 9 in Q1"),
             ("C + D + E (greedy)", "2,800,000", "16 of 24", "feasible, 73.49 % of best"),
             ("A + B + F (enumerated)", "3,810,000", "23 of 24", "feasible OPTIMUM"))
    cols = (INK, CRIMSON, SLATE, BLUE)
    for r, row in enumerate(notes):
        y = T2 + 12 + r * 22
        col = cols[r]
        wt = 700 if r == 0 else 600
        b2 += (f'<text x="{W2-R2+16}" y="{y}" font-size="9.8" fill="{col}" '
               f'font-weight="{wt}">{row[0]}</text>'
               f'<text x="{W2-R2+152}" y="{y}" font-size="9.8" fill="{col}" '
               f'text-anchor="end" font-weight="{wt}">{row[1]}</text>'
               f'<text x="{W2-R2+16}" y="{y+11}" font-size="9.0" fill="{SLATE}">'
               f'{row[2]} &#183; {row[3]}</text>')
    y0 = T2 + 12 + 4 * 22 + 14
    tail = ("the aggregate illusion overstates",
            "achievable value by 390,000 (10.24 %);",
            "greedy leaves 1,010,000 on the table",
            "(26.51 % of the optimum). Enumerating",
            "all 63 subsets finds 26 feasible ones.")
    for r, line in enumerate(tail):
        b2 += (f'<text x="{W2-R2+16}" y="{y0+r*14:.1f}" font-size="9.6" fill="{INK}">'
               f'{line}</text>')

    b2 += axes(L2, T2, W2 - R2, H2 - B2, "Quarter",
               "Demand on the scarce integration team (units)")
    (PML / "fig_15_2_1.svg").write_text(svg(W2, H2, b2))
