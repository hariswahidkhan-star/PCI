"""PML-AI Domain 9 — Quality, assurance and continuous improvement. PCI original artwork.

Fig 9.1.1  Auriga's cost of quality across five quality regimes: prevention, appraisal, internal
           failure and external failure stacked, with the interior total-cost minimum marked and
           the range of optimality on the external-failure unit cost annotated.
Fig 9.4.1  Rolled throughput yield: the compounding of six sequential first-time-right yields from
           100 % to 55.31 %, against the arithmetic mean of the same six yields (90.67 %).

Every value here is reproduced by the domain's verification script and printed in the manuscript.
Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ------------------------------------------------------------------ Fig 9.1.1
    # (name, prevention, appraisal, internal failure, external failure, escapes)
    regimes = [
        ("R0", "test at the end", 12000, 48000, 120000, 480000, 40),
        ("R1", "basic", 40000, 60000, 132600, 216000, 18),
        ("R2", "planned", 96000, 64000, 108000, 96000, 8),
        ("R3", "enhanced", 170000, 80000, 99000, 48000, 4),
        ("R4", "maximum", 285000, 100000, 84000, 24000, 2),
    ]
    W, H, L, R, T, B = 680, 470, 92, 172, 34, 78
    TOPV = 700000.0

    def Y(v):
        return (H - B) - v / TOPV * (H - T - B)

    grid = ""
    for v in range(0, 700001, 100000):
        grid += (f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W-R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Y(v)+4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="end">{v//1000}k</text>')

    bands = [("Prevention", BLUE, "1"), ("Appraisal", BLUE, "0.55"),
             ("Internal failure", CRIMSON, "0.45"), ("External failure", CRIMSON, "1")]
    slot = (W - R - L) / len(regimes)
    bw = slot * 0.48
    bars, tops = "", []
    for k, (code, label, p, a, itf, ext, esc) in enumerate(regimes):
        x = L + k * slot + (slot - bw) / 2
        run = 0.0
        for val, (bname, col, op) in zip((p, a, itf, ext), bands):
            y0, y1 = Y(run + val), Y(run)
            bars += (f'<rect x="{x:.1f}" y="{y0:.1f}" width="{bw:.1f}" height="{y1-y0:.1f}" '
                     f'fill="{col}" opacity="{op}" stroke="white" stroke-width="0.8"/>')
            run += val
        total = p + a + itf + ext
        tops.append((x + bw / 2, Y(total), total))
        bars += (f'<text x="{x+bw/2:.1f}" y="{Y(total)-22:.1f}" font-size="11.5" fill="{INK}" '
                 f'text-anchor="middle" font-weight="700">{total:,}</text>'
                 f'<text x="{x+bw/2:.1f}" y="{Y(total)-9:.1f}" font-size="9.5" fill="{SLATE}" '
                 f'text-anchor="middle">{esc} escaped</text>'
                 f'<text x="{x+bw/2:.1f}" y="{H-B+16:.1f}" font-size="11" fill="{INK}" '
                 f'text-anchor="middle" font-weight="600">{code}</text>'
                 f'<text x="{x+bw/2:.1f}" y="{H-B+29:.1f}" font-size="9.5" fill="{SLATE}" '
                 f'text-anchor="middle">{label}</text>')

    line = " ".join(f"{cx:.1f},{cy:.1f}" for cx, cy, _ in tops)
    mx, my, mtot = tops[2]
    marker = (f'<polyline points="{line}" fill="none" stroke="{INK}" stroke-width="1.6" '
              f'stroke-dasharray="5 4"/>'
              f'<circle cx="{mx:.1f}" cy="{my:.1f}" r="5" fill="none" stroke="{INK}" '
              f'stroke-width="2"/>'
              f'<line x1="{mx:.1f}" y1="{my+10:.1f}" x2="{mx:.1f}" y2="{H-B:.1f}" stroke="{INK}" '
              f'stroke-width="1" stroke-dasharray="3 3"/>')

    leg = ""
    for i, (bname, col, op) in enumerate(bands):
        ly = T + 6 + i * 16
        leg += (f'<rect x="{W-R+12}" y="{ly}" width="11" height="11" fill="{col}" opacity="{op}"/>'
                f'<text x="{W-R+28}" y="{ly+9}" font-size="10.5" fill="{INK}">{bname}</text>')
    note = (f'<text x="{W-R+12}" y="{T+96}" font-size="10.5" fill="{INK}" font-weight="700">'
            f'Minimum: R2</text>'
            f'<text x="{W-R+12}" y="{T+110}" font-size="10.5" fill="{INK}">'
            f'USD {mtot:,} &#8212; 9.1 % of BAC</text>'
            f'<text x="{W-R+12}" y="{T+130}" font-size="10" fill="{CRIMSON}">'
            f'Not at zero defects:</text>'
            f'<text x="{W-R+12}" y="{T+143}" font-size="10" fill="{CRIMSON}">'
            f'8 escapes is optimal</text>'
            f'<text x="{W-R+12}" y="{T+163}" font-size="10" fill="{SLATE}">'
            f'Optimal for external</text>'
            f'<text x="{W-R+12}" y="{T+176}" font-size="10" fill="{SLATE}">'
            f'unit cost 3,540&#8211;20,250</text>'
            f'<text x="{W-R+12}" y="{T+189}" font-size="10" fill="{SLATE}">'
            f'(assumed 12,000)</text>'
            f'<text x="{L+6}" y="{T+13}" font-size="10.5" fill="{SLATE}">'
            f'Internal failure peaks at R1: better detection finds more before it prevents more'
            f'</text>')
    body = (grid + bars + marker + leg + note
            + axes(L, T, W - R, H - B, "", "Cost of quality, USD")
            + f'<text x="{(L+W-R)/2:.1f}" y="{H-B+51:.1f}" font-size="12" fill="{SLATE}" '
              f'text-anchor="middle">Quality regime (rising prevention and appraisal spend)</text>')
    (PML / "fig_9_1_1.svg").write_text(svg(W, H, body))

    # ------------------------------------------------------------------ Fig 9.4.1
    steps = [("Design package issue", 0.95), ("Supplier document review", 0.92),
             ("Factory acceptance", 0.90), ("Site installation", 0.94),
             ("Site acceptance test", 0.88), ("Handover documentation", 0.85)]
    W, H, L, R, T, B = 680, 430, 92, 168, 40, 96

    def Yp(v):
        return (H - B) - (v - 40.0) / 62.0 * (H - T - B)

    grid = ""
    for v in range(40, 105, 10):
        grid += (f'<line x1="{L}" y1="{Yp(v):.1f}" x2="{W-R}" y2="{Yp(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Yp(v)+4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="end">{v} %</text>')

    span = (W - R - L) / len(steps)
    cum, pts, marks = 100.0, [(L, Yp(100.0))], ""
    for k, (name, y) in enumerate(steps):
        cum *= y
        cx = L + (k + 1) * span
        pts.append((cx, Yp(cum)))
        marks += (f'<circle cx="{cx:.1f}" cy="{Yp(cum):.1f}" r="3.6" fill="{BLUE}"/>'
                  f'<text x="{cx:.1f}" y="{Yp(cum)-10:.1f}" font-size="10" fill="{INK}" '
                  f'text-anchor="middle" font-weight="600">{cum:.2f}</text>'
                  f'<line x1="{cx:.1f}" y1="{H-B:.1f}" x2="{cx:.1f}" y2="{H-B+5:.1f}" '
                  f'stroke="{SLATE}"/>'
                  f'<text x="{cx:.1f}" y="{H-B+18:.1f}" font-size="9.5" fill="{SLATE}" '
                  f'text-anchor="middle">{y:.2f}</text>')
        words = name.split(" ")
        half = (len(words) + 1) // 2
        marks += (f'<text x="{cx:.1f}" y="{H-B+32:.1f}" font-size="9" fill="{SLATE}" '
                  f'text-anchor="middle">{" ".join(words[:half])}</text>'
                  f'<text x="{cx:.1f}" y="{H-B+43:.1f}" font-size="9" fill="{SLATE}" '
                  f'text-anchor="middle">{" ".join(words[half:])}</text>')
    poly = " ".join(f"{x:.1f},{y:.1f}" for x, y in pts)
    mean = (f'<line x1="{L}" y1="{Yp(90.6667):.1f}" x2="{W-R}" y2="{Yp(90.6667):.1f}" '
            f'stroke="{SLATE}" stroke-width="1.4" stroke-dasharray="6 4"/>'
            f'<text x="{L+8}" y="{Yp(90.6667)-6:.1f}" font-size="10.5" fill="{SLATE}">'
            f'Arithmetic mean of the six step yields &#8212; 90.67 %</text>')
    end = (f'<line x1="{W-R}" y1="{Yp(55.3074):.1f}" x2="{W-R+10}" y2="{Yp(55.3074):.1f}" '
           f'stroke="{CRIMSON}" stroke-width="1.4"/>'
           f'<text x="{W-R+14}" y="{Yp(55.3074)-4:.1f}" font-size="11" fill="{CRIMSON}" '
           f'font-weight="700">55.31 %</text>'
           f'<text x="{W-R+14}" y="{Yp(55.3074)+10:.1f}" font-size="10" fill="{CRIMSON}">'
           f'first time right</text>'
           f'<text x="{W-R+14}" y="{Yp(55.3074)+23:.1f}" font-size="10" fill="{SLATE}">'
           f'end to end</text>'
           f'<text x="{W-R+14}" y="{T+18}" font-size="10" fill="{INK}">Fix the weakest</text>'
           f'<text x="{W-R+14}" y="{T+31}" font-size="10" fill="{INK}">step (0.85 &#8594; 0.95):</text>'
           f'<text x="{W-R+14}" y="{T+45}" font-size="10.5" fill="{INK}" font-weight="700">'
           f'61.81 % (+6.51 pp)</text>'
           f'<text x="{W-R+14}" y="{T+63}" font-size="10" fill="{SLATE}">Fix the strongest</text>'
           f'<text x="{W-R+14}" y="{T+76}" font-size="10" fill="{SLATE}">(0.95 &#8594; 0.98):</text>'
           f'<text x="{W-R+14}" y="{T+89}" font-size="10" fill="{SLATE}">57.05 % (+1.75 pp)</text>'
           f'<text x="{W-R+14}" y="{T+107}" font-size="10" fill="{CRIMSON}">3.73&#215; the gain,</text>'
           f'<text x="{W-R+14}" y="{T+120}" font-size="10" fill="{CRIMSON}">same effort</text>')
    body = (grid + mean
            + f'<polyline points="{poly}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
            + marks + end
            + axes(L, T, W - R, H - B, "", "Cumulative first-time-right yield")
            + f'<text x="{(L+W-R)/2:.1f}" y="{H-B+64:.1f}" font-size="12" fill="{SLATE}" '
              f'text-anchor="middle">Sequential step (its own first-time-right yield beneath)'
              f'</text>')
    (PML / "fig_9_4_1.svg").write_text(svg(W, H, body))
