# Project Finance Formulas

> The 16 that decide whether a deal is bankable

## Most coverage-ratio errors are not formula errors

**PFL-AI · HOW TO READ THIS**

They are definition errors and period errors.

The expression is right. The cash flow feeding it was built on the wrong basis, or discounted at a rate that does not match its own period.

This deck gives the formula, and the mistake it attracts.

> **The consistency rule.** Discount rate, cash-flow period and compounding frequency must agree. If debt service is semi-annual, the rate is semi-annual and `n` counts half-years.

## The three that price everything

**TIME VALUE · DOMAIN 3**

`DF = 1 ÷ (1 + r)^n`

`AF = [1 − (1 + r)^-n] ÷ r`

`PV(x) = FV ÷ (1 + r)^n`

> **A free check.** The annuity factor must equal the sum of the individual discount factors. At 8% over 5 periods, `AF` = 3.9927 — and the five discount factors sum to 3.9927. If they disagree, your period convention is wrong.

## Appraisal, and its traps

**INVESTMENT APPRAISAL · DOMAIN 4**

`NPV = Σ CF_t ÷ (1 + r)^t`

`IRR: NPV = 0`

`PI = PV(inflows) ÷ Initial investment`

`PI = 1 + NPV ÷ Initial investment`

> **The last two are the same thing.** Compute both. If they disagree, something in the cash-flow signs is wrong.

## Three ways IRR misleads you

**IRR PATHOLOGIES**

**Multiple IRRs.** More than one sign change in the cash flows means more than one root. Check the sign pattern before quoting a number.

**No IRR at all.** If the flows never cross zero, there is nothing to solve.

**Reinvestment blindness.** IRR implicitly assumes you reinvest at the IRR itself.

`MIRR = (FV inflows at reinvest rate ÷ −PV outflows at finance rate)^(1/n) − 1`

> **On mutually exclusive projects, NPV governs.** IRR is blind to scale, and the highest IRR is regularly the smaller prize.

## Gearing is not debt-to-equity

**CAPITAL STRUCTURE · DOMAIN 9**

`WACC = (E÷V) × Re + (D÷V) × Rd × (1 − t)`

`Gearing = D ÷ (D + E)`

`Debt : equity = D ÷ E`

> **60% gearing is 1.5 : 1.** Not 60 : 40 as a ratio. And `(1 − t)` attaches to the cost of **debt** only — the tax shield never touches the equity term.

## The order the money moves

**THE WATERFALL · DOMAIN 6**

| | Paid in this order |
|---|---|
| 1 | Operating expenses |
| 2 | Tax |
| 3 | Senior debt interest |
| 4 | Senior debt principal |
| 5 | Reserve top-ups — DSRA, MRA |
| 6 | Subordinated debt |
| 7 | Distributions to equity |

Each level is paid only after the one above it.

## The most consequential error in project finance

**CFADS IS PRE-FINANCING**

`CFADS = EBITDA − Tax paid − Δ Working capital − Maintenance capex`

Notice what is **not** deducted. Interest.

Interest and principal are what CFADS is measured *against*.

> **Deduct interest before computing CFADS and you have double-counted it** — inflating every single cover ratio in the model, in the direction that makes the deal look financeable.

## Cover, in one period

**DSCR · DOMAIN 10**

`DSCR = CFADS ÷ DS`

`DS = Interest + Scheduled principal`

Worked: CFADS 42.0m, principal 18.0m, interest 9.5m → `DSCR` = 42.0 ÷ 27.5 = **1.53**

> **The minimum DSCR is the covenant.** The average is informative and is never the test. An average of 1.45 conceals the one period at 0.98 that breaches.

## Cover, over a lifetime

**LLCR AND PLCR**

`LLCR = [PV(CFADS over remaining loan life) + DSRA] ÷ Debt outstanding`

`PLCR = [PV(CFADS over remaining project life) + DSRA] ÷ Debt outstanding`

Same debt balance. Different windows.

## And the gap between them has a name

**THE TAIL**

Worked: debt outstanding 210m, DSRA 12m, `kd` 7%. CFADS of 38m for 8 remaining loan years, then 36m for a further 7 project years.

`LLCR` = **1.14**

`PLCR` = **1.68**

> **PLCR should always exceed LLCR** where a tail exists. The gap is the project's cash-generating life *after* the debt matures — the lender's room to reschedule. A thin tail is a credit concern even when LLCR looks comfortable.

## Sizing the debt backwards

**DEBT SIZING · DOMAIN 10**

`Debt service capacity_t = CFADS_t ÷ Target DSCR`

`Debt capacity = Σ [Capacity_t ÷ (1 + kd)^t]`

Worked: target DSCR 1.35, `kd` 7%, CFADS of 40, 42, 44, 43, 41m → debt capacity **127.4m**

> **Check it by reversing.** Service exactly that capacity and the DSCR lands on 1.35 in every single period. That is the definition of a sculpted profile.

## Sculpted or annuity

**REPAYMENT SHAPE**

`Sculpted: principal_t = Capacity_t − Interest_t`

**Sculpted** sets debt service as a constant multiple of CFADS, so DSCR is flat at the target every period.

**Annuity** sets a constant instalment, so DSCR rides up and down with CFADS.

> **Sculpting maximises debt capacity for a given minimum DSCR.** That is why project finance uses it and corporate lending largely does not.

## Two IRRs, two different questions

**RETURNS**

`Project IRR` — is the **asset** worth building?

`Equity IRR` — is the **deal** worth doing?

`MOIC = Total distributions ÷ Total equity invested`

> **A strong equity IRR on a weak project IRR is a statement about leverage**, not about the asset. Quote both, or you are quoting neither.

## The funding requirement is bigger than the contract

**CONSTRUCTION · DOMAIN 14**

`IDC = Σ Interest on the drawn balance during construction`

`Total project cost = Construction + IDC + Fees + Working capital + Reserves`

> **The bridge to project controls.** A lender's cost-to-complete test and a controls team's EAC answer the same question from two sides. If controls forecasts an outturn above the funded amount, funding adequacy has already failed — usually before anyone in finance has been told.

## The ten that cost the most

**MOST MISAPPLIED · 1–5**

1. Interest deducted before `CFADS` — every ratio inflated
2. Annual rate applied to semi-annual debt service
3. `LLCR` and `PLCR` treated as interchangeable
4. Average DSCR quoted as the covenant
5. `IRR` ranked above `NPV` on mutually exclusive projects

## And the other five

**MOST MISAPPLIED · 6–10**

6. Multiple IRRs unnoticed — more than one sign change, more than one root
7. Gearing confused with debt-to-equity
8. Tax shield applied to the equity term of `WACC`
9. `IDC` and fees omitted from total project cost
10. Equity IRR presented as project performance

> **Each of these survives model review**, because the spreadsheet computes cleanly and the output looks plausible.

## The full sheet is free

**PCI AI · PROJECT CONTROLS INSTITUTE GLOBAL**

This deck is 16 of them. The complete reference carries the whole quantitative surface of project finance — time value, appraisal, cost of capital, the cash waterfall, coverage ratios, debt sizing and sculpting, equity returns, construction drawdown and sensitivity — each with the mistake it attracts and reproducible worked examples.

No registration. No email gate.

**projectcontrolsinstitute.org**
