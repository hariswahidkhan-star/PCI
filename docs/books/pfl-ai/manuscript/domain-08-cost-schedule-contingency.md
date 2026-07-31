# Domain 8 — Cost, Schedule and Contingency Integration *(quantitative — the project-controls bridge)*

> **Group:** Structuring and modelling (Domain 4 of 5 in Part Two). **Target:** ~76 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This is the **bridge domain between the two books**: it consumes
> PML-AI Domain 7's earned-value machinery (`BAC`, `CPI`, `SPI`, the `EAC` family) and PML-AI
> Domain 8's `EMV` and confidence-level arithmetic, and converts both into the two questions a
> financing asks that a project control account never does — *is the remaining funding
> sufficient, and what does the coverage ratio do?* It uses Domain 3's compounding and
> `AF(r, n)`, Domain 4's `EAV`, Domain 5's completion-risk and delay-damages structure, Domain
> 6's sources-and-uses and capitalised-interest arithmetic, and Domain 10's `DSCR` machinery.
> British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).
> Tax, accounting and legal treatments described here are **illustrative and
> jurisdiction-specific**; none is presented as universal. In particular, whether a
> delay-damages rate, cap or stepped structure, a contingency-recalculation clause or a handback
> obligation is enforceable as drafted is a matter for qualified counsel in the governing
> jurisdiction, and whether a cost is capitalised into the depreciable base is a matter for the
> sponsor's own auditors under the applicable framework. Nothing in this domain is legal,
> tax or accounting advice. Kestrel Water SPC, Project Auriga and both case studies are
> illustrative constructs, not accounts of identifiable projects or organisations.

## Why this domain exists

Domain 6 built a model in which the balancing line was contingency, and reported it: **USD
3,645,403**, 7.59 % of the EPC price, "inside the band a lender would expect" (KA 6.2.1). It did
not say where that band comes from, whether 7.59 % is generous or reckless, or what would happen
if the number were wrong. Domain 5 priced a slip in the commercial operations date but only *at*
COD, where remaining spend is zero. Neither domain touched the fact that a project under
construction produces a monthly cost and schedule report — earned value, forecasts, variances —
that a lender receives, reads, and acts on differently from the project controller who wrote it.

This domain closes all three gaps, and its central claim is a single sentence: **a contingency
percentage is meaningless without the estimate class it was struck against and the basis it was
sized on.** Everything else here is that claim worked out. A cost estimate carries an accuracy
range that narrows as scope definition matures, and the contingency implied by that range at a
stated confidence is computable (KA 8.1). A schedule converts a cost estimate into a spend
profile, and the *shape* of that profile prices two things in opposite directions — capitalised
interest and escalation — with a breakeven between them (KA 8.2). Contingency sized from a
quantified risk register states what confidence it buys; contingency sized as a percentage of
base cost does not, and is wrong in ways this domain computes exactly (KA 8.3). And when the
build slips, the cost of a month is three components, not one, and its consequence lands on a
coverage ratio that will be tested for the next twelve years (KA 8.4).

The professional discipline the domain teaches is the discipline of the *interface*. A project
controller and a lender's technical adviser read the same monthly report and extract different
facts from it. The controller asks what the project will cost; the lender asks whether the money
still in the facility will finish it. Those are not the same question, and a finance leader who
cannot translate between them is dependent on whoever can.

**Learning objectives.** After this domain a candidate can: separate the four cost families a
financing must fund and place each in the sources-and-uses statement or the operating model;
state an estimate's accuracy class and derive the contingency its range implies at a nominated
confidence; explain why a fixed-price wrap changes the estimate class and therefore the
defensible contingency; compute a whole-life maintenance charge and size the reserve that
converts it from a covenant cliff into a distribution reduction; build a schedule-driven spend
profile and compute the capitalised interest it generates using the area rule; compute the
escalation the same profile generates and find the breakeven escalation rate at which the two
effects cancel; size contingency from a quantified register to a stated confidence level and
reconcile it against the estimate range; demonstrate in figures why a percentage-of-base
contingency fails; convert an earned-value forecast into a cost to complete and run the funds
sufficiency test a lender runs; price a month of construction slip as escalation plus extra
interest plus deferred revenue and compute its permanent `DSCR` consequence; and govern
AI-assisted estimating, simulation and forecasting.

**The master thread.** Kestrel Water SPC continues. Its **USD 60,000,000** envelope is funded
70/30 — **42,000,000** senior debt at **6.0 %** over 12 years (annual instalment **5,009,635.23**)
and **18,000,000** equity — against an EPC contract price of **48,000,000**, owner's costs and
land of 3,600,000, capitalised development costs of 1,800,000, fees of 840,000, capitalised
interest of **2,114,597** and the balancing contingency of **3,645,403** (Domain 6, KA 6.2.1).
Construction runs **eight quarters** on the certified spend profile **6, 9, 13, 16, 17, 15, 13,
11 per cent**, with cumulative debt drawn reaching 42,000,000 exactly at COD. Operating `CFADS`
is **6,384,000** and the base-case `DSCR` **1.2743** against a 1.20× covenant, giving annual
headroom of **372,438** (Domain 10, KA 10.2.1). Construction cost escalation is assumed at
**3.6 % per annum** on unwrapped scope. From PML-AI: **Project Auriga**, `BAC` **4,000,000**,
`CPI` **0.905660** and `SPI` **0.923077** at the week-13 data date, with the `EAC` family
**4,200,000 / 4,416,667 / 4,608,056** (PML-AI D7, KA 7.3.3) and a sanction-date risk register
whose mean exposure is **278,000**, standard deviation **252,642** and P80 **490,624** (PML-AI
D8, KA 8.2.4). This domain does not re-derive any of those figures. It uses them.

---

## Knowledge Area 8.1 — Development, capital, operating and lifecycle cost; estimate classes

*Topics: 8.1.1 the four cost families and where each lands · 8.1.2 estimate classes and the
contingency a range implies · 8.1.3 lifecycle cost, major maintenance and the reserve that
smooths it.*

### 8.1.1 The four cost families and where each lands

**Definition.** A financing must fund four distinct families of cost, and each enters the
structure in a different place, on a different basis, under different control.

**Development cost** is everything spent between concept and financial close: studies, surveys,
legal and advisory fees, permit applications, land options, the sponsors' own staff time. It is
spent at risk, before any facility exists, and its treatment at close is a negotiation — Kestrel
capitalised 1,800,000 of it into the funding envelope (Domain 6), which means the equity that
funded it is reimbursed and — subject to the accounting and tax framework that applies, which is a
question for the sponsor's own auditors (Domain 2) — the amount enters the depreciable base.
Development cost that is *not* accepted into the envelope is sunk equity, and Domain 5 (KA 5.1.2) priced it per close
rather than per deal for exactly that reason.

**Capital cost** is the asset: the EPC contract price, owner-supplied equipment, owner's costs,
land, connection charges, insurance during construction. It is what the estimate classes of
8.1.2 measure, and it is the base against which contingency percentages are quoted.

**Operating cost** is the recurring cash cost of running the asset — labour, energy, chemicals,
routine maintenance, insurance, administration, the operator's fee. It never appears in the
sources-and-uses statement; it appears as a deduction inside `CFADS`, which makes it a coverage
driver rather than a funding requirement. The distinction has teeth: an error of 500,000 per year
in operating cost is an error of 500,000 per year in `CFADS`, which at Kestrel's 1.20× covenant
is 1.34 times the entire annual headroom of 372,438.

**Lifecycle cost** is the periodic, non-routine expenditure that keeps the asset capable of
performing for its whole concession — membrane replacement, major overhauls, control-system
refresh, and at the end handback or decommissioning. It is lumpy, it falls in specific years, and
it is the family most often omitted from a first-pass model because it is invisible in year one
(KA 8.1.3).

**The professional point.** These four families are not four lines of one budget; they are four
different financial objects. Development cost is at-risk equity that may or may not be
reimbursed. Capital cost is funded, drawn and capitalised. Operating cost reduces `CFADS` every
period. Lifecycle cost creates a reserve obligation. A cost report that presents them as one
"project cost" figure has destroyed the information a financing needs.

### 8.1.2 Estimate classes and the contingency a range implies

**Definition.** An **estimate class** records how well the scope was defined when the estimate
was made, and carries with it a stated **accuracy range** — the band within which the estimate's
author expects the outturn to fall. The range is not a confidence interval in the statistical
sense; it is an expression of definitional maturity, and it narrows as engineering, site
investigation, procurement and contracting progress. Professional estimating bodies and large owner
organisations publish classification frameworks of their own; the principle is common practice, the
specific bands are always organisation-specific, and the table below is the PCI illustrative
ladder used throughout this book — it reproduces no other body's classification and should not be
read as one.

| Stage | Scope definition | Typical basis | Accuracy range | Implied contingency to the upper bound on a 48,000,000 base |
|---|---|---|---|---|
| **A — screening** | Concept only; capacity and location | Capacity-factored from analogues | −30 % / +50 % | 24,000,000 |
| **B — concept** | Process selected; block layout | Factored equipment costs | −20 % / +40 % | 19,200,000 |
| **C — feasibility** | Basic engineering; major equipment listed | Semi-detailed, some quotations | −15 % / +30 % | 14,400,000 |
| **D — definition** | Detailed engineering substantially complete | Quotations for most packages | −10 % / +18 % | 8,640,000 |
| **E — control** | Contract awarded; scope fixed | Contract prices, priced bill | −5 % / +8 % | 3,840,000 |

Read the last column before reading anything else. **The same 48,000,000 base estimate implies a
contingency of 24,000,000 or 3,840,000 depending only on how well the scope was known when it was
priced** — a factor of 6.25. Any conversation about "how much contingency is normal" that has not
first established the estimate class is a conversation about nothing.

**Worked example 8.1.2 — is Kestrel's 7.59 % contingency defensible?**

1. **Setup.** Domain 6's balancing line gave contingency of **USD 3,645,403** on a base of
   **48,000,000**. Establish the percentage, identify which estimate class that percentage is a
   provision for, and test whether Kestrel's contracting structure justifies it.
2. **Formula.** Contingency as a percentage of base = contingency ÷ base. Implied class = the
   ladder row whose upper bound is nearest above that percentage. Coverage of the class band =
   contingency ÷ (base × upper-bound percentage).
3. **Substitution.** `3,645,403 / 48,000,000`; compare with the upper bounds 8 %, 18 %, 30 %,
   40 %, 50 %; `3,645,403 / (48,000,000 × 0.08)`.
4. **Result.** Contingency is **7.59 %** of base. That is a **Stage E** provision — it sits inside
   the −5 %/+8 % control-estimate band and covers **94.93 %** of it, falling **194,597** short of the
   full +8 % position of 3,840,000. Against a Stage C feasibility estimate the same money covers
   only **25.32 %** of the +30 % band; the Stage C provision would be **14,400,000**, or
   **3.950 times** what is funded.
5. **Interpretation.** The 7.59 % is defensible, and the reason it is defensible is **not** that
   7.59 % is a normal number. It is that Kestrel let a **fixed-price, date-certain EPC contract
   with a full wrap** (Domain 5, KA 5.4.1) *before* the envelope was fixed, which converted a
   Stage C estimate into a Stage E position. The wrap is what makes a thin contingency honest: it
   transfers the base-estimate uncertainty — the quantity growth, the productivity assumption, the
   unpriced package — to a contractor who has accepted a price. What remains with the owner is a
   short list of retained risks (KA 8.3), which is why the owner's provision can be small. Reverse
   the contracting decision and the arithmetic reverses with it. **The same project, procured as
   six packages against a Stage C estimate with owner-managed interfaces, needs a provision of the
   order of 14,400,000 rather than 3,645,403** — and a 60,000,000 envelope does not fund it. The
   professional caution is therefore that the contingency line and the contracting strategy are
   one decision taken twice, and the two halves are usually taken by different people in different
   months. Where a lender sees a thin contingency and a weak wrap together, it is looking at a
   structure whose funding assumption has already failed; where it sees a thin contingency behind a
   full wrap, it is looking at a risk transfer it can price. **The breakeven question to ask of
   any contingency is: which estimate class does this percentage belong to, and did we buy the
   contract that earns it?**

**Common pitfall — the contingency inside the contract price.** A contractor's price contains the
contractor's own contingency, which the owner cannot see and does not control. Adding an owner's
contingency on top is correct — they cover different risks — but describing the total as "the
project's contingency" is not, because only one of the two is available to the owner on a draw
request (KA 8.4.1). Where a cost report shows a single contingency figure, ask whose it is.

### 8.1.3 Lifecycle cost, major maintenance and the reserve that smooths it

**Definition.** **Lifecycle cost** (also whole-life cost) is the total cost of ownership across
the asset's economic life: capital cost, operating cost, periodic major maintenance, and terminal
handback or decommissioning obligations. In a financing its distinctive property is *timing*: it
falls in a few specific years, in amounts large relative to a single year's cash, which is exactly
the profile a coverage covenant handles worst.

**Worked example 8.1.3 — Kestrel's whole-life charge, and the year-seven cliff.**

1. **Setup.** Kestrel's 25-year operating life requires **membrane replacement in years 7, 14 and
   21** at **USD 3,200,000** each at base-date prices, and **high-lift pump refurbishment in years
   12 and 24** at **USD 1,400,000** each. Costs escalate at **3.6 %** per annum; the sponsor's
   appraisal rate is **8.0 %** (Domain 4). Operating `CFADS` of 6,384,000 is struck **before**
   major-maintenance funding; annual debt service is **5,009,635.23**. Compute the present value of
   the lifecycle programme, the level annual charge equivalent to it, and what happens in year
   seven with and without a funded maintenance reserve.
2. **Formula.** Nominal cost at year `t` = real cost × (1 + 0.036)ᵗ; present value = nominal ÷
   (1 + 0.08)ᵗ (Domain 3, `DF(t)`); level annual charge = PV ÷ `AF(0.08, 25)` (Domain 4's `EAV`);
   year-seven `DSCR` without a reserve = (`CFADS` − nominal cost) ÷ debt service (Domain 10).
3. **Substitution.**

   | Year | Item | Real (USD) | Nominal (USD) | Present value (USD) |
   |---|---|---|---|---|
   | 7 | Membrane replacement | 3,200,000 | 4,098,909 | 2,391,674 |
   | 12 | Pump refurbishment | 1,400,000 | 2,140,154 | 849,885 |
   | 14 | Membrane replacement | 3,200,000 | 5,250,329 | 1,787,533 |
   | 21 | Membrane replacement | 3,200,000 | 6,725,194 | 1,335,999 |
   | 24 | Pump refurbishment | 1,400,000 | 3,271,615 | 515,931 |
   | | **Total** | **12,400,000** | **21,486,202** | **6,881,021** |

   `AF(0.08, 25) = 10.674776`; level charge `= 6,881,021 / 10.674776`.
4. **Result.** Present value of the lifecycle programme **USD 6,881,021**; **level annual charge
   USD 644,606**, which is **10.10 %** of `CFADS`. Without a reserve, year seven's `CFADS` falls to
   `6,384,000 − 4,098,909 =` **2,285,091** and the `DSCR` to **0.4561**. With the charge funded
   into a maintenance reserve account below `CFADS` and above distributions (Domain 10, KA 10.3.3),
   `CFADS` and therefore the `DSCR` stay at **1.2743** in every year, and the cost lands on
   distributions instead.
5. **Interpretation.** A `DSCR` of 0.4561 is not a covenant breach; it is a **payment default** —
   the project cannot pay debt service from cash in the year it replaces its membranes, having been
   entirely healthy in the six years before and the six years after. That is the whole case for a
   maintenance reserve in one number, and it explains why lenders commonly require one on an asset
   with a material overhaul cycle and why they generally require the reserve to be
   **forward-looking**: funded
   against the *next* overhaul, not accumulated at a policy percentage. Three professional
   qualifications belong beside the arithmetic. **The level charge is not the reserve deposit
   schedule.** 644,606 per year is the economically equivalent annual cost; the actual deposit
   schedule must have the money in the account before year seven, which with only six years of
   deposits means a higher early contribution or an opening balance funded at close — and the
   difference is a real funding requirement that belongs in the sources and uses. **The escalation
   assumption is load-bearing.** At 3.6 % the year-21 membrane costs 6,725,194 against 3,200,000
   today; assume 2.0 % instead and the whole-life PV falls to a materially smaller number, which is
   why the lifecycle escalation rate must be the same rate used for operating cost and stated once
   in the assumption register (Domain 6, KA 6.1). **And the reserve converts a covenant event into
   a distribution event.** That is the trade: 644,606 per year of distributions forgone buys the
   removal of a payment default. Sponsors who resist it are choosing to hold a 0.4561 `DSCR` in
   year seven, which no lender will accept and no board should.

**Handback and decommissioning.** Where a concession requires the asset to be returned in a
specified condition, the handback obligation is a lifecycle cost with a legal deadline, usually
secured by a dedicated reserve funded in the final years. Where the asset must instead be removed,
the decommissioning provision is both an accounting matter (Domain 2, KA 2.3.4) and a cash matter,
and its cash timing sits *after* the loan matures — which is why it reduces the tail that Domain
10's `PLCR` of 1.9431 measures without appearing in any coverage ratio the lender tests. A
reviewer's habit worth forming: read the `PLCR` and then ask what the project owes at the end of
the life the `PLCR` counts.

### AI in this KA

Estimate benchmarking is genuinely good machine work — comparing a new estimate's unit rates,
quantities and factored allowances against a library of prior estimates and outturns, and flagging
lines outside the historical distribution — and so is lifecycle-schedule construction from equipment
documentation into a dated, costable programme. Both are high-volume comparison tasks with
verifiable outputs. What an assistant must **not** do is *assign the estimate class*, because the
class is a statement about how well the scope is defined and a model reading the estimate cannot know
what engineering has not been done. The characteristic failure is confident class inflation: an
estimate presented in the *format* of a control estimate is classified as one, and a 30 % range is
reported as an 8 % range — a misclassification worth 10,560,000 of contingency on Kestrel's base that
nothing downstream will catch.

Verification is therefore three specific requirements. The class assignment carries a named human
owner and a one-line justification referencing the engineering deliverables actually complete. The
benchmark comparison is re-run on a held-out sample of the organisation's own outturns, with both
flagged and unflagged items reviewed — a benchmark that flags nothing is not evidence of a good
estimate. And the lifecycle programme is reconciled to the supplier's written maintenance schedule
item by item, because a missed overhaul is invisible until the year it happens. **AI proposes; the
professional verifies, decides and remains accountable.**

### Key terms — KA 8.1

| Term | Meaning |
|---|---|
| **Development cost** | Pre-close spend at risk; capitalised into the envelope only if accepted. |
| **Capital cost** | The asset: contract price, owner's costs, land, connection, insurance in construction. |
| **Operating cost** | Recurring cash cost; a deduction inside `CFADS`, never a funding line. |
| **Lifecycle (whole-life) cost** | Periodic major maintenance plus handback or decommissioning. |
| **Estimate class** | The definitional maturity of an estimate, carrying a stated accuracy range. |
| **Accuracy range** | The band the estimator expects the outturn to fall in; narrows with definition. |
| **Level annual charge** | Lumpy lifecycle PV converted to a level equivalent via `AF(r, n)` (Domain 4's `EAV`). |
| **Maintenance reserve (MRA)** | Funded account that converts a lifecycle spike into a distribution reduction. |

### Sample MCQs — KA 8.1

**MCQ 8.1-A `[8.1.2 · Application]`** A 48,000,000 base estimate carries a funded contingency of
3,645,403. Which statement is defensible?
- A. 7.59 % is a normal contingency, so the provision is adequate
- B. 7.59 % is a Stage E (control-estimate) provision, adequate only because a fixed-price wrap moved the estimate to that class ✅
- C. 7.59 % is adequate for any estimate class, since contingency is a policy percentage
- D. the provision is inadequate at every class, because contingency should be 10 %

*Rationale:* 3,645,403/48,000,000 = 7.59 %, which sits inside the −5/+8 control band and covers
94.93 % of it; the same money covers 25.32 % of a Stage C +30 % band. A appeals to a norm that does
not exist; C is the error the whole KA exists to remove; D substitutes a different unreasoned
percentage for the first one.

**MCQ 8.1-B `[8.1.2 · Analysis]`** A project is procured as six separate packages against a Stage C
feasibility estimate of 48,000,000 carrying a stated accuracy range of −15 % / +30 %, with the owner
managing interfaces. The contingency implied by the estimate's own upper bound is:
- A. 3,840,000
- B. 8,640,000
- C. 14,400,000 ✅
- D. 24,000,000

*Rationale:* Stage C's +30 % upper bound on 48,000,000 is 14,400,000. A is the Stage E provision
(the answer only a full wrap earns); B is Stage D's +18 %; D is Stage A's +50 %.

**MCQ 8.1-C `[8.1.3 · Application]`** Membranes costing 3,200,000 at base-date prices are replaced
in year 7; escalation is 3.6 % per annum. `CFADS` is 6,384,000 and debt service 5,009,635.23. With
no maintenance reserve, the year-seven `DSCR` is closest to:
- A. 1.2743
- B. 0.4561 ✅
- C. 0.6356
- D. 1.1457

*Rationale:* Nominal cost `3,200,000 × 1.036⁷ = 4,098,909`; `(6,384,000 − 4,098,909)/5,009,635.23 =
0.4561`. A is the ratio with a funded reserve; C forgets to escalate (using 3,200,000, giving
3,184,000/5,009,635.23 = 0.6356); D deducts only the level annual charge of 644,606 rather than the
actual spend (`5,739,394/5,009,635.23 = 1.1457`).

**MCQ 8.1-D `[8.1.1 · Analysis]`** An operating-cost forecast understates annual cost by 500,000.
Against Kestrel's 1.20× covenant with annual headroom of 372,438, the consequence is:
- A. a funding shortfall in the sources-and-uses statement
- B. no consequence, since operating cost is not a funded use
- C. a covenant breach — the error is 1.34 times the entire annual headroom, and it recurs every year ✅
- D. a one-off reduction in the contingency line

*Rationale:* Operating cost is a deduction inside `CFADS`, so a 500,000 understatement removes
500,000 of `CFADS` annually against 372,438 of headroom (500,000/372,438 = 1.34). A and D put an
operating error in a capital line; B confuses "not funded" with "no effect".

**MCQ 8.1-E `[8.1.2 · Comprehension]`** A sponsor asks how two estimates of the same plant, both
totalling 48,000,000, can carry defensible contingencies differing by a factor of 6.25. The best
explanation is:
- A. one estimator is more conservative than the other
- B. contingency provides for the range that the estimate's own definitional maturity implies, and that range narrows — on this book's illustrative ladder — from −30 %/+50 % at screening to −5 %/+8 % once scope is contracted ✅
- C. contingency is a policy percentage, so the difference reflects two companies' policies
- D. the two estimates must be stated at different base dates

*Rationale:* the class records how well scope was defined when the estimate was priced, and the
provision to the upper bound on the same 48,000,000 base moves from 24,000,000 to 3,840,000 (8.1.2).
A treats a structural property as a personal trait; C is the belief the whole KA exists to dislodge,
since a percentage that names no class states nothing; D names a real and separate parameter (8.2.3)
that governs escalation exposure rather than accuracy.

**MCQ 8.1-F `[8.1.3 · Evaluation]`** Kestrel's year-seven membrane replacement costs 4,098,909 in
nominal terms, and without a reserve the year-seven `DSCR` is 0.4561 — a payment default. The level
annual charge equivalent to the whole lifecycle programme is 644,606. The sponsors accept that a
reserve is required and ask what to deposit. The soundest recommendation is:
- A. deposit the level annual charge of 644,606 from year one, since that is the economically equivalent annual cost
- B. deposit 683,152 a year over the six years before the replacement, because the account must hold 4,098,909 before year seven and the level charge is an equivalence, not a deposit plan ✅
- C. deposit nothing and fund the replacement from a standby facility drawn in year seven
- D. defer the replacement to year eight, when the outstanding debt balance is lower

*Rationale:* the whole 4,098,909 must be in the account before the money is spent, with no credit
taken for interest on the balance — `4,098,909/6 = 683,151.50` a year, taken up to **683,152** so that
six deposits are not themselves short; six deposits of 644,606 total 3,867,636 and leave the
account **231,273** short in the one year it is needed (8.1.3). A is the right economic measure used
as a funding plan — precisely the confusion the worked example warns against. C is genuinely
available and is the weaker structure: it converts a funding certainty into a drawing risk in the
year the project would otherwise be at 0.4561, which is the year a lender is least willing to be
relied on. D subordinates a maintenance requirement to a credit calendar and does not remove the
cliff — it moves it and escalates it to 4,246,470.

**MCQ 8.1-G `[8.1.2 · Evaluation]`** A team declines the fixed-price wrap because six separate packages
priced lower, and proposes retaining the funded contingency of 3,645,403 — 7.59 % of the 48,000,000
base, which covers 94.93 % of a Stage E band and 25.32 % of a Stage C one. The soundest position is
that:
- A. the provision should stand: the packages priced lower, so the expected outturn is lower
- B. the provision should stand, and any difference should fall to the sponsors' cost-overrun support
  if it is needed
- C. declining the wrap returns the base-estimate uncertainty to the owner, so the provision must be
  resized on the range — 14,400,000 at the estimate's upper bound, 6,512,795 at P80 — and if the
  envelope cannot fund that, the envelope is wrong rather than the table ✅
- D. the provision should be raised to the 10 % rule, 4,800,000, as a policy compromise

*Rationale:* the contracting strategy and the contingency line are one decision taken twice, usually
by different people in different months (8.1.2); 7.59 % is a Stage E number and only a wrap earns it.
A confuses the lowest tendered price with the lowest outturn — the interfaces the packages exclude are
now the owner's. B converts funded provision into a contingent commitment worth less than cash (8.3.1)
and does so without resizing anything. D swaps one unreasoned percentage for another, and 4,800,000 is
still a third of the range-based upper bound.

**MCQ 8.1-H `[8.1.1 · Comprehension]`** Operating cost never appears in the sources-and-uses statement.
The reason is that it:
- A. is not a cost of the project, being incurred by the operator rather than the SPV
- B. is a recurring deduction inside `CFADS`, so it reduces coverage every period rather than creating
  a funding requirement before revenue exists ✅
- C. is met from the maintenance reserve, which is funded separately
- D. is funded by equity, which is why it sits outside the facility

*Rationale:* the sources-and-uses statement funds what must be paid before the asset earns, while
operating cost is a coverage driver — which is why a 500,000 understatement is 1.34 times the entire
annual headroom and recurs every year (8.1.1). A misstates who bears it: the SPV pays the operator's
fee and the costs the contract leaves with it. C confuses routine operating cost with periodic major
maintenance (8.1.3). D invents a funding route for a cost that is not funded at all.

### Self-check — KA 8.1

1. *Why is "10 % contingency" not an answer?* — Because contingency is only meaningful against a
   stated estimate class; 10 % is generous at Stage E and reckless at Stage C, and the ladder spans
   a factor of 6.25 on the same base.
2. *What did Kestrel buy that makes 7.59 % defensible?* — A fixed-price, date-certain EPC wrap,
   which transferred base-estimate uncertainty and moved the estimate to a Stage E position.
3. *State the year-seven consequence of omitting a maintenance reserve.* — `CFADS` 2,285,091,
   `DSCR` 0.4561 — a payment default, not merely a breach, on an otherwise healthy project.

---

## Knowledge Area 8.2 — Schedule-driven cash flow and escalation

*Topics: 8.2.1 from schedule to spend profile · 8.2.2 the shape of the S-curve and the area rule
for capitalised interest · 8.2.3 escalation, base dates and the breakeven that links the two.*

### 8.2.1 From schedule to spend profile

**Definition.** A **spend profile** (informally an **S-curve**) is a cost estimate distributed over
time according to the schedule that will execute it. It is produced by loading cost onto activities
or work packages and summing by period, and its characteristic shape — slow, then steep, then
slow — arises because mobilisation and commissioning are cheap per unit time while the middle of a
build is not.

The profile is the single most consequential output the schedule hands to the financing, and it is
handed over in a specific direction: **the schedule owns the timing and the financing owns the
consequence.** PML-AI Domain 6 builds the network, the critical path and the float that determine
when work happens; this domain takes that timing as given and prices it. Three distinctions matter
at the handover. **Certified spend is not incurred cost**, because a draw request is paid against
certified progress, and the certification lag shifts the funding profile relative to the cost
profile. **Committed is not spent**, because a purchase order creates a commitment that will
appear in a cost report long before it appears in a payment. And **the profile must be at a stated
price basis** — base-date money or then-current money — because the whole of 8.2.3 depends on
knowing which.

Kestrel's certified spend profile of **6, 9, 13, 16, 17, 15, 13 and 11 per cent** over eight
quarters is the profile Domain 6 funded. Note what it is not: it is not a straight line, and the
difference between it and a straight line is worth money.

### 8.2.2 The shape of the S-curve and the area rule

**The result to internalise.** Capitalised interest depends on the *shape* of the drawdown, not
only on its total. Two profiles that spend the same money over the same duration produce different
interest during construction, and the difference is computable in one line.

**The area rule.** With draws made at period end and interest accruing on the opening balance at
the periodic rate `r_q`, on total spend `S` funded at gearing `g`:

```
IDC = r_q × g × S × Σ cum(t−1)
```

where `cum(t−1)` is the cumulative fraction of spend completed *before* period `t`. The summation
is the discrete area under the cumulative drawdown curve — so **capitalised interest is
proportional to the area under the S-curve**, and "shape" is precisely the thing that area
measures.

**Worked example 8.2.2 — three shapes, one total, three interest bills.**

1. **Setup.** **USD 48,000,000** of certified spend over **eight quarters**, funded **70 %** by
   debt at **6.0 % per annum** (1.5 % per quarter) on the opening balance, draws at period end. For
   this example escalation is set to zero and interest is accrued outside the debt balance, to
   isolate the shape effect; Domain 6 (KA 6.2.1) runs the full circular capitalisation. Three
   profiles: **A**, Kestrel's S-curve (6, 9, 13, 16, 17, 15, 13, 11); **B**, front-loaded (14, 16,
   16, 14, 12, 11, 9, 8); **C**, back-loaded (5, 7, 9, 12, 15, 17, 18, 17). All sum to 100 %.
2. **Formula.** The area rule above; `r_q × g × S = 0.015 × 0.70 × 48,000,000 = 504,000` per unit
   of accumulated area.
3. **Substitution.** Σ cum(t−1) for A: `0 + 6 + 15 + 28 + 44 + 61 + 76 + 89 = 319` per cent, i.e.
   **3.1900**. For B: `0 + 14 + 30 + 46 + 60 + 72 + 83 + 92 = 397` → **3.9700**. For C:
   `0 + 5 + 12 + 21 + 33 + 48 + 65 + 83 = 267` → **2.6700**. Then `504,000 × area`.
4. **Result.** **A: USD 1,607,760 · B: USD 2,000,880 · C: USD 1,345,680.** The spread between
   front-loaded and back-loaded is **655,200**, or **40.75 %** of the S-curve's own interest bill,
   on identical total spend over an identical duration.
5. **Interpretation.** 655,200 is more than the arrangement fee on this facility would move in any
   plausible negotiation, and it is produced by a decision nobody in the room thinks of as
   financial: the sequence in which the contractor plans to build. That is the first professional
   consequence — **the construction programme is a financing variable, and the person who sets it
   is usually not consulted about interest.** The second is diagnostic. Because the area rule is
   linear in the area, a reviewer can test any construction model in one calculation: recompute
   `r_q × g × S × Σ cum(t−1)` from the profile and compare with the model's IDC. Agreement
   validates the whole interest calculation; disagreement localises the defect immediately (wrong
   periodic rate, average rather than opening balance, draws at period start, gearing applied to
   the wrong base). The third is a caution against the obvious inference. It does **not** follow
   that back-loading is good: delaying spend delays completion unless the duration is held, and
   holding the duration while back-loading the money usually means accepting a steeper, riskier
   finish. And it costs escalation, which is 8.2.3 — where the two effects meet.

### 8.2.3 Escalation, base dates and the breakeven that links the two

**Definitions.** An estimate is stated at a **base date** — the price level at which its rates
were compiled. **Escalation** is the movement in input prices between that base date and the date
the money is actually spent; it is a real cost, and it is not the same as **inflation** in the
general price level, which is why construction cost indices for labour, steel, cement and
specialist plant move differently from consumer price indices and from each other. Escalation
applies to the *timing* of spend, so it cannot be computed from a total: it must be computed
profile-period by profile-period.

```
Escalated spend = Σ  base spend(t) × (1 + e)^(t / periods per year)
```

Under a fixed-price contract the escalation on the contracted scope is the contractor's risk —
which is a large part of what a fixed price is *for*. The owner retains escalation on
owner-retained scope, on variations priced at then-current rates, and on any scope let after the
base date.

**Worked example 8.2.3 — the two prices of shape, and where they cancel.**

1. **Setup.** The three profiles of 8.2.2, now with **3.6 % per annum** construction escalation on
   the full 48,000,000 (the unwrapped variant — the same works procured as packages, so the owner
   carries escalation), quarterly escalation factor `1.036^(1/4) = 1.00888099`. Debt draws are
   70 % of *escalated* spend; interest as before. Compute escalated spend, IDC and the total for
   each profile, and find the escalation rate at which the S-curve and the back-loaded profile cost
   the same.
2. **Formula.** Escalated spend as above; IDC by accumulation of `opening balance × 0.015`; total
   funded construction cost = escalated spend + IDC. Breakeven `e*` solves
   `total(C, e*) = total(A, e*)`.
3. **Substitution and result.**

   | Profile | Escalated spend (USD) | Escalation over base (USD) | IDC (USD) | Total (USD) |
   |---|---|---|---|---|
   | **B** front-loaded | 49,750,365 | 1,750,365 | 2,052,036 | **51,802,401** |
   | **A** S-curve | 50,093,393 | 2,093,393 | 1,658,953 | **51,752,346** |
   | **C** back-loaded | 50,324,512 | 2,324,512 | 1,391,209 | **51,715,720** |

   Escalation spread B→C **574,146**; IDC spread C→B **660,827**; total spread **86,681**.
   Breakeven escalation between A and C: **4.1659 %**; between A and B: **4.1147 %**.
4. **Result.** At 3.6 % escalation the back-loaded profile is cheapest, but only by **36,626**
   against the S-curve and **86,681** against the front-loaded one — **0.17 %** of a 51.7 million
   spend. **Above an escalation rate of about 4.17 % the ranking reverses** and front-loading wins.
5. **Interpretation.** The near-cancellation is the most useful result in this KA, and it is not a
   coincidence. Deferring a dollar of spend by a period costs escalation at rate `e` on the whole
   dollar and saves interest at rate `r` on the geared fraction `g` of it, so shape is
   price-neutral when **`e ≈ g × r`** — here `0.70 × 6.0 % = 4.20 %`, against the computed 4.1659 %,
   the 3.4-basis-point gap being the discrete quarterly conventions. That heuristic is worth
   carrying: **at gearing of 70 % and a 6 % debt rate, construction escalation above roughly 4.2 %
   makes early spending cheaper and below it makes late spending cheaper**, and the further the
   actual rate is from that line, the more the programme sequence is worth arguing about. Three
   professional cautions. **The components are far larger than the net**, so a model that gets both
   wrong in the same direction produces a plausible total and two wrong lines — which is why the
   escalation and IDC lines are reviewed separately, never as a "construction cost" total.
   **The rates are not interchangeable**: using the revenue escalation assumption (2.967 % on
   Kestrel, Domain 6) for construction cost, or one construction index for all trades, is a
   silent error, and where an offtake escalates at one rate while costs escalate at another the
   difference compounds across the whole concession (Domain 7). **And the base date is a document,
   not an assumption.** An estimate whose base date nobody can name has an escalation exposure
   nobody can compute; the first question of any cost review is "as at what date are these rates?"

**Common pitfall — escalating the total.** Multiplying a 48,000,000 total by `1.036²` for a
two-year build gives 51,518,208, against the profile-correct 50,093,393 for the S-curve — an
overstatement of **1,424,815**, because it escalates all the money as though every dollar were
spent on the last day. The mirror error, escalating nothing because "the contract is fixed price",
understates the owner's retained exposure. Both are avoided by the same discipline: escalate the
profile, and state which scope is wrapped.

### AI in this KA

Cost-loading a schedule and pricing every plausible sequence for escalation and interest is a
deterministic search problem and proper machine work; so is index research — assembling and
documenting the published construction cost indices relevant to a project's trade mix with sources
and publication dates recorded. Two boundaries. **It must not select the escalation rate**, which is
a forecast of input prices over a specific build in a specific market; an assistant asked for "a
reasonable construction escalation assumption" supplies a plausible number with no provenance, and it
then propagates into the funding envelope, the depreciable base and the delay arithmetic of KA 8.4.
**And it must not re-shape the profile as an optimisation** — the area rule makes it trivially easy
to cut IDC by pushing spend later, and the constraint that makes that legitimate lives in the
construction logic, not in the cost model.

Verification: recompute IDC independently from the area rule and require agreement to the dollar;
confirm escalation on wrapped scope is zero and on retained scope is not; re-derive one period's
escalated spend by hand from the base rate and the stated base date; and require every escalation
assumption to carry a named index, source and human owner in the assumption register before the model
is run.

### Key terms — KA 8.2

| Term | Meaning |
|---|---|
| **Spend profile / S-curve** | A cost estimate distributed over time by the schedule that executes it. |
| **Certified spend** | Progress certified for payment; the basis of a draw request, lagging incurred cost. |
| **Area rule** | `IDC = r_q × g × S × Σ cum(t−1)`; capitalised interest is proportional to the area under the drawdown curve. |
| **Base date** | The price level at which an estimate's rates were compiled. |
| **Escalation** | Input-price movement between base date and spend date; trade-specific, not general inflation. |
| **Shape neutrality** | Profile timing is cost-neutral when escalation ≈ gearing × debt rate (4.20 % on Kestrel; 4.1659 % computed). |

### Sample MCQs — KA 8.2

**MCQ 8.2-A `[8.2.2 · Application]`** 48,000,000 of spend over eight quarters, 70 % debt-funded at
1.5 % per quarter on opening balances. The profile's cumulative-before-period fractions sum to
3.9700. Capitalised interest is:
- A. USD 1,607,760
- B. USD 2,000,880 ✅
- C. USD 2,858,400
- D. USD 504,000

*Rationale:* `0.015 × 0.70 × 48,000,000 × 3.9700 = 2,000,880`. A is the S-curve's area of 3.1900;
C omits the gearing, applying interest to full spend (`0.015 × 48,000,000 × 3.9700 = 2,858,400`);
D is the per-unit-of-area coefficient mistaken for the answer.

**MCQ 8.2-B `[8.2.3 · Analysis]`** A model escalates a 48,000,000 two-year construction estimate by
multiplying it by `1.036²`, giving 51,518,208, where the profile-correct figure for the same S-curve
is 50,093,393. The error is:
- A. 1,424,815 of understatement
- B. 1,424,815 of overstatement, because escalating a total prices every dollar as though spent on the final day ✅
- C. immaterial, since the rate is correct
- D. offset by the interest calculation

*Rationale:* 51,518,208 − 50,093,393 = 1,424,815 too much; escalation must be applied
period-by-period to the profile. C mistakes a correct rate for a correct method; D is wrong in
direction — a higher spend also raises draws and therefore IDC.

**MCQ 8.2-C `[8.2.3 · Analysis]`** At 70 % gearing and a 6.0 % debt rate, the construction
escalation rate above which front-loading spend becomes cheaper than back-loading it is closest to:
- A. 6.0 %
- B. 4.2 % ✅
- C. 3.6 %
- D. 1.8 %

*Rationale:* Deferral costs escalation on 100 % of spend and saves interest on the geared 70 %, so
neutrality is at `e ≈ g × r = 4.20 %` — computed as **4.1352 %** between Kestrel's front- and
back-loaded profiles on the quarterly convention (8.2.3 quotes 4.1659 % for the same effect measured
between the S-curve and the back-loaded profile; every pairwise breakeven sits just below `g × r`).
A ignores gearing; C is the assumed escalation rate, not the breakeven; D halves the rate for no
stated reason.

**MCQ 8.2-D `[8.2.2 · Analysis]`** A reviewer wants one calculation that validates a construction
model's entire capitalised-interest line. The best choice is:
- A. confirm the total spend equals the contract price
- B. recompute IDC from the area rule and require agreement to the dollar ✅
- C. confirm the closing debt balance equals the facility amount
- D. compare the IDC percentage against other projects

*Rationale:* The area rule reproduces IDC from the profile, rate and gearing, so agreement
validates all four inputs and disagreement localises the defect. A and C are necessary but pass
with a wrong interest convention; D is benchmarking, not verification.

**MCQ 8.2-E `[8.2.2 · Evaluation]`** A modeller proposes replacing Kestrel's S-curve (area 3.1900)
with the back-loaded profile (area 2.6700) to cut capitalised interest by 262,080 on identical total
spend over an identical duration, and asks the contractor to resequence accordingly. The soundest
response is:
- A. accept: 262,080 is a real saving produced by the area rule, on the same money over the same time
- B. reject the proposal as framed: at 3.6 % escalation the back-loaded profile's escalation is 231,119 higher and its interest 267,744 lower, so the total funded construction costs differ by only 36,626 — 0.07 % of a 51.7 million spend — and the constraint that makes a sequence legitimate lives in the construction logic, not in the cost model ✅
- C. reject: once escalation is counted, back-loading always costs more than front-loading
- D. accept, and extend the programme by a further quarter to reduce the area again

*Rationale:* escalation and interest run in opposite directions and nearly cancel near
`e ≈ g × r = 4.20 %`, so at 3.6 % the whole prize is 36,626 — set against a steeper, riskier finish
that no cost model prices (8.2.2, 8.2.3). A quotes the zero-escalation isolation as though it were the
answer, which is the trap the isolation exists to expose. C overstates the correction: below the
breakeven of about 4.17 % back-loading genuinely is cheaper, by exactly the 36,626 computed here.
D compounds the error, because lowering the area by extending the duration defers commercial
operations at 532,000 a month of `CFADS` (KA 8.4.2) — against a total interest saving of 262,080 for
the entire build.

**MCQ 8.2-F `[8.2.3 · Evaluation]`** Asked for "a reasonable construction escalation assumption", an
assistant returns 3.6 % per annum with no source. The modeller notes that 3.6 % is within the range of
published construction indices, and that the model already carries Domain 6's 2.967 % revenue
escalation. The soundest position is that:
- A. 3.6 % may be used, since it is within the range of published indices for this class of work
- B. the revenue escalation of 2.967 % should be used for both, so that the model is internally
  consistent
- C. no escalation rate may be relied on until it names an index, a source, a base date and a human
  owner in the assumption register, because the rate propagates into the funding envelope, the
  depreciable base and the delay arithmetic ✅
- D. one blended index should be used for all trades, since trade-level differences average out

*Rationale:* an assistant must not select the escalation rate, which is a forecast of input prices for
a specific build in a specific market (the AI boundary in KA 8.2), and construction and revenue
escalation are distinct assumptions — labour, steel, cement and specialist plant move differently from
consumer prices and from each other. A accepts plausibility as provenance. B imports a revenue
assumption into a cost line, which is the silent error 8.2.3 names. D discards the trade mix that makes
the rate mean anything. The related discipline is that escalation is applied period-by-period to the
profile: escalating the 48,000,000 total by 1.036² gives 51,518,208 against the profile-correct
50,093,393, an overstatement of 1,424,815.

**MCQ 8.2-G `[8.2.2 · Comprehension]`** The area rule states that `IDC = r_q × g × S × Σ cum(t−1)`.
What it tells a reader is that capitalised interest:
- A. depends on total spend and duration, so the shape of the drawdown is a presentational matter
- B. is proportional to the area under the cumulative drawdown curve, so two profiles spending the same
  money over the same duration produce different interest bills ✅
- C. is proportional to the peak debt balance reached during construction
- D. equals the average of the opening and closing balances multiplied by the rate

*Rationale:* the summation is the discrete area under the cumulative curve, which is precisely what
"shape" measures — on Kestrel, 1,345,680 back-loaded against 2,000,880 front-loaded on identical spend
and duration, a 655,200 spread and 40.75 % of the S-curve's own bill (8.2.2). A is the belief the rule
refutes. C and D describe quantities the rule does not use: the same peak balance is reached on every
profile that draws the full facility, and an average-of-two-balances calculation ignores the whole
interior of the curve.

### Self-check — KA 8.2

1. *State the area rule and what it proves.* — `IDC = r_q × g × S × Σ cum(t−1)`; capitalised
   interest is proportional to the area under the cumulative drawdown curve, so shape has a price.
2. *How much did shape alone move Kestrel's interest bill?* — 655,200 between front- and
   back-loaded profiles, 40.75 % of the S-curve's own 1,607,760, on identical spend and duration.
3. *Why is the total cost of the three profiles nearly equal at 3.6 % escalation?* — Escalation and
   interest run in opposite directions and cancel near `e = g × r = 4.20 %`; the two components
   differ by 574,146 and 660,827 while the totals differ by 86,681.

---

## Knowledge Area 8.3 — Contingency and management reserve

*Topics: 8.3.1 the reserve family in a financing, and who controls the draw · 8.3.2 sizing
contingency from quantified risk · 8.3.3 why the percentage method fails, computably.*

### 8.3.1 The reserve family in a financing, and who controls the draw

**Definition.** **Contingency** is funded provision for identified risks within the agreed scope.
**Management reserve** is provision for what the register does not contain — in delivery language,
unknown-unknowns and scope change. PML-AI Domain 8 (KA 8.3.2) sets out that distinction as a
delivery-governance matter, with contingency controlled by the project manager under a published
draw protocol and management reserve controlled by the sponsor through change control. In a
financing, three things change.

**Contingency becomes a funded line with an external gatekeeper.** It sits in the sources-and-uses
statement (Domain 6) as money the facility will lend and the equity will match, and a draw against
it typically requires the lenders' technical adviser to certify both that the cost is a proper
contingency item and that the *remaining* contingency is still adequate for the *remaining* risk
(KA 8.4.1). The project manager's draw protocol still applies internally; it is no longer the only
gate.

**Management reserve mostly leaves the envelope.** Lenders do not fund unknown scope, so the
financing equivalent is **contingent support outside the base case**: cost-overrun undertakings,
standby equity, a standby debt tranche, or sponsor several-liability support (Domain 5, KA 5.2.3).
The economic difference is important — funded contingency is drawn on certification, contingent
support is called on a defined trigger, and the second is worth less than the first to a lender
precisely because it depends on someone's willingness and ability to pay when called.

**Owner's contingency and contractor's contingency are different money.** The contractor's sits
inside the contract price, invisible and unavailable; the owner's is the only provision the project
can actually draw (KA 8.1.2's pitfall).

**Worked example 8.3.1 — sizing Kestrel's contingent support.**

1. **Setup.** Kestrel's funded contingency is **3,645,403**. The lender requires that the retained
   construction risk be covered to **P95** by funded contingency plus contingent equity support.
   The retained register's mean exposure is **2,690,000** with a standard deviation of **1,848,973**
   (derived in 8.3.2).
2. **Formula.** P95 ≈ mean + 1.6449 × σ (the 95th percentile of a normal approximation); standby
   requirement = P95 − funded contingency.
3. **Substitution.** `2,690,000 + 1.6449 × 1,848,972.69 = 2,690,000 + 3,041,375.17`; then
   `5,731,375 − 3,645,403`.
4. **Result.** P95 exposure **USD 5,731,375**; contingent support required **USD 2,085,972**.
5. **Interpretation.** The number to hold on to is the *ratio*: covering from P80 to P95 costs
   another 1,485,280 of committed support on a project whose whole funded contingency is 3,645,403.
   Tail cover is expensive, which is why it is provided as a *commitment* rather than as funded
   cash — the sponsors post an obligation, not money, and pay for it only in the futures where it
   is called. That is the correct structure and it carries a specific professional caution: a
   commitment is only as good as the entity behind it, so 2,085,972 of support from a well-rated
   sponsor and the same figure from a thinly capitalised project company are not the same credit,
   and Domain 5 (KA 5.2.3) showed the distinction between joint-and-several and several liability
   deciding which. **A lender's contingency question is never only "how much?"; it is "how much,
   funded by whom, callable on what trigger, and enforceable against what balance sheet?"**

### 8.3.2 Sizing contingency from quantified risk

**The method.** Take the register, quantify each item as probability × impact, aggregate to a mean
and a variance, and read the amount at the confidence level policy requires. The machinery is
PML-AI Domain 8's (KA 8.2.2 and 8.2.4) and is not re-derived here: `EMV = p × impact`; for
independent items mean = Σ `EMV` and variance = Σ `p(1 − p) × impact²`; a P80 amount ≈
mean + 0.8416 σ.

**Worked example 8.3.2 — Kestrel's retained construction risk, and two P80s that disagree.**

1. **Setup.** Under the fixed-price wrap the owner retains six quantified items on a 48,000,000
   base. Compute the mean, the standard deviation and the P80; separately, translate the Stage C
   accuracy range (−15 %/+30 %) into a P80 using a triangular distribution with minimum
   40,800,000, mode 48,000,000 and maximum 62,400,000; then reconcile the two answers.
2. **Formula.** As above. For a triangular distribution with parameters `a < m < b`, the cumulative
   probability above the mode is `F(x) = 1 − (b − x)² / ((b − a)(b − m))`, its mean is
   `(a + m + b)/3`, and the P80 solves `(b − x)² = 0.20 (b − a)(b − m)`.
3. **Substitution and result.**

   | ID | Retained risk | `p` | Impact (USD) | `EMV` (USD) |
   |---|---|---|---|---|
   | C1 | Ground conditions worse than surveyed (owner-retained geotechnical basis) | 0.40 | 2,400,000 | 960,000 |
   | C2 | Utility diversion and third-party interface scope growth | 0.30 | 1,800,000 | 540,000 |
   | C3 | Membrane supply price above the contract indexation formula | 0.35 | 1,400,000 | 490,000 |
   | C4 | Marine intake weather standby beyond the allowance | 0.50 | 900,000 | 450,000 |
   | C5 | Permit condition requiring additional monitoring works | 0.20 | 2,000,000 | 400,000 |
   | C6 | Early-completion rebate *(opportunity)* | 0.25 | (600,000) | **(150,000)** |
   | | **Mean exposure** | | | **2,690,000** |

   Variance `= 0.40×0.60×2,400,000² + 0.30×0.70×1,800,000² + 0.35×0.65×1,400,000² +
   0.50×0.50×900,000² + 0.20×0.80×2,000,000² + 0.25×0.75×600,000² = 3,418,700,000,000`;
   σ = **1,848,973**; P80 `= 2,690,000 + 0.8416 × 1,848,973 =` **4,246,095**. Worst-case sum of
   threats **8,500,000** (17.71 % of base). Triangular: mean **50,400,000** (contingency
   2,400,000, 5.00 %); P80 total **54,512,795**, contingency **6,512,795** (**13.57 %**).
4. **Result.** **Register P80 contingency USD 4,246,095 (8.85 % of base); range-based P80
   contingency USD 6,512,795 (13.57 %).** The funded 3,645,403 sits at the **69.7th percentile** of
   the register and the **62.81st percentile** of the Stage C range.
5. **Interpretation.** Two defensible methods have produced answers 2,266,700 apart, and the
   resolution is the most valuable idea in this KA: **they are not competing estimates of the same
   thing.** The accuracy range measures *systemic uncertainty in the base estimate* — quantities,
   productivity, unpriced scope, the things a better-defined design would resolve. The register
   measures *discrete risk events* that either happen or do not. Adding them double-counts, because
   a Stage C range already contains an allowance for the sort of surprise the register enumerates;
   taking the lower of them under-provides. The defensible convention is to **take the higher, and
   then ask which of the two the contracting structure has actually eliminated.** On Kestrel the
   fixed-price wrap transfers the base-estimate uncertainty to the contractor, so the range-based
   number is the right measure of a risk the owner no longer carries, and the register-based
   4,246,095 is the owner's provision. On the unwrapped variant of 8.1.2 the range governs and
   6,512,795 is the floor. **The funded 3,645,403 is therefore 600,692 short of the P80 the retained
   register supports** — a small number with a permanent consequence: capitalise it and debt becomes
   42,600,692, the instalment 5,081,284.04, the `DSCR` **1.2564** and annual headroom **286,459**,
   down 85,979 or **23.1 %** from the 372,438 Domain 10 measured, for the whole twelve-year loan
   life. Two standing caveats carry over from PML-AI Domain 8 unchanged and must be stated in any
   paper using this arithmetic: **independence is an optimistic assumption** (C1 and C2 both concern
   ground and third parties, and if they are correlated the variance and the true P80 are higher),
   and **the normal approximation is a convenience** for a handful of Bernoulli items where a
   simulation over the register and the schedule is the proper instrument. Neither caveat weakens
   the method against the alternative in 8.3.3; both bound what may be claimed for it.

> **Fig 8.3.1 — Six answers to one question: how much contingency?** Horizontal bar chart, x-axis
> contingency in USD on Kestrel's 48,000,000 base estimate (0–9m). Bars, top to bottom: worst-case
> sum of threats **8,500,000** (17.71 %); range P80 on the Stage C estimate, unwrapped
> **6,512,795** (13.57 %); the ten-per-cent rule **4,800,000** (10.00 %, crimson, annotated "P87.3
> on the register, P70.4 on the range"); register P80 under a wrap **4,246,095** (8.85 %, brand blue,
> "the defensible provision here"); the funded balancing line from Domain 6 **3,645,403** (7.59 %,
> crimson, "a P69.7 provision — 600,692 short"); register mean Σ`EMV` **2,690,000** (5.60 %). The
> two crimson bars are the two provisions that state no confidence level of their own — the
> percentage rule and the balancing line. Headline: a 3.16× spread between the six answers. Footer:
> a contingency percentage is meaningless without the estimate class and the basis it was sized on.
> Source: PCI original. Alt
> text: six horizontal bars showing contingency amounts from two point seven million to eight and a
> half million for the same project, with the funded amount third from the bottom and the
> percentage rule of thumb in the middle.

### 8.3.3 Why the percentage method fails, computably

**The practice.** Contingency is set at a policy percentage of base cost — 5 %, 10 %, 15 % — chosen
by convention, precedent or negotiation. It is fast, it is universally understood, and it has two
defects that can be stated as arithmetic rather than as opinion.

**Defect one: it does not know what confidence it buys.** Kestrel's ten-per-cent rule gives
**4,800,000**. Against the retained register (mean 2,690,000, σ 1,848,973) that is
`z = (4,800,000 − 2,690,000)/1,848,973 = 1.1412`, a **P87.3** provision. Against the Stage C
accuracy range it is `F(52,800,000) = 1 − (9,600,000)²/((21,600,000)(14,400,000)) = 0.7037`, a
**P70.4** provision. **The same 4,800,000 is a 87.3 % promise on one basis and a 70.4 % promise on
the other**, and the rule cannot tell which it is making. A provision whose confidence level is
unknown cannot be compared with a covenant, a support commitment or another project, which is the
entire practical purpose of sizing it.

**Defect two: it does not move when the risk moves.** Suppose C1, the ground-conditions risk, is
retired — the additional site investigation is complete, the conditions are as surveyed, the risk
window has closed with no impact. The register responds immediately: mean falls to **1,730,000**,
σ to **1,426,990**, and the P80 provision to **2,930,955** — a release of **1,315,141**. The
percentage rule responds not at all: 10 % of base is still 4,800,000, because base cost has not
changed. The organisation is now holding **1,869,045** of provision against risks that no longer
exist, and that holding has a price in two stages. **While it is undrawn** it is committed capital:
at a 0.60 % per annum commitment fee the excess costs **11,214** a year — small, and the reason the
excess is rarely challenged. **If it is drawn**, the price becomes permanent. Funded 70/30, the
excess adds **1,308,332** to senior debt, raising the annual instalment by **156,054** to
5,165,689.12, taking the `DSCR` from 1.2743 to **1.2358**, lifting the covenant cash trigger to
**6,198,827** and cutting annual headroom from 372,438 to **185,173** — a fall of **187,265**, or
**50.3 %**, for the whole twelve-year loan life. That is the financing translation of the
slush-fund failure PML-AI Domain 8 (KA 8.3.2) warns about: **an unreleased provision that gets spent
because it is there halves the project's covenant headroom for a decade**, and the percentage method
guarantees there will always be some.

**The symmetric error is worse.** A percentage rule that happens to under-provide is invisible in
exactly the same way. Nothing in "10 % of base" reacts when a new risk is added to the register, so
a project can accumulate exposure all through detailed design while its contingency line stays
constant, and the first evidence anyone sees is a draw request that cannot be certified (KA 8.4.1).

**What survives of the percentage method.** Two legitimate uses. As a **screening figure** at
Stage A or B, where no register exists and none could — a factored allowance is the honest
instrument for an estimate that is itself factored. And as a **cross-check** on a register-based
number: if quantified risk produces 1 % of base at Stage C, the register is incomplete, and if it
produces 40 %, the project is being asked to fund a scope that is not defined. The rule of thumb is
a plausibility test on the arithmetic, never a substitute for it.

### AI in this KA

Monte Carlo simulation over the register and the schedule is the correct instrument that 8.3.2's
normal approximation stands in for, and running it — with correlation structures, which are what make
the approximation optimistic — is machine work. Correlation *detection* is a further genuine use:
mining an organisation's own historical registers and outturns for items that have moved together is
beyond manual analysis and directly improves the variance estimate. So is register hygiene —
duplicates, items double-counted between register and base estimate, impacts stated in inconsistent
price bases. Three things a model must not do: **set probabilities**, which are expert judgments
about a specific site, contractor and market, and which if invented give the P80 a false precision a
board cannot see; **choose the confidence level**, a risk-appetite decision belonging to governance
(PML-AI D8, KA 8.2.4); and **reconcile the two P80s of 8.3.2**, which is a judgment about what the
contract transferred and therefore a reading of a document.

Verification: recompute the mean by hand and require it to equal Σ `EMV` — a simulated mean that
differs from the analytic mean means the model is not implementing the register; recompute one
percentile with the normal approximation and require the simulation to be in the same region, a large
divergence being either documented correlation or an error; and require every probability and impact
to carry a named owner and a dated basis, because nothing in a simulation adds credibility its inputs
do not have.

### Key terms — KA 8.3

| Term | Meaning |
|---|---|
| **Contingency** | Funded provision for identified risks within agreed scope; drawn on certification. |
| **Management reserve** | Provision for unidentified risk and scope change; in a financing, largely replaced by contingent support. |
| **Contingent support** | Cost-overrun undertaking, standby equity or standby tranche called on a defined trigger. |
| **Confidence level (P50 / P80 / P95)** | The probability that the provision covers the aggregate outcome; a policy choice. |
| **Owner's vs contractor's contingency** | Only the owner's is drawable by the project; the contractor's is inside the price. |
| **Percentage method** | Contingency as a policy share of base cost; states no confidence and does not respond to risk retirement. |
| **Contingency release** | Reduction in the provision when a risk retires; the percentage method cannot produce one. |

### Sample MCQs — KA 8.3

**MCQ 8.3-A `[8.3.2 · Application]`** A retained register has mean exposure 2,690,000 and standard
deviation 1,848,973. The P80 contingency is closest to:
- A. USD 2,690,000
- B. USD 4,246,095 ✅
- C. USD 8,500,000
- D. USD 5,731,375

*Rationale:* `2,690,000 + 0.8416 × 1,848,973 = 4,246,095`. A is the mean, which by construction is
exceeded about half the time; C is the worst-case sum of threats; D is the P95 (z = 1.6449), a
different policy choice.

**MCQ 8.3-B `[8.3.3 · Analysis]`** A 10 %-of-base contingency of 4,800,000 is a P87.3 provision
against the risk register and a P70.4 provision against the estimate's accuracy range. The correct
conclusion is:
- A. the rule is validated, since both figures exceed P50
- B. the rule states no confidence level, so the provision cannot be compared with a covenant, a support commitment or another project ✅
- C. the register must be wrong, since the two bases disagree
- D. the average of the two, P78.9, is the provision's true confidence

*Rationale:* The two percentiles measure different uncertainties (discrete events versus systemic
estimate error), so a single percentage of base cannot express a confidence at all — which is the
defect. A mistakes "above the median" for "sized"; C misreads a difference in basis as an error; D
averages two probabilities that are not on the same scale.

**MCQ 8.3-C `[8.3.3 · Application]`** The largest register item (p 0.40, impact 2,400,000) retires
with no impact. Register P80 falls from 4,246,095 to 2,930,955 while the 10 % rule stays at
4,800,000. If the resulting excess is drawn and funded 70/30, the effect on annual covenant headroom
of 372,438 is a fall of:
- A. nil — contingency is not carried at a cost
- B. 187,265, or 50.3 %, because 1,308,332 of extra senior debt raises the instalment by 156,054 ✅
- C. 11,214, the commitment fee on the excess
- D. 1,869,045, the excess itself

*Rationale:* `4,800,000 − 2,930,955 = 1,869,045`; `× 0.70 = 1,308,332`;
`/8.383844 = 156,054` of extra instalment; the 1.20× trigger rises to 6,198,827 and headroom falls
to 185,173. A ignores that drawn contingency is debt serviced for the loan life; C is the cost while
the excess is *undrawn*, not drawn; D confuses the provision with its coverage effect.

**MCQ 8.3-D `[8.3.1 · Analysis]`** Why do lenders treat 2,085,972 of contingent equity support as
worth less than 2,085,972 of funded contingency?
- A. it is a smaller amount in present-value terms
- B. it is only drawable on certification
- C. it depends on a sponsor's willingness and ability to pay when called, which is a credit exposure rather than cash in the structure ✅
- D. it cannot be documented

*Rationale:* Funded contingency is money the facility will lend against certification; contingent
support is a commitment whose value is the obligor's credit (8.3.1, and Domain 5 KA 5.2.3 on
several versus joint-and-several liability). B describes funded contingency; A confuses timing with
credit; D is false.

**MCQ 8.3-E `[8.3.2 · Evaluation]`** Kestrel's retained register supports a P80 contingency of
4,246,095 (8.85 % of base) while the Stage C accuracy range supports 6,512,795 (13.57 %); the funded
balancing line is 3,645,403, a P69.7 provision on the register. The works are let under a fixed-price,
date-certain wrap. The soundest recommendation is:
- A. fund 6,512,795, following the convention of taking the higher of two defensible provisions
- B. size the provision at 4,246,095 on the retained register, and settle in the funding documents whether the additional 600,692 comes from debt or from equity — the range-based 6,512,795 measures base-estimate uncertainty the wrap has transferred to the contractor ✅
- C. leave the funded 3,645,403 unchanged and disclose it as a P69.7 provision
- D. fund the 600,692 by capitalising it into senior debt, the coverage cost being only 23.1 % of headroom

*Rationale:* "take the higher" is where the reconciliation starts, not where it ends: the next question
is which of the two uncertainties the contracting structure has eliminated, and a full wrap transfers
base-estimate uncertainty, leaving the owner the discrete retained register (8.3.2, 8.1.2). A applies
half the rule and would fund 2,266,700 against a risk the owner no longer carries. C is honest and
insufficient — a named P69.7 beats an unnamed percentage but is still 600,692 short of the confidence
the register supports. D is a real funding route that pre-empts the decision it should present:
capitalising 600,692 takes the instalment to 5,081,284, the `DSCR` to 1.2564 and annual headroom
from 372,438 to 286,459 for the whole twelve-year loan life, which is a choice about where the money
lands and belongs to the sponsors before close.

**MCQ 8.3-F `[8.3.1 · Comprehension]`** A delivery manager asks where management reserve sits in a
project financing. The best explanation is:
- A. it is contingency under another name, relabelled for the lenders
- B. lenders will not fund undefined scope, so its financing equivalent sits outside the base case as contingent support — a cost-overrun undertaking, standby equity or a standby tranche — called on a trigger rather than drawn on certification ✅
- C. it is the contractor's own contingency inside the contract price
- D. it is the unused balance of contingency remaining at the end of the build

*Rationale:* contingency is funded provision for identified risks within agreed scope, drawn against
certification; management reserve covers what the register does not contain, which a facility will not
lend against (8.3.1). A collapses a distinction the funding structure depends on; C names a third and
separate pot the owner cannot draw at all (8.1.2's pitfall); D describes a release rather than a
reserve.

**MCQ 8.3-G `[8.3.3 · Evaluation]`** Kestrel's ground-conditions risk retires with no impact: the
register's P80 falls from 4,246,095 to 2,930,955 while the 10 % rule stays at 4,800,000, leaving
1,869,045 of excess provision. The project director resists reducing it while construction is running,
and the finance function does not press the point because the excess costs only the 11,214 a year of
commitment fee. The soundest position is that:
- A. the director is right: cover should not be reduced while construction continues, and 11,214 is
  immaterial
- B. the excess should be released, and the release relied on as a matter of judgement at each
  quarterly review
- C. a recalculation of the required provision at defined milestones should be written into the finance
  documents, because the asymmetry is 11,214 a year undrawn against 187,265 of annual covenant
  headroom — 50.3 % — permanently lost if the excess is drawn ✅
- D. the excess should be drawn now and held as project cash, so that it is available if needed

*Rationale:* released contingency is rarely given back because nobody is rewarded for handing money
back, so the answer is mechanical rather than behavioural (8.3.3, 8.A.2): funded 70/30 the excess adds
1,308,332 of senior debt and 156,054 to the annual instalment, taking the `DSCR` to 1.2358 and headroom
to 185,173 for the whole twelve-year loan life. A prices the visible cost and ignores the permanent one.
B is the arrangement that has just failed. D converts an option into the exposure the release exists to
avoid, and does so at once.

### Self-check — KA 8.3

1. *Why can two correct P80 contingencies differ by 2,266,700 on one project?* — They measure
   different uncertainties: the register measures discrete events, the accuracy range measures
   systemic base-estimate error; take the higher, then ask which the contract transferred.
2. *State the two computable defects of the percentage method.* — It states no confidence level
   (4,800,000 is P87.3 on one basis, P70.4 on the other), and it does not respond to risk
   retirement: 1,869,045 of excess, costing 11,214 a year undrawn and 50.3 % of covenant headroom
   if drawn.
3. *What does the P80-to-P95 step cost on Kestrel, and how is it provided?* — 1,485,280 more of
   cover, provided as a 2,085,972 contingent commitment rather than funded cash, because tail cover
   as cash is prohibitively expensive.

---

## Knowledge Area 8.4 — Delay impact, cost-to-complete, and the interface with project controls

*Topics: 8.4.1 from earned value to cost to complete and the funds sufficiency test · 8.4.2 the
cost of a month of slip during construction · 8.4.3 one data spine, two questions.*

### 8.4.1 From earned value to cost to complete and the funds sufficiency test

**Definition.** **Cost to complete** (`CTC`) is the money still required to finish: `CTC = EAC − AC`
on any chosen `EAC` method. The **funds sufficiency test** — often written into a facility as an
"in balance" condition — asks whether the funds remaining available, from the undrawn facility,
unused contingency and uncalled equity, are at least `CTC`. It is the test a lender runs on a
monthly cost report, and it is not a test a project control account produces.

**Worked example 8.4.1 — what a lender does with Auriga's earned value.**

1. **Setup.** Project Auriga (PML-AI D7, KA 7.3.3) at the week-13 data date: `BAC` **4,000,000**,
   `AC` **2,120,000**, `EV` **1,920,000**, `CPI` **0.905660**, `SPI` **0.923077**, and the three
   published forecasts `EAC` **4,200,000** (remaining work at the budgeted rate), **4,416,667**
   (`BAC/CPI`) and **4,608,056** (`CPI × SPI`). Assume the delivery organisation financed it: total
   funding **4,300,000**, being `BAC` 4,000,000 plus a funded contingency of **300,000** (7.5 % —
   the same order as Kestrel's 7.59 %), of which **180,000** has already been drawn. Of the sanction
   register (PML-AI D8, KA 8.2.2), R2 has materialised and R4 has been retired; R1 (p 0.35, impact
   240,000), R3 (p 0.25, 320,000) and the R5 opportunity (p 0.30, −120,000) remain open. Run both
   lender tests.
2. **Formula.** `CTC = EAC − AC`; funds available = total funding − `AC`; surplus/(shortfall) =
   available − `CTC`. Contingency adequacy: remaining contingency versus the P80 of the *remaining*
   register (mean = Σ `EMV`, variance = Σ `p(1 − p)`impact², P80 ≈ mean + 0.8416 σ).
3. **Substitution.** Available `= 4,300,000 − 2,120,000 = 2,180,000`. `CTC` by method:
   `4,200,000 − 2,120,000`; `4,416,667 − 2,120,000`; `4,608,056 − 2,120,000`. Remaining register
   mean `= 84,000 + 80,000 − 36,000`; variance
   `= 0.2275×240,000² + 0.1875×320,000² + 0.21×120,000²`.
4. **Result.**

   | `EAC` method | `EAC` (USD) | `CTC` (USD) | Funds available (USD) | Surplus / (shortfall) |
   |---|---|---|---|---|
   | Remaining at budgeted rate | 4,200,000 | 2,080,000 | 2,180,000 | **+100,000** |
   | `BAC/CPI` | 4,416,667 | 2,296,667 | 2,180,000 | **(116,667)** |
   | `CPI × SPI` | 4,608,056 | 2,488,056 | 2,180,000 | **(308,056)** |

   Remaining register: mean **128,000**, σ **187,957**, **P80 286,185** against remaining
   contingency of **120,000** — a **166,185** shortfall on the contingency-adequacy test.
5. **Interpretation.** This is the domain's bridge, in one table. A project controller's report
   ends at the `EAC` column and its honest conclusion is a range: the project will cost between 4.2
   and 4.6 million depending on which assumption about the remaining work holds (PML-AI D7's point,
   that a forecast is a statement about the future's resemblance to the past). **A lender reads the
   same three numbers and produces a date and an amount:** on the `BAC/CPI` forecast the project is
   out of balance by 116,667, so before the next drawdown the sponsor must inject 116,667 of equity
   or the facility does not fund. The forecast has become a cash call. Three consequences follow
   for a finance leader. **The choice of `EAC` method is a commercial negotiation, not a technical
   one**, because it sets the size of the cash call: the sponsor argues for the budgeted-rate
   forecast (+100,000, in balance), the technical adviser for `BAC/CPI` or worse, and the honest
   position is that a `CPI` of 0.906 sustained over thirteen weeks is evidence about the remaining
   work unless something specific has changed — which someone must name. **The contingency test
   fails independently and earlier.** With 120,000 of contingency left against a remaining P80
   exposure of 286,185, a certifier cannot honestly confirm that remaining contingency is adequate
   for remaining risk, and that conclusion needed no `EAC` at all — only the register and the draw
   history. A project can therefore be out of balance on the contingency test while every cost
   report still shows a forecast inside budget. **And the shortfall was arithmetically predictable
   at sanction.** The 300,000 funded was a **P53.5** provision against the sanction register
   (mean 278,000, σ 252,642), while the P80 was **490,624**; the actual overrun on the `BAC/CPI`
   forecast is 416,667, a **P70.8** event — comfortably inside a P80 provision and comfortably
   outside a P53.5 one. Nothing unlikely happened. **The project was funded to a confidence level
   nobody named, and then behaved normally.**

### 8.4.2 The cost of a month of slip during construction

**The three components.** A month of delay declared *during* construction costs three separable
things: **escalation** on the scope not yet bought, because the money will be spent a month later
at a month's higher prices; **extra interest** on the balance already drawn, which accrues for a
month with no offsetting progress; and **deferred revenue**, because the operating period starts a
month later. Domain 5 (KA 5.4.2) priced a slip *at* COD, where remaining spend is zero and the
escalation row vanishes. During the build it does not vanish, and the relative sizes of the three
rows change continuously through the programme.

**Worked example 8.4.2 — Kestrel declares a three-month slip at the end of quarter six.**

1. **Setup.** At the end of construction quarter six, cumulative debt drawn is **31,990,655**
   (Domain 6's funding profile). The remaining owner-retained scope — owner's costs and land of
   3,600,000 spent on the same profile — stands at **922,906** at then-current prices; the EPC scope
   is fixed-price and date-certain, so its escalation is the contractor's risk. Escalation on
   retained scope is **3.6 %** per annum, giving a monthly factor of `1.036^(1/12) = 1.00295161`,
   i.e. **0.2952 %** per month. The debt rate is **6.0 %**; operating `CFADS` would be **6,384,000**
   per year. Delay damages are **USD 20,000 per day** (Domain 5). Compute the cost of one month, the
   cost of the three-month slip, the damages coverage, and the coverage consequence if the cost
   components are capitalised.
2. **Formula.** Escalation row = remaining retained scope at then-current prices × monthly
   escalation factor − 1. Interest row = debt drawn × rate ÷ 12. Revenue row = annual `CFADS` ÷ 12.
   Coverage consequence: new debt = 42,000,000 + capitalised cost; instalment = new debt ÷
   `AF(0.06, 12)`; `DSCR` = `CFADS` ÷ instalment; covenant trigger = instalment × 1.20 (Domain 10).
3. **Substitution.** `922,906 × 0.00295161`; `31,990,655 × 0.06/12`; `6,384,000/12`;
   `20,000 × 30`; `(2,724 + 159,953) × 3`; `42,488,032 / 8.383844`.
4. **Result.**

   | Component | Per month (USD) | Share | Three months (USD) |
   |---|---|---|---|
   | Escalation on owner-retained scope | 2,724 | 0.39 % | 8,172 |
   | Extra interest on drawn debt | 159,953 | 23.03 % | 479,860 |
   | Deferred `CFADS` | 532,000 | 76.58 % | 1,596,000 |
   | **Total economic cost** | **694,677** | | **2,084,032** |
   | Delay damages at 20,000/day | 600,000 | 86.37 % recovered | 1,800,000 |
   | **Uncovered, borne by the SPV** | **94,677** | | **284,032** |

   Capitalising the escalation and interest components (488,032) takes debt to **42,488,032**, the
   instalment to **5,067,846.24**, the `DSCR` from 1.2743 to **1.2597**, the covenant cash trigger
   to **6,081,415** and annual headroom to **302,585** — down **69,853**, or **18.8 %**.
5. **Interpretation.** Read the share column first. **Three quarters of the cost of a delay during
   construction is revenue the project will never earn, and almost none of it is escalation** —
   which is precisely the reverse of the intuition a cost engineer brings to the meeting, because
   escalation is the row a cost report shows and deferred revenue is a row it does not contain. The
   0.39 % escalation share is not a general result; it is the *value of the wrap*, and it is
   measurable: on the unwrapped variant of 8.1.2 the escalation row would be **39,045** per month
   rather than 2,724, so the fixed-price contract is worth **36,321 per month of slip** on this row
   alone. The second reading is the damages calibration. At 20,000 per day the regime recovers
   86.37 % of a delay declared at quarter six, but its adequacy moves through the programme: at
   financial close a month of slip costs **552,329** and damages of 600,000 over-recover; at COD it
   costs **742,000** (reconciling exactly to Domain 5's 7,000 + 17,733.33 per day) and they recover
   80.86 %. **The crossing is between quarter two (580,033) and quarter three (604,156)** — after
   which a single flat damages rate is never again sufficient, because the interest row grows
   monotonically as the balance builds while the escalation row shrinks as scope is bought. A
   negotiator who understands that asks for a **stepped** damages rate rather than a higher flat
   one, which is both cheaper for the contractor to accept and better matched to the exposure. The
   third reading is the permanence. 488,032 of capitalised delay cost — a rounding error against a
   60,000,000 envelope — removes **18.8 %** of the covenant headroom for the entire twelve-year loan
   life, on a project that is otherwise exactly as forecast. That is the mechanism Domain 5 named
   and this example generalises: **a construction event becomes an operating constraint the moment
   it is capitalised**, and whether it is capitalised or funded with equity is a choice about
   whether the slip lands on coverage or on equity return. It belongs to the sponsors before the
   event, in the funding documents, not to a project director in the month it happens. Finally, the
   deferred-revenue row deserves the caution Domain 5 attached to it: 1,596,000 of `CFADS` deferred
   is a cash fact, not a value loss of that size, because most of it is postponed rather than
   destroyed — the value calculation (Domain 5, KA 5.4.2) is the right instrument for a damages
   negotiation and the cash calculation is the right instrument for a funding one.

> **Fig 8.4.1 — The cost of a month of slip rises through the build.** Stacked column chart,
> x-axis the point in the construction programme at which the slip is declared (financial close,
> quarters 1–7, COD), y-axis cost of one month of slip in USD (0–800k). Three bands per column:
> deferred `CFADS` (brand blue, 30 % opacity, constant at 532,000), extra interest on the drawn
> balance (ink, rising 9,240 → 210,000) and escalation on owner-retained scope (crimson, falling
> 11,089 → 0). Column totals labelled, rising from **552,329** at close to **742,000** at COD.
> Dashed crimson horizontal line at **600,000**, delay damages of 20,000 per day, crossing the
> total between quarter two (580,033) and quarter three (604,156). Source: PCI original. Alt text:
> nine stacked columns rising steadily from about five hundred and fifty thousand to seven hundred
> and forty-two thousand, with a flat damages line crossing them near the third column.

### 8.4.3 One data spine, two questions

**The principle.** The project controls function and the financing function should read the same
numbers. They will always ask different questions of them, and the interface exists to keep the
numbers common while letting the questions differ. Where two sets of numbers appear — a cost report
for management and a different one for lenders — the organisation has created a reconciliation
burden it will fail at exactly the moment it matters.

**What each side owns.**

| Object | Project controls owns | The financing asks |
|---|---|---|
| Cost breakdown and control accounts | The structure, the coding, the hundred-per-cent rule (PML-AI D4) | Do the accounts map to the funded uses in the sources and uses? |
| Earned value at the data date | `PV`, `EV`, `AC`, `CPI`, `SPI` and their integrity | What is `CTC`, and is the facility in balance? (8.4.1) |
| The `EAC` family | The method and its stated assumption | Which forecast does the technical adviser certify, and what cash call follows? |
| Schedule and forecast COD | The network, the critical path, the float | What is the cost per month of the slip, and who bears it? (8.4.2) |
| Risk register | Identification, probability, impact, response | What contingency does it support at P80, and is the remainder adequate? (8.3.2) |
| Contingency drawn and remaining | The internal draw protocol | Can a draw be certified against the remaining risk? |
| Change control | The baseline and its integrity | Is this a variation under the contract, a scope change requiring consent, or a cost overrun? |

**The monthly reporting pack this domain endorses** is therefore one document with a financing
annexe: certified progress and the draw request; earned value and the three `EAC` forecasts with
their assumptions named; `CTC` and the funds sufficiency position against undrawn facility, unused
contingency and uncalled equity; contingency drawn to date trended against **risk retired** rather
than against time (PML-AI D8, KA 8.3.2 — burning 60 % of contingency while retiring 20 % of the
register is the signal); the register's current P80 and the remaining-adequacy test; forecast COD
with the cost-per-month figure current at that point in the programme; and the covenant
consequence of any capitalised delay or overrun cost. Every one of those lines derives from data
the controls function already holds. **What the financing adds is not data collection; it is three
transformations — `CTC`, sufficiency, and the coverage consequence — and the discipline of stating
which confidence level the provisions represent.**

**The governance point.** The reconciliation must be scheduled, not discovered. A finance leader
who first meets a `CPI` of 0.906 in a certifier's refusal to fund has lost the two months in which
the problem was cheap to fix, and the relationship cost of that surprise is the one Domain 10 (KA
10.4.4) priced: a lender who learns of a problem from a compliance certificate prices the discovery
into everything afterwards.

### AI in this KA

The transformations of 8.4.3 are deterministic and monthly, which makes them ideal automation:
`CTC` under every `EAC` method, the sufficiency test against current facility balances, the register's
P80 recomputed as items retire, and the cost-per-month-of-slip figure recomputed as the drawn balance
moves. Anomaly detection is a second strong use — a `CPI` that steps without an operational cause is
usually a measurement change (PML-AI D7, MCQ 7.2-A), and a machine watching the series sees it before
a monthly reader does. Three prohibitions. It must not **choose the `EAC` method**, because that
choice sets the size of a cash call and is a commercial position requiring an accountable owner. It
must not **certify** — whether a cost is a proper contingency item, whether a variation falls within
the contract, whether remaining contingency is adequate are certifications with contractual
consequences, and Domain 12's boundary applies. And it must not **write the narrative** accompanying
a deteriorating forecast, because a fluent explanation of a variance is exactly what a lender must not
receive without a human who can be questioned about it.

Verification: recompute `CPI = EV/AC` and one `EAC` independently at each data date and require
reconciliation to the automated pack to the dollar; test PML-AI D7's identity (KA 7.3.4) that `TCPI`
to the `BAC/CPI` forecast equals the current `CPI`, as a free check on the forecast engine; confirm by
hand that the register mean behind the automated P80 equals Σ `EMV`; and tie the funds-sufficiency
line to the facility agent's own statement of undrawn commitment, not to the model's memory of it.

### Key terms — KA 8.4

| Term | Meaning |
|---|---|
| **Cost to complete (`CTC`)** | `EAC − AC`; the money still required on a stated forecast method. |
| **Funds sufficiency / in balance** | Funds available (undrawn facility + unused contingency + uncalled equity) ≥ `CTC`. |
| **Cash call** | The equity injection a sufficiency failure requires before the next drawdown. |
| **Contingency adequacy test** | Remaining contingency ≥ P80 of the remaining register; fails independently of `EAC`. |
| **Cost per month of slip** | Escalation on unbought scope + extra interest on drawn debt + deferred `CFADS`. |
| **Stepped damages** | Delay damages that rise through the programme to match a rising exposure. |
| **Data spine** | One set of numbers, two sets of questions; the controls-financing interface. |

### Sample MCQs — KA 8.4

**MCQ 8.4-A `[8.4.1 · Application]`** `BAC` 4,000,000, contingency 300,000, `AC` 2,120,000, `EAC`
on `BAC/CPI` 4,416,667. The funds sufficiency position is:
- A. a surplus of 100,000
- B. a shortfall of 116,667, requiring an injection before the next drawdown ✅
- C. a shortfall of 416,667
- D. in balance, since the `EAC` is within the total funding of 4,300,000

*Rationale:* Available `= 4,300,000 − 2,120,000 = 2,180,000`; `CTC = 4,416,667 − 2,120,000 =
2,296,667`; shortfall 116,667. A is the position on the budgeted-rate forecast; C is the overrun
against `BAC`, not against funding; D is false — 4,416,667 exceeds 4,300,000.

**MCQ 8.4-B `[8.4.1 · Analysis]`** Remaining contingency is 120,000 and the P80 of the remaining
register is 286,185. A cost report shows the forecast inside total funding. The correct reading is:
- A. no action is required, since the forecast is inside funding
- B. the contingency-adequacy test fails by 166,185 and does so independently of any `EAC` forecast ✅
- C. the register must be revised downwards to match the contingency
- D. the shortfall is 120,000

*Rationale:* Adequacy compares remaining provision with remaining exposure and needs only the
register and the draw history; 286,185 − 120,000 = 166,185. A misses the second test entirely; C is
the corruption the test exists to prevent; D subtracts in the wrong direction.

**MCQ 8.4-C `[8.4.2 · Application]`** Debt drawn 31,990,655 at 6.0 %; remaining owner-retained scope
922,906 escalating at 3.6 % per annum; annual `CFADS` 6,384,000. One month of slip costs closest to:
- A. USD 532,000
- B. USD 694,677 ✅
- C. USD 162,677
- D. USD 730,998

*Rationale:* `922,906 × (1.036^(1/12) − 1) = 2,724`; `31,990,655 × 0.06/12 = 159,953`;
`6,384,000/12 = 532,000`; total 694,677. A is the deferred revenue alone; C omits the revenue row;
D is the unwrapped variant, where escalation applies to the whole remaining scope (39,045).

**MCQ 8.4-D `[8.4.2 · Analysis]`** Flat delay damages of 20,000 per day recover 86.37 % of the cost
of a month's slip at quarter six, over-recover at financial close and recover 80.86 % at COD. The
best negotiating response is:
- A. accept the rate — it recovers most of the cost
- B. seek a higher flat rate calibrated to the COD figure
- C. seek a stepped rate rising through the programme, since the exposure rises monotonically as the drawn balance builds ✅
- D. remove damages and rely on contingency

*Rationale:* A flat rate cannot fit a cost that moves 34.3 % across the programme; a stepped rate
matches exposure and is cheaper for a contractor to accept than a flat rate set at the maximum. A
ignores the tail; B over-recovers early and is resisted for that reason; D abandons the transfer
altogether.

**MCQ 8.4-E `[8.4.1 · Evaluation]`** At Auriga's week-13 data date the facility is in balance by
+100,000 on the budgeted-rate forecast, out of balance by 116,667 on `BAC/CPI` and by 308,056 on
`CPI × SPI`; separately, remaining contingency of 120,000 stands against a remaining-register P80 of
286,185. All four courses below have been proposed. Which should the finance leader take?
- A. argue for the budgeted-rate `EAC` of 4,200,000, which leaves the facility in balance with 100,000 to spare
- B. accept `BAC/CPI`, prepare the 116,667 cash call, and treat the contingency position as a consequence of it
- C. take both tests to the sponsor now, leading with the 166,185 contingency shortfall, because that test rests on the register and the draw history alone and is therefore due whichever `EAC` is certified ✅
- D. revise the remaining register down so that the 120,000 still available is adequate

*Rationale:* the adequacy test needs no forecast at all and fails independently and earlier, so it is
the finding that survives whatever the certifier accepts about the `EAC` — and it exposes the real
defect, a project funded at sanction to a P53.5 provision nobody named (8.4.1). A is the sponsor's
negotiating position and is defensible as a position, but a `CPI` of 0.906 sustained over thirteen
weeks is evidence about the remaining work unless somebody can name what has changed, and it leaves
the 166,185 untouched. B is the right answer to the second test first: it makes the cash call turn on
a forecast method that is negotiable while the register shortfall is not. D is the corruption the
adequacy test exists to prevent.

**MCQ 8.4-F `[8.4.2 · Evaluation]`** A three-month slip is declared at construction quarter six.
Capitalising its escalation and interest components — 488,032 — takes debt to 42,488,032, the instalment
to 5,067,846.24, the `DSCR` from 1.2743 to 1.2597 and annual headroom from 372,438 to 302,585, a fall of
18.8 % for the whole twelve-year loan life. The soundest position is that:
- A. it should be capitalised: 488,032 is a rounding error against a 60,000,000 envelope
- B. this is a choice about whether the slip lands on coverage for twelve years or on equity return now,
  and it belongs to the sponsors before the event, in the funding documents ✅
- C. it should be funded with equity in every case, because lenders do not permit delay costs to be
  capitalised
- D. the decision should be left to the project director in the month the slip is declared, when the
  amount is known

*Rationale:* a construction event becomes an operating constraint the moment it is capitalised (8.4.2,
and Domain 5 KA 5.4.2). A prices the cash and ignores twelve years of coverage. C presents as universal
a lender position that is a negotiated term of the funding documents. D leaves a structural choice to
the person with the least room to negotiate it and the strongest reason to close it out quickly — which
is how the answer gets made by default rather than chosen.

**MCQ 8.4-G `[8.4.3 · Comprehension]`** A controls function already produces earned value, the `EAC`
family, the register and the contingency draw history. What the financing adds to that monthly pack is:
- A. a second set of numbers, prepared to the lenders' definitions
- B. three transformations of the same data — cost to complete, the funds sufficiency position, and the
  coverage consequence — plus the discipline of stating which confidence level each provision
  represents ✅
- C. an independent estimate of the cost to complete, prepared by the lenders' technical adviser
- D. the same information recast onto a different cost breakdown structure

*Rationale:* one data spine, two sets of questions: every line the financing annexe needs derives from
data the controls function already holds (8.4.3). A is the failure the principle exists to prevent — two
sets of numbers create a reconciliation burden that fails exactly when it matters. C describes a
diligence activity, not what the pack adds. D is a coding exercise that answers neither question the
financing asks.

### Self-check — KA 8.4

1. *What does a lender compute from an `EAC` that a controller does not?* — `CTC = EAC − AC`, then
   funds sufficiency against undrawn facility, unused contingency and uncalled equity — producing a
   cash call with a date rather than a forecast range.
2. *Why was Auriga's shortfall predictable at sanction?* — The 300,000 funded was a P53.5 provision
   against a register whose P80 was 490,624; the 416,667 overrun is a P70.8 event, normal behaviour
   against a provision nobody had priced.
3. *Which row of the slip cost is largest, and which is the wrap worth?* — Deferred `CFADS` at
   76.58 %; the wrap saves 36,321 per month on the escalation row (2,724 rather than 39,045).

---

## Advanced topics — Domain 8

### 8.A.1 Correlation, and why the independent P80 is the optimistic one

The aggregation of 8.3.2 assumes independence, and construction risks are systematically not
independent: a single adverse ground condition drives geotechnical cost, programme, dewatering and
third-party interface simultaneously; a single supply-chain event moves several package prices at
once. The consequence is directional and knowable. Variance under positive correlation is
`Σ σᵢ² + 2 Σᵢ<ⱼ ρᵢⱼ σᵢ σⱼ`, so any positive `ρ` raises σ and therefore raises the P80, while the
mean is unchanged — **correlation never changes what you expect to pay and always changes what you
must provide.** On Kestrel's register, treating C1 and C2 as correlated rather than independent
raises the provision without touching the 2,690,000 mean, which is why a P80 quoted without a
statement about correlation should be read as a lower bound. Practically, three habits: name the
common drivers (ground, weather, a shared supplier, a single approval authority) and group the
items that share them; run the simulation with a correlation matrix rather than an independence
assumption; and where the matrix cannot be defended, state the independence assumption explicitly
in the paper rather than letting the reader infer sophistication that is not there.

### 8.A.2 Contingency drawdown as a monitored curve

A funded contingency should be *expected* to deplete, and the useful control is not its balance but
the relationship between two curves: contingency consumed and register retired. Plot both as
percentages of their sanction values on the same axes and the diagnosis is immediate. Consumption
tracking retirement is a healthy project spending its provision on the risks it provided for.
Consumption ahead of retirement is the classic overrun signature — money going out against events
that were not in the register, which means either the register was incomplete or the draws are not
proper contingency items. Retirement ahead of consumption is the release case of 8.3.3, worth
1,315,141 on Kestrel when C1 closed, and it is the case organisations handle worst: released
contingency is rarely given back, because nobody is rewarded for returning money. The financing
answer is to write the release into the documents — a defined recalculation of the required
provision at stated milestones, with the excess applied to prepayment or released to distribution.
That converts a behavioural problem into a mechanical one.

### 8.A.3 The reviewer's cost-and-contingency eye

Testable invariants for any construction cost and contingency model:

- Every contingency percentage is accompanied by the **estimate class** of the base it is a
  percentage of; a percentage without a class is unreviewable (8.1.2).
- The four cost families are separately identifiable, and operating cost appears **only** inside
  `CFADS`, never in the sources and uses (8.1.1).
- Lifecycle cost is present, dated, escalated at the same rate as operating cost, and matched by a
  reserve whose deposit schedule funds the **first** overhaul before it falls due (8.1.3).
- Capitalised interest reproduces the **area rule** `r_q × g × S × Σ cum(t−1)` to the dollar; a
  mismatch localises the defect to rate, balance convention, draw timing or gearing (8.2.2).
- Escalation is applied **period by period to the profile**, never to a total; escalation on
  fixed-price wrapped scope is zero and on retained scope is not (8.2.3).
- Every escalation rate names an index, a source, a base date and a human owner, and construction and
  revenue escalation are distinct assumptions; contingency states a **confidence level**, the
  register's mean equals Σ `EMV`, and the independence assumption is stated or a correlation matrix is
  documented (8.2.3, 8.3.2, 8.A.1).
- Range-based and register-based provisions are both computed, the higher is taken, and the
  contracting structure that eliminates one of them is named (8.3.2).
- Contingency consumed is trended against **risk retired**, not against elapsed time (8.A.2).
- `CTC = EAC − AC` on a **named** `EAC` method, and the funds sufficiency line ties to the facility
  agent's undrawn commitment, not to the model (8.4.1).
- The contingency-adequacy test is run separately from the sufficiency test, because it fails
  earlier and independently (8.4.1).
- Delay cost has three rows, and the interest row uses the **balance drawn at the date of the
  slip**, not the full facility (8.4.2).
- Any capitalised overrun or delay cost is carried through to the instalment, the `DSCR`, the
  covenant trigger in cash and the headroom (8.3.2, 8.4.2, Domain 10).

---

## Industry variations — Domain 8

- **Water and desalination.** Short, well-characterised membrane and dosing cycles make 8.1.3's
  arithmetic unusually reliable and the maintenance reserve unusually large as a share of `CFADS` —
  10.10 % on Kestrel. Intake and marine works carry the ground and weather-standby risks that dominate
  the retained register, so the contingency negotiation is about who owns the geotechnical basis.
- **Solar and wind generation.** Capital cost is modular and heavily benchmarked, so ranges narrow
  earlier than in process plant and Stage D provisions are genuinely thin. The estimating battleground
  moves to grid connection and civil works, where one interface can carry more range than all the
  generating equipment; lifecycle cost turns on the *timing* of a few component replacements rather
  than their price.
- **Transport concessions.** Earthworks and structures make ground risk the largest line and quantity
  growth against a Stage C estimate the norm, so unwrapped procurement forces 8.3.2's range-based
  provision rather than the register-based one. Long construction periods magnify both the escalation
  exposure and the shape effect of 8.2.2.
- **Digital infrastructure.** Fit-out is phased against demand, so the profile is a sequence of
  S-curves and the area rule must be applied per phase. Short equipment lifecycles compress the tail
  and make the lifecycle charge a far larger share of operating economics than in civil assets.
- **Process industry and petrochemicals.** Estimate classes matter most here, because scope definition
  genuinely drives outturn and the ladder's factor of 6.25 is not theoretical. Long-lead equipment
  pulls escalation exposure early, pushing 8.2.3's breakeven towards front-loading; turnaround and
  inspection cycles drive lifecycle cost and are regulated in many jurisdictions, so their timing is
  less discretionary than elsewhere.
- **Social infrastructure and availability PPPs.** Capital cost is comparatively predictable and the
  lifecycle obligation *is* the commercial substance: a 25- or 30-year hard-facilities obligation with
  a handback condition, priced at bid and unalterable afterwards. Getting 8.1.3 wrong here is not a
  reserve problem; it is a bid that cannot be performed.

---

## Case study — Domain 8: the contingency that was a percentage (transport)

**Situation.** A sponsor group bid a 34-kilometre highway concession in an emerging market. The
capital estimate at bid was a **Stage C** feasibility estimate of **USD 420,000,000**, carrying a
stated accuracy range of **−15 % / +30 %**. The financing envelope allowed contingency at the
group's standing policy of **8 % of base cost — 33,600,000** — and the works were let as **four
separate packages** with the concessionaire managing interfaces, because splitting the packages had
produced the lowest bid prices.

**What happened.** The two decisions were incompatible and the incompatibility was computable at
bid. An 8 % provision is a Stage E number (8.1.2); the project had a Stage C estimate and, having
declined a wrap, had retained the base-estimate uncertainty the range describes. Translating the
range through a triangular distribution with minimum 357,000,000, mode 420,000,000 and maximum
546,000,000 gives a **P80 outturn of 476,986,958 — a contingency requirement of 56,986,958, or
13.57 % of base**. The funded 33,600,000 was a **P64.1** provision against the range
(`F = 1 − (92,400,000)²/((189,000,000)(126,000,000)) = 0.6415`). Twenty-one
months into a forty-month programme, earthworks quantities on two of the four packages exceeded the
bid quantities, the interface between the earthworks and structures packages produced a sequence of
compensation events that neither contractor owned, and cumulative certified cost reached
**252,000,000** against an earned value of **231,000,000** — a `CPI` of **0.9167**. The
`BAC/CPI` forecast was **458,181,818**, an overrun of **38,181,818** against a base of 420,000,000
and **4,581,818** more than the entire funded contingency.

**How it resolved.** The facility's in-balance test failed at the next certification: funds
available were `453,600,000 − 252,000,000 = 201,600,000` against a cost to complete of
`458,181,818 − 252,000,000 = 206,181,818` — a **4,581,818** shortfall, and the technical adviser
could not certify a further contingency draw because, with **26,200,000** of the 33,600,000 already
drawn, remaining contingency was **7,400,000** against a remaining register whose P80 exposure was
assessed at **19,300,000**. Drawdown stopped. The sponsors funded the shortfall as additional equity
of **20,000,000** — deliberately above the 4,581,818 minimum, because a cash call computed to the
last dollar fails again on the next report — and accepted a tightened reporting regime and a
lenders' quantity surveyor with access to the packages. Against an original equity of 113,400,000 on
a 75/25 structure, the injection **increased the sponsors' equity base by 17.6 %** and moved gearing
to **71.8/28.2**, diluting the modelled equity return correspondingly.

**What the domain teaches here.** The contingency was not too small by accident; it was sized by a
method that could not see the estimate class or the procurement decision. Both defects of 8.3.3 are
visible: the 8 % rule stated no confidence level, so nobody noticed it was a P64.1 provision against
a distribution its own estimate had published, and it did not respond when the decision to split the
packages transferred the base-estimate uncertainty back to the owner. The cost of the arithmetic
nobody did at bid was 20,000,000 of equity — 17.6 % of the sponsors' committed capital — and the
whole of it was computable from two documents the sponsors already held: the estimate's stated range
and the procurement strategy. **The provision was 23,386,958 short of the P80 its own estimate
implied, and the eventual call was 20,000,000.** The arithmetic was not merely available; it was
approximately right.

## Case study B — Domain 8: the release nobody wanted to take (solar generation)

**Situation.** A 180 MW solar project reached financial close with a **Stage D** capital estimate of
**USD 148,000,000**, a funded contingency of **11,100,000** (7.5 % of base) and a retained risk
register of eleven items with a mean exposure of **6,240,000**, a standard deviation of **4,050,926**
and a **P80 of 9,649,259**. Two items dominated: a grid-connection energisation risk (p 0.35, impact
6,000,000) and a land-title consolidation risk (p 0.25, impact 4,800,000), together carrying
**3,300,000** of the mean and, on their own, **3,536,948** of standard deviation — 76.2 % of the
register's total variance from two of eleven lines. The remaining nine carried a mean of 2,940,000
and a standard deviation of **1,974,842**. Contingency was drawable on certification against a
**0.60 % per annum** commitment fee on the undrawn balance.

**What happened.** Fourteen months in, both dominant items retired cleanly: the connection agreement
was energised on schedule and the final title parcels were consolidated. Recomputing the register
without them gave a mean of **2,940,000**, a standard deviation of **1,974,842** and a **P80 of
4,602,027** — the P80 requirement had fallen by **5,047,232**, more than half, on a single month's
news. Against 11,100,000 of contingency of which **1,800,000** had been drawn, the project was
holding an undrawn commitment of **9,300,000** against a P80 requirement of 4,602,027 — an excess of
**4,697,973**. Nobody proposed releasing it. The project director's position was that contingency
should not be reduced while construction was still running; the sponsor's finance
function did not challenge it, because the visible cost of the excess was only the commitment fee —
**28,188 a year** — which no operating budget felt.

**How it resolved.** The lenders' technical adviser raised it at the next quarterly review, and the
facility's contingency-recalculation clause was invoked at the second construction milestone. The
required provision was reset to the P80 of the live register plus a **15 % correlation uplift** —
**5,292,331** — and the difference between the undrawn balance and that figure, **4,007,669**, was
cancelled from the commitment. The fee saving was modest: **36,069** over the remaining eighteen
months. The real prize was what cancellation made impossible. Had the excess instead been drawn —
which is what available money tends to become — it would have added **2,805,368** of senior debt at
70 % gearing, and at the facility's 5.4 % over a 15-year tenor (`AF(0.054, 15) = 10.104624`) that is
**277,632** of additional annual debt service for fifteen years, against a project whose entire
contingency provision was 11,100,000.

**What the domain teaches here.** The cost of holding excess contingency while it is undrawn is
trivially small, which is exactly why it is never challenged; the cost if it is drawn is permanent,
and it lands on the coverage ratio rather than on any construction budget. That asymmetry — 36,069
of fee against 277,632 a year of debt service — is the whole case for a *mechanical* release. The
15 % correlation uplift is the honest form of 8.A.1: the release was taken, and the independence
assumption was not pretended to. And the governance failure is the general one — released
contingency requires a mechanism, because no individual is rewarded for handing money back. Write
the recalculation into the documents at defined milestones and the release happens; leave it to
judgment and the excess is carried to COD every time, and then spent.

---

## Executive perspective — Domain 8

What a project finance director cannot delegate in this domain:

- **The estimate class behind every contingency percentage.** Not the percentage — the class. A
  7.59 % provision is prudent at Stage E and negligent at Stage C, and the director is the only
  person in the room who sees both the estimate and the procurement decision that sets which one
  applies (8.1.2).
- **The link between the contracting strategy and the contingency line.** They are one decision
  taken twice, usually by different people in different months. Whoever declines a wrap has just
  resized the contingency, and someone must say so out loud before the envelope is fixed
  (8.1.2, Case study A).
- **The confidence level, named.** P80 or P50 is a risk-appetite decision belonging to governance,
  not to an analyst and never to a model. A provision whose confidence nobody stated is a provision
  nobody chose — Auriga's 300,000 was P53.5, and behaving normally broke it (8.4.1).
- **The lifecycle obligation and the reserve that funds it.** The director owns the question "what
  does this asset owe in year seven, and is the money going to be there?" A `DSCR` of 0.4561 in one
  year is a payment default on an otherwise healthy project, and it is entirely a reserve design
  question (8.1.3).
- **The release mechanism.** Contingency held after its risk has retired costs almost nothing while
  it is undrawn — 11,214 a year on Kestrel, 28,188 on Case study B — which is exactly why nobody
  challenges it, and a great deal if it is then spent: 50.3 % of Kestrel's covenant headroom, or
  277,632 a year of debt service for fifteen years in Case study B. The director's job is to have
  the recalculation written into the documents before anyone has to be brave (8.A.2, Case study B).
- **The interface, scheduled rather than discovered.** One data spine, two questions. A director who
  first meets a `CPI` of 0.906 in a certifier's refusal to fund has lost the months in which it was
  cheap, and has paid the relationship cost Domain 10 priced (8.4.3).

## Calculation exercises — Domain 8

**Exercise 8.1** A Stage C estimate of **30,000,000** carries a range of **−15 % / +30 %**. Using a
triangular distribution, compute the mean outturn, the P80 outturn and the contingency each implies;
then state the percentile a 10 %-of-base rule buys.
*Solution.* `a = 25,500,000`, `m = 30,000,000`, `b = 39,000,000`. Mean `= (a + m + b)/3 =
31,500,000` → contingency **1,500,000 (5.00 %)**. P80 solves `(b − x)² = 0.20 (b − a)(b − m) =
0.20 × 13,500,000 × 9,000,000 = 2.43 × 10¹³` → `b − x = 4,929,503` → `x =` **34,070,497**,
contingency **4,070,497 (13.57 %)**. The 10 % rule gives 3,000,000, i.e. `x = 33,000,000`;
`F = 1 − (6,000,000)²/(1.215 × 10¹⁴) = 0.7037` → **P70.4**. *Common error:* assuming the percentile
depends on project size — it does not. The percentile a percentage rule buys is a function of the
*range* alone, which is why −15/+30 always returns 13.57 % at P80 and P70.4 for a 10 % rule,
whatever the base.

**Exercise 8.2** **30,000,000** of spend over six quarters, **60 %** debt-funded at **8 %** per annum
(2 % per quarter) on opening balances, draws at period end. Profile as planned: 10, 15, 20, 25, 18,
12 per cent. Compute capitalised interest, and the interest under the reversed profile 12, 18, 25,
20, 15, 10.
*Solution.* `r_q × g × S = 0.02 × 0.60 × 30,000,000 = 360,000` per unit of area. As planned,
Σ cum(t−1) `= 0 + 10 + 25 + 45 + 70 + 88 = 238 %` → 2.3800 → IDC **856,800**. Reversed,
`0 + 12 + 30 + 55 + 75 + 90 = 262 %` → 2.6200 → IDC **943,200**. Difference **86,400** on identical
total spend and duration. *Common error:* computing interest on the *closing* balance, which adds one
period of interest to every draw and overstates IDC by `r_q × g × S = 360,000` — an error of 42 % on
the correct figure, and the single commonest defect in construction models.

**Exercise 8.3** A register carries p/impact pairs 0.30/1,500,000 · 0.45/800,000 · 0.20/2,200,000 ·
0.25/1,000,000, plus an opportunity 0.20/(500,000), on a base estimate of 22,000,000. Compute the
mean, σ and P80, and state the percentile bought by an 8 %-of-base rule.
*Solution.* Mean `= 450,000 + 360,000 + 440,000 + 250,000 − 100,000 =` **1,400,000**. Variance
`= 0.21×1,500,000² + 0.2475×800,000² + 0.16×2,200,000² + 0.1875×1,000,000² + 0.16×500,000² =
1,632,800,000,000`; σ **1,277,811**; P80 `= 1,400,000 + 0.8416 × 1,277,811 =` **2,475,405**. The 8 %
rule gives **1,760,000**, i.e. `z = 0.2817` → **P61.1**, leaving a **715,405** shortfall against P80.
*Common error:* omitting the opportunity from the variance. It reduces the mean by 100,000 but *adds*
`0.20 × 0.80 × 500,000² = 40,000,000,000` to the variance, because an opportunity is an uncertain
outcome like any other — dropping it understates σ and therefore the P80.

**Exercise 8.4** A project declares a one-month slip with **18,000,000** of debt drawn at **7.0 %**,
**2,400,000** of owner-retained remaining scope at then-current prices escalating at **4.2 %** per
annum, and annual `CFADS` of **4,200,000**. Delay damages are 12,000 per day (30-day month). Compute
the cost of the month, the damages coverage, and the share of cost a revenue-only calibration would
have missed.
*Solution.* Monthly escalation factor `1.042^(1/12) = 1.00343438`, i.e. 0.3434 %; escalation row
`2,400,000 × 0.00343438 =` **8,243**. Interest row `18,000,000 × 0.07/12 =` **105,000**. Revenue row
`4,200,000/12 =` **350,000**. Total **463,243**. Damages `12,000 × 30 = 360,000` → **77.71 %**
coverage; uncovered **103,243**. A calibration on forgone revenue alone targets 350,000 and misses
`(8,243 + 105,000)/463,243 =` **24.45 %** of the cost. *Common error:* using the full facility amount
rather than the balance actually drawn at the date of the slip in the interest row — the error grows
through the programme and is largest exactly where the delay exposure is largest.

**Exercise 8.5** `BAC` **30,000,000**, funded contingency **2,400,000**, at the data date `AC`
**16,800,000**, `EV` **15,600,000**, `PV` **16,000,000**. Compute `CPI`, `SPI`, the three `EAC`
forecasts, `CTC` and the funds sufficiency position on each.
*Solution.* `CPI = 15,600,000/16,800,000 = 0.928571`; `SPI = 15,600,000/16,000,000 = 0.975000`.
Available `= 32,400,000 − 16,800,000 =` **15,600,000**. (a) `EAC = 16,800,000 + 14,400,000 =
31,200,000`, `CTC` **14,400,000**, surplus **+1,200,000**. (b) `EAC = 30,000,000/0.928571 =
32,307,692`, `CTC` **15,507,692**, surplus **+92,308**. (c) `EAC = 16,800,000 +
14,400,000/(0.928571 × 0.975) = 32,705,325`, `CTC` **15,905,325**, shortfall **(305,325)**.
*Common error:* reporting the (b) result as "in balance" without qualification. A surplus of 92,308
is **0.28 %** of the facility — inside the rounding of a monthly report, not a margin — and on the
`CPI × SPI` forecast the same project is 305,325 short. Sufficiency conclusions must be stated with
the method that produced them.

## Practitioner's toolkit — Domain 8

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 8.T.1 — Contingency basis note (one page, per sanction and per recalculation)

One page that must exist before any contingency figure is quoted. **Base estimate**: amount, base
date, price basis, preparer. **Estimate class**: stage, accuracy range, and the engineering
deliverables complete that justify the class, with a named owner. **Procurement**: wrapped or
unwrapped, package count, interface owner — and the explicit statement of which uncertainties the
contract transfers. **Range-based provision**: distribution used, parameters, mean, P50, P80, P95.
**Register-based provision**: item count, mean = Σ `EMV`, σ, P80, P95, independence assumption or
correlation matrix. **The reconciliation**: which of the two governs and why. **The funded figure**:
amount, percentage of base, and the percentile it represents on the governing basis. **Contingent
support**: amount, form, trigger, obligor, enforceable against what balance sheet. **Approval**:
confidence level chosen, by whom, on what date. Rule: no contingency number leaves the organisation
without this page attached, and the page is reissued at every recalculation milestone.

### Toolkit 8.T.2 — Construction cost model check (before any envelope is fixed)

- [ ] Four cost families separately identified; operating cost appears only inside `CFADS`.
- [ ] Base date stated; every escalation rate names an index, a source and a human owner.
- [ ] Escalation applied period-by-period to the profile; zero on wrapped scope, non-zero on retained.
- [ ] Construction and revenue escalation are distinct assumptions.
- [ ] IDC reproduces the area rule `r_q × g × S × Σ cum(t−1)` to the dollar.
- [ ] Interest convention stated (opening / average balance) and paired with the period length.
- [ ] Balancing line identified; it is not a round number; it is tested against 8.T.1's percentile.
- [ ] Lifecycle programme dated, escalated and matched by a reserve deposit schedule that funds the first overhaul before it falls due.
- [ ] Handback or decommissioning obligation identified, with its cash date relative to loan maturity.
- [ ] Sensitivity run on: escalation rate, spend profile shape, contingency drawn, and COD date.
- [ ] Every capitalised overrun scenario carried through to instalment, `DSCR`, covenant trigger in cash and headroom.

### Toolkit 8.T.3 — Monthly construction pack, financing annexe

One annexe to the existing controls report — never a second set of numbers. Rows: certified progress
and this month's draw request against the funding profile · `PV`, `EV`, `AC`, `CPI`, `SPI` at the data
date · the three `EAC` forecasts with assumptions named, and the method the certifier is using ·
**`CTC` and the funds sufficiency position** against undrawn facility, unused contingency and uncalled
equity, tied to the agent's statement · contingency drawn and remaining, trended **against register
retired** rather than against time · the register's current P80 and the remaining-adequacy test ·
forecast COD and the cost per month of slip *current at this point in the programme*, in three rows ·
damages coverage at the current exposure · the `DSCR`, covenant trigger and headroom consequence of any
capitalised cost · exceptions, each with a named accountable person. Front line, always: **is the
facility in balance, and is the remaining contingency adequate for the remaining risk?**

## Exam preparation — Domain 8

**What is assessed.** The four cost families and where each enters a financing; estimate classes and
the contingency a range implies at a stated confidence; why a fixed-price wrap changes the defensible
contingency; whole-life cost, the level annual charge and the reserve that prevents a lifecycle payment
default; the area rule and the price of shape; escalation applied to a profile, base dates and the
shape-neutrality breakeven; contingency sized from a register and reconciled against a range-based
provision; the two computable defects of the percentage method; contingent support and why it is worth
less than funded cash; the conversion of an `EAC` into a `CTC`, a sufficiency test and a cash call; the
contingency-adequacy test as a separate and earlier failure; and the three components of a month's slip,
their movement through the programme, and the coverage consequence of capitalising any of them.

**The calculations to do under time pressure.** Contingency implied by an accuracy range at the upper
bound and at P80 via a triangular distribution (8.1.2, 8.3.2, Exercise 8.1) · the percentile a
percentage rule buys, on both bases (8.3.3) · present value and level annual charge of a lifecycle
programme, and the year-of-overhaul `DSCR` without a reserve (8.1.3) · IDC from the area rule, and the
same profile reversed (8.2.2, Exercise 8.2) · escalated spend period-by-period, and the shape-neutral
escalation rate `g × r` (8.2.3) · register mean, variance, σ, P80 and P95, including opportunities
(8.3.2, Exercise 8.3) · standby support as P95 (5,731,375) less funded contingency (8.3.1) · `CTC` and funds
sufficiency on all three `EAC` methods (8.4.1, Exercise 8.5) · remaining-register P80 against
remaining contingency (8.4.1) · cost per month of slip in three rows, damages coverage, and the
`DSCR`/headroom consequence of capitalising it (8.4.2, Exercise 8.4).

**The traps.**

- Quoting a contingency percentage without the estimate class of its base — the domain's central
  defect (8.1.2, MCQ 8.1-A).
- Applying a Stage E provision to a Stage C estimate after declining a wrap (8.1.2, MCQ 8.1-B, Case
  study A).
- Putting operating cost in the sources and uses, or forgetting that an operating error recurs every
  year against a fixed headroom (8.1.1, MCQ 8.1-D).
- Omitting lifecycle cost, or funding it at a policy percentage rather than against the next
  overhaul's date (8.1.3, MCQ 8.1-C).
- Escalating a total rather than the profile — 1,424,815 of overstatement on Kestrel (8.2.3,
  MCQ 8.2-B).
- Computing IDC on closing rather than opening balances — a 42 % overstatement in Exercise 8.2
  (8.2.2, Exercise 8.2).
- Summing worst cases, or funding the mean, and calling either a contingency (8.3.2, MCQ 8.3-A).
- Dropping opportunities from the variance while keeping them in the mean (Exercise 8.3).
- Presenting an independent P80 as conservative when correlation makes it a lower bound (8.A.1).
- Treating contingent support as equivalent to funded contingency (8.3.1, MCQ 8.3-D).
- Holding contingency after its risk has retired, and pricing it as the commitment fee (11,214 a
  year on Kestrel) rather than as the coverage loss if it is drawn — 50.3 % of headroom (8.3.3,
  MCQ 8.3-C, Case study B).
- Running the sufficiency test and not the contingency-adequacy test, which fails earlier and
  independently (8.4.1, MCQ 8.4-B).
- Pricing a delay on forgone revenue alone — 24.45 % of the cost missed in Exercise 8.4 — or using
  the full facility rather than the drawn balance in the interest row (8.4.2, MCQ 8.4-C).
- Accepting a flat damages rate against an exposure that moves 34.3 % across the programme (8.4.2,
  MCQ 8.4-D).
- Reporting a 0.28 %-of-facility surplus as "in balance" without naming the `EAC` method
  (Exercise 8.5).

**How the domain connects.** Backward: Domain 3 supplied the compounding and `AF(r, n)` this domain
escalates and annuitises with; Domain 4 supplied the `EAV` machinery behind the level lifecycle
charge and the appraisal rate; Domain 5 supplied the EPC wrap that makes a thin contingency
defensible and the at-COD delay arithmetic this domain generalises to the whole programme; Domain 6
supplied the sources-and-uses statement, the funding profile and the 3,645,403 balancing line this
domain finally justifies; Domain 7 supplied the revenue escalation that must not be confused with
cost escalation. Forward: Domain 9 supplies the standby tranches and concessional money that fund
the contingent support of 8.3.1; Domain 10 tests every coverage consequence computed here; Domain 11
allocates the risks the register enumerates and Domain 12 documents the wrap that transfers them;
Domain 13's model audit checks the area rule and the escalation basis; **Domain 14 is this domain in
operation** — draw requests, cost-to-complete reports and contingency certification, month by month.
Across the suite: PML-AI Domain 6 owns the schedule that produces the S-curve, PML-AI Domain 7 owns
the earned value this domain transforms, and PML-AI Domain 8 owns the register and the confidence-
level arithmetic. **The single most useful idea to carry between the two books is that a forecast and
a funding position are different objects, and the transformation between them — `CTC`, sufficiency,
coverage — is the finance leader's contribution to a monthly report they did not write.**

## Domain 8 summary
A contingency percentage is meaningless without the estimate class it was struck against. On a
48,000,000 base, the same estimate implies a provision of 24,000,000 at screening and 3,840,000 at
control-estimate maturity — a factor of 6.25 — so Kestrel's funded **3,645,403 (7.59 %)** is
defensible only because a fixed-price, date-certain EPC wrap moved a Stage C estimate to a Stage E
position; procured unwrapped, the same works need of the order of **14,400,000** and a 60,000,000
envelope does not fund them. Lifecycle cost is the family most often missed: Kestrel's membrane and
pump programme is worth **6,881,021** in present value, **644,606** as a level annual charge and
**10.10 %** of `CFADS`, and without a reserve the year-seven `DSCR` is **0.4561** — a payment
default, not a breach, on an otherwise healthy project. The schedule prices the financing through
its shape: capitalised interest obeys the area rule `IDC = r_q × g × S × Σ cum(t−1)`, so the same
48,000,000 over the same eight quarters costs **1,345,680** back-loaded, **1,607,760** on the
S-curve and **2,000,880** front-loaded — a **655,200** spread — while escalation runs the other way
by **574,146**, leaving a total spread of only **86,681** and a shape-neutral escalation rate of
**4.1659 %**, almost exactly the heuristic `g × r = 4.20 %`. Contingency sized from the retained
register (mean **2,690,000**, σ **1,848,973**) supports **4,246,095** at P80, against **6,512,795**
from the Stage C range at P80: two defensible numbers 2,266,700 apart, reconciled by taking the
higher and naming the contract that eliminated the other. The funded amount is a **P69.7** provision
and **600,692** short of the register's P80, which capitalised would take the `DSCR` to **1.2564**
and headroom to **286,459**, down **23.1 %**, for twelve years. The percentage method fails in two
computable ways: 10 % of base is **4,800,000**, a **P87.3** promise against the register and a
**P70.4** promise against the range — the rule cannot say which — and it does not move when risk
retires, leaving **1,869,045** of excess after one item closes, which costs **11,214** a year while
undrawn and, if drawn, adds 1,308,332 of senior debt, takes the `DSCR` to **1.2358** and removes
**50.3 %** of covenant headroom for twelve years.
Finally, the bridge: a controller's `EAC` family becomes a lender's cash call. Auriga's three
forecasts give costs to complete of 2,080,000, 2,296,667 and 2,488,056 against 2,180,000 available —
in balance, then **116,667** short, then **308,056** short — while the contingency-adequacy test
fails independently and earlier, 120,000 remaining against a remaining P80 of **286,185**; and the
shortfall was predictable at sanction, because a **P53.5** provision met a **P70.8** event. A month
of slip declared at quarter six costs **694,677** — 0.39 % escalation, 23.03 % interest, 76.58 %
deferred revenue — rising to **742,000** at COD, so a flat damages rate of 20,000 per day covers the
cost only until quarter three; capitalising three months of the cost components removes **18.8 %** of
Kestrel's covenant headroom permanently. Domain 9 funds the contingent support this domain sizes;
Domain 10 tests every ratio it moves; Domain 14 runs it monthly.
