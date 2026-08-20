---
platform:      LinkedIn Article
type:          guide
title:         Earned value misconceptions: six that cost real money
meta:          Six earned value misconceptions corrected with arithmetic: why SPI is not a schedule measure, and why two of the four EAC formulas are one formula.
primary_kw:    earned value misconceptions
secondary_kw:  schedule performance index, estimate at completion, earning rules, cost performance index
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1799
hashtags:      #ProjectControls #EarnedValue #CostEngineering #PMO #ProjectManagement
ab_id:         AB-00217
---

# Earned value misconceptions: six that cost real money

Most earned value misconceptions are not arithmetic errors. They are misreadings of what the numbers mean. SPI is not a schedule measure, percent complete is not progress, and no single EAC formula is the forecast. Earned value is the budgeted cost of the work actually done, expressed in currency, and every misconception below starts by forgetting that.

Written for LinkedIn as an original. It sits under the Institute's earned value management pillar.

## What does earned value actually measure?

Three quantities carry the whole method, and all three are money.

**Planned value (PV)** is the budget for the work that should have been done by now. **Earned value (EV)** is the budget for the work that has been done. **Actual cost (AC)** is what that work has cost.

Everything else is derived. CV = EV − AC. SV = EV − PV. CPI = EV ÷ AC. SPI = EV ÷ PV.

Take a package with BAC £12.0m. At the data date, PV = £5.0m, EV = £4.4m, AC = £5.5m.

CV = 4.4 − 5.5 = **−£1.1m**. SV = 4.4 − 5.0 = **−£0.6m**. CPI = 4.4 ÷ 5.5 = **0.80**. SPI = 4.4 ÷ 5.0 = **0.88**.

For every pound spent, 80 pence of budgeted work exists, and £0.6m of budgeted work that should be there is not. Those are the only two statements the raw indices support.

## Does SPI tell you whether the project is late?

No. SPI is a currency ratio, and it returns to 1.00 on the day the last activity finishes, however late that day is.

At completion, EV equals BAC because all the work is done, and PV also equals BAC because all the work was planned. SPI = BAC ÷ BAC = **1.00**, on a job that finished six months late.

Work the example. Planned duration 24 months, BAC £12.0m, straight-line plan of £0.5m per month. At month 18: PV £9.0m, EV £7.2m, so SPI = 7.2 ÷ 9.0 = **0.80**.

The project actually finishes in month 30. At that point EV = PV = £12.0m and SPI = **1.00**. The index has quietly deleted a six-month overrun.

Earned schedule converts EV back into time. Earned schedule (ES) is the point on the baseline at which the EV you have earned should have been earned.

£7.2m of planned value is reached at month 14.4 on this baseline. So ES = 14.4, actual time (AT) = 18, and SPI(t) = 14.4 ÷ 18 = **0.80**.

Forecast duration = planned duration ÷ SPI(t) = 24 ÷ 0.80 = **30 months**. That is exactly what happened, and it was visible in month 18.

Use SPI to talk about the value of missing work, and earned schedule to talk about dates. Float in the network remains the authority on which activities matter.

## Is percent complete the same as progress?

No. Percent complete is an opinion until an earning rule turns it into a measurement. The rule, agreed before the work starts, is what makes EV auditable.

| Earning rule | How EV is claimed | Where it fits | Failure mode |
|---|---|---|---|
| 0/100 | Nothing until the package is complete | Activities shorter than one reporting period | Long tasks look dead for months |
| 50/50 | Half on start, half on completion | Repetitive packages of similar size | Flatters the early months |
| Units complete | Quantity installed × unit budget | Piling, cabling, pipe, concrete | Only as good as the quantity base |
| Milestone weighting | Fixed percentages at defined, evidenced events | Engineering and design | Weights get negotiated, not measured |
| Level of effort | Earns to plan with the passage of time | Supervision, PMO, site management | Can never show a schedule variance |
| Apportioned effort | Earns in proportion to a discrete base | Inspection, QA, commissioning support | Inherits every error in the base |

Level of effort deserves a cap. If a third of your baseline earns automatically with the calendar, a third of your SPI is a clock, not a measurement.

The test of an earning rule is whether two people looking at the same site produce the same number. If they cannot, the rule is a conversation, not a control.

## Do the four EAC formulas give four different answers?

They give three. Two of the standard four are the same formula written twice, and a surprising number of cheat sheets present them as alternatives.

Using the package above: BAC £12.0m, EV £4.4m, AC £5.5m, CPI 0.80, SPI 0.88, remaining work BAC − EV = £7.6m.

| Method | Formula | What it assumes | EAC | VAC |
|---|---|---|---|---|
| 1 | AC + (BAC − EV) | The overrun was a one-off. Remaining work runs to budget | **£13.10m** | −£1.10m |
| 2 | BAC ÷ CPI | Remaining work runs at cumulative cost performance to date | **£15.00m** | −£3.00m |
| 3 | AC + (BAC − EV) ÷ CPI | Identical to method 2, algebraically | **£15.00m** | −£3.00m |
| 4 | AC + (BAC − EV) ÷ (CPI × SPI) | Schedule pressure costs money as well as time | **£16.30m** | −£4.30m |

Methods 2 and 3 are the same number because BAC ÷ CPI = BAC × AC ÷ EV, and AC + (BAC − EV) × AC ÷ EV collapses to AC × BAC ÷ EV. The same expression, rearranged.

Method 1 is a claim, not a calculation. It asserts that a specific identified cause produced the £1.1m variance and that the cause has gone. If you cannot name it, you are not entitled to the formula.

Method 4 is the pessimistic case and it double-counts when the schedule slip is caused by something that does not consume cost, such as a permit wait.

The fifth method is the one that gets signed: a bottom-up re-estimate of the remaining scope, with the index figures used to challenge it. When bottom-up says £13.5m and method 2 says £15.0m, the gap is the conversation.

## Does a CPI above 1.0 mean cost is under control?

Not on its own. A green CPI is often an invoicing lag wearing a cost report as a disguise.

Take EV £4.4m against posted actual cost of £4.1m. CPI = 4.4 ÷ 4.1 = **1.07**, and the package reports green.

Goods received but not invoiced stand at £0.9m. True AC is £5.0m, so CPI = 4.4 ÷ 5.0 = **0.88**.

Push both through method 2. At CPI 1.07, EAC = 12.0 × 4.1 ÷ 4.4 = **£11.18m**. At CPI 0.88, EAC = 12.0 × 5.0 ÷ 4.4 = **£13.64m**.

A **£2.45m** swing in the forecast, produced entirely by whether the accrual was captured. The earned value pack and the month-end close should be one exercise, not two with the same title.

## Is earned value only for large government contracts?

No. What earned value needs is a scope baseline, a time-phased budget and an earning rule for every work package. None of that requires a resource-loaded programme of ten thousand activities.

A £4m fit-out with forty work packages, a monthly data date and units-complete earning gives usable CPI and SPI within two periods.

What scales badly is the reporting apparatus, not the method. A defence-programme control account structure imposed on a forty-package job produces administration and no signal.

## Are variance thresholds just a reporting formality?

Only where nobody has attached a name and a required response to them. Thresholds that trigger a paragraph are decoration. [Thresholds that trigger an owner, a date and a decision](https://projectcontrolsinstitute.org/earned-value-reporting-thresholds) are controls.

The honest companion to a threshold is the to-complete performance index. TCPI to BAC = (BAC − EV) ÷ (BAC − AC) = 7.6 ÷ 6.5 = **1.169**.

Against a cumulative CPI of 0.80, the remaining work must run about 46% more efficiently than everything so far. When TCPI ÷ CPI exceeds roughly 1.1, recovery to budget is a story rather than a plan.

## Why do earned value misconceptions survive?

Because each of them makes a report easier to sign. SPI returning to 1.00 removes an awkward conversation about dates. A missing accrual makes CPI green. Method 1 makes the EAC smaller.

None of these are lies when they are written. They become lies when the writer knows the assumption and does not disclose it, which is a competence question before it is an ethics one.

That gap between the person who owns the earning rule and the person who owns the ledger is exactly what the PCI AI Project Controls Leader (PCL-AI) credential examines, across 13 domains and 61 knowledge areas.

## Frequently asked questions

**Can SPI ever be trusted as a schedule indicator?**
Early in a project, before the plan starts converging, SPI carries useful information about the value of work not yet done. Past roughly the two-thirds point it degrades, because the remaining planned value shrinks and the ratio drifts back to 1.00 regardless of the finish date. Use earned schedule for anything you intend to say about dates.

**Which EAC method should we report?**
Report the bottom-up re-estimate as the forecast and show method 2 alongside it as the independent check. If the two disagree by more than about 10%, the difference belongs in the narrative with a named reason. Method 1 is only defensible when you can identify the specific one-off event behind the variance.

**Does earned value work on agile or iterative delivery?**
Yes, with the work package replaced by the story or feature and the earning rule set to complete-only, because partially done increments earn nothing. What breaks is the time-phased baseline, so the baseline is re-established each release rather than held for the programme. The indices then describe the release, not the portfolio.

**Why does our CPI change when nothing on site changed?**
Almost always accruals, retention or a cost transfer between codes. EV moves with physical work while AC moves with the ledger, and the two are on different clocks. Fix the cut-off discipline before you interpret a single point of CPI movement as a performance change.

**Is a negative schedule variance always bad?**
No. SV = EV − PV compares against the plan, so a package resequenced deliberately to protect the critical path shows a negative SV while doing exactly what it should. Read SV alongside total float on the driving activities. A negative SV on a path with 40 days of float is a different problem from one on the critical path.

---

*PCI publishes certification requirements. Nothing in this article is legal, tax or accounting advice, and the technical methods described here belong to the discipline rather than to the Institute.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Internal links: one link is in the body, in the threshold section, where the sentence separates a threshold that produces a paragraph from one that produces a decision. "Thresholds that trigger an owner, a date and a decision" points to https://projectcontrolsinstitute.org/earned-value-reporting-thresholds, which is how those gates are designed. The standfirst pillar mention and the PCL-AI mention are left unlinked: three links to one domain from a single piece is the density that gets a group of sites devalued together. Reciprocal: https://projectcontrolsinstitute.org/earned-value-management could cite this piece for the proof that methods 2 and 3 are the same formula.*
