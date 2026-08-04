---
id: TPL-08
series: S10
series_name: Free Templates
title: Estimate at completion scenario comparison
subtitle: One input block, four methods side by side, and a recommendation field that forces you to say why
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 16
summary: >
  Four standard ways of computing an estimate at completion will give four different answers from the
  same inputs, because each encodes a different assumption about whether the variance suffered so far
  will happen again. This template puts them side by side from one input block, computes variance at
  completion and the to-complete performance index for each, names the assumption each one encodes, and
  requires the user to record which was selected and why.
linkedin:
  format: document
  hook: >
    The four standard ways of computing an estimate at completion do not disagree about arithmetic. They
    disagree about whether the variance you have already suffered will happen again — and that is a
    judgement someone should have to write down.
  tags: [EstimateAtCompletion, EarnedValue, Forecasting, ProjectControls, CostEngineering]
  asset: one-pager
gated: false
related: [TPL-04, TPL-06, TPL-07, BPG-09, BPG-10]
bok_domains: [3, 6]
sources:
  - "PCI Master Formula Sheet (docs/downloads/master-formula-sheet.md), August 2026 — published under the credential's retired code; the credential is PCL-AI"
placeholders: 0
---

# Estimate at completion scenario comparison

> Four forecasts from one set of inputs, with the assumption behind each one written down.

**In one paragraph.** Four standard ways of computing an estimate at completion will give four different
answers from the same inputs, because each encodes a different assumption about whether the variance
suffered so far will happen again. This template puts them side by side from one input block, computes
variance at completion and the to-complete performance index for each, names the assumption each one
encodes, and requires the user to record which was selected and why.

**Who this is for.** Cost engineers and project controls managers who prepare the forecast; project
directors and sponsors who approve it; and anyone who has been asked why the estimate at completion moved
and needs an answer better than "the index changed".

---

## 1. When to use this

Produce the comparison every period in which a forecast is reported, immediately after the earned value
sheet (`TPL-07`) closes. Producing a single estimate at completion by one method and reporting it as *the*
forecast conceals the judgement that was made; producing four and selecting one makes the judgement
visible and reviewable.

Three moments make it more than a routine:

- **The first period in which the cost performance index departs materially from one.** That is the period
  in which the project decides, usually without noticing, whether it believes the variance is structural
  or a one-off. The sheet forces the decision to be recorded.
- **Before a request for additional funding or a contingency release.** The forecast is the basis of the
  request, and the spread between methods is the honest expression of its uncertainty.
- **When the selected method changes.** A change of method moves the forecast without anything happening
  on site. It must be disclosed as a method change, with the previous method's answer shown alongside for
  one period.

Do not use index-based methods early. Below roughly fifteen to twenty per cent complete the indices are
ratios of small numbers, and extrapolating them across the remaining eighty per cent produces a forecast
with a wide error and a confident appearance. Early in a project, the bottom-up method is the only one
worth reporting.

## 2. How to complete it

**Enter the input block once.** Budget at completion, planned value, earned value and actual cost, all to
the same data date and all from the earned value sheet. Everything else on this sheet is computed from
them. The definitions and interpretation of the indices belong to `TPL-07` and are not restated here; they
are computed in the input block so that this sheet stands alone.

**State which budget at completion you are using.** The performance measurement baseline — distributed
control-account budget — is the correct base for an index-based forecast, because it is the only budget
against which earned value has been measured. If contingency sits inside your budget at completion, take
it out for this calculation and show the remaining contingency separately below the forecast. A forecast
that quietly absorbs contingency has removed a decision from the person entitled to make it.

**Compute the bottom-up estimate to complete separately, and properly.** It is the only method that is not
an extrapolation, and it is the only one that can see a change in the work ahead. Ask each control account
manager for a re-estimate of the remaining work at current rates, with the same estimate basis discipline
you would apply to a change request (`TPL-04`): quantity, rate, source, and the assumptions the number
depends on. A bottom-up figure assembled by scaling the remaining budget is not a bottom-up figure.

**Read the to-complete performance index as a credibility test, not as a target.** It states the cost
performance the remaining work must achieve for a given endpoint to be met. Compare it with the cost
performance index actually achieved. When the required performance is materially better than anything the
project has yet demonstrated, the endpoint is an aspiration and the sheet says so in one number.

**Use the three arithmetic self-checks in §3.4.** Each index-based method has a to-complete performance
index that must come out to a specific value. If yours does not, the sheet has an error in it.

**Compute at full precision, round for display.** Using a cost performance index rounded to three decimal
places rather than the underlying ratio moves the first method's answer in the worked example by CU 2
thousand. On a larger project the same error is material.

**Fill the recommendation block before the forecast leaves the room.** The methods do not choose between
themselves. The block requires the selected method, the reason it was selected over the others, what would
change the choice, and who challenged it.

**Using the tables.** Copy a table block, paste into a single spreadsheet column, split on the pipe
character, and delete the alignment row.

## 3. The template

### 3.1 Input block

Hold the inputs in a single column so every method references the same cells.

| Cell | Field | Type | Entry rule |
|---|---|---|---|
| B1 | Project / control account | Text | The level at which this forecast is made |
| B2 | Data date | Date | Must match the earned value sheet |
| B3 | Budget at completion (BAC) | Input | Distributed performance measurement baseline |
| B4 | Planned value (PV) | Input | Cumulative to the data date |
| B5 | Earned value (EV) | Input | Cumulative to the data date |
| B6 | Actual cost (AC) | Input | Cumulative to the data date, including accruals |
| B7 | Cost performance index (CPI) | Calculated | `=IF(N($B$6)=0,"",$B$5/$B$6)` |
| B8 | Schedule performance index (SPI) | Calculated | `=IF(N($B$4)=0,"",$B$5/$B$4)` |
| B9 | Bottom-up estimate to complete | Input | Re-estimate of the remaining work, assembled from the control accounts |
| B10 | To-complete performance index to BAC | Calculated | See §3.3 |
| B11 | Remaining contingency | Input | From the change register (`TPL-04`) |
| B12 | Currency, scale and basis | Text | e.g. CU thousands, nominal, no escalation |

### 3.2 The method comparison table

Four rows, one per method. Data rows 15 to 18.

| Col | Field | Type | Definition |
|---|---|---|---|
| A | Method | Text | |
| B | Assumption this method encodes | Text | Pre-filled below; do not paraphrase it away |
| C | Estimate at completion | Calculated | |
| D | Variance at completion | Calculated | |
| E | Variance at completion % | Calculated | |
| F | To-complete performance index to this estimate | Calculated | |
| G | Evidence supporting this assumption | Text | What is true about this project that makes it right |
| H | Evidence against | Text | What is true about this project that makes it wrong |

**Method 1 — Budget at completion divided by the cost performance index.**
In words: the remaining work will be delivered at the same cost efficiency as the work already done.
Formula: `EAC = BAC ÷ CPI`. Spreadsheet, written so it never divides by a rounded or blank index:

```
=IF(OR(N($B$5)=0,N($B$6)=0),"",$B$3*$B$6/$B$5)
```

Since the cost performance index is earned value divided by actual cost, dividing budget at completion by
it is the same as multiplying budget at completion by actual cost and dividing by earned value. The second
form evaluates at full precision and guards both denominators.

**Method 2 — Actual cost plus the remaining budget.**
In words: the variance to date was a one-off, and the remaining work will be delivered at the budgeted
rate. Formula: `EAC = AC + (BAC − EV)`. Spreadsheet:

```
=IF(OR($B$3="",$B$5="",$B$6=""),"",$B$6+($B$3-$B$5))
```

**Method 3 — Actual cost plus the remaining budget divided by the cost and schedule indices compounded.**
In words: the remaining work will be affected both by the cost efficiency achieved so far and by the
schedule pressure implied by working behind plan. Formula: `EAC = AC + (BAC − EV) ÷ (CPI × SPI)`.
Spreadsheet:

```
=IF(OR(N($B$4)=0,N($B$5)=0,N($B$6)=0),"",$B$6+($B$3-$B$5)/(($B$5/$B$6)*($B$5/$B$4)))
```

All three denominators are guarded: the two indices need non-zero actual cost and planned value, and the
compound product needs non-zero earned value.

**Method 4 — Actual cost plus a bottom-up estimate to complete.**
In words: historical performance does not describe the work remaining, so the remaining work has been
re-estimated. Formula: `EAC = AC + ETC`. Spreadsheet:

```
=IF($B$9="","",$B$6+$B$9)
```

**Variance at completion (column D).** In words: budget at completion less the estimate at completion.
Negative means a forecast overrun.

```
=IF(NOT(ISNUMBER(C15)),"",$B$3-C15)
```

**Variance at completion per cent (column E).** In words: variance at completion as a proportion of budget
at completion.

```
=IF(OR(NOT(ISNUMBER(D15)),N($B$3)=0),"",D15/$B$3)
```

**To-complete performance index to this estimate (column F).** In words: the cost performance the
remaining work must achieve for this estimate to be met — remaining budgeted work divided by remaining
forecast money.

```
=IF(OR(NOT(ISNUMBER(C15)),C15=$B$6),"",($B$3-$B$5)/(C15-$B$6))
```

The `ISNUMBER` test is what makes the guard safe: a blank estimate cell is text, and subtracting actual
cost from it would produce an error that propagates across the row.

### 3.3 To-complete performance index to budget at completion

In words: the cost performance the remaining work must achieve to finish within the original budget.
Formula: `TCPI to BAC = (BAC − EV) ÷ (BAC − AC)`. Spreadsheet, in cell `B10`:

```
=IF(OR($B$3="",$B$3=$B$6),"",($B$3-$B$5)/($B$3-$B$6))
```

Where actual cost has already reached or passed budget at completion the denominator is zero or negative
and the measure stops being meaningful — the guard returns blank, and the honest report is that the budget
cannot be met, not a negative index.

Compare `B10` with `B7`. The ratio of the two is how much better than achieved performance the remaining
work must run:

```
=IF(OR(N($B$7)=0,$B$10=""),"",$B$10/$B$7-1)
```

### 3.4 The three arithmetic self-checks

Each index-based method implies a specific to-complete performance index. These are identities, not
coincidences, and they are the fastest way to find an error in the sheet.

| Method | Its to-complete performance index must equal | Why |
|---|---|---|
| 1 — `BAC ÷ CPI` | the cost performance index | Substituting `EAC = BAC × AC ÷ EV` gives `EAC − AC = AC × (BAC − EV) ÷ EV`, so the index reduces to `EV ÷ AC` |
| 2 — `AC + (BAC − EV)` | exactly 1.000 | `EAC − AC = BAC − EV`, so numerator and denominator are identical |
| 3 — `AC + (BAC − EV) ÷ (CPI × SPI)` | the cost index multiplied by the schedule index | `EAC − AC = (BAC − EV) ÷ (CPI × SPI)`, so the ratio is `CPI × SPI` |

Method 4 has no identity, because its estimate to complete is entered rather than derived. That is the
point of it.

### 3.5 The recommendation block

| Field | Entry |
|---|---|
| Method selected | |
| **Why this method rather than the other three** | Required. Name the cause of the variance to date and say whether it applies to the work remaining. A reason that would be true on any project is not a reason. |
| Evidence relied on | Estimate basis, quotations, productivity records, subcontract position |
| Previous period's method and estimate | If the method has changed, both are reported for one period |
| Range reported | Lowest and highest of the four, as the honest expression of uncertainty |
| What would change this choice | The specific observation that would move the forecast next period |
| Remaining contingency | From the change register |
| Variance at completion against remaining contingency | Whether the forecast can be absorbed |
| Prepared by / date | |
| Challenged by / date | A second person, not the preparer |
| Approved by / date | |
| Next review | |

## 4. Worked fragment

*Illustrative figures.* The same fictional facility upgrade project as `TPL-07`, at the same cut-off.

- **Currency and scale:** generic currency units (CU), thousands.
- **Basis:** nominal, single currency, no escalation.
- **Data date:** 31 May 2026.
- **Budget at completion used:** CU 8,000 thousand, the distributed performance measurement baseline. The
  contingency reserve of CU 300 thousand sits outside it and is reported separately below.
- **Rounding:** amounts to the nearest CU thousand; indices to three decimal places; percentages to one
  decimal place, half away from zero. Adverse figures in parentheses. All arithmetic performed at full
  precision.

**Input block.**

| Field | Value |
|---|---|
| Budget at completion | 8,000 |
| Planned value | 4,000 |
| Earned value | 3,800 |
| Actual cost | 3,980 |
| Cost performance index | 0.955 |
| Schedule performance index | 0.950 |
| Bottom-up estimate to complete | 4,450 |
| To-complete performance index to budget at completion | 1.045 |
| Remaining contingency | 300 |

Indices verified: CPI = 3,800 ÷ 3,980 = 0.955 (0.954774 unrounded); SPI = 3,800 ÷ 4,000 = 0.950;
TCPI to BAC = (8,000 − 3,800) ÷ (8,000 − 3,980) = 4,200 ÷ 4,020 = 1.045.

**Method comparison.**

| Method | Assumption encoded | EAC | VAC | VAC % | TCPI to this EAC |
|---|---|---|---|---|---|
| 1 — BAC ÷ CPI | Cost efficiency achieved to date continues across all remaining work | 8,379 | (379) | (4.7 %) | 0.955 |
| 2 — AC + (BAC − EV) | The variance to date was a one-off; remaining work runs at budget | 8,180 | (180) | (2.3 %) | 1.000 |
| 3 — AC + (BAC − EV) ÷ (CPI × SPI) | Remaining work is affected by both cost efficiency and schedule pressure | 8,610 | (610) | (7.6 %) | 0.907 |
| 4 — AC + bottom-up ETC | History does not describe the work ahead; it has been re-estimated | 8,430 | (430) | (5.4 %) | 0.944 |

**Verification of each method.**

Method 1: 8,000 ÷ 0.954774 = 8,378.95, so CU 8,379 thousand. Equivalently 8,000 × 3,980 ÷ 3,800 =
8,378.95. Variance at completion = 8,000 − 8,378.95 = (378.95), shown as (379);
(378.95) ÷ 8,000 = (4.7 %). Using the rounded index of 0.955 instead would give 8,000 ÷ 0.955 = 8,376.96,
CU 2 thousand adrift — which is why the formula in §3.2 avoids the index cell.

Method 2: 3,980 + (8,000 − 3,800) = 3,980 + 4,200 = 8,180. Variance at completion = 8,000 − 8,180 = (180);
(180) ÷ 8,000 = (2.25 %), displayed as (2.3 %). Note that this variance at completion equals the cost
variance in `TPL-07`, which it must: the method assumes no further variance is incurred.

Method 3: CPI × SPI = 0.954774 × 0.950 = 0.907035. Remaining budgeted work ÷ that product =
4,200 ÷ 0.907035 = 4,630.47. Estimate at completion = 3,980 + 4,630.47 = 8,610.47, so CU 8,610 thousand.
Variance at completion = 8,000 − 8,610.47 = (610.47), shown as (610); (610.47) ÷ 8,000 = (7.6 %).

Method 4: 3,980 + 4,450 = 8,430. Variance at completion = 8,000 − 8,430 = (430);
(430) ÷ 8,000 = (5.4 %). The bottom-up estimate to complete of 4,450 sits CU 250 thousand — 6.0 per cent —
above the remaining budgeted work of 4,200.

**Verification of the self-checks.** Method 1: 4,200 ÷ (8,378.95 − 3,980) = 4,200 ÷ 4,398.95 = 0.955,
equal to the cost performance index. Method 2: 4,200 ÷ (8,180 − 3,980) = 4,200 ÷ 4,200 = 1.000.
Method 3: 4,200 ÷ (8,610.47 − 3,980) = 4,200 ÷ 4,630.47 = 0.907, equal to CPI × SPI. All three identities
hold, so the sheet is arithmetically sound.

**The credibility test.** Finishing within the original budget requires a to-complete performance index of
1.045 against a cost performance index of 0.955 achieved so far — a ratio of 1.094, meaning the remaining
work would have to be delivered about **nine per cent more efficiently than anything the project has yet
demonstrated**, having so far run about five per cent worse than budget. Nothing in the record supports
that, so budget at completion is not a forecast; it is a target.

**The range.** The four methods span CU 8,180 to CU 8,610 thousand, a spread of CU 430 thousand, or
5.4 per cent of budget at completion. That spread is the honest statement of forecast uncertainty at this
data date, and it belongs in the monthly report next to whichever single figure is selected.

**Recommendation, as it would be written.**

> **Method selected:** Method 4, bottom-up, at CU 8,430 thousand.
>
> **Why this method rather than the other three.** The cost variance to date is concentrated in CA-1000
> and has two identified causes: piling productivity below the estimate basis, and rework at the pile cap
> interface. The piling cause is structural and will persist across the remaining 132 piles, so Method 2 —
> which assumes the variance was a one-off — is rejected. The rework cause has been closed by a revised
> inspection sequence and will not recur, so Method 1, which projects the whole variance to date across
> all remaining work including the mechanical account that is currently ahead, overstates it. Method 3
> additionally loads the schedule performance index onto the forecast; the schedule variance here arises
> in CA-3000, whose work is largely still ahead and is not resource-constrained, so compounding is not
> supported. The control account managers have re-estimated the remaining work at CU 4,450 thousand
> against a remaining budget of CU 4,200 thousand, with the piling shortfall priced at current measured
> rates.
>
> **What would change this choice.** If piling output over June does not recover to the re-estimated rate,
> the bottom-up basis fails and Method 1 becomes the defensible forecast. If mechanical installation
> begins to consume float, Method 3 becomes relevant.
>
> **Funding position.** Forecast variance at completion of CU (430) thousand against remaining contingency
> of CU 300 thousand — a shortfall of CU 130 thousand — before BCR-014, which itself requests CU 295
> thousand from that contingency and is under review (`TPL-04` §4.1). If BCR-014 is approved, CU 5 thousand
> of contingency remains against a forecast overrun of CU 430 thousand. That is the decision the sponsor
> is being asked for, and it belongs on page one of the monthly report.

## 5. Common mistakes

**Reporting one method as though it were the answer.** The single figure hides the assumption. Report the
selected figure with the range and the reason.

**Using an index-based method too early.** Below roughly fifteen to twenty per cent complete the
extrapolation is built on a ratio of small numbers, and its confident appearance is the dangerous part.

**Using a rounded index in the formula.** Small on this example, material on a large project, and always
avoidable by referencing the input cells.

**Applying the compound method by default.** Multiplying by the schedule performance index assumes that
being behind schedule will cost money on the remaining work. Sometimes it will — acceleration,
out-of-sequence working, extended preliminaries. Sometimes the schedule variance sits in work that is not
resource-constrained and costs nothing extra. Compounding without that argument is not conservatism; it is
an unexamined assumption dressed as prudence.

**A "bottom-up" estimate that is the remaining budget with a factor applied.** That is Method 1 with extra
steps, and it inherits none of the diagnostic value.

**Treating the to-complete performance index as a target.** It is a statement of what is required, not an
instruction. Circulating it as a productivity goal is how a forecast becomes a negotiation.

**Forgetting that a contingency release changes budget at completion.** When a contingency release moves
budget into a control account, budget at completion for measurement rises and every index, variance and
forecast is computed on the new base. Recompute; do not carry the previous period's forecast forward.

**Changing method quietly.** The forecast moves and nothing happened. Disclose the change, show both
answers for one period, and say what caused the change of view.

**Comparing the forecast overrun with total contingency rather than remaining contingency**, or ignoring
pending change requests against it. The relevant number is what is left after everything already approved
and everything currently in the approval queue.

## 6. Adapting it

**Safe to change.** Running the sheet at control-account level as well as at project level, which is
usually more informative — a project-level forecast averages away the account where the problem is.
Adding methods: a weighted compound variant that blends the cost and schedule indices in some other
proportion is legitimate provided the weighting is stated and justified on the sheet rather than inherited
from a spreadsheet somebody was given. Adding an optimistic and pessimistic bottom-up case to widen the
range deliberately.

**Change with care.** Adding a probabilistic forecast from a cost risk analysis. It answers a different
question — the distribution of outcomes rather than a point estimate under an assumption — and placing it
in the same table invites readers to compare a percentile with a deterministic figure as though they were
the same kind of number. If you add it, label it separately.

**Do not remove.** The assumption column, the to-complete performance index column, the range, and the
recommendation block's "why this method rather than the others" field. Without those four the sheet
becomes a menu of numbers from which the most convenient can be chosen.

**Where the client mandates a method**, compute theirs and report it as the contractual forecast, and keep
this comparison as the internal position with the difference reconciled. Do not let a mandated method
become the project's own view of itself.

## 7. Completion checklist

- [ ] Inputs match the earned value sheet exactly, to the same data date
- [ ] Budget at completion used is the distributed performance measurement baseline, stated as such
- [ ] Contingency shown separately, not absorbed into the forecast
- [ ] All four methods computed from the same input cells
- [ ] Every division guarded, and every estimate cell tested with `ISNUMBER` before subtraction
- [ ] Formulas reference unrounded inputs, not displayed indices
- [ ] The three arithmetic self-checks in §3.4 all hold
- [ ] To-complete performance index to budget at completion computed and compared with the achieved index
- [ ] Bottom-up estimate to complete assembled from control accounts with a stated estimate basis
- [ ] Assumption column completed for every method, with evidence for and against
- [ ] Method selected, with a reason specific to this project's variance causes
- [ ] Range reported alongside the selected figure
- [ ] Variance at completion compared with remaining contingency after pending changes
- [ ] Forecast challenged by a second person and approved, both named and dated
- [ ] Any change of method disclosed, with the previous method's answer shown for one period

---

## Related

- `TPL-07 — Earned value calculation sheet` — the source of every input, and where the indices are defined
- `TPL-04 — Baseline change request` — the contingency position the forecast must be read against
- `TPL-06 — Monthly project controls report` — where the selected forecast and its range are reported
- `BPG-09 — Estimate at completion — choosing and defending a method` — the judgement in full
- `BPG-10 — Contingency and management reserve` — how a forecast overrun and contingency relate

## Sources and standards

- PCI Master Formula Sheet (`docs/downloads/master-formula-sheet.md`), August 2026: the estimate at
  completion, variance at completion and to-complete performance index formulas used throughout.
  Published under the credential's retired code; the credential is PCL-AI.

The four methods are common to established earned value practice and are stated here in the Institute's
own words. No third-party sheet, table or wording is reproduced. The identities in §3.4 are derived from
the formulas themselves and are verified in §4.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
