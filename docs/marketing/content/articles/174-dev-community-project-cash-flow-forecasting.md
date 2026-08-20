---
platform:      DEV Community
type:          guide
title:         Project cash flow forecasting: S-curves and peak funding
meta:          Project cash flow forecasting that survives a CFO review: the four curves, peak funding worked in full, payment-term levers and the cash conversion cycle.
primary_kw:    project cash flow forecasting
secondary_kw:  S-curve, peak funding requirement, cash conversion cycle, retention
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/project-cash-flow-forecasting
schema:        Article + FAQPage
word_count:    1793
hashtags:      #python #datascience #finance #tutorial
ab_id:         AB-00096
---

# Project cash flow forecasting: S-curves and peak funding

Project cash flow forecasting converts a cost and revenue baseline into dated receipts and payments using payment terms, retention and the certification lag. The S-curve is the cumulative shape. The two numbers a CFO reads are the peak funding requirement and the month it occurs, and everything else on the page is working.

Those two figures decide whether the contract can be financed at all.

## Four curves from one baseline

A project generates four distinct curves from the same plan, and confusing any two produces a forecast wrong by months.

| Curve | What it measures | Driven by |
|---|---|---|
| Cost incurred | Work performed and resources consumed | The schedule and the estimate |
| Cost paid | Money leaving the business | Supplier and subcontract payment terms |
| Revenue recognised | Value earned under the accounting policy | Measured progress against the contract price |
| Cash received | Money arriving | Certification, payment terms, retention |

A fifth, the commitment curve, runs ahead of all of them and is the earliest warning available, because a purchase order raised today becomes cost in six weeks whatever anyone decides afterwards.

The gap between cost incurred and cash received is the funding requirement. The gap between revenue recognised and cash received is working capital. Different gaps, closing at different times.

## The transform is a lag and a haircut

Cash is not a separate model. It is the cost and revenue arrays put through two operators.

```python
def cash_curves(cost, revenue, retention=0.05, cert_lag=1, pay_split=(0.5, 0.5)):
    """cost/revenue: per-period arrays. Returns (cash_in, cash_out)."""
    n = len(cost)
    cash_in = [0.0] * (n + 1)     # +1 period for the post-completion tail
    cash_out = [0.0] * (n + 1)

    for t, rev in enumerate(revenue):
        cash_in[min(t + cert_lag, n)] += (1 - retention) * rev

    cash_in[n] += retention * sum(revenue)        # retention released at completion

    for t, c in enumerate(cost):
        for k, share in enumerate(pay_split):     # 45-day terms -> (0.5, 0.5)
            cash_out[min(t + k, n)] += share * c  # 60-day terms -> (0.0, 1.0)

    return cash_in, cash_out
```

Two properties are worth asserting in a test. The sum of `cash_in` must equal total revenue, and the sum of `cash_out` must equal total cost, because payment terms move timing and never totals. If either fails, the model is losing money somewhere in the indexing.

## Worked: where the money runs out

A £24.0m fixed-price contract over 12 months. Forecast cost £19.2m, so a £4.8m margin. Six two-month periods, £m.

Terms: applications certified and paid 60 days after the period end, so cash arrives one period late. Retention 5%, released at completion. Suppliers on 45 days, which pays roughly half a period's cost in that period and half in the next.

| Period | Cost | Revenue | Cash in | Cash out | Net | Cumulative |
|---|---:|---:|---:|---:|---:|---:|
| 1 | 1.60 | 2.00 | 0.00 | 0.80 | −0.80 | **−0.80** |
| 2 | 3.20 | 4.00 | 1.90 | 2.40 | −0.50 | **−1.30** |
| 3 | 4.40 | 5.50 | 3.80 | 3.80 | 0.00 | **−1.30** |
| 4 | 4.40 | 5.50 | 5.23 | 4.40 | +0.83 | −0.48 |
| 5 | 3.40 | 4.25 | 5.23 | 3.90 | +1.33 | +0.85 |
| 6 | 2.20 | 2.75 | 4.04 | 2.80 | +1.24 | +2.09 |
| Post | — | — | 3.81 | 1.10 | +2.71 | **+4.80** |

The closing cumulative is £4.80m, equal to the margin, so the model ties. Period two cash in is 0.95 × £2.00m of period-one revenue, and the post-completion row carries the £1.20m retention release.

**Peak funding is £1.30m, reached at the end of period two and held through period three.** Months three to six are when this contract needs a facility, and a profitable contract is where most contractors run out of money.

## One lever, £1.30m

Move supplier terms from 45 days to 60, so a period's cost is paid entirely in the following period. In the code above that is one argument: `pay_split=(0.0, 1.0)`.

| Period | Cash in | Cash out | Net | Cumulative |
|---|---:|---:|---:|---:|
| 1 | 0.00 | 0.00 | 0.00 | 0.00 |
| 2 | 1.90 | 1.60 | +0.30 | +0.30 |
| 3 | 3.80 | 3.20 | +0.60 | +0.90 |
| 4 | 5.23 | 4.40 | +0.83 | +1.73 |
| 5 | 5.23 | 4.40 | +0.83 | +2.55 |
| 6 | 4.04 | 3.40 | +0.64 | +3.19 |
| Post | 3.81 | 2.20 | +1.61 | **+4.80** |

Peak funding falls from £1.30m to nil and the contract self-finances from period two. The closing position is identical at £4.80m, because timing moved and profit did not.

Fifteen days of supplier terms was worth more to this contract than a 5% cost saving, and it was available at signature rather than earned over a year.

## Which levers actually move the peak

| Lever | Effect on peak funding | Honest assessment |
|---|---|---|
| Supplier and subcontract terms | Large | The strongest lever, negotiated before award or not at all |
| Certification and payment lag | Large | Halving a 60-day lag can remove the peak entirely |
| Mobilisation or advance payment | Large, immediate | Recovered later, so it shifts the curve rather than improving the total |
| Milestone rather than measured billing | Moderate, either direction | Front-loaded milestones help cash and flatter the reported position |
| Retention percentage | Small on the peak, large on the tail | Released long after the funding gap has closed |
| Materials on site claimable | Moderate | Only where the contract permits it and title has passed |

Retention is the one people overestimate. Cutting it from 5% to 3% improves the peak on this contract by about £0.04m, because retention is withheld from every certificate and returned after the funding requirement has passed.

## The cash conversion cycle, for a project business

At portfolio level the same question is asked with three ratios. Revenue for the year £96.0m, cost of sales £77.0m.

**Days sales outstanding.** Receivables £14.2m: 14.2 ÷ 96.0 × 365 = **54.0 days**.

**Days inventory outstanding**, which for a contractor means work in progress and contract assets. £11.5m: 11.5 ÷ 77.0 × 365 = **54.5 days**.

**Days payable outstanding.** Payables £18.9m: 18.9 ÷ 77.0 × 365 = **89.6 days**.

**Cash conversion cycle** = 54.0 + 54.5 − 89.6 = **18.9 days**. Working capital tied up = 18.9 ÷ 365 × 77.0 = **£3.99m**.

Ten days off DSO releases 10 ÷ 365 × 96.0 = **£2.63m** of cash, permanently, with no change to revenue or cost. That is larger than most cost-reduction programmes deliver, and it comes from billing on time and resolving certification disputes faster.

The warning attached to a short cycle: a contractor with a negative cash conversion cycle is financed by its supply chain, and the position reverses violently when volumes fall.

## Three ways an S-curve lies

**It is drawn from the contract, not the plan.** A curve fitted to the payment schedule reproduces the commercial negotiation rather than the work. If the shape does not come from [the time-phased cost baseline](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting), it is decoration.

**Cost is back-loaded and value is front-loaded.** Where the schedule of rates carries margin in early activities, the value curve rises faster than the cost curve and the contract looks profitable throughout, then collapses in the final quarter. Plot the ratio of the two each period and the divergence shows early.

**It ignores the tail.** The last of the physical work routinely takes longer than its share of the value suggests, and defects, demobilisation and final account settlement sit beyond the curve entirely. Model the tail as its own period or the closing position is optimistic by months.

## What the CFO's page contains

One page, four items. Peak funding requirement and the month it occurs. Cumulative cash at completion, tied to the margin as a check.

The two assumptions the forecast is most sensitive to, each with the value of a 15-day change stated in money. And the date of the next event that could move it, such as a certification decision or a variation determination.

What not to send is a 48-column spreadsheet. A finance director reviewing eleven contracts needs the funding number and its sensitivity, and will ask for the model when the summary raises a question.

## The overlap this sits in

Cash forecasting is where a delivery discipline and a finance discipline each hold half the answer. The planner knows when the work happens and the treasurer knows when the money moves.

A chartered accountant is examined on working capital and cut-off but not on time-phasing a baseline. An engineer is examined on the baseline but not on the certification lag that turns it into cash. The PCI AI Project Finance Leader (PFL-AI) examines both, across 16 domains and 61 knowledge areas, with a Body of Knowledge proportioned 40 / 40 / 20 across finance and reporting, project management, and governed AI.

## Frequently asked questions

**How often should a project cash flow forecast be updated?**
Monthly at minimum, as part of the close, and immediately after any event that changes payment timing: a certification dispute, an agreed variation, a change of subcontract terms. Rolling 13-week detail with monthly periods beyond that is the usual shape, because the near term needs day-level accuracy and the far term does not.

**Is the S-curve the same as the cash curve?**
No, and treating them as the same is the most common error in this work. The S-curve normally plots cumulative cost or value against time. The cash curve is that shape displaced by payment terms and reduced by retention, so it can sit two or three months to the right and several per cent lower for most of the contract.

**What exactly is the peak funding requirement?**
The largest cumulative net cash outflow across the life of the contract, and the period it occurs in. It is the working capital the business must have available to deliver the work, and it is the figure a bank asks for when a facility is discussed. A profitable contract can still carry a substantial peak.

**How do advance payments change the picture?**
They remove the early funding gap and are usually recovered pro rata against later certificates, so they shift cash rather than create it, and they often require a bond that has a cost. Model both the receipt and the recovery, because a forecast showing the advance without the recovery overstates the whole second half.

**Where does this connect to the accounts?**
Through the forecast cost at completion, which drives measured progress and therefore recognised revenue. The billing and retention assumptions in the cash model must match the contract asset calculation, or the two will disagree at year-end and someone will spend a week finding out why.

---

*First published on projectcontrolsinstitute.org; the `canonical_url` on this post points there. DEV prohibits stub posts, so the full build including the S-curve arithmetic is here.*

*Internal links: one is now in the body. "The time-phased cost baseline" points at projectcontrolsinstitute.org/project-budgeting-and-forecasting, placed where the piece says an S-curve drawn from the payment schedule is decoration, because that sentence raises where the baseline behind the curve is supposed to come from and this piece assumes it exists. The eac-accounting and month-end-close links proposed earlier were dropped: one link per domain per piece, and those two sentences are already answered in place. No second domain earns a link here — cash arithmetic raises no question that the AI, careers, regional or verification sites answer better. Reciprocal: the eac-accounting page should link back to this one from its cash-effect paragraph, with an anchor about peak funding rather than about cash flow generally.*
