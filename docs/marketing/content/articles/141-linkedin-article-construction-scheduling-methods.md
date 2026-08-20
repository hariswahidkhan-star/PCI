---
platform:      LinkedIn Article
type:          comparison
title:         Construction scheduling methods: CPM, LOB, takt, agile
meta:          Construction scheduling methods compared: critical path, line of balance, takt planning and Last Planner, with the arithmetic and where each one fails.
primary_kw:    construction scheduling methods
secondary_kw:  critical path method, line of balance, takt planning, Last Planner System
pillar:        Planning and scheduling
credential:    PML-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,744
hashtags:      #ProjectControls #Scheduling #ProjectManagement #Primavera
ab_id:         AB-00229
---

# Construction scheduling methods: CPM, LOB, takt, agile

Construction scheduling methods fall into four families. Critical path method sequences activities and calculates float. Line of balance plans repeated units by production rate. Takt planning fixes a rhythm and sizes the work to fit it. Agile methods, in construction usually Last Planner, plan commitments a few weeks out. Most large programmes need two of them.

Written for LinkedIn as an original. It sits under the Institute's planning and scheduling pillar.

## What do the four construction scheduling methods actually optimise?

They optimise different things, which is why arguing about which is best rarely goes anywhere. CPM optimises the sequence of a one-off network. Line of balance and takt optimise the flow of repeated work through space. Last Planner optimises something else again: the reliability of the promises trades make to each other in the coming weeks.

A contract usually demands one of them and a site usually runs on another. That mismatch is normal and manageable, provided you know which document governs and which one the foreman believes.

| Method | Unit of planning | What it optimises | What it needs to work | Where it fails | Contract fit |
|---|---|---|---|---|---|
| Critical path method (CPM) | Activity with duration and logic | Sequence and the completion date | Complete logic, sensible durations, a maintained data date | Repeated identical work; hides trade collisions inside summary bars | Standard on most forms; the programme the engineer reviews |
| Line of balance (LOB) | Unit, or location | Continuity of crews across units | Genuinely repeated units and a known production rate | Bespoke or highly variable units; heavy design change | Usually a supporting diagram, not the contract programme |
| Takt planning | Zone, on a fixed beat | Rhythm and predictable handover | Zones of roughly equal work content and disciplined trade sizing | Unequal zones; a trade that cannot be resized | Supporting; drives the short-term plan |
| Last Planner and agile-lean methods | Commitment for the coming weeks | Reliability of promises between trades | Trades in the room, weekly measurement, no blame | Long-lead procurement and design; anything beyond the lookahead | Never the contract programme; the delivery layer beneath it |

## How does the critical path method calculate a date?

CPM runs a forward pass for early dates, then a backward pass for late dates, and the difference between them is float. It is arithmetic, not judgement, once the logic and durations are set. Setting them is where the judgement sits, and [the eight steps behind a P6 network that holds up](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) cover that half of the work.

Take six activities in working days. A (10) leads to B (15) and C (8). B leads to E (6), C leads to D (12), and both E and D lead to F (4).

**Forward pass.** A finishes on day 10. B runs 10 to 25, C runs 10 to 18. E runs 25 to 31, D runs 18 to 30. F starts at the later of 31 and 30, so it runs 31 to 35. The project takes **35 days**.

**Backward pass.** F must finish by 35, so it must start by 31. E must finish by 31, D must finish by 31. B must finish by 25, C must finish by 19. A must finish by 10.

**Total float** is late finish minus early finish. A, B, E and F all show 0. C shows 19 − 18 = **1 day**, and D shows 31 − 30 = **1 day**. The critical path is A, B, E, F at 10 + 15 + 6 + 4 = 35 days, and the parallel route totals 34 days, which is exactly the float the calculation returned.

That is the whole mechanism. Everything else in a scheduling tool is presentation on top of those two passes.

## When does line of balance beat CPM?

When the same work repeats across many units and the real risk is crews colliding rather than logic being wrong. A 40-flat fit-out modelled as 40 copies of the same eight activities produces 320 bars nobody reads, and it still will not show you the collision.

LOB plots units against time as a rate. Suppose first fix proceeds at **2 flats per week** from week 0, so it reaches flat *n* at week *n* ÷ 2 and finishes flat 40 at week 20.

Now put second fix behind it at **4 flats per week**, starting week 5. It reaches flat *n* at 5 + *n* ÷ 4.

Set the two equal: 5 + *n* ÷ 4 = *n* ÷ 2 gives *n* = 20, at week 10. The faster trade catches the slower one at flat 20, halfway through the job.

You have two honest fixes. Slow the second trade to the same rate, or start it later. For a one-week buffer at the last flat you need start + 40 ÷ 4 ≥ 40 ÷ 2 + 1, so the second trade starts at **week 11** and finishes week 21.

CPM will not tell you this, because in CPM those crews are inside different activities that never touch.

## How is takt time calculated?

Takt time is available time divided by the number of units, and then the work is sized to fit that beat rather than the beat being sized to fit the work. This is the reversal that people find uncomfortable.

Forty flats in twenty working weeks gives a takt of 0.5 weeks per flat, which is one flat every 2.5 working days.

The programme length follows the train. With **12 zones**, **5 trades** and a takt of **5 days**, the duration is (12 + 5 − 1) × 5 = **80 days**. The first trade finishes its last zone on day 60, and the tail is the four remaining trades clearing the final zone.

The hard part is not the sum. It is resizing each trade so that all five genuinely complete a zone in five days, which usually means changing crew sizes and sometimes changing scope boundaries between packages.

## Where does agile fit on a construction programme?

Agile in construction mostly means the Last Planner System, and it operates on the coming weeks rather than the whole job. It does not replace a contract programme and should never be offered as one.

The measurement that matters is percent plan complete: tasks completed as promised divided by tasks promised. Forty-eight of sixty promises kept is **80%**, and the value is in the twelve that failed and the reasons recorded against them.

Track those reasons for eight weeks and you get something a CPM network cannot give you: evidence about why durations slip, which is the input your next set of durations should be built from.

## Which method should you use?

Use CPM for the contract programme, because that is what the forms and the engineer expect, and because entitlement arguments are settled on logic and float. Use LOB or takt underneath it wherever the work genuinely repeats.

Use Last Planner as the weekly layer in all cases, and feed its data back into durations.

The failure mode to avoid is running a CPM programme that nobody on site uses and a whiteboard plan that nobody in the contract file has seen. When those two disagree and a delay event lands, the record you rely on is the one that was never maintained.

## What does the schedule owe the accounts?

Progress measurement is not only a delivery number. Where revenue is recognised over time using an input measure such as cost incurred against total forecast cost, the progress your method produces sits inside the reported revenue.

CPM percentage complete and LOB unit counts are not the same measurement, and they will not give finance the same answer in the same month. If your commercial team bases valuations on units complete while the programme reports activity percentages, the two positions diverge quietly until the year-end audit finds them.

An engineer is examined on float and progress measurement and almost never on cut-off. An accountant is examined on when revenue may be recognised and almost never on the critical path. Scheduling method choice lands squarely in that gap, which is why the PCI Project Management Leader – AI (PML-AI) credential, at 16 domains and 63 knowledge areas, examines both sides rather than one. Planners weighing that against the credentials already on the market can read [a side-by-side comparison of the planner credentials](https://credentialfinder.org/best-certification-for-planning-engineers) before choosing.

## Frequently asked questions

**Is line of balance the same as takt planning?**
No, though they are related and often confused. Line of balance plans continuity by production rate, allowing each trade its own rate provided the lines do not cross. Takt planning imposes a single beat that every trade must meet, and resizes crews until they do. LOB describes flow; takt enforces it.

**Can you run takt planning inside Primavera P6?**
Yes, by modelling zones as a repeating chain with fixed durations equal to the takt, but the tool will not stop you breaking the beat. P6 schedules what you tell it, so the discipline stays with the planner. Most teams plan the takt separately and mirror it into the contract programme once it is stable.

**Does the critical path method still work on repetitive projects?**
It still calculates a correct date, which is why contracts keep asking for it. What it does not do is show crews colliding in the same physical space, because CPM has no concept of location. Run CPM for the contract and a location-based view for the site, from the same durations.

**Which method handles change best?**
CPM, provided the logic is maintained, because change can be inserted and the effect calculated. Rate-based methods break more visibly when a unit stops being identical, which is arguably useful information. The worst outcome is a rate plan quietly carried forward after the units diverged.

**Should the contract programme ever be a takt plan?**
Rarely. Contract programmes are assessed on logic, float and the ability to demonstrate an effect on completion, and a beat-based plan is difficult to interrogate that way. Keep the takt plan as the production layer and keep the CPM network as the contractual record.

---

*PCI publishes certification requirements. Nothing here is legal, tax or accounting advice. All figures above are illustrative arithmetic, not project data.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Linking note: two cross-estate links now sit in the body. The hub link on building a P6 network sits in the CPM section, at the sentence that says the arithmetic only holds once logic and durations are set, because setting them is exactly what that guide covers and this piece does not. The credentialfinder.org link sits in the closing section, where the piece names the credential that examines both delivery and finance and a planner reasonably asks how the existing scheduling credentials compare. The note originally proposed three hub links, to the critical path method, total float and the P6 guide. Two were dropped: only one link per domain per piece is allowed, and this article already works the critical path and total float arithmetic itself, so those two targets would have answered a question the body had already answered.*
