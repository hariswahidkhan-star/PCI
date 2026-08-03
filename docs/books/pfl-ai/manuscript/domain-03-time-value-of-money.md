# Domain 3 — Time Value of Money and Financial Mathematics
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
discount-factor table; value level streams with annuity and perpetuity formulae, and **indexed
(growing) streams** with the real-rate identity that collapses them onto the same machinery; build
and check a loan schedule in all four shapes a term sheet offers — annuity, level-principal, bullet
and repayment holiday — and compute the **outstanding balance at any date** two independent ways;
**solve the inverse problems**, finding the rate implied by a price and the term implied by a
payment; convert between nominal, periodic and effective rates and say what a payment frequency
does that a compounding frequency does not; keep nominal and real quantities consistent using the
Fisher relation, and **price the error** when they are mixed; escalate costs and revenues
defensibly, including caps, floors and indexation lags; state how currency and forward rates enter
project cash flows and prove that the two routes to a hedged value agree; apply a day-count
convention over a full year and across a whole facility; and apply the governed-AI rule to any
machine-produced calculation of these kinds.

**The master financing.** One fictional project runs through this domain and returns in Domains 4,
6 and 10. **Kestrel Water SPC** is a special-purpose company developing a seawater desalination
plant inside a **USD 60,000,000** capital envelope funded 70/30. Its senior lenders offer a
**USD 42,000,000** loan repayable over **12 years at 6.0 %** annual interest, against
**USD 18,000,000** of equity; the offtaker will pay an **availability payment of USD 5,600,000 per
year for 25 years**; Kestrel's board evaluates offers at a discount rate of **8.0 %**. Two further
figures from the sibling domains are used here as given inputs rather than re-derived: Kestrel's
documented first-year cash available for debt service, `CFADS` **USD 6,384,000** (Domain 2, KA 2.2;
Domain 10, KA 10.1.1), which lets every schedule in this domain be read for its coverage
consequence, and the **USD 840,000** arrangement fee inside the funding envelope (Domain 6,
KA 6.2.1), which lets the domain's inverse-rate machinery be demonstrated on a real term sheet. All
of these numbers are used repeatedly below — by the end of the domain the reader can price every
side of Kestrel's deal, and every quoted figure ties to the digit across Domains 4, 6, 9 and 10.

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
5. **Interpretation.** Three years of compounding adds USD 1,971 over simple interest — under 2 %,
   which is precisely why the shortcut survives: at short horizons it is nearly right, and a
   professional who tests it only at short horizons will keep using it. The divergence is worth
   knowing as a curve rather than as a single fact. The ratio of compound to simple accumulation is
   **1.0159** at year 3, **1.0495** at year 5, **1.0986** at year 7, **1.1286** at year 8 — so the
   gap first passes ten per cent in year **7.0493** — **1.1994** at year 10 and **2.2828** at year
   25, where compounding reaches **6.848475** times the principal against simple interest's 3.0.
   Two invariants fall out of the same arithmetic and are worth carrying. First, the
   **interest-on-interest** term — the part of the growth that simple interest cannot see, being
   `(1+r)^n − 1 − rn` — is 0.0197 of the principal at year 3, 0.2109 at year 8, and passes **1.0**
   in year **15.1679**: at 8 %, interest on interest overtakes the entire original principal in the
   sixteenth year, and USD 100,000 has by then earned USD 114,594 of it. Second, the crossover year
   depends only on `r`, not on the amount, so the shape above is transferable to any principal at
   8 % and can be recomputed for any other rate in one line. The professional habit this example
   teaches: never extrapolate a short-horizon intuition to a long-horizon contract, and be
   specifically suspicious of any "conservative simplification" whose error grows with tenor —
   the direction of that error is almost always in the borrower's favour and against the model
   (MCQ 3.1-C makes the point as an examination item; KA 3.3.2 finds the same defect in escalation).

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
4. **Result.** **USD 356,493** (356,493.09 at full precision; indicatively ≈ SAR 1,336,849 at
   `USD 1 ≈ SAR 3.75`).
5. **Interpretation.** A five-year wait at 7 % costs the rebate just under 29 % of its face value.
   The number is a decision rule, not a valuation: USD 356,493.09 is the **indifference price**, so
   a counterparty offering USD 380,000 today should be accepted and one offering USD 330,000
   declined, and the whole of the professional judgment sits in the 7 %. Move the rate one point
   either way and the indifference price moves to **USD 373,629.09** at 6 % or **USD 340,291.60**
   at 8 % — a **USD 33,337.49** spread, **9.3515 %** of the value at 7 %, from two hundred basis
   points of opinion — which is why a review of any discounted number begins with the rate rather
   than the arithmetic. Two cautions belong with the result. The rate must reflect **this cash flow's** risk, not the project's average risk: a rebate
   payable by a creditworthy utility under a signed connection agreement is a different instrument
   from the project's equity, and discounting it at the project hurdle understates it — the
   risk-matching discipline Domain 4 (KA 4.1.1) sets out. And `PV(x)` is silent on **whether the
   money arrives at all**: discounting handles timing, not credit, and a counterparty who may not
   pay is a probability question that belongs in the cash flow or in an explicit credit adjustment,
   never smuggled into `r` without saying so (Domain 7, KA 7.4; Domain 11, KA 11.2).

**Common pitfall.** Discounting with simple interest — `500,000 / (1 + 0.07 × 5) = 370,370` —
overstates the value by USD 13,877 here. The error grows with horizon and rate; audit any model
whose discount factors were "simplified".

**Worked example 3.1.2b — reading an offer as a rate.**

1. **Setup.** The same USD 500,000 rebate, five years out. Rather than asking what the rebate is
   worth at Kestrel's rate, ask what rate the counterparty's cash offer implies. Two offers are on
   the table: **USD 380,000** and **USD 330,000**. What return is each offering Kestrel for giving
   up the wait?
2. **Formula.** Invert `PV(x) = x/(1+r)^n`: `r = (x / PV(x))^(1/n) − 1`. This is the third of the
   four TVM unknowns — value, rate, term, payment — and the one project finance uses most often
   under other names (yield, all-in cost, implied return).
3. **Substitution.** `r = (500,000/380,000)^(1/5) − 1 = 1.315789^0.2 − 1`; and
   `r = (500,000/330,000)^(1/5) − 1 = 1.515152^0.2 − 1`.
4. **Result.** The USD 380,000 offer implies **5.6422 %**; the USD 330,000 offer implies
   **8.6654 %**. The identity check: substituting the exact `PV(x)` of 356,493.09 returns
   **7.0000 %** — the rate the board started with, recovered to the last digit.
5. **Interpretation.** The inversion changes the conversation. "Is 380,000 enough?" is a matter of
   opinion; "the offer pays 5.6422 % for five years and our required return for this risk is 7 %"
   is a matter of record, and the second sentence is the one that survives a board minute. The
   arithmetic also exposes where the decision is genuinely difficult: the USD 330,000 offer implies
   **8.6654 %**, comfortably above the 7 % hurdle, so **taking the money is the value-creating
   choice even though it looks like the worse price** — the larger cash sum is the worse deal and
   the smaller one the better, which is exactly the reversal that intuition gets wrong and the
   reason the calculation is done at all. Three professional cautions. The implied rate is only
   comparable to a required return of the **same basis**: this is a nominal rate over a whole number
   of years with a single cash flow, so no timing or compounding adjustment is hiding in it, whereas
   an implied rate on a stream must be solved rather than derived in closed form (KA 3.2.3, WE
   3.2.3b). The inversion assumes the future amount is **certain**; where it is not, the implied
   rate is a promised yield and not an expected return, and the difference is the counterparty's
   default probability. And the sign convention matters: a solved rate above the hurdle favours
   **accepting cash today** for a receivable and **rejecting** it for a payable, because the same
   arithmetic run on an obligation Kestrel owes points the other way. Domain 4 (KA 4.1.2) develops
   the same inversion on a multi-period stream, where it acquires a name, `IRR`, and a set of
   pathologies this single-flow case cannot have.

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

> **Fig 3.1.2 — Discount-factor curves at 6 %, 10 % and 14 %.** Line chart, x-axis years 0–25,
> y-axis `DF(t)` 0–1. Three curves: `1/1.06^t` (reaching 0.233 at year 25), `1/1.10^t` (0.092),
> `1/1.14^t` (0.038), labelled at the right edge. A vertical marker at year 10 shows the spread
> (0.558 / 0.386 / 0.270). Caption: the rate choice dominates long-dated value. Source: PCI
> original. Alt text: three downward-curving discount-factor lines showing higher rates crushing
> far-future value toward zero over twenty-five years.

**Rounding discipline.** Factors are displayed to four decimals but **calculations use full
precision**. Multiplying a USD 900,000,000 programme cash flow by a four-decimal factor can move
results by tens of thousands; the display is for the reader, never for the arithmetic (see the
registry's decimal-arithmetic rule).

**Worked example 3.1.3b — the factor row that checks itself.**

1. **Setup.** Kestrel's senior facility runs 12 years at 6.0 %. Build the discount-factor row for
   `t` = 1 … 12, and establish the two relationships that let a reviewer validate the whole column —
   and the annuity factor that will price the loan in KA 3.2 — without re-adding twelve numbers.
2. **Formula.** `DF(t) = 1/(1+r)^t`. The annuity factor is the **sum** of the row,
   `AF(r,n) = Σ DF(t)` for `t` = 1 … `n`, and it satisfies the closed form
   `AF(r,n) = (1 − DF(n))/r`, which rearranges to the reviewer's identity
   **`AF(r,n) × r = 1 − DF(n)`**.
3. **Substitution.** `DF(1) = 1/1.06 = 0.943396`, `DF(2) = 0.889996`, … ,
   `DF(12) = 1/1.06¹² = 0.496969`. Adding all twelve gives **8.383844**. The closed form gives
   `(1 − 0.4969693636)/0.06`. The identity gives `8.3838439404 × 0.06` against `1 − 0.4969693636`.
4. **Result.** `Σ DF(t) = AF(0.06, 12) =` **8.383844** (8.3838439404 at full precision), and both
   sides of the identity equal **0.5030306364**. Repeating the exercise on Kestrel's 25-year
   evaluation horizon at 8 %: `DF(25) =` **0.1460179049**, `AF(0.08, 25) =` **10.674776**, and
   `10.6747761886 × 0.08 = 0.8539820951 = 1 − 0.1460179049`.
5. **Interpretation.** The identity is the cheapest audit tool in the domain.
   A model's annuity factor and its discount-factor row are usually computed in different places
   by different formulae — one a closed form, one a column — and the identity is the only check that
   ties them together, so it catches the specific defect that a column is right and a summary cell
   is wrong, or vice versa. It also has diagnostic content beyond pass or fail: **because
   `1 − DF(n)` can never exceed 1, `AF(r,n)` can never exceed `1/r`** — 16.6667 at 6 %, 12.5 at 8 %
   — which gives a reviewer an instant ceiling on any annuity factor and immediately condemns an
   annuity factor larger than the reciprocal of the rate, whose two usual causes are a tenor typed
   into the rate cell and a rate entered as 6 rather than 0.06. Two
   further readings. The ratio `AF(r,n) ÷ (1/r)` is exactly `1 − DF(n)`, so Kestrel's 25-year factor
   at 8 % already captures **85.3982 %** of what a perpetuity would be worth, and its 12-year factor
   at 6 % only **50.3031 %** of one: a quarter-century of contracted payments is most of forever,
   while a twelve-year loan is barely half of it. That single comparison explains why concession
   tails are worth arguing about and loan tails are not, and it is the arithmetic behind Domain 10's
   treatment of `PLCR` (KA 10.2.2) as real but weak security. And the row's own arithmetic is the
   reason for the rounding rule stated above:
   the four-decimal `DF(12)` of 0.4969 rather than 0.4969693636 shifts the derived `AF` by
   0.0011561 and Kestrel's instalment by roughly **USD 691** a year — small, visible, and entirely
   avoidable. The caution: an identity that holds is evidence about the *formulae*, not about the
   *rate*. A factor row built on the wrong `r` satisfies every check in this example perfectly.

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

**MCQ 3.1-F `[3.1.2 · Application]`** USD 250,000 is invested at 7 % compound for 6 years. Its
future value is closest to:
- A. USD 355,000
- B. USD 350,638
- C. USD 375,183 ✅
- D. USD 166,586

*Rationale:* `250,000 × 1.07⁶ = 375,183`. A is simple interest (`1 + 0.07 × 6`); B compounds
only 5 years; D divides instead of multiplying — discounting when the question asks for growth.

**MCQ 3.1-D `[3.1.3 · Recall]`** Which statement about discount factors is an invariant a
reviewer can test without knowing the project?
- A. `DF(t)` rises when cash flows are contracted
- B. `DF(t+1) = DF(t) / (1 + r)` for every `t` ✅
- C. `DF(t)` equals `1 − r × t`
- D. factors below 0.5 indicate an error

*Rationale:* Each factor is the prior factor discounted one more period — B holds for any `r`.
A confuses risk of flows with the factor row; C is the simple-interest approximation; D is false —
any long horizon at a normal rate passes 0.5 (see Fig 3.1.2).

**MCQ 3.1-E `[3.1.2 · Application]`** At 9 %, which is worth more today: USD 400,000 in 2 years,
or USD 470,000 in 4 years?
- A. the 470,000 — larger amounts always win
- B. the 400,000: PV 336,672 vs 332,960 ✅
- C. the 470,000: PV 395,590 vs 336,672
- D. they are equal at 9 %

*Rationale:* `400,000/1.09² = 336,672`; `470,000/1.09⁴ = 332,960` — the earlier, smaller amount
wins by USD 3,712. A ignores discounting entirely; C discounts the 470,000 across only two
periods instead of four; D asserts an equality the arithmetic denies.

**MCQ 3.1-G `[3.1.2 · Analysis]`** A counterparty offers **USD 380,000 today** to extinguish an
obligation to pay USD 500,000 in exactly five years. The annual rate of return the offer implies for
the party giving up the wait is closest to:
- A. 4.80 %
- B. 5.64 % ✅
- C. 6.32 %
- D. 7.00 %

*Rationale:* `(500,000/380,000)^(1/5) − 1 = 5.6422 %`. A divides the USD 120,000 premium by the
*future* amount and by five (`120,000/500,000/5`) — a simple return computed on the wrong base;
C annualises the 31.5789 % total premium by dividing by five, the simple-interest shortcut of KA
3.1.1 applied to a rate; D is the board's own required return, which is what the implied rate must
be *compared against*, not the answer to the question asked.

**MCQ 3.1-H `[3.1.3 · Comprehension]`** A reviewer wants one arithmetic test that ties a twelve-row
discount-factor column to the single annuity-factor cell that summarises it, without re-adding the
column. The correct relationship is:
- A. `AF(r,n) × r = 1 − DF(n)` ✅
- B. `AF(r,n) = n × DF(n)`
- C. `AF(r,n) × DF(n) = 1`
- D. `AF(r,n) = 1/r − DF(n)`

*Rationale:* At 6 % over 12 years, `8.383844 × 0.06 = 0.503031 = 1 − 0.496969` (WE 3.1.3b). B
averages the row by its last term and gives 5.963632; C inverts a relationship that holds for no
`r` (here 4.166514); D drops the division of `DF(n)` by `r` from the closed form and gives
16.169697 — each a plausible-looking rearrangement, and each falsifiable in one cell.

**MCQ 3.1-I `[3.1.2 · Evaluation]`** Kestrel will receive a USD 500,000 connection rebate in five
years from a creditworthy utility under a signed connection agreement. A board member proposes
discounting it at the project's 8.0 % appraisal rate "for consistency"; the treasurer proposes 6.0 %,
the rate on the project's own senior debt. The values are **340,291.60** at 8 % and **373,629.09** at
6 %. The sound recommendation is:
- A. use 8.0 % — one project, one discount rate, and consistency is the stronger discipline
- B. use a rate that reflects *this* cash flow's risk, which is a contracted receivable from a strong counterparty rather than the project's equity risk; state the rate chosen, and put any doubt about payment into the cash flow or an explicit credit adjustment rather than inside `r` ✅
- C. use 6.0 %, because the rebate will be applied to reduce senior debt, which makes the debt rate the opportunity cost
- D. use 8.0 % and additionally reduce the 500,000 for the risk of non-payment, capturing both effects

*Rationale:* the discount rate must match the risk of the flow being discounted, and discounting
handles timing rather than credit (3.1.2) — the two hundred basis points between the proposals move
this single receipt by **33,337.49**, or **9.3515 %** of its value at 7 %. A applies a portfolio
convention to an instrument of different risk and so understates it. C is the most defensible of the
wrong answers and still picks its rate from the *use* of the money rather than the risk of receiving
it. D uses the right technique for credit — an explicit haircut — and then leaves in place a rate set
for a riskier flow, charging the same risk twice.

### Self-check — KA 3.1

1. *Why must discount factors decline monotonically?* — Because `(1+r)^t` grows with `t` for any
   positive `r`; a rising factor implies a negative rate or a broken formula.
2. *Your model shows `DF(4) = 0.6830` and `DF(5) = 0.6209` at 10 %. One cell check confirms both.
   Which?* — `0.6830 / 1.10 = 0.6209`: each factor is the prior factor divided by `(1+r)`.
3. *A colleague says "8 % for three years is 24 %".* — Only under simple interest; compounded it is
   `1.08³ − 1 = 25.97 %`, and the gap widens every further year.
4. *What is the largest annuity factor possible at 8 %, and why?* — 12.5, the perpetuity factor
   `1/r`, because `AF(r,n) × r = 1 − DF(n)` and `1 − DF(n) < 1` for every finite `n`.
5. *An offer of cash today implies a 8.6654 % return against a 7 % hurdle. Should it be accepted for
   a receivable, and for a payable?* — Accepted for a receivable (the market is paying more than the
   risk requires) and refused for a payable, where the same rate is the cost of settling early.

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
availability regimes) is worth `(1 + r)` times the ordinary annuity; a **deferred annuity** starts
after a gap and is discounted twice — as a stream, then back across the deferral. Misreading the
payment convention in a concession agreement misprices the whole stream by one period's
discounting — check the contract, not the habit (Domain 12, KA 12.2).

**Worked example 3.2.1b — the lease quoted in advance.**

1. **Setup.** Kestrel's operations base is leased at USD 500,000 per year for 5 years, payable
   **in advance**. Discount rate 8.0 %. Value the obligation.
2. **Formula.** `PV(due) = A × AF(r, n) × (1 + r)`, with `A` = 500,000, `r` = 0.08, `n` = 5.
3. **Substitution.** `AF(0.08, 5) = 3.992710`; ordinary value `500,000 × 3.992710 = 1,996,355`;
   due value `1,996,355 × 1.08`.
4. **Result.** **USD 2,156,063** (2,156,063.42). The in-advance convention adds USD 159,708 —
   exactly one period's discounting on the whole stream.
5. **Interpretation.** Eight per cent of the stream's value hangs on one word in the lease.
   Reading "payable annually in advance" as an ordinary annuity is the commonest annuity mistake
   in diligence reviews (Domain 13, KA 13.1) — and it always flatters the tenant's model. The
   correction is worth holding as an invariant rather than as a formula: **the ratio of an
   annuity-due to an ordinary annuity is exactly `(1 + r)`, whatever `A` and whatever `n`** — here
   `2,156,063.42 ÷ 1,996,355.02 = 1.08` to every digit — so a reviewer who suspects a convention
   error does not need to rebuild the stream. Divide the model's answer by the ordinary-annuity
   answer and read the result: 1.00 means arrears, `1 + r` means advance, and anything else means a
   third defect is present as well. Two cautions. The `(1 + r)` factor is **one period's** rate, so
   on a quarterly-in-advance lease the adjustment is `1 + r/4` and not `1 + r` — applying the annual
   factor to a quarterly stream overstates the correction roughly fourfold, which is the error that
   follows the error. And the direction is asymmetric in consequence: on a **receivable** stream the
   in-advance reading raises value and flatters the seller, while on a **payable** stream — a lease,
   an availability payment Kestrel owes, an O&M fee — it raises the liability, so the same mistake
   is optimistic in one place and prudent in the other, and a model containing both conventions
   read wrongly does not self-correct. The lease clause, not the model's habit, decides
   (Domain 12, KA 12.2).

**Worked example 3.2.1c — the deferred availability stream.**

1. **Setup.** A grantor variant offers Kestrel the same USD 5,600,000 × 25-year availability
   supplement, but with the **first payment at the end of year 4** (a three-year deferral while
   the plant ramps up). Discount rate 8.0 %.
2. **Formula.** `PV = A × AF(r, n) / (1 + r)^d`, `d` = 3 (the stream's own valuation point sits
   at the end of year 3, one period before its first payment).
3. **Substitution.** `5,600,000 × 10.674776 = 59,778,747` (the stream valued at end-year 3);
   `59,778,747 / 1.08³ = 59,778,747 / 1.259712`.
4. **Result.** **USD 47,454,296**.
5. **Interpretation.** The three-year wait costs USD 12.3 million — a fifth of the stream's
   value — which is precisely what the grantor saves by deferring. Any negotiation over payment
   *timing* is a negotiation over value at compound rates; Case study A prices the whole family
   of such trades.

**The indexed stream — the annuity family's fourth member.** Almost no real project stream is level.
Availability payments index, tariffs escalate, O&M fees rise with a labour index, and a level annuity
is therefore the exception rather than the rule. A stream growing at a constant rate `g` per period
is a **growing (indexed) annuity**, and it does not need a new machine — it needs one substitution:

```
PV(indexed) = A × AF(r*, n)        where   r* = (1 + r)/(1 + g) − 1
```

with `A` the amount in **period-0 money** and the period-`t` payment `A(1 + g)^t`. `r*` is the
**growth-adjusted (real, if `g` is inflation) discount rate**. The limiting case as `n → ∞`, valid
only where `r > g`, is the growing perpetuity `PV = A(1 + g)/(r − g)`.

**Worked example 3.2.1d — the indexed availability payment, and the identity that prices it.**

1. **Setup.** A grantor variant offers Kestrel the 25-year availability supplement of
   **USD 5,600,000** in year-0 terms, but **fully indexed at 2.5 % a year**, so the first payment is
   5,740,000 and the last is larger again. Discount rate 8.0 %. Value the indexed stream, and value
   the reshaping that would leave the grantor no worse off than the level USD 5,600,000 offer.
2. **Formula.** `PV = Σ A(1+g)^t/(1+r)^t` for `t` = 1 … 25, which equals `A × AF(r*, n)` with
   `r* = (1+r)/(1+g) − 1`. For the value-neutral reshaping, solve for the base payment `A′` such
   that `A′ × AF(r*, 25) = 5,600,000 × AF(r, 25)`.
3. **Substitution.** `r* = 1.08/1.025 − 1 = 0.0536585366`; `AF(r*, 25) = 13.591332`;
   `PV = 5,600,000 × 13.591332`. Direct summation of all 25 terms is computed alongside as the
   check. For the reshaping, `A′ = 59,778,746.66 ÷ 13.591332`.
4. **Result.** Indexed stream **USD 76,111,457** (76,111,457.28 by both routes, identical to the
   cent). Against the level stream's **USD 59,778,747**, indexation at 2.5 % is worth
   **USD 16,332,711**, a **27.3219 %** uplift. The value-neutral reshaping is a base payment of
   **USD 4,398,299** — a first payment of **4,508,257** rising to **8,154,201** in year 25, crossing
   the level 5,600,000 in **year 10** (5,630,195).
5. **Interpretation.** The identity is the most useful single line in this Knowledge Area, and it
   deserves to be read in both directions. Forwards, it says an indexed stream needs no new formula:
   **escalating the flows at `g` and discounting at `r` gives exactly the same answer as leaving the
   flows in period-0 money and discounting at `r*`** — which is the Fisher consistency rule of KA
   3.3.1 arriving early, in stream form, and is why the same arithmetic reappears there priced as an
   audit defect (WE 3.3.1b). Backwards, it is a trap detector. The characteristic error is to
   escalate the flows **and** discount them at the growth-adjusted rate, which counts the indexation
   twice: here that produces **USD 99,768,254**, an overstatement of **USD 23,656,797** or
   **31.0818 %** — a third of the value of the largest single item in the concession, from one
   substitution made twice. The defect survives review because both halves look right in isolation
   and neither cell is wrong on its own. The second reading is commercial. Indexation is not a detail of drafting; at 2.5 % on a
   25-year stream it is worth **27.3219 %** of the stream, so a grantor who agrees to full indexation
   without repricing the base payment has made a concession larger than most of what the parties
   spent the negotiation arguing about — which is exactly why real grantors do the reshaping, and
   why the arithmetic above is a bid-preparation calculation rather than an academic one. Note what
   the reshaping actually does: **it holds present value constant and moves risk.** Both packages are
   worth 59,778,747 to Kestrel at 8 %, but the indexed one pays **19.4954 %** less in year one,
   crosses over in year 10, and pays a nominal total of **USD 153,991,976** against the level
   stream's 140,000,000 — so it is worse for early `DSCR` (Domain 10, KA 10.2.1) and better for
   inflation protection, and the choice between them is a risk-allocation decision that a present
   value cannot make. Three cautions. The formula requires `g` **constant**; a contractual index
   with caps, floors or a lag is not a constant `g`, and KA 3.3.2 shows what each does to the value.
   `r > g` is required for the perpetuity form and merely advisable for the annuity form: at `r ≤ g`
   the finite sum still exists but the value becomes extremely sensitive to `n`, and a model that
   prices a 40-year stream with `g` close to `r` is reporting an artefact of its own tenor
   assumption. And **`r*` must never be presented as the project's discount rate.** It is an
   arithmetic device inside one valuation; a board paper that quotes 5.3659 % as Kestrel's hurdle
   has confused a substitution with a policy.

### 3.2.2 Loan schedules

**The three canonical shapes.** A loan of principal `P` over `n` periods at rate `r` can be
scheduled three ways, and a project finance leader must read all three on sight:

| Shape | Rule | Debt-service profile | Where seen |
|---|---|---|---|
| **Annuity (equal instalment)** | Payment `A = P × r / (1 − (1+r)^−n)` each period | Level total; interest share falls, principal share rises | Most project term loans; mortgages |
| **Level principal** | Principal `P/n` each period + interest on balance | Front-loaded total, declining | Some ECA and development-bank facilities (Domain 9) |
| **Bullet** | Interest only; principal entire at maturity | Low until the final balloon | Bonds; mini-perm structures; refinancing plays (Domain 15) |
| **Repayment holiday (grace)** | Interest only for `k` periods, then annuity over the remaining `n − k` | Low, then a step up to a heavier level | Ramp-up periods; construction-to-operations transitions |

**Worked example 3.2.2 — Kestrel's senior loan.**

1. **Setup.** USD 42,000,000, 12 annual instalments, 6.0 % — annuity shape. Find the instalment
   and build the first three schedule rows.
2. **Formula.** `A = P × r / (1 − (1+r)^−n)`, with `P` = 42,000,000, `r` = 0.06, `n` = 12. Each
   row: interest = opening balance × `r`; principal = `A` − interest; closing = opening − principal.
3. **Substitution.** `A = 42,000,000 × 0.06 / (1 − 1.06⁻¹²) = 42,000,000 / 8.383844`.
4. **Result.** `A` = **USD 5,009,635** per year (5,009,635.23; indicatively ≈ SAR 18,786,132).
   Schedule:

   | Year | Opening balance | Interest (6 %) | Principal | Closing balance |
   |---|---|---|---|---|
   | 1 | 42,000,000 | 2,520,000 | 2,489,635 | 39,510,365 |
   | 2 | 39,510,365 | 2,370,622 | 2,639,013 | 36,871,351 |
   | 3 | 36,871,351 | 2,212,281 | 2,797,354 | 34,073,997 |

5. **Interpretation.** The instalment never changes; its composition does — year 1 is 50.30 %
   interest, and by the final year almost all principal, with interest down to 283,564. Two checks a
   reviewer runs instantly: the closing balance after year 12 must be exactly zero (a residual means
   a formula error), and total principal across all rows must equal `P`. Both need one honest
   qualification that separates a real model check from a superstition. The instalment is
   **5,009,635.233987873150895272537** at full precision; run the schedule on the *printed*
   5,009,635.23 and the year-12 closing balance is **USD 0.06**, not zero. That is not an error — it
   is twelve rounding decisions, and a schedule that closes at six cents on a 42,000,000 facility is
   correct while one that closes at 6,000 is not. The reviewer's rule is therefore **"zero to within
   accumulated display rounding, and materially zero to a stated tolerance"**, and the tolerance
   should be written into the model-check sheet rather than left to taste (Domain 6, KA 6.4). The
   composition path itself carries the professional content. Interest falls from 2,520,000 to 283,564
   — a factor of **8.8869** — while principal rises from 2,489,635 to 4,726,071, and the ratio of
   final-year to first-year principal is exactly `(1 + r)^(n−1) = 1.06¹¹ =` **1.8983**, an invariant
   that holds for any annuity loan and gives a reviewer a one-line test of the whole principal
   column. The interest share of the first instalment, **50.30 %**, is not a coincidence either: it
   is `1 − DF(n)` from WE 3.1.3b, since first-year interest divided by the instalment is
   `P × r ÷ (P × r/(1 − DF(n)))`. Two consequences that matter downstream. Because interest falls
   and the instalment does not, the **tax shield declines every year** while debt service does not,
   so a project's after-tax cash profile is not flat even when its debt service is (Domain 6,
   KA 6.2.2; Domain 9, KA 9.2.2). And because the loan is level while `CFADS` in a ramping project is
   not, **the annuity's constant instalment guarantees that the binding coverage period is the
   earliest one** — Kestrel's `DSCR` of `6,384,000 ÷ 5,009,635.23 =` **1.2743** is a year-one number
   and every later year is better on level cash, which is exactly the property Domain 10 (KA 10.1.3)
   exploits when it sculpts. This schedule is the direct input to Kestrel's debt service line — and
   therefore to its `DSCR`, `LLCR` and `PLCR` — in Domains 6 and 10.

**Worked example 3.2.2b — the same loan, level-principal.**

1. **Setup.** Kestrel's ECA co-lender offers the same USD 42,000,000 over 12 years at 6.0 % but
   on a **level-principal** schedule. Build the first two rows and the final row, and compare
   lifetime interest across all three shapes.
2. **Formula.** Principal each year `P/n = 42,000,000/12 = 3,500,000`; interest = opening
   balance × 6 %; total = principal + interest.
3. **Substitution.** Year 1: `3,500,000 + 42,000,000 × 0.06`. Year 2:
   `3,500,000 + 38,500,000 × 0.06`. Year 12: `3,500,000 + 3,500,000 × 0.06`.
4. **Result.** Year 1 **USD 6,020,000** · year 2 **USD 5,810,000** · year 12 **USD 3,710,000**
   — a declining profile. Lifetime interest: level-principal **USD 16,380,000** · annuity
   **USD 18,115,623** · bullet **USD 30,240,000**.
5. **Interpretation.** Level-principal is the cheapest shape in total interest because principal
   retires fastest — but its year-1 debt service is USD 1,010,365 *heavier* than the annuity's,
   exactly when a ramping project is cash-poorest. The two crossovers are worth having in the head,
   because they are where the argument between a treasurer and a credit officer actually happens.
   The **annual payment** crossover is in **year 6**: level-principal costs more than the annuity in
   years 1 to 5 and less from year 6, the exact parity point falling at **year 5.8113**. The
   **cumulative cash** crossover is much later: level-principal is still 653,648 of cumulative cash
   ahead of the annuity at the end of year 10 and only moves behind during **year 11**, finishing
   1,735,623 cheaper. So for ten of the twelve years the "cheaper" shape has taken more money out of
   the project, and the 1,735,623 lifetime advantage is collected almost entirely in the last two
   years. Read against Kestrel's coverage that is decisive: at documented `CFADS` of 6,384,000 the
   level-principal year-1 `DSCR` is `6,384,000 ÷ 6,020,000 =` **1.0605**, against the annuity's
   **1.2743** — the same loan, the same rate, the same lender, and one shape fails a 1.20× covenant
   in its first test period while the other clears it. Shape selection is a cash-timing decision, not
   a cost minimisation: Domain 10 (KA 10.1) sizes debt against the early-year coverage this
   choice determines, and the honest professional statement of the trade is that **level-principal
   buys 1,735,623 of interest with 1,010,365 of first-year coverage headroom**, which is a good
   trade for a project with flat contracted cash from day one and a bad one for anything that ramps.

**Worked example 3.2.2c — the repayment holiday, and the cliff it builds.**

1. **Setup.** Kestrel's ramp-up is slower than the base case assumes, so the sponsor asks for a
   **three-year repayment holiday**: interest only in years 1 to 3, then full amortisation of the
   unchanged USD 42,000,000 across years 4 to 12. Same 6.0 %, same 12-year maturity. Find the
   instalment, the lifetime interest, and the coverage consequence.
2. **Formula.** Interest-only service = `P × r`. Amortising instalment over the remaining periods
   `A_h = P × r/(1 − (1+r)^−(n−k))` with `n − k` = 9. Lifetime interest =
   `k × P × r + [(n − k) × A_h − P]`. Coverage per period = `CFADS ÷ debt service`.
3. **Substitution.** Years 1–3: `42,000,000 × 0.06 = 2,520,000`. Years 4–12:
   `42,000,000 × 0.06/(1 − 1.06⁻⁹) = 42,000,000 ÷ 6.801692`. Interest:
   `3 × 2,520,000 + (9 × A_h − 42,000,000)`.
4. **Result.** Instalment years 4–12 **USD 6,174,934** (6,174,933.87). Lifetime interest
   **USD 21,134,405**, against the plain annuity's **18,115,623** — the holiday costs
   **USD 3,018,782** more. Coverage: year 1 `DSCR` **2.5333**, year 4 `DSCR` **1.0339**.
5. **Interpretation.** The holiday does exactly what it is asked to do and creates exactly the
   problem nobody asks about. Years 1 to 3 are luxurious — a `DSCR` of 2.5333 against a 1.20×
   covenant is more headroom than the project will ever have again — and year 4 is a **cliff**: the
   instalment jumps **145.0371 %** from 2,520,000 to 6,174,934 in a single step, coverage falls to
   **1.0339**, and the project misses a 1.30× requirement by **USD 1,643,414** of `CFADS`
   (`6,174,933.87 × 1.30 = 8,027,414.03` needed against 6,384,000 available). The structure is not
   merely tight; it is **unbankable as drawn**, and it would have been described in the sponsor's own
   term sheet as a concession *in the sponsor's favour*. That is the professional point: a holiday
   granted on an unchanged maturity does not reduce debt service, it **concentrates** it, and the
   concentration lands on whichever operating year the sponsor was least willing to model. Three
   further readings. The interest cost of the holiday, 3,018,782, is not a fee anybody negotiated —
   it is the arithmetic consequence of leaving 42,000,000 outstanding for three extra years at 6 %,
   and it is *knowable in advance to the cent*, which means a sponsor who accepts a holiday without
   pricing it has given away a computable amount for a cash-flow convenience. The fix is structural
   rather than arithmetic: **extend the maturity by the holiday** and the instalment returns to a
   manageable level, which is why holidays and tenor extensions are negotiated together and why a
   holiday on a fixed concession end-date is nearly always a bad trade (the concession, not the
   lender, is the binding constraint — Domain 5, KA 5.4). And the reviewer's test is a period one:
   **a holiday makes the *first amortising* period the binding coverage test, not the first period**,
   so any covenant model that reports "minimum `DSCR`" from year 1 onwards without inspecting the
   step is reporting the wrong minimum. Domain 10 (KA 10.1.2) sizes debt against exactly this
   binding period; Domain 14 (KA 14.4) meets the same cliff where a construction-period holiday runs
   into a delayed commercial operation date.

**Worked example 3.2.2d — what is still owed, and how to know twice.**

1. **Setup.** Kestrel's sponsor is approached in year 7 about a refinancing, and separately needs to
   report debt outstanding for a covenant certificate. On the annuity schedule of WE 3.2.2, what is
   the principal outstanding immediately after the year-7 instalment — and how is that figure proved
   without the schedule?
2. **Formula.** Two independent routes must agree. The **recursion**: carry the schedule forward
   row by row, `closing = opening − (A − opening × r)`. The **closed form**: the outstanding balance
   after `k` payments is the present value of the payments that remain,
   `B_k = A × AF(r, n − k)`.
3. **Substitution.** Recursion: twelve rows of `opening × 0.06`, subtracted from 5,009,635.23,
   as tabulated in WE 3.2.2. Closed form: `5,009,635.23 × AF(0.06, 5) = 5,009,635.23 × 4.212364`.
4. **Result.** Closed form **USD 21,102,406.02**; recursion on the printed instalment
   **USD 21,102,406.08** — agreement to **six cents**, the accumulated display rounding of seven
   instalments. The same test at other dates: after year 3, **34,073,997.24** against
   **34,073,997.29**; after year 5, **27,965,694.73** against **27,965,694.78**.
5. **Interpretation.** The balance formula does four distinct jobs. It is a **check**: two routes computed from different data — one from a rate and a
   remaining tenor, one from a running balance — must agree, and where they do not, the defect is in
   the schedule and not in the formula, because the closed form has nowhere to hide an error. It is a
   **reporting tool**: covenant certificates, `LLCR` denominators and security-value tests all need
   debt outstanding at a date, and Domain 10 (KA 10.2.2) uses precisely this figure. It is a
   **prepayment price**: `B_k` is what the borrower owes to walk away, before any break cost or
   prepayment fee the facility may add, which sit *on top of* `B_k` rather than
   instead of it. And it is a **refinancing base**: the
   year-7 figure of 21,102,406 is the amount a refinancing must raise, and the fact that it is
   **50.2438 %** of the original 42,000,000 after seven of twelve years tells the sponsor immediately
   that the annuity's principal retirement is back-loaded — half the loan is still outstanding at
   58.33 % of the term. Two cautions. The closed form assumes the schedule has run **exactly as
   contracted**: one missed or partial payment, one capitalised amount, one cash sweep, and the
   recursion is right while the formula is wrong, so the formula is a check on an unbroken schedule
   and an *error* on a restructured one (Domain 15, KA 15.3 handles the restructured case). And the
   remaining interest is not the remaining balance: after year 7 Kestrel still owes
   `5 × 5,009,635.23 =` 25,048,176.15 of cash, of which **USD 3,945,770.13** is interest not yet
   accrued — a figure that appears in no ledger and is not a liability, which is why balance-sheet
   debt and total future debt service are different numbers and a covenant that names one must not be
   tested with the other (Domain 2, KA 2.1).

> **Fig 3.2.2 — Three shapes, one loan: annual debt service under annuity, level-principal and
> bullet.** Line/step chart, x-axis years 1–12, y-axis USD 0–45m (log-free, broken axis note for
> the bullet's year-12 spike). Annuity: level at 5,009,635. Level-principal: 6,020,000 declining
> to 3,710,000. Bullet: 2,520,000 flat, then 44,520,000 at year 12. Legend with lifetime interest
> totals (16.38m / 18.12m / 30.24m). Source: PCI original. Alt text: comparison of level,
> declining and balloon-shaped repayment profiles for the same loan, showing the bullet's large
> final payment.

> **Fig 3.2.1 — Anatomy of an annuity loan: Kestrel's USD 42,000,000, 12 years at 6 %.** Stacked
> bar chart, x-axis years 1–12, y-axis USD; each bar totals 5,009,635, split into an interest
> portion (2,520,000 in year 1, shrinking to ≈ 283,564 in year 12) and a principal portion
> (2,489,635 in year 1, growing to ≈ 4,726,071 in year 12). A horizontal rule marks the level
> instalment. Source: PCI original. Alt text: stacked bars showing a constant annual loan payment
> whose interest share shrinks and principal share grows across twelve years.

> **Fig 3.2.3 — Outstanding principal, not debt service: Kestrel's USD 42,000,000 under four
> shapes.** Line chart, x-axis years 0–12, y-axis principal outstanding USD 0–45m. Four series from
> a common 42,000,000 at year 0. **Annuity** (brand blue): 39,510,365 · 36,871,351 · 34,073,997 ·
> 31,108,802 · 27,965,695 · 24,634,001 · 21,102,406 · 17,358,915 · 13,390,815 · 9,184,628 ·
> 4,726,071 · 0, with the year-7 balance of **21,102,406** marked. **Level principal** (slate,
> dashed): a straight line falling 3,500,000 a year through exactly 21,000,000 at year 6.
> **Bullet** (crimson): flat at 42,000,000 for twelve years, then to zero. **Three-year repayment
> holiday** (amber, dashed): flat at 42,000,000 through year 3, then 38,345,066 · 34,470,836 ·
> 30,364,153 · 26,011,068 · 21,396,798 · 16,505,672 · 11,321,078 · 5,825,409 · 0 — visibly steeper
> than the annuity and crossing below it late in the term. A faint horizontal rule at 21,000,000
> marks half the original principal so the four half-life dates can be read off. Header states
> lifetime interest: level principal 16,380,000 · annuity 18,115,623 · holiday 21,134,405 · bullet
> 30,240,000. Source: PCI original. Alt text: four descending lines showing how fast principal
> actually retires under four repayment shapes, with the bullet flat across the whole term and the
> holiday flat for three years before falling more steeply than the annuity.

**Reading the balance path rather than the payment path.** Figure 3.2.2 shows what the project pays;
Figure 3.2.3 shows what it still owes, and the second is the picture a credit officer wants, because
**lifetime interest is `r` multiplied by the area under the balance path** and refinancing exposure is
the height of that path at maturity. The four areas order themselves exactly as the interest totals
do, which is why the ordering needs no memorising: level principal encloses the least
(16,380,000 ÷ 0.06 = 273,000,000 balance-years), then the annuity (18,115,623), then the holiday
(21,134,405), then the bullet (30,240,000, the maximum possible for the tenor because the balance
never falls at all). Two diagnostic readings a reviewer takes straight off the chart. The **half-life
of the principal** — the date at which half the loan has been retired — is year **6.0000** for level
principal, **7.0281** for the annuity, **8.0833** for the holiday and never for the bullet; a shape
whose half-life sits past the midpoint of its tenor has a back-loaded credit exposure whatever its
instalment looks like. And the **height at maturity** is the refinancing question in one number: nil
for three of the shapes and 42,000,000 for the fourth.

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
   and the differences compound over a 12-year loan. Three things are worth extracting. The
   **increments shrink**: annual to semi-annual costs **9.0000** basis points, semi-annual to
   quarterly a further **4.6355**, quarterly to monthly a further **3.1426**, and monthly to daily
   only **1.5350** — so the whole range from once a year to continuous compounding (KA 3.A.1) is
   **18.3655 basis points**, of which the first
   step alone is half. That bounds the argument: a negotiation over compounding frequency is a
   negotiation over at most 18 basis points at this rate, which is worth knowing before spending
   negotiating capital on it, and worth knowing so that a *misread* frequency is recognised as a
   small error rather than assumed to be a large one. The **direction is invariant**: `EAR ≥ i_nom`
   always, with equality only at `m` = 1, so a model reporting an effective rate below its nominal
   has a defect, full stop (3.A.4). And the practical rule stands independent of the arithmetic: term
   sheets are compared on effective rates, never on quoted nominals — and loan models must use the
   **periodic** rate `i_nom/m` with `n × m` periods, not the EAR with `n` years, or debt service will
   be misstated (Domain 6, KA 6.3; WE 3.2.3c prices exactly that substitution on Kestrel). The
   caution that belongs here is a scale one: 18 basis points on Kestrel's 42,000,000 is small, but
   the same misread on a floating-rate facility repriced quarterly across a portfolio is a systematic
   error rather than a rounding one, and systematic errors do not average out.

**Worked example 3.2.3b — the fee-inclusive cost, solved rather than added.**

1. **Setup.** Kestrel's mandate is USD 42,000,000 at 6.0 % over 12 years, with an **arrangement fee
   of USD 840,000** (2.0 % of the facility) deducted from the first drawdown — the figure sitting in
   the funding envelope at Domain 6, KA 6.2.1. A second lender offers the same amount and tenor at
   **6.15 % with no fee**. Which facility is cheaper, and by how much?
2. **Formula.** A fee cannot be added to a rate; it must be **solved out of the stream**. The all-in
   effective cost is the rate `r_all` satisfying `Σ A/(1 + r_all)^t = net proceeds`, where `A` is the
   contractual instalment (computed on the **gross** principal at the **contract** rate) and net
   proceeds are what the project actually receives. Solve numerically — there is no closed form.
3. **Substitution.** Instalment unchanged at `42,000,000 × 0.06/(1 − 1.06⁻¹²) =` 5,009,635.23; net
   proceeds `42,000,000 − 840,000 =` 41,160,000. Solve
   `Σ 5,009,635.23/(1 + r_all)^t = 41,160,000` for `t` = 1 … 12. For the comparison,
   `42,000,000 × 0.0615/(1 − 1.0615⁻¹²)`.
4. **Result.** All-in cost of the fee facility **6.3704 %** (6.370442 %) — a premium of **37.04
   basis points** over the 6.00 % headline. The rival at 6.15 % with no fee has an all-in cost of
   exactly **6.1500 %** and an instalment of **USD 5,050,914** (5,050,913.67, or 41,278.44 a year
   more than 5,009,635.23). **The 6.15 % facility is cheaper, by 22.04 basis points.**
5. **Interpretation.** The result reverses the intuition that a lower coupon is a lower cost, and it
   does so for a reason worth naming: an upfront fee is paid **once, at time zero, undiscounted**,
   while a margin is paid across twelve years on a declining balance, so a fee is a far more
   expensive way to deliver the same nominal amount of money to a lender. The exchange rate between
   the two is the number to carry: on this facility **each 1.00 % of upfront fee is worth 18.38 basis
   points of margin** (a 420,000 fee prices at 6.183800 %), so 2.0 % of fee is worth about 37 basis
   points and a borrower comparing "6.00 % plus 2 %" against "6.15 % flat" is comparing 37 against
   15 and should say so in one sentence. Run the inversion the other way and the negotiating position
   sharpens further: the fee that would make the 6.00 % facility exactly as expensive as the 6.15 %
   rival is **USD 343,244**, or **0.8172 %** of the facility — so the arranger's 840,000 is
   **USD 496,756 more** than the market-equivalent fee, and *that* is the number to put on the table,
   not a complaint about fees in general. Four cautions.
   The comparison is only valid where the two facilities have the **same amount, tenor, shape and
   security**; change any of those and the all-in rates are no longer commensurable, which is why
   Domain 9 (KA 9.3.3) builds a formal like-for-like tranche sheet before quoting all-in costs and why
   this example is about the *method* rather than about term-sheet negotiation. Fees that are
   **capitalised** rather than deducted behave differently — the project borrows the fee, so principal
   and instalment both rise while proceeds stay at 42,000,000, and the all-in cost is not the same
   number. Commitment fees on **undrawn** amounts cannot enter this calculation at all without a
   drawdown profile, because their base is unknown until the construction schedule is fixed
   (Domain 14, KA 14.2). And the solved rate is a **cost of debt, not a return**: it is what Kestrel
   pays, and the arranger's own yield differs because the arranger may sell down part of the loan
   (Domain 13, KA 13.4). What the example establishes for the rest of the book is the discipline:
   **no rate enters a comparison until it has been solved from the actual stream.**

**Worked example 3.2.3c — paying twice a year at the same rate, and the error that hides in it.**

1. **Setup.** Kestrel's lenders propose **semi-annual** debt service instead of annual: 24 payments,
   the nominal 6.0 % applied as 3.0 % a period. A modeller, wanting to keep the model annual, instead
   uses the **effective annual rate of 6.09 %** across 12 annual periods. Compute the true
   semi-annual schedule, the modeller's schedule, and the difference.
2. **Formula.** True: `S = P × (i_nom/m)/(1 − (1 + i_nom/m)^−(n×m))` with `m` = 2, `n × m` = 24;
   annual cash = `2S`. Modeller's: `A_w = P × EAR/(1 − (1 + EAR)^−n)` with `EAR = 1.03² − 1` and
   `n` = 12.
3. **Substitution.** `S = 42,000,000 × 0.03/(1 − 1.03⁻²⁴) = 42,000,000 ÷ 16.935542`. Then
   `A_w = 42,000,000 × 0.0609/(1 − 1.0609⁻¹²)`.
4. **Result.** Semi-annual instalment **USD 2,479,991** (2,479,991.47), annual cash
   **USD 4,959,983** (4,959,982.94), lifetime interest **USD 17,519,795**. The modeller's annual
   schedule gives **USD 5,034,383** a year and lifetime interest **USD 18,412,592** — overstating
   annual debt service by **USD 74,400** and lifetime interest by **USD 892,797**. For reference, the
   annual-payment facility of WE 3.2.2 costs 5,009,635.23 a year and 18,115,623 of interest.
5. **Interpretation.** Two results, and the surprising one first. **Paying twice a year at the same
   nominal rate is cheaper in total interest, not dearer** — 17,519,795 against the annual schedule's
   18,115,623, a saving of 595,828 — because principal begins retiring six months earlier and every
   subsequent interest charge is levied on a smaller balance. The frequency of *payment* and the
   frequency of *compounding* pull in opposite directions, and conflating them is the error that
   makes this topic feel counter-intuitive: more frequent **compounding** raises cost (WE 3.2.3),
   more frequent **amortisation** lowers it. The honest price of the semi-annual structure is
   therefore not interest but **liquidity**: the project must find 2,479,991 twice a year instead of
   5,009,635 once, and its annual-equivalent cost — the single year-end payment stream with the same
   present value at the same rate — is **5.8188 %** against the annual facility's 6.0000 %, so a
   sponsor who values the mid-year cash at more than 18 basis points should prefer the annual shape
   and one who does not should prefer the semi-annual. That is a treasury decision with a computable
   threshold, which is the only kind worth having. The second result is the modelling one. The
   EAR-with-annual-periods substitution is **not a conservative simplification, it is a different
   loan** — it overstates annual debt service by 74,400 and understates `DSCR` from **1.2871** to
   **1.2681**, so on this occasion the error happens to be prudent on coverage while being wrong on
   cash, and on a sculpted or seasonal profile the direction reverses. There is no rule of thumb
   available: the substitution must simply not be made. Note that it is undetectable by every check
   in Toolkit 3.T.2 — the schedule closes at zero, total principal equals the loan, and the payment
   recomputes correctly against its own stated rate — which places it squarely in the
   **document-conformance** layer of a model audit rather than the arithmetic layer (Domain 13,
   KA 13.2.1). The facility agreement names the periods; the model implements the periods named.

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
| **Annuity / annuity-due** | Level periodic stream, in arrears / in advance; the due form is worth `(1 + r)` times the ordinary. |
| **Annuity factor `AF(r,n)`** | `(1 − (1+r)^−n)/r`; PV of 1 per period for `n` periods; can never exceed `1/r`. |
| **Perpetuity / growing perpetuity** | Level stream forever, `PV = A/r`; growing at `g < r`, `PV = A(1+g)/(r − g)`. |
| **Growing (indexed) annuity** | Stream growing at `g`; `PV = A × AF(r*, n)` with `r* = (1+r)/(1+g) − 1`. |
| **Growth-adjusted rate `r*`** | `(1+r)/(1+g) − 1`; an arithmetic device inside one valuation, never a project hurdle. |
| **Annuity, level-principal, bullet, holiday** | The four canonical loan shapes a term sheet offers. |
| **Repayment holiday (grace)** | Interest-only periods followed by amortisation over the shortened remainder; concentrates rather than reduces service. |
| **Outstanding balance `B_k`** | `A × AF(r, n − k)`; the PV of the payments that remain, and the prepayment base. |
| **Balloon / refinancing risk** | The bullet's maturity principal and the risk of the rate then prevailing. |
| **Nominal rate / EAR** | Quoted annual rate with compounding frequency / its once-a-year equivalent; `EAR ≥ i_nom` always. |
| **All-in effective cost** | The rate solved from the actual stream against net proceeds; fees are never added to a rate. |

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

**MCQ 3.2-F `[3.2.1 · Analysis]`** A wayleave pays USD 800,000 per year; the discount rate is
8 %. Modelling it as a 30-year annuity instead of a perpetuity understates its value by:
- A. USD 993,773 — the present value of the post-year-30 tail ✅
- B. nothing material — thirty years is effectively forever
- C. USD 4,000,000 — one-third of the perpetuity value
- D. it overstates value, because perpetuities are riskier

*Rationale:* Perpetuity `800,000/0.08 = 10,000,000`; 30-year annuity `800,000 × 11.257783 =
9,006,227`; the gap — 9.9 % of value — is the discounted tail. B waves away a million dollars;
C invents a fraction; D confuses valuation arithmetic with a risk adjustment that belongs in the
rate, not the formula choice.

**MCQ 3.2-D `[3.2.1 · Application]`** A 5-year, USD 500,000-per-year lease payable **in advance**
is valued at 8 %. Its present value is closest to:
- A. USD 1,996,355
- B. USD 2,156,063 ✅
- C. USD 2,500,000
- D. USD 1,848,477

*Rationale:* Annuity-due = ordinary annuity × `(1+r)`: `500,000 × 3.992710 × 1.08 = 2,156,063`.
A prices it as an ordinary annuity (the classic misread); C is the undiscounted total; D divides
the ordinary value by 1.08 — the adjustment applied backwards.

**MCQ 3.2-E `[3.2.2 · Analysis]`** For the same principal, tenor and rate, which repayment shape
has the lowest lifetime interest, and why?
- A. the annuity — its instalments are level, so interest is averaged down
- B. the bullet — deferral shrinks the money's time exposure
- C. level-principal — the balance falls fastest, so less principal is outstanding for less time ✅
- D. all three are equal because rate and tenor are equal

*Rationale:* Interest is rate × outstanding balance × time; level-principal retires balance
fastest (Kestrel: 16.38m vs 18.12m annuity vs 30.24m bullet). A's "averaging" is not a
mechanism; B reverses the truth — the bullet maximises exposure; D ignores the balance path
entirely.

**MCQ 3.2-G `[3.2.1 · Application]`** A 25-year availability payment of USD 5,600,000 in year-0
terms is **fully indexed at 2.5 %** a year; the discount rate is 8.0 %. Its present value is closest
to:
- A. USD 59,778,747
- B. USD 76,111,457 ✅
- C. USD 99,768,254
- D. USD 104,363,636

*Rationale:* `r* = 1.08/1.025 − 1 = 5.3659 %`, `AF(r*, 25) = 13.591332`, `PV = 5,600,000 ×
13.591332 = 76,111,457` (WE 3.2.1d). A values the stream as level and ignores the indexation
entirely; C escalates the flows **and** discounts at `r*`, the double-count of WE 3.2.1d; D applies the growing-perpetuity form `A(1+g)/(r − g)` and so
values a stream that never ends.

**MCQ 3.2-H `[3.2.2 · Evaluation]`** Kestrel's sponsor obtains a three-year repayment holiday on the
unchanged USD 42,000,000 / 12-year / 6.0 % facility, against a **1.30× minimum `DSCR`** covenant. The
instalment for years 4 to 12 becomes USD 6,174,934 and year-4 `DSCR` falls to 1.0339 against
documented `CFADS` of 6,384,000. The soundest professional conclusion is:
- A. the holiday is cash-neutral, since the same principal is repaid over the same maturity and the lender's return is unchanged
- B. the holiday is prudent: a year-1 `DSCR` of 2.5333 against a 1.30× covenant shows the structure is comfortably covered
- C. the holiday concentrates rather than reduces debt service, costs USD 3,018,782 of extra interest and misses the covenant by USD 1,643,414 of `CFADS` ✅
- D. the holiday reduces lifetime interest, because principal repayment is deferred and interest accrues on a smaller average balance

*Rationale:* Lifetime interest rises from 18,115,623 to 21,134,405 and the binding test moves from
year 1 to year 4, where `6,174,933.87 × 1.30 = 8,027,414.03` is needed against 6,384,000 available
(WE 3.2.2c), so the holiday must be paired with a maturity extension or a smaller drawing. A ignores
the three extra years of interest on an undiminished balance; B reads a single non-binding period as
the covenant position, the specific error a holiday induces; D reverses the direction — deferring
principal leaves the balance *larger*, not smaller, so interest rises.

**MCQ 3.2-I `[3.2.3 · Application]`** Kestrel's USD 42,000,000 / 12-year / 6.0 % facility carries an
USD 840,000 arrangement fee deducted from proceeds. Its all-in effective cost is closest to:
- A. 6.0000 %
- B. 6.1667 %
- C. 6.3704 % ✅
- D. 8.0000 %

*Rationale:* Solving `Σ 5,009,635.23/(1 + r)^t = 41,160,000` over 12 years gives 6.3704 % (WE
3.2.3b). A quotes the headline and ignores the fee; B spreads the 2.0 % fee straight-line across
twelve years and adds 16.67 basis points to the coupon — the "amortise the fee" error, which omits
the time value of paying it at once; D adds the whole 2.0 % to the rate, an error of a full 163 basis
points in the same direction.

**MCQ 3.2-J `[3.2.2 · Comprehension]`** The closed form `B_k = A × AF(r, n − k)` gives Kestrel's
principal outstanding after seven of twelve years as **21,102,406**. Which statement shows what the
formula is, and is not?
- A. it is the total of the instalments still to be paid, so after year 7 Kestrel owes 25,048,176
- B. it is the present value of the payments that remain, which makes it a check on a schedule that has run exactly as contracted and simply wrong on one that has been restructured, swept or capitalised ✅
- C. it is the prepayment price, so a borrower settling early pays `B_k` and nothing further
- D. it equals the original principal less seven years of principal at the loan's average rate of retirement

*Rationale:* the formula discounts the remaining contractual payments, which is why it agrees with the
schedule recursion to a few cents and why any departure from the contract breaks the formula rather
than the schedule (3.2.2). A confuses cash still to be paid with principal outstanding: the difference,
**3,945,770.13**, is interest not yet accrued and is not a liability. C is nearly right and materially
wrong — `B_k` is the *base*, and break costs or prepayment fees sit on top of it. D describes
level-principal retirement; an annuity's principal is back-loaded, which is why **50.2438 %** of the
loan is still outstanding after 58.33 % of the term.

**MCQ 3.2-K `[3.2.3 · Analysis]`** Two like-for-like offers on the same 42,000,000, 12-year,
annuity-shaped, equally secured facility: 6.00 % with an 840,000 arrangement fee deducted from
proceeds — an all-in cost of 6.3704 % — or 6.15 % with no fee. The treasurer recommends the first,
"because the coupon is lower and the fee is a one-off". The best response is:
- A. agree: the 6.00 % coupon governs debt service, and the fee is a transaction cost outside the cost of funds
- B. take the 6.15 % facility, which is cheaper by **22.04** basis points — and put a number on the table, since the fee that would equalise the two is **343,244**, so the arranger is asking **496,756** more than the market-equivalent fee ✅
- C. take the 6.15 % facility, because an upfront fee is always a more expensive way to pay a lender than margin
- D. reject both and require the arranger to convert the fee to margin at 16.67 basis points, being the 2.0 % fee spread across the twelve-year tenor

*Rationale:* a fee is paid once and undiscounted while a margin is paid across a declining balance, so
each 1.00 % of upfront fee costs **18.38** basis points of margin on this facility — making 2.0 % of
fee worth about 37 and the 15-basis-point coupon difference the cheaper of the two (WE 3.2.3b). A
treats the instalment as the cost of funds and ignores that the project receives only 41,160,000. C
reaches the right verdict from an overstated rule: the exchange rate is computable, and a fee below
343,244 would make the 6.00 % facility the cheaper one. D is the "amortise the fee" error — dividing a
fee by the tenor omits the time value of paying it at time zero, and 16.67 basis points is under half
the 37.04 the stream actually gives.

### Self-check — KA 3.2

1. *What two instant checks validate any amortising schedule, and what tolerance applies?* — Final
   closing balance zero and total principal equal to the loan, both **to within accumulated display
   rounding**: on Kestrel the printed instalment leaves USD 0.06, which is correct, while 6,000
   would not be.
2. *Why is an annuity-due worth `(1+r)` × the ordinary annuity?* — Every payment arrives one
   period earlier, so each escapes one period of discounting; the ratio is `(1 + r)` whatever `A`
   and `n`, which makes it a one-line convention test.
3. *A term sheet quotes "5.9 % monthly"; another "6.0 % annual". Which is dearer?* — The first:
   `(1 + 0.059/12)¹² − 1 = 6.06 %` effective, above 6.00 %.
4. *State the two ways to compute debt outstanding after seven of twelve years, and why both are
   needed.* — The schedule recursion and `B_k = A × AF(r, n − k)`; they are computed from different
   data, so agreement is evidence and disagreement localises the defect to the schedule.
5. *Why can an upfront fee not be converted to a margin by dividing it by the tenor?* — Because it
   is paid once and undiscounted while a margin is paid across a declining balance: on Kestrel each
   1.00 % of fee is worth **18.38** basis points, not the 8.33 that division would give.
6. *A stream indexes at `g` and is discounted at `r`. Name the defect that produces a value
   31.0818 % too high.* — Escalating the flows and also discounting at `r*`, counting the indexation
   twice (WE 3.2.1d).

---

## Knowledge Area 3.3 — Real-world adjustments: inflation, escalation and currency

*Topics: 3.3.1 nominal and real rates · 3.3.2 inflation and escalation · 3.3.3 currency effects ·
3.3.4 day-count conventions.*

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
   **17.4757 basis points** high.
5. **Interpretation.** Seventeen basis points sounds academic until it is applied to a 25-year
   stream, and WE 3.3.1b prices it exactly: on Kestrel's 5,600,000 support stream the shortcut
   understates value by **USD 1,204,318**, which is **0.2151** of one year's payment. That is the
   right way to carry the error — as a fraction of a payment rather than as a percentage of a rate —
   because it is the form in which a board will recognise it. Note the direction: the subtraction
   shortcut always gives a real rate that is **too high** (it omits the `i_real × π` cross term), so
   it always **understates** the value of a real stream, and a bidder using it is systematically
   bidding low. Note also that the error scales with `i_nom × π`, so it is trivial at 2 % inflation
   and serious at 10 %: at a 9 % nominal rate and 10 % inflation the shortcut gives −1.00 % against
   an exact −0.9091 %, and the sign of the real rate — the thing the whole calculation exists to
   establish — is preserved only by accident. Models must live entirely in one world — nominal cash
   flows with nominal rates, or real with real. **Mixing them is the single most common TVM defect
   found in model audits** (Domain 6, KA 6.4; Domain 13, KA 13.2), and the next example is the reason
   that sentence is a finding rather than an opinion.

**Worked example 3.3.1b — the consistency rule, priced four ways.**

1. **Setup.** Kestrel's 25-year support stream is worth **USD 5,600,000 a year in year-0 purchasing
   power**. Inflation is 3.0 %, so the nominal receipt in year `t` is `5,600,000 × 1.03^t`. The
   board's nominal hurdle is 9.0 %, its real hurdle the 5.8252 % just derived. Value the stream the
   two consistent ways and the two inconsistent ways, and price each defect.
2. **Formula.** Consistent nominal: `Σ 5,600,000 × 1.03^t/1.09^t`. Consistent real:
   `5,600,000 × AF(i_real, 25)`. Defect 1 — real flows at the nominal rate:
   `5,600,000 × AF(0.09, 25)`. Defect 2 — nominal flows at the real rate:
   `Σ 5,600,000 × 1.03^t/1.058252^t`.
3. **Substitution.** `i_real = 1.09/1.03 − 1 = 0.0582524272`; `AF(i_real, 25) = 12.998413`;
   `AF(0.09, 25) = 9.822580`. The nominal route is summed term by term as the independent check on
   the identity.
4. **Result.**

   | Treatment | Present value | Against the truth |
   |---|---|---|
   | Nominal flows at 9.0000 % | **72,791,113** | — |
   | Real flows at 5.8252 % | **72,791,113** | identical to the cent |
   | Real flows at 9.0000 % (defect) | **55,006,446** | **−17,784,667**, −24.4325 % |
   | Nominal flows at 5.8252 % (defect) | **100,366,400** | **+27,575,287**, +37.8828 % |
   | Real flows at the 6.00 % shortcut | **71,586,794** | −1,204,318, −1.6545 % |

5. **Interpretation.** The first two rows are the entire content of the Fisher relation and they are
   worth stating as a theorem rather than a rule of thumb: **the two consistent treatments are
   arithmetically identical, not merely close.** 72,791,112.66 both ways, to the cent, which means a
   modeller can choose whichever world the source documents live in and be provably right — and
   means that a difference between a real model and a nominal model of the same project is not a
   basis difference to be reconciled but an **error to be found**. The two defective rows are why
   this is the domain's most expensive defect. Discounting real flows at a nominal rate deducts
   inflation twice and destroys **24.4325 %** of the value — on this stream 17,784,667, or **3.1758
   years' worth of payments**, and on a project of Kestrel's size enough to turn a fundable
   concession into a rejected one. The mirror error inflates value by **37.8828 %** and is the more
   dangerous of the two, because a model that overstates a support stream by 27.6 million produces a
   bid that wins and then fails to service its debt. Note the **asymmetry**: the two errors are not
   equal and opposite, since compounding is multiplicative, so a reviewer cannot reason that "the
   errors will roughly cancel across a portfolio". Three professional points. **The defect is
   invisible in the outputs and obvious in the inputs**, which is why the audit control is a single
   declared basis per model — the "one world" line in the assumption register (3.T.1) — rather than a
   plausibility check on the answer. **The defect's most damaging form is the partial one**: a
   model that indexes revenue and forgets to index operating cost, or indexes both and leaves a
   fixed-price O&M contract nominal, is mixing worlds *within* the cash flow rather than between the
   flow and the rate, and it produces an error of no predictable sign — which is why Domain 6
   (KA 6.4) makes the convention list a model-check artefact and Domain 8 (KA 8.2) escalates each
   cost line against its own named index. And the choice of world is not free even though the answer
   is identical: **lenders' covenants, tax computations and depreciation allowances are nominal
   constructs**, so a real model must be converted to nominal before any covenant or tax line can be
   computed at all, which is why project models are nearly always built nominal and interpreted in
   real terms rather than the reverse (Domain 6, KA 6.2.2; Domain 10, KA 10.2.1).

> **Fig 3.3.3 — One stream, four treatments: what mixing nominal and real costs.** Column chart,
> four bars, y-axis present value USD 0–110m. The stream is 5,600,000 a year of year-0 purchasing
> power for 25 years, inflation 3.0 %, nominal hurdle 9.0 %, real hurdle 5.8252 %. Bars: "Nominal
> flows, nominal 9.0 %" **72,791,113** (brand blue, labelled *correct*); "Real flows, real 5.8252 %"
> **72,791,113** (brand blue, labelled *correct — identical*); "Real flows, nominal 9.0 %"
> **55,006,446** (crimson, labelled **−24.4325 %**); "Nominal flows, real 5.8252 %" **100,366,400**
> (crimson, labelled **+37.8828 %**). A dashed ink rule across the plot at 72,791,113 labelled "the
> consistent answer"; a fine amber rule at **71,586,794** over the first two columns labelled
> "Fisher subtraction shortcut at 6.00 %: −1,204,318". Source: PCI original. Alt text: four columns
> showing two identical correct present values flanked by one far too low and one far too high,
> demonstrating that mixing nominal and real terms moves value by a quarter to a third in opposite
> directions.

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
(3.T.3) exists for exactly this. Three of those mechanics have arithmetic that this Knowledge Area
must supply rather than name, because each is easy to concede without pricing:

- an **indexation cap** replaces `g` with `min(g, cap)` in every period, so its value is the
  difference between two growing annuities;
- an **indexation lag** — indexing on the *previous* period's published figure, the usual
  arrangement because indices are published in arrears — shifts the whole escalated series back one
  period, and is therefore worth exactly one period's indexation on the whole stream;
- **partial indexation** at a weighting `w` (a tariff indexed at, say, 70 % of CPI) replaces `g`
  with `w × g`, not with the index itself, and the difference compounds.

**Worked example 3.3.2b — the cap, the lag, and what each is worth.**

1. **Setup.** Kestrel's O&M contract is **USD 2,700,000** a year at base date — the non-power element
   of the documented USD 4,500,000 of cash operating cost (Domain 2, KA 2.2; the remaining
   1,800,000 is electricity, passed through under the water-purchase agreement — Domain 11,
   KA 11.2.2) — for the 25-year operating life. The sponsor's forecast index runs at **4.0 %**; the
   draft contract offers full indexation. Kestrel's negotiators want a **3.0 % cap**, and separately
   want the usual **one-year publication lag** written in. Value the uncapped obligation, the capped
   obligation and the lagged-and-capped obligation at the 8.0 % project discount rate, and price each
   concession.
2. **Formula.** Each is a growing annuity (KA 3.2.1): `PV = A × AF(r*, n)` with
   `r* = (1 + r)/(1 + g) − 1`. Capped: the same with `g` = 3.0 %. Lagged: the period-`t` payment
   becomes `A(1 + g)^(t−1)` rather than `A(1 + g)^t`, so the whole stream divides by `(1 + g)`.
3. **Substitution.** Uncapped: `r* = 1.08/1.04 − 1 = 3.846154 %`, `AF = 15.879244`,
   `PV = 2,700,000 × 15.879244`. Capped: `r* = 1.08/1.03 − 1 = 4.854369 %`, `AF = 14.301981`,
   `PV = 2,700,000 × 14.301981`. Lagged: the capped figure ÷ 1.03.
4. **Result.** Uncapped **USD 42,873,960** · capped **USD 38,615,349** · capped and lagged
   **USD 37,490,630**. The **cap is worth USD 4,258,610** (9.9329 % of the uncapped obligation); the
   **lag is worth a further USD 1,124,719**; together they remove **USD 5,383,329** from the
   obligation. In the final year the capped price is **5,653,200** against the uncapped
   **7,197,758** — a single-year saving of 1,544,558.
5. **Interpretation.** The cap is worth **USD 4,258,610** — 7.10 % of Kestrel's entire 60,000,000
   capital envelope — and it is a one-line drafting change with no cash cost to either party at
   signature. That is the finding: **indexation mechanics are among the highest-value-per-word terms
   in a project contract, and they are routinely agreed without being valued.** Three
   readings make the number usable. As a **level annual equivalent** the cap is worth
   `4,258,610.20 ÷ AF(0.08, 25) =` **USD 398,941 a year**, which is the form to put beside a fee or a
   price concession when trading it — a counterparty asking 300,000 a year for the cap is offering a
   good deal and one asking 600,000 is not. As a **conditional** value it is worth nothing if the
   index runs at or below 3.0 % and rises steeply above it: at a 5.0 % outturn the same cap is worth
   **USD 9,157,382**, more than double, so the cap is an option on inflation and its value is convex
   in the outturn — which means valuing it at the single-point forecast systematically *understates*
   it, and a proper treatment stresses the index (Domain 7, KA 7.4; Domain 11, KA 11.3). And the
   lag's value is an exact identity worth remembering because it needs no computation: **a
   one-period indexation lag on a fully indexed stream is worth `1 − 1/(1 + g)` of the stream** —
   here 2.9126 %, giving 1,124,719 on 38,615,349 — so a model that ignores a contractual lag
   overstates an indexed cost, and understates an indexed revenue, by that fixed fraction. Three
   cautions. A cap is only worth this much if it is **cumulative rather than annual in effect**: a
   contract capping each year's increase at 3.0 % is what has been valued here, whereas a contract
   capping the increase at 3.0 % *but permitting catch-up* when the index later falls back is worth
   materially less, and the two are distinguished by a subordinate clause. A cap on a **cost** helps
   the payer while a cap on a **revenue** index hurts the receiver, so the same clause is worth
   +4,258,610 on the O&M contract and −4,258,610 on a tariff, and a project holding both must value
   them together rather than celebrating one. And caps, floors and collars are **enforceability- and
   drafting-sensitive** where the named index ceases publication, is rebased, or is redefined by its
   publisher: the fallback mechanism is the clause that matters most and is valued least, and its
   drafting is a matter for **qualified legal counsel in the governing jurisdiction** rather than for
   the model (Domain 12, KA 12.2).

> **Fig 3.3.2 — Compound versus simple escalation of a USD 10,000,000 cost at 4 %.** Line chart,
> x-axis years 0–25, y-axis USD 10m–27m. Compound curve `10m × 1.04^t` reaching 26.66m at year
> 25; simple line `10m × (1 + 0.04t)` reaching 20.0m. Shaded gap between them, annotated
> "+6.66m by year 25". Source: PCI original. Alt text: two rising lines showing compound
> escalation pulling away from simple escalation, with the gap between them shaded and growing
> to a third of the total by year twenty-five.

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
   Domain 12 allocates. The direction is the part worth internalising, because it is
   counter-intuitive and it decides how a forward strip looks on a page: the **higher-interest-rate
   currency trades at a forward discount**, so SAR — the 5.5 % currency here — buys fewer USD forward
   than spot, moving from 3.7500 to 3.7679. That is not a market view about Saudi Arabia; it is the
   only rate at which a bank can hedge the position without giving money away, and any model showing
   a *gain* from receiving in the higher-rate currency and converting forward has either mispriced
   the forward or is quietly forecasting. **Assumption honesty:** actual pegs, spreads and
   convertibility restrictions are jurisdiction- and time-specific; every currency figure in this
   book is illustrative.

**Worked example 3.3.3b — two routes to one hedged value, and why they must agree.**

1. **Setup.** A grantor variant pays Kestrel **SAR 20,000,000 a year for five years**, annually in
   arrears, while Kestrel's debt service and its board reporting are in USD. Spot
   `USD 1 = SAR 3.7500`; five-year money costs **5.0 % in USD** and **5.5 % in SAR**. Value the
   stream in USD by both available routes.
2. **Formula.** **Route 1 — discount then convert:** value the stream in SAR at the SAR rate, then
   convert the single figure at spot: `PV_USD = [C × AF(i_SAR, n)] ÷ S`. **Route 2 — convert then
   discount:** convert each year's receipt at that year's forward
   `F_t = S × [(1 + i_SAR)/(1 + i_USD)]^t`, then discount the resulting USD flows at the USD rate:
   `PV_USD = Σ (C/F_t)/(1 + i_USD)^t`.
3. **Substitution.** Route 1: `AF(0.055, 5) = 4.270284`; `20,000,000 × 4.270284 = 85,405,690` SAR;
   `÷ 3.7500`. Route 2: forwards **3.7679 · 3.7858 · 3.8038 · 3.8219 · 3.8401**, giving USD receipts
   **5,308,057 · 5,282,900 · 5,257,863 · 5,232,944 · 5,208,143**, each discounted at 5.0 %.
4. **Result.** Route 1 **USD 22,774,851** (22,774,850.54). Route 2 **USD 22,774,851** — the same
   figure to the cent. Individual discounted amounts: 5,055,292 · 4,791,746 · 4,541,940 · 4,305,156 ·
   4,080,717.
5. **Interpretation.** The agreement is not a coincidence and it is not an approximation; it is
   **covered interest parity, and it is the reason a currency conversion cannot create or destroy
   value on its own.** That single fact retires a whole family of bad arguments heard in project
   reviews: "we should take payment in the stronger currency", "converting at forward rates improves
   the return", "the FX gain funds the contingency". If two internally consistent routes to the same
   hedged cash flow give different answers, the difference is a defect — a spot rate used where a
   forward belongs, a foreign stream discounted at the domestic rate, or an inconsistent tenor — and
   the reviewer's test is to run both routes and demand agreement, which costs one column. Note where
   the real exposure actually lives, because parity removes only the arithmetic question. **Route 2
   requires the forwards to be *contracted*, not merely computed**: an uncontracted forward is a
   forecast wearing a formula, and the position remains open. The residual risks after a full hedge
   are the ones that matter in practice — **basis** (the hedge tenor rarely matches the cash-flow
   tenor exactly), **counterparty credit** on the hedge provider, the **cost** of the hedge itself
   (parity is a mid-market identity; a real bank quotes a spread on both sides, and the spread is the
   only part of this calculation that is genuinely a cost), and **convertibility and transfer** — the
   ability to move money at all, which no forward can hedge and which is a political-risk question
   (Domain 11, KA 11.3; Domain 12 allocates it). The professional caution: parity holds where **both
   currencies' money markets are freely accessible at the quoted rates**, which is exactly what fails
   in the jurisdictions where projects most need hedging, and pegged-currency arrangements are policy
   choices that can be changed. Currency-mismatch structures, exchange-control consents and the
   enforceability of hedge collateral are matters for **qualified legal and tax counsel in the
   relevant jurisdictions**; no treatment in this book should be read as applying universally.

### 3.3.4 Day-count conventions

**The problem.** "Three months' interest" is not one number. Facility agreements specify a
**day-count convention** — how days are counted and over what year they are divided — and the
convention moves real money on large balances:

| Convention | Rule | Typical home |
|---|---|---|
| **30/360** | Every month 30 days, year 360 | Bonds, some term loans |
| **actual/360** | Actual days elapsed, year 360 | Money markets, most floating-rate loans |
| **actual/365** | Actual days elapsed, year 365 | Sterling markets, some jurisdictions |

**Worked example 3.3.4 — one quarter, three conventions.**

1. **Setup.** Interest accrues on Kestrel's USD 42,000,000 drawn balance at 6.0 % for one
   calendar quarter that contains **92 actual days**. Compute the interest under each convention.
2. **Formula.** Interest = balance × rate × (days counted / year basis).
3. **Substitution.** 30/360: `42,000,000 × 0.06 × 90/360`. actual/360:
   `× 92/360`. actual/365: `× 92/365`.
4. **Result.** 30/360 **USD 630,000** · actual/365 **USD 635,178** · actual/360 **USD 644,000**.
5. **Interpretation.** The spread between conventions is USD 14,000 on one quarter of one
   drawdown — and actual/360 is systematically the most expensive because it counts every real
   day over a short year. Models must implement the convention *named in the facility agreement*
   (Domain 12), and the assumption register (3.T.1) records it per instrument; "6 % divided by
   4" is not a convention, it is a guess. Note which comparisons are and are not fair here: 30/360
   and actual/360 differ **because the numerator differs** (90 against 92, a calendar accident that
   reverses in February), while actual/360 and actual/365 differ **because the denominator differs**,
   which is a systematic uplift and not an accident. Only the second of those two differences persists
   over a full year, and WE 3.3.4b computes what it is worth.

**Worked example 3.3.4b — the same convention across a whole year, and across the facility.**

1. **Setup.** Take Kestrel's USD 42,000,000 balance at 6.0 % and step back from the quarter to the
   term. What does the actual/360 convention cost over one full 365-day year, what quoted rate is it
   equivalent to, and what does it do to the 12-year annuity schedule and its coverage?
2. **Formula.** Over a full year, actual/360 charges `balance × rate × 365/360`, so the convention is
   arithmetically a rate uplift of `365/360 = 1.0138889` — an **effective simple annual rate** of
   `i_nom × 365/360`. Rebuild the annuity on that rate: `A′ = P × r′/(1 − (1 + r′)^−12)`.
3. **Substitution.** `42,000,000 × 0.06 × 365/360` against `42,000,000 × 0.06`. Then
   `r′ = 0.06 × 365/360 = 0.0608333333` and
   `A′ = 42,000,000 × 0.0608333333/(1 − 1.0608333333⁻¹²)`.
4. **Result.** One year of interest **USD 2,555,000** under actual/360 against **USD 2,520,000** under
   30/360 — an uplift of **USD 35,000**, or **1.3889 %** of all interest. The convention is equivalent
   to a quoted rate of **6.0833 %** — **8.33 basis points** above the headline. Across the twelve-year
   annuity the instalment rises from 5,009,635.23 to **USD 5,032,548** (5,032,547.52, +22,912 a year)
   and lifetime interest from 18,115,623 to **USD 18,390,570** — **USD 274,947** more. `DSCR` falls
   from **1.2743** to **1.2685**. In a leap year the same balance accrues **USD 2,562,000**.
5. **Interpretation.** The convention is worth **8.33 basis points** and appears in no rate
   comparison anyone runs. That is the professional point, and it generalises: **the day-count basis
   belongs in the all-in cost calculation of WE 3.2.3b, on the same footing as a fee**, because
   1.3889 % of all interest on Kestrel's facility is USD 274,947 — **73.82 %** of one year's covenant
   headroom at a 1.20× requirement (`6,384,000 − 1.20 × 5,009,635.23 =` 372,437.72), and more than
   the difference many borrowers spend weeks negotiating out of the margin.
   Three readings. The uplift is **exactly `365/360` and independent of the rate, the balance and the
   tenor**, which makes it the easiest sensitivity in the domain to carry in the head: any actual/360
   facility costs 1.3889 % more interest than the same facility on a 365-day basis, and 1.6667 % more
   than 30/360 in a 366-day year (`366/360`). The `DSCR` consequence of **0.0058** is small in
   isolation and is exactly the kind of small consistent bias that matters on a structure sized to
   1.20×, because it is present in every period rather than in one. And the convention interacts with
   the payment calendar: on 30/360 every period is identical, so a model can use a single periodic
   rate, whereas on actual/360 or actual/365 **each period's day count differs**, February and leap
   years included, so a properly built model needs a date-driven interest calculation rather than a
   rate divided by a frequency — which is a model-architecture requirement (Domain 6, KA 6.1) and not
   a refinement. The caution: conventions are **contractual, not conventional**. Market practice
   differs by currency, product and jurisdiction and changes over time; the only authority is the
   facility agreement's own definition, and where a facility is silent or ambiguous on the basis, the
   point is one for **qualified legal counsel in the governing jurisdiction** before the model is
   built on an assumption.

### AI in this domain — the systematic view

TVM is deterministic, which makes it the safest place in finance to use machine assistance and the
most dangerous place to trust it unchecked: a wrong exponent produces a *plausible* number, not an
absurd one. The domain's governed workflow, applied wherever Domains 4–16 discount anything:

1. **AI proposes** — drafts the factor table, schedule or escalated series.
2. **Deterministic checks** — the sixteen invariants of 3.A.4, of which four catch the errors a
   machine assistant specifically makes: `AF(r,n) × r = 1 − DF(n)` (a generated factor column against
   its own summary cell), `B_k = A × AF(r, n − k)` against the recursion (a generated schedule against
   itself), the real-versus-nominal identity (a generated indexed stream, where the double-count of
   WE 3.2.1d is the single most likely machine output), and `EAR ≥ i_nom` (a frequency conversion).
   Plus one hand-recomputed cell per block, by a different method than the one generated.
3. **Assumption register** — rate, basis (nominal/real), compounding frequency, **payment
   frequency**, day-count basis, timing convention, index (with lag, cap and weighting) and
   currency of every stream written down (toolkit 3.T.1). This is where machine assistance is
   weakest and it is not a modelling weakness: a copilot cannot know which convention the facility
   agreement names, and it will produce a defensible schedule against the wrong one without
   signalling any uncertainty (WE 3.2.3c is the archetype — every check passes, the document is not
   implemented).
4. **The professional decides and remains accountable** — no AI-produced number reaches a board
   paper, bid or lender report without a named human owner (Domain 16, KA 16.4).

**What must not be delegated, stated precisely.** The three inverse operations of this domain —
solving for a rate (WE 3.1.2b, 3.2.3b), solving for a term, and solving for a value-neutral payment
(Case study A) — are where machine assistance is most useful and most dangerous, because a numerical
solver returns a plausible root without telling anyone which root, whether the function was monotone,
or whether the stream it solved was the stream in the contract. Require the **stream** to be shown
alongside the solved rate, not merely the rate; require the solution to be **verified by
substitution** (discount the stream at the solved rate and confirm it returns the net proceeds to the
cent, as WE 3.2.3b does); and record the **net-proceeds definition** used, because that single
definitional choice moves the answer more than any arithmetic in the calculation.

### Key terms — KA 3.3

| Term | Meaning |
|---|---|
| **Nominal / real rate** | Money-terms rate / purchasing-power rate; linked by Fisher. |
| **Fisher relation** | `(1+i_nom) = (1+i_real)(1+π)`; the subtraction shortcut always overstates `i_real`. |
| **Consistency rule** | One world per model; the two consistent treatments are identical to the cent, so a difference is an error and not a basis. |
| **Inflation `π` / escalation `e`** | General price drift / specific contractual price growth. |
| **Indexation** | Contractual escalation by a published index, with lags, caps, floors and weightings. |
| **Indexation cap / lag / weighting** | `min(g, cap)` per period · index published in arrears, worth `1 − 1/(1+g)` of the stream · partial indexation at `w × g`. |
| **Spot / forward rate** | Exchange rate now / contracted for a future date. |
| **Covered interest parity** | Forward ≈ spot × interest-ratio; the no-arbitrage forward, and the reason two routes to one hedged value agree. |
| **Day-count basis** | The contractual numerator and denominator for accruing interest; actual/360 is a `365/360` uplift on all interest. |

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

**MCQ 3.3-F `[3.3.1 · Application]`** A model shows a nominal cash flow of USD 2,000,000 in
year 5; inflation is 3 %. Its value in today's purchasing power (real terms) is:
- A. USD 2,000,000 — real and nominal are equal at year 5
- B. USD 1,725,218 ✅
- C. USD 1,700,000
- D. USD 2,318,548

*Rationale:* `2,000,000 / 1.03⁵ = 1,725,218` — deflating by the price level, not discounting for
time value (that happens separately, at a real rate). A ignores five years of inflation; C
deflates simply (`2,000,000 × (1 − 0.03 × 5) = 1,700,000`); D multiplies by `1.03⁵` — inflating
instead of deflating.

**MCQ 3.3-D `[3.3.4 · Application]`** A USD 42,000,000 balance accrues at 6 % over a 92-day
quarter. Under **actual/360** the interest is:
- A. USD 630,000
- B. USD 635,178
- C. USD 644,000 ✅
- D. USD 620,548

*Rationale:* `42,000,000 × 0.06 × 92/360 = 644,000`. A is 30/360 (90/360); B is actual/365;
D uses 90/365 — mixing the two conventions' halves.

**MCQ 3.3-E `[3.3.2 · Analysis]`** A 25-year operating model escalates O&M at "4 % simple" to
be "conservative". The effect is:
- A. conservative — simple escalation always overstates cost
- B. anti-conservative — compound escalation exceeds simple by an amount that grows every year, so late-life costs are materially understated ✅
- C. neutral — the choice only redistributes cost between years
- D. correct — operating contracts always escalate simply

*Rationale:* Escalation compounds (contracts index on last year's indexed price): by year 25 the
compound multiple `1.04²⁵ ≈ 2.67` far exceeds the simple `2.00`. A claims the reverse; C ignores
the widening gap; D asserts a contractual universal that KA 3.3.2's indexation discipline
contradicts.

**MCQ 3.3-G `[3.3.1 · Analysis]`** A 25-year stream worth USD 5,600,000 a year **in year-0
purchasing power** is valued against a 9.0 % nominal hurdle with inflation at 3.0 %. Which statement
is correct?
- A. discounting the nominal (escalated) flows at 9.0 % and discounting the level real flows at 5.8252 % both give USD 72,791,113 — they are arithmetically identical ✅
- B. discounting the level real flows at 9.0 % is correct, and gives USD 55,006,446
- C. discounting the nominal flows at 5.8252 % is correct, and gives USD 100,366,400
- D. the real treatment is an approximation of the nominal treatment and the two differ by the inflation cross term

*Rationale:* The Fisher relation makes the two consistent treatments equal to the cent (WE 3.3.1b).
B is the double-deduction defect, understating value by 24.4325 %; C is the mirror defect, overstating
it by 37.8828 %; D asserts an approximation where an identity holds — the cross term is *inside*
`i_real`, which is why the subtraction shortcut, and not the exact relation, is the approximation.

**MCQ 3.3-H `[3.3.2 · Evaluation]`** An O&M obligation of USD 2,700,000 at base date runs 25 years
and is fully indexed; the forecast index is 4.0 % and the discount rate 8.0 %. A negotiator secures a
**3.0 % cap** on annual indexation. The value of that concession to the payer is closest to:
- A. nil — the cap only bites if the index exceeds 3.0 %, which is a future event
- B. USD 398,941
- C. USD 1,544,558
- D. USD 4,258,610 ✅

*Rationale:* `2,700,000 × [AF(3.846154 %, 25) − AF(4.854369 %, 25)] = 42,873,960 − 38,615,349 =
4,258,610` (WE 3.3.2b). A contradicts its own premise — the forecast *is* 4.0 %, so on the stated
assumptions the cap bites in every period; B is the correct value expressed as a **level annual
equivalent** (`÷ AF(0.08, 25)`), a right number answering a different question; C is the
**year-25 single-period** saving, undiscounted and counted once.

**MCQ 3.3-I `[3.3.4 · Application]`** A USD 42,000,000 balance accrues at 6.0 % on an **actual/360**
basis across a full 365-day year. The interest charged, and the quoted rate the convention is
equivalent to, are:
- A. 2,520,000 and 6.0000 %
- B. 2,555,000 and 6.0833 % ✅
- C. 2,562,000 and 6.1000 %
- D. 2,485,479 and 5.9178 %

*Rationale:* `42,000,000 × 0.06 × 365/360 = 2,555,000`, and `0.06 × 365/360 = 6.0833 %` (WE 3.3.4b).
A is the 30/360 figure, which charges 360 days over 360; C is a 366-day leap year on the same
convention; D inverts the fraction to `360/365`, the direction error that makes an expensive
convention look cheap.

**MCQ 3.3-J `[3.3.1 · Analysis]`** A model audit finds that a project's real model values Kestrel's
25-year support stream at **55,006,446** while its nominal model values the same stream at
**72,791,113**. The modeller proposes to "reconcile the two bases and present the pair as a range".
The reviewer should:
- A. accept — a real and a nominal view of one project legitimately differ, and a range is the honest presentation
- B. reject the proposal and require the defect to be found: the two consistent treatments are arithmetically identical to the cent, so a **17,784,667** difference is an error, and this one carries the signature of real flows discounted at the nominal 9.0 % rate ✅
- C. accept the nominal figure and delete the real model, since covenants, tax and depreciation are nominal constructs
- D. average the two, document the choice as an assumption, and proceed

*Rationale:* the Fisher relation makes the consistent nominal and consistent real valuations equal, so
a difference is not a basis to be reconciled but a defect to be located — and its size, **−24.4325 %**,
identifies which defect it is (WE 3.3.1b). A dignifies an error as a perspective. C reaches a
defensible destination by the wrong route: the model must indeed be nominal before any covenant or tax
line can be computed, but deleting the real model conceals the error instead of fixing it, and the real
model may well be the correct one. D averages a right number with a wrong one and calls the result an
assumption.

**MCQ 3.3-K `[3.3.2 · Evaluation]`** Kestrel's 3.0 % cap on the indexation of a 2,700,000 O&M base is
worth **4,258,610** in present value at the sponsor's 4.0 % index forecast — a level annual equivalent
of **398,941**. The contractor offers to remove the cap in exchange for cutting the base price by
500,000 a year. The soundest recommendation is:
- A. accept: 500,000 a year exceeds the cap's level annual equivalent by **101,059** a year, so the trade creates value
- B. refuse on these terms: the cap is an option on the index and its value is convex in the outturn, so a single-point valuation understates it — at a 5.0 % outturn the cap is worth **9,157,382**, a level annual equivalent of **857,852**, and the trade must be priced against a stressed index ✅
- C. refuse, because a project should never exchange a contractual protection for a price concession
- D. accept, provided the 500,000 reduction is itself indexed at 3.0 % so that the two legs escalate together

*Rationale:* the cap pays nothing at or below 3.0 % and more the further the index runs above it, so its
expected value exceeds its value at the mean forecast, and 500,000 a year buys away protection worth
857,852 a year in precisely the state the protection exists for (WE 3.3.2b). A is the arithmetic
correctly done at one point of a convex payoff — the commonest way an option is given away. C forgoes a
class of trade that is frequently value-creating; the objection is to the price, not the principle. D
improves the fixed leg — an indexed 500,000 is worth **7,150,991** against **5,337,388** level — and
still leaves the payer short in the stressed states, so it changes the price without answering the
objection.

**MCQ 3.3-L `[3.3.4 · Comprehension]`** A facility accrues interest on an **actual/360** basis. Which
statement restates what that convention does?
- A. it charges interest on actual days elapsed, so the cost depends on how many days each period happens to contain and averages out across a full year
- B. over a full year it charges `365/360` of the interest a 365-day basis would — a **1.3889 %** uplift on all interest, independent of the rate, the balance and the tenor, and equivalent on a 6.0 % facility to a quoted **6.0833 %** ✅
- C. it is the market standard for floating-rate lending, so it carries no cost relative to the quoted rate
- D. it lowers the effective cost, because dividing by 360 rather than 365 produces a smaller daily rate

*Rationale:* the denominator is short by five days while the numerator counts them, so the uplift is
exactly `365/360` over a full year and does not average away (WE 3.3.4b) — **8.33** basis points at
6 %, **35,000** on Kestrel's 42,000,000 balance for one year and **274,947** across the twelve-year
schedule. A describes the numerator effect, which does reverse between a short February and a 92-day
quarter, and misses the denominator effect, which does not. C confuses prevalence with price. D inverts
the arithmetic: a smaller denominator produces a *larger* daily rate.

### Self-check — KA 3.3

1. *State the consistency rule.* — Nominal flows with nominal rates, real flows with real rates;
   never mixed in one calculation. The two consistent treatments give the identical figure, so any
   difference between a real and a nominal model of one project is a defect, not a basis.
2. *Why does a 25-year concession make the Fisher shortcut dangerous?* — The 17.4757 bp error (at
   9 %/3 %) compounds across every one of 25 discount factors, and on Kestrel's stream it understates
   value by USD 1,204,318, or 0.2151 of one year's payment.
3. *Is a forward rate a forecast?* — No: it is the no-arbitrage consequence of today's spot and the
   two interest rates, and it is only a hedge once it is **contracted**.
4. *What is a one-year indexation lag worth on a stream indexed at `g`?* — Exactly `1 − 1/(1 + g)` of
   the stream — 2.9126 % at `g` = 3 % — which needs no computation once it is recognised.
5. *Two routes value the same SAR stream in USD and differ by 400,000. What is the finding?* — A
   defect, not a basis difference: covered interest parity makes the routes identical, so a spot rate
   has been used where a forward belongs, or a foreign stream has been discounted at the domestic
   rate.
6. *Why does an actual/360 facility not appear more expensive in a rate comparison?* — Because the
   uplift lives in the day-count definition rather than the quoted rate; it is worth 8.33 basis
   points at 6 %, and it belongs in the all-in cost alongside fees.

---

## Advanced topics — Domain 3

### 3.A.1 Continuous compounding

As compounding frequency `m → ∞`, `(1 + r/m)^{mn} → e^{rn}`. USD 100,000 at 8 % for 3 years
compounds continuously to `100,000 × e^{0.24}` = **USD 127,124.92** — against 125,971.20 annually. In
project work continuous compounding appears mainly in derivative pricing and some academic
appraisal literature; its practical value here is as a bound: no compounding frequency can push
growth beyond `e^{rn}`.

Two quantities make the bound usable. First, the **continuously compounded equivalent** of a quoted
annual rate is `ln(1 + i)`: 6.0 % annual is **5.8269 %** continuous (`ln 1.06 = 0.0582689081`) and
8.0 % annual is **7.6961 %**. Second, the bound is **tight**, which is the practically important
fact: at 6 %, monthly compounding gives an effective 6.1678 % against the continuous limit's
6.1837 %, so everything from monthly to instantaneous fits inside **1.5873 basis points** — of which
daily compounding already uses 1.5350 (KA 3.2.3 gives the full ladder). A model arguing about
compounding frequency beyond monthly is arguing about less than two basis points, and a model whose
continuous and monthly results differ by more than that has an error rather than a refinement.

One confusion is worth naming because the numbers invite it. The continuous equivalent of 6 %,
**5.8269 %**, sits within two basis points of the real rate derived in KA 3.3.1 from a 9 % nominal
and 3 % inflation, **5.8252 %**. The two have nothing to do with each other — one is `ln(1 + i)`, the
other is `(1 + i_nom)/(1 + π) − 1` — and a model that quotes "5.83 %" without saying which it means
has lost the audit trail on the most confusable pair of numbers in the domain. Label rates by their
construction, not by their value.

### 3.A.2 Irregular timing and period conventions

Real cash flows do not arrive on anniversary dates. Three conventions bridge the gap: **exact-date
discounting** (each flow discounted by its actual day count — the XNPV approach, standard in
transaction models); the **mid-period convention** (flows treated as arriving mid-period,
appropriate for continuous operations revenue); and **period-end** (conservative for receipts,
default in lender base cases). The convention is an *assumption* — it belongs in the assumption
register, it changes results by up to half a period's discounting, and model audits (Domain 13,
KA 13.2) check that one convention is applied everywhere.

**Worked example 3.A.2 — the payment on day 500.**

1. **Setup.** A USD 1,000,000 completion payment falls on **day 500** from the valuation date;
   the discount rate is 9.0 % annual. Compare exact-date discounting with the period-end habit of
   "call it year 2".
2. **Formula.** Exact-date: `PV = x / (1 + r)^(days/365)`.
3. **Substitution.** `1,000,000 / 1.09^(500/365) = 1,000,000 / 1.09^1.36986`.
4. **Result.** **USD 888,650**. "Year 2" rounding gives 841,680 — **USD 46,970 low**; "year 1"
   gives 917,431 — USD 28,781 high.
5. **Interpretation.** Bucketing a mid-period flow to the nearer anniversary mis-states value by
   up to half a period's discounting — nearly 5 % here. Transaction models discount on dates;
   period models declare mid-period or period-end explicitly and apply it everywhere. The
   convention chosen matters less than its disclosure and consistency. The scale of "less than its
   disclosure" is worth pricing on the master thread rather than left as a maxim. Kestrel's 25-year
   support stream is worth **USD 59,778,747** on the period-end convention; on the **mid-period**
   convention every flow moves half a year earlier, so the whole stream multiplies by
   `1.08^0.5 = 1.0392305`, giving **USD 62,123,896** — **USD 2,345,149**, or **3.9230 %**, from a
   convention choice that no cash flow and no contract has changed. On the **annuity-due** reading
   (a full period earlier) it is **USD 64,561,046** — 8.0 % above period-end and a further
   **3.9230 %** above mid-period, each step being the same `1.08^0.5` factor applied again. Three points follow. The
   three conventions **bracket** rather than approximate one another: for a continuously earning
   operating revenue the mid-period convention is the *accurate* one and period-end is the
   conservative one, so period-end is not "right" — it is a deliberate 3.9230 % understatement, and
   describing it as prudence is only honest if the size of the prudence is stated. The convention
   must be **the same on both sides of the coverage ratio**: applying mid-period to revenue and
   period-end to debt service manufactures 3.92 % of `DSCR` out of nothing, which is a
   document-conformance failure rather than a rounding one, and debt service is contractually dated
   so it is never a matter of convention at all. And the multiplier is **exactly `(1 + r)^0.5`
   regardless of the stream's shape or length**, which makes the whole adjustment a single cell and
   the whole check a single division — a reviewer suspecting a convention mismatch divides the two
   models' present values and looks for 1.0392305 (Domain 6, KA 6.1; Domain 13, KA 13.2.1).

### 3.A.3 Term structures — when one rate is not enough

A single flat `r` assumes money has the same price at every horizon. Markets disagree: rates form
a **term structure**, and precise valuation discounts each period's flow at that period's rate
(equivalently, its own `DF(t)` from a curve). Project models typically justify a flat rate as a
deliberate simplification of a curve — acceptable when documented and stress-tested (Domain 6,
KA 6.4), a silent error when inherited unexamined from a template.

**Worked example 3.A.3 — flat rate versus the curve.**

1. **Setup.** A contractor is owed USD 1,000,000 at the end of each of years 1 and 2. The spot
   curve prices 1-year money at 5.0 % and 2-year money at 7.0 %; the model uses a flat 6.0 %.
   How far wrong is the flat rate?
2. **Formula.** Curve: `PV = x/(1 + r₁) + x/(1 + r₂)²`. Flat: `PV = x/(1+r) + x/(1+r)²`.
3. **Substitution.** Curve: `1,000,000/1.05 + 1,000,000/1.07² = 952,381 + 873,439`. Flat:
   `943,396 + 889,996`.
4. **Result.** Curve **USD 1,825,820**; flat **USD 1,833,393** — the flat model overstates value
   by **USD 7,573** (0.4 %).
5. **Interpretation.** The flat 6 % "averages" the curve but misprices *both* flows — too harsh
   on the near one, too kind on the far one — and the errors do not cancel unless the stream is
   symmetric around the average tenor. For a two-flow example the drift is small. The audit question
   is never "is the rate reasonable?" but "which curve does this rate summarise, and has the summary
   been stress-tested?"

**Worked example 3.A.3b — the same question at concession scale.**

1. **Setup.** The two-flow example above understates the problem, so put the curve against Kestrel's
   own 25-year support stream of **USD 5,600,000 a year**. Assume a documented upward-sloping spot
   curve rising **10 basis points a year** from **6.00 %** for one-year money to **8.40 %** for
   25-year money — an illustrative shape, chosen to be simple enough to reproduce and steep enough to
   matter. The board's model uses a flat **8.0 %**. How wrong is the flat rate, and what flat rate
   would have been right?
2. **Formula.** Curve: `PV = Σ 5,600,000/(1 + r_t)^t` with `r_t = 0.060 + 0.0010 × (t − 1)`. Flat:
   `5,600,000 × AF(0.08, 25)`. The **PV-equivalent flat rate** is the `r` solving
   `5,600,000 × AF(r, 25) = PV(curve)`.
3. **Substitution.** Twenty-five terms discounted at their own rates, against
   `5,600,000 × 10.674776`; then solve `AF(r, 25) = 63,564,261.78 ÷ 5,600,000 = 11.350761`.
4. **Result.** Curve **USD 63,564,262**; flat 8 % **USD 59,778,747**. The flat rate **understates the
   stream by USD 3,785,515**, or **6.3325 %**. The PV-equivalent flat rate is **7.2946 %** — not the
   curve's arithmetic mean of 7.20 %, and 70.54 basis points below the rate the board used.
5. **Interpretation.** The two-flow example drifted by 0.4 %; the concession drifts by **6.3325 %**
   and USD 3,785,515 — more than twice the 1,778,747 by which Case study A's whole support decision
   turns — which is the honest scale of the "flat rate as a simplification" question and the reason
   this Advanced topic exists. Three readings. **The equivalent flat rate is not the average rate.**
   7.2946 % against an arithmetic mean of 7.2000 % — the discrepancy arises because discount factors
   are convex in the rate and because the near years carry more weight, so averaging a curve
   understates the equivalent flat rate on a rising curve and the direction reverses on an inverted
   one. Anyone who computes a flat rate by averaging a curve should expect to be wrong by tens of
   basis points, with a sign that depends on the curve's slope and the stream's shape. **The sign of
   the error depends on the slope, not on the level.** Against this rising curve the flat 8 %
   understates a receivable and would *overstate* a payable of the same shape, so a model using one
   flat rate for both the support stream and the O&M obligation biases both in the same direction and
   therefore biases the net position twice. And **the summary is legitimate if it is stated**: a flat
   7.2946 % reproduces the curve's answer for *this stream* exactly, so a model can defensibly use it
   provided the documentation says which curve it summarises, for which cash-flow profile, and that
   re-shaping the stream invalidates the summary. That last clause is the one that fails in practice:
   a flat rate inherited from a previous transaction's template is a summary of a curve nobody has
   seen, for a stream nobody has matched, and Domain 6 (KA 6.4) treats it as an unsourced input rather
   than an assumption.

### 3.A.4 The reviewer's TVM eye

Experienced reviewers do not recompute everything; they run invariants. Each of the following is
cheap, is independent of the project, and localises a defect rather than merely detecting one — which
is why the list is worth learning as a list:

| Invariant | What a violation localises | Source |
|---|---|---|
| Discount factors decline monotonically | The factor formula or its exponent | 3.1.3 |
| `DF(t+1) = DF(t)/(1+r)`; `DF(1) × (1+r) = 1` | A hard-coded factor cell | 3.1.3 |
| `AF(r,n) × r = 1 − DF(n)` | A mismatch between a factor column and its annuity summary | 3.1.3b |
| `AF(r,n) < 1/r` always | A rate entered as a percentage, or a tenor in the rate cell | 3.1.3b |
| Annuity-due ÷ ordinary annuity = `(1 + r)` exactly | A payment-timing convention read against the contract | 3.2.1b |
| Indexed stream = `A × AF(r*, n)` by two routes | Double-counted indexation in an escalating stream | 3.2.1d |
| Schedule closes at zero to display-rounding tolerance | Instalment, rate or period-count error | 3.2.2 |
| Σ principal = `P`; final ÷ first principal = `(1 + r)^(n−1)` | The principal column | 3.2.2 |
| `B_k = A × AF(r, n − k)` equals the recursion | The schedule, not the formula | 3.2.2d |
| `EAR ≥ i_nom`, equality only at `m` = 1 | A compounding-frequency misread | 3.2.3 |
| All-in cost > contract rate whenever a fee is deducted | A fee added to a rate instead of solved from the stream | 3.2.3b |
| Real and nominal treatments agree to the cent | A mixed-basis model — an error, never a basis difference | 3.3.1b |
| Real < nominal whenever `π > 0` | An inverted Fisher relation | 3.3.1 |
| Two routes to a hedged FX value agree | A spot rate used where a forward belongs | 3.3.3b |
| actual/360 interest = 30/360 interest × `365/360` over a full year | A day-count basis not implemented as documented | 3.3.4b |
| Mid-period ÷ period-end present value = `(1 + r)^0.5` | A timing convention applied inconsistently | 3.A.2 |

Any violated invariant is a defect *somewhere* — find it before anyone downstream builds on it. The
converse deserves equal emphasis and is the reason this section is an Advanced topic rather than a
checklist: **every invariant above can hold while the answer is wrong.** They test formulae, not
inputs. A model built on the wrong rate, the wrong tenor, the wrong index or the wrong world passes
all sixteen, which is why the assumption register (3.T.1) and the invariants (3.T.2) are two controls
and not one.

---

## Industry variations — Domain 3

The machinery is universal; its parameters are sectoral, and a project finance leader reads the
sector before trusting any inherited rate, tenor or convention:

- **Power and renewables.** Long contracted tenors (20–35 years) make value acutely
  curve-sensitive (3.A.3) and escalation-sensitive (3.3.2); availability and capacity payments
  are near-annuities, so the annuity family does most of the valuation work. Debt sculpting to
  seasonal cash flows (Domain 10) starts from monthly, not annual, discounting.
- **Transport concessions.** Patronage risk pushes discount rates up and makes single-rate
  valuation least defensible — scenario-based rates and revenue stress tests (Domain 7, KA 7.4)
  ride directly on this domain's factors.
- **Water and regulated utilities.** Regulatory reset cycles (often 5-yearly) partition the
  horizon: within-period flows discount at financing rates; across resets, the *real/nominal
  discipline* (3.3.1) dominates because regulators commonly work in real terms while lenders
  live in nominal.
- **Digital infrastructure.** Shorter economic lives and refresh capex compress tenors: bullet
  and mini-perm shapes (3.2.2) with deliberate refinancing (Domain 15) are standard, so the
  balloon-risk arithmetic of Case B is daily practice, not a cautionary tale.
- **Oil, gas and mining.** Commodity-linked revenue makes escalation assumptions (3.3.2) the
  loudest value driver, and multi-currency operations make the day-count and FX conventions
  (3.3.3–3.3.4) contractual battlegrounds rather than back-office detail.
- **Social infrastructure PPPs.** Availability payments in advance are common — the annuity-due
  reading (WE 3.2.1b) is the difference between winning and mispricing a bid.

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
**8.3570 %** — the **breakeven rate** at which `AF(r, 25) × 5,600,000 = 58,000,000`, i.e.
`AF = 10.357143`, which is only **35.70 basis points** of headroom above the 8.0 % the board used.
That is a thin margin for a decision of this size, and it is the number the minute must carry rather
than the 1,778,747, because the surplus is the *output* and the rate is the *assumption*.

**The negotiating range, which the two-package framing conceals.** The offer on the table is one
point inside a range, and the range is computable. The payment at which **Kestrel** is exactly
indifferent between stream and grant is `58,000,000 ÷ AF(0.08, 25) = 58,000,000 ÷ 10.674776 =`
**USD 5,433,369**; the payment at which the **grantor** is exactly indifferent is
`58,000,000 ÷ AF(0.10, 25) = 58,000,000 ÷ 9.077040 =` **USD 6,389,748**. Any annual payment between
those two figures leaves both parties better off than the grant, so the whole negotiation is a
**USD 956,379 a year** band — and the grantor's proposed 5,600,000 sits only **17.4231 %** of the way
into it. Expressed in present value, the joint gain created by choosing the stream over the grant is
**USD 8,947,323** (exactly the wedge `5,600,000 × [AF(0.08, 25) − AF(0.10, 25)]`), of which Kestrel's
1,778,747 is **19.8802 %** and the grantor's 7,168,576 is 80.1198 %. Both parties gain; one gains four
times as much; and the standard analysis — "the stream is worth more to us than the grant, so accept
it" — is what causes the 80/20 split, because it stops at the sign of the surplus rather than
computing the range. A hybrid makes the same point in a form a negotiator can table: a
**USD 29,000,000 grant plus a stream at the grantor's own indifference price** of
`29,000,000 ÷ 9.077040 =` **USD 3,194,874** a year is worth **USD 63,104,566** to Kestrel — a surplus
of **USD 5,104,566**, nearly three times the 1,778,747 on offer, at no cost to the grantor by its own
measure.

**What the domain teaches here.** Valuation is always *at a rate, for a party*; streams and lumps
are only comparable through `PV(x)`; and the negotiation surface between two parties' rates is
where structuring value lives (Domain 7 revisits this as tariff design; Domain 9 as the
grant-versus-support decision). Three cautions keep the arithmetic honest. The grantor's 10 % is
**inferred, not disclosed** — real counterparties do not publish their discount rates, and the range
above is only as good as the inference, so the professional practice is to compute the range across a
plausible band of counterparty rates and negotiate against the *conservative* end. A payment stream
and a grant differ in more than value: the stream carries **25 years of grantor credit and political
risk** while the grant is cash at completion, and Domain 11 (KA 11.2) is where that difference is
priced rather than inside `r` without saying so. And a package that is value-neutral to the grantor
may be **fiscally or legally unavailable** to it — appropriation rules, budget-year constraints,
state-aid and public-accounting treatments of long-term commitments differ by jurisdiction, are
frequently decisive, and are matters for the grantor's own counsel and auditors rather than for this
arithmetic.

## Case study B — Domain 3: the bullet that had to be refinanced (energy / refinancing)

**Situation.** A 30 MW peaking-plant SPV borrowed **USD 30,000,000** for 7 years. The sponsor
chose a **bullet** (interest-only at 7.5 %, principal at maturity) over the lenders' preferred
**annuity** shape, to keep early cash for distributions.

**The two prices.** Annuity instalment: `A = 30,000,000 × 0.075/(1 − 1.075⁻⁷) =`
**USD 5,664,009** per year; total paid 39,648,066, of which interest **USD 9,648,066**. Bullet:
interest `30,000,000 × 0.075 =` 2,250,000 per year — total interest **USD 15,750,000**, plus the
full USD 30,000,000 due in year 7. The bullet pays **USD 6,101,934 more interest** for the
privilege of deferral — and retains the entire principal as **refinancing risk**.

**Was the choice wrong? The arithmetic says no.** Before judging the sponsor, price the deferral
properly. The bullet's cash-flow *advantage* is 5,664,009.46 − 2,250,000 = **USD 3,414,009 a year in
years 1 to 6**, against a *disadvantage* of `32,250,000 − 5,664,009.46 =` **USD 26,585,991 in year
7**. Discount that difference stream and the indifference rate — the rate at which deferral is worth
exactly nothing — comes out at **exactly 7.5000 %**, the loan rate itself. That is not a coincidence
and it is the case study's central result: **the bullet's 6,101,934 of extra nominal interest is not a
cost at all, it is precisely the price of the deferral at the contract rate**, so a sponsor whose
alternative use of cash earns more than 7.5 % is right to defer, and one whose alternative earns less
is wrong. At the sponsor's 15 % equity cost the deferral is worth **+USD 2,925,601** in present value
(at 12 %, +2,010,232; at 10 %, +1,226,084; at 20 %, +3,933,661). **The sponsor's decision was
value-creating on its own numbers, and the "USD 6,101,934 more interest" headline that condemns it is
double-counting the time value the deferral exists to capture.**

**What happened.** At year 7 the credit market had repriced: the refinancing cleared at 9.8 %, and
the new 7-year annuity instalment on the rolled principal became `30,000,000 × 0.098/(1 −
1.098⁻⁷) = 6,121,646` — against 5,664,009 had the original annuity simply been running to zero, an
increase of **USD 457,636 a year**. Distributions locked up under the new facility's covenants for two
years (Domain 10, KA 10.4). Note what the annuity's balance path (Fig 3.2.3) would have delivered
instead: an outstanding balance at the end of year 6 of `5,664,009.46 × AF(0.075, 1) =` **USD
5,268,846**, so the annuity structure faced the same repriced market with **17.5628 %** of the
exposure the bullet carried.

**What the domain teaches here.** A schedule shape is a risk allocation, not a formatting choice:
the annuity retires rate risk continuously; the bullet stores it at maturity, priced at a rate
nobody today knows. Deferral has a computable minimum cost (the interest differential) and an
uncomputable tail (the refinancing market) — leadership means pricing the first honestly and
sizing the second deliberately (Domain 15, KA 15.3). The precise lesson is sharper than "bullets are
dangerous", and it is the one worth carrying: the **priced** part of the bullet is fair — the sponsor
paid 7.5 % for the deferral and would have been right to take it against any use of cash earning more
— and the **unpriced** part is what destroyed the value. The failure was not choosing the bullet; it
was choosing it without buying anything against the refinancing exposure: no forward-starting hedge,
no committed refinancing facility, no amortising sinking-fund reserve, no tenor that outlived the
credit cycle. A structure whose entire risk sits in one date is a structure that needs an instrument
attached to that date, and the discipline is to separate the two decisions — **defer, priced at the
contract rate; and hedge the tail, priced separately** — rather than to let a good answer to the first
question stand in for no answer to the second.

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
- **The range, not the surplus.** When a counterparty offers a choice, the director's question is not
  "which option is worth more to us?" but "what is the band inside which both parties prefer this
  option, and where in that band does the offer sit?" On Kestrel's support decision the band is
  **USD 956,379 a year** wide and the offer sits **17.4231 %** into it, capturing **19.8802 %** of a
  joint gain of USD 8,947,323 — a fact that no calculation of the surplus alone reveals, and the
  single highest-return use of this domain's arithmetic in a negotiation (Case study A).
- **The small terms, priced.** Analysts negotiate margins because margins are quoted. The director
  asks what the *unquoted* terms are worth, and the answers on Kestrel are uncomfortable: an
  indexation cap USD 4,258,610, a repayment holiday USD 3,018,782, a day-count basis USD 274,947, an
  arrangement fee 37.04 basis points against a margin argument worth 15. Nothing in that list
  requires new information — only that somebody compute it before signature rather than after.
- **The verification culture.** The invariants of 3.A.4 cost minutes; unverified TVM in a signed
  bid costs careers. The director's question is never "who computed this?" but "who *re*computed
  this?" — and, where AI drafted it, the answer must name a human. The professional honesty this
  domain demands is that the invariants prove the **formulae** and never the **basis**: a model can
  pass every check in this chapter and still implement a loan the facility agreement does not
  contain, which is why the director's second question is "against which document?"

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

**Exercise 3.6** A concession tariff is worth USD 4,000,000 a year at base date and indexes fully at
3.0 % for 15 years, paid annually in arrears. The discount rate is 9.0 %. Value the stream.
*Solution.* `r* = 1.09/1.03 − 1 = 5.8252 %`; `AF(r*, 15) = 9.824117`;
`PV = 4,000,000 × 9.824117 =` **USD 39,296,469**. Common error 1: valuing it as a level annuity at
9 % — `4,000,000 × 8.060688 =` 32,242,754, understating by 7,053,715 (17.95 %) by ignoring the
indexation. Common error 2: escalating the flows **and** discounting at `r*` — 48,651,797,
overstating by 9,355,328 (23.81 %) by counting the indexation twice.

**Exercise 3.7** On Kestrel's annuity schedule (USD 42,000,000, 12 years, 6.0 %, instalment
5,009,635.23), state the principal outstanding immediately after the year-9 instalment, and prove it
without the schedule.
*Solution.* `B_9 = A × AF(r, n − k) = 5,009,635.23 × AF(0.06, 3) = 5,009,635.23 × 2.673012 =`
**USD 13,390,815** (13,390,814.83 by formula; 13,390,814.89 by recursion on the printed instalment —
agreement to six cents). Common error: assuming principal retires evenly and computing
`42,000,000 − 9 × 2,489,635 =` 19,593,285, overstating the balance by 6,202,470 because the annuity's
principal component grows by `(1 + r)` every year.

**Exercise 3.8** A USD 25,000,000 facility is repayable in 8 equal annual instalments at 5.5 %, with
a 1.5 % arrangement fee deducted from proceeds. Compute the instalment and the all-in effective cost.
*Solution.* Instalment `25,000,000 × 0.055/(1 − 1.055⁻⁸) =` **USD 3,946,600** (3,946,600.30). Net
proceeds `25,000,000 × 0.985 =` 24,625,000. Solving `Σ 3,946,600.30/(1 + r)^t = 24,625,000` over 8
years gives **5.8794 %** — a premium of **37.94 basis points** on a 1.5 % fee, because the tenor is
short and an upfront fee is spread over fewer periods. Common error: adding `1.5 % ÷ 8 = 18.75` basis
points to the coupon to get 5.6875 %, which understates the cost by more than half; the shorter the
tenor, the worse that approximation becomes.

**Exercise 3.9** A USD 20,000,000 facility runs 10 years at 7.0 % with the first **two years
interest-only**. Find the instalment for years 3 to 10 and the extra interest the holiday costs.
*Solution.* `A_h = 20,000,000 × 0.07/(1 − 1.07⁻⁸) = 20,000,000 ÷ 5.971299 =` **USD 3,349,355**
(3,349,355.25), against a plain 10-year instalment of 2,847,550.05. Lifetime interest
`2 × 1,400,000 + (8 × 3,349,355.25 − 20,000,000) =` **USD 9,594,842** against **USD 8,475,501** —
the holiday costs **USD 1,119,341** and raises the instalment by 17.62 %. Common error: assuming the
holiday is free because the maturity is unchanged; the interest on two years of undiminished
principal is the cost, and it is knowable at signature.

**Exercise 3.10** Spot `USD 1 = SAR 3.7500`; three-year money costs 5.0 % in USD and 5.5 % in SAR.
State the three-year forward and the USD value today of a receipt of SAR 10,000,000 in three years.
*Solution.* `F₃ = 3.7500 × (1.055/1.05)³ =` **SAR 3.8038** per USD. The USD amount receivable at
year 3 is `10,000,000 ÷ 3.8038 =` **USD 2,628,931**; converting at spot instead would show
**USD 2,666,667**, overstating the hedged receipt by 37,736. Common error: using the spot rate for a
future flow — the SAR is the higher-interest currency, so it trades at a forward *discount*, and spot
conversion of a future foreign receipt systematically flatters the model.

**Exercise 3.11** A cost of USD 5,000,000 at base date escalates for 10 years on an index forecast at
5.0 %; the contract caps annual indexation at 3.0 %. The discount rate is 8.0 %. Value the cap.
*Solution.* Uncapped `r* = 1.08/1.05 − 1 = 2.8571 %`, `AF = 8.592732`, `PV = 5,000,000 × 8.592732 =`
**USD 42,963,658**. Capped `r* = 1.08/1.03 − 1 = 4.8544 %`, `AF = 7.776638`, `PV =` **USD
38,883,189**. The cap is worth **USD 4,080,469**, or 9.50 % of the uncapped obligation. Common error:
valuing the cap as the final-year price saving (`5,000,000 × (1.05¹⁰ − 1.03¹⁰) =` 1,424,891) — a
single period, undiscounted, in place of a stream.

## Practitioner's toolkit — Domain 3

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 3.T.1 — TVM assumption register (one row per modelled stream)

| Field | Discipline it enforces |
|---|---|
| Stream name & source document | Every number traces to a contract or estimate |
| Currency | Mismatches surface (KA 3.3.3) |
| Nominal or real | The consistency rule (3.3.1), stated not assumed |
| Rate & its owner | The director owns `r` (Executive perspective) |
| Rate construction (quoted / EAR / continuous / growth-adjusted `r*`) | The domain's most confusable pair sit two basis points apart (3.A.1) |
| Compounding frequency **and payment frequency**, separately | They are different levers and pull in opposite directions (WE 3.2.3c) |
| Day-count basis | Worth 8.33 bp at 6 % on actual/360, and invisible in a rate (WE 3.3.4b) |
| Timing convention (arrears/advance/mid-period) | Half-period errors caught (3.A.2) |
| Escalation index, lag, cap/floor, weighting | Contractual, not invented (3.3.2, WE 3.3.2b) |

### Toolkit 3.T.2 — Schedule QA checklist

- [ ] Payment recomputed independently of the model (different implementer or method).
- [ ] Final balance zero **to a stated tolerance** (display rounding only); Σ principal = original loan.
- [ ] Outstanding balance at one interior date agrees between the recursion and `A × AF(r, n − k)`.
- [ ] Final-year principal ÷ first-year principal = `(1 + r)^(n−1)` on an annuity schedule.
- [ ] Periodic rate = nominal / **compounding** frequency; period count = years × **payment**
      frequency; the two frequencies confirmed separately against the facility agreement.
- [ ] Day-count basis implemented as named, and its `365/360`-type uplift reflected in the all-in cost.
- [ ] Binding coverage period identified by inspection of the service profile, not assumed to be
      period 1 (holidays, sculpting and step-ups move it).
- [ ] Discount factors monotone; `DF(1) × (1 + r) = 1`; `AF(r,n) × r = 1 − DF(n)`; `AF(r,n) < 1/r`.
- [ ] Display rounding only — full precision beneath every printed figure.
- [ ] AI-drafted content marked, and its human verifier named (Domain 16, KA 16.4).

### Toolkit 3.T.3 — Escalation register

For each escalating line: base amount and base date · index (publisher, series, definition) ·
lag · formula (full/partial indexation, weightings) · cap/floor · compounding basis · the clause
reference in the contract. A model whose escalation register is complete can survive a Domain 13
model audit; one whose escalation is "4 % because last year" cannot.

### Toolkit 3.T.4 — Rate comparison worksheet (one column per offer)

Never compare quoted rates. Build one column per competing facility and require every row to be
filled before any comparison is spoken aloud:

| Row | Why it is on the sheet |
|---|---|
| Facility amount · tenor · repayment shape | Only like-for-like offers are comparable (3.2.2) |
| Quoted rate · compounding frequency · payment frequency | Three different things behind one number (3.2.3, WE 3.2.3c) |
| Day-count basis, and its rate equivalent | Worth 8.33 bp at 6 % on actual/360 (WE 3.3.4b) |
| Upfront fees — deducted or capitalised | The two are not the same calculation (WE 3.2.3b) |
| Commitment fee and expected undrawn profile | Cannot be priced without a drawdown schedule (Domain 14) |
| Net proceeds actually received | The denominator of the all-in solve |
| Contractual instalment, recomputed independently | The numerator of the all-in solve |
| **All-in effective cost, solved from the stream** | The only comparable figure (WE 3.2.3b) |
| Fee-to-margin equivalence at this tenor | The negotiating currency: 18.38 bp per 1 % on Kestrel |
| First-period and minimum debt service, with `DSCR` | Cost is not the only variable (Domain 10) |
| Prepayment terms and break costs above `B_k` | Add to the balance formula, never replace it (WE 3.2.2d) |

Two mandatory footers: the method statement ("all-in costs solved from each stream against net
proceeds; no fee added to a rate") and the named individual who recomputed each column.

## Exam preparation — Domain 3

**The calculation traps.** Off-by-one periods ("in year 5" vs "after 5 years") · simple-for-
compound substitution · rounded discount factors in the arithmetic · annuity-due priced as
ordinary · EAR used as the periodic rate, or as an annual rate over `n` years (WE 3.2.3c) ·
nominal/real mixing · escalation applied simply · **indexation counted twice** (escalating the flows
*and* discounting at `r*` — WE 3.2.1d) · a growing
perpetuity used where a finite indexed annuity belongs · an annuity factor exceeding `1/r` ·
tenor mismatch in annuity factors (the 3.2-A distractor D) · a fee divided by the tenor and added to
the coupon instead of solved from the stream · a repayment holiday assumed to be free, and its
binding coverage period assumed to be period 1 · an outstanding balance estimated as
`P − k × first-year principal` · an indexation cap valued at its final-year saving rather than as a
stream · a contractual indexation lag ignored · a day-count basis omitted from an all-in comparison,
or its fraction inverted · spot conversion of a future foreign receipt · treating a forward as a
forecast · a flat rate assumed to be a curve's arithmetic mean.

**Reflection questions.**
1. Your CFO asks for "the value of the concession". What three questions must you answer before
   any number is defensible? *(At what rate; in which world — nominal or real; under which timing
   conventions.)*
2. A lender and a sponsor disagree about the value of the same payment stream. Under what
   conditions are both right — and where does that create negotiating room? *(Case study A: a
   USD 956,379-a-year band, of which the standard offer captures 17.4231 %.)*
3. Which invariant checks would have caught the last TVM error you saw in the wild — and why
   didn't they run? *(3.A.4; toolkit 3.T.2.)*
4. A model passes every check in 3.A.4 and every line of Toolkit 3.T.2, and its debt-service
   schedule is nevertheless wrong. Name two ways that can be true, and say which control catches
   each. *(A valid amortisation the facility does not provide for, and a right calculation on the
   wrong basis: the assumption register and document conformance, not the invariants — WE 3.2.3c,
   Domain 13 KA 13.2.1.)*
5. Your team has negotiated 15 basis points off a margin. Name three terms elsewhere in the same
   term sheet that are each worth more, and state their values on Kestrel's facility. *(A 2.0 %
   arrangement fee, 37.04 bp; an actual/360 day-count basis, 8.33 bp and USD 274,947 of interest; a
   three-year repayment holiday, USD 3,018,782 — and, outside the facility, an indexation cap worth
   USD 4,258,610.)*

## Domain 3 summary

Money's value is a function of time, and this domain built the complete conversion machinery:
compounding and discounting (`FV(x)`, `PV(x)`, `DF(t)`), together with the inverse operations that
solve a stream for its rate; the annuity family that prices every level **and indexed** stream from
availability payments to debt service (`AF(r,n)`, perpetuities, the growth-adjusted rate `r*`, the
four loan shapes, the outstanding-balance formula and their risk meanings); frequency and
effective-rate honesty, including the distinction between compounding frequency and payment
frequency; and the real-world adjustments — Fisher-consistent inflation treatment, contract-grade
escalation with caps, lags and weightings, no-arbitrage currency forwards, and day-count bases that
move real money.

Four results carry beyond the domain. The **consistency identity**: the nominal and real treatments
of one stream are equal to the cent, so any difference is a defect and not a basis — and mixing them
moves Kestrel's support stream by −24.4325 % or +37.8828 %. The **indexation identity**:
`PV = A × AF(r*, n)`, which prices every escalating stream with the machinery already built and whose
misuse, counting indexation twice, overstates value by 31.0818 %. The **all-in discipline**: no rate
enters a comparison until it has been solved from the actual stream, because on Kestrel each 1 % of
upfront fee is worth 18.38 basis points and a day-count basis is worth 8.33 more. And the
**shape-is-risk result**: the same loan under four schedules costs between 16,380,000 and 30,240,000
of interest and produces first-year coverage between 1.0605 and 2.5333, so choosing a repayment shape
is a risk allocation, and the deferral inside a bullet is fairly priced at the contract rate while the
tail it leaves is not priced at all.

Its discipline is as important as its formulae: one world per model, full precision beneath displayed
rounding, conventions in the assumption register, the sixteen invariants of 3.A.4 run on every
schedule, and machine-drafted arithmetic verified by a named human before anyone relies on it. Every
discounted number in the thirteen domains that follow stands on this one.
