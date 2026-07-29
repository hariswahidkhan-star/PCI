"""Domain 4 (PML-AI) extension figures — Integration and delivery architecture.

Fig 4.3.1  Pairwise configuration integrity is the power of item integrity: interface conformance
           p^2 (two-sided) and p^3 (three-party) against item conformance p, with Meridian's
           87.06 % item register giving 75.79 % two-sided conformance, and the 97.47 % item
           conformance a 95 % interface target actually requires.
Fig 4.3.2  Meridian's original-to-current baseline reconciliation bridge: a net residual of
           32,200 concealing a gross error of 122,000 in two opposite-signed classes.

All values reproduce the manuscript arithmetic exactly. Deterministic output.
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PML = ctx["PML"]

    # ---- Fig 4.3.1 — pairwise integrity is p^k ----------------------------------
    W, H, L, R, T, B = 660, 410, 84, 130, 40, 60
    lo, hi = 0.80, 1.00

    def X(v):
        return L + (v - lo) / (hi - lo) * (W - L - R)

    def Y(v):
        return H - B - (v - 0.50) / 0.50 * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W-R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L-8}" y="{Y(v)+4:.1f}" font-size="10" fill="{SLATE}" text-anchor="end">'
        f'{int(round(v*100))}%</text>'
        for v in (0.50, 0.60, 0.70, 0.80, 0.90, 1.00))
    xt = "".join(
        f'<line x1="{X(v):.1f}" y1="{H-B}" x2="{X(v):.1f}" y2="{H-B+5}" stroke="{INK}"/>'
        f'<text x="{X(v):.1f}" y="{H-B+18}" font-size="10" fill="{SLATE}" text-anchor="middle">'
        f'{v*100:.0f}%</text>'
        for v in (0.80, 0.85, 0.90, 0.95, 1.00))

    n = 200
    step = (hi - lo) / n
    p1 = " ".join(f"{X(lo+k*step):.1f},{Y(lo+k*step):.1f}" for k in range(n + 1))
    p2 = " ".join(f"{X(lo+k*step):.1f},{Y((lo+k*step)**2):.1f}" for k in range(n + 1))
    p3 = " ".join(f"{X(lo+k*step):.1f},{Y((lo+k*step)**3):.1f}" for k in range(n + 1))

    pm = 296.0 / 340.0          # 0.870588235
    pm2 = pm * pm               # 0.757923875
    pt = 0.95 ** 0.5            # 0.974679434

    body = (grid + axes(L, T, W - R, H - B,
                        "Item-level configuration conformance p",
                        "Conformance of the assembled relationship") + xt
            + f'<polyline points="{p1}" fill="none" stroke="{SLATE}" stroke-width="1.6" '
              f'stroke-dasharray="5 4"/>'
            + f'<polyline points="{p2}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
            + f'<polyline points="{p3}" fill="none" stroke="{CRIMSON}" stroke-width="2.2" '
              f'stroke-dasharray="8 4"/>'
            + f'<text x="{W-R+6}" y="{Y(0.995):.1f}" font-size="10.5" fill="{SLATE}">p (one item)</text>'
            + f'<text x="{W-R+6}" y="{Y(0.975):.1f}" font-size="10.5" fill="{BLUE}" '
              f'font-weight="600">p&#178; two-sided</text>'
            + f'<text x="{W-R+6}" y="{Y(0.945):.1f}" font-size="10.5" fill="{CRIMSON}" '
              f'font-weight="600">p&#179; three-party</text>'
            # Meridian point
            + f'<line x1="{X(pm):.1f}" y1="{H-B}" x2="{X(pm):.1f}" y2="{Y(pm2):.1f}" '
              f'stroke="{INK}" stroke-width="0.9" stroke-dasharray="3 3"/>'
            + f'<line x1="{L}" y1="{Y(pm2):.1f}" x2="{X(pm):.1f}" y2="{Y(pm2):.1f}" '
              f'stroke="{INK}" stroke-width="0.9" stroke-dasharray="3 3"/>'
            + f'<circle cx="{X(pm):.1f}" cy="{Y(pm2):.1f}" r="4" fill="{BLUE}"/>'
            + f'<text x="{X(pm)+8:.1f}" y="{Y(pm2)-10:.1f}" font-size="10.5" fill="{INK}" '
              f'font-weight="600">Meridian: 296/340 = 87.06% of items,</text>'
            + f'<text x="{X(pm)+8:.1f}" y="{Y(pm2)+3:.1f}" font-size="10.5" fill="{INK}" '
              f'font-weight="600">so only 75.79% of two-sided interfaces</text>'
            # 95 % interface target
            + f'<line x1="{X(pt):.1f}" y1="{H-B}" x2="{X(pt):.1f}" y2="{Y(0.95):.1f}" '
              f'stroke="{CRIMSON}" stroke-width="1.1"/>'
            + f'<circle cx="{X(pt):.1f}" cy="{Y(0.95):.1f}" r="4" fill="{CRIMSON}"/>'
            + f'<text x="{X(pt)-10:.1f}" y="{Y(0.95)-24:.1f}" font-size="10.5" fill="{CRIMSON}" '
              f'text-anchor="end" font-weight="600">a 95% interface target needs</text>'
            + f'<text x="{X(pt)-10:.1f}" y="{Y(0.95)-11:.1f}" font-size="10.5" fill="{CRIMSON}" '
              f'text-anchor="end" font-weight="600">97.47% item conformance</text>'
            + f'<text x="{L+2}" y="{H-14}" font-size="10" fill="{SLATE}">An interface is verified '
              f'only if both sides are on their recorded version: the curve, not the item rate, sets '
              f'the audit tolerance. Source: PCI original.</text>')
    (PML / "fig_4_3_1.svg").write_text(svg(W, H, body))

    # ---- Fig 4.3.2 — original-to-current reconciliation bridge ------------------
    W, H, L, R, T, B = 660, 420, 92, 26, 52, 74
    base, top = 2350000.0, 3000000.0

    def Yv(v):
        return H - B - (v - base) / (top - base) * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Yv(v):.1f}" x2="{W-R}" y2="{Yv(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L-8}" y="{Yv(v)+4:.1f}" font-size="10" fill="{SLATE}" text-anchor="end">'
        f'{v/1000000:.1f}m</text>'
        for v in (2400000, 2500000, 2600000, 2700000, 2800000, 2900000, 3000000))

    # columns: original | + approved changes | = expected | + unreferenced | - unrecorded | tool
    cols = [
        ("Original\nbaseline", 2400000.0, 0.0, "total", BLUE),
        ("+ approved\nchanges 516,700", 2400000.0, 516700.0, "delta", BLUE),
        ("= expected\ncurrent 2,916,700", 2916700.0, 0.0, "total", BLUE),
        ("+ movements with\nno change ref\n+77,100", 2916700.0, 77100.0, "delta", CRIMSON),
        ("&#8722; changes with no\nbaseline effect\n&#8722;44,900", 2948900.0, 44900.0, "neg", CRIMSON),
        ("Tool current\n2,948,900", 2948900.0, 0.0, "total", INK),
    ]
    slot = (W - L - R) / len(cols)
    bw = slot * 0.52
    bars = ""
    for k, (lab, bottom, delta, kind, col) in enumerate(cols):
        cx = L + slot * (k + 0.5)
        x0 = cx - bw / 2
        if kind == "total":
            y = Yv(bottom)
            bars += (f'<rect x="{x0:.1f}" y="{y:.1f}" width="{bw:.1f}" '
                     f'height="{(H-B)-y:.1f}" fill="{col}" opacity="0.90"/>')
            bars += (f'<text x="{cx:.1f}" y="{y-8:.1f}" font-size="10.5" fill="{col}" '
                     f'text-anchor="middle" font-weight="600">{bottom/1000000:.4f}m</text>')
        elif kind == "delta":
            y = Yv(bottom + delta)
            bars += (f'<rect x="{x0:.1f}" y="{y:.1f}" width="{bw:.1f}" '
                     f'height="{Yv(bottom)-y:.1f}" fill="{col}" opacity="0.55"/>')
        else:  # negative step drawn downward from the top of the previous total
            y = Yv(bottom)
            bars += (f'<rect x="{x0:.1f}" y="{y:.1f}" width="{bw:.1f}" '
                     f'height="{Yv(bottom-delta)-y:.1f}" fill="{col}" opacity="0.28"/>')
        for j, line in enumerate(lab.split("\n")):
            bars += (f'<text x="{cx:.1f}" y="{H-B+16+j*12:.1f}" font-size="9.6" fill="{INK}" '
                     f'text-anchor="middle">{line}</text>')

    netY = Yv(2948900.0)
    body = (grid + axes(L, T, W - R, H - B, "", "Cost baseline (USD)") + bars
            + f'<line x1="{L}" y1="{Yv(2916700.0):.1f}" x2="{W-R}" y2="{Yv(2916700.0):.1f}" '
              f'stroke="{BLUE}" stroke-width="1.1" stroke-dasharray="4 4"/>'
            + f'<line x1="{L+8}" y1="{netY:.1f}" x2="{W-R-8}" y2="{netY:.1f}" '
              f'stroke="{INK}" stroke-width="1.1" stroke-dasharray="2 3"/>'
            + f'<text x="{L+4}" y="{T-30}" font-size="11.5" fill="{INK}" font-weight="600">'
              f'Net residual 32,200 (1.34% of baseline) &#8212; gross error 122,000 (23.61% of the '
              f'approved changes)</text>'
            + f'<text x="{L+4}" y="{T-14}" font-size="10.5" fill="{CRIMSON}">The two error classes '
              f'have opposite signs, so netting them hides 89,800 of the 122,000. Reconcile gross.</text>'
            + f'<text x="{L+2}" y="{H-14}" font-size="10" fill="{SLATE}">Original + &#931; approved '
              f'changes must equal the current baseline exactly; every non-zero residual is decomposed '
              f'before it is netted. Source: PCI original.</text>')
    (PML / "fig_4_3_2.svg").write_text(svg(W, H, body))
