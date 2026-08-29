#!/usr/bin/env python3
"""Inline SVG figures for the course-outline deck.

**Colour.** Two hues only, both from the logo: a lightened brand navy and the crimson of the accent
bar. Validated with the dataviz skill's checker against the light surface — lightness band, chroma
floor, CVD separation (protan ΔE 26.1, tritan 31.4), normal-vision floor and contrast all pass. Do
not add a third hue without re-running it; dark gold against this crimson gives a deuteranope a ΔE
of 0.6, which is no separation at all.

An EAC-fan figure and a CPI-comparison figure lived here too. Both were cut with the slides that
carried them; `git log` has them if they are ever wanted back.
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
