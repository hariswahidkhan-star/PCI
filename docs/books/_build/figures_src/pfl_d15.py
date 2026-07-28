"""PFL-AI Domain 15 — Operations, performance and restructuring. PCI original artwork.

Fig 15.2.1  Kestrel Water's operating cash waterfall over its first six operating years,
            drawn twice: on the left, each year's CFADS allocated in contractual priority
            (senior interest, senior scheduled principal, maintenance-reserve top-up, then
            cash trapped by the distribution test or released to equity), with the backward
            DSCR annotated above each bar; on the right, the balance of the
            distribution-block account that the trapped cash builds — 774,364.77 after
            year one, 1,128,623.65 after year two, drawn down to 721,924.90 in year three
            to fund a 406,698.75 reserve shortfall, 1,206,931.59 after year four, and
            released in full as part of the 2,077,924.35 distribution in year five.

Fig 15.4.1  The restructuring frontier at the end of Kestrel's third operating year, on
            the permanent-deterioration branch: lender recovery (present value at a 7.00 %
            required return, as a percentage of the 34,073,997.28 outstanding) against
            equity value (present value at 8 %, USD millions), for a maturity extension
            (95.0779 %, 14.8985m), a principal haircut (82.9037 %, 18.6562m), a sponsor
            injection (96.3549 %, 14.0728m) and enforcement (91.4291 %, nil). The
            enforcement floor is drawn as the vertical line no lender will cross, which
            places the haircut outside the feasible set.

Deterministic: every annotated value is a literal verified in the domain's decimal
arithmetic pass (see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ================================================================= Fig 15.2.1
    # (year, interest, principal, mra from the year's own cash, trapped, distributed,
    #  dscr, mra shortfall drawn from the block account)
    years = [
        (1, 2520000.00, 2489635.23, 600000.00, 774364.77, 0.00, "1.2743", 0.00),
        (2, 2370621.89, 2639013.34, 600000.00, 354258.88, 0.00, "1.1905", 0.00),
        (3, 2212281.09, 2797354.14, 193301.25, 0.00, 0.00, "1.0386", 406698.75),
        (4, 2044439.84, 2965195.39, 600000.00, 485006.68, 0.00, "1.2166", 0.00),
        (5, 1866528.11, 3143107.12, 600000.00, 0.00, 870992.76, "1.2936", 0.00),
        (6, 1677941.69, 3331693.54, 600000.00, 0.00, 885953.11, "1.2966", 0.00),
    ]
    balances = [774364.77, 1128623.65, 721924.90, 1206931.59, 0.00, 0.00]

    W, H = 880, 500
    # ---- left panel geometry
    L, T, B = 74, 104, 92
    PW = 452                                     # plot width of the left panel
    hi = 8200000.0

    def Y(v):
        return H - B - v / hi * (H - T - B)

    def bar_x(i):
        return L + (i + 0.18) * PW / 6

    bw = PW / 6 * 0.64
    seg_cols = [(BLUE, "Senior interest"),
                ("#5B84E8", "Senior scheduled principal"),
                (SLATE, "Maintenance-reserve top-up"),
                ("#94A3B8", "Trapped &#8212; distribution test failed"),
                (CRIMSON, "Distributed to equity")]

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{L + PW}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 4:.1f}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="end">{int(v / 1000000)}m</text>'
        for v in (1000000, 2000000, 3000000, 4000000, 5000000, 6000000, 7000000, 8000000))

    bars = ""
    for i, (yr, ii, pp, mm, tr, dd, dscr, short) in enumerate(years):
        x = bar_x(i)
        base = 0.0
        for val, (col, _lab) in zip((ii, pp, mm, tr, dd), seg_cols):
            if val <= 0:
                continue
            bars += (f'<rect x="{x:.1f}" y="{Y(base + val):.1f}" width="{bw:.1f}" '
                     f'height="{(Y(base) - Y(base + val)):.1f}" fill="{col}"/>')
            base += val
        cf = ii + pp + mm + tr + dd
        dy_lab = min(Y(cf) - 8, Y(6560000.0))       # always clear of the threshold band
        bars += (f'<text x="{x + bw / 2:.1f}" y="{dy_lab - 12:.1f}" font-size="9.4" '
                 f'fill="{INK}" text-anchor="middle">{cf:,.0f}</text>')
        bars += (f'<text x="{x + bw / 2:.1f}" y="{dy_lab:.1f}" font-size="10.2" '
                 f'font-weight="700" fill="{BLUE if float(dscr) >= 1.20 else CRIMSON}" '
                 f'text-anchor="middle">{dscr}</text>')
        bars += (f'<text x="{x + bw / 2:.1f}" y="{H - B + 16}" font-size="10.4" '
                 f'fill="{INK}" text-anchor="middle">Year {yr}</text>')
        if short > 0:
            ty = Y(7620000.0)
            bars += (f'<line x1="{x + bw - 3:.1f}" y1="{Y(6740000.0):.1f}" '
                     f'x2="{x + bw - 3:.1f}" y2="{ty + 6:.1f}" '
                     f'stroke="{CRIMSON}" stroke-width="1.1"/>'
                     f'<text x="{x + bw / 2:.1f}" y="{ty - 5:.1f}" text-anchor="middle" '
                     f'font-size="9.4" font-weight="700" fill="{CRIMSON}">reserve short '
                     f'{short:,.0f}</text>'
                     f'<text x="{x + bw / 2:.1f}" y="{ty + 5:.1f}" text-anchor="middle" '
                     f'font-size="9.0" fill="{CRIMSON}">funded from the block account</text>')

    # covenant / lock-up / distribution thresholds in cash, labelled in the gutter
    thresholds = [(6262044.04, "1.25&#215; distribution", "6,262,044", CRIMSON),
                  (6011562.28, "1.20&#215; covenant", "6,011,562", INK),
                  (5761080.51, "1.15&#215; lock-up", "5,761,081", SLATE)]
    thr = ""
    for k, (v, lab, cash, col) in enumerate(thresholds):
        oy = (-9, 3, 15)[k]
        thr += (f'<line x1="{L}" y1="{Y(v):.1f}" x2="{L + PW + 8}" y2="{Y(v):.1f}" '
                f'stroke="{col}" stroke-width="1.1" stroke-dasharray="5 4" opacity="0.9"/>'
                f'<text x="{L + PW + 12}" y="{Y(v) + oy - 4:.1f}" font-size="8.8" '
                f'font-weight="600" fill="{col}">{lab}</text>'
                f'<text x="{L + PW + 12}" y="{Y(v) + oy + 5:.1f}" font-size="8.6" '
                f'fill="{col}">{cash}</text>')

    legend = ""
    for k, (col, lab) in enumerate(seg_cols):
        ly = 26 + k * 13
        legend += (f'<rect x="{L}" y="{ly - 8}" width="9" height="9" fill="{col}"/>'
                   f'<text x="{L + 14}" y="{ly}" font-size="9.2" fill="{INK}">{lab}</text>')

    # ---- right panel: the distribution-block account
    RL = L + PW + 130
    RW = W - RL - 26
    rhi = 1400000.0

    def RY(v):
        return H - B - v / rhi * (H - T - B)

    rgrid = "".join(
        f'<line x1="{RL}" y1="{RY(v):.1f}" x2="{RL + RW}" y2="{RY(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{RL - 8}" y="{RY(v) + 4:.1f}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="end">{v / 1000000:.1f}m</text>'
        for v in (200000, 400000, 600000, 800000, 1000000, 1200000, 1400000))

    def RX(i):
        return RL + (i + 0.5) * RW / 6

    pts = " ".join(f"{RX(i):.1f},{RY(b):.1f}" for i, b in enumerate(balances))
    rline = (f'<polyline points="{pts}" fill="none" stroke={chr(34)}{BLUE}{chr(34)} '
             f'stroke-width="2.6"/>')
    for i, b in enumerate(balances):
        col = CRIMSON if i in (2, 4) else BLUE
        rline += f'<circle cx="{RX(i):.1f}" cy="{RY(b):.1f}" r="4.2" fill="{col}"/>'
        rline += (f'<text x="{RX(i):.1f}" y="{H - B + 16}" font-size="10.0" fill="{INK}" '
                 f'text-anchor="middle">Y{i + 1}</text>')
    rline += (f'<text x="{RX(1) + 6:.1f}" y="{RY(1128623.65) - 9:.1f}" font-size="9.4" '
              f'font-weight="700" fill="{BLUE}">1,128,624</text>'
              f'<text x="{RX(2) - 4:.1f}" y="{RY(721924.90) + 16:.1f}" font-size="9.4" '
              f'font-weight="700" fill="{CRIMSON}" text-anchor="middle">721,925</text>'
              f'<text x="{RX(3) + 6:.1f}" y="{RY(1206931.59) - 9:.1f}" font-size="9.4" '
              f'font-weight="700" fill="{BLUE}">1,206,932</text>'
              f'<text x="{RX(4) - 6:.1f}" y="{RY(0) - 34:.1f}" font-size="9.4" '
              f'font-weight="700" fill="{CRIMSON}" text-anchor="end">released in full as part</text>'
              f'<text x="{RX(4) - 6:.1f}" y="{RY(0) - 24:.1f}" font-size="9.4" '
              f'font-weight="700" fill="{CRIMSON}" text-anchor="end">of the 2,077,924</text>'
              f'<text x="{RX(4) - 6:.1f}" y="{RY(0) - 14:.1f}" font-size="9.4" '
              f'font-weight="700" fill="{CRIMSON}" text-anchor="end">paid in year five</text>')

    head = (f'<text x="{L}" y="16" font-size="11.6" font-weight="700" fill="{INK}">'
            f'The operating waterfall, in priority order</text>'
            f'<text x="{RL}" y="16" font-size="11.6" font-weight="700" fill="{INK}">'
            f'Distribution-block account</text>'
            f'<text x="{RL}" y="30" font-size="9.2" fill="{SLATE}">'
            f'cash the tests trapped, and what it paid for</text>')

    body = (head + grid + thr + bars + legend
            + axes(L, T, L + PW, H - B, "Operating year &#8212; CFADS allocated (USD)",
                   "USD (millions)")
            + rgrid + rline
            + axes(RL, T, RL + RW, H - B, "Operating year", "Balance (USD m)"))
    (PFL / "fig_15_2_1.svg").write_text(svg(W, H, body))

    # ================================================================= Fig 15.4.1
    # (label, lender recovery % at 7.00 %, equity PV at 8 % in USD m, colour)
    # (label, recovery %, equity PV m, colour, label anchor, dx, dy of the first text line)
    opts = [
        ("Maturity extension to 11 years", 95.0779, 14.8985, BLUE, "end", -12, -16),
        ("Principal haircut of 4,583,353", 82.9037, 18.6562, CRIMSON, "start", 12, -3),
        ("Sponsor injection of 4,583,353", 96.3549, 14.0728, BLUE, "end", -12, 20),
        ("Enforcement and sale", 91.4291, 0.0000, INK, "start", 13, -14),
    ]
    W2, H2 = 760, 470
    L2, R2, T2, B2 = 92, 34, 74, 74
    xlo, xhi = 80.0, 100.0
    ylo, yhi = -1.0, 21.0

    def X2(v):
        return L2 + (v - xlo) / (xhi - xlo) * (W2 - L2 - R2)

    def Y2(v):
        return H2 - B2 - (v - ylo) / (yhi - ylo) * (H2 - T2 - B2)

    g2 = "".join(
        f'<line x1="{L2}" y1="{Y2(v):.1f}" x2="{W2 - R2}" y2="{Y2(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L2 - 8}" y="{Y2(v) + 4:.1f}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="end">{v}</text>' for v in (0, 5, 10, 15, 20))
    g2 += "".join(
        f'<line x1="{X2(v):.1f}" y1="{T2}" x2="{X2(v):.1f}" y2="{H2 - B2}" stroke="{GRID}"/>'
        f'<text x="{X2(v):.1f}" y="{H2 - B2 + 17}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="middle">{v} %</text>' for v in (80, 85, 90, 95, 100))

    floor_x = X2(91.4291)
    reject = (f'<rect x="{L2}" y="{T2}" width="{floor_x - L2:.1f}" '
              f'height="{H2 - B2 - T2}" fill="{CRIMSON}" opacity="0.055"/>'
              f'<line x1="{floor_x:.1f}" y1="{T2 - 8}" x2="{floor_x:.1f}" y2="{H2 - B2}" '
              f'stroke="{CRIMSON}" stroke-width="1.8" stroke-dasharray="6 4"/>'
              f'<text x="{floor_x - 8:.1f}" y="{T2 - 14}" font-size="10.4" font-weight="700" '
              f'fill="{CRIMSON}" text-anchor="end">enforcement floor 91.4291 % &#8212; '
              f'no lender crosses this line</text>'
              f'<text x="{L2 + 10}" y="{H2 - B2 - 12}" font-size="10.0" font-weight="600" '
              f'fill="{CRIMSON}">lenders reject &#8212; enforcement pays them more</text>')
    par = (f'<line x1="{X2(100):.1f}" y1="{T2}" x2="{X2(100):.1f}" y2="{H2 - B2}" '
           f'stroke="{INK}" stroke-width="1.2"/>'
           f'<text x="{X2(100) - 6:.1f}" y="{H2 - B2 - 8}" font-size="9.6" fill="{INK}" '
           f'text-anchor="end">par</text>')

    pts2 = ""
    for lab, rec, eq, col, anchor, dx, dy in opts:
        cx, cy = X2(rec), Y2(eq)
        pts2 += f'<circle cx="{cx:.1f}" cy="{cy:.1f}" r="6.4" fill="{col}"/>'
        pts2 += (f'<text x="{cx + dx:.1f}" y="{cy + dy:.1f}" font-size="10.4" '
                 f'font-weight="700" fill="{INK}" text-anchor="{anchor}">{lab}</text>'
                 f'<text x="{cx + dx:.1f}" y="{cy + dy + 12:.1f}" font-size="9.4" '
                 f'fill="{SLATE}" text-anchor="{anchor}">recovery {rec:.4f} % &#183; equity '
                 f'{eq:.4f}m</text>')

    note = (f'<text x="{L2}" y="18" font-size="11.6" font-weight="700" fill="{INK}">'
            f'The restructuring frontier &#8212; what each option is worth to each side</text>'
            f'<text x="{L2}" y="32" font-size="9.4" fill="{SLATE}">'
            f'Kestrel Water at the end of operating year three, on the permanent-'
            f'deterioration branch: 34,073,997 outstanding, revised CFADS 5,202,936</text>'
            f'<text x="{L2}" y="{H2 - 16}" font-size="9.4" fill="{SLATE}">'
            f'The haircut is the option equity most prefers and the only one outside the '
            f'feasible set &#8212; it recovers 8.5254 points less than enforcement.</text>')

    body2 = (note + g2 + reject + par + pts2
             + axes(L2, T2, W2 - R2, H2 - B2,
                    "Lender recovery &#8212; present value at 7.00 %, % of amount outstanding",
                    "Equity value &#8212; PV at 8 % (USD m)"))
    (PFL / "fig_15_4_1.svg").write_text(svg(W2, H2, body2))
