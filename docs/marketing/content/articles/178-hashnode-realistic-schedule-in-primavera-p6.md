---
platform:      Hashnode
type:          guide
title:         A realistic schedule in Primavera P6: eight steps in code
meta:          Eight steps to a realistic schedule in Primavera P6: calendars, durations from rates, honest logic, and a quality screen you can run over the export.
primary_kw:    realistic schedule in Primavera P6
secondary_kw:  schedule quality checks, productivity-based durations, retained logic, resource levelling
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6
schema:        HowTo
word_count:    1797
hashtags:      #python #tutorial #productivity #dataanalysis
ab_id:         AB-00078
---

# A realistic schedule in Primavera P6: eight steps in code

A realistic schedule in Primavera P6 is one where every duration came from a quantity and a rate, every relationship models a physical dependency, and the finish date is calculated rather than imposed. The software does not supply any of that. Eight steps do, and the last is a mechanical screen you can run over the export in an afternoon.

The test of realism is reconstruction: someone who was not in the planning room should be able to work out why each date is what it is.

## What makes a realistic schedule in Primavera P6?

Three properties, all testable before you open the file.

| Property | The test | The common failure |
|---|---|---|
| Traceable durations | Every duration has a quantity, a rate and a source behind it | A number typed straight into the cell |
| Honest logic | Every relationship names a physical or contractual reason | Links added to make the bars line up |
| A calculated date | The finish comes from the network, not from a constraint | A target date entered and the logic bent around it |

## Step one: fix the calendars first

Calendars convert durations into dates, so an error here moves every date in the file at once.

Set the working week, the public holidays, the annual shutdown and any shift patterns before anything else. A 77-working-day activity on a five-day calendar spans about 108 calendar days; the same 77 days on a six-day calendar spans about 90.

That is 18 calendar days of difference before anyone has planned anything. Keep the calendar count low, because float is expressed on the activity's own calendar and mixed calendars make it incomparable.

## Step two: build the WBS to match how cost is controlled

The work breakdown structure is not a folder system. It is the join between the programme and the cost report.

If cost is controlled at control-account level and the schedule is coded by area, nobody can explain why the schedule shows 62% and the cost report shows 47%. Build the structure so a control account maps to a set of activities without a translation table in between.

Use activity codes for every other slice; codes give views without breaking the structure that carries the money.

## Step three: derive durations from quantity and productivity

This is the step that separates a plan from a wish, and it is arithmetic rather than opinion.

Take a pipeline of 18.4 km where one spread lays 240 m a day. The duration is 18,400 ÷ 240 = 76.7, rounded to **77 working days**.

Now apply the conditions. Winter working on that section runs at about 80% of the rate, so 240 × 0.8 = 192 m a day and 18,400 ÷ 192 = 95.8, rounded to **96 working days**.

The seasonal assumption alone is worth 19 days, invisible in a duration typed straight into the cell. Record the quantity, the rate and its source as activity notebook text, so the month-nine argument is about the rate rather than about anyone's memory.

## Step four: build logic a reviewer can follow

Every relationship should describe a physical or contractual reason one thing follows another.

Prefer finish-to-start. Where activities genuinely overlap, use start-to-start with a positive lag and pair it with a finish-to-finish link, because a start-to-start link on its own leaves the successor with no logical end.

Never use a negative lag. A lead says the successor begins before the predecessor finishes, which cannot be resourced and breaks silently whenever the predecessor's duration changes.

## Step five: use constraints as rarely as you can

A constraint overrides the network's own arithmetic, so each one is a small region of the schedule that no longer calculates.

The usual damage comes from a "Finish On or Before" date, which caps the late dates on everything feeding it, so paths with genuine slack report zero float and the schedule loses the ability to say which delay matters.

Where a date is contractual, model it as a milestone with a constraint and record the reason in the basis of schedule. Where it is a preference, take it out and let the logic produce the date.

## Step six: resource-load, then read the histogram

Durations derived from rates assume the resource turns up. Loading the schedule tests that assumption.

Take three parallel activities each needing 8 fitters when only 16 exist. Unlevelled, the network finishes in 43 days; levelled to the real crew, one activity waits and the finish moves to **52 days**.

Levelling changes the driving path, so run [the forward and backward pass that calculate the critical path](https://projectcontrolsinstitute.org/critical-path-method) again afterwards. A resource-limited finish and a logic-driven finish answer different questions, and the report has to say which one it shows.

## Step seven: baseline, then write down the progress rules

Baseline before the first update, and write the progress rules down at the same time.

Set the data date discipline: nothing remaining to the left of it, no actual progress to the right. Decide the percent complete type per activity, and use physical percent complete against rules of credit where the activity earns value.

Declare the out-of-sequence setting. Retained logic holds the unfinished part of an activity behind its incomplete predecessors and progress override releases it, so the same update gives different finish dates under the two settings.

Rules of credit are the quiet one. If steel erection earns 20% at delivery, 60% at erection and 20% at bolt-up, everyone reports the same number for the same work and progress stops being a negotiation.

## Step eight: screen it before you issue it

Run a mechanical screen first, because it finds the defects that make every other number untrustworthy. The DCMA 14-point schedule assessment, developed for US defence programmes and widely reused, is the usual reference; the checks below are described in PCI's own words.

Export activities and relationships, then screen them outside the tool so the result is reproducible between updates.

```python
HARD = {"MSO", "MFO"}          # start-on and finish-on constraints
HIGH_FLOAT = LONG_DURATION = 44   # working days

def screen(acts, rels):
    ids       = {a["id"] for a in acts}
    has_pred  = {r["succ"] for r in rels}
    has_succ  = {r["pred"] for r in rels}
    n = len(acts)
    pct = lambda rows: round(100 * len(rows) / n, 1)
    return {
        "open_ends_pct":  pct([i for i in ids
                               if i not in has_pred or i not in has_succ]),
        "leads":          len([r for r in rels if r["lag"] < 0]),
        "lags_pct":       pct([r for r in rels if r["lag"] > 0]),
        "fs_share_pct":   round(100 * len([r for r in rels
                                           if r["type"] == "FS"]) / len(rels), 1),
        "hard_const_pct": pct([a for a in acts
                               if a.get("constraint") in HARD]),
        "high_float_pct": pct([a for a in acts
                               if a["total_float"] > HIGH_FLOAT]),
        "negative_float": len([a for a in acts if a["total_float"] < 0]),
        "long_dur_pct":   pct([a for a in acts
                               if a["remaining_duration"] > LONG_DURATION]),
    }
```

| Check | Common threshold | What a failure usually means |
|---|---|---|
| Missing predecessor or successor | 5% or fewer | Parts of the network are detached and their float is meaningless |
| Leads (negative lags) | none | The overlap is hidden and cannot be resourced |
| Lags | 5% or fewer | Duration is stored in the relationship instead of in an activity |
| Relationship types | 90% or more finish-to-start | The network was drawn to make the bars look right |
| Hard constraints | 5% or fewer | The schedule is being told its answer rather than calculating it |
| Total float above 44 days | 5% or fewer | Missing successors rather than genuine slack |
| Negative float | none | The programme already cannot meet a date it is committed to |
| Durations above 44 days | 5% or fewer | Activities too coarse to measure progress against |

Treat the thresholds as a screen rather than a verdict. A file can pass every check and still be wrong, because none of them knows whether the productivity rate was achievable.

Keep the screen output in version control beside the update. A diff across three months shows defects being introduced, which beats a single pass or fail.

## What the schedule does to cash

A programme is a cash instrument, and this is the part of the overlap finance sees before delivery does.

The cash conversion cycle measures how long money is tied up between paying for work and being paid for it. It is days sales outstanding, plus days of unbilled work in progress, minus days payable outstanding.

Take DSO of 68 days, unbilled work in progress of 41 days and DPO of 55 days: 68 + 41 − 55 = **54 days** of working capital the business is funding.

Now slip the programme by three weeks so a £2.1m application misses its cut-off and lands in the next cycle. At an 8% cost of funds, 2,100,000 × 0.08 × 30 ÷ 365 = **£13,808** for one application in one month.

Dates set when applications can be made, and applications set when cash arrives. Treatment depends on the contract, and nothing PCI publishes is accounting or tax advice.

## Where this sits in the PCI curriculum

The PCI AI Project Controls Leader (PCL-AI) examines 13 domains across 61 knowledge areas, with project scheduling as one domain among them.

The Body of Knowledge runs in a 40 / 40 / 20 proportion across finance and reporting, project management, and governed AI, so scheduling is examined beside cost management, earned value and risk rather than in its own compartment. That is why the cash section above belongs in a scheduling guide.

## Frequently asked questions

**How detailed should activities be?**
Detailed enough to measure progress against and no finer. A working rule is that an activity should not run longer than the reporting cycle plus its own float, so a monthly report wants most activities under about 44 working days. Fine detail early produces a network that is expensive to maintain and no more accurate.

**Should the schedule be resource-loaded?**
Load it where resource availability genuinely constrains the work, which on most projects means the trades rather than management. Loading every activity produces a heavy file that rarely improves the date. Partial loading of the constraining resources gives most of the benefit for a fraction of the maintenance.

**How often should the baseline change?**
Only when scope, funding or contractual dates change, and always with a recorded reason. Rebaselining because the current programme is uncomfortable destroys the only reference you have for measuring variance. Keep the original baseline visible even after a change is approved.

**What is a basis of schedule and do I need one?**
It is the short document recording calendars, productivity rates and their sources, key assumptions, constraints and their justification, and the progress rules. You need one, because it turns a file into something a third party can audit. Two or three pages is usually enough.

**Can AI build the schedule?**
It can draft logic from a scope document, find missing successors and compare durations against rates achieved on similar work, which is [as far as AI gets on a construction schedule](https://pciai.org/ai-for-construction-scheduling) today. It should not issue a date you cannot explain. Every output still needs the quantity, the rate and the assumption written next to it, because that is what you will be asked for.

---

*First published on projectcontrolsinstitute.org; this version is marked as republished in Draft Settings with the canonical set to the original.*

*Internal links: two are now in the body. "The forward and backward pass that calculate the critical path" points at projectcontrolsinstitute.org/critical-path-method, placed in step six, because levelling moves the driving path and that sentence raises how the path is recalculated. "As far as AI gets on a construction schedule" points at pciai.org/ai-for-construction-scheduling, placed in the FAQ answer on whether AI can build the schedule, because that answer raises where the tooling actually stops. The total float and QSRA links proposed earlier were dropped to hold one link per domain; the critical path sentence was the strongest of the three. Reciprocal: the critical path method page should point back here for schedule quality screening, with an anchor about the checks run over a P6 export.*
