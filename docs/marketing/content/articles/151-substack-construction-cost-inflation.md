---
platform:      Substack
type:          data-study
title:         Construction cost inflation: what an index really shows
meta:          Construction cost inflation indices measure three different things. How to read them, and how to turn a published rate into a defensible escalation allowance.
primary_kw:    construction cost inflation
secondary_kw:  tender price index, escalation allowance, price base date, cost-to-cost input method
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: pciglobal.ai
canonical:     original
schema:        Article
word_count:    1461
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00304
---

# Construction cost inflation: what an index really shows

A construction cost inflation index shows what a defined basket of construction cost did over a defined period, on a defined basis. It does not show what your project will cost. The headline percentage is an average of someone else's basket, measured to a date that is almost certainly not your price base date.

*Written first for this newsletter. No figures are reproduced from any published index. The arithmetic below uses assumptions you would have to source yourself, and every one of them is flagged as such.*

## What does a construction cost inflation index actually measure?

Three families of index exist, and they answer different questions. Treating one as a substitute for another is the most common error in an escalation allowance.

An input cost index tracks the price of what a contractor buys: wages, materials, plant and fuel, weighted into a fixed basket. It moves when steel or labour rates move, and it says nothing about what contractors are able to charge.

A tender price index tracks the level of accepted tenders. It carries input cost movement plus the margin and risk pricing that market conditions allow, which is why it can fall in a downturn while input costs are still rising.

An output price index, published by national statistics offices, tracks prices charged for construction work. It behaves more like a tender price index than an input one and is usually published with a longer lag.

| Index family | What it tracks | What it excludes | Use it for |
|---|---|---|---|
| Input cost index (the BCIS general building cost family in the UK) | Resource prices: wages, materials, plant | Contractor margin, market capacity | Escalating a resourced estimate; fluctuation clauses |
| Tender price index (published by BCIS and by cost consultancies) | The level of accepted tenders | Post-contract change and claims | Forecasting what a tender will come back at |
| Output price index (national statistics offices) | Prices charged for construction output | Anything project-specific | Long-run trend, macro comparison |
| Your own project basket | Your resource weights, your supply chain | Everything you do not buy | The escalation line in your own estimate |

The annual international market surveys published by the large cost consultancies sit across all three. Read their methodology page before their headline, because the basis changes between editions and between cities.

## Why the headline percentage is not your escalation allowance

An index rate is annual, and it is anchored to the index's own base period. Your allowance has to run from your price base date to the point at which you actually spend the money.

Those are different dates and different durations, and the gap between them is usually larger than the difference between one published rate and another.

The mid-point that matters is the mid-point of spend, not the mid-point of time. Front-loaded procurement pulls it earlier; a long commissioning tail pushes it later.

## How do you build the escalation line? A worked example

Take a package with a base estimate of £48.0m at a price base date of Q1 2026, constructed between Q3 2026 and Q1 2029. All figures here are illustrative.

First, build the rate from your own basket rather than taking a single headline. Assume labour is 42% of the estimate and escalating at 5.2%, materials 38% at 2.6%, and plant and other 20% at 3.1%.

Weighted rate = (0.42 × 5.2) + (0.38 × 2.6) + (0.20 × 3.1) = 2.184 + 0.988 + 0.620 = **3.79% per annum**.

Second, find the mid-point of spend. On this S-curve it falls at month 16 of the 30-month programme, which is 21 months after the price base date.

Third, compound: factor = 1.0379 ^ (21 ÷ 12) = 1.0379 ^ 1.75 = **1.0672**. Escalated estimate = 48.0 × 1.0672 = **£51.23m**, so the escalation allowance is **£3.23m**.

Compare that with the shortcut most estimates carry, which is one year at the headline rate: 48.0 × 1.0379 = £49.82m, an allowance of £1.82m. The shortcut understates the exposure by £1.41m on a £48m package, and it does so silently.

## What escalation does to revenue, not just to cost

This is the step that turns an estimating adjustment into a reported number, and it catches teams out at the first period after a re-forecast.

Where progress towards a performance obligation is measured by a cost-based input method, the measure is costs incurred divided by total expected costs. Raising the escalation allowance raises total expected costs, so the percentage complete falls even though nothing on site changed.

Continue the example. Costs incurred are £12.0m and the contract price is £54.0m.

| Total expected costs | Costs incurred ÷ total expected | Cumulative revenue |
|---|---:|---:|
| £48.0m (no escalation carried) | 12.0 ÷ 48.0 = 25.0% | £13.50m |
| £51.23m (escalation carried) | 12.0 ÷ 51.23 = 23.4% | £12.65m |

That is £0.85m of revenue that has to come back out, produced by an assumption about future prices. It is the clearest case there is for telling finance about an escalation change in the month you make it, not at year end.

The five-step revenue model behind that, in the Institute's own words rather than reproduced from the standard:

1. **Identify the contract** — an agreement with a customer creating enforceable rights and obligations, with commercial substance and a considered view on collection.
2. **Identify the performance obligations** — the distinct promises inside it, which on an integrated construction contract are frequently combined into one.
3. **Determine the transaction price** — including variable elements such as variations, claims and incentives, constrained so revenue is not taken where a significant reversal is expected.
4. **Allocate the price** across those obligations where there is more than one.
5. **Recognise revenue as obligations are satisfied**, over time where the criteria are met, measured by a chosen method of progress. The cost-based input method is the one your escalation assumption drives.

Nothing here is accounting advice. The point for a controls team is narrower: your escalation assumption is an input to reported revenue, so it needs an owner and a date.

## Where an index tells you nothing

An index cannot see your resource mix. A steel-intensive frame and a labour-intensive fit-out move at different rates in the same city in the same quarter.

It cannot see your market. National averages hide the single-city, single-trade shortage that produces most escalation pain, and by the time it shows in a national series your tenders have already returned.

It cannot see your contract. A fixed price does not remove escalation risk, it prices it, and the price is paid whether or not the risk occurs. A fluctuation clause moves the risk back to you and names the index that will settle it, which makes the choice of index a commercial decision rather than a technical one.

Judgements of this kind sit in the cost control and estimating domains of the PCI AI Project Finance Leader (PFL-AI) syllabus, which covers 16 domains and 61 knowledge areas.

## Frequently asked questions

**Which index should a fluctuation clause name?**
Whichever one matches what the clause is meant to compensate. If it compensates the contractor for resource price movement, an input cost index is the honest choice. Naming a tender price index in a fluctuation clause pays the contractor for market conditions as well as for costs, which is rarely what either party thought they agreed.

**Should escalation sit in the base estimate or in contingency?**
In the base estimate, as its own line, with the rate and the mid-point date shown. Contingency covers identified risk that may not occur; escalation between the price base date and the spend date will occur. Blending them hides which one is being drawn down, and that is the first question a sponsor asks.

**How often should the escalation assumption be revisited?**
At every re-baseline and at least annually, with the previous assumption kept visible beside the new one. The movement itself is the useful number, because a rate that never changes is not stable, it is unmaintained.

**Does deflation mean a negative escalation allowance?**
Arithmetically yes, and practically almost never. Falling tender prices usually reflect margin compression rather than falling resource cost, and a contractor bidding at a loss is a different risk arriving under a different name. Hold the allowance at zero and record why.

**Can an AI model set the rate?**
It can assemble the series, align base dates, and test whether your basket weights match your actual spend, which is tedious and easy to get wrong by hand. It should not choose the rate on its own. If you cannot state the basket, the base date and the source in one sentence, the number is not defensible in front of an auditor.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so nothing here is a copy of a page on the PCI site.*

*Internal links: this piece should link to [cost control in construction](https://pciglobal.ai/cost-control-in-construction) with the anchor "how escalation sits inside a cost control system", to [IFRS 15 for construction](https://projectcontrolsinstitute.org/ifrs-15-for-construction) with the anchor "how total expected costs drive revenue", and to [project budgeting and forecasting](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) with the anchor "where the escalation line belongs in the budget".*
