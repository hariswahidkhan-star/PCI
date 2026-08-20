---
platform:      Quora
type:          qa-list
title:         Control account vs work package: what is the difference?
meta:          A control account is where earned value is measured; a work package is where the work is planned. Worked numbers showing why the roll-up hides overruns.
primary_kw:    control account vs work package
secondary_kw:  control account manager, work breakdown structure, planning package, earned value measurement
pillar:        Earned value management
credential:    PML-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1473
hashtags:      n/a (Quora)
ab_id:         AB-00272
---

# Control account vs work package: what is the difference?

A control account is the management control point where scope, budget, schedule and actual cost meet, and where earned value is measured. A work package is the unit of work planned beneath it. The control account vs work package distinction matters because variance is explained at the control account, while the work is planned, resourced and finished in the packages.

One control account normally holds several work packages. It has one named owner. It is the lowest level at which someone signs for performance.

## Where does a control account sit in the work breakdown structure?

A control account sits at the intersection of a branch of the work breakdown structure and a single organisational owner. That intersection is what makes it a control point rather than a reporting label.

Above it, the WBS aggregates into deliverables and then the project. Below it, the work is decomposed into packages small enough to schedule and measure.

The rule of thumb that holds up in practice: a control account is as small as it can be while still having one person who can genuinely answer for it. Splitting further creates accounts nobody owns; merging further creates accounts nobody can explain.

## What is a work package, and when is it complete?

A work package is a discrete piece of scope with a start, a finish, a budget, an assigned resource and an objective completion test. It is the level at which a planner builds activities and a supervisor allocates people.

The completion test is the part teams skip. "Foundations 70% complete" is a claim; "18 of 26 bases poured and signed off" is a measurement.

Far-term scope that cannot be planned in that detail yet is held in a planning package: budget and schedule inside the control account, not yet broken into work packages. Converting planning packages into work packages as the horizon approaches is rolling wave planning, and it is a normal, disciplined thing to do.

## Control account vs work package: a side-by-side comparison

| Axis | Control account | Work package | Planning package |
|---|---|---|---|
| Position | WBS branch × one owner | Below the control account | Below the control account |
| Owner | Control account manager | Supervisor or lead | Control account manager |
| Holds budget? | Yes, the sum of its packages | Yes, its own share | Yes, unallocated to packages |
| Earned value measured? | Yes — this is the reporting level | Yes, and it feeds upward | No, until it is converted |
| Variance explained? | Yes, in writing, monthly | Diagnostically only | No |
| Detailed schedule? | Rolled up from below | Yes, activity by activity | Milestones only |
| Typical horizon | Whole scope of the branch | Next one to three months | Beyond the planning horizon |
| Changed by | Formal change control | Re-planning within the account | Conversion or change control |

The row to read twice is the one on explaining variance. Reporting at work-package level to a steering committee produces noise; reporting at control-account level without work packages beneath it produces numbers nobody can trace.

## How does the arithmetic actually behave?

Take control account CA-3200, substation civils, with a budget at completion of £4.20m and three work packages. The cut-off is the end of month 8.

| Work package | Budget | PV | EV | AC | CPI | CV |
|---|---:|---:|---:|---:|---:|---:|
| WP-3210 Piling (complete) | £1.50m | £1.50m | £1.50m | £1.38m | 1.087 | +£0.12m |
| WP-3220 Foundations | £1.80m | £1.10m | £0.85m | £1.19m | 0.714 | −£0.34m |
| WP-3230 Cable trenches | £0.90m | £0.30m | £0.25m | £0.28m | 0.893 | −£0.03m |
| **CA-3200 total** | **£4.20m** | **£2.90m** | **£2.60m** | **£2.85m** | **0.912** | **−£0.25m** |

The arithmetic, so it can be checked. EV = 1.50 + 0.85 + 0.25 = £2.60m. AC = 1.38 + 1.19 + 0.28 = £2.85m. CPI = 2.60 ÷ 2.85 = 0.912. SPI = 2.60 ÷ 2.90 = 0.897.

Cost variance at the control account is −£0.25m. That figure is the sum of −£0.34m, +£0.12m and −£0.03m, so a finished favourable package is offsetting about a third of a live overrun on foundations.

## Why does the roll-up hide the problem?

Forecast the control account as a single unit and you get EAC = BAC ÷ CPI = 4.20 ÷ 0.912 = **£4.60m**.

Forecast it package by package and the picture changes. Piling is finished, so its EAC is its actual cost of £1.38m. Foundations has £0.95m of budget left, which at its own CPI of 0.714 costs 0.95 ÷ 0.714 = £1.33m, giving 1.19 + 1.33 = £2.52m. Trenches has £0.65m left at 0.893, so 0.65 ÷ 0.893 = £0.73m, giving 0.28 + 0.73 = £1.01m.

Total: 1.38 + 2.52 + 1.01 = **£4.91m**. That is £0.31m more than the roll-up, on identical data.

The gap exists because dividing by a blended CPI applies a saving that has already been banked to work that has not started. A package that is complete cannot save you any more money. Freeze finished packages at actual cost before you forecast anything.

## Where does the control account meet the ledger?

The control account is usually the level at which cost codes are set, so it is where the delivery structure and the general ledger have to agree. If they do not, actual cost arrives in a shape that cannot be compared to earned value, and every variance conversation becomes an argument about mapping.

It is also where the estimate at completion is built, and the EAC is a financial reporting input, not only a controls one. Where progress towards a performance obligation is measured by a cost-based input method, the measure is costs incurred divided by total expected costs — and total expected costs is the sum of your control account EACs.

On the numbers above, that £0.31m difference is not a presentational nicety. It moves the percentage complete, and the percentage complete moves revenue.

This overlap is the reason PCI examines both sides. The PCI Project Management Leader – AI (PML-AI) credential covers 16 domains and 63 knowledge areas, built so that the person who owns the control account understands what their forecast does to the accounts.

## Frequently asked questions

**How many work packages should a control account have?**
Enough that each package can be scheduled and measured, few enough that the control account manager can hold them in their head. In practice that lands between three and about ten. If an account has thirty packages, it is probably two accounts; if it has one, the account and the package are the same thing and you have gained nothing.

**Can a work package span two control accounts?**
No. A work package belongs to exactly one control account, because a package split across two owners has no single person accountable for its cost. If scope genuinely straddles two owners, split the scope, not the accountability.

**Is a control account the same as a cost account?**
They are often used interchangeably, and in many organisations the control account is the cost account. The distinction worth keeping is that a cost code is an accounting label, while a control account is a management commitment with an owner, a baseline and a monthly explanation attached to it.

**Where does earned value get measured — account or package?**
It is calculated at work-package level using each package's own measurement method, then summed to the control account for reporting. Measuring only at account level loses the diagnosis; reporting only at package level buries the audience. Do both, and report upward.

**What happens to a work package when scope changes?**
A change within the account's approved budget is re-planning: close the package, open a revised one, keep the audit trail. A change to the account's total budget or scope goes through formal change control. Quietly re-baselining a package to erase a variance is the single fastest way to make an earned value system worthless.

**What is a control account manager responsible for?**
Owning the baseline, agreeing the measurement rules, explaining variance in writing each month, and producing the estimate at completion for their scope. The signature matters more than the spreadsheet: a control account with no named manager reverts to a reporting line, and reporting lines do not defend forecasts.

---

*Internal links: this answer should link once, at the end, to [the earned value management pillar](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "how earned value is measured and reported", and to [a full worked month-end](https://projectcontrolsinstitute.org/earned-value-worked-example) with the anchor "a worked month-end example"; Quora links are nofollow, so this is for qualified readers, not link equity.*
