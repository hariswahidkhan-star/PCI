---
platform:      LinkedIn Article
type:          guide
title:         How to read the cost probability distribution curve
meta:          What a cost probability distribution shows, how to read a P-value by hand, where contingency comes from, and why the mean sits above the likely cost.
primary_kw:    cost probability distribution
secondary_kw:  P80 contingency, Monte Carlo cost simulation, correlation, provision recognition
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,691
hashtags:      #RiskManagement #CostEngineering #ProjectFinance #ProjectControls
ab_id:         AB-00225
---

# How to read the cost probability distribution curve

A cost probability distribution is the ranked output of a Monte Carlo cost model: every simulated total, ordered from cheapest to dearest. Read it as a cumulative curve. The P80 is the figure that 80 per cent of iterations came in at or below, and contingency is the distance from your base estimate to whichever confidence level policy sets.

Written for LinkedIn as an original. It sits under the Institute's risk management pillar.

## What is a cost probability distribution actually plotting?

Two charts come out of the same run and people routinely confuse them. The histogram plots how many iterations landed in each cost band, so it shows shape. The S-curve plots the cumulative share of iterations at or below each cost, so it answers questions.

The S-curve is the one to take to a board. Every point on it is a sentence: at this number, this proportion of the simulated outcomes were no worse.

Neither chart is a forecast of what the project will cost. Both are a statement about the model you built, which is a different and more modest claim than the way these charts usually get presented.

## How do you read a P-value by hand?

Rank the iterations and count. The simulation software does nothing more mysterious than this, and doing it once by hand removes most of the mystique. Where the iterations come from is the separate job, and [how a Monte Carlo cost model is built, step by step](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) covers that side of it.

Take ten iteration results, sorted, in millions of pounds: **48.6, 49.9, 50.8, 51.7, 52.3, 53.0, 54.1, 55.6, 57.9, 61.2**.

The **P80** is the value at rank 0.8 × 10 = 8, so **£55.6m**. The **P50** sits between the fifth and sixth values, (52.3 + 53.0) ÷ 2 = **£52.65m**.

The **mean** is the sum, 535.1, divided by ten, which is **£53.51m**.

Notice that the mean is higher than the median. With ten values you can see why in one line: the top two results, 57.9 and 61.2, sit much further from the middle than the bottom two do.

## Where does contingency come from?

Contingency is the difference between the confidence level your policy sets and the base estimate. It is a subtraction, and every argument about "how much contingency" is really an argument about which row of this table your organisation lives on.

The table below comes from a fuller model on a £48.0m base, not from the ten hand-ranked iterations above, so its P80 is a different number. Two models, two answers, and neither is wrong.

| Read at | Simulated total cost | Contingency over a £48.0m base | As a percentage of base |
|---|---:|---:|---:|
| P10 | £49.2m | £1.2m | 2.5% |
| P50 | £52.4m | £4.4m | 9.2% |
| Mean | £53.1m | £5.1m | 10.6% |
| P80 | £56.9m | £8.9m | 18.5% |
| P90 | £59.3m | £11.3m | 23.5% |

Check the arithmetic on the row that matters: 56.9 − 48.0 = 8.9, and 8.9 ÷ 48.0 = 18.5%.

Many organisations fund projects at P50 and hold the difference between P50 and P80 centrally as portfolio reserve. That structure has a reason behind it: individual projects at P50 will overrun about half the time, but a portfolio of them is far less likely to overrun in aggregate than any single project is.

State which level you funded, in the document, every time. A number reported without its confidence level is not a forecast, and it is the single most common defect in cost reporting.

## Why is the mean above the most likely cost?

Because cost risk is one-sided in practice. The most likely outcome sits at the peak of the histogram, but the tail to the right is longer than the tail to the left, and the mean is pulled towards the long tail.

Things that can go wrong have far more room to run than things that can go right. A foundation can find unforeseen ground conditions and cost three times the estimate; it cannot cost minus twice the estimate.

This is why a deterministic estimate built from most-likely values, added up, is systematically optimistic. The sum of the modes is not the mode of the sum, and on a large estimate the difference is not small.

## What does the shape of the curve tell you?

Steepness is confidence. A steep S-curve says the model thinks the range is narrow, and a flat one says the opposite.

The most common reason a curve is too steep is that everything was modelled as independent. Correlation is the assumption that decides your spread, and it is usually set by whoever built the model without discussion.

Take three cost items, each with a base of **£10m** and a P80 of **£12m**, modelled as normal. The standard deviation implied is 2 ÷ 0.8416 = **£2.376m**, using the standard normal value at the 80th percentile.

Independent, the combined standard deviation is 2.376 × √3 = **£4.115m**, so the total P80 is 30 + 0.8416 × 4.115 = **£33.5m**.

Perfectly correlated, the P80s simply add: **£36.0m**.

That **£2.5m** difference is a modelling choice, not a fact about the project. If labour rates, productivity and market conditions drive several packages together, independence is the wrong assumption and the curve you are showing is narrower than reality.

## What can the curve not tell you?

It cannot tell you about risks nobody put in the register, and those are usually the expensive ones. A model is a summary of the workshop that fed it.

It cannot correct optimism in the base estimate. If the base is 15 per cent light, every percentile is 15 per cent light, and a confident-looking P80 on a poor base is worse than no curve at all because it manufactures assurance.

It cannot decide the confidence level for you. P80 is a convention, not a law, and the right level depends on who absorbs the overrun and what else is in the portfolio.

## How does the curve meet the accounts?

This is where the risk model stops being a delivery artefact and becomes a finance one, and where most organisations have never had the conversation.

An amount held as contingency in a cost forecast is not automatically a provision in the financial statements. Described in PCI's own words rather than reproduced, a provision is recognised where a present obligation exists as a result of a past event, an outflow of resources is probable, and the amount can be estimated reliably. A general allowance for future risk on work not yet performed does not meet that test.

There is a second connection, and it is sharper. Where revenue is recognised over time using an input measure such as cost incurred against total forecast cost, the total forecast cost is the denominator of your percentage complete. Move contingency into or out of the estimate at completion and you move reported revenue and margin in the same movement.

So the question "do we include contingency in the EAC?" is not a technical preference. It changes the accounts, and it needs a stated, consistent policy that finance and delivery both signed.

An accountant is examined on when a provision may be recognised and almost never on a P80. An engineer is examined on the P80 and almost never on the provision. The PCI AI Project Finance Leader (PFL-AI) credential, at 16 domains and 61 knowledge areas, is built for the overlap between the two.

## How should contingency be drawn down?

Against realised risks, with the drawdown recorded and the remaining exposure re-run. Contingency spent on scope growth is not contingency, it is an unapproved budget increase wearing a different label.

Plot the burn-down against remaining exposure rather than against time. Sixty per cent of the contingency consumed at thirty per cent complete is a clear signal, and it is only visible if someone maintains both lines.

Re-run the model at defined gates rather than continuously. The value of the exercise is the conversation about assumptions, and running it monthly turns it into a report nobody reads.

## Frequently asked questions

**Is P80 the right confidence level to fund at?**
It is a common convention rather than a rule. Funding at P50 with reserve held at portfolio level is equally defensible and often better capital discipline, because it stops each project holding its own buffer. What matters is that the level is stated, applied consistently, and understood by whoever bears the overrun.

**Why does my P80 barely differ from my base estimate?**
Usually because the ranges are too tight, the correlations are set to zero, or discrete risk events were never modelled. Ask what the widest item in the model is and whether anyone challenged the person who ranged it. A distribution that closely tracks the deterministic estimate is generally reporting the estimator's confidence rather than the project's risk.

**Should contingency be held inside the estimate at completion?**
That is a policy decision with an accounting consequence, so it must be made once and applied consistently. Where revenue is recognised on cost incurred against total forecast cost, including or excluding contingency changes the percentage complete and therefore reported revenue. Agree the treatment with finance and document it in the basis of estimate.

**How many iterations does a cost model need?**
Enough that the percentiles you report stop moving between runs. Check by running the same model several times and watching the P80. If it shifts materially, add iterations. Most cost models stabilise well before the iteration count becomes a computing problem, and stability is a better test than any fixed number.

**Does a risk model replace judgement?**
No, it structures it. Every input is a judgement, from the ranges to the correlations to which risks made the register at all. The output deserves the same scepticism as the inputs, and a model whose assumptions cannot be explained in a meeting should not be driving a funding decision.

---

*PCI publishes certification requirements. Nothing here is legal, tax or accounting advice. All figures above are illustrative arithmetic, not project data.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Linking note: one cross-estate link now sits in the body, in the section on reading a P-value by hand. That passage explains what to do with the iterations but not where they come from, which is precisely the question the hub's Monte Carlo cost simulation guide answers. The note originally proposed two further hub links, to quantitative schedule risk analysis and to project budgeting and forecasting. Both were dropped because a piece may carry only one link to any given domain, and the Monte Carlo guide is the closest match to what this article leaves unexplained. Nothing in the piece raises a question the AI, careers, regional or comparison domains answer, so no second cross-estate link was invented.*
