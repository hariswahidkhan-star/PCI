"""PML-AI Domain 16 — Transition, Closeout and Benefits Realisation. PCI original artwork.

Fig 16.1.1  Readiness is a conjunction, not an average (KA 16.1.3). Seven go-live readiness
            conditions for a Meridian clinic, each with its assessed probability of being met,
            drawn as grey columns on a 0&#8211;100 % scale; a brand-blue step line shows the
            running conjunction &#8719;p falling 96.00 &#8594; 86.40 &#8594; 81.22 &#8594; 79.59
            &#8594; 67.65 &#8594; 54.12 &#8594; 49.79 %. A crimson dashed rule marks the
            equal-weight dashboard average of 90.71 %, and the 40.92-percentage-point gap between
            the two is labelled. A right-hand panel ranks the gain in P(clean go-live) from lifting
            each single condition to 0.98 &#8212; champion +11.20 pp, workflow +7.62 pp, data
            +4.43 pp, fallback +3.25 pp, interfaces +2.12 pp, training +1.04 pp &#8212; showing
            that the gain tracks the proportional shortfall 0.98/p, not the absolute gap. A footer
            gives the two robustness anchors: all seven at 0.98 yields only 86.81 %, and a 95 %
            clean go-live needs 99.27 % on every condition.

Fig 16.4.1  The closing account of Meridian Care Records (KA 16.4.2). Waterfall bridge in NPV
            terms, USD, 8 years at 7 %, from the approved business case&#8217;s flat claim
            (+3,447,096) to the realised position and the post-project improvement plan:
            adoption-term correction &#8722;2,114,198 &#8594; honest ramped case +1,332,898
            &#8594; level variance &#8722;465,015 &#8594; timing variance &#8722;413,809 &#8594;
            operating cost omitted from the case &#8722;644,900 &#8594; cost outturn variance
            &#8722;114,000 &#8594; realised NPV &#8722;304,827 &#8594; improvement plan
            +340,156 &#8594; +35,329. Subtotal columns are solid brand blue, decrements crimson,
            the single increment blue-outlined; a zero rule crosses the plot so the realised
            column&#8217;s position below it is unmistakable.

All values are those computed in the manuscript with decimal arithmetic (discount rate 7 %,
AF(0.07,8) = 5.971299). The readiness probabilities, the USD 85/hour valuation rate, the
USD 24,000 remediation cost and the USD 108,000 annual run cost are locally calibrated or measured
planning figures, not constants &#8212; the manuscript says so at each use. Deterministic output:
no randomness, no timestamps, no locale-dependent formatting.
"""

from fractions import Fraction as F


def make(ctx):
    svg, PML = ctx["svg"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ================================================= Fig 16.1.1 — readiness as a conjunction
    COND = (("Clinician", "training", F(96, 100)),
            ("Data", "migration", F(90, 100)),
            ("Production", "interfaces", F(94, 100)),
            ("Network and", "devices", F(98, 100)),
            ("Workflow", "sign&#8722;off", F(85, 100)),
            ("Clinical", "champion", F(80, 100)),
            ("Fallback", "rehearsed", F(92, 100)))

    W, H = 800, 540
    L, R, T, B = 68, 250, 74, 128           # right band reserved for the ranking panel
    PW = W - L - R
    PH = H - T - B
    n = len(COND)
    step = PW / n

    def Y(v):                                # v is a fraction of 1.0
        return T + PH - float(v) * PH

    prod = F(1)
    running = []
    for _, _, p in COND:
        prod *= p
        running.append(prod)
    avg = sum(p for _, _, p in COND) / n

    body = ""
    # horizontal grid + percentage scale
    for v in range(0, 101, 20):
        y = Y(F(v, 100))
        body += (f'<line x1="{L}" y1="{y:.1f}" x2="{L+PW:.1f}" y2="{y:.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{y+4:.1f}" font-size="10.2" fill="{SLATE}" '
                 f'text-anchor="end">{v} %</text>')

    # condition columns
    bw = step * 0.52
    for k, (l1, l2, p) in enumerate(COND):
        cx = L + (k + 0.5) * step
        y = Y(p)
        body += (f'<rect x="{cx-bw/2:.1f}" y="{y:.1f}" width="{bw:.1f}" '
                 f'height="{T+PH-y:.1f}" fill="{SLATE}" opacity="0.28"/>'
                 f'<text x="{cx:.1f}" y="{y-6:.1f}" font-size="10.2" fill="{SLATE}" '
                 f'text-anchor="middle">{float(p)*100:.0f}</text>'
                 f'<text x="{cx:.1f}" y="{T+PH+16:.1f}" font-size="9.8" fill="{INK}" '
                 f'text-anchor="middle">{l1}</text>'
                 f'<text x="{cx:.1f}" y="{T+PH+28:.1f}" font-size="9.8" fill="{INK}" '
                 f'text-anchor="middle">{l2}</text>')

    # running conjunction step line
    pts = []
    for k, v in enumerate(running):
        cx = L + (k + 0.5) * step
        pts.append((cx, Y(v)))
    poly = " ".join(f"{x:.1f},{y:.1f}" for x, y in pts)
    body += f'<polyline points="{poly}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
    for k, (x, y) in enumerate(pts):
        body += f'<circle cx="{x:.1f}" cy="{y:.1f}" r="3.4" fill="{BLUE}"/>'
        if k in (0, 4, 5, 6):
            body += (f'<text x="{x+7:.1f}" y="{y-7:.1f}" font-size="10.2" fill="{BLUE}" '
                     f'font-weight="600">{float(running[k])*100:.2f}</text>')

    # dashboard average rule
    ya = Y(avg)
    body += (f'<line x1="{L}" y1="{ya:.1f}" x2="{L+PW:.1f}" y2="{ya:.1f}" stroke="{CRIMSON}" '
             f'stroke-width="1.4" stroke-dasharray="6 4"/>'
             f'<text x="{L+4}" y="{ya-7:.1f}" font-size="10.4" fill="{CRIMSON}" '
             f'font-weight="600">dashboard average {float(avg)*100:.2f} % &#8212; '
             f'&#8220;amber&#8211;green&#8221;</text>')

    # the gap
    xg = L + PW - 16
    yb = Y(running[-1])
    body += (f'<line x1="{xg:.1f}" y1="{ya:.1f}" x2="{xg:.1f}" y2="{yb:.1f}" stroke="{INK}" '
             f'stroke-width="1"/>'
             f'<line x1="{xg-5:.1f}" y1="{ya:.1f}" x2="{xg+5:.1f}" y2="{ya:.1f}" stroke="{INK}" '
             f'stroke-width="1"/>'
             f'<line x1="{xg-5:.1f}" y1="{yb:.1f}" x2="{xg+5:.1f}" y2="{yb:.1f}" stroke="{INK}" '
             f'stroke-width="1"/>'
             f'<text x="{xg-9:.1f}" y="{(ya+yb)/2:.1f}" font-size="10.4" fill="{INK}" '
             f'text-anchor="end" font-weight="600">gap 40.92 pp</text>')

    body += (f'<text x="{L}" y="{T-46}" font-size="13.4" fill="{INK}" font-weight="700">'
             f'Readiness is a conjunction, not an average</text>'
             f'<text x="{L}" y="{T-28}" font-size="10.6" fill="{SLATE}">Seven go&#8722;live '
             f'conditions for one Meridian clinic: assessed probability per condition (columns) '
             f'against the running</text>'
             f'<text x="{L}" y="{T-15}" font-size="10.6" fill="{SLATE}">product '
             f'&#8719;p (line). P(clean go&#8722;live) = 49.79 %.</text>')

    # ---- ranking panel: gain from lifting each condition to 0.98
    px = L + PW + 34
    body += (f'<text x="{px}" y="{T-28}" font-size="10.8" fill="{INK}" font-weight="700">'
             f'Gain in P(clean go&#8722;live)</text>'
             f'<text x="{px}" y="{T-15}" font-size="10.2" fill="{SLATE}">from lifting one '
             f'condition to 0.98</text>')
    gains = []
    for l1, l2, p in COND:
        if p >= F(98, 100):
            continue
        newc = running[-1] * F(98, 100) / p
        gains.append((f"{l1} {l2}", newc - running[-1], F(98, 100) / p))
    gains.sort(key=lambda g: -g[1])
    gmax = float(gains[0][1])
    for k, (lab, g, fac) in enumerate(gains):
        gy = T + 14 + k * 30
        gw = float(g) / gmax * 128.0
        body += (f'<text x="{px}" y="{gy:.1f}" font-size="9.9" fill="{INK}">{lab}'
                 f'<tspan fill="{SLATE}"> &#215;{float(fac):.4f}</tspan></text>'
                 f'<rect x="{px}" y="{gy+4:.1f}" width="{gw:.1f}" height="10" fill="{BLUE}" '
                 f'opacity="0.82"/>'
                 f'<text x="{px+gw+5:.1f}" y="{gy+13:.1f}" font-size="9.9" fill="{BLUE}" '
                 f'font-weight="600">+{float(g)*100:.2f} pp</text>')

    body += (f'<line x1="{L}" y1="{H-B+52}" x2="{W-40}" y2="{H-B+52}" stroke="{GRID}"/>'
             f'<text x="{L}" y="{H-B+70}" font-size="10.3" fill="{INK}">All seven conditions at '
             f'0.98 gives a conjunction of only <tspan font-weight="600">86.81 %</tspan>; a 95 % '
             f'chance of a clean go&#8722;live requires '
             f'<tspan font-weight="600">99.27 %</tspan> on every one of the seven.</text>'
             f'<text x="{L}" y="{H-B+85}" font-size="10.3" fill="{SLATE}">Independence is an '
             f'assumption stated, not proved: perfectly correlated failures give the optimistic '
             f'bound min(p) = 80.00 %. Both bounds are used in the hold&#8722;or&#8722;go '
             f'decision.</text>'
             f'<text x="{L}" y="{H-B+100}" font-size="9.8" fill="{SLATE}">Assessed probabilities '
             f'are locally calibrated planning figures for one clinic, not constants. '
             f'Source: PCI original.</text>')

    (PML / "fig_16_1_1.svg").write_text(svg(W, H, body))

    # ============================================ Fig 16.4.1 — the closing account (waterfall)
    # (label lines, delta, kind) — kind: "total" = absolute subtotal column, else a movement
    STEPS = (("Flat claim", "as approved", 3447095.50, "total"),
             ("Adoption term", "corrected", -2114197.83, "down"),
             ("Honest ramped", "case (D2)", 1332897.67, "total"),
             ("Level", "variance", -465015.16, "down"),
             ("Timing", "variance", -413809.38, "down"),
             ("Operating cost", "omitted", -644900.24, "down"),
             ("Cost outturn", "variance", -114000.00, "down"),
             ("REALISED", "NPV", -304827.11, "total"),
             ("Improvement", "plan", 340156.49, "up"),
             ("Post&#8722;plan", "NPV", 35329.38, "total"))

    W2, H2 = 900, 580
    L2, R2, T2, B2 = 92, 34, 88, 124
    PW2, PH2 = W2 - L2 - R2, H2 - T2 - B2
    VMIN, VMAX = -900000.0, 3700000.0
    stp = PW2 / len(STEPS)

    def Y2(v):
        return T2 + PH2 - (v - VMIN) / (VMAX - VMIN) * PH2

    body = ""
    for v in range(-500000, 3600000, 500000):
        y = Y2(float(v))
        body += (f'<line x1="{L2}" y1="{y:.1f}" x2="{L2+PW2:.1f}" y2="{y:.1f}" stroke="{GRID}"/>'
                 f'<text x="{L2-8}" y="{y+4:.1f}" font-size="10.2" fill="{SLATE}" '
                 f'text-anchor="end">{v/1000000.0:.1f}m</text>')
    y0 = Y2(0.0)
    body += (f'<line x1="{L2}" y1="{y0:.1f}" x2="{L2+PW2:.1f}" y2="{y0:.1f}" stroke="{INK}" '
             f'stroke-width="1.3"/>')

    bw2 = stp * 0.56
    run = 0.0
    prev_top = None
    for k, (l1, l2, delta, kind) in enumerate(STEPS):
        cx = L2 + (k + 0.5) * stp
        if kind == "total":
            top, bot, run = max(0.0, delta), min(0.0, delta), delta
            fill, op, stroke = BLUE, "1", "none"
        else:
            base = run
            run = base + delta
            top, bot = max(base, run), min(base, run)
            if kind == "down":
                fill, op, stroke = CRIMSON, "0.86", "none"
            else:
                fill, op, stroke = BLUE, "0.30", BLUE
        yt, yb = min(Y2(top), Y2(bot)), max(Y2(top), Y2(bot))
        extra = "" if stroke == "none" else f' stroke="{stroke}" stroke-width="1.6"'
        body += (f'<rect x="{cx-bw2/2:.1f}" y="{yt:.1f}" width="{bw2:.1f}" '
                 f'height="{max(yb-yt, 2.0):.1f}" fill="{fill}" opacity="{op}"{extra}/>')
        if prev_top is not None:
            body += (f'<line x1="{prev_top[0]:.1f}" y1="{prev_top[1]:.1f}" '
                     f'x2="{cx-bw2/2:.1f}" y2="{prev_top[1]:.1f}" stroke="{SLATE}" '
                     f'stroke-width="0.9" stroke-dasharray="3 3"/>')
        prev_top = (cx + bw2 / 2, Y2(run))

        # value label
        txt = f"{'+' if delta > 0 else '&#8722;'}{abs(delta):,.0f}"
        ly = yt - 8 if delta > 0 else yb + 14
        col = INK if kind == "total" else (CRIMSON if kind == "down" else BLUE)
        wt = "700" if kind == "total" else "600"
        body += (f'<text x="{cx:.1f}" y="{ly:.1f}" font-size="10.2" fill="{col}" '
                 f'text-anchor="middle" font-weight="{wt}">{txt}</text>')
        # category label
        lab_y = H2 - B2 + 18
        body += (f'<text x="{cx:.1f}" y="{lab_y:.1f}" font-size="9.7" fill="{INK}" '
                 f'text-anchor="middle" font-weight="{"700" if kind == "total" else "400"}">'
                 f'{l1}</text>'
                 f'<text x="{cx:.1f}" y="{lab_y+12:.1f}" font-size="9.7" fill="{INK}" '
                 f'text-anchor="middle" font-weight="{"700" if kind == "total" else "400"}">'
                 f'{l2}</text>')

    body += (f'<text x="{L2}" y="{T2-56}" font-size="13.6" fill="{INK}" font-weight="700">'
             f'The closing account of Meridian Care Records</text>'
             f'<text x="{L2}" y="{T2-38}" font-size="10.7" fill="{SLATE}">NPV, USD, eight years '
             f'at 7 % (AF = 5.971299). From what the business case promised to what was measured, '
             f'and what the</text>'
             f'<text x="{L2}" y="{T2-25}" font-size="10.7" fill="{SLATE}">post&#8722;project '
             f'improvement plan recovers. Blue columns are positions; crimson movements are '
             f'losses; the outlined column is the only gain.</text>')

    body += (f'<line x1="{L2}" y1="{H2-B2+46}" x2="{W2-40}" y2="{H2-B2+46}" stroke="{GRID}"/>'
             f'<text x="{L2}" y="{H2-B2+64}" font-size="10.3" fill="{INK}">Measured steady state: '
             f'27 of 40 clinics in daily use (67.50 %) releasing 5.4 clinician&#8722;hours per '
             f'week &#8212; <tspan font-weight="600">USD 594,864</tspan> a year, '
             f'<tspan font-weight="600">60.75 %</tspan> of the flat claim and '
             f'<tspan font-weight="600">86.79 %</tspan> of the honest case.</text>'
             f'<text x="{L2}" y="{H2-B2+79}" font-size="10.3" fill="{SLATE}">The realised position '
             f'is negative only because the approved case carried no operating&#8722;cost line; '
             f'the account turns positive on USD 180,000 of post&#8722;project adoption work that '
             f'no project plan contained.</text>'
             f'<text x="{L2}" y="{H2-B2+94}" font-size="9.8" fill="{SLATE}">Source: PCI '
             f'original.</text>')

    (PML / "fig_16_4_1.svg").write_text(svg(W2, H2, body))
