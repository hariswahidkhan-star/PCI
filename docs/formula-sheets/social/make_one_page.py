#!/usr/bin/env python3
"""Generate the one-page PCL-AI formula sheet HTML from data.

The sheet is a reference poster, so the formulas live here as a list rather than as
markup: layout can be retuned without touching sixty-three hand-written rows, and the
set can be checked against the Body of Knowledge's Appendix A at a glance.

Each group is (area, title, domain-label, [(formula, KA), ...]) where area is
A — financial reporting and accounting
B — project management, controls and delivery
C — where the two meet

Writes pcl-ai-one-page.html next to this file; render it with build_social.py.
"""
import pathlib

AREA_COLOUR = {"A": "#1D4ED8", "B": "#0F172A", "C": "#C13329"}

GROUPS = [
    ("A", "The accounting model", "D1", [
        ("A = L + E", "1.1.1"),
        ("Σ Debits = Σ Credits", "1.1.3"),
        ("Retained earnings = Open + Income − Expenses − Distrib.", "1.1.1"),
        ("Depreciation = (Cost − Residual) ÷ Useful life", "1.3.4"),
        ("Carrying amount = Cost − Depreciation − Impairment", "1.3.4"),
    ]),
    ("A", "Provisions", "D1 · IAS 37", [
        ("Expected value = Σ (probability × outcome)", "1.4.3"),
        ("Present value = Future amount ÷ (1 + r)^n", "1.4.3"),
    ]),
    ("A", "Revenue on a contract", "D2 · IFRS 15", [
        ("PoC = Costs incurred ÷ Total est. costs", "2.2.6"),
        ("Cumulative revenue = PoC × Transaction price", "2.2.6"),
        ("Period revenue = Cumulative − Recognised", "2.2.6"),
        ("Allocated price = Price × (SSP_i ÷ Σ SSP)", "2.2.5"),
        ("Capitalised interest = Weighted-avg expenditure × rate", "2.4.4"),
    ]),
    ("B", "Budget and estimate", "D3", [
        ("BAC = Σ control accounts + contingency", "3.1.4"),
        ("Total authorised = BAC + Mgmt reserve", "3.1.4"),
        ("Analogous = Past cost × (driver ÷ past driver)", "3.2.2"),
        ("Parametric = Parameter × Rate", "3.2.2"),
    ]),
    ("B", "Cost behaviour and control", "D5", [
        ("Total cost = Fixed + (Variable/unit × Volume)", "5.1.1"),
        ("OAR = Budgeted overhead ÷ Activity base", "5.1.3"),
        ("Over/(under) absorption = Absorbed − Incurred", "5.1.3"),
        ("Cost to date = Actuals + Accruals", "5.2.1"),
    ]),
    ("B", "Variance analysis", "D4", [
        ("Price var. = (Actual − Std price) × Actual qty", "4.2.3"),
        ("Quantity var. = (Actual − Std qty) × Std price", "4.2.3"),
        ("Total variance = Price var. + Quantity var.", "4.2.3"),
    ]),
    ("B", "Earned value — position", "D6", [
        ("CV = EV − AC", "6.2.1"),
        ("SV = EV − PV", "6.2.1"),
        ("CPI = EV ÷ AC", "6.2.2"),
        ("SPI = EV ÷ PV", "6.2.2"),
        ("% complete = EV ÷ BAC", "6.1"),
    ]),
    ("B", "Earned value — forecast", "D6", [
        ("EAC = AC + ETC", "6.3.1"),
        ("EAC = AC + (BAC − EV)", "6.3.2"),
        ("EAC = BAC ÷ CPI", "6.3.2"),
        ("EAC = AC + (BAC − EV) ÷ CPI", "6.3.2"),
        ("EAC = AC + (BAC − EV) ÷ (CPI × SPI)", "6.3.2"),
        ("VAC = BAC − EAC", "6.3.4"),
        ("TCPI = (BAC − EV) ÷ (BAC − AC)", "6.2.3"),
        ("TCPI = (BAC − EV) ÷ (EAC − AC)", "6.2.3"),
    ]),
    ("B", "Earned schedule", "D6", [
        ("ES = M + (EV − PV_M) ÷ (PV_M+1 − PV_M)", "6.4.3"),
        ("SV(t) = ES − AT", "6.4.3"),
        ("SPI(t) = ES ÷ AT", "6.4.3"),
    ]),
    ("B", "Adaptive delivery", "D9", [
        ("% complete = Points done ÷ Total points", "9.5.3"),
        ("EV = % complete × BAC", "9.5.3"),
        ("Velocity = Points done ÷ Sprint", "9.3"),
        ("Sprints left = Points left ÷ Velocity", "9.3"),
        ("Run rate = Team cost ÷ Sprint", "9.5.2"),
        ("Cycle time = WIP ÷ Throughput", "9.4"),
    ]),
    ("B", "Network and float", "D10", [
        ("EF = ES + Duration", "10.2"),
        ("LS = LF − Duration", "10.2"),
        ("TF = LS − ES = LF − EF", "10.2.4"),
        ("FF = min(successor ES) − EF", "10.2.4"),
        ("Critical path: TF = 0", "10.2.3"),
    ]),
    ("B", "Duration and compression", "D10", [
        ("tE = (O + 4M + P) ÷ 6", "10.1.4"),
        ("σ = (P − O) ÷ 6", "10.1.4"),
        ("σ² path = Σ σ² on the path", "10.3.4"),
        ("Crash slope = Δ cost ÷ Δ duration", "10.3.1"),
    ]),
    ("B", "Risk and contingency", "D12 · ISO 31000", [
        ("EMV = Probability × Impact", "12.2.3"),
        ("Total EMV = Σ (P_i × I_i)", "12.2.3"),
        ("Contingency ≈ Σ EMV, or P80 − P50", "12.3.1"),
    ]),
    ("B", "Commercial", "D7", [
        ("Fee = Target fee + Share × (Target − Actual)", "7.1.3"),
        ("Pain/gain = Share ratio × (Actual − Target)", "7.1.4"),
        ("LD exposure = LD rate × Days late", "7.2.3"),
        ("Amount due = Σ(% × item) − Retention − Previous", "7.4.3"),
    ]),
    ("C", "Where the two languages meet", "D7 · IFRS 15 · IAS 37", [
        ("Contract asset/(liability) = Revenue − Billed", "7.5"),
        ("EAC &gt; Contract value → onerous contract", "2.4"),
    ]),
    ("A", "Working capital", "D11", [
        ("DSO = Receivables ÷ Daily revenue", "11.1.3"),
        ("DIO = Inventory ÷ Daily COGS", "11.A.1"),
        ("DPO = Payables ÷ Daily COGS", "11.A.1"),
        ("CCC = DSO + DIO − DPO", "11.A.1"),
        ("Cash freed ≈ Δ DSO × Daily revenue", "11.A.1"),
    ]),
]

CSS = """
@font-face{font-family:'Archivo';src:url('/backend/wwwroot/assets/fonts/archivo-latin.woff2') format('woff2');font-weight:700 900}
@font-face{font-family:'Inter';src:url('/backend/wwwroot/assets/fonts/inter-latin.woff2') format('woff2');font-weight:400 700}
*{margin:0;padding:0;box-sizing:border-box}
html,body{width:100%;height:100%;background:#fff}
body{font-family:'Inter',sans-serif;color:#0F172A;overflow:hidden}

.card{position:relative;width:100vw;height:100vh;padding:40px 46px 26px;
  background:#fff;display:flex;flex-direction:column}
.card::after{content:"";position:absolute;top:0;left:0;right:0;height:11px;background:#1D4ED8}

.top{display:flex;align-items:center;margin-bottom:17px}
.top img{width:66px;height:66px}
.top .w{font-family:'Archivo';font-weight:900;font-size:35px;letter-spacing:-.02em;
  margin-left:16px;color:#1D4ED8}
.top .o{font-size:16px;line-height:1.25;margin-left:16px;padding-left:16px;
  border-left:2px solid #E3E8EF;color:#475569}
.top .badge{margin-left:auto;display:flex;align-items:center;gap:12px;
  border:2.5px solid #1D4ED8;border-radius:999px;padding:9px 19px 9px 16px}
.top .badge b{font-family:'Archivo';font-weight:900;font-size:23px;color:#1D4ED8}
.top .badge span{font-size:14px;letter-spacing:.13em;text-transform:uppercase;
  color:#64748B;border-left:2px solid #E3E8EF;padding-left:12px}

h1{font-family:'Archivo';font-weight:900;font-size:49px;line-height:1;
  letter-spacing:-.036em;margin-bottom:9px;white-space:nowrap}
.dot{color:#C13329}
.sub{font-size:15.5px;color:#475569;margin-bottom:9px}
.sub b{color:#0F172A;font-weight:700}

.legend{display:flex;gap:22px;align-items:center;font-size:13.5px;color:#475569;
  padding-bottom:9px;border-bottom:3px solid #0F172A;margin-bottom:11px;white-space:nowrap}
.legend i{display:inline-block;width:13px;height:13px;border-radius:3px;
  margin-right:9px;vertical-align:-2px;font-style:normal}
.legend em{font-style:normal;margin-left:auto;color:#94A3B8;letter-spacing:.11em;
  text-transform:uppercase;font-size:13px;font-weight:700}

.grid{flex:1 1 auto;min-height:0;overflow:hidden;column-count:3;column-gap:26px;column-fill:balance}
.grp{break-inside:avoid;margin-bottom:7px}
.grp h2{font-family:'Archivo';font-weight:900;font-size:15px;letter-spacing:.01em;
  text-transform:uppercase;color:#0F172A;display:flex;align-items:center;
  padding:0 0 4px 0;margin-bottom:4px;border-bottom:2.5px solid #0F172A}
.grp h2 i{display:inline-block;width:11px;height:11px;border-radius:2px;
  margin-right:8px;font-style:normal;flex:none}
.grp h2 span{margin-left:auto;font-family:'Inter';font-weight:700;font-size:11.5px;
  letter-spacing:.06em;color:#94A3B8;padding-left:7px;white-space:nowrap}

.f{display:flex;align-items:baseline;gap:9px;padding:2.1px 0;
  border-bottom:1px solid #EEF2F7}
.f:last-child{border-bottom:0}
.f code{font-family:'Inter';font-weight:700;font-size:15px;line-height:1.24;
  color:#1D4ED8;font-variant-numeric:tabular-nums;letter-spacing:-.006em}
.f span{margin-left:auto;font-size:11.5px;color:#94A3B8;white-space:nowrap;
  font-variant-numeric:tabular-nums}

.foot{flex:none;display:flex;justify-content:space-between;align-items:flex-end;gap:28px;
  margin-top:12px;padding-top:12px;border-top:3px solid #0F172A;
  font-size:13px;line-height:1.42;color:#475569}
.foot b{font-family:'Archivo';font-weight:900;color:#1D4ED8;font-size:17px;
  letter-spacing:-.01em;white-space:nowrap}
.foot em{font-style:normal;color:#94A3B8}
"""


def build() -> str:
    total = sum(len(g[3]) for g in GROUPS)
    blocks = []
    for area, title, domain, rows in GROUPS:
        entries = "".join(
            f'<div class="f"><code>{f}</code><span>{ka}</span></div>' for f, ka in rows
        )
        blocks.append(
            f'<div class="grp"><h2><i style="background:{AREA_COLOUR[area]}"></i>{title}'
            f'<span>{domain}</span></h2>{entries}</div>'
        )

    return f"""<!doctype html><html><head><meta charset="utf-8"><style>{CSS}</style></head><body>
<div class="card">
  <div class="top">
    <img src="/backend/wwwroot/assets/logo.svg" alt="">
    <span class="w">PCI AI</span>
    <span class="o">Project Controls<br>Institute Global, Inc.</span>
    <span class="badge"><b>PCL-AI</b><span>Certification</span></span>
  </div>

  <h1>The complete PCL-AI formula sheet<span class="dot">.</span></h1>
  <div class="sub"><b>{total} formulas on one page.</b> Every one cited to the Knowledge Area that
    develops it — the accounting standards and the delivery mechanics a project is measured by, together.</div>

  <div class="legend">
    <span><i style="background:#1D4ED8"></i>A — Financial reporting &amp; accounting</span>
    <span><i style="background:#0F172A"></i>B — Project management, controls &amp; delivery</span>
    <span><i style="background:#C13329"></i>Where the two meet</span>
    <em>Sixty-one Knowledge Areas</em>
  </div>

  <div class="grid">{"".join(blocks)}</div>

  <div class="foot">
    <span>Cited to the PCL-AI Body of Knowledge — thirteen domains, sixty-one Knowledge Areas.
      Standards named at principle level, never reproduced.<br>
      <em>One of three credentials in the PCI AI Project Leadership Certification Suite —
      PCL-AI · PFL-AI · PML-AI</em></span>
    <b>projectcontrolsinstitute.org</b>
  </div>
</div></body></html>
"""


if __name__ == "__main__":
    out = pathlib.Path(__file__).resolve().parent / "pcl-ai-one-page.html"
    out.write_text(build(), encoding="utf-8")
    print(f"wrote {out.name} · {sum(len(g[3]) for g in GROUPS)} formulas in {len(GROUPS)} groups")
