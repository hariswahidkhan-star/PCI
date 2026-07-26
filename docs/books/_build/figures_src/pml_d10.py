"""PML-AI Domain 10 — Procurement, contracts and supply networks. PCI original artwork.

Fig 10.1.1  make-or-buy total cost of ownership against volume: the two breakevens the unit
            price hides (133.33 units on cost alone; 358.33 once the capability stand-up delay
            is priced), with Auriga's required 84 units marked.
Fig 10.2.1  tender evaluation score against the price weighting: three bidders, three winning
            bands, crossovers at 57.70 % and 63.77 % price weight.

All values are those computed in the manuscript with decimal arithmetic.
Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ================================================================ Fig 10.1.1
    # TCO(make) = 480,000 + 3,600Q ; TCO(buy) = 240,000 + 5,400Q
    # TCO(make + 9 weeks of stand-up delay at 45,000/week) = 885,000 + 3,600Q
    W, H, L, R, T, B = 700, 430, 88, 172, 34, 62
    QMAX, VMAX = 400.0, 2500000.0

    def X(q):
        return L + q / QMAX * (W - R - L)

    def Y(v):
        return (H - B) - v / VMAX * (H - T - B)

    def line(f, a=0.0, b=QMAX):
        return f"{X(a):.1f},{Y(f(a)):.1f} {X(b):.1f},{Y(f(b)):.1f}"

    buy = lambda q: 240000 + 5400 * q          # noqa: E731
    mk = lambda q: 480000 + 3600 * q           # noqa: E731
    mkd = lambda q: 885000 + 3600 * q          # noqa: E731

    grid = ""
    for v in range(0, 2500001, 500000):
        grid += (f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W-R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Y(v)+4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="end">{v//1000:,}k</text>')
    for q in range(0, 401, 50):
        grid += (f'<text x="{X(q):.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{q}</text>')

    curves = (f'<polyline points="{line(buy)}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
              f'<polyline points="{line(mk)}" fill="none" stroke="{INK}" stroke-width="2.4"/>'
              f'<polyline points="{line(mkd)}" fill="none" stroke="{CRIMSON}" stroke-width="2.2" '
              f'stroke-dasharray="7 4"/>')

    # Breakevens.
    marks = ""
    for q, val, lab, col, anch, dx in ((133.3333, 960000.0, "breakeven 133.33 units", INK, "start", 9),
                                       (358.3333, 2175000.0, "breakeven 358.33 units", CRIMSON,
                                        "end", -9)):
        marks += (f'<circle cx="{X(q):.1f}" cy="{Y(val):.1f}" r="4.2" fill="{col}"/>'
                  f'<line x1="{X(q):.1f}" y1="{Y(val):.1f}" x2="{X(q):.1f}" y2="{H-B}" '
                  f'stroke="{col}" stroke-width="0.9" stroke-dasharray="3 3"/>'
                  f'<text x="{X(q)+dx:.1f}" y="{Y(val)-8:.1f}" font-size="10.5" fill="{col}" '
                  f'text-anchor="{anch}" font-weight="600">{lab}</text>')

    # Auriga's required volume: 84 units.
    q84 = 84.0
    band = (f'<rect x="{L}" y="{T}" width="{X(q84)-L:.1f}" height="{H-B-T}" fill="{BLUE}" '
            f'opacity="0.05"/>'
            f'<line x1="{X(q84):.1f}" y1="{T}" x2="{X(q84):.1f}" y2="{H-B}" stroke="{SLATE}" '
            f'stroke-width="1.3"/>'
            f'<text x="{X(q84)+7:.1f}" y="{H-B-10}" font-size="10.5" fill="{SLATE}">'
            f'Auriga needs 84 units</text>'
            f'<circle cx="{X(q84):.1f}" cy="{Y(buy(q84)):.1f}" r="3.6" fill="{BLUE}"/>'
            f'<circle cx="{X(q84):.1f}" cy="{Y(mk(q84)):.1f}" r="3.6" fill="{INK}"/>'
            f'<text x="{X(q84)+8:.1f}" y="{Y(mk(q84))-12:.1f}" font-size="10" fill="{INK}">'
            f'make 782,400</text>'
            f'<text x="{X(q84)+8:.1f}" y="{Y(buy(q84))+15:.1f}" font-size="10" fill="{BLUE}">'
            f'buy 693,600</text>'
            f'<text x="{X(q84)+8:.1f}" y="{Y(buy(q84))+28:.1f}" font-size="10" fill="{BLUE}">'
            f'cheaper by 88,800</text>')

    keys = ""
    for k, (lab, col, dash) in enumerate((
            ("Buy  240,000 + 5,400Q", BLUE, ""),
            ("Make  480,000 + 3,600Q", INK, ""),
            ("Make + 9-week delay", CRIMSON, ' stroke-dasharray="6 3"'))):
        y = T + 6 + k * 18
        keys += (f'<line x1="{W-R+10}" y1="{y}" x2="{W-R+30}" y2="{y}" stroke="{col}" '
                 f'stroke-width="2.4"{dash}/>'
                 f'<text x="{W-R+35}" y="{y+4}" font-size="9.6" fill="{INK}">{lab}</text>')
    keys += (f'<text x="{W-R+10}" y="{T+80}" font-size="10" fill="{SLATE}">Unit price favours</text>'
             f'<text x="{W-R+10}" y="{T+93}" font-size="10" fill="{SLATE}">making by 33.33 %</text>'
             f'<text x="{W-R+10}" y="{T+106}" font-size="10" fill="{SLATE}">&#8212; and is the</text>'
             f'<text x="{W-R+10}" y="{T+119}" font-size="10" fill="{CRIMSON}" '
             f'font-weight="600">wrong test</text>')

    body = (grid + band + curves + marks + keys
            + axes(L, T, W - R, H - B, "Volume Q (units over the ownership period)",
                   "Total cost of ownership, USD"))
    (PML / "fig_10_1_1.svg").write_text(svg(W, H, body))

    # ================================================================ Fig 10.2.1
    # Score(w) = w x price score + (1 - w) x quality score.
    # Alpha  62 -> 100.000000 ; Beta 78 -> 90.909091 ; Gamma 92 -> 80.645161
    W, H, L, R, T, B = 680, 420, 78, 150, 40, 64
    LOW, HIGH = 60.0, 102.0

    def Xw(w):
        return L + w * (W - R - L)

    def Ys(s):
        return (H - B) - (s - LOW) / (HIGH - LOW) * (H - T - B)

    bidders = (("Alpha  2,000,000", 62.0, 100.000000, INK),
               ("Beta   2,200,000", 78.0, 90.909091, BLUE),
               ("Gamma  2,480,000", 92.0, 80.645161, CRIMSON))

    grid = ""
    for s in range(60, 103, 6):
        grid += (f'<line x1="{L}" y1="{Ys(s):.1f}" x2="{W-R}" y2="{Ys(s):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Ys(s)+4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="end">{s}</text>')
    for pc in range(0, 101, 20):
        grid += (f'<text x="{Xw(pc/100.0):.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{pc} %</text>')

    # Winning bands.
    C1, C2 = 0.576988, 0.637681
    bands = ""
    for x0, x1, lab, col, dy in ((0.0, C1, "Gamma wins", CRIMSON, 14),
                                 (C1, C2, "Beta wins here", BLUE, 32),
                                 (C2, 1.0, "Alpha wins", INK, 14)):
        bands += (f'<rect x="{Xw(x0):.1f}" y="{T}" width="{Xw(x1)-Xw(x0):.1f}" '
                  f'height="{H-B-T}" fill="{col}" opacity="0.055"/>'
                  f'<text x="{(Xw(x0)+Xw(x1))/2:.1f}" y="{T+dy}" font-size="10.5" fill="{col}" '
                  f'text-anchor="middle" font-weight="600">{lab}</text>')
    for c, lab, anch, dx, dy in ((C1, "57.70 %", "end", -5, -8), (C2, "63.77 %", "start", 5, -22)):
        bands += (f'<line x1="{Xw(c):.1f}" y1="{T}" x2="{Xw(c):.1f}" y2="{H-B}" stroke="{SLATE}" '
                  f'stroke-width="1.1" stroke-dasharray="4 3"/>'
                  f'<text x="{Xw(c)+dx:.1f}" y="{H-B+dy}" font-size="10" fill="{SLATE}" '
                  f'text-anchor="{anch}">{lab}</text>')

    curves, labels = "", ""
    for k, (name, q, p, col) in enumerate(bidders):
        curves += (f'<polyline points="{Xw(0):.1f},{Ys(q):.1f} {Xw(1):.1f},{Ys(p):.1f}" '
                   f'fill="none" stroke="{col}" stroke-width="2.6"/>')
        y = T + 44 + k * 19
        labels += (f'<line x1="{W-R+10}" y1="{y}" x2="{W-R+32}" y2="{y}" stroke="{col}" '
                   f'stroke-width="2.6"/>'
                   f'<text x="{W-R+37}" y="{y+4}" font-size="9.4" fill="{INK}">{name}</text>')

    # The two published-weighting points.
    pts = ""
    for w, who, sc, col, anch, dx, dy in ((0.40, "Gamma", 87.458065, CRIMSON, "end", -8, 15),
                                          (0.70, "Alpha", 88.600000, INK, "start", 8, -10)):
        pts += (f'<circle cx="{Xw(w):.1f}" cy="{Ys(sc):.1f}" r="4.4" fill="{col}"/>'
                f'<text x="{Xw(w)+dx:.1f}" y="{Ys(sc)+dy:.1f}" font-size="10.5" fill="{col}" '
                f'text-anchor="{anch}" font-weight="600">'
                f'{int(w*100)}/{int(100-w*100)} &#8594; {who} {sc:.2f}</text>')

    note = (f'<text x="{W-R+10}" y="{H-B-46}" font-size="10" fill="{SLATE}">Same three bids.</text>'
            f'<text x="{W-R+10}" y="{H-B-33}" font-size="10" fill="{SLATE}">Three winners.</text>'
            f'<text x="{W-R+10}" y="{H-B-16}" font-size="10" fill="{CRIMSON}" font-weight="600">'
            f'Fix the weighting</text>'
            f'<text x="{W-R+10}" y="{H-B-3}" font-size="10" fill="{CRIMSON}" font-weight="600">'
            f'before opening.</text>')

    body = (grid + bands + curves + pts + labels + note
            + axes(L, T, W - R, H - B, "Weight given to price (quality takes the remainder)",
                   "Weighted evaluation score, points out of 100"))
    (PML / "fig_10_2_1.svg").write_text(svg(W, H, body))
