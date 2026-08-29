---
platform:      Medium
type:          glossary
title:         Critical path method definition, worked end to end
meta:          A critical path method definition with the forward and backward pass worked in full, the float table, what CPM assumes, and what a critical day costs.
primary_kw:    critical path method definition
secondary_kw:  critical path, forward and backward pass, longest path, near-critical path
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /critical-path-method (own site #039)
schema:        Article + FAQPage
word_count:    1,795
hashtags:      #ProjectControls #Scheduling #Primavera #PMO #ProjectManagement
ab_id:         AB-00591
---

# Critical path method definition, worked end to end

The critical path method is a scheduling technique that finds the longest path of dependent activities through a network and uses it to set the earliest possible completion date. That is the critical path method definition in one sentence. Activities on that path have no spare time, so a day lost on any of them is a day lost to the project. Everything else has float.

CPM came out of the late 1950s, from DuPont and Remington Rand working on plant maintenance, at almost the same time as the US Navy's PERT technique. The arithmetic has not changed since.

## A critical path method definition you can put in a report

The critical path is the sequence of logically linked activities with the longest total duration through the network, and therefore zero total float.

Two consequences follow from that one sentence. Delay on a critical activity delays the project by the same amount, and accelerating anything off the critical path buys nothing at all.

The second consequence is the one that gets ignored on site. Money spent speeding up work that has float does not move the completion date. It only moves the float.

## How is the critical path calculated?

Take a seven-activity structural package. Durations are working days and every relationship is finish-to-start.

| Activity | Duration | Predecessors |
|---|---:|---|
| A Mobilise | 5 | — |
| B Piling | 20 | A |
| C Pile caps | 12 | B |
| D Precast procurement | 30 | A |
| E Steel erection | 15 | C, D |
| F Cladding | 18 | E |
| G Commissioning | 10 | F |

**Forward pass.** Work left to right, taking the latest finish wherever paths merge. A runs 0–5. B runs 5–25. C runs 25–37. D runs 5–35. E needs both C and D, so it starts at 37 and runs to 52. F runs 52–70. G runs 70–80.

The project takes **80 working days**.

**Backward pass.** Work right to left from day 80, taking the earliest start wherever paths split. G must start by 70. F by 52. E by 37. C by 25. D must finish by 37, so it must start by 7. B by 5. A by 0.

**Total float** is late start minus early start, which is the same as late finish minus early finish.

| Activity | ES | EF | LS | LF | Total float |
|---|---:|---:|---:|---:|---:|
| A Mobilise | 0 | 5 | 0 | 5 | 0 |
| B Piling | 5 | 25 | 5 | 25 | 0 |
| C Pile caps | 25 | 37 | 25 | 37 | 0 |
| D Precast procurement | 5 | 35 | 7 | 37 | **2** |
| E Steel erection | 37 | 52 | 37 | 52 | 0 |
| F Cladding | 52 | 70 | 52 | 70 | 0 |
| G Commissioning | 70 | 80 | 70 | 80 | 0 |

The critical path is **A–B–C–E–F–G**, and it checks out: 5 + 20 + 12 + 15 + 18 + 10 = 80.

Procurement is not critical. It also has two days of float against a 30-day duration, which makes it a path to watch weekly rather than a path to relax about.

Whether those two days are the activity's own to spend is a separate question, and it is answered by [how total float is calculated and read](https://projectcontrolsinstitute.org/total-float).

## What happens when the critical path moves?

Critical paths are not fixed. They belong to the current network, and they change when the network changes.

Let precast procurement take 33 days instead of 30. D now runs 5–38, E starts at 38 and runs to 53, F runs 53–71, and G runs 71–81.

The project takes **81 days**, and the critical path is now **A–D–E–F–G**: 5 + 33 + 15 + 18 + 10 = 81. The concrete chain everybody was watching has picked up float and stopped mattering.

A three-day change in one procurement duration cost a day of project and moved management attention to a different discipline. That is why the driving path is reported every month rather than agreed once at sanction.

## What does P6 mean by longest path?

Scheduling software offers two ways of deciding what is critical, and they do not always agree.

The total float method marks activities with float at or below a threshold, usually zero. The longest path method traces the chain that actually drives the completion date, working back from the finish through each activity's driving relationship.

They diverge as soon as constraints or multiple calendars are in play. A constraint can push float negative on activities that are driving nothing, and a mixed-calendar network can report small positive float on the true driver.

Where the two disagree, longest path is normally the honest answer, and the disagreement itself is worth an hour of investigation.

Report the near-critical band as well. Activities with a small amount of float, commonly ten days or fewer, are the ones that will be critical next month.

## How does CPM compare with the other techniques?

| Technique | What it computes | What it assumes | Where it fails |
|---|---|---|---|
| Critical path method | One deterministic finish date from fixed durations and logic | Durations are known and resources are available | No view of uncertainty; ignores resource limits unless levelled |
| PERT | An expected duration from optimistic, most likely and pessimistic estimates | A weighted distribution per activity | Understates risk where parallel paths merge |
| Critical chain | A resource-feasible chain with buffers held centrally rather than in activities | Activity estimates contain padding that can be pooled | Needs behavioural discipline most organisations do not have |
| Longest path (software setting) | The chain actually driving the finish date, traced backwards | The network is complete and the logic is sound | Meaningless on a network full of open ends |
| Quantitative schedule risk analysis | A distribution of finish dates, read as P50, P80 and so on | Ranges and correlations that came from somewhere | Poor ranges produce a confident-looking wrong curve |

CPM is the base layer. The others are checks on it, and none of them replaces it.

## What does the critical path method assume?

Four assumptions, each of which fails on a real project in a way worth naming out loud.

Durations are deterministic. They are not, which is why a P80 date from a schedule risk analysis usually lands weeks later than the CPM date.

Resources are unlimited. They are not, and levelling to real crew sizes normally produces a later date and a different driving path.

The logic is complete and correct. On a schedule carrying open ends and hard constraints, the calculated critical path is arithmetic performed on the wrong network.

Work happens in the planned sequence. When it does not, the out-of-sequence setting decides the answer, and retained logic and progress override can produce different finish dates from the same update.

## What does one day on the critical path cost?

The honest answer is that it depends on the contract. The useful answer is that it is almost never zero.

Take a package with time-related site costs of £9,000 a day. Ten days of critical delay is 10 × 9,000 = **£90,000**, before any liquidated damages and before any acceleration is priced.

The second effect is the one finance sees. A slip that moves work out of the period changes cost incurred against total expected cost, which changes the measure of progress used to recognise revenue over time, which changes what gets reported this period.

This is the overlap PCI was built around. An engineer is examined on float and progress measurement, and almost never on cut-off. An accountant is examined on cut-off, and almost never on a driving path. The critical path sits in both, and the money leaks in the gap between them.

Nothing PCI publishes is legal, tax or accounting advice; the treatment always depends on the contract.

## Where CPM is examined

The PCI AI Project Controls Leader (PCL-AI) examines 13 domains across 61 knowledge areas, with project scheduling as one of them and earned value, cost control and risk sitting next to it.

The Body of Knowledge runs in a 40 / 40 / 20 proportion of finance and reporting, project management, and governed AI. Critical path arithmetic sits in the middle block and the consequences of it sit in the first.

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**Can a project have more than one critical path?**
Yes, and it is common on schedules with several parallel work fronts merging into commissioning. Two or more paths of identical length are both critical, and the project is more fragile than a single-path schedule, because delay on any of them delays completion. Merging paths also make the finish date less likely than the arithmetic suggests.

**What is the difference between the critical path and the longest path?**
The critical path is defined by float, usually zero or less. The longest path is traced back from the finish through driving relationships. They match on a clean network with one calendar and no constraints, and they separate as soon as constraints or mixed calendars distort the float calculation.

**Does the critical path include procurement and approvals?**
It should. Long-lead procurement, design approvals and permits sit on the driving path of many projects, and a schedule that models only construction hides its real risk. If procurement is not in the network, the first thing the network is wrong about is the finish date.

**How is negative float created?**
By a constraint or an imposed date the network cannot meet. Negative float means the calculated dates are already later than the dates the schedule has been told to achieve. It is not a warning about the future; it is a statement about the present.

**Is CPM still relevant with agile delivery?**
On work with physical dependencies it is unavoidable, because concrete cures at its own pace whatever the delivery method. On software-heavy scope, flow and throughput measures do more of the work. Most capital projects contain both, and the schedule has to model the dependent parts even where the delivery approach is iterative.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so treat this republish as distribution and qualified traffic, not as a backlink.*

*Internal links: one is now placed in the body. The total float definition (projectcontrolsinstitute.org) sits on "how total float is calculated and read", immediately after the worked network shows procurement carrying two days of float — the sentence asks whose two days those are, and that page answers it. The note originally proposed three more links to the same domain (schedule risk analysis, the Primavera P6 guide, the project controls pillar); all three are dropped from this republish, because four links to one domain from one article is a link-scheme footprint rather than a service to the reader. They are the own-site original's internal links and belong there. Reciprocal: the total float page should link back here with the anchor "the forward and backward pass worked end to end", since it uses the passes without deriving them.*
