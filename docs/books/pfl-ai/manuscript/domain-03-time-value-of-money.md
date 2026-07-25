# Domain 3 — Time Value of Money and Financial Mathematics *(quantitative flagship)*

> **Group:** Foundations (Domain 3 of 4 in Part One). **Target:** ~72 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain is the definitive home of the discounting symbols —
> `PV(x)`, `FV(x)`, `DF(t)`, `r`, `n`, `i_nom`, `i_real`, `π`, annuity payment `A` — every later
> domain restates them from here. Because this book is discounting-heavy, `PV` written bare is
> reserved for Earned Value contexts only; present value is always written `PV(x)` or in words.
> British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Every judgment a project finance leader makes — whether an investment is worth making (Domain 4),
how much debt a project can carry (Domain 10), what a concession's payment stream is worth
(Domain 7), whether a delay destroys value (Domain 8) — reduces at some point to one question:
*what is money at one date worth at another date?* Time value of money (TVM) is the machinery that
answers it. This domain builds that machinery from first principles: interest and growth (KA 3.1),
the annuity family and loan schedules that debt structures are made of (KA 3.2), and the
adjustments — inflation, escalation and currency — that separate a defensible model from a
spreadsheet accident (KA 3.3). Nothing in this domain is optional equipment: the financial models
of Domain 6 are, in the end, disciplined arrangements of exactly these calculations, and the
coverage ratios lenders live by (Domain 10) are discounted cash flows wearing covenants.

**Learning objectives.** After this domain a candidate can: distinguish simple and compound
interest and compute either; move any cash amount across time with `FV(x)` and `PV(x)` and read a
discount-factor table; value level streams with annuity and perpetuity formulae; build and check a
loan schedule (annuity, level-principal and bullet); convert between nominal, periodic and
effective rates; keep nominal and real quantities consistent using the Fisher relation; escalate
costs and revenues defensibly; state how currency and forward rates enter project cash flows; and
apply the governed-AI rule to any machine-produced calculation of these kinds.

**The master financing.** One fictional project runs through this domain and returns in Domains 4,
6 and 10. **Kestrel Water SPC** is a special-purpose company developing a seawater desalination
plant. Its senior lenders offer a **USD 42,000,000** loan repayable over **12 years at 6.0 %**
annual interest; the offtaker will pay an **availability payment of USD 5,600,000 per year for
25 years**; Kestrel's board evaluates offers at a discount rate of **8.0 %**. All three numbers are
used repeatedly below — by the end of the domain the reader can price every side of Kestrel's deal.

---

## Knowledge Area 3.1 — Interest and value

*Topics: 3.1.1 simple and compound interest · 3.1.2 present and future value · 3.1.3 discount
factors.*

### 3.1.1 Simple and compound interest

**Definitions.** Interest is the price of money over time. Under **simple interest** the charge
accrues on the original principal only:

```
FV(x) = x × (1 + r × n)        — simple interest
```

Under **compound interest** each period's interest joins the principal and itself earns interest:

```
FV(x) = x × (1 + r)^n          — compound interest
```

where `x` is the amount today (currency), `r` the interest rate per period (ratio) and `n` the
number of periods (count). Project finance is a compound-interest world: loans, discounting,
escalation and returns all compound. Simple interest survives only in narrow corners —
short-duration instruments, some penalty-interest clauses, and certain Islamic-finance structures
that avoid interest altogether and price time differently (Domain 9, KA 9.3).

**The principle.** Compounding is growth on growth. Its effect looks negligible over one or two
periods and decisive over ten: the gap between the two formulae widens geometrically, which is why
long-life infrastructure — concessions of 25 to 40 years — is acutely sensitive to the rate used.

**Worked example 3.1.1 — the same deposit, two rules.**

1. **Setup.** USD 100,000 is placed for 3 years at 8.0 % per year. Compare simple and compound
   growth.
2. **Formula.** Simple: `FV(x) = x(1 + rn)`. Compound: `FV(x) = x(1 + r)^n`, with `x` = 100,000,
   `r` = 0.08, `n` = 3.
3. **Substitution.** Simple: `100,000 × (1 + 0.08 × 3) = 100,000 × 1.24`. Compound:
   `100,000 × 1.08³ = 100,000 × 1.259712`.
4. **Result.** Simple: **USD 124,000**. Compound: **USD 125,971** (125,971.20 at full precision).
5. **Interpretation.** Three years of compounding adds USD 1,971 over simple interest — under 2 %.
   Over a 25-year concession life the same 8 % compounds to `1.08²⁵ ≈ 6.85` times the principal,
   while simple interest reaches only 3.0 times. The professional habit this example teaches:
   never extrapolate a short-horizon intuition to a long-horizon contract.

> **Fig 3.1.1 — Simple versus compound growth of USD 100,000 at 8 %.** Line chart, x-axis years
> 0–10, y-axis USD. Two series: simple `100,000 × (1 + 0.08t)` — a straight line reaching 180,000
> at year 10 — and compound `100,000 × 1.08^t` — a curve reaching 215,892 at year 10. Sample
> points: year 3 — 124,000 vs 125,971; year 5 — 140,000 vs 146,933; year 10 — 180,000 vs 215,892.
> Caption emphasises the widening gap. Source: PCI original. Alt text: line chart comparing
> straight-line simple-interest growth with the steeper compound-interest curve over ten years.

### 3.1.2 Present and future value

**Definitions.** Compounding runs both ways. **Future value** carries an amount forward;
**present value** brings a future amount back:

```
FV(x) = x × (1 + r)^n            PV(x) = x / (1 + r)^n
```

`PV(x)` answers the only question a bid evaluation, a settlement offer or a buy-out negotiation
really asks: *what is that future money worth today, at the return I could otherwise earn?* The
rate `r` used in this role is called the **discount rate**; choosing it is a Domain 4 judgment
(and, for a levered project, a Domain 9/10 conversation) — this KA takes it as given.

**Worked example 3.1.2 — pricing a deferred receipt.**

1. **Setup.** Kestrel will receive a USD 500,000 connection rebate exactly 5 years from now. The
   board discounts at 7.0 % for this risk. What is the rebate worth today?
2. **Formula.** `PV(x) = x / (1 + r)^n`, with `x` = 500,000, `r` = 0.07, `n` = 5.
3. **Substitution.** `PV(x) = 500,000 / 1.07⁵ = 500,000 / 1.402552`.
4. **Result.** **USD 356,493** (356,493.09 at full precision).
5. **Interpretation.** A five-year wait at 7 % costs the rebate just under 29 % of its face value.
   If a counterparty offered USD 380,000 cash today to extinguish the obligation, Kestrel should
   accept — and if offered USD 330,000, decline. `PV(x)` turns "later" into a number that can be
   compared, which is the whole trade of this profession.

**Common pitfall.** Discounting with simple interest — `500,000 / (1 + 0.07 × 5) = 370,370` —
overstates the value by USD 13,877 here. The error grows with horizon and rate; audit any model
whose discount factors were "simplified".

### 3.1.3 Discount factors

**Definition.** The **discount factor** for period `t` is the present value of one currency unit
received at `t`:

```
DF(t) = 1 / (1 + r)^t          so that   PV(x at t) = x × DF(t)
```

Discount factors are how models industrialise discounting: compute the factor row once, multiply
every cash-flow row by it (Domain 6, KA 6.1). They also make review fast — an experienced reviewer
reads a factor table the way a scheduler reads float.

At `r` = 10 %:

| `t` (years) | 1 | 2 | 3 | 4 | 5 |
|---|---|---|---|---|---|
| `DF(t)` | 0.9091 | 0.8264 | 0.7513 | 0.6830 | 0.6209 |

Two sanity rules a reviewer applies on sight: factors must **decline monotonically**, and each
factor must equal the previous factor divided by `(1 + r)`. A factor table that violates either has
a hard-coded cell or a broken formula — a Domain 6 model-check classic (KA 6.4).

**Rounding discipline.** Factors are displayed to four decimals but **calculations use full
precision**. Multiplying a USD 900,000,000 programme cash flow by a four-decimal factor can move
results by tens of thousands; the display is for the reader, never for the arithmetic (see the
registry's decimal-arithmetic rule).

### AI in this KA

Spreadsheet copilots will happily draft TVM formulae, and large-language-model assistants will
"explain" a discount factor with perfect fluency and an inverted exponent. The governed position
(the PCI principle: **AI proposes; the professional verifies, decides and remains accountable**)
is mechanical here because verification is cheap: recompute one factor by hand, check
monotonicity, check `DF(1) × (1+r) = 1`. An AI-drafted factor row that no human recomputed is not
a calculation; it is a claim.

### Key terms — KA 3.1

| Term | Meaning |
|---|---|
| **Simple interest** | Interest accruing on original principal only: `FV(x) = x(1 + rn)`. |
| **Compound interest** | Interest accruing on principal and accumulated interest: `FV(x) = x(1 + r)^n`. |
| **Present value `PV(x)`** | Today's worth of a future amount at discount rate `r`. |
| **Future value `FV(x)`** | The compounded worth of an amount at a future date. |
| **Discount rate** | The rate `r` used to move value across time; the required return for the risk. |
| **Discount factor `DF(t)`** | `1/(1+r)^t`; the PV of one unit received at `t`. |

### Sample MCQs — KA 3.1

**MCQ 3.1-A `[3.1.2 · Application]`** A payment of USD 500,000 is due in 5 years; the discount
rate is 7 %. Its present value is closest to:
- A. USD 370,370
- B. USD 356,493 ✅
- C. USD 381,448
- D. USD 701,276

*Rationale:* `500,000 / 1.07⁵ = 356,493`. A discounts with simple interest `(1 + 0.35)`; C uses
four years instead of five; D compounds forward instead of discounting back.

**MCQ 3.1-B `[3.1.3 · Application]`** At a 10 % discount rate, the discount factor for year 3 is:
- A. 0.7000
- B. 0.8264
- C. 0.7513 ✅
- D. 0.6830

*Rationale:* `1/1.10³ = 0.7513`. A subtracts 10 % three times (simple); B is the year-2 factor;
D is the year-4 factor — the two most common off-by-one-period errors.

**MCQ 3.1-C `[3.1.1 · Analysis]`** A lender quotes 25-year money and a borrower's analyst tests
affordability using simple interest "as a conservative shortcut". The analyst's error is that:
- A. simple interest overstates the debt cost, so the test is merely too strict
- B. compound growth exceeds simple growth over long horizons, so the shortcut materially understates the true accumulation ✅
- C. the two methods converge over long horizons, so the shortcut is harmless
- D. simple interest cannot be computed for horizons beyond ten years

*Rationale:* Compounding produces growth on growth: at 8 % over 25 years the compound multiple is
≈ 6.85 versus 3.0 simple — the shortcut is anti-conservative, the opposite of the analyst's claim
(so A is wrong); the methods diverge rather than converge (C); D is arithmetic nonsense.

### Self-check — KA 3.1

1. *Why must discount factors decline monotonically?* — Because `(1+r)^t` grows with `t` for any
   positive `r`; a rising factor implies a negative rate or a broken formula.
2. *Your model shows `DF(4) = 0.6830` and `DF(5) = 0.6209` at 10 %. One cell check confirms both.
   Which?* — `0.6830 / 1.10 = 0.6209`: each factor is the prior factor divided by `(1+r)`.
3. *A colleague says "8 % for three years is 24 %".* — Only under simple interest; compounded it is
   `1.08³ − 1 = 25.97 %`, and the gap widens every further year.

---

## Knowledge Area 3.2 — Annuities and loan schedules

*Topics: 3.2.1 annuities and perpetuities · 3.2.2 loan schedules · 3.2.3 compounding frequency and
effective rates.*

### 3.2.1 Annuities and perpetuities

**Definitions.** An **annuity** is a level stream — the same amount each period for `n` periods.
Project finance is built from annuities and near-annuities: availability payments, capacity
charges, lease rentals, O&M fees, debt service. The present value of an ordinary annuity (payments
at period-ends) of amount `A`:

```
PV(annuity) = A × AF(r, n)      where   AF(r, n) = (1 − (1 + r)^−n) / r
```

`AF(r, n)` is the **annuity factor** — the sum of the discount factors `DF(1) … DF(n)`. A
**perpetuity** is the limiting case as `n → ∞`:

```
PV(perpetuity) = A / r
```

**Worked example 3.2.1 — valuing Kestrel's availability stream.**

1. **Setup.** Kestrel's offtaker will pay USD 3,000,000 per year for 20 years under an early
   tariff proposal, paid annually in arrears. The board discounts at 9.0 %. Value the stream.
2. **Formula.** `PV = A × AF(r, n)`, `AF(r, n) = (1 − (1+r)^−n)/r`, with `A` = 3,000,000,
   `r` = 0.09, `n` = 20.
3. **Substitution.** `AF(0.09, 20) = (1 − 1.09⁻²⁰)/0.09 = 9.128546`;
   `PV = 3,000,000 × 9.128546`.
4. **Result.** **USD 27,385,637** (using the full-precision factor).
5. **Interpretation.** Twenty years of payments are worth barely nine years' worth of face value —
   discounting at 9 % has priced away more than half the nominal USD 60,000,000 total. Sensitivity:
   at a 10 % rate the factor falls to 8.513564 and the value to USD 25,540,691 — a 6.7 % value loss
   for one point of rate, the shape of sensitivity every concession bidder lives with (Domain 7,
   KA 7.4).

**Common pitfall — the rounded factor.** Using a four-decimal factor (9.1285) gives
USD 27,385,500 — USD 137 adrift here, but scale the stream by 100× and the drift is five figures.
Factors are display; arithmetic is full precision (KA 3.1.3).

**Timing variants.** An **annuity-due** (payments in advance, common in leases and some
availability regimes) is worth `(1 + r)` times the ordinary annuity. Misreading the payment
convention in a concession agreement misprices the whole stream by one period's discounting —
check the contract, not the habit (Domain 12, KA 12.2).

### 3.2.2 Loan schedules

**The three canonical shapes.** A loan of principal `P` over `n` periods at rate `r` can be
scheduled three ways, and a project finance leader must read all three on sight:

| Shape | Rule | Debt-service profile | Where seen |
|---|---|---|---|
| **Annuity (equal instalment)** | Payment `A = P × r / (1 − (1+r)^−n)` each period | Level total; interest share falls, principal share rises | Most project term loans; mortgages |
| **Level principal** | Principal `P/n` each period + interest on balance | Front-loaded total, declining | Some ECA and development-bank facilities (Domain 9) |
| **Bullet** | Interest only; principal entire at maturity | Low until the final balloon | Bonds; mini-perm structures; refinancing plays (Domain 15) |

**Worked example 3.2.2 — Kestrel's senior loan.**

1. **Setup.** USD 42,000,000, 12 annual instalments, 6.0 % — annuity shape. Find the instalment
   and build the first three schedule rows.
2. **Formula.** `A = P × r / (1 − (1+r)^−n)`, with `P` = 42,000,000, `r` = 0.06, `n` = 12. Each
   row: interest = opening balance × `r`; principal = `A` − interest; closing = opening − principal.
3. **Substitution.** `A = 42,000,000 × 0.06 / (1 − 1.06⁻¹²) = 42,000,000 / 8.383844`.
4. **Result.** `A` = **USD 5,009,635** per year (5,009,635.23). Schedule:

   | Year | Opening balance | Interest (6 %) | Principal | Closing balance |
   |---|---|---|---|---|
   | 1 | 42,000,000 | 2,520,000 | 2,489,635 | 39,510,365 |
   | 2 | 39,510,365 | 2,370,622 | 2,639,013 | 36,871,351 |
   | 3 | 36,871,351 | 2,212,281 | 2,797,354 | 34,073,997 |

5. **Interpretation.** The instalment never changes; its composition does — year 1 is 50 % interest,
   and by the final year almost all principal. Two checks a reviewer runs instantly: the closing
   balance after year 12 must be exactly zero (a residual means a formula error), and total
   principal across all rows must equal `P`. This schedule is the direct input to Kestrel's debt
   service line — and therefore to its DSCR — in Domains 6 and 10.

> **Fig 3.2.1 — Anatomy of an annuity loan: Kestrel's USD 42,000,000, 12 years at 6 %.** Stacked
> bar chart, x-axis years 1–12, y-axis USD; each bar totals 5,009,635, split into an interest
> portion (2,520,000 in year 1, shrinking to ≈ 283,564 in year 12) and a principal portion
> (2,489,635 in year 1, growing to ≈ 4,726,071 in year 12). A horizontal rule marks the level
> instalment. Source: PCI original. Alt text: stacked bars showing a constant annual loan payment
> whose interest share shrinks and principal share grows across twelve years.

**Common pitfall — the bullet that looks cheap.** A bullet's early debt service is a fraction of
an annuity's, and a cash-strapped sponsor will feel the temptation. The full price appears at
maturity as refinancing risk — the balloon must be refinanced at whatever the market then charges
(Domain 15, KA 15.3). Case study B prices this trade.

### 3.2.3 Compounding frequency and effective rates

**Definitions.** A quoted **nominal annual rate** `i_nom` compounded `m` times per year applies
`i_nom/m` per period. The **effective annual rate (EAR)** is the once-a-year rate that produces the
same growth:

```
EAR = (1 + i_nom / m)^m − 1
```

**Worked example 3.2.3 — the same 6 %, four ways.**

1. **Setup.** A facility is quoted at "6 % nominal". Find the effective annual rate if
   compounding is annual, semi-annual, quarterly and monthly.
2. **Formula.** `EAR = (1 + 0.06/m)^m − 1` for `m` = 1, 2, 4, 12.
3. **Substitution.** `m=4`: `(1 + 0.015)⁴ − 1 = 1.061364 − 1`.
4. **Result.** Annual **6.000 %** · semi-annual **6.090 %** · quarterly **6.136 %** · monthly
   **6.168 %**.
5. **Interpretation.** "Six per cent" is not one price; it is four prices in this example alone,
   and the differences compound over a 12-year loan. Term sheets are compared on effective rates,
   never on quoted nominals — and loan models must use the **periodic** rate `i_nom/m` with `n × m`
   periods, not the EAR with `n` years, or debt service will be misstated (Domain 6, KA 6.3).

### AI in this KA

Amortisation is where AI-assisted modelling earns its keep — and where it must be caged.
Generating a 360-row monthly schedule is exactly the mechanical work a copilot does well; whether
the schedule uses the periodic rate correctly, honours the day-count convention in the actual
facility agreement, and zeroes the final balance is exactly what it gets wrong silently. The
governed workflow (Domain 6, KA 6.4; Domain 16): AI drafts the schedule; the professional runs the
three deterministic checks (final balance zero; Σ principal = `P`; payment recomputed
independently) before any output leaves the model.

### Key terms — KA 3.2

| Term | Meaning |
|---|---|
| **Annuity / annuity-due** | Level periodic stream, in arrears / in advance. |
| **Annuity factor `AF(r,n)`** | `(1 − (1+r)^−n)/r`; PV of 1 per period for `n` periods. |
| **Perpetuity** | Level stream forever; `PV = A/r`. |
| **Annuity, level-principal, bullet** | The three canonical loan shapes. |
| **Balloon / refinancing risk** | The bullet's maturity principal and the risk of the rate then prevailing. |
| **Nominal rate / EAR** | Quoted annual rate with compounding frequency / its once-a-year equivalent. |

### Sample MCQs — KA 3.2

**MCQ 3.2-A `[3.2.2 · Application]`** A USD 42,000,000 loan is repaid by 12 equal annual
instalments at 6 %. The instalment is closest to:
- A. USD 3,500,000
- B. USD 2,520,000
- C. USD 5,009,635 ✅
- D. USD 5,706,454

*Rationale:* `42,000,000 × 0.06/(1 − 1.06⁻¹²) = 5,009,635`. A divides principal by 12 and ignores
interest; B is interest-only on the full balance; D uses a 10-year annuity factor — the
wrong-tenor error.

**MCQ 3.2-B `[3.2.3 · Application]`** A 6 % nominal rate compounded quarterly has an effective
annual rate of:
- A. 6.00 %
- B. 6.09 %
- C. 6.14 % ✅
- D. 6.17 %

*Rationale:* `(1 + 0.015)⁴ − 1 = 6.136 % ≈ 6.14 %`. A is the nominal itself; B is semi-annual
compounding; D is monthly — each a frequency misread.

**MCQ 3.2-C `[3.2.1 · Analysis]`** Two otherwise identical concession bids value the same
20-year availability stream. Bid X used the full-precision annuity factor; Bid Y rounded the
factor to four decimals. The most defensible statement is:
- A. the bids differ by an amount that grows with the size of the stream, and only X's practice passes model audit ✅
- B. rounding the factor is conservative, so Y understates value and is safer
- C. the difference is always immaterial because four decimals is industry standard
- D. Y's approach is wrong only if the discount rate exceeds 10 %

*Rationale:* Factor rounding error scales linearly with the cash flows and has no consistent
conservative direction (B, D wrong); materiality depends on scale, not convention (C wrong); model
audit standards require full-precision arithmetic with display-only rounding.

### Self-check — KA 3.2

1. *What two instant checks validate any amortising schedule?* — Final closing balance exactly
   zero; total principal equals the original loan.
2. *Why is an annuity-due worth `(1+r)` × the ordinary annuity?* — Every payment arrives one
   period earlier, so each escapes one period of discounting.
3. *A term sheet quotes "5.9 % monthly"; another "6.0 % annual". Which is dearer?* — The first:
   `(1 + 0.059/12)¹² − 1 = 6.06 %` effective, above 6.00 %.

---

## Knowledge Area 3.3 — Real-world adjustments: inflation, escalation and currency

*Topics: 3.3.1 nominal and real rates · 3.3.2 inflation and escalation · 3.3.3 currency effects.*

### 3.3.1 Nominal and real rates — the Fisher relation

**Definitions.** A **nominal** rate `i_nom` is quoted in money terms; a **real** rate `i_real` is
measured in purchasing power, after inflation `π`. They are linked by the **Fisher relation**:

```
(1 + i_nom) = (1 + i_real) × (1 + π)      so      i_real = (1 + i_nom)/(1 + π) − 1
```

The subtraction shortcut `i_real ≈ i_nom − π` is a first-order approximation only — usable for
intuition, never for models.

**Worked example 3.3.1 — the real cost of Kestrel's equity hurdle.**

1. **Setup.** Kestrel's shareholders require a 9.0 % nominal return; inflation is expected at
   3.0 %. What real return are they actually demanding?
2. **Formula.** `i_real = (1 + i_nom)/(1 + π) − 1`, with `i_nom` = 0.09, `π` = 0.03.
3. **Substitution.** `i_real = 1.09 / 1.03 − 1 = 1.058252 − 1`.
4. **Result.** **5.83 %** real (5.8252 % at full precision). The subtraction shortcut says 6.00 % —
   17 basis points high.
5. **Interpretation.** Seventeen basis points sounds academic until it is applied to a 25-year
   stream: mispricing the hurdle by that much moves a large concession's value by roughly one and
   a half years' worth of payments. Models must live entirely in one world — nominal cash flows
   with nominal rates, or real with real. **Mixing them is the single most common TVM defect found
   in model audits** (Domain 6, KA 6.4; Domain 13, KA 13.2).

### 3.3.2 Inflation and escalation

**Definitions.** **Inflation** is the general price level's drift; **escalation** is the
contractual or forecast growth of a *specific* price — a tariff indexed to CPI, labour rates under
a collective agreement, steel under a commodity formula. An amount `x` escalating at rate `e` for
`n` periods becomes `x × (1 + e)^n`. Escalation compounds, exactly like interest — and like
interest, it is routinely underestimated over long horizons.

**Worked example 3.3.2 — an O&M contract's third-year price.**

1. **Setup.** Kestrel's O&M contract prices year-0 services at USD 10,000,000, escalating 4.0 %
   annually. What is the year-3 price?
2. **Formula.** `x_n = x₀ × (1 + e)^n`, with `x₀` = 10,000,000, `e` = 0.04, `n` = 3.
3. **Substitution.** `10,000,000 × 1.04³ = 10,000,000 × 1.124864`.
4. **Result.** **USD 11,248,640**.
5. **Interpretation.** Simple-escalation thinking says "+12 %" (USD 11,200,000); compounding adds
   USD 48,640 by year 3 and the gap widens every year after. In a 25-year operating model the
   compound-vs-simple escalation choice changes lifecycle cost by whole percentage points —
   Domain 8 (KA 8.2) builds the full escalated cash-flow machinery on this rule.

**Indexation discipline.** Real contracts escalate on published indices with definitions, lags,
caps and floors (Domain 7, KA 7.3; Domain 12). A model's escalation assumptions must cite the
contractual index and its mechanics, not a bare percentage — the toolkit's escalation register
(3.T.3) exists for exactly this.

> **Fig 3.3.1 — What 3 % inflation does to USD 1,000,000 of purchasing power.** Line chart,
> x-axis years 0–20, y-axis USD (real purchasing power of a fixed nominal 1,000,000):
> `1,000,000 / 1.03^t`. Sample points: year 5 — 862,609; year 10 — 744,094; year 20 — 553,676.
> A dashed reference line at 1,000,000 marks the nominal illusion. Source: PCI original.
> Alt text: declining curve showing a fixed million dollars losing nearly half its purchasing
> power over twenty years at three per cent inflation.

### 3.3.3 Currency effects

**The problem.** Project cash flows often arrive in one currency while debt service, equipment or
returns are owed in another. Two rates matter: the **spot rate** now, and the **forward rate** at
which future exchange can be contracted. Forwards are priced off interest differentials (covered
interest parity, stated here in its textbook form):

```
F ≈ S × (1 + i_quote) / (1 + i_base)
```

**Worked example 3.3.3 — a one-year SAR/USD forward.**

1. **Setup.** Spot `USD 1 = SAR 3.7500` (the indicative peg rate used throughout this book).
   One-year money costs 5.0 % in USD and 5.5 % in SAR. Estimate the one-year forward.
2. **Formula.** `F = S × (1 + i_SAR)/(1 + i_USD)`, with `S` = 3.7500.
3. **Substitution.** `F = 3.7500 × 1.055 / 1.05`.
4. **Result.** **SAR 3.7679 per USD**.
5. **Interpretation.** The forward is not a forecast; it is arithmetic — the rate at which
   borrowing in one currency and lending in the other breaks even. A project that leaves a
   currency mismatch unhedged is not "expecting the spot to hold"; it is running an open position
   whose size Domain 11 (KA 11.3) teaches you to measure and whose contractual mitigations
   Domain 12 allocates. **Assumption honesty:** actual pegs, spreads and convertibility
   restrictions are jurisdiction- and time-specific; every currency figure in this book is
   illustrative.

### AI in this domain — the systematic view

TVM is deterministic, which makes it the safest place in finance to use machine assistance and the
most dangerous place to trust it unchecked: a wrong exponent produces a *plausible* number, not an
absurd one. The domain's governed workflow, applied wherever Domains 4–16 discount anything:

1. **AI proposes** — drafts the factor table, schedule or escalated series.
2. **Deterministic checks** — monotone factors; `DF(1)(1+r) = 1`; schedule zeroes; Σ principal
   = `P`; one hand-recomputed cell per block.
3. **Assumption register** — rate, basis (nominal/real), compounding frequency, index, and
   currency of every stream written down (toolkit 3.T.1).
4. **The professional decides and remains accountable** — no AI-produced number reaches a board
   paper, bid or lender report without a named human owner (Domain 16, KA 16.4).

### Key terms — KA 3.3

| Term | Meaning |
|---|---|
| **Nominal / real rate** | Money-terms rate / purchasing-power rate; linked by Fisher. |
| **Fisher relation** | `(1+i_nom) = (1+i_real)(1+π)`. |
| **Inflation `π` / escalation `e`** | General price drift / specific contractual price growth. |
| **Indexation** | Contractual escalation by a published index, with lags, caps, floors. |
| **Spot / forward rate** | Exchange rate now / contracted for a future date. |
| **Covered interest parity** | Forward ≈ spot × interest-ratio; the no-arbitrage forward. |

### Sample MCQs — KA 3.3

**MCQ 3.3-A `[3.3.1 · Application]`** Nominal return 9 %, inflation 3 %. The real return is
closest to:
- A. 6.00 %
- B. 5.83 % ✅
- C. 12.27 %
- D. 3.00 %

*Rationale:* `1.09/1.03 − 1 = 5.83 %`. A is the subtraction approximation; C multiplies
`1.09 × 1.03 − 1` (compounding inflation on instead of off); D confuses the inflation rate itself
with the real return.

**MCQ 3.3-B `[3.3.2 · Application]`** A USD 10,000,000 cost escalating at 4 % per year is, in
year 3:
- A. USD 11,200,000
- B. USD 10,400,000
- C. USD 12,486,400
- D. USD 11,248,640 ✅

*Rationale:* `10,000,000 × 1.04³ = 11,248,640`. A escalates simply (3 × 4 %); B stops at one year;
C applies four periods' escalation with a decimal slip.

**MCQ 3.3-C `[3.3.1 · Analysis]`** A model discounts real (uninflated) cash flows at the
sponsor's nominal 9 % hurdle. The result:
- A. overstates value, because inflation is counted twice
- B. understates value, because inflation is removed from the flows but still charged in the rate ✅
- C. is correct if inflation is below 5 %
- D. is correct because discount rates are always nominal

*Rationale:* Real flows with a nominal rate deduct inflation twice — once from the flows, once
inside the rate — so value is systematically understated. The consistency rule admits no threshold
(C) and no default (D); A reverses the direction of the error.

### Self-check — KA 3.3

1. *State the consistency rule.* — Nominal flows with nominal rates, real flows with real rates;
   never mixed in one calculation.
2. *Why does a 25-year concession make the Fisher shortcut dangerous?* — The 17 bp error (at
   9 %/3 %) compounds across every one of 25 discount factors.
3. *Is a forward rate a forecast?* — No: it is the no-arbitrage consequence of today's spot and the
   two interest rates.

---

## Advanced topics — Domain 3

### 3.A.1 Continuous compounding

As compounding frequency `m → ∞`, `(1 + r/m)^{mn} → e^{rn}`. USD 100,000 at 8 % for 3 years
compounds continuously to `100,000 × e^{0.24}` = **USD 127,125** — against 125,971 annually. In
project work continuous compounding appears mainly in derivative pricing and some academic
appraisal literature; its practical value here is as a bound: no compounding frequency can push
growth beyond `e^{rn}`.

### 3.A.2 Irregular timing and period conventions

Real cash flows do not arrive on anniversary dates. Three conventions bridge the gap: **exact-date
discounting** (each flow discounted by its actual day count — the XNPV approach, standard in
transaction models); the **mid-period convention** (flows treated as arriving mid-period,
appropriate for continuous operations revenue); and **period-end** (conservative for receipts,
default in lender base cases). The convention is an *assumption* — it belongs in the assumption
register, it changes results by up to half a period's discounting, and model audits (Domain 13,
KA 13.2) check that one convention is applied everywhere.

### 3.A.3 Term structures — when one rate is not enough

A single flat `r` assumes money has the same price at every horizon. Markets disagree: rates form
a **term structure**, and precise valuation discounts each period's flow at that period's rate
(equivalently, its own `DF(t)` from a curve). Project models typically justify a flat rate as a
deliberate simplification of a curve — acceptable when documented and stress-tested (Domain 6,
KA 6.4), a silent error when inherited unexamined from a template.

### 3.A.4 The reviewer's TVM eye

Experienced reviewers do not recompute everything; they run invariants. The domain's collected
set: factors decline; `DF(1)(1+r) = 1`; annuity factor × `r` → `1 − DF(n)`; schedules zero out;
Σ principal = `P`; effective ≥ nominal, with equality only at annual compounding; real < nominal
whenever `π > 0`; forward off-spot in the direction of the interest differential. Any violated
invariant is a defect *somewhere* — find it before anyone downstream builds on it.

---

## Case study — Domain 3: choosing between an upfront grant and an availability stream (water / PPP)

**Situation.** Kestrel Water SPC's grantor offers two support packages for the desalination
concession, and the board must choose one at financial close:

- **Package U:** a single upfront capital grant of **USD 58,000,000** at completion.
- **Package S:** an availability supplement of **USD 5,600,000 per year for 25 years**, paid
  annually in arrears from completion.

Kestrel evaluates support at its 8.0 % project discount rate. The grantor's advisers privately
evaluate at 10 %.

**Analysis.** The stream's value to Kestrel:
`AF(0.08, 25) = (1 − 1.08⁻²⁵)/0.08 = 10.674776`;
`PV = 5,600,000 × 10.674776 =` **USD 59,778,747** — worth **USD 1,778,747 more** than the upfront
grant. To the grantor's advisers at 10 %: `AF(0.10, 25) = 9.077040`;
`PV = 5,600,000 × 9.077040 =` **USD 50,831,424** — worth **USD 7,168,576 less** than the grant they
would otherwise pay today.

**The decision.** Both sides prefer a different package *from the same facts* — a discount-rate
wedge, not a disagreement about arithmetic. Kestrel takes the stream (worth more at 8 %); the
grantor is glad to give it (cheaper at 10 %); the deal closes with both counterparties better off
by their own measure. The board minute records the rate, the convention (annual, in arrears) and
the sensitivity: the stream's advantage to Kestrel disappears if its discount rate rises past
≈ 8.35 % — the **breakeven rate** at which `AF × 5,600,000 = 58,000,000`, i.e. `AF = 10.357`.

**What the domain teaches here.** Valuation is always *at a rate, for a party*; streams and lumps
are only comparable through `PV(x)`; and the negotiation surface between two parties' rates is
where structuring value lives (Domain 7 revisits this as tariff design; Domain 9 as the
grant-versus-support decision).

## Case study B — Domain 3: the bullet that had to be refinanced (energy / refinancing)

**Situation.** A 30 MW peaking-plant SPV borrowed **USD 30,000,000** for 7 years. The sponsor
chose a **bullet** (interest-only at 7.5 %, principal at maturity) over the lenders' preferred
**annuity** shape, to keep early cash for distributions.

**The two prices.** Annuity instalment: `A = 30,000,000 × 0.075/(1 − 1.075⁻⁷) =`
**USD 5,664,009** per year; total paid 39,648,066, of which interest **USD 9,648,066**. Bullet:
interest `30,000,000 × 0.075 =` 2,250,000 per year — total interest **USD 15,750,000**, plus the
full USD 30,000,000 due in year 7. The bullet pays **USD 6,101,934 more interest** for the
privilege of deferral — and retains the entire principal as **refinancing risk**.

**What happened.** At year 7 the credit market had repriced: the refinancing cleared at 9.8 %, and
the new 7-year annuity instalment on the rolled principal became `30,000,000 × 0.098/(1 −
1.098⁻⁷) = 6,121,646` — against 5,664,009 had the original annuity simply been running to zero.
Distributions locked up under the new facility's covenants for two years (Domain 10, KA 10.4).

**What the domain teaches here.** A schedule shape is a risk allocation, not a formatting choice:
the annuity retires rate risk continuously; the bullet stores it at maturity, priced at a rate
nobody today knows. Deferral has a computable minimum cost (the interest differential) and an
uncomputable tail (the refinancing market) — leadership means pricing the first honestly and
sizing the second deliberately (Domain 15, KA 15.3).

---

## Executive perspective — Domain 3

What a project finance director cannot delegate in this domain:

- **The rate.** Analysts compute with a discount rate; directors *own* it. Whoever sets `r` sets
  the answer — the choice is a governance act (Domain 4 gives it method; Domain 3 makes its power
  visible).
- **The world.** Nominal or real, once, for the whole model — and the director asks which, out
  loud, at the first model review.
- **The conventions.** In-arrears vs in-advance, compounding frequency, day counts, escalation
  indices: each is small, each moves millions at scale, and together they are why the assumption
  register (3.T.1) is a board-visible artefact, not analyst hygiene.
- **The verification culture.** The invariants of 3.A.4 cost minutes; unverified TVM in a signed
  bid costs careers. The director's question is never "who computed this?" but "who *re*computed
  this?" — and, where AI drafted it, the answer must name a human.

## Calculation exercises — Domain 3

**Exercise 3.1** A land payment of USD 850,000 falls due in 8 years; the applicable discount rate
is 11 %. Value it today.
*Solution.* `PV = 850,000 / 1.11⁸ = 850,000 / 2.304538 =` **USD 368,838** (368,837.52). Common
error: using `n` = 7 gives 409,410 — an off-by-one from counting "in 8 years" as 7 compounding
periods.

**Exercise 3.2** A USD 2,500,000 equipment loan is repaid monthly over 20 years at a 5.0 % nominal
annual rate compounded monthly. Find the monthly payment.
*Solution.* Periodic rate `0.05/12 = 0.004167`; `n = 240`;
`A = 2,500,000 × 0.004167/(1 − 1.004167⁻²⁴⁰) =` **USD 16,499** per month (16,498.89). Common
error: using the EAR (5.116 %) with 20 annual periods misstates debt service materially.

**Exercise 3.3** Convert a 12 % nominal rate compounded monthly to its effective annual rate.
*Solution.* `(1 + 0.01)¹² − 1 = 1.126825 − 1 =` **12.68 %**. Common error: reporting 12 % — the
nominal — as "the rate" in a comparison against an annually-compounded 12.5 % offer, which is in
fact cheaper.

**Exercise 3.4** A transmission wayleave pays USD 1,200,000 per year indefinitely; the discount
rate is 8.5 %. Value the perpetuity.
*Solution.* `PV = 1,200,000 / 0.085 =` **USD 14,117,647**. Common error: applying a long annuity
factor (say 30 years, `AF = 10.694`) "as approximately forever" understates value by ≈ 9 %.

**Exercise 3.5** A market forecast promises a 12 % nominal return with 5 % inflation. State the
real return, exactly and by the shortcut, and the shortcut's error.
*Solution.* Exact: `1.12/1.05 − 1 =` **6.67 %** (6.6667 %). Shortcut: `12 − 5 = 7.00 %`. The
shortcut overstates the real return by **33 basis points** — material over any long horizon.

## Practitioner's toolkit — Domain 3

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 3.T.1 — TVM assumption register (one row per modelled stream)

| Field | Discipline it enforces |
|---|---|
| Stream name & source document | Every number traces to a contract or estimate |
| Currency | Mismatches surface (KA 3.3.3) |
| Nominal or real | The consistency rule (3.3.1), stated not assumed |
| Rate & its owner | The director owns `r` (Executive perspective) |
| Compounding frequency & day count | Effective-rate honesty (3.2.3) |
| Timing convention (arrears/advance/mid-period) | Half-period errors caught (3.A.2) |
| Escalation index, lag, cap/floor | Contractual, not invented (3.3.2) |

### Toolkit 3.T.2 — Schedule QA checklist

- [ ] Payment recomputed independently of the model (different implementer or method).
- [ ] Final balance exactly zero; Σ principal = original loan.
- [ ] Periodic rate = nominal / frequency; period count = years × frequency.
- [ ] Discount factors monotone; `DF(1) × (1 + r) = 1`.
- [ ] Display rounding only — full precision beneath every printed figure.
- [ ] AI-drafted content marked, and its human verifier named (Domain 16, KA 16.4).

### Toolkit 3.T.3 — Escalation register

For each escalating line: base amount and base date · index (publisher, series, definition) ·
lag · formula (full/partial indexation, weightings) · cap/floor · compounding basis · the clause
reference in the contract. A model whose escalation register is complete can survive a Domain 13
model audit; one whose escalation is "4 % because last year" cannot.

## Exam preparation — Domain 3

**The calculation traps.** Off-by-one periods ("in year 5" vs "after 5 years") · simple-for-
compound substitution · rounded discount factors in the arithmetic · annuity-due priced as
ordinary · EAR used as the periodic rate · nominal/real mixing · escalation applied simply ·
tenor mismatch in annuity factors (the 3.2-A distractor D) · treating a forward as a forecast.

**Reflection questions.**
1. Your CFO asks for "the value of the concession". What three questions must you answer before
   any number is defensible? *(At what rate; in which world — nominal or real; under which timing
   conventions.)*
2. A lender and a sponsor disagree about the value of the same payment stream. Under what
   conditions are both right — and where does that create negotiating room? *(Case study A.)*
3. Which invariant checks would have caught the last TVM error you saw in the wild — and why
   didn't they run? *(3.A.4; toolkit 3.T.2.)*

## Domain 3 summary

Money's value is a function of time, and this domain built the complete conversion machinery:
compounding and discounting (`FV(x)`, `PV(x)`, `DF(t)`); the annuity family that prices every
level stream from availability payments to debt service (`AF(r,n)`, perpetuities, the three loan
shapes and their risk meanings); frequency and effective-rate honesty; and the three real-world
adjustments — Fisher-consistent inflation treatment, contract-grade escalation, and no-arbitrage
currency forwards. Its discipline is as important as its formulae: one world per model, full
precision beneath displayed rounding, conventions in the assumption register, invariants run on
every schedule, and machine-drafted arithmetic verified by a named human before anyone relies on
it. Every discounted number in the thirteen domains that follow stands on this one.
