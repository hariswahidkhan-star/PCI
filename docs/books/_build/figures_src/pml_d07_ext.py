"""PML-AI Domain 7 (extension) — Cost, resources and commercial awareness. PCI original artwork.

Fig 7.1.1  What an accuracy class actually says: Auriga's USD 4,000,000 point estimate inside its
           declared -15 %/+30 % class range, read as a triangular distribution, with the point
           estimate sitting at the 33.3rd percentile and the range's own mean at 4,200,000.
Fig 7.4.1  Auriga's cash bridge at week 13: cost incurred 2,120,000 less payables 742,000 gives
           cash paid out 1,378,000; less cash received 334,400 leaves USD 1,043,600 of funding
           absorbed - 44.80 days of spend - beside the same figure under 30-day client terms.

Every value here is computed in the domain's verification arithmetic and printed in the manuscript.
Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""


def make(ctx):
    svg, PML = ctx["svg"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ================================================================= Fig 7.1.1
    W, H, L, R, T, B = 690, 430, 74, 30, 60, 76
    XLO, XHI = 3_200_000.0, 5_450_000.0
    a, m, b = 3_400_000.0, 4_000_000.0, 5_200_000.0
    peak = 2.0 / (b - a)          # triangular density at the mode

    def X(v):
        return L + (v - XLO) / (XHI - XLO) * (W - L - R)

    def Y(d):
        return (H - B) - d / peak * (H - T - B) * 0.92

    base = H - B
    # shaded left third: the part of the range below the approved point estimate
    left = (f'<polygon points="{X(a):.1f},{base:.1f} {X(m):.1f},{Y(peak):.1f} '
            f'{X(m):.1f},{base:.1f}" fill="{CRIMSON}" opacity="0.16"/>')
    right = (f'<polygon points="{X(m):.1f},{base:.1f} {X(m):.1f},{Y(peak):.1f} '
             f'{X(b):.1f},{base:.1f}" fill="{BLUE}" opacity="0.13"/>')
    outline = (f'<polyline points="{X(a):.1f},{base:.1f} {X(m):.1f},{Y(peak):.1f} '
               f'{X(b):.1f},{base:.1f}" fill="none" stroke="{BLUE}" stroke-width="2.4"/>')

    grid = ""
    for v in range(3_400_000, 5_400_001, 200_000):
        grid += (f'<line x1="{X(v):.1f}" y1="{T}" x2="{X(v):.1f}" y2="{base:.1f}" '
                 f'stroke="{GRID}"/>'
                 f'<text x="{X(v):.1f}" y="{base+17:.1f}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="middle">{v/1_000_000:.1f}m</text>')

    axis = (f'<line x1="{L}" y1="{base:.1f}" x2="{W-R}" y2="{base:.1f}" stroke="{INK}" '
            f'stroke-width="1.2"/>'
            f'<text x="{(L+W-R)/2:.1f}" y="{base+40:.1f}" font-size="11.5" fill="{SLATE}" '
            f'text-anchor="middle">Outturn cost (USD), the declared class range read as a '
            f'triangular distribution</text>')

    marks = [
        (m, "point estimate / BAC 4,000,000", "approved at the 33.3rd percentile",
         CRIMSON, 196, "end"),
        (4_160_770.0, "median 4,160,770", "", SLATE, 74, "start"),
        (4_200_000.0, "mean 4,200,000 = week-13 EAC(a)", "", INK, 112, "start"),
        (4_542_733.0, "P80 4,542,733", "+13.57 % on the point", BLUE, 152, "start"),
    ]
    pins = ""
    for v, lab, sub, col, dy, anc in marks:
        top = base - dy
        pins += (f'<line x1="{X(v):.1f}" y1="{base:.1f}" x2="{X(v):.1f}" y2="{top:.1f}" '
                 f'stroke="{col}" stroke-width="1.5" stroke-dasharray="4 3"/>'
                 f'<circle cx="{X(v):.1f}" cy="{top:.1f}" r="3.2" fill="{col}"/>')
        off = -7 if anc == "end" else 7
        pins += (f'<text x="{X(v)+off:.1f}" y="{top-8:.1f}" font-size="10.4" fill="{INK}" '
                 f'text-anchor="{anc}" font-weight="600">{lab}</text>')
        if sub:
            pins += (f'<text x="{X(v)+off:.1f}" y="{top+5:.1f}" font-size="9.6" fill="{SLATE}" '
                     f'text-anchor="{anc}">{sub}</text>')

    band = (f'<line x1="{X(a):.1f}" y1="{T-22}" x2="{X(b):.1f}" y2="{T-22}" stroke="{SLATE}" '
            f'stroke-width="1"/>'
            f'<line x1="{X(a):.1f}" y1="{T-27}" x2="{X(a):.1f}" y2="{T-17}" stroke="{SLATE}"/>'
            f'<line x1="{X(b):.1f}" y1="{T-27}" x2="{X(b):.1f}" y2="{T-17}" stroke="{SLATE}"/>'
            f'<text x="{(X(a)+X(b))/2:.1f}" y="{T-28}" font-size="10" fill="{SLATE}" '
            f'text-anchor="middle">band 3,400,000 &#8211; 5,200,000 = 1,800,000, '
            f'45.0 % of the point estimate; &#963; = 374,166</text>')

    notes = (f'<text x="{L}" y="{base+56:.1f}" font-size="10.2" fill="{CRIMSON}" '
             f'font-weight="600">Shaded left: only 33.3 % of the declared range lies at or below '
             f'the number that was approved.</text>'
             f'<text x="{L}" y="{base+70:.1f}" font-size="10.2" fill="{SLATE}">'
             f'A PERT weighting of the same three points gives 4,100,000 &#8212; asymmetric '
             f'ranges put the mean above the mode.</text>')

    body = grid + left + right + outline + axis + band + pins + notes
    (PML / "fig_7_1_1.svg").write_text(svg(W, H, body))

    # ================================================================= Fig 7.4.1
    W2, H2, T2, B2 = 700, 440, 78, 92
    L2, R2 = 66, 236                      # left panel: the bridge
    TOP = 2_300_000.0

    def Y2(v):
        return (H2 - B2) - v / TOP * (H2 - T2 - B2)

    grid2 = ""
    for v in range(0, 2_300_001, 500_000):
        grid2 += (f'<line x1="{L2}" y1="{Y2(v):.1f}" x2="{W2-R2}" y2="{Y2(v):.1f}" '
                  f'stroke="{GRID}"/>'
                  f'<text x="{L2-8}" y="{Y2(v)+4:.1f}" font-size="9.6" fill="{SLATE}" '
                  f'text-anchor="end">{v/1_000_000:.1f}m</text>')

    # (label, bottom, top, colour, opacity, printed value, sublabel)
    steps = [
        ("Cost<br>incurred", 0.0, 2_120_000.0, BLUE, "1", "2,120,000", "AC at week 13"),
        ("less<br>payables", 1_378_000.0, 2_120_000.0, SLATE, "0.45", "(742,000)", "35.0 % of AC"),
        ("Cash<br>paid out", 0.0, 1_378_000.0, BLUE, "0.55", "1,378,000", ""),
        ("less cash<br>received", 1_043_600.0, 1_378_000.0, SLATE, "0.45", "(334,400)", ""),
        ("Funding<br>absorbed", 0.0, 1_043_600.0, CRIMSON, "1", "1,043,600", "44.80 days of spend"),
    ]
    slot = (W2 - R2 - L2) / len(steps)
    bw = slot * 0.56
    bars = ""
    for k, (lab, lo, hi, col, op, val, sub) in enumerate(steps):
        x = L2 + k * slot + (slot - bw) / 2
        bars += (f'<rect x="{x:.1f}" y="{Y2(hi):.1f}" width="{bw:.1f}" '
                 f'height="{Y2(lo)-Y2(hi):.1f}" fill="{col}" opacity="{op}" stroke="white" '
                 f'stroke-width="0.8"/>'
                 f'<text x="{x+bw/2:.1f}" y="{Y2(hi)-7:.1f}" font-size="10.6" fill="{INK}" '
                 f'text-anchor="middle" font-weight="700">{val}</text>')
        for j, part in enumerate(lab.split("<br>")):
            bars += (f'<text x="{x+bw/2:.1f}" y="{H2-B2+16+j*11:.1f}" font-size="9.8" '
                     f'fill="{INK}" text-anchor="middle">{part}</text>')
        if sub:
            bars += (f'<text x="{x+bw/2:.1f}" y="{H2-B2+40:.1f}" font-size="8.8" fill="{SLATE}" '
                     f'text-anchor="middle">{sub}</text>')
        if k < len(steps) - 1:
            nxt = steps[k + 1]
            link = nxt[2] if k % 2 == 0 else nxt[2]
            bars += (f'<line x1="{x+bw:.1f}" y1="{Y2(link):.1f}" '
                     f'x2="{x+slot:.1f}" y2="{Y2(link):.1f}" stroke="{SLATE}" '
                     f'stroke-width="0.9" stroke-dasharray="3 3"/>')

    axis2 = (f'<line x1="{L2}" y1="{H2-B2:.1f}" x2="{W2-R2}" y2="{H2-B2:.1f}" stroke="{INK}" '
             f'stroke-width="1.2"/>'
             f'<line x1="{L2}" y1="{T2}" x2="{L2}" y2="{H2-B2:.1f}" stroke="{INK}" '
             f'stroke-width="1.2"/>')

    # right panel: the terms lever
    PL, PR = W2 - R2 + 54, W2 - 22
    pbars = ""
    for k, (lab, val, col) in enumerate([("60-day terms", 1_043_600.0, CRIMSON),
                                         ("30-day terms", 458_400.0, BLUE)]):
        slot2 = (PR - PL) / 2
        bwp = slot2 * 0.5
        x = PL + k * slot2 + (slot2 - bwp) / 2
        pbars += (f'<rect x="{x:.1f}" y="{Y2(val):.1f}" width="{bwp:.1f}" '
                  f'height="{Y2(0)-Y2(val):.1f}" fill="{col}" opacity="0.9"/>'
                  f'<text x="{x+bwp/2:.1f}" y="{Y2(val)-7:.1f}" font-size="10.4" fill="{INK}" '
                  f'text-anchor="middle" font-weight="700">{val:,.0f}</text>'
                  f'<text x="{x+bwp/2:.1f}" y="{H2-B2+16:.1f}" font-size="9.6" fill="{INK}" '
                  f'text-anchor="middle">{lab}</text>')
    pbars += (f'<line x1="{PL-6}" y1="{H2-B2:.1f}" x2="{PR}" y2="{H2-B2:.1f}" stroke="{INK}" '
              f'stroke-width="1.2"/>'
              f'<text x="{(PL+PR)/2:.1f}" y="118" font-size="10.4" fill="{INK}" '
              f'text-anchor="middle" font-weight="600">Same work, two client</text>'
              f'<text x="{(PL+PR)/2:.1f}" y="132" font-size="10.4" fill="{INK}" '
              f'text-anchor="middle" font-weight="600">payment terms</text>'
              f'<text x="{(PL+PR)/2:.1f}" y="{H2-B2+34:.1f}" font-size="9.6" fill="{SLATE}" '
              f'text-anchor="middle">&#8722;56.08 % from one term</text>')

    head = (f'<text x="24" y="26" font-size="12.2" fill="{INK}" font-weight="700">'
            f'Auriga at week 13: the cost report says 200,000; the bank account says '
            f'1,043,600</text>'
            f'<text x="24" y="42" font-size="10.2" fill="{SLATE}">'
            f'Revenue recognised 2,112,000 against cost 2,120,000 &#8212; a margin of '
            f'(8,000).</text>'
            f'<text x="24" y="56" font-size="10.2" fill="{SLATE}">'
            f'Client exposure 1,777,600 = receivables 1,504,800 + retention 96,800 + unbilled '
            f'176,000.</text>')

    body2 = grid2 + bars + axis2 + pbars + head
    (PML / "fig_7_4_1.svg").write_text(svg(W2, H2, body2))
