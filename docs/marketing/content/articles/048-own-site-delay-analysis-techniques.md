---
platform:      Own site — projectcontrolsinstitute.org
type:          comparison
title:         Delay analysis techniques compared: which one, and when
meta:          The four delay analysis techniques compared: impacted as-planned, time impact analysis, windows and as-built but-for, with one 41-day delay run through each.
primary_kw:    delay analysis techniques
secondary_kw:  time impact analysis, windows analysis, impacted as-planned, collapsed as-built
pillar:        Project controls fundamentals
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1779
hashtags:      n/a (own site)
ab_id:         AB-00200
---

# Delay analysis techniques compared: which one, and when

Four delay analysis techniques dominate practice. Impacted as-planned adds the events to the baseline programme. Time impact analysis inserts each event into the programme as it stood when the event arose. Windows analysis splits the job into periods and asks what drove the finish in each. As-built but-for collapses the events out of the as-built programme.

They routinely produce different answers from identical facts, which is why the choice is argued before the arithmetic is.

*Nothing here is legal advice. Which technique a particular contract or forum expects is a question for the contract and for legal advisers.*

## The four delay analysis techniques in one table

| Technique | Direction | Method | Programme used | Data you need | Main weakness |
|---|---|---|---|---|---|
| Impacted as-planned | Prospective | Additive: insert delay activities | Baseline only | The baseline and the events | Ignores everything that actually happened, including your own delays |
| Time impact analysis | Prospective | Additive: insert one event at a time | Programme updated to just before each event | Contemporaneous updates, event dates | Needs updates that most projects never kept properly |
| Windows / time slice | Retrospective | Observational: compare period by period | A series of updated programmes | Reliable updates at each window boundary | Expensive; sensitive to where the window boundaries fall |
| As-built but-for (collapsed as-built) | Retrospective | Subtractive: remove events from the as-built | As-built programme with logic applied | A defensible as-built and imposed logic | The logic is applied after the fact, so it can be built to suit |

Two axes sit under that table. Additive methods build a hypothetical programme by adding delay; subtractive methods build one by removing it. Prospective methods ask what the delay was going to do; retrospective methods ask what it did.

## The same delay, run through all four

Take an illustrative package. Baseline completion is working day 300. Actual completion was day 341, so overall delay is 41 days. The figures are illustrative and chosen to show how the methods separate.

Three events are on the table. E1 is an employer instruction in month four, assessed at 18 days of work. E2 is a subcontractor default in month six, 12 days, entirely the contractor's own. E3 is exceptionally adverse weather in month seven, 9 days, a relief event giving time but not money under this contract.

One further fact matters: when E1 arrived, the affected path still held **6 days of float**.

**Impacted as-planned.** Insert E1 into the baseline as an 18-day activity. Completion moves from day 300 to day 318, so the method returns **18 days**. It ignores the 6 days of float that existed by month four, ignores E2 entirely, and ignores the fact that the project was already running differently from the baseline.

**Time impact analysis.** Take the programme updated to just before E1 arose and insert the event there. The affected path has 6 days of float on that update, so 18 days of event consumes the float and drives **12 days** of completion delay.

Do the same for E3 in its own update and you get 9 days. E2 is the contractor's, so it is analysed and excluded from entitlement.

**Windows analysis.** Cut the job at month boundaries and ask what drove the finish in each window. Month four returns 12 days driven by E1. Month six returns 12 days driven by E2, contractor-culpable.

Month seven returns 9 days driven by E3. The remaining **8 days** are drift across the other windows with no single dominant cause. 12 + 12 + 9 + 8 = 41, which reconciles to the as-built.

**As-built but-for.** Take the as-built programme, apply logic, then remove E1. The collapsed finish falls to day 329, so E1 contributed **12 days**. Here it agrees with time impact analysis. On projects with heavy concurrency it usually does not.

| Technique | E1 entitlement | Reconciles to the 41 days? | What it says about E2 |
|---|---:|---|---|
| Impacted as-planned | 18 days | No | Nothing |
| Time impact analysis | 12 days | No | Analysed separately, excluded |
| Windows | 12 days | Yes — 12 + 12 + 9 + 8 = 41 | 12 days contractor-culpable, shown explicitly |
| As-built but-for | 12 days | Partly | Only if E2 is also collapsed out |

The spread between 18 and 12 days on one event is the entire argument. On time-related costs of £14,000 a day, six days is £84,000 before anything else is discussed.

## Why does float decide the answer?

Because a delay event that consumes float delays activities without delaying completion, and only completion delay earns an extension of time.

That makes [total float and who owns it](https://projectcontrolsinstitute.org/total-float) a commercial question dressed as a technical one. Under most standard forms, float in the programme is not owned by either party and is available to the project on a first-come basis, which means whoever consumes it first benefits from it. Some contracts say otherwise, and some say nothing, which is worse.

The practical consequence: an event assessed on the baseline, where float is at its maximum, and the same event assessed on a month-eight update, where float has been eroded, will give different answers. Neither is arithmetic error. They are answers to different questions.

## What about concurrent delay?

Concurrency is where two delays, one the employer's responsibility and one the contractor's, affect completion over the same period.

The prevailing approach on many contracts is that the contractor gets time but not money for the concurrent period: an extension of time so it is not liable for liquidated damages, and no prolongation cost because it would have been delayed anyway.

The definitional fight is what counts as concurrent. True concurrency, where both delays are on the critical path at the same time, is rare, and it turns on [how a critical path is identified](https://projectcontrolsinstitute.org/critical-path-method) in the update being argued over. What is common is sequential delay that overlaps loosely and is argued as concurrent by whichever party benefits.

Windows analysis handles concurrency better than the other three because it shows, window by window, what was driving the finish rather than asserting a single cause for the whole job.

## The records decide which technique you can run

This is the part programmes discover too late. Technique selection is limited by what was recorded contemporaneously.

Without saved monthly programme updates, time impact analysis and windows are both unavailable, because both need the programme as it stood at a point in time. What is left is impacted as-planned, which is the weakest, or an as-built reconstruction, which is expensive and contestable.

The minimum record set is short and cheap: a baseline that was accepted, a monthly update saved and never overwritten, dated correspondence for each event, site diaries and allocation sheets, and a note of the programme's critical path each month. Those updates only help if they were sound when they were saved, which is a matter of [keeping programme updates that survive challenge](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) rather than of storage.

Saving one file a month for three years costs nothing. Reconstructing thirty-six months of programme from site diaries is expensive, slow, and produces a weaker answer.

## Which technique should you choose?

| If this is true | Use | Because |
|---|---|---|
| The event is live and you need an extension now | Time impact analysis | It is prospective and answers the question the contract asks at the time |
| The job is finished and the updates were kept | Windows | It reconciles to the as-built and shows concurrency honestly |
| The job is finished and updates were not kept | As-built but-for, with caveats | It only needs a defensible as-built, though the imposed logic will be challenged |
| The claim is small and the parties want a quick view | Impacted as-planned | Cheap and fast, but expect it to be discounted if it is properly tested |
| Two parties have run different methods | Windows as the reconciler | It is the only one of the four that must add back to the actual finish |

The Society of Construction Law publishes a delay and disruption protocol that sets out common methods and the circumstances each suits; it is guidance rather than a contractual requirement, and it is worth reading in the original rather than in summary. Where a contract names a method, the contract wins.

## Where this is examined

The PCI AI Project Controls Leader (PCL-AI) examines 13 domains across 61 knowledge areas, with programme analysis, records and change sitting inside the delivery block.

The Body of Knowledge runs in a 40 / 40 / 20 proportion across finance and reporting, project management, and governed AI. Delay analysis technique sits in the middle block; the cost consequence of a 41-day overrun, and how it is reported and recognised in the period, sits in the first.

## Frequently asked questions

**Which delay analysis technique is the most reliable?**
Windows analysis, where the records support it, because it is the only common method that must reconcile to the actual completion date. That constraint makes it hard to build an answer that suits one party. Its cost is real, though: it needs a saved programme update at every window boundary and several days of analysis per window on a large job.

**What is the difference between time impact analysis and impacted as-planned?**
Both insert delay events, but into different programmes. Impacted as-planned inserts them into the original baseline, so it answers the question as if nothing else had changed since day one. Time impact analysis inserts each event into the programme as it stood immediately before that event, so it accounts for the progress, re-sequencing and float erosion that had already happened.

**Is a delay analysis a claim?**
No. A delay analysis establishes what caused the delay and how much of it. A claim adds entitlement under the contract, the notices given, and the money sought. A technically sound analysis attached to a late or absent notice can still fail, and an excellent notice attached to a weak analysis usually fails too.

**How much does float matter to an extension of time?**
It decides it. An event that consumes float without pushing completion generally earns no extension, so the same event can be worth eighteen days on the baseline and twelve on a month-eight update. This is why the date at which the analysis is anchored is argued as hard as the events themselves.

**Can you run delay analysis in Primavera P6?**
Yes, and most practitioners do. P6 holds baselines and dated updates, which is what time impact analysis and windows both require, and it will recalculate the network when events are inserted or collapsed. The software is not the difficulty; the difficulty is having kept updates that were sound at the time, because P6 will happily analyse a network full of open ends and constraints and give a confident wrong answer.

---

*Internal links now in the body, all on this domain: [total float and who owns it](https://projectcontrolsinstitute.org/total-float) sits where float ownership decides entitlement, which is the question the section raises and does not answer; [how a critical path is identified](https://projectcontrolsinstitute.org/critical-path-method) sits in the concurrency section, because concurrency turns entirely on which path was driving; and [keeping programme updates that survive challenge](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) sits with the minimum record set, where the reader learns the updates must have been sound at the time. Three same-domain links is the limit, so nothing further was added. Reciprocal worth making: the [what project controls is](https://projectcontrolsinstitute.org/what-is-project-controls) pillar should link back with the anchor "how delay is analysed after the event".*
