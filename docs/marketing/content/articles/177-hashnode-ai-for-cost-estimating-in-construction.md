---
platform:      Hashnode
type:          guide
title:         AI for cost estimating in construction: measure it first
meta:          Test AI for cost estimating in construction on your own completed packages: MAPE, bias, the correction order, and where the estimate lands in the accounts.
primary_kw:    AI for cost estimating in construction
secondary_kw:  MAPE, estimate bias, cost-to-cost input method, historical cost data
pillar:        AI in project controls
credential:    PFL-AI
target_domain: pciai.org
canonical:     canonical -> pciai.org/ai-for-cost-estimating-in-construction
schema:        Article
word_count:    1603
hashtags:      #python #datascience #machinelearning #tutorial
ab_id:         AB-00046
---

# AI for cost estimating in construction: measure it first

AI for cost estimating in construction prices from historical cost, checks a draft estimate for missing scope against comparable jobs, and benchmarks a bid against what similar work actually cost. It cannot judge whether this job resembles the history it learned from. Before believing any vendor's accuracy claim, compute two numbers on your own completed packages.

Those two numbers are mean absolute percentage error and bias. They take about fifteen lines of Python and they settle most arguments about whether a tool is working.

## Where AI for cost estimating in construction earns its place

An estimate is not one task, and splitting it up shows quickly which parts a model carries.

| Estimating task | What a model does well | Data it needs | How it fails |
|---|---|---|---|
| Quantity take-off | Extracts quantities consistently, at speed | Structured models or clean drawings | Silently misses what is not drawn |
| Pricing from history | Matches items to prior rates, adjusts for time and location | Coded historical cost with outturn, not tender values | Overlooks that the old jobs were unusual |
| Scope completeness | Flags items present on comparable jobs and absent here | A library of comparable estimates | Recommends scope the contract excludes |
| Benchmarking a bid | Places the bid against prior outturns | Outturn cost, not award value | Comparables that are not comparable |
| Escalation | Applies indices consistently across a long programme | Published indices and a supply-market view | Cannot see a shock that has not happened |
| Contingency | Simulates across ranges rapidly | Defensible ranges and correlations | The ranges are judgement; the simulation dresses them up |
| First-of-a-kind work | Very little | There is no history to learn from | Answers confidently anyway |

The right-hand column is the estimating file in miniature. Every one of those failures also happens to human estimators, which is the point: the tool changes the speed and the volume, not the nature of the error.

## Measuring error and bias on your own packages

A vendor's accuracy claim describes their data. What matters is error and bias on yours, and both are computable from jobs you have already finished.

```python
def estimate_quality(pairs):
    """pairs: list of (estimate, outturn) for completed packages."""
    signed = [(e - o) / o for e, o in pairs]          # negative = under-estimated
    mape = sum(abs(x) for x in signed) / len(signed)
    bias = sum(signed) / len(signed)
    return {"mape_pct": round(100 * mape, 1),
            "bias_pct": round(100 * bias, 1),
            "n": len(pairs)}

packages = [(2_400_000, 2_760_000), (5_000_000, 5_150_000),
            (3_200_000, 3_040_000), (8_600_000, 9_460_000),
            (1_500_000, 1_725_000)]
estimate_quality(packages)      # {'mape_pct': 8.7, 'bias_pct': -6.6, 'n': 5}
```

Here is the same calculation as a table, so the code has something to be checked against.

| Package | Estimate | Outturn | Difference | Error against outturn |
|---|---:|---:|---:|---:|
| A | £2,400,000 | £2,760,000 | −£360,000 | 13.0% |
| B | £5,000,000 | £5,150,000 | −£150,000 | 2.9% |
| C | £3,200,000 | £3,040,000 | +£160,000 | 5.3% |
| D | £8,600,000 | £9,460,000 | −£860,000 | 9.1% |
| E | £1,500,000 | £1,725,000 | −£225,000 | 13.0% |

**Mean absolute percentage error** averages those percentages regardless of sign: (13.0 + 2.9 + 5.3 + 9.1 + 13.0) ÷ 5 = **8.7%**. That is the spread to expect on a new package of the same type.

**Bias** is the same average with the signs kept: (−13.0 − 2.9 + 5.3 − 9.1 − 13.0) ÷ 5 = **−6.6%**. Four of five estimates came in under, and the average miss is systematically low.

Those two numbers say different things. MAPE describes how wide your uncertainty is. Bias describes which way you are wrong, and bias is the expensive one because it does not average out across a portfolio.

## The correction order, and the double-count that hides in it

Correct for bias before adding contingency, and do it once.

A new estimate of £8,400,000 carrying a −6.6% bias implies an expected outturn of 8,400,000 ÷ 0.934 = **£8,993,576**, roughly £594,000 above what the estimate says.

A team that applies that correction and then adds the same allowance again as contingency has priced the same risk twice. They will lose bids for a reason nobody in the room can find, because each adjustment looks defensible on its own.

Keep the two steps in separate columns of the estimate summary and label them. Bias correction removes a known systematic error; contingency covers the remaining spread, which is what the 8.7% is telling you.

## Where the estimate lands in the accounts

An estimate does not stop working when the job starts. On a contract where revenue is recognised over time using a cost-to-cost input measure, expected total cost is the denominator of percentage complete, so the estimate drives reported revenue every month.

Take a contract priced at £11,000,000 with £4,200,000 of cost incurred. Using the original £8,400,000 expected cost, progress is 4,200,000 ÷ 8,400,000 = **50.0%** and revenue to date is 0.50 × 11,000,000 = **£5,500,000**.

Using the bias-corrected £8,993,576, progress is **46.7%** and revenue to date is **£5,137,000**. The same work in the same month, and £363,000 of difference decided by an estimating assumption made before the contract was signed.

That is the overlap this Institute exists for. A quantity surveyor who prices well and an accountant who closes cleanly can each do their job perfectly and still publish the wrong number, because the estimate is a finance input and almost nobody is examined on both sides of the handover.

This describes a mechanism. Nothing PCI publishes is legal, tax or accounting advice, and the treatment depends on the contract.

## Four questions for the vendor

Ask what the training data was: tender values or outturn cost. A model trained on what jobs were sold for has learned the market's optimism rather than the cost of building.

Ask for precision and recall on scope-completeness flags, measured on your projects. A checker that raises 200 items to find 40 real ones costs review time that has to be budgeted, and that arithmetic belongs in the business case.

Ask whether it will show its comparables. An estimate that cannot be traced to the jobs behind it cannot be defended to a board, however good the number turns out to be.

Ask what it does when it does not know. A tool that returns a wide range and says why is more useful than one that always returns a confident single figure.

## What no model can estimate

First-of-a-kind work, because there is nothing to learn from and the model will answer anyway.

Novel logistics, such as a constrained city site with one crane position, where the cost driver is a sequencing decision rather than a quantity.

Market conditions that have not yet reached the indices. A model reads history, and a supply shock is by definition not in it yet.

Commercial position, because what a job should cost and what you should bid are different questions and only the first one is arithmetic.

## How PCI examines this

PCI certifies the finance side of project work through the PCI AI Project Finance Leader (PFL-AI), which holds 16 domains and 61 knowledge areas. The controls credential, the PCI AI Project Controls Leader (PCL-AI), holds 13 domains and 61 knowledge areas, and the delivery credential, the PCI Project Management Leader – AI (PML-AI), holds 16 domains and 63 knowledge areas.

Each Body of Knowledge runs in a 40 / 40 / 20 proportion across finance and reporting, project management, and governed AI, so the chain from estimate to expected cost to recognised revenue is examined as one competence rather than two. The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**How accurate is AI cost estimating?**
As accurate as the outturn data behind it, and no more. Take ten completed packages, compute MAPE and bias with the function above, then compare against how your estimators performed on those same jobs. Any accuracy figure quoted without naming the data it was measured on should be ignored.

**How many completed packages do I need before the numbers mean anything?**
Five gives you an indication and hides a lot; twenty or more of the same work type gives you something to plan with. Segment before averaging, because civils and fit-out have different error profiles, and a MAPE computed across both describes neither.

**Can a model replace an estimator?**
No. It compresses take-off, pricing and checking, which is most of the hours, and leaves the judgement about whether this job resembles the history. That judgement is where estimates are won and lost, and it is not present in the data.

**What data do we need before starting?**
Coded historical cost with outturn values rather than tender sums, and a work breakdown stable enough that the codes mean the same thing across jobs. Most contractors have the cost and have lost the coding consistency, and that is the work to do first.

**Should contingency change if we use AI?**
Only if your measured spread has changed. Contingency should follow the MAPE you can demonstrate on your own completed jobs, and it should be set after the bias correction rather than on top of it. A tool that narrows the range is worth a lower allowance; one that simply produces numbers faster is not.

---

*First published on pciai.org; the republishing field in Draft Settings carries the canonical back to it. Treat this version as tag-feed reach rather than as a ranking page.*

*Internal links: this guide should link to [the AI in project controls pillar](https://pciai.org/ai-in-project-controls) with the anchor "how governed AI applies across the controls lifecycle", to [IFRS 15 for construction](https://projectcontrolsinstitute.org/ifrs-15-for-construction) with the anchor "percentage of completion and the cost-to-cost input method", and to [cost control methods that catch overruns early](https://projectcontrolsinstitute.org/cost-control-in-construction) with the anchor "keeping the estimate honest once the work starts".*
