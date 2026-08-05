# PML-AI Formula Sheet

> PCI AI Project Management Leader — the complete quantitative reference

**PCI AI · Project Controls Institute Global, Inc.**
Formula Sheet 03 · First edition

---

## How to use this sheet

Every formula the PML-AI Body of Knowledge teaches, in one place, with the domain where it is worked in full.

Three columns matter. **Formula** is the expression. **Meaning** is what it answers. **Watch for** is the mistake that formula attracts in practice.

This sheet is a study aid. The examination specification, including whether any formula reference is provided, is confirmed by the Institute for each published examination form.

**Conventions.** Durations in working periods unless stated. Ratios and indices to 2 dp. Currency in USD unless stated. Where a formula is shared with the project-controls discipline, the PCL-AI sheet is the deeper treatment and is cross-referenced.

> **The delivery-approach rule.** Several formulas below behave differently under predictive and adaptive delivery. Velocity forecasting and earned value answer the same question — when will this finish and at what cost — through different instruments. Under hybrid delivery, state which instrument governs the commitment before reporting either.

---

## 1. Notation

| Symbol | Meaning | Unit |
|---|---|---|
| `PV`, `EV`, `AC` | Planned value, earned value, actual cost | currency |
| `BAC`, `EAC`, `ETC`, `VAC` | Budget, estimate at completion, to complete, variance at completion | currency |
| `CPI`, `SPI` | Cost and schedule performance indices | ratio |
| `O`, `M`, `P` | Optimistic, most likely, pessimistic | periods |
| `tE`, `σ` | PERT expected duration, standard deviation | periods |
| `ES`, `EF`, `LS`, `LF` | Early start, early finish, late start, late finish | date |
| `TF`, `FF` | Total float, free float | periods |
| `n` | Number of stakeholders, resources or periods | count |
| `WIP` | Work in progress | items |
| `EMV` | Expected monetary value | currency |
| `PTA` | Point of total assumption | currency |
| `r`, `t` | Discount rate per period, period index | ratio, count |

---

## 2. Business case and benefits · Domains 2, 16

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `NPV = Σ CF_t ÷ (1 + r)^t` | Net present value of the investment | Benefits counted gross of the cost to realise them | 2.3 |
| `ROI = (Benefit − Cost) ÷ Cost` | Return on investment | Period over which benefit is counted left unstated | 2.3 |
| `BCR = PV(benefits) ÷ PV(costs)` | Benefit-cost ratio | Discounting one side and not the other | 2.3 |
| `Payback = Years before recovery + (Unrecovered ÷ Cash flow that year)` | Simple payback | — | 2.3 |
| `IRR: NPV = 0` | Internal rate of return | Multiple roots where signs change repeatedly | 2.3 |
| `Benefit realised = Actual measure − Baseline measure` | Benefits tracking | No baseline captured before delivery | 16.3 |
| `Benefits shortfall = Forecast benefit − Realised benefit` | Realisation gap | Measured too early to be meaningful | 16.3 |

> **The baseline rule.** A benefit cannot be measured without a pre-delivery baseline. If the baseline was never captured, the realisation number is an assertion. Capture it before delivery starts, not at handover.

---

## 3. Estimating and scheduling · Domain 6

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `tE = (O + 4M + P) ÷ 6` | PERT expected duration (beta) | Three points from a single estimator | 6.2 |
| `tE = (O + M + P) ÷ 3` | Triangular expected duration | Confused with the PERT weighting | 6.2 |
| `σ = (P − O) ÷ 6` | PERT standard deviation | — | 6.2 |
| `σ^2 path = Σ σ^2 along the path` | Path variance | Deviations summed instead of variances | 6.4 |
| `EF = ES + Duration` | Forward pass | Off-by-one at period boundaries | 6.3 |
| `LS = LF − Duration` | Backward pass | — | 6.3 |
| `TF = LS − ES = LF − EF` | Total float | If the two forms disagree, the pass is wrong | 6.3 |
| `FF = min(successor ES) − EF` | Free float | Lags on the successor link ignored | 6.3 |
| `Critical path: TF = 0` | Longest path through the network | Constraints creating artificial zero float | 6.3 |
| `Crash cost slope = (Crash cost − Normal cost) ÷ (Normal − Crash duration)` | Cost per period saved | Crashing off the critical path | 6.5 |
| `Analogous estimate = Past × (this driver ÷ past driver)` | Top-down estimating | — | 6.2 |
| `Parametric estimate = Parameter × Rate` | Rate-based estimating | Rate from a different scope basis | 6.2 |

**Compression, compared**

| Method | Mechanism | Cost | Risk added |
|---|---|---|---|
| **Crashing** | Add resource to critical activities | Direct cost rises | Diminishing returns, coordination load |
| **Fast-tracking** | Overlap activities previously in sequence | Little direct cost | Rework risk rises materially |

---

## 4. Cost and earned value · Domain 7

Shared with the project-controls discipline. The PCL-AI Formula Sheet §4 is the deeper treatment, including earned schedule.

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `CV = EV − AC` | Cost variance | Negative is adverse | 7.3 |
| `SV = EV − PV` | Schedule variance in currency | Converges to zero at completion however late | 7.3 |
| `CPI = EV ÷ AC` | Cost efficiency | `AC` excluding accruals | 7.3 |
| `SPI = EV ÷ PV` | Schedule efficiency in cost terms | — | 7.3 |
| `EAC = AC + (BAC − EV)` | Variance was a one-off | — | 7.4 |
| `EAC = BAC ÷ CPI` | Performance to date persists | — | 7.4 |
| `EAC = AC + (BAC − EV) ÷ (CPI × SPI)` | Cost and schedule pressure both persist | — | 7.4 |
| `VAC = BAC − EAC` | Variance at completion | — | 7.4 |
| `TCPI = (BAC − EV) ÷ (BAC − AC)` | Efficiency required to hit budget | Quoted without comparing to CPI achieved | 7.4 |
| `Cost to date = Actuals + Accruals` | True cost to date | The commonest omission in the discipline | 7.2 |
| `Resource utilisation = Productive hours ÷ Available hours` | Utilisation | Target set at 100%, which leaves no capacity for variability | 7.5 |

> **Which EAC.** The formula is not the difficulty; the assumption is. Was the variance a closed one-off, does performance persist, or are cost and schedule compounding? State the assumption and publish a range rather than a point.

---

## 5. Risk and uncertainty · Domain 8

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `EMV = Probability × Impact` | Expected monetary value | Applied to a single binary event | 8.2 |
| `Total EMV = Σ (P_i × I_i)` | Portfolio expected value | — | 8.2 |
| `Risk score = Probability rating × Impact rating` | Qualitative ranking | Ordinal ratings multiplied as if they were cardinal | 8.2 |
| `Contingency ≈ Σ EMV, or P80 − P50 from simulation` | Risk-based contingency | Percentage of budget used instead | 8.3 |
| `EV of a decision branch = Σ (P × outcome)` | Decision tree | Ignoring the cost of the option itself | 8.4 |
| `Value of perfect information = EV(with information) − EV(without)` | Whether to buy analysis | — | 8.4 |
| `Exposure after response = Residual probability × Residual impact` | Residual risk | Response cost not netted off | 8.3 |
| `P50 / P80` | Confidence levels from simulation | P80 quoted as if it were a maximum | 8.3 |

> **Ordinal ratings do not multiply.** A 3 × 4 = 12 heat-map score is a sorting device, not a quantity. It cannot be summed across risks, and it cannot size a contingency. Use expected value or simulation for anything that touches money.

---

## 6. Agile, adaptive and flow · Domain 13

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `Velocity = Points completed ÷ Sprint` | Delivery rate | Best-ever used rather than rolling average | 13.3 |
| `Sprints remaining = Points remaining ÷ Average velocity` | Forecast to completion | Backlog growth ignored | 13.3 |
| `Capacity = Team members × Available days × Focus factor` | Sprint capacity | Focus factor set at 1.0 | 13.3 |
| `Cycle time = WIP ÷ Throughput` | Little's Law | Only valid for a stable system over the window measured | 13.4 |
| `Throughput = Items completed ÷ Period` | Delivery rate in items | — | 13.4 |
| `Lead time = Cycle time + Queue time` | Customer-observed duration | Reported as cycle time | 13.4 |
| `Flow efficiency = Active time ÷ Total elapsed time` | Proportion of time actually worked | — | 13.4 |
| `WSJF = Cost of delay ÷ Job size` | Weighted shortest job first | Cost of delay guessed rather than derived | 13.3 |
| `% complete = Points completed ÷ Total planned points` | AgileEVM progress | Point inflation across sprints | 13.5 |
| `Run rate = Team cost ÷ Sprint` | Cost per sprint | — | 13.5 |
| `Burn-up: scope line and completed line plotted together` | Progress against a moving scope | Burn-down hides scope growth; burn-up does not | 13.3 |

> **Why burn-up beats burn-down for governance.** A burn-down chart shows work remaining, so scope added mid-flight looks like slow progress. A burn-up plots completed work and total scope as two lines, which makes scope growth visible as what it is. For any project reporting to a gate, use burn-up.

---

## 7. Procurement and contracts · Domain 10

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `PTA = [(Ceiling price − Target price) ÷ Buyer share ratio] + Target cost` | Point of total assumption (FPIF) | Seller's share ratio used in the denominator | 10.3 |
| `Fee = Target fee + Share ratio × (Target cost − Actual cost)` | Incentive fee | Sign convention on an overrun | 10.3 |
| `Final price = Actual cost + Fee` (capped at ceiling) | FPIF settlement | Ceiling not applied | 10.3 |
| `Pain/gain = Share ratio × (Actual − Target)` | Target-cost mechanism | — | 10.3 |
| `LD exposure = LD rate × Days late` | Liquidated damages | Cap not applied | 10.4 |
| `Make-or-buy: compare total cost of ownership, not price` | Sourcing decision | Transition and exit costs excluded | 10.2 |
| `Weighted score = Σ (criterion weight × score)` | Tender evaluation | Weights set after seeing the bids | 10.2 |

> **What the PTA means.** Above the point of total assumption, the seller absorbs every further dollar of cost — the buyer's contribution has reached the ceiling. It is the point at which the contract's incentive stops working and the seller's interest changes character. **Check:** at an actual cost exactly equal to the PTA, the computed final price equals the ceiling price precisely. If it does not, the share ratio has been applied the wrong way round.

---

## 8. Quality and assurance · Domain 9

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `Cost of quality = Prevention + Appraisal + Internal failure + External failure` | Total quality cost | Only failure costs counted | 9.2 |
| `DPMO = (Defects ÷ (Units × Opportunities)) × 1,000,000` | Defects per million opportunities | Opportunity count defined inconsistently | 9.3 |
| `Defect density = Defects ÷ Size` | Quality rate | — | 9.3 |
| `Rework ratio = Rework effort ÷ Total effort` | Rework burden | — | 9.3 |
| `Control limits = Mean ± 3σ` | Statistical process control | Control limits confused with specification limits | 9.3 |
| `Process capability = (USL − LSL) ÷ 6σ` | Capability index | — | 9.3 |
| `First-pass yield = Units passing first time ÷ Units started` | Right-first-time | — | 9.3 |

> **Control limits are not specification limits.** Control limits describe what the process *does*; specification limits describe what the customer *requires*. A process can be in control and incapable at the same time, and the response to each is different.

---

## 9. Stakeholders, teams and communication · Domains 11, 12

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `Channels = n(n − 1) ÷ 2` | Communication channels among `n` people | Growth is quadratic, and this is the point | 11.2 |
| `Δ Channels = n` when one person joins a team of `n` | Marginal communication load | — | 11.2 |
| `Influence/interest grid position` | Stakeholder strategy | Assessed once and never revisited | 11.1 |
| `RACI: exactly one A per activity` | Accountability rule | More than one A means none | 12.2 |
| `Team productivity ≠ Σ individual productivity` | Coordination overhead | — | 12.3 |

**Worked check.** A team of 9 has 9 × 8 ÷ 2 = **36** channels. Adding one person takes it to 45 — nine new channels for one new person. This is the arithmetic behind small-team preference, and it is why adding people to a late project rarely accelerates it.

---

## 10. Programmes and portfolios · Domain 15

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `Portfolio value = Σ NPV of component projects` | Aggregate value | Interdependencies ignored | 15.2 |
| `Scoring model = Σ (criterion weight × criterion score)` | Prioritisation | Weights adjusted to justify a chosen answer | 15.2 |
| `Capacity constraint: Σ resource demand ≤ Available capacity` | Portfolio feasibility | Demand summed at 100% utilisation | 15.3 |
| `Programme EV = Σ component EV` | Aggregated performance | Components on different measurement bases | 15.4 |
| `Benefit dependency: benefit realised only when all enablers delivered` | Benefits logic | Partial delivery credited with full benefit | 15.4 |

---

## 11. Worked micro-examples

Each is internally consistent and reproducible from this sheet.

**PERT.** `O` = 8, `M` = 12, `P` = 22.

- `tE` = (8 + 48 + 22) ÷ 6 = **13.0**
- `σ` = (22 − 8) ÷ 6 = **2.33** · `σ^2` = **5.44**
- Triangular for comparison = (8 + 12 + 22) ÷ 3 = **14.0** — a different method, a different answer

**Float.** `ES` = 4, duration 6, `LF` = 14.

- `EF` = 10 · `LS` = 8 · `TF` = 8 − 4 = **4** = 14 − 10 — the two forms agree

**Point of total assumption.** Target cost 100,000 · target price 113,000 *(fee 13,000)* · ceiling 120,000 · buyer share 80%.

- `PTA` = [(120,000 − 113,000) ÷ 0.80] + 100,000 = 8,750 + 100,000 = **108,750**
- Check at `AC` = 108,750: overrun 8,750; seller absorbs 20% = 1,750; fee = 13,000 − 1,750 = 11,250; price = 108,750 + 11,250 = **120,000**, exactly the ceiling price

**Earned value.** `PV` 168,000 · `EV` 151,200 · `AC` 170,400 · `BAC` 420,000.

- `CPI` = **0.887** · `SPI` = **0.900**
- `EAC` range = **439,200** (one-off) to **473,300** (CPI persists) to **507,000** (both persist)
- `TCPI` to BAC = (420,000 − 151,200) ÷ (420,000 − 170,400) = **1.08** against a CPI of 0.89 — the budget requires a 22% efficiency improvement on work that has so far run 11% below plan

**Little's Law.** `WIP` = 12 items, throughput = 4 items per week.

- Cycle time = 12 ÷ 4 = **3 weeks**. Halving WIP to 6 halves cycle time to 1.5 weeks at the same throughput.

**Communication channels.** `n` = 9 → **36** channels. `n` = 10 → **45**.

---

## 12. The ten formulas most often misapplied

1. **Risk scores multiplied and summed.** Ordinal heat-map ratings are a sorting device. They cannot size a contingency.
2. **`SV` used as a schedule measure late in a project.** Denominated in currency; converges to zero at completion however late. Use earned schedule.
3. **PTA computed with the seller's share ratio.** The denominator is the **buyer's** share. Check that the price at PTA equals the ceiling.
4. **Velocity taken as best-ever.** Use a rolling average across recent, comparable sprints.
5. **Burn-down used for gate reporting.** It conceals scope growth. Burn-up shows both lines.
6. **Standard deviations summed along a path.** Variances add; take the square root at the end.
7. **Crashing an activity off the critical path.** Cost incurred, no duration saved.
8. **Control limits confused with specification limits.** In control and incapable are different states with different responses.
9. **Resource utilisation targeted at 100%.** Leaves no capacity to absorb variability; queues grow non-linearly as utilisation approaches full.
10. **Benefits claimed without a pre-delivery baseline.** Without the baseline the realisation figure cannot be computed, only asserted.

---

## 13. Cross-references

| For the full treatment | See |
|---|---|
| Every formula worked with figures and exercises | The PML-AI Body of Knowledge, at the domain cited |
| Earned value, earned schedule and forecasting in depth | PCL-AI Formula Sheet, §4 |
| Investment appraisal and cost of capital in depth | PFL-AI Formula Sheet, §3 and §4 |
| Why forecasts fail to change decisions | *Knowing Early*, Knowledge Series 01, §5 |

**Website** — https://projectcontrolsinstitute.org
