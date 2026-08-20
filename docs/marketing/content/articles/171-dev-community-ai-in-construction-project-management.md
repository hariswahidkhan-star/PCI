---
platform:      DEV Community
type:          guide
title:         AI in construction project management: the failure mode
meta:          Where AI in construction project management helps, how to score a progress model on precision and recall, and the arithmetic that quietly moves margin.
primary_kw:    AI in construction project management
secondary_kw:  progress measurement, cost-to-cost input method, precision and recall, IFRS 15 five-step model
pillar:        AI in project controls
credential:    PML-AI
target_domain: pciai.org
canonical:     canonical -> pciai.org/ai-in-construction-project-management
schema:        Article
word_count:    1782
hashtags:      #ai #machinelearning #datascience #tutorial
ab_id:         AB-00041
---

# AI in construction project management: the failure mode

AI in construction project management works well in four places: reading documents, estimating from historical cost data, measuring physical progress from imagery, and drafting reports. The failure mode is the same in all four. The tool answers one question accurately, the answer is carried into the accounts as though it answered a different one, and margin moves.

This is written for the people who build and integrate those tools, because the error lives in the interface rather than in the model.

## What the tools actually do on a large project

The right-hand column is the part that does not become automatable by buying a better product.

| Function | What the tool does | What it cannot settle |
|---|---|---|
| Estimating and preconstruction | Prices from historical cost data, flags scope missing against comparable jobs | Whether this job's conditions resemble the ones in the training history |
| Document control | Classifies and routes RFIs, submittals and correspondence; extracts dates and obligations | The commercial meaning of a clause under dispute |
| Progress measurement | Turns site imagery, scans and sensor data into a physical percentage complete | Which definition of "complete" the number is answering |
| Safety | Detects unsafe conditions in images and near-miss patterns in reports | The decision to stop work, which is a person's to make |
| Commercial | Compares variation orders against contract terms and prior instructions | Entitlement, which is a legal and commercial judgement |
| Cost and forecasting | Runs every forecasting method and tests each against project history | Which method to publish, because that is a claim about cause |
| Reporting | Drafts commentary in the house format, in minutes | Ownership of the number in front of the client |

Every entry in that column is a decision with a consequence attached, and consequences need a named owner.

## The three percentages

A construction project carries at least three different percentages complete at any data date, and a vision model produces exactly one of them.

**Physical progress** is how much of the work exists on site. **Earned progress** is budgeted value earned against an agreed earning rule. **Cost-to-cost progress** is costs incurred as a share of total expected costs, and it is the one that commonly drives revenue recognised over time.

On a job with front-loaded costs — piling, temporary works, off-site fabrication — those three can differ by ten points or more on the same day, from the same underlying facts.

Treat them as three distinct types with three distinct units. If your integration passes any one of them into a field expecting another, you have a type error that the compiler cannot see and the ledger will not flag.

## The five-step revenue model, in plain terms

Revenue on a construction contract is not recognised because a valuation was certified. It follows a five-step sequence, described here in our own words rather than reproduced from the standard.

1. **Identify the contract.** An agreement with enforceable rights and obligations, where the consideration is probable.
2. **Identify the performance obligations.** The distinct promises inside it. On many construction contracts the whole works is a single obligation; on some it is not.
3. **Determine the transaction price.** Including variable consideration such as variations, claims and liquidated damages, constrained so a significant reversal is not expected.
4. **Allocate the price** across the identified obligations.
5. **Recognise revenue as each obligation is satisfied**, over time where the criteria are met, using a method that faithfully depicts progress.

Step five is the socket your model output plugs into. Nothing here is accounting advice; it is a description of the mechanism your data will feed.

## The arithmetic, in full

Take a contract with a transaction price of **£40m** and expected total costs of **£32m**. Costs incurred to date are **£12m**.

Naive cost-to-cost: progress = 12 ÷ 32 = **37.5%**. Revenue = 0.375 × 40 = **£15.0m**. Against £12m of cost, margin is **£3.0m**.

Now the detail. Of that £12m, **£2m** is structural steel delivered to site and not yet installed. Costs that do not depict progress are excluded from the measure, and such materials are commonly recognised at cost with no margin.

Recompute: progress = (12 − 2) ÷ (32 − 2) = 10 ÷ 30 = **33.3%**. Revenue on progress = 0.333 × (40 − 2) = **£12.67m**, plus £2m of materials at cost, giving **£14.67m**. Margin is 14.67 − 12 = **£2.67m**.

That is **£0.33m of margin** difference in one month, on identical site facts, produced by one exclusion rule.

Now add the tool. A progress engine reading drone imagery reports the structure as **45% complete**, and it may be entirely correct as a physical measure. Carried into revenue as though it were the progress measure: 0.45 × 40 = **£18.0m**, margin **£6.0m**.

That is **£3.33m above** the properly measured figure, produced by a model doing exactly what it was built to do. Nobody lied. The output answered a different question from the one the ledger asked.

## Scoring the model before it touches a ledger

A progress or defect classifier should be scored on your own data, against outcomes you already know, before any of its output reaches a report.

Take a model that classifies each of 1,000 installed components as complete or not at cut-off. A manual audit of the same 1,000 finds 400 genuinely complete. At its default threshold the model flags 500, of which 340 are right.

Precision = TP ÷ (TP + FP) = 340 ÷ 500 = **0.680**. Recall = TP ÷ (TP + FN) = 340 ÷ 400 = **0.850**.

F1 = 2PR ÷ (P + R) = (2 × 0.680 × 0.850) ÷ 1.530 = 1.156 ÷ 1.530 = **0.756**.

Now move the threshold and watch the trade change shape.

| Threshold | Flagged | True positives | Precision | Recall | F1 |
|---|---:|---:|---:|---:|---:|
| 0.5 | 500 | 340 | 0.680 | 0.850 | 0.756 |
| 0.7 | 380 | 300 | 0.789 | 0.750 | 0.769 |
| 0.9 | 250 | 225 | 0.900 | 0.563 | 0.692 |

The money version of that table matters more than F1. At an average earned value of £18,000 per component, the 160 false positives at the 0.5 threshold represent £2.88m of value claimed that does not exist, against £1.08m understated by the 60 misses — a net overstatement of **£1.80m**.

At the 0.9 threshold the same sum gives 25 false positives (£0.45m) against 175 misses (£3.15m), a net **understatement of £2.70m**. F1 barely moved; the direction of the error reversed completely.

Choose the operating point against the cost of each error type, not against the highest F1. On anything feeding a payment application, an understatement is recoverable next month and an overstatement is a restatement.

## The interface contract worth writing down

Make the model's output self-describing, so that no downstream service has to guess what it means.

Ship the measure name (physical, earned or cost-to-cost), the scope boundary it covers, the exclusions applied, the data date and time zone, the model version, and the confidence or threshold used. Six fields, and they prevent the failure at the top of this article.

Then pin the cut-off. A progress reading taken three days after the accounting cut-off is not wrong, it is unusable, and the difference is invisible in the report unless the timestamp travels with the number.

## What changes about the job

The tasks that compress are the ones a project manager was doing at eleven at night: chasing status, reconciling versions, assembling a pack. That gain is real and should be taken.

The tasks that grow are the ones the tools create demand for. More flags need dispositioning, more outputs need challenging, and more numbers arrive with a plausible face and no provenance.

The decisive skill is reading a number and knowing which question it answers. That has always been a controls skill, and AI has made it a delivery skill too, because the outputs now land on the delivery lead's desk without passing anyone trained to check them.

PCI certifies delivery leadership through the PCI Project Management Leader – AI (PML-AI), which holds 16 domains and 63 knowledge areas, with a Body of Knowledge proportioned 40 / 40 / 20 across finance and reporting, project management, and governed AI. PCI is an independent certifying body and claims no accreditation, endorsement or affiliation with any other organisation.

## Frequently asked questions

**Is AI reliable enough to measure progress for payment applications?**
For repetitive, visible work it is often more consistent than manual assessment. Whether it is reliable enough for a payment application depends on the contract's measurement provisions, which usually specify how quantities are agreed and by whom. The technology does not change what the contract says, and a model score is not an agreed quantity.

**Can a model read our contract and tell us our entitlement?**
It can find the clauses, the notice periods and the prior correspondence quickly, which is most of the labour. Entitlement is a legal and commercial judgement about facts, and a confident summary of a disputed clause is a liability rather than an opinion. Retrieval is the useful part; conclusion is not.

**Where does AI fail most expensively on construction projects?**
In the handover between measurement and reporting. A defensible physical percentage mapped without thought onto a revenue measure moves margin in the accounts without anyone deciding to move it. That failure is silent, monthly and cumulative, and it is usually found by an auditor rather than by the team.

**What controls does a team need before deploying these tools?**
Not new ones in kind. Provenance for every AI-assisted number, a measured baseline on your own data, a review step that is funded rather than assumed, and a named owner for each output. The same four controls you should already have for a spreadsheet nobody understands.

**Should the model produce a number or a recommendation?**
A number, with its measure name and exclusions attached. Recommendations hide the assumption that caused the error, and they invite acceptance without inspection. A well-typed measurement is more useful to a project than a confident suggestion, and far easier to audit later.

---

*First published on pciai.org; the `canonical_url` on this post points there. DEV prohibits stub posts that link out, so the whole argument including the failure table is here rather than behind a link.*

*Internal links: this piece should link to [the AI in project controls pillar](https://pciai.org/ai-in-project-controls) with the anchor "how governed AI applies across the controls lifecycle", to [whether AI will replace project managers](https://pciai.org/will-ai-replace-project-managers) with the anchor "what changes in the project manager's role", and to [IFRS 15 for construction contracts](https://projectcontrolsinstitute.org/ifrs-15-for-construction) with the anchor "the five-step model applied to a construction contract".*
