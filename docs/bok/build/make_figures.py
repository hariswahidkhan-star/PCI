#!/usr/bin/env python3
"""Render the PCL-AI BoK figure specifications as SVG diagrams.

Each figure is drawn from the exact underlying data in its spec block in the text.
Style: brand blue #1D4ED8, clean professional diagrams, DejaVu Sans labels.
Run with python3.12 (matplotlib installed there). Outputs to build/figures/.
"""
import pathlib
import matplotlib
matplotlib.use("svg")
import matplotlib.pyplot as plt
from matplotlib.patches import FancyBboxPatch, FancyArrowPatch, Polygon, Circle, Rectangle

BLUE, INK, SLATE, LIGHT, GREY = "#1D4ED8", "#0F172A", "#64748B", "#EEF3FF", "#CBD5E1"
RED, GREEN, AMBER, PALE = "#C13329", "#16A34A", "#D97706", "#F1F5F9"
OUT = pathlib.Path(__file__).resolve().parent / "figures"
OUT.mkdir(exist_ok=True)
plt.rcParams.update({
    "font.family": ["Inter", "DejaVu Sans"], "font.size": 9, "svg.fonttype": "none",
    "axes.edgecolor": GREY, "axes.linewidth": 0.8,
    "axes.titlesize": 10, "axes.titleweight": "bold", "axes.titlecolor": INK,
    "axes.labelcolor": SLATE, "axes.labelsize": 8.5,
    "xtick.color": SLATE, "ytick.color": SLATE,
    "xtick.labelsize": 8, "ytick.labelsize": 8,
    "lines.linewidth": 2.2, "lines.solid_capstyle": "round",
    "legend.frameon": False, "legend.fontsize": 8,
})
CHART_GRID = "#EDF1F7"

def _premium(fig):
    """Uniform premium pass applied to every figure at save time."""
    for ax in fig.get_axes():
        if not ax.axison:
            continue
        for s in ("top", "right"):
            ax.spines[s].set_visible(False)
        for s in ("left", "bottom"):
            ax.spines[s].set_color(GREY)
            ax.spines[s].set_linewidth(0.8)
        ax.tick_params(colors=SLATE, width=0.8, length=3)
        ax.set_axisbelow(True)
        ax.grid(axis="y", color=CHART_GRID, linewidth=0.7)
        leg = ax.get_legend()
        if leg is not None:
            leg.set_frame_on(False)
        for cont in getattr(ax, "containers", []):
            for patch in cont:
                if hasattr(patch, "set_linewidth"):
                    patch.set_linewidth(0)
        from matplotlib.ticker import ScalarFormatter, FuncFormatter
        if isinstance(ax.yaxis.get_major_formatter(), ScalarFormatter):
            ticks = ax.get_yticks()
            if len(ticks) and max(abs(t) for t in ticks) >= 1000:
                ax.yaxis.set_major_formatter(FuncFormatter(lambda v, _: f"{v:,.0f}"))

def newfig(w=7.0, h=3.4):
    fig, ax = plt.subplots(figsize=(w, h))
    ax.axis("off")
    return fig, ax

def save(fig, name):
    _premium(fig)
    fig.savefig(OUT / f"{name}.svg", bbox_inches="tight", pad_inches=0.08)
    plt.close(fig)
    print("made", name)

def bx(ax, x, y, w, h, text, fc=LIGHT, ec=BLUE, fs=8.5, tc=INK, lw=1.2, bold=False):
    ax.add_patch(FancyBboxPatch((x, y), w, h, boxstyle="round,pad=0.02,rounding_size=0.06",
                                fc=fc, ec=ec, lw=lw))
    ax.text(x + w/2, y + h/2, text, ha="center", va="center", fontsize=fs, color=tc,
            fontweight="bold" if bold else "normal", wrap=True)

def ar(ax, x1, y1, x2, y2, color=SLATE, lw=1.4, style="-|>"):
    ax.add_patch(FancyArrowPatch((x1, y1), (x2, y2), arrowstyle=style, mutation_scale=12,
                                 color=color, lw=lw))

# ---------------- Domain 1 ----------------
def fig_1_1_1():
    fig, ax = newfig(6.6, 3.2)
    ax.set_xlim(0, 12); ax.set_ylim(0, 6)
    ax.text(6, 5.45, "A  =  L  +  E", ha="center", fontsize=15, color=INK, fontweight="bold")
    ax.plot([2, 10], [4.2, 4.2], color=INK, lw=3, solid_capstyle="round")   # beam
    ax.add_patch(Polygon([(6, 4.2), (5.45, 2.9), (6.55, 2.9)], fc=SLATE))   # fulcrum
    for xp in (2.6, 9.4): ax.plot([xp, xp], [4.2, 3.6], color=GREY, lw=1.4)
    bx(ax, 0.9, 2.2, 3.4, 1.4, "Assets\n100,000", fc=BLUE, ec=BLUE, tc="white", bold=True, fs=9.5)
    bx(ax, 7.7, 2.5, 3.4, 1.1, "Equity  100,000", fc=BLUE, ec=BLUE, tc="white", bold=True, fs=9)
    bx(ax, 7.7, 1.7, 3.4, 0.65, "Liabilities  0", fc=PALE, ec=GREY, tc=SLATE, fs=8)
    save(fig, "fig_1_1_1")

def fig_1_1_2():
    fig, ax = newfig(7.4, 1.9)
    ax.set_xlim(0, 15); ax.set_ylim(0, 3.4)
    steps = ["Source\ndocument", "Journal entry\n(Dr / Cr)", "Ledger\n(T-accounts)",
             "Trial balance", "Financial\nstatements"]
    for i, s in enumerate(steps):
        bx(ax, 0.3 + i*3.0, 1.1, 2.4, 1.4, s, fc=BLUE if i == 3 else LIGHT,
           ec=BLUE, tc="white" if i == 3 else INK, bold=(i == 3))
        if i < 4: ar(ax, 2.75 + i*3.0, 1.8, 3.25 + i*3.0, 1.8, color=BLUE)
    ax.text(9.9, 0.35, "Debits 192,000 = Credits 192,000", ha="center", fontsize=8,
            color=BLUE, fontweight="bold")
    save(fig, "fig_1_1_2")

def fig_1_2_1():
    fig, ax = newfig(7.2, 3.5)
    ax.set_xlim(0, 12); ax.set_ylim(0, 7)
    bx(ax, 0.5, 4.4, 3.9, 2.1, "SOPL\nProfit 17,000", bold=True, fs=9.5)
    bx(ax, 7.6, 4.4, 3.9, 2.1, "SOCE\nRetained earnings\n17,000", bold=True, fs=9.5)
    bx(ax, 7.6, 0.7, 3.9, 2.1, "SOFP\nEquity 117,000\nCash 129,000", fc=BLUE, ec=BLUE, tc="white", bold=True, fs=9.5)
    bx(ax, 0.5, 0.7, 3.9, 2.1, "Cash flows\nNet increase\n129,000", bold=True, fs=9.5)
    ar(ax, 4.55, 5.45, 7.45, 5.45, color=BLUE, lw=2)
    ax.text(6, 5.8, "profit", ha="center", fontsize=8.5, color=BLUE, fontweight="bold")
    ar(ax, 9.55, 4.25, 9.55, 3.0, color=BLUE, lw=2)
    ax.text(10.05, 3.62, "to equity", fontsize=8.5, color=BLUE, fontweight="bold")
    ar(ax, 4.55, 1.75, 7.45, 1.75, color=BLUE, lw=2)
    ax.text(6, 2.1, "closing cash", ha="center", fontsize=8.5, color=BLUE, fontweight="bold")
    save(fig, "fig_1_2_1")

def fig_1_3_1():
    fig, ax = newfig(7.2, 2.2)
    ax.set_xlim(0, 12); ax.set_ylim(0, 4)
    ax.plot([1, 11], [1.6, 1.6], color=INK, lw=2)
    for i, m in enumerate(["Month 1\nwork performed", "Month 2\ninvoiced", "Month 3\npaid"]):
        x = 2 + i*4
        ax.plot([x, x], [1.45, 1.75], color=INK, lw=2)
        ax.text(x, 0.7, m, ha="center", fontsize=8, color=SLATE)
    ax.plot(2, 1.6, "o", ms=13, color=BLUE)
    ax.text(2, 2.6, "Accrual basis:\nexpense recognised", ha="center", fontsize=8, color=BLUE, fontweight="bold")
    ax.plot(10, 1.6, "o", ms=13, color=SLATE)
    ax.text(10, 2.6, "Cash basis:\nexpense recognised", ha="center", fontsize=8, color=SLATE, fontweight="bold")
    save(fig, "fig_1_3_1")

def fig_1_4_1():
    fig, ax = newfig(7.4, 4.2)
    ax.set_xlim(0, 13); ax.set_ylim(0, 9)
    qs = [("Present obligation\nfrom a past event?", 7.4), ("Outflow probable?", 4.7),
          ("Reliably estimable?", 2.0)]
    for q, y in qs:
        ax.add_patch(Polygon([(3.6, y+0.9), (5.6, y+1.7), (7.6, y+0.9), (5.6, y+0.1)], fc=LIGHT, ec=BLUE, lw=1.2))
        ax.text(5.6, y+0.9, q, ha="center", va="center", fontsize=8, color=INK)
    ar(ax, 5.6, 7.5, 5.6, 6.5, color=BLUE); ax.text(5.9, 7.0, "yes", fontsize=8, color=GREEN)
    ar(ax, 5.6, 4.8, 5.6, 3.8, color=BLUE); ax.text(5.9, 4.3, "yes", fontsize=8, color=GREEN)
    bx(ax, 9.3, 7.8, 3.3, 1.0, "No action", fc=PALE, ec=GREY, tc=SLATE)
    bx(ax, 9.3, 4.9, 3.3, 1.2, "Only possible?\nDisclose contingent\nliability", fc=PALE, ec=AMBER, tc=INK, fs=7.5)
    bx(ax, 9.3, 2.0, 3.3, 1.0, "Disclose", fc=PALE, ec=AMBER, tc=INK)
    bx(ax, 3.9, 0.1, 3.4, 0.9, "Recognise provision", fc=BLUE, ec=BLUE, tc="white", bold=True)
    ar(ax, 7.7, 8.3, 9.2, 8.3, color=SLATE); ax.text(8.4, 8.55, "no", fontsize=8, color=RED)
    ar(ax, 7.7, 5.6, 9.2, 5.6, color=SLATE); ax.text(8.4, 5.85, "no", fontsize=8, color=RED)
    ar(ax, 7.7, 2.9, 9.2, 2.7, color=SLATE); ax.text(8.4, 3.1, "no", fontsize=8, color=RED)
    ar(ax, 5.6, 2.1, 5.6, 1.1, color=BLUE); ax.text(5.9, 1.6, "yes", fontsize=8, color=GREEN)
    save(fig, "fig_1_4_1")

def fig_1_5_1():
    fig, ax = newfig(7.0, 3.4)
    ax.set_xlim(0, 12); ax.set_ylim(0, 6.5)
    wbs = ["Foundations", "Structure", "Fit-out"]; cbs = ["Labour", "Materials", "Subcontract", "Plant"]
    for j, c in enumerate(cbs):
        ax.text(3.6 + j*2.0, 5.4, c, ha="center", fontsize=8, color=SLATE)
    for i, wname in enumerate(wbs):
        ax.text(2.2, 4.5 - i*1.2, wname, ha="right", fontsize=8, color=SLATE)
        for j in range(4):
            hot = (i == 0 and j == 0)
            ax.add_patch(Rectangle((2.6 + j*2.0, 4.05 - i*1.2), 1.9, 1.0,
                                   fc=BLUE if hot else PALE, ec=GREY, lw=0.8))
            if hot:
                ax.text(3.55, 4.55, "USD 12,000", ha="center", va="center", fontsize=8,
                        color="white", fontweight="bold")
    ax.text(6.5, 6.2, "CBS — cost element →", ha="center", fontsize=8.5, color=INK, fontweight="bold")
    ax.text(0.4, 3.3, "WBS — scope ↓", rotation=90, va="center", fontsize=8.5, color=INK, fontweight="bold")
    ax.text(6.5, 0.5, "OBS: Civils team  →  control account CA-Civils-Foundations",
            ha="center", fontsize=8.5, color=BLUE, fontweight="bold")
    ar(ax, 4.5, 1.0, 3.7, 3.95, color=BLUE)
    save(fig, "fig_1_5_1")

# ---------------- Domain 2 ----------------
def fig_2_2_1():
    fig, ax = newfig(7.4, 2.4)
    ax.set_xlim(0, 15.4); ax.set_ylim(0, 4.6)
    steps = ["1 Identify\nthe contract", "2 Performance\nobligations", "3 Transaction\nprice",
             "4 Allocate", "5 Recognise\nrevenue"]
    for i, s in enumerate(steps):
        bx(ax, 0.3 + i*3.05, 2.6, 2.5, 1.6, s, fc=LIGHT, ec=BLUE, bold=False)
        if i < 4: ar(ax, 2.85 + i*3.05, 3.4, 3.3 + i*3.05, 3.4, color=BLUE)
    bx(ax, 6.4, 0.4, 4.2, 1.1, "Over time\n(input / output method)", fc=BLUE, ec=BLUE, tc="white", fs=8)
    bx(ax, 10.9, 0.4, 4.2, 1.1, "Point in time\n(on control transfer)", fc=PALE, ec=GREY, tc=SLATE, fs=8)
    ar(ax, 13.0, 2.5, 9.5, 1.6, color=SLATE); ar(ax, 13.4, 2.5, 13.0, 1.6, color=SLATE)
    save(fig, "fig_2_2_1")

def fig_2_2_2():
    fig, ax = plt.subplots(figsize=(6.4, 3.4))
    yrs = [0, 1, 2, 3]
    rev = [0, 2.4, 6.75, 12.0]; cost = [0, 1.8, 5.4, 9.6]
    ax.plot(yrs, rev, "-o", color=BLUE, lw=2.2, label="Cumulative revenue")
    ax.plot(yrs, cost, "-o", color=SLATE, lw=2.2, label="Cumulative cost")
    ax.fill_between(yrs, cost, rev, color=LIGHT, alpha=0.9)
    for x, r, c in zip(yrs[1:], rev[1:], cost[1:]):
        ax.annotate(f"profit {r-c:.2f}m", ((x), (r+c)/2), ha="center", fontsize=7.5, color=BLUE)
    ax.set_xticks(yrs); ax.set_xticklabels(["Award", "Year 1", "Year 2", "Year 3"])
    ax.set_ylabel("USD m"); ax.legend(frameon=False, fontsize=8)
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_2_2_2")

# ---------------- Domain 3 ----------------
def fig_3_1_1():
    fig, ax = plt.subplots(figsize=(6.6, 3.2))
    ax.bar(0, 9.0, color=BLUE)
    ax.bar(1, 0.7, bottom=9.0, color="#60A5FA")
    ax.bar(2, 9.7, color=BLUE, alpha=0.35)
    ax.bar(3, 0.5, bottom=9.7, color=GREY)
    ax.bar(4, 10.2, color=SLATE, alpha=0.4)
    for x, v, lab in [(0, 9.0, "Control-account\nbudgets 9.0m"), (1, 9.7, "+ Contingency 0.7m"),
                      (2, 9.7, "Cost baseline\nBAC 9.7m"), (3, 10.2, "+ Management\nreserve 0.5m"),
                      (4, 10.2, "Total budget\n10.2m")]:
        ax.text(x, v + 0.15, lab, ha="center", fontsize=7.5, color=INK)
    ax.set_xticks([]); ax.set_ylim(0, 12); ax.set_ylabel("USD m")
    ax.axhline(9.7, color=BLUE, lw=0.8, ls="--")
    ax.text(4.55, 9.55, "inside baseline ↓", fontsize=7, color=BLUE, ha="right")
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_3_1_1")

def fig_3_3_1():
    fig, ax = plt.subplots(figsize=(6.6, 3.3))
    months = list(range(1, 11))
    monthly = [40, 70, 110, 140, 160, 150, 130, 100, 60, 40]
    cum = [sum(monthly[:i+1]) for i in range(10)]
    ax.bar(months, monthly, color=GREY, alpha=0.6, label="Monthly PV")
    ax2 = ax.twinx()
    ax2.plot(months, cum, "-o", color=BLUE, lw=2.4, label="Cumulative PV (S-curve)")
    ax2.axvline(5, color=SLATE, lw=1, ls="--")
    ax2.annotate("data date: PV = 520", (5, 520), xytext=(6, 640), fontsize=8, color=BLUE,
                 arrowprops=dict(arrowstyle="->", color=BLUE))
    ax.set_xlabel("Month"); ax.set_ylabel("Monthly (USD 000)"); ax2.set_ylabel("Cumulative (USD 000)")
    ax.set_xticks(months)
    for a in (ax, ax2):
        for s in ("top",): a.spines[s].set_visible(False)
    save(fig, "fig_3_3_1")

def fig_3_5_1():
    fig, ax = plt.subplots(figsize=(6.6, 3.2))
    m = [0, 1, 2, 3, 4, 5, 6]
    cash = [0, -200, -280, -250, -120, 0, 110]
    ax.plot(m, cash, "-o", color=BLUE, lw=2.4)
    ax.axhline(0, color=INK, lw=1)
    ax.fill_between(m, cash, 0, where=[c < 0 for c in cash], color=RED, alpha=0.12)
    ax.annotate("peak funding requirement (280)", (2, -280), xytext=(3.1, -262),
                fontsize=8, color=RED, arrowprops=dict(arrowstyle="->", color=RED))
    ax.set_xlabel("Month"); ax.set_ylabel("Cumulative cash (USD 000)")
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_3_5_1")

# ---------------- Domain 4 ----------------
def fig_4_2_1():
    fig, ax = plt.subplots(figsize=(6.8, 3.2))
    labels = ["Budget\n130,000", "Material\nprice 2,200", "Material\nqty 5,000",
              "Labour\nrate 4,200", "Labour\neff. 4,000", "Actual\n145,400"]
    bases = [0, 130.0, 132.2, 137.2, 141.4, 0]
    vals = [130.0, 2.2, 5.0, 4.2, 4.0, 145.4]
    cols = [BLUE, RED, RED, RED, RED, SLATE]
    for i, (b, v, c) in enumerate(zip(bases, vals, cols)):
        ax.bar(i, v, bottom=b, color=c, alpha=0.55 if i in (0, 5) else 0.9)
        top = b + v
        if 0 < i < 5:
            ax.plot([i-0.4, i-0.6], [b, b], color=GREY, lw=1)
            ax.text(i, top + 1.2, f"+{v:.1f}", ha="center", fontsize=7.5, color=RED)
    ax.set_xticks(range(6)); ax.set_xticklabels(labels, fontsize=7.5)
    ax.set_ylabel("USD 000"); ax.set_ylim(0, 160)
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_4_2_1")

# ---------------- Domain 5 ----------------
def fig_5_3_1():
    fig, ax = newfig(6.8, 3.2)
    ax.set_xlim(0, 12); ax.set_ylim(0, 6)
    bx(ax, 4.3, 2.2, 3.4, 1.6, "CONTROL\nACCOUNT", fc=BLUE, ec=BLUE, tc="white", bold=True)
    ins = [("Scope (WBS)", 0.4, 4.6), ("Budget (PV)", 0.4, 0.6),
           ("Actual cost (AC)", 4.6, 5.0), ("Schedule (SPI)", 4.6, 0.2)]
    bx(ax, 0.4, 4.4, 2.9, 1.0, "Scope (WBS)")
    bx(ax, 0.4, 0.6, 2.9, 1.0, "Budget (PV)")
    bx(ax, 4.55, 4.7, 2.9, 1.0, "Actual cost (AC)")
    bx(ax, 4.55, 0.3, 2.9, 1.0, "Schedule status")
    ar(ax, 3.3, 4.9, 4.6, 3.7, color=SLATE); ar(ax, 3.3, 1.1, 4.6, 2.3, color=SLATE)
    ar(ax, 6.0, 4.65, 6.0, 3.9, color=SLATE); ar(ax, 6.0, 1.35, 6.0, 2.1, color=SLATE)
    bx(ax, 8.7, 2.3, 3.0, 1.4, "Earned-value\nperformance\n(CPI / SPI)", fc=LIGHT, ec=BLUE, bold=True, fs=8)
    ar(ax, 7.8, 3.0, 8.6, 3.0, color=BLUE, lw=2)
    save(fig, "fig_5_3_1")

# ---------------- Domain 6 ----------------
def fig_6_1_1():
    fig, ax = plt.subplots(figsize=(6.6, 3.4))
    months = list(range(11))
    pv_m = [0, 40, 110, 220, 360, 520, 670, 800, 900, 960, 1000]
    ax.plot(months, pv_m, "-", color=BLUE, lw=2.2, label="PV (baseline)")
    frac_ev, frac_ac = 480/520, 530/520
    ev = [v*frac_ev for v in pv_m[:6]]; ac = [v*frac_ac for v in pv_m[:6]]
    ax.plot(range(6), ev, "-o", color=GREEN, lw=2, ms=4, label="EV (earned)")
    ax.plot(range(6), ac, "-o", color=AMBER, lw=2, ms=4, label="AC (actual)")
    ax.axvline(5, color=SLATE, lw=1, ls="--"); ax.text(5.1, 30, "data date", fontsize=7.5, color=SLATE)
    ax.annotate("SV (40)", (5, 500), xytext=(6.2, 430), fontsize=8, color=GREEN,
                arrowprops=dict(arrowstyle="->", color=GREEN))
    ax.annotate("CV (50)", (5, 505), xytext=(6.2, 560), fontsize=8, color=AMBER,
                arrowprops=dict(arrowstyle="->", color=AMBER))
    ax.set_xlabel("Month"); ax.set_ylabel("USD 000"); ax.legend(frameon=False, fontsize=8, loc="upper left")
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_6_1_1")

def fig_6_3_1():
    fig, ax = plt.subplots(figsize=(6.6, 3.4))
    months = list(range(6))
    pv_m = [0, 40, 110, 220, 360, 520]
    ac = [v*530/520 for v in pv_m]
    ax.plot(months, ac, "-", color=INK, lw=2.2, label="Actual cost to date")
    for eac, lab, c in [(1050, "EAC (a) 1,050", GREEN), (1104, "EAC (b) 1,104", AMBER),
                        (1152, "EAC (c) 1,152", RED)]:
        ax.plot([5, 10], [530, eac], "--", color=c, lw=1.8)
        ax.text(10.05, eac, lab, fontsize=8, color=c, va="center")
    ax.axhline(1000, color=BLUE, lw=1.4)
    ax.text(0.2, 1015, "BAC 1,000", fontsize=8, color=BLUE)
    ax.set_xlim(0, 12.6); ax.set_xlabel("Month"); ax.set_ylabel("USD 000")
    ax.legend(frameon=False, fontsize=8, loc="upper left")
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_6_3_1")

# ---------------- Domain 7 ----------------
def fig_7_1_1():
    fig, ax = newfig(7.2, 2.2)
    ax.set_xlim(0, 12); ax.set_ylim(0, 4)
    for i in range(60):
        ax.add_patch(Rectangle((1 + i*10/60, 1.6), 10/60, 0.7, fc=plt.cm.Blues(0.25 + i/110), ec="none"))
    ax.add_patch(Rectangle((1, 1.6), 10, 0.7, fc="none", ec=SLATE, lw=1))
    marks = [("Lump sum /\nfixed price", 1.4), ("Remeasure-\nment", 3.4), ("Target cost\npain/gain", 6.0),
             ("CPIF", 8.4), ("CPFF & T&M", 10.4)]
    for lab, x in marks:
        ax.plot([x, x], [1.5, 2.4], color=INK, lw=1.2)
        ax.text(x, 2.75, lab, ha="center", fontsize=7.5, color=INK)
    ax.text(1, 0.9, "Contractor bears cost risk", fontsize=8, color=BLUE, fontweight="bold")
    ax.text(11, 0.9, "Client bears cost risk", fontsize=8, color=SLATE, fontweight="bold", ha="right")
    save(fig, "fig_7_1_1")

# ---------------- Domain 8 ----------------
def fig_8_1_1():
    fig, ax = plt.subplots(figsize=(5.6, 4.2))
    ax.set_xlim(0, 10); ax.set_ylim(0, 10)
    ax.axvline(5, color=GREY, lw=1); ax.axhline(5, color=GREY, lw=1)
    quad = [("Monitor", 2.5, 2.5), ("Keep informed", 7.5, 2.5), ("Keep satisfied", 2.5, 7.5),
            ("Manage closely", 7.5, 7.5)]
    for lab, x, y in quad:
        ax.text(x, y + 1.7, lab, ha="center", fontsize=9, color=SLATE, style="italic")
    pts = [("Sponsor", 8.2, 8.4), ("Operations team", 7.8, 2.2), ("Regulator", 2.4, 8.2),
           ("Local community", 2.0, 1.8)]
    for lab, x, y in pts:
        ax.plot(x, y, "o", ms=12, color=BLUE)
        ax.text(x, y - 0.85, lab, ha="center", fontsize=8, color=INK)
    ax.set_xlabel("Interest →"); ax.set_ylabel("Power →")
    ax.set_xticks([]); ax.set_yticks([])
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_8_1_1")

def fig_8_6_1():
    fig, ax = newfig(7.2, 2.6)
    ax.set_xlim(0, 12); ax.set_ylim(0, 5)
    bands = ["Predictive", "Iterative", "Incremental", "Adaptive"]
    for i, b in enumerate(bands):
        bx(ax, 0.6 + i*2.8, 3.2, 2.5, 1.2, b, fc=plt.cm.Blues(0.25 + i*0.18), ec=BLUE,
           tc="white" if i > 1 else INK, bold=True)
    for y, lab_l, lab_r, title in [(1.9, "high", "low", "Requirements certainty"),
                                   (0.7, "low", "high", "Change rate")]:
        for i in range(50):
            ax.add_patch(Rectangle((0.6 + i*11/50, y), 11/50, 0.45,
                                   fc=plt.cm.Greys(0.15 + (i/130 if lab_l == "low" else (50-i)/130)), ec="none"))
        ax.text(0.5, y + 0.22, lab_l, ha="right", fontsize=7.5, color=SLATE, va="center")
        ax.text(11.7, y + 0.22, lab_r, fontsize=7.5, color=SLATE, va="center")
        ax.text(6, y + 0.22, title, ha="center", fontsize=7.5, color=INK, va="center")
    save(fig, "fig_8_6_1")

def fig_8_6_2():
    fig, ax = newfig(6.8, 3.4)
    ax.set_xlim(0, 12); ax.set_ylim(0, 6.6)
    ax.text(0.3, 5.7, "Incremental — add usable parts", fontsize=9, color=INK, fontweight="bold")
    for p in range(3):
        for b in range(p + 1):
            ax.add_patch(Rectangle((1.2 + p*3.6 + b*0.85, 4.2), 0.75, 0.9, fc=BLUE, ec="white"))
        ax.text(1.2 + p*3.6 + 1.3, 3.7, f"cycle {p+1}", fontsize=7.5, color=SLATE)
    ax.text(0.3, 2.9, "Iterative — refine the same whole", fontsize=9, color=INK, fontweight="bold")
    import numpy as np
    for p, n in enumerate([7, 14, 80]):
        th = np.linspace(0, 2*np.pi, n, endpoint=False)
        r = 0.75 * (1 + (0.22 if n < 20 else 0.06 if n < 40 else 0.0) * np.cos(th*3))
        xs = 2.0 + p*3.6 + r*np.cos(th); ys = 1.35 + r*np.sin(th)
        ax.add_patch(Polygon(list(zip(xs, ys)), fc=LIGHT, ec=BLUE, lw=1.4))
        ax.text(2.0 + p*3.6, 0.15, f"cycle {p+1}", ha="center", fontsize=7.5, color=SLATE)
    save(fig, "fig_8_6_2")

# ---------------- Domain 9 ----------------
def fig_9_3_1():
    fig, ax = plt.subplots(figsize=(6.6, 3.3))
    sp = [0, 1, 2, 3, 4, 5]
    done = [0, 30, 61, 91, 123, 152]
    scope = [300, 300, 300, 320, 320, 320]
    ax.plot(sp, scope, "-", color=SLATE, lw=2, label="Total scope (points)", drawstyle="steps-post")
    ax.fill_between(sp, done, color=LIGHT); ax.plot(sp, done, "-o", color=BLUE, lw=2.2, label="Completed")
    rate = (152 - 123)
    proj_x = [5, 5 + (320-152)/rate]; proj_y = [152, 320]
    ax.plot(proj_x, proj_y, "--", color=BLUE, lw=1.6)
    ax.annotate("scope added", (3, 320), xytext=(1.6, 335), fontsize=8, color=SLATE,
                arrowprops=dict(arrowstyle="->", color=SLATE))
    ax.annotate(f"forecast finish ≈ Sprint {proj_x[1]:.0f}", (proj_x[1], 320), xytext=(6.2, 240),
                fontsize=8, color=BLUE, arrowprops=dict(arrowstyle="->", color=BLUE))
    ax.set_xlabel("Sprint"); ax.set_ylabel("Story points"); ax.set_ylim(0, 380)
    ax.legend(frameon=False, fontsize=8, loc="lower right")
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_9_3_1")

# ---------------- Domain 10 ----------------
def fig_10_2_1():
    fig, ax = newfig(7.4, 3.6)
    ax.set_xlim(0, 14); ax.set_ylim(0, 7)
    # node: (x, y, label, ES, EF, LS, LF, TF, critical)
    nodes = {"A": (0.4, 2.8, 3, 0, 3, 0, 3, 0, True), "B": (3.9, 4.7, 4, 3, 7, 3, 7, 0, True),
             "C": (3.9, 0.9, 2, 3, 5, 7, 9, 4, False), "D": (7.4, 4.7, 5, 7, 12, 7, 12, 0, True),
             "E": (7.4, 0.9, 3, 5, 8, 9, 12, 4, False), "F": (10.9, 2.8, 2, 12, 14, 12, 14, 0, True)}
    for k, (x, y, d, es, ef, ls, lf, tf, crit) in nodes.items():
        bx(ax, x, y, 2.6, 1.7, f"{k} (dur {d})\nES {es} | EF {ef}\nLS {ls} | LF {lf}  TF {tf}",
           fc=BLUE if crit else PALE, ec=BLUE if crit else GREY,
           tc="white" if crit else SLATE, fs=7.3, bold=crit)
    edges = [("A", "B", True), ("A", "C", False), ("B", "D", True), ("C", "E", False),
             ("D", "F", True), ("E", "F", False)]
    cxy = {k: (v[0] + 1.3, v[1] + 0.85) for k, v in nodes.items()}
    for a, b, crit in edges:
        x1, y1 = cxy[a]; x2, y2 = cxy[b]
        dx, dy = x2 - x1, y2 - y1
        f1, f2 = 0.34, 0.34
        ar(ax, x1 + dx*f1, y1 + dy*f1, x2 - dx*f2, y2 - dy*f2,
           color=BLUE if crit else GREY, lw=2.4 if crit else 1.2)
    ax.text(7, 6.7, "Critical path A → B → D → F = 14 days", ha="center", fontsize=9,
            color=BLUE, fontweight="bold")
    save(fig, "fig_10_2_1")

# ---------------- Domain 12 ----------------
def fig_12_2_1():
    fig, ax = plt.subplots(figsize=(5.4, 4.2))
    import numpy as np
    for i in range(5):
        for j in range(5):
            score = (i + 1) + (j + 1)
            c = GREEN if score <= 4 else (AMBER if score <= 7 else RED)
            ax.add_patch(Rectangle((j, i), 1, 1, fc=c, alpha=0.28, ec="white"))
    pts = [("Ground conditions", 3.5, 1.2), ("Late permit", 1.6, 1.8), ("Supplier failure", 4.4, 0.4),
           ("Design change", 1.2, 2.4)]
    for lab, x, y in pts:
        ax.plot(x, y, "o", ms=10, color=BLUE)
        ax.text(x, y + 0.28, lab, ha="center", fontsize=7.3, color=INK)
    ax.set_xlim(0, 5); ax.set_ylim(0, 5)
    ax.set_xlabel("Impact →"); ax.set_ylabel("Probability →")
    ax.set_xticks([]); ax.set_yticks([])
    for s in ("top", "right"): ax.spines[s].set_visible(False)
    save(fig, "fig_12_2_1")

# ---------------- Domain 13 ----------------
def fig_13_1_1():
    fig, ax = newfig(5.6, 4.2)
    ax.set_xlim(-5, 5); ax.set_ylim(-4.4, 4.4)
    for r, lab, c in [(4.1, "ARTIFICIAL INTELLIGENCE", GREY), (2.8, "MACHINE LEARNING", "#93C5FD"),
                      (1.5, "GENERATIVE AI", BLUE)]:
        ax.add_patch(Circle((0, -0.2), r, fc=c, alpha=0.30, ec=c, lw=1.4))
    ax.text(0, 3.35, "AI — rules-based validation", ha="center", fontsize=8, color=SLATE)
    ax.text(0, 1.9, "ML — cost-forecast model", ha="center", fontsize=8, color=INK)
    ax.text(0, -0.2, "GenAI —\ndrafting a variance\nnarrative", ha="center", fontsize=8, color=INK, fontweight="bold")
    save(fig, "fig_13_1_1")

def fig_13_2_1():
    fig, ax = newfig(7.4, 1.9)
    ax.set_xlim(0, 15.4); ax.set_ylim(0, 3.4)
    steps = ["Source\n(ERP, schedule,\ncontracts)", "Quality gate\n(accuracy,\ncompleteness)", "Governed dataset\n(lineage tracked)",
             "AI model /\nassistant", "Output\n(verified)"]
    for i, s in enumerate(steps):
        bx(ax, 0.3 + i*3.05, 1.0, 2.5, 1.7, s, fc=BLUE if i == 4 else LIGHT, ec=BLUE,
           tc="white" if i == 4 else INK, fs=7.6, bold=(i == 4))
        if i < 4: ar(ax, 2.85 + i*3.05, 1.85, 3.3 + i*3.05, 1.85, color=BLUE)
    ax.plot([0.5, 14.9], [0.45, 0.45], color=GREY, lw=1, ls=":")
    ax.text(7.7, 0.12, "lineage — every output traceable to source", ha="center", fontsize=7.5, color=SLATE)
    save(fig, "fig_13_2_1")

def fig_13_4_1():
    fig, ax = plt.subplots(figsize=(7.0, 3.3))
    needs = ["Extract", "Draft", "Forecast", "Schedule", "Reconcile", "Analyse"]
    cats = ["LLM", "RAG", "Sheet", "BI", "PM-suite", "ML", "RPA", "CLM", "Meeting", "Coding"]
    fit = [[2,2,1,0,0,0,0,2,1,0], [2,1,0,0,0,0,0,0,1,0], [0,0,1,1,1,2,0,0,0,0],
           [0,0,0,0,2,1,0,0,0,0], [0,0,1,0,0,1,2,0,0,1], [1,0,2,2,0,1,0,0,0,0]]
    for i in range(len(needs)):
        for j in range(len(cats)):
            v = fit[i][j]
            c = BLUE if v == 2 else ("#93C5FD" if v == 1 else PALE)
            ax.add_patch(Rectangle((j, len(needs)-1-i), 0.94, 0.94, fc=c, ec="white"))
    ax.set_xticks([j + 0.47 for j in range(len(cats))]); ax.set_xticklabels(cats, fontsize=7.5, rotation=30, ha="right")
    ax.set_yticks([len(needs)-1-i + 0.47 for i in range(len(needs))]); ax.set_yticklabels(needs, fontsize=8)
    ax.set_xlim(0, len(cats)); ax.set_ylim(0, len(needs))
    ax.set_title("strong fit = dark · partial = light — capabilities evolve, validate current features",
                 fontsize=7.5, color=SLATE)
    for s in ax.spines.values(): s.set_visible(False)
    ax.tick_params(length=0)
    save(fig, "fig_13_4_1")

def fig_13_5_1():
    import numpy as np
    fig, ax = newfig(6.2, 5.0)
    ax.set_xlim(-5.4, 5.4); ax.set_ylim(-5.0, 5.0)
    stages = ["Estimating", "Budgeting", "Forecasting / EAC", "Cost control", "Scheduling",
              "Agile delivery", "Contracts", "Reporting", "Risk", "Financial reporting"]
    ax.add_patch(Circle((0, 0), 1.7, fc=BLUE, ec=BLUE))
    ax.text(0, 0, "AI proposes\n→ verify →\nprofessional owns", ha="center", va="center",
            fontsize=8, color="white", fontweight="bold")
    for i, s in enumerate(stages):
        a = np.pi/2 - i*2*np.pi/len(stages)
        x, y = 3.4*np.cos(a), 3.4*np.sin(a)
        ax.plot([1.75*np.cos(a), 2.6*np.cos(a)], [1.75*np.sin(a), 2.6*np.sin(a)], color=GREY, lw=1.1)
        ax.text(x, y, s, ha="center", va="center", fontsize=7.6, color=INK,
                bbox=dict(boxstyle="round,pad=0.28", fc=LIGHT, ec=BLUE, lw=0.9))
    save(fig, "fig_13_5_1")

def fig_13_6_1():
    fig, ax = newfig(6.4, 4.6)
    ax.set_xlim(0, 12); ax.set_ylim(0, 10.4)
    ax.add_patch(Polygon([(2.5, 9.2), (4.5, 10.0), (6.5, 9.2), (4.5, 8.4)], fc=LIGHT, ec=BLUE))
    ax.text(4.5, 9.2, "Deterministic task?", ha="center", va="center", fontsize=7.6)
    bx(ax, 8.2, 8.75, 3.3, 0.9, "Use a rule", fc=PALE, ec=GREY, tc=SLATE)
    ar(ax, 6.6, 9.2, 8.1, 9.2, color=SLATE); ax.text(7.3, 9.45, "yes", fontsize=7.5, color=SLATE)
    ax.add_patch(Polygon([(2.0, 6.6), (4.5, 7.5), (7.0, 6.6), (4.5, 5.7)], fc=LIGHT, ec=BLUE))
    ax.text(4.5, 6.6, "Data adequate &\nconfidentiality governed?", ha="center", va="center", fontsize=7.4)
    bx(ax, 8.2, 6.15, 3.3, 0.9, "Do not use /\ngoverned tool only", fc=PALE, ec=AMBER, tc=INK, fs=7.3)
    ar(ax, 7.1, 6.6, 8.1, 6.6, color=SLATE); ax.text(7.5, 6.85, "no", fontsize=7.5, color=RED)
    ar(ax, 4.5, 8.35, 4.5, 7.55, color=BLUE); ax.text(4.8, 7.95, "no", fontsize=7.5, color=RED)
    chain = [("AI proposes", 4.2), ("Verify (assurance checklist)", 2.9), ("Professional signs off\n+ audit trail recorded", 1.5)]
    for lab, y in chain:
        bx(ax, 2.7, y, 3.6, 1.0, lab, fc=LIGHT, ec=BLUE, fs=7.6)
    ar(ax, 4.5, 5.65, 4.5, 5.25, color=BLUE); ax.text(4.8, 5.45, "yes", fontsize=7.5, color=GREEN)
    ar(ax, 4.5, 4.15, 4.5, 3.95, color=BLUE); ar(ax, 4.5, 2.85, 4.5, 2.55, color=BLUE)
    bx(ax, 8.2, 1.55, 3.3, 0.9, "RELEASE", fc=BLUE, ec=BLUE, tc="white", bold=True)
    ar(ax, 6.4, 2.0, 8.1, 2.0, color=BLUE, lw=2)
    save(fig, "fig_13_6_1")

def fig_13_7_1():
    fig, ax = newfig(6.8, 3.4)
    ax.set_xlim(0, 12); ax.set_ylim(0, 6.6)
    stages = [("Ad-hoc", "individual experiments,\nno governance"),
              ("Piloting", "defined use cases,\nfirst guardrails"),
              ("Standardised", "approved tools, policy,\nverification embedded"),
              ("Integrated", "AI in the workflow\nand tooling"),
              ("Governed &\noptimised", "measured value,\nfull audit trail")]
    for i, (t, d) in enumerate(stages):
        x, y = 0.4 + i*2.35, 0.5 + i*1.05
        bx(ax, x, y, 2.15, 1.35, t, fc=plt.cm.Blues(0.25 + i*0.16), ec=BLUE,
           tc="white" if i > 1 else INK, bold=True, fs=8)
        ax.text(x + 1.07, y - 0.34, d, ha="center", fontsize=6.6, color=SLATE)
    save(fig, "fig_13_7_1")


def fig_5_2_2():
    fig, ax = plt.subplots(figsize=(6.8, 3.2))
    m = list(range(1, 9))
    commit = [200,450,640,760,880,950,990,1000]
    vowd   = [60,160,300,460,620,760,870,950]
    inv    = [20,90,200,340,490,640,760,860]
    ax.step(m, commit, where="post", color=INK, lw=2.0, label="Commitment")
    ax.plot(m, vowd, color=BLUE, lw=2.2, label="VOWD (work performed)")
    ax.plot(m, inv, color=SLATE, lw=1.8, ls="--", label="Invoiced")
    ax.fill_between(m, inv, vowd, color=BLUE, alpha=0.08)
    ax.annotate("accrual", xy=(6, 700), fontsize=7.5, color=BLUE)
    ax.annotate("open commitment", xy=(3.1, 700), fontsize=7.5, color=SLATE)
    ax.set_xlabel("Month"); ax.set_ylabel("USD 000"); ax.legend(loc="lower right")
    save(fig, "fig_5_2_2")

def fig_7_2_1():
    fig, ax = newfig(7.4, 1.9)
    ax.set_xlim(0, 15); ax.set_ylim(0, 3.4)
    steps = ["Form\n(award, terms)", "Mobilise\n(bonds, advance)", "Administer\n(variations, claims,\ncertificates)",
             "Complete\n(punch, handover)", "Close\n(final account,\nretention release)"]
    for i, s in enumerate(steps):
        bx(ax, 0.3 + i*3.0, 1.0, 2.4, 1.6, s, fc=BLUE if i == 2 else LIGHT,
           ec=BLUE, tc="white" if i == 2 else INK, bold=(i == 2), fs=7.5)
        if i < 4: ar(ax, 2.75 + i*3.0, 1.8, 3.25 + i*3.0, 1.8, color=BLUE)
    ax.text(7.5, 0.3, "Records made while administering are the claims evidence at close (7.2.2, 7.T.2).",
            ha="center", fontsize=7.5, color=SLATE)
    save(fig, "fig_7_2_1")

def fig_8_2_2():
    fig, ax = plt.subplots(figsize=(6.6, 3.0))
    m = list(range(1, 11)); crew = [8,14,22,30,36,38,34,26,16,8]
    cols = [RED if c > 32 else BLUE for c in crew]
    ax.bar(m, crew, color=cols, width=0.72)
    ax.axhline(32, color=INK, lw=1.4, ls="--")
    ax.text(9.6, 33, "available: 32", fontsize=7.5, color=INK, ha="right")
    ax.set_xlabel("Month"); ax.set_ylabel("Headcount")
    save(fig, "fig_8_2_2")

def fig_9_3_4():
    fig, ax = plt.subplots(figsize=(6.6, 3.2))
    s = list(range(0, 11))
    scope = [250]*6 + [290]*5
    done = [0,22,48,72,95,120,150,172,196,222,248]
    ax.step(s, scope, where="post", color=INK, lw=2.0, label="Scope (points)")
    ax.plot(s, done, color=BLUE, lw=2.2, marker="o", ms=3.5, label="Done (cumulative)")
    ax.annotate("scope +40 at Sprint 6\n(rebaseline logged)", xy=(6, 290), xytext=(3.2, 300),
                fontsize=7.5, color=RED, arrowprops=dict(arrowstyle="->", color=RED, lw=1.1))
    ax.set_xlabel("Sprint"); ax.set_ylabel("Story points"); ax.legend(loc="lower right")
    save(fig, "fig_9_3_4")

def fig_10_3_1():
    fig, ax = plt.subplots(figsize=(6.4, 3.2))
    days = [14,13,12,11,10]; cost = [0,2000,4000,7500,12500]
    ax.plot(days, cost, color=BLUE, lw=2.2, marker="o", ms=4)
    for d, c, t in [(13,2000,"crash A (2,000/d)"),(12,4000,"crash A again"),(11,7500,"crash B (3,500)"),(10,12500,"B + C: both paths\n(5,000/d)")]:
        ax.annotate(t, xy=(d, c), xytext=(d-0.05, c+900), fontsize=7, color=SLATE)
    ax.invert_xaxis()
    ax.set_xlabel("Project duration (days)"); ax.set_ylabel("Cumulative crash cost (USD)")
    save(fig, "fig_10_3_1")


ALL = {n[4:].replace("_", "."): f for n, f in list(globals().items()) if n.startswith("fig_")}

if __name__ == "__main__":
    ok = fail = 0
    for name, fn in sorted(ALL.items()):
        try:
            fn(); ok += 1
        except Exception as e:
            print(f"FAIL {name}: {e}"); fail += 1
    print(f"figures: {ok} ok, {fail} failed")
