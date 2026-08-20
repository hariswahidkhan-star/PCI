---
platform:      LinkedIn carousel
type:          carousel
title:         The ten Primavera P6 layouts every planner should own
meta:          A float filter set to ten days returns activities with fourteen calendar days of slack next to ones with ten. Thirteen slides on P6 layouts that catch it.
primary_kw:    P6 layouts
secondary_kw:  total float, longest path, out-of-sequence progress, open ends
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo
word_count:    1,030
hashtags:      #Primavera #Scheduling #ProjectControls #PMO
ab_id:         AB-00194
---

# The ten Primavera P6 layouts every planner should own

*LinkedIn document post — 13 slides, 1080 × 1350. No link in the body; the link goes in the first comment.*

**Post caption (the first two lines carry the post):**

Your float filter says "total float ≤ 10 days".
It is returning activities with fourteen calendar days of slack alongside ones with ten, and calling them equally urgent.

Float is counted in each activity's own calendar. Thirteen slides on the layouts that catch this and nine other things.

---

**Slide 1 — A layout is a saved question**

A Primavera P6 layout is a filter, a column set, a grouping, a sort and a bar style, saved together. Each one exists to answer a specific question fast, which is why ten narrow layouts beat one wide one that answers nothing well.

**Slide 2 — Build them once, name them for the question**

Name a layout for what it asks, not for who made it. "Open ends" and "Should have started" get opened; "PLN_Layout_v3_final" does not.

**Slide 3 — The arithmetic**
Using elapsed-day numbering, activity A:

Early start = day **40**, original duration **20 days**, early finish = day **60**
The milestone it drives carries a Finish On or Before constraint at day **56**
Late finish = **56**, late start = 56 − 20 = **36**

**Total float = LS − ES = 36 − 40 = −4 days**

Now the trap. Those four days are counted in **A's own calendar**. A filter written as "total float ≤ 10 days" catches:

| Activity calendar | 10 days of float equals | Real recovery window |
|---|---|---|
| 5-day working week | 2 working weeks | **14 calendar days** |
| 7-day working week | 10 consecutive days | **10 calendar days** |

**A 40% difference in real slack, reported as the same number.** Group the layout by calendar, or the filter is lying to you.

**Slide 4 — Layout 1: longest path, not float ≤ 0**

Filter on the longest path flag rather than on total float less than or equal to zero. Where constraints or multiple calendars are in play the two sets differ, and the float-based filter can hand you a "critical path" that is really a constraint artefact.

Keep both layouts. Where they disagree, that disagreement is the finding.

**Slide 5 — Layout 2: total float banded**

Sort ascending on total float, grouped into bands: negative, 0–5, 6–20, 21–50, over 50. Add the activity calendar as a column so the bands mean something.

A schedule where 60% of activities sit above 50 days of float is not comfortable. It is under-linked.

**Slide 6 — Layout 3: open ends**

Filter on predecessor count = 0 or successor count = 0, excluding the project start and finish milestones. Every open end is an activity the backward pass could not reach properly, and it inflates float across everything behind it.

Run this before every baseline and after every import.

**Slide 7 — Layout 4: constraints and their owners**

Filter on primary constraint not equal to none, with the constraint type, constraint date and a notebook topic column showing why it was applied. Constraints are the most common cause of a schedule that will not move when it should.

If the reason column is empty, the constraint is undocumented, and undocumented constraints get inherited by the next planner as facts.

**Slide 8 — Layout 5: out-of-sequence progress**

Filter for activities with actual start dates whose predecessors are incomplete. Out-of-sequence work is not automatically wrong, but it means the logic no longer describes how the job is being built, and the forward pass is computing from a network that no longer exists.

Fix the logic or record why it stands. Do not let the count grow.

**Slide 9 — Layout 6: should have started, should have finished**

Two filters against the data date: activities with an early start before the data date and no actual start; activities with an early finish before the data date and no actual finish. This is the fastest quality check on any update.

Run it before you publish, not after somebody in the meeting spots it.

**Slide 10 — Layout 7: baseline variance**

Columns for start variance, finish variance, baseline start, baseline finish, sorted by finish variance descending, with baseline bars on. Ten seconds to see which activities moved and by how much.

Add a variance threshold filter so the layout shows movement worth explaining rather than every activity that shifted by a day.

**Slide 11 — Layout 8: resource and cost loading**

Group by WBS with budgeted units and budgeted cost totalled, filtered to activities with zero units where units are expected. Unloaded activities produce a planned value curve with holes in it, and the holes only appear when the earned value looks wrong.

**Slide 12 — Layout 9: three-week look-ahead by area**

Filter to a rolling window from the data date, grouped by area or by responsible party, sorted by early start. Bars trimmed to the window. This is the layout that goes to the site team, and it should contain nothing they cannot act on this month.

**Slide 13 — Layout 10: the field update sheet**

Columns the field can fill in and nothing else: activity ID, description, remaining duration, percent complete, actual start, actual finish, and a blank remarks column. Grouped by crew or subcontractor.

Every column you add beyond those six lowers the return rate. Progress data quality is a design problem before it is a discipline problem — and the schedule these layouts protect is the same schedule the cost report and the forecast are built on. The PCI AI Project Controls Leader (PCL-AI) examines that chain across 13 domains and 61 knowledge areas.

---

#Primavera #Scheduling #ProjectControls #PMO

**First comment:** Building a schedule that holds its dates through logic rather than constraints, and what to check before baselining: https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6

---

*Every figure above is illustrative arithmetic, not project data. Oracle Primavera P6 is named as the tool in common use; PCI claims no affiliation with or endorsement by Oracle. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and follow-up comment): [a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with the anchor "building a schedule that holds its dates", [total float](https://projectcontrolsinstitute.org/total-float) with the anchor "how float is calculated and why calendars change it", and [the critical path method](https://projectcontrolsinstitute.org/critical-path-method) with the anchor "the forward and backward pass in full".*
