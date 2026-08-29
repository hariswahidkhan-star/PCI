---
platform:      Reddit / forum — r/PrimaveraP6
type:          forum-post
title:         Querying the P6 database directly: what to know first
meta:          Durations are stored in hours, not days. Divide by 8 on a 10-hour calendar and a 1,200-day programme reports 1,500. Five traps before your first query.
primary_kw:    P6 database queries *
secondary_kw:  TASK table, total float hours, percent complete, driving path
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article + FAQPage
word_count:    1297
hashtags:      n/a (Reddit)
ab_id:         AB-01385
---

# Querying the P6 database directly: what to know first

A colleague pulled every activity duration straight out of the database, divided by 8, and produced a programme summary 300 days longer than the schedule. Nothing was wrong with his SQL. A third of the activities sat on a 10-hour calendar.

Short answer: P6 stores durations, float and lag in **hours**, converted for display using each activity's own calendar. Read the tables directly and you get the stored value, not the value on screen. That single fact accounts for most wrong answers from otherwise correct queries.

Querying P6 directly is worth doing — it is how you get history, cross-project reporting and anything the layouts cannot express. But read this first, because the schema is honest and unforgiving rather than friendly.

## The hours trap, with numbers

100 activities, 12 days each, on a 10-hour calendar. Stored duration per activity: 120 hours.

- Correct: 120 ÷ 10 = 12 days each → **1,200 days**
- Divided by a hard-coded 8: 120 ÷ 8 = 15 days each → **1,500 days**

A 25% inflation, invisible, and it survives review because the number looks plausible. Join through the activity's calendar and use that calendar's hours-per-day. Never hard-code the divisor, and never assume the project default applies to every activity.

Float carries the same trap. A stored total float of 40 hours is 5 days on an 8-hour calendar and 4 days on a 10-hour one. Two activities with identical stored float can have different float in days, which is exactly the sort of thing that turns a delay report into an argument.

Relationship lag is stored in hours too, and which calendar converts it depends on a project setting. Check that setting before you report anything about lag.

## The five things I check before trusting a query

| What you think you are reading | What the column actually holds | The fix |
|---|---|---|
| Duration in days | Duration in hours, per the activity's calendar | Join to the calendar and divide by its hours-per-day |
| Total float in days | Total float in hours | Same join, same divisor |
| "The" percent complete | Three separate fields: physical, duration-based and units-based | Pick one, record which, and check the activity's percent-complete type |
| Baseline variance | Nothing — the baseline is a separate project, not a column | Join to the baseline project and match on activity code |
| Critical path | Total float at or below a threshold, unless the project uses longest path | Use the driving-path flag when longest path is set |

## Baselines are projects

This surprises everyone once. A baseline in P6 is a copy of the project stored as its own project record, flagged as a baseline. There is no "baseline finish" column sitting next to the current finish on the activity row waiting to be subtracted.

So variance queries need the baseline project identifier, then a join on activity code between the two projects — and activity codes are not guaranteed unique across projects, so scope the join by project on both sides. Get this wrong and you compare against whichever baseline sorted first, silently, for months.

While you are there: confirm which baseline is assigned as the project baseline and which is assigned as the primary user baseline. Reports read different ones, and users have their own.

## Critical path is not always float equal to zero

On a schedule set to longest path, the critical path is the longest chain to the finish, and the tool marks it with a driving-path flag. Total float can be non-zero on those activities and zero on others.

Multiple calendars break the assumption in a different way: an activity on a 5-day calendar and one on a 7-day calendar can share stored float in hours and differ in days, which changes their apparent rank. And a "finish on or before" constraint can drive float negative on activities that are not driving anything, because a constraint is not a predecessor.

If your query returns a critical path that does not match the layout, the schedule setting is usually the reason, not the SQL.

## The tables you will actually use

`PROJECT` for the project header, including the last scheduled date, which tells you whether the data you are reading was ever recalculated. `PROJWBS` for the breakdown, holding the parent-child structure. `TASK` for activities: codes, names, type, status, the target, early, late and actual dates, remaining duration, and the float fields.

`TASKPRED` for relationships, with the relationship type and lag. `CALENDAR` for hours-per-day.

`TASKRSRC` and `RSRC` for resource assignments, budgeted and actual units and cost. `UDFVALUE` for user-defined fields, which is where most organisations hide the codes their reporting depends on.

Names shift a little between versions and between the enterprise and professional editions, so confirm against the schema documentation for your release rather than trusting a query someone posted in 2016.

## Read a replica, and never write

Two rules, and the second one is not negotiable.

Query a replica or a nightly restore, not the live database. A reporting query with a bad join on a live enterprise instance will make itself known to every user in the business at the same time.

Never write to the schema directly. Oracle's position is that direct writes are unsupported, and there is a practical reason underneath the policy: the scheduling engine maintains invariants across activities, relationships, calendars and resource assignments that your update statement will not respect.

The tool will accept the row and then behave in ways nobody can reproduce. Import through the application, or use the integration API.

## What you get that layouts cannot give you

History, mainly. Snapshot the tables each period and you can answer questions the tool cannot: which activities have been re-sequenced in the last six updates, how remaining duration on a package has trended, when a constraint appeared. That history is also the evidence base for delay analysis, and it is far easier to have kept it than to reconstruct it from monthly XER files two years later.

Cross-project reporting is the other one. Portfolio float, common resource loading and a consistent breakdown across twenty projects are straightforward in SQL and painful in the interface.

## Common follow-ups

**Can I do this with XER files instead?**
Yes, and for most reporting it is the better route: an XER export is a text file, versionable, and it is what your contract archive should hold anyway. Direct queries win when you need history the exports do not carry, or cross-project reporting.

**Why does my activity count disagree with the tool?**
Usually WBS summary rows, level-of-effort activities or milestones being counted differently, or a filter in the layout that is not in your query. Compare on activity type before you assume the data is wrong.

**Is the professional edition the same schema?**
Close enough that queries port with small changes, but the standalone edition uses a local database file and the enterprise edition sits on Oracle or SQL Server. Test on your own instance rather than assuming.

**What is the single most common mistake?**
Hard-coding 8 hours a day. Second most common: comparing against the wrong baseline. Both produce believable numbers, which is what makes them dangerous.

---

*Disclosure: I write for the Project Controls Institute. One link, at the end, and the traps above are checkable on your own instance without it: [a practical protocol for reviewing schedules with a language model](https://pciai.org/llm-schedule-review).*

*Internal links: the in-post link uses the anchor "a practical protocol for reviewing schedules with a language model". Comment replies should use [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) and [total float and free float explained](https://projectcontrolsinstitute.org/total-float) with those anchors.*
