# Domain 5 — Project Development and Bankability

> **Group:** Structuring and modelling (Domain 5 of 5 in Part Two). **Target:** ~75 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain is the home of **bankability**, the **special-purpose
> vehicle** and the development option premium. It consumes Domain 4's appraisal verdict and hands
> Domain 6 a structure to model. Coverage machinery (`DSCR`, `LLCR`, max debt capacity) is **cited
> from Domain 10, never re-derived here.** British English; USD (+SAR where useful, indicative
> `USD 1 ≈ SAR 3.75`).
> Tax, accounting and legal treatments described here are **illustrative and
> jurisdiction-specific**; none is presented as universal. In particular, whether a
> liquidated-damages rate or cap, a dilution or conversion mechanic, or a stated liability basis is
> enforceable as drafted is a matter for qualified counsel in the governing jurisdiction, and
> nothing in this domain is legal advice.

## Why this domain exists

Domain 4 established that Kestrel Water SPC is **valuable** — `NPV` +USD 16,179,360 at 8 %, `IRR`
12.19 %, `PI` 1.270. It left open the question that decides whether the project happens: **is it
financeable?** A valuable project with an unsigned offtake, an unregistered corridor, an unproven
technology or an unfundable minority sponsor does not get built, and no positive `NPV` repairs any of
those. Bankability is not a stronger form of value but a different test, and it is **conjunctive** —
every condition must pass, because lenders lending against project cash alone have no balance sheet to
fall back on and therefore no appetite for one unresolved fatal condition.

That structure is the domain's central claim, and it has three computable consequences. **The weakest
condition governs the whole:** six conditions each around 90 % likely give a joint probability of close
near 55 %, so effort on the strongest is nearly worthless while effort on the weakest is worth several
times as much (KA 5.3). **Development spend is an option premium, not a project cost:** it buys the
right to continue, is mostly spent on projects that never close, and must be judged across a portfolio
and killed early (KA 5.1) — the finance twin of the gate economics PML-AI Domain 3 (KA 3.3.1) priced for
delivery, differing only in what holding the gate costs. And **bankability is a set of contracts and
dates, not an opinion:** the sponsor group and its vehicle (KA 5.2) and the construction and readiness
package (KA 5.4) either deliver the conditions on the timetable or impose a cost this domain quantifies
to the day.

**Learning objectives.** After this domain a candidate can: place development kill gates where
irreversibility steps up; compute cost per closed project, close rate and breakeven close rate across a
development portfolio; compute a feasibility gate's net value, breakeven detection probability and the
bid-window risk at which it stops paying; explain what an SPV achieves and what its ring-fence does
not; compute per-sponsor equity, several-liability support exposure and the dilution consequence of a
funding default; value an equity bridge and show it is neutral at the bridge rate; compute a condition
set's joint probability, the marginal gain from lifting each condition and the breakeven close
probability; price a technology premium as lost debt capacity using Domain 10's sizing rule; compute
the full cost of a slip in the commercial operations date and size a performance buy-down that restores
lenders' coverage; and govern AI use across screening, document review and readiness assessment.

**The master project, at its development stage.** Kestrel Water SPC continues from Domains 1–4 and 10.
Capital cost **USD 60,000,000**, funded **70/30** as **USD 42,000,000** of senior debt at **6.0 % over
12 years** (annual instalment **USD 5,009,635.23**; `AF(0.06, 12) = 8.383844`) plus **USD 18,000,000**
of equity. Operating life **25 years**; documented first-year **`CFADS` USD 6,384,000** (6,984,000
before working-capital movements) on revenue of **12,000,000** and cash operating costs of
**4,500,000**. Coverage at close (Domain 10): `DSCR` **1.2743** = `LLCR` **1.2743**, `PLCR` **1.9431**,
covenant cash trigger **6,011,562**, annual headroom **372,438**. This domain adds three development
facts earlier chapters did not need: the **portfolio** that produced Kestrel (40 screened opportunities,
USD 14,800,000 of programme spend, two closes), the **sponsor group** behind the 18,000,000 (water
operator 55 %, infrastructure fund 35 %, industrial partner 10 %, with a several 6,000,000 overrun
support), and the **two-year construction window** whose commercial operations date the whole structure
is priced against.

---

## Knowledge Area 5.1 — Concept and feasibility

*Topics: 5.1.1 the development lifecycle and where its gates belong · 5.1.2 development spend as an
option premium · 5.1.3 the feasibility gate and the kill decision.*

### 5.1.1 The development lifecycle and where its gates belong

**Definition.** Project development is the pre-financial-close work that converts an opportunity into a
financeable transaction — identifying the payer, securing site and consents, selecting technology and
contractors, negotiating the revenue contract, structuring the financing, satisfying every condition
precedent. It runs through origination and screening, concept and prefeasibility, feasibility,
transaction and bid, and close, with spend intensity rising by roughly an order of magnitude at each
step while information improves only gradually. That asymmetry — **commitment rising faster than
knowledge** — is the economics of the whole phase, and everything before close is spent at sponsors'
risk with no financing to reimburse it.

Gates belong where **irreversibility steps up** — before feasibility spend, before bid submission,
before signature — not on the calendar; the design rule and its failure mode (gates that have never
stopped anything) are PML-AI Domain 3's, KA 3.3.1. What differs in finance is the *price of holding the
gate*: in delivery it is elapsed time against a cost of delay, while in development it is a **forgone
option** — a bid window that closes, a site optioned by a competitor, a procurement that runs without
you. The budget must also separate **pursuit costs** (expensed, belonging to the programme), **study and
adviser costs** (often capitalisable at close where the framework and the facility permit — Domain 2's
rules, jurisdiction-sensitive) and **at-risk commitments** (option premiums, bid bonds, long-lead
reservations: cash forfeited if the project dies), because without that split a kill decision cannot
answer its only question — *how much of this is already gone?*

### 5.1.2 Development spend as an option premium

**Definition.** Each stage of development spend buys a **real option**: the right, not the obligation,
to spend the next and larger tranche. It must therefore be evaluated as a **portfolio** — the closes pay
for the abandonments — and the correct unit is the **cost per closed project across everything
pursued**, not the cost of the deal that closed.

**Worked example 5.1.2 — what did Kestrel's financing actually cost to originate?**

1. **Setup.** Kestrel's sponsor group runs a four-stage water and utilities funnel. In the year of
   Kestrel's origination it **screened 40** opportunities at USD 25,000 each; **12** advanced to concept
   at USD 250,000 each; **5** entered feasibility and bid at USD 1,200,000 each; **2** were carried to
   close at a further USD 2,400,000 each. Value per close is Domain 4's Kestrel `NPV`,
   **USD 16,179,360** (cited, not re-derived).
2. **Formula.** Stage spend = count × unit cost; programme spend = Σ stage spend; cost per close =
   programme spend ÷ closes; close rate = closes ÷ screened; breakeven closes = programme spend ÷ value
   per close.
3. **Substitution.** `40 × 25,000 = 1,000,000`; `12 × 250,000 = 3,000,000`; `5 × 1,200,000 =
   6,000,000`; `2 × 2,400,000 = 4,800,000`; per close `= 14,800,000/2`; breakeven
   `= 14,800,000/16,179,360`.
4. **Result.** Programme spend **USD 14,800,000**; **cost per closed project USD 7,400,000**; close rate
   **5.0 %** (stage conversions 30.0 %, 41.67 %, 40.0 %); portfolio value **32,358,720**, net
   **+17,558,720**, value multiple **2.1864×**; **breakeven at 0.9147 closes — a breakeven close rate of
   2.29 %**.
5. **Interpretation.** The honest cost of Kestrel's financing is **7,400,000, not the 2,400,000** on its
   own charge code, and the difference is the premium paid for having found it at all; a sponsor that
   measures only the winners under-prices development, under-resources screening and is annually
   surprised by the expense line. The **breakeven close rate of 2.29 % against an achieved 5.0 %** gives
   the programme **2.19× of margin** — it could halve its hit rate and still create value — which converts
   "development is expensive" into "development stops paying below one close in 44 screenings", the
   sentence a budget is actually defended with (the exact breakeven is one close in 43.73). And
   sensitivity runs through value per close and late-stage conversion, not screening cost: the
   1,000,000 of screening is 6.8 % of programme spend and
   buys the whole funnel, while one lost late-stage project costs 2,400,000 outright — so the governance
   conclusion is counter-intuitive and consistent, **screen more widely and kill earlier**. The caution:
   value per close is a forecast (Domain 4, KA 4.3.3), so a programme justified on optimistic deal
   `NPV`s has hidden its true breakeven and must be re-tested against **realised** value.

> **Fig 5.1.1 — The development funnel as an option premium.** Four-band funnel diagram, band width
> proportional to count: Screening 40 at 25,000 each (stage spend 1,000,000; cumulative 1,000,000) ·
> Concept and prefeasibility 12 at 250,000 (3,000,000; cumulative 4,000,000) · Feasibility and bid 5 at
> 1,200,000 (6,000,000; cumulative 10,000,000) · To financial close 2 at 2,400,000 (4,800,000;
> cumulative 14,800,000), each band labelled with its conversion ratio. Footer panel: development cost
> per closed project 7,400,000; close rate 5.0 %; at 16,179,360 of value per close the programme breaks
> even at 0.9147 closes — a breakeven close rate of 2.29 %. Source: PCI original. Alt text: a narrowing
> four-stage funnel from forty screened opportunities to two financial closes, with stage and cumulative
> development spend labelled and a breakeven close rate of about two per cent.

### 5.1.3 The feasibility gate and the kill decision

**Definition.** A feasibility gate is the point at which a project must demonstrate that its **fatal
conditions** are capable of being satisfied before the transaction tranche is committed. Its function is
not to improve the project but to **stop the unfinanceable ones while stopping is cheap.** The
characteristic pathology is the reverse: a project with one unresolvable condition is carried late
because everything else about it is attractive, and dies in diligence having consumed the whole
transaction budget plus abortive adviser fees.

**Worked example 5.1.3 — the gate that pays, and the bid window that kills it.**

1. **Setup.** Of the five projects entering feasibility, the sponsor group's own post-mortem record on
   comparable pursuits puts at **40 %** the share carrying a fatal bankability condition (a pipeline
   statistic, not a market one). A conditions review costs **USD 180,000** per project and detects
   such a flaw with probability **0.75**. A flaw surviving the gate is found in diligence,
   by which point the project has spent its **2,400,000** transaction tranche plus **900,000** of
   abortive external fees — **3,300,000** wasted. The gate adds **8 weeks**, and in a competitive
   concession procurement that carries a **10 %** probability of missing the bid window, forgoing a
   project worth **16,179,360**.
2. **Formula.** The gate-net-value structure of PML-AI Domain 3 (KA 3.3.1), applied to development waste
   rather than build rework: waste without the gate `= P(flaw) × waste`; with the gate `= gate cost +
   P(flaw) × P(miss) × waste`; net value is the difference. Breakeven detection solves
   `gate + P(flaw)(1 − p) × waste = P(flaw) × waste`. Option cost of delay
   `= P(window missed) × value per close`.
3. **Substitution.** Without: `0.40 × 3,300,000`. With: `180,000 + 0.40 × 0.25 × 3,300,000`. Breakeven
   `p = 1 − (1,320,000 − 180,000)/(0.40 × 3,300,000)`. Window: `0.10 × 16,179,360`.
4. **Result.** Expected waste **1,320,000** without the gate, **510,000** with it — **gate net value
   USD 810,000 per project entering feasibility**, or **4,050,000** across five. The gate pays down to a
   **detection probability of 13.64 %**. But the bid-window option cost is **1,617,936**, so net value
   becomes **−USD 807,936**, and the **breakeven window-miss probability is 5.01 %**.
5. **Interpretation.** The same gate is worth +810,000 in a bilateral negotiation and −807,936 in a
   competitive tender, and nothing about the gate changed — only what the delay costs. That is the
   finance-specific lesson: **in development, elapsed time is priced as a lost option, not as a carrying
   cost**, and the option can be worth nine times the study fee. The **13.64 % breakeven detection rate**
   is a very low bar, which is why conditions reviews almost always pay *when they run in parallel*: a
   reviewer who can read a title register, a grid-queue position and an offtaker's credit standing inside
   the existing timetable is close to free money. The **5.01 % breakeven window-miss probability** is what
   the gate's *design* is negotiated with — run the review concurrently, stage it behind a two-week
   fatal-flaw screen, or bid conditionally; a gate whose delay risk is left unquantified will be
   abolished by the first commercial team that misses a deadline, taking the 810,000 with it. The
   caution: the 40 % flaw rate and 0.75 detection rate are **empirical claims about your own pipeline**,
   so a programme that has never recorded why its abandoned projects died cannot populate them — which
   makes the abandoned-project post-mortem the first artefact to build.

### AI in this KA

Screening is the strongest legitimate machine application here: assembling opportunity long lists,
extracting site and consent facts from public registers, tabulating comparables, first-pass filtering.
Two boundaries. **A machine must never own the kill decision** — a kill destroys an option irreversibly
and the model cannot see the strategic value the funnel exists to build; automated rejections above a
materiality line get a named human reviewer. And **a model will report a condition as satisfied when it
has found a document that mentions it**: the 40 % flaw rate of 5.1.3 is populated by exactly those
conditions — an easement that stops 200 metres short, a permit issued to a predecessor entity, an
offtaker whose credit sits in another group company. Verification runs to the primary source on every
fatal condition, sampled and signed. **AI proposes; the professional verifies, decides and remains
accountable.**

### Key terms — KA 5.1

| Term | Meaning |
|---|---|
| **Financial close** | The point at which funding documents become effective and drawdown is available. |
| **Option premium (development)** | Stage spend buying the right, not the obligation, to spend the next tranche. |
| **Development cost per close** | Programme spend ÷ closes; the honest cost of one financing. |
| **Breakeven close rate** | Programme spend ÷ (value per close × opportunities screened). |
| **At-risk commitment** | Option premiums, bid bonds, reservations — cash forfeited if the project dies. |
| **Fatal condition** | A condition whose failure ends the project regardless of everything else. |

### Sample MCQs — KA 5.1

**MCQ 5.1-A `[5.1.2 · Application]`** A programme spends 1,000,000 screening, 3,000,000 on concept,
6,000,000 on feasibility and bid and 4,800,000 carrying two projects to close. Development cost per
closed project is:
- A. USD 2,400,000
- B. USD 7,400,000 ✅
- C. USD 4,800,000
- D. USD 14,800,000

*Rationale:* `14,800,000/2 = 7,400,000`. A is the closing-stage unit cost, excluding the portfolio that
produced the winners; C is the whole closing stage undivided; D attributes the entire programme to one
project.

**MCQ 5.1-B `[5.1.2 · Analysis]`** A programme spends 14,800,000 across a funnel of 40 screened
opportunities and delivers 16,179,360 of value per close. Its breakeven close rate is closest to:
- A. 5.00 %
- B. 2.29 % ✅
- C. 45.74 %
- D. 91.47 %

*Rationale:* breakeven closes `= 14,800,000/16,179,360 = 0.9147`; `÷ 40 = 2.29 %`. A is the achieved
close rate; C divides by the 2 closes achieved rather than the 40 screened; D quotes the breakeven
closes as if it were a percentage.

**MCQ 5.1-C `[5.1.3 · Application]`** A fatal flaw is present with probability 0.40 and costs 3,300,000
if it survives to diligence. A gate costing 180,000 detects it with probability 0.75. The gate's net
value, ignoring elapsed time, is:
- A. USD 1,320,000
- B. USD 810,000 ✅
- C. USD 990,000
- D. USD 330,000

*Rationale:* `1,320,000 − [180,000 + 0.40 × 0.25 × 3,300,000] = 810,000`. A is the expected waste without
the gate; C omits the gate's 180,000 cost; D is the residual expected waste after it.

**MCQ 5.1-D `[5.1.3 · Analysis]`** A gate worth **+810,000** per project before elapsed time is counted
adds 8 weeks and so carries a 10 % chance of missing a bid window on a project worth 16,179,360. The
correct conclusion is:
- A. the gate still pays, since 810,000 is positive
- B. as designed it destroys 807,936 of value, so it should be run concurrently or staged rather than abolished ✅
- C. it should be abolished, since delay always dominates in competitive procurement
- D. bid-window risk is not a financial cost and is excluded

*Rationale:* `810,000 − 0.10 × 16,179,360 = −807,936` against a 5.01 % breakeven window-miss
probability — the design, not the review, is at fault. A ignores the option cost; C forfeits 810,000 of
detection value; D is the omission the arithmetic exists to prevent.

### Self-check — KA 5.1

1. *Why measure development spend per closed project?* — The spend buys options across a portfolio, so
   closes must repay abandonments: 7,400,000, not 2,400,000, is Kestrel's origination cost.
2. *What replaces "cost of delay" as the price of a development gate?* — The forgone option: 5.01 % of
   value per close is Kestrel's breakeven window-miss probability.
3. *Where do development gates belong?* — Where irreversibility steps up: before feasibility spend,
   before bid submission, before signature.

---

## Knowledge Area 5.2 — Sponsors and special-purpose vehicles

*Topics: 5.2.1 sponsors and what each brings · 5.2.2 the SPV and the limits of its ring-fence · 5.2.3
the shareholders' agreement: shares, support and dilution · 5.2.4 funding the equity and the equity
bridge.*

### 5.2.1 Sponsors and what each brings

**Definition.** A **sponsor** in this book's project-finance sense is an equity investor promoting the
project — distinct from PML-AI's use of the word for the accountable owner of a business case
(terminology registry). Sponsors contribute four separable things, and a group is assembled to assemble
them: **capital**, **capability** (operating, technical or construction competence lenders will
underwrite), **access** (market position, offtake relationships, host-country standing) and **credit**
(a balance sheet able to stand behind the support obligations of 5.2.3). Each archetype supplies some
and lacks others: an **industrial** sponsor brings technology and O&M capability but raises conflict
questions where it also supplies the EPC or O&M contract; a **fund** brings capital and structuring
discipline with a finite life and no operating capability; a **contractor** brings completion commitment
and the suspicion that its equity is a device to win the works; a **host-country partner** brings
consents and durability and is usually the smallest and least creditworthy holder (Case study B). The
task at group formation is to state in the shareholders' agreement **what each sponsor is relied on
for**, because that is what is tested at close — and a group whose only creditworthy member is also the
contractor will find its completion support discounted for correlation, since the party guaranteeing
completion is the party whose failure causes the claim.

### 5.2.2 The SPV and the limits of its ring-fence

**Definition.** A **special-purpose vehicle** is the ring-fenced legal entity created to own, finance and
operate a single project; Domain 1 (KA 1.1.3, Fig 1.1.2) established it as the hub of the contract
structure. It achieves *risk containment* (a failed project's creditors reach the vehicle, not the
sponsors, beyond agreed support), *security* (lenders take security over the whole of a single-purpose
entity, so enforcement delivers a working project rather than assets scattered through a group), *clean
cash* (one set of flows, so `CFADS` is measurable and the waterfall enforceable) and *credit separation*
(the project stands on its own contracts, not the weakest sponsor's rating). Its costs are extra
documentation layers, standalone audit, tax and secretarial functions, and lender restrictions on the
vehicle's own decisions.

**What it does not do**, each of which has surprised a sponsor:

- It does not survive **contractual reach-through**: completion guarantees, cost-overrun support, equity
  commitment letters, O&M performance guarantees and EPC parent guarantees each puncture the fence
  deliberately, and their aggregate is the sponsor's real exposure (5.2.3).
- It does not decide **accounting consolidation**, which turns on control and the applicable reporting
  framework — a jurisdiction-specific question for the sponsor's own auditors — nor simplify **tax**,
  since an added entity brings withholding questions and thin-capitalisation limits that can cap
  deductible interest, all for qualified tax counsel and all capable of changing after-tax cash.
- It does not remove **reputational exposure**, nor by itself deliver **bankruptcy remoteness**, which is
  engineered by restrictions in the constitutional and finance documents.

### 5.2.3 The shareholders' agreement: shares, support and dilution

**Definition.** The shareholders' agreement fixes the vehicle's economics and control: each sponsor's
**equity share**, the **funding obligation** for base equity and support commitments, the **liability
basis** for those commitments, reserved matters, transfer restrictions, deadlock resolution and the
**default and dilution mechanics** applied when a shareholder fails to fund. The most consequential term
is the liability basis: under **several** liability each sponsor owes its own share and no more, while
under **joint and several** liability each may be pursued for the whole. Lenders prefer the latter
because it converts a group of commitments into one strong one; sponsors resist it because it makes each
of them the backstop for the least creditworthy member. Most limited-recourse structures land on several
liability with **credit support** — a bank letter of credit or acceptable parent guarantee — required from
any sponsor whose own credit falls below the lenders' threshold.

**Worked example 5.2.3 — what each Kestrel sponsor has actually committed.**

1. **Setup.** Kestrel's USD 60,000,000 capital cost is funded **70/30**: USD 42,000,000 senior debt and
   **USD 18,000,000** equity, held by a **water operator at 55 %**, an **infrastructure fund at 35 %**
   and an **industrial partner at 10 %**. The facility requires a **several cost-overrun support of 10 %
   of capital cost**, subscribed pro rata.
2. **Formula.** `D/E` = debt ÷ equity. Per sponsor: equity = total equity × share; support = support pool
   × share; committed = equity + support.
3. **Substitution.** `42,000,000/18,000,000`; pool `= 60,000,000 × 0.10 = 6,000,000`;
   `18,000,000 × 0.55` and `6,000,000 × 0.55`; and so on.
4. **Result.** Gearing **2.3333 : 1** (debt 70.0 % of capex).

   | Sponsor | Share | Base equity | Several support | **Total committed** |
   |---|---|---|---|---|
   | Water operator | 55 % | 9,900,000 | 3,300,000 | **13,200,000** |
   | Infrastructure fund | 35 % | 6,300,000 | 2,100,000 | **8,400,000** |
   | Industrial partner | 10 % | 1,800,000 | 600,000 | **2,400,000** |
   | **Group** | 100 % | **18,000,000** | **6,000,000** | **24,000,000** |

   Group committed capital is **40.0 % of capital cost**, not the 30 % headline; every sponsor is
   committed **33.3 %** beyond its equity share.
5. **Interpretation.** The gap between the **30 % headline and the 40 % commitment** is where sponsor
   boards are most reliably surprised, and it matters three ways. For **capital planning**, the fund's
   board approved 6,300,000 and is exposed to 8,400,000 — a difference that belongs in its own commitment
   register, not a footnote. For **credit**, the partner's 2,400,000 is the smallest number in the table
   and the likeliest to fail: a 10 % holder is frequently the least creditworthy member, so lenders will
   require its support to be backed by a letter of credit as a condition precedent — a **timetable risk**
   outside the sponsors' control, which Case study B prices at eleven weeks. For **negotiation**, each
   point of support costs the group 600,000, so a lender's move from 10 % to 15 % is a 3,000,000 increase
   worth pricing against a coverage or tenor concession (Domain 10's four levers). The professional
   caution is the liability basis: make the support **joint and several** and
   each sponsor's worst case becomes its own equity plus the **whole** 6,000,000 pool, so the partner's
   exposure rises from 2,400,000 to **7,800,000** — a factor of **3.25** — while the operator's rises only
   from 13,200,000 to **15,900,000**, or **20.5 %**. Two words in one clause transfer most of the small
   holder's protection to the large ones, which is why lenders ask for them and why a small holder must
   not sign them lightly. It is a question for qualified counsel in the governing jurisdiction, not a
   modelling assumption.

### 5.2.4 Funding the equity, and the equity bridge

Equity may be funded **pro rata** with debt, **front-ended** (lenders' preference — sponsors' money at
risk before theirs), **back-ended** (sponsors' preference, permitted only against a firm commitment), or
through an **equity bridge loan** that funds the equity portion during construction and is repaid by the
sponsors at completion against their commitment letters.

Price the bridge on Kestrel. Construction runs **two years**; pro rata the sponsors would contribute
**9,000,000 at close** and **9,000,000 a year later**; a bridge at **5.5 %** funds both and is repaid at
t = 2. Bridge interest is `9,000,000 × (1.055² − 1) = 1,017,225` plus `9,000,000 × 0.055 = 495,000` =
**1,512,225**, so the repayment is **19,512,225**. Discounted at the bridge rate the two profiles are
**identical at USD 17,530,806** (`19,512,225/1.055²` against `9,000,000 + 9,000,000/1.055`) — a
difference of **zero**. At the sponsors' **12 %** equity requirement — an indicative sponsor hurdle used
for illustration here, not a derived cost of equity; Domain 9 (KA 9.1.3) builds Kestrel's `k_e` up from
its components — the bridge profile is worth **15,555,026** against **17,035,714**, a **saving of
USD 1,480,688**, or **8.69 %**.

The identity is the point: **an equity bridge creates no project value.** It is an arbitrage between the
bridge rate and the sponsors' required return — a 6.5-point spread over an average deferral of about
eighteen months — so the saving is **a financing return, not a project return**, and the equity `IRR`
uplift it produces is exactly the flattery Domain 4 (KA 4.1.2) warned about, since the money simply went
in later. Two further consequences: the bridge is **only available against firm commitments**, converting
an equity-timing preference into a credit-quality test the group may fail; and it **adds 1,512,225 to
project cost**, which if capitalised into senior debt consumes coverage by the mechanism 5.4.2
quantifies.

### AI in this KA

**Strong:** extracting funding obligations, liability bases, reserved matters, transfer restrictions and
default mechanics from a shareholders' agreement into a structured commitment register — the artefact
5.2.3 shows most sponsor boards lack — and reconciling it to the finance documents' conditions precedent
so no support obligation is discovered late; and maintaining per-sponsor exposure across a portfolio of
vehicles, where one sponsor's aggregate several commitments across ten SPVs is a number no single project
team can see. **Not here:** liability basis and default consequences are **legal conclusions**, and a
summary reporting "several" where the clause says otherwise misstates a balance sheet by a factor. Every
such reading is verified against the executed document by qualified counsel, with verifier and date
recorded.

### Key terms — KA 5.2

| Term | Meaning |
|---|---|
| **Sponsor (project finance)** | Equity investor promoting the project; brings capital, capability, access and credit. |
| **Special-purpose vehicle (SPV)** | The ring-fenced single-purpose entity that owns, finances and operates the project. |
| **Several liability** | Each sponsor owes only its own share; joint and several exposes each to the whole. |
| **Cost-overrun support** | A sponsor commitment to fund construction cost above budget, up to a cap. |
| **Committed capital** | Base equity + support commitments; the real exposure (40.0 % of capex for Kestrel). |
| **Equity bridge loan** | Construction-period facility funding equity, repaid by sponsors at completion. |

### Sample MCQs — KA 5.2

**MCQ 5.2-A `[5.2.3 · Application]`** Equity of 18,000,000 is split 55/35/10, with a several
cost-overrun support of 10 % of a 60,000,000 capital cost subscribed pro rata. The 35 % sponsor's total
committed capital is:
- A. USD 6,300,000
- B. USD 8,400,000 ✅
- C. USD 2,100,000
- D. USD 12,300,000

*Rationale:* `18,000,000 × 0.35 = 6,300,000` plus `6,000,000 × 0.35 = 2,100,000`. A is base equity only,
**25.0 % below** the committed figure — the omission the example corrects, and the same gap seen the other
way round as the 33.3 % that support adds to every sponsor's equity share; C is the support alone; D reads
the support as joint and several, adding the whole 6,000,000 pool.

**MCQ 5.2-B `[5.2.3 · Analysis]`** The 55 % sponsor holds 9,900,000 of an 18,000,000 equity ticket and
subscribes pro rata to a 6,000,000 cost-overrun support pool. The agreement is amended from several to
joint and several liability for that support. The 55 % sponsor's worst-case exposure becomes:
- A. unchanged at 13,200,000
- B. 15,900,000 — its own 9,900,000 of equity plus the whole 6,000,000 pool ✅
- C. 24,000,000
- D. 6,000,000

*Rationale:* the sponsor becomes pursuable for the entire support commitment: `9,900,000 + 6,000,000`, a
20.5 % rise against a 3.25× rise for the 10 % holder. A is the several answer; C is the group's total
committed capital, applicable only if equity subscriptions were also joint and several; D omits the
sponsor's own equity.

**MCQ 5.2-C `[5.2.4 · Analysis]`** An equity bridge at 5.5 % replaces pro-rata equity of 9,000,000 at
t = 0 and t = 1 with 19,512,225 at t = 2. At 5.5 % both profiles are worth 17,530,806. The correct
conclusion is:
- A. the bridge creates 1,512,225 of value
- B. it creates no value at the bridge rate; its benefit is entirely the spread between the bridge rate and the sponsors' required return ✅
- C. it destroys 1,512,225 of value
- D. it is value-neutral at every discount rate

*Rationale:* identical present values prove neutrality *at that rate*; at 12 % the saving is 1,480,688.
A treats accrued interest as a gain; C treats it as a pure cost and ignores the deferral; D generalises
the identity beyond the rate at which it holds.

**MCQ 5.2-D `[5.2.2 · Analysis]`** A sponsor states that the SPV caps its exposure at its equity
subscription. The most accurate correction is:
- A. correct — that is the purpose of the ring-fence
- B. the fence is punctured by every support obligation given, and consolidation, tax and reputational consequences are decided outside it ✅
- C. incorrect — sponsors are always liable for all project debt
- D. correct, provided the vehicle is bankruptcy-remote

*Rationale:* committed capital, not subscribed equity, is the exposure (5.2.3), and consolidation and tax
turn on control and framework. C describes full recourse, which limited-recourse structures exist to
avoid; D confuses insolvency engineering with the scope of contractual support.

### Self-check — KA 5.2

1. *State Kestrel's per-sponsor committed capital and the group total.* — 13,200,000 / 8,400,000 /
   2,400,000, totalling 24,000,000 — 40.0 % of capital cost against a 30 % headline.
2. *Name three things an SPV does not achieve.* — It does not defeat contractual support, does not decide
   accounting consolidation, and does not remove tax or reputational exposure.
3. *What is an equity bridge worth, and to whom?* — Nothing to the project; to the sponsors, 1,480,688 —
   8.69 % — the spread between the 5.5 % bridge rate and their 12 % requirement over the deferral.

---

## Knowledge Area 5.3 — The bankability test

*Topics: 5.3.1 bankability as a conjunction · 5.3.2 the revenue model and the offtake · 5.3.3 permits,
land and consents · 5.3.4 technology and its price.*

### 5.3.1 Bankability as a conjunction

**Definition.** **Bankability** is the degree to which a project's contracts, risks and cash flows
support limited-recourse financing on acceptable terms (terminology registry). It is not a score and not
a probability of success: it is the state in which **every** condition a lender requires is satisfied, on
the timetable, in documents. The structural consequence is that bankability composes
**multiplicatively**, and that one fact reorganises how a development programme is run.

**Worked example 5.3.1 — six conditions, one probability, and where the effort belongs.**

1. **Setup.** Kestrel's lenders require six conditions, each with an assessed probability of being
   satisfied on the target close timetable: **offtake / revenue model 0.92**; **permits and consents
   0.90**; **land and site tenure 0.95**; **technology (proven and guaranteed) 0.88**; **EPC wrap (fixed
   price, date certain) 0.93**; **financing market and credit approval 0.85**. Remaining development
   spend to close is **2,400,000**; value on close is **16,179,360**.
2. **Formula.** Assuming independence, joint probability `= Π pᵢ`. Marginal gain from lifting condition
   `i` from `p` to `p′` `= Π pᵢ × (p′/p − 1)`. Expected value of continuing `= joint probability × value
   − remaining spend`. Breakeven close probability `= remaining spend ÷ value`. Uniform probability
   needed for a joint target `J` over `k` conditions `= J^(1/k)`.
3. **Substitution.** `0.92 × 0.90 × 0.95 × 0.88 × 0.93 × 0.85`; weakest lift `× 0.95/0.85`; strongest
   lift `× 0.98/0.95`; continue test `0.547190 × 16,179,360 − 2,400,000`; uniform requirement
   `0.90^(1/6)`.
4. **Result.** Joint probability of close **0.5472 (54.72 %)** — against an **arithmetic mean of the six
   conditions of 90.5 %**. Lifting the **weakest** (financing, 0.85 → 0.95) gives **61.16 %**, a gain of
   **6.4375 points**; lifting the **strongest** (land, 0.95 → 0.98) gives **56.45 %**, a gain of
   **1.7280 points** — the weakest link is worth **3.7255×** more. Six conditions all at 0.98 still give
   only **88.58 %**; a **90 % joint probability requires every condition at 98.26 %**. Expected value of
   continuing `= 8,853,191 − 2,400,000 =` **+6,453,191**; the **breakeven close probability is 14.83 %**.
5. **Interpretation.** The 54.72 % changes behaviour because almost every development team reports the
   90.5 %: asked how the project is looking, they average their conditions and answer "about 90 per
   cent", wrong by 36 points. Three professional uses follow. **Resource allocation inverts:** the
   ranking of conditions by marginal gain — not by importance or by who is shouting — is the work plan,
   and 3.7× more value sits in the financing condition than in the land condition. **The continue/kill
   test separates from the condition test:** at a breakeven close probability of only **14.83 %**,
   expected value says "continue" on almost anything, which is precisely why a development programme
   cannot be governed by expected value alone and needs the *fatal condition* rule of 5.1.3. And the
   **uniform requirement of 98.26 %** explains what practitioners observe without deriving: bankable
   projects are not projects with good conditions, they are projects with **no open condition** — what
   conditions precedent (5.A.2) exist to enforce. The professional caution is independence: these
   conditions are correlated, usually positively (a supportive host government helps permits, land and
   financing together), which makes the true joint probability **higher** than the product, while
   clustered failures fatten the downside. Report the **ranking**, which is robust, and treat 54.72 % as
   a disciplined lower bound with its assumption stated — never as a forecast.

> **Fig 5.3.1 — Bankability as a conjunction.** Combined bar-and-line chart, y-axis probability 0–1, six
> categories: Offtake 0.92 · Permits 0.90 · Land 0.95 · Technology 0.88 · EPC wrap 0.93 · Financing 0.85
> (the weakest, in crimson). Bars show each condition's probability of being met on the target
> timetable; an ink line plots the running product, descending 0.92 → 0.828 → 0.7866 → 0.6922 → 0.6438 →
> **0.5472**, with a crimson dashed reference at 0.5472 labelled "joint probability of close" and a
> slate dashed reference at 0.9826 labelled "every condition would need 0.9826 for a 0.90 joint".
> Sub-caption: lifting the weakest from 0.85 to 0.95 adds 6.44 points; lifting the strongest from 0.95
> to 0.98 adds 1.73. Source: PCI original. Alt text: six probability bars around ninety per cent with a
> descending line showing their cumulative product falling to about fifty-five per cent, the weakest
> condition highlighted.

### 5.3.2 The revenue model and the offtake

**Definition.** The **revenue model** is the mechanism by which the project is paid, and it is diligenced
first because every other condition is contingent on it. The bankability question is not "how much
revenue?" but **"who is contractually obliged to pay, how much, for how long, and what can lawfully stop
them?"** Domain 7 builds the taxonomy; the development-stage test has five components.

| Component | The test | Why it is a bankability condition |
|---|---|---|
| **Volume or availability** | Does the payer pay for availability (Kestrel's structure) or for what is sold? | Merchant risk moves required coverage by 0.2× or more, and debt capacity by millions (Domain 10) |
| **Tenor** | Does contracted revenue extend beyond loan maturity with margin? | A 12-year loan against a 7-year offtake has five unfunded years; Kestrel's 25-year offtake is why its `PLCR` is 1.9431 |
| **Counterparty credit** | Can the payer pay, from what source, and is the obligation supported? | An offtake is worth the offtaker's ability to pay; **creditworthy** signature, not signature, is the condition |
| **Indexation** | Is every material cost either indexed in the tariff or fixed in a contract of matching tenor? | An unindexed O&M cost inside an indexed tariff is a slow structural margin loss (Domain 3, KA 3.3.2) |
| **Termination and change in law** | Does termination compensation repay outstanding debt in non-default scenarios? | Debt sized on that assumption but documented without it is unbankable at close, not at signature |

The recurring error sits in the third row: teams treat signature as satisfaction of the revenue
condition when the lenders' condition is a creditworthy payer. The remedy is structural — a guarantee,
letter of credit or escrow, or resizing to what the weaker credit supports — and it must be established
**before** debt is sized, because every ratio in Domain 10 assumes it.

### 5.3.3 Permits, land and consents

**Definition.** The **consent set** is the complete list of governmental and third-party permissions the
project needs to be built, to operate and to be financed; the **land and tenure set** is the complete
list of rights to occupy, cross and use land for the asset and all its corridors. Both are bankability
conditions, because security over a project that may not lawfully operate or remain where it is built is
worthless.

Two artefacts organise the work. **The consent register** lists every permission with issuing authority,
statutory basis, lead time, dependency, expiry, transferability to the SPV and attached conditions —
because a permit with unsatisfiable conditions is a refusal with a delay — and identifies which consents
drive the critical path, since consents are the development activities least susceptible to acceleration
by money. **The land register** does the same for freehold, leasehold, easements, rights of way,
wayleaves and access, and its characteristic failure is the **linear asset**: the plant sits on one
parcel, but the pipeline, cable, access road or outfall crosses many, and a single unregistered easement
over a few hundred metres can stop an otherwise complete project (Case study A).

Environmental and social consent deserves separate naming. Lenders on limited-recourse infrastructure
commonly require environmental and social risk to be assessed and managed to a defined standard, and
many financial institutions have voluntarily adopted the **Equator Principles**, a lender framework
under which participating institutions apply agreed environmental and social requirements to the
projects they finance and which in turn refers to the **IFC Performance Standards**. Whether either
applies, in what version and with what categorisation is a matter for the specific lenders and project;
both are named here for identification only, neither is reproduced or summarised as a source of
requirements, and neither body is associated with this book. The duty is to establish the applicable
requirement early, because retrofitting a stakeholder-engagement or
resettlement process to a completed design is the most expensive rework in infrastructure development.

### 5.3.4 Technology, the bankable track record and its price

**Definition.** A technology is **bankable** when lenders will lend against its performance without
recourse to sponsors — which requires operating references at comparable scale and duty, a supplier able
to stand behind performance guarantees, and an independent technical adviser's opinion the lenders
accept. **First-of-a-kind** is not a technical description but a financing category, and it has a price.

Price it on Kestrel. The proven process was sized at a target `DSCR` of **1.30×** on `CFADS` of
**6,384,000** over 12 years at 6.0 %. A variant using a novel membrane arrangement would cut operating
cost, but the technical adviser cannot supply operating references at scale and the credit committee
would require **1.45×** on unchanged cash. Using Domain 10's sizing rule (max debt service = `CFADS` ÷
target `DSCR`; max debt = that × `AF(0.06, 12) = 8.383844`): at 1.30×, `6,384,000/1.30 = 4,910,769`
supports **USD 41,171,123**; at 1.45×, `6,384,000/1.45 = 4,402,759` supports **USD 36,912,041**. The
**capacity loss is USD 4,259,082**, gearing falls from 68.6 % to **61.5 %** of capital cost, and the
4,259,082 must be found as **additional equity**.

The variant must therefore save more than the cost of 4,259,082 of equity displacing debt, and since
equity is the more expensive money the threshold is high: at a 12 % equity requirement against 6 % debt
the annual pre-tax cost of that substitution is about **255,545** (4,259,082 × 6 points of spread),
which the operating saving must beat *before* any allowance for the technology's own performance risk.
That is the arithmetic behind a maxim — **project finance is a poor place to innovate** — and two
refinements keep it honest. The coverage premium is not the only lever: an extended supplier guarantee, a
larger maintenance reserve, output insurance, or a first-loss contribution from a development-finance or
concessional lender (Domain 9, KA 9.3–9.4) can each buy the ratio back down, and pricing those against
4,259,082 *is* the negotiation. And the premium is **not permanent** — the same technology at its fourth
installation is a different credit. The caution runs the other way too: bankable is not the same as
good, and a leader who lets the lenders' comfort choose the engineering has outsourced a 25-year
lifecycle decision to a credit committee whose horizon is 12 years (Domain 8).

### AI in this KA

**Earns:** building and maintaining the consent and land registers from source documents; extracting
defined terms, tenors, indexation formulae, termination provisions and conditions precedent into a
structured condition set; and re-running the conjunction of 5.3.1 whenever an assessed probability
changes, so the marginal-gain ranking stays current instead of being computed once for a board paper.
**Must not go:** the probabilities themselves are professional judgment informed by counterparties and
counsel, and a model asked to supply them produces confident numbers with no evidential basis — a false
precision that propagates into a 54.72 % that looks computed and is invented. Nor may a model conclude
that a condition is **satisfied**: satisfaction is a legal state, evidenced by a document, verified by
counsel and recorded. Every condition in an AI-built register is traced to a primary document by a named
person, probabilities carry the assessor's name and date, and extraction is sampled against source.

### Key terms — KA 5.3

| Term | Meaning |
|---|---|
| **Bankability** | The degree to which contracts, risks and cash flows support limited-recourse financing on acceptable terms. |
| **Conjunctive test** | Composition by multiplication: every condition must pass, so the weakest governs. |
| **Marginal gain (condition)** | Joint probability × (p′/p − 1); the correct ranking of development effort. |
| **Breakeven close probability** | Remaining development spend ÷ value on close (14.83 % for Kestrel). |
| **Consent register** | Every permission with authority, lead time, dependency, expiry, transferability and conditions. |
| **Linear-asset tenure risk** | Corridor rights whose single gap can stop an otherwise complete project. |
| **First-of-a-kind premium** | The coverage, reserve or insurance cost of unproven technology, payable in equity. |

### Sample MCQs — KA 5.3

**MCQ 5.3-A `[5.3.1 · Application]`** Six bankability conditions have probabilities 0.92, 0.90, 0.95,
0.88, 0.93 and 0.85. The joint probability of close, assuming independence, is closest to:
- A. 90.5 %
- B. 54.72 % ✅
- C. 85.0 %
- D. 43.0 %

*Rationale:* the product is 0.5472. A is the arithmetic mean — the error the example corrects; C quotes
the weakest condition as though the others were certain; D sums the six shortfalls (0.57) and subtracts
the total from one, over-counting the failures by treating them as mutually exclusive.

**MCQ 5.3-B `[5.3.1 · Analysis]`** In a six-condition set whose joint probability is 0.5472, one week of
effort can lift either the financing condition from 0.85 to 0.95 or the land condition from 0.95 to 0.98.
The value-maximising choice is:
- A. land, because 0.98 is the higher absolute probability
- B. financing, which adds 6.44 points against land's 1.73 — a factor of 3.73 ✅
- C. either, since both raise one condition by a similar amount
- D. neither, because correlation makes the calculation meaningless

*Rationale:* marginal gain is the joint probability times the proportional lift: `0.5472 × (0.95/0.85 −
1) = 6.4375` points against `0.5472 × (0.98/0.95 − 1) = 1.7280`. A ranks by level rather than gain; C
ignores that a proportional lift on a low base moves the product much more; D discards a ranking
correlation does not reverse.

**MCQ 5.3-C `[5.3.4 · Application]`** A credit committee raises the target `DSCR` from 1.30× to 1.45×
for lack of operating references. On `CFADS` of 6,384,000 over 12 years at 6 % (`AF` = 8.383844), the
additional equity required is closest to:
- A. USD 4,259,082 ✅
- B. USD 6,300,000
- C. USD 508,011
- D. USD 4,910,769

*Rationale:* capacity falls from 41,171,123 to 36,912,041. B applies the 0.15 ratio increase to the
42,000,000 of debt as though ratio points were percentages of principal; C is the fall in annual debt
service; D is the maximum debt *service* at 1.30×, a per-period figure mistaken for a capital sum.

**MCQ 5.3-D `[5.3.2 · Analysis]`** A project has a signed 20-year offtake with a counterparty whose
payment obligations are unsupported and whose credit lenders assess as weak. The correct conclusion is:
- A. bankable — a signed long-tenor offtake is the strongest possible condition
- B. the revenue condition fails on counterparty credit; it is repaired by credit support or by resizing, not by the contract's length ✅
- C. bankable if the tariff is indexed
- D. unbankable permanently

*Rationale:* an offtake is worth the offtaker's ability to pay, so tenor and indexation do not cure
credit (A, C). D overstates: credit support and resizing are the standard structural remedies.

### Self-check — KA 5.3

1. *Why does averaging bankability conditions mislead?* — Conditions compose multiplicatively: six
   averaging 90.5 % give a 54.72 % joint probability, a 36-point error.
2. *What uniform per-condition probability does a 90 % joint probability require over six conditions?* —
   0.90^(1/6) = **98.26 %**, which is why bankable projects have no open condition rather than good ones.
3. *State Kestrel's first-of-a-kind premium in money.* — 1.45× instead of 1.30× costs 4,259,082 of debt
   capacity, payable as equity, at an annual substitution cost of about 255,545.

---

## Knowledge Area 5.4 — Construction and operational readiness

*Topics: 5.4.1 completion risk and the EPC wrap · 5.4.2 the cost of a slip in the commercial operations
date · 5.4.3 completion tests, performance guarantees and buy-down · 5.4.4 operational readiness.*

### 5.4.1 Completion risk and the EPC wrap

**Definition.** **Completion risk** is the risk that the project is not delivered on time, to cost and to
the performance the financing was sized on. It dominates limited-recourse structures because during
construction the project has **all** the debt and **none** of the cash flow: nothing absorbs a shock, and
every day of delay is financed.

The standard mitigation is the **EPC wrap**: one contractor takes **fixed-price, date-certain, turnkey**
responsibility for the whole works, so interface risk sits with the contractor rather than the SPV. Its
components are each a bankability condition — a fixed lump-sum price with defined change mechanics; a
date certain with **delay liquidated damages** (5.4.2); **performance guarantees** with performance
damages or **buy-down** (5.4.3); performance security sized to the residual exposure; a
defects-liability period; and a parent guarantee where the contracting entity is thinly capitalised
(Domain 12 drafts these; Domain 11 allocates the risks behind them). Whether a given damages rate, cap or
security instrument is enforceable as drafted is jurisdiction-specific and belongs to counsel, not to the
model; what this domain computes is the **cash** each provision would have to deliver to be adequate. The
wrap's price is the **wrap premium** — a contractor pricing interface and schedule risk it does not
control charges for it, and the charge is material enough that a multi-package alternative must be
compared against it on interface exposure and price together, never on price alone — and its two failure
modes are **multi-package delivery without a wrap**, where the SPV holds every interface dispute, and
**a wrap exceeding the contractor's
capacity to stand behind it**, since damages and guarantees are worth only the guarantor's balance sheet.

### 5.4.2 The cost of a slip in the commercial operations date

**Definition.** The **commercial operations date (COD)** is the contractual date from which the project is
treated as operating: revenue begins, the operating-period covenant regime starts, and construction debt
converts to term debt. A slip costs money in two distinct and frequently confused ways — **extra interest
during construction** on debt already drawn, and **forgone `CFADS`** — and the whole delay-damages
negotiation is an attempt to price them.

**Worked example 5.4.2 — Kestrel's COD slips 180 days.**

1. **Setup.** At the scheduled COD the facility is fully drawn at **42,000,000** at **6.0 %**; operating
   `CFADS` would have been **6,384,000** per year; the EPC contract carries **delay liquidated damages of
   USD 20,000 per day** capped at **10 % of the 48,000,000 EPC price**. Interest and daily rates use a
   **30/360** basis (a common convention; the applicable basis is a negotiated term, and actual/360 or
   actual/365 would shift these figures slightly). The concession's expiry was fixed at award — **27 years
   from scheduled financial close** — so a slip **shortens operations rather than extending the term.**
2. **Formula.** Daily extra interest = drawn debt × rate ÷ 360; daily forgone `CFADS` = annual `CFADS` ÷
   360; damages coverage = daily damages ÷ daily economic cost; cap-binding day = cap ÷ daily damages.
   Coverage consequence: new instalment = new debt ÷ `AF(0.06, 12)`, then `DSCR` and the covenant trigger
   per Domain 10.
3. **Substitution.** `42,000,000 × 0.06/360 = 7,000`; `6,384,000/360 = 17,733.33`; `20,000/24,733.33`;
   `4,800,000/20,000`; `43,260,000/8.383844`.
4. **Result.**

   | Item | Per day | 180 days | 360 days |
   |---|---|---|---|
   | Extra interest on drawn debt | 7,000.00 | 1,260,000 | 2,520,000 |
   | Forgone `CFADS` | 17,733.33 | 3,192,000 | 6,384,000 |
   | **Total economic cost** | **24,733.33** | **4,452,000** | **8,904,000** |
   | Delay damages at 20,000/day | 20,000.00 | 3,600,000 | 4,800,000 (capped) |
   | **Uncovered, borne by the SPV** | **4,733.33** | **852,000** | **4,104,000** |

   Damages recover **80.86 %** of the daily cost; the **cap binds at day 240**. If the 1,260,000 of extra
   interest is **capitalised**, debt becomes 43,260,000, the instalment rises to **5,159,924.29**, `DSCR`
   falls from 1.2743 to **1.2372**, the covenant cash trigger rises to **6,191,909**, and annual headroom
   falls from **372,438 to 192,090.85** — **51.6 %** of what it was. Funded as equity instead, equity rises
   to **19,260,000** and gearing moves from 70.0/30.0 to **68.6/31.4** (`D/E` 2.1807).
5. **Interpretation.** Three conclusions live in that table. **The damages rate is mis-calibrated, by a
   knowable amount.** A rate of 20,000 against a daily cost of 24,733.33 leaves equity carrying 4,733.33
   per day, and the negotiating position is not "higher damages" but "damages calibrated to interest plus
   forgone `CFADS`" — a computation the contractor can check and therefore argue about honestly. The
   commonest calibration error is to size on the **forgone `CFADS` alone**, which covers only 71.7 % of
   the cost; the omitted 7,000 per day is 28.3 % and the most certain component of all, because it accrues
   whether or not the plant would have run well. **The cap is where the structure actually breaks.**
   Below day 240 damages absorb most of the pain; beyond it every further day costs the SPV the full
   24,733.33, so a 360-day slip leaves **4,104,000** uncovered — which is why lenders test the delay
   scenario against the cap rather than the rate, and why cost-overrun support (5.2.3) and contingency
   (Domain 8, KA 8.3) exist for the tail the cap does not reach. **A construction event becomes a
   permanent operating constraint:** capitalising 1,260,000 of interest halves the covenant headroom
   Domain 10 measured, for the whole 12-year loan life, on a project otherwise exactly as forecast. The
   structural choice — capitalise into debt or fund with equity — is therefore a choice about **where the
   slip lands**, on coverage or on equity return, and it belongs to the sponsors before the event, in the
   funding documents.

   **The value view answers a different question.** The cash table is the right basis for calibrating
   damages; it is *not* the sponsors' loss. Discounting quarterly at Domain 4's 8 % (quarterly rate
   **1.942655 %**, `CFADS` 1,596,000 per quarter, concession 108 quarters from close), the operating
   stream is worth **60,150,401** at close on time and **57,491,504** if COD slips 180 days — a
   **present-value loss of 2,658,897**, *less* than the naive 3,192,000 of "six months of `CFADS`",
   because what is truly lost is the *final* half-year of a 25-year stream discounted 27 years, while the
   rest is merely deferred by two quarters. Adding the 1,260,000 of interest gives a total value loss of
   **3,918,897** — **24.2 %** of Domain 4's entire `NPV` of 16,179,360, destroyed by six months. Against
   that, 3,600,000 of damages received at the delayed COD is worth **2,969,909** at close, leaving a net
   loss of **948,988**, or **5.9 %** of the project `NPV`. That pair — 24.2 % gross, 5.9 % net — is the
   most useful single statement of what an EPC delay-damages regime is *for*.

### 5.4.3 Completion tests, performance guarantees and buy-down

**Definition.** Completion is not a date but a **test**. Financing documents distinguish **mechanical
completion** (built and safe to commission), **provisional or substantial completion** (defined
performance demonstrated, triggering COD and often conversion to term debt) and **final completion**
(punch list cleared, reliability run passed, documentation delivered). Lenders' test is usually the
strictest, and it commonly includes a **reliability run** — sustained output over a continuous period —
because a single-point test can be passed by a plant that cannot hold it. Where the plant completes but
**underperforms**, the remedy is **performance liquidated damages** or a **buy-down**: a payment
calibrated so that coverage returns to the sized level, usually applied to prepay debt.

**Worked example 5.4.3 — sizing the buy-down for a 3 % output shortfall.**

1. **Setup.** Kestrel completes at **97 %** of guaranteed output. Revenue is **12,000,000** at full
   output; cash operating costs are **4,500,000**, of which **85 % is fixed** (3,825,000) and 15 % varies
   with output (675,000). Depreciation is **2,400,000**, interest **2,520,000**, cash tax **20 %**, and
   the working-capital movement is **600,000** as in Domain 2's `CFADS` derivation. The sized `DSCR` was
   **1.2743** against debt service of **5,009,635.23**.
2. **Formula.** Revenue and variable cost scale with output, fixed cost does not; `EBITDA` = revenue −
   cash operating cost; `CFADS` = `EBITDA` − cash tax − Δworking capital (Domain 2, KA 2.3.1). At constant
   target coverage debt is proportional to `CFADS`, so buy-down = debt × (`CFADS` shortfall ÷ base
   `CFADS`).
3. **Substitution.** `12,000,000 × 0.97 = 11,640,000`; `3,825,000 + 675,000 × 0.97 = 4,479,750`;
   `EBITDA = 7,160,250`; `7,160,250 − 2,400,000 − 2,520,000 = 2,240,250`; tax `× 0.20 = 448,050`;
   `CFADS = 7,160,250 − 448,050 − 600,000`; buy-down `= 42,000,000 × (6,384,000 − 6,112,200)/6,384,000`.
4. **Result.** `EBITDA` falls to **7,160,250** — a fall of **339,750**, or **4.53 %**, on a 3 % output
   shortfall (**operating leverage 1.510×**). `CFADS` falls to **6,112,200** and `DSCR` to **1.2201**;
   covenant headroom collapses from 372,438 to **100,638**, so a 3 % shortfall consumes **73.0 %** of it.
   The buy-down restoring sized coverage is **USD 1,788,158**, reducing debt to **40,211,842**.
5. **Interpretation.** The **1.510× operating leverage** is what sponsors most often fail to carry into a
   performance-guarantee negotiation: because most of a plant's cash operating cost is fixed, a small
   output shortfall produces a proportionately larger cash shortfall, so a guarantee negotiated in
   **output percentage** must be converted into **cash** before anyone agrees it is adequate. The **73 %
   of headroom consumed by a 3 % shortfall** is the sentence for the board paper: a shortfall well inside
   most people's intuition of "close enough" leaves the project 100,638 of annual cash from a covenant
   breach, with all the lock-up consequences of Domain 10 (KA 10.4.2). And the **1,788,158 buy-down**
   demonstrates an identity worth carrying: at constant coverage **debt scales linearly with `CFADS`**, so
   the buy-down is simply debt times the proportional `CFADS` shortfall — computable in a negotiation, on
   one line, without a model. Two cautions: a buy-down restores the *lenders'* position, not the
   *sponsors'* — equity has permanently lost 271,800 of annual cash and gained only a smaller loan — and
   performance damages are **capped** like delay damages, so establish the shortfall at which the cap is
   exhausted and check the project is still bankable there.

### 5.4.4 Operational readiness

**Definition.** **Operational readiness** is the state in which the project can be operated to the
standard the revenue contract requires from the first day of the operating period. It is a bankability
condition that is systematically under-managed, because the construction contract has an owner with a
deadline and readiness usually does not. The readiness set, each item with an owner, a date and evidence:
an **operating organisation** (O&M contract with a creditworthy operator, or an in-house team recruited
and trained); **permits to operate**, distinct from permits to construct, frequently issued later and
occasionally conditional on demonstrated performance, so the consent register must separate the two;
**spares, consumables and supply** with tenor matching the offtake; **metering** on which the revenue
contract pays, calibrated and accepted by both parties, since a tariff paying on measured availability is
only as good as the meter and the dispute process behind it; **insurance transition**, construction cover
replaced by operating cover with lender endorsements in force on day one, a gap being both an uninsured
exposure and an event of default; **reporting readiness** — model updated to actuals, first covenant test
date and compliance certificate scheduled, reserve accounts funded (Domain 10, KA 10.3), because a first
covenant test failed for want of a report is a breach on a performing project; and **handover of the
record** — as-built documentation, warranties, manuals and a defect list with owners and dates.

The financing consequence is direct: the covenant regime starts at COD, so a project reaching COD
operationally unready spends its first test period generating exactly the shortfall 5.4.3 priced, with no
track record and no goodwill. Readiness therefore runs as a **separate workstream with its own gate**,
held before COD is declared — and the uncomfortable implication is accepted: **declaring COD early to
start revenue can be the most expensive decision in the project**, because it converts a construction
problem the contractor owns and damages cover into an operating problem equity owns and nothing covers.

### AI in this KA

The strong application is **readiness and completion evidence assembly**: tracking hundreds of
completion-test, permit-to-operate, insurance, spares and documentation items against owners and dates,
reconciling the punch list to defects-liability obligations, and flagging items whose evidence is missing
before COD is declared — list discipline at a scale humans do badly, and cheap to verify because each item
resolves to a document. Machine-assisted **COD forecasting** is legitimate with one non-negotiable
control: the forecast COD drives the arithmetic of 5.4.2, so a model-produced date entering a lender
report has become a representation and must carry the named person who owns it. **Where it must not go:**
declaring a completion test passed, or the project ready to operate, is a professional certification with
contractual and safety consequences — a machine may assemble the evidence and must never make the call;
and attributing a 3 % shortfall to design, construction, operation or feed conditions decides who pays
under which contract. **AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 5.4

| Term | Meaning |
|---|---|
| **Completion risk** | Risk of not delivering on time, to cost and to sized performance — all debt, no cash flow. |
| **EPC wrap** | Read here only as what makes completion risk *bankable*: one contractor answerable for the whole works, so lenders face a single counterparty. Defined and priced in Domain 12, KA 12.1. |
| **Commercial operations date (COD)** | Contractual start of operations: revenue, term debt and the operating covenant regime. |
| **Interest during construction** | Interest accruing on drawn debt before revenue; 7,000 per day for Kestrel. |
| **Delay liquidated damages** | Daily payment for late completion; calibrate to interest **plus** forgone `CFADS`. |
| **Damages cap** | The ceiling beyond which every further day is borne by the SPV (day 240 for Kestrel). |
| **Buy-down** | Payment applied to prepay debt so sized coverage is restored after a performance shortfall. |
| **Operating leverage (plant)** | `EBITDA` shortfall ÷ output shortfall; 1.510× for Kestrel at 85 % fixed cost. |
| **Operational readiness** | Capability to operate to contract standard from the first day of the operating period. |

### Sample MCQs — KA 5.4

**MCQ 5.4-A `[5.4.2 · Application]`** Debt of 42,000,000 is fully drawn at 6.0 %; annual `CFADS` would be
6,384,000; 30/360 applies. The daily economic cost of a COD slip is:
- A. USD 17,733.33
- B. USD 24,733.33 ✅
- C. USD 7,000.00
- D. USD 26,400.00

*Rationale:* `42,000,000 × 0.06/360 = 7,000` of interest plus `6,384,000/360 = 17,733.33` of forgone
`CFADS`. A is the forgone-`CFADS` side alone — the calibration error leaving 28.3 % uncovered; C is the
interest alone; D uses the pre-working-capital `CFADS` of 6,984,000 (Domain 2's other definition, 19,400
per day).

**MCQ 5.4-B `[5.4.2 · Analysis]`** The daily economic cost of a COD slip is 24,733.33. Delay damages are
20,000 per day, capped at 10 % of a 48,000,000 EPC price. For a 360-day slip the SPV bears:
- A. nothing — the damages cover the delay
- B. USD 4,104,000, because the cap binds at day 240 against an economic cost of 8,904,000 ✅
- C. USD 1,704,000
- D. USD 8,904,000

*Rationale:* cap `= 4,800,000`, binding at `4,800,000/20,000 = 240` days; cost `= 360 × 24,733.33 =
8,904,000`; uncovered `= 4,104,000`. A ignores the cap; C computes damages as though all 360 days were
payable (7,200,000) and subtracts; D omits recovery altogether.

**MCQ 5.4-C `[5.4.2 · Analysis]`** A facility of 42,000,000 over 12 years at 6 % (`AF` = 8.383844) carries
a 1.20× cash covenant against `CFADS` of 6,384,000, leaving annual headroom of 372,438. 1,260,000 of extra
construction interest is capitalised, taking debt to 43,260,000 at the same tenor and rate. The most
important consequence for the operating period is:
- A. none — the debt is repaid over the same period
- B. annual covenant headroom falls from 372,438 to 192,090.85, roughly halving it for the whole loan life ✅
- C. the loan tenor extends
- D. `CFADS` falls by 1,260,000

*Rationale:* the instalment rises to `43,260,000/8.383844 = 5,159,924.29`, `DSCR` falls to 1.2372 and the
1.20× trigger rises to 6,191,909. A ignores that debt service rises; C is a possible structural response,
not an automatic consequence; D confuses a financing cost with cash generation, which is unchanged.

**MCQ 5.4-D `[5.4.3 · Application]`** A 3 % output shortfall reduces `CFADS` from 6,384,000 to 6,112,200
against debt of 42,000,000. The buy-down restoring the originally sized coverage is:
- A. USD 271,800
- B. USD 1,788,158 ✅
- C. USD 1,260,000
- D. USD 4,259,082

*Rationale:* at constant coverage debt is proportional to `CFADS`, so the buy-down is `42,000,000 ×
271,800/6,384,000`. A is the annual `CFADS` shortfall, not the debt adjustment; C is the COD-slip
interest of 5.4.2; D is the technology premium of 5.3.4 — correct numbers in the wrong place.

### Self-check — KA 5.4

1. *What two costs make up a COD slip, and which is more often omitted?* — Extra interest on drawn debt
   (7,000 per day) and forgone `CFADS` (17,733.33 per day); the interest is omitted, and it is the more
   certain of the two.
2. *Why does a 3 % output shortfall cut `EBITDA` by 4.53 %?* — Operating leverage: 85 % of cash operating
   cost is fixed, so `EBITDA` falls faster than output (1.510×), and `CFADS` with it — by 4.26 % once tax
   and working capital are taken.
3. *Why can declaring COD early be the most expensive decision in a project?* — It converts a construction
   problem the contractor owns and damages cover into an operating problem equity owns and nothing covers,
   at the moment the covenant regime begins.

---

## Advanced topics — Domain 5

### 5.A.1 Development as a portfolio of real options, and the discipline of abandonment

**Volatility increases option value:** a project whose eventual value is highly uncertain is worth *more*
to hold than one with the same expected value and no uncertainty, because the downside can be abandoned
and the upside cannot be capped — the honest financial reason to hold some genuinely speculative pursuits,
valid only while the sponsor can still walk away cheaply. More important, **the option dies as commitment
accrues:** a bid bond posted, an equipment slot reserved, a jurisdictional reputation staked, each
converts option into obligation, so the abandonment decision belongs *before* each of them, exactly where
5.1.1 puts the gates. The pathology to name is the **sunk-cost carry**: a project retained because
10,000,000 has been spent. Spent money is relevant only to the post-mortem; the question is whether the
*remaining* spend is justified by the *remaining* probability — the 14.83 % breakeven of 5.3.1, so low
that a disciplined programme needs a second, categorical rule: **kill on a fatal condition regardless of
expected value.**

### 5.A.2 Conditions precedent: the bankability list made contractual

The conditions of KA 5.3 become **conditions precedent** (CPs) in the finance documents — the itemised,
evidenced list satisfied before first drawdown — and reading a CP schedule as the bankability test in
contractual form converts judgment into a critical path with named owners. **CPs are ordered by
dependency, not importance:** a permit requiring a land instrument, requiring a board resolution,
requiring a shareholders' agreement amendment is a four-link chain whose duration is the sum, so the close
date is set by the longest chain rather than the hardest item. **The categories behave differently:**
sponsor CPs (corporate authorisations, equity commitments, credit support) are within the group's control
and therefore forgivable in timetable terms, while third-party CPs (permits, offtaker approvals, letters
of credit from banks that are not the lenders) are not — each is the kind of timetable risk that made Case
study B's eleven weeks. And **waived CPs are not satisfied CPs:** a condition waived to permit close, or
converted into a post-close undertaking, remains open with a deadline and a consequence, and the register
must track it past close, because Domain 10's covenant regime will test it and Domain 13's close checklist
will be audited against it.

### 5.A.3 The reviewer's bankability eye

Invariants to test on any development or bankability paper. **The conjunction is computed, not averaged**,
with its independence assumption stated (5.3.1). **The weakest condition is identified and resourced** —
the marginal-gain ranking exists, is current, and matches the work plan. **Development cost is stated per
closed project across the portfolio**, with the breakeven close rate on the page (5.1.2). **Every gate's
option cost is quantified**, not just its study cost (5.1.3). **Committed capital, not subscribed equity,
is the exposure figure**, with the liability basis in the document's own words (5.2.3). **Contracted
revenue tenor exceeds loan tenor with margin**, and the offtaker's credit — not merely its signature — has
been assessed (5.3.2). **The consent and land registers cover corridors, not only the site**, and permits
to operate are separated from permits to construct (5.3.3, 5.4.4). **Delay damages are calibrated to
interest plus forgone `CFADS` on the facility's day-count convention**, with the cap-binding day stated
(5.4.2). **Performance guarantees are converted from output percentages into cash through operating
leverage**, and the buy-down is computed as debt × the proportional `CFADS` shortfall (5.4.3). **Every
capitalised construction cost is traced to its coverage consequence** — headroom after the event, not
before. And **every condition has an owner, a date, an evidence reference and a named verifier**, the
single discipline separating a bankability assessment from an opinion.

---

## Industry variations — Domain 5

- **Water and desalination (Kestrel's sector).** Availability-based offtakes with a public or utility payer
  make the revenue condition strong and move the battle to **consents, water source and discharge rights,
  and linear assets** — intake, outfall and pipeline corridors, where Case study A's tenure failure lives.
  Technology is usually proven, so the first-of-a-kind premium is rare and expensive when it appears.
- **Contracted power and renewables.** The binding conditions are **grid connection and curtailment
  terms** — a queue position is a development asset with a value and a date — and increasingly storage and
  shaping obligations that turn a simple offtake into an availability product. Portfolios are large and
  cheap per pursuit, so the funnel of 5.1.2 runs at far higher screened counts and lower close rates.
- **Transport concessions.** Land assembly and route consent dominate the timetable; **traffic and demand
  studies** substitute for the offtake and are diligenced as such; and public procurement makes the **bid
  window a hard date** — the case in which 5.1.3's option cost most often defeats a good gate.
- **Digital infrastructure.** Power availability and grid capacity are the critical consents, tenant credit
  is the offtake condition, and **short asset lives** thin the tail Domain 10's `PLCR` measures. Sponsor
  groups are frequently three-party with a thin local holder — Case study B's shape.
- **Oil, gas and mining.** Resource definition replaces the offtake as the primary condition, revenue is
  usually **merchant**, and social licence and closure obligations extend the consent set decades beyond
  the loan. Development spend per pursuit is an order of magnitude larger and close rates correspondingly
  lower, making the portfolio discipline of 5.1.2 the whole business rather than an overhead on it.
- **Social infrastructure PPPs.** Conditions are relatively standardised and the payer is usually a public
  authority, so bankability turns on **contract precedent and affordability approval** rather than
  technology; the risk is political and budgetary, and termination-compensation provisions do more of the
  credit work than anything in the physical asset.

---

## Case study — Domain 5: the easement nobody registered (water)

**Situation.** Kestrel's feasibility study cleared the plant site — a 14-hectare coastal parcel on a long
lease — and reported the **land and site tenure condition satisfied at 0.95**. The project's 4.6 km
product-water pipeline crossed eleven third-party parcels under easements granted to the host municipality
in an earlier scheme. Nine were registered. Two, covering **1.8 km through a single agricultural
holding**, existed only as a decades-old exchange of letters, and the holding had since been sold twice.
The bid was submitted, preferred-bidder status awarded, and the defect surfaced in **lender legal
diligence eleven weeks before scheduled close.**

**What happened.** The transaction stopped: lenders would not treat an unregistered corridor right as
security-worthy, and no title insurance was available on terms the credit committee would accept. Three
options existed — negotiate fresh easements with an owner who now knew the project's position, pursue a
statutory route through the municipality in an election period, or **re-route** the 1.8 km along an
existing road reserve. The sponsors re-routed, at an additional capital cost of **1,400,000** and a
**nine-month** delay to close and therefore to COD. The concession's expiry had been fixed at award, so
the operating period shortened rather than shifting.

**The arithmetic.** Discounting quarterly at 8 % (quarterly rate 1.942655 %, `CFADS` 1,596,000 per
quarter, concession 108 quarters from the original scheduled close), the operating stream was worth
**60,150,401** on the original timetable and **56,199,935** with COD nine months later — a present-value
loss of **3,950,466**, or **6.57 %** of the stream. The 3,600,000 of feasibility and transaction spend
already at risk was carried nine months at the sponsors' 12 %, costing **319,368**. With the re-route,
the defect cost **USD 5,669,834** — **35.0 %** of Domain 4's entire project `NPV`. A title-and-consents
review at the feasibility gate, of exactly the kind priced in Worked example 5.1.3, would have cost
**180,000**: the defect cost **31.5 times** the review that would have found it, and would have found it
while re-routing was a design choice rather than a crisis.

**How it resolved.** Close occurred nine months late on the original 42,000,000 / 18,000,000 structure,
the 1,400,000 re-route being absorbed by construction contingency inside the unchanged 60,000,000 capital
budget, so neither the debt nor the equity ticket moved. Lenders required as a condition precedent a
**complete registered-title schedule for every corridor parcel** with counsel's opinion. The
sponsor group changed two things permanently: the feasibility gate acquired a mandatory corridor-tenure
review for every linear asset, run **concurrently** with technical feasibility so that it adds no elapsed
time (the 5.1.3 remedy), and the bankability register stopped recording "land" as one condition, splitting
it into **site tenure** and **corridor tenure** with separate probabilities and owners.

**What the domain teaches here.** A condition assessed at 0.95 was two conditions, one of them near zero,
and the conjunction (5.3.1) could not reveal that because the register's granularity was wrong.
Bankability failures are rarely failures of judgment about a known condition; they are failures to have
decomposed a condition into the things that must independently be true. And the gate arithmetic of 5.1.3
is vindicated as such arithmetic usually is — not by the gates held, but by the 5,669,834 paid for the one
that was not.

## Case study B — Domain 5: the ten per cent that could not fund (digital infrastructure)

**Situation.** A **240,000,000** hyperscale data-centre campus was developed by a three-sponsor
consortium: an anchor operator at **55 %**, an infrastructure fund at **35 %** and a local partner at
**10 %** contributing land assembly, grid liaison and permitting. Funding was **70/30** —
**168,000,000** of senior debt and **72,000,000** of equity — with a **several cost-overrun support of
10 % of capital cost** subscribed pro rata, so committed capital was **52,800,000 / 33,600,000 /
9,600,000**, the local partner's comprising 7,200,000 of base equity and **2,400,000** of support.

**What happened.** Grid works and a mid-construction shift to higher-density cooling, to meet the anchor
tenant's revised rack plan, produced a **16,000,000** overrun. Support was called pro rata: **8,800,000**
from the operator, **5,600,000** from the fund, **1,600,000** from the local partner. The partner could
not fund; its balance sheet was committed elsewhere and its 2,400,000 support had been accepted at close
on a parent undertaking rather than a bank letter of credit. Under the shareholders' agreement the
operator funded the partner's share, credited at a **punitive 1.25× conversion**.

**The arithmetic.** Post-overrun equity became **50,400,000** (operator: 39,600,000 + 8,800,000 +
1,600,000 × 1.25), **30,800,000** (fund) and **7,200,000** (partner), totalling **88,400,000** — so shares
moved from 55/35/10 to **57.0136 % / 34.8416 / 8.1448**. The overrun bought no additional revenue, so
equity value (the present value of distributions, **96,000,000**) was unchanged by it. Had the partner
funded, it would hold 10 % of 96,000,000 = 9,600,000 having paid 1,600,000, a net **8,000,000**; having
declined, it holds 8.1448 % = **7,819,005** and kept its cash. **Declining therefore cost it 180,995 —
11.3 % of the 1,600,000 it withheld**; at a par conversion the cost would have been **145,455**. For
dilution to price the default at the full 1,600,000 the multiplier would have to be **13.50×** — far
outside the range such clauses use, and at that level a conversion whose enforceability counsel would have
to test before anyone relied on it. The conclusion is uncomfortable and general: **in a small-stake
structure, dilution cannot price a funding default.** It is a discount, not a deterrent.

**How it resolved.** The operator funded, took the dilution credit and — because dilution was inadequate —
invoked the agreement's alternative remedy, converting the shortfall into a **shareholder loan at 15 %**,
so the partner owed **1,840,000** after one year, **2,116,000** after two and **2,433,400** after three,
subordinated to senior debt and repayable from its distributions. The lenders, exposed to the support
commitment rather than to the sponsors' internal arrangements, required for the remainder of construction
that the partner's undrawn support be **backed by a bank letter of credit**; arranging it took **eleven
weeks**, during which the next drawdown was blocked — and that delay was the practical cost of the whole
episode.

**What the domain teaches here.** The 10 % holder is the structural weak point of a sponsor group (5.2.3):
smallest number, likeliest to be least creditworthy, able to stop a drawdown for a sum trivial against
the project. **Credit support belongs at close, not at default** — the eleven weeks were available at
close for a letter-of-credit fee and were paid for in full later. And **remedies must be priced, not
assumed:** a group that had computed the 180,995 against 1,600,000 before signing would have known its
dilution clause was decorative, and would have negotiated the shareholder-loan remedy, a drag-along or a
call option on the defaulting share instead.

---

## Executive perspective — Domain 5

What a project finance director cannot delegate in this domain:

- **The conjunction, and the ranking that follows.** The joint probability of close, computed and not
  averaged, with the marginal-gain ranking driving the work plan. A director who accepts "about 90 per
  cent" from six 90 per cent conditions has accepted a 36-point error (5.3.1).
- **The fatal-condition kill.** Expected value justifies continuing almost anything at a 14.83 %
  breakeven; only a categorical rule kills a project with an unresolvable condition (5.A.1).
- **The development portfolio's economics.** Cost per closed project (7,400,000, not 2,400,000), the close
  rate and the breakeven close rate — the numbers a development budget is defended with (5.1.2).
- **Committed capital and the liability basis.** What the group has actually promised (24,000,000, 40 % of
  capex, not the 30 % headline), on what basis, and whether the weakest sponsor's share is
  credit-supported *at close* (5.2.3, Case study B).
- **The damages calibration.** Whether delay and performance damages are calibrated to interest plus
  forgone `CFADS` and converted from output percentages into cash, and where each cap binds (5.4.2,
  5.4.3).
- **The COD declaration.** It is the director's, taken against evidence of operational readiness rather
  than a revenue start date, because the covenant regime begins the same day (5.4.4).

## Calculation exercises — Domain 5

**Exercise 5.1** A programme screens 60 opportunities at 30,000 each; 15 advance to concept at 200,000
each; 6 enter feasibility and bid at 900,000 each; 3 are carried to close at 1,800,000 each. Each close
delivers sponsor value of 11,000,000. Compute programme spend, cost per close, the close rate, the value
multiple and the breakeven close rate.
*Solution.* Stage spend `1,800,000 + 3,000,000 + 5,400,000 + 5,400,000 =` **15,600,000**; per close
`15,600,000/3 =` **5,200,000**; close rate `3/60 =` **5.0 %**; portfolio value **33,000,000**, net
**+17,400,000**, multiple **2.1154×**; breakeven closes `15,600,000/11,000,000 = 1.4182`, breakeven close
rate `1.4182/60 =` **2.36 %**.
*Common error:* dividing only the closing-stage spend by the closes (`5,400,000/3 = 1,800,000`), which
prices the winners and ignores the portfolio that produced them — a **65 %** understatement.

**Exercise 5.2** Five bankability conditions have probabilities 0.95, 0.90, 0.80, 0.96 and 0.92. Compute
the joint probability of close, identify the weakest, and compare the gain from lifting it to 0.92 with
the gain from lifting the 0.90 condition to 0.96.
*Solution.* Joint `= 0.95 × 0.90 × 0.80 × 0.96 × 0.92 =` **0.6041 (60.41 %)**, against an arithmetic mean
of **90.6 %**. The weakest is 0.80; lifting it to 0.92 multiplies the product by 1.15 for **69.47 %**, a
gain of **9.0616 points**. Lifting 0.90 to 0.96 multiplies by 1.0667 for **64.44 %**, a gain of **4.0274
points** — so the weakest link is worth **2.250×** more.
*Common error:* reporting the arithmetic mean (90.6 %) as the probability of close, a 30-point
overstatement; the next commonest is ranking by absolute lift (0.12 against 0.06) rather than by
proportional lift applied to the whole product.

**Exercise 5.3** A 90,000,000 project is funded with 25,000,000 of equity held 40/30/20/10, and the
facility requires a several cost-overrun support of 12 % of capital cost subscribed pro rata. State each
sponsor's committed capital, the group total as a percentage of capex, and the uplift over headline
equity.
*Solution.* Support pool `90,000,000 × 0.12 =` **10,800,000**.

| Share | Equity | Support | Committed |
|---|---|---|---|
| 40 % | 10,000,000 | 4,320,000 | **14,320,000** |
| 30 % | 7,500,000 | 3,240,000 | **10,740,000** |
| 20 % | 5,000,000 | 2,160,000 | **7,160,000** |
| 10 % | 2,500,000 | 1,080,000 | **3,580,000** |

Group committed **35,800,000**, or **39.78 %** of capital cost against a 27.8 % equity headline; the uplift
is a uniform **43.2 %** for every sponsor, because support is subscribed pro rata.
*Common error:* quoting the 10 % sponsor's exposure as its 2,500,000 of equity and omitting the 1,080,000
of several support — **30.2 % below** the 3,580,000 a lender will nonetheless call, the same gap that reads
as a 43.2 % uplift when taken the other way round.

**Exercise 5.4** Debt of 50,000,000 is fully drawn at 7.0 %; annual `CFADS` would be 8,000,000; delay
damages are 25,000 per day capped at 8 % of a 60,000,000 EPC price; 30/360 applies. For a 150-day COD slip
compute the daily economic cost, the total cost, damages recovered, the shortfall, and the cap-binding
day.
*Solution.* Daily interest `50,000,000 × 0.07/360 =` **9,722.22**; daily forgone `CFADS` `8,000,000/360 =`
**22,222.22**; total **31,944.44 per day**. Over 150 days: interest **1,458,333.33**, forgone `CFADS`
**3,333,333.33**, total **4,791,666.67**. Damages `150 × 25,000 =` **3,750,000**, leaving
**1,041,666.67**; coverage `25,000/31,944.44 =` **78.26 %**. The cap `60,000,000 × 0.08 = 4,800,000` binds
at `4,800,000/25,000 =` **192 days**.
*Common error:* calibrating the damages rate on the forgone `CFADS` alone (22,222.22 per day) and omitting
the interest, which is **30.43 %** of the daily cost and its most certain component.

**Exercise 5.5** A credit committee will size debt at a target `DSCR` of 1.30× on proven technology or
1.50× on a first-of-a-kind configuration. `CFADS` is 8,000,000; the loan runs 14 years at 6.5 %
(`AF(0.065, 14) = 9.013842`). Compute debt capacity under each and the equity consequence.
*Solution.* At 1.30×: service `8,000,000/1.30 = 6,153,846.15`, capacity `× 9.013842 =` **55,469,799**. At
1.50×: service **5,333,333.33**, capacity **48,073,826**. The capacity loss is **7,395,973**, to be found
as equity — exactly **13.33 %** of the 1.30× capacity, because at constant `CFADS`, tenor and rate the
ratio is `1 − 1.30/1.50`.
*Common error:* treating the 0.20 increase in the target ratio as a 20 % reduction in debt, which
overstates the loss by roughly 3.7 million; the reduction is 13.33 %.

## Practitioner's toolkit — Domain 5

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 5.T.1 — The bankability condition register

One row per condition, decomposed to the level at which something must independently be true (Case study
A's lesson: "land" is at least two rows — site tenure and corridor tenure). Columns: condition · what must
be true, in evidential terms · the document that will prove it · owner · target date · dependency ·
**assessed probability of satisfaction on the timetable**, with assessor and date · the corresponding
condition precedent once drafted · status and verifier. Footer, recomputed at every update: **the product
of the probabilities** (never the mean), the joint probability of close, and the conditions ranked by
**marginal gain** — the next period's work plan. Rule: a condition with no evidential test is not a
condition but a hope, and is recorded as such.

### Toolkit 5.T.2 — Sponsor commitment and support schedule

One row per sponsor. Columns: equity share · base equity commitment · each support commitment separately
(cost overrun, completion, equity commitment letter, O&M or supply guarantee, debt-service undertaking)
with its cap and its **liability basis in the words of the document** · **total committed capital** ·
sunset or release trigger for each obligation · credit support required by lenders and whether it is **in
place at close** · the default and dilution mechanic, with **the cost of declining a call computed** for a
representative call size (Case study B's 180,995 against 1,600,000). Footer: group committed capital as a
percentage of capital cost, and the identity of the weakest credit in the group. Rule: no board approves
an equity share without seeing the committed-capital column.

### Toolkit 5.T.3 — COD-slip and readiness pack

Two parts, maintained from twelve months before COD. **The slip calculator:** drawn debt × rate ÷
day-count (state the convention) = daily interest; annual `CFADS` ÷ day-count = daily forgone cash; the
sum = **daily economic cost**; the damages rate and its **coverage percentage**; the **cap and the day it
binds**; the coverage consequence of capitalising the extra interest (new instalment, `DSCR`, covenant
trigger, **headroom before and after**); and the structural decision — capitalise or fund with equity —
recorded in advance. **The readiness gate checklist**, held before COD is declared: O&M contract executed
and operator mobilised · permits **to operate** issued, listed separately from construction permits ·
initial spares and supply contracts with matching tenor · revenue metering calibrated and accepted by the
payer · operating insurance in force with lender endorsements · reserve accounts funded · model updated
to actuals, first covenant test date and compliance certificate scheduled · as-built documentation,
warranties and punch list with owners and dates. Rule: **COD is declared against this checklist, by a
named person, or it is not declared.**

## Exam preparation — Domain 5

**What is assessed.** The conjunctive nature of bankability and its arithmetic; development spend as an
option premium measured across a portfolio; gate economics adapted to development, where holding costs a
forgone option rather than a carry; the SPV's purposes and the limits of its ring-fence; per-sponsor
equity, several-liability support and dilution mechanics; the equity bridge's neutrality at the bridge
rate; the components of a bankable revenue model; the consent and tenure sets including corridors; the
price of unproven technology in lost debt capacity; and the full cost of a COD slip and a performance
shortfall, including the coverage consequence.

**The calculations to do under time pressure.** The product of a condition set and the marginal gain from
lifting one condition (5.3.1) · programme spend, cost per close and breakeven close rate (5.1.2) · gate
net value and breakeven detection probability (5.1.3) · committed capital per sponsor and the group total
as a share of capex (5.2.3) · debt capacity at two target coverage ratios using Domain 10's rule (5.3.4) ·
daily and total COD-slip cost, damages coverage and the cap-binding day (5.4.2) · the new instalment,
`DSCR` and headroom after capitalising construction interest (5.4.2) · `EBITDA` and `CFADS` under an
output shortfall with a fixed/variable split, and the buy-down that restores sized coverage (5.4.3).

**The traps.**

- Averaging condition probabilities instead of multiplying them — a 36-point error (5.3.1, MCQ 5.3-A).
- Ranking condition effort by absolute probability rather than marginal gain (5.3.1, MCQ 5.3-B).
- Quoting development cost per deal rather than per close (5.1.2, MCQ 5.1-A).
- Counting a gate's study cost but not its option cost (5.1.3, MCQ 5.1-D).
- Treating subscribed equity as sponsor exposure and omitting several support (5.2.3, MCQ 5.2-A).
- Reading several liability where the document says joint and several, or the reverse (5.2.3, MCQ 5.2-B).
- Presenting the equity bridge's saving as project value when it is neutral at the bridge rate (5.2.4,
  MCQ 5.2-C).
- Treating a signed offtake as a satisfied revenue condition without testing the offtaker's credit (5.3.2,
  MCQ 5.3-D).
- Applying a change in target `DSCR` to the principal as though ratio points were percentages (5.3.4,
  MCQ 5.3-C).
- Calibrating delay damages on the forgone `CFADS` alone and omitting interest during construction —
  28.3 % of the daily cost on Kestrel, 30.43 % in Exercise 5.4 (5.4.2, MCQ 5.4-A).
- Ignoring the damages cap, which is where the exposure actually sits (5.4.2, MCQ 5.4-B).
- Forgetting that capitalised construction interest is a permanent coverage cost (5.4.2, MCQ 5.4-C).
- Sizing a buy-down on the annual cash shortfall rather than on debt × the proportional `CFADS` shortfall
  (5.4.3, MCQ 5.4-D).

**How the domain connects.** Domain 4 supplied the value this domain tests for financeability, and
Domain 1 the recourse spectrum and SPV that make limited-recourse structures possible. Forward: Domain 6
models the structure assembled here; Domain 7 builds the revenue models 5.3.2 only classifies; Domain 8
supplies the estimate classes, contingency and delay arithmetic behind the development budget and the
overrun support; Domain 9 supplies the capital sources, including the concessional money that can buy down
a first-of-a-kind premium; Domain 10 sizes and covenants the debt these conditions permit; Domain 11
allocates the risks the conditions represent and Domain 12 documents them; Domain 13 converts the
condition register into conditions precedent and closes; Domain 14 monitors the construction whose slip
5.4.2 prices. PML-AI Domain 3 (KA 3.3.1) is the delivery twin of this domain's gate economics, and the
difference between them — elapsed time against forgone option — is the most useful single idea a finance
leader can carry between the two disciplines.

## Domain 5 summary
Bankability is a **conjunction**, and that is the domain's whole argument. Kestrel's six conditions —
offtake 0.92, permits 0.90, land 0.95, technology 0.88, EPC wrap 0.93, financing 0.85 — average **90.5 %**
and multiply to a **54.72 %** probability of close; lifting the weakest to 0.95 adds **6.4375 points**
while lifting the strongest to 0.98 adds **1.7280**, a **3.7255×** difference that reorders the whole work
plan; and a 90 % joint probability would require every condition at **98.26 %**, which is why bankable
projects are projects with no open condition rather than projects with good ones. The spend that buys
those conditions is an **option premium** measured across a portfolio: 40 screened opportunities,
USD 14,800,000 of programme spend and two closes make Kestrel's honest origination cost **7,400,000**, not
the 2,400,000 on its own charge code, against a **breakeven close rate of 2.29 %** on an achieved 5.0 %.
A feasibility gate costing 180,000 against a 40 % fatal-flaw rate and a 3,300,000 late-discovery waste is
worth **810,000 per project** and pays down to a **13.64 %** detection rate — but an 8-week delay carrying
a 10 % chance of missing a bid window costs **1,617,936**, turning the same gate **negative by 807,936**
and setting a **5.01 %** breakeven window-miss probability: in development, elapsed time is priced as a
lost option, not a carrying cost. The group behind Kestrel's 18,000,000 has committed **24,000,000 —
40.0 % of capital cost** — as 13,200,000 / 8,400,000 / 2,400,000 on a several basis, every sponsor exposed
**33.3 %** beyond its equity share and each exposed to the whole 6,000,000 pool if the basis becomes joint
and several; its equity bridge at 5.5 % is **exactly value-neutral at the bridge rate** (17,530,806 either
way), worth **1,480,688** only because the sponsors discount at 12 %. Unproven technology has a price:
**1.45× instead of 1.30× costs 4,259,082** of debt capacity, payable in equity. A slip in the commercial
operations date costs **24,733.33 per day** — 7,000 of interest on drawn debt and 17,733.33 of forgone
`CFADS` — so a 180-day slip costs **4,452,000** against damages of 3,600,000, leaves **852,000** with
equity, and if its interest is capitalised cuts covenant headroom from **372,438 to 192,090.85** for the
whole loan life, with the cap binding at **day 240** beyond which every day is the SPV's; in value terms
the same slip destroys **3,918,897**, 24.2 % of the project `NPV`, and 948,988 after damages. A 3 % output
shortfall cuts `CFADS` to **6,112,200** through **1.510×** operating leverage, consuming **73.0 %** of
covenant headroom, and is repaired by a buy-down of **1,788,158** — debt times the proportional `CFADS`
shortfall. Case study A paid **5,669,834**, 35.0 % of the project's `NPV`, for an unregistered 1.8 km
easement a 180,000 review would have found; Case study B proved a 1.25× dilution clause prices a 1,600,000
funding default at **180,995** and would need a **13.50×** multiplier to price it properly — so credit
support belongs at close, not at default. Domain 6 now models the structure this domain assembled;
Domain 10 sizes the debt its conditions permit.
