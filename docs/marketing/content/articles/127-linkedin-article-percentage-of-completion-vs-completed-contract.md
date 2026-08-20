---
platform:      LinkedIn Article
type:          comparison
title:         Percentage of completion method vs completed contract
meta:          The percentage of completion method against completed contract, worked over three years, including what each does when the contract turns loss-making.
primary_kw:    percentage of completion method
secondary_kw:  completed contract method, cost-to-cost, revenue recognition timing, onerous contract
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1523
hashtags:      #ProjectControls #ProjectFinance #ConstructionAccounting #CostEngineering #QuantitySurveying
ab_id:         AB-00254
---

# Percentage of completion method vs completed contract

The percentage of completion method recognises revenue and profit as work progresses, using a measure such as cost-to-cost. The completed contract method recognises nothing until the job finishes. Under IFRS the choice no longer exists: control either transfers over time or it does not, and that test decides for you.

The comparison still matters, because the two patterns describe what a contractor's accounts look like, and because the second one survives in tax rules and in owner-managed practice.

This is a LinkedIn original written under the Institute's cost control and estimating pillar, where contract accounting meets the cost report.

## How the two methods differ, before any arithmetic

| Axis | Percentage of completion | Completed contract |
|---|---|---|
| Revenue lands | Each period, in proportion to progress | Entirely in the period of completion |
| Profit lands | Each period, at the expected margin | Entirely at completion |
| What it needs | A reliable forecast cost at completion, every month | Nothing but a final account |
| Volatility | Moves whenever the forecast moves | Flat, then a spike |
| Information value | The accounts track the work | The accounts say nothing for years |
| Cash effect | None. Cash follows billing, not recognition | None, for the same reason |
| Loss-making contracts | Full expected loss recognised as soon as expected | Full expected loss recognised as soon as expected |
| Where permitted | The over-time pattern under IFRS 15 and ASC 606 | Not an IFRS policy choice; persists mainly in US tax rules for small contractors |

The last row is the one people get wrong in interviews. Completed contract is not a policy a listed contractor may elect under IFRS. Where an IFRS reporter recognises everything at handover it is because the obligation failed the over-time test, and the three conditions behind that test are set out in [IFRS 15 for construction contracts](https://projectcontrolsinstitute.org/ifrs-15-for-construction).

## When does the percentage of completion method produce a different answer?

Take a three-year contract. Fixed price **£30.0m**, forecast cost **£24.0m**, so an expected margin of **£6.0m**, or 20%.

Costs incurred: year one **£8.4m**, year two **£10.8m**, year three **£4.8m**. Total £24.0m, exactly as forecast.

Cost-to-cost progress runs 8.4 / 24.0 = **35.0%** at the end of year one, 19.2 / 24.0 = **80.0%** at the end of year two, and 100% at completion.

| Year | POC revenue | POC cost | POC profit | Completed contract profit |
|---|---:|---:|---:|---:|
| 1 | £10.50m | £8.40m | **£2.10m** | £0.00m |
| 2 | £13.50m | £10.80m | **£2.70m** | £0.00m |
| 3 | £6.00m | £4.80m | **£1.20m** | **£6.00m** |
| Total | £30.00m | £24.00m | £6.00m | £6.00m |

Year two revenue is the cumulative figure less the cumulative figure already taken: 0.80 × 30.0 = £24.0m, minus £10.5m, gives £13.5m.

Same contract, same cash, same final profit. One set of accounts reports a business trading steadily; the other reports two years of nothing and a windfall.

## What happens when the forecast turns bad

The interesting difference is not the profitable case. It is the one where the estimate at completion moves.

Keep the same contract. At the end of year two the forecast cost at completion rises to **£31.0m**, so the contract is now expected to lose **£1.0m**.

**Percentage of completion.** Progress = 19.2 / 31.0 = **61.94%**. Cumulative revenue = 0.6194 × 30.0 = **£18.58m** against cumulative cost of £19.2m, a cumulative loss of **£0.62m** through the measurement itself.

The full expected loss must be recognised as soon as it is expected, so a provision of 1.0 − 0.62 = **£0.38m** is added.

Year two therefore reports revenue of 18.58 − 10.50 = **£8.08m**, cost of 10.80 + 0.38 = **£11.18m**, and a result of **−£3.10m**.

Cumulative result: 2.10 − 3.10 = **−£1.00m**, the whole expected loss, taken in the year the forecast moved.

**Completed contract.** Nothing is recognised until year three, with one exception that is not optional: the expected loss. Year two reports **−£1.00m** as a provision, and year three reports revenue £30.0m, cost £31.0m and a provision release of £1.0m, netting to zero.

| Year | POC result | Completed contract result |
|---|---:|---:|
| 1 | +£2.10m | £0.00m |
| 2 | −£3.10m | −£1.00m |
| 3 | £0.00m | £0.00m |
| Total | −£1.00m | −£1.00m |

Recognising as the work progressed reported £2.10m of profit in year one and then took £3.10m back. That reversal is the honest cost of recognising profit against a forecast, and it is why the forecast is a financial statement number rather than a management one.

## Neither method is a cash flow

Both tables above are identical in cash terms. Recognition does not bill anyone, and billing does not collect.

A contractor can report the £2.10m of year one profit while its bank balance falls, because retention is held, applications are certified late, and materials are paid for before they are installed.

That gap between recognised profit and available cash is a separate discipline, worked out in [project cash flow forecasting](https://projectcontrolsinstitute.org/project-cash-flow-forecasting).

## What the choice does to a bank covenant and a tax bill

Banking covenants are usually written on earnings and net assets, both of which are recognition figures rather than cash figures.

On the profitable version of the contract above, recognising as you go delivers £2.10m of earnings in year one. The completed contract pattern delivers nothing, so a contractor with several such jobs can be trading well and still breach an earnings covenant in the year it is busiest.

The balance sheet moves too. Recognising as you go creates contract assets and contract liabilities on every job; deferring recognition parks the whole position in one line until completion, which tells a lender almost nothing about how the work is going.

Tax follows a different rule again, set by the jurisdiction rather than by the accounts. Deferring recognition defers the tax charge, and that deferral is the main reason a completed-contract method survives anywhere at all.

None of those three consequences is a reason to choose a method. They are reasons to know which one you are reporting under before somebody asks why the year looks like it does.

## What percentage of completion actually requires you to have

Cost-to-cost has one structural weakness: it treats money spent as work done. Spend £1m badly and progress goes up.

That is why the measure has to be defended by the same discipline earned value uses. A quantity surveyed and verified, an earning rule fixed before the work started, a cut-off applied to cost and progress on the same date.

Three inputs decide whether the method reports anything meaningful.

**Costs incurred to the cut-off**, accrued rather than invoiced, split between costs that measure progress and costs that do not.

**A forecast cost at completion** on the same basis as the numerator, with a change log for every movement.

**A claim and variation position** stating what may enter the transaction price and what may not.

Get those wrong and cost-to-cost produces a precise number that is wrong in a way no journal entry can detect. Get them right and it is the only method that tells you anything while the work is running.

## Frequently asked questions

**Is the completed contract method banned?**
Not banned, but it is not an IFRS policy election. Under IFRS 15 the pattern of recognition follows the transfer of control, so an entity recognises at a point in time only when the over-time conditions fail. In the United States the equivalent policy choice was removed for financial reporting by ASC 606, while a small-contractor exemption survives in the tax code, which is why the term is still heard in owner-managed practice.

**Does percentage of completion mean the same as cost-to-cost?**
No. Percentage of completion is the pattern; cost-to-cost is one way of measuring it. Output measures such as verified physical quantities, surveys of work performed or rules-of-credit tables are equally acceptable and are often better. Cost-to-cost dominates because the data already sits in the ledger and can be audited without a site visit.

**Can payment milestones be used as the measure of progress?**
Rarely, and usually not. Payment milestones are negotiated for cash flow and are frequently front-loaded, so using them as a progress measure imports that front-loading straight into reported revenue. A milestone only measures progress if it represents output genuinely transferred to the customer at that point.

**Why did year one profit reverse in the loss example?**
Because the profit was recognised against a forecast that later moved. Cost-to-cost recognises the expected margin proportionally, so a rise in the estimate at completion resets the margin percentage across every pound already recognised. The reversal is arithmetic working correctly, not an error being corrected.

**Which method should a contractor's management accounts use?**
The same one as the statutory accounts, or the board is managing a different business from the one it reports. Where an obligation genuinely fails the over-time test, run the internal reporting on progress anyway and reconcile it to the statutory position, because a project cannot be managed on a number that stays at zero for three years.

---

*PCI publishes certification requirements and does not provide accounting, legal or tax advice. The standards named here are described in the Institute's own words rather than reproduced.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Internal links: this article should link to [IFRS 15 for construction contracts](https://projectcontrolsinstitute.org/ifrs-15-for-construction) with that anchor, to [project cash flow forecasting](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) with that anchor, and to [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls) as the pillar it supports.*
