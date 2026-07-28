"""PML-AI Domain 14 — Digital Delivery, Data and Responsible AI. PCI original artwork.

Fig 14.1.1  Defect rate against consequence-weighted exposure across the six data classes of
            Meridian&#8217;s common data environment (KA 14.1.4). Paired horizontal bars per class:
            defect rate d on a 0&#8211;7 % scale, and exposure n&#183;d&#183;u on a
            0&#8211;70,000 USD scale, with exposure per record (d&#183;u) printed at the right.
            The pairing makes the rank inversion visible &#8212; migration reconciliation is last
            of six by defect rate (0.8 %) and second by exposure (56,000). A footer compares three
            regimes: observed 332 defects / USD 172,820; a uniform 2 % target 282 defects /
            USD 207,440 (15.06 % fewer defects, 20.03 % more cost); and the case study&#8217;s
            consequence-proportional schedule 275 defects / USD 76,820 at a 1.9504 % mean rate.

Fig 14.3.1  The verification standard proportional to consequence (KA 14.3.3). Required review
            tier against the consequence of one escaped error on a logarithmic axis, drawn as a
            staircase with risers at the derived thresholds u* = &#916;v/(p&#183;&#916;q) =
            261.90 / 785.71 / 3,208.33 / 18,333.33 at p = 0.12. Meridian&#8217;s five output
            classes are marked on the step each falls into. A dashed staircase shows the same
            standard at p = 0.04, shifted right by exactly three (785.71 / 2,357.14 / 9,625.00 /
            55,000.00). A side panel compares four regimes on the same 941 monthly outputs.

All values are those computed in the manuscript with decimal arithmetic. The defect rates, error
rate p, detection rates q and consequence figures u are locally calibrated or measured planning
figures, not constants &#8212; the manuscript says so at each use. Deterministic: no randomness,
no timestamps, no locale-dependent formatting.
"""

from fractions import Fraction as F


def _log10(x):
    """Deterministic base-10 log for the fixed set of positive values plotted here."""
    lo, hi = 0.0, 6.0
    for _ in range(80):
        mid = (lo + hi) / 2.0
        if 10.0 ** mid < x:
            lo = mid
        else:
            hi = mid
    return (lo + hi) / 2.0


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ============================================ Fig 14.1.1 — defect rate vs exposure by class
    # (label, records, defect rate, cost per defect) — KA 14.1.4
    CLS = (("Interface field-mapping records", 900, F(6, 100), F(1200)),
           ("Migration reconciliation records", 2000, F(8, 1000), F(3500)),
           ("Clinician accounts and roles", 4800, F(15, 1000), F(250)),
           ("Clinic readiness checklists", 1200, F(3, 100), F(400)),
           ("Device and network assets", 1600, F(4, 100), F(180)),
           ("Training and competency records", 3600, F(25, 1000), F(90)))

    W, H = 760, 520
    L, R, T, B = 226, 214, 58, 118          # left labels, right notes, top, bottom
    RATEMAX, EXPMAX = 7.0, 70000.0
    ROW = (H - T - B) / len(CLS)
    BH = 11.0                                # bar height
    PLOTW = W - R - L

    def XR(v):                               # x for a defect rate, per cent
        return L + v / RATEMAX * PLOTW

    def XE(v):                               # x for an exposure, USD
        return L + v / EXPMAX * PLOTW

    body = ""
    # vertical guides + twin scales
    for v in (0, 1, 2, 3, 4, 5, 6, 7):
        x = XR(float(v))
        body += (f'<line x1="{x:.1f}" y1="{T}" x2="{x:.1f}" y2="{H-B:.1f}" stroke="{GRID}"/>'
                 f'<text x="{x:.1f}" y="{T-24}" font-size="9.6" fill="{BLUE}" '
                 f'text-anchor="middle">{v} %</text>')
    for v in (0, 10000, 20000, 30000, 40000, 50000, 60000, 70000):
        x = XE(float(v))
        body += (f'<text x="{x:.1f}" y="{T-10}" font-size="9.6" fill="{CRIMSON}" '
                 f'text-anchor="middle">{v//1000}k</text>')
    body += (f'<text x="{L-6}" y="{T-24}" font-size="9.6" fill="{BLUE}" text-anchor="end" '
             f'font-weight="600">defect rate</text>'
             f'<text x="{L-6}" y="{T-10}" font-size="9.6" fill="{CRIMSON}" text-anchor="end" '
             f'font-weight="600">exposure, USD</text>')

    for k, (lab, n, d, u) in enumerate(CLS):
        y = T + k * ROW + 6
        rate = float(d) * 100.0
        exp = float(F(n) * d * u)
        perrec = F(d) * u
        body += (f'<text x="{L-10}" y="{y+11:.1f}" font-size="10.2" fill="{INK}" '
                 f'text-anchor="end">{lab}</text>'
                 f'<text x="{L-10}" y="{y+24:.1f}" font-size="9.2" fill="{SLATE}" '
                 f'text-anchor="end">n = {n:,} &#183; u = USD {int(u):,}</text>'
                 f'<rect x="{L}" y="{y:.1f}" width="{XR(rate)-L:.1f}" height="{BH}" '
                 f'fill="{BLUE}"/>'
                 f'<text x="{XR(rate)+6:.1f}" y="{y+9.5:.1f}" font-size="9.6" fill="{BLUE}" '
                 f'font-weight="600">{rate:.1f} %</text>'
                 f'<rect x="{L}" y="{y+BH+3:.1f}" width="{XE(exp)-L:.1f}" height="{BH}" '
                 f'fill="{CRIMSON}" opacity="0.9"/>'
                 f'<text x="{XE(exp)+6:.1f}" y="{y+BH+12.5:.1f}" font-size="9.6" fill="{CRIMSON}" '
                 f'font-weight="600">{int(exp):,}</text>'
                 f'<text x="{W-R+8}" y="{y+18:.1f}" font-size="10.2" fill="{INK}" '
                 f'font-weight="600">{float(perrec):.2f}</text>')
    body += (f'<text x="{W-R+8}" y="{T-24}" font-size="9.6" fill="{INK}" font-weight="600">'
             f'exposure</text>'
             f'<text x="{W-R+8}" y="{T-12}" font-size="9.6" fill="{INK}" font-weight="600">'
             f'per record</text>')

    # rank-inversion callouts
    yA = T + 1 * ROW + 6
    body += (f'<text x="{XE(56000.0)+62:.1f}" y="{yA+BH+12.5:.1f}" font-size="9.8" '
             f'fill="{INK}">&#8592; lowest rate of six, second by exposure</text>')
    yD = T + 4 * ROW + 6
    body += (f'<text x="{XR(4.0)+52:.1f}" y="{yD+9.5:.1f}" font-size="9.8" fill="{INK}">'
             f'&#8592; second by rate, fifth by exposure</text>')

    # footer: three regimes
    fy = H - B + 22
    body += (f'<line x1="{28}" y1="{H-B+8}" x2="{W-28}" y2="{H-B+8}" stroke="{INK}" '
             f'stroke-width="1"/>')
    rows = (("observed", "332 defects", "USD 172,820", "weighted mean rate 2.3546 %", INK),
            ("uniform 2 % target", "282 defects", "USD 207,440",
             "15.06 % fewer defects, 20.03 % MORE cost", CRIMSON),
            ("consequence-proportional", "275 defects", "USD 76,820",
             "mean rate 1.9504 % &#8212; passes the 2 % target at 62.97 % less cost", BLUE))
    for k, (a, b, c, note, col) in enumerate(rows):
        y = fy + k * 20
        body += (f'<text x="30" y="{y}" font-size="10.4" fill="{col}" font-weight="600">{a}</text>'
                 f'<text x="196" y="{y}" font-size="10.4" fill="{SLATE}">{b}</text>'
                 f'<text x="286" y="{y}" font-size="10.4" fill="{col}" font-weight="600">{c}</text>'
                 f'<text x="392" y="{y}" font-size="9.8" fill="{SLATE}">{note}</text>')

    body += axes(L, T, W - R, H - B, "Defect rate (blue, upper bars) and exposure "
                                     "n&#183;d&#183;u in USD (crimson, lower bars)", "")
    (PML / "fig_14_1_1.svg").write_text(svg(W, H, body))

    # ==================================== Fig 14.3.1 — the verification standard, tier vs consequence
    # tier: (label, v USD, q) — KA 14.3.3, blended programme rate USD 110/h
    TIERS = (("0 accept", F(0), F(0)),
             ("1 author scan", F(11), F(35, 100)),
             ("2 independent review", F(44), F(70, 100)),
             ("3 review + source trace", F(121), F(90, 100)),
             ("4 two reviewers + reperformance", F(275), F(97, 100)))

    def thresholds(p):
        out = []
        for k in range(1, 5):
            dv = TIERS[k][1] - TIERS[k - 1][1]
            dq = TIERS[k][2] - TIERS[k - 1][2]
            out.append(dv / (p * dq))
        return out

    TH12 = thresholds(F(12, 100))            # 261.9048 785.7143 3208.3333 18333.3333
    TH04 = thresholds(F(4, 100))             # 785.7143 2357.1429 9625.0000 55000.0000

    OUT = (("meeting summaries", 620, F(150)),
           ("readiness assessments", 180, F(480)),
           ("mapping proposals", 95, F(1200)),
           ("reconciliation scripts", 40, F(3500)),
           ("change impact assessments", 6, F(28560)))

    W2, H2 = 760, 480
    L2, R2, T2, B2 = 78, 240, 46, 96
    UMIN, UMAX = 100.0, 100000.0
    LO, HI = _log10(UMIN), _log10(UMAX)

    def X2(u):
        return L2 + (_log10(float(u)) - LO) / (HI - LO) * (W2 - R2 - L2)

    def Y2(t):
        return (H2 - B2) - t / 4.0 * (H2 - T2 - B2)

    b2 = ""
    for t in range(5):
        y = Y2(t)
        b2 += (f'<line x1="{L2}" y1="{y:.1f}" x2="{W2-R2}" y2="{y:.1f}" stroke="{GRID}"/>'
               f'<text x="{L2-8}" y="{y+4:.1f}" font-size="10.4" fill="{INK}" '
               f'text-anchor="end" font-weight="600">tier {t}</text>')
    for u in (100, 300, 1000, 3000, 10000, 30000, 100000):
        b2 += (f'<text x="{X2(u):.1f}" y="{H2-B2+17}" font-size="9.8" fill="{SLATE}" '
               f'text-anchor="middle">{u:,}</text>')

    def staircase(ths, colour, width, dash, tag):
        pts = [(L2, Y2(0))]
        for t, th in enumerate(ths, start=1):
            x = X2(th)
            pts.append((x, Y2(t - 1)))
            pts.append((x, Y2(t)))
        pts.append((W2 - R2, Y2(4)))
        p = " ".join(f"{x:.1f},{y:.1f}" for x, y in pts)
        s = (f'<polyline points="{p}" fill="none" stroke="{colour}" stroke-width="{width}"'
             f'{dash}/>')
        if tag:
            for t, th in enumerate(ths, start=1):
                x = X2(th)
                s += (f'<text x="{x-4:.1f}" y="{Y2(t)-7:.1f}" font-size="9.4" fill="{colour}" '
                      f'text-anchor="end" font-weight="600">{float(th):,.2f}</text>')
        return s

    b2 += staircase(TH04, SLATE, 1.6, ' stroke-dasharray="6 4"', False)
    b2 += staircase(TH12, BLUE, 2.8, '', True)

    for lab, n, u in OUT:
        t = 0
        for k, th in enumerate(TH12, start=1):
            if u >= th:
                t = k
        x, y = X2(u), Y2(t)
        b2 += (f'<circle cx="{x:.1f}" cy="{y:.1f}" r="4.4" fill="{CRIMSON}"/>'
               f'<text x="{x:.1f}" y="{y+17:.1f}" font-size="9.6" fill="{CRIMSON}" '
               f'text-anchor="middle" font-weight="600">{lab}</text>'
               f'<text x="{x:.1f}" y="{y+29:.1f}" font-size="9.2" fill="{SLATE}" '
               f'text-anchor="middle">u {int(u):,} &#183; n {n}</text>')

    b2 += (f'<text x="{X2(1400):.1f}" y="{T2+12}" font-size="10.2" fill="{BLUE}" '
           f'font-weight="600">u* = &#916;v / (p &#183; &#916;q), p = 0.12</text>'
           f'<text x="{X2(11000):.1f}" y="{T2+12}" font-size="10.2" fill="{SLATE}">'
           f'same standard at p = 0.04 &#8212; every threshold &#215; 3</text>')

    notes = (("Tiers (USD per item, measured q)", INK, 600),
             ("1 author scan 11.00 / 0.35", SLATE, 400),
             ("2 independent review 44.00 / 0.70", SLATE, 400),
             ("3 review + source trace 121.00 / 0.90", SLATE, 400),
             ("4 two reviewers, reperform 275.00 / 0.97", SLATE, 400),
             ("", INK, 400),
             ("941 outputs a month, total cost", INK, 600),
             ("= verification + expected escaped loss", SLATE, 400),
             ("no verification    72,571.20", CRIMSON, 400),
             ("uniform tier 2     63,175.36", CRIMSON, 400),
             ("uniform tier 4    260,952.14", CRIMSON, 400),
             ("TIERED              36,950.10", BLUE, 600),
             ("", INK, 400),
             ("Uniform tier 2 spends 3.27&#215; as", INK, 600),
             ("much on checking to cut escaped", SLATE, 400),
             ("loss by 10.41 %. At p = 0.04 the", SLATE, 400),
             ("tiered total falls to 16,036.44", BLUE, 600),
             ("&#8212; the checking bill by 72.09 %.", BLUE, 600))
    for k, (line, col, wt) in enumerate(notes):
        b2 += (f'<text x="{W2-R2+22}" y="{T2+10+k*15.4:.1f}" font-size="9.6" fill="{col}" '
               f'font-weight="{wt}">{line}</text>')

    b2 += axes(L2, T2, W2 - R2, H2 - B2,
               "Consequence of one escaped error, u (USD, logarithmic)",
               "Required verification tier")
    (PML / "fig_14_3_1.svg").write_text(svg(W2, H2, b2))
