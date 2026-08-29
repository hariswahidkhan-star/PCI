---
platform:      Substack
type:          data-study
title:         Cost per square metre to build: reading a league table
meta:          A published cost per square metre to build ranks a normalised basket, not your building. The five adjustments that decide a city ranking, with the arithmetic.
primary_kw:    cost per square metre to build
secondary_kw:  construction cost benchmarking, gross internal area, location factor, comparable projects
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,420
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00306
---

# Cost per square metre to build: reading a league table

A published cost per square metre to build ranks a normalised basket of building types on a stated basis. It does not tell you what your building will cost in that city. Change the area definition, the scope inclusions, the currency basis, the specification or the date, and the order of the table changes with it.

*Written first for this newsletter. No published league table is reproduced here and no city rates are quoted. The worked example uses invented figures to show the method, which is the part that transfers.*

## What does a published cost per square metre to build include?

Usually less than the reader assumes, and the exclusions are in the methodology note rather than the headline.

Most international tables quote construction cost only: the contractor's works, main preliminaries and contractor overhead and profit. Land, professional fees, statutory charges, finance costs, client contingency and value added tax are frequently outside the line.

Fit-out is the largest single ambiguity. Shell and core, Category A and a fully fitted building can differ by a third for the same frame, and each of the three is called an office in a table.

## Which five adjustments decide the ranking?

| Adjustment | The question to ask | Why it moves the number |
|---|---|---|
| Area definition | Gross external, gross internal or net internal? | The same cost over a smaller denominator is a higher rate; GIA typically runs a few per cent below GEA |
| Scope inclusions | Fit-out, external works, fees, tax? | Fees and fit-out sit outside most quoted rates, and each is large enough to reverse a comparison on its own |
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

Then apply the adjustments in a fixed order and show each step: scope, area basis, time, then location and specification. A benchmark that arrives as one number with no ladder underneath it will not survive a challenge from a contractor who has priced the job, and it will not convert into [a control budget anyone can monitor against](https://projectcontrolsinstitute.org/cost-control-in-construction) either.

The order matters, because each step changes the number the next one works on. The two cities above, run as a ladder:

| Step | What it does | City A | City B |
|---|---|---:|---:|
| Quoted rate | As published, on its own basis | 2,850 (GEA) | 3,400 (GIA) |
| 1 Scope | Strip fees at 12%, then Category A fit-out at 340 | 2,850 | 2,696 |
| 2 Area basis | Put A's GEA rate onto GIA at 94% | 3,032 | 2,696 |
| 3 Time | Escalate A's 14-month-old base at 3.6% a year | 3,160 | 2,696 |
| 4 Location and specification | Judgement, stated in a sentence beside the number | — | — |

Steps one to three are arithmetic and reversible. Anybody can re-run them with a different assumption and see what it does, which is the whole point of showing them.

Step four is the one that has to be argued rather than calculated. Write the reasoning next to the number — which trades drove the location adjustment, which code requirement is priced into the specification — because a benchmark that hides both inside one multiplier cannot be interrogated by the person signing the budget.

Every rate should carry four fields with it: area basis, inclusion list, price base date and source. If any field is blank, the rate is an anecdote.

Benchmarking of this kind is learned from worked sector material rather than from rules of thumb. The PCI Bodies of Knowledge carry 92 sector case studies across the three volumes, split 26, 33 and 33.

## Frequently asked questions

**Why do two consultancies rank the same city differently?**
Because they measure different baskets on different bases. One may price a prime office to a specification standard the other applies to a mid-market building, and the currency treatment may be spot in one and purchasing power parity in the other. Neither is wrong. They answer different questions, and only the methodology note tells you which.

**Can an AI model pick your comparables?**
It can propose candidates, which is the tedious half, but only if somebody scores the proposals. On a validation run of 40 proposals, a cost manager confirmed 24 of the 30 genuine comparables in the set: precision 24 ÷ 40 = 0.60, recall 24 ÷ 30 = 0.80. Precision matters more here, because a false comparable enters the rate and gets signed. Whether anyone has measured that is [where a model helps an estimator and where it does not](https://pciai.org/ai-for-cost-estimating-in-construction).

**Is purchasing power parity or the spot rate the right conversion?**
Spot, if you are actually going to buy in that currency and pay from a home-currency budget, because that is the exposure you carry. Purchasing power parity is the better basis for comparing how expensive construction is relative to local incomes and local costs, which is a different question and a poor basis for a budget.

**How far can a location factor be trusted?**
As far as the trades it was built from. A composite factor blends labour, materials and plant, and labour is the component that varies most between cities. If your building is labour-intensive and the factor was derived from a materials-heavy basket, the adjustment will be wrong in a direction you cannot predict from the factor alone.

**What is the single most common error?**
Comparing a rate that includes professional fees with one that does not. In the example above, stripping fees at 12% moved City B by 364 per m², most of the 464 that separated the two cities once everything else was normalised, so check what fees added on the jobs you have priced rather than assuming a percentage. The second most common is mixing gross external and gross internal area: converting a GEA rate onto GIA raises it, as 2,850 becomes 3,032 above.

**Should a cost per square metre appear in a board paper at all?**
Yes, as a sense check on an estimate built from quantities, never as the estimate. It answers the question a board actually asks, which is whether the number looks like other buildings of this kind. State the basis in the same sentence, or expect to defend a comparison you did not make.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so nothing here duplicates a page the PCI site needs to rank.*

*Linking note: two links are in the piece, one per domain. "A control budget anyone can monitor against" sits in the section on building a defensible benchmark set (https://projectcontrolsinstitute.org/cost-control-in-construction), because that paragraph asks what a benchmark has to become before it is any use. "Where a model helps an estimator and where it does not" sits in the FAQ answer on scoring proposed comparables (https://pciai.org/ai-for-cost-estimating-in-construction), which is the answer that raises whether anyone has measured the tool; it is deliberately not the closing question, because the same link in the same last slot across several issues is itself a pattern. The classifier-scoring section that used to carry it was cut to that answer: 269 words on precision and recall is not what a reader searching for a cost per square metre came for, and the space went back to the adjustment ladder. Two corrections were needed on the way: cost control is a hub page, not a regional one, and AI for cost estimating lives on pciai.org rather than on the hub. Budgeting and forecasting was dropped, because the hub link is already spent. Reciprocal: none warranted.*
