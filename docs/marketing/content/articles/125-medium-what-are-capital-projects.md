---
platform:      Medium
type:          pillar
title:         What are capital projects? A beginner's complete guide
meta:          What are capital projects: capex against opex, the stage gates, front-end loading, and the NPV and IRR arithmetic that decides whether one is funded.
primary_kw:    what are capital projects
secondary_kw:  capital expenditure, stage gate process, front-end loading, final investment decision
pillar:        Project controls fundamentals
credential:    PML-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /what-are-capital-projects (own site #009)
schema:        Article + FAQPage
word_count:    2,469
hashtags:      #ProjectControls #ProjectFinance #ProjectManagement #CostEngineering #PMO
ab_id:         AB-00207
---

# What are capital projects? A beginner's complete guide

Capital projects are large, discrete investments that create or materially extend a long-lived asset. They are funded from a capital budget rather than from operating expenditure, sanctioned through defined stage gates, and capitalised on the balance sheet instead of expensed. Size alone does not make one; creating a durable asset does.

A new terminal, a substation, a process train, a fleet replacement, a hospital wing, a data centre. Each still exists, and still earns, years after the project team has gone.

## What are capital projects, and what makes them different?

Three features separate a capital project from the rest of what an organisation does.

It consumes capital rather than operating budget. It is approved against a business case rather than against an annual plan. And it is governed by stage gates rather than by a line manager.

The consequence for anyone working on one is that the project is judged on two clocks at once: the delivery clock, measured in progress and float, and the investment clock, measured in when the asset starts earning.

## Capital or operating expenditure

The distinction is not a matter of preference. It follows from what the money buys.

| | Capital expenditure | Operating expenditure |
|---|---|---|
| What it buys | A new asset, or a material enhancement of an existing one | Consumption within the period |
| Accounting treatment | Capitalised, then depreciated over the asset's useful life | Charged to the income statement as incurred |
| Effect on profit | Spread across many years through depreciation | Immediate, in full |
| Effect on cash | Immediate and often very large | Matched to activity |
| Typical approval | Investment committee or board, against a business case | Departmental budget holder |
| Examples | Building a plant, replacing a bridge deck, installing new lines | Routine maintenance, consumables, short-campaign hire |

The boundary case is argued on nearly every project, so learn it early. Work that restores an asset to its original condition is maintenance and goes to the income statement. Work that extends its life or increases its capacity is enhancement and may be capitalised.

That decision changes reported profit without changing a single physical activity, which is why the classification is agreed and documented at the start rather than settled at year end.

## The stage-gate lifecycle

Capital projects are governed by gates because the cost of being wrong grows faster than the cost of being slow. Each gate is a decision to spend more money on definition, not a decision to build.

| Stage | Purpose | Key output | Decision at the gate |
|---|---|---|---|
| Concept framing | Establish that a business need exists | Statement of need, rough order of magnitude cost | Is this worth studying? |
| Feasibility (FEL-1) | Test whether any option works technically and commercially | Options list, screening economics | Is there a viable option? |
| Selection (FEL-2) | Choose one option and prove it | Selected concept, developed estimate, execution strategy | Which option, and do we develop it? |
| Definition (FEL-3) | Define the chosen option well enough to commit cost and schedule | Sanction estimate, control schedule, contracting plan | Final investment decision |
| Execution | Build it | The asset, plus a controlled cost and schedule record | Continue, intervene or stop |
| Commissioning and handover | Prove it works and transfer it | Test records, as-built information, operating readiness | Accept into operations |
| Close-out and benefits | Confirm the investment did what it promised | Final account, lessons, benefits review | Was the business case delivered? |

The final investment decision is the point of no meaningful return. Before it, the money spent is study cost. After it, commitments are placed and the cost of cancellation climbs steeply.

Two estimates matter around that gate. The sanction estimate supports the investment decision. The control estimate, built from the same basis, becomes the budget the project is measured against, and the two should reconcile line by line before anyone signs.

## Front-end loading: why definition maturity governs the outcome

Front-end loading is the deliberate investment of time and money in defining a project before committing to build it. It is the largest single lever available to a capital project, and it is available only before sanction.

The logic is arithmetic rather than philosophical. Design changes are cheap while they are drawings, expensive once they are purchase orders, and punitive once they are steel in the air.

A project sanctioned on thin definition does not become better defined by being started. It moves its unresolved questions into execution, where they arrive as variations, standing time and rework.

The practical test at a gate is not how many documents exist. It is whether the scope is stable enough that the estimate basis will still be true in six months. Where a discipline lead cannot say what they are building, the estimate for it is a placeholder.

## How a capital project gets funded: the arithmetic

Investment committees compare projects against each other and against doing nothing. Three measures do most of that work, and anyone in project controls should be able to derive all three.

Take a project costing **£50m**, spent at the start, generating **£9m** of net cash each year for ten years, against a required return of **8%**.

**Net present value.** The present value of an even ten-year stream at 8% uses an annuity factor of (1 − 1.08⁻¹⁰) / 0.08 = **6.710**. Multiply: £9m × 6.710 = **£60.39m**. Subtract the capital: 60.39 − 50 = **NPV of £10.39m**.

A positive NPV means the project earns more than the 8% the money is expected to earn elsewhere. That is the test it has to pass.

**Payback.** Simple payback is 50 ÷ 9 = **5.6 years**. It ignores the time value of money completely, which is why it is a screening measure and never a decision measure.

**Internal rate of return.** The IRR is the discount rate at which NPV is zero. At 12% the annuity factor is 5.650 and the NPV is +£0.85m. At 13% the factor is 5.426 and the NPV is −£1.16m. Interpolating between them gives an IRR a little over **12.4%**.

The project clears its 8% hurdle by roughly **4.4 percentage points**. That margin is what the delivery team is being asked to protect, and most delivery teams are never told the figure.

## What a twelve-month delay actually costs

Delay is usually reported as a schedule fact. It is a financial one, and the size surprises people.

Push every cash inflow in that example back by one year, with no change at all to capital cost. The present value of the inflows becomes 60.39 ÷ 1.08 = **£55.92m**, so the NPV falls to **£5.92m**.

A twelve-month delay has removed **£4.47m of value — 43% of the entire business case** — without a single pound of cost overrun.

That is the number to put in front of anyone treating an eight-week float loss as a scheduling matter. Time on a capital project is the business case, and float is being spent whether or not anybody is tracking it.

It also explains why acceleration is often rational even when it looks expensive. Spending £2m to recover six months on this project protects more value than it consumes.

## The control system a capital project needs

A capital project is a temporary organisation spending permanent money, so its control system has to be built rather than assumed.

**A work breakdown structure** that decomposes scope to the level at which somebody can be held responsible, and **a cost breakdown structure** that classifies spend so a labour problem is visible as a labour problem.

**A schedule** built on real logic, with a critical path that survives a health check and float that is understood rather than absorbed. **Progress measurement** on rules of credit rather than opinion.

**Change control** that captures scope movement before it is priced, because the cheapest change to reject is the one that has not yet been designed. **Contingency drawdown tracking**, so the rate of consumption is compared against the rate of progress.

**A risk process** that runs quantitative analysis at gates and qualitative review monthly. And a **forecast** produced every period with a written basis, using more than one method.

## Capitalisation: when spend becomes an asset

At some point the money spent stops being project cost and becomes an asset on the balance sheet. That transition is governed by the accounting standards, and [how those reporting standards land on a project's cost report](https://projectcontrolsinstitute.org/ifrs-for-project-controls) decides what evidence project controls has to supply.

Directly attributable costs of bringing the asset to the location and condition needed for its intended use may be capitalised. Site preparation, installation, testing and attributable professional fees qualify; general administration, training and the cost of an idle period do not.

Borrowing costs directly attributable to constructing a qualifying asset are capitalised while it is being readied, and capitalisation stops when the asset is substantially complete. Interest during an extended suspension is not capitalised.

Depreciation begins when the asset is available for use, not when the ribbon is cut. A three-month commissioning delay therefore moves a depreciation charge into a different financial year, which is why the finance team asks about commissioning dates far earlier than the delivery team expects.

The practical implication is that the cost report must be codeable to that boundary from day one. Splitting three years of accumulated cost into capital and revenue at handover is a painful exercise with a predictable outcome.

## Who does what

| Role | Owns | Where it goes wrong |
|---|---|---|
| Sponsor | The business case and the money | Stays at the gate and disappears during execution |
| Project director | Delivery of the sanctioned scope | Inherits a sanction decision made in three months and manages it for three years |
| Project controls | Budget, schedule, progress, forecast, change, risk | Reports through delivery only, so the forecast drifts optimistic |
| Estimating | The basis of cost | Basis never written down, so the estimate cannot be tested |
| Procurement | The commitment strategy | Commitments placed before the design is stable |
| Operations readiness | Whether anyone can run the thing on handover day | Under-resourced until the last six months |

Operations readiness is the role most often starved. An asset that is mechanically complete and operationally unready has not started earning, and the business case only counts cash.

## Where capital projects go wrong

They go wrong at sanction more often than during execution. A project sanctioned on an immature estimate with a compressed schedule has already decided its outcome, and the delivery team spends three years managing a decision taken in three months.

They go wrong when contingency is treated as a target rather than a provision. Contingency consumed at twice the rate of progress in the first year is a forecast, and usually one nobody reads until month eighteen.

They go wrong when the schedule and the cost report describe different projects. If the forecast completion date moved and the estimate at completion did not, one of the two is not being maintained.

And they go wrong quietly, through optimism in the forecast, until the correction arrives in one large step. Small honest movements are always cheaper than one large one, commercially and reputationally.

## Where this sits in the PCI curriculum

The PCI Project Management Leader – AI (PML-AI) covers **16 domains across 63 knowledge areas**, built around delivery of exactly this kind of work: gated investment, contracted execution, and a handover that has to satisfy operations and finance at the same time.

The curriculum carries both sides because a capital project is simultaneously an engineering object and a balance sheet object. The people who can only see one of the two are the reason the two versions of the truth diverge.

Across the three volumes the material is grounded in **92 sector case studies** (26 + 33 + 33). The calculation content behind the PCI AI Project Finance Leader (PFL-AI) and PML-AI is verified by **15,613 machine calculation checks, all passing**; the PCI AI Project Controls Leader (PCL-AI) has no equivalent suite.

## Frequently asked questions

**What counts as a capital project rather than just a big job?**
The test is what the spend creates, not how much it is. If the money produces or materially enhances an asset with a useful life beyond the current period, and it is approved against a business case from a capital budget, it is a capital project. Large recurring maintenance campaigns can cost more and still be operating expenditure.

**What is the difference between a capital project and a capital programme?**
A project delivers one defined asset or outcome with its own business case. A programme coordinates a set of related projects towards a shared benefit, such as a network upgrade delivered over several years. Programmes manage interdependency and benefit realisation; projects manage scope, cost and schedule.

**What is front-end loading and how does it relate to stage gates?**
Front-end loading is the work done before sanction to define a project properly, usually described in three phases: testing feasibility, selecting an option, then defining the selected option in enough detail to commit. Stage gates are the decision points between those phases, where a sponsor decides whether to fund the next stage of definition.

**Who approves a capital project?**
Typically an investment committee or a board, on the recommendation of a sponsor, at the final investment decision gate. Approval authority is normally tiered by value, so smaller capital projects are sanctioned within a business unit and larger ones escalate. The approving body owns the business case afterwards, not only at the gate.

**When does capitalisation start and stop?**
It starts when the project meets the criteria for recognising an asset and costs become directly attributable to bringing it into use. It stops when the asset is substantially ready for its intended use, which is generally before final account settlement. Costs incurred after that point, including most rectification and training, go to the income statement.

**Do capital projects always use earned value management?**
Not always, but the larger and more regulated they are, the more likely it is required. Even where a full earned value system is not mandated, the underlying discipline — a time-phased baseline, rules of credit and objective progress measurement — is what makes a capital project forecast defensible under challenge.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so this republish is here for readers rather than for link equity.*

*Internal links: one is now placed in the body. The reporting standards guide (projectcontrolsinstitute.org) sits on "how those reporting standards land on a project's cost report", in the capitalisation section, which tells the reader the transition is governed by the standards and then does not say which evidence that obliges the cost report to carry. The note also proposed the capital project management process and the project controls pillar; both are dropped from this republish, because three links to a single domain from one article is a link-scheme pattern, and both are the own-site original's internal links. Reciprocal: the capital project management process page should link back here with the anchor "what a twelve-month delay does to the business case", since the NPV arithmetic in this piece is the reason its gates exist.*
