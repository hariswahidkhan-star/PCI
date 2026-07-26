"""PFL-AI Domain 1 — Foundations of project finance leadership. PCI original artwork.

Fig 1.1.3  The recourse decision against facility size: the breakeven probability of a
           parent-impairing failure at which the limited-recourse route repays its own
           incremental cost, falling 93.7893 % at 15,000,000 of debt through 51.6392 % at
           Kestrel's 42,000,000 to an asymptote of 28.2224 %, and crossing 100 % — where the
           route cannot pay at any probability — at 13,702,087.
Fig 1.2.2  Equity return against project cash for one project at three debt shapes: all
           equity, 70 % interest-only and 70 % amortising over twelve years, with the
           crossover at 6,000,000, the two cliffs at 4,200,000 and 8,349,392, and the 1.20x
           covenant at 10,019,270.

Deterministic: every value below is a literal verified in the domain's arithmetic pass
(decimal.Decimal at 28 digits; see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 1.1.3 — breakeven failure probability against facility size ----------
    # p*(D) = (2,359,000 + D x 0.06769352751566043) / (D x 0.2398570778571429)
    FIXED = 2359000.0
    VAR = 0.06769352751566043
    EXP = 0.2398570778571429
    ASY = VAR / EXP * 100.0                      # 28.2224 %
    CROSS100 = 13702087.32                       # p* = 100 %
    marks = [(15e6, 93.7893), (25e6, 67.5625), (42e6, 51.6392),
             (75e6, 41.3358), (150e6, 34.7791), (300e6, 31.5008)]

    W, H = 700, 430
    L, R, T, B = 84, 176, 40, 62
    pw, ph = W - L - R, H - T - B
    dmin, dmax = 10e6, 310e6
    ymin, ymax = 20.0, 110.0

    def X(d):
        return L + (d - dmin) / (dmax - dmin) * pw

    def Y(p):
        return H - B - (p - ymin) / (ymax - ymin) * ph

    def pstar(d):
        return (FIXED + d * VAR) / (d * EXP) * 100.0

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W - R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v:.0f} %</text>'
        for v in (20, 30, 40, 50, 60, 70, 80, 90, 100, 110))
    xt = [15e6, 50e6, 100e6, 150e6, 200e6, 250e6, 300e6]
    xticks = "".join(
        f'<line x1="{X(d):.1f}" y1="{H - B}" x2="{X(d):.1f}" y2="{H - B + 4}" stroke="{INK}"/>'
        f'<text x="{X(d):.1f}" y="{H - B + 18:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{d / 1e6:.0f}m</text>'
        for d in xt)

    steps = 240
    pts = []
    for k in range(steps + 1):
        d = dmin + (dmax - dmin) * k / steps
        p = pstar(d)
        if p <= ymax:
            pts.append(f"{X(d):.1f},{Y(p):.1f}")
    curve = f'<polyline points="{" ".join(pts)}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'

    ceiling = (f'<line x1="{L}" y1="{Y(100):.1f}" x2="{W - R}" y2="{Y(100):.1f}" '
               f'stroke="{CRIMSON}" stroke-width="1.2" stroke-dasharray="5 4"/>'
               f'<text x="{L + 6}" y="{Y(100) - 7:.1f}" font-size="10.5" fill="{CRIMSON}">'
               f'100 % &#8212; cannot pay at any probability</text>')
    asym = (f'<line x1="{L}" y1="{Y(ASY):.1f}" x2="{W - R}" y2="{Y(ASY):.1f}" '
            f'stroke="{SLATE}" stroke-width="1" stroke-dasharray="3 4"/>'
            f'<text x="{W - R - 4}" y="{Y(ASY) - 6:.1f}" font-size="10.5" fill="{SLATE}" '
            f'text-anchor="end">asymptote 28.2224 % (margin differential alone)</text>')
    xcross = (f'<line x1="{X(CROSS100):.1f}" y1="{Y(100):.1f}" x2="{X(CROSS100):.1f}" '
              f'y2="{H - B}" stroke="{CRIMSON}" stroke-width="1" stroke-dasharray="3 3"/>'
              f'<text x="{X(CROSS100) + 5:.1f}" y="{H - B - 6:.1f}" font-size="10" '
              f'fill="{CRIMSON}">13,702,087</text>')

    dots = ""
    for d, p in marks:
        if p > ymax:
            continue
        dots += f'<circle cx="{X(d):.1f}" cy="{Y(p):.1f}" r="3.6" fill="{CRIMSON}"/>'
    kx, ky = X(42e6), Y(51.6392)
    dots += (f'<circle cx="{kx:.1f}" cy="{ky:.1f}" r="5.4" fill="none" stroke="{CRIMSON}" '
             f'stroke-width="1.6"/>'
             f'<text x="{kx + 11:.1f}" y="{ky - 9:.1f}" font-size="11" fill="{INK}" '
             f'font-weight="600">Kestrel 42,000,000: 51.6392 %</text>')

    LEG = W - R + 14
    legend = (f'<text x="{LEG}" y="{T + 14}" font-size="11" fill="{INK}" font-weight="700">'
              f'Breakeven p*</text>')
    for k, (d, p) in enumerate(marks):
        legend += (f'<text x="{LEG}" y="{T + 34 + k * 17}" font-size="10.5" fill="{SLATE}">'
                   f'{d / 1e6:.0f}m &#8594; {p:.2f} %</text>')
    legend += (f'<text x="{LEG}" y="{T + 34 + len(marks) * 17 + 14}" font-size="10.5" '
               f'fill="{INK}">Fixed close-cost</text>'
               f'<text x="{LEG}" y="{T + 34 + len(marks) * 17 + 29}" font-size="10.5" '
               f'fill="{INK}">premium 2,359,000</text>'
               f'<text x="{LEG}" y="{T + 34 + len(marks) * 17 + 48}" font-size="10.5" '
               f'fill="{SLATE}">Margin differential</text>'
               f'<text x="{LEG}" y="{T + 34 + len(marks) * 17 + 63}" font-size="10.5" '
               f'fill="{SLATE}">140 bp, PV 6.7694 %</text>'
               f'<text x="{LEG}" y="{T + 34 + len(marks) * 17 + 78}" font-size="10.5" '
               f'fill="{SLATE}">of debt</text>')

    body = (grid + curve + ceiling + asym + xcross + dots
            + axes(L, T, W - R, H - B, "Senior debt raised (USD)",
                   "Breakeven probability of a parent-impairing failure")
            + xticks + legend)
    (PFL / "fig_1_1_3.svg").write_text(svg(W, H, body))

    # ---- Fig 1.2.2 — equity return against project cash, three debt shapes -------
    AMORT = 8349392.06
    IO = 4200000.0
    W, H = 700, 430
    L, R, T, B = 84, 168, 40, 62
    pw, ph = W - L - R, H - T - B
    cmin, cmax = 3.0e6, 14.0e6
    ymin, ymax = -20.0, 35.0

    def Xc(c):
        return L + (c - cmin) / (cmax - cmin) * pw

    def Yr(v):
        return H - B - (v - ymin) / (ymax - ymin) * ph

    grid = "".join(
        f'<line x1="{L}" y1="{Yr(v):.1f}" x2="{W - R}" y2="{Yr(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Yr(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v:.0f} %</text>'
        for v in (-20, -10, 0, 10, 20, 30))
    xticks = "".join(
        f'<line x1="{Xc(c):.1f}" y1="{H - B}" x2="{Xc(c):.1f}" y2="{H - B + 4}" stroke="{INK}"/>'
        f'<text x="{Xc(c):.1f}" y="{H - B + 18:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{c / 1e6:.0f}m</text>'
        for c in (4e6, 6e6, 8e6, 10e6, 12e6, 14e6))
    zero = (f'<line x1="{L}" y1="{Yr(0):.1f}" x2="{W - R}" y2="{Yr(0):.1f}" stroke="{INK}" '
            f'stroke-width="1"/>')

    def line(fn, colour, width, dash=""):
        pts = " ".join(f"{Xc(cmin + (cmax - cmin) * k / 120):.1f},"
                       f"{Yr(fn(cmin + (cmax - cmin) * k / 120)):.1f}" for k in range(121))
        d = f' stroke-dasharray="{dash}"' if dash else ""
        return (f'<polyline points="{pts}" fill="none" stroke="{colour}" '
                f'stroke-width="{width}"{d}/>')

    unlev = line(lambda c: c / 100e6 * 100.0, SLATE, 2.2, "6 4")
    io = line(lambda c: (c - IO) / 30e6 * 100.0, BLUE, 2.6)
    am = line(lambda c: (c - AMORT) / 30e6 * 100.0, CRIMSON, 2.6)

    cross = (f'<circle cx="{Xc(6e6):.1f}" cy="{Yr(6.0):.1f}" r="4" fill="{INK}"/>'
             f'<text x="{Xc(6e6) - 6:.1f}" y="{Yr(6.0) - 10:.1f}" font-size="10.5" fill="{INK}" '
             f'text-anchor="end">crossover 6,000,000: both 6.0 %</text>')
    cliffs = (f'<line x1="{Xc(IO):.1f}" y1="{Yr(0):.1f}" x2="{Xc(IO):.1f}" y2="{H - B}" '
              f'stroke="{BLUE}" stroke-width="1" stroke-dasharray="3 3"/>'
              f'<text x="{Xc(IO) + 4:.1f}" y="{H - B - 8:.1f}" font-size="10" fill="{BLUE}">'
              f'4,200,000 (&#8722;65.00 %)</text>'
              f'<line x1="{Xc(AMORT):.1f}" y1="{Yr(0):.1f}" x2="{Xc(AMORT):.1f}" y2="{H - B}" '
              f'stroke="{CRIMSON}" stroke-width="1" stroke-dasharray="3 3"/>'
              f'<text x="{Xc(AMORT) + 4:.1f}" y="{H - B - 24:.1f}" font-size="10" '
              f'fill="{CRIMSON}">8,349,392 (&#8722;30.4217 %)</text>')
    cov = (f'<line x1="{Xc(10019270.47):.1f}" y1="{T}" x2="{Xc(10019270.47):.1f}" '
           f'y2="{H - B}" stroke="{INK}" stroke-width="1" stroke-dasharray="5 4" opacity="0.6"/>'
           f'<text x="{Xc(10019270.47):.1f}" y="{T - 8:.1f}" font-size="10.5" fill="{INK}" '
           f'text-anchor="middle">1.20&#215; covenant 10,019,270</text>')
    base = (f'<circle cx="{Xc(12e6):.1f}" cy="{Yr(26.0):.1f}" r="4" fill="{BLUE}"/>'
            f'<circle cx="{Xc(12e6):.1f}" cy="{Yr(12.1687):.1f}" r="4" fill="{CRIMSON}"/>'
            f'<circle cx="{Xc(12e6):.1f}" cy="{Yr(12.0):.1f}" r="3.2" fill="{SLATE}"/>')

    LEG = W - R + 12
    legend = (f'<text x="{LEG}" y="{T + 14}" font-size="11" fill="{INK}" font-weight="700">'
              f'At 12,000,000</text>'
              f'<rect x="{LEG}" y="{T + 26}" width="11" height="3" fill="{BLUE}"/>'
              f'<text x="{LEG + 17}" y="{T + 32}" font-size="10.5" fill="{INK}">'
              f'70 % interest-only</text>'
              f'<text x="{LEG + 17}" y="{T + 47}" font-size="10.5" fill="{BLUE}" '
              f'font-weight="600">26.0000 %</text>'
              f'<rect x="{LEG}" y="{T + 62}" width="11" height="3" fill="{CRIMSON}"/>'
              f'<text x="{LEG + 17}" y="{T + 68}" font-size="10.5" fill="{INK}">'
              f'70 % amortising</text>'
              f'<text x="{LEG + 17}" y="{T + 83}" font-size="10.5" fill="{CRIMSON}" '
              f'font-weight="600">12.1687 %</text>'
              f'<rect x="{LEG}" y="{T + 98}" width="11" height="3" fill="{SLATE}"/>'
              f'<text x="{LEG + 17}" y="{T + 104}" font-size="10.5" fill="{INK}">All equity</text>'
              f'<text x="{LEG + 17}" y="{T + 119}" font-size="10.5" fill="{SLATE}" '
              f'font-weight="600">12.0000 %</text>'
              f'<text x="{LEG}" y="{T + 146}" font-size="10.5" fill="{SLATE}">Same 70,000,000</text>'
              f'<text x="{LEG}" y="{T + 161}" font-size="10.5" fill="{SLATE}">at 6.0 %; year-one</text>'
              f'<text x="{LEG}" y="{T + 176}" font-size="10.5" fill="{SLATE}">interest identical</text>'
              f'<text x="{LEG}" y="{T + 191}" font-size="10.5" fill="{SLATE}">at 4,200,000</text>')

    body = (grid + zero + unlev + io + am + cov + cliffs + cross + base
            + axes(L, T, W - R, H - B, "Project operating cash (USD per year)",
                   "Equity cash return on 30,000,000")
            + xticks + legend)
    (PFL / "fig_1_2_2.svg").write_text(svg(W, H, body))
