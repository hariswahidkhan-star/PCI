---
platform:      Own site — projectcontrolsinstitute.org
type:          template
title:         Primavera P6 practice test: a free tool and how to use it
meta:          A free Primavera P6 practice test: 18 questions in four tiers with worked answers, a marking scheme that separates method from arithmetic, and a study loop.
primary_kw:    primavera p6 practice test
secondary_kw:  p6 exam questions, total float calculation, retained logic, schedule marking scheme
pillar:        Certification and careers
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1798
hashtags:      n/a (own site)
ab_id:         AB-00069
---

# Primavera P6 practice test: a free tool and how to use it

A useful Primavera P6 practice test makes you calculate a network by hand, then explain what the software did to it. A weak one asks where a menu lives. The test below is free, sits on this page, and runs in four tiers: calculation, tool behaviour, diagnosis and judgement. Answers follow each tier.

The tool is this page plus a score sheet you can rebuild in a spreadsheet. Nothing to download or register for.

## What should a Primavera P6 practice test cover?

Six skill areas.

| Skill area | A weak question | A question worth answering |
|---|---|---|
| Network arithmetic | What does CPM stand for? | Calculate the finish date and the float on this network |
| Calendars | Where do you set a calendar? | The activity spans a shutdown week: what happens to its float? |
| Progress rules | What is the data date? | Work happened out of sequence: which setting did you report on? |
| Constraints | List the constraint types | This constraint left a path with slack showing zero float: explain |
| Integration | How do you assign resources? | The schedule says 62% and the cost report says 47%: reconcile them |
| Judgement | What is a baseline? | Defend this date to a client who wants six weeks removed |

The integration row is where most planners lose marks, and [a matching set of earned value practice problems](https://projectcontrolsinstitute.org/earned-value-practice-questions) drills the cost side of that reconciliation.

## Tier one: the network

Work these by hand first. Durations are working days on one calendar, all relationships finish-to-start with no lag. If the arithmetic is not yet automatic, [the forward and backward pass in full](https://projectcontrolsinstitute.org/critical-path-method) works a seven-activity network through both directions before you start.

| Activity | Duration | Predecessors |
|---|---:|---|
| T1 Survey | 4 | — |
| T2 Excavate | 10 | T1 |
| T3 Foundations | 12 | T2 |
| T4 Steel delivery | 25 | T1 |
| T5 Erect steel | 8 | T3, T4 |
| T6 Roof | 6 | T5 |

**Q1.** What is the project duration?

**Q2.** What is the critical path?

**Q3.** What is the total float on T2 and T3?

**Q4.** What is the free float on T2 and on T3, and why do they differ?

**Q5.** Steel delivery slips to 30 days. What happens to the finish and to the float on the foundations chain?

### Tier one answers

**A1. 43 days.** Forward pass: T1 runs 0–4. T2 runs 4–14. T3 runs 14–26. T4 runs 4–29. T5 needs both T3 and T4, so it runs 29–37. T6 runs 37–43.

**A2. T1–T4–T5–T6**, which is 4 + 25 + 8 + 6 = 43. The concrete chain is not the driver; procurement is.

**A3. Three days each.** Backward pass: T5 must start by 29, so T3 must finish by 29 but finishes at 26. Total float = late start − early start = 17 − 14 = 3 for T3, and 7 − 4 = 3 for T2.

**A4. T2 has zero free float; T3 has three days.** Free float is the delay an activity can absorb without moving its successor's early start. T3 starts at 14 and T2 finishes at 14, so nothing is spare. T5 starts at 29 and T3 finishes at 26.

The three days belong to the chain, not to T2. That is interfering float: if T2 uses them, T3 loses them, and [how total float and free float differ](https://projectcontrolsinstitute.org/total-float) sets the three measures out side by side.

**A5. The project finishes in 48 days, five late, and the foundations chain gains float.** T4 now runs 4–34, T5 runs 34–42, T6 runs 42–48. T3 must now finish by 34 but still finishes at 26, so its total float rises from 3 to 8. Float belongs to a path, not to an activity.

## Tier two: what the software is doing

**Q6.** What does the data date represent, and what should never appear to its left?

**Q7.** Explain retained logic and progress override in one sentence each.

**Q8.** Duration percent complete, physical percent complete and units percent complete: which one should drive earned value?

**Q9.** A path with genuine slack shows zero total float. What has probably been applied?

**Q10.** Two activities on different calendars sit on the same path. Why can their float figures look inconsistent?

### Tier two answers

**A6.** The data date is the cut-off for the update: everything left of it is history, everything right of it is forecast. No remaining work belongs to its left, and no actual progress belongs to its right.

**A7.** Retained logic holds the unfinished part of an activity behind its incomplete predecessors. Progress override releases the remaining work as though the missing predecessor no longer governed it.

The same update run under both can give different completion dates. Name the setting in the narrative; the reader cannot tell from the bar chart.

**A8.** Physical percent complete, assessed against rules of credit. Duration percent complete measures time elapsed, which is the schedule reporting progress to itself, and units percent complete measures resource consumption, which is nearer to actual cost than to earned value.

**A9.** A constraint, most often a "Finish On or Before" date, which caps the late dates on everything feeding it. The float did not vanish; it was never released to the network.

**A10.** Float is expressed in working days on the relevant calendar. A 7-day and a 5-day calendar report different float for the same physical slack, so compare float across calendars only after converting it.

## Tier three: diagnosis

**Q11.** A programme has 62% of its relationships as start-to-start. What is the likely problem?
**Q12.** One activity carries 214 days of total float. What do you check?
**Q13.** A finish-to-start relationship has a lag of −10 days. What is wrong with it?
**Q14.** An activity has been 40% complete for three consecutive updates and its remaining duration has not changed. What is happening?

### Tier three answers

**A11.** The network has been drawn to make the bars look right rather than to model the work. Start-to-start links without matching finish-to-finish links leave activities with no logical end, so durations set the finish date instead of sequence.

**A12.** Whether it is detached from the network. Very high float usually means a missing successor or an unlinked milestone, not 214 days of real slack.

**A13.** A negative lag is a lead: the successor starts before the predecessor finishes. It hides the overlap, cannot be resourced honestly, and breaks when the predecessor's duration changes. Model the overlap with a start-to-start link and a positive lag.

**A14.** The percentage is being typed and the remaining duration is not being re-estimated. Progress is being reported as an opinion. Ask what quantity was installed in the period and derive the percentage from it.

## Tier four: judgement

**Q15.** A client asks for six weeks off the programme. What can you offer, and what must you refuse?

**Q16.** Half the progress this month happened out of sequence. Which setting do you report on?

**Q17.** The deterministic date is week 84 and the P80 date is week 91. Which one goes in the report?

**Q18.** Your forecast finish has just crossed the date that triggers liquidated damages. Who hears about it, and when?

### Tier four answers

**A15.** Offer sequence changes, extra crews with their costs, and a re-scoped completion boundary, each priced with its consequence. Refuse to shorten a duration that came from a rate without changing the rate or the resource. A cut with nothing behind it is a promise made with someone else's labour.

**A16.** Retained logic as the default, with the progress override result shown alongside if the difference is material. The setting that flatters the date is not the setting to lead with.

**A17.** Both, labelled. The deterministic date is what the network says if every duration behaves; the P80 is the date you would commit to. Reporting only the first is optimistic, and only the second hides the driver.

**A18.** Commercial and finance, in the week you see it, before the report goes out. A forecast that crosses a contractual trigger is an accounting event as much as a scheduling one, because it changes what has to be provided for at period end.

## How should the test be marked?

Split the marks, because a right answer for the wrong reason fails a real review.

| Component | Weight | What earns it |
|---|---:|---|
| Method | 50% | The right approach chosen and named before any arithmetic |
| Arithmetic | 20% | The numbers correct and checkable |
| Assumption | 20% | The assumption stated in one sentence, and it is the right one to worry about |
| Communication | 10% | An answer a sponsor could act on without a translator |

Score each tier separately. A rising arithmetic score with a flat method score means you are only getting faster at the easy half.

## If you use an AI marker, score the marker

Automated schedule checkers flag defects and are wrong often enough to need measuring. The standard measures are precision, recall and F1. How to run one of these reviews without handing it the judgement is set out in [a protocol for reviewing a schedule with a language model](https://pciai.org/llm-schedule-review).

Take a checker that flags **48** relationships as defective. **36** flags are genuine, and the schedule actually contains **45** defective relationships.

Precision = 36 ÷ 48 = **0.750**, so a quarter of what it flags is noise. Recall = 36 ÷ 45 = **0.800**, so it missed nine real defects. F1 is the harmonic mean: 2 × (0.750 × 0.800) ÷ (0.750 + 0.800) = 1.200 ÷ 1.550 = **0.774**.

A tool at 0.75 precision is worth running and not worth trusting. Use it to shorten the search, then check the flags yourself.

## A four-week loop

Week one, work tiers one and two by hand, then rebuild the network in the software to check yourself.

Week two, run tier three against a live schedule, writing one sentence per defect you find.

Week three, answer tier four out loud to someone who will argue. Week four, redo tier one cold and compare your method score against week one.

## Frequently asked questions

**Is this practice test tied to a particular version of P6?**
No. The arithmetic in tiers one and three is version-independent, and the tier two behaviours have been consistent across recent versions. Menu positions change between releases, which is one reason the test ignores them. A licence helps with tier two and is not needed elsewhere.

**How long should the test take?**
About 90 minutes for all four tiers if you are working the network by hand, which you should be. Tier one is 25 minutes of arithmetic, tier two is quick if you know the settings and impossible if you do not, and tier four deserves the most time.

**What score means I am ready for an examination?**
Consistent method marks above 80% across two cold attempts a fortnight apart, with tier four answered without hedging. A single high score on a test you have already seen measures recall of this page rather than competence.

**Does passing this mean I can build a schedule?**
No. It means you can read one and challenge it, which is the harder half of the job and the half most tool training skips. Building competence comes from producing a network from drawings and quantities and having it marked.

---

*Internal linking note: three same-domain links now sit in the body. "The forward and backward pass in full" points at the critical path method definition, placed in the tier one instructions, where a candidate about to work a network by hand needs the method rather than the answer. "How total float and free float differ" points at the total float definition, placed in answer A4, which is the exact point where free float, total float and interfering float have to be told apart. "A matching set of earned value practice problems" points at the earned value practice questions, placed under the skill-area table, whose integration row asks the reader to reconcile a schedule percentage with a cost percentage. The anchor was changed from the target's bare keyword so the two pages do not share an anchor. One cross-estate link is carried: "a protocol for reviewing a schedule with a language model" to pciai.org, placed at the AI-marker section, since governed use of a model is that domain's subject. The fourth proposal, a PCL-AI link on "how scheduling is examined", was dropped to stay inside the internal cap. Reciprocal: the P6 online course piece should link back here with an anchor about testing your own network arithmetic.*
