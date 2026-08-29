---
platform:      Own site — projectcontrolsinstitute.org
type:          pillar
title:         Project budgeting and forecasting: an end-to-end guide
meta:          Project budgeting and forecasting end to end: sanction estimate to authorised baseline, plus the four EAC methods worked through on a GBP 10m project.
primary_kw:    project budgeting and forecasting
secondary_kw:  estimate at completion, performance measurement baseline, management reserve, TCPI
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    2,633
hashtags:      []
ab_id:         AB-00095
---

# Project budgeting and forecasting: an end-to-end guide

Project budgeting and forecasting are two different jobs. Budgeting converts an approved estimate into a time-phased baseline that work is measured against. Forecasting states, from evidence gathered since, where the project will actually land. The baseline should change rarely and only through change control; the forecast should change every month.

## Project budgeting and forecasting are two jobs, not one

A budget is an authorisation. It says what may be spent, on what scope, and it is fixed at a point in time by somebody with the authority to fix it.

A forecast is a prediction. It says what will be spent, given what is now known, and it carries no authority at all.

Confusing the two is the most common cost control failure on a capital project. A team that revises the budget every time the forecast moves has destroyed its own measurement baseline and can no longer state a variance.

| | Budget | Forecast |
|---|---|---|
| Question answered | What was authorised? | Where will this land? |
| Changes | Only through formal change control | Every reporting period |
| Owner | The sponsor or investment committee | The project controls team |
| Basis | The sanction estimate plus approved changes | Actuals, commitments, performance and remaining risk |
| Audit trail | Change log | Forecast basis statement |
| Time horizon | Fixed at sanction | Rolling to completion |

## From estimate to authorised budget

The budget starts as an estimate, and an estimate carries a class. Classes run from the earliest concept work, where scope definition is minimal and the estimate is built from capacity factors or analogues, through to a fully defined estimate built from quantities and priced rates.

The important property is that expected accuracy narrows as definition matures, and the range is a function of what is known rather than of how much effort went into the spreadsheet. A detailed bottom-up build on 15% design definition is a precise expression of an uncertain scope.

Sanction is where the estimate becomes money. At that point the estimate is split: base cost, escalation, contingency for identified risks, and management reserve for what has not been identified.

The split matters because those pots have different owners and different release rules. Contingency belongs to the project and is drawn down against defined events. Management reserve sits outside the measurement baseline and is released by the sponsor.

## Structure: the part that decides whether control is possible

A budget that cannot be compared to progress is decoration. The structure that makes comparison possible has three layers.

The **work breakdown structure** decomposes the scope until every element has a deliverable, an owner and a way to measure completion. The **cost breakdown structure** classifies spend by resource type so that a labour overrun is visible as a labour overrun.

The **control account** is where the two meet. It is the lowest level at which budget, schedule and responsibility come together, and it is the level at which variance is explained.

Below the control account sit work packages, and beyond the near-term horizon, planning packages for scope that is real but not yet detailed. Rolling wave planning converts planning packages into work packages as definition arrives.

## Building the baseline

The performance measurement baseline is the time-phased sum of all control account budgets, plus any undistributed budget for authorised scope not yet allocated to a control account.

Add management reserve to that and you have the total allocated budget, which should reconcile to the contract or sanction value. If it does not reconcile, one of the two documents is wrong and the answer is not to plug the difference.

| Element | Sits inside the baseline? | Released by | Typical trigger |
|---|---|---|---|
| Control account budgets | Yes | Already allocated | n/a |
| Undistributed budget | Yes | Project controls, on definition | Scope allocated to a control account |
| Contingency for identified risks | Yes, held separately within it | Project manager | A registered risk materialises |
| Management reserve | No, held above it | Sponsor or investment committee | Unforeseen scope or a step change in exposure |
| Escalation allowance | Yes | Project manager | Index movement against the estimate basis |

Time-phasing turns the total into a curve. Each control account is spread across its activities, resource-loaded where the schedule supports it, and the sum is the familiar S-curve of cumulative planned cost.

That curve is the planned value line for earned value management, so a poor schedule produces a poor baseline no matter how good the estimate was.

## Three ledgers that never agree, and should not

Anyone forecasting a project needs to know which number they are holding.

**Committed** cost is the value of purchase orders and subcontracts placed, whether or not any work has been done. **Incurred** or accrued cost is the value of work actually performed, whether or not it has been invoiced. **Paid** cost is cash that has left the account.

A single package can read £2.4m committed, £1.5m incurred and £1.1m paid on the same day, and all three are correct. Earned value analysis uses the incurred figure, cash flow forecasting uses the paid figure, and commitment cover uses the committed figure.

Reporting the wrong one is how a project appears £900,000 underspent in month six and £900,000 overspent in month seven, having done nothing differently in either.

## Measuring what has been earned

The forecast depends on knowing how much work is complete, and percentage estimates supplied by the people doing the work are the weakest available input.

Rules of credit fix this. Each work package type gets a fixed schedule of milestones with fixed credit, so a piping spool is 20% at material release, 45% at fabrication complete, 75% at erection, 95% at test and 100% at handover.

The engineer no longer offers an opinion. They state which milestones are achieved, and the credit follows.

Earned value is then the budget for the work actually completed. It is a measurement of work, expressed in currency, and it is not a measurement of money spent.

## Forecasting: the four EAC methods, worked

Take a project with a budget at completion of £10,000,000, reporting at month eight.

Planned value is £4,000,000, earned value is £3,600,000, and actual cost is £4,200,000.

The variances come first. Cost variance is EV − AC = £3,600,000 − £4,200,000 = **−£600,000**. Schedule variance is EV − PV = £3,600,000 − £4,000,000 = **−£400,000**.

The indices normalise those. Cost performance index is EV / AC = 3,600,000 / 4,200,000 = **0.857**. Schedule performance index is EV / PV = 3,600,000 / 4,000,000 = **0.90**.

Read plainly: every pound spent has bought 85.7 pence of work, and the project has completed 90% of the work it planned to have completed by now.

Work remaining, measured in budget, is BAC − EV = £10,000,000 − £3,600,000 = £6,400,000. Every forecasting method is an argument about what that £6,400,000 will actually cost.

| Method | Formula | What it assumes | This project |
|---|---|---|---|
| 1. Remaining work at budget | EAC = AC + (BAC − EV) | The overrun was a one-off. Past performance will not repeat | £4.2m + £6.4m = **£10,600,000** |
| 2. Remaining work at current CPI | EAC = BAC / CPI | Cost performance to date is the best available predictor of the rest | £10m / 0.857 = **£11,666,667** |
| 3. Remaining work at CPI and SPI | EAC = AC + (BAC − EV) / (CPI × SPI) | Schedule pressure will keep damaging cost efficiency | £4.2m + (£6.4m / 0.771) = **£12,496,296** |
| 4. Bottom-up estimate to complete | EAC = AC + fresh ETC | Neither history nor indices apply, because the remaining work differs from the work done | £4.2m + £7.1m = **£11,300,000** |

The spread is £1.9m, which is 19% of the original budget, on identical data. The method is not a detail. Which formula answers which cause of variance, and how to defend the one you signed, is worked method by method in [how to run all four EAC formulas and pick one](https://projectcontrolsinstitute.org/four-eac-formulas).

Method one is the optimist's answer and is only defensible when you can name the one-off event and show it is closed. Method two is the default in most organisations because it is quick, objective and hard to argue with.

Method three produces the highest number and assumes the schedule recovery will be bought with money. It is honest on a project with liquidated damages and acceleration on the horizon, and pessimistic on one where the schedule slip is caused by a permit nobody can accelerate.

Method four is the only one that reflects a genuine change in the nature of the remaining work. It is also the most expensive to produce, and it invites the optimism the indices were protecting you from, so it needs the same rules of credit and the same basis discipline as the original estimate.

Variance at completion follows from whichever you pick. VAC = BAC − EAC, so method two gives £10,000,000 − £11,666,667 = **−£1,666,667**.

## TCPI: the question the forecast has to survive

The to-complete performance index asks what efficiency is required from here to hit a given target.

To still land on the original budget: TCPI = (BAC − EV) / (BAC − AC) = £6,400,000 / £5,800,000 = **1.103**.

The project has run at 0.857 for eight months and would now have to run at 1.103 for the remainder. That is a 29% improvement in cost efficiency, sustained, with no plan attached to it.

Against the method two forecast the same calculation reads (BAC − EV) / (EAC − AC) = £6,400,000 / £7,466,667 = **0.857**, which is exactly current performance. That consistency is the point: a forecast is credible when the efficiency it implies is one the project has actually demonstrated.

Any forecast requiring a TCPI more than about 5% above demonstrated performance needs a named recovery plan with owners and dates, or it is a wish.

## Working capital: the forecast finance actually asks about

A profitable project can still consume more cash than the business has. The cash conversion cycle is the standard measure, and it works on projects with the terms renamed.

Cash conversion cycle = days work is held before it is billed + days from application to cash received − days taken to pay suppliers.

Take 30 days from work performed to application submitted and certified, 75 days from application to cash, and 45 days of supplier payment terms. The cycle is 30 + 75 − 45 = **60 days**.

On a business turning over £60m a year, that is 60 / 365 × £60,000,000 = **£9.9m** of working capital funded by the contractor at any moment. Extend certification by 15 days and the funding requirement rises to £12.3m without a single extra pound of cost.

This is why the time-phased forecast matters as much as the total. The number at the bottom of the cost report is a solvency question before it is a profitability one, and turning the baseline into dated receipts and payments is its own exercise, set out in [how a cash flow forecast is built from payment terms](https://projectcontrolsinstitute.org/project-cash-flow-forecasting).

## Why the forecast is a financial statement number

A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy. An engineer is examined on float and progress measurement. Neither examination covers the handover between them, and that handover is where the money is lost.

Here is the mechanism, on the numbers above. Under cost-to-cost measurement, progress equals costs incurred divided by forecast total costs. Moving the forecast from £10.0m to £11.67m cuts measured progress from 42% to 36% at unchanged actual cost, and cumulative revenue falls with it.

If the same movement takes forecast cost above contract value, the contract is onerous and the whole expected loss is recognised at once rather than spread across the remaining work. [Which reporting standards consume a cost forecast](https://projectcontrolsinstitute.org/ifrs-for-project-controls) — revenue, provisions, capitalisation, borrowing costs and leases — is worth knowing before the month it happens.

So the forecast a cost engineer signs is not a management estimate that finance later interprets. It is the input to a reported number, and the month it moves is the month the profit moves.

The PCI AI Project Finance Leader (PFL-AI) is built around that handover, covering 16 domains across 61 knowledge areas, with the Body of Knowledge weighted 40% to finance and reporting, 40% to project management and 20% to governed AI.

## The monthly cycle that works

Freeze the data date and stick to it, because a cost report built on three different cut-offs cannot be reconciled to anything.

Collect actuals and accruals, update progress against rules of credit, calculate the indices, then produce the forecast. Forecast last, so that the arithmetic informs the judgement rather than the judgement selecting the arithmetic.

Write a forecast basis every month: what changed, why, which method was used, and what would have to be true for the number to be wrong. Three paragraphs is enough, and it is the single highest-value document in the cost report.

Then compare this month's forecast with last month's and explain the delta. A forecast that never moves is not being maintained, and one that moves without explanation is not being controlled.

## Frequently asked questions

**How often should the estimate at completion be updated?**
Monthly at minimum, and immediately when a material event occurs. A forecast that only moves at quarter end tends to move in large, unwelcome steps, because the underlying deterioration was visible for weeks before it was reported. Small monthly movements with written reasons are easier to act on and far easier to defend afterwards.

**Which EAC method should we use as standard?**
Publish one as the default, usually BAC / CPI, and require any departure to be justified in writing. Run at least two methods every month and report the range, because the gap between them is information. Where they diverge sharply, the reason is usually that the remaining work genuinely differs from the work completed.

**What is the difference between contingency and management reserve?**
Contingency covers identified risks, sits inside the performance measurement baseline, and is drawn down by the project against defined triggers. Management reserve covers what was not identified, sits outside the baseline, and is released by the sponsor. Merging them removes the sponsor's visibility of how much cover the project has already consumed.

**Can you forecast without earned value management?**
Yes, by rebuilding the estimate to complete from the bottom up each period, but it is slower and more exposed to optimism. Earned value gives an objective efficiency measure that a bottom-up forecast has to argue against. The strongest cost reports carry both: indices for the trend, a bottom-up estimate to test it.

**What causes the hockey stick forecast?**
A forecast that shows current underperformance recovering to plan in the final months, with no named actions. It appears when a team is unwilling to report a variance before it is certain. The test is TCPI: if the required efficiency exceeds anything the project has achieved, and no recovery plan exists, the curve is a hope rather than a forecast.

**Should the baseline ever be reset?**
Rarely, and only through formal change control with the sponsor's approval. Re-baselining is a governance act, because it erases the variance history that explains how the project got here. Where it is genuinely warranted, such as a major scope change, keep the original baseline visible alongside the new one.

---

*Internal linking note: three same-domain links are now in the body. [How to run all four EAC formulas and pick one](https://projectcontrolsinstitute.org/four-eac-formulas) sits under the forecasting table, where a £1.9m spread on identical data raises the question of which method to sign. [How a cash flow forecast is built from payment terms](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) sits in the working capital section, where the cycle has to become a dated profile. [Which reporting standards consume a cost forecast](https://projectcontrolsinstitute.org/ifrs-for-project-controls) sits where a forecast above contract value triggers a provision. The EAC accounting and cost control pieces were dropped to hold the two-to-three internal cap, and the IFRS pillar covers the same handover more directly here. Reciprocal link worth making: the four EAC formulas and cash flow forecasting pieces should point back here with the anchor "how a budget becomes a baseline and a forecast".*
