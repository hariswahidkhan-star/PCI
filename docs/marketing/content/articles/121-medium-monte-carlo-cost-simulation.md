---
platform:      Medium
type:          how-to
title:         Monte Carlo cost simulation: setting contingency at P80
meta:          A Monte Carlo cost simulation worked on an eight-line estimate: PERT ranges, discrete risks, the correlation choice, and contingency read off at P80.
primary_kw:    Monte Carlo cost simulation
secondary_kw:  cost contingency, P80 estimate, correlation, three-point estimate
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /monte-carlo-cost-simulation (own site #045)
schema:        HowTo + FAQPage
word_count:    1,824
hashtags:      #ProjectControls #RiskManagement #CostEngineering #ProjectFinance #PMO
ab_id:         AB-01051
---

# Monte Carlo cost simulation: setting contingency at P80

A Monte Carlo cost simulation samples every uncertain line in an estimate thousands of times and records the total each time. The output is a distribution instead of a single number, and contingency is read off that distribution at a chosen confidence, usually P80. The method is simple; the inputs are the work.

What follows is the whole thing run on an eight-line estimate, with arithmetic you can check on paper.

## Start with the question, not the tool

Write the question down in one sentence before opening anything. "How much contingency does this estimate need to be 80% likely to hold, at sanction, excluding scope change?"

That sentence fixes four things at once: the confidence level, the decision it supports, the base estimate under test, and what is deliberately outside the model.

Publish the exclusions rather than burying them. Scope growth, foreign exchange and force majeure are usually funded outside contingency, and a reader who is not told will assume they sit inside it.

## Build a risk model, not a copy of the estimate

Roll the estimate up to somewhere between 15 and 40 lines. Every line has to be one a competent person can range without embarrassment.

A 4,000-line estimate cannot be ranged honestly in the time available. Its inputs get guessed, and a simulation over guessed inputs returns a precise answer to a question nobody asked.

Keep the structure recognisable to the estimator. If the risk model does not reconcile to the base estimate line by line, the first hour of the review goes on the mapping rather than on the risk.

## Range each line with a three-point estimate

Each line needs an optimistic, most likely and pessimistic value with a source behind it: tender returns, historic outturn, benchmark rates, or a structured interview with whoever owns the package.

The PERT mean is (O + 4M + P) / 6. The standard deviation approximation is (P − O) / 6. Both are shown for every line, in £m.

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

That £2.40m gap opens before a single risk event is added. The most likely value is the mode of a right-skewed line, the mean of a right-skewed line sits above its mode, and adding modes across eight lines compounds the same error eight times.

## Keep discrete risks out of the ranges

Ranging captures how a line behaves when the work goes normally. It does not capture an event that either happens or does not happen.

Model those separately, each with its own probability and impact range, drawn from [the register the ranges came from](https://projectcontrolsinstitute.org/risk-register-that-gets-used) rather than invented at the modelling stage.

| Risk event | Probability | Impact O / M / P | Impact (PERT mean) | Expected value |
|---|---:|---:|---:|---:|
| Ground conditions worse than the boreholes indicated | 35% | £1.20m / £2.00m / £4.00m | £2.20m | £0.770m |
| Permit delay of 6 / 9 / 14 weeks at £0.11m a week | 25% | £0.66m / £0.99m / £1.54m | £1.027m | £0.257m |
| Long-lead vendor fails and the package is re-let | 10% | £2.00m / £3.50m / £5.00m | £3.50m | £0.350m |
| **Total expected value** | | | | **£1.377m** |

Each impact is the PERT mean of its own range, on the same (O + 4M + P) / 6 the estimate lines use. The permit row is (0.66 + 4 × 0.99 + 1.54) / 6 = **£1.027m**, which is 9.33 weeks at £0.11m.

That expected-value total is the most misused number in risk management. This project will never spend £0.770m on ground conditions. It spends nothing, 65% of the time, or somewhere between £1.20m and £4.00m.

Expected value is a portfolio number. The distribution is what actually gets funded, which is the reason to run a simulation at all.

## Correlation moves the answer more than anything else

This step is usually left at the software default. It is also the step that changes the result most.

Sample every line independently and the standard deviation of the total is the square root of the sum of the variances. Squaring and summing the eight σ values gives 3.813, so σ = **£1.95m**.

If the lines move together — one steel market, one labour market, one weather window — the standard deviation is the simple sum of the individual σ values, **£4.90m**.

| Correlation assumption | σ of the total | P80 ≈ mean + 0.84σ | Contingency over £48.50m base |
|---|---:|---:|---:|
| Fully independent | £1.95m | £52.54m | £4.04m (8.3%) |
| Fully correlated | £4.90m | £55.02m | £6.52m (13.4%) |

Same estimate, same ranges, **£2.48m** of difference, produced entirely by an assumption most models never state out loud.

Reality sits between the two. Commodity-linked lines usually carry a positive correlation of roughly 0.3 to 0.5 with each other; owner's costs mostly do not. Set the pairs you can defend, record why, and let the review attack it.

## Run it until it stops moving

Run 1,000 iterations, then 5,000, then 10,000, comparing the P50 and P80 each time. Stop when the P80 moves by less than the precision you intend to report.

Re-run with a different random seed and confirm the answer holds. If it does not, the sample is too small; that is not evidence the model is too complex.

More iterations buy precision and never accuracy. A poor model run 100,000 times is still a poor model, now quoted to four decimal places.

## Reading the result of the Monte Carlo cost simulation

Combining the ranged estimate with the three discrete risks gives a mean of 50.90 + 1.38 = **£52.28m**.

Each discrete risk adds p(1 − p)I² of variance, taking the impact at its mean: 0.35 × 0.65 × 2.20² = 1.1011, 0.25 × 0.75 × 1.027² = 0.1978, and 0.10 × 0.90 × 3.50² = 1.1025. Those three come to 2.4014, or **2.401**.

Adding that to the independent base variance of 3.813 gives 6.214, so σ = √6.214 = **£2.49m**.

The P80 is 52.28 + (0.84 × 2.49) = **£54.37m**. Against the £48.50m base estimate, contingency is **£5.87m**, or **12.1%**.

Two caveats belong next to that figure. The arithmetic here is a demonstration shortcut: a normal approximation, with each risk impact taken at its mean rather than sampled across its range. A real simulation preserves the lumpy shape that a 10% chance of a £3.50m event creates, and the true P80 sits around that lump rather than on a smooth curve.

The second caveat is the correlation judgement above. The answer still rests on it, and it is a judgement rather than a calculation.

## The tornado is the output that changes behaviour

The distribution tells you how much. The variance contribution tells you where.

Mechanical contributes 1.361 of the base variance of 3.813 — **36%** of the ordinary uncertainty in this estimate. That is where mitigation money buys the most, whatever the register says is scariest.

Left alone, risk attention follows fear. A contribution table redirects it towards the lines that are actually moving the total.

## Turn the number into a drawdown plan

Contingency with no owner is spent by month two and argued about in month nine.

Set the governance in one line: the team plans to the P50, the sponsor holds the gap to the P80, and release happens against defined trigger events rather than against optimism.

Then track drawdown as a curve against time. If contingency is falling faster than the work is progressing, the forecast is already wrong, and the monthly report should say so before somebody else finds it.

## Why this is a finance subject as much as a risk one

Contingency is capital held against a probability. It appears in the funding requirement, in the cash profile, and eventually in reported margin.

The £5.87m above has to be financed. The delay days inside it arrive as time-related cost, extended overheads and later certification, which means later cash.

A chartered accountant is examined on provisions and cut-off, almost never on a P80. An engineer is examined on ranges and float, almost never on what a contingency release does to a reported result. This number sits in both places, which is why it is where projects lose money quietly.

Nothing PCI publishes is legal, tax or accounting advice, and the treatment depends on the contract.

The PCI AI Project Finance Leader (PFL-AI) covers **16 domains and 61 knowledge areas**, with a Body of Knowledge that runs in a **40 / 40 / 20** proportion across finance and reporting, project management, and governed AI. The calculation content behind PFL-AI and PML-AI is verified by **15,613 machine calculation checks, all passing**; the PCI AI Project Controls Leader (PCL-AI) has no equivalent suite.

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**How many iterations does a Monte Carlo cost simulation need?**
Enough that the answer stops moving. Compare the P50 and P80 at 1,000, 5,000 and 10,000 iterations, then re-run with a new seed. When the P80 shifts by less than the precision you will report, the sample is stable. Ten thousand is a common landing point for a model of 15 to 40 lines.

**Should contingency be held at P50 or P80?**
Hold both, for different purposes. The P50 is a working target the team can plan against without demoralising anyone. The P80 is a funding position, because it covers most of the realistic downside without pricing the extreme tail. What breaks governance is committing to the P50 externally while reporting against it internally as though it were funded.

**Can I run this in a spreadsheet?**
Yes, at this size. A random draw per line, a few thousand recalculations and a sorted output will give a usable distribution. Spreadsheets get awkward at correlation between lines and at discrete risks with conditional impacts, and that is the point where purpose-built tools start to earn their licence fee.

**How do I range a line when there is no historic data?**
Use structured interviews and record the reasoning rather than only the number. Ask what the cheapest and most expensive comparable package cost and why, before revealing the estimate; showing the estimate first anchors the answer within a few per cent. A defensible range with a written source beats a precise one with none.

**Does a Monte Carlo result guarantee the project lands inside it?**
No, and any statement that it does should be corrected in the room. The output is conditional on the ranges, the correlation and the risks modelled. It says nothing about what was excluded, which is why the exclusions written down at the start are published alongside the number rather than filed in an appendix.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so this republish is here for readers rather than for link equity.*

*Internal links: one is now placed in the body. The risk register how-to (projectcontrolsinstitute.org) sits on "the register the ranges came from", in the sentence that tells the reader to model discrete risks with their own probability and impact — the obvious next question is where a defensible probability comes from, and that piece answers it. The note also proposed the QSRA guide and the cash flow forecasting page: both are dropped from this republish, since three links to a single domain in one article reads as a link scheme rather than as help, and both belong on the own-site original as internal links. Reciprocal: the risk register piece should link back here with the anchor "pricing the register at P80", because it produces the probabilities and impacts this simulation consumes.*
