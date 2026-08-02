#!/usr/bin/env python3
"""Render each volume's full-bleed A4 cover as PCI-original artwork.

The conformance matrix's first row expects a cover in the construction of the approved previous PCI
book. Neither volume had one; this generates both.

**The motif is drawn from the volume's own content**, not chosen for decoration, because a cover that
could belong to any book is a wasted page:

  * **PML-AI** — the interface motif of Domain 4. Left: eight nodes wired to each other, the
    ``n(n-1)/2`` mesh that grows quadratically. Right: the same eight nodes through a spine, the
    ``n`` layered architecture. Twenty-eight edges against eight, drawn to scale, which is the
    domain's arithmetic made visible before the reader opens the book.
  * **PFL-AI** — the coverage motif of Domains 10 and 15. A cash bar per year with the debt-service
    line across it, the gap above the line being the coverage the covenant measures, and the point
    where the bar meets the line being where the project stops paying.

Deterministic, no external assets, brand palette only. Both covers state that this is a draft not for
release, because a cover that implies a released edition would be the most visible false claim in the
volume.

Usage:  python3 make_cover.py
"""
import math
import pathlib

BLUE, CRIMSON, INK, SLATE = "#1D4ED8", "#C13329", "#0F172A", "#64748B"
PAPER = "#F8FAFC"
FONT = 'font-family="Inter, Helvetica, Arial, sans-serif"'
ROOT = pathlib.Path(__file__).resolve().parent.parent
W, H = 2100, 2970            # A4 at 10 units/mm

BOOKS = {
    "pml-ai": {
        "mark": "PML-AI",
        "title": "Body of Knowledge",
        "designation": "PCI Project Management Leader &#8211; AI",
        "strap": "Leadership &#183; delivery systems &#183; governance &#183; the governed use of AI",
        "motif": "interfaces",
    },
    "pfl-ai": {
        "mark": "PFL-AI",
        "title": "Body of Knowledge",
        "designation": "PCI Project Finance Leader &#8211; AI",
        "strap": "Structuring &#183; modelling &#183; coverage &#183; the governed use of AI",
        "motif": "coverage",
    },
}


def interfaces() -> str:
    """Domain 4's mesh against its layered alternative: 28 edges against 8, to scale."""
    out, n, r = [], 8, 150.0
    for cx, layered in ((560.0, False), (1540.0, True)):
        cy = 1500.0
        pts = [(cx + r * math.cos(2 * math.pi * k / n - math.pi / 2),
                cy + r * math.sin(2 * math.pi * k / n - math.pi / 2)) for k in range(n)]
        if layered:
            # every node to a single spine: n edges
            out.append(f'<circle cx="{cx:.1f}" cy="{cy:.1f}" r="26" fill="{CRIMSON}"/>')
            for x, y in pts:
                out.append(f'<line x1="{cx:.1f}" y1="{cy:.1f}" x2="{x:.1f}" y2="{y:.1f}" '
                           f'stroke="{BLUE}" stroke-width="3.4" opacity="0.85"/>')
        else:
            for i in range(n):
                for j in range(i + 1, n):
                    out.append(f'<line x1="{pts[i][0]:.1f}" y1="{pts[i][1]:.1f}" '
                               f'x2="{pts[j][0]:.1f}" y2="{pts[j][1]:.1f}" '
                               f'stroke="{BLUE}" stroke-width="2.2" opacity="0.5"/>')
        for x, y in pts:
            out.append(f'<circle cx="{x:.1f}" cy="{y:.1f}" r="15" fill="{PAPER}"/>'
                       f'<circle cx="{x:.1f}" cy="{y:.1f}" r="15" fill="none" stroke="{PAPER}" '
                       f'stroke-width="3"/>')
        lab = "n = 8" if layered else "n(n&#8722;1)/2 = 28"
        out.append(f'<text x="{cx:.1f}" y="{cy + 250:.1f}" font-size="40" fill="{PAPER}" '
                   f'text-anchor="middle" opacity="0.85">{lab}</text>')
    return "".join(out)


def coverage() -> str:
    """A coverage profile: cash bars, the service line, and the year the bar meets it."""
    out = []
    x0, y0, bw, gap = 380.0, 1660.0, 92.0, 26.0
    cash = [1.00, 0.98, 0.95, 0.93, 0.90, 0.86, 0.83, 0.79, 0.74, 0.70, 0.66, 0.61]
    scale, svc = 430.0, 0.62
    for i, c in enumerate(cash):
        x = x0 + i * (bw + gap)
        h = c * scale
        below = c <= svc + 0.02
        out.append(f'<rect x="{x:.1f}" y="{y0 - h:.1f}" width="{bw:.1f}" height="{h:.1f}" '
                   f'fill="{CRIMSON if below else BLUE}" opacity="{0.95 if below else 0.8}"/>')
    xe = x0 + len(cash) * (bw + gap) - gap
    ys = y0 - svc * scale
    out.append(f'<line x1="{x0 - 40:.1f}" y1="{ys:.1f}" x2="{xe + 40:.1f}" y2="{ys:.1f}" '
               f'stroke="{PAPER}" stroke-width="5" stroke-dasharray="18 12"/>')
    out.append(f'<text x="{xe + 30:.1f}" y="{ys - 26:.1f}" font-size="38" fill="{PAPER}" '
               f'text-anchor="end" opacity="0.9">debt service</text>')
    out.append(f'<text x="{x0 - 40:.1f}" y="{y0 + 62:.1f}" font-size="38" fill="{PAPER}" '
               f'opacity="0.85">cash available &#183; the gap is the coverage</text>')
    return "".join(out)


def cover(cfg: dict) -> str:
    motif = interfaces() if cfg["motif"] == "interfaces" else coverage()
    return (
        f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {W} {H}" {FONT}>'
        f'<rect width="{W}" height="{H}" fill="{INK}"/>'
        # a single blue field behind the motif, so the artwork reads as structure not ornament
        f'<rect x="0" y="1120" width="{W}" height="900" fill="{BLUE}" opacity="0.10"/>'
        f'<rect x="0" y="1120" width="{W}" height="6" fill="{BLUE}" opacity="0.55"/>'
        f'<rect x="0" y="2014" width="{W}" height="6" fill="{BLUE}" opacity="0.55"/>'
        + motif +
        # institute line
        f'<text x="170" y="300" font-size="40" fill="{SLATE}" letter-spacing="10">'
        f'PROJECT CONTROLS INSTITUTE GLOBAL</text>'
        # designation mark
        f'<text x="170" y="640" font-size="238" fill="{PAPER}" font-weight="700" '
        f'letter-spacing="-4">{cfg["mark"]}</text>'
        f'<text x="170" y="820" font-size="112" fill="{BLUE}" font-weight="600">'
        f'{cfg["title"]}</text>'
        f'<rect x="170" y="900" width="240" height="9" fill="{CRIMSON}"/>'
        f'<text x="170" y="1000" font-size="52" fill="{PAPER}" opacity="0.92">'
        f'The reference for the {cfg["designation"]}</text>'
        f'<text x="170" y="1070" font-size="40" fill="{SLATE}">{cfg["strap"]}</text>'
        # the principle
        f'<text x="170" y="2200" font-size="46" fill="{PAPER}" opacity="0.9" '
        f'font-style="italic">AI proposes; the professional verifies,</text>'
        f'<text x="170" y="2264" font-size="46" fill="{PAPER}" opacity="0.9" '
        f'font-style="italic">decides and remains accountable.</text>'
        # draft band — a cover must not imply a released edition
        f'<rect x="170" y="2420" width="1190" height="130" fill="{CRIMSON}"/>'
        f'<text x="210" y="2478" font-size="44" fill="{PAPER}" font-weight="700" '
        f'letter-spacing="3">FIRST EDITION &#8212; DRAFT</text>'
        f'<text x="210" y="2528" font-size="34" fill="{PAPER}" opacity="0.92">'
        f'For editorial and technical review &#183; not for release or distribution</text>'
        f'<text x="170" y="2760" font-size="36" fill="{SLATE}">'
        f'Finance intelligently. Control predictively. Deliver successfully.</text>'
        f'</svg>')


def main() -> int:
    for book, cfg in BOOKS.items():
        out = ROOT / book / "build" / "cover.svg"
        out.parent.mkdir(parents=True, exist_ok=True)
        out.write_text(cover(cfg), encoding="utf-8")
        print(f"{book}: cover -> {out.relative_to(ROOT.parent)}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
