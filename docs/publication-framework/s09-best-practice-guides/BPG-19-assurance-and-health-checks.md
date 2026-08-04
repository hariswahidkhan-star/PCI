---
id: BPG-19
series: S09
series_name: Best Practice Guides
title: Project controls assurance and health checks
subtitle: Testing whether the numbers can be relied on, early enough for the answer to matter
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: professional
reading_time_min: 15
summary: >
  A project controls health check tests whether a project's control system is capable of producing a number
  a decision can rest on. This guide sets out what such a review examines, how to score it on evidence
  rather than impression, how much independence the reviewer needs and what to do when full independence is
  unavailable, the difference between assurance and audit, why timing determines whether findings can be
  acted on at all, and how to write findings that are accepted rather than resented. The worked example
  scores a review and then demonstrates, arithmetically, why a composite score cannot express a blocking
  weakness.
linkedin:
  format: document
  hook: >
    A controls health check scores six elements and returns 55 per cent. One of those six is progress
    measurement, scored 1 out of 4 — and every other number on the project is computed from it. The
    composite is arithmetically correct and tells the board nothing useful.
  tags: [ProjectControls, Assurance, Governance, ProjectManagement, PMO]
  asset: checklist-pdf
gated: false
related: [TPL-15, BPG-05, TPL-14, BPG-04, BPG-14, BPG-16]
bok_domains: [4, 8, 10]
sources:
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 10 — Project Scheduling, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 8 — Project Management Lifecycle, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 4 — Performance Management, Variance Analysis and Management Reporting, first authored draft, August 2026"
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), verified August 2026"
placeholders: 0
---

# Project controls assurance and health checks

> Testing whether the numbers can be relied on, early enough for the answer to matter.

**In one paragraph.** A project controls health check tests whether a project's control system is capable of
producing a number a decision can rest on. This guide sets out what such a review examines, how to score it
on evidence rather than impression, how much independence the reviewer needs and what to do when full
independence is unavailable, the difference between assurance and audit, why timing determines whether
findings can be acted on at all, and how to write findings that are accepted rather than resented. The
worked example scores a review and then demonstrates, arithmetically, why a composite score cannot express a
blocking weakness.

**Who this is for.** PMO leads and heads of project controls who commission or run reviews; controls
managers who are reviewed; and sponsors and assurance committees who have to decide what a review's output
entitles them to conclude.

---

## 1. The question a health check is actually asking

A project controls health check is not an inspection of whether procedures are being followed. It asks one
question: **can this project's control system produce a number that a decision can safely rest on, and if
not, exactly where does the chain break?**

That framing does most of the work. It rules out the compliance sweep — a checklist of documents that exist,
signed by people who exist — because a project can have every artefact the framework requires and still be
incapable of telling you what it will cost. It also rules out the opinion piece, in which an experienced
reviewer walks the project for two days and reports what they felt. Both are common; neither can be
challenged, defended or acted on with any precision.

The chain being tested runs in one direction, and each link depends on the ones before it:

**Scope is defined and structured** — a breakdown that the budget, the schedule and the cost ledger all
share. Without a common structure, nothing downstream reconciles and every comparison is manual.

**A baseline exists and is controlled** — approved, dated, with a change process that is actually used.
Without it there is nothing to measure against, and variance analysis is comparison with the most recent
opinion.

**Cost is captured completely and on time** — commitments, actuals and accruals, at the cut-off, coded to
the structure. Without this, every efficiency measure has an unreliable denominator.

**Physical progress is measured against rules** — documented rules of credit, applied consistently,
verified independently on a sample. Without this, earned value is a claim.

**Forecasting is method-based and defensible** — a stated method, chosen for a reason connected to the cause
of the variance, with the assumption written next to the answer.

**Risk and change are live** — a register that changes decisions and a change process that captures scope
movement before it is executed rather than after.

**Reporting reaches the decision** — the right measure, at the right cadence, to the person who acts, with
exceptions visible.

A review that walks this chain in order produces findings that are self-prioritising, because a break early
in the chain invalidates everything after it. That property is worth more than any scoring scheme, and §8
shows why the scoring scheme cannot substitute for it.

## 2. Assurance and audit are different instruments

The two words are used interchangeably and should not be. The distinction is not organisational politics; it
changes what the reviewer may conclude and what the project is obliged to do.

| | Audit | Assurance / health check |
|---|---|---|
| Question | Was the defined requirement complied with? | Will this control system produce a reliable number? |
| Criterion | An existing rule — procedure, contract, standard, policy | Fitness for the decisions the project must support |
| Output | Exceptions against the rule | Capability, with the specific weaknesses that limit it |
| A project can fail by | Not following the process | Following the process, where the process is inadequate |
| Relationship to the team | Independent testing; findings are not negotiable in substance | Advisory; findings are negotiable in *how*, not in *whether* |
| Typical timing | After the fact | Early enough to change the outcome |

The consequential difference is the row about failing while compliant. An audit against a controls procedure
can find full compliance on a project whose forecast is meaningless, because the procedure never required the
forecast method to be justified. Assurance is the instrument that catches that, and it can only do so if its
criterion is fitness rather than conformance.

Both are legitimate and projects need both. What causes damage is running one and reporting it as the other:
an audit dressed as assurance produces a clean report on an uncontrolled project, and assurance dressed as an
audit produces findings the team treats as non-negotiable when they are in fact recommendations, which is
where the resentment in §6 begins.

## 3. Evidence-based scoring

A score that cannot be traced to something the reviewer inspected is an opinion with a number attached. It
will be argued with, and it will be argued with successfully, because there is nothing behind it.

Three requirements make a score defensible.

**A published rubric, written before the review.** Each element has descriptors for each score, expressed as
observable conditions rather than adjectives. Compare these two rubrics for progress measurement:

*Not defensible:* 0 poor, 1 weak, 2 adequate, 3 good, 4 excellent.

*Defensible:*

| Score | Condition |
|---|---|
| 0 | No documented rules of credit. Progress is a judgement, recorded as a percentage. |
| 1 | Rules of credit exist as a document but progress is claimed by percentage judgement in practice. |
| 2 | Rules of credit documented and applied on major packages; no independent verification. |
| 3 | Applied across the measured scope, with independent verification on a defined sample. |
| 4 | As 3, and physical progress, valuation and earned value are reconciled each period with differences explained. |

The second version can be evidenced, disputed on facts, and re-tested at the next review by a different
person who will reach the same answer.

**Evidence recorded per score: the artefact, its date, the test applied.** "Progress measurement scored 2"
is an assertion. "Progress measurement scored 2: rules of credit document rev C dated 14 March covers
packages 100–400; the April progress return for package 500 records 62 per cent with no supporting quantity
record; no evidence of independent verification in the last three periods" is a finding. It also survives the
reviewer leaving the organisation, which the assertion does not.

**A stated sample, with its coverage expressed two ways.** A review that inspects a handful of control
accounts should say which, why those, how many out of how many, and what proportion of value they represent.
Coverage by count and coverage by value are different numbers and both are needed — §8 shows a case where
they are 8.6 per cent and 65.3 per cent respectively, and reporting only the first would understate the
review while reporting only the second would overstate it.

Two disciplines protect the scoring from itself. **Score the element, not the person** — a low score for
progress measurement on a project whose measurement was never resourced is a finding about resourcing, and
the finding should say so. And **agree the condition before writing the conclusion**: show the project the
factual observations, correct any that are wrong, and only then write cause and consequence. Almost every
dispute about a review turns out to be a dispute about a fact that could have been corrected in ten minutes.

## 4. Independence, and what to do without it

The reviewer must not be assessing their own work, and must not report through the person whose project is
being reviewed. Both conditions are frequently breached, usually with good intentions: the most knowledgeable
person available is the one who helped build the thing.

The threats worth naming, because naming them is most of the management:

**Self-review** — the reviewer designed the process, the template or the baseline being examined. They
cannot find it wanting without finding themselves wanting.

**Reporting line** — the reviewer's next assignment, appraisal or invoice depends on the person receiving
the findings.

**Familiarity** — a long relationship with the team makes the uncomfortable finding costly to write, and
makes it easy to accept an explanation that would not be accepted from a stranger.

**Advocacy** — the reviewer has previously argued publicly for the approach in use.

Full independence is often unavailable, particularly in smaller organisations. The professional answer is
not to pretend, and not to refuse. It is to **declare the impairment on the face of the report, restrict the
conclusions accordingly, and add a compensating control**. Pair the internal reviewer with someone from
another project or another function. Have a second person re-perform the scoring on the same evidence.
Exclude from the reviewer's scope the specific elements they built, and say in the report which elements were
excluded and why. A report that states "the reviewer designed the reporting template; element 6 was scored by
a reviewer from the adjacent programme" is more credible than one that claims an independence nobody in the
room believes.

One more condition is often forgotten: **the reviewer needs access, and access is granted, not assumed.**
Access to the cost system rather than to extracts prepared for the review, to the schedule file rather than a
printed bar chart, to the change log, to the register, and to people without their manager in the room.
Access limitations are themselves a finding and should be reported as one.

## 5. Timing: early enough to matter

The value of a health check falls sharply with elapsed project time, and the reason is structural rather than
motivational.

Most controls weaknesses are *foundational*: a coding structure that does not support the reporting the
project needs, rules of credit that were never written, a baseline that was never properly approved. The cost
of fixing a foundational weakness scales with everything built on top of it. Correcting a coding structure in
month two means recoding two months of transactions. Correcting it in month fourteen means recoding fourteen
months of transactions, restating every report issued from them, and explaining to a board why the trend they
have been watching has changed shape. The second is not fourteen times harder than the first; it is often
simply not done, and the project runs to completion on a structure everyone knows is wrong.

Three timing points earn their cost:

**Before baseline acceptance.** The single highest-value review. Everything is still cheap to change,
nothing has been reported externally, and the review's findings can become conditions of baseline approval —
which is the only mechanism that reliably gets them fixed.

**At the first reporting cycle after mobilisation.** The first month-end is when the design meets reality.
Reviewing the first cycle catches the gap between the controls plan and what the team can actually execute,
while the correction is a process change rather than a restatement.

**At each gate, and on trigger.** Triggers worth defining in advance: a change of controls manager, a
re-baseline, the first month a forecast moves by more than a defined tolerance, and the mobilisation of a
major subcontract package.

A review at sixty per cent complete can still be worth doing — but its honest purpose is different. It is not
improving the control system; it is establishing what the current numbers can and cannot support, so that
decisions taken from here are taken with the right level of confidence. Reviewers should say which of those
two jobs they are doing, because a late review reported as an improvement exercise generates
recommendations nobody has the time or the appetite to implement.

## 6. Writing findings that get acted on

The difference between a finding that is implemented and one that is resented is mostly structure. Five
components, in this order:

**Condition** — what is, factually, with the evidence and its date. No adjectives. This is the part the
project must be given the chance to correct before anything else is written.

**Criterion** — what should be, and whose rule that is. "The project's own controls execution plan section
4.2 requires…" is far stronger than "good practice requires…", because it removes the argument about whose
standard applies. Where the criterion genuinely is the reviewer's professional judgement, say so and give the
reasoning.

**Cause** — why the gap exists, at the level of process, resourcing or design rather than person. "The
progress measurement role was not filled until month four" is a cause. "The cost engineer did not prioritise
this" is an accusation, and it converts a finding into a personnel dispute in which the substance is lost.

**Consequence** — what it costs in decisions, not in abstract risk. "The forecast presented to the September
board cannot be traced to a stated method, so its sensitivity to the steelwork productivity assumption is
unknown" tells a sponsor what they are exposed to. "This represents a significant risk to project delivery"
tells them nothing and will be read as boilerplate.

**Correction** — the specific act, its owner and its date. If the reviewer cannot name the act, the finding
is not ready; a finding whose correction is "improve the process" will be closed by an assertion that the
process has been improved.

Four further rules that determine whether the report changes anything:

**Rank by consequence, not by severity adjective.** Three findings ranked by what they prevent the project
from knowing beats fifteen ranked as high, medium and low.

**Cap the count.** A report with forty findings will have none of them fixed. If forty genuinely exist,
report the five that block the chain and list the rest as observations with no owner.

**Route each finding to the person who can fix it.** Some findings are not the project's to fix — a
corporate coding structure, an unresourced role, a template mandated by the PMO. Sending them to the project
guarantees they stay open.

**Track to closure with evidence.** A finding is closed when the reviewer sees the changed artefact, not
when the owner reports that it is done. Publishing the closure rate is what stops the next review starting
from the same place.

## 7. How this goes wrong

**The compliance sweep sold as assurance.** Every required document exists, so the project scores well, and
the forecast is still unusable. The tell is a report whose findings are all about documents and none about
numbers.

**The score without the rubric.** Numbers assigned on impression, disputed by the project, defended by
seniority. The next review reaches different scores on an unchanged project, and the trend is meaningless.

**The composite score as the deliverable.** A single percentage travels well and hides everything the review
found. §8 demonstrates arithmetically why it cannot represent a blocking weakness.

**No sample stated.** "We reviewed the cost reports" — how many, which, chosen how, covering what value.
Without those, the review cannot be repeated or extended.

**Independence asserted rather than analysed.** The report claims independence; the reviewer built the
template. Declaring the impairment costs a paragraph; being found to have concealed it costs the review.

**Findings written as accusations.** Cause stated at the level of a person. The project's response becomes a
defence of individuals, the substance is never discussed, and the next review is obstructed before it starts.

**Corrections that cannot be verified.** "Strengthen the change control process." Closed at the next review
on the strength of an assertion, because there is no artefact to inspect.

**Too late to matter, and not said.** A review at sixty per cent complete reported as though its findings
could still be implemented. The project agrees to everything and does none of it, which teaches everyone that
assurance findings are optional.

**Findings routed to the wrong owner.** Corporate-level causes sent to a project team with no authority to
change them, then reported as unresolved project failures at the next cycle.

**The review that is never closed out.** Findings raised, actions agreed, nobody tracks them, the next review
raises the same findings. Reporting the closure rate from the previous review is the cheapest correction
available and the most often omitted.

## 8. Worked example

*Illustrative figures. One project controls health check at a single review date. Scores are on a 0–4 scale
against a published rubric. Weights sum to 100 per cent. All arithmetic shown; percentages rounded to whole
numbers where stated.*

### 8.1 The sample and its coverage

The project has **140 control accounts** with a total budget of **9,500,000** (USD, illustrative). The review
inspected the **12 largest by budget**, carrying **6,200,000**.

```
coverage by count = 12 ÷ 140  = 0.0857  →  8.6 %
coverage by value = 6,200,000 ÷ 9,500,000 = 0.6526  →  65.3 %
```

Both numbers belong in the report. Coverage by count alone reads as a thin review; coverage by value alone
conceals that 128 accounts — including every small account, where control weaknesses concentrate — were not
examined. The sampling rule ("the twelve largest by budget") should also be stated, because it is not random
and its bias is knowable: it systematically excludes the small, unglamorous accounts, and a future review
should sample differently for exactly that reason.

### 8.2 The scores

| # | Element | Weight | Score (0–4) | Weighted contribution |
|---|---|---:|---:|---:|
| 1 | Baseline integrity and change control | 20 % | 3 | 0.20 × 3 = 0.60 |
| 2 | Schedule quality | 20 % | 2 | 0.20 × 2 = 0.40 |
| 3 | Cost and commitment capture | 15 % | 3 | 0.15 × 3 = 0.45 |
| 4 | Progress measurement | 15 % | 1 | 0.15 × 1 = 0.15 |
| 5 | Forecasting | 15 % | 2 | 0.15 × 2 = 0.30 |
| 6 | Risk and change management | 15 % | 2 | 0.15 × 2 = 0.30 |
| | **Total** | **100 %** | | **2.20** |

```
weight check   = 20 + 20 + 15 + 15 + 15 + 15 = 100 %
composite      = 0.60 + 0.40 + 0.45 + 0.15 + 0.30 + 0.30 = 2.20
as a percentage of the 4.00 maximum = 2.20 ÷ 4.00 = 0.550  →  55 %
```

### 8.3 Why 55 per cent is not the finding

Element 4, progress measurement, scored **1**: rules of credit exist as a document but progress is claimed
by percentage judgement in practice, with no independent verification in the last three periods.

Every subsequent number on this project is computed from that element. Earned value is derived from measured
progress. The cost performance index and schedule performance index are derived from earned value. The
forecast is derived from those indices. A score of 1 on element 4 therefore does not mean "one element is
weak"; it means the project's cost performance measurement, its schedule performance measurement and its
forecast are all resting on unverified percentage judgements.

The composite cannot express this, because a weighted average treats the elements as independent and
additive. They are neither.

### 8.4 Testing whether a dependency rule rescues the composite

A common remedy is to add a rule: **no element may be scored above the lowest score of the elements it
depends on**. Forecasting (element 5) depends on progress measurement (element 4, scored 1) and cost capture
(element 3, scored 3), so it is capped at 1 and falls from 2 to 1.

```
revised composite = 0.60 + 0.40 + 0.45 + 0.15 + (0.15 × 1) + 0.30
                  = 0.60 + 0.40 + 0.45 + 0.15 + 0.15 + 0.30
                  = 2.05

as a percentage   = 2.05 ÷ 4.00 = 0.5125  →  51 %
```

The composite moves from 55 per cent to 51 per cent — a change of four percentage points to represent a
project whose entire performance measurement chain is unverified.

**This is the point of the calculation, and it is not a failure of the rule.** A single weighted average is
structurally incapable of expressing a blocking weakness, because averaging is a compensating operation:
strength elsewhere always offsets weakness here. Improving the rule cannot fix a property of the arithmetic.

### 8.5 What the deliverable should therefore be

Three things, in this order:

**The blocking finding, stated first.** *Element 4 scored 1. Earned value, both performance indices and the
forecast presented to the board are derived from unverified percentage judgements. Until independent
verification is in place on a defined sample, no performance figure from this project should be relied on for
a funding decision.*

**The element profile**, as a labelled chart or table with the rubric descriptor next to each score — never
colour alone, and never a single dial.

**The composite, last, and only as a trend line for this project against itself.** Comparing 55 per cent
here with 55 per cent on another project is meaningless unless both used the same rubric, the same weights
and a comparable sample, which is rarely true and almost never checked.

### 8.6 Closure of the previous review

The previous review raised **14 findings**. At this review: **5** closed with evidence, **6** open with
revised dates, **3** open with no movement.

```
check: 5 + 6 + 3 = 14
closure rate = 5 ÷ 14 = 0.357  →  35.7 %
```

A closure rate of 35.7 per cent with three findings showing no movement at all is itself a finding, and it
belongs in the report ahead of any new observation — because a project that does not close findings will not
close the ones raised today either, and that fact changes what this review can reasonably expect to achieve.

## 9. Checklist

**Commissioning the review**

- [ ] The question is stated as fitness for decisions, not conformance to procedure — or, if an audit is
      wanted, it is called one.
- [ ] Rubric published before the review, with observable descriptors for every score.
- [ ] Weights published and summing to 100 per cent.
- [ ] Sampling rule agreed in advance, with coverage to be reported by count and by value.
- [ ] Reviewer's independence analysed against self-review, reporting line, familiarity and advocacy; any
      impairment declared in the report.
- [ ] Access agreed: source systems, not extracts; people, not only managers.
- [ ] Timing justified — before baseline, first cycle, gate or trigger — and the review's purpose stated
      accordingly.
- [ ] Previous review's findings retrieved and their closure status tested first.

**Conducting it**

- [ ] The chain walked in order: structure, baseline, cost capture, progress, forecast, risk and change,
      reporting.
- [ ] Each score evidenced by artefact, date and test applied.
- [ ] Factual conditions agreed with the project before cause and consequence are written.
- [ ] Access limitations recorded as findings.
- [ ] Findings capped, ranked by consequence, and each routed to the person able to fix it.

**Reporting it**

- [ ] Blocking finding stated first, before any score.
- [ ] Element profile published with descriptors; no meaning carried by colour alone.
- [ ] Composite reported last, and only as a trend against this project's own history.
- [ ] Every finding carries condition, criterion, cause, consequence and correction with an owner and date.
- [ ] No correction is written that cannot be verified by inspecting a changed artefact.
- [ ] Closure rate from the previous review published.

A review run this way produces fewer findings, and the project argues with almost none of them — because the
facts were agreed before the conclusions were written and every correction names something specific. The
measure of whether it worked is not the score. It is whether the next review can start somewhere new.

---

## Related

- `TPL-15 — Project controls health check` — the review instrument, rubric structure and scoring sheet
- `BPG-05 — Schedule quality — a practical review` — the schedule element of the chain, examined in depth
- `TPL-14 — Schedule quality review checklist` — the schedule checks in usable form
- `BPG-04 — Baselining and baseline change control` — the baseline integrity element, and why pre-baseline review is the highest-value timing
- `BPG-14 — Monthly reporting that gets read` — the reporting element of the chain and what "reaches the decision" means
- `BPG-16 — Risk registers that work` — the register health metrics a reviewer can compute in minutes

## Sources and standards

- PCL-AI Body of Knowledge (`docs/bok/`), Domain 10 — Project Scheduling, first authored draft, August 2026:
  schedule health-check metrics and the schedule basis document.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 8 — Project Management Lifecycle, first authored draft,
  August 2026: stage-gate design, gate conditions tracked to closure, and the PMO's assurance role.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 4 — Performance Management, Variance Analysis and
  Management Reporting, first authored draft, August 2026: measure design, gaming patterns and the
  countermeasure of auditing the measure rather than only the result.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  naming, status and claims policy.

No maturity model, assurance framework or published review methodology is reproduced or cited here, and no
scoring threshold in this guide is attributed to any external body. The weights, scale and rubric in §8 are
illustrative and were constructed for teaching; an organisation adopting them should set its own and publish
them before its first review.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
