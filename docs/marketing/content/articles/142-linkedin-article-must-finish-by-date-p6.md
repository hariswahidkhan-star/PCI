---
platform:      LinkedIn Article
type:          guide
title:         What a Must Finish By date in P6 does to your float
meta:          A Must Finish By date in P6 anchors the backward pass to a date you impose. Here is the arithmetic, what it breaks, and when it is defensible.
primary_kw:    must finish by date P6
secondary_kw:  negative float, total float, schedule constraints, longest path
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1521
hashtags:      #Primavera #Scheduling #ProjectControls #ProjectManagement
ab_id:         AB-00184
---

# What a Must Finish By date in P6 does to your float

A Must Finish By date in P6 is a project-level constraint that replaces the calculated late finish with a date you impose. The forward pass is untouched, so the work does not get shorter. The backward pass runs from your date instead, and every path that cannot meet it shows negative float.

Written for LinkedIn as an original. It sits under the Institute's planning and scheduling pillar.

## What does a Must Finish By date in P6 actually change?

It changes one number in the calculation: the late finish the backward pass starts from. Nothing else about the network moves.

The forward pass still adds durations along the logic and returns the earliest the work can finish. Set a Must Finish By date and P6 anchors the backward pass to your date rather than to that calculated finish, then propagates late dates backwards from it.

Total float is late finish minus early finish. Change the anchor and you change every total float value in the project by exactly the same amount.

## How does it produce negative float?

By arithmetic, not by judgement. Work in day numbers from the data date so the mechanism is visible.

Say the forward pass returns a calculated early finish of **day 268**. The contract completion date sits at **day 254**, and you set that as the Must Finish By date.

The backward pass now starts at 254 instead of 268. Total float on the driving path becomes 254 − 268 = **−14 days**, and every other path shifts by the same 14 days.

| Path | Total float before the constraint | Total float after a 14-day pull | What it now reads as |
|---|---:|---:|---|
| Structure and envelope (driving) | 0 | −14 | The path that must recover 14 days |
| Mechanical and electrical | 6 | −8 | Also late against the imposed date |
| External works | 25 | 11 | Still has genuine slack |

The ranking survives. What does not survive is the convention that zero float means critical, because after the constraint nothing sits at zero and three activities in that table are negative for two different reasons.

## What happens if the date is later than the calculated finish?

The reverse, and it is the failure mode nobody talks about. A Must Finish By date later than the calculated finish pushes the late dates outwards and manufactures float across the whole network.

Calculated finish at day 268 with a Must Finish By date of **day 290** gives every path an extra 22 days. The driving path now shows **+22 days** of total float.

Ask P6 for the critical path using the total-float-less-than-or-equal-to-zero setting and it returns nothing at all. The project appears to have no critical activities, which is never true and is entirely an artefact of the constraint.

This is why, whenever a project-level constraint is in play, criticality should be identified by longest path rather than by a float threshold. Longest path traces the driving logic and ignores the imposed anchor.

## How is it different from an activity constraint?

A Must Finish By date is a property of the project, applied once in the backward pass. Activity constraints are properties of individual activities and several of them behave very differently.

| Constraint | Where it is set | Effect on early dates | Effect on late dates | Typical float effect | When it is defensible |
|---|---|---|---|---|---|
| Must Finish By | Project dates | None | Anchors the whole backward pass | Shifts every total float value equally | Showing the gap to a contract completion date |
| Finish On or Before | Activity | None | Caps that activity's late finish | Negative float on its predecessors only | A sectional completion or an access obligation |
| Start On or After | Activity | Delays that activity's early start | None directly | Consumes float downstream | A confirmed access or permit date |
| Finish On | Activity | Fixes the finish | Fixes the finish | Both directions distorted | Rare; needs a stated reason |
| Mandatory Finish | Activity | Overrides both passes | Overrides both passes | Can break the logic silently | Almost never; it lets successors precede predecessors |
| Deadline or expected finish | Activity, in some tools | None | Reports variance only | None | Reporting a target without touching the calculation |

The practical rule is that a Must Finish By date is honest because it is visible in one place and can be removed for a clean recalculation. A scattering of activity constraints achieving the same effect is not, because no reviewer will find them all.

## What breaks when the whole network is negative?

Four things, and each one costs you somewhere different.

**Float ranking loses its meaning to readers.** People who did not set the constraint read −8 days as an activity in trouble, when it may have six days of genuine slack behind a path that is fourteen days short.

**Resource levelling behaves differently.** Levelling routines that prioritise by total float will order work against the constrained values, which is not the same order as the unconstrained network.

**Schedule risk analysis distorts.** A quantitative model run on a constrained network reports criticality indices against imposed late dates. Run the simulation on the unconstrained copy, then compare the resulting distribution of finish dates with the contract date afterwards. The setup decides the answer more than the tool does, and [how a quantitative schedule risk analysis is put together](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) is the part most teams skip.

**Delay analysis gets harder.** Float ownership arguments start from what float existed and when it was consumed. A constraint that suppressed float for six months makes that question far more expensive to answer than it needed to be.

## When is it legitimate to use one?

When you want the programme to state the gap between the work as planned and the date the contract requires. That is a real question and negative float answers it in one number.

Set it at the contract completion date, disclose it in the basis of schedule, and report the gap in days rather than hoping the reader notices the minus signs. A negative-float programme submitted without a covering explanation invites the reading that you have hidden something.

Do not use it to hold a date that the work cannot meet while presenting the programme as achievable. The constraint does not compress anything: it moves the reporting line, and the work still finishes on day 268.

## How should the gap be reported?

State the number, the driving path, and what you propose to do about it. Fourteen days negative on the structure and envelope path is a sentence, not a diagram.

Then price it. If delay damages run at **£40,000 per day**, a fourteen-day gap carries **£560,000** of exposure before prolongation costs, and that figure is what gets a recovery decision made rather than deferred.

There is a finance consequence as well. A completion date that moves changes when the asset starts earning, and where revenue is recognised over time on an input measure it changes the phasing of reported revenue and margin. A planner who can state the day-count gap and its effect on the forecast is having a different conversation from one who submits a red programme.

## Frequently asked questions

**Does a Must Finish By date change the finish date shown in P6?**
No. The scheduled finish is produced by the forward pass and reflects durations, logic and the data date. The constraint only anchors the backward pass. If your calculated finish is day 268, it stays day 268 whatever date you type into the project constraint field.

**Why does my whole project suddenly show negative float?**
Almost always a project-level Must Finish By date earlier than the calculated finish, or a Finish On or Before constraint high in the network. Check the project dates tab first, then filter for constrained activities. The size of the negative number tells you exactly how far the imposed date sits inside the calculated one.

**Should I remove the constraint before running a schedule risk analysis?**
Yes, run the model on an unconstrained copy. Simulation output is a distribution of calculated finish dates, and criticality indices should reflect driving logic rather than an imposed anchor. Compare the resulting P80 date with the contract date once the run is complete.

**Is negative float acceptable in a submitted programme?**
It is acceptable when it is deliberate, disclosed and accompanied by a recovery position. It is a problem when it appears without explanation, because the reviewer cannot tell whether it represents a genuine gap to contract or a constraint someone set and forgot. Say which in the narrative.

**What should I use instead if I only want to see a target date?**
Use a deadline or expected finish field where the tool offers one, which reports variance without touching the backward pass. Where it does not, keep the Must Finish By date but identify criticality by longest path, so the driving logic stays visible regardless of the anchor.

---

*PCI publishes certification requirements. Nothing here is legal, tax or accounting advice. All figures above are illustrative arithmetic, not project data.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Linking note: one cross-estate link now sits in the body, in the section on what breaks when the whole network is negative. The piece tells the reader to re-run the risk model on an unconstrained copy, which raises the question of how such a model is built, and the hub's quantitative schedule risk analysis guide answers it. The note originally proposed two further hub links, to total float and the critical path method. Both were dropped: only one link per domain per piece is allowed, and this article works the float arithmetic and both passes itself, so those links would have pointed at answers the reader had just been given. Nothing here raises a question the other four domains answer, so no second cross-estate link was forced in. A reciprocal link back to this piece would fit on the total float page, where negative float and imposed constraints come up.*
