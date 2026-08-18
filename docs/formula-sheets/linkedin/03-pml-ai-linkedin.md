# The Project Management Formula Sheet

> Every formula a delivery leader is accountable for — including the ones only PCI publishes

## What this is

**PML-AI · COMPLETE REFERENCE**

Forty-six formulas across project and programme delivery: business case, estimating, critical path, earned value, risk, agile and flow, procurement, quality, stakeholders and portfolio.

Eight of them exist nowhere else. The **governance arithmetic** in section 04 — decision latency, gate net value, committee capacity, baseline drift — is original to the PML-AI Body of Knowledge, because governance is the part of delivery everyone manages by instinct and nobody prices.

> **Save this one.** It is the reference, not a highlight reel.

## One rule before the rest

**CONVENTIONS**

Under hybrid delivery, velocity forecasting and earned value answer the same question through different instruments.

State which one governs the commitment **before** you report either.

> **And rates and periods must agree.** An annual rate against monthly periods is the arithmetic error the verification suite catches most often across the whole programme.

# 01 | Business case and benefits

Domains 2 and 16. Why the project exists, and whether it delivered.

## Appraising the case

**DOMAIN 2 · INVESTMENT**

| Formula | Meaning | Domain |
|---|---|---|
| `NPV = Σ CF_t ÷ (1 + r)^t − I_0` | Net present value | 2.3 |
| `IRR: NPV = 0` | Internal rate of return | 2.3 |
| `ROI = (Benefit − Cost) ÷ Cost` | Return on investment | 2.3 |
| `BCR = PV(benefits) ÷ PV(costs)` | Benefit-cost ratio | 2.3 |
| `Payback` | Period to recover the outlay | 2.3 |

> **Discount both sides or neither.** A benefit-cost ratio that discounts costs and takes benefits at face value will justify almost anything.

## Measuring what was actually delivered

**DOMAIN 16 · BENEFITS**

| Formula | Meaning | Domain |
|---|---|---|
| `Benefit realised = Actual measure − Baseline measure` | Benefits tracking | 16.3 |
| `Benefits shortfall = Forecast − Realised` | Realisation gap | 16.3 |
| `Benefit dependency: benefit lands only when all enablers deliver` | Benefits logic | 15.4 |

> **Without a pre-delivery baseline, a realisation figure cannot be computed — only asserted.** Capture it before delivery starts, not at handover, when the comparison no longer exists. The Institute registers the benefits measure as `EVA(benefit)` deliberately, to keep it clear of `EV` in earned value.

# 02 | Estimating and the network

Domain 6 — the quantitative flagship. Duration, sequence and float.

## Duration under uncertainty

**DOMAIN 6 · ESTIMATING**

| Formula | Meaning | Domain |
|---|---|---|
| `tE = (O + 4M + P) ÷ 6` | PERT expected duration | 6.2 |
| `tE = (O + M + P) ÷ 3` | Triangular expected duration | 6.2 |
| `σ = (P − O) ÷ 6` | PERT standard deviation | 6.2 |
| `σ^2 path = Σ σ^2 along the path` | Path variance | 6.4 |
| `Analogous = Past × (this driver ÷ past driver)` | Top-down estimating | 6.2 |
| `Parametric = Parameter × Rate` | Rate-based estimating | 6.2 |

> **Variances add. Standard deviations do not.** Sum the `σ^2` along the path and take the square root once, at the end. And three points from a single estimator is one number wearing three hats.

## The network and its float

**DOMAIN 6 · CRITICAL PATH**

| Formula | Meaning | Domain |
|---|---|---|
| `EF = ES + Duration` | Forward pass | 6.3 |
| `LS = LF − Duration` | Backward pass | 6.3 |
| `TF = LS − ES = LF − EF` | Total float | 6.3 |
| `FF = min(successor ES) − EF` | Free float | 6.3 |
| `Critical path: TF = 0` | The longest path | 6.3 |
| `Crash cost slope = (Crash − Normal cost) ÷ (Normal − Crash duration)` | Cost per period saved | 6.5 |

> **Total float from the starts must equal total float from the finishes.** If the two forms disagree, one of your passes is wrong. It costs nothing to check and catches a whole class of error — and crashing an activity off the critical path spends money to save nothing.

# 03 | Cost, earned value and risk

Domains 7 and 8. Where the project stands and where it will end.

## Position and forecast

**DOMAIN 7 · EARNED VALUE**

| Formula | Meaning | Domain |
|---|---|---|
| `CV = EV − AC` · `SV = EV − PV` | Variances | 7.3 |
| `CPI = EV ÷ AC` · `SPI = EV ÷ PV` | Performance indices | 7.3 |
| `Cost to date = Actuals + Accruals` | True cost to date | 7.2 |
| `EAC = AC + (BAC − EV)` | The variance was a one-off | 7.4 |
| `EAC = BAC ÷ CPI` | Performance to date persists | 7.4 |
| `EAC = AC + (BAC − EV) ÷ (CPI × SPI)` | Cost and schedule both persist | 7.4 |
| `VAC = BAC − EAC` | Variance at completion | 7.4 |
| `TCPI = (BAC − EV) ÷ (BAC − AC)` | Efficiency required to hit budget | 7.4 |

> **The third line breaks the first two.** If `AC` carries invoiced cost alone, `CPI` overstates performance and `EAC` understates cost — both invisibly. And set `TCPI` beside the `CPI` actually achieved: a `TCPI` far above it asserts a gain nobody has demonstrated.

## Schedule performance in time

**DOMAIN 6 · EARNED SCHEDULE**

| Formula | Meaning | Domain |
|---|---|---|
| `ES` | Earned schedule — the time at which the `EV` achieved was planned | 6.4 |
| `SV(t) = ES − AT` | Schedule variance in time | 6.4 |
| `SPI(t) = ES ÷ AT` | Time-based schedule efficiency | 6.4 |

> **`SV` and `SPI` are denominated in money.** At completion `EV = PV`, so both return to zero and 1.00 however late you finished. The time-based pair does not converge, which makes it the honest late-stage measure.

## Risk, priced properly

**DOMAIN 8 · RISK**

| Formula | Meaning | Domain |
|---|---|---|
| `EMV = Σ (probability × impact)` | Expected monetary value | 8.2 |
| `Risk score = Probability rating × Impact rating` | Qualitative sorting only | 8.2 |
| `Contingency ≈ Σ EMV, or P80 − P50` | Risk-based contingency | 8.3 |
| `Value of information = EV(with) − EV(without)` | Whether to buy analysis | 8.4 |
| `Residual exposure = Residual probability × Residual impact` | After response | 8.3 |

> **Ordinal ratings do not multiply.** A 3 × 4 = 12 heat-map score is a sorting device. It cannot be summed across risks and it cannot size a contingency. For anything touching money, use expected value or simulation.

# 04 | The governance arithmetic

Original to the PML-AI Body of Knowledge. Governance is the part of delivery everyone manages by instinct — these formulas price it.

## What a decision costs you in time {statement}

**DOMAIN 3 · DECISION LATENCY**

`E[wait] = M ÷ 2 + L`

Meeting interval `M`, paper lead time `L`. The expected wait for a committee decision — and it **sums across escalation tiers**.

> **A monthly committee with a two-week paper deadline has an expected wait of four weeks.** Escalate through two tiers and it is eight, before anyone has disagreed about anything. Multiply that by the cost of delay and governance stops being free.

## The price of elapsed time {statement}

**DOMAINS 1, 3 · COST OF DELAY**

`Cost of delay = Value forgone per unit of elapsed time`

This is the exchange rate between time and money — the price at which governance latency, gate duration and escalation paths are all evaluated.

> **Without a cost of delay, every governance argument is aesthetic.** With one, "add another approval tier" becomes a number, and the conversation changes character entirely.

## Is this gate worth holding

**DOMAIN 3 · GATE NET VALUE**

`Gate net value = P(defect) × build-fix cost − [review cost + elapsed × cost of delay + P(defect) × (P(detect) × design-fix + P(miss) × build-fix)]`

A gate earns its place only when the defects it catches cost more than the gate costs to hold.

> **A gate that catches nothing is not free — it costs its review effort plus its elapsed time priced at the cost of delay.** This is the formula that retires stage gates, and almost no organisation computes it.

## Can your governance absorb the demand

**DOMAIN 3 · COMMITTEE CAPACITY**

`Capacity = Meetings per year × Substantive items per meeting`

`Utilisation = Demand ÷ Capacity`

> **Governance queues behave like any other constrained system.** Push utilisation towards 1.0 and the wait time rises non-linearly — which is felt as "the board is a bottleneck" long before anyone measures it.

## The architecture choice, priced

**DOMAIN 4 · INTERFACES**

`Mesh interfaces = n(n − 1) ÷ 2`

`Layered interfaces = n` to an integration layer

Adding one party to a mesh of `n` creates `n` new interfaces, not one.

> **This is the same arithmetic as communication channels, applied to system and organisational design.** It is the case for an integration layer, and it is why adding a party to a late programme so rarely accelerates it.

## The rule that catches missing scope {statement}

**DOMAIN 4 · THE HUNDRED-PER-CENT RULE**

`Σ children − parent = 0` at every level of the WBS

A non-zero result is an omission or a duplication. There is no third explanation.

> **Run it as an arithmetic check, not a review opinion.** It is the cheapest scope-assurance test available and it takes one column in a spreadsheet.

## Pricing a change honestly

**DOMAIN 4 · ASSESSED TOTAL IMPACT**

`Assessed total impact = direct + (schedule weeks × cost of delay) + rework + interface re-verification + regression + documentation`

The basis a delegation threshold must read on — not the direct cost alone.

> **Approve on direct cost and you have set your delegation threshold against a fraction of the real number.** Small changes clear the threshold, and the missing terms land later as schedule.

## Measuring the drift {statement}

**DOMAIN 4 · BASELINE DRIFT**

`Baseline drift = (change count × average direct cost) + (affected count × average weeks × cost of delay)`

The cumulative-test threshold is derived from the observed change rate.

> **Individually approvable, collectively fatal.** Every change passed its own test; nobody ran the cumulative one. This formula is the cumulative test.

# 05 | Adaptive delivery and flow

Domain 13. The same questions, different instruments.

## Flow and forecast

**DOMAIN 13 · AGILE**

| Formula | Meaning | Domain |
|---|---|---|
| `Velocity = Points completed ÷ Sprint` | Delivery rate | 13.3 |
| `Sprints remaining = Points remaining ÷ Average velocity` | Forecast to completion | 13.3 |
| `Capacity = Members × Available days × Focus factor` | Sprint capacity | 13.3 |
| `Cycle time = WIP ÷ Throughput` | Little's Law | 13.4 |
| `Throughput = Items completed ÷ Period` | Delivery rate in items | 13.4 |
| `Flow efficiency = Active time ÷ Elapsed time` | Proportion actually worked | 13.4 |
| `WSJF = Cost of delay ÷ Job size` | Prioritisation | 13.3 |

> **Halve the work in progress and cycle time halves, at the same throughput.** That is why limiting WIP speeds delivery without anyone working faster. And `WSJF` requires a real cost of delay — guess it and you have ranked by guess.

## Burn-down hides the thing you need {statement}

**DOMAIN 13 · REPORTING TO A GATE**

A **burn-down** plots work remaining, so scope added mid-flight looks identical to slow progress.

A **burn-up** plots completed work and total scope as two separate lines.

> **Scope growth becomes visible as scope growth.** For anything reporting to a phase gate, use burn-up. The second line is the entire point.

# 06 | Procurement, quality and portfolio

Domains 9, 10 and 15.

## Where the incentive stops working

**DOMAIN 10 · CONTRACTS**

| Formula | Meaning | Domain |
|---|---|---|
| `PTA = [(Ceiling − Target price) ÷ Buyer share] + Target cost` | Point of total assumption | 10.3 |
| `Fee = Target fee + Share × (Target cost − Actual cost)` | Incentive fee | 10.3 |
| `Pain/gain = Share ratio × (Actual − Target)` | Target-cost mechanism | 10.3 |
| `LD exposure = LD rate × Days late` | Liquidated damages | 10.4 |
| `Weighted score = Σ (criterion weight × score)` | Tender evaluation | 10.2 |

> **Above the PTA the seller absorbs every further pound.** Check it in one line: at an actual cost equal to the PTA, the final price must equal the ceiling. If it does not, you have used the seller's share ratio in the denominator instead of the buyer's.

## Quality, measured

**DOMAIN 9 · QUALITY**

| Formula | Meaning | Domain |
|---|---|---|
| `Cost of quality = Prevention + Appraisal + Internal + External failure` | Total quality cost | 9.2 |
| `DPMO = (Defects ÷ (Units × Opportunities)) × 1,000,000` | Defect rate | 9.3 |
| `Control limits = Mean ± 3σ` | Statistical process control | 9.3 |
| `Process capability = (USL − LSL) ÷ 6σ` | Capability index | 9.3 |
| `First-pass yield = Passing first time ÷ Started` | Right first time | 9.3 |

> **Control limits describe what the process does. Specification limits describe what the customer requires.** A process can be perfectly in control and entirely incapable — and the response to each is different.

## Portfolio and capacity

**DOMAIN 15 · PORTFOLIO**

| Formula | Meaning | Domain |
|---|---|---|
| `Portfolio value = Σ NPV of components` | Aggregate value | 15.2 |
| `Scoring model = Σ (weight × score)` | Prioritisation | 15.2 |
| `Σ resource demand ≤ Available capacity` | Feasibility constraint | 15.3 |
| `Resource utilisation = Productive ÷ Available hours` | Utilisation | 7.5 |

> **Do not plan to 100% utilisation.** It leaves nothing to absorb variability, and queues grow non-linearly as utilisation approaches full — the same effect as committee capacity in section 04, in a different queue.

## The ten that cost the most

**MOST MISAPPLIED · 1–5**

1. Risk scores multiplied and then summed
2. `SV` read as a schedule measure late in a project
3. `PTA` computed with the seller's share ratio
4. Velocity taken as best-ever rather than a rolling average
5. Burn-down used for gate reporting

## And the other five

**MOST MISAPPLIED · 6–10**

6. Standard deviations summed along a path
7. Crashing an activity that is not on the critical path
8. Control limits confused with specification limits
9. Change approved on direct cost alone, not assessed total impact
10. Benefits claimed with no pre-delivery baseline

> **Nine of these produce a report that passes review.** The tenth produces a benefits case that cannot be checked at all.

## The Body of Knowledge is open

**PCI AI · PROJECT CONTROLS INSTITUTE GLOBAL**

Every formula here is developed to full depth in the PML-AI Body of Knowledge — sixteen domains with worked examples and calculation exercises, at the domain cited on each slide. The governance arithmetic in section 04 is original to it.

Published openly. **No registration, no email gate.**

**projectcontrolsinstitute.org**
