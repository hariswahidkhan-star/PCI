---
platform:      Quora
type:          qa-list
title:         What is a recovery schedule, and when is one demanded?
meta:          What is a recovery schedule? A re-plan of the remaining work to hold the contract date. Worked SPI, run-rate and crash-cost arithmetic on what it costs.
primary_kw:    what is a recovery schedule
secondary_kw:  schedule performance index, acceleration, crashing, liquidated damages
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1644
hashtags:      n/a (Quora)
ab_id:         AB-00289
---

# What is a recovery schedule, and when is one demanded?

A recovery schedule is a re-plan of the remaining work, prepared when actual progress has fallen behind the baseline, showing how the job will still meet the contractual completion date. It does not move the baseline and it does not move the date. It commits to a different way of doing what is left.

Most standard forms give the client the right to demand one once progress falls behind. That right is usually silent about who pays for the measures it contains, which is where the argument starts.

## What is a recovery schedule required to show?

Four things, and a document missing any of them will be rejected by a competent client. The current status against the baseline, the cause of the slippage, the specific measures that close the gap, and the revised logic and resources that deliver those measures.

Measures have to be nameable. "Increase productivity" is not a measure; "add a second piling rig from week 44 and move to a six-day week on the frame" is.

The revised network has to be a real network. If the recovery is achieved by shortening durations in the software without adding resource, changing sequence or reducing scope, it is a spreadsheet exercise with the evidence removed.

## How do you size the gap before writing one?

Take a job with a budget at completion of £20.00m and a planned duration of 100 weeks. At the week 40 cut-off, planned value is £7.60m, earned value is £6.40m and actual cost is £7.10m.

SPI = EV ÷ PV = 6.40 ÷ 7.60 = **0.842**. CPI = EV ÷ AC = 6.40 ÷ 7.10 = **0.901**.

Now convert that into a rate. Achieved output is 6.40 ÷ 40 = £0.160m of earned value per week. Remaining work is 20.00 − 6.40 = £13.60m over the remaining 60 weeks, which needs 13.60 ÷ 60 = £0.227m per week.

The required uplift is 0.227 ÷ 0.160 = 1.417. The recovery schedule has to explain where **42% more output per week** comes from, and hold it for sixty weeks.

That single ratio kills most recovery schedules before they are written. A team that has averaged £160,000 a week for forty weeks does not reach £227,000 a week by rescheduling.

A crude time forecast makes the same point: 100 ÷ 0.842 = 118.8 weeks, about nineteen weeks late. Treat that as an indicator only. SPI drifts back towards 1.0 as a job finishes whether or not it is late, so it flatters late projects, and the honest answer comes from re-running the critical path, not from a ratio.

## Recovery schedule, re-baseline, acceleration or extension of time?

| | What changes | Completion date | Who normally pays | Contractual effect |
|---|---|---|---|---|
| Recovery schedule | The plan for the remaining work | Unchanged | Contractor, if the delay is its own risk | A commitment, not a variation |
| Revised baseline | The baseline itself | Usually unchanged | n/a — it is a measurement change | Past variance is erased; do it rarely |
| Acceleration (instructed) | Resources and sequence | Brought forward | Client, as an instructed change | Priced as a variation |
| Extension of time | The contractual completion date | Moves later | Depends on the event and clause | Relieves liquidated damages |

The second row is the dangerous one. Re-baselining to make a variance disappear turns the whole earned value system into a record of decisions rather than a record of performance.

The difference between rows one and three is the money. A recovery schedule responding to contractor-caused delay is at the contractor's cost. The same measures instructed by the client to beat a date that has already been extended are acceleration, and acceleration is priced.

## How do you compress the remaining work?

Only critical path activities can shorten the job. Money spent compressing an activity with float buys nothing.

Crashing means paying for a shorter duration. Take an activity with a normal duration of 20 days at a cost of £200,000, which can be delivered in 15 days for £275,000 with a second crew and overtime.

The cost of the saving is 275,000 − 200,000 = £75,000 for 5 days, or **£15,000 per day saved**. Compare that against the liquidated damages it avoids. At £12,000 per day, crashing loses £3,000 for every day it buys.

Compression also moves the critical path. If the second-longest path sits 7 days behind the critical one, compressing the critical path by more than 7 days makes that second path critical, and further spending on the first path buys nothing at all. Those seven days are [total float, measured against the project rather than the activity](https://projectcontrolsinstitute.org/total-float), and the figure decides where compression stops paying.

Fast tracking — overlapping activities that were planned in sequence — costs less in cash and more in risk, because it usually means starting work on information that is not yet final. That rework risk belongs in the recovery schedule as a named assumption, not as optimism buried in a duration.

## What makes a recovery schedule worthless?

Deleting logic links so the software reports an earlier finish. Converting finish-to-start links to start-to-start with long leads that no crew could actually work to. Reducing durations of future activities that were already tight, on the argument that the team will be more efficient later.

A recovery schedule should be readable as a resource argument. If the histogram does not change, the sequence does not change and the scope does not change, then nothing has changed except the dates.

The most useful review question a client can ask is simple: which activities got shorter, by how many days, and what physically causes each one to take less time. Three or four answers of "improved productivity" is a rejection.

## Who pays for recovery, and where does it land in the accounts?

Acceleration cost is real cost, and it lands somewhere. If the delay is the contractor's risk, the cost sits in the estimate at completion and comes out of margin.

Run it through. On the numbers above, EAC = BAC ÷ CPI = 20.00 ÷ 0.901 = **£22.20m** before any recovery measures. Add £1.50m of acceleration and the forecast becomes £23.70m against a £20.00m budget.

Against that, nineteen weeks late at liquidated damages of £60,000 per week is 19 × 60,000 = £1.14m of exposure. Spending £1.50m to avoid £1.14m is a decision, not an obvious one, and it is a decision that belongs to the commercial team as much as to the planner.

There is a reporting consequence too. Where progress towards a performance obligation is measured by a cost-based input method, the measure is costs incurred divided by total expected costs — so a £1.50m increase in expected costs reduces the reported percentage complete even though the physical work is unchanged, and revenue recognised to date can go backwards.

That is why PCI examines the schedule and the ledger together rather than separately. The PCI AI Project Controls Leader (PCL-AI) credential covers 13 domains and 61 knowledge areas. Its Body of Knowledge is proportioned 40% finance and reporting, 40% project management and 20% governed AI; those are the syllabus's proportions, not an examination weighting, and no exam blueprint is published. Treatment depends on the contract and the reporting framework applied, and nothing PCI publishes is accounting or legal advice.

## Frequently asked questions

**Is a recovery schedule the same as a revised baseline?**
No, and conflating them is the most common error. A recovery schedule re-plans the remaining work while leaving the baseline intact, so variance against the original plan stays visible. A revised baseline changes the measuring stick, which erases the history of the slippage and should only happen through formal change control with a documented reason.

**How long should a recovery schedule take to produce?**
Contracts commonly allow seven to fourteen days from the client's request, and the period is usually stated. The realistic constraint is not drafting time but agreement time: the measures need commitment from the people who will resource them, and a recovery schedule the site team has not signed up to will not be delivered whatever the network says.

**Does submitting a recovery schedule admit liability for the delay?**
It should not, and most contractors submit under a reservation of rights, stating that the schedule is provided as required by the contract and without prejudice to any entitlement to an extension of time. How that reservation operates depends on the contract and the jurisdiction, and this is a point to take legal advice on rather than a point to settle from a template.

**What if the delay is the client's fault?**
Then the correct response is usually a notice and an extension of time claim, not silent recovery. Recovering from a client-caused delay at your own cost, without notice, weakens the claim later — the records will show the date was met, and the cost of meeting it will be hard to attribute.

**How is recovery progress then tracked?**
Track against the baseline for variance and against the recovery schedule for the commitment, and report both. A single line showing planned recovery output against achieved output each week is worth more than a re-issued Gantt chart, because it shows within three or four weeks whether the recovery is real.

**Can a recovery schedule ever recover a job?**
Yes, early, and rarely late. A gap identified at 15% complete has most of the work left to absorb it. The same percentage gap at 70% complete has to be absorbed by the remaining 30%, which is usually the commissioning and interface work with the least room in it.

---

*Internal links: one, in the body. [Total float, measured against the project rather than the activity](https://projectcontrolsinstitute.org/total-float) sits in the compression paragraph, where a reader told that seven days of slack on the second path caps what crashing can buy asks what that number is and how it is measured. The delay analysis page in the original note was dropped rather than placed here: it would have been a second link to the same domain, and attribution of delay is the subject of the extension of time answer, which carries that link instead. No reciprocal link is proposed: Quora links are nofollow, so this is qualified traffic rather than a backlink.*
