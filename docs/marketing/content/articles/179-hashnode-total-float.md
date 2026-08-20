---
platform:      Hashnode
type:          glossary
title:         Total float definition, the forward pass and the traps
meta:          A total float definition with the formula, both passes written as code, a worked eight-activity network, negative float, and how to track float erosion.
primary_kw:    total float definition
secondary_kw:  free float, interfering float, negative float, float erosion
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/total-float
schema:        Article
word_count:    1800
hashtags:      #algorithms #python #tutorial #computerscience
ab_id:         AB-00601
---

# Total float definition, the forward pass and the traps

Total float is the number of working days an activity can slip from its early dates before the project completion date or a contractual date moves. That is the total float definition; the arithmetic behind it is late start minus early start, which is the same as late finish minus early finish. Zero total float means the activity is critical.

Float is a property of a path rather than of an activity. It is the gap between two chains of work, so it moves when either chain moves, and it can vanish while nobody has used any of it.

## A total float definition, and the formulas behind it

Total float (TF) = late start − early start = late finish − early finish.

Free float (FF) = the earliest early start among the successors − the activity's early finish, adjusted for any lag.

Interfering float = TF − FF. That is the part of an activity's float that belongs to the chain rather than to the activity, and spending it takes time out of a successor's pocket.

## Both passes, as code

Float falls out of the two passes of the critical path method. The forward pass gives early dates, the backward pass gives late dates, and the subtraction is the float.

```python
from collections import defaultdict

def floats(acts, order):
    """acts: {id: (duration, [predecessors])}, finish-to-start, no lag.
       order: the activity ids in topological sequence."""
    succ = defaultdict(list)
    for a, (_, preds) in acts.items():
        for p in preds:
            succ[p].append(a)

    es, ef = {}, {}
    for a in order:                                   # forward pass
        d, preds = acts[a]
        es[a] = max((ef[p] for p in preds), default=0)
        ef[a] = es[a] + d

    finish = max(ef.values())
    ls, lf = {}, {}
    for a in reversed(order):                         # backward pass
        d, _ = acts[a]
        lf[a] = min((ls[s] for s in succ[a]), default=finish)
        ls[a] = lf[a] - d

    return {a: {"ES": es[a], "EF": ef[a], "LS": ls[a], "LF": lf[a],
                "TF": ls[a] - es[a],
                "FF": min((es[s] for s in succ[a]), default=finish) - ef[a]}
            for a in acts}
```

Two details are where hand calculations go wrong: the backward pass takes the **minimum** late start of the successors, and free float uses their **earliest** early start rather than the project finish.

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

**Forward pass.** F1 runs 0–8. F2 runs 8–22. F3a runs 8–12 and F3b runs 12–18. F4 waits for both F2 and F3b, so it runs 22–31. F5 runs 31–43, F6 runs 8–28, and F7 waits for F5 and F6, so it runs 43–48.

**Backward pass from day 48.** F7 must start by 43, F5 by 31, F4 by 22 and F2 by 8. F3b must finish by 22 so it must start by 16; F3a must finish by 16 so it must start by 12. F6 must finish by 43, so it must start by 23.

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

Read the setting-out row carefully. It carries four days of total float and none of it is free, because its successor starts the moment it finishes. Telling the joiner it has four days spare gives away the partitions team's contingency.

## What does negative float mean?

Negative float means the network already cannot meet a date it has been given. It is a statement about the present, not a warning about the future.

Let partition setting out start six days late, on day 14. F3a then runs 14–18 and F3b runs 18–24, while F3b's late finish is still 22. Total float is 22 − 24 = **−2 days**.

Follow it through. F4 now starts at 24, F5 runs 33–45 and F7 runs 45–50, so the project takes **50 days** instead of 48. The two days of negative float carried that warning a full update earlier, in a single number.

## Which kind of float are you looking at?

| Type | Formula | What it tells you | Common mistake |
|---|---|---|---|
| Total float | LS − ES | How far the activity can slip before the completion date moves | Treating it as the activity's own spare time |
| Free float | Earliest successor ES − EF | How far it can slip without disturbing anyone else | Assuming it exists; on a tight chain it is usually zero |
| Interfering float | TF − FF | The part of the float that belongs to the successors | Spending it and surprising the next trade |
| Project float | Contract date − calculated finish | Slack between the plan and the commitment | Distributing it to activities, where it disappears |
| Negative float | LS − ES, below zero | The imposed date is already unachievable | Reading it as a forecast rather than a fact |

## Why does float disappear when nobody has used it?

Three causes, and only one of them is real delay.

**The other path moved.** Float is the gap between two chains. If the driving chain lengthens, everyone else's float shrinks without anyone touching their work.

**A constraint ate it.** A "Finish On or Before" date caps the late dates on everything feeding it, so paths with genuine slack report zero float and the schedule stops being able to say which delay matters.

**The calendars are mixed.** Float is expressed in working days on the activity's own calendar, so an activity on a seven-day calendar and one on a five-day calendar can report different float for the same physical slack. Convert before comparing.

## How should float be tracked month to month?

Track float on each significant path rather than only the critical one, and read the trend rather than the value.

| Update | Float on the furniture delivery path |
|---|---:|
| Data date 1 | 15 days |
| Data date 2 | 11 days |
| Data date 3 | 6 days |
| Data date 4 | 2 days |

Thirteen days lost across three updates is about 4.3 days a month. At that rate the path goes negative before the next report, so it needs intervention now, while intervention is still cheap.

A path losing float steadily is a better early warning than a path that is already critical, because the critical path only tells you what has already happened.

## Who owns the float?

This is a contract question, answered in the contract rather than by the software. Nothing PCI publishes is legal advice.

Three approaches appear in practice: float belongs to the project and may be used by whoever needs it first; it is allocated to the contractor as part of its planning; or its use must be recorded and agreed.

What matters for the controls team is narrower. Know which clause applies before an update consumes float, because the schedule is the evidence in any later argument about who caused what.

## What is float worth in money?

Float decides whether spending money buys time. Four days bought back on a path with four days of float buys nothing; the same spend on a zero-float path buys four days of completion.

Take time-related site costs of £9,000 a day. Acceleration that removes six days from the driving path avoids 6 × 9,000 = **£54,000** of prelims, plus whatever the contract attaches to the date, while the same acceleration on the furniture path avoids nothing.

The finance consequence sits one step further on. Float erosion moves the forecast finish, and a slip that pushes work out of the period changes the measure of progress used to recognise revenue over time.

Delivery reports float and finance reports the consequence of it. They are the same event, read twice.

## Where float is examined

The PCI AI Project Controls Leader (PCL-AI) examines 13 domains across 61 knowledge areas, with project scheduling among them and cost management, earned value and risk beside it.

The Body of Knowledge runs in a 40 / 40 / 20 proportion across finance and reporting, project management, and governed AI. Float sits in the middle block and its consequences in the first, so the examination asks about both together.

## Frequently asked questions

**Is float the same as slack?**
Yes. Slack is the term used in some software and in parts of the North American literature: total slack means total float and free slack means free float. The arithmetic is identical, so use whichever word your reports already use.

**Can total float exceed the activity's duration?**
Easily, and it usually means the activity sits on a short branch feeding a long one. Very large values, such as several hundred days, normally point at a missing successor rather than genuine slack, so check the logic before believing the number.

**Should float be shown to the site team?**
Show the trend rather than the value. A trade told it has ten days spare will use them, and those ten days were never a private allowance. Reporting float erosion at path level keeps the early warning without inviting anyone to spend it.

**How much float should a schedule start with?**
Enough to cover the uncertainty in the network, which is a matter of [sizing schedule contingency with a QSRA](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) rather than a rule of thumb. Where the P80 date sits weeks beyond the deterministic finish, that gap is the contingency the programme needs. Padding durations hides the same time where nobody can manage it.

**What is the difference between float and buffer?**
Float is calculated by the network from logic and durations. A buffer is placed deliberately, usually as an activity at the end of a chain, and it is owned and released by a named person. Critical chain scheduling replaces distributed float with pooled buffers for exactly that reason.

---

*First published on projectcontrolsinstitute.org; the canonical is set through the republishing field in Draft Settings, so the definition ranks on the PCI site rather than here.*

*Internal links: one is now in the body. "Sizing schedule contingency with a QSRA" points at projectcontrolsinstitute.org/quantitative-schedule-risk-analysis, placed in the FAQ answer on how much float a schedule should start with, because that question is exactly the one the QSRA page answers and no rule of thumb does. The critical path method and Primavera P6 links proposed earlier were dropped: one link per domain per piece, and this piece already works both passes itself, so the float question does not need to be sent elsewhere. No second domain earns a link — float arithmetic raises no AI, careers, regional or verification question. Reciprocal: the QSRA page should point back here from its criticality-index section, with an anchor about a path that carries float and still drives the finish.*
