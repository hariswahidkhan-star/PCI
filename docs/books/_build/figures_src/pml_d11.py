"""PML-AI Domain 11 — Stakeholders, communication and influence. PCI original artwork.

Fig 11.1.1  two allocations of the same 480 engagement hours: salience-proportional
            (influence x interest) against the three-term design (value-led adoption core
            320 h + consent-risk floors 130 h + residual 30 h by salience). The two
            adoption-determining groups move from 180 h to 320 h; the salience allocation
            forgoes USD 55,080 a year of recurring benefit.
Fig 11.2.1  the age of the number an executive decides on: reporting period P,
            consolidation C and paper lead time L laid out backwards from the meeting.
            Monthly design (P=4, C=1, L=2) gives newest 3.0 / mean 5.0 / maximum blindness
            7.0 weeks; the weekly-cut redesign (P=1, C=0.5, L=1) gives 1.5 / 2.0 / 2.5.
            Savings priced at Meridian's cost of delay of USD 14,280 per week.

All values are those computed in the manuscript with decimal arithmetic.
Deterministic: no randomness, no timestamps, no locale-dependent formatting.
"""


def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))

    # ================================================================ Fig 11.1.1
    # group, salience-proportional hours, designed hours, designed basis
    ROWS = (("Clinic directors", 120, 160, "value core"),
            ("Clinicians (users)", 60, 160, "value core"),
            ("Regional health authority", 90, 60, "consent floor"),
            ("National reporting body", 60, 40, "consent floor"),
            ("Patient representatives", 54, 30, "consent floor"),
            ("Practice administrators", 48, 15, "residual"),
            ("Records-system supplier", 48, 15, "residual"))
    W, H, L, R, T, B = 720, 492, 178, 172, 52, 96
    HMAX = 180.0
    rowh = (H - T - B) / len(ROWS)
    bh = rowh * 0.30

    def X(h):
        return L + h / HMAX * (W - R - L)

    grid = ""
    for h in range(0, 181, 30):
        grid += (f'<line x1="{X(h):.1f}" y1="{T}" x2="{X(h):.1f}" y2="{H-B}" stroke="{GRID}"/>'
                 f'<text x="{X(h):.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{h}</text>')

    bars = ""
    for k, (name, sal, des, basis) in enumerate(ROWS):
        y = T + k * rowh + (rowh - 2 * bh - 4) / 2
        bars += (f'<text x="{L-10}" y="{y+bh+3:.1f}" font-size="10.4" fill="{INK}" '
                 f'text-anchor="end">{name}</text>'
                 f'<rect x="{L}" y="{y:.1f}" width="{X(sal)-L:.1f}" height="{bh:.1f}" '
                 f'fill="{BLUE}" opacity="0.85"/>'
                 f'<text x="{X(sal)+6:.1f}" y="{y+bh-1:.1f}" font-size="9.6" fill="{BLUE}">'
                 f'{sal}</text>'
                 f'<rect x="{L}" y="{y+bh+4:.1f}" width="{X(des)-L:.1f}" height="{bh:.1f}" '
                 f'fill="{CRIMSON}" opacity="0.88"/>'
                 f'<text x="{X(des)+6:.1f}" y="{y+2*bh+3:.1f}" font-size="9.6" fill="{CRIMSON}">'
                 f'{des}</text>'
                 f'<text x="{W-R+8}" y="{y+bh+3:.1f}" font-size="9.2" fill="{SLATE}">'
                 f'{basis}</text>')

    # Bracket the two adoption-determining rows in the right-hand gutter.
    ytop = T + (rowh - 2 * bh - 4) / 2
    ybot = T + rowh + (rowh - 2 * bh - 4) / 2 + 2 * bh + 4
    gx = W - R + 62
    brace = (f'<path d="M{gx-5:.1f},{ytop:.1f} L{gx:.1f},{ytop:.1f} L{gx:.1f},{ybot:.1f} '
             f'L{gx-5:.1f},{ybot:.1f}" fill="none" stroke="{CRIMSON}" stroke-width="1.1"/>'
             f'<text x="{gx+5:.1f}" y="{(ytop+ybot)/2-3:.1f}" font-size="10" fill="{CRIMSON}" '
             f'font-weight="600">320 h</text>'
             f'<text x="{gx+5:.1f}" y="{(ytop+ybot)/2+10:.1f}" font-size="9.4" fill="{CRIMSON}">'
             f'was 180</text>')

    keys = (f'<rect x="{L}" y="{T-34}" width="12" height="9" fill="{BLUE}" opacity="0.85"/>'
            f'<text x="{L+18}" y="{T-26}" font-size="10.4" fill="{INK}">'
            f'Salience-proportional (influence &#215; interest)</text>'
            f'<rect x="{L+256}" y="{T-34}" width="12" height="9" fill="{CRIMSON}" opacity="0.88"/>'
            f'<text x="{L+274}" y="{T-26}" font-size="10.4" fill="{INK}">'
            f'Three-term design</text>'
            f'<text x="{L-10}" y="{T-26}" font-size="10.4" fill="{SLATE}" text-anchor="end">'
            f'480 h capacity</text>')

    note = (f'<text x="24" y="{H-24}" font-size="10.6" fill="{INK}">'
            f'Salience alone funds the 8-hour adoption intervention at 22 of 40 clinics '
            f'&#8212; 6.875 pp, USD 67,320 a year.</text>'
            f'<text x="24" y="{H-9}" font-size="10.6" fill="{CRIMSON}" font-weight="600">'
            f'The three-term design funds all 40 &#8212; 12.5 pp, USD 122,400 a year. '
            f'Forgone by salience: USD 55,080 a year.</text>')

    body = (grid + bars + brace + keys + note
            + axes(L, T, W - R, H - B, "Engagement hours allocated over the 12-month rollout", ""))
    (PML / "fig_11_1_1.svg").write_text(svg(W, H, body))

    # ================================================================ Fig 11.2.1
    # Weeks measured backwards from the decision meeting at 0.
    W, H, L, R, T, B = 700, 400, 118, 130, 56, 62
    WMAX = 7.6

    def Xw(w):                      # w = weeks before the meeting
        return (W - R) - w / WMAX * (W - R - L)

    grid = ""
    for w in (0, 1, 2, 3, 4, 5, 6, 7):
        grid += (f'<line x1="{Xw(w):.1f}" y1="{T}" x2="{Xw(w):.1f}" y2="{H-B}" stroke="{GRID}"/>'
                 f'<text x="{Xw(w):.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" '
                 f'text-anchor="middle">{w}</text>')

    # track: (label, period start, period end, consolidation end, ages)
    TRACKS = ((("Monthly pack", "P = 4  C = 1  L = 2"), 7.0, 3.0, 2.0, (3.0, 5.0, 7.0), BLUE, 0),
              (("Weekly cut", "P = 1  C = 0.5  L = 1"), 2.5, 1.5, 1.0, (1.5, 2.0, 2.5), CRIMSON, 1))
    trh = (H - T - B) / 2
    body = grid
    for (lab, params), ps, pe, ce, ages, col, idx in TRACKS:
        y = T + idx * trh + trh * 0.20
        bh = trh * 0.30
        body += (f'<text x="{L-12}" y="{y+bh*0.5+2:.1f}" font-size="11" fill="{INK}" '
                 f'text-anchor="end" font-weight="600">{lab}</text>'
                 f'<text x="{L-12}" y="{y+bh*0.5+16:.1f}" font-size="9.4" fill="{SLATE}" '
                 f'text-anchor="end">{params}</text>'
                 # reporting period
                 f'<rect x="{Xw(ps):.1f}" y="{y:.1f}" width="{Xw(pe)-Xw(ps):.1f}" '
                 f'height="{bh:.1f}" fill="{col}" opacity="0.20" stroke="{col}" '
                 f'stroke-width="0.9"/>'
                 f'<text x="{(Xw(ps)+Xw(pe))/2:.1f}" y="{y+bh*0.68:.1f}" font-size="9.4" '
                 f'fill="{INK}" text-anchor="middle">reporting period</text>'
                 # consolidation
                 f'<rect x="{Xw(pe):.1f}" y="{y:.1f}" width="{Xw(ce)-Xw(pe):.1f}" '
                 f'height="{bh:.1f}" fill="{SLATE}" opacity="0.30"/>'
                 # paper lead time
                 f'<rect x="{Xw(ce):.1f}" y="{y:.1f}" width="{Xw(0)-Xw(ce):.1f}" '
                 f'height="{bh:.1f}" fill="{col}" opacity="0.55"/>')
        newest, mean, oldest = ages
        for w, tag, dy in ((oldest, f"blind {oldest:.1f} w", 21),
                           (mean, f"mean {mean:.1f} w", 37),
                           (newest, f"newest {newest:.1f} w", 53)):
            body += (f'<line x1="{Xw(w):.1f}" y1="{y+bh:.1f}" x2="{Xw(w):.1f}" '
                     f'y2="{y+bh+dy-9:.1f}" stroke="{col}" stroke-width="0.9" '
                     f'stroke-dasharray="3 3"/>'
                     f'<circle cx="{Xw(w):.1f}" cy="{y+bh:.1f}" r="3.2" fill="{col}"/>'
                     f'<text x="{Xw(w)-6:.1f}" y="{y+bh+dy:.1f}" font-size="9.8" fill="{col}" '
                     f'text-anchor="end">{tag}</text>')

    body += (f'<line x1="{Xw(0):.1f}" y1="{T-8}" x2="{Xw(0):.1f}" y2="{H-B}" stroke="{INK}" '
             f'stroke-width="1.6"/>'
             f'<text x="{Xw(0)+5:.1f}" y="{T-12}" font-size="10.6" fill="{INK}" '
             f'font-weight="600">decision</text>'
             f'<text x="{Xw(0)+5:.1f}" y="{T+1}" font-size="10.6" fill="{INK}" '
             f'font-weight="600">meeting</text>')

    saves = (f'<text x="{W-R+8}" y="{T+72}" font-size="9.8" fill="{SLATE}">Weeks saved</text>'
             f'<text x="{W-R+8}" y="{T+86}" font-size="9.8" fill="{INK}">newest 1.5 '
             f'&#8594; 21,420</text>'
             f'<text x="{W-R+8}" y="{T+100}" font-size="9.8" fill="{INK}">mean 3.0 '
             f'&#8594; 42,840</text>'
             f'<text x="{W-R+8}" y="{T+114}" font-size="9.8" fill="{CRIMSON}" '
             f'font-weight="600">blind 4.5 &#8594; 64,260</text>'
             f'<text x="{W-R+8}" y="{T+132}" font-size="9.2" fill="{SLATE}">at USD 14,280</text>'
             f'<text x="{W-R+8}" y="{T+144}" font-size="9.2" fill="{SLATE}">per week</text>')

    lx, ly = L + 16, T + 196
    for j, (lab, col, op) in enumerate((("reporting period P", BLUE, "0.20"),
                                        ("consolidation C", SLATE, "0.30"),
                                        ("paper lead time L", BLUE, "0.55"))):
        body += (f'<rect x="{lx}" y="{ly + j*16 - 8}" width="13" height="9" fill="{col}" '
                 f'opacity="{op}" stroke="{SLATE}" stroke-width="0.5"/>'
                 f'<text x="{lx+19}" y="{ly + j*16}" font-size="9.6" fill="{INK}">{lab}</text>')

    body += saves + axes(L, T, W - R, H - B,
                         "Weeks before the decision meeting", "")
    (PML / "fig_11_2_1.svg").write_text(svg(W, H, body))
