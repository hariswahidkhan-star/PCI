---
platform:      Own site — credentialfinder.org
type:          practice
title:         CCP exam questions: what the paper actually tests you on
meta:          CCP exam questions test judgement under a scenario, not recall. Seven original practice problems worked in full, with the arithmetic shown line by line.
primary_kw:    CCP exam questions
secondary_kw:  cost engineering practice questions, estimate at completion, learning curve, TCPI
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: credentialfinder.org
canonical:     original
schema:        Article
word_count:    1709
hashtags:      n/a (own site)
ab_id:         —
---

# CCP exam questions: what the paper actually tests you on

CCP exam questions test whether you can reach a defensible number under a realistic scenario, not whether you can recall a definition. Expect estimating, cost control and earned value, economic analysis, and the commercial judgement around them — with a written component where you have to produce the answer rather than recognise it.

Everything below is original PCI material. It contains no AACE questions, because reproducing examination items breaches the certification agreement you sign and would be worthless preparation anyway.

## What CCP exam questions are built around

AACE publishes the scope and structure for the Certified Cost Professional in its own certification handbook. Take the current domain list and item counts from there, not from a study blog.

The shape is stable enough to prepare against. Four kinds of item recur, and each fails candidates differently.

| Item type | What it gives you | How candidates lose the mark |
|---|---|---|
| Status and forecast | A dataset at a cut-off date | Calculating correctly, then choosing a forecasting method they cannot defend |
| Estimate development | A reference cost and a change of scale, scope or date | Scaling linearly, or forgetting escalation between the reference date and today |
| Economic analysis | Cash flows, a rate and a horizon | Comparing options over different lives without normalising them |
| Written response | A situation and an audience | Producing arithmetic when the question asked for a recommendation |

The last row is where experienced people are caught. A senior reader wants the number and the assumption behind it, in that order.

## Seven practice problems, worked in full

Work each one with a pen before reading the answer. Recognising a worked solution feels like competence and is not.

### 1. Status and forecast

A package has a budget at completion of **£8.0m**. At the cut-off, planned value is **£3.4m**, earned value is **£3.1m** and actual cost is **£3.5m**.

Cost variance = EV − AC = 3.1 − 3.5 = **−£0.4m**.
Schedule variance = EV − PV = 3.1 − 3.4 = **−£0.3m**.
Cost performance index = 3.1 ÷ 3.5 = **0.886**.
Schedule performance index = 3.1 ÷ 3.4 = **0.912**.

Now forecast the outturn four ways. A re-estimate of the remaining work by the package engineers gives an estimate to complete of £5.3m.

| Method | Arithmetic | Result | What it assumes |
|---|---|---|---|
| AC + (BAC − EV) | 3.5 + 4.9 | **£8.40m** | The overrun was a one-off; the rest runs to budget |
| BAC ÷ CPI | 8.0 ÷ 0.886 | **£9.03m** | Performance to date is the best predictor of the rest |
| AC + (BAC − EV) ÷ (CPI × SPI) | 3.5 + 4.9 ÷ 0.808 | **£9.56m** | Cost and schedule pressure both persist |
| AC + bottom-up ETC | 3.5 + 5.3 | **£8.80m** | The remaining plan has been re-estimated line by line |

Variance at completion on the index method = 8.0 − 9.03 = **−£1.03m**. Four defensible answers spanning **£1.16m** from one dataset, and the mark is for naming the assumption you are prepared to stand behind.

### 2. The recovery test

Using the same figures, what performance does the remaining work need to finish on budget?

To-complete performance index = (BAC − EV) ÷ (BAC − AC) = 4.9 ÷ 4.5 = **1.089**.

Read it out loud. Work delivered so far has cost about 13% more than budgeted, since 1 ÷ 0.886 = 1.129, and the remainder must now be delivered about 8% cheaper than budgeted. Nothing in the data explains how, so the honest recommendation is to re-baseline or release contingency, not to promise recovery.

### 3. Scaling an estimate

A processing unit rated at **400 m³/h** cost **£12.0m** three years ago. Estimate a **650 m³/h** unit at today's prices, with escalation running at 4.5% a year.

Capacity ratio = 650 ÷ 400 = **1.625**. Applying a 0.6 exponent, 1.625⁰·⁶ = **1.338**.

Scaled cost = 12.0 × 1.338 = **£16.06m** at the old price level.
Escalation factor = 1.045³ = **1.1412**.
Estimate = 16.06 × 1.1412 = **£18.33m**.

Then show your work on the assumption. At an exponent of 0.7 the factor becomes 1.405, giving 12.0 × 1.405 × 1.1412 = **£19.24m**. A single exponent choice moves the estimate by about **£0.9m**, which is why the exponent, its source and the capacity range it was derived over belong in the basis of estimate.

### 4. Learning curve

A fabrication contract has a first unit at **1,200 hours** and a 90% cumulative-average learning curve. Estimate the hours for the first eight units.

Eight units is three doublings. Cumulative average at unit 8 = 1,200 × 0.9³ = 1,200 × 0.729 = **874.8 hours**.
Total for eight units = 8 × 874.8 = **6,998 hours**.

The trap is the model. On a unit-based curve, 874.8 hours would be the eighth unit alone and the total would be materially higher. State which model you used, because an examiner and a commercial manager will both ask.

### 5. Float and the cost of a delay

Five activities. Excavation P takes 12 days from the start. Formwork Q takes 10 days and follows P. Rebar R takes 6 days and follows Q. Mechanical procurement S takes 30 days from the start. Installation T takes 8 days and needs both R and S complete.

Forward pass, in days from zero: P runs 0 to 12, Q runs 12 to 22, R runs 22 to 28, S runs 0 to 30. T waits for the later of 28 and 30, so it runs 30 to **38**.

The project takes **38 days** and the critical path is S → T, because 30 + 8 = 38.

Backward pass down the other chain: T must start by day 30, so R must finish by 30 and start by 24, Q must finish by 24 and start by 14, and P must finish by 14 and start by 2.

Total float is **2 days** on P, Q and R — the same two days shared once along the chain, not two days each. Free float tells a different story: P has 12 − 12 = **zero**, Q has 22 − 22 = **zero**, and R has 30 − 28 = **2 days**. All the slack sits at the end of the chain.

Now delay Q by five days. Q runs 12 to 27, R runs 27 to 33, T runs 33 to **41**. Two days were absorbed by float and three days landed on the completion date, which is the difference between a delay and a compensable delay.

### 6. Retention, priced

A subcontract is valued at **£1.8m** a month with **5%** retention withheld. What does retention cost the subcontractor over six months?

Retention withheld each month = 1.8m × 0.05 = **£90,000**.
Held after six months = 6 × 90,000 = **£540,000**.
Financing cost at 8%, over an average holding period of nine months = 540,000 × 0.08 × 0.75 = **£32,400**.

That is real money nobody budgets, and it is the sort of number a written response is looking for: the amount, the rate, the holding period, and the assumption that the release date is what the contract says it is.

### 7. Judging an automated check

A tool reviews **5,200** invoices for miscoding and flags **480**. Inspection confirms **312** are genuinely miscoded. A manual audit of the rest finds **96** miscoded invoices the tool missed.

Precision = 312 ÷ 480 = **0.650**.
Recall = 312 ÷ (312 + 96) = 312 ÷ 408 = **0.765**.
F1 = (2 × 0.650 × 0.765) ÷ (0.650 + 0.765) = **0.703**.

Now price it. Reviewing 480 flags at twelve minutes each is **96 hours** of somebody's month to catch 312 genuine errors, while 96 miscoded invoices still reach the cost report. Whether that trade is worth making is a cost decision, and stating it in those terms is what separates a controls professional from a tool operator.

## How to use these

Do them cold, then do them again a fortnight later. Practice questions build recall when you read them and judgement when you re-derive them.

Write one short recommendation for each: three sentences naming the number, the assumption and what you would do next. That is the written component in miniature, and it is the part that cannot be crammed.

Track which ones you got wrong for the right reason. A slip in arithmetic is a different problem from choosing a forecasting method you cannot defend, and only the second one fails you.

## Where the cost paper stops

None of the problems above asks what your forecast does to the accounts, and neither does a cost examination.

Move the estimate at completion in problem one from £8.40m to £9.56m and you move reported progress, reported revenue and reported margin on that contract, because where progress is measured by costs incurred against total expected costs, your forecast is the denominator. If forecast cost passes the contract price, the expected loss is recognised in full as soon as it is known.

A chartered accountant is examined on recognition and provisions, almost never on an earning rule. An engineer is examined on progress and float, almost never on cut-off.

PCI examines both sides in one syllabus: PCI AI Project Finance Leader (PFL-AI) holds 16 domains and 61 knowledge areas, and every PCI Body of Knowledge holds the same proportions of 40% finance and reporting, 40% project management and 20% governed AI. The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

## Frequently asked questions

**Are there official CCP practice papers?**
AACE publishes preparation material and recommended practices through its own store, and that is the source to use. Treat any site offering the actual examination items as a risk to your certification rather than a shortcut, because reproducing or using leaked items breaches the agreement you sign as a candidate.

**How many practice problems are enough?**
Fewer than most candidates think, worked properly. Thirty problems re-derived from a blank page beats three hundred read through once. The measure is whether you can produce the answer and the assumption without looking, not how many you have seen.

**What arithmetic should I be fluent in before booking?**
Earned value status and the four forecasting methods, to-complete performance index, present value and annuity factors, escalation, capacity-factor scaling, learning curves, and float. If any of those needs a lookup mid-problem, you are not ready for a timed paper.

**Is the written part harder than the multiple choice?**
It is different, and it is the part working professionals under-prepare. Practise writing to a page limit for a named audience, because a technically perfect answer that never states a recommendation loses marks that the arithmetic already earned.

**Should I memorise formulas?**
Memorise the small set above and understand what each one assumes. A formula recalled without its assumption produces confident wrong answers, which is exactly the failure mode a scenario examination is designed to catch.

---

*Internal links: this page should link to [project controls exam questions worked in full](https://projectcontrolsinstitute.org/project-controls-exam-questions) with that anchor, to [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with that anchor, to [whether CCP certification is worth it](https://credentialfinder.org/ccp-certification-worth-it) with that anchor, and to [earned value practice questions](https://projectcontrolsinstitute.org/earned-value-practice-questions) with that anchor; the AACE certification cost page should link back here with the anchor "what CCP exam questions actually test".*
