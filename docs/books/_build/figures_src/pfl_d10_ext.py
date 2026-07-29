"""PFL-AI Domain 10 — Debt sizing, covenants and credit metrics (extension figures).

Fig 10.1.2  Three ways to size the same project at 1.30x against Kestrel's declining bank-case
            CFADS: level service on the base-case (year-one) test (41,171,123, DSCR 1.2980 ->
            1.2087), level service on the minimum-period test (38,253,896, 1.3893 -> 1.3000) and
            sculpted service holding 1.3000 in every period (39,915,812).
Fig 10.2.2  Which covenant binds first: the revenue fall that crosses each of four covenants —
            DSCR year one 4.1382 %, debt/EBITDA 4.4444 %, LLCR 4.9833 %, ICR at 2.50x
            10.6667 % — with the year-twelve DSCR already breached at -0.8317 % and the marker
            at 1.7067 % showing where ICR would bind if drafted at 2.90x.

Deterministic: the coverage profiles are recomputed here from the master-thread constants
(42,000,000 / 41,171,122.86 / 38,253,896.42 at 6.0 %, tax 20 %, CFADS = 5,880,000 + 0.20 x
interest), and every annotated literal is a value verified in the domain's arithmetic pass
(decimal.Decimal throughout; see the manuscript's printed-values register).
"""

RATE = 0.06
TAX = 0.20
BASE = 5880000.0                      # CFADS before the interest tax shield


def _level_profile(debt, instalment, n=12):
    """DSCR per period for a level instalment, with CFADS = BASE + TAX x interest."""
    bal, out = debt, []
    for _ in range(n):
        interest = bal * RATE
        out.append((BASE + TAX * interest) / instalment)
        bal = bal * (1 + RATE) - instalment
    return out


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 10.1.2 — three ways to size the same project at 1.30x ----------------
    base_case = _level_profile(41171122.86, 6384000.0 / 1.30)      # 1.2980 -> 1.2087
    min_case = _level_profile(38253896.42, 5880000.0 / (1.30 - TAX * RATE / (1 + RATE)))
    sculpted = [1.30] * 12                                         # flat by construction

    W, H = 760, 470
    L, R, T, B = 82, 168, 84, 68
    pw, ph = W - L - R, H - T - B
    ymin, ymax = 1.18, 1.42

    def X(t):
        return L + (t - 1) / 11 * pw

    def Y(v):
        return T + ph - (v - ymin) / (ymax - ymin) * ph

    grid = ""
    for k in range(0, 13):
        v = ymin + k * 0.02
        grid += (f'<line x1="{L}" y1="{Y(v):.1f}" x2="{L+pw}" y2="{Y(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Y(v)+3.6:.1f}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="end">{v:.2f}</text>')
    xticks = "".join(
        f'<text x="{X(t):.1f}" y="{T+ph+17:.1f}" font-size="9.6" fill="{SLATE}" '
        f'text-anchor="middle">{t}</text>' for t in range(1, 13))

    def poly(series):
        return " ".join(f"{X(t+1):.1f},{Y(v):.1f}" for t, v in enumerate(series))

    lines = (
        f'<polyline points="{poly(min_case)}" fill="none" stroke="{SLATE}" stroke-width="2.0" '
        f'stroke-dasharray="7 4"/>'
        f'<polyline points="{poly(base_case)}" fill="none" stroke="{INK}" stroke-width="2.2"/>'
        f'<polyline points="{poly(sculpted)}" fill="none" stroke="{BLUE}" stroke-width="3.0"/>')

    # requirement and covenant references
    refs = (f'<line x1="{L}" y1="{Y(1.30):.1f}" x2="{L+pw}" y2="{Y(1.30):.1f}" stroke="{CRIMSON}" '
            f'stroke-width="1.1" stroke-dasharray="5 4"/>'
            f'<text x="{L+6}" y="{Y(1.30)-6:.1f}" font-size="10" fill="{CRIMSON}">'
            f'1.30&#215; sizing requirement</text>'
            f'<line x1="{L}" y1="{Y(1.20):.1f}" x2="{L+pw}" y2="{Y(1.20):.1f}" stroke="{CRIMSON}" '
            f'stroke-width="1.1" stroke-dasharray="2 3"/>'
            f'<text x="{L+6}" y="{Y(1.20)+13:.1f}" font-size="10" fill="{CRIMSON}">'
            f'1.20&#215; covenant</text>')

    mark = (f'<circle cx="{X(12):.1f}" cy="{Y(base_case[11]):.1f}" r="3.4" fill="{CRIMSON}"/>'
            f'<text x="{X(12)-8:.1f}" y="{Y(base_case[11])+14:.1f}" font-size="9.8" fill="{INK}" '
            f'text-anchor="end">1.2087 &#8212; inside the covenant, outside the sizing test</text>')

    labels = (
        f'<text x="{L+pw+10}" y="{Y(min_case[0])+3:.1f}" font-size="10.4" fill="{SLATE}" '
        f'font-weight="600">Level, minimum test</text>'
        f'<text x="{L+pw+10}" y="{Y(min_case[0])+16:.1f}" font-size="10" fill="{SLATE}">'
        f'38.25m &#183; 1.3893 &#8594; 1.3000</text>'
        f'<text x="{L+pw+10}" y="{Y(1.30)-8:.1f}" font-size="10.4" fill="{BLUE}" '
        f'font-weight="600">Sculpted, 1.30&#215; every year</text>'
        f'<text x="{L+pw+10}" y="{Y(1.30)+5:.1f}" font-size="10" fill="{BLUE}">'
        f'39.92m &#183; flat 1.3000</text>'
        f'<text x="{L+pw+10}" y="{Y(base_case[6])+3:.1f}" font-size="10.4" fill="{INK}" '
        f'font-weight="600">Level, base-case test</text>'
        f'<text x="{L+pw+10}" y="{Y(base_case[6])+16:.1f}" font-size="10" fill="{INK}">'
        f'41.17m &#183; 1.2980 &#8594; 1.2087</text>')

    header = (f'<text x="{L}" y="30" font-size="13.4" fill="{INK}" font-weight="700">'
              f'Three ways to size the same project at 1.30&#215;</text>'
              f'<text x="{L}" y="49" font-size="10.6" fill="{SLATE}">'
              f'Requiring 1.30&#215; in every period rather than in year one costs '
              f'<tspan fill="{INK}" font-weight="600">2,917,226</tspan> of debt capacity;</text>'
              f'<text x="{L}" y="64" font-size="10.6" fill="{SLATE}">'
              f'sculpting recovers <tspan fill="{BLUE}" font-weight="600">1,661,916</tspan> of it '
              f'without relaxing a single period&#8217;s requirement.</text>')

    body = (header + grid + refs + lines + mark
            + axes(L, T, L + pw, T + ph, "Loan year", "DSCR") + xticks + labels)
    (PFL / "fig_10_1_2.svg").write_text(svg(W, H, body))

    # ---- Fig 10.2.2 — which covenant binds first ---------------------------------
    # (label, breaking revenue fall %, trigger in own units, colour key)
    bars = [
        ("DSCR &#8805; 1.20&#215; (year 12)", -0.8317, "CFADS 6,011,562 &#8212; already breached, 74,849 short", "crimson"),
        ("DSCR &#8805; 1.20&#215; (year 1)", 4.1382, "CFADS 6,011,562", "blue"),
        ("debt/EBITDA &#8804; 6.00&#215;", 4.4444, "EBITDA 7,000,000", "blue"),
        ("LLCR &#8805; 1.15&#215;", 4.9833, "PV(CFADS) 48,300,000", "blue"),
        ("ICR &#8805; 2.50&#215;", 10.6667, "EBITDA 6,300,000", "blue"),
    ]
    W2, H2 = 760, 400
    L2, R2, T2, B2 = 196, 40, 88, 62
    pw2, ph2 = W2 - L2 - R2, H2 - T2 - B2
    xmin, xmax = -2.0, 12.0

    def X2(v):
        return L2 + (v - xmin) / (xmax - xmin) * pw2

    zero = X2(0.0)
    rows, n = "", len(bars)
    slot = ph2 / n
    bh = slot * 0.52
    for k, (name, val, trig, key) in enumerate(bars):
        cy = T2 + slot * (k + 0.5)
        col = CRIMSON if key == "crimson" else BLUE
        x0 = min(zero, X2(val))
        w = abs(X2(val) - zero)
        rows += (f'<rect x="{x0:.1f}" y="{cy-bh/2:.1f}" width="{w:.1f}" height="{bh:.1f}" '
                 f'fill="{col}" opacity="0.88"/>'
                 f'<text x="{L2-10}" y="{cy-1:.1f}" font-size="10.6" fill="{INK}" '
                 f'text-anchor="end" font-weight="600">{name}</text>'
                 f'<text x="{L2-10}" y="{cy+12:.1f}" font-size="9.4" fill="{SLATE}" '
                 f'text-anchor="end">{trig}</text>')
        tx = X2(val) + (8 if val >= 0 else -8)
        anc = "start" if val >= 0 else "end"
        rows += (f'<text x="{tx:.1f}" y="{cy+4:.1f}" font-size="10.6" fill="{col}" '
                 f'text-anchor="{anc}" font-weight="700">{val:+.4f} %</text>')

    # the ICR-at-2.90x marker sits on the ICR row
    icr_cy = T2 + slot * (n - 0.5)
    marker = (f'<line x1="{X2(1.7067):.1f}" y1="{icr_cy-bh/2-8:.1f}" x2="{X2(1.7067):.1f}" '
              f'y2="{icr_cy+bh/2+8:.1f}" stroke="{SLATE}" stroke-width="2.0"/>'
              f'<text x="{X2(1.7067):.1f}" y="{icr_cy+bh/2+21:.1f}" font-size="9.6" fill="{SLATE}" '
              f'text-anchor="middle">1.7067 % &#8212; ICR at 2.90&#215;: drafting, not economics</text>')

    xt = ""
    for v in range(-2, 13, 2):
        xt += (f'<line x1="{X2(v):.1f}" y1="{T2}" x2="{X2(v):.1f}" y2="{T2+ph2}" stroke="{GRID}"/>'
               f'<text x="{X2(v):.1f}" y="{T2+ph2+18}" font-size="9.6" fill="{SLATE}" '
               f'text-anchor="middle">{v}</text>')
    zline = (f'<line x1="{zero:.1f}" y1="{T2-6}" x2="{zero:.1f}" y2="{T2+ph2+4}" stroke="{INK}" '
             f'stroke-width="1.4"/>'
             f'<text x="{zero:.1f}" y="{T2-12}" font-size="9.6" fill="{INK}" '
             f'text-anchor="middle">base case</text>')

    header2 = (f'<text x="{L2-186}" y="30" font-size="13.4" fill="{INK}" font-weight="700">'
               f'Which covenant binds first</text>'
               f'<text x="{L2-186}" y="49" font-size="10.6" fill="{SLATE}">'
               f'Revenue fall from the 12,000,000 base that crosses each covenant on the mandated '
               f'42,000,000 facility.</text>'
               f'<text x="{L2-186}" y="64" font-size="10.6" fill="{SLATE}">'
               f'The period cash test binds first; the worst period is already breached.</text>')

    body2 = (header2 + xt + rows + marker + zline
             + f'<line x1="{L2}" y1="{T2+ph2}" x2="{L2+pw2}" y2="{T2+ph2}" stroke="{INK}" '
               f'stroke-width="1.2"/>'
             + f'<text x="{L2+pw2/2:.1f}" y="{T2+ph2+40}" font-size="11.4" fill="{SLATE}" '
               f'text-anchor="middle">Revenue fall from base (%)</text>')
    (PFL / "fig_10_2_2.svg").write_text(svg(W2, H2, body2))
