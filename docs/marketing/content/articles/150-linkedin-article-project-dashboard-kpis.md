---
platform:      LinkedIn Article
type:          faq
title:         Which project dashboard KPIs actually earn their place
meta:          A project dashboard needs about nine KPIs, each with an owner, a formula, a threshold and an action. The nine, the earned value arithmetic, and what to cut.
primary_kw:    project dashboard KPIs
secondary_kw:  earned value metrics, contingency drawdown, TCPI, precision and recall
pillar:        Project controls fundamentals
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1827
hashtags:      #ProjectControls #EarnedValue #PMO #AIGovernance
ab_id:         AB-00282
---

# Which project dashboard KPIs actually earn their place

A project dashboard needs about nine indicators, not thirty. Each one needs an owner, a formula, a threshold and a decision it triggers. If a number cannot change what somebody does this week, it is reporting rather than control, and it belongs in the appendix.

Project dashboard KPIs fail for one of two reasons: nobody owns them, or nobody agreed in advance what happens when they turn red.

Written for LinkedIn as an original. It sits under the Institute's project controls fundamentals pillar.

## Which project dashboard KPIs belong on the front page?

Nine, grouped into cost, schedule, delivery and cash. Each row below carries the four things a KPI needs to be more than decoration.

| KPI | Formula | Source | Example threshold | Decision it triggers | Owner |
|---|---|---|---|---|---|
| Cost performance index | EV ÷ AC | Cost ledger and progress measurement | Below 0.95 | Re-forecast the affected packages this period | Cost lead |
| Estimate at completion and variance at completion | EAC by a stated method; VAC = BAC − EAC | Forecast | VAC worse than 2% of BAC | Take the forecast to the change board | Project manager |
| Contingency drawn against progress | Contingency used ÷ contingency at sanction, against per cent complete | Change register | Draw rate ahead of progress | Re-run the risk analysis and reset the release rule | Sponsor |
| Total float on the longest path | Late finish minus early finish, driving path | Schedule | Float negative, or falling three periods running | Mitigation plan for that path, named actions and dates | Planning lead |
| Milestone date variance | Forecast date minus contract date, in days | Schedule | Any contract milestone late | Notice, then recovery or extension of time | Project manager |
| Productivity factor on the largest labour package | Hours earned ÷ hours spent | Timesheets and rules of credit | Below 0.90 | Supervision, access or scope review on that package | Construction manager |
| Rework as a share of direct hours | Rework hours ÷ direct hours | Timesheets and quality records | Rising two periods running | Root cause review on the failing discipline | Quality lead |
| Forecast cash position and the date of the trough | Cash in minus cash out, cumulative | Cash forecast | Trough below the facility available | Billing acceleration, payment terms, funding request | Commercial lead |
| Post-mitigation risk exposure against remaining contingency | Sum of probability × impact, after mitigation | Risk register | Exposure above remaining contingency | Escalate; the project is no longer covered | Risk lead |

Nine rows fit on one page at a readable size. That constraint is doing real work, because a dashboard nobody can read in ninety seconds is not read at all.

## What does the earned value block look like when you work it?

The whole block comes from four numbers, and the arithmetic takes two minutes.

Take **BAC = £10.0m**. At the data date: **PV = £4.20m**, **EV = £3.78m**, **AC = £4.30m**.

Per cent complete is EV ÷ BAC = 3.78 ÷ 10.0 = **37.8%**.

Cost variance is EV − AC = **−£0.52m**. Schedule variance is EV − PV = **−£0.42m**, which is money's worth of work not done rather than time.

Cost performance index is EV ÷ AC = 3.78 ÷ 4.30 = **0.879**. Schedule performance index is EV ÷ PV = 3.78 ÷ 4.20 = **0.900**.

Forecasting the remainder at the performance achieved so far, EAC = BAC ÷ CPI = 10.0 ÷ 0.879 = **£11.38m**, so VAC = 10.0 − 11.38 = **−£1.38m**.

The reality check is TCPI: (BAC − EV) ÷ (BAC − AC) = (10.0 − 3.78) ÷ (10.0 − 4.30) = 6.22 ÷ 5.70 = **1.091**. The remaining work must run 9% better than plan, having run 12% worse than plan so far.

Put TCPI on the dashboard next to the EAC. It converts "we will recover" into a number somebody has to justify, which is the entire purpose of the exercise.

## Why is contingency drawdown the most useful cost KPI?

Because it moves earlier than the cost variance and it is almost never reported properly. Contingency drawn against progress achieved shows whether the cover will last.

Continuing the same project, contingency at sanction is **£1.2m** and **£0.62m** has been released at **37.8%** complete.

That is 0.62 ÷ 1.2 = **51.7% of the cover consumed** for 37.8% of the work. Straight-lining the draw rate gives 0.62 ÷ 0.378 = **£1.64m** required in total, against **£1.2m** available: a shortfall of **£0.44m** with the difficult two-thirds still to come.

Nobody needs a cost variance report to see that. It is visible from the change register in the first third of the job, which is when there is still time to do something about it.

## What belongs on a dashboard but usually is not?

Cash, in the form that shows the trough rather than the balance. A project can report an acceptable margin and still be the reason the group draws on its overdraft.

Show unbilled work, retention held and the forecast low point with its date. Retention sits in someone else's account for months after completion and is rarely visible on any delivery report.

Show pending change alongside approved change. A dashboard showing only approved change reports a control budget everyone in the room knows is out of date.

## If your dashboard raises alerts, it is a classifier — so measure it

Any rule that turns a project amber is a prediction, whether it was written by a person or produced by a model. Predictions get measured with precision, recall and F1, and dashboards almost never are.

Over two quarters, the amber rule flagged **40 work packages**. Of those, **22** did go on to overrun. Across the same period **30 packages overran in total**.

Precision is 22 ÷ 40 = **0.55**: just over half the alarms were real. Recall is 22 ÷ 30 = **0.733**: the rule caught about three-quarters of the overruns.

F1 is the harmonic mean, 2 × (0.55 × 0.733) ÷ (0.55 + 0.733) = 0.807 ÷ 1.283 = **0.63**.

Tighten the threshold and precision rises while recall falls. Loosen it and the opposite happens. Which way to move is a business decision about the cost of a missed overrun against the cost of a false alarm, and it should be made deliberately rather than by whoever set the default.

The same discipline applies to any AI-generated forecast or risk flag on the dashboard: record what it predicted, compare against what happened, and publish the score. Governed AI is 20 of the 40/40/20 proportions in the PCI Bodies of Knowledge, alongside 40 finance and reporting and 40 project management, and the PCI AI Project Controls Leader (PCL-AI) credential examines the governance of automated outputs across 13 domains and 61 knowledge areas.

## What should come off the dashboard?

Anything with no owner. An indicator that belongs to everybody is checked by nobody, and it survives on dashboards for years.

Cumulative counts that only rise: hours worked to date, documents issued, metres installed without a denominator. They feel like progress and measure activity.

Per cent complete without a stated rule of credit. Two teams reporting 60% on different rules are reporting different things, and averaging them produces a number with no meaning at all.

Traffic lights set by opinion. A red that arrives because someone feels uneasy is not comparable across a portfolio, and a green that arrives because a meeting went well is worse. Tie every colour to the threshold in the table, and let the commentary carry the judgement.

## What makes the numbers trustworthy?

One data date across everything. Cost from one date and schedule from another produces a CPI and an SPI that cannot be reconciled, and the first person to notice will be the one you least want to notice.

Stamp the data date on the page and state the method behind the EAC. Change the method only with a note explaining why, because an EAC that improves in the month its method changes is the finding auditors look for.

Agree thresholds before the first report and write them into the reporting procedure. Thresholds negotiated after a number turns red are not thresholds, and everyone involved knows it.

## Frequently asked questions

**How many KPIs should a project dashboard have?**
Roughly nine on the front page, with everything else in supporting packs. The limit is not aesthetic: each KPI needs an owner, an agreed threshold and a defined action, and few organisations sustain more than about ten of those commitments. If a tenth earns a place, something else should lose one.

**Is SPI good enough as a schedule KPI?**
On its own, no. SPI is measured in money's worth of work and drifts towards 1.0 as a project finishes, even a late one, because the remaining value shrinks. Report it alongside total float on the longest path and milestone date variance in days. Those two answer the question a sponsor is actually asking.

**What threshold should turn a KPI red?**
Whatever your organisation can justify and will act on, written down before reporting starts. The example thresholds in the table above are illustrations, not standards. What matters more than the value is that it is set in advance, applied consistently across the portfolio, and attached to an action rather than to a conversation.

**How often should a dashboard be produced?**
Monthly for cost and forecast, weekly for schedule, float and productivity on active construction. Cost data usually cannot be trusted at a weekly cadence because accruals and commitments lag, and a weekly CPI mostly measures invoice timing. Match the frequency to the underlying data, not to the meeting diary.

**Should AI-generated forecasts appear on the dashboard?**
Only with the same evidence you would demand of a person: what the model predicted, what happened, and the precision and recall of those predictions over a meaningful sample. Label the output as model-generated, name the owner who accepts it, and keep the record. An unlabelled model output on a board pack is an unattributable claim.

---

*PCI publishes certification requirements. Nothing here is legal, tax or accounting advice. Every figure in the worked examples above is illustrative arithmetic, not project data, and the example thresholds are illustrations rather than published standards.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Internal links: this article should link to [earned value management](https://projectcontrolsinstitute.org/earned-value-management) as the pillar it supports, to [earned value reporting thresholds](https://projectcontrolsinstitute.org/earned-value-reporting-thresholds) with the anchor "how to set a threshold that triggers an action", and to [project cash flow forecasting](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) with the anchor "forecasting the cash trough and its date".*
