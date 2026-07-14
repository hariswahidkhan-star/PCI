# Domain 7 — Contracts, Commercial Management, BoQ, Invoicing & Revenue

> **Group:** Project management (Domain 7 of 8 in the PM group). **Target:** ~135 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). British English; USD (+SAR where useful); five-line
> worked examples. Closes the loop to IFRS 15 (Domain 2).

## Why this domain exists

The contract is where risk is allocated, where money is defined, and where a project's commercial outcome is
won or lost. A controls professional who does not understand the **contract type** cannot forecast who bears
an overrun; one who cannot read a **bill of quantities** cannot value work; one who does not link **billing**
to **earned value** and to **IFRS 15 revenue** cannot reconcile the three numbers a project reports. This
domain covers contract types and their risk allocation (KA 7.1); contract management across the lifecycle —
variations, claims, liquidated damages, retention, bonds (KA 7.2); the **bill of quantities** and how work is
measured and priced (KA 7.3); **invoicing and applications for payment** — interim valuations, certification,
retention (KA 7.4); and **revenue recognition in the commercial cycle**, tying billing back to IFRS 15 and
the over-/under-billing position (KA 7.5). It is the commercial counterpart of the finance group and the
bridge between the schedule/cost machinery and the money.

**Learning objectives.** After this domain a candidate can: select a contract type and explain its risk
allocation, including target-cost pain/gain mechanics; describe the contract lifecycle and the commercial
instruments (variations, claims, LDs, retention, bonds); price and remeasure a bill of quantities; prepare an
interim application for payment with retention; and reconcile billing to earned value and to IFRS 15 revenue
via the contract asset/liability.

---

## Knowledge Area 7.1 — Types of contract

*Topics: 7.1.1 the risk-allocation spectrum · 7.1.2 lump sum and remeasurement · 7.1.3 cost-plus and
incentive forms · 7.1.4 target cost and pain/gain · 7.1.5 T&M, unit-rate, framework and EPC.*

### 7.1.1 The risk-allocation spectrum

**The principle.** Contract types sit on a spectrum of **cost risk** between the parties. At one end, **lump
sum / fixed price** puts the cost risk on the **contractor** (a fixed price whatever it costs to deliver the
defined scope). At the other, **cost-plus** puts it on the **client** (the client reimburses actual cost plus
a fee). In between sit **target-cost** forms that **share** the risk. Choosing a type is choosing *who is
incentivised to control cost* and *who pays if it moves* — which is why it is the first commercial fact a
controls professional establishes.

> **Fig 7.1.1 — The contract risk-allocation spectrum.** *Caption:* who bears cost risk, by contract type.
> *Underlying data:* lump sum (contractor) → target cost (shared) → cost-plus (client). *Render-ready
> description:* a horizontal bar from "Contractor bears cost risk" (left) to "Client bears cost risk"
> (right), with markers placed along it: Lump sum / Fixed price (far left), Remeasurement (left-centre),
> Target cost with pain/gain (centre), Cost-plus incentive fee (right-centre), Cost-plus fixed fee & T&M (far
> right). Brand-blue gradient. *Animation storyboard (digital-only):* a slider moves along the spectrum; as
> it moves, a small pie shows the cost-risk split shifting from contractor to client.

### 7.1.2 Lump sum and remeasurement

- **Lump sum / fixed price** — a fixed price for a defined scope. The contractor bears the risk of cost and
  quantity overrun; the client bears the risk of paying for defined scope even if actual quantities are lower.
  Requires **well-defined scope** to price; poorly defined scope drives variations and claims (7.2).
- **Remeasurement** — priced at agreed **rates** against **estimated** quantities, then **re-measured** at
  actual quantities on completion. Suits work where quantities cannot be fixed up front (earthworks, ground
  works). The client bears quantity risk (pays for actual quantities); the contractor bears rate risk.

### 7.1.3 Cost-plus and incentive forms

- **Cost-plus fixed fee (CPFF)** — reimburse actual cost plus a **fixed** fee. The client bears almost all
  cost risk; the contractor has little incentive to minimise cost (the fee is fixed regardless).
- **Cost-plus incentive fee (CPIF)** — reimburse cost plus a fee that **varies with cost performance** against
  a target, via a **share ratio** — restoring a cost-control incentive.
- **Cost-plus award fee (CPAF)** — cost plus a fee awarded subjectively against performance criteria.

**Worked example 7.1.3 — CPIF fee adjustment.**

1. **Setup.** Target cost **USD 10,000,000**; target fee **USD 800,000**; share ratio **50/50** (client/
   contractor share of any under/overrun); actual cost **USD 9,400,000**.
2. **Formula.** `Fee = target fee + contractor's share × (target cost − actual cost)`.
3. **Substitution.** Underrun `= 10,000,000 − 9,400,000 = 600,000`; contractor share `= 50 % × 600,000 =
   300,000`; `fee = 800,000 + 300,000 = 1,100,000`.
4. **Result.** Contractor earns a fee of **USD 1,100,000**; client pays `actual cost 9,400,000 + fee
   1,100,000 = 10,500,000` — against a target price of 10,800,000, a **USD 300,000 saving** to the client.
5. **Interpretation.** The 600,000 underrun is shared 50/50 (300k to each party), so the contractor is
   *rewarded* for beating the target — the point of the incentive. An overrun would cut the fee symmetrically.

### 7.1.4 Target cost and pain/gain

**Definition & purpose.** A **target-cost** contract sets a **target cost** and a mechanism to **share** the
difference between target and actual — **gain-share** when under, **pain-share** when over — usually within a
cap/collar. It aligns both parties on cost control while sharing risk, and is common in collaborative/alliance
delivery.

**Worked example 7.1.4 — pain/gain both ways.** Target cost **USD 10,000,000**, share **50/50**.

- **Underrun:** actual **9,400,000** → saving **600,000** → each party keeps **300,000** (gain-share).
- **Overrun:** actual **10,800,000** → overrun **800,000** → contractor bears **50 % = 400,000** (pain-share),
  the client the other 400,000. The contractor's pain-share erodes its fee/margin, sharpening the incentive to
  control cost.

**Interpretation.** Target-cost forms make the controls professional's forecast (Domain 6, `EAC`) *directly
financial*: an `EAC` above target signals a pain-share the contractor will bear, quantifiable now — one of the
clearest places where an earned-value forecast becomes a commercial number a board acts on.

### 7.1.5 T&M, unit-rate, framework and EPC

- **Time & materials (T&M)** — pay for time (labour rates) and materials used; client bears cost risk; suits
  undefined or small works; often **capped** to limit exposure.
- **Unit-rate** — pay agreed rates per unit of work done (a cousin of remeasurement).
- **Framework** — a standing arrangement with agreed terms/rates under which call-off orders are placed; suits
  repeat/programme work.
- **EPC (engineer-procure-construct) / turnkey** — a single contractor delivers the whole asset, often lump
  sum, transferring integration risk to the contractor.

### Key terms — KA 7.1

| Term | Meaning |
|---|---|
| **Lump sum / fixed price** | Fixed price for defined scope; contractor bears cost risk. |
| **Remeasurement / unit-rate** | Priced at rates against re-measured actual quantities. |
| **Cost-plus (CPFF/CPIF/CPAF)** | Reimburse cost plus a fixed/incentive/award fee; client bears cost risk. |
| **Target cost / pain-gain** | Shared cost risk against a target, within a cap/collar. |
| **EPC / turnkey** | Single-contractor delivery of the whole asset. |

### Sample MCQs — KA 7.1

**MCQ 7.1-A `[7.1.1 · Analysis]`** Under a **lump-sum** contract with well-defined scope, an unexpected cost
overrun on that scope is primarily borne by:
- A. The client.
- B. The contractor. ✅
- C. Shared 50/50.
- D. The insurer.

*Rationale:* Lump sum fixes the price for defined scope, so the contractor bears the cost-overrun risk on that
scope. Cost-plus would put it on the client; target cost would share it.

**MCQ 7.1-B `[7.1.3 · Application]`** CPIF: target cost USD 10,000,000, target fee USD 800,000, 50/50 share,
actual cost USD 9,400,000. The contractor's fee is:
- A. USD 800,000
- B. USD 1,100,000 ✅
- C. USD 1,400,000
- D. USD 500,000

*Rationale:* Underrun 600,000 × 50 % = 300,000 added to the 800,000 target fee = **1,100,000**. A ignores the
incentive; C takes the whole underrun; D miscomputes.

**MCQ 7.1-C `[7.1.4 · Application]`** Target cost USD 10,000,000, 50/50 pain-share; actual cost USD 10,800,000.
The contractor's pain-share is:
- A. USD 800,000
- B. USD 400,000 ✅
- C. USD 0
- D. USD 10,800,000

*Rationale:* Overrun 800,000 × 50 % = **400,000** borne by the contractor. A is the whole overrun; C ignores
the mechanism; D is the actual cost.

### Self-check — KA 7.1

1. Order lump sum, target cost and cost-plus by who bears cost risk. *(Lump sum → contractor; target cost →
   shared; cost-plus → client.)*
2. Why does a target-cost contract make an `EAC` a commercial number? *(An `EAC` above target quantifies the
   contractor's pain-share now.)*

---

## Knowledge Area 7.2 — Contract management

*Topics: 7.2.1 the contract lifecycle · 7.2.2 variations and claims · 7.2.3 liquidated damages · 7.2.4
retention, bonds and guarantees · 7.2.5 standard forms (FIDIC awareness).*

### 7.2.1 The contract lifecycle

**The principle.** A contract is managed from **formation** (negotiation, award, mobilisation) through
**administration** (obligations, milestones, payments, variations, records) to **closure** (final account,
release of retention, warranties). The controls professional's role runs throughout: maintaining the records
and notices that protect entitlement, valuing work, tracking variations, and assembling the final account.
Good contract administration is largely about **contemporaneous records** — the notices, instructions and
progress evidence that decide who is entitled to what when a dispute arises.

### 7.2.2 Variations and claims

**Definitions.** A **variation (change order)** is an instructed change to scope/price, valued under the
contract's rules (the commercial side of change control, Domain 5, KA 5.4; the revenue side, Domain 2, KA
2.2.8). A **claim** is an assertion of entitlement to additional time and/or money arising from an event
(delay, disruption, changed conditions) — it must be **notified**, **substantiated** (cause, effect,
quantum) and **assessed**. The discipline is the same: identify early, record contemporaneously, quantify the
full impact (direct + disruption + prolongation), and progress to agreement.

### 7.2.3 Liquidated damages

**Definition & purpose.** **Liquidated damages (LDs)** are a pre-agreed sum payable by the contractor for a
defined breach — usually **late completion** — set as a **genuine pre-estimate** of the client's loss, not a
penalty. They give certainty (both parties know the cost of delay in advance) and cap the contractor's delay
exposure at the agreed rate. A controls professional forecasting a late finish (Domain 6/10) can quantify the
LD exposure directly: `LD exposure = LD rate × forecast days late`.

**Short example.** LDs of **USD 10,000/day**; the forecast shows **20 days** late → exposure **USD 200,000** —
a number the `EAC` and the commercial forecast should carry.

### 7.2.4 Retention, bonds and guarantees

- **Retention** — a percentage (commonly ~5 %) **withheld** from each payment as security for completion and
  defects, released in stages (e.g. half at completion, half after the defects period). Retention deepens the
  cash-flow trough (Domain 3, KA 3.5) and is a real receivable to track and recover.
- **Bonds / guarantees** — third-party security: a **performance bond** (against contractor default), an
  **advance-payment bond** (securing a client's advance), a **retention bond** (substituting for cash
  retention). They cost money (a percentage fee) and must be tracked and released.

### 7.2.5 Standard forms (FIDIC awareness)

**The principle.** Standard contract forms — such as the **FIDIC** suite internationally, and others by
jurisdiction — provide tested allocations of risk, roles (e.g. the Engineer), and procedures for variations,
claims, payment and dispute resolution. A controls professional should understand **at a concept level** how a
standard form allocates risk and structures the payment/variation/claims machinery, without reproducing its
wording. (Named at awareness level only, consistent with the citation rules — Spine §9.)

### Key terms — KA 7.2

| Term | Meaning |
|---|---|
| **Variation / change order** | An instructed, valued change to scope/price. |
| **Claim** | A notified, substantiated assertion of entitlement to time/money. |
| **Liquidated damages (LDs)** | A pre-agreed sum for a defined breach (usually late completion). |
| **Retention** | A percentage withheld as security, released in stages. |
| **Performance / advance-payment / retention bond** | Third-party security instruments. |

### Sample MCQs — KA 7.2

**MCQ 7.2-A `[7.2.3 · Application]`** LDs are USD 10,000/day; the forecast completion is 20 days late. The LD
exposure is:
- A. USD 10,000
- B. USD 200,000 ✅
- C. USD 20,000
- D. USD 2,000,000

*Rationale:* `10,000 × 20 = 200,000`. A is one day; C swaps the figures; D misplaces a zero.

**MCQ 7.2-B `[7.2.3 · Recall]`** Liquidated damages are enforceable when they represent:
- A. A punitive penalty to deter breach.
- B. A genuine pre-estimate of the client's likely loss. ✅
- C. The contractor's total revenue.
- D. The retention amount.

*Rationale:* LDs must be a genuine pre-estimate of loss, not a penalty. A describes an unenforceable penalty;
C and D are unrelated figures.

**MCQ 7.2-C `[7.2.4 · Analysis]`** Increasing retention from 5 % to 10 % on a project will, all else equal:
- A. Improve the contractor's cash position.
- B. Deepen the contractor's funding trough (more cash withheld for longer). ✅
- C. Have no cash effect.
- D. Reduce liquidated damages.

*Rationale:* Higher retention withholds more cash for longer, deepening the funding trough (Domain 3, KA 3.5).
It worsens (not improves) cash, has a clear cash effect, and is unrelated to LDs.

### Self-check — KA 7.2

1. What three things must a claim establish? *(Cause, effect and quantum — notified and substantiated.)*
2. Why does retention matter to cash-flow forecasting? *(It withholds cash until completion/defects release,
   deepening and lengthening the funding trough.)*

---

## Knowledge Area 7.3 — Bills of Quantities (BoQ)

*Topics: 7.3.1 what a BoQ is · 7.3.2 structure and measurement · 7.3.3 rates and preliminaries · 7.3.4 pricing
and remeasuring a BoQ.*

### 7.3.1 What a BoQ is

**Definition & purpose.** A **bill of quantities (BoQ)** is an itemised list of the measured **quantities** of
work in a project, against which **rates** are applied to build a price. It is prepared from drawings and
specifications under a standard method of measurement, and it serves three roles: a common basis for
**tendering** (all tenderers price the same quantities), a mechanism for **valuing** work done (7.4), and a
basis for **remeasurement** and variation pricing (7.1.2, 7.2.2).

### 7.3.2 Structure and measurement

**The principle.** A BoQ is structured into sections (by element or trade), each containing **items** with a
description, unit of measurement (m³, m², t, nr, m), and quantity. **Measurement** — deriving the quantities
from the design under a consistent method — is a skilled task; errors in measurement flow straight into price
and into every subsequent valuation.

### 7.3.3 Rates and preliminaries

- **Rates** — the price per unit for each item, covering labour, materials, plant, overhead and profit for
  that work. Rates may be built up from first principles or drawn from a cost database.
- **Preliminaries** — project-wide costs **not** attributable to a single measured item (site establishment,
  supervision, temporary works, welfare). Often a substantial lump or time-related sum; a controls
  professional watches preliminaries closely because they are **time-related** — a delay extends them
  (prolongation cost, linking to claims, 7.2.2).

### 7.3.4 Pricing and remeasuring a BoQ — worked

**Worked example 7.3.4 — price a small BoQ.**

1. **Setup.** A substructure BoQ:

   | Item | Description | Qty | Unit | Rate (USD) | Amount (USD) |
   |---|---|---:|---|---:|---:|
   | A | Excavation | 5,000 | m³ | 12 | 60,000 |
   | B | Concrete | 800 | m³ | 150 | 120,000 |
   | C | Reinforcement | 60 | t | 1,200 | 72,000 |
   | P | Preliminaries | 1 | sum | 48,000 | 48,000 |
   | | **Total** | | | | **300,000** |

2. **Formula.** `Amount = quantity × rate`; `total = Σ amounts`.
3. **Result.** `60,000 + 120,000 + 72,000 + 48,000 = 300,000`. ✓
4. **Remeasurement.** If actual excavation is **5,400 m³** (not 5,000), the remeasured amount is `5,400 × 12 =
   64,800` — an **extra USD 4,800** the client pays under a remeasurement contract (quantity risk sits with the
   client). Under lump sum, the same extra volume — if within the defined scope — is the contractor's risk.
5. **Interpretation.** The BoQ is the machine that turns *quantities and rates* into *money* at tender, at
   valuation, and at variation — and whether a quantity change is the client's or the contractor's cost is set
   by the contract type (7.1).

### Key terms — KA 7.3

| Term | Meaning |
|---|---|
| **Bill of quantities (BoQ)** | Itemised measured quantities priced at rates. |
| **Measurement** | Deriving quantities from design under a standard method. |
| **Rate** | Price per unit (labour, materials, plant, overhead, profit). |
| **Preliminaries** | Project-wide, often time-related costs not tied to a measured item. |
| **Remeasurement** | Re-pricing at actual quantities. |

### Sample MCQs — KA 7.3

**MCQ 7.3-A `[7.3.4 · Application]`** Excavation is 5,000 m³ at USD 12/m³. If actual quantity is 5,400 m³, the
remeasured amount is:
- A. USD 60,000
- B. USD 64,800 ✅
- C. USD 4,800
- D. USD 66,000

*Rationale:* `5,400 × 12 = 64,800`. A is the original; C is only the extra; D miscomputes.

**MCQ 7.3-B `[7.3.3 · Analysis]`** Preliminaries are significant on a project that is now forecast to finish
late. The main commercial consequence is:
- A. Preliminaries fall automatically.
- B. Time-related preliminaries extend, creating prolongation cost. ✅
- C. The BoQ rates change.
- D. Retention is released early.

*Rationale:* Preliminaries are largely time-related, so a delay extends them (prolongation), a common claim
head. Delay does not reduce them, change measured rates, or accelerate retention release.

### Self-check — KA 7.3

1. What three roles does a BoQ serve? *(Common tender basis; valuing work done; remeasurement/variation
   pricing.)*
2. Why do controls professionals watch preliminaries in a delay? *(They are time-related — a delay extends
   them, creating prolongation cost.)*

---

## Knowledge Area 7.4 — Invoicing and applications for payment

*Topics: 7.4.1 interim valuations and progress billing · 7.4.2 certification and payment · 7.4.3 retention in
the valuation · 7.4.4 linking BoQ/EVM progress to billing.*

### 7.4.1 Interim valuations and progress billing

**Definition & purpose.** On a long contract the contractor is paid **progressively** via **interim
valuations** (applications for payment): at each period, the **value of work done to date** is assessed
(against the BoQ or milestones), **retention** is deducted, and **previous payments** are subtracted to give
the **amount due** this period. This is the cash-inflow side of the funding curve (Domain 3, KA 3.5).

### 7.4.2 Certification and payment

**The principle.** The contractor **applies**; the client's representative (engineer/quantity surveyor)
**certifies** the amount agreed; the client **pays** the certified sum within the contractual period.
Differences between applied and certified amounts are a normal commercial tension — and a source of dispute if
persistent. The certified amount, not the applied amount, drives cash.

### 7.4.3 Retention in the valuation — worked

**Worked example 7.4.3 — an interim application for payment.**

1. **Setup.** Using the BoQ of 7.3.4, at a valuation date the work done is: Excavation **100 %**, Concrete
   **50 %**, Reinforcement **25 %**, Preliminaries **40 %**. Retention is **5 %**; **previous payments** total
   **USD 90,000**.
2. **Formula.** `Gross value = Σ (% complete × item amount)`; `retention = 5 % × gross value`; `net certified =
   gross − retention`; `amount due = net certified − previous payments`.
3. **Substitution.**
   - Excavation `100 % × 60,000 = 60,000`; Concrete `50 % × 120,000 = 60,000`; Reinforcement `25 % × 72,000 =
     18,000`; Preliminaries `40 % × 48,000 = 19,200`.
   - Gross value `= 60,000 + 60,000 + 18,000 + 19,200 = 157,200`.
   - Retention `= 5 % × 157,200 = 7,860`; net certified `= 157,200 − 7,860 = 149,340`.
   - Amount due `= 149,340 − 90,000 = 59,340`.
4. **Result.** **Amount due this application: USD 59,340** (gross value 157,200, less retention 7,860, less
   previous payments 90,000).
5. **Interpretation.** The retention of USD 7,860 is withheld now and recovered later (7.2.4) — cash the
   contractor has earned but cannot yet collect, part of the funding trough. The valuation is a direct
   application of the BoQ (7.3) to progress measured in the field.

### 7.4.4 Linking BoQ/EVM progress to billing

**The professional angle.** Three progress measures must be **reconciled**, and a controls professional owns
that reconciliation:

- **Earned value (`EV`)** — progress valued at *budget/cost* (Domain 6).
- **BoQ valuation** — progress valued at *contract rates* (this KA) → drives billing.
- **IFRS 15 revenue** — progress recognised under the accounting standard (Domain 2, KA 2.2) → drives the
  accounts.

They measure the *same physical progress* but at different *values* and under different *rules*, so they
differ — and the difference is meaningful, not error. Reconciling `EV`, valuation and revenue is how a project
knows simultaneously how it is performing (cost), what it can bill (cash), and what it can recognise (profit).

### Key terms — KA 7.4

| Term | Meaning |
|---|---|
| **Interim valuation / application** | Periodic assessment of value done, less retention and prior payments. |
| **Certification** | The client's agreement of the amount payable. |
| **Amount due** | Net certified value less previous payments. |
| **Progress reconciliation** | Tying `EV` (cost), BoQ valuation (billing) and IFRS 15 revenue. |

### Sample MCQs — KA 7.4

**MCQ 7.4-A `[7.4.3 · Application]`** Gross value of work done is USD 157,200; retention 5 %; previous payments
USD 90,000. The amount due this application is:
- A. USD 67,200
- B. USD 59,340 ✅
- C. USD 149,340
- D. USD 157,200

*Rationale:* Retention `5 % × 157,200 = 7,860`; net certified `157,200 − 7,860 = 149,340`; due `149,340 −
90,000 = 59,340`. A ignores retention; C omits previous payments; D is the gross value.

**MCQ 7.4-B `[7.4.4 · Analysis]`** Earned value, the BoQ valuation and IFRS 15 revenue for the same period
differ. This is:
- A. An error to be corrected.
- B. Expected — they value the same progress at different values under different rules; the differences are reconciled and meaningful. ✅
- C. A breach of IFRS 15.
- D. Impossible.

*Rationale:* `EV` (budget), valuation (contract rates) and revenue (IFRS 15) measure the same progress
differently, so they diverge by design; the professional reconciles them. It is neither an error nor a breach.

### Self-check — KA 7.4

1. Give the steps from gross value to amount due. *(Gross value → less retention → net certified → less
   previous payments → amount due.)*
2. Name the three progress measures a controls professional reconciles and what each values. *(`EV` — budget;
   BoQ valuation — contract rates/billing; IFRS 15 — recognised revenue.)*

---

## Knowledge Area 7.5 — Revenue recognition in the commercial cycle

*Topics: 7.5.1 tying billing to IFRS 15 · 7.5.2 over- and under-billing · 7.5.3 the commercial-to-accounting
loop.*

### 7.5.1 Tying billing to IFRS 15

**The principle.** Billing (this domain) and revenue recognition (Domain 2, KA 2.2) are **different measures**:
billing follows the **contract's payment mechanism** (interim valuations, milestones), while revenue follows
**IFRS 15 performance** (control transfer, usually over time by an input/output method). They rarely equalise
in a period, and the gap between them is the **contract asset or liability** (Domain 2, KA 2.2.7). A controls
professional who understands both can explain to finance *why* certified billing and recognised revenue differ
this month — and what that says about the project.

### 7.5.2 Over- and under-billing

**Worked example 7.5.2 — over- vs under-billing.**

1. **Setup.** For a period, IFRS 15 **revenue recognised** (by cost-to-cost, Domain 2) is **USD 6,750,000**;
   cumulative **certified billing** (this domain) is **USD 7,000,000**.
2. **Reasoning.** Billing exceeds revenue → the client has been billed ahead of performance.
3. **Result.** A **contract liability (over-billing) of USD 250,000** (Domain 2, KA 2.2.7). Had revenue
   exceeded billing, it would be a **contract asset (under-billing)**.
4. **Interpretation.** Over-billing improves cash now but is a liability to *earn out*; a large or growing
   contract **asset** (under-billing) is a warning that the project is performing ahead of what it is
   collecting — cash tied up in work done but not billed. The controls professional reads the direction and
   trend of this position as a commercial-and-cash health signal.

### 7.5.3 The commercial-to-accounting loop

**The synthesis.** This KA closes the loop that runs through the whole book: **scope** (WBS/BoQ) → **cost**
(Domains 1, 5) → **earned value** (Domain 6) → **billing** (this domain) → **revenue** (Domain 2) → **the
statements** (Domains 1–2) → **management reporting** (Domain 4). Each is a different value of the same
physical progress; a controls professional who can trace a slipped activity all the way through this loop —
to its cost, its billing, its revenue, its contract asset/liability, and the board narrative — is exercising
exactly the integrated judgement the credential certifies.

**AI in this KA.** AI supports the commercial cycle: extracting terms and variation clauses from contracts,
analysing claims/variations, checking valuations and flagging billing anomalies, and reconciling billing to
`EV` and IFRS 15 revenue. The recognition and entitlement judgements — whether a variation is valid, whether a
claim is substantiated, whether revenue may be recognised — remain the professional's, auditable and signed
off. **AI proposes, the professional disposes.**

### Key terms — KA 7.5

| Term | Meaning |
|---|---|
| **Billing vs revenue** | Contract payment mechanism vs IFRS 15 performance-based recognition. |
| **Contract asset (under-billing)** | Revenue recognised exceeds billing. |
| **Contract liability (over-billing)** | Billing exceeds revenue recognised. |
| **Commercial-to-accounting loop** | Scope → cost → EV → billing → revenue → statements → reporting. |

### Sample MCQs — KA 7.5

**MCQ 7.5-A `[7.5.2 · Analysis]`** IFRS 15 revenue recognised is USD 6,750,000; certified billing is USD
7,000,000. The position is:
- A. A contract asset of USD 250,000.
- B. A contract liability of USD 250,000. ✅
- C. A revenue error.
- D. Nil.

*Rationale:* Billing exceeds revenue by 250,000 → **over-billing → contract liability**. A reverses it; it is
not an error, and the difference is not nil.

**MCQ 7.5-B `[7.5.1 · Recall]`** Billing and IFRS 15 revenue differ because billing follows ____ while revenue
follows ____:
- A. IFRS 15; the payment mechanism
- B. the contract's payment mechanism; IFRS 15 performance ✅
- C. cash; cash
- D. the schedule; the schedule

*Rationale:* Billing follows the contract's payment mechanism (valuations/milestones); revenue follows IFRS 15
performance (control transfer). The other options invert or conflate the two.

### Self-check — KA 7.5

1. What does a large, growing *contract asset* signal? *(Under-billing — performing ahead of collections; cash
   tied up in unbilled work.)*
2. Trace the commercial-to-accounting loop in order. *(Scope → cost → EV → billing → revenue → statements →
   reporting.)*

---

## Domain 7 summary

The contract allocates cost risk along a spectrum — lump sum (contractor), target cost (shared, with pain/gain
mechanics that make an `EAC` a live commercial number), cost-plus (client) — and is administered across its
lifecycle through variations, claims (cause/effect/quantum), liquidated damages, retention and bonds. The
**bill of quantities** turns measured quantities and rates into money at tender, valuation and variation, with
time-related preliminaries a key delay exposure. **Interim valuations** convert measured progress into an
application for payment — value done, less retention, less previous payments — and the certified amount drives
cash. Finally, billing ties back to **IFRS 15 revenue**: the two measure the same progress under different
rules, and their difference is the contract asset or liability. This domain closes the commercial-to-accounting
loop — scope → cost → earned value → billing → revenue → statements → reporting — the integrated chain the
credential is built on.

**Cross-references.** IFRS 15 and contract asset/liability → 2.2; contract modifications in revenue → 2.2.8;
principal/agent (gross/net) → 2.3; cash-flow and the funding trough → 3.5; the EAC that drives pain-share →
Domain 6; change control → 5.4; delay forecasting and LDs → Domains 6 and 10; process cycles (O2C billing) →
Domain 11; contract-analytics AI → 13.4–13.5.

*Domain 7 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
