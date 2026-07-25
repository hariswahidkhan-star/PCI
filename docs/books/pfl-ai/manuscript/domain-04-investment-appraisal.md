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

### 4.1.2 IRR and its pathologies

**Definition.** The internal rate of return is the discount rate at which NPV is zero — the
break-even price of capital:

```
NPV(IRR) = 0
```

For Kestrel: solve `8,900,000 × AF(r, 15) = 60,000,000` → `AF = 6.741573` → **IRR = 12.19 %**.
Read against an 8 % hurdle, the project clears with 4.19 points to spare — the same verdict as
NPV, expressed as a margin of safety rather than a value.

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

### Self-check — KA 4.1

1. *State the master appraisal's three verdicts and their meanings.* — NPV +16.18m (value
   created at 8 %); IRR 12.19 % (break-even rate, 4.19-point margin); MIRR 9.73 % (return with
   honest 8 % reinvestment).
2. *Why does a mining-with-restoration cash flow break IRR?* — Two sign changes → up to two
   roots; "the" IRR does not exist.
3. *What single question decides between NPV and IRR when they disagree?* — Which project adds
   more money at the actual cost of capital — NPV's answer, by construction.

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

### Self-check — KA 4.2

1. *Why is discounted payback always later than simple payback?* — Discounting shrinks every
   inflow, so the cumulative line climbs more slowly (equal only if `r` = 0).
2. *State Kestrel's PI and its meaning.* — 1.270: each dollar invested buys 1.27 dollars of
   present value.
3. *What assumption does EAV smuggle in, and when must you drop it?* — Like-for-like
   replacement in perpetuity; drop it when the duty ends or technology shifts, and model the
   real chain.

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

### 4.3.3 The limits of the numbers

Every measure in this domain consumes forecasts and a rate, and both are judgments wearing
decimals. The professional frame: **appraisal quantifies; it does not decide strategy.**
Numbers cannot see option value (the pilot that buys the right to scale), strategic
foreclosure (the market position lost by not building), or the asymmetry of forecast error
(Domain 8's ranges belong beside every point NPV). The board paper this domain endorses shows:
NPV at the owned rate; the sensitivity of that NPV to the two or three assumptions that
dominate it; IRR/MIRR as narrative; exposure via discounted payback; and the explicit
statement of what the numbers cannot price. Anything less is theatre with spreadsheets.

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

### Self-check — KA 4.3

1. *Why does incremental IRR always agree with NPV on a pairwise choice?* — It tests whether
   the *difference* project clears the hurdle, which is exactly what the NPV difference
   measures.
2. *When does PI packing fail as a rationing rule?* — Lumpy projects and multi-period budget
   constraints; then it's an optimisation, not a sort.
3. *Name three things a positive NPV cannot tell the board.* — Exposure duration; sensitivity
   concentration; the value of flexibility and strategic position outside the modelled flows.

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

### 4.A.3 The reviewer's appraisal eye

The invariants: NPV at 0 % equals the raw flow sum; NPV at the IRR equals zero (substitute it
back — the cheapest IRR audit that exists); MIRR lies between the reinvestment rate and the
IRR; PI > 1 ⇔ NPV > 0 at the same rate; EAV × `AF(r, n)` reproduces the NPV; incremental IRR
above the hurdle ⇔ the bigger project's NPV is higher. Any violated line is a defect
somewhere — the appraisal analogue of Domain 3's factor-table checks, and wired into this
programme's golden-answer harness.

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
- **Public-sector appraisal.** The discount rate is policy (a social time preference rate,
  set centrally), distributional effects sit beside NPV, and appraisal guidance is published —
  the numbers travel with a governance file, which is the direction this domain pushes every
  sector.

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

## Case study B — Domain 4: the fund that bought percentages (infrastructure fund)

**Situation.** An infrastructure fund with a 9 % cost of capital repeatedly preferred
short-tenor, high-IRR deals — the P-shape — over long-tenor concessions at 13–15 % IRR. Five
years on, its realised portfolio return trailed its own reported deal IRRs by nearly four
points.

**What happened.** The gap was the reinvestment fiction industrialised: each 25–30 % IRR
assumed its distributions would earn the same, but the fund redeployed at market rates —
when it could redeploy at all. MIRR at honest reinvestment rates (the 4.1.3 arithmetic) had
flagged the true economics of every deal; it was computed, and filed. The long concessions it
passed over — lower IRR, far higher NPV per dollar and decade — went to competitors who now
hold them.

**What the domain teaches here.** A portfolio of intensities is not a portfolio of value.
IRR's flattery compounds at fund level because short deals *recycle* the fiction; NPV and
MIRR discipline is not academic tidiness — over a fund's life it is the difference between
the return promised and the return delivered.

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
with reasons, and the financing referral for high-PI rejects (Domain 9).

## Exam preparation — Domain 4

**The calculation traps.** Forgetting to deduct `I₀` (MCQ 4.1-A distractor D) · discounting
from `t` = 0 (Exercise 4.1) · payback reciprocal as IRR (Exercise 4.2) · ranking exclusives
by IRR (4.3.1) · greedy PI with lumpy budgets (Exercise 4.5) · comparing PVs across unequal
lives (Exercise 4.4) · quoting one IRR for sign-changing flows (4.1.2) · MIRR terminal value
mis-compounded (Exercise 4.3) · nominal/real mixing imported from Domain 3.

**Reflection questions.**
1. Your board paper shows one number: "IRR 22 %". List the five questions this domain obliges
   you to ask before anyone votes. *(Scale? Sign changes? Tenor? Reinvestment? NPV at our
   rate?)*
2. When is a negative-NPV decision defensible — and what must the record contain? *(4.3.3:
   explicit option/strategic value, owned and minuted.)*
3. Which invariant of 4.A.3 would have caught the last appraisal error you saw? *(Most often:
   NPV(IRR) ≠ 0 on substitution, or EAV × AF ≠ NPV.)*

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
judgments. The Kestrel thread continues into Domain 5 (is the project *bankable*, not merely
valuable?) and Domain 6 (the model that industrialises every calculation this domain taught).
