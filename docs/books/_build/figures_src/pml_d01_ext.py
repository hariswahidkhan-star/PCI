"""PML-AI Domain 1 — extension figures (Fig 1.2.1, 1.3.2, 1.3.3, 1.4.1).

Fig 1.3.1 is rendered by make_figures.py itself; this module adds only the figures introduced by the
depth expansion of Domain 1. PCI original artwork; deterministic output.
"""


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ---- Fig 1.2.1 the cost of a silent measure ----------------------------------
    # crimson: flat to week 13, +6,120/wk to week 39, +408/wk to week 52 -> 164,424
    # blue:    flat to week 13, +408/wk to week 52 -> 15,912
    # gap at week 52 = 148,512; gap reaches 132,000 at week 13 + 132,000/5,712 = 36.11
    W, H, L, R, T, B = 660, 400, 92, 26, 30, 58
    YMAX = 180000

    def X(w):
        return L + w / 52 * (W - L - R)

    def Y(v):
        return H - B - v / YMAX * (H - T - B)

    def crim(w):
        if w <= 13:
            return 0.0
        if w <= 39:
            return 6120.0 * (w - 13)
        return 159120.0 + 408.0 * (w - 39)

    def blu(w):
        return 0.0 if w <= 13 else 408.0 * (w - 13)

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v)}" x2="{W-R}" y2="{Y(v)}" stroke="{GRID}"/>'
        f'<text x="{L-8}" y="{Y(v)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v//1000}k</text>'
        for v in range(0, YMAX + 1, 30000))
    xt = "".join(f'<text x="{X(w)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{w}</text>'
                 for w in range(0, 53, 13))
    cline = " ".join(f"{X(w):.1f},{Y(crim(w)):.1f}" for w in (0, 13, 39, 52))
    bline = " ".join(f"{X(w):.1f},{Y(blu(w)):.1f}" for w in (0, 13, 52))
    body = (grid + axes(L, T, W - R, H - B, "Weeks from go-live", "Cumulative benefit forgone (USD)") + xt
            + f'<line x1="{X(39)}" y1="{T}" x2="{X(39)}" y2="{H-B}" stroke="{INK}" stroke-width="1" stroke-dasharray="4 4"/>'
            + f'<text x="{X(39)-6}" y="{T+14}" font-size="10.5" fill="{INK}" text-anchor="end">external audit &#8212; week 39</text>'
            + f'<line x1="{L}" y1="{Y(132000)}" x2="{W-R}" y2="{Y(132000)}" stroke="{INK}" stroke-width="1" stroke-dasharray="2 3"/>'
            + f'<text x="{L+6}" y="{Y(132000)-6}" font-size="10" fill="{INK}">cost of the remedy 132,000</text>'
            + f'<polyline points="{cline}" fill="none" stroke="{CRIMSON}" stroke-width="2.6"/>'
            + f'<polyline points="{bline}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
            + f'<circle cx="{X(13)}" cy="{Y(0)}" r="3.6" fill="{CRIMSON}"/>'
            + f'<text x="{X(13)+7}" y="{Y(0)-8}" font-size="10" fill="{INK}">week 13: measure shows 16 of 40 clinics</text>'
            + f'<line x1="{X(52)-14}" y1="{Y(164424)}" x2="{X(52)-14}" y2="{Y(15912)}" stroke="{SLATE}" stroke-width="1.4"/>'
            + f'<text x="{X(52)-20}" y="{Y(90000)}" font-size="10.5" font-weight="700" fill="{SLATE}" text-anchor="end">USD 148,512</text>'
            + f'<text x="{X(52)-20}" y="{Y(90000)+13}" font-size="9.5" fill="{SLATE}" text-anchor="end">the value of the timing alone</text>'
            + f'<circle cx="{X(36.11)}" cy="{Y(crim(36.11))}" r="3" fill="{INK}"/>'
            + f'<text x="{X(36.11)-8}" y="{Y(crim(36.11))+16}" font-size="9.5" fill="{INK}" text-anchor="end">gap = remedy cost at week 36.11 (23.11 weeks)</text>'
            + f'<text x="{X(41)}" y="{Y(158000)}" font-size="11" font-weight="600" fill="{CRIMSON}">escalated at the audit</text>'
            + f'<text x="{X(30)}" y="{Y(24000)}" font-size="11" font-weight="600" fill="{BLUE}">escalated when the measure showed</text>')
    (PML / "fig_1_2_1.svg").write_text(svg(W, H, body))

    # ---- Fig 1.3.3 the reinforcement trough and its crossover ---------------------
    # crew of 6: 0.6/wk -> 8 clinics at 13.3333.  crew of 9: 0.525/wk to wk 4 (2.1) then 0.9/wk
    # -> 8 at 10.5556.  Cross at week 5.00; deficit 0.3 clinics (12.5 %) at week 4.
    W, H, L, R, T, B = 660, 400, 88, 26, 30, 58
    WMAX, CMAX = 14, 9

    def Xw(w):
        return L + w / WMAX * (W - L - R)

    def Yc(c):
        return H - B - c / CMAX * (H - T - B)

    def six(w):
        return min(0.6 * w, 8.0)

    def nine(w):
        return min(0.525 * w if w <= 4 else 2.1 + 0.9 * (w - 4), 8.0)

    grid = "".join(
        f'<line x1="{L}" y1="{Yc(c)}" x2="{W-R}" y2="{Yc(c)}" stroke="{GRID}"/>'
        f'<text x="{L-8}" y="{Yc(c)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{c}</text>'
        for c in range(0, CMAX + 1, 1))
    xt = "".join(f'<text x="{Xw(w)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{w}</text>'
                 for w in range(0, WMAX + 1, 2))
    trough = (f'{Xw(0):.1f},{Yc(0):.1f} {Xw(4):.1f},{Yc(2.4):.1f} {Xw(5):.1f},{Yc(3.0):.1f} '
              f'{Xw(4):.1f},{Yc(2.1):.1f}')
    sl = " ".join(f"{Xw(w):.1f},{Yc(six(w)):.1f}" for w in (0, 13.3333, 14))
    nl = " ".join(f"{Xw(w):.1f},{Yc(nine(w)):.1f}" for w in (0, 4, 10.5556, 14))
    body = (grid + axes(L, T, W - R, H - B, "Weeks from the decision", "Cumulative clinics completed") + xt
            + f'<polygon points="{trough}" fill="{CRIMSON}" opacity="0.13"/>'
            + f'<polyline points="{sl}" fill="none" stroke="{SLATE}" stroke-width="2.4" stroke-dasharray="7 4"/>'
            + f'<polyline points="{nl}" fill="none" stroke="{BLUE}" stroke-width="2.8"/>'
            + f'<circle cx="{Xw(5)}" cy="{Yc(3.0)}" r="4" fill="{CRIMSON}"/>'
            + f'<text x="{Xw(5)+8}" y="{Yc(3.0)+2}" font-size="10.5" font-weight="700" fill="{CRIMSON}">crossover week 5.00</text>'
            + f'<text x="{Xw(5)+8}" y="{Yc(3.0)+16}" font-size="9.5" fill="{CRIMSON}">R &#215; (1 + d/g) = 4 &#215; 1.25</text>'
            + f'<line x1="{Xw(4)}" y1="{Yc(2.4)}" x2="{Xw(4)}" y2="{Yc(2.1)}" stroke="{CRIMSON}" stroke-width="1.6"/>'
            + f'<text x="{Xw(4)-8}" y="{Yc(1.55)}" font-size="9.5" fill="{CRIMSON}" text-anchor="end">the trough: 0.3 clinics,</text>'
            + f'<text x="{Xw(4)-8}" y="{Yc(1.55)+12}" font-size="9.5" fill="{CRIMSON}" text-anchor="end">12.5 % behind at week 4</text>'
            + f'<line x1="{Xw(10.5556)}" y1="{Yc(8)}" x2="{Xw(13.3333)}" y2="{Yc(8)}" stroke="{INK}" stroke-width="1.4"/>'
            + f'<text x="{Xw(9.6)}" y="{Yc(8)-10}" font-size="10" font-weight="700" fill="{INK}">2.7778 weeks saved = USD 39,666.67</text>'
            + f'<text x="{Xw(9.6)}" y="{Yc(8)+16}" font-size="9.5" fill="{INK}">15.0 extra specialist-weeks = USD 63,000 &#8212; net (USD 23,333.33)</text>'
            + f'<text x="{Xw(1.2)}" y="{Yc(7.6)}" font-size="11" font-weight="600" fill="{SLATE}">crew of 6</text>'
            + f'<text x="{Xw(1.2)}" y="{Yc(6.9)}" font-size="11" font-weight="600" fill="{BLUE}">crew of 9</text>')
    (PML / "fig_1_3_3.svg").write_text(svg(W, H, body))

    # ---- Fig 1.3.2 payback is hyperbolic in adoption -----------------------------
    W, H, L, R, T, B = 660, 400, 88, 26, 30, 58
    COST, POT = 2400000.0, 979200.0

    def Xa(a):  # a in per cent, 30..100
        return L + (a - 30) / 70 * (W - L - R)

    def Yy(y):
        return H - B - y / 9 * (H - T - B)

    def pb(a):
        return COST / (POT * a / 100.0)

    grid = "".join(
        f'<line x1="{L}" y1="{Yy(y)}" x2="{W-R}" y2="{Yy(y)}" stroke="{GRID}"/>'
        f'<text x="{L-8}" y="{Yy(y)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{y}</text>'
        for y in range(0, 10))
    xt = "".join(f'<text x="{Xa(a)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{a} %</text>'
                 for a in range(30, 101, 10))
    curve = " ".join(f"{Xa(a/2):.1f},{Yy(pb(a/2)):.1f}" for a in range(60, 201))
    body = (grid + axes(L, T, W - R, H - B, "Sustained adoption", "Simple payback (years)") + xt
            + f'<line x1="{L}" y1="{Yy(3)}" x2="{W-R}" y2="{Yy(3)}" stroke="{CRIMSON}" stroke-width="1.3" stroke-dasharray="5 4"/>'
            + f'<text x="{L+6}" y="{Yy(3)-7}" font-size="10.5" fill="{CRIMSON}">three-year payback rule</text>'
            + f'<line x1="{Xa(41.05)}" y1="{Yy(0)}" x2="{Xa(41.05)}" y2="{Yy(8.6)}" stroke="{SLATE}" stroke-width="1" stroke-dasharray="3 3"/>'
            + f'<text x="{Xa(41.05)+6}" y="{Yy(8.3)}" font-size="9.5" fill="{SLATE}">41.05 % &#8212; Domain 2 breakeven for value</text>'
            + f'<line x1="{Xa(70)}" y1="{Yy(0)}" x2="{Xa(70)}" y2="{Yy(4.6)}" stroke="{SLATE}" stroke-width="1" stroke-dasharray="3 3"/>'
            + f'<text x="{Xa(70)+6}" y="{Yy(4.45)}" font-size="9.5" fill="{SLATE}">70 % assumed &#8212; 3.5014 years</text>'
            + f'<polyline points="{curve}" fill="none" stroke="{BLUE}" stroke-width="2.8"/>'
            + f'<circle cx="{Xa(81.6993)}" cy="{Yy(3)}" r="4" fill="{CRIMSON}"/>'
            + f'<text x="{Xa(81.6993)+8}" y="{Yy(3)-8}" font-size="10.5" font-weight="700" fill="{CRIMSON}">81.70 %</text>'
            + f'<text x="{Xa(81.6993)+8}" y="{Yy(3)+6}" font-size="9.5" fill="{CRIMSON}">the adoption the rule requires</text>'
            + f'<line x1="{Xa(41.05)}" y1="{Yy(1.0)}" x2="{Xa(81.6993)}" y2="{Yy(1.0)}" stroke="{INK}" stroke-width="1.3"/>'
            + f'<text x="{Xa(61)}" y="{Yy(0.6)}" font-size="9.5" fill="{INK}" text-anchor="middle">40.65 points &#8212; the cost of using payback as a proxy for value</text>'
            + f'<text x="{Xa(36)}" y="{Yy(6.6)}" font-size="9.5" fill="{INK}">40 &#8594; 50 %: &#8722;1.2255 yr</text>'
            + f'<text x="{Xa(78)}" y="{Yy(2.0)}" font-size="9.5" fill="{INK}">80 &#8594; 90 %: &#8722;0.3404 yr (3.60&#215; flatter)</text>')
    (PML / "fig_1_3_2.svg").write_text(svg(W, H, body))

    # ---- Fig 1.4.1 breakeven error probability (log scale) -----------------------
    W, H, L, R, T, B = 660, 330, 176, 30, 34, 62
    import math
    LO, HI = 0.1, 200.0  # per cent

    def Xp(p):
        return L + (math.log10(p) - math.log10(LO)) / (math.log10(HI) - math.log10(LO)) * (W - L - R)

    rows = [("Board benefits figure", "510 / 220,320", 0.2314814814814815, BLUE),
            ("Clinical-safety assessment", "12,000 / 3,800,000", 0.3157894736842105, BLUE),
            ("Weekly status summary", "42.50 / 500", 8.5, BLUE),
            ("Six-hour recompute on the summary", "510 / 500", 102.0, CRIMSON)]
    bh, gap = 34, 20
    body = ""
    for i, (lab, sub, val, col) in enumerate(rows):
        y = T + 8 + i * (bh + gap)
        body += (f'<rect x="{L}" y="{y}" width="{max(Xp(val)-L, 2):.1f}" height="{bh}" rx="3" fill="{col}" opacity="0.9"/>'
                 f'<text x="{L-10}" y="{y+14}" font-size="10.5" fill="{INK}" text-anchor="end">{lab}</text>'
                 f'<text x="{L-10}" y="{y+27}" font-size="9" fill="{SLATE}" text-anchor="end">{sub}</text>'
                 f'<text x="{Xp(val)+7:.1f}" y="{y+22}" font-size="10.5" font-weight="700" fill="{col}">{val:.4f} %</text>')
    ticks = "".join(f'<line x1="{Xp(p)}" y1="{T}" x2="{Xp(p)}" y2="{H-B}" stroke="{GRID}"/>'
                    f'<text x="{Xp(p)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{p:g} %</text>'
                    for p in (0.1, 1, 10, 100))
    body = (ticks + body
            + f'<line x1="{L}" y1="{T}" x2="{L}" y2="{H-B}" stroke="{INK}" stroke-width="1.2"/>'
            + f'<line x1="{L}" y1="{H-B}" x2="{W-R}" y2="{H-B}" stroke="{INK}" stroke-width="1.2"/>'
            + f'<line x1="{Xp(100)}" y1="{T}" x2="{Xp(100)}" y2="{H-B}" stroke="{CRIMSON}" stroke-width="1.3" stroke-dasharray="4 3"/>'
            + f'<text x="{Xp(100)-6}" y="{T+12}" font-size="9.5" fill="{CRIMSON}" text-anchor="end">no error rate can justify the check &#8594;</text>'
            + f'<text x="{(L+W-R)/2}" y="{H-B+34}" font-size="11.5" fill="{SLATE}" text-anchor="middle">Breakeven error probability P* = C&#8341; / [(1 &#8722; q) &#215; L], log scale</text>'
            + f'<text x="{L+8}" y="{T+8+3*(bh+gap)-6}" font-size="9.5" fill="{SLATE}">36.72&#215; between the summary and the board figure &#8212; while the cost of checking differs 23.53&#215;</text>')
    (PML / "fig_1_4_1.svg").write_text(svg(W, H, body))
