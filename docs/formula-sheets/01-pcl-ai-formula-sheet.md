# PCL-AI Formula Sheet

> PCI AI Project Controls Leader — the complete quantitative reference

**PCI AI · Project Controls Institute Global, Inc.**
Formula Sheet 01 · First edition

---

## How to use this sheet

Every formula the PCL-AI Body of Knowledge teaches, in one place, with the Knowledge Area where it is worked in full.

Three columns matter. **Formula** is the expression. **Meaning** is what it answers. **Watch for** is the mistake that formula actually attracts in practice — the reason a correct expression still produces a wrong answer.

This sheet is a study aid. It is **not provided in the examination**; candidates are expected to know the core formulas.

**Conventions.** Currency in USD unless stated. Ratios and indices to 2 dp. Adverse amounts in parentheses. Periods are consistent within any single calculation — never mix monthly and cumulative figures in one expression.

> **The notation clash rule.** `PV` means **Planned Value** in earned-value contexts and **present value** in discounting contexts. Write "present value" in words, or `PV(x)`, whenever discounting. These two collide on the same page more often than any other pair of symbols in the discipline.

---

## 1. Notation

| Symbol | Meaning | Unit |
|---|---|---|
| `A`, `L`, `E` | Assets, liabilities, equity | currency |
| `PV` (BCWS) | Planned value / budgeted cost of work scheduled | currency |
| `EV` (BCWP) | Earned value / budgeted cost of work performed | currency |
| `AC` (ACWP) | Actual cost / actual cost of work performed | currency |
| `BAC` | Budget at completion | currency |
| `CV`, `SV` | Cost variance, schedule variance | currency |
| `CPI`, `SPI` | Cost performance index, schedule performance index | ratio |
| `EAC`, `ETC` | Estimate at completion, estimate to complete | currency |
| `VAC` | Variance at completion | currency |
| `TCPI` | To-complete performance index | ratio |
| `ES`, `AT` | Earned schedule, actual time | periods |
| `PoC` | Percentage of completion | % |
| `O`, `M`, `P` | Optimistic, most likely, pessimistic duration | periods |
| `tE`, `σ` | PERT expected duration, standard deviation | periods |
| `ES`, `EF`, `LS`, `LF` | Early start, early finish, late start, late finish | date/period |
| `TF`, `FF` | Total float, free float | periods |
| `r`, `n` | Discount rate per period, number of periods | ratio, count |
| `EMV` | Expected monetary value | currency |

---

## 2. Accounting and financial reporting · Domains 1–2

| Formula | Meaning | Watch for | KA |
|---|---|---|---|
| `A = L + E` | The accounting equation | — | 1.1.1 |
| `Σ Debits = Σ Credits` | Double-entry invariant | A trial balance that balances proves arithmetic, not correctness | 1.1.3 |
| `Retained earnings = Opening + Income − Expenses − Distributions` | Equity movement | — | 1.1.1 |
| `Depreciation = (Cost − Residual) ÷ Useful life` | Straight line | Residual omitted; life not reassessed | 1.3.4 |
| `Carrying amount = Cost − Accumulated depreciation − Impairment` | Net book value | — | 1.3.4 |
| `Expected value = Σ (probability × outcome)` | Provisions, variable consideration | Using most-likely outcome where the population is large | 1.4.3 / 2.2.4 |
| `Present value = Amount ÷ (1 + r)^n` | Discounting a provision | — | 1.4.3 |
| `PoC = Costs incurred ÷ Total estimated costs` | Cost-to-cost percentage of completion | Uninstalled materials inflating the numerator | 2.2.6 |
| `Cumulative revenue = PoC × Transaction price` | IFRS 15 over-time revenue | Applying to cumulative, then forgetting to deduct revenue already recognised | 2.2.6 |
| `Allocated price = Transaction price × (SSP_i ÷ Σ SSP)` | IFRS 15 allocation | — | 2.2.5 |
| `Contract asset / (liability) = Revenue recognised − Amounts billed` | Over/under-billing position | Sign convention reversed | 7.5 |

---

## 3. Budgeting, cost and performance · Domains 3–5

| Formula | Meaning | Watch for | KA |
|---|---|---|---|
| `BAC = Σ control-account budgets + contingency` | Cost baseline | Management reserve wrongly included | 3.1.4 |
| `Total authorised budget = BAC + Management reserve` | Full authorisation | — | 3.1.4 |
| `Analogous estimate = Past cost × (this driver ÷ past driver)` | Analogous estimating | Driver not actually the cost driver | 3.2.2 |
| `Parametric estimate = Parameter × Rate` | Parametric estimating | Rate from a different scope basis | 3.2.2 |
| `Total cost = Fixed + (Variable per unit × Volume)` | Cost behaviour | — | 5.1.1 |
| `OAR = Budgeted overhead ÷ Budgeted activity base` | Overhead absorption rate | — | 5.1.3 |
| `Over/(under) absorption = Absorbed − Incurred` | Absorption variance | — | 5.1.3 |
| **`Cost to date = Actuals + Accruals`** | **True cost to date** | **The single most damaging omission in project controls** | 5.2.1 |
| `Price variance = (Actual price − Standard price) × Actual quantity` | Variance decomposition | Standard quantity used in place of actual | 4.2.3 |
| `Quantity variance = (Actual qty − Standard qty) × Standard price` | Variance decomposition | Actual price used in place of standard | 4.2.3 |
| `Total variance = Price variance + Quantity variance` | Reconciliation check | If the two do not sum to the total, one used the wrong multiplier | 4.2.3 |

> **The accrual rule.** Every index below depends on `AC`. If `AC` excludes work performed but not yet invoiced, `CPI` overstates performance and `EAC` understates cost — and both errors are invisible in the report. Reconcile goods-received-not-invoiced to booked accruals before computing anything in Section 4.

---

## 4. Earned value and forecasting · Domains 6, 9

### 4.1 Position

| Formula | Meaning | Watch for | KA |
|---|---|---|---|
| `CV = EV − AC` | Cost variance | Negative is adverse | 6.2.1 |
| `SV = EV − PV` | Schedule variance (in currency) | Goes to zero at completion regardless of lateness | 6.2.1 |
| `CPI = EV ÷ AC` | Cost efficiency to date | Computed on invoiced cost only | 6.2.2 |
| `SPI = EV ÷ PV` | Schedule efficiency in cost terms | Also returns to 1.00 at completion | 6.2.2 |
| `% complete = EV ÷ BAC` | Progress by value | Not the same as physical or time progress | 6.1 |

### 4.2 Forecast

| Formula | Assumption it encodes | KA |
|---|---|---|
| `EAC = AC + ETC` | Identity — always true | 6.3.1 |
| `EAC = AC + (BAC − EV)` | The variance was a **one-off**; remaining work performs to plan | 6.3.2 |
| `EAC = BAC ÷ CPI` | Cost performance to date **persists** to completion | 6.3.2 |
| `EAC = AC + (BAC − EV) ÷ CPI` | Same assumption, stated in remaining-work form | 6.3.2 |
| `EAC = AC + (BAC − EV) ÷ (CPI × SPI)` | **Both** cost and schedule pressure persist | 6.3.2 |
| `VAC = BAC − EAC` | Variance at completion; negative is an overrun | 6.3.4 |
| `TCPI (to BAC) = (BAC − EV) ÷ (BAC − AC)` | Efficiency required to still hit the budget | 6.2.3 |
| `TCPI (to EAC) = (BAC − EV) ÷ (EAC − AC)` | Efficiency required to hit a revised forecast | 6.2.3 |

> **Identity worth knowing.** `BAC ÷ CPI` is algebraically identical to `AC × BAC ÷ EV`. And when `EAC = BAC ÷ CPI`, then `TCPI (to EAC) = CPI` exactly — the forecast assumes you continue performing precisely as you have been. If a forecast produces a TCPI far above the CPI achieved to date, it is asserting an improvement nobody has yet demonstrated.

### 4.3 Earned schedule

| Formula | Meaning | Watch for | KA |
|---|---|---|---|
| `ES = M + (EV − PV_M) ÷ (PV_M+1 − PV_M)` | Interpolate between the cumulative-PV periods bracketing EV | Using period rather than cumulative PV | 6.4.3 |
| `SV(t) = ES − AT` | Schedule variance in **time** | — | 6.4.3 |
| `SPI(t) = ES ÷ AT` | Time-based schedule efficiency | — | 6.4.3 |

**Why earned schedule exists.** `SV` and `SPI` are computed in currency and both converge to zero and 1.00 at completion, however late the project finishes. `SV(t)` and `SPI(t)` do not, which makes them the more honest late-stage measures.

### 4.4 Adaptive delivery

| Formula | Meaning | Watch for | KA |
|---|---|---|---|
| `% complete = Points completed ÷ Total planned points` | AgileEVM progress | Point inflation across sprints | 9.5.3 |
| `EV = % complete × BAC` | Earned value in adaptive delivery | — | 9.5.3 |
| `Velocity = Points completed ÷ Sprint` | Delivery rate | Averaging across a changed team | 9.3 |
| `Sprints remaining = Points remaining ÷ Velocity` | Forecast to completion | Using best-ever velocity rather than rolling average | 9.3 |
| `Run rate = Team cost ÷ Sprint` | Cost per sprint | — | 9.5.2 |
| `Cycle time = WIP ÷ Throughput` | Little's Law | Only valid for a stable system over the measured period | 9.4 |

---

## 5. Scheduling and risk · Domains 10, 12

| Formula | Meaning | Watch for | KA |
|---|---|---|---|
| `EF = ES + Duration` | Forward pass | Off-by-one at period boundaries | 10.2 |
| `LS = LF − Duration` | Backward pass | — | 10.2 |
| `TF = LS − ES = LF − EF` | Total float | If the two expressions disagree, the pass is wrong | 10.2.4 |
| `FF = min(successor ES) − EF` | Free float | Ignoring lags on the successor link | 10.2.4 |
| `Critical path: TF = 0` | Longest path | Constraints creating artificial zero float | 10.2.3 |
| `tE = (O + 4M + P) ÷ 6` | PERT expected duration | Three points sourced from one person | 10.1.4 |
| `σ = (P − O) ÷ 6` | PERT standard deviation | — | 10.1.4 |
| `σ^2 path = Σ σ^2 of activities on the path` | Path variance | Variances add; standard deviations do not | 10.3.4 |
| `Crash cost slope = (Crash cost − Normal cost) ÷ (Normal duration − Crash duration)` | Cost per period saved | Crashing an activity that is not on the critical path | 10.3.1 |
| `EMV = Probability × Impact` | Expected monetary value | Applied to a one-off event where the expected value never occurs | 12.2.3 |
| `Contingency ≈ Σ EMV (or P80 − P50 from simulation)` | Risk-based contingency | Percentage applied in place of quantification | 12.3.1 |

> **Variance adds, deviation does not.** To combine schedule uncertainty along a path, sum the **variances** (`σ^2`), then take the square root. Summing standard deviations overstates path uncertainty substantially.

---

## 6. Commercial · Domain 7

| Formula | Meaning | Watch for | KA |
|---|---|---|---|
| `Fee = Target fee + Share ratio × (Target cost − Actual cost)` | CPIF incentive fee | Sign convention on an overrun | 7.1.3 |
| `Pain/gain = Share ratio × (Actual − Target)` | Target-cost mechanism | — | 7.1.4 |
| `LD exposure = LD rate × Days late` | Liquidated damages | Cap not applied | 7.2.3 |
| `Amount due = Σ(% complete × item amount) − Retention − Previous payments` | Interim payment application | Retention release omitted at the milestone | 7.4.3 |
| `Retention released = Retention held × Release %` | Retention mechanics | — | 7.4.4 |
| `Remeasured value = Actual quantity × BoQ rate` | Remeasurement contract | Rate applied to the wrong measured unit | 7.3.4 |

---

## 7. Process cycles and working capital · Domain 11

| Formula | Meaning | Watch for | KA |
|---|---|---|---|
| `DSO = Receivables ÷ Daily revenue` | Days sales outstanding | Revenue not annualised consistently | 11.1.3 |
| `DIO = Inventory ÷ Daily COGS` | Days inventory outstanding | — | 11.A.1 |
| `DPO = Payables ÷ Daily COGS` | Days payables outstanding | — | 11.A.1 |
| `CCC = DSO + DIO − DPO` | Cash conversion cycle | Sign on DPO | 11.A.1 |
| `Cash released ≈ Δ DSO × Daily revenue` | Working capital freed | — | 11.A.1 |

---

## 8. Decision aid — which EAC method

The formula is not the difficulty. Selecting and defending the assumption is.

| Question | If yes | Method |
|---|---|---|
| Was the variance caused by a discrete, closed event that will not recur? | → | `EAC = AC + (BAC − EV)` |
| Does the remaining work resemble the work done, with performance likely to persist? | → | `EAC = BAC ÷ CPI` |
| Is the project also behind, with schedule pressure driving cost? | → | `EAC = AC + (BAC − EV) ÷ (CPI × SPI)` |
| Is the remaining scope materially different from what has been done? | → | Bottom-up ETC; indices do not apply |

**Publish the range, not the point.** Report at least two scenarios with the assumption named against each. A single EAC quoted to the nearest thousand implies a precision the method cannot support.

---

## 9. Decision aid — which progress measurement method

| Work package character | Method | Note |
|---|---|---|
| Short duration, spans one period | 0/100 | No partial credit; simplest and least gameable |
| Two periods, discrete start and finish | 50/50 | Credit on start and completion |
| Countable, homogeneous output | Units complete | Strongest objective measure |
| Long duration, distinct verifiable stages | Milestone weighting | Weights fixed **before** work starts |
| Level of effort, no discrete output | Apportioned / LoE | Earns with time; never the default |
| Cannot be measured physically | — | Re-scope the work package |

> **The rule.** Fix the measurement method per work package before work begins, and never accept a percentage supplied by the party being measured. That figure is a commercial position, not a measurement.

---

## 10. Worked micro-examples

Each is internally consistent and can be reproduced from the sheet.

**Earned value at a data date.** `PV` 168,000 · `EV` 151,200 · `AC` invoiced 158,000 · accrual 12,400 · `BAC` 420,000.

- True `AC` = 158,000 + 12,400 = **170,400**
- `CPI` = 151,200 ÷ 170,400 = **0.887** *(on invoiced cost alone it would read 0.957)*
- `SPI` = 151,200 ÷ 168,000 = **0.900**
- `EAC` one-off = 170,400 + (420,000 − 151,200) = **439,200**
- `EAC` CPI persists = 420,000 ÷ 0.887 = **473,300**
- `EAC` both persist = 170,400 + 268,800 ÷ (0.887 × 0.900) = **507,000**
- `VAC` range = **(19,200)** to **(87,000)**

The accrual alone moves the forecast by 34,400 — the difference between a contained overrun and an uncontained one.

**Earned schedule.** Cumulative `PV` by month: 20, 55, 100, 150, 210, 280. `EV` = 170 at `AT` = 5.

- `EV` falls between month 4 (150) and month 5 (210)
- `ES` = 4 + (170 − 150) ÷ (210 − 150) = 4 + 0.333 = **4.333**
- `SV(t)` = 4.333 − 5 = **(0.667) months**
- `SPI(t)` = 4.333 ÷ 5 = **0.867**

**PERT.** `O` = 8, `M` = 12, `P` = 22.

- `tE` = (8 + 48 + 22) ÷ 6 = **13.0**
- `σ` = (22 − 8) ÷ 6 = **2.33**; `σ^2` = **5.44**

**Float.** `ES` = 4, duration 6, `LF` = 14.

- `EF` = 10 · `LS` = 8 · `TF` = 8 − 4 = **4** = 14 − 10 — the two forms agree

**Working capital.** Receivables 4,500,000 · daily revenue 50,000 · inventory 2,000,000 · daily COGS 40,000 · payables 3,200,000.

- `DSO` = **90** · `DIO` = **50** · `DPO` = **80** · `CCC` = 90 + 50 − 80 = **60 days**

---

## 11. The ten formulas most often misapplied

1. **`CPI` computed on invoiced cost.** Without accruals, every downstream index and forecast is wrong in the same direction.
2. **`SV` read as a schedule measure late in a project.** It is denominated in currency and converges to zero at completion regardless of lateness. Use `SV(t)`.
3. **A single `EAC` presented as the forecast.** Three defensible answers exist. The range and the named assumption are the deliverable.
4. **`TCPI` ignored as a reality check.** A TCPI well above the CPI achieved to date asserts an unearned improvement.
5. **Percentage complete supplied by the measured party.** A commercial position presented as a measurement.
6. **Standard deviations summed along a schedule path.** Variances add; take the square root at the end.
7. **`EMV` applied to a single one-off event.** Expected value describes a portfolio of risks, not a binary outcome that will either happen or not.
8. **Contingency set as a percentage of budget.** Unless it reconciles to quantified risk exposure, it is a habit rather than an analysis.
9. **Crashing an activity off the critical path.** Cost incurred, zero duration saved.
10. **Management reserve included in `BAC`.** `BAC` is the cost baseline; management reserve sits outside it and is released by governance.

---

## 12. Cross-references

| For the full treatment | See |
|---|---|
| Every formula worked with figures and self-check exercises | The PCL-AI Body of Knowledge, at the KA cited in each row |
| The published syllabus and weightings | PCL-AI Examination Content Outline |
| Why cut-off and accrual discipline dominate outcomes | *Knowing Early*, Knowledge Series 01, §4 and §16 |
| Assessing whether a professional can defend these methods | *Show Your Work*, Knowledge Series 02, §4 and §8 |

**Website** — https://projectcontrolsinstitute.org
