---
id: AIG-09
series: S02
series_name: AI in Project Controls Guide
title: Bias, explainability and auditability
subtitle: If you cannot reconstruct how a number was produced, you cannot defend it
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: professional
reading_time_min: 13
summary: >
  Bias, explainability and auditability are controls problems, not ethics-seminar topics. This document
  sets the standard a project must meet — an AI-influenced number that cannot be reconstructed cannot be
  defended in a claim or an audit — then gives the fields of a reconstruction record, the places bias
  actually enters a controls workflow, how to test for skew and drift, and what to do when a tool cannot
  be explained at all.
linkedin:
  format: article
  hook: >
    An AI-assisted forecast you cannot reconstruct is not a forecast you can defend. Auditability is not
    paperwork about the model; it is the difference between a number and an assertion.
  tags: [ProjectControls, Auditability, Explainability, Forecasting, ResponsibleAI]
  asset: one-pager
gated: false
related: [AIG-08, AIG-10, AIG-04, BPG-09, CMP-08]
bok_domains: [6, 13]
sources:
  - "PCI Body of Knowledge, Domain 6 — Earned value management and forecasting (Institute manuscript, 2026)"
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
placeholders: 0
---

# Bias, explainability and auditability

> The evidence standard an AI-influenced number has to meet before it leaves the project.

**In one paragraph.** Bias, explainability and auditability are controls problems, not ethics-seminar
topics. This document sets the standard a project must meet — an AI-influenced number that cannot be
reconstructed cannot be defended in a claim or an audit — then gives the fields of a reconstruction
record, the places bias actually enters a controls workflow, how to test for skew and drift, and what to
do when a tool cannot be explained at all.

**Who this is for.** Cost engineers, planners, forecasting and risk analysts who produce AI-assisted
numbers, and the controls managers, auditors and assurance reviewers who have to stand behind them.

---

## 1. The question that settles it

An AI-assisted estimate at completion (EAC) is challenged eleven months later, in a claim, an internal
audit or a board paper. One question decides whether it survives: **can a competent person, using the
record you kept, reproduce how that number was arrived at — and say why it is what it is rather than
something else?**

That question is not about ethics or technology. It is the same evidential standard controls has always
applied to a manual forecast: source data, method, assumptions, the reason for the method, the reviewer,
the date. AI changes nothing about the standard. It changes how easy it is to fail it, because the
working that used to sit in a spreadsheet now sits inside a system that does not keep it and cannot be
re-run to the same answer.

Three things follow, and they are the substance of this document. **Explainability** is the property that
makes reconstruction possible. **Auditability** is the record that makes reconstruction possible *later*,
when memories have gone and the model has changed. **Bias** is the systematic error that reconstruction is
most likely to expose, and least likely to expose by accident.

## 2. Three kinds of explanation, and which one is worth having

"The tool is explainable" is claimed at three quite different depths. Know which one you have.

**Mechanism.** An account of how the model type works — a regression on historical outturn, a
classification against learned patterns, a language model predicting text. Useful for choosing a
capability class; useless in a challenge. It explains the machinery, not the number.

**Drivers.** An account of which inputs moved this particular output, and by how much: "the forecast rose
because the labour productivity input fell and the remaining duration extended". This is what most
vendors mean by explainability, and it is genuinely valuable — it lets a professional judge whether the
model is responding to the things that actually changed.

**Reconstruction.** The number can be rebuilt from stated inputs by a stated method, arriving at the same
answer within a stated tolerance, without the model. This is the only level that meets the evidential
standard of §1.

Reconstruction does not require the model to be simple. It requires the *reported* number to be
attributable. In practice that means one of three arrangements: the model's method is a known formula
applied to identifiable inputs, so it can be recomputed by hand; or the model's output is used as an
input to a human method that is itself reconstructable; or the model's proposal is checked against a
conventional calculation, and the conventional calculation is what is reported, with the model's
proposal recorded as corroboration. The third arrangement is the honest answer for opaque tools, and it
is treated in §8.

## 3. The reconstruction record

A verification record that says "verified: yes" is worthless in a challenge. These are the fields that
make one load-bearing. One record per material AI-assisted output, filed with the output.

| Field | Content | Why a challenge needs it |
|---|---|---|
| Output reference | The number or artefact, and where it was reported | Ties the record to what left the project |
| Permitted-use reference | The `AI-U-nn` row it was produced under | Shows the use was approved, and under which rules |
| Tool and version | Tool instance and model or version identifier; "not exposed" where the vendor gives none | Behaviour changes with version; without it, later re-runs prove nothing |
| Input data | The datasets and cut-off date or period; where they were extracted from | The commonest cause of an unreproducible number is an unrecorded data cut |
| Method and parameters | The method the model applied, and any parameter that changes the answer | "Which method" is the first question an auditor asks about a forecast |
| Assumptions | Every assumption the answer depends on, stated in the same breath as the answer | An unstated assumption is how a defensible number becomes an indefensible one |
| What the model produced | The unedited output, retained | Distinguishes the model's proposal from the human's judgement |
| What changed in review, and why | The delta and its reason | This is the field that proves a human actually reviewed it |
| Verification performed | Which tier (`AIG-08` §4), what was recomputed, against what source | A named check, not a tick |
| Named sign-off and date | The individual, not the team | A model cannot be accountable; a person can |

Two rules about the record. It is written **at the time**, because a reconstruction record assembled after
a challenge is worth very little and looks worse. And it is retained to the project's records-retention
schedule: a number whose evidence has been deleted is, for practical purposes, unsupported.

## 4. What breaks an audit trail

Four failures account for most unreconstructable numbers, and all four are avoidable at negligible cost.

**The moving data cut.** The record names a dataset but not the period or extract date. The dataset has
since been updated, so re-running the analysis gives a different answer and the original cannot be
recovered. Record the extract date and, for anything at Tier 1, retain the extract itself.

**The unversioned tool.** The tool has been upgraded; the vendor exposes no version identifier; the record
notes only the tool's name. Nobody can now say whether a different answer today reflects a data change or
a model change. Where a vendor exposes nothing, record that fact — and treat "version not exposed" as a
factor in the tool's suitability for Tier 1 work (`AIG-11` §3).

**The lost prompt or configuration.** For assistant-class and retrieval tools, the instruction given is
part of the method. A record that keeps the output but not the instruction has kept the answer and thrown
away the question.

**The invisible human edit.** The reported number differs from the model's output and nothing records the
difference or its reason. Under challenge, this is the worst of both worlds: the professional cannot show
they exercised judgement, and cannot show they did not simply overwrite an inconvenient result.

## 5. Where bias actually enters a controls workflow

Bias in a controls context is not primarily a question of protected characteristics — though where AI
influences decisions about people, that question is live and is governed by employment and data-protection
law that varies by jurisdiction. The dominant risk in cost, schedule and risk work is **systematic error
inherited from the data**, and it has specific, findable homes.

**Historical outturn that encodes one era.** A model trained on projects delivered in a period of stable
prices will under-forecast in a period of escalation. It is not wrong about the past; it is being asked
about a future the past does not contain.

**A portfolio mix that is not your project.** Training data dominated by one project type, size, region or
contracting model will systematically misjudge a project outside that mix. The failure is quiet: the
model produces a confident number, and its confidence carries no information about how unlike its training
data your project is.

**Optimism embedded in the labels.** If the historical record was itself produced under pressure to report
favourably — approved forecasts that were consistently late to recognise overrun — a model learns to
reproduce that optimism. It is a faithful model of a biased practice.

**Survivorship in the record.** Cancelled scopes, abandoned packages and terminated subcontracts often
leave the dataset. A model trained on what completed will understate the cost of what does not.

**Scoring that recycles a reputation.** Subcontractor or supplier risk scoring trained on historical
performance data will penalise parties who were previously given the difficult work, and reward those who
were not. The score is a measure of past allocation as much as past capability, and where it influences
selection it is also a commercial and legal exposure.

**Assumption inheritance.** Productivity rates, waste allowances and duration norms carried from prior
projects into a model's inputs are assumptions, not facts, and they are rarely re-examined once they are
inside a tool.

## 6. Testing for skew

Bias is found by segmenting, not by asking whether a tool is fair. Take the evaluation set used to accept
the tool — a sample of cases whose correct answers were established by professionals and kept under
version control — and score performance **by segment** rather than in aggregate.

Segment on the dimensions your work actually varies along: project type, contract form, value band,
region, delivery stage, and discipline. An aggregate accuracy figure conceals exactly the failure you care
about, which is that the tool performs well on the majority case and badly on the minority case — and the
minority case is often the project in front of you.

Two disciplines make this real. The evaluation set is **never used to tune the model it tests**, or the
score stops measuring anything. And segment results are recorded with the acceptance decision, so a
reviewer six months later can see the tool was accepted *knowing* it was weaker on, say, refurbishment
work — which is a legitimate decision, and an illegitimate surprise.

## 7. Drift

A model that was right last year can be silently wrong this year. Nothing announces it. The portfolio
mix changes, the cost breakdown structure is restructured, a market moves, the vendor updates the model,
and performance degrades while the interface looks identical.

The control is a **re-run on a cadence**: the same versioned evaluation set, scored again, compared with
the previous run, on the schedule and triggers set in the permitted-use register (`AIG-08` §6). A change
in score is investigated before anyone re-tunes a threshold — because re-tuning to restore the old score
without knowing why it moved converts a detectable problem into a hidden one.

Drift monitoring has an owner and a date, or it does not exist. The most common form of this failure is
not a missing procedure; it is a procedure with no name against it.

## 8. When explainability is unobtainable

Some tools will not give you reconstruction, and no amount of governance will extract it. This is a
decision point, not a reason to give up on the tool.

**Do not use it as the reported number.** An opaque output does not go into a baseline, a board forecast,
a contractual position or a disclosure — see `AIG-10 — Human in the loop: what AI may and may not decide`.

**Use it as a screen, a challenge or a corroboration.** An unexplainable model that flags projects for
attention, or that disagrees with your conventional forecast and prompts you to look again, is useful and
carries no evidential burden. What is reported is the conventional calculation; the model's disagreement
is recorded as a reason the calculation was re-examined.

**Say so in the record.** "Model output not reconstructable; used as corroboration only; reported figure
derived by [method]" is a complete and defensible entry. What is not defensible is an opaque number
reported as though it were reconstructable.

## 9. How this goes wrong

**Explainability accepted at the wrong depth.** The vendor demonstrated driver attribution; the project
assumed that meant reconstruction. The first time a number is challenged, the team can say which inputs
moved and cannot rebuild the figure.

**The record written after the challenge.** Nobody kept a reconstruction record. When the number is
queried, one is assembled from memory and calendar entries. It is probably accurate. It reads as
constructed, because it was.

**Bias discussed, never tested.** The function has a thoughtful position on AI bias and has never
segmented an evaluation set. Aggregate accuracy is good; performance on the two project types that make up
this year's growth has never been measured separately.

**Drift assumed away.** The tool was validated at deployment and never re-scored, because nothing in the
project changed. The vendor's model was updated twice in the period.

**Sign-off by team.** The record names "Project Controls" as reviewer. Under challenge, no individual can
say what they checked. A model cannot be accountable and neither can a department.

**Reconstruction defeated by the data cut.** Every other field is complete, but the record names a live
dataset with no extract date. Re-running produces a different answer, and the original working cannot be
recovered.

**The human edit that erased the evidence.** The professional corrected the model's output — correctly —
and reported the corrected figure without recording the change. The correction is now invisible, and so is
the review that produced it.

**Explainability treated as an ethics matter.** The topic is delegated to a policy team, discussed as
principle, and never turned into fields on a form. Nothing changes in how numbers are produced.

## 10. Worked example — reconstructing an AI-produced forecast

*Illustrative figures.* Currency USD; figures as at the month-end data cut; indices shown to four decimal
places; the division is performed on the unrounded index and the result rounded to the nearest whole
currency unit.

**Setup.** A forecasting tool reports an EAC of **USD 12,497,917** for a control account. The reviewer has
the source data: budget at completion (BAC) **USD 11,900,000**, actual cost (AC) **USD 6,300,000**, earned
value (EV) **USD 5,950,000**. The tool states no method.

**Step 1 — reconstruct by the standard method.** The cost performance index and the CPI-based EAC:

- `CPI = EV ÷ AC = 5,950,000 ÷ 6,300,000 = 0.9444`
- `EAC = AC + (BAC − EV) ÷ CPI = 6,300,000 + (11,900,000 − 5,950,000) ÷ 0.944444…`
- `= 6,300,000 + 5,950,000 ÷ 0.944444… = 6,300,000 + 6,300,000 = USD 12,600,000`

*Rounding note.* Using the four-decimal index instead of the unrounded one gives
`5,950,000 ÷ 0.9444 = 6,300,296`, an EAC of `USD 12,600,296` — a rounding difference of USD 296. State
which convention was used; do not let a rounding difference be mistaken for a method difference.

**Step 2 — quantify the gap.** `12,600,000 − 12,497,917 = USD 102,083`. The tool's figure is USD 102,083
lower than the cumulative-CPI method.

**Step 3 — find the method.** Solving for the index the tool must have used:
`(BAC − EV) ÷ (EAC − AC) = 5,950,000 ÷ (12,497,917 − 6,300,000) = 5,950,000 ÷ 6,197,917 = 0.9600`. That is
not the cumulative CPI of 0.9444; it is the three-period rolling CPI, which the reviewer confirms from the
period data.

**Result.** The tool applied a **three-period rolling CPI of 0.9600** rather than the cumulative 0.9444.
The difference is a method choice, not an error.

**Interpretation.** The reconstruction has converted an unexplained number into a professional judgement
that can be made, recorded and defended. Which index basis is right depends on whether the variance driver
is recent and closed or systemic and continuing — that judgement belongs to
`AIG-04 — AI-assisted cost forecasting` and `BPG-09 — Estimate at completion`, and it is exactly the
judgement an unreconstructed number silently makes on the professional's behalf. What the record must now
carry is the method identified, the basis chosen, the reason, and the USD 102,083 the choice is worth.
Note also what the reconstruction cost: three lines of arithmetic. Numbers go out unreconstructable
because nobody asked, not because reconstruction was hard.

## 11. Checklist

- [ ] The project knows which of the three explanation depths (§2) each tool actually provides — and does not confuse driver attribution with reconstruction.
- [ ] A reconstruction record with the fields in §3 exists for every material AI-assisted output.
- [ ] Records are written at the time, not assembled on challenge, and retained to the records schedule.
- [ ] Every record names the data extract date, and Tier 1 work retains the extract.
- [ ] Prompts and configuration are captured for assistant and retrieval tools.
- [ ] Every difference between model output and reported figure is recorded with its reason.
- [ ] Sign-off names an individual.
- [ ] The evaluation set is segmented on project type, contract form, value band, region and stage, and segment results are recorded with the acceptance decision.
- [ ] The evaluation set is versioned and is never used to tune the model it tests.
- [ ] Drift re-runs have a named owner, a cadence and a trigger list; a score change is investigated before any re-tuning.
- [ ] Where reconstruction is unobtainable, the tool is used only as screen or corroboration and the record says so.

The test is not whether your organisation trusts the tool. It is whether, eleven months from now, a
person who was not in the room can rebuild the number from what you wrote down.

---

## Related

- `AIG-08 — Governing AI on a project — the control framework` — the register, tiers and change control this record sits inside
- `AIG-10 — Human in the loop: what AI may and may not decide` — why an unreconstructable output cannot carry a decision
- `AIG-04 — AI-assisted cost forecasting` — the forecasting methods this document holds to account
- `BPG-09 — Estimate at completion — choosing and defending a method` — the underlying method choice, without AI in the picture
- `CMP-08 — Data, digital and AI competencies in depth` — the competence a reconstruction presupposes

## Sources and standards

- PCI Body of Knowledge, Domain 6 (Earned Value Management & Forecasting) and Domain 13 (AI for Project Controls & Project Management), `docs/bok/` — explained in our own words, not reproduced.

Where AI influences decisions about people — recruitment, allocation, performance — data-protection and
employment law apply and vary by jurisdiction. This document describes the controls discipline; it is not
legal advice, and it does not state any jurisdiction's requirements.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
