---
platform:      Own site — projectcontrolsinstitute.org
type:          glossary
title:         Critical path method definition, with a worked example
meta:          A critical path method definition with the forward and backward pass worked through, the float table, what CPM assumes and what critical delay costs.
primary_kw:    critical path method definition
secondary_kw:  critical path, forward and backward pass, longest path, near-critical path
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,728
hashtags:      n/a (own site)
ab_id:         AB-00591
---

# Critical path method definition, with a worked example

The critical path method definition, in one sentence: it is a scheduling technique that finds the longest path of dependent activities through a network and uses that path to set the earliest possible completion date. Activities on that path have no spare time, so a day lost on any of them is a day lost to the project. Everything else has float.

CPM was developed in the late 1950s, by DuPont and Remington Rand for plant maintenance work, at almost the same time as the US Navy's PERT technique. The arithmetic has not changed since.

## A critical path method definition you can use in a report

The critical path is the sequence of logically linked activities with the longest total duration through the network, and therefore zero total float.

Two consequences follow from that sentence. Delay on a critical activity delays the project by the same amount, and accelerating anything off the critical path buys nothing at all.

The second consequence is the one that gets ignored on site. Money spent speeding up work with float does not move the completion date; it only moves the float.

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

**Forward pass.** Work left to right, taking the latest finish where paths merge. A runs 0–5. B runs 5–25. C runs 25–37. D runs 5–35. E needs both C and D, so it starts at 37 and runs to 52. F runs 52–70. G runs 70–80.

The project takes **80 working days**.

**Backward pass.** Work right to left from day 80, taking the earliest start where paths split. G must start by 70. F by 52. E by 37. C by 25. D must finish by 37, so it must start by 7. B by 5. A by 0.

**Total float** is late start minus early start, which is the same as late finish minus early finish. [How total float is calculated and read](https://projectcontrolsinstitute.org/total-float) covers free float and the near-critical band alongside it.

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

Procurement is not critical, but it has two days of float against a 30-day duration. That is a path to watch weekly, not a path to relax about.

## What happens when the critical path moves?

Critical paths are not fixed. They belong to the current network, and they change when the network changes.

Let precast procurement take 33 days instead of 30. D now runs 5–38, E starts at 38 and runs to 53, F runs 53–71, G runs 71–81.

The project takes **81 days** and the critical path is now **A–D–E–F–G**: 5 + 33 + 15 + 18 + 10 = 81. The concrete chain that everyone was watching has picked up float and stopped mattering.

A three-day change in a procurement duration cost one day of project and moved the management attention to a different discipline. That is why the driving path is reported every month rather than agreed once.

## What does P6 mean by longest path?

Scheduling software offers two ways of deciding what is critical, and they do not always agree.

The total float method marks activities with float at or below a threshold, usually zero. The longest path method traces the chain that actually drives the completion date, working back from the finish through each activity's driving relationship.

They diverge when constraints or multiple calendars are in play. A constraint can push float negative on activities that are not driving anything, and a mixed-calendar network can report small positive float on the true driver. Where the two disagree, longest path is normally the honest answer and the disagreement itself is worth investigating.

Report the near-critical band as well. Activities with a small amount of float, commonly ten days or fewer, are the ones that will be critical next month.

## How does CPM compare with the other techniques?

| Technique | What it computes | What it assumes | Where it fails |
|---|---|---|---|
| Critical path method | One deterministic finish date from fixed durations and logic | Durations are known and resources are available | No view of uncertainty; ignores resource limits unless levelled |
| PERT | An expected duration from optimistic, most likely and pessimistic estimates | A weighted distribution per activity | Understates risk on merging parallel paths |
| Critical chain | A resource-feasible chain with buffers held centrally rather than in activities | Activity estimates contain padding that can be pooled | Needs behavioural discipline that most organisations do not have |
| Longest path (software setting) | The chain actually driving the finish date, traced backwards | The network is complete and the logic is sound | Meaningless on a network full of open ends |
| Quantitative schedule risk analysis | A distribution of finish dates, read as P50, P80 and so on | Ranges and correlations that came from somewhere | Rubbish ranges produce a confident-looking rubbish curve |

CPM is the base layer. The others are checks on it, and none of them replaces it.

## What does the critical path method assume?

Four assumptions, each of which fails on a real project in a way worth naming.

Durations are deterministic. They are not, which is why a P80 date is usually weeks later than the CPM date, and [testing the CPM date against uncertainty](https://projectcontrolsinstitute.org/schedule-risk-analysis) is a separate exercise with its own inputs.

Resources are unlimited. They are not, and levelling to real crew sizes usually produces a later date and a different driving path.

The logic is complete and correct. On a schedule with open ends and hard constraints, the calculated critical path is arithmetic performed on a wrong network, which is what [the eight steps that keep a network honest](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) exist to prevent.

Work happens in the planned sequence. When it does not, the out-of-sequence setting decides the answer, and retained logic and progress override can produce different finish dates from the same update.

## What does one day on the critical path cost?

The honest answer is that it depends on the contract, and the useful answer is that it is almost never zero.

Take a package with time-related site costs of £9,000 a day. Ten days of critical delay is 10 × 9,000 = **£90,000**, before any liquidated damages and before any acceleration.

The second effect is the one finance sees. A slip that moves work out of the period changes the cost incurred against total expected cost, which changes the measure of progress used to recognise revenue over time, which changes what is reported this period.

This is the overlap PCI was built around. An engineer is examined on float and progress measurement and almost never on cut-off. An accountant is examined on cut-off and almost never on a driving path. The critical path sits in both, and the money leaks in the gap.

## Where CPM is examined

PCL-AI examines 13 domains across 61 knowledge areas, with project scheduling as one of them and earned value, cost control and risk sitting next to it.

The Body of Knowledge runs in a 40 / 40 / 20 proportion of finance and reporting, project management, and governed AI. Critical path arithmetic sits in the middle block and the consequences of it sit in the first.

## Frequently asked questions

**Can a project have more than one critical path?**
Yes, and it is common on schedules with several parallel work fronts merging into commissioning. Two or more paths of identical length are both critical, and the project is more fragile than a single-path schedule because delay on any of them delays completion. Merging paths also make the finish date less likely than the arithmetic suggests.

**What is the difference between the critical path and the longest path?**
The critical path is defined by float, usually zero or less. The longest path is traced back from the finish through driving relationships. They match on a clean network with no constraints and one calendar, and they separate as soon as constraints or mixed calendars distort the float calculation.

**Does the critical path include procurement and approvals?**
It should. Long-lead procurement, design approvals and permits sit on the driving path of many projects, and a schedule that only models construction hides its real risk. If procurement is not in the network, the first thing the network is wrong about is the finish date.

**How is negative float created?**
By a constraint or an imposed date that the network cannot meet. Negative float means the calculated dates are already later than the dates the schedule has been told to achieve, so it is not a warning about the future; it is a statement about the present.

**Is CPM still relevant with agile delivery?**
On work with physical dependencies it is unavoidable, because concrete cures at its own pace regardless of the delivery method. On software-heavy scope, flow and throughput measures do more work. Most capital projects contain both, and the schedule has to model the dependent parts even where the delivery approach is iterative.

---

*Internal linking note: three same-domain links now sit in the body. "How total float is calculated and read" points at the total float definition, placed on the total float formula, immediately before the float table a reader has to interpret. "Testing the CPM date against uncertainty" points at the schedule risk analysis guide, placed on the deterministic-durations assumption, which is the sentence that raises the P80 date. "The eight steps that keep a network honest" points at the realistic-schedule guide, placed on the assumption that the logic is complete, because a reader who has just been told the arithmetic can be run on a wrong network will ask how to avoid building one. That anchor was written descriptively rather than reusing the target's own keyword. The fourth proposal, a link to what is project controls, was dropped to stay inside the two-to-three internal cap; it is the weakest of the four here, since this is a glossary entry rather than an orientation piece. No cross-estate link is carried. An AEO fix was also made: the opening now carries the phrase "critical path method definition" in its first line while still answering in one sentence. Reciprocal: the realistic-schedule guide and the P6 practice test already point here for the forward and backward pass.*
