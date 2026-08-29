---
platform:      LinkedIn post
type:          linkedin-post
title:         Contingency by expected value: AACE 44R-08 in practice
meta:          Five risks sum to £1.88m of expected value. Fund that and you are covered in 55% of outcomes. Reaching P80 costs £2.9m. The full distribution, worked.
primary_kw:    contingency by expected value
secondary_kw:  AACE 44R-08, expected value method, P80 contingency, estimate accuracy range
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    395
hashtags:      #RiskManagement #CostEngineering #ProjectFinance #ProjectControls
ab_id:         AB-00235
---

# Contingency by expected value: AACE 44R-08 in practice

**Post body (1,891 characters):**

The expected value method for contingency is the simplest defensible one. It is also the one most often quoted at the wrong confidence level, and the arithmetic shows why in a minute.

AACE International's recommended practice 44R-08 sets out the approach. Described in plain terms: price each risk as probability times impact, sum the results, and that sum is your starting contingency.

Five risks on a job:

Ground conditions, 35% × £1.4m = £490k
Late permit, 50% × £600k = £300k
Steel escalation, 60% × £900k = £540k
Subcontractor default, 10% × £2.5m = £250k
Commissioning rework, 40% × £750k = £300k

Total expected value: £1.88m

Enumerate all 32 outcomes and their probabilities, one column of a spreadsheet:

Probability that nothing happens at all: 7.0%
P50: £1.65m
P80: £2.90m
P90: £3.65m
Maximum exposure: £6.15m
Probability the total lands at or below £1.88m: 55%

So the expected value is not a P80. It is not even a P50 here. Fund the job at £1.88m and you are short in 45 outcomes out of 100. Reaching 80% confidence costs £2.90m, which is 54% more than the sum everyone quoted.

That is the whole trap. The sum is arithmetically correct and it answers a question nobody asked.

Three conditions the method needs to hold.

Impacts modelled as ranges, not points. A single-point impact is a guess with a decimal place, and it flattens the tail the method exists to show you.

Independence, or a stated correlation assumption. Risks from the same source move together: same mean, far heavier tail.

Separation from estimate uncertainty. The accuracy range on the base estimate is a different pot from the risk events. Combine them without saying so and the number defends nothing.

Report it as a distribution with a named confidence level, and say which level the sanction figure sits at. A contingency figure without a percentile is just a round number with a story attached.

#RiskManagement #CostEngineering #ProjectFinance #ProjectControls

**First comment:** Running the same five risks through a simulation, and reading the output curve properly: https://projectcontrolsinstitute.org/monte-carlo-cost-simulation

---

*Every figure above is illustrative arithmetic, not project data. AACE International's recommended practices are named and described here in PCI's own words; no protected text or table is reproduced. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) with the anchor "simulating the same risk set", and [project budgeting and forecasting](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) with the anchor "where the sanctioned contingency sits in the budget".*
