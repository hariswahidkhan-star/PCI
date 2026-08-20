---
platform:      Medium
type:          how-to
title:         The four EAC formulas: forecasting final project cost
meta:          How to run the four EAC formulas on one dataset, choose between them by cause of variance, and carry the chosen forecast into revenue and an expected loss.
primary_kw:    four EAC formulas
secondary_kw:  estimate at completion, cost-to-cost input method, variance at completion, onerous contract
pillar:        Earned value management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /four-eac-formulas (own site #023)
schema:        HowTo
word_count:    1692
hashtags:      #ProjectControls #EarnedValue #ProjectFinance #CostEngineering #RiskManagement
ab_id:         AB-00215
---

# The four EAC formulas: forecasting final project cost

The four EAC formulas take identical earned value inputs and return four different final costs, because each assumes something different about the work still to come. Choosing between them is a judgement about cause, not a preference about arithmetic. Here is how to run all four, pick one and defend it.

Estimate at completion (EAC) is the forecast total cost of the scope in question: cost already incurred, plus the estimated cost of the work remaining.

## Step one: fix the inputs before forecasting anything

A forecast is only as sound as the four numbers under it, and all four must be measured to the same cut-off.

The example is a rail systems package. BAC is €24.0m over 30 months, reporting at the end of month 14.

At cut-off: PV = €11.5m, EV = €10.2m, AC = €12.4m.

From those, CPI = 10.2 ÷ 12.4 = **0.823** and SPI = 10.2 ÷ 11.5 = **0.887**. The package is 10.2 ÷ 24.0 = **42.5%** complete by value, with €13.8m of budgeted work still to earn.

Before going further, confirm that actual cost includes accruals for work done and goods received, and excludes anything paid for but not installed. A forecast built on an incomplete AC understates the problem by exactly the size of the missing accrual.

## Step two: diagnose the cause of the variance

This step decides the answer, and it happens before any formula.

Ask what produced the €2.2m cost variance. A discrete event that has finished, a flood or a one-off remediation, argues that the remaining work is unaffected.

A rate or productivity error argues the opposite, because the wrong assumption is embedded in every unit still to install. A slipping programme being bought back with overtime argues that the schedule position will keep generating cost until it is recovered.

Write the cause down in one sentence before you calculate. If you cannot write it, you are not forecasting, you are extrapolating.

## Step three: run all four EAC formulas

| Method | Formula | Result | VAC | The assumption you are signing |
|---|---|---:|---:|---|
| 1. Remaining work at budget | EAC = AC + (BAC − EV) | **€26.2m** | −€2.2m | The loss is behind you; the rest runs at plan |
| 2. Remaining work at current CPI | EAC = BAC ÷ CPI | **€29.2m** | −€5.2m | Performance to date continues to the end |
| 3. Remaining work at CPI and SPI | EAC = AC + (BAC − EV) ÷ (CPI × SPI) | **€31.3m** | −€7.3m | Schedule recovery will keep costing money |
| 4. Bottom-up re-estimate | EAC = AC + a fresh ETC of €15.9m | **€28.3m** | −€4.3m | The team can re-estimate the remaining scope honestly |

The arithmetic, so it can be checked. Method 1: 12.4 + 13.8 = 26.2. Method 2: 24.0 ÷ 0.823 = 29.18. Method 3: CPI × SPI = 0.823 × 0.887 = 0.730, then 13.8 ÷ 0.730 = 18.91, plus 12.4 = 31.31.

A spread of €26.2m to €31.3m is €5.1m wide, which is 21% of the budget. That range is not a defect. It is the method showing how much of the forecast is assumption rather than measurement.

A fifth construction is in common use: weight CPI and SPI instead of multiplying them, usually 0.8 × CPI + 0.2 × SPI as the denominator. Here that gives 0.836, so 13.8 ÷ 0.836 = 16.51, and EAC = **€28.9m**. It is a documented judgement rule, not a law, and it belongs in the cost control procedure rather than in a spreadsheet nobody has reviewed.

## Step four: choose one, then sanity-check it with TCPI

Match the method to the cause you wrote down in step two.

On this package the cause is a productivity rate embedded in the estimate, so method 2 is the defensible default at **€29.2m**, with method 4 run alongside it as a challenge.

Now test whether the budget is still credible. TCPI to finish on budget = (BAC − EV) ÷ (BAC − AC) = 13.8 ÷ 11.6 = **1.190**.

Against a delivered CPI of 0.823, that is 1.190 ÷ 0.823 = **1.45**: a 45% improvement demanded from the same team on the same scope. Nobody produces that, and saying so in month 14 is the whole point of the exercise.

Run TCPI against your chosen EAC as well: 13.8 ÷ (29.2 − 12.4) = 13.8 ÷ 16.8 = 0.821, which is your CPI back again. That is the arithmetic being circular, not a confirmation, so never present it as one.

There is a long-standing rule of thumb from defence programme practice that cumulative CPI rarely improves much once a project is past roughly a fifth complete. Treat it as a prompt to test your own portfolio, not as a published finding.

## Step five: carry the forecast into the accounts

Most controls teams stop before this step, and it is where the money actually lands.

Where progress towards satisfying a performance obligation is measured by an input method based on cost, the measure is costs incurred divided by total expected costs. Total expected costs is your EAC, so changing the forecast changes revenue.

Take a contract price of €27.5m on this package.

| EAC chosen | Costs incurred ÷ total expected | Cumulative revenue |
|---|---:|---:|
| €26.2m (method 1) | 12.4 ÷ 26.2 = 47.3% | €13.02m |
| €29.2m (method 2) | 12.4 ÷ 29.2 = 42.5% | €11.68m |

A €1.34m difference in reported revenue, produced by a choice of forecast method, on identical delivery. That is why the EAC is a finance number as much as a controls one.

The second consequence is sharper. At an EAC of €29.2m against a price of €27.5m, the contract is expected to lose €1.7m. Under the applicable financial reporting standards, an expected loss on a contract is generally recognised in full in the period it becomes apparent, rather than spread across the remaining programme.

Neither of these is a discretionary presentation choice, and nothing here is legal, tax or accounting advice. The operating rule for a controls team is narrower: the month your forecast crosses the contract price is the month finance needs to hear about it.

## The five-step revenue model, in plain terms

Because the EAC feeds it, it helps to know where it feeds. The five-step model in IFRS 15, described in PCI's own words rather than reproduced:

1. **Identify the contract.** An agreement with a customer creating enforceable rights and obligations, with commercial substance, where collection is considered.

2. **Identify the performance obligations.** The distinct promises inside it. On an integrated construction or systems contract these are often combined into a single obligation, because the individual items are not distinct in the context of the contract.

3. **Determine the transaction price.** Including variable elements such as variations, claims and incentives, constrained so revenue is taken only where a significant reversal is not expected.

4. **Allocate the price.** Across the obligations on a relative standalone selling price basis, where there is more than one.

5. **Recognise revenue as obligations are satisfied.** Over time where the criteria are met, measured by a chosen method of progress. The cost-based input method is the one your EAC drives.

Step three is where a controls team is usually most useful, because unapproved variations sitting in the change log are precisely the variable consideration step three is asking about.

That overlap between a forecast and a ledger is the reason the [PCI AI Project Finance Leader (PFL-AI) credential](https://projectcontrolsinstitute.org/finance-and-project-management-certification) examines both sides. PFL-AI and the PCI Project Management Leader – AI (PML-AI) carry 15,613 machine calculation checks, all passing, which is how the arithmetic in that material is kept honest.

## What can go wrong with each method

| Method | Fails when |
|---|---|
| Remaining work at budget | The cause is systemic, which it usually is; this is the optimistic default and the first one an auditor challenges |
| BAC ÷ CPI | CPI comes from an early or unrepresentative sample, or the remaining work has a different mix |
| CPI × SPI | The schedule slip costs nothing to recover, such as waiting on a permit or an approval |
| Bottom-up | It takes real time to do, and a rushed one is the old estimate with a new date on it |

Run more than one, present the range, and name the one you have chosen. A single EAC with no visible alternatives is a forecast nobody can challenge, and a forecast nobody can challenge is one nobody should trust.

## Frequently asked questions

**Which EAC method is the default if I have no other information?**
BAC divided by CPI, because it makes the fewest assumptions about the future and the most use of what has been measured. It is also the hardest to argue with, since it simply says the project will keep behaving as it has behaved. Move off it only when you can name the reason for doing so.

**How often should the EAC be revised?**
Monthly, with the reasoning recorded each time. A forecast that never moves is stale rather than stable, and one that swings heavily every month says the inputs are unreliable rather than the project volatile. The movement itself is a useful measure of how well the system works.

**Does the EAC include contingency?**
Keep them separate and show both. The EAC is the expected cost of the work as currently understood; contingency covers identified risk that has not yet materialised. Blending them hides whether an overrun is being funded by a drawdown or is simply unfunded, which is the question a sponsor is really asking.

**What if all four methods land in a narrow range?**
That normally means the project is close to complete, because the remaining work is small enough that the method barely matters. Early in a job the spread is at its widest and the choice matters most, which is the reverse of how much attention it usually gets.

**Can AI produce the forecast?**
It can assemble the inputs, flag control accounts whose behaviour has shifted, and test whether a claimed recovery has ever been achieved on comparable work. It should not produce a number you cannot explain. If you cannot name the method and the assumption behind it, the forecast will not survive an auditor or a board.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so treat this republish as distribution, not as a backlink.*

*Internal links, as placed in the body. The one estate link sits in the five-step section, on [the PCI AI Project Finance Leader (PFL-AI) credential](https://projectcontrolsinstitute.org/finance-and-project-management-certification), because that paragraph asks who is examined on the crossing between a forecast and a ledger. It stays at one: the earned value pillar and the worked month-end proposed earlier would have put three links on a single domain, which is the footprint this run avoids. Reciprocal: the hub's worked month-end example has honest reason to point here, because its own forecast table raises the method choice this piece settles.*
