"""PFL-AI Domain 2 — accounting and financial-statement foundations (depth extension).
PCI original artwork.

Fig 2.3.1  Kestrel's DSCR as a straight function of the collection period, showing that
           the 1.25x distribution condition is reached after only 3.7095 further days of
           days-sales-outstanding and the 1.20x financial covenant after 11.3283, so that
           a coverage covenant is in substance a collections covenant.
Fig 2.4.1  The year-one return ladder: after-tax cost of debt 4.8000, after-tax ROCE on
           opening capital 6.8000, WACC 7.9860 (Domain 9), ROE on opening equity 11.4667,
           equity IRR 12.5311 and cost of equity 15.42 — showing the leverage wedge that
           the identity ROE = ROCE_at + (D/E)(ROCE_at - kd_at) produces exactly, and the
           1.1860 points by which a first-year accounting return sits below WACC on a
           project whose NPV is +16,179,360.

Deterministic: every annotated value is a literal verified in the domain's decimal
arithmetic pass (see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 2.3.1 — the covenant as a collection period -------------------------
    # DSCR(dso) = (6,984,000 - (12,000,000 * dso/365 - 300,000)) / 5,009,635.23
    CF_PRE, REV, PAY, DS = 6984000.0, 12000000.0, 300000.0, 5009635.23

    def dscr(dso):
        return (CF_PRE - (REV * dso / 365.0 - PAY)) / DS

    W, H = 800, 470
    L, R, T, B = 92, 214, 104, 64
    x0, x1 = 20.0, 50.0            # days sales outstanding
    y0, y1 = 1.10, 1.35            # DSCR

    def X(d):
        return L + (d - x0) / (x1 - x0) * (W - L - R)

    def Y(v):
        return (H - B) - (v - y0) / (y1 - y0) * (H - B - T)

    grid = "".join(
        f'<line x1="{X(v):.1f}" y1="{T}" x2="{X(v):.1f}" y2="{H - B}" stroke="{GRID}"/>'
        f'<text x="{X(v):.1f}" y="{H - B + 17}" font-size="9.8" fill="{SLATE}" '
        f'text-anchor="middle">{v}</text>'
        for v in (20, 25, 30, 35, 40, 45, 50))
    grid += "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W - R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 3.5:.1f}" font-size="9.8" fill="{SLATE}" '
        f'text-anchor="end">{v:.2f}</text>'
        for v in (1.10, 1.15, 1.20, 1.25, 1.30, 1.35))

    # crimson band below the 1.20 covenant
    band = (f'<rect x="{L}" y="{Y(1.20):.1f}" width="{W - R - L:.1f}" '
            f'height="{(H - B) - Y(1.20):.1f}" fill="{CRIMSON}" opacity="0.08"/>')

    line = (f'<line x1="{X(x0):.1f}" y1="{Y(dscr(x0)):.1f}" x2="{X(x1):.1f}" '
            f'y2="{Y(dscr(x1)):.1f}" stroke="{BLUE}" stroke-width="2.4"/>')

    refs = ""
    for lvl, lab in ((1.25, "1.25&#215; distribution condition"),
                     (1.20, "1.20&#215; financial covenant")):
        refs += (f'<line x1="{L}" y1="{Y(lvl):.1f}" x2="{W - R}" y2="{Y(lvl):.1f}" '
                 f'stroke="{SLATE}" stroke-width="1.4" stroke-dasharray="5 4"/>'
                 f'<text x="{W - R + 8}" y="{Y(lvl) + 3.5:.1f}" font-size="9.8" '
                 f'font-weight="600" fill="{SLATE}">{lab}</text>')

    pts = [(27.3750, 1.2743, INK, "actual 27.3750 days &#183; 1.2743", True),
           (31.0845, 1.2500, BLUE, "31.0845 days &#183; dividend stops", True),
           (38.7033, 1.2000, CRIMSON, "38.7033 days &#183; covenant breached", True),
           (45.0000, 1.1587, CRIMSON, "45 days &#183; 1.1587, in lock-up", False)]
    marks = ""
    for d, v, col, lab, filled in pts:
        marks += (f'<circle cx="{X(d):.1f}" cy="{Y(v):.1f}" r="5.4" '
                  f'fill="{col if filled else "#ffffff"}" stroke="{col}" stroke-width="2"/>')
        marks += (f'<text x="{X(d) + 9:.1f}" y="{Y(v) - 8:.1f}" font-size="9.6" '
                  f'font-weight="700" fill="{col}">{lab}</text>')

    # the headroom bracket, drawn inside the plot above the line
    by = Y(1.328)
    bracket = (f'<line x1="{X(27.3750):.1f}" y1="{by:.1f}" x2="{X(31.0845):.1f}" '
               f'y2="{by:.1f}" stroke="{CRIMSON}" stroke-width="2"/>'
               f'<line x1="{X(27.3750):.1f}" y1="{by - 5:.1f}" x2="{X(27.3750):.1f}" '
               f'y2="{by + 5:.1f}" stroke="{CRIMSON}" stroke-width="2"/>'
               f'<line x1="{X(31.0845):.1f}" y1="{by - 5:.1f}" x2="{X(31.0845):.1f}" '
               f'y2="{by + 5:.1f}" stroke="{CRIMSON}" stroke-width="2"/>'
               f'<text x="{X(31.0845) + 10:.1f}" y="{by + 4:.1f}" font-size="10.2" '
               f'font-weight="700" fill="{CRIMSON}">3.7095 days &#8212; the whole dividend</text>')

    body = (grid + band + refs + line + marks + bracket
            + axes(L, T, W - R, H - B,
                   "Days sales outstanding &#8212; receivables &#247; revenue &#215; 365",
                   "DSCR on the documented CFADS definition")
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'A coverage covenant is a collections covenant</text>'
            + f'<text x="24" y="45" font-size="10.4" fill="{SLATE}">'
              'CFADS before working capital 6,984,000 &#183; payables 300,000 &#183; debt '
              'service 5,009,635.23 &#183; revenue 12,000,000</text>'
            + f'<text x="24" y="61" font-size="10.4" fill="{BLUE}">'
              'One day of DSO is 32,876.71 of cash and 0.006563 of DSCR; the line is straight '
              'because debt service is fixed</text>'
            + f'<text x="24" y="77" font-size="10.4" fill="{CRIMSON}">'
              '50,096.35 of working-capital absorption costs exactly 0.01 of coverage</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 14}" font-size="11" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'Headroom: 3.7095 days to the distribution condition, 11.3283 to the covenant, '
              '18.9471 to the lock-up</text>')
    (PFL / "fig_2_3_1.svg").write_text(svg(W, H, body))

    # ---- Fig 2.4.1 — the year-one return ladder ---------------------------------
    # (label, per cent, colour, dashed outline, note)
    bars = [
        ("After-tax cost of debt", 4.8000, SLATE, False, "6.0 % &#215; (1 &#8722; 0.20)"),
        ("After-tax ROCE, opening capital", 6.8000, BLUE, False, "4,080,000 &#247; 60,000,000"),
        ("WACC (Domain 9, KA 9.1.4)", 7.9860, SLATE, True, "the external benchmark"),
        ("ROE, opening equity", 11.4667, BLUE, False, "2,064,000 &#247; 18,000,000"),
        ("Equity IRR over 12 years", 12.5311, INK, False, "Domain 9, at 70 % gearing"),
        ("Cost of equity at 70 % gearing", 15.4200, CRIMSON, False, "Domain 9, re-levered"),
    ]
    W, H = 800, 462
    L, R, T, B = 268, 152, 106, 62
    plot_w, plot_h = W - L - R, H - T - B
    hi = 16.0
    row_h = plot_h / len(bars)

    def Xp(pct):
        return L + pct / hi * plot_w

    grid = "".join(
        f'<line x1="{Xp(v):.1f}" y1="{T}" x2="{Xp(v):.1f}" y2="{H - B}" stroke="{GRID}"/>'
        f'<text x="{Xp(v):.1f}" y="{H - B + 17}" font-size="9.8" fill="{SLATE}" '
        f'text-anchor="middle">{v} %</text>'
        for v in (0, 2, 4, 6, 8, 10, 12, 14, 16))

    rows = ""
    ys = {}
    for i, (lab, pct, col, dashed, note) in enumerate(bars):
        y = T + i * row_h + row_h * 0.24
        h = row_h * 0.46
        ys[lab] = (y, h)
        if dashed:
            rows += (f'<rect x="{L}" y="{y:.1f}" width="{Xp(pct) - L:.1f}" height="{h:.1f}" '
                     f'fill="none" stroke="{col}" stroke-width="1.8" stroke-dasharray="6 4"/>')
        else:
            rows += (f'<rect x="{L}" y="{y:.1f}" width="{Xp(pct) - L:.1f}" height="{h:.1f}" '
                     f'fill="{col}" opacity="0.88"/>')
        rows += (f'<text x="{L - 14}" y="{y + h * 0.50:.1f}" font-size="10.4" fill="{INK}" '
                 f'text-anchor="end" font-weight="600">{lab}</text>')
        rows += (f'<text x="{L - 14}" y="{y + h * 0.50 + 12.5:.1f}" font-size="9.2" '
                 f'fill="{SLATE}" text-anchor="end">{note}</text>')
        rows += (f'<text x="{Xp(pct) + 8:.1f}" y="{y + h * 0.62:.1f}" font-size="10.4" '
                 f'font-weight="700" fill="{col}">{pct:.4f} %</text>')

    # leverage wedge bracket, from after-tax ROCE to ROE on opening equity
    ry, rh = ys["After-tax ROCE, opening capital"]
    ey, eh = ys["ROE, opening equity"]
    wx0, wx1 = Xp(6.8000), Xp(11.4667)
    wy = ey + eh + 8
    wedge = (f'<line x1="{wx0:.1f}" y1="{wy:.1f}" x2="{wx1:.1f}" y2="{wy:.1f}" '
             f'stroke="{BLUE}" stroke-width="2"/>'
             f'<line x1="{wx0:.1f}" y1="{wy - 5:.1f}" x2="{wx0:.1f}" y2="{wy + 5:.1f}" '
             f'stroke="{BLUE}" stroke-width="2"/>'
             f'<line x1="{wx1:.1f}" y1="{wy - 5:.1f}" x2="{wx1:.1f}" y2="{wy + 5:.1f}" '
             f'stroke="{BLUE}" stroke-width="2"/>'
             f'<text x="{(wx0 + wx1) / 2:.1f}" y="{wy + 16:.1f}" font-size="9.6" '
             f'font-weight="700" fill="{BLUE}" text-anchor="middle">'
             'the leverage wedge: 2.3333 &#215; 2.0000 = 4.6667 points</text>')

    # the WACC shortfall marker, shaded on the ROCE row
    gx0, gx1 = Xp(6.8000), Xp(7.9860)
    gap = (f'<rect x="{gx0:.1f}" y="{ry:.1f}" width="{gx1 - gx0:.1f}" height="{rh:.1f}" '
           f'fill="{CRIMSON}" opacity="0.25"/>'
           f'<text x="{gx1 + 4:.1f}" y="{ry - 7:.1f}" font-size="9.6" font-weight="700" '
           f'fill="{CRIMSON}">&#8722;1.1860 points against WACC</text>')

    body = (grid + rows + gap + wedge
            + axes(L, T, W - R, H - B, "Per cent, year one of operations", "")
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'Six returns, one project, one year &#8212; and none of them is the project&#8217;s '
              'return</text>'
            + f'<text x="24" y="45" font-size="10.4" fill="{BLUE}">'
              'The identity is exact on opening bases: 6.8000 + 2.3333 &#215; '
              '(6.8000 &#8722; 4.8000) = 11.4667</text>'
            + f'<text x="24" y="61" font-size="10.4" fill="{SLATE}">'
              'On closing equity the same profit gives 10.2871 %, on average equity 10.8449 % '
              '&#8212; 1.1796 points from the denominator alone</text>'
            + f'<text x="24" y="77" font-size="10.4" fill="{CRIMSON}">'
              'Year-one accounting return sits below WACC on a project whose NPV is '
              '+16,179,360 and IRR 12.19 %</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 14}" font-size="11" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'ROE of 11.4667 % is neither the 12.5311 % equity earns nor the 15.42 % it '
              'requires</text>')
    (PFL / "fig_2_4_1.svg").write_text(svg(W, H, body))
