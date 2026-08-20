---
platform:      Own site — projectcontrolsinstitute.org
type:          pillar
title:         Schedule risk analysis: everything the search actually asks
meta:          What schedule risk analysis is, how a QSRA is built and read, and how to set a defensible P80 date. Worked arithmetic on a 200-day five-activity chain.
primary_kw:    schedule risk analysis
secondary_kw:  quantitative schedule risk analysis, P80 date, schedule contingency, criticality index
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    2284
hashtags:      []
ab_id:         AB-00035
---

# Schedule risk analysis: everything the search actually asks

Schedule risk analysis tests a programme's dates against uncertainty instead of accepting them. It ranges activity durations, applies risk events to the network, then runs thousands of simulated passes to produce a distribution of completion dates. The output is a confidence level, such as a P80 date, and the schedule contingency needed to defend it.

## What is schedule risk analysis?

Schedule risk analysis is the practice of quantifying how likely a plan is to be achieved. A deterministic critical path schedule returns one date. This returns the odds attached to that date and to every date around it.

It exists in two grades. The qualitative grade scores threats against activities and ranks them. The quantitative grade, normally called QSRA, simulates the whole network and returns a distribution of finish dates and float.

Only the second grade produces a number you can defend in a contingency line. A heat map tells a board which risks are worst. It does not tell them whether the committed date needs another six weeks.

## Qualitative or quantitative: which grade you actually need

| | Qualitative assessment | Quantitative simulation (QSRA) |
|---|---|---|
| Output | Ranked threats, probability-impact scoring | Distribution of finish dates, float and criticality |
| Inputs | A risk register with probability and impact | Health-checked CPM network, duration ranges, risks mapped to activities |
| Effort | Hours, one workshop | Days, plus interview time and a schedule fix-up pass |
| Answers "how late?" | No | Yes, with a confidence level |
| Answers "which activity is driving it?" | Only by opinion | Yes, by criticality and sensitivity |
| Typical use | Screening, early stages, monthly risk review | Sanction, tender, gate review, delay claim, recovery decisions |
| Fails when | Scores are argued rather than evidenced | The underlying network is unsound |

Most organisations run the qualitative grade monthly and the quantitative grade at decision points. That is a reasonable split. The mistake is presenting the qualitative output as though it carried a confidence level.

## The three inputs a QSRA needs before it means anything

### A network that can carry the analysis

A simulation inherits every defect in the schedule it runs on. Open ends, date constraints, negative lag and out-of-sequence progress all distort the result, and the simulation will not warn you.

Schedule health checking exists for this reason. The best known approach is the 14-point assessment used across defence-linked programmes, which counts logic gaps, constraint use, lead and lag abuse, high float, long durations and invalid dates. Run something equivalent before you range a single duration.

The specific killers are worth naming. An activity with no successor cannot pass delay downstream, so its risk disappears. A "must finish on" constraint hides the very slippage the analysis is meant to expose.

### Duration ranges that come from somewhere

Every activity in scope needs an optimistic, most likely and pessimistic duration. Those three numbers must come from evidence, not from adding and subtracting a comfortable percentage.

Usable sources are historic actuals for similar scopes, productivity benchmarks held by the estimating team, and structured interviews with the people who will do the work. Interviews need a facilitator who separates the duration from the commitment, because a superintendent asked for a pessimistic case in front of a client will not give you one.

Anchoring is the standard failure. Show someone a planned 40 days and the range comes back as 38, 40, 46. Ask first what the fastest and slowest comparable job took, then reveal the plan.

### Risk events mapped to the activities they hit

Duration ranging captures ordinary variability. It does not capture a discrete event such as a permit refusal or a failed factory acceptance test, which either happens or does not.

The risk driver method handles these properly. Each register risk carries a probability and a multiplicative impact range, and is mapped to every activity it would affect. When the simulation fires that risk, all mapped activities stretch together.

That mapping is also how correlation enters the model honestly. One late vendor hitting nine fabrication activities is not nine independent events, and treating it as such understates the tail badly.

## Worked example: what ranges do to a 200-day chain

Take a five-activity chain with no parallel paths, so the arithmetic stays visible.

| Activity | Optimistic | Most likely | Pessimistic | Triangular mean | PERT mean | PERT σ |
|---|---:|---:|---:|---:|---:|---:|
| A Design | 25 | 30 | 45 | 33.3 | 31.7 | 3.33 |
| B Procure | 50 | 60 | 100 | 70.0 | 65.0 | 8.33 |
| C Fabricate | 40 | 45 | 75 | 53.3 | 49.2 | 5.83 |
| D Install | 35 | 40 | 60 | 45.0 | 42.5 | 4.17 |
| E Commission | 20 | 25 | 45 | 30.0 | 27.5 | 4.17 |
| **Total** | | **200** | | **231.7** | **215.8** | |

The deterministic schedule adds the most likely durations and reports 200 days. That is the number in the contract programme.

The triangular mean of each activity is (O + M + P) / 3. Summed, it gives 231.7 days. The PERT mean, (O + 4M + P) / 6, weights the most likely case more heavily and gives 215.8 days.

Neither mean is 200. The most likely duration is the mode of a right-skewed distribution, and the mean of a right-skewed distribution always sits later than its mode. Adding modes along a chain compounds that error at every link.

Now the spread. The PERT approximation for standard deviation is (P − O) / 6, so activity B contributes (100 − 50) / 6 = 8.33 days. Squaring each and summing gives a variance of 149.3, and a chain standard deviation of 12.2 days.

An 80% confidence date sits roughly 0.84 standard deviations above the mean on a normal approximation. That is 215.8 + (0.84 × 12.2) = 226 days.

So the P80 is 226 days against a plan of 200. The schedule contingency required is 26 days, and 16 of those exist before you add a single risk event, purely because the plan was built from modes.

Two honest caveats. This analytical shortcut assumes the durations are independent and that their sum is normal, and it only works on a single chain. Monte Carlo simulation needs neither assumption, which is why real analysis uses it.

## Why parallel paths make the deterministic date worse, not better

Merge bias is the effect most teams underestimate. When several paths converge on a milestone, the milestone waits for the latest of them, so probabilities multiply rather than average.

Suppose three paths each have a 50% chance of hitting the same handover date, and they are genuinely independent. The chance that all three arrive on time is 0.5 × 0.5 × 0.5 = 0.125.

A milestone with three merging paths at even odds is therefore a one-in-eight date, not a fifty-fifty date. Deterministic CPM cannot show this, because it reports the longest path and stops.

This is also why near-critical paths matter. A path with 10 days of total float and 30 days of duration uncertainty is a critical path that has not declared itself yet.

## Reading the output

| Measure | What it means | What to do with it |
|---|---|---|
| P50 date | Half the iterations finished on or before it | Internal working target; roughly the mean on a symmetric result |
| P80 date | Four iterations in five finished on or before it | Common basis for a committed date and for sizing contingency |
| P90 / P95 date | Reserved for penalty or regulatory exposure | Expensive to hold; justify it rather than defaulting to it |
| Criticality index | Share of iterations in which the activity was on the critical path | Find the paths that only become critical under stress |
| Sensitivity / cruciality | Correlation between an activity's duration and the project finish | Rank where mitigation actually buys days |
| Contingency drawdown curve | Planned consumption of the P80 gap over time | Compare actual drawdown against plan monthly |

The tornado chart is the deliverable that changes behaviour. It ranks activities and risks by their influence on the finish date, and it routinely shows that the loudest risk in the register is not in the top five.

A worked example from the table above makes the point. Activity B contributes 69.4 of the 149.3 total variance, which is 46% of the schedule uncertainty on that chain, so procurement is where mitigation money belongs.

## Setting schedule contingency, and who owns it

Schedule contingency is the difference between the committed date and the deterministic date, and it belongs to a named owner. If nobody owns it, it is consumed silently in the first two months and the recovery argument starts in month five.

The governance rule that works is simple. The project team plans to the P50, the sponsor commits to the P80, and the gap between them is released against defined trigger events rather than against optimism.

Drawdown is then tracked as a curve. If the contingency is falling faster than time is passing, the forecast is already wrong and the monthly report should say so.

This is where schedule risk meets cost risk. Extended time-related costs, preliminaries and site overheads convert delay days into money, so the schedule P80 feeds directly into the cost risk model rather than sitting beside it.

## Where the method fails

The method fails when the network is unsound, and no amount of simulation quality compensates. Ranging a schedule with 40 open ends produces a confident, precise, wrong answer.

It fails when ranges are symmetric. A three-point estimate of 38, 40, 42 says the team believes nothing can go materially wrong, which is a statement about the workshop rather than about the work.

It fails when correlation is ignored. Independent sampling across activities that share a vendor, a crew or a weather window makes the tail far too thin.

It also fails politically. A P80 that arrives after the date already promised to a client tends to be sent back for another look, and the second look is rarely more objective than the first. The defence against this is publishing the model, the ranges and their sources alongside the answer.

## Where this sits in the PCI curriculum

The PCI AI Project Controls Leader (PCL-AI) covers 13 domains across 61 knowledge areas, and quantitative risk sits among them alongside planning, progress measurement and forecasting. The reason for that placement is practical rather than academic.

A schedule risk result that never reaches the cost forecast changes nothing. The P80 date has to arrive as time-related cost in the estimate at completion, and the person carrying the credential should be able to make that conversion without handing it to somebody else.

## Frequently asked questions

**How many iterations should a Monte Carlo simulation run?**
Enough that the answer stops moving. Run 1,000, then 5,000, then 10,000, and compare the P50 and P80 dates each time. When the P80 shifts by less than a day between runs, the sample is stable. More iterations improve precision, never accuracy, so a poor model run 100,000 times is still a poor model.

**Is schedule risk analysis the same as a Monte Carlo simulation?**
No. Monte Carlo is the sampling technique most quantitative schedule risk work uses. The analysis is the whole exercise: fixing the network, evidencing ranges, mapping risks, running the simulation, then interpreting criticality and sensitivity. Teams that buy the tool and skip the first three steps get numbers rather than answers.

**What confidence level should we commit to?**
There is no universal answer, and anyone quoting one is guessing. P80 is a common basis for external commitments because it holds most of the realistic downside without pricing in the extreme tail. Regulated work or heavy liquidated damages can justify P90. What matters more than the number is that the same level is used consistently across the portfolio.

**Can you run a QSRA on a summary schedule?**
Yes, and sometimes you should. A 60 to 150 activity risk model built from the detailed schedule is easier to range honestly, runs faster, and is far easier to explain in a review. The requirement is that the summary preserves the real logic and the real merge points, because merge bias is the effect you are trying to see.

**How often should the analysis be repeated?**
At every gate, before any date is committed externally, and whenever the critical path changes materially. Between those points, track contingency drawdown monthly instead of re-running the model. A simulation repeated without new information produces motion rather than insight, and it trains the audience to ignore the output. New information means new ranges, a new risk, or a genuine change in logic.

**Does the risk register need to be finished first?**
It needs to be current, not complete. Map the risks that have a credible route to the schedule, and accept that duration ranging already carries ordinary variability. A register of 140 rows in which nine could move a date makes a better model than one padded to look thorough.

---

*Internal links: this piece should link to the [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) beginner's guide, [how to run a Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation), [how to build a risk register stakeholders actually use](https://projectcontrolsinstitute.org/risk-register-that-gets-used), the [critical path method](https://projectcontrolsinstitute.org/critical-path-method) definition and the [total float](https://projectcontrolsinstitute.org/total-float) definition, and upward to the [project controls](https://projectcontrolsinstitute.org/what-is-project-controls) pillar.*
