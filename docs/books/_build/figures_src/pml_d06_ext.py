"""PML-AI Domain 6 (extension) — Planning, scheduling and delivery flow. PCI original artwork.

Fig 6.2.2  Auriga's three paths drawn to length, with each path's float and the near-critical band
           at 10 % of project duration (2.50 weeks), showing that two of the three paths — and six
           of the seven activities — fall inside it.
Fig 6.4.2  The compression menu and the least-cost duration: net saving against project duration,
           peaking at 22 weeks (+USD 25,000), with the marginal cost of each bought week labelled
           against the USD 45,000 value of a week.

Every value here is printed in the manuscript and reproduced by the domain's verification script.
Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ------------------------------------------------------------------ Fig 6.2.2
    # (label, activities, length, float)
    paths = [
        ("A&#8211;B&#8211;C&#8211;E&#8211;F", "2+6+8+5+4", 25, 0),
        ("A&#8211;B&#8211;D&#8211;E&#8211;F", "2+6+7+5+4", 24, 1),
        ("A&#8211;B&#8211;D&#8211;G", "2+6+7+2", 17, 8),
    ]
    W, H, L, R, T, B = 690, 330, 132, 168, 40, 66
    XMAX = 26.0

    def X(v):
        return L + v / XMAX * (W - L - R)

    grid = ""
    for v in range(0, 27, 2):
        grid += (f'<line x1="{X(v):.1f}" y1="{T}" x2="{X(v):.1f}" y2="{H-B}" stroke="{GRID}"/>'
                 f'<text x="{X(v):.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{v}</text>')

    slot = (H - B - T) / len(paths)
    bh = slot * 0.46
    bars = ""
    for k, (label, sums, length, fl) in enumerate(paths):
        y = T + k * slot + (slot - bh) / 2
        col = BLUE if fl <= 2.5 else SLATE
        op = "1" if fl == 0 else ("0.55" if fl <= 2.5 else "0.32")
        inside = "white" if fl == 0 else INK
        bars += (f'<rect x="{L}" y="{y:.1f}" width="{X(length)-L:.1f}" height="{bh:.1f}" '
                 f'fill="{col}" opacity="{op}"/>'
                 f'<text x="{L-10}" y="{y+bh*0.45:.1f}" font-size="11" fill="{INK}" '
                 f'text-anchor="end" font-weight="600">{label}</text>'
                 f'<text x="{L-10}" y="{y+bh*0.45+13:.1f}" font-size="9.5" fill="{SLATE}" '
                 f'text-anchor="end">{sums}</text>'
                 f'<text x="{X(length)-10:.1f}" y="{y+bh*0.66:.1f}" font-size="11" '
                 f'fill="{inside}" text-anchor="end" font-weight="600">'
                 f'{length} wk &#183; float {fl}</text>')

    band_x = X(22.5)
    band = (f'<rect x="{band_x:.1f}" y="{T}" width="{X(25)-band_x:.1f}" height="{H-B-T}" '
            f'fill="{BLUE}" opacity="0.10"/>'
            f'<line x1="{band_x:.1f}" y1="{T-8}" x2="{band_x:.1f}" y2="{H-B}" stroke="{BLUE}" '
            f'stroke-width="1.4" stroke-dasharray="5 4"/>'
            f'<line x1="{X(25):.1f}" y1="{T-8}" x2="{X(25):.1f}" y2="{H-B}" stroke="{INK}" '
            f'stroke-width="1.6"/>'
            f'<text x="{X(25):.1f}" y="{T-13}" font-size="10.5" fill="{INK}" text-anchor="end" '
            f'font-weight="700">project 25 wk</text>'
            f'<text x="{band_x:.1f}" y="{H-B+34}" font-size="10" fill="{BLUE}" '
            f'text-anchor="middle" font-weight="600">22.50 &#8212; 10 % band</text>')

    note = (f'<text x="{W-R+14}" y="{T+8}" font-size="10.5" fill="{INK}" font-weight="700">'
            f'Inside the band</text>'
            f'<text x="{W-R+14}" y="{T+23}" font-size="10.5" fill="{INK}">2 of 3 paths</text>'
            f'<text x="{W-R+14}" y="{T+38}" font-size="10.5" fill="{INK}">6 of 7 activities</text>'
            f'<text x="{W-R+14}" y="{T+53}" font-size="10.5" fill="{INK}">= 85.71 %</text>'
            f'<text x="{W-R+14}" y="{T+80}" font-size="10" fill="{SLATE}">Any band at or</text>'
            f'<text x="{W-R+14}" y="{T+94}" font-size="10" fill="{SLATE}">above 4.00 % of</text>'
            f'<text x="{W-R+14}" y="{T+108}" font-size="10" fill="{SLATE}">25 wk captures</text>'
            f'<text x="{W-R+14}" y="{T+122}" font-size="10" fill="{SLATE}">A&#8211;B&#8211;D&#8211;E&#8211;F</text>')

    body = (grid + band + bars + note
            + f'<line x1="{L}" y1="{H-B}" x2="{W-R}" y2="{H-B}" stroke="{INK}" stroke-width="1.2"/>'
            + f'<text x="{(L+W-R)/2:.1f}" y="{H-B+50}" font-size="12" fill="{SLATE}" '
              f'text-anchor="middle">Path length (weeks)</text>')
    (PML / "fig_6_2_2.svg").write_text(svg(W, H, body))

    # ------------------------------------------------------------------ Fig 6.4.2
    # (duration, cumulative crash cost, net saving, marginal cost of the week just bought)
    steps = [
        (25, 0, 0, None),
        (24, 30000, 15000, 30000),
        (23, 70000, 20000, 40000),
        (22, 110000, 25000, 40000),
        (21, 165000, 15000, 55000),
        (20, 230000, -5000, 65000),
        (19, 295000, -25000, 65000),
    ]
    W, H, L, R, T, B = 660, 430, 92, 168, 46, 82
    LO, HI = -30000.0, 30000.0

    def X2(d):
        return L + (25 - d) / 6.0 * (W - L - R)

    def Y2(v):
        return (H - B) - (v - LO) / (HI - LO) * (H - T - B)

    grid = ""
    for v in range(-30000, 30001, 10000):
        grid += (f'<line x1="{L}" y1="{Y2(v):.1f}" x2="{W-R}" y2="{Y2(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Y2(v)+4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="end">{v//1000}k</text>')
    zero = (f'<line x1="{L}" y1="{Y2(0):.1f}" x2="{W-R}" y2="{Y2(0):.1f}" stroke="{INK}" '
            f'stroke-width="1.1"/>')

    pts = " ".join(f"{X2(d):.1f},{Y2(net):.1f}" for d, _, net, _ in steps)
    marks = ""
    for d, cum, net, marg in steps:
        marks += (f'<circle cx="{X2(d):.1f}" cy="{Y2(net):.1f}" r="3.6" fill="{BLUE}"/>'
                  f'<text x="{X2(d):.1f}" y="{H-B+17}" font-size="11" fill="{INK}" '
                  f'text-anchor="middle" font-weight="600">{d}</text>'
                  f'<text x="{X2(d):.1f}" y="{H-B+31}" font-size="9" fill="{SLATE}" '
                  f'text-anchor="middle">{cum//1000}k</text>')
        if marg is not None:
            mx = (X2(d) + X2(d + 1)) / 2
            col = CRIMSON if marg > 45000 else BLUE
            marks += (f'<text x="{mx:.1f}" y="{T+14:.1f}" font-size="10" fill="{col}" '
                      f'text-anchor="middle" font-weight="600">{marg//1000}k</text>')

    peak = (f'<circle cx="{X2(22):.1f}" cy="{Y2(25000):.1f}" r="7.5" fill="none" stroke="{INK}" '
            f'stroke-width="2"/>'
            f'<line x1="{X2(22):.1f}" y1="{Y2(25000)+11:.1f}" x2="{X2(22):.1f}" '
            f'y2="{Y2(0):.1f}" stroke="{INK}" stroke-width="1" stroke-dasharray="3 3"/>'
            f'<text x="{X2(22)+12:.1f}" y="{Y2(25000)+22:.1f}" font-size="11" fill="{INK}" '
            f'font-weight="700">least-cost: 22 wk, +25,000</text>')

    note = (f'<text x="{W-R+14}" y="{T+16}" font-size="10" fill="{SLATE}">Marginal cost of</text>'
            f'<text x="{W-R+14}" y="{T+29}" font-size="10" fill="{SLATE}">each bought week,</text>'
            f'<text x="{W-R+14}" y="{T+42}" font-size="10" fill="{SLATE}">above the axis</text>'
            f'<text x="{W-R+14}" y="{T+66}" font-size="10.5" fill="{INK}" font-weight="700">'
            f'A week is worth</text>'
            f'<text x="{W-R+14}" y="{T+81}" font-size="10.5" fill="{INK}" font-weight="700">'
            f'USD 45,000</text>'
            f'<text x="{W-R+14}" y="{T+100}" font-size="10" fill="{BLUE}">blue step &#8804; 45k:</text>'
            f'<text x="{W-R+14}" y="{T+113}" font-size="10" fill="{BLUE}">buy it</text>'
            f'<text x="{W-R+14}" y="{T+132}" font-size="10" fill="{CRIMSON}">crimson step &gt; 45k:</text>'
            f'<text x="{W-R+14}" y="{T+145}" font-size="10" fill="{CRIMSON}">stop</text>'
            f'<text x="{W-R+14}" y="{T+170}" font-size="10" fill="{SLATE}">Optimal at 22 wk for</text>'
            f'<text x="{W-R+14}" y="{T+183}" font-size="10" fill="{SLATE}">any week value</text>'
            f'<text x="{W-R+14}" y="{T+196}" font-size="10" fill="{SLATE}">40,000&#8211;55,000</text>')

    body = (grid + zero
            + axes(L, T, W - R, H - B, "", "Net saving vs the 25-week plan (USD)")
            + f'<text x="{(L+W-R)/2:.1f}" y="{H-B+54}" font-size="12" fill="{SLATE}" '
              f'text-anchor="middle">Project duration (weeks), with cumulative crash cost '
              f'beneath each point</text>'
            + f'<polyline points="{pts}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
            + marks + peak + note)
    (PML / "fig_6_4_2.svg").write_text(svg(W, H, body))
