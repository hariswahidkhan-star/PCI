#!/usr/bin/env python3
"""Generate the one-page 'AI in Project Controls' sheet for PCL-AI.

Content is Domain 13 of the PCL-AI Body of Knowledge, not general commentary. The
sheet is built around the two things that domain has and the market does not: the
governed workflow shape from KA 13.5.1, and KA 13.5's systematic application of it
across every other domain.

Writes ai-in-project-controls.html next to this file; render with build_social.py.
"""
import pathlib

# KA 13.5.2–13.5.10 — the AI step and the verification step, domain by domain.
APPLICATION = [
    ("Estimating &amp; budgeting", "D3",
     "Parametric model proposes a rate, a range and scenarios",
     "Are the analogues genuinely comparable? Class and range stated?"),
    ("Forecasting &amp; EVM", "D3 · D6",
     "Model projects EAC, an early-warning signal and drivers",
     "Does the EAC method match the variance cause? TCPI sane?"),
    ("Cost control", "D1 · D5",
     "Extraction, auto-coding, reconciliation, anomaly detection",
     "Recompute from source. Accruals booked before any index"),
    ("Scheduling", "D10",
     "Logic screening — open ends, negative lag, constraint abuse",
     "Hidden constraints. Is the float real or manufactured?"),
    ("Agile delivery", "D9",
     "Velocity and flow analysis, sprint forecasting",
     "Point inflation. Rolling average, never best-ever"),
    ("Contracts &amp; commercial", "D7",
     "Clause, obligation and claim extraction at volume",
     "Legal review of every extraction before it is relied on"),
    ("Reporting &amp; performance", "D4",
     "Draft commentary, exception detection, natural-language query",
     "Every figure traced. Fluency is not evidence"),
    ("Risk", "D12",
     "Risk identification from data, predictive scoring, simulation",
     "Register logic. EMV is a portfolio, not one event"),
    ("Financial reporting", "D1 · D2",
     "Disclosure drafting, consistency checks, standard mapping",
     "Named human sign-off. No standard applied unread"),
]

# KA 13.4.1 — the category map. Products change; categories do not.
CATEGORIES = [
    ("General LLM assistants", "Confidentiality · verify output"),
    ("Document / RAG &amp; knowledge", "Source access control · citations"),
    ("Spreadsheet &amp; data analysis", "Check every computation"),
    ("BI &amp; analytics AI", "Metric definitions must be consistent"),
    ("Scheduling &amp; PM-suite AI", "Hidden constraints · validate logic"),
    ("Risk, forecasting &amp; ML", "Data quality · explainability"),
    ("RPA &amp; process mining", "Scope of control-breach detection"),
    ("Contract analytics / CLM", "Legal review of extractions"),
    ("Transcription &amp; meetings", "Confidentiality · accuracy of actions"),
    ("AI coding &amp; automation", "Test and verify generated code"),
]

# KA 13.6.4 — the refusal conditions.
REFUSE = [
    "A <b>rule</b> does it better and more transparently",
    "<b>Confidentiality</b> cannot be assured",
    "The <b>stakes</b> demand certainty the model cannot give",
    "The <b>data</b> is inadequate — garbage in",
    "<b>Accountability</b> cannot be maintained",
]

# KA 13.6.5 — the assurance checklist applied to any AI-assisted output.
CHECKLIST = [
    "Source-checked — figures recomputed from source",
    "Method and assumption sound",
    "No hallucination — every claim traces to data",
    "Confidentiality — produced in a governed tool",
    "Cross-checked against schedule and prior period",
    "Signed off — named professional, assistance recorded",
]

# KA 13.7.1 — the maturity ladder.
MATURITY = [
    ("Ad-hoc", "individuals experimenting, no governance"),
    ("Piloting", "defined use cases, some guardrails"),
    ("Standardised", "approved tools, policy, verification embedded"),
    ("Integrated", "AI inside the controls workflow and tooling"),
    ("Governed", "measured value, full audit trail"),
]

# KA 13.1.5 — honest capabilities, and the limits every workflow is designed around.
STRONG = ["Extraction", "Drafting", "Classification",
          "Pattern detection", "Forecasting", "Summarisation"]
LIMITS = [
    ("Hallucination", "confident, false, fabricated figures and citations"),
    ("Data dependence", "garbage in, garbage out"),
    ("Bias", "reproduces what the training data carried"),
    ("No reasoning guarantee", "plausible is not correct"),
    ("No accountability", "a model cannot own a decision"),
    ("Knowledge cutoff", "does not know what it was not given"),
]

# KA 13.2.2 — the dimensions a controls function already manages.
QUALITY = [
    ("Accuracy", "Does it reflect reality?"),
    ("Completeness", "Anything missing — omitted accruals?"),
    ("Consistency", "Do controls and the ledger agree?"),
    ("Timeliness", "Current enough for the decision?"),
    ("Validity", "Valid codes, conforming format?"),
    ("Uniqueness", "Free of duplicates?"),
]

# KA 13.3.1–13.3.2 — the anatomy of a professional prompt, and the reusable patterns.
PROMPT = [
    ("Role &amp; context", "who the model is acting as, and the situation"),
    ("A clear task", "one instruction, unambiguous"),
    ("The input", "the data itself, or a governed RAG reference"),
    ("Desired format", "table, narrative, length"),
    ("Constraints", "tone, exclusions, what not to infer"),
]
PATTERNS = ["Extraction", "Analysis", "Drafting", "Summarisation", "Transformation"]

CSS = """
@font-face{font-family:'Archivo';src:url('/backend/wwwroot/assets/fonts/archivo-latin.woff2') format('woff2');font-weight:700 900}
@font-face{font-family:'Inter';src:url('/backend/wwwroot/assets/fonts/inter-latin.woff2') format('woff2');font-weight:400 700}
*{margin:0;padding:0;box-sizing:border-box}
html,body{width:100%;height:100%;background:#fff}
body{font-family:'Inter',sans-serif;color:#0F172A;overflow:hidden}

.card{position:relative;width:100vw;height:100vh;padding:38px 46px 24px;
  background:#fff;display:flex;flex-direction:column}
.card::after{content:"";position:absolute;top:0;left:0;right:0;height:11px;background:#1D4ED8}

.top{display:flex;align-items:center;margin-bottom:16px}
.top img{width:64px;height:64px}
.top .w{font-family:'Archivo';font-weight:900;font-size:34px;letter-spacing:-.02em;
  margin-left:15px;color:#1D4ED8}
.top .o{font-size:15px;line-height:1.25;margin-left:15px;padding-left:15px;
  border-left:2px solid #E3E8EF;color:#475569}
.top .badge{margin-left:auto;display:flex;align-items:center;gap:11px;
  border:2.5px solid #1D4ED8;border-radius:999px;padding:8px 18px 8px 15px}
.top .badge b{font-family:'Archivo';font-weight:900;font-size:22px;color:#1D4ED8}
.top .badge span{font-size:13px;letter-spacing:.13em;text-transform:uppercase;
  color:#64748B;border-left:2px solid #E3E8EF;padding-left:11px}

h1{font-family:'Archivo';font-weight:900;font-size:50px;line-height:1;
  letter-spacing:-.036em;margin-bottom:8px;white-space:nowrap}
.dot{color:#C13329}
.sub{font-size:15.5px;color:#475569;margin-bottom:14px}
.sub b{color:#0F172A;font-weight:700}

/* the governed workflow — the spine of the whole domain */
.pipe{display:flex;align-items:stretch;gap:0;margin-bottom:8px;
  border:2.5px solid #0F172A;border-radius:9px;overflow:hidden}
.st{flex:1;padding:11px 14px;border-right:2px solid #E3E8EF}
.st:last-child{border-right:0}
.st.ai{background:#EEF3FD}
.st.hu{background:#FBF0EF}
.st k{display:block;font-size:10.5px;font-weight:700;letter-spacing:.15em;
  text-transform:uppercase;color:#94A3B8;margin-bottom:4px}
.st.ai k{color:#1D4ED8}
.st.hu k{color:#C13329}
.st b{font-family:'Archivo';font-weight:900;font-size:16px;letter-spacing:-.015em;
  display:block;line-height:1.14}
.st i{font-style:normal;font-size:11.5px;color:#64748B;display:block;margin-top:2px}
.pipenote{font-size:12.5px;color:#94A3B8;margin-bottom:13px;text-align:right}
.pipenote b{color:#0F172A;font-weight:700}

.grid{flex:1 1 auto;min-height:0;overflow:hidden;display:flex;gap:24px}
.col{flex:1;min-width:0}
.col.wide{flex:1.42}

h2{font-family:'Archivo';font-weight:900;font-size:14.5px;letter-spacing:.01em;
  text-transform:uppercase;color:#0F172A;display:flex;align-items:center;
  padding:0 0 5px 0;margin:0 0 6px 0;border-bottom:2.5px solid #0F172A}
h2 i{display:inline-block;width:11px;height:11px;border-radius:2px;
  margin-right:8px;font-style:normal;flex:none}
h2 span{margin-left:auto;font-family:'Inter';font-weight:700;font-size:11px;
  letter-spacing:.06em;color:#94A3B8;white-space:nowrap;padding-left:6px}
.blk{margin-bottom:12px}

/* the application map */
.ap{padding:4.6px 0;border-bottom:1px solid #EEF2F7}
.ap:last-child{border-bottom:0}
.ap .hd{display:flex;align-items:baseline;gap:8px}
.ap .hd b{font-family:'Archivo';font-weight:900;font-size:14px;color:#0F172A;
  letter-spacing:-.01em}
.ap .hd span{margin-left:auto;font-size:10.5px;color:#94A3B8;font-weight:700;white-space:nowrap}
.ap .ai{font-size:12.3px;color:#1D4ED8;font-weight:700;line-height:1.26;margin-top:1px}
.ap .hu{font-size:12px;color:#64748B;line-height:1.26;margin-top:1px}
.ap .hu::before{content:"✓ ";color:#C13329;font-weight:700}

/* compact two-part rows */
.r{display:flex;align-items:baseline;gap:8px;padding:3.4px 0;border-bottom:1px solid #F3F6FA}
.r:last-child{border-bottom:0}
.r b{font-size:12.6px;font-weight:700;color:#0F172A;line-height:1.24}
.r span{margin-left:auto;font-size:10.8px;color:#94A3B8;text-align:right;
  line-height:1.22;max-width:52%}

/* simple listed rows */
.l{font-size:12.6px;color:#334155;line-height:1.3;padding:3.4px 0;
  border-bottom:1px solid #F3F6FA;padding-left:17px;text-indent:-17px}
.l:last-child{border-bottom:0}
.l b{color:#0F172A;font-weight:700}
.l::before{content:"—";color:#C13329;font-weight:700;margin-right:7px}
.l.tick::before{content:"□";color:#1D4ED8;font-weight:700;margin-right:8px}

.chips{display:flex;flex-wrap:wrap;gap:5px;margin:2px 0 4px}
.chips span{font-size:11.6px;font-weight:700;color:#1D4ED8;background:#EEF3FD;
  border-radius:4px;padding:3px 8px}

.lim{display:flex;align-items:baseline;gap:8px;padding:3.4px 0;border-bottom:1px solid #F3F6FA}
.lim:last-child{border-bottom:0}
.lim b{font-size:12.6px;font-weight:700;color:#C13329;line-height:1.24}
.lim span{margin-left:auto;font-size:10.8px;color:#94A3B8;text-align:right;
  line-height:1.22;max-width:56%}

.mat{display:flex;align-items:baseline;gap:8px;padding:3.2px 0;border-bottom:1px solid #F3F6FA}
.mat:last-child{border-bottom:0}
.mat em{font-style:normal;font-family:'Archivo';font-weight:900;font-size:12px;
  color:#1D4ED8;width:15px;flex:none}
.mat b{font-size:12.6px;font-weight:700;color:#0F172A}
.mat span{margin-left:auto;font-size:10.8px;color:#94A3B8;text-align:right;max-width:56%;line-height:1.22}

.foot{flex:none;display:flex;justify-content:space-between;align-items:flex-end;gap:26px;
  margin-top:11px;padding-top:11px;border-top:3px solid #0F172A;
  font-size:12.5px;line-height:1.42;color:#475569}
.foot b{font-family:'Archivo';font-weight:900;color:#1D4ED8;font-size:16.5px;
  letter-spacing:-.01em;white-space:nowrap}
.foot em{font-style:normal;color:#94A3B8}
"""


def build() -> str:
    apps = "".join(
        f'<div class="ap"><div class="hd"><b>{name}</b><span>{dom}</span></div>'
        f'<div class="ai">{ai}</div><div class="hu">{hu}</div></div>'
        for name, dom, ai, hu in APPLICATION
    )
    cats = "".join(f'<div class="r"><b>{n}</b><span>{g}</span></div>' for n, g in CATEGORIES)
    refuse = "".join(f'<div class="l">{r}</div>' for r in REFUSE)
    checks = "".join(f'<div class="l tick">{c}</div>' for c in CHECKLIST)
    prompt = "".join(f'<div class="r"><b>{n}</b><span>{d}</span></div>' for n, d in PROMPT)
    patterns = "".join(f"<span>{x}</span>" for x in PATTERNS)
    quality = "".join(f'<div class="r"><b>{d}</b><span>{q}</span></div>' for d, q in QUALITY)
    strong = "".join(f"<span>{x}</span>" for x in STRONG)
    limits = "".join(f'<div class="lim"><b>{n}</b><span>{d}</span></div>' for n, d in LIMITS)
    mat = "".join(
        f'<div class="mat"><em>{i}</em><b>{n}</b><span>{d}</span></div>'
        for i, (n, d) in enumerate(MATURITY, 1)
    )

    return f"""<!doctype html><html><head><meta charset="utf-8"><style>{CSS}</style></head><body>
<div class="card">
  <div class="top">
    <img src="/backend/wwwroot/assets/logo.svg" alt="">
    <span class="w">PCI AI</span>
    <span class="o">Project Controls<br>Institute Global, Inc.</span>
    <span class="badge"><b>PCL-AI</b><span>Certification</span></span>
  </div>

  <h1>AI in project controls, governed<span class="dot">.</span></h1>
  <div class="sub"><b>Domain 13 of the PCL-AI Body of Knowledge — 20% of the credential.</b>
    Not what AI might do one day. What it does now, where it applies, what you verify, and when a
    professional refuses to use it.</div>

  <div class="pipe">
    <div class="st"><k>1 · Input</k><b>Governed data</b><i>Quality, lineage, confidentiality</i></div>
    <div class="st ai"><k>2 · AI proposes</k><b>Draft · extract · forecast · detect</b><i>The step that accelerates</i></div>
    <div class="st hu"><k>3 · You verify</k><b>Check method, figures, assumptions</b><i>The step that assures</i></div>
    <div class="st"><k>4 · Output</k><b>A result you own</b><i>Signed, with the trail recorded</i></div>
  </div>
  <div class="pipenote">Every application below is this one shape — <b>KA 13.5.1</b></div>

  <div class="grid">
    <div class="col wide">
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>Where it applies, domain by domain
          <span>KA 13.5</span></h2>
        {apps}
      </div>
      <div class="blk">
        <h2><i style="background:#0F172A"></i>What a professional prompt contains<span>KA 13.3</span></h2>
        {prompt}
        <div class="chips" style="margin-top:6px">{patterns}</div>
      </div>
    </div>

    <div class="col">
      <div class="blk">
        <h2><i style="background:#0F172A"></i>Tool categories<span>KA 13.4</span></h2>
        {cats}
      </div>
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>The maturity ladder<span>KA 13.7</span></h2>
        {mat}
      </div>
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>Data — the fuel<span>KA 13.2</span></h2>
        {quality}
      </div>
    </div>

    <div class="col">
      <div class="blk">
        <h2><i style="background:#C13329"></i>When&nbsp;<em style="font-style:normal;text-decoration:underline">not</em>&nbsp;to use AI
          <span>KA 13.6.4</span></h2>
        {refuse}
      </div>
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>Output assurance checklist<span>KA 13.6.5</span></h2>
        {checks}
      </div>
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>Genuinely strong at<span>KA 13.1.5</span></h2>
        <div class="chips">{strong}</div>
        <h2 style="margin-top:10px"><i style="background:#C13329"></i>Hard limits<span>KA 13.1.5</span></h2>
        {limits}
      </div>
    </div>
  </div>

  <div class="foot">
    <span><b style="font-family:Inter;font-size:12.5px;color:#0F172A">AI proposes. The professional disposes.</b>
      The tool accelerates, the verification assures, the professional owns the result — and remains accountable for it.<br>
      <em>One of three credentials in the PCI AI Project Leadership Certification Suite — PCL-AI · PFL-AI · PML-AI</em></span>
    <b>projectcontrolsinstitute.org</b>
  </div>
</div></body></html>
"""


if __name__ == "__main__":
    out = pathlib.Path(__file__).resolve().parent / "ai-in-project-controls.html"
    out.write_text(build(), encoding="utf-8")
    print(f"wrote {out.name} · {len(APPLICATION)} applications, {len(CATEGORIES)} categories")
