---
platform:      Hashnode
type:          guide
title:         Quantitative schedule risk analysis (QSRA) in 20 lines
meta:          A code-first guide to quantitative schedule risk analysis (QSRA): ten iterations by hand, a Monte Carlo in 20 lines of Python, and how to read a P80 date.
primary_kw:    quantitative schedule risk analysis QSRA
secondary_kw:  Monte Carlo simulation, criticality index, merge bias, P80 date
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/quantitative-schedule-risk-analysis
schema:        Article + FAQPage
word_count:    1798
hashtags:      #python #datascience #statistics #tutorial
ab_id:         AB-00081
---

# Quantitative schedule risk analysis (QSRA) in 20 lines

Quantitative schedule risk analysis (QSRA) replaces one finish date with a distribution of dates and the confidence attached to each. You give every activity a low, likely and high duration, map risk events onto the activities they would hit, then recalculate the network thousands of times. The output is a confidence level, such as a P80 date.

The intimidating part is the vocabulary rather than the maths. This piece runs ten iterations by hand, then the same model in twenty lines of Python so you can check one against the other.

## What is quantitative schedule risk analysis (QSRA)?

It is the practice of testing a programme against uncertainty instead of accepting its dates, and it is the quantitative grade of a wider discipline: [which grade a review actually needs](https://projectcontrolsinstitute.org/schedule-risk-analysis) is worth settling before anyone opens a tool. Deterministic critical path method returns one answer, calculated from durations treated as facts.

They are not facts. Every duration is an estimate with a range behind it, and a QSRA samples that range repeatedly to produce a distribution of finish dates, from which any confidence level can be read.

## The vocabulary, in one table

| Term | What it means | What it is for |
|---|---|---|
| Iteration | One pass sampling every duration and recalculating the network | The unit of a simulation |
| Distribution | The shape describing how likely each duration is | Makes three estimates samplable |
| P50 / P80 / P90 | The date by which that share of iterations had finished | Reading the answer at a confidence |
| Criticality index | Share of iterations in which an activity drove the finish | Finding paths critical under stress |
| Merge bias | Delay caused by a milestone waiting for several paths | Why the deterministic date flatters |
| Risk driver | A register risk with probability and impact, mapped to the activities it hits | Modelling discrete events honestly |
| Schedule contingency | The gap between the deterministic and committed dates | What the analysis exists to size |

## Ten iterations by hand

Two paths converge on a handover milestone. Path X is most likely 46 days, ranging 40 to 64. Path Y is most likely 48 days, ranging 44 to 58.

Deterministic CPM puts the milestone at day **48**, because Y is longer. X carries two days of total float and therefore looks, in a bar chart, like somebody else's problem.

Sample ten times, taking the later of the two draws each time, because the milestone waits for both.

| Iteration | Path X | Path Y | Milestone | Driven by |
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

**The mean** is 523 ÷ 10 = **52.3 days**, four days later than the deterministic 48. **The P50** is the midpoint of the fifth and sixth values, (51 + 52) ÷ 2 = **51.5 days**. **The P80** is around the eighth value, **55 days**, so contingency is seven days.

**The deterministic date held once in ten.** Only iteration 6 finished on or before day 48, so the date that looked settled has roughly a one-in-ten chance in this model.

**Path X drove the milestone three times** despite carrying float in the plan. A path with float and a wide range is a critical path that has not declared itself.

## The same model in twenty lines

Ten iterations are too few to trust. The code below is what a tool does, repeated until the answer settles.

```python
import random, statistics

def simulate(paths, n=10_000, seed=7):
    """paths: {name: (low, likely, high)} converging on one milestone."""
    rng = random.Random(seed)
    finishes, driver_counts = [], {p: 0 for p in paths}
    for _ in range(n):
        draws = {p: rng.triangular(lo, hi, mode)          # low, high, mode
                 for p, (lo, mode, hi) in paths.items()}
        driver = max(draws, key=draws.get)                # the merge point
        driver_counts[driver] += 1
        finishes.append(draws[driver])
    finishes.sort()
    pick = lambda q: finishes[int(q * n) - 1]
    return {"mean": round(statistics.mean(finishes), 1),
            "p50": round(pick(0.50), 1),
            "p80": round(pick(0.80), 1),
            "criticality": {p: round(c / n, 2)
                            for p, c in driver_counts.items()}}

simulate({"X": (40, 46, 64), "Y": (44, 48, 58)})
```

Running that gives a mean of 52.4 days, a P50 of 52.1 and a P80 of 55.5, with criticality of 0.47 for X and 0.53 for Y. The hand table got close on the dates and badly underestimated how often X drives the milestone, which is exactly what ten samples do.

Convergence is worth watching rather than assuming:

| Iterations | P50 | P80 | Criticality of X |
|---:|---:|---:|---:|
| 1,000 | 52.1 | 55.3 | 0.44 |
| 5,000 | 52.1 | 55.5 | 0.48 |
| 10,000 | 52.1 | 55.5 | 0.47 |
| 50,000 | 52.0 | 55.5 | 0.47 |

The P80 settles by about 5,000 iterations here. Stop when it moves by less than a day, and record the number you used.

## Merge bias, in one line of probability

Merge bias is why the deterministic date is optimistic, and it needs no simulation to see.

If two independent paths each have a 50% chance of finishing on time, the milestone waiting for both is on time with probability 0.5 × 0.5 = **0.25**. Add a third such path and it falls to 0.125.

Deterministic CPM cannot represent that, because it takes the longest path and ignores how close the others were.

## Choosing a distribution

The distribution turns three numbers into something samplable. Triangular is a reasonable default.

| Distribution | What it assumes | Use it when | Where it misleads |
|---|---|---|---|
| Triangular | Straight lines from low to likely to high | You have three estimates and little else | Weights the tails more than most real work |
| PERT / beta | A smooth curve weighting the likely case four times | The likely value is genuinely well known | Understates the tail when optimists set the range |
| Uniform | Every value between the bounds equally likely | You know only the bounds | Almost always too pessimistic in the middle |
| Lognormal | Right-skewed with no upper bound | Durations that run away, such as commissioning | Needs data, and produces alarming outliers |
| Discrete | The event happens or it does not | Permits, factory tests, vendor failure | Often modelled as a range, not an event |

The choice matters less than the honesty of the inputs. A triangular distribution over evidenced ranges beats a careful curve over guessed numbers, and a symmetric range of 38, 40, 42 says nothing can go materially wrong, which is a statement about the workshop rather than the work.

## Your first QSRA, in seven steps

**One, fix the network first.** Open ends, hard constraints and negative lags all distort the answer, and the simulation will not warn you. A network with 40 open ends produces a precise, confident, wrong result.

**Two, build a risk model rather than a copy of the schedule.** Sixty to 150 activities preserving the real logic and merge points range more honestly than 4,000 lines.

**Three, take ranges from evidence.** Ask what the fastest and slowest comparable job took before showing anyone the plan.

**Four, map risk events separately.** A permit refusal is an event with a probability and an impact, not ordinary variability.

**Five, set correlation deliberately.** One late vendor hitting nine fabrication activities is one event, not nine, and independent sampling makes the tail far too thin.

**Six, run until the answer settles**, using the convergence table above rather than a habit of 1,000 iterations.

**Seven, publish the model with the answer.** A P80 with no visible inputs gets sent back.

## Three mistakes beginners make

**Treating the most likely duration as the average.** It is the mode. On a right-skewed distribution the mean sits later, and summing modes along a chain compounds the error at every link.

**Double-counting.** A risk modelled as a driver and also baked into a pessimistic duration is counted twice, and whoever finds it will argue the contingency away.

**Running it once and filing it.** Repeat the analysis when the logic changes, and track contingency drawdown monthly in between.

## Turning days into money

A P80 date that never reaches the cost forecast changes nothing. Those seven days have to arrive as time-related cost in the estimate at completion and in the cash profile.

At site costs of £9,000 a day, seven days is 7 × 9,000 = **£63,000** that belongs in the forecast rather than in a register nobody prices.

Delivery produces the days and finance carries the consequence. They are the same event read twice, which is the overlap this Institute exists for.

## Where this sits in the PCI curriculum

The PCI AI Project Controls Leader (PCL-AI) covers 13 domains across 61 knowledge areas, with quantitative risk alongside planning, progress measurement and forecasting. The Body of Knowledge runs in a 40 / 40 / 20 proportion across finance and reporting, project management, and governed AI, so the conversion from P80 days to forecast cost is examined rather than assumed.

## Frequently asked questions

**Is a QSRA the same thing as a Monte Carlo simulation?**
No. Monte Carlo is the sampling technique, the loop in the code above. The analysis is the whole exercise: fixing the network, evidencing ranges, mapping risks, setting correlation and interpreting criticality. Teams that buy the tool and skip those steps get numbers rather than answers.

**How many activities should the risk model have?**
Enough to preserve the logic and the merge points, and few enough that every duration can be defended. Sixty to 150 is a common working range on a major project. A model at full schedule detail cannot be ranged honestly, so its inputs end up guessed.

**What confidence level should we commit to?**
There is no universal answer and anyone quoting one is guessing. P80 is a common basis for external commitments because it holds most of the realistic downside without pricing the extreme tail. Regulated work can justify P90, and consistency across a portfolio matters more than the level.

**What if the P80 is later than the date we have already promised?**
Then the analysis has found the problem it exists to find, and the honest options are scope, resource, sequence or the promise. What does not work is sending the model back to be re-ranged until it agrees. Publishing the inputs is the defence, agreed before the first run.

---

*First published on projectcontrolsinstitute.org; this Hashnode version is flagged as republished with the canonical pointing to the original.*

*Internal links: one is now in the body. "Which grade a review actually needs" points at projectcontrolsinstitute.org/schedule-risk-analysis, placed in the definition section, because that sentence raises the qualitative-against-quantitative choice this piece assumes has already been made. The Monte Carlo cost simulation and total float links proposed earlier were dropped to hold one link per domain; the risk-register how-to carries the Monte Carlo link instead, which spreads the anchors rather than repeating them. No second domain earns a link here. Reciprocal: the schedule risk analysis pillar already points at this page for what building a QSRA involves, so the pair is complete and needs nothing added.*
