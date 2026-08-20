---
platform:      DEV Community
type:          how-to
title:         LLM schedule review: building a checkable pipeline
meta:          An LLM schedule review works as a pipeline: deterministic pre-pass, constrained model pass, verification in the tool. Schema, code and a worked float example.
primary_kw:    LLM schedule review
secondary_kw:  critical path method, total float, schedule quality checks, governed AI
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     canonical -> /llm-schedule-review (own site #068)
schema:        HowTo
word_count:    1714
hashtags:      #ai #python #productivity #careerdev
ab_id:         AB-00151
---

# LLM schedule review: building a checkable pipeline

An LLM schedule review is a scripted pass over a schedule export that returns named structural defects with activity IDs, each verified in the scheduling tool before anyone acts on it. It reliably finds open ends, hard constraints and out-of-sequence progress. It does not judge whether the sequence can actually be built.

Treat it as a pipeline with a typed output, not as a chat window. The difference between those two framings is the difference between a control and an anecdote.

## What is an LLM schedule review?

An LLM schedule review is a structured pass over a schedule export by a language model, producing a list of named structural defects with activity references, confirmed in the scheduling tool before anyone acts on them.

The model is the search step. The tool stays the system of record and the planner stays the author of the plan.

For anyone arriving from outside the discipline: a construction or engineering programme is a directed acyclic graph of activities with durations, relationships, lags, calendars and constraints. Most of the defects worth finding are graph defects, which is why a language model is useful at all.

## What can a language model actually check?

Structural questions, because those are pattern recognition over a table. The boundary matters more than the capability.

| Check | What it means | Model finds it? | How you verify |
|---|---|---|---|
| Open ends | Activities with no predecessor or no successor | Reliably | Filter on relationship count in the tool |
| Dangling logic | Only a start tie or only a finish tie, so the activity floats free at one end | Reliably | Inspect the relationship type on each flagged ID |
| Hard constraints | Fixed dates that override logic and hide real float | Reliably | Constraint column filter |
| Long or negative lags | Lags disguising a missing activity or forcing an overlap | Reliably | Sort relationships by lag value |
| Out-of-sequence progress | Progress recorded where predecessors are incomplete | Usually | Run the tool's out-of-sequence report |
| Very long durations | Activities long enough to hide progress and defeat measurement | Reliably | Duration sort against your own threshold |
| Calendar mismatches | Successors on calendars that make the stated dates impossible | Sometimes | Check calendar assignment per flagged pair |
| Critical path length | Whether the driving path reaches the completion milestone | Unreliably | Compute in the tool, always |
| Buildability | Whether crews, cranes and permits allow the sequence | No | Human, on site, with the delivery team |
| Float ownership | Whether the contract lets you consume the float you found | No | Read the contract |

The last three rows are the point of the table. A model that answers them confidently has not added a capability, it has removed a warning.

## Run the deterministic checks first

Anything expressible as a graph predicate should never reach the model. It is cheaper, faster and exactly reproducible in code, and every token you save on solved problems buys attention for the ambiguous ones.

```python
import csv
from collections import defaultdict

def load(path):
    rows = list(csv.DictReader(open(path, newline="", encoding="utf-8")))
    preds, succs = defaultdict(list), defaultdict(list)
    for r in rows:
        for link in filter(None, r["predecessors"].split(";")):
            pid, rel, lag = link.split(":")          # "A1020:FS:5"
            preds[r["activity_id"]].append((pid, rel, int(lag)))
            succs[pid].append((r["activity_id"], rel, int(lag)))
    return rows, preds, succs

def structural_findings(rows, preds, succs, max_duration=44, max_lag=10):
    out = []
    for r in rows:
        aid = r["activity_id"]
        if not preds[aid]:
            out.append((aid, "open_end", "no predecessor"))
        if not succs[aid]:
            out.append((aid, "open_end", "no successor"))
        if int(r["original_duration"]) > max_duration:
            out.append((aid, "long_duration", r["original_duration"]))
        if r["constraint_type"] not in ("", "SNET"):
            out.append((aid, "hard_constraint", r["constraint_type"]))
        for _, rel, lag in preds[aid]:
            if lag < 0 or lag > max_lag:
                out.append((aid, "lag_out_of_range", f"{rel} {lag}"))
    return out
```

That covers roughly half the findings list with zero variance between runs. The model then handles what a predicate cannot express: sequences that read wrongly, names that do not match the work, chains whose float pattern suggests an unrecorded constraint.

## Fix the output shape before you send anything

A prose essay about your programme is not reviewable. Demand one object per finding and reject anything that fails validation, in code, before a human sees it.

```json
{
  "activities_examined": 4218,
  "checks_not_completed": ["calendar_mismatch: calendar IDs absent from export"],
  "findings": [
    {
      "activity_id": "C-1420",
      "check": "hard_constraint",
      "evidence": "constraint_type=MSO, constraint_date=2026-04-13",
      "severity": "high",
      "verified": null
    }
  ]
}
```

Two fields carry most of the value. `activities_examined` lets you catch a model that quietly processed the first 400 rows of a 4,218-row export. `checks_not_completed` forces the omission into the open, and quiet omission is the characteristic extraction failure.

`verified` starts as `null` and is set by the planner in the tool. A finding that reaches a report with `verified: null` is a process failure, and a two-line assertion in your pipeline can enforce that.

## The seven steps around the code

**One: export logic, not a picture.** Activity ID, name, duration, predecessors with relationship type and lag, constraints, calendar, percent complete, total float. A Gantt screenshot carries almost none of it.

**Two: redact before you send.** Strip client names, subcontractor rates, personal names and anything commercially restricted. IDs and durations are usually enough for a structural review.

**Three: state your thresholds in the prompt.** Activities over 44 working days, lags over 10 days, any constraint other than start-no-earlier-than. Without your numbers the model applies whatever convention it absorbed.

**Four: constrain the output** to the schema above and validate on receipt.

**Five: require the count and the admission**, as the two fields already described.

**Six: verify every finding in the tool.** This is what makes the exercise defensible in a delay argument.

**Seven: record provenance** — export, prompt, model version, findings table, kept together.

## Worked example: does it understand the critical path?

Test any model on a network small enough to check by hand. Six activities, durations in working days: A takes 5 and starts the job; B takes 10 and follows A; C takes 4 and also follows A; D takes 8 and follows B; E takes 10 and follows C; F takes 4 and needs both D and E.

Forward pass, earliest start and earliest finish:

- A: ES 0, EF 5
- B: ES 5, EF 15
- C: ES 5, EF 9
- D: ES 15, EF 23
- E: ES 9, EF 19
- F: ES max(23, 19) = 23, EF 27

Project duration is **27 days** and the driving path is A → B → D → F, which is 5 + 10 + 8 + 4 = 27.

Backward pass from a required finish of 27:

- F: LF 27, LS 23
- D: LF 23, LS 15
- E: LF 23, LS 13
- B: LF 15, LS 5
- C: LF 13, LS 9
- A: LF 5, LS 0

Total float is LS − ES. A, B, D and F are zero, confirming the path. C carries 9 − 5 = **4 days** and E carries 13 − 9 = **4 days**.

Free float is the successor's earliest start minus this activity's earliest finish. For C that is 9 − 9 = **0 days**; for E it is 23 − 19 = **4 days**.

That gap is the lesson. C holds four days of total float and none of it is free, so slipping C by a day moves E even though the completion date holds. Models explain that distinction well and compute the passes above wrongly often enough to be useless, which is why the passes stay in the scheduling engine.

## Confidentiality and overreach

A schedule export can identify a client, a site and a commercial position, so redaction happens before the request, not after the finding. Many engineering agreements restrict disclosure to third parties, and a hosted model is a third party unless your contract says otherwise.

Overreach is the second risk. The moment model output is quoted as an opinion on the plan rather than a list of defects to verify, authorship has passed to a tool that cannot attend the site.

## Why a graph defect becomes a finance defect

Schedule dates set the timing of revenue, cash and claims, so a structural defect in a programme becomes a reporting defect two steps later.

An engineer is examined on float, logic and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised, and almost never on a driving path. Completion dates cross that boundary every month and nobody in the chain has been examined on the crossing.

A constraint hiding eight weeks of float also hides the point at which a milestone payment becomes doubtful. That is why schedule quality belongs in a finance conversation.

## How PCI examines this

Schedule quality and governed AI both sit inside the PCI AI Project Controls Leader (PCL-AI) credential, which has 13 domains and 61 knowledge areas.

Its Body of Knowledge is proportioned 40/40/20 across finance and reporting, project management, and governed AI. Behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements.

PCI is an independent certifying body. Nothing here is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Can a language model replace schedule quality checking in the tool?**
No. It replaces the manual hunt through a long export for structural defects, which is slow work people skip when a report is due. The tool still recalculates the network and still owns the dates, and every finding is confirmed there before it reaches anyone's report.

**Why not put the whole export in the context window?**
You can on a small programme, but cost and recall both degrade as the table grows, and long-context recall is uneven in the middle of a document. Run deterministic checks over the full file in code, then send the model the subset that needs judgement along with your thresholds.

**What file format works best?**
A tabular export carrying activity ID, name, duration, predecessors with relationship type and lag, constraints, calendar, percent complete and total float. Exports omitting relationship types make most useful checks impossible, because open ends and dangling logic are defined by the relationship rather than by the dates.

**Will it get the critical path right?**
Sometimes, which is worse than never. Forward and backward passes are multi-step arithmetic where a model produces a plausible answer containing one bad step. Take the path from the scheduling engine and use the model for explanation and for drafting the narrative around it.

**How do I measure whether the protocol is working?**
Score it against a manual audit of the same update. Count flags raised, flags a planner judged real, and real issues the audit found that the pass missed, then compute precision, recall and F1 on those counts. Re-run the same set when the model version changes, because providers update models without changing the name.

---

*First published on pciai.org; the `canonical_url` on this post points there. DEV prohibits stub posts that link out to a full article elsewhere, so the complete protocol is here rather than behind a link.*

*Internal links: this how-to should link to [the critical path method](https://projectcontrolsinstitute.org/critical-path-method) with the anchor "how the forward and backward passes work", to [prompt engineering for project professionals](https://pciai.org/prompt-engineering-for-project-professionals) with the anchor "how to specify and test the prompt this pipeline sends", and to [AI in project controls](https://pciai.org/ai-in-project-controls) with the anchor "the governed-AI controls pillar it supports".*
