---
platform:      Reddit / forum — r/PrimaveraP6
type:          forum-post
title:         Monte Carlo on a schedule: what it does and does not
meta:          Three parallel paths each 50/50 on time give a milestone a 12.5% chance of hitting its date. That is what QSRA shows and a Gantt chart cannot.
primary_kw:    quantitative schedule risk analysis QSRA
secondary_kw:  merge bias, three-point estimate, criticality index, P80 date
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1244
hashtags:      n/a (Reddit)
ab_id:         AB-01048
---

# Monte Carlo on a schedule: what it tells you and what it does not

Three parallel paths feed a milestone. Each has a genuine 50/50 chance of hitting its date. The milestone's chance of hitting the same date is 0.5 × 0.5 × 0.5 = **12.5%**. Your Gantt chart shows all three arriving on time and the milestone as green.

Quantitative schedule risk analysis (QSRA) is the practice of running a schedule thousands of times with durations sampled from ranges instead of fixed values, and reading the distribution of finish dates that comes out. It exists to expose exactly the arithmetic above, which no deterministic schedule can show you.

## Merge bias is the whole reason to bother

Extend it. Five feeding paths, each **80%** likely to be on time — a schedule most planners would call healthy. The milestone: 0.8⁵ = **0.328**, about a one-in-three chance.

Nothing is wrong with any individual path. The milestone is late because parallel work has to *all* arrive, and probability multiplies.

This is why integration milestones, ready-for-commissioning dates and system handovers slip on jobs where every individual package reports green. It is also why adding parallel paths to "de-risk" a milestone makes it worse, not better.

You can do this arithmetic on the back of an envelope. Monte Carlo just does it for a network with 3,000 activities and 60 merge points instead of one.

## Three-point durations: the optimism is already in the model

Take an activity with an optimistic duration of 18 days, most likely 25, pessimistic 45.

- Triangular mean = (18 + 25 + 45) ÷ 3 = **29.3 days**
- Beta / PERT mean = (18 + 4×25 + 45) ÷ 6 = 163 ÷ 6 = **27.2 days**

The deterministic duration in the schedule is 25. So before any risk event is modelled, this activity is between 2.2 and 4.3 days optimistic simply because the distribution is right-skewed and a single most-likely value ignores the tail.

Chain twelve of those in series: deterministic 12 × 25 = **300 days**; expected 12 × 27.2 = **326 days**. Twenty-six days of slip with no risk event, no weather, no late drawing. That is what "the schedule is achievable but not likely" actually means, expressed as a number.

## The correlation assumption moves the answer more than the durations

This is the setting most people leave at default and it dominates the result.

Take ten activities on a path, each with a standard deviation of 4 days. If you sample them independently, the standard deviation of the total is the root-sum-square:

√(10 × 4²) = √160 = **12.6 days**

If they are perfectly correlated — one common cause, such as a single labour pool or one engineering contractor — the standard deviation of the total is the simple sum:

10 × 4 = **40 days**

Convert to a P80, which sits roughly 0.84 standard deviations above the mean:

- Independent: mean + 0.84 × 12.6 = mean + **10.6 days**
- Correlated: mean + 0.84 × 40 = mean + **33.6 days**

Twenty-three days of difference from an assumption, not from data. Real projects sit between the two, usually 0.3 to 0.6 within a discipline. Set it deliberately, write down why, and show the board both ends. A P80 quoted without the correlation assumption stated is a number with no meaning attached.

## What it can and cannot answer

| QSRA answers this | It cannot answer this |
|---|---|
| How likely is the current date, given the ranges you supplied | Whether your logic reflects how the work will be built |
| Which paths drive the finish, and how often (criticality index) | Scope that is not in the schedule at all |
| How much schedule contingency to hold, and where | Whether the resources exist to work the parallel paths you modelled |
| Which risks buy the most time if mitigated, in days per pound | Whether the ranges you were given are honest |
| Whether a merge point is the real problem | The consequence of a constraint that stops the simulation dead |

The last one in the right-hand column is the killer. A "finish on or before" constraint clamps dates, so the simulation obediently reports a date it was told to report. Every hard constraint in a risk model is a place where the answer was decided in advance.

## Criticality index beats total float

Deterministic float tells you which path is critical in one scenario. The criticality index tells you what percentage of iterations each activity was on the critical path.

A path with 12 days of float that turns up critical in 38% of runs is a bigger threat than a zero-float path that is critical in 91% of runs and has tight, well-understood durations. Near-critical paths with wide ranges are where schedules actually fail, and float alone will never show you that.

Pair it with cruciality — how strongly an activity's duration correlates with the project finish — and you have a mitigation list ranked by effect rather than by float.

## Practicalities for a P6 model

Do not simulate the working schedule. Build or summarise to a risk model of roughly 150 to 300 activities: the working schedule has too many activities, too much detail-level logic and too many artefacts to sample sensibly, and the run time punishes you for it.

Before you sample anything, fix the model: remove hard date constraints, close open ends, remove negative lags, check that every activity has a predecessor and successor, and confirm the calendars are what you think they are. A risk analysis of a defective network is a defective answer with a confidence level printed on it.

Then sample durations by band rather than one activity at a time. Well-defined repetitive work might be 90/100/115 as a percentage of the plan; first-of-a-kind commissioning might be 95/100/160. And model discrete risk events — the permit that either arrives or does not — as probabilistic events, not as widened durations, because a 30% chance of a 60-day delay is not the same distribution as a duration that is sometimes 60 days longer.

## Common follow-ups

**How many iterations?**
Enough that the P80 stops moving between runs. A few thousand is usually plenty; the number of iterations is never the reason an answer is wrong.

**Where do the ranges come from?**
Structured interviews with the people who will do the work, run band by band, and calibrated against your own completed jobs where you have them. Ranges collected by circulating a spreadsheet come back as ±10% on everything, which is a way of saying nobody engaged.

**Why is my P80 barely later than my deterministic date?**
Usually correlation at zero, constraints still in the model, or ranges collected as ±10%. Check those three before you believe a comfortable answer.

**Does this replace float management?**
No. It tells you where contingency should sit and which paths deserve attention. Float still runs the week. The two answer different questions and both belong in the report.

---

*Disclosure: I write for the Project Controls Institute. One link, at the end, and the merge-bias arithmetic above is checkable without it: [quantitative schedule risk analysis for beginners](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis).*

*Internal links: the in-post link uses the anchor "quantitative schedule risk analysis for beginners". Comment replies should use [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis) and [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with those anchors.*
