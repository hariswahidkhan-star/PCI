---
id: BPG-08
series: S09
series_name: Best Practice Guides
title: Earned value in practice
subtitle: Making the three measures mean something on a real control account
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 16
summary: >
  Earned value management fails at the measurement, not at the arithmetic. This guide sets out what
  planned value, earned value and actual cost each have to be measured against before any index is
  worth reading, how the control account holds them together, how to read cost variance, schedule
  variance and the two performance indices, why the schedule performance index becomes actively
  misleading late in a project, and the questions earned value cannot answer at all.
linkedin:
  format: article
  hook: >
    A schedule performance index of 1.00 tells you nothing about whether the project finished on time —
    at completion it reaches 1.00 whatever happened, because all the planned value has been earned.
  tags: [ProjectControls, EarnedValue, CostEngineering, ProjectManagement]
  asset: carousel-8
gated: false
related: [BPG-04, BPG-06, BPG-07, BPG-09, TPL-07]
bok_domains: [4, 6]
sources: []
placeholders: 0
---

# Earned value in practice

> Making the three measures mean something on a real control account.

**In one paragraph.** Earned value management fails at the measurement, not at the arithmetic. This guide
sets out what planned value, earned value and actual cost each have to be measured against before any
index is worth reading, how the control account holds them together, how to read cost variance, schedule
variance and the two performance indices, why the schedule performance index becomes actively misleading
late in a project, and the questions earned value cannot answer at all.

**Who this is for.** Cost engineers, control account managers, planners and project controls managers who
already produce an earned value report every month and want it to survive being challenged.

---

## 1. The measurement problem comes before the formula

The arithmetic of earned value management (EVM) is three subtractions and two divisions. Nobody has ever
failed at that. Projects fail at EVM because one of the three inputs was not measured against the thing it
claims to measure, and the indices then describe a project that does not exist.

Of the three quantities, two are supposed to be found and one is created:

- **Planned value (PV)** is read from the time-phased baseline. If the baseline was never properly
  time-phased — if a lump of budget was dropped into the month it was approved rather than the months the
  work is planned — then PV is fiction and the schedule variance derived from it is fiction too. Baseline
  integrity is the subject of `BPG-04 — Baselining and baseline change control`.
- **Actual cost (AC)** is read from the ledger. If accruals are not booked at cut-off, AC is not the cost of
  the work performed; it is the cost of the invoices that happened to arrive. `BPG-07 — Accruals and
  cut-off discipline` owns that problem.
- **Earned value (EV)** is created by a person, every month, by applying an earning rule to physical
  progress. It is the only one of the three that a project manufactures rather than retrieves, which is
  exactly why it is the one that gets bent. Which rule applies to which work package is the subject of
  `BPG-06 — Progress measurement and rules of credit`.

The consequence is a hierarchy of trust that most reports never state. A cost performance index is only as
honest as its earning rules, and only as complete as its accruals. Before you defend an index, you should
be able to say which rule earned the value and whether the ledger was closed against the same date.

## 2. The control account is the unit of control

A **control account** is the point in the work breakdown structure where scope, schedule and budget
intersect and one named person — the **control account manager (CAM)** — is accountable for the result. It
is the level at which earned value is *managed*. It is not the level at which earned value is *measured*:
value is earned bottom-up in work packages, each under its own fixed earning rule, and rolled up.

Three disciplines make a control account usable:

1. **One owner.** If two people can explain a variance differently and both be right, the account is drawn
   wrong.
2. **One cause per variance, as far as possible.** An account that mixes civil works, a specialist
   subcontract and a currency-exposed equipment purchase will produce a cost variance that nobody can act
   on. The test is not size; it is whether the CAM can name the cause within a day.
3. **A fixed boundary shared by the schedule and the ledger.** The same scope must define the activities
   that generate PV, the packages that earn EV, and the cost codes that collect AC. Where those three
   boundaries differ even slightly, the variance is partly an artefact of coding.

The commonest structural failure in an otherwise competent EVM system is a control account that is a
reporting convenience rather than a management unit — sized to fit a page rather than to isolate a cause.

## 3. The three measures, stated precisely

| Measure | Definition | Read from |
|---|---|---|
| Planned value (PV) | The budget for the work **scheduled** to be complete by the data date | The time-phased cost baseline |
| Earned value (EV) | The budget for the work **actually performed** by the data date | Physical progress × earning rule |
| Actual cost (AC) | The cost **actually incurred** for that work, including accruals | The ledger, at cut-off |

All three are expressed in the same currency, over the same period, for the same scope boundary, at the
same data date. Every word in that sentence is load-bearing. A report that compares progress measured on
the 28th with costs closed on the 25th is comparing two different projects, and the difference shows up as
a variance that no action will fix.

Note what EV is not. It is not the value of the work to the client, not the amount billed, and not the cost
incurred. It is *budget, re-earned by performance* — the only common currency in which planned work and
performed work can be compared.

## 4. Variances and indices

Four derived numbers do almost all the work:

```
CV  = EV − AC        Cost variance — currency ahead of (+) or behind (−) budget
SV  = EV − PV        Schedule variance — currency of work ahead of (+) or behind (−) plan
CPI = EV ÷ AC        Cost performance index — value earned per unit of cost
SPI = EV ÷ PV        Schedule performance index — value earned per unit of value planned
```

Variances answer *how much*; indices answer *how efficiently*, and because they are dimensionless they can
be compared across accounts of wildly different size. A single work package with EV of 630,000 and AC of
665,000 gives `CPI = 630,000 ÷ 665,000 = 0.947` — a 5 % cost inefficiency that is directly comparable with
an account ten times its size.

Two habits separate a useful index from a decorative one.

**Read cumulative and period indices together.** The cumulative CPI is stable and damps the signal; the
current-period CPI is noisy but leads. On a project of any size a cumulative CPI that moves three points in
a month is a large movement, because it means the period performance was far worse than everything that
preceded it. Reporting only the cumulative figure is how a deteriorating trend stays invisible for two
quarters.

**Read the account beneath the rollup.** A project-level CPI is an average weighted by spend. It will
comfortably conceal one account in serious trouble behind three that are fine — which is precisely the
condition under which management attention is worth most.

## 5. Why the schedule performance index is a poor schedule indicator

SPI is a cost-based measure wearing a schedule's name, and it fails as a schedule indicator in three
distinct ways.

**It is denominated in currency, not time.** An SPI of 0.80 does not mean the project is 20 % late. It
means 20 % of the value planned to have been earned by now has not been. Whether that represents two weeks
or five months depends entirely on the shape of the baseline curve at that point.

**It is blind to the critical path.** Value earned is value earned, whether the work sat on the driving
path or on an activity with four months of float. A project can hold an SPI near 1.00 while the one
sequence that determines the completion date slips steadily, because the team is earning value on
everything that is easy to reach. EVM must be read alongside a critical-path analysis, never instead of
one — see `BPG-05 — Schedule quality: a practical review`.

**It converges to 1.00 at completion, by construction.** When all the work is done, EV equals the budget at
completion (BAC), and PV also equals BAC. The ratio is therefore exactly 1.00 no matter how late the
project finished. Worse, it approaches 1.00 smoothly through the late stages, so it *improves* while the
project is running out of time.

*Illustrative figures.* A control account with a BAC of USD 4,800,000, baselined to finish at Month 12,
actually finishes at Month 15:

| Point | EV | PV | `SPI = EV ÷ PV` |
|---|---:|---:|---:|
| Month 12 (planned finish) | 4,320,000 | 4,800,000 | `4,320,000 ÷ 4,800,000 = 0.900` |
| Month 14 | 4,700,000 | 4,800,000 | `4,700,000 ÷ 4,800,000 = 0.979` |
| Month 15 (actual finish) | 4,800,000 | 4,800,000 | `4,800,000 ÷ 4,800,000 = 1.000` |

The index rises from 0.900 to 1.000 across the three months in which the project is delivering its
lateness. A reader who watches SPI alone sees recovery.

The practical response is to stop asking SPI to be a schedule measure. Report schedule status in days
against milestones and in critical-path terms, and if an index is wanted, use an **earned schedule** measure
— the point in baseline *time* at which the current EV should have been earned, compared with actual time —
which does not collapse to 1.00 at completion. The method is developed in BoK Domain 6 (EVM/EAC).

## 6. What earned value cannot tell you

A professional states the limits of the instrument as clearly as the readings.

- **Whether the baseline deserved to be believed.** EVM measures conformance to a plan. A generous plan
  produces flattering indices; that is not performance, it is estimating.
- **Whether the remaining work resembles the work done.** Every index is a statement about the past.
  Turning it into a forecast requires an explicit assumption, which is the subject of
  `BPG-09 — Estimate at completion: choosing and defending a method`.
- **Where on the network the trouble is.** See §5.
- **Anything about cash.** EV is budget re-earned; it is not billed, certified or collected. A control
  account with a CPI of 1.05 can sit inside a project that cannot pay its subcontractors — see
  `BPG-13 — Cash flow forecasting`.
- **Anything about quality or rework not yet found.** Value earned under a units-completed rule counts
  metres installed, not metres that will survive inspection. Rework arrives later as AC with no
  corresponding EV, and reads as a cost variance months after the cause.
- **Scope that has changed but is not yet in the baseline.** Instructed variations not yet incorporated
  distort every measure at once: work is being performed and paid for that the baseline does not recognise.
  `BPG-11 — Change orders and variations` deals with the treatment.

There is one further distortion worth naming because it is silent. **Level of effort (LOE)** work packages —
supervision, project controls, site management — earn value by the calendar, so EV is set equal to PV and
their schedule variance is zero by construction. Mixed into a control account, LOE drags both indices
towards 1.00 and makes the account look healthier than the discrete work inside it. The §9 example
quantifies the effect.

## 7. Trend, tolerance and the conversation the report should start

Indices are inputs to a decision, not the decision. Two conventions make them operational.

Set **tolerances in advance** — the thresholds at which an account moves from normal reporting into
exception reporting — and set them at the control account level rather than the project level, because that
is where action is taken. Tolerances agreed after a bad month are not tolerances.

Then report **trend, cause and action** rather than status. "CPI 0.899" is a fact. "CPI 0.899, down from
0.94 two months ago, driven by cable-pulling productivity at 1.3 times budget hours per metre since access
was released piecemeal; four supplementary crews mobilise next week" is a decision. `BPG-14 — Monthly
reporting that gets read` owns the form of that narrative.

## 8. How this goes wrong

**Earned value is derived from cost.** The single most destructive error: setting EV as a percentage of
budget equal to the percentage of budget spent. EV then tracks AC, CPI is 1.00 by construction, and the
system reports perfect cost performance forever. It usually enters through a spreadsheet formula nobody has
opened in two years.

**The 90 % plateau.** Percent-complete claims advance briskly to about 90 % and then stall for months while
the last of the work is finished. The pattern is a symptom of subjective earning rules on long work
packages; the fix is objective rules — units, weighted milestones or 0/100 on short packages — fixed before
work starts.

**Level of effort quietly dominating a control account.** Because LOE cannot show a variance, an account
that is one-third LOE by value can only ever report two-thirds of the variance actually occurring in it.
Segregate LOE into its own packages and cap its share.

**Cost booked where no value can be earned.** Mobilisation, standing time or early procurement charged to a
package that has not started produces AC with no EV, and the resulting variance is attributed to the wrong
cause. It is not always miscoding — sometimes the cost is real and the baseline simply never planned for it,
which is a different and more serious finding.

**Mismatched data dates.** Progress cut on one date and the ledger on another. The variance that results is
a timing artefact, and chasing it wastes a control account manager's month.

**Re-baselining to remove a variance.** Adjusting the baseline so that PV agrees with EV eliminates the
schedule variance and, with it, the only record that anything went wrong. Baseline changes are legitimate
and necessary; a baseline change made *because* a variance is uncomfortable is not one. The change log,
not the index, is the audit trail.

**Reporting the rollup only.** The project CPI is an average. If nobody reads the account detail beneath
it, the system is producing a number rather than a control.

## 9. Worked example

*Illustrative figures.* Currency USD; all values at the Month 7 data date; earning rules fixed at baseline;
the ledger is closed at the same date with accruals booked; indices to three decimal places.

**Control account CA-4200, cabling and terminations. BAC = USD 4,800,000.**

Earned value is measured package by package, each under its own rule:

| Work package | Budget | Earning rule | Status at data date | `EV` |
|---|---:|---|---|---:|
| WP-1 Containment | 900,000 | Units completed | 6,300 of 9,000 m | `(6,300 ÷ 9,000) × 900,000 = 630,000` |
| WP-2 Cable pulling | 1,600,000 | Units completed | 18,000 of 40,000 m | `(18,000 ÷ 40,000) × 1,600,000 = 720,000` |
| WP-3 Terminations | 1,200,000 | Weighted milestones | Milestones worth 25 % achieved | `0.25 × 1,200,000 = 300,000` |
| WP-4 Testing and commissioning | 700,000 | 0/100 | Not started | `0` |
| WP-5 Supervision | 400,000 | Level of effort | Earns with time | `= PV = 260,000` |
| **Total** | **4,800,000** | | | **1,910,000** |

Planned value and actual cost against the same packages:

| Work package | `PV` | `AC` (incl. accruals) |
|---|---:|---:|
| WP-1 Containment | 810,000 | 665,000 |
| WP-2 Cable pulling | 960,000 | 845,000 |
| WP-3 Terminations | 360,000 | 330,000 |
| WP-4 Testing and commissioning | 0 | 20,000 |
| WP-5 Supervision | 260,000 | 265,000 |
| **Total** | **2,390,000** | **2,125,000** |

**The four derived measures.**

```
CV  = EV − AC = 1,910,000 − 2,125,000 = (215,000)
SV  = EV − PV = 1,910,000 − 2,390,000 = (480,000)
CPI = EV ÷ AC = 1,910,000 ÷ 2,125,000 = 0.899
SPI = EV ÷ PV = 1,910,000 ÷ 2,390,000 = 0.799
```

Percent complete by value: `EV ÷ BAC = 1,910,000 ÷ 4,800,000 = 39.8 %`.

**What the numbers say, and what they hide.**

WP-4 carries USD 20,000 of actual cost against zero earned value. It is not a rounding matter: either the
cost is miscoded, or testing equipment was procured earlier than the baseline planned. Both are findings.
Until it is resolved, USD 20,000 of the cost variance has nothing to do with cabling productivity, and the
CAM should say so in the narrative rather than let it be absorbed into a single number.

Now remove the level-of-effort package and recompute the discrete work only:

```
EV (discrete) = 1,910,000 − 260,000 = 1,650,000
PV (discrete) = 2,390,000 − 260,000 = 2,130,000
AC (discrete) = 2,125,000 − 265,000 = 1,860,000

CPI (discrete) = 1,650,000 ÷ 1,860,000 = 0.887
SPI (discrete) = 1,650,000 ÷ 2,130,000 = 0.775
```

Supervision is 8.3 % of the account by budget (`400,000 ÷ 4,800,000`), and it moves the reported CPI from
0.887 to 0.899 and the reported SPI from 0.775 to 0.799 — a 2.4-point improvement in the schedule index
bought entirely by including work that is incapable of reporting a schedule variance. The account is
performing worse than its headline says, and the difference is structural rather than behavioural.

**The schedule reading.** The SV of (480,000) is 480,000 of *budget* not yet earned. It is not a duration.
Converting it into a completion date requires the schedule: which of WP-2's remaining 22,000 m sit on the
driving path to the testing start, and what does the network say about the finish. The index frames the
question; it does not answer it.

**Assumptions this example depends on.** Earning rules were fixed before work started and have not changed;
the ledger and the progress measurement share the Month 7 data date; accruals for delivered but uninvoiced
subcontract work are booked; no variation has been added to the baseline during the period. Change any one
of these and every figure above moves.

## 10. Checklist

Before you publish an earned value report, confirm each of the following. Anything you cannot answer in one
sentence is the thing to fix before the meeting.

**Measurement**

- [ ] Every work package has an earning rule recorded in the baseline, and the rule applied this month is
      the rule recorded.
- [ ] Percent-complete claims above 85 % are supported by an objective basis, not an assessment.
- [ ] Level of effort is segregated in its own packages, and its share of each control account is known.
- [ ] Progress and the cost ledger share one data date.
- [ ] Accruals are booked for work performed but not invoiced.

**Structure**

- [ ] Each control account has one named manager who can state the cause of its variance.
- [ ] The scope boundary is identical across the schedule activities, the work packages and the cost codes.
- [ ] No control account contains actual cost against a package that cannot yet earn value — or, where it
      does, the reason is stated.

**Reading**

- [ ] Cumulative and current-period CPI are both reported.
- [ ] The rollup is accompanied by the two or three accounts driving it.
- [ ] SPI is reported with a schedule status in days, never on its own.
- [ ] Any account past 85 % complete has its SPI read with the convergence effect stated.
- [ ] Tolerances were set before the period, not after.

**Integrity**

- [ ] Every baseline change in the period is in the change log with an approval reference.
- [ ] No variance was removed by a baseline change during the period.
- [ ] Instructed but unincorporated variations are disclosed separately, so the reader knows the baseline is
      incomplete.

---

## Related

- `BPG-06 — Progress measurement and rules of credit` — the earning rules that determine whether EV means
  anything; read it before this one if your rules are not fixed.
- `BPG-07 — Accruals and cut-off discipline` — the other half of an honest CPI, on the actual cost side.
- `BPG-04 — Baselining and baseline change control` — where planned value comes from, and what a legitimate
  baseline change looks like.
- `BPG-09 — Estimate at completion: choosing and defending a method` — turning these indices into a forecast
  you can defend.
- `TPL-07 — Earned value calculation sheet` — the working instrument for the calculations in §9.

## Sources and standards

Earned value practice is described in several published frameworks — among them the AACE International
Total Cost Management framework and the PMBOK Guide, alongside national and sector earned value management
system standards. Their principles are explained here in our own words; no text, table or figure from any of
them is reproduced. The internal reference for this guide is BoK Domain 6 (EVM/EAC) and BoK Domain 4
(Performance Management, Variance Analysis & Management Reporting). All figures in §5 and §9 are
illustrative and were computed for this document.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
