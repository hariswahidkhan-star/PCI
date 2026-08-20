---
platform:      Substack
type:          data-study
title:         Cost per square metre: reading the city league table
meta:          A published cost per square metre to build ranks a normalised basket, not your building. The five adjustments that decide a city ranking, with the arithmetic.
primary_kw:    cost per square metre to build
secondary_kw:  construction cost benchmarking, gross internal area, location factor, comparable projects
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: pciglobal.ai
canonical:     original
schema:        Article
word_count:    1331
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00306
---

# Cost per square metre: reading the city league table

A city league table ranks a normalised basket of building types on a stated basis. It does not tell you what your building will cost in that city. Change the area definition, the scope inclusions, the currency basis, the specification or the date, and the order of the table changes with it.

*Written first for this newsletter. No published league table is reproduced here and no city rates are quoted. The worked example uses invented figures to show the method, which is the part that transfers.*

## What does a published cost per square metre to build include?

Usually less than the reader assumes, and the exclusions are in the methodology note rather than the headline.

Most international tables quote construction cost only: the contractor's works, main preliminaries and contractor overhead and profit. Land, professional fees, statutory charges, finance costs, client contingency and value added tax are frequently outside the line.

Fit-out is the largest single ambiguity. Shell and core, Category A and a fully fitted building can differ by a third for the same frame, and each of the three is called an office in a table.

## Which five adjustments decide the ranking?

| Adjustment | The question to ask | Why it moves the number |
|---|---|---|
| Area definition | Gross external, gross internal or net internal? | The same cost over a smaller denominator is a higher rate; GIA typically runs a few per cent below GEA |
| Scope inclusions | Fit-out, external works, fees, tax? | Professional fees alone commonly add a tenth to the construction figure |
| Currency basis | Spot rate or purchasing power parity? | A currency move re-orders a table without a single rate changing |
| Specification and code | Seismic, thermal, fire, accessibility, ground | Local code is not an optional upgrade; it is embedded in the rate |
| Date and market | What price base, and what were tender conditions? | A rate is a snapshot of a market, and markets in different cities are not in phase |

Two of these are arithmetic and three are judgement. The arithmetic ones are where most of the error lives, because they look settled and are not.

## Worked example: two cities, and the ranking flips

City A is quoted at 2,850 per m² on gross external area, excluding fit-out and professional fees. City B is quoted at 3,400 per m² on gross internal area, including Category A fit-out at 340 per m² and professional fees at 12%. Both figures are invented for this example.

Read at face value, City B is 19% more expensive. Now put them on the same basis.

Strip B back to A's scope. Remove fees: 3,400 ÷ 1.12 = **3,036**. Remove fit-out: 3,036 − 340 = **2,696** per m² of GIA.

Put A on the same denominator. If GIA is 94% of GEA for this building type, then per GIA m²: 2,850 ÷ 0.94 = **3,032**.

City A 3,032, City B 2,696. The ranking has reversed, and nothing about either building has changed.

Now the date. A's rate is 14 months older, and assume 3.6% annual escalation in that market: 3,032 × 1.036 ^ (14 ÷ 12) = 3,032 × 1.0421 = **3,160**.

The normalised gap is 3,160 − 2,696 = 464 per m², or 17%. On a 22,000 m² building that is 464 × 22,000 = **10.2m** of currency, decided entirely by adjustments made after the table was read.

## How do you build a benchmark set you can defend?

Start from projects you have delivered or priced, not from published averages. Two of your own comparables, properly analysed, beat twenty rates whose basis you cannot reconstruct.

Record each comparable at the elemental level rather than as a single rate. A total rate cannot be adjusted honestly, because you cannot tell whether the difference sits in the substructure, the frame or the services.

Then apply the adjustments in a fixed order and show each step: area basis, scope, location factor, time factor, market conditions, specification. A benchmark that arrives as one number with no ladder underneath it will not survive a challenge from a contractor who has priced the job.

Every rate should carry four fields with it: area basis, inclusion list, price base date and source. If any field is blank, the rate is an anecdote.

Benchmarking of this kind is taught through worked sector material rather than rules of thumb. The PCI AI Project Finance Leader (PFL-AI) syllabus and its companion volumes carry 92 sector case studies across the three volumes, split 26, 33 and 33.

## Can an AI model pick your comparables?

It can propose them, and that is worth having, because the tedious part of benchmarking is finding candidates rather than adjusting them. What matters is measuring how well it proposes.

Score it like any classifier. Take a validation run: the model proposes 40 projects as comparable, a cost manager confirms 24 of them, rejects 16, and separately identifies 6 genuine comparables the model missed. There were 30 genuine comparables in the set.

Precision = correct proposals ÷ all proposals = 24 ÷ 40 = **0.60**. Recall = correct proposals ÷ all genuine comparables = 24 ÷ 30 = **0.80**.

F1 is the harmonic mean of the two: 2 × (0.60 × 0.80) ÷ (0.60 + 0.80) = 0.96 ÷ 1.40 = **0.686**.

| Model setting | Proposed | Correct | Precision | Recall | F1 |
|---|---:|---:|---:|---:|---:|
| Default threshold | 40 | 24 | 0.60 | 0.80 | 0.686 |
| Tightened threshold | 22 | 19 | 0.864 | 0.633 | 0.731 |

For benchmarking, precision is worth more than recall. A missed comparable costs you a review you never did; a false comparable enters the rate and gets signed. Tighten the threshold, accept that you will miss some, and keep the human review on the survivors.

Report both numbers whenever a model is used in an estimate. A tool described only as "AI-assisted", with no precision figure and no validation set, cannot be relied on in an estimate basis document.

## Frequently asked questions

**Why do two consultancies rank the same city differently?**
Because they measure different baskets on different bases. One may price a prime office to a specification standard the other applies to a mid-market building, and the currency treatment may be spot in one and purchasing power parity in the other. Neither is wrong. They answer different questions, and only the methodology note tells you which.

**Is purchasing power parity or the spot rate the right conversion?**
Spot, if you are actually going to buy in that currency and pay from a home-currency budget, because that is the exposure you carry. Purchasing power parity is the better basis for comparing how expensive construction is relative to local incomes and local costs, which is a different question and a poor basis for a budget.

**How far can a location factor be trusted?**
As far as the trades it was built from. A composite factor blends labour, materials and plant, and labour is the component that varies most between cities. If your building is labour-intensive and the factor was derived from a materials-heavy basket, the adjustment will be wrong in a direction you cannot predict from the factor alone.

**What is the single most common error?**
Comparing a rate that includes professional fees with one that does not. Fees are commonly around a tenth of construction cost, which is larger than most genuine differences between comparable cities. The second most common is mixing gross external and gross internal area, which typically moves a rate by a few per cent in the opposite direction.

**Should a cost per square metre appear in a board paper at all?**
Yes, as a sense check on an estimate built from quantities, never as the estimate. It answers the question a board actually asks, which is whether the number looks like other buildings of this kind. State the basis in the same sentence, or expect to defend a comparison you did not make.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so nothing here duplicates a page the PCI site needs to rank.*

*Internal links: this piece should link to [cost control in construction](https://pciglobal.ai/cost-control-in-construction) with the anchor "how a benchmark becomes a control budget", to [AI for cost estimating in construction](https://projectcontrolsinstitute.org/ai-for-cost-estimating-in-construction) with the anchor "where a model helps an estimator and where it does not", and to [project budgeting and forecasting](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) with the anchor "turning a benchmark into a budget you can control".*
