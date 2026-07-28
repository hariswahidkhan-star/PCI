"""PFL-AI Domain 11 — Risk identification and allocation. PCI original artwork.

Fig 11.1.1  The allocation price test on Kestrel's construction risk register: for each of
            eight items, the owner's expected cost if retained against the contractor's
            loaded premium, drawn as the net value of transfer. Three items create
            2,292,000 of value; five destroy 1,780,000, and destroy 460,000 even at a zero
            margin.
Fig 11.3.1  Kestrel's DSCR against the reference rate at three hedge ratios: unhedged
            breaches the 1.20 covenant at +73.9 bp, a 75 % hedge survives +200 bp with
            1.2085, and 70.06 % is the minimum hedge ratio that survives it at all.

Deterministic: every annotated value is a literal verified in the domain's decimal
arithmetic pass (see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 11.1.1 — the allocation price test ---------------------------------
    # (id, short label, owner EMV if retained, contractor loaded premium, net)
    items = [
        ("A1", "Workmanship, defects, performance shortfall", 1800000, 672000, 1128000),
        ("A2", "Programme and labour productivity", 1820000, 952000, 868000),
        ("A3", "Equipment procurement and logistics", 800000, 504000, 296000),
        ("A8", "Permit / change-in-law monitoring works", 400000, 560000, -160000),
        ("A7", "Marine weather standby beyond allowance", 450000, 630000, -180000),
        ("A6", "Membrane price above the contract index", 490000, 735000, -245000),
        ("A5", "Utility diversion, third-party interface", 540000, 868000, -328000),
        ("A4", "Ground beyond the disclosed baseline", 960000, 1827000, -867000),
    ]
    W, H = 730, 470
    L, R, T, B = 268, 96, 96, 62
    plot_w = W - L - R
    span = 1200000.0                       # net value axis: &#8722;1.2m .. +1.2m
    zero = L + plot_w / 2
    n = len(items)
    row_h = (H - T - B) / n

    def X(v):
        return zero + v / span * (plot_w / 2)

    grid = "".join(
        f'<line x1="{X(v):.1f}" y1="{T}" x2="{X(v):.1f}" y2="{H - B}" stroke="{GRID}"/>'
        f'<text x="{X(v):.1f}" y="{H - B + 16}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="middle">{"+" if v > 0 else ("&#8722;" if v < 0 else "")}'
        f'{abs(v) / 1000000:.1f}m</text>'
        for v in (-1200000, -800000, -400000, 0, 400000, 800000, 1200000))
    bars = ""
    for i, (idd, lab, own, prem, net) in enumerate(items):
        y = T + i * row_h + row_h * 0.24
        h = row_h * 0.44
        col = BLUE if net > 0 else CRIMSON
        x0 = min(zero, X(net))
        bars += (f'<rect x="{x0:.1f}" y="{y:.1f}" width="{abs(X(net) - zero):.1f}" '
                 f'height="{h:.1f}" fill="{col}" opacity="0.92"/>')
        bars += (f'<text x="{L - 12}" y="{y + h * 0.58:.1f}" font-size="10.2" fill="{INK}" '
                 f'text-anchor="end" font-weight="600">{idd} &#183; {lab}</text>')
        bars += (f'<text x="{L - 12}" y="{y + h * 0.58 + 12.5:.1f}" font-size="9.2" '
                 f'fill="{SLATE}" text-anchor="end">retain {own:,} &#183; '
                 f'premium {prem:,}</text>')
        bars += (f'<text x="{W - R + 10}" y="{y + h * 0.62:.1f}" font-size="10.4" '
                 f'font-weight="700" fill="{col}" text-anchor="start">'
                 f'{"+" if net > 0 else "&#8722;"}{abs(net):,}</text>')
    body = (grid + bars
            + f'<line x1="{zero:.1f}" y1="{T}" x2="{zero:.1f}" y2="{H - B}" '
              f'stroke="{INK}" stroke-width="1.4"/>'
            + axes(L, T, W - R, H - B,
                   "Net value of transferring the item (owner&#8217;s expected cost if retained "
                   "&#8722; the contractor&#8217;s loaded premium, USD)", "")
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'Allocation is a price, not a preference &#8212; three transfers create 2,292,000, '
              'five destroy 1,780,000</text>'
            + f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
              'Contractor prices its own expected cost at a 40 % loading. Blue: it controls '
              'the risk. Crimson: it does not</text>'
            + f'<text x="24" y="60" font-size="10.4" fill="{CRIMSON}">'
              'The crimson five destroy 460,000 even at a zero margin: the contractor&#8217;s '
              'own expected cost, 3,300,000, exceeds the owner&#8217;s 2,840,000</text>'
            + f'<text x="24" y="76" font-size="10.4" fill="{BLUE}">'
              'The blue three still create value at a premium 190.79 % above the '
              'contractor&#8217;s expected cost &#8212; control is what pays</text>'
            + f'<text x="{zero:.1f}" y="{H - 16}" font-size="11.2" font-weight="600" '
              f'fill="{INK}" text-anchor="middle">'
              'Retained residue 2,690,000 &#8212; the register Domain 8 provisions against'
              '</text>')
    (PFL / "fig_11_1_1.svg").write_text(svg(W, H, body))

    # ---- Fig 11.3.1 — DSCR against the reference rate at three hedge ratios -----
    PRIN, DEBTB, CF = 2489635.23, 42000000.0, 6384000.0
    COV, LOCK, SWAP = 1.20, 1.15, 0.062

    def dscr(bp, h):
        rate = SWAP * h + (0.06 + bp / 10000.0) * (1 - h)
        return CF / (PRIN + DEBTB * rate)

    W, H = 720, 450
    L, R, T, B = 78, 178, 84, 64
    plot_w, plot_h = W - L - R, H - T - B
    lo_bp, hi_bp = -200.0, 200.0
    lo_y, hi_y = 1.04, 1.56

    def Xb(bp):
        return L + (bp - lo_bp) / (hi_bp - lo_bp) * plot_w

    def Yd(v):
        return H - B - (v - lo_y) / (hi_y - lo_y) * plot_h

    grid = "".join(
        f'<line x1="{L}" y1="{Yd(v / 100.0):.1f}" x2="{W - R}" y2="{Yd(v / 100.0):.1f}" '
        f'stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Yd(v / 100.0) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v / 100.0:.2f}</text>'
        for v in range(105, 156, 10))
    xticks = "".join(
        f'<text x="{Xb(bp):.1f}" y="{H - B + 17}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{"+" if bp > 0 else ("&#8722;" if bp < 0 else "")}{abs(bp)}</text>'
        for bp in (-200, -100, 0, 100, 200))
    series = ""
    for h, col, wid, dash in ((0.0, CRIMSON, 2.6, ""),
                              (0.75, BLUE, 2.6, ""),
                              (1.0, INK, 2.2, ' stroke-dasharray="7 4"')):
        pts = " ".join(f"{Xb(bp):.1f},{Yd(dscr(bp, h)):.1f}"
                       for bp in range(-200, 201, 10))
        series += (f'<polyline points="{pts}" fill="none" stroke="{col}" '
                   f'stroke-width="{wid}"{dash}/>')
    thresh = (f'<line x1="{L}" y1="{Yd(COV):.1f}" x2="{W - R}" y2="{Yd(COV):.1f}" '
              f'stroke="{INK}" stroke-width="1.6" stroke-dasharray="5 4"/>'
              f'<text x="{L + 6}" y="{Yd(COV) - 7:.1f}" font-size="10.6" font-weight="700" '
              f'fill="{INK}">1.20 covenant &#8212; cash trigger 6,011,562</text>'
              f'<line x1="{L}" y1="{Yd(LOCK):.1f}" x2="{W - R}" y2="{Yd(LOCK):.1f}" '
              f'stroke="{SLATE}" stroke-width="1.2" stroke-dasharray="3 4"/>'
              f'<text x="{L + 6}" y="{Yd(LOCK) - 6:.1f}" font-size="10" fill="{SLATE}">'
              '1.15 lock-up</text>')
    marks = (f'<circle cx="{Xb(73.9):.1f}" cy="{Yd(COV):.1f}" r="4" fill="{CRIMSON}"/>'
             f'<text x="{Xb(73.9) + 8:.1f}" y="{Yd(COV) + 20:.1f}" font-size="10.4" '
             f'font-weight="700" fill="{CRIMSON}">+73.9 bp &#8212; unhedged breach</text>'
             f'<circle cx="{Xb(200):.1f}" cy="{Yd(dscr(200, 0.75)):.1f}" r="4" fill="{BLUE}"/>'
             f'<circle cx="{Xb(0):.1f}" cy="{Yd(dscr(0, 0.0)):.1f}" r="3.6" fill="{CRIMSON}"/>'
             f'<text x="{Xb(0) + 8:.1f}" y="{Yd(dscr(0, 0.0)) - 8:.1f}" font-size="10.2" '
             f'fill="{INK}">1.2743 at close</text>')
    labels = (f'<text x="{W - R + 8}" y="{Yd(dscr(200, 0.0)) + 4:.1f}" font-size="10.4" '
              f'font-weight="700" fill="{CRIMSON}">1.0914</text>'
              f'<text x="{W - R + 8}" y="{Yd(dscr(200, 0.0)) + 17:.1f}" font-size="9.6" '
              f'fill="{CRIMSON}">unhedged, +200 bp</text>'
              f'<text x="{W - R + 8}" y="{Yd(dscr(200, 0.75)) + 4:.1f}" font-size="10.4" '
              f'font-weight="700" fill="{BLUE}">1.2085</text>'
              f'<text x="{W - R + 8}" y="{Yd(dscr(200, 0.75)) + 17:.1f}" font-size="9.6" '
              f'fill="{BLUE}">75 % hedged, +200 bp</text>'
              f'<text x="{W - R + 8}" y="{Yd(dscr(0, 1.0)) + 4:.1f}" font-size="10.4" '
              f'font-weight="700" fill="{INK}">1.2533</text>'
              f'<text x="{W - R + 8}" y="{Yd(dscr(0, 1.0)) + 17:.1f}" font-size="9.6" '
              f'fill="{INK}">fully swapped at 6.20 %</text>'
              f'<text x="{L + 8}" y="{Yd(1.5311) - 9:.1f}" font-size="10.2" '
              f'font-weight="700" fill="{CRIMSON}">1.5311 unhedged at &#8722;200 bp</text>')
    body = (grid + xticks + series + thresh + marks + labels
            + axes(L, T, W - R, H - B,
                   "Shift in the reference rate from its 4.00 % level at close (basis points)",
                   "Year-one DSCR")
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'A swap costs 0.0210 of coverage and removes 0.4397 of range &#8212; 20.92 '
              'units of range per unit given up</text>'
            + f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
              'Principal is fixed by the schedule at 2,489,635.23; only interest floats. '
              'Swap rate 4.20 % + 200 bp margin = 6.20 % all-in</text>'
            + f'<text x="24" y="60" font-size="10.4" fill="{BLUE}">'
              '70.06 % is the minimum hedge ratio that survives a 200 bp shock &#8212; which is '
              'why lenders covenant a ratio, not a full hedge</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 14}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'Unhedged, 74 basis points of the reference rate stand between this project and a '
              'covenant breach</text>')
    (PFL / "fig_11_3_1.svg").write_text(svg(W, H, body))
