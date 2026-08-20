---
platform:      Instagram / Facebook carousel
type:          carousel
title:         The critical path, drawn properly: a worked network
meta:          Eight activities, two paths, 29 days against 24. Forward pass, backward pass and every float shown. Ten slides on what the critical path really is.
primary_kw:    critical path
secondary_kw:  forward pass, total float, free float, longest path
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo
word_count:    993
hashtags:      #Scheduling #ProjectControls #Primavera #ProjectManagement #PMO #RiskManagement
ab_id:         AB-00595
---

# The critical path, drawn properly: a worked network

*Instagram and Facebook carousel — 10 slides, 1080 × 1350. Instagram captions carry no clickable link, so the link goes in the bio; on Facebook it goes in the post.*

**Caption (the first 125 characters have to earn the swipe):**

Eight activities. Two paths. One takes 29 days, the other 24. The five days between them decide everything.

The critical path is not a list of important activities and it is not whatever the software highlighted red. Ten slides: the forward pass on slide 3, the backward pass, every float calculated, and the four things that quietly break the calculation in a real schedule.

Save it for the next programme review.

---

**Slide 1 — What the critical path is**

The critical path is the longest continuous chain of dependent activities through a network. Its length is the shortest possible duration of the project.

Any activity on it has no float: delay one day and the finish date moves one day. That is the definition, and everything else is a consequence of it.

**Slide 2 — The network**

| Activity | Duration | Depends on |
|---|---:|---|
| A Site set-up | 5 | — |
| B Piling | 8 | A |
| C Temporary power | 3 | A |
| D Pile caps | 6 | B |
| E Site cabins | 4 | C |
| F Cabin fit-out | 2 | E |
| G Steel erection | 7 | D, F |
| H Cladding | 3 | G |

Counting elapsed days from time zero: an activity starting at day 0 with a duration of 5 finishes at day 5.

**Slide 3 — The forward pass**

Early start = the latest early finish of everything feeding it. Early finish = early start + duration.

| Activity | ES | Duration | EF |
|---|---:|---:|---:|
| A | 0 | 5 | 5 |
| B | 5 | 8 | 13 |
| C | 5 | 3 | 8 |
| D | 13 | 6 | 19 |
| E | 8 | 4 | 12 |
| F | 12 | 2 | 14 |
| G | max(19, 14) = **19** | 7 | 26 |
| H | 26 | 3 | **29** |

Project duration **29 days**. G is where the two paths meet, and it waits on D at 19, not on F at 14.

**Slide 4 — The backward pass**

Late finish = the earliest late start of everything that follows. Late start = late finish − duration. Start at the project finish, 29, and work back.

H: LS 26, LF 29. G: LS 19, LF 26. D: LS 13, LF 19. F: LS 17, LF 19. E: LS 13, LF 17. C: LS 10, LF 13. B: LS 5, LF 13. A: LS 0, LF 5.

**Slide 5 — Total float, and the answer**

Total float = late start − early start.

A 0 · B 0 · C **5** · D 0 · E **5** · F **5** · G 0 · H 0

Critical path: **A → B → D → G → H** = 5 + 8 + 6 + 7 + 3 = **29 days**.
The other path: A → C → E → F → G → H = 5 + 3 + 4 + 2 + 7 + 3 = **24 days**.

The difference, 29 − 24 = **5 days**, is exactly the float on C, E and F. Float is not generosity. It is the gap between two paths.

**Slide 6 — Total float and free float are different**

Free float is the delay an activity can absorb without moving any successor's early start.

C has 5 days total float and **0** free float: delay C and E moves. F has 5 days total float and **5** days free float, because G is waiting on D anyway.

Total float belongs to the chain and is spent once. Free float belongs to the activity. Confusing them is how three subcontractors each take "their" five days and the project loses fifteen.

**Slide 7 — Five days of float is not five days of comfort**

C, E and F share one pool of five days. If temporary power slips four days, cabins and fit-out have one day left between them, and the second path becomes critical at the fifth day.

Track the near-critical paths, not just the red one. A path with less float than your reporting cycle is critical in practice, whatever the software has coloured it.

**Slide 8 — What breaks the calculation**

A hard constraint on an activity stops the backward pass reflecting logic and produces negative float instead of a path. Open ends give activities float that came from nowhere. Excessive lags hide real work inside a relationship nobody can progress. Different calendars on adjacent activities make float arithmetic non-comparable across the network.

Before you read a critical path, check those four. In a schedule of any size, at least one of them is present.

**Slide 9 — Longest path is not always the critical path**

Primavera P6 can show a "longest path" and a "critical" flag, and they diverge when multiple calendars are in use, when constraints are set, or when a total float threshold has been configured.

Longest path traces driving relationships back from the project finish. Total float compares dates. When they disagree, the network needs fixing before the report goes out.

**Slide 10 — What the critical path is not**

It is not a list of important activities. It is not the highest-value work. It is not the activities the client cares about, and it is not permanent — it moves the moment progress is applied.

Reading it correctly is the base of everything downstream. The PCI AI Project Controls Leader (PCL-AI) examines network analysis and its forecasting consequences together, across 13 domains and 61 knowledge areas.

---

#Scheduling #ProjectControls #Primavera #ProjectManagement #PMO #RiskManagement

**Link (bio on Instagram, in-post on Facebook):** the critical path method worked in full, with the constraint traps — https://projectcontrolsinstitute.org/critical-path-method

---

*Every figure above is illustrative arithmetic, not project data. Oracle Primavera P6 is named as a tool in common use; PCI claims no affiliation with or endorsement by Oracle. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (bio link and first comment): [the critical path method](https://projectcontrolsinstitute.org/critical-path-method) with that anchor, [total float](https://projectcontrolsinstitute.org/total-float) with the anchor "total float against free float", and [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with that anchor.*
