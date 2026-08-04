---
id: AIG-03
series: S02
series_name: AI in Project Controls Guide
title: "Data readiness: what AI needs before it is any use"
subtitle: Coding structures, cut-off discipline, master data and labels — and what to do when they are not there
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 13
summary: >
  Most AI failures in a project controls function are data failures wearing a model's clothes. This
  document sets out what AI actually needs from controls data — a coding structure that is used rather than
  merely published, cut-off discipline, governed master data, comparable and normalised history, and above
  all usable labels — and gives an eight-question readiness test naming the artefact, the field, the
  frequency and the owner. It then deals with the situation practitioners are actually in: what to attempt
  when the data is not ready and will not be ready this year.
linkedin:
  format: newsletter
  hook: >
    A cost ledger with 26,400 rows can still be a dataset with six usable examples. Counting rows is not a
    readiness assessment.
  tags: [ProjectControls, DataQuality, CostEngineering, ArtificialIntelligence]
  asset: checklist-pdf
gated: false
related: [AIG-02, AIG-04, AIG-09, BPG-03, BPG-07]
bok_domains: [1, 5, 11, 13]
sources:
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
placeholders: 0
---

# Data readiness: what AI needs before it is any use

> Coding structures, cut-off discipline, master data and labels — and what to do when they are not there.

**In one paragraph.** Most AI failures in a project controls function are data failures wearing a model's
clothes. This document sets out what AI actually needs from controls data — a coding structure that is used
rather than merely published, cut-off discipline, governed master data, comparable and normalised history,
and above all usable labels — and gives an eight-question readiness test naming the artefact, the field,
the frequency and the owner. It then deals with the situation practitioners are actually in: what to
attempt when the data is not ready and will not be ready this year.

**Who this is for.** Cost engineers, cost managers, planners and project controls managers preparing a
function for AI, and the data owners in finance and commercial whose records they depend on.

---

## 1. The failure is usually upstream

When an AI initiative in controls disappoints, the post-mortem almost never finds a bad algorithm. It finds
a cost ledger in which a fifth of the lines sit in a general or miscellaneous code; accruals posted on
invoice date rather than service date, so three months of the history are shifted by a period; two vendor
records for the same supplier; a percentage complete that means one thing in the schedule and another in
the cost report; and nine completed projects, three of which were restated after closeout, offered as
"historical data".

None of that is exotic. It is the ordinary condition of controls data in a busy organisation, and it has
always been tolerable because a human reading the ledger silently corrects for it — the cost engineer knows
that code 9990 is where the site supervisor puts anything awkward, and mentally re-allocates. A model does
not know. It learns the miscoding as if it were the business. This is the practical content of "garbage in,
garbage out": not that the data is wrong in obvious ways, but that its known-and-tolerated distortions
become the pattern the model treats as truth.

The consequence for planning is direct. **Data remediation is not the preliminary to the AI programme; for
the first year it usually is the AI programme.** A function that budgets for licences and not for coding
work has mispriced the initiative.

## 2. What AI actually needs

Six things, in the order they usually bite.

### 2.1 A coding structure that is used, not merely published

The work breakdown structure (WBS), cost breakdown structure (CBS) and code of accounts are what make a
cost line comparable to another cost line. Their design belongs to `BPG-03 — Cost breakdown structure and
the code of accounts`; what matters here is a narrower question: **is the structure actually applied?**

Three diagnostics, each answerable from your own ledger this afternoon. First, the **catch-all share**: what
proportion of value and of lines sits in general, sundry, miscellaneous or default codes? Anything above a
few per cent means the structure is being routed around. Second, the **obsolete-code share**: how many
postings use codes retired in an earlier structure, and is there a mapping table from the old structure to
the current one, owned by a named person? Third, the **granularity mismatch**: does the level at which cost
is coded correspond to the level at which the schedule is planned? A model asked to relate cost to progress
across a mismatch will learn noise.

### 2.2 Cut-off discipline

Cut-off is the discipline that puts a cost in the period in which the work happened. It is covered as
practice in `BPG-07 — Accruals and cut-off discipline`; its relevance here is that cut-off errors are
**systematic**, and systematic errors are exactly what models learn most eagerly.

The specific failures that matter: accruals raised from document date rather than service date; a month
closed before late invoices arrive, with no accrual, so the cost appears in the following period; period
locking that is not enforced, so a prior period changes after a model has been trained on it; and
restatements that are made in the reporting layer but not in the source, so the ledger and the reported
history disagree. If your monthly actuals move after the fact, record when and by how much — a model that
cannot tell a real trend from a posting-lag artefact will forecast the artefact.

### 2.3 Governed master data

Master data is the reference information every transaction points at: vendors, cost elements, resources,
calendars, currencies, units of measure, contract records. It fails in ordinary ways — the same supplier
under three spellings, a unit of measure recorded as "m2" in one system and "sqm" in another, two calendars
with different holiday sets, currency held without the rate basis used.

The control is unglamorous and specific: **one owner per master-data domain**, a documented process for
creating and retiring records, a periodic duplicate check, and a rule about what happens to historical
records when a master record is merged. Duplicate-detection and anomaly work is worthless where the vendor
master itself is duplicated: the model will faithfully report that two different suppliers submitted
identical invoices.

### 2.4 Labels — the thing you are asking the model to predict

This is the point at which most controls datasets turn out to be far smaller than they look, and it is the
single most under-discussed item on the list.

Supervised learning — the kind that predicts a cost at completion, a probability of overrun, an actual
finish date — learns from **examples with known answers**. The known answer is the *label*. In controls the
label is usually a project-level or control-account-level outcome: what the final cost actually was, when
the work actually finished, whether the risk actually occurred. A ledger with hundreds of thousands of
transaction rows may contain a few dozen such outcomes, and fewer that can be trusted.

Three questions establish whether you have labels at all. **Is the outcome recorded in a field, or only in
a closeout report?** A number in a document is not a label. **Is the recorded outcome final?** A final cost
restated after closeout, or an actual finish taken from a schedule never updated after handover, is a
mislabelled example, and mislabelled examples do more damage than missing ones. **Is the outcome
attributable to the same unit as the input?** A final cost at project level cannot be learned from features
recorded at package level unless someone has done the mapping.

### 2.5 Comparable history, normalised

Historical projects are only informative if the comparison is fair. Before history is fed to anything, four
normalisations are usually needed, and each should be a stated, versioned assumption rather than a habit:
a **price basis** (costs brought to a common date with a stated escalation basis), a **location basis**, a
**scope basis** (what was in and out — a project that self-performed what another subcontracted is not
comparable without adjustment) and a **contract basis** (a reimbursable job and a lump-sum job have
different cost behaviour).

Skip normalisation and the model learns escalation and calls it productivity.

### 2.6 One definition per number, and lineage

Two controls that cost little and prevent a great deal. **One governed definition per metric** — percentage
complete, committed cost, cost to date, forecast — held centrally and used by every report and every tool.
Where two systems compute the same metric differently, decide which is authoritative and record the
reconciling difference rather than letting both circulate. **Lineage** is the ability to trace a figure to
its source and through every transformation. It is what allows an AI-influenced number to be defended
months later, and its absence is what turns a challenged number into an argument about memory.
`AIG-09 — Bias, explainability and auditability` covers the evidencing standard.

## 3. The readiness test

Eight questions. Each has an artefact, an owner and a cadence, because "we should improve data quality" is
not an action.

| # | Question | Artefact that answers it | Owner | Cadence |
|---|---|---|---|---|
| 1 | What share of cost lines and value sits in catch-all codes? | Ledger extract profiled by code | Cost manager | Monthly |
| 2 | Do postings use retired codes, and is there a current mapping table? | Code-of-accounts mapping register | Cost manager | On structure change |
| 3 | Are accruals raised on service date? | Accrual listing with service and document dates | Financial controller | Monthly at close |
| 4 | Do closed periods stay closed, and are restatements logged? | Period-lock report and restatement log | Financial controller | Monthly |
| 5 | How many duplicate master records exist in vendor, resource and cost element? | Master-data duplicate report | Master-data owner | Quarterly |
| 6 | How many completed units have a recorded, final, attributable outcome? | Label inventory — one row per completed project or control account | Controls manager | Quarterly |
| 7 | Is history normalised to a stated price, location, scope and contract basis? | Normalisation basis note, versioned | Estimating lead | On each dataset build |
| 8 | Is there one governed definition per reported metric, with lineage to source? | Metric definitions register and data-lineage map | Controls manager | Half-yearly |

A function that can produce all eight artefacts is ready to pilot with confidence. A function that can
produce none of them should not conclude that AI is unavailable to it — see §4 — but should certainly not
begin with a predictive model.

## 4. When the ideal is unavailable, which is usually

Very few functions pass §3 on the first attempt, and waiting for a data programme to finish is a way of
never starting. Four honest routes forward.

**Start where history is not required.** Extraction, retrieval-grounded answering over documents,
classification against current rules and schedule logic checking need no historical labels. They work on
today's material. This is why document-heavy and rules-heavy tasks are the sensible first pilots for a
function with poor history — the value arrives while the ledger is still being cleaned.

**Use rules while the data matures.** Deterministic checks — tolerance breaches, missing successors,
postings to closed periods, approvals out of sequence — are transparent, auditable and immune to the data
problems that defeat models. They also generate the exception discipline a model will later need.

**Build the label set forwards.** If there is no reliable record of final cost, actual finish and realised
risk, start recording one now: a single register, one row per completed control account, populated at
closeout by a named person as a condition of closeout. Two years of deliberate labelling is worth more than
ten years of archaeology.

**Narrow the scope until the data supports it.** A model over one repeatable package type with fifty clean
instances will outperform a portfolio-wide ambition with nine mixed ones. Narrow, prove, extend.

What is not an acceptable route is training on data known to be distorted and treating the output as
indicative. An indicative number that is wrong in a consistent direction is more dangerous than no number,
because it will be quoted.

## 5. How this goes wrong

**Volume is mistaken for readiness.** "We have ten years of data" describes a row count. The question is
how many *labelled, comparable, trustworthy* examples exist, and the answer is usually one or two orders of
magnitude smaller. §6 works this through.

**The data assessment is done by the vendor.** A supplier profiling your data to establish whether their
product will work is not the same as your function establishing whether its data supports the decision.
Both may be worth doing; only one is assurance.

**Cleaning is done once, for the pilot.** A one-off remediation produces a clean training set and a dirty
production feed. Within two cycles the live data no longer resembles what the model learned, and
performance degrades without anyone deciding anything.

**The catch-all code is fixed by renaming it.** Splitting "miscellaneous" into six specific codes achieves
nothing if the site team still has no time to code accurately. The remedy is at the point of capture —
default coding rules, a shorter picklist, a purchase-order-driven default — not in the chart of accounts.

**Confidential data is prepared carelessly.** Anonymisation that leaves project names in free-text fields,
or a "cleaned" extract that retains personal data in a comment column, is a confidentiality incident
waiting to happen. Preparation includes checking what left with the data, not only what was intended to.

**Restatements are silently absorbed.** Prior-period figures change and nothing records that they did, so
the difference between a real trend and a posting correction becomes unrecoverable. A restatement log costs
one line per event and saves an entire class of forecasting error.

**Definitions drift between the tool and the report.** A natural-language query answers from a subtly
different definition of committed cost than the controlled report uses, and two credible numbers now
circulate. Reconcile every AI-produced figure to the governed one before it is quoted, and fix the
definition rather than the answer.

## 6. Worked example — a readiness profile

*Illustrative figures.* A controls function proposes to build a cost-at-completion model. It offers 24
months of cost ledger across nine completed projects: **26,400 rows**. Before anything is built, the data
is profiled against the checks of §3.

| Check | Rows failing | Share of 26,400 |
|---|---|---|
| Invalid or retired cost code | 1,320 | `1,320 ÷ 26,400 = 5.0 %` |
| Missing or defaulted WBS reference | 792 | `792 ÷ 26,400 = 3.0 %` |
| Posted in a period after the service date crossed a cut-off | 1,584 | `1,584 ÷ 26,400 = 6.0 %` |
| Duplicate candidate (same vendor, amount and date) | 264 | `264 ÷ 26,400 = 1.0 %` |
| Currency or unit-of-measure inconsistency | 396 | `396 ÷ 26,400 = 1.5 %` |
| **Total flags** | **4,356** | `4,356 ÷ 26,400 = 16.5 %` |

Flags are not rows: some rows fail more than one check. De-duplicating the flag list gives **3,696 distinct
rows affected**, which is `3,696 ÷ 26,400 = 14.0 %` of the dataset, with `4,356 − 3,696 = 660` rows counted
twice.

Remediation effort, at an assumed 2 minutes per affected row for review and re-coding:

`3,696 × 2 = 7,392 minutes = 7,392 ÷ 60 = 123.2 hours = 123.2 ÷ 8 = 15.4 person-days`

**Then the sentence that matters.** The model is to predict final cost. The label is one number per
completed project, so the dataset does not have 26,400 examples; it has **nine**. Of those nine, three were
restated after closeout and the restatement was never carried back to the source ledger, so three labels
are known to be wrong. **Six trustworthy labelled examples.**

**Result.** Do not build the predictive model. Do the 15.4 days of remediation because it improves this
month's reporting regardless; begin a label register populated at closeout; and put the AI effort into
extraction and rules-based checking, which need no labels. Revisit the model when the label count supports
it.

**Assumptions this answer depends on.** That 2 minutes per row is a measured rate for this team, not an
estimate; that the five checks are independent enough for the overlap figure to be meaningful; and that
project-level final cost is the right prediction target at all — a control-account-level target would give
more labels, at the price of needing control-account-level features.

## 7. Checklist — before you feed anything to a model

1. **Profile first, build second.** Run the five checks of §6 on the actual extract, and record the result
   with a date. Do not accept a description of the data in place of a profile of it.
2. **Count labels, not rows.** State how many completed units have a final, attributable, unrestated
   outcome. If the number is in single figures, the honest answer is that you do not have a training set.
3. **Name the price, location, scope and contract basis** to which history has been normalised, and version
   the note.
4. **Confirm the cut-off rule** — service date, not document date — and confirm that closed periods are
   locked and restatements logged.
5. **Check the master data** for duplicates in vendor, resource, cost element and calendar before relying on
   any anomaly or duplicate detection.
6. **Fix one definition per metric** and reconcile every AI-produced figure to the governed one.
7. **Confirm what left with the extract**: personal data, project names in free text, commercially sensitive
   rates. Preparation is a confidentiality control, not only a quality one.
8. **Write down what you will not do yet**, and what would change that. A readiness assessment that
   concludes "not yet" has done its job.

---

## Related

- `AIG-02 — What AI actually does in a controls function` — which capability classes need history and which
  do not, the distinction §4 turns on.
- `AIG-04 — AI-assisted cost forecasting` — the first place inadequate labels and unnormalised history do
  visible damage.
- `AIG-09 — Bias, explainability and auditability` — the evidencing and monitoring standard that lineage
  supports.
- `BPG-03 — Cost breakdown structure and the code of accounts` — designing the structure whose use §2.1
  tests.
- `BPG-07 — Accruals and cut-off discipline` — the practice behind §2.2.

## Sources and standards

- **PCI Body of Knowledge, Domain 13** — *AI for project controls and project management* (Institute
  manuscript, 2026), Knowledge Area 13.2 on data quality dimensions, structure, governance and lineage,
  which this document extends into a readiness test and a not-ready-yet route.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
