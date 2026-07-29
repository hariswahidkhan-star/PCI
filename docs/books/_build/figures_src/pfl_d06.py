"""PFL-AI Domain 6 — Financial modelling and model governance. PCI original artwork.

Fig 6.2.1  Construction funding and interest during construction: quarterly spend and
           capitalised interest, with cumulative debt and equity drawn, closing on
           42,000,000 / 18,000,000 exactly.
Fig 6.4.1  The minimum coverage a level line hides: DSCR by loan year on three cases
           (level illusion, modelled bank case, sponsor case with escalation) against
           the 1.20x covenant.

Deterministic: every value below is a literal verified in the domain's arithmetic pass
(decimal.Decimal throughout; see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 6.2.1 — construction funding and IDC ---------------------------------
    # (period label, spend, capitalised interest, cumulative debt, cumulative equity)
    q = [
        ("close", 2640000, 0, 1848000, 792000),
        ("q1", 3314724, 27720, 4187711, 1794733),
        ("q2", 4972086, 62816, 7712142, 3305204),
        ("q3", 7181902, 115682, 12820451, 5494479),
        ("q4", 8839264, 192307, 19142551, 8203951),
        ("q5", 9391718, 287138, 25917751, 11107608),
        ("q6", 8286810, 388766, 31990655, 13710281),
        ("q7", 7181902, 479860, 37353888, 16008809),
        ("q8", 6076994, 560308, 42000000, 18000000),
    ]
    W, H = 700, 460
    L, R, T, B = 78, 128, 74, 76
    n = len(q)
    plot_w = W - L - R
    ymax = 10000000.0
    cmax = 42000000.0

    def X(i):
        return L + (i + 0.5) * plot_w / n

    def Y(v):
        return H - B - v / ymax * (H - T - B)

    def Yc(v):
        return H - B - v / cmax * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W - R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v / 1000000:.0f}m</text>'
        for v in range(0, 11000000, 2000000))
    barw = plot_w / n * 0.50
    bars = ""
    for i, (lab, sp, it, cd, ce) in enumerate(q):
        x = X(i) - barw / 2
        bars += (f'<rect x="{x:.1f}" y="{Y(sp):.1f}" width="{barw:.1f}" '
                 f'height="{(H - B) - Y(sp):.1f}" fill="{BLUE}" opacity="0.55"/>')
        if it:
            bars += (f'<rect x="{x:.1f}" y="{Y(sp + it):.1f}" width="{barw:.1f}" '
                     f'height="{Y(sp) - Y(sp + it):.1f}" fill="{CRIMSON}" opacity="0.9"/>')
        bars += (f'<text x="{X(i):.1f}" y="{H - B + 16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{lab}</text>')
    debt_line = " ".join(f"{X(i):.1f},{Yc(r[3]):.1f}" for i, r in enumerate(q))
    eq_line = " ".join(f"{X(i):.1f},{Yc(r[4]):.1f}" for i, r in enumerate(q))
    body = (grid + bars + axes(L, T, W - R, H - B, "Construction period (quarters)",
                               "Funding requirement (USD)")
            + f'<polyline points="{debt_line}" fill="none" stroke="{INK}" stroke-width="2.6"/>'
            + f'<polyline points="{eq_line}" fill="none" stroke="{SLATE}" stroke-width="2.2" '
              f'stroke-dasharray="6 4"/>'
            + "".join(f'<circle cx="{X(i):.1f}" cy="{Yc(r[3]):.1f}" r="3.0" fill="{INK}"/>'
                      for i, r in enumerate(q))
            + f'<text x="{W - R + 8}" y="{Yc(42000000) + 4:.1f}" font-size="10.6" '
              f'font-weight="700" fill="{INK}">debt drawn</text>'
            + f'<text x="{W - R + 8}" y="{Yc(42000000) + 18:.1f}" font-size="10.6" '
              f'font-weight="700" fill="{INK}">42,000,000</text>'
            + f'<text x="{W - R + 8}" y="{Yc(18000000) + 4:.1f}" font-size="10.6" '
              f'fill="{SLATE}">equity drawn</text>'
            + f'<text x="{W - R + 8}" y="{Yc(18000000) + 18:.1f}" font-size="10.6" '
              f'fill="{SLATE}">18,000,000</text>'
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'Every use is funded 70/30, and capitalised interest is a use like any other'
              '</text>'
            + f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
              'Blue: certified construction spend. Crimson: interest during construction, '
              '2,114,597 in total &#8212; 3.52 % of the 60,000,000 envelope</text>'
            + f'<text x="24" y="59" font-size="10.4" fill="{CRIMSON}">'
              'The same profile on an annual timeline with opening-balance interest returns '
              '1,247,352 &#8212; an understatement of 867,245, or 41.01 %</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 16}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'Sources 60,000,000 = uses 60,000,000, and the balancing line is the '
              'contingency: 3,645,403, or 7.59 % of the EPC price</text>')
    (PFL / "fig_6_2_1.svg").write_text(svg(W, H, body))

    # ---- Fig 6.4.1 — the minimum the level line hides ------------------------------
    bank = ["1.2743", "1.2684", "1.2621", "1.2554", "1.2483", "1.2407",
            "1.2327", "1.2243", "1.2153", "1.2058", "1.1957", "1.1851"]
    spon = ["1.2743", "1.3004", "1.3270", "1.3542", "1.3820", "1.4104",
            "1.4394", "1.4691", "1.4994", "1.5303", "1.5618", "1.5940"]
    W, H = 700, 440
    L, R, T, B = 82, 150, 70, 74
    lo, hi = 1.10, 1.65
    plot_w = W - L - R

    def Xy(t):
        return L + (t - 1) / 11.0 * plot_w

    def Yr(v):
        return H - B - (v - lo) / (hi - lo) * (H - T - B)

    ticks = [1.10, 1.20, 1.30, 1.40, 1.50, 1.60]
    grid = "".join(
        f'<line x1="{L}" y1="{Yr(v):.1f}" x2="{W - R}" y2="{Yr(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Yr(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v:.2f}</text>' for v in ticks)
    xt = "".join(f'<text x="{Xy(t):.1f}" y="{H - B + 16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{t}</text>' for t in range(1, 13))
    pb = " ".join(f"{Xy(t + 1):.1f},{Yr(float(v)):.1f}" for t, v in enumerate(bank))
    ps = " ".join(f"{Xy(t + 1):.1f},{Yr(float(v)):.1f}" for t, v in enumerate(spon))
    body = (grid + xt + axes(L, T, W - R, H - B, "Loan year", "DSCR")
            + f'<line x1="{Xy(1):.1f}" y1="{Yr(1.2743):.1f}" x2="{Xy(12):.1f}" '
              f'y2="{Yr(1.2743):.1f}" stroke="{SLATE}" stroke-width="2.2" '
              f'stroke-dasharray="7 5"/>'
            + f'<polyline points="{ps}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
            + f'<polyline points="{pb}" fill="none" stroke="{INK}" stroke-width="2.8"/>'
            + f'<line x1="{L}" y1="{Yr(1.20):.1f}" x2="{W - R}" y2="{Yr(1.20):.1f}" '
              f'stroke="{CRIMSON}" stroke-width="1.6" stroke-dasharray="5 4"/>'
            + f'<text x="{L + 6}" y="{Yr(1.20) - 7:.1f}" font-size="10.6" font-weight="700" '
              f'fill="{CRIMSON}">1.20&#215; covenant</text>'
            + f'<circle cx="{Xy(12):.1f}" cy="{Yr(1.1851):.1f}" r="4.4" fill="{CRIMSON}"/>'
            + f'<text x="{Xy(12) - 6:.1f}" y="{Yr(1.1851) + 20:.1f}" font-size="10.8" '
              f'font-weight="700" fill="{CRIMSON}" text-anchor="end">1.1851 &#8212; breach</text>'
            + f'<text x="{W - R + 8}" y="{Yr(1.5940) + 4:.1f}" font-size="10.4" '
              f'font-weight="700" fill="{BLUE}">sponsor case</text>'
            + f'<text x="{W - R + 8}" y="{Yr(1.5940) + 18:.1f}" font-size="10" '
              f'fill="{BLUE}">2.967 % escalation</text>'
            + f'<text x="{W - R + 8}" y="{Yr(1.2743) - 4:.1f}" font-size="10.4" '
              f'fill="{SLATE}">level illusion</text>'
            + f'<text x="{W - R + 8}" y="{Yr(1.2743) + 10:.1f}" font-size="10" '
              f'fill="{SLATE}">1.2743 for 12 years</text>'
            + f'<text x="{W - R + 8}" y="{Yr(1.1851) - 10:.1f}" font-size="10.4" '
              f'font-weight="700" fill="{INK}">bank case</text>'
            + f'<text x="{W - R + 8}" y="{Yr(1.1851) + 4:.1f}" font-size="10" fill="{INK}">'
              'flat revenue,</text>'
            + f'<text x="{W - R + 8}" y="{Yr(1.1851) + 17:.1f}" font-size="10" fill="{INK}">'
              'tax on the schedule</text>'
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'Coverage falls as the interest deduction falls &#8212; and a level CFADS line '
              'cannot show it</text>'
            + f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
              'All three cases start at 1.2743. The escalation assumption decides whether the '
              'minimum is in year 1 or year 12</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 14}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'Average DSCR 1.2340 on the bank case &#8212; and the average is not the number '
              'the covenant tests</text>')
    (PFL / "fig_6_4_1.svg").write_text(svg(W, H, body))
