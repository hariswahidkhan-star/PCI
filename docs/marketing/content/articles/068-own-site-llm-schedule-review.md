---
platform:      Own site — pciai.org
type:          how-to
title:         LLM schedule review: a protocol for planning teams
meta:          An LLM schedule review finds structural defects fast and judges buildability not at all. The seven-step protocol, the checks, and a worked float example.
primary_kw:    LLM schedule review
secondary_kw:  critical path method, total float, schedule quality checks, governed AI
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        HowTo
word_count:    1708
hashtags:      n/a (own site)
ab_id:         AB-00151
---

# LLM schedule review: a protocol for planning teams

An LLM schedule review checks a programme's structure, not its realism. Export the logic, redact what you must, name the checks you want, demand a table of activity IDs, and verify every finding in the scheduling tool. It will find open ends, hard constraints and out-of-sequence progress. It will not tell you whether the sequence can be built.

Used that way it removes an afternoon of grinding each month. Used as an opinion on the plan it produces confident nonsense, and confident nonsense in a programme review is expensive. The same split between search and judgement decides [where AI earns its place across project controls](https://pciai.org/ai-in-project-controls).

## What is an LLM schedule review?

An LLM schedule review is a structured pass over a schedule export by a language model, producing a list of named structural defects with activity references, each of which is then confirmed in the scheduling tool before anyone acts on it.

The model is the search step. The tool remains the system of record, and the planner remains the author of the plan.

## What can a language model actually check?

Structural questions, because they are pattern recognition over a table. The list below is the useful boundary.

| Check | What it means | Model finds it? | How you verify |
|---|---|---|---|
| Open ends | Activities with no predecessor or no successor | Reliably | Filter on relationship count in the tool |
| Dangling logic | Only a start tie or only a finish tie, so the activity floats free at one end | Reliably | Inspect the relationship type on each flagged ID |
| Hard constraints | Fixed dates that override the logic and hide real float | Reliably | Constraint column filter |
| Long or negative lags | Lags used to disguise missing activities or to force an overlap | Reliably | Sort relationships by lag value |
| Out-of-sequence progress | Progress recorded where predecessors are incomplete | Usually | Run the tool's out-of-sequence report |
| Very long durations | Activities long enough to hide progress and defeat measurement | Reliably | Duration sort against your own threshold |
| Calendar mismatches | Successors on calendars that make the stated dates impossible | Sometimes | Check calendar assignment per flagged pair |
| Float distribution | Whole chains sitting at improbable float values | Sometimes | Recalculate and inspect the float histogram |
| Critical path length | Whether the driving path reaches the completion milestone | Unreliably | Compute in the tool, always |
| Buildability | Whether crews, cranes and permits allow the sequence | No | Human, on site, with the delivery team |
| Float ownership | Whether the contract lets you consume the float you found | No | Read the contract |

The last three rows are the point of the table. A model that answers those questions confidently is not adding a capability, it is removing a warning.

## The protocol, step by step

Seven steps. It takes about twenty minutes once, then about five each month.

**One: export the logic, not a picture.** A tabular export with activity ID, name, duration, predecessors with relationship type and lag, constraints, calendar, percent complete and total float. Screenshots of a Gantt chart carry almost none of what the review needs.

**Two: redact before you paste.** Strip client names, subcontractor rates, personal names and anything commercially restricted. Activity IDs and durations are usually enough for a structural review.

**Three: state the standard in the prompt.** Name the checks you want and your own thresholds — for example, activities over 44 working days, lags over 10 days, any constraint other than start-no-earlier-than. Without your thresholds the model uses whatever convention it absorbed, which is why [how to write and test the instruction you give it](https://pciai.org/prompt-engineering-for-project-professionals) is worth an hour of practice before the first run.

**Four: fix the output shape.** Demand one row per finding with activity ID, check name, the evidence from the export, and a severity you defined. A prose essay about your schedule is not reviewable.

**Five: require a count and an admission.** Ask for the number of activities examined and an explicit list of checks it could not complete on the data supplied. Quiet omission is the characteristic extraction failure.

**Six: verify each finding in the tool.** Every row gets confirmed or dismissed before it reaches a report. This step is not optional and it is what makes the whole exercise defensible.

**Seven: record provenance.** Keep the export, the prompt, the model version and the findings table together. If a finding later matters in a delay argument, you will need to show where it came from.

## Worked example: does the model understand the critical path?

Test it on a network small enough to check by hand; if the passes are not fresh, [the critical path method set out with a worked example](https://projectcontrolsinstitute.org/critical-path-method) covers them. Six activities, durations in working days.

A takes 5 days and starts the job. B takes 10 and follows A. C takes 4 and also follows A. D takes 8 and follows B. E takes 10 and follows C. F takes 4 and needs both D and E.

Forward pass, earliest start and earliest finish:

- A: ES 0, EF 5
- B: ES 5, EF 15
- C: ES 5, EF 9
- D: ES 15, EF 23
- E: ES 9, EF 19
- F: ES max(23, 19) = 23, EF 27

Project duration is **27 days**, and the driving path is A → B → D → F, which is 5 + 10 + 8 + 4 = 27.

Backward pass, latest finish and latest start, working from a required finish of 27:

- F: LF 27, LS 23
- D: LF 23, LS 15
- E: LF 23, LS 13
- B: LF 15, LS 5
- C: LF 13, LS 9
- A: LF 5, LS 0

Total float is LS − ES. A, B, D and F are all zero, which confirms the critical path. C and E each carry 9 − 5 = **4 days** and 13 − 9 = **4 days**.

Free float is the earliest start of the successor minus the activity's own earliest finish. For C that is 9 − 9 = **0 days**, and for E it is 23 − 19 = **4 days**.

That difference is the whole lesson. C has four days of total float and none of it is free: slip C by a day and E moves, even though the completion date holds. A model asked to explain that distinction usually explains it well; a model asked to compute the passes above gets it right often enough to be dangerous and wrong often enough to be useless.

Compute the passes in the scheduling tool. Use the model to explain the result, to hunt for structural defects, and to draft the narrative that goes around it.

## How do you keep the review out of trouble?

Two risks, both manageable, and both belong in [the clauses a controls team's AI policy needs](https://pciai.org/ai-policy-for-project-controls).

Confidentiality is the first. A schedule export can identify a client, a site and a commercial position, so redaction happens before the paste, not after the finding.

Overreach is the second. The moment a model's output is quoted as an opinion on the plan rather than a list of defects to verify, the planner has handed authorship to a tool that cannot attend the site.

## What does any of this have to do with the money?

Schedule dates set the timing of revenue, cash and claims, so a structural defect in a programme becomes a reporting defect two steps later.

An engineer is examined on float, logic and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised and almost never on a driving path. The completion dates cross that boundary every month and nobody in the chain has been examined on the crossing.

A constraint that hides eight weeks of float also hides the point at which a milestone payment becomes doubtful. That is why schedule quality belongs in a finance conversation and not only in a planning one.

## How does PCI examine this?

Schedule quality and governed AI both sit inside the PCI AI Project Controls Leader (PCL-AI) credential, which has 13 domains and 61 knowledge areas.

The Body of Knowledge is proportioned 40/40/20 across finance and reporting, project management, and governed AI. Behind it sit 113 mandatory PCI Standards carrying 532 process requirements.

PCI is an independent certifying body. Nothing here is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Can a language model replace a schedule quality check in the tool?**
No. What it replaces is the manual hunt through a long export for structural defects, which is slow, dull work that people skip when a report is due. The tool still recalculates the network and still owns the dates, and every finding the model raises is confirmed there before it reaches anyone's report.

**What file should I give it?**
A tabular export, CSV or similar, carrying activity ID, name, duration, predecessors including relationship type and lag, constraints, calendar, percent complete and total float. Exports that omit relationship types make most of the useful checks impossible, because open ends and dangling logic are defined by the relationship rather than by the dates.

**Will it get the critical path right?**
Sometimes, and that is the problem. Forward and backward passes are exactly the kind of multi-step arithmetic where a model produces a plausible answer with one bad step inside it. Take the path from the tool and use the model for explanation.

**Is it safe to paste a client schedule into a hosted model?**
Not without checking the contract and your own data rules first, because many engineering agreements restrict disclosure to third parties and a hosted model is one. Redact client identifiers, site names, rates and personal names, and if the commercial position would still be recoverable from what remains, do not paste it at all.

**How often should this run?**
Monthly, against the same checks and the same thresholds, on the schedule you are about to report rather than on a copy taken afterwards. Most of the value is in the trend: the same constraint or open end reappearing after three updates is telling you something about how the programme is being maintained, and that is a management finding rather than a technical one.

---

*Internal links: placed in the body. Three on pciai.org — the AI in project controls pillar, where the intro states the search-versus-judgement split; prompt engineering, at the step that tells the model your thresholds; and the AI policy template, where the confidentiality and overreach risks belong in writing. One cross-estate link, to the hub's critical path method page, at the point the worked example asks a reader to run the passes by hand. The originally proposed second hub link, to total float, was dropped: one link per cross-estate domain is the cap, and the total float subject is better served from the India course comparison, which turns on it. Reciprocal: pciai.org's generative AI project reporting piece links back here from its schedule narrative row, since the checks in this protocol run before those dates reach a report.*
