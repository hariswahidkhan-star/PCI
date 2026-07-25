# Shared Formula & Symbol Registry — PML-AI · PFL-AI (binds to the PCP-AI master table)

**Rule:** every symbol means the same thing in every chapter of both books (and matches PCP-AI where the
symbol exists there). A chapter restates a symbol's definition inline when it uses it, but never changes
it. Every formula entered here must carry at least one independently verified **golden example**
(inputs → full-precision result → display rounding) before any chapter that uses it can pass gate.
Golden examples are automated as checks in `_build/verify_formulas.py`.

Status legend: ✅ verified golden example exists · ⏳ registered, verification pending.

## 1. Symbols inherited from the PCP-AI master table (unchanged)

| Symbol | Meaning | Unit | Status |
|---|---|---|---|
| `PV` (BCWS) | Planned Value (EVM context) | currency | ✅ (PCP-AI D6) |
| `EV` (BCWP) | Earned Value | currency | ✅ (PCP-AI D6) |
| `AC` (ACWP) | Actual Cost | currency | ✅ (PCP-AI D6) |
| `BAC` / `EAC` / `ETC` / `VAC` | Budget / Estimate at Completion, Estimate to Complete, Variance at Completion | currency | ✅ (PCP-AI D6) |
| `CV` = `EV − AC` · `SV` = `EV − PV` | Cost / Schedule Variance | currency | ✅ (PCP-AI D6) |
| `CPI` = `EV/AC` · `SPI` = `EV/PV` | Performance indices | ratio | ✅ (PCP-AI D6) |
| `TCPI` | To-Complete Performance Index | ratio | ✅ (PCP-AI D6) |
| `PoC` | Percentage of completion | % | ✅ (PCP-AI D2/D7) |
| `r`, `n` | Discount rate per period; number of periods | ratio; count | ✅ (PCP-AI D3) |
| `PV(x)` | Present value of amount `x` | currency | ✅ (PCP-AI D3) |

> **Notation clash rule (inherited).** `PV` = Planned Value in EVM contexts; discounting always writes
> "present value" in words or `PV(x)`. PFL-AI is discounting-heavy: its chapters use `PV(x)`/`FV(x)`
> forms throughout and reserve bare `PV` for EVM material only.

## 2. New symbols — PML-AI

| Symbol | Meaning | Unit | First home | Status |
|---|---|---|---|---|
| `ES` | Earned Schedule (time-based schedule measure) | time | PML-AI D6 | ✅ |
| `SPI(t)` | Schedule Performance Index (time) = `ES/AT` | ratio | PML-AI D6 | ✅ |
| `TF`, `FF` | Total float, free float | time | PML-AI D6 | ✅ |
| `EMV` | Expected monetary value = Σ(probability × impact) | currency | PML-AI D8 | ✅ |
| `EVA(benefit)` | Benefit measure in benefits register (named in words to avoid EV clash) | currency | PML-AI D2/D16 | ⏳ |

## 3. New symbols — PFL-AI

| Symbol | Meaning | Unit | First home | Status |
|---|---|---|---|---|
| `FV(x)` | Future value of amount `x` | currency | PFL-AI D3 | ✅ |
| `NPV` | Net present value = Σ CFₜ/(1+r)ᵗ − I₀ | currency | PFL-AI D4 | ✅ |
| `IRR` / `MIRR` | Internal / modified internal rate of return | %/period | PFL-AI D4 | ✅ |
| `PI` | Profitability index = PV(future CF)/I₀ | ratio | PFL-AI D4 | ✅ |
| `EAV` | Equivalent annual value | currency/period | PFL-AI D4 | ✅ |
| `WACC` | Weighted average cost of capital | %/period | PFL-AI D4/D9 | ⏳ |
| `CFADS` | Cash flow available for debt service (a **defined term** — see PFL-AI D2 KA 2.3.1: whether it is struck before or after working-capital movements changes every ratio built on it) | currency/period | PFL-AI D2/D10 | ✅ |
| `DSCR` | Debt service coverage ratio = CFADS/(interest + scheduled principal) per period | ratio | PFL-AI D10 | ⏳ |
| `LLCR` | Loan life coverage ratio = PV(CFADS over loan life, at loan rate)/outstanding debt | ratio | PFL-AI D10 | ⏳ |
| `PLCR` | Project life coverage ratio = PV(CFADS over project life)/outstanding debt | ratio | PFL-AI D10 | ⏳ |
| `ICR` | Interest coverage ratio | ratio | PFL-AI D10 | ⏳ |
| `D/E` | Gearing (debt : equity) | ratio | PFL-AI D9 | ⏳ |
| `i_nom`, `i_real`, `π` | Nominal rate, real rate, inflation (Fisher: `1+i_nom = (1+i_real)(1+π)`) | %/period | PFL-AI D3 | ✅ |
| `DF(t)` | Discount factor at period `t` = 1/(1+r)ᵗ | ratio | PFL-AI D3 | ✅ |
| `A` | Annuity payment (context-flagged; PCP-AI uses `A` = Assets in accounting chapters — write "annuity payment `A`" at first use) | currency/period | PFL-AI D3 | ✅ |

## 4. Verification protocol

- Decimal arithmetic for all financial calculations (no binary-float artefacts in printed results);
  full-precision intermediate values; rounding only at display, per the Style Spine conventions.
- Independent verification required (two computations, different implementers/methods) for: CPM and
  float; EVM; forecasts; probability and contingency; discounted cash flow; NPV/IRR/MIRR; debt service;
  DSCR/LLCR/PLCR; loan amortisation; cash waterfall; escalation; currency conversion;
  percentage-of-completion.
- Every worked example in either book cites its registry row; a formula not in this registry cannot
  appear in a chapter.
