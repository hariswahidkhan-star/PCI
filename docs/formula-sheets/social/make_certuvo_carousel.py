#!/usr/bin/env python3
"""Build the seven-slide Certuvo announcement carousel.

The announcement is that Certuvo is PCI AI's official preparation platform. What
makes it worth posting is the half most announcements leave out: what an official
preparation relationship is not allowed to do.

That is not caution, it is the substance. PCI's published Impartiality Policy calls
the separation between preparation and certification "the single most important
safeguard of impartiality", and states that completing Certuvo's courses "confers no
advantage". A post that announced a preparation platform without saying that would
contradict the Institute's own governance and hand a complainant the citation.

Sources, all published: certuvo.html, impartiality-policy.html, training-partners.html,
and the site-wide status disclosure in the footer.

Usage:
    python3 -m http.server 8899 &
    python3 make_certuvo_carousel.py
"""
import pathlib

import build_social as bs
import carousel_kit as kit

HERE = pathlib.Path(__file__).resolve().parent
WORK = HERE / "carousel-certuvo"
OUT = bs.ROOT / "backend" / "wwwroot" / "assets" / "social" / "certuvo-carousel"

MARK = "/backend/wwwroot/assets/partners/certuvo-mark"
kit.PARTNER = MARK          # both marks ride the header bar on every slide

FOOT = kit.footer("Official preparation platform for PCI AI")

COBRAND = f'''<div class="cobrand">
  <span class="pci"><img src="/backend/wwwroot/assets/logo.svg" alt=""><b>PCI AI</b></span>
  <hr><img class="partner" src="{MARK}-white.png" alt="Certuvo">
</div>'''


def slide(n, body, ink=False, show_foot=True, show_top=True):
    return kit.slide(n, body, FOOT, ink=ink, show_foot=show_foot, show_top=show_top)


SLIDES = {}

SLIDES["01-cover"] = slide("", f"""
  {COBRAND}
  <div class="eyebrow">Official preparation platform</div>
  <h1>PCI sets the<br>standard.<br>Certuvo is where<br>you meet it<span class="dot">.</span></h1>
  <p class="wide"><b>For the PCI AI certifications</b><br>PCL-AI &middot; PFL-AI &middot; PML-AI</p>
""", ink=True, show_top=False)

SLIDES["02-what"] = slide("01", """
  <div class="eyebrow">What it delivers</div>
  <h1 class="sm">Study and<br>practice, fully<br>online<span class="dot">.</span></h1>
  <div class="row tick"><em>&mdash;</em><b>Study aligned to the body of knowledge</b></div>
  <div class="row tick"><em>&mdash;</em><b>Scenario banks that mirror the exam</b></div>
  <div class="row tick"><em>&mdash;</em><b>Full-length mock examinations</b></div>
  <div class="row tick"><em>&mdash;</em><b>Progress and readiness insight</b></div>
""")

SLIDES["03-scenario"] = slide("02", """
  <div class="eyebrow">Why the format matters</div>
  <h1 class="sm">The exam is<br>entirely<br>scenario-based<span class="dot">.</span></h1>
  <p class="wide">Every question puts you in a situation and asks for the best course
    of action. Certuvo's practice mirrors that format, so the examination feels
    familiar rather than surprising.</p>
  <div class="kicker">Recall is not the test.<br><em>Judgement is.</em></div>
""")

SLIDES["04-separation"] = slide("03", """
  <div class="eyebrow">And now the important half</div>
  <h1>What this<br>does not<br>mean<span class="dot">.</span></h1>
  <div class="row"><em>&times;</em><b>No role in setting the standard</b></div>
  <div class="row"><em>&times;</em><b>No role in setting the cut score</b></div>
  <div class="row"><em>&times;</em><b>No role in any certification decision</b></div>
""", ink=True)

SLIDES["05-no-advantage"] = slide("04", """
  <div class="eyebrow">Stated plainly</div>
  <h1 class="sm">Completing the<br>courses confers<br>no advantage<span class="dot">.</span></h1>
  <p class="wide">That sentence is from our published Impartiality Policy, not from this
    post. Preparation gets you ready. The examination is scored against the standard on
    demonstrated competence.</p>
  <div class="kicker">A pass is earned.<br><em>It is never bought.</em></div>
""")

SLIDES["06-optional"] = slide("05", """
  <div class="eyebrow">What candidates should know</div>
  <h1 class="sm">You do not<br>have to use it<span class="dot">.</span></h1>
  <p class="wide">Certuvo is the official preparation, not a requirement. Candidates may
    prepare however they wish &mdash; self-study, an employer programme, a recognised
    training partner, or nothing at all.</p>
  <div class="kicker">Eligibility and the examination
    are set by PCI. <em>Only by PCI.</em></div>
""")

SLIDES["07-why"] = slide("06", f"""
  <div class="eyebrow">Why we structure it this way</div>
  <h1 class="sm">A credential is<br>worth what it<br>refuses to sell<span class="dot">.</span></h1>
  <p class="wide">Separating preparation from certification is the single most important
    safeguard of impartiality. It is why the two are announced together with the limits
    attached, and why the limits are published rather than implied.</p>
  <div class="kicker">PCI sets the standard.<br>Certuvo prepares you for it.<br>
    <em>Neither does the other's job.</em></div>
  <div class="cta">
    <span><k>Prepare with</k><img class="ctamark" src="{MARK}-white.png" alt="Certuvo"></span>
    <p>Enrolled candidates practise inside<br>the PCI portal<br>
      <b>projectcontrolsinstitute.org</b></p></div>
""", ink=True, show_foot=False)


if __name__ == "__main__":
    kit.render_deck(SLIDES, WORK, OUT, "certuvo-official-platform-carousel.pdf")
