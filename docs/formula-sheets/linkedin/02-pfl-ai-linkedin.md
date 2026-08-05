# The Project Finance Formula Sheet

> Every formula that decides whether a project is bankable

## What this is

**PFL-AI · COMPLETE REFERENCE**

Forty-one formulas across project and infrastructure finance: time value, appraisal, cost of capital, the cash waterfall, coverage ratios, debt sizing and sculpting, equity returns and construction drawdown.

Every one is drawn from the PFL-AI Body of Knowledge and carries the domain where it is worked in full.

Against each, the error it attracts — because in project finance most wrong answers come from a definition or a period convention, not from the expression.

> **Save this one.** It is the reference, not a highlight reel.

## The rule that catches most model errors

**CONVENTIONS**

Rates, periods and compounding must agree.

An annual rate applied to semi-annual debt service is the single error the verification suite catches most often across the whole programme.

> **And arithmetic is decimal, never binary floating point.** Intermediate values carry full precision; rounding happens at display only — which is why a printed figure will not reproduce if you round as you go.

# 01 | Time value of money

Domain 3. The machinery every other number in this deck is built on.

## Discounting and compounding

**DOMAIN 3 · CORE**

| Formula | Meaning | Domain |
|---|---|---|
| `FV(x) = PV(x) × (1 + r)^n` | Compounding | 3.1 |
| `PV(x) = FV(x) ÷ (1 + r)^n` | Discounting | 3.1 |
| `DF(t) = 1 ÷ (1 + r)^t` | Discount factor at period `t` | 3.1 |
| `AF(r, n) = [1 − (1 + r)^-n] ÷ r` | Annuity factor | 3.2 |
| `A = Principal ÷ AF(r, n)` | Annuity payment | 3.2 |

> **A free check.** The annuity factor must equal the sum of the individual discount factors over the same periods. If the two disagree, your period convention is wrong before anything else is.

## Perpetuities and real rates

**DOMAIN 3 · EXTENSIONS**

| Formula | Meaning | Domain |
|---|---|---|
| `PV(perpetuity) = CF ÷ r` | Level perpetuity | 3.2 |
| `PV(growing perpetuity) = CF_1 ÷ (r − g)` | Gordon growth; valid only while `g < r` | 3.2 |
| `1 + i_nom = (1 + i_real) × (1 + π)` | Fisher relation | 3.4 |
| `Effective annual rate = (1 + r/m)^m − 1` | `m` compounds per year | 3.3 |

> **Fisher is multiplicative, not additive.** Subtracting inflation from the nominal rate is close enough at 2% and materially wrong at 20% — which is precisely where the projects that need it are.

# 02 | Investment appraisal

Domain 4. Whether the asset is worth building at all.

## The decision measures

**DOMAIN 4 · APPRAISAL**

| Formula | Meaning | Domain |
|---|---|---|
| `NPV = Σ CF_t ÷ (1 + r)^t − I_0` | Net present value | 4.1 |
| `IRR: NPV = 0` | Internal rate of return | 4.1 |
| `MIRR` | Modified IRR — explicit reinvestment rate | 4.1 |
| `PI = PV(future CF) ÷ I_0` | Profitability index | 4.2 |
| `EAV = NPV ÷ AF(r, n)` | Equivalent annual value — unequal lives | 4.3 |
| `Payback` | Period to recover the outlay | 4.2 |

> **`PI` also equals `1 + NPV ÷ I_0`.** Compute it both ways as a check; if the two disagree, a cash-flow sign is wrong.

## Three ways IRR misleads

**DOMAIN 4 · IRR PATHOLOGIES**

**Multiple roots.** More than one sign change in the cash flows means more than one IRR. Check the sign pattern before quoting one.

**No solution.** If the flows never cross zero there is nothing to solve for.

**Scale and reinvestment blindness.** IRR implicitly assumes reinvestment at itself — which is exactly why `MIRR` exists.

> **On mutually exclusive projects, NPV governs.** The higher IRR is regularly the smaller prize, and IRR cannot see that.

# 03 | Cost of capital

Domain 9. The rate everything above is discounted at.

## Capital structure

**DOMAIN 9 · WACC AND GEARING**

| Formula | Meaning | Domain |
|---|---|---|
| `WACC = g × k_d × (1 − T) + (1 − g) × k_e` | Cost of capital, in gearing form | 9.1 |
| `g = D ÷ (D + E)` | Gearing | 9.2 |
| `D/E` | Debt to equity | 9.2 |
| `k_e = R_f + β × (R_m − R_f)` | CAPM cost of equity | 9.1 |
| `β_asset = β_equity ÷ [1 + (1 − T) × D/E]` | Ungearing beta | 9.1 |

> **Two cautions.** `(1 − T)` attaches to the cost of **debt** only — the tax shield never touches the equity term. And WACC is linear in gearing only while `k_e` is held fixed; in reality `k_e` rises with leverage, which is what the ungearing formula exists to handle.

# 04 | Cash flow and the waterfall

Domains 2, 6 and 14. What the ratios are actually measured on.

## The order the money moves

**DOMAINS 6, 14 · WATERFALL**

| | Paid in this order |
|---|---|
| 1 | Operating expenses |
| 2 | Tax |
| 3 | Senior debt interest |
| 4 | Senior debt scheduled principal |
| 5 | Reserve top-ups — DSRA, MRA |
| 6 | Subordinated debt service |
| 7 | Distributions to equity, subject to lock-up |

Each level is paid only after the one above it is satisfied.

## The most consequential definition in the model

**DOMAIN 2 · CFADS**

`CFADS = EBITDA − Tax paid − Δ Working capital − Maintenance capex`

Note what is **not** deducted: interest. Interest and principal are what CFADS is measured *against*.

> **CFADS is a defined term, not a standard one.** Whether it is struck before or after working-capital movements changes every ratio built on it — so the definition belongs in the term sheet, agreed, not assumed. Deduct interest above the line and you double-count it, inflating every cover ratio in the direction that makes a deal look financeable.

## Reserves and triggers

**DOMAIN 10 · STRUCTURE**

| Formula | Meaning | Domain |
|---|---|---|
| `DS = Interest + Scheduled principal` | Debt service | 10.2 |
| `DSRA = Debt service × months ÷ 12` | Reserve, expressed as the shortfall it survives | 10.3 |
| `Lock-up trigger in cash = Debt service × threshold ratio` | The number that belongs on a dashboard | 10.3 |
| `Cash to equity = CFADS − DS − Reserve top-ups` | Distributable cash | 6.4 |

> **Put the covenant on the dashboard in cash, not as a ratio.** "DSCR must exceed 1.20" is an abstraction. "We must generate 33m this period" is a number an operations team can actually act on.

# 05 | Coverage ratios

Domain 10 — the quantitative flagship. The three questions every lender asks.

## Cover, in one period and over a lifetime

**DOMAIN 10 · THE THREE RATIOS**

| Formula | Meaning | Domain |
|---|---|---|
| `DSCR = CFADS ÷ (Interest + Scheduled principal)` | Cover in a single period | 10.2 |
| `LLCR = PV(CFADS over loan life, at the loan rate) ÷ Outstanding debt` | Cover over the life of the loan | 10.2 |
| `PLCR = PV(CFADS over project life) ÷ Outstanding debt` | Cover over the life of the project | 10.2 |
| `ICR = EBIT (or EBITDA) ÷ Interest` | Accounting cover; ignores principal entirely | 10.2 |

> **The minimum DSCR is the covenant. The average never is.** A comfortable average conceals the single period that breaches — and the single period is what triggers lock-up.

## Why all three exist

**THE TAIL**

`Tail = Project life − Loan life`

`PLCR` exceeds `LLCR` wherever a tail exists, because it discounts a longer stream of cash against the same debt balance.

> **The gap between the two ratios is the tail** — the project's cash-generating life after the debt matures, and therefore the lender's room to reschedule rather than enforce. A thin tail is a credit concern even when LLCR looks comfortable.

## Sizing the debt backwards

**DOMAIN 10 · DEBT CAPACITY**

| Formula | Meaning | Domain |
|---|---|---|
| `Max debt service = CFADS ÷ Target DSCR` | Affordable service per period | 10.1 |
| `Max debt capacity = Max debt service × AF(r, n)` | Quantum at the loan rate and tenor | 10.1 |
| `Sculpted debt service_t = CFADS_t ÷ Target DSCR` | Coverage constant by construction | 10.1 |
| `Average debt life = Σ (Principal_t × t) ÷ Σ Principal_t` | Weighted average life | 10.1 |

> **Check it by reversing.** Service exactly the sized capacity and the DSCR must land on the target in every period. If it does not, the capacity and the discounting are on different period conventions.

## Sculpted or annuity

**REPAYMENT SHAPE**

**Sculpted** sets debt service as a constant multiple of CFADS, so DSCR is flat at the target in every period.

**Annuity** sets a constant instalment, so DSCR rides up and down as CFADS moves.

> **Sculpting maximises debt capacity for a given minimum DSCR.** That is why limited-recourse project finance uses it and corporate lending largely does not — and why, on a sculpted profile, the minimum DSCR is the only ratio worth arguing about.

# 06 | Returns and construction

Domains 14 and 15. What equity earns, and what the build actually costs to fund.

## Two IRRs, two questions

**DOMAINS 6, 15 · RETURNS**

| Formula | Meaning | Domain |
|---|---|---|
| `Project IRR` | Return on total project cost, pre-financing | 4.1 |
| `Equity IRR` | Return on equity cash flows only | 6.4 |
| `MOIC = Total distributions ÷ Total equity invested` | Money multiple; ignores timing entirely | 15.2 |

> **Project IRR asks whether the asset is worth building. Equity IRR asks whether the deal is worth doing.** A strong equity IRR on a weak project IRR is a statement about leverage, not about the asset — so quote both, or you have quoted neither.

## What the build really costs to fund

**DOMAIN 14 · CONSTRUCTION**

| Formula | Meaning | Domain |
|---|---|---|
| `IDC = Σ Interest on the drawn balance during construction` | Interest during construction | 14.2 |
| `Total project cost = Construction + IDC + Fees + Working capital + Reserves` | The funding requirement | 14.2 |
| `Drawdown_t = Cost incurred_t × Debt proportion` | Pro-rata draw | 14.1 |
| `Cost to complete = Total budget − Costs incurred` | Remaining requirement | 14.3 |
| `Funding adequacy: Available funds ≥ Cost to complete` | The lender's construction test | 14.3 |

> **The bridge to project controls.** A lender's cost-to-complete test and a controls team's `EAC` answer the same question from two sides. If controls forecasts an outturn above the funded amount, funding adequacy has already failed — usually before anyone in the finance team has been told.

## Sensitivity and the downside case

**DOMAIN 11 · RISK**

| Formula | Meaning | Domain |
|---|---|---|
| `EMV = Probability × Impact` | Expected monetary value | 11.2 |
| `Switching value = Δ% that drives NPV or minimum DSCR to its limit` | Breakeven sensitivity | 11.3 |
| `Breakeven tariff / volume` | Price or throughput at the covenant | 7.2 |

> **The standard set:** capex overrun, delay to commercial operation, revenue or tariff reduction, opex increase, interest-rate movement, availability shortfall. Each run singly, then combined into one downside case — because in reality they correlate and never arrive alone.

## The ten that cost the most

**MOST MISAPPLIED · 1–5**

1. Interest deducted before `CFADS` — every ratio inflated
2. Annual rate applied to semi-annual debt service
3. `CFADS` struck on an undefined working-capital basis
4. `LLCR` and `PLCR` treated as interchangeable
5. Average DSCR quoted as the covenant

## And the other five

**MOST MISAPPLIED · 6–10**

6. `IRR` ranked above `NPV` on mutually exclusive projects
7. Multiple IRRs unnoticed — more than one sign change, more than one root
8. Tax shield applied to the equity term of `WACC`
9. `IDC` and fees omitted from total project cost
10. Equity IRR presented as project performance

> **Each of these survives model review**, because the spreadsheet computes cleanly and the output looks entirely plausible.

## The Body of Knowledge is open

**PCI AI · PROJECT CONTROLS INSTITUTE GLOBAL**

Every formula here is developed to full depth in the PFL-AI Body of Knowledge — sixteen domains with worked examples, master-model threads and calculation exercises, at the domain cited on each slide. Domain 10 carries coverage ratios, sculpting and covenants as the quantitative flagship.

Published openly. **No registration, no email gate.**

**projectcontrolsinstitute.org**
