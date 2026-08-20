---
platform:      X / Threads
type:          thread
title:         A Monte Carlo run, from inputs to the P50/P80 split
meta:          One correlation setting moved P80 from £131.2m to £136.9m. Eight posts on running a cost simulation and reading the curve it gives you back.
primary_kw:    Monte Carlo cost simulation *
secondary_kw:  contingency at P80, cost risk analysis, correlation in risk models, PERT distribution
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    462
hashtags:      #RiskManagement #CostEngineering
ab_id:         AB-01047
---

# A Monte Carlo run, from inputs to the P50/P80 split

*X / Threads thread — 8 posts, each under 280 characters and each able to stand alone. The link sits in the final post. Character counts are for production; X counts any URL as 23 characters, so the live figures run lower.*

**Post 1/8 — the hook** (198 characters)
Same estimate. Same risk register. One setting changed, and P80 moved from £131.2m to £136.9m.

£5.7m of contingency turned on a correlation coefficient that nobody in the room had been asked about.

**Post 2/8 — what the method is** (234 characters)
A Monte Carlo cost simulation samples every uncertain cost line thousands of times, using a range and a distribution for each, and adds them up each time. The output is not a number. It is a curve of probability against out-turn cost.

**Post 3/8 — the arithmetic** (199 characters)
Ten thousand iterations on a £120.0m base estimate.

P50 £127.4m. P80 £136.9m.
Contingency at P80 = 136.9 − 120.0 = £16.9m, 14.1% of base.
At P50 it is £7.4m, which is a coin toss with a budget code.

**Post 4/8 — inputs decide everything** (209 characters)
A three-point range on every significant cost line, sourced from the estimate class, tender returns or historic out-turn. Ranges invented in the workshop give you a curve that is smooth, precise and worthless.

**Post 5/8 — correlation is the expensive default** (234 characters)
Left independent, the model lets overruns cancel each other out and the spread collapses. Set labour rates across packages to correlate at 0.4 and P80 goes from £131.2m to £136.9m. That £5.7m appears when you tell the model the truth.

**Post 6/8 — the distribution changes the mean** (214 characters)
One line, low 8.0, most likely 9.5, high 14.0.
Triangular mean = (8.0 + 9.5 + 14.0) ÷ 3 = £10.5m.
PERT mean = (8.0 + 4 × 9.5 + 14.0) ÷ 6 = £10.0m.
PERT weights the most likely four times, so it plays the tail down.

**Post 7/8 — what it cannot do** (223 characters)
It cannot see a risk that is not in the register, and it will report a confident number anyway. P80 is a statement of appetite, not a fact. Write down who chose it and why, because that is the question at the funding board.

**Post 8/8 — the run is not the end** (188 characters)
Then profile the drawdown. Contingency with no release plan is a slush fund by month nine.
https://projectcontrolsinstitute.org/monte-carlo-cost-simulation
#RiskManagement #CostEngineering

---

*Internal links: the final post carries the only link and points at [how to run a Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) with that anchor. Reply posts should use [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) and [a risk register that gets used](https://projectcontrolsinstitute.org/risk-register-that-gets-used) with those anchors.*
