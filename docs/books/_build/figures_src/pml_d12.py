"""PML-AI Domain 12 — Leadership, Teams and Organizational Behaviour. PCI original artwork.

Fig 12.2.1  coordination overhead as a share of team capacity: the fully connected identity
            (n &minus; 1)/160 rising to 50 % at 81 people, against a pods-of-eight structured
            design that stays between 4.375 % and 5.156 %. Markers at Auriga's 6 (3.125 %),
            Meridian's original 12 (6.875 %) and Meridian's scaled 40 (24.375 % mesh against
            4.6875 % in pods, a release of 7.875 FTE).

Fig 12.3.1  coaching minutes per report against span of control: the naive curve 6.5/n and the
            honest curve (9.5 &minus; 0.5n)/n that recognises the leader's fixed load growing
            with the team, against a 45-minute sufficiency convention. Sustainable span 7 on the
            honest curve (threshold 7.6) against 8 on the naive one (threshold 8.667); coaching
            capacity reaches exactly zero at 19 reports.

All values are those computed in the manuscript with decimal arithmetic (link cost 0.5 hours of
total team time per pairwise link per week; 40-hour weeks; residual 6.5 discretionary hours;
0.5 hours of additional fixed load per report above six).
Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""

from fractions import Fraction as F


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ============================================================ Fig 12.2.1
    # share(n) = c * n(n-1)/2 / (h * n) with c = 0.5 h per link per week, h = 40 h.
    #          = (n - 1)/160  for the fully connected case.
    # pods of s = 8: links = (n/s) * s(s-1)/2 + (n/s)(n/s - 1)/2
    C, HRS, PODS = F(1, 2), F(40), 8

    def mesh(n):
        return C * F(n * (n - 1), 2) / (HRS * F(n))

    def podlinks(n):
        p = n // PODS
        return F(p * PODS * (PODS - 1), 2) + F(p * (p - 1), 2)

    def pod(n):
        return C * podlinks(n) / (HRS * F(n))

    W, H, L, R, T, B = 710, 440, 84, 176, 36, 62
    NMAX, SMAX = 90.0, 55.0

    def X(n):
        return L + (n - 2) / (NMAX - 2) * (W - R - L)

    def Y(s):
        return (H - B) - s / SMAX * (H - T - B)

    grid = ""
    for s in range(0, 56, 5):
        grid += (f'<line x1="{L}" y1="{Y(s):.1f}" x2="{W-R}" y2="{Y(s):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Y(s)+4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="end">{s} %</text>')
    for n in (2, 10, 20, 30, 40, 50, 60, 70, 80, 90):
        grid += (f'<text x="{X(n):.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{n}</text>')

    meshpts = " ".join(f"{X(n):.1f},{Y(float(mesh(n)) * 100):.1f}" for n in range(2, 91))
    podpts = " ".join(f"{X(n):.1f},{Y(float(pod(n)) * 100):.1f}"
                      for n in range(PODS, 89, PODS))
    curves = (f'<polyline points="{meshpts}" fill="none" stroke="{CRIMSON}" stroke-width="2.6"/>'
              f'<polyline points="{podpts}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>')

    # The half-capacity line and the 81-person crossing.
    half = (f'<line x1="{L}" y1="{Y(50):.1f}" x2="{W-R}" y2="{Y(50):.1f}" stroke="{INK}" '
            f'stroke-width="1" stroke-dasharray="5 4"/>'
            f'<text x="{L+8}" y="{Y(50)-7:.1f}" font-size="10.5" fill="{INK}">'
            f'half the team&#8217;s gross hours</text>'
            f'<circle cx="{X(81):.1f}" cy="{Y(50):.1f}" r="4.2" fill="{CRIMSON}"/>'
            f'<text x="{X(81)-8:.1f}" y="{Y(50)+16:.1f}" font-size="10.5" fill="{CRIMSON}" '
            f'text-anchor="end" font-weight="600">n = 81: the 81st member adds nothing</text>')

    marks = ""
    for n, lab, col, dx, dy, anch in (
            (6, "Auriga 6 &#8594; 3.125 %", INK, 6, -9, "start"),
            (12, "Meridian 12 &#8594; 6.875 %", INK, 8, -8, "start"),
            (40, "40 in one mesh &#8594; 24.375 %", CRIMSON, 8, -9, "start")):
        marks += (f'<circle cx="{X(n):.1f}" cy="{Y(float(mesh(n))*100):.1f}" r="3.8" fill="{col}"/>'
                  f'<text x="{X(n)+dx:.1f}" y="{Y(float(mesh(n))*100)+dy:.1f}" font-size="10.2" '
                  f'fill="{col}" text-anchor="{anch}">{lab}</text>')
    marks += (f'<circle cx="{X(40):.1f}" cy="{Y(float(pod(40))*100):.1f}" r="3.8" fill="{BLUE}"/>'
              f'<text x="{X(40)+8:.1f}" y="{Y(float(pod(40))*100)+15:.1f}" font-size="10.2" '
              f'fill="{BLUE}">40 in five pods of 8 &#8594; 4.6875 %</text>'
              f'<line x1="{X(40):.1f}" y1="{Y(float(pod(40))*100):.1f}" x2="{X(40):.1f}" '
              f'y2="{Y(float(mesh(40))*100):.1f}" stroke="{SLATE}" stroke-width="1.1" '
              f'stroke-dasharray="3 3"/>'
              f'<text x="{X(40)-7:.1f}" y="{(Y(float(pod(40))*100)+Y(float(mesh(40))*100))/2:.1f}" '
              f'font-size="10.2" fill="{SLATE}" text-anchor="end" font-weight="600">'
              f'7.875 FTE</text>')

    keys = ""
    for k, (lab, col) in enumerate((("Fully connected  (n &#8722; 1)/160", CRIMSON),
                                    ("Five-to-eleven pods of 8", BLUE))):
        y = T + 8 + k * 19
        keys += (f'<line x1="{W-R+10}" y1="{y}" x2="{W-R+32}" y2="{y}" stroke="{col}" '
                 f'stroke-width="2.6"/>'
                 f'<text x="{W-R+37}" y="{y+4}" font-size="9.5" fill="{INK}">{lab}</text>')
    keys += (f'<text x="{W-R+10}" y="{T+66}" font-size="10" fill="{SLATE}">Link cost 0.5 h of</text>'
             f'<text x="{W-R+10}" y="{T+79}" font-size="10" fill="{SLATE}">team time per week;</text>'
             f'<text x="{W-R+10}" y="{T+92}" font-size="10" fill="{SLATE}">40-hour weeks.</text>'
             f'<text x="{W-R+10}" y="{T+112}" font-size="10" fill="{SLATE}">Calibrate the link</text>'
             f'<text x="{W-R+10}" y="{T+125}" font-size="10" fill="{CRIMSON}" font-weight="600">'
             f'cost locally &#8212; the</text>'
             f'<text x="{W-R+10}" y="{T+138}" font-size="10" fill="{CRIMSON}" font-weight="600">'
             f'shape is the lesson.</text>')

    body = (grid + curves + half + marks + keys
            + axes(L, T, W - R, H - B, "Team size n (people working as one team)",
                   "Coordination overhead, % of team capacity"))
    (PML / "fig_12_2_1.svg").write_text(svg(W, H, body))

    # ============================================================ Fig 12.3.1
    # naive(n)  = 6.5/n hours            -> minutes
    # honest(n) = (9.5 - 0.5n)/n hours   -> minutes  (fixed load grows 0.5 h per report above 6)
    def naive(n):
        return F(13, 2) / F(n) * 60

    def honest(n):
        return (F(19, 2) - F(1, 2) * F(n)) / F(n) * 60

    W, H, L, R, T, B = 700, 430, 86, 168, 36, 62
    NLO, NHI, MMAX = 4, 16, 120.0

    def Xn(n):
        return L + (n - NLO) / (NHI - NLO) * (W - R - L)

    def Ym(v):
        return (H - B) - v / MMAX * (H - T - B)

    grid = ""
    for v in range(0, 121, 15):
        grid += (f'<line x1="{L}" y1="{Ym(v):.1f}" x2="{W-R}" y2="{Ym(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Ym(v)+4:.1f}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="end">{v}</text>')
    for n in range(NLO, NHI + 1):
        grid += (f'<text x="{Xn(n):.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{n}</text>')

    npts = " ".join(f"{Xn(n):.1f},{Ym(float(naive(n))):.1f}" for n in range(NLO, NHI + 1))
    hpts = " ".join(f"{Xn(n):.1f},{Ym(max(0.0, float(honest(n)))):.1f}"
                    for n in range(NLO, NHI + 1))
    curves = (f'<polyline points="{npts}" fill="none" stroke="{SLATE}" stroke-width="2.3" '
              f'stroke-dasharray="7 4"/>'
              f'<polyline points="{hpts}" fill="none" stroke="{BLUE}" stroke-width="2.7"/>')

    # 45-minute sufficiency convention and the two thresholds.
    rule = (f'<line x1="{L}" y1="{Ym(45):.1f}" x2="{W-R}" y2="{Ym(45):.1f}" stroke="{CRIMSON}" '
            f'stroke-width="1.4" stroke-dasharray="6 4"/>'
            f'<text x="{L+8}" y="{Ym(45)-7:.1f}" font-size="10.5" fill="{CRIMSON}" '
            f'font-weight="600">45 minutes &#8212; the stated sufficiency convention</text>')
    for n, lab, col, dy in ((7.6, "7.6", BLUE, -8), (8.6667, "8.67", SLATE, -22)):
        rule += (f'<line x1="{Xn(n):.1f}" y1="{T}" x2="{Xn(n):.1f}" y2="{H-B}" stroke="{col}" '
                 f'stroke-width="1.1" stroke-dasharray="3 3"/>'
                 f'<text x="{Xn(n)+5:.1f}" y="{H-B+dy:.1f}" font-size="10" fill="{col}">'
                 f'{lab}</text>')

    pts = ""
    for n, val, lab, col, dx, dy, anch in (
            (6, 65.0, "6 reports &#8594; 65 min", INK, 7, -10, "start"),
            (7, 51.4286, "7 &#8594; 51.4 min (the last span that passes)", BLUE, 8, -8, "start"),
            (8, 41.25, "8 &#8594; 41.25 min", CRIMSON, 8, 14, "start"),
            (11, 21.8182, "11 &#8594; 21.8 min", CRIMSON, 7, -9, "start")):
        pts += (f'<circle cx="{Xn(n):.1f}" cy="{Ym(val):.1f}" r="4.0" fill="{col}"/>'
                f'<text x="{Xn(n)+dx:.1f}" y="{Ym(val)+dy:.1f}" font-size="10.2" fill="{col}" '
                f'text-anchor="{anch}">{lab}</text>')
    pts += (f'<circle cx="{Xn(19 if NHI >= 19 else 16):.1f}" cy="{Ym(0):.1f}" r="0" fill="none"/>'
            f'<text x="{Xn(16)-4:.1f}" y="{Ym(0)-9:.1f}" font-size="10.2" fill="{BLUE}" '
            f'text-anchor="end">reaches zero at 19 reports</text>')

    keys = ""
    for k, (lab, col, dash) in enumerate((
            ("Honest: fixed load grows with span", BLUE, ""),
            ("Naive: residual held at 6.5 h", SLATE, ' stroke-dasharray="6 3"'))):
        y = T + 8 + k * 19
        keys += (f'<line x1="{W-R+10}" y1="{y}" x2="{W-R+32}" y2="{y}" stroke="{col}" '
                 f'stroke-width="2.5"{dash}/>'
                 f'<text x="{W-R+37}" y="{y+4}" font-size="9.4" fill="{INK}">{lab}</text>')
    keys += (f'<text x="{W-R+10}" y="{T+66}" font-size="10" fill="{SLATE}">Ignoring the growth</text>'
             f'<text x="{W-R+10}" y="{T+79}" font-size="10" fill="{SLATE}">of the leader&#8217;s own</text>'
             f'<text x="{W-R+10}" y="{T+92}" font-size="10" fill="{SLATE}">fixed load overstates</text>'
             f'<text x="{W-R+10}" y="{T+105}" font-size="10" fill="{CRIMSON}" font-weight="600">'
             f'the sustainable span</text>'
             f'<text x="{W-R+10}" y="{T+118}" font-size="10" fill="{CRIMSON}" font-weight="600">'
             f'by one report.</text>')

    body = (grid + curves + rule + pts + keys
            + axes(L, T, W - R, H - B, "Span of control (direct reports)",
                   "Coaching minutes per report per week"))
    (PML / "fig_12_3_1.svg").write_text(svg(W, H, body))
