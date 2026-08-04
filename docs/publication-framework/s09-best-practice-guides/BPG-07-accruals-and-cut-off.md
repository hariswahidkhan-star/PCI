---
id: BPG-07
series: S09
series_name: Best Practice Guides
title: Accruals and cut-off discipline
subtitle: Why a missed accrual flatters this month and invents an overrun next month
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 16
summary: >
  How to close a reporting period so the cost figure describes the work done rather than the invoices
  processed: the three cost states, what must be accrued and from which source, how to size an accrual
  when the subcontractor has not certified, reversal discipline, the cut-off calendar and what happens
  when it is compressed. Includes a worked month-end showing the cost performance index with and
  without the accrual, the phantom overrun it creates in the following period, and the effect on the
  forecast.
linkedin:
  format: post
  hook: >
    Miss the accrual and the cost performance index reads 1.185. Book it and the same month reads
    0.945. Nothing physical changed — and next month the report invents an overrun that never happened.
  tags: [ProjectControls, CostControl, Accruals, EarnedValue, ProjectAccounting]
  asset: one-pager
gated: false
related: [BPG-01, BPG-03, BPG-06, BPG-08, BPG-13, TPL-07]
bok_domains: [1, 2, 5]
sources: []
placeholders: 0
---

# Accruals and cut-off discipline

> Making the period's cost figure describe the work that was done, not the invoices that happened to
> arrive.

**In one paragraph.** How to close a reporting period so the cost figure describes the work done rather
than the invoices processed: the three cost states, what must be accrued and from which source, how to
size an accrual when the subcontractor has not certified, reversal discipline, the cut-off calendar and
what happens when it is compressed. Includes a worked month-end showing the cost performance index with
and without the accrual, the phantom overrun it creates in the following period, and the effect on the
forecast.

**Who this is for.** Cost engineers and cost managers who close a period; project accountants and
finance business partners who own the ledger side of it; project managers who have been told the
overrun is "a timing thing" and want to know whether that is true.

---

## 1. The month does not end when the calendar does

A project's cost ledger is a record of invoices processed. A project's cost report is meant to be a
record of resources consumed. These are different things, and the gap between them is a matter of
weeks: a subcontractor works through March, certifies in early April, invoices mid-April and is paid in
May. Read the ledger at the end of March and none of that work exists.

The accrual is the entry that closes the gap. It says: this work has been done, we owe for it, and the
cost belongs to the period in which the work happened rather than the period in which the paperwork
caught up.

Everything else in this guide follows from one observation. Because earned value measures work done at
budgeted cost, comparing it to a cost figure that excludes work done but not invoiced compares two
different things — and produces an index that is not slightly wrong but systematically flattering, in
every period, by an amount that varies with invoice timing. §9 shows a twenty-four-point swing on a
single control account from one month's accruals.

## 2. Three states, and where the accrual sits

Cost passes through three states, and control depends on tracking all three rather than only the last.

**Commitment.** A purchase order or subcontract is raised. Nothing has been received and nothing paid,
but the organisation is exposed. Commitments are the earliest signal of future spend and the only one
that arrives before the money does.

**Accrual.** Goods or services have been received, so the cost has been incurred, but no invoice has
been processed. The accrual recognises the incurred cost in the period the work happened.

**Actual.** The invoice has been received and posted. The cost is in the ledger.

The cost figure a controls report needs is **actuals plus accruals** — everything incurred, whether or
not it has been billed. Add open commitments and an estimate for uncommitted remaining scope and you
have the forecast. Reporting invoices alone understates cost by the accrual and ignores the commitment
entirely, which is a blind spot in both directions at once.

The transaction-state segment of the code of accounts is what makes these three separable in the first
place; `BPG-03 — Cost breakdown structure and the code of accounts` covers its design.

## 3. What has to be accrued

Five categories account for nearly all of it.

**Goods received not invoiced.** Materials and equipment delivered and receipted before the cut-off,
with the invoice still to arrive. This is the most reliable accrual because it comes straight from the
goods receipt records, and it is the one most systems can produce without judgement.

**Work performed by subcontractors but not yet certified or billed.** The largest and hardest category.
Certification cycles rarely align with the reporting cut-off, so at any month end there is a slice of
subcontract work that is unambiguously done and entirely absent from the ledger.

**Labour worked after the payroll extract.** The last few days of the month, where the timesheet
system's cut-off does not match the reporting cut-off. Small per day, material per month, and
completely predictable — which means it can be computed rather than estimated.

**Services consumed and not billed.** Plant hire, temporary utilities, professional services, freight
in transit under terms that have already transferred the cost.

**Adjustments already known.** A credit note agreed but not issued, a rejected delivery still sitting in
the receipt records, a rate correction agreed but not yet applied. These accrue in the direction that
makes the period true, which is sometimes negative.

What must *not* be accrued is work that has not been performed. An accrual for a cost that is expected
but not yet incurred is a provision or a forecast, not an accrual, and mixing the two destroys the
meaning of both. The accounting frameworks in use internationally draw this line carefully — the
distinction between a liability whose timing or amount is uncertain and an accrual for goods or
services already received is fundamental to standards such as IAS 37 — and the exact recognition point,
along with the treatment of the resulting balances for tax, varies by framework and by jurisdiction.
The controls professional's job is not to determine the accounting treatment but to supply an accurate
statement of what was received before the cut-off, and to agree the boundary with finance rather than
assume it.

## 4. Sizing the accrual

Sources, in descending order of reliability. Use the highest one available for each item and record
which you used, because the reliability of the accrual is part of the information.

1. **A certified quantity or milestone.** The work has been measured and signed, only the invoice is
   missing. Effectively exact.
2. **A goods receipt.** Quantity received against a priced purchase order. Exact for materials.
3. **A rate multiplied by a measured quantity.** Contract rate against a surveyed quantity — the
   standard method where certification is behind.
4. **A rate multiplied by a recorded input.** Hours from timesheets or plant records at agreed rates.
   Reliable for labour, less so where the input does not translate directly into value.
5. **Progress applied to a package value.** The package's earned percentage multiplied by its value,
   where nothing more direct exists. Weakest, because it inherits any error in the progress measurement
   — see `BPG-06 — Progress measurement and rules of credit`.
6. **A judgement by the responsible manager.** Acceptable only with a written basis, and it should be
   the exception. An accrual nobody can reconstruct is a number, not a measurement.

Two rules. **Accrue to the receipt, not to the document.** The date that matters is when the goods or
services were received, not the date on the invoice or the date it was entered. An automated accrual
driven by document date reproduces a cut-off error at scale. And **do not net accruals against
retentions or contra-charges** unless the contract makes them a single obligation — netting hides both
numbers and makes the cash forecast in `BPG-13 — Cash flow forecasting` unreliable.

## 5. Reversal discipline

Every accrual is reversed in the following period, and the actual invoice is posted in its place. This
sounds mechanical and is where most accrual processes fail.

The failure mode is the accrual that is not reversed, so that when the invoice arrives the cost is
recorded twice. It usually happens where accruals are entered manually into a controls spreadsheet
rather than as reversing journals in a system, and it is detected by a discipline rather than by a
control: maintain an **accrual register** with one row per accrual carrying the period raised, the
basis, the source used, the value, the period reversed, and the actual value when it landed.

That last column is the one that earns the register its place, because it gives an **accrual accuracy**
measure: compare each accrual to the invoice that replaced it. Persistent under-accrual on one
subcontract is a signal about that subcontractor's certification lag; persistent under-accrual across
the project is a signal about the process. Neither is visible without the comparison, and both change
what you do next month.

## 6. The cut-off calendar, and the pressure on it

A cut-off is a date, published in advance, to which every source reports: cost extraction, goods
receipts, timesheets, subcontract certification, schedule progress and field quantities. It works only
if it is the same date for all of them.

The working-day sequence from cut-off to issue is designed backwards from the meeting that receives the
report, and `BPG-01 — Building a project controls function from zero` works that calendar through. The
relevant point here is what happens under compression. When the reporting deadline moves earlier, the
steps with external deadlines — data extraction, the report issue itself — cannot move. The step with
no external deadline is accrual determination, so that is the one that gets shortened, and the
shortening takes the form of using a weaker source from §4 or skipping the smaller categories entirely.

The result is not a slightly less precise report. It is a report whose cost figure has moved
systematically in one direction, because every omitted accrual understates cost and none overstates it.

Two defences are worth building in. **Pre-cut-off preparation**: subcontract quantities surveyed in the
last week of the period rather than the first week of the next, and goods-receipt exceptions cleared
before the cut-off rather than after. And a **standing de minimis**, agreed in advance, below which
items are not individually accrued — which converts an implicit omission into an explicit, bounded and
disclosed one.

## 7. Two accruals, one reconciliation

The accrual the project needs and the accrual the statutory accounts need are not always the same
number, and pretending otherwise causes more argument than accepting it.

The project's accrual serves management control. It wants completeness at a fine granularity, at
control-account level, on a fixed monthly rhythm, and it will accept a defensible estimate where an
exact figure is unavailable. The statutory accrual serves financial reporting. It applies the
recognition criteria of the governing framework, is subject to audit, and applies materiality
thresholds set for the entity as a whole rather than for one project.

The two should be **reconciled, not forced to converge**. Where the project accrues something finance
does not, the difference should be explainable in one line — usually a materiality threshold or a
recognition timing difference. A reconciliation that cannot be explained is the finding; an
unreconciled difference written off as "timing" every month for a year is how a real problem hides.

## 8. How this goes wrong

**The accrual is skipped because the period was tight.** One month's omission produces a flattering
index and a decision taken on it. The following month, when the invoices land, the same account shows
an overrun that nothing in that month caused. §9.3 works this through, and the phantom is severe: a
period index below 0.44 in a month where performance was actually near 0.93.

**The accrual is driven by invoice date.** An automated rule accrues everything with a document date in
the period. Work received in the period but invoiced with a later document date is missed; work
received later but invoiced early is included. The rule looks systematic and is systematically wrong.

**Accruals are estimated by the person whose performance they affect.** A control account manager who
sizes their own accrual has an incentive to be conservative. Not fraudulently — but conservative in one
direction, every month, and the cumulative effect is a project that always looks slightly better than
it is until the final account.

**Accruals are not reversed.** The cost is counted twice when the invoice lands. Detected, if at all,
by a reconciliation that nobody has time to do. Prevented by reversing journals and an accrual register.

**Retention is treated as an accrual.** Retention withheld from a certified valuation is a payment
timing matter, not a cost timing matter — the cost was incurred and should be recognised in full.
Treating retention as though it reduces cost understates the project and misstates the cash forecast.

**Accrual accuracy is never measured.** The register exists but the "actual when it landed" column is
never filled in, so nobody knows whether the accruals are systematically low. This is the cheapest
improvement available to most cost functions and the one most often left undone.

**The cut-off is held open for a large invoice.** Somebody notices that a significant invoice will
arrive on the second and delays the close. The period is now longer than a month, the comparison to
every other period is broken, and the precedent is established that cut-off is negotiable when the
number is unhelpful.

## 9. Worked example

*Illustrative figures.* Generic currency units. One control account, monthly reporting cycle, values
cumulative to the data date unless stated. Indices to three decimal places; currency to the nearest
1,000 where a rounded figure is presented. No real project, jurisdiction or accounting framework is
implied — the treatment of any specific item follows the framework the entity reports under.

### 9.1 Month 9 at cut-off

| Item | Value |
|---|---:|
| Budget at completion | 5,600,000 |
| Earned value at the data date | 3,720,000 |
| Invoices processed to date (ledger actuals) | 3,140,000 |
| Goods received not invoiced (from the goods-receipt report) | 268,000 |
| Subcontract work surveyed in the period, not yet certified or billed | 412,000 |
| Labour worked after the payroll extract: 1,850 hours at a blended 62 per hour | 114,700 |

```
Timesheet accrual = 1,850 × 62 = 114,700
Total accrual     = 268,000 + 412,000 + 114,700 = 794,700
True actual cost  = 3,140,000 + 794,700 = 3,934,700
```

### 9.2 The same month, reported two ways

```
Without the accrual:  CPI = EV ÷ AC = 3,720,000 ÷ 3,140,000 = 1.185
                      CV  = EV − AC = 3,720,000 − 3,140,000 = +580,000

With the accrual:     CPI = EV ÷ AC = 3,720,000 ÷ 3,934,700 = 0.945
                      CV  = EV − AC = 3,720,000 − 3,934,700 = −214,700
```

The index swings by 1.185 − 0.945 = **0.240**, twenty-four index points. The variance swings by
580,000 − (−214,700) = **794,700**, which is exactly the accrual — the arithmetic check that the two
statements describe the same project.

**What this does to the forecast.** Forecasting on the assumption that performance to date persists —
one method among several, and `BPG-09 — Estimate at completion` owns the choice — the two views diverge
by more than the accrual itself:

```
Without the accrual: EAC = BAC ÷ CPI = 5,600,000 ÷ 1.184713 = 4,727,000
                     VAC = 5,600,000 − 4,727,000 = +873,000   (an underrun)

With the accrual:    EAC = BAC ÷ CPI = 5,600,000 ÷ 0.945434 = 5,923,000
                     VAC = 5,600,000 − 5,923,000 = −323,000   (an overrun)
```

The forecast moves by 5,923,000 − 4,727,000 = **1,196,000**, which is 1,196,000 ÷ 5,600,000 = **21.4 %**
of budget at completion. One month's accruals, worth 794,700, change the project's forecast outcome
from a 873,000 underrun to a 323,000 overrun. The unrounded indices are used in the divisions and the
results rounded once, at the end.

### 9.3 The phantom overrun in month 10

In month 10 the 794,700 of month-9 accruals arrive as invoices, a further 690,000 of genuine month-10
work is incurred, and earned value rises by 640,000.

```
Cumulative actual cost = 3,140,000 + 794,700 + 690,000 = 4,624,700
Cumulative earned value = 3,720,000 + 640,000 = 4,360,000
```

**On the un-accrued path**, month 9 reported cost of 3,140,000, so the period cost recorded in month 10
is everything since:

```
Period AC  = 4,624,700 − 3,140,000 = 1,484,700
Period EV  = 640,000
Period CPI = 640,000 ÷ 1,484,700 = 0.431
```

**On the accrued path**, the month-9 accrual reverses, the invoices replace it, and the month-10 accrual
is booked. The period cost is the 690,000 genuinely incurred in month 10:

```
Period AC  = 690,000
Period EV  = 640,000
Period CPI = 640,000 ÷ 690,000 = 0.928
```

Month 10 was a period of mildly disappointing performance — a period index of 0.928. The un-accrued
report shows 0.431, which would in most organisations trigger an intervention, a recovery plan and a
difficult conversation about a month in which nothing unusual happened.

**And the cumulative index converges anyway.** At month 10 the cumulative figure is the same under both
treatments: 4,360,000 ÷ 4,624,700 = **0.943**. That is the trap. The cumulative index eventually tells
the truth, which is why missing accruals so often go undetected — but every *decision* is taken on a
period number, and the period number is destroyed. In month 9 the project was told it was 580,000 under
budget. In month 10 it is told it has collapsed. Neither statement was true.

## 10. Checklist

Take this into the month-end close, or into the review of a period that produced a surprise.

**Completeness**

- [ ] Goods received not invoiced: pulled from the receipt records, or estimated? (It should never be estimated.)
- [ ] Subcontract work performed and not certified: has the quantity been surveyed, or assumed?
- [ ] Labour after the payroll extract: how many days, how many hours, at what rate?
- [ ] Plant hire, utilities, freight and professional services: is anything consumed and unbilled?
- [ ] Known credits, rejections and rate corrections: accrued in the correct direction?
- [ ] Is anything being accrued that has not actually been received? (That is a provision, and it belongs elsewhere.)

**Basis and evidence**

- [ ] For each accrual, which of the six sources in §4 was used, and is it recorded?
- [ ] Is any accrual sized by the person whose reported performance it affects?
- [ ] Is any accrual driven by document date rather than receipt date?
- [ ] Is retention being treated as a reduction in cost? (It is not one.)
- [ ] Is anything netted that should be reported gross?

**Reversal**

- [ ] Is there an accrual register with the period raised, the basis, the value and the reversal period?
- [ ] Have all prior-period accruals actually reversed?
- [ ] Is the "actual when it landed" column filled in, and what is the accrual accuracy trend?
- [ ] Which supplier or subcontract is consistently under-accrued, and why?

**Cut-off**

- [ ] Is the cut-off the same date for cost, goods receipts, timesheets, certification and schedule progress?
- [ ] Was the cut-off held open for anything this period? For what, and who authorised it?
- [ ] Was the accrual step compressed because the reporting deadline moved?
- [ ] Is there an agreed de minimis, written down, and is the total below it disclosed?

**Reconciliation**

- [ ] Does the project accrual reconcile to the finance accrual, with the difference explainable in one line?
- [ ] Has that difference been "timing" for more than two consecutive periods?
- [ ] Has the boundary between a controls accrual and a statutory provision been agreed with finance in writing?

---

## Related

- `BPG-01 — Building a project controls function from zero` — the cut-off calendar this depends on, and why it precedes reporting.
- `BPG-03 — Cost breakdown structure and the code of accounts` — the transaction-state coding that makes commitment, accrual and actual separable.
- `BPG-06 — Progress measurement and rules of credit` — the earned value figure that the cost figure here is compared against.
- `BPG-08 — Earned value in practice` — how the indices in §9 are read and reported.
- `BPG-13 — Cash flow forecasting` — why accrual and payment timing must be kept distinct.
- `TPL-07 — Earned value calculation sheet` — the calculation, with the accrual input made explicit.

## Sources and standards

Drawn from the Institute's Body of Knowledge: Domain 5 (Cost Management and Cost Control) for the
commitment–accrual–actual cycle and cost-to-date, Domain 1 (Foundations of Accounting for Project
Controls) for accrual-basis recognition, and Domain 2 (Financial Reporting and the Standards) for the
relationship between project cost records and statutory reporting.

Accounting standards are named where relevant and their principles described in the Institute's own
words; no standard text, table or clause numbering is reproduced. The distinction between an accrual
for goods or services already received and a provision for a liability of uncertain timing or amount is
treated by frameworks such as IAS 37, and the recognition point, the materiality thresholds and any tax
consequences vary by reporting framework and by jurisdiction. Nothing in this guide should be read as
determining the accounting treatment of any item; that is a matter for the entity's finance function
under the framework it reports under.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
