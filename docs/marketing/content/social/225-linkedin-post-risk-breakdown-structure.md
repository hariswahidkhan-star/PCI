---
platform:      LinkedIn post
type:          linkedin-post
title:         The risk breakdown structure teams skip, then need
meta:          Sixty-one of 96 rows under one category tells you who was in the workshop. And the correlation the RBS catches, which doubled a P90 from £1.5m to £3m.
primary_kw:    risk breakdown structure
secondary_kw:  RBS, risk categories, correlation in Monte Carlo, risk identification
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    389
hashtags:      #RiskManagement #ProjectControls #ProjectManagement #PMO
ab_id:         AB-01039
---

# The risk breakdown structure teams skip, then need

**Post body (1,861 characters):**

Map a 96-row risk register onto a risk breakdown structure and the shape of it will tell you something the register never says on its own.

A risk breakdown structure is a hierarchical decomposition of risk sources, the way a work breakdown structure decomposes scope. Level 1 is categories. Level 2 is sources within them. Level 3 is where the actual rows hang.

Here is the mapping from a real-shaped register:

Technical: 61 rows
Delivery: 22
Commercial: 9
External: 4
Organisational: 0

Sixty-four per cent of the register sits under one category, and one category has nothing in it at all. That is not a picture of the project. It is a picture of who was in the workshop.

The engineers turned up. Commercial and finance did not, and the person who could have named the organisational risks was on leave.

You cannot see that from a list sorted by score. You can see it in ten seconds from the structure.

The second use is the one that changes numbers. Rows sharing an RBS parent are usually correlated, and correlation is what the tail of a simulation is made of.

Six risks, each 30% likely, each £500k. Model them independently:

Mean 6 × 0.3 × £500k = £900k
P(two or fewer occur) = 74%
P(three or fewer) = 93%
So the P90 lands at three hits, £1.5m

Now assume they all share one parent — say, availability of a single trade in one labour market. They move together. Seventy per cent of the time nothing happens. Thirty per cent of the time all six do.

Mean, unchanged: £900k
P90: £3.0m

Same expected value. Double the tail. If contingency is being set at P80 or P90, that assumption is the whole answer, and it was never discussed because the register was a flat list.

Build the RBS before the identification workshop, not after. Used first it drives the questions and shows you the empty branches. Used afterwards it only tells you what you missed.

#RiskManagement #ProjectControls #ProjectManagement #PMO

**First comment:** How the same structure feeds a schedule risk model, and where correlation gets set: https://projectcontrolsinstitute.org/schedule-risk-analysis

---

*Every figure above is illustrative arithmetic, not project data. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis) with the anchor "where correlation between risks gets modelled", and [a risk register that gets used](https://projectcontrolsinstitute.org/risk-register-that-gets-used) with the anchor "the register the RBS organises".*
