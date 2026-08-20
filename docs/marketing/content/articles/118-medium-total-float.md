---
platform:      Medium
type:          glossary
title:         Total float definition: the formula and a worked network
meta:          A total float definition with the formula, a worked eight-activity network, free and interfering float, negative float, and how to track float erosion.
primary_kw:    total float definition
secondary_kw:  free float, interfering float, negative float, float erosion
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /total-float (own site #040)
schema:        Article
word_count:    1531
hashtags:      #ProjectControls #Scheduling #Primavera #PMO #RiskManagement
ab_id:         AB-00601
---

# Total float definition: the formula and a worked network

Total float is the number of working days an activity can slip from its early dates without delaying project completion or a contractual date. It is calculated as late start minus early start, which is the same as late finish minus early finish. Zero total float means the activity is critical.

Float is a property of a path rather than of an activity. It is the gap between two chains of work, so it moves when either chain moves, and it can vanish without anybody having used it.

## A total float definition, and the formulas behind it

Total float (TF) = late start − early start = late finish − early finish.

Free float (FF) = the earliest early start of the successors − the activity's early finish, adjusted for any lag.

Interfering float = total float − free float. That is the part of an activity's float belonging to the chain rather than to the activity, and spending it takes the time out of a successor's pocket.

## How is total float calculated?

Take an eight-activity fit-out. Durations are working days and every relationship is finish-to-start with no lag.

| Activity | Duration | Predecessors |
|---|---:|---|
| F1 Strip out | 8 | — |
| F2 M&E first fix | 14 | F1 |
| F3a Partition setting out | 4 | F1 |
| F3b Partitions | 6 | F3a |
| F4 Ceilings | 9 | F2, F3b |
| F5 Finishes | 12 | F4 |
| F6 Furniture delivery | 20 | F1 |
| F7 Handover | 5 | F5, F6 |

**Forward pass.** F1 runs 0–8. F2 runs 8–22. F3a runs 8–12 and F3b runs 12–18. F4 needs F2 and F3b, so it runs 22–31. F5 runs 31–43. F6 runs 8–28. F7 needs F5 and F6, so it runs 43–48.

**Backward pass from day 48.** F7 must start by 43. F5 by 31. F4 by 22. F2 by 8. F3b must finish by 22, so it must start by 16. F3a must finish by 16, so it must start by 12. F6 must finish by 43, so it must start by 23.

| Activity | ES | EF | LS | LF | Total float | Free float |
|---|---:|---:|---:|---:|---:|---:|
| F1 Strip out | 0 | 8 | 0 | 8 | 0 | 0 |
| F2 M&E first fix | 8 | 22 | 8 | 22 | 0 | 0 |
| F3a Partition setting out | 8 | 12 | 12 | 16 | **4** | **0** |
| F3b Partitions | 12 | 18 | 16 | 22 | **4** | **4** |
| F4 Ceilings | 22 | 31 | 22 | 31 | 0 | 0 |
| F5 Finishes | 31 | 43 | 31 | 43 | 0 | 0 |
| F6 Furniture delivery | 8 | 28 | 23 | 43 | **15** | **15** |
| F7 Handover | 43 | 48 | 43 | 48 | 0 | 0 |

The critical path is F1–F2–F4–F5–F7: 8 + 14 + 9 + 12 + 5 = **48 days**.

Read the setting-out row carefully. It has four days of total float and none of it is free, because its successor starts the moment it finishes.

Telling the joiner that setting out has four days spare therefore hands away the partitions team's contingency, not his own.

## What does negative float mean?

Negative float means the network already cannot meet a date it has been given. It is a statement about the present rather than a warning about the future.

Let partition setting out start six days late, on day 14. F3a runs 14–18, F3b runs 18–24, and F3b's late finish is still 22. Total float is 22 − 24 = **−2 days**.

Follow it through: F4 now starts at 24, F5 runs 33–45, F7 runs 45–50. The project takes **50 days** instead of 48, and those two days of negative float were the warning, one update earlier, in a single number.

## Which kind of float are you actually looking at?

| Type | Formula | What it tells you | Common mistake |
|---|---|---|---|
| Total float | LS − ES | How far the activity can slip before the completion date moves | Treating it as the activity's own spare time |
| Free float | Earliest successor ES − EF | How far it can slip without disturbing anyone else | Assuming it exists; on a tight chain it is usually zero |
| Interfering float | TF − FF | The part of the float that belongs to the successors | Spending it and surprising the next trade |
| Project float | Contract date − calculated finish | Slack between the plan and the commitment | Distributing it to activities, where it disappears |
| Negative float | LS − ES, where the result is below zero | The imposed date is already unachievable | Reading it as a forecast rather than a fact |

## Why does float disappear when nobody has used it?

Three causes, and only one of them is real delay.

**The other path moved.** Float is the gap between two chains. If the driving chain gets shorter, everyone else's float grows; if it gets longer, their float shrinks without anyone touching their work.

**A constraint ate it.** A "Finish On or Before" date caps the late dates on everything feeding it. Paths with genuine slack then report zero float, and the schedule loses its ability to say which delay matters.

**The calendars are mixed.** Float is expressed in working days on the relevant calendar, so an activity on a seven-day calendar and one on a five-day calendar can report different float for the same physical slack. Compare float across calendars only after converting it.

## How should float be tracked month to month?

Track the float on each significant path, not only on the critical one, and read the trend rather than the value.

| Update | Float on the furniture delivery path |
|---|---:|
| Data date 1 | 15 days |
| Data date 2 | 11 days |
| Data date 3 | 6 days |
| Data date 4 | 2 days |

Thirteen days lost across three updates is about 4.3 days a month. At that rate the path goes negative before the next report, and it needs intervention now, while intervention is still cheap.

A path losing float steadily is a better early warning than a path that is already critical, because the critical path only tells you what has already happened.

## Who owns the float?

This is a contract question, answered in the contract rather than by the software. Nothing PCI publishes is legal advice.

Three approaches appear in practice. Some contracts state that float belongs to the project and may be used by whoever needs it first. Some allocate it to the contractor as part of its planning. Some require float to be shared, with any use recorded and agreed.

What matters for the controls team is narrower. Know which clause applies before an update consumes float, because the schedule recording how float was used is the evidence in any later argument about who caused what.

## What is float worth in money?

Float decides whether spending money buys time. Four days bought back on a path with four days of float buys nothing at all; the same spend on a zero-float path buys four days of completion.

Take time-related site costs of £9,000 a day on a package. Acceleration that removes six days from the driving path avoids 6 × 9,000 = **£54,000** of prelims, plus whatever the contract attaches to the date. The same acceleration on the furniture path avoids nothing and costs whatever it costs.

The finance consequence sits one step further on. Float erosion changes the forecast finish, the forecast finish changes what has to be provided for at period end, and a slip that moves work out of the period changes the measure of progress used to recognise revenue over time.

Delivery reports float. Finance reports the consequence of it. They are the same event, described by two people who were examined on different halves of it.

## Where float is examined

The PCI AI Project Controls Leader (PCL-AI) examines 13 domains across 61 knowledge areas, with project scheduling among them and cost management, earned value and risk beside it.

The Body of Knowledge runs in a 40 / 40 / 20 proportion of finance and reporting, project management, and governed AI. Float belongs in the middle block and its consequences belong in the first, which is why the examination asks about both together.

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**Is float the same as slack?**
Yes. Slack is the term used in some software and in parts of the North American literature, and total slack means total float. Free slack means free float. The arithmetic is identical, so treat the two words as interchangeable and use whichever your organisation's reports already use.

**Can total float be larger than the activity's duration?**
Easily, and it usually means the activity sits on a short branch feeding a long one. Very large values, such as several hundred days, normally point at a missing successor rather than genuine slack. Check the logic before believing the number.

**Should float be shown to the site team?**
Show the trend, not the value. A trade told it has ten days spare will use them, and those ten days were never a private allowance. Reporting float erosion at path level keeps the early warning without inviting anyone to spend it.

**How much float should a schedule start with?**
Enough to cover the risk in the network, which is a question for a quantitative schedule risk analysis rather than a rule of thumb. Where the P80 date sits several weeks beyond the deterministic finish, that difference is the contingency the programme actually needs. Padding activities hides the same time somewhere nobody can manage it.

**What is the difference between float and buffer?**
Float is calculated by the network from logic and durations. A buffer is placed deliberately, usually as an activity at the end of a chain, and it is owned and released by a named person. Critical chain scheduling replaces distributed float with pooled buffers for exactly that reason.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so treat this republish as distribution and qualified traffic, not as a backlink.*

*Internal links: this piece should link to [the critical path method definition](https://projectcontrolsinstitute.org/critical-path-method) with the anchor "how the critical path is calculated", to [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis) with the anchor "how much float the programme actually needs", to [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with that anchor, and to [PCL-AI certification](https://projectcontrolsinstitute.org/pcl-ai-certification) with the anchor "how scheduling and cost are examined together".*
