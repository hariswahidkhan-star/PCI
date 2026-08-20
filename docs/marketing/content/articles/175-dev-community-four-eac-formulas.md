---
platform:      DEV Community
type:          how-to
title:         Using EVM to forecast final cost: the four EAC formulas
meta:          Using EVM to predict final project cost: the four EAC formulas run on one dataset, TCPI as a reality check, and what the forecast does to reported revenue.
primary_kw:    four EAC formulas
secondary_kw:  estimate at completion, TCPI, cost-to-cost input method, variance at completion
pillar:        Earned value management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/four-eac-formulas
schema:        HowTo + FAQPage
word_count:    1766
hashtags:      #python #datascience #finance #tutorial
ab_id:         AB-00215
---

# Using EVM to forecast final cost: the four EAC formulas

The four EAC formulas take the same earned value inputs and return four different final costs, because each assumes something different about the work still to come. Choosing between them is a judgement about cause, not a preference about arithmetic. Here is all four run on one dataset, with the code.

Estimate at completion (EAC) is the forecast total cost of the scope in question: cost already incurred, plus the estimated cost of the work remaining.

## Step 1 — Fix the inputs before forecasting anything

Three measurements, all taken to the same cut-off, produce everything else, and [a worked month-end that produces them](https://projectcontrolsinstitute.org/earned-value-worked-example) shows where each one is read from.

Planned value (PV) is the budgeted cost of work scheduled. Earned value (EV) is the budgeted cost of work performed. Actual cost (AC) is what was spent on that work.

Our example is a rail systems package. Budget at completion (BAC) is €24.0m over 30 months, reporting at the end of month 14. At cut-off: PV = €11.5m, EV = €10.2m, AC = €12.4m.

| Measure | Formula | Value |
|---|---|---:|
| Cost variance | CV = EV − AC | −€2.2m |
| Schedule variance | SV = EV − PV | −€1.3m |
| Cost performance index | CPI = EV ÷ AC | **0.823** |
| Schedule performance index | SPI = EV ÷ PV | **0.887** |
| Per cent complete by value | EV ÷ BAC | 42.5% |
| Budget remaining | BAC − EV | €13.8m |

Before going further, check that AC includes accruals for work done and goods received, and excludes anything paid for but not installed. A forecast built on an incomplete AC understates the problem by exactly the size of the missing accrual, and it does so silently.

## Step 2 — Diagnose the cause of the variance

This step decides the answer, and it happens before any formula runs.

Ask what produced the €2.2m cost variance. A discrete event that has finished, such as a flood or a one-off remediation, argues the remaining work is unaffected.

A rate or productivity error argues the opposite, because the wrong assumption is embedded in every unit still to install. A slipping programme being bought back with overtime argues the schedule position will keep generating cost.

Write the cause down in one sentence before calculating. If you cannot write it, you are not forecasting, you are extrapolating.

## Step 3 — Run all four

```python
def eac_remaining_at_budget(ac, bac, ev):        # method 1
    return ac + (bac - ev)

def eac_at_current_cpi(bac, cpi):                # method 2
    return bac / cpi

def eac_cpi_and_spi(ac, bac, ev, cpi, spi):      # method 3
    return ac + (bac - ev) / (cpi * spi)

def eac_bottom_up(ac, etc):                      # method 4
    return ac + etc

PV, EV, AC, BAC = 11.5, 10.2, 12.4, 24.0
cpi, spi = EV / AC, EV / PV

for name, eac in (
    ("remaining at budget", eac_remaining_at_budget(AC, BAC, EV)),
    ("at current CPI",      eac_at_current_cpi(BAC, cpi)),
    ("CPI x SPI",           eac_cpi_and_spi(AC, BAC, EV, cpi, spi)),
    ("bottom-up",           eac_bottom_up(AC, 15.9)),
):
    print(f"{name:22} EAC {eac:6.2f}  VAC {BAC - eac:6.2f}")
```

| Method | Formula | Result | VAC | The assumption you are signing |
|---|---|---:|---:|---|
| 1. Remaining work at budget | AC + (BAC − EV) | **€26.2m** | −€2.2m | The loss is behind you; the rest runs at plan |
| 2. Remaining work at current CPI | BAC ÷ CPI | **€29.2m** | −€5.2m | Performance to date continues to the end |
| 3. Remaining work at CPI and SPI | AC + (BAC − EV) ÷ (CPI × SPI) | **€31.3m** | −€7.3m | Schedule recovery will keep costing money |
| 4. Bottom-up re-estimate | AC + a fresh ETC of €15.9m | **€28.3m** | −€4.3m | The team can re-estimate the remaining scope honestly |

The arithmetic, so it can be checked by hand. Method 1: 12.4 + 13.8 = 26.2. Method 2: 24.0 ÷ 0.823 = 29.18. Method 3: 0.823 × 0.887 = 0.730, then 13.8 ÷ 0.730 = 18.91, plus 12.4 = 31.31.

A spread of €26.2m to €31.3m is €5.1m wide, or 21% of the budget. That range is not a defect in the method. It is the method showing how much of the forecast is assumption rather than measurement.

A fifth construction is in common use: weight CPI and SPI rather than multiplying them, typically 0.8 × CPI + 0.2 × SPI as the denominator. Here that gives 0.836, so 13.8 ÷ 0.836 = 16.51 and EAC = **€28.9m**. It is a documented judgement rule, not a law, and it belongs in the cost control procedure rather than in a spreadsheet nobody has seen.

## Step 4 — Choose one, then test it with TCPI

Match the method to the cause written down in step 2. On this package the cause is a productivity rate embedded in the estimate, so method 2 is the defensible default at **€29.2m**, with method 4 run alongside as a challenge.

Now test whether the budget is still credible. TCPI to finish on budget = (BAC − EV) ÷ (BAC − AC) = 13.8 ÷ 11.6 = **1.190**.

Against a delivered CPI of 0.823, that is 1.190 ÷ 0.823 = **1.45**: a 45% improvement demanded from the same team on the same scope. Nobody produces that, and saying so in month 14 is the entire value of the exercise.

Run TCPI against the chosen EAC as well: 13.8 ÷ (29.2 − 12.4) = 0.821, which is CPI back again. That is the arithmetic being circular, not a confirmation, so never present it as one.

## Where each method fails

| Method | Fails when |
|---|---|
| Remaining work at budget | The cause is systemic, which it usually is; the optimistic default, and the one auditors challenge first |
| BAC ÷ CPI | CPI comes from an early or unrepresentative sample, or the remaining work has a different mix |
| CPI × SPI | Schedule slip costs nothing to recover, such as waiting on a permit or an approval |
| Bottom-up | It takes real time, and a rushed one is the old estimate with a new date on it |

Run more than one, present the range, and name the one you have chosen. A single EAC with no visible alternatives is a forecast nobody can challenge, and a forecast nobody can challenge is one nobody should trust.

## Step 5 — Carry the forecast into the accounts

This is the step most controls teams stop short of, and where the money actually lands.

Where progress towards satisfying a performance obligation is measured by an input method based on costs, the measure is costs incurred divided by total expected costs. Total expected costs is your EAC, so changing the forecast changes revenue.

Take a contract price of €27.5m on this package.

| EAC chosen | Costs incurred ÷ total expected | Cumulative revenue |
|---|---:|---:|
| €26.2m (method 1) | 12.4 ÷ 26.2 = 47.3% | €13.02m |
| €29.2m (method 2) | 12.4 ÷ 29.2 = 42.5% | €11.68m |

A €1.34m difference in reported revenue, from a choice about a forecast method, on identical delivery. The EAC is a finance number as much as a controls one.

The second consequence is sharper. At an EAC of €29.2m against a price of €27.5m, the contract is expected to lose €1.7m, and an expected loss on a contract is generally recognised in full in the period it becomes apparent rather than spread across the remaining programme.

Nothing PCI publishes is legal, tax or accounting advice. The point for a controls team is narrower and non-negotiable: the month your forecast crosses the contract price is the month finance needs to hear about it.

## The five-step revenue model, in plain terms

Because the EAC feeds it, it helps to know where. The five-step model in IFRS 15, described here in our own words rather than reproduced.

1. **Identify the contract.** An agreement creating enforceable rights and obligations, with commercial substance, where collection is considered.

2. **Identify the performance obligations.** The distinct promises within it. On an integrated construction or systems contract these are often combined into one obligation.

3. **Determine the transaction price.** Including variable elements such as variations, claims and incentives, constrained so revenue is only taken where a significant reversal is not expected.

4. **Allocate the price** across the obligations on a relative standalone selling price basis, where there is more than one.

5. **Recognise revenue as obligations are satisfied**, over time where the criteria are met, measured by a chosen method of progress. The cost-based input method is the one your EAC drives.

Step three is where a controls team is usually most useful, because unapproved variations sitting in the change log are exactly the variable consideration that step asks about.

The PCI AI Project Finance Leader (PFL-AI) examines that crossing across 16 domains and 61 knowledge areas. The calculation content behind the PFL-AI and PCI Project Management Leader – AI (PML-AI) volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

## Frequently asked questions

**Which EAC method is the default when there is no other information?**
BAC divided by CPI, because it makes the fewest assumptions about the future and the most use of what has been measured. It is also the hardest to argue with, since it says the project will continue behaving as it has behaved. Move off it only when you can name the reason.

**How often should the EAC be revised?**
Monthly, with the reasoning recorded each time. A forecast that never moves is stale rather than stable, and one that swings widely every month indicates unreliable inputs rather than a volatile project. Tracking the movement itself is a useful measure of how well the reporting system works.

**Does the EAC include contingency?**
Keep them separate and show both. The EAC is the expected cost of the work as currently understood; contingency covers identified risk that has not yet materialised. Blending them hides whether an overrun is being funded by a drawdown or is simply unfunded, which is the question a sponsor is actually asking.

**What if all four methods land in a narrow range?**
That usually means the project is close to complete, because the remaining work is too small for the method to matter. Early in a job the spread is widest and the choice matters most, which is the opposite of how much scrutiny it normally receives.

**Can a model produce the forecast?**
It can assemble the inputs, flag control accounts whose behaviour has changed, and test whether a claimed recovery has ever been achieved on comparable work. It should not emit a number you cannot explain, and [the line between what a model prepares and what a person decides](https://pciai.org/ai-in-project-controls) is the whole of governed AI in a controls function. If you cannot say which method was used and which assumption it rests on, the forecast will not survive an audit or a board.

---

*First published on projectcontrolsinstitute.org; the `canonical_url` on this post points there. DEV prohibits promotional-first posts, so this carries the four formulas and the worked figures rather than a pitch.*

*Internal links: two are now in the body. "A worked month-end that produces them" points at projectcontrolsinstitute.org/earned-value-worked-example, placed in step one, because that sentence raises where PV, EV and AC are actually read from and this piece takes them as given. "The line between what a model prepares and what a person decides" points at pciai.org/ai-in-project-controls, placed in the FAQ answer on whether a model can produce the forecast, because that answer raises where the boundary sits and the AI site is where it is worked out. The earned value pillar and eac-accounting links proposed earlier were dropped to hold one link per domain. Reciprocal: the eac-accounting page should link back here from its method-selection table, with an anchor naming the four methods on one dataset.*
