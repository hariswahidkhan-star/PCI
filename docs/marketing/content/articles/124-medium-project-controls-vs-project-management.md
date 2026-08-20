---
platform:      Medium
type:          comparison
title:         Project controls vs project management: decision rights
meta:          Project controls vs project management compared on decision rights, outputs and failure modes, with one month-end worked through in earned value.
primary_kw:    project controls vs project management
secondary_kw:  project controls role, project manager responsibilities, estimate at completion, independent reporting
pillar:        Project controls fundamentals
credential:    suite
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /project-controls-vs-project-management (own site #047)
schema:        Article + FAQPage
word_count:    1,924
hashtags:      #ProjectControls #ProjectManagement #EarnedValue #PMO #CostEngineering
ab_id:         AB-00028
---

# Project controls vs project management: decision rights

In project controls vs project management, the split is decision rights. Project management decides and directs: scope, the plan, the team, the commitments made to the client. Project controls measures and forecasts: the baseline, progress measurement, the cost and schedule forecast, and how much of it can be trusted.

One role owns the outcome. The other owns the number the outcome is judged by.

## Project controls vs project management, side by side

| | Project management | Project controls |
|---|---|---|
| Owns | Delivery of the scope to time, cost and quality | The integrity of the baseline and the forecast |
| Decides | What to do about a problem | What the problem currently measures |
| Core outputs | Plan, contract commitments, change decisions, team direction | Baseline, progress measurement, cost report, forecast, risk quantification |
| Typical measure of success | The project completed and accepted | The forecast was right early enough to act on |
| Reporting line | Sponsor or programme director | Usually project manager, with a dotted line to a functional controls head |
| Fails like this | Optimism that hardens into commitments the plan never supported | An accurate report that lands two weeks after the decision was taken |
| Skills that dominate | Contract, negotiation, sequencing, people | Estimating, scheduling, earned value, cost accounting, statistics |

The row that matters most is the second from the bottom. The two roles fail in opposite directions, which is exactly why capital projects run them as separate functions.

## What project controls produces every month

Project controls produces six things on a monthly cycle: an updated schedule, a progress measurement, a cost report, a forecast at completion, a risk and contingency position, and a variance narrative explaining what moved.

Each is a measurement with a method behind it. Progress is measured against rules of credit agreed at the start, not by asking a foreman for a percentage on the day of the cut-off, which is [how earned value is measured against rules of credit](https://projectcontrolsinstitute.org/earned-value-management) rather than claimed.

Project management consumes all six and converts them into decisions: accelerate, re-sequence, raise a change, escalate, or accept the position and revise the commitment.

## One month-end, in numbers

A £12,000,000 package, month nine. Controls reports the position; management decides what to do about it.

Planned value is **£4,200,000** — the budgeted cost of the work the baseline said would be complete by now. Earned value is **£3,780,000** — the budgeted cost of the work actually complete, measured against the rules of credit. Actual cost is **£4,300,000**.

Cost variance is EV − AC = 3,780,000 − 4,300,000 = **−£520,000**. Schedule variance is EV − PV = 3,780,000 − 4,200,000 = **−£420,000**.

Cost performance index is EV ÷ AC = 3,780,000 ÷ 4,300,000 = **0.879**. Schedule performance index is EV ÷ PV = 3,780,000 ÷ 4,200,000 = **0.90**.

The package is getting 87.9p of budgeted value for every pound spent, and has completed 90% of the value the baseline expected by this date. Both are measurements, and neither is a decision.

## The forecast is where the judgement sits

There is no single estimate at completion. There are four standard methods, each carrying a different assumption about what happens next, and the same three inputs give four different answers.

| Method | Formula | What it assumes | EAC |
|---|---|---|---:|
| Remaining work at budgeted rate | AC + (BAC − EV) | The overrun to date was a one-off that will not repeat | £12.52m |
| Performance to date continues | BAC ÷ CPI | Cost efficiency is stable and applies to everything left | £13.65m |
| Remaining work at the current CPI | AC + (BAC − EV) ÷ CPI | The same assumption — algebraically identical to the row above when CPI is cumulative | £13.65m |
| Cost and schedule pressure both continue | AC + (BAC − EV) ÷ (CPI × SPI) | Recovering lost time costs money, so the two indices compound | £14.69m |

The spread between the cheapest and dearest method is **£2.17m** on a £12m package, more than most of the variances anybody will argue about this month.

Controls picks the method, states the assumption in writing and defends it. Here the overrun is spread across several cost codes rather than sitting in one settled claim, so **£13.65m** — an overrun of about £1.65m — is the defensible answer.

What controls does not decide is whether 0.879 is acceptable, whether to accelerate the steel, or whether to raise a claim. It decides whether 0.879 is *true*: whether earned value is measured properly, whether actual cost includes everything committed, and whether the cut-off is clean.

Management makes the call from there. The reason to separate the two is that the person who will have to explain a £1.65m overrun should not also be the person choosing the percentage complete.

## Why do two of the four EAC formulas give the same answer?

Because two of them are one calculation in different clothes. BAC ÷ CPI and AC + (BAC − EV) ÷ CPI both reduce to AC × BAC ÷ EV whenever CPI is the cumulative index, so the two rows cannot disagree.

Check it on the figures above. BAC ÷ CPI is 12,000,000 ÷ 0.879 = **£13.65m**. AC + (BAC − EV) ÷ CPI is 4,300,000 + 8,220,000 ÷ 0.879 = 4,300,000 + 9,350,794 = **£13.65m**.

It matters in a forecast review. A pack listing both as separate methods and noting that they agree has quoted one method twice and called it corroboration. The two genuinely different assumptions are in the first and fourth rows, and the spread between those is the one worth arguing about.

## Why project controls reports independently

Because measurement that reports only to the person being measured drifts.

On most capital projects the controls lead sits inside the project team day to day, with a functional line to a head of project controls outside it. The functional line owns the method: how progress is measured, how contingency is drawn down, how a forecast is challenged.

That structure costs a little friction and buys the sponsor a number that has not been negotiated. On smaller projects a monthly forecast review chaired from outside the project achieves the same thing.

Independence is about method, not suspicion. A project manager under commercial pressure accepting an optimistic progress claim is not a character flaw; it is the predictable result of asking one person to make a commitment and then grade it.

## Where the two collide

Change is the collision point. A variation arrives and both roles have a legitimate claim on it.

A division that works: controls prices the time and cost effect and tests it against the programme, management decides whether to accept, contest or absorb it, commercial owns the contractual notice, and the sponsor approves anything that moves the sanction figure.

Baseline change follows the same logic. Controls maintains the baseline and refuses to move it quietly, management requests a re-baseline with a stated reason, and the sponsor authorises it. A baseline a project can change on its own authority has stopped being a baseline.

## Do you need both on a small project?

You need both functions. You may not need two people.

Below roughly £2m of value, or a team of ten, one competent person can plan, measure and report while the manager runs delivery — provided the method is written down and somebody outside the project reviews the forecast quarterly.

What does not work is folding controls into management and keeping no method: a schedule updated from memory, a percentage complete set by feel, and a forecast that equals the budget until the month it cannot.

## Which career is which?

| | Project controls route | Project management route |
|---|---|---|
| Entry | Planner, cost engineer, estimator, risk analyst | Site or package engineer, contract administrator |
| Middle | Lead planner or lead cost engineer | Package manager, senior project engineer |
| Senior | Project controls manager across all four disciplines | Project manager, then project director |
| What you are trusted with | The integrity of the position | The commitment to the client |

The two converge near the top. A controls manager and a project manager on the same large programme are doing recognisably similar governance work with different centres of gravity.

The move from controls into management is common, because a controls background gives a manager numbers they can defend under challenge. The move the other way needs deliberate retraining: reading a cost report is not the same skill as producing one that survives an audit.

## The part neither discipline is traditionally examined on

Project management syllabuses examine planning, stakeholders and scope. Project controls training examines schedule, cost and earned value. Neither, traditionally, examines what happens once the number leaves the project.

The £3,780,000 of earned value above was produced by a rule of credit that a planner wrote and a controls lead approved. Neither of them was examined on where that figure goes next.

It goes into the accounts. Where progress towards a performance obligation is measured on a cost-based input basis, the same measurement feeds revenue recognised in the period, so a rule of credit written to make a monthly report readable ends up standing behind a number in the financial statements.

Loose earning rules make loose accounting, and the audit conversation lands on a controls method never designed to be defended in those terms.

Nothing PCI publishes is legal, tax or accounting advice, and the treatment depends on the contract.

## How PCI examines the two sides

The PCI AI Project Controls Leader (PCL-AI) examines **13 domains across 61 knowledge areas**, covering the measurement side and its reporting consequences together.

The PCI Project Management Leader – AI (PML-AI) examines **16 domains across 63 knowledge areas**, covering the delivery and decision side of the same work.

Both Bodies of Knowledge run in a **40 / 40 / 20** proportion across finance and reporting, project management, and governed AI, which is the deliberate answer to the gap described above.

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**Is project controls part of project management?**
Organisationally, usually yes: it is a function within the project, reporting to the project manager. Methodologically it is a separate discipline with its own techniques and standards, and treating it as administrative support is the most common way a capital project loses its early warning. The distinction that matters is decision rights, not the organisation chart.

**Does a project controls manager outrank a project manager?**
No, and the question usually signals a structural problem. They are peers with different accountabilities: the project manager owns delivery, the controls manager owns the integrity of the measurement. Where controls sits under commercial pressure with no functional line out of the project, reported numbers tend to improve without the project improving.

**Which is harder to learn?**
Project controls has more technique to acquire — earned value arithmetic, network analysis, estimating, statistics, cost accounting — and it can be studied and examined. Project management has less technique and more judgement, which takes longer to build and cannot be examined as cleanly. Most people find the first hard to start and the second hard to master.

**Can one person do both on a large project?**
Not credibly beyond a certain size, because the roles conflict at exactly the moment they matter. The person carrying the commitment to the client should not also be setting the percentage complete that reports against it. On large capital work that separation is normally a client requirement rather than a preference.

**What does project controls do that a good project manager could not?**
Sustain a method under pressure. Any competent manager can build a schedule and a cost report once. Controls maintains both every month to the same standard when the project is late, the team is stretched and the news is bad — which is precisely when the numbers stop being maintained on projects without a controls function.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so this republish is here for readers rather than for link equity.*

*Internal links: one is now placed in the body. The earned value pillar (projectcontrolsinstitute.org) sits on "how earned value is measured against rules of credit", in the sentence that names rules of credit without defining them — the reader's question at that point is what a rule of credit is and who agrees it. The note also proposed the project controls definition pillar and the four EAC formulas; both are dropped from this republish. Three links to one domain is a footprint, and the EAC link would have pointed at a page answering a question this article has already answered in full in its own table, which is the weakest reason there is to place a link. Both are the own-site original's internal links. Reciprocal: the four EAC formulas page should link back here with the anchor "who chooses the EAC method, and who lives with it", since it derives the formulas without settling the decision rights.*
