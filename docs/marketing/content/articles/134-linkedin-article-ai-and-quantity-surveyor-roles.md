---
platform:      LinkedIn Article
type:          faq
title:         Will AI replace quantity surveyors? An honest answer
meta:          AI and quantity surveyor roles, answered with worked precision, recall and F1 on an automated take-off, and why a 0.90 score leaves a bill you cannot price.
primary_kw:    AI and quantity surveyor roles
secondary_kw:  automated take-off, precision recall F1, IFRS 15 five-step model, interim valuation
pillar:        AI in project controls
credential:    PFL-AI
target_domain: pciai.org
canonical:     original
schema:        FAQPage
word_count:    1563
hashtags:      #QuantitySurveying #ProjectControls #CostEngineering #ProjectFinance #AIGovernance
ab_id:         AB-00148
---

# Will AI replace quantity surveyors? An honest answer

Not the role, and the reason is measurable rather than sentimental. AI and quantity surveyor roles are being reshaped at the take-off, where automated classification is now good but not good enough to price from. A tool scoring 0.90 on F1 has still under-measured, and somebody has to find the missing 11%.

Written for LinkedIn as an original. It sits under the Institute's [AI in project controls](https://pciai.org/ai-in-project-controls) pillar.

## What does the evidence actually support?

Less than the headlines claim in either direction, and this piece is not going to give you a percentage of quantity surveying jobs at risk. No such figure exists in a form where the sample and the method can be inspected, and a number you cannot source is worse than no number.

What can be inspected is the performance of the tools themselves, because take-off and classification are measurable tasks with published-style metrics. That is a better basis for a career decision than a survey.

So the question becomes narrower and more useful. Which quantity surveying tasks does a measurable machine already do well, and what happens to the ones it does at 90%?

## How good is automated take-off, in numbers?

Take a drawing set containing 900 genuine items of one measured class. The tool returns 880 items, of which 800 are genuine.

That gives 800 true positives, 80 false positives and 100 false negatives.

**Precision** is true positives divided by everything flagged: 800 ÷ 880 = **0.909**. Of what the tool returned, 91% was real.

**Recall** is true positives divided by everything that exists: 800 ÷ 900 = **0.889**. Of what was there, 89% was found.

**F1** is the harmonic mean of the two, which is 2 × true positives divided by (2 × true positives + false positives + false negatives): 1,600 ÷ 1,780 = **0.899**.

An F1 of 0.90 is a respectable result in most machine learning contexts. In a bill of quantities it is a defect.

## What does 0.90 cost in money?

The netting is what makes this dangerous, because two large errors in opposite directions produce a small error in the total.

Suppose those 900 items carry £2.40m of value, an average of £2,667 each. The 100 missed items are worth roughly **£0.27m** at that average. The 80 spurious items add roughly **£0.21m** of value that does not exist.

The measured total lands at about **£2.35m** against a true £2.40m. Net understatement of **£0.05m**, close to 2%, which most reviewers would accept.

The gross error is **£0.48m** across 180 wrong lines, which is 20% of the value. Every one of those lines is a potential variation argument, a re-measurement, or a rate that gets set against a quantity that was never right.

That is the specific reason a 0.90 take-off cannot be priced without a surveyor. The total looks fine. The bill is wrong in 180 places, and the errors are not randomly distributed, because the items a classifier misses are the awkward junctions and non-standard details that also carry the highest rates.

## Why does a valuation have to survive the revenue standard?

Because an interim valuation is not only a payment document. It is the input to a number that appears in a set of accounts, and the accounting test is stricter than the contractual one.

[IFRS 15 for construction contracts](https://projectcontrolsinstitute.org/ifrs-15-for-construction) works in five steps, described here in the Institute's own words rather than reproduced.

**One, identify the contract.** An agreement with enforceable rights and obligations, commercial substance, and a realistic expectation of being paid.

**Two, identify the performance obligations.** The distinct promises inside that contract. On an integrated build there is usually one, because the contractor is combining many inputs into a single output the customer could not take separately.

**Three, determine the transaction price.** Including variable amounts such as variations, claims, incentives and liquidated damages, constrained so that only the portion highly unlikely to reverse is included.

**Four, allocate the price** across the obligations by relative standalone selling price, where more than one exists.

**Five, recognise revenue** as each obligation is satisfied. Where control transfers over time, that means a faithful measure of progress such as cost-to-cost. Otherwise nothing is recognised until control passes.

Steps three and five both run on the quantity surveyor's file. The measure of progress comes from measurement, and the constraint on claims is assessed head by head against entitlement and correspondence.

That is the finance and delivery overlap stated plainly. A chartered accountant is examined on when revenue may be recognised, and almost never on measurement. A surveyor is examined on measurement, and almost never on the constraint test. Projects lose money in the gap between the two.

## Where do AI and quantity surveyor roles actually diverge?

| Task | How exposed | What stays with the surveyor |
|---|---|---|
| Take-off from drawings and models | High | Everything the model does not contain, the non-standard details, and rules of measurement that need interpretation |
| Bill production and rate build-up | High | Rate judgement in a disturbed market, where history is a poor guide |
| Interim valuation preparation | Medium | Deciding what is properly installed, accepted and payable, as distinct from delivered |
| Variation and claim assessment | Medium | Entitlement, validity of instruction, and whether the amount survives the constraint |
| Cost planning and benchmarking | High where the data is clean | Knowing which historic project is genuinely comparable |
| Final account negotiation | Low | It is a negotiation. The other side also has tools |
| Risk allocation advice at tender | Low | Reading the contract for the clauses that move money quietly |

The exposed rows are the ones where the answer can be computed from a file. The protected rows are the ones where the answer depends on entitlement, acceptance or negotiation.

## What should a quantity surveyor be able to prove now?

Take a machine-produced take-off and state its recall against a sample you measured by hand. That single exercise turns AI from an opinion into a control.

Explain why a claim assessed as probable at £4.0m might enter revenue at £1.5m, and what evidence moves it.

Say what a contract asset is, why it is not a receivable, and what has to happen for the balance to move.

That combined ground, measurement discipline plus the accounting consequence, is what the [PCI AI Project Finance Leader (PFL-AI)](https://projectcontrolsinstitute.org/finance-and-project-management-certification) credential examines across 16 domains and 61 knowledge areas.

## Where should a firm actually deploy this?

On the tasks where a 10% miss is recoverable and the volume is high. Preliminary cost planning, order-of-magnitude checks, benchmarking, and first-pass take-off that a surveyor then audits.

Not on final accounts, not on valuations issued without review, and not on variation assessment where the constraint test has money attached.

The sensible operating rule is to set a recall target per task rather than a single accuracy figure for the tool. Cost planning can live with 0.90. A valuation cannot.

## Frequently asked questions

**Will AI replace quantity surveyors in the next decade?**
There is no sourced figure that answers this and anyone offering one should be asked for their sample and method. What is observable is that measurable, file-computable tasks are automating and judgement tasks are not. Firms are likely to need fewer surveyors per pound of take-off and more per pound of dispute, which changes the shape of the profession rather than its existence.

**Is automated take-off accurate enough to price from?**
Not yet, on the evidence of its own metrics. A recall of 0.889 means one item in nine is missing, and the missing items concentrate in non-standard details that carry the higher rates. It is accurate enough for cost planning and for a first pass that a surveyor audits against a hand-measured sample.

**What does F1 actually tell a surveyor?**
It balances two different failures in one number: measuring things that are not there, and missing things that are. A high F1 with unbalanced precision and recall still hides a systematic bias, so always look at the two underlying figures. For measurement work, recall matters more, because a missed item becomes a variation later.

**Should quantity surveyors learn data skills or contract law?**
Both, and contract law first if you have to choose. The tools reduce the value of measurement throughput and increase the value of knowing what a clause entitles you to. The data skill worth having is narrow: sampling, error rates and the ability to test a tool's output rather than trust it.

**Does this change what a surveyor should be certified in?**
It pushes certification towards the overlap. Measurement standards on their own describe an activity that is partly automating. Measurement plus the accounting consequence, plus the ability to govern a tool's output, describes the work that remains, which is why the AI-era credentials place governed AI alongside finance and project management rather than treating it as a separate subject.

---

*PCI publishes certification requirements. Nothing in this article is legal, tax or accounting advice, the standards named are described in the Institute's own words rather than reproduced, and no claim is made about employment outcomes.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Internal links: this article should link to [AI in project controls](https://pciai.org/ai-in-project-controls) as the pillar it supports, to [quantity surveyor certification](https://pciglobal.ai/quantity-surveyor-certification) with that anchor, and to [IFRS 15 for construction contracts](https://projectcontrolsinstitute.org/ifrs-15-for-construction) with that anchor.*
