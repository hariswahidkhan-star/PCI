#!/usr/bin/env python3
"""Inline SVG figures for the course-outline deck.

Project controls is a discipline of curves, fans and variances, and a deck about it that carries no
data graphics is a text document cut into slides. These are the figures the argument needs.

**Colour.** Two hues only: a lightened brand navy for what happened, brand crimson for what is
forecast. Validated with the dataviz skill's checker against the light surface — lightness band,
chroma floor, CVD separation (protan ΔE 26.1, tritan 31.4), normal-vision floor and contrast all
pass. Three hues were tried first so the three EAC branches could each have their own; dark gold
against crimson gives a deuteranope ΔE of 0.6, which is no separation at all. The branches are
distinguished the way the manuscript's own figure specification distinguishes them — dashed, fanning
apart, each directly labelled with its method and its final value — so hue never has to carry it.

**Data.** Figure 6.3.7 is the manuscript's own, values and all. Nothing here is invented.
"""

BLUE = "#2F5BD0"      # what happened — a step lighter than the logo navy so it reads as a mark
CRIMSON = "#C13329"   # what is forecast — the logo's accent bar
INK = "#0F172A"
SLATE = "#475569"
MIST = "#64748B"
GRID = "#DDE3EB"
NEUTRAL = "#94A3B8"   # baseline and reference lines: present, never competing


def _txt(x, y, s, size=15, fill=SLATE, weight=500, anchor="start"):
    return (f"<text x='{x:.1f}' y='{y:.1f}' font-family='Inter' font-size='{size}' "
            f"font-weight='{weight}' fill='{fill}' text-anchor='{anchor}'>{s}</text>")


def eac_fan(w=880, h=430):
    """Figure 6.3.7 — the EAC fan. Manuscript data, USD thousands, months 1–6.

    Three forecasts leave the same point and land 130,000 apart. That picture is the argument for
    naming your method, and it makes it faster than any sentence can.
    """
    pv = [60, 150, 270, 420, 540, 600]
    ac = {3: 280, 5: 506.25}
    branches = [(701.25, "Method 1", "one-off overrun"),
                (750.0, "Method 2", "efficiency persists"),
                (831.25, "Method 3", "cost + schedule")]
    bac = 600
    # 701 and 750 are 49 apart on a 900 scale — about 20 px of plot. A two-line endpoint label needs
    # 34, so the labels collided. Each endpoint now carries one line, and the assumption behind each
    # method moves to a legend under the plot where it has room to be read.
    L, R, T, B = 58, 214, 26, 84
    ymax = 900
    px = lambda m: L + (m - 1) * (w - L - R) / 5
    py = lambda v: T + (1 - v / ymax) * (h - T - B)

    s = [f"<svg viewBox='0 0 {w} {h}' "
         f"xmlns='http://www.w3.org/2000/svg' role='img'>"]

    # Recessive grid, then the axis line. Gridlines are context, not content.
    for v in (0, 300, 600, 900):
        s.append(f"<line x1='{L}' y1='{py(v):.1f}' x2='{w - R + 8}' y2='{py(v):.1f}' "
                 f"stroke='{GRID}' stroke-width='1'/>")
        s.append(_txt(L - 10, py(v) + 5, f"{v}", 13, MIST, 500, "end"))
    for m in range(1, 7):
        s.append(_txt(px(m), h - 58, f"M{m}", 13, MIST, 500, "middle"))
    s.append(_txt(L - 10, T - 8, "$k", 13, MIST, 600, "end"))

    # BAC reference — dotted, neutral, labelled where it will not collide with a branch.
    s.append(f"<line x1='{L}' y1='{py(bac):.1f}' x2='{w - R + 8}' y2='{py(bac):.1f}' "
             f"stroke='{NEUTRAL}' stroke-width='2' stroke-dasharray='2 5'/>")
    s.append(_txt(L + 6, py(bac) - 10, "BAC 600", 13, MIST, 700))

    # PV baseline — the plan.
    pts = " ".join(f"{px(i + 1):.1f},{py(v):.1f}" for i, v in enumerate(pv))
    s.append(f"<polyline points='{pts}' fill='none' stroke='{NEUTRAL}' stroke-width='2' "
             f"stroke-linejoin='round'/>")
    # Series labels sit clear of their own line, not across it.
    s.append(_txt(px(1) + 4, py(pv[2]) + 4, "PV baseline", 14, MIST, 700))

    # AC — what actually happened, through the two measured points.
    s.append(f"<line x1='{px(3):.1f}' y1='{py(ac[3]):.1f}' x2='{px(5):.1f}' y2='{py(ac[5]):.1f}' "
             f"stroke='{BLUE}' stroke-width='3' stroke-linecap='round'/>")
    for m, v in ac.items():
        s.append(f"<circle cx='{px(m):.1f}' cy='{py(v):.1f}' r='6' fill='{BLUE}' "
                 f"stroke='#FFFFFF' stroke-width='2'/>")
    s.append(_txt(px(3) + 10, py(ac[3]) + 30, "AC actual", 14, BLUE, 700))

    # The fan. Dashed, one hue, each endpoint labelled — the manuscript's own encoding.
    for val, name, _why in branches:
        s.append(f"<line x1='{px(5):.1f}' y1='{py(ac[5]):.1f}' x2='{px(6):.1f}' y2='{py(val):.1f}' "
                 f"stroke='{CRIMSON}' stroke-width='2.5' stroke-dasharray='7 5' "
                 f"stroke-linecap='round'/>")
        s.append(f"<circle cx='{px(6):.1f}' cy='{py(val):.1f}' r='6' fill='{CRIMSON}' "
                 f"stroke='#FFFFFF' stroke-width='2'/>")
        s.append(_txt(px(6) + 14, py(val) + 6, f"{val:,.0f}", 17, INK, 800))
        s.append(_txt(px(6) + 76, py(val) + 6, name, 14, MIST, 600))

    # Legend below the axis, each item carrying the dashed swatch of the branch it names, so identity
    # is never colour alone. Sits under the month labels rather than across them.
    ly = h - 16
    lx = L
    for _val, name, why in branches:
        s.append(f"<line x1='{lx}' y1='{ly - 5}' x2='{lx + 22}' y2='{ly - 5}' stroke='{CRIMSON}' "
                 f"stroke-width='2.5' stroke-dasharray='7 5'/>")
        s.append(_txt(lx + 30, ly, f"{name} — {why}", 13.5, MIST, 500))
        lx += 272
    s.append("</svg>")
    return "".join(s)


def cpi_gap(w=880, h=300):
    """The same month-end, read two ways. Two bars and the gap between them.

    A magnitude comparison of two values is a bar chart; the point is the distance between them, so
    the distance is the thing that gets annotated.
    """
    L, T, BW, GAP = 60, 58, 168, 74
    base = h - 58
    scale = (base - T) / 1.30          # headroom above 1.19
    bars = [("1.19", 1.19, BLUE, "invoiced cost only"),
            ("1.05", 1.05, CRIMSON, "accrual booked")]
    s = [f"<svg viewBox='0 0 {w} {h}' "
         f"xmlns='http://www.w3.org/2000/svg' role='img'>"]
    s.append(f"<line x1='{L - 14}' y1='{base}' x2='{w - 30}' y2='{base}' "
             f"stroke='{GRID}' stroke-width='1.5'/>")
    for i, (lab, v, col, note) in enumerate(bars):
        x = L + i * (BW + GAP)
        bh = v * scale
        # 4px rounded data-end, anchored to the baseline.
        s.append(f"<path d='M{x} {base} L{x} {base - bh + 4} Q{x} {base - bh} {x + 4} {base - bh} "
                 f"L{x + BW - 4} {base - bh} Q{x + BW} {base - bh} {x + BW} {base - bh + 4} "
                 f"L{x + BW} {base} Z' fill='{col}'/>")
        s.append(_txt(x + BW / 2, base - bh - 16, lab, 40, INK, 800, "middle"))
        s.append(_txt(x + BW / 2, base + 26, note, 14, MIST, 600, "middle"))
    # The gap is the story, so it is drawn and named.
    gx = L + 2 * BW + GAP + 46
    y1, y2 = base - 1.19 * scale, base - 1.05 * scale
    s.append(f"<line x1='{gx}' y1='{y1:.1f}' x2='{gx}' y2='{y2:.1f}' stroke='{INK}' "
             f"stroke-width='2'/>")
    for y in (y1, y2):
        s.append(f"<line x1='{gx - 7}' y1='{y:.1f}' x2='{gx + 7}' y2='{y:.1f}' stroke='{INK}' "
                 f"stroke-width='2'/>")
    s.append(_txt(gx + 16, (y1 + y2) / 2 - 2, "0.14", 26, INK, 800))
    s.append(_txt(gx + 16, (y1 + y2) / 2 + 20, "one missing entry", 14, MIST, 600))
    s.append("</svg>")
    return "".join(s)


def weight_bar(w=880, h=132):
    """The 40/40/20 split as one bar. Three numbers in a row are three facts; a bar is a shape."""
    parts = [(40, "Finance, accounting\nand reporting", BLUE),
             (40, "Project management", "#7E97D9"),
             (20, "Governed AI", CRIMSON)]
    s = [f"<svg viewBox='0 0 {w} {h}' "
         f"xmlns='http://www.w3.org/2000/svg' role='img'>"]
    x = 0
    for pct, label, col in parts:
        bw = (pct / 100) * w - 4        # 2px surface gap on each side, per the mark spec
        s.append(f"<rect x='{x:.1f}' y='0' width='{bw:.1f}' height='34' rx='3' fill='{col}'/>")
        s.append(_txt(x, 72, f"{pct}%", 30, INK, 800))
        for j, line in enumerate(label.split("\n")):
            s.append(_txt(x, 98 + j * 19, line, 14.5, MIST, 600))
        x += (pct / 100) * w
    s.append("</svg>")
    return "".join(s)
