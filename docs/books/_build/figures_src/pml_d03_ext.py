"""PML-AI Domain 3 — Governance, Organization and Decision Rights (depth extension).
PCI original artwork.

Fig 3.1.1  Why unanimity is a power law (KA 3.1.2). Expected wait for one decision against the
           number of parties on a board meeting every 4 weeks with a 2-week paper lead time, so
           that E[wait] = M/2 + L + (1/p**n - 1)M. Logarithmic vertical axis. Two crimson
           unanimity curves — p = 0.90 giving 4.9383 / 5.4870 / 6.0966 / 6.7740 / 7.5267 /
           8.3630 weeks for n = 2..7, and p = 0.70 giving 8.1633 / 11.6618 / 16.6597 / 23.7996 /
           33.9994 / 48.5706 — against two flat brand-blue reference rules: a single integrating
           authority at 4.0000 weeks and a two-of-three majority at 4.1152 weeks (p = 0.90) and
           5.1020 weeks (p = 0.70). Three-party points ringed and priced at USD 14,280 per week.

Fig 3.1.2  What share of an iterative stream a periodic committee can serve (KA 3.1.3). Grouped
           columns for three governance designs — E[wait] 4.0 / 3.0 / 2.0 weeks — split into
           servable in time (12.3077 % / 23.0769 % / 36.9231 %) and unservable (87.6923 % /
           76.9231 % / 63.0769 %) of Meridian's 65 above-authority decisions. A crimson rule at
           63.0769 % marks the minimum envelope coverage the same-increment decisions require.
           Side panel prints the need-by distribution 41 / 9 / 7 / 8 and the upper-bound wait cost
           USD 813,960 at the as-designed latency.

Fig 3.3.2  The conditional pass: how closure rate decides a gate's value (KA 3.3.1). The straight
           line net = &#8722;130,680 + 187,200 r, whose slope is P(defect) x q x (build fix &#8722;
           design fix) = 0.30 x 0.80 x 780,000, crossing zero at r = 69.8077 %. Marked points at
           r = 0 (&#8722;130,680), Meridian's observed 9 of 23 = 39.1304 % (&#8722;57,427.83),
           17 of 23 = 73.9130 % (+7,685.22) and r = 1 (+56,520).

All values are those computed in the manuscript with decimal arithmetic. Deterministic output: no
randomness, no timestamps, no locale-dependent formatting.
"""

import math


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # =========================================================== Fig 3.1.1 — unanimity power law
    W, H, L, R, T, B = 660, 420, 92, 152, 30, 58
    UN90 = (4.9383, 5.4870, 6.0966, 6.7740, 7.5267, 8.3630)
    UN70 = (8.1633, 11.6618, 16.6597, 23.7996, 33.9994, 48.5706)
    NS = (2, 3, 4, 5, 6, 7)
    YLO, YHI = 3.0, 60.0

    def X(n):
        return L + (n - 2) / 5 * (W - R - L)

    def Y(v):
        f = (math.log10(v) - math.log10(YLO)) / (math.log10(YHI) - math.log10(YLO))
        return (H - B) - f * (H - B - T)

    body = ""
    for v in (3, 4, 5, 6, 8, 10, 15, 20, 30, 40, 60):
        body += (f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W-R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Y(v)+3.6:.1f}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="end">{v}</text>')
    for n in NS:
        body += (f'<text x="{X(n):.1f}" y="{H-B+17}" font-size="10.4" fill="{SLATE}" '
                 f'text-anchor="middle">{n}</text>')
    body += axes(L, T, W - R, H - B, "Number of parties, n", "Expected wait (weeks, log scale)")

    # flat reference rules
    for val, col, dash, lab in ((4.0000, BLUE, "5 4", "single integrating authority 4.0000 w"),
                                (4.1152, BLUE, "2 3", "two-of-three majority, p 0.90 &#8212; 4.1152 w"),
                                (5.1020, BLUE, "2 3", "two-of-three majority, p 0.70 &#8212; 5.1020 w")):
        body += (f'<line x1="{L}" y1="{Y(val):.1f}" x2="{W-R}" y2="{Y(val):.1f}" stroke="{col}" '
                 f'stroke-width="1.6" stroke-dasharray="{dash}"/>'
                 f'<text x="{W-R+6}" y="{Y(val)+3.4:.1f}" font-size="8.8" fill="{BLUE}">{lab}</text>')

    for series, lab in ((UN90, "unanimity, p 0.90"), (UN70, "unanimity, p 0.70")):
        pts = " ".join(f"{X(n):.1f},{Y(v):.1f}" for n, v in zip(NS, series))
        body += f'<polyline points="{pts}" fill="none" stroke="{CRIMSON}" stroke-width="2.5"/>'
        for n, v in zip(NS, series):
            body += f'<circle cx="{X(n):.1f}" cy="{Y(v):.1f}" r="2.6" fill="{CRIMSON}"/>'
        body += (f'<text x="{W-R+6}" y="{Y(series[-1])+3.4:.1f}" font-size="8.8" fill="{CRIMSON}" '
                 f'font-weight="600">{lab}</text>')

    for v, cost in ((5.4870, "78,353.91"), (11.6618, "166,530.61")):
        body += (f'<circle cx="{X(3):.1f}" cy="{Y(v):.1f}" r="6.2" fill="none" stroke="{CRIMSON}" '
                 f'stroke-width="1.4"/>'
                 f'<text x="{X(3)+10:.1f}" y="{Y(v)-7:.1f}" font-size="9.6" fill="{INK}">'
                 f'3 parties: {v:.4f} w = USD {cost}</text>')
    body += (f'<text x="{L+6}" y="{T+13}" font-size="9.6" fill="{SLATE}">'
             f'E[wait] = M/2 + L + (1/p&#8319; &#8722; 1)M, with M = 4 and L = 2</text>')
    (PML / "fig_3_1_1.svg").write_text(svg(W, H, body))

    # ================================================== Fig 3.1.2 — servable share of a stream
    W, H, L, R, T, B = 660, 420, 92, 176, 30, 62
    DESIGNS = (("monthly, 2-week papers", "E[wait] 4.0 w", 12.3077, 87.6923),
               ("monthly, 1-week papers", "E[wait] 3.0 w", 23.0769, 76.9231),
               ("fortnightly, 1-week papers", "E[wait] 2.0 w", 36.9231, 63.0769))

    def Yp(pct):
        return (H - B) - pct / 100 * (H - B - T)

    body = ""
    for pct in range(0, 101, 20):
        body += (f'<line x1="{L}" y1="{Yp(pct):.1f}" x2="{W-R}" y2="{Yp(pct):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Yp(pct)+3.6:.1f}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="end">{pct} %</text>')
    colw = (W - R - L) / len(DESIGNS)
    bw = colw * 0.44
    for k, (name, lat, serv, uns) in enumerate(DESIGNS):
        cx = L + (k + 0.5) * colw
        x = cx - bw / 2
        body += (f'<rect x="{x:.1f}" y="{Yp(100):.1f}" width="{bw:.1f}" '
                 f'height="{Yp(serv)-Yp(100):.1f}" fill="{SLATE}" opacity="0.42"/>'
                 f'<rect x="{x:.1f}" y="{Yp(serv):.1f}" width="{bw:.1f}" '
                 f'height="{(H-B)-Yp(serv):.1f}" fill="{BLUE}"/>'
                 f'<text x="{cx:.1f}" y="{Yp(serv)-6:.1f}" font-size="10.4" fill="{INK}" '
                 f'text-anchor="middle" font-weight="600">{serv:.2f} %</text>'
                 f'<text x="{cx:.1f}" y="{Yp(100)+18:.1f}" font-size="9.6" fill="{INK}" '
                 f'text-anchor="middle">{uns:.2f} % unservable</text>'
                 f'<text x="{cx:.1f}" y="{H-B+17}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="middle">{name}</text>'
                 f'<text x="{cx:.1f}" y="{H-B+29}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="middle">{lat}</text>')
    body += axes(L, T, W - R, H - B, "", "Share of the 65 above-authority decisions")
    body += (f'<line x1="{L}" y1="{Yp(63.0769):.1f}" x2="{W-R}" y2="{Yp(63.0769):.1f}" '
             f'stroke="{CRIMSON}" stroke-width="1.8" stroke-dasharray="6 4"/>'
             f'<text x="{W-R+6}" y="{Yp(63.0769)-5:.1f}" font-size="9" fill="{CRIMSON}" '
             f'font-weight="600">minimum envelope</text>'
             f'<text x="{W-R+6}" y="{Yp(63.0769)+6:.1f}" font-size="9" fill="{CRIMSON}" '
             # Two decimals, matching the caption and every other percentage in this chart. At
             # four it printed "63.0769 %" for the crimson rule while the column beside it printed
             # "63.08 %" for the same quantity — one picture stating one number two ways, and the
             # comparison the reader is asked to make is exactly between those two marks.
             f'font-weight="600">coverage 63.08 %</text>')
    panel = ("Need-by times of the 65 decisions",
             "&#8804; 2 weeks (same increment) &#8212; 41",
             "2&#8211;3 weeks &#8212; 9",
             "3&#8211;4 weeks &#8212; 7",
             "&gt; 4 weeks &#8212; 8",
             "",
             "Upper-bound wait cost at 4.0 w:",
             "57 &#215; 0.25 &#215; 4 &#215; 14,280",
             "= USD 813,960")
    for i, line in enumerate(panel):
        weight = ' font-weight="600"' if i in (0, 8) else ""
        body += (f'<text x="{W-R+6}" y="{T+96+i*13.4:.1f}" font-size="9" fill="{INK}"'
                 f'{weight}>{line}</text>')
    (PML / "fig_3_1_2.svg").write_text(svg(W, H, body))

    # ============================================ Fig 3.3.2 — conditional pass and gate value
    W, H, L, R, T, B = 660, 420, 96, 40, 30, 58
    VLO, VHI = -150000.0, 75000.0

    def Xr(r):
        return L + r * (W - R - L)

    def Yv(v):
        return (H - B) - (v - VLO) / (VHI - VLO) * (H - B - T)

    body = ""
    for v in range(-150000, 75001, 25000):
        body += (f'<line x1="{L}" y1="{Yv(v):.1f}" x2="{W-R}" y2="{Yv(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Yv(v)+3.6:.1f}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="end">{v//1000}k</text>')
    for pct in range(0, 101, 20):
        body += (f'<text x="{Xr(pct/100):.1f}" y="{H-B+17}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="middle">{pct} %</text>')
    # value-destroying band
    body += (f'<rect x="{L}" y="{Yv(0):.1f}" width="{W-R-L}" height="{(H-B)-Yv(0):.1f}" '
             f'fill="{CRIMSON}" opacity="0.06"/>'
             f'<text x="{L+10}" y="{(H-B)-10:.1f}" font-size="9.6" fill="{CRIMSON}">'
             f'the gate costs more than it returns</text>')
    body += axes(L, T, W - R, H - B, "Condition-closure rate, r", "Gate net value (USD)")
    body += (f'<line x1="{L}" y1="{Yv(0):.1f}" x2="{W-R}" y2="{Yv(0):.1f}" stroke="{INK}" '
             f'stroke-width="1" stroke-dasharray="4 4"/>')
    body += (f'<line x1="{Xr(0):.1f}" y1="{Yv(-130680):.1f}" x2="{Xr(1):.1f}" '
             f'y2="{Yv(56520):.1f}" stroke="{BLUE}" stroke-width="2.6"/>')
    body += (f'<line x1="{Xr(0.698077):.1f}" y1="{T}" x2="{Xr(0.698077):.1f}" '
             f'y2="{H-B}" stroke="{INK}" stroke-width="1" stroke-dasharray="3 3"/>'
             f'<text x="{Xr(0.698077)+6:.1f}" y="{T+13}" font-size="9.6" fill="{INK}" '
             f'font-weight="600">breakeven r = 69.8077 %</text>')
    MARKS = ((0.0, -130680.0, "r = 0 &#8212; &#8722;130,680", 10, -8, SLATE),
             (0.391304, -57427.83, "9 of 23 closed &#8212; &#8722;57,427.83", 10, 16, CRIMSON),
             (0.739130, 7685.22, "17 of 23 &#8212; +7,685.22", -6, -10, INK),
             (1.0, 56520.0, "r = 1 &#8212; +56,520", -6, -10, INK))
    for r, v, lab, dx, dy, col in MARKS:
        anchor = "end" if dx < 0 else "start"
        body += (f'<circle cx="{Xr(r):.1f}" cy="{Yv(v):.1f}" r="4.2" fill="{col}"/>'
                 f'<text x="{Xr(r)+dx:.1f}" y="{Yv(v)+dy:.1f}" font-size="9.6" fill="{col}" '
                 f'text-anchor="{anchor}" font-weight="600">{lab}</text>')
    body += (f'<text x="{L+10}" y="{T+13}" font-size="9.6" fill="{SLATE}">'
             f'q_eff = q &#215; r; slope 0.30 &#215; 0.80 &#215; 780,000 = 187,200; '
             f'breakeven detection 55.8462 %</text>')
    (PML / "fig_3_3_2.svg").write_text(svg(W, H, body))
