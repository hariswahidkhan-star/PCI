---
id: BPG-05
series: S09
series_name: Best Practice Guides
title: Schedule quality — a practical review
subtitle: The checks that tell you whether a schedule can be trusted to recalculate, and how to report the answer
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 15
summary: >
  How to review a schedule you did not build and decide whether its dates are results or assertions:
  logic completeness and open ends, constraints, lags and leads, calendars, negative and excessive
  float, out-of-sequence progress and update integrity, duration distribution, and a walk of the
  critical path. Every check is described in the Institute's own words, with the difference between a
  rule and a convention stated explicitly. Includes a full review of a worked schedule and the verdict
  it supports.
linkedin:
  format: carousel
  hook: >
    In the worked review, nineteen and a half per cent of the critical path was lag — elapsed time no
    activity owns, nobody is resourced for, and no progress report ever mentions.
  tags: [ProjectControls, Scheduling, Planning, ScheduleQuality, ProjectManagement]
  asset: carousel-8
gated: false
related: [BPG-04, BPG-12, BPG-17, BPG-19, TPL-14, TPL-11]
bok_domains: [10]
sources: []
placeholders: 0
---

# Schedule quality — a practical review

> How to decide whether a schedule's dates are calculated results or asserted opinions.

**In one paragraph.** How to review a schedule you did not build and decide whether its dates are
results or assertions: logic completeness and open ends, constraints, lags and leads, calendars,
negative and excessive float, out-of-sequence progress and update integrity, duration distribution, and
a walk of the critical path. Every check is described in the Institute's own words, with the difference
between a rule and a convention stated explicitly. Includes a full review of a worked schedule and the
verdict it supports.

**Who this is for.** Planners and schedulers inheriting a schedule; project controls managers who have
to sign a baseline; anyone asked to accept a contractor's programme, or to say whether a completion
date can be relied on.

---

## 1. What the review is actually testing

A critical path method schedule earns its authority from one property: when something changes, it
recalculates, and the new dates are consequences of the change rather than opinions about it. A
schedule quality review tests whether that property holds.

It is not a test of whether the plan is *good*. A schedule can pass every check here and still describe
a sequence that will not work, durations that are heroic and a completion date nobody believes — those
are questions for the schedule basis, the estimators and the delivery team. Conversely, a schedule can
fail several checks and still have roughly the right end date, arrived at by judgement.

The distinction determines what the review can conclude. A schedule that fails these checks is not
necessarily wrong; it is **not defensible**, which is a different and more actionable finding — its
dates cannot be shown to follow from anything. That matters when the schedule is used to support a
delay claim, justify an acceleration cost, set a contractual completion date or feed a quantitative
risk analysis, all of which assume a network that recalculates honestly.

**A word about thresholds.** Several checks below are naturally expressed as a percentage: how many
open ends is too many, how many constraints are acceptable. There is no universal answer, and this
guide does not pretend otherwise. Where a number appears, it is either a starting point the Institute
recommends agreeing and recording *before* the review, or it is an observation from the worked example.
No threshold here is attributed to any published standard, and none should be quoted as though a
governing body had set it. The professional discipline is to agree your thresholds with the schedule's
owner in advance, record them, and apply them consistently — an argument about the threshold after the
result is known is not a review.

## 2. Before you open the file

Three preconditions determine whether the review will mean anything.

**The data date.** Every metric below is relative to it. Confirm it, and confirm that it matches the
period the progress report covers. A schedule progressed to a different date than the cost report is a
finding before any check is run.

**The schedule basis.** The controlled document recording what the baseline assumes — calendars and
their justification, productivity and crew assumptions behind key durations, sequencing rationale,
exclusions, third-party dependencies, approvals. Without it, every subsequent question ("why is this
duration 40 days?") has no answer, and the review can only observe structure, never judgement. Its
absence is itself the most significant finding a review can make.

**The calendars in use.** List them before anything else. Calendar effects are invisible on a printed
bar chart and change elapsed durations without changing the duration field, so a review that has not
enumerated the calendars is reviewing something it cannot see.

## 3. Logic completeness and open ends

Every activity except the project's genuine start and finish should have at least one predecessor and
at least one successor. An activity missing either is an **open end**: it floats free of the network,
so when reality changes around it, its dates do not move. The forward and backward passes quietly stop
being true for that activity and, more damagingly, for anything downstream of it.

Two refinements matter in practice. First, **state the base**. Open ends as a percentage of all
activities and open ends as a percentage of *incomplete* activities are different numbers, and the
second is usually the more useful, because a completed activity's missing successor no longer affects
the forecast. Second, **exclude the legitimate ends explicitly** rather than silently — a schedule with
three "project finish" milestones has a structural problem the count would otherwise hide.

**Logic density** — total relationships divided by total activities — is a coarse companion measure. A
value close to 1.0 suggests a chain with little cross-linking, which usually means real dependencies
are missing. A very high value can mean redundant logic that makes the network hard to reason about.
Density is a prompt to look, never a verdict on its own.

**Relationship types.** Finish-to-start is the type that most reliably expresses a physical dependency.
Start-to-start and finish-to-finish pairs are legitimate and often necessary for overlapping work, but
a start-to-start without a matching finish-to-finish leaves the successor's finish unconstrained, which
is an open end in disguise. Start-to-finish relationships are rare enough that each one should be
individually justified. Report the mix, and interrogate the non-finish-to-start share rather than
assuming it is wrong.

## 4. Constraints

A date constraint overrides the network. That is its purpose and its danger: a constrained activity
stops responding to its predecessors, so the schedule reports a date it has been told rather than a
date it has computed.

Some constraints are legitimate — a contractual milestone, a permit date, a fixed possession window, a
client-supplied item with a known delivery date. Each should be individually justified in the schedule
basis, naming the external fact that fixes it.

Distinguish **soft** constraints, which restrict a date without overriding logic (start no earlier
than, finish no later than), from **hard** ones, which pin a date outright (must start on, must finish
on). Hard constraints are the ones that generate silent nonsense: an activity pinned to a date its
predecessors cannot support does not report a conflict, it reports negative float somewhere else, or
nothing at all.

The review question is not "how many constraints?" but "how many constrained activities lie on the
longest path?" — because those are the ones whose dates are assertions. A schedule can carry a hundred
constraints on peripheral work and still recalculate honestly through its critical path; one hard
constraint on the critical path means the critical path is partly declared.

## 5. Lags, leads and calendars

**Lags** inject elapsed time that no activity owns. A finish-to-start relationship with a fifteen-day
lag adds fifteen days that have no resource, no progress measurement, no owner and no scrutiny. Some
lags are physically real — concrete curing, paint drying, a mandated notice period — and those should
be justified in physical terms. Where a lag represents work, it should be an activity.

The review approach is threshold-based and simple: expose **every** lag above an agreed length,
require a written physical justification for each, and convert anything unjustified into a resourced
activity. Then sum the lag on the critical path, because that is the number that shows how much of the
project's duration is unowned. §9 does this, and the result is usually the most persuasive line in the
report.

**Leads** — negative lags — allow a successor to start before its predecessor finishes. They are almost
always a modelling shortcut for an overlap that should be expressed as start-to-start with an
appropriate lag, and they behave unpredictably when progress is applied out of sequence. Treat every
lead as a finding requiring justification.

**Calendars** change elapsed duration without changing the duration field: moving an activity from a
five-day to a seven-day calendar shortens its elapsed time with no visible change anywhere.
Conventions also differ on which calendar a *lag* follows, so the same numeric lag can mean different
elapsed times in different parts of one schedule. Enumerate every calendar; confirm every assignment is
deliberate rather than inherited; fix the lag-calendar convention once, in writing; and confirm that
seasonal non-work days on weather-sensitive work sit in a calendar rather than buried in durations.

## 6. Float

**Negative float** means a late date earlier than an early date — the schedule is telling you it cannot
meet a constraint or a deadline as modelled. It is never a metric to be normalised. Either the
constraint is wrong, the logic is wrong, or the project is already late and nobody has said so. Report
the worst value and the count, and identify which of the three explanations applies.

**Excessive float** is the more interesting signal, because it usually indicates missing logic rather
than genuine slack: an activity with no successor that needs it shows enormous total float and means
nothing. Cluster the high-float activities and look at what they share — frequently a whole area or
subcontract package that was never linked in. What counts as excessive depends on project duration, so
express the threshold relative to remaining duration and agree it before the review.

## 7. Update integrity

These checks apply to a progressed schedule and are the fastest way to find out whether the update is
trustworthy.

**Actuals beyond the data date.** An activity showing an actual start or finish later than the data
date is a data error, not a judgement. There is no acceptable count other than zero. **Remaining
duration on complete activities** and **progress on activities with no actual start** are the same
class of error: both indicate the update was typed rather than derived.

**Out-of-sequence progress** — a successor started before its finish-to-start predecessor finished —
means either the logic was wrong or the update is. Either way the current critical path is suspect,
because the tool's handling of out-of-sequence work depends on a setting most readers of the schedule
have never seen. Report the count, and check the setting.

**Stale updates.** Activities whose remaining duration has not moved for several periods while showing
progress are work that is being progressed on paper and not in fact.

## 8. Durations and the critical path

**Duration distribution.** Long remaining durations hide detail: a 60-day activity reports one
percentage and can absorb months of slippage before its finish date moves. Very short ones in large
numbers inflate maintenance without adding control. Express the check against the reporting cycle — an
activity whose remaining duration exceeds two reporting periods cannot be meaningfully statused — and
see `BPG-06 — Progress measurement and rules of credit` for the ones that must stay long.

**The critical path walk** is the check that no metric replaces, and the only one that requires
judgement rather than a query. Print the longest path and read it end to end, asking of each link: does
this activity physically have to finish before that one starts? A credible critical path tells a story
a site manager would recognise. An incredible one jumps between unrelated areas, passes through
administrative activities, or runs through a chain of lags and constraints rather than work.

Three specific tests. Does the path run through **activities or through lags and constraints**? What
proportion of its duration is lag (§9 computes this)? And does it pass through **level-of-effort or
management activities** — "project management", "monthly reporting" — which is a sign the network is
being held together by activities that describe overhead rather than production.

## 9. How this goes wrong

**The review becomes a metric report.** Twelve percentages are computed, tabulated and sent; nobody
walks the critical path, so the one finding that would have changed a decision is missed. Metrics
locate the questions; they do not answer them.

**Thresholds are argued after the fact.** The reviewer reports 8 % constraints and the owner says the
acceptable level is 10 %. Agree thresholds in writing beforehand and the conversation is about the
schedule rather than about the reviewer.

**A failed check is treated as a verdict.** A schedule with open ends is reported as "wrong". The
finding is that its dates cannot be shown to follow from its logic — defensibility, not accuracy.

**The schedule is fixed to pass the checks.** Open ends are closed by linking activities to the project
finish milestone; constraints are converted to long lags; high float is removed by adding logic that
does not exist. Every metric improves and the schedule becomes less true. This is the most damaging
failure in this guide, because it is invisible in the next review.

**Calendars are never examined.** The review covers logic, constraints and float, and never lists the
calendars. An activity on a seven-day calendar in an otherwise five-day schedule quietly shortens the
critical path, and no metric in the report would show it.

**Out-of-sequence progress is normalised.** A recurring count of out-of-sequence activities is accepted
as "how the site works". It may be — in which case the logic is wrong and should be corrected, because
the network is now describing a sequence that is not being followed.

**The review has no owner and no follow-up.** Findings are issued, the next baseline is accepted
anyway, and the same findings recur. A review without a named person accountable for closing each
finding is an audit trail for someone else's later argument.

## 10. Worked example

*Illustrative figures.* A single progressed schedule at one data date. Percentages are stated with
their base. Working-day durations on a five-day calendar unless otherwise stated. No real project is
implied, and no threshold below is attributed to any published standard — each is a starting point
agreed with the schedule's owner before the review.

### 10.1 The population

| Item | Count |
|---|---:|
| Total activities | 1,480 |
| — of which milestones | 96 |
| Complete | 612 |
| In progress | 174 |
| Not started | 694 |
| Total relationships | 3,120 |

Check: 612 + 174 + 694 = 1,480. Incomplete activities = 174 + 694 = 868. Activities carrying progress
= 612 + 174 = 786.

### 10.2 Logic

```
Logic density = relationships ÷ activities = 3,120 ÷ 1,480 = 2.11
```

Relationship mix: finish-to-start 2,410; start-to-start 428; finish-to-finish 233; start-to-finish 49.
Check: 2,410 + 428 + 233 + 49 = 3,120.

```
Non-finish-to-start share = (3,120 − 2,410) ÷ 3,120 = 710 ÷ 3,120 = 22.8 %
```

Open ends: 63 activities lack a predecessor or a successor. Against all activities that is
63 ÷ 1,480 = **4.3 %**. Two of the 63 are the project's genuine start and finish milestones, so the
meaningful figure excludes them on both sides of the ratio: 61 ÷ 1,478 = **4.1 %**. The 49
start-to-finish relationships are each a separate question, since that type is rare enough to require
justification one by one.

### 10.3 Constraints, lags and leads

| Check | Finding |
|---|---|
| Activities carrying a date constraint | 118 (118 ÷ 1,480 = 8.0 %) |
| — of which hard constraints | 22 |
| Relationships carrying a lag | 274 (274 ÷ 3,120 = 8.8 %) |
| — of which exceed 10 working days | 41, totalling 690 days |
| Relationships carrying a lead (negative lag) | 37 |
| Calendars in use | 4 (five-day, six-day, seven-day, shutdown) |

Mean length of the 41 long lags = 690 ÷ 41 = **16.8 working days**. Each requires a physical
justification; the 37 leads each require a justification or conversion to an explicit overlap.

### 10.4 Float and update integrity

| Check | Finding |
|---|---|
| Activities with negative total float | 84; worst value −27 days |
| Activities with total float above 60 working days | 212 (212 ÷ 1,480 = 14.3 %) |
| Incomplete activities with remaining duration above 44 working days | 133 (133 ÷ 868 = 15.3 %) |
| Out-of-sequence progress | 46 (46 ÷ 786 = 5.9 % of activities carrying progress) |
| Actual dates later than the data date | 9 |

The 44-working-day threshold is two reporting periods on a monthly cycle with a five-day calendar,
which is where the check comes from rather than from any published figure.

### 10.5 The critical path

The longest path returned by the tool contains **41 activities** and spans **318 working days** from the
data date to project completion. Reading it end to end:

- **11 of the 41** activities carry a date constraint.
- **5 relationships** on the path carry lags, totalling **62 working days**.

```
Lag share of the critical path = 62 ÷ 318 = 19.5 %
```

Nearly a fifth of the remaining critical path is elapsed time that no activity owns, that no resource
is assigned to, that no progress report mentions and that nobody is accountable for delivering. That
single number does more to explain why the completion date has not moved in four months than every
other metric in this review combined.

### 10.6 The verdict

The schedule is **not currently defensible as a forecast**, for three reasons that are all remediable:
9 actual dates beyond the data date are a data error with no acceptable count other than zero; 37 leads
make out-of-sequence behaviour unpredictable; and 62 working days of unjustified lag on the critical
path mean the completion date is partly asserted rather than computed.

It may still be approximately right about the end date. That is not the same claim, and a review that
conflates the two helps nobody. The report should say what it can support: *these dates cannot at
present be shown to follow from this network, and here are the six things that would change that.*

## 11. Checklist

Take this into the schedule review. Agree every threshold with the schedule's owner before you run a
single query, and record what you agreed.

**Preconditions**

- [ ] What is the data date, and does it match the cost report's cut-off?
- [ ] Is there a schedule basis document, and when was it last updated?
- [ ] How many calendars are in use, and is every assignment deliberate?
- [ ] Which calendar do lags follow, and is that written down anywhere?

**Logic**

- [ ] Open ends: how many, against which base, and how many are on the longest path?
- [ ] Logic density, and does a low value point to missing cross-links?
- [ ] What is the non-finish-to-start share, and is every start-to-finish relationship justified?
- [ ] Does every start-to-start have a matching finish-to-finish?

**Steering instruments**

- [ ] How many constrained activities, how many hard, and how many on the longest path?
- [ ] Is every constraint justified against a named external fact in the schedule basis?
- [ ] Every lag above the agreed length: listed, with a physical justification each?
- [ ] Every lead: justified, or converted to an explicit overlap?

**Float and updates**

- [ ] Negative float: count, worst value, and which of the three explanations applies?
- [ ] High-float clusters: do they share an area, a package or a subcontractor?
- [ ] Actual dates beyond the data date: the count must be zero.
- [ ] Out-of-sequence progress: count, and what is the tool's out-of-sequence setting?
- [ ] Any activity progressed for several periods with no movement in remaining duration?

**The path**

- [ ] Has someone read the longest path end to end and can they narrate it?
- [ ] What proportion of the critical path's duration is lag?
- [ ] Does the path run through level-of-effort or administrative activities?
- [ ] Would a site manager recognise this as the sequence that governs the finish date?

**Reporting**

- [ ] Is each finding written as a defensibility statement rather than a verdict on accuracy?
- [ ] Does each finding have a named owner and a date?
- [ ] Has anyone checked that last review's findings were closed by correction rather than by cosmetics?

---

## Related

- `BPG-04 — Baselining and baseline change control` — the review runs before a schedule baseline is accepted, and at every re-baseline.
- `BPG-12 — Claims and extension of time` — why a network that cannot recalculate cannot support a delay analysis.
- `BPG-17 — Quantitative schedule risk analysis` — the analysis that must not be run on a schedule that fails these checks.
- `BPG-19 — Project controls assurance and health checks` — where the schedule review sits within a wider assurance cycle.
- `TPL-14 — Schedule quality review checklist` — the instrument, with the threshold-agreement fields.
- `TPL-11 — Quantitative schedule risk analysis input sheet` — what a schedule must satisfy before it becomes a risk model.

## Sources and standards

Drawn from the Institute's Body of Knowledge, Domain 10 (Project Scheduling): network analysis and the
forward and backward passes, leads and lags, calendars, schedule health checks and the schedule basis
document.

Several published bodies of practice define schedule assessment metrics, and organisations frequently
adopt one as a house standard. This guide deliberately describes the underlying checks in the
Institute's own words and reproduces no third party's metric definitions, thresholds or scoring. Where
a number appears here it is either an assumption of the worked example or a threshold the Institute
recommends agreeing locally, and it is labelled as such. No threshold in this document should be quoted
as the requirement of any named standard.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
