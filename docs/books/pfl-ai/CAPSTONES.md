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

## Capstones Two to Four

The three new-project capstones — the toll-road concession, the solar-plus-storage IPP and the
hyperscale data centre — are **not yet written**. They are listed in this volume's plan and in
`CORPUS_GATE_REPORT.md` as outstanding, and this appendix does not pretend otherwise. Each requires
its own verified arithmetic on a different risk shape: unhedged demand risk on the toll road, resource
and dispatch risk with a contracted price on the IPP, and re-contracting risk on a short-lived asset
for the data centre. None of them may reuse Kestrel's figures, because the point of each is that a
different risk shape produces a different binding constraint — which is precisely the finding G.1.3
establishes for a water PPP and which cannot be assumed to transfer.

Stating that plainly is the honest alternative to filling the space. A capstone that recycled the
master thread's numbers under a new project name would add pages and subtract credibility.
