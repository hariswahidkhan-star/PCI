---
id: TPL-01
series: S10
series_name: Free Templates
title: Project controls execution plan
subtitle: The plan that says which system is the source of truth for each field, and who is accountable when two systems disagree
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 16
summary: >
  A project controls execution plan states how a specific project will be measured: which structures
  exist, what the baseline is, how progress is claimed and verified, what gets reported to whom and
  when, how change is controlled, which system is authoritative for each field, and where artificial
  intelligence may and may not touch the numbers. This template gives the section structure and, under
  every heading, the prompts you must answer — not prose to be adapted, but questions that expose the
  decisions most plans leave unmade.
linkedin:
  format: document
  hook: >
    Most project controls execution plans describe what the function will do. The useful ones state
    which system is the source of truth for each field, and who is accountable when two systems
    disagree.
  tags: [ProjectControls, ProjectGovernance, CostEngineering, PMO, ResponsibleAI]
  asset: one-pager
gated: false
related: [TPL-02, TPL-05, TPL-06, BPG-01, BPG-19, AIG-08, TPL-15]
bok_domains: [5, 8, 12, 13]
sources:
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), verified August 2026"
placeholders: 0
---

# Project controls execution plan

> A completed statement of how one project will be measured, reported and controlled.

**In one paragraph.** A project controls execution plan states how a specific project will be measured:
which structures exist, what the baseline is, how progress is claimed and verified, what gets reported to
whom and when, how change is controlled, which system is authoritative for each field, and where
artificial intelligence may and may not touch the numbers. This template gives the section structure and,
under every heading, the prompts you must answer — not prose to be adapted, but questions that expose the
decisions most plans leave unmade.

**Who this is for.** Project controls managers and heads of project management office mobilising a
project or a portfolio; cost engineers and planners inheriting a project with no written controls basis;
and the project directors who will be asked to approve the plan.

---

## 1. When to use this

Write the plan during mobilisation, before the first baseline is approved and before the first report is
issued. The plan is the thing the baseline and the reports are consistent *with*; producing it afterwards
turns it into a description of whatever habits formed in the first two months.

Three other moments justify writing or rewriting one:

- **On taking over a project that has none.** The absence is itself the finding. Write the plan by
  documenting current practice honestly, marking every place where practice and intention differ, and
  presenting that gap list as the mobilisation backlog.
- **At a re-baseline.** A re-baseline changes what is measured against what. If the plan is not updated
  in the same approval, the next month's variances will be computed against a basis nobody has agreed.
- **When the delivery model changes** — a joint venture forms, a major package moves from remeasurable
  to lump sum, a second reporting entity appears. Each changes the source of truth for whole fields.

Do not use this template as a bid deliverable dressed as a plan. A controls execution plan that is
written to be scored rather than to be followed is worse than none, because it creates a documented
standard the project is then measured against and cannot meet.

## 2. How to complete it

**Answer the prompts; do not adapt the prose.** Every heading in §3 carries prompts rather than model
text, because model text invites paraphrase and paraphrase hides the decisions. A prompt you cannot
answer is a live issue for the mobilisation log, and writing "to be advised" against it is a legitimate
and useful outcome — far better than a sentence that sounds settled and is not.

**Name four things in every answer.** The framework this series is written to treats detail as
specificity: the artefact, the field, the frequency and the owner. "Progress is reviewed regularly" fails
all four. "The control account manager submits the progress claim in the progress measurement sheet
(`TPL-05`) by 12:00 on the second working day after cut-off; the cost engineer verifies the evidence
reference on every line above 500 budget hours" passes.

**Fill the definitions section first (§3.2).** Most arguments in a controls function are definitional, not
arithmetical. Two people disagreeing about the cost variance almost always agree about subtraction and
disagree about whether an accrual is in the actual cost. Settle those in writing before the first report.

**Decide the source of truth per field, not per system.** Systems overlap. The useful statement is
field-level: commitment value is authoritative in the procurement system; actual cost is authoritative in
the finance ledger; earned value is authoritative in the controls workbook; forecast completion date is
authoritative in the schedule tool. Then state what happens when two disagree — who reconciles, on what
cycle, and what tolerance is accepted without escalation.

**Get it approved by the person who will be asked to overrule it.** A plan approved only inside the
controls function has no authority in the month someone senior wants a number changed.

**Using the tables.** The tables in §3 are pipe-delimited Markdown. To move one into a spreadsheet, copy
the block, paste it into a single column, split on the pipe character, and delete the alignment row.

## 3. The template

Copy from here. Numbering is designed for citation — a change request can reference "PCEP §3.5" and mean
exactly one clause.

### 3.0 Document control

| Field | Entry |
|---|---|
| Project / contract | |
| Plan version and date | |
| Prepared by (name, role) | |
| Reviewed by (name, role) | |
| Approved by (name, role, date) | |
| Supersedes | |
| Distribution | |
| Classification | |
| Next review trigger | |

**Prompts.** What event — not what date — triggers the next review? Which of the client's own controls
requirements does this plan sit beneath, and where are the two inconsistent?

### 3.1 Purpose and scope of the controls function

**Prompts.** Which contracts, packages, entities and joint-venture interests does this plan govern? What
is explicitly outside it, and who controls those? Does the plan bind subcontractors, and through which
contractual clause? Where the client has its own controls procedure, which prevails, and who has
confirmed that in writing?

### 3.2 Definitions used on this project

**Prompts.** Define each of the following once, here, with the arithmetic where arithmetic applies:
budget at completion (does it include the contingency reserve, or not — the choice governs every index
reported in `TPL-07`); management reserve and who holds it; commitment; accrual; actual cost; per cent
complete; earned value;
milestone; float and who owns it; "approved change"; "authorised to proceed". For each, name the system
that holds the value.

### 3.3 Structures and coding

**Prompts.** How many levels does the work breakdown structure have and at which level is the control
account set? What is the sizing rule for a work package — by budget, by duration, or by both? Who owns
the organisational breakdown structure and how is it mapped to the work breakdown structure? What is the
cost breakdown structure and code of accounts, and how does it map to the general ledger? What is the
resource breakdown structure? Who may create a new code, and against what test? Where are the structures
published, and how is a change to a structure controlled? See `TPL-02` and `TPL-03`.

### 3.4 Baseline

**Prompts.** What documents constitute the baseline, at which version, frozen on what date? What is the
performance measurement baseline and what sits outside it? Where is the contingency reserve held, who has
custody, and what is the release test? Where is management reserve held and who authorises its release?
Which schedule is the baseline schedule and how is it identified in the tool? What is measured against
what — is a variance computed against the original baseline, the current approved baseline, or both, and
in which report?

### 3.5 Change control

**Prompts.** What are the approval thresholds and who sits at each? What is the routing and the target
turnaround at each stage? Which form is used and where does it live? Who maintains the register? What
triggers a re-baseline and what is expressly forbidden as a reason for one? How are pending changes
treated in the forecast — excluded, disclosed separately, or included with probability weighting? Who may
draw on contingency, and what evidence must accompany the request? See `TPL-04`.

### 3.6 Progress measurement

**Prompts.** Which measurement techniques are permitted on this project, and which are prohibited? Where
is the rule-of-credit library and who approves an entry in it? Who claims progress and who verifies it?
What objective evidence is required at each credit step, and where is it filed? What is the cut-off
calendar? How is level of effort treated and capped? What happens when a claimed quantity exceeds the
budgeted quantity? See `TPL-05`.

### 3.7 Cost management

**Prompts.** What are the sources for commitments, actual costs and accruals, and what is the cut-off for
each? What is the timesheet discipline and who enforces it? Who reconciles the controls workbook to the
finance ledger, on what cycle, and what difference is tolerated without escalation? How are inter-company
and joint-venture charges captured? How are currency and escalation handled, and on what basis are
figures reported — nominal or constant prices?

### 3.8 Schedule management

**Prompts.** What schedule levels exist and who owns each? What is the update cycle and the statusing
rule — actual dates only, or remaining duration overrides? What controls a change to logic or to
calendars? Who owns float, and is it consumed first-come or allocated? What schedule quality check is
run, by whom, and on what cadence? See `TPL-14`.

### 3.9 Forecasting

**Prompts.** Which estimate at completion methods are permitted, and who selects? Who prepares the
forecast, who challenges it, and at what forum? How often is a bottom-up estimate to complete required
rather than an index-based extrapolation? How is the selected method recorded and how is a change of
method disclosed? See `TPL-08`.

### 3.10 Risk process

**Prompts.** Who owns the risk register and on what cadence is it reviewed? What are the probability and
impact scales, and are they defined in absolute terms rather than adjectives? What triggers a quantitative
schedule or cost risk analysis? How does risk exposure connect to the contingency position, and who makes
that connection in writing? See `TPL-10` and `TPL-11`.

### 3.11 Reporting cadence and distribution

| Report | Data date | Issue offset (working days) | Prepared by | Approved by | Distribution | Classification |
|---|---|---|---|---|---|---|
| | | | | | | |

**Calculated column — issue date.** In words: the issue date is the data date advanced by the stated
number of working days, excluding weekends and the project holiday calendar. Spreadsheet, with the data
date in `B2`, the offset in `C2` and a named range `Holidays`:

```
=IF(OR(B2="",C2=""),"",WORKDAY(B2,C2,Holidays))
```

**Calculated column — working days actually taken.** In words: the count of working days between the data
date and the actual issue date, excluding the data date itself. With the actual issue date in `E2`:

```
=IF(OR(B2="",E2=""),"",NETWORKDAYS(B2,E2,Holidays)-1)
```

`NETWORKDAYS` counts both end dates, so subtract one to express elapsed working days.

**Prompts.** What is on each distribution list, by name and role rather than by team? What is the
escalation route when a report is late, and at what point does lateness itself get reported? Which
report is the single authoritative statement of position, so that a figure quoted from anywhere else is
known to be indicative?

### 3.12 Systems and data

| Data field | System of record | Owner | Update frequency | Feeds | Reconciled to | Tolerance |
|---|---|---|---|---|---|---|
| | | | | | | |

**Prompts.** For every field in the monthly report, which system is authoritative? Which integrations are
automated and which are manual re-keying — and where a field is re-keyed, who checks it? What are the
access levels and who approves them? What is the backup and archive arrangement, and who has tested a
restore? Which data quality checks run, on what cadence, and who sees the exceptions?

### 3.13 Artificial intelligence: use and governance

The Institute's position is that artificial intelligence proposes and the professional disposes. Anything
that reaches a report must be explainable, validated and owned by a competent human. This section makes
that operational on one project.

| Task | AI permitted? | Tool and version | Human validation gate | Validator (role) | Evidence retained | Data classification permitted |
|---|---|---|---|---|---|---|
| | | | | | | |

**Prompts.** Which controls tasks may be AI-assisted — narrative drafting, anomaly detection in cost
ledgers, schedule logic screening, document search, first-pass risk identification? Which are prohibited
outright, and why? For each permitted task, who is the named human who validates the output before it
leaves the controls function, and what does validation consist of? What project data may be sent to a
model hosted outside the project's own environment, and what may not? How is an AI-assisted output
labelled in the report, so a reader knows what they are reading? What evidence is retained so the
contribution of a model to a reported number can be reconstructed at audit? Who is notified when a tool's
model version changes, and does the validation gate change with it? On what cadence is this register
reviewed?

### 3.14 Roles, responsibilities and RACI

| Activity | Controls manager | Cost engineer | Planner | Control account manager | Project director | Finance |
|---|---|---|---|---|---|---|
| | | | | | | |

Use R (responsible), A (accountable), C (consulted), I (informed). Exactly one A per row.

**Calculated column — accountability check.** In words: flag any activity row that does not carry exactly
one accountable party. With the role columns in `B2:G2`:

```
=IF(COUNTIF(B2:G2,"A")=1,"OK","CHECK — one A per row")
```

**Prompts.** What are the delegation limits in value and in kind? What are the cover arrangements when the
accountable person is unavailable, and are they named? Which decisions may the controls function make and
which may it only recommend?

### 3.15 Interfaces

**Prompts.** Which internal functions does controls depend on for data, and what does each owe by when —
finance, procurement, engineering, construction, commercial, human resources? Which external parties, and
under which contractual reporting obligation? Where is the interface register? What is the agreed data
exchange format and calendar for each? What happens when an input is late — does the report slip, or
does it issue with a stated gap?

### 3.16 Assurance

**Prompts.** What does the function check on itself, how often, and who reviews the result? When is an
independent review carried out and by whom? What is the health-check instrument and its cadence — see
`TPL-15`. What audit trail exists for a number that changed between two reports?

### 3.17 Mobilisation and closeout

**Prompts.** What must exist by which date for the function to be operating — structures approved,
baseline loaded, rules of credit signed off, first report issued? At closeout, what data is handed back,
in what format, to whom, and by when? What is captured for benchmarking and lessons learned, and who
owns that after the project team disperses? See `TPL-16`.

### 3.18 Appendices

Cut-off and reporting calendar · distribution matrix · forms and templates index · glossary of terms used
on this project · list of open items from §3.2 onwards.

## 4. Worked fragment

*Illustrative figures.* An extract from a completed §3.11 and §3.14 for a fictional facility upgrade
project. Dates use the calendar for 2026; the holiday calendar is assumed empty across the range, and the
week is Monday to Friday.

**§3.11 Reporting cadence — extract**

| Report | Data date | Issue offset (working days) | Prepared by | Approved by | Distribution | Classification |
|---|---|---|---|---|---|---|
| Weekly progress flash | Friday | +1 | Planner | Controls manager | Project delivery team | Internal |
| Monthly project controls report | Last calendar day of month | +6 | Controls manager | Project director | Client project manager, sponsor, function heads | Confidential |
| Cost-to-ledger reconciliation | Finance month-end close | +8 | Cost engineer | Project accountant | Controls manager, finance business partner | Internal |
| Quarterly forecast review pack | Quarter end | +10 | Controls manager | Project director and sponsor | Steering group | Confidential |

Worked issue date for the monthly report at the May cut-off: the data date is Sunday 31 May 2026; six
working days advance through Monday 1, Tuesday 2, Wednesday 3, Thursday 4 and Friday 5 June, then
Monday 8 June. `=WORKDAY(DATE(2026,5,31),6)` returns **Monday 8 June 2026**. The May report in `TPL-06`
carries that issue date.

**§3.14 RACI — extract**

| Activity | Controls manager | Cost engineer | Planner | Control account manager | Project director | Finance | Check |
|---|---|---|---|---|---|---|---|
| Submit monthly progress claim | C | C | I | **A** / R | I | — | OK |
| Verify progress evidence | **A** | R | C | I | — | — | OK |
| Post accruals at cut-off | I | R | — | C | — | **A** | OK |
| Approve a change up to 250 budget units | C | R | C | I | **A** | I | OK |
| Select the estimate at completion method | **A** | R | C | C | C | I | OK |

The check column applies the formula in §3.14 and returns `OK` because each row carries exactly one A.
The row "post accruals at cut-off" is the one worth arguing about: accountability sits with finance
because the ledger is the system of record for actual cost under §3.12, while the cost engineer does the
work. Writing it the other way round is the most common source of a month-end reconciliation that never
closes.

## 5. Common mistakes

**Describing the discipline instead of the project.** A section that explains what earned value is has
been copied from a textbook. The plan is not a teaching document; it says what *this* project does, at
which level, with which technique, verified by whom.

**Leaving budget at completion undefined.** Whether it includes the contingency reserve changes every
variance, every index and every forecast in every report. Two organisations can both be right and the
joint venture between them will publish two different cost performance indices for the same month.

**A source-of-truth table by system rather than by field.** "Cost is in the finance system, schedule is in
the planning tool" leaves commitments, accruals, forecast dates and earned value unallocated — and those
are precisely the fields that get argued about.

**RACI with two accountable parties, or with none.** A row with two As is a row where nobody decides. The
check formula in §3.14 exists because this is very common and invisible at a glance.

**A reporting calendar with no offsets.** "Monthly report issued in the first week" is not a commitment
anyone can plan around, and it silently permits the data date to drift towards the issue date until the
report describes a position nobody recognises.

**An artificial intelligence section that lists principles.** "AI will be used responsibly" is not a
control. The control is a named task, a named validator, a stated data classification and a retained
evidence trail. If §3.13 could be pasted into any other project's plan unchanged, it does nothing.

**Approval by the controls function only.** The plan's purpose is to be quotable when someone senior asks
for a number to be presented differently. That requires a signature from someone senior.

## 6. Adapting it

**Safe to change.** Section order, provided cross-references are updated. The role names, to match your
organisation. The number of reports and their cadence. The approval thresholds, which are project-specific
by nature. Adding sections — contract administration, document control, materials management,
sustainability reporting — where the project needs them.

**Change with care.** Merging §3.12 (systems and data) into §3.3 (structures) loses the field-level
source-of-truth statement, which is the section most often cited later. Merging §3.5 (change control) into
§3.4 (baseline) tends to lose the routing and threshold detail.

**Do not remove.** The definitions section (§3.2), the field-level system of record table (§3.12), the
accountability table (§3.14) and the artificial intelligence register (§3.13). Those four are what make
the document a control rather than a description. A plan without them will read well and settle nothing.

**On a small project**, keep every heading and let the answers be short. A one-line answer under a heading
is a decision recorded. A deleted heading is a decision not taken, and it will be taken later by whoever
is in the room.

## 7. Completion checklist

- [ ] Every prompt in §3 has an answer or a dated open item with an owner
- [ ] Budget at completion, actual cost, accrual and per cent complete are defined with their arithmetic
- [ ] Every field in the monthly report appears in the §3.12 system-of-record table
- [ ] Reconciliation cycle and tolerance stated, with a named reconciler
- [ ] Every RACI row returns `OK` on the one-accountable check
- [ ] Every permitted AI task has a named human validator and a stated data classification
- [ ] Prohibited AI uses stated explicitly, not implied by omission
- [ ] Reporting calendar has data dates, working-day offsets and named approvers
- [ ] Contingency and management reserve custody and release tests stated
- [ ] Change thresholds and routing agreed by the approvers named in them
- [ ] Approved by the project director or equivalent, not only within the controls function
- [ ] Review trigger stated as an event, not a date

---

## Related

- `TPL-02 — Work breakdown structure and WBS dictionary` — the structure §3.3 requires you to define
- `TPL-05 — Progress measurement and rules of credit sheet` — the instrument that delivers §3.6
- `TPL-06 — Monthly project controls report` — the output §3.11 schedules
- `TPL-15 — Project controls health check` — the assurance instrument referenced at §3.16
- `BPG-01 — Building a project controls function from zero` — the reasoning behind the sequence of §3
- `BPG-19 — Project controls assurance and health checks` — how to test whether the plan is being followed
- `AIG-08 — Governing AI on a project — the control framework` — the full treatment behind §3.13

## Sources and standards

- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified
  August 2026: the Institute's position on responsible, governed use of artificial intelligence, from
  which §3.13 takes its validation-gate structure.

This template is an original instrument. Where the section structure resembles established practice, that
is because the underlying activities — baselining, change control, progress measurement, reporting — are
common to the discipline. No third-party plan, form or checklist is reproduced, and no standard is quoted.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
