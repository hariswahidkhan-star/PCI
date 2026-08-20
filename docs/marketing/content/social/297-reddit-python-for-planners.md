---
platform:      Reddit / forum — r/dataengineering
type:          forum-post
title:         Python for planners: where it saves you a day a week
meta:          6.4 hours a week saved for 35 hours of build: a 5.5 week payback. The schedule and cost tasks worth scripting, with precision and recall figures.
primary_kw:    Python for planners *
secondary_kw:  precision recall F1, schedule diff, XER parsing, earned value automation
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article + FAQPage
word_count:    1354
hashtags:      n/a (Reddit)
ab_id:         AB-01401
---

# Python for planners: where it genuinely saves a day a week

Three tasks on a construction programme took a planner 6.4 hours a week by hand. Scripted, they take about 25 minutes. The scripts took roughly 35 hours to write, so payback landed at 35 ÷ 6.4 = **5.5 weeks**.

Short answer: the wins in project controls scheduling are not modelling wins, they are integration and reconciliation wins. Schedule and cost data live in two systems that disagree about identifiers, dates and units, and a planner spends most of their week manually reconciling that. Almost all of it is deterministic transformation work.

For context: a planner maintains a network of activities with logic and calendars in Primavera P6 or Microsoft Project, and reports progress against a cost baseline every month. The data volumes are small — a large programme is a few hundred thousand rows — but the semantics are unusually nasty.

## The three tasks and what they cost

| Task | By hand | Scripted | Frequency |
|---|---:|---:|---|
| Schedule health check across 4,200 activities (open ends, negative lag, hard constraints, out-of-sequence progress, missing calendars) | 3.5 h | 40 s + ~20 min/month maintenance | monthly |
| Cost ledger to WBS mapping, 9,800 ledger lines against a 340-node breakdown | 4.0 h | ~15 min including exception review | monthly |
| Diffing 12 project files against last week's snapshot: date moves, logic changes, added and deleted activities | 5.0 h | ~12 min | weekly |

Weekly saving: 0.73 + 0.87 + 4.8 ≈ **6.4 hours**. The diff job is the one that pays, because it is weekly and because doing it by hand is genuinely unpleasant, so in practice it does not get done at all — which is worse than the time cost.

## Why the diff job matters more than it sounds

A schedule update is not a state change you can trust. Between two updates, activities are added, deleted, re-coded, re-sequenced and re-calendared, and the update narrative says "progressed as planned".

The diff you want is keyed on a stable identifier, not on activity name and not on the internal row id, which is not stable across export and reimport. Use the activity code the business assigns, keep a surrogate key table, and treat re-codes as an explicit event rather than as a delete plus an insert. If you have ever seen a programme "gain" 140 activities in a week, that is what you were looking at.

Four columns matter in the output: finish date movement, total float movement, logic changes touching the driving path, and scope added or removed. Everything else is noise on a weekly cycle.

## The awkward parts of the data

Durations and float are stored in hours, not days, and the conversion is per the activity's own calendar. Dividing everything by 8 when a third of your activities sit on a 10-hour calendar inflates the programme by 25% on those activities.

There are at least three different "percent complete" fields with different meanings — physical, duration-based, and units-based — and different reports pick different ones. Pin the definition in your pipeline and record which one you used.

A baseline is stored as a separate project, not as a column on the activity. Variance-to-baseline queries need the baseline project identifier joined in, which is where most home-grown reporting quietly compares against the wrong baseline for months.

Calendars are a first-class entity. Any date arithmetic done in pandas with plain business-day offsets will disagree with the tool, and the tool is what the contract references.

## The bit that is a classification problem

One script flags activities as "at risk of slipping" from float trend, remaining duration versus remaining work, and predecessor status. That is a classifier, so it should be measured like one.

Over a month, on 4,200 activities: the script flagged **180**. A planner reviewed all 180 and agreed **126** were genuinely at risk. A full manual review of the whole schedule found **168** genuinely at risk in total.

- Precision = 126 ÷ 180 = **0.700** — of what it flagged, 70% were real
- Recall = 126 ÷ 168 = **0.750** — of what was real, it found 75%
- F1 = 2 × (0.700 × 0.750) ÷ (0.700 + 0.750) = 1.05 ÷ 1.45 = **0.724**

Then we loosened the threshold. It flagged **260**, of which **152** were real.

- Precision = 152 ÷ 260 = **0.585**
- Recall = 152 ÷ 168 = **0.905**
- F1 = 2 × (0.585 × 0.905) ÷ 1.490 = **0.710**

F1 got worse. The tool got better, and this is the point worth arguing about here. F1 assumes a false positive and a false negative cost the same.

In planning they do not. A false positive costs about 20 seconds of a planner's attention; 108 of them is 36 minutes a month. A false negative is a slipping activity nobody looked at until it appeared on the critical path.

Tighter setting: 54 false positives (18 minutes of review) and **42 missed**. Looser setting: 108 false positives (36 minutes) and **16 missed**.

Eighteen extra minutes a month buys 26 activities you would otherwise have found late. Optimise for recall, quote precision honestly so people trust the flags, and stop reporting F1 as though it settled anything.

## Stack notes, since this is the sub for it

Land the raw export unchanged. Whatever you do downstream, keep the original file: it is the only artefact that matches what the tool produced, and on a claim it is evidence.

Put it in a real database rather than reading files repeatedly. The volumes are small; the value is having queryable history of every update, which is what makes delay analysis possible two years later.

Version the mapping table between cost ledger codes and breakdown nodes, and treat changes to it as a migration with a date. Half of all "the numbers changed and nobody knows why" incidents are an unversioned mapping table.

Never write back into the scheduling tool's database. Read a replica, produce outputs, hand them to a human. Direct writes are unsupported by the vendor, and more practically, the tool's scheduling engine holds invariants your insert will not respect.

Keep the pipeline boring: pandas, a scheduler, and tests on the transformation rules. The interesting problems here are semantic, not computational.

## What is not worth scripting

Anything requiring a judgement about whether logic reflects how the work will be built. You can detect that an activity has no predecessor. You cannot detect that a predecessor is technically present and physically absurd, and a planner who trusts a script to do that will sign a date they cannot defend.

## Common follow-ups

**Do I need a data engineer or can a planner learn this?**
A planner can learn enough in a few months, and the domain knowledge is the hard half. What planners consistently miss is versioning, testing and idempotency, which is exactly what this sub is good at teaching.

**Is the XER format documented?**
Adequately, and the schema is stable enough to build on, but column names vary by version. Write the parser defensively, assert on the columns you depend on, and fail loudly rather than silently producing a wrong date.

**Where do LLMs fit?**
Narrative drafting and first-pass review of logic changes, with the arithmetic done deterministically. Never let a model compute a float value or a forecast; use it to explain a change the pipeline has already found.

**What breaks first at scale?**
The mapping table and the identifier strategy, both social problems rather than technical ones. The compute never breaks; a programme of this size fits comfortably in memory.

---

*Disclosure: I write for the Project Controls Institute. One link, at the end, and the arithmetic above stands without it: [a practical protocol for reviewing schedules with a language model](https://pciai.org/llm-schedule-review).*

*Internal links: the in-post link uses the anchor "a practical protocol for reviewing schedules with a language model". Comment replies should use [AI in project controls](https://pciai.org/ai-in-project-controls) and [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with those anchors.*
