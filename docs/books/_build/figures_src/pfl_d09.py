"""PFL-AI Domain 9 — Funding structure and sources of capital. PCI original artwork.

Fig 9.1.4  The leverage ladder: across 60-80 % senior gearing Kestrel's equity IRR rises
           11.7685 -> 13.6146 %, its WACC falls only 8.0880 -> 7.8840 %, and DSCR falls
           1.4867 -> 1.1151 — with the gearing at which the 1.30x sizing target (68.6185 %),
           the 1.20x covenant (74.3367 %) and the 1.15x lock-up (77.5688 %) bind.
Fig 9.2.3  Four candidate capital structures priced: tranche mix and after-tax cost, with the
           resulting WACC, total DSCR and equity IRR beneath each, against the derived project
           WACC of 8.0001 % at the coverage-binding gearing.

Deterministic: every value below is a literal verified in the domain's arithmetic pass
(decimal.Decimal throughout; see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 9.1.4 — the leverage ladder -----------------------------------------
    # (gearing %, equity IRR %, WACC %, DSCR)
    ladder = [
        (60.0, 11.7685, 8.0880, 1.4867),
        (65.0, 12.1206, 8.0370, 1.3724),
        (70.0, 12.5311, 7.9860, 1.2743),
        (75.0, 13.0193, 7.9350, 1.1894),
        (80.0, 13.6146, 7.8840, 1.1151),
    ]
    # (DSCR threshold, gearing at which it binds, label)
    binds = [
        (1.30, 68.6185, "1.30&#215; sizing target"),
        (1.20, 74.3367, "1.20&#215; covenant"),
        (1.15, 77.5688, "1.15&#215; lock-up"),
    ]
    W, H = 780, 470
    L, R, T, B = 78, 206, 92, 74
    plot_w, plot_h = W - L - R, H - T - B
    gmin, gmax = 58.0, 82.0
    pmin, pmax = 7.0, 14.5           # left axis, per cent
    dmin, dmax = 1.05, 1.55          # right axis, DSCR
    LEG = W - R + 56                 # legend column x

    def X(g):
        return L + (g - gmin) / (gmax - gmin) * plot_w

    def YP(v):
        return H - B - (v - pmin) / (pmax - pmin) * plot_h

    def YD(v):
        return H - B - (v - dmin) / (dmax - dmin) * plot_h

    grid = "".join(
        f'<line x1="{L}" y1="{YP(v):.1f}" x2="{W - R}" y2="{YP(v):.1f}" stroke="{GRID}"/>'
        f'<text x="{L - 8}" y="{YP(v) + 4:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="end">{v:.0f} %</text>'
        for v in (7, 8, 9, 10, 11, 12, 13, 14))
    grid += (f'<line x1="{W - R}" y1="{T}" x2="{W - R}" y2="{H - B}" stroke="{CRIMSON}" '
             f'stroke-width="1.2" opacity="0.55"/>')
    grid += "".join(
        f'<text x="{W - R + 6}" y="{YD(v) + 4:.1f}" font-size="10" fill="{CRIMSON}">{v:.2f}</text>'
        for v in (1.10, 1.20, 1.30, 1.40, 1.50))
    xticks = "".join(
        f'<line x1="{X(g):.1f}" y1="{H - B}" x2="{X(g):.1f}" y2="{H - B + 4}" stroke="{INK}"/>'
        f'<text x="{X(g):.1f}" y="{H - B + 17:.1f}" font-size="10" fill="{SLATE}" '
        f'text-anchor="middle">{g:.0f} %</text>'
        for g, _, _, _ in ladder)

    # shaded infeasible region: gearing above the 1.20x covenant bind
    shade = (f'<rect x="{X(74.3367):.1f}" y="{T}" width="{W - R - X(74.3367):.1f}" '
             f'height="{plot_h:.1f}" fill="{CRIMSON}" opacity="0.055"/>')

    eq = " ".join(f"{X(g):.1f},{YP(e):.1f}" for g, e, _, _ in ladder)
    wa = " ".join(f"{X(g):.1f},{YP(w):.1f}" for g, _, w, _ in ladder)
    ds = " ".join(f"{X(g):.1f},{YD(d):.1f}" for g, _, _, d in ladder)
    lines = (f'<polyline points="{eq}" fill="none" stroke="{BLUE}" stroke-width="2.8"/>'
             f'<polyline points="{ds}" fill="none" stroke="{CRIMSON}" stroke-width="2.8"/>'
             f'<polyline points="{wa}" fill="none" stroke="{SLATE}" stroke-width="2.2" '
             f'stroke-dasharray="7 4"/>')
    for g, e, w, d in ladder:
        lines += (f'<circle cx="{X(g):.1f}" cy="{YP(e):.1f}" r="3.4" fill="{BLUE}"/>'
                  f'<circle cx="{X(g):.1f}" cy="{YD(d):.1f}" r="3.4" fill="{CRIMSON}"/>'
                  f'<circle cx="{X(g):.1f}" cy="{YP(w):.1f}" r="2.6" fill="{SLATE}"/>')
    # end-point value labels (equity IRR and DSCR only; WACC is labelled on the line)
    lines += (f'<text x="{X(60) - 8:.1f}" y="{YP(11.7685) + 4:.1f}" font-size="10" fill="{BLUE}" '
              f'text-anchor="end" font-weight="700">11.77</text>'
              f'<text x="{X(80) - 6:.1f}" y="{YP(13.6146) + 16:.1f}" font-size="10" fill="{BLUE}" '
              f'text-anchor="end" font-weight="700">13.61</text>'
              f'<text x="{X(60) - 8:.1f}" y="{YD(1.4867) + 4:.1f}" font-size="10" '
              f'fill="{CRIMSON}" text-anchor="end" font-weight="700">1.4867</text>'
              f'<text x="{X(80) - 6:.1f}" y="{YD(1.1151) + 16:.1f}" font-size="10" '
              f'fill="{CRIMSON}" text-anchor="end" font-weight="700">1.1151</text>'
              f'<text x="{X(66.5):.1f}" y="{YP(7.9995) + 18:.1f}" font-size="9.8" fill="{SLATE}" '
              f'text-anchor="middle">WACC 8.0880 &#8594; 7.8840 % &#8212; a 20.40 bp fall across '
              'the whole ladder</text>')

    # binding markers
    marks = ""
    for thr, g, lab in binds:
        marks += (f'<line x1="{L}" y1="{YD(thr):.1f}" x2="{X(g):.1f}" y2="{YD(thr):.1f}" '
                  f'stroke="{CRIMSON}" stroke-width="1" stroke-dasharray="3 3" opacity="0.7"/>'
                  f'<line x1="{X(g):.1f}" y1="{YD(thr):.1f}" x2="{X(g):.1f}" y2="{H - B - 16}" '
                  f'stroke="{CRIMSON}" stroke-width="1.3" stroke-dasharray="3 3" opacity="0.7"/>'
                  f'<circle cx="{X(g):.1f}" cy="{YD(thr):.1f}" r="3.4" fill="none" '
                  f'stroke="{CRIMSON}" stroke-width="1.8"/>'
                  f'<text x="{L + 6}" y="{YD(thr) - 6:.1f}" font-size="9.4" fill="{CRIMSON}">'
                  f'{lab}</text>'
                  f'<text x="{X(g):.1f}" y="{H - B - 6:.1f}" font-size="9.6" fill="{CRIMSON}" '
                  f'text-anchor="middle" font-weight="700">{g:.2f} %</text>')
    marks += (f'<text x="{(X(74.3367) + W - R) / 2:.1f}" y="{T + 15}" font-size="9.6" '
              f'fill="{CRIMSON}" text-anchor="middle" font-weight="600">'
              'covenant fails</text>'
              f'<text x="{(X(74.3367) + W - R) / 2:.1f}" y="{T + 27}" font-size="9.6" '
              f'fill="{CRIMSON}" text-anchor="middle" font-weight="600">'
              'on base case</text>')

    legend = (f'<line x1="{LEG}" y1="{T + 8}" x2="{LEG + 22}" y2="{T + 8}" '
              f'stroke="{BLUE}" stroke-width="2.8"/>'
              f'<text x="{LEG + 28}" y="{T + 12}" font-size="10.2" fill="{INK}">equity IRR</text>'
              f'<line x1="{LEG}" y1="{T + 26}" x2="{LEG + 22}" y2="{T + 26}" '
              f'stroke="{CRIMSON}" stroke-width="2.8"/>'
              f'<text x="{LEG + 28}" y="{T + 30}" font-size="10.2" fill="{INK}">DSCR (right)</text>'
              f'<line x1="{LEG}" y1="{T + 44}" x2="{LEG + 22}" y2="{T + 44}" '
              f'stroke="{SLATE}" stroke-width="2.2" stroke-dasharray="7 4"/>'
              f'<text x="{LEG + 28}" y="{T + 48}" font-size="10.2" fill="{INK}">WACC</text>'
              f'<text x="{LEG}" y="{T + 78}" font-size="10" fill="{INK}" '
              f'font-weight="700">Across 20 points</text>'
              f'<text x="{LEG}" y="{T + 91}" font-size="10" fill="{INK}" '
              f'font-weight="700">of gearing</text>'
              f'<text x="{LEG}" y="{T + 109}" font-size="9.8" fill="{BLUE}">'
              'equity IRR +184.61 bp</text>'
              f'<text x="{LEG}" y="{T + 124}" font-size="9.8" fill="{SLATE}">'
              'WACC &#8722;20.40 bp</text>'
              f'<text x="{LEG}" y="{T + 139}" font-size="9.8" fill="{CRIMSON}">'
              'DSCR &#8722;0.3716</text>'
              f'<text x="{LEG}" y="{T + 161}" font-size="10.2" fill="{INK}" '
              f'font-weight="700">The equity gain is</text>'
              f'<text x="{LEG}" y="{T + 175}" font-size="10.2" fill="{INK}" '
              f'font-weight="700">9.05&#215; the WACC gain</text>'
              f'<text x="{LEG}" y="{T + 197}" font-size="9.6" fill="{SLATE}">At the margin,</text>'
              f'<text x="{LEG}" y="{T + 210}" font-size="9.6" fill="{SLATE}">'
              '70 &#8594; 75 %: +48.82 bp</text>'
              f'<text x="{LEG}" y="{T + 223}" font-size="9.6" fill="{SLATE}">'
              'of equity return for</text>'
              f'<text x="{LEG}" y="{T + 236}" font-size="9.6" fill="{SLATE}">'
              '0.0850 of coverage</text>'
              f'<text x="{LEG}" y="{T + 258}" font-size="9.8" fill="{CRIMSON}" '
              f'font-weight="700">Crimson circles: the</text>'
              f'<text x="{LEG}" y="{T + 271}" font-size="9.8" fill="{CRIMSON}" '
              f'font-weight="700">gearing at which each</text>'
              f'<text x="{LEG}" y="{T + 284}" font-size="9.8" fill="{CRIMSON}" '
              f'font-weight="700">threshold binds</text>')

    body = (grid + shade + xticks
            + axes(L, T, W - R, H - B, "Senior gearing (debt as a share of 60,000,000 capex)",
                   "Return / cost of capital (% per year)")
            + lines + marks + legend
            + f'<text x="24" y="28" font-size="12.5" font-weight="700" fill="{INK}">'
              'Gearing is a transfer to equity, not an efficiency for the project</text>'
            + f'<text x="24" y="46" font-size="10.4" fill="{SLATE}">'
              'Kestrel Water SPC: level CFADS 6,384,000; senior debt at 6.0 % over 12 years; '
              'cost of equity re-levered at each gearing</text>'
            + f'<text x="24" y="61" font-size="10.4" fill="{CRIMSON}">'
              'The 1.30 &#215; sizing target binds at 68.62 % gearing; above 74.34 % the 1.20 '
              '&#215; covenant fails on base-case cash, before any stress</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 14}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'The structure is chosen by the coverage constraint; the cost of capital is the '
              'consequence</text>')
    (PFL / "fig_9_1_4.svg").write_text(svg(W, H, body))

    # ---- Fig 9.2.3 — four capital structures, priced ------------------------------
    # (title, subtitle, [(amount, after-tax cost text, colour key)], WACC, DSCR, eqIRR, emphasis)
    structures = [
        ("A &#8212; base 70/30", "senior 42.0m + equity 18.0m",
         [(42000000, "4.80 %", "s"), (18000000, "15.42 %", "e")],
         "7.9860 %", "1.2743", "12.5311 %", 0),
        ("B &#8212; mezzanine displaces senior", "36.0m + 9.0m mezz + 15.0m",
         [(36000000, "4.56 %", "s"), (9000000, "9.20 %", "m"), (15000000, "17.34 %", "e")],
         "8.4510 %", "1.1586", "12.0824 %", 0),
        ("C &#8212; mezzanine displaces equity", "42.0m + 6.0m mezz + 12.0m",
         [(42000000, "4.80 %", "s"), (6000000, "9.20 %", "m"), (12000000, "20.22 %", "e")],
         "8.3240 %", "1.0881", "12.7378 %", 0),
        ("D &#8212; the financeable structure", "senior 41.0m + equity 19.0m",
         [(41000000, "4.80 %", "s"), (19000000, "14.91 %", "e")],
         "8.0030 %", "1.3054", "12.3868 %", 1),
    ]
    W, H = 760, 480
    L, R, T, B = 246, 44, 104, 112
    plot_w = W - L - R
    total = 60000000.0
    row_h = (H - T - B) / len(structures)
    fills = {"s": (BLUE, "0.92"), "m": (SLATE, "0.80"), "e": (CRIMSON, "0.88")}

    bars = ""
    for i, (title, sub, tranches, wacc, dscr, eqirr, emph) in enumerate(structures):
        y = T + i * row_h + row_h * 0.06
        h = row_h * 0.44
        x = L
        for amt, cost, key in tranches:
            wdt = amt / total * plot_w
            col, op = fills[key]
            bars += (f'<rect x="{x:.1f}" y="{y:.1f}" width="{wdt:.1f}" height="{h:.1f}" '
                     f'fill="{col}" opacity="{op}"/>')
            fa, fb = (10, 9.2) if wdt >= 58 else (8.8, 8.1)
            bars += (f'<text x="{x + wdt / 2:.1f}" y="{y + h * 0.44:.1f}" font-size="{fa}" '
                     f'fill="white" text-anchor="middle" font-weight="700">'
                     f'{amt / 1000000:.0f}m</text>')
            bars += (f'<text x="{x + wdt / 2:.1f}" y="{y + h * 0.85:.1f}" font-size="{fb}" '
                     f'fill="white" text-anchor="middle">{cost}</text>')
            x += wdt
        lab_col = CRIMSON if emph else INK
        bars += (f'<text x="{L - 14}" y="{y + h * 0.42:.1f}" font-size="10.6" fill="{lab_col}" '
                 f'text-anchor="end" font-weight="700">{title}</text>')
        bars += (f'<text x="{L - 14}" y="{y + h * 0.42 + 14:.1f}" font-size="9.4" fill="{SLATE}" '
                 f'text-anchor="end">{sub}</text>')
        yv = y + h + 16
        for dx, lbl, val, col in ((0, "WACC", wacc, INK),
                                  (plot_w * 0.30, "total DSCR", dscr, CRIMSON),
                                  (plot_w * 0.62, "equity IRR", eqirr, BLUE)):
            bars += (f'<text x="{L + dx:.1f}" y="{yv:.1f}" font-size="9.2" fill="{SLATE}">'
                     f'{lbl}</text>')
            bars += (f'<text x="{L + dx + 62:.1f}" y="{yv:.1f}" font-size="10.6" fill="{col}" '
                     f'font-weight="700">{val}</text>')

    legend = (f'<rect x="{L}" y="{T - 32}" width="11" height="11" fill="{BLUE}" opacity="0.92"/>'
              f'<text x="{L + 16}" y="{T - 23}" font-size="10.2" fill="{INK}">'
              'senior debt (after tax)</text>'
              f'<rect x="{L + 142}" y="{T - 32}" width="11" height="11" fill="{SLATE}" '
              f'opacity="0.80"/>'
              f'<text x="{L + 158}" y="{T - 23}" font-size="10.2" fill="{INK}">'
              'mezzanine (after tax)</text>'
              f'<rect x="{L + 292}" y="{T - 32}" width="11" height="11" fill="{CRIMSON}" '
              f'opacity="0.88"/>'
              f'<text x="{L + 308}" y="{T - 23}" font-size="10.2" fill="{INK}">'
              'equity (re-levered)</text>')

    # bottom reference band — two columns, one fact per line (no inline positioning)
    by = H - B + 24
    ref = (f'<line x1="24" y1="{by - 16}" x2="{W - 24}" y2="{by - 16}" stroke="{GRID}" '
           f'stroke-width="1.4"/>'
           f'<text x="24" y="{by}" font-size="10.4" fill="{INK}" font-weight="700">'
           'Derived project WACC at the</text>'
           f'<text x="24" y="{by + 14}" font-size="10.4" fill="{INK}" font-weight="700">'
           'coverage-binding gearing of 68.6185 %</text>'
           f'<text x="24" y="{by + 32}" font-size="11.6" fill="{CRIMSON}" font-weight="800">'
           '8.0001 % &#8212; the rate Domain 4 was given</text>'
           f'<text x="398" y="{by}" font-size="10.4" fill="{INK}" font-weight="700">'
           'Sponsor group corporate WACC,</text>'
           f'<text x="398" y="{by + 14}" font-size="10.4" fill="{INK}" font-weight="700">'
           'offered for the same appraisal</text>'
           f'<text x="398" y="{by + 32}" font-size="11.6" fill="{SLATE}" font-weight="800">'
           '6.544 % &#8212; overstates NPV by 44.92 %</text>')

    body = (bars + legend + ref
            + f'<text x="24" y="28" font-size="12.5" font-weight="700" fill="{INK}">'
              'Mezzanine raises the blended cost in both directions</text>'
            + f'<text x="24" y="46" font-size="10.4" fill="{SLATE}">'
              'Kestrel Water SPC: 60,000,000 of capital, four candidate structures, cash tax '
              '20.0 %, cost of equity re-levered for each</text>'
            + f'<text x="24" y="61" font-size="10.4" fill="{CRIMSON}">'
              'Structure C reaches the highest equity return and the lowest coverage &#8212; '
              '1.0881 is below every covenant in the facility</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 12}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'The tranche a junior instrument displaces decides what it buys</text>')
    (PFL / "fig_9_2_3.svg").write_text(svg(W, H, body))
