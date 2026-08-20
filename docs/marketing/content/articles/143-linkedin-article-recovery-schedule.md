---
platform:      LinkedIn Article
type:          how-to
title:         How to build a recovery schedule the engineer accepts
meta:          How to build a recovery schedule: size the gap in days, isolate the driving path, price each lever, and prove the recovery survives the path shift.
primary_kw:    how to build a recovery schedule
secondary_kw:  acceleration cost, crash cost per day, driving path, delay damages
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo + FAQPage
word_count:    1,859
hashtags:      #ProjectControls #Scheduling #ProjectManagement #CostEngineering
ab_id:         AB-00199
---

# How to build a recovery schedule the engineer accepts

A recovery schedule is a revised programme showing how the remaining work will meet the contract completion date after progress has fallen behind. It is not a new baseline and not an extension of time. How to build a recovery schedule comes down to six steps: status honestly, size the gap in days, isolate the driving path, price the levers, test the logic, submit the assumptions.

Written for LinkedIn as an original. It sits under the Institute's planning and scheduling pillar.

## When does the engineer get to demand one?

When actual progress no longer accords with the accepted programme and the completion date is at risk. Most standard forms carry a provision along those lines, expressed differently in each, and the demand usually follows a progress report rather than arriving out of nowhere.

Read your own contract for the trigger, the notice period and what the submission must contain. The mechanism varies enough between forms that a general description is a starting point, not an answer, and nothing here is legal advice.

The commercial reality is simpler than the drafting. The engineer is asking you to demonstrate that the completion date is still real, and a document that does not demonstrate it will be rejected however neat the bar chart looks.

## What a recovery schedule is not

It is not a re-baseline. Re-baselining resets the measurement datum and usually needs agreement, and doing it inside a recovery submission looks like an attempt to erase the variance you were asked to explain.

It is not an extension of time claim. Entitlement and recovery are separate arguments, and mixing them weakens both. Submit the recovery position, preserve the notices, and keep the entitlement analysis in its own document, built with [the techniques used to analyse delay after the event](https://projectcontrolsinstitute.org/delay-analysis-techniques).

It is not an acceleration instruction either. Building a recovery schedule does not by itself decide who pays for the additional cost, and that question turns on the contract and the facts.

## Step one: status the schedule honestly

Update actual starts, actual finishes and remaining durations against a firm data date before you touch anything else. A recovery plan built on optimistic remaining durations fails twice: once when it does not recover, and again when the record shows you knew.

Check the retained-logic setting and the out-of-sequence progress before you read any float value. Progress override quietly deletes logic and produces a forecast that no reviewer will accept once they find it.

Run the network and write down the calculated finish. That figure is the honest starting point for everything that follows.

## Step two: size the gap in days

The gap is calculated finish minus contract completion, in working days on the project calendar. One number, stated plainly.

Take a contract completion at **day 240** from the data date and a calculated finish at **day 274**. The gap is **34 working days**, and that is what the recovery has to find.

Price it before you plan it. At delay damages of **£40,000 per day**, thirty-four days carries **£1,360,000** of exposure, and the financing carry sits on top of that.

## Step three: isolate the driving path

Recovery only comes from the driving path. Time taken out of an activity with float buys nothing, and a submission full of shortened non-critical bars is the most common reason for rejection.

Identify criticality by longest path rather than by a total float threshold, particularly where a project constraint is in play. A constrained network reports float values relative to an imposed date and will mislead you about which chain actually drives the finish.

Then record the float on the paths immediately behind it. Those numbers decide how far your recovery can go before the problem moves.

## Step four: price each lever

Every recovery option has a cost per day recovered, and ranking by that slope is the whole of the decision. Crash cost per day is the additional cost divided by the days it buys.

| Activity | On driving path | Normal duration | Normal cost | Crashed duration | Crashed cost | Days available | Cost per day recovered |
|---|---|---:|---:|---:|---:|---:|---:|
| Piling | Yes | 40 | £400,000 | 32 | £520,000 | 8 | £15,000 |
| Steel erection | Yes | 60 | £1,800,000 | 50 | £2,050,000 | 10 | £25,000 |
| Cladding | Yes | 45 | £900,000 | 40 | £1,060,000 | 5 | £32,000 |
| Mechanical first fix | No, 12 days float | 55 | £1,200,000 | 47 | £1,344,000 | 8 | £18,000 |

Piling works out at (520,000 − 400,000) ÷ 8 = **£15,000 per day**, steel at (2,050,000 − 1,800,000) ÷ 10 = **£25,000 per day**, cladding at (1,060,000 − 900,000) ÷ 5 = **£32,000 per day**.

The driving path therefore offers **8 + 10 + 5 = 23 days** in total, against a gap of 34. The arithmetic has already told you that crashing alone will not close it.

## Step five: test what happens when the path moves

This is the step that separates a recovery schedule from a wish. The mechanical path carries **12 days** of float, so it becomes co-critical once you have recovered twelve days on the driving path.

Days one to twelve are cheap. Eight days of piling at £15,000 and four days of steel at £25,000 costs **£220,000** and avoids **12 × £40,000 = £480,000** of damages.

Days thirteen onwards have to come off both paths at once. Each further day now costs £25,000 on steel plus £18,000 on mechanical first fix, which is **£43,000 per day**, above the damages rate on its own.

Look at the full exposure before you stop there. With **£180,000,000** drawn and earning nothing at a **7%** nominal rate, the carrying cost is £12,600,000 a year, which is 12,600,000 ÷ 365 = **£34,521 per day**.

Total exposure is therefore roughly 40,000 + 34,521 = **£74,521 per day**, and recovery at £43,000 per day is still worth buying. That is the calculation the delivery team cannot do alone and the finance team will not do unprompted, and it is precisely where projects lose money.

The last **11 days** are not available from crashing at all. They have to come from re-sequencing, overlapping design and construction, off-site fabrication, a sectional handover, or an entitlement position on the causes of the original slippage.

| Lever | Days it can buy | Cost driver | Risk it introduces | Evidence to keep |
|---|---|---|---|---|
| Additional crews | Moderate | Labour rate plus supervision | Congestion and falling productivity | Crew histograms, area access records |
| Second shift | Moderate to high | Shift premium, lighting, supervision | Quality and handover between shifts | Shift records, output per shift |
| Sustained overtime | Low | Premium hours | Productivity decay; state your assumption | Hours booked against output |
| Re-sequencing and overlap | High | Rework risk, coordination | Design changes landing late | Revised logic and the reason for it |
| Off-site fabrication | High on the right package | Preliminary cost and transport | Interface tolerance, delivery risk | Procurement dates, factory programme |
| Sectional or partial handover | High | Commercial negotiation | Fragmented completion obligations | Agreed sectional definition |

Do not model sustained overtime at normal productivity. State the loss you assumed and where the assumption came from, because a reviewer who thinks you have ignored it will discount the whole submission.

## Step six: submit the assumptions, not just the bars

A recovery submission that gets accepted contains six things. The updated programme with a stated data date, the calculated finish before recovery, the recovery measures with the days each one buys, the revised logic showing the new driving path, the assumptions and their owners, and the cost consequence with a clear statement of the position on who bears it.

Include what happens if the measures do not deliver. A recovery schedule with a stated fallback is treated as a plan, and one without a fallback is treated as a promise, which is a much worse document to be holding in three months.

Reserve your notices. Recovery and entitlement are separate arguments and submitting one does not concede the other, but silence on the record is difficult to repair later.

## What gets a recovery schedule rejected?

Shortened durations with no method behind them. If a 60-day activity becomes 50 days, the submission has to say what changed: more crews, longer shifts, a different sequence, or a supplier commitment in writing.

Recovery taken from activities with float, which changes no date at all. Logic changes made silently between revisions. Removing a constraint so the negative float disappears. And a resource profile that no organisation could staff, which is the fastest way to lose the reviewer's trust on everything else in the file.

## Frequently asked questions

**How long should a recovery schedule cover?**
To contract completion, not to the next milestone. The engineer is asking whether the completion date holds, so a plan that stops at the end of the current phase does not answer the question. Detail the next eight to twelve weeks at working level and keep the remainder at the level the accepted programme uses.

**Does submitting a recovery schedule waive an extension of time claim?**
That depends on the contract and the facts, and it is a question for your commercial and legal advisers rather than a general answer. What planners can do is keep the two documents separate, preserve the notices, and make sure the recovery narrative does not concede causation it did not intend to concede.

**Should the recovery schedule become the new baseline?**
Not by default. The baseline is the measurement datum for variance and entitlement, and replacing it removes the history that explains the current position. Where a re-baseline is genuinely needed, request it explicitly and separately, with the reasons stated and agreed. Recovery submissions that quietly reset the datum tend to be read as an attempt to erase the variance rather than to close it.

**How do you recover time when the driving path is procurement?**
Rarely through the site programme, which is why the arithmetic matters early. The levers are expediting, split deliveries, alternative suppliers, and re-sequencing to a package that can start without the missing item. Each one carries a cost and a risk that belongs in the submission.

**What if the gap cannot be closed at all?**
Say so, with the number. A submission showing 23 days of achievable recovery against a 34-day gap, with the remaining 11 days identified and priced, is far more useful to everyone than a document that closes the gap on paper and fails in month three.

---

*PCI publishes certification requirements. Nothing here is legal, tax or accounting advice, and contract mechanisms differ between forms. All figures above are illustrative arithmetic, not project data.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Linking note: one cross-estate link now sits in the body, in the section separating recovery from entitlement. That sentence tells the reader to keep the entitlement analysis in its own document, which raises the question of how that analysis is actually done, and the hub's comparison of delay analysis techniques answers it. The note originally proposed two more hub links, to total float and project cash flow forecasting. Both were dropped because only one link per domain is allowed per piece, and the float behind the driving path is explained in step three of this article rather than elsewhere. A reciprocal link back to this piece would fit on the delay analysis techniques page, where the difference between a recovery submission and an entitlement position deserves a pointer.*
