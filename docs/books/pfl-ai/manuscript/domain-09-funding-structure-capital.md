# Domain 9 — Funding Structure and Sources of Capital

## Why this domain exists

Domain 4 discounted Kestrel's cash flows at 8.0 % because the board said 8.0 %. Domain 5 asked
whether the project was bankable, Domain 6 built the model, Domains 7 and 8 filled it with revenue
and cost. None of them asked the question this domain exists to answer: **where does the money
come from, what does each source cost, and who decides the mix?** A hurdle rate inherited rather
than derived is the largest single unexamined assumption a project finance leader can carry.

The domain's central claim is deliberately contrarian. In corporate finance, capital structure
is chosen by minimising the weighted average cost of capital. **In project finance it is not.**
Moving Kestrel from 60 % to 80 % gearing cuts its `WACC` by **20.4 basis points** while lifting
the sponsors' equity return by **184.6 basis points** and destroying **0.3716** of `DSCR` — and
the 1.20× covenant fails outright above 74.34 % gearing. The structure is chosen by the
**coverage constraint**, negotiated between people whose returns move in opposite directions,
and the cost of capital is the *consequence* of that negotiation rather than its objective.
Everything here follows from that inversion: equity and shareholder instruments and what they
cost (KA 9.1); the debt stack from senior to mezzanine to bonds and the blended arithmetic
across it (KA 9.2); the specialist sources (Islamic finance structures, export credit,
development finance) that change tenor and price in ways commercial banks cannot (KA 9.3); and
the public-sector and market instruments that shift the answer again (KA 9.4).

**Learning objectives.** After this domain a candidate can: distinguish the forms of project
equity and shareholder instruments and state what each does to coverage, tax and control; build a
cost of equity from a documented premium build-up and re-lever a beta for a project's own
structure; compute an after-tax cost of debt and a project `WACC`, and say precisely why a project
`WACC` is not the sponsor's corporate `WACC`; blend a senior/mezzanine/equity stack with correct
weights and tax treatment; quantify the leverage trade-off on both faces and identify the gearing
at which each covenant threshold binds; describe subordinated, mezzanine and bond instruments
including negative arbitrage; describe Islamic finance structures in economic terms and compare
them with conventional debt on an all-in basis; compute the all-in cost of an export-credit or
development-finance tranche and separate that cost effect from the debt-capacity effect of tenor;
compute a grant's effect on `WACC`, `DSCR` and equity return and state the treatment choice that
drives it; test sustainability-linked pricing against its own verification cost; compute and
decompose a refinancing gain and price a gain-share obligation; and govern AI-assisted structuring
so that no structure is recommended on an unverified cost of capital.

**The master financing.** Kestrel Water SPC continues from Domains 1–8 and 10. Capex is **USD
60,000,000**, funded 70/30 as **USD 42,000,000** of senior debt at **6.0 % over 12 years** (annual
instalment **USD 5,009,635.23**; year-one interest **2,520,000**, principal **2,489,635**) plus
**USD 18,000,000** of equity. Operating life is **25 years**. Documented `CFADS` is **USD
6,384,000** (6,984,000 before working-capital movements), against `EBITDA` of **7,500,000** and
`EBIT` of **5,100,000**; the resulting ratios are `DSCR` **1.2743** = `LLCR` **1.2743**, `PLCR`
**1.9431** (Domain 10, KA 10.2). Domain 4's appraisal at 8 % gave `NPV` **+16,179,360**, `IRR`
**12.19 %**, `MIRR` **9.73 %**, `PI` **1.270**.

Two Kestrel cash-flow streams appear in this volume and this domain keeps them apart. Domain 4's
appraisal stream (net inflows of **USD 8,900,000 a year for fifteen years**) is the
pre-financing screening forecast against which a **discount rate** is applied. Domain 10's
documented `CFADS` of **6,384,000**, held level over the 25-year life on that domain's stated
illustrative convention, is the stream against which **coverage** is tested and from which
**distributions** are paid. This domain applies costs of capital to the first and computes
coverage and equity returns on the second, never mixing them inside one calculation; equity
returns quoted on the level-`CFADS` basis are labelled as such. Their divergence is not an
inconsistency to be hidden: KA 9.1.4 turns it into the domain's most useful breakeven.

**The tax rate, derived rather than assumed.** Every after-tax cost below depends on it, and the
thread fixes it. `EBITDA` **7,500,000** less `CFADS`-before-working-capital **6,984,000** leaves
cash tax of **516,000**; taxable profit is `EBIT` **5,100,000** less interest **2,520,000** =
**2,580,000**; the implied rate is `516,000 / 2,580,000 =` **20.0 %** exactly, and depreciation
reconciles as `EBITDA − EBIT =` **2,400,000**. Kestrel's year-one interest tax shield is
`2,520,000 × 20 % =` **504,000**. A tax rate is a jurisdictional fact and an interest deduction a
jurisdictional privilege: both come from the project's actual tax position (Domain 2), never from
a modelling convention.

---

## Knowledge Area 9.1 — Equity and shareholder instruments

*Topics: 9.1.1 equity in a limited-recourse structure · 9.1.2 shareholder instruments · 9.1.3 the
cost of equity, built up · 9.1.4 the leverage trade-off, computed on both faces.*

### 9.1.1 Equity in a limited-recourse structure

**Definition.** Project equity is the capital that ranks last in the cash waterfall, absorbs the
first losses, carries no contractual return, and in exchange holds the residual and the votes.
In a limited-recourse structure it is also the *only* capital whose provider is normally
expected to lose the whole of it without the transaction failing, which is why lenders size it
as a risk buffer rather than as a funding source.

Three practical distinctions decide how equity behaves in a financing. **Committed versus
contributed:** equity is committed at close through a subscription agreement and contributed when
called, and lenders care about the enforceability of the commitment far more than its size — a
commitment supported by a letter of credit from an investment-grade bank is worth more, in credit
terms, than a larger promise from a thinly-capitalised sponsor. **Pro-rata versus back-ended
funding:** equity drawn alongside debt in the agreed ratio, or debt drawn first with equity
plugging the final gap; sponsors prefer the latter because it defers the outflow, lenders resist
it because it leaves them exposed while the sponsor has nothing at risk, and where they concede it
they require a standby letter of credit for the undrawn balance. **The equity bridge loan (EBL):**
a construction-period facility secured on the equity commitment rather than on the project,
funding the sponsor's contribution and repaid by it at commercial operations date. Because a rate
of return is sensitive to timing, this is one of the cheapest available uplifts to a sponsor's
headline number — and one of the least understood.

**Worked example 9.1.1 — what an equity bridge is actually worth.**

1. **Setup.** For this example only, Kestrel's two-year construction period is modelled
   explicitly: equity of **18,000,000** at *t* = 0, operations from year 3, `CFADS` **6,384,000**,
   senior debt service **5,009,635.23** for twelve operating years, then `CFADS` alone to the end
   of the 25-year operating life. An EBL is offered at **5.0 %**, interest capitalised, repaid
   from equity at COD.
2. **Formula.** Equity `IRR` is the rate at which the equity cash-flow stream has zero present
   value. The EBL's capitalised cost is `E × ((1 + r_EBL)^2 − 1)`.
3. **Substitution.** Without the EBL the stream is `(−18,000,000; 0; 0; 1,374,364.77 × 12;
   6,384,000 × 13)`. The EBL's cost is `18,000,000 × (1.05² − 1) = 18,000,000 × 0.1025`; the
   contribution at *t* = 2 becomes `18,000,000 + 1,845,000`.
4. **Result.** Base equity `IRR` **10.6696 %**. With the EBL, the sponsor injects **19,845,000**
   at *t* = 2 and the equity `IRR` rises to **11.6157 %** (an uplift of **94.61 basis points**,
   bought by paying **1,845,000** of interest).
5. **Interpretation.** The uplift is real but it is timing, not value: the project generates
   exactly the same cash. The governing test is a clean identity; **an equity bridge is
   accretive if and only if its rate is below the equity `IRR` it defers**, and at a rate
   exactly equal to that `IRR` the uplift is precisely zero; solving for the indifference rate
   here returns **10.6696 %**, the base equity `IRR` itself, to every decimal tested. Three
   cautions follow. The bridge is **debt**, and a sponsor reporting an EBL-enhanced `IRR`
   without disclosing it has presented a financing artefact as a project return. It is
   **recourse to the sponsor**, so it consumes corporate credit capacity the treasury may value
   above 95 basis points. And a bridge that cannot be repaid at COD (because the commitment has
   weakened or COD has slipped past the bridge's maturity) converts a timing benefit into a
   refinancing event at the worst moment in the project's life (Domain 8's delay arithmetic, in
   funding form).

### 9.1.2 Shareholder instruments

Equity is rarely a single instrument. The standard family, and what each is for:

| Instrument | Economic character | Why it is used |
|---|---|---|
| **Ordinary share capital** | Residual claim, votes, no contractual return | The irreducible risk layer lenders require |
| **Subordinated shareholder loan** | Contractual interest and repayment, ranked below all third-party debt | Interest may be deductible; repayment is easier than a capital reduction |
| **Preference shares** | Fixed or participating dividend, priority over ordinary equity, usually non-voting | Sizing a passive investor's return without giving control |
| **Sponsor support / contingent equity** | An obligation to inject on a defined trigger (cost overrun, ratio breach) | Bridges lender risk without funding it at close |
| **Deferred consideration / development-cost roll-in** | Development spend credited as equity at close | Recognises promoter value without cash |

**The shareholder loan is the instrument that repays study**, because it changes the after-tax
arithmetic: part of a sponsor's contribution structured as subordinated debt can create a
deductible interest expense while remaining, in ranking terms, indistinguishable from equity to
third-party lenders.

**Worked example 9.1.2 — the shareholder-loan shield, and the ranking that destroys it.**

1. **Setup.** Of Kestrel's 18,000,000 of equity, **6,000,000** is provided as a subordinated
   shareholder loan at **12.0 %**, ranked below the senior facility and serviceable only from
   distributions. Cash tax rate **20.0 %** (derived above). Discount the shield at the board's 8.0
   % over the 25-year life.
2. **Formula.** Annual shield = interest × tax rate. `PV = shield × AF(0.08, 25)`.
3. **Substitution.** Interest `6,000,000 × 12.0 % = 720,000`; shield `720,000 × 20 % = 144,000`;
   `AF(0.08, 25) = 10.674776`; `144,000 × 10.674776`.
4. **Result.** Annual shield **144,000**; present value **USD 1,537,168**: **25.62 %** of the
   6,000,000 tranche, created by a documentation choice rather than by any change in the
   project.
5. **Interpretation.** A quarter of the tranche's face value in present-value terms explains why
   shareholder loans are ubiquitous. It is also the most jurisdiction-dependent number in this
   domain: thin-capitalisation limits, interest-deduction caps expressed as a share of `EBITDA`,
   transfer-pricing constraints on the rate, and withholding tax on cross-border interest can
   each reduce the shield to nothing. **No shareholder-loan shield may be modelled without a
   written tax opinion on the specific structure.** The structural caution is sharper still: the
   shield exists only while the loan is genuinely subordinated. If the interest is drafted to
   rank **above** `CFADS` rather than being paid out of distributions, Kestrel's `DSCR` falls
   from **1.2743** to `6,384,000 / (5,009,635.23 + 720,000) =` **1.1142** (straight through the
   1.20× covenant and the 1.15× lock-up), and the tax saving has bought a default. Serviced
   correctly, out of distributions of **1,374,364.77**, the same interest is covered **1.9088**
   times and the senior ratio is untouched. The instrument is safe; the ranking clause is where
   it becomes dangerous.

### 9.1.3 The cost of equity, built up

**Definition.** The cost of equity `k_e` is the return an equity provider requires for bearing the
project's residual risk. It is not observed and never will be; it is **constructed**, and the
professional obligation is to construct it in a way that is documented, decomposable and
challengeable. The build-up form used throughout this volume:

```
k_e = r_f + β_e × ERP + CRP + SP
β_e = β_a × (1 + (1 − T) × D/E)
```

where `r_f` is the risk-free rate (a long-dated government yield in the cash flows' currency),
`ERP` the equity risk premium, `β_a` the asset (unlevered) beta of the project's activity, `β_e`
the equity beta after re-levering for the project's own capital structure, `CRP` a country risk
premium and `SP` a project-specific premium for single-asset concentration and illiquidity. Each
term is a judgment; stating them separately is what makes the judgment reviewable.

**Worked example 9.1.3 — Kestrel's cost of equity.**

1. **Setup.** Illustrative inputs, all to be replaced with the transaction's own: `r_f` **4.10 %**
   (long-dated sovereign yield in USD), `ERP` **6.00 %**, `β_a` **0.60** (contracted water utility
   — low systematic exposure because revenue is availability-based, per Domain 7), `CRP` **0.50
   %**, `SP` **0.50 %**. Capital structure `D/E = 42,000,000 / 18,000,000`; `T` **20.0 %**.
2. **Formula.** As above.
3. **Substitution.** `D/E = 2.333333`; `(1 − 0.20) × 2.333333 = 1.866667`; `β_e = 0.60 × (1 +
   1.866667) = 0.60 × 2.866667`; `β_e × ERP = 1.72 × 6.00`; `k_e = 4.10 + 10.32 + 0.50 + 0.50`.
4. **Result.** `β_e` **1.72**; `k_e` **15.42 %**.
5. **Interpretation.** Read the decomposition, not the total. Of the 15.42 %, **4.10 points** is
   the time value of money, **10.32 points** is systematic risk amplified by leverage, and
   **1.00 point** is country and single-asset risk. The leverage term is the largest single
   component and it is **created by the financing decision**: the first hint of this domain's
   central claim, that the sponsors' required return is a property of the structure they chose
   rather than of the plant. Two disciplines follow. `k_e` is **not constant across
   structures**: re-lever it whenever gearing changes, or every comparison in KA 9.2 is invalid.
   And the non-beta terms are *not* re-levered in this convention (a modelling choice with
   visible consequences in 9.A.1, to be stated and applied uniformly). Sponsors also carry a
   **target** return, commonly a round 15 % or 16 % set by committee policy rather than by
   build-up; the gap between a derived 15.42 % and a policy 16 % is a negotiation to be minuted,
   not a discrepancy to be resolved by adjusting a beta until the two agree.

### 9.1.4 The leverage trade-off, computed on both faces

Gearing is the only structural lever that moves the sponsor's return and the lender's protection
in opposite directions at the same time, which is why it is negotiated last and hardest. The two
faces must be computed together or the conversation is not honest.

**Worked example 9.1.4 — five gearings, three consequences.**

1. **Setup.** Kestrel's 60,000,000 capex funded at 60 %, 65 %, 70 %, 75 % and 80 % senior gearing;
   loan rate **6.0 %**, tenor **12 years**, `AF(0.06, 12) = 8.383844`; level `CFADS` **6,384,000**
   over the 25-year life; `k_e` re-levered at each gearing per 9.1.3; after-tax cost of debt
   **4.80 %** (KA 9.2.1). Equity cash flow is `CFADS` less senior debt service for twelve years,
   then `CFADS` for thirteen.
2. **Formula.** `DSCR = CFADS ÷ (D ÷ AF(r, n))` (Domain 10, KA 10.2.1); equity `IRR` from the
   equity stream; `WACC = g × k_d(1 − T) + (1 − g) × k_e`.
3. **Substitution.** At 70 %: `42,000,000 / 8.383844 = 5,009,635.23`; `6,384,000 / 5,009,635.23`;
   equity stream `(−18,000,000; 1,374,364.77 × 12; 6,384,000 × 13)`; `0.70 × 4.80 + 0.30 × 15.42`.
4. **Result.**

| Gearing | Senior debt | Equity | Debt service | `DSCR` | Distributable yrs 1–12 | `k_e` | Equity `IRR` | `WACC` |
|---|---|---|---|---|---|---|---|---|
| 60 % | 36,000,000 | 24,000,000 | 4,293,973.06 | **1.4867** | 2,090,026.94 | 13.02 % | **11.7685 %** | **8.0880 %** |
| 65 % | 39,000,000 | 21,000,000 | 4,651,804.15 | **1.3724** | 1,732,195.85 | 14.05 % | **12.1206 %** | **8.0370 %** |
| **70 %** | 42,000,000 | 18,000,000 | 5,009,635.23 | **1.2743** | 1,374,364.77 | 15.42 % | **12.5311 %** | **7.9860 %** |
| 75 % | 45,000,000 | 15,000,000 | 5,367,466.32 | **1.1894** | 1,016,533.68 | 17.34 % | **13.0193 %** | **7.9350 %** |
| 80 % | 48,000,000 | 12,000,000 | 5,725,297.41 | **1.1151** | 658,702.59 | 20.22 % | **13.6146 %** | **7.8840 %** |

5. **Interpretation.** Across twenty points of gearing the sponsors gain **184.61 basis points**
   of equity return, the project saves **20.40 basis points** of `WACC`, and the lenders lose
   **0.3716** of coverage. The ratio is the finding: **the equity return gain is 9.05 times the
   `WACC` gain**, which means gearing in a project financing is overwhelmingly a *transfer* to
   equity rather than an *efficiency* for the project. Priced at the margin, moving from 70 % to
   75 % buys **48.82 basis points** of equity `IRR` for **8.50** points of `DSCR` (about **5.75
   basis points of equity return per hundredth of coverage surrendered**), and that exchange
   rate, computed for the actual structure, is the only defensible basis on which to argue a
   gearing. Now impose Domain 10's covenants and the table stops being a menu. At **75 %** the
   1.20× covenant requires `CFADS` of `5,367,466.32 × 1.20 =` **6,440,960**, which exceeds the
   base-case 6,384,000: the covenant **fails on the base case, before any stress**. At **80 %**
   even the 1.15× lock-up requires **6,584,092** and also fails. Precisely: the 1.30× target
   binds at **41,171,123** of debt (**68.6185 %** gearing, Domain 10's Case A), the 1.20×
   covenant at **44,602,050** (**74.3367 %**), and the 1.15× lock-up at **46,541,269**
   (**77.5688 %**). The cost-minimising structure is unavailable and the constraint is not
   price, it is coverage. That is the domain's central claim, in a table.

> **Fig 9.1.4 — The leverage ladder: what gearing buys and what it costs.** Dual-scale line chart,
> x-axis senior gearing 60–80 %, left y-axis equity `IRR` (%) rising from **11.7685** at 60 % to
> **13.6146** at 80 % in brand blue, right y-axis `DSCR` falling from **1.4867** to **1.1151** in
> crimson. A slate line shows `WACC` falling only from **8.0880** to **7.8840** — visibly flat
> against the other two. Horizontal crimson reference bands at the 1.30× sizing target, the 1.20×
> covenant and the 1.15× lock-up, with vertical markers where each binds: **68.62 %**, **74.34 %**
> and **77.57 %**. The region right of 74.34 % is shaded and labelled "covenant fails on base
> case".
> Source: PCI original. Alt text: a rising equity-return line crossed by a falling coverage line
> against senior gearing, with a nearly flat cost-of-capital line between them and shaded
> thresholds
> marking the gearing at which each lender protection is breached.

**The two cases, and where the equity return really comes from.** On the level-`CFADS` basis the
base structure's equity `IRR` is **12.5311 %** — *below* the **15.42 %** cost of equity derived
in 9.1.3. That is not an arithmetic error; it is the difference between a lender's flat "bank
case" and a sponsor's escalating "sponsor case", and it is where equity's return actually lives.
Solving for the constant `CFADS` escalation that lifts the equity `IRR` to exactly 15.42 % gives
**1.7403 %** a year: `CFADS` of 6,384,000 in year one becomes 6,495,101 in year two, 6,608,136
in year three, and `DSCR` improves from **1.2743** to **1.5407** by year twelve. The whole
equity case therefore rests on **174 basis points of annual escalation** (a tariff-indexation
clause, not a financial-engineering choice). State it that way in the board paper: the equity
return is a contracted escalation assumption, and Domain 7's indexation drafting is the asset
that produces it.

### AI in this KA

**Where it earns its place.** Re-levering a beta across a gearing ladder, recomputing equity `IRR`
for twenty candidate structures, and solving for the escalation breakeven above are mechanical,
high-volume and error-prone by hand — exactly the work to delegate.

**Where it must not go.** It must not choose the premiums. `ERP`, `β_a`, `CRP` and `SP` determine
`k_e`, they are unobservable, and a model asked to "estimate the cost of equity" returns a
confident figure assembled from whatever premium conventions dominated its training data, with no
disclosure of which convention, which market or which date. A cost of equity without a named,
dated, owned source for each component is not an input, it is an opinion wearing decimals. Nor may
it decide the gearing: that trades a lender's protection against a sponsor's return and belongs to
accountable people.

**Verification, concretely.** Re-lever one case by hand and confirm `β_e`. Confirm the `WACC`
weights sum to one and that the debt weight uses the same debt figure as the coverage test. Test
the equity-bridge identity: set the bridge rate equal to the base equity `IRR` and confirm the
uplift computes to zero; a model reporting a gain there has a timing error. Confirm every quoted
equity `IRR` names its cash-flow case. **AI proposes; the professional verifies, decides and
remains accountable.**

### Key terms — KA 9.1

| Term | Meaning |
|---|---|
| **Committed vs contributed equity** | Enforceable obligation to subscribe, versus cash actually injected. |
| **Pro-rata / back-ended funding** | Equity drawn alongside debt, versus equity plugging the final gap. |
| **Equity bridge loan (EBL)** | Construction facility against the equity commitment; accretive only below the equity `IRR` it defers. |
| **Subordinated shareholder loan** | Sponsor funding as ranked debt; may create a tax shield, dangerous if mis-ranked. |
| **Contingent / sponsor support** | Obligation to inject on a defined trigger rather than at close. |
| **`k_e`** | Cost of equity, built up as `r_f + β_e × ERP + CRP + SP`. |
| **`β_a` / `β_e`** | Asset (unlevered) beta; equity beta re-levered as `β_a(1 + (1 − T)D/E)`. |
| **Bank case / sponsor case** | Lender's conservative cash forecast versus sponsor's escalating forecast; equity's return lives in the gap. |

### Sample MCQs — KA 9.1

**MCQ 9.1-A `[9.1.3 · Application]`** With `β_a` = 0.60, `D/E` = 42,000,000 / 18,000,000, `T` = 20
%, `r_f` = 4.10 %, `ERP` = 6.00 %, `CRP` = 0.50 % and `SP` = 0.50 %, the cost of equity is:
- A. 15.42 % ✅
- B. 8.70 %
- C. 17.10 %
- D. 14.42 %

*Rationale:* `β_e = 0.60 × (1 + 0.80 × 2.333333) = 1.72`; `k_e = 4.10 + 1.72 × 6.00 + 0.50 +
0.50 = 15.42 %`. B uses the **unlevered** beta throughout (`4.10 + 0.60 × 6.00 + 1.00 = 8.70`)
(the error of not re-levering at all). C re-levers **without** the tax adjustment (`β_e = 0.60 ×
3.333333 = 2.00` → `4.10 + 12.00 + 1.00 = 17.10`). D omits the country and single-asset premiums
(`4.10 + 10.32 = 14.42`) (building the beta term correctly and then forgetting the rest of the
build-up).

**MCQ 9.1-B `[9.1.4 · Analysis]`** Moving Kestrel from 70 % to 80 % gearing changes `WACC` from
7.9860 % to 7.8840 % and equity `IRR` from 12.5311 % to 13.6146 %, while `DSCR` falls from 1.2743
to 1.1151 against a 1.20× covenant. The correct conclusion is:
- A. gear to 80 %; the `WACC` is lower and `WACC` minimisation is the objective
- B. the 80 % structure is not financeable: the 1.20× covenant fails on the base case, and the
  10.2 basis points of `WACC` saved are irrelevant beside a breach ✅
- C. gear to 80 % and negotiate a 1.10× covenant, since the equity gain is large
- D. the two structures are equivalent because total capital is unchanged

*Rationale:* At 80 % the covenant requires `CFADS` of `5,725,297.41 × 1.20 = 6,870,357` against
6,384,000 available — a base-case breach, so the structure does not exist to be optimised (9.1.4).
A applies the corporate-finance objective to a constrained problem; C proposes a covenant no
lender sizing at 1.30× would grant and ignores that the lock-up also fails; D confuses the funding
total with its risk allocation.

**MCQ 9.1-C `[9.1.1 · Analysis]`** A sponsor's equity `IRR` on Kestrel is 10.6696 % without an
equity bridge. An EBL is offered at exactly 10.6696 %, interest capitalised over the two-year
build. The effect on the reported equity `IRR` is:
- A. it rises, because the equity outflow is deferred
- B. it is unchanged: a bridge priced exactly at the equity `IRR` it defers is value-neutral ✅
- C. it falls, because the sponsor pays interest it would otherwise not have paid
- D. it rises by 94.61 basis points

*Rationale:* Deferring an outflow at exactly the rate at which that outflow discounts leaves the
`IRR` identical (the indifference identity of 9.1.1, which the bisection confirms to the tested
precision). A states the general direction of a *cheap* bridge but ignores its pricing; C
confuses a cash cost with a rate effect, since the capitalised interest is exactly compensated
by the deferral; D quotes the uplift computed at a **5.0 %** bridge rate and applies it as
though the bridge rate were irrelevant.

**MCQ 9.1-D `[9.1.2 · Analysis]`** A 6,000,000 shareholder loan at 12 % is drafted so that its
interest ranks **above** `CFADS` rather than being paid from distributions. The consequence is:
- A. an additional tax shield of 144,000 a year and no other effect
- B. `DSCR` falls from 1.2743 to 1.1142, breaching both the 1.20× covenant and the 1.15× lock-up ✅
- C. the senior lenders are unaffected because the loan is subordinated in name
- D. the loan becomes senior debt for all purposes

*Rationale:* `6,384,000 / (5,009,635.23 + 720,000) = 1.1142` (9.1.2). A is the shield without
the ranking consequence; C mistakes a label for a waterfall position; D overstates: the ranking
clause changes the coverage calculation without converting the instrument.

**MCQ 9.1-E `[9.1.2 · Evaluation]`** A sponsor's model carries the 144,000 annual tax shield on
a 6,000,000 shareholder loan (1,537,168 of present value, 25.62 % of the tranche) inside the
base case that will be shown to lenders and to the investment committee. The soundest
professional position is that the shield:
- A. belongs in the base case, because interest on a shareholder loan is deductible
- B. should be halved, as a prudent allowance for jurisdictional uncertainty
- C. should be excluded from the base case until a written tax opinion on this specific structure
  confirms deductibility, thin-capitalisation headroom and withholding treatment, and should then be
  disclosed as a documented upside rather than embedded in the cash flow ✅
- D. means the shareholder loan should be replaced with ordinary share capital

*Rationale:* A quarter of a tranche's face value created by a documentation choice is the most
jurisdiction-dependent figure in this domain: thin-capitalisation limits, interest-deduction
caps expressed as a share of `EBITDA`, transfer-pricing constraints on the rate and withholding
tax on cross-border interest can each reduce it to nothing, so it is not an input until counsel
says it is (9.1.2). A states one jurisdiction's treatment as though it were universal. B
substitutes an arbitrary haircut for a determination that is obtainable and close to binary. D
discards a legitimate instrument to avoid a question of evidence, and the instrument's real
danger is the ranking clause, not the shield.

**MCQ 9.1-F `[9.1.3 · Comprehension]`** The build-up states `r_f`, `β_e × ERP`, `CRP` and `SP`
separately rather than quoting a single required return because:
- A. accounting standards require a disclosed decomposition of a discount rate
- B. only the risk-free rate is a judgment; the remaining terms are market data
- C. the components must be summed in that order for the total to be correct
- D. the cost of equity is constructed rather than observed, so separating the terms is what
  makes each judgment reviewable; and what allows the leverage term to be re-levered when
  gearing changes ✅

*Rationale:* `k_e` is not observed and never will be; stating the premiums separately is what
turns an assertion into something a reviewer can challenge term by term, and the beta term is
the one that must move with the structure or every structure comparison is invalid (9.1.3). A
invents a reporting requirement. C confuses a sum with a sequence. B reverses the position:
`ERP`, `β_a`, `CRP` and `SP` are all judgments, which is exactly why each needs a named, dated,
owned source.

**MCQ 9.1-G `[9.1.4 · Evaluation]`** The sponsors argue for **75 %** gearing on the exchange rate
computed in 9.1.4: moving from 70 % to 75 % buys **48.82 basis points** of equity `IRR` for 8.50
points of `DSCR`, about **5.75 basis points of equity return per hundredth of coverage surrendered**,
which they describe as the best-value structural trade available. The lenders size on a 1.30× target
(41,171,123, 68.6185 % gearing, `WACC` 8.0001 %); the 1.20× covenant binds at 44,602,050 (74.3367 %
gearing, `WACC` 7.9418 %). The soundest response is that the exchange-rate argument:
- A. is the right frame and stops applying at 74.34 %: above that gearing there is no coverage left
  to sell, and at 75 % the covenant fails on the base case, so the trade should be priced inside the
  feasible region and the recommendation made at the 68.62 % the sizing target permits ✅
- B. is correct and decisive: 5.75 basis points per hundredth is a favourable rate and the board
  should mandate 75 %
- C. is invalid, because coverage and equity return are not commensurable quantities
- D. should be resolved by mandating 74.34 %, the highest gearing the covenant permits

*Rationale:* At 75 % debt service of 5,367,466.32 requires `CFADS` of **6,440,960** against
6,384,000 available (short by **56,960** before any stress), so the marginal rate is being
quoted across a boundary at which the structure ceases to exist (9.1.4). B applies a valid
marginal calculation outside its domain. C rejects the only honest way to argue a gearing: the
exchange rate is exactly how the trade should be framed, and framing it is what reveals where it
ends. D is the defensible weaker course: it satisfies the covenant arithmetically, with nil
headroom, and lenders set a target above the covenant precisely so that a covenant is not a
base-case condition; the constraint's price against the sponsors' proposal is **6.51 basis
points** of `WACC`, which is what belongs in the paper.

### Self-check — KA 9.1

1. *When is an equity bridge accretive?* Only when its rate is below the equity `IRR` it defers;
   at equality the uplift is exactly zero.
2. *Why must `k_e` be re-levered before any structure comparison?* Because the leverage term is
   the largest component of `k_e` (10.32 of Kestrel's 15.42 points); holding it constant across
   gearings compares incomparable things.
3. *State Kestrel's leverage exchange rate at the margin.* About 5.75 basis points of equity
   `IRR` per 0.01 of `DSCR` surrendered, moving from 70 % to 75 %.

---

## Knowledge Area 9.2 — Senior, subordinated and mezzanine debt; bonds

*Topics: 9.2.1 the senior tranche and the after-tax cost of debt · 9.2.2 subordination and
mezzanine · 9.2.3 the blended cost of a multi-tranche stack and the project `WACC` · 9.2.4 the
capital-markets route.*

### 9.2.1 The senior tranche and the after-tax cost of debt

**Definition.** Senior debt ranks first in the cash waterfall, holds the security package, sets
the covenants, and is therefore the cheapest and largest tranche in almost every project
financing. Its cost to the borrower has three layers: the **base rate** (a floating reference
rate, or a fixed rate achieved by swapping), the **credit margin** (basis points over base,
stepping up or down with project phase and sometimes with coverage), and **fees** — arrangement,
commitment on undrawn amounts, agency, and any hedging cost. The headline rate is never the cost;
9.3 makes that point quantitatively.

**The after-tax cost of debt** is what enters `WACC`, because interest is (in many but not all
jurisdictions) deductible:

```
k_d(after tax) = k_d × (1 − T)
```

For Kestrel: `6.00 % × (1 − 0.20) =` **4.80 %**. The shield is worth **504,000** in year one
(2,520,000 × 20 %) and declines with the interest profile, which matters because a project's
amortising debt front-loads the shield: a level after-tax rate is a simplification, and where
tax losses, carry-forwards or interest-limitation rules bite it is a *material* simplification
that the model must handle explicitly (Domain 6, KA 6.2).

Three properties distinguish senior project debt from corporate borrowing: it is **drawn
progressively** against certified construction spend, so commitment fees and interest during
construction are real costs (Domain 14); it is **covenanted on cash** rather than on accounting
measures (Domain 10, KA 10.4.1); and its **tenor is bounded by the concession or offtake term**,
which is why Domain 10's sizing levers include tenor but not appetite.

### 9.2.2 Subordination and mezzanine

**Definition.** Subordinated debt ranks behind senior debt in the waterfall and in enforcement,
and is compensated for that position with a materially higher return. **Mezzanine** describes
instruments sitting between senior debt and ordinary equity: subordinated loans, high-yield notes,
payment-in-kind instruments, and hybrids with equity participation (warrants, conversion rights,
or a return that steps up if the project outperforms).

Mezzanine exists because there is a gap between what senior lenders will advance and what
sponsors will fund with equity, and it is priced in that gap, above senior margins, below the
equity return, at a level reflecting a genuine risk of total loss with no upside beyond the
coupon. Two placements matter enormously and are routinely confused. **Project-level (SPV)
mezzanine** sits inside the borrowing entity, so its debt service appears in the project's own
coverage calculation; senior lenders normally prohibit it or confine it to a defined basket,
precisely because it consumes the `CFADS` their covenant measures. **HoldCo mezzanine** sits
above the SPV in the sponsors' holding company and is serviced from **distributions**: invisible
to the SPV's `DSCR`, which is why sponsors prefer it, but fully exposed to the distribution
lock-up of Domain 10, KA 10.4.2 (the single most under-appreciated risk in the instrument, which
Case study A quantifies).

### 9.2.3 The blended cost of a multi-tranche stack, and the project `WACC`

**Definition.** The weighted average cost of capital is the weighted mean of each tranche's cost,
weighted by its share of total capital, with debt-like tranches taken **after tax**:

```
WACC = Σ (Vᵢ / V) × kᵢ        with kᵢ = kᵢ(pre-tax) × (1 − T) for deductible tranches
```

Two rules do all the damage when broken. **Weights are market or funding values of the capital
actually deployed**, not book equity or authorised capital. And **only genuinely deductible
costs are tax-adjusted**; equity never is, and mezzanine may or may not be, depending on the
instrument and the jurisdiction.

**Worked example 9.2.3 — three structures priced, and the reconciliation of Domain 4's 8 %.**

1. **Setup.** Kestrel's 60,000,000 funded three ways. **A (base):** senior 42,000,000 at 6.00 %,
   equity 18,000,000. **B (mezzanine displacing senior):** senior 36,000,000 at 5.70 % (the
   margin tightens because 9,000,000 of junior capital now sits beneath it) mezzanine 9,000,000
   at 11.50 % amortising over 15 years, equity 15,000,000. **C (mezzanine displacing equity):**
   senior 42,000,000 at 6.00 %, mezzanine 6,000,000 at 11.50 % over 15 years, equity 12,000,000.
   Tax 20.0 %; mezzanine interest assumed deductible (a jurisdictional assumption, flagged);
   `k_e` re-levered at each structure per 9.1.3 with all debt-like tranches counted as debt;
   `AF(0.057, 12) = 8.523470`, `AF(0.115, 15) = 6.996708`.
2. **Formula.** `WACC` as above; `DSCR` per Domain 10, KA 10.2.1, computed both on senior debt
   service alone and on total debt service; equity `IRR` from the residual stream.
3. **Substitution.** B: senior service `36,000,000 / 8.523470 = 4,223,631.91`; mezzanine
   `9,000,000 / 6.996708 = 1,286,319.25`; `k_e` at `D/E = 45,000,000/15,000,000 = 3.0` gives `β_e
   = 0.60 × (1 + 0.80 × 3.0) = 2.04` and `k_e = 5.10 + 2.04 × 6.00 = 17.34 %`; `WACC = (36 × 4.56
   + 9 × 9.20 + 15 × 17.34) / 60`.
4. **Result.**

| | A — base 70/30 | B — mezz displaces senior | C — mezz displaces equity |
|---|---|---|---|
| Senior | 42,000,000 @ 6.00 % | 36,000,000 @ 5.70 % | 42,000,000 @ 6.00 % |
| Mezzanine | — | 9,000,000 @ 11.50 % | 6,000,000 @ 11.50 % |
| Equity | 18,000,000 | 15,000,000 | 12,000,000 |
| `k_e` (re-levered) | 15.42 % | 17.34 % | 20.22 % |
| Senior debt service | 5,009,635.23 | 4,223,631.91 | 5,009,635.23 |
| Mezzanine service | — | 1,286,319.25 | 857,546.17 |
| **`DSCR` — senior only** | **1.2743** | **1.5115** | **1.2743** |
| **`DSCR` — total service** | **1.2743** | **1.1586** | **1.0881** |
| **Equity `IRR`** | **12.5311 %** | **12.0824 %** | **12.7378 %** |
| **`WACC`** | **7.9860 %** | **8.4510 %** | **8.3240 %** |

5. **Interpretation.** **Mezzanine raises the blended cost in both directions** (by 46.50 basis
   points in B and 33.80 in C), and the reason is structural: mezzanine's after-tax cost of
   **9.20 %** sits far above senior's **4.80 %**, and the equity left underneath it is more
   levered and therefore dearer (17.34 % and 20.22 % against 15.42 %). Anyone selling mezzanine
   as a route to cheaper capital is selling arithmetic that does not exist. What it actually
   buys depends on **which tranche it displaces**. In B it displaces senior debt and buys the
   senior lenders comfort (`DSCR` **1.5115** against 1.2743, inside the 1.30× target Domain 10's
   Case A could not reach) at the price of 46.50 basis points of `WACC` *and* 44.87 basis points
   of equity return, because 11.50 % money replaced 6.00 % money. In C it displaces equity and
   buys the sponsors 20.67 basis points of equity `IRR`, but total coverage collapses to
   **1.0881**, below any covenant in the facility, so C exists only if the mezzanine sits at
   HoldCo level and is invisible to the SPV test. **Mezzanine is bought for coverage relief or
   for equity uplift, never for a lower cost of capital, and the tranche it displaces decides
   which.**

**Now the reconciliation this volume has owed since Domain 4.** Substitute Kestrel's re-levering
into the `WACC` definition and the whole ladder collapses to a straight line:

```
WACC(g) = [F + ERP × β_a] − g × [F + ERP × β_a × T − k_d(1 − T)]
        = 8.70 % − 1.02 % × g          with F = r_f + CRP + SP = 5.10 %
```

Every entry in the 9.1.4 table satisfies it exactly: 8.0880 at *g* = 0.60, 7.9860 at 0.70,
7.8840 at 0.80. The slope tells the real story. Its two parts are `F − k_d(1 − T) = 5.10 − 4.80
= 0.30` and the tax-shield term `ERP × β_a × T = 3.60 × 0.20 = 0.72`. **Because the
non-systematic part of the equity build-up (5.10 %) barely exceeds the after-tax cost of senior
debt (4.80 %), leverage buys the project almost nothing**, 1.02 basis points of `WACC` per point
of gearing, most of it supplied by the tax shield. The benefit of gearing accrues to equity as
return, not to the project as cheaper capital.

Now evaluate the line where the project is actually financeable. Domain 10 established that a
1.30× target caps senior debt at **41,171,123** (**68.6185 %** gearing, with equity of
**18,828,877**). At that gearing `β_e` is **1.649565**, `k_e` is **14.9974 %**, and

```
WACC = 8.70 % − 1.02 % × 0.686185 = 8.0001 %
```

**Domain 4's given 8.0 % was exactly right, and for a reason nobody had written down.** It is
not a policy round number: it is the project's own weighted average cost of capital at the
maximum gearing its coverage requirement permits. The board's rate and the credit committee's
ratio were describing the same structure from opposite sides of the table. Two consequences. The
rate is right **only for that structure**: the actual 70/30 proposal prices at **7.9860 %**,
raising the appraisal `NPV` from 16,179,360 to **16,244,525** (**+65,164**) — a harmless
difference, but only because it was checked. And the cost-minimising 80 % structure prices at
**7.8840 %**, so the coverage constraint costs **11.61 basis points** of `WACC`. That is the
honest price of bankability, and it is small (the best argument available for not fighting the
coverage requirement).

> **Fig 9.2.3 — Four capital structures, priced.** Stacked horizontal bars, one per structure —
> base 70/30; mezzanine-displacing-senior (B); mezzanine-displacing-equity (C); and the
> financeable
> structure of 41,000,000 senior with 19,000,000 equity — each bar segmented by tranche share of
> 60,000,000 and labelled with its after-tax cost (senior 4.80 % or 4.56 % in brand blue,
> mezzanine 9.20 % in slate, equity 14.91 %–20.22 % in crimson). Beneath each bar, three outcome
> figures: `WACC` (**7.9860 · 8.4510 · 8.3240 · 8.0030 %**), total `DSCR`
> (**1.2743 · 1.1586 · 1.0881 · 1.3054**) and equity `IRR`
> (**12.5311 · 12.0824 · 12.7378 · 12.3868 %**). A crimson vertical reference at 8.0001 % labelled
> "the project `WACC` at the coverage-binding gearing — the rate Domain 4 was given". Source: PCI
> original. Alt text: four horizontal stacked bars showing tranche mix and after-tax cost for four
> candidate capital structures, with cost of capital, coverage and equity return reported beneath
> each bar.

### 9.2.4 The capital-markets route

**Definition.** Instead of a bank loan, a project may issue **bonds**, notes sold to
institutional investors, publicly or by private placement, with or without a rating and with or
without a third-party guarantee ("wrapped"). Three axes drive the choice. **Tenor:** insurers
and pension funds matching long liabilities will hold 20- to 30-year paper that few banks will,
and for a long concession that is decisive (9.3.3 quantifies why tenor beats rate). **Price and
rating:** rated investment-grade paper can price below bank margins and sub-investment grade
well above, and the rating process imposes disclosure and a published opinion on the project's
credit — valuable or unwelcome depending on circumstances. **Negative arbitrage:** bonds are
normally drawn in full at close while construction spends over years, so idle proceeds earn a
deposit rate far below the coupon — a real cost that progressively-drawn bank debt does not
incur.

**Worked example 9.2.4 — the price of drawing early.**

1. **Setup.** Kestrel's 42,000,000 raised as a bond at a 6.0 % coupon, fully drawn at close, spent
   evenly over a two-year construction period so the average idle balance is half the issue.
   Deposit rate on idle proceeds **3.0 %**. Bank alternative: progressive drawdown with a **0.60
   %** commitment fee on the average undrawn balance.
2. **Formula.** Negative arbitrage = average idle balance × (coupon − deposit rate) × years.
   Commitment cost = average undrawn × fee rate × years.
3. **Substitution.** `21,000,000 × (6.0 % − 3.0 %) × 2` against `21,000,000 × 0.60 % × 2`.
4. **Result.** Negative arbitrage **1,260,000**; commitment fees **252,000**; the bank route is
   **1,008,000** cheaper on this dimension alone. Spread over the 12-year facility, the negative
   arbitrage is worth **35.78 basis points** a year on the 42,000,000.
5. **Interpretation.** Thirty-six basis points is the same order as the entire `WACC` benefit of
   twenty points of gearing (9.2.3), which puts the drawdown mechanism in its proper place: a
   first-order structuring decision, not an administrative detail. It also explains the standard
   market answer (a **bank facility during construction, refinanced into bonds at or after
   completion**) capturing progressive drawdown while the spend is uncertain and long
   institutional tenor once the risk profile has changed. That is not a compromise but the
   correct sequencing, and KA 9.4.4 prices it. Two asymmetries complete the comparison.
   **Flexibility:** a bank syndicate of six can approve a waiver in weeks (Domain 10, KA
   10.4.3), while a dispersed bondholder base cannot easily be assembled at all — making bonds a
   poor choice where amendments are foreseeable. **Prepayment:** bonds typically carry
   make-whole or spens provisions capturing the lender's lost yield, so the refinancing gain
   computed in 9.4.4 may simply not be available.

### AI in this KA

**Where it earns its place.** Normalising a dozen indicative term sheets (rate, tenor,
amortisation profile, fee schedule, prepayment mechanics, covenant thresholds) into one
comparable table is genuinely strong machine work and the foundation of Toolkit 9.T.2, as is
generating the candidate structures whose arithmetic 9.2.3 tabulates.

**Where it must not go.** It must not decide the tax treatment of a tranche. Whether mezzanine
interest is deductible, whether a shareholder loan survives thin-capitalisation rules and whether
withholding applies are questions of law in a specific jurisdiction at a specific date, and a
wrong answer moves `WACC` by hundreds of basis points while looking entirely plausible. Nor may it
assert a market price: an indicative margin generated by a model is not a quote, and presenting
one as though it were misrepresents to whoever relies on it.

**Verification, concretely.** Recompute one structure's `WACC` by hand and confirm the weights sum
to one. Confirm every tax adjustment traces to a written opinion rather than a default. Confirm
any all-in cost was solved from the actual stream rather than assembled by adding fees to a
headline rate. And test the closed form: under the re-levering convention of 9.1.3 a `WACC` ladder
that is not linear in gearing contains an error.

### Key terms — KA 9.2

| Term | Meaning |
|---|---|
| **`k_d`(after tax)** | `k_d × (1 − T)`; the debt cost that enters `WACC`. |
| **`WACC`** | Σ(tranche share × tranche cost), debt-like tranches after tax. |
| **Mezzanine** | Capital between senior debt and ordinary equity; priced in the gap, never cheap. |
| **SPV vs HoldCo mezzanine** | Inside the borrower and visible to `DSCR`, versus above it and exposed to lock-up. |
| **Negative arbitrage** | Cost of holding fully drawn bond proceeds at deposit rates below the coupon. |
| **Wrapped bond** | Notes credit-enhanced by a third-party guarantor. |
| **Make-whole / spens** | Prepayment compensation that can eliminate a refinancing gain. |

### Sample MCQs — KA 9.2

**MCQ 9.2-A `[9.2.3 · Application]`** A 150,000,000 project is funded with senior 80,000,000 at
5.2 %, mezzanine 20,000,000 at 10.8 % (both deductible) and equity 50,000,000 at 14.5 %; tax 30 %.
The `WACC` is:
- A. 7.7827 % ✅
- B. 9.0467 %
- C. 8.5667 %
- D. 6.3327 %

*Rationale:* `(80 × 3.64 + 20 × 7.56 + 50 × 14.5)/150 = 7.7827 %`. B omits the tax shield on both
debt tranches; C takes a simple average of the three costs, ignoring the weights; D tax-adjusts
the **equity** cost as well, which no jurisdiction permits.

**MCQ 9.2-B `[9.2.3 · Analysis]`** Adding 9,000,000 of 11.50 % mezzanine in place of senior debt
raises Kestrel's senior-only `DSCR` from 1.2743 to 1.5115 and its `WACC` from 7.9860 % to 8.4510
%. The correct reading is:
- A. the structure is superior because coverage improved
- B. the structure is inferior because `WACC` rose
- C. mezzanine bought 23.72 points of senior coverage for 46.50 basis points of `WACC` and 44.87
  basis points of equity `IRR`; whether that is worth paying depends on whether the senior tranche
  is otherwise unavailable ✅
- D. `WACC` and `DSCR` cannot both be affected by one tranche

*Rationale:* Both faces move and the decision is the exchange rate between them (9.2.3). A and B
each optimise one number in isolation: the specific error this domain exists to prevent; D is
simply false, since the mezzanine changes both the weighted cost and the debt service.

**MCQ 9.2-C `[9.2.4 · Application]`** A 42,000,000 bond at a 6.0 % coupon is drawn in full at
close and spent evenly over two years; idle proceeds earn 3.0 %. The negative arbitrage is:
- A. USD 2,520,000
- B. USD 1,260,000 ✅
- C. USD 630,000
- D. nil, because the proceeds are invested

*Rationale:* Average idle balance 21,000,000 × 3.0 % spread × 2 years = 1,260,000. A applies the
spread to the **full** 42,000,000 for two years, ignoring the drawdown profile; C covers one year
only; D confuses earning a return with earning enough of one.

**MCQ 9.2-D `[9.2.2 · Analysis]`** Why do senior lenders usually prohibit mezzanine debt at SPV
level while tolerating it at HoldCo level?
- A. HoldCo debt is cheaper
- B. SPV mezzanine consumes the `CFADS` their covenant measures, while HoldCo mezzanine is
  serviced from distributions that already rank behind every senior test ✅
- C. HoldCo debt carries no security
- D. accounting standards require it

*Rationale:* The placement determines whether the junior service appears in the coverage
calculation (9.2.2). A is not generally true; HoldCo debt is usually dearer, being further from
the cash; C is incidental; D confuses drafting practice with reporting requirements.

**MCQ 9.2-E `[9.2.3 · Evaluation]`** A board paper discounts Kestrel's cash flows at 7.8840 %
(the `WACC` of the 80 % gearing structure) on the ground that it is the lowest cost of capital
available to the project. The soundest position is that the appraisal should use:
- A. 7.8840 %, because minimising `WACC` is the objective of a capital-structure decision
- B. the sponsors' corporate `WACC`, since it is the sponsors' shareholders who set the hurdle
- C. the `WACC` of a structure the project can actually raise (8.0001 % at the coverage-binding
  gearing) quoted together with the structure it belongs to ✅
- D. any rate between 7.8840 % and 8.0001 %, since the whole range is 11.61 basis points

*Rationale:* A cost of capital is a property of one specific structure, and the 80 % structure fails
the 1.20× covenant on the base case, so its rate prices a financing that does not exist; the
coverage constraint costs 11.61 basis points of `WACC`, and that is the honest price of bankability
(9.1.4, 9.2.3). A imports the corporate-finance objective into a constrained problem. B discounts a
single ring-fenced asset at a parent's blended risk. D is right about materiality and wrong about
discipline: the difference is small only because someone checked it, and an unlabelled rate is
reused in places where the difference is not small.

**MCQ 9.2-F `[9.2.4 · Evaluation]`** The board is attracted by a 20-year project bond at close:
it matches institutional appetite to a 25-year asset and removes the refinancing question. Drawn
in full at close against a two-year construction spend, the bond incurs **1,260,000** of
negative arbitrage (**35.78 basis points** a year on 42,000,000, levelised over the facility)
against **252,000** of commitment fees on a progressively drawn bank facility, and it would
carry make-whole prepayment protection. The soundest recommendation is:
- A. issue the bond: 35.78 basis points of negative arbitrage is a modest price for twenty years of
  committed tenor
- B. bank facility only: bonds are inappropriate for project financings because amendments are
  impracticable across a dispersed holder base
- C. issue the bond and negotiate a delayed-draw structure, which removes the negative arbitrage
- D. bank facility during construction, refinanced into bonds at or after completion: capturing
  progressive drawdown while the spend is uncertain and institutional tenor once the risk
  profile has changed; and the bond's make-whole terms must be settled at that point, since they
  can eliminate the refinancing gain the sequencing exists to capture ✅

*Rationale:* The two instruments are strong in different phases, so the answer is sequencing
rather than selection, and 1,008,000 of avoidable cost on this dimension alone is the same order
as the entire `WACC` benefit of twenty points of gearing (9.2.4). A treats a first-order
structuring decision as an administrative detail. C is the defensible weaker course: a
delayed-draw or forward-purchase structure does address negative arbitrage, at a commitment cost
of its own, and it leaves the amendment inflexibility of a bond in place through the phase in
which amendments are most likely. B promotes one true asymmetry into a prohibition and forgoes
the tenor that makes a long concession financeable.

### Self-check — KA 9.2

1. *State the closed form for Kestrel's `WACC` and what its slope means.* `WACC(g) = 8.70 % −
   1.02 % × g`; the slope is small because the non-systematic equity premium (5.10 %) barely
   exceeds after-tax senior debt (4.80 %), so gearing transfers return to equity rather than
   cheapening the project.
2. *What is the project `WACC` at the coverage-binding gearing, and why does it matter?*
   **8.0001 %** at 68.6185 % gearing: it is Domain 4's given rate, derived.
3. *What does mezzanine buy?* Coverage relief or equity uplift, depending on the tranche it
   displaces; never a lower cost of capital.

---

## Knowledge Area 9.3 — Islamic finance concepts, export credit and development finance

*Topics: 9.3.1 Islamic finance structures in economic terms · 9.3.2 export credit · 9.3.3
development finance · 9.3.4 assembling a multi-source stack.*

### 9.3.1 Islamic finance structures in economic terms

**Scope statement, and it binds.** This topic describes, in economic and cash-flow terms,
structures used in Islamic finance markets, because a project finance leader must be able to price
them, model them and compare them. Whether any particular structure is compliant with Shariah is a
determination for the relevant Shariah supervisory board, guided by the standards of bodies such
as the Accounting and Auditing Organisation for Islamic Financial Institutions; **it is outside
the scope of this book, and nothing here expresses, implies or substitutes for any religious or
legal ruling.**

The structures share one economic feature: the financier's return arises from a transaction in
an asset (a sale, a lease, a partnership, an agency), rather than from lending money at
interest. The principal forms encountered in project financing:

| Structure | Economic mechanics | Typical project use |
|---|---|---|
| **Istisna'a** | Commissioned manufacture or construction: the financier procures the asset to specification and takes delivery risk during the build | The construction-period facility |
| **Ijara** | Lease of a completed asset; the lessee pays rentals comprising a capital and a return element; ownership sits with the lessor | The operating-period facility |
| **Ijara mawsufah fi al-dhimmah** (forward lease) | Rentals commence on a described future asset, bridging construction to operation | The standard istisna'a-to-ijara conversion |
| **Murabaha** | Cost-plus sale with deferred payment; the mark-up is agreed and fixed at inception | Working capital, procurement of specific inputs |
| **Wakala** | Agency: the financier appoints an agent to invest funds for an expected return | Portfolio and liquidity tranches |
| **Sukuk** | Certificates representing undivided beneficial ownership in an asset, usufruct or business, with returns generated by that asset | The capital-markets tranche, often alongside a bank ijara |

Three consequences a leader must handle. **The asset must exist and be identifiable**, which
constrains what can be financed and makes security and title arrangements materially different
from a conventional mortgage-and-charge package. **Ownership carries obligations**: a lessor's
responsibility for major maintenance and insurance in an ijara is usually passed back to the
lessee through a service agency agreement, and the drafting of that pass-back is where economic
equivalence is achieved or lost. And **intercreditor arrangements between Islamic and
conventional tranches are the hardest documentation in the deal**, because both must share
security and enforcement proceeds on a genuinely equal footing without either being subordinated
in substance.

**Worked example 9.3.1 — comparing an ijara tranche with a conventional tranche, properly.**

1. **Setup.** Kestrel's 42,000,000 is to be raised half conventionally and half through an ijara
   tranche, each **21,000,000 over 12 years**. The conventional tranche carries **6.00 %** with a
   **1.20 %** arrangement fee deducted from proceeds. The ijara carries a **6.15 %** profit rate
   with a **0.60 %** structuring fee. The sponsor's question is which is dearer.
2. **Formula.** The only defensible comparison is the **all-in effective cost**: the rate `r`
   solving `net proceeds = periodic payment × AF(r, n)`. Headline rates and fees cannot be added.
3. **Substitution.** Conventional: payment `21,000,000 / 8.383844 = 2,504,817.62`; fee `21,000,000
   × 1.20 % = 252,000`; net proceeds **20,748,000**; solve `20,748,000 = 2,504,817.62 × AF(r,
   12)`. Ijara: `AF(0.0615, 12) = 8.315327`; rental `21,000,000 / 8.315327 = 2,525,456.84`; fee
   **126,000**; net proceeds **20,874,000**; solve likewise.
4. **Result.** Conventional all-in **6.2209 %**; ijara all-in **6.2604 %**. The ijara is dearer by
   **3.95 basis points** — not by the 15 basis points the headline rates suggest. Annual payments
   differ by **20,639.22**, worth **173,036** in present value at 6 % over the twelve years.
5. **Interpretation.** The headline gap of 15 basis points shrank to 4 once the lower structuring
   fee was counted, and that reversal is the whole lesson: **a tranche is compared on the
   effective rate solved from its own cash-flow stream, never on its stated rate plus its fees.**
   Four basis points on 21,000,000 is 173,036 of present value — real money, but small enough that
   it should not decide the question. What should decide it is what the arithmetic cannot show:
   whether the ijara tranche reaches **investors the conventional tranche cannot**, widening the
   pool and reducing concentration risk; whether the extra documentation and intercreditor
   complexity are worth **173,036**; and whether the service-agency pass-back leaves the SPV with
   a residual ownership obligation the model has not priced. A leader who compares only the rates
   decides on the wrong axis in either direction.

### 9.3.2 Export credit

**Definition.** An export credit agency (ECA) is a state-backed institution that supports its
country's exporters by guaranteeing, insuring or directly lending against the purchase of that
country's goods and services. The effect for a project is a tranche whose credit risk is largely
sovereign rather than commercial, producing a lower margin and a longer tenor than the
commercial market would offer (in exchange for three conditions). **Sourcing (content)
requirements** tie support to eligible goods and services from the supporting country, usually
as a minimum share of contract value; this constrains procurement, and Case study B prices what
that constraint can cost. **An exposure premium** reflecting buyer-country risk and tenor is
charged, commonly **capitalised into the loan**, so it does not reduce cash proceeds but
increases the amount repaid: precisely the structure that makes headline-rate comparison
worthless. And participating agencies operate within the **OECD Arrangement on Officially
Supported Export Credits**, an inter-governmental understanding constraining minimum premium
rates, maximum repayment terms, starting points and local-content treatment; its terms are
revised periodically and vary by sector, so they must be checked as at the transaction date
rather than assumed.

**Worked example 9.3.2 — the true cost of a cheap ECA tranche.**

1. **Setup.** **15,000,000** of eligible equipment is financed by an ECA-supported tranche at
   **3.80 %** over **14 years**, with an exposure premium of **6.0 %** of the tranche
   **capitalised** into the loan. The commercial comparator is 15,000,000 at **6.00 %** over 12
   years.
2. **Formula.** All-in cost is the rate solving `cash proceeds = instalment × AF(r, n)`, where the
   instalment is computed on the **grossed-up** loan and the proceeds are the amount actually
   available to the project.
3. **Substitution.** Premium `15,000,000 × 6.0 % = 900,000`; loan **15,900,000**; `AF(0.038, 14) =
   10.703972`; instalment `15,900,000 / 10.703972 = 1,485,429.84`; solve `15,000,000 =
   1,485,429.84 × AF(r, 14)`.
4. **Result.** All-in cost **4.6895 %** (**88.95 basis points** above the 3.80 % headline, and
   still **131 basis points** below the commercial alternative). Annual debt service falls from
   `15,000,000 / 8.383844 = 1,789,155.44` to **1,485,429.84**, a saving of **303,725.60** a
   year.
5. **Interpretation.** Both halves of that result matter and they are usually conflated. The
   **cost** advantage is 131 basis points, which is genuine but not transformational. The
   **coverage** advantage is 303,726 of annual debt service released, and that is decisive:
   substituting the ECA tranche for 15,000,000 of Kestrel's commercial senior debt leaves total
   debt service of `27,000,000/8.383844 + 1,485,429.84 = 3,220,479.79 + 1,485,429.84 =`
   **4,705,909.63**, so `DSCR` rises from **1.2743** to **1.3566** — **8.23 points**, straight
   through the 1.30× sizing target the all-commercial structure could not reach. The two effects
   have different causes (the cost advantage from the sovereign-backed margin, the coverage
   advantage mostly from the **two extra years of tenor**), and separating them is the
   discipline of 9.3.3. The cautions are procedural and expensive when missed: ECA processes run
   to their own timetable and can add months to financial close; documentation is
   agency-specific and in places non-negotiable; and the eligible-content test is verified
   against invoices after the fact, so a procurement change during construction can
   retrospectively disqualify a tranche.

### 9.3.3 Development finance

**Definition.** Development finance institutions (DFIs), multilateral and bilateral development
banks and their private-sector arms, lend and invest to advance development objectives,
contributing what commercial lenders often cannot: **tenor** well beyond the bank market,
**availability** through periods when commercial appetite has withdrawn, political-risk comfort
that can reduce other lenders' margins, and structures such as an A/B loan in which the
institution is lender of record and commercial banks take participations behind its umbrella.

The price is **conditionality**: environmental and social performance requirements (the IFC
Performance Standards and the Equator Principles are the frameworks most commonly referenced by
lenders in this market) together with procurement, disclosure, integrity and reporting
obligations. Meeting them costs money and time, and that cost belongs in the tranche's economics
rather than in an overhead line where nobody sees it.

**Worked example 9.3.3 — separating the cost effect from the capacity effect.**

1. **Setup.** A DFI offers **12,000,000** at **5.25 %** over **18 years**, with a **1.00 %**
   front-end fee, an incremental environmental and social advisory cost of **350,000** at close,
   and **120,000** a year of monitoring and reporting cost through the loan life.
2. **Formula.** All-in economic cost solves `net proceeds = (instalment + annual compliance cost)
   × AF(r, n)`. Debt capacity is Domain 10's `max debt = (CFADS ÷ target DSCR) × AF(r, n)`,
   computed on the **contractual** rate and tenor.
3. **Substitution.** `AF(0.0525, 18) = 11.464588`; instalment `12,000,000 / 11.464588 =
   1,046,701.34`; net proceeds `12,000,000 − 120,000 − 350,000 = 11,530,000`; annual outflow
   `1,046,701.34 + 120,000 = 1,166,701.34`; solve for `r`. Capacity: `6,384,000 / 1.30 =
   4,910,769.23`, then `× 11.464588` against `× 8.383844`.
4. **Result.** All-in economic cost **7.2465 %** (**199.65 basis points** above the 5.25 %
   headline, and **125 basis points above the commercial 6.00 %**). Yet on the contractual
   terms, debt capacity at a 1.30× target rises from Domain 10's **41,171,123** to
   **56,299,948**, an uplift of **15,128,825**; and Kestrel's existing 41,171,123 of debt,
   repaid over 18 years at 5.25 %, would carry debt service of only **3,591,155.80** and a
   `DSCR` of **1.7777**.
5. **Interpretation.** This is the domain's most counter-intuitive result and it must not be
   collapsed into one verdict. On **cost** the DFI tranche is expensive: the headline 5.25 % is
   a fiction once the front-end fee, the advisory spend and 120,000 a year of monitoring are
   counted, and at 7.2465 % it is dearer than the commercial market. On **capacity** it is
   transformational: six extra years of tenor support **36.7 % more debt** at the same coverage,
   more than any plausible margin negotiation could deliver. **Tenor beats rate**, because
   coverage is the binding constraint and tenor is the lever that moves it most: Domain 10, KA
   10.1.2 lists four levers, and this is the arithmetic showing which one pays. Three cautions.
   The compliance cost is **not** waste: it buys an environmental and social management system a
   well-run project would want anyway, and other lenders price the comfort it provides, so
   charging all of it against the DFI tranche overstates its cost. The monitoring obligation
   runs for the full 18 years and must be budgeted for 18 years, not three. And DFI processes
   are slow: putting a DFI tranche on the critical path to financial close without allowing for
   that creates a schedule risk Domain 8's arithmetic prices at hundreds of thousands a month.

### 9.3.4 Assembling a multi-source stack

Combining commercial banks, an ECA, a DFI and an Islamic tranche in one financing is ordinary in
large infrastructure, and it creates a documentation problem often larger than the financing
problem. Four recurring issues. **Common terms:** one common terms agreement carries the
representations, covenants, events of default and the definition of `CFADS`, each facility
agreement then carrying only its own commercial terms — without it, coverage is tested on
different definitions by different lenders, the defect Domain 10, KA 10.1.1 warns against,
multiplied by the number of tranches. **Intercreditor:** voting thresholds, instructing-group
mechanics, sharing of enforcement proceeds and which decisions require unanimity; a tranche
holding a blocking position it did not pay for is a structural defect visible only in a workout.
**Availability and drawdown order:** tranches whose conditions precedent mature at different times
cannot be drawn pro rata, and whichever is drawn first bears the most construction risk and prices
accordingly. **Currency:** a tranche in a currency other than the revenue currency introduces an
exposure that must be hedged or accepted, and the cheapest tranche is frequently the one that
creates it — a project with local-currency availability payments and a hard-currency ECA tranche
has borrowed a devaluation risk in exchange for a margin saving. The natural hedge, borrowing in
the currency of revenue, is worth paying for and its price is exactly that margin difference
(Domain 3, KA 3.3.3; Domain 11, KA 11.3).

### AI in this KA

**Where it earns its place.** Normalising heterogeneous tranche offers onto a common all-in
basis is arithmetic with many steps and one method, and it is where analysts most often err
under time pressure. Solving effective rates across a dozen structures, and re-solving when a
fee schedule changes, should be automated, with the method (solve from the cash-flow stream)
fixed in the tool rather than left to the user.

**Where it must not go.** It must not opine on Shariah compliance: that belongs to the relevant
supervisory board and to no model. It must not assert current ECA premium rates, maximum tenors
or content thresholds from memory: those come from a periodically revised inter-governmental
framework, and a stale figure presented confidently is worse than no figure. And it must not
draft or interpret intercreditor mechanics, where a misread voting threshold shows up only in a
default.

**Verification, concretely.** For each tranche, confirm the all-in cost was solved from the
stream and that the proceeds figure excludes capitalised premiums and deducted fees. Confirm the
compliance-cost assumption runs for the full tenor. Confirm any framework constraint cited was
checked against the current published text at a stated date by a named person. And keep cost and
capacity in separate columns: a model reporting one number per tranche has already lost the
9.3.3 distinction.

### Key terms — KA 9.3

| Term | Meaning |
|---|---|
| **Istisna'a / ijara / forward lease** | Commissioned-construction, lease, and described-future-asset lease structures used in Islamic finance markets. |
| **Murabaha / wakala / sukuk** | Cost-plus deferred sale; agency investment; certificates of undivided beneficial ownership. |
| **All-in effective cost** | The rate solving `net proceeds = payment × AF(r, n)`; the only valid basis for tranche comparison. |
| **ECA / exposure premium** | State-backed export-credit support; the risk premium it charges, usually capitalised. |
| **Content requirement** | Minimum share of contract value sourced from the supporting country. |
| **DFI conditionality** | Environmental, social, procurement and reporting obligations attached to development finance. |
| **Common terms agreement** | One document carrying shared covenants and definitions across all tranches. |
| **Natural hedge** | Borrowing in the currency of revenue; its price is the margin forgone. |

### Sample MCQs — KA 9.3

**MCQ 9.3-A `[9.3.2 · Application]`** A 40,000,000 tranche carries 4.10 % over 12 years with a 5.5
% exposure premium capitalised into the loan. The all-in cost on the 40,000,000 of proceeds is
closest to:
- A. 4.10 %
- B. 5.0378 % ✅
- C. 4.5583 %
- D. 9.60 %

*Rationale:* Loan `42,200,000`; `AF(0.041, 12) = 9.330854`; instalment `4,522,630.17`; solve
`40,000,000 = 4,522,630.17 × AF(r, 12)` → **5.0378 %**. A ignores the premium entirely; C spreads
the premium straight-line over the tenor and adds it to the rate (`4.10 + 5.5/12`), which
understates because it ignores that the premium is also financed; D adds the premium to the rate
as though it were annual.

**MCQ 9.3-B `[9.3.3 · Analysis]`** A DFI tranche at 5.25 % over 18 years has an all-in economic
cost of 7.2465 % against a commercial market at 6.00 % over 12 years. The soundest conclusion is:
- A. reject the DFI tranche; it is more expensive
- B. accept it; the headline rate is lower
- C. the cost comparison favours the commercial market by about 125 basis points, while the six
  extra years of tenor raise debt capacity at a 1.30× target from 41,171,123 to 56,299,948; the
  decision turns on whether coverage or cost is the binding constraint ✅
- D. the two are equivalent because tenor and rate offset

*Rationale:* Cost and capacity are different effects with different causes (9.3.3). A optimises
cost while ignoring the constraint that actually binds; B is the headline-rate error the worked
example exists to destroy; D asserts an offset without computing either side.

**MCQ 9.3-C `[9.3.1 · Application]`** A conventional tranche of 21,000,000 at 6.00 % over 12 years
carries a 1.20 % fee; an ijara tranche of the same size and tenor carries a 6.15 % profit rate and
a 0.60 % fee. The difference in all-in cost is closest to:
- A. 15 basis points in favour of the conventional tranche
- B. 4 basis points in favour of the conventional tranche ✅
- C. 60 basis points in favour of the ijara tranche
- D. nil, since the structures are economically identical

*Rationale:* All-in 6.2209 % against 6.2604 %, **3.95 basis points** (9.3.1). A is the headline
difference before fees; C confuses the fee saving with a rate advantage; D asserts an
equivalence the arithmetic disproves, small though the gap is.

**MCQ 9.3-D `[9.3.4 · Recall]`** The primary purpose of a common terms agreement in a multi-source
financing is to:
- A. reduce legal fees
- B. ensure every tranche tests coverage on the same definitions and shares one covenant and
  default architecture ✅
- C. give the DFI a veto
- D. permit tranches to be drawn in any order

*Rationale:* One set of shared definitions and covenants prevents the same project being measured
differently by different lenders (9.3.4). A is a by-product; C describes an intercreditor outcome
to be negotiated, not the purpose; D is a drawdown question the agreement constrains rather than
liberates.

**MCQ 9.3-E `[9.3.3 · Evaluation]`** A funding paper describes a 12,000,000 DFI tranche as "5.25 %,
the cheapest money in the structure". Its all-in economic cost, once a 1.00 % front-end fee, 350,000
of advisory spend at close and 120,000 a year of monitoring across an 18-year tenor are counted, is
7.2465 %. The soundest way to report the tranche is:
- A. at 7.2465 % and, in a separate column, the capacity effect (six extra years of tenor
  lifting debt capacity at a 1.30× target from 41,171,123 to 56,299,948) with a statement of
  which compliance costs the project would have incurred in any event ✅
- B. at 5.25 %, which is the contractual rate the facility agreement will carry
- C. as rejected, because 7.2465 % exceeds the commercial market's 6.00 %
- D. as one net figure combining the cost penalty and the capacity benefit

*Rationale:* Cost and capacity are different effects with different causes and belong in separate
columns; charging the whole of an environmental and social management system against the tranche also
overstates its cost, because a well-run project wants the system and other lenders price the comfort
it provides (9.3.3). B repeats the headline the worked example exists to destroy. C optimises cost
while ignoring that coverage is the binding constraint. D collapses two decisions into one number and
conceals which of them is driving it.

**MCQ 9.3-F `[9.3.1 · Comprehension]`** In economic terms, an istisna'a facility differs from an
ijara facility in that:
- A. istisna'a is a lease and ijara a cost-plus sale with deferred payment
- B. istisna'a finances commissioned construction, the financier procuring the asset to specification
  and holding delivery risk during the build, while ijara is a lease of a completed asset whose
  rentals comprise a capital and a return element ✅
- C. they differ only in name, both being loans at interest under another label
- D. istisna'a funds working capital and ijara funds procurement of specific inputs

*Rationale:* The two structures cover the two phases of a project's life, which is why the
istisna'a-to-ijara conversion, commonly through a forward lease, is the standard project shape
(9.3.1). A inverts them. C denies the asset-based mechanics that determine title, security and
the lessor's ownership obligations, which is where economic equivalence with conventional debt
is achieved or lost. D describes murabaha and wakala uses. Whether any structure is compliant
with Shariah is a determination for the relevant supervisory board and is outside this book's
scope.

**MCQ 9.3-G `[9.3.4 · Evaluation]`** A project whose availability payments are denominated in the
host currency is offered two ways to fund the same 15,000,000: a hard-currency export-credit tranche
at a materially lower margin and longer tenor, or a local-currency commercial tranche at the market
margin. The structuring team recommends the export-credit tranche on the strength of its all-in cost
and tenor. The soundest recommendation is:
- A. accept the recommendation: the all-in cost and the capacity effect both favour the
  export-credit tranche, and cost and capacity are the two tests this Knowledge Area sets
- B. accept it and rely on the local cost base as a natural hedge, since local costs fall in
  hard-currency terms as the currency weakens
- C. reject the export-credit tranche: a currency mismatch is unmanageable at any margin
- D. price the exposure before choosing, and on these facts take the local-currency tranche and
  pay the margin, unless the tariff can be exchange-rate-indexed, in which case the
  export-credit tranche is the better answer: borrowing in the currency of revenue is the
  natural hedge, and its price is exactly the margin forgone ✅

*Rationale:* The cheapest tranche is frequently the one that creates the exposure, and a margin
saving bought with an unhedged devaluation risk is not a saving (9.3.4). A applies the right two
tests to the wrong currency and recommends on cost and tenor alone. C reaches the same instruction
as the key for the wrong reason and is the closest rival: it is defensible in a market where long
tenors genuinely cannot be hedged, but it forecloses the indexation remedy — Domain 11 (KA 11.3.2)
shows an exchange-rate-indexed tariff share lifting the tolerable devaluation from 5.06 % to
37.17 %, which makes the tranche usable and makes the choice a priced one rather than a
prohibition. B is the seductive error: local costs do provide a partial offset, and it is far too
small, because debt service and hard-currency operating costs do not devalue at all — on Kestrel's
numbers a **5.06 %** movement breaches the covenant even with the whole local cost base offsetting.

**MCQ 9.3-H `[9.3.2 · Comprehension]`** An export credit agency's exposure premium is described as
"capitalised into the loan". In cash-flow terms that means:
- A. the premium is added to the amount borrowed, so cash proceeds are unchanged while the sum
  repaid rises, which is why the headline rate understates the cost and only the rate solved
  from proceeds against instalments measures it ✅
- B. the premium is deducted from the amount advanced, so proceeds fall and repayments are unchanged
- C. the premium is treated as a capital cost of the project and depreciated
- D. the premium is waived in exchange for a higher margin

*Rationale:* Capitalisation grosses the loan up: 15,000,000 of eligible equipment financed with
a 6.0 % premium becomes a 15,900,000 loan against 15,000,000 of proceeds, and the all-in cost is
**4.6895 %** against a 3.80 % headline, **88.95 basis points** the ranking would otherwise miss
(9.3.2). B describes a deducted arrangement fee, which reduces proceeds instead. C is an
accounting treatment, not a cash-flow mechanic, and it does not change what is repaid. D
describes a different bargain altogether.

### Self-check — KA 9.3

1. *How is a tranche's cost compared, and why not by rate plus fees?* By solving the effective
   rate from its own cash-flow stream; fees, capitalised premiums and tenor interact and cannot
   be added.
2. *State the two distinct effects of a longer-tenor tranche.* — A cost effect (usually adverse
   once fees and conditionality are counted) and a capacity effect (36.7 % more debt at the same
   coverage, in the 9.3.3 case) — reported separately.
3. *What does this book decline to determine about Islamic finance structures?* Their compliance
   with Shariah, which rests with the relevant supervisory board; the book describes only
   economics and cash flows.

---

## Knowledge Area 9.4 — Government support, grants, sustainable finance and refinancing

*Topics: 9.4.1 forms of government support · 9.4.2 grants and concessional tranches · 9.4.3 green
and sustainability-linked finance · 9.4.4 refinancing.*

### 9.4.1 Forms of government support

**Definition.** Government support is any intervention that improves a project's financeability
without the grantor becoming an ordinary equity investor. Every form transfers value, and every
form has a fiscal price, either cash now or a contingent liability later, which is why the
discipline of this topic is to name the price alongside the benefit.

| Form | What it does for the project | What it costs the grantor |
|---|---|---|
| **Capital grant / viability gap funding** | Reduces the amount to be financed | Cash, at the point of greatest budget pressure |
| **Availability or capacity payment** | Removes demand risk from the revenue line (Domain 7) | A long-dated committed expenditure line |
| **Minimum revenue guarantee** | Caps downside on a demand-based revenue | A contingent liability, valued as an option |
| **Sovereign or agency guarantee of debt** | Converts project credit into sovereign credit | Full contingent exposure to the debt |
| **Concessional or subordinated public loan** | Fills the gap between senior debt and equity cheaply | The margin forgone, plus subordination risk |
| **Tax incentives, allowances, exemptions** | Raises `CFADS` by reducing cash tax | Forgone revenue, often unbudgeted and unmeasured |
| **Land, permits, connections in kind** | Reduces development cost and timeline | Opportunity cost of the asset, rarely quantified |

Two professional points cut across the table. **A guarantee is not free because no cash moves**.
It is a written option whose value can be estimated, and a grantor that does not estimate it is
accumulating unmeasured liabilities. And **support that improves coverage is worth more to a
project than support of equal value that improves return**, because coverage is the binding
constraint (9.1.4). A grantor seeking maximum financeability per unit of fiscal cost should
therefore direct support at the coverage face, which is exactly what 9.4.2 quantifies.

**The third point, and it belongs on the funding plan rather than on the grantor's side of it.**
Every form in the table raises a question the funding plan cannot answer for itself: **has the
support been properly granted?** Many jurisdictions are understood to operate régimes governing
whether and how public support may be given (variously described as subsidy control or State
aid), and there are international disciplines on subsidies operating between states; grants,
concessional loans, guarantees, tax measures and support in kind can each fall within them. The
professional reason this belongs in a chapter about *funding plans* is the shape of the
consequence. Where such a régime applies and support has not been properly granted, the
consequence is characteristically described as **recovery from the beneficiary, with interest**,
and the beneficiary is the project company, not the grantor. That turns a public-law question
into a hole in the funding plan, sitting on the exact line the reader has just been taught to
build, and one that opens after financial close when the money has been spent.

What the professional owes is therefore concrete and is not an opinion of their own. **A written
legal confirmation that the support is lawfully granted** (or that any required notification,
clearance or approval has been made and obtained) is obtained from qualified counsel in the
relevant jurisdiction **before financial close**; it is listed on the condition-precedent
schedule as a third-party condition with a named owner and a date (Domain 13, KA 13.3.1); and it
is retained with the closing set for as long as recovery can be pursued, which is longer than
most people assume. Lenders will ask for it in diligence in any event, and a funding plan that
shows the support without showing the confirmation has recorded the benefit and omitted its
condition.

Two cautions of the kind this book always attaches. The applicable régime, its thresholds, what
falls inside it, what procedure applies and what the consequences of a defect are, differ by
jurisdiction and change over time, nothing here states the position anywhere, and nothing here
characterises any support as lawful or unlawful. And this is not a matter on which a financial
adviser, a model or a sponsor's own view is worth anything: it is a legal question with a legal
answer, obtained in writing, from counsel who will be relied on.

### 9.4.2 Grants and concessional tranches

**Worked example 9.4.2 — one grant, two structures, two entirely different projects.**

1. **Setup.** A **6,000,000** capital grant (10 % of capex) is awarded to Kestrel. The funding
   plan must place it. **Case G1:** the grant **displaces equity**, senior stays at 42,000,000,
   equity falls to 12,000,000. **Case G2:** the grant **displaces senior debt**, senior falls to
   36,000,000, equity stays at 18,000,000. In both, the grant is treated as zero-cost capital
   that absorbs risk alongside equity, so `β_e` is re-levered on debt against equity **plus
   grant**.
2. **Formula.** `WACC = Σ(share × cost)` with the grant at zero cost; `DSCR` per Domain 10; equity
   `IRR` from the residual stream.
3. **Substitution.** G1: `WACC = (42 × 4.80 + 12 × 15.42 + 6 × 0)/60`; `DSCR` unchanged; equity
   stream `(−12,000,000; 1,374,364.77 × 12; 6,384,000 × 13)`. G2: senior service `36,000,000 /
   8.383844 = 4,293,973.06`; `β_e = 0.60 × (1 + 0.80 × 36/24) = 1.32` so `k_e = 13.02 %`; `WACC =
   (36 × 4.80 + 18 × 13.02)/60`; equity stream `(−18,000,000; 2,090,026.94 × 12; 6,384,000 × 13)`.
4. **Result.**

| | Base (no grant) | G1 — grant displaces equity | G2 — grant displaces debt |
|---|---|---|---|
| Senior debt | 42,000,000 | 42,000,000 | 36,000,000 |
| Equity | 18,000,000 | 12,000,000 | 18,000,000 |
| `k_e` | 15.42 % | 15.42 % | 13.02 % |
| **`WACC`** | **7.9860 %** | **6.4440 %** | **6.7860 %** |
| **`DSCR`** | **1.2743** | **1.2743** | **1.4867** |
| **Equity `IRR`** | **12.5311 %** | **16.8231 %** | **14.9940 %** |

5. **Interpretation.** The same 6,000,000 produces two different projects, and the difference is
   the allocation question every grant negotiation is really about. **G1 is the sponsors'
   case:** equity return jumps **429.20 basis points**, from 12.5311 % to 16.8231 % — clearing
   the 15.42 % cost of equity for the first time on the flat-`CFADS` basis, without a single
   change to the project — while lenders gain **nothing**, because `DSCR` is untouched at
   1.2743. **G2 is the lenders' case:** coverage rises **21.24 points** to 1.4867, comfortably
   above the 1.30× sizing target that Domain 10's Case A could not reach, while equity gains
   246.29 basis points rather than 429.20. A grantor who does not specify which face the grant
   is intended to strengthen has funded whichever the sponsors preferred, and the answer will be
   G1. The tie-break should follow the public purpose: if the objective is **financeability**,
   getting a project financed that otherwise could not be, the grant must reduce debt (G2),
   because coverage is the binding constraint; if the objective is **attracting private capital
   to a thin sector**, raising equity returns (G1) is defensible, and should be said out loud.
   **The treatment caution is as large as the allocation question.** The re-levering convention
   above places the grant in the risk-bearing base with equity. Treat it instead as *outside*
   that base (so `D/E` in G1 becomes 42/12 = 3.5, `β_e` = 2.28 and `k_e` = **18.78 %**), and
   G1's `WACC` rises from 6.4440 % to **7.1160 %**, a difference of **67.20 basis points** on
   identical cash flows. Both treatments are defensible; only one can be used; and it must be
   documented before the numbers are quoted, because 67 basis points is more than the entire
   `WACC` effect of twenty points of gearing.

**Concessional tranches and the grant element.** Support often arrives as cheap debt rather than
cash, and its subsidy content can be measured precisely. Take **12,000,000 at 2.0 % over 25 years
with 7 years' grace** (interest-only, then level amortisation), against a **6.0 %** market rate.
Interest during grace is `12,000,000 × 2.0 % =` **240,000** a year; the amortising instalment is
`12,000,000 / AF(0.02, 18) = 12,000,000 / 14.992031 =` **800,425.23**. Discounting the whole
service stream at 6.0 % gives a present value of **7,103,613**, so the **grant element** is `1 −
7,103,613/12,000,000 =` **40.80 %**, and the subsidy is worth **4,896,387** in present-value
terms. A concessional tranche of 12,000,000 is therefore 7.1 million of debt and 4.9 million of
grant, and reporting it as 12 million of borrowing misstates both the leverage and the support
received.

### 9.4.3 Green and sustainability-linked finance

**Two mechanisms, routinely confused.** A **use-of-proceeds** instrument, a green bond or green
loan, commits the borrower to spend the money on defined eligible assets, with reporting and
usually an external review; the pricing benefit, where it exists, comes from demand. A
**sustainability-linked** instrument leaves the use of proceeds unrestricted and instead ties
the **margin** to performance against key performance indicators through a ratchet, so the
borrower pays less for hitting targets and more for missing them. The first constrains what the
money does; the second prices what the borrower does.

Both rest on voluntary market frameworks (the principles published by international market
associations for green bonds and green loans, and their sustainability-linked counterparts)
together with jurisdiction-specific taxonomies and disclosure regimes that differ materially
between jurisdictions and change frequently. None of them is a global standard, and treating one
jurisdiction's taxonomy as universal is the characteristic error in this area.

**Worked example 9.4.3 — does the ratchet pay for itself?**

1. **Setup.** Kestrel's 42,000,000 senior facility is offered as a sustainability-linked loan
   with a **±15 basis point** margin ratchet tied to two KPIs (specific energy consumption per
   cubic metre produced, and a verified brine-discharge metric). Meeting both targets earns the
   full 15 basis point reduction. The apparatus required (metering, data assurance, annual
   limited-assurance verification, a second-party opinion refreshed periodically, and reporting)
   costs **85,000** a year. Assess over the 12-year facility at 6 %.
2. **Formula.** Annual benefit = tranche × ratchet. Compare present values using `AF(0.06, 12) =
   8.383844`.
3. **Substitution.** `42,000,000 × 0.15 % = 63,000` a year against 85,000 a year; `63,000 ×
   8.383844` against `85,000 × 8.383844`.
4. **Result.** Present value of the margin saving **528,182**; present value of the compliance
   cost **712,627**; **net −184,445**. On margin alone the ratchet **destroys value**, even
   assuming both KPIs are met every year. The breakeven is a ratchet of **20.24 basis points**, or
   equivalently a base-margin reduction of **5.24 basis points** alongside the 15.
5. **Interpretation.** This is the number the market conversation usually omits, and stating it
   is not scepticism about sustainability. It is honesty about where the value comes from. A
   ratchet of 15 basis points on a facility of this size cannot pay for a credible verification
   apparatus, so **if the case for the label rests on the ratchet, there is no case**. The case
   rests elsewhere, and it is quantifiable: if the label genuinely widens the lender pool enough
   to cut the **base** margin by **10 basis points** (42,000 a year, **352,121** in present
   value) the combined position turns **positive by 167,677**. That reframes the negotiation
   correctly: ask the arranger what the label does to the **base** margin and to the size of the
   club, not what the ratchet is worth. Three further cautions. The ratchet is **symmetric** in
   most drafting, so missing targets *increases* the margin, and a KPI set without headroom
   converts an ESG initiative into a cost. Targets must be **material and measurable**: a KPI
   the project would have met anyway invites the accusation of greenwashing, which is a
   reputational and, increasingly, a regulatory exposure. And **the verification cost is
   permanent** for the life of the facility, while the ratchet benefit is contingent on
   performance; budget them asymmetrically.

### 9.4.4 Refinancing

**Definition.** Refinancing replaces existing debt with new debt on better terms, and in project
finance the opportunity is structural rather than opportunistic: construction risk, ramp-up risk
and counterparty uncertainty all resolve in the first operating years, so the project that emerges
is a materially better credit than the one that was financed. Better terms mean a lower margin, a
longer tenor, a looser covenant package, or all three.

**Worked example 9.4.4 — Kestrel's refinancing gain, decomposed.**

1. **Setup.** At the end of **year 6**, six of twelve instalments of 5,009,635.23 have been
   paid. The market offers **4.75 %**. Transaction costs (arrangement on the new facility plus a
   prepayment fee on the old) total **1.75 %** of the refinanced balance. Two structures are
   available: refinance the remaining **6 years**, or refinance over **12 years**, extending
   maturity from year 12 to year 18 within the 25-year operating life.
2. **Formula.** Outstanding balance = instalment × `AF(r_old, remaining)`; new instalment =
   balance ÷ `AF(r_new, new tenor)`; the equity gain is the present value of the change in the
   equity cash-flow stream, discounted at the cost of equity.
3. **Substitution.** Balance `5,009,635.23 × AF(0.06, 6) = 5,009,635.23 × 4.917324`. Rate-only:
   `balance / AF(0.0475, 6)`. Extended: `balance / AF(0.0475, 12) = balance / 8.989557`.
   Transaction cost `balance × 1.75 %`.
4. **Result.** Outstanding balance **24,634,001.18** (principal repaid to date **17,365,998.82**).
   Transaction cost **431,095.02**.

| | Rate only, 6 years | Rate + extension, 12 years |
|---|---|---|
| New instalment | **4,814,595.21** | **2,740,290.88** |
| Annual saving, years 7–12 | **195,040.02** | **2,269,344.35** |
| New `DSCR` | **1.3260** | **2.3297** |
| New `LLCR` | 1.3260 | **2.3297** |
| Tail (project life beyond maturity) | 13 years | **7 years** |
| PV of equity gain at `k_e` 15.42 % | **298,757** | **3,723,616** |
| PV of equity gain at 8.00 % | **470,552** | **2,076,800** |
| PV of equity gain at 4.75 % | — | **566,832** |

5. **Interpretation.** The extension appears to create **3.72 million** of equity value against
   299 thousand from the rate reduction alone, and the decomposition shows why that comparison
   is misleading: of the 3.72 million, only **298,757** is the rate saving and **3,424,859** is
   the **extension**, which is not a gain at all but a **deferral** (six years of debt service
   moved from years 7–12 into years 13–18). Its apparent value is almost entirely an artefact of
   the discount rate: at 15.42 % the deferral is worth 3.42 million, at 8.00 % it is worth 1.61
   million, and at the new loan rate of 4.75 % the whole package is worth only **566,832**. **A
   refinancing gain that shrinks by 85 % when the discount rate falls to the loan rate is a
   financing artefact, not value creation**, and a leader who presents the 3.72 million without
   the sensitivity has misled the board. What is unambiguously real: the rate saving of 195,040
   a year; the coverage improvement to **2.3297**, which creates genuine covenant headroom; and
   the reduction of the **tail from 13 years to 7**, which is a genuine loss — `PLCR` at the
   refinancing date rises from 2.8917 to 3.1968 only because the new rate discounts more gently,
   while the *contractual* cushion behind the lenders has almost halved. Two further
   disciplines. **Costs are real and immediate:** 431,095 is paid at once against a gain earned
   over years, and at the rate-only structure the costs consume 59 % of the discounted benefit
   at 15.42 %. **Refinancing gain-share is common in concession-based structures**, requiring
   the grantor's consent and a defined share of the gain: at 50 % of the 15.42 % figure,
   **1,861,808** returns to the grantor, which changes the economics enough that the clause must
   be read before the refinancing is modelled, not after (Domain 12, KA 12.2).

### AI in this KA

**Where it earns its place.** Refinancing screening across a portfolio (outstanding balances,
candidate rates and tenors, opportunities ranked by gain net of costs) is repetitive arithmetic
on data the organisation already holds, and it surfaces what manual review misses. Grant-element
and sustainability-linked breakeven calculations are similarly mechanical.

**Where it must not go.** It must not conclude what a grant, subsidy or state guarantee means
legally or fiscally: state-aid and subsidy-control regimes, the accounting treatment of grants
and the tax character of concessional support are jurisdiction-specific and consequential. It
must not generate sustainability claims or KPI narratives: statements about environmental
performance carry regulatory and reputational exposure, and model-written text published as a
project's own assertion is the shortest route to a greenwashing allegation. And it must not
settle a refinancing recommendation, which turns on the discount-rate judgment 9.4.4 shows
dominates the answer.

**Verification, concretely.** Recompute the outstanding balance independently: instalment ×
`AF(r, remaining)` must equal the schedule's closing balance (Domain 3's check). Confirm every
gain is reported at two or more discount rates, one of them the new loan rate, and net of
transaction costs and any gain-share. Confirm the grant treatment used in a `WACC` is stated.
And confirm a sustainability-linked benefit is compared against the **full-tenor** verification
cost, not the first year's.

### Key terms — KA 9.4

| Term | Meaning |
|---|---|
| **Viability gap funding** | Capital grant sized to make an otherwise unfinanceable project viable. |
| **Minimum revenue guarantee** | Grantor-provided floor under demand-based revenue; a written option. |
| **Grant element** | `1 − PV(debt service at a market rate) ÷ face value`; the subsidy content of concessional debt. |
| **Use-of-proceeds instrument** | Green bond or loan; proceeds restricted to eligible assets. |
| **Sustainability-linked loan** | Margin ratchet tied to KPI performance; usually symmetric. |
| **Refinancing gain** | PV of improved terms, net of costs; decomposed into rate and extension components. |
| **Gain-share** | Contractual obligation to pass a defined share of a refinancing gain to the grantor. |
| **Tail** | Project life beyond debt maturity; reduced by tenor extension. |
| **Subsidy-control régime** | The régime, where one applies, governing whether public support may be given; its existence, scope and consequences are jurisdiction-specific and are a question for counsel. |
| **Confirmation of lawful grant** | The written legal confirmation, obtained before close, that support in the funding plan has been properly granted; a third-party condition precedent with a named owner, retained with the closing set. |
| **Recovery exposure (support)** | The funding-plan risk that improperly granted support is recovered from the project company with interest — a hole in the plan rather than a grantor problem. |

### Sample MCQs — KA 9.4

**MCQ 9.4-A `[9.4.2 · Analysis]`** A 6,000,000 grant applied to reduce **equity** leaves `DSCR` at
1.2743 and lifts equity `IRR` from 12.5311 % to 16.8231 %; applied to reduce **debt** it lifts
`DSCR` to 1.4867 and equity `IRR` to 14.9940 %. If the grantor's stated objective is to make an
otherwise unfinanceable project financeable, it should:
- A. apply the grant to reduce equity, maximising private-sector return
- B. apply the grant to reduce debt, because coverage is the binding constraint on financeability
  and 1.4867 clears the 1.30× sizing target ✅
- C. split the grant evenly, as a neutral position
- D. either; the `WACC` reduction is what matters

*Rationale:* Financeability is a coverage question (9.1.4, 9.4.2). A serves a different
objective and should be stated as such; C is a decision avoided rather than made; D optimises
the number that this domain shows is not binding, and `WACC` actually falls *further* under the
equity-displacing case (6.4440 % against 6.7860 %), which is exactly why `WACC` is the wrong
test here.

**MCQ 9.4-B `[9.4.3 · Application]`** A 42,000,000 facility offers a 15 basis point sustainability
ratchet; the verification and reporting apparatus costs 85,000 a year; the facility runs 12 years
and `AF(0.06, 12) = 8.383844`. On margin alone the arrangement is:
- A. value-positive by 528,182
- B. value-negative by 184,445 ✅
- C. value-neutral
- D. value-positive by 63,000 a year

*Rationale:* `63,000 × 8.383844 = 528,182` of benefit against `85,000 × 8.383844 = 712,627` of
cost → **−184,445**. A counts the benefit and omits the cost; D quotes the annual benefit gross,
also omitting cost; C asserts an offset that the arithmetic contradicts.

**MCQ 9.4-C `[9.4.4 · Analysis]`** A refinancing that extends maturity shows an equity gain of
3,723,616 at a 15.42 % cost of equity, 2,076,800 at 8.00 % and 566,832 at the new 4.75 % loan
rate. The correct board disclosure is:
- A. a gain of 3,723,616
- B. a gain of 3,723,616, since the cost of equity is the correct discount rate for equity cash
  flows
- C. that most of the reported gain is deferral rather than saving (the rate component is
  298,757 and the extension component 3,424,859 at 15.42 %), and that the package is worth only
  566,832 discounted at the loan rate ✅
- D. no gain, since the total cash paid over the loan's life increases

*Rationale:* The decomposition and the rate sensitivity are the disclosure (9.4.4). A and B report
a correct arithmetic result while concealing that it is an artefact of the discount rate; D swings
to the opposite error, ignoring the genuine 195,040 a year of rate saving and the coverage
improvement.

**MCQ 9.4-D `[9.4.2 · Application]`** A 12,000,000 concessional loan at 2.0 % over 25 years with 7
years' grace has a debt-service stream worth 7,103,613 at a 6.0 % market rate. Its grant element
is:
- A. 40.80 % ✅
- B. 4.00 %
- C. 59.20 %
- D. nil; it is a loan, not a grant

*Rationale:* `1 − 7,103,613/12,000,000 = 40.80 %`, a subsidy worth 4,896,387 (9.4.2). B quotes
the interest-rate difference as though it were the subsidy; C is the complement (the debt
content, not the grant content; D confuses legal form with economic substance).

**MCQ 9.4-E `[9.4.3 · Evaluation]`** A 15 basis point sustainability ratchet on a 42,000,000
facility is worth 528,182 in present value against 712,627 of verification and reporting cost
(net **−184,445**) even assuming both key performance indicators are met every year. The
soundest professional response is:
- A. take the label: 15 basis points is a saving, and the reporting would be done anyway
- B. reject sustainability-linked structures generally, since the ratchet is value-negative
- C. ask the arranger what the label does to the **base** margin and to the size of the club,
  because a 10 basis point base-margin reduction turns the combined position positive by
  167,677, and if the case rests on the ratchet alone there is no case ✅
- D. accept the ratchet and set key performance indicators the project would meet in any event, so
  that the reduction is certain

*Rationale:* A ratchet of this size cannot pay for a credible verification apparatus, so the
negotiation belongs on the base margin and the lender pool, where the value actually is (9.4.3). A
weighs a contingent benefit against a permanent cost and omits the cost. B generalises one
arithmetic result into a policy and forgoes a benefit the same arithmetic shows is available. D is
the greenwashing failure in one sentence: a target the project would have met is not a target, it
carries reputational and increasingly regulatory exposure, and because the ratchet is symmetric in
most drafting a KPI set chosen without headroom converts the initiative into a cost.

**MCQ 9.4-F `[9.4.2 · Comprehension]`** A finance ministry official describes a concessional loan as
"part loan and part gift". Expressed in this domain's terms, that means:
- A. part of the principal will be forgiven at maturity and the remainder repaid
- B. the loan's subsidy content is the shortfall between its face value and the present value of
  its debt service discounted at a market rate, 40.80 % of a 12,000,000 tranche at 2.0 %, so
  about 7.1 million of borrowing and 4.9 million of support, with every dollar repaid ✅
- C. the gift is the total interest saved over the loan's life relative to a market rate, before
  discounting
- D. the description is loose talk: a loan repayable in full contains no grant

*Rationale:* The grant element is `1 − PV(debt service at a market rate) ÷ face value`, which
measures subsidy in present-value terms without any principal being written off (9.4.2). A
describes forgiveness, a different instrument. C omits discounting, which is where a seven-year
grace period does most of its work. D confuses legal form with economic substance and would
report 12,000,000 of borrowing where 7.1 million exists (misstating both the leverage and the
support received).

**MCQ 9.4-G `[9.4.1 · Comprehension]`** A grantor with a fixed fiscal envelope can direct its support at
a project either as a capital grant reducing the amount to be financed or as a minimum revenue
guarantee capping the downside on a demand-based revenue line. Its stated objective is to get an
otherwise unfinanceable project financed at the least fiscal cost. Restated in the terms 9.4.1
uses, the two professional points that decide the advice are that:
- A. no cash leaves the budget when a guarantee is given, so its fiscal cost is nil
- B. the choice belongs to the sponsors, since they are the party that must raise the financing
- C. a grant is always cheaper for a grantor than a contingent commitment
- D. support directed at the **coverage** face buys more financeability per unit of fiscal cost than
  support of equal value directed at the return face; and a guarantee is a written option whose
  value can be estimated, so an unvalued guarantee is an unmeasured liability rather than a free
  one ✅

*Rationale:* Those are 9.4.1's two cross-cutting points, and both are needed here: coverage is
the binding constraint, so support aimed at the coverage face converts fiscal cost into
financeability most efficiently (9.1.4); and a guarantee is a written option whose value can be
estimated, so "no cash moves" is a budgeting statement and not a cost statement. A is the error
the topic exists to correct. A contingent liability is unbudgeted, not costless. C reverses it
into a universal ranking: a guarantee that is never called costs nothing, and its expected cost
is what has to be compared with the grant's certain one. B delegates a public-purpose judgement
to the party whose interest lies in the return face: on the grant arithmetic of 9.4.2, the
answer will be the structure that lifts equity `IRR` by 429.20 basis points and leaves `DSCR`
untouched.

### Self-check — KA 9.4

1. *A grant of fixed size: which face should it strengthen and why?* The coverage face, if the
   objective is financeability, because coverage binds; the return face only if attracting
   equity to a thin sector is the stated objective.
2. *When does a sustainability-linked ratchet pay for itself?* — Only above about 20 basis points
   on a facility of this size, or when the label also reduces the base margin — 5.24 basis points
   is the breakeven addition here.
3. *How must a refinancing gain be reported?* Net of costs and gain-share, decomposed into rate
   and extension components, at no fewer than two discount rates including the new loan rate.
4. *Why is the lawfulness of government support a funding-plan question rather than a grantor
   question?* Because where support has not been properly granted the characteristic remedy is
   recovery from the beneficiary with interest, and the beneficiary is the project company. The
   exposure sits on the funding plan, after close, when the money has been spent.
5. *What evidence must exist, and when?* A written confirmation from counsel in the relevant
   jurisdiction that the support is lawfully granted, or that any required notification or
   approval has been made and obtained; before financial close; on the condition-precedent
   schedule as a third-party condition with a named owner; retained with the closing set. The
   applicable régime and its consequences are jurisdiction-specific and are never assumed from
   another country or an earlier deal.

---

## Advanced topics — Domain 9

### 9.A.1 Why capital-structure irrelevance fails in a project SPV

The classical result (that in frictionless markets the value of a firm is independent of how it
is financed) is a useful diagnostic precisely because every one of its assumptions fails in a
project financing, and naming the failures tells a leader where the value actually sits.
**Taxes** are the first: the deductibility of interest supplies 0.72 of Kestrel's 1.02 `WACC`
slope, so most of the apparent benefit of leverage is a transfer from the tax authority,
contingent on a jurisdictional rule that can change. **Bankruptcy and distress costs** are the
second, and in a single-asset SPV they are severe: there is no diversified balance sheet to
absorb a bad year, which is why lenders demand coverage rather than accepting the theory's
indifference. **Contracting frictions** are the third (covenants, lock-up, reserve accounts,
cure limits), and they impose a hard boundary on the feasible set that no cost calculation can
cross (9.1.4). The fourth is a modelling artefact worth confronting: the build-up of 9.1.3
re-levers only the beta term, leaving `r_f`, `CRP` and `SP` un-levered, which is what makes
`WACC` linear in gearing and gives the slope its value. Re-lever the country and single-asset
premiums as well and the slope changes; leave the beta un-levered and `WACC` rises with gearing.
The convention is therefore not innocent, and the honest presentation states it, applies it
uniformly, and reports how much of the conclusion depends on it.

### 9.A.2 Concessionality, blending, and measuring what the public sector gave

The grant-element calculation of 9.4.2 (**40.80 %** on the illustrative 12,000,000 tranche)
generalises into the discipline of **blended finance**: combining concessional public capital
with commercial capital so that a project unfinanceable on market terms becomes financeable.
Three disciplines make it defensible. **Measure the subsidy**, using the grant element at an
explicit market comparator rate, and report it as subsidy rather than as debt; a portfolio
described as "5 billion of development lending" with an unstated 30 % grant element has
misreported both its leverage and its fiscal cost. **Test additionality**: concessional capital
that displaces commercial capital that would have come anyway has funded a transfer, not a
project. And **price the subordination separately**: public capital that ranks behind commercial
debt is providing first-loss protection whose value is not captured by the interest-rate
difference at all, and which should be valued as the credit enhancement it is.

### 9.A.3 The reviewer's funding-structure eye

Invariants to test on any funding structure or cost-of-capital model:

- Sources equal uses, at every drawdown date and in total (Domain 14's discipline, applied at
  close).
- `WACC` weights sum to one, and the debt weight uses the same debt figure as the coverage test.
- Every debt-like tranche is tax-adjusted and equity is not; each adjustment traces to a written
  opinion.
- `k_e` is re-levered for each candidate structure; no two structures share a cost of equity
  unless they share a gearing.
- Under the re-levering convention of 9.1.3, `WACC` is **linear in gearing**; a non-linear ladder
  contains an error.
- The `WACC` at the coverage-binding gearing is reported alongside the `WACC` at the proposed
  gearing (**8.0001 %** and **7.9860 %** for Kestrel); the gap is the price of bankability.
- Every quoted equity `IRR` names its cash-flow case (bank case or sponsor case) and its treatment
  of any equity bridge.
- An equity bridge priced at exactly the equity `IRR` it defers produces **zero** uplift; a model
  reporting a gain there has a timing defect.
- Every tranche's cost is an **all-in effective rate solved from its own stream**, with
  capitalised premiums added to the loan and deducted fees excluded from proceeds.
- Compliance and monitoring costs run for the tranche's full tenor.
- The grant treatment used in `WACC` is stated; the alternative treatment's effect is disclosed
  (**67.20 basis points** for Kestrel).
- Every refinancing gain is net of costs and gain-share, decomposed, and reported at two or more
  discount rates.
- Cost effects and capacity effects are reported in separate columns and never netted.

---

## Industry variations — Domain 9

- **Contracted power and renewables.** Availability or fixed-price offtake supports high gearing
  and a deep pool of infrastructure debt funds; the distinctive feature is a large,
  jurisdiction-specific incentive layer (allowances, credits, feed-in mechanisms) whose value is
  often monetised through a specialist tranche with its own investor base and its own tax opinion.
  Green-labelled instruments are near-universal, so the label carries little pricing benefit and
  the 9.4.3 analysis matters more, not less.
- **Merchant power and commodities.** Coverage requirements of 1.5× and above compress feasible
  gearing, so structures lean on mezzanine and on private credit funds that will price risk banks
  will not; reserve-based and borrowing-base facilities re-size against periodic technical
  redeterminations, making debt capacity a variable rather than a term.
- **Transport concessions.** Ramp-up risk pushes structures toward long-tenor institutional
  debt, substantial capital grants or viability gap funding, and staged refinancing after
  patronage stabilises, which is why refinancing gain-share clauses (9.4.4) are most common in
  this sector and why the grantor's consent regime is a financing term, not a legal footnote.
- **Water and regulated utilities.** Regulatory reset cycles cap the tenor over which revenue is
  contractually visible, so debt maturities are often set to reset dates and refinancing is
  structural. Public grant support is common and the G1/G2 allocation question of 9.4.2 is a
  standing negotiation with the regulator as well as the grantor.
- **Digital infrastructure.** Shorter asset lives compress tails, so lenders rely on contracted
  tenant credit rather than on `PLCR`; capital arrives fast and in size from private credit and
  from vendor financing, and gearing is constrained by lease-term coverage rather than by
  concession length.
- **Social infrastructure PPPs.** Availability payments from a public counterparty support the
  thinnest equity in the market (gearing of 90 % is ordinary), which makes the leverage exchange
  rate of 9.1.4 extreme: tiny coverage movements dominate the equity return, and a lock-up is
  catastrophic rather than inconvenient.

---

## Case study — Domain 9: three ways to close 828,877 (water)

**Situation.** Domain 10's Case A left Kestrel with an arithmetic gap. Against base-case `CFADS`
of 6,384,000 and a 1.30× requirement, sustainable senior debt service is **4,910,769**
supporting **41,171,123** (**828,877** short of the 42,000,000 requested). The bank would not
move on coverage. The sponsors, whose investment committee had approved 18,000,000 of equity and
no more, asked their adviser for options.

**Option one: more equity.** Senior 41,000,000 (debt service `41,000,000/8.383844 =`
**4,890,358**, `DSCR` **1.3054**) with equity of **19,000,000**. Distributions run at
**1,493,642** a year, the equity `IRR` on the flat-`CFADS` basis is **12.3868 %**, and the
structure's `WACC` is **8.0030 %**. This is Domain 10's answer, and it works.

**Option two: mezzanine at SPV level.** Senior 41,000,000, mezzanine 4,000,000 at 11.50 % over 15
years (debt service `4,000,000/6.996708 =` **571,697**), equity 15,000,000. The bank rejected it
in one line: total debt service becomes 5,462,056 and the project's `DSCR` falls to **1.1688**,
below both the covenant and the lock-up. Mezzanine inside the borrower consumes the `CFADS` the
covenant measures (9.2.2), and no amount of subordination language changes the arithmetic.

**Option three: mezzanine at HoldCo level.** The same 4,000,000 raised by the sponsors' holding
company, serviced from **distributions**. The SPV's `DSCR` stays at **1.3054** because the SPV
has no additional obligation; the mezzanine is covered `1,493,642/571,697 =` **2.6126** times
out of distributions; and the sponsors' equity falls to 15,000,000 with an `IRR` of **12.4934
%** (10.66 basis points better than option one, on 4,000,000 less cash committed). The bank
consented, subject to the mezzanine having no recourse to the SPV and no security over its
shares.

**How it resolved, and what nearly went wrong.** The sponsors took option three. Six months into
operations the adviser ran the stress that should have been run first. The distribution lock-up
triggers when `CFADS` falls below `4,890,358.20 × 1.15 =` **5,623,912**, which is **11.9061 %**
below base case. Immediately above that line the mezzanine still looks serviceable: at an **11.5
%** shortfall `CFADS` is **5,649,840**, the SPV's `DSCR` is **1.1553** (a covenant breach, but
payment made in full), distributions are **759,482** and HoldCo cover is **1.3285**, yet that
case sits only **25,928** above the trigger. Four-tenths of a percentage point further down (a
**12 %** shortfall, `CFADS` **5,617,920**, a further **31,920** of cash gone) the lock-up trips,
the SPV retains cash it would otherwise have distributed, and **HoldCo coverage goes from 1.3285
to zero in a single step** while the senior lenders are paid on time. The mezzanine's 2.6126×
cover in the base case was therefore worth almost nothing in the one scenario that mattered: it
stood 2.6 times covered against a cliff **11.9 %** away, with no gradual deterioration to warn
anyone. The sponsors renegotiated a 12-month HoldCo interest reserve of **571,697** and a
standstill on HoldCo enforcement while an SPV lock-up subsisted, and the structure held.

**What the domain teaches here.** Mezzanine's placement, not its price, is its defining
characteristic. HoldCo mezzanine is invisible to the SPV's coverage test and *entirely*
dependent on the distribution it sits behind, so its true coverage is not the base-case multiple
but the distance between base case and the **lock-up trigger** (for Kestrel, **11.9061 %**). Any
junior instrument serviced from distributions must be stress-tested against the lock-up rather
than the covenant, because the exposure is a cliff and not a slope: the number that matters is
the shortfall at which the cash stops, not the multiple by which it is covered today.

## Case study B — Domain 9: the cheap money that bought expensive rolling stock (transport)

**Situation.** A metropolitan rail authority's concessionaire needed a **120,000,000**
rolling-stock and signalling package. Supplier N, in a country with an active export credit
agency, quoted **120,000,000**. Supplier S quoted **111,111,111** (**8.0 %** less, a difference
of **8,888,889**), but with no ECA support available. The ECA offered **85 %** of the contract
value at **3.90 %** over **14 years**, with an exposure premium of **6.5 %** capitalised. The
commercial alternative for Supplier S was 85 % at **6.30 %** over **12 years** with a 1.10 %
fee. The concessionaire's finance team recommended Supplier N: "the money is 240 basis points
cheaper."

**The arithmetic done properly.** All-in costs first. Route N: ECA loan `120,000,000 × 85 % =`
**102,000,000**, premium **6,630,000**, financed loan **108,630,000**, instalment
`108,630,000/AF(0.039,14) = 108,630,000/10.633202 =` **10,216,114**; solving on the 102,000,000
of proceeds gives an all-in of **4.8656 %** (a full point above the 3.90 % headline). Route S:
commercial loan **94,444,444**, fee **1,038,889**, proceeds **93,405,556**, instalment
`94,444,444/8.247657 =` **11,451,063**, all-in **6.5041 %**. So the money really is **164 basis
points** cheaper, not 240.

But the money and the equipment must be decided together. Discounting each route's whole cash
profile, the own-funds outflow at close plus the debt service, at the project's 8.0 % cost of
capital: Route N costs **102,224,065** in present value against Route S's **104,001,662**.
**Route N wins by 1,777,597** on a 120,000,000 package, **1.5 %**, and the finance team's
recommendation was right, for reasons it had not calculated.

**How it resolved.** The team then computed the number that should have opened the analysis: the
**breakeven equipment premium**, the price differential at which the two routes are indifferent.
At an 8.0 % discount rate it is **9.8780 %**. The ECA route was worth paying up to about a **9.9
%** premium on the equipment, and the actual premium was **8.0 %**, a margin of **1.9 percentage
points**, against a sole-source negotiation still in progress and a discount-rate assumption
that moved the breakeven to **8.7179 %** at 6 % and **10.8612 %** at 10 %. The decision was
live, not comfortable. Two months later the ECA re-rated the host country and raised its
exposure premium from 6.5 % to **9.0 %**, lifting the financed loan to 111,180,000, the
instalment to **10,455,929**, the all-in to **5.2291 %** and the route's present value to
**104,201,155** (**199,494 worse** than Route S). The flip point on the exposure premium was
**8.7477 %**. The concessionaire switched to Supplier S, having preserved the option by refusing
to sign a sole-source commitment while the premium was un-fixed.

**What the domain teaches here.** A financing decision that is tied to a procurement decision is
one decision, and it must be evaluated on the present value of the combined cash flows rather
than on a comparison of rates. The output a leader should demand is not "which tranche is
cheaper" but **the breakeven premium** (the price differential at which the cheap money stops
being worth its conditions), because that number is a negotiating position and a monitoring
trigger, while a rate comparison is neither. Route N also carried a real coverage advantage
worth naming: annual debt service of 10,216,114 against 11,451,063, **1,234,949** a year lower,
almost entirely from two extra years of tenor (9.3.3). Had coverage been the binding constraint
rather than cost, the answer would have survived the premium re-rating.

---

## Executive perspective — Domain 9

What a project finance director cannot delegate in this domain:

- **The hurdle rate, derived.** Not inherited, not a policy round number. The director owns the
  build-up (`r_f`, `ERP`, `β_a`, `CRP`, `SP`, tax rate, each with a named source and a date),
  and owns the reconciliation. Kestrel's 8.0001 % at the coverage-binding gearing was Domain 4's
  given 8.0 %; that agreement was luck until somebody checked it.
- **The refusal to use the group's cost of capital.** Kestrel's project `WACC` is **8.0001 %**
  against the sponsor's corporate **6.544 %**; discounting the project at the group rate raises
  its `NPV` from 16,179,360 to **23,447,722** (an overstatement of **7,268,362**, or **44.92
  %**). The gap is structural, not a rounding difference: a diversified investment-grade group
  carries lower gearing, cheaper debt, a lower asset beta, and neither a country premium nor a
  single-asset illiquidity premium. **The rate belongs to the project, not to the balance sheet
  that sponsors it**, and a director who allows the group rate into a project appraisal has
  authorised the systematic acceptance of value-destroying projects.
- **The gearing decision, on both faces.** The exchange rate (5.75 basis points of equity return
  per hundredth of `DSCR` at the margin), and the gearing at which each covenant binds (68.62 %,
  74.34 %, 77.57 %). These are the director's numbers because they trade a lender's protection
  against a shareholder's return, and no analyst should be asked to make that trade.
- **Where a grant lands.** The same 6,000,000 either lifts equity return 429 basis points or lifts
  coverage 21 points. The director states the objective **before** the allocation is negotiated,
  and states the treatment convention before any `WACC` is quoted.
- **The all-in discipline.** No tranche enters a comparison on its headline rate. The DFI tranche
  at 5.25 % costs 7.2465 %; the ECA tranche at 3.80 % costs 4.6895 %; the ijara's 15 basis point
  premium is really 4. The director enforces the method, because the method is where the errors
  are.
- **The honesty of a refinancing.** Report gains net of cost and gain-share, decomposed into rate
  and extension, at more than one discount rate — and never let a deferral be presented as value
  created.

## Calculation exercises — Domain 9

**Exercise 9.1** A 150,000,000 project is geared 70/30. Senior debt costs 5.5 % and the tax rate
is 25 %. Build the cost of equity from `r_f` 3.80 %, `ERP` 5.5 %, `β_a` 0.55, `CRP` 1.20 % and
`SP` 0.40 %, then compute the `WACC`. *Solution.* `D/E = 105,000,000/45,000,000 = 2.333333`; `(1
− 0.25) × 2.333333 = 1.75`; `β_e = 0.55 × 2.75 =` **1.5125**; `β_e × ERP = 8.31875`; `k_e = 3.80 +
8.31875 + 1.20 + 0.40 =` **13.71875 %**. After-tax debt `5.5 × 0.75 =` **4.125 %**; `WACC =
0.70 × 4.125 + 0.30 × 13.71875 =` **7.0031 %**. *Common error:* using the pre-tax cost of debt,
giving 7.9656 % — a 96 basis point overstatement that would kill marginal projects. A second
common error is failing to re-lever at all (`β_e = β_a = 0.55`), giving `k_e` 8.425 % and a
`WACC` of 5.4150 % (a 159 basis point understatement, which approves projects that destroy
value).

**Exercise 9.2** A 100,000,000 project generates level `CFADS` of 14,000,000 over a 22-year life;
debt costs 6.5 % over 15 years (`AF(0.065, 15) = 9.402669`). Compare 65 % and 75 % gearing on
`DSCR` and equity `IRR`. *Solution.* At 65 %: debt 65,000,000, equity 35,000,000, debt service
`65,000,000/9.402669 =` **6,912,930.89**, `DSCR` **2.0252**, distributable 7,087,069.11, equity
`IRR` **20.7836 %**. At 75 %: debt 75,000,000, equity 25,000,000, debt service **7,976,458.72**,
`DSCR` **1.7552**, distributable 6,023,541.28, equity `IRR` **24.8151 %**. Ten points of gearing
buys **403.15 basis points** of equity return for **0.2700** of coverage. *Common error:*
computing the equity `IRR` on the whole `CFADS` stream rather than on `CFADS` less debt service,
which produces the *project* return and makes gearing look irrelevant.

**Exercise 9.3** Senior 80,000,000 at 5.2 %, mezzanine 20,000,000 at 10.8 %, equity 50,000,000
at 14.5 %; tax 30 %; both debt tranches deductible. Compute the `WACC`. *Solution.* After-tax
senior `5.2 × 0.7 =` **3.64 %**; mezzanine `10.8 × 0.7 =` **7.56 %**; `WACC = (80 × 3.64 + 20 ×
7.56 + 50 × 14.5)/150 = (291.2 + 151.2 + 725)/150 =` **7.7827 %**. *Common error:* tax-adjusting
the equity cost as well, giving 6.3327 % (a 145 basis point understatement). Also common: a
simple average of the three costs (8.5667 %), which ignores that senior debt is more than half
the capital.

**Exercise 9.4** A 40,000,000 tranche carries 4.10 % over 12 years with a 5.5 % exposure premium
capitalised into the loan. Compute the all-in cost on the proceeds actually available. *Solution.*
Premium **2,200,000**; loan **42,200,000**; `AF(0.041, 12) = 9.330854`; instalment
`42,200,000/9.330854 =` **4,522,630.17**; solve `40,000,000 = 4,522,630.17 × AF(r, 12)` → **5.0378
%**, an uplift of **93.78 basis points** over the headline. *Common error:* spreading the premium
straight-line across the tenor and adding it to the rate (`4.10 + 5.5/12 = 4.5583 %`), which
ignores that the premium is itself financed and repaid with a return on it.

**Exercise 9.5** A 120,000,000 project has `CFADS` of 13,000,000 and debt at 6.2 % over 13 years
(`AF(0.062, 13) = 8.750167`); tax 25 %. An 18,000,000 grant is awarded. Case (a): the grant
displaces equity, leaving debt 84,000,000 and equity 18,000,000, with `k_e` 16.50 %. Case (b):
the grant displaces debt, leaving debt 66,000,000 and equity 36,000,000, with `k_e` 13.50 %.
Compute `DSCR` and `WACC` for each. *Solution.* After-tax debt `6.2 × 0.75 =` **4.65 %**. Case
(a): debt service `84,000,000/8.750167 =` **9,599,817.02**, `DSCR` **1.3542**; `WACC = (84 ×
4.65 + 18 × 16.50 + 18 × 0)/120 =` **5.7300 %**. Case (b): debt service `66,000,000/8.750167 =`
**7,542,713.37**, `DSCR` **1.7235**; `WACC = (66 × 4.65 + 36 × 13.50)/120 =` **6.6075 %**. The
equity-displacing case has the **lower** `WACC` and the **worse** coverage (the 9.4.2 result, in
a different project). *Common error:* omitting the grant from the denominator of the `WACC`
weights, which inflates the debt and equity shares to sum to more than one and produces a
meaningless figure.

## Practitioner's toolkit — Domain 9

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable, and set a
retention period against each. These registers are the evidence that a decision was taken
properly, so each is retained at least as long as the obligation it supports, in a form that
opens without the tool that created it, with a named custodian who holds it once the engagement
ends. The applicable minimum periods are set by the organisation's own policy and by
jurisdiction-specific statutory, tax and limitation requirements, which this book does not
state. Where a register holds information about identified individuals, the retention period and
any minimisation or deletion obligation that cuts across it are settled with the organisation's
data-protection adviser before the register is adopted.*

### Toolkit 9.T.1 — Cost-of-capital derivation record (one per project, per structure)

One page, signed. Rows: `r_f` — instrument, tenor, currency, quotation date, source · `ERP` —
value, source, date, market · `β_a` — value, comparator set named, source of the unlevering ·
re-levering formula and whether non-beta premiums are re-levered · `D/E` used, and confirmation it
matches the funding plan · `CRP` and `SP` — value and basis · resulting `k_e` · tax rate and the
written opinion supporting deductibility, per tranche · each tranche's pre- and post-tax cost ·
weights and their sum · resulting `WACC` · **the `WACC` at the coverage-binding gearing** and the
gap to the proposed structure · the sponsor's corporate `WACC`, stated for contrast, with a note
that it is **not** the project rate. Rule: no appraisal in the organisation uses a discount rate
without a current signed record, and the record is re-signed whenever the funding plan changes.
*Retention:* held by a named custodian for the longest of the life of the facility and its tail, the
limitation period applicable to claims under the relevant documents, and any statutory tax,
accounting or regulatory requirement the organisation has established — with each source, opinion
and quotation date held alongside the record rather than separately, in a form that reproduces the
rate from its own trail. The periods themselves are jurisdiction-specific and are confirmed, not
assumed.

### Toolkit 9.T.2 — Tranche comparison sheet (all-in, like-for-like)

One column per candidate tranche. Rows: amount · currency · base rate and margin, or profit rate ·
fee schedule (arrangement, commitment, agency, structuring) · capitalised premiums · tenor,
availability period and amortisation profile · grace · prepayment mechanics and any make-whole ·
security and ranking · conditions precedent and expected time to satisfy · compliance and
monitoring cost, for the full tenor · currency of repayment against currency of revenue · **cash
proceeds actually available** · **all-in effective rate, solved from the stream** · annual debt
service · `DSCR` contribution · **debt capacity supported at the target coverage** · grant
element, where concessional. Two mandatory footers: the method statement (all-in rates solved, not
added) and the separation rule (cost effects and capacity effects reported in different rows,
never netted). *Retention:* held by a named custodian with the term sheets and fee letters the
columns were built from, for the life of the tranche selected plus the applicable limitation period,
in a form that reproduces each all-in rate from its own stream.

### Toolkit 9.T.3 — Capital-structure decision record

For each candidate structure, one row: tranche amounts and costs · re-levered `k_e` · `WACC` ·
senior-only `DSCR` · total `DSCR` · minimum `DSCR` over the loan life · gearing at which each
covenant and the lock-up binds · equity `IRR` on the bank case **and** on the sponsor case, with
the escalation assumption that separates them · treatment of any grant, stated · junior
instruments and their placement (SPV or HoldCo), with the lock-up shortfall at which HoldCo
service stops · refinancing assumption, if any, and whether a gain-share applies. Front line:
**which constraint binds, at what gearing, and what the structure costs in `WACC` to stay inside
it.** The record is minuted with the decision, and the rejected structures stay in it. Where the
structure relies on government support, the row also carries the **confirmation of lawful
grant** (KA 9.4.1), who obtained it, its date, and the condition-precedent line it sits on.
*Retention:* held by a named custodian with the board minute, for the longest of the facility's
life and tail, the applicable limitation period and any statutory requirement the organisation
has established; the confirmation of lawful grant is retained for as long as recovery of the
support could be pursued, which is a period established with counsel rather than assumed.

## Exam preparation — Domain 9

**What is assessed.** Building a cost of equity from stated components with correct re-levering;
computing an after-tax cost of debt and a multi-tranche `WACC` with correct weights and tax
treatment; quantifying the leverage trade-off on both faces and identifying the gearing at which a
covenant binds; distinguishing a tranche's cost effect from its debt-capacity effect; computing an
all-in effective cost from a stream containing fees and capitalised premiums; computing a grant's
effect on `WACC`, `DSCR` and equity return, and a concessional loan's grant element; testing
sustainability-linked pricing against its verification cost; and computing and decomposing a
refinancing gain.

**The calculations to have automatic under time pressure.** `k_d(1 − T)`. `β_e = β_a(1 + (1 −
T)D/E)` and the build-up onto it. `WACC` as a weighted sum with the grant at zero cost and in the
denominator. `DSCR = CFADS ÷ (D ÷ AF(r, n))` and its inversion to a binding gearing (`max debt =
CFADS/target × AF`, then ÷ capex). All-in cost as the rate solving `proceeds = payment × AF(r,
n)`. Grant element as `1 − PV(service at market rate)/face`. Outstanding balance as `instalment ×
AF(r, remaining)`.

**The traps.**

- Using the pre-tax cost of debt in `WACC` (Exercise 9.1, 96 basis points).
- Tax-adjusting the cost of equity (Exercise 9.3, 145 basis points).
- Failing to re-lever `k_e` when comparing structures: 9.1.3, 9.2.3; every comparison in KA 9.2
  is invalid without it.
- Re-levering **without** the tax adjustment (MCQ 9.1-A, distractor C).
- Omitting a grant from the `WACC` denominator so the weights exceed one, Exercise 9.5.
- Comparing tranches on headline rate plus fees instead of the solved all-in rate (9.3.1, 9.3.2,
  Exercise 9.4).
- Treating a capitalised premium as a one-off percentage added to the rate, Exercise 9.4.
- Netting a cost effect against a capacity effect (9.3.3, MCQ 9.3-B).
- Placing mezzanine at SPV level and expecting the senior `DSCR` to be unaffected (9.2.2, Case A
  option two).
- Stress-testing HoldCo junior debt against the covenant instead of the lock-up (Case A, 11.9061
  %).
- Reporting a refinancing extension as value created (9.4.4, MCQ 9.4-C).
- Discounting a project at the sponsor's corporate `WACC` (Executive perspective, 44.92 %
  overstatement).
- Quoting an equity `IRR` without naming its cash-flow case, or without disclosing an equity
  bridge, 9.1.1, 9.1.4.
- Modelling a shareholder-loan tax shield without a tax opinion, or with the interest mis-ranked
  (9.1.2, MCQ 9.1-D).
- Valuing a sustainability ratchet against a single year's compliance cost rather than the full
  tenor, 9.4.3.

**How the domain connects.** Domain 3 supplied `AF(r, n)`, which does almost all the arithmetic
here, and Domain 4 the appraisal this domain's `WACC` finally justifies. Domain 7's indexation
drafting supplies the 1.7403 % escalation on which the equity case rests; Domain 8's contingency
and delay arithmetic sets the funding headroom. Domain 10 supplies the coverage machinery that
constrains everything here and the four sizing levers this domain prices; Domain 11 allocates the
risks that set `CRP`, `SP` and the required coverage; Domain 12 documents the intercreditor,
security and gain-share terms; Domain 13 audits the model; Domain 14 draws the tranches; and
Domain 15 lives with the structure and executes the refinancing this domain sized.

## Domain 9 summary

Funding structure is where a project's economics are finally decided, and this domain's central
claim is that the decision is **not** made by minimising the cost of capital. Kestrel's cost of
equity, built up from a 4.10 % risk-free rate, a 6.00 % equity risk premium on an asset beta of
0.60 re-levered to **1.72**, and one point of country and single-asset premium, is **15.42 %**;
its after-tax cost of debt, on the 20.0 % cash tax rate the thread's own numbers imply, is
**4.80 %**; and its `WACC` at the proposed 70/30 structure is **7.9860 %**. The whole ladder
collapses to `WACC(g) = 8.70 % − 1.02 % × g`, whose slope is small precisely because the
non-systematic equity premium of 5.10 % barely exceeds after-tax senior debt at 4.80 %, so
across twenty points of gearing the project saves **20.40 basis points** while the sponsors gain
**184.61** and the lenders lose **0.3716** of `DSCR`. Gearing is a transfer, not an efficiency,
and its exchange rate at the margin is about **5.75 basis points of equity return per 0.01 of
coverage surrendered**. The constraint that decides the structure is coverage: the 1.30× sizing
target binds at **68.6185 %** gearing, the 1.20× covenant fails on the base case above **74.3367
%**, and the 1.15× lock-up above **77.5688 %**. Evaluated at the coverage-binding gearing the
project `WACC` is **8.0001 %**, which is Domain 4's given 8.0 %, derived at last, and right only
for that structure; the sponsor's corporate `WACC` of **6.544 %** would have overstated the
appraisal `NPV` by **7,268,362**, or **44.92 %**, and is never the project's rate. Within the
stack, mezzanine raises the blended cost in every direction (**8.4510 %** displacing senior,
**8.3240 %** displacing equity), and is bought for coverage relief (senior `DSCR` **1.5115**) or
equity uplift, never for cheaper capital; its **placement** decides everything, and HoldCo
mezzanine covered **2.6126** times in the base case receives nothing once `CFADS` falls
**11.9061 %** to the lock-up trigger. Every tranche is compared on an all-in rate solved from
its own stream: the ijara's 15 basis point headline premium is really **3.95**; the ECA's 3.80 %
is **4.6895 %** and releases **303,726** a year of debt service; the DFI's 5.25 % is **7.2465
%** all-in yet its six extra years of tenor lift debt capacity from **41,171,123** to
**56,299,948**, because tenor, not rate, moves the binding constraint. Public support must be
aimed: the same 6,000,000 grant either lifts equity return **429.20 basis points** or coverage
**21.24 points**, and the treatment convention alone moves `WACC` by **67.20 basis points**; a
12,000,000 concessional loan at 2 % carries a **40.80 %** grant element. A 15 basis point
sustainability ratchet worth **528,182** cannot pay for **712,627** of verification, so the case
for a label must rest on the base margin and the investor pool, not on the ratchet. And a
refinancing gain of **3,723,616** is **298,757** of rate saving and **3,424,859** of deferral,
worth only **566,832** discounted at the loan rate and halved again by a gain-share, which is
why every gain is reported net, decomposed, and at more than one rate. Domain 10 tests the
coverage this domain has just priced; Domain 13 audits the model behind it; Domain 15 operates
the structure and executes the refinancing.
