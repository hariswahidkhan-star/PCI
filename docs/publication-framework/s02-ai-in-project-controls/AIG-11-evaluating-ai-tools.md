---
id: AIG-11
series: S02
series_name: AI in Project Controls Guide
title: Evaluating AI tools — a buyer's due-diligence guide
subtitle: The six questions that separate tools, the trial that tests them, and the cost the licence quote leaves out
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [manager, executive, practitioner]
level: leader
reading_time_min: 13
summary: >
  A due-diligence guide for buying AI capability into a controls function, written by capability class and
  not by product. It gives the six questions that actually separate one tool from another — data residency,
  training on your data, version management, export, audit trail and failure behaviour — with the
  acceptable and evasive answers to each; the trial that tests a claim on your own data; the total cost the
  licence quote omits; the contract terms worth negotiating; and how to run a pilot that is allowed to fail.
linkedin:
  format: document
  hook: >
    Six questions separate AI tools: where your data is processed, whether it trains their model, how
    versions are managed, what you can export, what audit trail you get, and how it behaves when it fails.
  tags: [ProjectControls, Procurement, AIGovernance, DueDiligence, ResponsibleAI]
  asset: checklist-pdf
gated: false
related: [AIG-02, AIG-03, AIG-08, AIG-09, TPL-15]
bok_domains: [7, 13]
sources:
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
placeholders: 0
---

# Evaluating AI tools — a buyer's due-diligence guide

> What to ask, what to test, and what the licence quote leaves out.

**In one paragraph.** A due-diligence guide for buying AI capability into a controls function, written by
capability class and not by product. It gives the six questions that actually separate one tool from
another — data residency, training on your data, version management, export, audit trail and failure
behaviour — with the acceptable and evasive answers to each; the trial that tests a claim on your own data;
the total cost the licence quote omits; the contract terms worth negotiating; and how to run a pilot that
is allowed to fail.

**Who this is for.** Heads of project controls, PMO leads and commercial managers who specify, evaluate or
approve AI tooling, and the practitioners asked to trial it.

---

## 1. What you are actually deciding

A tool evaluation looks like a product comparison and is not one. It is three decisions taken together,
and confusing them is the commonest reason a purchase disappoints.

The first is **which capability class fits the task** — extraction, retrieval-grounded answering, tabular
analysis, forecasting, anomaly detection, drafting. Settle it before any vendor is contacted, because a
tool from the wrong class demonstrates beautifully and fails in production.
`AIG-02 — What AI actually does in a controls function` owns that decision.

The second is **whether your data can support it**, which `AIG-03 — Data readiness` owns and which
determines more of the outcome than the tool choice will.

The third — this document — is **which instance, on what terms**. It is a procurement decision, and it is
governed by the same discipline the function applies to any other contract: the claim is a hypothesis,
the terms are where the risk actually sits, and the total cost is not the quoted price.

This guide names no products. Capabilities and terms change on the vendor's release cycle, so any
recommendation would be out of date before it was read. What does not change is the question set.

## 2. Specify before you shop

Write four things down before the first demonstration. They take an hour and they change the conversation
from "what can it do" to "can it do this".

**The task, in your own words.** Not "AI for cost control" but "propose a cost code for each incoming
invoice line, at a confidence threshold, with low-confidence lines routed to a human queue".

**The maximum data classification** the tool will handle, from the project's classification map
(`AIG-08` §5). This eliminates candidates faster than any feature comparison.

**The verification tier** the outputs will sit at (`AIG-08` §4). A Tier 1 use has requirements — version
identification, reconstructable method — that a Tier 3 use does not, and a tool that cannot meet them is
unsuitable regardless of how well it performs.

**The measure of success**, as a number, with the baseline it is measured against. "Reduces coding effort"
is not a measure. "Codes at least 80 % of lines at 97 % precision or better, against a baseline of three
minutes per line" is.

## 3. The six questions

These are the questions that separate tools. Product feature lists converge; the answers to these do not.
For each, the acceptable answer, the answer that should worry you, and what to ask next.

### 3.1 Where is our data processed and stored, and under whose law?

*Acceptable:* named regions for processing and storage, named subprocessors, a commitment that the region
does not change without notice, and a statement of which jurisdiction's law governs access to the data.

*Worrying:* "in the cloud", "globally distributed", "we use industry-standard providers", or a region
commitment for storage with silence about processing.

*Ask next:* does inference run in the same region as storage? Which subprocessors see the data, and what
happens if one changes? What is the notification period for a region change?

### 3.2 Is our data used to train or improve your models?

*Acceptable:* a contractual statement that customer data is not used for training, with any exception
named; a description of retention periods for prompts, inputs and outputs; and whether the answer differs
between plan tiers, because it very often does.

*Worrying:* "your data is safe with us", an answer that covers the base model but not fine-tuning,
telemetry or "product improvement", or an assurance available only in a marketing page.

*Ask next:* is the commitment in the contract or in a policy the vendor can change unilaterally? Does it
cover human review of samples for quality purposes? What is retained, where, and for how long after
deletion is requested?

### 3.3 How are versions managed, and what happens when the model changes?

*Acceptable:* a version identifier exposed in the interface or the audit log; advance notice of model
changes; the ability to remain on a version for a defined period; and published notes on behavioural
change, not only feature releases.

*Worrying:* no version identifier at all, or "we always give you the latest and best model". Continuous
silent updating is a legitimate product decision and it is disqualifying for Tier 1 work, because a number
produced last quarter cannot be reproduced.

*Ask next:* what identifier appears in the record of an output? What notice do we get, and can we test
before the change applies? See `AIG-09` §4 for why "version not exposed" limits how a number can be
defended.

### 3.4 What can we export, in what format, and how quickly?

*Acceptable:* export of your source data, the outputs, the configuration, the mappings and rules you
built, and the audit log — in a documented, machine-readable format, on demand, without a professional
services engagement.

*Worrying:* export of raw data only. The configuration is where your investment sits: the coding rules,
the templates, the trained mappings, the tuned thresholds. A tool you can leave without your
configuration is a tool you cannot leave.

*Ask next:* can we run an export now, during evaluation, and inspect it? What is excluded? What happens to
our data at termination, and within what period?

### 3.5 What audit trail does the tool produce, and can we read it?

*Acceptable:* per-output records showing input reference, instruction or parameters, model or version,
timestamp, user, and any human edit — exportable and retained for a period you set.

*Worrying:* a usage log rather than an output log. Knowing that a user ran 40 queries on Tuesday is
telemetry; knowing which inputs produced which output under which version is an audit trail.

*Ask next:* show a real record for a real output. Can we retain it beyond the vendor's default? Does it
survive an export?

### 3.6 How does it fail?

*Acceptable:* it declines when it cannot ground an answer, returns a stated confidence or a "not found",
surfaces when a source was unavailable, and errors visibly on a malformed or partial input.

*Worrying:* it always returns an answer. A tool that never says "I do not know" has not eliminated
uncertainty; it has stopped reporting it, and it has transferred the entire detection burden to your
reviewer.

*Ask next:* show it failing. Give it a question its sources cannot answer, an input outside its range, a
document it has not indexed. What the tool does in those three cases tells you more about your review
workload than any accuracy figure.

## 4. Test it on your own data

A demonstration on the vendor's data is not evidence. Build a small evaluation set — inputs from your own
work whose correct answers have been established by your own professionals, prepared to the project's
confidentiality rules (`AIG-08` §5) and kept under version control. Fifty to a few hundred cases is
usually enough to separate candidates.

Score every candidate on the same set, and record the results with the acceptance decision. Three
disciplines make the trial worth running.

**Segment the results.** An aggregate score hides the failure that matters — good on the majority case,
poor on the minority case that is often the project in front of you. See `AIG-09` §6.

**Include the hard cases deliberately.** Not the clean examples: the ambiguous invoice, the badly scanned
drawing, the contract with a bespoke amendment, the project type you do least often.

**Keep the set.** It becomes the re-validation instrument for the life of the tool (`AIG-08` §6), and it
is never used to tune the tool it tests.

## 5. Total cost, honestly netted

The licence quote is one line of five. A value case built on it will be approved and will not be true.

| Cost | How to get it |
|---|---|
| Licence or metered usage | The quote — the only figure most business cases contain |
| Integration | Engineering effort to connect the systems the data lives in, plus ongoing maintenance |
| Data preparation | Cleaning, coding, structuring — usually the largest single item in year one (`AIG-03`) |
| Training | Everyone who will use it, plus refresher time |
| Governance and verification | The reviewer hours the framework requires, priced in `AIG-08` §10 |

The last line is the one vendors' business cases omit and the one that persists for the life of the tool.
Netting it does not usually destroy a value case; it changes what is claimed. See
`AIG-01 — AI in project controls — the executive guide` for the director-level version.

## 6. Terms worth negotiating

Six clauses, in the order they usually matter. This is contract discipline turned on the function's own
purchases.

1. **Data residency and processing location**, with notice before change.
2. **No training on customer data**, stated in the contract rather than a policy, with any exception named.
3. **Intellectual property in outputs and configuration** — who owns the outputs, and who owns the
   mappings, rules and prompts built on your data.
4. **Export and exit** — format, scope (including configuration), timescale, cost, and deletion on
   termination.
5. **Notification of material model change**, with a defined notice period and a testing window.
6. **Audit rights or third-party assurance**, proportionate to the data classification the tool handles.

Lock-in through un-exportable configuration is priced at signature or discovered at exit. There is no
third option.

## 7. What not to evaluate on

**Demonstration polish.** A rehearsed performance on curated data tells you the tool can do the task once,
under favourable conditions, with the vendor driving.

**The roadmap.** Buy what exists. A committed future feature is worth its contractual remedy, which is
usually nothing.

**The reference customer list.** Ask a reference what went wrong, not whether they are happy; only the
first question is informative.

**Benchmark scores.** Published accuracy figures were measured on someone else's data and task definition.
Your evaluation set is the only benchmark that speaks to your decision.

**Whether it uses the newest model.** The model is the least durable attribute of the product, and the one
most likely to change without you.

## 8. Piloting, and letting a pilot fail

A pilot enters the permitted-use register as Provisional, with a success measure defined in advance (§2),
an end date, a named owner and a defined data scope (`AIG-08` §3).

Two rules make a pilot worth running.

**The measure is set before the pilot starts and is not revised during it.** A measure adjusted mid-pilot
to accommodate the result has stopped being a measure.

**A pilot that misses its measure is closed, not extended.** This is the rule that costs organisations the
most to keep, because by the time a pilot ends someone senior has been associated with it publicly. The
function that can close a failed pilot cleanly, record what it learned and say so is the function whose
next business case will be believed.

Record the closure and the reason. A closed pilot with a documented reason is an asset: it stops the same
idea returning in eighteen months with the same flaw and a different vendor.

## 9. How this goes wrong

**The class was wrong and the evaluation could not tell.** A general assistant is evaluated for a task
needing a grounded retrieval tool. It demonstrates well — general assistants demonstrate well at
everything — and hallucinates in production.

**Residency answered for storage only.** The contract commits to storage in a named region; inference runs
elsewhere. Nobody asked the second question, and commercially sensitive material has crossed a border
nobody agreed to.

**Training exclusion in the wrong document.** The assurance lives in a policy page the vendor can revise,
not in the agreement. It was true when it was read.

**A silent upgrade in the middle of a reporting cycle.** Outputs shift, nobody was notified, and the
figures either side of the change are not comparable. There was no version identifier to notice it by.

**Export tested at exit.** The data comes out; the configuration does not. Two years of coding rules and
tuned thresholds stay with the vendor, and the migration cost exceeds the remaining licence cost.

**The tool that never declines.** It answers every question, so the reviewer stops expecting it to be
unable to, and an ungroundable answer passes because it looked like every other answer.

**The business case that counted the licence.** The purchase is approved on the quote. Data preparation
and verification effort arrive unbudgeted, verification is treated as optional, and the governance the
value case implicitly assumed does not happen.

**The pilot that could not fail.** The measure was never numeric, the end date passed, and the tool is now
in production because closing it would have been awkward.

## 10. Worked example — the total cost the quote did not show

*Illustrative figures.* Currency USD; a loaded internal labour rate is assumed; one tool for one controls
function; three-year horizon; integration amortised straight-line over three years; rounding to the
nearest whole currency unit.

**Setup.** A vendor quotes **USD 480 per seat per year** for **40 seats**. Internal estimates: integration
**USD 35,000** one-off; data preparation **240 hours** in year one and **60 hours** in each later year;
training **40 people × 4 hours**; verification effort **102 hours a year** (from `AIG-08` §10). Loaded
labour rate **USD 85 per hour**.

**Formulae.** `licence = seats × price per seat`; `labour cost = hours × rate`;
`amortised integration = one-off ÷ 3`.

**Substitution — year one.**

- Licence `= 40 × 480 = USD 19,200`
- Integration `= 35,000 ÷ 3 = USD 11,667` (11,666.67 rounded)
- Data preparation `= 240 × 85 = USD 20,400`
- Training `= 40 × 4 × 85 = USD 13,600`
- Verification `= 102 × 85 = USD 8,670`
- **Year one total `= 19,200 + 11,667 + 20,400 + 13,600 + 8,670 = USD 73,537`**

**Substitution — years two and three (each).**

- `= 19,200 + 11,667 + (60 × 85 = 5,100) + 8,670 = USD 44,637`

**Three-year total.** Computed from the unamortised components:
`(19,200 × 3) + 35,000 + (20,400 + 5,100 + 5,100) + 13,600 + (8,670 × 3)`
`= 57,600 + 35,000 + 30,600 + 13,600 + 26,010 = USD 162,810`.
*Rounding note:* summing the three annual figures gives `73,537 + 44,637 + 44,637 = USD 162,811`, one
dollar higher, because the amortised integration line rounds up each year. Quote one basis and say which.

**Result.** The quoted licence of **USD 19,200** is **26.1 %** of year-one cost
(`19,200 ÷ 73,537 = 0.261`) and **35.4 %** of the three-year cost (`57,600 ÷ 162,810 = 0.354`).

**Interpretation.** Nothing here argues against the purchase. It argues against approving it on the quote.
Three consequences follow. The approval goes to whoever can approve USD 162,810, not USD 57,600. The
benefit side must clear a bar nearly four times the licence in year one, which is a genuine test and
usually a passable one. And the two internal lines — data preparation and verification — are the ones that
do not appear unless someone puts them there, which is why they are also the two that are silently cut
when the year gets difficult. Note what is *not* in the table: the cost of the tool being wrong. That is
priced separately, and it is why the verification line exists rather than being an overhead to trim.

## 11. Evaluation scorecard

Score each candidate; an unanswered question is a scored answer.

- [ ] The capability class was decided before the vendor was contacted (`AIG-02`).
- [ ] The task, maximum data classification, verification tier and numeric success measure are written down.
- [ ] Processing *and* storage regions are named, with subprocessors and notice on change.
- [ ] The no-training commitment is in the contract, with exceptions named and retention periods stated.
- [ ] A version identifier appears in the output record, with advance notice of model change and a testing window.
- [ ] An export has been run during evaluation and inspected — data, outputs, **and** configuration.
- [ ] A real per-output audit record has been shown, and it survives export.
- [ ] The tool has been observed failing on an ungroundable question, an out-of-range input and an unindexed document.
- [ ] It has been scored on your own versioned evaluation set, segmented, including the hard cases.
- [ ] The cost case includes integration, data preparation, training and verification, not the licence alone.
- [ ] The six contract terms in §6 have been raised, and the answers recorded.
- [ ] The pilot has a numeric measure, an end date, a named owner and a stated data scope — and permission to fail.

The question that ends most evaluations honestly is the last one in §3.6: ask to see it fail. A vendor who
can show you that is telling you what your reviewers will actually spend their time on, which is the only
number in the whole exercise that you will still be living with in two years.

---

## Related

- `AIG-02 — What AI actually does in a controls function` — the capability class decision that precedes any tool choice
- `AIG-03 — Data readiness: what AI needs before it is any use` — the constraint that determines the outcome more than the tool does
- `AIG-08 — Governing AI on a project — the control framework` — the register, data classes and tiers a purchased tool must fit into
- `AIG-09 — Bias, explainability and auditability` — why version exposure and audit records are evaluation criteria, not nice-to-haves
- `TPL-15 — Project controls health check` — the wider assurance instrument this evaluation feeds

## Sources and standards

- PCI Body of Knowledge, Domain 7 (Contracts, Commercial Management, BoQ, Invoicing & Revenue) and Domain 13 (AI for Project Controls & Project Management), `docs/bok/` — explained in our own words, not reproduced.

No product is named, recommended or criticised in this document, and no vendor claim is reproduced.
Data-protection, data-residency and procurement obligations vary by jurisdiction and by sector; this
document describes the due-diligence discipline and is not legal advice.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
