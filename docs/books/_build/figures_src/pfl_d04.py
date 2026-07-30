"""PFL-AI Domain 4 — Investment appraisal and capital budgeting. PCI original artwork.

Fig 4.1.1  Kestrel's NPV profile: value against the discount rate, +16,179,360 at the 8 % board
           rate, crossing zero at the retained IRR of 12.192120 %.
Fig 4.2.1  Two paybacks — cumulative cash against cumulative present value — crossing at 6.74 and
           10.07 years, the band between them being the cost of time value.
Fig 4.3.1  The two intake designs' profiles and their crossover: P and Q both worth exactly
           2,500,000 at 10.424845 %, Q leading below it and P above.
Fig 4.4.1  What flexibility is worth. The same expected inflow (8,900,000) under a narrow and a
           wide spread: staging loses 57,928 on the narrow case even with a free pilot, and gains
           3,595,521 on the wide one.

Deterministic: every annotated value is a literal verified in checks/pfl_d04.py.
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    def af(r, n):
        return n if r == 0 else (1 - (1 + r) ** -n) / r

    # ---- Fig 4.1.1 — Kestrel's NPV profile -------------------------------------------
    I0, A, N, IRR = 60000000.0, 8900000.0, 15, 0.12192120
    W, H, L, R, T, B = 660, 400, 86, 30, 28, 58

    def X(r):
        return L + r / 0.20 * (W - L - R)

    def Y(v):
        return H - B - (v - (-20000000)) / 95000000 * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Y(v):.1f}" x2="{W - R}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y(v) + 4:.1f}" font-size="10" fill="{SLATE}" text-anchor="end">'
        f'{"+" if v > 0 else ("&#8722;" if v < 0 else "")}{abs(v) // 1000000}m</text>'
        for v in range(-20000000, 80000001, 20000000))
    xt = "".join(
        f'<text x="{X(r / 100):.1f}" y="{H - B + 17}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{r}%</text>' for r in range(0, 21, 4))
    pts = " ".join(f"{X(k / 400):.1f},{Y(A * af(k / 400, N) - I0):.1f}" for k in range(0, 81))
    body = (grid + axes(L, T, W - R, H - B, "Discount rate", "NPV (USD)") + xt
            + f'<line x1="{L}" y1="{Y(0):.1f}" x2="{W - R}" y2="{Y(0):.1f}" stroke="{INK}" '
              f'stroke-width="0.9" stroke-dasharray="3 3"/>'
            + f'<polyline points="{pts}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
            + f'<line x1="{X(0.08):.1f}" y1="{T}" x2="{X(0.08):.1f}" y2="{H - B}" '
              f'stroke="{SLATE}" stroke-width="1.1" stroke-dasharray="5 4"/>'
            + f'<circle cx="{X(0.08):.1f}" cy="{Y(16179360):.1f}" r="4" fill="{BLUE}"/>'
            + f'<text x="{X(0.08) + 9:.1f}" y="{Y(16179360) - 9:.1f}" font-size="10.5" '
              f'fill="{INK}" font-weight="600">8 % board rate: +16,179,360</text>'
            + f'<circle cx="{X(IRR):.1f}" cy="{Y(0):.1f}" r="4.2" fill="{CRIMSON}"/>'
            + f'<text x="{X(IRR) + 9:.1f}" y="{Y(0) - 10:.1f}" font-size="10.5" fill="{CRIMSON}" '
              f'font-weight="600">IRR 12.1921 % &#183; NPV = 0</text>'
            + f'<text x="{L + 10}" y="{Y(73500000) + 4:.1f}" font-size="10.2" fill="{SLATE}">'
              f'undiscounted surplus +73,500,000</text>'
            + f'<text x="{X(0.126):.1f}" y="{Y(-14500000):.1f}" font-size="10.2" '
              f'fill="{SLATE}">value destroyed</text>')
    (PFL / "fig_4_1_1.svg").write_text(svg(W, H, body))

    # ---- Fig 4.2.1 — two paybacks ----------------------------------------------------
    W, H, L, R, T, B = 660, 400, 92, 30, 28, 58

    def X2(t):
        return L + t / 15 * (W - L - R)

    def Y2(v):
        return H - B - (v + 60000000) / 145000000 * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Y2(v):.1f}" x2="{W - R}" y2="{Y2(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y2(v) + 4:.1f}" font-size="10" fill="{SLATE}" text-anchor="end">'
        f'{"+" if v > 0 else ("&#8722;" if v < 0 else "")}{abs(v) // 1000000}m</text>'
        for v in range(-60000000, 80000001, 20000000))
    xt = "".join(
        f'<text x="{X2(t):.1f}" y="{H - B + 17}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{t}</text>' for t in range(0, 16, 3))
    nom = " ".join(f"{X2(t):.1f},{Y2(-I0 + A * t):.1f}" for t in range(16))
    dis = " ".join(f"{X2(t):.1f},{Y2(-I0 + A * af(0.08, t)):.1f}" for t in range(16))
    # the band between the two cumulative lines: nominal forward, discounted back
    band = (" ".join(f"{X2(t):.1f},{Y2(-I0 + A * t):.1f}" for t in range(16))
            + " " + " ".join(f"{X2(t):.1f},{Y2(-I0 + A * af(0.08, t)):.1f}"
                             for t in range(15, -1, -1)))
    body = (grid + f'<polygon points="{band}" fill="{BLUE}" opacity="0.10"/>'
            + axes(L, T, W - R, H - B, "Years", "Cumulative (USD)") + xt
            + f'<line x1="{L}" y1="{Y2(0):.1f}" x2="{W - R}" y2="{Y2(0):.1f}" stroke="{INK}" '
              f'stroke-width="0.9" stroke-dasharray="3 3"/>'
            + f'<polyline points="{nom}" fill="none" stroke="{SLATE}" stroke-width="2.3" '
              f'stroke-dasharray="6 4"/>'
            + f'<polyline points="{dis}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
            + f'<circle cx="{X2(6.74):.1f}" cy="{Y2(0):.1f}" r="4" fill="{SLATE}"/>'
            + f'<text x="{X2(6.74):.1f}" y="{Y2(0) - 12:.1f}" font-size="10.5" fill="{INK}" '
              f'text-anchor="middle">6.74</text>'
            + f'<circle cx="{X2(10.07):.1f}" cy="{Y2(0):.1f}" r="4.2" fill="{CRIMSON}"/>'
            + f'<text x="{X2(10.07) + 6:.1f}" y="{Y2(0) + 18:.1f}" font-size="10.5" '
              f'fill="{CRIMSON}" font-weight="600">10.07</text>'
            + f'<text x="{X2(10.5):.1f}" y="{Y2(24000000):.1f}" font-size="10.2" fill="{BLUE}">'
              f'the cost of time value &#183; 3.33 years</text>'
            + f'<text x="{X2(8.7):.1f}" y="{Y2(70000000):.1f}" font-size="11" fill="{SLATE}" '
              f'font-weight="600">cumulative cash</text>'
            + f'<text x="{X2(11.4):.1f}" y="{Y2(11000000):.1f}" font-size="11" fill="{BLUE}" '
              f'font-weight="600">cumulative PV</text>')
    (PFL / "fig_4_2_1.svg").write_text(svg(W, H, body))

    # ---- Fig 4.3.1 — two profiles and the crossover ----------------------------------
    W, H, L, R, T, B = 660, 400, 92, 30, 28, 58
    XO = 0.10424845

    def X3(r):
        return L + r / 0.30 * (W - L - R)

    def Y3(v):
        return H - B - (v + 6000000) / 17000000 * (H - T - B)

    grid = "".join(
        f'<line x1="{L}" y1="{Y3(v):.1f}" x2="{W - R}" y2="{Y3(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{Y3(v) + 4:.1f}" font-size="10" fill="{SLATE}" text-anchor="end">'
        f'{"+" if v > 0 else ("&#8722;" if v < 0 else "")}{abs(v) // 1000000}m</text>'
        for v in range(-6000000, 11000001, 2000000))
    xt = "".join(
        f'<text x="{X3(r / 100):.1f}" y="{H - B + 17}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{r}%</text>' for r in range(0, 31, 5))
    pp = " ".join(f"{X3(k / 400):.1f},{Y3(2000000 * af(k / 400, 5) - 5000000):.1f}"
                  for k in range(0, 121))
    qq = " ".join(f"{X3(k / 400):.1f},{Y3(6000000 * af(k / 400, 5) - 20000000):.1f}"
                  for k in range(0, 121))
    body = (grid + axes(L, T, W - R, H - B, "Discount rate", "NPV (USD)") + xt
            + f'<line x1="{L}" y1="{Y3(0):.1f}" x2="{W - R}" y2="{Y3(0):.1f}" stroke="{INK}" '
              f'stroke-width="0.9" stroke-dasharray="3 3"/>'
            + f'<line x1="{X3(0.08):.1f}" y1="{T}" x2="{X3(0.08):.1f}" y2="{H - B}" '
              f'stroke="{SLATE}" stroke-width="1.1" stroke-dasharray="5 4"/>'
            + f'<text x="{X3(0.08) - 5:.1f}" y="{T + 12}" font-size="10" fill="{SLATE}" '
              f'text-anchor="end">8 % hurdle</text>'
            + f'<polyline points="{qq}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
            + f'<polyline points="{pp}" fill="none" stroke="{SLATE}" stroke-width="2.3"/>'
            + f'<circle cx="{X3(XO):.1f}" cy="{Y3(2500000):.1f}" r="4.4" fill="{CRIMSON}"/>'
            + f'<text x="{X3(XO) + 9:.1f}" y="{Y3(2500000) - 22:.1f}" font-size="10.5" '
              f'fill="{CRIMSON}" font-weight="600">crossover 10.4248 %</text>'
            + f'<text x="{X3(XO) + 9:.1f}" y="{Y3(2500000) - 9:.1f}" font-size="10.2" '
              f'fill="{CRIMSON}">both exactly +2,500,000</text>'
            + f'<text x="{X3(0.012):.1f}" y="{Y3(9700000):.1f}" font-size="11" fill="{BLUE}" '
              f'font-weight="600">Q (full-scale, 20m)</text>'
            + f'<text x="{X3(0.014):.1f}" y="{Y3(3400000):.1f}" font-size="11" fill="{SLATE}" '
              f'font-weight="600">P (compact, 5m)</text>'
            + f'<text x="{X3(0.223):.1f}" y="{Y3(1500000):.1f}" font-size="10" fill="{SLATE}">'
              f'P: IRR 28.65 %</text>'
            + f'<text x="{X3(0.126):.1f}" y="{Y3(-3000000):.1f}" font-size="10" fill="{BLUE}">'
              f'Q: IRR 15.24 %</text>')
    (PFL / "fig_4_3_1.svg").write_text(svg(W, H, body))

    # ---- Fig 4.4.1 — what flexibility is worth ---------------------------------------
    # Two panels, one per spread. Each shows build-now against staging, with the two state NPVs.
    W, H = 720, 440
    panels = [
        ("Narrow spread", "p = 0.6 / 0.4", "10,400,000", "6,650,000", "+29,018,578",
         "&#8722;3,079,467", "+16,179,360", "+16,121,432", "&#8722;57,928", "+1,231,787", False),
        ("Wide spread", "p = 0.5 / 0.5", "12,000,000", "5,800,000", "+42,713,744",
         "&#8722;10,355,024", "+16,179,360", "+19,774,882", "+3,595,521", "+5,177,512", True),
    ]
    body = (f'<text x="{W / 2}" y="26" font-size="12.5" fill="{INK}" text-anchor="middle" '
            f'font-weight="600">Same expected inflow (8,900,000) &#183; same build-now NPV &#183; '
            f'opposite conclusions</text>')
    for k, (title, prob, gi, bi, gn, bn, now, staged, delta, avoided, wins) in enumerate(panels):
        x0 = 24 + k * (W / 2)
        pw = W / 2 - 48
        col = BLUE if wins else CRIMSON
        body += (f'<rect x="{x0}" y="48" width="{pw:.1f}" height="366" fill="none" '
                 f'stroke="{GRID}" stroke-width="1.4"/>'
                 f'<text x="{x0 + pw / 2:.1f}" y="72" font-size="12" fill="{INK}" '
                 f'text-anchor="middle" font-weight="600">{title}</text>'
                 f'<text x="{x0 + pw / 2:.1f}" y="89" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{prob}</text>')
        # the two states
        for j, (lab, inflow, npvv, good) in enumerate(
                [("strong", gi, gn, True), ("weak", bi, bn, False)]):
            y = 118 + j * 52
            sc = BLUE if good else CRIMSON
            body += (f'<rect x="{x0 + 18}" y="{y}" width="{pw - 36:.1f}" height="40" '
                     f'fill="{sc}" opacity="0.08"/>'
                     f'<text x="{x0 + 30}" y="{y + 17}" font-size="10.4" fill="{INK}">'
                     f'{lab} state &#183; inflow {inflow}</text>'
                     f'<text x="{x0 + 30}" y="{y + 32}" font-size="10.8" fill="{sc}" '
                     f'font-weight="600">NPV {npvv}</text>')
        # the two strategies
        for j, (lab, val) in enumerate([("Build now", now), ("Stage (free pilot)", staged)]):
            y = 234 + j * 46
            body += (f'<text x="{x0 + 30}" y="{y + 14}" font-size="10.6" fill="{SLATE}">'
                     f'{lab}</text>'
                     f'<text x="{x0 + pw - 30:.1f}" y="{y + 14}" font-size="11.4" fill="{INK}" '
                     f'text-anchor="end" font-weight="600">{val}</text>'
                     f'<line x1="{x0 + 26}" y1="{y + 24}" x2="{x0 + pw - 26:.1f}" y2="{y + 24}" '
                     f'stroke="{GRID}"/>')
        body += (f'<text x="{x0 + 30}" y="344" font-size="10.4" fill="{SLATE}">'
                 f'abandonment option avoids</text>'
                 f'<text x="{x0 + pw - 30:.1f}" y="344" font-size="10.8" fill="{SLATE}" '
                 f'text-anchor="end">{avoided}</text>'
                 f'<rect x="{x0 + 18}" y="360" width="{pw - 36:.1f}" height="38" '
                 f'fill="{col}" opacity="0.12"/>'
                 f'<text x="{x0 + 30}" y="384" font-size="11" fill="{col}" font-weight="600">'
                 f'staging is worth {delta}</text>')
    (PFL / "fig_4_4_1.svg").write_text(svg(W, H, body))
