---
id: CMP-08
series: S03
series_name: Competency Frameworks
title: Data, digital and AI competencies in depth
subtitle: The chain from data readiness to human accountability, and the evidence that proves each link
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, employer]
level: professional
reading_time_min: 15
summary: >
  All three PCI credentials carry a group of data, digital and AI competencies, and in every case they
  form one chain: data readiness, analysis, workflow design, reporting and automation, governance, and the
  human validation or accountability that closes it. This document sets out what each link requires, what
  distinguishes practitioner from professional, and how each is evidenced by a decision rather than by a
  tool. It includes a worked back-test showing how to state a model's error and its bias.
linkedin:
  format: article
  hook: >
    A forecasting model with a 7.6 % typical error and a 3.4 % low bias is telling you two different
    things, and only one of them is fixed by better data. Knowing which is the competency.
  tags: [AIinProjectControls, ResponsibleAI, DataQuality, Forecasting, Competency]
  asset: checklist-pdf
gated: false
related: [CMP-03, CMP-04, CMP-05, AIG-03, AIG-09, AIG-10]
bok_domains: [13]
sources:
  - PCI platform certification catalogue — seeded competency sets for PCL-AI, PFL-AI and PML-AI, backend/Data/MultiCert.cs, verified August 2026
placeholders: 1
---

# Data, digital and AI competencies in depth

> The chain from data readiness to human accountability, and the evidence that proves each link.

**In one paragraph.** All three PCI credentials carry a group of data, digital and AI competencies, and in
every case they form one chain: data readiness, analysis, workflow design, reporting and automation,
governance, and the human validation or accountability that closes it. This document sets out what each
link requires, what distinguishes a practitioner from a professional, and how each link is evidenced by a
decision rather than by a tool. It includes a worked back-test showing how to state a model's typical error
and its bias, and what to do with each.

**Who this is for.** Controls, finance and delivery professionals who use AI-assisted methods in work that
someone relies on; heads of function deciding what to admit into their process; and assessors reviewing
evidence in this cluster.

---

## 1. The chain, and where each credential's competencies sit on it

The Institute's position is a single sentence, and everything below is its operating detail: **AI
proposes; the professional disposes.** An output is fit to be relied upon when it is explainable,
validated and owned by a competent human.

The competencies that carry that position are distributed across the three credentials, but they occupy
the same six positions:

| Link | PCL-AI | PFL-AI | PML-AI |
|---|---|---|---|
| Data foundation | *implicit in* predictive analytics | *implicit in* predictive cash-flow analysis | *implicit in* digital delivery |
| Analysis | Predictive analytics | Predictive cash-flow analysis; digital due diligence | Decision intelligence |
| Workflow design | AI-enabled project controls | AI-enabled financial modelling | AI-enabled project management |
| Carriage | Digital reporting; automation | *within* digital due diligence | Digital delivery; automation |
| Governance | Responsible AI | Responsible AI | Responsible AI |
| Closure | Human validation | Human validation | Human accountability |

Two observations that matter for assessment. First, the data foundation is not itself a named competency
in any of the three sets — it sits inside the analysis competencies, which means an assessor has to probe
for it deliberately or it goes unexamined. Second, the closing link differs: PCL-AI and PFL-AI name *human
validation* (the act of checking), while PML-AI names *human accountability* (answerability for the
outcome, including for checks performed by others). The distinction is real and is drawn in §7.

*AIG-01* to *AIG-12* own the Institute's teaching on AI in a controls function. This document is about
what competence in it looks like and how it is proved.

## 2. Data readiness, and the question that exposes it

Every analytical or AI-assisted method inherits the quality of what it was given. The professional-level
marker is the ability to state, before running anything, what the available data can and cannot support.

The four properties that decide it:

- **Consistency of definition over time.** A cost series in which the coding structure changed eighteen
  months ago is two series wearing one name. Any trend across the join is an artefact.
- **Completeness of the periods that matter.** Missing data is rarely random. The periods most often
  missing are the disrupted ones, which are exactly the periods a risk or forecast model most needs.
- **Granularity relative to the question.** Monthly totals cannot answer a question about weekly
  productivity, however sophisticated the method applied.
- **Independence from the answer.** Data that was itself produced by a process the answer will judge —
  progress claimed by the party being assessed — carries the incentive with it.

The evidence is a document in which someone declined or reshaped a request: *"the question as asked
cannot be answered from this data; here is what it can support, and here is what would have to be
collected to answer the original question."* That artefact is rare and it is nearly conclusive of
professional-level competence. *AIG-03 — Data readiness* covers the underlying assessment.

## 3. Analysis: the competency is the error statement, not the number

Anyone can produce a forecast. The competency is producing one accompanied by an honest account of how
wrong it usually is.

Three habits distinguish the levels:

**Hold-out testing.** Fit on part of the history, test on the part withheld, report the result. A model
evaluated only on the data it was fitted to has been described, not tested.

**Separating noise from bias.** These are different problems with different remedies. Noise — a model that
is wrong by a similar amount in both directions — is reduced by better data or a better method. Bias — a
model that is consistently wrong in one direction — can be corrected arithmetically once measured, and
usually points to something structural. §9 works both numbers from the same four observations.

**Stating the range and its basis.** A single-point analytical output invites the reader to treat it as
precise. A range with a stated basis invites them to treat it as a forecast, which it is.

The recurring false positive is high explanatory power over history. A model that fits the past closely is
easy to build and easy to sell; whether it predicts anything is a separate question, answered only out of
sample. A candidate who has reported that their own model was wrong, and by how much, is demonstrating
something a candidate who reports only successes cannot.

## 4. Workflow design: where the output enters, and what stands between it and reliance

This is the operating-model competency — *AI-enabled project controls*, *AI-enabled financial modelling*,
*AI-enabled project management* — and it is the one most often evidenced with a list of tools, which
answers a question nobody asked.

Designing an AI-assisted step means specifying six things, and the specification is the evidence:

1. **The task**, narrowly. "Summarise the 40 supplier reports into an exceptions list against these five
   criteria" is a task. "Help with reporting" is not.
2. **The input contract** — what the model receives, in what form, and what it must never receive
   (personal data, priced commercial positions, counterparty confidential material).
3. **The output contract** — the form the output must take, so that it can be checked mechanically where
   possible. Output that must be read to be verified costs more to check than to produce.
4. **The entry point** — precisely where in the process the output is used, and by whom.
5. **The check** — what stands between the output and reliance, described in §7.
6. **The failure mode** — what a wrong output looks like here, and what it would cause. Some tasks fail
   loudly and cheaply; others fail silently into a board paper.

The decisive professional-level marker is an **exclusion**: a task deliberately not delegated, with the
reason recorded. Every competent design has some. A workflow with no exclusions has not been designed; it
has been adopted.

## 5. Carriage: reporting and automation, and the problem of silent failure

Digital reporting and automation carry the output to the point of use, and they share one failure mode
that separates the levels: **failing quietly while continuing to produce plausible numbers.**

The competency is expressed in build decisions:

- **Lineage.** Every figure in a report traces to a source system, a field and an extraction time. Where
  it cannot, that figure is manual and should be labelled as such.
- **Reconciliation as a control, not an activity.** An automated pipeline that reconciles its own totals to
  the source and stops on a difference is a control. One that reconciles when someone remembers is not.
- **Break-detection sized to the error that matters.** A row-count check catches a truncated extract; it
  does not catch a mapping error that silently reassigns cost to the wrong account. Design the check for
  the failure that would be expensive, not the failure that is easy to detect.
- **Survivability.** Documented, owned, version-controlled, and demonstrably operable by someone who did
  not build it. An automation that only its author can maintain has converted a manual process into a
  single-person dependency and called it efficiency.

The assessment question is one sentence: *when this breaks, how will anyone know?* If the honest answer is
"when someone notices the number looks odd", the automation is not at professional level however elegant
its construction.

## 6. Governance: responsible AI as a decision, not a document

Responsible AI appears in all three sets. Its content is well covered in *AIG-08 — Governing AI on a
project* and *AIG-09 — Bias, explainability and auditability*; what matters here is what evidences it.

Four decisions constitute the competency:

- **Data boundaries.** Which categories of information may be entered into which class of tool. The
  boundary is only real if someone has been told no.
- **Disclosure.** Where AI assistance is disclosed to a reader whose reliance would change if they knew.
  A convention applied consistently — including to a board paper, and including when it invites an awkward
  question — is the evidence.
- **Explainability as a precondition.** For some decisions, an unexplainable output is inadmissible
  regardless of accuracy, because the decision has to be defended to someone entitled to an explanation.
  Knowing which of your decisions those are is a professional-level judgement.
- **Prohibition.** At least one use case declined. A framework that has never prevented anything is a
  document.

An open question for this framework, recorded rather than assumed:
[CONFIRM: whether AI assistance must be declared in candidate-submitted competence evidence, and in what
form].

## 7. Closure: validation, and the difference between validating and being accountable

**Human validation** (PCL-AI, PFL-AI) is the act: checking an AI-derived or automated output before
reliance, and owning it as your own work. A check is capable when four things are true:

1. **It is independent of the thing being checked.** Re-running the same model, or asking the same model
   whether it is right, is not a check. The independent source may be a different system, a physical
   observation, a manual recomputation of a sample, or a person who did not see the first answer.
2. **It could actually fail.** If no realistic output would be rejected, the check is a formality. A useful
   discipline is to specify the tolerance before seeing the output.
3. **It is sized to the risk.** Full recomputation for a board forecast; a stated sample for a routine
   extraction — with the sample size and selection method recorded, because an unstated sample is an
   anecdote.
4. **It leaves a record.** What was checked, against what, by whom, on what date, and what was found —
   including the times nothing was found, because a record showing only clean checks is a record of a
   process nobody ran.

**Human accountability** (PML-AI) is broader: being answerable for an outcome, including for work checked
by others and produced by systems. It has three components — a named owner for every output that reaches a
decision, the refusal to release what you cannot stand behind, and the absence of deflection to "the model
said" or "the system produced it". The organisational version is the design obligation: no decision may be
taken by a process with no owner. A function where every individual validated correctly and nobody was
accountable for the aggregate has an accountability gap by design, and it will be discovered by an
incident rather than by an audit.

## 8. How this goes wrong

**Tools are offered as evidence of competence.** A list of platforms describes a procurement history. Every
competency in this cluster is evidenced by a decision: what enters where, what check stands between the
output and reliance, what was excluded, what was prohibited.

**The check is performed by the same system that produced the output.** Asking a model to review its own
work produces agreement, which is not verification. The independence requirement in §7 is the whole point.

**Validation is a signature.** "Reviewed by" with no statement of what was checked against what records
that a signature occurred. A single line — *"totals reconciled to the ledger extract of 30 June; 10 of 240
line codings sampled, one error found and corrected"* — turns it into evidence.

**A model's fit is reported as its accuracy.** Explanatory power over history is a statement about the
past. Out-of-sample error is a statement about the model. Only the second belongs next to a forecast.

**Bias is treated as noise.** A model that has run 3 % low for two years does not need more data; it needs
a correction and an investigation into the structural cause. Reporting only absolute error hides this
entirely, which is why §9 computes both.

**Automation is measured by hours saved.** Hours saved is a benefit claim. The competency question is what
happens when it fails, and a silent failure in an automated pipeline can cost more than the automation
ever saved.

**Disclosure is applied selectively.** AI assistance is disclosed in routine documents and omitted from the
paper that matters, on the grounds that it would raise questions. That is precisely the document where the
reader's reliance would change, and therefore the one where disclosure is owed.

**Responsible AI is evidenced by a policy.** A policy is a document until it prevents something. Name the
prohibited use case and the reasoning.

**The data foundation is never examined.** Because no credential names data readiness as a standalone
competency, it is the easiest link to skip — and it is the link on which every other link in the chain
depends. Ask what the data can support before asking what the model concluded.

## 9. Worked example: stating a model's error and its bias

*Illustrative figures.* A cost forecasting model has produced a monthly forecast for four completed
periods. Currency-neutral cost units (cu). Percentage errors are expressed **as a percentage of actual**
and shown to two decimal places; money rounded to the nearest 1,000 cu.

| Period | Forecast (cu) | Actual (cu) | Error = actual − forecast (cu) | Error as % of actual |
|---|---|---|---|---|
| 1 | 1,200,000 | 1,320,000 | +120,000 | +9.09 % |
| 2 | 900,000 | 855,000 | −45,000 | −5.26 % |
| 3 | 1,500,000 | 1,725,000 | +225,000 | +13.04 % |
| 4 | 1,100,000 | 1,067,000 | −33,000 | −3.09 % |

Substitutions for the percentage column:
`120,000 ÷ 1,320,000 = 0.0909 = +9.09 %` · `−45,000 ÷ 855,000 = −0.0526 = −5.26 %` ·
`225,000 ÷ 1,725,000 = 0.1304 = +13.04 %` · `−33,000 ÷ 1,067,000 = −0.0309 = −3.09 %`

**Typical size of error** — mean absolute percentage error (MAPE), the average of the errors ignoring sign:

`MAPE = (9.09 + 5.26 + 13.04 + 3.09) ÷ 4 = 30.48 ÷ 4 = 7.62 %`

**Direction of error** — mean percentage error, the average keeping the sign:

`MPE = (+9.09 − 5.26 + 13.04 − 3.09) ÷ 4 = 13.78 ÷ 4 = +3.45 %`

The two numbers say different things. The model is typically wrong by **7.62 % of actual**, and it is
wrong **low** by **3.45 %** on average — so roughly `3.45 ÷ 7.62 = 45 %` of its typical error is
systematic rather than random. Noise needs a better method or better data; bias needs a correction and an
explanation.

**Applying it to a live forecast.** The model now outputs 2,400,000 cu for the current period. Because the
error is measured as a proportion of actual, the bias-corrected central estimate is:

`corrected = 2,400,000 ÷ (1 − 0.0345) = 2,400,000 ÷ 0.9655 = 2,486,000 cu` (to the nearest 1,000)

— an uplift of `2,486,000 − 2,400,000 = 86,000 cu`. A working band of plus or minus the typical error
around that central estimate gives:

`2,486,000 × 0.0762 = 189,000` → range `2,297,000 to 2,675,000 cu`

**What this example does not establish.** Four observations cannot demonstrate that a bias exists; the
apparent 3.45 % could easily be four periods of ordinary variation. The MAPE band is a working range, not
a statistical confidence interval, and it assumes future errors resemble past ones — which fails precisely
when a project changes character. The teaching point is the discipline, not the figures: **a forecast
presented without its error history is an opinion with decimal places**, and the professional-level
behaviour is to keep the back-test, report both numbers, and say how many observations sit behind them.

## 10. Checklist

Before relying on an analytical or AI-assisted output, or when assessing evidence in this cluster:

- [ ] What question is this answering, and can the underlying data support that question?
- [ ] Has any definition in the data series changed during the period being analysed?
- [ ] Was the model tested on data it had not seen?
- [ ] Are both the typical error and the directional bias reported, and over how many observations?
- [ ] For each AI-assisted step: task, input contract, output contract, entry point, check, failure mode —
      all specified?
- [ ] Which tasks were deliberately **not** delegated, and is the reason recorded?
- [ ] Is the check independent of the thing being checked, and could it realistically fail?
- [ ] Is the check sized to the risk, with sample size and selection recorded?
- [ ] When the automation breaks, how will anyone know — and has that detection ever fired?
- [ ] Does every figure in the report trace to a source, field and extraction time?
- [ ] Is AI assistance disclosed to readers whose reliance would change if they knew — including in the
      documents where the question would be awkward?
- [ ] Is there a named person accountable for every output that reaches a decision?

---

## Related

- `CMP-03 — PCL-AI: the fourteen competencies` — predictive analytics, AI-enabled project controls, digital reporting, automation, responsible AI, human validation
- `CMP-04 — PFL-AI: the nineteen competencies` — AI-enabled financial modelling, predictive cash-flow analysis, digital due diligence
- `CMP-05 — PML-AI: the twenty-four competencies` — AI-enabled project management, decision intelligence, digital delivery, human accountability
- `AIG-03 — Data readiness: what AI needs before it is any use` — the assessment behind §2
- `AIG-09 — Bias, explainability and auditability` — the governance content behind §6
- `AIG-10 — Human in the loop: what AI may and may not decide` — the decision boundary behind §7

## Sources and standards

- PCI platform certification catalogue (`backend/Data/MultiCert.cs`), verified August 2026 — the seeded
  competency sets from which this cluster is drawn.
- PCI Body of Knowledge domain 13, *AI for Project Controls and PM: concepts, tools and practice*
  (`docs/bok/`) — cited by domain; not reproduced.
- The error measures used in §9 (mean absolute percentage error and mean percentage error) are standard
  published definitions described in our own words. All figures are illustrative and recomputed from first
  principles.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
