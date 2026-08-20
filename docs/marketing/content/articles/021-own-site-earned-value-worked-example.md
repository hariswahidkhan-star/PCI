---
platform:      Own site — projectcontrolsinstitute.org
type:          guide
title:         Earned value worked example: a full month, end to end
meta:          An earned value worked example run properly: quantities to EV, ledger to AC, then CV, SV, CPI, SPI, four forecasts and earned schedule, every figure shown.
primary_kw:    earned value worked example
secondary_kw:  earning rules, cost performance index, estimate at completion, accruals
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1502
hashtags:      n/a (own site)
ab_id:         AB-00080
---

# Earned value worked example: a full month, end to end

An earned value worked example starts with three numbers at one cut-off date: planned value, earned value and actual cost. Everything else is arithmetic on those three. This piece runs a £3.6m electrical package through a full month-end, from quantities and accruals to CPI, four forecasts and earned schedule, showing every figure.

The numbers below are illustrative. The method is not.

## The package and the cut-off

An electrical installation package on a hospital extension. Budget at completion (BAC) is £3.6m over twelve months. We are reporting at the end of month five.

The cut-off is 23:59 on the last day of month five. Quantities, timesheets, goods received and subcontract valuations all have to describe that same instant, or the arithmetic later is meaningless.

Cut-off drift is the single most common defect in a controls system. Progress measured on the 28th against costs posted to the 31st will report an efficiency that nobody earned.

## Step one: earn the value, account by account

Earned value is the budgeted cost of work actually complete. It is measured in baseline money, never in what the work turned out to cost. Readers who want the surrounding method rather than the arithmetic will find it in [what earned value management measures and why](https://projectcontrolsinstitute.org/earned-value-management).

Each control account has an earning rule agreed before the work started. Here is the position at cut-off.

| Control account | BAC (£k) | Earning rule | Progress at cut-off | EV (£k) |
|---|---:|---|---|---:|
| CA-01 Containment | 820 | Units complete | 6,400 m of 9,200 m | 570.4 |
| CA-02 Cable pull and terminate | 1,140 | Units complete | 118 of 340 circuits | 395.6 |
| CA-03 Switchgear install | 640 | Milestone weighting | 45% of weighted milestones | 288.0 |
| CA-04 Design and as-builts | 280 | Milestone weighting | 60% of weighted milestones | 168.0 |
| CA-05 Site supervision | 320 | Level of effort | 5 of 12 months elapsed | 133.3 |
| CA-06 Test and commission | 400 | 0/100 | Nothing complete | 0.0 |
| **Total** | **3,600** | | | **1,555.3** |

The containment figure is 6,400 ÷ 9,200 = 0.6957, and 0.6957 × 820 = £570.4k. The cabling figure is 118 ÷ 340 = 0.3471, and 0.3471 × 1,140 = £395.6k.

So EV = **£1,555.3k**, and the package is 1,555.3 ÷ 3,600 = **43.2% complete** by value.

Note CA-05. Level of effort earns value because time passed, not because anything was installed. It contributed £133.3k, which is 8.6% of all earned value on this report, and it will always report on plan.

## Step two: build an actual cost the accountant would sign

Actual cost is what has been incurred for the same scope to the same cut-off. Incurred, not invoiced.

| Component | £k |
|---|---:|
| Invoices posted to the ledger at cut-off | 1,586 |
| Goods received not yet invoiced (accrual) | 142 |
| Subcontract work done, not yet applied for | 96 |
| Less: materials delivered but not installed | (64) |
| Less: invoice covering work after cut-off | (31) |
| **Actual cost (AC)** | **1,729** |

The two deductions are where cost engineers and accountants usually argue. Materials sitting in the container have been paid for but have earned nothing, so leaving them in AC would depress CPI for a month and then flatter it later.

Take them out of AC and the cost report matches the work. The stock still exists in the balance sheet; it is simply not a cost of work performed yet.

Planned value comes straight off the baseline curve at the same date: PV = **£1,742.0k**.

## Step three: variances and indices

Four calculations, in the order you use them. If you want the units and sign conventions beside you while they run, [every earned value formula in one place](https://projectcontrolsinstitute.org/earned-value-formulas-cheat-sheet) carries them.

**Cost variance.** CV = EV − AC = 1,555.3 − 1,729.0 = **−£173.7k**.

**Schedule variance.** SV = EV − PV = 1,555.3 − 1,742.0 = **−£186.7k**.

**Cost performance index.** CPI = EV ÷ AC = 1,555.3 ÷ 1,729.0 = **0.900**.

**Schedule performance index.** SPI = EV ÷ PV = 1,555.3 ÷ 1,742.0 = **0.893**.

Read them together. You are getting 90 pence of budgeted work for every pound spent, and you are short of the plan by about a tenth of the value you should have earned by now.

Remaining budgeted work is BAC − EV = 3,600 − 1,555.3 = **£2,044.7k**.

**To-complete performance index.** TCPI = (BAC − EV) ÷ (BAC − AC) = 2,044.7 ÷ 1,871.0 = **1.093**.

That is the number to put in front of the sponsor. The crew has run at 0.900 for five months, and finishing on budget now needs 1.093, which is 1.093 ÷ 0.900 = **1.21**, a 21% step change from the same people on the same site.

## Step four: four forecasts from one dataset

| Method | Formula | Result (£k) | What it assumes |
|---|---|---:|---|
| Remaining work at budget | AC + (BAC − EV) | 1,729 + 2,044.7 = **3,773.7** | The loss is behind you and the rest runs at plan |
| Remaining work at current CPI | BAC ÷ CPI | 3,600 ÷ 0.900 = **4,001.9** | Today's efficiency continues to the end |
| Remaining work at CPI and SPI | AC + (BAC − EV) ÷ (CPI × SPI) | 1,729 + 2,044.7 ÷ 0.8037 = **4,273.1** | Schedule pressure will keep costing money |
| Bottom-up re-estimate | AC + a fresh ETC | 1,729 + 2,310 = **4,039.0** | The team can re-estimate honestly |

A spread of £3.77m to £4.27m, half a million pounds wide, from one set of inputs. That range is the method asking which assumption you are prepared to sign.

Here the cause decides it. Circuit terminations are running slower than the rate in the estimate, which is a productivity error and therefore systemic, so the CPI method is the defensible default. [Choosing between the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) works that judgement through case by case.

**Variance at completion.** VAC = BAC − EAC = 3,600 − 4,001.9 = **−£401.9k**. That is what the contingency conversation is actually about, and it should be raised in month five rather than month nine.

## Step five: convert money into time

SPI is measured in money, so it drifts back towards 1.00 as a project completes even when the project is late. Earned schedule fixes the units.

The baseline curve reaches £1,468k of PV at the end of month four and £1,742k at the end of month five. Our EV of £1,555.3k falls between them.

ES = 4 + (1,555.3 − 1,468) ÷ (1,742 − 1,468) = 4 + 87.3 ÷ 274 = **4.32 months**.

Actual time is 5.00 months, so SV(t) = 4.32 − 5.00 = **−0.68 months**, roughly three weeks behind. SPI(t) = 4.32 ÷ 5.00 = **0.863**.

"Three weeks behind" is a sentence a project manager can act on. "SV is minus £186.7k" needs translating first.

## The earned value worked example in one table

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
| EAC (CPI method) | £4,001.9k |
| VAC | −£401.9k |
| SPI(t) / SV(t) | 0.863 / −0.68 months |

## What this example still hides

Quality. Any cable pulled, tested, failed and re-pulled was counted as earned value when it was pulled.

Scope that was never baselined. If the estimate missed a switchroom, the package will report respectable performance against the wrong £3.6m.

And the earning rules themselves. Move CA-05 from level of effort onto a milestone rule and the whole report changes shape, which is why the rules belong in the cost control procedure and not in a month-end conversation.

One more, and it is the one that reaches the accounts. The 43.2% here is a control number built in baseline money.

It is not a revenue percentage, and it should never travel into the ledger without someone who understands both sides signing it. That crossing point is exactly where a project reports one position on the delivery side and a different one in the accounts.

## Frequently asked questions

**Should uninstalled materials be in actual cost?**
No, if you want CPI to describe work performed. Materials on site that have been paid for but not installed have earned nothing, so including them understates efficiency in one period and overstates it in the next. Hold them as stock and bring the cost in when the associated work is earned. Whatever you choose, apply it to every control account and write it in the procedure.

**Why does EV use budgeted rates rather than actual rates?**
Because earned value has to be independent of what the work cost, or the comparison collapses. If EV were measured at actual rates it would move with AC and CPI would sit at 1.00 permanently. Budgeted money in, budgeted money out, and the difference between that and the ledger is the variance you are trying to see.

**How often should this be run?**
Monthly is the norm on capital projects because it matches the finance cycle and the accrual process. Weekly quantity tracking is useful for the site team but rarely worth a full forecast, since the cost data is not complete enough to support one. The test is whether the numbers would change a decision that week.

**What if CPI and SPI point in opposite directions?**
That usually means acceleration. A package running ahead of plan while overspending is often buying time with overtime or extra crews, which is a legitimate choice if someone decided it. The forecast should then use a method that carries the schedule effect forward rather than assuming the extra cost stops.

**Is a CPI of 0.900 recoverable?**
Rarely by working harder. The TCPI of 1.093 says the remaining work has to run 21% better than everything so far, and recovery plans written after five months of consistent under-performance almost never deliver that. Recovering usually means changing the method, the crew mix or the scope, and each of those is a change the sponsor has to authorise.

---

*Internal linking note: three same-domain links now sit in the body. "What earned value management measures and why" points at the earned value pillar, placed at the definition of earned value in step one, where a reader arriving for the arithmetic may still need the method around it. "Every earned value formula in one place" points at the cheat sheet, placed at the head of the variances and indices step, which is exactly where units and sign conventions get looked up. "Choosing between the four EAC formulas" points at the EAC guide, placed where the piece picks one of the four forecasts and has to justify the choice. The original note pointed that last link at /eac-formulas, which does not exist; the live page is /four-eac-formulas. No cross-estate link is carried: the whole piece is hub territory. Reciprocal: the EAC guide and the cheat sheet should each link back here with an anchor naming this as the worked month-end behind their numbers.*
