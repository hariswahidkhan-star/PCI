---
platform:      Medium
type:          pillar
title:         What is earned value management? A practitioner's guide
meta:          What is earned value management: PV, EV and AC, the variances and indices they produce, four EAC forecasts and earned schedule, worked on one package.
primary_kw:    what is earned value management
secondary_kw:  cost performance index, estimate at completion methods, earned schedule, earning rules
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /earned-value-management (own site #003)
schema:        Article + FAQPage
word_count:    2,285
hashtags:      #ProjectControls #EarnedValue #CostEngineering #ProjectManagement #ProjectFinance
ab_id:         AB-00091
---

# What is earned value management? A practitioner's guide

What is earned value management? It is the method that compares the budgeted value of the work actually finished against what that work was planned to cost and against what has been spent. Three numbers produce two variances, two indices and a forecast of the final cost. Its output is a defensible outturn, not a dashboard.

Those three numbers are planned value, earned value and actual cost, and every other measure is arithmetic built on top of them.

Everything below is worked on one package, so the figures agree with each other and can be checked line by line.

## What is earned value management measuring? The three inputs

**Planned value (PV)** is the budgeted cost of the work scheduled to be complete at the cut-off date. It comes off the approved baseline and moves only through change control.

**Earned value (EV)** is the budgeted cost of the work actually complete at that date. The word budgeted is doing the work: EV is stated in baseline money, never in what the job turned out to cost.

**Actual cost (AC)** is what has been incurred against that same scope, to that same date. Incurred, not invoiced, which means accruals for goods received and work done are inside it.

Two defects account for most broken earned value systems. Measuring EV to one date and AC to another, and building AC from invoices while accrued deliveries sit outside it.

## The formulas, in the order a cost engineer uses them

| Measure | Formula | Reads as |
|---|---|---|
| Cost variance | CV = EV − AC | Negative means you paid more than the work was worth |
| Schedule variance | SV = EV − PV | Negative means less work is done than planned, in money |
| Cost performance index | CPI = EV ÷ AC | Value earned per unit of cost; below 1.00 is overspending |
| Schedule performance index | SPI = EV ÷ PV | Below 1.00 is behind plan, measured in money rather than time |
| Estimate to complete | ETC = the cost of the work remaining | Depends entirely on the method chosen |
| Estimate at completion | EAC = AC + ETC | The forecast outturn |
| Variance at completion | VAC = BAC − EAC | Negative means an overrun against budget |
| To-complete performance index | TCPI = (BAC − EV) ÷ (BAC − AC) | The efficiency now required to finish on budget |

BAC is budget at completion: the total authorised budget for whatever scope you are measuring, whether that is a control account, a package or the whole project.

## A worked month, end to end

A piling and substructure package. BAC is $12.0m over 40 weeks, and we are reporting at the end of week 20.

At cut-off: PV = $6.4m, EV = $5.2m, AC = $6.1m.

**Variances.** CV = 5.2 − 6.1 = **−$0.9m**. SV = 5.2 − 6.4 = **−$1.2m**.

**Indices.** CPI = 5.2 ÷ 6.1 = **0.852**. SPI = 5.2 ÷ 6.4 = **0.813**.

Read the pair together before forecasting anything. The package is returning 85 cents of budgeted work for every dollar spent, and it is roughly a fifth short of the value the baseline says should have been earned by now.

**Remaining work.** BAC − EV = 12.0 − 5.2 = **$6.8m** of budgeted work still to earn.

**To-complete performance index.** TCPI to finish on budget = 6.8 ÷ (12.0 − 6.1) = 6.8 ÷ 5.9 = **1.153**.

That is the number for the sponsor. The crew has run at 0.852 for twenty weeks, and finishing on budget now requires 1.153, which is 1.153 ÷ 0.852 = **1.35**. A 35% step change in efficiency, from the same team on the same site, has never come out of a recovery plan written in week 21.

## The four EAC methods, and what each one assumes

The forecast is where earned value either earns its keep or discredits itself. Four methods are in common use and they will not agree with each other.

| Method | Formula | Assumes | This package | Fails when |
|---|---|---|---|---|
| Remaining work at budget | EAC = AC + (BAC − EV) | The overrun is behind you and future work runs at plan | 6.1 + 6.8 = **$12.9m** | The cause is systemic, such as a bad rate, which it usually is |
| Remaining work at current CPI | EAC = BAC ÷ CPI | Performance to date continues to the end | 12.0 ÷ 0.852 = **$14.08m** | Early CPI is unrepresentative, or the mix of remaining work differs |
| Remaining work at CPI and SPI | EAC = AC + (BAC − EV) ÷ (CPI × SPI) | Schedule pressure keeps costing money through overtime and acceleration | 6.1 + 6.8 ÷ 0.693 = **$15.92m** | The slip has a cause that consumes no cost, such as a permit wait |
| Bottom-up re-estimate | EAC = AC + a fresh ETC | The team can re-estimate the remaining scope honestly | 6.1 + 7.4 = **$13.5m** | Optimism, or no time to do it properly |

Check the third line by hand, because it is the one that gets mangled: CPI × SPI = 0.852 × 0.813 = 0.693, then 6.8 ÷ 0.693 = 9.82, and 6.1 + 9.82 = $15.92m.

A spread of $12.9m to $15.9m out of one dataset. That range is not a weakness in the method. It is the method telling you which assumption you are being asked to sign.

Some organisations soften the schedule effect by weighting rather than multiplying, typically 0.8 × CPI plus 0.2 × SPI in the denominator. It is a documented judgement rule, not a law, and it belongs in the cost control procedure rather than in a spreadsheet invented at month-end.

**Choosing between them.** Ask what caused the variance. A one-off event that has finished argues for the first method; a rate or productivity error argues for the second.

A slipping programme being bought back with overtime argues for the third. A change of scope or method argues for a bottom-up re-estimate, and usually for a change notice as well.

**VAC.** On the CPI method: VAC = 12.0 − 14.08 = **−$2.08m**. That is the number the contingency conversation is really about.

## Earned schedule: why SPI lies near the end

SPI has a structural fault. As a project completes, EV converges on PV, so SPI returns to 1.00 even on a job finishing a year late. It reports on time at the moment it is most wrong.

Earned schedule fixes the units. Rather than asking how much value has been earned, it asks when the baseline said that value would have been earned.

On this package EV = $5.2m. The baseline curve reaches $4.8m of PV at week 16 and $5.4m at week 17. Interpolating: ES = 16 + (5.2 − 4.8) ÷ (5.4 − 4.8) = 16 + 0.67 = **16.67 weeks**.

Actual time (AT) is 20 weeks. So SV(t) = ES − AT = 16.67 − 20 = **−3.33 weeks**, and SPI(t) = 16.67 ÷ 20 = **0.83**.

"We are 3.3 weeks behind" is a sentence a project manager can act on. "SV is minus $1.2m" has to be translated first, and it stops working entirely as the job closes out.

## Earning rules: the decision that sets everything else

Every earned value number depends on how progress was measured. Pick the rule before the work starts, write it into the procedure, and leave it alone mid-package.

| Rule | How it works | Use for | Risk |
|---|---|---|---|
| 0/100 | No credit until complete | Short activities inside one reporting period | Looks harsh early, which is fine, because it cannot be gamed |
| 50/50 | Half on start, half on completion | Two-period activities | Rewards starting things, which is a real behaviour |
| Units complete | Credit per physical unit installed | Piles, cable pulls, welds, square metres | Needs a quantity survey somebody trusts |
| Milestone weighting | Weighted credit at defined checkpoints | Engineering and design deliverables | Milestone definitions drift unless written tightly |
| Percent complete (assessed) | Judgement of the responsible engineer | Where nothing else fits | The weakest rule, and always the one under argument |
| Level of effort | Credit accrues with time | Genuine support functions only | Applied to real work, it manufactures earned value from nothing |

A package with a large share of scope on level of effort will report a CPI near 1.00 whatever happens, because time passing earns value. When a report looks suspiciously well behaved, check the earning rules before you check the crew.

## Where earned value fails

It is blind to quality. Work earned, then ripped out and redone, was counted as value on the day it was installed.

It is blind to scope that was never baselined. If the estimate missed 200 tonnes of steel, earned value will report healthy performance against the wrong scope, which is the most expensive kind of good news.

It depends on a baseline that models the work. A programme resource-loaded to hit a promised date produces a PV curve that nothing can honestly be measured against.

And it needs a cost system that accrues. Where actuals run two months behind, CPI describes the spring, not the month printed on the cover.

## Earned value is not revenue

Earned value is a control number, produced in baseline money to manage a project. Revenue is an accounting number, recognised under the applicable financial reporting standard, and the two exist for different purposes.

They are related. Where progress towards satisfying a performance obligation is measured by an input method based on cost, the measure is costs incurred over total expected costs, which looks like earned value arithmetic and is not the same thing.

The working rule: never let a percentage complete travel from the cost report into the accounts without someone who understands both sides signing it. That handover is exactly where a project reports 62% on the delivery side and something different in the ledger, and nobody notices until the audit.

This overlap is why the [PCI AI Project Controls Leader (PCL-AI) Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) sets project accounting and finance beside the delivery disciplines across its 13 domains and 61 knowledge areas, in the 40 / 40 / 20 proportions of finance and reporting, project management, and governed AI.

## Where AI helps, and where it must not

Useful now: assembling the inputs, flagging control accounts whose behaviour has changed, and testing whether a claimed recovery rate has ever been achieved on comparable work.

Not acceptable: a forecast nobody can explain. If you cannot name the method that produced the EAC and the assumption it rests on, you cannot defend it, and an unexplainable forecast is worse than a wrong one you understand.

Defending a forecast under challenge is what a scenario-based examination is for, and that is how the PCI credentials assess this material rather than by recall.

## Frequently asked questions

**What is a good CPI?**
At or above 1.00 means you are earning at least a unit of budgeted value per unit of cost. Mature projects usually sit between about 0.95 and 1.05, and the trend matters more than the level. A CPI that improves smoothly every month with no management action behind it is normally telling you about the earning rules, not the performance.

**What is the difference between SPI and SPI(t)?**
SPI is measured in money, EV divided by PV, and it converges on 1.00 as a project finishes however late it is. SPI(t) is earned schedule divided by actual time, so it stays meaningful to the end and is expressed in weeks rather than currency. Most controls teams now report both and act on the time one.

**Which EAC method should I use?**
The one matching the cause of the variance. A finished one-off event supports remaining work at budget; a rate or productivity problem supports BAC divided by CPI; a slipping programme bought back with overtime supports the CPI and SPI method. Whichever you pick, record the reasoning in the cost report so the next person can challenge it.

**Does earned value work on agile delivery?**
Yes, with care. Story points or throughput can be converted into a value measure, and the discipline of a baseline, an earning rule and a forecast still applies. What does not survive is claiming credit for work in progress, so treat an unfinished sprint the way you would treat an unfinished activity under a 0/100 rule.

**Is earned value management mandatory?**
It is contractually required on many government and major capital programmes and expected on most large private ones. Elsewhere it is a choice. The practical threshold is whether the project is big enough that you cannot see its position by walking round it, which is roughly the point at which a forecast has to be defended in writing.

**Can I run earned value in a spreadsheet?**
For a single package, yes, and plenty of good cost engineers still do. The difficulty is not the arithmetic but the data: cut-off consistency, accruals, change control and earning rules across dozens of control accounts. Spreadsheets fail on governance long before they fail on calculation.

---

*First published on projectcontrolsinstitute.org; the canonical for this article points there, and Medium links are nofollow, so this republish is here for readers rather than for link equity.*

*Internal links now in the body: one only, to [the PCI AI Project Controls Leader (PCL-AI) Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge), placed where the piece explains that earned value is not revenue and the reader asks where the two are examined together. The second link to the same domain, to the certification page, was removed: this is a republish on an off-estate platform, and two links to one domain in one article is the pattern to avoid. The sentence it sat in was kept intact. The canonical in the front matter already points home, and Medium links are nofollow, so this piece earns readers rather than equity. No further link should be added here.*
