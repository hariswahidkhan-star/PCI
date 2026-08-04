---
id: TPL-07
series: S10
series_name: Free Templates
title: Earned value calculation sheet
subtitle: Four inputs per control account, eight derived measures, and the rounding rules that stop two people getting different answers
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 12
summary: >
  An earned value sheet takes four entered numbers per control account — budget at completion, planned
  value, earned value and actual cost — and derives everything else by arithmetic. This template gives
  the control-account table with every derived measure defined in words and as a spreadsheet expression,
  the guard against dividing by zero on each one, the units and rounding rules that make two people's
  answers agree, and the roll-up rule that stops indices being averaged.
linkedin:
  format: document
  hook: >
    On an earned value sheet only four numbers per control account are entered — budget at completion,
    planned value, earned value and actual cost. Everything else is arithmetic, which means every
    argument about the numbers is really an argument about how earned value was measured.
  tags: [EarnedValue, ProjectControls, CostEngineering, PerformanceMeasurement]
  asset: one-pager
gated: false
related: [TPL-05, TPL-06, TPL-08, BPG-06, BPG-08]
bok_domains: [6]
sources:
  - "PCI Master Formula Sheet (docs/downloads/master-formula-sheet.md), August 2026 — published under the credential's retired code; the credential is PCL-AI"
placeholders: 0
---

# Earned value calculation sheet

> Four inputs per control account; everything else computed, guarded and rounded to a stated rule.

**In one paragraph.** An earned value sheet takes four entered numbers per control account — budget at
completion, planned value, earned value and actual cost — and derives everything else by arithmetic. This
template gives the control-account table with every derived measure defined in words and as a spreadsheet
expression, the guard against dividing by zero on each one, the units and rounding rules that make two
people's answers agree, and the roll-up rule that stops indices being averaged.

**Who this is for.** Cost engineers who build the sheet; control account managers who are asked to
explain the indices it produces; and project controls managers who have to reconcile it with the schedule
and the ledger.

---

## 1. When to use this

Produce the sheet every reporting period, at the cut-off set in the controls execution plan (`TPL-01`
§3.7), immediately after the progress measurement sheet (`TPL-05`) closes and the actual cost cut-off has
been applied. It sits between those two inputs and the monthly report (`TPL-06`) and the forecast
(`TPL-08`), and it should be produced in that order, because every downstream number depends on it.

Two conditions must hold before the output means anything.

**The three cumulative measures must be to the same data date and the same scope.** Planned value from a
schedule statused on the 31st, earned value from a progress claim cut off on the 25th and actual cost
from a ledger closed on the 28th produce indices that describe nothing. Where the cut-offs genuinely
cannot align, the actual cost figure must include an accrual to the data date, and the sheet must say so.

**Actual cost must include commitments received but not yet invoiced.** A ledger figure alone reports the
invoicing cycle, not the work. The cost performance index of a project whose subcontractor invoices
quarterly will look excellent for two months and terrible in the third, and none of the three months will
be true.

Do not produce the sheet for a control account that is less than roughly fifteen per cent complete. The
indices are ratios of small numbers and move violently on small absolute changes; reporting a cost
performance index of 0.62 on an account that has earned three per cent of its budget invites a
conversation about a problem that may not exist.

## 2. How to complete it

**Enter four numbers per control account; compute the rest.** Budget at completion, planned value, earned
value and actual cost. If anything else on the sheet is typed rather than computed, someone will
eventually type a figure that does not follow from the inputs, and the sheet will lose the property that
makes it trustworthy.

**Know where each input comes from.** Budget at completion comes from the approved baseline as amended by
the change register (`TPL-04`). Planned value comes from the time-phased baseline in the schedule. Earned
value comes from the progress measurement sheet (`TPL-05`), as rolled-up per cent complete multiplied by
budget at completion. Actual cost comes from the ledger plus accruals, reconciled to the code register
(`TPL-03`). Four inputs, four different owners, and the reconciliation of each is a named responsibility
in the controls execution plan.

**Measure only distributed budget.** Contingency and any other undistributed budget carry no planned
value, no earned value and no actual cost. Include a row for them so the sheet reconciles to the total
authorised budget, but compute no measures on that row — indices on a row with zero earned value are
either zero or undefined and mean nothing either way.

**Roll up by summing the inputs, never by averaging the indices.** The project cost performance index is
total earned value divided by total actual cost. It is not the mean of the control-account indices, and
it is not a budget-weighted mean of them either — summing the inputs gives the budget weighting
automatically and exactly.

**Fix the units and the rounding, and put them on the sheet.** State the currency, the scale (units,
thousands, millions), the basis (nominal or constant prices, and whether escalation is included), and the
rounding rule for each type of figure. Two people working from the same inputs disagree far more often
about rounding than about arithmetic.

**Round for display only; compute at full precision.** A sheet that rounds an index to three decimal
places and then uses the rounded value in a forecast introduces an error that grows with the size of the
project. Every downstream formula should reference the input cells, not the displayed index.

**Show adverse figures in parentheses if that is your house convention**, but do it with number
formatting, not by typing brackets. A cell containing `(220)` as text is text, and every formula that
touches it fails or, worse, treats it as zero.

**Using the tables.** Copy a table block, paste into a single spreadsheet column, split on the pipe
character, and delete the alignment row.

## 3. The template

### 3.1 The control-account table

| Col | Field | Type | Definition and entry rule |
|---|---|---|---|
| A | Control account ID | Text | From `TPL-02` |
| B | Control account title | Text | |
| C | Control account manager | Text | The person accountable for the variance, not the person who typed the numbers |
| D | Budget at completion (BAC) | Input | Approved distributed budget for the account, including approved changes to the cut-off |
| E | Planned value (PV) | Input | Cumulative budgeted cost of work scheduled to the data date |
| F | Earned value (EV) | Input | Cumulative budgeted cost of work performed to the data date, from `TPL-05` |
| G | Actual cost (AC) | Input | Cumulative cost incurred to the data date, including accruals |
| H | Cost variance (CV) | Calculated | |
| I | Cost variance % | Calculated | |
| J | Schedule variance (SV) | Calculated | |
| K | Schedule variance % | Calculated | |
| L | Cost performance index (CPI) | Calculated | |
| M | Schedule performance index (SPI) | Calculated | |
| N | % complete | Calculated | |
| O | % of budget spent | Calculated | |
| P | Variance cause and owner | Text | Required on any row breaching the reporting threshold |

### 3.2 The derived measures

Data rows begin at row 2.

**Cost variance.** In words: earned value less actual cost. A negative figure means more was spent than
the work performed was budgeted to cost.

```
=IF(OR($F2="",$G2=""),"",$F2-$G2)
```

**Cost variance per cent.** In words: cost variance as a proportion of earned value — the base is what was
earned, not what was budgeted, because the question is how much the work performed overran.

```
=IF(N($F2)=0,"",$H2/$F2)
```

**Schedule variance.** In words: earned value less planned value, expressed in currency. It is a measure of
work not done, not of time; a project can carry a large negative schedule variance and still finish on
time if the missing work is off the critical path.

```
=IF(OR($F2="",$E2=""),"",$F2-$E2)
```

**Schedule variance per cent.** In words: schedule variance as a proportion of planned value — the base is
what was planned, because the question is how far behind the plan the work is.

```
=IF(N($E2)=0,"",$J2/$E2)
```

**Cost performance index.** In words: earned value divided by actual cost. Below one means the work
performed cost more than it was budgeted to cost.

```
=IF(N($G2)=0,"",$F2/$G2)
```

**Schedule performance index.** In words: earned value divided by planned value. Below one means less work
has been performed than was scheduled by this date.

```
=IF(N($E2)=0,"",$F2/$E2)
```

**Per cent complete.** In words: earned value as a proportion of budget at completion.

```
=IF(N($D2)=0,"",$F2/$D2)
```

**Per cent of budget spent.** In words: actual cost as a proportion of budget at completion. Reported
beside per cent complete because the gap between the two is the cost variance stated as a proportion, and
some readers see it faster that way.

```
=IF(N($D2)=0,"",$G2/$D2)
```

`N()` returns zero for a blank or text cell, so the guard holds whether the input is zero, empty or
mistyped. `IFERROR` would also suppress the error, but it suppresses every error, including the ones that
signal a broken reference; prefer an explicit guard on the denominator.

### 3.3 Roll-up and reconciliation

**Roll-up.** In words: sum the four inputs across the control accounts, then apply the same formulas to
the totals. Every index in the total row must reference the summed inputs, never the individual indices.

```
D_total  =SUM($D$2:$D$20)     E_total  =SUM($E$2:$E$20)
F_total  =SUM($F$2:$F$20)     G_total  =SUM($G$2:$G$20)
```

**Reconciliation to the authorised budget.** Below the measured total, add rows that carry budget but no
performance measurement:

| Row | Content |
|---|---|
| Measured total — distributed budget | Sum of the control accounts above |
| Undistributed budget | Budget allocated but not yet in a control account |
| Contingency reserve | Held against identified risk; released by change (`TPL-04`) |
| **Total authorised budget** | The figure that must agree with the change register |

State on the sheet whether the contingency reserve is inside or outside budget at completion on this
project, because both conventions are in use and the same set of facts produces two different totals
under them. The convention is fixed once, in `TPL-01` §3.2.

### 3.4 Units, rounding and presentation

State these on the face of the sheet:

| Item | Rule |
|---|---|
| Currency and scale | Named currency and scale, e.g. thousands. Currency-neutral sheets use a generic unit. |
| Basis | Nominal or constant prices; whether escalation and exchange effects are included |
| Data date | One date, applying to all three cumulative measures |
| Amounts | Rounded to the stated scale; never rounded before summing |
| Indices | Three decimal places throughout, or two throughout — state which and never mix |
| Percentages | One decimal place; rounding half away from zero |
| Adverse figures | Parentheses by number format, not by typed brackets |
| Precision | Display rounding only; all downstream formulas reference unrounded cells |

On the index convention: the Institute's master formula sheet rounds indices to two decimal places, while
worked examples in the publication framework are shown to three. Either is defensible. Three decimal
places is preferable on a large project, because at two decimal places a cost performance index of 0.955
and one of 0.954 both display as 0.95 while differing by tens of thousands of currency units in the
resulting forecast. Choose one convention per project, write it on the sheet, and apply it everywhere.

### 3.5 Reporting thresholds

Set a threshold in the controls execution plan and apply it here, so that the variance narrative in
`TPL-06` is written for a defined set of rows rather than for whichever ones caught someone's eye.

```
=IF($L2="","",IF(OR(ABS(N($I2))>threshold_pct,ABS(N($H2))>threshold_value),"EXPLAIN",""))
```

A threshold with both a percentage and an absolute limb is the workable form: percentage alone flags
trivial amounts on small accounts, and value alone misses a large proportional overrun on a small one.

## 4. Worked fragment

*Illustrative figures.* A fictional facility upgrade project at the May 2026 cut-off.

- **Currency and scale:** generic currency units (CU), thousands.
- **Basis:** nominal, single currency, no escalation or exchange effects included.
- **Data date:** 31 May 2026, applying to planned value, earned value and actual cost alike.
- **Rounding:** amounts to the nearest CU thousand; indices to three decimal places; percentages to one
  decimal place, half away from zero. Adverse figures in parentheses.
- **Contingency convention on this project:** the contingency reserve sits inside budget at completion and
  outside the control accounts, as recorded in the controls execution plan.

Earned value for CA-1000 is the figure produced in `TPL-05` §4.1: a rolled-up 55.0 per cent complete
against a budget at completion of CU 4,000 thousand.

| CA | Title | BAC | PV | EV | AC | CV | CV % | SV | SV % | CPI | SPI | % compl. | % spent |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| CA-1000 | Civil works | 4,000 | 2,400 | 2,200 | 2,420 | (220) | (10.0 %) | (200) | (8.3 %) | 0.909 | 0.917 | 55.0 % | 60.5 % |
| CA-2000 | Mechanical works | 3,000 | 1,200 | 1,260 | 1,200 | 60 | 4.8 % | 60 | 5.0 % | 1.050 | 1.050 | 42.0 % | 40.0 % |
| CA-3000 | Controls and commissioning | 1,000 | 400 | 340 | 360 | (20) | (5.9 %) | (60) | (15.0 %) | 0.944 | 0.850 | 34.0 % | 36.0 % |
| | **Measured total** | **8,000** | **4,000** | **3,800** | **3,980** | **(180)** | **(4.7 %)** | **(200)** | **(5.0 %)** | **0.955** | **0.950** | **47.5 %** | **49.8 %** |
| | Contingency reserve | 300 | — | — | — | — | — | — | — | — | — | — | — |
| | **Total authorised budget** | **8,300** | | | | | | | | | | | |

**Verification, control account by control account.**

CA-1000: CV = 2,200 − 2,420 = (220); CV % = (220) ÷ 2,200 = (10.0 %); SV = 2,200 − 2,400 = (200);
SV % = (200) ÷ 2,400 = (8.3 %); CPI = 2,200 ÷ 2,420 = 0.909; SPI = 2,200 ÷ 2,400 = 0.917;
% complete = 2,200 ÷ 4,000 = 55.0 %; % spent = 2,420 ÷ 4,000 = 60.5 %.

CA-2000: CV = 1,260 − 1,200 = 60; CV % = 60 ÷ 1,260 = 4.8 %; SV = 1,260 − 1,200 = 60;
SV % = 60 ÷ 1,200 = 5.0 %; CPI = 1,260 ÷ 1,200 = 1.050; SPI = 1,260 ÷ 1,200 = 1.050;
% complete = 1,260 ÷ 3,000 = 42.0 %; % spent = 1,200 ÷ 3,000 = 40.0 %.

CA-3000: CV = 340 − 360 = (20); CV % = (20) ÷ 340 = (5.9 %); SV = 340 − 400 = (60);
SV % = (60) ÷ 400 = (15.0 %); CPI = 340 ÷ 360 = 0.944; SPI = 340 ÷ 400 = 0.850;
% complete = 340 ÷ 1,000 = 34.0 %; % spent = 360 ÷ 1,000 = 36.0 %.

**Verification of the totals.** BAC 4,000 + 3,000 + 1,000 = 8,000. PV 2,400 + 1,200 + 400 = 4,000.
EV 2,200 + 1,260 + 340 = 3,800. AC 2,420 + 1,200 + 360 = 3,980. Then CV = 3,800 − 3,980 = (180);
CV % = (180) ÷ 3,800 = (4.7 %); SV = 3,800 − 4,000 = (200); SV % = (200) ÷ 4,000 = (5.0 %);
CPI = 3,800 ÷ 3,980 = 0.955; SPI = 3,800 ÷ 4,000 = 0.950; % complete = 3,800 ÷ 8,000 = 47.5 %;
% spent = 3,980 ÷ 8,000 = 49.8 % (49.75 % before rounding).

**Why the total is not the average.** The mean of the three cost performance indices is
(0.909 + 1.050 + 0.944) ÷ 3 = 0.968, against the correct 0.955. The averaging method flatters the project
because it gives the small, well-performing mechanical account the same weight as the large, poorly
performing civil account. The gap widens as the accounts become less equal in size.

**What the sheet is actually saying.** The project has earned 47.5 per cent of its budget and spent
49.8 per cent of it. CA-3000 has the worst schedule performance index at 0.850 but the smallest budget, so
it contributes least to the project total; CA-1000 carries both the largest budget and the worst cost
performance, and is therefore where the variance narrative in `TPL-06` and the forecast in `TPL-08` should
concentrate. The forecast at completion is not derived here — the choice of method is a judgement, and it
belongs to `TPL-08`.

## 5. Common mistakes

**Mixed data dates.** Earned value cut off on the 25th, actual cost on the 28th and planned value statused
on the 31st produce indices that describe no real state of the project. The most common form is actual
cost with no accrual, which reports the invoicing cycle rather than the work.

**Averaging indices to roll up.** See §4. Sum the inputs; the weighting then happens by itself.

**Computing indices on undistributed budget or contingency.** There is no earned value to divide by, so
the result is either zero or an error, and both look like a finding.

**Reading schedule variance as time.** A schedule variance of CU (200) thousand does not mean twenty days
late. It means the work performed was budgeted at CU 200 thousand less than the work scheduled. The
translation to time requires either the schedule itself or an earned-schedule calculation, and both are
separate exercises.

**Publishing indices on a barely started account.** At three per cent complete the index is a ratio of two
small numbers and swings on rounding. Suppress it or annotate it.

**Rounding before summing.** Rounding each control account to the nearest thousand and then adding gives a
total that differs from the total of the unrounded figures. Sum first, round for display.

**Typed brackets for negatives.** A cell containing the text `(220)` is not the number −220. Use number
formatting.

**Chasing a favourable variance no harder than an adverse one.** A cost performance index of 1.05 on
CA-2000 is as much a measurement question as one of 0.909. Common causes are earned value claimed ahead of
the evidence, actual cost not yet accrued, and scope quietly moved out of the account — and all three
reverse in a later period.

**A sheet with typed values in the derived columns.** The moment one derived figure is entered by hand,
the sheet stops being a calculation and becomes a claim, and no reader can tell which cells are which.

## 6. Adapting it

**Safe to change.** Adding period columns beside the cumulative ones, provided the two are visually
distinguished — period indices are volatile and are often the more useful diagnostic. Adding a prior-period
block so movement is visible on the face of the sheet. Adding columns for hours as well as cost, on
projects that control both. Reporting at work-package level beneath the control account.

**Change with care.** Adding earned-schedule measures. They answer the time question that schedule
variance does not, but they require the time-phased planned value curve and a separate interpolation, and
half-implementing them produces a number that looks like time and is not.

**Do not remove.** The reconciliation to total authorised budget, the statement of the contingency
convention, and the units and rounding block. Without them the sheet cannot be reconciled to anything and
two readers will legitimately compute different answers from it.

**Where the client requires a different index convention** — two decimal places, a different variance base,
percentages against budget rather than earned value — run theirs on the client report and keep this one as
the internal sheet, with the conversion stated. Do not change the internal convention mid-project; the
comparability of your own history is worth more than the convenience.

## 7. Completion checklist

- [ ] One data date, applying to planned value, earned value and actual cost alike, stated on the sheet
- [ ] Actual cost includes accruals for work received and not yet invoiced
- [ ] Earned value reconciles to the roll-up in the progress measurement sheet
- [ ] Budget at completion reconciles to the baseline plus approved changes in the change register
- [ ] Only the four inputs are typed; every other column is a formula
- [ ] Every division guarded against a zero or blank denominator
- [ ] Totals computed by summing inputs, not by averaging indices
- [ ] Undistributed budget and contingency shown, with no measures computed on them
- [ ] Sheet reconciles to total authorised budget
- [ ] Currency, scale, basis, data date and rounding rules stated on the face of the sheet
- [ ] Index convention — two or three decimal places — stated and applied consistently
- [ ] Accounts below the materiality threshold for meaningful indices annotated or suppressed
- [ ] Every row breaching the reporting threshold has a named cause and owner before issue

---

## Related

- `TPL-05 — Progress measurement and rules of credit sheet` — where the earned value input is produced
- `TPL-06 — Monthly project controls report` — where these measures are narrated and explained
- `TPL-08 — Estimate at completion scenario comparison` — the forecast this sheet deliberately does not make
- `BPG-06 — Progress measurement and rules of credit` — why the earned value input is the contested one
- `BPG-08 — Earned value in practice` — interpreting the indices on a real control account

## Sources and standards

- PCI Master Formula Sheet (`docs/downloads/master-formula-sheet.md`), August 2026: the definitions of cost
  variance, schedule variance, the cost and schedule performance indices, and the budget identities used in
  §3.3. Published under the credential's retired code; the credential is PCL-AI.

The measures here are common to established earned value practice and are stated in the Institute's own
words. No third-party sheet, table or wording is reproduced.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
