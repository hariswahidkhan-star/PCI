# Project Controls Formulas

> The 16 that decide whether your forecast is defensible

## The formula is never the hard part

**PCL-AI · HOW TO READ THIS**

Every controls professional can write `CPI = EV ÷ AC`.

Far fewer can say what makes that number wrong, which is almost always the input rather than the expression.

This deck gives the formula, and the mistake it attracts.

> **The rule underneath all of it.** A correct formula applied to a compromised input produces a confident, well-formatted, entirely wrong answer. Those are the expensive ones.

## Two symbols. One page. Constant confusion.

**NOTATION**

`PV` means **Planned Value** in earned value.

`PV` means **present value** in discounting.

Both appear in the same cost report every month.

> **Write it out.** Use "present value" in words, or `PV(x)`, whenever you are discounting. This pair collides more often than any other in the discipline.

## Where you are

**POSITION · DOMAIN 6**

`CV = EV − AC`

`SV = EV − PV`

`CPI = EV ÷ AC`

`SPI = EV ÷ PV`

Negative variance is adverse. Index below 1.00 is adverse.

## The number that breaks all four

**THE ACCRUAL RULE**

`Cost to date = Actuals + Accruals`

Every index above divides by `AC`. If `AC` is invoiced cost only, work has been performed that nobody has charged you for yet.

`CPI` overstates performance. `EAC` understates cost. Both errors are invisible in the report.

> **Before you compute anything:** reconcile goods-received-not-invoiced against the accrual booked. A persistent gap is not a timing quirk. It is a wrong forecast.

## What one missed accrual does

**WORKED · 420M PROGRAMME, MONTH 14**

Invoiced cost only → `CPI` 0.957 → `EAC` **438.9m**

Accrual booked, +12.4m → `CPI` 0.887 → `EAC` **473.3m**

> **The 34.4m swing exceeded the entire 21m contingency.** The board had been told the project was uncomfortable but contained. It was neither — and the forecasting was never wrong. The input was.

## Three answers. All correct.

**FORECAST · THE EAC FAMILY**

`EAC = AC + (BAC − EV)`

`EAC = BAC ÷ CPI`

`EAC = AC + (BAC − EV) ÷ (CPI × SPI)`

Same data. Three defensible numbers. The arithmetic is trivial; the assumption is the professional judgement.

## So which one

**DECISION AID**

| If | Then |
|---|---|
| The variance was a closed, one-off event | `AC + (BAC − EV)` |
| Performance to date will persist | `BAC ÷ CPI` |
| Cost **and** schedule pressure both persist | `AC + (BAC − EV) ÷ (CPI × SPI)` |
| Remaining scope differs materially | Bottom-up ETC — indices do not apply |

> **Publish the range, not the point.** A single EAC quoted to the nearest thousand claims a precision the method cannot support.

## The reality check nobody runs

**TCPI**

`TCPI = (BAC − EV) ÷ (BAC − AC)`

This is the cost efficiency required on all remaining work to still land on budget.

Compare it to the `CPI` you have actually achieved.

> **If TCPI is far above CPI**, the budget is asserting an improvement that nobody has yet demonstrated. On the worked case: TCPI 1.08 against a CPI of 0.89 — the plan needs a 22% efficiency gain on work running 11% below plan.

## Why SV lies to you at the end

**EARNED SCHEDULE**

`SV` and `SPI` are denominated in **currency**.

At completion `EV = PV`, so `SV` goes to zero and `SPI` goes to 1.00 — however late you finished.

`ES = M + (EV − PV_M) ÷ (PV_M+1 − PV_M)`

`SV(t) = ES − AT`

`SPI(t) = ES ÷ AT`

> **These do not converge.** Late in a project, earned schedule is the honest measure and the currency-based one is not.

## Float, two ways

**SCHEDULE · DOMAIN 10**

`EF = ES + Duration`

`LS = LF − Duration`

`TF = LS − ES = LF − EF`

`FF = min(successor ES) − EF`

> **A free integrity check.** Total float computed from the starts must equal total float computed from the finishes. If the two disagree, your forward or backward pass is wrong.

## Three points, one duration

**ESTIMATING**

`tE = (O + 4M + P) ÷ 6`

`σ = (P − O) ÷ 6`

PERT weights the most likely case four times. The triangular alternative, `(O + M + P) ÷ 3`, does not — and gives a different answer.

> **Watch for:** three points supplied by one person. That is one estimate wearing three hats, and it produces a false sense of range.

## Variance adds. Deviation does not.

**PATH UNCERTAINTY**

`σ^2 path = Σ σ^2 along the path`

Sum the **variances** along the path, then take the square root at the end.

Summing standard deviations directly overstates path uncertainty, often substantially.

## Where contingency should come from

**RISK · DOMAIN 12**

`EMV = Probability × Impact`

`Contingency ≈ Σ EMV, or P80 − P50 from simulation`

> **A percentage of budget is not an analysis.** If contingency does not reconcile to quantified risk exposure, it is a habit. And `EMV` describes a portfolio of risks — applied to one binary event, the expected value is a number that will never occur.

## Never accept this number

**PROGRESS MEASUREMENT**

| Work package | Method |
|---|---|
| Spans one period | 0/100 |
| Two periods, discrete ends | 50/50 |
| Countable homogeneous output | Units complete |
| Long, with verifiable stages | Milestone weighting |
| No discrete output | Level of effort — never the default |

> **Fix the method before work starts.** And never accept a percentage complete supplied by the party being measured. That figure is a commercial position, not a measurement.

## The ten that cost the most

**MOST MISAPPLIED · 1–5**

1. `CPI` computed on invoiced cost, with no accruals
2. `SV` read as a schedule measure late in a project
3. A single `EAC` presented as *the* forecast
4. `TCPI` never compared against the `CPI` achieved
5. Percentage complete supplied by the measured party

## And the other five

**MOST MISAPPLIED · 6–10**

6. Standard deviations summed along a schedule path
7. `EMV` applied to a single binary event
8. Contingency set as a percentage of budget
9. Crashing an activity that is not on the critical path
10. Management reserve included inside `BAC`

> **Every one of these produces a report that passes review.** That is precisely why they are expensive.

## The full sheet is free

**PCI AI · PROJECT CONTROLS INSTITUTE GLOBAL**

This deck is 16 of them. The complete reference carries every formula in the discipline — accounting, budgeting, cost, earned value, scheduling, risk, commercial and working capital — each with the mistake it attracts, the Knowledge Area where it is worked in full, and reproducible worked examples.

No registration. No email gate.

**projectcontrolsinstitute.org**
