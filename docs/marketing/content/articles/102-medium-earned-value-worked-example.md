---
platform:      Medium
type:          guide
title:         Earned value worked example: one month, end to end
meta:          An earned value worked example on a £3.6m package: quantities to EV, ledger to AC, then CPI, TCPI, four forecasts and earned schedule, every figure shown.
primary_kw:    earned value worked example
secondary_kw:  earning rules, cost performance index, estimate at completion, accruals
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /earned-value-worked-example (own site #021)
schema:        Article + FAQPage
word_count:    1,740
hashtags:      #ProjectControls #EarnedValue #CostEngineering #Scheduling #ProjectManagement
ab_id:         AB-00080
---

# Earned value worked example: one month, end to end

An earned value worked example begins with three numbers at a single cut-off date: planned value, earned value and actual cost. Everything after that is arithmetic. This piece runs a £3.6m electrical package through one month-end, from quantities and accruals to CPI, four forecasts and earned schedule, showing every figure.

The numbers are illustrative. The method is not.

## The package and the cut-off

An electrical installation package on a hospital extension. Budget at completion (BAC) is £3.6m over twelve months, and we are reporting at the end of month five.

The cut-off is 23:59 on the last day of month five. Quantities, timesheets, goods received notes and subcontract valuations all have to describe that same instant, or every calculation after it is meaningless.

Cut-off drift is the most common defect in a controls system. Progress measured on the 28th against costs posted to the 31st reports an efficiency nobody earned.

## Step one: earn the value, account by account

Earned value is the budgeted cost of work actually complete. It is stated in baseline money, never in what the work turned out to cost.

Each control account carries an earning rule agreed before the work started. Here is the position at cut-off.

| Control account | BAC (£k) | Earning rule | Progress at cut-off | EV (£k) |
|---|---:|---|---|---:|
| CA-01 Containment | 820 | Units complete | 6,400 m of 9,200 m | 570.4 |
| CA-02 Cable pull and terminate | 1,140 | Units complete | 118 of 340 circuits | 395.6 |
| CA-03 Switchgear install | 640 | Milestone weighting | 45% of weighted milestones | 288.0 |
| CA-04 Design and as-builts | 280 | Milestone weighting | 60% of weighted milestones | 168.0 |
| CA-05 Site supervision | 320 | Level of effort | 5 of 12 months elapsed | 133.3 |
| CA-06 Test and commission | 400 | 0/100 | Nothing complete | 0.0 |
| **Total** | **3,600** | | | **1,555.3** |

The containment line is 6,400 ÷ 9,200 = 0.6957, and 0.6957 × 820 = £570.4k. The cabling line is 118 ÷ 340 = 0.3471, and 0.3471 × 1,140 = £395.6k.

So EV = **£1,555.3k**, which makes the package 1,555.3 ÷ 3,600 = **43.2% complete** by value.

Look at CA-05. Level of effort earns value because time passed, not because anything was installed. It contributed £133.3k, which is 8.6% of all earned value on this report, and it will report on plan every month until the job ends.

## Step two: build an actual cost the accountant would sign

Actual cost is what has been incurred against the same scope to the same cut-off. Incurred, not invoiced.

| Component | £k |
|---|---:|
| Invoices posted to the ledger at cut-off | 1,586 |
| Goods received not yet invoiced (accrual) | 142 |
| Subcontract work done, not yet applied for | 96 |
| Less: materials delivered but not installed | (64) |
| Less: invoice covering work after cut-off | (31) |
| **Actual cost (AC)** | **1,729** |

The two deductions are where cost engineers and accountants argue. Materials sitting in a container have been paid for and have earned nothing, so leaving them in AC depresses CPI this month and flatters it later.

Take them out and the cost report describes the work. The stock still exists on the balance sheet; it is simply not yet a cost of work performed.

Planned value comes straight off the baseline curve at the same date: PV = **£1,742.0k**.

## Step three: variances and indices

Four calculations, in the order you use them.

**Cost variance.** CV = EV − AC = 1,555.3 − 1,729.0 = **−£173.7k**.

**Schedule variance.** SV = EV − PV = 1,555.3 − 1,742.0 = **−£186.7k**.

**Cost performance index.** CPI = EV ÷ AC = 1,555.3 ÷ 1,729.0 = **0.900**.

**Schedule performance index.** SPI = EV ÷ PV = 1,555.3 ÷ 1,742.0 = **0.893**.

Read them together. The package returns 90 pence of budgeted work per pound spent, and it is short of the plan by about a tenth of the value it should have earned by now.

Remaining budgeted work is BAC − EV = 3,600 − 1,555.3 = **£2,044.7k**.

**To-complete performance index.** TCPI = (BAC − EV) ÷ (BAC − AC) = 2,044.7 ÷ 1,871.0 = **1.093**.

Put that in front of the sponsor. Five months at 0.900, and finishing on budget now needs 1.093, which is 1.093 ÷ 0.900 = **1.21**: a 21% step change from the same people on the same site.

## Step four: four forecasts from one dataset

| Method | Formula | Result (£k) | What it assumes |
|---|---|---:|---|
| Remaining work at budget | AC + (BAC − EV) | 1,729 + 2,044.7 = **3,773.7** | The loss is behind you and the rest runs at plan |
| Remaining work at current CPI | BAC ÷ CPI | 3,600 ÷ 0.900 = **4,000.0** | Today's efficiency continues to the end |
| Remaining work at CPI and SPI | AC + (BAC − EV) ÷ (CPI × SPI) | 1,729 + 2,044.7 ÷ 0.8037 = **4,273.1** | Schedule pressure keeps costing money |
| Bottom-up re-estimate | AC + a fresh ETC of £2,310k | 1,729 + 2,310 = **4,039.0** | The team can re-estimate honestly |

All four use the indices exactly as printed above, rounded to three decimals. Carried unrounded, CPI is 0.899537 and the second method returns £4,002.1k instead of £4,000.0k, which is a rounding difference rather than a different forecast — but say which convention you are using, because a reviewer redoing your arithmetic will land on the other one.

A spread of £3.77m to £4.27m, half a million pounds wide, from one set of inputs. That range is the method asking which assumption you are prepared to sign, and the answer comes from [choosing an EAC method by the cause of the variance](https://projectcontrolsinstitute.org/four-eac-formulas) rather than from the arithmetic.

Here the cause settles it. Circuit terminations are running slower than the rate embedded in the estimate, which is a productivity error and therefore systemic, so the CPI method is the defensible default.

**Variance at completion.** VAC = BAC − EAC = 3,600 − 4,000.0 = **−£400.0k**. That is what the contingency conversation is actually about, and it should happen in month five rather than month nine.

## Step five: convert the money into time

SPI is denominated in money, so it drifts back towards 1.00 as a project completes even when the project is late. Earned schedule restates the same position in time.

The baseline curve reaches £1,468k of PV at the end of month four and £1,742k at the end of month five. Our EV of £1,555.3k sits between them.

ES = 4 + (1,555.3 − 1,468) ÷ (1,742 − 1,468) = 4 + 87.3 ÷ 274 = **4.32 months**.

Actual time is 5.00 months, so SV(t) = 4.32 − 5.00 = **−0.68 months**, roughly three weeks behind, and SPI(t) = 4.32 ÷ 5.00 = **0.864**.

"Three weeks behind" is a sentence a project manager can act on. "SV is minus £186.7k" needs translating first.

## The whole earned value worked example in one table

| Measure | Value |
|---|---:|
| BAC | £3,600.0k |
| PV | £1,742.0k |
| EV | £1,555.3k |
| AC | £1,729.0k |
| CV / SV | −£173.7k / −£186.7k |
| CPI / SPI | 0.900 / 0.893 |
| Per cent complete | 43.2% |
| TCPI to BAC | 1.093 |
| EAC (CPI method) | £4,000.0k |
| VAC | −£400.0k |
| SPI(t) / SV(t) | 0.864 / −0.68 months |

## What this example still hides

Quality. Any cable pulled, tested, failed and pulled again was counted as earned value the first time it went in.

Scope that was never baselined. If the estimate missed a switchroom, the package will report respectable performance against the wrong £3.6m.

The earning rules themselves. Move CA-05 off level of effort onto a milestone rule and the whole report changes shape, which is why the rules belong in the cost control procedure rather than in a month-end conversation.

And one that reaches the accounts. The 43.2% above is a control number built in baseline money.

It is not a revenue percentage, and it should not travel into the ledger without someone who understands both sides signing it. That crossing point is where a project reports one position on the delivery side and a different one in the accounts, so it is worth checking [which certifications examine the reporting side as well as the site](https://credentialfinder.org/best-project-controls-certification) before deciding whose signature settles it.

## Frequently asked questions

**Should uninstalled materials sit in actual cost?**
Not if you want CPI to describe work performed. Materials paid for but not installed have earned nothing, so including them understates efficiency this period and overstates it next. Hold them as stock and bring the cost in when the associated work is earned. Whatever you decide, apply it to every control account and write it into the procedure.

**Why does EV use budgeted rates rather than actual rates?**
Because earned value has to be independent of what the work cost, or the comparison collapses. Measured at actual rates, EV would move with AC and CPI would sit at 1.00 permanently. Budgeted money in, budgeted money out, and the gap against the ledger is the variance you are trying to see.

**How often should this be run?**
Monthly is the norm on capital projects, because it matches the finance cycle and the accrual process. Weekly quantity tracking helps the site team but rarely supports a full forecast, since the cost data is not complete enough. The test is whether the numbers would change a decision that week.

**What if CPI and SPI point in opposite directions?**
That usually means acceleration. A package running ahead of plan while overspending is often buying time with overtime or extra crews, which is legitimate if somebody chose it. The forecast should then use a method that carries the schedule effect forward rather than assuming the extra cost stops.

**Is a CPI of 0.900 recoverable?**
Rarely by working harder. The TCPI of 1.093 says the remaining work must run 21% better than everything so far, and recovery plans written after five consistent months almost never deliver that. Real recovery means changing the method, the crew mix or the scope, and each of those is a change the sponsor has to authorise.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so this version exists for readers, not for link equity.*

*Estate links, as placed in the body. This is a Medium republish, so both links leave the platform for the estate and neither is a same-domain internal link. Step four links to [choosing an EAC method by the cause of the variance](https://projectcontrolsinstitute.org/four-eac-formulas), because the £500k spread in that table asks which of the four answers a forecaster is prepared to sign. The closing section links to [which certifications examine the reporting side as well as the site](https://credentialfinder.org/best-project-controls-certification), because a control number crossing into the ledger raises who has been examined on both halves. One estate link per domain and no more: the earned value pillar and the formulas cheat sheet proposed earlier were dropped rather than stacked three-deep on the hub, and the old `/eac-formulas` target does not exist — the real slug is `/four-eac-formulas`. Reciprocal: the hub's own EAC guide has honest reason to point back here, for the worked month-end that produces its inputs.*
