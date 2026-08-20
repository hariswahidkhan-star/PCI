---
platform:      Substack
type:          faq
title:         What does a CPI below 1 mean, and what should you do next?
meta:          CPI below 1 meaning, in plain terms: you are earning less budgeted work than you spend. How to test whether it is real, and what to do before the date slips.
primary_kw:    CPI below 1 meaning
secondary_kw:  cost performance index, cost variance, to-complete performance index, control account
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1,826
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00265
---

# What does a CPI below 1 mean, and what should you do next?

A CPI below 1 means you are earning less budgeted work than you are spending. Cost performance index is earned value divided by actual cost, so a CPI of 0.87 says every £1.00 spent has bought 87p of budgeted work. It is a measurement of the past, and on most projects it is also a forecast.

*Written first for this newsletter. The project below is invented so the arithmetic can be checked line by line.*

## CPI below 1 meaning: the formula and a worked example

**In one sentence:** the work you have earned is worth less than the money you have spent to earn it.

Cost performance index is earned value divided by actual cost, both measured to the same cut-off and on the same scope.

Take a building project with a BAC of £62.0m, reporting at the end of month 11. Earned value is £18.6m, actual cost £21.4m and planned value £20.1m.

CPI = 18.6 ÷ 21.4 = **0.869**. Cost variance = EV − AC = −**£2.8m**. SPI = 18.6 ÷ 20.1 = **0.925**, and schedule variance = EV − PV = −**£1.5m**.

So the project is behind on cost by £2.8m of budgeted work, and behind on programme by £1.5m of budgeted work. The two are different currencies of the same problem and should never be added together.

Three things a CPI below 1 does not mean. It does not mean the project is over budget at completion, because that depends on the forecast.

It does not mean the team is working badly, because a bad estimate produces the same number. And it does not mean money is missing, because an incomplete actual cost produces the opposite error.

## Is the CPI real, or is it an accrual problem?

Before acting, test the two inputs. Most CPI movements that reverse the following month were never performance at all.

Actual cost is the usual culprit. If goods received and work done are not accrued, AC is understated and CPI is flattered; if a large payment lands early against work not yet installed, AC is overstated and CPI drops for a month.

Earned value is the other. A CPI below 1 caused by a percentage-complete claim made optimistically two months ago corrects itself the moment someone measures the quantity properly, and it looks like a performance collapse when it does.

| Symptom | Likely cause | Test |
|---|---|---|
| CPI falls one month, recovers the next | Accrual timing or a large payment out of phase | Rebuild AC on a goods-received basis and re-run |
| CPI drifts down slowly over four or more months | Real productivity or rate variance | Compare hours per unit installed against the estimate |
| CPI collapses on one control account only | An earning rule that does not match how the work is done | Check whether the rule is 0/100, 50/50 or units complete |
| CPI below 1 with SPI above 1 | Acceleration, overtime, extra crews | Look at the labour hours per week, not the cost |

Do this before the report goes out, not after somebody asks about it in the meeting.

## Cumulative CPI or period CPI: which one is telling you something?

Cumulative CPI is stable and slow. Period CPI is noisy and early. You need both, and reporting only the cumulative one hides a deterioration for months.

On the same project, this month's movement was EV £1.62m against AC £2.25m, so period CPI = 1.62 ÷ 2.25 = **0.720**.

Last month's cumulative position was EV £16.98m against AC £19.15m, so cumulative CPI was 16.98 ÷ 19.15 = **0.887**. It has fallen to 0.869 this month, a drop of 0.018.

The cumulative number moved by less than two points while the month itself ran at 0.72. That gap is the whole argument for putting period CPI on the report: the cumulative figure is averaging in ten months of better performance that will never come back.

## Where is the money actually being lost?

A project-level CPI tells you there is a problem and nothing about where it is. Decompose to control accounts, and rank by money rather than by index.

| Control account | EV (£m) | AC (£m) | CV (£m) | CPI |
|---|---:|---:|---:|---:|
| Earthworks | 4.20 | 4.05 | +0.15 | 1.037 |
| Piling | 2.10 | 2.95 | −0.85 | 0.712 |
| Structural steel | 7.60 | 8.90 | −1.30 | 0.854 |
| M&E first fix | 3.30 | 4.10 | −0.80 | 0.805 |
| Site preliminaries | 1.40 | 1.40 | 0.00 | 1.000 |
| **Total** | **18.60** | **21.40** | **−2.80** | **0.869** |

Piling has the worst index at 0.712, but structural steel is losing the most money at −£1.30m. Both matter, for different reasons.

Steel is where the recovery effort goes, because that is where the money is. Piling is where the estimating review goes, because an index that bad on a completed trade usually means the rate was wrong rather than the crew.

Report both columns. A ranking by CPI alone sends the team to the smallest problem with the loudest number.

## What does a CPI below 1 do to the forecast and the date?

Two calculations, both short, both worth putting in the same report as the index itself.

Forecast: EAC = BAC ÷ CPI = 62.0 ÷ 0.869 = **£71.3m**, so the variance at completion is −£9.3m if current performance continues. That is one of four defensible methods, and it is the one that assumes the least about the future.

Credibility: TCPI to finish on budget = (BAC − EV) ÷ (BAC − AC) = 43.4 ÷ 40.6 = **1.069**. Against a delivered CPI of 0.869, that is 1.069 ÷ 0.869 = **1.23**, a 23% improvement demanded from here to the end. Say that out loud in month 11 and the conversation changes.

Then check the schedule, because a cost variance sitting on the critical path costs twice. If the piling overrun is also driving a six-week delay, the time-related cost keeps running: site preliminaries at £1.40m over 11 months is roughly £127k a month, so six weeks adds about **£191k** that has not appeared in any control account yet.

The critical path is what makes that arithmetic real. A variance on an activity with 30 days of float is a cost problem; the same variance on a zero-float activity is a cost problem plus every day of preliminaries, plant hire and supervision behind it.

## What should you do about a CPI below 1?

Validate the inputs first, as above. Acting on a number that reverses next month costs credibility you will need later.

Write down the cause in one sentence per control account. "The piling rate was taken from a different ground condition" is actionable. "Productivity is below plan" is a restatement of the index.

Re-forecast with the method that matches the cause, and show the range rather than a single figure. [Which forecast method matches your cause](https://projectcontrolsinstitute.org/four-eac-formulas) is the whole decision, because a forecast with no visible alternatives cannot be challenged, and one that cannot be challenged should not be trusted.

Fix the estimating basis for work not yet started, which is usually the largest available recovery. Nothing you do to a completed trade recovers its money; what the analysis buys you is the chance not to repeat the error on the next twelve.

Tell finance in the month the forecast crosses the contract price, not at year end. Where an expected loss on a contract becomes apparent, the applicable reporting standards generally require it in full in that period rather than spread forward, and the controls team is normally the first to know.

Record the decision. PCI examines against 113 mandatory PCI Standards carrying 532 process requirements; they are certification requirements established by the Institute, not law, and the habit they build is that a forecast movement carries a written cause and an owner.

Reading a cost variance and carrying it into the accounts sits across the delivery and finance domains of the PCI AI Project Controls Leader (PCL-AI) syllabus, which covers 13 domains and 61 knowledge areas.

## Frequently asked questions

**What is a good CPI?**
Between about 0.95 and 1.05 with a stable trend, on a project where earned value is measured rather than claimed. A CPI of 1.15 deserves the same scrutiny as one of 0.85, because it usually means the earning rules are generous or the estimate carried slack. A perfectly flat 1.00 for months is the least believable result of all.

**Can CPI recover once it falls below 1?**
Cumulative CPI moves slowly by construction, since each month is a small fraction of the total. Period CPI can recover quickly when the cause was a discrete event or a measurement error. Long-standing defence programme practice suggests cumulative CPI rarely improves much past about a fifth complete; treat that as a prompt to test your own portfolio rather than as a published finding.

**Why is my CPI below 1 while the project is under budget?**
Because budget is a spending plan and CPI is a performance ratio. Spending less than planned while earning even less than you spent gives an underspend and a poor index at the same time. That combination usually means the work is late, so check SPI and the critical path before treating the underspend as good news.

**Should CPI drive an escalation to the sponsor?**
Set a threshold in the cost control procedure and apply it without discussion. A common shape is a variance over a fixed percentage or a fixed amount, whichever is smaller, tested at control account level. Fixed rules avoid the pattern where a variance is escalated only once it is too large to solve.

**Does a CPI below 1 change reported revenue?**
Not directly, but the forecast it drives does. Where progress is measured by a cost-based input method, revenue follows costs incurred divided by total expected costs, and total expected costs is your EAC. Raising the EAC lowers percentage complete and reverses revenue already recognised. Nothing here is accounting advice; the point is the timing of the conversation.

**Can an AI model predict which control accounts will fall below 1?**
It can flag accounts whose behaviour has changed against their own history, which is genuinely useful across a portfolio of hundreds. Score it honestly before relying on it, because [how to test whether a model actually helps a controls team](https://pciai.org/ai-in-project-controls) comes down to precision, recall and F1 on a validation set of accounts a human has already reviewed. An alert tool with no measured false-positive rate will be ignored within two months, and it should be.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so this is not a republish of a PCI site page.*

*Linking note: two links are now in the body, one per domain. "Which forecast method matches your cause" sits in the list of what to do about a poor index (https://projectcontrolsinstitute.org/four-eac-formulas), because that instruction is useless without the four methods beside it. "How to test whether a model actually helps a controls team" sits in the FAQ on predictive flagging (https://pciai.org/ai-in-project-controls). The pillar page and the reporting-thresholds page were dropped: both are hub pages, and this piece already spends its one hub link. The title is written the way a person asks the question rather than around the exact search string, and the definition sentence answers it in the first line, which is the point of the opening. Reciprocal: none warranted.*
