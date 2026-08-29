---
platform:      X / Threads
type:          thread
title:         Three-point estimating: where the PERT mean misleads
meta:          Same three inputs, two answers £23k apart, and £755k of contingency riding on a correlation assumption nobody states. Six posts on three-point estimating.
primary_kw:    three-point estimating *
secondary_kw:  PERT formula, contingency, P80, estimate to complete
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    389
hashtags:      #CostEngineering #ProjectFinance
ab_id:         AB-00273
---

# Three-point estimating: where the PERT mean misleads

*X / Threads thread — 6 posts, each under 280 characters and each able to stand alone. The link sits in the final post. Character counts are for production; X counts any URL as 23 characters, so the live figures run lower.*

**Post 1/6 — the hook** (221 characters)
One package. Optimistic £180k, most likely £220k, pessimistic £400k.

PERT says £243k. A straight triangular average says £267k.

Same three inputs, £23k apart, and I have never seen anyone write down which one they used.

**Post 2/6 — the formulas** (228 characters)
PERT mean = (O + 4M + P) ÷ 6 = (180 + 880 + 400) ÷ 6 = £243.3k
Triangular mean = (O + M + P) ÷ 3 = £266.7k
PERT standard deviation = (P − O) ÷ 6 = 220 ÷ 6 = £36.7k

PERT weights the mode four times. That is the whole difference.

**Post 3/6 — the arithmetic that decides the funding** (230 characters)
Thirty packages like it. Mean total = 30 × 243.3 = £7.30m.

Independent: σ = 36.7 × √30 = £201k → P80 ≈ £7.47m
Perfectly correlated: σ = 30 × 36.7 = £1.10m → P80 ≈ £8.22m

£755k of contingency turns on an assumption nobody states.

**Post 4/6 — the tail gets one-sixth** (214 characters)
PERT assumes a beta shape and gives P one-sixth of the weight. Defensible if P is a genuine one-in-twenty outcome.

If P is "the worst I have seen", you have averaged in an anecdote and handed it 17% of the answer.

**Post 5/6 — the mode is usually the tender** (263 characters)
The quieter failure is M. On most estimates the most likely value is the tender number, so O and P get drawn symmetrically around a figure that was already the answer.

Ask what has to be true for O, and what has to go wrong for P, before either enters the sheet.

**Post 6/6 — where the number lands** (303 characters)
The P80 is a funding decision, not a spreadsheet one. It sets the estimate to complete, that sets the estimate at completion, and an expected loss is recognised in full the month it becomes expected.
https://projectcontrolsinstitute.org/project-budgeting-and-forecasting
#CostEngineering #ProjectFinance

---

*Figures are a worked example. PCI publishes certification requirements; nothing here is accounting advice.*

*Internal links: the final post carries the only link and points at [project budgeting and forecasting](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) with that anchor. Reply posts should use [Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) and [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) with those anchors.*
