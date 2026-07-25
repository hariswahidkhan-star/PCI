#!/usr/bin/env python3
"""Render the prototype domains' figures as SVG masters (PCI original artwork).

Style per the pattern spec: brand blue #1D4ED8, crimson #C13329, ink #0F172A, slate greys,
Inter labels, clean axes, no decoration. Deterministic output — safe to re-run.
"""
import pathlib

BLUE, CRIMSON, INK, SLATE, GRID = "#1D4ED8", "#C13329", "#0F172A", "#64748B", "#E2E8F0"
FONT = 'font-family="Inter, Helvetica, Arial, sans-serif"'
ROOT = pathlib.Path(__file__).resolve().parent.parent
PFL = ROOT / "pfl-ai" / "build" / "figures"
PML = ROOT / "pml-ai" / "build" / "figures"
for d in (PFL, PML):
    d.mkdir(parents=True, exist_ok=True)


def svg(w, h, body):
    return (f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {w} {h}" {FONT}>'
            f'<rect width="{w}" height="{h}" fill="white"/>{body}</svg>')


def axes(x0, y0, x1, y1, xlab, ylab):
    return (f'<line x1="{x0}" y1="{y1}" x2="{x1}" y2="{y1}" stroke="{INK}" stroke-width="1.2"/>'
            f'<line x1="{x0}" y1="{y0}" x2="{x0}" y2="{y1}" stroke="{INK}" stroke-width="1.2"/>'
            f'<text x="{(x0+x1)/2}" y="{y1+34}" font-size="12" fill="{SLATE}" text-anchor="middle">{xlab}</text>'
            f'<text x="{x0-52}" y="{(y0+y1)/2}" font-size="12" fill="{SLATE}" text-anchor="middle" transform="rotate(-90 {x0-52} {(y0+y1)/2})">{ylab}</text>')


# ---- Fig 3.1.1 simple vs compound -------------------------------------------------
W, H, L, R, T, B = 640, 400, 80, 24, 24, 56
def X(t): return L + t / 10 * (W - L - R)
def Y(v): return H - B - (v - 100000) / 120000 * (H - T - B)
grid = "".join(f'<line x1="{L}" y1="{Y(v)}" x2="{W-R}" y2="{Y(v)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Y(v)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v//1000}k</text>'
               for v in range(100000, 230000, 20000))
simple = " ".join(f"{X(t)},{Y(100000*(1+0.08*t))}" for t in range(11))
comp = " ".join(f"{X(t)},{Y(100000*1.08**t)}" for t in range(11))
xticks = "".join(f'<text x="{X(t)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{t}</text>' for t in range(0, 11, 2))
body = (grid + axes(L, T, W - R, H - B, "Years", "Value (USD)") + xticks
        + f'<polyline points="{simple}" fill="none" stroke="{SLATE}" stroke-width="2.2" stroke-dasharray="6 4"/>'
        + f'<polyline points="{comp}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
        + f'<circle cx="{X(3)}" cy="{Y(125971)}" r="3.5" fill="{CRIMSON}"/>'
        + f'<text x="{X(3)+8}" y="{Y(125971)-8}" font-size="10.5" fill="{INK}">yr 3: 125,971 vs 124,000</text>'
        + f'<text x="{X(8.5)}" y="{Y(100000*1.08**8.6)-14}" font-size="11.5" fill="{BLUE}" font-weight="600">Compound 8%</text>'
        + f'<text x="{X(8.5)}" y="{Y(100000*(1+0.08*8))+22}" font-size="11.5" fill="{SLATE}" font-weight="600">Simple 8%</text>')
(PFL / "fig_3_1_1.svg").write_text(svg(W, H, body))

# ---- Fig 3.2.1 amortisation stacked bars ------------------------------------------
pay = 5009635.23
rows, bal = [], 42000000.0
for _ in range(12):
    i = bal * 0.06; p = pay - i; bal -= p; rows.append((i, p))
W, H, L, R, T, B = 640, 400, 80, 24, 24, 56
bw = (W - L - R) / 12 * 0.62
def Yv(v): return H - B - v / 5500000 * (H - T - B)
bars = ""
for k, (i, p) in enumerate(rows):
    x = L + (k + 0.19) * (W - L - R) / 12
    bars += (f'<rect x="{x:.1f}" y="{Yv(i):.1f}" width="{bw:.1f}" height="{(H-B)-Yv(i):.1f}" fill="{CRIMSON}" opacity="0.85"/>'
             f'<rect x="{x:.1f}" y="{Yv(i+p):.1f}" width="{bw:.1f}" height="{Yv(i)-Yv(i+p):.1f}" fill="{BLUE}"/>'
             f'<text x="{x+bw/2:.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{k+1}</text>')
grid = "".join(f'<line x1="{L}" y1="{Yv(v)}" x2="{W-R}" y2="{Yv(v)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Yv(v)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v/1000000:.0f}m</text>'
               for v in range(0, 6000000, 1000000))
body = (grid + bars + axes(L, T, W - R, H - B, "Year", "Debt service (USD)")
        + f'<line x1="{L}" y1="{Yv(pay)}" x2="{W-R}" y2="{Yv(pay)}" stroke="{INK}" stroke-width="1" stroke-dasharray="4 4"/>'
        + f'<text x="{W-R-4}" y="{Yv(pay)-6}" font-size="10.5" fill="{INK}" text-anchor="end">level instalment 5,009,635</text>'
        + f'<rect x="{L+10}" y="{T+6}" width="11" height="11" fill="{CRIMSON}" opacity="0.85"/><text x="{L+26}" y="{T+15}" font-size="11" fill="{INK}">Interest</text>'
        + f'<rect x="{L+96}" y="{T+6}" width="11" height="11" fill="{BLUE}"/><text x="{L+112}" y="{T+15}" font-size="11" fill="{INK}">Principal</text>')
(PFL / "fig_3_2_1.svg").write_text(svg(W, H, body))

# ---- Fig 3.3.1 purchasing power ---------------------------------------------------
W, H, L, R, T, B = 640, 400, 80, 24, 24, 56
def Xt(t): return L + t / 20 * (W - L - R)
def Yp(v): return H - B - (v - 500000) / 550000 * (H - T - B)
curve = " ".join(f"{Xt(t)},{Yp(1000000/1.03**t)}" for t in range(21))
grid = "".join(f'<line x1="{L}" y1="{Yp(v)}" x2="{W-R}" y2="{Yp(v)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Yp(v)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v//1000}k</text>'
               for v in range(500000, 1050001, 100000))
xt = "".join(f'<text x="{Xt(t)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{t}</text>' for t in range(0, 21, 5))
body = (grid + axes(L, T, W - R, H - B, "Years at 3% inflation", "Real purchasing power (USD)") + xt
        + f'<line x1="{L}" y1="{Yp(1000000)}" x2="{W-R}" y2="{Yp(1000000)}" stroke="{SLATE}" stroke-width="1.2" stroke-dasharray="5 4"/>'
        + f'<text x="{W-R-4}" y="{Yp(1000000)-6}" font-size="10.5" fill="{SLATE}" text-anchor="end">nominal USD 1,000,000</text>'
        + f'<polyline points="{curve}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
        + "".join(f'<circle cx="{Xt(t)}" cy="{Yp(1000000/1.03**t)}" r="3.2" fill="{CRIMSON}"/>'
                  f'<text x="{Xt(t)}" y="{Yp(1000000/1.03**t)+18}" font-size="10" fill="{INK}" text-anchor="middle">{round(1000000/1.03**t/1000)}k</text>'
                  for t in (5, 10, 20)))
(PFL / "fig_3_3_1.svg").write_text(svg(W, H, body))

# ---- Fig 6.2.1 Auriga network -----------------------------------------------------
NODES = {"A": (60, 170, 2, 0, 2, 0, 2, 0), "B": (185, 170, 6, 2, 8, 2, 8, 0),
         "C": (310, 90, 8, 8, 16, 8, 16, 0), "D": (310, 250, 7, 8, 15, 9, 16, 1),
         "E": (435, 170, 5, 16, 21, 16, 21, 0), "F": (560, 170, 4, 21, 25, 21, 25, 0),
         "G": (435, 320, 2, 15, 17, 23, 25, 8)}
EDGES = [("A", "B", 1), ("B", "C", 1), ("B", "D", 0), ("C", "E", 1), ("D", "E", 0), ("E", "F", 1), ("D", "G", 0)]
NW, NH = 108, 74
body = '<defs><marker id="ar" viewBox="0 0 10 10" refX="9" refY="5" markerWidth="7" markerHeight="7" orient="auto"><path d="M0 0L10 5L0 10z" fill="context-stroke"/></marker></defs>'
for a, b, crit in EDGES:
    ax, ay = NODES[a][0] + NW, NODES[a][1] + NH / 2
    bx, by = NODES[b][0], NODES[b][1] + NH / 2
    col, wd = (BLUE, 2.6) if crit else (SLATE, 1.4)
    body += f'<line x1="{ax}" y1="{ay}" x2="{bx-4}" y2="{by}" stroke="{col}" stroke-width="{wd}" marker-end="url(#ar)"/>'
for k, (x, y, d, es, ef, ls, lf, tf) in NODES.items():
    crit = tf == 0
    body += (f'<rect x="{x}" y="{y}" width="{NW}" height="{NH}" rx="7" fill="white" '
             f'stroke="{BLUE if crit else SLATE}" stroke-width="{2.4 if crit else 1.3}"/>'
             f'<text x="{x+10}" y="{y+19}" font-size="14" font-weight="700" fill="{INK}">{k}</text>'
             f'<text x="{x+NW-10}" y="{y+19}" font-size="11" fill="{CRIMSON}" text-anchor="end">{d} wk</text>'
             f'<text x="{x+10}" y="{y+37}" font-size="10.5" fill="{INK}">ES {es} · EF {ef}</text>'
             f'<text x="{x+10}" y="{y+52}" font-size="10.5" fill="{SLATE}">LS {ls} · LF {lf}</text>'
             f'<text x="{x+10}" y="{y+66}" font-size="10.5" font-weight="600" fill="{BLUE if crit else CRIMSON}">TF {tf}</text>')
body += (f'<text x="60" y="52" font-size="13" font-weight="700" fill="{INK}">Critical path A–B–C–E–F = 25 weeks</text>'
         f'<line x1="300" y1="47" x2="330" y2="47" stroke="{BLUE}" stroke-width="2.6"/>'
         f'<text x="336" y="52" font-size="11" fill="{SLATE}">zero-float path</text>')
(PML / "fig_6_2_1.svg").write_text(svg(700, 420, body))

# ---- Fig 6.3.1 crew histogram -----------------------------------------------------
before = {8: 3, 9: 5, 10: 5, 11: 5, 12: 5, 13: 5, 14: 5, 15: 5, 16: 2, 17: 0}
after = {8: 3, 9: 4, 10: 4, 11: 4, 12: 4, 13: 4, 14: 4, 15: 4, 16: 3, 17: 2}
W, H, L, R, T, B = 640, 380, 70, 24, 30, 56
def Xw(i): return L + i * (W - L - R) / 10
def Yc(c): return H - B - c / 6 * (H - T - B)
bars = ""
for i, wk in enumerate(range(8, 18)):
    bw = (W - L - R) / 10 * 0.36
    bars += (f'<rect x="{Xw(i)+6:.1f}" y="{Yc(before[wk]):.1f}" width="{bw:.1f}" height="{(H-B)-Yc(before[wk]):.1f}" fill="{SLATE}" opacity="0.55"/>'
             f'<rect x="{Xw(i)+10+bw:.1f}" y="{Yc(after[wk]):.1f}" width="{bw:.1f}" height="{(H-B)-Yc(after[wk]):.1f}" fill="{BLUE}"/>'
             f'<text x="{Xw(i)+8+bw:.1f}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{wk}</text>')
grid = "".join(f'<line x1="{L}" y1="{Yc(c)}" x2="{W-R}" y2="{Yc(c)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Yc(c)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{c}</text>' for c in range(0, 7))
body = (grid + bars + axes(L, T, W - R, H - B, "Week", "Field crews")
        + f'<line x1="{L}" y1="{Yc(4)}" x2="{W-R}" y2="{Yc(4)}" stroke="{CRIMSON}" stroke-width="2" stroke-dasharray="6 4"/>'
        + f'<text x="{W-R-4}" y="{Yc(4)-8}" font-size="11" font-weight="600" fill="{CRIMSON}" text-anchor="end">site cap: 4 crews</text>'
        + f'<rect x="{L+10}" y="{T-2}" width="11" height="11" fill="{SLATE}" opacity="0.55"/><text x="{L+26}" y="{T+7}" font-size="11" fill="{INK}">Before</text>'
        + f'<rect x="{L+86}" y="{T-2}" width="11" height="11" fill="{BLUE}"/><text x="{L+102}" y="{T+7}" font-size="11" fill="{INK}">After smoothing</text>')
(PML / "fig_6_3_1.svg").write_text(svg(W, H, body))

# ---- Fig 3.1.2 discount-factor curves ---------------------------------------------
W, H, L, R, T, B = 640, 400, 76, 60, 24, 56
def Xy(t): return L + t / 25 * (W - L - R)
def Yd(v): return H - B - v * (H - T - B)
grid = "".join(f'<line x1="{L}" y1="{Yd(v/10)}" x2="{W-R}" y2="{Yd(v/10)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Yd(v/10)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v/10:.1f}</text>'
               for v in range(0, 11, 2))
xt = "".join(f'<text x="{Xy(t)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{t}</text>' for t in range(0, 26, 5))
curves = ""
for r, col in ((0.06, BLUE), (0.10, INK), (0.14, CRIMSON)):
    pts = " ".join(f"{Xy(t)},{Yd(1/(1+r)**t)}" for t in range(26))
    curves += (f'<polyline points="{pts}" fill="none" stroke="{col}" stroke-width="2.4"/>'
               f'<text x="{W-R+6}" y="{Yd(1/(1+r)**25)+4}" font-size="11" font-weight="600" fill="{col}">{int(r*100)}%</text>')
mark = (f'<line x1="{Xy(10)}" y1="{Yd(0)}" x2="{Xy(10)}" y2="{Yd(1/1.06**10)}" stroke="{SLATE}" stroke-dasharray="4 4"/>'
        + "".join(f'<circle cx="{Xy(10)}" cy="{Yd(1/(1+r)**10)}" r="3" fill="{c}"/>'
                  for r, c in ((0.06, BLUE), (0.10, INK), (0.14, CRIMSON))))
body = grid + curves + mark + axes(L, T, W - R, H - B, "Years", "Discount factor DF(t)") + xt
(PFL / "fig_3_1_2.svg").write_text(svg(W, H, body))

# ---- Fig 3.2.2 three loan shapes --------------------------------------------------
pay = 5009635.23
W, H, L, R, T, B = 640, 420, 84, 24, 30, 56
def Xr(y): return L + (y - 0.5) / 12 * (W - L - R)
def Ys(v): return H - B - v / 46000000 * (H - T - B)
lp = [3500000 + (42000000 - 3500000 * k) * 0.06 for k in range(12)]
bullet = [2520000] * 11 + [44520000]
grid = "".join(f'<line x1="{L}" y1="{Ys(v)}" x2="{W-R}" y2="{Ys(v)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Ys(v)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v//1000000}m</text>'
               for v in range(0, 46000001, 10000000))
series = ""
for vals, col, w in ((bullet, CRIMSON, 2.0), (lp, INK, 2.2), ([pay] * 12, BLUE, 2.6)):
    pts = " ".join(f"{Xr(y+1)},{Ys(v)}" for y, v in enumerate(vals))
    series += f'<polyline points="{pts}" fill="none" stroke="{col}" stroke-width="{w}"/>'
series += "".join(f'<circle cx="{Xr(12)}" cy="{Ys(v)}" r="3.2" fill="{c}"/>'
                  for v, c in ((44520000, CRIMSON), (3710000, INK), (pay, BLUE)))
xt = "".join(f'<text x="{Xr(y)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{y}</text>' for y in range(1, 13))
leg = (f'<text x="{L+10}" y="{T+10}" font-size="11" fill="{BLUE}" font-weight="600">Annuity — interest 18.12m</text>'
       f'<text x="{L+10}" y="{T+26}" font-size="11" fill="{INK}" font-weight="600">Level-principal — interest 16.38m</text>'
       f'<text x="{L+10}" y="{T+42}" font-size="11" fill="{CRIMSON}" font-weight="600">Bullet — interest 30.24m, balloon yr 12</text>')
body = grid + series + leg + axes(L, T, W - R, H - B, "Year", "Debt service (USD)") + xt
(PFL / "fig_3_2_2.svg").write_text(svg(W, H, body))

# ---- Fig 3.3.2 compound vs simple escalation --------------------------------------
W, H, L, R, T, B = 640, 400, 84, 24, 24, 56
def Xe(t): return L + t / 25 * (W - L - R)
def Ye(v): return H - B - (v - 10000000) / 17000000 * (H - T - B)
comp_pts = [(t, 10000000 * 1.04 ** t) for t in range(26)]
simp_pts = [(t, 10000000 * (1 + 0.04 * t)) for t in range(26)]
shade = ("M" + " L".join(f"{Xe(t)},{Ye(v)}" for t, v in comp_pts)
         + " L" + " L".join(f"{Xe(t)},{Ye(v)}" for t, v in reversed(simp_pts)) + " Z")
grid = "".join(f'<line x1="{L}" y1="{Ye(v)}" x2="{W-R}" y2="{Ye(v)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Ye(v)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v//1000000}m</text>'
               for v in range(10000000, 27000001, 4000000))
xt = "".join(f'<text x="{Xe(t)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{t}</text>' for t in range(0, 26, 5))
body = (grid + f'<path d="{shade}" fill="{BLUE}" opacity="0.10"/>'
        + f'<polyline points="{" ".join(f"{Xe(t)},{Ye(v)}" for t, v in comp_pts)}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
        + f'<polyline points="{" ".join(f"{Xe(t)},{Ye(v)}" for t, v in simp_pts)}" fill="none" stroke="{SLATE}" stroke-width="2.2" stroke-dasharray="6 4"/>'
        + f'<text x="{Xe(20)}" y="{Ye(10000000*1.04**21)-10}" font-size="11.5" fill="{BLUE}" font-weight="600">Compound 4%</text>'
        + f'<text x="{Xe(20)}" y="{Ye(10000000*(1+0.04*18))+22}" font-size="11.5" fill="{SLATE}" font-weight="600">Simple 4%</text>'
        + f'<text x="{Xe(24.6)}" y="{Ye(23300000)}" font-size="10.5" fill="{CRIMSON}" font-weight="600" text-anchor="end">+6.66m by yr 25</text>'
        + axes(L, T, W - R, H - B, "Years", "Escalated cost (USD)") + xt)
(PFL / "fig_3_3_2.svg").write_text(svg(W, H, body))

# ---- Fig 6.3.2 rolling-wave horizon -----------------------------------------------
W, H, L, R, T, B = 700, 320, 30, 20, 46, 40
def Xm(mo): return L + mo / 24 * (W - L - R)
bars = ""
for i in range(14):  # L4 dense small bars, months 0-3
    x = Xm(i * 0.21 + 0.05); yy = 70 + (i % 4) * 28
    bars += f'<rect x="{x:.1f}" y="{yy}" width="{Xm(0.18)-Xm(0):.1f}" height="16" rx="2" fill="{BLUE}"/>'
for i in range(6):  # L3 medium bars, months 3-9
    x = Xm(3.2 + i * 0.95); yy = 76 + (i % 3) * 34
    bars += f'<rect x="{x:.1f}" y="{yy}" width="{Xm(0.8)-Xm(0):.1f}" height="18" rx="2" fill="{INK}" opacity="0.75"/>'
for i, (mo, span, lab) in enumerate([(9.5, 4.4, "12–16 wk"), (14.4, 4.6, "14–18 wk"), (19.4, 4.2, "13–17 wk")]):
    yy = 84 + i * 42
    bars += (f'<rect x="{Xm(mo):.1f}" y="{yy}" width="{Xm(span)-Xm(0):.1f}" height="24" rx="4" fill="white" '
             f'stroke="{SLATE}" stroke-width="1.6" stroke-dasharray="7 4"/>'
             f'<text x="{Xm(mo+span/2):.1f}" y="{yy+16}" font-size="10.5" fill="{SLATE}" text-anchor="middle">package · {lab}</text>')
zones = (f'<text x="{Xm(1.5)}" y="{T+12}" font-size="11" font-weight="700" fill="{BLUE}" text-anchor="middle">EXECUTION DETAIL (L4)</text>'
         f'<text x="{Xm(6)}" y="{T+12}" font-size="11" font-weight="700" fill="{INK}" text-anchor="middle">CONTROL DETAIL (L3)</text>'
         f'<text x="{Xm(16.5)}" y="{T+12}" font-size="11" font-weight="700" fill="{SLATE}" text-anchor="middle">PLANNING PACKAGES (ranged)</text>'
         f'<line x1="{Xm(3)}" y1="{T+20}" x2="{Xm(3)}" y2="{H-B}" stroke="{CRIMSON}" stroke-width="2" stroke-dasharray="5 4"/>'
         f'<text x="{Xm(3)+6}" y="{H-B-10}" font-size="10.5" fill="{CRIMSON}" font-weight="600">elaboration point — wave advances →</text>')
axis = (f'<line x1="{L}" y1="{H-B}" x2="{W-R}" y2="{H-B}" stroke="{INK}" stroke-width="1.2"/>'
        + "".join(f'<text x="{Xm(mo)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{mo}</text>'
                  for mo in range(0, 25, 6))
        + f'<text x="{(L+W-R)/2}" y="{H-B+32}" font-size="12" fill="{SLATE}" text-anchor="middle">Months</text>')
(PML / "fig_6_3_2.svg").write_text(svg(W, H, bars + zones + axis))

# ---- Fig 6.1.1 schedule hierarchy -------------------------------------------------
W, H = 700, 360
bands = [("L1 — EXECUTIVE SUMMARY", "10–30 bars · board & sponsor", SLATE, 0.35),
         ("L2 — MANAGEMENT SCHEDULE", "50–300 activities · steering & PMO", SLATE, 0.55),
         ("L3 — CONTROL SCHEDULE", "300–5,000 activities · logic · float · critical path live here", BLUE, 1.0),
         ("L4 — EXECUTION / LOOKAHEAD", "daily & shift detail · site and squads", SLATE, 0.45)]
body = ""
for i, (t, sub, col, emph) in enumerate(bands):
    y = 34 + i * 76
    wgt = 700 if emph == 1.0 else 620
    x = (W - wgt) / 2
    body += (f'<rect x="{x+40}" y="{y}" width="{wgt-80}" height="58" rx="8" fill="white" '
             f'stroke="{col}" stroke-width="{3 if emph==1.0 else 1.4}"/>'
             f'<text x="{W/2}" y="{y+24}" font-size="13.5" font-weight="700" fill="{col if emph==1.0 else INK}" text-anchor="middle">{t}</text>'
             f'<text x="{W/2}" y="{y+43}" font-size="10.5" fill="{SLATE}" text-anchor="middle">{sub}</text>')
    if i < 3:
        body += (f'<line x1="{W/2-90}" y1="{y+58}" x2="{W/2-90}" y2="{y+76}" stroke="{SLATE}" stroke-width="1.4" marker-end="url(#ar2)"/>'
                 f'<line x1="{W/2+90}" y1="{y+76}" x2="{W/2+90}" y2="{y+58}" stroke="{SLATE}" stroke-width="1.4" marker-end="url(#ar2)"/>')
body = ('<defs><marker id="ar2" viewBox="0 0 10 10" refX="9" refY="5" markerWidth="6" markerHeight="6" orient="auto">'
        f'<path d="M0 0L10 5L0 10z" fill="{SLATE}"/></marker></defs>' + body
        + f'<text x="{W/2-96}" y="{34+58+50}" font-size="9.5" fill="{SLATE}" text-anchor="end">summarised from ↑</text>'
        + f'<text x="{W/2+96}" y="{34+58+50}" font-size="9.5" fill="{SLATE}">↓ traceable to</text>'
        + f'<text x="{W-24}" y="{34+2*76+30}" font-size="10" font-weight="600" fill="{CRIMSON}" text-anchor="end">under change control</text>')
(PML / "fig_6_1_1.svg").write_text(svg(W, H, body))

# ---- Fig 6.4.1 crash economics ----------------------------------------------------
W, H, L, R, T, B = 640, 380, 84, 24, 30, 56
def Xc(wk): return L + wk / 2.4 * (W - L - R)
def Yc2(v): return H - B - v / 70000 * (H - T - B)
grid = "".join(f'<line x1="{L}" y1="{Yc2(v)}" x2="{W-R}" y2="{Yc2(v)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Yc2(v)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v//1000}k</text>'
               for v in range(0, 70001, 15000))
def steps(pts, col, w):
    d = f"M{Xc(pts[0][0])},{Yc2(pts[0][1])}"
    for (x0, y0), (x1, y1) in zip(pts, pts[1:]):
        d += f" L{Xc(x1)},{Yc2(y0)} L{Xc(x1)},{Yc2(y1)}"
    return f'<path d="{d}" fill="none" stroke="{col}" stroke-width="{w}"/>'
body = (grid + steps([(0, 0), (1, 30000), (2, 60000)], SLATE, 2.2)
        + steps([(0, 0), (1, 45000), (2, 45000)], BLUE, 2.6)
        + f'<circle cx="{Xc(1)}" cy="{Yc2(45000)}" r="4" fill="{CRIMSON}"/>'
        + f'<text x="{Xc(1)+8}" y="{Yc2(45000)-10}" font-size="10.5" fill="{CRIMSON}" font-weight="600">path migration — second week buys nothing</text>'
        + f'<text x="{Xc(1.62)}" y="{Yc2(60000)+2}" font-size="11" fill="{SLATE}" font-weight="600">cumulative crash cost</text>'
        + f'<text x="{Xc(1.55)}" y="{Yc2(42000)+16}" font-size="11" fill="{BLUE}" font-weight="600">value of weeks saved</text>'
        + f'<text x="{Xc(1)}" y="{H-B+34}" font-size="10.5" fill="{INK}" text-anchor="middle">net +15,000 at 1 wk · net −15,000 at 2 wk</text>'
        + axes(L, T, W - R, H - B, "Weeks of crash bought on C", "USD")
        + "".join(f'<text x="{Xc(wk)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{wk}</text>' for wk in (0, 1, 2)))
(PML / "fig_6_4_1.svg").write_text(svg(W, H, body))

# ---- Fig 4.1.1 NPV profile --------------------------------------------------------
def af_f(r, n):
    return (1 - (1 + r) ** -n) / r if r else float(n)
W, H, L, R, T, B = 640, 400, 84, 24, 24, 56
def Xr4(r): return L + r / 0.20 * (W - L - R)
def Yn4(v): return H - B - (v + 20e6) / 95e6 * (H - T - B)
pts = " ".join(f"{Xr4(r/1000)},{Yn4(8.9e6*af_f(r/1000,15)-60e6)}" for r in range(0, 201, 5))
grid = "".join(f'<line x1="{L}" y1="{Yn4(v*1e6)}" x2="{W-R}" y2="{Yn4(v*1e6)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Yn4(v*1e6)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v}m</text>'
               for v in range(-20, 76, 20))
xt = "".join(f'<text x="{Xr4(r/100)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{r}%</text>' for r in range(0, 21, 5))
irr_x, hz_x = Xr4(0.1219), Xr4(0.08)
body = (grid + f'<line x1="{L}" y1="{Yn4(0)}" x2="{W-R}" y2="{Yn4(0)}" stroke="{INK}" stroke-width="1"/>'
        + f'<polyline points="{pts}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
        + f'<line x1="{hz_x}" y1="{Yn4(-20e6)}" x2="{hz_x}" y2="{Yn4(16.18e6)}" stroke="{SLATE}" stroke-dasharray="5 4"/>'
        + f'<circle cx="{hz_x}" cy="{Yn4(16.18e6)}" r="3.6" fill="{BLUE}"/>'
        + f'<text x="{hz_x+6}" y="{Yn4(16.18e6)-8}" font-size="10.5" fill="{INK}">8% hurdle: NPV +16.18m</text>'
        + f'<circle cx="{irr_x}" cy="{Yn4(0)}" r="4" fill="{CRIMSON}"/>'
        + f'<text x="{irr_x+6}" y="{Yn4(0)+16}" font-size="10.5" font-weight="600" fill="{CRIMSON}">IRR 12.19%</text>'
        + axes(L, T, W - R, H - B, "Discount rate", "NPV (USD)") + xt)
(PFL / "fig_4_1_1.svg").write_text(svg(W, H, body))

# ---- Fig 4.2.1 two paybacks -------------------------------------------------------
W, H, L, R, T, B = 640, 400, 84, 24, 24, 56
def Xy4(t): return L + t / 15 * (W - L - R)
def Yc4(v): return H - B - (v + 60e6) / 140e6 * (H - T - B)
nom, dis = [], []
cn = cd = -60e6
for t in range(16):
    nom.append((t, cn)); dis.append((t, cd))
    cn += 8.9e6; cd += 8.9e6 / 1.08 ** (t + 1)
shade = ("M" + " L".join(f"{Xy4(t)},{Yc4(v)}" for t, v in nom)
         + " L" + " L".join(f"{Xy4(t)},{Yc4(v)}" for t, v in reversed(dis)) + " Z")
grid = "".join(f'<line x1="{L}" y1="{Yc4(v*1e6)}" x2="{W-R}" y2="{Yc4(v*1e6)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Yc4(v*1e6)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v}m</text>'
               for v in range(-60, 81, 20))
xt = "".join(f'<text x="{Xy4(t)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{t}</text>' for t in range(0, 16, 3))
body = (grid + f'<path d="{shade}" fill="{BLUE}" opacity="0.08"/>'
        + f'<line x1="{L}" y1="{Yc4(0)}" x2="{W-R}" y2="{Yc4(0)}" stroke="{INK}" stroke-width="1"/>'
        + f'<polyline points="{" ".join(f"{Xy4(t)},{Yc4(v)}" for t, v in nom)}" fill="none" stroke="{SLATE}" stroke-width="2.2" stroke-dasharray="6 4"/>'
        + f'<polyline points="{" ".join(f"{Xy4(t)},{Yc4(v)}" for t, v in dis)}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
        + f'<circle cx="{Xy4(6.74)}" cy="{Yc4(0)}" r="3.6" fill="{INK}"/>'
        + f'<text x="{Xy4(6.74)}" y="{Yc4(0)-10}" font-size="10.5" fill="{INK}" text-anchor="middle">payback 6.74</text>'
        + f'<circle cx="{Xy4(10.07)}" cy="{Yc4(0)}" r="4" fill="{CRIMSON}"/>'
        + f'<text x="{Xy4(10.07)+4}" y="{Yc4(0)+18}" font-size="10.5" font-weight="600" fill="{CRIMSON}">discounted 10.07</text>'
        + f'<text x="{Xy4(12.6)}" y="{Yc4(30e6)}" font-size="10" fill="{SLATE}">the cost of time value</text>'
        + axes(L, T, W - R, H - B, "Years", "Cumulative cash (USD)") + xt)
(PFL / "fig_4_2_1.svg").write_text(svg(W, H, body))

# ---- Fig 4.3.1 crossover ----------------------------------------------------------
W, H, L, R, T, B = 640, 400, 84, 24, 24, 56
def Xr5(r): return L + r / 0.30 * (W - L - R)
def Yn5(v): return H - B - (v + 6e6) / 18e6 * (H - T - B)
p_pts = " ".join(f"{Xr5(r/1000)},{Yn5(2e6*af_f(r/1000,5)-5e6)}" for r in range(0, 301, 5))
q_pts = " ".join(f"{Xr5(r/1000)},{Yn5(6e6*af_f(r/1000,5)-20e6)}" for r in range(0, 301, 5))
grid = "".join(f'<line x1="{L}" y1="{Yn5(v*1e6)}" x2="{W-R}" y2="{Yn5(v*1e6)}" stroke="{GRID}"/>'
               f'<text x="{L-8}" y="{Yn5(v*1e6)+4}" font-size="10" fill="{SLATE}" text-anchor="end">{v}m</text>'
               for v in range(-6, 13, 3))
xt = "".join(f'<text x="{Xr5(r/100)}" y="{H-B+16}" font-size="10" fill="{SLATE}" text-anchor="middle">{r}%</text>' for r in range(0, 31, 5))
cx = Xr5(0.1042)
cy = Yn5(2e6 * af_f(0.1042, 5) - 5e6)
body = (grid + f'<line x1="{L}" y1="{Yn5(0)}" x2="{W-R}" y2="{Yn5(0)}" stroke="{INK}" stroke-width="1"/>'
        + f'<polyline points="{q_pts}" fill="none" stroke="{BLUE}" stroke-width="2.6"/>'
        + f'<polyline points="{p_pts}" fill="none" stroke="{SLATE}" stroke-width="2.2"/>'
        + f'<line x1="{Xr5(0.08)}" y1="{Yn5(-6e6)}" x2="{Xr5(0.08)}" y2="{Yn5(12e6)}" stroke="{SLATE}" stroke-dasharray="5 4"/>'
        + f'<text x="{Xr5(0.08)+4}" y="{T+14}" font-size="10" fill="{SLATE}">8% hurdle</text>'
        + f'<circle cx="{cx}" cy="{cy}" r="4" fill="{CRIMSON}"/>'
        + f'<text x="{cx+8}" y="{cy-8}" font-size="10.5" font-weight="600" fill="{CRIMSON}">crossover 10.42% — ranking flips</text>'
        + f'<text x="{Xr5(0.015)}" y="{Yn5(10.2e6)}" font-size="11" font-weight="600" fill="{BLUE}">Q (full-scale)</text>'
        + f'<text x="{Xr5(0.015)}" y="{Yn5(4.6e6)}" font-size="11" font-weight="600" fill="{SLATE}">P (compact)</text>'
        + f'<text x="{Xr5(0.152)+6}" y="{Yn5(0)+16}" font-size="9.5" fill="{BLUE}">IRR(Q) 15.24%</text>'
        + f'<text x="{Xr5(0.2865)+2}" y="{Yn5(0)-8}" font-size="9.5" fill="{SLATE}">IRR(P) 28.65%</text>'
        + axes(L, T, W - R, H - B, "Discount rate", "NPV (USD)") + xt)
(PFL / "fig_4_3_1.svg").write_text(svg(W, H, body))

# ---- Fig 1.1.1 recourse spectrum --------------------------------------------------
W, H = 700, 330
L2, R2 = 60, 60
def Xs(f): return L2 + f * (W - L2 - R2)
positions = [(0.0, "Corporate loan", "full recourse"), (0.33, "Guaranteed project loan", "parent guarantee"),
             (0.62, "Limited recourse", "completion support"), (1.0, "Non-recourse", "reserves only")]
body = (f'<line x1="{Xs(0)}" y1="120" x2="{Xs(1)}" y2="120" stroke="{INK}" stroke-width="2.4"/>'
        f'<text x="{Xs(0)}" y="66" font-size="12" font-weight="700" fill="{INK}">FULL RECOURSE</text>'
        f'<text x="{Xs(0)}" y="82" font-size="9.5" fill="{SLATE}">lender looks to the whole balance sheet</text>'
        f'<text x="{Xs(1)}" y="66" font-size="12" font-weight="700" fill="{BLUE}" text-anchor="end">NON-RECOURSE</text>'
        f'<text x="{Xs(1)}" y="82" font-size="9.5" fill="{SLATE}" text-anchor="end">lender looks only to project cash and security</text>')
for f, t, sub in positions:
    x = Xs(f)
    body += (f'<circle cx="{x}" cy="120" r="6" fill="white" stroke="{CRIMSON}" stroke-width="2.4"/>'
             f'<text x="{x}" y="147" font-size="10.5" font-weight="600" fill="{INK}" text-anchor="middle">{t}</text>'
             f'<text x="{x}" y="161" font-size="9" fill="{SLATE}" text-anchor="middle">{sub}</text>')
for label, y0, col, left_h, right_h in (("Sponsor risk retained", 200, SLATE, 44, 6),
                                        ("Financing cost & complexity", 262, BLUE, 8, 40)):
    body += f'<text x="{Xs(0)}" y="{y0-8}" font-size="10" font-weight="600" fill="{col}">{label}</text>'
    for i in range(9):
        f = i / 8
        h = left_h + (right_h - left_h) * f
        body += f'<rect x="{Xs(f)-7}" y="{y0+44-h}" width="14" height="{h}" rx="2" fill="{col}" opacity="0.55"/>'
(PFL / "fig_1_1_1.svg").write_text(svg(W, H, body))

# ---- Fig 1.1.2 SPV hub ------------------------------------------------------------
import math as _m
W, H = 700, 460
cx, cy = W / 2, H / 2
spokes = [("Sponsors", "equity in · distributions out", -90), ("Lenders", "debt in · debt service out", -30),
          ("Offtaker", "service out · availability payments in", 30), ("EPC contractor", "plant in · milestone payments out", 90),
          ("O&M contractor", "operations in · fees out", 150), ("Government", "permits & direct agreement", 210)]
body = ""
for name, flow, ang in spokes:
    a = _m.radians(ang)
    x, y = cx + 240 * _m.cos(a), cy + 165 * _m.sin(a)
    body += (f'<line x1="{cx + 62*_m.cos(a):.0f}" y1="{cy + 44*_m.sin(a):.0f}" x2="{x - 60*_m.cos(a):.0f}" y2="{y - 26*_m.sin(a):.0f}" stroke="{SLATE}" stroke-width="1.6"/>'
             f'<rect x="{x-78:.0f}" y="{y-26:.0f}" width="156" height="52" rx="8" fill="white" stroke="{INK}" stroke-width="1.4"/>'
             f'<text x="{x:.0f}" y="{y-6:.0f}" font-size="12" font-weight="700" fill="{INK}" text-anchor="middle">{name}</text>'
             f'<text x="{x:.0f}" y="{y+12:.0f}" font-size="8.6" fill="{SLATE}" text-anchor="middle">{flow}</text>')
body += (f'<ellipse cx="{cx}" cy="{cy}" rx="86" ry="52" fill="white" stroke="{BLUE}" stroke-width="3"/>'
         f'<text x="{cx}" y="{cy-4}" font-size="14" font-weight="800" fill="{BLUE}" text-anchor="middle">PROJECT SPV</text>'
         f'<text x="{cx}" y="{cy+15}" font-size="9" fill="{SLATE}" text-anchor="middle">ring-fenced · single purpose</text>')
(PFL / "fig_1_1_2.svg").write_text(svg(W, H, body))

# ---- Fig 1.2.1 bankability triangle -----------------------------------------------
W, H = 640, 470
ax, ay = W / 2, 62
bx, by = 96, 396
cxx, cyy = W - 96, 396
body = (f'<polygon points="{ax},{ay} {bx},{by} {cxx},{cyy}" fill="{BLUE}" opacity="0.05" stroke="{BLUE}" stroke-width="2.4"/>'
        f'<text x="{ax}" y="{ay-30}" font-size="13" font-weight="800" fill="{INK}" text-anchor="middle">VALUE</text>'
        f'<text x="{ax}" y="{ay-14}" font-size="9.5" fill="{SLATE}" text-anchor="middle">worth doing — NPV (Domain 4)</text>'
        f'<text x="{bx}" y="{by+24}" font-size="13" font-weight="800" fill="{INK}" text-anchor="middle">CASH</text>'
        f'<text x="{bx}" y="{by+40}" font-size="9.5" fill="{SLATE}" text-anchor="middle">arrives when needed (D6–8)</text>'
        f'<text x="{cxx}" y="{by+24}" font-size="13" font-weight="800" fill="{INK}" text-anchor="middle">RISK</text>'
        f'<text x="{cxx}" y="{by+40}" font-size="9.5" fill="{SLATE}" text-anchor="middle">sits with those who can bear it (D11–12)</text>'
        f'<text x="{W/2}" y="{(ay+by+by)/3+4}" font-size="12.5" font-weight="800" fill="{CRIMSON}" text-anchor="middle">BANKABLE</text>'
        f'<text x="{W/2}" y="{(ay+by+by)/3+21}" font-size="9.5" fill="{SLATE}" text-anchor="middle">all three, together (Domain 5)</text>'
        f'<text x="{(ax+bx)/2-14}" y="{(ay+by)/2}" font-size="9" fill="{SLATE}" text-anchor="end" transform="rotate(-60 {(ax+bx)/2-14} {(ay+by)/2})">no risk corner → repriced at close</text>'
        f'<text x="{(ax+cxx)/2+14}" y="{(ay+cyy)/2}" font-size="9" fill="{SLATE}" transform="rotate(60 {(ax+cxx)/2+14} {(ay+cyy)/2})">no cash corner → liquidity failure</text>'
        f'<text x="{W/2}" y="{by-12}" font-size="9" fill="{SLATE}" text-anchor="middle">no value corner → no equity</text>')
(PFL / "fig_1_2_1.svg").write_text(svg(W, H, body))

print("figures written:",
      *[p.relative_to(ROOT) for p in sorted(PFL.glob("*.svg")) + sorted(PML.glob("*.svg"))], sep="\n  ")
