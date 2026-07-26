"""Domain 8 (PML-AI) extension figures — Risk, uncertainty and resilience.

Fig 8.2.2  Auriga's aggregate risk distribution under independence and under one shared driver,
           with the USD 490,624 reserve marked and the confidence it actually buys.
Fig 8.2.3  Expected value against standard-deviation contribution, Meridian's five quantified
           risks — the rank reversal that decides which response to buy.

All values reproduce the manuscript arithmetic exactly. Deterministic output.
"""
import math


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PML = ctx["PML"]

    # ---- Fig 8.2.3 — EMV against sigma contribution (Meridian) --------------------
    reg = [("M1", 0.40, 90000.0), ("M2", 0.35, 120000.0), ("M3", 0.20, 150000.0),
           ("M4", 0.30, 70000.0), ("M5", 0.25, -40000.0)]
    rows = []
    for k, p, i in reg:
        emv = abs(p * i)
        sd = math.sqrt(p * (1 - p) * i * i)
        rows.append((k, emv, sd, i < 0))

    W, H, L, R, T, B = 660, 400, 84, 26, 40, 60
    top = 66000.0

    def Yv(v):
        return H - B - v / top * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Yv(v):.1f}" x2="{W-R}" y2="{Yv(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L-8}" y="{Yv(v)+4:.1f}" font-size="10" fill="{SLATE}" text-anchor="end">{int(v/1000)}k</text>'
        for v in (0, 10000, 20000, 30000, 40000, 50000, 60000))
    slot = (W - L - R) / len(rows)
    bw = slot * 0.26
    bars = ""
    for n, (k, emv, sd, opp) in enumerate(rows):
        x0 = L + slot * (n + 0.5) - bw * 1.05
        bars += (f'<rect x="{x0:.1f}" y="{Yv(emv):.1f}" width="{bw:.1f}" '
                 f'height="{(H-B)-Yv(emv):.1f}" fill="{BLUE}"'
                 + (' opacity="0.45"' if opp else '') + '/>'
                 f'<rect x="{x0+bw*1.1:.1f}" y="{Yv(sd):.1f}" width="{bw:.1f}" '
                 f'height="{(H-B)-Yv(sd):.1f}" fill="{CRIMSON}" opacity="0.85"/>'
                 f'<text x="{L+slot*(n+0.5):.1f}" y="{H-B+17}" font-size="11" fill="{INK}" '
                 f'text-anchor="middle" font-weight="600">{k}</text>'
                 f'<text x="{x0+bw/2:.1f}" y="{Yv(emv)-5:.1f}" font-size="9.5" fill="{BLUE}" '
                 f'text-anchor="middle">{emv/1000:.0f}k</text>'
                 f'<text x="{x0+bw*1.6:.1f}" y="{Yv(sd)-5:.1f}" font-size="9.5" fill="{CRIMSON}" '
                 f'text-anchor="middle">{sd/1000:.1f}k</text>')
    m3x = L + slot * 2.5
    body = (grid + bars + axes(L, T, W - R, H - B, "Meridian register entry",
                               "USD (expected value / &#963; contribution)")
            + f'<rect x="{L+8}" y="{T-26}" width="11" height="11" fill="{BLUE}"/>'
            + f'<text x="{L+24}" y="{T-17}" font-size="11" fill="{INK}">Expected value (|p &#215; impact|)</text>'
            + f'<rect x="{L+218}" y="{T-26}" width="11" height="11" fill="{CRIMSON}" opacity="0.85"/>'
            + f'<text x="{L+234}" y="{T-17}" font-size="11" fill="{INK}">&#963; contribution (&#8730;(p(1&#8722;p))&#215;impact)</text>'
            + f'<line x1="{m3x:.1f}" y1="{Yv(60000):.1f}" x2="{W-R-92}" y2="{T+22}" '
              f'stroke="{SLATE}" stroke-width="0.9" stroke-dasharray="3 3"/>'
            + f'<text x="{W-R-88}" y="{T+18}" font-size="10.5" fill="{INK}" font-weight="600">M3: 3rd by EMV,</text>'
            + f'<text x="{W-R-88}" y="{T+31}" font-size="10.5" fill="{INK}" font-weight="600">1st by variance</text>'
            + f'<text x="{L+8}" y="{H-14}" font-size="10" fill="{SLATE}">M5 is the opportunity '
              f'(shown at |EMV| = 10,000); variance is unsigned. Source: PCI original.</text>')
    (PML / "fig_8_2_3.svg").write_text(svg(W, H, body))

    # ---- Fig 8.2.2 — independent vs correlated aggregate -------------------------
    mean, s_ind, s_cor, reserve = 278000.0, 252642.04, 340339.07, 490623.54
    W, H, L, R, T, B = 660, 400, 84, 30, 40, 60
    lo, hi = -300000.0, 1300000.0
    peak = 1.0 / (s_ind * math.sqrt(2 * math.pi))

    def Xc(v):
        return L + (v - lo) / (hi - lo) * (W - L - R)

    def Yd(d):
        return H - B - d / (peak * 1.08) * (H - T - B)

    def dens(v, s):
        return math.exp(-0.5 * ((v - mean) / s) ** 2) / (s * math.sqrt(2 * math.pi))

    n = 240
    step = (hi - lo) / n
    ind = " ".join(f"{Xc(lo+k*step):.1f},{Yd(dens(lo+k*step, s_ind)):.1f}" for k in range(n + 1))
    cor = " ".join(f"{Xc(lo+k*step):.1f},{Yd(dens(lo+k*step, s_cor)):.1f}" for k in range(n + 1))
    fill_pts = [f"{Xc(lo):.1f},{H-B}"]
    k = 0
    while lo + k * step <= reserve:
        v = lo + k * step
        fill_pts.append(f"{Xc(v):.1f},{Yd(dens(v, s_cor)):.1f}")
        k += 1
    fill_pts.append(f"{Xc(reserve):.1f},{Yd(dens(reserve, s_cor)):.1f}")
    fill_pts.append(f"{Xc(reserve):.1f},{H-B}")
    xt = "".join(
        f'<line x1="{Xc(v):.1f}" y1="{H-B}" x2="{Xc(v):.1f}" y2="{H-B+5}" stroke="{INK}"/>'
        f'<text x="{Xc(v):.1f}" y="{H-B+18}" font-size="10" fill="{SLATE}" text-anchor="middle">'
        f'{"&#8722;" if v < 0 else ""}{abs(int(v/1000))}k</text>'
        for v in (-300000, 0, 300000, 600000, 900000, 1200000))
    body = (f'<polygon points="{" ".join(fill_pts)}" fill="{BLUE}" opacity="0.12"/>'
            + axes(L, T, W - R, H - B, "Aggregate risk outcome (USD)", "Relative likelihood") + xt
            + f'<polyline points="{ind}" fill="none" stroke="{BLUE}" stroke-width="2.4"/>'
            + f'<polyline points="{cor}" fill="none" stroke="{CRIMSON}" stroke-width="2.4" stroke-dasharray="7 4"/>'
            + f'<line x1="{Xc(mean):.1f}" y1="{Yd(0):.1f}" x2="{Xc(mean):.1f}" y2="{T+8}" '
              f'stroke="{SLATE}" stroke-width="1" stroke-dasharray="3 3"/>'
            + f'<text x="{Xc(mean):.1f}" y="{T+4}" font-size="10.5" fill="{SLATE}" text-anchor="middle">mean 278,000 (unmoved by correlation)</text>'
            + f'<line x1="{Xc(reserve):.1f}" y1="{Yd(0):.1f}" x2="{Xc(reserve):.1f}" y2="{T+30}" '
              f'stroke="{INK}" stroke-width="1.4"/>'
            + f'<text x="{Xc(reserve)+7:.1f}" y="{T+42}" font-size="11" fill="{INK}" font-weight="600">reserve 490,624</text>'
            + f'<text x="{Xc(reserve)+7:.1f}" y="{T+57}" font-size="10.5" fill="{BLUE}">P80 if independent</text>'
            + f'<text x="{Xc(reserve)+7:.1f}" y="{T+71}" font-size="10.5" fill="{CRIMSON}" font-weight="600">P73.4 with one shared driver</text>'
            + f'<line x1="{L+300}" y1="{T-24}" x2="{L+330}" y2="{T-24}" stroke="{BLUE}" stroke-width="2.4"/>'
            + f'<text x="{L+336}" y="{T-20}" font-size="11" fill="{INK}">independent &#963; 252,642</text>'
            + f'<line x1="{L+300}" y1="{T-8}" x2="{L+330}" y2="{T-8}" stroke="{CRIMSON}" stroke-width="2.4" stroke-dasharray="7 4"/>'
            + f'<text x="{L+336}" y="{T-4}" font-size="11" fill="{INK}">&#961; = 0.5 on R1/R3/R4: &#963; 340,339</text>'
            + f'<text x="{L+8}" y="{H-14}" font-size="10" fill="{SLATE}">Same mean, fatter tail: the '
              f'label on the reserve fails before the money does. Source: PCI original.</text>')
    (PML / "fig_8_2_2.svg").write_text(svg(W, H, body))
