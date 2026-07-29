"""PFL-AI Domain 12 — Contracts and transaction structure. PCI original artwork.

Fig 12.1.1  Kestrel's cap stack against the loss it was drafted for: combined 300-day delay
            and 5 % output shortfall exposure of 12,255,674 against nominal contractual cover
            of 9,600,000 (two 10 % sub-caps that exhaust the 20 % aggregate exactly) and
            risk-adjusted cover of 8,160,000 once the parent guarantee is taken at a 0.70
            probability of payment. Residue 2,655,674 nominal, 4,095,674 credit-adjusted —
            14.75 % and 22.75 % of the 18,000,000 equity contribution.
Fig 12.2.1  The contracted volume floor a covenant requires: DSCR(x) = (9,060,000x
            &#8722; 2,676,000)/5,009,635.23, a straight line of slope 0.0181 of coverage per
            percentage point. The 1.20 covenant is met at 95.8892 % of guaranteed output, the
            1.15 lock-up engages at 93.1245 %, coverage reaches 1.0000 at 84.8304 %, and the
            commercially negotiated 90 % floor delivers only 1.0935.

Deterministic: every annotated value is a literal verified in the domain's decimal arithmetic
pass (see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 12.1.1 — the cap stack against the loss ----------------------------
    W, H = 880, 500
    L, R, T, B = 258, 176, 128, 74
    plot_w = W - L - R
    SPAN = 12500000.0                      # money axis 0 .. 12.5m

    def X(v):
        return L + v / SPAN * plot_w

    rows = [
        # (label, sublabel, segments [(value, colour, opacity, seg label)])
        ("Stress exposure", "300 days late &#183; 95 % of guaranteed output",
         [(7420000.0, CRIMSON, 0.90, "delay 7,420,000"),
          (4835673.61, CRIMSON, 0.55, "performance 4,835,674")], 12255673.61),
        ("Nominal contractual cover", "the caps as drafted &#8212; 20 % aggregate",
         [(4800000.0, BLUE, 0.92, "delay cap 4,800,000"),
          (4800000.0, BLUE, 0.55, "performance cap 4,800,000")], 9600000.0),
        ("Risk-adjusted cover", "bond at 1.00 &#183; parent guarantee at 0.70",
         [(4800000.0, BLUE, 0.92, "on-demand bond 4,800,000"),
          (3360000.0, SLATE, 0.75, "guarantee 3,360,000")], 8160000.0),
    ]
    row_h = (H - T - B) / len(rows)

    grid = "".join(
        f'<line x1="{X(v):.1f}" y1="{T - 12}" x2="{X(v):.1f}" y2="{H - B}" stroke="{GRID}"/>'
        f'<text x="{X(v):.1f}" y="{H - B + 16}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="middle">{v / 1000000:.0f}m</text>'
        for v in (0, 2000000, 4000000, 6000000, 8000000, 10000000, 12000000))

    bars = ""
    tops = {}
    for i, (lab, sub, segs, total) in enumerate(rows):
        y = T + i * row_h + row_h * 0.16
        h = row_h * 0.40
        tops[i] = (y, h)
        cursor = 0.0
        for val, col, op, seglab in segs:
            bars += (f'<rect x="{X(cursor):.1f}" y="{y:.1f}" '
                     f'width="{X(cursor + val) - X(cursor):.1f}" height="{h:.1f}" '
                     f'fill="{col}" opacity="{op}"/>')
            bars += (f'<text x="{X(cursor) + 8:.1f}" y="{y + h * 0.63:.1f}" font-size="9.4" '
                     f'fill="white" font-weight="600">{seglab}</text>')
            cursor += val
        bars += (f'<line x1="{X(total):.1f}" y1="{y - 3:.1f}" x2="{X(total):.1f}" '
                 f'y2="{y + h + 3:.1f}" stroke="{INK}" stroke-width="1.2"/>')
        bars += (f'<text x="{L - 14}" y="{y + h * 0.46:.1f}" font-size="11" fill="{INK}" '
                 f'text-anchor="end" font-weight="700">{lab}</text>')
        bars += (f'<text x="{L - 14}" y="{y + h * 0.46 + 13:.1f}" font-size="9.2" '
                 f'fill="{SLATE}" text-anchor="end">{sub}</text>')
        bars += (f'<text x="{X(total) + 9:.1f}" y="{y + h * 0.66:.1f}" font-size="10.6" '
                 f'font-weight="700" fill="{INK}">{total:,.0f}</text>')

    # residue brackets from the exposure end back to each cover level
    ex_end = 12255673.61
    brackets = ""
    for i, (cover, resid, pct, col) in enumerate(
            ((9600000.0, 2655673.61, "14.75 % of equity", INK),
             (8160000.0, 4095673.61, "22.75 % of equity", CRIMSON)), start=1):
        y, h = tops[i]
        yb = y + h + 15 + (i - 1) * 2
        brackets += (f'<line x1="{X(cover):.1f}" y1="{yb:.1f}" x2="{X(ex_end):.1f}" '
                     f'y2="{yb:.1f}" stroke="{col}" stroke-width="1.4"/>'
                     f'<line x1="{X(cover):.1f}" y1="{yb - 4:.1f}" x2="{X(cover):.1f}" '
                     f'y2="{yb + 4:.1f}" stroke="{col}" stroke-width="1.4"/>'
                     f'<line x1="{X(ex_end):.1f}" y1="{yb - 4:.1f}" x2="{X(ex_end):.1f}" '
                     f'y2="{yb + 4:.1f}" stroke="{col}" stroke-width="1.4"/>'
                     f'<text x="{(X(cover) + X(ex_end)) / 2:.1f}" y="{yb + 15:.1f}" '
                     f'font-size="9.8" font-weight="700" fill="{col}" text-anchor="middle">'
                     f'uncovered {resid:,.0f} &#183; {pct}</text>')

    # dashed reference at the exposure total, carried down the chart
    refline = (f'<line x1="{X(ex_end):.1f}" y1="{T - 12}" x2="{X(ex_end):.1f}" '
               f'y2="{H - B}" stroke="{CRIMSON}" stroke-width="1.1" opacity="0.5" '
               f'stroke-dasharray="5 4"/>')

    body = (grid + refline + bars + brackets
            + axes(L, T - 12, W - R, H - B,
                   "USD &#8212; exposure and recoverable cover in the combined stress", "")
            + f'<text x="24" y="28" font-size="12.6" font-weight="700" fill="{INK}">'
              'A cap is a promise; a recovery is a cap multiplied by the credit behind it</text>'
            + f'<text x="24" y="47" font-size="10.4" fill="{SLATE}">'
              'Kestrel: 48,000,000 EPC price &#183; delay damages 20,000/day capped at 10 %, '
              'binding at day 240 &#183; performance damages capped at 10 %</text>'
            + f'<text x="24" y="64" font-size="10.4" fill="{CRIMSON}">'
              'The two 10 % sub-caps exhaust the 20 % aggregate cap exactly &#8212; a third head '
              'of claim recovers nothing</text>'
            + f'<text x="24" y="83" font-size="10.4" fill="{BLUE}">'
              'One dollar of on-demand bank cover is worth 1.4286 dollars of parent guarantee at '
              'a 0.70 assessment:</text>'
            + f'<text x="24" y="100" font-size="10.4" fill="{BLUE}">'
              'the face amount equivalent to the 4,800,000 bond is 6,857,143</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 16}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'Every number above was on two pages of the contract before signature</text>')
    (PFL / "fig_12_1_1.svg").write_text(svg(W, H, body))

    # ---- Fig 12.2.1 — the contracted volume floor a covenant requires -----------
    SLOPE, ICPT, DS = 9060000.0, 2676000.0, 5009635.23

    def dscr(x):
        return (SLOPE * x - ICPT) / DS

    W, H = 830, 470
    L, R, T, B = 84, 172, 96, 70
    plot_w, plot_h = W - L - R, H - T - B
    x_lo, x_hi = 0.80, 1.02
    y_lo, y_hi = 0.90, 1.32

    def Xv(x):
        return L + (x - x_lo) / (x_hi - x_lo) * plot_w

    def Yd(v):
        return H - B - (v - y_lo) / (y_hi - y_lo) * plot_h

    grid = "".join(
        f'<line x1="{L}" y1="{Yd(v / 100.0):.1f}" x2="{W - R}" y2="{Yd(v / 100.0):.1f}" '
        f'stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Yd(v / 100.0) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v / 100.0:.2f}</text>'
        for v in range(90, 133, 5))
    xticks = "".join(
        f'<line x1="{Xv(x / 100.0):.1f}" y1="{T}" x2="{Xv(x / 100.0):.1f}" y2="{H - B}" '
        f'stroke="{GRID}"/>'
        f'<text x="{Xv(x / 100.0):.1f}" y="{H - B + 17}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{x} %</text>'
        for x in (80, 85, 90, 95, 100))

    line = (f'<polyline points="{Xv(x_lo):.1f},{Yd(dscr(x_lo)):.1f} '
            f'{Xv(x_hi):.1f},{Yd(dscr(x_hi)):.1f}" fill="none" stroke="{BLUE}" '
            f'stroke-width="2.8"/>')

    thresh = (f'<line x1="{L}" y1="{Yd(1.20):.1f}" x2="{W - R}" y2="{Yd(1.20):.1f}" '
              f'stroke="{INK}" stroke-width="1.6" stroke-dasharray="5 4"/>'
              f'<text x="{L + 6}" y="{Yd(1.20) - 7:.1f}" font-size="10.6" font-weight="700" '
              f'fill="{INK}">1.20 covenant &#8212; cash trigger 6,011,562</text>'
              f'<line x1="{L}" y1="{Yd(1.15):.1f}" x2="{W - R}" y2="{Yd(1.15):.1f}" '
              f'stroke="{SLATE}" stroke-width="1.2" stroke-dasharray="3 4"/>'
              f'<text x="{L + 6}" y="{Yd(1.15) - 6:.1f}" font-size="10" fill="{SLATE}">'
              '1.15 lock-up &#8212; engages at 93.1245 % (cash 5,761,081)</text>'
              f'<line x1="{L}" y1="{Yd(1.00):.1f}" x2="{W - R}" y2="{Yd(1.00):.1f}" '
              f'stroke="{SLATE}" stroke-width="1" stroke-dasharray="2 4"/>'
              f'<text x="{W - R - 6}" y="{Yd(1.00) - 7:.1f}" font-size="9.8" '
              f'fill="{SLATE}" text-anchor="end">DSCR 1.0000 &#8212; cash equals debt '
              'service</text>')

    marks = (f'<circle cx="{Xv(0.958892):.1f}" cy="{Yd(1.20):.1f}" r="4.4" fill="{CRIMSON}"/>'
             f'<text x="{Xv(0.958892) + 9:.1f}" y="{Yd(1.20) + 17:.1f}" font-size="10.6" '
             f'font-weight="700" fill="{CRIMSON}">'
             '95.8892 % &#8212; the floor</text>'
             f'<text x="{Xv(0.958892) + 9:.1f}" y="{Yd(1.20) + 30:.1f}" font-size="10.6" '
             f'font-weight="700" fill="{CRIMSON}">the covenant requires</text>'
             f'<circle cx="{Xv(0.931245):.1f}" cy="{Yd(1.15):.1f}" r="3.8" fill="{SLATE}"/>'
             f'<circle cx="{Xv(0.848304):.1f}" cy="{Yd(1.00):.1f}" r="3.6" fill="{SLATE}"/>'
             f'<text x="{Xv(0.848304) + 8:.1f}" y="{Yd(1.00) + 15:.1f}" font-size="9.8" '
             f'fill="{SLATE}">84.8304 %</text>'
             f'<circle cx="{Xv(1.00):.1f}" cy="{Yd(1.2743):.1f}" r="4.4" fill="{BLUE}"/>'
             f'<text x="{W - R + 8}" y="{Yd(1.2743) + 4:.1f}" font-size="10.4" '
             f'font-weight="700" fill="{BLUE}">1.2743</text>'
             f'<text x="{W - R + 8}" y="{Yd(1.2743) + 17:.1f}" font-size="9.4" fill="{BLUE}">'
             '100 % &#8212; the sized case</text>'
             f'<circle cx="{Xv(0.90):.1f}" cy="{Yd(dscr(0.90)):.1f}" r="4.4" fill="{CRIMSON}"/>'
             f'<text x="{Xv(0.90) + 10:.1f}" y="{Yd(dscr(0.90)) + 20:.1f}" font-size="10.6" '
             f'font-weight="700" fill="{CRIMSON}">1.0935 at the negotiated 90 % floor</text>'
             f'<text x="{Xv(0.90) + 10:.1f}" y="{Yd(dscr(0.90)) + 33:.1f}" font-size="9.6" '
             f'fill="{CRIMSON}">&#8212; a breach, and a lock-up two points lower</text>')

    # the 5.8892-point bracket between the negotiated floor and the required floor
    yb = Yd(1.285)
    brack = (f'<line x1="{Xv(0.90):.1f}" y1="{yb:.1f}" x2="{Xv(0.958892):.1f}" y2="{yb:.1f}" '
             f'stroke="{CRIMSON}" stroke-width="1.4"/>'
             f'<line x1="{Xv(0.90):.1f}" y1="{yb - 4:.1f}" x2="{Xv(0.90):.1f}" '
             f'y2="{yb + 4:.1f}" stroke="{CRIMSON}" stroke-width="1.4"/>'
             f'<line x1="{Xv(0.958892):.1f}" y1="{yb - 4:.1f}" x2="{Xv(0.958892):.1f}" '
             f'y2="{yb + 4:.1f}" stroke="{CRIMSON}" stroke-width="1.4"/>'
             f'<text x="{(Xv(0.90) + Xv(0.958892)) / 2:.1f}" y="{yb - 9:.1f}" font-size="10" '
             f'font-weight="700" fill="{CRIMSON}" text-anchor="middle">'
             '5.8892 points of volume short of the covenant</text>')

    body = (grid + xticks + thresh + line + brack + marks
            + axes(L, T, W - R, H - B,
                   "Contracted volume floor as a share of guaranteed output",
                   "Year-one DSCR")
            + f'<text x="24" y="28" font-size="12.6" font-weight="700" fill="{INK}">'
              'The contracted volume floor is a financing parameter, not a commercial '
              'preference</text>'
            + f'<text x="24" y="47" font-size="10.4" fill="{SLATE}">'
              'CFADS(x) = 9,060,000x &#8722; 2,676,000 against fixed debt service of '
              '5,009,635.23 &#8212; each point of volume is worth 0.0181 of coverage</text>'
            + f'<text x="24" y="64" font-size="10.4" fill="{CRIMSON}">'
              '85 % of cash operating cost is fixed, so a 10 % volume cut removes 14.19 % of '
              'CFADS and debt service does not move at all</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 14}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'The offtaker&#8217;s flexibility is worth 4.11 points of capacity, not ten'
              '</text>')
    (PFL / "fig_12_2_1.svg").write_text(svg(W, H, body))
