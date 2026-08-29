---
platform:      Medium
type:          pillar
title:         AI in project controls: what it does and does not do
meta:          What AI in project controls does well, how to measure a model on your own data with precision and recall, and the forecasting judgement it cannot make.
primary_kw:    AI in project controls
secondary_kw:  governed AI, precision and recall, estimate at completion, project controls automation
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     canonical -> /ai-in-project-controls (own site #056)
schema:        Article + FAQPage
word_count:    2,475
hashtags:      #ProjectControls #AIGovernance #EarnedValue #CostEngineering #ProjectManagement
ab_id:         AB-00038
---

# AI in project controls: what it does and does not do

AI in project controls is the governed use of machine learning and language models to assemble, check and explain the numbers a controls function publishes. It reads documents, ranks anomalies, drafts commentary and tests scenarios far faster than any team. It does not decide which forecast is true, and it cannot be accountable for one.

That last sentence is the whole subject. Everything below is about where the line sits and how to prove a tool is on the right side of it.

## What does AI in project controls actually do?

A controls function does four things. It measures what happened, forecasts what happens next, explains the gap, and hands somebody a decision to take.

Machines are strong on the first and the third, usable under supervision on the second, and irrelevant to the fourth. The split is commercial rather than academic: a product sold as "AI forecasting" that in fact does fast data assembly is still worth buying, but it should not be governed as though it produced the forecast.

| Task | What a model does well | What still needs a person |
|---|---|---|
| Data assembly | Pulls commitments, invoices, timesheets and progress into one dated set and flags the gaps | Deciding whether the cut-off is clean and the accruals are complete |
| Anomaly detection | Ranks transactions, quantities and durations that do not fit the pattern | Judging whether an outlier is an error, a real event, or the start of a trend |
| Document extraction | Reads contracts, variations and RFIs; pulls dates, values and obligations | Confirming commercial meaning, particularly of a disputed clause |
| Schedule checking | Finds open ends, negative lags, hard constraints, out-of-sequence progress | Deciding whether the logic describes how the work will be built |
| Forecasting | Computes every method instantly and back-tests them against history | Choosing the method, because the choice is a claim about cause |
| Reporting | Drafts variance commentary in the house format | Owning the number in front of a board, a client or an auditor |
| Scenario testing | Runs hundreds of what-ifs faster than a workshop discusses three | Deciding which scenario gets funded |

Read that as a division of labour rather than a league table. The left column is worth real money precisely because it releases people for the right column.

## How do you tell whether a model is good enough?

You measure it on your own data, against outcomes you already know. Three numbers carry most of the argument: precision, recall and F1.

Precision is the share of the model's alarms that turned out to be real. Recall is the share of real problems it caught. F1 is their harmonic mean, used when you want one figure that penalises a lopsided model.

Take a cost-coding checker running across 12,000 transactions in a month. It flags 400. Review confirms 120 of those flags as genuine miscodings, and a later audit finds another 60 miscodings it never raised.

- Precision = true positives ÷ all flags = 120 ÷ 400 = **0.30**
- Recall = true positives ÷ all real errors = 120 ÷ (120 + 60) = 120 ÷ 180 = **0.67**
- F1 = 2 × (0.30 × 0.67) ÷ (0.30 + 0.67) = 0.402 ÷ 0.97 = **0.41**

Now price it. Four hundred flags at six minutes of review each is 40 hours of a cost engineer's month, spent to recover 120 errors, with 60 errors still through the net and a sample check on the unflagged population still required.

Tighten the confidence threshold and the trade moves. Suppose the model now raises 150 flags, of which 90 are real, leaving 90 real errors missed.

| Setting | Flags raised | True positives | Precision | Recall | F1 | Review hours |
|---|---:|---:|---:|---:|---:|---:|
| Loose threshold | 400 | 120 | 0.30 | 0.67 | 0.41 | 40 |
| Tight threshold | 150 | 90 | 0.60 | 0.50 | 0.55 | 15 |

The second setting is better on F1 and costs a little over a third of the review time, at 15 hours against 40. It also lets more through: 90 real errors missed against 60, half as many again. Which one you want depends on what a missed miscoding costs you, and that is a business decision rather than a modelling one.

A vendor who will not run this test on your data has told you something useful. Ask for precision and recall on a golden set you control, with the review time priced in.

## The forecast a model cannot choose for you

Estimate at completion is where the limit becomes visible, because the arithmetic is trivial and the answer still turns on a judgement.

Take a programme with a budget at completion (BAC) of £120m. At the data date: planned value (PV) £48m, earned value (EV) £42m, actual cost (AC) £50m.

Cost variance is EV − AC = 42 − 50 = **−£8m**. Schedule variance is EV − PV = 42 − 48 = **−£6m**. The indices are CPI = 42 ÷ 50 = **0.84** and SPI = 42 ÷ 48 = **0.875**.

Four methods, four answers, one set of inputs.

| Method | Formula | What it assumes | Answer |
|---|---|---|---:|
| Remaining work at budget | AC + (BAC − EV) | The overrun was a discrete event that will not repeat | £128.0m |
| Performance continues | BAC ÷ CPI | Cost performance to date is the best available predictor | £142.9m |
| Cost and schedule both bite | AC + (BAC − EV) ÷ (CPI × SPI) | Recovering the programme will itself cost money | £156.1m |
| Bottom-up re-estimate | AC + a fresh estimate to complete | History says nothing useful about the work that remains | £136.0m |

The arithmetic, so it can be checked. Method one is 50 + 78 = £128.0m. Method two is 120 ÷ 0.84 = £142.9m.

Method three uses CPI × SPI = 0.84 × 0.875 = 0.735, so 50 + (78 ÷ 0.735) = 50 + 106.1 = £156.1m. Method four re-prices the remaining scope at £86m, giving 50 + 86 = £136.0m.

That is a spread of **£28.1m on identical inputs**. A model produces all four in milliseconds and can tell you which one has been closest across your last thirty projects. It cannot tell you whether the flood that caused this overrun is over, which is why [how to choose and defend an EAC method](https://projectcontrolsinstitute.org/four-eac-formulas) stays a human judgement.

One further number frames the conversation. The to-complete performance index needed to land on the original budget is (BAC − EV) ÷ (BAC − AC) = 78 ÷ 70 = **1.114**.

A team delivering 0.84 is being asked to deliver 1.11 for the remainder of the job. A recovery plan that does not explain that jump is a wish with a spreadsheet attached.

## Where the money actually leaks

The expensive failures are rarely inside cost engineering or inside finance. They sit in the handover between them, and a model trained on one side has no idea the other side exists.

An engineer is examined on float, earning rules and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised and what a provision must satisfy, and almost never on a critical path. The same number crosses that boundary every month and nobody in the chain has been examined on the crossing.

Here is the crossing in one step. Progress read from site imagery is physical completion; revenue measured by a cost-to-cost input method is costs incurred divided by total expected costs.

On a job where the expensive work is front-loaded, those two percentages are nowhere near each other, and using the first to drive the second overstates margin.

A report that says "the project is 40% complete" without saying which 40% has produced a figure that is right for the site meeting and wrong for the ledger. That is a scoping defect rather than a modelling one, and it is the most common defect in the market.

The financial reporting side of that handover runs on a five-step revenue model, described in the Institute's own words rather than reproduced. Only one of the five steps is a controls problem: step three, which determines the transaction price, including variable amounts such as variations and claims, constrained so revenue is not taken where a significant reversal is expected.

That step is the change log in accounting language. The unapproved variations sitting in it are exactly the variable consideration step three is asking about, and nobody outside the controls team knows what they are worth. The other four steps belong to the accountant. Nothing PCI publishes is legal, tax or accounting advice.

## What governed AI means in practice

Governed AI is the principle PCI certifies against: AI proposes, the professional disposes. A tool may generate the schedule, the forecast or the risk analysis; a competent person validates it, understands how it was produced, and owns the decision that follows.

That principle turns into five concrete requirements, none of them difficult.

**Provenance.** Every AI-assisted number can be traced to its inputs and the model version that produced it. A figure you cannot reproduce is a figure you cannot defend.

**A measured baseline.** Precision and recall on your own golden set, refreshed as the data changes. A model that scored well on last year's contract mix is an assumption, not a control.

**A priced review step.** Somebody checks the output, the check takes hours, and those hours are budgeted. Unbudgeted review is the same as no review.

**Named accountability.** A person, not a function, owns each AI-assisted output. Auditors ask who, and "the system" is not an answer.

**A failure route.** What happens when the tool is wrong, who finds out, and how quickly. Confident wrongness is the characteristic failure mode of a language model and it does not announce itself.

## What is genuinely changing in the work

Three shifts are visible on real projects, and none of them is the one the marketing describes.

Reporting cycles compress. The collecting, reconciling and drafting that filled the first five days of a month collapses, and the released time either goes into analysis or quietly disappears. Which of those happens is a management decision taken before the tool arrives.

The junior task mix changes fastest. The apprenticeship in this discipline ran through doing the assembly by hand, which is precisely the part now automated. Building judgement without that route is an unsolved problem and pretending otherwise does new entrants no favours.

Assurance moves upstream. When outputs are produced in minutes, the control has to sit on the inputs and the method rather than on a review of the finished pack. That is a different skill from checking a spreadsheet.

## How PCI examines this

PCI certifies three AI-era credentials, each with its own Body of Knowledge and examination, and each built against [what an AI-era controls credential has to examine](https://pciai.org/ai-project-controls-certification) rather than against a syllabus written before the tools arrived.

| Credential | Full name | Shape | Centre of gravity |
|---|---|---|---|
| **PCL-AI** | PCI AI Project Controls Leader | 13 domains, 61 knowledge areas | The integrated controls discipline, cost through schedule to reported number |
| **PFL-AI** | PCI AI Project Finance Leader | 16 domains, 61 knowledge areas | Project finance, funding and financial reporting |
| **PML-AI** | PCI Project Management Leader – AI | 16 domains, 63 knowledge areas | Delivery leadership with controls literacy |

Each Body of Knowledge is proportioned 40/40/20 across finance and reporting, project management, and governed AI. The AI material covers concepts, data, prompting, tooling, applied workflows, governance and capability, including how to evaluate an output with a golden set and precision and recall — the arithmetic worked through above.

Behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements, and 92 sector case studies across the three volumes (26 + 33 + 33). The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation. The PCI Standards are certification requirements set by the Institute, not law.

## Frequently asked questions

**Will AI replace project controls professionals?**
No, but it changes the task mix sharply. Assembly and checking compress; choosing methods, defending forecasts and owning decisions do not. The risk to an individual is not redundancy but spending a career on the half that automates and never building the half that does not.

**Can AI produce an estimate at completion I can put in a board pack?**
It can produce every method and test each against your own history, which is genuinely valuable. It cannot choose between them, because the choice states what caused the variance and whether that cause persists. Publish the range, name the method you selected, and say why you selected it.

**What accuracy should I demand from an AI tool?**
There is no universal threshold. Demand precision and recall measured on your data, then price the review time each setting implies against the cost of the errors each setting misses. A model at 0.60 precision that saves 25 hours a month can beat one at 0.90 precision that flags almost nothing.

**Is our project data good enough for this?**
Usually the cost data is adequate, the schedule data is patchy, and the link between them is the weak point. Before buying anything, check that control accounts reconcile to the work breakdown structure and that progress is measured by a rule somebody can state out loud.

**Does using AI create an audit problem?**
Only where provenance is missing. An AI-assisted number is no different from a spreadsheet-derived one: the auditor asks what the inputs were, what method was applied, who reviewed it and who owns it. Store the model version and the input set with the output and every one of those is answerable.

**Where should a team start?**
With the checks that are deterministic and cheap to verify: schedule structural checking and cost-coding anomalies. Measure both for a quarter against outcomes you can confirm, price the review time honestly, and only then consider anything that touches a published forecast. Starting at the forecast end is how teams end up defending a number nobody understands.

---

*First published on pciai.org; the canonical points there. Medium links are nofollow, so treat this republish as distribution and qualified traffic, not as a backlink.*

*Internal links, as placed in the body. The forecast section links to [how to choose and defend an EAC method](https://projectcontrolsinstitute.org/four-eac-formulas), because the £28.1m spread on identical inputs asks how anyone picks one; the certification section links to [what an AI-era controls credential has to examine](https://pciai.org/ai-project-controls-certification), because naming three credentials raises what an examination of this subject should contain. Two links, two domains, one each. The AI for construction scheduling target was dropped: it would have been a second link to the same domain. The proposed `/eac-formulas` slug does not exist and was corrected to `/four-eac-formulas`. The FAQ on whether AI replaces project controls professionals is the whole subject of the planning-engineer piece on this same domain, and that piece already links up to this pillar; the relationship runs one way on purpose, because a second link to this domain from here would break the one-per-domain cap. Reciprocal: the scheduling guide on this domain should point back here for the pillar treatment of governed AI.*
