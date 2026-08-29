---
platform:      LinkedIn post
type:          linkedin-post
title:         Prompting a model to review a schedule: the protocol we use
meta:          A model will call your critical path wrong with total confidence. Three passes, a typed output, and the precision, recall and F1 that decide if it stays.
primary_kw:    LLM schedule review
secondary_kw:  precision recall F1, schedule quality checks, governed AI, critical path
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    398
hashtags:      #ProjectControls #Scheduling #AIGovernance #Primavera
ab_id:         AB-00151
---

# Prompting a model to review a schedule: the protocol we use

**Post body (1,829 characters):**

Paste a schedule into a chat window and the model will tell you the critical path is wrong, with total confidence and no method. The fix is not a better prompt. It is an LLM schedule review built as a pipeline with three passes.

Pass one is code, not a model. Open ends, dangling logic, hard constraints, lags over your threshold, durations over your threshold, out-of-sequence progress. All of that is a graph predicate. It is cheaper, faster and exactly reproducible, and no model should ever be asked a question that a filter answers.

Pass two is the model, and only on what survived pass one. It gets the reduced table and a schema, and every finding must come back typed: activity ID, defect name, the field that evidences it, a confidence. If it cannot name an activity ID, it is not a finding.

Pass three is a planner in the tool. Nothing is reported until it has been reproduced in P6.

Now measure the thing, because "it found some issues" is not a control.

Take a 4,200-activity programme. Pass one returns 316 flags; after dedupe and thresholds, 74 go to the model. It returns 61 findings. Verified in the tool, 44 stand.

Precision = 44 ÷ 61 = 0.72
Seed 50 known defects, the pipeline catches 41, so recall = 41 ÷ 50 = 0.82
F1 = 2 × 0.72 × 0.82 ÷ (0.72 + 0.82) = 0.77

Manage precision, not recall. Below roughly 0.70 the planners stop opening the report, and a control nobody reads has a real recall of zero however good the number looks.

Three things stay off the list permanently. Whether the sequence can be built, which needs the site team. Whether you are contractually allowed to consume the float you found, which needs the contract.

And the critical path itself, which gets computed in the tool every time. A model that answers those three confidently has not gained a capability. It has lost a warning.

#ProjectControls #Scheduling #AIGovernance #Primavera

**First comment:** The full protocol, with the output schema, the deterministic pre-pass and the boundary table of what a model can and cannot check: https://pciai.org/llm-schedule-review

---

*Every figure above is illustrative arithmetic, not project data. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [LLM schedule review](https://pciai.org/llm-schedule-review) with the anchor "the schedule review protocol in full", and [critical path method](https://projectcontrolsinstitute.org/critical-path-method) with the anchor "why the critical path is computed in the tool, not inferred".*
