---
platform:      Own site — pciai.org
type:          pillar
title:         AI in project controls: what works and what does not
meta:          A practitioner's guide to AI in project controls: what models do well, how to measure whether one is good enough, and the judgements they cannot make.
primary_kw:    AI in project controls
secondary_kw:  governed AI, precision and recall, estimate at completion, project controls automation
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    2230
hashtags:      n/a (own site)
ab_id:         AB-00038
---

# AI in project controls: what works and what does not

AI in project controls is the governed use of machine learning and language models to assemble, check and explain the numbers a controls function produces. It reads documents, spots anomalies, drafts commentary and tests scenarios at a speed no team can match. It does not decide which forecast is true, and it cannot carry accountability for one.

That last sentence is the whole subject. Everything below is about where the line sits, and how to prove a tool is on the right side of it.

## What does AI in project controls actually do?

A controls function does four things: it measures what happened, forecasts what will happen, explains the difference, and gives someone a decision to make. AI is useful in the first and third, useful with supervision in the second, and irrelevant to the fourth.

The distinction matters commercially. A tool sold as "AI forecasting" that in fact does fast data assembly is still worth buying — it just should not be priced or governed as though it produces the forecast.

| Task | What a model does well | What still needs a person |
|---|---|---|
| Data assembly | Pulls commitments, invoices, timesheets and progress into one dated set; flags gaps | Deciding whether the cut-off is clean and the accruals are complete |
| Anomaly detection | Ranks transactions, quantities and durations that do not fit the pattern | Judging whether an outlier is an error, a genuine event, or the start of a trend |
| Document extraction | Reads contracts, variation orders and RFIs; pulls dates, values and obligations | Confirming the commercial meaning, especially of a disputed clause |
| Schedule checking | Finds open ends, negative lags, constraints, out-of-sequence progress | Deciding whether the logic reflects how the work will actually be built |
| Forecasting | Computes every method instantly and tests them against history | Choosing the method, because that is a statement about cause |
| Reporting | Drafts variance commentary in the house format | Owning the number in front of a board, a client or an auditor |
| Scenario testing | Runs hundreds of what-ifs faster than a workshop can discuss three | Choosing which scenario to fund |

Read the table as a division of labour, not a ranking. The left column is worth real money precisely because it frees people for the right column.

## How do you tell whether a model is good enough?

You measure it, on your own data, against outcomes you already know. Three numbers do most of the work: precision, recall and F1.

Precision is the share of the model's alarms that were real. Recall is the share of real problems the model caught. F1 is their harmonic mean, used when you need one number that punishes a model for being lopsided.

Take a cost-coding checker running over 12,000 transactions in a month. It flags 400. Review shows 120 of the flags were genuine miscodings, and a later audit finds 60 miscodings it never flagged.

- Precision = true positives ÷ all flags = 120 ÷ 400 = **0.30**
- Recall = true positives ÷ all real errors = 120 ÷ (120 + 60) = 120 ÷ 180 = **0.67**
- F1 = 2 × (0.30 × 0.67) ÷ (0.30 + 0.67) = 0.402 ÷ 0.97 = **0.41**

Now price it. Four hundred flags at six minutes of review each is 40 hours of a cost engineer's month, spent to recover 120 errors — and 60 errors still went through, so a sample check on the unflagged population is still required.

Tighten the confidence threshold and the trade moves. Suppose the model now flags 150, of which 90 are real, leaving 90 real errors missed.

Precision rises to 90 ÷ 150 = **0.60**, recall falls to 90 ÷ 180 = **0.50**, and F1 improves to 2 × 0.30 ÷ 1.10 = **0.55**. Review load drops to 15 hours.

The second setting is better on F1 and much cheaper to run, and it lets twice as many errors through. Which one you want depends on what a missed miscoding costs you, and that is a business decision, not a modelling one.

A vendor who will not run this test on your data has told you something. Ask for precision and recall on a golden set you control, with the review time priced in.

## The forecast a model cannot make for you

Estimate at completion is where the limit becomes obvious, because the arithmetic is trivial and the answer still depends on a judgement.

Take a programme with a budget at completion (BAC) of £120m. At the data date: planned value (PV) £48m, earned value (EV) £42m, actual cost (AC) £50m.

Cost variance is EV − AC = 42 − 50 = **−£8m**. Schedule variance is EV − PV = 42 − 48 = **−£6m**. The indices are CPI = 42 ÷ 50 = **0.84** and SPI = 42 ÷ 48 = **0.875**.

Four EAC methods, four answers, same inputs; [how to choose and defend an EAC method](https://projectcontrolsinstitute.org/four-eac-formulas) is the part that stays with a person.

| Method | Formula | What it assumes | Answer |
|---|---|---|---|
| Remaining work at budget | AC + (BAC − EV) | The overrun was a discrete event that will not repeat | £128.0m |
| Performance continues | BAC ÷ CPI | Cost performance to date is the best available predictor | £142.9m |
| Cost and schedule both bite | AC + (BAC − EV) ÷ (CPI × SPI) | Recovering the schedule will itself cost money | £156.1m |
| Bottom-up re-estimate | AC + a fresh estimate to complete | History says nothing useful about the work that remains | £136.0m |

Worked through: method one is 50 + 78 = £128.0m, and method two is 120 ÷ 0.84 = £142.9m.

Method three uses CPI × SPI = 0.84 × 0.875 = 0.735, so 50 + (78 ÷ 0.735) = 50 + 106.1 = £156.1m. Method four takes a re-priced remaining scope of £86m, giving 50 + 86 = £136.0m.

The spread is **£28.1m on identical inputs**. A model computes all four in milliseconds and can tell you which one has been closest on your last thirty projects. It cannot tell you whether the flood that caused this overrun is over.

One more number frames the conversation. The to-complete performance index needed to land on the original budget is (BAC − EV) ÷ (BAC − AC) = 78 ÷ 70 = **1.114**.

A team running at 0.84 is being asked to run at 1.11 for the rest of the job. Any recovery plan that does not explain that jump is a wish.

## Where the money actually leaks

The interesting failures are not inside cost engineering or inside finance. They are in the handover between them, and a model trained on one side has no idea the other side exists.

An engineer is examined on float, earning rules and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised and what a provision must satisfy, and almost never on a critical path. The number crosses that boundary every month, unchanged, and nobody in the chain has been examined on the crossing.

Here is the crossing in one step. Progress read from site photographs is physical completion, while revenue measured by the cost-to-cost input method is a ratio of costs incurred to total expected costs.

On a job where the expensive work is front-loaded, those two percentages are not close, and using the first to drive the second overstates margin.

A model that reports "the project is 40% complete" without saying which 40% it means has produced a number that is right for the site meeting and wrong for the ledger. That is not a modelling defect. It is a scoping defect, and it is the most common one in the market.

## What governed AI means in practice

Governed AI is the principle PCI certifies against: AI proposes, the professional disposes. The tool may generate the schedule, the forecast or the risk analysis; a competent human validates it, understands how it was produced, and owns the decision that follows.

That principle turns into five concrete requirements, and none of them is difficult.

**Provenance.** Every AI-assisted number in a report can be traced to its inputs and the version of the model that produced it. If you cannot reproduce last month's figure, you cannot defend it.

**A measured baseline.** Precision and recall on your own golden set, refreshed as the data changes. A model that was accurate on last year's contract mix is an assumption, not a control.

**A priced review step.** Somebody checks the output, the check takes time, and that time is in the budget. Unbudgeted review is the same as no review.

**Named accountability.** A person, not a function, owns each AI-assisted output. Auditors ask who; "the system" is not an answer.

**A failure route.** What happens when the tool is wrong, who finds out, and how fast. Confident wrongness is the characteristic failure mode of a language model, and it does not announce itself.

## Where AI is genuinely reshaping the work

Three shifts are visible in practice, and none of them is the one the marketing describes.

Reporting cycles compress. Work that took the first five days of a month — collecting, reconciling, drafting — collapses, and the released time either goes into analysis or gets absorbed. Which of those happens is a management choice made before the tool arrives.

Junior task mix changes fastest. The apprenticeship in this discipline was built on doing the assembly by hand, which is exactly the part now automated. Building judgement without that apprenticeship is an unsolved problem, and pretending otherwise does new entrants no favours.

Assurance moves upstream. When outputs are generated quickly, the control has to sit on the inputs and the method, not on the review of the finished pack. That is a different skill from checking a spreadsheet.

## How PCI examines this

PCI certifies three AI-era credentials, each with its own Body of Knowledge and examination, and [what an AI project controls credential should examine](https://pciai.org/ai-project-controls-certification) is the test to apply to any of them, including these.

| Credential | Full name | Shape | Centre of gravity |
|---|---|---|---|
| **PCL-AI** | PCI AI Project Controls Leader | 13 domains, 61 knowledge areas | The integrated controls discipline, cost through schedule to reported number |
| **PFL-AI** | PCI AI Project Finance Leader | 16 domains, 61 knowledge areas | Project finance, funding and financial reporting |
| **PML-AI** | PCI Project Management Leader – AI | 16 domains, 63 knowledge areas | Delivery leadership with controls literacy |

The PCL-AI Body of Knowledge is proportioned 40/40/20 across project accounting and finance, project management principles, and governed AI. The AI domain covers concepts, data, prompting, tooling, applied workflows, governance and capability — including evaluating outputs with golden sets and precision and recall, which is the arithmetic worked above.

Behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements, and 92 sector case studies across the three volumes (26 + 33 + 33). The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

PCI is an independent certifying body. Nothing on this page is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute, not law.

## Frequently asked questions

**Will AI replace project controls professionals?**
No, but it changes the task mix sharply. The assembly and checking work compresses, and the work that survives is choosing methods, defending forecasts and owning decisions. The risk to an individual is not redundancy; it is spending a career on the part that automates and never building the part that does not. For planners specifically, [what happens to the planning engineer's role](https://pciai.org/will-ai-replace-planning-engineers) is worked through case by case.

**Can AI produce an estimate at completion I can put in a board pack?**
It can produce every method and test them against your history, which is genuinely useful. It cannot choose between them, because the choice is a claim about what caused the variance and whether that cause persists. Present the range, name the method you chose, and say why.

**What accuracy should I demand from an AI tool?**
There is no universal threshold. Demand precision and recall measured on your own data, then price the review time each setting implies and the cost of the errors each setting misses. A model with 0.60 precision that saves 25 hours a month may beat one with 0.90 precision that flags almost nothing.

**Is our project data good enough for this?**
Usually the cost data is, the schedule data is patchy, and the link between them is the weak point; [what AI does with a live construction schedule](https://pciai.org/ai-for-construction-scheduling) depends almost entirely on that patchiness. Before buying anything, check whether your control accounts reconcile to your work breakdown structure and whether progress is measured by a rule that someone can state out loud.

**Does using AI create an audit problem?**
Only if you cannot show provenance. An AI-assisted number is no different from a spreadsheet-derived one: the auditor asks what the inputs were, what method was applied, who reviewed it and who owns it. Keep the model version and the input set with the output and the question is answerable.

---

*Linking note: one cross-estate link is in the body, to the hub's [how to choose and defend an EAC method](https://projectcontrolsinstitute.org/four-eac-formulas), placed at the point where four methods give four answers from identical inputs and the piece says the choice is a claim about cause. Three same-domain links sit in the sentences that raise them: what an AI credential should examine, beside PCI's own three; AI applied to a live construction schedule, in the FAQ about patchy schedule data; and the planning engineer's future, in the FAQ about replacement. As the pillar for this domain, it links out sparingly and takes cluster links back rather than pointing at every sibling. Reciprocal links worth making: each of the three cluster pages should cite this pillar once, naming the governed-AI line it depends on.*
