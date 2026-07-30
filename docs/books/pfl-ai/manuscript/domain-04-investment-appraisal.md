# Domain 4 — Investment Appraisal and Capital Budgeting *(quantitative)*

> **Group:** Foundations (Domain 4 of 4 in Part One). **Target:** ~70 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain is the home of the appraisal symbols — `NPV`, `IRR`,
> `MIRR`, `PI`, `EAV` — and builds directly on Domain 3's machinery (`PV(x)`, `DF(t)`,
> `AF(r, n)`): every formula here is a disciplined arrangement of those parts.
> British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Domain 3 taught how to move money across time; this domain teaches how to *decide* with it.
Investment appraisal is where analysis meets commitment: the techniques here — net present
value, internal rates of return, payback, profitability, equivalent annual value — are how a
project finance leader turns forecast cash flows into a defensible yes, no, or not-this-one.
The measures are deliberately plural because each answers a different question: NPV measures
value created; IRR measures the return embedded in the flows; payback measures exposure;
the profitability index measures value per scarce dollar; equivalent annual value compares
unlike lifetimes. Leadership in this domain means knowing which question the decision actually
asks — and refusing to let a single seductive percentage (usually IRR) answer all of them.
Domain 6 industrialises these calculations in the financial model; Domain 10 replays them from
the lender's side of the table.

**Learning objectives.** After this domain a candidate can: compute and interpret NPV and
explain why it is the primary measure of value; compute IRR, recognise its pathologies
(multiple roots, scale-blindness, reinvestment assumption) and know when it misleads; compute
MIRR and state what it fixes and what it does not; apply payback and discounted payback as
exposure measures, not value measures; rank with the profitability index under capital
rationing; compare unequal lives with equivalent annual value; resolve NPV–IRR conflicts on
mutually exclusive projects; and subject any machine-produced appraisal to the family's
verification rule.

**The master appraisal.** The Kestrel Water SPC financing of Domain 3 now faces its investment
decision. Building the plant requires **I₀ = USD 60,000,000** today; the operating model
forecasts **net cash inflows of USD 8,900,000 per year for 15 years**; the board's discount
rate remains **8.0 %**. Every KA below interrogates this one decision from a different angle.

---

## Knowledge Area 4.1 — The discounted measures: NPV, IRR and MIRR

*Topics: 4.1.1 net present value · 4.1.2 IRR and its pathologies · 4.1.3 MIRR.*

### 4.1.1 Net present value

**Definition.** NPV discounts every cash flow to today and nets off the investment:

```
NPV = Σ CFₜ / (1 + r)ᵗ − I₀
```

A positive NPV means the project returns more than the capital's required return `r` — it
*creates* value; a negative NPV destroys it. NPV's authority rests on three properties no rival
measure shares: it is **additive** (portfolio NPV is the sum of project NPVs), it is
**scale-aware** (twice the project, twice the NPV), and it embeds the **correct reinvestment
assumption** (cash thrown off is assumed to earn `r`, the opportunity cost — not the project's
own return).

**Worked example 4.1.1 — should Kestrel build?**

1. **Setup.** `I₀` = USD 60,000,000; net inflows USD 8,900,000 per year for 15 years (in
   arrears); `r` = 8.0 %.
2. **Formula.** For a level stream, `NPV = A × AF(r, n) − I₀` (Domain 3, KA 3.2).
3. **Substitution.** `AF(0.08, 15) = 8.559479`; `NPV = 8,900,000 × 8.559479 − 60,000,000 =
   76,179,360 − 60,000,000`.
4. **Result.** **NPV = +USD 16,179,360** (≈ SAR 60.7 million indicatively).
5. **Interpretation.** At the board's 8 % the plant is worth USD 16.2 million more than it
   costs — build, on these forecasts. The phrase carrying the professional weight is *on these
   forecasts*: NPV is only as honest as the cash flows and the rate beneath it, which is why
   Domain 6's model discipline and this domain's sensitivity habits (KA 4.3.3) are part of the
   appraisal, not decoration around it.

> **Fig 4.1.1 — Kestrel's NPV profile.** Line chart, x-axis discount rate 0–20 %, y-axis NPV
> (USD millions). The curve starts at +73.5 (undiscounted: 8.9 × 15 − 60), falls through
> +16.18 at 8 % (marked), crosses zero at **12.19 % — the IRR** (crimson marker), and continues
> negative to about −18.4 at 20 %. Dashed vertical line at the 8 % board rate. Source: PCI
> original. Alt text: downward-sloping curve of project value against discount rate, positive
> at low rates, crossing zero at the internal rate of return of about twelve per cent.

**Which cash flow belongs in the appraisal.** Before any formula, a category rule that decides
more appraisals than the arithmetic does: the flows discounted here are the project's **free
cash flows** — operating receipts less operating costs, less tax on operating profit, less
capital expenditure and working-capital movements — and they contain **no financing flows at
all**. No interest, no loan drawdown, no repayment, no dividend. The reason is not convention
but double-counting: the cost of the financing is already inside `r`. Put interest in the
numerator *and* in the discount rate and the project is charged twice for its debt; a viable
project is then rejected by an error that no reviewer of the spreadsheet's arithmetic will ever
find, because every cell is correct. The mirror-image discipline is that if you *do* discount
equity cash flow — after debt service — you must use the cost of equity, not the project rate
(KA 4.A.3 prices that mistake for Kestrel; it is worth 25.2 million of NPV).

Two consequences follow for the master appraisal. The 8,900,000 is a project-level figure, so
the 8 % it meets is a project-level rate — a weighted average cost of capital, which Domain 9
derives properly and KA 4.A.3 reconciles. And sunk costs stay out: Kestrel's feasibility and
transaction spend — the development capital Domain 5 tracks as already at risk before this
decision — appears nowhere in the numerator, because a cost that cannot be avoided by deciding no
is not part of the decision. Note the discipline this cuts both ways: spend that *is* still
avoidable belongs in the flows even if it has already been contracted, and a capital item absorbed
into the 60,000,000 budget (Domain 5's corridor re-route is the thread's example) is part of `I₀`
rather than a sunk cost, however early it was committed. The development spend that this appraisal
excludes belongs in the record of what the development stage cost, which is Domain 5's question,
not this one's.

**Worked example 4.1.1b — the same money, later.**

The level 8,900,000 a year is a convenience. Real plants ramp: commissioning takes a year,
the offtaker's demand builds, and full output arrives in year three. Kestrel's engineers
restate the forecast with the **identical total** — so that timing is the only variable.

1. **Setup.** `I₀` = 60,000,000; inflows 4,600,000 (year 1), 6,700,000 (year 2), then
   9,400,000 for years 3–15. Total nominal inflow 4,600,000 + 6,700,000 + 13 × 9,400,000 =
   **133,500,000**, exactly the level case's 8,900,000 × 15. `r` = 8 %.
2. **Formula.** Discount the two irregular years individually and treat the tail as a deferred
   annuity: `PV = CF₁ · DF(1) + CF₂ · DF(2) + CF₃₋₁₅ · [AF(r, 15) − AF(r, 2)]`. The bracketed
   term is Domain 3's deferred-annuity device (KA 3.2.1) — the 15-year factor less the two years
   that are not in the stream.
3. **Substitution.** `DF(1) = 0.925926`, `DF(2) = 0.857339`,
   `AF(0.08, 15) − AF(0.08, 2) = 8.559479 − 1.783265 = 6.776214`;
   `PV = 4,600,000 × 0.925926 + 6,700,000 × 0.857339 + 9,400,000 × 6.776214
   = 4,259,259 + 5,744,170 + 63,696,411`.
4. **Result.** `PV = 73,699,840`; **NPV = +USD 13,699,840** — against +16,179,360 on the level
   profile. The ramp costs **USD 2,479,520**, or 15.3 % of the project's value.
5. **Interpretation.** Not one dollar of revenue was lost; 2.48 million of value was, purely by
   arriving later. This is the single most under-appreciated number in early-stage appraisal,
   because the level-annuity shortcut is how almost every screening model is built and the ramp
   is how almost every plant actually starts. Two professional consequences follow. First, a
   screening NPV computed on a level profile is an *upper bound*, and should be labelled as one.
   Second, commissioning acceleration is worth real money that is invisible in a level model:
   pulling the ramp forward by one year here is worth roughly the same order as the whole ramp
   penalty, which is why Domain 14's drawdown and completion tests carry the commercial weight
   they do. The reviewer's question is therefore never "what is the NPV?" but "on what profile?"

**Worked example 4.1.1c — the convention that is worth three million.**

Annual flows do not arrive on 31 December. Cash accrues through the year, so discounting every
year's inflow as though it landed on the final day systematically understates value. The
**mid-period convention** discounts each year's flow from its mid-point instead.

1. **Setup.** The master appraisal, discounted (a) at year-end and (b) at mid-year.
2. **Formula.** Shifting every flow half a period earlier multiplies the whole present value by
   `(1 + r)^0.5`: `PV_mid = PV_end × (1 + r)^0.5`. (Discounting each flow individually from
   `t − 0.5` gives the identical result; the single multiplier is the audit-friendly route.)
3. **Substitution.** `PV_end = 8,900,000 × 8.559479 = 76,179,360`;
   `(1.08)^0.5 = 1.0392305`; `PV_mid = 76,179,360 × 1.0392305`.
4. **Result.** `PV_mid = 79,167,914`; **NPV = +USD 19,167,914** against +16,179,360 at year-end
   — a convention difference of **USD 2,988,553**, or 18.5 % of the reported value.
5. **Interpretation.** Neither number is wrong; the *silence* is. Almost three million of
   reported value turns on a modelling choice that appears in no formula and is rarely written
   down, and the same choice applied to a competing project changes both NPVs — which is why
   comparisons across projects modelled by different teams are unsafe until the convention is
   confirmed. The discipline is simple and non-negotiable: **the convention is declared in the
   assumption register, applied to every line including terminal values, and held constant
   across every option in a decision.** A useful reviewer's shortcut falls out of the algebra:
   the ratio between the two conventions is always `(1 + r)^0.5`, so at 8 % any correctly
   mid-period model reports about 3.9 % more present value than the year-end version of itself.
   A gap of a different size means the convention was applied to some lines and not others.

### 4.1.2 IRR and its pathologies

**Definition.** The internal rate of return is the discount rate at which NPV is zero — the
break-even price of capital:

```
NPV(IRR) = 0
```

For Kestrel: solve `8,900,000 × AF(r, 15) = 60,000,000` → `AF = 6.741573` → **IRR = 12.19 %**.
Read against an 8 % hurdle, the project clears with 4.19 points to spare — the same verdict as
NPV, expressed as a margin of safety rather than a value.

**Worked example 4.1.2 — solving the rate, then proving it.**

No closed form exists for `r` in an annuity of fifteen terms, so the IRR is always *solved* —
by a spreadsheet's iteration, by interpolation between tabulated factors, or by bisection. What
matters professionally is not which route you take but that you then **verify the answer against
the equation it is supposed to satisfy.**

1. **Setup.** The master appraisal. Find `r` such that `8,900,000 × AF(r, 15) = 60,000,000`.
2. **Formula.** The target factor is `AF = I₀ / A = 60,000,000 / 8,900,000 = 6.741573`. Bracket
   it between two rates whose factors straddle the target, then interpolate linearly:
   `r ≈ r_low + (AF_low − AF_target) / (AF_low − AF_high) × (r_high − r_low)`.
3. **Substitution.** `AF(0.12, 15) = 6.810864` (too high — NPV is still +616,694) and
   `AF(0.13, 15) = 6.462379` (too low — NPV is −2,484,828), so the root lies between.
   Interpolating: `0.12 + (6.810864 − 6.741573) / (6.810864 − 6.462379) × 0.01`.
4. **Result.** Interpolation gives **12.1988 %**; bisection to full precision gives
   **IRR = 12.192120 %**. The interpolation is high by **0.67 of a basis point**.
5. **Interpretation.** Three lessons sit in that small error. First, linear interpolation across
   a *convex* factor function always overstates the root, and the error grows with the width of
   the bracket — a one-point bracket is accurate to under a basis point, a five-point bracket is
   not. Second, the verification is free and mandatory: substitute the root back and confirm
   `NPV(IRR) = 0`, which at 12.192120 % it does to the cent. Third, and least obvious, **the
   rounding you publish is not innocent.** Quoting "12.19 %" and substituting *that* leaves a
   residual NPV of **+6,751**; quote 12.1921 % and the residual falls to **+65**. A reviewer
   testing the invariant against a two-decimal IRR will find a non-zero residual and must know
   whether they are looking at a rounding artefact or a defect. The convention that resolves it:
   **publish the rate to two decimals, retain it unrounded in the model, and run the substitution
   test on the retained value.**

**Why decision-makers love it — and where it lies.** A percentage needs no context, which is
precisely its danger. The three standing pathologies:

1. **Multiple roots.** IRR is a polynomial root; flows that change sign more than once can have
   more than one. The classic mining/decommissioning shape — invest, harvest, pay to restore —
   `(−1,000,000, +2,300,000, −1,320,000)` — satisfies NPV = 0 at **both 10 % and 20 %**;
   between them NPV is *positive* (about +1,890 at 15 %), outside, negative. Quoting "the IRR"
   of such a project is meaningless; NPV at the actual cost of capital remains well defined.
2. **Scale-blindness.** A 40 % IRR on USD 100,000 creates less value than 15 % on
   USD 20,000,000. IRR ranks intensity, NPV ranks money; mutually exclusive choices need money
   (KA 4.3.1).
3. **The reinvestment fiction.** IRR mathematically assumes interim cash is reinvested *at the
   IRR itself* — flattering for high-IRR projects, since no treasury desk reinvests at 28 %.
   MIRR (4.1.3) exists to repair exactly this.

**Worked example 4.1.2b — the project with two rates, mapped.**

Asserting that a sign-changing project has two IRRs is easy; seeing what its value actually does
is what stops a practitioner quoting one of them. Kestrel's sponsor also holds a small quarry
concession: extract for a year, then pay to restore.

1. **Setup.** Flows `(−1,000,000; +2,300,000; −1,320,000)` at t = 0, 1, 2. Two sign changes, so
   up to two roots.
2. **Formula.** `NPV(r) = −1,000,000 + 2,300,000/(1+r) − 1,320,000/(1+r)²`. Substituting
   `x = 1/(1+r)` makes it a quadratic in `x`, which is why there are at most two roots — the
   polynomial degree, not the project, sets the limit.
3. **Substitution.** Evaluate across the range rather than at one point:

| Discount rate | 0 % | 5 % | **10 %** | 14.7826 % | 15 % | **20 %** | 25 % | 30 % |
|---|---|---|---|---|---|---|---|---|
| NPV (USD) | −20,000 | −6,803 | **0** | +1,894 | +1,890 | **0** | −4,800 | −11,834 |

4. **Result.** Zeros at exactly **10 %** and **20 %**; the maximum NPV of **+1,894** occurs at
   **14.7826 %**; and the undiscounted sum is **negative** (−20,000).
5. **Interpretation.** Read the row and the pathology stops being abstract. The project creates
   value only for costs of capital *between* 10 % and 20 % — a shape that inverts every instinct
   trained on ordinary projects, where more expensive capital always destroys value. Here a
   *higher* discount rate shrinks the year-2 restoration liability faster than it shrinks the
   year-1 receipt, so cheap capital is the enemy. Note also the first cell: at a zero discount
   rate the concession loses 20,000 in plain cash, which means it is not a good project that
   discounting flatters — it is a project whose entire case rests on the time value of a
   liability. Two rules follow. **Quote no IRR when signs change more than once**; report NPV at
   the actual cost of capital, plus the range over which the sign holds, because that range is
   the real risk statement. And treat a decommissioning obligation as a financing question as
   much as an engineering one: whether the restoration is funded from a sinking fund, a bond or
   the balance sheet changes the flows this table is drawn from (Domain 15 takes this up).

**Common pitfall — comparing IRRs across different tenors.** A 3-year 18 % and a 15-year 13 %
are not ordered: the short project leaves twelve years of reinvestment risk the percentage
hides. The honest comparison is NPV over a common horizon, or EAV (KA 4.2.3).

### 4.1.3 MIRR — the modified internal rate of return

**Definition.** MIRR discounts outflows at the finance rate and compounds inflows to the
terminal date at an explicit reinvestment rate, then reads the single rate connecting the two:

```
MIRR = ( FV(inflows at r_reinvest) / PV(outflows at r_finance) )^(1/n) − 1
```

**Worked example 4.1.3 — Kestrel's honest percentage.**

1. **Setup.** The master appraisal, with both finance and reinvestment rates set to the 8 %
   opportunity cost.
2. **Formula.** Terminal value of the inflows `TV = A × FVAF(r, n)` where
   `FVAF(0.08, 15) = (1.08¹⁵ − 1)/0.08 = 27.152114`; then `MIRR = (TV/I₀)^(1/15) − 1`.
3. **Substitution.** `TV = 8,900,000 × 27.152114 = 241,653,814`;
   `MIRR = (241,653,814 / 60,000,000)^(1/15) − 1 = 4.027564^(1/15) − 1`.
4. **Result.** **MIRR = 9.73 %** — against an IRR of 12.19 %.
5. **Interpretation.** Two and a half points of Kestrel's IRR were the reinvestment fiction.
   MIRR is single-valued even for sign-changing flows and states its reinvestment assumption in
   the open — which is why credit committees increasingly ask for it alongside IRR. It remains
   a *rate*: still scale-blind, still not additive. The order of authority stands: **NPV
   decides; rates explain.**

**Worked example 4.1.3b — MIRR when the two rates are honestly different.**

Setting both rates to 8 % was a simplification that made MIRR comparable to NPV. In practice the
two rates answer different questions and are rarely equal: money is *raised* at the cost of the
facility that funds the outflows, and *reinvested* at whatever the treasury can actually earn on
short-dated deposits. Kestrel's case makes the gap concrete — a senior facility at 6.0 % against
a treasury policy that permits only investment-grade short paper at 4.0 % — and adds the mid-life
outflow every real asset has.

1. **Setup.** `I₀` = 60,000,000 at t = 0; a **membrane replacement of 6,000,000 at the end of
   year 8**; inflows 8,900,000 per year for 15 years. Finance rate `r_f` = **6.0 %** (the debt
   the outflows draw on); reinvestment rate `r_i` = **4.0 %** (treasury policy). Horizon 15 years.
2. **Formula.** `MIRR = [ FV(inflows at r_i) / PV(outflows at r_f) ]^(1/n) − 1`, with
   `FV = A × FVAF(r_i, n)` and every outflow discounted at `r_f` to t = 0.
3. **Substitution.** `PV(outflows) = 60,000,000 + 6,000,000 / 1.06⁸ = 60,000,000 + 3,764,474 =
   63,764,474`. `FVAF(0.04, 15) = 20.023588`, so
   `FV(inflows) = 8,900,000 × 20.023588 = 178,209,930`.
   `MIRR = (178,209,930 / 63,764,474)^(1/15) − 1 = 2.794815^(1/15) − 1`.
4. **Result.** **MIRR = 7.09 %** — against 9.73 % at an 8 % reinvestment rate and an IRR of
   12.19 %.
5. **Interpretation.** More than five points of the headline IRR have now been accounted for, and
   the accounting is worth reading in order — one change at a time, so that the three parts sum to
   the whole. **2.46 points** were the reinvestment fiction at the opportunity cost (12.1921 % IRR
   against 9.7327 % MIRR at an 8 % reinvestment rate). A further **2.21 points** disappear when the
   reinvestment rate alone is moved from the project's own 8 % opportunity cost to the 4 % the
   treasury can genuinely earn (MIRR 7.5273 %, still on the original outflow). The membrane
   replacement, discounted at the 6.0 % finance rate, takes the last **0.44 points** (MIRR
   7.0920 %). Those three add to **5.10 points**, which is exactly the gap between the published IRR
   and the honest MIRR — and a decomposition whose parts do not sum is the first sign that two
   changes have been made at once and one of them attributed twice. That is the whole point of
   MIRR: it does not make the project
   worse, it makes the *assumptions visible*, and every point it strips out was a point the IRR was
   quietly asserting. Note carefully what this does **not** mean. A MIRR of 7.09 % below the 8 %
   hurdle is not an accept/reject signal. Discount the same flows properly at the 8 % project
   rate — the membrane costs `6,000,000 × 0.540269 = 3,241,613` in present value — and the project
   is worth **+12,937,747**, with an IRR of **11.4250 %**: comfortably viable, on flows that
   produced a MIRR two-thirds of a point *below* the hurdle. MIRR's terminal-value construction is
   not a discounted value measure and cannot be read against a discount rate as though it were one.
   Doing so is the commonest misuse of the measure, and it kills good projects.
   **MIRR ranks and explains; it never decides.**

### AI in this KA

Appraisal is where optimisation pressure meets a suggestible tool: an assistant asked to "get
the IRR above 12 %" will find assumptions that do. The governed workflow inverts the prompt —
AI is asked to *attack* the appraisal (which assumptions move NPV most? where does the IRR
become multiple?), the analyst reruns the golden checks (NPV at the stated rate recomputed
independently; IRR verified by substitution back into the NPV equation; MIRR's terminal value
re-derived), and the professional owns the recommendation. **AI proposes; the professional
verifies, decides and remains accountable.**

### Key terms — KA 4.1

| Term | Meaning |
|---|---|
| **`NPV`** | Σ discounted cash flows − investment; the primary value measure. |
| **`IRR`** | The rate at which NPV = 0; the flows' embedded return. |
| **NPV profile** | NPV plotted against the discount rate; IRR is its zero-crossing. |
| **Multiple roots** | Two or more IRRs when flow signs change more than once. |
| **Reinvestment assumption** | NPV assumes interim cash earns `r`; IRR assumes it earns the IRR. |
| **`MIRR`** | Single rate from explicit finance and reinvestment rates. |
| **Free cash flow (project)** | Operating cash after tax, capex and working capital, **before all financing flows** — the only stream the project rate may discount. |
| **Mid-period convention** | Discounting each year's flow from its mid-point; multiplies year-end PV by `(1 + r)^0.5`. |
| **Deferred annuity factor** | `AF(r, n) − AF(r, k)`; prices a level stream that starts after year `k`. |
| **Ramp profile** | A build-up to full output; costs value against a level profile even at identical totals. |
| **Sunk cost** | Spend that no decision can avoid; excluded from every appraisal flow. |

### Sample MCQs — KA 4.1

**MCQ 4.1-A `[4.1.1 · Application]`** A project costs USD 60,000,000 and returns
USD 8,900,000 per year for 15 years; the discount rate is 8 % (`AF = 8.559479`). Its NPV is:
- A. +USD 16,179,360 ✅
- B. +USD 73,500,000
- C. −USD 16,179,360
- D. +USD 76,179,360

*Rationale:* `8,900,000 × 8.559479 − 60,000,000 = +16,179,360`. B is the undiscounted surplus
(8.9 × 15 − 60); C reverses the sign (investment minus value); D forgets to deduct the
investment at all.

**MCQ 4.1-B `[4.1.2 · Analysis]`** A project's cash flows are −1,000,000, +2,300,000,
−1,320,000. Which statement is correct?
- A. its IRR is 10 %
- B. its IRR is 20 %
- C. it has two IRRs (10 % and 20 %), so decision by IRR is indeterminate and NPV at the cost of capital must decide ✅
- D. it has no IRR, so it must be rejected

*Rationale:* Two sign changes admit two roots — both 10 % and 20 % zero the NPV, which is
positive between them. A and B are each half the truth and therefore wrong as "the" IRR; D
confuses indeterminate ranking with non-viability — at a 15 % cost of capital the NPV is
(slightly) positive.

**MCQ 4.1-C `[4.1.3 · Application]`** Using an 8 % finance and reinvestment rate, the master
appraisal's terminal value of inflows is USD 241,653,814 on an investment of USD 60,000,000
over 15 years. MIRR is closest to:
- A. 12.19 %
- B. 9.73 % ✅
- C. 8.00 %
- D. 26.85 %

*Rationale:* `(241,653,814/60,000,000)^(1/15) − 1 = 9.73 %`. A is the unmodified IRR; C is the
reinvestment rate itself; D divides the 4.03× money multiple by 15 as if returns were simple.

**MCQ 4.1-D `[4.1.1 · Analysis]`** Which property justifies NPV's primacy over IRR for
accept/reject *and* ranking decisions?
- A. NPV is easier to compute
- B. NPV is expressed as a percentage
- C. NPV is additive and scale-aware, and assumes reinvestment at the opportunity cost ✅
- D. NPV never requires a forecast

*Rationale:* The three structural properties of 4.1.1. A is irrelevant and untrue at scale;
B describes IRR, not NPV; D is false — NPV consumes the same forecasts as every measure.

**MCQ 4.1-E `[4.1.1 · Comprehension]`** Why are interest and loan repayments excluded from the
cash flows that a project discount rate is applied to?
- A. because lenders require their flows to be kept confidential
- B. because the cost of the financing is already represented inside the discount rate, so
  including it in the cash flows charges the project for its debt twice ✅
- C. because interest is not a cash flow
- D. because interest is a sunk cost

*Rationale:* The discount rate *is* the cost of capital, debt included; putting debt service in
the numerator as well double-counts it and rejects viable projects with arithmetic that is
individually correct in every cell (4.1.1). C is plainly false — interest is paid in cash. D
confuses a cost that cannot be avoided by deciding no with one that recurs because of the
decision. The mirror discipline: equity cash flow, after debt service, is discounted at the cost
of equity instead.

**MCQ 4.1-F `[4.1.3 · Comprehension]`** A paper reports the same 15-year project twice: IRR
12.19 %, and MIRR 9.73 % computed with both the finance and the reinvestment rate set to the 8 %
cost of capital. No cash flow and no rate has changed between the two lines. The 2.46-point gap
exists because:
- A. MIRR discounts the inflows twice, once to today and once to the terminal date
- B. IRR's arithmetic assumes interim cash is reinvested at the IRR itself, while MIRR compounds it
  at the rate stated — so the gap is the price of the reinvestment assumption, made explicit ✅
- C. MIRR uses the risk-free rate rather than the cost of capital
- D. the two measures answer the same question and one of them must have been mis-computed

*Rationale:* Nothing about the project changed; only the reinvestment assumption became explicit,
and 12.19 % − 9.73 % is what that assumption was worth (4.1.3). A misdescribes the construction —
inflows are compounded forward once and the result is annualised. C names a rate MIRR does not use.
D is the misreading the item exists to close off: the two measures differ by construction, so a gap
is expected rather than evidence of error, and it runs in this direction whenever the IRR exceeds
the reinvestment rate.

**MCQ 4.1-G `[4.1.3 · Evaluation]`** A sponsor's paper reports IRR 12.19 %, MIRR 7.09 % on a
4 % treasury reinvestment rate, and NPV +12,937,747 at the 8 % project rate. A committee member
moves to reject, on the grounds that MIRR is below the hurdle. The soundest response is that:
- A. the motion is correct — any return measure below the hurdle disqualifies the project
- B. MIRR is not a discounted value measure and cannot be read against a discount rate; the
  accept/reject test is the positive NPV at the owned rate, and MIRR's role here is to show how
  much of the 12.19 % was the reinvestment assumption ✅
- C. the motion is correct, because the treasury rate is the true opportunity cost
- D. MIRR should be removed from the paper to avoid confusing the committee

*Rationale:* MIRR is built from a terminal value, not a present value, so a hurdle comparison is
a category error even though both are quoted as percentages (4.1.3). B also identifies MIRR's
legitimate contribution — quantifying the reinvestment fiction — which is exactly why D is the
wrong remedy: the answer to a misread disclosure is a better-explained disclosure, not less of
it. C substitutes the reinvestment rate for the cost of capital, which are different quantities
answering different questions.

**MCQ 4.1-H `[4.1.1 · Evaluation]`** Two teams appraise competing plants. Team A reports NPV
+16,179,360 on a level 15-year inflow discounted at year-end; Team B reports +19,167,914 on the
same 8 % rate using the mid-period convention. The board must choose one plant. The professionally
sound handling is to:
- A. prefer Team B's plant, which shows the higher NPV
- B. average the two conventions to be even-handed
- C. restate both appraisals on a single declared convention before comparing, and record the
  convention in the assumption register — the 2,988,553 gap here is a modelling choice, not a
  difference in value ✅
- D. prefer Team A's plant, because year-end discounting is conservative

*Rationale:* `(1 + r)^0.5` at 8 % is 1.0392305, so mid-period reporting adds about 3.9 % of
present value to *any* project modelled that way; comparing across conventions ranks the
modellers, not the plants (4.1.1c). A and D each let the convention decide. B produces a number
that describes neither project and belongs to no convention — the appearance of fairness with none
of the substance.

### Self-check — KA 4.1

1. *State the master appraisal's three verdicts and their meanings.* — NPV +16.18m (value
   created at 8 %); IRR 12.19 % (break-even rate, 4.19-point margin); MIRR 9.73 % (return with
   honest 8 % reinvestment).
2. *Why does a mining-with-restoration cash flow break IRR?* — Two sign changes → up to two
   roots; "the" IRR does not exist.
3. *What single question decides between NPV and IRR when they disagree?* — Which project adds
   more money at the actual cost of capital — NPV's answer, by construction.
4. *The forecast total is unchanged but the profile ramps over two years. What happens to NPV,
   and why?* — It falls (16.18m → 13.70m, a loss of 2.48m): no revenue is lost, only its
   timing, and timing is what discounting prices.
5. *A colleague reports 3.9 % more NPV than you on identical flows and the same rate. What is
   the first thing to check?* — The discounting convention: `(1.08)^0.5 − 1 = 3.92 %` is exactly
   the mid-period premium at 8 %.
6. *You substitute a published IRR of 12.19 % back into the NPV equation and get +6,751 rather
   than zero. Defect or artefact?* — Artefact of two-decimal rounding; the unrounded
   12.192120 % returns zero. Run the test on the retained value, not the printed one.

---

## Knowledge Area 4.2 — The complementary measures

*Topics: 4.2.1 payback and discounted payback · 4.2.2 profitability index · 4.2.3 equivalent
annual value.*

### 4.2.1 Payback and discounted payback

**Definitions.** **Payback** is the time until cumulative cash inflows repay the investment;
**discounted payback** repeats the question in present-value terms. Neither measures value —
both measure **exposure**: how long the capital is at risk before it has come home.

**Worked example 4.2.1 — how long is Kestrel's money in the ground?**

1. **Setup.** The master appraisal: `I₀` = 60,000,000; inflows 8,900,000 per year; `r` = 8 %.
2. **Formula.** Payback = `I₀ / A` for a level stream. Discounted payback: the smallest `n`
   with `A × AF(r, n) ≥ I₀`, interpolated within the crossing year.
3. **Substitution.** Simple: `60,000,000 / 8,900,000 = 6.74`. Discounted: after year 10 the
   cumulative PV is `8,900,000 × 6.710081 = 59,719,724` — USD 280,276 short; year 11's
   discounted flow is `8,900,000 × 0.428883 = 3,817,057`; fraction `280,276 / 3,817,057 =
   0.07`.
4. **Result.** Payback **6.74 years**; discounted payback **10.07 years**.
5. **Interpretation.** The gap between the two numbers — three and a third years — is the
   price of time value: nominal repayment by year 7 does not return the capital's *worth*
   until year 10. For a 15-year concession, ten years of exposure is a risk fact the board
   should see beside the +16.2m NPV, not instead of it. The standing rule: **payback screens;
   NPV decides.** Payback's known biases — it ignores everything after the cut-off and
   penalises long-life infrastructure precisely for being long-lived — make it a veto-check,
   never a ranking.

> **Fig 4.2.1 — Two paybacks: cumulative cash versus cumulative present value.** Line chart,
> x-axis years 0–15, y-axis cumulative USD (−60m to +80m). Nominal cumulative line crossing
> zero at 6.74 (marked); discounted cumulative line crossing at 10.07 (crimson marker); both
> start at −60m. Shaded band between the lines labelled "the cost of time value". Source: PCI
> original. Alt text: two rising cumulative cash lines from minus sixty million, the nominal
> line reaching break-even three years before the discounted line.

**Worked example 4.2.1b — payback when the flows are not level.**

`I₀ / A` works only for a level stream. Real payback is read off the cumulative column and
interpolated inside the year that crosses — and the ramp profile of 4.1.1b shows why the shortcut
flatters.

1. **Setup.** The ramped forecast: 4,600,000 · 6,700,000 · then 9,400,000 for years 3–15;
   `I₀` = 60,000,000; `r` = 8 %. Total inflow identical to the level case.
2. **Formula.** Find the first year `t` where cumulative inflow ≥ `I₀`, then
   `payback = (t − 1) + (I₀ − cumulative at t − 1) / CF(t)`. For discounted payback, run the same
   procedure on the cumulative *present values*.
3. **Substitution.** Cumulative nominal: 4,600,000 · 11,300,000 · 20,700,000 · 30,100,000 ·
   39,500,000 · 48,900,000 · **58,300,000** (year 7) · 67,700,000 (year 8). The crossing is in
   year 8: shortfall `60,000,000 − 58,300,000 = 1,700,000`, and `1,700,000 / 9,400,000 = 0.1809`.
4. **Result.** Payback **7.18 years** against the level case's 6.74; discounted payback **10.91
   years** against 10.07.
5. **Interpretation.** The ramp adds **0.44 years** of nominal exposure and **0.84 years** of
   discounted exposure on cash flows whose fifteen-year total is identical to the pennies. The
   asymmetry is the lesson: discounting punishes late money twice — once in the value measure and
   again in the exposure measure — so a ramp is worse for a lender's tenor test than a
   sponsor's undiscounted payback rule reveals. This is also the cleanest available demonstration
   that **payback is a function of the profile, not of the total**, which is why "payback under
   five years" is such a poor policy: it can be satisfied or failed by rescheduling commissioning
   without changing a single dollar of forecast revenue. Interpolation within the crossing year
   assumes cash accrues evenly through it; where it does not — a seasonal offtake, a single annual
   availability payment — say so and report the payback to the whole year instead of inventing a
   decimal the data cannot support.

### 4.2.2 The profitability index

**Definition.** Value created per unit of scarce capital:

```
PI = PV(future cash flows) / I₀
```

Kestrel: `76,179,360 / 60,000,000 = **1.270**` — each invested dollar buys USD 1.27 of present
value. PI ranks *efficiency* and earns its keep in exactly one situation: **capital rationing**
(KA 4.3.2), where the question is not "is this project good?" but "which portfolio of good
projects fits the budget?". Unconstrained, PI adds nothing to NPV and can mis-rank mutually
exclusive projects of different scale — the same scale-blindness as IRR, in ratio form.

**Two definitions, one point apart.** PI is published in two forms and the difference is exactly
1.000. The **gross** (or absolute) index used above is `PV(inflows) / I₀` and its accept threshold
is **1.0**. The **net** index is `NPV / I₀` — the same information restated, threshold **0.0**.
Kestrel's are **1.269656** and **0.269656**. Neither is wrong; a paper that does not say which it
uses is, and a rationing table that mixes the two ranks projects by which analyst filled the row.
Because the two differ by a constant they always produce the **same ranking**, which is why the
ambiguity survives so long undetected — it corrupts the threshold, not the order.

**Worked example 4.2.2 — the index, its invariant, and what the ramp does to it.**

1. **Setup.** The master appraisal (`PV` = 76,179,360, `I₀` = 60,000,000, NPV = +16,179,360) and
   the ramped profile of 4.1.1b (`PV` = 73,699,840). `r` = 8 %; IRR = 12.192120 %.
2. **Formula.** `PI = PV / I₀`; the invariant to test is `PI > 1 ⇔ NPV > 0` **at the same rate**,
   with equality at the IRR by construction.
3. **Substitution.** Level: `76,179,360 / 60,000,000`. Ramped:
   `73,699,840 / 60,000,000`. At the IRR: `8,900,000 × AF(0.12192120, 15) / 60,000,000`.
4. **Result.** Level **PI = 1.269656**; ramped **PI = 1.228331**; at the IRR **PI = 1.000000**
   exactly.
5. **Interpretation.** The third figure is the useful one. `PI = 1` at the IRR is not a
   coincidence but a definition seen from another angle — the IRR is the rate at which the project
   returns exactly its capital in present value — and it gives a reviewer a second, independent
   way to audit a published IRR without re-solving anything: compute PI at the quoted rate and it
   must come to one. The first two figures carry a different lesson: the ramp costs 4.13 points of
   index, and index points are the currency of a rationing decision. A project that ranks third
   on a level-profile screening table can rank fifth once profiles are modelled honestly, and the
   two projects it displaced are the ones that never got funded. **Screen on the profile you will
   actually build.**

### 4.2.3 Equivalent annual value

**Definition.** EAV converts an NPV over a life of `n` years into the level annual amount with
the same present value:

```
EAV = NPV / AF(r, n)
```

Its natural habitat is **unequal lives**: comparing a 3-year asset with a 5-year asset by raw
NPV smuggles in the assumption that the world ends when the shorter one does.

**Worked example 4.2.3 — two pumps, unequal lives.**

1. **Setup.** Kestrel must choose a dosing-pump system. **System A**: costs 5,000,000, runs
   3 years, operating cost 800,000 per year. **System B**: costs 7,600,000, runs 5 years,
   operating cost 500,000 per year. Same duty; `r` = 8 %. (Costs, so lower is better.)
2. **Formula.** PV of cost = capex + opex × `AF(r, n)`; equivalent annual cost
   `EAC = PV / AF(r, n)`.
3. **Substitution.** A: `5,000,000 + 800,000 × 2.577097 = 7,061,678`;
   `EAC = 7,061,678 / 2.577097`. B: `7,600,000 + 500,000 × 3.992710 = 9,596,355`;
   `EAC = 9,596,355 / 3.992710`.
4. **Result.** A: **USD 2,740,168 per year**. B: **USD 2,403,469 per year** — B is cheaper by
   USD 336,699 every year, despite the 52 % higher purchase price.
5. **Interpretation.** Raw PV comparison (7.06m vs 9.60m) points the wrong way because it
   prices three years against five. EAV puts both on a per-year footing under the standard
   assumption that each system is replaced in kind at the end of its life. When that
   assumption fails — technology shifts, the duty ends — model the actual replacement chain
   instead (Domain 8's lifecycle costing).

**Worked example 4.2.3b — the master appraisal's value, per year.**

EAV is usually introduced as an unequal-lives device, which undersells it. Applied to a single
project it answers a question boards ask constantly and NPV answers badly: *how much is this worth
per year?*

1. **Setup.** The master appraisal: NPV = +16,179,360 over 15 years at 8 %.
2. **Formula.** `EAV = NPV / AF(r, n)`; the invariant is `EAV × AF(r, n) = NPV`.
3. **Substitution.** `16,179,360 / 8.559479`; then multiply back.
4. **Result.** **EAV = USD 1,890,227 per year**, and `1,890,227 × 8.559479 = 16,179,360` —
   the invariant closes to the cent.
5. **Interpretation.** Two readings, both useful in a board room. First, the plant creates about
   1.89 million a year of value beyond the cost of the capital tied up in it — a figure that can
   be set against an annual operating budget, an annual availability payment or a management fee
   in a way that a lump present value cannot. Second, and more revealing, the annual inflow
   decomposes **exactly**: capital recovery `I₀ / AF(r, n) = 60,000,000 / 8.559479 = 7,009,773`
   plus value created 1,890,227 sums to **8,900,000**, the inflow itself. So **21.24 % of every
   year's cash is value and 78.76 % is the capital coming home** — and the first of those numbers
   is the honest answer to "how much margin do we have?" Note what this decomposition also gives
   for free: 7,009,773 is precisely the annual inflow at which NPV would be zero, so the
   breakeven analysis of KA 4.3.3 is already implicit in the EAV arithmetic. One identity, three
   uses.

**Worked example 4.2.3c — the same pump choice, proved a second way.**

EAC assumes like-for-like replacement in perpetuity. The assumption is testable: replace each
system in kind over a **common horizon** — the lowest common multiple of the two lives — and
compare present values directly, with no annualisation at all. If the two methods disagree, one
of them has been mis-applied.

1. **Setup.** System A (5,000,000 capex, 3-year life, 800,000 p.a. opex) against System B
   (7,600,000 capex, 5-year life, 500,000 p.a.). `r` = 8 %. LCM of 3 and 5 is **15 years**: five
   cycles of A against three of B.
2. **Formula.** `PV_chain = Σ PV_cycle / (1 + r)^(k·life)` for `k = 0 … (cycles − 1)`, where
   `PV_cycle` is the one-life present value of costs.
3. **Substitution.** `PV_cycle(A) = 5,000,000 + 800,000 × 2.577097 = 7,061,678`, repeated at
   t = 0, 3, 6, 9, 12. `PV_cycle(B) = 7,600,000 + 500,000 × 3.992710 = 9,596,355`, repeated at
   t = 0, 5, 10.
4. **Result.** Chain A = **23,454,406**; chain B = **20,572,442**; B is cheaper by
   **2,881,964** over the common horizon. Cross-check through the annual route:
   `EAC_A × AF(0.08, 15) = 2,740,168 × 8.559479 = 23,454,406` and
   `EAC_B × AF(0.08, 15) = 2,403,469 × 8.559479 = 20,572,442` — **identical to the cent**.
5. **Interpretation.** The methods agree because they are the same statement: an equivalent annual
   cost multiplied by any horizon's annuity factor reconstructs the chain cost over that horizon.
   That exact agreement is worth more than the answer, because it is the family's verification
   rule applied to a decision rather than a formula — two independent routes, one number, and any
   disagreement localises the error immediately. What the chain view adds is **visibility of the
   assumption**: laid out over fifteen years, the five replacement events for A and three for B
   are explicit, and a reviewer can ask whether a fifth generation of the same pump will really be
   available and priced at 5,000,000 in year twelve. Where the honest answer is no — and for
   anything with a technology curve it usually is — neither method should be trusted past the
   first replacement, and the comparison becomes a staged one (KA 4.A.4). EAC's convenience is
   that it hides the chain; its danger is exactly the same.

### AI in this KA

Screening portfolios is high-volume, formulaic work — the natural first place an organisation
automates appraisal, and the first place systematic error industrialises. Two governed habits:
the screening tool's formulae are validated once against this domain's golden examples (the
registry pattern, `_build/verify_formulas.py`), and every auto-screened rejection above a
materiality line gets a human review — a portfolio can lose its best project to one wrong sign
convention, silently, forever.

### Key terms — KA 4.2

| Term | Meaning |
|---|---|
| **Payback / discounted payback** | Years for cumulative (discounted) inflows to repay `I₀`; exposure measures. |
| **`PI`** | PV of inflows / `I₀`; value per scarce dollar, for rationing. |
| **`EAV` / EAC** | NPV (cost PV) converted to a level annual equivalent via `AF(r, n)`. |
| **Unequal lives** | The comparison EAV exists to make honest. |
| **Replacement chain** | The explicit alternative when like-for-like replacement fails. |
| **Gross vs net `PI`** | `PV/I₀` (threshold 1.0) vs `NPV/I₀` (threshold 0.0); same ranking, different threshold. |
| **Capital-recovery annuity** | `I₀ / AF(r, n)`; the annual inflow at which NPV is exactly zero. |
| **Common horizon (LCM)** | Comparing replacement chains over the lowest common multiple of the lives. |

### Sample MCQs — KA 4.2

**MCQ 4.2-A `[4.2.1 · Application]`** For the master appraisal (60m in, 8.9m/yr, 8 %), the
discounted payback is closest to:
- A. 6.74 years
- B. 10.07 years ✅
- C. 8.00 years
- D. 15.00 years

*Rationale:* Cumulative PV reaches 59.72m after year 10; the 0.28m shortfall is 7 % of year
11's 3.82m discounted flow → 10.07. A is the simple payback; C confuses the discount rate with
a duration; D is the whole life.

**MCQ 4.2-B `[4.2.3 · Application]`** System A (PV of costs 7,061,678 over 3 years,
`AF = 2.577097`) versus System B (PV 9,596,355 over 5 years, `AF = 3.992710`). The correct
comparison and choice is:
- A. raw PV: A is cheaper, choose A
- B. equivalent annual cost: A 2,740,168 vs B 2,403,469 — choose B ✅
- C. equivalent annual cost: A 2,353,893 vs B 1,919,271 — choose B
- D. purchase price: A is cheaper, choose A

*Rationale:* Unequal lives require EAC: `7,061,678/2.577097 = 2,740,168` vs
`9,596,355/3.992710 = 2,403,469`. A and D compare unlike horizons; C divides each PV by its
raw life in years (÷3 and ÷5), annualising without discounting — the right instinct with the
wrong arithmetic.

**MCQ 4.2-C `[4.2.1 · Analysis]`** A board adopts "payback under 5 years" as its sole
investment criterion. The predictable portfolio distortion is:
- A. none — payback is conservative, so the portfolio is safe
- B. systematic bias against long-lived infrastructure and toward short-cycle projects, regardless of value created ✅
- C. excessive investment in high-NPV projects
- D. elimination of all risk

*Rationale:* Payback ignores everything beyond its cut-off, so a 15-year concession with NPV
+16m loses to a 4-year project with NPV +1m. That is a value distortion, not conservatism (A);
C reverses the effect; D confuses shorter exposure with no risk.

**MCQ 4.2-D `[4.2.2 · Recall]`** The profitability index earns its place in appraisal when:
- A. projects are mutually exclusive and differ in scale
- B. capital is rationed and the question is which portfolio of positive-NPV projects fits the budget ✅
- C. cash flows change sign more than once
- D. lives are unequal

*Rationale:* PI ranks value per scarce dollar — the rationing question exactly. A is where PI
(like IRR) mis-ranks by scale; C is MIRR's territory; D is EAV's.

**MCQ 4.2-E `[4.2.2 · Comprehension]`** A screening pack reports one project at "PI 1.27" and
another at "PI 0.31". Before any comparison, the analyst must establish that:
- A. both indices use the same definition — gross `PV/I₀` (threshold 1.0) or net `NPV/I₀`
  (threshold 0.0) — because the two differ by exactly 1.000 ✅
- B. the second project has been rejected, since its index is below 1.0
- C. both projects have the same life
- D. the discount rate exceeds the IRR in both cases

*Rationale:* The two published forms differ by a constant, so a mixed table corrupts the accept
threshold while leaving the ranking intact — which is why the defect survives review (4.2.2).
B commits exactly that error: 0.31 on the net definition is a healthy project. C matters for EAV,
not PI. D is unrelated to either definition.

**MCQ 4.2-F `[4.2.1 · Comprehension]`** The master appraisal's simple payback is 6.74 years and its
discounted payback 10.07 years, on identical cash flows. What does the three-and-a-third-year gap
between the two figures tell the board?
- A. that discounting has removed three and a third years of cash flow from the forecast
- B. that repaying the 60,000,000 in nominal cash by year seven does not return the capital's
  *worth* until year ten — the gap is the price of time value, and both figures measure exposure
  duration rather than value ✅
- C. that the 8 % rate is too high for a fifteen-year concession
- D. that the two measures disagree, so one of them has been computed on the wrong cash flows

*Rationale:* The same flows produce both numbers; the later crossing is what it costs to require
recovery in present value rather than in nominal cash, and neither figure says anything about how
much value the project creates (4.2.1). A confuses discounting a stream with shortening it. C reads
a rate judgement out of an arithmetic consequence that holds at every positive rate. D treats an
expected relationship — discounted payback is always the later of the two unless `r` = 0 — as a
defect.

**MCQ 4.2-G `[4.2.3 · Evaluation]`** An asset manager must choose between a 3-year and a 5-year
dosing system. EAC favours the 5-year system by 336,699 a year; the 15-year replacement chain
favours it by 2,881,964 in present value; the two agree exactly. The plant, however, is on a
concession with **seven years** left to run and no renewal right. The soundest position is that:
- A. choose the 5-year system — both methods agree, and agreement is the strongest evidence
  available
- B. choose the 3-year system, because it is cheaper to buy
- C. neither number answers the question asked: both price perpetual like-for-like replacement,
  and over a seven-year duty the relevant comparison is the actual cost of each option to
  concession end, including residual value or removal ✅
- D. average the two methods and choose the cheaper

*Rationale:* The exact agreement of EAC and the chain method is real but proves only internal
consistency — they encode the *same* assumption, so agreeing cannot validate it (4.2.3c). A
mistakes consistency for applicability. Over a seven-year duty the 3-year system implies two full
cycles and one stranded year, the 5-year implies one cycle and two stranded years, and residual
value decides; that is a different calculation, not a correction to these. D averages two answers
to a question nobody asked.

**MCQ 4.2-H `[4.2.1 · Evaluation]`** A sponsor's board holds a standing rule: reject anything with
discounted payback beyond nine years. The Kestrel appraisal shows NPV +16,179,360, IRR 12.19 %
and discounted payback 10.07 years. The professionally sound recommendation is to:
- A. reject the project, since the rule is clear
- B. recommend the project on its value, and put the rule itself to the board as the decision it
  actually is — a stated tolerance for exposure duration that here costs 16.18 million of value,
  with the exposure disclosed rather than hidden ✅
- C. reprofile the model until payback falls under nine years
- D. reject the project but note the NPV in an appendix

*Rationale:* Payback screens and NPV decides (4.2.1); a screening rule that vetoes a positive-NPV
project is a policy choice about exposure, and the leader's obligation is to surface it with the
value at stake attached, not to let a threshold decide silently. C is model manipulation to satisfy
a rule — the dishonesty of MCQ 4.3-C's distractor D in a different costume. A and D both apply the
rule as though it were an appraisal result; D adds the pretence of disclosure while burying the
finding where it changes nothing.

### Self-check — KA 4.2

1. *Why is discounted payback always later than simple payback?* — Discounting shrinks every
   inflow, so the cumulative line climbs more slowly (equal only if `r` = 0).
2. *State Kestrel's PI and its meaning.* — 1.270: each dollar invested buys 1.27 dollars of
   present value.
3. *What assumption does EAV smuggle in, and when must you drop it?* — Like-for-like
   replacement in perpetuity; drop it when the duty ends or technology shifts, and model the
   real chain.
4. *Decompose Kestrel's 8,900,000 annual inflow.* — 7,009,773 capital recovery
   (`I₀ / AF`) + 1,890,227 value created (EAV); 78.76 % / 21.24 %.
5. *What single figure proves a published IRR without re-solving it?* — PI at that rate: it must
   equal 1.000 exactly.
6. *The ramp profile of 4.1.1b has the same fifteen-year total. What does it do to payback?* —
   Lengthens it: 6.74 → 7.18 nominal, 10.07 → 10.91 discounted. Payback prices the profile, not
   the total.

---

## Knowledge Area 4.3 — Decision contexts: exclusivity, rationing and judgment

*Topics: 4.3.1 mutually exclusive investments · 4.3.2 capital rationing · 4.3.3 the limits of
the numbers.*

### 4.3.1 Mutually exclusive investments

**The conflict.** Choosing one project *instead of* another is where NPV and IRR famously
disagree. Kestrel's board weighs two intake designs (same risk class, `r` = 8 %):

| Design | `I₀` | Net inflow × 5 yrs | NPV @ 8 % | IRR |
|---|---|---|---|---|
| **P (compact)** | 5,000,000 | 2,000,000 | **+2,985,420** | **28.65 %** |
| **Q (full-scale)** | 20,000,000 | 6,000,000 | **+3,956,260** | **15.24 %** |

IRR shouts P; NPV says Q adds USD 970,840 more money. The tie-breaker is the **incremental
project** Q−P: invest a further 15,000,000 for a further 4,000,000 per year — an incremental
IRR of **10.42 %**, above the 8 % cost of capital. The extra capital earns its keep, so take
the bigger project. **Rule: when exclusivity forces a choice, rank by NPV; use incremental IRR
only to narrate the same answer.** (At any hurdle above 10.42 % the ranking genuinely flips —
the crossover rate is where the two NPV profiles intersect.)

> **Fig 4.3.1 — Two NPV profiles and the crossover.** Line chart, x-axis discount rate 0–30 %,
> y-axis NPV (USD millions). Q's profile starts higher (+10.0 at 0 %) and falls steeply
> through zero at 15.24 %; P's starts lower (+5.0) and falls gently through zero at 28.65 %.
> The curves cross at **10.42 %** (crimson marker, "crossover — ranking flips"); dashed
> vertical at the 8 % hurdle where Q leads. Source: PCI original. Alt text: two downward
> sloping value curves crossing at about ten per cent, the larger project leading at low
> discount rates and the smaller at high rates.

**Worked example 4.3.1 — the crossover, derived exactly and then proved.**

The crossover rate is usually read off a chart. Deriving it takes one line of algebra, and the
derivation produces a check that no chart can give.

1. **Setup.** P: `I₀` 5,000,000, inflow 2,000,000 for 5 years. Q: `I₀` 20,000,000, inflow
   6,000,000 for 5 years. Same risk class.
2. **Formula.** The profiles intersect where their NPVs are equal, which is where the
   **incremental** project Q−P has zero NPV — so the crossover rate *is* the incremental IRR.
   Setting `2,000,000 · AF − 5,000,000 = 6,000,000 · AF − 20,000,000` gives
   `4,000,000 · AF = 15,000,000`, i.e. `AF(r, 5) = 3.750000`.
3. **Substitution.** Solve `AF(r, 5) = 3.75` for `r`; then substitute the root back into **both**
   original projects.
4. **Result.** **Crossover = 10.424845 %.** At that rate `NPV_P = 2,000,000 × 3.75 − 5,000,000 =
   2,500,000` and `NPV_Q = 6,000,000 × 3.75 − 20,000,000 = 2,500,000` — **equal, exactly, to the
   dollar.**
5. **Interpretation.** The exactness is the point. Because the crossover falls out of a single
   factor value (3.750000), both NPVs at that rate are round by construction, and any model
   reporting a crossover where the two NPVs differ has an error — in the flows, in the factor, or
   in the root. Read the geometry with the algebra and the whole decision becomes legible: below
   10.42 % the extra 15,000,000 that Q requires earns more than that capital costs, so Q leads;
   above it, P leads; at the 8 % hurdle Q leads by 970,840, which is 2.42 points of hurdle away
   from indifference. That last figure is the one to put in the paper, because it answers the
   question a director will actually ask — *how wrong would our rate have to be to change this
   decision?* Here: 2.42 points, a comfortable margin. Where the answer comes back at twenty basis
   points, the ranking is not a finding but a coin toss with decimals, and the choice should be
   made on the qualitative grounds instead — and recorded as having been made that way.

| Discount rate | 0 % | 8 % | **10.4248 %** | 15 % | 15.2382 % | 20 % | 28.6493 % |
|---|---|---|---|---|---|---|---|
| NPV_P (USD) | 5,000,000 | 2,985,420 | **2,500,000** | 1,704,310 | **1,666,667** | 981,224 | **0** |
| NPV_Q (USD) | 10,000,000 | 3,956,260 | **2,500,000** | 112,931 | **0** | −2,056,327 | **−5,000,000** |
| Leader | Q | Q | tie | P | P | P | P |

Two features of the row deserve a moment. Q's profile is **steeper** — its zero-crossing is at
15.24 % against P's 28.65 % — because Q's value is concentrated in the same five years but on
three times the capital; steepness is duration-and-scale sensitivity made visible. And P's IRR of
28.65 %, the number that would have won the argument in most rooms, corresponds to a rate at which
Q has already destroyed exactly five million. A percentage that high tells you the small project is
*efficient*; the table tells you the organisation would be poorer for choosing it.

**The two cross-cells are exact, and that is a third invariant.** At either project's own IRR the
annuity factor is pinned by that project's own ratio `I₀ / A`, so the *other* project's NPV there is
a round number by construction. At P's IRR the factor is `5,000,000 / 2,000,000 = 2.500000`, so
`NPV_Q = 6,000,000 × 2.5 − 20,000,000 = −5,000,000` to the dollar; at Q's IRR the factor is
`20,000,000 / 6,000,000 = 3.333333…`, so `NPV_P = 2,000,000 × 10/3 − 5,000,000 = 1,666,667`. A
profile table whose cross-cells are *not* round has been drawn on a published two-decimal rate
rather than the retained one — the rounding trap of 4.1.2 reappearing in a presentation table
instead of a check block, and a defect worth catching because a reader who spots it will trust
nothing else in the exhibit.

### 4.3.2 Capital rationing

**The problem.** When the budget cannot fund every positive-NPV project, maximise **NPV per
budget dollar** — rank by PI and pack the budget:

| Project | `I₀` (m) | NPV (m) | PI | Cumulative `I₀` |
|---|---|---|---|---|
| W | 8 | 2.40 | 1.300 | 8 |
| X | 12 | 3.00 | 1.250 | 20 ← budget |
| Y | 10 | 2.20 | 1.220 | — |
| Z | 6 | 1.02 | 1.170 | — |

With a USD 20m budget the PI ranking funds **W + X for NPV +5.40m**; the tempting "biggest
NPV first, then fill" (X, then Y won't fit, then W) lands on the same 5.40m here, but the
next-best feasible set (W + Y, +4.60m) shows what a wrong packing costs. Discipline points:
PI packing is exact only when projects are divisible or the budget binds once — lumpy
multi-period rationing is an optimisation problem (Domain 6's model does it honestly); and
rationing itself deserves challenge, because turning away a 1.17-PI project is a financing
failure as often as a screening success (Domain 9).

**Worked example 4.3.2 — the ranking rule that funds the wrong portfolio.**

The table above is the comfortable case: PI order and value maximisation coincide. Add one small,
attractive project and they part company — which is why "rank by PI and fill the budget" must be
taught as a heuristic and never as a rule.

1. **Setup.** The same USD 20,000,000 budget, now with five candidates. All are independent, all
   have positive NPV, and each is indivisible — you fund it or you do not.

| Project | `I₀` (USD) | NPV (USD) | `PI` |
|---|---|---|---|
| W | 8,000,000 | 2,400,000 | **1.3000** |
| V | 4,000,000 | 1,160,000 | **1.2900** |
| X | 12,000,000 | 3,000,000 | **1.2500** |
| Y | 10,000,000 | 2,200,000 | **1.2200** |
| Z | 6,000,000 | 1,020,000 | **1.1700** |

2. **Formula.** Greedy PI: take projects in descending index order, skipping any that no longer
   fits, until the budget is exhausted. Then compare against the value-maximising set found by
   **enumerating every feasible combination** — with five projects there are 31 non-empty subsets, of
   which **16** fit the budget (all five singles, nine of the ten pairs, and two of the ten triples;
   no set of four can fit, since the four cheapest already total 28,000,000), so exhaustive search is
   a minute's work, not a research problem.
3. **Substitution.** Greedy takes W (8,000,000; 12,000,000 left), then V (4,000,000; 8,000,000
   left), then skips X (12,000,000 — will not fit) and Y (10,000,000 — will not fit), then takes Z
   (6,000,000; 2,000,000 left and nothing to spend it on). Enumeration tests all sixteen feasible
   sets.
4. **Result.** Greedy funds **W + V + Z for NPV +4,580,000** on 18,000,000 spent. The
   value-maximising set is **W + X for NPV +5,400,000** on 20,000,000 spent exactly. Greedy leaves
   **USD 820,000 of value unfunded** — and leaves 2,000,000 of budget unspent while doing it.
5. **Interpretation.** The mechanism is worth naming, because it is not an arithmetic slip: greedy
   PI maximises value *per dollar committed*, and the budget constraint asks for value *per dollar
   available*. V, with the second-best index, is what causes the damage — taking it consumes four
   million that then cannot be combined with X's twelve, and the fragment left over is unusable.
   High-index small projects are the classic trap, and they are precisely the ones a screening
   committee finds most attractive. Three disciplines follow. **Use PI to shortlist, then enumerate
   or optimise** — the exhaustive check is cheap up to about twenty candidates and standard solvers
   handle the rest (Domain 6). **Report the unspent budget**, because 2,000,000 idle beside a
   rejected 1,020,000-NPV project is a visible symptom of the defect. And **report the runner-up
   set with its value**, so the board sees the cost of the packing it approved rather than a single
   list presented as though it were the only one. Note finally that this whole exercise assumes the
   budget is genuinely fixed; 4.3.2's standing challenge applies with more force here, since
   820,000 of foregone value is a strong argument for finding two million more.

**Where the budget binds twice.** A single-period budget is already a simplification. Real capital
plans bind in each of several years, and a project's spend profile — not just its total — decides
whether it fits. A two-year-spend project can be feasible on totals and infeasible in year one, and
no index computed on total `I₀` can see that. This is the point at which appraisal hands the
problem to modelling: the constraint set belongs in the model with one row per period, and the
answer is an optimisation the spreadsheet performs and the professional interrogates (Domain 6,
KA 6.3). What appraisal keeps is the obligation to state which constraints were imposed, because a
portfolio is only optimal with respect to the constraints someone chose to write down.

### 4.3.3 The limits of the numbers

Every measure in this domain consumes forecasts and a rate, and both are judgments wearing
decimals. The professional frame: **appraisal quantifies; it does not decide strategy.**
Numbers cannot see option value (the pilot that buys the right to scale), strategic
foreclosure (the market position lost by not building), or the asymmetry of forecast error
(Domain 8's ranges belong beside every point NPV). The board paper this domain endorses shows:
NPV at the owned rate; the sensitivity of that NPV to the two or three assumptions that
dominate it; IRR/MIRR as narrative; exposure via discounted payback; and the explicit
statement of what the numbers cannot price. Anything less is theatre with spreadsheets.

**Worked example 4.3.3 — the two numbers that replace a point estimate.**

A single NPV invites a single question ("is it positive?"). Two constructions turn the same model
into a decision document: the **breakeven** — how far can the dominant assumption fall before value
disappears — and the **two-way table** that shows where the sign changes across the pair of
assumptions that actually dominate.

1. **Setup.** The master appraisal: `I₀` = 60,000,000, inflow 8,900,000 for 15 years, `r` = 8 %.
   The two dominating assumptions are the annual net inflow (a function of tariff and availability)
   and the discount rate.
2. **Formula.** Breakeven inflow: set NPV = 0, so `A* = I₀ / AF(r, n)` — the capital-recovery
   annuity already computed in 4.2.3b. Breakeven rate: the IRR. Then tabulate
   `NPV = A · m · AF(r, 15) − I₀` across multipliers `m` and rates `r`.
3. **Substitution.** `A* = 60,000,000 / 8.559479 = 7,009,773`, which is
   `(8,900,000 − 7,009,773) / 8,900,000 = 21.24 %` below the base case. The rate breakeven is the
   IRR, 12.1921 %, which is 4.19 points above the hurdle.
4. **Result.**

| Inflow vs base | 6 % | 7 % | **8 %** | 9 % | 10 % | 12.1921 % |
|---|---|---|---|---|---|---|
| **80 %** (7.12m) | +9,151,213 | +4,848,348 | **+943,488** | −2,607,898 | −5,844,714 | **−12,000,000** |
| **90 %** (8.01m) | +17,795,114 | +12,954,391 | **+8,561,424** | +4,566,114 | +924,697 | **−6,000,000** |
| **100 %** (8.90m) | +26,439,016 | +21,060,435 | **+16,179,360** | +11,740,127 | +7,694,108 | **0** |
| **110 %** (9.79m) | +35,082,918 | +29,166,478 | **+23,797,296** | +18,914,140 | +14,463,518 | **+6,000,000** |

The final column is computed on the **retained** IRR (12.192120 %), not the published two-decimal
figure, and it is exact for a reason worth knowing: at the IRR the present value of the base inflow
equals `I₀` by definition, so scaling the inflow by a multiplier `m` gives
`NPV = m × I₀ − I₀ = (m − 1) × I₀` — 60,000,000 × (−0.20, −0.10, 0, +0.10). Run the same column on
the published 12.19 % instead and every cell shifts by roughly six thousand while the 100 % cell
stops being zero. Either convention is defensible; **mixing them inside one exhibit is not**, and
this column is the standing example of the discipline in 4.1.2 — publish two decimals, retain the
root, compute from the root.

5. **Interpretation.** The table says something no point estimate can: the project survives a
   **21.24 % revenue shortfall** at the board's rate, and survives a 20 % shortfall at any rate up
   to about 8.2 % — but a 20 % shortfall *combined* with a 9 % cost of capital destroys value. The
   professional discipline is to read the **joint** cell rather than two separate sensitivities,
   because the two assumptions are not independent in reality: the conditions that depress a water
   project's availability revenue (a weak offtaker, a stressed sector) are the same conditions that
   raise its cost of capital, so the down-left cells of this table are not remote corners but a
   coherent scenario. That is the difference between sensitivity analysis and scenario analysis, and
   Domain 8 formalises it. Two further readings for the board paper. The 21.24 % headroom is the
   figure to negotiate against — it tells the sponsor how much tariff can be conceded before the
   investment case, not merely the profit, is gone. And the bottom-right cell (**+6,000,000** at the
   IRR on a 110 % case) shows that even the breakeven rate is a breakeven only for the base case;
   an IRR is a statement about one forecast, and quoting it without the forecast is the omission
   this whole Knowledge Area exists to prevent. The linearity of that last column — exactly ten per
   cent of `I₀` per ten per cent of inflow — is also the sharpest available statement of how little
   an IRR contains: at the rate where the project is worth nothing on the base forecast, a tenth
   more revenue is worth precisely a tenth of the outlay, and no percentage can carry that
   information because it has divided the scale out.

### AI in this KA

Ranking under constraints is combinatorial — machine territory — and constraint errors are
silent. The governed pattern from Domain 6 applies unchanged: the optimiser proposes the
portfolio; the analyst verifies the constraint set against reality (is the budget truly
annual? are W and X really independent?); golden checks re-verify the NPVs being packed; and
the leader owns the strategic overrides the model cannot see (4.3.3) — recorded as decisions,
not adjustments smuggled into assumptions.

### Key terms — KA 4.3

| Term | Meaning |
|---|---|
| **Mutually exclusive** | Choosing one forecloses the other; rank by NPV. |
| **Incremental IRR** | IRR of the difference project; narrates the NPV ranking. |
| **Crossover rate** | Rate where two NPV profiles intersect and rankings flip. |
| **Capital rationing** | Budget binds before value runs out; pack by PI. |
| **Option value** | Value of flexibility the static NPV cannot see. |
| **Greedy PI packing** | Descending-index selection; a heuristic that can leave value and budget unused. |
| **Breakeven inflow** | `I₀ / AF(r, n)`; the revenue level at which NPV reaches zero. |
| **Two-way sensitivity** | NPV tabulated across the two dominating assumptions jointly, not separately. |
| **Indivisible project** | Funded whole or not at all; the reason enumeration replaces sorting. |

### Sample MCQs — KA 4.3

**MCQ 4.3-A `[4.3.1 · Application]`** P: NPV +2,985,420, IRR 28.65 %. Q: NPV +3,956,260, IRR
15.24 %. Cost of capital 8 %; only one may proceed. The correct choice is:
- A. P — higher IRR
- B. Q — higher NPV, confirmed by an incremental IRR of 10.42 % above the hurdle ✅
- C. P — lower investment is always safer
- D. both, split the budget

*Rationale:* Exclusive choices rank by money added: Q adds USD 970,840 more, and the extra
15m earns 10.42 % > 8 %. A is the scale-blindness pathology; C prices fear, not value; D
violates the premise.

**MCQ 4.3-B `[4.3.2 · Application]`** Budget USD 20m. Projects (I₀, NPV): W (8, 2.4),
X (12, 3.0), Y (10, 2.2), Z (6, 1.02). The value-maximising funded set is:
- A. X and Y
- B. W and X: NPV +5.40m ✅
- C. W, Y and Z
- D. X and Z

*Rationale:* PI order W (1.300), X (1.250) packs the budget exactly for +5.40m. A needs 22m;
C needs 24m; D fits (18m) but yields +4.02m — a 1.38m sacrifice for leaving W unfunded.

**MCQ 4.3-C `[4.3.3 · Analysis]`** A pilot plant shows NPV −800,000, but building it creates
the option to deploy at 20× scale if the technology proves. The appraisal-sound treatment is:
- A. reject — negative NPV is disqualifying
- B. approve by overriding the NPV silently
- C. present the static NPV alongside an explicit valuation (or structured judgment) of the scaling option, and decide on the combined case, recorded as such ✅
- D. raise the forecast cash flows until NPV turns positive

*Rationale:* Static NPV cannot see option value; the remedy is to price or judge the option
*in the open*. A discards real value; B and D are the same dishonesty at different altitudes —
one hides the judgment, the other disguises it as a forecast.

**MCQ 4.3-D `[4.3.1 · Recall]`** The crossover rate of two NPV profiles is:
- A. the rate at which both projects' NPVs equal zero
- B. the rate at which the two NPVs are equal — above it, the ranking flips ✅
- C. the average of the two IRRs
- D. the cost of capital

*Rationale:* Crossover is the intersection of profiles (equivalently the incremental
project's IRR). A describes two separate IRRs; C is arithmetic superstition; D is a property
of the firm, not of the pair.

**MCQ 4.3-E `[4.3.2 · Comprehension]`** Why does ranking indivisible projects by descending
profitability index sometimes fund a portfolio worth less than the best feasible one?
- A. because the index is computed at the wrong discount rate
- B. because the index maximises value per dollar *committed*, while the budget constraint asks for
  value per dollar *available* — so a high-index small project can consume capital that a larger
  project then cannot use, stranding a fragment ✅
- C. because the index ignores the discount rate entirely
- D. because indivisible projects always have lower NPVs

*Rationale:* The mechanism, not an arithmetic error: greedy selection is exact for divisible
projects and only a heuristic for lumpy ones (4.3.2). A and C misdescribe the index, which is built
from a present value at the project rate. D is simply untrue.

**MCQ 4.3-F `[4.3.3 · Comprehension]`** A water project's sensitivity exhibit tabulates NPV across
annual net inflow (80 % to 110 % of base) and discount rate (6 % to 10 %) jointly. Taken one at a
time, the project survives a 20 % inflow shortfall and survives a 9 % cost of capital; taken
together, those two movements destroy value. Why must the analyst read the joint cells rather than
the two single-assumption sensitivities?
- A. because joint cells are easier to compute
- B. because the assumptions are correlated in reality — the conditions that depress availability
  revenue also tend to raise the cost of capital — so the combined case is a coherent scenario
  rather than a remote corner ✅
- C. because single-assumption sensitivities are arithmetically invalid
- D. because the discount rate has no effect on NPV in isolation

*Rationale:* Individually the project survives a 20 % revenue shortfall and survives a 9 % rate;
jointly it does not, and correlation makes that combination realistic rather than extreme (4.3.3).
C is false — one-way sensitivities are valid, merely insufficient. D contradicts the table's own
rows.

**MCQ 4.3-G `[4.3.2 · Evaluation]`** A committee's rationing pack presents one funded set, ranked by
PI, spending 18,000,000 of a 20,000,000 budget for NPV +4,580,000. You establish overnight, by
enumeration, that a feasible set spends the full 20,000,000 for +5,400,000. Every NPV in the pack is
correct. The board meets tomorrow and the pack has already circulated. The soundest course is to:
- A. let the recommendation stand and log the finding for the next cycle: 820,000 is 15 % of the
  value on the table but the pack's arithmetic is sound, and re-opening a circulated
  recommendation the night before a board costs the committee credibility it will need later
- B. table a one-page addendum carrying the enumerated set, the 2,000,000 the recommendation leaves
  unspent and the runner-up set with its NPV, so the board approves a portfolio knowing that the
  packing it was offered costs 820,000 ✅
- C. withdraw the recommendation and present both sets without one, on the grounds that choosing
  between them is the board's decision rather than the committee's
- D. keep the PI ranking as the recommendation, since greedy indexing is the committee's
  established method, and record the enumeration in an appendix for completeness

*Rationale:* The defect is disclosure, not arithmetic, and the remedy is proportionate to it: one
page, in time, changes the decision the board actually takes (4.3.2). A is a real argument — process
discipline has value and the number is not enormous — but it trades 820,000 of shareholder value for
the committee's comfort, and the asymmetry decides it. C over-corrects: the committee is paid to
recommend, and handing up two sets with no view transfers work rather than judgement. D is the
subtlest wrong answer, because it discloses the finding while leaving it where nothing depends on it
— an appendix that contradicts the recommendation is not disclosure, it is cover.

**MCQ 4.3-H `[4.3.3 · Evaluation]`** A sponsor's appraisal shows Kestrel surviving a 21.24 % revenue
shortfall at the board's rate. In tariff negotiation the offtaker asks for a 15 % reduction. The
soundest use of that headroom figure is:
- A. concede up to 21.24 %, since the project remains value-positive throughout
- B. treat 21.24 % as the point at which the *investment case* fails, not as negotiating room —
  a 15 % concession leaves 6.24 points of revenue headroom against every other adverse assumption
  combined, and on the reduced tariff the project breaks even at a cost of capital of 9.28 %, so
  barely 1.3 points of rate movement would exhaust what remains ✅
- C. refuse any reduction, since the base case is the only defensible position
- D. concede 15 % and re-run the model to show a positive NPV afterwards

*Rationale:* Breakeven headroom is a buffer against *all* remaining uncertainty, not a budget to
spend on one counterparty; consuming three-quarters of it in a negotiation leaves the project
exposed to the correlated rate movement the table already prices (4.3.3). A spends the entire
buffer. C mistakes a forecast for a position. D is B's arithmetic with the judgement removed — the
model will indeed show a positive NPV, which is precisely why the number alone cannot settle it.

### Self-check — KA 4.3

1. *Why does incremental IRR always agree with NPV on a pairwise choice?* — It tests whether
   the *difference* project clears the hurdle, which is exactly what the NPV difference
   measures.
2. *When does PI packing fail as a rationing rule?* — Lumpy projects and multi-period budget
   constraints; then it's an optimisation, not a sort.
3. *Name three things a positive NPV cannot tell the board.* — Exposure duration; sensitivity
   concentration; the value of flexibility and strategic position outside the modelled flows.
4. *Both NPVs at a computed crossover rate come to 2,500,000 exactly. Coincidence?* — No: the
   crossover falls out of one factor value (`AF = 3.75`), so both NPVs are determined by it.
   Unequal NPVs at a reported crossover are proof of an error.
5. *State Kestrel's revenue headroom and what it is for.* — 21.24 % (breakeven inflow 7,009,773);
   it is the buffer against every remaining adverse assumption, not room to concede in one
   negotiation.
6. *Greedy PI selects W, V, Z for +4,580,000 on an 18,000,000 spend. What two facts must the
   board be shown?* — The 2,000,000 unspent, and the runner-up set (W + X, +5,400,000) — together
   they disclose the 820,000 the packing costs.

---

## Advanced topics — Domain 4

### 4.A.1 Reading NPV profiles like a professional

The profile (Fig 4.1.1) compresses the whole appraisal into one curve: its height at 0 % is
the undiscounted surplus; its slope measures duration-sensitivity (long-dated flows steepen
it); its zero-crossing is the IRR; and the *distance* between the hurdle and the crossing is
the margin for rate error. Two profiles per decision (Fig 4.3.1) add the crossover — the
complete geometry of a mutually exclusive choice. A reviewer who asks for the profile instead
of the point estimate has converted an argument about numbers into an inspection of shape.

### 4.A.2 Inflation-consistent appraisal

The Fisher discipline of Domain 3 (KA 3.3.1) binds hardest here: nominal flows with the
nominal rate, or real flows with the real rate — and the same world for *every* line,
including the terminal value. The classic appraisal defect is a nominal hurdle (board-set,
inflation-inclusive) discounting real flows (an engineer's "today's money" forecast): NPV is
systematically understated and good projects die politely. The assumption register names the
world; the reviewer checks one line item end to end.

### 4.A.3 Where the 8 % comes from — reconciling the hurdle to the structure

Every worked example in this domain has taken the board's 8.0 % as given. That is legitimate
pedagogy and indefensible practice: a rate assumed is a conclusion assumed, and this domain's own
sensitivity table shows a single point of rate moving Kestrel's NPV by roughly four and a half
million. The rate must be **derived from the structure that will actually be signed**, and the
derivation belongs to Domain 9. Closing that loop here is what turns four chapters into a book.

**What Domain 9 derives.** At the proposed structure — 70 % senior gearing, 42,000,000 of debt at
6.0 % over 12 years, an after-tax cost of debt of 4.80 % and a re-levered cost of equity of 15.42 %
— the weighted average cost of capital is `WACC = 0.70 × 4.80 + 0.30 × 15.42 =` **7.9860 %**
(Domain 9, KA 9.1.4). Domain 9 also reports the WACC at the **coverage-binding** gearing, the most
debt the lenders' cover test will actually carry, at **8.0001 %**.

**What the difference is worth.** Recomputing the master appraisal at each rate:

| Rate | Source | `AF(r, 15)` | NPV (USD) |
|---|---|---|---|
| **7.9860 %** | Domain 9 WACC at the proposed 70/30 structure | 8.56680051 | **+16,244,525** |
| **8.0000 %** | the board's stated appraisal hurdle | 8.55947869 | **+16,179,360** |
| **8.0001 %** | Domain 9 WACC at the coverage-binding gearing | 8.55942642 | **+16,178,895** |

The board's round 8 % sits **1.40 basis points above** the structure's true cost of capital and so
understates value by **USD 65,164** — about **46,546 per basis point**. Three things follow, in
ascending order of importance.

First, the direction is conservative, and that is not an accident to be corrected quietly: rounding
a hurdle *up* biases against investment, which is a defensible policy so long as it is a stated
policy rather than an artefact of preferring round numbers. Second, the magnitude licenses the
simplification. Sixty-five thousand on a sixteen-million NPV is 0.40 % — immaterial to this
decision, and a domain that reported the appraisal to four decimal places while the forecast carries
a 21 % breakeven band would be practising false precision. **State the rounding, quantify it once,
and then use the round number.** Third — and this is the finding worth carrying — the two Domain 9
rates are **1.40 basis points apart from the board rate in opposite directions**, and the gap
between the proposed structure and the coverage-binding one is only 1.41 basis points of WACC. The
appraisal is therefore insensitive to the financing structure, while the *sponsors'* return is not:
across the same gearing range Domain 9 shows equity IRR moving 184.61 basis points. That asymmetry
is the professional point. **Gearing barely changes what the project is worth; it changes who gets
it.** A director who reads only the project NPV will see nothing happen as the structure is
negotiated, and will therefore not be in the room where the value moves.

**The category error this reconciliation protects against.** Because 7.9860 % is a *project* rate,
it discounts *project* free cash flow (KA 4.1.1). Apply it to equity cash flow — after debt service
— and it overstates equity value grossly. Run the mistake in reverse, discounting the project's
8,900,000 at the 15.42 % cost of equity, and `AF(0.1542, 15) = 5.730514` gives an NPV of
**−8,998,423**: a viable project rejected by **25,177,784** of pure category error. Neither number is
a rounding issue; both are the same failure to keep the numerator and the denominator in the same
world. The reviewer's test is one line long: *whose cash flow is this, and whose rate is that?*

### 4.A.4 Staged investment, and what the option to abandon is actually worth

KA 4.3.3 says static NPV cannot see option value. That is true and, left there, useless — it reads
as licence to approve anything by asserting an option. The arithmetic below prices one, and the
result is not the one most sponsors expect.

**The situation.** Kestrel's 8,900,000 forecast is an expectation over two states of the world: a
strong offtake case at **10,400,000** with probability **0.6**, and a weak case at **6,650,000**
with probability **0.4**. (Check the mean: `0.6 × 10,400,000 + 0.4 × 6,650,000 = 8,900,000` — the
master thread's figure, exactly.) A one-year pilot would resolve which state obtains before the
60,000,000 is committed.

**Build now.** NPV = +16,179,360, as computed throughout. Because NPV is linear in the annual
inflow, this is *identical* to the probability-weighted NPV of the two states:
`0.6 × 29,018,578 + 0.4 × (−3,079,467) = 16,179,360`. The strong state is worth +29,018,578; the
weak state **destroys 3,079,467**.

**Stage instead.** Pay for the pilot, wait a year, learn, then build only in the strong state.
Expected value at t = 1 is `0.6 × 29,018,578 = 17,411,147`; discounted one year at 8 %, that is
**16,121,432** at t = 0, *before* the pilot's cost.

**The result that matters.** Even with a **free** pilot, staging is worth **57,928 less** than
building now. The option to abandon is genuinely valuable — it avoids `0.4 × 3,079,467 =
1,231,787` of expected loss — but a year's delay costs slightly more than that, because deferring
a 16.18-million NPV by a year at 8 % forgoes about 1.20 million. Net of a pilot cost of any size,
staging loses. **The breakeven pilot cost here is negative**, which is the arithmetic's way of
saying the option is not worth having on these numbers.

**What changes the answer — and it is not the mean.** Hold the expectation at exactly 8,900,000 and
widen the spread: a 50/50 split between **12,000,000** and **5,800,000**. Build-now NPV is
unchanged at +16,179,360, because the mean is unchanged. But the weak state now destroys
**10,355,024**, so the abandonment option avoids `0.5 × 10,355,024 = 5,177,512` of expected loss,
and staging is worth `0.5 × 42,713,744 / 1.08 =` **19,774,882** — a gain over building now of
**3,595,521**, which is the most a rational sponsor would pay for the pilot.

> **Fig 4.4.1 — What flexibility is worth: the same mean, two spreads.** Paired panels, each
> showing the two states of the world above the two strategies. Left, the narrow spread: state
> NPVs +29,018,578 and −3,079,467; build-now +16,179,360 against staging with a free pilot
> +16,121,432, so **staging loses 57,928** — the abandonment option's avoided loss of 1,231,787
> failing to cover the delay cost. Right, the same expected inflow on a wide spread: state NPVs
> +42,713,744 and −10,355,024, build-now unchanged at +16,179,360, staging +19,774,882, so
> **staging gains 3,595,521** as the avoided loss rises to 5,177,512. Source: PCI original. Alt
> text: two side-by-side panels with identical build-now values, showing that the value of
> waiting depends on the spread of outcomes rather than on their average.

**The three rules this arithmetic supports.** *One:* the value of waiting is driven by the **spread**
of outcomes, not their mean — an identical expected case can make staging worthless or worth 3.6
million. *Two:* delay is never free, and its cost is roughly `r × NPV` per year of deferral, so
staging pays only where the downside it avoids is larger than the value it postpones. *Three:* the
option must be real. A pilot that cannot actually stop the project — because the land is bought, the
turbines are ordered, or the political commitment is made — avoids nothing, and its value is zero
however elaborately it is modelled. This is where "option value" most often becomes the
rationalisation that 4.3.3 warns about: the test is not whether flexibility exists in the model but
whether the organisation would genuinely walk away, and who has the authority to make it do so
(Domain 5's stage gates, and the governance question behind them).

### 4.A.5 The reviewer's appraisal eye

The invariants: NPV at 0 % equals the raw flow sum; NPV at the IRR equals zero (substitute it
back — the cheapest IRR audit that exists); MIRR lies between the reinvestment rate and the
IRR; PI > 1 ⇔ NPV > 0 at the same rate; EAV × `AF(r, n)` reproduces the NPV; incremental IRR
above the hurdle ⇔ the bigger project's NPV is higher. Any violated line is a defect
somewhere — the appraisal analogue of Domain 3's factor-table checks, and wired into this
programme's golden-answer harness.

The fuller list, in the order a reviewer should run it:

- **No financing flow in the numerator** when a project rate is in the denominator, and no project
  rate on an equity stream (4.A.3 prices the error at 25,177,784 for Kestrel).
- **Sunk costs absent**; the decision flows are the avoidable ones only.
- **NPV at 0 % equals the arithmetic sum** of the flows. One subtraction, and it catches a
  surprising share of sign and offset errors.
- **NPV at the retained (unrounded) IRR is zero.** Against a two-decimal published rate expect a
  small residual — for Kestrel +6,751 at 12.19 % — and know it for a rounding artefact, not a defect.
- **PI at the IRR equals 1.000000**, which audits the IRR a second way without re-solving it.
- **Capital recovery plus EAV reconstructs the annual inflow**: `I₀/AF + NPV/AF = A`
  (7,009,773 + 1,890,227 = 8,900,000).
- **EAC × `AF(r, H)` reproduces the replacement-chain PV** over any horizon `H` (2,740,168 ×
  8.559479 = 23,454,406).
- **Both NPVs are equal at a reported crossover rate** — exactly, since one factor value determines
  both (2,500,000 each for P and Q at 10.424845 %).
- **Sign changes counted**; more than one and no single IRR may be quoted.
- **One discounting convention throughout**, stated; a mid-period model reports exactly
  `(1 + r)^0.5` more present value than its year-end self (3.92 % at 8 %) — a different ratio means
  the convention was applied to some lines only.
- **The rate reconciles to a structure**, with the gap to the derived WACC quantified rather than
  assumed away (1.40 bp and 65,164 here).
- **Rationing packs disclose the unspent budget and the runner-up set.** Neither is optional: the
  greedy heuristic's 820,000 shortfall is invisible without them.
- **Every probability-weighted appraisal reconciles to its own expected case**: the weighted NPV of
  the states equals the NPV of the mean forecast, because NPV is linear in the flows. A discrepancy
  means the states were built inconsistently.

---

## Industry variations — Domain 4

- **Regulated utilities and availability PPPs.** Contracted revenue narrows the forecast
  range, so appraisal battles concentrate on the *rate* (and the regulator's allowed return);
  NPV margins are thin and Fisher errors (4.A.2) are decision-changing.
- **Merchant power and commodities.** The forecast is the battlefield: point NPVs are close
  to meaningless without the scenario set (Domain 7's stress tests), and payback recovers
  status as a survival measure — how long must prices hold?
- **Oil, gas and mining.** Decommissioning liabilities put the sign-change pathology (4.1.2)
  on almost every appraisal: MIRR and NPV are standing practice, "the IRR" is treated with
  suspicion by convention.
- **Technology and corporate transformation.** Option value (4.3.3) dominates: staged
  investments are the norm, and static NPV is a floor, not an answer. Rationing is annual and
  lumpy — the optimisation caveat of 4.3.2 is the daily reality.
- **Public-sector appraisal.** Where a government publishes appraisal guidance, the discount rate is
  a matter of policy rather than of estimation — typically a centrally set social time preference or
  social discount rate — distributional effects sit beside NPV, and the method is prescribed rather
  than chosen. The rate, its basis and the required sensitivities differ by jurisdiction and are
  revised over time, so the applicable guidance is read at the date of the appraisal rather than
  assumed; what generalises is only the shape, that the numbers travel with a governance file, which
  is the direction this domain pushes every sector.
- **Real estate and social infrastructure.** Terminal value dominates: a large share of NPV can sit
  in a single residual figure at the horizon, so the appraisal's centre of gravity moves from the
  cash-flow years to the exit assumption, and the sensitivity that matters is on that one line.
  Payback is close to meaningless where the return is a capital event rather than a stream.
- **Renewables with contracted offtake, then merchant exposure.** The profile splits: contracted
  cash for the tariff term, market cash afterwards. Appraising the two segments at one rate is the
  standing error, since their risk is not the same — the disciplined treatment discounts each on its
  own basis and discloses the split, which is a Domain 9 conversation this domain must set up
  correctly rather than average away.

**A cross-sector caution on tax.** Every figure in this domain is pre-tax at the project level, and
where tax appears in an appraisal its treatment is jurisdiction-specific — the availability and
timing of capital allowances or depreciation deductions, the deductibility of interest and any
limitation on it, loss carry-forward rules, and withholding on distributions all vary by country and
change over time. This domain therefore states the arithmetic and refuses to state the treatment:
what belongs in the appraisal is the after-tax operating cash flow **computed on written advice for
the specific jurisdiction and structure**, with the advice referenced in the assumption register.
Where a comparison spans jurisdictions, the correct disclosure is one set of flows per tax regime,
not a blended rate that describes none of them. Presenting one country's treatment as though it were
general is the most common way an otherwise sound appraisal becomes indefensible.

---

## Case study — Domain 4: the intake decision, taken properly (water / desalination)

**Situation.** Kestrel's board must (1) confirm the plant investment, (2) choose between
intake designs P and Q, and (3) allocate the sponsor group's residual USD 20m development
budget across four unrelated feeder projects (W–Z of KA 4.3.2). One meeting, three different
appraisal questions.

**The decisions.** *Build:* NPV +16.18m at 8 %, IRR 12.19 %, MIRR 9.73 %, discounted payback
10.07 years — build, with the exposure noted and the two dominating assumptions (tariff
escalation, availability) sensitivity-tested per KA 4.3.3. *Intake:* Q over P — +970,840 more
NPV, incremental IRR 10.42 % over the extra 15m; the 28.65 % on P is recorded as the intensity
it is, not the ranking it isn't. *Budget:* W + X for +5.40m, with the un-funded 1.17-PI
project Z referred to Domain 9's financing question rather than silently killed.

**What the domain teaches here.** Three questions, three measures, one hierarchy: NPV decided
all three calls; IRR, MIRR, PI and payback each explained a facet the board needed narrated.
The minute records rates, worlds (nominal), sensitivity owners and the option-value judgments
— the appraisal file *is* the governance file.

**How the meeting actually went, and the two things that nearly went wrong.** The build paper
arrived showing a level 8,900,000 profile. The finance director's first question was the one KA
4.1.1b exists to prompt — *on what profile?* — and the restatement on the engineers' ramp took the
NPV to **+13,699,840**, still comfortably positive but 2,479,520 lower, and moved discounted payback
from 10.07 to **10.91 years**, past the nine-year screening rule the board had been applying without
having voted on it. That is the first near-miss: on the level profile the rule was never triggered
and would never have been examined. It was examined here, and the board resolved to treat exposure
duration as a disclosed characteristic rather than a veto — a decision minuted with the 16.18-million
value at stake attached to it, exactly as MCQ 4.2-H frames the choice.

The second near-miss came from the intake comparison. The paper's recommendation was P, on a 28.65 %
IRR against Q's 15.24 %, and it took the incremental test to reverse it: 15,000,000 more capital
earning **10.4248 %** against an 8 % cost, worth **970,840** more in value. What made the reversal
stick rather than becoming a contest of percentages was the crossover figure and its distance from
the hurdle — **2.42 points**. The director could say, in one sentence, that the rate would have to be
wrong by more than two and a half points to change the answer. A ranking defended that way survives
the meeting; a ranking defended by "NPV is the better measure" does not.

**The rate was the last item, not the first.** The board's 8.0 % was carried into the paper as a
standing assumption. The reconciliation of KA 4.A.3 was tabled alongside it: Domain 9's derived WACC
at the proposed structure is **7.9860 %**, so the hurdle is 1.40 basis points conservative and
understates value by **65,164** — immaterial, and now on the record as immaterial rather than
unexamined. The same table carried the finding that mattered more: the WACC at the coverage-binding
gearing is 8.0001 %, so **the project's value is almost indifferent to the financing structure while
the sponsors' return moves by 184.61 basis points across it.** The board's conclusion was procedural
and correct — the gearing negotiation is a shareholder-return question, and it belongs on the agenda
as one, not buried in an appraisal input.

## Case study B — Domain 4: the fund that bought percentages (infrastructure fund)

**Situation.** An infrastructure fund with a 9 % cost of capital repeatedly preferred
short-tenor, high-IRR deals — the P-shape — over long-tenor concessions at 13–15 % IRR. Five
years on, its realised portfolio return trailed its own reported deal IRRs by nearly four
points.

**What happened.** The gap was the reinvestment fiction industrialised: each 25–30 % IRR
assumed its distributions would earn the same, but the fund redeployed at market rates —
when it could redeploy at all. MIRR at honest reinvestment rates (the 4.1.3 arithmetic) had
flagged the true economics of every deal; it was computed, and filed. The long concessions it
passed over went to competitors who now hold them, on a comparison the fund never actually made.

**The four points, computed.** The wedge is not an impression; it falls out of the fund's own
records in three lines. The representative deal committed **10,000,000** and returned
**20,000,000** three years later — a doubling, so its IRR is `2^(1/3) − 1 =` **25.9921 %**, and that
rate is achieved *only* if the money is recommitted on the day it comes back. It was not. The
fund's average interval between realisation and recommitment ran about **six months**, held in cash
at **2.0 %**, which makes each recycling cycle 3.5 years long and multiplies capital by
`2 × 1.02^0.5 = 2.019901` per cycle. The realised compound return is therefore
`2.019901^(1/3.5) − 1 =` **22.2467 %** — a wedge of **374.54 basis points**, which is the "nearly
four points" the investors eventually measured and the fund could not explain. In the two years
when origination slowed and the interval ran nearer twelve months, the cycle multiple was
`2 × 1.02 = 2.040000` over four years and the realised rate fell to **19.5109 %**, a wedge of
**648.12 basis points**. Nothing was misstated at any point: every deal returned exactly what was
reported, and the entire wedge lived in the interval *between* deals, which appears in no deal's
IRR because no deal's IRR contains it.

**The comparison the fund thought it was making.** A representative declined concession committed
the same **10,000,000** against **1,800,000 a year for twelve years** — `AF(r, 12) = 5.555556`, so
an IRR of **14.4284 %**, and an NPV at the fund's 9 % cost of capital of **+2,889,305**. Set the
three figures side by side and the fund's strategic argument changes shape:

| | Believed | Actual at a six-month gap | Actual at a twelve-month gap |
|---|---|---|---|
| Short-deal book | **25.9921 %** | **22.2467 %** | **19.5109 %** |
| Declined concession | 14.4284 % | 14.4284 % | 14.4284 % |
| **Advantage claimed** | **1,156.38 bp** | **781.84 bp** | **508.26 bp** |

The honest conclusion is narrower than the one a reader expects, and stating it that way is the
point. **The short-deal strategy was still the better one** — it compounded faster than the
concession at every redeployment gap the fund actually ran, and it bought liquidity and optionality
besides. What the fund got wrong was not the choice but the **margin**, and therefore its own
confidence: it declined 14.4284 % believing it held an eleven-and-a-half-point advantage when it
held under eight, and in the slow years barely five. And there is a threshold at which the argument
inverts altogether. Solving `[2 × 1.02^g]^(1/(3 + g)) − 1 = 14.4284 %` gives a **breakeven
redeployment gap of 2.5119 years**: at that average interval the recycled short-deal book earns
precisely what the concession it refused would have earned. The fund never approached that gap —
but it had no instrument that would have told it if it had, because the gap was measured by nobody
and appeared in no report. That is the difference between being right and knowing you are right,
and only one of the two survives a bad year.

**The variable this makes visible.** Between a six-month and a twelve-month gap the fund's realised
return moves **273.58 basis points**, or roughly **46 basis points of fund return per month** of
average idle capital. No deal-level metric contains that number, and it is larger than any
plausible improvement in deal selection. **Origination capacity, not screening skill, was the
fund's highest-return investment**, and the arithmetic that says so is three lines long and was
available from the first realisation.

**What the domain teaches here.** A portfolio of intensities is not a portfolio of value.
IRR's flattery compounds at fund level because short deals *recycle* the fiction; NPV and
MIRR discipline is not academic tidiness — over a fund's life it is the difference between
the return promised and the return delivered.

**Why it was rational for everyone involved.** The uncomfortable part of this case is that no one
behaved dishonestly. The investment committee's mandate was expressed as a target IRR, its incentive
compensation was measured against realised deal IRRs, and its quarterly reporting to investors ranked
deals by the same measure. Each of those is a defensible instrument in isolation; together they made
the reinvestment fiction the fund's operating assumption, and MIRR — computed, filed, never tabled —
was the only document in the building that disagreed. The lesson is therefore not about a measure but
about a **measurement system**: where the mandate, the incentive and the report all speak in
percentages, no amount of NPV literacy in the analyst pool will change the decisions, because the
analysts are not the ones deciding. Changing the outcome required changing the mandate.

The case is an original illustration and no view is expressed on any real fund. Note also that how
fund performance may be measured, presented and marketed to investors — and how carried interest is
calculated against it — is regulated in most jurisdictions and governed by the fund's own
constitutional documents. The appraisal point here is arithmetical; changing a mandate, an incentive
formula or an investor report is a matter for the fund's counsel and its regulator, not for a
measurement argument alone.

**What a corrected pack looks like.** The remedy the fund eventually adopted is worth stating because
it is cheap and general. Every deal paper now carries four figures beside the IRR: **NPV at the fund's
cost of capital in money**, so scale is visible; **MIRR at the treasury's actual reinvestment rate**,
so the fiction is priced; the **tenor**, so nothing is compared across horizons without EAV; and the
**redeployment assumption for distributions**, named and owned. The single most effective of the four
was the third. Once tenor sat in the same row as the percentage, the committee stopped comparing a
three-year 28 % with a fifteen-year 13 % as though the comparison meant something — which is the
error 4.1.2's pitfall names, and the one that had quietly cost the fund four points of realised
return.

---

## Executive perspective — Domain 4

What a project finance director cannot delegate in this domain:

- **The hurdle.** As in Domain 3: whoever sets `r` sets the answer. The director owns the
  rate, its world (nominal or real), and the standing sensitivity band around it.
- **The measure hierarchy.** NPV decides; rates and ratios explain. The director enforces
  this in every paper — and personally reads the profile, not the point.
- **The exclusivity discipline.** Incremental thinking (Q−P) is the director's question in
  the room: "what does the extra money earn?" beats "which percentage is bigger?" every time.
- **The honesty of rejections.** Under rationing, what was *not* funded and why is board
  information; a rejected 1.17-PI project is a financing gap wearing a screening verdict.
- **The unpriced.** Option value, strategic position, forecast asymmetry — the director's
  overrides are legitimate exactly when they are explicit, minuted and owned (4.3.3).
- **The profile.** Every screening NPV built on a level annuity is an upper bound. The director asks
  *on what profile?* before asking anything else, because the ramp is worth 2.48 million on this
  project and is invisible in the formula that produced the headline.
- **The category question.** Whose cash flow, and whose rate? One sentence, asked of every paper.
  It is the cheapest control in the domain and the failure it prevents is the largest — 25.2 million
  here.
- **The measurement system, not the measure.** Case study B's fund lost four points of realised
  return with competent analysts, because its mandate, its incentives and its investor reporting all
  spoke in percentages. A director who fixes the appraisal template and leaves the mandate alone has
  fixed nothing.
- **Screening rules as decisions.** A payback or PI threshold that vetoes a positive-NPV project is
  a policy choice about exposure, and the director's job is to put it to the board with the foregone
  value attached — not to let a threshold decide silently and call the result prudence.
- **Reconciliation, not assumption.** The hurdle is reconciled to the structure that will be signed
  (Toolkit 4.T.4) before the paper circulates. Where the gap is immaterial, saying so on the record
  is itself the deliverable; an unexamined rate is a conclusion nobody owns.

## Calculation exercises — Domain 4

**Exercise 4.1** `I₀` = 12,000,000; net flows 3,000,000 · 4,000,000 · 5,000,000 · 5,000,000
in years 1–4; `r` = 10 %. Compute the NPV.
*Solution.* PV = `3/1.10 + 4/1.10² + 5/1.10³ + 5/1.10⁴` (millions) = 2,727,273 + 3,305,785 +
3,756,574 + 3,415,067 = 13,204,699; **NPV = +1,204,699**. Common error: discounting year 1 at
`t` = 0 (no discount) shifts every factor one period and overstates NPV by ≈ 1.19m.

**Exercise 4.2** `I₀` = 9,000,000; level inflow 1,500,000 for 10 years. Find the IRR.
*Solution.* `AF(r, 10) = 9/1.5 = 6.0`; solving gives **IRR = 10.56 %** (AF(10 %) = 6.145,
AF(11 %) = 5.889 bracket it). Common error: reading the payback reciprocal (1.5/9 = 16.7 %)
as the IRR — that shortcut holds only for perpetuities.

**Exercise 4.3** `I₀` = 10,000,000; inflows 2,600,000 for 6 years; reinvestment and finance
rate 7 %. Compute MIRR.
*Solution.* `FVAF(0.07, 6) = 7.153291`; TV = 18,598,556;
`MIRR = (1.859856)^(1/6) − 1 =` **10.90 %**. Common error: compounding the inflows for 6
periods each (rather than to a common terminal date) inflates TV and the rate.

**Exercise 4.4** Machine C1: 3,000,000 capex, 4-year life, 250,000/yr opex. Machine C2:
4,200,000, 6-year life, 150,000/yr. Same duty; `r` = 9 %. Choose.
*Solution.* C1: PV = 3,000,000 + 250,000 × 3.239720 = 3,809,930; EAC = **1,176,006**. C2:
PV = 4,200,000 + 150,000 × 4.485919 = 4,872,888; EAC = **1,086,263**. **Choose C2** — cheaper
by 89,743 per year. Common error: comparing PVs across unequal lives (which picks C1).

**Exercise 4.5** Budget 25,000,000. Projects (I₀; NPV): A (10; 2.60) · B (15; 3.30) ·
C (8; 1.84) · D (7; 1.40). Fund the best set.
*Solution.* PIs: A 1.260 · C 1.230 · B 1.220 · D 1.200. **Greedy PI packing fails here**:
A + C + D fits (25) for +5.84m, but **A + B (25 exactly) yields +5.90m**. With lumpy projects
the rule is: use PI to shortlist, then *enumerate feasible sets* (or optimise). This is the
4.3.2 caveat as arithmetic.

**Exercise 4.6** `I₀` = 18,000,000 today; nothing in years 1–2 while the asset is built; then
3,600,000 per year for years 3–12; `r` = 9 %. Compute the NPV.
*Solution.* Deferred annuity: `AF(0.09, 12) − AF(0.09, 2) = 7.160725 − 1.759111 = 5.401614`;
PV = `3,600,000 × 5.401614 = 19,445,811`; **NPV = +1,445,811**. Common error: using
`AF(0.09, 10) = 6.417658` for "ten years of cash" without deferring it — that treats the stream
as starting in year 1 and gives +5,103,568, overstating NPV by 3,657,757. The count of payments is
right and their *position* is wrong, which is why the deferred form subtracts the factor for the
years you do not receive rather than counting the years you do.

**Exercise 4.7** A model reports a present value of 41,250,000 on year-end discounting at 7 %.
Restate it on the mid-period convention.
*Solution.* Multiply by `(1.07)^0.5 = 1.0344080`: **PV = 42,669,332**, an uplift of **1,419,332**
(3.44 %). Common error: multiplying by `1 + r/2 = 1.035`, which is a linear approximation to a
compound adjustment; it happens to be close here (42,693,750, out by 24,418) and drifts as the rate
rises. The exact multiplier is the square root of the annual factor, at every rate.

**Exercise 4.8** `I₀` = 25,000,000 at t = 0 and a further 4,000,000 outflow at the end of year 5;
inflows 5,200,000 per year for 12 years. Finance rate 6.5 %, reinvestment rate 3.5 %. Compute MIRR.
*Solution.* `PV(outflows) = 25,000,000 + 4,000,000 × 0.729881 = 27,919,523`;
`FVAF(0.035, 12) = 14.601962`, so `TV = 5,200,000 × 14.601962 = 75,930,201`;
`MIRR = (75,930,201 / 27,919,523)^(1/12) − 1 = 2.719609^(1/12) − 1 =` **8.6948 %**. Common error:
using one rate for both roles — at 6.5 % throughout the answer becomes 10.2790 %, overstating by
158 basis points, and the overstatement is exactly the reinvestment assumption MIRR was chosen to
remove.

**Exercise 4.9** `I₀` = 32,000,000; level inflow 4,400,000 for 20 years; `r` = 7.5 %. Compute the
NPV, the EAV, the capital-recovery annuity and the revenue headroom, and show they reconcile.
*Solution.* `AF(0.075, 20) = 10.194491`; NPV = `4,400,000 × 10.194491 − 32,000,000 =`
**+12,855,762**; `EAV = 12,855,762 / 10.194491 =` **1,261,050**; capital recovery
`= 32,000,000 / 10.194491 =` **3,138,950**. Reconciliation: `3,138,950 + 1,261,050 = 4,400,000`,
the inflow itself — so **28.66 % of each year's cash is value** and the breakeven inflow is
3,138,950, i.e. **28.66 % below base**. Common error: computing headroom off the NPV
(12,855,762 / 60,000,000-style ratios) rather than off the annuity; the two answer different
questions and only the annuity form is the revenue breakeven.

**Exercise 4.10** Two exclusive designs, both 6-year lives, `r` = 8 %: **S** (`I₀` 7,000,000,
inflow 2,100,000) and **T** (`I₀` 16,000,000, inflow 4,300,000). Rank them, find the crossover, and
verify it.
*Solution.* `AF(0.08, 6) = 4.622880`. S: NPV **+2,708,047**, IRR **19.9054 %**. T: NPV
**+3,878,383**, IRR **15.6321 %**. IRR prefers S; NPV prefers T by 1,170,335. Incremental project
T−S is `(−9,000,000; +2,200,000 × 6)`, so `AF = 9,000,000/2,200,000 = 4.090909` and the
**crossover is 12.1767 %** — above the 8 % hurdle, so T stands. Verification: at 12.1767 % both
NPVs are **1,590,909**, identical. Common error: stopping at the NPV ranking without the crossover.
The hurdle here is 4.18 points below indifference, which is the sentence that defends the decision;
without it the recommendation is an assertion about which measure to prefer.

**Exercise 4.11** `I₀` = 30,000,000; `r` = 10 %; 15-year inflows either 5,400,000 (probability
0.55) or 3,200,000 (0.45). A one-year study would resolve which. What is the most the study can be
worth?
*Solution.* `AF(0.10, 15) = 7.606080`. Expected inflow `= 0.55 × 5,400,000 + 0.45 × 3,200,000 =
4,410,000`, so build-now NPV = **+3,542,811**. State NPVs: good **+11,072,829**, bad
**−5,660,546**; the weighted average is `0.55 × 11,072,829 + 0.45 × (−5,660,546) = 3,542,811`,
confirming linearity. Staging builds only in the good state:
`0.55 × 11,072,829 / 1.10 =` **5,536,415**, so the **maximum study cost is 1,993,604**. Common
error: valuing the option as the avoided loss alone (`0.45 × 5,660,546 = 2,547,246`) and paying up
to that — it ignores the delay cost of postponing a positive-NPV project by a year, which is the
553,641 difference between the two figures.

## Practitioner's toolkit — Domain 4

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 4.T.1 — The appraisal one-pager (per decision)

Decision question (accept/reject · exclusive choice · rationing) · cash-flow source and world
(nominal/real, per 3.T.1) · rate and its owner · **NPV at the owned rate** · profile sketch or
IRR/MIRR · discounted payback · PI (if rationing) · EAV (if unequal lives) · two-way
sensitivity on the dominating assumptions · unpriced factors, stated · recommendation and the
named accountable approver.

### Toolkit 4.T.2 — IRR pathology checklist

- [ ] Sign changes counted; more than one → MIRR + NPV only, "the IRR" banned from the paper.
- [ ] Scale stated beside every percentage (I₀ and NPV in money).
- [ ] Tenors aligned before any cross-project rate comparison (else EAV).
- [ ] Reinvestment assumption named; MIRR computed when IRR > hurdle + 5 points.
- [ ] IRR substituted back: NPV(IRR) recomputes to zero.
- [ ] Incremental IRR computed for every exclusive pair; agrees with NPV ranking.

### Toolkit 4.T.3 — Rationing worksheet

Columns: project · `I₀` · NPV · PI · strategic notes/dependencies. Steps: rank by PI →
shortlist to ~2× budget → enumerate feasible sets (lumpy) → record funded set, rejected set
with reasons, and the financing referral for high-PI rejects (Domain 9). Two mandatory disclosure
lines at the foot of the table: **budget unspent** and **runner-up set with its NPV** — without
them a greedy result is indistinguishable from an optimum (WE 4.3.2's 820,000).

### Toolkit 4.T.4 — Hurdle-rate reconciliation sheet

One page, completed before the appraisal is circulated rather than after it is challenged.

| Line | Entry | Kestrel worked |
|---|---|---|
| Rate used in the appraisal | as stated in the paper | 8.0000 % |
| Derived `WACC` at the proposed structure | from the funding model (Domain 9) | 7.9860 % |
| Derived `WACC` at the coverage-binding structure | the most debt the cover test carries | 8.0001 % |
| Gap, in basis points | used − derived | +1.40 bp |
| Gap, in NPV | recompute; do not estimate | 65,164 (0.40 % of NPV) |
| Direction | conservative or optimistic, stated | conservative |
| Materiality conclusion | with the threshold that decided it | immaterial |
| Whose cash flow is discounted | project free cash flow, or equity after debt service | project |
| Matching rate for that stream | `WACC` for project flow, `k_e` for equity flow | `WACC` |
| Discounting convention | year-end or mid-period, applied to every line | year-end |
| Rate owner | named individual, not a committee | finance director |

The ninth and tenth lines are the ones that catch career-shortening errors: a project rate on an
equity stream is worth 25,177,784 of misstatement on this project alone (KA 4.A.3), and a convention
applied inconsistently is worth 3.9 % of present value at 8 %.

## Exam preparation — Domain 4

**The calculation traps.** Forgetting to deduct `I₀` (MCQ 4.1-A distractor D) · discounting
from `t` = 0 (Exercise 4.1) · payback reciprocal as IRR (Exercise 4.2) · ranking exclusives
by IRR (4.3.1) · greedy PI with lumpy budgets (Exercise 4.5) · comparing PVs across unequal
lives (Exercise 4.4) · quoting one IRR for sign-changing flows (4.1.2) · MIRR terminal value
mis-compounded (Exercise 4.3) · nominal/real mixing imported from Domain 3 · counting the years
you *receive* rather than deferring by the years you do not (Exercise 4.6) · the linear
`1 + r/2` in place of `(1 + r)^0.5` for mid-period (Exercise 4.7) · one rate doing both MIRR jobs
(Exercise 4.8) · headroom computed off NPV instead of the annuity (Exercise 4.9) · valuing an
abandonment option at the avoided loss and forgetting the delay cost (Exercise 4.11).

**The judgement traps** — the ones that cost more than arithmetic. Reading MIRR against the hurdle
as though it were an IRR (MCQ 4.1-G) · comparing NPVs modelled on different discounting conventions
(MCQ 4.1-H) · mixing gross and net PI in one table (MCQ 4.2-E) · treating the exact agreement of two
methods as evidence that their shared assumption holds (MCQ 4.2-G) · letting a screening threshold
veto value without putting the threshold to the board (MCQ 4.2-H) · presenting a greedy packing as
an optimum (MCQ 4.3-G) · spending breakeven headroom in a negotiation as though it were margin
rather than the buffer against everything else (MCQ 4.3-H) · discounting an equity stream at a
project rate, or the reverse (4.A.3) · asserting an option that the organisation has no authority to
exercise (4.A.4).

**Fluency drills.** Each should take under a minute with a factor table: `AF(0.08, 15) = 8.559479`
and therefore breakeven inflow on 60,000,000 is 7,009,773 · a mid-period model reports 3.92 % more
PV than its year-end self at 8 % · PI at the IRR is 1.000000 · `I₀/AF + NPV/AF = A` · a crossover
falls out of one factor value, so both NPVs there are equal · a two-sign-change project has no
quotable IRR · deferring a positive NPV by a year costs about `r × NPV`.

**Reflection questions.**
1. Your board paper shows one number: "IRR 22 %". List the five questions this domain obliges
   you to ask before anyone votes. *(Scale? Sign changes? Tenor? Reinvestment? NPV at our
   rate?)*
2. When is a negative-NPV decision defensible — and what must the record contain? *(4.3.3:
   explicit option/strategic value, owned and minuted.)*
3. Which invariant of 4.A.5 would have caught the last appraisal error you saw? *(Most often:
   NPV(IRR) ≠ 0 on substitution, or EAV × AF ≠ NPV.)*
4. Your organisation's appraisals all use a round hurdle rate nobody has reconciled to a funding
   structure. Draft the three lines of Toolkit 4.T.4 you would need to complete before the next
   paper, and say what you would do if the gap came back at fifty basis points rather than 1.40.
   *(Recompute at the derived rate, disclose direction and magnitude, and at fifty basis points —
   worth roughly 2.3 million here — stop using the round number.)*
5. A sponsor proposes a 3,000,000 pilot to "de-risk" a project whose land is already purchased and
   whose political commitment is public. What is the pilot worth, and why? *(Nothing as an option:
   the organisation cannot walk away, so no downside is avoided. It may still be worth paying for
   as engineering de-risking — but that is a cost of the project, not option value, and must be
   presented as one.)*
6. Two teams' appraisals of competing plants differ by 3.9 % of present value on identical rates.
   What do you check first, and what does it tell you about comparing the two recommendations?
   *(The discounting convention — `(1.08)^0.5 − 1`; until both are restated on one convention the
   comparison ranks the modellers rather than the plants.)*

## Domain 4 summary

Appraisal turns Domain 3's machinery into decisions, under one hierarchy: **NPV decides;
everything else explains.** NPV earns primacy by additivity, scale-awareness and the honest
reinvestment assumption; IRR narrates a margin of safety but carries three standing
pathologies — multiple roots, scale-blindness, the reinvestment fiction — of which MIRR
repairs only the last; payback and its discounted form measure exposure, never value; PI
ranks value per scarce dollar under rationing (with the lumpy-budget caveat proven in
Exercise 4.5); EAV makes unequal lives commensurable. Mutually exclusive choices resolve by
NPV, narrated by incremental IRR and the crossover geometry; rationing resolves by
disciplined packing plus the financing question for what was turned away; and the limits of
the numbers — options, strategy, forecast asymmetry — are stated in the open, as owned
judgments.

Three findings from this domain's arithmetic outlast the formulae. **Timing is priced even when
totals are not:** the ramp profile of 4.1.1b moves not one dollar of forecast revenue and costs
2,479,520 of value, which is why a level-annuity screening NPV is an upper bound and should be
labelled as one. **Conventions and categories move more money than measures do:** the mid-period
choice is worth 2,988,553 on this project and appears in no formula, and discounting project cash
flow at the 15.42 % cost of equity instead of the 7.9860 % project rate would have destroyed a
viable investment by 25,177,784 — neither is a modelling subtlety, and both are caught by one
sentence asked of every paper. And **the hurdle must be reconciled, not assumed:** the board's round
8 % is 1.40 basis points and 65,164 away from the structure's derived cost of capital, which is
immaterial here and is now on the record as immaterial rather than unexamined — while the same
reconciliation surfaces the finding that matters more, that gearing barely changes what the project
is worth and moves the sponsors' return by 184.61 basis points.

The reviewer's invariants of 4.A.5 make each of these testable rather than merely asserted, and every
printed result above is independently recomputed in this programme's golden-answer harness. The
Kestrel thread continues into Domain 5 (is the project *bankable*, not merely valuable?), Domain 6
(the model that industrialises every calculation this domain taught) and Domain 9, which derives the
rate this domain has now stopped taking on trust.
