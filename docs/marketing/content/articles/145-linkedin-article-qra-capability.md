---
platform:      LinkedIn Article
type:          guide
title:         QRA capability: how to build it into an organisation
meta:          QRA capability is data, method, tooling, governance and competence. How to build each one, how to calibrate your models, and how to measure AI assistance.
primary_kw:    QRA capability
secondary_kw:  quantitative risk analysis, contingency governance, model calibration, precision and recall
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,801
hashtags:      #RiskManagement #ProjectControls #AIGovernance #CostEngineering
ab_id:         AB-00226
---

# QRA capability: how to build it into an organisation

QRA capability is the organisational ability to produce a quantitative risk analysis that is repeatable, auditable and believed. It has five parts: input data, a documented method, tooling, governance over contingency, and assessed competence. Buying a simulation tool builds none of them, which is why so many organisations own the software and not the capability.

Written for LinkedIn as an original. It sits under the Institute's risk management pillar.

## What does a QRA capability consist of?

Five components, and a weakness in any one of them makes the other four unusable. The pattern is consistent across organisations: tooling is bought first, governance is added last, and the gap in between is where models lose credibility.

| Component | What good looks like | How it fails in practice |
|---|---|---|
| Input data | A maintained risk register with quantified exposure, three-point ranges owned by the people who do the work, outturn data from completed projects | Ranges invented by the modeller the week before the gate |
| Method | A written procedure covering ranging, correlation, discrete risks, iteration counts and reporting conventions | Every analyst does it differently, so results are not comparable |
| Tooling | One agreed toolchain with version control over models and inputs | Spreadsheets on individual laptops, no audit trail |
| Governance | Stated funding confidence level, drawdown authority, gate re-runs, a named owner of contingency | Contingency treated as a slush fund at project level |
| Competence | Assessed skills, not attendance at a course | The one person who understands the model leaves |

The order to build in is data, method, governance, competence, tooling. That is roughly the reverse of how most organisations do it.

## How do you know whether your models are any good?

Calibrate them against completed projects. A confidence level is a testable claim, and almost nobody tests it.

Take twenty completed projects that were funded at a stated **P80**. If the models were well calibrated, roughly **16 of 20** should have finished at or below that figure, because that is what P80 means.

Suppose only **8 of 20** did. Your P80 behaved like a P40, and the corrective action is not a better tool. It is wider ranges, honest correlation, and a look at whoever signs off the base estimate.

Twenty projects is a small sample and a run of unusual years can move it, so treat the check as a signal rather than a verdict. Repeat it annually and the trend becomes far more informative than any single reading.

Do the same test on schedule. Count how many projects finished at or before their P80 date, and compare the two calibration results. Organisations are commonly better calibrated on cost than on time, because time optimism is socially rewarded.

## What does QRA maturity look like at each level?

Four levels, each recognisable by what an auditor could actually find in the files.

| Level | What happens | Evidence you would find | What it costs you |
|---|---|---|---|
| Ad hoc | A QRA is run when a gate demands one | A model file, no procedure, no inputs traceable to owners | Contingency argued by seniority |
| Repeatable | One method, applied by a small central team | A written procedure, models under version control | Results comparable within the team only |
| Governed | Confidence level set by policy, drawdown rules enforced, gate re-runs mandatory | Contingency register, drawdown approvals, re-run records | Reliable portfolio position |
| Predictive | Outturn data feeds the ranges; calibration measured annually | Calibration results, revised range guidance, closed feedback loop | Models the board treats as evidence |

Most organisations that believe they are governed are repeatable, because the drawdown rule exists in a policy document but no one has ever refused a drawdown.

## Who owns contingency?

Someone who is not the project manager spending it. That single separation does more for capability than any modelling improvement.

Set the funding confidence level as policy, hold the difference between the project level and the portfolio level centrally, and require a named risk to be cited on every drawdown. Contingency released against scope growth is an unapproved budget increase, and [a quantified register people actually maintain](https://projectcontrolsinstitute.org/risk-register-that-gets-used) should show that distinction plainly.

Re-run the model at gates and at defined triggers, such as a major risk realising or a significant change to the base estimate. Monthly re-runs turn the exercise into a report nobody reads, and annual ones are too late to change a decision.

## Where does AI fit, and how do you measure it?

The useful application is not simulation, which is already automated. It is identification: reading progress narratives, change logs, correspondence and site reports to flag emerging risks a human reviewer might not have raised in the workshop.

Anything that flags things must be measured on how well it flags, and that means precision, recall and F1 rather than a vendor's accuracy claim.

Suppose a model flags **120** items in a quarter. **84** turn out to be genuine emerging risks, so precision is 84 ÷ 120 = **0.70**.

Over the same quarter there were **140** genuine emerging risks in total. The model caught 84, so recall is 84 ÷ 140 = **0.60**.

F1 is the harmonic mean of the two: 2 × (0.70 × 0.60) ÷ (0.70 + 0.60) = 0.84 ÷ 1.30 = **0.65**.

Now the judgement that F1 cannot make for you. F1 weights a false alarm and a missed risk equally, and on a capital project they are not equal: a false alarm costs a reviewer twenty minutes, while a missed risk can cost a quarter's margin.

So state which error is more expensive in your context, weight recall accordingly, and report the two numbers separately rather than hiding them inside a single score. A model at 0.60 recall is missing two risks in five, and that sentence is the one the risk committee needs to hear. Which decisions such a model may originate, and which it may only inform, belongs in writing before any of it reaches a board pack; [an AI policy template for project controls teams](https://pciai.org/ai-policy-for-project-controls) sets out the clauses to settle.

Governed AI is the third element of the Body of Knowledge behind PCI's credentials, whose proportions are 40 per cent finance and reporting, 40 per cent project management and 20 per cent governed AI. The reason it is only 20 per cent is that the other 80 per cent is what the model has to be checked against.

## What competence does the organisation actually need?

Three distinct skills, and they rarely sit in one person. Someone who can facilitate a ranging workshop without leading the witness. Someone who can build and interrogate the model. Someone who can defend the output to a finance director who has a different definition of the word contingency.

Assess these rather than counting training days. A practical test works better than any certificate: give a candidate a model with a deliberate error in the correlation settings and see whether they find it.

Certification helps where it examines both sides of the boundary. The PCI AI Project Controls Leader (PCL-AI) credential covers 13 domains and 61 knowledge areas across delivery and finance, which is the range this work sits in. PCI's own examinable arithmetic is machine-verified: 15,613 calculation checks covering PFL-AI and PML-AI material, all passing, with no equivalent suite for PCL-AI.

The Institute's requirements are set out as 113 mandatory PCI Standards carrying 532 process requirements. These are certification requirements established by the Institute, not law, and they exist so that a process claim can be tested rather than asserted.

## How do you build this in a year?

Sequence it so each step produces something usable, because a capability programme that delivers nothing for nine months will be cancelled in month seven.

**Quarter one.** Write the method. Twelve pages covering ranging, correlation, discrete risks, iteration convention, reporting format and the confidence level policy. Apply it to two live projects.

**Quarter two.** Fix the inputs. Ranges owned by discipline leads, risks quantified in the register rather than scored on a colour scale, and a review of who has ever refused a range as too narrow.

**Quarter three.** Turn on governance. Named contingency owner, drawdown rules with a cited risk, gate re-runs mandatory, and one refusal on the record so people know the rule is real.

**Quarter four.** Calibrate. Pull the last twenty completed projects, count how many landed at or below their stated confidence level, and publish the answer internally even if it is embarrassing. That publication is the moment a capability becomes credible.

## Frequently asked questions

**Do we need a central risk team or embedded analysts?**
Both, in a specific split. A small central team owns the method, the calibration and the tooling, and embedded analysts run the models on projects. Central-only produces work nobody on the project believes; embedded-only produces results that cannot be compared across a portfolio.

**How much does a QRA capability cost to run?**
Less than the tooling budget suggests, because the expensive parts are people's time in ranging workshops and governance meetings rather than licences. Size it against the contingency being decided: an organisation setting nine-figure contingency positions on an unexamined model has a mismatch between the decision and the effort behind it.

**Can spreadsheets do this properly?**
For cost, often yes, provided the model is version-controlled, the assumptions are documented, and correlation is handled explicitly rather than ignored. Schedule risk analysis needs a tool that runs the network. The failure is not the spreadsheet, it is the absence of an audit trail from input to output.

**How do we stop the QRA becoming a gate-passing exercise?**
Give it a decision to serve other than gate approval. Contingency drawdown, funding confidence level and the recovery case for a late programme are all decisions the model can inform. A model that only ever appears at gates will be tuned to pass gates.

**What is the single first step for an organisation with nothing?**
Quantify the risk register. Replace probability and impact colour scores with cost and duration ranges owned by named people. Everything else in a QRA capability depends on that data existing, and no tool, method or governance layer can compensate for its absence.

---

*PCI publishes certification requirements. The PCI Standards are certification requirements established by the Institute, not law, and nothing here is legal, tax or accounting advice. All figures describing projects and models above are illustrative arithmetic, not organisational data.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Linking note: two cross-estate links now sit in the body. The hub link to the risk register guide sits in the contingency ownership section, because that sentence asks the register to show a distinction most registers cannot, and the guide is about building one that is maintained rather than filed. The pciai.org link sits in the section on measuring a flagging model, where the piece raises what the model may and may not decide but does not settle it. The note originally proposed three hub links, to the schedule risk analysis pillar, the Monte Carlo guide and the risk register guide; two were dropped because a piece may carry only one link to any one domain, and the register is the input the whole capability depends on.*
