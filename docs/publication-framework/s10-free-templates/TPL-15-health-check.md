---
id: TPL-15
series: S10
series_name: Free Templates
title: Project controls health check
subtitle: Eleven dimensions, evidence-based maturity levels, and an action plan with names against it
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [manager, executive, practitioner]
level: leader
reading_time_min: 18
summary: >
  A health check across eleven dimensions of a project controls function — structures and coding, baseline
  integrity, progress measurement, cost capture and cut-off, forecasting, change control, risk, reporting,
  systems and data, AI governance, and capability. Levels are awarded against defined descriptors and only
  where evidence was seen: absent evidence caps the level, whatever the interview said. The output is not a
  score but a ranked action plan with named owners and target levels.
linkedin:
  format: document
  hook: >
    Score a controls function only on what you were shown, not on what you were told. One rule — no
    evidence caps the level at 2 — changes a maturity assessment from a conversation about intent into a
    record of what exists.
  tags: [ProjectControls, Assurance, MaturityAssessment, GovernanceOfProjects, AIGovernance]
  asset: one-pager
gated: true
related: [BPG-19, BPG-01, AIG-08, TPL-16, BPG-14]
bok_domains: [4, 5, 11, 13]
sources: []
placeholders: 0
---

# Project controls health check

> Eleven dimensions, evidence-based maturity levels, and an action plan with names against it.

**In one paragraph.** A health check across eleven dimensions of a project controls function — structures
and coding, baseline integrity, progress measurement, cost capture and cut-off, forecasting, change control,
risk, reporting, systems and data, AI governance, and capability. Levels are awarded against defined
descriptors and only where evidence was seen: absent evidence caps the level, whatever the interview said.
The output is not a score but a ranked action plan with named owners and target levels.

**Who this is for.** Project controls managers and heads of function assessing their own operation; PMO
leads and assurance reviewers assessing someone else's; and the project directors and sponsors who
commission the review and have to fund the actions.

---

## 1. When to use this

**At mobilisation**, as a design specification rather than an assessment. Run it on the function you intend
to build, use the level descriptors to define what "done" means, and you have a set-up plan instead of a
wish list.

**At a stage gate or before a major commitment.** A sanction decision rests on numbers the controls function
produced. If the function cannot demonstrate how it produced them, the decision is resting on less than it
appears to be.

**When the numbers have started arguing with each other.** A forecast that moves without explanation, a cost
report that will not reconcile to the ledger, a schedule that nobody quotes — these are symptoms, and a
structured review across all eleven dimensions is a faster route to the cause than chasing each symptom.

**Annually on a long programme**, so that improvement is visible and reversal is caught.

Do not run it as an audit unless you are prepared to say so. A health check is a diagnostic done with the
function; an audit is done to it. Both are legitimate and they need different framing, different access and
different reporting. Say which one this is in the first line of the report.

## 2. How to complete it

### 2.1 Who runs it

Not the person who owns the control being assessed. Self-assessment has a place — it is fast, it builds
ownership, and it is a reasonable first pass — but its output is a view, not a finding, and the report must
say which it is. Where possible use a reviewer from outside the project who understands the discipline: an
insider who does not know controls will accept plausible answers, and an outsider who does not know the
project will misread deliberate choices as failures.

### 2.2 Ask for the evidence pack before you arrive

Request it in advance, by artefact and not by topic. The request itself is diagnostic — a function that can
produce all of it in two days is already telling you something, and so is one that cannot.

The pack: the work breakdown structure and cost breakdown structure with the mapping between them; the
approved baseline and the log of baseline changes; the rules of credit and the last three progress
assessments; the last three cost reports with their reconciliation to the ledger; the current forecast with
its basis of estimate; the change log; the risk register and the last risk review minutes; the report
distribution list and the last three reports issued; a list of systems with their owners and interfaces; any
record of where AI is used in the controls process; and the team structure with roles, names and the
competence held.

### 2.3 Award levels against the descriptors, and only on evidence

Five levels. They are deliberately coarse, and there are no half levels — a half level is how a review
avoids a conversation.

| Level | Name | Descriptor |
|---|---|---|
| 1 | Absent | The control does not exist, or exists only as one individual's habit |
| 2 | Ad hoc | It is done, inconsistently. It depends on who is present. It is not defined in writing |
| 3 | Defined | It is documented, owned and followed for the main scope. The gaps are known and named |
| 4 | Consistent | It is followed across the whole scope, evidenced, with checks that catch failures, and it survives the absence of any one person |
| 5 | Improving | As level 4, and the output is measured, challenged and used to change how the project is run. Deficiencies are found by the process itself, not by a review |

**The evidence rule: no evidence caps the level at 2.** If the reviewer has not seen the artefact named in
the evidence column, the dimension cannot be scored above 2, regardless of how convincingly it was
described. This single rule is what separates a health check from a conversation about intent, and it should
be stated to the function before the review starts so that nobody is ambushed by it.

Two consequences worth being ready for. Functions that do good work but document none of it will score
lower than their delivery deserves — and that is the correct result, because a control that lives in one
person's head fails the moment that person is on leave. And a reviewer will occasionally be shown an
artefact that exists but is not used; level 3 requires that it is followed, not that it exists, so ask for
the last three instances of it being applied.

### 2.4 Weight the dimensions, and set targets to risk rather than ambition

Equal weights are a defensible default. Where the project's risk profile is lopsided — a heavily
subcontracted job, a reimbursable contract, a first-of-a-kind design — weight accordingly and record the
reason. Set the weights **before** the assessment, so they cannot be tuned to the answer.

Target levels are a project decision, not an aspiration. Not every dimension needs to reach level 4 on every
project, and a target of 5 everywhere is a statement that nobody has thought about cost. Set the target
where the consequence of the control failing justifies the effort, and write the justification down.

If the review finds something that changes the weighting argument — an AI tool generating forecast
commentary that reaches the board unvalidated, on a dimension weighted at 0.5 — the reviewer should
recommend a change to the weight and say so explicitly in the report. What they must not do is quietly
re-weight to make the answer come out differently.

### 2.5 Write findings that name the effect

A finding with no effect is an opinion. Every one should state what is true, what it costs or risks, and
what the evidence for it was. "Progress is self-assessed by the delivery team without independent check" is
a fact. "Progress is self-assessed by the delivery team without independent check, so the earned value
underpinning the forecast is unverified, and the last three periods show progress claimed at over 98 % of
plan while cost ran at 106 % of plan" is a finding.

Show every finding to the person who owns the control before it is reported. Not for approval — for
correction. A review that is factually wrong on one point loses the argument on all of them.

## 3. The template

Two sheets: the assessment and the action plan.

### 3.1 Sheet 1 — `Assessment`

| Col | Field | Input or calculated | What goes in it |
|---|---|---|---|
| A | Dimension ID | Input | `HC-01` to `HC-11` |
| B | Dimension | Input | From §3.2 |
| C | What good looks like here | Input | The level-4 statement for this dimension, agreed before the review |
| D | Weight | Input | Set before the assessment, with the reason in column K |
| E | Assessed level | Input | 1 to 5, whole numbers only |
| F | Evidence seen | Input | The specific artefacts inspected, with dates and versions. Blank caps column E at 2 |
| G | Evidence not available | Input | What was asked for and not produced |
| H | Target level | Input | Set to the project's risk, with the justification in column K |
| I | Gap to target | Calculated | |
| J | Priority | Calculated | |
| K | Basis and notes | Input | The reason for the weight, the target and the level awarded |

| Col | Formula in words | Spreadsheet expression |
|---|---|---|
| I | Target level less assessed level; blank until both are entered | `=IF(OR($E2="",$H2=""),"",$H2-$E2)` |
| J | Gap multiplied by weight | `=IF(OR($I2="",$D2=""),"",$I2*$D2)` |

Summary cells:

| Field | Formula in words | Spreadsheet expression |
|---|---|---|
| Weighted score | The sum of level multiplied by weight, divided by the sum of the weights; blank if no weights are set | `=IF(SUM($D$2:$D$12)=0,"",SUMPRODUCT($E$2:$E$12,$D$2:$D$12)/SUM($D$2:$D$12))` |
| Unweighted mean | The average of the assessed levels; blank if none is set | `=IF(COUNT($E$2:$E$12)=0,"",AVERAGE($E$2:$E$12))` |
| Dimensions at level 2 or below | The count of dimensions assessed at 1 or 2 | `=COUNTIF($E$2:$E$12,"<=2")` |
| Dimensions capped by absent evidence | The count where a level above 2 was claimed but no evidence was recorded | `=COUNTIFS($F$2:$F$12,"",$E$2:$E$12,">2")` |

The last of those should always read zero in a completed review. If it does not, the evidence rule has been
broken and the report is not ready.

### 3.2 The eleven dimensions

**HC-01 Structures and coding.** Whether a work breakdown structure and a cost breakdown structure exist,
whether they map to each other and to the schedule and the ledger, whether the code of accounts is complete,
and whether new scope can be coded without a debate. *Evidence:* the structures themselves, the mapping, and
a sample transaction traced from the ledger to the control account to the activity.

**HC-02 Baseline integrity.** What is baselined — scope, cost, schedule — who approved it, whether every
change since is traceable to an approval, and whether the current baseline is the one being reported
against. *Evidence:* the approved baseline, the change log, and a reconciliation from the original to the
current.

**HC-03 Progress measurement.** Whether rules of credit exist, whether they are objective, who assesses
progress, whether that person is independent of the person whose progress it is, and whether the method
suits the work. *Evidence:* the rules of credit, the last three assessments, and a sample walked back to the
physical work.

**HC-04 Cost capture and cut-off.** Whether commitments are captured at the point of commitment, whether
accruals are made on a defined basis, whether the cut-off date is enforced, and whether the cost report
reconciles to the ledger without manual adjustment. *Evidence:* the cut-off calendar, the accrual basis, and
the last three reconciliations including the unreconciled differences.

**HC-05 Forecasting.** Whether the forecast has a stated method, whether the method suits the cause of the
variance, who owns the forecast, whether it is challenged by anyone, and whether previous forecasts are
compared against outturn. *Evidence:* the current forecast with its basis of estimate, the challenge record,
and the last three forecasts against what actually happened.

**HC-06 Change control.** Whether changes are identified early, whether notice obligations are tracked,
whether changes reach the cost forecast and the schedule, and whether anyone reconciles the change log to
the forecast. *Evidence:* the change log, and the reconciliation between unrecognised exposure and the
forecast — see `TPL-12 — Change order log`.

**HC-07 Risk.** Whether the register is used or merely maintained, whether risks have named owners who
attend, whether responses are completed, whether the register feeds quantification, and whether contingency
is linked to it. *Evidence:* the register, the last three review minutes, the contingency basis, and the
drawdown record.

**HC-08 Reporting.** Who the report is for, what decisions it has actually driven, how long it takes from
cut-off to issue, whether it says anything a reader could disagree with, and whether the numbers in it
reconcile to the systems they came from. *Evidence:* the last three reports, the distribution list, the
issue dates against the cut-off dates, and a decision traced back to the report that prompted it.

**HC-09 Systems and data.** What tools are in use, who owns each, how they interface, how much manual
re-keying happens between them, who has access to change what, and whether there is an audit trail.
*Evidence:* the system list with owners, the interface map, and a sample of a figure traced across systems.

**HC-10 AI governance.** Where AI is used in the controls process — drafting, classification, forecasting,
document review, anomaly detection — what it is permitted to decide and what it is not, how outputs are
validated before use, whether the human decision is recorded, whether the data going into it is permitted to
be there, and whether anyone could explain a given output to a reviewer. The Institute's position is that AI
proposes and the professional disposes: an output that reaches a decision must be explainable, validated and
owned by a competent human. *Evidence:* the record of where AI is in use, the validation records, and a
sample output traced to the human who accepted it.

**HC-11 Capability.** Whether the roles required by the other ten dimensions exist and are filled, whether
the people in them hold the competence the role needs, whether there is cover for absence, and whether
development is happening. Assess against a defined competency set rather than an impression —
`CMP-03 — PCL-AI: the fourteen competencies` is one such set. *Evidence:* the team structure with named
roles, the competence held against each, and the cover arrangements.

### 3.3 Sheet 2 — `Findings and actions`

| Col | Field | What goes in it |
|---|---|---|
| A | Finding ID | `F-01` onward |
| B | Dimension ID | Links to sheet 1 |
| C | Finding | What is true, stated as a fact |
| D | Evidence | What was inspected that establishes it |
| E | Effect | What it costs or risks, quantified where possible |
| F | Severity | High · Medium · Low, defined by effect and not by tidiness |
| G | Recommendation | What to do, specifically |
| H | Action owner | A named individual, not a function |
| I | Target level for the dimension | |
| J | Due date | |
| K | Status | Open · In progress · Complete · Accepted risk |
| L | Verification evidence | What was seen at re-verification |
| M | Date verified | |
| N | Owner acceptance | The control owner's response, including where they disagree |

Column N matters. A finding the owner disputes, recorded with their reason, is more useful than a finding
they were talked out of disputing — and if they turn out to be right, it is on the record that they said so.

### 3.4 Pasting it into a spreadsheet

Copy each header line into cell A1 of its own sheet and split on the pipe character.

```
Dimension ID|Dimension|What good looks like here|Weight|Assessed level|Evidence seen|Evidence not available|Target level|Gap to target|Priority|Basis and notes
```

```
Finding ID|Dimension ID|Finding|Evidence|Effect|Severity|Recommendation|Action owner|Target level|Due date|Status|Verification evidence|Date verified|Owner acceptance
```

## 4. Worked fragment

*Illustrative figures.* A complete assessment sheet, weights set before the review, target level 4 across
all eleven dimensions on this project's agreed basis.

| Dimension | Weight | Assessed level | Target | Gap | Priority |
|---|---|---|---|---|---|
| HC-01 Structures and coding | 1.0 | 4 | 4 | 0 | 0.0 |
| HC-02 Baseline integrity | 1.5 | 3 | 4 | 1 | 1.5 |
| HC-03 Progress measurement | 1.5 | 2 | 4 | 2 | 3.0 |
| HC-04 Cost capture and cut-off | 1.5 | 3 | 4 | 1 | 1.5 |
| HC-05 Forecasting | 1.5 | 2 | 4 | 2 | 3.0 |
| HC-06 Change control | 1.5 | 2 | 4 | 2 | 3.0 |
| HC-07 Risk | 1.0 | 3 | 4 | 1 | 1.0 |
| HC-08 Reporting | 1.0 | 3 | 4 | 1 | 1.0 |
| HC-09 Systems and data | 1.0 | 2 | 4 | 2 | 2.0 |
| HC-10 AI governance | 0.5 | 1 | 4 | 3 | 1.5 |
| HC-11 Capability | 1.0 | 3 | 4 | 1 | 1.0 |

**The substitutions.** Sum of weights:
`1.0 + 1.5 + 1.5 + 1.5 + 1.5 + 1.5 + 1.0 + 1.0 + 1.0 + 0.5 + 1.0 = 13.0`.

Sum of level multiplied by weight:
`(1.0×4) + (1.5×3) + (1.5×2) + (1.5×3) + (1.5×2) + (1.5×2) + (1.0×3) + (1.0×3) + (1.0×2) + (0.5×1) + (1.0×3)`
`= 4 + 4.5 + 3 + 4.5 + 3 + 3 + 3 + 3 + 2 + 0.5 + 3 = 33.5`.

Weighted score: `33.5 ÷ 13.0 = 2.58` to two decimal places. Unweighted mean:
`28 ÷ 11 = 2.55`. Dimensions at level 2 or below: HC-03, HC-05, HC-06, HC-09 and HC-10, so `5`.

**How to read this.** The weighted score of 2.58 is the least useful number on the page, and it is the one
that will be quoted. The report is the priority column: HC-03 progress measurement, HC-05 forecasting and
HC-06 change control all sit at 3.0, and they are not three separate problems. You cannot forecast what you
cannot measure, and you cannot forecast what change control has not told you about. Progress measurement is
the one to fix first, because the other two consume its output. That sentence — not the average — is the
finding.

HC-10 deserves a second look for the opposite reason. It has the largest raw gap on the sheet, three levels,
and the lowest priority score, because the weight of 0.5 was set before the review. If the review found AI
being used to draft forecast commentary that reaches the board without a recorded human validation, then the
weight is wrong, and the honest response is to say so in the report and recommend re-weighting for the next
cycle — not to quietly change the number now and present a different score.

The two averages — 2.58 weighted and 2.55 unweighted — being almost identical is itself informative: the
weighting is doing very little work on this profile, which means the argument about weights can be short and
the argument about progress measurement can be long.

## 5. Common mistakes

**Scoring the intent rather than the artefact.** The most common failure, and the reason the evidence rule
exists. A well-run interview will produce a description of a level-4 process that is not happening.

**Half levels.** A 3.5 is a way of not deciding. It also destroys the arithmetic's meaning, because the
descriptors do not have midpoints.

**Reporting the average.** A function scoring 2.58 with one dimension at 1 has a specific problem, and the
average is designed not to show it. Report the count at level 2 or below and the priority ranking; put the
average in an appendix if it must appear at all.

**Weights set after the assessment.** Once the levels are known, weighting becomes score management. Set
them first and record the date.

**Targets of 5 everywhere.** A target is a commitment to spend effort. Setting every target at the top says
either that the reviewer has not costed the recommendations or that nobody intends to act on them.

**No named owner.** "The PMO" cannot be chased. Column H takes a person.

**Findings without effects.** "The rules of credit are not documented" invites a shrug. "The rules of credit
are not documented, three assessors are applying different bases to the same work type, and the resulting
earned value feeds a forecast that has moved twice this quarter without explanation" invites a decision.

**No re-verification.** A health check with no verification column becomes an annual event that produces the
same findings each year with different dates. Columns L and M are what make it a control rather than a
report.

**AI governance assessed as a technology question.** HC-10 is not about which tools are licensed. It is
about what a machine is permitted to decide, who validated the output that reached a decision, and whether
that validation is recorded. A function with no AI in use scores this dimension as such and explains how it
knows — which requires having asked, because the answer is frequently that AI is in use and nobody had been
asked.

## 6. Adapting it

**Safe to change.** The weights, the targets, the dimension names, and the addition of dimensions your
organisation needs — earned value specifically, interface management, subcontractor controls, benefits
tracking, health and safety reporting. If a dimension does not apply, mark it not applicable with a reason
rather than scoring it low.

**Safe to add.** A trend column carrying the previous cycle's level, which turns the instrument into a
control. A cost-to-remedy estimate against each recommendation, which makes the action plan fundable. A
split of the assessment by project within a portfolio, which usually reveals that the function is not the
problem and one project is.

**Do not change.** The evidence rule. The whole-number levels. The requirement that the action owner is a
named individual. And the practice of showing every finding to the control owner before reporting it — that
one costs a day and buys the credibility of the whole exercise.

### 6.1 Before the report is issued

- The report states whether this was a health check or an audit, and who ran it.
- Weights and targets were set before the assessment, with dates recorded.
- Every dimension has a level, and every level above 2 has an evidence entry — the capped-by-absent-evidence
  count reads zero.
- Everything requested and not produced is recorded in column G, not omitted.
- Every finding states an effect, quantified where the data allowed.
- Every finding has been shown to the control owner, and their response is recorded, including disagreement.
- Every action has a named individual, a target level and a date.
- The priority ranking, not the average, leads the report.
- Any recommendation to change a weight for the next cycle is stated openly rather than applied silently.
- A re-verification date is in the diary before the report is circulated.

---

## Related

- `BPG-19 — Project controls assurance and health checks` — the method behind this instrument, including how
  to run the review and how to report it without losing the room
- `BPG-01 — Building a project controls function from zero` — the level-4 descriptions used as the
  specification when this template is run at mobilisation
- `AIG-08 — Governing AI on a project — the control framework` — what HC-10 is assessing against
- `TPL-16 — Lessons learned and closeout register` — where the findings go when the project ends, and the
  route by which they change the organisation rather than the project
- `BPG-14 — Monthly reporting that gets read` — the standard HC-08 assesses reporting against

## Sources and standards

This is an original instrument developed by the Institute. It reproduces no third-party maturity model,
assessment framework, questionnaire or scoring scheme. The five level descriptors, the eleven dimensions and
the evidence rule are the Institute's own formulation, offered for adaptation rather than as a standard. The
competency set referenced in HC-11 is the Institute's own, published in `CMP-03`. Where an organisation is
required to assess against a specific published maturity model, that model governs and this template is a
supplement to it, not a substitute.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
