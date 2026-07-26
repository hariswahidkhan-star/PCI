"""PFL-AI Domain 14 — Construction monitoring and drawdown. PCI original artwork.

Fig 14.1.1  Kestrel's three funding orders drawn twice: the sponsor's share of cumulative
            funding drawn at each of the nine draw dates (pro rata flat at 30.00 %,
            equity-first falling from 100.00 % to 30.39 %, debt-first at 0.00 % until
            quarter six) beside the capitalised interest each order produces
            (1,338,006 / 2,114,597 / 2,804,070 — a spread of 1,466,064, or 2.4434 % of the
            60,000,000 envelope, and 41.6 times the cost of a ten-basis-point margin move).

Fig 14.4.1  The debt service coverage ratio at Kestrel's first, calendar-fixed repayment
            date against months of slip in the commercial operations date, drawn twice:
            once on operating cash alone (1.2743 falling to 0.4248 at eight months, through
            the 1.20 covenant at 0.7001 months and 1.00 at 2.5834 months) and once with
            delay damages of 20,000 a day credited to CFADS, which *rises* to 1.3829 at the
            cap and then collapses through 1.20 at 9.7226 months.

Deterministic: every annotated value is a literal verified in the domain's decimal
arithmetic pass (see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ================================================================= Fig 14.1.1
    # Left panel: sponsor share of cumulative funding drawn, by draw date.
    # Right panel: total capitalised interest by funding order.
    labels = ["Close", "Q1", "Q2", "Q3", "Q4", "Q5", "Q6", "Q7", "Q8"]
    pro = [30.00, 30.00, 30.00, 30.00, 30.00, 30.00, 30.00, 30.00, 30.00]
    eqf = [100.00, 100.00, 100.00, 99.40, 66.79, 49.35, 39.96, 34.20, 30.39]
    dbf = [0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 9.03, 22.20, 30.80]
    idc = [("Equity first", 1338006, BLUE), ("Pro rata 70/30", 2114597, SLATE),
           ("Debt first", 2804070, CRIMSON)]

    W, H = 820, 470
    L, T, B = 74, 74, 78
    PW = 430                                   # left panel width
    GAPX = 96
    RL = L + PW + GAPX                         # right panel left edge
    RW = W - RL - 30
    ph = H - T - B

    def X(i):
        return L + i / (len(labels) - 1) * PW

    def Y(pct):
        return H - B - pct / 100.0 * ph

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{L + PW}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 4:.1f}" font-size="9.8" fill="{SLATE}" '
        f'text-anchor="end">{v} %</text>'
        for v in (0, 20, 40, 60, 80, 100))
    xticks = "".join(
        f'<text x="{X(i):.1f}" y="{H - B + 17}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="middle">{lab}</text>' for i, lab in enumerate(labels))

    def poly(series):
        return " ".join(f"{X(i):.1f},{Y(v):.1f}" for i, v in enumerate(series))

    lines = (
        f'<polyline points="{poly(eqf)}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
        f'<polyline points="{poly(pro)}" fill="none" stroke="{SLATE}" stroke-width="2.4" '
        f'stroke-dasharray="7 4"/>'
        f'<polyline points="{poly(dbf)}" fill="none" stroke="{CRIMSON}" stroke-width="2.6"/>')
    dots = "".join(
        f'<circle cx="{X(i):.1f}" cy="{Y(v):.1f}" r="2.6" fill="{col}"/>'
        for series, col in ((eqf, BLUE), (pro, SLATE), (dbf, CRIMSON))
        for i, v in enumerate(series))

    ann = (
        f'<text x="{X(0.25):.1f}" y="{Y(100) - 10:.1f}" font-size="10.6" fill="{BLUE}" '
        f'font-weight="600">Equity first &#8212; sponsor funds first</text>'
        f'<text x="{X(4.1):.1f}" y="{Y(30) - 9:.1f}" font-size="10.6" fill="{SLATE}" '
        f'font-weight="600">Pro rata 70/30 &#8212; flat at 30.00 %</text>'
        f'<text x="{X(1.0):.1f}" y="{Y(0) - 11:.1f}" font-size="10.6" fill="{CRIMSON}" '
        f'font-weight="600">Debt first &#8212; no sponsor cash until Q6</text>'
        f'<line x1="{X(5):.1f}" y1="{Y(49.35):.1f}" x2="{X(5):.1f}" y2="{Y(0):.1f}" '
        f'stroke="{INK}" stroke-width="0.9" stroke-dasharray="3 3" opacity="0.6"/>'
        f'<text x="{X(5) + 7:.1f}" y="{Y(40):.1f}" font-size="9.8" fill="{INK}">at Q5 the lender'
        f'&#8217;s</text>'
        f'<text x="{X(5) + 7:.1f}" y="{Y(40) + 12:.1f}" font-size="9.8" fill="{INK}">exposure differs by</text>'
        f'<text x="{X(5) + 7:.1f}" y="{Y(40) + 24:.1f}" font-size="9.8" fill="{CRIMSON}" '
        f'font-weight="600">USD 11,406,157</text>')

    # right panel: horizontal IDC bars
    bmax = 3000000.0
    bar_h = 34
    bar_gap = 30

    def BX(v):
        return RL + v / bmax * RW

    bars = ""
    for k, (lab, v, col) in enumerate(idc):
        y = T + 22 + k * (bar_h + bar_gap)
        bars += (f'<rect x="{RL}" y="{y}" width="{BX(v) - RL:.1f}" height="{bar_h}" '
                 f'fill="{col}" opacity="0.82"/>'
                 f'<text x="{RL}" y="{y - 7}" font-size="10.4" fill="{INK}" '
                 f'font-weight="600">{lab}</text>'
                 f'<text x="{BX(v) + 7:.1f}" y="{y + bar_h / 2 + 4:.1f}" font-size="10.6" '
                 f'fill="{INK}" font-weight="600">{v:,}</text>')
    bars += (f'<line x1="{RL}" y1="{T + 8}" x2="{RL}" y2="{H - B - 6}" stroke="{INK}" '
             f'stroke-width="1.2"/>')
    bars += (f'<text x="{RL}" y="{H - B + 16}" font-size="10.2" fill="{SLATE}">Capitalised '
             f'interest, USD &#8212; spread 1,466,064</text>'
             f'<text x="{RL}" y="{H - B + 30}" font-size="10.2" fill="{SLATE}">= 2.4434 % of the '
             f'envelope, 41.6&#215; a 10 bp margin move</text>')

    titles = (
        f'<text x="{L - 46}" y="34" font-size="13.2" fill="{INK}" font-weight="700">Funding order: '
        f'who is exposed, and what the interest costs</text>'
        f'<text x="{L - 46}" y="52" font-size="10.4" fill="{SLATE}">Kestrel Water SPC &#8212; '
        f'60,000,000 envelope, 42,000,000 senior debt at 6.0 %, eight construction quarters</text>'
        f'<text x="{L}" y="{T - 12}" font-size="10.6" fill="{INK}" font-weight="600">Sponsor share '
        f'of cumulative funding drawn</text>'
        f'<text x="{RL}" y="{T - 12}" font-size="10.6" fill="{INK}" font-weight="600">Total interest '
        f'during construction</text>')

    body = (grid + axes(L, T, L + PW, H - B, "Draw date", "Equity as % of funds drawn")
            + xticks + lines + dots + ann + bars + titles)
    (PFL / "fig_14_1_1.svg").write_text(svg(W, H, body))

    # ================================================================= Fig 14.4.1
    # DSCR at the first (calendar-fixed) repayment date against months of COD slip.
    cash = [(0.0, 1.2743), (0.5, 1.2212), (1.0, 1.1681), (1.5, 1.1151), (2.0, 1.0620),
            (2.5, 1.0089), (3.0, 0.9558), (3.5, 0.9027), (4.0, 0.8496), (4.5, 0.7965),
            (5.0, 0.7434), (5.5, 0.6903), (6.0, 0.6372), (6.5, 0.5841), (7.0, 0.5310),
            (7.5, 0.4779), (8.0, 0.4248), (8.5, 0.3717), (9.0, 0.3186), (9.5, 0.2655),
            (10.0, 0.2124), (10.5, 0.1593), (11.0, 0.1062), (11.5, 0.0531), (12.0, 0.0000)]
    withld = [(0.0, 1.2743), (0.5, 1.2811), (1.0, 1.2879), (1.5, 1.2947), (2.0, 1.3015),
              (2.5, 1.3083), (3.0, 1.3151), (3.5, 1.3219), (4.0, 1.3286), (4.5, 1.3354),
              (5.0, 1.3422), (5.5, 1.3490), (6.0, 1.3558), (6.5, 1.3626), (7.0, 1.3694),
              (7.5, 1.3761), (8.0, 1.3829), (8.5, 1.3298), (9.0, 1.2767), (9.5, 1.2236),
              (10.0, 1.1705), (10.5, 1.1174), (11.0, 1.0643), (11.5, 1.0113), (12.0, 0.9582)]

    W2, H2 = 760, 470
    L2, R2, T2, B2 = 78, 176, 66, 74
    pw2, ph2 = W2 - L2 - R2, H2 - T2 - B2

    def X2(mo):
        return L2 + mo / 12.0 * pw2

    def Y2(v):
        return H2 - B2 - v / 1.45 * ph2

    grid2 = "".join(
        f'<line x1="{L2}" y1="{Y2(v):.1f}" x2="{W2 - R2}" y2="{Y2(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L2 - 8}" y="{Y2(v) + 4:.1f}" font-size="9.8" fill="{SLATE}" '
        f'text-anchor="end">{v:.2f}</text>'
        for v in (0.00, 0.20, 0.40, 0.60, 0.80, 1.00, 1.20, 1.40))
    xt2 = "".join(
        f'<text x="{X2(mo):.1f}" y="{H2 - B2 + 17}" font-size="9.8" fill="{SLATE}" '
        f'text-anchor="middle">{mo}</text>' for mo in range(0, 13, 2))

    p_cash = " ".join(f"{X2(mo):.1f},{Y2(v):.1f}" for mo, v in cash)
    p_ld = " ".join(f"{X2(mo):.1f},{Y2(v):.1f}" for mo, v in withld)

    thresholds = (
        f'<line x1="{L2}" y1="{Y2(1.20):.1f}" x2="{W2 - R2}" y2="{Y2(1.20):.1f}" '
        f'stroke="{CRIMSON}" stroke-width="1.1" stroke-dasharray="6 4"/>'
        f'<text x="{W2 - R2 + 6}" y="{Y2(1.20) + 4:.1f}" font-size="9.8" fill="{CRIMSON}">1.20 '
        f'covenant</text>'
        f'<line x1="{L2}" y1="{Y2(1.00):.1f}" x2="{W2 - R2}" y2="{Y2(1.00):.1f}" '
        f'stroke="{INK}" stroke-width="1.1" stroke-dasharray="4 4" opacity="0.7"/>'
        f'<text x="{W2 - R2 + 6}" y="{Y2(1.00) + 4:.1f}" font-size="9.8" fill="{INK}">1.00 &#8212; '
        f'cash pays</text>')

    marks = ""
    for mo, v, lab, dy in ((0.7001, 1.2000, "0.7001 mo &#8212; covenant breached", -12),
                           (2.5834, 1.0000, "2.5834 mo &#8212; cash no longer pays", -12),
                           (7.2917, 0.5000, "7.2917 mo &#8212; cash + DSRA exhausted", -12)):
        marks += (f'<circle cx="{X2(mo):.1f}" cy="{Y2(v):.1f}" r="4" fill="{CRIMSON}"/>'
                  f'<text x="{X2(mo) + 8:.1f}" y="{Y2(v) + dy:.1f}" font-size="9.8" '
                  f'fill="{INK}">{lab}</text>')
    marks += (f'<circle cx="{X2(8.0):.1f}" cy="{Y2(1.3829):.1f}" r="4" fill="{BLUE}"/>'
              f'<text x="{X2(8.0) - 6:.1f}" y="{Y2(1.3829) - 10:.1f}" font-size="9.8" '
              f'fill="{BLUE}" text-anchor="end">damages cap binds at 8 months</text>'
              f'<circle cx="{X2(9.7226):.1f}" cy="{Y2(1.20):.1f}" r="4" fill="{BLUE}"/>'
              f'<text x="{X2(9.7226) + 7:.1f}" y="{Y2(1.20) + 15:.1f}" font-size="9.8" '
              f'fill="{BLUE}">9.7226 mo</text>')

    lab2 = (f'<text x="{X2(6.4):.1f}" y="{Y2(1.3558) - 12:.1f}" font-size="10.6" fill="{BLUE}" '
            f'font-weight="600">with delay damages credited to CFADS</text>'
            f'<text x="{X2(4.3):.1f}" y="{Y2(0.80) + 26:.1f}" font-size="10.6" fill="{INK}" '
            f'font-weight="600">operating cash only</text>')

    t2 = (f'<text x="{L2 - 52}" y="30" font-size="13.2" fill="{INK}" font-weight="700">Coverage at '
          f'a calendar-fixed first repayment date</text>'
          f'<text x="{L2 - 52}" y="48" font-size="10.4" fill="{SLATE}">Kestrel: CFADS 6,384,000 a '
          f'year, debt service 5,009,635.23, DSRA 2,504,818, damages 20,000 a day capped at '
          f'4,800,000</text>')

    body2 = (grid2 + thresholds
             + axes(L2, T2, W2 - R2, H2 - B2, "Months of slip in the commercial operations date",
                    "DSCR at the first test")
             + xt2
             + f'<polyline points="{p_ld}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
             + f'<polyline points="{p_cash}" fill="none" stroke="{INK}" stroke-width="2.6"/>'
             + marks + lab2 + t2)
    (PFL / "fig_14_4_1.svg").write_text(svg(W2, H2, body2))
