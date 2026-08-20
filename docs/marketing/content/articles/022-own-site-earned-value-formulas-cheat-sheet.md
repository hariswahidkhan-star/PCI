---
platform:      Own site — projectcontrolsinstitute.org
type:          guide
title:         Earned value formulas cheat sheet, explained properly
meta:          An earned value formulas cheat sheet with units, sign conventions and one worked dataset carried through every formula, plus earned schedule and TCPI.
primary_kw:    earned value formulas cheat sheet
secondary_kw:  CPI formula, TCPI, earned schedule, variance at completion
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1444
hashtags:      n/a (own site)
ab_id:         AB-00032
---

# Earned value formulas cheat sheet, explained properly

An earned value formulas cheat sheet needs four things a printed list usually leaves out: the units each measure is in, the sign convention, what the formula assumes, and one dataset carried through all of them so the numbers agree. This page gives all four, then shows where each formula stops working.

Everything below is built from four inputs: PV, EV, AC and BAC.

## The four inputs, defined so they stand alone

**Planned value (PV)** is the budgeted cost of the work scheduled to be complete at the cut-off date. It comes from the approved baseline and moves only through change control.

**Earned value (EV)** is the budgeted cost of the work actually complete at that date, measured in baseline money rather than in what the work cost.

**Actual cost (AC)** is the cost incurred for that same work to that same date, including accruals for goods received and work done but not yet invoiced.

**Budget at completion (BAC)** is the total authorised budget for the scope being measured, at whatever level you are reporting: control account, work package or project.

## The worked dataset

One project, carried through every formula on this page. A process plant control system, BAC $8.0m over 24 months, reporting at the end of month nine.

At cut-off: PV = $3.2m, EV = $2.9m, AC = $3.4m.

## What belongs on an earned value formulas cheat sheet

| Measure | Formula | Units | Our numbers | Reads as |
|---|---|---|---|---|
| Cost variance | CV = EV − AC | currency | 2.9 − 3.4 = **−$0.5m** | Negative: paid more than the work was worth |
| Schedule variance | SV = EV − PV | currency | 2.9 − 3.2 = **−$0.3m** | Negative: less work done than planned, in money |
| Cost variance % | CV% = CV ÷ EV | per cent | −0.5 ÷ 2.9 = **−17.2%** | Overspend as a share of value earned |
| Schedule variance % | SV% = SV ÷ PV | per cent | −0.3 ÷ 3.2 = **−9.4%** | Shortfall as a share of value planned |
| Cost performance index | CPI = EV ÷ AC | ratio | 2.9 ÷ 3.4 = **0.853** | Value earned per unit spent |
| Schedule performance index | SPI = EV ÷ PV | ratio | 2.9 ÷ 3.2 = **0.906** | Value earned per unit planned |
| Per cent complete | EV ÷ BAC | per cent | 2.9 ÷ 8.0 = **36.3%** | Work done by value |
| Per cent spent | AC ÷ BAC | per cent | 3.4 ÷ 8.0 = **42.5%** | Budget consumed |
| Estimate to complete | ETC = EAC − AC | currency | see below | Cost of the work remaining |
| Estimate at completion | EAC = AC + ETC | currency | see below | Forecast outturn |
| Variance at completion | VAC = BAC − EAC | currency | see below | Negative: an overrun |
| To-complete index (to BAC) | (BAC − EV) ÷ (BAC − AC) | ratio | 5.1 ÷ 4.6 = **1.109** | Efficiency needed to finish on budget |
| To-complete index (to EAC) | (BAC − EV) ÷ (EAC − AC) | ratio | see below | Efficiency needed to hit the forecast |

Sign convention throughout: negative is bad on the variances, below 1.00 is bad on the indices. The only exception is TCPI, where a number above your current CPI is the warning.

## The four forecasts, from the same numbers

Remaining budgeted work is BAC − EV = 8.0 − 2.9 = **$5.1m**.

| Method | Formula | EAC | VAC | Assumes |
|---|---|---:|---:|---|
| Remaining work at budget | AC + (BAC − EV) | **$8.50m** | −$0.50m | The overrun is behind you |
| Remaining work at current CPI | BAC ÷ CPI | **$9.38m** | −$1.38m | Today's efficiency continues |
| Remaining work at CPI and SPI | AC + (BAC − EV) ÷ (CPI × SPI) | **$10.00m** | −$2.00m | Schedule pressure keeps costing money |
| Bottom-up re-estimate | AC + a fresh ETC of $5.6m | **$9.00m** | −$1.00m | The team re-estimates honestly |

The third line is worth checking by hand, because it is the one people get wrong. CPI × SPI = 0.853 × 0.906 = 0.773. Then 5.1 ÷ 0.773 = 6.60, and 3.4 + 6.60 = $10.00m.

Now the pair that catches people out. TCPI measured against the CPI-based EAC is 5.1 ÷ (9.38 − 3.4) = 5.1 ÷ 5.98 = **0.853**, which is exactly the CPI you started with.

That is not a coincidence and it is not a result. A forecast built by assuming today's efficiency continues will always tell you that today's efficiency is enough to hit it. Test the forecast against BAC, where TCPI is 1.109 against an actual 0.853, a 30% improvement required.

## Cumulative and period: the same formula, two answers

Every index on the sheet can be run on cumulative figures or on the period alone, and the two say different things.

Say last month's cumulative position was EV $2.62m and AC $2.95m. The period figures are EV of 2.9 − 2.62 = **$0.28m** and AC of 3.4 − 2.95 = **$0.45m**.

Period CPI = 0.28 ÷ 0.45 = **0.622**, against a cumulative CPI of 0.853.

Cumulative CPI is a heavy flywheel. It moves slowly, which is what makes it a fair basis for a forecast and a poor basis for noticing that this month went badly. Report both, forecast from the cumulative, and act on the period.

## Earned schedule: the formulas the printed sheets omit

SPI is denominated in money, so as a project finishes EV converges on PV and SPI returns to 1.00 whether or not the work is late. Earned schedule restates the same idea in time.

Earned schedule (ES) is the point on the baseline curve at which the PV equals today's EV. Our EV is $2.9m; the baseline reaches $2.74m at month eight and $3.20m at month nine.

ES = 8 + (2.90 − 2.74) ÷ (3.20 − 2.74) = 8 + 0.16 ÷ 0.46 = **8.35 months**.

With actual time (AT) of 9.00 months: SV(t) = ES − AT = **−0.65 months**, and SPI(t) = ES ÷ AT = 8.35 ÷ 9.00 = **0.928**.

The time forecast follows: IEAC(t) = PD ÷ SPI(t), where PD is the planned duration. Here 24 ÷ 0.928 = **25.9 months**, so about two months late on current performance.

## Where each formula stops working

| Formula | Breaks when |
|---|---|
| SV and SPI | The project nears completion; both drift to zero and 1.00 regardless of lateness |
| CPI | Actuals lag, so the index describes an earlier month than the one on the cover |
| EAC = BAC ÷ CPI | Early in the job, when CPI is built on a small and unrepresentative sample |
| CPI × SPI method | The schedule slip has a cause that does not consume cost, such as a permit wait |
| TCPI | Measured against an EAC derived from CPI, where it simply returns CPI |
| Per cent complete | A large share of scope sits on level of effort, which earns value from time passing |
| All of them | The baseline was built to hit a promised date rather than to model the work |

## One caution about what these numbers are for

Per cent complete on this sheet is 36.3%, built in baseline money to control a project. It is not a revenue figure, and an input method of measuring progress towards a performance obligation under the applicable accounting standard uses costs incurred against total expected costs, which is different arithmetic with a different purpose.

The two are related and they are not interchangeable. Where a controls percentage is handed to finance without that being understood, the delivery report and the ledger start describing different projects.

## Frequently asked questions

**Do I need to memorise these for an exam?**
You need to be able to derive them, which is a different skill. Scenario-based examinations tend to give you a dataset with a defect in it, such as an inconsistent cut-off or materials sitting in actual cost, and the marks are in noticing that before you calculate anything. The formulas themselves take an afternoon; the judgement takes practice.

**What is the difference between ETC and the remaining budget?**
Remaining budget is BAC − EV, the budgeted cost of work not yet earned. ETC is what that work will actually cost, which is the same figure only if you expect performance to return to plan immediately. On our numbers, the remaining budget is $5.1m and the CPI-based ETC is $5.98m, and the gap between them is the forecast overrun.

**Why is SPI in money rather than time?**
Because it is built from the same value curve as everything else, and value is the only unit PV and EV share. That makes SPI easy to compute and hard to interpret, which is why earned schedule exists. Most controls teams now report SPI for continuity and act on SPI(t).

**Can CPI be above 1.00 on a project that is failing?**
Yes, and it is a familiar pattern. Under-claimed progress, generous earning rules or actuals that have not caught up will all lift CPI. So will a project that is quietly deferring work rather than doing it, since neither EV nor AC accrues on work nobody has started.

**What is a sensible reporting threshold on these variances?**
A percentage gate on its own floods you with small accounts and misses large ones, so most mature systems pair a percentage with an absolute value, and add a second higher absolute trigger that fires regardless of percentage. The thresholds belong in the baseline documentation, set once, and not adjusted at month-end to avoid writing a report.

---

*Internal links: this piece should link to [the earned value management pillar](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "the earned value management pillar", to [the full month-end example](https://projectcontrolsinstitute.org/earned-value-worked-example) with the anchor "these formulas applied to a full month-end", and to [the reporting thresholds guide](https://projectcontrolsinstitute.org/earned-value-reporting-thresholds) with the anchor "when a variance should trigger action".*
