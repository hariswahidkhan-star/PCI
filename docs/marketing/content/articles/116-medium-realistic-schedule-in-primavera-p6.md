---
platform:      Medium
type:          guide
title:         A realistic schedule in Primavera P6: the eight steps
meta:          The eight steps to a realistic schedule in Primavera P6: calendars first, a WBS that matches cost, durations from rates, honest logic and a real screen.
primary_kw:    realistic schedule in Primavera P6
secondary_kw:  schedule quality checks, productivity-based durations, resource levelling, retained logic
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /realistic-schedule-in-primavera-p6 (own site #038)
schema:        HowTo
word_count:    1795
hashtags:      #ProjectControls #Scheduling #Primavera #PMO #ProjectManagement
ab_id:         AB-00078
---

# A realistic schedule in Primavera P6: the eight steps

A realistic schedule in Primavera P6 is one where every duration came from a rate, every relationship models a physical dependency, and the finish date survives a challenge. The software does not supply any of that. Eight steps do, and the order matters, because skipping one distorts the next.

Realism has a test attached. Someone who was not in the planning room should be able to reconstruct why each date is what it is, from the file alone.

## What makes a realistic schedule in Primavera P6?

Three properties, all testable before you open the file.

| Property | The test | The common failure |
|---|---|---|
| Traceable durations | Every duration has a quantity, a rate and a source behind it | A number typed straight into the cell |
| Honest logic | Every relationship names a physical or contractual reason | Links added to make the bars line up |
| A calculated date | The finish comes from the network, not from a constraint | A target date entered and the logic bent around it |

## Step one: fix the calendars before anything else

Calendars convert durations into dates, so a mistake here moves every date in the file at once.

Set the working week, public holidays, the annual shutdown and any shift patterns before you type a single duration. A 77-working-day activity on a five-day calendar spans about 108 calendar days; the same 77 days on a six-day calendar spans about 90. That is 18 calendar days of difference before anyone has planned anything.

Keep the calendars few, and name them for what they represent. Three or four are manageable. Twenty produce a float calculation nobody in the room can explain.

## Step two: build the WBS to match how cost is controlled

The work breakdown structure is not a folder system. It is the join between the programme and the cost report.

If cost is controlled at control-account level and the schedule is coded by area, nobody can explain why the programme shows 62% and the cost report shows 47%. Build the WBS so a control account maps to a set of activities without a translation table in between.

Use activity codes for everything else you slice by: discipline, subcontractor, area, phase. Codes give you the views without breaking the structure that carries the money.

## Step three: derive durations from quantity and productivity

This is the step that separates a plan from a wish, and it is arithmetic rather than opinion.

Take a pipeline of 18.4 km. One spread lays 240 m a day, so the duration is 18,400 ÷ 240 = 76.7, rounded to **77 working days**.

Now apply the conditions. Winter working on that section runs at about 80% of the rate: 240 × 0.8 = 192 m a day, so 18,400 ÷ 192 = 95.8, rounded to **96 working days**.

The seasonal assumption alone is worth 19 days. It is also exactly the sort of thing that becomes invisible the moment a duration is typed straight into the cell.

Record the quantity, the rate and its source against the activity. When the duration is challenged in month nine, the argument should be about the rate, not your memory.

## Step four: build logic a reviewer can follow

Every relationship should describe a physical or contractual reason that one thing follows another.

Prefer finish-to-start. Use start-to-start with a positive lag where activities genuinely overlap, and pair it with a finish-to-finish link so the successor cannot end before its predecessor does. A start-to-start link with no matching finish link leaves the activity with no logical end at all.

Never use a negative lag. A lead on a finish-to-start relationship says the successor begins before the predecessor finishes, which cannot be resourced and breaks the moment the predecessor's duration changes.

Open ends are the other standard defect. Only the first activity should have no predecessor and only the last no successor. Anything else is detached from the network, and its float is fiction.

## Step five: use constraints as rarely as you can

A constraint overrides the network's own arithmetic. Each one is a small piece of the schedule that no longer calculates.

The usual damage comes from a "Finish On or Before" date, which caps the late dates on everything feeding it. Paths that had genuine slack start reporting zero float, and the schedule loses its ability to tell you which delay actually matters.

Where a date is contractual, model it as a milestone with a constraint and say so in the basis of schedule. Where it is a preference, take it out and let the logic produce the date.

## Step six: resource-load, then look at the histogram

Durations built from rates assume the resource is there. Loading the schedule is how you test that assumption rather than inherit it.

Take three parallel activities each needing 8 fitters, on a project where 16 fitters exist. Unlevelled, the network finishes in 43 days. Levelled to the real crew, one activity waits and the finish moves to **52 days**.

Levelling changes the driving path, so recheck the critical path afterwards. A resource-limited finish and a logic-driven finish are different answers to different questions, and the report should name which one it is showing.

## Step seven: baseline, then set the progress rules

Baseline before the first update, and write the progress rules down before the first update as well.

Hold the data date discipline: nothing remaining to the left of it, no actual progress to the right. Decide the percent complete type per activity, and use physical percent complete against rules of credit wherever the activity earns value.

Decide the out-of-sequence setting and declare it. Retained logic holds the unfinished part of an activity behind its incomplete predecessors; progress override releases it. The same update run under both settings can produce different finish dates, so the narrative has to name the setting it used.

Rules of credit are the quiet one. If steel erection earns 20% at delivery, 60% at erection and 20% at bolt-up, everyone reports the same number for the same work, and progress stops being a negotiation.

## Step eight: screen it before you issue it

Run a mechanical screen first, because it finds the defects that make every other number in the file untrustworthy. The DCMA 14-point schedule assessment, developed for US defence programmes and widely reused since, is the common reference; the checks below are described in PCI's own words.

| Check | Common threshold | What a failure usually means |
|---|---|---|
| Activities missing predecessor or successor | 5% or fewer | Parts of the network are detached and their float is meaningless |
| Leads (negative lags) | none | The overlap is hidden and cannot be resourced |
| Lags | 5% or fewer | Duration is being stored in the relationship instead of in an activity |
| Relationship types | 90% or more finish-to-start | The network has been drawn to make the bars look right |
| Hard constraints | 5% or fewer | The schedule is being told its answer rather than calculating it |
| High total float, above 44 working days | 5% or fewer | Missing successors, not genuine slack |
| Negative float | none | The programme already cannot meet a date it is committed to |
| Long durations, above 44 working days | 5% or fewer | Activities too coarse to measure progress against |

Treat the thresholds as a screen and not a verdict. A schedule can pass every check on that list and still be wrong, because none of them knows whether the productivity rate was achievable.

The last screen matters most. Give the network to someone who will argue with it, and see whether the date survives.

## What the schedule does to the cash

A programme is a cash instrument, and this is the part of the overlap finance usually sees before delivery does.

The cash conversion cycle measures how long money is tied up between paying for work and being paid for it. It is days sales outstanding, plus days of unbilled work in progress, minus days payable outstanding.

Take DSO of 68 days, unbilled work in progress of 41 days and DPO of 55 days: 68 + 41 − 55 = **54 days** of working capital the business is funding.

Now slip the programme by three weeks, so a £2.1m application misses its cut-off and lands in the next cycle. At an 8% cost of funds, 2,100,000 × 0.08 × 30 ÷ 365 = **£13,808** for one application, in one month.

Nothing PCI publishes is legal, tax or accounting advice, and the treatment depends on the contract. The planner's part of it is simple: dates set when applications can be made, and applications set when cash arrives.

## Where this sits in the PCI curriculum

The PCI AI Project Controls Leader (PCL-AI) examines 13 domains across 61 knowledge areas, with project scheduling as one domain among them.

The Body of Knowledge runs in a 40 / 40 / 20 proportion of finance and reporting, project management, and governed AI. Scheduling sits beside cost management, earned value and risk rather than in its own compartment, which is the reason the cash section above belongs in a scheduling guide at all.

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**How detailed should activities be?**
Detailed enough to measure progress against, and no finer. A working rule is that an activity should not run longer than the reporting cycle plus its own float, so a monthly report wants most activities under about 44 working days. Very fine detail early produces a network that is expensive to maintain and no more accurate.

**How often should the baseline change?**
Only when scope, funding or contractual dates change, and always with a recorded reason. Rebaselining because the current programme is uncomfortable destroys the only reference you have for measuring variance. Keep the original baseline visible even after a change has been approved.

**What is a basis of schedule, and do I need one?**
It is the short document recording calendars, productivity rates and their sources, key assumptions, constraints and their justification, and the progress rules. You need one, because it is what turns a file into something a third party can audit. Two or three pages is normally enough.

**Can AI build the schedule?**
It can draft logic from a scope document, spot missing successors, and compare your durations against rates achieved on similar work. It should not issue a date you cannot explain. Every output still needs the quantity, the rate and the assumption written next to it, because that is what you will be asked for.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so treat this republish as distribution and qualified traffic, not as a backlink.*

*Internal links: this guide should link to [the critical path method definition](https://projectcontrolsinstitute.org/critical-path-method) with the anchor "how the critical path is calculated", to [the total float definition](https://projectcontrolsinstitute.org/total-float) with the anchor "what total float actually tells you", to [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis) with the anchor "testing the date with a QSRA", and to [project cash flow forecasting](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) with the anchor "how the programme drives the cash curve".*
