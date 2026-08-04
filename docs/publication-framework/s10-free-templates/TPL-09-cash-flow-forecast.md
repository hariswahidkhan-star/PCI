---
id: TPL-09
series: S10
series_name: Free Templates
title: Cash flow forecast
subtitle: A period-by-period model of the distance between work done, value certified and money received
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 17
summary: >
  A working cash flow forecast that separates the three things most forecasts merge: the value of work
  executed, the value certified for payment, and the cash that actually arrives. It models retention
  withheld and released, advance-payment recovery, other deductions and contractual payment terms, and
  it derives the receipt period from the due date rather than assuming one. Complete it and you will
  know your peak funding requirement and the period it occurs in.
linkedin:
  format: document
  hook: >
    A project can be exactly on programme and still run out of money. Work is executed in one period,
    certified in another and paid in a third — and retention and advance recovery change the number
    twice on the way.
  tags: [ProjectControls, CashFlow, CostEngineering, CommercialManagement, ProjectFinance]
  asset: one-pager
gated: false
related: [BPG-13, BPG-07, BPG-06, TPL-12, TPL-06]
bok_domains: [3, 5, 7]
sources: []
placeholders: 0
---

# Cash flow forecast

> A period-by-period model of the distance between work done, value certified and money received.

**In one paragraph.** A working cash flow forecast that separates the three things most forecasts merge:
the value of work executed, the value certified for payment, and the cash that actually arrives. It models
retention withheld and released, advance-payment recovery, other deductions and contractual payment terms,
and it derives the receipt period from the due date rather than assuming one. Complete it and you will know
your peak funding requirement and the period it occurs in.

**Who this is for.** Cost engineers, project controls managers, commercial managers and quantity surveyors
who own the forecast; and the project directors and finance business partners who have to fund the gap it
reveals.

---

## 1. When to use this

Use it at three moments, and it does a different job at each.

**At tender or sanction**, to price the cost of funding the work. The peak funding requirement this sheet
produces is a real cost — it is financed, and someone pays the finance charge. A tender that has not
calculated it has priced the work and not the job.

**Monthly, alongside the cost report**, as the forecast the treasury function actually uses. The cost report
answers "what will this cost". This sheet answers "when do we need the money, and how much of it".

**Whenever the payment mechanics change** — a variation to the payment terms, a change in the retention
regime, an advance payment agreed or a certification dispute that stops the certified value tracking the
executed value. Each of those moves the cash curve without moving the cost forecast at all, which is
precisely why a project can be reporting a healthy margin while it cannot pay its subcontractors.

Do not use it as a substitute for the cost forecast. It takes the cost forecast as an input and converts it
into money and timing. If the underlying forecast is wrong, this sheet will produce a confidently wrong
cash curve. `BPG-13 — Cash flow forecasting` sets out the method; this is the instrument.

## 2. How to complete it

### 2.1 Set the period basis first

Decide the period — calendar month, four-week accounting period, or quarter — and use the same period in
the cost report, the payment application cycle and this sheet. Two different period bases in one project is
the single most common cause of a cash forecast that never reconciles.

Enter the **period-end date** for every period in the model, including a **period 0** row that carries the
opening position: any advance payment received, mobilisation spend, and opening balances. Every lookup in
this template keys off the period-end dates, so they must be complete, ascending, and one per row.

State the **currency and the base date**. If more than one currency is in play, run one sheet per currency
and consolidate outside this template — converting inside it hides the exposure rather than showing it.

### 2.2 Set the parameters

Put these on a separate sheet named `Parameters` and define each as a named range. Every formula below
refers to them by name.

| Name | What it is | Where it comes from |
|---|---|---|
| `Contract_Value` | The current contract sum, updated for agreed change | The contract and the change order log |
| `Retention_Rate` | Proportion of each gross certified valuation withheld, as a decimal | The contract |
| `Retention_Cap` | The maximum retention that may be held at any one time, as a currency amount | The contract, usually a percentage of `Contract_Value` |
| `Recovery_Rate` | Proportion of each gross certified valuation applied to recovering the advance payment | The contract |
| `Cert_Lag` | Whole periods between work being executed and its value being certified | The valuation cycle in the contract |
| `Payment_Terms_Days` | Days from the certification date to the contractual due date | The contract |

Two notes on these. `Retention_Cap` is a currency amount, not a percentage, because the cap and the rate are
usually expressed against different bases and mixing them is a common error. `Payment_Terms_Days` is in
calendar days here; if your contract counts working days, replace the addition in column Q with
`WORKDAY(B3, Payment_Terms_Days, Holidays)`.

Payment regimes, retention rules and any statutory protection over payment differ by contract and by
jurisdiction. This template models the mechanics; which mechanics apply to you is a matter for your contract
and the law governing it.

### 2.3 Work through the columns in order

The sheet runs left to right in the order the money moves: what was planned, what was built, what was
certified, what was deducted, what is due, when it lands, and what is left. Complete a period fully before
moving to the next. The field definitions are in §3.

Two columns deserve attention before you start. **Column G, gross value certified**, defaults to the value
of work executed but should be overridden with the certified figure the moment it is known — the gap between
those two numbers is the earliest warning you get of a commercial problem. **Column I** reports that gap
cumulatively; a figure below 100 % means you have built work you have not yet been paid for on paper, quite
apart from the payment terms.

## 3. The template

Header row in row 1. Row 2 is period 0 and is entered as values. Formulas begin in row 3 and fill down.

### 3.1 Input columns

| Col | Field | What goes in it |
|---|---|---|
| A | Period number | Integer, starting at 0 for the opening row. Unique, ascending, no gaps |
| B | Period ending | The date the period closes and the valuation is cut off. Ascending, one per row |
| C | Planned value in period | Value of work the baseline says will be executed in the period, at contract rates |
| E | Work executed in period | Value of work actually executed in the period at contract rates; forecast for future periods |
| G | Gross value certified in period | The value the certifier has agreed in the period, before deductions. Defaults to the formula in §3.2; override with the certified figure once known |
| K | Retention released in period | Retention returned in the period, per the contractual release events |
| O | Other deductions in period | Contra-charges, back-charges, liquidated damages, agreed set-offs. One line each in a supporting note |
| S | Other receipts in period | Advance payment, milestone lump sums outside the valuation cycle, insurance recoveries |
| U | Cash out in period | Payments made in the period: supply chain, labour, plant, staff, overheads, finance charges |

### 3.2 Calculated columns

Formulas are written for row 3 and fill down. Where a formula uses a whole-column reference such as
`$A:$A`, that is deliberate: `MATCH` and `COUNTIF` return positions relative to the start of the range, and
using whole columns keeps the position and the sheet row identical.

| Col | Field | Formula in words | Spreadsheet expression |
|---|---|---|---|
| D | Cumulative planned value | Running total of planned value from period 0 to this period | `=SUM($C$2:C3)` |
| F | Cumulative work executed | Running total of work executed | `=SUM($E$2:E3)` |
| G | Gross value certified (default) | The work executed in the period `Cert_Lag` periods earlier; zero if that period does not exist | `=IFERROR(INDEX($E:$E,MATCH($A3-Cert_Lag,$A:$A,0)),0)` |
| H | Cumulative gross certified | Running total of gross certified value | `=SUM($G$2:G3)` |
| I | Certified as a proportion of executed | Cumulative certified divided by cumulative executed; blank while nothing has been executed | `=IF(F3=0,"",H3/F3)` |
| J | Retention withheld in period | The retention rate applied to the gross certified value, but never more than the headroom left under the cap | `=MIN(Retention_Rate*G3,MAX(0,Retention_Cap-L2))` |
| L | Retention balance held | Last period's balance, plus what was withheld, less what was released | `=L2+J3-K3` |
| M | Advance recovery in period | The recovery rate applied to the gross certified value, but never more than the advance still outstanding | `=MIN(Recovery_Rate*G3,N2)` |
| N | Advance balance outstanding | Last period's outstanding advance less this period's recovery | `=N2-M3` |
| P | Net certified payable in period | Gross certified, less retention withheld, plus retention released, less advance recovery, less other deductions | `=G3-J3+K3-M3-O3` |
| Q | Payment due date | The period-end date plus the contractual payment period | `=B3+Payment_Terms_Days` |
| R | Receipt period | The number of the first period whose end date falls on or after the due date | `=IFERROR(INDEX($A$2:$A$200,COUNTIF($B$2:$B$200,"<"&$Q3)+1),"")` |
| T | Cash in — receipts in period | Every net payable amount whose receipt period is this period, plus other receipts | `=SUMIF($R$2:$R$200,$A3,$P$2:$P$200)+S3` |
| V | Net cash in period | Cash in less cash out | `=T3-U3` |
| W | Cumulative net cash | Last period's cumulative position plus this period's net movement | `=W2+V3` |

Two summary cells sit outside the table:

| Cell | Field | Formula in words | Spreadsheet expression |
|---|---|---|---|
| — | Peak funding requirement | The largest negative cumulative cash position over the model, expressed as a positive number; zero if the position never goes negative | `=MAX(0,-MIN($W$2:$W$200))` |
| — | Period of peak funding | The period number at which that position occurs | `=IFERROR(INDEX($A$2:$A$200,MATCH(MIN($W$2:$W$200),$W$2:$W$200,0)),"")` |

Column R is the mechanism that makes this template worth using. It does not assume a lag in whole periods —
it computes the contractual due date and then asks which period that date lands in. Thirty-day terms on a
month-end certificate do not mean the money arrives next month, and the worked fragment in §4 shows why.

### 3.3 Pasting it into a spreadsheet

Copy the header line below into cell A1, then use the text-to-columns tool with the pipe character as the
delimiter. Freeze row 1, format columns B and Q as dates, and apply the formulas from §3.2 to row 3 before
filling down.

```
Period|Period ending|Planned value in period|Cumulative planned value|Work executed in period|Cumulative work executed|Gross value certified in period|Cumulative gross certified|Certified / executed (cum)|Retention withheld|Retention released|Retention balance held|Advance recovery|Advance balance outstanding|Other deductions|Net certified payable|Payment due date|Receipt period|Other receipts|Cash in|Cash out|Net cash in period|Cumulative net cash
```

For use in Markdown, split the same columns into two blocks that share the period key — a valuation block
(A to P) and a cash block (A, B and Q to W). That is how the worked fragment below is laid out, and it reads
far better on a page than twenty-three columns in one table.

## 4. Worked fragment

*Illustrative figures.* Currency-neutral units, rounded to whole units. Monthly periods with calendar
month-end cut-offs, period 0 ending 31 January 2027. Contract value 10,000,000. Retention 5 % of gross
certified value, capped at 250,000. Advance payment of 500,000 received in period 0, recovered at 20 % of
gross certified value. Certification lag zero periods. Payment terms 30 calendar days from the period-end
certification date. No other deductions and no retention released in the periods shown.

**Valuation block**

| Period | Ending | Planned value | Cum planned | Executed | Cum executed | Gross certified | Cum certified | Cert / exec | Retention withheld | Retention held | Advance recovery | Advance outstanding | Net payable |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 0 | 31 Jan 27 | 0 | 0 | 0 | 0 | 0 | 0 | — | 0 | 0 | 0 | 500,000 | 0 |
| 1 | 28 Feb 27 | 450,000 | 450,000 | 400,000 | 400,000 | 400,000 | 400,000 | 100.0 % | 20,000 | 20,000 | 80,000 | 420,000 | 300,000 |
| 2 | 31 Mar 27 | 750,000 | 1,200,000 | 700,000 | 1,100,000 | 640,000 | 1,040,000 | 94.5 % | 32,000 | 52,000 | 128,000 | 292,000 | 480,000 |
| 3 | 30 Apr 27 | 850,000 | 2,050,000 | 900,000 | 2,000,000 | 960,000 | 2,000,000 | 100.0 % | 48,000 | 100,000 | 192,000 | 100,000 | 720,000 |
| 4 | 31 May 27 | 1,050,000 | 3,100,000 | 1,100,000 | 3,100,000 | 1,100,000 | 3,100,000 | 100.0 % | 55,000 | 155,000 | 100,000 | 0 | 945,000 |

**Cash block**

| Period | Ending | Due date | Receipt period | Other receipts | Cash in | Cash out | Net cash | Cumulative net cash |
|---|---|---|---|---|---|---|---|---|
| 0 | 31 Jan 27 | 2 Mar 27 | 2 | 500,000 | 500,000 | 120,000 | 380,000 | 380,000 |
| 1 | 28 Feb 27 | 30 Mar 27 | 2 | 0 | 0 | 340,000 | −340,000 | 40,000 |
| 2 | 31 Mar 27 | 30 Apr 27 | 3 | 0 | 300,000 | 600,000 | −300,000 | −260,000 |
| 3 | 30 Apr 27 | 30 May 27 | 4 | 0 | 480,000 | 780,000 | −300,000 | −560,000 |
| 4 | 31 May 27 | 30 Jun 27 | 5 | 0 | 720,000 | 940,000 | −220,000 | −780,000 |

**The substitutions.**

Period 2 retention withheld: `MIN(0.05 × 640,000, MAX(0, 250,000 − 20,000)) = MIN(32,000, 230,000) = 32,000`.
The cap has not yet bitten, so the rate governs.

Period 4 advance recovery: `MIN(0.20 × 1,100,000, 100,000) = MIN(220,000, 100,000) = 100,000`. Here the cap
does bite — only 100,000 of the advance remains outstanding, so recovery stops there. Total recovered across
the four periods is `80,000 + 128,000 + 192,000 + 100,000 = 500,000`, exactly the advance.

Period 4 net payable: `1,100,000 − 55,000 + 0 − 100,000 − 0 = 945,000`.

Period 2 receipt period: the certificate is dated 28 February 2027 and the due date is
`28 Feb + 30 days = 30 March 2027`. The first period-end on or after 30 March is 31 March, which is period
2 — so the money from period 1's work arrives in period 2. Period 2's own certificate, dated 31 March, is
due `31 Mar + 30 days = 30 April`, and 30 April is period 3's end date, so it lands in period 3.

Cumulative net cash at period 4: `380,000 − 340,000 − 300,000 − 300,000 − 220,000 = −780,000`. The peak
funding requirement over the fragment is therefore `MAX(0, −(−780,000)) = 780,000`, occurring in period 4.

**Read the two blocks together.** By period 4 the cumulative value of work executed, 3,100,000, is exactly
equal to the cumulative planned value. On every progress measure this project is on plan. It is also
780,000 out of pocket, and 945,000 of certified value is still unpaid — it does not arrive until period 5.
Nothing has gone wrong. This is what 30-day payment terms, 5 % retention and a 20 % advance recovery do to a
project that is performing exactly as intended, and it is the number that should have been financed at
tender.

A cross-check worth running every month: gross certified less retention held less advance recovered should
equal cumulative net payable. Here, `3,100,000 − 155,000 − 500,000 = 2,445,000`, and the net payable column
sums to `300,000 + 480,000 + 720,000 + 945,000 = 2,445,000`.

## 5. Common mistakes

**Forecasting cash from the cost curve.** The most common error is to take the cost forecast, shift it by a
month and call it cash. That treats certification as automatic, retention as invisible and the advance as
free money. Each of the three is a separate mechanism with its own timing, and all three are in this sheet
because all three move the answer.

**Assuming the payment lag in whole periods.** "Thirty-day terms means next month" is wrong more often than
it is right, as §4 shows. Compute the due date, then find the period it falls in.

**Modelling the advance as income.** An advance payment is a loan against future work, recovered from your
own valuations. It flatters period 0 and it takes the money back exactly when the spend rate is highest.
Column N exists so that the outstanding balance is always visible.

**Letting the retention cap and the retention rate drift apart.** The rate applies to each valuation; the cap
applies to the balance held. If you only model the rate you will over-withhold at the top of the job; if you
only model the cap you will under-withhold at the bottom. The `MIN`/`MAX` construction in column J handles
both, and it is worth testing by entering a very large certified value in a scratch row and checking that the
retention balance stops at the cap.

**Certified value silently tracking executed value.** Leaving column G on its default formula for periods
that have already been certified turns a control into a mirror. Column I goes to 100 % and stays there, and
the disallowed valuation nobody has resolved never appears. Override G with the certified figure every
period, and treat any month where the gap widens as a commercial escalation, not a data-entry issue.

**Ignoring the payment behaviour you actually observe.** The contract says one thing; the record of when
money has historically arrived may say another. Model the contractual position in this sheet, and if actual
receipts run consistently later, add a second scenario using the observed pattern and report the difference.
Quietly building the slippage into the contractual model destroys your ability to make the case for it.

**Forgetting the tail.** In the fragment, 945,000 arrives in period 5 — after the last period shown. A model
that stops at practical completion misses the final account, the retention release and the defects period,
which between them can be the difference between a profitable job and a funded one.

## 6. Adapting it

**Safe to change.** The period basis, the currency, the number of periods, the column headings, and the
addition of columns for anything your contract deducts or adds. If you invoice against milestones rather
than measured progress, replace column E with milestone values and set `Cert_Lag` to reflect the
certification cycle. If you hold retention from your own supply chain, add a mirror block for retention you
withhold — it is cash you hold, and it belongs in column U's netting.

**Safe to add.** A financing block, converting the peak funding requirement into an interest cost at your
facility's rate. A currency-exposure block if you have receipts and payments in different currencies. A
sensitivity block that re-runs the model with certification at 90 % of executed value and payment one period
later, which is a more honest downside than a single-point forecast.

**Do not change.** The separation of the three quantities — executed, certified, received. Merging any two
of them removes the only thing this sheet does that a cost report does not. And do not remove the period 0
row: every lookup in the template depends on the period sequence being complete from the opening position.

### 6.1 Before you issue it

- Period-end dates are complete, ascending and one per row, with no gaps in the period numbers.
- Every parameter on the `Parameters` sheet has been read out of the contract, not assumed.
- Column G has been overridden with actual certified values for every period already certified.
- Column I has been read, and any period below 100 % has a named owner and a resolution date.
- The retention balance in column L stops at `Retention_Cap` when tested with a large valuation.
- The advance balance in column N reaches zero and does not go negative.
- The cross-check reconciles: cumulative gross certified, less retention held, less advance recovered,
  equals cumulative net payable.
- The peak funding requirement and the period it occurs in have been stated to whoever funds it.
- The tail beyond the last period shown — final account, retention release, defects period — is either in
  the model or explicitly noted as excluded.
- The currency, base date, period basis and rounding are written on the face of the sheet.

---

## Related

- `BPG-13 — Cash flow forecasting` — the method behind this instrument, including curve shapes and how to build the first forecast when there is no history
- `BPG-07 — Accruals and cut-off discipline` — why the cut-off date governs both the cost report and this sheet, and what goes wrong when they differ
- `BPG-06 — Progress measurement and rules of credit` — where the value of work executed in column E legitimately comes from
- `TPL-12 — Change order log` — the source of the changes that must reach column C before this forecast is complete
- `TPL-06 — Monthly project controls report` — where the peak funding requirement and the cash position are reported

## Sources and standards

This is an original instrument developed by the Institute. It reproduces no third-party template, form or
worked example. The retention, advance-recovery and payment-term mechanics it models are general commercial
practice described here in the Institute's own words; the mechanics that bind any particular project are
those in its contract, under its governing law. No accounting, tax or statutory payment treatment is
presented as universal, and none should be inferred.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
