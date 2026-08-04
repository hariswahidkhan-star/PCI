---
id: SAL-03
series: S08
series_name: Salary and Skills Report
title: Role taxonomy and levelling
subtitle: Ten canonical roles and a rubric that makes "senior" mean the same thing twice
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [employer, manager, practitioner, academic]
level: professional
reading_time_min: 15
summary: >
  Ten canonical project controls roles, each defined by what it is accountable for rather than what it is
  called, and a four-level rubric that levels a person on autonomy, scope, judgement and accountability
  instead of tenure or title. Together they are the classification the Institute's salary and skills
  survey uses, the aggregation hierarchy that governs how thin data may be combined, and a usable job
  architecture for any organisation that has never written one down.
linkedin:
  format: document
  hook: >
    "Senior planner" means two different jobs at two employers on the same site. Until you level people on
    what they are accountable for, every pay comparison you make is comparing labels.
  tags: [ProjectControls, JobArchitecture, Levelling, Competency]
  asset: carousel-8
gated: false
related: [SAL-02, SAL-04, SAL-05, CMP-01, CMP-02, CAR-01]
bok_domains: [4, 5, 6, 10, 12]
sources: []
placeholders: 3
---

# Role taxonomy and levelling

> Ten canonical roles, defined by accountability, and a rubric that levels people on what they own rather than what they are called.

**In one paragraph.** This document defines ten canonical project controls roles, each by what it is
accountable for rather than what it is called, and a four-level rubric that levels a person on autonomy,
scope, judgement and accountability instead of tenure or title. Together they are the classification the
Institute's salary and skills survey uses, the aggregation hierarchy that governs how thin data may be
combined, and a usable job architecture for any organisation that has never written one down.

**Who this is for.** Heads of project controls and reward specialists doing job matching or building a
career structure; practitioners working out which role and level genuinely describes them; and the
analysts coding survey responses.

---

## 1. Why titles cannot be the unit of comparison

On a single site it is normal to find a "senior planner" who owns the integrated programme for a
multi-contractor scope, and a "planning manager" who maintains one contractor's schedule and reports it
upward. Both titles are honest inside their own organisations. Compared with each other, they are noise.

Titles inflate over time, deflate under headcount pressure, absorb local convention, and are frequently
used as a substitute for pay. They are also the only classification most market data uses — the single
largest reason published pay figures for this profession are unreliable. The cell labelled "planner"
contains four different jobs.

The alternative is not complicated. Classify on two axes that describe the work rather than the label:
**what the person is accountable for** (the role) and **how much judgement and scope they carry** (the
level). A cost engineer at practitioner level in a contractor organisation is a recognisable, comparable
thing in Perth, Riyadh and Rotterdam. "Senior cost engineer" is not.

## 2. How to use this taxonomy

**In the survey.** Question Q07 in `SAL-02` asks for the role by what the respondent spends most of their
time doing; Q09 asks for the level using the behavioural descriptors in §4 below; Q06 captures the
employer's title as free text and is never used as the classification variable. Coding rules are in §8.

**In an organisation.** The ten roles and four levels are a job architecture you may adopt directly. They
are deliberately fewer than most internal structures, because a taxonomy with forty roles cannot be
applied consistently by the twelve people who have to apply it.

**What this is not.** Not a pay structure, and it carries no pay implication of any kind. Not a statement
that every organisation needs all ten roles — most need three or four. And not a competency framework:
competence is defined in the Institute's competency series, which this document maps to rather than
duplicates (see §7).

`[CONFIRM: whether this taxonomy is ratified by the Standards Committee as the Institute's published job
architecture, or published as a research instrument only]`

## 3. The ten canonical roles

Each definition states the role's purpose, what it is accountable for, the artefacts it owns, the decision
it is trusted with, and the boundary that most often gets blurred. The Body of Knowledge domains are
given by number, per `docs/bok/`.

### 3.1 Planner / scheduler

**Purpose.** To construct and maintain the model of how the work will be sequenced in time, and to tell
the truth about where it currently is against that model.

**Accountable for.** Schedule structure and logic; the baseline and its integrity; progress capture and
status; critical path and float analysis; time impact of change.
**Owns.** The programme or schedule file, the schedule basis and assumptions document, the progress
report, the schedule narrative.
**Trusted to decide.** How the work is logically represented, and what the schedule says about completion
— including when that is unwelcome.
**Boundary.** A planner is not a progress clerk. Where the role is only updating percentages supplied by
others, that is a foundation-level activity within the role, not the role itself.
**BoK domains.** 10 (scheduling), 8 (lifecycle), 6 (earned value forecasting), 12 (risk).

### 3.2 Cost engineer

**Purpose.** To connect scope, quantity and money — building the cost model and keeping it consistent with
what is actually being built.

**Accountable for.** The cost breakdown structure and its alignment to the work breakdown structure;
budgets and commitments at control-account level; unit rates and quantities; cost impact of change; the
data behind the forecast.
**Owns.** The cost model, the control-account budget, the variance analysis, the cost basis of estimate at
completion (EAC).
**Trusted to decide.** How cost is structured and how a variance is explained.
**Boundary.** The cost engineer builds and analyses; the cost controller runs the periodic control cycle.
In smaller teams one person does both, and in the survey they pick the larger half of their time.
**BoK domains.** 5 (cost management), 3 (budgeting and forecasting), 6 (EVM/EAC), 1 (accounting
foundations).

### 3.3 Cost controller

**Purpose.** To operate the monthly control cycle so that reported cost is complete, cut off correctly and
reconciled to the accounts.

**Accountable for.** Commitment and expenditure capture; accruals and cut-off; reconciliation between the
project ledger and the finance system; invoice and payment status; the integrity of the reported actuals.
**Owns.** The cost report, the accrual schedule, the reconciliation, the cost ledger interface.
**Trusted to decide.** What is accrued, what is deferred, and when the books close.
**Boundary.** Not project accounting, and not management accounting. The cost controller is accountable
for the project's numbers being right and reconcilable, not for the entity's financial statements.
**BoK domains.** 5, 4 (performance management and management reporting), 11 (business process cycles),
2 (financial reporting).

### 3.4 Estimator

**Purpose.** To produce a defensible view of what work will cost before it is done, with the uncertainty
stated.

**Accountable for.** Estimate structure, methodology and class; quantity take-off and pricing basis;
benchmark and norm selection; allowances and the estimate's uncertainty range; the basis of estimate.
**Owns.** The estimate, the basis of estimate, the estimate reconciliation between revisions.
**Trusted to decide.** Method and class of estimate, and what is included in an allowance rather than a
contingency.
**Boundary.** The estimator sets the number the project starts with; the cost engineer controls it
thereafter. Where they are the same person the roles must still be distinguished, because estimating your
own control baseline is a governance weakness worth knowing about.
**BoK domains.** 3, 5, 7 (contracts and commercial).

### 3.5 Risk analyst

**Purpose.** To make uncertainty explicit and quantified, so that decisions account for it rather than
discovering it.

**Accountable for.** Risk identification and register quality; qualitative assessment discipline;
quantitative cost and schedule risk analysis; contingency derivation and drawdown analysis; the link
between risk and the forecast.
**Owns.** The risk register, the quantitative schedule risk analysis (QSRA) model and its inputs, the
contingency basis.
**Trusted to decide.** How uncertainty is modelled, and what the analysis says about confidence in a
target date or cost.
**Boundary.** Not the risk owner. The analyst runs the process and the arithmetic; the accountable manager
owns the risk and the response.
**BoK domains.** 12 (risk), 10, 6.

### 3.6 Change / commercial controller

**Purpose.** To control the boundary between what was agreed and what is now being done, in both
directions.

**Accountable for.** Change identification, valuation and authorisation; variation and instruction
records; entitlement and notice discipline; claim and extension-of-time (EOT) substantiation; the
commercial audit trail.
**Owns.** The change register, the variation file, the claim narrative and its supporting records.
**Trusted to decide.** Whether something is a change, and what evidence is required before it is valued.
**Boundary.** Sits between project controls and the commercial or quantity surveying function, and in some
organisations reports into the latter. The role is included here because the work is controls work
wherever it reports.
**BoK domains.** 7, 11, 5.

### 3.7 Reporting and data analyst

**Purpose.** To turn controls data into something a decision-maker can act on, without distorting it in
transit.

**Accountable for.** Data model and data quality across controls systems; the reporting pipeline;
dashboards and their definitions; automation of recurring reporting; increasingly, the responsible
application of analytical and AI tooling to controls data.
**Owns.** The data model, the dashboard definitions, the reporting calendar, the automation scripts.
**Trusted to decide.** How a metric is defined and computed, and what a visualisation is allowed to imply.
**Boundary.** Not a business-intelligence developer who happens to sit near a project: the role requires
knowing what an earned-value number means before displaying it. Distinguishing the two is one of the
things this taxonomy exists to make possible.
**BoK domains.** 4, 13 (AI for project controls), 6.

### 3.8 Project controls lead

**Purpose.** To integrate cost, schedule, risk and change on a project into one coherent account of
status and forecast.

**Accountable for.** Integration across the controls disciplines; the integrity of the reported position;
the controls calendar and cycle; the forecast the project reports; the quality of what the team produces.
**Owns.** The project controls execution plan, the integrated report, the forecast position.
**Trusted to decide.** What the project reports, and when a number is not yet fit to report.
**Boundary.** A lead is defined by *integration*, not by headcount. A lead with no direct reports is
normal; a "senior planner" who integrates cost, schedule and risk into a single reported position is
functioning as a lead and is coded as one.
**BoK domains.** 4, 5, 6, 10.

### 3.9 Project controls manager

**Purpose.** To run the controls function for a project, programme or business unit — its people, its
methods and its standards.

**Accountable for.** The controls team and its capability; method, process and tooling; assurance over
project reporting; interface with finance, commercial and delivery leadership; recruitment and
development.
**Owns.** The functional standard and procedures, the assurance regime, the resourcing plan.
**Trusted to decide.** How controls is done here, and whether a project's reported position is credible
enough to leave the function.
**Boundary.** Distinguished from the lead by accountability for *how the work is done* across more than
one delivery vehicle, rather than for one vehicle's numbers.
**BoK domains.** 4, 8, 12, 5.

### 3.10 Head of project controls

**Purpose.** To own the controls capability of the organisation: its standard, its systems, its people
pipeline and its voice at executive level.

**Accountable for.** The functional strategy and standard; systems and data architecture for controls;
capability development and succession; governance and assurance across the portfolio; representing the
integrity of reported performance to the board or client at the point where it is uncomfortable.
**Owns.** The functional standard, the systems roadmap, the capability plan, the assurance framework.
**Trusted to decide.** What the organisation's controls standard is, and what it will not report.
**Boundary.** Titles vary widely at this level — director, head of function, discipline lead, chief.
Classify on organisational scope, not on the word.
**BoK domains.** 8, 4, 13.

**A note on roles not listed.** Document control, project accounting, quantity surveying, contract
administration and project management proper are adjacent professions with their own structures. They are
out of scope here rather than junior versions of anything above; the survey routes respondents in those
roles out at screening.

## 4. The levelling rubric

Four levels, matching the Institute's competency levels so the survey and the competency framework can be
read together: **foundation · practitioner · professional · leader** (`CMP-02`).

Two vocabulary warnings, both of which cause real confusion. First, *professional* here is a level on a
competency scale — it is not the same idea as the level at which the Institute's credentials sit, which is
Leader. Second, *senior* is not a level in this rubric and never will be, because it is the word employers
use to mean whatever they need it to mean.

Levelling runs on six dimensions.

| Dimension | Foundation | Practitioner | Professional | Leader |
|---|---|---|---|---|
| **Autonomy** | Works to instruction on defined tasks; output is checked | Works to an objective; chooses method within an established framework | Sets the approach for a scope and is relied on without checking | Sets the framework others work within |
| **Scope owned** | Part of a discipline on part of a project | A discipline on a project, or part of one on a large project | An integrated scope, a large or complex project, or a discipline across several | A function, a portfolio, or an organisational capability |
| **Judgement under ambiguity** | Escalates when data is missing or contradictory | Resolves routine ambiguity; escalates the novel | Makes and defends a call where the data does not settle it | Decides where reasonable experts disagree, and carries the consequence |
| **Accountability for the number** | Accurate execution of a defined calculation | The number for their scope, and its explanation | The reported position, including what it implies | The organisation's reported position and its integrity |
| **Influence** | Within the immediate team | Across the project team; explains to non-specialists | Persuades delivery and commercial leadership, including against preference | Board, client and external; sets expectations others must meet |
| **Development of others and of method** | Learning the method | Supports others; applies method as given | Improves method; coaches; reviews others' work | Owns the method; builds the capability and the pipeline |

**The signature test.** Beyond the table, each level has work that only it does. Foundation produces
accurate data. Practitioner produces a defensible answer. Professional produces a defensible answer that
survives contact with someone who does not want to hear it. Leader decides what the organisation will
stand behind, and is the person who says the number is not ready.

## 5. Applying the rubric

Nobody sits cleanly at one level on all six dimensions. The decision rule:

1. Score each of the six dimensions independently.
2. The assigned level is the one with the most dimensions. Ties are broken by **accountability for the
   number**, then by **autonomy** — the two that most reliably distinguish real from nominal seniority.
3. **The scope ceiling.** A person cannot be levelled above the scope they actually own. Someone with
   leader-level influence and practitioner-level scope is levelled on the scope. This rule prevents the
   most common inflation, which is levelling on visibility.
4. **Tenure is not a dimension.** Long service at one level is long service at one level.
5. **Headcount is not a dimension.** It appears nowhere in the table, deliberately: technical leadership
   without direct reports is real leadership in this discipline.

Where a survey respondent's self-assessment at Q09 conflicts with their answers on scope (Q12), reporting
line (Q10) and role, the conflict is recorded but the self-assessment stands. Overriding it would replace
a stated measurement with an analyst's inference, and inference cannot be disclosed to a reader in the way
a rule can.

## 6. Aggregation hierarchy

`SAL-01` §6 rule 5 permits combining thin cells only along a hierarchy declared in advance. This is that
hierarchy, and it may not be varied for a particular table.

**Roles** combine upward into four groups, and no further:

| Group | Roles |
|---|---|
| Cost and commercial | Cost engineer · Cost controller · Estimator · Change / commercial controller |
| Planning and risk | Planner / scheduler · Risk analyst |
| Data and reporting | Reporting and data analyst |
| Controls leadership | Project controls lead · Project controls manager · Head of project controls |

**Levels** combine only as adjacent pairs: foundation with practitioner, professional with leader. A
combination spanning all four is never published, because a figure covering every level of a profession
describes nobody in it.

**Geography** combines country into sub-region into region, along a stated list published with the report.
Countries are never combined ad hoc to reach a threshold.

Every combined cell is labelled as combined, in the cell and not only in a footnote.

## 7. Mapping to the competency framework

The Institute's competency series (`CMP-01` to `CMP-10`) defines what competence looks like; this taxonomy
defines what jobs exist and how they are levelled. The two meet at the level labels, which are shared, and
at the competency sets.

For the project controls credential PCL-AI, the platform's seeded set of fourteen competencies maps to the
roles as follows. Each role's *primary* competencies are those the role is accountable for; every role
touches more.

| Role | Primary PCL-AI competencies |
|---|---|
| Planner / scheduler | Planning and scheduling · Performance measurement · Project risk |
| Cost engineer | Cost management · Forecasting and EAC · Earned value management |
| Cost controller | Cost management · Performance measurement · Project controls governance |
| Estimator | Cost management · Forecasting and EAC |
| Risk analyst | Project risk · Predictive analytics · Forecasting and EAC |
| Change / commercial controller | Commercial and contract controls · Cost management |
| Reporting and data analyst | Digital reporting · Automation · AI-enabled project controls · Predictive analytics |
| Project controls lead | Performance measurement · Earned value management · Forecasting and EAC · Project controls governance |
| Project controls manager | Project controls governance · Human validation · Responsible AI |
| Head of project controls | Project controls governance · Responsible AI · Human validation |

Two competencies — **responsible AI** and **human validation** — appear as primary only at manager and
head level, and that placement is a position, not an oversight. Every role uses AI-assisted tooling;
accountability for whether its output is fit to rely on escalates with the level that carries
accountability for the reported number. This is the Institute's standing position — AI proposes; the
professional disposes — expressed as a levelling consequence.

`[CONFIRM: whether the role-to-competency mapping above is reviewed and endorsed by the Standards
Committee before publication, or presented as an editorial mapping]`

`[CONFIRM: reconciliation of these role definitions with the task statements produced by the forthcoming
PCL-AI job-task analysis, once that study has been run]`

## 8. Coding rules for free-text titles

For analysts coding Q06 where Q07 was answered `Other`, or where a response is inconsistent.

1. Code from the accountability described, never from the title string alone.
2. Where a title contains two roles ("planning and cost engineer"), code the primary role from Q07 and
   record the second at Q08. Do not create hybrid categories.
3. Ignore all seniority words when assigning the role — senior, principal, lead, chief, junior, graduate,
   assistant, associate. They inform nothing about the role and are handled by the level, which is
   measured separately.
4. Treat "lead" in a title as evidence of the lead role only where the described accountability is
   *integration across disciplines*. Otherwise it is a level signal, not a role signal, and even then it
   is not decisive.
5. Where an employer's title has no relationship to the described work, code the work and flag the record.
   Flagged records are counted and the count is published.
6. Where a role genuinely falls outside the ten, code it `OTHER` with the verbatim description retained.
   `OTHER` is reported as a count and never distributed across the ten to tidy the table.
7. Double-code a sample of records independently and report the agreement, under the same rules the skills
   codebook uses (`SAL-04` §5).

## 9. How this goes wrong

**Levelling on title after all.** The rubric is adopted, then someone maps titles to levels once and
applies the mapping thereafter. Within two cycles the data is title data with a rubric-shaped label on it.

**Levelling on tenure.** Fifteen years in the same role becomes "professional" because it feels
disrespectful not to. The scope ceiling in §5 exists for this moment.

**Headcount as the leadership test.** A technically formidable practitioner who owns the method for a
portfolio is levelled below a manager of three, because one has reports and the other does not. In this
discipline that is usually the wrong way round.

**The everything role.** In a small organisation one person is planner, cost engineer, risk analyst and
lead. Coding them into all four inflates every cell; coding them into the largest half of their time, with
the second recorded, is the only defensible option and must be applied consistently.

**Roles invented to fit an internal structure.** An organisation adopts the taxonomy and adds four local
roles. Legitimate internally, fatal to comparison — local roles must resolve back to one of the ten before
any figure is compared with anyone else's.

**Assuming the taxonomy is a pay ladder.** The ten roles are not ordered by value and the four levels
carry no pay implication. The honest use is job matching, after which pay is a separate question with its
own evidence.

## 10. Checklist — matching a job to this taxonomy

- [ ] Read the job's accountabilities, not its title
- [ ] Identify what artefact the job owns and what decision it is trusted with
- [ ] Choose the role by where most of the time goes; record a second role if the split is genuine
- [ ] Score all six levelling dimensions independently before deciding the level
- [ ] Apply the scope ceiling: no level above the scope actually owned
- [ ] Check the tie-breakers — accountability for the number, then autonomy
- [ ] Confirm tenure and headcount played no part in the decision
- [ ] Record any conflict between the self-assessed level and the scope answers
- [ ] Where the role does not fit, code it as other and keep the description — do not force it
- [ ] Re-read the resulting classification and ask whether two people classified identically would
      recognise each other's jobs

---

## Related

- `SAL-02 — The survey instrument` — the questions this taxonomy classifies, and the definitions linked from them
- `SAL-04 — The skills demand taxonomy` — the skills vocabulary applied across these roles
- `SAL-05 — Report template and data tables` — where role and level become reporting dimensions
- `CMP-01 — The PCI competency framework — overview` — what competence means, as distinct from what a job is
- `CMP-02 — Competency levels and how they are evidenced` — the shared level labels and the evidence each requires
- `CAR-01 — The project controls career roadmap` — how a career moves through these roles and levels

## Sources and standards

No external source is cited. The competency names in §7 are the Institute's own seeded competency set for
PCL-AI as recorded in `00-framework/CANONICAL-FACTS.md` §7; the level labels are those used by the
competency series and the framework's front-matter schema. Body of Knowledge domain numbers refer to the
thirteen domains in `docs/bok/`. The role definitions are the Institute's editorial synthesis of how the
work is organised across sectors; they are not drawn from, and do not reproduce, any other body's job
architecture.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
