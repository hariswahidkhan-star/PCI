#!/usr/bin/env python3
"""Build the seven-slide Honorary Fellow (PCI) carousel.

Why this exists alongside the one-page sheet: they are different artefacts for
different moments. The one-pager is a reference someone saves and opens full-screen.
A carousel is read in a feed, on a phone, at roughly a third of its rendered width —
so the governing constraint is not how much fits, it is how little can be there and
still be worth a swipe. Body copy is set at ~34px on a 1080px slide so it lands near
12px on a phone; a slide carrying more than about sixty words has already failed.

One idea per slide, in the order an unconvinced reader needs them: the problem, the
fee, the bar, the discretion, the boundary, the process.

Renders through build_social so the carousel gets the same clipping and paint checks
as everything else, then assembles a PDF — LinkedIn's carousel format is a document
post, not seven images.

Usage:
    python3 -m http.server 8899 &
    python3 make_honorary_carousel.py
"""
import pathlib

from PIL import Image

import build_social as bs

HERE = pathlib.Path(__file__).resolve().parent
WORK = HERE / "carousel-honorary"
OUT = bs.ROOT / "backend" / "wwwroot" / "assets" / "social" / "honorary-carousel"

CSS = """
@font-face{font-family:'Archivo';src:url('/backend/wwwroot/assets/fonts/archivo-latin.woff2') format('woff2');font-weight:700 900}
@font-face{font-family:'Inter';src:url('/backend/wwwroot/assets/fonts/inter-latin.woff2') format('woff2');font-weight:400 700}
*{margin:0;padding:0;box-sizing:border-box}
html,body{width:100%;height:100%}
body{font-family:'Inter',sans-serif;color:#0F172A;background:#fff;overflow:hidden}

.s{position:relative;width:100vw;height:100vh;padding:62px 68px 52px;
  display:flex;flex-direction:column;background:#fff}
.s::after{content:"";position:absolute;top:0;left:0;right:0;height:14px;background:#1D4ED8}
.s.ink{background:#0F172A;color:#fff}
.s.ink::after{background:#C13329}

/* ---------- furniture ---------- */
/* Lockup pinned top, footer pinned bottom, content centred in what is left. Letting
   two auto margins split the slack instead floats the content at a different height
   on every slide, which reads as carelessness when they are swiped in sequence. */
.body{flex:1;display:flex;flex-direction:column;justify-content:center;min-height:0}
.top{display:flex;align-items:center;gap:18px;margin-bottom:40px}
.top img{width:66px;height:66px;flex:none}
.top .w{font-family:'Archivo';font-weight:900;font-size:34px;letter-spacing:-.02em;color:#1D4ED8}
.s.ink .top .w{color:#fff}
.top .n{margin-left:auto;font-family:'Archivo';font-weight:900;font-size:26px;
  color:#CBD5E1;letter-spacing:.06em}
.s.ink .top .n{color:rgba(255,255,255,.45)}

.eyebrow{font-size:25px;font-weight:700;letter-spacing:.2em;text-transform:uppercase;
  color:#1D4ED8;margin-bottom:26px}
.s.ink .eyebrow{color:#F0776C}

h1{font-family:'Archivo';font-weight:900;font-size:96px;line-height:.98;
  letter-spacing:-.038em;margin-bottom:30px}
h1.sm{font-size:78px}
.dot{color:#C13329}
.s.ink .dot{color:#F0776C}

p{font-size:34px;line-height:1.45;color:#475569;max-width:22ch}
.s.ink p{color:rgba(255,255,255,.8)}
p.wide{max-width:none}
p b{color:#0F172A;font-weight:700}
.s.ink p b{color:#fff}
p + p{margin-top:22px}

.kicker{margin-top:34px;padding-top:28px;border-top:4px solid #0F172A;
  font-family:'Archivo';font-weight:900;font-size:42px;line-height:1.14;
  letter-spacing:-.02em;color:#0F172A;max-width:26ch}
.s.ink .kicker{border-color:rgba(255,255,255,.28);color:#fff}
.kicker em{font-style:normal;color:#C13329}
.s.ink .kicker em{color:#F0776C}

.foot{margin-top:36px;padding-top:26px;border-top:2px solid #E3E8EF;display:flex;
  justify-content:space-between;align-items:baseline;font-size:23px;color:#94A3B8}
.s.ink .foot{border-color:rgba(255,255,255,.2);color:rgba(255,255,255,.5)}
.foot b{font-weight:700;color:#1D4ED8}
.s.ink .foot b{color:#fff}

/* ---------- cover ---------- */
.badge{display:inline-flex;align-items:center;gap:16px;border:4px solid #1D4ED8;
  border-radius:999px;padding:12px 28px 12px 24px;margin-bottom:36px;align-self:flex-start}
.badge b{font-family:'Archivo';font-weight:900;font-size:30px;color:#1D4ED8}
.badge span{font-size:20px;letter-spacing:.16em;text-transform:uppercase;color:#64748B;
  border-left:3px solid #E3E8EF;padding-left:16px}

/* ---------- the three gates ---------- */
.gates{display:flex;gap:26px;margin-bottom:8px}
.g{flex:1;border:4px solid #0F172A;border-radius:14px;padding:26px 24px 24px;
  display:flex;flex-direction:column}
.g.blue{background:#1D4ED8;border-color:#1D4ED8}
/* fixed slot, bottom-aligned: the word gate is set smaller than the numeral gates, and
   without this their labels sit at three different heights across the row */
.g .n{font-family:'Archivo';font-weight:900;font-size:112px;line-height:.86;
  letter-spacing:-.05em;color:#1D4ED8;height:96px;display:flex;align-items:flex-end}
.g .n.word{font-size:66px;letter-spacing:-.035em}
.g.blue .n{color:#fff}
.g k{display:block;font-size:23px;font-weight:700;letter-spacing:.1em;
  text-transform:uppercase;margin:16px 0 14px;color:#0F172A}
.g.blue k{color:rgba(255,255,255,.92)}
.g i{font-style:normal;font-size:27px;line-height:1.28;color:#64748B;margin-top:auto}
.g.blue i{color:rgba(255,255,255,.85)}

/* ---------- big listed rows ---------- */
.row{display:flex;align-items:baseline;gap:22px;padding:24px 0;
  border-bottom:2px solid #E3E8EF}
.row:last-child{border-bottom:0}
.s.ink .row{border-color:rgba(255,255,255,.18)}
.row em{font-style:normal;flex:none;font-family:'Archivo';font-weight:900;font-size:34px;
  color:#C13329;width:44px}
.row.tick em{color:#1D4ED8}
.row b{font-size:38px;font-weight:700;letter-spacing:-.01em;color:#0F172A}
.s.ink .row b{color:#fff}
.row span{margin-left:auto;text-align:right;font-size:25px;color:#64748B;
  line-height:1.28;max-width:46%}
.s.ink .row span{color:rgba(255,255,255,.62)}

/* ---------- numbered process ---------- */
.step{display:flex;gap:24px;align-items:flex-start;padding:22px 0;
  border-bottom:2px solid #E3E8EF}
.step:last-child{border-bottom:0}
.step em{font-style:normal;flex:none;width:52px;height:52px;line-height:52px;
  text-align:center;border-radius:12px;background:#1D4ED8;color:#fff;
  font-family:'Archivo';font-weight:900;font-size:28px}
.step b{display:block;font-size:36px;font-weight:700;letter-spacing:-.01em;margin-bottom:4px}
.step span{display:block;font-size:25px;color:#64748B;line-height:1.3}

/* ---------- the apply strip ---------- */
/* the closing slide has no footer rule — the strip itself is the foot, so it sits low */
.cta{margin-top:auto;background:#1D4ED8;border-radius:14px;padding:30px 34px;
  display:flex;align-items:center;gap:26px}
.cta k{display:block;font-size:20px;font-weight:700;letter-spacing:.2em;
  text-transform:uppercase;color:rgba(255,255,255,.7);margin-bottom:8px}
.cta u{text-decoration:none;font-family:'Archivo';font-weight:900;font-size:44px;
  color:#fff;letter-spacing:-.02em}
.cta p{margin-left:auto;text-align:right;font-size:22px;color:rgba(255,255,255,.78);
  max-width:34%;line-height:1.3}
"""

LOCKUP = ('<div class="top"><img src="/backend/wwwroot/assets/logo.svg" alt="">'
          '<span class="w">PCI AI</span><span class="n">{n}</span></div>')

FOOT = ('<div class="foot"><span>Honorary Fellow (PCI) &middot; board-conferred recognition</span>'
        '<b>projectcontrolsinstitute.org</b></div>')


def slide(n: str, body: str, ink: bool = False, foot: bool = True) -> str:
    return (f'<!doctype html><html><head><meta charset="utf-8"><style>{CSS}</style></head>'
            f'<body><div class="s{" ink" if ink else ""}">{LOCKUP.format(n=n)}'
            f'<div class="body">{body}</div>{FOOT if foot else ""}</div></body></html>')


# --------------------------------------------------------------------------
# The seven. Each carries one idea; the word counts are deliberately small.
# Every claim traces to route-honorary.html or honorary-application.html.
# --------------------------------------------------------------------------

SLIDES = {}

SLIDES["01-cover"] = slide("", """
  <span class="badge"><b>Honorary Fellow</b><span>PCI</span></span>
  <h1>Conferred,<br>never<br>purchased<span class="dot">.</span></h1>
  <p class="wide"><b>A board-conferred recognition of distinguished contribution to
    project controls.</b> No examination. No fee. And no entitlement.</p>
""")

SLIDES["02-problem"] = slide("01", """
  <div class="eyebrow">Why this needs saying</div>
  <h1>Most honorary<br>titles are<br>bought<span class="dot">.</span></h1>
  <p class="wide">Not in those words. A nomination fee. An assessment fee. A credential
    fee. A ceremony ticket. And a review that has never once declined an applicant
    who paid.</p>
  <div class="kicker">The letters arrive.<br><em>The distinction does not.</em></div>
""", ink=True)

SLIDES["03-fee"] = slide("02", """
  <div class="eyebrow">What it costs to apply</div>
  <h1>Nothing<span class="dot">.</span></h1>
  <div class="row tick"><em>&mdash;</em><b>No nomination fee</b></div>
  <div class="row tick"><em>&mdash;</em><b>No assessment fee</b></div>
  <div class="row tick"><em>&mdash;</em><b>No credential fee</b></div>
  <div class="kicker">Payment does not influence<br>the decision, and buys<br>
    <em>no entitlement to it.</em></div>
""")

SLIDES["04-bar"] = slide("03", """
  <div class="eyebrow">Who it is for</div>
  <h1 class="sm">The bar, in<br>three parts<span class="dot">.</span></h1>
  <div class="gates">
    <div class="g"><div class="n">8</div><k>Years</k>
      <i>Project controls, cost, finance or project management</i></div>
    <div class="g blue"><div class="n">3</div><k>Managerial</k>
      <i>Leading teams, functions, programmes or budgets</i></div>
    <div class="g"><div class="n word">Merit</div><k>A record</k>
      <i>Sustained contribution to the profession</i></div>
  </div>
  <div class="kicker">All three. <em>Not any one of them.</em></div>
""")

SLIDES["05-discretion"] = slide("04", """
  <div class="eyebrow">What meeting the bar gets you</div>
  <h1>Nothing,<br>automatically<span class="dot">.</span></h1>
  <p class="wide">Conferral is at the board's sole discretion. We may ask for more
    information. We may decline without stating reasons.</p>
  <div class="kicker">A distinction granted to everyone
    who qualifies to be considered is not a distinction.
    <em>It is a mailing list.</em></div>
""", ink=True)

SLIDES["06-boundary"] = slide("05", """
  <div class="eyebrow">The line we will not blur</div>
  <h1 class="sm">It is not a<br>certification<span class="dot">.</span></h1>
  <div class="row"><em>&times;</em><b>An examined PCI credential</b></div>
  <div class="row"><em>&times;</em><b>Accreditation or a licence</b></div>
  <div class="row"><em>&times;</em><b>Evidence of competence</b></div>
  <div class="kicker">PCL-AI, PFL-AI and PML-AI are earned by examination, on every
    route. <em>Nobody is given one for a distinguished career</em> &mdash; including
    people who have one.</div>
""")

SLIDES["07-apply"] = slide("06", """
  <div class="eyebrow">How it works</div>
  <h1 class="sm">Four steps<span class="dot">.</span></h1>
  <div class="step"><em>1</em><div><b>Apply</b>
    <span>A r&eacute;sum&eacute;, your experience and a summary in your own words. A reference is issued.</span></div></div>
  <div class="step"><em>2</em><div><b>Board review</b>
    <span>Fifteen to thirty days &mdash; an estimate, not a commitment.</span></div></div>
  <div class="step"><em>3</em><div><b>If shortlisted</b>
    <span>One government ID and a photograph, by secure link. Deleted after the decision.</span></div></div>
  <div class="step"><em>4</em><div><b>Conferral</b>
    <span>An award number, publicly verifiable on the register.</span></div></div>
  <div class="cta"><span><k>Apply</k><u>Admin@pciai.org</u></span>
    <p>Or apply through the form at projectcontrolsinstitute.org</p></div>
""", foot=False)


def main() -> None:
    WORK.mkdir(parents=True, exist_ok=True)
    over = bs.viewport_overhead()
    print(f"viewport overhead: {over}px")

    pages = []
    for name, html in SLIDES.items():
        src = WORK / f"{name}.html"
        src.write_text(html, encoding="utf-8")
        pages.append(bs.build(src, OUT / f"{name}.png", over))

    # LinkedIn's carousel is a document post, so the deck ships as one PDF too.
    first, *rest = [Image.open(p).convert("RGB") for p in pages]
    pdf = OUT / "honorary-fellow-carousel.pdf"
    first.save(pdf, save_all=True, append_images=rest, resolution=200.0)
    print(f"OK  {pdf.relative_to(bs.ROOT)}  {len(pages)} slides")


if __name__ == "__main__":
    main()
