"""PFL-AI Domain 16 — Data, automation and responsible AI in finance. PCI original artwork.

Fig 16.1.2  The automation breakeven, drawn as net annual saving against annual review
            volume for three assumptions about the cost of an undetected error. All three
            lines start at &#8722;148,000 (the automated pipeline's committed annual fixed
            cost) and rise with volume at the per-record advantage: 12.56 when the error
            cost is ignored, 10.00 at USD 320 per undetected error, and 2.96 at USD 1,200.
            The x-intercepts are the breakevens — 11,783, 14,800 and 50,000 records a year
            — and the two vertical markers are Kestrel Water alone (9,400 records, a loss
            of 54,000 on the 320 line) and the six-asset portfolio (56,400 records, a gain
            of 416,000 on the 320 line but only 18,944 on the 1,200 line).

Fig 16.1.3  The cost-optimal alert threshold for the portfolio's payment-and-journal
            anomaly detector over 48,000 items a year at a 2.5 % anomaly rate, with a
            false positive costing USD 40 and a missed error USD 320. Bars are total annual
            cost at five thresholds (196,400 / 161,800 / 131,200 / 114,800 / 154,400); the
            overlaid line is classification accuracy (98.5208 / 98.5729 / 98.4167 /
            97.5208 / 93.7083 per cent). Cost is minimised at T4 and accuracy is maximised
            at T2 — two different thresholds, 47,000 a year apart.

Deterministic: every annotated value is a literal verified in the domain's decimal
arithmetic pass (see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ================================================================= Fig 16.1.2
    FIXED = 148000.0
    # (per-record advantage, breakeven volume, legend text, colour, dash)
    lines = [
        (12.56, 11783.4395, "Error cost ignored &#8212; 12.56 a record; breakeven 11,783",
         SLATE, "6 4"),
        (10.00, 14800.0000, "USD 320 an undetected error &#8212; 10.00; breakeven 14,800",
         BLUE, ""),
        (2.96, 50000.0000, "USD 1,200 an undetected error &#8212; 2.96; breakeven 50,000",
         CRIMSON, ""),
    ]
    W, H = 820, 540
    L, R, T, B = 96, 44, 118, 92
    xhi = 70000.0
    ylo, yhi = -220000.0, 780000.0

    def X(v):
        return L + v / xhi * (W - L - R)

    def Y(v):
        return H - B - (v - ylo) / (yhi - ylo) * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W - R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 4:.1f}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="end">{"&#8722;" if v < 0 else ""}{abs(v) / 1000:.0f}k</text>'
        for v in (-200000, 0, 200000, 400000, 600000))
    grid += "".join(
        f'<line x1="{X(v):.1f}" y1="{T}" x2="{X(v):.1f}" y2="{H - B}" stroke="{GRID}"/>'
        f'<text x="{X(v):.1f}" y="{H - B + 17}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="middle">{v // 1000:,}k</text>'
        for v in (10000, 20000, 30000, 40000, 50000, 60000, 70000))

    zero = (f'<line x1="{L}" y1="{Y(0):.1f}" x2="{W - R}" y2="{Y(0):.1f}" stroke="{INK}" '
            f'stroke-width="1.3"/>')

    series, legend = "", ""
    for k, (slope, be, lab, col, dash) in enumerate(lines):
        y_end = slope * xhi - FIXED
        da = f' stroke-dasharray="{dash}"' if dash else ""
        series += (f'<line x1="{X(0):.1f}" y1="{Y(-FIXED):.1f}" x2="{X(xhi):.1f}" '
                   f'y2="{Y(y_end):.1f}" stroke="{col}" stroke-width="2.6"{da}/>')
        series += f'<circle cx="{X(be):.1f}" cy="{Y(0):.1f}" r="4.4" fill="{col}"/>'
        ly = T + 20 + k * 15
        legend += (f'<line x1="{L + 150}" y1="{ly - 3.5}" x2="{L + 174}" y2="{ly - 3.5}" '
                   f'stroke="{col}" stroke-width="2.6"{da}/>'
                   f'<text x="{L + 180}" y="{ly}" font-size="9.4" fill="{INK}">{lab}</text>')

    # on-chart breakeven callouts (the two that matter to the argument)
    series += (f'<text x="{X(14800.0) + 5:.1f}" y="{Y(0) + 30:.1f}" font-size="9.6" '
               f'font-weight="700" fill="{BLUE}">14,800</text>'
               f'<text x="{X(50000.0):.1f}" y="{Y(0) + 30:.1f}" font-size="9.6" '
               f'font-weight="700" fill="{CRIMSON}" text-anchor="middle">50,000</text>')

    marks = ""
    for vol, lab2, anchor in ((9400.0, "Kestrel alone &#183; 9,400", "start"),
                              (56400.0, "Six-asset portfolio &#183; 56,400", "end")):
        marks += (f'<line x1="{X(vol):.1f}" y1="{T - 8}" x2="{X(vol):.1f}" y2="{H - B}" '
                  f'stroke="{INK}" stroke-width="1.1" stroke-dasharray="3 4" opacity="0.75"/>'
                  f'<text x="{X(vol) + (6 if anchor == "start" else -6):.1f}" y="{T - 14}" '
                  f'font-size="10.0" font-weight="700" fill="{INK}" '
                  f'text-anchor="{anchor}">{lab2}</text>')
    marks += (f'<circle cx="{X(9400.0):.1f}" cy="{Y(-54000.0):.1f}" r="4.4" fill="{BLUE}"/>'
              f'<text x="{X(9400.0) - 8:.1f}" y="{Y(-54000.0) - 10:.1f}" font-size="9.6" '
              f'font-weight="700" fill="{BLUE}" text-anchor="end">&#8722;54,000</text>')
    marks += (f'<circle cx="{X(56400.0):.1f}" cy="{Y(416000.0):.1f}" r="4.4" fill="{BLUE}"/>'
              f'<text x="{X(56400.0) - 8:.1f}" y="{Y(416000.0) - 8:.1f}" font-size="9.8" '
              f'font-weight="700" fill="{BLUE}" text-anchor="end">+416,000</text>')
    marks += (f'<circle cx="{X(56400.0):.1f}" cy="{Y(18944.0):.1f}" r="4.4" fill="{CRIMSON}"/>'
              f'<text x="{X(56400.0) + 8:.1f}" y="{Y(18944.0) - 7:.1f}" font-size="9.6" '
              f'font-weight="700" fill="{CRIMSON}" text-anchor="end">+18,944</text>')

    head = (f'<text x="{L}" y="22" font-size="11.8" font-weight="700" fill="{INK}">'
            f'Where automation starts to pay &#8212; and what the error cost does to the '
            f'answer</text>'
            f'<text x="{L}" y="38" font-size="9.4" fill="{SLATE}">Net annual saving against '
            f'the manual process. Committed automated fixed cost 148,000 a year;</text>'
            f'<text x="{L}" y="51" font-size="9.4" fill="{SLATE}">manual review 13.60 a '
            f'record at a loaded 96.00 an hour; automated variable 1.04 a record.</text>'
            f'<text x="{L}" y="{H - 26}" font-size="9.6" fill="{SLATE}">Raising the cost of '
            f'an undetected error from 320 to 1,200 moves the breakeven from 14,800 records '
            f'to 50,000: the more</text>'
            f'<text x="{L}" y="{H - 12}" font-size="9.6" fill="{SLATE}">consequential the '
            f'error, the more volume automation needs &#8212; because its error rate is the '
            f'higher of the two.</text>')

    body = (head + grid + zero + series + legend + marks
            + axes(L, T, W - R, H - B, "Records reviewed a year",
                   "Net annual saving (USD)"))
    (PFL / "fig_16_1_2.svg").write_text(svg(W, H, body))

    # ================================================================= Fig 16.1.3
    # (label, score cut, recall %, total annual cost, accuracy %, precision %, alerts)
    thr = [
        ("T1", "0.90", 50, 196400.0, 98.5208, 84.5, 710),
        ("T2", "0.80", 60, 161800.0, 98.5729, 77.8, 925),
        ("T3", "0.70", 70, 131200.0, 98.4167, 67.7, 1240),
        ("T4", "0.60", 80, 114800.0, 97.5208, 50.3, 1910),
        ("T5", "0.45", 90, 154400.0, 93.7083, 27.1, 3980),
    ]
    W2, H2 = 820, 570
    L2, R2, T2_, B2 = 92, 92, 98, 142
    chi = 220000.0
    alo, ahi = 92.0, 99.6

    def CY(v):
        return H2 - B2 - v / chi * (H2 - T2_ - B2)

    def AY(v):
        return H2 - B2 - (v - alo) / (ahi - alo) * (H2 - T2_ - B2)

    span = (W2 - L2 - R2) / 5

    def BX(i):
        return L2 + (i + 0.20) * span

    bw = span * 0.60

    g2 = "".join(
        f'<line x1="{L2}" y1="{CY(v):.1f}" x2="{W2 - R2}" y2="{CY(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L2 - 8}" y="{CY(v) + 4:.1f}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="end">{v / 1000:.0f}k</text>'
        for v in (50000, 100000, 150000, 200000))
    g2 += "".join(
        f'<text x="{W2 - R2 + 10}" y="{AY(v) + 4:.1f}" font-size="9.6" fill="{CRIMSON}">'
        f'{v:.0f} %</text>' for v in (93, 95, 97, 99))

    bars = ""
    for i, (lab, cut, rec, cost, acc, prec, alerts) in enumerate(thr):
        x = BX(i)
        best = cost == 114800.0
        col = CRIMSON if best else BLUE
        bars += (f'<rect x="{x:.1f}" y="{CY(cost):.1f}" width="{bw:.1f}" '
                 f'height="{(H2 - B2) - CY(cost):.1f}" fill="{col}" '
                 f'opacity="{"1" if best else "0.85"}"/>')
        bars += (f'<text x="{x + bw / 2:.1f}" y="{CY(cost) - 8:.1f}" font-size="10.6" '
                 f'font-weight="700" fill="{INK}" text-anchor="middle">{cost:,.0f}</text>')
        for j, txt in enumerate((f'score &#8805; {cut}', f'recall {rec} %',
                                 f'precision {prec} %', f'{alerts:,} alerts')):
            bars += (f'<text x="{x + bw / 2:.1f}" y="{H2 - B2 + 32 + j * 13}" font-size="9.2" '
                     f'fill="{SLATE}" text-anchor="middle">{txt}</text>')
        bars += (f'<text x="{x + bw / 2:.1f}" y="{H2 - B2 + 18}" font-size="10.8" '
                 f'font-weight="700" fill="{INK}" text-anchor="middle">{lab}</text>')

    pts = " ".join(f"{BX(i) + bw / 2:.1f},{AY(t[4]):.1f}" for i, t in enumerate(thr))
    accline = (f'<polyline points="{pts}" fill="none" stroke="{CRIMSON}" stroke-width="2.4" '
               f'stroke-dasharray="7 4"/>')
    for i, t in enumerate(thr):
        accline += (f'<circle cx="{BX(i) + bw / 2:.1f}" cy="{AY(t[4]):.1f}" r="4.0" '
                    f'fill="{CRIMSON}"/>')
    accline += (f'<text x="{BX(1) + bw / 2:.1f}" y="{AY(98.5729) - 13:.1f}" font-size="9.8" '
                f'font-weight="700" fill="{CRIMSON}" text-anchor="middle">'
                f'accuracy peak 98.5729 %</text>')

    call = (f'<text x="{BX(3) + bw / 2:.1f}" y="{CY(114800.0) + 34:.1f}" font-size="9.6" '
            f'font-weight="700" fill="white" text-anchor="middle">cost minimum</text>'
            f'<text x="{BX(3) + bw / 2:.1f}" y="{CY(114800.0) + 48:.1f}" font-size="9.2" '
            f'fill="white" text-anchor="middle">accuracy only</text>'
            f'<text x="{BX(3) + bw / 2:.1f}" y="{CY(114800.0) + 60:.1f}" font-size="9.2" '
            f'fill="white" text-anchor="middle">97.5208 %</text>'
            f'<line x1="{BX(1) + bw / 2:.1f}" y1="{CY(161800.0):.1f}" '
            f'x2="{BX(3) + bw / 2 - 48:.1f}" y2="{CY(161800.0):.1f}" stroke="{INK}" '
            f'stroke-width="1.1" stroke-dasharray="4 3"/>'
            f'<line x1="{BX(3) + bw / 2 - 48:.1f}" y1="{CY(161800.0):.1f}" '
            f'x2="{BX(3) + bw / 2 - 48:.1f}" y2="{CY(114800.0):.1f}" stroke="{INK}" '
            f'stroke-width="1.1" stroke-dasharray="4 3"/>'
            f'<text x="{BX(2) + bw / 2:.1f}" y="{CY(161800.0) - 7:.1f}" font-size="10.0" '
            f'font-weight="700" fill="{INK}" text-anchor="middle">47,000 a year</text>')

    xlabel = (f'<text x="{(L2 + W2 - R2) / 2:.1f}" y="{H2 - B2 + 100}" font-size="12" '
              f'fill="{SLATE}" text-anchor="middle">Alert threshold &#8212; strict on the '
              f'left, permissive on the right</text>')

    head2 = (f'<text x="{L2}" y="22" font-size="11.8" font-weight="700" fill="{INK}">'
             f'The accuracy-maximising threshold is not the cost-minimising threshold</text>'
             f'<text x="{L2}" y="38" font-size="9.4" fill="{SLATE}">48,000 payments and '
             f'journal entries a year, 2.5 % genuinely anomalous. A false positive costs 40 '
             f'to investigate; a missed error costs 320.</text>'
             f'<text x="{L2}" y="51" font-size="9.4" fill="{SLATE}">Bars: total annual cost '
             f'(left axis). Dashed line: classification accuracy (right axis).</text>'
             f'<text x="{L2}" y="{H2 - 24}" font-size="9.6" fill="{SLATE}">Act on an alert '
             f'whenever the chance it is real exceeds 40 &#247; 320 = 12.5 %, one in eight. '
             f'The optimum is where the marginal precision of</text>'
             f'<text x="{L2}" y="{H2 - 10}" font-size="9.6" fill="{SLATE}">loosening the '
             f'threshold crosses that line &#8212; 17.9104 % from T3 to T4, and 5.7971 % '
             f'from T4 to T5 &#8212; not where average precision looks respectable.</text>')

    body2 = (head2 + g2 + bars + accline + call + xlabel
             + axes(L2, T2_, W2 - R2, H2 - B2, "", "Total annual cost (USD)"))
    (PFL / "fig_16_1_3.svg").write_text(svg(W2, H2, body2))
