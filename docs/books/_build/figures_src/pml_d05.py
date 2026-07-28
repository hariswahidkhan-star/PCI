"""PML-AI Domain 5 — Scope, requirements and value definition. PCI original artwork.

Fig 5.2.1  the requirement-defect correction ladder (logarithmic), with the elicitation breakeven.
Fig 5.3.1  greedy value-per-effort ranking against enumeration under a 20-week capacity constraint.

Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""
import math


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ---------------------------------------------------------------- Fig 5.2.1
    # Correction cost of one requirement defect by the stage at which it is found.
    # Meridian's own observed ladder: 400 / 1,600 / 6,400 / 25,600 / 102,400 (x1 x4 x16 x64 x256).
    stages = [("Definition", 400, "&#215;1"), ("Design", 1600, "&#215;4"),
              ("Build", 6400, "&#215;16"), ("Test &amp; integration", 25600, "&#215;64"),
              ("Live service", 102400, "&#215;256")]
    W, H, L, R, T, B = 660, 420, 84, 150, 30, 62
    lo, hi = math.log10(200.0), math.log10(200000.0)

    def Yl(v):
        return (H - B) - (math.log10(v) - lo) / (hi - lo) * (H - T - B)

    grid = ""
    for tick, lab in ((200, "200"), (1000, "1,000"), (10000, "10,000"),
                      (100000, "100,000"), (200000, "200,000")):
        grid += (f'<line x1="{L}" y1="{Yl(tick):.1f}" x2="{W-R}" y2="{Yl(tick):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Yl(tick)+4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="end">{lab}</text>')
    slot = (W - R - L) / len(stages)
    bw = slot * 0.52
    bars = ""
    for k, (name, val, mult) in enumerate(stages):
        x = L + k * slot + (slot - bw) / 2
        y = Yl(val)
        fill = CRIMSON if k == len(stages) - 1 else BLUE
        op = "1" if k == len(stages) - 1 else "0.88"
        bars += (f'<rect x="{x:.1f}" y="{y:.1f}" width="{bw:.1f}" height="{(H-B)-y:.1f}" '
                 f'fill="{fill}" opacity="{op}"/>'
                 f'<text x="{x+bw/2:.1f}" y="{y-16:.1f}" font-size="11" fill="{INK}" '
                 f'text-anchor="middle" font-weight="600">{val:,}</text>'
                 f'<text x="{x+bw/2:.1f}" y="{y-4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{mult}</text>'
                 f'<text x="{x+bw/2:.1f}" y="{H-B+16:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{name}</text>')
    # Breakeven annotation: the 84,000 elicitation programme against one avoided live defect.
    ymark = Yl(84000)
    note = (f'<line x1="{L}" y1="{ymark:.1f}" x2="{W-R+8}" y2="{ymark:.1f}" stroke="{INK}" '
            f'stroke-width="1" stroke-dasharray="5 4"/>'
            f'<text x="{W-R+12}" y="{ymark-4:.1f}" font-size="10.5" fill="{INK}">'
            f'Elicitation spend</text>'
            f'<text x="{W-R+12}" y="{ymark+9:.1f}" font-size="10.5" fill="{INK}" '
            f'font-weight="600">USD 84,000</text>'
            f'<text x="{W-R+12}" y="{ymark+28:.1f}" font-size="10" fill="{CRIMSON}">'
            f'One live defect moved</text>'
            f'<text x="{W-R+12}" y="{ymark+41:.1f}" font-size="10" fill="{CRIMSON}">'
            f'to definition saves</text>'
            f'<text x="{W-R+12}" y="{ymark+54:.1f}" font-size="10" fill="{CRIMSON}" '
            f'font-weight="600">102,000 &#8212; pays for it</text>'
            f'<text x="{L+4}" y="{T+12}" font-size="11" fill="{SLATE}">'
            f'Logarithmic scale &#8212; each stage costs four times the last</text>')
    body = (grid + bars + note
            + axes(L, T, W - R, H - B, "Stage at which the requirement defect is found",
                   "Correction cost, USD per defect (log)"))
    (PML / "fig_5_2_1.svg").write_text(svg(W, H, body))

    # ---------------------------------------------------------------- Fig 5.3.1
    # Greedy value-per-effort ranking vs enumeration, 20 development-weeks of Release 1 capacity.
    W, H, L, R, T, B = 660, 360, 128, 132, 44, 58
    CAP = 20
    span = W - R - L
    rows = [
        ("Greedy ranking", [("A  unified record", 13, 299000, BLUE),
                            ("D  reporting", 5, 110000, BLUE)], 2, 409000, "18 of 20 weeks"),
        ("Enumeration", [("B  e-prescribing", 10, 228000, BLUE),
                         ("C  appointments", 10, 225000, BLUE)], 0, 453000, "20 of 20 weeks"),
    ]
    bh = 54
    body = ""
    for r, (label, segs, idle, total, util) in enumerate(rows):
        y = T + 22 + r * (bh + 56)
        x = L
        body += (f'<text x="{L-12}" y="{y+bh/2+4:.1f}" font-size="12" fill="{INK}" '
                 f'text-anchor="end" font-weight="600">{label}</text>')
        for name, wk, val, col in segs:
            w = wk / CAP * span
            body += (f'<rect x="{x:.1f}" y="{y}" width="{w:.1f}" height="{bh}" fill="{col}" '
                     f'opacity="0.9" stroke="white" stroke-width="1.5"/>'
                     f'<text x="{x+w/2:.1f}" y="{y+22}" font-size="10.5" fill="white" '
                     f'text-anchor="middle" font-weight="600">{name}</text>'
                     f'<text x="{x+w/2:.1f}" y="{y+37}" font-size="10" fill="white" '
                     f'text-anchor="middle">{wk} wk &#183; {val:,}</text>')
            x += w
        if idle:
            w = idle / CAP * span
            body += (f'<rect x="{x:.1f}" y="{y}" width="{w:.1f}" height="{bh}" fill="none" '
                     f'stroke="{CRIMSON}" stroke-width="1.4" stroke-dasharray="4 3"/>'
                     f'<text x="{x+w/2:.1f}" y="{y+bh+15}" font-size="10" fill="{CRIMSON}" '
                     f'text-anchor="middle">{idle} wk idle</text>')
        body += (f'<text x="{W-R+12}" y="{y+20}" font-size="12" fill="{INK}" font-weight="600">'
                 f'{total:,}</text>'
                 f'<text x="{W-R+12}" y="{y+35}" font-size="10" fill="{SLATE}">per year</text>'
                 f'<text x="{W-R+12}" y="{y+49}" font-size="10" fill="{SLATE}">{util}</text>')
    body += (f'<line x1="{L}" y1="{T+8}" x2="{L}" y2="{T+22+2*bh+56}" stroke="{INK}" '
             f'stroke-width="1.2"/>'
             f'<line x1="{W-R}" y1="{T+8}" x2="{W-R}" y2="{T+22+2*bh+56}" stroke="{INK}" '
             f'stroke-width="1.2" stroke-dasharray="4 4"/>'
             f'<text x="{W-R}" y="{T-2}" font-size="11" fill="{INK}" text-anchor="end">'
             f'Release 1 capacity: 20 development-weeks</text>'
             f'<text x="{L+span/2:.1f}" y="{H-30}" font-size="11.5" fill="{CRIMSON}" '
             f'text-anchor="middle" font-weight="600">'
             f'Enumeration is worth +44,000 a year &#8212; 10.76 % more benefit</text>'
             f'<text x="{L+span/2:.1f}" y="{H-14}" font-size="11" fill="{SLATE}" '
             f'text-anchor="middle">'
             f'and USD 262,737 of present value over eight years at 7 %</text>')
    (PML / "fig_5_3_1.svg").write_text(svg(W, H, body))
