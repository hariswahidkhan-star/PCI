# The Project Controls Formula Sheet

> Every formula the discipline runs on — and the mistake each one attracts

## What this is

**PCL-AI · COMPLETE REFERENCE**

Fifty-eight formulas across the whole of project controls: accounting, revenue recognition, budgeting, cost, earned value, forecasting, scheduling, risk, commercial and working capital.

Every one is drawn from the PCL-AI Body of Knowledge and carries the Knowledge Area where it is worked in full.

Against each, the error it attracts in practice — because a correct formula on a compromised input produces a confident, well-formatted, entirely wrong answer.

> **Save this one.** It is the reference, not a highlight reel.

## Two symbols. One page. Constant collision.

**NOTATION · THE CLASH RULE**

`PV` is **Planned Value** in earned value.

`PV(x)` is **present value** in discounting.

Both appear in the same monthly cost report.

> **Write "present value" in words, or `PV(x)`, whenever you discount.** This pair collides more often than any other in the discipline, and the Style Spine fixes it as a rule rather than a preference.

# 01 | Accounting and reporting

Domains 1 and 2. Where project reality becomes a number the accounts will recognise.

## The accounting core

**DOMAINS 1–2 · THE MODEL**

| Formula | Meaning | KA |
|---|---|---|
| `A = L + E` | The accounting equation | 1.1.1 |
| `Σ Debits = Σ Credits` | Double-entry invariant | 1.1.3 |
| `Retained earnings = Opening + Income − Expenses − Distributions` | Equity movement | 1.1.1 |
| `Depreciation = (Cost − Residual) ÷ Useful life` | Straight line | 1.3.4 |
| `Carrying amount = Cost − Accumulated depreciation − Impairment` | Net book value | 1.3.4 |

> **A trial balance that balances proves arithmetic, not correctness.** Both sides can be equally and identically wrong.

## Provisions and the time value of an obligation

**DOMAIN 1 · IAS 37**

| Formula | Meaning | KA |
|---|---|---|
| `Expected value = Σ (probability_i × outcome_i)` | Provision over a large population | 1.4.3 |
| `Present value = Future amount ÷ (1 + r)^n` | Discounting a provision | 1.4.3 |

> **Expected value applies to a population.** For a single obligation with one outcome, the most likely amount is the measure — using expected value there produces a number that will never be paid.

## Revenue on a contract

**DOMAIN 2 · IFRS 15**

| Formula | Meaning | KA |
|---|---|---|
| `PoC = Costs incurred ÷ Total estimated costs` | Cost-to-cost percentage of completion | 2.2.6 |
| `Cumulative revenue = PoC × Transaction price` | Over-time revenue | 2.2.6 |
| `Allocated price_i = Transaction price × (SSP_i ÷ Σ SSP)` | Allocation to obligations | 2.2.5 |
| `Capitalised borrowing cost = Weighted-avg qualifying expenditure × rate` | IAS 23 | 2.4.4 |
| `Contract asset / (liability) = Revenue recognised − Amounts billed` | Over- and under-billing | 7.5 |

> **`PoC` is cumulative, and so is the revenue it produces.** Deduct what you have already recognised before booking the period. And uninstalled materials sitting in the numerator inflate progress that has not happened.

# 02 | Budget, cost and variance

Domains 3 to 5. The baseline, the cost states, and the bridge from plan to actual.

## Building the baseline

**DOMAIN 3 · BUDGET**

| Formula | Meaning | KA |
|---|---|---|
| `BAC = Σ control-account budgets + contingency` | Cost baseline | 3.1.4 |
| `Total authorised budget = BAC + Management reserve` | Full authorisation | 3.1.4 |
| `Analogous estimate = Past cost × (this driver ÷ past driver)` | Analogous estimating | 3.2.2 |
| `Parametric estimate = Parameter × Rate` | Parametric estimating | 3.2.2 |

> **Management reserve sits outside `BAC`.** Fold it in and you have quietly given yourself a contingency nobody approved and the baseline no longer measures anything.

## Cost behaviour and absorption

**DOMAIN 5 · COST**

| Formula | Meaning | KA |
|---|---|---|
| `Total cost = Fixed + (Variable per unit × Volume)` | Cost behaviour | 5.1.1 |
| `OAR = Budgeted overhead ÷ Budgeted activity base` | Overhead absorption rate | 5.1.3 |
| `Over/(under) absorption = Absorbed − Incurred` | Absorption variance | 5.1.3 |

## The one line that governs every index

**DOMAIN 5 · THE COST STATES**

`Cost to date = Actuals + Accruals`

Cost moves through three states — **commitment**, then **accrual**, then **actual**. Only the third has an invoice.

If `AC` carries invoiced cost alone, work has been performed that nobody has billed you for.

> **`CPI` overstates performance and `EAC` understates cost, and both errors are invisible in the report.** Reconcile goods-received-not-invoiced against the accrual booked before you compute anything in the next section.

## The variance bridge

**DOMAIN 4 · VARIANCE**

| Formula | Meaning | KA |
|---|---|---|
| `Price variance = (Actual price − Standard price) × Actual quantity` | Price effect | 4.2.3 |
| `Quantity variance = (Actual qty − Standard qty) × Standard price` | Quantity effect | 4.2.3 |
| `Total variance = Price variance + Quantity variance` | Reconciliation check | 4.2.3 |

> **Note which multiplier belongs to which.** Price variance takes **actual** quantity; quantity variance takes **standard** price. Swap them and the two will not sum to the total — which is exactly why the third line exists.

# 03 | Earned value and forecasting

Domain 6, with Domain 9 for adaptive delivery. Where you are, and where you will end.

## Position

**DOMAIN 6 · THE INDICES**

| Formula | Meaning | KA |
|---|---|---|
| `CV = EV − AC` | Cost variance | 6.2.1 |
| `SV = EV − PV` | Schedule variance, in currency | 6.2.1 |
| `CPI = EV ÷ AC` | Cost efficiency to date | 6.2.2 |
| `SPI = EV ÷ PV` | Schedule efficiency in cost terms | 6.2.2 |
| `% complete = EV ÷ BAC` | Progress by value | 6.1 |

> **`SV` and `SPI` are denominated in money, not time.** At completion `EV = PV`, so both return to zero and 1.00 however late you finished. They are honest early and misleading late.

## The EAC family

**DOMAIN 6 · FORECAST**

| Formula | The assumption it encodes | KA |
|---|---|---|
| `EAC = AC + ETC` | Identity — always true | 6.3.1 |
| `EAC = AC + (BAC − EV)` | The variance was a one-off | 6.3.2 |
| `EAC = BAC ÷ CPI` | Performance to date persists | 6.3.2 |
| `EAC = AC + (BAC − EV) ÷ CPI` | Same assumption, remaining-work form | 6.3.2 |
| `EAC = AC + (BAC − EV) ÷ (CPI × SPI)` | Cost and schedule both persist | 6.3.2 |
| `VAC = BAC − EAC` | Variance at completion | 6.3.4 |

> **The arithmetic is trivial. The assumption is the professional judgement** — and it is the assumption a board will ask you to defend, not the division.

## Which EAC, and why

**DECISION AID**

| If | Then |
|---|---|
| The variance was a closed, one-off event | `AC + (BAC − EV)` |
| Performance to date will persist | `BAC ÷ CPI` |
| Cost **and** schedule pressure both persist | `AC + (BAC − EV) ÷ (CPI × SPI)` |
| Remaining scope differs materially | Bottom-up ETC — indices do not apply |

> **Publish the range and name the assumption against each.** A single EAC quoted to the nearest thousand claims a precision the method cannot support.

## The check that exposes a wishful budget

**DOMAIN 6 · TCPI**

| Formula | Meaning | KA |
|---|---|---|
| `TCPI (to BAC) = (BAC − EV) ÷ (BAC − AC)` | Efficiency needed to still hit budget | 6.2.3 |
| `TCPI (to EAC) = (BAC − EV) ÷ (EAC − AC)` | Efficiency needed to hit a revised forecast | 6.2.3 |

> **Set `TCPI` beside the `CPI` you have actually achieved.** A `TCPI` far above it is asserting an efficiency gain nobody has demonstrated. And an identity worth knowing: when `EAC = BAC ÷ CPI`, `TCPI (to EAC)` equals `CPI` exactly.

## Schedule performance, measured in time

**DOMAIN 6 · EARNED SCHEDULE**

| Formula | Meaning | KA |
|---|---|---|
| `ES = M + (EV − PV_M) ÷ (PV_M+1 − PV_M)` | Interpolate between the cumulative-PV periods bracketing `EV` | 6.4.3 |
| `SV(t) = ES − AT` | Schedule variance in time | 6.4.3 |
| `SPI(t) = ES ÷ AT` | Time-based schedule efficiency | 6.4.3 |

> **These do not converge at completion.** Late in a project, earned schedule is the honest measure and the currency-based pair is not. Use cumulative planned value in the interpolation, never the period figure.

## Earned value under adaptive delivery

**DOMAIN 9 · AGILE EVM**

| Formula | Meaning | KA |
|---|---|---|
| `% complete = Points completed ÷ Total planned points` | AgileEVM progress | 9.5.3 |
| `EV = % complete × BAC` | Earned value from points | 9.5.3 |
| `Velocity = Points completed ÷ Sprint` | Delivery rate | 9.3 |
| `Sprints remaining = Points remaining ÷ Velocity` | Forecast to completion | 9.3 |
| `Run rate = Team cost ÷ Sprint` | Cost per sprint | 9.5.2 |
| `Cycle time = WIP ÷ Throughput` | Little's Law | 9.4 |

> **Point inflation breaks the first two lines.** If a story point in sprint 20 is smaller than in sprint 4, `% complete` rises without work being done. And use a rolling average velocity, never best-ever.

# 04 | Schedule and risk

Domains 10 and 12. The network, its float, and the uncertainty priced into contingency.

## Network and float

**DOMAIN 10 · CPM**

| Formula | Meaning | KA |
|---|---|---|
| `EF = ES + Duration` | Forward pass | 10.2 |
| `LS = LF − Duration` | Backward pass | 10.2 |
| `Total float = LS − ES = LF − EF` | Slack without delaying the project | 10.2.4 |
| `Free float = min(successor ES) − EF` | Slack without delaying a successor | 10.2.4 |
| `Critical path: TF = 0` | The longest path | 10.2.3 |

> **A free integrity check.** Total float from the starts must equal total float from the finishes. If the two disagree, one of your passes is wrong — and constraints can manufacture a zero float that is not a critical path at all.

## Duration under uncertainty

**DOMAIN 10 · PERT**

| Formula | Meaning | KA |
|---|---|---|
| `tE = (O + 4M + P) ÷ 6` | PERT expected duration | 10.1.4 |
| `σ = (P − O) ÷ 6` | PERT standard deviation | 10.1.4 |
| `σ^2 path = Σ σ^2 along the path` | Path variance | 10.3.4 |
| `Crash cost slope = (Crash − Normal cost) ÷ (Normal − Crash duration)` | Cost per period saved | 10.3.1 |

> **Variances add. Standard deviations do not.** Sum the `σ^2` along the path and take the square root once, at the end. And crashing an activity that is not on the critical path spends money to save nothing.

## Risk, and where contingency comes from

**DOMAIN 12 · RISK**

| Formula | Meaning | KA |
|---|---|---|
| `EMV = Probability × Impact` | Expected monetary value | 12.2.3 |
| `Total EMV = Σ (P_i × I_i)` | Portfolio exposure | 12.2.3 |
| `Contingency ≈ Σ EMV, or P80 − P50` | Risk-based contingency | 12.3.1 |

> **A percentage of budget is a habit, not an analysis.** If contingency does not reconcile to quantified exposure, nobody can say what it is for or when it may be released.

# 05 | Commercial and cash

Domains 7 and 11. The contract, the valuation and the working capital underneath.

## Contract mechanics

**DOMAIN 7 · COMMERCIAL**

| Formula | Meaning | KA |
|---|---|---|
| `Fee = Target fee + Contractor share × (Target cost − Actual cost)` | CPIF incentive fee | 7.1.3 |
| `Pain/gain = Share ratio × (Actual − Target)` | Target-cost mechanism | 7.1.4 |
| `LD exposure = LD rate × Days late` | Liquidated damages | 7.2.3 |
| `Remeasured value = Actual quantity × BoQ rate` | Remeasurement | 7.3.4 |

## Getting paid

**DOMAIN 7 · VALUATION**

`Amount due = Σ(% complete × item amount) − Retention − Previous payments`

Every interim application is this line. The failures are always in the last two terms.

> **Retention release is the one most often missed** at the milestone that triggers it — and previous payments must be cumulative, or you will bill the same work twice and find out at final account.

## Working capital

**DOMAIN 11 · CASH CYCLE**

| Formula | Meaning | KA |
|---|---|---|
| `DSO = Receivables ÷ Daily revenue` | Days sales outstanding | 11.1.3 |
| `DIO = Inventory ÷ Daily COGS` | Days inventory outstanding | 11.A.1 |
| `DPO = Payables ÷ Daily COGS` | Days payables outstanding | 11.A.1 |
| `CCC = DSO + DIO − DPO` | Cash conversion cycle | 11.A.1 |
| `Cash freed ≈ Δ DSO × Daily revenue` | Working capital released | 11.A.1 |

> **`DPO` carries a minus sign.** Paying suppliers later shortens your cash cycle and lengthens theirs — which is a commercial decision about the supply chain, not a treasury optimisation.

## Never accept this number

**PROGRESS MEASUREMENT**

| Work package character | Method |
|---|---|
| Spans a single period | 0/100 |
| Two periods, discrete ends | 50/50 |
| Countable homogeneous output | Units complete |
| Long, with verifiable stages | Milestone weighting |
| No discrete output | Level of effort — never the default |

> **Fix the method per work package before work starts.** And never accept a percentage complete supplied by the party being measured. That figure is a commercial position, not a measurement.

## The ten that cost the most

**MOST MISAPPLIED · 1–5**

1. `CPI` computed on invoiced cost, with no accrual
2. `SV` read as a schedule measure late in a project
3. A single `EAC` presented as *the* forecast
4. `TCPI` never set beside the `CPI` achieved
5. Percentage complete supplied by the measured party

## And the other five

**MOST MISAPPLIED · 6–10**

6. Standard deviations summed along a schedule path
7. `EMV` applied to a single binary event
8. Contingency set as a percentage of budget
9. Crashing an activity that is not on the critical path
10. Management reserve folded inside `BAC`

> **Every one of these produces a report that passes review.** That is precisely why they are the expensive ones.

## The Body of Knowledge is open

**PCI AI · PROJECT CONTROLS INSTITUTE GLOBAL**

Every formula here is developed to full handbook depth in the PCL-AI Body of Knowledge — thirteen domains, sixty-one Knowledge Areas, with worked examples, figures and self-check exercises at the KA cited on each slide.

Published openly. **No registration, no email gate.**

**projectcontrolsinstitute.org**
