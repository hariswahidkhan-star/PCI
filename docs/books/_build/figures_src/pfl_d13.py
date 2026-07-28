"""PFL-AI Domain 13 — Due diligence and financial close. PCI original artwork.

Fig 13.1.1  The breakeven detection rate of each of Kestrel's seven diligence streams,
            drawn twice: once as the stream costs inside a common twelve-week envelope
            (fee only) and once as the stream is sequenced serially on the critical path
            (fee plus its own elapsed time at 124,133.33 a week). Inside the envelope every
            stream pays above a 31.25 % detection rate; sequenced serially, five need
            72.02–99.42 % and two cannot pay at any detection rate at all.
Fig 13.3.1  Kestrel's five conditions-precedent chains against the twenty-week base close
            date, showing duration, float and the slip exposure that survives float, beside
            the resulting close-date distribution — 34.125 % at twenty weeks, 11.375 % at
            twenty-two and 54.500 % at twenty-four, giving an expected close of 22.4075
            weeks against a critical chain whose own expected duration is 21.2000.

Deterministic: every annotated value is a literal verified in the domain's decimal
arithmetic pass (see the manuscript's printed-values register).
"""


def make(ctx):
    svg, axes = ctx["svg"], ctx["axes"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    PFL = ctx["PFL"]

    # ---- Fig 13.1.1 — breakeven detection rate, envelope against serial ----------
    # (label, fee, weeks, d* inside the envelope %, d* sequenced serially %)
    streams = [
        ("Market", 190000, 9, 13.31, 91.54),
        ("Technical &#8212; independent engineer", 260000, 8, 14.94, 72.02),
        ("Environmental and social", 240000, 12, 16.53, 119.12),
        ("Tax", 150000, 6, 16.67, 99.42),
        ("Legal", 420000, 10, 19.20, 75.95),
        ("Financial &#8212; model audit, accounts", 180000, 5, 19.55, 86.95),
        ("Insurance", 60000, 4, 31.25, 289.86),
    ]
    W, H = 760, 480
    L, R, T, B = 250, 108, 104, 64
    plot_w, plot_h = W - L - R, H - T - B
    hi = 130.0                                   # % axis ceiling; 289.86 is clipped and labelled
    n = len(streams)
    row_h = plot_h / n

    def X(pct):
        return L + min(pct, hi) / hi * plot_w

    grid = "".join(
        f'<line x1="{X(v):.1f}" y1="{T}" x2="{X(v):.1f}" y2="{H - B}" stroke="{GRID}"/>'
        f'<text x="{X(v):.1f}" y="{H - B + 17}" font-size="9.8" fill="{SLATE}" '
        f'text-anchor="middle">{v} %</text>'
        for v in (0, 20, 40, 60, 80, 100, 120))
    rows = ""
    for i, (lab, fee, wk, d_env, d_ser) in enumerate(streams):
        yc = T + i * row_h + row_h / 2
        x_env, x_ser = X(d_env), X(d_ser)
        clipped = d_ser > hi
        rows += (f'<line x1="{x_env:.1f}" y1="{yc:.1f}" x2="{x_ser:.1f}" y2="{yc:.1f}" '
                 f'stroke="{SLATE}" stroke-width="1.5" opacity="0.55"/>')
        if clipped:
            rows += (f'<polygon points="{x_ser:.1f},{yc - 6:.1f} {x_ser + 11:.1f},{yc:.1f} '
                     f'{x_ser:.1f},{yc + 6:.1f}" fill="{CRIMSON}"/>')
        else:
            rows += f'<circle cx="{x_ser:.1f}" cy="{yc:.1f}" r="5.2" fill="{CRIMSON}"/>'
        rows += f'<circle cx="{x_env:.1f}" cy="{yc:.1f}" r="5.2" fill="{BLUE}"/>'
        rows += (f'<text x="{L - 14}" y="{yc - 2:.1f}" font-size="10.4" fill="{INK}" '
                 f'text-anchor="end" font-weight="600">{lab}</text>')
        rows += (f'<text x="{L - 14}" y="{yc + 11:.1f}" font-size="9.2" fill="{SLATE}" '
                 f'text-anchor="end">fee {fee:,} &#183; {wk} weeks</text>')
        rows += (f'<text x="{x_env - 9:.1f}" y="{yc + 4:.1f}" font-size="9.6" '
                 f'font-weight="700" fill="{BLUE}" text-anchor="end">{d_env:.2f}</text>')
        tail = f'{d_ser:.2f} %' if not clipped else f'{d_ser:.2f} % &#8212; impossible'
        rows += (f'<text x="{(x_ser + 15) if clipped else (x_ser + 9):.1f}" y="{yc + 4:.1f}" '
                 f'font-size="9.8" font-weight="700" fill="{CRIMSON}" '
                 f'text-anchor="start">{tail}</text>')
    ceiling = (f'<line x1="{X(100):.1f}" y1="{T - 6}" x2="{X(100):.1f}" y2="{H - B}" '
               f'stroke="{INK}" stroke-width="1.6" stroke-dasharray="5 4"/>'
               f'<text x="{X(100) - 6:.1f}" y="{T - 12}" font-size="10.4" font-weight="700" '
               f'fill="{INK}" text-anchor="end">100 % &#8212; no review detects everything</text>')
    body = (grid + ceiling + rows
            + axes(L, T, W - R, H - B,
                   "Breakeven detection rate &#8212; (fee + priced elapsed time) &#247; "
                   "p &#215; (C &#8722; F)", "")
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'Diligence is bought with time, not money: the same seven streams are worth '
              '+4,066,700 in parallel and &#8722;1,146,900 in series</text>'
            + f'<text x="24" y="45" font-size="10.4" fill="{BLUE}">'
              'Blue &#8212; the stream sits inside a common twelve-week envelope, so only its '
              '1,500,000 of fees is charged against it</text>'
            + f'<text x="24" y="61" font-size="10.4" fill="{CRIMSON}">'
              'Crimson &#8212; the stream is sequenced serially, so it also carries its own '
              'elapsed weeks at 124,133.33 each</text>'
            + f'<text x="24" y="77" font-size="10.4" fill="{SLATE}">'
              'Insurance and environmental-and-social review cannot pay in series at any '
              'detection rate: the delay alone exceeds the loss avoided</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 15}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'The 5,213,600 that separates the two readings is 42 weeks of scheduling &#8212; '
              'not one hour of diligence</text>')
    (PFL / "fig_13_1_1.svg").write_text(svg(W, H, body))

    # ---- Fig 13.3.1 — the conditions-precedent conjunction ----------------------
    # (chain, duration wk, slip probability, slip wk, float wk, exposed wk)
    chains = [
        ("C &#183; advisers, model audit, credit approval", 20, 0.30, 4, 0, 4),
        ("B &#183; permits, land instrument, licence", 18, 0.35, 6, 2, 4),
        ("D &#183; finance and security documents", 17, 0.25, 5, 3, 2),
        ("A &#183; sponsor authorisations and equity", 9, 0.20, 3, 11, 0),
        ("E &#183; insurance placement and endorsements", 7, 0.15, 2, 13, 0),
    ]
    W, H = 770, 470
    L, R, T, B = 262, 210, 100, 66
    plot_w, plot_h = W - L - R, H - T - B
    maxw = 26.0
    row_h = plot_h / len(chains)

    def Xw(wk):
        return L + wk / maxw * plot_w

    grid = "".join(
        f'<line x1="{Xw(v):.1f}" y1="{T}" x2="{Xw(v):.1f}" y2="{H - B}" stroke="{GRID}"/>'
        f'<text x="{Xw(v):.1f}" y="{H - B + 17}" font-size="9.8" fill="{SLATE}" '
        f'text-anchor="middle">{v}</text>'
        for v in (0, 4, 8, 12, 16, 20, 24))
    rows = ""
    for i, (lab, dur, pq, slip, flt, exp) in enumerate(chains):
        y = T + i * row_h + row_h * 0.22
        h = row_h * 0.40
        rows += (f'<rect x="{Xw(0):.1f}" y="{y:.1f}" width="{Xw(dur) - Xw(0):.1f}" '
                 f'height="{h:.1f}" fill="{BLUE}" opacity="0.88"/>')
        if flt:
            rows += (f'<rect x="{Xw(dur):.1f}" y="{y:.1f}" width="{Xw(dur + flt) - Xw(dur):.1f}" '
                     f'height="{h:.1f}" fill="{SLATE}" opacity="0.20"/>')
        if slip:
            col = CRIMSON if exp else SLATE
            op = "0.85" if exp else "0.35"
            rows += (f'<rect x="{Xw(dur):.1f}" y="{y + h * 0.28:.1f}" '
                     f'width="{Xw(dur + slip) - Xw(dur):.1f}" height="{h * 0.44:.1f}" '
                     f'fill="{col}" opacity="{op}"/>')
        rows += (f'<text x="{L - 14}" y="{y + h * 0.52:.1f}" font-size="10.3" fill="{INK}" '
                 f'text-anchor="end" font-weight="600">{lab}</text>')
        rows += (f'<text x="{L - 14}" y="{y + h * 0.52 + 12.5:.1f}" font-size="9.2" '
                 f'fill="{SLATE}" text-anchor="end">{dur} wk &#183; float {flt} &#183; '
                 f'slip {slip} wk at p {pq:.2f} &#183; risk weight {pq * slip:.2f}</text>')
        tag = (f'{exp} wk exposed' if exp else 'absorbed by float')
        rows += (f'<text x="{W - R + 8}" y="{y + h * 0.62:.1f}" font-size="10" '
                 f'font-weight="700" fill="{CRIMSON if exp else SLATE}">{tag}</text>')
    base = (f'<line x1="{Xw(20):.1f}" y1="{T - 8}" x2="{Xw(20):.1f}" y2="{H - B}" '
            f'stroke="{INK}" stroke-width="1.6"/>'
            f'<text x="{Xw(20) + 6:.1f}" y="{T - 14}" font-size="10.4" font-weight="700" '
            f'fill="{INK}">base close 20 wk</text>'
            f'<line x1="{Xw(22.4075):.1f}" y1="{T - 8}" x2="{Xw(22.4075):.1f}" y2="{H - B}" '
            f'stroke="{CRIMSON}" stroke-width="1.6" stroke-dasharray="5 4"/>'
            f'<text x="{Xw(22.4075) + 6:.1f}" y="{T - 1:.1f}" font-size="10.4" '
            f'font-weight="700" fill="{CRIMSON}">E[close] 22.4075 wk</text>')
    # close-date distribution, drawn as a small panel bottom right
    px, py, pw = W - R + 8, T + plot_h * 0.60, 190.0
    dist = [("20 wk", 34.125, INK), ("22 wk", 11.375, SLATE), ("24 wk", 54.500, CRIMSON)]
    panel = (f'<text x="{px:.1f}" y="{py - 22:.1f}" font-size="10.4" font-weight="700" '
             f'fill="{INK}">Close-date distribution</text>')
    for j, (lab, pct, col) in enumerate(dist):
        yy = py + j * 19
        panel += (f'<rect x="{px + 44:.1f}" y="{yy - 9:.1f}" width="{pct / 60.0 * (pw - 96):.1f}" '
                  f'height="11" fill="{col}" opacity="0.85"/>'
                  f'<text x="{px:.1f}" y="{yy:.1f}" font-size="9.8" fill="{SLATE}">{lab}</text>'
                  f'<text x="{px + 48 + pct / 60.0 * (pw - 96):.1f}" y="{yy:.1f}" '
                  f'font-size="9.8" font-weight="700" fill="{col}">{pct:.3f} %</text>')
    body = (grid + rows + base + panel
            + axes(L, T, W - R, H - B,
                   "Weeks from mandate to satisfaction of the chain&#8217;s last condition", "")
            + f'<text x="24" y="26" font-size="12.5" font-weight="700" fill="{INK}">'
              'The close date is the expected maximum of the chains, not the expected duration '
              'of the critical one</text>'
            + f'<text x="24" y="45" font-size="10.4" fill="{SLATE}">'
              'Five chains, each individually likely to hold its date &#8212; and only a '
              '34.125 % chance that the close holds its own</text>'
            + f'<text x="24" y="61" font-size="10.4" fill="{CRIMSON}">'
              'Expected slip 2.4075 wk = 298,851; chain C&#8217;s own expected slip is 1.2 wk '
              '= 148,960. The conjunction premium is 149,891</text>'
            + f'<text x="24" y="77" font-size="10.4" fill="{BLUE}">'
              'Chain B carries the largest risk weight (2.10) and is not the critical path: '
              'de-risking it to p 0.10 is worth 76,031.67</text>'
            + f'<text x="{(L + W - R) / 2:.1f}" y="{H - 15}" font-size="11.2" '
              f'font-weight="600" fill="{INK}" text-anchor="middle">'
              'Blue: chain duration. Grey: float. Crimson: the slip that float cannot absorb'
              '</text>')
    (PFL / "fig_13_3_1.svg").write_text(svg(W, H, body))
