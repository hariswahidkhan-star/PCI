---
id: BPG-16
series: S09
series_name: Best Practice Guides
title: Risk registers that work
subtitle: Turning a compliance artefact back into a management instrument
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 15
summary: >
  Most risk registers are maintained and not used. This guide sets out what separates the two: risk
  statements built as cause, event and effect; why an effect such as "delay to project" cannot be managed
  as a risk; what qualitative probability-impact scoring can and cannot order; ownership and action dates
  as the register's live parts; the link between the register and contingency; and the health metrics that
  tell you, in a few minutes, whether your register is a management tool or wallpaper. The worked example
  shows two risks with identical matrix scores whose expected values differ by a factor of more than three.
linkedin:
  format: article
  hook: >
    Two risks on your register score 8 out of 25 on the matrix and sit in the same amber cell. One has an
    expected value of 60,000; the other 200,000. The matrix cannot see the difference, because multiplying
    two rank positions does not produce money.
  tags: [RiskManagement, ProjectControls, RiskRegister, Contingency, ProjectManagement]
  asset: checklist-pdf
gated: false
related: [BPG-10, BPG-17, BPG-19, TPL-10, AIG-06]
bok_domains: [3, 12]
sources:
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 12 — Risk Management for Project Controls, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 3 — Budgeting and Forecasting, first authored draft, August 2026"
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), verified August 2026"
placeholders: 0
---

# Risk registers that work

> Turning a compliance artefact back into a management instrument.

**In one paragraph.** Most risk registers are maintained and not used. This guide sets out what separates
the two: risk statements built as cause, event and effect; why an effect such as "delay to project" cannot
be managed as a risk; what qualitative probability-impact scoring can and cannot order; ownership and
action dates as the register's live parts; the link between the register and contingency; and the health
metrics that tell you, in a few minutes, whether your register is a management tool or wallpaper. The
worked example shows two risks with identical matrix scores whose expected values differ by a factor of
more than three.

**Who this is for.** Risk managers and project controls managers who own a register; cost engineers and
planners who have to turn one into contingency or a schedule model; and project managers who chair the
review that the register is supposed to serve.

---

## 1. The register that nobody uses

Almost every project has a risk register. A small minority of projects have one that changes a decision in
any given month. The gap between those two states is not effort — the unused registers are often
meticulously maintained — and it is not tooling. It is that the register was built to demonstrate that risk
management is happening rather than to make risk decisions, and an artefact built for demonstration acquires
a predictable shape: many entries, vague statements, team-level ownership, scores that never move, actions
without dates, and a review meeting that reads the top ten aloud and closes.

The diagnosis matters because the remedies are different. A register that is failing from neglect needs
attention. A register that is failing from purpose needs to be rebuilt around the decisions it is meant to
inform: what we fund, what we mitigate, what we escalate, and what we tell the client. Everything below
follows from that.

## 2. Cause, event, effect — and why the structure is not pedantry

A risk entry that can be managed has three separable parts.

**The cause** is a condition that exists now, or a dependency that is real now. It is not uncertain; it is a
fact about the project. "The permanent power connection is delivered by a utility outside the contract."

**The event** is the uncertain thing. It either happens or it does not, and a probability can be attached to
it. "Energisation is granted later than the commissioning window requires."

**The effect** is what the event does to an objective, measurable in time, money, scope or quality.
"Systems commissioning is delayed and time-related site costs extend."

Written as a single entry: *because the permanent power connection depends on a utility outside the
contract, there is a risk that energisation is granted later than the commissioning window requires, which
would delay systems commissioning and extend time-related site costs.*

The structure earns its place because each part drives a different management act. The **cause** is what a
mitigation attacks — you cannot reduce the probability of an event except by acting on its cause, which is
why registers full of causeless risks generate mitigations that are really just monitoring. The **event** is
what carries the probability, and it is the thing that can be declared closed when the uncertainty resolves.
The **effect** is what carries the impact, feeds quantification, and determines who should own the entry —
because the owner of a risk should be the person who owns the objective it threatens, not the person who
first mentioned it.

Two quick tests. If the cause is not a present fact, it is not a cause — it is a second risk, and the two
should be separated. If the event cannot be declared to have happened or not happened on a given date, it is
not an event — it is a condition, and it belongs in the assumptions or the issue log.

## 3. Why "delay to project" is not a risk

"Delay to project" is an effect. So is "cost overrun", "reputational damage" and "loss of client
confidence". A register of effects cannot be managed, for three reasons that compound.

**No action addresses it.** Delay arises from dozens of unrelated causes with different owners. An entry
that aggregates them can only be assigned to the project manager, and the mitigation can only be "manage the
project". That is not a control; it is a job description.

**It can never be closed.** The project is at risk of delay from the first day to the last. The entry
therefore sits on the register permanently, its score unchanged, and it teaches every reader that entries on
this register do not move. One immortal entry is enough to establish the culture.

**It double-counts.** If the register also carries the specific events that would cause delay, the aggregate
entry counts them again. Any quantification built on the register is then wrong by an unknown amount, and
the error is in the direction of overstating exposure — which sounds prudent until a contingency request is
challenged and cannot be defended line by line.

The same logic disposes of two other common entries. "Poor contractor performance" is an assessment, not an
event; the events are the specific things a poorly performing contractor does — misses a delivery date,
fails an inspection, under-resources a work front — each with its own cause and response. And an entry
describing something that has already happened is not a risk at all: it is an issue, with a probability of
100 per cent, and it belongs in the issue log with an action and a date. A register that carries issues has
stopped distinguishing what might happen from what has, which is the one distinction it exists to make.

## 4. Qualitative scoring: what it orders, and what it cannot

Almost every register scores probability and impact on ordinal bands — typically one to five — and multiplies
them into a matrix score. Used for what it is good at, this is a sound and fast technique: it triages a large
register quickly, needs no cost model, can be done in a workshop by people who will not tolerate a
quantitative exercise, and identifies which entries deserve quantification.

Its limitation is structural, not a matter of calibration. **The bands are ordinal and the impact scale is
usually geometric, while the score is arithmetic.** Band 4 is not twice band 2 in money; it is often ten or
twenty times. Multiplying two rank positions therefore produces a number with no monetary meaning, and two
entries in the same cell can differ in expected value by an order of magnitude. Section 8 works this through
with exact figures.

Three consequences for practice.

**Use the matrix to triage, not to rank for funding.** The matrix answers "which entries deserve more
analysis". It does not answer "which entries drive the contingency", and any register whose top ten is
selected by matrix score will have the wrong ten in it.

**Define the bands in the units of the project, and write them on the register.** Bands defined as "low,
medium, high" are re-interpreted by every person who uses them. Bands defined as a stated money range and a
stated probability range are at least argued about consistently. Impact bands should be calibrated so that
band 5 is genuinely material to the project — a band-5 threshold set at a figure the project spends monthly
makes the whole top row meaningless.

**Score impact against a stated objective.** A risk can be band 2 for cost and band 5 for schedule. A single
combined impact score hides which objective is threatened, and therefore hides who should own it. Score cost
and schedule impact separately, and let the register show both.

Where a project has an appetite statement — a declared level of exposure it is willing to carry — the
matrix is also where the appetite is expressed, by drawing the escalation line on the grid rather than
leaving each chair to apply their own tolerance. That is professional judgement made explicit, which is the
only form in which it can be reviewed.

## 5. Ownership, actions and dates — the live parts

The columns that determine whether a register is alive are not the scoring columns.

**Owner must be an individual.** "Engineering" cannot receive a phone call. Where a risk genuinely spans
functions, name one owner and list the others as contributors; a shared owner is an unowned risk.

**Every response has an action, an owner and a date, and the date is in the future.** An action without a
date is an intention. A register where a third of the action dates have already passed has told you
precisely how much attention the register receives, and it has done so more reliably than any maturity
assessment.

**Responses are typed, and the type is honest.** The threat strategies are avoid, transfer, reduce and
accept; the opportunity strategies are exploit, share, enhance and accept. Most entries that claim to be
mitigated are in fact accepted with monitoring, and calling that "mitigate" inflates the perceived control of
the project. Accepting a risk consciously, with contingency held against it, is a legitimate and often
correct decision; disguising acceptance as mitigation is not.

**A response is justified when it reduces probability times impact by more than it costs.** That test is
arithmetic and should be shown, not asserted — the register should carry the pre-response and post-response
assessment and the cost of the response, so the decision is visible. Responses whose cost exceeds the
exposure they remove should be recorded as rejected with a reason, because that record is what stops the same
proposal returning every quarter.

**Every entry carries a review date and a next expected change.** A risk whose score has not moved in three
review cycles is either genuinely stable, in which case say so and reduce its review frequency, or nobody has
looked at it. Recording which of the two it is takes one column and removes most of the ambiguity from the
register's health.

## 6. From register to contingency, and to the schedule model

Summing the **expected monetary value (EMV)** of every entry — probability multiplied by impact, added
across the register — gives a first-pass figure for the contingency needed against identified risks. That sum
is valid whatever the correlations between entries, because the expected value of a sum is the sum of the
expected values.

It is also not the answer, for two reasons that pull in the same direction.

**The expected value is not a fundable number.** No individual risk costs its expected value; each costs
either its impact or nothing. What a project funds is a confidence level on the distribution of total
outcomes, and that level sits above the expected-value sum whenever risks can coincide.

**Correlation fattens the tail without moving the mean.** Real registers are full of shared drivers: one
weather season behind several delays, one overheated supply market behind several vendor slips, one client
behind several payment risks. Shared drivers make coincidence far more likely than independent arithmetic
suggests, which raises the funded confidence level while leaving the expected-value sum exactly where it was.

The practical discipline needs no mathematics. For each pair of significant entries, ask what would make both
happen. A nameable shared cause is a correlation to declare — to the modeller if there is a model, and to the
contingency basis if there is not. `BPG-10 — Contingency and management reserve` owns how the funded level is
chosen, drawn down and governed; `BPG-17 — Quantitative schedule risk analysis` owns what happens when the
register is mapped onto the schedule and simulated, including how much of the answer the correlation
assumption controls.

One connecting discipline belongs here because it is the register's responsibility rather than the model's:
**risk loading**. Each entry that will be quantified should name the specific activities and cost lines it
would strike. Without that mapping, the model is built on a parallel set of assumptions and the register
becomes a document the model happens to resemble. With it, the register is the model's input and any change
to one is visible in the other.

## 7. How this goes wrong

**The register is a deliverable, not a tool.** It is produced because the governance framework requires one,
reviewed because the review is in the schedule, and consulted by nobody between reviews. The tell is that
nothing in the project plan changes as a result of it. A register that has never caused a schedule change, a
contingency draw or a contract negotiation is not managing risk.

**Entries are effects, so nothing can be closed.** Covered in §3. The visible symptom is a register whose
population only ever grows.

**The top ten is chosen by matrix score.** The heavy-tail entries — low probability, very high impact — sit
below the line because the matrix rewards likelihood. Those are usually the entries that determine the
contingency and, occasionally, the entries that end the project.

**Mitigations are monitoring in disguise.** "Monitor closely", "maintain dialogue with the supplier",
"escalate if required". None of these changes probability or impact, so the residual assessment should be
unchanged — yet the register almost always shows a reduced residual score after they are added, which
manufactures comfort out of nothing.

**Everything is owned by the project manager.** A register with one owner is a to-do list belonging to
someone who already has one. Ownership should follow the objective the effect threatens.

**Impact is scored on the project, not on the objective.** "High" impact turns out to mean high for schedule
in one entry and high for reputation in another, and the two are then compared as though they were
commensurable.

**Opportunities are absent.** Risk has two tails. A register with no upside entries is evidence about the
workshop, not about the project, and it systematically biases the contingency conversation, because the
opportunities that would net against threats are simply not on the page.

**The register outlives its assumptions.** Scores set at sanction are still there at handover, describing a
project that no longer exists. Re-identification at each phase gate is not optional; the risk profile a
project starts with is not the one it carries.

**Quantification double-counts responses.** The entry is quantified at its pre-response impact, and the cost
of the response is also in the budget. The exposure is then funded twice, and the contingency request cannot
survive scrutiny.

## 8. Worked example

*Illustrative figures. Currency is USD, whole dollars, no escalation and no discounting applied. Impacts are
cost impacts on a single objective. Probabilities are point estimates from a workshop.*

### 8.1 The scales in use

| Band | Probability | Cost impact |
|---|---|---|
| 1 | below 10 % | below 25,000 |
| 2 | 10 % to 25 % | 25,000 to 250,000 |
| 3 | 25 % to 50 % | 250,000 to 750,000 |
| 4 | 50 % to 75 % | 750,000 to 2,500,000 |
| 5 | above 75 % | above 2,500,000 |

Matrix score is the product of the two band numbers, on a scale of 1 to 25. The project's escalation line is
drawn at a score of 8.

### 8.2 Three entries

| Ref | Event (abbreviated) | Probability | P band | Impact | I band | Matrix score |
|---|---|---:|---:|---:|---:|---:|
| R-11 | Late utility energisation | 60 % | 4 | 100,000 | 2 | 8 |
| R-24 | Single-source vendor fails to deliver the main package | 20 % | 2 | 1,000,000 | 4 | 8 |
| R-31 | Contaminated ground discovered on the east plot | 5 % | 1 | 1,200,000 | 4 | 4 |

R-11 and R-24 share a matrix score of 8 and sit on the escalation line. R-31 scores 4 and sits below it.

### 8.3 Expected value

```
EMV = probability × impact

R-11:  0.60 × 100,000   =   60,000
R-24:  0.20 × 1,000,000 =  200,000
R-31:  0.05 × 1,200,000 =   60,000

Total EMV = 60,000 + 200,000 + 60,000 = 320,000
```

### 8.4 What the two orderings disagree about

**Same score, different money.** R-11 and R-24 both score 8. Their expected values are 60,000 and 200,000.

```
ratio = 200,000 ÷ 60,000 = 3.33 (to two decimal places)
```

R-24 carries more than three times the expected exposure of R-11, and the matrix places them in the same
cell. A register that funds by matrix position funds these two identically.

**Different score, same money.** R-11 scores 8 and R-31 scores 4 — R-11 is ranked at twice R-31's priority.
Their expected values are identical at 60,000 each. The matrix has ordered by likelihood, and likelihood is
not what the project pays.

**Where the tail lives.** R-31 is the entry that can cost 1,200,000 in a single event, which is 3.75 times
the entire expected-value sum of the register (`1,200,000 ÷ 320,000 = 3.75`). It is also the entry the matrix
places below the escalation line. If this register's top ten were selected by matrix score, the largest
single loss on the project would not be in it.

### 8.5 Why this happens, in one line

The impact bands step by roughly an order of magnitude while the band numbers step by one. Multiplying two
band numbers multiplies rank positions; multiplying probability by impact multiplies a proportion by an
amount of money. Only the second produces a quantity you can add up, compare or fund.

### 8.6 What follows for this register

Use the matrix to decide which entries go forward for quantification — and set the forward criterion on
either dimension, so that a band-4 or band-5 impact goes forward regardless of probability. Order the funding
conversation by expected value, and hold the heavy-tail entries separately because their expected value
understates what they can do in a single occurrence. Then ask what R-24 and R-31 have in common: if the
answer is a shared driver — one contaminated-land condition that also delays the vendor's foundation works,
say — declare it, because that is the pairing that moves the funded level.

The assumptions this rests on should travel with the numbers: the probabilities are workshop point estimates
with no range around them; the impacts are single figures rather than distributions; nothing here nets
opportunities against threats; and the expected-value sum is a starting point for contingency, not a
contingency figure. `BPG-10` takes it from there.

## 9. Register health in five minutes

*Illustrative figures from a single register at one review date.*

A register of **84 entries**, of which:

- **61** have a named individual as owner: `61 ÷ 84 = 72.6 %`. Twenty-three entries are owned by a team,
  a function or nobody.
- **31** have a next action date that has already passed: `31 ÷ 84 = 36.9 %`.
- **44** have not changed score in three or more review cycles: `44 ÷ 84 = 52.4 %`.
- **2** were added and **0** closed in the last cycle.

Each figure is a question rather than a verdict, but together they are decisive. A register where more than
half the entries have not moved in three cycles, more than a third of actions are overdue, and nothing has
closed all cycle is not being used; it is being maintained. And 84 is itself a finding — a project board
cannot manage 84 of anything, so either the register needs a working subset with a stated selection rule, or
the majority of entries should be closed, merged or moved to a watch list with a longer review cycle.

Run these four counts before the review meeting rather than after it. They take minutes, they are not
disputable, and they change what the meeting is about.

## 10. Checklist

**Every entry**

- [ ] Cause is a present fact, event is a discrete uncertainty, effect is measurable in time or money.
- [ ] The event can be declared to have happened or not happened on a stated date.
- [ ] The entry is not an effect, an assessment or an issue.
- [ ] Owner is a named individual who owns the threatened objective.
- [ ] Probability and impact are assessed against published bands, in project units.
- [ ] Cost impact and schedule impact are scored separately.
- [ ] Response is typed honestly — avoid, transfer, reduce, accept, or the opportunity equivalents.
- [ ] Response has an action, an owner and a date in the future.
- [ ] Pre-response and post-response assessments are both recorded, with the response's cost.
- [ ] Where the entry will be quantified, the activities and cost lines it would strike are named.

**Every review**

- [ ] Overdue action count, unowned count and unchanged-score count computed before the meeting.
- [ ] Entries closed as well as entries added.
- [ ] Heavy-tail entries reviewed regardless of matrix position.
- [ ] Shared drivers between significant entries named and recorded.
- [ ] Opportunities reviewed on the same footing as threats.

**Every gate**

- [ ] Re-identification run, not just re-scoring, with the people doing the next phase's work.
- [ ] Assumptions behind the original scores tested against what is now known.
- [ ] The expected-value sum reconciled to the contingency held, with the difference explained.

The register that passes this list is smaller than the one it replaced and is read by people who are not
paid to read it. That is the only durable evidence that risk management is happening: the register changed
what the project did, and someone can name the month it happened.

---

## Related

- `BPG-10 — Contingency and management reserve` — how the quantified register becomes a funded number, and who may draw it
- `BPG-17 — Quantitative schedule risk analysis` — what happens when the register is mapped onto the schedule and simulated
- `BPG-19 — Project controls assurance and health checks` — how a reviewer tests whether the register is genuinely in use
- `TPL-10 — Risk register` — the column standard and definitions this guide assumes
- `AIG-06 — AI for risk identification and quantification` — where machine assistance helps, and what must stay with the professional

## Sources and standards

- PCL-AI Body of Knowledge (`docs/bok/`), Domain 12 — Risk Management for Project Controls, first authored
  draft, August 2026: the cause-event-effect standard, qualitative and quantitative analysis, response
  strategies, correlation and shared drivers.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 3 — Budgeting and Forecasting, first authored draft,
  August 2026: estimating bias and the treatment of uncertainty in a budget.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  naming, status and claims policy.

ISO 31000 is the international risk-management standard most often referenced in this area. This guide does
not reproduce its text, structure or clause numbering; where its principles are relevant — that risk
management is integrated into decisions rather than run alongside them, and that it addresses uncertainty in
both directions — they are explained here in the Institute's own words. No survey data, benchmark or
industry average is cited, because none was verified for this guide.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
