---
platform:      Own site — projectcontrolsinstitute.org
type:          guide
title:         Project performance management: metrics to decisions
meta:          Project performance management: the metric set worth reporting, thresholds and owners, the critical path trap, and how to score an early-warning model.
primary_kw:    project performance management
secondary_kw:  leading indicators, critical path, precision and recall, forecast stability
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,905
hashtags:      n/a (own site)
ab_id:         AB-00101
---

# Project performance management: metrics to decisions

Project performance management is the practice of turning measurement into decisions: choosing a small set of metrics, setting a threshold on each, naming who acts when it is crossed, and recording what they did. A metric with no threshold and no owner is a cost, not a control.

Most reporting packs fail on the second half of that sentence rather than the first.

## The test a metric has to pass

Before a number goes on a report, answer three questions about it. What decision does it change, who makes that decision, and at what value do they have to make it.

If a metric cannot survive those questions it belongs in an appendix or nowhere. A forty-page monthly pack containing four numbers that could change behaviour is worse than a four-page pack containing the same four, because the signal is buried and everyone learns to skim.

The reason this matters more than presentation: reports are read under time pressure by people deciding whether to intervene. Anything that delays the intervention has cost money.

## The project performance management metric set that earns its place

| Metric | Question it answers | Decision it triggers | How it gets gamed |
|---|---|---|---|
| Cumulative CPI and its trend | Are we converting money into work at the planned rate? | Change method, crew mix or scope | Generous earning rules; level-of-effort scope |
| Period CPI | Did this month go badly? | Immediate supervisory action | Timing of accruals across the cut-off |
| SPI(t) in weeks | How far behind are we, in time? | Re-sequence, accelerate, or accept | Baseline rebaselined to today |
| TCPI against BAC | Is the budget still achievable? | Release contingency or reforecast | Comparing TCPI to an EAC built from CPI |
| EAC movement period on period | Is the forecast stable? | Challenge the inputs | Small monthly moves that never sum to the truth |
| Float erosion on the top paths | Is the finish date under pressure? | Protect or re-plan the driving path | Reporting only the single named critical path |
| Unapproved change ageing | Is scope being delivered before it is authorised? | Escalate to the client or stop work | Instructions recorded late |
| Accrual completeness | Is the cost report real? | Withhold the forecast until fixed | Nothing; this one is usually just missing |

Eight metrics is roughly the practical ceiling for a monthly cycle. Beyond it, the reviewing group runs out of attention before it runs out of pages.

## Leading and lagging, and why most packs are all lagging

CPI, SPI and actual cost describe work that has already happened. They are lagging by construction, and by the time they move the money has gone. [The measurement system these indices come from](https://projectcontrolsinstitute.org/earned-value-management) explains how each one is built.

Leading indicators describe the conditions that produce next month's CPI. Drawing approvals outstanding, permits not yet granted, materials with no confirmed delivery date, vacancies against the planned resource curve, rework raised as a proportion of work inspected.

A report with no leading indicators can only explain. A report with a few can occasionally prevent, and the ratio between the two is a reasonable measure of how mature a controls function is.

## The schedule side: float, the critical path, and why the list moves

The critical path is the longest continuous sequence of activities through the network. It sets the earliest possible finish, and it carries the least total float.

Total float is the time an activity can slip without moving the project finish. It equals late start minus early start, which is the same as late finish minus early finish.

Take a small network. Activity A takes 10 days and precedes both B and C. Path one is A → B (15) → D (20) → F (8). Path two is A → C (12) → E (18) → F (8).

Path one totals 10 + 15 + 20 + 8 = **53 days**. Path two totals 10 + 12 + 18 + 8 = **48 days**. So path one is critical, and every activity on path two carries 53 − 48 = **5 days** of total float.

Now let C slip by seven days. Path two becomes 55 days and path one stays at 53. The project is now two days longer than planned, the critical path has moved to A → C → E → F, and B and D have acquired two days of float.

This is why a performance report that tracks a frozen list of critical activities goes quietly wrong. Watch every path carrying less than fifteen days of float, and let the network tell you which one is driving. Watching and acting are different bands. Fifteen days is where a path earns a line on the report; ten days is where someone has to act on it. Setting the watch band above the trigger is what makes pressure visible for a period or two before a decision is forced.

Two habits corrupt the measure. Constraints applied to hold a date, which manufacture float that does not exist, and negative lags used to compress a sequence nobody intends to work that way.

## What to demand of an early-warning model

Once a model sits on top of these metrics, whether it is a rule set or something trained, it becomes a thing that must itself be measured. Whether such a model is worth having at all is a separate question, and [what AI can and cannot be trusted with in project controls](https://pciai.org/ai-in-project-controls) is where that argument, and the scoring arithmetic behind it, is set out.

Ask for three things before you believe any flag rate. Precision: of the accounts it flagged, how many really overran. Recall: of the accounts that really overran, how many it flagged. And the confusion matrix both are calculated from, on completed accounts rather than open ones.

Never accept accuracy as the headline. On a portfolio where a quarter of accounts overrun, a model that flags nothing at all is right three times in four, and a number that cannot fail is not a measurement.

Where the alert threshold sits is a business decision rather than a modelling one. Loosening it catches more of the real overruns and raises the number of false alarms, which is defensible on a capital programme, because a missed overrun costs more than an unnecessary review. It is only defensible if the reviews are staffed. Unstaffed alerts are ignored within two cycles, and an ignored alert is worse than no alert at all.

## From metric to decision: write it down as four columns

The artefact that makes this real is short. For each metric: the trigger value, the owner, the decision they are authorised to make, and the deadline.

| Metric | Trigger | Owner | Decision | By |
|---|---|---|---|---|
| Period CPI | Below 0.90 for two consecutive periods | Package manager | Method or crew change, or escalate | 5 working days |
| EAC movement | Moves by more than 2% of BAC | Cost manager | Written explanation of the driver | With the report |
| Float erosion | Any path drops below 10 days | Planner | Re-sequence proposal to the PM | 10 working days |
| Unapproved change | Instruction over £25k older than 30 days | Commercial lead | Escalate to client or stop the work | 5 working days |

Thresholds are set at baseline and not adjusted at month-end. A threshold moved to avoid writing a report is a governance failure, and it is visible in the audit trail whether or not anyone looks.

Choosing the trigger values themselves is the harder half of that table. [How a percentage gate and an absolute gate are sized together](https://projectcontrolsinstitute.org/earned-value-reporting-thresholds) works through it account by account, including why a percentage on its own floods a small account and misses a large one.

## The number that has to reconcile

One discipline separates a performance report that survives scrutiny from one that does not. The cost position on the report has to reconcile to the ledger, every period, with the differences named.

Accruals, retention, materials on site and unapproved variations account for almost all of it. Where the reconciliation is not produced, the two records drift apart a little each period until neither side trusts the other's number, and the gap is normally found in an audit rather than at a month-end.

That crossing point is why project accounting and finance sit next to the delivery disciplines in the Body of Knowledge behind the PCI AI Project Controls Leader (PCL-AI), rather than in a chapter of their own: [how its 13 domains and 61 knowledge areas are divided](https://projectcontrolsinstitute.org/body-of-knowledge) puts them in a 40 / 40 / 20 proportion with governed AI.

## Frequently asked questions

**How many metrics should a monthly report carry?**
Between six and ten for a project, and fewer for a portfolio roll-up. The constraint is attention rather than availability, since most systems can produce hundreds. A useful test is whether the review meeting reaches the last metric on the list before it runs out of time; if it never does, the list is too long.

**What is the difference between a KPI and a metric here?**
A metric is any measurement. A KPI is a metric that someone is accountable for, with a target and a consequence attached. Most organisations call everything a KPI, which drains the term of meaning, and the practical fix is to keep a short list of genuinely accountable measures and label the rest as reporting.

**Should performance metrics be tied to individual bonuses?**
Be very careful. Any metric attached to pay is optimised, and earned value metrics are unusually easy to optimise without doing any work, mainly through earning rules and the timing of accruals. If you do it, tie the reward to the reconciliation and the forecast quality rather than to the index itself.

**How do I measure forecast quality?**
Track the movement of the EAC over time and compare each forecast to the eventual outturn once the package closes. A forecasting function that is consistently optimistic by a similar margin is more useful than one that is unbiased but wildly variable, because a known bias can be corrected and noise cannot.

**Does any of this change with AI in the workflow?**
The metrics do not change. What changes is that you now have a second thing to measure, since the model that flags accounts or drafts explanations has its own precision and recall, and it drifts as the portfolio changes. Score it on a held-out set of completed accounts, and re-score it at least annually.

---

*Internal linking note: three same-domain links now sit in the body. "How its 13 domains and 61 knowledge areas are divided" points at the Body of Knowledge from the reconciliation section, where the finance and delivery overlap is named. Two were added: "the measurement system these indices come from" points at the earned value pillar, placed where CPI, SPI and actual cost are called lagging by construction and a reader may not yet know how they are built; and "how a percentage gate and an absolute gate are sized together" points at the reporting thresholds guide, placed under the metric-trigger-owner table, which sets trigger values without explaining how to choose them. That anchor is deliberately about the sizing mechanics, since the cheat sheet already points at the same page on the question of when a variance should trigger action. One cross-estate link is carried: "what AI can and cannot be trusted with in project controls" to pciai.org, placed where an early-warning model first appears, because whether to run one at all is that domain's subject rather than the hub's. Reciprocal: the reporting thresholds guide should link back here with an anchor about turning measurement into decisions.*
