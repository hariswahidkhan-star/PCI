# Formula Registry

_Version-controlled registry of every calculation formula the platform teaches, grades or displays.
Content (scenarios, challenges, questions, templates, study sheets) must agree with this registry;
the automated gates listed per family enforce that agreement in CI. Registry version: **1**
(2026-07-25). Changes to a formula's definition or conventions require a version bump here plus a
matching update to the authoritative implementation and its tests._

## Platform-wide conventions

| Concern | Policy | Where enforced |
|---|---|---|
| Internal arithmetic (education engine) | IEEE-754 `double`, full precision until the engine boundary | `backend/Core/SimCalc.cs` |
| Engine boundary rounding | 4 dp, `MidpointRounding.AwayFromZero` (`SimCalc.R`) | `SimCalc.cs` |
| Money (billing/commission paths) | Integer minor units / decimal — never floats | `backend/Core/Money.cs`, `PartnerCommission.cs` |
| Grading tolerance (numbers) | pass iff `|got − want| ≤ max(0.01, tolerance × |want|)`; scenario `tolerance` defaults 0.01 | `SimGrade.CompareAnswer`, `WorldScore` |
| Grading of sets | SimLab: unordered, case-insensitive. PCI World: **ordered**, case-insensitive (see DEF-20) | `SimGrade.cs` / `WorldScore.cs` |
| Grading of booleans | `true/yes/y/1/valid` vs `false/no/n/0/invalid` | `SimGrade.cs` |
| Percentages | Indices (CPI/SPI/factor) as decimals (0.91); `*_pct`/`percent_*` keys as 0–100 | per-key, see tables |
| Division by zero | Guarded per key: ratio → 0 (or null for classic `eac`), never NaN/∞; `answer_nonfinite` blocks publication | `SimCalc.cs`, `SimContent.cs` |
| Partial credit | Per-ask binary; attempt score = `round(100 × correct/total, 2)`; pass at `pass_pct` (default 70) | `SimGrade.cs` |
| Variant reproducibility | Seeded SplitMix64 over declared `[min,max,step]` ranges; attempt pins `(scenario_id, version, seed)` | `SimVariant.cs`, `SimVersion.cs` |

Authoritative engine test: `SimCalcTests.cs` + `SimCalcNextReleaseTests.cs` (expected values
hand-computed from first principles, independent of the engine). Content-level gates:
`SimContentTests`, `SimLabContentSeedTests` (182 scenarios), `WorldTests` (50 challenges),
`QuestionBankTests` (MCQ banks, independent decimal re-derivations).

## EVM & forecasting (`evm`, `timeline`, `earned_schedule` tasks; MCQ banks; formula sheet A3)

| ID | Formula | Definition | Conventions / edge behaviour |
|---|---|---|---|
| EVM-SV | Schedule variance | `SV = EV − PV` | Currency; negative = behind schedule |
| EVM-CV | Cost variance | `CV = EV − AC` | Currency; negative = over budget |
| EVM-SPI | Schedule performance index | `SPI = EV / PV` | Decimal index; `PV=0 → 0` |
| EVM-CPI | Cost performance index | `CPI = EV / AC` | Decimal index; `AC=0 → 0` |
| EVM-PC | Percent complete | `EV / BAC` | Engine returns decimal fraction; `BAC=0 → 0` |
| EVM-PS | Percent spent | `AC / BAC` | Decimal fraction; `BAC=0 → 0` |
| EVM-EAC-CPI | EAC, current-CPI method | `EAC = BAC / CPI` (≡ `AC + (BAC−EV)/CPI`) | Assumes demonstrated cost efficiency continues; classic `eac` key returns null without `bac` or with `CPI=0`; `eac_cpi` key falls back to `AC + (BAC−EV)` when `CPI=0` |
| EVM-EAC-BUD | EAC, budget-rate method | `EAC = AC + (BAC − EV)` | Assumes remaining work at planned rates (variance was one-off) |
| EVM-EAC-CMP | EAC, composite method | `EAC = AC + (BAC−EV)/(CPI×SPI)` | Assumes cost AND schedule performance both persist; `CPI×SPI=0 →` budget-rate fallback |
| EVM-ETC | Estimate to complete | `ETC = EAC − AC` | Null when EAC unresolved |
| EVM-VAC | Variance at completion | `VAC = BAC − EAC` | Negative = forecast overrun |
| EVM-TCPI | To-complete performance index (to BAC) | `TCPI = (BAC−EV)/(BAC−AC)` | `(BAC−AC)=0 → 0`; to-EAC form `(BAC−EV)/(EAC−AC)` appears in the formula sheet only |
| ES-ES | Earned schedule | Interpolate `ES` on the cumulative PV curve at current EV | Fractional periods via linear interpolation; ordered period series required |
| ES-SVT | Schedule variance (time) | `SV(t) = ES − AT` | Time units of the period series |
| ES-SPIT | Schedule performance index (time) | `SPI(t) = ES / AT` | `AT=0 → 0` |
| ES-EACT | Estimated duration | `EAC(t) = PD / SPI(t)` | Null without `planned_duration` or with `SPI(t)=0` |

Timeline (period-series) keys reuse EVM-SPI/CPI/EAC per period; `worst_*_period` ties resolve to
the **first** period; `final_eac` returns 0 (not null) when final CPI = 0.

## Scheduling & estimating (`cpm`, `pert`, `procurement`, `resource` tasks; formula sheet A4)

| ID | Formula | Definition | Conventions / edge behaviour |
|---|---|---|---|
| CPM-PASS | Forward/backward pass | `EF = ES + d`; `LS = LF − d`; project duration = max EF | Continuous-time (day-0) convention; Kahn topological order with declared-order tie-break; cycles/unknown predecessors throw (`answer_error` at validation) |
| CPM-TF | Total float | `TF = LS − ES = LF − EF` | Zero-float (`|TF| < 1e-9`) activities form the critical path |
| CPM-CP | Critical path | Zero-total-float activities in topological order | Graded as a set in SimLab, ordered list in PCI World — see DEF-20 |
| PERT-TE | PERT expected duration | `tE = (O + 4M + P) / 6` per activity, summed along the path | |
| PERT-SD | PERT standard deviation | `σ_act = (P − O)/6`; path `σ = √Σσ²` | Variance sums along the path, not σ |
| PERT-P | Probability of meeting a deadline | `100 × Φ((deadline − tE)/σ)` | Abramowitz–Stegun erf approximation; `σ=0` → 100 if deadline ≥ tE else 0; returned 0–100 |
| PRC-CD | Procurement critical delay | `max(0, supplier_delay − remaining_float)` | Days |
| RES-OVL | Resource overload | per period `max(0, demand − capacity)`; peak/total/count keys | |

## Cost, progress & commercial (`cbs`, `wbs`, `progress`, `productivity`, `boq`, `change`, `cashflow` tasks; formula sheets A2/A5)

| ID | Formula | Definition | Conventions / edge behaviour |
|---|---|---|---|
| CBS-ROLL | Cost breakdown roll-up | Parent = Σ children (budget, actual); `variance = budget − actual` | Positive variance = under budget |
| WBS-100 | 100 percent rule | Every parent's declared value equals its children's roll-up | Engine absolute tolerance 0.01 (independent of grading tolerance) |
| PRG-WTD | Weighted percent complete | `Σ(wᵢ × pᵢ) / Σwᵢ` | Skips nodes missing weight or percent; `Σw=0 → 0`; percent 0–100 |
| PRD-RATE | Productivity | `qty / hours` (planned & actual); `factor = actual/planned` | `hours=0 → 0`; factor > 1 = better than plan |
| BOQ-TOT | Bill of quantities | `Σ(qty × rate)`; `average_rate` = unweighted mean of rates | |
| CHG-BAC | Revised baseline | `baseline + Σ approved deltas` (cost & schedule) | Only status `approved` counts (case-insensitive); pending/rejected excluded |
| CF-CUM | Cumulative cash position | Running `Σ(inflow − outflow)` in period order | `peak_funding` = deepest deficit as a positive number, 0 if never negative |
| OAR | Overhead absorption rate | `budgeted overhead / budgeted activity base`; absorbed = OAR × actual base | MCQ banks / formula sheet only |
| DEP-SL | Straight-line depreciation | `(cost − residual) / useful life` | MCQ banks / formula sheet only |
| POC-CTC | Cost-to-cost percent complete | `costs to date / total estimated costs` | Displayed as % in MCQ banks |
| EV-5050 | 50/50 earning rule | 50% of budget on start, 100% on finish | Public sample bank |

## Risk, decision & analytics (`risk`, `portfolio`, `decision`, `data_quality` tasks; formula sheet A4/A6)

| ID | Formula | Definition | Conventions / edge behaviour |
|---|---|---|---|
| RSK-EMV | Expected monetary value | `EMV = Σ(probabilityᵢ × impactᵢ)` | Signed (threats negative if modelled as such); probability as decimal fraction |
| RSK-P80 | Confidence-level contingency | `contingency = P_target − base estimate` | MCQ banks; requires quantified distribution |
| PF-SCORE | Portfolio weighted score | `w_npv×(npv/max|npv|) + w_fit×fit − w_risk×risk` | Default weights 0.5/0.2/0.3; rank desc, ties by id |
| DEC-SCORE | Decision cost score | `w_cost×|cost| + w_sched×|sched| + w_risk×risk`, best = minimum | Default weights 1 |
| DQ-ANOM | Data anomaly count | rows where `|value − expected| > threshold` | Missing values count against `completeness_pct` (0–100), excluded from `mean_abs_error` |
| MC-DUR | Monte Carlo schedule duration | Triangular(O,M,P) sample per activity → CPM per iteration → percentile by linear rank interpolation | Seeded xorshift64* — reproducible per seed; analysis exhibit, not a graded ask |
| WC-CCC | Cash-conversion cycle | `CCC = DSO + DIO − DPO` | Formula sheet only |

## Registry-to-content traceability

| Content family | Items | Formula authority | CI gate |
|---|---|---|---|
| SimLab scenarios (schema + pack + JSON library) | 213 | `SimCalc` (18 tasks) | `SimContentTests`, `SimLabContentSeedTests`, `backend-unit` job |
| PCI World challenges | 50 | `SimCalc` via `WorldScore` | `WorldTests` |
| Certuvo practice MCQs | 40 | This registry (independent decimal derivations) | `QuestionBankTests` |
| Demo live-exam MCQs | 24 (×3 certs) | This registry | `QuestionBankTests` |
| Public sample questions download | 16 | This registry | `QuestionBankTests` |
| Master formula sheet | ~40 formulas | This registry (manually reconciled 2026-07-25, all correct) | manual review; naming defect DEF-21 open |
| Free templates (CSV) | 15 | N/A — blank learner worksheets, no embedded formulas | `integration_test.py` download checks |

_Historical scoring: every SimLab attempt pins `(scenario_id, scenario_version, seed)` and World
attempts pin an immutable challenge version, so corrections to content produce a NEW version and
never rescore history implicitly (see `SimVersion.cs`, `pciworld_challenge_versions`)._
