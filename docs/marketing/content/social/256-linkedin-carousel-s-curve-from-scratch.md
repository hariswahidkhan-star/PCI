---
platform:      LinkedIn carousel
type:          carousel
title:         The project S-curve, built from scratch in ten slides
meta:          A straight-line baseline reported SPI 0.940. The properly spread curve reported 0.839. Ten slides on building an S-curve that tells the truth about progress.
primary_kw:    S-curve formula
secondary_kw:  planned value curve, cost loading, cash flow curve, cash conversion cycle
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo
word_count:    1045
hashtags:      #ProjectControls #EarnedValue #CostEngineering #ProjectFinance
ab_id:         AB-00212
---

# The project S-curve, built from scratch in ten slides

*LinkedIn document post — 10 slides, 1080 × 1350. No link in the body; the link goes in the first comment.*

**Post caption (the first two lines carry the post):**

Same progress, same month, two baselines.
The straight-line curve reported SPI 0.940. The properly spread curve reported 0.839.

Nobody worked differently. The spread was wrong. Ten slides on building an S-curve from the budget up.

---

**Slide 1 — An S-curve is a series, not a picture**

An S-curve is cumulative planned value plotted against time. It exists so that at any data date you can say what should have been earned by now, which is the only reason schedule performance can be measured at all. The shape is a consequence, not the point.

**Slide 2 — Three inputs and nothing else**

A budget at completion, a calendar, and a spread that says how the budget distributes across the periods. Get the spread from the resourced schedule where you have one, and from a defensible profile where you do not.

**Slide 3 — The arithmetic**
Control account: **BAC £5.0m**, ten months.

| Month | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| Period % | 3 | 7 | 12 | 16 | 18 | 16 | 12 | 8 | 5 | 3 |
| Cumulative % | 3 | 10 | 22 | 38 | **56** | 72 | 84 | 92 | 97 | 100 |
| Cumulative £m | 0.15 | 0.50 | 1.10 | 1.90 | **2.80** | 3.60 | 4.20 | 4.60 | 4.85 | 5.00 |

At the month 5 data date: **PV £2.80m**, **EV £2.35m**, **AC £2.62m**.

SV = EV − PV = 2.35 − 2.80 = **−£0.45m** SPI = 2.35 ÷ 2.80 = **0.839**
CV = EV − AC = 2.35 − 2.62 = **−£0.27m** CPI = 2.35 ÷ 2.62 = **0.897**
EAC = BAC ÷ CPI = 5.00 ÷ 0.897 = **£5.57m**, an overrun of **£0.57m**

The curve is what makes the 0.839 possible. Without a time-phased PV there is no schedule index at all.

**Slide 4 — The formula, and what it is for**

Where you have no resourced schedule, a smooth symmetric profile is a defensible placeholder. Cumulative fraction P at elapsed fraction t:

**P(t) = 3t² − 2t³**

t = 0.3 → 3(0.09) − 2(0.027) = **21.6%**
t = 0.5 → 3(0.25) − 2(0.125) = **50.0%**
t = 0.7 → 3(0.49) − 2(0.343) = **78.4%**

It is symmetric about the midpoint. Real projects rarely are — the table on slide 3 reaches 56% at the halfway point because it front-loads. Use the formula to start a curve, never to finish one.

**Slide 5 — Why a straight line flatters you**

Straight-line the same £5.0m and month 5 PV becomes £2.50m rather than £2.80m. On the same EV of £2.35m:

Straight line: SPI = 2.35 ÷ 2.50 = **0.940**
Real spread: SPI = 2.35 ÷ 2.80 = **0.839**

A tenth of an SPI point conjured out of a lazy baseline, and it disappears late in the job when there is no time left to recover.

**Slide 6 — Three curves on one chart, and only three**

Planned value is the baseline curve. Earned value is what has been achieved, valued at budget. Actual cost is what has been spent. The vertical gap between PV and EV is schedule variance in money; the gap between EV and AC is cost variance.

Anything else on that chart — forecast lines, revised baselines, targets — needs its own legend entry or the reader will misread it.

**Slide 7 — Early and late curves bound the honest range**

Run the schedule with everything at its early dates and again at its late dates, and plot both. The envelope between them is where actual progress can legitimately sit without the programme being wrong.

Progress tracking below the late curve is not a variance to explain. It is a date that has already been missed.

**Slide 8 — The cash curve is not the cost curve**

Cost is recognised when the work is done. Cash leaves when the supplier is paid. On 30-day terms, cash out at month 5 corresponds to cost incurred to month 4: **£1.90m paid against £2.80m of cost incurred**.

That £0.90m gap is not a saving. It is a liability that has not yet been settled, and treating a cash report as a cost report is one of the most common reasons a project looks under budget until the month it does not.

**Slide 9 — The working capital the curve implies**

For the contractor's side of the same account: days of unbilled work in progress 21, days from invoice to cash 62, days from supplier invoice to payment 45.

**Cash conversion cycle = 21 + 62 − 45 = 38 days**

Average daily spend = £5.0m ÷ 304 days = **£16,447**
Working capital tied up = 38 × 16,447 = **£625k**

That £625k is real money the business funds so the project can run, and it never appears anywhere on the cost curve.

**Slide 10 — Six checks before you publish one**

Does cumulative reach exactly 100% of BAC. Does the spread come from the schedule or from a stated assumption. Is the data date marked. Are PV, EV and AC on the same cut-off. Is the calendar the project calendar. Is the vertical axis money, not percentage complete.

The last one matters most, because a percentage curve hides the size of the variance and finance cannot use it. Your time-phased budget is also the basis on which cost-to-cost progress is measured for revenue — the PCI AI Project Finance Leader (PFL-AI) examines that crossing across 16 domains and 61 knowledge areas.

---

#ProjectControls #EarnedValue #CostEngineering #ProjectFinance

**First comment:** How the time-phased budget becomes a controllable baseline, and what to do when the spread and the schedule disagree: https://projectcontrolsinstitute.org/project-budgeting-and-forecasting

---

*Every figure above is illustrative arithmetic, not project data. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and follow-up comment): [project budgeting and forecasting](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) with the anchor "turning an estimate into a controllable baseline", [an earned value worked example](https://projectcontrolsinstitute.org/earned-value-worked-example) with the anchor "the same month worked through in full", and [project cash flow forecasting](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) with the anchor "why the cash curve lags the cost curve".*
