---
platform:      Own site — projectcontrolsinstitute.org
type:          pillar
title:         What is earned value management? The practitioner's guide
meta:          What is earned value management, in formulas and a worked month: CV, SV, CPI, SPI, the four EAC methods, earned schedule and the earning rules behind them.
primary_kw:    what is earned value management
secondary_kw:  cost performance index, estimate at completion methods, earned schedule, earning rules
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    2277
hashtags:      n/a (own site)
ab_id:         AB-00091
---

# What is earned value management? The practitioner's guide

Earned value management compares the value of work actually completed against what that work was budgeted to cost and against what has been spent. So the short answer to *what is earned value management* is this: planned value, earned value and actual cost, with every other measure built as arithmetic on top of those three.

It produces two variances and two indices from those numbers, and from those a forecast of the final cost. Its purpose is a defensible outturn, not a dashboard.

## What is earned value management measuring? The three inputs

**Planned value (PV)** is the budgeted cost of the work scheduled to be complete by the cut-off date. It comes from the baseline, and it changes only through approved change control.

**Earned value (EV)** is the budgeted cost of the work actually complete by that date. Note the word budgeted: EV is measured in baseline money, never in what the work turned out to cost.

**Actual cost (AC)** is what has been incurred for that same work, over the same period, to the same cut-off. Incurred, not invoiced.

Two errors account for most broken earned value systems. Measuring EV to one date and AC to another, and putting invoices into AC while accrued deliveries sit outside it.

## The formulas, in the order you use them

| Measure | Formula | Reads as |
|---|---|---|
| Cost variance | CV = EV − AC | Negative means you paid more than the work was worth |
| Schedule variance | SV = EV − PV | Negative means less work is done than planned, in money |
| Cost performance index | CPI = EV / AC | Value earned per unit of cost. Below 1.0 is overspending |
| Schedule performance index | SPI = EV / PV | Below 1.0 is behind the plan, measured in money not time |
| Estimate to complete | ETC = the cost of the work remaining | Depends on the method chosen |
| Estimate at completion | EAC = AC + ETC | The forecast outturn |
| Variance at completion | VAC = BAC − EAC | Negative means an overrun against budget |
| To-complete performance index | TCPI = (BAC − EV) / (BAC − AC) | The efficiency now required to finish on budget |

BAC is the budget at completion: the total authorised budget for the scope in question.

## A worked month, end to end

A piling and substructure package. BAC is $12.0m over 40 weeks. We are at the end of week 20.

At cut-off: PV = $6.4m, EV = $5.2m, AC = $6.1m.

**Variances.** CV = 5.2 − 6.1 = **−$0.9m**. SV = 5.2 − 6.4 = **−$1.2m**.

**Indices.** CPI = 5.2 / 6.1 = **0.852**. SPI = 5.2 / 6.4 = **0.813**.

Read those together before forecasting anything. You are getting 85 cents of budgeted work for every dollar spent, and you are roughly a fifth of the way behind where the plan says you should be by value.

**Remaining work.** BAC − EV = 12.0 − 5.2 = **$6.8m** of budgeted work still to earn.

**To-complete performance index.** TCPI to finish on budget = 6.8 / (12.0 − 6.1) = 6.8 / 5.9 = **1.153**.

That last number is the one to put in front of the sponsor. The crew has run at 0.852 for twenty weeks.

Finishing on budget now requires 1.153, which is 1.153 / 0.852 = **1.35**, a 35% step change in efficiency from the same team on the same site. Nobody has ever produced that from a recovery plan written in week 21.

## The four EAC methods, and what each one assumes

The forecast is where earned value either earns its place or discredits itself. Four methods are in common use and they will not agree.

| Method | Formula | Assumes | Our package | Fails when |
|---|---|---|---|---|
| Remaining work at budget | EAC = AC + (BAC − EV) | The overrun is behind you and future work runs at plan | 6.1 + 6.8 = **$12.9m** | The cause is systemic (bad estimate, wrong rates), which is usually |
| Remaining work at current CPI | EAC = BAC / CPI | Performance to date continues to the end | 12.0 / 0.852 = **$14.08m** | Early-phase CPI is unrepresentative, or the mix of work changes |
| Remaining work at CPI and SPI | EAC = AC + (BAC − EV) / (CPI × SPI) | Schedule pressure will keep costing money, through overtime and acceleration | 6.1 + 6.8 / 0.693 = **$15.92m** | The schedule slip is caused by something that does not consume cost |
| Bottom-up re-estimate | EAC = AC + a fresh ETC | The team can re-estimate the remaining scope honestly | 6.1 + 7.4 = **$13.5m** | Optimism, or no time to do it properly |

A spread of $12.9m to $15.9m from one dataset. That range is not a weakness of the method; it is the method telling you which assumption you are being asked to sign.

Some organisations use a weighted version of the third method, typically 0.8 × CPI plus 0.2 × SPI in the denominator, to soften the schedule effect. It is a judgement rule, not a law, and it should be documented in the cost control procedure rather than invented at month-end.

**How to choose.** Ask what caused the variance. A one-off event that has finished argues for the first method, and a rate or productivity error argues for the second.

A slipping programme being bought back with overtime argues for the third. A change of scope or method argues for a bottom-up re-estimate, and probably for a change notice as well.

**VAC.** Using the CPI method: VAC = 12.0 − 14.08 = **−$2.08m**. That is the number the contingency conversation is actually about.

## Earned schedule: why SPI lies at the end

SPI has a structural fault. As a project completes, EV converges on PV, so SPI returns to 1.00 even on a project finishing a year late. It reports on time at the exact moment it is most wrong.

Earned schedule fixes the units. Instead of asking how much value you have earned, it asks when the plan said you would have earned it.

In our package EV = $5.2m. The baseline reaches $4.8m of PV at week 16 and $5.4m at week 17. Interpolating: ES = 16 + (5.2 − 4.8) / (5.4 − 4.8) = 16 + 0.67 = **16.67 weeks**.

Actual time (AT) is 20 weeks. So SV(t) = ES − AT = 16.67 − 20 = **−3.33 weeks**, and SPI(t) = ES / AT = 16.67 / 20 = **0.83**.

"We are 3.3 weeks behind" is a sentence a project manager can act on. "SV is minus $1.2m" is a sentence that needs translating first, and it stops working near completion.

## Earning rules: the decision that sets everything else

Every EV number depends on how progress was measured. Choose the rule before the work starts, write it in the procedure, and do not change it mid-package.

| Rule | How it works | Use for | Risk |
|---|---|---|---|
| 0/100 | No credit until complete | Short activities, under one reporting period | Looks harsh early; fine, because it cannot be gamed |
| 50/50 | Half on start, half on completion | Two-period activities | Rewards starting things, which is a real behaviour |
| Units complete | Credit per physical unit installed | Piles, cable pulls, welds, square metres | Needs a reliable quantity survey |
| Milestone weighting | Weighted credit at defined checkpoints | Engineering and design deliverables | Milestone definitions drift unless they are written tightly |
| Percent complete (assessed) | Judgement of the responsible engineer | Where nothing else fits | The weakest rule; always the one being argued about |
| Level of effort | Credit accrues with time | Genuine support functions only | Applied to real work, it manufactures earned value from nothing |

A project that puts a large share of its scope on level of effort will report a CPI near 1.00 whatever happens, because time passing earns value. If a package looks suspiciously well behaved, check the earning rules before you check the crew.

Earning rules also decide whether the whole method is worth running. They sit inside the monthly cadence of cut-off, progress, cost and forecast set out in [what is project controls](https://projectcontrolsinstitute.org/what-is-project-controls), and a rule agreed after work has started is a rule someone has already priced.

## Where earned value fails

It is blind to quality. Work earned and later ripped out was counted as value at the time.

It is blind to scope you never baselined. If the estimate missed 200 tonnes of steel, earned value will report you performing well on the wrong scope, which is the most expensive kind of good news.

It is dependent on the baseline being real. A programme resource-loaded to hit a promised date rather than to model the work will produce a PV curve that nothing can be measured against.

And it needs a cost system that accrues. Where actuals are two months behind, CPI describes the spring, not the month you are reporting.

## Earned value is not revenue

Earned value is a control number, produced in baseline money to manage a project. Revenue is an accounting number, recognised under the applicable financial reporting standard, and the two are computed for different purposes.

They can be related. An input method of measuring progress towards satisfying a performance obligation may use costs incurred against total expected costs, which looks like earned value arithmetic and is not the same thing.

The practical rule: never let a percentage complete travel from the cost report into the accounts without someone who understands both signing it. That handover is exactly where a project reports 62% on the delivery side and something different in the ledger, and nobody notices until audit.

This overlap is why the [PCL-AI Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) puts project accounting and finance alongside the delivery disciplines across its 13 domains and 61 knowledge areas, in the proportions 40 / 40 / 20 with governed AI.

## Where AI helps, and where it must not

Useful today: assembling the data, detecting cost codes whose behaviour has changed, and testing whether a claimed recovery has ever been achieved by a comparable package.

Not acceptable: a forecast nobody can explain. If you cannot say which method produced the EAC and which assumption it rests on, you cannot defend it, and an unexplainable forecast is worse than a wrong one you understand.

That defence is exactly what a scenario-based examination asks for, and [the certification pillar](https://projectcontrolsinstitute.org/project-controls-certification) sets out how the PCI credentials assess it.

## Frequently asked questions

**What is a good CPI?**
Anything at or above 1.00 means you are earning at least a unit of budgeted value per unit of cost. In practice, mature projects run between about 0.95 and 1.05 and the trend matters more than the level. A CPI that improves smoothly every month without a management action behind it usually indicates the earning rules, not the performance.

**What is the difference between SPI and SPI(t)?**
SPI is measured in money, EV divided by PV, and it converges on 1.00 as a project finishes regardless of lateness. SPI(t) is earned schedule divided by actual time, so it stays meaningful to the end and is expressed in weeks rather than currency. Most controls teams now report both, and act on the time one.

**Which EAC method should I use?**
The one that matches the cause of the variance. A finished one-off event supports remaining work at budget; a rate or productivity problem supports BAC divided by CPI; a slipping programme being bought back with overtime supports the CPI and SPI method. Whichever you choose, record the reasoning in the cost report so the next person can challenge it.

**Does earned value work on agile projects?**
Yes, with care. Story points or throughput can be converted to a value measure, and the discipline of a baseline, an earning rule and a forecast still applies. What does not survive is claiming credit for work in progress, so treat an incomplete sprint the way you would treat an incomplete activity under a 0/100 rule.

**Is earned value management mandatory?**
It is contractually required on many government and major capital programmes, and expected on most large private ones. Elsewhere it is a choice. The realistic threshold is whether the project is large enough that you cannot see its true position by walking round it, which is roughly where a forecast has to be defended in writing.

**Can I run earned value in a spreadsheet?**
For one package, yes, and many good cost engineers still do. The problem is not the arithmetic but the data: cut-off consistency, accruals, change control and earning rules across dozens of control accounts. Spreadsheets fail on governance long before they fail on calculation.

---

*Internal links: this pillar should link to [what is project controls](https://projectcontrolsinstitute.org/what-is-project-controls) with the anchor "the wider project controls discipline", to [the certification pillar](https://projectcontrolsinstitute.org/project-controls-certification) with the anchor "how earned value is examined", and to the [PCL-AI Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) with the anchor "PCL-AI Body of Knowledge"; the earned value formulas cheat sheet, the EAC method guide and the earned value training pieces link back here with the anchor "the earned value management pillar".*
