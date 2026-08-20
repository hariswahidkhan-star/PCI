---
platform:      Instagram / Facebook carousel
type:          carousel
title:         What AI actually does to a planning engineer's week
meta:          Of a 40-hour planning week, 5 hours were analysis. Automate the collation honestly and it becomes 16.5. Eight slides on AI in project controls.
primary_kw:    AI in project controls
secondary_kw:  schedule automation, precision recall F1, AI governance, reporting automation
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    830
hashtags:      #AIGovernance #ProjectControls #Scheduling #ProjectManagement #PMO #Primavera
ab_id:         AB-00038
---

# What AI actually does to a planning engineer's week

*Instagram and Facebook carousel — 8 slides, 1080 × 1350. Instagram captions carry no clickable link, so the link goes in the bio; on Facebook it goes in the post.*

**Caption (the first 125 characters have to earn the swipe):**

Of a 40-hour planning week, 5 hours were analysis. The other 35 were fetching, chasing and formatting.

AI in project controls is not a machine that plans. It is a machine that removes the fetching. Eight slides: the week broken down in hours, what the automation actually gives back, the three numbers that decide whether an at-risk flag deserves to be on the report, and the four things that never move off a human.

Save it for the next tooling conversation.

---

**Slide 1 — What AI in project controls actually touches**

It touches the parts of the job that are repetitive and verifiable: pulling data from many systems into one shape, drafting narrative from numbers, and flagging patterns across more records than a person will read.

It does not touch judgement, entitlement or accountability. Those stay where they were, and pretending otherwise is how organisations buy tools that nobody trusts.

**Slide 2 — The week, as it actually is**

A planning engineer's 40 hours, on a typical monthly cycle:

Data collation 9 · Progress chasing 6 · Schedule updating 7 · Report production 6 · Narrative writing 4 · Analysis 5 · Meetings 3

Five hours of analysis in forty. That is the number the tooling argument is really about.

**Slide 3 — The arithmetic**

| Task | Before | After | Change |
|---|---:|---:|---:|
| Data collation | 9.0 | 2.0 | −7.0 |
| Report production | 6.0 | 1.5 | −4.5 |
| Narrative drafting | 4.0 | 2.0 | −2.0 |
| **Released** | | | **13.5** |
| Verifying model output | 0.0 | 2.0 | +2.0 |
| **Net moved** | | | **11.5** |

Analysis goes from **5 hours to 16.5** — a little over three times as much thinking, in the same week.

Progress chasing does not move. It is a human problem about people not answering, and no tool has ever fixed it.

**Slide 4 — Where it earns its place**

Collation, because merging fourteen contractor returns into one shape is deterministic work with a right answer.

First drafts, because a narrative generated from the variance table is faster to correct than to write, provided the planner corrects it.

Anomaly detection, because a machine will read 4,000 activities for broken logic and a person will read 400 and get bored.

**Slide 5 — Measure the flag, or do not fly it**

A logic checker runs over **500** activities. It flags **96**. Of those, **71** are real defects. There are **89** real defects in total.

Precision = 71 ÷ 96 = **0.740** — how often a flag was right
Recall = 71 ÷ 89 = **0.798** — how much of the real problem it caught
F1 = 2 × (0.740 × 0.798) ÷ (0.740 + 0.798) = **0.768**

That is **25 false flags** and **18 defects missed**. Publish those three numbers beside the output, every month, or the flag is a colour with nobody's name on it.

**Slide 6 — F1 assumes the two errors cost the same**

They never do. A false flag costs a planner ten minutes. A missed defect on a driving activity costs a month.

So set the threshold on the consequence, not on the highest F1. Loosening a filter until it catches nearly everything is often correct, and it will make your F1 worse. Report both, and say which you optimised for.

**Slide 7 — What does not move**

Choosing an estimate at completion method, because that is a judgement about cause and a model cannot see cause.

Deciding entitlement, because that is a contractual question answered by records.

Signing the forecast, because accountability cannot be delegated to a system that cannot be asked why.

Explaining an interface delay to somebody who does not read schedules, because that is the actual job.

**Slide 8 — The governance minimum**

Version the model. Stamp the data date and the refresh time on the output. Keep the prompt and the source data alongside the result. Have a named human sign anything that leaves the team. Publish the model's accuracy beside its recommendations.

Governed AI is **20%** of every PCI Body of Knowledge for exactly this reason, alongside 40% finance and reporting and 40% project management. The PCI AI Project Controls Leader (PCL-AI) covers 13 domains and 61 knowledge areas, and the governed AI portion is about proving a model is fit to be believed, not about using one.

---

#AIGovernance #ProjectControls #Scheduling #ProjectManagement #PMO #Primavera

**Link (bio on Instagram, in-post on Facebook):** where AI genuinely belongs in project controls, and where it does not — https://pciai.org/ai-in-project-controls

---

*Every figure above is illustrative arithmetic, not project data or a benchmark. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (bio link and first comment): [AI in project controls](https://pciai.org/ai-in-project-controls) with that anchor, [whether AI will replace planning engineers](https://pciai.org/will-ai-replace-planning-engineers) with that anchor, and [an AI policy for project controls](https://pciai.org/ai-policy-for-project-controls) with the anchor "the governance minimum, written down".*
