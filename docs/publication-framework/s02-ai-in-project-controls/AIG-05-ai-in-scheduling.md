---
id: AIG-05
series: S02
series_name: AI in Project Controls Guide
title: AI in scheduling — and what must not be automated
subtitle: Where machine checking earns its place in a planning function, and the decisions that stay with the planner
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: professional
reading_time_min: 14
summary: >
  A method document on AI in planning and scheduling. It sets out what machine capability genuinely
  contributes — logic and health checking at volume, duration realism against history, progress-based slip
  prediction, scenario assembly and integration checks against cost — and the eight decisions that must
  never be automated, from accepting a critical path to inferring progress, fixing dates with constraints
  and judging delay causation. Its centre is a validation protocol for an AI-produced schedule output and a
  worked example in which a health sweep uncovers a hidden constraint concealing a slip.
linkedin:
  format: article
  hook: >
    A schedule checker can find every dangling activity in a 1,480-line programme in minutes. Deciding what
    each one means, and whether the finish date was ever real, is still the planner's job.
  tags: [ProjectControls, Scheduling, Planning, ArtificialIntelligence]
  asset: checklist-pdf
gated: false
related: [AIG-04, AIG-06, BPG-05, BPG-12, TPL-14]
bok_domains: [10, 13]
sources:
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
  - "PCI Body of Knowledge, Domain 10 — Project scheduling (Institute manuscript, 2026)"
placeholders: 0
---

# AI in scheduling — and what must not be automated

> Where machine checking earns its place in a planning function, and the decisions that stay with the
> planner.

**In one paragraph.** A method document on AI in planning and scheduling. It sets out what machine
capability genuinely contributes — logic and health checking at volume, duration realism against history,
progress-based slip prediction, scenario assembly and integration checks against cost — and the eight
decisions that must never be automated, from accepting a critical path to inferring progress, fixing dates
with constraints and judging delay causation. Its centre is a validation protocol for an AI-produced
schedule output and a worked example in which a health sweep uncovers a hidden constraint concealing a
slip.

**Who this is for.** Planners, planning managers, project controls managers and the commercial staff who
rely on a programme as a contractual record.

---

## 1. What a schedule is, and why that constrains automation

A schedule is not a forecast of dates. It is an **argument about sequence** — that this work can follow
that work, in this duration, with these resources — from which dates are a consequence. It is also, on most
contracts, a record with commercial weight: the baseline against which delay is measured, the artefact an
extension-of-time (EOT) claim is built on, and often a submission the other party may accept or reject.

That dual character sets the boundary for automation cleanly. Machine capability is excellent at
**interrogating the argument** — finding the places where the network cannot mean what it appears to mean.
It is not competent to **make the argument**, because the argument rests on knowledge of the work, the
site, the resources and the contract that is not in the file. And it must never be allowed to quietly
change the record.

Two terms are used throughout: the **critical path method (CPM)** is the network technique that derives
dates and float from activity durations and logic; **float** is the time an activity can move without
affecting a later constraint or the finish.

## 2. What AI contributes to a planning function

**2.1 Logic and health checking at volume.** This is the strongest use, and it is close to pure gain. A
checker can examine every activity in a large programme for open ends (activities without a predecessor or
successor), hard constraints that fix dates irrespective of logic, long lags substituting for real
activities, negative float, out-of-sequence progress, misassigned calendars, durations far outside the
population, and logic types used in ways that break the network's meaning. The output is an exception list
in minutes rather than the two days a manual review of a large programme takes — which in practice means
the review happens every update instead of at baseline only. `BPG-05 — Schedule quality: a practical
review` owns the checks themselves; the contribution here is frequency and coverage.

**2.2 Duration realism against history.** Where a normalised history of comparable activities exists, a
model can flag durations that sit far outside what similar work has actually taken. This is a challenge
mechanism, not a correction: the flag opens a conversation with the person who owns the duration, whose
reasons may be perfectly good.

**2.3 Progress-based slip prediction.** Extrapolating achieved progress rates onto remaining work gives a
finish date derived from behaviour rather than from planned durations. Its value is precisely that it
usually disagrees with the CPM date. The disagreement is the finding — see §6 — and it must be explained,
not averaged away.

**2.4 Scenario assembly.** Building and comparing what-if cases — a delayed permit, a resequenced area, an
additional crew — is mechanical work that a model can prepare quickly. The scenarios worth running, and the
one adopted, are chosen by people.

**2.5 Integration checks against cost and commitments.** Cross-checking that an activity showing progress
has cost against it, that a package with committed cost has activities in the network, and that the
schedule's remaining duration is consistent with the cost forecast's remaining work is tedious, valuable
and well suited to automation. It also catches the failure mode where the cost report and the programme
have drifted into telling different stories.

**2.6 Drafting the schedule narrative.** A first draft of the update commentary — what moved, what drove
it, what the mitigation is — saves time, with every date and float figure recomputed and every causal claim
confirmed before it is issued.

**2.7 Preparing risk inputs.** Proposing three-point ranges for activities from historical spread gives a
starting point for quantitative schedule risk analysis. The ranges are then owned by the people who own the
work; `AIG-06 — AI for risk identification and quantification` deals with the quantification discipline and
`BPG-17` with the analysis itself.

## 3. What must not be automated

Eight decisions. The general rule belongs to `AIG-10 — Human in the loop`; these are the scheduling
specifics.

**Accepting the critical path.** A tool reports a driving path; a planner establishes whether it is real by
tracing the logic and asking whether the sequence makes physical sense. A driving path produced by a
constraint, a calendar artefact or a missing link is a reporting artefact, not a plan.

**Baseline acceptance.** Accepting a baseline is a contractual and governance act with an approver. No
automated process may set, replace or amend a baseline, and no model may write to baseline fields.

**Inferring progress.** Percentage complete, actual start and actual finish are **measurements**, made
against rules of credit by someone accountable for the measurement. A model may draw attention to an
activity whose reported progress is inconsistent with its cost, its successors or its resource usage; it
may not set the value. Inferred progress is the fastest route to a programme that is confidently wrong.

**Inserting or adjusting constraints.** A constraint is a statement that something outside the network
fixes a date. Each one requires a justification recorded against it. Automated date-fixing — including
optimisation features that quietly apply constraints to achieve a target — destroys the network's meaning.

**Delay causation and EOT entitlement.** Which delay caused which effect, whether it is excusable, whether
it is compensable, and what notice was given are contractual determinations resting on facts and conduct.
Analysis tools assist; entitlement is decided by people, with legal input where the exposure warrants it.
`BPG-12 — Claims and extension of time` owns this ground.

**Choosing between mitigation and acceleration.** Re-sequencing within existing resources and spending
money to compress are different decisions with different commercial consequences and, often, different
contractual entitlements. A model may cost and compare the options; a person chooses.

**Resource levelling that moves dates.** Automatic levelling can produce a feasible-looking programme by
moving work in ways nobody has agreed. Levelling output is a proposal that must be inspected activity by
activity on the affected paths.

**Re-logicking to reach an acceptable date.** The most damaging automation of all is any process that
adjusts logic, durations or calendars until the finish date is the one required. When the date is wrong,
the answer is a mitigation plan or a claim, not a network that has been persuaded.

## 4. Validating an AI-produced schedule output

Seven checks, applied to any machine-produced schedule, health report or predicted date before it informs a
decision.

1. **Establish the status data.** Confirm the data date, and confirm that actual dates, remaining durations
   and percentage complete came from measurement rather than from a tool's default. Confirm how
   out-of-sequence progress is being handled — whether the calculation retains the original logic or lets
   progressed work override it — because the two settings can produce materially different finish dates from
   identical data.
2. **List and justify every constraint.** Not "count them" — list them, with the reason and owner for each.
   Anything unjustified is removed and the network re-run.
3. **Trace the driving path by hand.** Take the reported critical path and walk it backwards through the
   logic for a representative stretch. Confirm each link is a real dependency rather than a sequencing
   preference, and that no constraint or calendar is doing the driving.
4. **Reconcile the predicted finish with the CPM finish.** Where a trend-based prediction differs from the
   network date, quantify the difference and explain it in terms of durations and achieved rates. Do not
   average, and do not report the more comfortable of the two.
5. **Check the calendars.** Confirm activity calendars, holiday sets and shift patterns are the ones
   intended; calendar errors move dates invisibly and survive every other check.
6. **Work the exception list, do not count it.** A health report that says "26 open ends" and is filed has
   achieved nothing. Each exception is resolved, justified or accepted with a reason, and the network is
   re-run afterwards — the re-run is where the finding usually appears.
7. **Cross-check against cost and commitments.** Confirm that the programme's remaining work and the cost
   forecast's remaining work describe the same job; see `AIG-04 — AI-assisted cost forecasting` §4.

## 5. How this goes wrong

**The exception list becomes the deliverable.** A weekly automated health report circulates, the counts
trend downwards because the easy items get fixed, and nobody notices that the nine hard constraints have
been there since baseline. Counting is not checking.

**The checker is trusted on what it does not check.** Machine checks find structural defects — missing
links, constraints, negative float. They do not find a plausible-looking sequence that is physically
impossible, a duration that ignores a permit, or an omitted scope. A clean health report says the network
is well formed, not that the plan is sound.

**Progress is back-filled from cost.** Because an integration check flags activities with cost but no
progress, someone resolves the exception by updating progress to match the cost. The check was designed to
find measurement failures and has instead been used to manufacture measurements.

**Predicted dates are reported without their basis.** A trend-based finish date reported alongside the CPM
date, with no explanation of why they differ, invites the reader to pick one. The planner's job is to say
which is credible and why.

**Optimisation quietly fixes dates.** A scheduling assistant that "improves" a programme by applying
constraints or compressing durations produces a schedule that looks achievable and has stopped modelling
the work. Every change a tool proposes is inspected before acceptance.

**The delay analysis is done by the tool that produced the schedule.** Where the same automated process
both maintains the programme and analyses the delay, an error in the first is invisible to the second. Keep
the analysis independent, particularly where entitlement is in issue.

**Confidential programme data leaves the organisation.** A contractor's programme, resource loading and
sequencing intent are commercially sensitive. Uploading a submitted programme to an ungoverned tool for
"a quick check" is a confidentiality incident with contractual consequences.

## 6. Worked example — the health sweep and the hidden constraint

*Illustrative figures.* A 1,480-activity contractor programme is submitted for acceptance at month 24. A
machine health sweep is run before the planner's review. Working days throughout; a five-day working week
is assumed for calendar conversions.

**Step 1 — the sweep.**

| Finding | Count | Note |
|---|---|---|
| Open ends (no predecessor or no successor) | 26 | `26 ÷ 1,480 = 1.8 %` of activities |
| Hard date constraints | 9 | Each requires a justification |
| Lags longer than 10 days | 14 | Candidate substitutes for real activities |
| Activities with out-of-sequence progress | 63 | `63 ÷ 1,480 = 4.3 %` |
| Remaining duration exceeding original duration | 31 | Progress reported without duration reassessment |

**Step 2 — working the list.** The planner re-logics the 26 open ends, replaces 11 of the 14 long lags with
real activities and justifies the other 3, and reviews the 63 out-of-sequence activities against the actual
site sequence. Of the 9 constraints, 8 have recorded reasons; the ninth is a "finish on or before"
constraint on a commissioning activity, applied at a previous update with no note.

**Step 3 — the re-run.** With the unjustified constraint removed and the logic corrected, the programme's
completion moves from **working day 640 to working day 658**:

`658 − 640 = 18 working days`, which on a five-day week is `18 ÷ 5 × 7 = 25.2`, say **25 calendar days**

The slip was not created by the review. It existed and was being held out of the reported date by a
constraint nobody had justified.

**Step 4 — the trend prediction, reconciled.** The data date is working day 528, so the corrected CPM
leaves `658 − 528 = 130` working days remaining. Over the last eight update periods the programme has
achieved progress at approximately **0.90 of the planned rate**. Extrapolating that rate onto the remaining
work:

`130 ÷ 0.90 = 144 working days remaining`, giving a finish at `528 + 144 = working day 672`

`672 − 658 = 14 working days` beyond the corrected CPM date, and `672 − 640 = 32` working days beyond the
submitted date, or `32 ÷ 5 × 7 = 44.8`, say **45 calendar days**.

The two dates are not competing forecasts to be averaged. The CPM date says: *if the remaining durations
are achieved as planned, the finish is day 658.* The trend date says: *nothing in the last eight periods
suggests the remaining durations will be achieved as planned.* The planner's task is to decide which
remaining durations are credible, re-duration where they are not, and say so — or to produce a mitigation
plan that explains how the planned rate will now be achieved when it has not been.

**Step 5 — the commercial consequence, flagged not decided.** Assume for the example a liquidated-damages
(LD) rate of USD 25,000 per calendar day of delay. The constraint-concealed slip alone represents
`25,000 × 25 = USD 625,000` of potential exposure; on the trend-based date, `25,000 × 45 = USD 1,125,000`.

**These are exposure figures, not liabilities.** Whether damages apply at all depends on entitlement,
notices, concurrency and the contract's mechanism — determinations that belong to the commercial team and
`BPG-12 — Claims and extension of time`, not to the planner and certainly not to the checker. The purpose
of computing them is to make the priority of the finding legible to people who do not read programmes.

**Assumptions this answer depends on.** A five-day working calendar with no holiday effects in the affected
period; that the achieved-rate figure of 0.90 is measured over a period representative of the work
remaining; that the removed constraint had no contractual basis, which the planner confirms before removing
it; and that the LD rate is taken from the executed contract rather than assumed.

## 7. Checklist — before an AI-assisted schedule output is relied upon

1. **Data date and status confirmed**, with actual dates and remaining durations from measurement, not
   defaults.
2. **Out-of-sequence handling known** and its effect on the finish date understood.
3. **Every constraint listed with a justification and an owner**; the unjustified ones removed and the
   network re-run.
4. **Driving path traced by hand** for a representative stretch, and confirmed to be logic-driven.
5. **Calendars verified** against the intended working pattern and holiday set.
6. **Exception list worked to closure**, each item resolved, justified or accepted with a reason.
7. **Predicted and CPM dates reconciled**, with the difference explained in durations and achieved rates.
8. **Programme reconciled to cost and commitments** for the same remaining scope.
9. **No automated write-back** to baseline, progress, logic or constraints, and the tool's proposals
   inspected before acceptance.
10. **Named planner owns the issued programme**, with AI assistance recorded and the review changes noted.

---

## Related

- `AIG-04 — AI-assisted cost forecasting` — the cost half of the same forecast, and the integration check at
  §4.7.
- `AIG-06 — AI for risk identification and quantification` — where the three-point ranges of §2.7 are
  quantified, and the confidence discipline that governs them.
- `BPG-05 — Schedule quality — a practical review` — the check definitions this document automates.
- `BPG-12 — Claims and extension of time` — the entitlement ground §3 and §6 deliberately stop short of.
- `TPL-14 — Schedule quality review checklist` — the manual instrument behind the machine sweep.

## Sources and standards

- **PCI Body of Knowledge, Domain 10** — *Project scheduling* (Institute manuscript, 2026). The network
  fundamentals, float and health-check disciplines applied here.
- **PCI Body of Knowledge, Domain 13** — *AI for project controls and project management* (Institute
  manuscript, 2026), Knowledge Area 13.5.5, the scheduling workflow and its warning about hidden
  constraints and unrealistic durations in machine-assisted schedules.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
