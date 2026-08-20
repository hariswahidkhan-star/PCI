---
platform:      Own site — pciworld.org
type:          qa-list
title:         Planning engineer interview questions: 20 that come up
meta:          Planning engineer interview questions with strong answers: float, retained logic, out-of-sequence progress, delay analysis, risk and what AI may not sign.
primary_kw:    planning engineer interview questions
secondary_kw:  total float, retained logic, time impact analysis, schedule risk analysis
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: pciworld.org
canonical:     original
schema:        FAQPage
word_count:    2240
hashtags:      n/a (own site)
ab_id:         —
---

# Planning engineer interview questions: 20 that come up

Planning engineer interview questions fall into four groups: network logic and float, statusing and progress measurement, delay and risk, and how the programme reaches the cost report. Twenty of the ones that genuinely come up are below, each with the answer an interviewer is listening for and the arithmetic where it matters.

Nobody is testing whether you can recite a definition. They are testing whether you have ever had to defend a date.

| Group | Questions | What is really being tested |
|---|---|---|
| Network logic and float | 1–6 | Whether you understand the network or only the software |
| Statusing and progress | 7–11 | Whether your reporting would survive an audit |
| Delay, risk and recovery | 12–16 | Whether you can act, not just describe |
| Cost, finance and AI | 17–20 | Whether the programme connects to money |

## Planning engineer interview questions on network logic and float

**1. What is the critical path, and is it the same as the longest path?**
The critical path is the set of activities with no total float: delay one and the completion date moves. In a single-calendar network with no constraints it is also the longest path. Add multiple calendars, constraints or negative lags and the two can diverge, at which point the longest path is the more reliable answer to what is driving the date.

**2. Explain total float and free float, with numbers.** Total float is how long an activity can slip before the completion date moves. Free float is how long it can slip before its own successor moves.

An activity finishing on day 28 whose only successor starts on day 28 has zero free float even if it carries 25 days of total float. It can push everything downstream immediately while the end date still looks safe.

**3. What does negative float mean, and what causes it?**
Negative float means the network cannot meet an imposed date — the late dates fall before the early dates. The usual causes are a constraint tighter than the logic supports, a milestone imposed after the programme was built, or genuine slip against a contract date. Negative float is not an error to be deleted; it is the network reporting that the commitment and the plan disagree.

**4. Which constraints would you accept in a programme?**
As few as possible. A start-no-earlier-than on a real access or approval date is defensible. Finish-no-later-than and mandatory dates are the ones that hide logic and manufacture float. Every constraint should carry a written reason in the activity notes, because an unexplained constraint discovered at claim time damages the credibility of the whole programme.

**5. Retained logic or progress override?**
Both handle work done out of sequence. Retained logic keeps the original relationships, so remaining work still waits for its predecessor — more conservative and easier to defend. Progress override ignores the relationship for the started activity, which usually shortens the programme. Know which setting your project uses, because changing it moves the completion date without anyone changing the plan.

**6. How would you spot a calendar problem?**
Compare the duration in working days against the elapsed span between start and finish. A five-day activity spanning nine calendar days is a calendar effect, not a delay. Check that curing, testing and commissioning sit on calendars that match reality, and that no global calendar edit has quietly shifted a thousand dates.

## Statusing and progress measurement

**7. Walk me through statusing a programme.** Fix the data date first and do not move it. Collect actual starts, actual finishes and remaining durations rather than percentages.

Enter the actuals, review out-of-sequence work, then let the network recalculate before touching anything else. Only after that do you investigate the movement. Statusing towards a wanted date, instead of statusing and reporting what falls out, is the integrity failure interviewers probe for.

**8. How do you measure progress on partly built work?**
With a rule published before the work starts. On a 20 km pipeline budgeted at £2.00m with 8 km laid and 5 km tested, a units-installed rule gives 8/20 × 2.00 = **£0.80m** of earned value. A 60/30/10 rule across lay, test and commission gives (0.40 × 0.60) + (0.25 × 0.30) = 0.315, so 0.315 × 2.00 = **£0.63m**. The £170,000 difference is a policy choice, not progress.

**9. What is the difference between percent complete and percent spent?**
Percent complete is earned value over budget at completion — how much work is done. Percent spent is actual cost over budget — how much money is gone. With a £16.0m budget, £6.6m earned and £7.4m spent, that is 41.25% complete against 46.25% spent. The five-point gap is £0.8m of budget that bought no work, and reporting percent spent as progress is the classic error.

**10. How do you handle out-of-sequence progress?**
Find it, then fix the cause rather than the symptom. Work done out of sequence usually means the logic was wrong, not that the site was. Decide whether the relationship should change permanently or the activity should be split, and record the decision. Quietly switching to progress override so the dates improve is the wrong answer, and this question exists to hear it.

**11. What makes a programme poor quality?**
Open ends that leave activities floating free of the network. Long lags doing the work that logic should do. Excessive constraints. Activities longer than the reporting period, which cannot be statused meaningfully. Level of effort dressed up as discrete work, which earns to plan by definition and flatters the schedule performance index. Any of these on a driving path turns the completion date into an opinion.

## Delay, risk and recovery

**12. Which delay analysis method would you use?**

| Method | What it does | When it fits | Weakness |
|---|---|---|---|
| Impacted as-planned | Adds delay events to the baseline network | Prospective assessment, before the effect is known | Ignores what actually happened on site |
| Time impact analysis | Inserts the event into the update current at the time | Contemporaneous, event by event | Only as good as the update it is run on |
| As-planned versus as-built windows | Compares planned and actual across periods | A full contemporaneous record exists | Labour-intensive and heavy on judgement |
| Collapsed as-built | Removes delay events from the as-built network | No reliable baseline survives | Building an agreed as-built network is contentious |

Name the method the contract or the protocol in use points to, then name the records you would need. A method quoted without the evidence behind it is the answer that fails.

**13. What is concurrent delay?**
Two or more delays taking effect in the same period, typically one the contractor's and one the employer's, both bearing on the completion date. It usually decides whether time is granted with money or without. The strong answer names the ambiguity: true concurrency is rare, apparent concurrency is common, and the outcome turns on the contract and the jurisdiction rather than on the network alone.

**14. How do you run a schedule risk analysis, and what does P80 mean?**
Build a sound network first, because a risk model on bad logic is worse than no model. Apply duration ranges drawn from evidence, add discrete risk events with probability and impact, correlate only what genuinely correlates, then run the simulation. P80 is the duration you would meet or beat in four runs out of five. Deterministic 320 days against P50 338 and P80 366 makes the 46-day gap the contingency conversation.

**15. How would you build a recovery plan?**
Only from the driving path. Re-sequence first because it costs nothing, then consider additional resource, then additional shifts, then deferring scope. Attach a cost and an assumed recovery rate to each option, and a review gate that tests the assumed rate against the actual one. A recovery achieved on paper by compressing durations with no resource change is the version everyone in the room has watched fail.

**16. How would you tell a client the date has moved?**
Early, with the cause, the options and a recommendation. Lead with the movement and the driver rather than the mitigation. Bring the arithmetic: what the slip costs, what each option costs, what exposure remains. The failure mode is holding the news until the monthly report, by which time the client has heard it from site and the programme has lost its authority.

## Cost, finance and AI

**17. How does the programme reach the cost report?**
Through resource loading and the earning rules. The schedule sets when value is planned, the rules set when it is earned, and the ledger supplies what was spent. The schedule performance index falls straight out as earned value divided by planned value. If the programme and the cost report show different progress, one of them is applying a rule the other has never been told about.

**18. Is the schedule performance index a good measure of schedule health?**
Only early, and only with care. It is measured in money rather than time, and it drifts back towards 1.0 as a project completes, because earned value must eventually equal planned value even on a late job. Earned schedule expresses the same idea in time units. Quote the index alongside the driving path, never instead of it.

**19. What would you not let an AI tool do to a programme?**
Sign anything. As a reviewer it is genuinely useful for flagging open ends, odd lags and out-of-sequence work. Judge it as you would any classifier: if it flags 240 activities and 168 are real, precision = 168 ÷ 240 = **0.70**; if 55 real defects were missed, recall = 168 ÷ 223 = **0.753**; F1 = (2 × 0.70 × 0.753) ÷ 1.453 = **0.726**. Nearly a third of flags are noise and 55 defects still slipped through, so the tool triages and a planner decides.

**20. Tell me about a programme you got wrong.** Answer it properly. Name the call, why it was wrong, what it cost and what you changed in your method afterwards.

Interviewers ask this to find out whether you have ever taken a position, because a planner who has never been wrong in public has never truly committed to a date. A polished non-answer does more damage here than the mistake ever did.

## Where a credential fits

Questions 17 to 19 are where most planning candidates lose ground, and they are all on the boundary between delivery and money: earning rules that become reported progress, an index that gets quoted to a board, a model whose error rate nobody has measured.

PCI examines that boundary deliberately. The PCI AI Project Controls Leader (PCL-AI) credential covers 13 domains and 61 knowledge areas, with a Body of Knowledge proportioned **40 / 40 / 20** across finance and reporting, project management and governed AI, and resting on **113 mandatory PCI Standards carrying 532 process requirements**.

For the deeper treatment of the float questions above, see [what total float really means](https://projectcontrolsinstitute.org/total-float); for the risk questions, [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis).

## Frequently asked questions

**How technical is a planning engineer interview?**
More technical than most candidates expect and less than they fear. Expect to be asked to define float and to explain a real programme you built, usually with the interviewer probing one answer until you either show depth or run out. Preparation that consists of memorised definitions falls apart at the second follow-up question.

**Will I be given a practical test?**
Often, on capital projects. The common formats are a short network to analyse by hand, a schedule file with deliberate defects to find, or a written narrative from a set of variances. Practise the hand calculation, because it is the one people skip and the one that exposes reliance on the software fastest.

**How much Primavera P6 do I need to show?**
Enough to build, status and report without help, and enough to say what the tool can hide — a constraint doing the work of logic, an actual start dated in the future, a calendar stretching a five-day activity across nine. If P6 dominates your market, working through [a Primavera P6 practice test](https://projectcontrolsinstitute.org/primavera-p6-practice-test) beforehand is a better use of an evening than rereading a manual.

**What should I ask them?**
Who owns the baseline and who can change it. How progress is measured and whether the rules are written down. Whether the planner reports to delivery or to controls. The answers tell you whether the role is a real one or a reporting seat, and asking them signals that you have held the job rather than watched it.

**What if I am asked something I have never done?**
Say so, then say what you would do and what you would need to find out. Interviewers at this grade are calibrated for it, and an honest boundary followed by a method is a much stronger answer than an invented one. Bluffing on delay analysis in particular is usually detected in a single follow-up.

---

*Internal links: link to [what total float really means](https://projectcontrolsinstitute.org/total-float), [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) and [a Primavera P6 practice test](https://projectcontrolsinstitute.org/primavera-p6-practice-test), each with that anchor; the how to become a planning engineer and project controls interview questions pieces should link back here with the anchor "planning engineer interview questions".*
