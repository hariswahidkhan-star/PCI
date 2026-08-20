---
platform:      Own site — projectcontrolsinstitute.org
type:          how-to
title:         The four EAC formulas: predicting final project cost
meta:          How to use the four EAC formulas to forecast final project cost, choose between them by cause of variance, and carry the answer into revenue and provisions.
primary_kw:    four EAC formulas
secondary_kw:  estimate at completion, cost-to-cost input method, variance at completion, onerous contract
pillar:        Earned value management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo + FAQPage
word_count:    1,786
hashtags:      n/a (own site)
ab_id:         AB-00215
---

# The four EAC formulas: predicting final project cost

The four EAC formulas take the same earned value inputs and produce four different final costs, because each one assumes something different about the work still to come. Choosing between them is a judgement about cause, not a preference about arithmetic. This is how to run all four, pick one, and defend it.

Estimate at completion (EAC) is the forecast total cost of the scope in question, expressed as cost already incurred plus the estimated cost of the work remaining.

## Step one: fix the inputs before you forecast anything

A forecast is only as good as the four numbers under it — BAC, PV, EV and AC — and all four have to be measured to the same cut-off. Where each of them comes from is the prior question, and [how PV, EV, AC and BAC are each built](https://projectcontrolsinstitute.org/earned-value-management) answers it; this guide assumes them.

Our example is a rail systems package. BAC is €24.0m over 30 months, and we are reporting at the end of month 14.

At cut-off: PV = €11.5m, EV = €10.2m, AC = €12.4m.

From those, CPI = 10.2 ÷ 12.4 = **0.823** and SPI = 10.2 ÷ 11.5 = **0.887**. The package is 10.2 ÷ 24.0 = **42.5%** complete by value, with €13.8m of budgeted work still to earn.

Before going further, check that actual cost includes accruals for work done and goods received, and excludes anything paid for that has not been installed. A forecast built on an incomplete AC understates the problem by exactly the amount of the missing accrual.

Building those inputs is its own exercise: [a worked month-end that produces these inputs](https://projectcontrolsinstitute.org/earned-value-worked-example) runs a package from quantities and accruals through to PV, EV and AC.

## Step two: diagnose the cause of the variance

This step decides the answer, and it happens before any formula.

Ask what produced the €2.2m cost variance. A discrete event that has finished, such as a flood or a one-off remediation, argues that the remaining work is unaffected.

A rate or productivity error argues the opposite, because the wrong assumption is embedded in every unit still to install. A slipping programme being bought back with overtime argues that the schedule position will keep generating cost.

Write the cause down in one sentence before you calculate. If you cannot write it, you are not forecasting, you are extrapolating.

## Step three: run all four EAC formulas

| Method | Formula | Result | VAC | Assumption you are signing |
|---|---|---:|---:|---|
| 1. Remaining work at budget | EAC = AC + (BAC − EV) | **€26.2m** | −€2.2m | The loss is behind you; the rest runs at plan |
| 2. Remaining work at current CPI | EAC = BAC ÷ CPI | **€29.2m** | −€5.2m | Performance to date continues to the end |
| 3. Remaining work at CPI and SPI | EAC = AC + (BAC − EV) ÷ (CPI × SPI) | **€31.3m** | −€7.3m | Schedule recovery will keep costing money |
| 4. Bottom-up re-estimate | EAC = AC + a fresh ETC of €15.9m | **€28.3m** | −€4.3m | The team can re-estimate the remaining scope honestly |

The arithmetic, so it can be checked. The indices are shown to three decimals above and carried to four here, which is what the divisions actually use. Method 1: 12.4 + 13.8 = 26.2. Method 2: 24.0 ÷ 0.8226 = 29.18. Method 3: CPI × SPI = 0.8226 × 0.8870 = 0.7296, then 13.8 ÷ 0.7296 = 18.91, plus 12.4 = 31.31.

A spread of €26.2m to €31.3m is €5.1m wide, which is 21% of the budget. That range is not a defect in the method. It is the method showing you how much of the forecast is assumption rather than measurement.

There is a fifth construction in common use: weight CPI and SPI rather than multiplying them, typically 0.8 × CPI + 0.2 × SPI as the denominator. Here that gives 0.8355, so 13.8 ÷ 0.8355 = 16.52, and EAC = **€28.9m**. The weights are a convention some cost control procedures adopt, not a rule with authority behind it. If you use them, write them into the procedure so the choice is visible and can be argued with, rather than into a spreadsheet nobody has seen.

## Step four: choose one, and sanity-check it with TCPI

Match the method to the cause you wrote down in step two.

On this package the cause is a productivity rate embedded in the estimate, so method 2 is the defensible default at **€29.2m**, with method 4 run alongside it as a challenge.

Now test whether the budget is still credible. TCPI to finish on budget = (BAC − EV) ÷ (BAC − AC) = 13.8 ÷ 11.6 = **1.190**.

Against a delivered CPI of 0.823, that is 1.190 ÷ 0.823 = **1.45**, a 45% improvement demanded from the same team on the same scope. Nobody produces that, and saying so in month 14 is the whole value of the exercise.

Run TCPI against your chosen EAC as well: 13.8 ÷ (29.2 − 12.4) = 13.8 ÷ 16.8 = 0.821, which is your CPI back again. That is the arithmetic being circular, not a confirmation, so never present it as one.

If you want to know whether recovery of that size has ever happened where you work, the check is available to you: take a set of completed packages, find their cumulative CPI at the same point of completion, and compare it with the CPI they finished on. That is evidence about your own portfolio, which is the only kind worth arguing a forecast with.

## Step five: carry the forecast into the accounts

This is the step most controls teams stop short of, and it is where the money actually lands.

Where progress towards satisfying a performance obligation is measured by an input method based on costs, the measure is costs incurred divided by total expected costs. Total expected costs is your EAC. Changing the forecast therefore changes revenue.

Take a contract price of €27.5m on this package.

| EAC chosen | Costs incurred ÷ total expected | Cumulative revenue |
|---|---:|---:|
| €26.2m (method 1) | 12.4 ÷ 26.2 = 47.3% | €13.02m |
| €29.2m (method 2) | 12.4 ÷ 29.2 = 42.5% | €11.68m |

A €1.34m difference in reported revenue, from a choice about a forecast method, on identical delivery. That is why the EAC is a finance number as much as a controls one, and it is the reason [where forecasting meets financial reporting](https://projectcontrolsinstitute.org/finance-and-project-management-certification) is examined as one subject rather than two.

The second consequence is sharper. At an EAC of €29.2m against a price of €27.5m, the contract is expected to lose €1.7m. Under the applicable financial reporting standards, an expected loss on a contract is generally recognised in full in the period it becomes apparent, rather than spread across the remaining programme.

Neither of these is a discretionary presentation choice, and neither is legal, tax or accounting advice from PCI. The point for a project controls team is narrower and non-negotiable: the month your forecast crosses the contract price is the month finance needs to hear about it.

## The five-step revenue model, in plain terms

Because the EAC feeds it, it helps to know where it feeds. The five-step model in IFRS 15, described here in PCI's own words rather than reproduced:

1. **Identify the contract.** An agreement with a customer that creates enforceable rights and obligations, has commercial substance, and where collection is considered.

2. **Identify the performance obligations.** The distinct promises within it. On an integrated construction or systems contract these are often combined into a single obligation, because the individual items are not distinct in the context of the contract.

3. **Determine the transaction price.** Including variable elements such as variations, claims and incentives, constrained so that revenue is only taken where a significant reversal is not expected.

4. **Allocate the price.** Across the obligations on a relative standalone selling price basis, where there is more than one.

5. **Recognise revenue as obligations are satisfied.** Over time where the criteria are met, measured by a chosen method of progress. The cost-based input method is the one your EAC drives.

Step three is where a controls team is usually most useful, because unapproved variations sitting in the change log are exactly the variable consideration that step three is asking about.

## What can go wrong with each method

| Method | Fails when |
|---|---|
| Remaining work at budget | The cause is systemic, which it usually is; this is the optimistic default and the one auditors challenge first |
| BAC ÷ CPI | CPI is drawn from an early or unrepresentative sample, or the remaining work has a different mix |
| CPI × SPI | The schedule slip costs nothing to recover, such as waiting on a permit or an approval |
| Bottom-up | It takes real time to do, and a rushed one is just the old estimate with a new date |

Run more than one, present the range, and name the one you have chosen. A single EAC with no visible alternatives is a forecast nobody can challenge, and a forecast nobody can challenge is one nobody should trust.

## Frequently asked questions

**Which EAC method is the default if I have no information?**
BAC divided by CPI, because it makes the fewest assumptions about the future and the most use of what you have measured. It is also the hardest to argue with, since it simply says the project will continue behaving as it has behaved. Move off it only when you can name the reason.

**How often should the EAC be revised?**
Monthly, with the reasoning recorded each time. A forecast that never moves is not stable, it is stale, and a forecast that moves every month by a large amount indicates the inputs are unreliable rather than the project volatile. Tracking the movement itself is a useful measure of how well the system works.

**Does the EAC include contingency?**
Keep them separate and show both. The EAC is the expected cost of the work as currently understood; contingency covers identified risk that has not yet materialised. Blending them hides whether an overrun is being funded by a drawdown or is simply unfunded, which is the question a sponsor is really asking.

**What if the four methods sit within a very narrow range?**
That normally means the project is close to complete, since the remaining work is small enough that the method barely matters. Early in a job the spread is wide and the choice matters most, which is the opposite of how much attention it usually receives.

**Can AI produce the forecast?**
It can assemble the inputs, flag control accounts whose behaviour has changed, and test whether a claimed recovery has ever been achieved on comparable work. It should not produce a number you cannot explain. If you cannot say which method was used and which assumption it rests on, the forecast cannot be defended in front of an auditor or a board.

---

*Internal linking note: three same-domain links now sit in the body. "How PV, EV, AC and BAC are each built" points at the earned value pillar, placed on the sentence that makes the whole forecast depend on four inputs measured to one cut-off, because a reader who cannot build those four cannot use anything below it. "A worked month-end that produces these inputs" points at the worked example, placed after the accrual check in step one, where a reader is being told to verify inputs the piece has simply handed them. "Where forecasting meets financial reporting" points at the finance and project management certification page, placed at the €1.34m revenue swing, which is the exact point where a controls decision becomes an accounting one. No cross-estate link is carried; the accounting overlap is hub territory by design. A GEO fix was also made: the five-step revenue model was one 140-word block and is now five separate blocks, each able to be lifted on its own. Reciprocal: the worked example and the reporting thresholds guide should link back here with an anchor naming this as the forecast-method choice.*
