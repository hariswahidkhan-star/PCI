---
platform:      Substack
type:          faq
title:         When to use each EAC formula, and what each assumes
meta:          The four estimate at completion formulas run on one set of numbers, and when to use each EAC formula: match the method to the cause, then test it with TCPI.
primary_kw:    when to use each EAC formula
secondary_kw:  estimate at completion, cost performance index, to-complete performance index, bottom-up ETC
pillar:        Earned value management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1459
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00267
---

# When to use each EAC formula, and what each assumes

The four estimate at completion formulas are: actual cost plus remaining budget, budget divided by cost performance index, actual cost plus remaining budget divided by CPI times SPI, and a bottom-up re-estimate. Each assumes something different about the work still to come. Knowing when to use each EAC formula is a judgement about cause, not about arithmetic.

*Written first for this newsletter, on a different set of numbers from anything on the Institute's site. Run the arithmetic yourself as you read; the point is the choosing, not the formulas.*

## What are the four EAC formulas?

Estimate at completion is the forecast total cost of the scope in question: what you have spent, plus what you expect the rest to cost.

| Method | Formula | Assumption you are signing |
|---|---|---|
| 1. Remaining work at budget | EAC = AC + (BAC − EV) | The overrun is behind you and the rest runs at plan |
| 2. Remaining work at current CPI | EAC = BAC ÷ CPI | Performance to date continues to the end |
| 3. Remaining work at CPI and SPI | EAC = AC + (BAC − EV) ÷ (CPI × SPI) | Recovering the schedule will keep costing money |
| 4. Bottom-up re-estimate | EAC = AC + a fresh ETC | The remaining scope can be re-estimated honestly, now |

Only the fourth is an estimate. The first three are extrapolations, which is their strength as well as their weakness: they are fast, repeatable and cannot be talked down in a review meeting.

## What do the four produce on one set of numbers?

A water treatment plant, BAC $58.0m over 36 months, reporting at the end of month 18. All figures are illustrative.

At cut-off: PV = $30.5m, EV = $27.4m, AC = $31.9m.

From those: CPI = 27.4 ÷ 31.9 = **0.859**. SPI = 27.4 ÷ 30.5 = **0.898**. Cost variance = 27.4 − 31.9 = −$4.5m. The plant is 27.4 ÷ 58.0 = **47.2%** complete by value with $30.6m of budgeted work left to earn.

| Method | Arithmetic | EAC | VAC |
|---|---|---:|---:|
| 1 | 31.9 + 30.6 | **$62.5m** | −$4.5m |
| 2 | 58.0 ÷ 0.859 | **$67.5m** | −$9.5m |
| 3 | CPI × SPI = 0.772; 30.6 ÷ 0.772 = 39.7; + 31.9 | **$71.6m** | −$13.6m |
| 4 | 31.9 + a re-estimated ETC of 36.8 | **$68.7m** | −$10.7m |

A spread of $62.5m to $71.6m, which is $9.1m, or 16% of the budget. That range is not a flaw in the method. It is the method showing how much of the forecast is assumption rather than measurement.

Before running any of it, check that actual cost is complete. Missing accruals for work done and goods received depress AC, flatter CPI, and produce a forecast that improves for a month and then collapses. If the month-end that produces these inputs is the part you are unsure of, [a full month of earned value worked end to end](https://projectcontrolsinstitute.org/earned-value-worked-example) shows where each figure comes from.

## When to use each EAC formula: matching the method to the cause

Write down the cause of the variance in one sentence, then pick. If you cannot write the sentence, you are not forecasting, you are extending a line on a chart.

| Cause of the variance | Method that matches it | Why |
|---|---|---|
| A discrete event that has finished: a flood, a one-off remediation, a settled claim | 1 | The money is spent and the driver is gone |
| A rate or productivity assumption that was wrong in the estimate | 2 | The same wrong assumption sits under every remaining unit |
| Acceleration: cost being spent to buy back a slipping date | 3 | The schedule position keeps generating cost until it is recovered |
| Remaining scope differs in kind from the work done, or the method has changed | 4 | Past performance is not evidence for different work |
| Early in the job with a small earned value sample | 4, with 2 as a check | CPI from 10% complete is noise as often as signal |

On this plant, the driver is a membrane installation rate that was assumed from a different plant and never validated. That is systemic, so method 2 at **$67.5m** is the defensible answer, with the bottom-up at $68.7m run alongside it as a challenge. The two landing $1.2m apart is the useful result.

## What breaks each method?

| Method | Fails when |
|---|---|
| 1. AC + remaining budget | The cause is systemic, which it usually is. This is the optimistic default and the first one an auditor challenges |
| 2. BAC ÷ CPI | CPI comes from an unrepresentative sample, or the remaining work has a different resource mix |
| 3. CPI × SPI | The delay costs nothing to recover, such as waiting on a permit, or the schedule is already being recovered free |
| 4. Bottom-up | It takes real time. A rushed one is the old estimate with a new date on it |

Method 3 deserves a warning. Multiplying two indices below 1 compounds pessimism quickly: 0.859 × 0.898 = 0.772 means the remaining work is forecast to run 30% worse than budget, which needs an acceleration case behind it and not just a slipped programme.

## How do you check the answer with TCPI?

To-complete performance index is the cost performance the remaining work must achieve to hit a stated target. Against the original budget: TCPI = (BAC − EV) ÷ (BAC − AC).

Here: 30.6 ÷ (58.0 − 31.9) = 30.6 ÷ 26.1 = **1.172**.

Compare that with delivered CPI of 0.859. The ratio is 1.172 ÷ 0.859 = **1.36**, a 36% improvement demanded from the same team on the same scope with 53% of the work still to do. Nobody delivers that, and saying so in month 18 is the entire value of the calculation.

Run TCPI against your chosen EAC and you get 30.6 ÷ (67.5 − 31.9) = 0.859, which is CPI again. That is the arithmetic being circular, not a confirmation, so never present it as evidence.

## Which number do you actually report?

Report the range, name the one you have chosen, and record the cause you wrote down. Three lines, every month.

A single EAC with no visible alternatives is a forecast nobody can challenge, and a forecast nobody can challenge is one nobody should rely on.

Then track how your own forecasts have moved. If the EAC has risen every month for six months, the forecasting method is not the problem; the willingness to write down the cause is.

The calculation worked examples across the PCI AI Project Finance Leader (PFL-AI) and PCI Project Management Leader – AI (PML-AI) Bodies of Knowledge are verified by a machine suite of 15,613 calculation checks, all currently passing. The PCI AI Project Controls Leader (PCL-AI) volume has no equivalent suite, and it would be wrong to imply otherwise.

## Frequently asked questions

**What is the difference between EAC and ETC?**
Estimate to complete is the forecast cost of the work remaining. Estimate at completion is that plus the cost already incurred, so EAC = AC + ETC. Methods 1 to 3 derive the ETC arithmetically from indices; method 4 builds it from scratch. Keeping the two labelled separately matters, because a sponsor asking "what is left to spend" is asking for ETC.

**Can an EAC be lower than the budget?**
Yes, and it should be when CPI is above 1 for a real reason, such as a procurement saving or a productivity gain that will continue. The reason has to be as specific as it would be for an overrun. Reporting an underrun without a cause is exactly the behaviour that makes an overrun report unbelievable later.

**How early is CPI reliable enough to forecast from?**
Once earned value is measured on a representative slice of the work rather than on mobilisation and preliminaries. Roughly a fifth complete is a common rule of thumb, and there is long-standing defence programme practice suggesting cumulative CPI rarely improves much after that point. Treat it as a prompt to test your own portfolio, not as a published finding.

**Is the weighted 0.8 CPI plus 0.2 SPI version legitimate?**
It is a documented judgement rule, not a law. On these numbers the denominator becomes 0.867, giving an EAC of $67.2m, close to method 2 because SPI is only slightly better than CPI. Use it if your cost control procedure names it and explains the weights; do not use it because a spreadsheet inherited it.

**Should contingency sit inside the EAC?**
Show both, separately. The EAC is the expected cost of the work as currently understood, and contingency covers identified risk that has not yet occurred. Blending them hides whether an overrun is being funded from a drawdown or is simply unfunded, and that is the question a sponsor is actually asking.

**Does the EAC change reported revenue?**
Where progress is measured by a cost-based input method, yes, because that method divides costs incurred by total expected costs, and total expected costs is your EAC. A higher EAC lowers percentage complete and reverses revenue already taken. That is a reason to tell finance the month the forecast moves, not accounting advice from PCI.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so this piece is not a copy of the PCI site's earned value pages.*

*Linking note: one link is now in the body. "A full month of earned value worked end to end" sits beside the warning about incomplete actual cost (https://projectcontrolsinstitute.org/earned-value-worked-example), because that sentence raises the question of where EV and AC come from in the first place. All three links proposed here pointed at the hub, and one link per domain is the limit, so the pillar page and the credential page were dropped rather than retargeted at something this piece does not raise. Nothing else in the piece asks a question another domain answers, so one link is the honest count. Reciprocal: none warranted.*
