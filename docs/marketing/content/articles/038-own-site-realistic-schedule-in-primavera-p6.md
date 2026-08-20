---
platform:      Own site — projectcontrolsinstitute.org
type:          guide
title:         Build a realistic schedule in Primavera P6: eight steps
meta:          Eight steps to a realistic schedule in Primavera P6: calendars, a WBS that matches cost, durations from rates, honest logic and a screen before you issue it.
primary_kw:    realistic schedule in Primavera P6
secondary_kw:  schedule quality checks, productivity-based durations, resource levelling, retained logic
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo
word_count:    1797
hashtags:      n/a (own site)
ab_id:         AB-00078
---

# Build a realistic schedule in Primavera P6: eight steps

A realistic schedule in Primavera P6 is one where every duration came from a rate, every relationship models a physical dependency, and the finish date survives a challenge. The software does not make a schedule realistic. Eight steps do, and they run in this order because each one distorts the next if it is skipped.

Realism has a test attached: someone who was not in the room should be able to reconstruct why each date is what it is.

## What makes a realistic schedule in Primavera P6?

Three properties, testable before you open the file.

| Property | The test | The common failure |
|---|---|---|
| Traceable durations | Every duration has a quantity, a rate and a source behind it | A number typed straight into the cell |
| Honest logic | Every relationship names a physical or contractual reason | Links added to make the bars line up |
| A calculated date | The finish comes from the network, not from a constraint | A target date entered and the logic bent around it |

## Step one: fix the calendars before anything else

Calendars convert durations into dates, so getting them wrong moves every date in the file at once.

Set the working week, the public holidays, the annual shutdown and any shift patterns first. A 77-working-day activity on a five-day calendar spans about 108 calendar days; the same 77 days on a six-day calendar spans about 90. That is 18 calendar days of difference before anyone has planned anything.

Keep calendars few and name them for what they represent. Three or four are manageable; twenty is a float calculation nobody can explain.

## Step two: build the WBS to match how cost is controlled

The work breakdown structure is not a folder system. It is the join between the programme and the cost report.

If the cost report controls at control-account level and the schedule is coded to areas, nobody can say why the schedule shows 62% and the cost report shows 47%. Build the WBS so a control account maps to a set of activities without a translation table.

Use activity codes for everything else you slice by: discipline, subcontractor, area, phase. Codes give you views without breaking the structure that carries the money.

## Step three: derive durations from quantity and productivity

This is the step that separates a plan from a wish, and it is arithmetic rather than opinion.

Take a pipeline of 18.4 km. One spread lays 240 m a day, so the duration is 18,400 ÷ 240 = 76.7, rounded to **77 working days**.

Now apply the conditions. Winter working on that section runs at about 80% of the rate: 240 × 0.8 = 192 m a day, so 18,400 ÷ 192 = 95.8, rounded to **96 working days**. The seasonal assumption alone is worth 19 days, and it is the sort of thing that is invisible in a duration typed straight into the cell.

Record the quantity, the rate and its source against the activity. When someone challenges the duration in month nine, you want to argue about the rate, not your memory.

## Step four: build logic a reviewer can follow

Every relationship should describe a physical or contractual reason one thing follows another.

Prefer finish-to-start. Use start-to-start with a positive lag where activities genuinely overlap, and pair it with a finish-to-finish link so the successor cannot end before its predecessor. A start-to-start link with no matching finish link leaves the activity with no logical end.

Never use a negative lag. A lead on a finish-to-start relationship says the successor begins before the predecessor finishes, which cannot be resourced and breaks when the predecessor's duration changes.

Open ends are the other common defect. Only the first activity should have no predecessor and only the last no successor; anything else is detached, and its float is fiction.

## Step five: use constraints as rarely as you can

A constraint overrides the network's own arithmetic, so each one is a small piece of the schedule that no longer calculates.

The usual damage comes from a "Finish On or Before" date, which caps the late dates on everything feeding it. Paths that had genuine slack report zero float, and the schedule loses its ability to tell you which delay actually matters. A hard constraint destroys [what total float actually tells you](https://projectcontrolsinstitute.org/total-float) before it destroys anything else.

Where a date is contractual, model it as a milestone with a constraint and say so in the basis of schedule. Where it is a preference, take it out and let the logic produce the date.

## Step six: resource-load, then look at the histogram

Durations from rates assume the resource is there. Loading the schedule tests that assumption.

Take three parallel activities each needing 8 fitters when only 16 fitters exist. Unlevelled, the network finishes in 43 days; levelled to the real crew, one activity waits and the finish moves to **52 days**.

Levelling changes the driving path, so check the critical path again afterwards; [how the critical path is calculated](https://projectcontrolsinstitute.org/critical-path-method) shows why the driving chain moves when a duration or a resource does. A resource-limited finish date and a logic-driven finish date are different answers to different questions, and the report should say which one it is showing.

## Step seven: baseline, then set the progress rules

Baseline before the first update, and write the progress rules down before the first update as well.

Set the data date discipline: nothing remaining to the left of it, no actual progress to the right. Decide the percent complete type per activity, and use physical percent complete against rules of credit wherever the activity earns value.

Decide the out-of-sequence setting and declare it. Retained logic holds the unfinished part of an activity behind its incomplete predecessors; progress override releases it. The same update run under both settings can give different finish dates, so the narrative has to name the setting.

Rules of credit are the quiet one. If steel erection earns 20% at delivery, 60% at erection and 20% at bolt-up, everyone reports the same number for the same work, and progress stops being a negotiation.

## Step eight: screen it before you issue it

Run a mechanical screen first, because it finds the defects that make every other number untrustworthy. The DCMA 14-point schedule assessment, developed for US defence programmes and widely reused, is the common reference; the checks below are described in PCI's own words.

| Check | Common threshold | What a failure usually means |
|---|---|---|
| Activities missing predecessor or successor | 5% or fewer | Parts of the network are detached and their float is meaningless |
| Leads (negative lags) | none | The overlap is hidden and cannot be resourced |
| Lags | 5% or fewer | Duration is being stored in the relationship instead of in an activity |
| Relationship types | 90% or more finish-to-start | The network has been drawn to make bars look right |
| Hard constraints | 5% or fewer | The schedule is being told its answer rather than calculating it |
| High total float, above 44 working days | 5% or fewer | Missing successors, not genuine slack |
| Negative float | none | The programme already cannot meet a date it is committed to |
| Long durations, above 44 working days | 5% or fewer | Activities too coarse to measure progress against |

Treat the thresholds as a screen and not a verdict. A schedule can pass every one of them and still be wrong, because none of the checks knows whether the productivity rate was achievable.

The last screen matters most: give the network to someone who will argue with it, and see whether the date survives.

## What the schedule does to cash

A programme is a cash instrument, and this is the part of the overlap that finance sees before delivery does. [How the programme drives the cash curve](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) works the S-curve side of the same arithmetic.

The cash conversion cycle measures how long money is tied up between paying for work and being paid for it. It is days sales outstanding, plus days of unbilled work in progress, minus days payable outstanding.

Take DSO of 68 days, unbilled work in progress of 41 days and DPO of 55 days: 68 + 41 − 55 = **54 days** of working capital the business is funding.

Now slip the programme by three weeks so a £2.1m application misses its cut-off and lands in the next cycle. At 8% cost of funds, 2,100,000 × 0.08 × 30 ÷ 365 = **£13,808** for one application in one month.

Nothing PCI publishes is accounting or tax advice, and the treatment depends on the contract. The planner's part is simple: dates set when applications can be made, and applications set when cash arrives.

## Where this sits in the PCI curriculum

PCL-AI examines 13 domains across 61 knowledge areas, with project scheduling as one domain among them.

The Body of Knowledge runs in a 40 / 40 / 20 proportion of finance and reporting, project management, and governed AI. Scheduling sits beside cost management, earned value and risk rather than in its own compartment, which is the same reason the cash section above belongs in a scheduling guide.

## Frequently asked questions

**How detailed should activities be?**
Detailed enough to measure progress against, and no finer. A working rule is that an activity should not run longer than the reporting cycle plus its own float, so a monthly report wants most activities under about 44 working days. Very fine detail early produces a network that is expensive to maintain and no more accurate.

**Should the schedule be resource-loaded?**
Load it where resource availability actually constrains the work, which on most construction and systems projects is true for the trades and not for management. Full loading of every activity produces a file that is heavy to update and rarely improves the date. Partial loading of the constraining resources gives most of the benefit.

**How often should the baseline change?**
Only when scope, funding or contractual dates change, and always with a recorded reason. Rebaselining because the current programme is uncomfortable destroys the only reference you have for measuring variance. Keep the original baseline visible even after a change is approved.

**What is a basis of schedule and do I need one?**
It is the short document that records calendars, productivity rates and their sources, key assumptions, constraints and their justification, and the progress rules. You need one, because it is what turns a file into something a third party can audit. Two or three pages is normally enough.

**Can AI build the schedule?**
It can draft logic from a scope document, spot missing successors, and compare your durations against rates achieved on similar work. It should not issue a date you cannot explain. Every output still needs the quantity, the rate and the assumption written next to it, because that is what you will be asked for.

---

*Internal linking note: three same-domain links now sit in the body. "What total float actually tells you" points at the total float definition, placed in step five where a hard constraint is shown wiping out the float calculation and the reader has to know what was lost. "How the critical path is calculated" points at the critical path method definition, placed in step six where levelling moves the driving path. "How the programme drives the cash curve" points at project cash flow forecasting, placed at the head of the cash section, which raises the S-curve without drawing one. No cross-estate link is carried: every question this guide raises is answered on the hub. The fourth proposal, a schedule risk analysis link on "testing the date with a QSRA", was dropped because this guide never puts a range around the date, so the sentence that would carry it does not exist. Reciprocal: the critical path method definition should link back here with an anchor about building a network that calculates its own date.*
