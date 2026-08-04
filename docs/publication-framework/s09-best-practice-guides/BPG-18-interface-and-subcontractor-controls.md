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
  is not a milestone, the reporting cadence mismatch between a subcontractor's period end and your cut-off,
  keeping subcontract obligations back-to-back with the head contract, the difference between progress
  claimed and progress verified, and what to do when a subcontractor's own controls are too weak to produce
  the data your reporting depends on. The worked example separates one subcontractor claim into a payment
  decision and a cost decision, and shows a swing of 0.065 on the cost performance index that came from a
  cadence mismatch alone.
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
an interface is not a milestone, the reporting cadence mismatch between a subcontractor's period end and
your cut-off, keeping subcontract obligations back-to-back with the head contract, the difference between
progress claimed and progress verified, and what to do when a subcontractor's own controls are too weak to
produce the data your reporting depends on. The worked example separates one subcontractor claim into a
payment decision and a cost decision, and shows a swing of 0.065 on the cost performance index that came
from a cadence mismatch alone.

**Who this is for.** Project controls managers and cost engineers on projects with substantial subcontracted
scope; commercial managers and quantity surveyors who certify subcontractor applications; and planners
managing interfaces between contractors, clients and third parties.

---

## 1. The control problem when the work is somebody else's

Direct work gives a controls function primary data: your timesheets, your goods-received notes, your
progress measurement, on your cut-off. Subcontracted work gives you *reported* data: a claim, prepared by an
organisation with a commercial interest in the number, covering a period that ends when their accounting
month ends, measured against rules they applied.

Three properties follow, and every failure in this area is a version of one of them.

**The data is an assertion until you verify it.** A subcontractor's application is a starting position in a
commercial conversation, not a measurement. Treating it as a measurement transfers their optimism directly
into your cost report and, through the cost report, into your forecast.

**The data arrives on their calendar.** Their period end and your cut-off are different dates and there is
usually no contractual reason for them to converge. The gap is not a rounding issue; it is a systematic
mismatch between numerator and denominator that recurs every month in the same direction.

**Their control weakness becomes your reporting weakness.** If a subcontractor cannot produce reliable
progress data, no clause in the subcontract converts data you never received into a number you can report.
You will either measure their work yourself or report an estimate, and the honest choice is to decide which
in advance rather than to discover it at cut-off.

The rest of this guide is about installing controls against those three properties, at the point where the
work crosses an organisational boundary.

## 2. The interface register, and why an interface is not a milestone

An **interface** is a point where a deliverable, a piece of information, an access, a service or a physical
connection passes between two organisations. Interfaces are where integrated projects fail, because both
sides believe they are managing them and neither owns the gap.

The register that manages them is not the milestone list. A milestone has one owner and one date. **An
interface has two owners, two schedules and a tolerance**, and its defining property is that neither party
can deliver it alone. That distinction determines the columns:

| Field | Why it exists |
|---|---|
| Interface reference | A permanent identifier that both parties quote; never renumbered |
| Parties | The two organisations, named — not "us and them" |
| Direction | Who provides, who receives; an interface with no direction is two interfaces |
| What crosses | The specific deliverable, information set, access or connection, defined tightly enough to be checked |
| Acceptance criterion | What the receiving party must be able to do with it before it counts as delivered |
| Required-by date | The receiver's need date, taken from their schedule |
| Committed date | The provider's date, taken from theirs |
| Float between them | Required-by minus committed; the interface's health, in days |
| Owner — provider side | A named individual |
| Owner — receiver side | A named individual |
| Consequence of late | What it costs, in days or money, expressed against the receiver's critical path |
| Status and last confirmed | When both parties last agreed the dates, not when the register was edited |

Two of those columns do the real work. **Float between the two dates** turns an interface from a status into
a number, and a register sorted by that number is a management agenda. **Consequence of late** is what makes
the provider's organisation care; an interface with no stated consequence is a request.

The discipline that makes the register live is bilateral confirmation. A date recorded from one party's
schedule and never confirmed by the other is not an agreement — it is a hope with a date field. The interface
review should confirm dates from both sides on the record, and an interface whose dates have not been jointly
confirmed within an agreed period should be flagged regardless of how comfortable its float looks, because
comfortable float is exactly the condition under which nobody checks.

Interfaces with third parties who are not under any contract — utilities, authorities, neighbouring
projects, the client's own operations teams — need the same treatment and are the ones most often missing
from the register entirely. They have no commercial lever behind them, so the only control is early
identification, long lead times and explicit contingency. `BPG-17 — Quantitative schedule risk analysis`
covers what convergence of several such paths does to a completion date, and the answer is usually worse than
the schedule shows.

## 3. Cadence mismatch: their period end and your cut-off

Subcontractors report on their own commercial cycle. A common pattern is a claim to a fixed day of the month
— the twentieth, say — valued and submitted a few days later, while your cost cut-off is the last day of the
month and your progress measurement is taken at the same time.

If the claim is posted unadjusted, the month's cost covers work to the twentieth and the month's earned value
covers work to the thirtieth. The cost performance index — earned value divided by actual cost — then
compares thirty days of output with twenty days of input, and it flatters. Next month the missing ten days
arrive on top of a full month, and the index drops for a reason that has nothing to do with performance. The
result is a sawtooth that costs the controls function its credibility, because the pattern is visible to
everyone and the explanation sounds like an excuse.

There are three responses and only one of them is generally right.

**Move their cut-off.** Contractually the cleanest: require the subcontract's valuation date to coincide with
the project's cut-off. It is worth negotiating at award, when it costs nothing, and it is usually impossible
to change later. Where a subcontractor's own accounting calendar makes this genuinely impractical, the
requirement can be limited to a *progress return* on your cut-off, separate from the commercial application.

**Accrue the gap.** Where the dates cannot be aligned, estimate the value of work performed between their
period end and your cut-off and post it as an accrual, so that cost and earned value describe the same
period. This is standard cut-off discipline — `BPG-07 — Accruals and cut-off discipline` owns the accounting
treatment — with one important refinement that belongs here: **the gap should be estimated on the verified
run rate, not on the claimed one**, because using the claimed rate imports the over-claim into the estimate
and compounds it.

**Do nothing and explain it every month.** Common, and it is not a control. If it is genuinely the only
option, at least publish the mismatch on the face of the report — "cost to 20th, progress to 30th" — so that
readers do not attribute the pattern to performance.

The same mismatch runs in the other direction on schedule. A subcontractor's progress update, prepared on
their data date, is stale by the time it enters your integrated schedule, and their float is calculated
against their own network, not yours. An activity with ten days of float in the subcontractor's schedule may
have none in yours, because yours contains the successor that matters.

## 4. Back-to-back obligations, and the gaps that are not

"Back-to-back" describes a subcontract whose obligations mirror the head contract's, so that the main
contractor is not left carrying an obligation upstream that it cannot pass down. The concept is
straightforward; the failures are all in the details, and they are all arithmetic on dates and percentages.

Four dimensions to check, contract by contract:

**Notice regimes.** If the head contract requires notice of a delaying event within a stated period and the
subcontract allows the subcontractor a longer one, there is a window in which you can be time-barred upstream
before the subcontractor is obliged to tell you anything. The exposure is the difference between the two
periods, and it is worth computing explicitly for each subcontract rather than assuming the drafting handled
it. Notice periods, time bars and their consequences vary substantially between contract forms and between
jurisdictions; the arithmetic of comparing two periods does not.

**Retention.** Percentages, the trigger for the first release and the trigger for the balance should mirror.
Where the subcontract releases retention earlier than the head contract, the main contractor funds the
difference. `BPG-13 — Cash flow forecasting` covers the working-capital consequence.

**Payment timing.** Paying subcontractors on shorter terms than you are paid on is a working-capital leak
whose size is the value of work in progress multiplied by the difference in days. It may be a deliberate
commercial or relationship decision — it often should be — but it should be a decision with a number
attached, not an accident of two separately negotiated documents. Statutory payment regimes constrain what
may be agreed in many jurisdictions, and they differ; the principle of pricing the gap does not.

**Change and variation mechanics.** If the head contract requires a variation to be instructed and valued in
a particular way and the subcontract permits a different route, changes will arrive downstream that cannot
be passed upstream. `BPG-11 — Change orders and variations` owns the change process; the interface point is
that the two processes must be capable of carrying the same instruction.

The controls function's role here is not to draft the contract. It is to hold a **back-to-back matrix** — a
single table, per subcontract, comparing head-contract obligation and subcontract obligation on each of these
dimensions, with the gap quantified in days, percentage points or currency, and an owner for each gap that
cannot be closed. That table is what turns a legal review into a managed exposure.

## 5. Progress claimed versus progress verified

A subcontractor's application states a value of work done. Your measurement states a different one. Both may
be prepared honestly; the systematic difference between them is not fraud but the ordinary consequence of a
party valuing its own output.

Three separate quantities are in play, and conflating any two of them produces a reporting error:

**Claimed value** — the subcontractor's assertion. Never posted to the cost ledger.

**Certified value** — what your measurement supports, valued at contract rates, less retention and previous
payments. This drives payment and, through the certificate, the accrual.

**Earned value** — the budgeted value of physical work performed under *your* rules of credit. This drives
performance reporting.

Certified value and earned value are close relatives but not the same quantity, and their difference is
usually informative. A valuation typically pays for materials delivered to site; rules of credit typically
credit installed work only. The gap between the two is therefore a *timing* difference that reverses when the
material is installed, and reporting it as a performance variance is a mistake the worked example
illustrates. `BPG-06 — Progress measurement and rules of credit` owns the rules that make earned value
mean something.

The verification method should be agreed at award and stated in the subcontract, because retrofitting it is
a negotiation. What works, in descending order of reliability: joint measurement of installed quantities
against a bill; witnessed milestone achievement against defined completion criteria; independent quantity
survey on a sample with a stated sampling rule; and, last, review of the subcontractor's own records. The
first three are measurements. The fourth is a review of an assertion, and should be labelled as such
wherever the resulting number appears.

Two practical rules. **Verify on a sample with a rule, not on whatever looks suspicious**, because a sample
chosen by suspicion cannot be extrapolated and a sample chosen by rule can. And **record the over-claim, do
not just remove it** — a subcontractor whose claims run consistently above measure is a forecasting signal
and, eventually, a commercial conversation. A single month's over-claim is ordinary; a trend is information.

## 6. When the subcontractor's own controls are weak

Sooner or later you will engage an organisation that is good at the work and poor at the data: no coded cost
structure, progress claimed as a percentage judgement, a schedule maintained in a spreadsheet, and a
programme manager who regards all of this as your problem. It is your problem, because their scope still has
to appear in your report.

Recognise it early. The diagnostic signs are visible in the first two claims: percentages that move in round
numbers, progress that tracks the payment curve exactly, a schedule with no logic behind the bars, no
resource information, and an inability to answer what the remaining work consists of.

The responses that work, roughly in order of cost:

**Specify the data at award.** A short data schedule in the subcontract — the format, the fields, the
frequency, the cut-off date and the consequence of non-delivery — costs nothing at award and is unobtainable
afterwards. Specify the *fields*, not the tool; requiring a particular software package excludes capable
subcontractors and does not by itself produce good data.

**Measure it yourself.** Where their measurement cannot be relied on, take the quantity survey in-house and
budget for it. This is the most common practical answer and it should be a resourcing decision made openly,
not an overload absorbed quietly by one cost engineer.

**Shadow-schedule the scope.** Maintain your own network for their work at a level sufficient to drive your
integrated schedule, updated from physical observation rather than from their reporting. It is duplication
and it is sometimes the only way to keep the interface dates honest.

**Tie data to money.** Where the contract allows, make a compliant progress return a condition of a valid
application. This is the strongest lever available and it works, but it should be applied as a standing
requirement from the first month rather than deployed as a sanction later, because a data requirement
introduced during a dispute reads as a payment tactic.

**Escalate as a commercial matter, not a technical one.** Weak controls raise the probability of latent
overrun, unnotified delay and disputed final accounts. That is a risk-register entry with an owner and an
exposure, not a grumble in a progress meeting.

Whatever combination is used, one thing must be recorded: **the basis of the number that reaches your
report**. A cost line derived from your own measurement and a cost line derived from a subcontractor's
assertion are different qualities of information, and a report that presents them identically has lost
information its readers need.

## 7. How this goes wrong

**The claim is posted as the cost.** No verification, no accrual for the cadence gap, no distinction between
claimed and certified. Everything downstream — index, forecast, revenue recognition — inherits the
subcontractor's commercial position.

**The gap accrual is estimated from the claimed rate.** A refinement of the same error: the over-claim is
removed from the certificate and then reintroduced through the accrual, which was calculated using the
claim's run rate.

**Interfaces are recorded as milestones.** One owner, one date, no float column, no consequence. When the
date slips, both parties can demonstrate that they were waiting for the other.

**Third-party interfaces are missing.** Utilities, permitting authorities, the client's operations staff and
neighbouring projects appear nowhere, because nobody in the delivery team has a contract with them. These
are usually the longest-lead and least controllable dependencies on the project.

**Back-to-back is assumed, not checked.** The head contract and the subcontract were reviewed by different
people at different times, and the notice-period and retention differences are discovered when a claim
arrives.

**Materials on site are read as a performance variance.** The valuation pays for stored material, the rules
of credit do not credit it, the resulting index dip is investigated as a productivity problem, and next month
it reverses without explanation.

**Their float is treated as your float.** A subcontractor reports comfortable float against their own
network. Your successor activities are not in their network.

**The subcontractor's schedule is accepted rather than integrated.** Received as a bar chart, filed, and not
linked into the project network. The consequence appears at the first interface slip, when nobody can compute
its effect.

**Weak controls are managed as a relationship issue.** Raised repeatedly at progress meetings, never entered
as a risk, never priced, never escalated commercially — until the final account arrives with no supporting
records on either side.

## 8. Worked example

*Illustrative figures. Currency is USD, whole dollars unless stated. One subcontract package, viewed by the
main contractor. Assumptions stated as they arise; percentages rounded to one decimal place.*

### 8.1 The setup

| Item | Value |
|---|---|
| Subcontract value | 4,800,000 |
| Programme | 12 months from 1 January, planned value spread evenly |
| Subcontractor's period end | 20th of the month |
| Main contractor's cut-off | Last day of the month |
| Retention | 5 %, back-to-back with the head contract |
| Previously certified, net of retention | 1,600,000 |

At 20 June the subcontractor applies for a cumulative gross value of **2,150,000**. Your quantity surveyor's
measure at the same date supports **2,020,000**. Measured cumulative value at 20 March was **820,000**.
Earned value for the package at 30 June, under the project's rules of credit, is **2,100,000**.

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
rather than the measure would have paid **123,500** more than the work supports, this month.

### 8.4 The cost decision — closing the cadence gap

The certificate covers work to 20 June. The cost report closes on 30 June. The gap is **10 days**.

The run rate is taken from *verified* measure over the three preceding periods, not from the claims:

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

**A coincidence worth noticing, because it is a trap.** The number arrived at — 2,153,333 — is within 3,333
of the 2,150,000 the subcontractor claimed. A cost engineer who had simply posted the claim would have been
almost exactly right this month, which is precisely why the shortcut survives. But the two figures are right
for different reasons and are composed differently: 2,153,333 is 2,020,000 of verified work plus 133,333 of
estimated work, which supports three separate actions — certify 319,000, accrue 133,333 with no payment
attached, and pursue the 130,000 over-claim. The single figure of 2,150,000 supports none of them, and the
agreement between the two numbers will not survive any change in the over-claim or the run rate.

### 8.5 What it does to the reported indices

*Assumption: the package's planned value is spread evenly across twelve months from 1 January, so planned
value at 30 June is six months of 400,000. SPI is the schedule performance index, earned value divided by
planned value.*

```
PV (planned value)  = 6 × 400,000 = 2,400,000
EV (earned value)   = 2,100,000
AC (actual cost)    = 2,153,333

SPI = EV ÷ PV = 2,100,000 ÷ 2,400,000 = 0.875
CPI = EV ÷ AC = 2,100,000 ÷ 2,153,333 = 0.975
```

Now the same month with the cadence gap ignored — certificate posted, no accrual:

```
AC = 2,020,000
CPI = 2,100,000 ÷ 2,020,000 = 1.040
```

```
swing = 1.040 − 0.975 = 0.065
```

**A swing of 0.065 on the cost performance index, produced entirely by a ten-day mismatch between two
calendars.** On a tolerance band a few hundredths wide, that is the difference between green and an
exception report — and next month, when the missing ten days land on top of a full period, the index will
fall for a reason that has nothing to do with how the work was performed.

### 8.6 Reading the 0.975 correctly

The 0.975 should not be reported as a productivity problem. The recognised cost of 2,153,333 is a valuation
basis; the earned value of 2,100,000 is a rules-of-credit basis. The difference:

```
2,153,333 − 2,100,000 = 53,333
```

is material delivered to site and paid for, which the valuation includes and the rules of credit do not
credit until it is installed. It is a timing difference that reverses on installation, and the narrative
should say so. Investigating it as a performance variance would consume a month of attention and find
nothing, which is a real cost even though it does not appear in any ledger.

### 8.7 The schedule reading

The package is behind, and that reading is unaffected by any of the cost treatment above:

```
measured completion = 2,020,000 ÷ 4,800,000 = 42.1 %
planned completion at 30 June = 2,400,000 ÷ 4,800,000 = 50.0 %
SPI = 0.875
```

A package at a schedule performance index of 0.875 with a 6.0 per cent over-claim on its latest application
is a specific management situation: the subcontractor is behind and is valuing optimistically, which is the
ordinary sequence and the moment at which the interface dates downstream of this package should be
re-confirmed with the parties that depend on them.

## 9. Checklist

**At award**

- [ ] Valuation date aligned to the project cut-off, or a separate progress return required on the cut-off.
- [ ] Data schedule in the subcontract: fields, format, frequency, cut-off, consequence of non-delivery.
- [ ] Verification method stated — joint measurement, witnessed milestone, sampled survey or record review.
- [ ] Back-to-back matrix completed: notice periods, retention percentages and triggers, payment timing,
      change mechanics — each gap quantified in days, points or currency, each with an owner.
- [ ] Interface points identified, including third parties with no contract.
- [ ] Rules of credit for the package agreed and written, distinct from the valuation basis.

**Every period**

- [ ] Claim measured before it is certified; over-claim recorded, not just removed.
- [ ] Certificate computed from measure, retention and previous payments — with the arithmetic shown.
- [ ] Cadence gap accrued, using the verified run rate.
- [ ] Cost basis labelled: measured, certified, accrued or asserted.
- [ ] Materials on site identified and excluded from the performance narrative.
- [ ] Subcontractor's progress update integrated into the project network, not filed.
- [ ] Interface register updated with dates confirmed by *both* parties, and float recomputed.
- [ ] Over-claim trend reviewed across periods, not only this month's figure.

**Every quarter, or at each gate**

- [ ] Interfaces with no bilateral confirmation in the agreed period flagged regardless of float.
- [ ] Subcontractors with weak controls carried as named risk entries with an exposure, not as grievances.
- [ ] Back-to-back gaps that could not be closed reviewed for whether they have been triggered.
- [ ] The cost of measuring subcontracted work in-house reviewed against the resource actually assigned.

The projects that lose control of subcontracted scope rarely do so through one large failure. They do it
through a claim posted as a cost, an interface with one owner and a notice period that was seven days out of
step — each individually small, each invisible in the report, and all of them arriving together in the final
account.

---

## Related

- `BPG-06 — Progress measurement and rules of credit` — the rules that make earned value on subcontracted scope mean something
- `BPG-07 — Accruals and cut-off discipline` — the accounting treatment behind the gap accrual in §8.4
- `BPG-11 — Change orders and variations` — the change process that must be capable of carrying an instruction in both directions
- `BPG-12 — Claims and extension of time` — where notice-period gaps and unpassed subcontractor claims end up
- `BPG-13 — Cash flow forecasting` — the working-capital effect of payment and retention terms that are not back-to-back
- `BPG-05 — Schedule quality — a practical review` — the checks to apply to a subcontractor's schedule before integrating it

## Sources and standards

- PCL-AI Body of Knowledge (`docs/bok/`), Domain 7 — Contracts, Commercial Management, BoQ, Invoicing and
  Revenue, first authored draft, August 2026: interim valuation and certification, back-to-back subcontract
  discipline, and the reconciliation of earned value, valuation and revenue.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 5 — Cost Management and Cost Control, first authored draft,
  August 2026: commitment and accrual capture, and the cost ledger's relationship to certification.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 10 — Project Scheduling, first authored draft, August 2026:
  integration of contractor schedules and the treatment of float across networks.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  naming, status and claims policy.

Notice periods, time bars, retention mechanics, payment terms and statutory payment regimes vary between
contract forms and between jurisdictions. This guide names no contract form and reproduces no clause; where
a mechanism is described it is described generically, and the reader is expected to apply the arithmetic to
the documents actually in force on their project.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
