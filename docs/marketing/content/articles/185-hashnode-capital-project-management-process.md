---
platform:      Hashnode
type:          process-guide
title:         The capital project management process, stage by stage
meta:          The capital project management process in five gated stages, with the options appraisal and the sanction estimate built up in full, line by line.
primary_kw:    capital project management process
secondary_kw:  stage gate process, front end loading, sanction estimate, capital project lifecycle
pillar:        Project controls fundamentals
credential:    PML-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/capital-project-management-process
schema:        HowTo
word_count:    1800
hashtags:      #productivity #finance #tutorial #datascience
ab_id:         AB-00208
---

# The capital project management process, stage by stage

The capital project management process moves an idea through five governed stages — identify, select, define, execute, operate — with a decision gate between each. At every gate the sponsor funds the next stage, sends the work back for definition, or stops. Project controls supplies the estimate, schedule, risk and business-case numbers the gate decides on.

Stage names vary by industry. The decision structure does not.

*This describes practice. It is not accounting, tax or legal advice.*

## The capital project management process in one table

| Stage | The question it answers | Controls deliverables | Estimate maturity | Gate decision |
|---|---|---|---|---|
| 1. Identify | Is there a problem worth capital? | Order-of-magnitude range, indicative programme, initial risk list | Screening | Fund a study, or stop |
| 2. Select | Which option should we take forward? | Option estimates, comparative schedules, options-level risk, discounted cash flow | Study | Choose one option, or stop |
| 3. Define | Is the option defined well enough to commit? | Sanction estimate, baseline programme, quantified risk analysis, execution plan | Sanction-quality | Authorise the capital, or stop |
| 4. Execute | Are we delivering what was sanctioned? | Baseline maintenance, progress measurement, cost report, forecast, change control | Control budget | Continue, re-baseline, or stop |
| 5. Operate | Did we get what we paid for? | Final cost, close-out report, benefits measurement, lessons | Actual | Close, and test the benefit case |

Money spent in stages 1 to 3 is small and buys most of the influence over the outcome. Money spent in stage 4 is large and buys almost none: the design decisions that set the cost are already taken.

## Stage 1: identify

The purpose is to establish that a need exists and that capital is a plausible answer.

Deliverables are deliberately thin: the problem stated, a cost range wide enough to be honest, an indicative duration, and the constraints that would kill it. A screening estimate is a range, never a point.

The gate test is whether the need is real and material, and whether the organisation would build it if the study came back positive. Studies commissioned without that appetite are waste.

## Stage 2: select

The purpose is to compare genuine alternatives and take one forward. It carries the most influence over eventual cost and is the most often rushed.

Two options for a plant upgrade. Option A costs £84m of capital and returns £11.0m of net benefit a year for fifteen years. Option B costs £62m and returns £8.4m a year over the same life.

Simple payback separates neither: 84 ÷ 11.0 = **7.6 years** against 62 ÷ 8.4 = **7.4 years**.

Discount the benefits at 8 per cent over fifteen years. The annuity factor is (1 − 1.08⁻¹⁵) ÷ 0.08, and since 1.08¹⁵ = 3.1722 that is (1 − 0.3152) ÷ 0.08 = **8.5595**.

Option A: 11.0 × 8.5595 = 94.15, less 84 of capital, is a net present value of **£10.15m**.
Option B: 8.4 × 8.5595 = 71.90, less 62 of capital, is **£9.90m**.

```python
def annuity_factor(rate, years):
    return (1 - (1 + rate) ** -years) / rate

af = annuity_factor(0.08, 15)                 # 8.5595
print(11.0 * af - 84, 8.4 * af - 62)          # 10.15  9.90
```

The gap is £250,000 on estimates that at this stage carry ranges of tens of millions. The appraisal does not separate the options on value, so the decision falls to risk, deliverability and optionality instead.

A discounted cash flow eliminates options that are clearly wrong. It does not rank close options, and a preference stated on a £250,000 gap between estimates that could each be £15m out is false precision.

## Stage 3: define

The purpose is to define the chosen option well enough to commit capital to it. In process industries this is front-end engineering design; in construction, a developed design and a firm procurement strategy.

Three things must be true at the end: scope fixed and documented, the estimate built from quantities and quotations rather than factors, and the schedule a resource-tested network.

AACE International publishes a recommended practice classifying estimates by how mature the project definition is, with accuracy ranges for each class. Use it to check whether an estimate is fit to sanction against, and read it in the original.

### Building the sanction estimate

A sanction estimate is not one number but a build-up, and every line should be separately defensible.

| Line | Basis | Amount |
|---|---|---:|
| Base estimate | Quantities priced from quotations and rates, at today's prices | £62.0m |
| Escalation | 8.61% applied to the £48.4m not yet under firm price | £4.2m |
| Contingency | P80 from the quantified risk analysis | £7.6m |
| **Project control budget** | Base + escalation + contingency | **£73.8m** |
| Management reserve | Held by the sponsor, outside the project | £3.0m |
| **Total authorised** | | **£76.8m** |

The escalation line is worked, not assumed. At 3.5 per cent a year over a weighted 2.4 years to the midpoint of construction, 1.035^2.4 = 1.0861, so escalation is 8.61 per cent. Seventy-eight per cent of the base estimate is not yet under firm price: 0.78 × 62.0 = £48.4m, and 48.4 × 0.0861 = **£4.2m**.

Contingency at £7.6m is 12.3 per cent of the base estimate, and that percentage is an output of [the quantified risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis), not an input. Contingency set as a round percentage first and justified afterwards is a budget decision wearing a risk analysis as cover.

Management reserve sits outside the project deliberately. Releasing it is a governance act, so the project cannot quietly absorb the P50 to P80 gap.

## Stage 4: execute

The purpose is delivery against the sanctioned position, and the controls job changes character. Before sanction it estimates what a thing might cost; after, it measures what it is costing and forecasts where it lands.

The baseline is frozen, progress is measured against rules of credit agreed in advance, and every change is trended before approval.

The monthly rhythm is fixed: update the programme, measure progress, book actual costs to the same cut-off, calculate earned value, revise the forecast, and reconcile contingency movement to the risk register.

## Stage 5: operate

The purpose is to prove the capital bought what the business case promised, and it is the stage most organisations abandon.

Three things belong here: a final cost reconciled from sanction to actual with the variance explained line by line, a benefits measurement twelve to twenty-four months after handover, and a lessons record with named owners.

Without stage 5 the estimating basis never improves, because nobody learns which assumptions failed.

## Where the capital accounting bites

This is the seam where capital projects lose money quietly, because the two professions involved were trained separately.

An owner capitalises the cost of bringing an asset to the condition and location necessary for it to operate as intended, and stops when the asset is ready for use, whether or not it has been brought into use. Costs after that date go to profit and loss, and that date is a physical fact the project team owns and finance must be told about.

A contractor building the same asset faces the opposite question, recognising revenue over time as it satisfies its obligation, with progress commonly measured by cost incurred against total expected cost — a measure the project's own forecast produces.

The same facts become a capitalisation cut-off for one party and a revenue measure for the other, and both rest on a project controls number. An engineer is examined on progress measurement and almost never on cut-off; an accountant, the reverse. The gate process either closes that gap or institutionalises it, and the work either side of it is set out in [what project controls covers](https://projectcontrolsinstitute.org/what-is-project-controls).

## What a gate actually tests

A gate is not a presentation. Four questions decide it.

Is the scope defined to the standard this stage requires, evidenced rather than asserted? Is the estimate built at the maturity the stage demands, with the basis of estimate written down? Is the schedule achievable with resources that exist? Is the risk quantified, with contingency derived from it?

A gate that reviews a slide deck rather than the basis of estimate is a ceremony. Require the basis of estimate at the gate, and read it.

The PCI Project Management Leader – AI (PML-AI) examines that lifecycle across 16 domains and 63 knowledge areas, covering sanction governance and benefits realisation alongside delivery, with the Body of Knowledge running 40 per cent finance and reporting, 40 per cent project management and 20 per cent governed AI.

## Frequently asked questions

**How many stages should a capital project have?**
Five is the common structure, and the number matters far less than the discipline of a real decision between each one. Some run three, some seven with sub-gates in the define stage. What separates a working process from a paper one is whether a gate can genuinely return a "no".

**What is front-end loading?**
It is the deliberate investment of effort and money in stages 1 to 3 before capital is committed, to define scope, estimate properly and quantify risk. The argument for it is influence: decisions taken before sanction set most of the eventual cost, while decisions after sanction mostly rearrange it. Projects that shorten definition to hit a sanction date pay for the saved weeks later.

**Who owns contingency in a capital project?**
The project holds contingency for identified risks inside the control budget; the sponsor holds management reserve outside it. Splitting them keeps the project honest, because drawing on reserve requires a governance decision and leaves a record. Where both sit inside the project, the distinction erodes within about six months.

**When should the baseline be re-set?**
When the sanctioned scope changes materially, or when the baseline is so detached from reality that variance against it no longer informs a decision. Neither test is "the project is late". Re-baselining to hide accumulated variance destroys the only trend data the organisation has.

**What does project controls do before sanction?**
Estimating, scheduling and risk quantification, plus the business-case arithmetic. It is a different job from execute-stage work: fewer actuals, more assumptions, and the deliverable is a basis of estimate somebody can challenge line by line. Most who do it well have run an execute stage first.

---

*First published on projectcontrolsinstitute.org; the republishing field in Draft Settings sets the canonical back to the original stage-by-stage guide.*

*Internal links: this guide should link to [what are capital projects](https://projectcontrolsinstitute.org/what-are-capital-projects) with that anchor, to [project budgeting and forecasting](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) with the anchor "building and maintaining the control budget", and to [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) with the anchor "quantifying the risk that sets contingency".*
