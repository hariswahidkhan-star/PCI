---
platform:      Own site — projectcontrolsinstitute.org
type:          guide
title:         Quantitative schedule risk analysis: a beginner's guide
meta:          A beginner's guide to quantitative schedule risk analysis (QSRA): the vocabulary, ten hand-run iterations you can check, and five mistakes to avoid.
primary_kw:    quantitative schedule risk analysis QSRA
secondary_kw:  Monte Carlo simulation, criticality index, merge bias, P80 date
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1800
hashtags:      n/a (own site)
ab_id:         AB-00081
---

# Quantitative schedule risk analysis: a beginner's guide

Quantitative schedule risk analysis (QSRA) replaces a single finish date with a range of dates and the odds attached to each. You give every activity a low, likely and high duration, map risk events onto the activities they would hit, then run the network thousands of times. The output is a confidence level, such as a P80 date.

The intimidating part is usually the vocabulary, not the maths. This page fixes that, then walks ten iterations by hand so you can see what a simulation does.

## What is quantitative schedule risk analysis (QSRA)?

It is the practice of testing a programme against uncertainty instead of accepting its dates, and it is the quantified end of [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis); the qualitative end ranks risks in a workshop and never recalculates the network. The deterministic critical path method returns one answer, calculated from durations treated as facts.

They are not facts. Every duration is an estimate with a range behind it, and the QSRA makes that range explicit, samples from it repeatedly, and records where the project finished each time.

The result is a distribution of thousands of finish dates, from which any confidence level can be read.

## The vocabulary, in one table

| Term | What it means | What it is for |
|---|---|---|
| Iteration | One pass in which every duration is sampled once and the network recalculated | The unit of a simulation; thousands make the distribution |
| Distribution | The shape describing how likely each duration is | Turns three estimates into something samplable |
| P50 / P80 / P90 | The date by which that percentage of iterations had finished | Reading the answer at a chosen confidence |
| Deterministic date | The date the plan reports with no ranging | The number in the contract programme |
| Criticality index | Share of iterations in which an activity was on the critical path | Finding paths that turn critical only under stress |
| Sensitivity | Correlation between a duration and the project finish | Ranking where mitigation buys days |
| Merge bias | Delay caused by a milestone waiting for the latest of several paths | Why the deterministic date is optimistic |
| Risk driver | A register risk with a probability and impact range, mapped to the activities it hits | Modelling discrete events, and correlation, honestly |
| Schedule contingency | The gap between the deterministic date and the committed date | The thing the analysis exists to size |
| Tornado chart | Ranked bar chart of the biggest influences on the finish date | The output that changes behaviour |

## Worked: ten iterations by hand

Two paths converge on a handover milestone. Path X is most likely 46 days, ranging 40 to 64. Path Y is most likely 48 days, ranging 44 to 58.

[The deterministic critical path method](https://projectcontrolsinstitute.org/critical-path-method) says the milestone lands at day **48**, because Y is longer. X has two days of total float and is therefore, in a bar chart, not a problem.

Now sample ten times. Each iteration draws one duration per path and takes the later of the two, because the milestone waits for both. The draws are illustrative; the arithmetic is exactly what a tool performs.

| Iteration | Path X | Path Y | Milestone = later of the two | Which path drove it |
|---:|---:|---:|---:|---|
| 1 | 49 | 47 | 49 | X |
| 2 | 44 | 52 | 52 | Y |
| 3 | 58 | 46 | 58 | X |
| 4 | 46 | 49 | 49 | Y |
| 5 | 52 | 55 | 55 | Y |
| 6 | 43 | 45 | 45 | Y |
| 7 | 47 | 51 | 51 | Y |
| 8 | 61 | 48 | 61 | X |
| 9 | 45 | 50 | 50 | Y |
| 10 | 50 | 53 | 53 | Y |

Sort the milestone column: 45, 49, 49, 50, 51, 52, 53, 55, 58, 61.

**The mean** is 523 / 10 = **52.3 days**, four days later than the deterministic 48. **The P50** is the midpoint of the fifth and sixth values, (51 + 52) / 2 = **51.5 days**. **The P80** is around the eighth value, **55 days**, so contingency is seven days.

**The deterministic date held once in ten.** Only iteration 6 finished on or before day 48. The date that looked settled has roughly a one-in-ten chance in this model.

**The criticality index of X is 30%.** Path X drove the milestone in iterations 1, 3 and 8, despite carrying float in the deterministic plan. A path with float and a wide range is a critical path that has not declared itself yet.

Ten iterations are too few to trust; the P80 would move if you drew ten more. What they show is that a simulation is this table, repeated until the answer stops moving.

## Choosing a distribution

The distribution turns three numbers into a range you can sample. Triangular is a reasonable default.

| Distribution | What it assumes | Use it when | Where it misleads |
|---|---|---|---|
| Triangular | Straight lines from low to likely to high | You have three estimates and little else | Puts more weight in the tails than most real work |
| PERT / beta | A smooth curve weighting the likely case four times | The most likely value is genuinely well known | Understates the tail when the range came from optimists |
| Uniform | Every value between low and high equally likely | You genuinely know only the bounds | Almost always too pessimistic in the middle |
| Lognormal | Right-skewed, no upper bound | Durations that can run away — commissioning, approvals | Needs data, and produces alarming outliers |
| Discrete / Bernoulli | The event happens or it does not | Permits, factory tests, vendor failure | Modelled as a range instead of an event |

The choice matters less than the honesty of the inputs. A triangular distribution over evidenced ranges beats a carefully chosen curve over numbers somebody guessed, and a symmetric range of 38, 40, 42 says nothing can go materially wrong — a statement about the workshop rather than the work.

## Your first QSRA, in seven steps

**One, fix the network first.** Open ends, hard constraints, negative lag and out-of-sequence progress all distort the answer, and the simulation will not warn you. Run a schedule quality assessment before ranging anything.

**Two, build a risk model, not a copy of the schedule.** Sixty to 150 activities preserving the real logic and merge points range more honestly and explain far better than 4,000 lines.

**Three, get ranges from evidence.** Historic actuals, productivity benchmarks and structured interviews. Ask what the fastest and slowest comparable job took before showing anyone the plan, or the plan comes back with a few days added.

**Four, map risk events separately.** A permit refusal is not ordinary variability; it is an event with a probability and an impact, mapped to every activity it would hit.

**Five, set correlation deliberately.** One late vendor hitting nine fabrication activities is one event, not nine. Sampling them independently makes the tail far too thin.

**Six, run until the answer settles.** Try 1,000 iterations, then 5,000, then 10,000, comparing the P50 and P80 each time. When the P80 moves by less than a day, stop.

**Seven, report the model with the answer.** Publish the ranges, their sources and the assumptions beside the date. A P80 with no visible inputs gets sent back for another look.

## Five mistakes beginners make

**Ranging a broken schedule.** A network with 40 open ends produces a precise, confident, wrong answer. Fix first, range second.

**Treating the most likely duration as the average.** It is the mode. On a right-skewed distribution the mean sits later, and summing modes along a chain compounds that error at every link.

**Ignoring near-critical paths.** A floated path drove the date 30% of the time above. Anything with less float than uncertainty belongs in the model.

**Double-counting.** A risk modelled as a driver and also baked into a pessimistic duration is counted twice, and whoever spots it will argue the contingency away.

**Running it once and filing it.** A QSRA at sanction and never again is a document. Repeat it when the logic changes, and track contingency drawdown monthly in between.

## Where this sits in the PCI curriculum

PCI AI Project Controls Leader (PCL-AI) covers **13 domains and 61 knowledge areas**, and quantitative risk sits among them alongside planning, progress measurement and forecasting.

The placement is practical. A P80 date that never reaches the cost forecast changes nothing: the extra days have to arrive as time-related cost in the estimate at completion and as a movement in the cash profile. That conversion is covered in [how to run a Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation).

## Frequently asked questions

**Is a QSRA the same thing as a Monte Carlo simulation?**
No. Monte Carlo is the sampling technique, the repeated drawing shown in the table above. The analysis is the whole exercise: fixing the network, evidencing ranges, mapping risks, setting correlation, running the simulation and interpreting criticality. Teams that buy the tool and skip the first four steps get numbers, not answers.

**How many activities should the risk model have?**
Enough to preserve the logic and the merge points, and few enough that every duration can be defended. Sixty to 150 is a common working range on a major project. A model at full schedule detail cannot be ranged honestly in the time available, so its inputs end up guessed.

**What confidence level should we commit to?**
There is no universal answer and anyone quoting one is guessing. P80 is a common basis for external commitments because it holds most of the realistic downside without pricing the extreme tail. Regulated work or heavy liquidated damages can justify P90. Consistency across the portfolio matters more than the level.

**What if the P80 is later than the date we have already promised?**
Then you have found the problem the analysis exists to find, and the honest options are scope, resource, sequence or the promise. What does not work is sending the model back to be re-ranged until it agrees. Publishing the inputs is the defence, agreed before the first run.

---

*Internal links now in the body, all on this domain: [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis) in the definition section, where a reader asks what else sits under schedule risk beyond simulation; [the deterministic critical path method](https://projectcontrolsinstitute.org/critical-path-method) at the point the worked example quotes a single deterministic date and the reader needs to know how that date was produced; and [how to run a Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) where the P80 has to reach the cost forecast. The total float definition was dropped rather than added as a fourth: three same-domain links is the limit here. Reciprocal worth making: the project scheduler certification piece should link back with the anchor "quantifying schedule risk on a programme".*
