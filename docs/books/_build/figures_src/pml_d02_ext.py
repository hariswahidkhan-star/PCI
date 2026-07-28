"""PML-AI Domain 2 — Strategy, Selection and Business Alignment (depth extension).
PCI original artwork.

Fig 2.1.1  The strategy-portfolio alignment index (KA 2.1.1). Paired horizontal bars for four
           declared strategic objectives: declared weight (slate) against funded share of the
           strategically mapped USD 18,000,000 (brand blue) — Access 40.0000 % / 26.6667 %,
           Safety 25.0000 % / 36.6667 %, Digital 20.0000 % / 10.0000 %, Cost efficiency
           15.0000 % / 26.6667 %. Deficits are marked in crimson. A side panel prints the
           alignment index on mapped spend (76.6667 %), the index on total discretionary spend
           (59.0000 %), the 40.0000 % unmapped sustainment share and the reallocation distance
           USD 4,200,000 = (1 &#8722; 0.766667) &#215; 18,000,000.

Fig 2.3.2  Four breakevens from one programme (KA 2.2.2 / 2.3.2). A single sustained-adoption axis
           from 25 % to 50 % carrying the flat-equivalent breakeven adoption under four treatments
           of the same Meridian facts: 31.4498 % (honest do-nothing baseline, 6.0 h), 35.6035 %
           (honest baseline and attributable 5.3 h), 41.0460 % (the case as approved) and
           46.4672 % (attribution corrected, counterfactual still omitted). Meridian&#8217;s actual
           40.0000 % sustained adoption is drawn as a crimson rule, showing that it clears two of
           the four thresholds and misses two — the same programme is value-creating or
           value-destroying according to which baseline errors were made.

Fig 2.4.1  Why a kill criterion on a weak signal destroys value (KA 2.4.3). Two columns for the
           month-6 criterion of the worked example: value saved by stopping the Low-adoption
           projects it correctly flags (USD 33,364) against value destroyed by stopping the Mid and
           High projects it wrongly flags (USD 208,693, split 141,706 / 66,987), with the net
           USD (175,328) annotated. A footnote panel prints the forward breakeven adoption at the
           gate (28.1283 %), the ramped-basis investment breakeven (45.0053 %) and the maximum
           false-flag rate on Mid projects that a perfect Low detector could tolerate (10.3009 %).

All values are those computed in the manuscript with decimal arithmetic. Deterministic output: no
randomness, no timestamps, no locale-dependent formatting.
"""


def make(ctx):
    svg, PML = ctx["svg"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ================================================ Fig 2.1.1 — the alignment index
    W, H = 700, 400
    L, R, T, B = 148, 214, 46, 54
    XMAX = 45.0                                   # per cent of mapped spend

    def X(v):
        return L + v / XMAX * (W - R - L)

    ROWS = (("Access and capacity", 40.0, 26.6667, True),
            ("Safety and quality", 25.0, 36.6667, False),
            ("Digital foundation", 20.0, 10.0, True),
            ("Cost efficiency", 15.0, 26.6667, False))
    rowh = (H - T - B) / len(ROWS)
    body = ""
    for pct in range(0, 46, 5):
        body += (f'<line x1="{X(pct):.1f}" y1="{T}" x2="{X(pct):.1f}" y2="{H-B}" stroke="{GRID}"/>'
                 f'<text x="{X(pct):.1f}" y="{H-B+16}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="middle">{pct} %</text>')
    for k, (name, decl, act, short) in enumerate(ROWS):
        y = T + k * rowh + 8
        bh = 12.6
        body += (f'<text x="{L-10}" y="{y+bh+3:.1f}" font-size="10.4" fill="{INK}" '
                 f'text-anchor="end">{name}</text>')
        body += (f'<rect x="{L}" y="{y:.1f}" width="{X(decl)-L:.1f}" height="{bh}" '
                 f'fill="{SLATE}" opacity="0.55"/>'
                 f'<text x="{X(decl)+5:.1f}" y="{y+bh-2.4:.1f}" font-size="9.4" fill="{SLATE}">'
                 f'{decl:.4f} % declared</text>')
        body += (f'<rect x="{L}" y="{y+bh+3.4:.1f}" width="{X(act)-L:.1f}" height="{bh}" '
                 f'fill="{BLUE}"/>'
                 f'<text x="{X(act)+5:.1f}" y="{y+2*bh+1:.1f}" font-size="9.4" fill="{BLUE}" '
                 f'font-weight="600">{act:.4f} % funded</text>')
        if short:
            body += (f'<line x1="{X(act):.1f}" y1="{y+bh+3.4+bh/2:.1f}" x2="{X(decl):.1f}" '
                     f'y2="{y+bh+3.4+bh/2:.1f}" stroke="{CRIMSON}" stroke-width="1.6" '
                     f'stroke-dasharray="4 3"/>')
    body += (f'<line x1="{L}" y1="{T}" x2="{L}" y2="{H-B}" stroke="{INK}" stroke-width="1.2"/>'
             f'<text x="{(L+W-R)/2:.1f}" y="{H-B+36}" font-size="11.4" fill="{SLATE}" '
             f'text-anchor="middle">Share of the strategically mapped USD 18,000,000</text>')
    panel = [("Alignment index, mapped spend", "76.6667 %", BLUE),
             ("Alignment index, all spend", "59.0000 %", SLATE),
             ("Unmapped sustainment", "40.0000 %", SLATE),
             ("Reallocation distance", "USD 4,200,000", CRIMSON),
             ("&#8212; deficits = surpluses", "4,200,000 each side", SLATE)]
    px = W - R + 12
    body += (f'<text x="{px}" y="{T-14}" font-size="10.6" fill="{INK}" font-weight="600">'
             f'What the picture measures</text>')
    for k, (lab, val, col) in enumerate(panel):
        yy = T + 6 + k * 34
        body += (f'<text x="{px}" y="{yy}" font-size="9.4" fill="{SLATE}">{lab}</text>'
                 f'<text x="{px}" y="{yy+14}" font-size="11.4" fill="{col}" '
                 f'font-weight="700">{val}</text>')
    (PML / "fig_2_1_1.svg").write_text(svg(W, H, body))

    # ==================================== Fig 2.3.2 — four breakevens from one programme
    W, H = 700, 340
    L, R, T, B = 92, 88, 62, 96
    A0, A1 = 25.0, 50.0

    def Xa(v):
        return L + (v - A0) / (A1 - A0) * (W - R - L)

    AXIS = H - B
    body = (f'<line x1="{L}" y1="{AXIS}" x2="{W-R}" y2="{AXIS}" stroke="{INK}" '
            f'stroke-width="1.4"/>')
    for pct in range(25, 51, 5):
        body += (f'<line x1="{Xa(pct):.1f}" y1="{AXIS}" x2="{Xa(pct):.1f}" y2="{AXIS+6}" '
                 f'stroke="{INK}"/>'
                 f'<text x="{Xa(pct):.1f}" y="{AXIS+20}" font-size="9.8" fill="{SLATE}" '
                 f'text-anchor="middle">{pct} %</text>')
    body += (f'<text x="{(L+W-R)/2:.1f}" y="{AXIS+44}" font-size="11.4" fill="{SLATE}" '
             f'text-anchor="middle">Flat-equivalent sustained adoption at which NPV = 0</text>')
    MARKS = (("31.4498 %", 31.4498, "honest do-nothing baseline, 6.0 h", 126, BLUE),
             ("35.6035 %", 35.6035, "honest baseline and attributable 5.3 h", 60, BLUE),
             ("41.0460 %", 41.0460, "the case as approved", 96, INK),
             ("46.4672 %", 46.4672, "attribution corrected, baseline still omitted", 30, SLATE))
    for lab, v, note, up, col in MARKS:
        x = Xa(v)
        body += (f'<line x1="{x:.1f}" y1="{AXIS}" x2="{x:.1f}" y2="{AXIS-up}" stroke="{col}" '
                 f'stroke-width="1.6"/>'
                 f'<circle cx="{x:.1f}" cy="{AXIS:.1f}" r="4.2" fill="{col}"/>'
                 f'<text x="{x:.1f}" y="{AXIS-up-16:.1f}" font-size="11.6" fill="{col}" '
                 f'font-weight="700" text-anchor="middle">{lab}</text>'
                 f'<text x="{x:.1f}" y="{AXIS-up-4:.1f}" font-size="9.2" fill="{SLATE}" '
                 f'text-anchor="middle">{note}</text>')
    xa = Xa(40.0)
    body += (f'<line x1="{xa:.1f}" y1="{T-24}" x2="{xa:.1f}" y2="{AXIS+30}" stroke="{CRIMSON}" '
             f'stroke-width="2.2"/>'
             f'<text x="{xa+7:.1f}" y="{T-28}" font-size="11.4" fill="{CRIMSON}" '
             f'font-weight="700">Meridian&#8217;s actual sustained adoption 40.0000 %</text>')
    body += (f'<text x="{L}" y="{AXIS+66}" font-size="9.8" fill="{SLATE}">Clears: 31.4498 % and '
             f'35.6035 % &#8212; value created.</text>'
             f'<text x="{W-R}" y="{AXIS+66}" font-size="9.8" fill="{SLATE}" text-anchor="end">'
             f'Misses: 41.0460 % and 46.4672 % &#8212; value destroyed.</text>'
             f'<text x="{L}" y="{AXIS+80}" font-size="9.8" fill="{INK}">Same facts, same cost, '
             f'same horizon: the verdict is set by which two baseline errors were made.</text>')
    (PML / "fig_2_3_2.svg").write_text(svg(W, H, body))

    # ============================ Fig 2.4.1 — why a kill criterion on a weak signal loses
    W, H = 700, 420
    L, R, T, B = 92, 250, 46, 74
    VMAX = 240000.0

    def Yv(v):
        return (H - B) - v / VMAX * (H - T - B)

    body = ""
    for v in range(0, 240001, 40000):
        body += (f'<line x1="{L}" y1="{Yv(v):.1f}" x2="{W-R}" y2="{Yv(v):.1f}" stroke="{GRID}"/>'
                 f'<text x="{L-8}" y="{Yv(v)+4:.1f}" font-size="9.6" fill="{SLATE}" '
                 f'text-anchor="end">{int(v/1000)}k</text>')
    bw = 96
    x1 = L + 44
    x2 = L + 232
    body += (f'<rect x="{x1}" y="{Yv(33364.45):.1f}" width="{bw}" '
             f'height="{(H-B)-Yv(33364.45):.1f}" fill="{BLUE}"/>'
             f'<text x="{x1+bw/2}" y="{Yv(33364.45)-8:.1f}" font-size="11.4" fill="{BLUE}" '
             f'font-weight="700" text-anchor="middle">33,364</text>')
    body += (f'<rect x="{x2}" y="{Yv(141705.89):.1f}" width="{bw}" '
             f'height="{(H-B)-Yv(141705.89):.1f}" fill="{CRIMSON}" opacity="0.9"/>'
             f'<rect x="{x2}" y="{Yv(208692.82):.1f}" width="{bw}" '
             f'height="{Yv(141705.89)-Yv(208692.82):.1f}" fill="{CRIMSON}" opacity="0.55"/>'
             f'<text x="{x2+bw/2}" y="{Yv(208692.82)-8:.1f}" font-size="11.4" fill="{CRIMSON}" '
             f'font-weight="700" text-anchor="middle">208,693</text>'
             f'<text x="{x2+bw+7}" y="{Yv(75000):.1f}" font-size="9.2" fill="{INK}">'
             f'Mid wrongly stopped 141,706</text>'
             f'<text x="{x2+bw+7}" y="{Yv(175000):.1f}" font-size="9.2" fill="{INK}">'
             f'High wrongly stopped 66,987</text>')
    body += (f'<line x1="{L}" y1="{H-B}" x2="{W-R}" y2="{H-B}" stroke="{INK}" stroke-width="1.2"/>'
             f'<line x1="{L}" y1="{T}" x2="{L}" y2="{H-B}" stroke="{INK}" stroke-width="1.2"/>'
             f'<text x="{x1+bw/2}" y="{H-B+18}" font-size="10.2" fill="{INK}" '
             f'text-anchor="middle">Value saved</text>'
             f'<text x="{x1+bw/2}" y="{H-B+31}" font-size="9.2" fill="{SLATE}" '
             f'text-anchor="middle">Low projects correctly stopped</text>'
             f'<text x="{x2+bw/2}" y="{H-B+18}" font-size="10.2" fill="{INK}" '
             f'text-anchor="middle">Value destroyed</text>'
             f'<text x="{x2+bw/2}" y="{H-B+31}" font-size="9.2" fill="{SLATE}" '
             f'text-anchor="middle">Mid and High wrongly stopped</text>'
             f'<text x="{L-52}" y="{(T+H-B)/2:.1f}" font-size="11.4" fill="{SLATE}" '
             f'text-anchor="middle" transform="rotate(-90 {L-52} {(T+H-B)/2:.1f})">'
             f'Expected value (USD)</text>')
    body += (f'<text x="{(x1+x2+bw)/2:.1f}" y="{T-16}" font-size="12.4" fill="{CRIMSON}" '
             f'font-weight="700" text-anchor="middle">Net effect of the criterion: '
             f'USD (175,328)</text>')
    panel = (("Forward breakeven adoption at the gate", "28.1283 %"),
             ("Investment breakeven, ramped basis", "45.0053 %"),
             ("Continue everything, E[NPV]", "USD 1,033,038"),
             ("Stop on a weak signal, E[NPV]", "USD 857,709"),
             ("Tolerable false-flag rate on Mid", "10.3009 %"))
    px = W - R + 16
    body += (f'<text x="{px}" y="{T-16}" font-size="10.6" fill="{INK}" font-weight="600">'
             f'The calibration figures</text>')
    for k, (lab, val) in enumerate(panel):
        yy = T + 10 + k * 40
        body += (f'<text x="{px}" y="{yy}" font-size="9.4" fill="{SLATE}">{lab}</text>'
                 f'<text x="{px}" y="{yy+15}" font-size="11.6" fill="{INK}" '
                 f'font-weight="700">{val}</text>')
    (PML / "fig_2_4_1.svg").write_text(svg(W, H, body))
