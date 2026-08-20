---
platform:      Substack
type:          faq
title:         What is generative scheduling, and can you trust it?
meta:          Generative scheduling produces candidate programmes from scope, history and constraints. What it can do, how to score it, and what one missing link costs.
primary_kw:    generative scheduling
secondary_kw:  AI for construction scheduling, critical path, precision and recall, schedule logic
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        FAQPage
word_count:    1,775
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00283
---

# What is generative scheduling, and can you trust it?

Generative scheduling is the use of a model to produce candidate programmes — activities, logic, durations and resource assignments — from a scope description, a body of past projects and a set of constraints, and to regenerate alternatives when a constraint changes. It proposes. A planner still decides, and still owns the critical path.

*Written first for this newsletter. The validation numbers below are invented so the scoring can be followed line by line, but the method is exactly the one to run on any tool you are offered.*

## What is generative scheduling, precisely?

It is the generation of a programme structure from inputs that are not themselves a programme. Give the model a scope of work, a contract milestone set, resource limits and a library of comparable projects, and it returns an activity list with logic and durations attached.

That differs from what planning tools already automate. Scheduling engines calculate dates from logic you supplied; levelling routines resolve resource conflicts against rules you set. Neither invents an activity or proposes a dependency.

It also differs from asking a language model to summarise a programme. Reviewing a schedule and creating one are separate tasks with separate failure modes, and a tool that is good at the first is not automatically usable for the second.

| Approach | Input | Output | What it guarantees | How it fails |
|---|---|---|---|---|
| Manual planning | The planner's judgement and the team's input | A programme with owned logic | Nothing, beyond the planner's experience | Slowly, and visibly |
| Rules-based automation | A complete network and stated rules | Calculated dates, levelled resources | Arithmetic consistency with what you supplied | Silently, when the network is wrong |
| Optimisation solvers | A defined network, an objective, hard constraints | A sequence that scores best against the objective | Optimality against the objective you chose | By optimising the wrong objective |
| Generative model | Scope, constraints, historical projects | Candidate activities, logic and durations | Nothing at all | Confidently, with plausible-looking logic |

The fourth row is the one worth reading twice. A generative tool produces output that looks like the work of a competent planner whether or not it is, which is a different risk from a tool that produces an obvious error.

## What can it actually do today?

Three things well, and they are worth having.

It drafts a first-pass activity list and work breakdown from a scope document, which removes several days from the front end of a bid programme. It proposes logic from comparable past projects, which is genuinely useful where an organisation has a consistent history to learn from. It regenerates alternatives fast when a constraint moves, so testing four sequences costs an afternoon instead of a fortnight.

Three things it does badly. Durations that reflect your crews, unless it has been given your own productivity data rather than an industry average. Calendars, shifts, weather windows and holiday sets, which it will populate confidently and wrongly. Commercial context, because it does not know which milestone carries liquidated damages.

The honest way to place it: generative scheduling is good at the first 60% of a programme and dangerous in the last 40%, and the last 40% is where the completion date lives.

## How do you score a generative scheduler?

Like any classifier, on a validation set a planner has already reviewed. The unit to score is the logic link, because logic is where a programme is right or wrong.

On one package, the model proposed **1,240** links. The planner accepted **812** and rejected **428**. A separate review found **190** required links the model never proposed, so the set contains 812 + 190 = **1,002** genuine links.

Precision = accepted ÷ proposed = 812 ÷ 1,240 = **0.655**. Recall = accepted ÷ genuine = 812 ÷ 1,002 = **0.810**.

F1 is the harmonic mean: 2 × (0.655 × 0.810) ÷ (0.655 + 0.810) = 1.0611 ÷ 1.465 = **0.724**.

| Setting | Proposed | Accepted | Precision | Recall | F1 |
|---|---:|---:|---:|---:|---:|
| Default | 1,240 | 812 | 0.655 | 0.810 | 0.724 |
| Tightened threshold | 690 | 601 | 0.871 | 0.600 | 0.711 |

Tightening the threshold makes the tool look better on precision and worse where it matters. For schedule logic, recall is the number to protect, and this is the opposite of the trade-off you want in cost benchmarking.

The reason is asymmetric. A spurious link is visible, because it produces a date somebody knows is wrong. A missing link is invisible, because it produces float that does not exist and a critical path that runs through the wrong work. [How a critical path is calculated and read](https://projectcontrolsinstitute.org/critical-path-method) is the only way to see that the wrong work is on it.

## What does one missing link cost?

A short network makes it concrete. Two paths run from the same start.

Path A: 14 + 18 + 12 + 9 = **53 days**. Path B: 20 + 16 + 11 = **47 days**. Completion is 53 days, and Path B carries 6 days of total float.

Now restore the link the model missed: system testing, 15 days, cannot start until Path B's last activity finishes. Path B becomes 20 + 16 + 11 + 15 = **62 days**.

Completion moves to 62 days. Path B is now critical, and Path A has 62 − 53 = **9 days** of float. The generated programme understated the duration by 9 days, or 17%, and pointed the recovery effort at the wrong path.

Nobody would have found that by looking at the dates, because every date in the generated programme was internally consistent. It is found by [the review protocol that catches a missing link](https://pciai.org/llm-schedule-review): check that every activity has a predecessor and a successor, that open ends are deliberate, and that the critical path reads like a sequence a site engineer recognises.

## What has to be in place before you use one?

Four controls, none of them technical.

A named accountable planner for every generated programme. The tool has no professional obligation and cannot be examined; the person who accepts the output can be, and should be recorded as having done so.

A record of what produced the output: the model and its version, the inputs and constraints given to it, the source projects it drew on, and the date. A programme that cannot be reproduced cannot be defended in a claim, and a contract programme is a claims document before it is anything else.

A rule that the baseline is human-approved, whatever generated the draft. Acceptance is the control point, and it belongs in the schedule management procedure rather than in a tool setting.

A published error rate. If nobody can state the tool's precision and recall on your own projects, the answer to whether it can be trusted is that nobody knows.

Governed use of this kind is why the PCI AI Project Controls Leader (PCL-AI) Body of Knowledge is built in the proportions 40 finance and reporting, 40 project management, 20 governed AI, across 13 domains and 61 knowledge areas. The AI content is a fifth of the syllabus because the judgement being governed is the other four fifths.

## Where does it pay, and where does it not?

It pays at the front end, where speed is worth more than precision and the output is going to be rebuilt anyway: bid programmes, option studies, early works sequencing, and testing what a changed constraint does to a plan.

It does not pay on a contract baseline, on a programme that will support an extension of time claim, or on any schedule where the logic has been argued over and settled. Regenerating those loses the argument that produced them.

Between those two, the useful pattern is one direction only: generate the draft, then have a planner build the version that gets issued. Reversing it, by generating a replacement for a programme people have already agreed, throws away the most valuable thing in the file.

## Frequently asked questions

**Will generative scheduling replace planning engineers?**
Not on current evidence, and the reason is accountability rather than capability. Someone has to defend the programme in a progress meeting, an adjudication and a claim, and to know which milestone carries damages. The task most at risk is first-draft production, which is a small share of a senior planner's week and the least valuable part of it; [what the rest of that week is spent on](https://pciworld.org/senior-planning-engineer-career-path) is what the role is actually assessed on.

**Can it produce a contract-compliant programme?**
Only as a starting point. Contract programmes carry specific requirements on activity coding, calendars, milestone naming, resource loading, constraint use and open ends, and each is checkable. Run the compliance check you would run on any subcontractor's submission, because a generated programme fails these checks more often than an experienced planner's does.

**What data does it need to be useful on my projects?**
Your own completed programmes with as-built dates, your productivity records, and a consistent activity coding standard. A model trained only on public or generic data proposes generic durations, and generic durations are the reason the programme will not survive contact with the site. Data quality decides the output more than model choice does.

**Does it help with schedule risk analysis?**
Indirectly. It can generate the alternative sequences a risk analysis then tests, which is useful because most quantitative analyses examine one network and a set of duration ranges. It does not replace the risk model, and correlation assumptions and risk drivers still have to be set by people who know the project.

**How do you stop it inventing activities?**
Constrain it to an approved activity library and a coding standard, then reconcile the output against the scope document line by line. Treat any activity it proposes that is not in the library as a finding to be accepted or rejected explicitly. Invented activities are less dangerous than missing ones, but both are found the same way.

**Is a generated schedule auditable?**
Only if you make it so. Record the model, the version, the inputs, the source projects and the accepting planner alongside the programme file, and keep the generated draft as well as the issued version. Without that trail, the honest answer to how the programme was produced is that nobody can now say.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so this is not a republish of anything on the PCI site.*

*Linking note: three links are now in the body, one per domain. "The review protocol that catches a missing link" sits in the missing-link worked example (https://pciai.org/llm-schedule-review), because that paragraph says how the omission is found and the protocol is the method. "How a critical path is calculated and read" sits on the sentence about a critical path running through the wrong work (https://projectcontrolsinstitute.org/critical-path-method). "What the rest of that week is spent on" sits in the FAQ on replacement (https://pciworld.org/senior-planning-engineer-career-path), because the answer turns on what a senior planner's job actually consists of. The two other pciai pages proposed here were dropped: three links to one domain is the pattern the architecture forbids, and the schedule-review page is the one that answers a question this piece raises. Reciprocal: the pciai.org LLM schedule review page has a genuine reason to cite this piece's missing-link arithmetic, and that is the one link back worth making.*
