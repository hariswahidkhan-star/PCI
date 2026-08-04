# Domain 10 — Procurement, Contracts and Supply Networks

## Why this domain exists

Domains 5 to 9 assumed that the organisation would have the capability to do the work. On most real
projects it does not, and buys a substantial part of it. Auriga's own baseline says so: of its
USD 4,000,000, the installation package alone is a **USD 2,000,000** subcontract, and the controllers,
the civils and the commissioning support are all bought. More than half the project is delivered by
people the project leader cannot instruct, on terms someone else negotiated, against a specification
written before the design was finished.

That is the gap this domain closes, and the reason it sits last in Part Two rather than first. Every
technique in the preceding five domains — the requirement, the critical path, the earned-value
forecast, the risk response — becomes something the leader can only *ask for* once the boundary of the
contract is crossed. The instrument that converts an intention into an obligation is the contract, and
the instrument that decides who is obliged is the procurement process. Both are usually treated as
functions belonging to somebody else, and both are among the least reversible decisions a project
makes. A poorly chosen supplier can be managed; a poorly chosen contract structure cannot, because it
is the thing you would have to use to manage it.

The domain's central claim is that **procurement decisions are delivery decisions with a computable
price, and almost all of them are decided on the wrong number.** Make-or-buy is decided on unit price
when the answer lies in total cost of ownership including transition and exit. Tenders are decided on
a weighting that nobody has tested, when the weighting itself picks the winner. Contract types are
chosen by habit, when their incentive arithmetic determines what the supplier will do at month eight.
Sourcing is called resilient because there are two suppliers, when the exposure sits in a sub-tier
neither of them will mention. In each case the right number exists, is small enough to compute in a
meeting, and changes the decision.

**Learning objectives.** After this domain a candidate can: compute a procurement chain's total lead
time including governance latency and treat it as a schedule predecessor; build a make-or-buy comparison
on total cost of ownership including transition-in, operating and exit costs, compute the breakeven
volume, and show how pricing the capability stand-up delay moves it; select a route to market and state
what each route buys; construct a price/quality evaluation model, compute the weighting at which the
winner changes, and state the professional obligation to publish the model before bids are opened; test a
bid for abnormal pricing against a named benchmark; compute buyer outturn and supplier margin under
fixed-price, cost-plus and target-cost structures at the same actual cost, state the marginal-dollar
allocation each implies, and say where the `PTA` binds; demonstrate that an incentive structure only
creates value if it changes behaviour, and compute the behaviour change the supplier needs to accept it;
size a performance regime so that its cap deters rather than prices non-performance; assemble a claim's
heads of cost and quantify what a notice provision puts at risk; price a dispute against its
irrecoverable cost; assess an ethical-sourcing programme honestly, including where the expected-value
case fails and obligation governs instead; compare single, dual-split and dual-qualified sourcing on
expected cost, compute the breakeven disruption probability and the premium resilience is worth, and show
why a shared sub-tier destroys the calculation; and govern AI-assisted contract and supplier analysis
without letting it become commercial or legal advice.

**The master project.** Project Auriga continues. This domain buys its **84 substation controllers**,
lets its **USD 2,000,000 installation subcontract** on the target-cost structure Domain 7 priced
(target fee 150,000, share 70/30, ceiling 2,450,000, `PTA` **USD 2,428,571.43**), decides whether to
build or buy its remote-terminal-unit configuration capability, evaluates three installation bids
whose ranking depends entirely on a weighting, prices a variation claim at the blended engineering
rate of **USD 130.625 per hour** established in KA 7.4.1, and tests three sourcing structures against
the controller lead-time risk that Domain 8 carries as R1 (`p` 0.35, impact USD 240,000). Every figure
in this domain is either computed here or cited from the domain that derived it.

---

## Knowledge Area 10.1 — Make-or-buy and the procurement lifecycle

*Topics: 10.1.1 what procurement is for · 10.1.2 make-or-buy on total cost of ownership ·
10.1.3 the procurement lead time as a delivery constraint.*

### 10.1.1 What procurement is for

**Definition.** Project procurement is the process by which a project **converts a need it cannot meet
internally into an enforceable obligation held by another organisation**, and then manages that
obligation to delivery and closure. The definition is deliberately narrow at one end and wide at the
other: it excludes nothing about performance management, and it insists on *enforceability*, because a
supplier relationship that rests only on goodwill has no mechanism for the week goodwill runs out.

**The lifecycle, and what each stage decides.** Six stages, and the value of naming them is that each
one closes options the next cannot reopen.

1. **Define the need** — the specification, and the choice between specifying an *output* (what it must
   achieve) and an *input* (how it must be built). This choice, not the contract type, determines who
   is answerable for fitness for purpose, and it is made before anyone in procurement is involved.
2. **Decide make-or-buy** — treated in 10.1.2, and the only stage at which the answer "we should not
   buy this at all" is still available.
3. **Choose the route to market and the contract structure** — 10.2.1 and 10.3.1.
4. **Tender, evaluate and award** — KA 10.2. The stage with the most process and the least remaining
   freedom, which is why it absorbs attention disproportionate to its leverage.
5. **Administer the contract** — KA 10.3. Where nearly all of the value is won or lost, and where
   staffing is nearly always thinnest.
6. **Close out** — final account, retention release, warranties, knowledge and data transfer, and the
   lessons that inform the next specification. Routinely under-resourced because the project is over,
   which is precisely why final accounts settle badly.

**The three questions a leader must answer before procurement starts**, none of which a procurement
function can answer for them. *What must this supplier be accountable for, in words that will still
mean the same thing in a dispute?* *What will I do if they fail — and is that response available under
the structure I am about to sign?* *What information am I entitled to, on what cadence, and is it
enough to run Domain 7's earned-value system across the boundary?* A project that reaches award without
those three answers has bought a supplier rather than a capability.

**The boundary problem.** Domain 3 established that governance must span the contract boundary and that
decision rights are most often left undefined exactly there (KA 3.1.2). The specific asymmetry to watch
is **threshold mismatch**: the client's change-approval threshold and the supplier's are set
independently, by different organisations, and are almost never equal. Where the supplier may commit to
a change at 25,000 and the client's project leader may approve one only at 10,000, every change between
those figures arrives already committed on one side and unapproved on the other — a design failure at
the moment the contract was signed, not a governance failure at the moment it appears.

### 10.1.2 Make-or-buy on total cost of ownership

**The principle.** A make-or-buy decision compares **total cost of ownership over the whole period the
capability is needed, including the cost of getting into the arrangement and the cost of getting out of
it.** Almost every make-or-buy decision that is later regretted was made on unit price, and unit price
is the one component that systematically favours the option with the higher fixed cost — because it
hides that cost.

```
TCO(option) = transition-in + (unit cost × volume) + transition-out
Breakeven volume  Q* = (F_make − F_buy) / (v_buy − v_make)
```
where `F` is the option's total fixed cost (transition-in plus exit) and `v` is its cost per unit of
output. `Q*` exists only where the option with the higher fixed cost also has the lower unit cost —
which is the normal case, and the reason the decision is a genuine choice rather than an arithmetic
one.

**The four costs the unit price hides.** *Transition-in* — recruitment, training, tooling, test
environments, licences, and the management attention consumed while none of it works yet. *Exit* — and
this is the one most often scored at zero: redeployment or severance, decommissioning, data extraction
in a usable form, knowledge transfer, re-qualification of whoever takes over, and the cost of running
both arrangements in parallel during handover. *Stand-up elapsed time*, priced at the project's cost of
delay wherever the capability is on the critical path. And *residual management cost*, which does not
vanish when work is outsourced — it changes shape, from supervising people to administering a contract,
and a buy case that assumes it falls to zero is understating by the cost of the contract manager it
will need.

**Worked example 10.1.2 — Auriga's remote-terminal-unit configuration: make or buy?**

1. **Setup.** Auriga must configure and commission remote terminal units. Building the capability
   in-house costs **USD 420,000** to stand up (two recruits, training, a test rig, licences), **USD
   3,600** per unit configured, and **USD 60,000** to wind down at the end (rig decommissioning,
   redeployment). Buying it from a specialist costs **USD 95,000** to transition in (specification,
   tender, mobilisation, interface definition), **USD 5,400** per unit, and **USD 145,000** to
   transition out (data extraction into a supportable format, knowledge transfer, re-qualification of
   the utility's own team at handover). Standing the in-house capability up takes **14 weeks** against
   the supplier's **5 weeks**, and the capability is on the critical path. Auriga requires **84 units**.
2. **Formula.** `TCO = F + vQ`, with `F` = transition-in + exit; breakeven
   `Q* = (F_make − F_buy)/(v_buy − v_make)`; and, where the stand-up is on the critical path, add
   (weeks of difference × cost of delay) to the make case.
3. **Substitution.** `F_make = 420,000 + 60,000 = 480,000`; `F_buy = 95,000 + 145,000 = 240,000`.
   `Q* = (480,000 − 240,000)/(5,400 − 3,600) = 240,000/1,800`. At `Q` = 84:
   make `480,000 + 3,600 × 84`; buy `240,000 + 5,400 × 84`. Delay term
   `(14 − 5) × 45,000 = 9 × 45,000`.
4. **Result.** Breakeven volume **`Q*` = 133.33 units**. At the required 84 units, make costs
   **USD 782,400** and buy costs **USD 693,600** — **buy is USD 88,800 cheaper**, 11.35 % of the make
   case. With the 9-week stand-up delay priced at **USD 405,000**, the make case rises to
   **USD 1,187,400** and the breakeven volume moves to **358.33 units**.
5. **Interpretation.** On unit price alone, making looks decisively cheaper: 3,600 against 5,400 is a
   **33.33 %** saving per unit, and that is the number a make-or-buy paper usually leads with. It is
   the wrong test, and the arithmetic shows exactly why: the unit-price advantage of 1,800 has to
   recover a fixed-cost disadvantage of 240,000, which needs 133.33 units, and Auriga has 84. The
   decision is therefore **a bet on volume, and the honest question is not "is making cheaper?" but
   "do we believe the volume will exceed 134 units?"** That reframing is the deliverable. It also
   yields the sensitivity that matters: a second phase of 60 further units takes the total to 144 and
   *reverses* the answer — make USD 998,400 against buy USD 1,017,600, a **USD 19,200** advantage to
   making — so a decision to buy at 84 units should be written so that it can be revisited if the
   second phase is authorised, with the exit cost of 145,000 understood as the price of that
   reversibility rather than as an overhead. Two further cautions belong in the paper, and both are
   about what the number cannot carry. First, **the schedule term dominates everything else here**:
   pricing the 9-week difference at Auriga's cost of delay adds 405,000 and moves the breakeven from
   133.33 to 358.33 units, a factor of **2.69** — so on a project with a critical-path constraint the
   make case is not marginal, it is unavailable, and no volume Auriga will ever see makes it
   competitive. Second, the calculation is silent on **capability strategy**. A utility that intends to
   own its control systems for twenty years may rationally build a capability that loses money on this
   project, and that is a legitimate decision — but it must be taken as a deliberate investment with
   the 88,800 (or 493,800 with delay) named as its price, not smuggled in behind a unit-price
   comparison. The failure mode is not choosing to make; it is choosing to make while believing it is
   cheaper.

> **Fig 10.1.1 — Make-or-buy on total cost of ownership: the breakeven the unit price hides.** Line
> chart, x-axis volume `Q` from 0 to 400 units, y-axis total cost of ownership USD 0 to 2,500,000.
> Three lines: buy `240,000 + 5,400Q` in brand blue; make `480,000 + 3,600Q` in ink; make plus the
> 9-week stand-up delay `885,000 + 3,600Q` as a crimson dashed line. Breakeven points marked at
> **133.33 units** (make vs buy) and **358.33 units** (make-with-delay vs buy). A vertical rule at
> Auriga's required **84 units** with the two outturns labelled — make **782,400**, buy **693,600**,
> buy cheaper by **88,800** — and a side note that the unit price favours making by **33.33 %** and is
> the wrong test. Source: PCI original. Alt text: three rising straight lines on a cost-versus-volume
> chart, the steeper buy line crossing the flatter make lines at 133 and 358 units, with the project's
> required volume of 84 units far to the left of both crossings.

### 10.1.3 The procurement lead time as a delivery constraint

**The principle.** A procurement process has a duration, that duration is a predecessor in the
schedule, and it is systematically absent from plans because it is nobody's activity. Domain 6's
critical path is computed over the work; the procurement chain that must complete before the work can
start is computed, if at all, by a different function on a different document.

**The chain, and its two kinds of time.** Every procurement contains *process* time (specification,
tender, evaluation, approval, execution) and *physical* time (manufacture, transport, acceptance).
Practitioners assume the physical time dominates. It usually does not.

**Worked example 10.1.3 — the controller procurement chain against a 25-week project.**

1. **Setup.** Auriga's 84 substation controllers are a long-lead item. The chain: write and internally
   review the specification **3 weeks**; obtain authority to go to market — a decision for a body
   meeting every 4 weeks with a 1-week paper deadline; tender period **4 weeks**; evaluation and
   clarification **3 weeks**; award approval — the same body again; contract execution and mobilisation
   **2 weeks**; manufacture **11 weeks**; delivery, site acceptance and functional test **2 weeks**.
2. **Formula.** Total lead time = Σ process legs + Σ governance latency + Σ physical legs, with
   governance latency `E[wait] = M/2 + L` per approval (Domain 3, KA 3.2.3).
3. **Substitution.** Each approval: `E[wait] = 4/2 + 1 = 3.0` weeks, twice. Total
   `3 + 3 + 4 + 3 + 3 + 2 + 11 + 2`.
4. **Result.** **31 weeks** of total procurement lead time, against a **25-week** project — an excess
   of **6 weeks**. Governance latency accounts for **6 weeks**, **19.35 %** of the chain, worth
   **USD 270,000** at Auriga's cost of delay. Process time totals **18 weeks** and physical time
   **13 weeks**, so **58.06 %** of the chain is administration.
5. **Interpretation.** The headline is uncomfortable and completely ordinary: **the procurement chain
   for a critical component is longer than the project that needs it.** Three consequences follow, and
   a leader who has done this arithmetic behaves differently at three specific moments. First, long-lead
   items must be committed **before the baseline is approved**, which means committing on an incomplete
   specification and accepting the variation exposure that follows — an explicit, priced trade, not an
   oversight, and the origin of Domain 8's R1 (controller lead-time slip, `p` 0.35, impact 240,000).
   Second, the largest single compressible block is not the manufacture but the **process**: 18 of 31
   weeks, of which 6 are pure governance latency. Domain 3's result applies unchanged and is worth more
   here than anywhere — cutting the paper deadline from 1 week to nothing saves a full week per
   approval, so two approvals give back **2 weeks and USD 90,000** for the price of an administrative
   decision, while collapsing the two approvals into one delegated award authority gives back
   **3 weeks and USD 135,000**. Third, and least popular: the honest schedule shows procurement on the
   critical path from day one. A plan that starts at "contract awarded" is not optimistic, it is
   incomplete by 18 weeks, and the difference will be discovered at the worst possible moment — when
   the site team is standing ready and the controllers are in manufacture. The professional caution:
   the 11-week manufacture figure is a *supplier's* number, and it is the one figure in the chain that
   a supplier has an incentive to state optimistically at bid stage and revise after award. It should
   be tested against the supplier's actual recent performance, not against its quotation.

### AI in this KA

**Where it earns its place.** Assembling total-cost-of-ownership comparisons from disparate sources —
payroll, licence schedules, historical supplier invoices, exit provisions buried in incumbent
contracts — is a data-collation task with a clear right answer, and the exit column is precisely the
one humans leave blank because the evidence is scattered. Extracting stated lead times from a set of
bids and comparing them with the same suppliers' delivery history. Modelling the breakeven volume
across ranges of unit cost, transition cost and delay, which is deterministic and cheap to sweep.
Reading an incumbent contract for its termination, transition-out, data-return and intellectual-property
clauses and listing the exit obligations, which is where the exit cost actually comes from.

**Where it must not go.** It must not supply the *estimates* it is asked to compare. A model asked "what
does it cost to stand up an RTU configuration team?" will produce a confident figure with no provenance,
and that figure will then anchor a real decision — the same defect Domain 3 identified for critical-path
shares. It must not decide capability strategy, which is a decision about what the organisation intends
to be able to do, and belongs to an accountable executive. And its reading of an exit clause is a
summary, not the clause: the obligation is what the contract says.

**Verification, concretely.** Every input to a make-or-buy paper carries a named source and a date.
The breakeven volume is recomputed by hand — it is one subtraction and one division — and stated in the
paper alongside the point estimate, because the breakeven is what tells the decision-maker whether the
recommendation is robust. Where an AI tool has listed exit obligations, each one is confirmed against
the clause before it enters the cost.

### Key terms — KA 10.1

| Term | Meaning |
|---|---|
| **Procurement** | Converting a need the project cannot meet internally into an enforceable obligation held by another organisation, and managing it to closure. |
| **Total cost of ownership (TCO)** | Transition-in + operating cost over the period of need + transition-out; the only valid basis for make-or-buy. |
| **Transition-in / transition-out** | The cost of entering and of leaving an arrangement; the exit column is the one most often scored at zero. |
| **Breakeven volume (`Q*`)** | The volume at which two options cost the same: `(F_make − F_buy)/(v_buy − v_make)`. |
| **Output vs input specification** | Specifying what must be achieved against how it must be built; determines who is answerable for fitness for purpose. |
| **Threshold mismatch** | Client and supplier change-approval thresholds set independently, so changes between them arrive committed on one side and unapproved on the other. |
| **Long-lead item** | A component whose procurement chain exceeds the time available after baseline approval, and must therefore be committed earlier. |
| **Procurement lead time** | Process legs + governance latency + physical legs; a schedule predecessor, not an administrative overhead. |

### Sample MCQs — KA 10.1

**MCQ 10.1-A `[10.1.2 · Application]`** In-house provision costs 420,000 to stand up, 3,600 per unit
and 60,000 to exit; buying costs 95,000 to transition in, 5,400 per unit and 145,000 to exit. The
breakeven volume is:
- A. 66.67 units
- B. 133.33 units ✅
- C. 180.56 units
- D. 227.78 units

*Rationale:* `(480,000 − 240,000)/(5,400 − 3,600) = 240,000/1,800 = 133.33` (10.1.2). A divides the
fixed-cost difference by the *make* unit cost instead of the unit-cost difference
(`240,000/3,600 = 66.67`). C omits both exit costs (`325,000/1,800 = 180.56`). D assigns each option's
exit cost to the other option (`410,000/1,800 = 227.78`).

**MCQ 10.1-B `[10.1.2 · Analysis]`** At 84 units the make option costs 782,400 and the buy option
693,600, yet the make option's unit cost is 33.33 % lower. The correct reading is that:
- A. the unit costs must have been miscalculated
- B. the fixed-cost difference of 240,000 has not been recovered at this volume, so the decision is a bet on volume exceeding 133.33 units ✅
- C. unit cost is the more reliable comparison because it excludes one-off items
- D. the two options are equivalent because the difference is under 15 %

*Rationale:* The unit-price advantage must recover a fixed-cost disadvantage, and 84 units does not
(10.1.2). C inverts the principle — excluding the one-off items is the error. D substitutes a
tolerance for an answer.

**MCQ 10.1-C `[10.1.2 · Evaluation]`** Standing the in-house capability up takes 9 weeks longer than
mobilising the supplier, and the capability is on the critical path at a cost of delay of 45,000 per
week. The effect on the breakeven volume is to move it from 133.33 units to:
- A. 133.33 units — elapsed time does not affect a cost comparison
- B. 225.00 units
- C. 358.33 units ✅
- D. 491.67 units

*Rationale:* `(480,000 + 405,000 − 240,000)/1,800 = 645,000/1,800 = 358.33` (10.1.2). A omits the
delay entirely. B prices the delay alone and forgets the fixed-cost difference
(`405,000/1,800 = 225.00`). D omits the buy option's fixed cost from the numerator
(`885,000/1,800 = 491.67`).

**MCQ 10.1-D `[10.1.3 · Application]`** A procurement chain has 12 weeks of process legs, 13 weeks of
manufacture and delivery, and two approvals by a body meeting every 4 weeks with a 1-week paper
deadline. Total lead time is:
- A. 25 weeks
- B. 29 weeks
- C. 31 weeks ✅
- D. 33 weeks

*Rationale:* Each approval adds `E[wait] = 4/2 + 1 = 3.0` weeks, so `12 + 13 + 6 = 31` (10.1.3,
Domain 3 KA 3.2.3). A omits governance latency altogether; B counts only half of each interval and
omits the paper deadlines; D adds the whole meeting interval twice.

**MCQ 10.1-E `[10.1.1 · Comprehension]`** Which stage of the procurement lifecycle is the last at
which the option "we should not buy this at all" remains available?
- A. define the need
- B. decide make-or-buy ✅
- C. choose the route to market
- D. tender and evaluate

*Rationale:* After the make-or-buy decision, subsequent stages choose *how* to buy, not *whether*
(10.1.1). The stage with the most process — tender and evaluate — has the least remaining freedom.

### Self-check — KA 10.1

1. *Why does unit price systematically favour the wrong option in make-or-buy?* — It excludes fixed
   transition-in and exit costs, which are exactly the costs the low-unit-cost option carries.
2. *What single figure converts a make-or-buy recommendation into a testable proposition?* — The
   breakeven volume: the decision becomes "do we believe volume will exceed `Q*`?"
3. *Which part of a procurement chain is usually most compressible, and why is that surprising?* — The
   process and governance legs — 18 of Auriga's 31 weeks — not the manufacture, which is where
   attention goes.

---

## Knowledge Area 10.2 — Tendering and evaluation

*Topics: 10.2.1 routes to market · 10.2.2 the evaluation model and the weighting that chooses the
winner · 10.2.3 probity, and fixing the model before the bids are opened.*

### 10.2.1 Routes to market

**The choice.** A route to market is a decision about **how much competition to buy and what to pay for
it**, and competition is not free: it costs elapsed time, bid cost on both sides, and the goodwill of
suppliers who lose repeatedly. The main routes, with what each actually purchases:

| Route | What it buys | What it costs | Suits |
|---|---|---|---|
| **Open competition** | The widest price discovery and the strongest probity record | The longest chain (10.1.3), high evaluation effort, high aggregate bid cost | Novel requirements, unknown market, regulated buyers |
| **Selective (pre-qualified) competition** | Price discovery among capable bidders only | Pre-qualification effort; the risk of excluding a capable newcomer | Most substantial project packages |
| **Framework call-off** | Speed — the tender is already done | The framework's rates, which may have drifted from the market; and the aggregation illusion below | Repeat purchases of definable scope |
| **Single-source negotiation** | Speed and continuity of a known capability | The absence of price discovery; a documented justification is mandatory | Genuine sole source, proprietary interfaces, emergency |
| **Two-stage** | Early supplier involvement on buildability while price is settled later | Weak price tension at stage two, when the supplier is embedded | Complex or undefined scope needing supplier design input |

**Three cautions worth stating.** *Competition is not price tension.* Three bidders who all believe one
of them is favoured produce three prices and no competition; price tension requires that each bidder
believes it can win, which is a function of how the model is published (10.2.3), not of the number of
bidders. *The aggregation illusion in frameworks*: a call-off placed without any comparison inside the
framework buys the framework's rate, not the market's, so framework rates need periodic benchmarking
against a live test — a framework that has never been benchmarked is a single-source arrangement with
paperwork. *Two-stage arrangements* fail predictably at stage two, when the supplier is the only party
who understands the design it helped produce and the buyer's walk-away option has quietly disappeared;
the countermeasure is to fix the stage-two commercial terms — fee percentage, overhead recovery, basis of
preliminaries — at stage *one*, while that option still exists.

### 10.2.2 The evaluation model, and the weighting that chooses the winner

**The principle.** An evaluation model converts a set of bids into a ranking. It is a piece of
arithmetic, it is designed by people, and **the design determines the outcome at least as much as the
bids do.** A leader who does not understand that is not evaluating bids; they are ratifying whatever
the model was set up to produce.

```
S_i(w) = w × P_i + (1 − w) × Q_i
```
where `S_i` is bidder `i`'s weighted score, `P_i` its normalised price score, `Q_i` its quality score,
and `w` the weight given to price. Because each `S_i(w)` is linear in `w`, two bidders' scores cross at
exactly one weighting:

```
w* = (Q_b − Q_a) / [(P_a − Q_a) − (P_b − Q_b)]
```

**Worked example 10.2.2 — three bids, three winners.**

1. **Setup.** Auriga's installation package attracts three compliant bids. Alpha USD 2,000,000, quality
   **62**/100; Beta USD 2,200,000, quality **78**; Gamma USD 2,480,000, quality **92**. Price is
   normalised on the ratio convention — the lowest price scores 100, others score
   `lowest ÷ own × 100`. The evaluation panel has not yet fixed the weighting.
2. **Formula.** `S_i(w) = w P_i + (1 − w) Q_i`; crossover
   `w* = (Q_b − Q_a)/[(P_a − Q_a) − (P_b − Q_b)]`.
3. **Substitution.** Price scores: Alpha `2,000,000/2,000,000 × 100 = 100.000000`; Beta
   `2,000,000/2,200,000 × 100 = 90.909091`; Gamma `2,000,000/2,480,000 × 100 = 80.645161`. At
   `w` = 0.70: Alpha `0.70 × 100 + 0.30 × 62`; Beta `0.70 × 90.909091 + 0.30 × 78`; Gamma
   `0.70 × 80.645161 + 0.30 × 92`. Then at `w` = 0.40, and the crossovers.
4. **Result.**

   | Bidder | Price (USD) | Quality | Price score | `S` at 70/30 | `S` at 60/40 | `S` at 40/60 |
   |---|---|---|---|---|---|---|
   | Alpha | 2,000,000 | 62 | 100.00 | **88.60** | 84.80 | 77.20 |
   | Beta | 2,200,000 | 78 | 90.91 | 87.04 | **85.75** | 83.16 |
   | Gamma | 2,480,000 | 92 | 80.65 | 84.05 | 85.19 | **87.46** |

   **Three different winners from one bid set.** The crossovers are Beta/Gamma at a price weight of
   **57.70 %** and Alpha/Beta at **63.77 %**: Gamma wins below 57.70 %, Beta in the band between, and
   Alpha above 63.77 %.
5. **Interpretation.** The first and most important reading is procedural, not commercial: **with the
   bids in hand, a panel could choose a weighting to produce any of the three outcomes it liked.** That
   is why the weighting must be published before bids are opened (10.2.3), and it is not a formality —
   it is the only thing standing between an evaluation and a rationalisation. The second reading is
   about the model's fragility. Beta wins in a band **6.07 percentage points** wide, which is narrower
   than the uncertainty in the quality scores themselves; a panel whose consensus moved Beta's quality
   score by two points would relocate the boundary. A model whose output is knife-edge is telling the
   buyer something useful — that the three bids are, on this model, close to indistinguishable, and the
   decision should turn on something the model does not capture rather than on the third decimal place.
   The third reading is the commercial one, and it is the test that should discipline any quality
   weighting. Gamma's premium over Alpha is **USD 480,000** — **24.00 %**, or USD 16,000 per quality
   point. What does that buy? Suppose the panel's own risk assessment maps the quality scores to the
   probability of the integration rework that Domain 8 carries as R3 (impact USD 320,000): Alpha 0.30,
   Beta 0.18, Gamma 0.10, giving expected rework of **96,000**, **57,600** and **32,000**. Risk-adjusted
   cost is then Alpha **2,096,000**, Beta **2,257,600**, Gamma **2,512,000** — and Alpha is still
   cheapest by a wide margin, because Gamma's 480,000 premium buys only `96,000 − 32,000 =` **64,000**
   of avoided expected cost. The premium is **7.5 times** the risk it removes. Add a generous allowance
   for schedule as well — say Gamma's quality also avoids three weeks of integration delay at 45,000 a
   week, worth 135,000 — and the total avoided is 199,000, still **281,000** short. Expressed as a
   breakeven: Gamma's premium is worth paying only if its quality avoids more than 480,000 of expected
   cost, which at Auriga's cost of delay is **10.67 weeks** of delay. That is the professional obligation
   in a sentence: **a quality weighting is a statement about how much money quality is worth, and if
   the number it implies cannot be defended against the risks it is supposed to remove, the weighting
   is not a judgement, it is a preference.** Two cautions. Quality may buy things that are genuinely
   outside this arithmetic — safety performance, regulatory standing, workforce capability retention,
   the ability to be relied on in a crisis — and where it does, those must be **named and, so far as
   possible, priced**, not asserted as a reason the arithmetic does not apply. And the risk mapping
   above is the panel's own assessment; it must be recorded as an assumption with its basis, because
   the whole conclusion is proportional to it.

> **Fig 10.2.1 — The weighting chooses the winner.** Line chart, x-axis the weight given to price from
> 0 % to 100 % (quality takes the remainder), y-axis weighted evaluation score 60 to 102 points. Three
> straight lines: Alpha (2,000,000) rising from 62 to 100.00; Beta (2,200,000) rising from 78 to 90.91;
> Gamma (2,480,000) falling from 92 to 80.65. Three shaded winning bands — Gamma below **57.70 %**,
> Beta between 57.70 % and **63.77 %**, Alpha above 63.77 % — with the two crossover weightings marked.
> Two points labelled: **40/60 → Gamma 87.46** and **70/30 → Alpha 88.60**. Side note: same three bids,
> three winners; fix the weighting before opening. Source: PCI original. Alt text: three straight lines
> crossing on a score-versus-price-weight chart, dividing the horizontal axis into three shaded regions
> in which a different bidder achieves the highest score.

**Normalisation is part of the model, and it moves the answer too.** The ratio convention above is one
of several. A linear convention — `100 × (1 − (bid − lowest)/lowest)` — gives Alpha 100, Beta **90.00**
and Gamma **76.00**, because it penalises price differences more steeply. Under it the winner still
changes with the weighting, but the boundaries move: Beta/Gamma crosses at **50.00 %** and Alpha/Beta at
**61.54 %**. The Beta/Gamma boundary has shifted by **7.70 percentage points** purely because of a
choice about arithmetic that most invitation documents do not state. The obligation is therefore wider
than publishing the weights: **publish the normalisation formula, the scoring scale, the moderation
process and the treatment of non-compliance**, because each of them is a lever on the outcome.

**Testing a bid for abnormal pricing.** A price far below the others may be an efficiency the buyer
should welcome, or a bid that cannot be delivered and will return as claims (Domain 7, KA 7.4.2). The
test is quantitative and must specify its benchmark, because the benchmark changes the answer. Against
the *bid spread*, the mean of the three bids is **USD 2,226,666.67** and Alpha is **10.18 %** below it —
enough to trigger a clarification duty under most published thresholds. Against the *buyer's own
estimate*, Alpha at 2,000,000 sits exactly on Auriga's target cost, and is not low at all. Both
readings are true, and the professional response is neither to disqualify nor to ignore: it is to seek a
structured explanation of the price build-up, satisfy the panel that the scope and the risk are
actually covered, and record the answer — because if the bid is genuinely under-priced, the buyer will
meet the difference again as variations, and the record is what makes that argument winnable.

### 10.2.3 Probity, and fixing the model before the bids are opened

**The obligation.** The evaluation model — weights, normalisation, scoring scale, sub-criteria and their
weights, the treatment of non-compliant bids — must be **fixed, documented and disclosed to bidders
before bids are opened**, and not changed afterwards. This is a professional obligation independent of
whatever a particular jurisdiction's procurement law requires, and 10.2.2 is the reason: the model
selects the winner, so a model chosen after the prices are known is not an evaluation method but a
means of choosing a supplier while appearing not to.

**The mechanics that make it real**, in the order they occur. Publish the model in the invitation, with
sub-criteria weights, not merely the top-level split. Receive bids to a single controlled point with a
recorded receipt time, and hold them unopened. **Separate the price and quality submissions** and score
quality *before* any price is seen — this is the single most effective anchoring control available, and
its absence is the commonest defect in otherwise diligent processes. Have each panel member score
independently and record those scores before moderation, so that the moderation is a documented
convergence rather than a consensus with no history. Record the reason for every score, especially the
low ones, since that is what a debrief and any challenge will test. Manage conflicts of interest by
declaration and exclusion, before the panel is constituted. And where a change to the model becomes
genuinely unavoidable — a criterion turns out to be unassessable — **re-run the evaluation under the
original model as well**, disclose both results, and have the decision taken by an authority senior to
the panel, because the only defence against a post-hoc change is transparency about what it altered.

**Debriefing, and why it is a commercial instrument.** A specific, evidence-based debrief to unsuccessful
bidders costs a few hours and buys three things: better bids next time, a market that continues to bid,
and a documented rationale that has already survived contact with the party most motivated to attack
it. A vague debrief buys a challenge.

### AI in this KA

**Where it earns its place.** Compliance checking a bid against a long requirements schedule and listing
omissions and qualifications — high-volume, low-judgement, and exactly where human panels tire and miss
things. Extracting a comparable price build-up from bids submitted in different formats, so that like is
compared with like. Sweeping the evaluation model across weightings and normalisation conventions to
reveal how sensitive the outcome is — the arithmetic of 10.2.2 run over hundreds of combinations, which
is deterministic, verifiable and genuinely informative *if it is done before bids are opened, as model
design*. Drafting the clarification questions that a price build-up implies.

**Where it must not go.** It must not score quality. A quality score is an expert judgement for which a
named human must be answerable, and an AI-generated score cannot be defended in a debrief or a
challenge because no one can explain it in the terms the criterion was written in. It must not be used
to sweep weightings **after** bids are opened — the same computation that is good model design before
opening is the mechanism of a rigged evaluation after it, and the difference is entirely one of timing.
And it must not decide non-compliance, which is a contractual judgement with legal consequences.

**Verification, concretely.** Any AI-produced compliance list is confirmed against the bid text before
it affects a score, with the confirmation recorded against the bidder. Any AI-assembled price
comparison is reconciled to the bidders' own totals to the cent. The evaluation arithmetic itself —
weighted scores and crossovers — is reproduced by hand for the top two bidders, because it is four
multiplications and the whole award rests on it. And the audit trail must show that any weighting
sensitivity analysis was performed and locked **before** the receipt deadline, with its date.

### Key terms — KA 10.2

| Term | Meaning |
|---|---|
| **Route to market** | The competitive structure chosen for a procurement; each route buys a different amount of price discovery at a different cost in time. |
| **Aggregation illusion** | Treating a framework call-off as competitively priced because the framework was competed; the rate may have drifted from the market. |
| **Evaluation model** | Weights, normalisation, scoring scale and moderation rules; a piece of arithmetic that materially selects the winner. |
| **Price normalisation** | The formula converting prices into scores; the ratio and linear conventions give different scores and different crossovers. |
| **Crossover weighting (`w*`)** | The price weight at which two bidders' weighted scores are equal. |
| **Quality premium test** | Comparing a higher-quality bid's price premium with the expected cost it demonstrably avoids. |
| **Abnormally low tender** | A bid materially below its benchmark; the benchmark must be stated, since the bid spread and the buyer's estimate can disagree. |
| **Two-envelope evaluation** | Scoring quality before price is revealed, to remove price anchoring from quality judgement. |

### Sample MCQs — KA 10.2

**MCQ 10.2-A `[10.2.2 · Application]`** Bids: Alpha 2,000,000 quality 62; Beta 2,200,000 quality 78;
Gamma 2,480,000 quality 92. Price is scored `lowest ÷ own × 100`. At a 70/30 price/quality weighting the
winner and score are:
- A. Gamma, 84.05
- B. Beta, 87.04
- C. Alpha, 88.60 ✅
- D. Alpha, 81.00

*Rationale:* `0.70 × 100 + 0.30 × 62 = 88.60`, against Beta 87.04 and Gamma 84.05 (10.2.2). A and B name
the other bidders' correct scores but not the winner at this weighting; D is Alpha's score at a 50/50
weighting.

**MCQ 10.2-B `[10.2.2 · Analysis]`** For the same three bids, the price weight at which Beta overtakes
Gamma is closest to:
- A. 50.00 %
- B. 57.70 % ✅
- C. 60.78 %
- D. 63.77 %

*Rationale:* `w* = (92 − 78)/[(90.909091 − 78) − (80.645161 − 92)] = 14/24.263930 = 57.70 %` (10.2.2).
A is the Beta/Gamma crossover under the *linear* normalisation convention, not the ratio convention. C is
the Alpha/Gamma crossover, which lies inside Beta's winning band and is therefore not a boundary at all.
D is the Alpha/Beta crossover.

**MCQ 10.2-C `[10.2.2 · Evaluation]`** Gamma's price premium over Alpha is 480,000. On the panel's own
risk mapping, Gamma's higher quality reduces expected integration rework from 96,000 to 32,000. The
strongest statement a leader can make about the premium is that it is:
- A. justified, because Gamma scores 30 quality points higher
- B. 7.5 times the expected cost it avoids, so it must be justified by something the risk assessment does not capture — named and priced ✅
- C. unjustifiable in all circumstances
- D. justified, because 480,000 is only 24 % of Alpha's price

*Rationale:* `480,000/(96,000 − 32,000) = 7.5` (10.2.2). A and D restate inputs as conclusions. C
overreaches: quality may buy safety, regulatory standing or capability the mapping omits — but those
must be named, not assumed.

**MCQ 10.2-D `[10.2.3 · Analysis]`** A panel opens the priced envelopes, then confirms the price/quality
weighting at 40/60. The defect is that:
- A. 40/60 is too low a price weighting for a construction package
- B. the weighting should have been an odd split to avoid ties
- C. with the bids known, the weighting can be selected to produce any of three winners, so a model fixed after opening is not an evaluation method ✅
- D. the panel should have used the linear normalisation convention

*Rationale:* The bid set in 10.2.2 has three different winners across the weighting range, so post-hoc
weighting selects the supplier (10.2.3). A and D are matters of judgement, not defects; B is invented.

**MCQ 10.2-E `[10.2.2 · Analysis]`** Alpha's 2,000,000 bid is 10.18 % below the mean of the three bids
but exactly equal to the buyer's own target cost. The correct professional response is to:
- A. disqualify Alpha as abnormally low
- B. ignore the test, since the bid matches the buyer's estimate
- C. seek a structured explanation of the price build-up, satisfy the panel that scope and risk are covered, and record the answer ✅
- D. average the two benchmarks and apply the threshold to the result

*Rationale:* The two benchmarks disagree, which is information rather than a tie to be broken; the
response is clarification and a record, because an under-priced bid returns as variations (10.2.2,
Domain 7 KA 7.4.2). D invents a procedure.

### Self-check — KA 10.2

1. *Why must the evaluation weighting be published before bids are opened?* — Because the weighting
   selects the winner: one bid set can yield three different winners across the weighting range.
2. *What does a very narrow winning band tell the buyer?* — That the bids are close to
   indistinguishable on this model, so the decision should turn on something the model does not capture.
3. *Name the control that removes price anchoring from quality scoring.* — Two-envelope evaluation:
   score quality before any price is revealed.

---

## Knowledge Area 10.3 — Contract strategy and supplier governance

*Topics: 10.3.1 contract strategy as risk allocation · 10.3.2 incentive arithmetic: buyer and supplier
outturn · 10.3.3 supplier governance and performance regimes.*

### 10.3.1 Contract strategy as risk allocation

**The starting point, cited not re-derived.** Domain 7 KA 7.4.2 sets out the contract models — firm
fixed price, fixed-price incentive, cost plus fixed fee, cost plus incentive fee, time and materials —
ordered by who carries cost risk, together with the discipline that **risk transferred is risk priced**.
That taxonomy is assumed here. What this KA adds is the arithmetic of what each model does to the two
parties' money at the same actual cost, and what that predicts about behaviour.

**The four questions a contract strategy must answer.** *Who carries which risk* — and the test of a
sound allocation is whether the party carrying a risk can actually **influence** it; a risk allocated to
a party with no control over it is not managed, it is priced, and priced badly. *What the payment
mechanism rewards* — the marginal-dollar question of 10.3.2. *What happens when things go wrong* — the
change, notice, suspension, step-in and termination machinery of KA 10.4, which must exist before it is
needed. *What the buyer is entitled to know* — access, records, audit and progress data granular enough
to run Domain 7's earned-value system across the boundary, because a supplier reporting percentage
complete on its own definition supplies comfort, not control.

**Packaging, and the interface cost of splitting work.** How work is divided between contracts is a
contract-strategy decision with a computable cost, and Domain 4's result applies directly: `n` packages
let by a buyer who integrates them itself create up to `n(n−1)/2` pairwise interfaces, all of which the
buyer owns by default (Domain 4, KA 4.2.3). Five packages create ten interfaces; a single
main-contract-plus-subcontracts structure creates five to one integrator. Splitting work buys price
tension in each package and sells the integration risk to yourself, and the trade should be stated in
those terms: **the price saving from splitting must exceed the cost of the interfaces it creates**, and
on a technically integrated scope it frequently does not.

### 10.3.2 Incentive arithmetic: buyer and supplier outturn

**The principle.** A contract's payment mechanism determines, for every additional dollar of cost, who
pays it. That allocation is what the supplier's management responds to at month eight, long after the
strategy paper has been filed.

**The marginal-dollar allocation identity.** For any payment mechanism, at any cost level,

```
buyer's share of the next dollar of cost + supplier's share = 1
```

The identity is trivial and its consequences are not: **a contract cannot make a dollar of cost
disappear; it can only decide who holds it.** Every claim that a structure "removes cost risk" is
therefore a claim about allocation, and the allocation can be read off the mechanism:

| Mechanism | Buyer's share of the marginal dollar | Supplier's share | What the supplier is rewarded for |
|---|---|---|---|
| Firm fixed price | 0.00 | 1.00 | Efficiency — and, unless specification and inspection are tight, for reducing quality and scope |
| Cost plus fixed fee | 1.00 | 0.00 | Nothing; cost recovery is certain and effort is not rewarded |
| Target cost, 70/30 share, **below** the `PTA` | 0.70 | 0.30 | Efficiency, at 30 cents in the dollar — both parties lose on overrun |
| Target cost, **above** the `PTA` | 0.00 | 1.00 | Entitlement, not efficiency: the marginal incentive is a fixed price the supplier never priced |

The last row is Domain 7's point of total assumption read as a behavioural statement rather than a
commercial one, and Auriga's installation subcontract puts it at **USD 2,428,571.43** (Domain 7,
KA 7.4.3 — target cost 2,000,000, target fee 150,000, share 70/30, ceiling 2,450,000). Nothing about
that figure is re-derived here; what matters here is that it is the cost level at which the supplier's
marginal share jumps from 0.30 to 1.00, and therefore the level at which a cooperative relationship
becomes an adversarial one for reasons that have nothing to do with the people involved.

**Worked example 10.3.2 — the same scope, three mechanisms, one cost distribution.**

1. **Setup.** Auriga's installation scope. The supplier's own assessment of its outturn cost, disclosed
   in negotiation: **1,850,000** with probability **0.20**, **2,000,000** with **0.40**, **2,300,000**
   with **0.30**, **2,600,000** with **0.10**. The supplier requires an expected margin of
   **USD 150,000**. Three candidate mechanisms: a firm fixed price; cost plus a fixed fee of 150,000;
   and the target-cost structure of Domain 7 (target cost 2,000,000, target fee 150,000, share 70/30,
   ceiling 2,450,000, `PTA` 2,428,571.43).
2. **Formula.** `E[cost] = Σ p × cost`. Fixed price (risk-neutral) `= E[cost] + required margin`.
   Cost-plus buyer outturn `= cost + fee`. Target-cost fee `= target fee − overrun × supplier share`
   (negative overrun increases the fee), buyer outturn `= cost + fee`, capped at the ceiling. Supplier
   margin `=` buyer outturn `−` cost.
3. **Substitution.** `E[cost] = 0.20 × 1,850,000 + 0.40 × 2,000,000 + 0.30 × 2,300,000 +
   0.10 × 2,600,000 = 370,000 + 800,000 + 690,000 + 260,000`. Fixed price `= 2,120,000 + 150,000`.
   Target-cost fee at 1,850,000: `150,000 + 150,000 × 0.30 = 195,000`; at 2,300,000:
   `150,000 − 300,000 × 0.30 = 60,000`; at 2,600,000 the uncapped outturn 2,570,000 exceeds the ceiling,
   so the buyer pays 2,450,000 and the supplier absorbs the rest.
4. **Result.** `E[cost]` = **USD 2,120,000**; risk-neutral firm fixed price **USD 2,270,000**.

   | Actual cost | Firm fixed price: buyer / supplier margin | Cost plus fixed fee: buyer / margin | Target cost: buyer / margin |
   |---|---|---|---|
   | 1,850,000 | 2,270,000 / **420,000** | 2,000,000 / 150,000 | 2,045,000 / 195,000 |
   | 2,000,000 | 2,270,000 / 270,000 | 2,150,000 / 150,000 | 2,150,000 / 150,000 |
   | 2,300,000 | 2,270,000 / (30,000) | 2,450,000 / 150,000 | 2,360,000 / 60,000 |
   | 2,600,000 | 2,270,000 / **(330,000)** | 2,750,000 / 150,000 | 2,450,000 / (150,000) |
   | **Expected** | **2,270,000 / 150,000** | **2,270,000 / 150,000** | **2,222,000 / 102,000** |

   The two expected buyer outturns for fixed price and cost-plus are **identical at USD 2,270,000**. The
   target-cost structure gives the buyer **USD 2,222,000** and the supplier **USD 102,000** — the
   buyer's **48,000** saving is exactly the supplier's **48,000** shortfall.
5. **Interpretation.** Three results, each of which changes how a leader argues about contract type.

   **First: at risk-neutral pricing, transferring risk is free in expectation and buys only variance.**
   The fixed price and the cost-plus arrangement cost the buyer the same 2,270,000 on average, because
   the fixed-price premium of `2,120,000 − 2,000,000 =` 120,000 over the efficient cost *is* the expected
   cost of the risk. What the buyer gains is certainty: the standard deviation of its outturn falls from
   **USD 230,434** under cost-plus to **zero** under fixed price. That is the honest case for fixed price
   — not that it is cheaper, but that it converts a distribution into a number — and it is worth paying a
   real premium for where the buyer's own funding or covenants cannot absorb the variance. A risk-averse
   supplier will charge one: a 25 % loading on the 120,000 risk element takes the price to
   **USD 2,300,000**, and that 30,000 is the visible, arguable price of certainty. The corollary is the
   part buyers forget: the transfer is only as good as the supplier's balance sheet. At the 2,600,000
   outcome the fixed-price supplier loses **330,000**, and a supplier that cannot absorb that does not
   deliver a fixed price — it delivers an insolvency and a re-procurement, at which point the risk
   returns to the buyer with the delay added (KA 10.4.4).

   **Second: an incentive structure does not create value, it reallocates it — unless behaviour
   changes.** As specified, the target-cost arrangement is simply 48,000 transferred from supplier to
   buyer, and a supplier that can do this arithmetic will respond by raising the target cost, raising the
   target fee, or declining. The reason is structural and worth stating precisely: **the target fee is
   the fee at the target, not the expected fee.** Any real probability of overrun makes the expected fee
   less than the target fee, so a supplier facing the distribution above expects 102,000 against a
   requirement of 150,000. Negotiations that stall over "the fee" are usually stalling over this,
   unnamed.

   **Third: the behaviour change required can be computed, and it is the number the negotiation is
   actually about.** Suppose the incentive causes the supplier to shift probability towards the underrun
   outcome, the remaining mass staying in its original 3:1 ratio between the 2,300,000 and 2,600,000
   branches. The supplier's expected margin is then `187,500q + 64,500` where `q` is the probability of
   the 1,850,000 outcome, so it reaches 150,000 at **`q` = 0.456** — the underrun probability must rise
   from 0.20 to 0.456, a factor of **2.28**. At that point expected cost falls from 2,120,000 to
   **USD 1,985,600**, so the incentive has created **USD 134,400** of genuine value; the buyer's expected
   outturn is **USD 2,135,600**, which is 134,400 below the fixed price; and the supplier is exactly
   indifferent. In other words, **at the point where the supplier will just accept the structure, the
   buyer captures the entire value created.** That is the real subject of the negotiation: not the fee in
   the abstract, but how the 134,400 is split, and a buyer who understands this can concede part of it
   knowing precisely what it is buying — a supplier with an actual reason to be efficient. The
   professional cautions are three. The distribution is the *supplier's* assessment and it has every
   incentive to shade it pessimistically, so it must be tested against comparable outturns rather than
   accepted. The behaviour shift is a hypothesis, not a fact; the structure should therefore carry
   measurable efficiency commitments rather than resting on the incentive alone. And none of this
   arithmetic survives contact with a badly defined scope: on undefined scope every mechanism converges
   on cost-plus with a dispute attached, whatever the contract says (Domain 7, KA 7.4.2).

### 10.3.3 Supplier governance and performance regimes

**The principle.** A contract is a document until someone administers it. Supplier governance is the
machinery that turns obligations into observed performance, and its design errors are the same three
every time: it measures what is easy rather than what matters, it meets on a cadence slower than the
decisions it must take, and its sanctions are too small to change anyone's behaviour.

**Governing across the boundary.** Domain 3's structures apply, with three additions specific to a
contract. Decision rights must be **mapped on both sides**, with the threshold mismatch of 10.1.1
resolved explicitly. The governance cadence must be set against the decision rate, not the reporting
rhythm: a supplier review every eight weeks with a two-week paper deadline imposes
`E[wait] = 8/2 + 2 =` **6 weeks** on every decision it owns (Domain 3, KA 3.2.3), which is longer than
most delivery problems will politely wait, so an out-of-cycle route with a named decision-maker on each
side is not a refinement but a requirement. And the buyer must specify the **data** it receives — cost
and progress at a granularity that lets its own earned-value system run across the boundary — because a
governance body reading the supplier's own summary is governing the summary.

**Performance regimes: what a cap actually does.** Service credits, liquidated sums for late delivery
and KPI-linked fee adjustments all work by making failure expensive. Whether they work at all depends on
a comparison almost nobody makes: **the cap against the supplier's cost of compliance.**

Auriga's five-year support contract runs at **USD 2,200,000** a year with service credits capped at
**5 %** — **USD 110,000**. Meeting the availability target requires the supplier to hold a standby crew
costing **USD 180,000** a year. The supplier's arithmetic is immediate: fail, pay the cap, and save
**USD 70,000**. The regime has not deterred non-performance; it has **priced** it, and at a discount.
Two different caps follow from two different purposes, and confusing them is the design error. For the
credits to *deter*, the cap must exceed the cost of compliance — at least `180,000/2,200,000 =`
**8.18 %** of contract value. For the credits to *compensate*, they must cover the buyer's own loss from
unavailability, assessed at **USD 320,000** a year, which needs `320,000/2,200,000 =` **14.55 %**. At the
5 % cap actually agreed, the credits recover **34.38 %** of the buyer's loss and deter nothing. The
professional discipline is to state which purpose the regime serves, compute the cap that purpose
requires, and — where the required cap is unobtainable in negotiation — record that the residual
exposure is the buyer's, so that it appears in the risk register at its real size rather than being
assumed away because "there are service credits".

**Relationship management, and why it is not softness.** Suppliers allocate their best people by
judgement, not by contract, and they allocate them away from clients who are slow to pay, slow to decide,
late with access and inclined to treat every issue as a breach. The behaviours that keep a supplier's
A-team on a project are concrete and cheap: decide inside the contractual period, pay to terms, give
access and information when promised, escalate issues before they become entitlements, and keep the
commercial conversation separate from the delivery one so that neither poisons the other. **A buyer who
is difficult to work with pays for it in the next tender and in every discretionary decision the supplier
takes in between** — a real cost that appears in no report.

### AI in this KA

**Where it earns its place.** Contract analytics at scale: extracting obligations, notice periods,
liability caps, change mechanisms and termination rights from a portfolio of agreements into a
structured obligations register, and flagging inconsistencies between the main agreement and its
schedules. Modelling the payment mechanism across cost outcomes — the whole of 10.3.2, swept over
share ratios, ceilings and distributions — which is deterministic and tedious. Monitoring supplier
performance data for trend breaks. Comparing a supplier's reported progress with its own historical
reporting patterns to flag optimistic drift.

**Where it must not go.** It must not give legal or commercial advice, and it must not draft a clause
that is signed without qualified review; the boundary Domain 7 states holds here in full. It must not
set a liability cap, a share ratio or a target cost — those are risk-appetite decisions belonging to
accountable people. It must not generate the supplier's cost distribution: a model asked for
probabilities will supply them, and they will look exactly like data. And an AI reading of a clause is a
summary; the obligation is the clause.

**Verification, concretely.** Every extracted obligation is cited to a clause number and spot-checked at
a stated sample rate before the register is relied on. Fee, share and `PTA` arithmetic is reproduced by
hand for at least three cost points, including one above the `PTA`, because that is where the mechanism
changes shape. Any distribution used in an incentive model carries a named source, and the paper states
the result at the pessimistic and optimistic ends, not only at the mean.

### Key terms — KA 10.3

| Term | Meaning |
|---|---|
| **Marginal-dollar allocation identity** | For any payment mechanism, buyer share + supplier share of the next dollar of cost = 1; a contract allocates cost, it does not remove it. |
| **Point of total assumption (`PTA`)** | The cost above which the supplier bears 100 % of further overrun (Domain 7, KA 7.4.3); read here as the point at which behaviour turns from efficiency to entitlement. |
| **Risk-neutral fixed price** | Expected cost plus the required margin; at this price, transferring risk costs the buyer nothing in expectation and buys variance reduction only. |
| **Risk loading** | The premium a risk-averse supplier adds above the risk-neutral price; the arguable, visible price of certainty. |
| **Target fee vs expected fee** | The fee at the target cost, against the probability-weighted fee actually expected; any real overrun risk makes the second lower than the first. |
| **Influence test** | A risk should be allocated to the party that can influence it; a risk allocated elsewhere is priced, not managed. |
| **Deterrence cap vs compensation cap** | The service-credit cap needed to exceed the supplier's cost of compliance, against the cap needed to cover the buyer's loss; they differ. |
| **Out-of-cycle route** | Domain 3's mechanism (KA 3.3.3) applied across a contract boundary: named authorities on **both** sides, since a route that exists for only one party is not a route. |

### Sample MCQs — KA 10.3

**MCQ 10.3-A `[10.3.2 · Application]`** A supplier's cost outcomes are 1,850,000 (0.20), 2,000,000
(0.40), 2,300,000 (0.30) and 2,600,000 (0.10), and it requires a 150,000 expected margin. The
risk-neutral firm fixed price is:
- A. USD 2,150,000
- B. USD 2,270,000 ✅
- C. USD 2,120,000
- D. USD 2,450,000

*Rationale:* `E[cost] = 2,120,000`, so the price is `2,120,000 + 150,000 = 2,270,000` (10.3.2). A adds
the margin to the *target* cost rather than the expected cost — the commonest error, and it understates
by 120,000. C is the expected cost with no margin. D is the ceiling of the target-cost alternative.

**MCQ 10.3-B `[10.3.2 · Analysis]`** Under a target-cost contract with a 70/30 share, above the point of
total assumption the buyer's and supplier's shares of the next dollar of cost are:
- A. 0.70 and 0.30, unchanged
- B. 0.00 and 1.00 ✅
- C. 1.00 and 0.00
- D. 0.50 and 0.50

*Rationale:* Above the `PTA` the ceiling binds the buyer, so the supplier carries every further dollar
(Domain 7 KA 7.4.3; 10.3.2). A misses that sharing has stopped; C reverses the exposure; D invents a
split.

**MCQ 10.3-C `[10.3.2 · Evaluation]`** The expected buyer outturn is 2,270,000 under both firm fixed
price and cost plus fixed fee, while the outturn standard deviation is 230,434 under cost-plus and zero
under fixed price. The correct conclusion is that at risk-neutral pricing, fixed price:
- A. is cheaper in expectation and therefore always preferable
- B. costs the same in expectation and buys variance reduction, whose value depends on the buyer's ability to absorb variance — and on the supplier's solvency ✅
- C. is more expensive in expectation by the risk premium
- D. eliminates the cost risk from the project

*Rationale:* The premium equals the expected cost of the risk, so nothing is gained in expectation
except certainty (10.3.2). D contradicts the marginal-dollar identity — the risk is allocated, not
removed, and returns if the supplier fails.

**MCQ 10.3-D `[10.3.2 · Analysis]`** As specified, the target-cost structure gives the buyer an expected
outturn 48,000 below the fixed price and the supplier an expected margin 48,000 below its requirement.
The most useful inference is that:
- A. the target-cost structure is superior for the buyer and should be used
- B. an incentive structure reallocates value unless it changes behaviour; the supplier will raise the target cost or fee, or decline ✅
- C. the supplier has mis-stated its cost distribution
- D. the share ratio should be 50/50

*Rationale:* The buyer's gain is exactly the supplier's loss, so no value is created (10.3.2). A treats a
transfer as an improvement and will not survive negotiation; C is possible but not inferable; D is an
unmotivated fix.

**MCQ 10.3-E `[10.3.3 · Evaluation]`** Service credits are capped at 5 % of a 2,200,000 contract.
Compliance costs the supplier 180,000 a year and non-performance costs the buyer 320,000 a year. The
regime:
- A. deters non-performance, because 110,000 is a material sum
- B. prices non-performance at a 70,000 discount to compliance, and recovers only 34.38 % of the buyer's loss ✅
- C. is adequate because service credits are a secondary remedy
- D. should be capped at 5 % of the buyer's loss instead

*Rationale:* `0.05 × 2,200,000 = 110,000 < 180,000`, so failing is the supplier's cheaper course; and
`110,000/320,000 = 34.38 %` of the buyer's loss (10.3.3). Deterrence needs 8.18 %, compensation 14.55 %.

### Self-check — KA 10.3

1. *State the marginal-dollar allocation identity and its consequence.* — Buyer share + supplier share
   of the next dollar of cost equals 1; a contract allocates cost, it cannot remove it.
2. *Why does a supplier expect less than the target fee?* — Because the target fee is the fee at the
   target cost; any real probability of overrun reduces the probability-weighted fee below it.
3. *What two different caps can a service-credit regime need, and why?* — A deterrence cap exceeding the
   supplier's cost of compliance (8.18 % on Auriga's support contract) and a compensation cap covering
   the buyer's loss (14.55 %); they serve different purposes and rarely coincide.

---

## Knowledge Area 10.4 — Claims, change and disputes; ethical sourcing; supply resilience

*Topics: 10.4.1 claims awareness and the anatomy of an entitlement · 10.4.2 dispute avoidance and the
escalation ladder · 10.4.3 ethical and sustainable sourcing · 10.4.4 supply-chain resilience: single,
dual-split and dual-qualified sourcing.*

### 10.4.1 Claims awareness and the anatomy of an entitlement

**The professional position.** A project leader is not a lawyer and should not act as one. What a leader
must be able to do is recognise a claim situation as it forms, preserve the record that will decide it,
and quantify the exposure well enough to escalate it accurately. **Claims are decided on
contemporaneous records, and the records are made — or not made — long before anyone uses the word
claim.**

**The four elements of a claim**, all of which must be present. **Entitlement** — an identified
contractual basis: an instruction, a variation, a relevant event, a breach. Without it the merits do not
matter. **Causation** — a demonstrated link from that basis to the effect claimed, which is where most
claims are actually won or lost, because concurrent causes and the claimant's own delays are pleaded
against it. **Quantum** — the money, built up by head of cost and evidenced. **Notice** — given in the
form and within the period the contract requires, which in many contracts is a precondition rather than
a courtesy.

**The heads of cost, and what each requires.** *Direct cost* — additional labour, plant and materials,
evidenced by records at the time. *Prolongation* — time-related site and management costs for the period
of extended duration; requires a demonstrated critical-path effect (Domain 6), not merely a late
finish. *Disruption* — lost productivity on work that was not itself changed, the hardest head to prove
and usually established by a **measured mile**: comparing productivity in an unimpacted period with the
impacted one. *Finance cost* on the funded difference, where the contract or the governing law allows it.
*Overhead and profit*, at the contractual percentage. Acceleration and mitigation costs, where
instructed or reasonably incurred.

**Worked example 10.4.1 — a variation claim, and what a notice provision puts at risk.**

1. **Setup.** Auriga's installation supplier is instructed to relocate two remote-terminal-unit
   cabinets. It claims: **240 hours** of additional labour at the blended engineering rate of
   **USD 130.625** per hour (Domain 7, KA 7.4.1); **USD 18,400** of materials; **1.5 weeks** of
   prolongation at **USD 14,000** per week of site establishment; and disruption on **1,204** planned
   hours of unaffected work whose productivity fell to a factor of **0.86** of the measured-mile
   baseline. Overhead and profit is **12 %** by the contract. The instruction issued on day 1; the
   supplier gave notice on **day 41**, against a contractual requirement of **28 days**, and under this
   contract the prolongation and disruption heads depend on valid notice.
2. **Formula.** Claim `= (direct + prolongation + disruption) × (1 + OH&P)`, with disruption
   `= (planned hours ÷ productivity factor − planned hours) × rate`. Notice-bar exposure `=` (heads
   dependent on notice) `× (1 + OH&P)`.
3. **Substitution.** Labour `240 × 130.625`; disruption hours `1,204/0.86 = 1,400`, so extra
   `1,400 − 1,204 = 196` hours at 130.625; prolongation `1.5 × 14,000`. Subtotal then `× 1.12`.
4. **Result.** Labour **USD 31,350.00**; materials 18,400; prolongation **USD 21,000**; disruption
   **USD 25,602.50**. Subtotal **USD 96,352.50**; overhead and profit **USD 11,562.30**; **total claim
   USD 107,914.80** (≈ SAR 404,681 indicatively). The heads exposed to the notice bar total
   **USD 52,194.80** — **48.37 %** of the claim — leaving **USD 55,720.00** unaffected.
5. **Interpretation.** Nearly half of a properly constructed claim is at risk from a diary entry nobody
   made, and that is the single most useful thing a delivery leader can know about claims: **notice
   provisions destroy more value than negotiation does, and they do it silently.** The lesson runs in
   both directions, which is what makes it a professional rather than a tactical point. As a *seller*,
   the discipline is a notice register — every instruction, every relevant event, logged on the day with
   its notice deadline computed, because the 28 days runs from the event and not from the moment the
   cost becomes obvious. As a *buyer*, resisting a genuine entitlement purely on a time bar buys a
   number and sells a relationship: the supplier's response is predictable and expensive — its A-team
   moves, its pricing of every subsequent variation stiffens, and its own notices thereafter arrive on
   day one for everything, generating administrative load out of all proportion to the 52,194.80. Three
   further cautions. **Whether a notice provision of this kind operates as a condition precedent varies
   by jurisdiction and by the law of the contract**; in some legal systems such bars are enforced
   strictly, in others they may be read down or fall foul of statutory controls, and nothing here is
   legal advice or a statement of any particular jurisdiction's law. The measured-mile figure is only as
   good as the comparison period — an unimpacted baseline drawn from a different crew, season or work
   type is not a measured mile, and this is the head most often successfully challenged. And the
   productivity arithmetic must be done the right way round: dividing by 0.86 (giving 196 extra hours)
   is correct, whereas multiplying by 0.14 gives 168.56 hours and understates the claim by 27.44 hours,
   or **USD 3,584.35** before overhead and profit.

### 10.4.2 Dispute avoidance and the escalation ladder

**The principle.** Disputes are expensive in a way that is systematically mis-estimated, because the
irrecoverable cost of resolving one is compared with the amount in issue only after the decision to
fight has already been taken.

**The ladder, and what each rung costs.** Direct negotiation between the people accountable for
delivery; then structured negotiation between executives outside the project; then a neutral evaluation
or mediation; then a determinative but interim process — adjudication, expert determination or a dispute
board, depending on the contract and the jurisdiction; then arbitration or litigation. Cost and elapsed
time rise by roughly an order of magnitude across the ladder, and **the relationship's usable life ends
somewhere around the fourth rung**, which matters when the supplier still has work to do.

**Worked example 10.4.2 — the arithmetic of settling.**

1. **Setup.** A **USD 400,000** claim is in dispute at week 30 of Auriga. Settlement now at **55 %** —
   USD 220,000 — is achievable, with **USD 15,000** of internal and advisory cost and **3 weeks** to
   conclude. The alternative is arbitration: the buyer's advisers assess the outcomes as the claim
   largely upheld (400,000) with probability **0.55**, partly upheld (180,000) with **0.30**, and
   dismissed (nil) with **0.15**; the buyer's own **irrecoverable** costs are **USD 340,000** whatever
   the result, and the process takes **78 weeks**.
2. **Formula.** Expected award `= Σ p × award`. Expected total `=` expected award `+` irrecoverable
   cost. Compare with the negotiated total.
3. **Substitution.** `0.55 × 400,000 + 0.30 × 180,000 + 0.15 × 0 = 220,000 + 54,000 + 0`. Expected total
   `= 274,000 + 340,000`. Negotiated total `= 220,000 + 15,000`.
4. **Result.** Expected award **USD 274,000** — **68.50 %** of the claim, above the 55 % on offer.
   Expected total cost of arbitrating **USD 614,000** against **USD 235,000** for settling: a saving of
   **USD 379,000**, or **61.73 %**, and 75 weeks of elapsed time.
5. **Interpretation.** The decisive figure is not the 379,000 but the structural one behind it: **the
   irrecoverable cost of the formal route, USD 340,000, exceeds the entire negotiated settlement of
   235,000.** On these numbers the buyer cannot come out ahead by arbitrating *even if it wins
   outright*, because a total victory saves the 235,000 it would have paid and spends 340,000 to do it —
   a net loss of 105,000 in the best case. That is the arithmetic that should be on the table before
   anyone instructs anybody, and it is very often not, because the comparison usually made is between
   the settlement and the *expected award* (274,000 against 220,000), which favours fighting and omits
   the cost of doing so. Three cautions keep this honest. Costs recovery, interest and the availability
   of interim determinative processes **vary substantially by jurisdiction and by forum**, and the
   340,000 irrecoverable assumption must be replaced with advice specific to the contract's governing
   law — this is illustrative arithmetic, not a rule. Some disputes should be fought despite the
   arithmetic: where a point of principle will otherwise be conceded across a portfolio of contracts,
   where a regulator or auditor requires the matter tested, or where settling would create a precedent
   worth more than 379,000 to the other side. And the 78 weeks carries its own cost beyond money — the
   management attention consumed, and the supplier's behaviour on the remaining work while the matter is
   live.

**The avoidance machinery, which is cheaper than any rung of the ladder.** A single agreed set of
records, maintained jointly, so that the facts are not themselves in dispute. Early-warning obligations
running both ways with short periods, so that problems arrive as information rather than as
entitlements. A change mechanism fast enough to be used — Domain 4's integrated change control across
the boundary, with the threshold mismatch of 10.1.1 resolved. A named commercial lead on each side
with authority to settle at a stated value, and an out-of-cycle route above it (Domain 3, KA 3.3.3).
And a scheduled joint review of open commercial items, so that they are closed at fifty rather than
argued at five hundred.

### 10.4.3 Ethical and sustainable sourcing

**The obligation, stated carefully.** Buyers increasingly carry duties in respect of conditions in their
supply chains — labour standards and forced labour, health and safety, environmental impact, anti-bribery
and sanctions compliance, and the accuracy of what they publish about all of it. Several jurisdictions
have enacted human-rights, modern-slavery or supply-chain due-diligence legislation, and their scope,
thresholds and reporting requirements **differ materially**; the applicable regime depends on where the
buyer is established, where it sells, and the contract's governing law. Nothing in this section states
the requirements of any particular jurisdiction, and none of it is legal advice. What is portable is the
*method*: risk-tier the supply base, apply proportionate diligence, contract for the standard and for
the right to verify it, and act on what verification finds.

**What "contracting for it" means concretely.** A stated standard the supplier must meet, not an
aspiration. A right of audit — including of sub-tiers — with a duty to disclose them, which is also the
control that makes 10.4.4's resilience analysis possible. A duty to notify adverse findings. A
remediation-first response, because terminating a supplier where a labour abuse is found removes the
buyer's leverage and frequently harms the workers concerned; termination belongs at the end of an
escalation, not the start. And flow-down obligations, since the risk concentrates where the buyer's
visibility ends.

**The arithmetic, and its honest limits.** Consider a proportionate programme across Auriga's supply
base: **34** first-tier suppliers desktop-screened at **USD 1,200** each, and the **6** assessed as
highest risk audited on site at **USD 14,000** each — a programme cost of **USD 124,800**. Against it:
the assessed probability of a material breach in the supply chain over a three-year window is **0.08**,
with a consequence — remediation, re-procurement, contractual and reputational cost — assessed at
**USD 1,900,000**, giving an expected exposure of **USD 152,000** (`EMV`, Domain 8 KA 8.2.2). If the
programme is **70 %** effective at detecting and preventing, it avoids `0.70 × 152,000 =`
**USD 106,400** — which is **USD 18,400 less** than it costs.

That result is uncomfortable and it is stated deliberately, because pretending otherwise is worse. Three
things follow. First, the breakevens are useful and should be in the paper: the programme pays at a
detection effectiveness above **82.11 %**, or at a breach probability above **9.38 %** — both entirely
plausible in higher-risk categories and geographies, which is precisely why the programme should be
**risk-tiered rather than uniform**, concentrating spend where the probability is highest instead of
screening everyone equally. Second, `EMV` is a poor instrument here for the reason Domain 8 gives: it is
an average of outcomes that will not individually occur, and the consequence distribution for a serious
supply-chain finding is heavily skewed — the 1,900,000 is a mean whose upper tail includes losing a
licence, a listing or a market. A decision taken on the mean of a fat-tailed distribution is a decision
taken on the wrong statistic. Third, and decisively: **where legal duties apply, this is not an
expected-value decision at all.** Compliance is not optional because the arithmetic is marginal, and
where the obligation is one of values rather than law, the organisation should say so plainly rather
than construct a business case it does not believe. The arithmetic's proper job is to allocate the
programme's effort, not to decide whether to have one.

### 10.4.4 Supply resilience: single, dual-split and dual-qualified sourcing

**The principle.** Resilience is bought, and the thing being bought is the ability to keep going when a
supplier cannot deliver. The mistake made almost universally is to treat "two suppliers" as the product
being purchased, when what actually reduces exposure is **a qualified, exercisable alternative** — and
those are not the same purchase.

```
Expected cost of disruption = Σ P(state) × consequence(state)
Total expected cost = certain cost + expected cost of disruption
Breakeven disruption probability  p* = extra certain cost ÷ reduction in consequence
```

**Worked example 10.4.4 — Auriga's 84 controllers: three sourcing structures.**

1. **Setup.** Domain 8 carries controller lead-time slip as R1. Three structures are on the table.
   **Option 1, single source:** supplier A at **USD 9,600** per unit, one supplier qualified at
   **USD 40,000**. **Option 2, dual source with a 60/40 volume split:** A at 9,600 and B at
   **USD 10,600**, both qualified (**USD 80,000**), plus **USD 20,000** to hold two build
   configurations to a common specification. **Option 3, single award with a qualified alternate:** full
   volume to A at 9,600, both suppliers qualified (80,000), and **USD 25,000** a year to keep the
   alternate exercisable — specification maintenance and an annual sample build. Each supplier has an
   independent **0.18** probability of a disruption in the delivery window. Consequences: a single
   supplier disrupted with no qualified alternate requires a 14-week re-source —
   `14 × 45,000 = 630,000` of delay plus 120,000 of re-qualification and rework plus a 150,000
   contractual step — **USD 900,000**; under the dual split, one supplier disrupted means a 3-week
   surge ramp on the other, `3 × 45,000 = 135,000` plus a 60,000 surge premium — **USD 195,000**; with
   a qualified alternate, activation takes 5 weeks — `5 × 45,000 = 225,000` plus 70,000 of switching and
   re-qualification — **USD 295,000**.
2. **Formula.** Certain cost `= units × unit price + qualification + holding cost`. Expected disruption
   `= Σ P(state) × consequence`, with `P(exactly one of two) = 2p(1 − p)` and `P(both) = p²` under
   independence. Breakeven `p* =` extra certain cost `÷` consequence reduction.
3. **Substitution.** Option 1: `84 × 9,600 + 40,000`, disruption `0.18 × 900,000`. Option 2: blended
   unit `0.60 × 9,600 + 0.40 × 10,600 = 10,000`, so `84 × 10,000 + 80,000 + 20,000`; disruption
   `2 × 0.18 × 0.82 × 195,000 + 0.18² × 900,000`. Option 3: `84 × 9,600 + 80,000 + 25,000`, disruption
   `0.18 × 295,000`.
4. **Result.**

   | | Certain cost (USD) | Expected disruption (USD) | Total expected (USD) |
   |---|---|---|---|
   | Option 1 — single source | 846,400 | 162,000 | **1,008,400** |
   | Option 2 — dual source, 60/40 split | 940,000 | 86,724 | **1,026,724** |
   | Option 3 — qualified alternate | 911,400 | 53,100 | **964,500** |

   Option 3 is best: **USD 43,900** better than single sourcing and **USD 62,224** better than the dual
   split. Its breakeven disruption probability against Option 1 is
   `65,000/(900,000 − 295,000) =` **10.74 %**; the dual split's, from
   `510,000p² − 510,000p + 93,600 = 0`, is **24.22 %**. The maximum premium worth paying for Option 3's
   resilience — the whole reduction in expected disruption cost — is
   `162,000 − 53,100 =` **USD 108,900**, or **12.87 %** of the single-source cost.
5. **Interpretation.** The result that carries the teaching is the ranking: **the resilience is in the
   qualified alternate, not in the split volume.** Splitting the order does two things the buyer rarely
   accounts for. It pays a price premium — the blended unit cost rises from 9,600 to 10,000, worth
   33,600 on this volume — and, under independence, it **doubles the exposure surface**: the probability
   that *something* goes wrong rises from 0.18 to `1 − 0.82² =` 0.3276, and although each individual
   event is now far cheaper, the arithmetic has to earn back the premium before it helps. Dual
   qualification, by contrast, retains the full volume with the single supplier (and its discount) and
   buys only the option, at 65,000 of extra certain spend against a 108,900 reduction in expected
   exposure. That leads to the general design rule: **qualify two, award one, and keep the second
   exercisable** — with the emphasis on *exercisable*, because a second supplier that has not built the
   part for three years is a name on a list, not an alternative, and the 25,000 a year is what
   distinguishes them. The breakevens are the negotiating numbers: Option 3 pays whenever the disruption
   probability exceeds **10.74 %**, while the dual split needs more than **24.22 %** — so a colleague
   arguing for split sourcing on resilience grounds is implicitly asserting a disruption probability
   more than twice as high as the one that justifies the cheaper structure, and should be asked for it.

   **The caution that overturns everything above: correlation.** All three calculations assume the two
   suppliers fail independently. Suppose instead that both draw a critical module from a single
   sub-tier source, so that the joint probability of disruption is **0.12** rather than 0.0324 (each
   supplier's marginal probability still 0.18, leaving `0.18 − 0.12 =` 0.06 of idiosyncratic risk each).
   Then `P(exactly one) = 2 × 0.06 =` 0.12, and the expected disruption costs become
   `0.12 × 195,000 + 0.12 × 900,000 =` **USD 131,400** for Option 2, total **1,071,400**, and
   `0.06 × 295,000 + 0.12 × 900,000 =` **USD 125,700** for Option 3, total **1,037,100**. Both are now
   **worse than single sourcing's 1,008,400**: the buyer has paid for resilience and bought exposure.
   The rule to take away is blunt — **two suppliers with one sub-tier is one supplier with two
   invoices** — and the correct response is not to choose between the options but to attack the
   sub-tier: require disclosure of the critical sub-tier sources as a contractual obligation (10.4.3),
   qualify a second module design, or hold buffer stock sized to the re-source interval. This is
   Domain 8's correlation result (KA 8.A.1) in a commercial setting, and it is the reason a supply
   network must be mapped rather than assumed.

### AI in this KA

**Where it earns its place.** Building the notice register of 10.4.1 — extracting every instruction,
relevant event and correspondence item from a project's document set, computing each notice deadline and
flagging what is approaching or missed — is high-volume, deterministic work with an unambiguous right
answer, and it is exactly the work that goes undone. Assembling a chronology from thousands of documents
for a delay analysis, for expert verification. Screening a supply base against sanctions, adverse-media
and insolvency signals, and flagging changes. Reading a set of supplier disclosures to build a sub-tier
map and identify shared dependencies — the concentration that 10.4.4 shows destroys a resilience case.
Modelling sourcing options and their breakevens, as computed above.

**Where it must not go.** It must not assess entitlement, causation or the strength of a claim; those are
legal judgements on which qualified advice is required and on which a confident, unattributable opinion
is actively dangerous. It must not generate a delay analysis presented as evidence — a chronology is an
input to an expert's opinion, not the opinion. It must not decide a supplier's ethical standing from
inference: a screening or adverse-media hit is a prompt to investigate, and treating it as a finding is
both unfair and, in some jurisdictions, restricted — take advice before any such inference is acted
on. And it must not supply the probabilities in 10.4.4;
those come from delivery history and market intelligence, and a model asked for them will produce
numbers indistinguishable from data.

**Verification, concretely.** Every notice deadline the tool computes is checked against the clause and
the event date before anyone relies on it, and the notice register is reconciled to the correspondence
log at a stated sample rate. Every chronology entry cited in a claim is verified against the source
document. Every screening hit is human-reviewed before any action, with the review recorded. And the
sourcing arithmetic is reproduced by hand at two probabilities — the point estimate and the breakeven —
because the breakeven is the number that decides whether the recommendation survives an argument.

### Key terms — KA 10.4

| Term | Meaning |
|---|---|
| **Entitlement** | The contractual basis for a claim; without it, merits are irrelevant. |
| **Causation** | The demonstrated link from the contractual basis to the effect claimed; where most claims are decided. |
| **Quantum** | The claim's money, built up by head of cost and evidenced contemporaneously. |
| **Notice provision** | The required form and period for notifying a claim; in some contracts and jurisdictions a condition precedent. |
| **Prolongation** | Time-related costs of extended duration; requires a demonstrated critical-path effect. |
| **Disruption / measured mile** | Lost productivity on unchanged work, established by comparing an unimpacted period's productivity with the impacted period's. |
| **Notice-bar exposure** | The value of the heads of claim that depend on valid notice, grossed up for overhead and profit. |
| **Irrecoverable cost** | The share of a formal dispute's cost that cannot be recovered whatever the outcome; the figure that usually decides whether fighting can pay. |
| **Risk-tiered diligence** | Concentrating supply-chain assurance effort where breach probability is highest, rather than screening uniformly. |
| **Qualified alternate** | A second supplier approved and kept exercisable, receiving no volume; the structure that actually buys resilience. |
| **Sub-tier concentration** | A shared dependency below the first tier that correlates suppliers' failures and destroys a dual-sourcing case. |
| **Expected cost of disruption** | Σ P(state) × consequence(state); compared against the certain cost of the structure that reduces it. |

### Sample MCQs — KA 10.4

**MCQ 10.4-A `[10.4.1 · Application]`** Disruption is claimed on 1,204 planned hours whose productivity
fell to 0.86 of the measured-mile baseline, at USD 130.625 per hour. The disruption cost is:
- A. USD 22,018.15
- B. USD 25,602.50 ✅
- C. USD 29,770.35
- D. USD 157,272.50

*Rationale:* Hours required `= 1,204/0.86 = 1,400`, so extra hours `= 196` and cost
`= 196 × 130.625 = 25,602.50` (10.4.1). A multiplies the planned hours by `(1 − 0.86)` instead of
dividing by 0.86 — 168.56 hours rather than 196, understating by USD 3,584.35. C applies the correct
division to 1,400 hours instead of 1,204 (227.91 extra hours). D prices all 1,204 hours rather than the
extra ones.

**MCQ 10.4-B `[10.4.1 · Analysis]`** A USD 107,914.80 claim comprises labour and materials of 49,750
and prolongation and disruption of 46,602.50, each grossed up by 12 % overhead and profit. Notice was
given on day 41 against a 28-day requirement, and the prolongation and disruption heads depend on
notice. The amount at risk is:
- A. USD 46,602.50
- B. USD 52,194.80 ✅
- C. USD 55,720.00
- D. USD 107,914.80

*Rationale:* `46,602.50 × 1.12 = 52,194.80`, 48.37 % of the claim (10.4.1). A omits the overhead and
profit attaching to those heads; C is the surviving amount; D assumes the whole claim falls, which the
direct heads do not.

**MCQ 10.4-C `[10.4.2 · Evaluation]`** A 400,000 claim can be settled for 220,000 plus 15,000 of costs.
Arbitration has an expected award of 274,000 and irrecoverable costs of 340,000. The strongest argument
for settling is that:
- A. the expected award of 274,000 exceeds the 220,000 settlement
- B. arbitration takes 78 weeks
- C. the irrecoverable cost of 340,000 exceeds the entire negotiated settlement of 235,000, so arbitrating cannot pay even on a total win ✅
- D. settlements are always cheaper than awards

*Rationale:* A actually argues *against* settling and is the comparison usually made; it omits the cost
of obtaining the award. B is real but secondary. C is decisive: a total victory saves 235,000 and spends
340,000 (10.4.2). D is an unsupported generalisation.

**MCQ 10.4-D `[10.4.4 · Application]`** Single sourcing costs 846,400 with a 0.18 probability of a
900,000 disruption. A qualified alternate adds 65,000 of certain cost and cuts the consequence to
295,000. The breakeven disruption probability is:
- A. 7.22 %
- B. 10.74 % ✅
- C. 18.00 %
- D. 22.03 %

*Rationale:* `p* = 65,000/(900,000 − 295,000) = 65,000/605,000 = 10.74 %` (10.4.4). A divides by the
900,000 consequence rather than the reduction; C restates the assumed probability; D divides by 295,000.

**MCQ 10.4-E `[10.4.4 · Analysis]`** Two suppliers each have a 0.18 disruption probability, but both
draw a critical module from one sub-tier source, so the joint probability is 0.12 rather than 0.0324.
The effect on a dual-split sourcing case is that expected disruption cost:
- A. falls, because two suppliers are still better than one
- B. is unchanged, since the marginal probabilities are unchanged
- C. rises from 86,724 to 131,400, making the dual split worse than single sourcing ✅
- D. rises, but the dual split remains the best option

*Rationale:* `P(exactly one) = 2(0.18 − 0.12) = 0.12`, so expected cost is
`0.12 × 195,000 + 0.12 × 900,000 = 131,400`, taking the total to 1,071,400 against single sourcing's
1,008,400 (10.4.4). B mistakes marginals for the joint distribution, which is the error the whole
example exists to expose.

**MCQ 10.4-F `[10.4.3 · Evaluation]`** A 124,800 supply-chain diligence programme avoids an expected
106,400 of exposure. The correct professional conclusion is that:
- A. the programme should be cancelled, since it fails an expected-value test
- B. the programme should be risk-tiered to raise effectiveness where probability is highest, the breakevens (82.11 % effectiveness, 9.38 % probability) stated, and any legal duty met regardless of the arithmetic ✅
- C. the consequence figure should be raised until the business case works
- D. expected monetary value is the correct basis for this decision

*Rationale:* The arithmetic allocates effort; it does not decide whether a legal or values obligation
applies, and the consequence distribution is fat-tailed so its mean is the wrong statistic (10.4.3,
Domain 8 KA 8.2.2). C is the manipulation the honest presentation of breakevens is designed to prevent.

### Self-check — KA 10.4

1. *Name the four elements of a claim and the one on which most claims are decided.* — Entitlement,
   causation, quantum and notice; causation decides most of them.
2. *Which figure usually decides whether a formal dispute can pay, and why is it missed?* — The
   irrecoverable cost, because the comparison usually made is between the settlement and the expected
   award, which omits the cost of obtaining it.
3. *Why is dual-splitting an order a weaker resilience purchase than dual qualification?* — It pays a
   price premium and, under independence, doubles the exposure surface, while the option value comes
   from having an exercisable alternate, which dual qualification buys without either cost.

---

## Advanced topics — Domain 10

### 10.A.1 Supply-network mapping and the visibility that stops at tier one

Almost every organisation's supplier data stops at the parties it pays. Almost every serious supply
disruption originates below that line. The consequence is a systematic error in the direction of
optimism: exposure is assessed on the first tier, where it is diversified, and materialises in the
lower tiers, where it is concentrated.

Mapping is therefore a deliberate exercise with a defined scope, and the scope should be set by
criticality rather than by spend — the cheapest component on a critical path can stop a project that a
major package cannot. For each critical item, the questions are: which sub-tier sources does each of my
suppliers depend on; do any of my suppliers share one; what is the single-source geography,
qualification or intellectual-property constraint that makes substitution slow; and what is the
**re-source interval** — the elapsed time from a failure to an alternative in production, which is the
figure that converts a supply risk into a schedule risk at the project's cost of delay. The
contractual enabler is a disclosure obligation with an audit right (10.4.3), because a supplier will
not volunteer a dependency that is also a commercial vulnerability.

Two structural insights are worth carrying. Domain 4's interface arithmetic applies to supply networks:
`n` suppliers who must interoperate create up to `n(n−1)/2` pairwise relationships, and a buyer who
splits packages to gain price tension acquires all of them (Domain 4, KA 4.2.3). And Domain 8's
correlation result governs the aggregate: a register with many supplier risks and few underlying
drivers is concentrated, however it is presented (Domain 8, KA 8.A.1). Restructuring a supplier risk
register **by shared driver** rather than by supplier is a half-day exercise that regularly reveals a
single point of failure behind a dozen entries — and, as 10.4.4 computes, a single shared sub-tier is
enough to make a paid-for dual-sourcing arrangement worse than the single source it replaced.

### 10.A.2 Contracting for AI-delivered and outcome-based services

Where a supplier's deliverable is produced with, or consists of, an AI-enabled service, several
contractual assumptions that normally go unexamined stop holding, and the gaps must be closed
deliberately rather than left to a standard schedule.

**The deliverable can change without a variation.** A model updated by the supplier can alter output
quality, latency and behaviour while the contract's description of the service remains satisfied. Insist
on a defined baseline of behaviour with measurable acceptance criteria, notice of material model or
version changes, and a right to re-test against the baseline after one — otherwise the buyer has bought a
moving target and has no mechanism to notice it moving.

**Data rights need four separate answers, not one.** Who owns the input data; what the supplier may do
with it, specifically including whether it may train models serving other customers; what happens to
derived data, embeddings and model improvements at termination; and which sub-processors and jurisdictions
are involved. "Confidentiality" answers none of these.

**Performance obligations must be honest about probabilistic output.** A service whose output is correct
with some probability cannot be warranted as correct, and a supplier accepting such a warranty has either
mispriced it or will resist it in practice. The workable structure specifies accuracy against a defined
test set, a human-verification obligation on the buyer's side for consequential uses, and an allocation of
responsibility for errors that reflects who was required to verify — the contractual expression of the
family principle that **AI proposes; the professional verifies, decides and remains accountable**. If the
contract does not say who the professional is, no one is.

**Outcome-based structures inherit the same problem commercially.** Paying for an outcome shifts the
burden onto a definition that must now bear weight: what is measured, who measures it, what happens when
the measure is disputed, and what proportion of the outcome the supplier actually controls. Where a
supplier controls only part of it, an outcome-based payment is a lottery with a margin attached, and it
will be priced as one.

### 10.A.3 The reviewer's procurement eye

Invariants to test on any procurement or contract arrangement, each cheap and each diagnostic.

Every make-or-buy decision states a **total cost of ownership including a non-zero exit cost** and a
**breakeven volume**, and where the capability is on the critical path, the stand-up delay is priced at
the cost of delay. The procurement chain for every long-lead item has a **computed total lead time**
including governance latency, and is a predecessor in the schedule rather than an assumption behind it.
The evaluation model — weights, sub-weights, normalisation formula, scoring scale, moderation and
non-compliance treatment — was **fixed and disclosed before the receipt deadline**, with a dated record,
and quality was scored before price was seen. Any quality premium has been tested against the expected
cost it avoids, and the ratio is stated. Every bid materially below its benchmark has a recorded
explanation, with the benchmark named. Every payment mechanism has its **marginal-dollar allocation**
written down, and every target-cost contract has its `PTA` computed and known to the delivery team
before mobilisation, not after. Every performance regime states whether its cap is set to deter or to
compensate, and shows the arithmetic for whichever it claims. Every contract's notice periods are in a
live **notice register** with computed deadlines. Every critical item has a mapped sub-tier and a stated
**re-source interval**. And the resilience claim for every dual-sourced item is tested for
**correlation**, because a resilience arrangement that has not been tested for a shared sub-tier is an
assertion, and 10.4.4 shows what the assertion costs when it is wrong.

---

## Industry variations — Domain 10

- **Public sector and regulated buyers.** Procurement route, timescales, publication, evaluation
  disclosure, standstill and challenge rights may be prescribed by law, and those rules — which differ
  by jurisdiction — override commercial preference. The practical effects are that the chain of 10.1.3 is
  longer and largely incompressible, that the model-fixing discipline of 10.2.3 is a legal requirement
  rather than good practice, and that the leader's remaining levers are the paper deadline, the
  delegation of award authority, and early market engagement before the formal process begins.
- **Construction and infrastructure.** Standard-form contracts dominate, with well-developed variation,
  extension-of-time, notice and dispute machinery, and interim determinative processes available in some
  jurisdictions. Notice discipline (10.4.1) carries more value here than anywhere else, retention and
  payment cycles drive cash (Domain 7, KA 7.4.4), and multi-party governance follows the contract
  structure rather than the organisation chart (Domain 3, KA 3.1.2).
- **Technology and software services.** Scope is defined late by nature, so fixed price on undefined
  scope is the characteristic error and the AI-service gaps of 10.A.2 are the characteristic gap. Exit
  cost is systematically understated because data extraction, re-implementation and licence
  discontinuity are discovered only at termination — which is exactly why the make-or-buy exit column
  matters most in this sector.
- **Energy, utilities and process industries.** Long-lead, engineered-to-order equipment with limited
  qualified sources; the re-source interval of 10.A.1 is measured in quarters, so 10.4.4's qualified
  alternate and buffer strategies dominate, and the procurement chain frequently starts before the
  design is complete. Auriga is this shape.
- **Pharmaceutical and life sciences.** Supplier qualification is a regulated activity: changing a
  supplier or a manufacturing site may require re-validation and regulatory notification, which makes
  the alternate expensive to keep exercisable and the switching consequence far larger than a
  commercial calculation suggests. Dual qualification must be planned at development, not at supply.
- **Consumer manufacturing and retail.** High volumes make the unit-price term dominant, so the
  make-or-buy breakeven of 10.1.2 is often comfortably passed — but the ethical-sourcing exposure of
  10.4.3 is at its most acute, sub-tiers are numerous and opaque, and reputational consequence
  distributions are the most heavily skewed of any sector.

---

## Case study — Domain 10: the weighting confirmed after the envelopes (utilities, Auriga)

**Situation.** Auriga's installation package attracted three compliant bids: Alpha USD 2,000,000, Beta
2,200,000, Gamma 2,480,000. The invitation stated a price/quality weighting of **70/30**. The panel,
under time pressure and conscious that the previous installation contractor had performed poorly,
opened the priced envelopes alongside the technical submissions, scored quality at 62, 78 and 92, and
then — recording it as a "confirmation" — applied a **40/60** weighting on the grounds that quality
mattered more on this scope than the invitation had implied. Gamma won with **87.46** against Alpha's
**77.20**, and the award recommendation went to the utility's procurement board.

**What happened.** Alpha requested a debrief, was told that quality had been weighted at 60 %, and
challenged the award. The utility's internal audit function reconstructed the evaluation and reached
three findings the panel had not anticipated. Under the **published** 70/30 model the same scores gave
Alpha **88.60**, Beta 87.04 and Gamma 84.05 — **Alpha won**. The bid set had **three** possible winners
across the weighting range, with boundaries at **57.70 %** and **63.77 %** price weight, so the choice
of weighting had selected the supplier. And the panel's own risk mapping — integration rework
probabilities of 0.30, 0.18 and 0.10 against a 320,000 impact — made Gamma's **USD 480,000** premium
worth only `96,000 − 32,000 =` **USD 64,000** of avoided expected cost, a ratio of **7.5 to 1**, so the
quality case the re-weighting was meant to express did not survive its own numbers.

**How it resolved.** The award was withdrawn and re-made under the published model. Alpha was appointed
at 2,000,000 with two additions the audit finding made possible: a supplier-development condition
funding 90,000 of integration-team secondment against the identified rework risk, and a target-cost
structure with the `PTA` computed and shared with the delivery team before mobilisation (Domain 7,
KA 7.4.3). The process cost **11 weeks** — 3 weeks of probity review, 3 weeks of re-evaluation, 3 weeks
of re-approval latency at `E[wait] = 4/2 + 1`, and 2 weeks to execute — worth **USD 495,000** at
Auriga's cost of delay, plus **USD 62,000** of external review: **USD 557,000** in total.

**What the domain teaches here.** The arithmetic is brutal and simple: **the probity failure cost
USD 557,000 — more than the entire USD 480,000 price gap the re-weighting was arguing about, and
1.16 times it.** The panel was not corrupt and was not even wrong about quality mattering; it was wrong
about *when* that judgement may be made. Had the same conviction been expressed in the invitation as a
40/60 weighting, published before bids were received, the award to Gamma would have been unimpeachable —
and the bidders would have priced and pitched differently, which is the point of publishing it. Two
further lessons. Scoring quality **after** seeing price is not a procedural nicety: the panel's quality
scores were never tested for the anchoring the sequence introduced, and could not be defended in the
debrief. And the 7.5-to-1 ratio is the discipline a quality weighting needs before it is set, not after
it is challenged — a panel that had computed it during model design would have chosen 70/30 with
conviction rather than 40/60 with hindsight.

## Case study B — Domain 10: two suppliers, one foundry (rail rolling stock)

**Situation.** A rolling-stock refurbishment programme needed **1,200** traction converters. Wary of
single-source exposure after an earlier disruption, the sourcing team dual-sourced on a **55/45** split:
supplier V at **USD 14,800** per unit and supplier W at **USD 15,600**, a blended **USD 15,160** and a
contract value of **USD 18,192,000**. Awarding the full volume to V would have secured a **3 %** volume
discount — **USD 14,356** a unit, **USD 17,227,200** — so the programme paid **USD 964,800** for
resilience. The board minute recorded the arrangement as "dual-sourced, exposure mitigated". Each
supplier was assessed at a **0.15** probability of disruption in the delivery window, treated as
independent, giving an assumed expected disruption cost of `0.2550 × 294,000 + 0.0225 × 1,297,000 =`
**USD 104,152.50** — with a one-supplier disruption costing 2 weeks of fleet unavailability at
USD 128,000 a week plus a 38,000 surge premium (**294,000**), and both suppliers disrupted costing
9 weeks plus 145,000 of re-engineering (**1,297,000**).

**What happened.** Both suppliers slipped in the same quarter. Their converters used the same power
module, and that module had a single qualified foundry, which reallocated capacity. Neither supplier had
disclosed the dependency and neither had been asked. Reconstructed with the true joint probability of
**0.11**, the arrangement's expected disruption cost was `0.12 × 294,000 + 0.11 × 1,297,000 =`
**USD 166,190**, not 104,152.50 — the independence assumption had understated exposure by
**USD 62,037.50**, or **59.56 %** — and the realised outcome was the 9-week case.

**How it resolved.** A sub-tier mapping exercise to tier three found the single foundry behind both
suppliers and two further shared dependencies. The programme then did the arithmetic it had never done:
qualifying a **second power-module design** would cost **USD 380,000** — against the **USD 964,800**
already spent on a volume split that had bought nothing, a ratio of **2.54 to 1**. The arrangement was
restructured to a single award to V at the discounted 14,356, plus the 380,000 second module design,
plus a contractual sub-tier disclosure and audit obligation. With the module risk reduced (joint
probability 0.02, leaving V's idiosyncratic 0.04, so 0.06 in total) and the recovery shortened to
4 weeks plus 90,000, expected disruption fell to **USD 36,120** and the total position to
**USD 17,643,320** against the dual-split arrangement's **USD 18,358,190** — better by
**USD 714,870**, with the actual exposure reduced rather than relabelled.

**What the domain teaches here.** **Two suppliers with one sub-tier is one supplier with two invoices**,
and the money spent on the split is not resilience, it is a price premium with a resilience narrative
attached. Three transferable points. The independence assumption is the one most likely to be both
convenient and wrong, and it understated this programme's exposure by nearly 60 % — Domain 8's
correlation lesson (KA 8.A.1) arriving through a purchase order. Resilience spending must be directed at
the **actual single point of failure**, which requires knowing where it is: 380,000 spent on the module
would have delivered what 964,800 spent on the split did not. And a sub-tier disclosure obligation is
not administrative box-ticking — it is the contractual instrument that makes the analysis possible at
all, and it costs nothing to include at tender and cannot be obtained afterwards.

---

## Executive perspective — Domain 10

What a programme director cannot delegate in this domain:

- **The make-or-buy basis, and its exit column.** Insist on total cost of ownership with a non-zero
  exit cost and a stated breakeven volume, and require the stand-up delay to be priced at the cost of
  delay wherever the capability is on the critical path. A unit-price comparison is not a make-or-buy
  paper (10.1.2).
- **The evaluation model, fixed and dated before the receipt deadline.** Weights, sub-weights,
  normalisation and the sequence in which quality and price are seen. One bid set can have three
  winners; whoever chooses the model after opening has chosen the supplier, and Case study A shows the
  cost of learning that afterwards (10.2.2, 10.2.3).
- **The marginal-dollar allocation of every material contract, and where its `PTA` sits.** You should
  be able to say, without notes, who pays the next dollar of cost under each of your major contracts,
  and at what cost level that changes (10.3.2; Domain 7, KA 7.4.3).
- **Whether your performance regimes deter or merely price non-performance.** Compare every cap with
  the supplier's cost of compliance and with your own loss. A 5 % cap against a 180,000 compliance cost
  is a discount voucher for failure (10.3.3).
- **The notice register.** Your suppliers keep one, or their advisers do. Nearly half of a properly
  constructed claim can turn on a diary entry, in both directions (10.4.1).
- **The correlation test on every resilience claim.** Ask, of every dual-sourced critical item, which
  sub-tier sources the two suppliers share — and treat "we don't know" as the answer that it is. Two
  suppliers with one sub-tier is one supplier (10.4.4, Case study B).

---

## Calculation exercises — Domain 10

**Exercise 10.1** In-house provision of a support capability costs 260,000 to stand up, 1,450 per unit
and 40,000 to exit. Outsourcing costs 55,000 to transition in, 1,900 per unit and 85,000 to transition
out. The requirement is 520 units. Compute the breakeven volume and the better option; then recompute
with a 7-week capability stand-up delay on the critical path at a cost of delay of 18,000 per week.
*Solution.* `F_make = 300,000`, `F_buy = 140,000`, so
`Q* = 160,000/(1,900 − 1,450) = 160,000/450 =` **355.56 units**. At 520 units: make
`300,000 + 754,000 =` **1,054,000**; buy `140,000 + 988,000 =` **1,128,000** — **make is 74,000
cheaper**, because 520 exceeds the breakeven. Adding the delay: `7 × 18,000 =` **126,000**, so make
becomes **1,180,000** and **buy is now 52,000 cheaper**; the breakeven moves to
`(300,000 + 126,000 − 140,000)/450 =` **635.56 units**. Common error: omitting the exit costs from both
sides, which gives a breakeven of `205,000/450 =` **455.56 units** — it happens to reach the right
answer at 520 units, for the wrong reason, and would reach the wrong one at any volume between 356 and
456.

**Exercise 10.2** Two bids: P at 1,600,000 with quality 70, and Q at 1,840,000 with quality 88. Price
is scored `lowest ÷ own × 100`. Determine the winner at a 70/30 and a 50/50 price/quality weighting,
compute the crossover weighting, and state the implied price per quality point.
*Solution.* Price scores: P **100.00**, Q `1,600,000/1,840,000 × 100 =` **86.96**. At 70/30: P
`70 + 21 =` **91.00**, Q `60.87 + 26.4 =` **87.27** — **P wins**. At 50/50: P `50 + 35 =` **85.00**, Q
`43.48 + 44 =` **87.48** — **Q wins**. Crossover
`w* = (88 − 70)/[(100 − 70) − (86.956522 − 88)] = 18/31.043478 =` **57.98 %**. Q's premium is
`240,000` — **15.00 %** — for 18 quality points, **13,333.33 per point**, which must be tested against
the expected cost those points avoid. Common error: scoring price as a share of the *highest* bid or on
a linear scale without saying so; the normalisation convention is part of the model and moves the
crossover, as 10.2.2 shows by 7.70 percentage points on a three-bid set.

**Exercise 10.3** A target-cost subcontract has a target cost of 1,500,000, a target fee of 120,000, a
60/40 share (buyer 60 %) and a ceiling of 1,800,000. The actual cost is 1,700,000. Compute the fee,
buyer outturn and supplier margin, the `PTA`, and compare with a firm fixed price of 1,650,000 and with
cost plus a fixed fee of 120,000.
*Solution.* Overrun `1,700,000 − 1,500,000 = 200,000`; fee `120,000 − 200,000 × 0.40 =` **40,000**;
buyer pays **1,740,000** (below the ceiling); supplier margin **40,000**. Target price
`1,620,000`, so `PTA = 1,500,000 + (1,800,000 − 1,620,000)/0.60 = 1,500,000 + 300,000 =` **1,800,000**
— here the `PTA` coincides with the ceiling, and at a cost of 1,800,000 the fee is exactly nil. Firm
fixed price: buyer pays **1,650,000**, supplier margin **(50,000)**. Cost plus fixed fee: buyer pays
**1,820,000**, margin **120,000**. The buyer's outturn spans **170,000** across the three mechanisms on
the *same* actual cost, and the marginal-dollar allocation is 0/1.00 under fixed price, 1.00/0 under
cost-plus, and 0.60/0.40 under the target cost below the `PTA`. Common error: computing the fee as
`target fee − overrun × buyer share` (120,000 − 120,000 = 0), which reverses the share ratio; the
supplier bears its own share of the overrun, not the buyer's.

**Exercise 10.4** A single-source arrangement has a certain cost of 1,240,000 and a 0.22 probability of
a disruption costing 640,000. Qualifying an alternate adds 58,000 of certain cost and cuts the
consequence to 210,000. Compute both total expected costs, the breakeven disruption probability and the
maximum premium the resilience is worth. Then recompute assuming a shared sub-tier makes the alternate
unavailable in 0.14 of the 0.22.
*Solution.* Single: `1,240,000 + 0.22 × 640,000 =` **1,380,800**. Alternate:
`1,298,000 + 0.22 × 210,000 =` **1,344,200** — better by **36,600**. Breakeven
`p* = 58,000/(640,000 − 210,000) = 58,000/430,000 =` **13.49 %**. Maximum premium worth paying
`= 140,800 − 46,200 =` **94,600**. With the shared sub-tier: expected disruption
`(0.22 − 0.14) × 210,000 + 0.14 × 640,000 = 16,800 + 89,600 =` **106,400**, total **1,404,400** —
**worse than single sourcing**, and the correct response is to attack the sub-tier rather than to
choose between the two structures. Common error: computing the breakeven by dividing the extra certain
cost by the full consequence (58,000/640,000 = 9.06 %) rather than by the *reduction* in consequence,
which overstates the case for the alternate.

**Exercise 10.5** A variation claim comprises 320 hours of labour at 118.50 per hour, 12,600 of
materials, 2 weeks of prolongation at 16,500 per week, and disruption on 900 planned hours whose
productivity fell to 0.90 of the measured-mile baseline. Overhead and profit is 10 %. Notice was late,
and the prolongation and disruption heads depend on valid notice. Compute the total claim, the amount at
risk and the percentage lost.
*Solution.* Labour `320 × 118.50 =` **37,920**; materials **12,600**; prolongation
`2 × 16,500 =` **33,000**; disruption — hours required `900/0.90 = 1,000`, extra **100** hours, so
`100 × 118.50 =` **11,850**. Subtotal **95,370**; with 10 % overhead and profit, **total claim
104,907.00**. At risk: `(33,000 + 11,850) × 1.10 =` **49,335.00**, which is **47.03 %** of the claim;
surviving `(37,920 + 12,600) × 1.10 =` **55,572.00**. Common error: computing disruption as
`900 × 0.10 × 118.50 = 10,665`, which multiplies by the productivity shortfall instead of dividing by
the productivity factor and understates the head by 1,185 before overhead and profit.

---

## Practitioner's toolkit — Domain 10

*Adoption-ready artefacts; adapt the headings to your organisation, then keep them stable.*

### Toolkit 10.T.1 — Make-or-buy total-cost-of-ownership sheet

One page per decision, completed before any market engagement. Two columns, make and buy, and these
rows: transition-in cost, itemised (recruitment, training, tooling, environments, licences,
specification and tender effort, mobilisation); unit cost with the volume basis stated; residual
management cost — **never zero on either side**; **exit cost**, itemised (redeployment or severance,
decommissioning, data extraction in a supportable format, knowledge transfer, re-qualification, parallel
running); stand-up elapsed time in weeks and whether the capability is on the critical path; and the
delay term, priced at the project's cost of delay. Below the columns, three computed figures: the
**breakeven volume `Q*`**, the outturn at the required volume, and the outturn at the plausible upper
volume. The sheet's purpose is to make the decision a proposition about volume rather than a preference
about sourcing; a paper that cannot fill in the exit row has not done the analysis, and a paper with a
zero in it is asserting something the incumbent's contract will disprove.

### Toolkit 10.T.2 — Evaluation model lock sheet

One page, completed, dated, signed by the accountable authority and **issued with the invitation**.
Contents: the criteria and their weights; the sub-criteria and their weights; the **price normalisation
formula written out in full**; the quality scoring scale with what each score means in words; the
sequence of evaluation, stating explicitly that quality is scored before price is disclosed; the
moderation process and how individual scores are recorded before it; the treatment of non-compliant,
qualified and abnormally low bids, with the benchmark and threshold for the last of these named; the
panel members and their conflict declarations; and — the row that turns the sheet into a control — the
**pre-award sensitivity analysis**: the winner at three weightings and the crossover weightings between
the leading bidders, computed before the receipt deadline and dated. Any change after issue is recorded
on the sheet with its author, its reason and the result under **both** the original and the amended
model.

### Toolkit 10.T.3 — Supplier resilience and sub-tier register

One row per critical item, not per supplier, and criticality set by schedule effect rather than by
spend. Columns: item; why it is critical (which path, which milestone); first-tier supplier(s) and
volume allocation; **disclosed sub-tier sources for the critical component**, to the depth the
disclosure obligation reaches; **shared dependencies flagged across rows** — this is the column the
register exists for; qualification status of any alternate (qualified / qualified and exercised in the
last 12 months / named only); **re-source interval** in weeks; consequence of disruption, priced at the
cost of delay plus switching cost; assessed disruption probability with its source; expected cost of
disruption; the annual cost of keeping the alternate exercisable; and the **breakeven disruption
probability** at which that cost is justified. Reviewed quarterly, with two standing questions: has any
shared dependency appeared since the last review, and has any alternate lapsed from *exercisable* to
*named only*?

---

## Exam preparation — Domain 10

**What is assessed.** The procurement lifecycle and what each stage forecloses; make-or-buy on total cost
of ownership and its breakeven volume; the procurement chain as a schedule constraint including governance
latency; routes to market; the evaluation model, price normalisation, the crossover weighting and the
obligation to fix the model before opening; abnormally low tender testing against a named benchmark;
contract strategy as risk allocation and the influence test; buyer and supplier outturn under
fixed-price, cost-plus and target-cost mechanisms; the marginal-dollar allocation identity and the `PTA`
as a behavioural boundary; why an incentive reallocates value unless behaviour changes; performance-regime
caps for deterrence against compensation; the four elements of a claim, the heads of cost and notice-bar
exposure; the dispute ladder and irrecoverable cost; risk-tiered ethical sourcing and the honest limits of
its arithmetic; and single, dual-split and dual-qualified sourcing with correlation.

**The calculations to be able to do under time pressure.** `TCO = F + vQ` and
`Q* = (F_make − F_buy)/(v_buy − v_make)`, with and without a delay term. A procurement chain's total
lead time including `E[wait] = M/2 + L` per approval. Price normalisation on both conventions,
`S_i(w) = wP_i + (1 − w)Q_i`, and the crossover `w*`. Target-cost fee, buyer outturn and supplier margin
at a stated actual cost, plus the `PTA`. Expected cost, risk-neutral fixed price and expected supplier
margin from a cost distribution. A service-credit cap against a compliance cost and against a buyer
loss. Claim heads including measured-mile disruption, overhead and profit, and notice-bar exposure.
Expected cost of disruption under independence and under a stated joint probability, with the breakeven
probability `p*` and the maximum resilience premium.

**The traps.** Deciding make-or-buy on unit price, which systematically favours the high-fixed-cost
option (10.1.2) · scoring exit cost at zero (Toolkit 10.T.1) · dividing the fixed-cost difference by a
unit cost rather than by the unit-cost *difference* (MCQ 10.1-A) · omitting governance latency from a
procurement chain (Exercise implied by 10.1.3; Domain 3 KA 3.2.3) · treating the evaluation weighting as
a detail rather than as the thing that selects the winner (10.2.2, Case study A) · failing to state the
normalisation convention, which moves a crossover by up to 7.70 percentage points (10.2.2) · testing an
abnormally low bid against an unnamed benchmark (10.2.2) · adding the required margin to the *target*
cost instead of the *expected* cost when pricing a fixed-price bid (MCQ 10.3-A) · computing an incentive
fee with the buyer's share instead of the supplier's (Exercise 10.3) · assuming sharing continues above
the `PTA` (MCQ 10.3-B; Domain 7 KA 7.4.3) · reading a buyer's expected saving under an incentive as
value created when it is value transferred (10.3.2) · comparing a service-credit cap with nothing at all
(10.3.3) · computing measured-mile disruption by multiplying by the productivity shortfall instead of
dividing by the productivity factor (Exercise 10.5) · comparing a settlement with the expected award and
omitting irrecoverable cost (10.4.2) · dividing an extra certain cost by the full consequence rather
than by the consequence *reduction* when computing a breakeven probability (Exercise 10.4) · and
assuming supplier independence, which understated Case study B's exposure by 59.56 %.

**How the domain connects.** Domain 3 supplies the governance latency that makes the procurement chain
31 weeks long and the escalation design that a dispute ladder rests on. Domain 4 supplies the interface
arithmetic that prices a packaging decision and the change control that must run across the contract
boundary. Domain 5's specification determines whether any contract mechanism can work — on undefined
scope they all converge on cost-plus with a dispute attached. Domain 6 supplies the critical path that
makes a stand-up delay or a re-source interval expensive, and the cost of delay of USD 45,000 a week at
which everything here is priced. Domain 7 supplies the contract-model taxonomy, the blended rate, the
cash-flow consequence of payment terms and the `PTA` — all cited here, none re-derived. Domain 8 supplies
`EMV`, the correlation result that Case study B demonstrates commercially, and the R1 and R3 risks these
sourcing and evaluation decisions act on. Domain 11 handles the negotiation behaviour that determines
where inside a bargaining range these arithmetic results actually land, and Domain 13 the contracting
problem for adaptive delivery, where scope is deliberately not fixed. PFL-AI Domain 11 treats the
risk-allocation face of the same problem from the lender's side.

---

## Domain 10 summary
Procurement decisions are delivery decisions, and almost all of them are taken on the wrong number.

**Make-or-buy** is a proposition about volume, not about unit price. Auriga's remote-terminal-unit
capability looks 33.33 % cheaper to build per unit and is USD 88,800 more expensive to own at the 84
units required, because the unit-price advantage of 1,800 has to recover a fixed-cost disadvantage of
240,000 — a breakeven of **133.33 units**. Price the 9-week capability stand-up at the project's cost of
delay and the breakeven moves to **358.33 units**, which is not a marginal case but an unavailable one.
The exit column is where make-or-buy papers fail, and it is never zero. The procurement chain that
delivers the controllers runs to **31 weeks against a 25-week project**, of which 18 weeks are process
and 6 are pure governance latency worth **USD 270,000** — which is why long-lead items are committed
before the baseline and why the compressible time is administrative, not industrial.

**Tender evaluation** is arithmetic that selects a supplier. One bid set — Alpha 2,000,000/62, Beta
2,200,000/78, Gamma 2,480,000/92 — produces **three different winners**: Gamma below a 57.70 % price
weight, Beta between 57.70 % and 63.77 %, Alpha above. Changing only the normalisation convention moves
the Beta/Gamma boundary by **7.70 percentage points**. That is the whole case for fixing and publishing
the model, in full, before bids are opened, and for scoring quality before price is seen; Case study A
paid **USD 557,000** to learn it, against the **USD 480,000** price gap the re-weighting was arguing
about. The discipline that should precede any quality weighting is the premium test: Gamma's 480,000
premium bought **USD 64,000** of avoided expected rework, a ratio of **7.5 to 1**.

**Contract strategy** is the allocation of the marginal dollar, and the allocation always sums to one — a
contract cannot make cost disappear. On the same cost distribution (expected cost USD 2,120,000), a
risk-neutral firm fixed price of **USD 2,270,000** and cost plus a 150,000 fee cost the buyer *the same in
expectation*; what fixed price buys is the collapse of a **USD 230,434** standard deviation to zero, and
it is only as good as the supplier's balance sheet — at the 2,600,000 outcome that supplier loses 330,000.
The target-cost structure as specified moves **USD 48,000** from supplier to buyer and creates nothing:
the supplier's expected margin is 102,000 against a 150,000 requirement, because the target fee is the fee
at the target and not the expected fee. Value appears only when behaviour changes — the underrun
probability must rise from 0.20 to **0.456** for the supplier to accept the structure, at which point
expected cost falls by **USD 134,400**, all of it accruing to the buyer, which is what the negotiation is
really about. Above Auriga's `PTA` of **USD 2,428,571.43** (Domain 7, KA 7.4.3) the supplier's marginal
share jumps from 0.30 to 1.00 and cooperation turns to entitlement for structural reasons. And a
performance regime capped at 5 % of a 2,200,000 contract against a 180,000 compliance cost prices failure
at a **USD 70,000 discount**: deterrence needs **8.18 %**, compensation **14.55 %**, and confusing the two
is the design error.

**Claims, disputes and resilience** are where records earn their keep. A properly built USD 107,914.80
variation claim had **48.37 %** of its value — USD 52,194.80 — exposed to a notice provision, which is why
a notice register is worth more than any negotiating skill, in both directions. In a 400,000 dispute
settling at 235,000 against an expected arbitration cost of 614,000, the decisive figure is that the
**USD 340,000 of irrecoverable cost exceeds the entire settlement**, so fighting cannot pay even on a
total win. An ethical-sourcing programme costing 124,800 against 106,400 of avoided expected exposure
fails an expected-value test at 70 % effectiveness — and the honest response is to risk-tier it, state the
breakevens (**82.11 %** effectiveness, **9.38 %** probability), recognise the mean of a fat-tailed
consequence as the wrong statistic, and meet any legal duty regardless. Resilience, finally, is bought in
the wrong form almost universally: for Auriga's 84 controllers, single sourcing costs **1,008,400** in
expectation, a 60/40 dual split **1,026,724**, and a **qualified alternate 964,500** — so qualify two,
award one, keep the second exercisable, at a breakeven disruption probability of **10.74 %** against the
split's **24.22 %**. Then test the assumption that matters: introduce a shared sub-tier at a joint
probability of 0.12 and both resilience options become **worse than single sourcing** — which Case study B
lived, at a cost of 964,800 spent on a split that bought nothing while 380,000 spent on the actual single
point of failure would have.

The through-line: **every procurement decision has a number that decides it, the number is usually not
the one on the front of the paper, and the breakeven is more useful than the point estimate.** Compute
the breakeven volume, the crossover weighting, the marginal dollar, the deterrence cap and the breakeven
disruption probability, and a procurement conversation stops being about preference and becomes about
evidence.
