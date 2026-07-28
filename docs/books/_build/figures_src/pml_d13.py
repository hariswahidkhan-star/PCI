"""PML-AI Domain 13 — Agile, Adaptive and Hybrid Delivery. PCI original artwork.

Fig 13.2.1  Little&#8217;s Law on Meridian&#8217;s records team. Cycle time (weeks) and throughput
            (items per week) against work in progress, under the two-regime model stated in
            KA 13.2.3: below the capacity point W* = 18 the team is starved, so throughput rises
            linearly (T = 6W/18) and cycle time sits on its floor of 3.00 weeks; above it each
            extra concurrent item costs 2 % of capacity (T = 6(1 &#8722; 0.02(W &#8722; 18))), so
            throughput falls and cycle time climbs. Markers at W = 18 (T 6.00, CT 3.00) and
            W = 30 (T 4.56, CT 6.58) &#8212; +66.7 % WIP, 0 % more throughput, +119.3 % cycle time.

Fig 13.3.1  Governance latency against flow. Average cycle time (2.40 + 0.15 &#215; E[wait]) and
            blocked work in progress as a share of the team&#8217;s WIP (0.9 &#215; E[wait] / 18)
            against E[wait] = M/2 + L from Domain 3 KA 3.2.3. Markers at Meridian&#8217;s designed
            4.0 weeks (CT 3.00, 20.0 % of WIP parked) and at the 1.0-week written-resolution route
            (CT 2.55, 5.0 %). A vertical reference at 2.0 weeks marks the point where governance
            latency equals one iteration.

All values are those computed in the manuscript with decimal arithmetic. Assumptions are the
manuscript&#8217;s stated ones (throughput 6 items per week at a WIP of 18; 0.9 weeks of active work
per item; 15 % of items requiring a decision above the product owner&#8217;s authority) and are to
be calibrated locally &#8212; the shape is the lesson.
Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""

from fractions import Fraction as F


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    def notecol(x, y0, lines):
        out = ""
        for k, (line, col, wt) in enumerate(lines):
            out += (f'<text x="{x}" y="{y0 + k * 14}" font-size="9.6" fill="{col}" '
                    f'font-weight="{wt}">{line}</text>')
        return out

    # ==================================================== Fig 13.2.1 — Little's Law
    T0, WSTAR, S = F(6), F(18), F(1, 50)          # 6 items/week at WIP 18; 2 % per extra item

    def thr(w):
        w = F(w)
        return T0 * w / WSTAR if w <= WSTAR else T0 * (1 - S * (w - WSTAR))

    def ct(w):
        return F(w) / thr(w)

    W, H, L, R, T, B = 820, 460, 88, 250, 42, 64
    WMIN, WMAX, CTMAX, TMAX = 6.0, 42.0, 14.0, 7.0

    def X(w):
        return L + (w - WMIN) / (WMAX - WMIN) * (W - R - L)

    def Yc(v):
        return (H - B) - v / CTMAX * (H - T - B)

    def Yt(v):
        return (H - B) - v / TMAX * (H - T - B)

    grid = ""
    for v in range(0, 15, 2):
        grid += (f'<line x1="{L}" y1="{Yc(v):.1f}" x2="{W-R}" y2="{Yc(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Yc(v)+4:.1f}" font-size="10" fill="{BLUE}" '
                 f'text-anchor="end">{v}</text>')
    for v in range(0, 8):
        grid += f'<text x="{W-R+7}" y="{Yt(v)+4:.1f}" font-size="10" fill="{CRIMSON}">{v}</text>'
    for w in (6, 12, 18, 24, 30, 36, 42):
        grid += (f'<text x="{X(w):.1f}" y="{H-B+17}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{w}</text>')

    ctpts = " ".join(f"{X(w):.1f},{Yc(float(ct(w))):.1f}" for w in range(6, 43))
    thpts = " ".join(f"{X(w):.1f},{Yt(float(thr(w))):.1f}" for w in range(6, 43))
    curves = (f'<polyline points="{ctpts}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
              f'<polyline points="{thpts}" fill="none" stroke="{CRIMSON}" stroke-width="2.6"/>')

    limit = (f'<line x1="{X(18):.1f}" y1="{T+34}" x2="{X(18):.1f}" y2="{Yc(1.25):.1f}" stroke="{INK}" '
             f'stroke-width="1.1" stroke-dasharray="5 4"/>'
             f'<text x="{X(18):.1f}" y="{T+14}" font-size="10.5" fill="{INK}" '
             f'text-anchor="middle" font-weight="600">capacity point W* = 18</text>'
             f'<text x="{X(18)-7:.1f}" y="{T+29}" font-size="10" fill="{SLATE}" '
             f'text-anchor="end">starved &#8592;</text>'
             f'<text x="{X(18)+7:.1f}" y="{T+29}" font-size="10" fill="{SLATE}">'
             f'&#8594; switching losses</text>')

    marks = (f'<circle cx="{X(18):.1f}" cy="{Yc(3.0):.1f}" r="4.2" fill="{BLUE}"/>'
             f'<text x="{X(18)-9:.1f}" y="{Yc(3.0)+16:.1f}" font-size="10.5" fill="{BLUE}" '
             f'text-anchor="end" font-weight="600">CT 3.00 wk</text>'
             f'<circle cx="{X(30):.1f}" cy="{Yc(float(ct(30))):.1f}" r="4.2" fill="{BLUE}"/>'
             f'<text x="{X(30)+9:.1f}" y="{Yc(float(ct(30)))+16:.1f}" font-size="10.5" '
             f'fill="{BLUE}" font-weight="600">CT 6.58 wk</text>'
             f'<circle cx="{X(18):.1f}" cy="{Yt(6):.1f}" r="4.2" fill="{CRIMSON}"/>'
             f'<text x="{X(18)-9:.1f}" y="{Yt(6)-9:.1f}" font-size="10.5" fill="{CRIMSON}" '
             f'text-anchor="end" font-weight="600">6.00 items/wk</text>'
             f'<circle cx="{X(30):.1f}" cy="{Yt(4.56):.1f}" r="4.2" fill="{CRIMSON}"/>'
             f'<text x="{X(30)-9:.1f}" y="{Yt(4.56)+16:.1f}" font-size="10.5" fill="{CRIMSON}" '
             f'text-anchor="end" font-weight="600">4.56 items/wk</text>')
    yb = Yc(1.25)
    marks += (f'<path d="M{X(18):.1f} {yb-5:.1f} L{X(18):.1f} {yb:.1f} L{X(30):.1f} {yb:.1f} '
              f'L{X(30):.1f} {yb-5:.1f}" fill="none" stroke="{SLATE}" stroke-width="1.1"/>'
              f'<text x="{(X(18)+X(30))/2:.1f}" y="{yb+14:.1f}" font-size="10.2" fill="{SLATE}" '
              f'text-anchor="middle">+66.7 % WIP bought 0 % more throughput</text>')

    keys = ""
    for k, (lab, col) in enumerate((("Cycle time, weeks (left)", BLUE),
                                    ("Throughput, items/week", CRIMSON))):
        y = T + 8 + k * 19
        keys += (f'<line x1="{W-R+48}" y1="{y}" x2="{W-R+70}" y2="{y}" stroke="{col}" '
                 f'stroke-width="2.6"/>'
                 f'<text x="{W-R+75}" y="{y+4}" font-size="9.5" fill="{INK}">{lab}</text>')
    keys += notecol(W - R + 48, T + 64, (
        ("Raising WIP from 18 to 30:", INK, 600),
        ("throughput 6.00 &#8594; 4.56", CRIMSON, 600),
        ("(&#8722;24.0 %); cycle time", CRIMSON, 400),
        ("3.00 &#8594; 6.58 wk (+119.3 %).", CRIMSON, 600),
        ("The 240-item backlog then", SLATE, 400),
        ("takes 52.63 weeks, not 40.00", SLATE, 400),
        ("&#8212; USD 180,379 at Meridian&#8217;s", CRIMSON, 600),
        ("14,280 per week of delay.", CRIMSON, 400),
        ("This model reaches zero", SLATE, 400),
        ("throughput at WIP 68, which", SLATE, 400),
        ("is a warning: calibrate the", INK, 600),
        ("switching cost, do not", INK, 600),
        ("extrapolate the line.", INK, 600)))

    body = (grid + curves + limit + marks + keys
            + axes(L, T, W - R, H - B, "Work in progress, W (items being worked at once)",
                   "Cycle time (weeks)"))
    (PML / "fig_13_2_1.svg").write_text(svg(W, H, body))

    # ============================================ Fig 13.3.1 — governance latency vs flow
    CU, SH, TP, WIP = F(12, 5), F(3, 20), F(6), F(18)   # 2.40 wk unblocked; 15 %; 6/wk; WIP 18

    def avg_ct(ew):
        return CU + SH * ew

    def blocked_share(ew):
        return SH * TP * ew / WIP

    W, H, L, R, T, B = 820, 440, 88, 250, 42, 64
    EMAX, CTLO, CTHI, SHMAX = 10.0, 2.0, 4.2, 55.0

    def X2(e):
        return L + e / EMAX * (W - R - L)

    def Y2c(v):
        return (H - B) - (v - CTLO) / (CTHI - CTLO) * (H - T - B)

    def Y2s(v):
        return (H - B) - v / SHMAX * (H - T - B)

    g2 = ""
    for v in (2.0, 2.4, 2.8, 3.2, 3.6, 4.0):
        g2 += (f'<line x1="{L}" y1="{Y2c(v):.1f}" x2="{W-R}" y2="{Y2c(v):.1f}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Y2c(v)+4:.1f}" font-size="10" fill="{BLUE}" '
               f'text-anchor="end">{v:.1f}</text>')
    for v in (0, 10, 20, 30, 40, 50):
        g2 += f'<text x="{W-R+7}" y="{Y2s(v)+4:.1f}" font-size="10" fill="{CRIMSON}">{v} %</text>'
    for e in range(0, 11, 2):
        g2 += (f'<text x="{X2(e):.1f}" y="{H-B+17}" font-size="10" fill="{SLATE}" '
               f'text-anchor="middle">{e}</text>')

    cpts = " ".join(f"{X2(e/2):.1f},{Y2c(float(avg_ct(F(e, 2)))):.1f}" for e in range(0, 21))
    spts = " ".join(f"{X2(e/2):.1f},{Y2s(float(blocked_share(F(e, 2))) * 100):.1f}"
                    for e in range(0, 21))
    c2 = (f'<polyline points="{cpts}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
          f'<polyline points="{spts}" fill="none" stroke="{CRIMSON}" stroke-width="2.6" '
          f'stroke-dasharray="7 4"/>')

    iterline = (f'<line x1="{X2(2):.1f}" y1="{T+22}" x2="{X2(2):.1f}" y2="{H-B}" stroke="{SLATE}" '
                f'stroke-width="1.1" stroke-dasharray="4 4"/>'
                f'<text x="{X2(2):.1f}" y="{T+14}" font-size="10.2" fill="{SLATE}" '
                f'text-anchor="middle">E[wait] = one iteration</text>')

    m2 = (f'<circle cx="{X2(4):.1f}" cy="{Y2c(3.0):.1f}" r="4.2" fill="{BLUE}"/>'
          f'<text x="{X2(4):.1f}" y="{Y2c(3.0)-11:.1f}" font-size="10.5" fill="{BLUE}" '
          f'text-anchor="middle" font-weight="600">3.00 wk</text>'
          f'<circle cx="{X2(4):.1f}" cy="{Y2s(20.0):.1f}" r="4.2" fill="{CRIMSON}"/>'
          f'<text x="{X2(4)+9:.1f}" y="{Y2s(20.0)+15:.1f}" font-size="10.5" fill="{CRIMSON}" '
          f'font-weight="600">20.0 % of WIP parked</text>'
          f'<circle cx="{X2(1):.1f}" cy="{Y2c(2.55):.1f}" r="4.2" fill="{BLUE}"/>'
          f'<text x="{X2(1)+9:.1f}" y="{Y2c(2.55)+4:.1f}" font-size="10.5" fill="{BLUE}" '
          f'font-weight="600">2.55 wk</text>'
          f'<circle cx="{X2(1):.1f}" cy="{Y2s(5.0):.1f}" r="4.2" fill="{CRIMSON}"/>'
          f'<text x="{X2(1)+9:.1f}" y="{Y2s(5.0)+15:.1f}" font-size="10.5" fill="{CRIMSON}" '
          f'font-weight="600">5.0 %</text>')
    m2 += (f'<line x1="{X2(1.9):.1f}" y1="{Y2c(3.62):.1f}" x2="{X2(1.05):.1f}" '
           f'y2="{Y2c(2.63):.1f}" stroke="{INK}" stroke-width="1.3" marker-end="url(#d13ar)"/>'
           f'<text x="{X2(2.05):.1f}" y="{Y2c(3.66):.1f}" font-size="10.4" fill="{INK}" '
           f'font-weight="600">a 1-week out-of-cycle route: cycle time &#8722;15.0 %,</text>'
           f'<text x="{X2(2.05):.1f}" y="{Y2c(3.51):.1f}" font-size="10.4" fill="{INK}" '
           f'font-weight="600">throughput +17.6 % at unchanged WIP</text>')

    k2 = ""
    for k, (lab, col, dash) in enumerate((("Average cycle time, weeks", BLUE, ""),
                                          ("Blocked WIP, % of team WIP", CRIMSON,
                                           ' stroke-dasharray="7 4"'))):
        y = T + 8 + k * 19
        k2 += (f'<line x1="{W-R+48}" y1="{y}" x2="{W-R+70}" y2="{y}" stroke="{col}" '
               f'stroke-width="2.6"{dash}/>'
               f'<text x="{W-R+75}" y="{y+4}" font-size="9.5" fill="{INK}">{lab}</text>')
    k2 += notecol(W - R + 48, T + 64, (
        ("Meridian as designed:", INK, 600),
        ("M = 4, L = 2 &#8594; E[wait] 4.0 wk", INK, 400),
        ("(Domain 3, KA 3.2.3) &#8212; two", SLATE, 400),
        ("whole iterations of waiting.", SLATE, 400),
        ("3.60 items parked = 20.0 %", CRIMSON, 600),
        ("of the team&#8217;s WIP. Blocked", CRIMSON, 400),
        ("items take 6.40 weeks against", CRIMSON, 400),
        ("2.40 for the rest &#8212; 2.667&#215;.", CRIMSON, 600),
        ("Both lines are linear in", SLATE, 400),
        ("E[wait], so the whole cost of", SLATE, 400),
        ("a governance cadence is read", INK, 600),
        ("off one horizontal axis.", INK, 600)))

    defs = ('<defs><marker id="d13ar" viewBox="0 0 10 10" refX="9" refY="5" markerWidth="7" '
            'markerHeight="7" orient="auto"><path d="M0 0L10 5L0 10z" fill="context-stroke"/>'
            '</marker></defs>')
    body2 = (defs + g2 + c2 + iterline + m2 + k2
             + axes(L, T, W - R, H - B,
                    "Governance latency E[wait] = M/2 + L (weeks)",
                    "Average cycle time (weeks)"))
    (PML / "fig_13_3_1.svg").write_text(svg(W, H, body2))
