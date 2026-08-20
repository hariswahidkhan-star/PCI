---
platform:      Hashnode
type:          guide
title:         Cost control in construction: catching overruns early
meta:          Cost control in construction that catches an overrun at order placement, not at invoice: commitment control, earning rules, cut-off and a worked package.
primary_kw:    cost control in construction
secondary_kw:  commitment control, rules of credit, accrual cut-off, cost report
pillar:        Cost control and estimating
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/cost-control-in-construction
schema:        Article + FAQPage
word_count:    1,675
hashtags:      #datascience #finance #tutorial #productivity
ab_id:         AB-00079
---

# Cost control in construction: catching overruns early

Cost control in construction catches overruns early only when it compares committed cost against budget rather than invoices against budget. A piling subcontract placed at £2.30m against a £2.00m budget is a £300,000 overrun on the day it is signed, months before the first valuation is certified.

Everything below is about moving that detection point earlier, and about where the signal gets lost on the way to the report. The figures are illustrative; the method is not.

## The four numbers a cost report has to carry

Cost control is the practice of comparing what a project has earned against what it has committed and spent, at a fixed cut-off, and acting on the difference before the difference becomes a fact.

Four numbers carry the whole job. Most cost reports carry two of them well and two of them badly.

| Number | What it answers | Where it comes from | Signal latency |
|---|---|---|---|
| Budget (BAC) | What we said this would cost | The sanctioned estimate, mapped to control accounts | Fixed at sanction |
| Commitment | What we have already promised to pay | Placed orders and subcontracts, at order value | Immediate |
| Actual cost (AC) | What has been consumed to the cut-off | Invoices plus accruals for work performed | One period, if accruals are honest |
| Earned value (EV) | What the work done was worth at budget rates | Measured progress under a published earning rule | One period |

A report built on invoices alone answers none of those on time. It answers a different question: what the finance system happened to process before somebody closed the ledger.

Commitment is the cheapest of the four to capture and the one most often missing from the data model. It arrives from the procurement system, not the ledger, which is usually why it never reaches the report.

## Cost control in construction: which methods see the money first

Detection time is the axis that matters. A method that finds a £400,000 problem at practical completion is a historian, not a control.

| Method | What it detects | Earliest signal | Where it fails |
|---|---|---|---|
| Commitment control | Order value above the budget it draws on | The day the order is placed | Blind to scope not yet ordered |
| Trend and change register | Change that is likely but not yet approved | When somebody raises the trend | Dies quietly if only approved change is logged |
| Productivity and unit rates | Hours or output per unit drifting off estimate | Weekly, during execution | Needs quantity measurement people trust |
| Earned value | Work done worth less than the money spent on it | Each reporting cut-off | Flattered by weak earning rules |
| Forecast (EAC) trending | A forecast that rises every month | Month over month | Late if the forecast only moves when it is safe to move it |
| Cash and payment variance | Money leaving faster than planned | After certification | A cash signal is a cost signal that already happened |

The top two rows are the ones site teams run informally and nobody minutes. Formalising them is the cheapest control improvement available on most projects.

## Worked: one package, four detection points

A piling package. The estimate is 1,000 piles at £2,000 each, so the budget is **£2.00m**.

**Detection point one, at order.** Tenders return at £2,300 per pile and the order is placed at **£2.30m**. Commitment variance is 2.30 − 2.00 = **−£0.30m**, known on signature day.

**Detection point two, at the cut-off.** By month four, 420 piles are complete. EV = 420 × 2,000 = **£840,000**. Actual cost is 420 × 2,300 = £966,000 plus £40,000 of standing time, so AC = **£1,006,000**.

CV = 840,000 − 1,006,000 = **−£166,000**, and CPI = 840,000 ÷ 1,006,000 = **0.835**.

**Detection point three, in the forecast.** EAC = BAC ÷ CPI = 2,000,000 ÷ 0.835 = **£2.395m**, so VAC = 2.000 − 2.395 = **−£0.395m**. The efficiency needed from here to still land on budget is TCPI = (2,000,000 − 840,000) ÷ (2,000,000 − 1,006,000) = 1,160,000 ÷ 994,000 = **1.167**, against 0.835 achieved.

**Detection point four, in the ledger.** The invoices arrive, are certified, and the overrun reaches the board as news.

```python
budget, unit_budget, unit_ordered = 2_000_000, 2_000, 2_300
committed = 1_000 * unit_ordered
print(committed - budget)                      # -300000 on signature day

placed, standing_time = 420, 40_000
ev = placed * unit_budget                      # 840_000
ac = placed * unit_ordered + standing_time     # 1_006_000
cpi = ev / ac                                  # 0.8350
print(budget / cpi, (budget - ev) / (budget - ac))   # 2_395_238  1.1670
```

The commitment signal was four months earlier than the earned value signal and larger than it. That is the whole argument for commitment control, in one package.

Choosing between forecasting methods is a separate judgement, worked in full in [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas). Here the £300,000 is a contracted rate, so it repeats on every remaining pile, and any method assuming the overrun was a one-off is wrong.

## Earning rules decide whether the progress is honest

They decide it before any arithmetic happens. Earned value is only as truthful as the rule that says when a unit of work may be claimed.

Take a £600,000 engineering package of 25 drawings, so £24,000 per drawing. Twelve are issued for review and five are approved.

Under a 0/100 rule that credits only approval, EV = 5 × 24,000 = **£120,000**. Under a rule crediting 50 per cent at issue and 50 per cent at approval, EV = (12 × 0.5 × 24,000) + (5 × 0.5 × 24,000) = **£204,000**.

Eighty-four thousand pounds of difference, and no difference in work. Publish the rule in the cost control procedure, apply it to every package, and never let a package change its rule mid-project.

## The cut-off is where construction overruns hide

The delivery side does not check cut-off and the finance side does not check the earning rule, so the seam between them goes unexamined by both.

A package reports AC of £4.60m against EV of £5.00m, so CPI reads 5.00 ÷ 4.60 = **1.087** and the package looks 8.7 per cent under. Then three cut-off items surface: £0.85m of subcontract work performed and not yet invoiced, £0.22m invoiced in the period for plant delivered after cut-off, and £0.09m of prepaid site insurance covering next period.

Corrected AC = 4.60 + 0.85 − 0.22 − 0.09 = **£5.14m**. Corrected CPI = 5.00 ÷ 5.14 = **0.973**.

The package crossed from comfortably under to overspending on accrual discipline alone, with no new transaction and no new work.

A cost engineer who never tests cut-off and an accountant who never tests the earning rule can both sign that report, and both be wrong. That overlap is why the PCI AI Project Controls Leader (PCL-AI) examines reporting and delivery in one credential across 13 domains and 61 knowledge areas, rather than assuming somebody else covers the other half.

## What turns a variance into an action

A threshold needs two limbs, because one alone breaks. A percentage limb alone escalates trivial money on small accounts; an absolute limb alone ignores a 40 per cent overrun on a package that is small today and large later.

Set both, publish them, and escalate on trend rather than on a single period. Three consecutive months of deterioration at 3 per cent is a worse signal than one month at 8 per cent.

Every escalation needs a named owner, a date and a cause phrased as an event. "Piling rates re-tendered 15 per cent above estimate" is a cause. "Alignment to latest view" is an admission that nobody knows.

## Where cost control fails, honestly

It fails when the budget was never an estimate, only a number that made the business case work. No control system recovers from that; it reports the gap slowly.

It fails when contingency is drawn without a record, so packages appear on budget while the provision empties. Track drawdown against progress: a project 40 per cent complete that has spent 65 per cent of its contingency is forecasting a shortfall whether or not anybody has said so.

It fails when the person who owns the forecast is judged on the forecast. Separate the estimate from the performance conversation, or expect the estimate to arrive shaped by the conversation.

## Frequently asked questions

**What is the difference between cost control and cost reporting?**
Reporting states the position; control changes it. A cost report that arrives three weeks after cut-off with no threshold, no owner and no action date is a monthly history lesson. The test is simple: name one decision that was made differently because of last month's report. If nobody can, the function is reporting.

**How early can a construction overrun really be detected?**
At commitment, for anything bought, which on most construction budgets is the larger part of the value. For self-performed work the earliest reliable signal is productivity against estimate, measured weekly. Both are available long before earned value, and far before an invoice.

**Does earned value work on a construction site?**
It works where quantities are measurable and earning rules are agreed in advance, which covers most civil, structural and repetitive packages. It works badly on design, commissioning and anything where progress is claimed by opinion. The answer is not to abandon it but to restrict it to work you can measure and use unit rates elsewhere.

**Should the cost report use committed or actual cost?**
Both, in adjacent columns, with the budget and the forecast. Commitment shows exposure, actual cost shows consumption, and the gap between them is the accrual you have not yet booked. Reports showing one and not the other are the ones that surprise people.

**Who should own the cost report?**
A cost controller who reports to project controls rather than to the person whose package it describes, and whose numbers reconcile monthly to the ledger. The reconciliation matters more than the reporting line: when the cost report and the accounts differ, they are describing two different projects.

---

*First published on projectcontrolsinstitute.org; this version is marked as republished in Draft Settings and the canonical points to the original article.*

*Internal links: one is now in the body. "The four EAC formulas" points at projectcontrolsinstitute.org/four-eac-formulas, kept because the worked package reaches a forecast and that sentence raises which method to choose when the overrun is a contracted rate rather than a one-off. The second link to the same domain, a closing sentence pointing at what-is-project-controls, was removed with the sentence: it existed to carry a link rather than to answer anything, and the cap is one link per domain per piece. The earned value reporting thresholds link proposed earlier was dropped for the same reason. No second domain earns a link here. Reciprocal: the four EAC formulas page should point back to this one for commitment control, with an anchor about catching the overrun on signature day.*
