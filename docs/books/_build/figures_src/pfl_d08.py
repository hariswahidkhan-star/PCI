"""PFL-AI Domain 8 — Cost, schedule and contingency integration. PCI original artwork.

Fig 8.3.1  Six answers to one question: contingency for Kestrel's 48,000,000 base estimate
           by method, from the register mean (2,690,000) to the worst-case sum (8,500,000),
           with the funded balancing line (3,645,403) and the confidence each buys.
Fig 8.4.1  The cost of a month of slip rises through the build: escalation on owner-retained
           scope, extra interest on the drawn balance and deferred CFADS, by quarter, against
           delay damages of 600,000 per month.

Deterministic: every value below is a literal verified in the domain's arithmetic pass
(decimal.Decimal throughout; see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 8.3.1 — six contingency answers -------------------------------------
    # (label, amount, % of 48,000,000 base, note, emphasis)
    methods = [
        ("Worst-case sum of threats", 8500000, "17.71 %", "funds nothing anyone will approve", 0),
        ("Range P80 (Stage C, unwrapped)", 6512795, "13.57 %", "estimate uncertainty, no wrap", 0),
        ("Ten per cent of base &#8212; the rule", 4800000, "10.00 %", "P87.3 on the register, P70.4 on the range", 2),
        ("Register P80 (wrapped scope)", 4246095, "8.85 %", "the defensible provision here", 1),
        ("Funded balancing line (D6)", 3645403, "7.59 %", "a P69.7 provision &#8212; 600,692 short", 2),
        ("Register mean, &#931; EMV", 2690000, "5.60 %", "funds the average, exceeded half the time", 0),
    ]
    W, H = 720, 430
    L, R, T, B = 250, 132, 78, 56
    plot_w = W - L - R
    xmax = 9000000.0
    n = len(methods)
    row_h = (H - T - B) / n

    def X(v):
        return L + v / xmax * plot_w

    # Gridlines every 1.5m, labelled to one decimal. At zero decimals the 1.5m, 4.5m and 7.5m lines
    # printed as "2m", "4m" and "8m" — a ruler misstating its own gridlines by up to half a million
    # and reading 0/2/3/4/6/8/9, which implies uneven intervals on a chart whose entire subject is
    # the spread between six contingency figures.
    grid = "".join(
        f'<line x1="{X(v):.1f}" y1="{T}" x2="{X(v):.1f}" y2="{H - B}" stroke="{GRID}"/>'
        f'<text x="{X(v):.1f}" y="{H - B + 16}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{v / 1000000:.1f}m</text>'
        for v in range(0, 9000001, 1500000))
    bars = ""
    for i, (lab, amt, pct, note, emph) in enumerate(methods):
        y = T + i * row_h + row_h * 0.20
        h = row_h * 0.46
        col = CRIMSON if emph == 2 else (BLUE if emph == 1 else SLATE)
        op = "0.95" if emph else "0.42"
        bars += (f'<rect x="{L}" y="{y:.1f}" width="{X(amt) - L:.1f}" height="{h:.1f}" '
                 f'fill="{col}" opacity="{op}"/>')
        bars += (f'<text x="{L - 10}" y="{y + h * 0.62:.1f}" font-size="10.4" '
                 f'fill="{INK}" text-anchor="end" font-weight="600">{lab}</text>')
        bars += (f'<text x="{L - 10}" y="{y + h * 0.62 + 13:.1f}" font-size="9.4" '
                 f'fill="{SLATE}" text-anchor="end">{note}</text>')
        bars += (f'<text x="{X(amt) + 8:.1f}" y="{y + h * 0.62:.1f}" font-size="10.4" '
                 f'font-weight="700" fill="{col if emph else INK}">{amt:,}</text>')
        bars += (f'<text x="{X(amt) + 8:.1f}" y="{y + h * 0.62 + 13:.1f}" font-size="9.4" '
                 f'fill="{SLATE}">{pct}</text>')
    body = (grid + bars
            + axes(L, T, W - R, H - B, "Contingency on a 48,000,000 base estimate (USD)", "")
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'Six methods, one question &#8212; and a 3.16&#215; spread between the answers'
              '</text>'
            + f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
              'Crimson: the two numbers that do not state a confidence level. Blue: the provision '
              'the register supports at P80 under a fixed-price wrap</text>'
            + f'<text x="24" y="59" font-size="10.4" fill="{CRIMSON}">'
              'The rule of thumb buys P87.3 against the risk register and P70.4 against the '
              'estimate range &#8212; the same 4,800,000, two different promises</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 16}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'A contingency percentage is meaningless without the estimate class and the basis '
              'it was sized on</text>')
    (PFL / "fig_8_3_1.svg").write_text(svg(W, H, body))

    # ---- Fig 8.4.1 — cost of a month of slip through the build --------------------
    # (x label, escalation on owner-retained scope, extra interest, deferred CFADS)
    slip = [
        ("close", 11089, 9240, 532000),
        ("q1", 10446, 20939, 532000),
        ("q2", 9473, 38561, 532000),
        ("q3", 8054, 64102, 532000),
        ("q4", 6293, 95713, 532000),
        ("q5", 4405, 129589, 532000),
        ("q6", 2724, 159953, 532000),
        ("q7", 1255, 186769, 532000),
        ("COD", 0, 210000, 532000),
    ]
    W, H = 710, 450
    L, R, T, B = 84, 154, 80, 76
    plot_w = W - L - R
    ymax = 800000.0
    n = len(slip)

    def Xs(i):
        return L + (i + 0.5) * plot_w / n

    def Ys(v):
        return H - B - v / ymax * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Ys(v):.1f}" x2="{W - R}" y2="{Ys(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Ys(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v / 1000:.0f}k</text>'
        for v in range(0, 800001, 200000))
    barw = plot_w / n * 0.56
    bars = ""
    for i, (lab, e, it, rev) in enumerate(slip):
        x = Xs(i) - barw / 2
        bars += (f'<rect x="{x:.1f}" y="{Ys(rev):.1f}" width="{barw:.1f}" '
                 f'height="{(H - B) - Ys(rev):.1f}" fill="{BLUE}" opacity="0.30"/>')
        bars += (f'<rect x="{x:.1f}" y="{Ys(rev + it):.1f}" width="{barw:.1f}" '
                 f'height="{Ys(rev) - Ys(rev + it):.1f}" fill="{INK}" opacity="0.80"/>')
        if e:
            bars += (f'<rect x="{x:.1f}" y="{Ys(rev + it + e):.1f}" width="{barw:.1f}" '
                     f'height="{Ys(rev + it) - Ys(rev + it + e):.1f}" fill="{CRIMSON}"/>')
        bars += (f'<text x="{Xs(i):.1f}" y="{H - B + 16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{lab}</text>')
        bars += (f'<text x="{Xs(i):.1f}" y="{Ys(rev + it + e) - 7:.1f}" font-size="9.2" '
                 f'fill="{INK}" text-anchor="middle" font-weight="600">'
                 f'{(rev + it + e) / 1000:.0f}</text>')
    body = (grid + bars + axes(L, T, W - R, H - B,
                               "Point in the construction programme at which the slip is declared",
                               "Cost of one month of slip (USD)")
            + f'<line x1="{L}" y1="{Ys(600000):.1f}" x2="{W - R}" y2="{Ys(600000):.1f}" '
              f'stroke="{CRIMSON}" stroke-width="1.8" stroke-dasharray="6 4"/>'
            + f'<text x="{L + 6}" y="{Ys(600000) - 7:.1f}" font-size="10.6" font-weight="700" '
              f'fill="{CRIMSON}">delay damages 20,000/day = 600,000 per month</text>'
            + f'<text x="{W - R + 8}" y="{Ys(742000) + 4:.1f}" font-size="10.4" '
              f'font-weight="700" fill="{INK}">742,000</text>'
            + f'<text x="{W - R + 8}" y="{Ys(742000) + 17:.1f}" font-size="9.6" fill="{SLATE}">'
              'at COD (D5, KA 5.4.2)</text>'
            + f'<text x="{W - R + 8}" y="{Ys(520000) + 4:.1f}" font-size="10" fill="{BLUE}">'
              'deferred CFADS</text>'
            + f'<text x="{W - R + 8}" y="{Ys(520000) + 15:.1f}" font-size="9.6" fill="{BLUE}">'
              '532,000 &#8212; flat</text>'
            + f'<text x="{W - R + 8}" y="{Ys(640000) + 4:.1f}" font-size="10" fill="{INK}">'
              'extra interest</text>'
            + f'<text x="{W - R + 8}" y="{Ys(640000) + 15:.1f}" font-size="9.6" fill="{INK}">'
              '9,240 &#8594; 210,000</text>'
            + f'<text x="{W - R + 8}" y="{Ys(715000) + 4:.1f}" font-size="10" fill="{CRIMSON}">'
              'escalation</text>'
            + f'<text x="{W - R + 8}" y="{Ys(715000) + 15:.1f}" font-size="9.6" fill="{CRIMSON}">'
              '11,089 &#8594; 0</text>'
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'A month of slip costs 552,329 at close and 742,000 at COD &#8212; the same month, '
              '34.3 % dearer</text>'
            + f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
              'Escalation falls as the remaining scope runs out; interest rises as the balance '
              'builds. Only the deferred-revenue band is constant</text>'
            + f'<text x="24" y="59" font-size="10.4" fill="{CRIMSON}">'
              'Damages calibrated at 20,000 per day cover the cost until quarter three and never '
              'again &#8212; the crossing is between q2 (580,033) and q3 (604,156)</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 14}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'A single damages rate cannot be right for a cost that moves 34.3 % across the '
              'programme it insures</text>')
    (PFL / "fig_8_4_1.svg").write_text(svg(W, H, body))
