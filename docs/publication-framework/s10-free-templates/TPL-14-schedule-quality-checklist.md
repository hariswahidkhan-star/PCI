---
id: TPL-14
series: S10
series_name: Free Templates
title: Schedule quality review checklist
subtitle: Fourteen checks, each with what is measured, how to measure it, and a place for the evidence
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: professional
reading_time_min: 14
summary: >
  A schedule quality review in fourteen checks covering logic, open ends, constraints, negative float, lags
  and leads, out-of-sequence progress, duration distribution, critical path credibility, resourcing,
  calendars, float integrity, baseline and progress integrity, milestones and interfaces, and coding. The
  Institute supplies the measurement; the project supplies the threshold, because thresholds are conventions
  to be agreed and not universal truths. Every check carries a result, an evidence field and an owner.
linkedin:
  format: document
  hook: >
    A schedule quality review that gives you a percentage and no evidence is theatre. Separate the two
    things: the measurement, which is arithmetic and belongs to the reviewer, and the threshold, which is a
    convention and belongs to the project.
  tags: [ProjectControls, Scheduling, Planning, ScheduleQuality, Assurance]
  asset: checklist-pdf
gated: true
related: [BPG-05, BPG-17, TPL-11, TPL-15, BPG-04]
bok_domains: [10]
sources: []
placeholders: 0
---

# Schedule quality review checklist

> Fourteen checks, each with what is measured, how to measure it, and a place for the evidence.

**In one paragraph.** A schedule quality review in fourteen checks covering logic, open ends, constraints,
negative float, lags and leads, out-of-sequence progress, duration distribution, critical path credibility,
resourcing, calendars, float integrity, baseline and progress integrity, milestones and interfaces, and
coding. The Institute supplies the measurement; the project supplies the threshold, because thresholds are
conventions to be agreed and not universal truths. Every check carries a result, an evidence field and an
owner.

**Who this is for.** Planners and schedulers reviewing their own work before issue; project controls
managers and assurance reviewers reviewing someone else's; and the risk analysts who must not run a
simulation on a schedule that has not passed.

---

## 1. When to use this

**Before a baseline is accepted.** A baseline is a commitment, and accepting one that cannot pass these
checks means committing to a network that will not behave predictably when it is updated.

**Every reporting period, on the update.** Most of these checks take minutes once the reports are built,
and the failures that matter — out-of-sequence progress, actual dates beyond the data date, constraints
added quietly — arise in updates rather than in baselines.

**Before a quantitative schedule risk analysis.** A simulation propagates whatever logic it is given. On a
network with open ends the uncertainty never reaches the completion milestone and the model reports a
confident, narrow and worthless distribution. `TPL-11 — Quantitative schedule risk analysis input sheet`
requires a reference to this review in its run configuration for exactly that reason.

**Before a schedule is relied on in a submission.** A delay analysis built on an unvalidated programme fails
the moment the other party runs these checks themselves.

The review answers one question: *can this schedule be relied on for the purpose it is about to be used
for?* That purpose belongs in the header, because the answer changes with it. A schedule good enough to
manage a fortnight's work is not necessarily good enough to support a claim.

## 2. How to complete it

### 2.1 Separate the measurement from the threshold

This is the design principle of the whole instrument, and it is why the checks below give you formulas and
not pass marks.

**The measurement is arithmetic.** How many activities have no successor. What proportion of activities
carry a date constraint. What the minimum total float is. These are facts about a file and two competent
reviewers will get the same answer.

**The threshold is a convention.** Whether three per cent of activities without successors is acceptable is
a judgement about this project, its size, its stage, its contract and what the schedule is being used for.
There is no universal figure, and any number presented as one should be treated with suspicion until its
provenance is produced.

So: agree the thresholds at baseline, with the planner in the room, and record them in the parameter block.
Write them down before the first review, not after the first result. A threshold set after the measurement
is not a threshold; it is a negotiation.

Where this template offers a number, it is labelled **convention to be agreed** and it is offered as a
starting point for that conversation. No threshold here is attributed to any published standard, because
none of them comes from one.

### 2.2 Define the population before you count anything

Every proportion in this checklist has a denominator, and an undefined denominator makes the whole review
unreproducible. Agree and record:

- Whether milestones are included in the activity count. Usually they are not, because a milestone has zero
  duration and no predecessor requirement of the same kind.
- Whether level-of-effort, hammock or summary activities are included. Usually they are not, and they are
  usually the reason a first review reports alarming open-end figures.
- Whether completed activities are included. For an update review, completed activities should generally be
  excluded from the forward-looking checks and included in the progress-integrity checks.
- Whether subcontractor or supplier fragnets integrated into the schedule are in scope.

Record the population as a number in the parameter block, and record the filter used to produce it. Someone
must be able to reproduce your denominator.

### 2.3 Grade honestly, in five states

| Result | What it means |
|---|---|
| **Pass** | The check is met, and the evidence field names what was inspected |
| **Observation** | The check is not met, but the effect is understood and contained. Recorded with an owner and a date |
| **Fail** | The check is not met and the schedule cannot be relied on for the stated purpose until it is corrected |
| **Not applicable** | The check does not apply, with the reason stated |
| **Not tested** | The reviewer could not test it — no access, no report, no time. Stated, not omitted |

**Not tested** is the state that makes a checklist honest. The usual failure of schedule reviews is silent
omission: a check nobody ran appears indistinguishable from a check that passed. If you could not test it,
say so, and the reader can decide what to do about it.

### 2.4 Make the evidence field reproducible

The evidence field should let another reviewer get the same answer without asking you anything. That means:
the file name and revision, the data date, the report or filter used, the count returned, and where the
output is stored. "Reviewed logic" is not evidence. "Filter TF < 0 on rev 12, data date 31 July 2026,
returned 87 activities, output at `\reviews\2026-07\SQ-04.csv`" is.

## 3. The template

### 3.1 Parameter block

Held above the checklist or on a `Parameters` sheet, completed before the first review.

| Parameter | Value | Note |
|---|---|---|
| Schedule file and revision | | The exact file reviewed |
| Data date | | |
| Purpose of the review | | Baseline acceptance · Monthly update · Pre-QSRA · Pre-submission · Assurance |
| `Total_Activities` | | The agreed population per §2.2 |
| Population filter used | | So the denominator is reproducible |
| Reporting period length, in working days | | Drives the duration check |
| Agreed thresholds | | One per check, agreed at baseline |
| Reviewer and date | | |
| Planner and date received | | |

### 3.2 The checks

Each row of the instrument carries: check ID, check, what is measured, how to measure it, the agreed
threshold, the result, the evidence, the owner and a due date. The measurements are set out below; the
thresholds are yours.

**SQ-01 — Logic completeness.** *What is measured:* the number of activities with no predecessor and the
number with no successor, excluding the single project start and the single project finish. *Why it
matters:* an activity with no predecessor can start at any time the software likes; an activity with no
successor cannot pass delay to anything, which means it can slip indefinitely without the completion date
moving. *Measure it:* count from the network report and express each as a proportion of the population.

*Formula in words:* activities missing a predecessor, divided by the agreed population.
*Spreadsheet:* `=IF(N(Total_Activities)=0,"",Missing_Predecessors/Total_Activities)` and
`=IF(N(Total_Activities)=0,"",Missing_Successors/Total_Activities)`.
*Threshold:* convention to be agreed. A common starting position is zero, on the grounds that every open end
should have a written reason.

**SQ-02 — Undriven starts and finishes.** *What is measured:* activities whose only successor relationship
is start-to-start, and activities whose only predecessor relationship is finish-to-finish. *Why it matters:*
these pass SQ-01 and are still open ends. An activity whose only successor is start-to-start has an undriven
finish — it can run as long as it likes without consequence. This is the open end that survives the first
review. *Measure it:* count from the relationship report, filtered by relationship type per activity.
*Threshold:* convention to be agreed.

**SQ-03 — Constraint use.** *What is measured:* the number and type of date constraints, the proportion of
the population carrying one, and how many are of a type that can override logic. *Why it matters:* a
constraint that overrides logic stops the network telling you the truth. Delay stops propagating, float
becomes fictional, and the schedule reports a date it is holding rather than a date it is calculating. *Also
record:* whether each constraint has a written reason, a named owner and a review date, and whether it comes
from the contract or from convenience.

*Formula in words:* constrained activities divided by the agreed population.
*Spreadsheet:* `=IF(N(Total_Activities)=0,"",Constrained_Activities/Total_Activities)`.
*Threshold:* convention to be agreed. Contract-imposed dates are usually accepted; convenience constraints
usually are not, and the count of logic-overriding constraints is often agreed at zero.

**SQ-04 — Negative float.** *What is measured:* the minimum total float in the schedule, the number of
activities carrying negative float, and the paths they sit on. *Why it matters:* negative float is not a
status, it is a message — the plan as drawn does not work. It usually means a constraint is fighting the
logic, and it must be either resolved by re-planning or accepted explicitly by someone with the authority to
accept it. *Measure it:* filter on total float below zero; report the count, the minimum value and the
milestone each path drives.
*Threshold:* convention to be agreed. Zero is the usual starting position for an accepted baseline; an
update carrying negative float needs a recovery position, not a threshold.

**SQ-05 — Lags and leads.** *What is measured:* the number of relationships carrying a lag, the number
carrying a negative lag, the longest lag, and whether each has a written reason. *Why it matters:* a lag is
a promise that nothing happens for a period, and nothing can be progressed, resourced or reported during it.
A lag longer than a reporting period is usually work — curing, drying, approval, delivery — and should be an
activity so it can be tracked. A negative lag almost always means either the logic is wrong or the activity
is too coarse to describe what actually happens. *Measure it:* count from the relationship report.
*Threshold:* convention to be agreed. Negative lags are often set at zero; a maximum lag duration is worth
agreeing at the same time.

**SQ-06 — Out-of-sequence progress.** *What is measured:* the number of activities progressed ahead of their
predecessors, and the effect on the completion date under retained logic versus progress override. *Why it
matters:* out-of-sequence progress means the network no longer describes how the work is being done, and the
two calculation settings can give materially different completion dates from the same file. *Measure it:*
the software's out-of-sequence report, plus a calculation under each setting with both dates recorded.
*Threshold:* convention to be agreed. The count matters less than whether the logic has been corrected to
reflect what is actually happening.

**SQ-07 — Duration distribution.** *What is measured:* the number of activities with an original duration
longer than the reporting period, the number below a very short floor, and the shape of the distribution.
*Why it matters:* an activity longer than a reporting period cannot be progressed meaningfully — its percent
complete will be an opinion — and a schedule full of one-day activities generates noise without adding
control. *Measure it:* count from the duration report against the agreed bounds.

*Formula in words:* activities longer than the reporting period, divided by the agreed population.
*Spreadsheet:* `=IF(N(Total_Activities)=0,"",Long_Duration_Activities/Total_Activities)`.
*Threshold:* convention to be agreed. The principle worth agreeing is that an activity that cannot be
measured within one reporting cycle is a candidate for breakdown; the tolerated proportion is a project
decision.

**SQ-08 — Critical path credibility.** *What is measured:* whether the longest path, traced end to end,
describes how the job will actually be built. *Why it matters:* this is the only check on the list that
cannot be automated, and it is the one that finds the serious problems. *Measure it:* trace the longest path
from completion back to the data date and read it aloud to someone who knows the work. Then ask three
questions. Does it pass through procurement, design approvals, commissioning and handover, or does it run
only through construction? Does it change wildly between updates, which usually means near-critical paths
are separated by almost nothing? How many near-critical paths are there, and what is the float separation
between them? *Record:* the path itself, the answer to each question, and the number of paths within the
agreed near-critical band.
*Threshold:* not a numeric check. The result is a reviewer's judgement, and the evidence is the traced path.

**SQ-09 — Resourcing.** *What is measured:* whether activities carry resources, whether resource assignments
reconcile to the cost baseline, and whether the resulting histogram is achievable. *Why it matters:* an
unresourced schedule cannot tell you whether the plan is possible, only whether it is arithmetically
consistent. *Measure it:* count unresourced activities in scope, reconcile total resourced value to the cost
baseline, and inspect the histogram for peaks nobody has agreed to staff.
*Threshold:* convention to be agreed, and often "not applicable" where the project does not resource-load —
in which case say so rather than passing the check by default.

**SQ-10 — Calendar sanity.** *What is measured:* the number of distinct calendars, which activities sit on
the default calendar, whether non-working periods are reflected, and whether milestone calendars match the
activities that drive them. *Why it matters:* a seven-day calendar on work that happens five days a week is
a silent accelerator that shortens every affected path. A milestone on a different calendar from its driving
activity produces dates that do not reconcile. *Measure it:* activity count by calendar; inspect the
non-working periods in each; compare each contract milestone's calendar with its driver's.
*Threshold:* convention to be agreed. Every calendar in use should have a stated purpose.

**SQ-11 — Float integrity.** *What is measured:* the distribution of total float, the count of activities
with float above an agreed high bound, and whether free float and total float are being used correctly in
reporting. *Why it matters:* a large block of activities carrying very high float usually means missing
successors or a constraint holding the end of the network, not genuine slack. *Measure it:* a histogram of
total float across the population, with the high-float tail investigated rather than reported.
*Threshold:* convention to be agreed, and the high bound needs setting alongside it.

**SQ-12 — Baseline and progress integrity.** *What is measured:* whether the current schedule reconciles to
the approved baseline; whether every difference traces to an approved change; whether the data date is
correct; whether any activity is progressed beyond the data date; whether any actual date is in the future;
and whether any completed activity carries remaining duration. *Why it matters:* these are the errors that
make a schedule report a position that cannot be true, and they are all detectable in one pass. *Measure
it:* activity-count and date reconciliation to the baseline; filters for actual dates after the data date
and for remaining duration on completed activities.
*Threshold:* the mechanical checks are usually agreed at zero. The baseline reconciliation is a count of
unexplained differences.

**SQ-13 — Milestones and interfaces.** *What is measured:* whether every contract milestone is present and
correctly dated, whether interface milestones with other parties exist and are agreed, and whether each has
a named owner. *Why it matters:* interfaces are where projects fail and they are the part of the schedule
nobody owns by default. *Measure it:* reconcile the milestone list against the contract and against each
interface agreement.
*Threshold:* usually complete presence, agreed at baseline.

**SQ-14 — Coding and structure.** *What is measured:* whether the WBS coding is complete, whether activity
codes are populated for every report the project is contractually or internally required to produce, and
whether the schedule maps to the cost breakdown structure. *Why it matters:* incomplete coding is discovered
at the worst possible moment, which is when a report is due. *Measure it:* count of activities with a blank
value in each required code field.
*Threshold:* usually zero blanks in the required fields.

### 3.3 Summary calculations

| Field | Formula in words | Spreadsheet expression |
|---|---|---|
| Checks tested | Count of results that are Pass, Observation or Fail | `=COUNTIF($F:$F,"Pass")+COUNTIF($F:$F,"Observation")+COUNTIF($F:$F,"Fail")` |
| Fails | Count of Fail results | `=COUNTIF($F:$F,"Fail")` |
| Pass rate of checks tested | Passes divided by checks tested; blank if nothing was tested | `=IF(COUNTIF($F:$F,"Pass")+COUNTIF($F:$F,"Observation")+COUNTIF($F:$F,"Fail")=0,"",COUNTIF($F:$F,"Pass")/(COUNTIF($F:$F,"Pass")+COUNTIF($F:$F,"Observation")+COUNTIF($F:$F,"Fail")))` |
| Not tested | Count of checks the reviewer could not test | `=COUNTIF($F:$F,"Not tested")` |
| Open actions past due | Actions with a due date before the review date and no closure | `=COUNTIFS($I:$I,"<"&Review_Date,$J:$J,"<>Closed")` |

The pass rate deliberately excludes Not applicable and Not tested from its denominator, and the count of
Not tested is reported beside it. A single figure that quietly counts an untested check as a pass is how a
review becomes reassurance.

**Read the fails, not the percentage.** The pass rate is a communication device for a report page. The
verdict is the list of Fails and what they prevent the schedule being used for, and that verdict belongs in
a sentence, not a number.

### 3.4 Pasting it into a spreadsheet

Copy the header line into cell A1 and split on the pipe character. Column F is the result column referenced
by the summary formulas.

```
Check ID|Check|What is measured|Measured value|Agreed threshold|Result|Evidence|Owner|Action due|Action status|Notes
```

## 4. Worked fragment

*Illustrative figures.* Three checks from a monthly update review. Schedule revision 12, data date 31 July
2026. Agreed population: 1,240 activities, excluding 46 milestones and 8 level-of-effort activities, filter
recorded in the parameter block. Reporting period: one calendar month, taken as 21 working days for the
duration check.

| Check | What is measured | Measured value | Agreed threshold | Result | Evidence |
|---|---|---|---|---|---|
| SQ-01 Logic completeness | Activities with no predecessor / no successor, excluding project start and finish | 30 (2.4 %) / 43 (3.5 %) | Zero, other than project start and finish | Fail | Network report, rev 12, DD 31 Jul 26; filters "no predecessor" and "no successor"; counts 31 and 44 before excluding the project start and finish; output at `\reviews\2026-07\SQ-01.csv` |
| SQ-04 Negative float | Minimum total float; count of activities with total float below zero | −18 days; 87 activities | Zero | Fail | Total float report, rev 12, DD 31 Jul 26, filter TF < 0; 87 rows; all on the path to the substation energisation milestone; output at `\reviews\2026-07\SQ-04.csv` |
| SQ-07 Duration distribution | Activities with original duration greater than the reporting period | 96 (7.7 %) | 5 %, agreed at baseline as a project convention | Observation | Duration report, rev 12, filter OD > 20 working days; 96 rows; 61 of them in the commissioning phase; output at `\reviews\2026-07\SQ-07.csv` |

**The substitutions.** SQ-01: missing predecessors `31 − 1 = 30`, and `30 ÷ 1,240 = 0.0242`, reported as
2.4 %. Missing successors `44 − 1 = 43`, and `43 ÷ 1,240 = 0.0347`, reported as 3.5 %. SQ-07:
`96 ÷ 1,240 = 0.0774`, or 7.7 %, against a threshold of 5 % agreed at baseline.

**How to read this fragment.** Two Fails and one Observation, and they are not equal. SQ-04 is the one that
stops work: 87 activities at up to eighteen days of negative float on a single milestone path means the
schedule is asserting that a contractual date cannot be met, and either the plan changes or somebody accepts
that in writing. SQ-01 is the one that invalidates other uses: with 43 activities unable to pass delay to
anything, this file cannot support a risk simulation and cannot support a delay analysis, whatever it says
about the completion date. SQ-07 is a genuine observation — 7.7 % against an agreed 5 %, concentrated in
commissioning, where long activities are common and often defensible. It gets an owner and a date, not an
escalation.

Note what the threshold column is doing. The 5 % in SQ-07 is not a standard and is not presented as one; it
is what this project agreed at baseline, recorded before the first review, and it appears on the face of the
result so that anyone reading it knows what the measured 7.7 % is being judged against.

## 5. Common mistakes

**A pass mark with no evidence.** A review reporting 86 % with an empty evidence column cannot be repeated,
challenged or trusted. The evidence field is the check on the checker.

**Thresholds invented after the measurement.** Setting the bar once you know the score is the most common
way a review becomes decorative. Agree thresholds at baseline and record the date they were agreed.

**Borrowed thresholds presented as standards.** A number picked up from another project, or half-remembered
from a course, and then reported as though it came from a published standard, is a claim that will not
survive being asked for a source. If it is a convention, label it a convention.

**An undefined denominator.** A proportion computed on "all activities" one month and "incomplete
activities excluding milestones" the next produces a trend that is an artefact of the filter.

**Silent omission.** The check nobody had time to run, reported as nothing at all, reads as a pass. Use Not
tested.

**Running only the automated checks.** SQ-08 cannot be automated and is where the serious findings are. A
review that reports twelve clean metrics and never traced the longest path has not looked at the schedule.

**Reviewing the baseline once and never the updates.** Most of these defects arrive in updates: constraints
added under pressure, out-of-sequence progress, actual dates typed into the wrong column. A baseline-only
review checks the schedule at the one moment it was most likely to be clean.

**Treating Observations as a filing system.** An Observation without an owner and a date is a Fail that has
been made comfortable.

**Confusing schedule quality with schedule realism.** Every check here can pass on a schedule whose durations
are fantasy. Quality is about whether the network behaves correctly; realism is a separate question, and it
is answered by the duration elicitation in `TPL-11` and by asking the people doing the work.

## 6. Adapting it

**Safe to change.** Every threshold — that is the point. The population definition, the check IDs, the
addition of checks your organisation or contract requires: earned value fields populated, a required update
narrative, a specific coding scheme, a client's own reporting requirement.

**Safe to add.** A trend block carrying each measured value across the last six periods, which turns the
review from a snapshot into a control and shows whether the schedule is getting better or worse. A severity
weighting where some checks matter more for the stated purpose. A second result column recording the
position at the previous review, so closure is visible.

**Do not change.** The separation of measurement from threshold. The evidence field. The Not tested state.
The requirement to record the purpose of the review in the header, because the verdict depends on it. And
SQ-08 — the moment the critical path trace comes off the list, the review stops looking at the schedule and
starts looking at the file.

### 6.1 Issuing the review

- The parameter block is complete, including the population, the filter and the purpose of the review.
- Every threshold in use was agreed before this review, and the agreement date is recorded.
- Every check has one of the five results — none is blank.
- Every Fail and Observation has a named owner and a due date.
- Every evidence field names the file, the revision, the data date, the filter and the count.
- The longest path has been traced and read to someone who knows the work, and the trace is attached.
- The Not tested count is reported next to the pass rate, not buried.
- The verdict is stated as a sentence answering the header's question: whether this schedule can be relied
  on for the purpose it is about to be used for.
- The planner has seen the findings before anyone else did.

---

## Related

- `BPG-05 — Schedule quality — a practical review` — the reasoning behind each check and how to fix what the
  review finds
- `BPG-17 — Quantitative schedule risk analysis` — why a simulation on an unreviewed network produces a
  confident wrong answer
- `TPL-11 — Quantitative schedule risk analysis input sheet` — the run configuration that must reference
  this review
- `TPL-15 — Project controls health check` — where schedule quality sits within a whole-function assessment
- `BPG-04 — Baselining and baseline change control` — what a baseline commits to, and why SQ-12 reconciles
  to it

## Sources and standards

This is an original instrument developed by the Institute. It reproduces no third-party checklist, template,
metric set or scoring scheme, and no threshold in it is attributed to any published standard — because none
of them comes from one. Every check is described in the Institute's own words and every numeric value that
appears is labelled as a convention to be agreed by the project. Where a project is contractually required
to meet a specific published schedule-quality specification, that specification governs and this checklist
is a supplement to it, not a substitute.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
