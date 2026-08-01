"""PFL-AI Domain 5 — Project development and bankability. PCI original artwork.

Fig 5.1.1  The development funnel as an option premium (stage counts, stage spend,
           cumulative spend, cost per close and the breakeven close count).
Fig 5.3.1  Bankability as a conjunction (six conditions and the cumulative product).

Deterministic: every value below is a literal verified in the domain's arithmetic pass.
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 5.1.1 — the development funnel ---------------------------------------
    W, H = 700, 440
    L, R, T = 150, 210, 62
    # stage, count, unit cost, stage spend, cumulative spend
    stages = [
        ("Screening", 40, "25,000", "1,000,000", "1,000,000"),
        ("Concept / prefeasibility", 12, "250,000", "3,000,000", "4,000,000"),
        ("Feasibility and bid", 5, "1,200,000", "6,000,000", "10,000,000"),
        ("To financial close", 2, "2,400,000", "4,800,000", "14,800,000"),
    ]
    band = 74
    maxw = W - L - R

    def bw(n):
        return maxw * (0.14 + 0.86 * (n / 40.0))

    body = (f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
            'Forty screened opportunities buy two financings &#8212; and the two must repay all forty'
            '</text>'
            f'<text x="24" y="44" font-size="10.4" fill="{SLATE}">'
            'Stage spend is the premium paid for the option to continue; only the last stage is spent '
            'on projects that close</text>')
    for i, (name, n, unit, spend, cum) in enumerate(stages):
        y = T + i * band
        w = bw(n)
        x = L + (maxw - w) / 2
        shade = 0.30 + 0.22 * i
        body += (f'<rect x="{x:.1f}" y="{y}" width="{w:.1f}" height="46" rx="5" fill="{BLUE}" '
                 f'opacity="{shade:.2f}"/>'
                 f'<text x="{x + w / 2:.1f}" y="{y + 21}" font-size="13" font-weight="800" '
                 f'fill="white" text-anchor="middle">{n}</text>')
        # The unit label is white, so anything overflowing the bar lands white-on-white and simply
        # vanishes. The narrowest stage ("To financial close", 2 of 40) is exactly that case: "at
        # 2,400,000 each" is wider than its bar and the reader saw "t 2,400,000 eac". When it does
        # not fit inside, it goes underneath in ink instead.
        unit_label = f"at {unit} each"
        if len(unit_label) * 9.4 * 0.5 <= w - 8:
            body += (f'<text x="{x + w / 2:.1f}" y="{y + 37}" font-size="9.4" fill="white" '
                     f'text-anchor="middle">{unit_label}</text>')
        else:
            body += (f'<text x="{x + w / 2:.1f}" y="{y + 58}" font-size="9.4" fill="{SLATE}" '
                     f'text-anchor="middle">{unit_label}</text>')
        body += (
                 f'<text x="{L - 12}" y="{y + 21}" font-size="10.8" font-weight="600" fill="{INK}" '
                 f'text-anchor="end">{name}</text>'
                 f'<text x="{L - 12}" y="{y + 36}" font-size="9.4" fill="{SLATE}" '
                 f'text-anchor="end">stage spend {spend}</text>'
                 f'<text x="{W - R + 14}" y="{y + 21}" font-size="10.4" fill="{SLATE}">'
                 f'cumulative {cum}</text>')
        if i:
            prev = stages[i - 1][1]
            body += (f'<text x="{W - R + 14}" y="{y + 36}" font-size="9.4" fill="{CRIMSON}">'
                     f'conversion {n}/{prev}</text>')
    yb = T + 4 * band + 6
    body += (f'<line x1="24" y1="{yb}" x2="{W - 24}" y2="{yb}" stroke="{GRID}" stroke-width="1.4"/>'
             f'<text x="24" y="{yb + 24}" font-size="11.6" font-weight="700" fill="{INK}">'
             'Development cost per closed project 7,400,000</text>'
             f'<text x="24" y="{yb + 41}" font-size="10.6" fill="{SLATE}">'
             'Close rate 2/40 = 5.0 %. At a sponsor value of 16,179,360 per close the programme '
             'breaks even at</text>'
             f'<text x="24" y="{yb + 56}" font-size="10.6" fill="{CRIMSON}" font-weight="700">'
             '0.9147 closes &#8212; a breakeven close rate of 2.29 %, less than half the rate '
             'achieved</text>')
    (PFL / "fig_5_1_1.svg").write_text(svg(W, H, body))

    # ---- Fig 5.3.1 — bankability as a conjunction ---------------------------------
    W, H, L, R, T, B = 700, 430, 78, 26, 52, 86
    conds = [("Offtake", "0.92", 0.92), ("Permits", "0.90", 0.90), ("Land", "0.95", 0.95),
             ("Technology", "0.88", 0.88), ("EPC wrap", "0.93", 0.93),
             ("Financing", "0.85", 0.85)]
    n = len(conds)
    plot_w = W - L - R

    def X(i):
        return L + (i + 0.5) * plot_w / n

    def Y(v):
        return H - B - v * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v / 10):.1f}" x2="{W - R}" y2="{Y(v / 10):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v / 10) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v / 10:.1f}</text>'
        for v in range(0, 11, 2))
    barw = plot_w / n * 0.44
    bars, run, pts = "", 1.0, []
    for i, (name, lab, p) in enumerate(conds):
        run *= p
        pts.append((X(i), Y(run)))
        weak = p == 0.85
        bars += (f'<rect x="{X(i) - barw:.1f}" y="{Y(p):.1f}" width="{barw:.1f}" '
                 f'height="{(H - B) - Y(p):.1f}" fill="{CRIMSON if weak else BLUE}" '
                 f'opacity="{0.85 if weak else 0.42}"/>'
                 f'<text x="{X(i) - barw / 2:.1f}" y="{Y(p) - 6:.1f}" font-size="10.2" '
                 f'font-weight="600" fill="{CRIMSON if weak else INK}" '
                 f'text-anchor="middle">{lab}</text>'
                 f'<text x="{X(i):.1f}" y="{H - B + 17}" font-size="10.4" fill="{INK}" '
                 f'text-anchor="middle">{name}</text>')
    line = " ".join(f"{x:.1f},{y:.1f}" for x, y in pts)
    body = (grid + bars + axes(L, T, W - R, H - B, "", "Probability")
            + f'<polyline points="{line}" fill="none" stroke="{INK}" stroke-width="2.6"/>'
            + "".join(f'<circle cx="{x:.1f}" cy="{y:.1f}" r="3.4" fill="{INK}"/>' for x, y in pts)
            + f'<line x1="{L}" y1="{Y(0.547190424):.1f}" x2="{W - R}" y2="{Y(0.547190424):.1f}" '
              f'stroke="{CRIMSON}" stroke-width="1.4" stroke-dasharray="6 4"/>'
            + f'<text x="{W - R - 4}" y="{Y(0.547190424) - 8:.1f}" font-size="11" '
              f'font-weight="700" fill="{CRIMSON}" text-anchor="end">joint probability of close '
              f'0.5472</text>'
            + f'<line x1="{L}" y1="{Y(0.98259234):.1f}" x2="{W - R}" y2="{Y(0.98259234):.1f}" '
              f'stroke="{SLATE}" stroke-width="1.2" stroke-dasharray="3 4"/>'
            + f'<text x="{L + 6}" y="{Y(0.98259234) - 7:.1f}" font-size="10" fill="{SLATE}">'
              'every condition would need 0.9826 for a 0.90 joint</text>'
            + f'<text x="24" y="24" font-size="12.5" font-weight="700" fill="{INK}">'
              'Six conditions averaging 90.5 % give a 54.7 % chance of financial close</text>'
            + f'<text x="24" y="41" font-size="10.4" fill="{SLATE}">'
              'Bars: each condition met on the target timetable. Line: the running product. '
              'Lifting the weakest from 0.85 to 0.95 adds 6.44 points; lifting the strongest from '
              '0.95 to 0.98 adds 1.73</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - B + 44}" font-size="11.4" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'The weakest condition governs &#8212; and no amount of strength elsewhere repairs it'
              '</text>')
    (PFL / "fig_5_3_1.svg").write_text(svg(W, H, body))
