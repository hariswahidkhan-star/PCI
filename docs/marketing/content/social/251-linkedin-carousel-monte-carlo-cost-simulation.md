---
platform:      LinkedIn carousel
type:          carousel
title:         Monte Carlo cost simulation: inputs, correlation, curve
meta:          Two runs on the same 20 cost items returned £2.41m and £4.00m of contingency. Only the correlation setting changed. Eleven slides on building the model.
primary_kw:    Monte Carlo cost simulation
secondary_kw:  contingency at P80, correlation in cost risk models, triangular distribution, quantitative risk analysis
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo
word_count:    1055
hashtags:      #ProjectControls #RiskManagement #CostEngineering #ProjectFinance
ab_id:         AB-01052
---

# Monte Carlo cost simulation: inputs, correlation, curve

*LinkedIn document post — 11 slides, 1080 × 1350. No link in the body; the link goes in the first comment.*

**Post caption (the first two lines carry the post):**

Same twenty cost items. Same three-point ranges. Same software.
One run asked for £2.41m of contingency, the other for £4.00m.

The only thing that changed was a correlation setting nobody had written down. Eleven slides on building a cost simulation you can defend.

---

**Slide 1 — A simulation is an assumption engine**

A Monte Carlo cost simulation samples every uncertain cost item thousands of times and builds a distribution of total out-turn cost. It does not find risk. It arithmetically compounds the assumptions you feed it, which is why the inputs matter more than the software.

**Slide 2 — What the model is actually made of**

Four things: a base estimate stripped of contingency, a range on each item, a rule for how items move together, and a number of iterations. Change any one and the answer changes. Only the first is usually documented.

**Slide 3 — The arithmetic**
Twenty cost items, £1.0m each, base total £20.0m.
Each carries a triangular range: minimum 0.9, most likely 1.0, maximum 1.4.

Mean per item = (0.9 + 1.0 + 1.4) ÷ 3 = **1.10** → mean total **£22.0m**
Standard deviation per item = **£108k**

**Independent items (correlation 0):**
σ total = 0.108 × √20 = **£0.483m**
P80 ≈ 22.0 + (0.842 × 0.483) = **£22.41m**
Contingency over base = **£2.41m**

**Perfectly correlated items (correlation 1):**
The sum keeps the triangular shape, so P80 per item = 1.4 − √(0.2 × 0.5 × 0.4) = **1.20**
P80 total = 1.20 × 20 = **£24.00m**
Contingency over base = **£4.00m**

**£1.59m of funding decided by a setting, not by a risk.**

**Slide 4 — Input one: a base with the fat taken out**

Strip every allowance, rounding-up and "bit of comfort" from the base before you model it. If the base already carries £1.8m of padding and the model adds contingency on top, the same risk is funded twice and the project looks more expensive than it is.

**Slide 5 — Input two: ranges that came from somewhere**

A range needs a reason. Quoted price with a firm validity date narrows it; a rate from a project three years ago in a different market widens it. Write the reason in the model, in the same row as the number, because in six months you will be asked where 1.4 came from.

**Slide 6 — The distribution choice is worth a million pounds**

Same three numbers, different shape. Triangular mean = (a + m + b) ÷ 3 = 1.10. PERT mean = (a + 4m + b) ÷ 6 = (0.9 + 4.0 + 1.4) ÷ 6 = 1.05.

Across the same twenty items that is £22.0m against £21.0m. Triangular is easy to elicit and treats the extremes generously. PERT weights the most likely and thins the tails. Pick one, say why, apply it consistently.

**Slide 7 — Uncertainty and risk events are different objects**

Uncertainty is a range on work you will definitely do: a rate, a quantity, a duration. A discrete risk event either happens or it does not, and it belongs in the model as a probability with an impact, not as a line in the base.

Adding a 25% × £2.4m risk to the base as a £600k allowance gets the mean right and destroys the tail. The model then cannot see the £2.4m outcome it was built to price.

**Slide 8 — Correlation: the setting nobody documents**

If steel price, fabrication hours and site labour all move with the same market, they are correlated, and independent sampling cancels them out against each other. That cancellation is why an uncorrelated model always looks reassuringly tight.

You do not need a precise coefficient. You need to group items that share a driver and apply a stated, moderate correlation within each group, then record the assumption on the face of the output.

**Slide 9 — Iterations: the mean settles long before the tail does**

Standard error of the mean falls with the square root of the sample count. At σ = £0.48m, 1,000 iterations gives a standard error of £15.3k and 10,000 gives £4.8k.

The tail is a different problem. At 1,000 iterations only about 50 samples land above P95, so the number you are quoting to a board is built from fifty draws. Run more than feels necessary and check that the percentile stops moving.

**Slide 10 — Reading the curve, and what P80 commits you to**

The output is a cumulative curve: cost on the x-axis, probability of coming in at or under it on the y-axis. P50 is the point you are as likely to beat as to miss. P80 is the point you would beat four times in five.

| Confidence | What it means in practice | Where it usually belongs |
|---|---|---|
| P50 | Even odds; no funded headroom | Team target, internal challenge |
| P80 | Overrun expected once in five | Project funding request |
| P90+ | Rarely justified; ties up capital | Single-shot, no-recovery scope |

Choosing P80 over P50 is a treasury decision, not a technical one. Say which you have quoted, on the slide, every time.

**Slide 11 — Where the model fails, honestly**

It cannot price a risk nobody raised. It inherits optimism from whoever set the most-likely values. And it will produce a smooth, credible curve from inputs that are guesses, which is exactly what makes an undocumented model dangerous.

The crossing point matters to finance too: the contingency you draw down becomes cost incurred, and cost incurred is the denominator of a cost-to-cost progress measure. The PCI AI Project Finance Leader (PFL-AI) examines that crossing across 16 domains and 61 knowledge areas.

---

#ProjectControls #RiskManagement #CostEngineering #ProjectFinance

**First comment:** The full build, including how the risk register feeds the model and how contingency is drawn down: https://projectcontrolsinstitute.org/monte-carlo-cost-simulation

---

*PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and follow-up comment): [Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) with the anchor "the full build", [a risk register that gets used](https://projectcontrolsinstitute.org/risk-register-that-gets-used) with the anchor "where the model's inputs come from", and [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) with the anchor "the same method applied to dates".*
