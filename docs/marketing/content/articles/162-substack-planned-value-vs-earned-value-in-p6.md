---
platform:      Substack
type:          guide
title:         Planned value vs earned value in P6: the six settings
meta:          Planned value vs earned value in P6 is decided by settings, not by fieldwork. One activity, one data date, and an SPI that moves from 0.60 to 1.00.
primary_kw:    planned value vs earned value in P6
secondary_kw:  schedule percent complete, performance percent complete, physical percent complete, performance factor
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,622
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00187
---

# Planned value vs earned value in P6: the six settings

Planned value vs earned value in P6 comes down to two multiplications: baseline cost times schedule per cent complete, and baseline cost times performance per cent complete. Both multipliers are chosen in settings rather than measured on site. Two planners with identical fieldwork can report schedule performance indices half a point apart and both be right.

*Written first for this newsletter. The example below is one activity carried through six settings, with the arithmetic on the page, so you can repeat it in your own database before you next issue a report.*

## What decides planned value vs earned value in P6?

Schedule per cent complete answers how much of the baseline duration should have elapsed by the data date. It comes from the baseline dates, not from progress, and it drives planned value.

Performance per cent complete answers how much of the work has been done. Where it comes from is a setting on the WBS, and that setting is the single largest source of unexplained variance in P6 earned value reporting.

Cost variance and schedule variance follow from there. If the multipliers are chosen inconsistently across a programme, every index above them is a blend of measurement and configuration.

## The six settings that decide the answer

| # | Setting | Where it lives | What it changes |
|---|---|---|---|
| 1 | Per cent complete type | Activity, per activity | What "activity per cent complete" means: duration, units or physical |
| 2 | Technique for computing performance per cent complete | WBS, per branch | Whether earned value follows activity progress, 0/100, 50/50 or a custom split |
| 3 | Earned value calculation basis | WBS, per branch | Whether the plan is budgeted cost on baseline dates, or at-completion cost on current dates |
| 4 | Which baseline is assigned | Project and user baseline assignment | Which snapshot supplies budget and baseline dates |
| 5 | Technique for computing ETC | WBS, per branch | The performance factor that turns remaining budget into a forecast |
| 6 | Cost loading and spreading | Resource assignments, curves, expenses | Whether the time-phased plan is linear or curved, and what carries the money |

Five of the six are inherited from whoever built the project or the template. None of them are visible on a printed report.

## One activity, five ways to earn it

Activity A1020, install piperack steel. Baseline cost £400,000, baseline duration 20 working days, data date at the end of baseline day 10.

Actual cost to date is £168,000, and all figures are illustrative.

Schedule per cent complete is 10 ÷ 20 = 50%, so planned value = 400,000 × 0.50 = **£200,000**.

The steel is 300 tonnes erected of 1,000 tonnes. The activity started three days late, so ten calendar days into the window only seven days of its own duration have elapsed. Labour booked is 1,850 hours against a budget of 6,000.

| Performance technique | Multiplier | Earned value | CPI | SPI |
|---|---:|---:|---:|---:|
| Activity %, type = Duration (35%) | 0.350 | £140,000 | 0.833 | 0.700 |
| Activity %, type = Physical (300/1,000 t) | 0.300 | £120,000 | 0.714 | 0.600 |
| Activity %, type = Units (1,850/6,000 h) | 0.308 | £123,333 | 0.734 | 0.617 |
| 0/100 | 0.000 | £0 | 0.000 | 0.000 |
| 50/50 | 0.500 | £200,000 | 1.190 | 1.000 |

The same steel, the same day, the same money spent. Schedule performance index runs from 0.00 to 1.00 and cost performance index from 0.00 to 1.19, entirely on configuration.

Physical per cent complete at 0.300 is the only one measuring the steel. Duration measures the calendar, and units measures the timesheet, which is a proxy for effort rather than for output.

The 50/50 rule earns half the budget for turning up.

Short activities are where 0/100 is honest, because the reporting lag is smaller than the distortion. On a 20-day activity it produces the sawtooth performance curve that makes senior readers stop believing the chart.

## What does the earned value calculation basis change?

Setting 3 is the one that catches experienced people. Left on budgeted values with planned dates, planned value is fixed by the baseline and the comparison stays honest.

Switch it to at-completion values with current dates and the plan re-forms around the current schedule. Take the same activity with an at-completion cost of £430,000 after a change, now forecast to run 24 days from a start three days late.

Elapsed current duration at the data date is 7 of 24, so planned value = 430,000 × (7 ÷ 24) = **£125,417**. Against earned value of £120,000 on physical progress, SPI = 120,000 ÷ 125,417 = **0.957**.

The activity is three days late and 7.5% over budget, and schedule performance now reads within 5% of plan. Nothing improved. The yardstick moved with the work, which is exactly what an earned value measurement exists to prevent.

## Which baseline is P6 actually reading?

There are two assignments and they are not the same thing. The project baseline drives the earned value calculations; the primary user baseline drives most of the baseline columns and bars on a layout.

If those two point at different snapshots, the variance columns on the report and the indices beside them are computed from different budgets. Both look correct and they disagree.

Check the assignment before every reporting cycle, and record the baseline name and date in the report header. A baseline that was re-taken mid-period explains more surprising CPI movements than any site event.

## Which ETC technique should the performance factor use?

Setting 5 chooses how P6 turns remaining budget into an estimate to complete. The options map directly onto [the recognised EAC methods under another name](https://projectcontrolsinstitute.org/four-eac-formulas).

| P6 ETC technique | Equivalent EAC method | Assumption you are signing |
|---|---|---|
| Remaining cost (accrued) | Bottom-up | The remaining plan is still the best estimate |
| PF = 1 | EAC = AC + (BAC − EV) | The overrun is behind you |
| PF = 1 ÷ CPI | EAC = BAC ÷ CPI | Performance to date continues |
| PF = 1 ÷ (CPI × SPI) | EAC = AC + (BAC − EV) ÷ (CPI × SPI) | Recovering the date will keep costing money |
| Custom PF | A judgement you must document | Whatever the number says, in writing |

Leaving this on a default and reading the resulting at-completion column as a forecast is how a schedule tool ends up quietly producing the number a board sees.

## What belongs in the procedure

Fix the settings once, at template level, and write them into the project controls procedure with the reason for each. Six lines of documentation removes an argument that otherwise recurs every month.

Set per cent complete type by discipline, not by preference: physical for measurable installation, units for engineering and management effort, duration only for activities with no other measure.

Then test the template. Load one activity, progress it, and check that the columns produce the numbers you expect before four thousand activities inherit the setting.

One more to watch: the planned value cost column derives from schedule per cent complete, while the time-phased profile follows the resource spread. Give a resource a non-linear curve and the two will not agree, and the report you export decides which one your reader sees.

## Frequently asked questions

**Why does my SPI say 1.0 when the project is clearly late?**
Two common causes. The earned value calculation basis is set to current dates, so the plan has moved with the work and the comparison has been erased. Or performance per cent complete is set to 50/50 on long activities, which earns half of everything that has started. Check setting 3 first, because it takes ten seconds and explains most cases.

**Should schedule performance be read from SPI or from the dates?**
From the dates, once the work is past about 80% complete. SPI is a cost-denominated measure and returns to 1.00 at completion however late the job finishes, because earned value converges on budget at completion. Use SPI for the middle of the job and the critical path for the end of it, and never report SPI alone in the final quarter.

**Is physical per cent complete safe to let engineers enter?**
It is the most accurate measure available and the easiest to inflate, which is the trade. Make it a rules-of-credit calculation rather than an opinion: tonnes erected, welds tested, drawings issued for construction. If the number comes from a count, the entry is a transcription and can be audited. If it comes from a feeling, it is a negotiation.

**Do level of effort activities distort earned value?**
Yes, and the distortion runs in the flattering direction. Level of effort earns against elapsed time, so supervision and site management always show as performing to plan while the discrete work slips. Report discrete work separately, or the management cost you cannot control will hide the installation you can.

**Does P6 need financial periods switched on?**
For cumulative reporting, no. For period-by-period performance, yes, because without stored period performance the current-period figures are derived and change retrospectively when the data date moves. If anyone reports monthly CPI rather than cumulative CPI, store period performance or the series will not reconcile month to month.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so this piece is not a republish of a PCI site page.*

*Linking note — the links now in the body: "the recognised EAC methods under another name" points at projectcontrolsinstitute.org/four-eac-formulas from the paragraph on setting 5, because that sentence raises which forecasting method a P6 performance factor quietly commits you to. That is the only cross-estate link here — nothing else in the piece raises a question another domain answers better, and a second link to the same domain would be a pattern rather than a reference. No reciprocal link is asked for: a hub page pointing back at PCI's own newsletter copy is the manufactured symmetry the link architecture exists to prevent.*
