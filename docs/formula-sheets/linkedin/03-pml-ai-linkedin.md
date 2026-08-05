# Project Management Formulas

> The 16 that separate a plan from a guess

## Knowing the formula is the easy half

**PML-AI · HOW TO READ THIS**

Most project managers can compute a critical path and a CPI.

Far fewer can say when the number stops meaning what they think it means — which is where projects are actually lost.

This deck gives the formula, and the mistake it attracts.

> **One rule throughout.** Under hybrid delivery, velocity forecasting and earned value answer the same question through different instruments. State which one governs the commitment before you report either.

## Three points, weighted

**ESTIMATING · DOMAIN 6**

`tE = (O + 4M + P) ÷ 6`

`σ = (P − O) ÷ 6`

PERT weights the most likely case four times.

The triangular alternative, `(O + M + P) ÷ 3`, does not — and returns a different duration from identical inputs.

> **Watch for:** three points supplied by one estimator. That is one number wearing three hats.

## Float, and a free integrity check

**CRITICAL PATH · DOMAIN 6**

`EF = ES + Duration`

`LS = LF − Duration`

`TF = LS − ES = LF − EF`

`FF = min(successor ES) − EF`

> **Total float from the starts must equal total float from the finishes.** If the two forms disagree, your forward or backward pass is wrong. It costs nothing to check and catches a whole class of error.

## Two ways to compress. Different risks.

**SCHEDULE COMPRESSION**

`Crash cost slope = (Crash cost − Normal cost) ÷ (Normal − Crash duration)`

| | Mechanism | What it costs you |
|---|---|---|
| **Crashing** | Add resource to critical activities | Direct cost, coordination load |
| **Fast-tracking** | Overlap what was sequential | Rework risk rises materially |

> **Crashing an activity that is not on the critical path** spends money and saves nothing. It happens more often than anyone admits.

## Where you are, in cost terms

**EARNED VALUE · DOMAIN 7**

`CPI = EV ÷ AC`

`SPI = EV ÷ PV`

`Cost to date = Actuals + Accruals`

> **That third line breaks the first two.** If `AC` is invoiced cost only, work has been performed that nobody has billed you for. `CPI` overstates performance and `EAC` understates cost — and both errors are invisible in the report.

## Three forecasts. All defensible.

**FORECASTING**

`EAC = AC + (BAC − EV)` — the variance was a one-off

`EAC = BAC ÷ CPI` — performance persists

`EAC = AC + (BAC − EV) ÷ (CPI × SPI)` — cost and schedule both persist

> **The arithmetic is trivial. The assumption is the judgement.** State which one you have chosen, publish a range rather than a point, and be ready to defend the choice to a board.

## The check that exposes wishful budgets

**TCPI**

`TCPI = (BAC − EV) ÷ (BAC − AC)`

The cost efficiency required on every remaining pound to still land on budget.

Now compare it to the `CPI` you have actually achieved.

> **If TCPI sits far above the CPI achieved**, the budget is asserting an efficiency improvement on work that is already running behind. Nobody has demonstrated it. It is a wish with a decimal place.

## Ordinal ratings do not multiply

**RISK · DOMAIN 8**

`Risk score = Probability rating × Impact rating`

A 3 × 4 = 12 heat-map score is a **sorting device**. Nothing more.

It cannot be summed across risks. It cannot size a contingency.

`EMV = Probability × Impact`

`Contingency ≈ Σ EMV, or P80 − P50 from simulation`

> **For anything that touches money, use expected value or simulation.** And note that `EMV` describes a portfolio — applied to one binary event, it returns a number that will never occur.

## The law that governs every queue

**FLOW · DOMAIN 13**

`Cycle time = WIP ÷ Throughput`

Halve the work in progress and cycle time halves — at exactly the same throughput.

> **This is why limiting WIP speeds delivery** without anybody working faster. It holds only for a stable system over the window you measured.

## Forecasting an adaptive delivery

**VELOCITY**

`Velocity = Points completed ÷ Sprint`

`Sprints remaining = Points remaining ÷ Average velocity`

`Capacity = Members × Available days × Focus factor`

> **Two failures here.** Using best-ever velocity rather than a rolling average of recent comparable sprints. And setting the focus factor to 1.0, which assumes nobody attends a meeting, answers a question or takes a day off.

## Burn-down hides the thing you need to see

**REPORTING TO A GATE**

A **burn-down** plots work remaining. Scope added mid-flight looks identical to slow progress.

A **burn-up** plots completed work and total scope as two separate lines.

> **Scope growth becomes visible as scope growth.** For any project reporting to a phase gate, use burn-up. The second line is the whole point.

## The point where the incentive stops working

**CONTRACTS · DOMAIN 10**

`PTA = [(Ceiling price − Target price) ÷ Buyer share ratio] + Target cost`

Above the point of total assumption, the seller absorbs **every further pound**. The buyer's contribution has hit the ceiling.

> **Check it in one line.** At an actual cost exactly equal to the PTA, the computed final price must equal the ceiling price. If it does not, you have used the seller's share ratio in the denominator instead of the buyer's.

## Why small teams move faster

**COMMUNICATION · DOMAIN 11**

`Channels = n(n − 1) ÷ 2`

Adding one person to a team of `n` creates `n` new channels, not one.

> **The growth is quadratic, and that is the entire point.** It is the arithmetic behind why adding people to a late project so rarely accelerates it.

## In control and incapable are different states

**QUALITY · DOMAIN 9**

`Control limits = Mean ± 3σ`

`Process capability = (USL − LSL) ÷ 6σ`

**Control limits** describe what your process *does*.

**Specification limits** describe what the customer *requires*.

> **A process can be perfectly in control and entirely incapable.** The response to each is different, and confusing them sends improvement effort in the wrong direction.

## The ten that cost the most

**MOST MISAPPLIED · 1–5**

1. Risk scores multiplied and then summed
2. `SV` read as a schedule measure late in a project
3. `PTA` computed with the seller's share ratio
4. Velocity taken as best-ever rather than rolling average
5. Burn-down used for gate reporting

## And the other five

**MOST MISAPPLIED · 6–10**

6. Standard deviations summed along a path — variances add, not deviations
7. Crashing an activity that is not on the critical path
8. Control limits confused with specification limits
9. Resource utilisation targeted at 100%, leaving nothing to absorb variability
10. Benefits claimed with no pre-delivery baseline ever captured

> **Without a baseline, a realisation figure cannot be computed.** It can only be asserted.

## The full sheet is free

**PCI AI · PROJECT CONTROLS INSTITUTE GLOBAL**

This deck is 16 of them. The complete reference carries the whole quantitative surface of project delivery — business case and benefits, estimating, critical path, earned value, risk, agile and flow, procurement and contracts, quality, stakeholders and portfolio — each with the mistake it attracts and reproducible worked examples.

No registration. No email gate.

**projectcontrolsinstitute.org**
