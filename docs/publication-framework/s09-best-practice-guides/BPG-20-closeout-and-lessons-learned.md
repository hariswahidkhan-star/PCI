---
id: BPG-20
series: S09
series_name: Best Practice Guides
title: Closeout, lessons learned and benchmarking
subtitle: Collecting what the project is owed, and what it knows, before both disperse
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 15
summary: >
  Closeout is where a project's remaining money and its entire accumulated knowledge are either collected or
  lost, and both are usually lost for the same reason: the people who hold them have already moved on. This
  guide covers commercial and financial closeout and the final account, the open-items position that keeps a
  project's reported margin provisional, why lessons-learned registers are almost universally useless and
  the four properties that fix them, and how to capture benchmark data that the next estimate can actually
  use. The worked example reconciles a final account and then shows a productivity rate that reads as 11.1
  per cent over estimate or 2.2 per cent under it, depending only on which hours were counted.
linkedin:
  format: article
  hook: >
    Your closeout record says the steel took 15.0 hours a tonne against an estimate of 13.5 — an 11.1 per
    cent overrun. Strip out the supervision hours the estimate excluded and it took 13.2, which is 2.2 per
    cent better than estimate. Same job, opposite conclusion, and only the definition changed.
  tags: [ProjectControls, Closeout, LessonsLearned, Benchmarking, CostEngineering]
  asset: checklist-pdf
gated: false
related: [TPL-16, BPG-19, BPG-11, BPG-12, BPG-10, BPG-09]
bok_domains: [3, 7, 8]
sources:
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 8 — Project Management Lifecycle, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 7 — Contracts, Commercial Management, BoQ, Invoicing and Revenue, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 3 — Budgeting and Forecasting, first authored draft, August 2026"
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), verified August 2026"
placeholders: 0
---

# Closeout, lessons learned and benchmarking

> Collecting what the project is owed, and what it knows, before both disperse.

**In one paragraph.** Closeout is where a project's remaining money and its entire accumulated knowledge are
either collected or lost, and both are usually lost for the same reason: the people who hold them have
already moved on. This guide covers commercial and financial closeout and the final account, the open-items
position that keeps a project's reported margin provisional, why lessons-learned registers are almost
universally useless and the four properties that fix them, and how to capture benchmark data that the next
estimate can actually use. The worked example reconciles a final account and then shows a productivity rate
that reads as 11.1 per cent over estimate or 2.2 per cent under it, depending only on which hours were
counted.

**Who this is for.** Project controls managers and cost engineers running a project to closure; commercial
managers settling final accounts; and estimating and PMO functions who depend on what closeout hands them.

---

## 1. Closeout starts before the work does

The characteristic failure of closeout is that it is scheduled as a phase and staffed as an afterthought.
The team demobilises on completion, the strongest people leave first because they are wanted elsewhere, and
the residual work — final account, retention recovery, record assembly, knowledge capture — falls to whoever
is left, at exactly the moment the project's budget for staff has closed.

Two structural corrections do most of the work, and both are planning decisions rather than closeout
decisions.

**Fund and resource closeout in the baseline.** A named budget line and named people, with an end date that
is later than the completion date. If closeout has no budget, it will be done by people who are also doing
something else, which is a reliable way to lose money on the last two per cent of a project.

**Schedule the closure deliverables backwards from the last commercial event, not from practical
completion.** The final retention release, the last defect certificate and the last subcontract final account
often sit many months after the asset is handed over. The project is not closed when the client takes the
asset; it is closed when the last receivable is collected and the last liability is extinguished.

A third correction is cultural and harder: **collect while the evidence is warm.** Records, quantities,
photographs, correspondence and the reasoning behind decisions are all cheap to capture during execution and
expensive to reconstruct afterwards. The reconstruct-later approach fails specifically at closeout, because
the person who knew has gone.

## 2. Commercial and financial closeout

Commercial closeout settles the position with every party. Financial closeout settles the position in the
books. They are related and not identical, and running them as one exercise causes a project to be reported
as closed while money is still outstanding.

**Commercial closeout** covers, at minimum: agreement of the final account with the client; agreement of
every subcontract final account; resolution or provision of every claim in both directions; release of
retention on both sides; discharge or expiry of bonds, guarantees and warranties; settlement of contra-charges
and back-charges; and closure of insurances at the appropriate date rather than at handover.

**Financial closeout** covers: reversal of accruals that will never be invoiced; release of provisions that
are no longer needed and recognition of those that are; final cost allocation; closing the cost collection
structure so nothing further can be posted; and archiving the records that support every number.

Two disciplines are worth stating explicitly because they are where reported margin most often moves after a
project is declared complete.

**No provision is released without a decision and a date.** Provisions carried against unsettled
subcontractor accounts, latent defects or disputed variations are judgements. Releasing them quietly to
improve a final result is a reporting failure and, depending on materiality and jurisdiction, potentially
more than that. The correct treatment is a documented decision with the basis stated.

**The cost structure is closed, not just abandoned.** An open cost code on a closed project will receive
postings — recharges, late invoices, corrections — and they will land on a project nobody is monitoring.
Close the codes and route genuine late items through a named person.

Accounting treatment, tax treatment, retention mechanics, defects-liability periods and statutory payment
regimes differ substantially between jurisdictions and contract forms. What is general is the discipline: a
closeout position is not complete until every open item has a value, an owner and an expected settlement
date.

## 3. The final account and the open-items position

The final account is the last definitive reconciliation of scope, cost, billing and entitlement for a
contract. Its components are owned by other guides — `BPG-11 — Change orders and variations` for the
variation account, `BPG-12 — Claims and extension of time` for the claim settlement — and what belongs here
is the assembly and what it leaves outstanding.

The assembly is straightforward arithmetic and is worked in §7: original value, plus approved variations,
adjusted for remeasurement of provisional or approximate quantities, plus agreed claim settlements, less
contra-charges and back-charges. What follows the assembly is where projects go wrong.

**Retention is a receivable, not a rounding.** It is typically released in stages, and the last stage
usually falls after a defects-liability period that has no reason to align with anyone's financial year. Track
each stage with its own trigger, its own date and its own owner. The project is not closed while it is
outstanding.

**Every open item carries a value and an exposure.** Subcontractor final accounts still in negotiation,
disputed variations, unresolved back-charges and outstanding defects each have a claimed value and a provided
value. The difference is the exposure, and the project's reported margin is provisional by exactly that
amount. Publishing the open-items table with the claimed, provided and exposure columns is the single most
useful thing closeout reporting does, because it converts "final result subject to closeout" into a number.

**The final account is the reconciliation point for everything upstream.** It should tie to the change log,
to the cost ledger, to the billing record and to whatever was recognised as revenue — and where it does not
tie, the difference should be explained rather than adjusted. Differences between measured value, earned
value and recognised revenue are usually legitimate and informative; making them disappear destroys the
information.

## 4. Why lessons-learned registers fail

Almost every organisation has lessons-learned registers. Almost none of them changes what the next project
does. The failure is systematic enough to be diagnosed precisely, and every element of the diagnosis points
at the same remedy.

**They are written at the end, by the people who are left.** The lesson belongs to whoever was there when it
happened, and by closeout that person has moved. What gets written is what the remaining team remembers,
which is systematically the recent, the visible and the uncontroversial.

**They are written as feelings about the past.** "Communication with the client could have been better."
"Earlier engagement with the supply chain would have helped." These are true, unactionable, and identical
across every project in the world. Nothing in them tells anyone what to do differently.

**They have no addressee.** A lesson about design co-ordination belongs to the person who owns the design
management process. A lesson about schedule templates belongs to the head of planning. A register that
addresses nobody is filed, and filing is where lessons go to die.

**They have no route into a process.** A lesson that does not change a document — a template, a checklist, a
procedure, a standard scope of work, an estimating norm — changes nothing at all. Knowledge that lives only
in a register is knowledge that must be searched for by someone who does not know it exists.

**They are stored where nobody looks and tested at no gate.** If the next project's gate review does not ask
which lessons apply and what was done with them, the register has no consumer, and an artefact with no
consumer decays.

**They are anonymised into uselessness.** Removing the specifics to avoid blame removes the content. A
lesson without the quantity, the date, the package and the mechanism is a proverb.

## 5. What a usable lesson looks like

Four properties fix the failures above: a lesson must be **specific**, **attributable**, **actionable** and
**routed**.

**Specific** — it names the package, the date, the quantity and the mechanism. A lesson that could apply to
any project applies to none.

**Attributable** — it names who observed it and who can confirm it, so the next reader can ask a question.
This is not blame; it is provenance, and it is the difference between a lesson and a rumour.

**Actionable** — it states a change to a named artefact. Not "we should plan commissioning earlier" but
"the standard schedule template adds an activity for each witness test, showing client-supplied resource as a
dependency".

**Routed** — it names the process owner who receives it, and the register records their disposition:
accepted with a date, rejected with a reason, or deferred with a review date. A rejected lesson with a
recorded reason is a healthy outcome; an unanswered lesson is not.

The structure that produces all four is a fixed statement form:

> **Situation** (what the circumstances were) → **action** (what we did) → **outcome** (what happened, with
> the number) → **change** (what should be different, in which document) → **owner** (which process owner
> receives it) → **test** (at which gate the next project is asked about it).

Compare, on the same event:

*Unusable:* "Better co-ordination with the client's operations team was needed during commissioning."

*Usable:* "**Situation:** the water treatment package required client operations staff to witness
performance tests, an obligation in the contract but not represented in the schedule. **Action:** witness
tests were scheduled assuming staff availability on request. **Outcome:** commissioning of the package was
delayed 23 days waiting for witness attendance; time-related site costs continued throughout. **Change:** the
standard schedule template adds a client-resource activity for every witness test, and the contract review
checklist adds a check for client-supplied resource obligations. **Owner:** head of planning for the
template; head of commercial for the checklist. **Test:** gate 3 schedule acceptance asks whether
client-supplied resource obligations are represented in the network."

The second is longer, and length is not the point — the point is that someone can act on it without asking a
question, and that a gate reviewer can check whether they did.

**Capture while the pain is fresh.** The strongest single change an organisation can make in this area is to
stop treating lessons capture as a closeout activity. Capture at each phase gate, at each significant event,
and at the resolution of each major risk or claim — while the people involved are still present and still
remember why the decision looked reasonable at the time. Closeout then assembles and routes what has already
been captured, which is a manageable task, instead of attempting recall, which is not.

`TPL-16 — Lessons learned and closeout register` carries the field structure and disposition columns this
implies.

## 6. Benchmarking: capturing data the next estimate can use

The controls function is the natural custodian of a project's quantitative legacy, because it holds the
performance data. What it hands over determines the quality of the next estimate — and, increasingly, the
quality of whatever is fitted to the organisation's historical data.

What is worth capturing at closeout:

**Outturn performance indices** — final cost and schedule performance, and the shape of their history rather
than only the end point.

**Unit rates and productivity** — hours per unit and cost per unit for the significant work types, at a
level of detail the next estimator will actually use.

**Change volume** — the number and value of variations against original value, which is the most transferable
single indicator of how well the scope was defined.

**Contingency behaviour** — how much was funded, at what confidence level, how much was drawn, and against
which risks. This is what calibrates the next project's contingency, and it is almost never recorded.

**Estimate accuracy by gate** — the estimate at each gate against the outturn, which tells the organisation
how much its own early numbers are worth.

All of that is worthless without the property that makes it comparable: **normalisation metadata**. A rate
without its definition is not data; it is a number that will be misapplied. Five fields are the minimum, and
§7.3 shows what happens when they are missing:

- **What is in the numerator** — which hours, which costs; direct only, or with supervision, scaffold,
  preliminaries, overtime premium?
- **What is in the denominator** — which quantity, measured how; net installed, gross delivered, contract
  measure?
- **Conditions** — site access, working hours, weather exposure, congestion, height, shift pattern.
- **Basis of currency and time** — currency, price base date, and whether escalation has been removed.
- **Scope boundary** — what the rate includes and specifically excludes.

Two further rules. **Capture the metadata at the same time as the number**, because it cannot be
reconstructed later — nobody will remember in two years whether the supervision hours were in. And
**publish the sample size and the variability**, not only the average: a rate derived from one package on
one project is an observation, and presenting it as a norm is how one project's bad month becomes an
organisation's estimating standard.

## 7. Worked example

*Illustrative figures. Currency is USD, whole dollars. One construction contract within a larger project,
viewed by the party that engaged the contractor. Retention and defects mechanics are those assumed for this
example and vary by contract form and jurisdiction. No escalation or discounting is applied.*

### 7.1 The final account

| Component | Value |
|---|---:|
| Original contract value | 8,400,000 |
| Approved variations | + 610,000 |
| Remeasurement of provisional quantities | − 85,000 |
| Agreed claim settlement | + 240,000 |
| Contra-charges and back-charges | − 55,000 |
| **Final account** | **9,110,000** |

```
8,400,000 + 610,000 = 9,010,000
9,010,000 −  85,000 = 8,925,000
8,925,000 + 240,000 = 9,165,000
9,165,000 −  55,000 = 9,110,000
```

### 7.2 Retention and what is still to collect

*Assumption for this example: retention is 5 per cent of the final account, half released at practical
completion and the balance at the end of the defects-liability period.*

```
total retention          = 9,110,000 × 0.05 = 455,500
released at practical completion = 455,500 ÷ 2 = 227,750
still held               = 455,500 − 227,750 = 227,750
```

With **8,760,000** certified and paid to date, net of retention still held:

```
amount outstanding excluding retention = final account − retention still held − paid to date
                                       = 9,110,000 − 227,750 − 8,760,000
                                       = 122,250

total still to collect = 122,250 + 227,750 = 350,000
```

The contract is not closed while **350,000** — nearly four per cent of the final account
(`350,000 ÷ 9,110,000 = 3.8 %`) — remains outstanding, and roughly two thirds of it depends on a
defects-liability period whose end date has nothing to do with the reporting calendar.

### 7.3 The open-items position

| Open item | Claimed against us | Provided | Exposure |
|---|---:|---:|---:|
| Subcontractor A — final account in negotiation | 210,000 | 140,000 | 70,000 |
| Subcontractor B — disputed variation | 165,000 | 110,000 | 55,000 |
| Subcontractor C — back-charge counter-claim | 105,000 | 60,000 | 45,000 |
| **Total** | **480,000** | **310,000** | **170,000** |

```
column check: 210,000 + 165,000 + 105,000 = 480,000
              140,000 + 110,000 +  60,000 = 310,000
exposure    = 480,000 − 310,000 = 170,000
```

The project's reported result is provisional by up to **170,000**, and saying so with the number attached is
the difference between a closeout report and a closeout announcement.

### 7.4 The benchmark that means two opposite things

The project erected **1,240 tonnes** of structural steel using **18,600 recorded labour hours**, of which
**12 per cent** were supervision. The estimate for this work assumed **13.5 direct hours per tonne**.

**All-in rate, as most closeout records capture it:**

```
18,600 ÷ 1,240 = 15.0 hours per tonne

variance against the 13.5 estimate = 15.0 − 13.5 = 1.5 hours per tonne
                                   = 1.5 ÷ 13.5 = 11.1 % above estimate
```

**Direct-only rate, on the same basis as the estimate:**

```
supervision hours = 18,600 × 0.12 = 2,232
direct hours      = 18,600 − 2,232 = 16,368
direct rate       = 16,368 ÷ 1,240 = 13.2 hours per tonne

variance against the 13.5 estimate = 13.2 − 13.5 = −0.3 hours per tonne
                                   = 0.3 ÷ 13.5 = 2.2 % below estimate
```

**Same job. Read as 11.1 per cent over estimate, or 2.2 per cent under it, entirely according to which hours
were counted.** No performance question separates them; only a definition does.

**What the definitional error costs the next estimate.** Suppose the 15.0 figure is filed without its
definition and the next estimator applies it to a 1,500-tonne job, believing it comparable with the 13.5
norm:

```
estimated on the all-in rate    = 1,500 × 15.0 = 22,500 hours
like-for-like on the direct rate = 1,500 × 13.2 = 19,800 hours
difference                       = 22,500 − 19,800 = 2,700 hours

overstatement = 2,700 ÷ 19,800 = 13.6 %
```

A **2,700-hour** error on one line, produced by a missing metadata field, not by any disagreement about how
long steel takes to erect. And the error is invisible: both numbers are correct, both are traceable to real
records, and nothing in the estimate signals that the two rates are measured differently.

### 7.5 The rest of the benchmark set from this contract

```
change volume = approved variations ÷ original value
              = 610,000 ÷ 8,400,000 = 7.3 %

estimate accuracy at the sanction gate:
gate estimate for this contract = 8,000,000
outturn (final account)         = 9,110,000
variance = (9,110,000 − 8,000,000) ÷ 8,000,000
         = 1,110,000 ÷ 8,000,000 = 13.9 % above the gate estimate
```

Both figures need the same treatment as the steel rate. The 7.3 per cent change volume is comparable only
with projects whose variation definition matches — some organisations count client changes only, others
include design development and remeasurement. The 13.9 per cent estimate variance is measured against the
sanction-gate estimate for this contract alone, and would be a different number measured against the
feasibility estimate, the tender sum or the total project outturn. Record which, or the number will be
compared with something it does not describe.

## 8. How this goes wrong

**Closeout is unfunded and unstaffed.** The last two per cent of the project is delivered by whoever has not
yet been reassigned. Retention goes uncollected, final accounts are settled on the counterparty's terms
because nobody has time to argue, and the lessons are never captured.

**The project is declared closed at handover.** The asset is transferred, the report says complete, and
350,000 of receivables and 170,000 of exposure are still live.

**Provisions are released to improve the result.** Quietly, without a documented decision. The number
improves and the basis for it is gone.

**Lessons are captured at the end, from memory.** Covered in §4. The tell is a register whose entries are all
about communication and stakeholder engagement, because those are the things people remember feeling.

**Lessons are written without an addressee.** No process owner, no artefact to change, no disposition. The
register is complete and inert.

**Specifics are removed to avoid blame.** The quantity, the date and the package go, the mechanism becomes a
generality, and the entry joins the proverbs.

**The gate does not ask.** No project is required to demonstrate what it took from the register, so nobody
reads it, so nobody writes it well.

**Benchmarks are captured without definitions.** §7.4 prices this. The organisation ends up with a
respectable-looking rate library that cannot be safely used and, worse, that people use anyway.

**One project's rate becomes a norm.** A single observation, published as an organisational standard, with
no sample size and no indication of variability.

**Contingency behaviour is never recorded.** How much was funded, at what confidence level, how much was
drawn and against what. Without it, the next contingency argument is conducted entirely on assertion, and
`BPG-10 — Contingency and management reserve` has nothing to calibrate against.

**The archive is assembled but not indexed.** Everything is kept and nothing can be found, which is
operationally identical to keeping nothing while costing considerably more.

## 9. Checklist

**Planned at the start, not at the end**

- [ ] Closeout has a budget line, named people and an end date later than completion.
- [ ] Closure deliverables scheduled backwards from the last commercial event, not from handover.
- [ ] Lessons capture scheduled at each gate and at each significant event, not only at closeout.
- [ ] Benchmark metadata fields defined at the same time as the cost and quantity structures.

**Commercial and financial**

- [ ] Final account assembled and tied to the change log, cost ledger, billing record and revenue.
- [ ] Every subcontract final account settled or provided for, with the exposure stated.
- [ ] Retention tracked by stage, each with its trigger, date and owner, to zero.
- [ ] Bonds, guarantees, warranties and insurances discharged or diarised to their true end dates.
- [ ] Accruals reversed, provisions released only on a documented decision with a stated basis.
- [ ] Cost collection codes closed; a named person routes genuine late items.
- [ ] Open-items table published with claimed, provided and exposure columns.

**Lessons**

- [ ] Every entry is specific, attributable, actionable and routed.
- [ ] Every entry names a document to change and a process owner to receive it.
- [ ] Every entry has a recorded disposition: accepted with a date, rejected with a reason, or deferred.
- [ ] Entries carry the quantity, the date and the package — specifics not stripped.
- [ ] The next project's gate review asks which entries apply and what was done with them.

**Benchmarking**

- [ ] Unit rates and productivity captured with all five normalisation fields.
- [ ] Numerator and denominator definitions recorded at the moment of capture.
- [ ] Currency, price base date and escalation treatment stated.
- [ ] Change volume recorded with its variation definition.
- [ ] Estimate accuracy recorded against each gate estimate, with the gate named.
- [ ] Contingency funded, confidence level, drawn amount and the risks it was drawn against, all recorded.
- [ ] Sample size and variability published alongside any rate offered as a norm.
- [ ] Archive indexed to the structure the next project will search by, not the one this project used.

A project that closes this way hands its successor two things that are otherwise unobtainable: a rate library
whose numbers mean what they say, and a small set of lessons that have already changed a document. Everything
else in a closeout file is a record. Those two are an inheritance, and they are the only part of a finished
project that goes on earning.

---

## Related

- `TPL-16 — Lessons learned and closeout register` — the field structure, disposition columns and routing this guide assumes
- `BPG-19 — Project controls assurance and health checks` — the review that tests whether closeout data is being captured while it is still cheap
- `BPG-11 — Change orders and variations` — the variation account that feeds the final account
- `BPG-12 — Claims and extension of time` — the claim settlements that feed it, and the records that support them
- `BPG-10 — Contingency and management reserve` — what closeout's contingency-behaviour data is used to calibrate
- `BPG-09 — Estimate at completion — choosing and defending a method` — the forecasting method whose accuracy closeout finally measures

## Sources and standards

- PCL-AI Body of Knowledge (`docs/bok/`), Domain 8 — Project Management Lifecycle, first authored draft,
  August 2026: contract and project closure, handover and the final account, completions and turnover, and
  the controls function's custody of quantitative lessons.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 7 — Contracts, Commercial Management, BoQ, Invoicing and
  Revenue, first authored draft, August 2026: remeasurement, retention, certification and the reconciliation
  of valuation with earned value and recognised revenue.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 3 — Budgeting and Forecasting, first authored draft,
  August 2026: the use of historical performance data in estimating, and estimating bias.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  naming, status and claims policy.

Retention percentages, release triggers, defects-liability periods, remeasurement rules and the accounting
treatment of provisions differ between contract forms and between jurisdictions; the mechanics in §7 are
stated as assumptions of the example and are not presented as general rules. No industry benchmark,
productivity norm or estimate-accuracy range is cited in this guide, because none was verified for it; every
figure in §7 is illustrative and was constructed for teaching.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
