---
platform:      Reddit / forum — r/CostEngineering
type:          forum-post
title:         Contingency: expected value vs parametric compared
meta:          A risk register priced 4.2% of contingency. The company's own closed-out jobs said 18.5%. Both methods were applied correctly. Here is why they disagree.
primary_kw:    contingency by expected value *
secondary_kw:  parametric contingency, AACE 44R-08, contingency drawdown, expected loss
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,362
hashtags:      n/a (Reddit)
ab_id:         AB-00235
---

# Contingency: expected value vs parametric, and when each breaks

The risk register priced contingency at 4.2% of the base estimate. The same company's last fourteen closed-out jobs, sanctioned at the same level of engineering definition, had averaged 18.5% growth. Neither method was applied badly. They were measuring different things, and nobody in the estimate review noticed.

Short answer: expected value prices the risks you have identified. Parametric prices the risks that projects like yours have historically had, whether or not anyone listed them. If you only run the first, you will underfund every early-stage estimate you ever produce.

## The expected value method, worked

The approach behind AACE International's recommended practice 44R-08, described in my own words: take each risk in the register, agree a probability and a cost impact, multiply, and sum. Opportunities go in with negative impact.

Base estimate: **£86.0m**.

| Risk | Probability | Impact | Expected value |
|---|---:|---:|---:|
| Ground conditions worse than the borehole log | 0.30 | £3,600k | **£1,080k** |
| MEP subcontract retender above allowance | 0.45 | £2,200k | **£990k** |
| Commissioning extended by six weeks | 0.25 | £1,450k | **£363k** |
| Client scope growth on MEP | 0.55 | £1,900k | **£1,045k** |
| Currency movement on imported switchgear | 0.40 | £900k | **£360k** |
| Early access to plot C (opportunity) | 0.30 | −£700k | **−£210k** |
| **Net expected value** | | | **£3,628k** |

£3,628k on an £86.0m base is **4.2%**.

That is arithmetically correct and it is defensible line by line. It is also, on a job at a low definition level, badly wrong — and the reason is not in the table.

## What expected value structurally cannot see

It prices identified, discrete risks. It cannot price the accuracy of the estimate itself.

If your quantities came off a design that is 25% engineered, the take-off is provisional, the rates are benchmark rates, and the scope has not finished growing. None of that appears as a line in a risk register, because it is not an event. It is a property of the estimate.

That gap is called systemic risk, and on an early estimate it is usually larger than everything in the register put together.

## The parametric method, worked

The approach behind recommended practice 42R-08, again in my own words: model contingency as a function of the drivers that historically predicted growth — level of engineering definition, technology newness, process complexity, schedule aggression, team experience, and the quality of the estimating basis — with the model calibrated against your own completed projects.

Suppose your closed-out set of fourteen comparable jobs, sanctioned at the same definition level, shows growth over base estimate averaging **18.5%** with a standard deviation of about **9 points**.

- 86.0 × 0.185 = **£15.9m**

That is **4.4 times** what the register produced, on the same job, on the same day. The parametric answer is not a better guess. It is a different question answered: not "what might go wrong" but "what has gone wrong on jobs that looked like this one".

## Side by side

| | Expected value | Parametric |
|---|---|---|
| Prices | Identified, discrete risk events | Systemic risk carried by the estimate's maturity |
| Needs | A credible register, agreed probabilities and impacts | Calibrated history of your own closed-out projects |
| Strongest | Later stages, where design is firm and risks are specific | Early stages, sanction and pre-FID, where design is not firm |
| Breaks when | The biggest exposure is not on the register | Your history is thin, or this job is unlike the calibration set |
| Fails silently by | Underfunding, and looking rigorous while doing it | Producing an authoritative percentage from drivers someone tuned |
| Answers | "What might go wrong here?" | "What went wrong on jobs like this?" |

## The double-count trap

Do not simply add them. £3.63m plus £15.9m is £19.5m, or 22.7%, and it is very likely an overstatement.

Your parametric model was calibrated on outturns. Those outturns already contain realised ground conditions, realised retenders and realised scope growth. Adding an expected value register on top counts the same exposures twice.

The defensible combination is to split the register cleanly. Parametric or a systemic-risk model covers estimate maturity and scope definition. Expected value or a Monte Carlo simulation covers the project-specific events that are genuinely additional to the historic pattern — the single client-supplied item with a known problem, the one permit with a live objection.

Write down which category each line sits in. If you cannot decide, it belongs in the systemic bucket.

## Where each one actually breaks

**Expected value breaks** when the register is the only input; when correlation is ignored, because risks with a common cause land together and the sum understates the tail; when the largest exposure is unlisted, which is normal; and when the resulting mean is funded and then called "contingency to P80", which it is not.

**Parametric breaks** when you have no calibrated history and borrow someone else's; when the current project genuinely is unlike the set, which is a judgement, not an output; and when a driver is adjusted downwards after the first answer proved unpopular. That last failure is common enough to be worth naming in the estimate basis document.

## The drawdown test, monthly

Whichever method funded it, contingency should fall roughly in step with risk being retired.

If you are 45% complete and £3.4m of a £3.6m contingency remains, you have drawn 5.6% while completing nearly half the work. Either every exposure is still ahead of you, or the register has stopped reflecting the job and the risks are being absorbed elsewhere as "scope". It is almost never the first.

## The part that reaches the accounts

Here is where this stops being an estimating argument. Undrawn contingency is a cost the project expects to incur. In the accounts it belongs in expected costs to complete, and that has consequences.

Contract price **£96.0m**. Estimate at completion excluding undrawn contingency: **£84.5m**, so the project reports margin of **£11.5m**. Including £3.6m of undrawn contingency: **£88.1m**, so expected margin is **£7.9m**.

At 40% complete, margin recognised to date is 0.40 × 11.5 = **£4.60m** on the project's number, and 0.40 × 7.9 = **£3.16m** on the other. **£1.44m** of recognised margin turns on whether contingency is treated as a real cost or as a cushion the project intends not to spend.

That is the overlap where money quietly moves. A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy, almost never on how contingency was sized.

A cost engineer is examined on estimate classes and drawdown, almost never on the margin their treatment of contingency releases. PCI publishes certification requirements; nothing here is accounting advice, and your reporting framework decides the treatment.

## Common follow-ups

**Which method for a Class 3 estimate?**
Both, split as above. Class 3 is where systemic risk is still large enough to matter and specific risks are finally identifiable, so neither alone is sufficient.

**Is management reserve part of contingency?**
No. Contingency funds identified and expected variability within the approved scope. Management reserve sits above the project baseline for changes outside it, and it should be held by a different person for exactly that reason.

**How do I build a calibration set from scratch?**
Start recording base estimate at sanction, definition level, and final outturn on every job as it closes. Ten to fifteen comparable projects makes a usable model. Until then, use published estimate classification ranges and say plainly in the estimate basis that the model is uncalibrated.

**Can I just take P80 from a Monte Carlo and skip this?**
Only if your simulation includes systemic risk as well as the register, and states its correlation assumptions. A simulation over an incomplete register produces a precise number about an incomplete question.

---

*Disclosure: I write for the Project Controls Institute. One link, at the end, and every figure above is checkable without it: [how to run a Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation).*

*Internal links: the in-post link uses the anchor "how to run a Monte Carlo cost simulation". Comment replies should use [how to build a risk register stakeholders actually use](https://projectcontrolsinstitute.org/risk-register-that-gets-used) and [how the estimate at completion reaches the accounts](https://projectcontrolsinstitute.org/eac-accounting) with those anchors.*
