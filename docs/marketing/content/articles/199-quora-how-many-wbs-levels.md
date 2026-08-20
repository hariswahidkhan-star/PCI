---
platform:      Quora
type:          qa-list
title:         How many WBS levels should a project actually have?
meta:          How many WBS levels do you need? Four to six on most projects. The stopping tests, the arithmetic of one level too many, and where the schedule takes over.
primary_kw:    how many WBS levels
secondary_kw:  work breakdown structure, work package, control account, rolling wave planning
pillar:        Planning and scheduling
credential:    PML-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1545
hashtags:      n/a (Quora)
ab_id:         AB-00271
---

# How many WBS levels should a project actually have?

Most work breakdown structures settle at four to six levels, and the number is an outcome rather than a target. Decompose until each lowest element can be estimated with confidence, owned by one person, scheduled as activities of sensible length and measured objectively — then stop.

Depth is not a sign of rigour. A seven-level WBS on a £20m job is usually a sign that nobody applied a stopping test.

## How many WBS levels do most projects actually use?

The common shape runs like this. Level 1 is the project. Level 2 is major deliverables, phases or areas. Level 3 is sub-deliverables. Level 4 is where work packages usually land, and level 5 where they land on larger jobs.

Activities are not a WBS level. They live in the schedule beneath the work package, which is why a "level 6" that turns out to be a list of verbs is a schedule that has been pasted into the wrong document.

Control accounts sit wherever a branch of the structure meets a single owner, typically at level 3 or 4. That is the level at which variance gets explained in writing each month, so it should be a level a named person can defend.

## What tells you to stop decomposing?

Four tests, applied to the element in front of you, not to the tree as a whole.

**Estimable.** Can someone give a cost and duration they would defend? If the estimate is still a guess with a range of ±40%, the element covers too much.

**Ownable.** Is there exactly one person who can answer for it? Not a department. A person.

**Schedulable.** Can a planner write activities beneath it with durations that fit inside a reporting period or two? A work package that spans fourteen months cannot be status-checked meaningfully.

**Measurable.** Is there an objective completion test — units complete, weighted milestones, 0/100, or a defensible level-of-effort rule? "60% complete" with no rule behind it means the element is not decomposed enough.

The 8/80 rule of thumb — a work package of somewhere between eight and eighty hours of effort — is a useful sanity check on small projects and unusable on capital ones, where an eighty-hour package on a £200m job would give you tens of thousands of them. Use the four tests. Treat the rules of thumb as prompts, not requirements.

## What does one more level actually cost?

This is the calculation almost nobody does. Assume a branching factor of five at each split on a £20.00m project.

| Level | Elements | Average element value | Status effort at 5 minutes each |
|---|---:|---:|---:|
| 2 | 5 | £4,000,000 | 25 minutes |
| 3 | 25 | £800,000 | 2 hours |
| 4 | 125 | £160,000 | 10.4 hours |
| 5 | 625 | £32,000 | 52 hours |
| 6 | 3,125 | £6,400 | 260 hours |

The arithmetic: 3,125 × 5 minutes = 15,625 minutes = 260.4 hours, which at a 37.5-hour week is about **seven working weeks of someone's month**, every month, purely to update status.

The value column matters as much. At level 6 the average element is £6,400, which on a £20m job is a rounding error — and progress on a £6,400 element is a yes or a no, so the extra level has bought no additional precision, only additional clicks.

At level 4 the same project has 125 elements averaging £160,000, and status takes about a day and a half a month. That is a control system a team will actually maintain, which is the only kind that works.

## Do all branches need the same depth?

No, and forcing symmetry is one of the most common WBS mistakes. A WBS is not a tidy diagram; it is a control structure, and different scope needs different control.

A £6m civils branch with twelve subcontractors may need five levels before the work is estimable and measurable. A £0.3m training package delivered by one supplier against a fixed price may need two.

The rule that has to hold at every split is completeness: children must sum to their parent exactly, with no gap and no overlap. Depth is free to vary; coverage is not.

Rolling wave planning makes depth vary over time as well. Far-term scope sits at level 3 as a planning package with milestones and budget but no work packages, and is decomposed as the horizon approaches. That is disciplined, not lazy — decomposing year-three scope in month two produces detail that will be wrong.

## Where does the WBS stop and the schedule start?

The WBS stops at the work package. Below that, the planner writes activities, links them, and calculates dates and float.

The join is one-to-one in one direction: every activity belongs to exactly one work package. That single rule is what lets you roll schedule progress up into cost reporting without a reconciliation exercise.

It is also the test for whether you have gone too deep. If your lowest WBS level would have exactly one activity beneath it, you have written the schedule twice and gained nothing.

## Where does the WBS level meet the ledger?

Actual cost arrives through cost codes, and cost codes are usually set at the control account level. That sets a hard floor on how deep useful cost reporting can go.

If the ledger codes at level 4 and the WBS runs to level 6, actual cost cannot be split below level 4. Planned value and earned value exist at level 6, but there is no actual cost to compare them against, so CPI below level 4 is not measurable — anything reported there is an allocation someone invented.

Check this before the structure is signed off, not at the first month-end. The question to ask the finance team is simply which level they will code to, and the answer decides where the WBS should stop being a control tool and start being a planning convenience.

The same alignment drives the accounts. Where progress towards a performance obligation is measured by a cost-based input method, total expected costs is the sum of the control account forecasts, so the level at which forecasts are built is the level at which reported revenue is built.

That overlap between structure and reporting is what the PCI Project Management Leader – AI (PML-AI) credential examines across 16 domains and 63 knowledge areas, with a Body of Knowledge weighted 40% finance and reporting, 40% project management and 20% governed AI. Treatment depends on the contract and the reporting framework applied, and nothing PCI publishes is accounting advice.

## Frequently asked questions

**Is there a maximum number of WBS levels?**
No standard sets a hard limit, and none usefully could — a shipbuilding programme and an office fit-out do not need the same depth. The practical maximum is the point where the effort of maintaining the structure exceeds the value of the control it gives, which on most projects arrives at level 5 or 6.

**Should the WBS levels match the organisation chart?**
No. The WBS decomposes scope; the organisational breakdown structure decomposes responsibility. They meet at the control account, where a branch of scope is assigned to one owner. Building the WBS to mirror the org chart produces a structure that has to be rebuilt every time the team changes, which is often.

**Does every level need a budget?**
Every level carries a budget, but only the lowest level in each branch has a budget of its own — higher levels are the sum of their children. If a level-3 element holds budget that is not distributed to its work packages, that is either undistributed budget or contingency, and it should be labelled as such rather than left looking like scope.

**How deep should a WBS be at the tender stage?**
Usually two or three levels, matched to how the estimate is structured and how the client wants the price broken down. Full decomposition to work packages belongs to the mobilisation period, once the team, the subcontract strategy and the sequence are known. Tender-stage detail that is invented rather than known tends to survive into the baseline unchallenged.

**What if a work package is too big but cannot be split?**
Then it can usually be split by measurement rather than by scope. Keep the package whole and define weighted milestones inside it, each with an objective completion test. That gives measurable progress without adding a level and without creating elements nobody owns.

**How do you know a WBS is too deep?**
Three signs. Status updates take longer than the analysis of them, elements are small enough that percentage complete is meaningless, and the lowest level maps one-to-one onto schedule activities. Any one of those means the last level added control to a spreadsheet rather than to a project.

---

*Internal links: this answer should link once, at the end, to [the critical path method](https://projectcontrolsinstitute.org/critical-path-method) with the anchor "how work packages become scheduled activities", and to [a worked month-end example](https://projectcontrolsinstitute.org/earned-value-worked-example) with the anchor "what control account reporting looks like in practice"; Quora links are nofollow, so this is for qualified readers, not link equity.*
