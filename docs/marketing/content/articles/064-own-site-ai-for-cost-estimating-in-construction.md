---
platform:      Own site — pciai.org
type:          guide
title:         AI for cost estimating in construction: an honest guide
meta:          Where AI for cost estimating in construction earns its place, how to measure its error and bias on past jobs, and where the estimate lands in the accounts.
primary_kw:    AI for cost estimating in construction
secondary_kw:  estimate bias, MAPE, cost-to-cost input method, PFL-AI
pillar:        AI in project controls
credential:    PFL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    1664
hashtags:      n/a (own site)
ab_id:         AB-00046
---

# AI for cost estimating in construction: an honest guide

AI for cost estimating in construction does three things well: pricing from historical cost data in minutes, checking a draft estimate against comparable jobs for missing scope, and testing a bid against what similar work actually cost. It does one thing badly — judging whether this job resembles the history it learned from.

That last judgement is the estimator's job and it always was. The rest of this guide is about the three useful parts, how to measure whether a tool is any good on your own data, and what happens to the number after the bid is won.

## Where AI for cost estimating in construction earns its place

An estimate is not one task. Splitting it up shows quickly which parts a model can carry and which it cannot.

| Estimating task | What a model does well | Data it needs | How it fails |
|---|---|---|---|
| Quantity take-off from models and drawings | Extracts quantities consistently, at speed | Structured models or good drawings | Silently misses what is not drawn |
| Pricing from historical cost | Matches items to prior rates and adjusts for time and location | Coded, cleaned historical cost with outturn, not just tender values  | Overlooks that the old jobs were unusual |
| Scope completeness checking | Flags items present on comparable jobs and missing here | A library of comparable estimates | Recommends scope the contract excludes |
| Benchmarking a bid | Places the bid against prior outturns | Outturn cost, not just award value | Comparables that are not comparable |
| Escalation and market adjustment | Applies indices consistently | Published indices and a supply-market view | Cannot see a supply shock that has not happened yet |
| Risk and contingency | Runs simulations across ranges rapidly | Defensible ranges, correlations | Ranges are judgement; the simulation dresses them up |
| First-of-a-kind work | Very little | There is no history to learn from | Answers confidently anyway |

The right-hand column is the estimating file in miniature. Every one of those failures also happens to human estimators, which is the point: the tool changes the speed and the volume, not the nature of the error.

## The four estimating methods, and where the model fits

AI does not replace an estimating method. It sits inside one, which is why "we use AI for estimating" tells a reviewer nothing on its own.

| Method | How the number is built | Data required | Best used for | Typical failure |
|---|---|---|---|---|
| Analogous | Scale a comparable completed project | A genuinely similar job with known outturn | Early screening | The comparable is not comparable |
| Parametric | A cost relationship applied to a driver, such as cost per square metre | Enough consistent history to fit a relationship | Concept and option comparison | The driver stops explaining cost outside its range |
| Bottom-up unit rate | Quantities priced at resource rates and built up | Full take-off, current rates, productivity | Tender and control estimates | Slow, and confident-looking because it is detailed |
| Model-assisted hybrid | Bottom-up build, machine-checked against history | All of the above plus clean outturn data | Bid review and challenge | The check is treated as approval |

Estimate class matters more than method. The screening number produced at concept and the control estimate produced at award answer different questions, and the classes defined in AACE International's recommended practices are the usual reference — named here and described in our own words, not reproduced.

## Measuring whether the tool is any good on your data

A vendor's accuracy claim is about their data. What matters is error and bias on yours, and both are calculable from jobs you have already finished.

Take five completed packages, estimate against outturn:

| Package | Estimate | Outturn | Difference | Error against outturn |
|---|---|---|---|---|
| A | £2,400,000 | £2,760,000 | −£360,000 | 13.0% |
| B | £5,000,000 | £5,150,000 | −£150,000 | 2.9% |
| C | £3,200,000 | £3,040,000 | +£160,000 | 5.3% |
| D | £8,600,000 | £9,460,000 | −£860,000 | 9.1% |
| E | £1,500,000 | £1,725,000 | −£225,000 | 13.0% |

**Mean absolute percentage error (MAPE)** is the average of those percentages regardless of sign: (13.0 + 2.9 + 5.3 + 9.1 + 13.0) ÷ 5 = **8.7%**. That is the spread you should expect on a new job of the same type.

**Bias** is the same average with the signs kept: (−13.0 − 2.9 + 5.3 − 9.1 − 13.0) ÷ 5 = **−6.6%**. Four of five estimates came in under, and the average miss is systematically low.

Those two numbers say different things. MAPE says how wide your uncertainty is; bias says which way you are wrong, and bias is the one that costs money because it does not average out across a portfolio.

Correct for it before adding contingency. A new estimate of £8,400,000 carrying a −6.6% bias implies an expected outturn of 8,400,000 ÷ 0.934 = **£8,993,576**, or roughly £590,000 more than the estimate says.

Do that correction once, not twice. A team that adjusts for bias and then adds the same allowance again as contingency has priced the same risk twice, and will lose bids for a reason nobody can find.

## What happens to the estimate after award

An estimate does not stop being useful when the job starts. On a contract where revenue is recognised over time using a cost-to-cost input measure, expected total cost becomes the denominator of the percentage complete — so the estimate drives the reported revenue every month.

Take a contract priced at £11,000,000 with £4,200,000 of cost incurred. Using the original £8,400,000 expected cost, progress is 4,200,000 ÷ 8,400,000 = **50.0%**, and revenue to date is 0.50 × 11,000,000 = **£5,500,000**.

Using the bias-corrected £8,993,576, progress is 46.7% and revenue to date is **£5,137,000**. The same work, the same month, £363,000 of difference, decided by an estimating assumption made before the contract was signed.

That is the overlap this Institute exists for. A quantity surveyor who prices well and an accountant who closes cleanly can both do their jobs perfectly and still publish the wrong number, because the estimate is a finance input and almost nobody is examined on both sides of that handover.

This describes the mechanism. Nothing PCI publishes is legal, tax or accounting advice.

## What to demand from a vendor

Ask for precision and recall on scope-completeness flags, measured on your projects, not theirs. A flagging tool that raises 200 items to find 40 real ones costs review time you have to budget for.

Ask what the training data was: tender values or outturn costs. A model trained on what jobs were sold for learns the market's optimism, not the cost of building.

Ask how it handles escalation and location, and whether it will show its comparables. An estimate you cannot trace to comparable jobs cannot be defended to a board, however good it is.

Ask what happens when it does not know. A tool that returns a wide range and says so is more useful than one that always returns a confident single figure.

## What AI cannot estimate

First-of-a-kind work, because there is nothing to learn from. Novel logistics — a constrained city site with a single crane position — where the cost driver is a sequencing decision, not a quantity.

Market conditions that have not yet appeared in the indices. A model reads history; a supply shock is by definition not in it.

And commercial position. What a job should cost and what you should bid are different questions, and only one of them is arithmetic.

## How PCI examines this

PCI certifies the finance side of project work through the PCI AI Project Finance Leader (PFL-AI), which holds 16 domains and 61 knowledge areas. The controls credential, the PCI AI Project Controls Leader (PCL-AI), holds 13 domains and 61 knowledge areas, and the delivery credential, the PCI Project Management Leader – AI (PML-AI), holds 16 domains and 63 knowledge areas.

Each Body of Knowledge is proportioned 40/40/20 across finance and reporting, project management, and governed AI, so the handover shown above — estimate to expected cost to recognised revenue — is examined as one competence rather than two. The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite. Across the three volumes there are 92 sector case studies (26 + 33 + 33).

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**How accurate is AI cost estimating in construction?**
As accurate as the outturn data behind it, and no more. Measure it yourself: take ten completed packages, compute mean absolute percentage error and bias, and compare against how your estimators performed on the same jobs. Any accuracy figure quoted without naming the data it was measured on should be ignored.

**Can AI replace an estimator?**
No. It compresses take-off, pricing and checking, which is most of the hours, and leaves the judgement about whether this job resembles the history. That judgement is where estimates are won and lost, and it is not present in the data.

**What data do we need before starting?**
Coded historical cost with outturn values, not just tender sums, and a work breakdown that has stayed stable enough for the codes to mean the same thing across jobs. Most contractors have the cost and have lost the coding consistency, and that is the work to do first.

**Does an AI-assisted estimate cause audit problems?**
Not if provenance is recorded. Keep the inputs, comparables, model version, adjustments applied and the reviewer with the estimate. An auditor's question is who decided and on what basis, and that is answerable with a record and unanswerable without one.

**Should the contingency change if we use AI?**
Only if your measured error has changed. Contingency should follow the spread you can demonstrate on your own completed jobs, and it should be set after correcting for bias, not on top of a correction that has already been applied.

---

*Internal links: this guide should link to [the AI in project controls pillar](https://pciai.org/ai-in-project-controls) with the anchor "how governed AI applies across the controls lifecycle", to [IFRS 15 for construction](https://projectcontrolsinstitute.org/ifrs-15-for-construction) with the anchor "percentage of completion and the cost-to-cost input method", and to [cost control methods that catch overruns early](https://projectcontrolsinstitute.org/cost-control-in-construction) with the anchor "keeping the estimate honest once the work starts".*
