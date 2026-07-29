"""PFL-AI Domain 3 extension figures — time value of money. PCI original artwork.

Fig 3.2.3  Outstanding principal, not debt service: the balance path of Kestrel's
           USD 42,000,000 under four schedule shapes over twelve years at 6.0 % —
           annuity (42,000,000 falling to 21,102,406 at the half-way point and zero at
           maturity), level principal (a straight line through 21,000,000 at year 6),
           bullet (flat at 42,000,000 until the balloon) and a three-year repayment
           holiday (flat for three years, then steeper, 21,396,798 at year 8).
Fig 3.3.3  The four ways to discount one 25-year stream of USD 5,600,000 of year-0
           purchasing power at a 9 % nominal hurdle with 3 % inflation: the two
           consistent treatments both give 72,791,113; real flows at the nominal rate
           give 55,006,446 (&#8722;24.4325 %); nominal flows at the real rate give
           100,366,400 (+37.8828 %); and the Fisher subtraction shortcut gives
           71,586,794.

Deterministic: every value below is a literal verified in the domain's arithmetic pass
(decimal.Decimal at 28 digits; see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 3.2.3 — outstanding balance under four schedule shapes ---------------
    annuity = [42000000, 39510365, 36871351, 34073997, 31108802, 27965695,
               24634001, 21102406, 17358915, 13390815, 9184628, 4726071, 0]
    levelpr = [42000000 - 3500000 * k for k in range(13)]
    bullet = [42000000] * 12 + [0]
    holiday = [42000000, 42000000, 42000000, 42000000, 38345066, 34470836,
               30364153, 26011068, 21396798, 16505672, 11321078, 5825409, 0]

    W, H, L, R, T, B = 700, 430, 86, 150, 30, 60
    pw, ph = W - L - R, H - T - B

    def X(t):
        return L + t / 12 * pw

    def Y(v):
        return H - B - v / 45000000.0 * ph

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W - R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v // 1000000}m</text>'
        for v in range(0, 50000000, 7000000))
    xticks = "".join(
        f'<text x="{X(t):.1f}" y="{H - B + 16}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{t}</text>' for t in range(0, 13))

    def path(series, colour, width, dash=""):
        pts = " ".join(f"{X(t):.1f},{Y(v):.1f}" for t, v in enumerate(series))
        d = f' stroke-dasharray="{dash}"' if dash else ""
        return (f'<polyline points="{pts}" fill="none" stroke="{colour}" '
                f'stroke-width="{width}"{d}/>')

    lines = (path(bullet, CRIMSON, 2.6)
             + path(holiday, "#B45309", 2.2, "7 4")
             + path(annuity, BLUE, 2.8)
             + path(levelpr, SLATE, 2.0, "4 4"))

    marks = (f'<circle cx="{X(7):.1f}" cy="{Y(21102406):.1f}" r="3.6" fill="{BLUE}"/>'
             f'<text x="{X(7) + 7:.1f}" y="{Y(21102406) - 8:.1f}" font-size="10" '
             f'fill="{BLUE}">21,102,406</text>'
             f'<circle cx="{X(8):.1f}" cy="{Y(21396798):.1f}" r="3.2" fill="#B45309"/>'
             f'<circle cx="{X(6):.1f}" cy="{Y(21000000):.1f}" r="3.2" fill="{SLATE}"/>'
             f'<text x="{X(6) - 6:.1f}" y="{Y(21000000) + 18:.1f}" font-size="10" '
             f'fill="{SLATE}" text-anchor="end">21,000,000 at year 6</text>')

    halfway = (f'<line x1="{L}" y1="{Y(21000000):.1f}" x2="{W - R}" y2="{Y(21000000):.1f}" '
               f'stroke="{INK}" stroke-width="0.8" stroke-dasharray="2 5"/>')

    lab = [("Bullet &#8212; balloon 42,000,000", CRIMSON, Y(42000000) - 6),
           ("Holiday, yrs 1&#8211;3 interest only", "#B45309", Y(30364153) + 4),
           ("Annuity 5,009,635", BLUE, Y(24634001) + 4),
           ("Level principal 3,500,000", SLATE, Y(21000000) + 22)]
    legend = "".join(
        f'<text x="{W - R + 8}" y="{y:.1f}" font-size="10.5" fill="{c}" '
        f'font-weight="600">{t}</text>' for t, c, y in lab)

    head = (f'<text x="{L}" y="{T - 12}" font-size="11" fill="{INK}">'
            f'Lifetime interest: level principal 16,380,000 &#183; annuity 18,115,623 '
            f'&#183; holiday 21,134,405 &#183; bullet 30,240,000</text>')

    body = (grid + halfway + lines + marks
            + axes(L, T, W - R, H - B, "Year", "Principal outstanding (USD)")
            + xticks + legend + head)
    (PFL / "fig_3_2_3.svg").write_text(svg(W, H, body))

    # ---- Fig 3.3.3 — four treatments of one stream --------------------------------
    bars = [("Nominal flows,\nnominal 9.0 %", 72791113, BLUE, "correct"),
            ("Real flows,\nreal 5.8252 %", 72791113, BLUE, "correct &#8212; identical"),
            ("Real flows,\nnominal 9.0 %", 55006446, CRIMSON, "&#8722;24.4325 %"),
            ("Nominal flows,\nreal 5.8252 %", 100366400, CRIMSON, "+37.8828 %")]

    W, H, L, R, T, B = 700, 430, 96, 30, 46, 74
    pw, ph = W - L - R, H - T - B

    def Yb(v):
        return H - B - v / 110000000.0 * ph

    grid = "".join(
        f'<line x1="{L}" y1="{Yb(v):.1f}" x2="{W - R}" y2="{Yb(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Yb(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v // 1000000}m</text>'
        for v in range(0, 120000000, 20000000))

    slot = pw / 4
    bw = slot * 0.52
    rects = ""
    for k, (name, val, colour, note) in enumerate(bars):
        x = L + k * slot + (slot - bw) / 2
        rects += (f'<rect x="{x:.1f}" y="{Yb(val):.1f}" width="{bw:.1f}" '
                  f'height="{(H - B) - Yb(val):.1f}" fill="{colour}" '
                  f'opacity="{0.92 if colour == BLUE else 0.8}"/>'
                  f'<text x="{x + bw / 2:.1f}" y="{Yb(val) - 20:.1f}" font-size="11" '
                  f'fill="{INK}" text-anchor="middle" font-weight="600">'
                  f'{val:,}</text>'
                  f'<text x="{x + bw / 2:.1f}" y="{Yb(val) - 7:.1f}" font-size="10" '
                  f'fill="{colour}" text-anchor="middle">{note}</text>')
        for j, part in enumerate(name.split("\n")):
            rects += (f'<text x="{x + bw / 2:.1f}" y="{H - B + 18 + j * 13}" font-size="10.5" '
                      f'fill="{SLATE}" text-anchor="middle">{part}</text>')

    truth = (f'<line x1="{L}" y1="{Yb(72791113):.1f}" x2="{W - R}" y2="{Yb(72791113):.1f}" '
             f'stroke="{INK}" stroke-width="1" stroke-dasharray="5 4"/>'
             f'<text x="{W - R - 4}" y="{Yb(72791113) - 8:.1f}" font-size="10.5" fill="{INK}" '
             f'text-anchor="end">the consistent answer, 72,791,113</text>')
    short = (f'<line x1="{L}" y1="{Yb(71586794):.1f}" x2="{L + slot * 2:.1f}" '
             f'y2="{Yb(71586794):.1f}" stroke="#B45309" stroke-width="1" '
             f'stroke-dasharray="2 3"/>'
             f'<text x="{L + 6}" y="{Yb(71586794) + 15:.1f}" font-size="10" fill="#B45309">'
             f'Fisher subtraction shortcut at 6.00 %: 71,586,794 (&#8722;1,204,318)</text>')

    head = (f'<text x="{L}" y="{T - 24}" font-size="11.5" fill="{INK}">'
            f'One stream: 5,600,000 a year of year-0 purchasing power for 25 years, '
            f'inflation 3.0 %, nominal hurdle 9.0 %</text>')

    body = (grid + rects + truth + short
            + axes(L, T, W - R, H - B, "Treatment", "Present value (USD)") + head)
    (PFL / "fig_3_3_3.svg").write_text(svg(W, H, body))
