---
id: AIG-02
series: S02
series_name: AI in Project Controls Guide
title: What AI actually does in a controls function
subtitle: A capability map by class of work, and an honest account of where the capability is weak
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 12
summary: >
  A map of what artificial intelligence genuinely does inside a project controls function, organised by
  capability class rather than by product: extraction, classification, retrieval-grounded answering,
  anomaly detection, forecasting from tabular data, simulation support, drafting and code generation. Each
  class is matched to the controls tasks it fits, with the verification the professional still owes. The
  second half is the part most maps omit — where the capability is weak, why causal reasoning, novel scope
  and contractual judgement remain human, and how to triage a task between a rule, a model and a generative
  tool.
linkedin:
  format: carousel
  hook: >
    Most AI disappointment in project controls comes from choosing the wrong class of capability for the
    task — not from the tool being bad at its job.
  tags: [ProjectControls, ArtificialIntelligence, CostEngineering, Planning]
  asset: carousel-8
gated: false
related: [AIG-01, AIG-03, AIG-04, AIG-05, AIG-11]
bok_domains: [13]
sources:
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
placeholders: 0
---

# What AI actually does in a controls function

> A capability map by class of work, and an honest account of where the capability is weak.

**In one paragraph.** A map of what artificial intelligence genuinely does inside a project controls
function, organised by capability class rather than by product: extraction, classification,
retrieval-grounded answering, anomaly detection, forecasting from tabular data, simulation support,
drafting and code generation. Each class is matched to the controls tasks it fits, with the verification
the professional still owes. The second half is the part most maps omit — where the capability is weak, why
causal reasoning, novel scope and contractual judgement remain human, and how to triage a task between a
rule, a model and a generative tool.

**Who this is for.** Cost engineers, planners, project controls managers and PMO leads deciding where to
apply AI first, and the managers who must approve their choice.

---

## 1. Map the capability, not the product

Product capability changes faster than any document can track, and a feature list is not evidence that a
tool does the thing on your data. What is stable enough to plan against is the **class** of capability:
what kind of problem the underlying technique is good at, what it needs as input, and how it fails. A
professional chooses the class that fits the task, then finds a governed tool inside that class — and
tests it. The tool test belongs to `AIG-11 — Evaluating AI tools`; the class choice belongs here, and it is
where most of the value or waste is decided.

Three terms are needed and then used. **Machine learning (ML)** is the family of techniques that learn
patterns from data rather than following written rules. **Generative AI** is the subset that produces new
content — text, tables, code — of which large language models are the familiar instance. **Retrieval-augmented
generation (RAG)** is the pattern in which relevant documents are retrieved and given to a generative model
at the moment of answering, so the answer is grounded in your material and can cite it, rather than being
recalled from whatever the model absorbed in training.

## 2. The eight classes that matter in controls

| Class | What it does | Controls tasks it fits | What the professional still owes |
|---|---|---|---|
| **Extraction** | Pulls structured fields out of unstructured documents | Contract terms, notification windows, rates and retention from subcontracts; quantities from a bill of quantities; dates from correspondence | Open the cited clause and confirm every extracted value; reject anything ungrounded |
| **Classification and coding** | Assigns a category to a record | Coding invoice and timesheet lines to work breakdown structure (WBS) and cost element; sorting correspondence by contract event | Own the coding rules; work the exception queue; audit a sample of the confidently-coded |
| **Retrieval-grounded answering (RAG)** | Answers a question over your own document set, with citations | "What do our contracts say about notification periods?"; "what does our procedure require at gate 3?" | Open the citations; confirm the corpus is current and permissioned |
| **Anomaly and pattern detection** | Flags records that differ from the population | Duplicate invoices, out-of-tolerance postings, approval bypasses, unusual timesheet patterns | Set and justify the thresholds; investigate flags; know the miss rate as well as the hit rate |
| **Forecasting from tabular data** | Projects a value from historical and current data | Cost at completion candidates, delay likelihood, cash-flow shape, quantity growth | Choose and defend the method; confirm the assumption matches the cause; own the number |
| **Simulation support** | Runs and interprets probabilistic models | Monte Carlo on a quantified risk register; schedule risk analysis; scenario ranges | Own the input ranges, the correlations and the confidence level chosen |
| **Drafting and summarisation** | Produces first-cut language from data or documents | Variance narratives, basis-of-estimate text, meeting actions, board summaries | Recompute every figure; confirm every causal claim; delete what the data does not support |
| **Code and query generation** | Writes scripts, formulas and queries from a description | Data preparation, recurring reconciliations, report assembly, one-off analyses | Test against known cases before use; review it as code; never run it untested against live data |

Two classes deserve a warning label. **Drafting** is where hallucination — confident, fluent, false content —
does its damage, because prose hides a wrong figure better than a spreadsheet does. **Code generation**
fails silently: generated code that is subtly wrong looks exactly like generated code that is right.

## 3. The map against the controls calendar

The same eight classes, arranged the way the work actually arrives.

| When | The task | Class that fits | Realistic contribution |
|---|---|---|---|
| Set-up | Building the cost breakdown structure and code of accounts | None — this is design | AI can check consistency once you have decided; it cannot decide |
| Set-up | Normalising historical projects for estimating | Classification, extraction | Substantial: the tedium of mapping old codes to a current structure |
| Estimating | A parametric check estimate from analogues | Forecasting from tabular data | A challenge figure, never the estimate of record |
| Estimating | Drafting the basis of estimate | Drafting | First draft only; assumptions are the estimator's |
| Monthly | Coding and reconciling cost | Classification, anomaly detection | High value, comparatively low risk — the strongest first pilot |
| Monthly | Proposing accruals from goods received not invoiced | Classification with rules | Proposal only; service-date discipline is human |
| Monthly | Variance narrative | Drafting | Speed, with every figure recomputed |
| Monthly | Cost at completion candidates and early warning | Forecasting from tabular data | Genuine early warning; see `AIG-04` |
| Monthly | Schedule health and logic checking | Anomaly detection, rules | Very high value; see `AIG-05` |
| Monthly | Progress-based slip prediction | Forecasting from tabular data | A second opinion on the critical path analysis, not a replacement |
| Commercial | Portfolio sweep for notification windows and claim triggers | Extraction, RAG | Coverage no team can match; entitlement stays human |
| Risk | Candidate risks from analogous history and correspondence | Extraction, pattern detection | Fills gaps in a register; cannot see novel risk |
| Risk | Quantification and simulation | Simulation support | Runs the model; the inputs and the chosen confidence level are yours |
| Reporting | Assembling the pack, answering questions over it | RAG, drafting, BI querying | Fast, and dependent on one governed definition per metric |
| Closeout | Extracting lessons from records | Extraction, summarisation | Good at surfacing candidates; the lesson is a judgement |

Nothing in that table changes who signs. The shape is constant: **governed input → AI step → the
professional's verification → an output that a named person owns.**

## 4. Where the capability is weak

This is the half of the map that matters most, because expectations set here determine whether an
initiative is judged a success.

**Causal reasoning.** A model learns association. It will report that cost rose when a particular
subcontractor was on site; it cannot tell you that the subcontractor caused it, or that both followed from
a late design release. In controls, cause is the deliverable — a variance narrative that misattributes is
worse than no narrative, because it directs the recovery action at the wrong thing.

**Novel scope.** Learning from history requires comparable history. First-of-a-kind work, a new
contracting model, an unfamiliar jurisdiction or a technology with no delivery record gives a model nothing
to learn from, and models rarely announce that they are extrapolating. The professional's judgement about
comparability is the control here, and it cannot be automated.

**Contractual judgement.** Extraction finds the words; entitlement is a legal question about facts, notices
and conduct. A model can tell you that a clause exists and where; it cannot tell you whether you are
entitled under it. Entitlement-bearing extractions go to a qualified reviewer before they move a commercial
position.

**Arithmetic across several steps.** Language models predict plausible continuations, and a chain of
calculation is exactly where plausible and correct come apart. Arithmetic belongs in a spreadsheet, a
script or a calculation engine — with the model used to draft or explain the working, not to be it.

**Change in the world.** A model learned a portfolio mix, a market and a coding structure. When any of
those shift, performance degrades quietly rather than failing loudly. Monitoring is not optional for
anything that feeds a decision; `AIG-09 — Bias, explainability and auditability` covers what to monitor and
how to evidence it.

**Accountability.** Unchanged and unchangeable: a model cannot be questioned, cannot be sanctioned and
cannot sign. Every output remains explainable, validated and owned by a competent human.

## 5. Rule, model or generative tool — a triage

The most expensive error on this map is reaching for a model where a rule belongs. Rules are transparent,
testable, cheap to audit and do not drift. Use them wherever the logic is known.

- **Use a rule** when the decision is deterministic and stateable: an invoice whose price differs from the
  purchase order beyond tolerance; an activity with no successor; a posting to a closed period; a change
  above a delegated authority. If you can write it down, write it down.
- **Use machine learning** when there is a pattern in data worth learning that you cannot state as a rule:
  which cost lines are likely duplicates given vendor, amount and timing; which activities historically
  slip; which projects overrun given their features.
- **Use a generative tool** when the task is producing or transforming language: drafting, summarising,
  extracting from documents, explaining. Ground it in your documents where the answer must come from them,
  and verify.
- **Use nothing** when the data is inadequate, when confidentiality cannot be assured, when a rule already
  works, or when the verification the stakes demand would cost more than the task. Deciding not to use AI is
  a professional judgement, and functions that never make it are not exercising one.

## 6. How this goes wrong

**A general assistant is asked to do precise arithmetic.** It produces a beautifully formatted table of
figures, some of which are wrong. The class was wrong: this was a spreadsheet task with a drafting layer on
top, not a drafting task.

**A document question is asked without grounding.** Without retrieval over your actual contracts, the model
answers from general patterns and invents a plausible clause reference. The failure looks like a correct
answer, which is why ungrounded extraction is the most dangerous single habit in this list.

**The pilot is chosen for visibility rather than fit.** Executive dashboards and natural-language querying
demonstrate well and depend on the cleanest data and the most contested definitions. Coding and
reconciliation demonstrate poorly and work. Pilot where the class fits and the data exists.

**Thresholds are set to make the exception queue small.** An anomaly detector tuned until it stops
complaining has been tuned into uselessness. The queue size is a consequence of the threshold, and the
threshold is a control decision with an owner, not a comfort setting.

**Only the hit rate is measured.** A detector that is right about 90 % of what it flags may still be missing
half of what is there. Both numbers are needed before anyone decides the manual check can stop.

**Capability is assumed to transfer.** A model that codes cost well on one portfolio is deployed to another
with a different coding structure and a different vendor population, and nobody re-tests. Class fit is
general; performance is local.

**The professional's step is described but not staffed.** Every row of §2 has a "what the professional still
owes" column. If nobody's time is allocated to that column, the map has been read as an automation plan
rather than as a division of labour.

## 7. Worked example — what coverage is worth

*Illustrative figures.* A commercial team holds 48 live subcontracts. Each carries roughly 140 pages of
contract and, over the year, some 220 pages of correspondence, requests for information (RFIs) and
notices — the material in which a missed notification window hides.

`Total pages = 48 × (140 + 220) = 48 × 360 = 17,280 pages`

At a sustained review rate of 30 pages an hour — a generous assumption for careful reading — full human
coverage costs:

`17,280 ÷ 30 = 576 hours`, which at 8 hours a day is `576 ÷ 8 = 72 person-days`

No commercial team of ordinary size reads 17,280 pages looking for something that may not be there.
Sampling is what actually happens, and sampling means the missed window is found late or not at all.

An extraction-and-retrieval workflow reads all of it and returns, say, nine subcontracts with delay-notice
language and an approaching window. The professional then reads nine, opens each cited clause, confirms
each date against the contract, and refers the two with material exposure for legal review.

**Read it honestly.** The saving is not 72 person-days, because the 72 days were never being spent. What
changed is that a control which was previously infeasible is now feasible — the class of capability that
matters here is coverage, not cleverness. The correct value argument is the exposure avoided by not missing
a window, which is specific to your contracts and should be argued with your own numbers, not asserted.
And the workflow only works if the extraction is grounded in the documents and cited; an ungrounded sweep
returns nine confident answers that may include a clause that does not exist.

## 8. Checklist — triage a task in five questions

Use this before a tool is chosen, not after.

1. **Is the logic stateable?** If yes, write a rule and stop. Rules beat models on transparency,
   auditability and cost every time they are available.
2. **What class is this?** Name it from §2. If the answer is "a bit of everything", the task is not yet
   defined tightly enough to automate or to test.
3. **Does the data exist, and is it fit?** Which system, which fields, how far back, how clean. If this
   cannot be answered in a sentence, go to `AIG-03 — Data readiness` before going to a vendor.
4. **What does the professional still owe?** Name the verification step, the person and the frequency.
   Write it into someone's workload before the pilot starts.
5. **How will we know it is still working in six months?** Name the measure, the sample and the cadence. A
   capability with no monitoring plan is a capability with an unknown current performance.

---

## Related

- `AIG-01 — AI in project controls — the executive guide` — the approval requirements this map supports.
- `AIG-03 — Data readiness: what AI needs before it is any use` — what question 3 of the checklist actually
  requires.
- `AIG-04 — AI-assisted cost forecasting` — the forecasting class, worked through in method detail.
- `AIG-05 — AI in scheduling — and what must not be automated` — the anomaly-detection and forecasting
  classes applied to the schedule.
- `AIG-11 — Evaluating AI tools — a buyer's due-diligence guide` — testing a specific product once the
  class is chosen.

## Sources and standards

- **PCI Body of Knowledge, Domain 13** — *AI for project controls and project management* (Institute
  manuscript, 2026), particularly its tool-category map and its account of strengths and hard limits, which
  this document restates for practitioners in capability-class terms.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
