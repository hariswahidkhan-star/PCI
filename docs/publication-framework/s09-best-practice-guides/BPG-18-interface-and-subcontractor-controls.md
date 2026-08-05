---
id: BPG-18
series: S09
series_name: Best Practice Guides
title: Interface and subcontractor controls
subtitle: Controlling the work you do not do yourself
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 15
summary: >
  Most of the scope on a large project is delivered by organisations that do not report to you and whose
  data arrives on their calendar, not yours. This guide covers the interface register and why an interface
  is not a milestone, the cadence mismatch between a subcontractor's period end and your cut-off, keeping
  subcontract obligations back-to-back with the head contract, progress claimed versus progress verified,
  and what to do when a subcontractor's own controls cannot produce the data your reporting depends on. The
  worked example separates one claim into a payment decision and a cost decision, and shows a swing of 0.065
  on the cost performance index caused by a cadence mismatch alone.
linkedin:
  format: article
  hook: >
    Your subcontractor's period ends on the 20th. Your cut-off is the 30th. Take their claim straight into
    your cost report and you have booked twenty days of cost against thirty days of earned value — which is
    a cost performance index that flatters by construction, not by performance.
  tags: [ProjectControls, Subcontracts, InterfaceManagement, CostControl, Commercial]
  asset: checklist-pdf
gated: false
related: [BPG-06, BPG-07, BPG-11, BPG-12, BPG-13, BPG-05]
bok_domains: [5, 7, 10]
sources:
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 7 — Contracts, Commercial Management, BoQ, Invoicing and Revenue, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 5 — Cost Management and Cost Control, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 10 — Project Scheduling, first authored draft, August 2026"
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), verified August 2026"
placeholders: 0
---

# Interface and subcontractor controls

> Controlling the work you do not do yourself.

**In one paragraph.** Most of the scope on a large project is delivered by organisations that do not report
to you and whose data arrives on their calendar, not yours. This guide covers the interface register and why
an interface is not a milestone, the cadence mismatch between a subcontractor's period end and your cut-off,
keeping subcontract obligations back-to-back with the head contract, progress claimed versus progress
verified, and what to do when a subcontractor's own controls cannot produce the data your reporting depends
on. The worked example separates one claim into a payment decision and a cost decision, and shows a swing of
0.065 on the cost performance index caused by a cadence mismatch alone.

**Who this is for.** Project controls managers and cost engineers on projects with substantial subcontracted
scope; commercial managers and quantity surveyors who certify applications; and planners managing interfaces
between contractors, clients and third parties.

---

## 1. The control problem when the work is somebody else's

Direct work gives a controls function primary data: your timesheets, your goods-received notes, your progress
measurement, on your cut-off. Subcontracted work gives you *reported* data — a claim prepared by an
organisation with a commercial interest in the number, covering a period that ends when their accounting
month ends, measured against rules they applied. Three properties follow, and every failure in this area is a
version of one of them.

**The data is an assertion until you verify it.** An application is a starting position in a commercial
conversation. Treating it as a measurement transfers the subcontractor's optimism straight into your forecast.

**The data arrives on their calendar** — a systematic mismatch between numerator and denominator that recurs
every month in the same direction.

**Their control weakness becomes your reporting weakness.** No clause converts data you never received into a
number you can report. Decide in advance whether you will measure their work yourself or report an estimate.

## 2. The interface register, and why an interface is not a milestone

An **interface** is a point where a deliverable, information, an access, a service or a physical connection
passes between two organisations. Interfaces are where integrated projects fail, because both sides believe
they are managing them and neither owns the gap. The register that manages them is not the milestone list: a
milestone has one owner and one date, while **an interface has two owners, two schedules and a tolerance.**
That determines the columns:

| Field | Why it exists |
|---|---|
| Interface reference | A permanent identifier both parties quote; never renumbered |
| Parties and direction | The two organisations named, and who provides and who receives |
| What crosses | The deliverable, information set, access or connection, defined tightly enough to check |
| Acceptance criterion | What the receiver must be able to do with it before it counts as delivered |
| Required-by date | The receiver's need date, from their schedule |
| Committed date | The provider's date, from theirs |
| Float between them | Required-by minus committed; the interface's health, in days |
| Owner, each side | A named individual on the provider side and on the receiver side |
| Consequence of late | What it costs, in days or money, against the receiver's critical path |
| Status and last confirmed | When both parties last agreed the dates, not when the register was edited |

Two columns do the real work. **Float** turns an interface from a status into a number, and a register sorted
by it is a management agenda. **Consequence of late** makes the provider's organisation care.

Bilateral confirmation keeps the register alive. A date taken from one party's schedule and never confirmed
by the other is a hope with a date field. Flag any interface not jointly confirmed within an agreed period
regardless of how comfortable its float looks — comfortable float is the condition under which nobody checks.
Interfaces with third parties under no contract — utilities, authorities, neighbouring projects, the client's
own operations teams — are the ones most often missing from the register entirely, and with no commercial
lever behind them the only control is early identification, long lead times and explicit contingency.

## 3. Cadence mismatch: their period end and your cut-off

Subcontractors report on their own commercial cycle: typically a claim to a fixed day of the month — the
twentieth, say — while your cost cut-off is the last day of the month and your progress is measured then.

If the claim is posted unadjusted, the month's cost covers work to the twentieth and the month's earned value
covers work to the thirtieth. The cost performance index — earned value divided by actual cost — then
compares thirty days of output with twenty days of input, and it flatters. Next month the missing ten days
arrive on top of a full month and the index drops for a reason unrelated to performance. The resulting
sawtooth costs the controls function its credibility, because everyone can see it and the explanation sounds
like an excuse.

Three responses, of which only one is generally right.

**Move their cut-off.** Contractually cleanest: require the subcontract's valuation date to coincide with the
project's cut-off. Negotiate it at award, when it costs nothing; it is usually impossible later. Where their
accounting calendar makes this impractical, require a *progress return* on your cut-off, separate from the
commercial application.

**Accrue the gap.** Estimate the value of work performed between their period end and your cut-off and post
it as an accrual, so cost and earned value describe the same period. `BPG-07 — Accruals and cut-off
discipline` owns the accounting treatment; the refinement that belongs here is that **the gap is estimated on
the verified run rate, not the claimed one**, which otherwise imports the over-claim and compounds it.

**Do nothing and explain it every month.** Common, and not a control. If it really is the only option,
publish the mismatch on the face of the report — "cost to 20th, progress to 30th". The same mismatch runs the
other way on schedule: their progress update is stale on arrival, and their float is calculated against their
own network, not yours.

## 4. Back-to-back obligations, and the gaps that are not

"Back-to-back" describes a subcontract whose obligations mirror the head contract's, so the main contractor
does not carry an obligation upstream that it cannot pass down. The failures are all in the details, and they
are all arithmetic on dates and percentages.

**Notice regimes.** If the head contract requires notice of a delaying event within a stated period and the
subcontract allows a longer one, there is a window in which you can be time-barred upstream before the
subcontractor is obliged to tell you anything. The exposure is the difference between the two periods, worth
computing for each subcontract rather than assuming the drafting handled it. Notice periods, time bars and
their consequences vary substantially between contract forms and jurisdictions; the arithmetic of comparing
two periods does not.

**Retention.** Percentages and release triggers should mirror; where the subcontract releases retention
earlier, the main contractor funds the difference. `BPG-13 — Cash flow forecasting` covers the consequence.

**Payment timing.** Paying subcontractors on shorter terms than you are paid on is a working-capital leak
whose size is work in progress multiplied by the difference in days — sometimes a deliberate commercial
decision, but one that should carry a number rather than be an accident of two separately negotiated
documents. Statutory payment regimes constrain what may be agreed in many jurisdictions and differ between
them; the principle of pricing the gap is general.

**Change and variation mechanics.** Both processes must be capable of carrying the same instruction, or
changes arrive downstream that cannot be passed upstream; `BPG-11 — Change orders and variations` owns the
process itself.

The controls function's role is not to draft the contract but to hold a **back-to-back matrix** — one table
per subcontract comparing head-contract and subcontract obligation on each dimension, with the gap quantified
and an owner for each gap that cannot be closed.

## 5. Progress claimed versus progress verified

A subcontractor's application states a value of work done; your measurement states a different one. Both may
be prepared honestly — the systematic difference is the ordinary consequence of a party valuing its own
output. Three quantities are in play, and conflating any two produces a reporting error.

| Quantity | What it is | What it drives |
|---|---|---|
| Claimed value | The subcontractor's assertion | Nothing. Never posted to the cost ledger |
| Certified value | What your measurement supports at contract rates, less retention and previous payments | Payment, and through the certificate, the accrual |
| Earned value | Budgeted value of physical work performed under *your* rules of credit | Performance reporting |

Certified value and earned value are close relatives, not the same quantity, and their difference is usually
informative: a valuation typically pays for materials delivered to site, while rules of credit credit
installed work only. That gap is a *timing* difference reversing on installation, and reporting it as a
performance variance is the mistake the worked example illustrates.
`BPG-06 — Progress measurement and rules of credit` owns the rules that make earned value mean something.

Agree the verification method at award, because retrofitting it is a negotiation. In descending order of
reliability: joint measurement of installed quantities against a bill; witnessed milestone achievement
against defined criteria; independent survey on a sample with a stated rule; and last, review of the
subcontractor's own records — a review of an assertion rather than a measurement, and to be labelled as such
wherever the number appears. **Verify on a sample with a rule, not on whatever looks suspicious**, because a
sample chosen by suspicion cannot be extrapolated. And **record the over-claim, do not just remove it**: one
month's over-claim is ordinary, a trend is a forecasting signal.

## 6. When the subcontractor's own controls are weak

Sooner or later you will engage an organisation that is good at the work and poor at the data: no coded cost
structure, progress claimed as a percentage judgement, a schedule in a spreadsheet. It is your problem,
because their scope appears in your report. The signs show in the first two claims: percentages moving in
round numbers, progress tracking the payment curve exactly, a schedule with no logic behind the bars, and no
answer to what the remaining work consists of. The responses that work, roughly in order of cost:

- **Specify the data at award.** A short data schedule — format, fields, frequency, cut-off, consequence of
  non-delivery — costs nothing at award and is unobtainable afterwards. Specify the *fields*, not the tool;
  mandating a software package excludes capable subcontractors and does not by itself produce good data.
- **Measure and schedule it yourself.** Take the quantity survey in-house and maintain your own network for
  their scope, updated from physical observation. It is duplication, it should be an open resourcing decision
  rather than an overload absorbed by one cost engineer, and it is sometimes the only way to keep the
  interface dates honest.
- **Tie data to money.** Where the contract allows, make a compliant progress return a condition of a valid
  application — the strongest lever available, but applied as a standing requirement from the first month,
  because a data requirement introduced during a dispute reads as a payment tactic.
- **Escalate as a commercial matter.** Weak controls raise the probability of latent overrun, unnotified
  delay and disputed final accounts: a risk-register entry with an owner and an exposure.

Whatever the combination, record **the basis of the number that reaches your report**. A cost line from your
own measurement and one from an assertion are different qualities of information.

## 7. How this goes wrong

**The claim is posted as the cost.** No verification, no accrual for the cadence gap, no distinction between
claimed and certified. Everything downstream inherits the subcontractor's commercial position. The refined
version of the same error removes the over-claim from the certificate and reintroduces it through an accrual
calculated on the claimed run rate.

**Interfaces are recorded as milestones.** One owner, one date, no float column, no consequence. When the
date slips, both parties can demonstrate they were waiting for the other.

**Third-party interfaces are missing**, because nobody in the delivery team has a contract with the utility,
the authority or the neighbouring project — usually the longest-lead dependencies on the job.

**Back-to-back is assumed, not checked.** The two documents were reviewed by different people at different
times, and the notice-period and retention differences surface when a claim arrives.

**Materials on site are read as a performance variance.** The index dip is investigated as a productivity
problem, and next month it reverses without explanation (§8.6).

**Their schedule is filed rather than integrated.** A bar chart never linked into the project network cannot
tell you what an interface slip costs, and float comfortable in their network may not exist in yours.

**Weak controls are managed as a relationship issue.** Raised at progress meetings, never entered as a risk,
never priced, never escalated — until the final account arrives with no supporting records on either side.

## 8. Worked example

*Illustrative figures. Currency USD, whole dollars. One subcontract package, viewed by the main contractor.
Assumptions stated as they arise; percentages to one decimal place.*

### 8.1 The setup

| Item | Value |
|---|---|
| Subcontract value | 4,800,000 |
| Programme | 12 months from 1 January, planned value spread evenly |
| Subcontractor's period end | 20th of the month |
| Main contractor's cut-off | Last day of the month |
| Retention | 5 %, back-to-back with the head contract |
| Previously certified, net of retention | 1,600,000 |

At 20 June the subcontractor applies for a cumulative gross value of **2,150,000**; your quantity surveyor's
measure supports **2,020,000**. Measured cumulative value at 20 March was **820,000**. Earned value at
30 June under the project's rules of credit is **2,100,000**.

### 8.2 The over-claim

```
over-claim = claimed − measured
           = 2,150,000 − 2,020,000
           = 130,000

as a proportion of the claim = 130,000 ÷ 2,150,000 = 6.0 %
```

### 8.3 The payment decision — the certificate

```
net certified to date = measured × (1 − retention)
                      = 2,020,000 × 0.95
                      = 1,919,000

this certificate      = net certified to date − previously certified net
                      = 1,919,000 − 1,600,000
                      = 319,000
```

Had the application been certified as submitted:

```
2,150,000 × 0.95 = 2,042,500
2,042,500 − 1,600,000 = 442,500
difference = 442,500 − 319,000 = 123,500
```

which reconciles to the over-claim net of retention: `130,000 × 0.95 = 123,500`. Certifying the application
rather than the measure would have paid **123,500** more than the work supports.

### 8.4 The cost decision — closing the cadence gap

The certificate covers work to 20 June; the cost report closes on 30 June. The gap is **10 days**. The run
rate comes from *verified* measure over the three preceding periods, not from the claims:

```
measured growth  = 2,020,000 − 820,000 = 1,200,000  (20 March to 20 June)
per month        = 1,200,000 ÷ 3 = 400,000
per day (30-day) = 400,000 ÷ 30 = 13,333.33

gap accrual = 13,333.33 × 10 = 133,333
```

Cost to be recognised at 30 June for this package:

```
2,020,000 (certified measure) + 133,333 (accrued 21–30 June) = 2,153,333
```

**A coincidence worth noticing, because it is a trap.** The 2,153,333 is within 3,333 of the 2,150,000
claimed, so posting the claim would have been almost exactly right this month — which is why the shortcut
survives. But the two are right for different reasons and composed differently: 2,153,333 is 2,020,000 of
verified work plus 133,333 of estimated work, and that composition supports three separate actions — certify
319,000, accrue 133,333 with no payment attached, pursue the 130,000 over-claim. The single figure supports
none of them, and the agreement between the two will not survive a change in the over-claim or the run rate.

### 8.5 What it does to the reported indices

*Assumption: planned value is spread evenly across twelve months from 1 January, so planned value at 30 June
is six months of 400,000. SPI is the schedule performance index, earned value divided by planned value.*

```
PV (planned value)  = 6 × 400,000 = 2,400,000
EV (earned value)   = 2,100,000
AC (actual cost)    = 2,153,333

SPI = EV ÷ PV = 2,100,000 ÷ 2,400,000 = 0.875
CPI = EV ÷ AC = 2,100,000 ÷ 2,153,333 = 0.975
```

The same month with the cadence gap ignored — certificate posted, no accrual:

```
AC  = 2,020,000
CPI = 2,100,000 ÷ 2,020,000 = 1.040
swing = 1.040 − 0.975 = 0.065
```

**A swing of 0.065 on the cost performance index, produced entirely by a ten-day mismatch between two
calendars.** On a tolerance band a few hundredths wide that is the difference between green and an exception
report — and next month, when the missing ten days land on top of a full period, the index falls for a reason
unconnected to how the work was performed.

### 8.6 Reading both indices correctly

The 0.975 is not a productivity problem. The recognised cost of 2,153,333 is a valuation basis; the earned
value of 2,100,000 is a rules-of-credit basis, and the difference:

```
2,153,333 − 2,100,000 = 53,333
```

is material delivered to site and paid for, which the valuation includes and the rules of credit do not credit
until installed. It is a timing difference that reverses on installation, and the narrative should say so.

The schedule reading is unaffected by any of the cost treatment above:

```
measured completion           = 2,020,000 ÷ 4,800,000 = 42.1 %
planned completion at 30 June = 2,400,000 ÷ 4,800,000 = 50.0 %
SPI = 0.875
```

A schedule performance index of 0.875 with a 6.0 per cent over-claim on the latest application is a specific
management situation — behind, and valuing optimistically, which is the ordinary sequence — and the moment to
re-confirm the interface dates downstream with the parties that depend on them.

## 9. Checklist

**At award**

- [ ] Valuation date aligned to the project cut-off, or a progress return required on the cut-off.
- [ ] Data schedule in the subcontract: fields, format, frequency, cut-off, consequence of non-delivery.
- [ ] Verification method stated — joint measurement, witnessed milestone, sampled survey or record review.
- [ ] Back-to-back matrix completed: notice periods, retention, payment timing, change mechanics — each gap
      quantified, each with an owner.
- [ ] Interface points identified, including third parties with no contract.
- [ ] Rules of credit agreed, distinct from the valuation basis.

**Every period**

- [ ] Claim measured before it is certified; over-claim recorded, not just removed.
- [ ] Certificate computed from measure, retention and previous payments, with the arithmetic shown.
- [ ] Cadence gap accrued, using the verified run rate.
- [ ] Cost basis labelled: measured, certified, accrued or asserted.
- [ ] Materials on site identified and excluded from the performance narrative.
- [ ] Subcontractor's progress update integrated into the project network, not filed.
- [ ] Interface register updated with dates confirmed by *both* parties, and float recomputed.
- [ ] Over-claim trend reviewed across periods, not only this month's figure.

**Every quarter, or at each gate**

- [ ] Interfaces with no bilateral confirmation in the agreed period flagged regardless of float.
- [ ] Subcontractors with weak controls carried as named risk entries with an exposure.
- [ ] Unclosed back-to-back gaps reviewed for whether they have been triggered.

Projects rarely lose control of subcontracted scope through one large failure. They do it through a claim
posted as a cost, an interface with one owner and a notice period seven days out of step — each small, each
invisible in the report, and all of them arriving together in the final account.

---

## Related

- `BPG-06 — Progress measurement and rules of credit` — what makes earned value on subcontracted scope mean something
- `BPG-07 — Accruals and cut-off discipline` — the accounting treatment behind the gap accrual in §8.4
- `BPG-11 — Change orders and variations` — the change process that must carry an instruction both ways
- `BPG-12 — Claims and extension of time` — where notice-period gaps and unpassed subcontractor claims end up
- `BPG-13 — Cash flow forecasting` — the working-capital effect of terms that are not back-to-back
- `BPG-05 — Schedule quality — a practical review` — the checks to apply before integrating their schedule

## Sources and standards

- PCL-AI Body of Knowledge (`docs/bok/`), Domain 7 — Contracts, Commercial Management, BoQ, Invoicing and
  Revenue, first authored draft, August 2026: interim valuation and certification, back-to-back subcontract
  discipline, and the reconciliation of earned value, valuation and revenue.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 5 — Cost Management and Cost Control, first authored draft,
  August 2026: commitment and accrual capture, and the cost ledger's relationship to certification.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 10 — Project Scheduling, first authored draft, August 2026:
  integration of contractor schedules and float across networks.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  naming, status and claims policy.

Notice periods, time bars, retention mechanics, payment terms and statutory payment regimes vary between
contract forms and jurisdictions. This guide names no contract form and reproduces no clause; the arithmetic
is to be applied to the documents in force on the reader's project.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
