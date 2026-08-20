---
platform:      Reddit / forum — r/projectmanagement
type:          forum-post
title:         How a capital project's month-end close really works
meta:          A £1.1m move in the forecast reversed £660k of margin already taken. The close timetable, the accrual list and the arithmetic behind the swing.
primary_kw:    month-end close for projects *
secondary_kw:  cut-off, accruals, estimate at completion, IFRS 15
pillar:        Project controls fundamentals
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1311
hashtags:      n/a (Reddit)
ab_id:         AB-00141
---

# How month-end close actually works on a capital project

The cost engineer added £1.1m to the forecast on the Tuesday. By the Friday, finance had reversed £660k of margin already taken, on a job that still shows £5.4m of margin. Nobody had done anything wrong, and nobody in the room could explain it.

Short answer: month-end close is the point where a delivery forecast becomes a recognised number. Cut-off decides which costs count, the rules of credit decide how much work counts, and the estimate at completion decides what share of the whole margin you are allowed to have kept. Change the last one and every prior month is restated at once.

## The order things happen in

Close has a fixed shape on almost every capital project I have worked on, and the order is not negotiable.

Progress is surveyed and agreed on site. Rules of credit convert that progress into earned value by control account. The cost ledger is cut. Accruals are added for what happened but has not been invoiced. Only then is the estimate at completion revised, and only then does finance calculate revenue.

People try to run these in parallel to save two days. It does not save two days. It produces a forecast built on a cost position that changes underneath it.

## Cut-off is the whole game

The ledger is not a record of what the project did in the month. It is a record of what the finance system processed in the month. Those are different, and the gap is the accrual list.

| Adjustment | Direction | Why |
|---|---|---|
| Goods received, not invoiced — £0.82m | add | The cost was incurred. The invoice is simply late. |
| Subcontract work done, not applied for — £0.34m | add | Performance happened inside the period, whatever the payment cycle says. |
| Materials delivered but not installed — £0.61m | remove | Delivery is not progress. Left in, it inflates the measure of completion. |
| Invoices dated after cut-off — £0.15m | remove | Wrong period. Someone posted them to clear a desk. |

That last removal is the one that causes arguments, because taking it out makes this month look worse and next month look better, and someone always has a view about which month needs the help.

## The worked month

Fixed-price contract, £64.0m. Cost-to-cost is the measure of progress: costs incurred to date divided by the total cost expected.

**Last close.** Estimate at completion was £57.5m, so expected margin was £6.5m. Costs that measure progress stood at £34.5m.

- Percentage complete = 34.5 ÷ 57.5 = **60.0%**
- Revenue recognised to date = 0.600 × 64.0 = **£38.40m**
- Margin recognised to date = 38.40 − 34.50 = **£3.90m** (which is 0.600 × 6.5)

**This close.** The raw ledger says £38.90m. Apply the accrual list: 38.90 + 0.82 + 0.34 − 0.61 − 0.15 = **£39.30m**. The estimate at completion moves to £58.6m, so expected margin falls to £5.4m.

- Percentage complete = 39.30 ÷ 58.60 = **67.06%**
- Revenue recognised to date = 0.6706 × 64.0 = **£42.92m**
- Margin recognised to date = 42.92 − 39.30 = **£3.62m**

| Line | Last close | This close |
|---|---:|---:|
| Contract price | £64.0m | £64.0m |
| Estimate at completion (cost) | £57.5m | £58.6m |
| Expected margin | £6.5m | £5.4m |
| Costs that measure progress | £34.5m | £39.3m |
| Percentage complete | 60.0% | 67.06% |
| Revenue to date | £38.40m | £42.92m |
| Margin to date | £3.90m | £3.62m |

## Why the result moved the wrong way

Margin booked in the month is 3.62 − 3.90 = **−£0.28m**. The job earned money and the accounts recorded a loss for the period. It splits into two parts.

Catch-up on work already recognised: 0.600 × (5.4 − 6.5) = **−£0.66m**. You had taken 60% of a £6.5m margin. You are now only entitled to 60% of a £5.4m margin, so the difference comes back immediately.

This month's own earning: (0.6706 − 0.600) × 5.4 = **+£0.38m**. Seven points of progress at the new margin rate.

Add them: −0.66 + 0.38 = −£0.28m. That is the mechanism. An estimate at completion is not a forward-looking number in the accounts. It restates every month you have already reported.

## What the revenue standard is asking

IFRS 15 in plain terms, as five questions the close has to answer:

1. Is there a contract — enforceable, with commercial substance and identified payment terms?
2. What has been promised in it, and are those promises distinct from one another?
3. What is the transaction price, including variable consideration such as incentives, liquidated damages and unapproved variations?
4. How is that price allocated across the promises?
5. When is each promise satisfied — at a point in time, or over time as the work proceeds?

Most capital work satisfies over time, which is why percentage of completion applies at all. The input method you pick in step 5 is what makes cut-off and accruals decisive rather than administrative. PCI publishes certification requirements; nothing here is accounting advice, and your reporting framework may differ.

## The line where it stops being a rounding argument

If the estimate at completion exceeds the contract price, the whole expected loss is recognised now, not spread. Suppose the forecast had landed at £65.2m instead of £58.6m.

Expected loss = 65.2 − 64.0 = £1.2m, taken in full, plus the £3.90m of margin already recognised comes back. £5.1m in one month, from a cost report.

This is the overlap the two professions do not share. A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy, almost never on a critical path or an earning rule.

An engineer is examined on float and progress measurement, almost never on cut-off or a contract asset. The money is lost in the space between them, and close is where that space is four days wide.

## What actually fixes it

One estimate at completion, one owner, one cut-off date, published before the month starts. A written cause attached to every movement over an agreed threshold — "cable pulling is running 10% below tender rate and the remaining 1,350m carries that rate" is something finance can price. A number on its own is not.

## Common follow-ups

**Why not just use the ledger and skip the accruals?**
Because the ledger records processing dates, not performance. On a job with 60-day subcontractor payment cycles, the unadjusted ledger understates cost by roughly a month's spend, which flatters the cost performance index and the margin at the same time.

**Is percentage complete always cost-to-cost?**
No. Cost-to-cost is common because it is auditable, but physical measures such as surveyed quantities or milestones can be more faithful. The test is whether the measure tracks the transfer of value to the customer, not whether it is convenient.

**Who should own the estimate at completion?**
One named person, usually the project controls manager, with the project director accountable for approving movements. Two owners means two numbers, and the one that reaches the accounts is whichever arrived first.

**What if the forecast comes back down next month?**
Then margin catches back up the same way, and you have taught your board that your forecast oscillates. That is worse than being wrong once. Move the estimate when the cause is understood, not when the mood changes.

---

*Disclosure: I write for the Project Controls Institute. One link, at the end, and the post stands without it: [month-end close for projects](https://projectcontrolsinstitute.org/month-end-close-for-projects).*

*Internal links: the in-post link uses the anchor "month-end close for projects". Follow-up comments should use [how the estimate at completion reaches the accounts](https://projectcontrolsinstitute.org/eac-accounting) and [IFRS 15 for construction](https://projectcontrolsinstitute.org/ifrs-15-for-construction) with those anchors.*
