---
platform:      LinkedIn Article
type:          faq
title:         AI and planning engineer roles: replacement or shift?
meta:          AI and planning engineer roles, answered task by task, with a worked critical path showing which half of the job automates and which half does not.
primary_kw:    AI and planning engineer roles
secondary_kw:  will AI replace planning engineers, critical path method, total float, schedule quality checks
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        FAQPage
word_count:    1554
hashtags:      #ProjectControls #Scheduling #Primavera #AIGovernance #ProjectManagement
ab_id:         AB-00147
---

# AI and planning engineer roles: replacement or shift?

No, and the honest version of that answer is more useful than either the hype or the reassurance. AI and planning engineer roles are separating into two halves: the mechanical half, which is already being automated, and the judgement half, which decides what the logic means. Only one of those halves is at risk.

Written for LinkedIn as an original. It sits under the Institute's AI in project controls pillar.

## What is the planner's job, once you strip out the software?

A planner produces a defensible statement about when things will finish and what has to happen first. The programme file is the record of that statement, not the statement itself.

Four things sit underneath it. The activity logic, which encodes how the work is physically and contractually sequenced. The durations, which encode productivity assumptions. The calendars and constraints, which encode access. And the progress, which encodes what is actually true today.

Every one of those four is a claim someone has to be able to defend under questioning. That is the part of the role that has never been about software.

## How are AI and planning engineer roles splitting, task by task?

More than most planners are comfortable admitting, and none of them are the four above.

| Task | What tools do now | What still breaks | Who carries the answer |
|---|---|---|---|
| Schedule quality checks | Open ends, negative lag, excess constraints, long durations, all flagged automatically | Nothing. This is fully mechanical | The tool |
| First-draft logic from a method statement | Produces a plausible network in minutes | It does not know the access permit, the crane hire window or the crew count | Planner |
| Progress extraction from site reports and photos | Reads quantities and dates out of unstructured text at speed | Confuses claimed with installed, and installed with accepted | Planner and QS |
| Resource levelling | Optimisation across constrained resources is a solved computation | Which levelling is acceptable depends on labour agreements and subcontractor terms | Planner |
| Narrative and variance commentary | Fast, fluent, well structured | Asserts causes it has no evidence for | Planner, who signs it |
| Delay analysis data preparation | Slices windows, builds as-built comparisons | Entitlement is a contractual question, not a data one | Commercial and planner |

The pattern is consistent. Tools are strong where the answer is computable from the file, and weak where the answer depends on something that was never written into the file.

## Why can software compute the critical path but not decide it?

Because the critical path is an output of the logic, and the logic is an assertion about the world. Get the assertion wrong and the arithmetic is still perfect.

Take four activities. A, site establishment, 10 days. B, piling, 15 days, after A. C, switchgear procurement and delivery, 20 days, after A. D, erection and connection, 8 days, after both B and C.

**Forward pass.** A runs day 0 to 10. B runs 10 to 25. C runs 10 to 30. D cannot start until both predecessors finish, so D runs 30 to 38.

**Backward pass.** Project finish is day 38, so D must start by day 30. B must finish by 30, so its late start is 15. C must finish by 30, so its late start is 10. A must finish by 10.

**Total float** is late start minus early start. A = 0. B = 15 − 10 = **5 days**. C = 0. D = 0.

The critical path is A, C, D at **38 days**, and piling has five days of float that nobody would guess from the site's noise level.

Now the judgement. If B slips four days it changes nothing, because it consumes float. If B slips eight days it consumes five days of float and pushes the finish to day 41. If C slips one day the project is one day late immediately.

Every one of those statements depends on D genuinely requiring B. If that link is there because someone assumed the piling rig would still be occupying the erection area, and the rig has since been rescheduled, the whole answer is wrong and no tool will tell you.

One constraint changes it again. A finish-no-later-than date on D set to day 38 makes the reported float on B collapse towards zero and the file starts reporting five days of pressure that does not exist.

## What breaks when the schedule is generated rather than built?

The audit trail breaks first, and it breaks quietly.

A planner who built the network can say why activity 1420 has a seven-day lag. A planner handed a generated network can only say the tool produced it, which is not an answer that survives a delay claim.

The second thing to break is calibration. A model producing durations from historical data reproduces historical productivity, including the productivity of the jobs that went badly, unless somebody has separated those out.

The third is uniformity. When every planner on a programme accepts the same generated defaults, the schedules stop disagreeing with each other, and a portfolio where nothing disagrees has lost its early warning system.

A [working review protocol for machine-produced schedules](https://pciai.org/llm-schedule-review) handles all three. Regenerate nothing you cannot interrogate, require a stated source for every duration outside the historical band, and keep at least one path built by hand as the control.

## What does an employer actually pay a planner for now?

Not for driving the software. Assume the software is competent and getting more so.

They pay for the ability to say, in a meeting where money is at stake, why the date is the date. That means knowing which links are physical, which are contractual, which are convenience, and being able to point at each.

They also increasingly pay for the ability to review a machine-produced schedule properly. Checking a generated network is a harder skill than building one, because a wrong network that is internally consistent looks exactly like a right one.

That reviewing skill is why governed AI carries 20% of the Body of Knowledge's proportions across PCI's credentials, alongside 40% finance and reporting and 40% project management. The [PCI AI Project Controls Leader (PCL-AI)](https://projectcontrolsinstitute.org/pcl-ai-certification) credential examines the schedule side of that across 13 domains and 61 knowledge areas.

## Which planning roles are genuinely exposed?

The ones defined by throughput rather than judgement. If the job description is updating percent complete across 3,000 activities from a spreadsheet each Monday, that work is already being done faster by software.

The role most at risk is the junior post whose only content is data entry, which is also the post that used to teach people the network. That is the real problem in this: not job losses, but a broken training route.

The response is to move juniors onto interrogation earlier. Give them a generated schedule and ask them to find the three links that are wrong. It teaches the same thing that ten years of manual updating used to teach, faster.

## Frequently asked questions

**Will AI replace planning engineers within five years?**
There is no credible published figure for this and anybody quoting one should be asked for the sample. What can be observed is the task mix: quality checking, data extraction and levelling are automating quickly, while logic ownership, constraint justification and delay entitlement are not. Roles built on the first group shrink, roles built on the second do not.

**Can a language model build a critical path network?**
It can draft one, and the draft is often a reasonable starting sequence for standard work. It cannot verify access, permits, crew availability or the physical clashes that create half the real links. Treat the output as a first pass by a competent graduate who has never visited the site.

**Should planners learn to code or learn prompting?**
Prompting is the smaller skill and it is learned in an afternoon. The more valuable skill is structured verification: knowing what a schedule must satisfy, and testing machine output against it in a repeatable order. Basic data literacy, enough to check a pivot and a regression, pays back more than a scripting language.

**Does automation make schedule risk analysis less necessary?**
The opposite. Faster schedule production means more schedules, generated with less scrutiny per file, so the spread of possible outcomes matters more rather than less. Quantitative risk analysis is one of the few areas where more computation genuinely improves the answer, provided the correlation and duration ranges are set by someone who understands the work.

**What should a planner do this year to stay employable?**
Learn to review, not just produce. Be able to take any schedule and state within an hour which activities drive the finish, which constraints are doing hidden work, and which durations sit outside the historical band. That skill is scarce, it is portable, and no tool currently offers it as a finished product.

---

*PCI publishes certification requirements. Nothing here is legal or contractual advice, and no claim is made about employment outcomes.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Internal links: two links are in the body, on two different domains. "A working review protocol for machine-produced schedules" points to https://pciai.org/llm-schedule-review, in the paragraph that lists the three things a generated network breaks and says a protocol handles all three. "PCI AI Project Controls Leader (PCL-AI)" points to https://projectcontrolsinstitute.org/pcl-ai-certification, because the credential itself is the hub's territory and the sentence asks what it examines. The standfirst pillar link was removed: with the protocol link already going to that domain, a second one is a tell rather than a help. Reciprocal: https://pciai.org/will-ai-replace-planning-engineers could cite this piece for the four-activity float example.*
