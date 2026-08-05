---
id: BPG-04
series: S09
series_name: Best Practice Guides
title: Baselining and baseline change control
subtitle: What a baseline actually freezes, what the gate has to test, and why a variance you baseline away is still there
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: professional
reading_time_min: 16
summary: >
  What a project baseline is composed of and what it genuinely freezes, what a change-control gate must
  test before it approves anything, how re-baselining differs from rolling wave, and why moving a
  baseline to absorb a variance destroys the only signal the project had. Includes a worked comparison
  of the correct treatment of an approved scope addition against a re-baseline to forecast, showing what
  each does to the reported variance and to the funding question underneath it.
linkedin:
  format: article
  hook: >
    Re-baselining to the current forecast makes the variance disappear from the report. It does not make
    the money appear in the budget — the funding gap survives, now with nobody accountable for it.
  tags: [ProjectControls, BaselineManagement, ChangeControl, EarnedValue, ProjectGovernance]
  asset: one-pager
gated: false
related: [BPG-02, BPG-06, BPG-08, BPG-10, BPG-11, TPL-04]
bok_domains: [3, 5, 8, 10]
sources: []
placeholders: 0
---

# Baselining and baseline change control

> The composition of a baseline, the gate that protects it, and the discipline of keeping a variance
> visible.

**In one paragraph.** What a project baseline is composed of and what it genuinely freezes, what a
change-control gate must test before it approves anything, how re-baselining differs from rolling wave,
and why moving a baseline to absorb a variance destroys the only signal the project had. Includes a
worked comparison of the correct treatment of an approved scope addition against a re-baseline to
forecast, showing what each does to the reported variance and to the funding question underneath it.

**Who this is for.** Project controls managers and control account managers who maintain a baseline;
project managers and sponsors who sit on the change board; anyone being asked to approve a re-baseline
and wondering what they are actually approving.

---

## 1. A baseline is a claim, not a number

A baseline is the project's formal statement that *this scope, delivered this way, will take this long
and cost this much*. Its value comes entirely from being fixed: a number that moves whenever
performance disappoints measures nothing, because every comparison against it is a comparison with
itself.

That is the whole argument, and it is worth stating before the mechanics, because most baseline
failures are not procedural. They happen because somebody found the variance uncomfortable and the
baseline was the softest thing in the room.

A useful test for whether a project has a baseline at all: can you produce, in under an hour, the exact
version of the scope, schedule and cost plan that the current variance is measured against, together
with the list of changes approved into it since sanction? If not, the project has a plan and a forecast
but nothing in between.

## 2. What the baseline is composed of

Three components, approved together, each with its own version.

**The scope baseline** — the work breakdown structure (WBS), the dictionary, and the scope statement it
implements. This is the reference for every subsequent question about whether something is a change or
was always included. `BPG-02 — The work breakdown structure` covers its construction.

**The schedule baseline** — the network, durations, logic, calendars and constraints as approved, with
the schedule basis document that records what the durations assume. A baselined schedule with no basis
document is a set of dates whose reasoning has been discarded.

**The cost baseline** — the budget, time-phased across the schedule at control-account level. This is
the planned value curve, and its time-phasing is what makes a cost variance distinguishable from a
timing difference.

Two totals sit around these. **Budget at completion** is the sum of the time-phased budgets plus
distributed contingency where the organisation's convention places it inside the baseline. Management
reserve normally sits *outside* the performance measurement baseline and is not available to a control
account without a formal transfer. Conventions differ legitimately between organisations, so the rule
is to state yours in the controls execution plan and apply it consistently rather than to assume; the
treatment of both reserves is owned by `BPG-10 — Contingency and management reserve`.

## 3. What a baseline actually freezes

This is where most disputes originate, because the baseline freezes less than people assume and more
than they would like.

**It freezes the measurement reference.** Every variance reported from now on is measured against this
version. That is its primary function.

**It freezes the scope commitment.** Work not in the scope baseline is, by definition, a change — which
is why an ambiguous WBS dictionary entry is a commercial exposure and not just untidiness.

**It freezes the time-phasing at control-account level.** Not at activity level: activities can and
will move within a control account as the schedule is updated, and that movement is progress
management, not baseline change. The distinction between "the schedule has been updated" and "the
schedule baseline has been changed" is one every planner must be able to state on demand.

**It does not freeze the forecast.** The forecast is expected to move every period; that is what makes
it a forecast. Confusing the two produces the request to "update the baseline to match the forecast",
which is §7's subject.

**It does not freeze the method.** How a work package earns progress can be corrected if the rule was
wrong, though the correction must be applied to the whole package and disclosed, because it will move
the reported earned value without anything physical happening.

**It does not freeze reality.** A baseline that has become impossible should be said to be impossible.
The correct response is a change request with a defensible recovery plan, not a quiet re-phasing.

## 4. The change-control gate

A change-control gate is not an approval meeting; it is a test. A change request that passes it should
have answered six questions in writing, and a board that approves without them is rubber-stamping.

1. **What exactly changes in the scope baseline?** Named WBS elements, with dictionary amendments
   drafted. "Additional works to the inlet structure" is not an answer.
2. **What is the cost impact, in full?** Direct cost, indirect and time-related cost, escalation on the
   new scope, and the effect on any allowance. A cost impact stated without its time-related component
   is usually understated.
3. **What is the schedule impact?** Not "four weeks" but which activities, which logic, and what
   happens to the critical path and to float elsewhere. A change consuming somebody else's float is
   still consuming a project asset.
4. **Where does the money come from?** Contingency drawdown, management reserve transfer, client
   variation, or an unfunded increase in budget at completion. "To be confirmed" means the change is
   not ready.
5. **What is the impact of *not* approving it?** The counterfactual is the only way a board can
   distinguish a genuine requirement from a preference.
6. **What is the effect on reported performance?** Specifically: does approval change any historical
   variance? The correct answer is almost always no, and if the proposal says yes, §7 applies.

Authority thresholds should be set so that the great majority of changes are approved at the lowest
competent level, with a documented escalation. A gate that sends everything to the sponsor is a gate
that will be bypassed. `TPL-04 — Baseline change request` provides the instrument.

**The trend register is the front end of the gate.** A trend is a change that has been identified but
not yet approved — a design development, a quantity growth, an early warning from a subcontractor. It
is logged with an estimated value the moment it is identified, and it appears in the forecast before it
appears in the baseline. Projects that log trends only when they become change requests report a
forecast that jumps in steps, always upward, always late. `BPG-11 — Change orders and variations`
covers the commercial instrument; the trend register is the internal one, and it should run ahead of it.

## 5. Rolling wave is not re-baselining

Rolling wave planning holds future scope as **planning packages** — real elements with budget and
approximate duration, but no work-package detail — and converts them to detailed work packages as the
scope becomes definable.

The conversion moves budget *within* the baseline. The budget at completion is unchanged, the total
time-phased budget is unchanged, and no historical variance is affected. It is a planning act, not a
change, and it does not go through the change gate — though it should be recorded, because a conversion
that changes the phasing of a planning package's budget will move the planned value curve and therefore
the schedule performance index.

Two disciplines keep this honest. The conversion must be **budget-neutral**: if the detailed estimate
of the newly defined work exceeds the planning package's budget, that excess is a change and goes
through the gate — it does not get absorbed by quietly enlarging the package. And planning packages
must have a **conversion deadline**: undefined scope held indefinitely is not rolling wave, it is
undistributed budget with no plan, and it hides both scope risk and estimate risk until very late.

## 6. Re-baselining

There are legitimate reasons to re-baseline. A major approved scope change that reshapes the project; a
contractual reset; a suspension and restart; an original baseline so far from reality that variance
reporting against it has stopped conveying information. In each case the honest test is whether the
existing baseline can still support a *decision*, not whether it is uncomfortable.

A re-baseline done properly has five properties. The old baseline is **archived and remains
retrievable**, not overwritten. The **reason is documented** and approved at the level that approved
the original. The **cumulative variance to date is stated explicitly** in the approval — the number
that is about to disappear from the report is written into the record. The **funding position is
resolved**: if the new baseline is larger, the source of the additional funds is named. And **reporting
continues against both** for at least one cycle, so readers can see the step.

Where none of those five is present, what is happening is not a re-baseline.

## 7. Keeping the variance visible

The most consequential discipline in this guide is the refusal to let a baseline absorb a performance
problem.

The request is always reasonable-sounding. The baseline is out of date; the variance is historical; the
team has a credible recovery plan; reporting a large negative variance every month is demoralising and
distracts from the work. All of that can be true, and the answer is still no — because the variance is
the project's only quantitative record that performance has not matched the plan, and removing it
removes the basis for every remaining forecast.

Three consequences follow from baselining a variance away, and §9 demonstrates each.

**The forecast loses its evidence.** A cost performance index of 0.88 is the empirical basis for
expecting the remaining work to cost more than budgeted. Reset it to 1.00 and the forecast has to be
justified from optimism instead of from data. `BPG-09 — Estimate at completion` depends on this history
existing.

**The funding gap survives.** Moving the baseline changes what the report compares against; it does not
change what the project will cost or what has been sanctioned. The difference between the new baseline
and the authorised funding is a real gap that now belongs to nobody.

**The trend is destroyed.** Performance trend — is this getting worse, stable, or recovering? — is the
single most useful thing a controls function produces, and it requires an unbroken series. A reset
breaks it, and no amount of subsequent reporting reconstructs it.

The alternative is neither difficult nor dramatic: report the variance, report the cause, report the
recovery plan and the evidence for it, and let the trend show whether the recovery is happening. A
board that sees the same negative variance narrowing over four periods is being told something true. A
board that sees it vanish is being told nothing.

## 8. How this goes wrong

**The baseline is set before the scope is decomposed.** A sanction total is declared the baseline. With
no time-phasing, the first variance question — over, or early? — is unanswerable, and the project
quietly rebuilds the baseline retrospectively from whatever was spent.

**Approved changes are not incorporated.** Change requests are approved but never applied to the
baseline. The baseline and the authorised scope drift apart, and the reported variance becomes the sum
of a real performance problem and an administrative backlog, with no way to separate them.

**Historical earned value is restated when scope is added.** New scope is added to the baseline *and*
backdated into the earlier periods, so past performance improves without anything having happened.
Added scope enters from the approval date forward.

**The schedule is re-baselined and the cost baseline is not.** The two now describe different plans.
Every schedule performance index computed from them is meaningless, and nobody notices for a quarter
because the numbers still look like numbers.

**Contingency is drawn down without a change record.** Money moves from the allowance into a control
account to cover an overrun. The control account now shows no variance, the allowance is depleted, and
there is no record connecting the two. This is baselining away a variance with extra steps.

**"Re-baseline" means "delete the history".** The old baseline is overwritten in the tool. Nobody can
now state what the project originally committed to, which makes the closeout lessons exercise in
`BPG-20` impossible to run.

**Planning packages never convert.** Undefined budget sits in planning packages until late execution,
by which time the estimate is worse and the schedule is fixed. The rolling wave discipline exists to
prevent exactly this, and it works only with conversion deadlines.

## 9. Worked example

*Illustrative figures.* Generic currency units. Period: month 8 of a monthly reporting cycle. Values
are cumulative to the data date unless stated. Rounding: indices to three decimal places, currency to
the nearest 1,000 where a rounded figure is presented. No real project is implied.

### 9.1 The position at month 8

| Measure | Value |
|---|---:|
| Budget at completion (BAC) | 6,000,000 |
| Planned value (PV) | 2,400,000 |
| Earned value (EV) | 2,040,000 |
| Actual cost (AC) | 2,310,000 |

Cost variance (CV), schedule variance (SV), cost performance index (CPI) and schedule performance index
(SPI) follow directly:

```
CV  = EV − AC = 2,040,000 − 2,310,000 = −270,000
SV  = EV − PV = 2,040,000 − 2,400,000 = −360,000
CPI = EV ÷ AC = 2,040,000 ÷ 2,310,000 = 0.883
SPI = EV ÷ PV = 2,040,000 ÷ 2,400,000 = 0.850
```

Forecasting on the assumption that performance to date persists — one method among several, and
`BPG-09 — Estimate at completion` owns the choice — gives an estimate at completion (EAC) and a
variance at completion (VAC):

```
EAC = BAC ÷ CPI = 6,000,000 ÷ 0.883116 = 6,794,000  (to the nearest 1,000)
VAC = BAC − EAC = 6,000,000 − 6,794,000 = −794,000
```

The unrounded index 2,040,000 ÷ 2,310,000 = 0.883116 is used in the division; using the rounded 0.883
would give 6,795,000, a difference of 1,000 that illustrates why indices are carried unrounded through
a calculation and rounded only at presentation.

### 9.2 An approved scope addition, treated correctly

A change request for additional scope valued at **480,000** is approved at the month-8 board, funded by
a client variation. The correct treatment adds the new scope's time-phased budget to the baseline from
the approval date forward, and leaves every historical figure alone.

```
New BAC = 6,000,000 + 480,000 = 6,480,000
EV, AC and the historical CPI are unchanged: CPI remains 0.883
EAC = AC + (BAC − EV) ÷ CPI
    = 2,310,000 + (6,480,000 − 2,040,000) ÷ 0.883116
    = 2,310,000 + 4,440,000 ÷ 0.883116
    = 2,310,000 + 5,028,000
    = 7,338,000  (to the nearest 1,000)
VAC = 6,480,000 − 7,338,000 = −858,000
```

The forecast overrun has grown from 794,000 to 858,000 — because the added scope is assumed to be
delivered at the same efficiency as the work so far, which is an assumption the board should be asked
to accept or reject explicitly. The performance signal is intact: CPI is still 0.883, the trend is
continuous, and the board can see both that scope was added *and* that the project is running over.

### 9.3 The same month, re-baselined to forecast

Now suppose the request is instead to re-baseline the account to the current forecast, resetting earned
value to actual cost at the data date so that the account "starts clean".

| Measure | Correct treatment (§9.2) | Re-baselined to forecast |
|---|---:|---:|
| BAC | 6,480,000 | 7,338,000 |
| CPI to date | 0.883 | 1.000 |
| CV to date | −270,000 | 0 |
| VAC | −858,000 | 0 |
| Forecast cost of the project | 7,338,000 | 7,338,000 |
| Authorised funding | 6,480,000 | 6,480,000 |
| **Unresolved funding gap** | **858,000, reported** | **858,000, not reported** |

Both columns describe the same project, costing the same money. Nothing physical differs. What differs
is that in the right-hand column the 858,000 gap between what the project will cost and what has been
authorised no longer appears anywhere in the performance report — and the 0.883 that was the evidence
for expecting it has been replaced by a 1.000 that is evidence of nothing.

Check: 7,338,000 − 6,480,000 = 858,000, which equals the VAC in the correct treatment. The gap is the
same number under both treatments, which is the point.

### 9.4 Rolling wave, for contrast

The same project holds a planning package of **1,200,000** for later scope. In month 9 that scope
becomes definable and is decomposed into five work packages:

```
210,000 + 260,000 + 190,000 + 300,000 + 240,000 = 1,200,000
```

Budget at completion is unchanged at 6,480,000. No historical variance moves. No change request is
required, because no scope and no budget has changed — only the level of detail at which it is planned.
Had the detailed estimate come to 1,340,000, the excess of 1,340,000 − 1,200,000 = **140,000** would be
a change, and would go through the gate in §4 with its funding source named.

## 10. Checklist

Take this into the change board, or into the meeting where a re-baseline is being proposed.

**Before approving any change**

- [ ] Which named WBS elements change, and are the dictionary amendments drafted?
- [ ] Does the cost impact include time-related cost, escalation on the new scope, and any allowance effect?
- [ ] Which activities and which logic change, and what happens to the critical path and to others' float?
- [ ] Where does the money come from — contingency, reserve, client variation, or an unfunded increase?
- [ ] What happens if this is not approved?
- [ ] Does approval alter any historical variance? (If yes, ask why before anything else.)

**Baseline hygiene**

- [ ] Can the current baseline version and its change history be produced in under an hour?
- [ ] Have all approved changes actually been incorporated, and how many are outstanding?
- [ ] Do the schedule baseline and the cost baseline describe the same plan?
- [ ] Is there a schedule basis document, and was it updated at the last change?
- [ ] Is management reserve outside the performance measurement baseline, and is the convention written down?

**Rolling wave**

- [ ] Does every planning package have a conversion deadline?
- [ ] Have conversions been budget-neutral, and where they were not, did the excess go through the gate?
- [ ] What proportion of the budget at completion is still undefined?

**If a re-baseline is proposed**

- [ ] Is the old baseline archived and retrievable, or is it about to be overwritten?
- [ ] Is the cumulative variance being written off stated as a number in the approval?
- [ ] Is the funding gap between the new baseline and the authorised amount identified and owned?
- [ ] Will both baselines be reported for at least one cycle?
- [ ] If the answer to any of the above is no: what is being proposed is not a re-baseline.

---

## Related

- `BPG-02 — The work breakdown structure` — the scope baseline this depends on, and why dictionary exclusions are a change-control asset.
- `BPG-06 — Progress measurement and rules of credit` — the earning rules that make the baseline measurable.
- `BPG-08 — Earned value in practice` — how the variances in §9 are computed and read.
- `BPG-10 — Contingency and management reserve` — the reserve conventions referenced in §2, and drawdown discipline.
- `BPG-11 — Change orders and variations` — the commercial instrument that sits alongside the internal gate.
- `TPL-04 — Baseline change request` — the instrument implementing the six-question gate in §4.

## Sources and standards

Drawn from the Institute's Body of Knowledge: Domain 3 (Budgeting and Forecasting) for the time-phased
cost baseline and reserves, Domain 5 (Cost Management and Cost Control) for change control and cost
impact assessment, Domain 8 (Project Management Lifecycle) for the scope baseline and integrated change
control, and Domain 10 (Project Scheduling) for the schedule baseline and its basis document.

The six-question gate in §4 and the five properties of a legitimate re-baseline in §6 are PCI
recommended practice. Conventions on whether contingency sits inside or outside the performance
measurement baseline differ legitimately between organisations and between published bodies of
practice; this guide names the difference rather than asserting one convention as universal. No
external standard is reproduced.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
