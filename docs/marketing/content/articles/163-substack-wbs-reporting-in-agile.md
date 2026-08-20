---
platform:      Substack
type:          guide
title:         WBS reporting in agile: how to measure hybrid delivery
meta:          WBS reporting in agile works when the WBS holds the money and the backlog holds the sequence. Feature-level earned value, frozen weights and the roll-up trap.
primary_kw:    WBS reporting in agile
secondary_kw:  hybrid delivery, feature-level earned value, story points, output method
pillar:        Planning and scheduling
credential:    PML-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1591
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00105
---

# WBS reporting in agile: how to measure hybrid delivery

WBS reporting in agile works when the WBS holds the money and the backlog holds the sequence. Map every feature to the WBS element carrying its budget, earn value when a feature is accepted, and freeze the relative weights at baseline. Story points then measure team capacity, which is what they are good at, and never value.

*Written first for this newsletter. The hybrid programme below is invented, the arithmetic is not, and it is the roll-up at the end that most reporting packs get wrong.*

## What breaks when agile work meets a work breakdown structure?

Three things, in a predictable order. Points are re-estimated as the team learns, so any budget expressed in points moves under you.

Sprint boundaries do not align with the accounting period. And the backlog is reprioritised, so the plan a variance is measured against no longer exists.

Each of those has a fix that costs almost nothing. Freeze the weights at baseline, report on the calendar month rather than the sprint, and hold the baseline plan separate from the live backlog.

None of this asks the delivery team to change how they work. It asks the reporting layer to stop borrowing the team's units.

## WBS reporting in agile: the mapping that makes it measurable

| Layer | Delivery object | What earns | Who accepts |
|---|---|---|---|
| WBS level 3 | Product or workstream | Nothing directly | Programme manager |
| WBS level 4 | Epic or capability | Sum of its features | Product owner |
| WBS level 5 | Feature | Its baseline weight, at acceptance | Product owner against a definition of done |
| Below the WBS | Story, task, spike | Nothing | The team, on its own board |

The line under level 5 matters more than the levels above it. Stories churn daily and belong to the team; the WBS stops where the churn starts.

A feature is the right earning unit because it is small enough to complete inside a reporting period and large enough to mean something to a sponsor. If features routinely span three months, the breakdown is too coarse to report on and the problem is not agile.

Use a binary earning rule at feature level: accepted or not. Part-earned features reintroduce the argument about per cent complete that agile removed, and features are short enough not to need it.

## Worked example: a hybrid programme at sprint 10

A depot modernisation, total budget £4.80m and all figures illustrative. Civil and installation £3.00m on conventional earned value; control system software £1.80m delivered by two squads over 24 two-week sprints.

The software workstream carries 60 features weighted at baseline to 300 points in total, so budget per point = 1,800,000 ÷ 300 = **£6,000**. Weight is relative size fixed at baseline; velocity points are a separate number the team keeps for itself.

At the end of sprint 10 the baseline acceptance plan says 118 weight points should be accepted. Actual acceptance is 96 points. Cost booked is £760,000, which is ten sprints at £76,000.

Planned value = 1,800,000 × (118 ÷ 300) = **£708,000**.
Earned value = 1,800,000 × (96 ÷ 300) = **£576,000**.

Cost performance index = 576,000 ÷ 760,000 = **0.758**. Schedule performance index = 576,000 ÷ 708,000 = **0.814**.

Forecast at current performance = 1,800,000 ÷ 0.758 = **£2.375m**, a variance at completion of **−£575,000** on a £1.80m workstream.

To-complete performance index against the original budget = (1,800,000 − 576,000) ÷ (1,800,000 − 760,000) = 1,224,000 ÷ 1,040,000 = **1.177**. The squads would have to run 55% better than they have managed, which nobody does, so the conversation at sprint 10 is about scope or money rather than about encouragement.

## Why the programme roll-up hides it

The civil workstream in the same month: planned value £1.40m, earned value £1.35m, actual cost £1.28m. That is CPI 1.055 and SPI 0.964, a workstream performing well.

| Line | PV | EV | AC | CPI | SPI |
|---|---:|---:|---:|---:|---:|
| Civil and installation | £1.400m | £1.350m | £1.280m | 1.055 | 0.964 |
| Control system software | £0.708m | £0.576m | £0.760m | 0.758 | 0.814 |
| **Programme total** | **£2.108m** | **£1.926m** | **£2.040m** | **0.944** | **0.914** |

A programme CPI of 0.944 reads as a small problem. Underneath it, one workstream is forecasting a 32% overrun and the other is subsidising the appearance.

That is the case for WBS-level reporting rather than programme-level reporting, and it applies as much to conventional work as to agile. Report the indices at the level where a manager can act, and let the total be a total rather than a message.

Set an exception threshold per workstream instead of per programme. A 5% variance on £1.8m is worth a paragraph; the same 5% on the programme total is invisible.

## Freezing the weights, and what happens when scope changes

Relative sizing drifts. Teams re-estimate as they learn, and a feature sized at 8 points in month one becomes 5 points in month six because the framework got easier.

If budget is tied to live points, that re-estimation silently reduces the budget of unstarted work and inflates apparent performance. Freeze the baseline weights and let the team's live estimates float free of them.

New scope enters through change control, which adds both features and weight points and re-baselines the total. Dropping a feature releases its weight and its budget in the same movement, so the denominator always reconciles to something a reviewer can trace.

Record three columns per feature: baseline weight, current status, acceptance date. That is the entire data requirement, and it fits in the backlog tool the team already uses.

## What the accountant needs from the same data

Feature acceptance is an output measure, and output measures are the ones a finance team can work with. The five-step model in IFRS 15 runs: identify the contract; identify the distinct promises in it; determine the transaction price including any variable consideration; allocate that price across the promises in proportion to their standalone selling prices; and recognise revenue as each promise is satisfied, over time where the criteria for that are met.

The fifth step is where delivery reporting and the ledger meet. Progress towards satisfying a performance obligation over time is measured either by an input method, typically costs incurred against total expected costs, or by an output method such as units delivered or milestones achieved.

A feature acceptance log with fixed baseline weights is an output measure with an audit trail already attached. That is a better position than most conventional projects manage, provided the weights were fixed before anyone knew which features would be late.

This is the overlap PCI exists to examine. A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy, and almost never on how a delivery team sizes work. An engineer or a product owner is examined on flow and acceptance, and almost never on cut-off.

The money is lost in between, and on hybrid programmes it is lost twice, because neither side recognises the other's units.

Nothing here is accounting advice. Which measure faithfully depicts transfer of control is a judgement for the reporting entity and its auditor, and the point of the reporting design above is to make that judgement possible rather than to make it for anyone.

The PCI Project Management Leader – AI (PML-AI) syllabus covers 16 domains and 63 knowledge areas, and hybrid delivery measurement sits across the planning and reporting domains rather than in a chapter of its own.

## Frequently asked questions

**Can you use earned value on a purely agile product team?**
You can, and it is often not worth it. Earned value answers whether an agreed scope is being delivered for an agreed budget by an agreed date. A continuously funded product team with a rolling backlog has no fixed scope, so a burn-up chart and a cost-per-period figure tell the whole story. Use earned value where a scope has been committed to a sponsor.

**How do you handle a sprint that spans a month end?**
Report on the calendar month and earn only features accepted by the cut-off date. A feature accepted on the second of the month belongs to that month, not to the sprint that produced it. Trying to align sprints to accounting periods breaks the team's cadence and gains nothing the cut-off rule does not already give you.

**Do story points work as a budget unit if we never re-estimate?**
In theory yes, and in practice teams do re-estimate, because that is how relative sizing improves. The safe arrangement is two numbers with different jobs: frozen baseline weights for value, and live velocity points for capacity and forecasting. They start identical and diverge, and that divergence is information rather than an error.

**What level should features sit at in the WBS?**
Low enough that most finish inside one reporting period and high enough that a sponsor recognises the name. On a £1.8m workstream, 60 features gives an average of £30,000 each, which is a sensible granularity: fine enough to earn monthly, coarse enough that maintaining the register is not a job.

**How do you forecast a hybrid programme when one side is fixed and the other is not?**
Forecast the workstreams separately and add them, never blend the indices first. The civil work forecasts from CPI on a fixed scope; the software work forecasts from feature throughput against remaining weight, which is a different calculation with a different error. Adding two honest forecasts beats one index built from an average of unlike things.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so nothing here duplicates a page the PCI site needs to rank.*

*Internal links: this piece should link to [what is earned value management](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "the earned value mechanics behind these indices", to [IFRS 15 for construction contracts](https://projectcontrolsinstitute.org/ifrs-15-for-construction) with the anchor "output methods and how progress is measured for revenue", and to [project performance management](https://projectcontrolsinstitute.org/project-performance-management) with the anchor "reporting at the level a manager can act on".*
