#!/usr/bin/env python3
"""Generate the Honorary Fellow (PCI) one-page graphic.

Every claim on this sheet is taken from what the Institute has already published —
backend/wwwroot/route-honorary.html and honorary-application.html — and nothing is
softened in the retelling. The eligibility bar, the fee position, the boundary against
the examined credentials and the board's discretion are all on the page because they
are all on the form. An honorary distinction that hides its own terms is the kind
people discount on sight.

Writes honorary-fellow.html next to this file; render with build_social.py.
"""
import pathlib

# The eligibility bar, verbatim in substance from route-honorary.html: all three, not any.
GATES = [
    ("8", "years", "Professional experience in project controls, cost control,"
                   " finance, project management or a closely related field"),
    ("3", "of them managerial", "Leading teams, functions, programmes or budgets"
                               " in one or more of those fields"),
    # The first two gates are quantities; this one is a judgement, and the layout
    # says so rather than inventing a number for it.
    ("Merit", "a distinguished record", "Sustained contribution, leadership or"
                                        " service that merits recognition"),
]

# What the board actually reads. Required items are marked; the rest are optional.
SUBMISSION = [
    ("R&eacute;sum&eacute; / CV", "required", "The evidence the record rests on"),
    ("Relevant experience", "required", "In your own words, not a job description"),
    ("Professional summary", "required", "The contribution, stated plainly"),
    ("Suitability declaration", "required", "Reviewed confidentially and in context"),
    ("Academic qualifications", "", "Highest first, with institution and year"),
    ("Professional certifications", "", "Issuing body and year &mdash; PMP, CCP, PSP"),
    ("Career history", "", "The roles most relevant, with the years held"),
]

# The four stages, from the published route and the terms.
PROCESS = [
    ("Apply", "A reference is issued and emailed to you"),
    ("Board review", "15&ndash;30 days &mdash; an estimate, not a commitment"),
    ("If shortlisted", "One government ID and a photograph, by secure one-time link"),
    ("Conferral", "An award number, publicly verifiable on the register"),
]

# The boundary. This is the whole integrity of the thing, so it is stated twice as
# hard as the invitation.
IS = [
    "A board-conferred recognition of distinguished contribution",
    "Free to apply for, and free to receive",
    "Assessed on merit by independent review",
]
IS_NOT = [
    "An examined PCI certification",
    "Accreditation, registration or a licence",
    "A statement that the holder has been examined",
    "Something meeting the criteria entitles you to",
]

# From the published routes table. Honorary is the row that does not lead to a
# credential, and putting the other two beside it is the honest way to say so.
ROUTES = [
    ("Standard", "Paid", "Yes", "Your PCI credential", "earned"),
    ("Founding", "Free", "Yes", "Your PCI credential", "earned"),
    ("Honorary", "Free", "No", "Honorary Fellow (PCI)", "conferred"),
]

# Data minimisation, from the programme terms. Worth stating plainly: most
# recognition schemes ask for identity documents on day one.
DOCUMENTS = [
    "Identity documents are requested <b>only if you are shortlisted</b>",
    "No ID number or document detail is recorded &mdash; the image only",
    "Access is restricted to the board, on a need-to-know basis",
    "Deleted after the decision, or sooner if you ask",
]

# What conferral actually carries, stated without inflation.
OUTCOME = [
    ("An award number", "publicly verifiable on the register"),
    ("Portal access", "learning resources and study materials"),
    ("No examined credential", "those are earned by examination"),
]

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
.top .badge b{font-family:'Archivo';font-weight:900;font-size:20px;color:#1D4ED8}
.top .badge span{font-size:12.5px;letter-spacing:.13em;text-transform:uppercase;
  color:#64748B;border-left:2px solid #E3E8EF;padding-left:11px}

h1{font-family:'Archivo';font-weight:900;font-size:60px;line-height:1;
  letter-spacing:-.038em;margin-bottom:9px;white-space:nowrap}
.dot{color:#C13329}
.sub{font-size:15.5px;color:#475569;margin-bottom:15px;line-height:1.42}
.sub b{color:#0F172A;font-weight:700}

/* ---------- HERO: the eligibility bar, all three required ---------- */
.gates{display:flex;align-items:stretch;margin-bottom:7px}
.g{flex:1;position:relative;padding:15px 19px 15px;border-radius:10px;
  margin-right:26px;border:2.5px solid #0F172A;display:flex;flex-direction:column}
.g:last-child{margin-right:0}
/* a plus sign between gates: these are cumulative, not alternatives */
.g:not(:last-child)::after{content:"+";position:absolute;right:-20px;top:50%;
  margin-top:-17px;font-family:'Archivo';font-weight:900;font-size:27px;color:#0F172A;
  width:28px;text-align:center}
.g.blue{background:#1D4ED8;border-color:#1D4ED8}
.g .n{font-family:'Archivo';font-weight:900;font-size:52px;line-height:.9;
  letter-spacing:-.045em;color:#1D4ED8}
/* the qualitative gate carries a word, set smaller so it never reads as a numeral */
.g .n.word{font-size:40px;letter-spacing:-.03em}
.g.blue .n{color:#fff}
.g k{display:block;font-size:12px;font-weight:700;letter-spacing:.15em;
  text-transform:uppercase;color:#0F172A;margin:5px 0 6px}
.g.blue k{color:rgba(255,255,255,.9)}
.g i{font-style:normal;font-size:12.6px;color:#64748B;line-height:1.3;margin-top:auto}
.g.blue i{color:rgba(255,255,255,.85)}
.gatenote{font-size:13px;color:#64748B;margin-bottom:13px;text-align:right}
.gatenote b{color:#C13329;font-weight:700}

.grid{flex:1 1 auto;min-height:0;overflow:hidden;display:flex;gap:24px}
.col{flex:1;min-width:0}
.col.wide{flex:1.12}

h2{font-family:'Archivo';font-weight:900;font-size:16px;letter-spacing:.01em;
  text-transform:uppercase;color:#0F172A;display:flex;align-items:center;
  padding:0 0 5px 0;margin:0 0 7px 0;border-bottom:2.5px solid #0F172A}
h2 i{display:inline-block;width:11px;height:11px;border-radius:2px;
  margin-right:8px;font-style:normal;flex:none}
h2 span{margin-left:auto;font-family:'Inter';font-weight:700;font-size:11px;
  letter-spacing:.06em;color:#94A3B8;white-space:nowrap;padding-left:6px}
.blk{margin-bottom:13px}

/* the submission list */
.s{padding:5.4px 0;border-bottom:1px solid #F1F5F9}
.s:last-child{border-bottom:0}
.s .hd{display:flex;align-items:baseline;gap:8px}
.s .hd b{font-size:15px;font-weight:700;color:#0F172A}
.s .hd em{font-style:normal;margin-left:auto;font-size:10px;font-weight:700;
  letter-spacing:.09em;text-transform:uppercase;color:#fff;background:#C13329;
  border-radius:3px;padding:2px 6px;flex:none}
.s p{font-size:13px;color:#64748B;line-height:1.28;margin-top:1px}

/* the process */
.p{display:flex;gap:11px;align-items:baseline;padding:5.2px 0;
  border-bottom:1px solid #F1F5F9}
.p:last-child{border-bottom:0}
.p em{font-style:normal;flex:none;width:22px;height:22px;line-height:22px;
  text-align:center;border-radius:5px;background:#1D4ED8;color:#fff;
  font-family:'Archivo';font-weight:900;font-size:13px}
.p b{font-size:14.6px;font-weight:700;color:#0F172A;flex:none}
.p span{margin-left:auto;font-size:12.4px;color:#64748B;text-align:right;
  line-height:1.25;max-width:62%}

/* the boundary */
.b{font-size:14.2px;line-height:1.3;padding:4.6px 0;border-bottom:1px solid #F1F5F9;
  padding-left:20px;text-indent:-20px;color:#334155}
.b:last-child{border-bottom:0}
.b::before{content:"✓";color:#1D4ED8;font-weight:700;margin-right:9px}
.b.no::before{content:"✕";color:#C13329}

/* the routes table — honorary located against the two examined routes */
.rt{width:100%;border-collapse:collapse;font-size:13.4px}
.rt th{text-align:left;font-size:10.5px;font-weight:700;letter-spacing:.1em;
  text-transform:uppercase;color:#94A3B8;padding:0 8px 5px 0;border-bottom:1.5px solid #E3E8EF}
.rt td{padding:6px 8px 6px 0;border-bottom:1px solid #F1F5F9;color:#475569;
  vertical-align:baseline}
.rt tr:last-child td{border-bottom:0}
.rt td:first-child{font-weight:700;color:#0F172A}
.rt .hon td{background:#F5F8FE}
.rt .hon td:first-child{color:#1D4ED8}
.rt em{font-style:normal;display:block;font-size:11px;color:#94A3B8}
.rt .no{color:#C13329;font-weight:700}
.rt .yes{color:#0F172A;font-weight:700}

/* what conferral carries */
.o{display:flex;align-items:baseline;gap:8px;padding:4.4px 0;border-bottom:1px solid #F1F5F9}
.o:last-child{border-bottom:0}
.o b{font-size:13.8px;font-weight:700;color:#0F172A;flex:none}
.o span{margin-left:auto;font-size:12.2px;color:#64748B;text-align:right;
  line-height:1.24;max-width:62%}

.foot{flex:none;display:flex;justify-content:space-between;align-items:flex-end;gap:26px;
  margin-top:11px;padding-top:11px;border-top:3px solid #0F172A;
  font-size:12.5px;line-height:1.42;color:#475569}
.foot b{font-family:'Archivo';font-weight:900;color:#1D4ED8;font-size:16.5px;
  letter-spacing:-.01em;white-space:nowrap}
.foot em{font-style:normal;color:#94A3B8}

/* ---------- the apply strip ---------- */
.band{flex:none;background:#0F172A;border-radius:10px;padding:16px 24px;
  margin-top:12px;display:flex;align-items:center;gap:24px}
.band b{font-family:'Archivo';font-weight:900;font-size:28px;letter-spacing:-.026em;
  color:#fff;line-height:1.05;white-space:nowrap}
.band b em{font-style:normal;color:#F0776C}
.band span{font-size:12.8px;color:rgba(255,255,255,.72);line-height:1.36}
.band .cta{margin-left:auto;flex:none;text-align:right}
.band .cta k{display:block;font-size:10.5px;font-weight:700;letter-spacing:.16em;
  text-transform:uppercase;color:rgba(255,255,255,.55);margin-bottom:4px}
.band .cta u{text-decoration:none;font-family:'Archivo';font-weight:900;font-size:22px;
  color:#fff;letter-spacing:-.015em;white-space:nowrap}
"""


def build() -> str:
    gates = "".join(
        f'<div class="g{" blue" if i == 1 else ""}">'
        f'<div class="n{"" if n.isdigit() else " word"}">{n}</div>'
        f'<k>{label}</k><i>{detail}</i></div>'
        for i, (n, label, detail) in enumerate(GATES)
    )
    submission = "".join(
        f'<div class="s"><div class="hd"><b>{name}</b>'
        f'{f"<em>{tag}</em>" if tag else ""}</div><p>{note}</p></div>'
        for name, tag, note in SUBMISSION
    )
    process = "".join(
        f'<div class="p"><em>{i}</em><b>{name}</b><span>{note}</span></div>'
        for i, (name, note) in enumerate(PROCESS, 1)
    )
    is_ = "".join(f'<div class="b">{x}</div>' for x in IS)
    is_not = "".join(f'<div class="b no">{x}</div>' for x in IS_NOT)
    def route_row(name, fee, exam, result, how):
        honorary = exam == "No"          # the honorary row is the one with no exam
        tr = ' class="hon"' if honorary else ""
        cell = "no" if honorary else "yes"
        return (f'<tr{tr}><td>{name}</td><td>{fee}</td>'
                f'<td class="{cell}">{exam}</td>'
                f'<td>{result}<em>{how}</em></td></tr>')

    routes = "".join(route_row(*r) for r in ROUTES)
    documents = "".join(f'<div class="b">{d}</div>' for d in DOCUMENTS)
    outcome = "".join(
        f'<div class="o"><b>{n}</b><span>{d}</span></div>' for n, d in OUTCOME
    )

    return f"""<!doctype html><html><head><meta charset="utf-8"><style>{CSS}</style></head><body>
<div class="card">
  <div class="top">
    <img src="/backend/wwwroot/assets/logo.svg" alt="">
    <span class="w">PCI AI</span>
    <span class="o">Project Controls<br>Institute Global, Inc.</span>
    <span class="badge"><b>Honorary Fellow</b><span>PCI</span></span>
  </div>

  <h1>Conferred, never purchased<span class="dot">.</span></h1>
  <div class="sub"><b>Honorary Fellow (PCI) recognises a distinguished record in the
    profession.</b> There is no examination and no fee. There is also no shortcut:
    it is separate from the examined credentials, and it is not given to everyone who
    qualifies to be considered.</div>

  <div class="gates">{gates}</div>
  <div class="gatenote">All three are required &mdash; <b>not any one of them</b></div>

  <div class="grid">
    <div class="col wide">
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>What the board reads<span>PDF · JPG · PNG · 3 MB each</span></h2>
        {submission}
      </div>
      <div class="blk">
        <h2><i style="background:#0F172A"></i>Where it sits<span>The three routes</span></h2>
        <table class="rt"><thead><tr><th>Route</th><th>Fee</th><th>Exam</th><th>Result</th></tr></thead>
        <tbody>{routes}</tbody></table>
      </div>
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>What conferral carries</h2>
        {outcome}
      </div>
    </div>

    <div class="col">
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>How it is decided</h2>
        {process}
      </div>
      <div class="blk">
        <h2><i style="background:#1D4ED8"></i>What it is</h2>
        {is_}
      </div>
      <div class="blk">
        <h2><i style="background:#C13329"></i>What it is not</h2>
        {is_not}
      </div>
      <div class="blk">
        <h2><i style="background:#0F172A"></i>Your documents<span>Data minimisation</span></h2>
        {documents}
      </div>
    </div>
  </div>

  <div class="band">
    <b>Merit decides.<br><em>Payment never does.</em></b>
    <span>No nomination fee, no assessment fee, no credential fee. Meeting the criteria
      does not entitle an applicant to recognition &mdash; conferral is at the board's
      sole discretion, and PCI may decline without stating reasons.</span>
    <span class="cta"><k>Apply</k><u>Admin@pciai.org</u></span>
  </div>

  <div class="foot">
    <span>Honorary Fellow (PCI) is separate from the examined credentials.
      <em>PCL-AI, PFL-AI and PML-AI are always earned by passing the examination, on every route.</em></span>
    <b>projectcontrolsinstitute.org</b>
  </div>
</div></body></html>
"""


if __name__ == "__main__":
    out = pathlib.Path(__file__).resolve().parent / "honorary-fellow.html"
    out.write_text(build(), encoding="utf-8")
    print(f"wrote {out.name} · {len(SUBMISSION)} submission items, {len(PROCESS)} stages")
