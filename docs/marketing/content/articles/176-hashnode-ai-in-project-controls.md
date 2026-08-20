---
platform:      Hashnode
type:          pillar
title:         AI in project controls: what a model can and cannot do
meta:          What AI in project controls does well, how to score a flagging model on your own data with precision, recall and F1, and the forecast it cannot choose.
primary_kw:    AI in project controls
secondary_kw:  precision and recall, F1 score, estimate at completion, governed AI
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     canonical -> pciai.org/ai-in-project-controls
schema:        Article
word_count:    2406
hashtags:      #machinelearning #python #datascience #tutorial
ab_id:         AB-00038
---

# AI in project controls: what a model can and cannot do

AI in project controls is the governed use of machine learning and language models to assemble, check and explain the numbers a controls function reports. The useful part is measurable: precision, recall and F1 on data you have labelled yourself. Choosing which forecast to publish is not model work, because that choice is a claim about cause.

This is the engineering version of that argument. Code where code helps, arithmetic you can check by hand, and a clear line around the part of the job that stays human.

## What does AI in project controls actually do?

A controls function measures what happened, forecasts what will happen, explains the difference, and hands someone a decision. Models are strong in the first and third, useful under supervision in the second, and irrelevant to the fourth.

The distinction is commercial as well as technical. A tool sold as "AI forecasting" that in fact does fast data assembly is still worth buying. It should not be priced or governed as though it produces the forecast.

| Task | What a model does well | What still needs a person |
|---|---|---|
| Data assembly | Pulls commitments, invoices, timesheets and progress into one dated set; flags gaps | Deciding whether the cut-off is clean and the accruals are complete |
| Anomaly detection | Ranks transactions, quantities and durations that do not fit the pattern | Judging whether an outlier is an error, a genuine event or a new trend |
| Document extraction | Reads contracts, variation orders and RFIs; pulls dates, values and obligations | Confirming the commercial meaning, especially of a disputed clause |
| Schedule checking | Finds open ends, negative lags, hard constraints, out-of-sequence progress | Deciding whether the logic reflects how the work will actually be built |
| Forecasting | Computes every method instantly and backtests them against history | Choosing the method, because that is a statement about cause |
| Reporting | Drafts variance commentary in the house format | Owning the number in front of a board, a client or an auditor |
| Scenario testing | Runs hundreds of what-ifs faster than a workshop discusses three | Choosing which scenario to fund |

Read that as a division of labour rather than a ranking. The left column is worth real money because it frees people for the right column.

## Why accuracy is the wrong metric here

Project controls anomalies are rare events, and rare events break accuracy as a measure. A month with 12,000 cost transactions might hold 180 genuine miscodings, a base rate of 1.5%.

A model that flags nothing at all is 98.5% accurate on that data. It is also worthless, and any vendor quoting accuracy on an imbalanced set is either careless or counting on you being.

Precision and recall survive the imbalance because they ignore the vast true-negative population. Precision is the share of alarms that were real. Recall is the share of real problems the model caught.

## Scoring a flagging model on your own data

Score the model on a golden set: a period you have already reviewed line by line, where you know the answer. Twenty lines of Python does the rest.

```python
def score(flagged: set[int], truth: set[int]) -> dict[str, float]:
    """flagged and truth are sets of transaction ids."""
    tp = len(flagged & truth)
    fp = len(flagged - truth)
    fn = len(truth - flagged)
    precision = tp / (tp + fp) if tp + fp else 0.0
    recall    = tp / (tp + fn) if tp + fn else 0.0
    f1 = (2 * precision * recall / (precision + recall)
          if precision + recall else 0.0)
    return {"tp": tp, "fp": fp, "fn": fn,
            "precision": round(precision, 3),
            "recall": round(recall, 3),
            "f1": round(f1, 3),
            "review_hours": round(len(flagged) * 6 / 60, 1)}
```

The `review_hours` line is the one people leave out. Every flag costs a cost engineer about six minutes, so the confusion matrix has a wage bill attached to it.

Run the same golden set at several confidence thresholds and the trade-off becomes visible. Here is one model over that month of 12,000 transactions, with 180 real miscodings.

| Threshold | Flags | True positives | Precision | Recall | F1 | Errors missed | Review hours |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 0.30 | 600 | 150 | 0.250 | 0.833 | 0.385 | 30 | 60.0 |
| 0.50 | 400 | 120 | 0.300 | 0.667 | 0.414 | 60 | 40.0 |
| 0.70 | 150 | 90 | 0.600 | 0.500 | 0.545 | 90 | 15.0 |
| 0.85 | 60 | 48 | 0.800 | 0.267 | 0.400 | 132 | 6.0 |

Check one row by hand. At 0.70 precision is 90 ÷ 150 = 0.600 and recall is 90 ÷ 180 = 0.500, so F1 is 2 × 0.600 × 0.500 ÷ 1.100 = 0.545.

F1 peaks in the middle because it is the harmonic mean and punishes lopsidedness. The harmonic mean is why a model that flags everything cannot game the score: recall goes to 1.0 and precision collapses.

Now read the table as a manager rather than a data scientist. Moving from 0.50 to 0.70 saves 25 hours of review a month and lets 30 more miscodings through.

That is not a modelling decision. It depends on what an undetected miscoding costs you when it reaches a capitalisation judgement or a client application, and only your organisation knows that number.

A vendor who will not run this on your golden set has told you something. Ask for precision, recall and the review hours each threshold implies, measured on your data, not theirs.

## The forecast a model cannot choose for you

Estimate at completion is where the limit gets obvious, because the arithmetic is trivial and the answer still turns on a judgement.

```python
def eac(bac, pv, ev, ac, etc_bottom_up=None):
    cpi, spi = ev / ac, ev / pv
    return {
        "cv": ev - ac,
        "sv": ev - pv,
        "cpi": round(cpi, 3),
        "spi": round(spi, 3),
        "remaining_at_budget":  ac + (bac - ev),
        "performance_continues": bac / cpi,
        "cost_and_schedule":     ac + (bac - ev) / (cpi * spi),
        "bottom_up": None if etc_bottom_up is None else ac + etc_bottom_up,
        "tcpi_to_bac": (bac - ev) / (bac - ac),
    }

eac(bac=120, pv=48, ev=42, ac=50, etc_bottom_up=86)
```

Take a programme with a budget at completion (BAC) of £120m. At the data date, planned value (PV) is £48m, earned value (EV) is £42m and actual cost (AC) is £50m.

Cost variance is EV − AC = −£8m. Schedule variance is EV − PV = −£6m. The indices are CPI = 42 ÷ 50 = **0.84** and SPI = 42 ÷ 48 = **0.875**.

Four methods, four answers, identical inputs.

| Method | Formula | What it assumes | Answer |
|---|---|---|---|
| Remaining work at budget | AC + (BAC − EV) | The overrun was a discrete event that will not repeat | £128.0m |
| Performance continues | BAC ÷ CPI | Cost performance to date is the best available predictor | £142.9m |
| Cost and schedule both bite | AC + (BAC − EV) ÷ (CPI × SPI) | Recovering the schedule will itself cost money | £156.1m |
| Bottom-up re-estimate | AC + a fresh estimate to complete | History says nothing useful about the work that remains | £136.0m |

Worked through: 50 + 78 = £128.0m, and 120 ÷ 0.84 = £142.9m. Method three uses CPI × SPI = 0.735, so 50 + (78 ÷ 0.735) = £156.1m, and method four takes a re-priced remaining scope of £86m for £136.0m.

The spread is **£28.1m on the same inputs**. A model returns all four in microseconds and can tell you which one has been closest across your last thirty projects, which is genuinely useful evidence.

It cannot tell you whether the flood that caused this overrun is over. That is the difference between backtesting and causation, and no amount of training data closes it.

One more figure frames the recovery conversation. The to-complete performance index needed to land on the original budget is (BAC − EV) ÷ (BAC − AC) = 78 ÷ 70 = **1.114**.

A team running at 0.84 is being asked to run at 1.11 for the remainder of the job. Any recovery plan that does not explain that jump is a wish with a Gantt chart attached.

## Where the money actually leaks

The expensive failures are not inside cost engineering or inside finance. They sit in the handover between them, and a model trained on one side has no representation of the other.

Progress read from site is physical completion. Revenue measured by a cost-to-cost input method is a ratio of costs incurred to total expected costs. They are different quantities that share a percent sign.

Take a contract priced at £11.0m with expected total cost of £8.4m and £2.8m of cost incurred. Cost-to-cost progress is 2.8 ÷ 8.4 = **33.3%**, so revenue to date is 0.333 × 11.0 = **£3.667m**.

Site reports 40% physically complete, because the heavy structural work is done. Apply that instead and revenue to date is 0.40 × 11.0 = **£4.400m**, a difference of **£733,000** in one month on one contract.

A model that reports "the project is 40% complete" without saying which 40% has produced a number that is right for the site meeting and wrong for the ledger. That is a scoping defect, not a modelling defect, and it is the most common one in the market.

An engineer is examined on float, earning rules and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised, and almost never on a critical path. The number crosses that boundary every month and nobody in the chain has been examined on the crossing.

## What governed AI looks like in a repository

Governed AI is the principle PCI certifies against: AI proposes, the professional disposes. In engineering terms it is five artefacts, and none of them is hard to build.

**Provenance.** Every AI-assisted number in a report resolves to its inputs and the model version that produced it. Store the record beside the output.

```json
{
  "output_id": "eac-2026-07-programme-14",
  "produced_at": "2026-07-31T18:04:11Z",
  "model": "cost-anomaly-v3.2",
  "inputs": ["ledger_2026-07.csv#sha256:9f2c...",
             "p6_export_2026-07-31.xer#sha256:41ab..."],
  "method": "performance_continues",
  "reviewed_by": "a.okafor",
  "accepted": true
}
```

If last month's figure cannot be reproduced from that record, it cannot be defended when someone asks in month nine.

**A measured baseline.** Precision and recall on your golden set, refreshed as the contract mix changes. A model that scored well on last year's data is an assumption until re-measured.

**A priced review step.** Somebody checks the output, the check takes hours, and those hours sit in a budget. Unbudgeted review is the same as no review, and it fails first when the month is busy.

**Named accountability.** A person owns each AI-assisted output, not a function and not a system. Auditors ask who, and "the model" is not an answer.

**A failure route.** What happens when the tool is wrong, who finds out and how quickly. Confident wrongness is the characteristic failure mode of a language model, and it does not announce itself in the output.

## What actually changes in the job

Reporting cycles compress. The collecting, reconciling and drafting that filled the first five days of a month collapses, and the released time either goes into analysis or gets quietly absorbed. Which one happens is a management decision taken before the tool arrives.

The junior task mix changes fastest. The apprenticeship in this discipline was built on doing the assembly by hand, which is exactly the part now automated. Building judgement without that apprenticeship is an unsolved problem, and pretending otherwise does new entrants no favours.

Assurance moves upstream. When outputs are generated in seconds, the control has to sit on the inputs and the method rather than on a review of the finished pack. That is a different skill from checking a spreadsheet, and most teams have not hired for it.

## How PCI examines this

PCI certifies three AI-era credentials, each with its own Body of Knowledge and examination.

| Credential | Full name | Shape |
|---|---|---|
| **PCL-AI** | PCI AI Project Controls Leader | 13 domains, 61 knowledge areas |
| **PFL-AI** | PCI AI Project Finance Leader | 16 domains, 61 knowledge areas |
| **PML-AI** | PCI Project Management Leader – AI | 16 domains, 63 knowledge areas |

Each Body of Knowledge runs in a 40 / 40 / 20 proportion across finance and reporting, project management, and governed AI. Evaluating model outputs with golden sets, precision and recall sits in that last block, which is why the arithmetic above is examinable material rather than commentary.

Behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements, and 92 sector case studies across the three volumes (26 + 33 + 33). The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

PCI is an independent certifying body and claims no accreditation, endorsement or affiliation with any other organisation. Nothing published here is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Will AI replace project controls professionals?**
No, but it changes the task mix sharply. Assembly and checking compress, and what survives is choosing methods, defending forecasts and owning decisions. The risk to an individual is not redundancy. It is spending a career on the part that automates and never building the part that does not.

**Can a model produce an estimate at completion I can put in a board pack?**
It can produce every method and backtest them against your history, which is real evidence. It cannot choose between them, because the choice asserts what caused the variance and whether that cause persists. Publish the range, name the method you selected, and say why in one sentence.

**What accuracy should I demand from a tool?**
There is no universal threshold, and anyone quoting one has not seen your data. Demand precision and recall on your own golden set, then price the review hours each threshold implies against the cost of the errors it misses. A model at 0.60 precision that saves 25 hours a month can beat one at 0.90 that flags almost nothing.

**Is our data good enough to start?**
Usually the cost data is, the schedule data is patchy, and the join between them is the weak point. Before buying anything, check that control accounts reconcile to the work breakdown structure and that progress is measured by a rule someone can state out loud. Both problems are cheaper to fix than to model around.

**Does AI create an audit problem?**
Only when provenance is missing. An AI-assisted number is no different from a spreadsheet-derived one: the auditor asks what the inputs were, what method was applied, who reviewed it and who owns it. Keep the model version and the input hashes with the output and every one of those is answerable.

---

*First published on pciai.org; this Hashnode version is flagged as republished in Draft Settings with the canonical pointing there. Reach here comes from tag feeds, not from search.*

*Internal links: this piece should link to [AI project controls certification](https://pciai.org/ai-project-controls-certification) with the anchor "what an AI project controls credential should examine", to [AI for cost estimating in construction](https://pciai.org/ai-for-cost-estimating-in-construction) with the anchor "measuring estimating error and bias on your own jobs", and to [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with the anchor "how to choose and defend an EAC method".*
