# Domain 7 — Revenue, Demand and Commercial Models

## Why this domain exists

Domains 5 and 6 left one question open. Domain 5 established that a bankable project needs a
creditworthy payer under a contract of adequate tenor (KA 5.3.2); Domain 6 built the model that
turns a revenue forecast into coverage; Domain 10 sized debt from `CFADS` and a target coverage
ratio and treated both as given. None of them addressed how the **shape** of the revenue promise
(its architecture, not its expectation) determines how much debt exists to be sized. This domain
shows where `CFADS` and the target ratio come from, and the answer changes the negotiation.

The central claim is a single sentence with expensive consequences: **debt is sized on the worst
period a lender will underwrite, so the distribution of revenue matters more than its mean, and
two commercial structures with identical expected cash can differ in debt capacity by a quarter
of the facility.** Everything in the domain is a corollary. The contracted-to-merchant spectrum
(KA 7.1) is a spectrum of *dispersion*, priced by lenders in required coverage before it is
priced in a tariff. Demand-based and concession structures (KA 7.2) put the dispersion inside a
payment mechanism whose banded, floored and shared forms are risk-transfer instruments in tariff
clothing. Escalation and volume risk (KA 7.3) are the two ways a structure that looked adequate
at close stops being adequate later (one slowly and arithmetically, the other quickly and
non-linearly). And counterparty credit and stress testing (KA 7.4) make the rest conditional: a
contracted stream is worth the payer's ability to pay, and concentration is invisible to the
expected-loss calculation a credit committee will nevertheless be shown.

**Learning objectives.** After this domain a candidate can: place a revenue structure on the
contracted–merchant spectrum and state the coverage consequence of its position; distinguish
capacity, energy, availability, take-or-pay and merchant mechanisms and compute revenue under an
availability-deduction formula; demonstrate that two structures with identical expected `CFADS`
support materially different debt, and size each correctly; distinguish sponsor, bank and downside
cases and model a ramp; compute revenue under banded, floored and shared mechanisms and value the
risk transfer they effect; compute effective escalation rates on partly indexed tariffs and cost
bases and quantify the resulting margin drift; compute the degree of operating leverage and the
`CFADS` elasticity to price and to volume and explain why they differ; convert a coverage
threshold into a tolerance in each driver a business manages; compute expected loss as probability
of default times exposure times loss given default, and demonstrate why it is blind to
concentration while the loss distribution is not; price credit enhancement against the coverage it
unlocks; design and read a stress matrix and a reverse stress test; and govern AI-assisted demand
forecasting, contract extraction and counterparty monitoring.

**The master thread.** Kestrel Water SPC continues. Its first operating year, established in
Domain 2 and modelled in Domain 6 (KA 6.2.2), is revenue **USD 12,000,000**, cash operating
costs **4,500,000**, `EBITDA` **7,500,000**, depreciation 2,400,000, interest 2,520,000, cash tax
516,000 and a working-capital absorption of 600,000, giving **`CFADS` 6,384,000** against debt
service of **5,009,635.23** — a `DSCR` of **1.2743** (Domain 10, KA 10.2.1). This domain opens
that revenue line for the first time. The plant's nameplate capacity is **30,000,000 m³ a year**
(about 82,192 m³ a day); expected despatch is **24,000,000 m³**, an 80 % load factor; the
water-purchase agreement pays **USD 0.50 per m³** of expected despatch, and the cost base splits
into **3,600,000 of fixed** cash operating cost and **USD 0.0375 per m³ variable**. Every figure
in the domain is built from those five inputs, and one closed form recurs throughout:

```
CFADS = 0.75 × revenue − 0.03 × volume(m³) − 1,896,000
```

: the `CFADS` bridge of KA 6.2.2 written as a function of the two commercial variables,
absorbing the fixed cost, the 20 % tax rate applied to `EBITDA` less depreciation and interest,
and the working-capital absorption of 5 % of revenue. Substituting revenue 12,000,000 and volume
24,000,000 returns **6,384,000** exactly. It is the domain's arithmetic engine, and every result
below is reproducible from it.

---

## Knowledge Area 7.1 — Contracted and merchant revenue, tariffs and availability payments

*Topics: 7.1.1 the revenue-risk spectrum · 7.1.2 tariff architecture · 7.1.3 availability
payments and the deduction mechanism.*

### 7.1.1 The revenue-risk spectrum

**Definition.** A project's revenue structure sits on a spectrum defined by **who bears the risk
that the asset earns less than forecast**. At the contracted end a creditworthy payer is obliged
to pay a defined amount whether or not it takes the output, provided the asset is available; at
the merchant end the project sells into a market at prices and volumes nobody has promised.
Intermediate forms (take-or-pay, capacity plus energy, minimum revenue guarantees, volume bands)
allocate the risk in specified proportions.

**Why the spectrum, not the mean, governs debt.** Domain 10 sized debt as `CFADS` divided by the
target `DSCR`, times the annuity factor. Both numerator and divisor move with position on this
spectrum, and they move in the same direction, so the effect compounds: merchant revenue lowers the
case a lender will underwrite *and* raises the coverage it requires. That double movement is the
domain's first quantitative result.

**Worked example 7.1.1 — two structures, one expected cash flow, and 10.7 million of debt.**

1. **Setup.** Kestrel's offtaker will accept either of two structures for the same plant.
   **Structure A (availability payment):** a capacity charge of **12,000,000 a year** for making
   30,000,000 m³ of capacity available, subject to deduction for unavailability; the offtaker
   takes all volume risk. **Structure B (volume tariff):** **USD 0.50 per m³** of water actually
   taken, with no minimum. Independent demand study gives three despatch outcomes:
   **28,800,000 m³** with probability 0.25, **24,000,000 m³** with probability 0.50 and
   **19,200,000 m³** with probability 0.25. Cost base and tax as the master thread. Compute
   `CFADS` and `DSCR` in each outcome, the expected values, and the debt each structure supports.
2. **Formula.** `CFADS = 0.75R − 0.03V − 1,896,000`; `DSCR = CFADS ÷ 5,009,635.23`; expected
   value = Σ(probability × outcome); debt capacity = (`CFADS` ÷ target `DSCR`) × `AF(0.06, 12)`,
   with `AF(0.06, 12) = 8.383844` (Domain 3).
3. **Substitution and result.**

   | Structure B outcome | Volume (m³) | Revenue | `EBITDA` | Cash tax | Δ working capital | `CFADS` | `DSCR` |
   |---|---|---|---|---|---|---|---|
   | High (p 0.25) | 28,800,000 | 14,400,000 | 9,720,000 | 960,000 | 720,000 | **8,040,000** | **1.6049** |
   | Base (p 0.50) | 24,000,000 | 12,000,000 | 7,500,000 | 516,000 | 600,000 | **6,384,000** | **1.2743** |
   | Low (p 0.25) | 19,200,000 | 9,600,000 | 5,280,000 | 72,000 | 480,000 | **4,728,000** | **0.9438** |
   | **Expected** | **24,000,000** | **12,000,000** | — | — | — | **6,384,000** | **1.2743** |

   Structure A delivers `CFADS` of **6,384,000** with certainty subject to availability. Debt
   capacity: Structure A at the credit committee's 1.30 × target supports **41,171,123**
   (Domain 10's figure, unchanged). Structure B, underwritten on the low case at the same
   1.30 ×, supports debt service of 3,636,923 and debt of **30,491,396**; underwritten on
   expected `CFADS` at a merchant-appropriate 1.50 × it supports **35,681,640**.
4. **Interpretation.** The expected `CFADS` is **identical to the dollar** (6,384,000 in both
   structures), and so is the expected `DSCR`, at 1.2743, because `DSCR` is linear in `CFADS`
   when debt service is fixed. An adviser who reports expected values has reported that the two
   structures are the same. They differ by **10,679,727 of debt capacity**, **25.94 %** of the
   contracted structure's, and by **11,508,604** against the sponsors' 42,000,000 request. Every
   dollar of that gap is equity, and Domain 10 showed what a mere 828,877 of it did to the
   sponsors' return.

   Three readings deserve to leave this example. **First, the mean is not a coverage ratio.**
   Coverage is tested in a period, and lenders underwrite the period they are prepared to be
   wrong about: commonly a P90 or an explicitly stressed case, never the mean. In the low
   outcome Structure B's `DSCR` is **0.9438**: debt service is not merely uncovered by covenant,
   it is unpaid from operating cash. A structure with a one-in-four chance of failing to pay is
   not a 1.2743 × project with a caveat; it is a different credit. **Second, the case moves as
   well as the ratio**, and a leader who negotiates only the ratio while letting the case be
   chosen unopposed has conceded the larger of the two variables. **Third, the risk has a
   computable price.** Structure A transfers volume risk to the offtaker, who will charge for it
   in tariff, tenor or termination terms. The question is never "which structure is better" but
   "is the offtaker's price for taking volume risk less than 10,679,727 of equity plus the
   return on it?" Asking that question is the professional act.

> **Fig 7.1.1 — Identical expected cash, different bankability.** Bar chart of the volume
> tariff's three `DSCR` outcomes (0.9438, 1.2743, 1.6049) against the availability payment's
> single certain 1.2743, drawn as a dashed horizontal line that is simultaneously the volume
> structure's *expected* `DSCR`; the low-demand bar is crimson because it falls below the 1.00 ×
> line at which scheduled debt service goes unpaid, with the 1.20 × covenant marked. Annotation
> carries the debt each structure supports — 41,171,123 against 30,491,396 — and the 10,679,727
> difference. Source: PCI original. Alt text: three coverage-ratio bars for a volume tariff, one
> of them below the level at which debt service can be paid, compared with a single flat line
> representing an availability payment that delivers the same average coverage with certainty.

### 7.1.2 Tariff architecture

**Definition.** A **tariff** is the formula by which the payer's obligation is computed; its
architecture, not its headline level, determines which party bears which risk. Four building blocks
recur across sectors under different names.

| Block | What it pays for | Risk it leaves with the project |
|---|---|---|
| **Capacity / availability charge** | Making the asset available, whether or not output is taken | Availability, and therefore reliability and maintenance |
| **Energy / variable / usage charge** | Output actually taken, usually at a pass-through or indexed unit rate | Efficiency against a contracted rate; sometimes none |
| **Take-or-pay minimum** | A floor volume, paid whether taken or not | Volume above the floor only |
| **Merchant tail** | Output sold outside the contract | Price and volume, entirely |

The **two-part tariff**, capacity plus variable, is the most common bankable form because it
separates the recovery of fixed costs and debt service (which cannot flex) from the recovery of
variable costs (which can). Its discipline is a matching test: **the capacity charge should
recover fixed cash costs, debt service and the return on capital; the variable charge should
recover variable costs at contracted efficiency, no more.** A tariff that recovers fixed costs
through a volume-linked charge has converted a fixed obligation into a variable receipt, which
is the structural error behind much demand-risk distress. Kestrel's Structure B is that error in
pure form: 3,600,000 of fixed cost and 5,009,635 of debt service, recovered per cubic metre.

**Take-or-pay and its limits.** A floor is worth what the party writing it is good for (Domain
5's creditworthiness condition, with a number attached). Two cautions. Take-or-pay volumes are
frequently **make-up** rights rather than pure obligations: the payer pays for volume not taken
but may take it later at no further charge, which protects cash timing far less than a summary
suggests. And **the floor's tenor must reach the loan's**; an eight-year take-or-pay against a
twelve-year loan is a merchant tail with a delay (KA 7.A.2).

### 7.1.3 Availability payments and the deduction mechanism

**Definition.** An **availability payment** pays for the asset being available to a defined
standard rather than for output, reduced by **deductions** when the standard is not met. The
deduction formula, not the headline payment, is where the exposure lives: it converts an operational
measure into a cash loss, usually at a multiple, and is routinely modelled as though the multiple
were one.

**Worked example 7.1.3 — the four points of availability that breach a covenant.**

1. **Setup.** Kestrel adopts Structure A. The capacity charge is **12,000,000**; guaranteed
   availability is **95 %** of nameplate hours; the deduction is the capacity charge multiplied
   by the availability shortfall in decimal terms and by a **1.5 × liquidated-damage multiplier**.
   Output falls in proportion to the shortfall applied to expected despatch, so variable cost
   falls with it. Compute revenue, `CFADS` and `DSCR` at 93 % and 91 % availability, and find the
   availability at which the 1.20 × covenant, the 1.15 × lock-up and payment itself fail.
2. **Formula.** Let `s` be the shortfall in decimal availability points. Deduction
   `= 12,000,000 × 1.5 × s`; revenue `= 12,000,000 − 18,000,000 s`; volume
   `= 24,000,000 × (1 − s)`. Substituting into the `CFADS` bridge gives the linear form
   `CFADS = 6,384,000 − 12,780,000 s`.
3. **Substitution.** At `s = 0.02`: deduction 360,000; revenue 11,640,000; volume 23,520,000. At
   `s = 0.04`: deduction 720,000; revenue 11,280,000; volume 23,040,000. Thresholds solve
   `6,384,000 − 12,780,000 s = 5,009,635.23 × threshold`.
4. **Result.**

   | Availability | Shortfall `s` | Deduction | Revenue | `CFADS` | `DSCR` |
   |---|---|---|---|---|---|
   | 95.000 % | — | — | 12,000,000 | 6,384,000 | 1.2743 |
   | 93.000 % | 2.000 pts | 360,000 | 11,640,000 | 6,128,400 | 1.2233 |
   | **92.086 %** | 2.914 pts | 524,560 | 11,475,440 | **6,011,562** | **1.2000** ← covenant |
   | 91.000 % | 4.000 pts | 720,000 | 11,280,000 | 5,872,800 | 1.1723 |
   | **90.126 %** | 4.874 pts | 877,351 | 11,122,649 | **5,761,081** | **1.1500** ← lock-up |
   | **84.246 %** | 10.754 pts | 1,935,725 | 10,064,275 | **5,009,635** | **1.0000** ← payment fails |

5. **Interpretation.** The operational number a plant manager recognises is availability, and the
   sentence this table produces belongs on an operations dashboard: **Kestrel breaches its covenant
   at 92.09 % availability — 2.91 points of headroom against a 95 % guarantee.** That is not
   comfortable. It is the same headroom Domain 10 reported as 372,438 of annual cash and Domain 6
   as a 4.14 % revenue fall, expressed in the unit the people who control it actually manage.

   The mechanism is where the arithmetic surprises people. A single point of availability costs
   **127,800 of `CFADS`**, the slope of the linear form, of which 180,000 is lost revenue and
   52,200 is recovered through lower variable cost, tax and working-capital absorption. **The
   1.5 × multiplier is doing most of the damage.** The general slope is `9,000,000 × multiplier
   − 720,000`, so at a multiplier of 1.0 it falls to 8,280,000 and the covenant does not break
   until **90.502 %** — the negotiated multiplier costs **1.584 percentage points** of operating
   tolerance. (At a multiplier of 1.0 that slope is exactly the volume term of KA 7.3.2, because
   a pro-rata deduction is economically identical to losing the output.) Multipliers, caps on
   cumulative deductions, cure periods, planned-outage allowances and the treatment of force
   majeure and offtaker-caused unavailability are therefore first-order commercial terms, not
   schedules for the technical adviser. The caution is blunt: **an availability structure is not
   a risk-free structure, it is one whose risk has been converted from demand into operations.**
   Kestrel's revenue is now a function of its own reliability — a risk it controls but must then
   actually control, with Domain 10's maintenance reserve funded and the outage plan built
   around the deduction formula rather than engineering convenience.

**Who measures, who certifies, and what happens while it is disputed.** The arithmetic above
assumes a number, availability, arrives each month and is agreed. It does not arrive; it is
*produced*, by somebody, from something, and it can be contested. The governance layer around
the formula decides whether a deduction is ever actually applied, and it is a bankability
question of the same order as the multiplier.

*The measurement source.* A deduction formula is only as good as the metering behind it. What
physically measures availability (the plant control system, a revenue meter, an offtaker's own
instrumentation, a manual log), and how is it calibrated, by whom, on what cycle, and to what
tolerance? Who may recalibrate, and does a recalibration reopen prior periods? What happens
during a metering outage: is availability deemed, estimated from a preceding period, or treated
as nil? A structure in which the offtaker owns the meters and the deemed-availability rule on
meter failure is "nil" has transferred more risk than the multiplier suggests. **Metering,
calibration and the failure rule are bankability conditions** and belong in the condition
register of Domain 5, not in a technical annexe.

*The certification chain.* Somebody produces the primary availability record and somebody certifies
it. The three common architectures — SPV self-reports and the offtaker may challenge; the offtaker
measures and the SPV may challenge; an independent engineer certifies and both may challenge — put
the burden of proof in three different places, and the burden of proof is worth more than a
percentage point of multiplier. Establish which one the contract creates, what the challenge window
is, and **what happens to a period nobody challenges in time**, because an unchallenged month is
usually final.

*The evidence, and how long it must survive.* Whoever bears the burden must be able to prove the
month years later. The underlying availability data (meter records, control-system logs, outage
records with their causes and permits, maintenance records, correspondence notifying outages) is
retained through the contractual challenge window **and** the period in which a dispute about it
can still be brought, which is longer and is jurisdiction-specific. The standing retention rule
in the toolkit preamble applies: period, form, named custodian, in a form that opens without the
system that created it, since control-system data is exactly the kind that becomes unreadable
when the plant's software is upgraded.

*Escalation, and who may settle.* The escalation route is typically a stated sequence
(operational meeting, senior representatives, then expert determination or arbitration), and its
cost allocation matters, because a mechanism in which each party bears its own costs is one the
larger party can afford to use for its own sake. Two internal locks belong on the SPV side.
First, **name who may settle a deduction claim on the SPV's behalf**, with a threshold above
which it goes to the board. Second, and more easily missed: **a settlement that changes a
covenant ratio is not an operational decision.** Agreeing a disputed deduction that moves the
coverage ratio, or conceding a run of months to close a commercial argument, reaches into the
finance documents; whether it needs anything of the lenders is a document question, and the
answer is established before the concession is offered, not after.

*The cash-timing consequence, which is the one with a number attached.* Ask whether deductions
are **applied pending resolution**. If they are, a pay-then-argue mechanism, a disputed
operational judgment becomes an immediate cash reduction and therefore an immediate coverage
event, **before anyone has ruled on it**. On the table above, a contested run of deductions
equivalent to three points of availability moves reported coverage by roughly the slope already
computed, and it does so in the quarter the dispute starts rather than the quarter it ends. If
instead the disputed amount is withheld or escrowed pending determination, the exposure is to
the counterparty's credit and to the delay, which is a different risk and a smaller one. This
single question, pay first or argue first, changes the reserve the structure needs, and it is
the question most often left to the operations team to discover.

*The standing caution.* Whether deductions may be applied pending resolution, what remedy attaches
to a deduction later found to be wrongly applied, whether an unchallenged period is conclusive, and
how long a dispute about a past month may be brought are **drafting and jurisdiction questions for
qualified counsel on the governing law of the contract**. Nothing here states the position in any
jurisdiction, and nothing here characterises any deduction as rightful or wrongful. What the
professional owes is to establish each of them from the executed documents, record them in
Toolkit 7.T.1, and make sure the operations team that will live with them has read the answer.

### AI in this KA

Machine assistance earns its place in three tasks. **Extracting the payment mechanism** (charge
formulae, deduction multipliers, caps, outage allowances, indexation clauses and their
cross-references) is high-volume reading that models do well and humans do inconsistently, and
the output is a specification a modeller can implement (Toolkit 7.T.1). **Reconciling the
modelled tariff to the contractual tariff** by recomputing a historical invoice from the
extracted formula is a genuine test with a right answer. And **monitoring deductions** against
availability data as it accrues turns a quarterly surprise into a weekly signal.

Before any of it, the data question, which comes first: an offtake or concession contract, and
above all an **unsigned draft** of one, is the highest-classification material in the
transaction, because it carries the counterparty's position as well as the project's. It is
processed **only in an environment approved for that data classification and permitted by the
confidentiality undertakings that cover it**, and establishing that permission is a precondition
of the extraction rather than a review of it. It is usually a permitted-recipient question
rather than a tool-quality one: an approved tool used outside the disclosure the undertaking
permits is still a disclosure, and a grantor's tender rules frequently restrict where bid
material may be processed at all. Domain 1, KA 1.3.4 states the rule; Domain 16 builds the
machinery.

Where it must not go: **the deduction formula must never be paraphrased into the model.** A summary
rendering "1.5 times the availability shortfall applied to the capacity charge" as a pro-rata
reduction understates Kestrel's exposure by a third and moves the covenant breakpoint by 1.584
availability points — an error no downstream check catches, because the model is internally
consistent and wrong. And a model must not be asked to *choose* the structure: Worked example
7.1.1 is arithmetic, but judging what the offtaker's price for volume risk is worth is a commercial
decision with an accountable owner. Verification is concrete: recompute one period's revenue from
the clause text by hand, at full and at reduced availability, and confirm both against the model
before quoting any coverage ratio derived from it. **AI proposes; the professional verifies,
decides and remains accountable.**

### Key terms — KA 7.1

| Term | Meaning |
|---|---|
| **Contracted / merchant revenue** | Revenue under an obligation to pay / revenue sold into a market with no obligation. |
| **Capacity (availability) charge** | Payment for making the asset available, independent of output taken. |
| **Two-part tariff** | Capacity charge plus variable charge; separates fixed-cost recovery from output. |
| **Take-or-pay** | Contractual floor volume paid whether taken or not; often subject to make-up rights. |
| **Availability deduction** | Reduction in the capacity charge for failing the availability standard, commonly at a multiplier. |
| **Deduction multiplier** | The factor applied to the availability shortfall; converts one operational point into more than one point of cash. |
| **Deemed availability** | The value availability takes when it cannot be measured — deemed, estimated from a prior period, or nil; a term with the same cash consequence as the multiplier. |
| **Certification chain** | Who produces the primary availability record, who certifies it, who may challenge, and within what window; it decides where the burden of proof sits. |
| **Pay-then-argue mechanism** | A deduction regime under which amounts are applied pending resolution, so a contested operational judgment becomes an immediate coverage event before anyone has ruled on it. |
| **Settlement lock** | The rule that settling a deduction claim which moves a covenant ratio is not a purely operational decision; it is checked against the finance documents before it is offered. |

### Sample MCQs — KA 7.1

**MCQ 7.1-A `[7.1.1 · Analysis]`** Two structures for the same plant have identical expected
`CFADS` of 6,384,000 and identical expected `DSCR` of 1.2743. Structure A pays that amount with
certainty; Structure B's outcomes are 4,728,000 / 6,384,000 / 8,040,000 with probabilities
0.25 / 0.50 / 0.25. The soundest conclusion is:
- A. the structures are equivalent, since expected coverage is the same
- B. Structure B supports materially less debt, because a lender underwrites a stressed or low case and requires higher coverage for dispersion ✅
- C. Structure B supports more debt, because its upside outcome is higher
- D. the difference can be resolved by raising the target `DSCR` alone

*Rationale:* Sizing on the low case at 1.30 × gives 30,491,396 against 41,171,123 — a
10,679,727 gap (7.1.1). A treats a mean as a coverage ratio, which coverage tests never are; C
mistakes an upside the lender has no claim on for capacity; D captures only part of the effect,
since the case moves as well as the ratio.

**MCQ 7.1-B `[7.1.3 · Application]`** A capacity charge of 12,000,000 carries a 95 %
availability guarantee and a deduction equal to the charge multiplied by the shortfall and by
1.5. At 91 % availability the deduction is:
- A. USD 480,000
- B. USD 720,000 ✅
- C. USD 1,620,000
- D. USD 600,000

*Rationale:* `12,000,000 × 1.5 × 0.04 = 720,000`. A omits the 1.5 × multiplier; C applies the
multiplier to total unavailability (9 points, `1 − 0.91`) instead of the shortfall against the
95 % guarantee; D applies a 5 % reduction, reading the guarantee as the deduction base.

**MCQ 7.1-C `[7.1.2 · Analysis]`** A project recovers all of its fixed cash costs and its entire
debt service through a per-unit volume charge with no floor. The structural defect is:
- A. the unit rate will be too low to be commercial
- B. fixed obligations are funded by a variable receipt, so any volume shortfall falls entirely on coverage ✅
- C. the tariff cannot be indexed
- D. it prevents the use of an availability standard

*Rationale:* Matching discipline requires fixed-cost and debt-service recovery in a charge that
does not flex with volume (7.1.2). A is a pricing observation, not a structural one; C and D are
simply untrue of volume tariffs.

**MCQ 7.1-D `[7.1.3 · Analysis]`** Kestrel breaches its 1.20 × covenant at 92.086 %
availability. Reducing the negotiated deduction multiplier from 1.5 to 1.0 would move that
breakpoint to 90.502 %. The correct reading is:
- A. the multiplier is a technical schedule with no financial consequence
- B. the multiplier costs 1.584 percentage points of operating tolerance and is therefore a first-order commercial term ✅
- C. the multiplier only matters if availability falls below 90 %
- D. the breakpoint depends on the tariff level, not the multiplier

*Rationale:* The multiplier scales the slope of the `CFADS` line against availability, so it
scales headroom directly (7.1.3). C inverts the logic; the multiplier is what brings the
breakpoint *closer*; D ignores that the deduction is computed on the charge and the multiplier
together.

**MCQ 7.1-E `[7.1.1 · Evaluation]`** Kestrel's offtaker will take volume risk under Structure A,
but only for a capacity charge of **11,600,000** instead of 12,000,000. `CFADS` becomes
6,084,000, `DSCR` 1.2145, and debt capacity at 1.30 × becomes **39,236,390**, against 41,171,123
at the full charge and 30,491,396 under the volume tariff sized on its low case. The 400,000 a
year has a present value of 4,269,910 over the 25-year concession at 8 %. The soundest
recommendation is:
- A. reject the reduction: 4,269,910 of present value is surrendered permanently for a one-off financing benefit
- B. accept it: the offtaker's price for taking volume risk is 4,269,910 of concession-life value against 8,744,994 of debt capacity released, so it is well below what the risk transfer is worth ✅
- C. accept it, because an availability structure is always more bankable than a volume tariff
- D. reject it and negotiate the 1.30 × target down instead, since the ratio is the variable that matters

*Rationale:* this is the question KA 7.1.1 says is the professional act (is the offtaker's price
for volume risk less than the equity the transfer releases?), and here it is, by a factor of
more than two, with 1.2145 still clear of the covenant. A performs half the calculation: the
capacity it buys is not a one-off benefit but equity permanently not subscribed. C is the
unsupported generality, and 7.1.3 shows what the structure actually does (converts demand risk
into operational risk, with the covenant breaking at 92.09 % availability). D inverts the
domain's central finding: the case moves as well as the ratio, and the case is the larger of the
two variables.

**MCQ 7.1-F `[7.1.3 · Evaluation]`** A contract summary renders the deduction clause (the
capacity charge multiplied by the availability shortfall and by a 1.5 × liquidated-damage
multiplier) as a pro-rata reduction of the charge, and the modeller builds it that way. The
model balances and every check in it passes. The soundest professional position is that:
- A. the difference is immaterial while the model is internally consistent and its checks pass
- B. one period's revenue must be recomputed from the clause text by hand, at full and at
  reduced availability, before any coverage ratio derived from it is quoted: the paraphrase cuts
  the slope of `CFADS` against availability from 12,780,000 to 8,280,000 (35.21 %), and moves
  the covenant breakpoint from 92.086 % to 90.502 % ✅
- C. the deduction schedule belongs to the technical adviser, so the commercial team should work from
  the summary it was given
- D. the target `DSCR` should be raised to restore the coverage the paraphrase removed

*Rationale:* the slope is `9,000,000 × multiplier − 720,000`, so a multiplier read as 1.0 turns
127,800 of `CFADS` a point into 82,800, and **no downstream check catches it** because the model is
internally consistent and wrong (7.1.3). A is precisely the reasoning that lets the error survive
review. C misallocates a first-order commercial term to a technical annexe. D prices a definitional
error as though it were a risk, which buries it inside a ratio instead of correcting it.

**MCQ 7.1-G `[7.1.2 · Comprehension]`** A take-or-pay minimum and an availability payment differ in
what each leaves with the project:
- A. they are equivalent, both being contracted revenue from a creditworthy payer
- B. a take-or-pay floor covers volume below a contracted minimum (and is frequently a make-up
  right, so the payer may take the volume later at no further charge), while an availability
  payment removes volume risk altogether and substitutes an obligation to be available ✅
- C. take-or-pay transfers operating risk to the payer, while an availability payment retains it
- D. take-or-pay protects against price and an availability payment against cost

*Rationale:* a floor is worth what the party writing it is good for and often protects cash timing far
less than a summary suggests, while an availability structure converts demand risk into operational
risk the project must then genuinely manage (7.1.2, 7.1.3). A ignores that the two leave different
risks behind. C reverses the availability mechanism, whose whole point is that the project answers for
availability. D names risks neither instrument addresses.

**MCQ 7.1-H `[7.1.3 · Evaluation]`** Two bidders' term sheets carry the identical deduction formula
and the identical 1.5× multiplier. In sheet A the offtaker owns and maintains the availability
meters, availability is deemed nil during any metering failure, and deductions are applied pending
resolution of a dispute. In sheet B the meters are jointly witnessed and independently calibrated,
metering failure is deemed at the preceding period's availability, and disputed deductions are
escrowed until determination. The correct assessment is:
- A. the two are the same exposure, since the multiplier and the formula are identical and the multiplier is what does the damage
- B. sheet A carries materially more exposure: the measurement source, the deemed-availability rule and the pay-then-argue timing each move cash independently of the formula, and the last of them converts a contested operational judgment into an immediate coverage event before anyone has ruled ✅
- C. sheet B is worse, because escrow exposes the project to the offtaker's credit for the escrowed amounts
- D. the difference is a technical-adviser matter and does not belong in a commercial comparison

*Rationale:* the formula is only the last step; who measures, what is deemed when measurement
fails, and whether cash moves before determination decide whether a deduction is ever actually
applied and when (7.1.3). A stops at the arithmetic the domain has already priced. C identifies
a real but smaller exposure (credit and delay on the escrowed sum), and weighs it against an
immediate coverage event. D is the misclassification the section exists to correct: these are
first-order commercial terms.

### Self-check — KA 7.1

1. *Why does an expected `DSCR` convey nothing about bankability?* Coverage is tested in a
   period; lenders underwrite a low or stressed case, and dispersion also raises the required
   ratio, so the mean moves neither variable.
2. *State Kestrel's covenant breakpoint in availability terms and in cash terms.* 92.086 %
   availability; `CFADS` of 6,011,562, which is 372,438 of annual cash below base (Domain 10).
3. *What has an availability structure actually done to a project's risk?* Converted demand risk
   into operational risk, which the project controls but must then genuinely manage.
4. *Who produces the availability number the deduction formula operates on?* (Somebody, from
   something: the certification chain) primary record, certifier, challenge window, and what
   happens to a month nobody challenges — decides where the burden of proof sits, and that is
   worth more than a percentage point of multiplier.
5. *What is the deemed-availability rule, and why does it matter?* What availability is taken to
   be when it cannot be measured. A structure in which the offtaker owns the meters and failure
   is deemed nil has transferred more risk than the multiplier suggests.
6. *Why is "are deductions applied pending resolution?" the question with a number attached?*
   Because a pay-then-argue mechanism turns a contested operational judgment into an immediate
   cash reduction and therefore an immediate coverage event, in the quarter the dispute starts
   rather than the quarter it ends. Withholding or escrow converts the same exposure into
   counterparty credit and delay, which is smaller.
7. *Who may settle a disputed deduction?* (A named person, below a stated threshold), and never
   where the settlement moves a covenant ratio, which reaches into the finance documents and is
   checked against them before anything is offered.

---

## Knowledge Area 7.2 — Demand models, concessions and service revenue

*Topics: 7.2.1 demand forecasting and the ramp · 7.2.2 concession payment mechanisms · 7.2.3
subscription and service revenue.*

### 7.2.1 Demand forecasting and the ramp

**Definitions.** A **demand model** forecasts the quantity of output the market will take at a
given price over the asset's life. The **ramp** is the period after commercial operations during
which realised demand approaches its mature level as users discover and reorganise around the
asset. **Optimism bias** is the tendency of forecasts prepared by parties interested in the
project proceeding to exceed outturn: a documented pattern in demand-risk infrastructure, and
the reason lenders commission their own market adviser rather than reviewing the sponsors'.

The professional content here is not forecasting technique, which belongs to market specialists,
but the **discipline of using a forecast in a financing**. Three practices carry almost all the
value.

**Separate the cases and name their owners.** The **sponsor case** is the sponsors' central
expectation; the **bank case** is what lenders will lend against, typically the sponsor case
with specified haircuts, a slower ramp, no unindexed growth and a conservative price deck; the
**downside case** is the stress the structure must survive. Domain 6 (KA 6.1.3) made this a
modelling rule; here it is a commercial one, because the gap between the cases is the gap
between the sponsors' return and the lenders' credit (Domain 6 priced it on Kestrel at
**17,107,567 of `NPV`** from the escalation assumption alone).

**Model the ramp explicitly and let the debt schedule respond.** Level debt service against a
rising cash profile wastes coverage late and breaches early, which is the case for sculpting
(Domain 10, KA 10.1.3). The recurring error is to model the ramp in the revenue line and then size
level debt on the mature year: the ratio computed on year five is met, and the covenant tested in
year two is not.

**State the forecast's elasticity, not only its level.** A demand forecast at a given tariff is
an implicit price-elasticity assumption. Where the project sets price (a toll road, a car park,
a data-centre re-let) a shortfall can be met by raising price only if elasticity permits; where
price is contractual it cannot be met at all. A demand model presented without its elasticity
has withheld the one property that determines whether management has a lever.

### 7.2.2 Concession payment mechanisms

**Definition.** A **concession** grants a private party the right and obligation to build, finance
and operate a public asset for a defined term, with revenue arriving through a payment mechanism the
grant defines and the asset handed back at the end. The mechanism *is* the risk allocation, and the
standard family is worth naming because the names travel badly between jurisdictions and sectors.

| Mechanism | Who pays | Volume risk sits with | Typical use |
|---|---|---|---|
| **Availability payment** | The grantor, from budget | The grantor | Social infrastructure, non-tolled roads, some water |
| **Real toll / user charge** | Users, directly | The project | Toll roads, ports, car parks |
| **Shadow toll** | The grantor, per unit of measured usage | Shared: the project bears volume, the grantor bears price | Roads where user charging is politically unavailable |
| **Volume band** | Either, per unit within defined bands at declining rates | Shared by construction | Water, waste, transport with uncertain demand |
| **Minimum revenue guarantee with revenue sharing** | Grantor tops up below a floor; shares above a ceiling | Shared symmetrically | Long concessions with contested forecasts |

**Worked example 7.2.2 — what a volume band is worth.**

1. **Setup.** Kestrel's offtaker will not accept Structure A's full volume risk but offers a
   **banded tariff**: **USD 0.55 per m³** on the first 18,000,000 m³, **USD 0.35** on the next
   6,000,000 m³, and **USD 0.10** on anything above 24,000,000 m³. At expected despatch of
   24,000,000 m³ the band structure pays exactly 12,000,000, so the headline tariff is unchanged.
   Using the same three demand outcomes as Worked example 7.1.1, compute revenue, `CFADS` and
   `DSCR`, the expected values, and the debt capacity on the low case at 1.30 ×.
2. **Formula.** Banded revenue = Σ(volume in band × band rate); `CFADS` and `DSCR` as before.
3. **Result.**

   | Outcome | Volume (m³) | Banded revenue | Flat-tariff revenue | `CFADS` banded | `DSCR` banded | `DSCR` flat |
   |---|---|---|---|---|---|---|
   | High (p 0.25) | 28,800,000 | 12,480,000 | 14,400,000 | 6,600,000 | **1.3175** | 1.6049 |
   | Base (p 0.50) | 24,000,000 | 12,000,000 | 12,000,000 | 6,384,000 | **1.2743** | 1.2743 |
   | Low (p 0.25) | 19,200,000 | 10,320,000 | 9,600,000 | 5,268,000 | **1.0516** | 0.9438 |
   | **Expected** | 24,000,000 | **11,700,000** | 12,000,000 | **6,159,000** | 1.2294 | 1.2743 |

   Debt capacity on the low case at 1.30 ×: **33,973,915** banded against 30,491,396 flat (a
   gain of **3,482,520**). The expected revenue given up is **300,000 a year**, 2.50 % of base
   revenue, whose present value over the twelve loan years at 6 % is **2,515,153**.
4. **Interpretation.** The band is a risk-transfer instrument dressed as a price list, and its
   economics are explicit once computed: the sponsors surrender 2,515,153 of present value and
   receive 3,482,520 of debt capacity, a **net gain of 967,367** before the return on the equity
   released. Whether that is a good trade depends on the cost of equity against the cost of debt
   (Domain 9), but the method matters more than the answer: a negotiating team that has not computed
   it is arguing about band rates without knowing which way it wants them to move.

   Two further readings matter more than the headline. **The band compresses the distribution
   from both ends, and the lender pays for only one end.** `DSCR` dispersion narrows from
   0.9438–1.6049 to 1.0516–1.3175: the low tail improves by 0.1078 of coverage and the high tail
   is surrendered entirely. Since lenders price the low tail and equity owns the high one, a
   band transfers value **from equity's upside to debt capacity**, which is why sponsors
   confident in demand resist bands and sponsors who need leverage accept them, and why the same
   band is a good deal for one shareholder and a poor one for another in the same consortium.
   **And the low case still breaches.** At 1.0516 the banded structure pays its debt service but
   fails both the 1.20 × covenant and the 1.15 × lock-up, so the band has bought bankability,
   not compliance. A band should be sized against the lock-up, not against zero.

### 7.2.3 Subscription and service revenue

**Definition.** In **subscription and service models** revenue arrives as recurring payments from a
population of customers under contracts materially shorter than the financing, so the revenue risk
is **re-contracting risk** rather than volume or price risk. Three measures govern: **annual
recurring revenue** (`ARR`, the annualised value of contracts in force), **net revenue retention**
(next year's revenue from this year's customers after churn and expansion, as a percentage), and the
**weighted-average remaining contract term** (`WARCT`), the contract-weighted mean of remaining
tenors. Digital infrastructure, managed services and waste all finance on this shape.

**Worked example 7.2.3 — the run-off case, and why a 7-year loan was refused.**

1. **Setup.** Halyard Connect Networks (a fictitious SPV) operates a metropolitan fibre network.
   At financial close, `ARR` is **24,000,000**; cash operating costs are wholly fixed at
   **13,200,000**, so `EBITDA` is 10,800,000; maintenance capex is **1,800,000** and cash tax is
   nil in the period by reason of capital allowances (stated as an assumption of the illustration,
   not a general treatment). Net revenue retention is **93 %**. The contract book is 40 % of
   `ARR` with five years remaining, 35 % with three years and 25 % with one year. The sponsors
   seek **40,000,000** of senior debt at **7 % over seven years**. Compute the lenders' run-off
   case, the coverage it delivers against level debt service, the `WARCT`, and the debt the
   run-off case actually supports at a 1.30 × target.
2. **Formula.** Run-off revenue in year `t` = `ARR × 0.93^(t−1)` (contracted revenue only, with
   no credit for unsigned new sales). `CFADS` = revenue − 13,200,000 − 1,800,000. Level
   instalment = 40,000,000 ÷ `AF(0.07, 7)`, with `AF(0.07, 7) = 5.389289`. Sculpted capacity = Σ
   (`CFADS(t)` ÷ 1.30) × `DF(0.07, t)` (Domain 10, KA 10.1.3). `WARCT` = Σ(share × remaining
   term).
3. **Result.**

   | Year | Run-off revenue | `EBITDA` | `CFADS` | `DSCR` on level service |
   |---|---|---|---|---|
   | 1 | 24,000,000 | 10,800,000 | 9,000,000 | **1.2126** |
   | 2 | 22,320,000 | 9,120,000 | 7,320,000 | **0.9862** |
   | 3 | 20,757,600 | 7,557,600 | 5,757,600 | 0.7757 |
   | 4 | 19,304,568 | 6,104,568 | 4,304,568 | 0.5800 |
   | 5 | 17,953,248 | 4,753,248 | 2,953,248 | 0.3979 |
   | 6 | 16,696,521 | 3,496,521 | 1,696,521 | 0.2286 |
   | 7 | 15,527,764 | 2,327,764 | 527,764 | 0.0711 |

   Level instalment **7,422,128.79**. `WARCT` **3.30 years** (**47.14 %** of the seven-year
   tenor). Sculpted debt capacity on the run-off case at 1.30 × is **20,271,839**, **50.68 %**
   of the 40,000,000 sought.
4. **Interpretation.** The structure fails in **year two**, and the mechanism is worth stating
   precisely because it is the characteristic failure of service-revenue financings. A 7 %
   annual revenue decline against a wholly fixed cost base takes `EBITDA` down **15.56 %** in
   the first year of run-off and **17.13 %** in the second, and `CFADS` down **18.67 %** and
   then **21.34 %**: the operating-leverage amplification of KA 7.3.2 applied to attrition
   rather than to demand, and accelerating, because the fixed cost base is a growing share of a
   shrinking revenue. Net revenue retention of 93 % is in many service markets a perfectly
   respectable number; against a wholly fixed cost base and a seven-year loan it is a sixth of
   `EBITDA` in the first year and more thereafter.

   The professional consequences are three. **The lender's case is the run-off case, and arguing
   otherwise is arguing to be lent against unsigned revenue.** Sponsors routinely present a
   forecast including new sales; the honest response is to present both, with debt sized on the
   contracted book and the new-sales case shown as the equity story. **The binding metric is
   `WARCT` against tenor, not `ARR`.** At 3.30 years against a seven-year loan, less than half
   the loan's life is contracted, and the disciplined responses are a shorter tenor, a
   re-contracting reserve, a cash sweep that retires debt while the contracted book still
   exists, or anchor extensions as a condition precedent. **And 20,271,839 is the honest answer,
   so the negotiation is about the other 19.7 million**: a shorter tenor, a lower amount,
   extensions before close, or more equity. The sponsors are better served by a leader who
   computes 50.68 % early than by one who discovers it in credit committee.

### AI in this KA

Demand forecasting is the most attractive and most dangerous machine application in this domain.
It earns its place in **pattern extraction from usage data** (seasonality, elasticity estimation
from observed price and volume, cohort-level churn and expansion behaviour no spreadsheet
summary reveals) in **churn and re-contracting prediction** for service models, and in
**scenario generation**, producing the coherent bundles a stress matrix needs faster and more
consistently than a workshop.

Where it must not go. **A model trained on comparable assets' ramps inherits their optimism
bias**, because the training data is forecasts and outturns from projects that were *financed*
(a selected population), and the assistant will not say so. The output must be tested against
the base rate that matters: outturn against forecast for comparable assets including those that
underperformed, which is precisely the data least likely to be available. **A forecast must
never be presented without its case label and its owner**, and a machine-generated forecast has
no owner until a named professional adopts it. **And no assistant should select the bank case**,
which is a negotiating position rather than an estimate. Verification: reconstruct the
forecast's implied elasticity and load factor by hand, check them against the asset's physical
and contractual capacity, and require that any forecast used in a financing carry the market
adviser's name.

### Key terms — KA 7.2

| Term | Meaning |
|---|---|
| **Ramp** | The period during which realised demand approaches its mature level after commercial operations. |
| **Sponsor / bank / downside case** | Central expectation · the case lenders will lend against · the stress the structure must survive. |
| **Shadow toll** | Grantor pays the project per unit of measured usage; volume risk stays with the project, price risk with the grantor. |
| **Volume band** | Declining unit rates across volume tranches; compresses revenue dispersion from both ends. |
| **`ARR`** | Annual recurring revenue: the annualised value of contracts in force. |
| **Net revenue retention** | Next year's revenue from this year's customers, after churn and expansion, as a percentage. |
| **`WARCT`** | Weighted-average remaining contract term; the contracted horizon to be compared with loan tenor. |
| **Run-off case** | Contracted revenue only, with no credit for unsigned new business. |

### Sample MCQs — KA 7.2

**MCQ 7.2-A `[7.2.2 · Application]`** A banded tariff pays 0.55 per m³ on the first 18,000,000 m³,
0.35 on the next 6,000,000 and 0.10 above 24,000,000. At despatch of 19,200,000 m³ revenue is:
- A. USD 9,600,000
- B. USD 10,320,000 ✅
- C. USD 10,560,000
- D. USD 12,000,000

*Rationale:* `18,000,000 × 0.55 + 1,200,000 × 0.35 = 9,900,000 + 420,000 = 10,320,000`. A
applies the flat 0.50 tariff; C applies 0.55 to the whole volume; D is the base-case revenue,
which the band does not guarantee.

**MCQ 7.2-B `[7.2.3 · Analysis]`** An SPV has `ARR` of 24,000,000, net revenue retention of 93 %
and wholly fixed cash costs of 13,200,000. In the first year of run-off, `EBITDA` falls by
approximately:
- A. 7 %
- B. 15.6 % ✅
- C. 18.7 %
- D. it does not fall, because retention is above 90 %

*Rationale:* `EBITDA` falls from 10,800,000 to 9,120,000, a fall of **15.56 %** — the 7 %
revenue decline amplified by the fixed cost base. A mistakes the revenue decline for the earnings
decline; C is the **`CFADS`** decline (9,000,000 to 7,320,000, −18.67 %), the right amplification
applied to the wrong measure; D misreads a retention rate as a growth rate.

**MCQ 7.2-C `[7.2.3 · Analysis]`** A service-revenue SPV has a `WARCT` of 3.30 years and seeks a
seven-year loan. The lender's most likely structural response is:
- A. to accept the tenor, since `ARR` covers debt service in year one
- B. to size on the contracted run-off case and require a shorter tenor, a re-contracting reserve, a cash sweep or contract extensions before close ✅
- C. to size on the sponsor case including new sales, discounted by 10 %
- D. to require an availability guarantee

*Rationale:* Less than half the loan's life is contracted, so the loan is being asked to bridge
uncontracted years (7.2.3). A relies on year one alone; C lends against unsigned revenue; D
applies a mechanism from a different revenue architecture.

**MCQ 7.2-D `[7.2.2 · Analysis]`** A volume band raises the low-case `DSCR` from 0.9438 to
1.0516 and lowers the high-case `DSCR` from 1.6049 to 1.3175, reducing expected revenue by
300,000 a year. The correct characterisation is:
- A. the band destroys value, since expected revenue falls
- B. the band transfers value from equity's upside to debt capacity, and the trade is computable: 2,515,153 of present value surrendered for 3,482,520 of capacity ✅
- C. the band has no effect on debt capacity, since expected `CFADS` still exceeds debt service
- D. the band removes the covenant risk

*Rationale:* Lenders price the low tail and equity owns the high one, so compressing both is a
transfer, not a loss (7.2.2). A ignores the capacity gain; C treats expected cash as the sizing
basis; D is false (the low case at 1.0516 still breaches the 1.20 × covenant).

**MCQ 7.2-E `[7.2.3 · Comprehension]`** A sponsor complains that its lenders "refuse to count our
sales pipeline". The statement that best conveys what a run-off case is:
- A. lenders assume the business stops selling, which is conservatism for its own sake
- B. it values only revenue already under contract, declining at the retention rate, so debt is sized on promises that exist rather than on sales not yet made ✅
- C. it is the sponsor case with a standard haircut applied to forecast new sales
- D. it assumes every customer leaves at the end of its current term

*Rationale:* at 93 % net revenue retention the run-off case takes 24,000,000 of `ARR` to 22,320,000
in the following year — a statement about what is signed, not a prediction that selling stops
(7.2.3). A reads a sizing basis as a forecast; C describes a haircut to a sponsor case, which is a
different and weaker discipline because it still lends against unsigned revenue; D describes nil
retention rather than 93 %.

**MCQ 7.2-F `[7.2.1 · Evaluation]`** A sponsor team proposes to present its central forecast as the
base case and the same forecast with a flat 10 % haircut as "the bank case". On Kestrel the escalation
assumption alone separates the sponsor and bank cases by 17,107,567 of `NPV`. The soundest position is
that:
- A. the approach is acceptable, since a 10 % haircut is a conservative adjustment in the sponsors'
  own favour to concede
- B. the bank case is a negotiating position built from specified haircuts, a slower ramp, no
  unindexed growth and a conservative price deck, and a haircut with no stated composition
  cannot be defended line by line: conceding the case unopposed concedes more than the coverage
  ratio does ✅
- C. the work is unnecessary, because the lenders' market adviser will produce the bank case
- D. only the sponsor case should be presented, leaving the lenders to make their own adjustments

*Rationale:* the case moves as well as the ratio, and it is the larger of the two sizing variables
(7.1.1, 7.2.1); a bank case is also a different object from a downside case, with a different purpose
and owner. A dresses an arbitrary percentage as conservatism and cannot survive a question about which
line it applies to. C and D hand the case to the counterparty that gains from choosing it, which is
how sponsors arrive in credit committee with no position of their own.

### Self-check — KA 7.2

1. *What distinguishes a bank case from a downside case?* The bank case is what lenders will
   lend against; the downside case is the stress the structure must survive. They have different
   purposes and different owners, and conflating them hides both.
2. *Why is `WARCT` more informative than `ARR` for a service-revenue financing?* `ARR` measures
   the size of the contracted book; `WARCT` measures how much of the loan's life it covers,
   which is the risk being financed.
3. *Which tail does a volume band buy, and who pays for it?* It buys the low tail, which lenders
   price; equity pays for it by surrendering the high tail.

---

## Knowledge Area 7.3 — Price escalation and volume risk

*Topics: 7.3.1 indexation architecture and margin drift · 7.3.2 volume risk and operating
leverage · 7.3.3 turning revenue risk into a tolerance.*

### 7.3.1 Indexation architecture and margin drift

**Definition.** **Indexation** is the contractual escalation of a price by reference to a
published index. Domain 3 (KA 3.3.2) established the compounding arithmetic and the register of
index mechanics (publisher, series, definition, lag, cap, floor, compounding basis). This topic
addresses the commercial structure those mechanics sit inside: the **indexation architecture**,
meaning which *proportion* of each revenue and cost line escalates, on which index, and what the
mismatch is worth.

Each line has three parameters and only the first two are usually negotiated with care: the
index, the rate it is expected to run at, and the **indexed share**. A tariff described as
"CPI-indexed" may escalate 60 %, 80 % or 100 % of its value; the unindexed remainder is a fixed
nominal amount eroding in real terms for the whole concession. Because revenue-side and
cost-side shares are set in different negotiations, by different people, at different times,
they are almost never matched, and the mismatch compounds.

**Worked example 7.3.1 — the 36 basis points that cost 3.27 margin points.**

1. **Setup.** Kestrel's water-purchase agreement indexes **80 %** of the tariff to a consumer
   price index assumed to run at **2.5 %** a year; the remaining 20 % is fixed for the 25-year
   concession. On the cost side, **70 %** of the 4,500,000 cash operating cost (labour,
   chemicals and power) escalates at **3.2 %** under a separate index in the O&M contract; the
   remaining 30 % is fixed under long-term agreements. Compute revenue, cost, `EBITDA` and the
   `EBITDA` margin in years 1, 12 (loan maturity) and 25, the effective compound rates, and the
   value of the unindexed tariff slice.
2. **Formula.** Revenue in year `t` = `12,000,000 × [0.80 × 1.025^(t−1) + 0.20]`; cost in year
   `t` = `4,500,000 × 0.70 × 1.032^(t−1) + 4,500,000 × 0.30`. Effective rate = (year-25 value ÷
   year-1 value)^(1/24) − 1.
3. **Result.**

   | Year | Revenue | Cash operating cost | `EBITDA` | Margin |
   |---|---|---|---|---|
   | 1 | 12,000,000 | 4,500,000 | 7,500,000 | **62.50 %** |
   | 5 | 12,996,604 | 4,922,970 | 8,073,634 | 62.12 % |
   | 12 | 14,996,032 | 5,804,380 | 9,191,652 | **61.29 %** |
   | 25 | 19,763,769 | 8,058,467 | 11,705,302 | **59.23 %** |

   Effective compound rates: revenue **2.101 %**, cost **2.457 %** (a gap of **35.7 basis
   points**). Under full tariff indexation at the same 2.5 % the year-25 margin would be **62.87
   %** and `EBITDA` **13,646,244**; the present value at 8 % of the `EBITDA` forgone across 25
   years by indexing only 80 % is **6,204,143**, of which **2,619,217** falls inside the twelve
   loan years discounted at 6 %. Under a fully indexed cost base at 3.2 % the year-25 margin
   would be **51.51 %**.
4. **Interpretation.** **Neither contractual index is the rate that matters.** The headline gap
   between a 2.5 % tariff index and a 3.2 % cost index is 70 basis points; the *effective* rates
   that partial indexation actually produces are 2.101 % and 2.457 %, a gap of **35.7 basis
   points**. Half the apparent mismatch is neutralised by the 30 % of cost that does not escalate,
   so a negotiator working from headline indices has overstated the exposure twofold. The corrective
   is the same in both directions: **compute effective rates from shares and indices, and negotiate
   on those.**

   **The decomposition is where the money is, and it is counter-intuitive.** Of the 3.27 margin
   points lost by year 25, the unindexed 20 % of the tariff accounts for **3.65 points** (the
   year-25 margin under full tariff indexation is 62.87 %) while the fixed 30 % of the cost base
   *saves* **7.72 points** (a fully indexed cost base would end at 51.51 %). The single
   most valuable term in Kestrel's commercial package is therefore not the tariff index but the
   fixed-price portion of the O&M contract, worth more than twice what the unindexed tariff slice
   costs — an insight invisible unless the architecture is decomposed.

   **The drift is slow, which is why it is dangerous.** Nothing breaks: year-12 coverage is a
   comfortable **1.4253**, because the interest deduction has fallen and the tariff has risen
   faster than the covenant needs. The damage lands after the loan matures (in refinancing,
   handback economics and the equity return), so the party most exposed to indexation drift is
   the party least present in the financing negotiation. Price the whole concession, not the
   loan life: the tariff index a lender is indifferent to is the one shareholders live with for
   thirteen years after the lender has gone. **And an index the model assumes is not an index
   the contract grants.** CPI at 2.5 % is a forecast; entitlement to 80 % of CPI is a term. The
   first belongs in the sensitivity table, the second in the contract review, and confusing them
   is how projects end up with an escalation forecast where they needed an escalation right.

> **Fig 7.3.1 — Margin drift over the concession.** Line chart, x-axis concession years 1–25,
> y-axis `EBITDA` margin 50–64 %. Three lines from a common 62.50 % start: the contracted
> architecture (80 % of tariff at 2.5 %, 70 % of cost at 3.2 %) falling to **59.23 %**; full
> tariff indexation rising to a plateau and easing to **62.87 %**; a fully indexed cost base
> falling to **51.51 %**. A dashed vertical at year 12 marks loan maturity, showing that all
> three cases are still comfortable while the lender is present. Source: PCI original. Alt text:
> three margin curves diverging slowly over twenty-five years from a common starting point, the
> fully indexed cost case falling furthest, with the loan maturity marked early in the divergence.

### 7.3.2 Volume risk and operating leverage

**Definition.** **Operating leverage** is the amplification of a change in volume into a larger
proportional change in earnings, caused by costs that do not fall when volume does. The **degree
of operating leverage** is the ratio of contribution to `EBITDA`:

```
DOL = contribution ÷ EBITDA = (unit price − unit variable cost) × volume ÷ EBITDA
```

and the corresponding **`CFADS` elasticity** to a driver is the proportional change in `CFADS` per
one per cent change in that driver. Domain 6 (KA 6.4.2) computed Kestrel's `CFADS` elasticity to
**revenue** as **1.4098** and used it to convert coverage headroom into a revenue breakeven. This
topic supplies the mechanism and one distinction Domain 6 did not need.

**Worked example 7.3.2 — why a price miss hurts more than a volume miss.**

1. **Setup.** Kestrel's master thread: price 0.50 per m³, variable cost 0.0375 per m³, volume
   24,000,000 m³, fixed cash cost 3,600,000, `EBITDA` 7,500,000, `CFADS` 6,384,000. Compute the
   degree of operating leverage, the `CFADS` elasticity to price and to volume, and verify each
   against a finite move.
2. **Formula.** Contribution = `(0.50 − 0.0375) × 24,000,000`. From the bridge
   `CFADS = 0.75R − 0.03V − 1,896,000`: elasticity to price (revenue moving alone) =
   `0.75R ÷ CFADS`; elasticity to volume (revenue *and* variable cost and working capital moving
   together) = `(0.75 × 0.50 − 0.03) × V ÷ CFADS`.
3. **Substitution.** Contribution `11,100,000`; `DOL = 11,100,000 ÷ 7,500,000`. Price elasticity
   `9,000,000 ÷ 6,384,000`; volume elasticity `8,280,000 ÷ 6,384,000`.
4. **Result.** `DOL` **1.4800**. `CFADS` elasticity to **price 1.4098** (reproducing Domain 6's
   figure exactly); `CFADS` elasticity to **volume 1.2970**. Verification: a 20 % volume fall
   takes `CFADS` to 4,728,000, a fall of **25.94 %** — 20 × 1.2970; a 10 % price fall takes it to
   5,484,000, a fall of **14.10 %** — 10 × 1.4098.
5. **Interpretation.** **A price cut and a volume shortfall of the same revenue magnitude are not the
   same event.** Losing 10 % of revenue through price costs 14.10 % of `CFADS`; losing the same 10 %
   through volume costs 12.97 %, because lost volume brings relief on variable cost, cash tax and
   working-capital absorption. The 1.13-point difference is small at Kestrel because only 20 % of its
   cash cost is variable; where the variable share is 60 % the two elasticities diverge sharply, and a
   matrix treating "revenue down 10 %" as one scenario has merged two materially different ones.
   **Always stress the driver, not the revenue line.**

   **The generalisation is the useful part, and Domain 10 supplies its test case.** Domain 10's Case
   study B reported a toll road whose 12 % patronage shortfall produced a **17.9 %** fall in `CFADS`
   — an implied elasticity of **1.4931**. Since a road's cash costs are substantially fixed,
   elasticity in that limiting case is simply revenue divided by `CFADS`, so the reported outcome
   implies base revenue of **35,833,333** against cash costs and tax of **11,833,333**.
   The relationship generalises to a table every practitioner should be able to reconstruct, whose
   last column answers the only question a board asks:

   | `CFADS` as % of revenue | `CFADS` elasticity to volume (fixed costs) | Volume fall that takes a 1.30 × base to a 1.20 × covenant |
   |---|---|---|
   | 80 % | 1.2500 | **6.15 %** |
   | 67 % | 1.4925 | **5.15 %** |
   | 60 % | 1.6667 | **4.62 %** |
   | 40 % | 2.5000 | **3.08 %** |
   | 20 % | 5.0000 | **1.54 %** |

   Reaching a 1.20 × covenant from a 1.30 × base requires a `CFADS` fall of **7.6923 %** in
   every row; the demand tolerance that produces it ranges from **6.15 % to 1.54 %** purely on
   cost structure. **Two projects with the same coverage ratio and the same covenant can have
   demand tolerances that differ fourfold, and the ratio does not show it.** That is the most
   important thing this domain has to say to a credit committee, and it is why cost structure
   belongs in a revenue-risk discussion. High operating leverage is not a defect (it is what
   makes infrastructure profitable when demand holds), but it must be *known*, *disclosed* and
   *matched* by a structure that tolerates it: lower gearing, a larger reserve, a floor
   mechanism (KA 7.A.1) or sculpting.

### 7.3.3 Turning revenue risk into a tolerance

**The principle.** A ratio conveys no magnitude and an elasticity conveys no threshold; together
they convert a covenant into a sentence an operating team can act on. Domain 6 (KA 6.4.2)
established the breakeven translation as a modelling output; the commercial extension is to express
it in **each driver the business actually manages**, because different teams control different
drivers.

**Kestrel's tolerance set, in four units.** From a base `DSCR` of 1.2743, the 1.20 × covenant
requires `CFADS` of 6,011,562, a fall of 372,438, or **5.8339 %** (Domain 10). Dividing that
`CFADS` tolerance by each elasticity gives:

| Driver | Elasticity | Tolerance | Level at which the covenant fails |
|---|---|---|---|
| **Price / tariff** | 1.4098 | **4.14 %** | revenue 11,503,416 (Domain 6) |
| **Volume / despatch** | 1.2970 | **4.50 %** | 22,920,470 m³ — a loss of 1,079,530 m³ |
| **Availability** | — | **2.91 points** | 92.086 % availability (KA 7.1.3) |
| **`CFADS`** | 1.0000 | **5.83 %** | 6,011,562 |

Four numbers, one covenant, each right for a different conversation: the commercial team hears
4.14 % on tariff, operations hears 4.50 % on despatch or 2.91 points on availability, finance
hears 5.83 % on `CFADS`, and the board hears whichever is smallest. **Quoting the `CFADS`
tolerance to a team that manages volume overstates their room by 30 %**, the same error Domain 6
identified when it observed that a `CFADS` percentage overstates revenue room by 41 %. The
lock-up bites earlier still, at **22,194,436 m³**, a **7.52 %** volume fall; since lock-up is
what a sponsor actually feels, that is the threshold for the shareholder dashboard.

### AI in this KA

Escalation and elasticity work is arithmetically simple and definitionally treacherous: the
profile in which machine output is most confidently wrong. The legitimate applications are
narrow: **reconciling every escalating line to its index clause** (publisher, series, lag, cap,
floor, indexed share), which is Domain 3's escalation register populated by extraction rather
than transcription; **recomputing effective compound rates** across a whole revenue and cost
base; and **producing the tolerance table** for every covenant and driver combination on demand.

Two failure modes. **An assistant asked for "the escalation rate" returns the headline index, not
the effective rate**, because the headline is what the document says while the effective rate
requires the indexed share — the parameter most often omitted from a contract summary. Kestrel's
70-basis-point headline against a 35.7-basis-point effective gap is that error in one comparison.
**And an elasticity is a property of a case, not of a project** (Domain 6, KA 6.4.2): elasticities
computed on a generous case and quoted against a marginal one are meaningless, and machine-generated
sensitivity tables are frequently unlabelled as to case. Verification: recompute one escalating line
for one year from the clause text by hand; confirm the effective rate against first- and final-year
values; and require every elasticity to carry its case label and base value.

### Key terms — KA 7.3

| Term | Meaning |
|---|---|
| **Indexation architecture** | Which proportion of each line escalates, on which index — the negotiated structure, distinct from the index itself. |
| **Indexed share** | The proportion of a price that escalates; the remainder is fixed nominal and erodes in real terms. |
| **Effective escalation rate** | The compound rate a partly indexed line actually achieves; computable only from shares and indices together. |
| **Margin drift** | The slow change in margin caused by a mismatch between effective revenue and cost escalation. |
| **Operating leverage (`DOL`)** | Contribution ÷ `EBITDA`; the amplification of volume changes into earnings changes. |
| **`CFADS` elasticity** | Proportional change in `CFADS` per one per cent change in a driver; differs by driver. |
| **Tolerance** | The fall in a named driver that takes coverage to a threshold; the operational form of headroom. |

### Sample MCQs — KA 7.3

**MCQ 7.3-A `[7.3.1 · Application]`** A tariff indexes 80 % of its value at 2.5 % a year; the
remaining 20 % is fixed. Over 24 years the tariff's **effective** compound escalation rate is
closest to:
- A. 2.500 %
- B. 2.101 % ✅
- C. 2.000 %
- D. 3.125 %

*Rationale:* Year-25 revenue is `12,000,000 × (0.80 × 1.025²⁴ + 0.20) = 19,763,769`, giving
`(19,763,769/12,000,000)^(1/24) − 1 = 2.101 %`. A is the headline index applied as though the
whole tariff escalated; C is the naive `0.80 × 2.5 %`, which is an approximation valid only over
one period; D divides the index by the indexed share instead of multiplying.

**MCQ 7.3-B `[7.3.2 · Application]`** Price 0.50 per m³, variable cost 0.0375 per m³, volume
24,000,000 m³, `EBITDA` 7,500,000. The degree of operating leverage is:
- A. 1.4800 ✅
- B. 1.6000
- C. 1.2970
- D. 0.6757

*Rationale:* `contribution 11,100,000 ÷ EBITDA 7,500,000 = 1.4800`. B uses revenue rather than
contribution (`12,000,000/7,500,000`); C is the `CFADS` elasticity to volume, a different measure
computed after tax and working capital; D inverts the ratio.

**MCQ 7.3-C `[7.3.2 · Analysis]`** Two projects each report a base `DSCR` of 1.30 × against a
1.20 × covenant. Project M's `CFADS` is 80 % of revenue; Project N's is 40 %. Their demand
tolerances are:
- A. identical, since the coverage ratio and covenant are identical
- B. M 6.15 %, N 3.08 %: cost structure halves the tolerance at identical coverage ✅
- C. M 3.08 %, N 6.15 %
- D. indeterminate without the debt amount

*Rationale:* Both need a 7.6923 % `CFADS` fall; dividing by elasticities of 1.25 and 2.50 gives
6.15 % and 3.08 % (7.3.2). A is the error the table exists to correct; C reverses the
relationship: a thinner `CFADS` margin means higher elasticity and less tolerance; D is wrong
because the tolerance is a ratio property, independent of scale.

**MCQ 7.3-D `[7.3.3 · Analysis]`** Kestrel's `CFADS` tolerance to its covenant is 5.83 %. The
figure that should be given to the operations team, which manages despatch, is:
- A. 5.83 %, the `CFADS` tolerance
- B. 4.50 %, the volume tolerance, being 5.83 % divided by the `CFADS` elasticity to volume of 1.2970 ✅
- C. 4.14 %, the revenue tolerance
- D. 7.52 %, the lock-up tolerance

*Rationale:* Each team needs the tolerance in the driver it controls; quoting the `CFADS` figure
to a volume-managing team overstates their room by about 30 % (7.3.3). C is the correct figure
for the commercial team, which sets tariff, not for despatch; D is the correct volume figure for
the *lock-up*, a different and later threshold.

**MCQ 7.3-E `[7.3.1 · Comprehension]`** A colleague asks what the "indexed share" of a tariff is and
why it is recorded separately from the index. The best explanation is:
- A. it is the proportion by which the tariff rises each year
- B. it is the proportion of the tariff's value that escalates; the remainder is a fixed nominal amount eroding in real terms, so the tariff's effective compound rate is lower than its headline index ✅
- C. it is the share of the tariff denominated in the indexed currency
- D. it is the ceiling on annual escalation the contract permits

*Rationale:* Kestrel indexes 80 % of its tariff to an index assumed at 2.5 %, which compounds to
an **effective** 2.101 % over 24 years rather than 2.5 % (7.3.1). A describes the index rate
itself; C invents a currency mechanic; D names a different negotiated parameter: a cap limits
the index in a period, while the indexed share limits the base the index applies to.

**MCQ 7.3-F `[7.3.1 · Evaluation]`** Kestrel's O&M contractor offers a swap: it will index the
currently fixed 30 % of the cost base at the same 3.2 %, and in exchange the offtaker will index
100 % of the tariff at 2.5 % rather than 80 %. On the assumed rates the year-25 `EBITDA` margin
moves from 59.23 % to **55.85 %** (a loss of 3.38 points), while the present value of `EBITDA`
over the 25-year concession at 8 % **rises by 1,516,002**, full tariff indexation adding
6,204,143 and full cost indexation removing 4,688,141. The soundest recommendation is:
- A. reject: the fixed 30 % of the cost base saves 7.7164 margin points by year 25, more than twice the 3.6462 that full tariff indexation adds
- B. accept: 1,516,002 of present value at 8 % over the concession is a real gain, and margin points at a single year are not the decision metric
- C. accept, but only against a cap on the O&M index, because the 1,516,002 holds only at the assumed 3.2 % and the swap exchanges a fixed obligation for an unbounded indexed one ✅
- D. accept, and seek a matching cap on the tariff index so that both sides are symmetrical

*Rationale:* the two metrics genuinely disagree — the year-25 margin falls while discounted value
rises, because the cost escalation compounds on a smaller base and lands later, where discounting
bites — and B is right to prefer the discounted figure. What B misses, and C supplies, is that
3.2 % is a *forecast* while indexation is a *term* (7.3.1): the tariff's gain is bounded by its
index, the cost's loss is not, so the swap is sound conditioned on a cap and speculative without one.
A applies the correct decomposition to the wrong horizon, comparing snapshot margin points instead of
value over the concession. D concedes the one thing worth keeping, capping the escalation the project
receives in order to cap the escalation it pays.

**MCQ 7.3-G `[7.3.2 · Evaluation]`** A credit paper presents two projects as equivalent credits: both
report a base `DSCR` of 1.30 × against a 1.20 × covenant. Project M's `CFADS` is 80 % of revenue and
Project N's is 40 %, so the same 7.6923 % `CFADS` fall is reached by a 6.15 % demand fall in M and a
3.08 % fall in N. The soundest position is that:
- A. they are equivalent for credit purposes, since coverage and covenant are identical
- B. cost structure must be reported beside the ratio, and high operating leverage matched by
  structure (lower gearing, a larger reserve, a floor mechanism or sculpting), rather than
  treated as a defect ✅
- C. Project N is simply the weaker credit and should be declined
- D. Project N's covenant should be raised to 1.30 × so that the two tolerances are equalised

*Rationale:* two projects with the same ratio can have demand tolerances differing fourfold, and
the ratio does not show it (7.3.2), which is why cost structure belongs in a revenue-risk
discussion. A is the error the generalised table exists to correct. C treats leverage as a
fault, when it is what makes infrastructure profitable while demand holds; the requirement is
that it be known, disclosed and matched. D moves the wrong lever: raising the covenant reduces
N's tolerance further, since it shortens the distance from base coverage to the trigger.

### Self-check — KA 7.3

1. *Why is the headline index not the rate that matters?* Because the indexed share determines
   the effective rate; Kestrel's 70-basis-point headline gap is a 35.7-basis-point effective
   gap.
2. *Why does a price miss cost more `CFADS` than a volume miss of equal revenue?* Lost volume
   brings relief on variable cost, cash tax and working capital; a price cut brings none.
3. *State the one sentence that converts a covenant into an operational instruction.* "Coverage
   breaks at a 4.50 % fall in despatch, or 1,079,530 m³ of the 24,000,000 forecast."

---

## Knowledge Area 7.4 — Counterparty credit quality and revenue stress testing

*Topics: 7.4.1 counterparty credit and expected loss · 7.4.2 concentration and credit
enhancement · 7.4.3 revenue stress testing and the bank case.*

### 7.4.1 Counterparty credit and expected loss

**Definition.** A contracted revenue stream is a claim on a counterparty, and its value is bounded
by that counterparty's ability and willingness to pay. **Expected loss** decomposes it into three
estimable parts:

```
Expected loss = probability of default × exposure at default × loss given default
```

where **probability of default** (`PD`) is the likelihood of failure to perform over a stated
horizon, **exposure at default** (`EAD`) is the amount at risk when it happens, and **loss given
default** (`LGD`) is the proportion of that exposure not recovered. In project finance the
exposure that matters is rarely the outstanding receivable: it is the **present value of the
contracted stream that would be lost**, because the loss event is termination of the revenue, not
non-payment of an invoice.

**Worked example 7.4.1 — what Kestrel's single offtaker is worth as a credit.**

1. **Setup.** Kestrel's water-purchase agreement runs 25 years with a single regional water
   authority. Mapping its published rating to an internal grade gives an **annual `PD` of 0.60
   %**. If the offtaker fails, Kestrel can re-sell water into a regional merchant market at a
   discount and expects to recover 55 % of the contracted value, so `LGD` is **45 %**. Exposure
   is taken as the present value of `CFADS` over the loan life at the loan rate, the amount the
   lenders are relying on. Compute the twelve-year cumulative `PD`, the exposure, the expected
   loss, and the coverage ratio net of an annualised credit charge. (Rating mappings, recovery
   assumptions and regulatory capital treatments are institution- and jurisdiction-specific; the
   arithmetic transfers, the parameters do not.)
2. **Formula.** Cumulative `PD` over `n` years = `1 − (1 − PD)ⁿ`; `EAD` = `CFADS × AF(0.06, 12)`;
   `EL = cumulative PD × EAD × LGD`; annualised credit charge = `EL ÷ AF(0.06, 12)`.
3. **Substitution.** `1 − 0.994¹² `; `6,384,000 × 8.383844`;
   `0.069671 × 53,522,460 × 0.45`; `1,678,031 ÷ 8.383844`.
4. **Result.** Twelve-year cumulative `PD` **6.9671 %**; `EAD` **USD 53,522,460**; expected loss
   **USD 1,678,031**, which is **3.135 %** of exposure; annualised credit charge **200,151**,
   reducing `CFADS` to 6,183,849 and the `DSCR` to **1.2344**.
5. **Interpretation.** The last line is the one worth carrying: **charging the counterparty's
   expected loss against coverage costs Kestrel 0.0400 of `DSCR`** (**53.7 %** of its total
   covenant headroom of 0.0743), and the project is still compliant. That is a defensible
   position, and stating it that way converts an opinion ("the offtaker is investment grade")
   into a quantified one ("the credit is worth four hundredths of a coverage ratio, against
   0.0743 of headroom").

   Three cautions belong with the arithmetic, each a way the calculation is routinely abused.
   **`PD` is not a fact.** It is a mapping from a rating or a model to a number, on a horizon,
   in a state of the economy; a public-sector offtaker's `PD` in particular is often taken from
   sovereign or sub-sovereign proxies that assume a support relationship the documents may not
   create. Present it with its source and horizon, and move it in the sensitivity table like any
   other assumption. **`EAD` depends on what the loss event is**, and choosing it is a judgment:
   the receivable (2,958,904 on 90-day terms — trivial), the present value of contracted `CFADS`
   over the loan life (53,522,460 — the lenders' exposure, used above), or the present value
   over the whole concession (much larger — the shareholders'). Three exposures, three expected
   losses, one correct answer per decision, and quoting the wrong one is how a concentration
   problem gets presented as a working-capital matter. **And `LGD` embeds a market and a legal
   outcome.** Kestrel's 45 % assumes a merchant market exists to re-sell into and that
   termination compensation, step-in rights and the security package work as drafted: matters
   for Domain 12 and, in a real transaction, for qualified counsel in the relevant jurisdiction.
   An `LGD` not traced to those documents is a guess wearing a decimal point.

### 7.4.2 Concentration and credit enhancement

**The result that matters.** Expected loss is a linear function of exposure, so **it cannot see
concentration**. Splitting one counterparty into four of equal size and equal `PD` leaves
expected loss exactly unchanged, while changing the loss distribution beyond recognition. A
credit committee shown only expected loss has been shown a number that is blind to the risk the
committee exists to control.

**Worked example 7.4.2 — one offtaker or four, and what enhancement is worth.**

1. **Setup.** Compare Kestrel's single offtaker (annual `PD` 0.60 %, twelve-year cumulative
   6.9671 %, exposure 53,522,460, `LGD` 45 %) with a hypothetical structure in which four
   offtakers each take 25 % of the output on identical terms, with independent defaults. Compute
   expected loss in each, and the probability of losing at least half of the revenue and of
   losing all of it. Then price a sovereign guarantee that reduces the effective annual `PD` to
   **0.20 %** for a fee of **0.35 % a year on the outstanding debt balance**, against both its
   expected-loss benefit and its effect on required coverage: the lenders having indicated that
   a guaranteed structure would be sized at **1.20 ×** rather than **1.30 ×**.
2. **Formula.** Four-counterparty default counts follow the binomial distribution with `n = 4`
   and `p = 0.069671`; losing at least half the revenue requires at least two defaults. Guarantee
   fee present value = Σ (0.35 % × opening balance in year `t`) × `DF(0.06, t)` over the
   amortisation schedule of Domain 3. Debt capacity as in Domain 10.
3. **Result.**

   | Measure | One offtaker | Four offtakers at 25 % |
   |---|---|---|
   | Expected loss | **1,678,031** | **1,678,031** — identical |
   | P(no default) | 93.0329 % | 74.9111 % |
   | P(exactly one default) | — | 22.4399 % |
   | P(losing ≥ 50 % of revenue) | **6.9671 %** | **2.6489 %** |
   | P(losing all revenue) | **6.9671 %** | **0.0024 %** |

   Splitting the offtake reduces the probability of a revenue loss of half or more by **61.98 %**
   and the probability of total loss by a factor of about 2,957 — with **no change whatever in
   expected loss**. On the guarantee: expected loss falls from 1,678,031 to **571,726**, a
   reduction of **1,106,304**, against a fee whose total is 1,056,745 and whose present value at
   6 % is **805,901** — an expected-loss gain of only **300,403**. But the coverage step from
   1.30 × to 1.20 × raises debt capacity from 41,171,123 to **44,602,050**, a gain of
   **3,430,927**, or **2,625,026** net of the fee's present value.
4. **Interpretation.** **Two structures with identical expected loss are not equally
   creditworthy, and that difference is the whole subject of concentration risk.** The four-way
   structure is far more likely to suffer a *small* loss (a 22.4 % chance of one default against
   a 6.97 % chance of any default at all in the single-offtaker case), and far less likely to
   suffer a *fatal* one. Since a project can absorb a quarter of its revenue disappearing for a
   period but not all of it, the four-way structure is materially better credit while reporting
   the same expected loss. **Report the loss distribution, or at minimum the probability of
   crossing the thresholds the structure cannot survive, and never let expected loss stand
   alone.** Kestrel's largest revenue risk is not the offtaker's credit quality (0.60 % a year
   is respectable) it is that the offtaker is one entity, and diversification is unavailable
   because a single regional authority is the only buyer of the water. Where concentration
   cannot be diversified away it must be **structured** against: guarantees, letters of credit,
   escrow, step-in rights, termination compensation that repays debt, or lower gearing.

   **And credit enhancement is bought for the coverage it unlocks, not the expected loss it
   removes.** On expected-loss grounds the guarantee barely earns its fee: 300,403 of net
   benefit is inside the noise of the `PD` assumption. On coverage grounds it releases
   **2,625,026** of equity net of cost, because it changes the *case the lender underwrites* and
   therefore the divisor in Domain 10's sizing arithmetic. That is how experienced sponsors
   evaluate every mitigant: a letter of credit, a parent guarantee, an escrow account and a
   termination-compensation regime are all priced by asking what they do to the required ratio
   and the underwritten case, then comparing that with the cost of the equity they release.
   Expected loss is the wrong metric for the decision, though it remains the right metric for
   the provision.

### 7.4.3 Revenue stress testing and the bank case

**Definition.** **Revenue stress testing** moves the commercial drivers to levels the structure
must survive and reports the outputs the decision turns on — for a financing, coverage and
liquidity, not value. **Reverse stress testing** inverts the question, asking **what combination
of driver movements reaches a defined failure point**: the form that produces an answer
management can monitor.

**Worked example 7.4.3 — Kestrel's stress matrix and its reverse.**

1. **Setup.** Move tariff (price) and despatch (volume) jointly across the ranges a market
   adviser considers credible, and report year-one `DSCR`. Then find the volume required to hold
   the 1.20 × covenant at a 5 % tariff reduction.
2. **Formula.** `CFADS = 0.75R − 0.03V − 1,896,000` with `R = 0.50 × price factor × V`;
   `DSCR = CFADS ÷ 5,009,635.23`. Reverse: solve
   `V × (0.375 × price factor − 0.03) = 6,011,562 + 1,896,000`.
3. **Result.** Year-one `DSCR`:

   | Tariff \ Despatch | −20 % | −10 % | base | +10 % |
   |---|---|---|---|---|
   | **+5 %** | 1.0156 | 1.1899 | 1.3642 | 1.5384 |
   | **base** | 0.9438 | 1.1091 | **1.2743** | 1.4396 |
   | **−5 %** | 0.8719 | 1.0282 | 1.1845 | 1.3408 |
   | **−10 %** | 0.8001 | 0.9474 | 1.0947 | 1.2420 |

   At a 5 % tariff cut, despatch must **rise 0.9906 %** (to 24,237,739 m³) to hold 1.20 ×.
4. **Interpretation.** Read the matrix by its boundaries, not its cells. **Six of the sixteen
   cells clear the 1.20 × covenant, and not one of them at despatch below forecast**: the whole
   +10 % despatch column clears, base despatch clears only at base tariff and above, and every
   cell at −10 % despatch or worse fails. The covenant contour therefore runs down the
   right-hand edge and steps left only in the top two tariff rows: the visual statement that
   Kestrel has almost no joint tolerance, and none at all once volume falls. **Four cells fall
   below 1.00 ×**, three of them in the −20 % despatch column, where debt service cannot be paid
   from operating cash at all and the debt-service reserve (2,504,818, Domain 10 KA 10.3.2) is
   the difference between a difficult year and a payment default. Two cells tie the matrix to
   the rest of the book: the base-despatch, −10 % tariff cell is **1.0947**, exactly Domain 6's
   revenue-down-10 % figure, and extending the tariff row to +10 % reproduces Domain 6's
   **1.4540**.

   **The reverse stress is the more useful instrument, and the one usually missing.** A 5 %
   tariff reduction cannot be recovered by any volume response: 0.99 % more despatch is
   attainable in principle, but Kestrel has no *right* to more despatch, because volume is the
   offtaker's choice, so the recovery lever does not exist. That is what a reverse stress test
   is for. It identifies not just the failure point but **whether the project holds the lever
   that would avoid it**, and a breakeven reported without asking who controls the driver is
   arithmetic without decision content.

   Three disciplines complete the practice. **Stress drivers, not outputs**; "revenue −10 %" is
   two different stresses with different elasticities (KA 7.3.2). **Stress in correlated
   bundles**, because a demand recession arrives with a price response and a delayed indexation
   catch-up, and Domain 6 (KA 6.4.2) showed that one-at-a-time analysis is additive on this
   model and therefore says nothing about joint probability. **And report the minimum over the
   loan life**. This matrix is a year-one snapshot, and Domain 6's Fig 6.4.1 showed the
   bank-case minimum falling to 1.1851 in year 12 with no revenue stress at all, so every cell
   here is optimistic as a statement about the loan's worst year.

### AI in this KA

Counterparty and stress work divides cleanly. **Machines should**: monitor counterparty credit
signals continuously (rating actions, payment behaviour, filings, published statements), and
raise exceptions, which is surveillance no human team performs consistently across a portfolio;
generate and run large stress grids including the correlated bundles a workshop would never
enumerate; and extract guarantee, letter-of-credit and termination-compensation terms into the
structured form Toolkit 7.T.3 requires.

**Before any of it, the data question, which comes first.** A counterparty credit assessment is
material about an identified third party, assembled from sources with their own terms of use and
often held under a confidentiality undertaking given to that counterparty; the guarantee and
termination-compensation terms being extracted are frequently unsigned. All of it is processed
**only in an environment approved for that data classification and permitted by the
confidentiality undertakings that cover it**, and establishing that permission is a precondition
of the task rather than a review of it. Where the assessment records information about
identified individuals (a counterparty's directors, owners or politically exposed persons in its
control chain) the data-protection basis for holding and processing it is settled with the
organisation's data-protection adviser before the file is built, and the retention position is
set at the same time (Toolkit 7.T.3). Domain 1, KA 1.3.4 states the rule; Domain 16 builds the
machinery.

**They must not**: assign a `PD`, `LGD` or recovery assumption that becomes an input to a
financing decision without a named professional adopting it, because each embeds a legal and
market judgment a model states with unwarranted confidence; conclude whether facts constitute a
counterparty default or trigger termination compensation, which is a matter for qualified
counsel; or select the bank case. One failure mode deserves naming: asked to assess a
concentrated revenue structure, a model readily computes expected loss (that is the formula in
its training data), and does not volunteer that expected loss is blind to concentration, which
is the entire question. **Ask for the distribution, not the expectation**, and check that the
threshold used is one the structure cannot survive rather than an arbitrary confidence level.

### Key terms — KA 7.4

| Term | Meaning |
|---|---|
| **`PD` / `EAD` / `LGD`** | Probability of default over a stated horizon · exposure when it occurs · proportion not recovered. |
| **Expected loss** | `PD × EAD × LGD`; linear in exposure, and therefore blind to concentration. |
| **Cumulative `PD`** | `1 − (1 − PD)ⁿ`; the horizon matters and single-year `PD` understates a twelve-year exposure. |
| **Concentration risk** | The exposure created by dependence on few counterparties; visible in the loss distribution, not in expected loss. |
| **Credit enhancement** | Guarantee, letter of credit, escrow or reserve that improves the credit; priced against the coverage it unlocks. |
| **Reverse stress test** | Solving for the driver movements that reach a defined failure point, rather than testing given movements. |

### Sample MCQs — KA 7.4

**MCQ 7.4-A `[7.4.1 · Application]`** Annual `PD` 0.60 %, exposure 53,522,460, `LGD` 45 %, over a
twelve-year loan life. Expected loss is closest to:
- A. USD 144,511
- B. USD 1,678,031 ✅
- C. USD 3,728,957
- D. USD 24,085,107

*Rationale:* Twelve-year cumulative `PD` is `1 − 0.994¹² = 6.9671 %`, so
`0.069671 × 53,522,460 × 0.45 = 1,678,031`. A applies the single-year `PD`, understating the
twelve-year exposure by a factor of eleven; C omits `LGD`, treating default as total loss;
D omits `PD` entirely.

**MCQ 7.4-B `[7.4.2 · Analysis]`** A single offtaker taking 100 % of output is replaced by four
independent offtakers taking 25 % each, with identical `PD` and `LGD`. Expected loss:
- A. falls by 75 %
- B. is unchanged, while the probability of losing at least half the revenue falls from 6.9671 % to 2.6489 % ✅
- C. falls to one quarter of its previous value
- D. rises, because there are four counterparties who might default

*Rationale:* Expected loss is linear in exposure, so splitting it changes nothing; the loss
*distribution* changes profoundly (7.4.2). A and C confuse per-counterparty exposure with total
expected loss; D confuses the probability of *some* default (which does rise, to 25.09 %) with
expected loss.

**MCQ 7.4-C `[7.4.2 · Analysis]`** A sovereign guarantee reduces expected loss by 1,106,304 at a
fee with a present value of 805,901, and would allow the lenders to size at 1.20 × rather than
1.30 ×, raising debt capacity from 41,171,123 to 44,602,050. The correct basis for the decision
is:
- A. reject it: the expected-loss saving of 300,403 net is immaterial
- B. accept it: it releases 3,430,927 of debt capacity, or 2,625,026 net of the fee's present value, which is what credit enhancement is bought for ✅
- C. accept it because guarantees always improve bankability
- D. the two effects cannot be compared

*Rationale:* Enhancement is priced against the coverage and case it unlocks, not the provision it
reduces (7.4.2). A applies the wrong metric to the decision, though it is the right metric for
the provision; C is an unsupported generality; D is false — both effects are in present-value
terms.

**MCQ 7.4-D `[7.4.3 · Analysis]`** A stress matrix shows that six of sixteen tariff and despatch
combinations clear the 1.20 × covenant (none of them at despatch below forecast), and that a 5 %
tariff cut requires despatch 0.99 % above forecast to remain compliant. The most valuable
observation for management is:
- A. the matrix should be widened until more cells comply
- B. the project has almost no joint tolerance, and it does not hold the lever (despatch is the
  offtaker's choice) that would recover a tariff cut ✅
- C. the year-one figures understate the risk, so the matrix should be discarded
- D. the covenant should be renegotiated to 1.00 ×

*Rationale:* A reverse stress test must identify both the failure point and whether the project
controls the driver that would avoid it (7.4.3). A is presentational dishonesty; C is half
right. Year-one figures are optimistic, so the matrix should be *extended* to the minimum year,
not discarded; D mistakes a covenant for the problem.

**MCQ 7.4-E `[7.4.1 · Analysis]`** Kestrel's offtaker exposure can be measured three defensible
ways: the receivable of **2,958,904** on 90-day terms, the present value of contracted `CFADS` over
the twelve loan years at 6 % (**53,522,460**), or the present value over the whole 25-year concession,
which is larger again. The credit committee is deciding whether to advance 41,171,123. The exposure it
should be shown is:
- A. the receivable, since that is the amount actually owed at any moment
- B. 53,522,460, because the loss event for this decision is the loss of the contracted stream over the period being lent against, not an unpaid invoice ✅
- C. the whole-concession figure, being the largest and therefore the most prudent
- D. all three, averaged, so that the committee neither over- nor understates

*Rationale:* `EAD` follows from what the loss event is, and choosing it is a judgment tied to a
decision (7.4.1). A is the working-capital exposure, trivial by comparison, and quoting it is
exactly how a concentration problem gets presented as a working-capital matter. C is genuinely
defensible and is the right number for a *different* decision (the sponsors' own exposure across
the concession), but selecting it here because it is the largest substitutes an instinct for
prudence for the question asked, and it overstates what the lenders are relying on. D is
arithmetic without meaning: three exposures answer three questions and their mean answers none.

**MCQ 7.4-F `[7.4.3 · Evaluation]`** The stress matrix clears the 1.20 × covenant in six of
sixteen cells and falls below 1.00 × in four, and the unstressed bank case already reaches a
year-twelve minimum of 1.1851 (Domain 6, Fig 6.4.1). Four requirements are proposed. Which should the
committee impose first?
- A. extend the matrix to the loan's minimum year, since every cell is a year-one snapshot and therefore optimistic about the loan's worst year ✅
- B. widen the tariff and despatch ranges beyond the market adviser's credible bounds
- C. attach a probability to each cell so that the committee can weigh the outcomes
- D. increase the debt-service reserve from six months to twelve, because four cells cannot pay debt service from operating cash

*Rationale:* the matrix understates the problem before any stress is applied: coverage already
falls to 1.1851 by year twelve on the unstressed bank case, so ten failing cells is a floor
rather than a finding (7.4.3). D is a real mitigant applied in the wrong order: a reserve sized
against a mis-stated worst year is sized against the wrong number, and the right sequence is
measure, then mitigate. C is what committees usually ask for and is the more dangerous request:
joint probabilities here would have to be invented, and a probability-weighted matrix built on
assumed correlations implies knowledge nobody has (Domain 6, KA 6.A.1). B changes nothing, since
the ranges are already the adviser's credible bounds and extending them past that only adds
cells nobody will underwrite.

**MCQ 7.4-G `[7.4.2 · Evaluation]`** A credit paper records that the sole offtaker is investment grade,
with an annual `PD` of 0.60 %, a twelve-year cumulative `PD` of 6.9671 % and an expected loss of
1,678,031 on exposure of 53,522,460. A single regional authority is the only buyer of the water. The
soundest assessment of the paper is that it:
- A. is adequate: expected loss is the standard measure and the counterparty's grade is strong
- B. is inadequate because a 0.60 % annual `PD` is implausibly low for a sub-sovereign counterparty
- C. is incomplete: expected loss is identical whether one counterparty or four carry the same
  revenue, so the paper must also state the probability of losing more revenue than the structure can
  survive and what has been structured against it ✅
- D. is inadequate, and the remedy is to split the offtake among four independent payers

*Rationale:* expected loss is linear in exposure and therefore blind to concentration; four
payers would show the same 1,678,031 while the probability of losing half or more of the revenue
falls from 6.9671 % to 2.6489 % and of losing all of it to 0.0024 % (7.4.2). A lets the
expectation stand alone on a concentrated base. B attacks the one parameter the paper at least
sources, and misses the omission. D prescribes a remedy this market does not offer: where
concentration cannot be diversified away it must be structured against, guarantees, letters of
credit, escrow, step-in rights, termination compensation that repays debt, or lower gearing.

**MCQ 7.4-H `[7.4.1 · Comprehension]`** Kestrel's `LGD` of 45 % rests on an expectation of recovering
55 % of contracted value. What that figure embeds is:
- A. the proportion of an unpaid invoice that is written off
- B. a market judgment, that a merchant market exists to re-sell into, together with a legal
  one, that termination compensation, step-in rights and the security package work as drafted ✅
- C. a figure fixed for each counterparty class by regulation, and therefore not an assumption at all
- D. the probability that recovery efforts fail

*Rationale:* `LGD` is the proportion of exposure not recovered, and on a project the recovery
depends on both a market and a set of documents, so an `LGD` not traced to them is a guess
wearing a decimal point (7.4.1). A describes a receivable write-off, not the loss of a
contracted stream. C states as universal a treatment that is institution- and
jurisdiction-specific. D is `PD`'s territory, likelihood, not the severity `LGD` measures.

### Self-check — KA 7.4

1. *Why does expected loss understate a concentrated exposure?* It is linear in exposure, so it
   is identical whether one counterparty or four carry the same total; only the loss
   distribution shows the difference.
2. *On what basis is credit enhancement priced?* (On the coverage and underwritten case it
   unlocks, and therefore the equity it releases) 2,625,026 net for Kestrel's guarantee — not on
   the expected loss it removes.
3. *What must a reverse stress test establish beyond the breakeven?* Whether the project
   controls the driver that would avoid the failure.

---

## Advanced topics — Domain 7

### 7.A.1 The collar: a minimum revenue guarantee with revenue sharing

A **minimum revenue guarantee** (`MRG`) is a floor the grantor writes; **revenue sharing** is a
ceiling it buys back. Together they form a **collar**, which can be structured to cost the
grantor nothing in expectation while transforming bankability (making it the most efficient
instrument in this domain).

Take Kestrel's Structure B distribution and add an `MRG` at **10,800,000** (90 % of base revenue)
with **100 % revenue sharing above 13,200,000** (110 %). In the low outcome the grantor tops up by
1,200,000; in the high outcome it recovers 1,200,000; in the base outcome nothing happens. The
**expected net transfer is exactly nil** and **expected `CFADS` is unchanged at 6,384,000**. What
changes is the distribution: `CFADS` outcomes narrow from 4,728,000–8,040,000 to
**5,628,000–7,140,000**, `DSCR` outcomes from 0.9438–1.6049 to **1.1234–1.4253**, and debt capacity
sized on the low case at 1.30 × rises from 30,491,396 to **36,295,595** — a gain of **5,804,200 for
nothing in expectation.**

Three professional points. The collar is **not free to the grantor**, whose exposure has moved
from an expectation to a contingency: a 25 % chance of paying 1,200,000 is a real budgetary and
accounting matter in its own jurisdiction and framework, and it is why guarantees of this kind
are increasingly recognised as contingent liabilities rather than treated as costless policy. It
is **not free to equity** either: the shared upside is the shareholders', and 5,804,200 of extra
debt is 5,804,200 of leverage risk. And **it does not fix compliance**: the collared low case at
1.1234 still breaches both the 1.20 × covenant and the 1.15 × lock-up, so a floor set at the
lock-up level is worth far more than a floor at a round percentage of base revenue, and costs
the grantor only slightly more.

### 7.A.2 The merchant tail and the contracted horizon

The tenor a project can support is set by its **contracted horizon**, not its asset life.
Suppose Kestrel's offtake ran eight years rather than twenty-five, with output thereafter sold
merchant at 70 % of the contracted tariff: merchant-year revenue is 8,400,000, `CFADS`
**3,684,000** and `DSCR` on the existing instalment **0.7354** (the loan cannot be paid in those
years). Three structural responses, each with a computed price:

- **Shorten the tenor to the contracted horizon.** Eight years at 1.30 × on contracted `CFADS`,
  with `AF(0.06, 8) = 6.209794`, supports **30,494,864** (10,676,259 less than the twelve-year
  contracted structure, and it creates refinancing risk at year eight).
- **Keep twelve years with level service.** The weakest period governs: 3,684,000 ÷ 1.30 =
  2,833,846 of service, supporting only **23,758,524**. Level service against a cliff is the
  worst of the three.
- **Sculpt, with a higher target in the merchant years.** At 1.30 × through the contracted years
  and 1.80 × through the merchant years, sculpted capacity is **34,944,420**: the best answer,
  and the reason sculpting (Domain 10, KA 10.1.3) is close to mandatory where a contract cliff
  sits inside the tenor.

The leadership point is that the second option, which looks like the conservative one, destroys
**11,185,896** of capacity by pricing every year at the worst year's rate. And all three answers
depend on a merchant price forecast for years nine to twelve: a market forecast embedded in a
debt structure, which is Domain 10's discipline on refinancing assumptions applied to revenue.

### 7.A.3 The reviewer's revenue eye

Invariants to test on any revenue model or commercial structure:

- Base-case revenue reconciles to the tariff formula applied to base-case volume, clause by
  clause, including every deduction, cap and multiplier.
- The capacity or availability charge covers fixed cash costs, debt service and return; the
  variable charge covers variable costs at contracted efficiency and no more (7.1.2).
- Expected revenue and expected `CFADS` are reported **with** the low case and the coverage each
  delivers; a case labelled "expected" never appears as a sizing basis (7.1.1).
- Every escalating line carries an index, a lag and an **indexed share**, and the model's
  effective compound rate reconciles to first-year and final-year values (7.3.1).
- Revenue and cost effective escalation rates are compared, and the margin at loan maturity and
  at concession end are both reported (7.3.1).
- `CFADS` elasticity is stated separately for price and for volume, with its case label and base
  value (7.3.2).
- Every covenant is expressed as a tolerance in each driver the business manages, and the
  smallest tolerance is identified (7.3.3).
- Counterparty exposure is stated as the present value of the stream at risk, on a named horizon,
  with cumulative rather than single-year `PD` (7.4.1).
- Revenue concentration is reported as a distribution or as the probability of crossing the
  thresholds the structure cannot survive — never as expected loss alone (7.4.2).
- Contracted revenue tenor is compared with loan tenor, and any uncontracted years inside the
  tenor are separately sized and separately priced (7.A.2).
- Stress is applied to drivers, in correlated bundles, and reported on the **minimum** period over
  the loan life (7.4.3).

---

## Industry variations — Domain 7

- **Contracted power and renewables.** Two-part tariffs and capacity payments dominate; the
  specific difference is **curtailment and deemed generation**, whether the offtaker pays for
  output it instructs the plant not to produce, a drafting question worth several points of
  availability. Under a contract-for-differences the project sells merchant and settles the
  difference against a strike: economically contracted, operationally merchant, and modelled
  wrongly whenever the two are conflated.
- **Merchant power and commodities.** There is no contracted case, so the **price deck is the
  commercial structure**: lenders size on a conservative deck, require coverage well above 1.5 ×,
  shorten tenors and often demand a hedge over a defined share of output. The specific difference
  is that the hedge is itself a counterparty exposure, so KA 7.4 applies to the mitigant as well as
  to the revenue.
- **Transport concessions.** Patronage risk with a ramp; the specific difference is that **the
  project often controls price but faces elasticity**, so a toll increase to recover volume may
  reduce revenue. Operating leverage is extreme (Domain 10's Case study B implies an elasticity
  of 1.4931), so tolerance is thin at any coverage ratio, and ramp reserves and sculpting are
  standard rather than optional.
- **Water and regulated utilities.** Availability or take-or-pay with a public counterparty; the
  specific difference is the **regulatory reset**, a scheduled discontinuity no escalation formula
  describes. Covenant testing and reserve sizing must straddle resets, and the indexation
  architecture applies only between them.
- **Digital infrastructure.** Contracted revenue with short tenors relative to asset life; the
  specific difference is that **re-contracting risk replaces volume risk**, so `WARCT` against
  tenor is the binding metric (7.2.3), tenant concentration is usually severe, and required coverage
  is driven by re-letting assumptions and tenant credit rather than demand forecasts.
- **Social infrastructure PPP.** Pure availability payments from a public budget; the specific
  difference is that **the deduction and performance regime is the entire revenue risk**. With
  no demand exposure at all, diligence and management attention belong on the
  performance-measurement system, the multiplier, the cumulative-deduction cap and the cure
  regime, which KA 7.1.3 showed can move a covenant breakpoint by 1.584 points of availability.

---

## Case study — Domain 7: the concession that was worth less than it looked (water)

**Situation.** Kestrel's tariff negotiation reached indexation last, as it usually does. The
offtaker's position was **80 % of the tariff indexed to CPI**, assumed at 2.5 %, with 20 % fixed
for the 25-year concession. The sponsors' commercial director, uncomfortable with an unindexed
slice, asked for **full indexation**; the offtaker agreed (at **CPI less 50 basis points**,
presented as a fair exchange). The finance director asked for the calculation before the term
sheet was initialled.

**What happened.** The exchange was value-destroying. Present value of revenue over 25 years at
8 %: 80 % at 2.5 % gives **152,913,886**; 100 % at 2.0 % gives **152,088,430** — the "full
indexation" offer was worth **825,456 less**, and 166,192 less over the twelve loan years at 6
%. The mechanism is not obvious, and that is the point: the unindexed 20 % compounds at nothing,
but the indexed 80 % compounds at the higher rate, so by year 25 the partly indexed tariff
reaches 19,763,769 against the fully indexed one's 19,301,247. Minimum `DSCR` over the loan life
was 1.2743 in year one under both and year-12 coverage was 1.4253 against 1.4140, so **no
coverage test would have detected the loss**. A covenant-compliant structure can be materially
worse than the alternative and nothing in the coverage model will say so.

The second finding was larger. Decomposing the architecture (Worked example 7.3.1) showed the
year-25 margin falling from 62.50 % to **59.23 %**, and the **effective** rate gap at 35.7 basis
points rather than the headline 70, half the apparent mismatch already neutralised by the 30 %
fixed portion of the O&M contract. The lever nobody had pulled was on the cost side: the O&M
escalation index, negotiated separately by the technical team, ran at 3.2 % against the tariff's
2.5 % for no reason beyond the contractor having proposed it.

**How it resolved.** Two changes, negotiated together. The tariff moved to **90 % indexed at
full CPI**, and the O&M contract was re-tendered with its escalating 70 % **matched to the
tariff's index**. Present value of `EBITDA` over 25 years at 8 % rose from **93,938,399** to
**99,836,527**, a gain of **5,898,128**, of which **3,102,071** came from the extra ten points
of tariff indexation and **2,796,057** from index matching, the two effects being exactly
additive. The year-25 margin rose to **66.01 %**, above the year-1 62.50 %, because matched
indices against a partly fixed cost base produce margin *expansion*. The offtaker took a tighter
availability regime in exchange, which KA 7.1.3 priced separately.

**What the domain teaches here.** Indexation is an **architecture**, not a rate, and it must be
valued rather than preferred. The sponsors' instinct (more indexation is better) was wrong by
825,456 as offered, and the valuable change was on a line nobody had connected to the revenue
negotiation. Two disciplines follow: compute the present value of every indexation proposal
before responding to it, and negotiate revenue and cost indices **in the same room**, because
the exposure is the gap between them and neither team can see it alone.

## Case study B — Domain 7: the tenant they gave a discount to (digital infrastructure)

**Situation.** Northwind Data Campus (a fictitious SPV) was a 200,000,000 colocation development
fully pre-let to a single investment-grade anchor tenant on a ten-year lease: revenue
**34,000,000**, fixed cash operating costs **12,000,000**, maintenance capex **1,500,000**, so
`CFADS` **20,500,000**, with cash tax nil in the early years under capital allowances. The sponsors
regarded one strong tenant as their best credit feature. The lead arranger, pricing a ten-year
facility at **6.5 %** (`AF(0.065, 10) = 7.188830`), required **1.60 ×** coverage precisely because
100 % of revenue depended on one counterparty and one lease: 20,500,000 ÷ 1.60 = 12,812,500 of
service, supporting **92,106,887** and leaving equity of **107,893,113**, **53.95 %** of capital,
against a business plan built on 40 %.

**What happened.** The sponsors first argued the ratio, which failed for the reason Domain 10's
Case study A established. A coverage requirement is an output of the credit, not a negotiating
variable in isolation. Their second response worked. They asked the anchor, whose own growth
forecast had softened, to release **25 % of the capacity**, and re-let it to three independent
tenants at an average **6 % below** the anchor rate. Revenue fell to **33,490,000**, down
510,000 or **1.50 %**; operating costs rose 400,000 on the additional billing and service load;
`CFADS` fell to **19,590,000**, down **910,000**.

**How it resolved.** With four tenants and none above 75 % of revenue, the arranger reduced the
requirement to **1.35 ×**: debt service of 14,511,111 supporting **104,317,914** (**12,211,027**
more debt on **910,000 less `CFADS`**). Equity fell to **95,682,086**, from 53.95 % to **47.84
%** of capital. The arranger then imposed a condition the sponsors had not anticipated and that
was entirely correct: because the three new leases ran three years against the anchor's ten,
`WARCT` fell from **10.00 to 8.25 years** against a ten-year loan, so a **2,000,000 re-letting
reserve** was required, reducing the net capacity gain to **10,211,027**.

**What the domain teaches here.** **Concentration, not expected value, priced the debt.** The
sponsors gave up 910,000 a year of `CFADS` (**4.44 %**), and received twelve million of debt,
because what the arranger charged 0.25 × of coverage for was not the level of the cash flow but
the fact that all of it came from one signature (KA 7.4.2). The second lesson is the reserve:
reducing concentration by adding shorter leases traded counterparty risk for **re-contracting
risk**, and a lender who accepts the first while ignoring the second has not improved its
position. Every diversification must be assessed on both dimensions (how many payers, and for
how long), which is the `WARCT`-against-tenor test of KA 7.2.3.

---

## Executive perspective — Domain 7

What a project finance director cannot delegate in this domain:

- **The choice of revenue structure, priced.** The decision between an availability payment and a
  volume tariff is worth 10,679,727 of debt capacity on Kestrel's numbers (7.1.1). It is a
  capital-structure decision wearing commercial clothes, and it cannot be settled by the
  commercial team alone.
- **The deduction and performance regime.** Multipliers, caps, cure periods and outage
  allowances set the operational tolerance the whole project then lives inside: 2.91
  availability points for Kestrel, of which a point and a half is the negotiated multiplier
  (7.1.3).
- **The indexation architecture, on both sides.** Indexed shares and indices for revenue *and*
  cost, negotiated together, with effective rates computed. Case study A's 5,898,128 of present
  value came from two terms nobody had connected.
- **The case that debt is sized on.** The bank case is a negotiating position, and conceding it
  unopposed concedes more than the coverage ratio does (7.2.1).
- **The concentration statement.** Not the counterparty's rating and not the expected loss, but
  the probability of losing more revenue than the structure can survive, and what has been
  structured against it (7.4.2).
- **The tolerance in each team's own unit.** 4.14 % on tariff, 4.50 % on despatch, 2.91 points on
  availability, 5.83 % on `CFADS` — the same covenant, translated into four dashboards, with the
  smallest one owned (7.3.3).

## Calculation exercises — Domain 7

**Exercise 7.1** A capacity charge of 20,000,000 a year carries a 97 % availability guarantee
and a deduction equal to the charge multiplied by the availability shortfall and by a 1.25 ×
multiplier. Availability outturns at 94 %. Compute the deduction and the revenue received.
*Solution.* Shortfall `0.97 − 0.94 = 0.03`; deduction `20,000,000 × 1.25 × 0.03 =` **750,000**,
which is **3.75 %** of the charge; revenue **19,250,000**. Common error: omitting the multiplier
(600,000), or applying the multiplier to total unavailability of 6 % rather than to the 3 %
shortfall against the guarantee (1,500,000) (the first understates exposure by a fifth, the
second doubles it).

**Exercise 7.2** A project earns 0.80 per unit with variable cost 0.10 per unit and fixed cash
cost 4,000,000; there is no tax or working-capital movement. Volume is 18,000,000 units with
probability 0.60 and 10,500,000 with probability 0.40. Debt service is 6,000,000 a year and the
loan runs ten years at 7 % (`AF(0.07, 10) = 7.023582`). Compute `CFADS` and `DSCR` in each
outcome, the expected values, and debt capacity at a 1.25 × target sized (a) on expected `CFADS`
and (b) on the low case. *Solution.* High: revenue 14,400,000, `CFADS` `14,400,000 − 4,000,000 −
1,800,000 =` **8,600,000**, `DSCR` **1.4333**. Low: revenue 8,400,000, `CFADS` **3,350,000**,
`DSCR` **0.5583**. Expected volume 15,000,000; expected `CFADS` **6,500,000**; expected `DSCR`
**1.0833**. Capacity on expected `CFADS`: `6,500,000/1.25 × 7.023582 =` **36,522,624**; on the
low case: `3,350,000/1.25 × 7.023582 =` **18,823,199** (a difference of **17,699,425**). Common
error: sizing on the expected case and reporting the expected `DSCR` of 1.0833 as the project's
coverage; the low outcome does not pay debt service at all, and a 40 % probability of that is
not a rounding matter.

**Exercise 7.3** A concession's revenue is 30,000,000 with 90 % indexed at 2.2 %; its operating
cost is 18,000,000 with 60 % indexed at 3.5 %. Compute the `EBITDA` margin in year 1 and year
20, and the effective compound rates. *Solution.* Year 1: `EBITDA` **12,000,000**, margin
**40.00 %**. Year 20: revenue `30,000,000 × (0.90 × 1.022¹⁹ + 0.10) =` **43,825,432**; cost
`18,000,000 × 0.60 × 1.035¹⁹ + 7,200,000 =` **27,963,014**; `EBITDA` **15,862,417**, margin
**36.19 %** (a drift of **3.81 points**). Effective rates: revenue **2.015 %**, cost **2.346
%**. Common error: comparing the headline 2.2 % and 3.5 % and concluding a 130-basis-point
exposure; the effective gap is 33.1 basis points, because both sides are only partly indexed.

**Exercise 7.4** Revenue 50,000,000; variable costs are 30 % of revenue; fixed cash costs
18,000,000. Compute the degree of operating leverage, and the volume fall that takes a 1.40 ×
base coverage ratio to a 1.25 × covenant. *Solution.* Contribution `50,000,000 × 0.70 =`
**35,000,000**; `EBITDA` **17,000,000**; `DOL` `35,000,000/17,000,000 =` **2.0588**. Required
earnings fall `1 − 1.25/1.40 =` **10.7143 %**; volume fall `10.7143 % ÷ 2.0588 =` **5.2041 %**.
Verification: volume down 5.2041 % gives `EBITDA` of **15,178,571**, which is **89.29 %** of
base, and `1.40 × 0.8929 = 1.25`. Common error: reading the coverage gap as the volume tolerance
(a 10.71 % volume fall would take coverage to 1.09 ×, well below the covenant); operating
leverage means the tolerance is always smaller than the earnings tolerance.

**Exercise 7.5** A single offtaker has an annual `PD` of 1.0 %. Exposure over a ten-year loan is
80,000,000 and `LGD` is 50 %. Compute the expected loss, then compute the probability of losing
at least half the revenue if the same output were sold instead to **two** independent offtakers
of 50 % each on identical terms. Comment. *Solution.* Ten-year cumulative `PD` `1 − 0.99¹⁰ =`
**9.5618 %**; expected loss `0.095618 × 80,000,000 × 0.50 =` **3,824,717**. With two offtakers,
losing at least half the revenue requires **at least one** default: `1 − (1 − 0.095618)² =`
**18.2093 %**, against **9.5618 %** with a single offtaker. The probability has almost
**doubled**, while the probability of total loss falls from 9.5618 % to `0.095618² =` **0.9143
%**. Comment: splitting a revenue base into parcels at or above the survival threshold does not
reduce the risk of crossing it; diversification only helps when each parcel is **strictly
smaller** than the loss the structure can absorb. Common error: assuming that any
diversification reduces concentration risk, the four-way split of Worked example 7.4.2 reduces
the probability of a ≥ 50 % loss to 2.6489 %, but a two-way split increases it.

## Practitioner's toolkit — Domain 7

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable, and set a
retention period against each. These registers are the evidence that a decision was taken
properly, so each is retained at least as long as the obligation it supports, in a form that
opens without the tool that created it, with a named custodian who holds it once the engagement
ends. The applicable minimum periods are set by the organisation's own policy and by
jurisdiction-specific statutory, tax and limitation requirements, which this book does not
state. Where a register holds information about identified individuals, the retention period and
any minimisation or deletion obligation that cuts across it are settled with the organisation's
data-protection adviser before the register is adopted.*

### Toolkit 7.T.1 — Revenue mechanism specification

One row per component of the payment mechanism, populated from the agreement and signed off
before the model is built. Columns: component (capacity charge · variable charge · take-or-pay
floor · band · deduction · bonus · merchant) · **clause reference** · the formula in full, with
every rate, threshold, multiplier and cap · the units and the measurement source · the billing
period and payment terms · indexation reference (cross-refer to 7.T.2) · the model line that
implements it · the person who reconciled model to clause, and the date. Rule: **no revenue figure
is reportable until a historical or hypothetical period has been computed from the clause text by
hand and agreed with the model** (7.1.3). The deduction row carries its multiplier, its cap and
its cure regime explicitly, because a summary that omits them understates exposure by a third.

*Section B — the governance of the measurement, one block per measured quantity (KA 7.1.3).*
**Measurement:** the instrument or system of record · who owns and maintains it · the
calibration regime, its cycle and its tolerance · who may recalibrate and whether a
recalibration reopens prior periods · the **deemed-availability rule on metering failure**,
quoted from the clause. **Certification:** who produces the primary record · who certifies it ·
the challenge window and what happens to a period nobody challenges in time. **Evidence:** the
underlying data to be retained (meter records, control-system logs, outage records with causes,
maintenance records, outage notifications), the retention period against the longer of the
challenge window and the period in which a dispute may still be brought, the form (must open
without the system that created it) and the named custodian. **Dispute:** the escalation
sequence with its time limits · the determination forum · who bears the cost · **who may settle
on the SPV's behalf, and the threshold above which the board decides** · the standing lock that
a settlement changing a covenant ratio is not an operational decision and is checked against the
finance documents first. **Cash timing:** whether deductions are applied pending resolution, or
withheld or escrowed, stated as a yes/no on the face of the sheet, with the reserve consequence
noted. Rule: the periods, the conclusiveness of an unchallenged month and the remedy for a
deduction later found to be wrongly applied are established from the executed documents with
qualified counsel in the governing jurisdiction, and recorded here as answers rather than
assumptions.

### Toolkit 7.T.2 — Indexation mismatch map

Domain 3's escalation register (Toolkit 3.T.3) catalogues each line's index mechanics. This
artefact pairs them. One row per **cost** line: amount · indexed share · index and expected rate ·
**effective rate** · the revenue line that funds it · that revenue line's indexed share, index
and effective rate · **the gap in basis points** · the annual and present-value exposure the gap
represents over the concession, and separately over the loan life. Footer rows: total unindexed
revenue exposure, total unindexed cost protection, net effective gap, and margin at loan maturity
and at concession end. Rule: **negotiate on effective rates, never on headline indices**, and
review the map whenever either side of a pair is re-tendered — Case study A's second finding came
from an O&M re-tender nobody had connected to the tariff.

### Toolkit 7.T.3 — Revenue stress and counterparty pack

Section A, **counterparty**: each payer's share of revenue · rating or internal grade with source
and date · annual and horizon-cumulative `PD` · exposure defined as the present value of the
stream at risk, on a stated horizon · `LGD` with its recovery assumption traced to the security
and termination provisions · expected loss · enhancement in place, its cost, and **the coverage
step it purchased**. Section B, **concentration**: revenue share by payer · the loss the structure
can absorb · the probability of exceeding it · what has been structured against it. Section C,
**stress**: the driver list with elasticities and case labels · the two-way matrix on the drivers,
not on revenue · the correlated bundles with their rationale · the **minimum** coverage over the
loan life in each · the reverse stress result for every covenant, expressed as a tolerance in each
driver, with a note on **who controls that driver**. Front page: the smallest tolerance, its
driver, its owner, and the date it was last recomputed.

*Retention and data protection.* Section A holds information about identified third parties, and
frequently about identified individuals in a counterparty's ownership, control or management chain.
Record against the pack: its retention period and basis, its form, and its named custodian, on the
standing basis in the toolkit preamble; the confidentiality undertaking under which each item of
counterparty information is held, and any restriction on onward disclosure; and the lawful basis and
minimisation position for the personal information it contains, settled with the organisation's
data-protection adviser before the pack is built rather than at the first refresh. The applicable
periods and obligations are jurisdiction-specific and are confirmed, not assumed.

## Exam preparation — Domain 7

**What is assessed.** Placing a structure on the revenue-risk spectrum and stating its coverage
and capacity consequence; computing revenue under two-part, banded, floored and deduction-based
mechanisms; demonstrating and quantifying the gap between expected value and bankable value;
computing effective escalation rates and margin drift from indexed shares; computing `DOL` and
`CFADS` elasticities and converting a coverage threshold into a driver tolerance; computing
expected loss with a cumulative `PD` and explaining its blindness to concentration; and pricing
credit enhancement against coverage.

**The calculations to do under time pressure.** Revenue from a banded tariff at a given volume.
Deduction from a shortfall, a multiplier and a charge. `CFADS` and `DSCR` from the domain's bridge.
Expected value across a discrete distribution, and debt capacity from a low case
(`CFADS` ÷ target × `AF`). Effective compound rate from an indexed share, an index and a horizon.
`DOL` as contribution ÷ `EBITDA`. A driver tolerance as the `CFADS` tolerance divided by that
driver's elasticity. Cumulative `PD` as `1 − (1 − PD)ⁿ`, and expected loss as the product of three
factors.

**The traps.**
- Treating expected `CFADS` or expected `DSCR` as a sizing basis (Worked example 7.1.1; Exercise
  7.2) (the single most consequential error in the domain).
- Omitting the deduction multiplier, or applying it to total unavailability rather than to the
  shortfall against the guarantee (MCQ 7.1-B; Exercise 7.1).
- Recovering fixed costs and debt service through a purely volume-linked charge (7.1.2).
- Using the headline index instead of the effective rate on a partly indexed line (MCQ 7.3-A;
  Exercise 7.3), and its mirror, comparing headline indices across revenue and cost and
  overstating the gap twofold (Case study A).
- Assuming more indexation is better without computing the present value (Case study A).
- Treating "revenue −10 %" as one stress when price and volume have different elasticities
  (7.3.2, 7.4.3).
- Quoting a `CFADS` tolerance to a team that manages volume or availability (7.3.3).
- Using a single-year `PD` for a multi-year exposure (MCQ 7.4-A), or omitting `LGD`.
- Letting expected loss stand alone on a concentrated revenue base (7.4.2; MCQ 7.4-B).
- Assuming that any diversification reduces concentration risk, when parcels at or above the
  survival threshold make matters worse (Exercise 7.5).
- Reporting a year-one stress matrix as though it described the loan's worst year (7.4.3).
- Sizing a tenor on asset life rather than on the contracted horizon (7.A.2).

**How the domain connects.** Domain 5 (KA 5.3.2) named the five components of the revenue
bankability test and deferred the taxonomy here; Domain 6 built the model this domain feeds and
supplied the elasticity and breakeven machinery it extends; Domain 8 escalates the cost side with
the same indexation arithmetic; Domain 9 prices the equity that a weaker revenue structure makes
necessary; Domain 10 divides this domain's `CFADS` by the coverage this domain's structure earns;
Domain 11 places demand, price and counterparty risk in the allocation framework; Domain 12
drafts the mechanisms described here; and Domain 15 operates the covenant tests whose tolerances
KA 7.3.3 computed.

## Domain 7 summary
Revenue structure, not revenue level, decides how much debt a project can carry. Kestrel's two
offers (a 12,000,000 availability payment and a 0.50 per m³ volume tariff) have **identical
expected `CFADS` of 6,384,000** and identical expected `DSCR` of **1.2743**, and differ by
**10,679,727 of debt capacity**, 25.94 %, because a lender underwrites the low case (`CFADS`
4,728,000, `DSCR` **0.9438** — debt service unpaid) and raises the required ratio for dispersion
as well. An availability structure does not remove volatility, it converts demand risk into
operational risk: Kestrel breaches its 1.20 × covenant at **92.086 %** availability against a 95
% guarantee, **2.91 points** of headroom, of which about a point and a half is the negotiated
1.5 × deduction multiplier. Banded, floored and shared mechanisms are risk-transfer instruments
priced in tariff form — a volume band costing 2,515,153 of present value bought 3,482,520 of
capacity, and a value-neutral collar with an `MRG` at 90 % and full sharing above 110 % bought
**5,804,200 for nothing in expectation** — while service-revenue structures fail on
re-contracting rather than volume: at 93 % net revenue retention against a fixed cost base,
Halyard Connect's run-off case supports **20,271,839**, half the 40,000,000 sought, and its
`WARCT` of 3.30 years covers 47 % of the tenor. Escalation is an architecture: 80 % of Kestrel's
tariff at 2.5 % against 70 % of its cost at 3.2 % produces effective rates of **2.101 %** and
**2.457 %** (a 35.7-basis-point gap, half the headline), and **3.27 margin points** of drift by
year 25, all of it landing after the lender has gone; the unindexed tariff slice costs 3.65
points and the fixed cost slice saves 7.72, which is why Case study A's most valuable term was
in the O&M contract. Operating leverage explains why a 12 % patronage miss became Domain 10's
17.9 % cash miss (an elasticity of **1.4931**), and generalises: at identical 1.30 × coverage
and an identical 1.20 × covenant, demand tolerance ranges from **6.15 % to 1.54 %** on cost
structure alone. Kestrel's own tolerance is **4.14 %** on tariff, **4.50 %** on despatch
(1,079,530 m³) and **5.83 %** on `CFADS`, and quoting the last to a team that manages the second
overstates their room by 30 %. Counterparty quality bounds all of it: an annual `PD` of 0.60 %
over twelve years is **6.9671 %** cumulative, giving an expected loss of **1,678,031** on
exposure of **53,522,460** and a credit charge worth **0.0400** of `DSCR`; but expected loss is
identical whether one offtaker or four carry the revenue, while the probability of losing half
of it falls from **6.9671 % to 2.6489 %**, so concentration must be reported as a distribution
and never as an expectation. And credit enhancement is bought for the coverage it unlocks:
Kestrel's guarantee saves 300,403 of net expected loss and releases **2,625,026** of equity.
Domain 8 escalates the cost side, Domain 9 prices the equity a weak revenue structure demands,
and Domain 10 divides this domain's `CFADS` by the coverage this domain's structure has earned.
