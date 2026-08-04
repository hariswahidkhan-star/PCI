---
id: AIG-08
series: S02
series_name: AI in Project Controls Guide
title: Governing AI on a project — the control framework
subtitle: Permitted uses, approval authority, records, confidentiality and model change control, written as project controls
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [manager, executive, practitioner]
level: leader
reading_time_min: 14
summary: >
  A corporate AI policy does not govern a project; project controls do. This document sets out the six
  control objects that make AI use on a project defensible — a permitted-use register, verification tiers,
  approval authority, the verification record, data classification and tool-and-model change control —
  gives the clauses to write into the project execution plan, and shows how to size and budget the
  verification effort the framework creates.
linkedin:
  format: document
  hook: >
    A corporate AI policy tells people to be careful. A project controls framework tells them which task,
    with which data, in which tool, verified how, signed by whom — and keeps the record.
  tags: [ProjectControls, AIGovernance, ProjectExecutionPlan, RiskManagement, ResponsibleAI]
  asset: one-pager
gated: false
related: [AIG-09, AIG-10, AIG-11, ETH-05, TPL-01]
bok_domains: [12, 13]
sources: []
placeholders: 0
---

# Governing AI on a project — the control framework

> The six control objects that make AI use on a project defensible, and the clauses that put them in the project execution plan.

**In one paragraph.** A corporate AI policy does not govern a project; project controls do. This document
sets out the six control objects that make AI use on a project defensible — a permitted-use register,
verification tiers, approval authority, the verification record, data classification and tool-and-model
change control — gives the clauses to write into the project execution plan, and shows how to size and
budget the verification effort the framework creates.

**Who this is for.** Project controls managers, project directors, PMO leads and heads of function who
have to answer for AI-assisted numbers, and the assurance and audit staff who will test the answer.

---

## 1. The gap a corporate policy leaves

Most organisations that have adopted AI have written a policy: confidential data must not be entered into
public tools, outputs must be verified, staff remain accountable. All correct, and none of it governs a
project.

A project needs to know: on *this* project, which task may use AI, drawing on which data, in which tool,
verified to what standard, approved by whom, with what recorded, retained for how long. A corporate policy
answers none of those questions. It states a principle; a project needs a control.

It is the distinction controls professionals already make about cost. "We will manage cost carefully" is
a policy; a cost breakdown structure, a change control procedure, delegated authority thresholds and a
monthly reconciliation are controls. AI governance fails for the reason cost control fails: the principle
was written and the control was not.

This document is the control set — the Institute's position, **AI proposes; the professional disposes**,
turned into artefacts with owners, frequencies and fields.

## 2. Six control objects

Everything that follows attaches to one of six objects. If your framework has all six, it is auditable.
If it has fewer, name which one is missing and why.

| # | Control object | What it fixes | Owner (typical) | Cadence |
|---|---|---|---|---|
| 1 | **Permitted-use register** | Which task, capability class, data class and tool | Project controls manager | Reviewed at each stage gate; entries added on request |
| 2 | **Verification standard (tiers)** | How hard an output is checked before use | Project controls manager | Set at mobilisation; changed by documented decision only |
| 3 | **Approval authority** | Who may decide what, at what threshold | Project director | Set at mobilisation; aligned to delegated authority |
| 4 | **Verification record** | Evidence that the check happened | Output owner (named) | Per material output |
| 5 | **Data classification** | What may go into which tool | Information manager | Inherited; mapped to project artefacts at mobilisation |
| 6 | **Tool and model change control** | What happens when the tool changes under you | Named tool owner | Re-validation on trigger and on a stated calendar |

Objects 3 and 4 are owned elsewhere in this series: the decision boundary and authority thresholds in
`AIG-10 — Human in the loop: what AI may and may not decide`, the audit-trail content in
`AIG-09 — Bias, explainability and auditability`. This document owns objects 1, 2, 5 and 6, and the
assembly of all six into the project execution plan.

## 3. The permitted-use register

The register is the operative document — not a list of approved products but a list of **approved uses**,
because the same tool is appropriate for one task and reckless for another. One row per permitted use:

| Column | Content | Why it is there |
|---|---|---|
| Use reference | `AI-U-nn` | So a verification record or an audit finding can cite it |
| Task | The controls task in the function's own words, e.g. "draft first-cut control account variance narrative" | A task, not a capability — "summarisation" is not a permitted use |
| Capability class | Assistant, retrieval-grounded, tabular analysis, forecasting model, process automation, transcription | Ties the row to its class-specific failure mode; see `AIG-02` |
| Approved tool | The named instance, and whether it is the governed enterprise deployment | The same product ungoverned is a different control |
| Maximum data classification | The highest classification permitted in this use | The most commonly breached rule |
| Grounding required | Yes/no, and against what corpus | Ungrounded and grounded answers carry different risk |
| Verification tier | 1, 2 or 3 (§4) | Fixed in advance, not chosen under deadline |
| Output owner role | The role accountable for outputs from this use | Accountability assigned before the output exists |
| Approved on / review due | Dates | Enforces §6 re-validation |
| Status | Approved · Provisional (pilot) · Suspended | A suspended row is more useful than a deleted one |

Three rules give the register its force.

**Absence is prohibition.** A use not in the register is not permitted — the only formulation that
survives contact with a busy team. "Use your judgement" produces an unbounded surface with no audit trail.

**Provisional means measured.** A pilot enters as Provisional with a defined success measure and an end
date. A pilot that misses its measure is closed, not extended — see `AIG-11 — Evaluating AI tools` §8.

**Requests are cheap and logged.** A framework people cannot add to is a framework people work around, so
add a lightweight request route to the register owner and log refusals with a reason. The refusal log is
the most informative document in the framework after six months.

## 4. Verification tiers

The most common governance failure is a uniform rule — "all AI outputs must be verified" — which is
either unaffordable or ignored. Set three tiers and assign one to every register row.

**Tier 1 — Independent recomputation.** The reviewer reproduces the number by an independent route
without reference to the model's working, then compares; assumptions, method and the reason for the method
are recorded. Applied to anything reported outside the project, anything entering a baseline or a forecast
that funds a decision, and anything with a contractual effect.

**Tier 2 — Source verification.** Every figure and extracted item is traced to its source document or
record, every causal claim checked against the underlying analysis, and anything the model could not
ground is deleted rather than softened. Applied to internal analysis, extraction, narrative drafting over
verified data and register enrichment.

**Tier 3 — Sampled acceptance with monitoring.** High-volume, low-unit-consequence output — coded
transaction lines, metadata enrichment — accepted on a defined sample with an error threshold and
monitored for drift on a stated cadence. Sample size, threshold and the action on breach are written down
before the first run.

Three points decide whether tiers work.

- **The tier is a property of the use, not of the day.** It is set in the register when the use is
  approved. Nobody chooses their own tier at 17:00 on a reporting day.
- **A tier is lowered only by documented decision** from the register owner, on measured error rate — not
  on elapsed time without incident. Lowering a tier is a control change and is logged as one.
- **Tier 3 is not "no verification".** It replaces per-item review with a sample plus a monitoring
  obligation, and the monitoring is the part that gets dropped. Name the person who reads the drift number
  and the month they read it.

## 5. Confidentiality of commercially sensitive data

A project holds four things that make confidentiality more than a compliance formality: unit rates and
build-ups, subcontract terms and commercial positions, claims and dispute strategy, and personal data in
correspondence and resource records. In joint ventures, some of it is confidential *between the parties on
your own project*.

**Map the classification before mobilisation, not at first use.** Write down which project artefacts fall
into which class of the organisation's existing scheme: cost model, risk register, subcontract set,
correspondence file, resource plan, claims file. The register's "maximum data classification" column is
meaningless until this map exists.

**The pre-entry test.** Before material enters any tool, the person entering it answers three questions:
what is this material's classification; is this tool approved for that classification; would the data
owner be content to watch me do this. The third catches what the first two miss — notably joint-venture
and client data whose classification nobody has set.

**Preparation is a control.** Removing names, rates or party identifiers before analysis often converts a
prohibited use into a permitted one at negligible cost. Record what was removed: the analysis's
limitations follow from it.

**Residency and training are procurement questions.** Where data is processed, under whose law, and
whether it improves the vendor's models are settled at purchase — see `AIG-11` §3. The register records
the answer; it does not discover it. And a retrieval system indexed across the project must inherit the
underlying access model, tested with a named unprivileged account before go-live.

## 6. Tool and model change control

This is the control object most frameworks omit, and the one that ages an otherwise sound framework into
a false assurance.

The system you validated is not the system you are using. Hosted models are updated by their vendors;
platform features change on the vendor's release cycle; a retrieval corpus changes every time someone
files a document; a forecasting model's world changes as the portfolio changes. Nothing in your project
changed, and the behaviour did. Four controls follow.

**Version identification.** The verification record captures the tool, and the model or version identifier
where the vendor exposes one. Where the vendor exposes nothing — common for embedded platform features —
record that fact: "version unknown" is itself a finding, and it caps how far a number can be
reconstructed later.

**Re-validation triggers.** Re-validate on any of: a vendor-notified model or feature change; a change to
the retrieval corpus's scope or permissions; a change to an upstream data structure such as the cost
breakdown structure or the code of accounts; a project stage gate; a failed sample under Tier 3; and a
stated calendar cadence for anything at Tier 1. Write the trigger list into the plan.

**Re-validation method.** Re-run a versioned set of test cases whose correct answers were established by
professionals, and compare with the previous run — the same instrument used to evaluate the tool at
purchase, kept and re-used. `AIG-09` §7 covers what a change in the results means.

**Change is announced, not discovered.** The register's tool owner tracks vendor release notes and tells
users when behaviour has changed. Users who discover a change by getting a different answer have already
relied on the old one.

## 7. What goes in the project execution plan

The framework lives in the project execution plan, or the project controls plan where the function keeps
one. Ten clauses, numbered so they can be cited in an assurance finding, to be adopted and adapted — see
`TPL-01 — Project controls execution plan`.

**AI-1 Scope.** Which parties the section binds — staff, contractors, secondees, consultants and, where
the contract allows, subcontractors producing controls deliverables — and that accountability for every
output rests with a named individual, not a tool.

**AI-2 Permitted-use register.** Where it lives, who owns it, how a use is added, and the rule that a use
not in the register is not permitted.

**AI-3 Data classification and prohibited data.** The project's classification map (§5), the maximum
classification per approved tool, and material that may not enter any tool on this project.

**AI-4 Verification tiers.** The three tiers, what each requires, and that a tier is assigned in the
register and lowered only by documented decision on measured evidence.

**AI-5 Approval authority and sign-off.** The decision boundary and thresholds, cross-referenced to the
project's delegated authority schedule so the two cannot diverge. See `AIG-10`.

**AI-6 Disclosure.** Where AI assistance is disclosed — client, board, auditor, certifier — and in what
form. Contractual and reporting requirements govern; where they are silent, the project states its own
rule rather than leaving it to individuals.

**AI-7 Records and retention.** What a verification record contains, where it is filed, and for how long.
Retention matches the project's records-retention schedule: an AI-assisted number that outlives its
evidence is undefendable.

**AI-8 Tool and model change control.** The four controls of §6, with the named tool owner and the trigger
list.

**AI-9 Incidents and near-misses.** What is reported, to whom, within what period; that a near-miss is
reported as readily as an incident; and that the register and plan are updated where an incident shows a
gap.

**AI-10 Review cadence and ownership.** Who owns this section, when it is reviewed, and that everyone
using AI on the project is briefed on it before first use.

## 8. Knowing whether the framework is working

Three tests, none of which is "we have a policy".

**The reconstruction test.** Pick an AI-assisted number that left the project last quarter and ask its
named owner to reconstruct it: source data, method, what the model produced, what changed in review, who
signed, when. More than an hour means the record is not doing its job.

**The refusal test.** Read the refusal log. No refusals in six months means the framework is either not
being used or not being consulted — both findings.

**The incident test.** Read the incident log. Zero incidents *and* zero near-misses across an active
function is not a good result: near-misses are not being reported, and the first thing you will learn
about is an incident that reached the client.

## 9. How this goes wrong

**Policy without register.** An AI policy exists and no project-level permitted-use register. Everyone is
"being careful", nobody can say which tasks use AI on what data, and nothing is either auditable or
prohibited.

**The register nobody can add to.** Additions require a committee that meets monthly. Within one reporting
cycle staff have gone around it, and the shadow use is unmonitored, unverified and invisible — worse than
the use the framework meant to prevent.

**Uniform verification, quietly abandoned.** "Everything is verified" survives four weeks. Because no tier
distinguishes a board forecast from a metadata field, the rule is impossible, so it is ignored — and it is
ignored for the board forecast too.

**Verification recorded as a tick.** "Verified — yes" proves nothing; under challenge the reviewer cannot
say what they recomputed or against what. `AIG-09` §3 sets out the fields that make a record load-bearing.

**Governance costed at zero.** The business case counted licences and time saved, not the verification
hours the framework requires, so it arrives with no budget and is treated as optional. §10 prices this
deliberately.

**The silent upgrade.** A tool's model is updated, outputs shift, and nobody re-validated because nothing
in the project changed. Three months of numbers rest on an assurance that expired without notice.

**Confidentiality by exhortation.** The rule is "no confidential data in public tools", but the project
never classified its own artefacts. Staff judge sincerely about material they have no classification for,
and the joint-venture partner's rates end up in a public assistant.

**The framework as a shield.** Documents exist, boxes are ticked, and nobody has ever refused an output,
re-validated a tool or investigated a near-miss. That is worse than nothing: it manufactures confidence.

## 10. Worked example — pricing the verification the framework creates

*Illustrative figures.* One project, one month, then annualised. Currency stated as USD; loaded labour
rate assumed; rounding to the nearest whole unit.

**Setup.** The register lists nine uses producing material outputs each month: **four at Tier 1** (monthly
cost forecast, contingency drawdown analysis, schedule-risk input pack, client cost report) and **five at
Tier 2** (variance narratives, register enrichment, contract-term extraction, exception commentary,
risk-register update). Tier 1 verification averages **90 minutes** per output, Tier 2 **30 minutes**.
Loaded cost of the reviewing grade: **USD 85 per hour**.

**Formulae.** `monthly minutes = (Tier 1 count × Tier 1 minutes) + (Tier 2 count × Tier 2 minutes)`;
`annual hours = (monthly minutes ÷ 60) × 12`; `annual cost = annual hours × loaded rate`.

**Substitution.**

- Tier 1 `= 4 × 90 = 360` minutes
- Tier 2 `= 5 × 30 = 150` minutes
- Monthly `= 360 + 150 = 510` minutes `= 510 ÷ 60 = 8.5` hours
- Annual `= 8.5 × 12 = 102` hours
- Annual cost `= 102 × 85 = USD 8,670`

**Result.** The framework costs **102 reviewer-hours and USD 8,670 a year** on this project, before any
tool licence.

**Interpretation.** Three things follow from having the number. It goes in the controls budget as a line,
so verification is resourced rather than expected. It goes in the value case as a cost netted against
whatever the AI use saves — a case that omits it is not a case. And it is the honest test of scope: if the
register grows to thirty material outputs a month at these rates, verification is
`(4 × 90) + (26 × 30) = 360 + 780 = 1,140` minutes, or **19 hours a month**. At that point the function
resources it, moves lower-consequence uses to Tier 3 with monitoring, or removes uses from the register.
What it must not do is keep the register and abandon the tier, which is how §9's third failure begins.

## 11. Checklist

- [ ] A permitted-use register exists for this project, with an owner and a review date.
- [ ] Every row states task, capability class, tool instance, maximum data classification, grounding, verification tier and output-owner role.
- [ ] "A use not in the register is not permitted" is written in the plan.
- [ ] A request route into the register exists; refusals are logged with reasons.
- [ ] Three verification tiers are defined and every row carries one; lowering a tier requires a documented decision on measured evidence.
- [ ] The data classification map exists and names actual project artefacts.
- [ ] Each approved tool has a maximum data classification, and recorded residency and training-on-your-data answers.
- [ ] Retrieval permissions have been tested with a named unprivileged account.
- [ ] Each tool has a named owner, a re-validation trigger list and a versioned test set.
- [ ] Verification records are filed where an auditor can find them, retained to the project's records schedule.
- [ ] Clauses AI-1 to AI-10 are in the project execution plan.
- [ ] Verification effort is a costed line in the controls budget.
- [ ] An incident and near-miss route exists, and someone has read the log this quarter.

When an AI-assisted number is challenged — by a client, an auditor, an adjudicator or a board — the answer
should be a record and a name produced within the hour, not a search for who ran what, in which tool, on
which day. That is the whole purpose of the framework.

---

## Related

- `AIG-09 — Bias, explainability and auditability` — what the verification record must contain for a number to be reconstructable
- `AIG-10 — Human in the loop: what AI may and may not decide` — the decision boundary and authority thresholds that control object 3 depends on
- `AIG-11 — Evaluating AI tools — a buyer's due-diligence guide` — how residency, training and versioning answers are obtained before a tool reaches the register
- `ETH-05 — The ethical use of AI and data` — the professional-conduct obligations underneath the controls
- `TPL-01 — Project controls execution plan` — the plan these clauses are written into

## Sources and standards

- PCI Body of Knowledge, Domain 13 (AI for Project Controls & Project Management), `docs/bok/` — the governance, assurance and maturity material this document draws on; explained here in our own words.
- The Institute's candidate AI-use policy (`docs/downloads/`) — the governing position on AI in preparation, examination and practice.

Standards named but not reproduced or summarised in detail: ISO/IEC 42001 (management systems for
artificial intelligence) and ISO 31000 (risk management) are the natural reading for a function
formalising this at organisational rather than project level. This document is a project controls
framework, not a management-system standard, and is not a route to conformity with either.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
