---
platform:      Substack
type:          faq
title:         When should you use a bottom-up ETC instead of CPI?
meta:          A bottom-up ETC re-prices the remaining work instead of extrapolating it. The five triggers that call for one, and a worked forecast against the index method.
primary_kw:    bottom-up ETC
secondary_kw:  estimate to complete, estimate at completion, to-complete performance index, remaining work forecast
pillar:        Earned value management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1,822
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00266
---

# When should you use a bottom-up ETC instead of CPI?

Use a bottom-up ETC when the work still to come is not like the work already done. Re-price the remaining scope from quantities, rates and the current programme whenever the scope changes in kind, the method changes, the sample behind CPI is too small, or the index-based forecast has stopped being believed.

*Written first for this newsletter, on a project and a set of numbers that appear nowhere on the Institute's site. Every figure below is invented so the arithmetic can be checked line by line.*

## What is a bottom-up ETC?

Estimate to complete is the forecast cost of the work remaining at the data date. A bottom-up ETC builds that figure from the remaining quantities, the resources needed to install them, current rates and the time the programme says it will take.

The alternative is an index-based ETC, which divides the remaining budget by a performance index and accepts whatever comes out. Those are [the index-based forecasts this one is checked against](https://projectcontrolsinstitute.org/four-eac-formulas), and each is an extrapolation of the past rather than an estimate of the future.

Both produce an estimate at completion, because EAC = AC + ETC in every method. What differs is where the ETC came from and what you are able to say when someone asks why.

## Which five triggers call for a re-estimate?

Five triggers. If any one of them is true, the index forecast is answering a question about a project you are no longer running.

| Trigger | Why the index method fails | What the bottom-up gives you |
|---|---|---|
| Remaining scope differs in kind from completed scope | CPI was earned on earthworks and piling; the rest is systems and commissioning | A forecast built on the trades that remain |
| Execution method or contract has changed | A resequence, a novation or a switch from direct labour to subcontract resets the cost base | Rates from the contracts you actually hold |
| Earned value sample is too small | CPI at 8% complete is dominated by mobilisation and preliminaries | An estimate that does not depend on a thin sample |
| A material change has been approved | The baseline no longer describes the scope, so BAC ÷ CPI divides the wrong number | A re-priced remainder against the current scope |
| Nobody believes the forecast any more | An EAC that has crept up for five months has lost the room | A defensible position with owners' names against it |

The last one is not a technical trigger and it is the most common in practice. A forecast that has moved every month is telling you the method has stopped working.

## What does the arithmetic look like on one project?

A grid connection and substation package. BAC £34.6m over 26 months, reporting at the end of month 14.

At cut-off: PV = £19.4m, EV = £17.1m, AC = £20.5m.

From those: CPI = 17.1 ÷ 20.5 = **0.834**. SPI = 17.1 ÷ 19.4 = **0.881**. Cost variance = 17.1 − 20.5 = −**£3.4m**. The package is 17.1 ÷ 34.6 = **49.4%** complete by value, with £17.5m of budgeted work left to earn.

The index forecast: EAC = BAC ÷ CPI = 34.6 ÷ 0.834 = **£41.5m**, so VAC = −£6.9m.

Now the bottom-up, built package by package from remaining quantities and current rates:

| Remaining scope | Remaining budget (£m) | Re-estimated ETC (£m) | Delta (£m) |
|---|---:|---:|---:|
| Cable installation | 6.20 | 8.05 | +1.85 |
| Primary plant erection | 4.30 | 4.55 | +0.25 |
| Protection and control | 3.10 | 4.40 | +1.30 |
| Civils completion | 1.60 | 1.45 | −0.15 |
| Testing and energisation | 1.40 | 2.30 | +0.90 |
| Time-related site costs | 0.90 | 1.35 | +0.45 |
| **Total** | **17.50** | **22.10** | **+4.60** |

EAC = AC + ETC = 20.5 + 22.1 = **£42.6m**, so VAC = 34.6 − 42.6 = −**£8.0m**.

The two forecasts land £1.1m apart, which is the least interesting result. The useful one is that the bottom-up implies the remaining work will run at 17.50 ÷ 22.10 = **0.792**, worse than the 0.834 already delivered, and it names the two rows that say so.

## Why is the remaining work forecast worse than delivered performance?

Because two of the six rows are not a productivity problem at all, and the index method cannot see either of them.

Protection and control was priced from a scheme that has since been redesigned, so its remaining budget describes work nobody is going to do. Testing and energisation was estimated at a duration the current programme no longer supports.

Time-related site costs are the row that catches teams out. Site establishment, supervision and standing plant run against the calendar, so an eight-week extension adds cost to a package where no quantity has changed and no rate has moved.

Check the credibility of the original budget while you are there. TCPI = (BAC − EV) ÷ (BAC − AC) = 17.5 ÷ (34.6 − 20.5) = 17.5 ÷ 14.1 = **1.241**. Against a delivered CPI of 0.834 that is 1.241 ÷ 0.834 = **1.49**, a 49% improvement demanded from the same team on harder work. The budget is gone, and saying so in month 14 is worth more than any forecast method.

## How do you build one that survives a challenge?

Six steps, in this order. Skipping the first two is how a bottom-up ETC ends up as the old estimate with a new date on it.

1. Fix the data date and freeze the actual cost, accruals included. An ETC built on an understated AC understates the EAC by the same amount.

2. Take the remaining quantities from the current programme and the site measure, never from the budget. The budget's quantities are what the estimator assumed.

3. Price with rates you can point at: signed subcontracts, current purchase orders, agreed labour rates, quoted plant hire.

4. Cost the time separately. Take the time-related items from the current critical path, because a package that finishes eight weeks late carries eight weeks of preliminaries whatever the quantities do.

5. Keep identified risk out of the ETC and in the contingency line. Blending them hides whether an overrun is funded or unfunded.

6. Have each package owner sign their number. An ETC nobody owns is a spreadsheet, and it will be renegotiated in the meeting.

## Bottom-up or index-based: which one, and how often?

Run both. Use the index method every month as the fast challenge, and the bottom-up at points where the answer has to hold.

| | Index-based ETC | Bottom-up ETC |
|---|---|---|
| Input | CPI, and sometimes SPI | Quantities, rates, resources, the programme |
| Effort | Minutes, from data you already have | Days to weeks, across several people |
| Frequency | Every reporting cycle | Stage gates, major change, loss of confidence, year end |
| Strength | Repeatable and impossible to talk down | Explains itself row by row |
| Failure mode | Assumes tomorrow resembles yesterday | Optimism re-entering through the package owners |
| Best use | The monthly challenge to the reported EAC | The number that goes to the board |

Where the two disagree by more than a few per cent, the gap is the finding. Write down which rows cause it before choosing between them.

## What happens downstream when the ETC moves?

A bottom-up ETC does not stay in the cost report. Where a contractor measures progress by a cost-based input method, revenue follows costs incurred divided by total expected costs, and total expected costs is your EAC.

On these numbers, moving the EAC from £41.5m to £42.6m cuts measured completion from 20.5 ÷ 41.5 = 49.4% to 20.5 ÷ 42.6 = 48.1%, and revenue already taken is corrected in the period the forecast changes. Where the expected cost of finishing exceeds the consideration still to come, the whole expected loss is generally recognised at once rather than spread forward.

That is a reason to tell finance in the month the forecast moves. Nothing PCI publishes is accounting advice, and the point here is timing rather than treatment.

Forecasting across that boundary is what the PCI AI Project Finance Leader (PFL-AI) credential examines, across 16 domains and 61 knowledge areas. The worked calculations in the PFL-AI and PCI Project Management Leader – AI (PML-AI) Bodies of Knowledge are verified by a machine suite of 15,613 calculation checks, all currently passing.

## Frequently asked questions

**How long should a bottom-up ETC take?**
On a package of this size, two to three weeks with the package owners doing the pricing and cost control doing the assembly. Anything done in two days is a re-badged budget. Anything taking two months has been overtaken by the position it was meant to describe, and you will be re-running it before it is signed.

**Can you do a bottom-up ETC on part of the project only?**
Yes, and it is often the right answer. Re-price the control accounts where the trigger applies and leave the rest on the index method, provided the report states which is which. A mixed forecast with the boundary marked is honest; a mixed forecast presented as one number is not.

**Does a bottom-up ETC replace the baseline?**
No. The baseline is the measurement yardstick and a forecast is a prediction. Overwriting BAC with the new EAC destroys the variance history and, with it, any ability to show how the position developed. Change the baseline only through the change control process, for approved scope.

**What if the bottom-up comes out better than the index forecast?**
Treat it with the same scepticism you would apply to an overrun. Ask which rows improved and what evidence sits behind them, because a favourable re-estimate usually rests on assumed productivity gains rather than on signed rates. Underruns and overruns need the same standard of proof.

**Should contingency be re-set at the same time?**
Assess it, but keep it separate and show the drawdown rate against progress. If half the contingency has gone at 30% complete, the ETC is not the problem you need to be discussing. Re-setting contingency inside the same exercise makes it impossible to see which of the two moved.

**Who signs a bottom-up ETC?**
The package owners for their rows, the project controls manager for the assembly and the project director for the total. Record the date, the data date and the rate sources with it. A forecast without named owners reverts to the previous number the first time it is challenged.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so nothing here is a copy of a page on the PCI site.*

*Linking note: one link is now in the body. "The index-based forecasts this one is checked against" sits where the index method is first described (https://projectcontrolsinstitute.org/four-eac-formulas), because a reader meeting "divides the remaining budget by a performance index" wants to know which indices and which formulas. The pillar page and the budgeting page were dropped: all three proposals were hub pages and one link per domain is the limit. Nothing in this piece raises a question the other four domains answer, so a second link would have been decoration. Reciprocal: none warranted.*
