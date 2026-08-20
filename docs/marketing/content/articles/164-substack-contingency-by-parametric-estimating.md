---
platform:      Substack
type:          guide
title:         Contingency by parametric estimating: what 42R-08 does
meta:          Contingency by parametric estimating, as AACE 42R-08 frames it: systemic risk from a model, event risk from the register, and why adding both double-counts.
primary_kw:    contingency by parametric estimating
secondary_kw:  systemic risk, AACE 42R-08, scope definition maturity, expected value of a risk register
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,767
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00234
---

# Contingency by parametric estimating: what 42R-08 does

Contingency by parametric estimating sets the number from a model relating cost growth to a few systemic drivers, chiefly how well the scope was defined when the money was committed. AACE International's recommended practice 42R-08 is the standard reference for the method. The risk register still has a job, and it is a different one.

*Written first for this newsletter. The model below is invented to show the mechanics; the recommended practice is described in my own words and none of its content is reproduced here. Calibrate on your own completed projects or do not use a model at all.*

## What contingency by parametric estimating actually does

42R-08 describes determining contingency by regression: build a model from your own completed projects that relates outturn cost growth to a small number of measurable project characteristics, then read the contingency for a new project off the model.

The characteristics that matter are systemic. How complete was the scope definition at sanction, how novel is the process technology, how complex is the execution, how experienced is the team, and how good is the estimate itself.

The appeal is that these are the drivers a risk workshop is least able to see. Nobody writes "we sanctioned at 40% definition" on a risk register, and that is usually the largest single cause of the growth that follows.

The requirement is data. A parametric model built on someone else's projects is a borrowed opinion with an equation around it.

## Systemic risk against event risk

| | Systemic risk | Project-specific event risk |
|---|---|---|
| What it is | Cost growth caused by how the project is set up and defined | Identified events that may or may not occur |
| Where it is visible | Only in the pattern across completed projects | In the risk register, one line each |
| How it is quantified | Parametric model, or a stated allowance by class | Probability times impact, or a simulation |
| Typical size at sanction | The larger share on a poorly defined project | The smaller share, and better understood |
| Fails when | The new project is unlike the calibration set | The drivers are systemic and nobody named them |

Both are real, and they overlap less than people fear. The double-counting risk comes from running a parametric model and then adding the full register on top, without checking which events the model has already absorbed.

## A worked parametric model

An illustrative model of the kind you would calibrate yourself, expressing contingency as a percentage of the base estimate:

**C% = 3.5 + 0.60·D + 2.4·T + 1.6·X**

D is a definition gap score from 0 to 20, where 20 means no scope definition. T is technology novelty from 0 to 3, and X is execution complexity from 0 to 4.

The intercept is the residual growth seen on the best-defined projects in the calibration set.

A brownfield process unit at the start of FEED: D = 11, T = 1, X = 2.

C% = 3.5 + (0.60 × 11) + (2.4 × 1) + (1.6 × 2) = 3.5 + 6.6 + 2.4 + 3.2 = **15.7%**.

Base estimate £86.0m, so contingency = 86.0 × 0.157 = **£13.50m**, and the sanction total is **£99.50m**.

Each coefficient is a claim about your portfolio that can be tested. That is the property the method has and a percentage rule of thumb does not.

## What better definition is worth

Run the same project again at the end of FEED, with the definition gap closed from 11 to 5 and nothing else changed.

C% = 3.5 + (0.60 × 5) + 2.4 + 3.2 = **12.1%**, so contingency = 86.0 × 0.121 = **£10.41m**.

The difference is **£3.09m** of contingency released by definition work alone, on one unit, before a single risk has been mitigated.

Now the other direction. Take the same brownfield unit with genuinely novel technology, T = 3 instead of 1: C% = 20.5%, contingency £17.63m. Novelty costs £4.13m more than proven process on this model, which is the sort of figure a technology selection paper should carry and rarely does.

Those two comparisons are the argument for a model. They convert an argument about optimism into an argument about inputs.

## The register on the same project

Six identified events, probability times impact, all figures illustrative:

| Event | Probability | Impact £m | Expected value £m |
|---|---:|---:|---:|
| Ground conditions worse than survey | 0.35 | 4.2 | 1.47 |
| Long-lead valve delay | 0.25 | 1.8 | 0.45 |
| Tie-in window missed | 0.20 | 3.6 | 0.72 |
| Permit condition change | 0.15 | 2.4 | 0.36 |
| Currency movement on imported skid | 0.40 | 0.9 | 0.36 |
| Commissioning rework | 0.30 | 1.5 | 0.45 |
| **Total** | | | **3.81** |

Expected value of the register is £3.81m against a parametric figure of £13.50m. The gap of **£9.69m** is not an error in either method.

The register prices what the team could name in a workshop. The model prices what has happened to projects sanctioned at this level of definition, including the scope nobody had thought of yet, which is by definition absent from the register.

Take the higher figure and use the register for mitigation planning, not for sizing the pot. Adding £3.81m to £13.50m assumes the model saw none of these events, and a model calibrated on completed projects saw most of them.

## Four ways to set contingency

| Method | What it prices | What it assumes | Where it fails |
|---|---|---|---|
| Percentage rule of thumb | Nothing specific | This project resembles the average project | It is a habit, not an estimate, and it is defended as if it were data |
| Expected value of the register | Named events | The workshop found the material risks | Silent on undefined scope, and the mean is not a funding level |
| Parametric model | Systemic drivers | The new project resembles the calibration set | Needs a real data set, and transfers badly between portfolios |
| Integrated cost and schedule simulation | Uncertainty and events together | The distributions and correlations are honest | Expensive, and precision in the output flatters weak inputs |

Most organisations should run two of these and explain the difference. A parametric figure alongside a register expected value gives a range and a reason, and the reason is the part a sanction committee actually needs.

Be careful about percentiles. A parametric model calibrated to average cost growth returns an expected value, not a P80. If a funding level at a stated confidence is required, that comes from a distribution, which means [sampling the ranged estimate to read a P80 off the result](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation). The model gives you a central estimate to put a distribution around rather than a substitute for one.

## Where the contingency lands in the accounts

Contingency is not a provision. Under IAS 37 a provision requires a present obligation from a past event, and money held against risks that have not occurred does not meet that test, so contingency generally sits in the cost forecast rather than the balance sheet.

That matters for reporting. When contingency is drawn down, the estimate at completion moves, and where progress is measured by a cost-based input method the total expected cost is the denominator of percentage complete. Drawing contingency changes recognised revenue without anyone touching the ledger.

This is the overlap PCI examines. A chartered accountant is examined on when a provision may be recognised and what it must satisfy, and almost never on how a contingency was sized. An engineer is examined on quantities, float and progress, and almost never on cut-off or the contract asset.

The money goes missing between them, and contingency drawdown is one of the most common places it happens.

Nothing here is accounting advice. It is a description of why the cost engineer and the reporting accountant need to be reading the same forecast on the same day.

The PCI AI Project Finance Leader (PFL-AI) syllabus covers 16 domains and 61 knowledge areas, with risk quantification and financial reporting examined together rather than in separate papers.

## Frequently asked questions

**How many completed projects do I need before a model is credible?**
Enough that each driver has variation across the set: projects sanctioned at different definition levels, with different technology novelty, of different complexity. A set of twenty projects that were all sanctioned at the same maturity cannot tell you what maturity is worth, however large it is. Variation matters more than count, and a small well-spread set beats a large uniform one.

**Can you use a published model instead of calibrating your own?**
As a sense check, yes. As the basis of a sanction figure, only if your projects genuinely resemble the ones the model was built from in sector, contracting model and scale. The safer use is to run a published model beside your own experience and treat a large divergence as a question to answer rather than a number to adopt.

**Does parametric contingency replace the risk register?**
No. The register drives mitigation, ownership and early warning, which is most of its value and none of it is arithmetic. What the register should stop doing is setting the size of the contingency pot on its own, because the expected value of named events systematically understates growth on poorly defined projects.

**How should contingency be released?**
Against defined criteria and with a named approver, tracked as drawdown against a forecast rather than as a fund that quietly empties. Show the remaining contingency and the remaining risk exposure side by side each month. If the pot falls faster than the exposure, the project has been consuming its risk allowance to pay for scope, and that is a conversation to have at 30% complete rather than at 80%.

**Should management reserve sit inside this number?**
No, and mixing them is a common reporting failure. Contingency covers identified and systemic risk within the approved scope and is normally the project manager's to draw. Management reserve sits above the project for scope changes and strategic events, and is the sponsor's. Reporting them as one line hides which one is being spent.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so this piece is not a copy of anything on the PCI site.*

*Linking note — the links now in the body: "sampling the ranged estimate to read a P80 off the result" points at projectcontrolsinstitute.org/monte-carlo-cost-simulation from the paragraph on percentiles, because the sentence saying a parametric model returns an expected value rather than a P80 raises where a funding level at a stated confidence actually comes from. One cross-estate link only — the register, the drawdown rules and the IAS 37 passage are complete in themselves, and two links to the same domain would be a footprint rather than a reference. Reciprocal: the Monte Carlo how-to could point back here for the systemic-risk half that a ranged estimate does not price.*
