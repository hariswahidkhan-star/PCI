"""PFL-AI Domain 7 — Revenue, demand and commercial models. PCI original artwork.

Fig 7.1.1  Identical expected cash, different bankability: the DSCR outcome distribution
           of a volume tariff against the single certain DSCR of an availability payment,
           both with an expected CFADS of 6,384,000, and the debt each one supports.
Fig 7.3.1  Margin drift over a 25-year concession: EBITDA margin under the contracted
           80 % / 70 % indexation split, against full revenue indexation and a fully
           indexed cost base.

Deterministic: every value below is a literal verified in the domain's arithmetic pass
(decimal.Decimal throughout; see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 7.1.1 — same expected cash, different bankability ---------------------
    # (state label, probability text, CFADS, DSCR)
    states = [
        ("low demand", "p = 0.25", 4728000, 0.9438),
        ("base demand", "p = 0.50", 6384000, 1.2743),
        ("high demand", "p = 0.25", 8040000, 1.6049),
    ]
    W, H = 700, 470
    L, R, T, B = 84, 176, 78, 88
    lo, hi = 0.80, 1.70
    plot_w = W - L - R
    n = len(states)

    def X(i):
        return L + (i + 0.5) * plot_w / n

    def Y(v):
        return H - B - (v - lo) / (hi - lo) * (H - T - B)

    ticks = [0.80, 1.00, 1.20, 1.40, 1.60]
    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W - R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v:.2f}</text>' for v in ticks)

    barw = plot_w / n * 0.44
    bars = ""
    for i, (lab, prob, cf, ds) in enumerate(states):
        x = X(i) - barw / 2
        colour = CRIMSON if ds < 1.0 else BLUE
        bars += (f'<rect x="{x:.1f}" y="{Y(ds):.1f}" width="{barw:.1f}" '
                 f'height="{(H - B) - Y(ds):.1f}" fill="{colour}" opacity="0.62"/>')
        bars += (f'<text x="{X(i):.1f}" y="{Y(ds) - 8:.1f}" font-size="11" font-weight="700" '
                 f'fill="{INK}" text-anchor="middle">{ds:.4f}</text>')
        bars += (f'<text x="{X(i):.1f}" y="{H - B + 16}" font-size="10.4" fill="{SLATE}" '
                 f'text-anchor="middle">{lab}</text>')
        bars += (f'<text x="{X(i):.1f}" y="{H - B + 30}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="middle">{prob} &#183; CFADS {cf:,}</text>')

    body = (grid + bars + axes(L, T, W - R, H - B, "Volume-tariff outcome", "DSCR")
            + f'<line x1="{L}" y1="{Y(1.2743):.1f}" x2="{W - R}" y2="{Y(1.2743):.1f}" '
              f'stroke="{INK}" stroke-width="2.4" stroke-dasharray="8 4"/>'
            + f'<line x1="{L}" y1="{Y(1.20):.1f}" x2="{W - R}" y2="{Y(1.20):.1f}" '
              f'stroke="{CRIMSON}" stroke-width="1.6" stroke-dasharray="5 4"/>'
            + f'<line x1="{L}" y1="{Y(1.00):.1f}" x2="{W - R}" y2="{Y(1.00):.1f}" '
              f'stroke="{SLATE}" stroke-width="1.4"/>'
            + f'<text x="{L + 6}" y="{Y(1.20) - 6:.1f}" font-size="10.4" font-weight="700" '
              f'fill="{CRIMSON}">1.20&#215; covenant</text>'
            + f'<text x="{L + 6}" y="{Y(1.00) + 14:.1f}" font-size="10.4" fill="{SLATE}">'
              '1.00&#215; &#8212; scheduled debt service unpaid</text>'
            + f'<text x="{W - R + 8}" y="{Y(1.2743) - 20:.1f}" font-size="10.6" '
              f'font-weight="700" fill="{INK}">availability</text>'
            + f'<text x="{W - R + 8}" y="{Y(1.2743) - 7:.1f}" font-size="10.6" '
              f'font-weight="700" fill="{INK}">payment: 1.2743</text>'
            + f'<text x="{W - R + 8}" y="{Y(1.2743) + 7:.1f}" font-size="9.8" fill="{SLATE}">'
              'with certainty &#8212; and</text>'
            + f'<text x="{W - R + 8}" y="{Y(1.2743) + 19:.1f}" font-size="9.8" fill="{SLATE}">'
              'the expected DSCR of</text>'
            + f'<text x="{W - R + 8}" y="{Y(1.2743) + 31:.1f}" font-size="9.8" fill="{SLATE}">'
              'the volume tariff too</text>'
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'The same expected CFADS of 6,384,000 &#8212; and 10,679,727 less debt'
              '</text>'
            + f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
              'Expected DSCR is 1.2743 in both structures. The lender sizes on the low case, '
              'not the mean</text>'
            + f'<text x="24" y="59" font-size="10.4" fill="{CRIMSON}">'
              'Availability: 41,171,123 of debt at 1.30&#215;. Volume tariff: 30,491,396 at '
              '1.30&#215; on the low case</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 18}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'A mean is not a coverage ratio: coverage is tested in the worst period, '
              'never in the average one</text>')
    (PFL / "fig_7_1_1.svg").write_text(svg(W, H, body))

    # ---- Fig 7.3.1 — margin drift over the concession ------------------------------
    contracted = [62.50, 62.41, 62.32, 62.22, 62.12, 62.02, 61.91, 61.79, 61.67, 61.55,
                  61.42, 61.29, 61.16, 61.02, 60.88, 60.73, 60.58, 60.42, 60.26, 60.10,
                  59.93, 59.76, 59.59, 59.41, 59.23]
    fullrev = [62.50, 62.60, 62.68, 62.76, 62.83, 62.90, 62.96, 63.00, 63.05, 63.08,
               63.11, 63.14, 63.15, 63.16, 63.16, 63.16, 63.15, 63.14, 63.12, 63.09,
               63.06, 63.02, 62.98, 62.93, 62.87]
    fullcost = [62.50, 62.06, 61.62, 61.17, 60.73, 60.28, 59.83, 59.38, 58.93, 58.48,
                58.02, 57.57, 57.11, 56.65, 56.19, 55.73, 55.27, 54.80, 54.34, 53.87,
                53.40, 52.93, 52.46, 51.98, 51.51]
    W, H = 700, 450
    L, R, T, B = 84, 168, 76, 74
    lo, hi = 50.0, 64.0
    plot_w = W - L - R

    def Xy(t):
        return L + (t - 1) / 24.0 * plot_w

    def Ym(v):
        return H - B - (v - lo) / (hi - lo) * (H - T - B)

    ticks = [50, 52, 54, 56, 58, 60, 62, 64]
    grid = "".join(
        f'<line x1="{L}" y1="{Ym(v):.1f}" x2="{W - R}" y2="{Ym(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Ym(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v} %</text>' for v in ticks)
    xt = "".join(f'<text x="{Xy(t):.1f}" y="{H - B + 16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{t}</text>' for t in (1, 5, 10, 12, 15, 20, 25))

    def poly(series):
        return " ".join(f"{Xy(t + 1):.1f},{Ym(v):.1f}" for t, v in enumerate(series))

    body = (grid + xt + axes(L, T, W - R, H - B, "Concession year", "EBITDA margin")
            + f'<line x1="{Xy(12):.1f}" y1="{T}" x2="{Xy(12):.1f}" y2="{H - B}" '
              f'stroke="{SLATE}" stroke-width="1.3" stroke-dasharray="5 4"/>'
            + f'<text x="{Xy(12) + 5:.1f}" y="{T + 14}" font-size="10" fill="{SLATE}">'
              'loan matures</text>'
            + f'<polyline points="{poly(fullrev)}" fill="none" stroke="{BLUE}" '
              f'stroke-width="2.4" stroke-dasharray="7 4"/>'
            + f'<polyline points="{poly(contracted)}" fill="none" stroke="{INK}" '
              f'stroke-width="2.8"/>'
            + f'<polyline points="{poly(fullcost)}" fill="none" stroke="{CRIMSON}" '
              f'stroke-width="2.4"/>'
            + f'<circle cx="{Xy(25):.1f}" cy="{Ym(59.23):.1f}" r="4.0" fill="{INK}"/>'
            + f'<circle cx="{Xy(25):.1f}" cy="{Ym(51.51):.1f}" r="4.0" fill="{CRIMSON}"/>'
            + f'<text x="{W - R + 8}" y="{Ym(62.87) + 4:.1f}" font-size="10.2" '
              f'font-weight="700" fill="{BLUE}">tariff fully indexed</text>'
            + f'<text x="{W - R + 8}" y="{Ym(62.87) + 17:.1f}" font-size="9.8" fill="{BLUE}">'
              '62.87 % in year 25</text>'
            + f'<text x="{W - R + 8}" y="{Ym(59.23) + 4:.1f}" font-size="10.2" '
              f'font-weight="700" fill="{INK}">as contracted</text>'
            + f'<text x="{W - R + 8}" y="{Ym(59.23) + 17:.1f}" font-size="9.8" fill="{INK}">'
              '80 % / 70 % split</text>'
            + f'<text x="{W - R + 8}" y="{Ym(59.23) + 30:.1f}" font-size="9.8" fill="{INK}">'
              '59.23 % in year 25</text>'
            + f'<text x="{W - R + 8}" y="{Ym(51.51) + 4:.1f}" font-size="10.2" '
              f'font-weight="700" fill="{CRIMSON}">cost fully indexed</text>'
            + f'<text x="{W - R + 8}" y="{Ym(51.51) + 17:.1f}" font-size="9.8" fill="{CRIMSON}">'
              '51.51 % in year 25</text>'
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'Indexation architecture, not the index, decides the margin'
              '</text>'
            + f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
              'Tariff index 2.5 %, cost index 3.2 %; effective rates 2.10 % on revenue and '
              '2.46 % on cost</text>'
            + f'<text x="24" y="59" font-size="10.4" fill="{SLATE}">'
              'The unindexed 20 % of the tariff costs 3.65 margin points by year 25; the fixed '
              '30 % of cost saves 7.72</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 14}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'A 36-basis-point gap between effective rates compounds into 3.27 margin points '
              'over 25 years</text>')
    (PFL / "fig_7_3_1.svg").write_text(svg(W, H, body))
