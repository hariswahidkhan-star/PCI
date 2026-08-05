# PFL-AI Formula Sheet

> PCI AI Project Finance Leader — the complete quantitative reference

**PCI AI · Project Controls Institute Global, Inc.**
Formula Sheet 02 · First edition

---

## How to use this sheet

Every formula the PFL-AI Body of Knowledge teaches, in one place, with the domain where it is worked in full.

Three columns matter. **Formula** is the expression. **Meaning** is what it answers. **Watch for** is the mistake that formula attracts in practice.

This sheet is a study aid. The examination specification, including whether any formula reference is provided, is confirmed by the Institute for each published examination form.

**Conventions.** Cash flows are period-end unless stated. Rates are per period and must match the period of the cash flows — an annual rate applied to semi-annual flows is the most common error in project finance modelling. Ratios to 2 dp. Currency in USD unless stated.

> **The consistency rule.** Discount rate, cash-flow period and compounding frequency must agree. If debt service is semi-annual, the discount rate is the semi-annual rate, and `n` counts half-years. Most coverage-ratio errors are period-mismatch errors, not formula errors.

---

## 1. Notation

| Symbol | Meaning | Unit |
|---|---|---|
| `CF_t` | Cash flow in period `t` | currency |
| `r`, `n` | Discount rate per period, number of periods | ratio, count |
| `DF` | Discount factor | ratio |
| `AF` | Annuity factor | ratio |
| `NPV`, `IRR` | Net present value, internal rate of return | currency, ratio |
| `MIRR` | Modified internal rate of return | ratio |
| `CFADS` | Cash flow available for debt service | currency |
| `DS` | Debt service — interest plus scheduled principal | currency |
| `DSCR` | Debt service cover ratio | ratio |
| `LLCR`, `PLCR` | Loan life and project life cover ratios | ratio |
| `DSRA`, `MRA` | Debt service and maintenance reserve accounts | currency |
| `D`, `E`, `V` | Debt, equity, total capital (`V = D + E`) | currency |
| `Rd`, `Re` | Cost of debt, cost of equity | ratio |
| `kd` | Discount rate used for cover ratios (typically cost of debt) | ratio |
| `t` | Tax rate (in cost-of-capital contexts) | ratio |
| `WACC` | Weighted average cost of capital | ratio |
| `MOIC` | Multiple on invested capital | ratio |
| `EBITDA` | Earnings before interest, tax, depreciation, amortisation | currency |

---

## 2. Time value of money · Domain 3

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `FV = PV(x) × (1 + r)^n` | Compounding | — | 3.1 |
| `PV(x) = FV ÷ (1 + r)^n` | Discounting | — | 3.1 |
| `DF = 1 ÷ (1 + r)^n` | Discount factor | — | 3.1 |
| `AF = [1 − (1 + r)^-n] ÷ r` | Annuity factor — PV of 1 per period for `n` periods | Applying to a growing stream | 3.2 |
| `PV(perpetuity) = CF ÷ r` | Level perpetuity | — | 3.2 |
| `PV(growing perpetuity) = CF_1 ÷ (r − g)` | Gordon growth | Valid only where `g < r` | 3.2 |
| `Effective annual rate = (1 + r/m)^m − 1` | Nominal to effective, `m` compounds per year | Comparing a nominal to an effective rate | 3.3 |
| `Real rate ≈ [(1 + nominal) ÷ (1 + inflation)] − 1` | Fisher relation | The additive approximation drifts at high inflation | 3.4 |
| `Payment = Principal ÷ AF` | Level annuity instalment | — | 3.2 |

> **Annuity factor check.** `AF` must equal the sum of the individual discount factors. At 8% over 5 periods, `AF` = 3.9927, and the five discount factors sum to 3.9927. If they disagree, the period convention is wrong.

---

## 3. Investment appraisal · Domain 4

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `NPV = Σ CF_t ÷ (1 + r)^t` | Net present value | Including financing flows in an unlevered NPV | 4.1 |
| `IRR: NPV = 0` | Internal rate of return | Multiple IRRs when signs change more than once | 4.1 |
| `MIRR = (FV of inflows at reinvest rate ÷ −PV of outflows at finance rate)^(1/n) − 1` | Modified IRR | Mixing the two rates | 4.1 |
| `PI = PV(inflows) ÷ Initial investment` | Profitability index | — | 4.2 |
| `PI = 1 + NPV ÷ Initial investment` | Equivalent form — use as a check | — | 4.2 |
| `Payback = Years before recovery + (Unrecovered ÷ Cash flow in that year)` | Simple payback | Ignores everything after the payback point | 4.2 |
| `Discounted payback` | As above, on discounted flows | — | 4.2 |
| `Equivalent annual cost = NPV ÷ AF` | Comparing unequal-life assets | — | 4.3 |
| `Switching value = Δ% in a variable that drives NPV to zero` | Sensitivity breakeven | — | 4.4 |

> **IRR pathologies — the three to know.** (1) **Multiple IRRs** where the cash-flow sign changes more than once; the equation has more than one root. (2) **No IRR** where the flows never cross zero. (3) **Scale and reinvestment blindness** — IRR implicitly assumes reinvestment at the IRR itself, which is why MIRR exists and why NPV governs where the two disagree on ranking.

---

## 4. Cost of capital and structure · Domain 9

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `WACC = (E÷V) × Re + (D÷V) × Rd × (1 − t)` | Weighted average cost of capital | Tax shield applied to equity | 9.1 |
| `Re = Rf + β × (Rm − Rf)` | CAPM cost of equity | Equity beta used where asset beta is required | 9.1 |
| `Gearing = D ÷ (D + E)` | Leverage as a proportion of capital | Confused with debt-to-equity | 9.2 |
| `Debt : equity = D ÷ E` | Leverage as a ratio | 60% gearing is 1.5 : 1, not 60 : 40 as a ratio | 9.2 |
| `β_asset = β_equity ÷ [1 + (1 − t) × D÷E]` | Ungearing beta | — | 9.1 |
| `Interest cover = EBIT ÷ Interest` | Income-based cover | EBITDA variant is a different covenant | 9.2 |
| `Leverage = Net debt ÷ EBITDA` | Leverage covenant | Gross vs net debt definition | 10.2 |

---

## 5. Cash flow definitions · Domains 6, 14

**The waterfall, in order.** Each level is paid only after the one above.

| Order | Level |
|---|---|
| 1 | Operating expenses |
| 2 | Tax |
| 3 | Senior debt interest |
| 4 | Senior debt scheduled principal |
| 5 | Reserve account top-ups — DSRA, MRA |
| 6 | Subordinated / mezzanine debt service |
| 7 | Distributions to equity, subject to the lock-up test |

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `CFADS = EBITDA − Tax paid − Δ Working capital − Maintenance capex` | Cash available for debt service | Deducting interest — interest sits *below* CFADS | 6.3 |
| `DS = Interest + Scheduled principal` | Debt service | Excluding fees where the covenant includes them | 10.2 |
| `Cash available to equity = CFADS − DS − Reserve top-ups` | Distributable cash | — | 6.4 |
| `Lock-up: distributions blocked if DSCR < lock-up threshold` | Distribution test | Lock-up threshold differs from default threshold | 10.3 |
| `DSRA target = Next 6 or 12 months of debt service` | Reserve sizing | — | 10.3 |

> **CFADS is pre-financing.** Interest and principal are what CFADS is measured *against*. Deducting interest before computing CFADS double-counts it and inflates every cover ratio. This is the most consequential definitional error in project finance.

---

## 6. Coverage ratios · Domain 10 — the flagship

| Formula | Meaning | Domain |
|---|---|---|
| `DSCR = CFADS ÷ DS` | Cover in a **single period** | 10.2 |
| `LLCR = [PV(CFADS over remaining loan life, at kd) + DSRA] ÷ Debt outstanding` | Cover over the **life of the loan** | 10.2 |
| `PLCR = [PV(CFADS over remaining project life, at kd) + DSRA] ÷ Debt outstanding` | Cover over the **life of the project** | 10.2 |
| `Minimum DSCR` | The lowest DSCR in any period — the binding constraint | 10.2 |
| `Average DSCR` | Mean across periods — informative, never the covenant | 10.2 |

**How the three differ, and why all three exist.**

| Ratio | Window | Answers |
|---|---|---|
| **DSCR** | One period | Can the project service debt *this period*? |
| **LLCR** | Remaining loan term | Is there enough cash over the loan's life to repay it? |
| **PLCR** | Remaining project life | Is there headroom *beyond* loan maturity — the tail? |

> **PLCR is normally greater than LLCR**, because it discounts cash flows over a longer window against the same debt balance. The gap between them is the **tail** — the cash-generating life of the project after the debt matures. A thin tail is a credit concern even when LLCR looks comfortable, because it removes the lender's room to reschedule.

**Debt sizing from a target ratio**

| Formula | Meaning | Domain |
|---|---|---|
| `Debt service capacity_t = CFADS_t ÷ Target DSCR` | Maximum affordable service each period | 10.1 |
| `Debt capacity = Σ [Capacity_t ÷ (1 + kd)^t]` | Sculpted debt quantum | 10.1 |
| `Sculpted repayment: principal_t = Capacity_t − Interest_t` | Repayment shaped to cash flow | 10.1 |

**Sculpting versus annuity.** A sculpted profile sets debt service as a constant multiple of CFADS, so DSCR is flat at the target in every period. An annuity profile sets a constant instalment, so DSCR varies with CFADS. Sculpting maximises debt capacity for a given minimum DSCR — which is why project finance uses it and corporate lending largely does not.

| Formula | Meaning | Domain |
|---|---|---|
| `Average debt life = Σ (Principal_t × t) ÷ Σ Principal_t` | Weighted average life | 10.1 |
| `Tail = Project life − Loan life` | Headroom beyond maturity | 10.2 |
| `Gearing achieved = Debt capacity ÷ Total project cost` | Resulting leverage | 10.1 |

---

## 7. Equity returns · Domains 6, 15

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `Equity IRR: NPV of equity cash flows = 0` | Return to shareholders | Computed on project rather than equity flows | 6.4 |
| `Project IRR` | Return on total project cost, pre-financing | — | 4.1 |
| `MOIC = Total distributions ÷ Total equity invested` | Money multiple | Ignores timing entirely | 15.2 |
| `Equity IRR > Project IRR when Project IRR > cost of debt` | The leverage effect | Leverage magnifies losses symmetrically | 9.2 |
| `Payback to equity` | Period to recover equity injections | — | 15.2 |

> **Two IRRs, two questions.** Project IRR asks whether the *asset* is worth building. Equity IRR asks whether the *deal* is worth doing. A strong equity IRR on a weak project IRR is a statement about leverage, not about the asset.

---

## 8. Construction phase · Domain 14

| Formula | Meaning | Watch for | Domain |
|---|---|---|---|
| `Drawdown_t = Cost incurred_t × Debt proportion` | Pro-rata debt draw | Equity-first structures draw differently | 14.1 |
| `IDC = Σ Interest on drawn balance during construction` | Interest during construction | Omitted from project cost | 14.2 |
| `Total project cost = Construction + IDC + Fees + Working capital + Reserves` | Funding requirement | IDC and fees excluded | 14.2 |
| `Cost to complete = Total budget − Costs incurred` | Remaining requirement | Not the same as remaining committed | 14.3 |
| `Funding adequacy: Available funds ≥ Cost to complete` | The lender's construction test | — | 14.3 |
| `EAC (see PCL-AI sheet §4.2)` | Forecast construction outturn | The controls and finance forecasts must reconcile | 8.2 |

> **The bridge to project controls.** A lender's cost-to-complete test and a project controls EAC answer the same question from two sides. If the controls function forecasts an outturn above the funded amount, the funding adequacy test has already failed — usually before anyone in the finance team has been told.

---

## 9. Risk and sensitivity · Domain 11

| Formula | Meaning | Domain |
|---|---|---|
| `EMV = Probability × Impact` | Expected monetary value | 11.2 |
| `Switching value = Δ% to drive NPV or minimum DSCR to its limit` | Breakeven sensitivity | 11.3 |
| `Sensitivity = Δ Output ÷ Δ Input` | Elasticity of the metric to a driver | 11.3 |
| `Downside case: minimum DSCR ≥ 1.00` | Base survival test | 11.3 |
| `Breakeven tariff / volume` | Price or throughput at which DSCR hits the covenant | 7.2 |

**The standard sensitivity set.** Capex overrun, delay to commercial operation, revenue or tariff reduction, opex increase, interest-rate movement, and availability or throughput shortfall — each run singly, then in a combined downside case.

---

## 10. Worked micro-examples

Each is internally consistent and reproducible from this sheet.

**Appraisal.** Cash flows −1,000 · 300 · 400 · 500 · 400, discount rate 10%.

- `NPV` = **252.17**
- `IRR` = **20.50%** *(NPV at that rate is zero)*
- `MIRR` at 8% finance / 10% reinvestment = **16.36%**
- `PI` = 1,252.17 ÷ 1,000 = **1.252** = 1 + 252.17 ÷ 1,000 — the two forms agree
- Payback = 2 + 300 ÷ 500 = **2.60 years**

**Discount and annuity factors** at 8% over 5 periods: `DF` = **0.6806** · `AF` = **3.9927**

**DSCR.** CFADS 42,000,000 · principal 18,000,000 · interest 9,500,000.

- `DS` = 27,500,000 → `DSCR` = 42,000,000 ÷ 27,500,000 = **1.53**

**LLCR and PLCR.** Debt outstanding 210,000,000 · DSRA 12,000,000 · `kd` 7% · CFADS 38,000,000 for 8 remaining loan years, then 36,000,000 for a further 7 project years.

- PV of CFADS over loan life = 226,909,000 → `LLCR` = (226,909,000 + 12,000,000) ÷ 210,000,000 = **1.14**
- PV of CFADS over project life = 339,828,000 → `PLCR` = (339,828,000 + 12,000,000) ÷ 210,000,000 = **1.68**
- The gap is the seven-year tail. `PLCR` > `LLCR`, as it should be.

**WACC.** `E` 400,000,000 · `D` 600,000,000 · `Re` 13.5% · `Rd` 7.0% · tax 25%.

- Gearing = 600 ÷ 1,000 = **60%** *(debt : equity = 1.5 : 1)*
- `WACC` = 0.40 × 13.5% + 0.60 × 7.0% × 0.75 = 5.40% + 3.15% = **8.55%**

**Debt sizing.** Target DSCR 1.35 · `kd` 7% · CFADS 40.0, 42.0, 44.0, 43.0, 41.0 million over five years.

- Capacity each year = CFADS ÷ 1.35 → 29.63, 31.11, 32.59, 31.85, 30.37 million
- Debt capacity = PV of that stream at 7% = **127,423,000**
- Check: servicing exactly that capacity returns a DSCR of **1.35 in every year** — the definition of a sculpted profile.

---

## 11. The ten formulas most often misapplied

1. **Interest deducted before CFADS.** CFADS is pre-financing. Deducting interest double-counts it and inflates every cover ratio.
2. **Period mismatch.** An annual rate applied to semi-annual debt service. Rate, period and compounding must agree.
3. **LLCR and PLCR treated as interchangeable.** They discount over different windows against the same debt. The gap is the tail, and the tail is a credit question.
4. **Average DSCR quoted as the covenant.** The **minimum** DSCR is the binding constraint. An average conceals the one period that breaches.
5. **IRR ranked above NPV on mutually exclusive projects.** IRR assumes reinvestment at itself and is blind to scale. NPV governs.
6. **Multiple IRRs unnoticed.** More than one sign change means more than one root. Check the sign pattern before quoting an IRR.
7. **Gearing confused with debt-to-equity.** 60% gearing is 1.5 : 1.
8. **Tax shield applied to the equity term of WACC.** `(1 − t)` attaches to the cost of debt only.
9. **IDC and fees omitted from total project cost.** The funding requirement is larger than the construction contract, often materially.
10. **Equity IRR presented as project performance.** It is a statement about leverage as much as about the asset. Quote both.

---

## 12. Cross-references

| For the full treatment | See |
|---|---|
| Every formula worked with figures and exercises | The PFL-AI Body of Knowledge, at the domain cited |
| Coverage ratios, sculpting and covenants in depth | PFL-AI Domain 10 — the quantitative flagship |
| The construction-phase bridge to project controls | PFL-AI Domain 8 and Domain 14 |
| Cost forecasting that feeds the funding adequacy test | PCL-AI Formula Sheet, §4.2 |

**Website** — https://projectcontrolsinstitute.org
