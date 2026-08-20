---
platform:      DEV Community
type:          glossary
title:         Critical path method: the algorithm and a worked example
meta:          A critical path method definition, the forward and backward pass written as an algorithm, a worked float table, and what CPM assumes on a real network.
primary_kw:    critical path method definition
secondary_kw:  forward and backward pass, total float, longest path, topological sort
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/critical-path-method
schema:        Article
word_count:    1748
hashtags:      #algorithms #python #computerscience #tutorial
ab_id:         AB-00591
---

# Critical path method: the algorithm and a worked example

The critical path method finds the longest path of dependent activities through a network and uses it to set the earliest possible finish date. Activities on that path have zero total float, so a day lost on any of them is a day lost to the project. It is two passes over a topologically sorted graph.

CPM was developed in the late 1950s by DuPont and Remington Rand for plant maintenance, at almost the same time as the US Navy's PERT technique. The arithmetic has not changed since.

## A critical path method definition you can put in a report

The critical path is the sequence of logically linked activities with the longest total duration through the network, and therefore zero total float.

Two consequences follow from that sentence. Delay on a critical activity delays the project by the same amount, and accelerating anything off the critical path buys nothing.

The second is the one ignored on site. Money spent speeding up work that has float does not move the completion date; it only moves the float.

## The algorithm

A schedule is a directed acyclic graph. Activities are nodes, relationships are edges, and the two passes are a dynamic program over a topological ordering.

```python
from collections import defaultdict, deque

def cpm(acts):                       # acts: {name: (duration, [predecessors])}
    succ, indeg = defaultdict(list), {a: 0 for a in acts}
    for a, (_, preds) in acts.items():
        for p in preds:
            succ[p].append(a)
            indeg[a] += 1

    queue, order = deque(a for a in acts if indeg[a] == 0), []
    while queue:                     # Kahn's topological sort
        a = queue.popleft()
        order.append(a)
        for s in succ[a]:
            indeg[s] -= 1
            if indeg[s] == 0:
                queue.append(s)
    if len(order) != len(acts):
        raise ValueError("network contains a loop")

    ES, EF = {}, {}
    for a in order:                  # forward pass: latest predecessor wins
        d, preds = acts[a]
        ES[a] = max((EF[p] for p in preds), default=0)
        EF[a] = ES[a] + d
    finish = max(EF.values())

    LS, LF = {}, {}
    for a in reversed(order):        # backward pass: earliest successor wins
        d, _ = acts[a]
        LF[a] = min((LS[s] for s in succ[a]), default=finish)
        LS[a] = LF[a] - d

    return finish, {a: (ES[a], EF[a], LS[a], LF[a], LS[a] - ES[a]) for a in acts}
```

It runs in O(V + E), which is why a 30,000-activity schedule recalculates in under a second.

The `raise` matters more than the rest. A cycle makes the topological sort terminate early, and that is exactly the loop error scheduling software reports before it refuses to calculate. There is no critical path in a network containing a cycle, because there is no longest finite path.

## The worked example

Seven activities on a structural package. Durations are working days and every relationship is finish-to-start.

| Activity | Duration | Predecessors |
|---|---:|---|
| A Mobilise | 5 | — |
| B Piling | 20 | A |
| C Pile caps | 12 | B |
| D Precast procurement | 30 | A |
| E Steel erection | 15 | C, D |
| F Cladding | 18 | E |
| G Commissioning | 10 | F |

**Forward pass**, left to right, taking the latest finish where paths merge. A runs 0–5. B runs 5–25. C runs 25–37. D runs 5–35. E needs both C and D, so it starts at 37 and runs to 52. F runs 52–70. G runs 70–80.

The project takes **80 working days**.

**Backward pass**, right to left from day 80, taking the earliest start where paths split. G must start by 70, F by 52, E by 37, C by 25. D must finish by 37, so it must start by 7. B by 5, A by 0.

**Total float** is late start minus early start, identical to late finish minus early finish.

| Activity | ES | EF | LS | LF | Total float |
|---|---:|---:|---:|---:|---:|
| A Mobilise | 0 | 5 | 0 | 5 | 0 |
| B Piling | 5 | 25 | 5 | 25 | 0 |
| C Pile caps | 25 | 37 | 25 | 37 | 0 |
| D Precast procurement | 5 | 35 | 7 | 37 | **2** |
| E Steel erection | 37 | 52 | 37 | 52 | 0 |
| F Cladding | 52 | 70 | 52 | 70 | 0 |
| G Commissioning | 70 | 80 | 70 | 80 | 0 |

The critical path is **A–B–C–E–F–G**, and it checks: 5 + 20 + 12 + 15 + 18 + 10 = 80.

Procurement is not critical, but it holds two days of float against a 30-day duration. That is a path to watch weekly, not one to relax about.

## What happens when the path moves

Critical paths belong to the current network. Change the network and they move.

Give precast procurement 33 days instead of 30. D now runs 5–38, E starts at 38 and runs to 53, F runs 53–71, G runs 71–81.

| Activity | Total float, D = 30 | Total float, D = 33 |
|---|---:|---:|
| A Mobilise | 0 | 0 |
| B Piling | 0 | **1** |
| C Pile caps | 0 | **1** |
| D Precast procurement | 2 | **0** |
| E, F, G | 0 | 0 |

The project takes **81 days** and the driving path is now **A–D–E–F–G**: 5 + 33 + 15 + 18 + 10 = 81. The concrete chain everybody was watching has picked up a single day of float and stopped mattering.

Three days added to one procurement duration cost one day of project and moved management attention to a different discipline. That is why the driving path is reported monthly rather than agreed once.

## Total float against longest path

Scheduling tools offer two ways of deciding what is critical, and they do not always agree.

The total float method marks activities at or below a float threshold, usually zero. The longest path method traces the chain that actually drives the finish date, working back from the end through each activity's driving relationship.

They diverge when constraints or multiple calendars are in play. A constraint can push float negative on activities driving nothing, and a mixed-calendar network can report small positive float on the true driver.

Where the two disagree, longest path is normally the honest answer, and the disagreement itself is worth investigating before the report goes out.

## CPM against the alternatives

| Technique | What it computes | What it assumes | Where it fails |
|---|---|---|---|
| Critical path method | One deterministic finish date from fixed durations and logic | Durations are known, resources available | No view of uncertainty; ignores resource limits unless levelled |
| PERT | An expected duration from optimistic, most likely and pessimistic values | A weighted distribution per activity | Understates risk where parallel paths merge |
| Critical chain | A resource-feasible chain with buffers pooled centrally | Activity estimates contain padding that can be shared | Needs behavioural discipline most organisations lack |
| Longest path (software setting) | The chain driving the finish date, traced backwards | The network is complete and the logic sound | Meaningless on a network full of open ends |
| Quantitative schedule risk analysis | A distribution of finish dates, read as P50, P80 | Ranges and correlations that came from somewhere | Poor ranges produce a confident-looking poor curve |

CPM is the base layer. The others are checks on it, and none replaces it.

## What CPM assumes

Four assumptions, each of which fails on a real project in a way worth naming.

Durations are deterministic. They are not, which is why a P80 date from a schedule risk analysis usually sits weeks after the CPM date.

Resources are unlimited. They are not, and levelling to real crew sizes typically produces a later date and a different driving path.

The logic is complete and correct. On a network with open ends and hard constraints, the calculated critical path is arithmetic performed on a wrong graph.

Work happens in the planned sequence. When it does not, the out-of-sequence setting decides the answer, and retained logic and progress override return different finish dates from the same update.

## What one day on the critical path costs

It depends on the contract, and it is almost never zero.

Take a package with time-related site costs of £9,000 a day. Ten days of critical delay is 10 × 9,000 = **£90,000**, before liquidated damages and before any acceleration.

The second effect is the one finance sees. A slip that pushes work out of the period changes costs incurred against total expected costs, which changes the measure of progress used to recognise revenue over time, which changes what is reported this period.

That crossing is the point of the discipline. An engineer is examined on float and progress measurement and almost never on cut-off; an accountant is examined on cut-off and almost never on a driving path. The PCI AI Project Controls Leader (PCL-AI) examines both, across 13 domains and 61 knowledge areas, with the Body of Knowledge proportioned 40 / 40 / 20 across finance and reporting, project management, and governed AI.

## Frequently asked questions

**Can a project have more than one critical path?**
Yes, and it is common where several parallel fronts merge into commissioning. Two paths of identical length are both critical, and the project is more fragile than a single-path schedule because delay on either one delays completion. Merging paths also make the calculated finish date less likely than the arithmetic alone suggests.

**What is the difference between the critical path and the longest path?**
The critical path is defined by float, usually zero or less. The longest path is traced back from the finish through driving relationships. They match on a clean network with one calendar and no constraints, and they separate as soon as constraints or mixed calendars distort the float calculation.

**Does the critical path include procurement and approvals?**
It should. Long-lead procurement, design approvals and permits sit on the driving path of many projects, and a schedule modelling only construction hides its real risk. If procurement is missing from the network, the first thing the network is wrong about is the finish date.

**How is negative float created?**
By a constraint or an imposed date the network cannot meet. Negative float means the calculated dates are already later than the dates the schedule has been told to achieve. It is not a warning about the future; it is a statement about the present.

**Is CPM still relevant with agile delivery?**
On work with physical dependencies it is unavoidable, because concrete cures at its own pace whatever the delivery method. On software-heavy scope, flow and throughput measures do more work. Most capital projects contain both, and the schedule still has to model the dependent parts.

---

*First published on projectcontrolsinstitute.org; the `canonical_url` on this post points there. DEV prohibits stub posts, so the complete algorithm and the worked pass are here rather than behind a link.*

*Internal links: this piece should link to [the total float definition](https://projectcontrolsinstitute.org/total-float) with the anchor "how total float is calculated and read", to [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis) with the anchor "testing the CPM date against uncertainty", to [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with that anchor, and to [what is project controls](https://projectcontrolsinstitute.org/what-is-project-controls) with the anchor "where scheduling sits in project controls".*
