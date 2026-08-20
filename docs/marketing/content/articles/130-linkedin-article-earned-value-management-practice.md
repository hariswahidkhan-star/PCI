---
platform:      LinkedIn Article
type:          guide
title:         Earned value management practice, not compliance theatre
meta:          An earned value management practice changes decisions; theatre changes reports. The tests, a worked month, four forecasts and where to switch EVM off.
primary_kw:    earned value management practice
secondary_kw:  earning rules, to-complete performance index, estimate at completion, level of effort
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1614
hashtags:      #EarnedValue #ProjectControls #CostEngineering #PMO #ProjectManagement
ab_id:         AB-00216
---

# Earned value management practice, not compliance theatre

Earned value becomes management when the numbers change a decision, and theatre when they change a report. The difference is set before any data is collected, by the earning rules, the cut-off discipline and who owns the forecast. Better dashboards do not fix it.

Run as an earned value management practice, the system produces one or two numbers a director can act on each month. Run as compliance, it produces eight indices nobody has ever overruled.

This is a LinkedIn original written under the Institute's earned value pillar.

## What separates an earned value management practice from theatre?

| Artefact | Theatre | Practice |
|---|---|---|
| Earning rules | Assessed percent complete on most packages | Rule fixed per package before work starts, written in the procedure |
| Cut-off | Progress to one date, cost to another | Same date for progress, cost and accrual, every month |
| Actual cost | Invoices only | Incurred, with goods received and not invoiced accrued |
| Forecast | Produced by the cost engineer alone | Owned by the manager who can act on it, challenged monthly |
| Reporting | Every index, every package, coloured | Two indices, exceptions only, with a named action |
| Response | The variance is explained | The variance triggers a decision at a stated threshold |
| Baseline | Re-baselined when it looks bad | Changed only through change control, with the old one kept |

Row four is the one that decides the rest. A forecast the delivery manager did not produce is a forecast the delivery manager will not defend, and an undefended forecast is a report.

## A worked month that earns its place

An electrical and instrumentation package. Budget at completion **€18.0m** over 52 weeks. It is the end of week 26.

At cut-off: planned value **€9.6m**, earned value **€8.7m**, actual cost **€9.4m**.

**Variances.** CV = EV − AC = 8.7 − 9.4 = **−€0.70m**. SV = EV − PV = 8.7 − 9.6 = **−€0.90m**.

**Indices.** CPI = EV / AC = 8.7 / 9.4 = **0.926**. SPI = EV / PV = 8.7 / 9.6 = **0.906**.

So far this is arithmetic anyone can produce, and on its own it changes nothing. The next number is the one that ends the meeting.

**Remaining budgeted work.** BAC − EV = 18.0 − 8.7 = **€9.3m**.

**To-complete performance index.** TCPI = (BAC − EV) / (BAC − AC) = 9.3 / (18.0 − 9.4) = 9.3 / 8.6 = **1.081**.

The crew has run at 0.926 for six months. Finishing on budget now requires 1.081, which is 1.081 / 0.926 = **1.17**, a 17% step change in productivity from the same people, on the same site, with the same drawings.

That sentence is the whole argument for running earned value. Nobody delivers a 17% step change from a recovery plan written in week 27, so the conversation moves to scope, money or date, which is where it should have been a month earlier.

## Four forecasts from one dataset, and the choice you are signing

| Method | Formula | Assumes | This package |
|---|---|---|---|
| Remaining work at budget | EAC = AC + (BAC − EV) | The overrun is behind you | 9.4 + 9.3 = **€18.70m** |
| Remaining work at current CPI | EAC = BAC / CPI | Performance to date continues | 18.0 / 0.926 = **€19.45m** |
| Remaining work at CPI and SPI | EAC = AC + (BAC − EV) / (CPI × SPI) | Schedule pressure keeps costing money | 9.4 + 9.3 / 0.839 = **€20.49m** |
| Bottom-up re-estimate | EAC = AC + a fresh estimate to complete | The team can re-estimate honestly | 9.4 + 10.2 = **€19.60m** |

A spread of €18.7m to €20.5m from one set of inputs. That range is not a defect. It is the method asking which assumption you are prepared to sign.

Choose by cause. A one-off event that has finished argues for the first row. A rate or productivity error argues for the second. A slipping programme being bought back with overtime argues for the third. A change of method or scope argues for a re-estimate, and probably a change notice as well.

The first row is the one that gets picked in theatre, because it produces the smallest number and requires no argument. It is defensible only when someone can name the event that has ended.

Using the CPI method, variance at completion is 18.0 − 19.45 = **−€1.45m**. That is what the contingency conversation is actually about, and the four methods are compared in detail in [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas).

## The money variance says nothing about the date

SV of −€0.90m is a cost-shaped statement about time, and it has two faults. It cannot tell you which activities are late, and it converges to zero as a project completes, so it reports on time at the moment it is most wrong.

Only the network answers the date question. On this package the driving path runs through cable pulling, termination and loop checks, and it carried 15 days of total float at baseline. It now carries −5 days.

"The driving path has lost 20 days of float and the commissioning window closes in April" is actionable. "SV is minus 0.9" is a number waiting for a translator.

Earned schedule closes part of the gap by converting earned value into the week the baseline said you would reach it, which at least gives an answer in time units. It still does not name the activity, and the [critical path method](https://projectcontrolsinstitute.org/critical-path-method) does.

## Three settings that decide whether any of it is honest

**The earning rule.** Fix it per package before work starts. Units complete for cable and welds, milestone weighting for engineering deliverables, 0/100 for anything shorter than a reporting period. Assessed percent complete is the weakest rule and always the one being argued about.

**The share of scope on level of effort.** Level of effort earns value as time passes, so a package with a large level-of-effort share will report a CPI near 1.00 whatever happens on site. When a report looks suspiciously calm, check the earning rules before you check the crew.

**The cut-off.** Progress, cost and accrual to the same date. Where invoices lag by two months and nothing is accrued, CPI describes the spring rather than the month being reported, and every forecast built on it is late as well as wrong.

There is a working rule in cost engineering, treated as a rule of thumb rather than a law, that cumulative CPI rarely improves once a package passes roughly 20% complete. Whether or not it holds on your job, the useful version is this: a forecast that assumes future performance better than past performance needs a named reason, and "the team has been briefed" is not one.

## Thresholds are what turn a number into a decision

Reporting every variance guarantees that none of them is acted on. A threshold names the size that requires a response and the person who must give it.

Two gates work better than one. A significance gate in money, so a 30% variance on a €40k package does not consume a steering group, and a materiality gate in percentage, so a 2% variance on a €18m package still gets seen.

Attach an owner and a deadline to each gate. A variance report that arrives without a named decision-maker is the definition of theatre, and the design of those gates is set out in [earned value reporting thresholds](https://projectcontrolsinstitute.org/earned-value-reporting-thresholds).

## Where to switch earned value off

On packages too small to measure without spending more than the answer is worth. On genuine support functions, where level of effort is honest and the indices are meaningless anyway.

On work with no stable baseline, such as early-stage design being re-scoped monthly, where the sensible control is a spend cap and a decision log.

Saying that out loud protects the system where it does apply. A method claimed to work everywhere is one nobody has to take seriously anywhere.

## Frequently asked questions

**Is earned value only for large contracts?**
No, but the effort has to be proportionate. A twelve-account package with units-complete rules and a monthly cut-off gives most of the value of a full system for a fraction of the administration. What does not scale down is the discipline: earning rules agreed in advance and a cut-off applied to both cost and progress.

**Our CPI is always 1.00. Is that good?**
It is usually a warning. A cost performance index that never moves normally means a large share of scope sits on level of effort, or that progress is being assessed to match spend. Check the proportion of budget carried on level of effort and the proportion on assessed percent complete before congratulating anyone.

**Does earned value replace the schedule?**
No. It measures work in money and is blind to sequence, so it cannot tell you which activities drive the finish date. Earned value tells you how much of the plan has been delivered and what it cost; the network tells you when the job ends. A practice that reports one without the other is missing half its instruments.

**Who should own the estimate at completion?**
The manager accountable for delivering the work, with the cost engineer producing and challenging the arithmetic. When the cost engineer owns the number alone, the forecast becomes an opinion the project can disown, and it will be disowned in the month it matters.

**Why does the forecast matter beyond the project?**
Because it is an accounting input. On a contract measured cost-to-cost, a movement in the estimate at completion resets the margin percentage across every pound of revenue already recognised, so a forecast produced on site lands in reported profit. That overlap between delivery and finance is what the PCI AI Project Controls Leader (PCL-AI) credential examines, across 13 domains and 61 knowledge areas.

---

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Internal links: this article should link to [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with that anchor, to [earned value reporting thresholds](https://projectcontrolsinstitute.org/earned-value-reporting-thresholds) with that anchor, and to [what is earned value management](https://projectcontrolsinstitute.org/earned-value-management) as the pillar it supports.*
