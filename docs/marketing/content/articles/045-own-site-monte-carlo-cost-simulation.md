---
platform:      Own site — projectcontrolsinstitute.org
type:          how-to
title:         How to run a Monte Carlo cost simulation, step by step
meta:          How to run a Monte Carlo cost simulation: ranging an estimate, setting correlation, modelling discrete risks and sizing contingency at P80, step by step.
primary_kw:    Monte Carlo cost simulation
secondary_kw:  cost contingency, P80 estimate, correlation, three-point estimate
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo + FAQPage
word_count:    1801
hashtags:      n/a (own site)
ab_id:         AB-01051
---

# How to run a Monte Carlo cost simulation, step by step

A Monte Carlo cost simulation samples every uncertain line in an estimate thousands of times and records the total each time, producing a distribution of outcomes rather than one number. You read contingency off that distribution at a chosen confidence, usually P80. The work is in the inputs: ranges, correlation and discrete risks.

This guide runs the whole method on an eight-line estimate, with arithmetic you can check on paper.

## Step 1 — Decide the question before you open the tool

Write the question down in one sentence. "How much contingency does this estimate need to be 80% likely to hold, at sanction, excluding scope change?"

That sentence fixes four things: the confidence level, the decision point, the base estimate being tested, and what is deliberately outside the model.

Say explicitly what is excluded. Scope growth, foreign exchange and force majeure are usually held outside the contingency model and funded separately, and a reader who is not told will assume they are inside it.

## Step 2 — Build a risk model, not a copy of the estimate

Roll the estimate up to somewhere between 15 and 40 lines. Every line must be one somebody can range with a straight face.

A 4,000-line estimate cannot be ranged honestly in the time available, so its inputs get guessed, and a simulation over guessed inputs returns a precise answer to a question nobody asked.

Keep the structure recognisable to the estimator. If the risk model does not reconcile to the base estimate line by line, the first challenge in the review is about the mapping rather than the risk.

## Step 3 — Range each line with a three-point estimate

Every line needs an optimistic, most likely and pessimistic value from evidence: tender returns, historic outturn, benchmark rates, or a structured interview with the person who owns the package.

Here is the model, in £m. The PERT mean of each line is (O + 4M + P) / 6, and the standard deviation approximation is (P − O) / 6.

| Estimate line | Optimistic | Most likely | Pessimistic | PERT mean | σ |
|---|---:|---:|---:|---:|---:|
| Civils and earthworks | 8.00 | 9.50 | 14.00 | 10.000 | 1.000 |
| Structural steel | 5.00 | 6.00 | 9.00 | 6.333 | 0.667 |
| Mechanical | 11.00 | 13.00 | 18.00 | 13.500 | 1.167 |
| Electrical, instrumentation and control | 6.50 | 7.50 | 10.50 | 7.833 | 0.667 |
| Commissioning | 2.00 | 2.50 | 4.50 | 2.750 | 0.417 |
| Site preliminaries (time-related) | 4.00 | 4.80 | 7.20 | 5.067 | 0.533 |
| Design and engineering | 3.00 | 3.40 | 4.60 | 3.533 | 0.267 |
| Owner's costs | 1.50 | 1.80 | 2.60 | 1.883 | 0.183 |
| **Total** | | **48.50** | | **50.900** | |

The base estimate — the sum of the most likely values — is **£48.50m**. The sum of the PERT means is **£50.90m**.

That £2.40m gap exists before a single risk event is added. It appears because the most likely value is the mode of a right-skewed line, the mean of a right-skewed line sits above its mode, and adding modes across eight lines compounds the error eight times.

## Step 4 — Separate ordinary variability from discrete risk events

Ranging captures how much a line varies when the work goes normally. It does not capture an event that either happens or does not.

Model those separately, each with a probability and its own impact range.

| Risk event | Probability | Impact (mean) | Expected value |
|---|---:|---:|---:|
| Ground conditions worse than the boreholes indicated | 35% | £2.20m | £0.770m |
| Permit delay of 6–14 weeks at £0.11m a week | 25% | £1.027m | £0.257m |
| Long-lead vendor fails and the package is re-let | 10% | £3.50m | £0.350m |
| **Total expected value** | | | **£1.377m** |

Read that total carefully; it is the most misused number in risk management. The project will never spend £0.770m on ground conditions. It spends nothing, 65% of the time, or between £1.20m and £4.00m.

Expected value is a portfolio number. The distribution is what you actually fund, and it is why the simulation exists at all.

## Step 5 — Set correlation deliberately

This is the step that moves the answer most, and the one most often left at the default.

If every line is sampled independently, the total's standard deviation is the square root of the sum of the variances. Squaring and summing the eight σ values gives 3.813, so σ = **£1.95m**.

If the lines move together — one steel market, one labour market, one weather window — the standard deviation is the simple sum of the individual σ values, which is **£4.90m**.

| Correlation assumption | σ of the total | P80 ≈ mean + 0.84σ | Contingency over £48.50m base |
|---|---:|---:|---:|
| Fully independent | £1.95m | £52.54m | £4.04m (8.3%) |
| Fully correlated | £4.90m | £55.02m | £6.52m (13.4%) |

The same estimate, the same ranges, and **£2.48m** of difference produced entirely by an assumption most people never state.

Reality sits between the two. Commodity-linked lines usually carry a positive correlation of roughly 0.3 to 0.5 with each other; owner's costs mostly do not. Set the pairs you can justify, write down why, and let the review challenge it.

## Step 6 — Run it, and check it has settled

Run 1,000 iterations, then 5,000, then 10,000, comparing the P50 and P80 each time. When the P80 moves by less than the precision you will report, stop.

Re-run with a different random seed and confirm the answer holds. If it does not, the sample is too small, not the model too complex.

More iterations improve precision and never accuracy. A poor model run 100,000 times is still a poor model, to four decimal places.

## Step 7 — Read the result and set contingency

Combining the ranged estimate with the three risk events gives a mean of 50.90 + 1.38 = **£52.28m**. Adding the variance of the risk events (2.483) to the independent base variance (3.813) gives 6.296, so σ = **£2.51m**.

The P80 is then 52.28 + (0.84 × 2.51) = **£54.39m**. Against the £48.50m base estimate, contingency is **£5.89m**, or **12.1%**.

Two honest caveats. This normal approximation is a demonstration shortcut; a real simulation preserves the lumpy shape that a 10% chance of a £3.50m event creates, and the true P80 will differ around that lump. And the whole result rests on the correlation choice in step 5, which is a judgement, not a calculation.

The tornado chart is the output that changes behaviour. In this model, mechanical contributes 1.361 of the base variance of 3.813 — **36%** of the ordinary uncertainty — so that is where mitigation money buys the most, whatever the risk register says is scariest.

## Step 8 — Turn the number into a drawdown plan

A contingency figure with no owner is spent by month two and argued about in month nine.

Set the governance in one line: the team plans to the P50, the sponsor holds the gap to the P80, and release happens against defined trigger events rather than against optimism.

Then track drawdown as a curve against time. If contingency is falling faster than the work is progressing, the forecast is already wrong and the monthly report should say so before somebody else notices. Which line the drawdown moves, and how the movement is reported, is covered in [where contingency sits inside the budget and the forecast](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting).

## Why this belongs in a finance syllabus

Contingency is not a schedule artefact. It is capital held against a probability, and it shows up in the funding requirement, in the cash profile, and eventually in reported margin. Where in the year that money has to be available is a question for [the S-curve behind a project cash flow forecast](https://projectcontrolsinstitute.org/project-cash-flow-forecasting), not for the risk model.

The £5.89m above has to be funded, and the delay days that produced part of it arrive as time-related cost, extended overheads and later certification — which means later cash. A schedule risk result that never crosses into the cost model and the cash forecast changes nothing at all. That crossing is set out from the schedule side in [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis).

PCI AI Project Finance Leader (PFL-AI) covers **16 domains and 61 knowledge areas**, with a Body of Knowledge weighted **40 / 40 / 20** across finance and reporting, project management and governed AI. The calculation content behind PFL-AI and PML-AI is verified by **15,613 machine calculation checks, all passing**; PCL-AI has no equivalent suite.

## Frequently asked questions

**How many iterations does a Monte Carlo cost simulation need?**
Enough that the answer stops moving. Compare the P50 and P80 at 1,000, 5,000 and 10,000 iterations, then re-run with a new seed. When the P80 shifts by less than the precision you will report, the sample is stable. Ten thousand is a common landing point for a cost model of 15 to 40 lines.

**Should contingency be held at P50 or P80?**
Hold both, for different purposes. The P50 is a working target the team can plan against without demoralising anyone. The P80 is a funding position, because it covers most of the realistic downside without pricing the extreme tail. What breaks governance is committing to the P50 externally and reporting against it internally as though it were funded.

**Can I run this in a spreadsheet?**
Yes, for a model of this size. A spreadsheet with a random draw per line, a few thousand recalculations and a sorted output will produce a usable distribution. Where spreadsheets get difficult is correlation between lines and discrete risks with conditional impacts, which is the point at which purpose-built tools earn their fee.

**How do I range a line when there is no historic data?**
Use structured interviews and record the reasoning, not only the number. Ask what the cheapest and most expensive comparable package cost and why, before revealing the estimate; showing it first anchors the answer within a few per cent. A defensible range with a written source beats a precise one with none.

**Does a Monte Carlo result guarantee the project lands inside it?**
No, and any statement that it does should be corrected. The output is conditional on the ranges, the correlation and the risks modelled. It says nothing about what was left out, which is why the exclusions from step 1 are published alongside the number rather than buried in an appendix.

---

*Internal links now in the body, all on this domain: [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) where the schedule side of the same contingency question is raised; [the S-curve behind a project cash flow forecast](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) where contingency becomes a funding requirement with a date on it; and [where contingency sits inside the budget and the forecast](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) in the drawdown step, which raises which line the money moves against. The schedule risk analysis pillar was dropped as a fourth: three same-domain links is the limit, and the QSRA guide already carries the reader there. Reciprocal worth making: the cost engineer certification piece should link back with the anchor "sizing contingency by simulation".*
