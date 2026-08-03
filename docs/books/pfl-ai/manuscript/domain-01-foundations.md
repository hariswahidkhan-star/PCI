# Domain 1 — Foundations of Project Finance Leadership

## Why this domain exists

Project finance is a distinctive answer to a distinctive problem: how to fund a large,
long-lived, single-purpose asset whose only security is *itself* — its contracts and the cash
they will produce. This domain establishes what makes that answer work, and what the leader at
the centre of it actually does. It maps the role across the project lifecycle (KA 1.1), builds
the discipline's three-cornered logic — value, cash and risk (KA 1.2) — and grounds the
profession's obligations: fiduciary awareness, independence, and the governed use of AI
(KA 1.3). Everything later in the book is a specialisation of this domain: the mathematics
(Domains 3–4), the structures (Domains 5, 9, 12), the lender's machinery (Domains 10, 13–15)
and the AI curriculum (Domain 16) all stand on the concepts fixed here. A reader who finishes
only this domain should already reason like the profession: *follow the cash, price the risk,
know who bears it, and stay accountable.*

**Learning objectives.** After this domain a candidate can: describe the project finance
leader's role at each lifecycle stage; place a financing on the recourse spectrum and explain
what limited recourse buys and costs; **price** the recourse decision — computing the
incremental cost of the limited-recourse route and the breakeven probability of a
parent-impairing failure at which it repays that cost — and explain why the breakeven falls
with facility size and why it is nonetheless the wrong sole test; explain the SPV's purpose,
the interests of each party around it, and the five ways a ring-fence leaks; describe the
infrastructure-finance market's asset classes and investors, and compute the Macaulay duration
of a project's cash stream to distinguish matching an asset's *life* from matching its
*duration*; explain why cash, not profit, is the binding constraint, demonstrate the
difference, and translate covenant headroom into days of receivables; derive the levered-return
identity, locate the crossover at which leverage stops helping, and show how far the equity
cliff moves when the same debt amortises rather than paying interest only; price a retained
risk through the capital structure the lenders will impose; price sponsor support as a
contingent claim; state the risk-return-bankability logic; and apply the profession's ethical
and responsible-AI obligations to realistic situations — including in explicit expected-value
terms, while explaining why the duty does not depend on the arithmetic.

**The master thread.** Kestrel Water SPC — whose loan, availability stream and investment case
Domains 3 and 4 priced — began here: a sponsor group weighing *how* to finance a desalination
plant at all. The figures this domain works with are the ones the rest of the book inherits:
capital cost **USD 60,000,000**, funded **70/30** as **USD 42,000,000** of senior debt at
**6.0 % over 12 years** — annual instalment **USD 5,009,635.23**, of which year-one interest is
**2,520,000** and principal **2,489,635** (Domain 3) — plus **USD 18,000,000** of sponsor
equity; a 25-year asset life; first-year documented `CFADS` of **USD 6,384,000** on revenue of
**12,000,000** (Domain 2), giving `DSCR` **1.2743** (Domain 10); and an appraisal at 8.0 % of
**NPV +16,179,360** on a 15-year operating stream of 8,900,000 (Domain 4). This domain tells the
part of the story that precedes all of it — the choice of financing route — and Domain 5 takes
the project through development to bankability.

---

## Knowledge Area 1.1 — The project finance leader and the financing landscape

*Topics: 1.1.1 the role across the lifecycle · 1.1.2 corporate versus project finance — the
recourse spectrum · 1.1.3 the SPV and its stakeholders · 1.1.4 the infrastructure-finance
market.*

### 1.1.1 The role across the lifecycle

The project finance leader is the person accountable for a project's **financial integrity
end to end** — not the deal-closer alone, and not the accountant alone. The role changes
costume by stage while keeping one spine:

| Stage | What the finance leader owns |
|---|---|
| Development | Screening economics (Domain 4); funding development spend at risk; shaping a *financeable* concept (Domain 5) |
| Structuring | Capital structure and funding sources (Domain 9); risk allocation into contracts (Domains 11–12) |
| Execution (financial close) | Due diligence, model audit, documentation, conditions precedent (Domain 13) |
| Construction | Drawdowns, cost-to-complete, lender reporting (Domain 14) |
| Operations | Covenant compliance, waterfall management, distributions, refinancing (Domain 15) |
| Maturity/exit | Handback, sale, restructuring where needed (Domain 15) |

The spine is a single question asked at every stage: **will the cash arrive, and who is
exposed if it does not?** The leader's authority rests on being the person in the room who can
answer it with evidence.

### 1.1.2 Corporate versus project finance — the recourse spectrum

**Definitions.** In **corporate (balance-sheet) finance**, lenders lend to a company and are
repaid from its whole cash flow; every asset stands behind every debt. In **project finance**,
lenders lend to a ring-fenced project and are repaid **only from that project's cash flows**,
with security over its assets and contracts — **non-recourse** to the sponsors, or **limited
recourse** where sponsors give bounded support (a completion guarantee, a cost-overrun
facility). Real deals sit on a spectrum between the poles.

What limited recourse *buys* sponsors: risk containment (a failed project cannot sink the
parent), balance-sheet capacity, the ability to share a mega-project among partners, and
discipline — lenders' due diligence becomes a second pair of eyes on every assumption. What it
*costs*: higher margins and fees (lenders carry risk they cannot chase a parent for), heavy
transaction and diligence costs, long documentation timelines, and covenant control over the
project's cash (Domain 10). The break-even is scale and risk-shape: single-asset,
contract-backed, capital-intensive projects with long lives are where the machinery pays.

> **Fig 1.1.1 — The recourse spectrum.** Horizontal spectrum diagram. Left pole: "Corporate /
> full recourse — lender looks to the whole balance sheet"; right pole: "Non-recourse — lender
> looks only to project cash flows and security". Between them, marked positions: guaranteed
> project loan · limited recourse (completion support) · non-recourse with reserves. Beneath
> each position, two mini-bars: sponsor risk retained (shrinking left to right) and financing
> cost/complexity (growing left to right). Source: PCI original. Alt text: a spectrum from
> full-recourse corporate lending to non-recourse project lending, showing sponsor risk
> falling and financing cost rising toward the non-recourse pole.

**Pricing the choice.** "Scale and risk-shape" is the right answer and an unusable one until it
is arithmetic. The limited-recourse route costs more in two distinguishable ways — a **margin
and fee differential** that scales with the facility, and a **close-cost premium** that is
largely *fixed* — and it buys one thing that can be valued: the parent's exposure to a failure
it did not cause and cannot survive. Setting the cost against the exposure gives a breakeven
probability, and the shape of that breakeven against deal size is the whole of the scale
argument.

```
Incremental cost of the limited-recourse route
  = PV(project-finance debt service − corporate debt service) + (close costs_PF − close costs_corporate)

Breakeven failure probability  p* = incremental cost ÷ exposure the ring-fence removes
```

**Worked example 1.1.2 — Kestrel's recourse decision, priced.**

1. **Setup.** Kestrel needs **USD 42,000,000** of debt inside a **60,000,000** envelope. Two
   routes. *Project route:* the SPV borrows at **6.0 % over 12 years** (the master facility) and
   the close-cost budget is Domain 13's itemised **USD 2,709,000** (KA 13.3.4). *Corporate
   route:* the international water operator borrows the same amount on its own balance sheet at
   **4.60 %** over the same 12 years — 140 basis points tighter, because the lender is looking at
   an established credit rather than at a plant that does not yet exist — with close costs of
   **350,000** (arrangement and legal only — none of the seven diligence streams, no model audit,
   no security perfection across a contract set). The downside being insured against is a
   post-completion performance failure in operating year three, at which point the sponsors'
   completion support has already fallen away: the offtake terminates and enforcement realises the
   single-purpose asset at **40 % of capital cost — USD 24,000,000**. Cash costs are compared at
   the appraisal rate of **8.0 %** (Domain 4).
2. **Formula.** As above. `AF(0.046, 12)` and `AF(0.06, 12)` size the two instalments;
   `AF(0.08, 12)` prices the annual differential; the exposure is the loan's year-three closing
   balance less the enforcement recovery.
3. **Substitution.** `AF(0.06, 12) = 8.383844` → `42,000,000/8.383844 = 5,009,635.23`.
   `AF(0.046, 12) = 9.066641` → `42,000,000/9.066641 = 4,632,366.30`. Differential
   `377,268.94` a year; `AF(0.08, 12) = 7.536078` → PV `2,843,128.16`. Close-cost premium
   `2,709,000 − 350,000 = 2,359,000`. Year-three closing balance from Domain 3's schedule:
   `34,073,997.27`; exposure `34,073,997.27 − 24,000,000`.
4. **Result.**

   | | Project route | Corporate route | Differential |
   |---|---|---|---|
   | Annual debt service | 5,009,635.23 | 4,632,366.30 | **377,268.94** |
   | PV of debt service at 8 % | 37,753,001.96 | 34,909,873.80 | **2,843,128.16** |
   | Close costs at financial close | 2,709,000 | 350,000 | **2,359,000** |
   | **Incremental cost of limited recourse** | | | **USD 5,202,128.16** |
   | Exposure removed (year-three balance 34,073,997.27 less 24,000,000 recovery) | | | **USD 10,073,997.27** |
   | **Breakeven failure probability `p*`** | | | **51.6392 %** |

   The incremental cost is **12.3860 %** of the debt raised, of which the fixed close-cost
   premium is **45.3468 %**.
5. **Interpretation.** Read the headline first and then refuse to stop there. **On expected value
   alone, the limited-recourse route does not pay:** it requires the sponsors to believe there is
   better than a **one-in-two** chance of a failure severe enough to leave 10,073,997 of debt
   unrecovered, and no board that has just approved a 60,000,000 plant believes anything of the
   kind. That is not an argument against project finance; it is the discovery that **project
   finance is not bought with expected-value arithmetic**. Three reasons the 51.64 % is the wrong
   sole test.
   *First, it prices a mean and the sponsor is insuring a tail.* The loss that matters is not the
   10,073,997 itself but its correlation with everything else — a failed project consolidates
   onto the parent's balance sheet in the same quarter that its own lenders are re-testing
   covenants and its own rating is under review, so the realised cost of the corporate route in
   the bad state is far above the enforcement shortfall. Expected value averages exactly that
   asymmetry away; Domain 13 (KA 13.1.3) makes the same caution about the right tail of a
   diligence loss. *Second, it prices only one of the three things the structure buys.* The
   regional infrastructure fund cannot guarantee anything beyond its equity, so without the
   ring-fence there is no partnership at all and the operator funds 18,000,000 of equity it does
   not have; that option has a value the table does not contain. *Third, the corporate route's
   4.60 % is not a fact but a facility* — it consumes the operator's own borrowing capacity, and
   the next project priced at the margin is the one that pays for it. Now the **scale** result,
   which the breakeven does capture cleanly and which Fig 1.1.3 plots in full. Because the
   close-cost premium is essentially fixed while the exposure scales with the facility, `p*` falls hyperbolically with size: **93.7893 %
   at 15,000,000** of debt, **51.6392 %** at Kestrel's 42,000,000, **34.7791 % at 150,000,000**,
   approaching **28.2224 %** — the margin differential alone — as the fixed premium becomes
   immaterial. Below about **USD 13,702,087** of debt, `p*` exceeds 100 %: the route cannot pay at
   *any* failure probability, because the close-cost premium alone exceeds the whole exposure it
   removes. That is the same reading Domain 13 gives an impossible breakeven detection rate
   (KA 13.1.3) — an impossible configuration, not a worthless idea — and it is the arithmetic
   reason the machinery is not reached for on small facilities, whatever their risk shape. Two cautions for the
   reviewer. The 140 basis points and the 40 % recovery are the two parameters that move the
   answer most and both are estimates: raise the enforcement recovery from 40 % to **50 %** of
   capital cost and the exposure collapses to **4,073,997.27** while `p*` rises to
   **127.6910 %** — impossible — so a route decision resting on this number alone is resting on
   the recovery assumption. And the fixed premium is only *approximately* fixed — diligence scope
   does grow with project complexity — which flattens the curve without changing its direction.

### 1.1.3 The SPV and its stakeholders

**Definition.** The **special-purpose vehicle (SPV)** is a company created to do exactly one
thing — own, build, finance and operate the project — and legally *incapable* of doing
anything else. Ring-fencing is what makes non-recourse lending possible: the SPV's contracts
are its assets, and every major relationship is written down (Domain 12 builds the contract
matrix in full).

The parties and what each optimises:

| Party | Wants | Watches |
|---|---|---|
| **Sponsors** (equity) | Return on equity; contained risk; distributions | Equity IRR, distribution tests |
| **Lenders** | Repayment with margin; downside protection | DSCR/LLCR, covenants, security (Domain 10) |
| **Offtaker / grantor** | Reliable service at agreed price | Availability, tariffs, handback condition |
| **EPC contractor** | Construction margin | Variations, delay LDs (Domain 12) |
| **Operator (O&M)** | Fee; performance regime it can meet | Availability/output guarantees |
| **Government / regulator** | Delivery of policy outcomes; compliance | Permits, obligations, public interest |
| **Community & environment** | Benefit without harm | E&S performance (Domain 11) |

The finance leader's daily craft is reconciling these optimisations *inside one cash flow* —
which is why the cash waterfall (Domain 15) reads like a peace treaty.

> **Fig 1.1.2 — The SPV at the centre of its contracts.** Hub-and-spoke diagram. Centre node:
> "Project SPV". Spokes to: Sponsors (equity subscription, shareholder agreement) · Lenders
> (facility agreement, security) · Offtaker (offtake/concession agreement) · EPC contractor
> (turnkey construction contract) · O&M contractor (operating agreement) · Government
> (permits, direct agreement). Each spoke labelled with the money/service flowing each way.
> Source: PCI original. Alt text: a central project company connected by labelled contract
> spokes to sponsors, lenders, offtaker, contractors and government.

### 1.1.4 The infrastructure-finance market

The asset classes and their financing habits, in one professional sweep: **transport** (toll
roads, rail, airports — patronage or availability models); **power and renewables** (PPAs,
capacity markets; the energy transition's build-out is the market's largest engine);
**water** (desalination and treatment concessions — Kestrel's world); **digital
infrastructure** (data centres, towers, fibre — shorter refresh cycles, credit-tenant
leases); **social infrastructure** (hospitals, schools, housing under availability PPPs); and
**natural resources** (commodity-linked, price-hedged). The capital comes from commercial
banks (construction-phase specialists), institutional investors and infrastructure funds
(long-dated operations-phase capital), export credit agencies and development banks
(Domain 9), and bond markets (refinancing bankable operating assets). The leader's market
literacy is matching **asset shape to capital shape**: construction risk to banks and ECAs,
stabilised cash flows to institutions — mismatches are expensive at best and fatal at close.

**Two axes, routinely conflated.** "Asset shape" is doing two jobs in that sentence, and
separating them is what makes the principle operational. The first axis is **risk-holding
capability** — which investor can price, absorb and be paid for construction risk, demand risk
or technology risk. That axis dominates, and it is the axis MCQ 1.1-C tests. The second is
**timing**, and here the profession's own shorthand misleads: capital described as "long-dated"
is being matched to the *horizon over which cash arrives*, not to the stream's **Macaulay
duration** — the present-value-weighted average time to receipt, which is the measure of how far
a value moves when a rate moves. The two are very different numbers on the same asset, and an
investor who matches the wrong one will systematically buy the wrong asset.

```
Macaulay duration  D = Σ [ t × CFₜ / (1 + r)ᵗ ] ÷ Σ [ CFₜ / (1 + r)ᵗ ]     (years)
For a level stream of n payments:  D = (1 + r)/r − n ÷ [ (1 + r)ⁿ − 1 ]
Limit as n → ∞:                    D → (1 + r)/r
```

**Worked example 1.1.4 — a fifteen-year asset with a six-year duration.**

1. **Setup.** An institutional investor with liabilities of average duration **14 years** is
   offered three positions on the same desalination economics, all discounted at **8.0 %**:
   (a) Kestrel operating — the level availability stream of **8,900,000 a year for 15 years**
   (Domain 4's appraisal stream); (b) Kestrel greenfield — the identical stream, but only from
   year 4, after a three-year construction period; (c) Kestrel indexed — the same 15-year stream
   starting at 8,900,000 and escalating at **2.5 %** a year (Domain 3's Fisher discipline governs
   the world it is quoted in). Which matches the liabilities?
2. **Formula.** As above. For (c) the closed form does not apply, so the summation is used
   directly with `CFₜ = 8,900,000 × 1.025^(t−1)`.
3. **Substitution.** (a) `D = 1.08/0.08 − 15/(1.08¹⁵ − 1) = 13.5 − 15/2.172169`.
   (b) The same weights, every receipt three years later, so `D = 6.594460 + 3`.
   (c) `Σ t × CFₜ/1.08ᵗ ÷ Σ CFₜ/1.08ᵗ` over 15 terms.
4. **Result.**

   | Position | Present value | Macaulay duration | Gap to a 14-year liability |
   |---|---|---|---|
   | (a) Operating, 15 years level | 76,179,360.32 | **6.5945 years** | (7.4055) |
   | (b) Greenfield, 3-year deferral | 60,473,632.32 | **9.5945 years** | (4.4055) |
   | (c) Operating, 15 years indexed at 2.5 % | 87,937,828.16 | **7.0342 years** | (6.9658) |
   | Level stream over the full 25-year asset life | — | **9.2254 years** | (4.7746) |
   | Any level stream at 8 %, however long | — | **capped at 13.5000 years** | never closes |

5. **Interpretation.** The first result is the one that reorders a reader's intuitions: **a
   15-year asset has a duration of 6.59 years — 44 % of its life.** Level streams front-load
   present value: **54.01 %** of the operating stream's present value arrives in the first six of
   its fifteen years, and an investor who bought it to match a 14-year liability has bought something
   less than half as rate-sensitive as the liability it is hedging. The second result is stronger
   and it is an *identity*, not an estimate: because `D → (1 + r)/r`, **no level stream discounted
   at 8 % can ever have a duration above 13.5 years, at any tenor.** Extending the concession from
   15 years to 25 buys 2.63 years of duration; extending it to 63 years would be needed to reach
   13.0. A 14-year duration is therefore unreachable by tenor alone, and the reviewer's cue is
   immediate: any liability-matching claim that rests on lengthening a level stream is
   arithmetically confused. What *does* add duration is deferral (+3.00 years, exactly the
   deferral, because deferring every weight shifts the weighted mean) and **escalation**
   (+0.44 years at 2.5 %, because escalation moves weight to the later periods). That is the
   investor-side reason indexed availability payments and inflation-linked tariffs are prized by
   liability-driven capital — a fact usually explained on the revenue side (Domain 7) and rarely
   on the buyer's. Now the honest complication, and it is the professional point of the example.
   By duration alone, the **greenfield** position at 9.59 years is the *closest* match to the
   14-year liability — and that is emphatically not advice to a pension fund to buy construction
   risk. The two axes disagree, and the risk axis wins: a construction-phase position carries
   completion, cost-overrun and technology exposures the fund cannot price, cannot manage and is
   not paid for, and duration is a first-order sensitivity measure that says nothing whatever
   about the probability that the cash arrives at all. The correct professional conclusion is the
   layered one: **match risk first, then close the residual timing gap with instruments rather
   than with asset selection** — indexation in the revenue contract, and interest-rate or
   inflation hedging at the fund level. One caution on the arithmetic
   itself: duration is computed at a stated rate and moves with it, so a duration quoted without
   its discount rate is not a number — at 6 % the same 15-year level stream has a duration of
   **6.9260 years** and the ceiling rises to **17.6667**.

> **Fig 1.1.3 — The recourse decision against facility size.** Line chart. X-axis senior debt
> raised, 10,000,000 to 310,000,000; y-axis the breakeven probability of a parent-impairing
> failure, 20–110 %. A single blue hyperbola plots
> `p* = (2,359,000 + debt × 0.0676935) ÷ (debt × 0.2398571)`, with crimson markers at
> **15m → 93.7893 %**, **25m → 67.5625 %**, **42m → 51.6392 %** (ringed and labelled "Kestrel"),
> **75m → 41.3358 %**, **150m → 34.7791 %** and **300m → 31.5008 %**. A dashed crimson horizontal
> at 100 % is captioned "cannot pay at any probability", with a vertical dropping from its
> intersection at **13,702,087**. A dashed grey horizontal at **28.2224 %** is captioned
> "asymptote (margin differential alone)". A right-hand panel lists the plotted pairs and the two
> cost components: fixed close-cost premium 2,359,000; margin differential 140 bp, PV 6.7694 % of
> debt. Source: PCI original. Alt text: a falling curve showing that the failure probability
> needed to justify limited recourse is impossible on small facilities, roughly one in two at
> forty-two million, and settles near twenty-eight per cent on large ones.

### AI in this KA

Market screening and precedent research are natural AI accelerants — summarising comparable
transactions, extracting terms from public disclosures, drafting stakeholder maps. The
governed habits start in Domain 1 because the failure modes do: a fluent but invented
"precedent transaction" is this profession's textbook hallucination case. Sources are
verified against the registry discipline (every claimed deal traced to a public record), and
the stakeholder analysis an AI drafts is walked, party by party, by someone who has sat
across from those parties. **AI proposes; the professional verifies, decides and remains
accountable.** Worked example 1.3.3 prices exactly this failure on Kestrel's own facility, and
the number is larger than most readers expect.

**One verification habit specific to this KA.** The two computations above are the ones a machine
gets *plausibly* wrong rather than obviously wrong. A recourse comparison assembled by a tool
will usually discount both routes correctly and still be worthless, because the exposure term —
the one number that is not in any term sheet — has to be constructed from an enforcement
assumption a human must own and state. And a duration figure is the classic silently-wrong
output: the formula is standard, so the answer arrives with no warning that it was computed at a
different rate, on a different stream, or on the asset's life rather than its cash flows. In both
cases the discipline is the same and it is cheap: **make the tool state the parameter, then
recompute one line by hand.**

### Key terms — KA 1.1

| Term | Meaning |
|---|---|
| **Recourse / non-recourse / limited recourse** | Whom the lender can pursue: the sponsor's balance sheet; the project only; the project plus bounded sponsor support. |
| **SPV** | Ring-fenced single-purpose project company; the borrower and contract hub. |
| **Sponsor** | Equity investor promoting the project (this book's project-finance sense). |
| **Offtaker** | The buyer of the project's output or service under contract. |
| **Ring-fencing** | Legal isolation of the project's assets, contracts and cash. |
| **Asset-capital matching** | Pairing project risk phases with the capital suited to hold them — on two axes, risk-holding capability and timing. |
| **Recourse sentence** | The three-limbed statement of sponsor support: who stands behind what, until when, capped at what. |
| **Close-cost premium** | The largely fixed excess of project-finance transaction costs over a corporate facility's; the source of the scale effect. |
| **Breakeven failure probability `p*`** | Incremental cost of the limited-recourse route ÷ the exposure the ring-fence removes. |
| **Macaulay duration** | Present-value-weighted average time to receipt; for a level stream capped at `(1 + r)/r`, whatever the tenor. |

### Sample MCQs — KA 1.1

**MCQ 1.1-A `[1.1.2 · Application]`** A sponsor gives lenders a guarantee that covers cost
overruns until the plant passes its completion test, after which lenders may look only to
project cash flows. This financing is best described as:
- A. full recourse
- B. non-recourse
- C. limited recourse ✅
- D. unsecured corporate lending

*Rationale:* Bounded sponsor support (here, to completion) between the poles is the defining
shape of limited recourse. A would expose the sponsor for the loan's life; B would mean no
sponsor support at all; D abandons both the security package and the ring-fence.

**MCQ 1.1-B `[1.1.3 · Analysis]`** Which single feature of the SPV makes non-recourse lending
possible?
- A. its tax registration
- B. legal ring-fencing: the SPV can conduct only the project, so its contracts and cash are isolated and chargeable ✅
- C. its sponsors' credit ratings
- D. the size of its share capital

*Rationale:* Lenders can accept project-only recourse because the ring-fence guarantees no
other business can dilute, encumber or divert the cash they are lending against. C reverses
the concept (sponsor credit is what non-recourse lending does *without*); A and D are
administrative facts, not the mechanism.

**MCQ 1.1-C `[1.1.4 · Application]`** A fund holding long-dated pension liabilities wants
infrastructure exposure. The asset-capital matching principle points it toward:
- A. construction-phase risk in a greenfield project
- B. stabilised operating assets with contracted cash flows ✅
- C. development-stage equity at risk
- D. short-term bridge lending

*Rationale:* Long-dated stable liabilities match long-dated stable cash flows on the axis that
dominates — risk-holding capability. A and C sit where construction and development specialists
(banks, ECAs, developers) hold the risk; D matches a treasury desk, not a pension profile. Note
that on the *timing* axis alone a deferred greenfield stream is the closer duration match
(WE 1.1.4) — which is why the risk axis must be settled first.

**MCQ 1.1-D `[1.1.2 · Application]`** The limited-recourse route costs 5,202,128 more in present
value than the corporate route and removes an enforcement exposure of 10,073,997. The breakeven
probability of a parent-impairing failure is:
- A. 12.39 %
- B. 23.42 %
- C. 51.64 % ✅
- D. 28.22 %

*Rationale:* `5,202,128/10,073,997 = 51.64 %` (WE 1.1.2). A divides the incremental cost by the
42,000,000 of debt instead of by the exposure — a cost intensity, not a breakeven; B uses only
the 2,359,000 close-cost premium and drops the 2,843,128 margin differential; D is the
large-facility asymptote, which omits the fixed close-cost premium altogether and therefore
applies to no actual facility.

**MCQ 1.1-E `[1.1.4 · Analysis]`** A level availability stream pays 8,900,000 a year for
15 years, discounted at 8.0 %. Its Macaulay duration is closest to:
- A. 6.59 years ✅
- B. 8.00 years
- C. 13.50 years
- D. 15.00 years

*Rationale:* `D = 1.08/0.08 − 15/(1.08¹⁵ − 1) = 6.5945` (WE 1.1.4). B is the *unweighted* mean of
the payment dates 1 to 15 — the duration you get by forgetting to discount the weights; C is the
`(1 + r)/r` ceiling, which a level stream approaches only as the tenor approaches infinity; D
confuses the asset's life with its duration, the error the example exists to disarm.

**MCQ 1.1-F `[1.1.2 · Evaluation]`** A sponsor's board is shown a breakeven failure probability of
51.64 % and concludes that limited recourse "fails its own test" and should be abandoned. The
best professional response is:
- A. agree — the arithmetic is decisive
- B. the expected-value test prices a mean while the sponsor is insuring a correlated tail, and it values none of the partnership or balance-sheet capacity the structure delivers; the breakeven is one input, not the decision ✅
- C. recompute at a higher discount rate until the answer changes
- D. the calculation is invalid because probabilities of project failure cannot be estimated

*Rationale:* WE 1.1.2's own interpretation: the exposure term is an expected shortfall, not the
correlated loss that would arise in the bad state, and the structure additionally buys an option
(the fund's participation) the table does not contain. A treats one input as the decision; C is
assumption-shopping, which is the misconduct 1.3.1 names; D overstates the objection — the
parameter is uncertain, which is an argument for stating it and testing it, not for discarding
the frame.

**MCQ 1.1-G `[1.1.4 · Analysis]`** An investment committee paper recommends a 25-year availability
concession to an insurer whose liabilities have an average duration of 14 years, on the ground that
"extending the tenor from 15 years to 25 brings the asset's duration into line with the liability".
The stream is level and the discount rate is 8.0 %. The reviewer should:
- A. accept the recommendation — a 25-year asset is the closest available match to a 14-year liability
- B. reject the reasoning as arithmetically impossible: at 8 % a level stream's duration is capped at `(1 + r)/r` = 13.5000 years, and 25 years reaches only 9.2254, so no tenor closes the gap ✅
- C. correct the reasoning but keep the recommendation — 9.2254 years is the longest duration the concession can offer, so tenor is still the right lever and the residual gap belongs to the liability side
- D. reject the recommendation because duration is not a meaningful measure for infrastructure assets

*Rationale:* extending 15 years to 25 buys **2.6309** years of duration (6.5945 → 9.2254) and the
ceiling is never reached at any finite tenor, so the paper's stated mechanism cannot deliver what it
claims (WE 1.1.4); duration is added by deferral (+3.0000 years), escalation (+0.4398 at 2.5 %) or a
lower rate (the ceiling is 17.6667 at 6 %), and the residual gap is closed with instruments rather
than asset selection. A confuses the asset's life with its duration, the error the example exists to
disarm. C is the strongest of the wrong answers and gets halfway: it drops the paper's mechanism and
then repeats its conclusion, when 9.2254 is *not* the longest duration available — deferring the same
15-year stream by three years reaches **9.5945**, which is why WE 1.1.4 ranks the greenfield position
ahead of the long concession. D discards a first-order measure the profession uses correctly; the
defect is the claim made with it, not the measure.

**MCQ 1.1-H `[1.1.2 · Comprehension]`** Kestrel's breakeven failure probability `p*` falls from
93.7893 % on a 15,000,000 facility to 51.6392 % at 42,000,000 and approaches 28.2224 % on very large
ones. Which statement restates the reason correctly?
- A. lenders charge a lower margin on larger facilities, so the incremental cost of the project route shrinks
- B. the close-cost premium is broadly fixed while the exposure the ring-fence removes scales with the facility, so a partly fixed cost divided by a proportional benefit falls toward the margin differential alone ✅
- C. larger projects fail more often, so the probability required to justify the structure is lower
- D. enforcement recoveries improve with project size, which enlarges the exposure the structure removes

*Rationale:* the cost has a fixed element (the **2,359,000** close-cost premium) and a proportional
one (140 basis points, worth **6.7694 %** of the debt in present value), while the exposure removed
is proportional (**23.9857 %** of debt on these assumptions) — which is exactly why the curve is a
hyperbola with 28.2224 % as its asymptote (1.1.2). A asserts a pricing pattern the example does not
contain and the arithmetic does not need. C confuses the probability *required* with the probability
*expected*. D reverses the direction: a better recovery reduces the exposure removed and therefore
*raises* `p*` — at a 50 % recovery it reaches 127.6910 %.

### Self-check — KA 1.1

1. *State the finance leader's one recurring question.* — Will the cash arrive, and who is
   exposed if it does not?
2. *Name two things limited recourse buys a sponsor and two things it costs.* — Buys: risk
   containment, balance-sheet capacity (also partnering, lender discipline). Costs: pricing
   and fees, transaction complexity (also covenant control, time).
3. *Why does every party around the SPV get a contract?* — Non-recourse credit is built from
   contracts: each relationship must be enforceable because the cash flow is the only
   security.
4. *Why does the recourse breakeven fall as the facility grows?* — The close-cost premium is
   largely fixed while the exposure removed scales with the facility, so `p*` falls
   hyperbolically toward the margin differential alone (28.2224 %).
5. *State the duration ceiling and what it rules out.* — A level stream's duration cannot exceed
   `(1 + r)/r` — 13.5 years at 8 % — so no amount of tenor can match a longer-duration liability;
   only deferral, escalation or a lower rate moves it.

---

## Knowledge Area 1.2 — Value, cash and risk: the discipline's logic

*Topics: 1.2.1 value creation in projects · 1.2.2 cash as the binding constraint · 1.2.3
leverage, risk and the bankability triangle.*

### 1.2.1 Value creation in projects

A project creates value when the present value of what it will produce exceeds what it costs
to build and run — Domain 4's NPV, stated in words. The foundation point is *where* value can
be created or destroyed by *financing*: structure does not conjure value from a bad project,
but it can (1) allocate each risk to the party who bears it cheapest — lowering the priced-in
premiums; (2) match capital to risk phase — lowering the blended cost of funds; and (3)
impose diligence and covenant discipline that keeps forecast value from leaking in execution.
The corollary the profession lives by: **financing engineering amplifies project quality; it
never substitutes for it.**

**Channel one, made computable.** "Allocate each risk to the party who bears it cheapest" is
usually taught as a comparison of two parties' expected costs, and Domain 11 (KA 11.2.2) builds
that register properly, item by item. What that comparison omits — and what makes the allocation
question a *financing* question rather than a risk-register question — is that a risk the SPV
retains is not paid for out of a contingency line. **It is paid for in the capital structure**,
because lenders respond to retained construction risk by de-gearing rather than by pricing, and
equity is the most expensive money in the structure. The consequence is that the sponsor's true
reservation price for transferring a risk is usually several times the risk's own expected cost,
and is invisible on any risk register.

**Worked example 1.2.1 — what retaining a risk actually costs.**

1. **Setup.** Kestrel's EPC contractor will accept a defined construction risk — say a scope
   interface the sponsors would otherwise carry — for a fixed **1,350,000** addition to the
   contract price. The sponsors' own estimate of the risk's expected cost is **900,000**, so the
   quote carries a **50 %** loading and the risk register says refuse. The lenders' credit
   committee, told the risk is retained, does not reprice: it reduces senior gearing from
   **70 %** to **62 %** of the 60,000,000 envelope. Cost of equity is Domain 9's derived
   **15.42 %** (KA 9.1.3); the senior rate is **6.0 %**; the horizon is the 12-year debt term and
   the comparison rate is the appraisal's **8.0 %**.
2. **Formula.** Extra equity = capex × (new equity share − old equity share). Annual cost of the
   substitution = extra equity × (`k_e` − `k_d`). Cost of retention = that annuity discounted;
   the **breakeven transfer price** is that present value.
3. **Substitution.** Equity `60,000,000 × 0.30 = 18,000,000` becomes `60,000,000 × 0.38 =
   22,800,000`, so **4,800,000** of debt is replaced by equity — 8.0000 % of the envelope. Annual
   `4,800,000 × (0.1542 − 0.0600) = 4,800,000 × 0.0942 = 452,160`. PV `452,160 × AF(0.08, 12) =
   452,160 × 7.536078`.
4. **Result.** Cost of retention **USD 3,407,513.04** in present value — **3.7861 times** the
   risk's own expected cost of 900,000. Against a transfer price of 1,350,000, transferring
   creates **USD 2,057,513.04** of value, and the sponsor should be willing to pay up to
   **3,407,513.04** — the breakeven — before retention becomes the cheaper answer.
5. **Interpretation.** The register said refuse and the register was wrong by **2,057,513**,
   because it compared the quote with the wrong number. This is the first channel of 1.2.1 in
   arithmetic: value was created not by reducing anyone's risk but by **moving it to where it is
   financed more cheaply**, and the gain is the spread between equity and debt — 942 basis points
   on 4,800,000 — not any difference in the parties' views of the hazard. Four things a reviewer
   should take from it. **The form of the lender's response matters more than the risk.** Had the
   committee priced the retention at, say, 35 basis points of margin on the full 42,000,000
   instead of de-gearing, the cost would have been `147,000` a year and **1,107,803.47** in present
   value — under a third as much — and the same 1,350,000 quote would then have been *rejected*
   correctly. The professional discipline is therefore to ask the credit committee *how* it will
   respond before negotiating the contract price, which is a conversation most sponsors have in
   the wrong order. **The answer is robust to the discount convention.** At 10 % the retention
   cost is 3,080,878.89 and at 6 % it is 3,790,838.88, so the transfer pays across the plausible
   range; when a conclusion survives its own sensitivity band, say so, because that is what makes
   it usable in a negotiation. **Two costs, not one, and they add.** Domain 11's method prices the
   transferee's premium against the retainer's expected cost; this prices the financing
   consequence. Both are real, they are additive, and an allocation decision taken on either alone
   is taken on half the arithmetic. And **the transfer is only worth its price if the transferee
   can pay** — a fixed-price wrap from a contractor whose balance sheet cannot absorb the loss is a
   priced illusion, which is precisely why Domain 11 tests capability and Domain 12 writes the
   security for it. One caution belongs on the record: the 8-point de-gearing is a *lender
   judgment*, not a formula — Domain 10's sizing shows what governs it — so the honest presentation
   of this result quotes the gearing response as the assumption it is, and re-runs it if the credit
   committee lands somewhere else.

### 1.2.2 Cash, not profit, is the binding constraint

**The principle.** Profit is an opinion about periods (Domain 2's accrual model); **cash pays
debt service**. Projects die of cash exhaustion, usually while reporting profits.

**Worked example 1.2.2 — profitable and out of cash.**

1. **Setup.** In its first operating quarter a project company recognises revenue of
   USD 10,000,000 against costs of USD 8,000,000. But customers have paid only 7,000,000 of
   the revenue (receivables +3,000,000); spare-parts inventory was built up by 1,000,000; and
   suppliers extended 500,000 of additional credit (payables +500,000).
2. **Formula.** Operating cash flow = profit − Δreceivables − Δinventory + Δpayables.
3. **Substitution.** `2,000,000 − 3,000,000 − 1,000,000 + 500,000`.
4. **Result.** Profit **+USD 2,000,000**; operating cash flow **−USD 1,500,000**.
5. **Interpretation.** The same quarter is a success in the income statement and a crisis in
   the bank account: a 2.0m "profitable" company is 1.5m short of the cash its debt service
   assumed. This is why lenders size and test debt against **CFADS** — cash flow available
   for debt service (defined fully in Domain 10) — and why every model in Domain 6 is a *cash*
   model first. Domain 2 builds the full accrual-to-cash bridge. Three things worth extracting
   before moving on. **The breakeven is 1,500,000 of receivables.** Holding the inventory build
   and the supplier credit where they are, operating cash flow crosses zero when receivables rise
   by `2,000,000 − 1,000,000 + 500,000 = 1,500,000` — which on quarterly revenue of 10,000,000 is
   **13.6875 days** of sales, on a 91.25-day quarter. The company did not miss that threshold by a
   little: at 3,000,000 its receivables are **27.3750 days**, twice the level at which the quarter
   turns cash-negative. **The drain compounds if the pattern is structural rather than
   seasonal.** A first quarter's working-capital build is a genuine one-off — the balance sheet is
   being filled — but the same absorption repeated through the year is **6,000,000** of cash gone
   against 8,000,000 of annual profit, and telling those two cases apart is the whole of the
   analysis. **The shortfall has to be funded before it is explained.** A 1,500,000 hole is met
   from a working-capital facility, a funded reserve or an equity call, and the working-capital
   line is among the most commonly omitted items in a sources-and-uses statement — the same class
   of defect as the unfunded reserve Domain 13 (KA 13.2.3) found in Kestrel's own funding plan,
   discovered late because such statements are reviewed for arithmetic, which always balances,
   rather than for completeness, which does not.

On a financed project the same divergence does not merely embarrass a management report — it walks
a covenant, and the distance to that covenant is best stated in the one unit an operating team
controls, because collection days are something a team can be held to and a currency amount is not.

**Worked example 1.2.2b — Kestrel's cash gap, in days of receivables.**

1. **Setup.** Kestrel's first operating year (Domain 2): revenue **USD 12,000,000**; `CFADS`
   **6,984,000** before working-capital movements and **6,384,000** after, the difference being
   **600,000** absorbed into receivables and spares. Debt service is the master instalment
   **5,009,635.23**. The facility's financial covenant is a `DSCR` of **1.20×** (Domain 10,
   KA 10.2.1).
2. **Formula.** `DSCR = CFADS ÷ debt service`. Covenant `CFADS` = 1.20 × debt service. Headroom =
   actual `CFADS` − covenant `CFADS`. Days of revenue = amount ÷ annual revenue × 365.
3. **Substitution.** `6,984,000/5,009,635.23` and `6,384,000/5,009,635.23`; covenant cash
   `1.20 × 5,009,635.23`; headroom `6,384,000 − 6,011,562.28`; days `× 365/12,000,000`.
4. **Result.**

   | | USD | `DSCR` | Days of revenue |
   |---|---|---|---|
   | `CFADS` before working capital | 6,984,000 | **1.3941** | — |
   | Working capital absorbed | (600,000) | (0.1198) | **18.2500** |
   | `CFADS` as documented | 6,384,000 | **1.2743** | — |
   | Covenant floor at 1.20× | 6,011,562.28 | 1.2000 | — |
   | **Remaining headroom** | **372,437.72** | **0.0743** | **11.3283** |
   | Total working-capital tolerance from 6,984,000 | 972,437.72 | 0.1941 | **29.5783** |

   Sensitivity: **50,096.35** of `CFADS` — **1.5238 days** of revenue — moves `DSCR` by 0.01×.
5. **Interpretation.** The project has **29.58 days** of revenue-equivalent working-capital
   tolerance before a 1.20× covenant fails, and it has already spent **18.25** of them — **61.70 %
   of the tolerance, consumed in the first operating year and reported as normal.** What remains is
   **11.33 days**, which is the sentence to put in front of an operations director, because "the
   covenant is eleven days of collections away" is actionable in a way that "headroom is 372,438"
   is not. Four professional readings follow. **The two `DSCR` figures are the same project.**
   1.3941 and 1.2743 differ by nothing but a definition of `CFADS`, and Domain 10's case study
   records a sponsor arguing for the higher one; the facility's documented definition decides, and
   a ratio quoted without its definition is not a ratio. **The sensitivity is the number to
   memorise.** At this debt-service level roughly a day and a half of receivables is worth 0.01× of
   `DSCR` — which is also, not coincidentally, the materiality threshold Domain 13 set for its model
   audit (KA 13.2.2), so the two disciplines are calibrated to the same unit. **Working capital is
   recoverable and that is exactly what makes it dangerous.** The 600,000 is not a loss; it comes
   back if collections normalise, which is why it attracts less scrutiny than a cost overrun of the
   same size — and a covenant test is measured on the date it falls, not on the date the cash
   eventually arrives. **The direction of the next movement is not symmetric.** Growth, indexation
   and a rising tariff all *increase* the receivables balance, so a project whose revenue is
   escalating is consuming headroom at an increasing rate unless collection days improve, which is
   the honest reason a first-year covenant test is the one lenders watch hardest. The caution: the
   day translation assumes the whole absorption behaves like receivables on this revenue base; a
   spares build is not collectable and does not respond to a collections campaign, so the
   instruction to an operations team must name *which* days it is asking for.

### 1.2.3 Leverage, risk and the bankability triangle

**Leverage amplifies.** Debt is cheaper than equity, and fixed: whatever the project earns,
debt service is owed. That fixity cuts both ways.

**Worked example 1.2.3 — the two faces of leverage.**

1. **Setup.** A project costs USD 100,000,000 and produces steady operating cash of
   USD 12,000,000 per year. Compare an all-equity structure with 70 % debt
   (USD 70,000,000, interest-only at 6.0 % = 4,200,000 per year), in the base case and with
   cash down 25 % and 50 %.
2. **Formula.** Unlevered return = cash / 100,000,000. Equity cash = cash − 4,200,000;
   levered return = equity cash / 30,000,000.
3. **Substitution.** Base: `12.0 − 4.2 = 7.8` on 30. Down 25 %: `9.0 − 4.2 = 4.8`. Down 50 %:
   `6.0 − 4.2 = 1.8`.
4. **Result.**

   | Scenario | Project cash | Unlevered return | Equity cash | Levered return |
   |---|---|---|---|---|
   | Base | 12,000,000 | 12.0 % | 7,800,000 | **26.0 %** |
   | −25 % | 9,000,000 | 9.0 % | 4,800,000 | **16.0 %** |
   | −50 % | 6,000,000 | 6.0 % | 1,800,000 | **6.0 %** |
   | −65 % | 4,200,000 | 4.2 % | 0 | **0.0 %** |

5. **Interpretation.** Leverage more than doubles the base-case equity return (26 % vs 12 %)
   — and makes the downside three times steeper (26 → 6 % as cash halves, versus 12 → 6 %
   unlevered). At a 65 % cash decline the equity earns nothing and the lender is next in
   line. Gearing is chosen, not maximised: Domain 9 structures it, Domain 10 shows how
   lenders cap it with coverage ratios sized precisely against scenarios like this table.
   Three structural facts sit inside that table and are worth extracting, because each one is a
   check a reviewer can run in a single line. **The 26 % is an identity, not a coincidence.**
   `r_equity = r_unlevered + (D/E) × (r_unlevered − r_debt)`, so
   `12.0 % + (70/30) × (12.0 % − 6.0 %) = 12.0 % + 2.333333 × 6.0 % = 26.0000 %` — exactly the
   table's base-case row. Read it as the sentence it is: **the levered return is the unlevered
   return plus the gearing ratio times the spread the project earns over its debt.** That is where
   the amplification comes from, it is why the amplification is *linear* in `D/E`, and it gives an
   instant audit of any levered-return figure presented without its workings. **The crossover is
   where the spread vanishes.** When the project's unlevered return equals the cost of debt —
   here at project cash of exactly `100,000,000 × 6.0 % = 6,000,000` — the spread term is zero and
   the levered and unlevered returns coincide at 6.0 %, which is the `−50 %` row's apparent
   coincidence. Above that cash level leverage helps; below it, leverage *hurts*, and the identity
   says so with a sign. A structure is therefore not "geared for return" in any general sense: it
   is geared for return only in the states of the world where the project out-earns its debt, and
   the whole downside case is the region where the arrangement runs in reverse. **The cliff is a
   property of the debt's shape, not of the gearing.** The 65 % decline in the last row is the
   distance to zero equity cash *for interest-only debt*, and interest-only debt is the exception
   in project finance rather than the rule; the next example moves the cliff by more than half its
   distance without changing the gearing, the rate or the project by one dollar.

**Worked example 1.2.3b — the same 70 % gearing, amortising: where the cliff really is.**

1. **Setup.** The identical project and structure — 100,000,000 of capital cost, 70,000,000 of
   senior debt at 6.0 %, 30,000,000 of equity, steady operating cash of 12,000,000 — but the debt
   amortises over **12 years** on the master facility's terms rather than paying interest only.
   The lender's financial covenant is a `DSCR` of **1.20×**.
2. **Formula.** Level instalment = `debt ÷ AF(r, n)`. Equity cash = project cash − instalment;
   cash-on-cash return = equity cash ÷ equity. `DSCR` = project cash ÷ instalment. The cliff is
   the cash level at which equity cash reaches zero; the covenant bites where `DSCR` = 1.20.
3. **Substitution.** `AF(0.06, 12) = 8.383844`; `70,000,000/8.383844 = 8,349,392.06`. Equity cash
   `12,000,000 − 8,349,392.06`. `DSCR = 12,000,000/8,349,392.06`. Covenant cash
   `8,349,392.06 × 1.20`.
4. **Result.**

   | | Interest-only (WE 1.2.3) | Amortising over 12 years |
   |---|---|---|
   | Annual debt service | 4,200,000 | **8,349,392.06** |
   | Of which year-one interest | 4,200,000 | 4,200,000 |
   | Of which principal repaid | nil | **4,149,392.06** |
   | Equity cash at 12,000,000 | 7,800,000 | **3,650,607.94** |
   | Cash-on-cash return on 30,000,000 | **26.0000 %** | **12.1687 %** |
   | `DSCR` at base case | 2.8571 | **1.4372** |
   | Cash at which the 1.20× covenant fails | 5,040,000 | **10,019,270.47** — a **16.5061 %** decline |
   | Cash at which equity cash reaches zero | 4,200,000 — a 65.0000 % decline | **8,349,392.06** — a **30.4217 %** decline |

5. **Interpretation.** Two conclusions, and they point in opposite directions, which is why this
   example matters more than its arithmetic. **The first is a warning about the metric.** The
   headline equity return collapsed from 26.00 % to **12.1687 %** — barely above the 12.0 %
   unlevered return — and the project did not get worse by one cent. Year-one interest is
   **identical at 4,200,000** in both columns; the entire difference is the **4,149,392.06** of
   principal repaid, and principal repayment is a *return of capital*, not a cost. The amortising
   equity is better off than the cash-on-cash figure implies, because it also owns a claim that is
   de-levering by 4,149,392 in the first year and by more in every year after it. Single-period
   cash-on-cash return is therefore the wrong
   instrument for judging a levered structure whenever debt amortises, and the right one is the
   equity `IRR` over the whole life (Domain 4's measure, applied to the structure in Domain 9,
   KA 9.1.4) — which is exactly why an interest-only illustration of leverage, WE 1.2.3 included,
   flatters every structure it is applied to. **The second is a warning about the cliff, and it is
   the one that reaches a credit committee.** The distance to zero equity cash more than halved,
   from a 65.00 % decline to **30.42 %** — and the *covenant* bites far earlier still, at a
   **16.51 %** decline in project cash. Rank them: the lender's protection engages at −16.51 %, the
   equity's own cash runs out at −30.42 %, and the interest-only reading suggested −65.00 %. An
   analyst who models leverage interest-only and reports the resilience of the structure has
   overstated the equity's cushion by a factor of roughly four, and the error is invisible in the
   base case, where every number looks fine. The caution to carry: the
   amortising column is not "worse". It is a **different bargain** — a structure that repays
   principal converts operating cash into ownership and buys future distribution capacity, which is
   why sculpted and back-ended profiles exist and why Domain 10's sizing levers include the
   repayment shape and not only the amount. What is unarguable is that the three thresholds in the
   table are the ones a leader must be able to state from memory about their own project.
   Fig 1.2.2 draws all three lines together, and the geometry is worth more than the table: two
   straight lines of identical slope, offset by the principal repayment, crossing the unlevered
   line at two very different places.

**The bankability triangle.** Three tests every financing must pass simultaneously:
**value** (the project is worth doing — Domain 4), **cash** (the flows arrive in the periods
that need them — Domains 6–8), and **risk allocation** (each hazard sits with a party able
and bound to bear it — Domains 11–12). A project strong on two corners and weak on one is
not two-thirds bankable; it is unbankable until the corner is fixed. **Bankability** (built
fully in Domain 5) is precisely the state of passing all three tests in the eyes of the
capital being asked to commit.

> **Fig 1.2.1 — The bankability triangle.** Equilateral triangle diagram. Corners: "VALUE —
> worth doing (NPV, Domain 4)" · "CASH — arrives when needed (Domains 6–8)" · "RISK — sits
> with those who can bear it (Domains 11–12)". Centre label: "BANKABLE — all three, together
> (Domain 5)". Each edge annotated with the failure mode of the missing corner: valuable but
> cash-mistimed → liquidity failure; cash-rich but mispriced risk → repricing at close;
> well-allocated but low value → no equity. Source: PCI original. Alt text: triangle whose
> corners are value, cash and risk allocation, with bankability at the centre and each
> missing corner's failure mode noted along the edges.

> **Fig 1.2.2 — Where leverage helps, and where the cliff is.** Line chart. X-axis project
> operating cash 3,000,000 to 14,000,000; y-axis equity cash return on the 30,000,000 equity,
> −20 % to +35 %. Three lines: dashed grey **all equity** (`cash/100,000,000`); solid blue
> **70 % interest-only** (`(cash − 4,200,000)/30,000,000`); solid crimson **70 % amortising over
> 12 years** (`(cash − 8,349,392.06)/30,000,000`). Marked: the crossover at **6,000,000** where
> the all-equity and interest-only lines meet at **6.0 %** (below it, leverage subtracts);
> vertical drop-lines at the two cliffs, **4,200,000 (−65.00 %)** in blue and **8,349,392
> (−30.4217 %)** in crimson; a dashed vertical at the **1.20× covenant, 10,019,270**. Three dots
> at the base case of 12,000,000 read **26.0000 %** (interest-only), **12.1687 %** (amortising) and
> **12.0000 %** (all equity). Side panel notes that the same 70,000,000 at 6.0 % pays identical
> year-one interest of 4,200,000 in both geared cases. Source: PCI original. Alt text: three
> return lines against project cash showing that leverage adds return only above the crossover,
> and that an amortising structure's equity cliff sits at half the cash decline of an
> interest-only one, with the lender's covenant biting earlier than either.

### AI in this KA

Scenario tables like 1.2.3's are ideal machine work — and the place where a subtly wrong
fixed-charge assumption (interest-only vs amortising; Domain 3's shapes) silently reshapes
every row. WE 1.2.3b measures the damage: the same gearing, rate and project, and a cliff that
moves from a 65.00 % cash decline to 30.42 %, with the covenant at 16.51 %. The governed pattern:
the machine drafts the grid; the analyst re-derives one row by hand and checks the boundary case
(where equity cash crosses zero) analytically; the leader reads the *downside* rows first,
because that is what the structure is actually being designed against.

**The verification that is specific to a foundations model.** Ask the tool for the *identity*, not
only the table. A grid that cannot reproduce
`r_equity = r_unlevered + (D/E) × (r_unlevered − r_debt)` on its own base-case row has an error in
it somewhere, and the check costs one line; a grid whose rows satisfy the identity is still wrong
if the fixed charge is the wrong shape, so the second check is to confirm that the debt service in
the table equals `debt ÷ AF(r, n)` for the documented tenor. Those are the first two entries on the
reviewer's list at 1.A.3, and they are the two worth running before any other.

### Key terms — KA 1.2

| Term | Meaning |
|---|---|
| **CFADS** | Cash flow available for debt service (name fixed here; machinery in Domain 10). |
| **Working capital drag** | Cash absorbed by receivables and inventory ahead of profit. |
| **Leverage / gearing** | Debt's share of funding; amplifier of equity return and risk. |
| **Fixed charge** | Debt service owed regardless of performance. |
| **Bankability triangle** | Value + cash + risk allocation, passed together. |
| **Levered-return identity** | `r_e = r_u + (D/E) × (r_u − r_d)`; amplification is linear in gearing and proportional to the spread over the debt rate. |
| **Leverage crossover** | The cash level at which the unlevered return equals the cost of debt, so gearing neither adds nor subtracts. |
| **Equity cliff** | The cash level at which debt service consumes all project cash; a function of the debt's *shape*, not only its size. |
| **Cost of retention** | The present value of the capital-structure change lenders impose when a risk is retained; the sponsor's true reservation price for transferring it. |

### Sample MCQs — KA 1.2

**MCQ 1.2-A `[1.2.2 · Application]`** A company reports quarterly profit of 2,000,000 while
receivables rose 3,000,000, inventory rose 1,000,000 and payables rose 500,000. Its operating
cash flow is:
- A. +2,000,000
- B. −1,500,000 ✅
- C. +500,000
- D. −2,500,000

*Rationale:* `2.0 − 3.0 − 1.0 + 0.5 = −1.5m`. A stops at profit; C nets only the payables
against profit and forgets the asset build; D subtracts the payables increase instead of
adding it — supplier credit is a cash *source*.

**MCQ 1.2-B `[1.2.3 · Application]`** In the leverage example (70m debt, interest-only 6 %;
equity 30m), project cash of 9,000,000 produces a levered equity return of:
- A. 9.0 %
- B. 16.0 % ✅
- C. 26.0 %
- D. 30.0 %

*Rationale:* `(9.0 − 4.2)/30 = 16.0 %`. A is the unlevered return; C is the base-case levered
return; D divides project cash by equity without paying the lender first.

**MCQ 1.2-C `[1.2.3 · Analysis]`** A project shows strong NPV and well-allocated risks, but
its revenue arrives seasonally while debt service is quarterly and level. The bankability
verdict is:
- A. bankable — two of three corners suffice
- B. unbankable as structured: the cash corner fails; reshape the debt profile or add liquidity support ✅
- C. unbankable permanently — reject the project
- D. bankable if the sponsors accept a higher equity IRR

*Rationale:* The triangle is conjunctive: mistimed cash defeats value and allocation. The
cure is structural (sculpted or seasonal debt service, reserve accounts — Domains 9–10), not
rejection (C) and not a return adjustment that changes nothing about timing (D).

**MCQ 1.2-D `[1.2.3 · Application]`** The same project (100,000,000 cost, 12,000,000 of annual
operating cash, 70,000,000 of senior debt at 6.0 %, 30,000,000 of equity) is financed with debt
**amortising over 12 years** rather than interest-only. `AF(0.06, 12) = 8.383844`. The base-case
cash-on-cash equity return is:
- A. 12.1687 % ✅
- B. 26.0000 %
- C. 40.0000 %
- D. 12.0000 %

*Rationale:* Instalment `70,000,000/8.383844 = 8,349,392.06`; `(12,000,000 − 8,349,392.06)/
30,000,000 = 12.1687 %` (WE 1.2.3b). B is the interest-only reading, which charges only the
4,200,000 of interest; C divides project cash by equity and never pays the lender at all;
D concludes that amortisation makes leverage exactly neutral — close, and wrong by the 17 basis
points that are the whole of the remaining spread benefit.

**MCQ 1.2-E `[1.2.2 · Application]`** Kestrel's documented `CFADS` is 6,384,000, debt service is
5,009,635 and the covenant is 1.20×; annual revenue is 12,000,000. Expressed in days of revenue,
the remaining covenant headroom is:
- A. 11.33 days ✅
- B. 18.25 days
- C. 29.58 days
- D. 1.52 days

*Rationale:* Headroom `6,384,000 − 6,011,562 = 372,438`; `372,438/12,000,000 × 365 = 11.33 days`
(WE 1.2.2b). B is the working capital *already* absorbed, which is history rather than headroom;
C is the total tolerance measured from the pre-working-capital `CFADS` of 6,984,000, so it
double-counts the 600,000 already spent; D is the sensitivity — the days worth 0.01× of `DSCR` —
mistaken for the headroom itself.

**MCQ 1.2-F `[1.2.1 · Evaluation]`** A contractor offers to take a construction risk for
1,350,000; the sponsors' expected cost of the risk is 900,000, and lenders have said they will
de-gear from 70 % to 62 % of a 60,000,000 envelope if it is retained. `k_e` = 15.42 %,
`k_d` = 6.0 %, `AF(0.08, 12)` = 7.536078. The sound conclusion is:
- A. refuse: the quote is loaded 50 % above expected cost
- B. accept: retention costs 3,407,513 in present value through the capital structure, so the transfer creates 2,057,513 of value ✅
- C. accept only if the contractor reduces the price to 900,000
- D. indifferent: risk transfer is value-neutral by construction

*Rationale:* `4,800,000 × (0.1542 − 0.0600) × 7.536078 = 3,407,513.04` (WE 1.2.1). A compares the
quote with the risk's own expected cost and ignores where a retained risk is actually paid for; C
demands a price at which no contractor would accept the risk, since the transferee must be paid for
uncertainty as well as expectation; D denies the gain from trade that the equity-debt spread
creates.

**MCQ 1.2-G `[1.2.3 · Evaluation]`** An analyst's paper on the WE 1.2.3 project — 100,000,000 of
capital cost, 12,000,000 of steady operating cash, 70,000,000 of senior debt at 6.0 % and 30,000,000
of equity — reports that the structure "absorbs a 65 % fall in project cash before the equity's own
cash runs out", having modelled the debt as interest-only when the facility amortises over 12 years
and carries a 1.20× `DSCR` covenant. Each objection below is a fair one. Which is the more decisive
for the credit committee?
- A. the paper overstates the cushion that matters: on the amortising facility the equity's cash exhausts at a 30.42 % decline and the 1.20× covenant engages at 16.51 %, so the reported resilience is roughly four times the distance to the first consequence ✅
- B. the paper's 26.0000 % cash-on-cash return is overstated, since the amortising structure returns 12.1687 %
- C. the paper should have measured the equity with an `IRR` over the whole life rather than a single-period return
- D. the paper omits the tax shield on the interest charge

*Rationale:* A and B follow from the same substitution, but the committee is buying downside
protection, and only A quantifies it — the covenant bites at −16.51 %, the equity's cash runs out at
−30.42 %, and the paper reported −65.00 % (WE 1.2.3b). B is true and second-order, and on its own it
is a *misleading* correction: the **4,149,392.06** of principal repaid is a return of capital rather
than a cost, so the amortising equity is better off than 12.1687 % implies. C is a sound
methodological point that quantifies nothing by itself. D is true of both columns equally —
year-one interest is identical at 4,200,000 — so it cannot be an objection to this paper.

**MCQ 1.2-H `[1.2.3 · Comprehension]`** The levered-return identity is
`r_e = r_u + (D/E) × (r_u − r_d)`. Which statement restates what it says about a structure geared
70/30 against debt costing 6.0 %?
- A. gearing adds return in every state of the world, because debt is cheaper than equity
- B. gearing adds 2.333333 times whatever the project earns above 6.0 % and subtracts 2.333333 times whatever it earns below it, so one structure amplifies in both directions ✅
- C. gearing raises the equity return by the difference between the cost of equity and the cost of debt
- D. gearing raises the equity return wherever the project's unlevered return exceeds the equity holders' required return

*Rationale:* the identity multiplies the **spread over the debt rate** by the debt-to-equity ratio, so
its sign follows the spread: at an unlevered 12.0 % it adds **14.0000** points to reach 26.0000 %, and
at an unlevered 4.0 % it subtracts **4.6667** points to −0.6667 % (WE 1.2.3). A ignores the sign; below
the crossover — project cash of 6,000,000 on this structure — leverage subtracts. C names the wrong
spread: the identity uses `r_u − r_d`, not `k_e − k_d`. D substitutes the equity's required return for
the debt rate and so puts the crossover in the wrong place.

### Self-check — KA 1.2

1. *Why do lenders test CFADS rather than profit?* — Debt service is paid in cash; accrual
   profit can coexist with cash exhaustion (WE 1.2.2).
2. *State leverage's two faces in one sentence each.* — It multiplies the equity return on
   the same project cash; it makes every downside steeper because debt service is fixed.
3. *Why is the triangle conjunctive?* — Each corner defeats the financing alone: no value →
   no equity; no cash timing → default risk regardless of value; no allocation → risk premia
   or collapse at close.
4. *State the levered-return identity and what it implies below the crossover.* —
   `r_e = r_u + (D/E) × (r_u − r_d)`; where `r_u < r_d` the spread term is negative and gearing
   *reduces* the equity return.
5. *Why does amortisation move the equity cliff so far?* — Debt service rises from interest alone
   to interest plus principal (4,200,000 → 8,349,392), halving the cash decline the equity can
   absorb, from 65.00 % to 30.42 %, with the covenant engaging at 16.51 %.
6. *Where is a retained risk paid for?* — In the capital structure: lenders de-gear, equity
   replaces debt, and the cost is the equity-debt spread on the substituted amount.

---

## Knowledge Area 1.3 — Ethics, fiduciary awareness and responsible AI

*Topics: 1.3.1 obligations and duties · 1.3.2 conflicts and independence · 1.3.3 the
responsible-AI principle in finance.*

### 1.3.1 Obligations and duties

The project finance leader acts inside a lattice of duties: **fiduciary-type duties** to the
employer or client (loyalty, care, confidentiality); **contractual duties** under mandates
and finance documents; **statutory duties** (companies law, anti-bribery and corruption,
sanctions, market conduct); and **professional duties** — competence, candour, and records
that let others check the work. Two standing disciplines follow. *Candour about numbers*:
forecasts are presented with their assumptions and sensitivities, never as certainties
(Domain 4, KA 4.3.3); an optimistic case knowingly presented as a base case is a
misrepresentation, whatever the spreadsheet says. *Candour about limits*: this book's own
rule — educational reference, not individualized advice; jurisdiction-specific matters go to
qualified counsel and advisers — is the same professional humility applied to oneself.

### 1.3.2 Conflicts and independence

Project finance concentrates conflicts because the same institutions recur in many roles:
the adviser who would earn a success fee on close; the sponsor-affiliated contractor pricing
the EPC; the bank advising the government while lending to bidders. The professional
machinery is disclosure and separation: conflicts are declared before engagement, managed
with information barriers or declined; advice and self-interest are never silently blended.
The leader's test for any arrangement is the *daylight test* — would every party, seeing the
full fee and relationship map, still regard the advice as independent? Where the answer
wavers, independence has already failed. (Case study B applies this to a live tender;
Domain 13's diligence streams exist partly to give lenders advice that passes the test.)

**Why the commercial argument runs the same way as the duty.** The duty to disclose does not
depend on what disclosure costs, and a professional who needs the arithmetic before deciding has
already answered a different question. But the arithmetic is worth having for one specific
reason: partners and committees do sometimes ask to "weigh it commercially", and the leader who
can show that the commercial case points the same way removes the last excuse for not disclosing.
The structure of the calculation is what carries the lesson.

**Worked example 1.3.2 — the conflict priced at discovery.**

1. **Setup.** Fictitious throughout. An advisory firm holds a mandate advising a grantor on a
   toll-road tender. It is offered a second, unrelated mandate from the lead sponsor of one of the
   bidding consortia. Declining that second mandate — or accepting it, disclosing it and living
   with the barriers — would cost the firm the **250,000** fee at risk on the second engagement.
   If instead the relationship is not disclosed and is later discovered, the firm's own estimates
   are: the unpaid balance of the tender mandate, **900,000**, forfeited; the loss of its
   public-sector practice in that jurisdiction — **three mandates a year at 850,000 for five
   years**; and **400,000** of legal and professional-indemnity cost. On the grantor's side the
   discovered conflict forces a re-run: **1,100,000** to re-appoint an adviser, **2,100,000** of
   fresh evaluation and legal cost, and **three bidders' abortive bid costs at 1,800,000 each**.
2. **Formula.** Expected value of concealment = cost avoided by not disclosing −
   (probability of discovery × loss on discovery). The **breakeven discovery probability**
   `p* = cost avoided ÷ loss on discovery`.
3. **Substitution.** Franchise loss `3 × 850,000 × 5 = 12,750,000`; total private loss
   `900,000 + 12,750,000 + 400,000`. `p* = 250,000 ÷ 14,050,000`. Grantor's re-run
   `1,100,000 + 2,100,000 + 3 × 1,800,000`.
4. **Result.** Loss on discovery to the firm **USD 14,050,000**; breakeven discovery probability
   **1.7794 %**. Cost imposed on others **USD 8,600,000**. Total value destroyed **USD 22,650,000**
   against a **250,000** saving — a ratio of **90.6000 to 1**. Expected value of concealment at a
   10 % discovery probability: **−1,155,000**; at 25 %: **−3,262,500**; at 50 %: **−6,775,000**.
5. **Interpretation.** Concealment pays only if the chance of discovery is below **1.78 %**, and in
   a competitive tender that number is not remotely available: the losing bidders have every
   incentive to look, professional relationships in a single jurisdiction's infrastructure market
   are small and traceable, and the discovery event does not require anyone to prove that the advice
   was actually biased — Case study B turns on exactly that point. So **the expected value of
   concealment is negative at every plausible probability**, which is the first useful result.
   The second is structural, and it is the one to carry: **the calculation has no upside term.**
   What concealment "buys" is bounded above by the fee at risk on the engagement being hidden —
   250,000 here — while what it risks is the franchise, which is a multiple of any single fee
   because it is the discounted value of every future fee. Any framing in which non-disclosure
   appears to have a large benefit has mis-specified the benefit, and the reviewer's cue is to ask
   what, precisely, is on the left-hand side. Third, note **who bears the loss**: 14,050,000 falls
   on the firm and 8,600,000 on parties who had no part in the decision — the grantor's programme
   and three bidders' abortive costs — which is why this is an ethical question and not a
   risk-appetite question. A firm entitled to gamble its own franchise is not entitled to gamble
   other people's. And the boundary of the arithmetic: this is expected-value reasoning applied to
   a **duty**, and duties are not tradeable. The correct use of the table is to close a commercial
   argument that should never have been opened; the wrong use is to invite the calculation wherever
   the duty is inconvenient, and a leader who reaches for it more than once has a culture problem
   rather than a disclosure problem. *Counsel pointer:* whether an undisclosed relationship gives
   losing bidders a right of challenge, what remedies attach and what disclosure a procurement
   regime positively requires are jurisdiction-specific and time-variable questions for qualified
   counsel; nothing here states the law of any jurisdiction.

### 1.3.3 The responsible-AI principle in finance

The suite principle — **AI proposes; the professional verifies, decides and remains
accountable** — lands hardest in finance, where machine output *looks* like the work product
itself (a model, a memo, a covenant summary). Domain 16 builds the full governance
architecture; the foundations fixed here:

- **Verification is not optional and not delegable to the tool.** Golden-answer checks for
  calculations (the discipline this book applies to itself); source-tracing for claims;
  document-against-summary checks for AI-read contracts.
- **Accountability cannot be transferred.** "The model said so" is never a defence — the
  signing professional owns the output as if hand-made.
- **Confidentiality travels with the data.** Deal information entering an AI tool is a
  disclosure; it happens only within approved, contracted environments.
- **Material AI use is disclosed** within the team and, where it touches deliverables,
  to the client — the daylight test again, applied to method.

**What the verification duty actually asks for.** "Check the output" is not an instruction until
it distinguishes two different objects. Verifying a **number** means recomputing it from stated
inputs by an independent route and reconciling to the digit — the discipline this book applies to
itself, and the reason every figure in this domain carries a derivation. Verifying a **claim** —
that a transaction happened, that a clause says what a summary says it says, that a market
convention is current — cannot be done by recomputation at all; it requires tracing the assertion
to a source that exists and is the right version. The two failure modes are different, they are
caught by different habits, and the second is the one that costs a negotiation. A verification
record that satisfies both is short: what was checked, by whom, against what, on what date, and
what was found. Anything shorter is an assertion that checking happened.

**Worked example 1.3.3 — the precedent that did not exist, priced.**

1. **Setup.** Fictitious throughout. Preparing for Kestrel's term-sheet negotiation, an analyst
   asks a general-purpose AI tool for comparable senior margins on recent water-concession
   financings. The summary returned is fluent, cites five transactions and reports comparable
   pricing at **175 basis points**. Verified pricing on the transactions that actually exist is
   **235 basis points**; two of the five cited deals cannot be traced to any public record. On the
   strength of the 175, the sponsor anchors its position there and holds it for **nine weeks**
   before accepting 235. Kestrel's cost of pre-close delay is Domain 13's **USD 124,133.33 per
   calendar week** (KA 13.1.3, derived from Domain 5's forgone `CFADS` of 17,733.33 a day on a
   30/360 basis). The facility is 42,000,000 over 12 years; `AF(0.06, 12) = 8.383844`.
2. **Formula.** Cost of the episode = weeks lost × cost of delay. Value the false benchmark
   appeared to offer = margin difference × facility, discounted over the tenor. Breakeven
   probability of the negotiation succeeding = cost ÷ value.
3. **Substitution.** `9 × 124,133.33`. `(0.0235 − 0.0175) × 42,000,000 = 252,000` a year;
   `252,000 × 8.383844`. Then `1,117,200 ÷ 2,112,729`.
4. **Result.** Cost of the nine weeks **USD 1,117,200**. Present value of the 60-basis-point prize,
   had it been real: **USD 2,112,729**. Breakeven success probability **52.8795 %**. Realised
   benefit: **nil**.
5. **Interpretation.** The unverified figure cost **1,117,200** and delivered none of the
   **2,112,729** it appeared to promise — and the cost was incurred not by using AI but by allowing
   an unsourced number to become a *negotiating position*. Four readings. **The breakeven is the
   discipline, not the loss.** Even if the 175 had been genuine, spending nine weeks to win it
   required better than a **52.88 %** chance of the lender conceding — which is the question a
   negotiating team should have asked on day one and did not, because the benchmark's apparent
   authority made the position feel free. Any negotiating position held on a critical path has a
   breakeven success probability, and computing it converts stubbornness into a decision. **The
   cost of verification was trivial and the loss was not.** Tracing five cited transactions to
   public records is an hour's work; it would have found the two that do not exist and would have
   changed the position before it was taken. That asymmetry — hours against a million — is the whole
   economic case for the verification duty, and it is why "we did not have time to check" inverts
   the arithmetic. **The failure is not machine-specific, which is why the control is not
   machine-specific.** A stale internal benchmark spreadsheet produces the identical loss, and so
   does a scenario grid built before the debt's shape is settled (WE 1.2.3b); what the
   machine changes is the *fluency*, and therefore the probability that an unsourced figure survives
   into a decision unchallenged. The control is accordingly a rule about numbers, not about tools:
   **no benchmark enters a negotiating position without a source line naming the transaction, the
   document and the date.** And **the accountability sits with a person.** No part of the 1,117,200
   is recoverable from a tool vendor, and none of it would have been mitigated by the tool being an
   approved one (MCQ 1.3-C). Domain 16 builds the governance architecture that makes this systematic, including
   how to measure a tool's error rate on your own material rather than trusting a general claim; the
   foundational point is smaller and prior: **a number without a source is not evidence, and a
   professional who presents one has made an assertion in their own name.**

### Key terms — KA 1.3

| Term | Meaning |
|---|---|
| **Fiduciary awareness** | Acting in the principal's interest with loyalty, care and confidentiality. |
| **Conflict of interest** | An interest that could bias judgment; declared, managed or declined. |
| **Daylight test** | Would full disclosure of interests leave the advice trusted? |
| **Responsible-AI principle** | AI proposes; the professional verifies, decides, remains accountable. |
| **Verification duty** | The named human's obligation to check machine output before reliance. |
| **Breakeven discovery probability** | Cost avoided by concealment ÷ loss on discovery; the arithmetic that has no upside term. |
| **Source line** | The transaction, document and date behind a benchmark; without it a figure is an assertion, not evidence. |
| **Verifying a number vs a claim** | Recomputation by an independent route; versus tracing an assertion to a source that exists. |

### Sample MCQs — KA 1.3

**MCQ 1.3-A `[1.3.1 · Analysis]`** An analyst is asked to present the upside case as the
base case "because the committee needs confidence". The professional response is:
- A. comply — labels are a presentation choice
- B. decline: presenting a knowingly optimistic case as the base misrepresents the forecast; offer the honest base with sensitivities instead ✅
- C. comply, but keep a private note of the true base
- D. resign immediately without discussion

*Rationale:* Candour about numbers is a duty, not a style (A); a private note documents the
misrepresentation without preventing it (C); B both refuses the breach and offers the
legitimate route to confidence — evidence. D skips the professional obligation to fix the
problem before escalating personal exits.

**MCQ 1.3-B `[1.3.2 · Application]`** A bank advising a grantor on a tender also wishes to
lend to one of the bidders. The minimum acceptable handling is:
- A. proceed — different departments are involved
- B. disclose the dual role to the grantor, and either obtain informed consent with effective information barriers or decline one role ✅
- C. keep the lending discussion confidential until after award
- D. advise the grantor to select that bidder

*Rationale:* Disclosure plus genuine separation (or declination) is the standing machinery;
department labels alone are not barriers (A); concealment converts a conflict into
misconduct (C); D is the conflict operating in the open.

**MCQ 1.3-C `[1.3.3 · Recall]`** Under the PCI responsible-AI principle, responsibility for
an AI-drafted covenant summary used in a credit paper rests with:
- A. the AI vendor
- B. the model itself
- C. the professional who verified, signed and used it ✅
- D. nobody, if the tool was approved

*Rationale:* Accountability cannot be delegated to software or its supplier; tool approval
governs *which* tools may be used, never *who* answers for the output.

**MCQ 1.3-D `[1.3.2 · Application]`** Disclosing a second mandate would cost a firm the 250,000
fee at risk on it. If concealed and later discovered, the firm loses 900,000 of unpaid fees,
12,750,000 of jurisdictional franchise and 400,000 of legal cost. The breakeven probability of
discovery is:
- A. 1.7794 % ✅
- B. 27.7778 %
- C. 1.9608 %
- D. there is no breakeven — concealment can never pay at any probability

*Rationale:* `250,000/14,050,000 = 1.7794 %` (WE 1.3.2). B divides by the 900,000 of forfeited fees
alone and ignores the franchise, which is the largest term; C counts the franchise and drops the
forfeited fees and legal cost. D is the answer a reader gives from the duty rather than from the
arithmetic — the duty is indeed unconditional, but the calculation does have a finite breakeven,
and being able to state it is what closes a commercial argument.

**MCQ 1.3-E `[1.3.3 · Application]`** A negotiating position taken on an unverified AI-reported
benchmark held Kestrel's close nine weeks at a cost of delay of 124,133.33 per week. The cost of
the episode was:
- A. USD 1,117,200 ✅
- B. USD 2,112,729
- C. USD 252,000
- D. USD 124,133

*Rationale:* `9 × 124,133.33 = 1,117,200` (WE 1.3.3). B is the present value of the 60-basis-point
prize the false benchmark appeared to offer — the amount at stake, not the amount spent; C is one
year of that margin difference undiscounted; D prices a single week.

**MCQ 1.3-F `[1.3.3 · Comprehension]`** Under the responsible-AI principle as fixed in this domain,
which statement is correct?
- A. an approved tool's output may be relied on without further checking, since approval is the control
- B. verifying a number and verifying a claim are different acts: the first is independent recomputation, the second is tracing the assertion to a source that exists and is the right version ✅
- C. AI output must be disclosed to the client in every case, however immaterial
- D. confidentiality obligations do not attach to data entered into an AI tool, because no third party reads it

*Rationale:* 1.3.3 separates the two verification acts precisely because they fail differently and
are caught by different habits. A confuses tool governance with output accountability (MCQ 1.3-C);
C overstates the rule, which attaches to *material* use touching deliverables; D is wrong on the
foundational point that entering deal data into a tool is itself a disclosure.

**MCQ 1.3-G `[1.3.3 · Evaluation]`** After an unverified benchmark cost Kestrel **1,117,200** of
delay (WE 1.3.3), four controls are proposed. Which should the leader adopt first?
- A. prohibit general-purpose AI tools for market and precedent research
- B. require that no benchmark enter a negotiating position without a source line naming the transaction, the document and the date ✅
- C. procure an enterprise AI tool whose licence carries a vendor indemnity
- D. require a second analyst to review every AI-generated summary before it is circulated

*Rationale:* the loss arose because an unsourced number became a negotiating position, and a stale
internal spreadsheet produces the identical loss — so the control that matches the failure is a rule
about numbers rather than about tools (1.3.3). A is defensible and narrower than the risk: it forgoes
a genuine accelerant while leaving the human failure mode untouched. C buys a commercial remedy the
profession cannot rely on — on Kestrel's facts none of the 1,117,200 was a loss the licence reached,
and what a particular indemnity in fact covers is a question for the contract and for counsel rather
than a planning assumption — and tool approval governs *which* tools may be used, never *who* answers
for the output (MCQ 1.3-C). D would
probably have caught this instance and is the right *second* control, applied to material items; as a
blanket rule it charges a second analyst against every summary, most of which never reach a
negotiation.

**MCQ 1.3-H `[1.3.1 · Evaluation]`** A board asks for "one number" for the incremental cost of the
limited-recourse route, having been shown **5,202,128** built on a 140-basis-point margin
differential and a 40 % enforcement recovery. The most professional response is:
- A. give the 5,202,128 without qualification, since the board asked for a single figure
- B. give 5,202,128 as the base case, name the two parameters that move it most, and state the consequence — at a 50 % recovery the breakeven rises to 127.6910 % and the route cannot pay at any probability ✅
- C. decline to give a single figure, because the enforcement recovery is unknowable
- D. give the figure computed on the 50 % recovery, as the more prudent of the two

*Rationale:* candour about numbers means presenting a forecast with its assumptions and
sensitivities, never as a certainty (1.3.1), and B does what the board asked while disclosing what
would change the answer. A presents an estimate as a fact. C is a defensible instinct that fails the
duty from the other side — declining to quantify leaves the judgment to whoever will. D substitutes
one point estimate for another, and an unlabelled prudence misrepresents a forecast exactly as an
unlabelled optimism does (MCQ 1.3-A).

### Self-check — KA 1.3

1. *State the daylight test.* — Would every party, seeing the full relationship and fee map,
   still trust the advice as independent?
2. *What three checks make AI-assisted work professionally usable?* — Recomputed numbers
   (golden checks), traced sources, document-against-summary verification — by a named human.
3. *Why does confidentiality bind AI use?* — Data entering a tool is a disclosure; it must
   stay within approved, contracted environments.
4. *Why does the concealment calculation have no upside term?* — What concealment buys is bounded
   by the fee at risk on the hidden engagement; what it risks is the franchise, the value of every
   future fee.
5. *What must accompany a benchmark before it enters a negotiating position?* — A source line: the
   transaction, the document and the date — because a number without a source is an assertion made
   in the presenter's own name.

---

## Advanced topics — Domain 1

### 1.A.1 Where the ring-fence leaks

The ring-fence is a legal construction, not a law of nature, and a leader who treats it as
absolute has bought containment they may not own. Five leaks recur, and only the first is widely
discussed.

**Operational disregard.** The isolation that lenders relied on assumes the SPV is *run* as a
separate undertaking. Where accounts are commingled, board minutes are not kept, affiliate
contracts are not at arm's length, staff are indistinguishable from the parent's and the SPV has
no independent decision-making, a court asked whether the entities should be treated as one may
find that they should. The disciplines that preserve the fence are unglamorous and cheap — the
SPV's own bank accounts, its own books, its own minuted board, written affiliate agreements
priced as third parties would price them — and they are cheapest to install at inception and
almost impossible to reconstruct in a dispute.

**Cross-default in the *sponsor's* documents.** This is the leak that most often defeats the
purpose of the structure, and it is not in the project's documents at all. A parent whose own
revolving facility defines an event of default to include a material subsidiary's insolvency, or
whose covenants capture guarantees and contingent obligations, has converted a contained project
failure into a corporate event. The finance leader's obligation is therefore to read the
**parent's** debt documents before certifying that the project is ring-fenced — a step routinely
omitted because it belongs to a different team.

**Economic recourse without the label.** A support that is not called a guarantee may still
behave as one: an equity bridge drawn against the parent's lines, a debt-service-reserve letter
of credit issued on the parent's credit, an availability undertaking with an uncapped indemnity
buried in a schedule. The test is not what the instrument is called but **whose balance sheet
answers if it is called** — the same discipline that makes the recourse sentence a three-limbed
one (who, until when, capped at what).

**Behavioural recourse.** Lenders price the probability that a sponsor of standing will not walk
away from a project bearing its name, and sophisticated sponsors know they are being priced on it.
This expectation appears in no document and is not enforceable, and it is real: it is one reason
the margin differential in WE 1.1.2 is narrower for a well-known sponsor than the legal recourse
position alone would justify. The leader's task is to notice when a negotiation is trading on it,
because a benefit taken today on an implicit promise is a support obligation the board has never
approved.

**The fence works in both directions.** The construction that stops a failing project from
reaching the parent also stops the parent from helping it without consent: distributions are
locked up, additional debt is prohibited, and voluntary equity injections are often permitted only
on terms lenders control. Sponsors are regularly surprised by this at the first stress event,
which is the wrong time to discover it.

*Counsel pointer:* whether, when and on what basis a court will look through a corporate structure
— and what disclosure or consolidation an applicable financial-reporting framework requires — is
jurisdiction-specific, fact-specific and changes over time. These are questions for qualified
counsel and the reporting adviser at structuring, not matters on which any book can state a
universal position.

### 1.A.2 Sponsor support as a contingent claim

A recourse position on the spectrum of Fig 1.1.1 is a *contingent claim*, and treating it as one
converts an argument about principle into a computation. Domain 5 (KA 5.2.3) records Kestrel's
sponsor support: cost-overrun undertakings of **10 % of the 60,000,000 capital cost — 6,000,000 —
subscribed pro rata**. Two numbers follow immediately. The **effective recourse fraction** —
support ÷ senior debt — is `6,000,000/42,000,000 =` **14.2857 %**, which places the financing
quantitatively on the spectrum rather than gesturally, and is the figure to put in a board paper
in place of the words "limited recourse". And because lenders will price the facility differently
without the support, the support has a *market value* the sponsors can compare against their own
view of it.

**Worked example 1.A.2 — what is the cost-overrun undertaking worth?**

1. **Setup.** Lenders indicate that, without the 6,000,000 of cost-overrun support, Kestrel's
   senior margin would be **40 basis points** wider on the full 42,000,000 for the 12-year term.
   The support is capped at 6,000,000, uncalled unless overruns exceed contingency.
   `AF(0.06, 12) = 8.383844`.
2. **Formula.** Value of the support to the sponsors = present value of the margin saving it buys.
   Breakeven call probability `p* = that present value ÷ the amount at risk if called`.
3. **Substitution.** `0.0040 × 42,000,000 = 168,000` a year; `168,000 × 8.383844`. Then
   `1,408,485.78 ÷ 6,000,000`, and again against an expected draw of `6,000,000 × 0.55`.
4. **Result.** The support buys **USD 1,408,485.78** of present-value margin saving. Against the
   full cap, the breakeven call probability is **23.4748 %**. Against an expected draw of 55 % of
   the cap if a call occurs — **3,300,000** — it is **42.6814 %**.
5. **Interpretation.** The sponsors should give the undertaking if they believe the chance of it
   being called is below the breakeven, and the two breakevens — **23.47 %** and **42.68 %** — are
   the whole lesson: **the same support, the same fee saving, and a threshold that nearly doubles
   depending on how the contingent claim is shaped.** A cap is not an exposure. What matters is the
   distribution of the draw *given* a call, and a sponsor negotiating the cap while ignoring the
   likely draw is negotiating the less important parameter. Two further readings. The margin
   differential is the lenders' price for the risk, and the sponsors' own probability estimate is
   theirs; the transaction happens because the two differ, which is the risk-transfer logic of 1.2.1
   appearing on the recourse spectrum rather than in an EPC contract. And the arithmetic makes the
   *sunset* negotiable on evidence: a support that falls away at the completion test is a shorter
   claim than one running to final maturity, so a sponsor asked to extend the sunset can price the
   extension instead of arguing about it. The honest limits: 40 basis points is an indication and
   lenders may respond to withdrawn support by reducing gearing instead — in which case the
   WE 1.2.1 arithmetic applies rather than this one — and the estimate of a call probability on a
   single project is a judgment with no sample behind it, which is why the output a board should be
   given is the *breakeven*, against which its own judgment can be tested, and not a valuation.
   *Counsel pointer:* whether such an undertaking is enforceable as drafted, how it is characterised
   for accounting and disclosure, and whether it constitutes a guarantee for regulatory purposes are
   jurisdiction- and framework-specific matters for counsel and the reporting adviser.

### 1.A.3 The reviewer's foundations eye

The invariants below hold by construction. Each is one line to check, and any violated line means
a defect somewhere upstream — the foundations-level analogue of Domain 3's factor-table checks and
Domain 4's appraisal invariants (KA 4.A.5), and wired into this programme's golden-answer harness.

- **The recourse sentence has three limbs.** Who stands behind what, until when, capped at what. A
  recourse description missing a limb is incomplete, not concise; the missing limb is usually the
  cap.
- **Levered return.** `r_e = r_u + (D/E) × (r_u − r_d)`. Substitute the base case back: WE 1.2.3's
  illustration gives `12.0 % + 2.333333 × 6.0 % = 26.0000 %`. Where `r_u = r_d` the two returns
  coincide; where `r_u < r_d` gearing subtracts.
- **The equity cliff is debt service, not debt.** Equity cash reaches zero where project cash
  equals debt service — `4,200,000` interest-only, `8,349,392.06` amortising on the same
  70,000,000. Any "leverage resilience" claim quoted without the repayment shape is unverifiable.
- **Debt service reconciles to the annuity factor.** `debt ÷ AF(r, n)` must reproduce the
  instalment: `42,000,000/8.383844 = 5,009,635.23`. If it does not, the tenor, the rate or the
  convention in the table is not the one in the facility.
- **Coverage identity.** `DSCR × debt service = CFADS`, and the covenant cash follows as
  `1.20 × 5,009,635.23 = 6,011,562.28`. Reverse the reported ratio to test it, and expect the
  rounding: `1.2743 × 5,009,635.23 = 6,383,778.17` against a `CFADS` of 6,384,000, a difference of
  **221.83** that is entirely the ratio's four-decimal rounding — a larger residual than that means
  the numerator or the denominator is not the one reported. A ratio quoted without its `CFADS`
  definition is not a ratio at all (WE 1.2.2b).
- **The cash bridge's signs.** Operating cash flow = profit − Δreceivables − Δinventory
  + Δpayables. Supplier credit is a *source*, so an *increase* in payables is added while an
  increase in receivables is subtracted: if the two movements are in the same direction and appear
  in the bridge with the same sign, there is a sign error.
- **Duration is capped.** A level stream's Macaulay duration cannot exceed `(1 + r)/r` — 13.5 years
  at 8 % — however long the tenor. Deferral adds exactly the deferral; escalation adds a little; a
  quoted duration without its discount rate is not a number.
- **Breakevens are ratios of a spend to what it avoids.** Recourse: incremental cost ÷ exposure
  removed. Risk transfer: the transfer price against the cost of retention. Disclosure: cost
  avoided ÷ loss on discovery. A breakeven above 100 % means the configuration cannot pay at any
  probability — an impossible configuration, not a worthless idea (the reading Domain 13 applies to
  detection rates).
- **The triangle is conjunctive.** Value, cash and risk allocation, together. Two corners is zero,
  not two-thirds; and the cure for a failed corner is structural.
- **Every number carries a source.** Computed figures carry their derivation; asserted figures carry
  the transaction, document and date. A benchmark without a source line is the presenter's own
  assertion, whoever or whatever drafted it.

---

## Industry variations — Domain 1

The foundations flex by sector mainly in *who* the counterparties are — and, because recourse is
priced against the exposure a failure creates, each sector's counterparty shape changes the
arithmetic of WE 1.1.2 as well as the vocabulary.

- **Power and water.** The offtaker is often a state utility, so sovereign credit and political risk
  enter the stakeholder map (Domain 11) and the effective credit ceiling on the financing is usually
  the sovereign's rather than the project's. The practical consequence for this domain: the enforcement
  recovery that drives the recourse breakeven is high in a contracted availability structure and
  collapses if the offtake terminates, so the recovery assumption — not the margin — is where the
  route decision is really decided.
- **Transport.** The "offtaker" may be the travelling public, so demand risk reshapes the whole
  triangle: the cash corner becomes probabilistic rather than contracted, lenders size against a
  stressed patronage case, and gearing falls. Lower gearing means less debt to contain, which
  *raises* the recourse breakeven of WE 1.1.2 — one reason toll-road sponsors contribute more equity
  and argue harder about the ramp-up forecast than availability-payment sponsors ever do.
- **Social PPPs.** The grantor's availability payment makes government the cash engine, handback
  condition a first-order obligation, and the concession term the binding constraint on tenor — which
  matters directly for WE 1.1.4, because a fixed grantor payment with no indexation carries a shorter
  duration than an indexed stream of the same tenor, and is correspondingly harder for
  liability-driven capital to use without hedging.
- **Digital infrastructure.** Corporate credit-tenants replace state offtakers, so the credit
  analysis becomes a corporate one and refresh cycles shorten every horizon: shorter contracted terms
  mean shorter debt, which mechanically steepens the amortisation and moves the equity cliff of
  WE 1.2.3b closer, whatever the gearing.
- **Natural resources.** The market itself is the offtaker, so hedging policy joins the foundations
  and the "cash" corner of the triangle is a price-distribution rather than a contract. Enforcement
  recovery is unusually low on a single-purpose processing asset with no contracted revenue, which
  makes the exposure term large and the recourse breakeven correspondingly easy to clear — one
  sectoral reason ring-fenced structures are reached for here despite volatile cash.

The leader's first map in any new sector: who pays, under what compulsion, and what can stop them.

## Case study — Domain 1: how Kestrel chose project finance (water)

**Situation.** Kestrel's sponsor group — an international water operator at **55 %**, a regional
infrastructure fund at **35 %** and an industrial partner at **10 %** (the shares Domain 5,
KA 5.2.3 carries) — faced a **USD 60,000,000** plant with a 25-year availability offtake.
Corporate borrowing was available to the operator at attractive rates; neither of the other two
could guarantee anything beyond its equity.

**Analysis.** Corporate route: cheapest debt, fastest close — but the operator alone carries
100 % of construction and performance risk on its balance sheet, the fund cannot participate
on equal terms, and a project failure would impair the operator's whole credit. Project
route: an SPV with limited-recourse debt (completion and cost-overrun support only), 70/30
gearing — **42,000,000** of senior debt at 6.0 % over 12 years against **18,000,000** of equity —
pricier debt and eighteen months of structuring, but risk contained at the ring-fence, the
partners aligned through one shareholders' agreement, and lender diligence pressure-testing every
assumption (the discipline dividend of 1.2.1). The leverage logic of WE 1.2.3 gave the equity
story; the triangle gave the test — value (Domain 4's **+16,179,360** NPV at 8 %), cash (the
availability stream priced in Domain 3), risk (allocated through the contract matrix built
later in Domain 12).

**The numbers on the table.** WE 1.1.2 is this meeting's paper. The limited-recourse route costs
**USD 5,202,128** more in present value — **2,843,128** of margin differential over the 12 years
and **2,359,000** of close-cost premium — which is **12.3860 %** of the debt raised. Set against
the **10,073,997** of enforcement exposure the ring-fence removes in the modelled downside, the
route requires a **51.6392 %** probability of that downside to pay for itself, and nobody in the
room believed anything close to it. The board approved it anyway, and the minute records why in
three lines: without the ring-fence the fund cannot participate, so the operator would have had to
fund all **18,000,000** of equity alone; the exposure being insured is correlated with the
operator's own access to capital, so its cost in the bad state exceeds the expected shortfall used
in the calculation; and the 4.60 % corporate rate consumes borrowing capacity the operator has
already earmarked for two other projects.

**The decision.** Project finance — not because it was cheaper (it was not, by 5,202,128), but
because it made the partnership possible, contained a correlated tail rather than an expected loss,
and converted lender scrutiny into project quality. The recourse position was minuted in the
three-limbed form: the sponsors stand behind cost overruns pro rata (55/35/10), to a cap of
**6,000,000** — **14.2857 %** of senior debt — until the completion test, after which the
obligation falls away. WE 1.A.2 prices that undertaking at **1,408,486** of margin saving, which
the board accepted as good value against its own view of the call probability.

**What the domain teaches here.** The financing route is a *risk and partnership* decision
before it is a cost decision; the cheapest debt attached to the wrong recourse shape is the
expensive option. And the arithmetic's job was not to make the decision but to make its price
explicit: a board that approves 5,202,128 of incremental cost knowingly, with the three reasons
minuted, has governed the choice. A board told only that "project finance is the market standard"
has not.

## Case study B — Domain 1: the adviser with two hats (transport tender)

**Situation.** A grantor's financial adviser on a toll-road tender was discovered — after
preferred-bidder selection — to hold an advisory mandate for the winning consortium's lead
sponsor on an unrelated deal, undisclosed. The losing bidders challenged; the award was
suspended pending review.

**What happened.** The review found no evidence the evaluation was slanted — and it did not
matter. The undisclosed relationship alone failed the daylight test: the tender was re-run
with a new adviser at a cost of fourteen months and the grantor's credibility, and the
adviser's firm lost its public-sector practice in the jurisdiction. A one-line disclosure at
engagement, with barriers or a declined mandate, would have cost the firm the **250,000** fee at
risk on the second mandate, and nothing else.

**The bill.** WE 1.3.2 prices this case on the firm's own estimates. Private loss on discovery:
**900,000** of forfeited fees, **12,750,000** of jurisdictional franchise over five years and
**400,000** of legal and indemnity cost — **USD 14,050,000**. Cost imposed on parties who had no
part in the decision: **1,100,000** to re-appoint an adviser, **2,100,000** of fresh evaluation and
legal work, and **5,400,000** of abortive bid costs across three bidders — **USD 8,600,000**. Total
**USD 22,650,000** against a 250,000 saving, **90.6 times over**, and a breakeven discovery
probability of **1.7794 %** that was never remotely achievable in a tender with losing bidders
holding a reason to look.

**What the domain teaches here.** Conflicts are priced at *discovery*, not at occurrence —
and the price is paid in time, trust and franchise, not fees. Independence is an asset that
only disclosure can insure. Note also **where the 8,600,000 landed**: on the grantor's programme
and on three bidders, none of whom chose the risk. That is what distinguishes this from a
commercial gamble a firm is entitled to take with its own franchise, and it is why the duty is
unconditional rather than a matter of expected value — the arithmetic corroborates the answer, it
does not produce it.

## Executive perspective — Domain 1

What a project finance director cannot delegate in this domain:

- **The recourse position.** Exactly what the sponsor stands behind, until when, capped at
  what — the director signs this sentence personally and re-reads it before every support
  call.
- **The price of that position, stated.** The incremental cost of the chosen route, the exposure it
  removes, and the failure probability that would make the two equal (Toolkit 1.T.4) — with the
  recovery assumption and the discount rate on the face of the paper. A director who cannot produce
  those three numbers has approved a cost without seeing it; a director who treats them as the
  decision has mistaken an input for a judgment.
- **The three thresholds.** The project cash at which the covenant fails, at which equity cash
  reaches zero, and at which distributions lock up — quoted from memory, and always with the debt's
  repayment shape attached, because the interest-only version of any of them is roughly twice as
  reassuring as the truth (WE 1.2.3b).
- **The stakeholder map's honesty.** Every party's real incentive, including the
  uncomfortable ones, on one page the board has seen.
- **The cash question.** Asked in every meeting until it embarrasses no one: *will the cash
  arrive, and who is exposed if it does not?*
- **The conflicts register.** Kept current, disclosed early, tested against daylight —
  because the director's own relationships are usually the largest entries.
- **The AI accountability line.** Named humans own machine output; the director owns the
  culture that makes that real (Domain 16 gives the machinery; Domain 1 gives the law).

## Calculation exercises — Domain 1

**Exercise 1.1** Profit 3,500,000; receivables +2,200,000; inventory +900,000; payables
+600,000. Operating cash flow?
*Solution.* `3.5 − 2.2 − 0.9 + 0.6 =` **+USD 1,000,000**. Common error: sign on payables
(supplier credit is a source: +0.6, not −0.6; the wrong sign gives −0.2m and a false alarm).

**Exercise 1.2** The WE 1.2.3 project refinances to 80 % debt (80,000,000 interest-only at
6.5 %). Rebuild the base-case levered return and the cash decline at which equity income
reaches zero.
*Solution.* Debt service `80 × 0.065 = 5,200,000`; equity 20,000,000; base equity cash
`12.0 − 5.2 = 6.8` → **34.0 %**. Zero at project cash = 5,200,000 — a **56.6667 % decline**
(from 12.0). Versus the 70/30 case: eight points more base return (34 vs 26 %, on a third
less equity) bought a materially nearer cliff (−56.6667 % vs −65.0000 %). Common error: comparing
levered percentages without comparing the cliffs.

**Exercise 1.3** Classify: (a) parent guarantees debt until completion test, then released;
(b) parent comfort letter, non-binding; (c) no support, reserves funded from cash flow.
*Solution.* (a) limited recourse; (b) effectively non-recourse in law — comfort letters are
generally not enforceable guarantees (jurisdiction-specific; counsel confirms); (c)
non-recourse with structural mitigation. Common error: treating a comfort letter as
recourse — lenders price it as goodwill, not security.

**Exercise 1.4** A sponsor compares routes for a **30,000,000** facility over **10 years**:
project finance at **5.8 %** with close costs of 2,000,000, or a corporate facility at **4.5 %**
with close costs of 300,000. Cash costs are compared at 8.0 %. The modelled downside leaves
**7,500,000** of debt unrecovered after enforcement. Compute the incremental cost of the
limited-recourse route and the breakeven failure probability.
*Solution.* `AF(0.058, 10) = 7.430333` → instalment `30,000,000/7.430333 =` **4,037,504.15**;
`AF(0.045, 10) = 7.912718` → **3,791,364.65**; differential **246,139.50** a year.
`AF(0.08, 10) = 6.710081` → PV **1,651,616.08**. Close-cost premium `2,000,000 − 300,000 =
1,700,000`. Incremental cost **USD 3,351,616.08**; `p* = 3,351,616.08/7,500,000 =` **44.6882 %**.
Common error: dividing the incremental cost by the 30,000,000 facility (11.1721 %) instead of by
the exposure removed — that ratio is a cost intensity and answers a different question.

**Exercise 1.5** Take Exercise 1.2's 80/20 structure (80,000,000 of senior debt at 6.5 %,
20,000,000 of equity, project cash 12,000,000) and amortise the debt over **10 years** instead of
paying interest only. Recompute the base-case equity return, the `DSCR` and the cliff.
*Solution.* `AF(0.065, 10) = 7.188830` → instalment `80,000,000/7.188830 =` **11,128,375.20**.
Equity cash `12,000,000 − 11,128,375.20 =` **871,624.80** → **4.3581 %** on 20,000,000.
`DSCR = 12,000,000/11,128,375.20 =` **1.0783**. Equity cash reaches zero at project cash of
11,128,375.20 — a decline of only **7.2635 %**, against **56.6667 %** on the interest-only
reading. The structure is not financeable: a 1.0783 `DSCR` would not clear any commercial
coverage requirement, and a 7.26 % tolerance is inside the noise of an operating year. Common
error: reporting Exercise 1.2's 34.0 % return as the structure's economics without stating the
repayment shape — the same 80 % gearing is a strong story interest-only and an unbankable one
amortising over ten years.

**Exercise 1.6** At a discount rate of **6.0 %**, compute the Macaulay duration of a level stream
of (a) 20 payments and (b) 25 payments, and state the ceiling.
*Solution.* `D = (1 + r)/r − n/[(1 + r)ⁿ − 1]`. Ceiling `1.06/0.06 =` **17.6667 years**.
(a) `17.666667 − 20/(1.06²⁰ − 1) =` **8.6051 years**. (b) `17.666667 − 25/(1.06²⁵ − 1) =`
**10.0722 years**. Five extra years of tenor bought **1.4671** years of duration — the marginal
return on tenor falls away as the ceiling approaches. Common error: quoting a duration without its
rate; at 8 % the same 25-payment stream has a duration of **9.2254 years** and a ceiling of 13.5.

**Exercise 1.7** A project reports `CFADS` of **4,800,000** against debt service of **4,000,000**,
on annual revenue of **9,000,000**. The covenant is a `DSCR` of **1.15×**. State the `DSCR`, the
headroom in currency and in days of revenue, and the days worth 0.01× of `DSCR`.
*Solution.* `DSCR = 4,800,000/4,000,000 =` **1.2000**. Covenant cash `1.15 × 4,000,000 =
4,600,000`; headroom **200,000**, which is `200,000/9,000,000 × 365 =` **8.1111 days** of revenue.
A 0.01× movement in `DSCR` is `0.01 × 4,000,000 = 40,000` of `CFADS`, or **1.6222 days**. Common
error: computing headroom against the *reported* ratio rather than the covenant — the distance from
1.2000 to 1.15 is 0.05×, which is 200,000, not the 800,000 by which `CFADS` exceeds debt service.

## Practitioner's toolkit — Domain 1

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 1.T.1 — Stakeholder map (one page per project)

Per party: name and role · what they optimise · contract binding them (Domain 12 reference) ·
cash flows to/from the SPV · veto points and consents · relationship owner. Rule: the map is
board-visible and includes the uncomfortable incentives.

### Toolkit 1.T.2 — Financing-route decision record

Options considered (corporate / limited recourse / non-recourse / hybrid) · recourse sentence
per option (who stands behind what, until when, capped at what) · pricing and cost deltas ·
partnership and balance-sheet effects · risk containment analysis (triangle test per
option) · decision, rationale, decision-maker, date.

### Toolkit 1.T.3 — Conflicts and AI-use register

Conflicts: relationship · parties affected · disclosure date · handling (barriers/consent/
declined) · review date. AI use: tool and environment · data classification cleared ·
verification steps and named verifier · disclosure status. One register, one owner, standing
agenda item.

### Toolkit 1.T.4 — Recourse-cost worksheet (one per financing route considered)

The arithmetic of WE 1.1.2, in the order a decision paper needs it, so that the incremental cost of
a recourse position is always a stated number rather than an impression.

*Inputs.* Facility amount · tenor · indicative all-in rate on each route · close costs itemised per
route (diligence, model audit, legal, perfection, arrangement) · comparison discount rate and why ·
the downside scenario in one sentence · outstanding debt at the modelled failure date ·
enforcement recovery assumption, with its basis.

*Computed.* Instalment per route (`amount ÷ AF(r, n)`) · annual differential · PV of the
differential · close-cost premium · **incremental cost** · exposure removed · **breakeven failure
probability** · incremental cost as a share of debt raised · the same breakeven recomputed at
±10 percentage points of recovery.

*Judgment, recorded separately from the arithmetic.* What the structure buys that the exposure term
does not value — partnership capacity, correlation with the parent's own funding, borrowing
capacity preserved · the form the lenders' response to any retained risk will take (margin or
gearing — WE 1.2.1) · the recourse sentence in three limbs · decision, decision-maker, date.

*Rule.* The recovery assumption and the discount rate are named on the face of the paper. A
breakeven presented without them is not reviewable.

## Exam preparation — Domain 1

**The traps.** Recourse classifications (comfort letter ≠ guarantee — Exercise 1.3) ·
payables sign in the cash bridge (Exercise 1.1) · levered-return arithmetic that skips debt
service (MCQ 1.2-B distractor D) · reading the triangle as two-out-of-three ·
"sponsor" meaning equity investor in this book's project-finance chapters (terminology
registry) · assigning AI accountability anywhere but the signing professional · dividing a
recourse route's incremental cost by the *facility* rather than by the exposure removed
(Exercise 1.4) · quoting an equity return or a "cliff" without stating whether the debt amortises
(WE 1.2.3b, Exercise 1.5) · treating an asset's *life* as its duration, or quoting a duration
without its discount rate (WE 1.1.4, Exercise 1.6) · measuring covenant headroom from the reported
ratio instead of from the covenant (Exercise 1.7) · comparing a risk-transfer quote with the risk's
expected cost instead of with the cost of retention (WE 1.2.1) · reading a breakeven above 100 % as
an arithmetic error rather than as an impossible configuration.

**Reflection questions.**
1. Take a project you know: write its recourse sentence in under 25 words. Who stands behind
   what, until when, capped at what?
2. Which corner of the bankability triangle does your current project stress most — and what
   structural (not cosmetic) fix would close it?
3. What in your team's current AI usage would fail the daylight test if disclosed in full —
   and what changes tomorrow because you asked?
4. For your current financing: what is the incremental cost of its recourse position, what exposure
   does that position remove, and what failure probability would make the two equal? If nobody can
   answer, who approved the cost?
5. State your project's three thresholds from memory — the cash level at which the covenant fails,
   the level at which equity cash reaches zero, and the level at which the lender's coverage test
   for distributions locks up. If the second is much further away than you expected, check whether
   your mental model has the debt amortising.

## Domain 1 summary

Project finance funds single-purpose assets against their own cash, made possible by the
ring-fenced SPV and priced along the recourse spectrum; the leader's role is the financial
integrity of that machine across the whole lifecycle, under one recurring question — will
the cash arrive, and who is exposed if it does not? The recourse position on that spectrum is
computable: Kestrel's limited-recourse route costs **5,202,128** more in present value than a
corporate facility and removes **10,073,997** of enforcement exposure, so it repays itself only at a
**51.6392 %** failure probability — a breakeven that falls toward **28.2224 %** with scale and is
unachievable below about **13,702,087** of debt. That the number is high and the structure is still
correct is the domain's most professional lesson: the arithmetic prices a mean, the sponsor is
insuring a correlated tail, and the ring-fence also buys a partnership the arithmetic cannot see —
which is why it is a decision input and never the decision. The discipline's logic triangulates
value, cash and risk. Financing amplifies project quality but never substitutes for it, and its one
genuine value channel is computable too: a risk the SPV retains is paid for in the capital
structure, at **3,407,513** of present value for 4,800,000 of equity substituted for debt at a
942-basis-point spread, which is nearly four times the risk's own expected cost and the sponsor's
true reservation price for transferring it. Cash, not profit, binds — a profitable quarter can be a
cash crisis, and Kestrel's own first year absorbed **18.25 days** of revenue into working capital
leaving **11.33 days** before a 1.20× covenant fails. Leverage multiplies returns and steepens every
downside, on an identity — `r_e = r_u + (D/E) × (r_u − r_d)` — that also locates the crossover below
which gearing subtracts, and the equity cliff belongs to the debt's *shape*: the same 70 % gearing
tolerates a 65.00 % cash decline interest-only and **30.42 %** amortising, with the covenant engaging
at **16.51 %**. Timing has its own arithmetic and its own trap: a 15-year availability stream has a
Macaulay duration of **6.5945 years**, and no level stream at 8 % can exceed **13.5** at any tenor,
so matching an asset's life is not matching its duration. Around the technique stands the profession:
fiduciary-grade candour about numbers and limits, conflicts managed in daylight — where the breakeven
discovery probability is **1.7794 %** and the calculation has no upside term — and machine assistance
governed by the suite principle, whose foundational demand is that a benchmark carry a source line
before it becomes a negotiating position, the omission that cost Kestrel **1,117,200** for nothing.
Domain 2 builds the accounting the cash bridge assumed; Domain 5 takes Kestrel from concept
to bankability.
