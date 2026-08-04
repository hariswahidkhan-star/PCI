---
id: TPL-05
series: S10
series_name: Free Templates
title: Progress measurement and rules of credit sheet
subtitle: Five techniques, one rule-of-credit library, and a roll-up that weights by budget rather than averaging
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 13
summary: >
  Per cent complete is an assertion, and the sheet that produces it is where the assertion is either
  evidenced or invented. This template gives the measurement sheet and the rule-of-credit library behind
  it: five techniques with the arithmetic for each, a defined credit step for every claim, the evidence
  required before credit is released, and a roll-up that weights activities by budget instead of
  averaging their percentages.
linkedin:
  format: document
  hook: >
    A per cent complete that is the average of six activities' percentages is not a per cent complete.
    Weight by budget, or you will report progress you have not earned.
  tags: [ProgressMeasurement, EarnedValue, ProjectControls, CostEngineering, RulesOfCredit]
  asset: one-pager
gated: false
related: [TPL-01, TPL-02, TPL-06, TPL-07, BPG-06, BPG-08]
bok_domains: [5, 6, 10]
sources:
  - "PCI Master Formula Sheet (docs/downloads/master-formula-sheet.md), August 2026 — published under the credential's retired code; the credential is PCL-AI"
placeholders: 0
---

# Progress measurement and rules of credit sheet

> The evidence trail behind every per cent complete you report.

**In one paragraph.** Per cent complete is an assertion, and the sheet that produces it is where the
assertion is either evidenced or invented. This template gives the measurement sheet and the rule-of-credit
library behind it: five techniques with the arithmetic for each, a defined credit step for every claim, the
evidence required before credit is released, and a roll-up that weights activities by budget instead of
averaging their percentages.

**Who this is for.** Cost engineers and planners who produce the monthly progress position; control
account managers who claim it; and project controls managers who have to defend it when the earned value
it produces is challenged.

---

## 1. When to use this

Build the sheet before the first progress claim, not after the first argument. The measurement technique
for an activity is a decision that belongs with the baseline: it is recorded in the work breakdown
structure dictionary (`TPL-02`) when the element is created, and the rule of credit is approved before any
credit is claimed against it.

Use it every reporting period, at the cut-off defined in the controls execution plan (`TPL-01` §3.6). Its
output feeds three places:

- **Earned value**, where per cent complete multiplied by the budget at completion of the element gives
  the earned value used in `TPL-07`.
- **The monthly report**, where period movement is the number that tells a reader whether the project is
  accelerating or stalling.
- **Payment**, on projects where interim application is driven by measured progress. Where that is the
  case, say so on the sheet, because it changes the incentives on every claim.

The one time not to use it is on an activity whose output is genuinely unmeasurable and whose budget is
material. There is no technique that rescues that; the answer is to decompose the activity until part of
it becomes measurable, or to accept level of effort and state plainly that the element earns to plan.

## 2. How to complete it

**Choose the technique from the work, not from convenience.** The five techniques, and the condition each
requires:

| Technique | Use when | Per cent complete is |
|---|---|---|
| Units complete | Output is countable and homogeneous, and the total quantity is known | Quantity installed ÷ budgeted quantity |
| Incremental milestone | One work item passes through a fixed sequence of steps | The cumulative credit of the steps completed, in order |
| Weighted milestone | A deliverable is reached through discrete events that may complete out of order | The sum of the credits of the milestones achieved |
| Level of effort | The work is support or supervision, with no output of its own | Elapsed duration ÷ planned duration |
| Apportioned effort | The work varies directly with another activity and has no independent output | The per cent complete of the named base activity |

**Distinguish incremental from weighted milestone properly.** Both assign credit at defined points. The
difference is whether out-of-order achievement is permitted. Incremental milestone is a sequence: a step
cannot be credited before its predecessor, and the sheet should refuse it. Weighted milestone is a set:
each milestone carries a weight and may be achieved in any order. Choosing incremental where the work is
genuinely non-sequential forces claimants to under-report; choosing weighted where the work is sequential
lets them claim the easy milestones first.

**Level of effort must be capped and must be small.** Level of effort earns to plan by construction, so it
generates no cost variance from schedule causes and never reports a schedule problem. Two disciplines
follow: cap it at 100 per cent so it cannot keep earning past its planned finish, and keep it to a small
share of the control account. If most of a control account is level of effort, its cost performance index
is measuring almost nothing.

**Apportioned effort must name its base activity.** Not a discipline, not a control account — one activity,
recorded on the sheet. Its per cent complete is then a formula referencing that row, so it cannot drift.

**Write the rules of credit before the first claim, and freeze them.** Changing a credit step mid-project
changes reported progress without any work being done. If a rule must change, treat it as a measurement
basis change under `TPL-04` and report the effect separately for one period.

**Require objective evidence at every credit step.** The evidence reference on the sheet is a document
number, a survey, a test certificate, an inspection release — not a name. "Confirmed by the site manager"
is not evidence; it is the same assertion again.

**Roll up by weighting, never by averaging.** The rolled-up per cent complete of a control account is total
earned budget hours divided by total budget hours, not the mean of the activity percentages. This is the
single most common arithmetic error in the discipline and §4.3 shows what it costs.

**Store percentages as decimal fractions with percentage formatting.** Type `0.55` into a cell formatted as
a percentage, not `55`. A sheet that mixes the two produces credit totals of 5,500 per cent and roll-ups
that are silently wrong by a factor of a hundred.

**Using the tables.** Copy a table block, paste into a single spreadsheet column, split on the pipe
character, and delete the alignment row.

## 3. The template

### 3.1 Sheet 1 — the measurement sheet

| Col | Field | Type | Definition and entry rule |
|---|---|---|---|
| A | Activity ID | Text | Matches the schedule activity identifier |
| B | Activity description | Text | |
| C | WBS element / control account | Text | From `TPL-02` |
| D | Technique | List | `UC` units complete · `IM` incremental milestone · `WM` weighted milestone · `LOE` level of effort · `AE` apportioned effort |
| E | Rule of credit reference | Text | The rule identifier in Sheet 2. Required for `IM` and `WM`. |
| F | Base activity (apportioned effort only) | Text | The activity ID whose per cent complete this follows |
| G | Unit of measure | Text | For `UC` — piles, cubic metres, welds, drawings, tonnes |
| H | Budgeted quantity | Number | For `UC` — the total scope in the stated unit |
| I | Budgeted hours | Number | The weight used in the roll-up. Cost may be used instead, but not both in one sheet. |
| J | Quantity to date / credit basis | Number | `UC`: quantity installed. `IM`/`WM`: cumulative credit from Sheet 2. `LOE`: elapsed duration. `AE`: leave blank. |
| K | Planned duration (LOE only) | Number | Same unit as column J for level-of-effort rows |
| L | Per cent complete this period | Calculated | See below |
| M | Per cent complete last period | Number | Carried forward from the previous issue of the sheet |
| N | Period movement | Calculated | |
| O | Earned quantity | Calculated | For `UC` rows — a cross-check |
| P | Earned hours | Calculated | The figure the roll-up uses |
| Q | Evidence reference | Text | Document, survey, certificate or inspection release number |
| R | Claimed by / verified by | Text | Two names, not one |
| S | Exception flag | Calculated | |

**Calculated column L — Per cent complete.** In words, by technique:

- **Units complete** — quantity installed divided by budgeted quantity.
- **Incremental and weighted milestone** — the cumulative credit of the milestones achieved, brought in
  from Sheet 2.
- **Level of effort** — elapsed duration divided by planned duration, capped at 100 per cent.
- **Apportioned effort** — the per cent complete of the named base activity.

Spreadsheet, as one expression covering all five:

```
=IF($D2="UC",IF($H2=0,"",$J2/$H2),
 IF(OR($D2="IM",$D2="WM"),IF($J2="","",$J2),
 IF($D2="LOE",IF($K2=0,"",MIN(1,$J2/$K2)),
 IF($D2="AE",IFERROR(INDEX($L$2:$L$500,MATCH($F2,$A$2:$A$500,0)),""),""))))
```

Every division is guarded against a zero denominator. The apportioned-effort branch looks up the base
activity's per cent complete by activity identifier; the `IFERROR` returns blank when the base activity
cannot be found, which is itself a defect worth seeing. If you prefer to keep the branches separate,
split the sheet by technique — the arithmetic is identical.

**Calculated column N — Period movement.** In words: this period's per cent complete less last period's,
expressed in percentage points.

```
=IF(OR($L2="",$M2=""),"",$L2-$M2)
```

**Calculated column O — Earned quantity.** In words: per cent complete multiplied by the budgeted
quantity. For a units-complete row this must equal the quantity to date, which makes it a free check that
the technique and the entry agree.

```
=IF(OR($L2="",$H2=""),"",$L2*$H2)
```

**Calculated column P — Earned hours.** In words: per cent complete multiplied by the budgeted hours.

```
=IF(OR($L2="",$I2=""),"",$L2*$I2)
```

**Calculated column S — Exception flag.** In words: flag a per cent complete above 100 per cent, a
backwards movement, or a milestone row with no rule reference.

```
=IF($L2="","",IF($L2>1,"OVER 100 % — quantity growth: raise a change or re-baseline the quantity",
 IF($N2<0,"NEGATIVE MOVEMENT — explain",
 IF(AND(OR($D2="IM",$D2="WM"),$E2=""),"NO RULE OF CREDIT",""))))
```

Do not cap a units-complete row at 100 per cent. A quantity that has exceeded its budget is real
information — it means the take-off was wrong or the scope has grown — and capping it hides the very thing
the sheet should surface. Level of effort is different and is capped, because it has no quantity to grow.

**Roll-up to a control account.** In words: total earned hours divided by total budgeted hours across the
activities in the account. Spreadsheet, over the sheet's data range:

```
=IF(SUMIF($C$2:$C$500,$C2,$I$2:$I$500)=0,"",SUMIF($C$2:$C$500,$C2,$P$2:$P$500)/SUMIF($C$2:$C$500,$C2,$I$2:$I$500))
```

**Earned value from the roll-up.** In words: the control account's rolled-up per cent complete multiplied
by its budget at completion. That figure is the earned value input to `TPL-07`, whose treatment of the
resulting measures this document does not repeat.

### 3.2 Sheet 2 — the rule-of-credit library

One block per rule. Rules are shared across activities of the same type; that is the point of a library.

| Col | Field | Type | Definition |
|---|---|---|---|
| A | Rule ID | Text | e.g. `ROC-CIV-03` |
| B | Technique | List | `IM` or `WM` |
| C | Applies to | Text | The work type this rule measures |
| D | Step / milestone | Text | The event that releases credit |
| E | Sequence number | Number | For `IM`, the order that must be observed. Blank for `WM`. |
| F | Credit | Number | The credit released by this step, as a decimal fraction |
| G | Cumulative credit | Calculated | For `IM` only |
| H | Objective evidence required | Text | The document or test that proves the step |
| I | Released by (role) | Text | Who confirms the evidence |
| J | Achieved this period | List | `Yes` / `No` |
| K | Date achieved | Date | |
| L | Approved by / date | Text | Approval of the rule itself, not of a claim |

**Calculated column G — Cumulative credit.** In words: the running total of credit down the sequence.

```
=IF($E2="","",SUMIFS($F$2:F2,$A$2:A2,$A2))
```

**Rule total validation.** In words: the credits within a rule must total exactly 100 per cent.

```
=IF(ROUND(SUMIF($A$2:$A$200,$A2,$F$2:$F$200),6)=1,"OK","CHECK — credits must total 100 %")
```

`ROUND` to six decimal places prevents a false failure from floating-point representation, which is
otherwise a genuine nuisance on rules with thirds.

**Claimed credit for a rule.** In words: for weighted milestone, the sum of the credits of milestones
marked achieved. For incremental milestone, the cumulative credit at the highest sequence number achieved,
which enforces the ordering.

Weighted milestone:

```
=SUMIFS($F$2:$F$200,$A$2:$A$200,$A2,$J$2:$J$200,"Yes")
```

Incremental milestone:

```
=IF(COUNTIFS($A$2:$A$200,$A2,$J$2:$J$200,"Yes")=0,0,SUMIFS($F$2:$F$200,$A$2:$A$200,$A2,$E$2:$E$200,"<="&MAXIFS($E$2:$E$200,$A$2:$A$200,$A2,$J$2:$J$200,"Yes")))
```

The incremental expression deliberately credits everything up to the highest achieved step, so a claim
that skips a step credits the skipped step too — which is the correct behaviour for a sequence, and makes
an out-of-order claim visible rather than profitable. Where `MAXIFS` is unavailable, sort the rule by
sequence and use a lookup on the last `Yes`.

## 4. Worked fragment

*Illustrative figures.* Control account CA-1000, civil works, on a fictional facility upgrade project.
The period is May 2026, cut-off 31 May 2026. The weighting basis is budgeted direct labour hours.
Currency is not used on this sheet; the resulting earned value is expressed in generic currency units (CU)
thousands in `TPL-07`. Percentages are shown to one decimal place, rounded half away from zero; hours to
whole hours.

### 4.1 The measurement sheet

| Activity | Description | Tech. | Rule | Unit | Budget qty | Budget hrs | Qty / basis | % this period | % last period | Movement (pp) | Earned hrs |
|---|---|---|---|---|---|---|---|---|---|---|---|
| ACT-1010 | Bulk excavation | UC | — | m³ | 12,000 | 2,000 | 9,600 | 80.0 % | 68.0 % | +12.0 | 1,600 |
| ACT-1020 | Bored piling | UC | — | piles | 240 | 7,800 | 108 | 45.0 % | 37.5 % | +7.5 | 3,510 |
| ACT-1030 | Pile caps and ground beams | IM | ROC-CIV-03 | — | — | 1,200 | 50.0 % | 50.0 % | 25.0 % | +25.0 | 600 |
| ACT-1040 | Structural steel erection | WM | ROC-STR-01 | — | — | 6,000 | 60.0 % | 60.0 % | 20.0 % | +40.0 | 3,600 |
| ACT-1090 | Civil supervision | LOE | — | months | — | 2,000 | 11 of 20 | 55.0 % | 50.0 % | +5.0 | 1,100 |
| ACT-1095 | Steel inspection and quality assurance | AE | base ACT-1040 | — | — | 800 | — | 60.0 % | 20.0 % | +40.0 | 480 |
| | **Control account CA-1000** | | | | | **19,800** | | **55.0 %** | **35.1 %** | **+19.9** | **10,890** |

**Verification of the activity arithmetic.**
Excavation: 9,600 ÷ 12,000 = 0.800; earned hours 0.800 × 2,000 = 1,600.
Piling: 108 ÷ 240 = 0.450; earned hours 0.450 × 7,800 = 3,510.
Pile caps: cumulative credit 50.0 per cent from rule `ROC-CIV-03` below; earned hours 0.500 × 1,200 = 600.
Steel: achieved credit 60.0 per cent from rule `ROC-STR-01` below; earned hours 0.600 × 6,000 = 3,600.
Supervision: 11 ÷ 20 = 0.550; earned hours 0.550 × 2,000 = 1,100.
Inspection: follows ACT-1040 at 60.0 per cent; earned hours 0.600 × 800 = 480.

**Verification of the roll-up.**
Budgeted hours: 2,000 + 7,800 + 1,200 + 6,000 + 2,000 + 800 = 19,800.
Earned hours: 1,600 + 3,510 + 600 + 3,600 + 1,100 + 480 = 10,890.
Per cent complete = 10,890 ÷ 19,800 = 0.550 = **55.0 per cent**.
Last period's earned hours were 1,360 + 2,925 + 300 + 1,200 + 1,000 + 160 = 6,945, giving
6,945 ÷ 19,800 = 0.3508 = 35.1 per cent, so the movement is 19.9 percentage points.

**Earned value.** Control account CA-1000 has a budget at completion of CU 4,000 thousand
(`TPL-02` §4.1). Earned value = 0.550 × 4,000 = **CU 2,200 thousand**, which is the earned value entered
for CA-1000 in `TPL-07` §4.

**A feature, not a fault.** The 40-point movement on ACT-1040 is what milestone techniques do: nothing was
credited while the frame was being erected, and 40 per cent landed the day the primary frame was signed
off. Milestone progress is lumpy by construction. If a reader needs a smoother curve, the answer is more
milestones with smaller weights, not a technique that credits partial achievement of a milestone.

### 4.2 The rules of credit behind the two milestone rows

**`ROC-CIV-03` — pile caps and ground beams, incremental milestone.** Sequential; a step cannot be
credited before its predecessor.

| Seq | Step | Credit | Cumulative | Objective evidence | Achieved |
|---|---|---|---|---|---|
| 1 | Setting out complete and checked | 10.0 % | 10.0 % | Setting-out survey record | Yes |
| 2 | Blinding placed | 15.0 % | 25.0 % | Pour record | Yes |
| 3 | Reinforcement fixed and inspected | 25.0 % | 50.0 % | Reinforcement inspection release | Yes |
| 4 | Concrete placed | 40.0 % | 90.0 % | Pour record and delivery tickets | No |
| 5 | Stripped, cured and survey accepted | 10.0 % | 100.0 % | 28-day cube results and as-built survey | No |
| | **Total** | **100.0 %** | | | |

Credit claimed: cumulative credit at the highest achieved sequence, step 3 → **50.0 per cent**.
Verification: 10.0 + 15.0 + 25.0 = 50.0.

**`ROC-STR-01` — structural steel erection, weighted milestone.** Milestones may be achieved in any order;
credit is the sum of those achieved.

| Milestone | Credit | Objective evidence | Achieved |
|---|---|---|---|
| Steel delivered to site and receipt-inspected | 20.0 % | Delivery notes and receipt inspection record | Yes |
| Primary frame erected and bolted | 40.0 % | Erection sign-off sheet | Yes |
| Secondary steel erected | 15.0 % | Erection sign-off sheet | No |
| Frame aligned, plumbed and surveyed | 15.0 % | Survey report | No |
| Grouted and final inspection released | 10.0 % | Inspection release | No |
| | **100.0 %** | | |

Credit claimed: 20.0 + 40.0 = **60.0 per cent**. Both rule totals return `OK` on the validation formula.

### 4.3 What averaging would have cost

The six activity percentages are 80.0, 45.0, 50.0, 60.0, 55.0 and 60.0. Their simple average is
350.0 ÷ 6 = 58.3 per cent. The correct budget-weighted figure is 55.0 per cent. Applied to the control
account's budget at completion of CU 4,000 thousand, the average would report earned value of
0.5833 × 4,000 = CU 2,333 thousand against the correct CU 2,200 thousand — **CU 133 thousand of earned
value that has not been earned**, and a cost performance index overstated by the same margin.

The error is small here only because the budgets are not wildly unequal. Take two activities: one of
100 budget hours at 90 per cent complete, and one of 9,900 budget hours at 10 per cent. The simple
average is (90 + 10) ÷ 2 = 50.0 per cent. The weighted answer is
(0.90 × 100 + 0.10 × 9,900) ÷ 10,000 = (90 + 990) ÷ 10,000 = **10.8 per cent**. The averaging method
reports a project nearly half complete that has barely started.

## 5. Common mistakes

**Averaging activity percentages.** See §4.3. It is the error that survives longest because the answer
always looks plausible.

**Level of effort used because measurement is inconvenient.** Level of effort earns to plan and therefore
never reports a problem. A control account that is mostly level of effort has a cost performance index
that measures the passage of time.

**Level of effort with no cap.** Past its planned finish it keeps earning and eventually reports more than
100 per cent complete, which then propagates into earned value greater than budget at completion.

**Apportioned effort pointing at a discipline rather than an activity.** It stops being a formula and
becomes an opinion, and it drifts.

**Milestone credits that do not total 100 per cent.** The activity can never reach complete, or it reaches
complete before it is. The validation formula in §3.2 exists because this is common and invisible.

**Credit released without evidence.** Once a claim can be made on assertion, the sheet stops being a
measurement instrument and becomes a negotiation. The evidence reference column is the whole control.

**Changing a rule of credit mid-project.** Reported progress moves without any work being done. Treat it
as a measurement basis change under `TPL-04` and disclose the effect for one period.

**Capping a units-complete row at 100 per cent.** Quantity growth is information: it means the take-off
was wrong or the scope has grown, and either way somebody needs to know. Flag it; do not hide it.

**One person claiming and verifying.** The two-name requirement in column R is not bureaucracy. It is the
only structural control on a number that determines both reported performance and, on many projects,
payment.

**Backwards movement passed over in silence.** Progress that reduces is either an error being corrected or
a claim being withdrawn. Both need an explanation in the same period, not a quiet restatement.

## 6. Adapting it

**Safe to change.** Weighting by cost instead of hours, provided you use one basis consistently — a sheet
that weights some activities by hours and others by cost produces a roll-up with no meaning. Adding
columns for subcontractor claim reference, payment application line, or physical location. Splitting the
sheet by control account on a large project. Adding techniques that are variants of those here, such as
fixed-formula start/finish rules, which are incremental milestone with two steps.

**Change with care.** Moving an activity from one technique to another mid-project. It is sometimes right —
a planning package decomposed into measurable work should move from level of effort to units complete —
but it changes reported progress without work being done, so it is a measurement basis change and belongs
in the change register.

**Do not remove.** The evidence reference, the two-name claim and verification, the credit-total
validation, and the budget-weighted roll-up. Those four are the reason the output can be defended.

**Where the client's own progress measurement differs**, run both and reconcile them on the sheet as an
additional column pair. Do not adopt a single method that satisfies neither; the reconciliation itself is
usually the most valuable thing on the page.

## 7. Completion checklist

- [ ] Every activity has a technique recorded in the dictionary before its first claim
- [ ] Every milestone activity references an approved rule of credit
- [ ] Every rule of credit totals exactly 100 per cent and returns `OK` on the validation
- [ ] Every credit step names the objective evidence and who releases it
- [ ] Level of effort is capped, and its share of each control account is known and stated
- [ ] Every apportioned-effort row names one base activity by identifier
- [ ] Percentages stored as decimal fractions in percentage-formatted cells
- [ ] Earned quantity equals quantity to date on every units-complete row
- [ ] Exception flags cleared or explained: over 100 per cent, negative movement, missing rule
- [ ] Roll-up computed as earned hours ÷ budgeted hours, not as an average of percentages
- [ ] Every row carries an evidence reference and two names
- [ ] Period movement reviewed activity by activity before the sheet is issued
- [ ] Resulting earned value reconciled to the figure entered in the earned value sheet

---

## Related

- `TPL-01 — Project controls execution plan` — where permitted techniques and the cut-off calendar are set
- `TPL-02 — Work breakdown structure and WBS dictionary` — where an element's technique is first recorded
- `TPL-06 — Monthly project controls report` — where period movement is reported and explained
- `TPL-07 — Earned value calculation sheet` — what the earned value produced here is used for
- `BPG-06 — Progress measurement and rules of credit` — the design reasoning behind technique selection
- `BPG-08 — Earned value in practice` — how a measured per cent complete becomes a defensible index

## Sources and standards

- PCI Master Formula Sheet (`docs/downloads/master-formula-sheet.md`), August 2026: the earned value
  identity used at §4.1 to convert a rolled-up per cent complete into earned value. Published under the
  credential's retired code; the credential is PCL-AI.

The five techniques named here are common to established cost engineering practice and are described in
the Institute's own words. No third-party sheet, rule set or wording is reproduced; the rules of credit in
§4.2 are illustrative and original.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
