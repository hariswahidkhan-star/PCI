# Appendix G — Integrated capstones

A capstone is not a longer case study. Each domain's cases test whether a candidate can apply one
domain's machinery; a capstone tests something the chapters structurally cannot — whether the
**answers agree with each other**, and whether the reader can see which of them is actually binding
at each point in a project's life. A financing is not a sequence of sixteen correct calculations. It
is one asset whose appraisal, structure, contracts, diligence, drawdown and covenants must all be
true at the same time, and the interesting failures are the ones that live in the joints.

**Four capstones, and why the first is different.** Three of the four are new projects, deliberately
chosen for different risk shapes: a **toll-road concession** where the revenue risk is demand and
nothing hedges it, a **solar-plus-storage independent power project** where the contract is a price
and the risk is resource, and a **hyperscale data centre** where the asset is short-lived, the
counterparty is investment-grade and the whole case turns on re-contracting. Each is worked from its
own arithmetic.

The first capstone is the **Kestrel Water desalination PPP** — the master thread this volume has
carried since Domain 3. It is not reworked here, because reworking it would be the duplication this
programme's editorial rules forbid. What it has never had is an **assembly**: the thread is
distributed across sixteen chapters and has never been laid out in one place, so no reader has yet
seen the whole ledger, the sequence in which its decisions were actually taken, the three places its
own numbers had to be reconciled against one another, or — the finding that matters most — how the
**binding constraint moves** as the project ages. That assembly is new content, and it is the single
strongest test available of whether this volume is internally consistent.

---

## Capstone One — Kestrel Water: the master thread, assembled

**The asset.** A single-purpose company holds a 25-year concession to build and operate a seawater
desalination plant, sized at 60,000,000 of capital expenditure against a 48,000,000 fixed-price,
date-certain EPC wrap, funded 70/30 senior debt to sponsor equity, with a 12-year senior facility at
6.0 % and an availability-based offtake. Every figure below is a result printed in this volume and
independently recomputed by the golden-answer suite; the cross-references are the authority.

### G.1.1 The ledger — which chapter produced which number

| # | Domain | The result that leaves the chapter | Where it is used again |
|---|---|---|---|
| 1 | Foundations | limited-recourse premium **5,202,128** PV; enforcement exposure removed **10,073,997**; breakeven failure probability **51.6392 %** | frames every later structural choice |
| 2 | Accounting | `EBITDA` **7,500,000** → net income **2,064,000** → operating cash flow **3,864,000**; working capital absorbs **600,000** | D10's `CFADS` definition; D15's operating bridge |
| 3 | Time value | senior instalment **5,009,635.23** on `AF(0.06, 12) = 8.383844` | D9, D10, D15 — every coverage ratio |
| 4 | Appraisal | `NPV` **+16,179,360** at 8 %; `IRR` **12.1921 %**; `MIRR` **9.7327 %**; discounted payback **10.07 years** | D6 reconciles it; D9 supplies the rate |
| 5 | Bankability | six conditions averaging **90.5 %** multiply to **54.72 %** probability of close; honest origination cost **7,400,000** | G.1.4 — the finding that changes the case |
| 6 | Modelling | **five** defensible `NPV`s spanning **29,545,516**; capitalised interest **2,114,597** quarterly against **1,247,352** annually; sources and uses close at **60,000,000** on contingency **3,645,403** | D8's provision; D14's drawdown |
| 7 | Revenue | availability and volume offers with identical expected `CFADS` **6,384,000** and `DSCR` **1.2743**, differing by **10,679,727** of debt capacity | D10's sizing; D15's covenant |
| 8 | Cost and contingency | contingency **3,645,403** (7.59 %); lifecycle programme **6,881,021** PV, **644,606** a year | G.1.3 — the constraint nobody funds |
| 9 | Funding structure | `k_e` **15.42 %**, after-tax `k_d` **4.80 %**, `WACC` **7.9860 %**; `WACC(g) = 8.70 % − 1.02 % × g` | D4's hurdle, reconciled in KA 4.A.3 |
| 10 | Debt sizing | capacity **41,171,123** at 1.30×, against a **42,000,000** request; covenant headroom **372,438** | G.1.2 — the first reconciliation |
| 11 | Risk allocation | three risks transferred for **2,128,000** of premium against **4,420,000** retained; five declined | D8's retained register; D12's caps |
| 12 | Contracts | exposure **12,255,674** against risk-adjusted cover **8,160,000** — residue **4,095,674** | G.1.5 — what equity actually carries |
| 13 | Diligence | seven streams worth **+4,066,700** in parallel and **−1,146,900** in series | the 5,213,600 that is scheduling alone |
| 14 | Drawdown | quarter-five draw **9,924,564**; the funding-order sub-clause worth **1,466,064** of capitalised interest | D6's interest convention |
| 15 | Operations | covenant binds at `CFADS` **6,011,562.28**; the **distribution** test binds at **6,262,044.04** | G.1.3 — the real constraint |
| 16 | Data and AI | manual review **19.36** a record against **9.36** automated; breakeven **14,800** records a year | why the estate automates and the project does not |

### G.1.2 The first reconciliation — the 828,877 that was never in the plan

Domain 9 proposes 70/30 gearing: 42,000,000 of senior debt against 18,000,000 of equity. Domain 10
sizes the debt from cash flow outwards and finds that level `CFADS` of 6,384,000 at a 1.30× target
supports **41,171,123**. The gap is **828,877**, and it does not close by negotiation — every route
to closing it runs through redefining `CFADS`, which fails at the first competent credit review.

It closes with equity. The consequence is small in percentage terms and large in what it reveals:

| | As proposed | As financeable |
|---|---|---|
| Senior debt | 42,000,000 | **41,171,123** |
| Sponsor equity | 18,000,000 | **18,828,877** |
| Equity share of capex | 30.00 % | **31.38 %** |

Two lessons sit in that table. First, **the sizing target, not the gearing target, decides the
structure** — a sponsor who negotiates gearing without computing coverage is negotiating a number
the lender will not honour. Second, the equity cheque in every subsequent calculation should be
18,828,877, not the 18,000,000 in the term sheet, and a return computed on the term-sheet figure
overstates the sponsor's outcome. Domain 9's own ladder makes the same point structurally: the
1.30× requirement binds at **68.6185 %** gearing, so 70 % was never available.

### G.1.3 The binding constraint moves — and this is the capstone's central finding

Ask, at four points in this project's life, *what would stop it?* The answer is a different quantity
each time, and the gap between the answers is where projects are actually lost.

| Life stage | The binding test | Its value | Headroom |
|---|---|---|---|
| Appraisal | `NPV` positive at the owned rate | +16,179,360 | 21.24 % of revenue (KA 4.3.3) |
| Financial close | joint probability of all conditions | **54.72 %** | no headroom — it is a probability |
| Sizing | `DSCR` ≥ 1.30 at the target | 1.2743 achieved | **372,438** of annual cash |
| Operation | the **distribution** condition | binds at `CFADS` 6,262,044.04 | **121,956**, or 1.9103 % |

The operating row is the one that should change behaviour. The covenant everyone watches has
**372,438** of annual headroom — 5.8339 % of base-case cash. The distribution condition, which
decides whether the sponsors are paid at all, has **121,956**, only **32.75 %** of the covenant's
headroom and **1.9103 %** of base cash. A dashboard built around the covenant is therefore
monitoring the *lender's* risk while the *shareholders'* risk fails almost three times earlier, and
the sponsor discovers the difference the first quarter a distribution is blocked.

Set the lifecycle programme beside that number and the picture sharpens into something close to
alarming. Domain 8 prices Kestrel's membrane and pump replacement at 6,881,021 in present value —
**644,606** as a level annual charge, **10.0972 %** of `CFADS`. That annual charge is **5.2856
times** the distribution headroom and **1.7308 times** the covenant headroom. Fund it out of
operating cash and `CFADS` falls to 5,739,394, taking `DSCR` from **1.2743 to 1.1457** — below the
1.20 covenant. So the lifecycle reserve **cannot** be funded from the cash the covenant is measured
on; it has to come from the capital envelope, a dedicated reserve built at close, or a tariff term
that recognises it. Domain 8's year-seven `DSCR` of **0.4561** on an unreserved programme is the same
finding at its sharpest: a payment default, on an otherwise healthy asset, caused by a maintenance
schedule everyone knew about from the first day.

This is what a capstone is for. No chapter is wrong. Domain 10 correctly reports 1.2743, Domain 8
correctly prices 644,606, and Domain 15 correctly identifies the distribution test. Only the assembly
shows that the three are in tension, and only the assembly forces the question of which balance sheet
absorbs it.

### G.1.4 The project is excellent and the business barely pays

Domain 4 reports +16,179,360 of value. Domain 5 reports that the six conditions precedent multiply to
a **54.72 %** probability of ever reaching it, and that the honest origination cost — the programme
spend across forty screened opportunities divided by the two that closed — is **7,400,000**, not the
2,400,000 on this project's own charge code.

Put the two together, which no chapter does:

```
expected value at the decision to develop = 16,179,360 × 0.5472 = 8,853,346
honest origination cost                                        = 7,400,000
                                                                 ---------
                                              net              = 1,453,346
```

The origination cost is **83.58 %** of the probability-weighted value. A project that looks like a
sixteen-million-dollar success is, at the moment the development decision is actually taken, a
one-and-a-half-million-dollar business — and that is *before* the recourse premium of 5,202,128 that
Domain 1 prices for the structure that makes it financeable at all. Three professional consequences
follow, and none of them is "reject the project".

**The development portfolio is the unit of account, not the project.** A 54.72 % close probability is
not a defect to be argued away; it is the ordinary state of a development pipeline, and it means the
economics only work across a portfolio with a close rate above the **2.29 %** breakeven Domain 5
computes. A sponsor that charges origination to individual projects will always conclude that
development is cheap and will always be wrong.

**Conditions precedent are the highest-return work available.** Domain 5's own sensitivity says
lifting the weakest condition from 0.85 to 0.95 adds **6.4375 points** of close probability while
lifting the strongest adds **1.7280** — a 3.7255× difference. At an 8,853,346 expected value, 6.4375
points is worth more than a million dollars of expected value for what is usually a few weeks of
legal and commercial work. Nothing else in this ledger has that return.

**Elapsed time is priced, not free.** Domain 13's diligence programme is worth **+4,066,700** run
inside a twelve-week parallel envelope and **−1,146,900** run in series — a **5,213,600** swing that
is *entirely* scheduling. The same seven streams, the same 1,500,000 of fees, the same findings, and
a five-million-dollar difference in value depending only on whether someone drew a critical path.

### G.1.5 What the equity cheque actually carries

Domain 12 prices the contractual protection: against a 300-day delay and a 5 % output shortfall, the
exposure is **12,255,674** and the risk-adjusted cover is **8,160,000**, leaving a credit-adjusted
residue of **4,095,674**. Against the equity actually required from G.1.2 — 18,828,877 — that residue
is **21.7521 %** of the cheque, and against Domain 4's `NPV` it is **25.3142 %**. Net of it, the
project's value to the sponsor is **12,083,686**.

The temptation is to buy more protection. Domain 11 prices that too: the full EPC wrap on all eight
risk items raises capex to **64,620,000** and pushes year-one `DSCR` to **1.1832** — below the 1.20
covenant. **The protection is unaffordable not because of its price but because of its effect on
coverage**, which is a sentence worth committing to memory, because it is invisible to anyone
comparing premium against expected loss. Domain 11's own allocation test explains why the eight items
divide as they do: the three the contractor genuinely controls transfer at **2.0771** dollars of
expected-cost reduction per dollar of premium, while the five it cannot control destroy **460,000** of
value *even at a zero margin*, because the bidder's own expected cost (3,300,000) exceeds the owner's
(2,840,000). Risk transfer is not insurance; it is a bet on who is the cheaper bearer, and it is
negative-value whenever the answer is the person already holding it.

### G.1.6 Which of the five NPVs the board saw

Domain 6's most uncomfortable result is that this one project supports **five arithmetically correct
net present values spanning 29,545,516** — from −9,670,265 to +19,875,251 — differing only in basis,
horizon and case. Domain 4's headline +16,179,360 is the pre-tax, fifteen-year member of that family,
and it sits at the **87.49th percentile** of the span.

That is not an accusation; the pre-tax fifteen-year basis is a defensible screening convention and
Domain 6 reconciles it to `CFADS` to within 661 of present value on an implied escalation of 2.967 %.
But a director who has seen only that number has seen the second-best of five defensible answers
without being told that four others exist. The deliverable Domain 6 insists on — **the bridge, not the
number** — is therefore not modelling hygiene. It is the difference between a board that has been
informed and a board that has been persuaded.

### G.1.7 The five questions this capstone equips a candidate to ask

1. **Is the debt sized or negotiated?** If the gearing came before the coverage arithmetic, the equity
   cheque in the term sheet is understated (here by 828,877, taking equity from 30.00 % to 31.38 % of
   capex).
2. **Which test is actually binding, today?** Appraisal, close probability, coverage and the
   distribution condition bind at different times, and the distribution test — 1.9103 % of headroom —
   is invisible on a covenant dashboard.
3. **Where is the lifecycle programme funded from?** If the answer is operating cash, compute the
   resulting `DSCR` before agreeing (1.2743 → 1.1457 here, through the covenant).
4. **What is the expected value at the development decision, and what did origination really cost?**
   8,853,346 against 7,400,000 is a different business from 16,179,360 against 2,400,000.
5. **How many defensible NPVs does this model support, and which one am I looking at?** If the answer
   is "one", the model has not been interrogated yet.

---

## Capstone Two — Aurora Ridge: the road that cannot be geared

Kestrel's revenue is an availability payment: a counterparty pays for a plant that is ready, and
volume risk sits with the offtaker. Every ratio in Capstone One rests on that. **Aurora Ridge is the
same discipline applied to a revenue nobody guarantees**, and the arithmetic does not merely shift —
it changes what the project can be.

**The asset.** A 30-year concession to build and operate an inter-urban toll road. Capital
expenditure **USD 240,000,000**. Design traffic **18,000 vehicles a day** at a **2.40** toll, so
mature annual revenue is `18,000 × 365 × 2.40 =` **15,768,000**. Operations and routine maintenance
**4,200,000** a year, largely fixed; a pavement and structures reserve of **1,368,000** a year, so
mature **`CFADS` = 10,200,000**. Senior debt is offered at **7.0 %** over a **20-year** tenor. There
is no offtaker, no availability payment, no minimum revenue guarantee, and no hedge for traffic —
which is the whole point of the case. *(Fictitious project. Cash taxes are assumed nil in the ramp
years under capital allowances and are excluded from `CFADS` here; the treatment is
jurisdiction-specific and would need written advice, per Domain 4's cross-sector caution.)*

### G.2.1 The demand-risk premium, in coverage and then in money

Lenders price unhedged demand risk in the **required ratio**, not in the margin. Where Kestrel's
availability structure was sized at **1.30×**, a merchant toll road is sized at **1.40×** or higher.
That is a small-looking change:

| Required `DSCR` | Annual service | Debt capacity on `AF(0.07, 20) = 10.594014` | Gearing |
|---|---|---|---|
| 1.30× | 7,846,153.85 | **83,122,265.62** | 34.63 % |
| **1.40×** | 7,285,714.29 | **77,184,960.93** | **32.16 %** |
| 1.45× | 7,034,482.76 | 74,523,410.55 | 31.05 % |

Ten basis points of coverage costs **5,937,304.69** of debt capacity — **7.1429 %** of it, which is
exactly `1 − 1.30/1.40`, because capacity is inversely proportional to the required ratio and nothing
else in the calculation moved. That identity is worth carrying: **a coverage requirement is a
proportional tax on debt capacity**, so a negotiation over "just a tenth of a turn" is a negotiation
over seven per cent of the senior facility, and it should be conducted in money.

Note the gearing column, though, because it is the finding. Even at the *lower* 1.30× requirement this
road supports only **34.63 %** senior debt against Kestrel's 68.6185 %. Nothing is wrong with the
road; the ratio is simply being asked to cover a cash flow that no one has promised.

### G.2.2 The ramp is the project, and level sizing cannot see it

Toll roads do not open at design traffic. Assume a conventional three-year ramp — **60 %, 80 %,
100 %** of design traffic — with operating cost and the pavement reserve unchanged, because they are
functions of the road and not of the traffic on it. That last clause is where the damage comes from:
**revenue ramps and cost does not.**

| Year | Traffic | Revenue | `CFADS` | `DSCR` on level-sized service of 7,285,714.29 |
|---|---|---|---|---|
| 1 | 10,800 | 9,460,800 | 3,892,800 | **0.5343** |
| 2 | 14,400 | 12,614,400 | 7,046,400 | **0.9672** |
| 3 | 18,000 | 15,768,000 | 10,200,000 | 1.4000 |

Year one does not breach a covenant; it **fails to pay**, at barely half of debt service, and year two
still cannot. The cash shortfall across the two years is **3,632,228.57** and the shortfall against the
1.40× covenant across the ramp is **9,460,800**. A facility sized on mature `CFADS` was never
financeable; it was arithmetically correct and commercially fictional.

The instinctive correction is to size on year one instead. That produces a capacity of
**29,457,413.32** — **47,727,547.61** less than level sizing, a 62 % reduction — and a gearing of
12.27 %, which is not a project financing at all.

### G.2.3 What actually works, and what it costs

Neither extreme is the answer; the structure has to be **shaped to the ramp**. Take interest only for
three years and amortise over the remaining seventeen, and two constraints now compete:

```
year-one interest cover at 1.40x :  0.07 x D  <=  3,892,800 / 1.40   ->  D <= 39,722,448.98
steady-state amortisation at 1.40x:  D / AF(0.07,17) <= 10,200,000 / 1.40
                                     with AF(0.07,17) = 9.763223     ->  D <= 71,132,053.24
```

**The binding constraint is year-one interest cover**, and it binds at **39,722,448.98** — 55.8 % of
what the amortisation test would allow. Sculpting recovers **10,265,035.66** over sizing on year one
and still gives up **37,462,511.95** against level sizing. The resulting structure is comfortable
everywhere except the place it was built for: year-two cover on interest only is **2.5342×**, and
steady-state cover after amortisation begins is **2.5070×** on a service of **4,068,579.51**. Three
years of interest-only costs **8,341,714.29** of interest with no principal retired.

Gearing lands at **16.55 %** of capital expenditure. Set that beside Capstone One:

| | Kestrel Water | Aurora Ridge |
|---|---|---|
| Revenue basis | availability payment | unhedged demand |
| Required `DSCR` | 1.30× | 1.40× |
| Senior gearing achieved | **68.6185 %** | **16.55 %** |
| Equity or support required | 31.38 % | **83.45 %** |

**The same discipline, applied to a different revenue risk, produces a four-fold difference in
gearing** — 4.1459× on these figures. This is the single most important thing a second capstone can
teach, and it cannot be learned from Kestrel at any depth, because Kestrel's offtaker absorbed the
risk that Aurora Ridge's structure has to absorb with capital.

**Which is why real demand-risk roads are not financed the way this one has been.** Read the last row
as a requirement rather than a result: to bring the equity cheque down to Kestrel's 30 %, the project
needs **128,277,551.02** of support — **53.45 %** of capital expenditure — as a construction grant,
availability element, minimum revenue guarantee, or subordinated public debt. That is not a subsidy
smuggled in to rescue a weak project; it is the price of the risk transfer the market declines to make,
computed rather than asserted. A leader who cannot produce that number is not in a position to argue
for it, and a public authority that has not been shown it is being asked to approve a structure whose
economics it has not seen.

### G.2.4 The lever that looks free and is not

Faced with the ramp, sponsors reach for the toll. With a constant-elasticity demand curve at
**e = −0.40** — inelastic, as inter-urban traffic with no competing route usually is — a toll rise
does raise revenue, and this is where judgement is needed rather than arithmetic:

| Toll change | Toll | Traffic | Revenue | Change |
|---|---|---|---|---|
| −10 % | 2.16 | 18,775 | 14,802,058.52 | −6.1260 % |
| base | 2.40 | 18,000 | 15,768,000.00 | — |
| **+10 %** | 2.64 | 17,327 | **16,695,991.78** | **+5.8853 %** |
| +20 % | 2.88 | 16,734 | 17,590,790.60 | +11.5601 % |

Because |e| < 1, **revenue rises with every toll increase**, without limit inside the model. The
revenue-maximising toll would require `e = −1`, and the model never reaches it. That is precisely the
trap. **The binding limit on the toll is contractual and political, not economic**, so a model that
optimises the toll will always recommend raising it, and will be right about the revenue and silent
about the concession's cap, the escalation formula, the shadow-toll politics of a road whose users vote,
and the diversion onto unpriced local roads that a 3.7 % traffic loss represents in someone else's
network. An elasticity of −0.40 is also an estimate from a mature network applied to a road with no
operating history — Domain 7's demand-forecast discipline applies in full, and the estimate should carry
a range rather than a point.

The professional handling is to price the toll lever, disclose that the model has no interior optimum,
and put the ceiling where it belongs: in the concession agreement, negotiated once, rather than in an
annual revenue decision the sponsor will always want to resolve one way.

### G.2.5 The four questions Aurora Ridge adds to Capstone One

1. **Who has promised this revenue?** If the answer is nobody, expect the required ratio to rise and
   read the consequence in money — 7.1429 % of capacity per tenth of a turn here.
2. **What does year one look like, not the mature year?** Level sizing on this project produced a
   facility that fails to pay debt service in its first year at 0.5343× while every mature-case ratio
   was healthy.
3. **Which constraint binds after sculpting?** Here it is year-one interest cover at 39,722,448.98,
   binding at 55.8 % of what the amortisation test allows — so effort spent negotiating tenor or
   steady-state cover buys nothing.
4. **If the structure needs support, how much, and stated as what?** 128,277,551.02, or 53.45 % of
   capex, to reach a conventional equity cheque. A number of that size is a policy decision and must be
   presented as one.

---

## Capstone Three — Helios Flats: the ratio that is not what it says

Aurora Ridge's revenue risk was **demand**, and the lender priced it in the required ratio: 1.40×
instead of 1.30×. Helios Flats has a **contracted price** — so on the face of it the credit is far
better, and the market sizes it at **1.20×**. A leader who reads those two numbers side by side and
concludes that a solar project is the more bankable proposition has made the most expensive mistake
available in this volume, and this capstone exists to show why.

**The asset.** A 25-year fixed-price power purchase agreement at **42.00 per MWh** for energy
delivered from a 200 MW solar plant with a co-located 50 MW / 200 MWh battery. Capital expenditure
**USD 180,000,000**, of which **140,000,000** is the solar plant and **40,000,000** the battery. P50
first-year generation **460,000 MWh**, so P50 revenue is **19,320,000**. Operations and maintenance
**4,320,000** a year and a reserve contribution of **1,000,000**, both essentially fixed, so P50
first-year **`CFADS` = 14,000,000**. Senior debt at **6.5 %** over an **18-year** tenor. Interannual
resource variability **σ = 7.0 %**; module and system degradation **0.5 % a year**. *(Fictitious
project. Cash taxes excluded as in Capstone Two; the treatment is jurisdiction-specific.)*

### G.3.1 The risk is priced in the quantity, not the ratio

Nobody guarantees the weather. A lender therefore does not size on the P50 energy estimate — the
central case — but on a **one-year exceedance probability**, conventionally P90: the level the resource
beats nine years in ten. For a normally distributed annual yield that is `P50 × (1 − z₉₀σ)` with
`z₉₀ = 1.2816`:

```
P90 factor = 1 - 1.2816 x 0.070 = 0.910288
P90 energy = 460,000 x 0.910288 = 418,732.5 MWh
P90 revenue = 418,732.5 x 42.00 = 17,586,764.16
```

The energy falls **8.9712 %**. `CFADS` falls **12.3803 %**, to **12,266,764.16**, because the
4,320,000 of operations and the 1,000,000 reserve do not fall at all. That ratio — **1.3800× of cash
sensitivity per unit of resource sensitivity** — is the project's operating leverage, and it is the
first thing to compute on any asset whose revenue is a quantity times a fixed price. It is also why a
"P90 sensitivity" run on revenue alone understates the coverage effect by nearly four points.

### G.3.2 The binding year is the last one, and that is the mirror image of Aurora Ridge

Solar output declines. At 0.5 % a year the cumulative factor by the final year of an 18-year facility
is `0.995¹⁷ =` **0.918316**, so P90 energy in year 18 is 384,528.9 MWh and P90 `CFADS` is
**10,830,215.15** — **11.7109 %** below the first year.

| Year | P90 energy (MWh) | P90 `CFADS` |
|---|---|---|
| 1 | 418,732.5 | 12,266,764.16 |
| 5 | 410,420.4 | 11,917,658.11 |
| 10 | 400,262.0 | 11,491,004.58 |
| **18** | **384,528.9** | **10,830,215.15** |

Size at 1.20× on **year one** and the facility is **106,643,837.27**, on a service of
10,222,303.47. Carry that service to year 18 and coverage is **1.0595×** — a covenant breach, and one
that arrives not from any adverse event but from the degradation curve that was in the technical
report at financial close. Sizing on the **binding year** instead gives **94,154,879.59**, at a cost
of **12,488,957.68** of capacity.

Set that beside Capstone Two and the pair becomes a single lesson. **Aurora Ridge's binding year is
its first** — revenue ramps up while cost does not. **Helios Flats' binding year is its last** —
revenue degrades while cost does not. Neither project's binding year is the one a mature-case or
first-year model looks at, and the two fail in opposite directions from the same omission: *cost
does not move with revenue.* The general rule the two cases establish together is that **the year to
size on is the year of minimum coverage, and it must be found rather than assumed.**

### G.3.3 What 1.20× actually means, restated

Now the comparison that gives this capstone its title. Restate the sizing requirement onto a common
basis — first-year P50 `CFADS`, which is the figure a sponsor's own model reports:

The restatement has to be done on **cash**, not on energy, and the distinction is not pedantry — it is
G.3.1's operating leverage doing its work a second time. Scaling the ratio by the two energy factors
(`1.20 / 0.910288 / 0.918316 = 1.4355×`) is the intuitive move and it is **wrong**, because it assumes
`CFADS` falls in proportion to generation. It does not: `C₅₀ × 0.910288 × 0.918316 = 11,703,054`
against an actual year-18 P90 `CFADS` of **10,830,215**, the **872,839** difference being the
operations and reserve that never fall. The correct equivalent ratio is the one that reproduces the
same facility from first-year P50 cash — `1.20 × C₅₀ / C₁₈`:

| | Aurora Ridge | Helios Flats |
|---|---|---|
| Quoted required `DSCR` | **1.40×** | **1.20×** |
| Cash-flow basis it applies to | level, P50-equivalent | **P90, in the final year** |
| Restated onto first-year P50 cash — resource only | 1.40× | `1.20 × 14,000,000 / 12,266,764 =` **1.3696×** |
| Restated onto first-year P50 cash — resource **and** degradation | 1.40× | `1.20 × 14,000,000 / 10,830,215 =` **1.5512×** |

**The 1.20× is a 1.5512×.** Twenty basis points of apparent advantage is in fact **fifteen and a
tenth points of genuine disadvantage**, and the whole difference is in two words nobody reads: *which*
cash flow the ratio multiplies. A term sheet quoting a ratio without its basis has quoted nothing, and
a sponsor comparing offers across two projects on the ratio alone is comparing the conventions of two
credit committees.

Note carefully what the energy-scaled shortcut does and does not cost you, because it is not a
disaster and that is what makes it dangerous. At 1.4355× it still lands **above** Aurora Ridge's
1.40×, so it reaches the right *conclusion*: this project is the more demanding credit. What it loses
is the size of the finding. It reports a margin of **0.0355** of a turn against a true **0.1512** —
**23.49 %** of it — which turns a substantial difference into a rounding argument, and a rounding
argument is one a committee will overrule. Getting the direction right and the magnitude wrong by
three-quarters is how correct analysis loses to confident analysis.

This is the single most transferable finding in the appendix. **A coverage ratio is a fraction, and
both parts of it are negotiable.** Aurora Ridge's lenders took their protection in the numerator's
multiplier; Helios Flats' lenders took the same protection in the denominator's definition, and took
slightly more of it. Neither is wrong; only the comparison is.

### G.3.4 The forty million that earns no contracted revenue

The PPA pays for **energy delivered from the solar plant**. The battery does not generate energy — it
shifts it — so under this contract it earns nothing contracted at all. Its economics are arbitrage,
capacity payments or ancillary services: merchant revenue, on which no lender will advance senior debt
sized at 1.20×.

The consequence is arithmetic, not opinion. The **94,154,879.59** of capacity is supported by the
solar plant's contracted cash and stands at **67.25 %** of the solar plant's 140,000,000 — a healthy
contracted-asset gearing. Against the **180,000,000** actually being spent, it is **52.31 %**, and
**85,845,120.41** — **47.69 %** — has to come from equity or from a separate merchant financing on
different terms. The battery therefore dilutes blended gearing by nearly fifteen points, and it does
so **without any change to the solar project's credit**.

Three professional consequences, in the order they get missed:

**The hybrid asset must be financed as two assets.** A single facility sized against blended capex
either over-advances against the battery or under-advances against the solar. The disciplined
structure is two tranches with two bases, and the merchant tranche priced for what it is.

**The battery's business case is a separate paper.** Whether 40,000,000 of storage earns its keep is a
merchant-revenue question — dispatch spreads, cycle life, degradation of the cells on a different
curve from the modules — and it cannot be answered inside a contracted-project model. Presenting it
inside one is how storage gets approved on the solar plant's credit.

**The PPA's definition of the delivered product is a financing term.** If the offtake had been written
for *dispatchable* energy at the point of interconnection rather than for energy from the plant, some
of the battery's value would have become contracted, and some of that 47.69 % would have become
bankable. That clause is negotiated by commercial teams, years before anyone computes a gearing, and
it is worth tens of millions here.

### G.3.5 The three questions Helios Flats adds

1. **What basis does this ratio apply to?** P50 or P90, first year or minimum year, before or after
   degradation. Until that is answered the ratio is a number without units.
2. **Where is the year of minimum coverage?** Not the first, not the mature one — found. It is year one
   on Aurora Ridge and year eighteen here, and both projects would have breached if sized on the other's
   binding year.
3. **Which part of the asset earns the contracted revenue?** Whatever does not is not collateral for
   senior debt at the contracted ratio, however physically integrated it is.

---

## Capstone Four

The last of the four — the **hyperscale data centre** — is **not yet written**, and is listed in this
volume's plan and in `CORPUS_GATE_REPORT.md` as outstanding. Its risk shape is exercised by neither of
the two new-project capstones above and by nothing in the sixteen domains: a **short-lived asset whose
entire case turns on re-contracting**. Where Aurora Ridge's revenue was unguaranteed and Helios Flats'
was contracted for the asset's whole life, a data centre's leases expire long before its debt does, so
the binding question is not the coverage in any modelled year but the probability and price of the
lease that has not been signed. It may not reuse the figures above.

Saying so is the honest alternative to filling the space. A capstone that recycled another project's
numbers under a new name would add pages and subtract credibility.
