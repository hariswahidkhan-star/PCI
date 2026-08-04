---
id: BPG-13
series: S09
series_name: Best Practice Guides
title: Cash flow forecasting
subtitle: The gap between doing the work and having the money
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: practitioner
reading_time_min: 15
summary: >
  A cost S-curve says nothing about whether a project can pay its people next month. This guide sets out the
  four mechanisms that separate progress from cash — measurement and certification, payment terms, retention
  and advance recovery — builds a monthly forecast through the certificate cascade, identifies the peak
  funding requirement, shows why a profitable project can still fail on cash, and runs the payment-timing
  sensitivity that turns a forecast into a decision about facilities.
linkedin:
  format: article
  hook: >
    One month of extra payment delay raised the peak funding requirement on this package from 2.08 million
    to 3.07 million — a 47.6 % increase, with the profit, the scope and the programme all unchanged.
  tags: [ProjectControls, CashFlow, CostEngineering, ProjectFinance]
  asset: one-pager
gated: false
related: [BPG-07, BPG-09, BPG-11, BPG-14, TPL-09]
bok_domains: [3, 7, 11]
sources: []
placeholders: 0
---

# Cash flow forecasting

> The gap between doing the work and having the money.

**In one paragraph.** A cost S-curve says nothing about whether a project can pay its people next month.
This guide sets out the four mechanisms that separate progress from cash — measurement and certification,
payment terms, retention and advance recovery — builds a monthly forecast through the certificate cascade,
identifies the peak funding requirement, shows why a profitable project can still fail on cash, and runs the
payment-timing sensitivity that turns a forecast into a decision about facilities.

**Who this is for.** Cost engineers, commercial managers and project controls managers who produce or rely
on a project cash forecast, and the finance and treasury colleagues who have to fund the answer.

---

## 1. The S-curve is a cost curve

The cumulative cost curve every project draws describes when value is *earned* and cost is *incurred*. Cash
is a different curve with a different shape, and on most contracts it sits well below and well behind the
cost curve for most of the project's life.

The distinction is not academic. A project can be exactly on budget, exactly on programme, and unable to pay
its subcontractors, because being owed money and having money are different states. Every mechanism that
separates them is contractual, predictable and modellable — which means a funding shortfall is almost always
a forecasting failure rather than a surprise.

Two curves are therefore needed, and they answer different questions:

| Curve | Question it answers | Driven by |
|---|---|---|
| Cumulative cost / earned value | Are we going to be within budget? | Progress and productivity |
| Cumulative net cash | Can we pay for the work while we do it? | Certification, terms, retention, advances |

## 2. The four gaps

**Gap 1 — measurement and certification.** Work performed is not work valued. The contractor applies for
what it believes the work is worth; the certifier assesses; the certified figure, not the applied figure, is
what becomes payable. Persistent under-certification is a commercial issue, but for the forecast it is a
simple discipline: **forecast on expected certified value, and hold the application-to-certification gap as
a stated assumption** rather than assuming full certification and treating shortfalls as bad luck.

**Gap 2 — payment terms.** Certified sums are paid after a contractual period running from the certificate
or the application. On a monthly cycle the practical lag between performing work and receiving cash for it
is usually two months, sometimes three, and the difference between those two is enormous — §9 quantifies it.

**Gap 3 — retention.** A percentage of each certified amount is withheld as security, released in stages,
often long after the work is done and partly after a defects period. Retention converts margin into a
deferred receivable. A project can finish, hand over, be paid everything payable, and still be cash-negative
until release.

**Gap 4 — advance recovery.** Where a mobilisation advance is paid, it is a loan repaid through the
measure — recovered as a deduction on each certificate, usually at a stated percentage of the gross value.
The forecast must carry both the early inflow and the reduced net certificates that repay it, or the advance
looks like free money in the first month and an unexplained shortfall thereafter.

Together these four produce the shape every project cash curve has: a trough, whose depth and timing are the
things that actually need managing.

## 3. Building the forecast: the certificate cascade

The inflow side of the forecast is one repeated calculation, per period:

```
Gross value certified        (from the valuation of work done)
 − retention                 (stated % of gross)
 − advance recovery          (stated % of gross, until the advance is repaid)
 − previous payments         (where the valuation is cumulative)
 = net amount due
 → received on the payment date given by the contract terms
```

Three modelling rules make it reliable:

**Model the value, then the deductions, then the date — separately.** Collapsing them into one net figure
per month makes the forecast impossible to interrogate when it goes wrong, and it will go wrong.

**Phase from the schedule, not from the calendar.** Gross value per period comes from the time-phased
baseline and the current forecast of progress. A cash forecast built by spreading the contract value evenly
across the programme is not a forecast.

**Carry retention forward explicitly, with its release dates.** Retention is not a cost and it is not lost;
it is cash held. It belongs on the forecast as a receivable with dates, and somebody must be accountable for
recovering it. Retention that nobody chases is a real and recurring loss.

## 4. The outflow side

Outflows are governed by different terms from inflows, and the mismatch is the whole problem.

- **Labour** is paid weekly or monthly, in arrears of days rather than months. It is close to
  uncompressible.
- **Subcontractors** are paid on their own terms. Where those terms are shorter than the terms on which the
  main contract pays, every period of work is being funded by the contractor.
- **Materials and equipment** are paid on supplier terms, sometimes with deposits or stage payments before
  delivery — an outflow that precedes any possibility of certifying the corresponding value.
- **Plant and preliminaries** run at a rate per period regardless of progress, which is why they dominate the
  cash consequence of any delay.

The two levers on this side are supplier payment terms and the timing of procurement commitments. Both have
limits: stretching supplier terms transfers a funding problem down the supply chain, has commercial
consequences, and in some jurisdictions is regulated. Treat it as a lever with a cost, not as free
financing.

## 5. The funding trough

The deepest point of the cumulative net cash curve is the **peak funding requirement** — the amount of money
the project must have available, from a facility, from group treasury, or from partners, in order to keep
operating. It is a single number, it has a date, and it is the most useful output of the whole exercise.

Its drivers are precisely the levers a commercial and controls team manages: certification performance,
payment terms both ways, retention percentage and release profile, billing cadence, advance payments and
their recovery rate, and the margin itself.

Two consequences follow that reports rarely state:

**The trough is a decision, not a fact.** Every driver in that list is negotiable at some point — at tender,
at award, or in the running of the job. A forecast that presents the trough as a given has skipped the
conversation that matters.

**The trough moves for reasons that have nothing to do with performance.** A perfectly performing project
whose client moves from 30-day to 60-day payment has a materially worse funding position and an unchanged
cost report. That divergence is exactly why cash is reported separately.

## 6. Why a profitable project fails on cash

Profit is measured on value earned against cost incurred. Cash is measured on money received against money
paid. On a contract of any length the second is far more volatile than the first, for a structural reason:
cost is incurred continuously and revenue is collected in lumps, late, net of deductions.

Three patterns account for most failures.

**Growth.** A contractor winning more work funds each new project's trough before the previous one has
recovered. The business is profitable and getting steadily closer to insolvency. This is why the aggregate
funding profile across a portfolio matters as much as any single project's.

**Front-loaded cost, back-loaded value.** Design, mobilisation, temporary works, procurement deposits and
long-lead equipment are paid early and certified late — or, in some structures, not certified as value at
all until incorporated into the works.

**Retention plus a long defects period.** The entire margin can be smaller than the retention held. Where
that is true, the project's profitability is contingent on a release that happens a year after everyone has
moved on.

## 7. Sensitivity: change one thing at a time

A cash forecast that is presented as a single line is not decision-ready. Run a small, disciplined set of
sensitivities, each moving exactly one variable, and report the effect on two numbers only: the depth of the
trough and its date.

The four worth running on almost every project:

1. **Payment terms slip by one period.** The single most common and most damaging change.
2. **Certification at less than application.** A stated percentage, with the balance certified later.
3. **No advance, or a slower advance.** Tests how much of the funding position depends on it.
4. **Retention released late.** Moves the end of the curve rather than the trough, but decides when the
   margin becomes real.

Moving one variable at a time is not a stylistic preference. A forecast that changes three assumptions
simultaneously produces a number nobody can attribute, and attribution is the entire purpose — the point is
to know which lever to pull.

## 8. Timing effects that are not costs

Several flows move cash without changing project cost, and they belong in the forecast for their timing
alone.

Where an indirect tax such as a value-added or goods-and-services tax applies, invoices are issued and paid
gross of it, and the tax then leaves again on its own remittance calendar. Whether such a tax applies, at
what rate, on what basis and with what remittance timing depends entirely on the jurisdiction and is a
question for the finance function — but where it applies, the gross flows can be material at the trough.
Similarly, where withholding applies to cross-border payments, the total cash is unchanged but its
counterparties and dates are not. Neither affects project cost; both affect the funding requirement.

The discipline is to model gross flows with their remittance dates, and never to let a tax-inclusive invoice
value reach the cost ledger.

## 9. How this goes wrong

**The cash forecast is the cost curve with a lag applied.** A single blanket delay applied to the S-curve,
with no retention, no advance recovery and no certification assumption. It will be wrong by the size of the
deductions, which is the size of the problem.

**Forecasting on applications rather than certificates.** Optimistic by exactly the assessment gap, every
month, cumulatively.

**Retention modelled as lost or forgotten entirely.** Either the forecast understates the eventual position
or it overstates the current one. Model it as held cash with release dates and an owner.

**The advance treated as income.** It arrives, the curve lifts, and nobody models the recovery deductions
that follow. The second and third months then look inexplicably poor.

**Ignoring the outflow terms.** Modelling receipts carefully and assuming all costs are paid in the month
incurred understates the position on projects with long supplier terms and overstates it where subcontractors
are paid quickly.

**No sensitivity on payment timing.** The one assumption most likely to change, and the one most often
presented as fixed.

**Unpriced variations invisible in the cash line.** Work instructed but not agreed is being funded with
nothing certified against it. A growing unpriced book is a cash problem before it is a commercial one — see
`BPG-11 — Change orders and variations`.

**One project at a time.** The funding requirement that matters to a business is the aggregate of its
projects' troughs at each point in time, which is not the sum of their peak requirements and is never
visible from any single project's report.

## 10. Worked example

*Illustrative figures.* Currency USD; monthly periods; a six-month package of contract value 6,000,000;
retention 5 % of gross certified value; mobilisation advance 8 % of contract value, paid at the start of
Month 0 and recovered at 8 % of each gross certificate; the certifier is assumed to certify the full applied
value; payment is received two months after the month in which the work is performed; all outflows are
assumed paid in the month the cost is incurred; total cost 5,400,000, giving a margin of 600,000, which is
10 % of contract value. Figures are rounded to the nearest whole unit and are exact as stated.

### 10.1 The certificate cascade

| Month | Gross value certified | Retention 5 % | Advance recovery 8 % | Net amount due |
|---|---:|---:|---:|---:|
| 1 | 500,000 | 25,000 | 40,000 | 435,000 |
| 2 | 900,000 | 45,000 | 72,000 | 783,000 |
| 3 | 1,300,000 | 65,000 | 104,000 | 1,131,000 |
| 4 | 1,500,000 | 75,000 | 120,000 | 1,305,000 |
| 5 | 1,100,000 | 55,000 | 88,000 | 957,000 |
| 6 | 700,000 | 35,000 | 56,000 | 609,000 |
| **Total** | **6,000,000** | **300,000** | **480,000** | **5,220,000** |

Two reconciliations to run every time, because they catch most modelling errors:

```
Retention total       = 5 % × 6,000,000 = 300,000 ✓
Advance recovered     = 8 % × 6,000,000 = 480,000 = the advance paid ✓
Net certified total   = 6,000,000 − 300,000 − 480,000 = 5,220,000 ✓
```

The second one matters: the recovery percentage must be set so that the advance is fully repaid by the end
of the works. Set it too low and the final certificates carry a balance nobody has planned for.

### 10.2 The cash position, month by month

Costs are `0.9 × gross value` in each month, paid in the month incurred. Receipts arrive two months after
the work month.

| Month | Cash in | Cash out | Net in month | Cumulative cash |
|---|---:|---:|---:|---:|
| 0 | 480,000 (advance) | — | 480,000 | 480,000 |
| 1 | — | 450,000 | (450,000) | 30,000 |
| 2 | — | 810,000 | (810,000) | (780,000) |
| 3 | 435,000 | 1,170,000 | (735,000) | **(1,515,000)** |
| 4 | 783,000 | 1,350,000 | (567,000) | **(2,082,000)** |
| 5 | 1,131,000 | 990,000 | 141,000 | (1,941,000) |
| 6 | 1,305,000 | 630,000 | 675,000 | (1,266,000) |
| 7 | 957,000 | — | 957,000 | (309,000) |
| 8 | 609,000 | — | 609,000 | 300,000 |

```
Peak funding requirement = (2,082,000), at the end of Month 4
Total cash in  = 480,000 + 5,220,000 = 5,700,000
Total cash out = 5,400,000
Closing position at Month 8 = 300,000
```

The closing 300,000 is not the margin. The other 300,000 is sitting in retention, released in two tranches —
half at completion and half after the defects period — at which point the cumulative position reaches
`300,000 + 150,000 + 150,000 = 600,000`, the margin, in cash, roughly a year after the work finished.

### 10.3 Profitable, and needing 2.08 million

The package makes 600,000 on 6,000,000 of value. To make it, the business must fund 2,082,000 at Month 4:

```
Peak funding ÷ margin = 2,082,000 ÷ 600,000 = 3.47
```

The funding requirement is about three and a half times the entire profit on the job. Nothing has gone
wrong; this is what a normally performing contract looks like.

### 10.4 Sensitivity 1 — payment one month later

Everything unchanged except that receipts arrive three months after the work month rather than two:

| Month | Cash in | Cash out | Cumulative cash |
|---|---:|---:|---:|
| 0 | 480,000 | — | 480,000 |
| 1 | — | 450,000 | 30,000 |
| 2 | — | 810,000 | (780,000) |
| 3 | — | 1,170,000 | (1,950,000) |
| 4 | 435,000 | 1,350,000 | (2,865,000) |
| 5 | 783,000 | 990,000 | **(3,072,000)** |
| 6 | 1,131,000 | 630,000 | (2,571,000) |
| 7 | 1,305,000 | — | (1,266,000) |
| 8 | 957,000 | — | (309,000) |
| 9 | 609,000 | — | 300,000 |

```
New peak funding requirement = (3,072,000), at the end of Month 5
Increase = 3,072,000 − 2,082,000 = 990,000
As a proportion = 990,000 ÷ 2,082,000 = 47.6 %
```

One month of payment timing raises the funding requirement by nearly half, and moves the trough a month
later. Scope, programme, cost and margin are all identical. This is the number that should be in front of
whoever negotiates payment terms.

### 10.5 Sensitivity 2 — no mobilisation advance

Remove the advance and its recovery. Net certificates rise to gross less retention only — 475,000, 855,000,
1,235,000, 1,425,000, 1,045,000 and 665,000 — still received two months in arrears:

| Month | Cash in | Cash out | Cumulative cash |
|---|---:|---:|---:|
| 0 | — | — | 0 |
| 1 | — | 450,000 | (450,000) |
| 2 | — | 810,000 | (1,260,000) |
| 3 | 475,000 | 1,170,000 | (1,955,000) |
| 4 | 855,000 | 1,350,000 | **(2,450,000)** |
| 5 | 1,235,000 | 990,000 | (2,205,000) |
| 6 | 1,425,000 | 630,000 | (1,410,000) |
| 7 | 1,045,000 | — | (365,000) |
| 8 | 665,000 | — | 300,000 |

```
Peak without the advance = (2,450,000)
Benefit of the advance   = 2,450,000 − 2,082,000 = 368,000
```

The advance was 480,000, but it improves the funding position by only 368,000, because by the trough at
Month 4 two certificates have already repaid part of it:

```
Recovery deducted from the Month 1 and Month 2 certificates = 40,000 + 72,000 = 112,000
480,000 − 112,000 = 368,000 ✓
```

An advance is worth less than its face value to the funding position, and how much less depends on the
recovery rate and on where the trough falls. A forecast that credits the full advance overstates the funding
benefit by 112,000 — which is `112,000 ÷ 480,000 = 23.3 %` of the advance.

**Assumptions this example depends on.** Full certification of applied value; a fixed two-month (or, in
§10.4, three-month) payment lag; all outflows paid in the month incurred; no variations, escalation or
indirect tax; retention released half at completion and half after the defects period, both outside the
eight-month window shown; cost at a constant 90 % of value in every month. Relaxing any one of these moves
the trough, which is exactly why sensitivities are run one variable at a time.

## 11. Checklist

**Structure**

- [ ] The cash forecast is a separate model from the cost forecast, driven by the same schedule.
- [ ] Gross value is phased from the time-phased baseline and the current progress forecast, not spread
      evenly.
- [ ] Retention, advance recovery and previous payments are modelled as separate lines, not netted.
- [ ] The payment date rule is stated explicitly, in periods, and matches the contract.
- [ ] Outflow terms are modelled by category — labour, subcontract, materials, plant — not as a single lag.

**Reconciliations, every month**

- [ ] Retention deducted to date equals the retention percentage times gross certified to date.
- [ ] Advance recovered to date is on track to repay the advance by completion.
- [ ] Total cash in over the life equals contract value, once retention is released.
- [ ] The closing cash position equals the margin, once all deductions have unwound.

**Reporting**

- [ ] The peak funding requirement is stated with its date.
- [ ] Sensitivities are run one variable at a time, and reported as depth and date of the trough.
- [ ] The payment-terms sensitivity is on the page every month, not on request.
- [ ] Retention outstanding is reported as a receivable with release dates and a named owner.
- [ ] Instructed-but-unpriced work is shown as cash exposure, not omitted because it has no certificate.

**Judgement**

- [ ] The certification assumption is stated, and compared against actual certification history.
- [ ] The forecast has been tested against the aggregate position of the portfolio, not just this project.
- [ ] Where the trough exceeds the available facility, the date it does so is escalated before it arrives.

---

## Related

- `BPG-07 — Accruals and cut-off discipline` — the difference between cost incurred, cost invoiced and cost
  paid, which this guide depends on.
- `BPG-11 — Change orders and variations` — why an unpriced variation book is a funding problem as well as a
  commercial one.
- `BPG-09 — Estimate at completion: choosing and defending a method` — the cost forecast this model takes as
  its input.
- `BPG-14 — Monthly reporting that gets read` — how the peak funding requirement reaches a decision-maker in
  time to matter.
- `TPL-09 — Cash flow forecast` — the instrument implementing §10's cascade.

## Sources and standards

The valuation, certification, retention and advance-recovery mechanisms described here follow the general
structure used across construction and engineering contracts, including the standard forms published by
bodies such as FIDIC and the NEC family; no clause, wording or numbering is reproduced, and the specific
terms of any contract prevail over this description. Indirect tax and withholding treatment is jurisdiction
specific and is a matter for the finance function. Internal references are BoK Domain 3 (Budgeting &
Forecasting), BoK Domain 7 (Contracts, Commercial Management, BoQ, Invoicing & Revenue) and BoK Domain 11
(Business Process Cycles). All figures in §10 are illustrative and were computed for this document.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
