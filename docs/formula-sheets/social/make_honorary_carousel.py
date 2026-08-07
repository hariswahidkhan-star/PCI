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

Slide furniture, type scale and archetypes come from carousel_kit, which every deck
shares so they read as one institution. This file supplies only content and order.

Usage:
    python3 -m http.server 8899 &
    python3 make_honorary_carousel.py
"""
import pathlib

import build_social as bs
import carousel_kit as kit

HERE = pathlib.Path(__file__).resolve().parent
WORK = HERE / "carousel-honorary"
OUT = bs.ROOT / "backend" / "wwwroot" / "assets" / "social" / "honorary-carousel"

FOOT = kit.footer("Honorary Fellow (PCI) &middot; board-conferred recognition")


def slide(n, body, tint=False, show_foot=True):
    return kit.slide(n, body, FOOT, tint=tint, show_foot=show_foot)


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
""", tint=True)

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
""", tint=True)

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
    <p>Or apply through the form at<br><b>projectcontrolsinstitute.org</b></p></div>
""", show_foot=False)


def main() -> None:
    kit.render_deck(SLIDES, WORK, OUT, "honorary-fellow-carousel.pdf")


if __name__ == "__main__":
    main()
