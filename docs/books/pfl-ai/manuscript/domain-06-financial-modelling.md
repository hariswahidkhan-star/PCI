# Domain 6 — Financial Modelling and Model Governance *(quantitative)*

> **Group:** Structuring and modelling (Domain 2 of 5 in Part Two). **Target:** ~78 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain is the home of the *model* — the artefact on which
> Domains 3, 4, 5 and 10 all silently depended. It consumes Domain 3's discounting and
> amortisation machinery (`AF(r, n)`, `DF(t)`), Domain 4's appraisal measures (`NPV`, `IRR`,
> `MIRR`, `PI`), Domain 2's three statements and `CFADS` definition, and Domain 10's coverage
> ratios (`DSCR`, `LLCR`, `PLCR`), and it takes responsibility for whether any of them is
> computed correctly. British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).
> Tax and accounting treatments are described in principle and are jurisdiction-specific: the
> arithmetic here is transferable, the treatments are not, and neither is a substitute for
> professional tax or accounting advice.

## Why this domain exists

Every number in Domains 3 to 5 and 10 came out of a model, and not one of those domains examined
the model. Domain 4 decided to build on an `NPV` of **+16,179,360**. Domain 10 negotiated debt
capacity against a `CFADS` of **6,384,000** and a `DSCR` of **1.2743**. Domain 5 priced a
commercial-operations-date slip at **24,733.33** a day. Each is the output of one workbook,
produced by one analyst, reviewed by whoever had time — and each has been quoted without its
basis, its horizon or its checks attached. That is the gap this domain closes.

The central claim is worth stating plainly: **a financial model's authority comes from its
architecture and its checks, not from its answers.** Two competent analysts modelling the same
project on the same contracts will produce different numbers, and the difference will almost never
be arithmetic. It will be *basis* (pre-tax or post-tax, accrual or cash, before or after working
capital), *horizon* (fifteen years of evaluation or twenty-five of concession), *convention*
(annual or quarterly periods, interest on opening or average balances) and *case* (the sponsor's
escalation or the lender's flat line). On Kestrel's own figures one project supports five
arithmetically correct net present values spanning **USD 29,545,516** — which is why a model
output quoted without its basis and horizon is not information but decoration.

**Learning objectives.** After this domain a candidate can: design a three-block architecture with
a single timeline and defend each convention chosen; build a sources-and-uses statement in which
capitalised interest is computed rather than assumed, identify its balancing line and test that
line against policy; quantify how period length and interest convention change capitalised
interest, the depreciation base and after-tax value; distinguish accounting depreciation from tax
allowances and compute the coverage and debt-capacity consequences; build a debt schedule that
closes to zero, run a cash waterfall through reserve funding to distributions, and compute an
equity return and an equity payback; state and test the six arithmetic invariants a project model
must satisfy, explain why a balancing balance sheet is necessary and not sufficient, and localise
an error from the check that fails; reconcile two models of one project into a basis bridge; build
a one-at-a-time sensitivity table with elasticities, state honestly what it cannot see, and convert
ratio headroom into input breakevens; compute the net value, breakeven fee, breakeven error rate
and breakeven detection rate of a model audit and show why timing matters more than price; and
govern AI-assisted modelling with controls specific enough to fail.

**The master model.** Kestrel Water SPC continues from Domains 1–5 and 10. Capital cost
**USD 60,000,000** funded **70/30** as **USD 42,000,000** of senior debt at **6.0 % over 12 years**
(annual instalment **USD 5,009,635.23**; year-one interest **2,520,000**, principal
**2,489,635**) plus **USD 18,000,000** of equity. Operating life **25 years**; revenue
**12,000,000**, cash operating costs **4,500,000**, `EBITDA` **7,500,000**, depreciation
**2,400,000**, `EBIT` **5,100,000**, cash tax **516,000**, documented first-year **`CFADS`
USD 6,384,000** (6,984,000 before working-capital movements). Coverage at close (Domain 10):
`DSCR` **1.2743** = `LLCR` **1.2743**, `PLCR` **1.9431**, covenant cash trigger **6,011,562**,
six-month DSRA **2,504,818**. Appraisal (Domain 4): `NPV` **+16,179,360** at 8 %, `IRR`
**12.19 %**, `MIRR` **9.73 %**, `PI` **1.270**. This domain adds the assumptions those chapters
did not need and could not do without: the **eight-quarter construction drawdown** funded 70/30
against certified spend, the **EPC price of 48,000,000** (Domain 5) inside a **60,000,000
envelope** whose balancing line is contingency at **3,645,403**, capitalised interest of
**2,114,597**, the **2.967 % escalation** that reconciles Domain 4's appraisal to Domain 2's
`CFADS`, the DSRA funded from operating cash over two years, and the two cases — sponsor and bank —
that every subsequent number has to be labelled against.

---

## Knowledge Area 6.1 — Model architecture

*Topics: 6.1.1 the three-block rule · 6.1.2 the timeline as the model's spine · 6.1.3 basis,
horizon and case — the labels without which an output means nothing.*

### 6.1.1 The three-block rule

**Definition.** A project financial model is organised into three strictly separated blocks:
**inputs** (every assumption, each entered exactly once, in one place, with a source), **calculations**
(formulae only, no constants), and **outputs** (presentation only, no calculation). Nothing flows
backwards: outputs never feed calculations, calculations never contain assumptions, inputs never
contain formulae.

**Why the separation carries the weight.** It is not tidiness; it is what makes a model auditable
by a stranger in the time a stranger has. An assumption entered once can be *changed* once, so a
scenario is a switch rather than a search-and-replace across sheets — which is why models without
the separation end up with scenarios that differ in ways nobody intended. A calculation block
containing no constants can be **read**: any number inside a formula is by construction a defect,
so review reduces to scanning for numerals. And an output block that computes nothing cannot
disagree with the model, which removes the most damaging class of audit finding — the summary page
that no longer ties to the engine behind it.

**The cardinal defect is the hard-coded constant.** A tax rate typed into a formula, a tariff
escalator embedded in a growth row, a debt margin buried in an interest calculation: each is
invisible to a reviewer, survives every scenario switch, and produces a model that is *stable in
the wrong answer*. Two further rules earn their place. **One row, one formula, copied across**: a
row whose formula changes mid-timeline is invisible in the printed output and usually marks the
point where somebody patched a symptom. And **sign discipline stated once and enforced everywhere** —
either convention works, and mixing them produces a model that adds a cost to a revenue and
balances anyway.

### 6.1.2 The timeline as the model's spine

**Definition.** The timeline is a single row of periods with, for each period, its start and end
dates, its day count on the stated convention, and a set of mutually exclusive **flags** —
construction, operations, loan life, post-loan tail — that every calculation multiplies by
instead of testing dates itself.

The flags convert scattered date logic into arithmetic that can be checked by addition. Kestrel's
annual model runs **27 periods** — two construction and twenty-five operating — and its quarterly
construction model runs **eight**. The invariants are trivial to state and therefore trivial to
test: construction flags sum to the construction period, operating flags to the operating life, no
period carries two flags, and all flags sum to the model length. A model in which those four
additions hold cannot have a period that is silently both building and operating, which is the
defect that puts revenue into a construction quarter or capitalised interest into an operating year.

**Periodicity is an economic choice, not a presentation one.** Coarser periods do not merely round;
they systematically misstate anything that compounds within a period. Worked example 6.2.1 prices
this on Kestrel: the same drawdown profile at the same 6.0 % returns capitalised interest of
**2,114,597** quarterly and **1,247,352** annually — an understatement of **867,245**, or
**41.01 %**, before a single assumption has been challenged.

**The conventions that must be stated once, in the inputs block:** day count (30/360, actual/360,
actual/365 — Domain 5's slip calculation used 30/360 and said so); whether flows arise at period
end, start or mid-period; whether interest accrues on the opening or the average balance; and
whether the first period is a stub. None is a matter of taste; each changes printed results, and a
model that does not state them cannot be reproduced.

### 6.1.3 Basis, horizon and case

**Definition.** Every model output carries three labels without which it is meaningless: its
**basis** (pre-tax or post-tax; accrual or cash; before or after working capital; levered or
unlevered), its **horizon** (the number of periods valued, and what is assumed about value beyond
them), and its **case** (which set of assumptions was switched on).

**Worked example 6.1.3 — one project, five correct net present values.**

1. **Setup.** Kestrel, on the master model. Domain 4 appraised net inflows of **8,900,000** a year
   for **15 years** against `I₀` = 60,000,000 at **8.0 %**. Domain 2 documented a first-year
   `CFADS` of **6,384,000**. The concession's operating life is **25 years**. Reconcile the two
   figures and value the project on each defensible basis and horizon.
2. **Formula.** Domain 4's present value of inflows is `8,900,000 × AF(0.08, 15)`. An `EBITDA`
   stream starting at 7,500,000 and escalating at `g` has present value
   `Σ 7,500,000 (1+g)^(t−1)/(1.08)^t`; solve for the `g` that reproduces it. Post-tax unlevered
   free cash flow, with revenue, cash costs and working capital all indexed at the same `g` and
   depreciation fixed at 2,400,000, reduces to
   `FCF(t) = 5,400,000 (1+g)^(t−1) + 480,000` (the second term is the depreciation shield,
   `0.20 × 2,400,000`).
3. **Substitution.** `8,900,000 × 8.559479 = 76,179,360`. Solving the escalating `EBITDA` stream
   over 15 years at 8 % gives **`g` = 2.967 %**, reproducing a present value of 76,180,021 —
   **661** above Domain 4's 76,179,360, or 0.001 %. Then value `FCF(t)` at 8 % over 15 and 25
   years, at `g` = 2.967 % and at `g` = 0.
4. **Result.**

   | # | Basis | Horizon | Case | `NPV` at 8 % |
   |---|---|---|---|---|
   | 1 | Pre-tax operating cash (Domain 4) | 15 years | escalating | **+16,179,360** |
   | 2 | Post-tax, unlevered, after working capital | 15 years | `g` = 2.967 % | **−1,041,835** |
   | 3 | Post-tax, unlevered, after working capital | 25 years | `g` = 2.967 % | **+19,875,251** |
   | 4 | Post-tax, unlevered, after working capital | 25 years | flat (bank case) | **+2,767,684** |
   | 5 | Post-tax, unlevered, after working capital | 15 years | flat | **−9,670,265** |

   The widest defensible spread is **29,545,516** — from −9,670,265 to +19,875,251 — on one
   asset, one contract set and one discount rate.
5. **Interpretation.** Not one of those five figures is an error, and the reconciliation proves
   it: Domain 4's 8,900,000 is precisely what Kestrel's year-one `EBITDA` of 7,500,000 becomes as
   a fifteen-year level equivalent at 2.967 % escalation, so the two models agree on the asset and
   disagree only about what to measure. The spread decomposes cleanly and each component is a
   decision somebody must own. **Basis** moves the fifteen-year answer from +16,179,360 to
   −1,041,835: cash tax and working capital cost the project **17,221,195** of present value, and
   whoever approves an appraisal on a pre-tax basis has approved that omission. **Horizon** moves
   the post-tax answer from −1,041,835 to +19,875,251: ten further years of a twenty-five-year
   concession are worth **20,917,086**, so a fifteen-year evaluation of a twenty-five-year asset
   is not conservatism, it is a valuation of a different asset. **Case** moves the twenty-five-year
   answer from +19,875,251 to +2,767,684: the lender's refusal to give credit for escalation is
   worth **17,107,567** of the sponsor's case, which is why escalation is negotiated as hard as
   margin. The professional consequence is a rule, not a caution: **no `NPV`, `IRR` or coverage
   ratio leaves a model without its basis, horizon and case attached to it in the same sentence.**
   And the reviewer's habit that follows is the **basis bridge** — a reconciliation from the
   appraisal number the board remembers to the financing number the lenders enforce, showing every
   step between them. Kestrel's bridge is four lines long and worth 29.5 million; a project
   without one is carrying that exposure undocumented.

### AI in this KA

**Where it earns its place.** Architectural conformance is machine-checkable and tedious, the ideal
combination: scanning a calculation block for embedded numerals, finding rows whose formula changes
mid-timeline, listing inputs referenced from more than one place, testing the flag additions, and
diffing two versions into a change list a human can read. A large model has tens of thousands of
cells and a reviewer has two days; this is the part of review that should never be done by a person.

**Where it must not go.** An assistant must not choose the model's conventions, and in practice it
will: asked to "build a construction model" it picks an annual timeline with opening-balance
interest, because that is the simplest defensible construction — and Worked example 6.2.1 prices
that choice at 867,245. Nor may it decide the basis, horizon or case; those are governance
decisions with owners, and a model whose horizon was chosen by a tool is a model nobody has
approved.

**Verification, concretely.** Require the tool to output the conventions it assumed as an explicit
list, and check that list against the term sheet and the assumption register line by line.
Re-derive one period of the timeline by hand — dates, day count, flags — and confirm the four flag
additions. For any machine-generated architecture review, confirm a sample of at least ten flagged
cells against the workbook before the findings are reported: a false positive in an architecture
review costs more credibility than the defect it replaced. **AI proposes; the professional
verifies, decides and remains accountable.**

### Key terms — KA 6.1

| Term | Meaning |
|---|---|
| **Three-block architecture** | Inputs, calculations, outputs — strictly separated; no back-flow. |
| **Hard-coded constant** | An assumption typed inside a formula; invisible to review and immune to scenarios. |
| **Timeline flags** | Mutually exclusive period markers (construction, operations, loan life, tail) that calculations multiply by. |
| **Periodicity** | Period length; an economic choice because compounding happens within periods. |
| **Basis** | Pre/post-tax, accrual/cash, before/after working capital, levered/unlevered. |
| **Horizon** | Periods valued, and the treatment of value beyond them. |
| **Case** | The named set of assumptions switched on (sponsor case, bank case, downside). |
| **Basis bridge** | The documented reconciliation between two models of one project. |

### Sample MCQs — KA 6.1

**MCQ 6.1-A `[6.1.3 · Analysis]`** Kestrel's appraisal shows `NPV` +16,179,360 (pre-tax operating
cash, 15 years) and its financing model shows +2,767,684 (post-tax unlevered, flat, 25 years).
The correct conclusion is:

- A. the financing model contains an error of 13,411,676
- B. the appraisal is optimistic and should be discarded
- C. both are correct on their stated basis, horizon and case, and the required deliverable is the bridge between them ✅
- D. the difference is the interest tax shield

*Rationale:* Worked example 6.1.3 reconciles them exactly — the appraisal's 8,900,000 is the
fifteen-year level equivalent of an `EBITDA` stream escalating at 2.967 %, and the gap is basis,
horizon and case. A and B assume one number must be wrong; D names a term that is absent from
both, since each figure here is unlevered.

**MCQ 6.1-B `[6.1.2 · Application]`** Kestrel's construction spend is modelled with the same
profile and the same 6.0 % rate on an annual rather than a quarterly timeline, interest accruing
on the opening balance in both. Capitalised interest changes from 2,114,597 to:

- A. 2,114,597 — periodicity does not affect a total
- B. 1,247,352, an understatement of 867,245 ✅
- C. 2,427,554
- D. 8,458,388

*Rationale:* Coarse periods ignore intra-period draws, so the opening balance on which interest
accrues is far too small (6.1.2, Worked example 6.2.1). A assumes periodicity is presentational;
C is the quarterly figure on the *average*-balance convention; D multiplies the quarterly figure by
four, confusing periods with rates.

**MCQ 6.1-C `[6.1.1 · Analysis]`** A reviewer finds a 20 % tax rate typed inside three
calculation formulae rather than referenced from the inputs block. The most serious consequence
is:

- A. the model is harder to read
- B. the tax rate cannot be changed
- C. scenario switches silently leave the tax rate unchanged, so every case in the model is
  internally inconsistent in a way no output reveals ✅
- D. the model will not balance

*Rationale:* The hard-coded constant survives the scenario switch, so the downside case is run at
the base-case tax rate and nothing on any output page says so (6.1.1). A understates it; B is
false — it can be changed, three times, which is the problem; D is wrong, because a consistent
wrong number balances perfectly (6.4.1).

**MCQ 6.1-D `[6.1.1 · Comprehension]`** A modeller adds a subtotal formula to a summary page and
defends it: "it is only a sum". The statement that best conveys why the three-block rule forbids
it is:

- A. spreadsheets round subtotals differently from the cells beneath them
- B. an output block that computes anything can disagree with the engine behind it, and a summary page that no longer ties to the model is the most damaging class of audit finding ✅
- C. it enlarges the file and slows recalculation
- D. subtotals belong in the inputs block, where they can be changed in one place

*Rationale:* the separation exists so that each block can be read for one kind of defect — inputs
for wrong assumptions, calculations for embedded constants, outputs for nothing at all, because they
compute nothing (6.1.1). A invents a rounding problem; C is a performance claim, not the principle;
D inverts the architecture, since the inputs block holds no formulae.

**MCQ 6.1-E `[6.1.3 · Evaluation]`** A board paper reports "`NPV` +16,179,360, `IRR` 12.19 %" with
no labels. The financing model shows +2,767,684 post-tax unlevered over 25 years on the flat case,
and an unlevered post-tax `IRR` of 8.54 % against an 8 % hurdle. All four corrections below are
legitimate. Which most changes the decision the board is being asked to take?

- A. attach basis, horizon and case to both figures, as the standing rule requires
- B. state that basis alone — cash tax and working capital — costs 17,221,195 of present value over the fifteen-year horizon
- C. put the twenty-five-year post-tax flat case in front of them — +2,767,684, and an asset return of 8.54 % against an 8 % hurdle — because that is the basis on which the decision is close ✅
- D. disclose the full defensible spread of 29,545,516, from −9,670,265 to +19,875,251

*Rationale:* A is the standing rule and its breach is why the defect arose, but a *labelled*
+16,179,360 still tells the board a comfortable story; B is one of the three components of the gap
and leaves horizon and case unaddressed; D discloses without recommending and invites a board to
choose its own number from a range. Only C changes what the board is deciding, by showing the
basis on which the asset barely clears its hurdle (6.1.3, 6.3.3) — after which the bridge,
the labels and the spread are all supporting material.

**MCQ 6.1-F `[6.1.2 · Evaluation]`** An assistant is asked to build Kestrel's construction model and,
unprompted, chooses an annual timeline with interest on the opening balance. Capitalised interest
comes out at 1,247,352 against the quarterly opening-balance figure of 2,114,597 — an understatement
of 867,245, or 41.01 %. The modeller observes that every check in the model passes. The soundest
position is that:
- A. the choice is acceptable, because both conventions are defensible and the checks pass
- B. periodicity and the interest accrual base are economic choices with a named owner, must be
  stated in the conventions sheet, and here have understated the depreciable base by 867,245 as well
  as the interest — a defect no check in the model can see ✅
- C. the annual timeline is wrong and a quarterly one is always required
- D. the difference is immaterial at 3.52 % of the envelope

*Rationale:* an assistant must not choose the model's conventions, and this is the choice it makes
(6.1.2, and the AI boundary in KA 6.1): the 41 % error needs the coarse timeline *and* the
opening-balance base together, since an annual model on average balances lands within 367,022 of the
quarterly answer. B is sounder than C because the pairing, not the period length, is the defect — and
the understatement propagates, cutting annual depreciation by 34,690 and the present value of the tax
it shelters by 74,061. A is the reasoning that lets a convention error survive review; D misquotes a
share of the envelope as a measure of a 41 % error in one line.

### Self-check — KA 6.1

1. *State the four additions that test a timeline.* — Construction flags sum to the construction
   period; operating flags to the operating life; no period carries two flags; all flags sum to
   the model length.
2. *Why is a hard-coded constant worse than a wrong constant?* — A wrong constant is visible in
   the inputs and can be challenged; a hard-coded one is invisible, immune to scenarios and
   therefore stable in the wrong answer.
3. *State the three labels every model output must carry.* — Basis, horizon, case (6.1.3); without
   them the spread on Kestrel is 29,545,516 and the number conveys nothing.

---

## Knowledge Area 6.2 — Construction and operating models

*Topics: 6.2.1 sources and uses, and capitalised interest · 6.2.2 the operating model and the
articulation check · 6.2.3 depreciation, tax allowances and the line most often modelled wrongly.*

### 6.2.1 Sources and uses, and capitalised interest

**Definition.** The **sources-and-uses statement** lists everything the project must pay for
between mandate and commercial operations (uses) against everything that funds it (sources), and
the two totals are identical by construction. **Interest during construction (IDC)**, or
capitalised interest, is the interest accruing on debt drawn before the asset earns revenue; it is
a use of funds like any other, is itself funded, and enters the depreciable asset base.

**The identity and the balancing line.** Sources equal uses is an identity, not a check — a model
can always be made to satisfy it. What makes it informative is knowing **which line balances**. The
funding envelope is fixed first, by what sponsors will commit and lenders will lend, and one use
absorbs the residual: usually contingency, sometimes owner's costs. The discipline has three steps:
let one line balance; **test that line against policy** (is the resulting contingency inside the
band lenders require for this technology and contract structure?); and if it is not, the envelope
is wrong, not the table. One corollary catches plugged tables at a glance: **the balancing line is
never a round number.**

**Worked example 6.2.1 — Kestrel's construction funding, and what the timeline costs.**

1. **Setup.** A **60,000,000** envelope funded **70/30** — 42,000,000 senior debt at 6.0 %,
   18,000,000 equity — with every use, capitalised interest included, funded in that proportion
   against certified spend. Committed uses: EPC price **48,000,000** (Domain 5), owner's costs and
   land **3,600,000**, capitalised development costs **1,800,000**, arrangement and financing fees
   at 2.0 % of the facility, **840,000**. Construction runs **eight quarters** with a certified
   spend profile of **6, 9, 13, 16, 17, 15, 13 and 11 per cent**. Interest accrues quarterly at
   6.0 %/4 on the **opening** debt balance (draws treated as made at period end). Fees and
   development costs are funded at financial close. Compute capitalised interest, the balancing
   contingency, and the cost of running the same model annually.
2. **Formula.** For each quarter: `interest = opening debt balance × 0.015`;
   `funding requirement = certified spend + interest`; `debt draw = 0.70 × requirement`;
   `equity draw = 0.30 × requirement`; closing balance = opening + debt draw. Contingency solves
   `fees + development + EPC + owner's costs + contingency + IDC(contingency) = 60,000,000`, and
   the profile applies to the **certified-spend base** — EPC plus owner's costs plus contingency.
3. **Substitution.** Close: requirement 2,640,000, debt draw 1,848,000. The certified-spend base
   solves to **55,245,403**, so quarter 1's 6 % is 3,314,724. Quarter 1: interest
   `1,848,000 × 0.015 = 27,720`; requirement `3,314,724 + 27,720 = 3,342,444`; debt draw
   2,339,711; closing balance 4,187,711. Iterating to quarter 8 gives a closing balance of exactly
   42,000,000 and cumulative equity of exactly 18,000,000.
4. **Result.**

   | Uses | USD | Sources | USD |
   |---|---|---|---|
   | EPC contract price | 48,000,000 | Senior debt drawn | 42,000,000 |
   | Owner's costs and land | 3,600,000 | Equity contributed | 18,000,000 |
   | Capitalised development costs | 1,800,000 | | |
   | Arrangement and financing fees (2.0 %) | 840,000 | | |
   | Contingency *(balancing line)* | **3,645,403** | | |
   | Interest during construction | **2,114,597** | | |
   | **Total uses** | **60,000,000** | **Total sources** | **60,000,000** |

   Capitalised interest is **3.52 %** of the envelope. Contingency is **7.59 %** of the EPC price.
   On an **average**-balance interest convention IDC rises to **2,427,554** (+312,957); on an
   **annual** timeline with opening-balance interest it collapses to **1,247,352** (−867,245, or
   −41.01 %); on an annual timeline with average-balance interest it is **2,481,619**.
5. **Interpretation.** Three results leave this example. **The convention is worth more than most
   of the assumptions people argue about.** A 41 % swing in capitalised interest from period length
   alone dwarfs a 10-basis-point margin negotiation, and it is invisible in every output the model
   prints. It also propagates: 867,245 of understated IDC understates the depreciable base by the
   same amount, annual depreciation by **34,690**, and the present value of the tax it shelters —
   20 % over 25 years at 8 % — by **74,061**. Small, but silent: a reviewer finds it in one line and
   a board never sees it. **The pairing of a coarse timeline with an opening-balance convention is
   the defect, not either choice alone.** An annual model on average balances lands within 367,022
   of the quarterly answer, so the 41 % error requires both mistakes together — which is why
   convention lists must be read as a set. **And the balancing line is evidence.** Kestrel's
   3,645,403 of contingency is 7.59 % of the EPC price, inside the band a lender would expect for
   proven water-treatment technology under a full EPC wrap (Domain 5, KA 5.4.1; Domain 8, KA 8.1.2
   tests that percentage against estimate class), so the envelope is credible. Had the residual
   come out at 2 %, the correct response would not have been to accept a thin contingency but to
   conclude that a 60,000,000 envelope does not fund this project — a financing conversation, not
   a modelling one. **Contingency is what is left, and what is left
   must still be defensible**; the arithmetic cannot tell you which of those two is binding.

> **Fig 6.2.1 — Construction funding and interest during construction.** Combination chart,
> x-axis the eight construction quarters plus financial close, left y-axis funding requirement
> (USD 0–10m). Stacked bars per period: certified construction spend (brand blue, 55 % opacity)
> topped by capitalised interest (crimson) rising from 27,720 in quarter 1 to 560,308 in
> quarter 8. Overlaid ink line: cumulative debt drawn, closing on **42,000,000**; dashed slate
> line: cumulative equity drawn, closing on **18,000,000**. Annotations: total IDC **2,114,597**,
> 3.52 % of the envelope; the annual-timeline understatement of **867,245** (41.01 %); footer
> stating sources 60,000,000 = uses 60,000,000 with the balancing contingency of **3,645,403**,
> 7.59 % of the EPC price. Source: PCI original. Alt text: stacked bars of quarterly construction
> spend with a thin crimson band of capitalised interest growing through the build, and two
> cumulative funding lines rising to forty-two million of debt and eighteen million of equity.

### 6.2.2 The operating model and the articulation check

**Definition.** The operating model produces, for every operating period, an income statement, a
balance sheet and a cash-flow statement that **articulate** — each derived from the same
calculations, so that the three cannot disagree — and from that cash-flow statement it derives
`CFADS` on the facility's documented definition.

Domain 2 built the three statements and Domain 10 built the ratios; the modelling content here is
the *linkage*, and one identity does most of the work. Where interest paid is classified in
operating cash flow, **`CFADS` equals operating cash flow plus interest paid** — because `CFADS` is
struck before all debt service and operating cash flow is struck after the interest half of it.
That single line converts `CFADS` from an assertion into something a reviewer can tie to a
statement, which is the whole point of building the statements at all.

**Worked example 6.2.2 — Kestrel's first operating year, proved three ways.**

1. **Setup.** Opening balance sheet at the commercial operations date: plant 60,000,000, cash nil,
   debt 42,000,000, equity 18,000,000. Year one: revenue 12,000,000, cash operating costs
   4,500,000, depreciation 2,400,000, interest 2,520,000, tax at 20 % of taxable profit,
   receivables up 900,000, payables up 300,000. Debt service 5,009,635.23 of which principal
   2,489,635.23. The facility requires a six-month DSRA of **2,504,818**, funded from operating
   cash in two equal annual instalments of **1,252,409**, ranking above distributions (Domain 10,
   KA 10.3.3). Distributions are the residual, subject to the 1.20× test.
2. **Formula.** `EBITDA` = revenue − cash costs; `EBIT` = `EBITDA` − depreciation; taxable profit =
   `EBIT` − interest; operating cash flow = net income + depreciation − Δreceivables + Δpayables;
   `CFADS` = operating cash flow + interest paid; distributable cash = `CFADS` − debt service −
   reserve funding; closing cash = operating cash flow − principal − distributions.
3. **Substitution.** `EBITDA` 7,500,000; `EBIT` 5,100,000; taxable profit 2,580,000; tax 516,000;
   net income 2,064,000. Operating cash `2,064,000 + 2,400,000 − 900,000 + 300,000`. `CFADS`
   `3,864,000 + 2,520,000`. Distributions `6,384,000 − 5,009,635.23 − 1,252,408.81`.
4. **Result.** Operating cash flow **3,864,000**; `CFADS` **6,384,000** — reconciling to the
   documented figure exactly; `DSCR` **1.2743**; distributable cash **121,956**.

   | Closing balance sheet | USD | | USD |
   |---|---|---|---|
   | Plant, net | 57,600,000 | Payables | 300,000 |
   | Receivables | 900,000 | Senior debt | 39,510,365 |
   | Cash (restricted, DSRA) | 1,252,409 | Equity | 19,942,044 |
   | **Total assets** | **59,752,409** | **Total** | **59,752,409** |

5. **Interpretation.** The balance sheet balances to the cent, closing cash equals the DSRA
   balance, and `CFADS` ties to the cash-flow statement through the interest line — three
   independent confirmations of internal consistency. What the year says about the project is more
   interesting than any of them. Equity receives **121,956** in its first operating year: **0.68 %**
   of the 18,000,000 contributed, on a project with a healthy `DSCR` and a positive `NPV`. That is
   not distress, it is the ordinary arithmetic of a leveraged project with a reserve to build, and
   it is the number sponsors most reliably fail to model before close — Domain 10's Case study B is
   the same error with a board attached. Three consequences follow. **Reserve funding is a claim on
   early cash, and early cash is all the equity has**: a distribution forecast struck before reserve
   funding is an aspiration. **The residual is where modelling risk concentrates** — the
   distribution's elasticity to revenue is **73.8** against the `CFADS` elasticity of 1.4098, so the
   equity line is about **52 times** more revenue-sensitive than the coverage ratio, and a
   **1.36 %** revenue miss (162,608 of revenue) erases the first-year distribution entirely. And
   **the restricted-cash label is load-bearing**: the 1,252,409 in the DSRA is on the balance sheet,
   is not available to the business, and a liquidity statement showing it as cash is wrong in
   exactly the way that matters.

### 6.2.3 Depreciation, tax allowances and the line most often modelled wrongly

**Definitions.** **Accounting depreciation** allocates the cost of an asset over its useful life
under the applicable financial-reporting framework — Kestrel's 60,000,000 over 25 years, straight
line, **2,400,000** a year. **Tax depreciation**, or capital allowances, is what the tax authority
permits to be deducted, on its own profile and its own base. **Cash tax** is what is actually paid.
They are three different numbers, and only the last enters `CFADS`.

Domain 2 established the principle (KA 2.A.1) and named the standard finding: modelling accounting
tax as if it were cash tax. This is where it is priced.

**Worked example 6.2.3 — what a tax assumption is worth.**

1. **Setup.** Kestrel's base model, following Domain 2, assumes tax depreciation equals accounting
   depreciation — a simplification, chosen for exposition and stated as one. Suppose instead the
   jurisdiction grants **declining-balance capital allowances at 15 %** on the 60,000,000 base,
   with tax losses carried forward without limit. Compute cash tax for the first five operating
   years and the consequences for coverage and debt capacity. (Rates, profiles, loss-carry rules
   and the availability of any allowance are jurisdiction-specific and change; the arithmetic
   below is transferable, the treatment is not, and the treatment is a matter for qualified tax
   advice in the relevant jurisdiction.)
2. **Formula.** Allowance in year `t` = 15 % of the written-down value; taxable profit = `EBITDA` −
   allowance − interest − losses brought forward; cash tax = 20 % of taxable profit if positive,
   otherwise nil with the loss carried forward.
3. **Substitution and result.**

   | Year | Allowance | Interest | Taxable profit | Loss carried forward | Cash tax |
   |---|---|---|---|---|---|
   | 1 | 9,000,000 | 2,520,000 | (4,020,000) | 4,020,000 | **nil** |
   | 2 | 7,650,000 | 2,370,622 | (6,540,622) | 6,540,622 | **nil** |
   | 3 | 6,502,500 | 2,212,281 | (7,755,403) | 7,755,403 | **nil** |
   | 4 | 5,527,125 | 2,044,440 | (7,826,968) | 7,826,968 | **nil** |
   | 5 | 4,698,056 | 1,866,528 | (6,891,552) | 6,891,552 | **nil** |

   Year-one `CFADS` becomes `7,500,000 − nil − 600,000 =` **6,900,000**, a `DSCR` of **1.3773**
   against 1.2743 — **0.1030** of coverage from a tax assumption alone. At Domain 10's 1.30×
   sizing target, debt capacity rises from **41,171,123** to **44,498,864**: an extra
   **3,327,741** of debt, and therefore 3,327,741 less equity.
4. **Interpretation.** The comparison is not between a right and a wrong model but between two
   assumptions about a jurisdiction, and the gap between them is **more than four times the 828,877
   debt-capacity shortfall Domain 10's entire negotiation was fought over**. That is why the tax
   line is both the most commonly mis-modelled row in project finance and the most consequential:
   technically difficult, jurisdiction-specific, liable to change during the loan's life, and
   sitting directly in `CFADS`, so every coverage ratio and sizing calculation inherits it. Four
   disciplines follow. **The tax line traces to a written opinion**, not to a modeller's
   understanding, and the opinion's date and scope go in the assumption register. **Cash tax and
   accounting tax are separate rows**, always, even where they happen to be equal — a model that
   conflates them cannot represent the year in which they diverge. **Tax losses carry in a stated
   balance with a stated expiry rule**, since losses that vanish or persist wrongly misstate cash
   tax for a decade. And **the sponsor case must not bank an allowance regime the lenders will not
   credit**: sizing debt on 44,498,864 because a fifteen-per-cent allowance exists today embeds a
   legislative forecast in a twelve-year loan — a risk for Domain 11's register, not a benefit. The
   honest presentation shows both, states which case governs which decision, and puts the
   3,327,741 of capacity difference in front of the credit committee rather than inside the model.

### AI in this KA

**Where it earns its place.** Building a first-pass construction and operating model from a term
sheet, a spend curve and stated conventions is fast machine work, and so is the surrounding
apparatus: the sources-and-uses skeleton, the loss-carry-forward logic, the articulation checks, and
the assumption register drafted from the model's own input block so that register and model cannot
drift apart.

**Where it must not go.** It must not select the tax treatment. Worked example 6.2.3 shows that
choice moving debt capacity by 3,327,741, and an assistant asked about capital allowances will
produce a fluent, jurisdiction-neutral answer that is nobody's tax opinion. The same prohibition
covers the useful life behind accounting depreciation, whether a cost is capitalised or expensed
(Domain 2, KA 2.3.3), and whether an allowance survives a change of control — advice questions with
named professional owners.

**Verification, concretely.** Recompute one construction quarter and one operating year by hand and
tie all three statements; it is a dozen operations and it catches structural error immediately.
Confirm the implied effective tax rate period by period against the statutory rate and the explained
differences — the check that catches Worked example 6.4.1's failure. Require every tax input to
carry the reference and date of the opinion supporting it, and reject the model, not the input,
where that reference is absent.

### Key terms — KA 6.2

| Term | Meaning |
|---|---|
| **Sources and uses** | The funding requirement against its funding; identical totals by construction. |
| **Interest during construction (IDC)** | Interest accruing before revenue; a funded use, and part of the depreciable base. |
| **Balancing line** | The use that absorbs the residual of a fixed envelope; must be tested against policy. |
| **Articulation** | The three statements derived from one calculation set, so they cannot disagree. |
| **`CFADS` tie** | `CFADS` = operating cash flow + interest paid, where interest is classified as operating. |
| **Capital allowance** | Tax depreciation on the tax authority's profile and base; not accounting depreciation. |
| **Restricted cash** | Reserve balances on the balance sheet that are not available to the business. |

### Sample MCQs — KA 6.2

**MCQ 6.2-A `[6.2.1 · Application]`** Kestrel's committed uses are 48,000,000 of EPC, 3,600,000 of
owner's costs, 1,800,000 of development costs and 840,000 of fees, inside a 60,000,000 envelope,
with capitalised interest computed at 2,114,597. The contingency is:

- A. 3,600,000
- B. 3,645,403 ✅
- C. 5,760,000
- D. 6,000,000

*Rationale:* `60,000,000 − 48,000,000 − 3,600,000 − 1,800,000 − 840,000 − 2,114,597 = 3,645,403`
(6.2.1). A is a plausible round number and therefore the tell-tale of a plugged table; C omits
capitalised interest from the deduction; D applies a 10 % rule of thumb to the envelope instead of
computing the residual.

**MCQ 6.2-B `[6.2.2 · Application]`** Operating cash flow is 3,864,000 and interest paid, included
in operating cash flow, is 2,520,000. `CFADS` is:

- A. 1,344,000
- B. 3,864,000
- C. 6,384,000 ✅
- D. 8,904,000

*Rationale:* `CFADS` = operating cash flow + interest paid (6.2.2). A deducts interest a second
time; B forgets that operating cash flow is already struck after interest; D adds interest a
second time to a `CFADS` already struck before debt service, a figure no definition produces —
and principal, a financing flow, never enters operating cash flow at all.

**MCQ 6.2-C `[6.2.3 · Analysis]`** A model of a project with flat revenue and level annuity debt
service shows cash tax of 516,000 against taxable profit of 2,580,000 in year one and, in year
eight, cash tax of 766,771 against taxable profit of 3,833,856. The statutory rate is 20 %. A
reviewer should conclude:

- A. the tax line is wrong, because cash tax has risen while revenue is flat
- B. the effective rate is 20.0 % in both years, so the tax line is consistent; cash tax rises
  because the interest deduction falls as the loan amortises ✅
- C. the model has omitted deferred tax
- D. the loss carry-forward has been applied incorrectly

*Rationale:* `516,000/2,580,000 = 766,771/3,833,856 = 20.0 %`; the rise is the amortising interest
deduction, the mechanism behind the year-12 minimum of 6.4.1 (6.2.3, KA 6.4.1). A mistakes a
correct behaviour for an error; C and D name plausible defects for which there is no evidence here.

**MCQ 6.2-D `[6.2.3 · Analysis]`** Under 15 % declining-balance allowances Kestrel pays no cash tax
for five years, lifting year-one `DSCR` from 1.2743 to 1.3773 and debt capacity at 1.30× from
41,171,123 to 44,498,864. The sound treatment is:

- A. size the debt at 44,498,864, since the allowance exists
- B. ignore the allowance, since lenders will not credit it
- C. model both, state which case governs which decision, and put the 3,327,741 of capacity
  difference — and the legislative risk in it — in front of the credit committee ✅
- D. average the two capacities

*Rationale:* The difference is an assumption about a jurisdiction over a twelve-year loan, so it is
disclosed and owned rather than banked or suppressed (6.2.3). A embeds a legislative forecast in a
financing; B discards a real economic benefit; D is arithmetic without meaning.

**MCQ 6.2-E `[6.2.1 · Evaluation]`** A comparable water project is modelled exactly as Kestrel was,
inside a fixed 60,000,000 envelope, but with higher committed uses; the balancing contingency solves
to 1,152,000 — **2.4 %** of its 48,000,000 EPC price, against Kestrel's 3,645,403 at 7.59 %. Sources
equal uses to the cent. The soundest recommendation is:

- A. accept the table, since sources equal uses and the identity is satisfied
- B. report that a 60,000,000 envelope does not fund this project on a defensible contingency — a financing conversation about the envelope, not an adjustment to the table ✅
- C. hold contingency at 7.59 % of the EPC price and let capitalised interest become the balancing line instead
- D. present the contingency as 1,200,000, a cleaner figure, with the approximation noted

*Rationale:* the identity is satisfied by construction and therefore proves nothing; what makes the
balancing line informative is testing it against policy, and 2.4 % sits below the band a lender would
expect for this technology and contract structure (6.2.1). A treats an identity as a check. C is the
defensible-looking alternative and is the more dangerous answer: capitalised interest is *computed*
from the drawdown profile, the rate and the interest convention, so making it the plug converts a
derived quantity into an assumption and buries the funding gap in the one line nobody re-derives.
D destroys the single tell that catches a plugged table, since a balancing line is never a round
number.

**MCQ 6.2-F `[6.2.2 · Evaluation]`** A sponsor's investment-committee paper shows first-operating-year
distributions struck as `CFADS` less debt service — 1,374,365 — because the DSRA is described in the
paper as "a balance-sheet item". Modelled through the waterfall the distribution is 121,956, or
0.68 % of the 18,000,000 contributed, after 1,252,409 of reserve funding. The soundest position is
that:
- A. the paper is acceptable, since the reserve is indeed on the balance sheet and is repaid at
  maturity
- B. the paper overstates the first distribution by an order of magnitude: reserve funding ranks
  above distributions, so a distribution forecast struck before it is an aspiration, and the
  1,252,409 held in the DSRA is restricted cash unavailable to the business ✅
- C. the paper is acceptable if a footnote records the reserve requirement
- D. the paper is wrong because it ignores the lock-up test, which is the operative constraint at this
  coverage

*Rationale:* early cash is all the equity has, and the reserve is a claim on it ranking above
distributions (6.2.2, 6.3.2). A and C treat a ranking in the waterfall as a disclosure matter. D
names a real test that does not bite here — at a `DSCR` of 1.2743 the 1.15 × lock-up is not engaged,
so the paper's error is the omitted reserve, not the omitted test; picking the wrong reason is how a
correct objection gets dismissed.

**MCQ 6.2-G `[6.2.3 · Comprehension]`** Kestrel's accounting depreciation is 2,400,000 a year and its
year-one cash tax is 516,000. Accounting depreciation, tax depreciation and cash tax are best
described as:
- A. three names for one charge, presented on different bases
- B. three different numbers — an allocation of cost under the reporting framework, a deduction on the
  tax authority's own base and profile, and the amount actually paid — of which only the last enters
  `CFADS` ✅
- C. three deductions that all enter `CFADS`, at different points in the waterfall
- D. two accounting measures and one forecast, so only the accounting figures are auditable

*Rationale:* only cash actually paid reduces cash available for debt service, which is why cash tax
and accounting tax are separate rows even in years when they are equal (6.2.3). A is the conflation
Domain 2 (KA 2.A.1) names as the standard finding; C puts non-cash charges inside a cash measure;
D invents an audit distinction — every one of the three is evidenced, and the tax line traces to a
dated written opinion.

### Self-check — KA 6.2

1. *State the `CFADS` tie and why it matters.* — `CFADS` = operating cash flow + interest paid;
   it converts `CFADS` from an assertion into a figure tied to a statement.
2. *What does Kestrel's first-year distribution of 121,956 tell a sponsor?* — That reserve funding
   consumes almost all first-year equity cash, and that the residual is far more sensitive to
   revenue than the coverage ratio is.
3. *Why must cash tax and accounting tax be separate rows even when equal?* — Because a model that
   conflates them cannot represent the year in which they diverge, and it will diverge.

---

## Knowledge Area 6.3 — Debt schedules, the cash waterfall and returns

*Topics: 6.3.1 the debt schedule and its circularity · 6.3.2 the waterfall in the model ·
6.3.3 project return and equity return.*

### 6.3.1 The debt schedule and its circularity

**Definition.** The debt schedule is the period-by-period record of opening balance, drawings,
interest, scheduled principal, prepayments and closing balance, for each facility separately. It is
the denominator of every coverage ratio (Domain 10, KA 10.3.3) and therefore load-bearing for
covenant compliance.

Its arithmetic is Domain 3's amortisation table and needs no re-derivation; its **invariants** are
what a modeller owns. The closing balance at maturity is zero; total scheduled principal equals total
drawings; each period's interest equals the balance on which it accrues times the rate for the days
in the period, on the stated convention; and debt service equals interest plus scheduled principal,
tying to the schedule the facility agreement annexes. Kestrel's twelve years satisfy all four:
principal rises from **2,489,635** in year one to **4,726,071** in year twelve, interest falls from
**2,520,000** to **283,564**, the closing balance is **zero**, and the principal column sums to
**42,000,000** exactly.

**Circularity, and the convention that removes it.** Capitalised interest depends on the debt
balance; the balance depends on drawings; drawings include the funding of capitalised interest. In a
workbook that is a circular reference, with three honest resolutions. **Charge interest on the
opening balance** and the circularity disappears, because the period's interest is fixed before its
draw — Worked example 6.2.1's choice, at a cost of 312,957 against the average-balance answer.
**Iterate to convergence**, which is more accurate and needs either the workbook's iterative
calculation or a documented macro. **Solve algebraically**, which for simple structures is best
because it is deterministic. What is not acceptable is an undocumented iteration whose convergence
nobody has tested, or a switch left off: both produce results that depend on the order in which
someone pressed the keys. **The circularity resolution is a documented convention with a named
owner, and the model states which one it uses.**

Two structural features belong in the schedule explicitly rather than as adjustments: a **cash
sweep** applies a defined share of surplus cash to prepayment, so the schedule carries a prepayment
row that changes future interest; and **sculpted** service (Domain 10, KA 10.1.3) sets each period's
debt service from that period's `CFADS`, which is itself circular through the tax deduction — the
same three resolutions apply, and the same requirement to say which.

### 6.3.2 The waterfall in the model

**Definition.** The cash waterfall is the contractual priority order in which each period's cash
is applied. In a model it is a column of sequential deductions, each with a **test** and each
producing a balance that can be nil but never negative.

Kestrel's order, per Domain 10 KA 10.3.3: cash operating costs, then cash tax, then **senior debt
service**, then **reserve funding and top-ups**, then subordinated debt if any, then — only if every
distribution test passes — **distributions to equity**. Three requirements make the difference
between a waterfall and a subtraction.

**Every tier is floored at zero and the shortfall is carried, not lost.** A tier that goes negative
has silently netted its shortfall against a later one, which is the defect that makes a distressed
case look survivable. The correct construction pays what is available, records the shortfall, and
applies the documented consequence — reserve drawing, lock-up, event of default.

**Tests are modelled, not assumed.** The distribution test is a formula with a threshold, so the
model must show cash *trapped* when it fails. Kestrel's lock-up bites at a `DSCR` of 1.15×, a
`CFADS` of **5,761,081**; a model whose distribution row is simply the residual after debt service
has modelled an arithmetic identity, not a facility.

**Reserve movements are two-directional and ranked.** Funding a reserve is a use above
distributions; drawing on it is a source; releasing it at maturity is a source. Kestrel's DSRA is
funded at 1,252,409 in each of the first two operating years, sits at 2,504,818 for the rest of the
loan, and **releases in year twelve** — which is why year twelve's distribution is 3,431,895 against
year eleven's 980,580, and why a model that omits the release understates the equity return without
touching any coverage ratio.

### 6.3.3 Project return and equity return

**Definitions.** The **project return** values the asset before financing: unlevered post-tax free
cash flow against total capital cost. The **equity return** values the sponsors' position: equity
drawn during construction against distributions actually received after every prior claim in the
waterfall. They answer different questions and are routinely conflated.

**Worked example 6.3.3 — what Kestrel actually returns, to whom, and when.**

1. **Setup.** The master model on the **bank case**: revenue flat at 12,000,000, cash tax computed
   on the real interest schedule so that `CFADS` declines as the interest deduction amortises,
   DSRA funded over two years and released in year twelve. Equity is drawn 30 % of each period's
   funding requirement — **8,203,951** in construction year one and **9,796,049** in year two.
   Compare with the **sponsor case**, identical but with revenue, cash costs and working capital
   escalating at the 2.967 % of Worked example 6.1.3.
2. **Formula.** Unlevered free cash flow = `EBITDA` − 20 % × (`EBITDA` − depreciation) − Δworking
   capital; project `NPV` = `PV` at 8 % − 60,000,000; project `IRR` solves `NPV` = 0.
   Distribution in period `t` = `CFADS(t)` − debt service − reserve funding + reserve release;
   equity `IRR` solves the present value of draws and distributions to zero.
3. **Substitution.** Unlevered free cash flow `7,500,000 − 0.20 × 5,100,000 − 600,000 = 5,880,000`,
   level for 25 years; `× AF(0.08, 25) = 5,880,000 × 10.674776`. Equity flows: −8,203,951, −9,796,049,
   then 121,956 · 92,080 · 1,312,821 · … · 980,580 · 3,431,895, then 5,880,000 for thirteen years.
4. **Result.**

   | Measure | Bank case | Sponsor case |
   |---|---|---|
   | Project (unlevered, post-tax) `NPV` at 8 % | **+2,767,684** | **+19,875,251** |
   | Project `IRR` (unlevered, post-tax) | **8.54 %** | — |
   | Equity `IRR` | **9.83 %** | **13.52 %** |
   | Total distributions over 25 years | **90,507,502** | **151,536,729** |
   | Money multiple on 18,000,000 | **5.028×** | **8.419×** |
   | Equity payback from financial close | **14.67 years** | — |

5. **Interpretation.** Set these beside Domain 4's appraisal — `IRR` **12.19 %**, discounted payback
   **10.07 years** — and the gap is the lesson. On a post-tax, twenty-five-year, flat-revenue basis
   the *asset* earns **8.54 %**, barely above the 8 % hurdle; the *equity* earns **9.83 %**, and the
   difference is leverage doing what leverage does. Domain 4's 12.19 % is neither: a pre-tax return
   over a fifteen-year horizon, sitting above both. Anyone who remembers "the project returns 12 %"
   has remembered a number describing no party's position. **Leverage pushes equity cash to the far
   side of the loan.** The 14.67-year equity payback against Domain 4's 10.07-year discounted
   payback is a different question, not a contradiction: equity waits for the loan to be repaid and
   the reserve released, and the first two operating years return **121,956** and **92,080** on
   18,000,000 contributed. A sponsor whose investment committee expects distributions in year one
   has misread the waterfall, not the project. **The case, not the asset, produces most of the
   return.** The 3.69-point difference between 9.83 % and 13.52 % comes entirely from a 2.967 %
   escalation assumption — which is why escalation is contested in offtake negotiation (Domain 7)
   and why an equity `IRR` without its case is worthless. The habit that follows: state the
   escalation at which the equity return meets the sponsor's hurdle, and ask whether it is
   contracted, indexed to a published index, or hoped for. Finally, **the multiple and the rate tell
   different stories on purpose** — 5.028× over twenty-five years *is* 9.83 % a year, and a board
   shown only the multiple has been shown a long horizon dressed as a return.

### AI in this KA

**Where it earns its place.** Solving a sculpted schedule, testing a cash sweep across hundreds of
cases and computing rates of return over a scenario grid is root-finding and iteration at volume —
work at which a human is slow and no more accurate. Generating the waterfall's test logic from the
drafted priority order and then attacking that logic with adversarial cash flows is equally strong:
the failure cases are enumerable, and a machine will enumerate them.

**Where it must not go.** It must not decide the waterfall's order or the tests' thresholds, which
are terms of the finance documents, read by lawyers rather than inferred from a model. It must not
choose the circularity resolution silently — an assistant picks whichever converges, and Worked
example 6.2.1 prices that choice. And it must not be the source of a quoted return: an equity `IRR`
is a number with a case attached, and the case is a governance object.

**Verification, concretely.** Substitute any machine-produced `IRR` back into the present-value
equation and confirm it returns zero (Domain 4, KA 4.A.5 — the cheapest audit that exists). Run the
waterfall with a `CFADS` low enough to fail every tier and confirm that no tier goes negative, that
the shortfall is carried, and that the lock-up traps cash rather than the model netting it away.
Re-derive one period's interest and principal by hand and tie to the annexed schedule. And require
the tool to state, in words, which case it ran.

### Key terms — KA 6.3

| Term | Meaning |
|---|---|
| **Debt schedule invariants** | Closing balance nil at maturity; Σ principal = drawings; interest ties to balance, rate and days. |
| **Circularity** | Interest depends on balance depends on drawings depends on interest; resolved by convention, iteration or algebra. |
| **Waterfall tier** | A ranked deduction with a test, floored at zero, with the shortfall carried. |
| **Reserve release** | The return of a funded reserve at maturity; a source that lifts the final period's distribution. |
| **Project return** | Unlevered post-tax cash against total capital cost; the asset's return. |
| **Equity return** | Draws against distributions after every prior claim; the sponsors' return. |
| **Money multiple** | Total distributions ÷ equity contributed; a horizon-blind measure. |

### Sample MCQs — KA 6.3

**MCQ 6.3-A `[6.3.3 · Analysis]`** Kestrel's unlevered post-tax `IRR` is 8.54 %, its bank-case
equity `IRR` is 9.83 %, and Domain 4's appraisal reported 12.19 %. The correct reading is:

- A. the appraisal was wrong by 3.65 points
- B. the asset earns 8.54 %, the equity earns 9.83 % because of leverage, and 12.19 % is a pre-tax
  fifteen-year figure describing neither party's position ✅
- C. the equity return should exceed the project return by the debt margin
- D. the three should be equal once tax is removed

*Rationale:* Each measure is correct on its own basis and horizon; conflating them is the defect
(6.3.3, 6.1.3). C invents a relationship — the gap depends on gearing, tenor and the shape of
distributions; D is false, since horizon alone separates them.

**MCQ 6.3-B `[6.3.2 · Application]`** Kestrel's year-twelve distribution is 3,431,895 against year
eleven's 980,580, on `CFADS` that falls between the two years. The explanation is:

- A. a cash sweep
- B. the DSRA of 2,504,818 releasing at final repayment ✅
- C. an error, since `CFADS` fell
- D. the final principal instalment being smaller

*Rationale:* The reserve is released when the debt it secures is repaid, a source in the waterfall
(6.3.2). C mistakes a modelled contractual event for an inconsistency; D is the opposite of the
truth — year twelve's principal, 4,726,071, is the largest of the twelve.

**MCQ 6.3-C `[6.3.1 · Analysis]`** A construction model charges interest on the average debt
balance and the workbook's iterative calculation is switched off. The consequence is:

- A. interest is understated by a known amount
- B. the model returns a stale or unconverged figure whose value depends on calculation order, so
  the result is not reproducible ✅
- C. the model will not open
- D. capitalised interest becomes zero

*Rationale:* Average-balance interest is genuinely circular; without a resolution the answer is
whatever the last pass left behind (6.3.1). A describes the *opening*-balance convention, which is
deliberate and quantified at 312,957 on Kestrel; C and D are not how circular references behave.

**MCQ 6.3-D `[6.3.3 · Application]`** Kestrel returns 90,507,502 of distributions on 18,000,000 of
equity over 25 years — a 5.028× multiple — at an equity `IRR` of 9.83 %. A board shown only the
multiple has been shown:

- A. a complete picture, since the multiple includes every distribution
- B. a long horizon dressed as a return, because the multiple is blind to when the cash arrives ✅
- C. an understatement, since multiples ignore reinvestment
- D. the same information as the `IRR`

*Rationale:* A multiple has no time dimension; 5.028× over 25 years is 9.83 % a year (6.3.3, and
Domain 4's insistence that rates and ratios explain rather than decide). C reverses the bias; D is
false — the same multiple over ten years would be a materially higher rate.

**MCQ 6.3-E `[6.3.3 · Comprehension]`** An investment committee member asks why one project has two
reported returns, and which of them is the real one. The best restatement is:

- A. the project return is struck before tax and the equity return after it
- B. they answer different questions — the project return values the asset before financing, the equity return values the sponsors' position after every prior claim in the waterfall — so both are real and neither substitutes for the other ✅
- C. the equity return is the project return plus the debt margin
- D. the project return is the lenders' return and the equity return the sponsors'

*Rationale:* the distinction is *whose* cash is being measured and after which claims, not a basis
difference: on Kestrel both figures are post-tax, and the 8.54 % and 9.83 % differ because leverage
reorders the cash (6.3.3). A names a real labelling axis (6.1.3) that is not this one; C invents an
arithmetic relationship, when the gap depends on gearing, tenor and the shape of distributions;
D miscasts the project return, which measures the asset and not any lender, whose return is its
margin.

**MCQ 6.3-F `[6.3.1 · Evaluation]`** A construction model charges interest on the average debt
balance and resolves the resulting circularity by iteration, with no stated convergence criterion and
no named owner. On Kestrel that convention is worth 2,427,554 of capitalised interest against
2,114,597 on the opening-balance convention — a difference of 312,957. The reviewer's best
recommendation is:

- A. switch to opening-balance interest, which removes the circularity outright and is the convention the master model uses
- B. keep the average-balance convention but require the resolution to be documented, with a tested convergence criterion and a named owner, because the defect is the undocumented resolution rather than the convention ✅
- C. solve the interest algebraically instead, since a deterministic solution is always preferable
- D. correct the 312,957, which is an overstatement of interest

*Rationale:* all three resolutions in 6.3.1 are honest, and average-balance interest is the more
accurate measure of what the facility will actually charge; what is unacceptable is a resolution
whose convergence nobody has tested, because the printed answer then depends on the order in which
somebody pressed the keys. A is genuinely defensible and would restore reproducibility, but it buys
it by surrendering 312,957 of correctly measured interest when documentation delivers both. C is also
defensible and is the better answer for simple structures, but it is a rebuild rather than a control
and is not available for a sculpted or swept schedule. D is wrong: 312,957 is the priced cost of a
convention, not an error.

**MCQ 6.3-G `[6.3.3 · Evaluation]`** A board paper presents Kestrel's equity return as "5.028 times
money, 13.52 % `IRR`", naming no case: the multiple is the **bank** case's, over the whole
twenty-five-year concession, while the `IRR` is the **sponsor** case's, which a 2.967 % escalation
assumption alone lifts 3.69 points above the bank case's 9.83 %. The soundest
presentation:
- A. leads with the 5.028 times multiple, because it counts every dollar actually distributed
- B. reports the sponsor case, since it is the sponsors' central expectation and they are the investor
- C. reports both cases with their labels, states the horizon the multiple covers, and states whether
  the 2.967 % escalation is contracted, indexed to a published index or merely assumed ✅
- D. reports the bank case alone, because that is the case the lenders underwrite

*Rationale:* a multiple has no time dimension, so 5.028 times says nothing about when the cash arrives
and a multiple shown without its horizon is a long horizon dressed as a return, while an equity `IRR`
without its case is worthless — and pairing one case's multiple with another's `IRR` is a figure that
describes no case at all (6.3.3). A does exactly that; B presents the more flattering case unlabelled,
and would have to carry the sponsor case's own multiple of 8.419 times rather than the 5.028 the paper
quotes, when the habit the
domain prescribes is to state the escalation at which the return meets the hurdle and ask whether it
is a right or a hope; D discards the sponsors' own economics, which is the case the equity decision
turns on.

### Self-check — KA 6.3

1. *Name the four debt-schedule invariants.* — Closing balance nil at maturity; Σ principal =
   drawings; interest ties to balance, rate and day count; debt service = interest + scheduled
   principal, tying to the annexed schedule.
2. *Why must a waterfall tier never go negative?* — A negative tier has silently netted a shortfall
   against a later claim, which makes a distressed case look survivable.
3. *State Kestrel's first two operating-year distributions and what they teach.* — 121,956 and
   92,080 on 18,000,000: reserve funding consumes almost all early equity cash, so distribution
   forecasts must be struck after reserve funding and lock-up tests.

---

## Knowledge Area 6.4 — Checks, sensitivity, model audit and AI controls

*Topics: 6.4.1 the check block and the six invariants · 6.4.2 sensitivity, scenario and the
breakeven translation · 6.4.3 model audit, governance and its economics · 6.4.4 AI-assisted
modelling controls.*

### 6.4.1 The check block and the six invariants

**Definition.** A **check block** is a dedicated output area in which every arithmetic invariant the
model must satisfy is computed as a difference that should be nil, with a single aggregate flag
that turns red if any check fails. It is built first, not last, and no output is quoted while it is
red.

The six invariants a project model must satisfy:

| # | Invariant | What its failure localises |
|---|---|---|
| 1 | The balance sheet balances, every period | An omitted flow, or a flow posted once |
| 2 | Sources equal uses, every construction period | A funding gap or a double-funded use |
| 3 | Closing debt is nil at maturity, for every facility | A schedule error or a missing prepayment |
| 4 | Σ scheduled principal = total drawings | A rounding accumulation or a mis-specified profile |
| 5 | Cash is never negative in any period or account | A liquidity failure the model has netted away |
| 6 | `CFADS` = operating cash flow + interest paid | A `CFADS` line built outside the statements |

To these belong three ratio-level checks inherited from Domain 10 (KA 10.A.3): `LLCR` = `DSCR`
where cash is level and service is an annuity at the loan rate; `PLCR` ≥ `LLCR` wherever a tail
exists; and the **minimum** `DSCR` over the loan life reported alongside the average. And one that
belongs to this domain: the **implied effective tax rate** reconciles, period by period, to the
statutory rate plus explained differences.

**Worked example 6.4.1 — the error that balances perfectly.**

1. **Setup.** Kestrel's first operating year. A modeller computes tax as 20 % of `EBIT` rather than
   20 % of taxable profit — a one-cell error, omitting the interest deduction. Every other formula
   is correct and consistent. Trace the consequences and identify which check catches it.
2. **Formula.** Correct: tax = 20 % × (`EBIT` − interest). As modelled: tax = 20 % × `EBIT`. Then
   propagate through net income, operating cash flow, `CFADS`, `DSCR`, distributable cash, the
   balance sheet and debt capacity at Domain 10's 1.30× target.
3. **Substitution.** Tax `0.20 × 5,100,000 = 1,020,000` against the correct 516,000; net income
   `2,580,000 − 1,020,000 = 1,560,000`; operating cash flow `1,560,000 + 2,400,000 − 600,000`;
   `CFADS` `3,360,000 + 2,520,000`.
4. **Result.** `CFADS` **5,880,000** against 6,384,000; `DSCR` **1.1737** against 1.2743 — **below
   the 1.20× covenant**; distributable cash **−382,044**, so the DSRA cannot be funded on
   schedule; debt capacity at 1.30× falls from **41,171,123** to **37,920,771**, a shortfall of
   **3,250,352**. And the balance sheet **balances to the cent**, because the error is propagated
   consistently: net income is lower, equity is lower by the same amount, and assets tie.
5. **Interpretation.** This is the single most important result in the domain, and it is a negative
   one: **invariant 1 is necessary and not sufficient.** A consistent wrong number balances
   perfectly, and a modeller who reports "the model balances" has reported nothing about accuracy.
   What catches this error is the **effective-rate check**: `1,020,000/2,580,000 =` **39.53 %**
   against a statutory 20 %, a discrepancy no reader can rationalise. The negative distributable
   cash catches it too, through invariant 5 — but only if the waterfall is floored rather than
   allowed to run negative, which is precisely why 6.3.2 insists on the floor. The consequences
   scale badly in both directions. Understated, as here, the model reports a covenant breach that
   does not exist, kills 3,250,352 of debt capacity, and sends the sponsors to find equity they do
   not need. Substituting 3,250,352 of equity for debt at a 6-point spread (a 12 % equity
   requirement against the 6.0 % achieved on debt) costs 195,021 a year, a present value of
   **1,469,694** over the twelve years at 8 %. Overstated, the same class of error sizes debt the
   project cannot service, which is Worked example 6.4.3's illustrative error. The discipline is
   therefore not "check that it balances" but **"know what each check can and cannot see"**: the
   six invariants test structure, the effective-rate and ratio checks test economics, and only a
   reconciliation to the documents tests definitions (Domain 10, Toolkit 10.T.1).

**The other half of the check block: the minimum, not the average.** Domain 10 held Kestrel's
`CFADS` level "for this illustration" and derived `DSCR` = `LLCR` = 1.2743. The modelled profile
does not behave that way, and the reason is 6.2.3's: as the loan amortises the interest deduction
shrinks, cash tax rises, and `CFADS` falls against a level instalment.

**Worked example 6.4.1b — the minimum the level line hides.**

1. **Setup.** Kestrel's bank case, revenue flat at 12,000,000, cash tax computed on the actual
   interest schedule. Compute `DSCR` for each of the twelve loan years and compare with the level
   assumption.
2. **Formula.** `CFADS(t) = 7,500,000 − 0.20 × (5,100,000 − interest(t)) − 600,000`;
   `DSCR(t) = CFADS(t)/5,009,635.23`.
3. **Substitution.** Year 1: interest 2,520,000, cash tax 516,000, `CFADS` 6,384,000. Year 12:
   interest 283,564, cash tax 963,287, `CFADS` 5,936,713.
4. **Result.** `DSCR` falls monotonically from **1.2743** in year one to **1.1851** in year twelve.
   The **minimum is 1.1851 — a breach of the 1.20× covenant** — and the average over the loan life
   is **1.2340**, comfortably above it. Present values move too: `PV` of the modelled `CFADS` at
   8 % over 25 years is **65,315,883** against **68,147,771** on the level assumption, so the
   level line overstates value by **2,831,888**, or **4.34 %**. `LLCR` becomes **1.2395** and
   `PLCR` **1.8555**, against Domain 10's level-case 1.2743 and 1.9431 — the identity breaks, as
   Domain 10 said it must when cash is not level.
5. **Interpretation.** A model that holds `CFADS` level reports 1.2743 for twelve years and never
   shows the year in which the project breaches. The mechanism is unglamorous and entirely
   predictable — the interest tax shield amortises with the loan — and it is missed constantly
   because it requires the tax line to be modelled period by period rather than assumed. Three
   professional consequences. **Report the minimum and the year it occurs**, which is Domain 10's
   rule now earned rather than asserted: the average of 1.2340 passes and the project still
   breaches. **The escalation assumption decides where the minimum lies.** On the sponsor case,
   with revenue escalating at 2.967 %, `DSCR` *rises* from 1.2743 to **1.5940** and the minimum is
   year one — which is why lenders insist on the flat case: not because they believe it, but
   because it is the case that finds this defect. **And the fix belongs to sizing, not
   reporting.** At Domain 10's properly sized **41,171,123** the instalment is 4,910,769 and the
   year-twelve `DSCR` is **1.2087** — still inside the covenant. The debt level at which the
   year-twelve minimum is exactly 1.20× is **41,472,081**, so the structure needed **527,919** less
   debt than the 42,000,000 requested. Domain 10's 828,877 gap, argued on year-one coverage, was
   already protecting against a year-twelve problem nobody in that negotiation had modelled. That
   is the most valuable thing a properly built model does: it makes a conservative constraint
   *legible* as the specific risk it was guarding.

> **Fig 6.4.1 — The minimum coverage a level line hides.** Line chart, x-axis loan years 1–12,
> y-axis `DSCR` 1.10–1.65. Three series, all starting at **1.2743**: dashed slate horizontal at
> 1.2743 (the level assumption); ink line falling monotonically to **1.1851** in year 12 (bank
> case — flat revenue, cash tax on the actual interest schedule), with a crimson marker and
> "1.1851 — breach"; brand-blue line rising to **1.5940** (sponsor case, 2.967 % escalation).
> Crimson dashed horizontal at the **1.20× covenant**. Footer: average `DSCR` 1.2340 on the bank
> case, and the note that the average is not the number the covenant tests. Source: PCI original.
> Alt text: three coverage-ratio lines from a common starting point, one flat, one declining below
> the covenant threshold in the final loan year, one rising well above it.

### 6.4.2 Sensitivity, scenario and the breakeven translation

**Definitions.** **Sensitivity analysis** moves one input at a time and records the effect on
outputs. **Scenario analysis** moves a coherent set of inputs together to represent a state of the
world. **Elasticity** expresses a sensitivity as a ratio: the percentage change in an output per
one per cent change in an input, so that inputs measured in different units can be ranked.

**Worked example 6.4.2 — Kestrel's sensitivity table, honestly labelled.**

1. **Setup.** Two cases run in parallel: the **sponsor case** (2.967 % escalation, `NPV`
   **+19,875,251**) and the **bank case** (flat, `NPV` **+2,767,684**). `NPV` is unlevered,
   post-tax, over 25 years at 8 %; `DSCR` figures are from the bank case with cash tax on the
   actual interest schedule. Each input is moved ±10 %; the capital-cost case assumes the overrun
   is funded by equity, the lenders' commitment being fixed.
2. **Formula.** Elasticity = `(ΔNPV/NPV) ÷ (Δinput/input)`.
3. **Result.**

   | Input | Sponsor `NPV` at −10 % / +10 % | Elasticity | Bank `NPV` at −10 % / +10 % | Elasticity | Bank `DSCR` yr 1 at +10 % | Bank minimum `DSCR` at +10 % |
   |---|---|---|---|---|---|---|
   | Revenue | 7,416,691 / 32,333,810 | **6.27** | −6,839,615 / 12,374,983 | **34.71** | 1.4540 | 1.3647 |
   | Cash operating cost | 24,858,674 / 14,891,827 | **−2.51** | 6,610,603 / −1,075,235 | **−13.88** | 1.2025 | 1.1132 |
   | Capital cost | 25,362,861 / 14,387,640 | **−2.76** | 8,255,295 / −2,719,927 | **−19.83** | 1.2839 | 1.1946 |
   | Interest rate | 19,875,251 / 19,875,251 | **0.00** | 2,767,684 / 2,767,684 | **0.00** | 1.2432 | 1.1485 |
   | Tax rate | 21,439,288 / 18,311,213 | **−0.79** | 3,856,511 / 1,678,857 | **−3.93** | 1.2640 | 1.1658 |
   | Escalation | 17,852,671 / 21,978,907 | **1.02 / 1.06** | not applicable | — | 1.2743 | 1.1851 |
   | Discount rate | 26,469,750 / 14,022,003 | **−3.32 / −2.94** | 7,305,980 / −1,294,646 | **−16.40 / −14.68** | 1.2743 | 1.1851 |

4. **Interpretation.** Five readings, in order of what a practitioner is paid for.

   **Revenue dominates, and operating leverage is why.** An elasticity of 6.27 means a one per cent
   revenue miss costs 6.27 % of `NPV`: cash operating costs are largely fixed, so `CFADS` amplifies
   revenue at an elasticity of **1.4098**, and the discount factors and the fixed depreciation
   shield do the rest. Diligence effort should be allocated in the order of this column, which is
   the table's real purpose.

   **Elasticity is a property of the case, not of the project.** The same revenue move is 6.27 on
   the sponsor case and **34.71** on the bank case, because the bank case's base `NPV` is thin. A
   tornado drawn on a marginal case looks terrifying and one drawn on a generous case looks
   reassuring, for the same asset. Never compare elasticities across cases; always label the case.

   **The interest-rate row is the most instructive line in the table.** Unlevered `NPV` does not move
   at all — elasticity **0.00** — because financing does not change the asset's cash. But the
   year-one `DSCR` falls to **1.2432** and the minimum to **1.1485**, below the lock-up as well as
   the covenant. A table reporting only `NPV` would rank interest rate last among Kestrel's risks.
   **Sensitivity must be run on the outputs the decision turns on**, and for a financing that means
   coverage, not only value.

   **The capital-cost row is counterintuitive.** A 10 % overrun *raises* the year-one `DSCR` to
   **1.2839**, because the depreciation shield grows with the asset base and cash tax falls while
   the instalment is unchanged; `NPV` falls sharply, as it must. A model showing only coverage would
   report an overrun as good news — which is why no single output is a control.

   **The asymmetry in the discount-rate row is convexity, not error.** −3.32 downward against −2.94
   upward is the curvature of the present-value function (Domain 4, KA 4.A.1); a symmetric
   elasticity reported for a discount rate means somebody has linearised.

**What one-at-a-time cannot see — stated precisely, because it is usually stated wrongly.** The
common objection is that one-at-a-time "misses interactions". On Kestrel it does not: the model is
linear in revenue and cash cost, so a joint move of revenue −10 % and cash cost −5 % gives an `NPV`
of **−4,918,155**, *exactly* the sum of the two individual effects, to the cent. What it genuinely
misses is more dangerous. **It says nothing about correlation** — the joint probability of two moves
occurring together. Revenue and cost are commonly indexed to related measures, so the realistic
downside is a correlated bundle rather than "revenue −10 %", and one-at-a-time never assigns it a
likelihood; that is what scenarios and, where the exposure justifies the parameter work,
probabilistic simulation are for. **And it breaks at thresholds.** At revenue −10 % Kestrel's
year-one `DSCR` is **1.0947**, below both the 1.20× covenant and the 1.15× lock-up, so distributions
are trapped and the equity return collapses by far more than the `NPV` column suggests.
Discontinuities — breach, lock-up, cash sweep, tax losses starting or exhausting, a reserve
emptying — are where linear intuition fails and a sensitivity table must be replaced by a case.

**The breakeven translation.** A ratio conveys no magnitude; an input level does. Converting each
threshold into the input that crosses it is the most useful single page a model produces:

| Threshold | `CFADS` | Revenue | Fall from base |
|---|---|---|---|
| 1.30× sizing target | 6,512,526 | 12,171,368 | revenue must **rise 1.43 %** |
| 1.20× covenant | 6,011,562 | 11,503,416 | **4.14 %** |
| 1.15× lock-up | 5,761,081 | 11,169,441 | **6.92 %** |
| 1.00× (payment fails) | 5,009,635 | 10,167,514 | **15.27 %** |

Two of those lines change a conversation. Domain 10 reported covenant headroom as **372,438** of
annual cash, **5.83 %** of `CFADS`; in revenue terms the headroom is only **4.14 %**, because
`CFADS` amplifies revenue by 1.4098. Quoting headroom as a percentage of `CFADS` to an operations
team that manages revenue overstates their room by 41 %. And the first line closes the loop with
Domain 10's negotiation: revenue would have to be **1.43 % higher** for the requested 42,000,000
to deliver the 1.30× the credit committee wanted. On the cost side the covenant breaks at cash
operating costs of **4,965,547**, **10.35 %** above base, and on the financing side at an interest
rate of **7.48 %** against the 6.0 % achieved.

### 6.4.3 Model audit, governance and its economics

**Definition.** A **model audit** is an independent review of a financial model against the
transaction documents, the model's own logic and its arithmetic, reported as findings by severity.
It is a standard condition precedent in limited-recourse financing (Domain 13, KA 13.2) and it is
the lenders' control, not the sponsor's convenience.

**Model governance** is the surrounding apparatus that makes the audit possible and its findings
durable: a **version register** (one authoritative file, a version number in the file name and on
every printed page, a change log naming the author and the reason); **input provenance** (every
external input traced to a source document with its version and date, so that a superseded traffic
forecast cannot be silently retained — Case study B); **input lock and change control** after a
declared freeze; **a named model owner** and a named reviewer per version; and a **regression
suite** of golden answers that the model must still reproduce after any edit.

**The economics: is the audit worth its fee and its elapsed time?** The shape is Domain 3's gate
economics from PML-AI (PML-AI KA 3.3.1 — reviewed here in the same form, on financing rather than
delivery parameters), and the answer is more interesting than "yes".

**Worked example 6.4.3 — the model audit priced.**

1. **Setup.** Illustrative parameters an organisation must estimate from its own record; the
   arithmetic, not the parameters, is transferable. Probability that an unaudited model of this
   class carries a **material** error, `p` = **0.35**. Probability the audit detects it,
   `d` = **0.85**. Audit fee **180,000**. The audit adds **two weeks** to financial close; at
   Kestrel's forgone `CFADS` of **17,733.33** a day on a 30/360 basis (Domain 5, KA 5.4.2, where
   the concession's expiry is fixed so a slip shortens operations), that elapsed cost is
   **248,267**. Correcting a detected error pre-close costs 60,000 of model rework plus one further
   week, **184,133**. The illustrative material error is an **omitted 500,000 annual maintenance
   provision**: true year-one `CFADS` **5,984,000**, a `DSCR` of **1.1945**, and debt capacity at
   1.30× of **38,591,479** against 42,000,000 signed — a resize of **3,408,521**.
2. **Formula.** Cost if the error reaches close = amendment fee + advisers + duress premium on the
   resized equity + reopening delay. Expected cost without audit = `p × C`. Expected cost with
   audit = fee + elapsed + `p × [d ×` correction `+ (1 − d) × C]`. Net value = the difference.
3. **Substitution.** `C` = amendment fee at 0.30 % of 42,000,000 = 126,000, plus advisers 320,000,
   plus a 200-basis-point duress premium on 3,408,521 of equity raised at short notice for twelve
   years (`3,408,521 × 0.02 × AF(0.08, 12) =` 513,738), plus a ten-week reopening at 24,733.33 a
   day (forgone `CFADS` plus interest on drawn debt, Domain 5's daily figure) = 1,731,333. Without
   audit `0.35 × 2,691,071`. With audit
   `180,000 + 248,267 + 0.35 × [0.85 × 184,133 + 0.15 × 2,691,071]`.
4. **Result.** `C` = **2,691,071**. Expected cost without the audit **941,875**; with it
   **624,328**. **The audit is worth 317,547.** Its **breakeven fee** is **497,547** — **2.76×**
   what it costs. Its **breakeven error rate** is `p*` = **20.10 %**: below that, the audit
   destroys value. Its **breakeven detection rate** is `d*` = **48.81 %**.
5. **Interpretation.** The headline is not 317,547 but what the breakevens do to the argument.
   **Elapsed time, not fee, decides whether the audit pays.** Run the same audit *early*, in
   parallel with other conditions precedent so that it adds no time to close and a detected error
   costs only its 60,000 of rework, and the net value nearly doubles to **602,744** while the
   breakeven error rate collapses from 20.10 % to **8.05 %**. The fee is 180,000 and the elapsed
   cost is 248,267: **the delay costs more than the auditor.** A leader arguing about the fee is
   arguing about the smaller number, and the negotiation that matters is the timetable. **A weak
   audit is worse than none**, because below `d*` = 48.81 % it consumes the fee and the delay and
   returns less than it costs — so the auditor's competence on *this asset class and this
   jurisdiction's tax* is a commercial question, not a procurement formality. **And the honest
   caveat sits in `p`.** An organisation whose material-error rate is genuinely below 20 % cannot
   justify a late audit on expected value alone, and should not pretend otherwise; what justifies
   it anyway is that the audit is the lenders' condition precedent, that the distribution of `C` has
   a long tail this arithmetic averages away, and that the 20.10 % breakeven is itself an estimate
   nobody has ever tested on a sample of one project. The defensible position is therefore: audit
   early, audit competently, and use the arithmetic to argue about *timing and scope* rather than
   about whether to audit at all.

### 6.4.4 AI-assisted modelling controls

**The specific opportunity.** Four uses are strong enough to change how the work is staffed.
**Formula and structure scanning** at whole-workbook scale — embedded constants, mid-row formula
changes, inconsistent references, orphaned cells, flag arithmetic: the 6.1.1 rules, enforced
mechanically. **Version diffing** into a human-readable change list, which is what makes a change
log trustworthy rather than aspirational. **Adversarial test generation**: the cases that break a
waterfall — a `CFADS` failing every tier, a reserve emptying, tax losses exhausting mid-loan — which
humans under time pressure reliably fail to imagine. And **documentation from the model**, drafting
the assumption register from the input block so the two cannot drift apart.

**The specific prohibitions.** No AI output is a tax treatment, an accounting policy, a covenant
definition, a waterfall order or a legal consequence — each has a named professional owner and a
document behind it. No AI-generated case becomes a reported case without a human declaring its
basis, horizon and assumptions. No AI edit reaches the authoritative file without appearing in the
version diff and the change log with a named human author. And nothing produced by a model whose
check block is red is quoted, whatever produced it.

**The controls that make this auditable**, in the order they bind: a **golden-answer regression
suite** — this domain's verified figures (`CFADS` 6,384,000; `DSCR` 1.2743; minimum `DSCR` 1.1851 in
year twelve; IDC 2,114,597; contingency 3,645,403; closing balance sheet 59,752,409) rerun after
every edit, machine or human, with any change explained before it is accepted; **provenance for
every input**, source document and version, because Case study B's failure was a paste and not a
formula; **a named human owner per model version**; **sampled manual recomputation** of one
construction period and one operating year per version; and **a stated verification sample size**
for any machine-produced finding list, every finding confirmed by a human before it is reported.

**The arithmetic of an AI pre-check, and why it does not settle the question.** Suppose a machine
scan costs **8,000**, adds no elapsed time and detects **55 %** of material errors. On Worked
example 6.4.3's parameters its net value is **498,481** — above the late audit's 317,547, purely
because it consumes no time. As a pre-check before an early audit it cuts the residual error
probability from 0.35 to **0.1575** and yields **670,716**, the best of the four options.

| Control | Expected cost | Net value |
|---|---|---|
| Nothing | 941,875 | — |
| Late model audit (2 weeks of delay) | 624,328 | **317,547** |
| AI pre-check alone | 443,394 | **498,481** |
| Early model audit | 339,131 | **602,744** |
| AI pre-check + early model audit | 271,159 | **670,716** |

And here the arithmetic must be allowed to lose an argument. Expected value ranks the pre-check above
the late audit, and a careless reader would conclude that a tool can replace a review. Three reasons
it cannot, all about the *parameters* rather than the sums. The audit is a condition precedent; a
sponsor does not get to substitute for it. The detection rate `d` is not constant across error
types: a scanner finds structural defects and systematically misses the definitional ones — whether
`CFADS` matches the facility's clause, whether the tax treatment is the jurisdiction's, whether the
waterfall follows the drafted priority — which are precisely the errors carrying the largest `C`, so
treating `d` as independent of error class is itself a model error, and one that biases the
comparison toward the cheap control. And nobody has validated 0.55 on this asset class at all. The
conclusion the table supports and the arithmetic alone does not: **machine checks are additive to
review and never substitutive**, and the honest use of these numbers is to fund the pre-check *and*
move the audit earlier.

### Key terms — KA 6.4

| Term | Meaning |
|---|---|
| **Check block** | A dedicated output area computing every invariant as a difference that must be nil. |
| **Elasticity** | Percentage change in an output per one per cent change in an input. |
| **Tornado** | Sensitivities ranked by magnitude; meaningless without its case labelled. |
| **Breakeven translation** | Converting a ratio threshold into the input level that crosses it. |
| **Model audit** | Independent review against documents, logic and arithmetic; a lenders' condition precedent. |
| **Input provenance** | Every external input traced to a source document, version and date. |
| **Golden-answer regression suite** | Verified results rerun after every edit; unexplained change blocks release. |
| **Breakeven fee / error rate / detection rate** | The three points at which a review stops adding value. |

### Sample MCQs — KA 6.4

**MCQ 6.4-A `[6.4.1 · Analysis]`** A modeller taxes `EBIT` of 5,100,000 at 20 % instead of taxable
profit of 2,580,000. The balance sheet still balances. The check that catches it is:

- A. the balance-sheet check, once the period is recalculated
- B. sources equal uses
- C. the implied effective tax rate — 1,020,000/2,580,000 = 39.53 % against a statutory 20 % ✅
- D. closing debt nil at maturity

*Rationale:* A consistently propagated error balances, so invariant 1 is necessary and not
sufficient (6.4.1). B and D test construction funding and the debt schedule, neither of which the
error touches.

**MCQ 6.4-B `[6.4.1 · Application]`** Kestrel's bank case shows `DSCR` of 1.2743 in year one,
1.1851 in year twelve and an average of 1.2340 against a 1.20× covenant. The reportable position
is:

- A. compliant, since the average exceeds the covenant
- B. compliant, since year one exceeds the covenant
- C. a breach in year twelve; the minimum and its year must be reported, because covenants are
  tested in periods ✅
- D. indeterminate without the sponsor case

*Rationale:* Domain 10's rule, earned here: an average conceals the period that breaches (6.4.1b,
KA 10.A.3). D inverts the logic — the sponsor case, in which coverage rises to 1.5940, is the case
that hides the problem.

**MCQ 6.4-C `[6.4.2 · Analysis]`** Kestrel's unlevered `NPV` has an elasticity of 0.00 to the
interest rate, while a 10 % rate rise takes the minimum `DSCR` to 1.1485 — below the 1.15× lock-up.
The correct inference is:

- A. interest-rate risk is immaterial
- B. the model has an error, since a higher rate must reduce value
- C. sensitivity must be run on the outputs the decision turns on: financing does not change the
  asset's cash, but it changes coverage, which is what the covenant tests ✅
- D. the discount rate and the interest rate should be equal

*Rationale:* Unlevered cash is financing-independent by construction (6.4.2). A reads only the
`NPV` column, which is exactly the mistake; B misunderstands what unlevered means; D confuses the
opportunity cost of capital with the cost of debt.

**MCQ 6.4-D `[6.4.2 · Application]`** Domain 10 reported covenant headroom of 372,438 of annual
`CFADS`, 5.83 % of base case. Expressed as revenue, the headroom is:

- A. 5.83 %
- B. 4.14 % ✅
- C. 8.22 %
- D. 15.27 %

*Rationale:* `CFADS` amplifies revenue by 1.4098, so the covenant breaks at revenue of 11,503,416,
a 4.14 % fall (6.4.2). A carries the `CFADS` percentage across as if the two were interchangeable —
overstating the operations team's room by 41 %; C multiplies rather than divides by the elasticity;
D is the fall at which payment itself fails.

**MCQ 6.4-E `[6.4.3 · Analysis]`** A model audit costs 180,000 and two weeks of delay worth
248,267; `p` = 0.35, `d` = 0.85, `C` = 2,691,071, pre-close correction 184,133. Moving the audit
early, so it adds no delay and correction costs only 60,000, changes its net value and breakeven
error rate to:

- A. 317,547 and 20.10 %
- B. 602,744 and 8.05 % ✅
- C. 941,875 and nil
- D. 497,547 and 20.10 %

*Rationale:* `941,875 − [180,000 + 0.35 × (0.85 × 60,000 + 0.15 × 2,691,071)] = 602,744`, and
`180,000/(0.85 × (2,691,071 − 60,000)) = 8.05 %` (6.4.3). A is the late-audit answer; C is the
expected cost of no control at all; D pairs the late audit's breakeven fee with its breakeven error
rate.

**MCQ 6.4-F `[6.4.1 · Evaluation]`** On the bank case Kestrel's `DSCR` falls from 1.2743 to 1.1851
in year twelve, averaging 1.2340 against a 1.20× covenant, on the requested 42,000,000. The credit
committee's sizing of 41,171,123 gives a year-twelve `DSCR` of 1.2087; at 41,472,081 the year-twelve
`DSCR` is exactly 1.20×. The soundest recommendation is:

- A. keep 42,000,000, disclose the year-twelve minimum, and rely on the 1.2340 average
- B. size at 41,472,081 — the largest facility that holds the covenant in every period — and record that the 828,877 the committee withheld was already protecting a year-twelve exposure nobody in that negotiation had modelled ✅
- C. size at 41,171,123 as the committee proposed, since 1.2087 clears the covenant with margin
- D. keep 42,000,000 and negotiate the covenant down to 1.15×, matching the lock-up

*Rationale:* covenants are tested in periods, so the constraint is the minimum and the binding period
is year twelve; the correct facility is therefore the largest that satisfies it everywhere (6.4.1,
6.4.1b). A relies on an average that passes while the project breaches. C is defensible and safe, and
it is the answer the committee will accept — but it was sized on year-one coverage and clears year
twelve by accident, forgoing 300,958 of debt capacity for coverage the covenant does not ask for.
D looks like an equivalent trade and is the weakest option: collapsing the covenant onto the 1.15 ×
lock-up removes the tier between a distribution trap and an event of default, which is the early
warning the whole structure depends on.

**MCQ 6.4-G `[6.4.4 · Evaluation]`** On the domain's parameters an AI pre-check is worth 498,481, a
late model audit 317,547, an early audit 602,744, and the pre-check with an early audit 670,716. A
sponsor proposes replacing the audit with the pre-check, since the pre-check ranks above the late
audit on expected value. The soundest position is that:
- A. the substitution is right: 498,481 exceeds 317,547, so the cheaper control is the better one
- B. it should be refused, and both funded — the audit brought earlier and the pre-check added —
  because the audit is the lenders' condition precedent and the scanner's detection rate is not
  independent of the error classes that carry the largest cost ✅
- C. both should be refused, because a measured material-error rate below the 20.10 % breakeven means
  neither pays
- D. the substitution is right provided the pre-check's detection rate is first validated on this
  asset class

*Rationale:* machine checks are additive to review and never substitutive (6.4.4): a scanner finds
structural defects and systematically misses the definitional ones — whether `CFADS` matches the
facility's clause, whether the tax treatment is the jurisdiction's, whether the waterfall follows the
drafted priority — which are precisely the errors with the largest cost, so treating detection as
constant across error classes biases the comparison toward the cheap control. A does that. C ignores
that the audit is a condition the sponsor cannot trade away, and that the tail of the cost
distribution is averaged out of the expectation. D fixes the parameter and leaves the governance
objection untouched.

**MCQ 6.4-H `[6.4.2 · Comprehension]`** A committee asks why the model's sensitivity table cannot
answer the question "what happens to Kestrel in a recession". The best explanation is that:
- A. the table's ±10 % steps are too small to represent a recession, so the range must be widened
- B. a recession is a state of the world in which several drivers move together, and a table that
  moves one input at a time carries no view about how they move together or how likely that is —
  which is the object a scenario is built to represent ✅
- C. the table is reported on value, and a recession is a coverage event, so the defect is the output
  chosen rather than the technique
- D. the two are the same procedure, so the recession case is the table's rows added together

*Rationale:* the difference is what each technique can represent, not the size or the number of the
moves it makes: one-at-a-time measures the model's response to a single input and is silent on
correlation and on behaviour at thresholds, which is what a coherent case exists to carry (6.4.2). A
treats a difference in kind as a difference in degree — a wider step is still one input. C names a
real discipline in the wrong place: coverage as well as value must be reported for *either* technique,
which is why a table reporting only `NPV` ranks interest-rate risk last on Kestrel. D is the
arithmetically tempting answer, because on this linear model a joint move is exactly the sum of the
separate ones — and that is the trap, since adding the rows reproduces the number while still
assigning the bundle no likelihood.

### Self-check — KA 6.4

1. *Why is "the model balances" not evidence of accuracy?* — A consistently propagated error
   balances to the cent; invariant 1 tests structure, not economics (6.4.1).
2. *What does one-at-a-time sensitivity genuinely miss?* — Correlation, and behaviour at
   thresholds; on a linear model it does not miss the arithmetic of combining moves, which is
   additive to the cent.
3. *What decides whether a model audit pays?* — Its elapsed time, not its fee: moving it early
   lifts net value from 317,547 to 602,744 and cuts the breakeven error rate from 20.10 % to
   8.05 %.

---

## Advanced topics — Domain 6

### 6.A.1 Probabilistic modelling, and when it earns its cost

Where a one-at-a-time table cannot assign likelihoods, a simulation can — drawing correlated inputs
from stated distributions and reporting a distribution of outputs. The attraction is real: it yields
the probability of covenant breach over the loan life, which is what a credit committee wants and no
sensitivity table can give. The cost is usually understated. A simulation requires **distributions**
and a **correlation matrix**, assumptions with far less evidence behind them than the point estimates
they replace, and a plausible-looking distribution of `NPV` built on invented correlations is more
dangerous than a point estimate because its shape implies knowledge nobody has. The proportionate
rule: simulate where the exposure justifies defensible parameter work, report the assumptions as
prominently as the results, and never let a distribution's smoothness stand in for evidence. Used
well, the deliverable is not a mean `NPV` — roughly what the base case already gave — but a
**probability of breach** and the **inputs that drive it**.

### 6.A.2 Model reuse, templates and the drift problem

Templates are the right answer for organisations that finance repeatedly: they carry conventions,
check blocks and a tested waterfall, and they make a new model reviewable on day one. They carry two
risks that must be managed rather than hoped away. **Silent inheritance**: a template's tax logic,
day-count convention or reserve mechanic is *this* transaction's only once someone confirms it, and
the confirmation is a task with an owner. **Drift**: each transaction improves its copy, none of the
improvements returns, and after five deals there are five templates and no standard. The governance
answer is a template owner, a versioned template with a change log, a deviation register per
transaction ("this model departs from template v4.2 in these six respects, for these reasons") and a
post-close pass that promotes genuine improvements back. The reuse argument survives being
disciplined; it does not survive being informal.

### 6.A.3 The reviewer's model eye

Invariants and habits to test on any project model before relying on a single output. The **six
invariants** of 6.4.1 all compute to nil, and the check block is visible on the output page rather
than hidden in a working sheet. The **implied effective tax rate** reconciles period by period to
the statutory rate plus explained differences (Worked example 6.4.1). The **flag additions** of
6.1.2 hold, and no period is both construction and operating. **Sources equal uses in every
construction period**, not merely in total. **Capitalised interest** is computed from a drawdown
profile, and the interest convention and periodicity are stated (Worked example 6.2.1 — the
41.01 % swing). **`CFADS` = operating cash flow + interest paid**, and the `CFADS` line reconciles
to the facility's definition clause by clause (Domain 10, Toolkit 10.T.1). **Minimum `DSCR` is
reported with the year in which it occurs**, and the level-cash shortcut is not doing the work
(Worked example 6.4.1b — 1.1851 in year twelve). **`LLCR` = `DSCR`** only where cash is level and
service is an annuity at the loan rate; where cash is not level, the divergence is explained and
not smoothed. **Every waterfall tier is floored at zero** and shortfalls are carried; the model has
been run with a `CFADS` that fails every tier. **Cash tax, not accounting tax, feeds `CFADS`**, and
tax losses carry with a stated expiry rule. **No calculation cell contains a numeral** other than
a period counter. **Every external input has a source document with a version and a date.** And
the model file bears a version number that appears on every printed page, so that the paper on the
table can be tied to the workbook that produced it. Any violated line is a defect, and the
violated line localises it — which is the entire value of building the checks before the answers.

---

## Industry variations — Domain 6

- **Contracted power and availability PPPs.** Revenue is nearly deterministic, so modelling effort
  concentrates on the **availability and deduction mechanism** — a formula-heavy translation of the
  performance regime into cash — and on the tax line. Coverage is thin by design, which makes the
  minimum-`DSCR` discipline of 6.4.1b decisive rather than academic.
- **Merchant power, commodities and mining.** The price deck *is* the model, and it arrives as a
  third-party forecast with versions, so 6.4.3's provenance control is the first-order defence
  (Case study B is this failure in another sector). Lenders size on stressed decks, so the bank case
  must be genuinely separate from the sponsor case rather than a switch changing one row.
- **Transport concessions.** Patronage ramps make the level-cash shortcut untenable from period one,
  so sculpted service (Domain 10, KA 10.1.3) is close to mandatory and the model carries 6.3.1's
  sculpting circularity openly. Traffic models are separate models, and the interface is where the
  errors live.
- **Water and regulated utilities.** Reset cycles create step-changes in revenue at known dates, so
  the timeline carries a **reset flag** and the model shows coverage across the reset rather than
  assuming continuity. Regulatory and statutory asset values coexist and must never be conflated.
- **Digital infrastructure.** Short useful lives and heavy refresh capex make the capital-expenditure
  profile an operating-model problem: lifecycle capex competes with debt service in the waterfall,
  and its position relative to `CFADS` is negotiated.
- **Oil, gas and heavy industry.** Decommissioning provisions and their funding create an
  end-of-life obligation most templates handle badly, and Domain 4's sign-change pathology
  (KA 4.1.2) follows directly — a further reason these sectors report `NPV` and treat "the `IRR`"
  with suspicion.

---

## Case study — Domain 6: the year the level line hid (water)

**Situation.** Kestrel's financing model went to the lenders' model auditor three weeks before the
scheduled close, showing a base-case `DSCR` of **1.2743** held constant across all twelve loan
years, a `CFADS` of **6,384,000**, and an `NPV` of **+16,179,360** carried forward from the
sponsors' appraisal. The senior facility was **42,000,000** with a **1.20×** covenant and a
**1.15×** lock-up. The audit's first substantive finding was not an arithmetic error.

**What happened.** The auditor asked why `CFADS` was constant when the interest deduction was not,
and rebuilt the tax line period by period. Cash tax rose from **516,000** in year one to
**963,287** in year twelve as interest fell from **2,520,000** to **283,564**, and `CFADS` fell
correspondingly from 6,384,000 to **5,936,713**. The `DSCR` profile declined monotonically to
**1.1851** — a **covenant breach in year twelve** — while the twelve-year average stayed at
**1.2340**, comfortably compliant. Three further findings followed from the same source: the
present value of `CFADS` at 8 % was **65,315,883**, not the 68,147,771 the level line implied, an
overstatement of **2,831,888** or **4.34 %**; the `LLCR` was **1.2395**, not the 1.2743 the
level-cash identity had produced; and the appraisal `NPV` of +16,179,360 was a **pre-tax** figure
over a **fifteen-year** horizon that had been carried into a financing paper without a bridge —
the same asset on a post-tax, twenty-five-year, flat-revenue basis is **+2,767,684**.

**How it resolved.** The sponsors first proposed reporting the average, which the auditor rejected
in one line: covenants are tested in periods. They then proposed an equity cure in the affected
years, which is arithmetically small — restoring 1.20× in years eleven and twelve costs
**21,347** and **74,849**, **96,196** in total — and was rejected for the right reason: a
structure that is designed to need a cure has consumed an option it should be holding in reserve
(Domain 10, KA 10.4.3). The structure closed instead at Domain 10's independently derived
**41,171,123**, at which the instalment is **4,910,769** and the year-twelve `DSCR` is **1.2087**,
inside the covenant. The debt level at which the year-twelve minimum is exactly 1.20× is
**41,472,081**, so the binding constraint at close remained Domain 10's year-one 1.30× sizing
test — 828,877 below the request — and the year-twelve problem was already covered by it. The
sponsors funded the difference with equity, as Domain 10's case recorded, and the model went
forward with a period-by-period tax line, a minimum-`DSCR` output with its year, and a four-line
basis bridge from the appraisal to the financing case.

**What the domain teaches here.** A model can be arithmetically perfect and still hide the year in
which the project breaches, because the hiding is done by an *assumption* — level cash — and not by
an error. The mechanism was entirely predictable: the interest tax shield amortises with the loan.
Two habits would have caught it before the auditor did, and both are cheap: report the minimum
`DSCR` with its year, and never hold a line constant when a driver underneath it is not. The
third lesson is about conservatism. Domain 10's 1.30× requirement looked like a bank being
difficult; it was in fact protecting against a year-twelve exposure nobody in that negotiation had
modelled. **A well-built model makes a conservative constraint legible as the specific risk it was
guarding** — and that, rather than any single output, is what makes it worth its cost.

## Case study B — Domain 6: the forecast that was pasted, not linked (transport)

**Situation.** A toll-road concession, capital cost **420,000,000**, sought **294,000,000** of
senior debt — 70 % gearing — at **6.5 % over 18 years**, on a sizing target of **1.35×** and a
**1.30×** covenant. The annuity factor `AF(0.065, 18)` is **10.432466**, giving level debt service
of **28,181,255**. The financing model showed steady-state `CFADS` of **38,000,000** and a `DSCR`
of **1.3484** — comfortably inside the sizing target. The traffic forecast underneath it came from
the technical adviser's separate demand model, imported as **pasted values** into a tab labelled
"traffic input v3".

**What happened.** Eleven weeks before close the technical adviser issued a revised forecast **7 %
lower**, reflecting a change to a competing route's toll policy. The revision was circulated,
acknowledged and filed. Nobody re-pasted it, because the financing model's traffic tab was an input
sheet and the model's change log recorded only formula changes. The lenders' model auditor found it
by doing the one thing the sponsor's team had not: checking each external input's document version
against the current issue. Rebuilt on the revised forecast, `CFADS` was **35,340,000** and the
`DSCR` **1.2540** — **below the 1.30× covenant on the first test date**, and 0.096 below the
sizing target the facility had been structured against.

**How it resolved.** Debt capacity at 1.35× on the revised forecast is
`35,340,000/1.35 × 10.432466 =` **273,098,787**, so the facility had to fall by **20,901,213** and
gearing from 70/30 to **65.0/35.0**. The sponsors funded it. Substituting 20,901,213 of equity for
debt, at a 12 % equity requirement against 6.5 % debt over the 18-year tenor, costs
`20,901,213 × 0.055 × AF(0.08, 18) = 20,901,213 × 0.055 × 9.371887 =` **10,773,610** of present
value — the true price of a paste. The resize and re-diligence took six weeks; at forgone `CFADS` of
**105,556** a day (38,000,000/360) the delay cost **4,433,333**, and re-cutting the schedule,
re-running credit approval and re-issuing the information memorandum cost a further **900,000**:
**5,333,333** in total, over and above the equity substitution the truth required in any event. Had
the error reached close, the reopening would have run twelve weeks against drawn debt at
**158,639** a day (forgone `CFADS` plus interest on 294,000,000), with a 0.30 % amendment fee of
882,000 and 1,400,000 of advisers — **15,607,667**. **The audit's timing avoided 10,274,333.**

**What the domain teaches here.** Not one formula in the model was wrong. The defect was a **model
boundary** with no provenance control across it, and the change log's scope — formula changes only —
guaranteed that an input change would be invisible. Three controls would each have caught it
independently: input provenance recording every external input's source document, version and date;
a live link or a documented import with a version check rather than a paste; and a golden-answer
regression suite, which would have flagged that `CFADS` had not moved when the forecast underneath
it had. The economic lesson generalises beyond the sector: **the cost of a model error is set by
how late it is found, and the expensive errors are almost never arithmetic.** Domain 13 makes this
a diligence stream; here it is a modelling discipline that costs a spreadsheet column.

---

## Executive perspective — Domain 6

What a project finance director cannot delegate in this domain:

- **The basis, horizon and case of every number quoted upward.** Kestrel supports five correct
  `NPV`s spanning 29,545,516 (6.1.3). The director owns which one appears in a board paper and
  insists on the bridge to the others — because the number that survives in an organisation's
  memory is the one nobody labelled.
- **The tax line's provenance.** It moves debt capacity by 3,327,741 on one allowance assumption
  (6.2.3), it is jurisdiction-specific, it changes during a loan's life, and it must trace to a
  dated written opinion. This is the one input a director should be able to name the author of.
- **The minimum, with its year.** Kestrel's average `DSCR` of 1.2340 complies and its year-twelve
  minimum of 1.1851 breaches (6.4.1b). Averages in covenant reporting are a governance failure
  wearing a statistic.
- **The audit's timetable, not its fee.** Two weeks of delay cost 248,267 against a fee of
  180,000; moving the audit early nearly doubles its value and cuts its breakeven error rate from
  20.10 % to 8.05 % (6.4.3). The director's intervention is on the plan, and it must happen months
  before the auditor is appointed.
- **The distribution profile after reserve funding and lock-up.** 121,956 in operating year one on
  18,000,000 of equity, and a 14.67-year equity payback from close (6.2.2, 6.3.3). Boards that have
  not seen this before close are boards that will be surprised after it.
- **One authoritative file, one named owner, one version on every page.** Case study B cost
  5,333,333 and could have cost 15,607,667 because a superseded input survived a change log that
  did not cover inputs. Model governance is not a modelling matter; it is a control the director
  signs for.

## Calculation exercises — Domain 6

**Exercise 6.1** A project has a **40,000,000** envelope funded **65/35** — 26,000,000 of debt at
**7.0 %**, 14,000,000 of equity — with every use funded in that proportion. Committed uses: EPC
**33,000,000**, owner's costs **2,500,000**, capitalised development **1,200,000**, arrangement
fees at **1.75 %** of the facility. Construction runs **five quarters** with a spend profile of
**10, 20, 25, 25 and 20 per cent**; interest accrues quarterly at 7.0 %/4 on the opening debt
balance. Compute the fees, capitalised interest and the balancing contingency, and test the
contingency against a 5–10 % of EPC policy band.
*Solution.* Fees `26,000,000 × 0.0175 =` **455,000**. Iterating the quarterly draw and interest
calculation gives capitalised interest of **849,752**. Contingency balances:
`40,000,000 − 33,000,000 − 2,500,000 − 1,200,000 − 455,000 − 849,752 =` **1,995,248**, which is
**6.05 %** of the EPC price — inside the band, so the envelope is credible. Total uses 40,000,000;
debt drawn 26,000,000; equity 14,000,000. *Common error:* treating contingency as a policy
percentage and letting capitalised interest balance instead — which hides an IDC figure nobody has
computed and produces a model whose construction interest is a plug.

**Exercise 6.2** Net income 3,680,000; depreciation 2,500,000; receivables up 1,100,000; payables
up 400,000; interest paid 2,100,000, classified in operating cash flow. Compute operating cash flow
and `CFADS`.
*Solution.* Operating cash flow `3,680,000 + 2,500,000 − 1,100,000 + 400,000 =` **5,480,000**;
`CFADS = 5,480,000 + 2,100,000 =` **7,580,000**. *Common error:* deducting interest again to reach
`CFADS` (5,480,000 − 2,100,000 = 3,380,000) — the interest has already been paid inside operating
cash flow, and `CFADS` is struck before all debt service.

**Exercise 6.3** A facility of **30,000,000** at **5.5 %** amortises over **10 years** on a level
annuity. `EBITDA` is **6,000,000** flat, accounting and tax depreciation both **1,800,000**, the tax
rate **25 %**, and the working-capital absorption **250,000** a year. Compute the instalment, the
`DSCR` in years 1, 5 and 10, and state the minimum and its year.
*Solution.* `AF(0.055, 10) = 7.537626`; instalment `30,000,000/7.537626 =` **3,980,033**. Year 1:
interest 1,650,000, cash tax `0.25 × (6,000,000 − 1,800,000 − 1,650,000) =` 637,500, `CFADS`
**5,112,500**, `DSCR` **1.2845**. Year 5: interest 1,093,531, tax 776,617, `CFADS` 4,973,383,
`DSCR` **1.2496**. Year 10: interest 207,490, tax 998,128, `CFADS` 4,751,872, `DSCR` **1.1939**.
Minimum **1.1939 in year 10** — the final year, as it must be when revenue is flat and the interest
deduction amortises. *Common error:* computing year one and assuming coverage improves as debt
falls; the debt service is level, so the only moving part is cash tax, and it moves the wrong way.

**Exercise 6.4** On Kestrel's bank case (`CFADS` 6,384,000; instalment 5,009,635.23; covenant
1.20×; `CFADS` = 0.75 × revenue − 2,616,000), compute the covenant's cash trigger, the revenue at
which it is crossed, the fall as a percentage of revenue, and reconcile that percentage with
Domain 10's headroom of 5.83 % of `CFADS`.
*Solution.* Trigger `5,009,635.23 × 1.20 =` **6,011,562**. Revenue:
`(6,011,562 + 2,616,000)/0.75 =` **11,503,416**, a fall of **4.14 %**. The reconciliation is the
elasticity: `CFADS` responds to revenue at `0.75 × 12,000,000/6,384,000 =` **1.4098**, so
`5.83 %/1.4098 = 4.14 %`. *Common error:* quoting the `CFADS` percentage to a team that manages
revenue, overstating their room by 41 %.

**Exercise 6.5** A model audit costs **210,000** and adds delay worth **300,000**. The material-error
probability is **0.28**, the detection probability **0.80**, the pre-close correction cost
**90,000**, and the cost if the error reaches close **3,400,000**. Compute the audit's net value and
its breakeven error rate.
*Solution.* Without the audit `0.28 × 3,400,000 =` **952,000**. With it
`210,000 + 300,000 + 0.28 × (0.80 × 90,000 + 0.20 × 3,400,000) = 510,000 + 0.28 × 752,000 =`
**720,560**. Net value **231,440**. Breakeven error rate
`(210,000 + 300,000)/(0.80 × (3,400,000 − 90,000)) =` **19.26 %** — below that, the audit destroys
value at this timing. *Common error:* omitting the elapsed cost, which reports a net value of
531,440 and a breakeven error rate of 7.93 %, flattering the control by more than twice.

## Practitioner's toolkit — Domain 6

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 6.T.1 — The model conventions sheet (one page, in the inputs block)

One line per convention, each with the value chosen, the alternative rejected, and the reason:
period length (and why); flow timing (period end, start, mid-period); day-count basis; interest
accrual base (opening or average balance) **and the circularity resolution chosen**; first-period
stub treatment; currency and units; sign convention; rounding, and where it is applied; the basis
(pre/post-tax, levered/unlevered, before/after working capital); the horizon and the treatment of
value beyond it; and the named case list with the single input that switches between them. Rule: a
convention absent from this sheet may not be relied on in a printed output, and the sheet's version
matches the model's.

### Toolkit 6.T.2 — The check block (built before the first answer)

- [ ] Balance sheet balances, every period — difference nil.
- [ ] Sources = uses, every construction period, not only in total.
- [ ] Closing debt nil at maturity, per facility; Σ scheduled principal = total drawings.
- [ ] Cash ≥ 0 in every period and every account; every waterfall tier floored, shortfalls carried.
- [ ] `CFADS` = operating cash flow + interest paid.
- [ ] Implied effective tax rate = statutory rate + explained differences, period by period.
- [ ] Timeline flags: four additions hold; no period double-flagged.
- [ ] Minimum `DSCR` **and its year** reported beside the average; `LLCR` = `DSCR` only where cash
      is level and service is an annuity at the loan rate; `PLCR` ≥ `LLCR` where a tail exists.
- [ ] No numeral in any calculation cell other than a period counter.
- [ ] Golden-answer regression suite reruns clean, or every change is explained and accepted.
- [ ] Aggregate flag visible on the output page; **no output quoted while it is red.**

### Toolkit 6.T.3 — Model governance register (per model, per version)

Columns: version number (appearing on every printed page) · date · author · reviewer · reason for
the version · change list (from the version diff, inputs **and** formulae) · check-block status ·
golden-answer status · **input provenance table** (each external input: source document, its
version, its date, who confirmed it against the current issue) · freeze status and change-control
authority after freeze · basis, horizon and case list as released · outstanding audit findings by
severity with owners and dates · AI-assisted edits, with the tool, the prompt intent, the human
author and the diff reference. Front line: **which file is authoritative, who owns it, and what
its check block says today.**

## Exam preparation — Domain 6

**What is assessed.** The three-block rule and the flag invariants; sources and uses with computed
capitalised interest and an identified balancing line; the effect of periodicity and interest
convention; accounting depreciation against tax allowances, and their coverage and capacity
consequences; the three-statement articulation and the `CFADS` tie; debt-schedule invariants and
circularity resolutions; waterfall construction, reserve funding and release, and the distribution
test; project return against equity return; the six check-block invariants and what each can and
cannot see; sensitivity, elasticity, the breakeven translation and the limits of one-at-a-time; and
the economics of model audit and AI-assisted controls.

**The calculations to do under time pressure.** Capitalised interest from a drawdown profile on a
stated convention, and the balancing line that follows (6.2.1, Exercise 6.1). Operating cash flow
to `CFADS` via the interest tie (6.2.2, Exercise 6.2). Cash tax from taxable profit, and the
`DSCR` in a late loan year when the interest deduction has shrunk (6.2.3, 6.4.1b, Exercise 6.3).
Minimum `DSCR` and its year. Elasticity from two `NPV`s, and a ratio threshold translated into an
input level (6.4.2, Exercise 6.4). Review net value and its three breakevens (6.4.3,
Exercise 6.5).

**The traps.** Quoting an `NPV` without its basis, horizon and case — the 29,545,516 spread
(6.1.3, MCQ 6.1-A) · an annual timeline with opening-balance interest, understating capitalised
interest by 41.01 % (6.1.2, MCQ 6.1-B) · letting capitalised interest balance the sources-and-uses
table instead of contingency (Exercise 6.1) · a round balancing line, which means the table was
plugged (6.2.1, MCQ 6.2-A) · deducting interest twice on the way to `CFADS` (6.2.2, MCQ 6.2-B) ·
taxing `EBIT` instead of taxable profit, which balances perfectly (6.4.1, MCQ 6.4-A) · treating a
balancing balance sheet as evidence of accuracy (6.4.1) · reporting average rather than minimum
`DSCR` (6.4.1b, MCQ 6.4-B) · holding `CFADS` level when the interest deduction is amortising
(6.4.1b, Case study A) · reading only the `NPV` column of a sensitivity table and concluding
interest-rate risk is immaterial (6.4.2, MCQ 6.4-C) · comparing elasticities across cases (6.4.2) ·
carrying a `CFADS` percentage across as a revenue percentage, overstating headroom by 41 %
(6.4.2, MCQ 6.4-D) · omitting elapsed time from review economics (6.4.3, Exercise 6.5) · treating
an AI detection rate as constant across error classes (6.4.4) · a change log that covers formulae
but not inputs (Case study B).

**How the domain connects.** It industrialises Domain 3's discounting and Domain 4's appraisal and
audits both — Worked example 6.1.3 reconciles Domain 4's `NPV` to the financing model — and it
implements Domain 2's statements, `CFADS` definition and cash-versus-accounting tax distinction. It
supplies the drawdown Domain 5's slip arithmetic priced and the model whose coverage Domain 10
negotiated, earning Domain 10's minimum-`DSCR` rule rather than asserting it. Domains 7, 8 and 9
supply the revenue, cost and funding blocks this architecture consumes; Domain 13 makes the model
audit a diligence stream and a condition precedent; Domain 14 runs the construction model against
actual drawdowns; Domain 15 operates the waterfall of 6.3.2; Domain 16 generalises 6.4.4's controls
into model-risk governance.

## Domain 6 summary
A financial model's authority comes from its architecture and its checks, not from its answers, and
Kestrel proves it four times over. **One project supports five arithmetically correct net present
values spanning USD 29,545,516** — from −9,670,265 to +19,875,251 — differing only in basis,
horizon and case; Domain 4's **+16,179,360** is the pre-tax, fifteen-year member of that family,
reconciled to Domain 2's `CFADS` by an implied escalation of **2.967 %** to within 661 of present
value, and the required deliverable is the bridge rather than any one number. **Convention is worth
more than most contested assumptions**: the same drawdown at the same 6.0 % returns capitalised
interest of **2,114,597** quarterly on opening balances and **1,247,352** annually — a 41.01 %
understatement that propagates silently into the depreciation base — while the sources-and-uses
table closes at **60,000,000** on a balancing contingency of **3,645,403**, 7.59 % of the EPC
price, and a balancing line that is never round. **The tax line is the most consequential row in
the model**: a 15 % declining-balance allowance regime would take year-one `CFADS` from 6,384,000
to **6,900,000**, `DSCR` from 1.2743 to **1.3773**, and debt capacity at 1.30× from 41,171,123 to
**44,498,864** — a 3,327,741 swing, four times the gap Domain 10's whole negotiation was fought
over. **Balancing is necessary and not sufficient**: taxing `EBIT` instead of taxable profit gives
a `CFADS` of 5,880,000, a `DSCR` of 1.1737, a debt-capacity shortfall of **3,250,352** — and a
balance sheet that balances to the cent, caught only by an implied effective tax rate of
**39.53 %** against a statutory 20 %. And **the level-cash shortcut hides a breach**: modelled
period by period, Kestrel's coverage declines from 1.2743 to **1.1851 in year twelve** against a
1.20× covenant, while the twelve-year average of **1.2340** complies — which is why the minimum and
its year are reported, and why Domain 10's 1.30× sizing test, at **41,171,123** with a year-twelve
`DSCR` of 1.2087, was already protecting against a risk nobody in that negotiation had modelled.
The first operating year articulates completely — operating cash flow **3,864,000**, `CFADS`
**6,384,000** through the interest tie, a closing balance sheet of **59,752,409** on both sides, and
a distribution to equity of **121,956**, 0.68 % of the 18,000,000 contributed — and the returns it
leads to are honest about who earns what: the asset **8.54 %**, the equity **9.83 %** on the bank
case and **13.52 %** with escalation, with equity payback **14.67 years** from close. Sensitivity
ranks revenue first at an elasticity of **6.27** on the sponsor case and **34.71** on the bank
case — proof that elasticity is a property of the case — reports **0.00** elasticity of unlevered
value to the interest rate while the same move drives the minimum `DSCR` to 1.1485, and translates
coverage into the language operations use: the covenant breaks at revenue **4.14 %** below base,
not the 5.83 % of `CFADS` Domain 10 quoted. Review economics close the domain: a model audit worth
**317,547** late and **602,744** early, with breakeven fee **497,547**, breakeven error rate
falling from **20.10 %** to **8.05 %** on timing alone, and an AI pre-check worth **498,481** that
still cannot substitute for the audit because its detection rate is not independent of the error
class it misses. Domain 7 builds the revenue block this architecture consumes, Domain 8 the cost
and contingency block, Domain 9 the sources side, Domain 13 the audit as a condition precedent,
and Domain 16 the model-risk governance this domain's controls anticipate.
