---
platform:      Hashnode
type:          guide
title:         Earned value worked example: a full month, computed
meta:          An earned value worked example run properly: quantities to EV, ledger to AC, then CV, SV, CPI, SPI, four forecasts and earned schedule, every figure shown.
primary_kw:    earned value worked example
secondary_kw:  earning rules, cost performance index, estimate at completion, earned schedule
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/earned-value-worked-example
schema:        Article
word_count:    1800
hashtags:      #python #datascience #finance #tutorial
ab_id:         AB-00080
---

# Earned value worked example: a full month, computed

An earned value worked example starts with three numbers at one cut-off date: planned value, earned value and actual cost. Everything after that is arithmetic on those three. This runs a £3.6m electrical package through a full month-end, from quantities and accruals to CPI, four forecasts and earned schedule, with the code that reproduces every figure.

The numbers are illustrative. The method is not.

## The package and the cut-off

An electrical installation package on a hospital extension. Budget at completion (BAC) is £3.6m over twelve months, reporting at the end of month five.

The cut-off is 23:59 on the last day of month five. Quantities, timesheets, goods received and subcontract valuations must all describe that instant, or the arithmetic afterwards is meaningless.

Cut-off drift is the most common defect in a controls system: progress measured on the 28th against costs posted to the 31st reports an efficiency nobody earned.

## Step one: earn the value, account by account

Earned value is the budgeted cost of work actually complete, measured in baseline money and never in what the work turned out to cost.

Each control account has an earning rule agreed before the work started. The position at cut-off:

| Control account | BAC (£k) | Earning rule | Progress at cut-off | EV (£k) |
|---|---:|---|---|---:|
| CA-01 Containment | 820 | Units complete | 6,400 m of 9,200 m | 570.4 |
| CA-02 Cable pull and terminate | 1,140 | Units complete | 118 of 340 circuits | 395.6 |
| CA-03 Switchgear install | 640 | Milestone weighting | 45% of weighted milestones | 288.0 |
| CA-04 Design and as-builts | 280 | Milestone weighting | 60% of weighted milestones | 168.0 |
| CA-05 Site supervision | 320 | Level of effort | 5 of 12 months elapsed | 133.3 |
| CA-06 Test and commission | 400 | 0/100 | Nothing complete | 0.0 |
| **Total** | **3,600** | | | **1,555.3** |

Containment is 6,400 ÷ 9,200 = 0.6957, and 0.6957 × 820 = £570.4k. Cabling is 118 ÷ 340 = 0.3471, and 0.3471 × 1,140 = £395.6k.

So EV = **£1,555.3k**, and the package is 1,555.3 ÷ 3,600 = **43.2 per cent** complete by value.

Note CA-05. Level of effort earns value because time passed, not because anything was installed. It contributed £133.3k, which is 8.6 per cent of all earned value here, and it will always report on plan.

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

The two deductions are where cost engineers and accountants argue. Materials sitting in the container have been paid for and earned nothing, so leaving them in AC would depress CPI this month and flatter it later.

Take them out and the cost report matches the work. The stock still sits on the balance sheet; it is not yet a cost of work performed.

Planned value comes off the baseline curve at the same date: PV = **£1,742.0k**.

## Step three: variances and indices

Four calculations, in the order you use them.

**Cost variance.** CV = EV − AC = 1,555.3 − 1,729.0 = **−£173.7k**.

**Schedule variance.** SV = EV − PV = 1,555.3 − 1,742.0 = **−£186.7k**.

**Cost performance index.** CPI = EV ÷ AC = 1,555.3 ÷ 1,729.0 = **0.900**.

**Schedule performance index.** SPI = EV ÷ PV = 1,555.3 ÷ 1,742.0 = **0.893**.

Read them together. You are getting 90 pence of budgeted work for every pound spent, and you are a tenth short of the value you should have earned by now.

Remaining budgeted work is BAC − EV = 3,600 − 1,555.3 = **£2,044.7k**.

**To-complete performance index.** TCPI = (BAC − EV) ÷ (BAC − AC) = 2,044.7 ÷ 1,871.0 = **1.093**.

That is the number to put in front of a sponsor. The crew has run at 0.900 for five months, and finishing on budget now requires 1.093 ÷ 0.900 = **1.21**: a 21 per cent step change from the same people on the same site.

## Step four: four forecasts from one dataset

| Method | Formula | Result (£k) | What it assumes |
|---|---|---:|---|
| Remaining work at budget | AC + (BAC − EV) | 1,729 + 2,044.7 = **3,773.7** | The loss is behind you and the rest runs at plan |
| Remaining work at current CPI | BAC ÷ CPI | 3,600 ÷ 0.8995 = **4,002.1** | Today's efficiency continues to the end |
| Remaining work at CPI and SPI | AC + (BAC − EV) ÷ (CPI × SPI) | 1,729 + 2,044.7 ÷ 0.8031 = **4,274.9** | Schedule pressure will keep costing money |
| Bottom-up re-estimate | AC + a fresh ETC | 1,729 + 2,310 = **4,039.0** | The team can re-estimate honestly |

A spread of £3.77m to £4.27m from one set of inputs. That range is the method asking which assumption you are prepared to sign.

The cause decides it. Circuit terminations are running slower than the estimated rate, a productivity error and therefore systemic, so the CPI method is the defensible default. The other three, and the conditions under which each of them beats the CPI method, are set out in [choosing between the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas).

**Variance at completion.** VAC = BAC − EAC = 3,600 − 4,002.1 = **−£402.1k**. That is the contingency conversation, and it belongs in month five rather than month nine.

```python
BAC, PV, EV, AC = 3600.0, 1742.0, 1555.3, 1729.0
cpi, spi = EV / AC, EV / PV
remaining = BAC - EV

print(round(cpi, 4), round(spi, 4))                  # 0.8995 0.8928
print(round(remaining / (BAC - AC), 3))              # 1.093  TCPI to BAC
print(round(AC + remaining, 1))                      # 3773.7
print(round(BAC / cpi, 1))                           # 4002.1
print(round(AC + remaining / (cpi * spi), 1))        # 4274.9
```

## Step five: convert money into time

SPI is measured in money, so it drifts back towards 1.00 as a project completes even when the project is late. Earned schedule fixes the units.

The baseline curve reaches £1,468k of PV at the end of month four and £1,742k at the end of month five. EV of £1,555.3k falls between them, so interpolate.

ES = 4 + (1,555.3 − 1,468) ÷ (1,742 − 1,468) = 4 + 87.3 ÷ 274 = **4.32 months**.

Actual time is 5.00 months, so SV(t) = 4.32 − 5.00 = **−0.68 months**, roughly three weeks behind, and SPI(t) = 4.32 ÷ 5.00 = **0.864**.

"Three weeks behind" is a sentence a project manager can act on. "SV is minus £186.7k" needs translating.

## The earned value worked example in one table

| Measure | Value |
|---|---:|
| BAC | £3,600.0k |
| PV / EV / AC | £1,742.0k / £1,555.3k / £1,729.0k |
| CV / SV | −£173.7k / −£186.7k |
| CPI / SPI | 0.900 / 0.893 |
| Per cent complete | 43.2% |
| TCPI to BAC | 1.093 |
| EAC (CPI method) | £4,002.1k |
| VAC | −£402.1k |
| SPI(t) / SV(t) | 0.864 / −0.68 months |

## What this example still hides

Quality. Any cable pulled, tested, failed and re-pulled was counted as earned value when it was pulled.

Scope that was never baselined. If the estimate missed a switchroom, the package reports respectable performance against the wrong £3.6m.

And the earning rules. Move CA-05 off level of effort onto a milestone rule and the whole report changes shape, which is why the rules belong in the cost control procedure rather than in a month-end conversation.

One more reaches the accounts. The 43.2 per cent here is a control number in baseline money. It is not a revenue percentage, and it should never travel into the ledger without somebody who understands both sides signing it.

That crossing point is where a project reports one position on the delivery side and another in the accounts. It is examined by the PCI AI Project Finance Leader (PFL-AI); the calculation content behind the PFL-AI and PCI Project Management Leader – AI (PML-AI) volumes carries 15,613 machine calculation checks, all passing, and that suite covers those two credentials only.

## Frequently asked questions

**Should uninstalled materials be in actual cost?**
Not if you want CPI to describe work performed. Materials paid for and not installed have earned nothing, so including them understates efficiency in one period and overstates it in the next. Hold them as stock and bring the cost in when the associated work is earned. Whatever you choose, apply it to every control account.

**Why does EV use budgeted rates rather than actual rates?**
Because earned value has to be independent of what the work cost, or the comparison collapses. If EV were measured at actual rates it would move with AC and CPI would sit at 1.00 permanently. Budgeted money in, budgeted money out, and the gap to the ledger is the variance you are trying to see.

**How often should this be run?**
Monthly is the norm on capital projects because it matches the finance cycle and the accrual process. Weekly quantity tracking helps the site team but rarely supports a full forecast, because the cost data is not complete enough. The test is whether the numbers would change a decision.

**What if CPI and SPI point in opposite directions?**
That usually means acceleration. A package ahead of plan while overspending is buying time with overtime or extra crews, which is legitimate if somebody decided it. The forecast should then use a method carrying the schedule effect forward rather than assuming the extra cost stops.

**Is a CPI of 0.900 recoverable?**
Rarely by working harder. The TCPI of 1.093 says the remaining work must run 21 per cent better than everything so far, and recovery plans written after five months of under-performance almost never deliver that. Recovery usually means changing the method, the crew mix or the scope, each of which the sponsor has to authorise.

---

*First published on projectcontrolsinstitute.org; this Hashnode version is flagged as republished, with the canonical pointing at the original worked month.*

*Internal links: the body carries one link, to https://projectcontrolsinstitute.org/four-eac-formulas, anchored "choosing between the four EAC formulas". It sits where the four forecasts produce a £3.77m–£4.27m spread and the reader has to pick one, which is exactly the question that page answers. The earlier link to the earned value management pillar was removed and the cheat-sheet link was not added: this is a republication whose canonical already points home, and a second or third link to the same domain in one piece is a pattern worth avoiding. Reciprocal: none needed — the hub original is the canonical parent, not a peer.*
