---
platform:      X / Threads
type:          thread
title:         Generative scheduling: what it can and cannot do yet
meta:          A model flagged 120 relationships in a 4,300-line programme and 78 were real. Six posts on generative scheduling, scored with precision, recall and F1.
primary_kw:    generative scheduling *
secondary_kw:  AI in project controls, precision and recall, schedule logic review, critical path
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    370
hashtags:      #ProjectControls #AIGovernance
ab_id:         AB-00283
---

# Generative scheduling: what it can and cannot do yet

*X / Threads thread — 6 posts, each under 280 characters and each able to stand alone. The link sits in the final post. Character counts are for production; X counts any URL as 23 characters, so the live figures run lower.*

**Post 1/6 — the hook** (214 characters)
A model read 4,300 relationships in a programme and flagged 120 of them. Seventy-eight of the flags were real defects.

That is the honest position on generative scheduling: worth the hour, nowhere near unattended.

**Post 2/6 — the definition** (256 characters)
Generative scheduling is a model proposing the schedule itself: activities, logic and durations drafted from a scope document or past projects, rather than typed in by a planner.

The planner's job moves from typing to adjudicating. That is the harder job.

**Post 3/6 — score it properly** (276 characters)
120 flags raised, 78 correct, 96 genuine defects in the file.

Precision = 78 ÷ 120 = 0.65
Recall = 78 ÷ 96 = 0.81
F1 = 2PR ÷ (P + R) = 0.72

So 42 false alarms to clear and 18 real defects still sitting in the programme. Publish those three numbers or the tool is unmeasured.

**Post 4/6 — what it is genuinely good at** (258 characters)
First-pass activity lists from a scope document. Open ends, dangling logic and missing predecessors. Calendar mismatches. Alternative sequences under a constraint you state.

All of it checkable in minutes, which is the test any generated output has to pass.

**Post 5/6 — what it cannot do yet** (255 characters)
It cannot own a duration. Durations come from resource, productivity and access, and the model has measured none of them.

CPM arithmetic is deterministic. A plausible network with invented durations returns a confident, precise and wrong completion date.

**Post 6/6 — the governance line** (275 characters)
Treat generated logic like a draft from a new starter. Read it, correct it, sign it. Record which parts a model wrote and who accepted them. That record separates assistance from a programme nobody owns.
https://pciai.org/ai-in-project-controls
#ProjectControls #AIGovernance

---

*Figures are a worked example of how to score a review tool, not a benchmark of any product.*

*Internal links: the final post carries the only link and points at [AI in project controls](https://pciai.org/ai-in-project-controls) with that anchor. Reply posts should use [running an LLM schedule review](https://pciai.org/llm-schedule-review) and [an AI policy for project controls](https://pciai.org/ai-policy-for-project-controls) with those anchors.*
