# Domain 7 — Contracts, Commercial Management, BoQ, Invoicing & Revenue

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

**Worked example 7.1.4b — a target-cost pain-share with a cap.**

1. **Setup.** Target cost **USD 10,000,000**; target fee **USD 800,000**; share ratio **50/50**; but the
   contractor's pain-share is **capped at USD 600,000** (a fee floor of USD 200,000). Actual cost turns out to
   be **USD 12,000,000**.
2. **Formula.** `Uncapped pain-share = share ratio × overrun`, then apply the cap.
3. **Substitution.** Overrun `= 12,000,000 − 10,000,000 = 2,000,000`; uncapped pain-share `= 50 % × 2,000,000 =
   1,000,000`; **capped at 600,000**.
4. **Result.** The contractor bears **USD 600,000** (the fee falls from 800,000 to the 200,000 floor); the
   client bears the remaining **USD 1,400,000** of the overrun. Without the cap the contractor would have borne
   USD 1,000,000.
5. **Interpretation.** Caps/collars bound each party's exposure — protecting the contractor from catastrophic
   overruns but also weakening the incentive beyond the cap. A controls professional forecasting an `EAC` above
   target (Domain 6) must know where the cap bites to quantify the real commercial exposure.

### 7.1.5 T&M, unit-rate, framework and EPC

- **Time & materials (T&M)** — pay for time (labour rates) and materials used; client bears cost risk; suits
  undefined or small works; often **capped** to limit exposure.
- **Unit-rate** — pay agreed rates per unit of work done (a cousin of remeasurement).
- **Framework** — a standing arrangement with agreed terms/rates under which call-off orders are placed; suits
  repeat/programme work.
- **EPC (engineer-procure-construct) / turnkey** — a single contractor delivers the whole asset, often lump
  sum, transferring integration risk to the contractor.

**Worked example 7.1.5 — a capped T&M engagement.**

1. **Setup.** Specialist commissioning support engaged on T&M at **USD 120/hour**, estimated effort
   **2,000 hours** (USD 240,000); the contract caps total T&M charges at **USD 260,000**. Actual effort runs
   to **2,300 hours**.
2. **Formula.** `Charge = min(hours × rate, cap)`.
3. **Substitution.** Uncapped charge `= 2,300 × 120 = 276,000`; this exceeds the cap, so the charge is
   **260,000**.
4. **Result.** The client pays the **cap (USD 260,000)**; the provider bears the **USD 16,000** above it
   (`276,000 − 260,000`). Had actual effort been 2,100 hours (`2,100 × 120 = 252,000`), the client would have
   paid 252,000 — the cap only bites above it.
5. **Interpretation.** Capped T&M shares risk asymmetrically: the client bears cost risk *up to* the cap, the
   provider beyond it. The cap converts open-ended T&M exposure into a budgetable ceiling — which is why
   capped T&M suits undefined scope, and agile delivery in particular (see 9.6.3).

**Man-day / man-hour (MDMH) rate contracts** are the labour-schedule variant of T&M, dominant in manpower
supply, consultancy secondment, commissioning support and EPC site-services packages — and a fixture of
Gulf-region contracting. The schedule prices each **grade** (senior engineer, technician, …) per approved
man-day or man-hour, with the contract defining the working day (8 or 10 hours), overtime multipliers, and
mobilisation/demobilisation terms. The commercial essence: the client buys **input, not output** — a man-day
is earned by attendance, not progress — so **productivity risk sits wholly with the client**. That drives the
controls that matter: **timesheet verification** against approved attendance; **grade audit** (is the person
billed at the grade actually mobilised?); rate build-up transparency (base salary, burdens, overheads,
margin); and **productivity tracking** — man-hours consumed mapped to deliverables or earned progress
(Domain 6), because nothing in the payment mechanism does that mapping for you. Distinguish from unit-rate
(7.1.2/7.3): unit-rate pays per unit of *output*; MDMH pays per unit of *input*. As scope firms up, the
professional move is conversion — to capped T&M, deliverable-based milestones or lump sum (7.1.1's spectrum
walked deliberately, not drifted).

**Worked example 7.1.5b — a month on man-day rates, verified.**

1. **Setup.** A commissioning-support contract prices a senior engineer at **USD 720 per man-day** and a
   technician at **USD 360 per man-day** (10-hour site day defined in the schedule). In the month, **2 senior
   engineers** and **5 technicians** each work **22 approved days**.
2. **Formula.** `invoice = Σ (heads × approved days × grade rate)`.
3. **Substitution.** Senior: `2 × 22 = 44` man-days; `44 × 720 = 31,680`. Technician: `5 × 22 = 110` man-days;
   `110 × 360 = 39,600`.
4. **Result.** Month invoice `= 31,680 + 39,600 =` **USD 71,280** — payable on verified timesheets and grade
   evidence.
5. **Interpretation.** The invoice check is attendance arithmetic, but the *control* question is different:
   what did 154 man-days buy? If commissioning progress (Domain 6's EV, or a deliverable log) does not move in
   step with man-days consumed, the client is funding presence, not progress — the MDMH form only works when
   someone owns that comparison every month.

### Key terms — KA 7.1

| Term | Meaning |
|---|---|
| **Lump sum / fixed price** | Fixed price for defined scope; contractor bears cost risk. |
| **Remeasurement / unit-rate** | Priced at rates against re-measured actual quantities. |
| **Cost-plus (CPFF/CPIF/CPAF)** | Reimburse cost plus a fixed/incentive/award fee; client bears cost risk. |
| **Target cost / pain-gain** | Shared cost risk against a target, within a cap/collar. |
| **EPC / turnkey** | Single-contractor delivery of the whole asset. |
| **Man-day / man-hour (MDMH) rates** | Labour billed per approved day/hour at graded rates — input bought; productivity risk stays with the client. |

### Sample MCQs — KA 7.1

**MCQ 7.1-A `[7.1.1 · Analysis]`** Under a **lump-sum** contract with well-defined scope, an unexpected cost
overrun on that scope is primarily borne by:
- A. The client.
- B. The insurer.
- C. Shared 50/50.
- D. The contractor. ✅

*Rationale:* Lump sum fixes the price for defined scope, so the contractor bears the cost-overrun risk on that
scope. Cost-plus would put it on the client; target cost would share it.

**MCQ 7.1-B `[7.1.3 · Application]`** CPIF: target cost USD 10,000,000, target fee USD 800,000, 50/50 share,
actual cost USD 9,400,000. The contractor's fee is:
- A. USD 800,000
- B. USD 1,400,000
- C. USD 1,100,000 ✅
- D. USD 500,000

*Rationale:* Underrun 600,000 × 50 % = 300,000 added to the 800,000 target fee = **1,100,000**. A ignores the
incentive; B takes the whole underrun; D miscomputes.

**MCQ 7.1-C `[7.1.4 · Application]`** Target cost USD 10,000,000, 50/50 pain-share; actual cost USD 10,800,000.
The contractor's pain-share is:
- A. USD 800,000
- B. USD 400,000 ✅
- C. USD 0
- D. USD 10,800,000

*Rationale:* Overrun 800,000 × 50 % = **400,000** borne by the contractor. A is the whole overrun; C ignores
the mechanism; D is the actual cost.

**MCQ 7.1-D `[7.1.3 · Application]`** CPIF: target cost USD 6,000,000, target fee USD 500,000, share ratio
**70/30** (client/contractor), actual cost USD 6,800,000. The contractor's fee is:
- A. USD 260,000 ✅
- B. USD 500,000
- C. USD 740,000
- D. USD 100,000

*Rationale:* `Fee = target fee + contractor's share × (target − actual) = 500,000 + 30 % × (6,000,000 −
6,800,000) = 500,000 − 240,000 = 260,000`. B ignores the incentive adjustment; C adds the share instead of
subtracting it on an overrun; D wrongly applies a 50 % share.

**MCQ 7.1-E `[7.1.2 · Recall]`** Under a **remeasurement** contract, the client bears ____ risk and the
contractor bears ____ risk:
- A. quantity; rate ✅
- B. rate; quantity
- C. all cost; no
- D. no; all cost

*Rationale:* Remeasurement pays agreed rates against actual quantities, so the client pays for whatever
quantities arise (quantity risk) while the contractor is held to its tendered rates (rate risk). B inverts the
allocation; C describes cost-plus; D describes lump sum.

### Self-check — KA 7.1

1. Order lump sum, target cost and cost-plus by who bears cost risk. *(Lump sum → contractor; target cost →
   shared; cost-plus → client.)*
2. Why does a target-cost contract make an `EAC` a commercial number? *(An `EAC` above target quantifies the
   contractor's pain-share now.)*

---

## Knowledge Area 7.2 — Contract management

*Topics: 7.2.1 the contract lifecycle · 7.2.2 variations and claims · 7.2.3 liquidated damages · 7.2.4
retention, bonds and guarantees · 7.2.5 standard forms (FIDIC awareness).*

> **Standing caution for this knowledge area.** What follows describes the *management discipline* of contract
> administration — the records, notices, valuations and forecasts a controls professional owns. It does not
> state the law of any jurisdiction and it is not legal advice. Whether a clause means what a party thinks it
> means, whether an entitlement arises, whether an instrument is effective as drafted, and what any of it
> obliges either party to do are questions determined by the contract and by the applicable law; they vary
> between contract families and jurisdictions, they change, and they belong to qualified counsel. The
> professional obligation this book does impose is to keep the record, raise the question early, and never
> assert a contractual position the function is not qualified to take.

### 7.2.1 The contract lifecycle

> **Fig 7.2.1 — The contract lifecycle.** *Caption:* form, mobilise, administer, complete, close — administration is where the money and the records move. *Data:* five stages; administer highlighted.

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

**Worked example 7.2.2 — building up a prolongation claim's quantum.**

1. **Setup.** A **30-day** client-caused delay (cause established by contemporaneous records and a delay
   analysis). Time-related preliminaries run at **USD 6,000/day**; specialist equipment on standby costs
   **USD 1,500/day**.
2. **Formula.** `quantum = Σ (time-related cost rate × delay days)` — for costs actually incurred *because*
   of the delay, evidenced, with no double-count against variations.
3. **Substitution.** Preliminaries `30 × 6,000 = 180,000`; equipment standby `30 × 1,500 = 45,000`.
4. **Result.** A substantiated quantum of **USD 225,000**, presented with its cause (the instruction/event),
   effect (the critical-path delay — Domain 10) and evidence trail.
5. **Interpretation.** Cause, effect, quantum — all three, or the claim fails. The rates come from the
   contract/BoQ preliminaries (7.3.3); the delay days from a critical-path analysis, not a bar-chart
   impression; and the records from the discipline of 7.2.1. A claim assembled after the fact from memory is
   worth a fraction of one built contemporaneously.

### 7.2.3 Liquidated damages

**Definition & purpose.** **Liquidated damages (LDs)** are a pre-agreed sum payable by the contractor for a
defined breach — usually **late completion**. Professional practice sets the rate as a **genuine
pre-estimate** of the client's loss rather than as a punishment, and that is the discipline this book teaches.
Legal systems differ in how they treat a pre-agreed sum that looks punitive rather than compensatory — the
distinction is a live one in many common-law jurisdictions, and several civil-law systems approach the
question differently again, with scope for adjustment. Which position applies to a given contract, and with
what effect, turns on the governing law and is a question for qualified counsel; the professional obligation
is to have it checked rather than assumed, and to record the answer. They give certainty (both parties know the cost of delay in advance) and cap the contractor's delay
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
- B. USD 20,000
- C. USD 200,000 ✅
- D. USD 2,000,000

*Rationale:* `10,000 × 20 = 200,000`. A is one day; B swaps the figures; D misplaces a zero.

**MCQ 7.2-B `[7.2.3 · Recall]`** In professional practice a liquidated-damages rate is set to represent:
- A. A punitive penalty to deter breach.
- B. The contractor's total revenue.
- C. The retention amount.
- D. A genuine pre-estimate of the client's likely loss. ✅

*Rationale:* practice sets the rate against a genuine pre-estimate of the client's likely loss rather than as
a punishment (7.2.3); how a particular legal system treats a rate that looks punitive varies and is a question
for counsel. A describes the punitive framing practice avoids;
B and C are unrelated figures.

**MCQ 7.2-C `[7.2.4 · Analysis]`** Increasing retention from 5 % to 10 % on a project will, all else equal:
- A. Improve the contractor's cash position.
- B. Deepen the contractor's funding trough (more cash withheld for longer). ✅
- C. Have no cash effect.
- D. Reduce liquidated damages.

*Rationale:* Higher retention withholds more cash for longer, deepening the funding trough (Domain 3, KA 3.5).
It worsens (not improves) cash, has a clear cash effect, and is unrelated to LDs.

**MCQ 7.2-D `[7.2.2 · Application]`** A substantiated client-caused delay of **45 days** extends time-related
preliminaries at USD 8,000/day and keeps specialist plant on standby at USD 2,000/day. The prolongation
quantum is:
- A. USD 360,000
- B. USD 90,000
- C. USD 10,000
- D. USD 450,000 ✅

*Rationale:* `quantum = Σ (time-related rate × delay days) = 45 × 8,000 + 45 × 2,000 = 360,000 + 90,000 =
450,000`. A omits the plant standby; B omits the preliminaries; C is the combined daily rate for one day only.

**MCQ 7.2-E `[7.2.4 · Analysis]`** A contractor substitutes a **retention bond** for 5 % cash retention. The
main commercial effect is:
- A. The contractor's cash position improves — payments are received in full — at the cost of the bond fee. ✅
- B. The client loses all security for defects.
- C. The contractor's cash position worsens.
- D. Liquidated damages no longer apply.

*Rationale:* A retention bond substitutes third-party security for withheld cash, so the contractor collects
full value now, shallowing the funding trough, in exchange for the bond's fee. The client still holds security
(the bond), so B is wrong; C reverses the cash effect; D confuses unrelated instruments.

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

**Worked example 7.3.3 — building up a unit rate from first principles.**

1. **Setup.** Build the rate for one unit of installed pipework. Inputs per unit: **labour 2.5 hours at
   USD 45/hour**; **materials USD 60**; **plant USD 18**. Company overheads are recovered at **12 %** on
   direct cost, and profit at **8 %** on cost including overheads.
2. **Formula.** `direct cost = labour + materials + plant`; `+ overheads (12 %)`; `+ profit (8 % of the
   subtotal)`.
3. **Substitution.** Labour `2.5 × 45 = 112.50`; direct cost `112.50 + 60 + 18 = 190.50`; overheads
   `190.50 × 12 % = 22.86` → subtotal `213.36`; profit `213.36 × 8 % = 17.07`.
4. **Result.** A tendered rate of **USD 230.43 per unit** (≈ USD 230).
5. **Interpretation.** Every BoQ rate decomposes into labour, materials, plant, overhead and profit — which
   is exactly how a controls professional interrogates a rate in negotiation (which element moved?) and how
   variance analysis later splits a rate problem from a usage problem (Domain 4, KA 4.2.3).

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
- B. USD 4,800
- C. USD 66,000
- D. USD 64,800 ✅

*Rationale:* `5,400 × 12 = 64,800`. A is the original; B is only the extra; C miscomputes.

**MCQ 7.3-B `[7.3.3 · Analysis]`** Preliminaries are significant on a project that is now forecast to finish
late. The main commercial consequence is:
- A. Preliminaries fall automatically.
- B. Time-related preliminaries extend, creating prolongation cost. ✅
- C. The BoQ rates change.
- D. Retention is released early.

*Rationale:* Preliminaries are largely time-related, so a delay extends them (prolongation), a common claim
head. Delay does not reduce them, change measured rates, or accelerate retention release.

**MCQ 7.3-C `[7.3.3 · Application]`** A unit rate is built up from first principles: labour 3 hours at USD
40/hour, materials USD 50, plant USD 30; overheads at **10 %** on direct cost; profit at **5 %** on the
subtotal including overheads. The tendered rate is:
- A. USD 200
- B. USD 220
- C. USD 231 ✅
- D. USD 230

*Rationale:* Direct cost `= 3 × 40 + 50 + 30 = 200`; overheads `200 × 10 % = 20` → subtotal `220`; profit
`220 × 5 % = 11` → **231**. A is direct cost only; B forgets profit; D wrongly takes profit on direct cost
(200 × 5 % = 10) instead of on the subtotal.

**MCQ 7.3-D `[7.3.1 · Recall]`** At tender stage, the principal purpose of issuing a BoQ to bidders is:
- A. To let each tenderer measure its own quantities.
- B. To fix the final contract sum regardless of quantities.
- C. To give all tenderers a common set of quantities to price, making bids comparable. ✅
- D. To replace the drawings and specification.

*Rationale:* The BoQ provides a common tender basis — every bidder prices the same measured quantities, so
rates and totals compare like for like. A defeats that purpose; B describes lump sum pricing, not the BoQ's
role; D is wrong because the BoQ is prepared *from* the drawings and specification, not instead of them.

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

**Worked example 7.4.3b — retention release at completion and defects.**

1. **Setup.** Over the job, retention of **5 %** was withheld on a final contract value of **USD 300,000**,
   i.e. **USD 15,000** held. The contract releases **half at practical completion** and **half after the
   12-month defects period**.
2. **Formula.** `Retention held = 5 % × final value`; released in two tranches.
3. **Substitution.** `Retention held = 5 % × 300,000 = 15,000`; release `= 7,500` at completion and `7,500`
   after defects.
4. **Result.** **USD 7,500** is released at practical completion and the final **USD 7,500** after the defects
   period — cash the contractor earned early but collects late.
5. **Interpretation.** Retention is a real receivable to track and chase to full recovery; a controls
   professional includes the phased release in the cash-flow forecast (Domain 3, KA 3.5) rather than treating
   the job as "paid" at completion.

**Advance payment and its recovery.** Many EPC and Gulf-region contracts pay the contractor a **mobilisation
advance** — commonly **5–15 % of the contract price**, secured by the advance-payment bond of 7.2.4 — before
significant work is measured. The advance is then **recovered pro-rata** as a deduction on each interim
certificate, typically `recovery = advance % × gross value certified this period`, so the valuation cascade
becomes gross value − retention − advance recovery − previous certificates.

**Worked example 7.4.3c — recovering a mobilisation advance in the valuation.**

1. **Setup.** A contract of **USD 8,000,000** pays a mobilisation advance of **10 % = USD 800,000**,
   recovered pro-rata at 10 % of each gross certificate; retention is **5 %**. A period certificate certifies
   gross work of **USD 500,000** (no previous deductions on this slice).
2. **Formula.** `retention = 5 % × gross`; `advance recovery = 10 % × gross`; `net for the period = gross −
   retention − advance recovery`.
3. **Substitution.** Retention `= 5 % × 500,000 = 25,000`; advance recovery `= 10 % × 500,000 = 50,000`.
4. **Result.** Net for the period `= 500,000 − 25,000 − 50,000 = 425,000`.
5. **Interpretation.** The advance is a **loan repaid through the measure**: the cash-flow forecast
   (Domain 3, KA 3.5) must carry both the early inflow and the reduced net certificates that repay it, and by
   the time the advance is fully recovered the advance-payment bond steps down and is released (7.2.4).

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

**The downstream side: subcontractor applications.** Everything this KA describes also runs **downstream**:
subcontractors apply to the main contractor, who values, certifies and pays on the same discipline — usually
**back-to-back** with the main contract (retention percentages, payment terms and certification timing
mirrored down, so the main contractor is not funding the gap between what it pays and what it collects). The
controls: certify against **measured** work, not the application (the sub's optimism is the same
over-measurement risk as 7.5.2, one tier down); hold back-to-back retention; keep payment terms aligned
(paying subs in 30 while collecting in 60 is a working-capital leak, Domain 11, Advanced 11.A.1); and mirror
the main-contract notice regimes, because a sub's claim un-passed-up in time becomes the main contractor's
own cost (7.2.2).

**Worked example 7.4.4 — certifying a subcontractor's application.**

1. **Setup.** A subcontractor applies for **USD 300,000** gross work done to date. The QS measure supports
   **280,000**. Retention is **5 %** back-to-back; previously certified net is **190,000**.
2. **Formula.** `net certified to date = certified gross × (1 − retention)`; `this certificate = net to date −
   previously certified net`.
3. **Substitution.** `280,000 × 0.95 = 266,000`; `266,000 − 190,000 = 76,000`.
4. **Result.** Certify **USD 76,000** this period — against an application implying `300,000 × 0.95 − 190,000
   = 95,000`; the 19,000 difference is the over-application the measure removed.
5. **Interpretation.** The certificate, not the application, drives the accrual (Domain 1, KA 1.3.5) and the
   cost ledger — book the accrual at the **certified** measure and the P2P three-way-match discipline
   (Domain 11, KA 11.2.2) holds one tier down. The 20,000 gross over-application is not an insult; it is
   Tuesday — which is why the measure, not the paperwork, is the control.

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
- B. Impossible.
- C. A breach of IFRS 15.
- D. Expected — they value the same progress at different values under different rules; the differences are reconciled and meaningful. ✅

*Rationale:* `EV` (budget), valuation (contract rates) and revenue (IFRS 15) measure the same progress
differently, so they diverge by design; the professional reconciles them. It is neither an error nor a breach.

**MCQ 7.4-C `[7.4.3 · Application]`** Gross value of work done to date is USD 240,000; retention is 5 %;
previous payments total USD 180,000. The amount due this application is:
- A. USD 48,000 ✅
- B. USD 60,000
- C. USD 228,000
- D. USD 57,000

*Rationale:* Retention `= 5 % × 240,000 = 12,000`; net certified `= 240,000 − 12,000 = 228,000`; amount due
`= 228,000 − 180,000 = 48,000`. B ignores retention; C omits previous payments; D wrongly applies retention to
the period movement (`(240,000 − 180,000) × 95 %`) instead of to the gross value.

**MCQ 7.4-D `[7.4.2 · Recall]`** The amount that actually drives the contractor's cash inflow each period is:
- A. The applied amount.
- B. The certified amount. ✅
- C. The BoQ tender total.
- D. The earned value.

*Rationale:* The contractor applies, but the client's representative certifies, and the client pays the
certified sum — so certification, not application, drives cash. C is a tender-stage figure; D values progress
at budget for performance measurement, not billing.

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

**Balance-sheet geography.** A project throws three distinct balances onto the balance sheet, and each answers
a different question. **Accrued (recognised) revenue** is what performance has *earned* under IFRS 15
(Domain 2, KA 2.2); **invoiced revenue** is what the billing engine has *asked for* (7.4). They differ in
timing on almost every project, and the difference lives in one of two places: where recognised revenue
exceeds billing, a **contract asset** (commonly "unbilled" or "WIP") — a right to consideration still
conditional on something other than the passage of time; where billing exceeds recognised revenue, a
**contract liability** — cash or invoices ahead of performance. A **trade receivable** is different from both:
it arises only when the right to payment becomes *unconditional* (typically on invoicing/certification) — so
the project's cycle is performance → contract asset (unbilled) → invoice → receivable → cash. The controls
consequences follow. DSO (Domain 11, Advanced 11.A.1) starts at the *invoice*, so value sitting unbilled is
invisible to DSO — a project can show pristine DSO while months of earned value sit unbilled; the honest
monitor pairs DSO with **days unbilled** (the age of the contract asset). And a growing contract asset is
either a billing-discipline problem (fix the application cycle, 7.4.1) or an early sign that recognised
revenue is running ahead of what the client will certify (the over-measurement risk of 7.5.2) — the two have
opposite remedies, so the balance must be *aged* and *explained*, not just reported.

**Worked example 7.5.2b — one contract, three balances.**

1. **Setup.** At month-end, cumulative recognised revenue on a contract is **USD 1,000,000** (over-time, input
   method). Cumulative applications invoiced and certified: **USD 800,000**, of which the client has paid
   **USD 650,000**.
2. **Formula.** `contract asset (unbilled) = recognised − invoiced`; `receivable = invoiced − collected`.
3. **Substitution.** Contract asset `= 1,000,000 − 800,000 = 200,000`; receivable
   `= 800,000 − 650,000 = 150,000`.
4. **Result.** Balance sheet: **contract asset USD 200,000** + **trade receivable USD 150,000** — USD 350,000
   of performance not yet turned into cash. (Had billing instead run to 1,150,000 against the same 1,000,000
   recognised, the position would be a **contract liability of 150,000** — billing ahead of performance.)
5. **Interpretation.** Three balances, three different questions: the contract asset asks "why haven't we
   billed it?" (7.4.1) or "will the client certify it?" (7.5.2); the receivable asks "why haven't they paid?"
   (Domain 11, KA 11.1); the contract liability asks "have we borrowed performance from next month?" — and
   cash-flow forecasting (Domain 3, KA 3.5) must model all three, because each converts to cash on a
   different clock.

### 7.5.3 The commercial-to-accounting loop

**The synthesis.** This KA closes the loop that runs through the whole book: **scope** (WBS/BoQ) → **cost**
(Domains 1, 5) → **earned value** (Domain 6) → **billing** (this domain) → **revenue** (Domain 2) → **the
statements** (Domains 1–2) → **management reporting** (Domain 4). Each is a different value of the same
physical progress; a controls professional who can trace a slipped activity all the way through this loop —
to its cost, its billing, its revenue, its contract asset/liability, and the board narrative — is exercising
exactly the integrated judgement the credential certifies.

### 7.5.4 Sector mini-case — a rail contract variation and claim

A light-rail contractor on a lump-sum contract priced at **USD 80,000,000** hits changed ground conditions.
The client instructs a **variation** adding **USD 3,000,000** of priced scope — a contract modification in
revenue terms (Domain 2, KA 2.2.8). The contractor also lodges a **claim** for **prolongation**: time-related
preliminaries running at **USD 200,000/month** for an assessed **4 months** = **USD 800,000** (cause: the
client-caused delay; effect: extended preliminaries; quantum: 800,000). Certified interim billing continues
against the updated contract value while IFRS 15 revenue is recognised on performance — the difference sitting
in the contract asset/liability (7.5.2). Throughout, the controls professional keeps the contemporaneous
records that substantiate the claim and reconciles the variation, the claim, billing and revenue — the
integrated commercial-to-accounting judgement the credential certifies.

**AI in this KA.** AI supports the commercial cycle: extracting terms and variation clauses from contracts,
analysing claims/variations, checking valuations and flagging billing anomalies, and reconciling billing to
`EV` and IFRS 15 revenue. The recognition and entitlement judgements — whether a variation is valid, whether a
claim is substantiated, whether revenue may be recognised — remain the professional's, auditable and signed
off. **AI proposes; the professional verifies, decides and remains accountable.**

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
- B. A revenue error.
- C. A contract liability of USD 250,000. ✅
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

**MCQ 7.5-C `[7.5.2 · Application]`** Cumulative IFRS 15 revenue recognised is USD 4,200,000; cumulative
certified billing is USD 3,900,000. The balance-sheet position is:
- A. A contract asset of USD 300,000. ✅
- B. A contract liability of USD 300,000.
- C. A contract asset of USD 8,100,000.
- D. Nil.

*Rationale:* Revenue exceeds billing by `4,200,000 − 3,900,000 = 300,000` → **under-billing → contract
asset**. B reverses the direction; C wrongly adds the two figures; D ignores the gap.

**MCQ 7.5-D `[7.5.2 · Analysis]`** A project shows a persistent and **growing contract liability**
(over-billing). The best commercial reading is:
- A. The project has collected cash ahead of performance and still owes the work to earn it out. ✅
- B. The project has under-billed and cash is tied up in unbilled work.
- C. Revenue has been recognised incorrectly.
- D. The project is certain to be profitable.

*Rationale:* Over-billing means billing runs ahead of IFRS 15 performance — favourable for cash now, but a
liability to be earned out through future work. B describes a contract *asset*; C is wrong because the gap is
a designed feature of two rule-sets, not an error; D confuses a billing-timing position with profitability.

### Self-check — KA 7.5

1. What does a large, growing *contract asset* signal? *(Under-billing — performing ahead of collections; cash
   tied up in unbilled work.)*
2. Trace the commercial-to-accounting loop in order. *(Scope → cost → EV → billing → revenue → statements →
   reporting.)*

---

## Advanced topics — Domain 7

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 7.A.1 — Concurrent delay at concept level

**The problem.** The prolongation claim of 7.2.2 assumed a clean cause: a 30-day *client-caused* delay,
established by records. Real projects are rarely so tidy. **Concurrent delay** arises when both parties cause
delay over the **same window** — the client's late instruction and the contractor's own resource shortfall
each, independently, drive the critical path across the same weeks. The apportionment problem follows: how
much **time** (an extension of time, relieving liquidated damages, 7.2.3) and how much **money** (prolongation
cost, 7.2.2) is the contractor entitled to when its own default contributed to the very period it claims for?

**Common approaches, described neutrally.** Practice recognises several ways through, and different contracts
and forums resolve it differently — this reference stays at concept level, consistent with its stance on
standard forms (7.2.5). Under a **dominant cause** approach, the effective or dominant cause of the delay is
identified and carries the whole entitlement. Under **apportionment**, time and/or money are divided in
proportion to each party's causative contribution. Under a **time-but-no-money** outcome, the contractor
receives the extension of time — relief from LDs for the concurrent period — but not its prolongation cost,
on the reasoning that it would have incurred those time-related costs anyway because of its own delay. Note
the commercial asymmetry: an extension of time *without* money is still valuable, because it switches off an
LD exposure priced per day (7.2.3), even though it recovers nothing.

**Why records decide it.** Whichever approach a contract adopts, the analysis stands or falls on
**contemporaneous records** (7.2.1–7.2.2): programme updates, daily diaries, instructions and notices that
show what was actually driving the critical path week by week (Domain 10). Untangling two overlapping causes
after the fact, from memory, is close to impossible — which is why, in practice, the party with the better
records shapes the concurrent-delay analysis, and the controls professional's daily discipline is the real
determinant of the outcome.

### Advanced 7.A.2 — Global claims and their weakness

**Definition.** A **global claim** (or total-cost claim) asserts the whole cost overrun — actual cost minus
tender — as the composite result of many events, **without linking each cause to its own effect and
quantum**. It is tempting when disruption arises from dozens of interacting events and the contractor argues
that separating their effects is impracticable: one global number, one global cause list.

**Why tribunals resist them.** A global claim rests on implicit assumptions that rarely survive scrutiny:
that the tender was perfectly priced, that the contractor caused none of the overrun, and that no neutral
events contributed. Because the quantum is a single undifferentiated total, demonstrating *any*
contractor-caused cost inside it — or any tender underpricing — undermines the whole claim; there is no
mechanism for removing the bad element from the composite. The form also inverts the discipline of 7.2.2:
instead of cause → effect → quantum, it offers a quantum in search of causes. In practice such claims are
heavily discounted in negotiation and rarely succeed intact.

**The antidote.** The **cause–effect–quantum discipline**, applied event by event: each variation,
instruction and delay event is notified when it occurs (7.2.2), its effect on the critical path is analysed
(Domain 10), and its cost is isolated and evidenced — as the rail mini-case does with its prolongation head
(7.5.4: cause, effect, quantum of USD 800,000). The controls professional's contribution is structural and
early: cost coding fine enough to capture each event's cost separately as it is incurred (Domain 5, with the
change log of KA 5.4), and the contemporaneous records of 7.2.1. A global claim presented at the final
account is usually the symptom of a controls failure a year earlier — costs that were never coded to events
when they could have been.

### Advanced 7.A.3 — On-demand vs conditional securities

**The practical difference.** KA 7.2.4 introduced bonds as instruments; the distinction that matters in
practice is **how they are called**. An **on-demand** security is payable by the bank or surety on written
demand, **without proof of default** — pay first, argue later. A **conditional** (default) security pays only
on **proven default** under its conditions — slower, and harder to call. The difference is a risk allocation:
on-demand gives the client immediate, liquid security but exposes the contractor to the risk of an unfair
call that converts instantly into cash; conditional protects the contractor from that risk but gives the
client weaker, slower security. Each costs a **fee** (a percentage per annum of the bond value), and
on-demand instruments typically consume the contractor's bank facility headroom much as borrowing does — an
opportunity cost against working capital (Domain 3, KA 3.5) beyond the visible fee.

**Worked example 7.A.3 — the cost of an unreleased bond.**

1. **Setup.** A performance bond of **10 %** on a **USD 20,000,000** contract, at a fee of **1.5 % per
   annum** of bond value. Release is conditioned on the defects certificate, which slips **9 months**.
2. **Formula.** `Bond value = 10 % × contract value`; `fee = 1.5 % p.a. × bond value × time outstanding`.
3. **Substitution.** Bond value `= 10 % × 20,000,000 = 2,000,000`; annual fee `= 1.5 % × 2,000,000 =
   30,000`; cost of the slip `= 30,000 × 9/12 = 22,500`.
4. **Result.** The 9-month slip costs **USD 22,500** in bond fees alone — before counting the facility
   headroom the instrument occupies for those extra months.
5. **Interpretation.** Securities cost money for exactly as long as they run — which is why a controls
   professional tracks **security expiry and release dates like milestones** (7.2.4): a register of each
   instrument's form (on-demand or conditional), value, beneficiary, expiry and release conditions, with the
   release triggers (practical completion, the defects certificate) diarised and chased exactly as retention
   is (7.4.3b). An expired bond leaves the client unsecured; an unreleased one bleeds fees; and an on-demand
   bond left in force through a dispute is live exposure.

### Advanced 7.A.4 — FX and escalation clauses

**The principle.** The contract type allocates *cost* risk (7.1); specific clauses allocate **price-level**
and **currency** risk, and they are read the same way — *who pays if it moves?* Under a **firm (fixed)
price**, the contractor bears inflation risk and must price it at tender, using exactly the escalation
estimating of Domain 3's advanced topics (3.A.1) — the client buys certainty at a premium. Under a
**fluctuation provision**, the price is adjusted — usually by a published index applied to defined cost
elements — so the client bears inflation risk transparently and the contractor stops gambling on indices.
Currency works the same way: where the currency of cost differs from the currency of payment, the contract's
FX clause decides who bears the movement — a single-currency price (the contractor bears and may hedge), a
multi-currency payment schedule, or an exchange-rate adjustment mechanism — and the consequences flow into
the multi-currency accounting of Domain 1's advanced topics (1.A.1).

**Worked example 7.A.4 — a fluctuation adjustment.**

1. **Setup.** Under a fluctuation provision, the **labour element** of the year's certified value —
   **USD 4,000,000** — is index-linked; the agreed labour index moves from **100** at the base date to
   **106** at the valuation date.
2. **Formula.** `Adjustment = indexed element × (current index − base index) / base index`.
3. **Substitution.** `Adjustment = 4,000,000 × (106 − 100)/100 = 4,000,000 × 6 % = 240,000`.
4. **Result.** The client pays a fluctuation adjustment of **USD 240,000** on top of the measured value.
5. **Interpretation.** Under a firm price the same 240,000 does not vanish — it sits inside the contractor's
   tendered escalation allowance (3.A.1), paid by the client whether or not the inflation arrives. The clause
   moves the **bearer** of the risk, not the existence of the cost.

**The controls consequence.** Read the escalation and FX clauses before forecasting: they determine whether
an index spike is a client-borne adjustment (fluctuation), a contractor variance against a firm price, or an
exchange difference for the accounts (1.A.1) — and the cost forecast (Domain 3) must escalate and convert on
the **same basis the contract pays**, or the commercial and controls numbers will diverge for no real reason.

### Advanced 7.A.5 — Disruption and the measured mile

**The principle.** **Disruption** is distinct from **delay** (Advanced 7.A.1): the work may still finish on
time, but it costs more because productivity was degraded — out-of-sequence working, congestion, piecemeal
access, repeated remobilisation. Disruption claims fail for the same reason global claims fail (Advanced
7.A.2): asserting a lump of lost money without a causal chain. The **measured mile** is the accepted
antidote: compare the claimant's own achieved productivity in an un-impacted period (the "mile") with
productivity in the impacted period, and price the difference. The benchmark is the contractor's own
performance on the same work — not a theoretical norm — which is precisely what makes it persuasive.

**Worked example 7.A.5 — a measured-mile disruption claim.**

1. **Setup.** Cable-pulling on the same spread, with the same crews. Un-impacted period: **1,200 m**
   installed in **3,000 labour-hours**. Impacted period (piecemeal access after the client's late release of
   work fronts): **800 m** in **2,600 labour-hours**. Demonstrated cost rate: **USD 85/hour**.
2. **Formula.** `Productivity = hours / quantity`; `disruption hours = (impacted rate − mile rate) ×
   impacted quantity`; `quantum = disruption hours × cost rate`.
3. **Substitution.** Mile `= 3,000 / 1,200 = 2.5 h/m`; impacted `= 2,600 / 800 = 3.25 h/m`; excess
   `= 3.25 − 2.5 = 0.75 h/m`; disruption hours `= 0.75 × 800 = 600 h`; quantum `= 600 × 85 = 51,000`.
4. **Result.** A disruption claim of **USD 51,000**, priced entirely from the contractor's own records.
5. **Interpretation.** Cause (late access, evidenced), effect (productivity degradation, measured), quantum
   (priced from records) — the claim structure of 7.2.2 satisfied with the contractor's own data.

**The caveats that make it honest.** The mile must be genuinely comparable — same work type, same crews,
same conditions; an early mile flattered by the learning curve overstates the claim. Records make or break
it: the method needs timesheets coded to areas and periods, which is exactly the data-integrity discipline
of Domain 5 (KA 5.2.4) applied a year before anyone knew a claim was coming. And where no clean mile exists,
the fallback is a properly evidenced bottom-up build of the lost hours — never a global assertion (7.A.2).
Pattern-finding productivity dips across coded timesheet data is a strong machine task; choosing the
defensible mile and owning the causal story is the commercial professional's.

### Advanced 7.A.6 — When agreement fails: the dispute-resolution ladder

**The principle.** Most variations and claims settle through the contract machinery of 7.2.2; a **dispute**
is what remains when they do not. Contracts arrange what happens next as an **escalating ladder**, each rung
slower, costlier and less controllable than the one below it:

- **Structured negotiation.** Commercial teams first; failing that, escalation to executives who are not
  personally invested in the positions taken below. Cheap, fast, relationship-preserving — and where the
  great majority of disputes end.
- **Standing dispute boards / adjudication.** An independent board or adjudicator — appointed at the outset
  or on referral — gives a decision typically expressed as **binding at least temporarily**, the design
  intent being that the parties act on it while
  the underlying dispute resolves or is escalated, so the project keeps moving — a
  decision in weeks, not years. Whether, and with what effect, such a decision binds in a particular case
  turns on the contract and on the applicable law, and is a question for counsel rather than for this book.
- **Arbitration or litigation.** The final rung: a binding award or judgment. Final, slow and costly — and
  by the time a claim reaches this rung, the records discipline of 7.2.2 and the claim file of Toolkit 7.T.2
  **is** the case, because the tribunal sees only what was recorded at the time.

**The controls angles.** Each rung up costs more and takes longer, so pricing settlement against escalation
is an **expected-value decision** (Domain 12, KA 12.2.3): a probability-weighted recovery, net of the costs
and delay of climbing, compared with the offer on the table. Disputed sums need **consistent treatment** in
the EAC and in revenue — recognised only within the variable-consideration constraint (7.5.1), never booked
at the claimed figure merely because the ladder exists. And the ladder changes behaviour before anyone climbs
it: where the next rung is fast and cheap (adjudication), parties negotiate harder and settle earlier; where
it is an arbitration years away, weak positions can be held for leverage. Securities interact too — a
formal dispute is precisely when an on-demand bond call becomes a live risk (Advanced 7.A.3, 7.2.4).

---

## Case study — Domain 7: the commercial year on a rail megaproject

### Background

A joint-venture contractor holds the **tunnelling-and-stations package** on a metropolitan rail megaproject —
twin-bored running tunnels, two underground stations and the cross-passages between them. The package is
delivered under a collaborative **target-cost contract**: target cost **USD 120,000,000**, target fee
**USD 8,000,000**, with a **50/50** pain/gain share on any difference between the adjusted target and the
final actual cost, and the contractor's pain-share **capped at USD 10,000,000**. The client chose the form
deliberately (KA 7.1.4): the ground risk on a bored-tunnel job is too large for a sensible lump sum — a
fixed-price tenderer would either load the price with contingency or gamble on the geology — while pure
cost-plus would leave the client carrying every inefficiency. Sharing the risk 50/50 keeps both parties
leaning on the same side of the cost line, and the cap bounds the contractor's downside so that a
catastrophic overrun cannot destroy it.

The commercial team and the project-controls team sit in the same weekly meeting — a deliberate arrangement,
because on a target-cost contract the controls forecast *is* a commercial number. This case study follows one
commercial year on the package through four connected events: an instructed **variation** that adjusts the
target; a routine **interim application for payment**; an emerging **liquidated-damages exposure** on a
sectional milestone; and the **year-end pain/gain position** that ties the earned-value forecast (Domain 6)
directly to the contractor's fee. A closing section reconciles the three measures of the same physical
progress — earned value, the BoQ valuation and IFRS 15 revenue — that the year produces.

### The variation (KA 7.2.2, Domain 2 KA 2.2.8)

In the second quarter the client's fire-and-life-safety review requires an **additional cross-passage**
between the running tunnels. The client issues a formal instruction; the contractor prices the change from
the BoQ-derived rates for excavation, lining, mechanical fit-out and the associated time-related
preliminaries (KA 7.3), and the parties agree the variation at **USD 2,500,000** — *before* the work is put
in hand. Under the contract's mechanism the agreed variation **adjusts the target cost** from
USD 120,000,000 to **USD 122,500,000**; the target fee is unchanged in this case, and the pain/gain
calculation at year end will run against the adjusted figure.

The discipline matters more than the arithmetic. The variation is priced from rates the parties already
agreed at tender, so the negotiation is about quantities and method, not about re-opening the pricing basis;
it is agreed before the work wherever possible, so neither party carries an unpriced exposure into the
ground; and it is folded into the target immediately, so that the controls team's variance analysis
(Domain 6) is always measured against a target that reflects the *authorised* scope. This is the commercial
counterpart of change control (Domain 5, KA 5.4) — the same instructed change flows through the change log,
the baseline, the target and, on the revenue side, is treated as a **contract modification** under IFRS 15
(Domain 2, KA 2.2.8), adjusting the transaction price and the measure of progress. One event, four
disciplined entries; an undisciplined project records it in none of them and argues about all four at the
final account.

### An interim application (KA 7.4)

At the October valuation date the quantity surveyors assess the value of work done to date against the BoQ
and the milestone schedule, and the contractor submits its interim application.

**Worked example CS7-1 — the October interim application.**

1. **Setup.** Gross value of work done to date **USD 62,000,000** (measured at contract rates against the
   BoQ, KA 7.3); retention **3 %**; **previous payments** total **USD 55,400,000**.
2. **Formula.** `Retention = 3 % × gross value`; `amount due = gross value − retention − previous payments`.
3. **Substitution.** Retention `= 3 % × 62,000,000 = 1,860,000`; amount due `= 62,000,000 − 1,860,000 −
   55,400,000 = 4,740,000`.
4. **Result.** **Amount due this application: USD 4,740,000** — gross value 62,000,000, less cumulative
   retention 1,860,000, less previous payments 55,400,000.
5. **Interpretation.** The USD 1,860,000 of retention is cash the contractor has earned but cannot yet
   collect (KA 7.2.4) — it sits in the funding trough (Domain 3, KA 3.5) until release at completion and
   after defects. The cumulative structure of the calculation is the point: each application values *all*
   work to date and nets off *all* prior payments, so an error in any month self-corrects in the next.

One line of professional caution attaches to every application: it is the **certified** amount, not the
applied amount, that drives cash (KA 7.4.2). The contractor applies for 4,740,000; the engineer certifies
what it agrees; and the controls team tracks the **certified-versus-applied gap** month by month as a
commercial signal — a small, stable gap is normal negotiating friction, while a widening gap flags a valuation
dispute, an unagreed variation or a deteriorating relationship long before it reaches a formal claim.

### The LD exposure (KA 7.2.3)

In the same quarter the integrated schedule (Domain 10) delivers unwelcome news: the eastern station box is
running late, and the critical path shows a possible **60-day** late completion against a **sectional
milestone** — handover of the station structure to the systems contractor — which carries liquidated damages
of **USD 25,000/day**. The exposure is quantified immediately: `60 × 25,000 = 1,500,000`. This
**USD 1,500,000** is not left in the scheduler's report; it is carried in the **commercial forecast alongside
the EAC**, because a forecastable LD liability is as real a cost as a forecastable quantity overrun.

Quantifying the exposure also enables the decision it exists to inform: an **acceleration-versus-LDs
comparison**. The planners price a recovery plan — additional shifts on the station fit-out and resequenced
mechanical works — at **USD 900,000** of acceleration cost. Accepting the delay costs 1,500,000 in LDs;
accelerating costs 900,000; accelerating **saves USD 600,000** — *provided the acceleration genuinely
recovers the 60 days*. That proviso is the analytical heart of the decision, and it belongs to the crashing
logic of Domain 10: acceleration money only buys time on the **critical path**, recovery plans suffer
diminishing returns as crews stack up, and a plan that recovers only 30 of the 60 days changes the
arithmetic entirely. The commercial team supplies the LD rate and the exposure; the planning team supplies
an honest answer on recoverability; neither can make the decision alone.

### The year-end pain/gain position (KA 7.1.3–7.1.4)

At year end the controls team's estimate at completion (`EAC`, Domain 6) — built bottom-up from performance
to date, the remaining quantities and the recovery plan — is presented to the project board alongside the
commercial team's pain/gain calculation. On this contract they are two halves of one number.

**Worked example CS7-2 — the year-end pain/gain position.**

1. **Setup.** Forecast actual cost (`EAC`) **USD 129,000,000**; adjusted target cost **USD 122,500,000**
   (the original 120,000,000 plus the agreed 2,500,000 variation); target fee **USD 8,000,000**; pain-share
   **50/50**, capped at **USD 10,000,000**.
2. **Formula.** `Overrun = EAC − adjusted target`; `pain-share = 50 % × overrun` (subject to the cap);
   `forecast fee = target fee − pain-share`.
3. **Substitution.** Overrun `= 129,000,000 − 122,500,000 = 6,500,000`; pain-share `= 50 % × 6,500,000 =
   3,250,000` — below the 10,000,000 cap, so it applies in full; forecast fee `= 8,000,000 − 3,250,000 =
   4,750,000`.
4. **Result.** The contractor's forecast fee falls from 8,000,000 to **USD 4,750,000**; the client's
   forecast total is `cost 129,000,000 + fee 4,750,000 = 133,750,000`, of which the client's own share of
   the pain is the other 3,250,000 of the overrun.
5. **Interpretation.** On a target-cost megaproject the `EAC` **is** a commercial number: every 1,000,000 of
   forecast overrun costs the contractor 500,000 of fee, until the cap bites — here at an overrun of
   20,000,000 (where `50 % × 20,000,000 = 10,000,000` exhausts the cap). This is why the controls forecast
   and the commercial position must be **one conversation**: an optimistic `EAC` understates the pain-share
   the board is already committed to, and a controls professional who reports `CPI` without translating it
   into fee is answering half the question.

The board's response illustrates the incentive working as designed. With 3,250,000 of fee already forecast
to be lost and 6,750,000 of headroom before the cap, every cost-saving and every recovered day has a
measurable owner on both sides of the table. The 600,000 saved by the acceleration decision above, if the
recovery holds, flows straight through this calculation — which is precisely the alignment the collaborative
form was chosen to create.

### Reconciling the three progress measures (KA 7.4.4–7.5)

The year closes with the reconciliation that KA 7.4.4 makes the controls professional's own. Three systems
have each measured the same physical progress under a different rule-set:

| Measure | Rule-set | Values progress at | Year-end position |
|---|---|---|---|
| Earned value (`EV`) | Performance baseline (Domain 6) | Budget | **52 %** complete |
| BoQ valuation | Contract payment mechanism (KA 7.3–7.4) | Contract rates | **USD 62,000,000** certified |
| IFRS 15 revenue | Cost-to-cost over time (Domain 2, KA 2.2) | Recognised revenue | A slightly different cumulative figure; billing-vs-revenue gap held as a contract asset/liability (Domain 2, KA 2.2.7) |

These are **three values of the same physical progress under three rule-sets**, and they will not — and
should not — agree. Earned value prices progress at budget to answer a *performance* question; the BoQ
valuation prices it at contract rates to answer a *cash* question; IFRS 15 recognises revenue on a
cost-to-cost measure to answer an *accounting* question, with the difference between cumulative billing and
cumulative revenue sitting on the balance sheet as a contract asset or liability. The project reconciles the
three **monthly**: each difference is explained (rate versus budget differentials, unbilled variations,
retention, the modification's effect on the transaction price), the direction and trend of the contract
asset/liability position is read as a health signal (KA 7.5.2), and the numbers are **never forced to
match** — a project that "aligns" them by adjustment has destroyed three independent signals to manufacture
one false one.

### What the credential expects

This case study is Domain 7 in miniature, and the credential expects a candidate to move through it without
changing gear. From **KA 7.1**, read the contract type first: identify who bears cost risk under a
target-cost form, run the pain/gain arithmetic in both directions, and know where the cap bites — because
the cap defines the real exposure the board is managing. From **KA 7.2**, administer the instruments:
a variation instructed, priced and agreed before the work; an LD exposure quantified from the LD rate and
the forecast delay, and weighed against the cost of acceleration; and behind both, the contemporaneous
records that protect entitlement. From **KA 7.3**, price from the bill: the variation and the valuation both
stand on BoQ-derived rates, which is what makes them negotiable on quantities and method rather than
first principles. From **KA 7.4**, run the payment cycle: gross value, less retention, less previous
payments, and the certified-versus-applied gap watched as a signal. And from **KA 7.5**, close the loop:
billing, earned value and IFRS 15 revenue reconciled monthly, with the contract asset/liability explaining
the gap. AI accelerates the paperwork throughout — contract-analytics tools that extract terms and variation
clauses, and billing-anomaly checks that flag a mispriced application in seconds (KA 13.5.7) — but the
entitlement judgements and the pain/gain conversation stay human: **AI proposes; the professional verifies, decides and remains accountable.**

---

## Case study B — Domain 7: a compensation-event season on a gas-plant EPC package (oil & gas)

### Background

An EPC contractor is delivering a **brownfield gas-dehydration train and its tie-ins** into a live gas plant,
under a reimbursable **target-cost contract**: target cost **USD 85,000,000**, fee **USD 6,000,000**, with
pain and gain shared **60/40 client/contractor** and the contractor's pain **capped at the fee** — beyond
that point the client carries the overrun alone. The contract calls its changes **compensation events (CEs)**,
and the brownfield setting breeds them: tie-ins into forty-year-old pipework reveal conditions no survey
fully caught, and much of the hot work is compressed into a fixed plant-shutdown window. **Provisional
acceptance** is roughly five months away.

The commercial quarter this case follows is unglamorous and typical: three CEs to price under three
different pricing routes, a prolongation claim the client disputes on concurrency grounds, a
retention-and-bond position that turns the acceptance dossier into a cash instrument, and the quarterly
pain/gain update that converts the controls team's `EAC` (Domain 6) into the contractor's fee. Running
beneath all four is the discipline the outcome actually turns on: **contemporaneous records** — daily
diaries, signed daywork sheets, weekly programme updates (KA 7.2.1).

### Three compensation events, three pricing routes (KA 7.3)

**CE-014 — instructed pipe-rack extension, priced at BoQ rates.** The scope is measurable and the BoQ has
rates for all of it, so the pricing is quantities × tendered rates plus the time-related element:

| Item | Qty | Unit | Rate (USD) | Amount (USD) |
|---|---:|---|---:|---:|
| Structural steel, supply and erect | 150 | t | 4,800 | 720,000 |
| Pipework, fabricate and erect | 600 | dia-in | 850 | 510,000 |
| Cable, supply, pull and terminate | 12,000 | m | 35 | 420,000 |
| Time-related preliminaries | 6 | week | 75,000 | 450,000 |
| **CE-014 total** | | | | **2,100,000** |

Because the rates were agreed at tender, the negotiation is confined to quantities and method — no re-opening
of the pricing basis (KA 7.3.4). Note the preliminaries line: six weeks of extended time-related cost is part
of the change's *full* impact, not an afterthought to claim later.

**CE-019 — emergent work, priced on dayworks.** During the shutdown window the crews open up the rack and
find **corroded pipe supports** that must be replaced immediately — no drawings, no BoQ item, no time to
pre-price. The site instruction puts the work on **dayworks**: recorded resources at the contract's daywork
schedule.

1. **Setup.** Recorded and signed daily: **2,400 labour hours** at USD 95/hour; **20 crane-days** at
   USD 3,600/day; materials at invoiced cost **USD 80,000** plus the schedule's **10 %** handling allowance.
2. **Formula.** `CE value = Σ (recorded resource × schedule rate) + materials at cost × 1.10`.
3. **Substitution.** Labour `2,400 × 95 = 228,000`; crane `20 × 3,600 = 72,000`; materials `80,000 × 1.10 =
   88,000`.
4. **Result.** **CE-019 = USD 388,000.**
5. **Interpretation.** Dayworks trade pricing certainty for record dependency: the value *is* the records.
   Every sheet here was signed by the client's supervisor at the end of each shift — which is why the number
   settled in a week. An unsigned daywork claim assembled after demobilisation is not a price; it is an
   argument.

**CE-023 — a star rate.** A corrosive service line must be re-specified from carbon steel to **duplex
stainless**, for which the BoQ simply has no item. The parties build a **star rate** — a new rate constructed
from first principles on the same anatomy as a tendered rate (KA 7.3.3): labour `2.8 h × 100 = 280`;
materials `870`; plant and consumables `50` — direct cost **USD 1,200 per dia-in** — plus site overheads at
**15 %** giving **USD 1,380/dia-in**. No separate profit element is added: on this target-cost form the
margin lives in the fee, not in the rates. Agreed for **450 dia-in**: `450 × 1,380 = ` **USD 621,000**.

The three agreed CEs total `2,100,000 + 388,000 + 621,000 = ` **USD 3,109,000**, and the mechanism folds them
into the target: **adjusted target cost = 85,000,000 + 3,109,000 = USD 88,109,000** — so the pain/gain
calculation at quarter end runs against a target that reflects the authorised scope (the commercial twin of
Domain 5's change control).

### The disputed prolongation claim (KA 7.2.2, Advanced 7.A.1)

The contractor claims **45 days** of prolongation — cause: the client's late release of revised P&IDs and
delayed permit-to-work access at the tie-in points — at the contract's time-related rate of **USD 60,000/day**:
a claimed quantum of `45 × 60,000 = ` **USD 2,700,000**, built cause → effect → quantum with a critical-path
analysis attached (KA 7.2.2).

The client's review does not attack the cause; it attacks the *window*. The weekly programme updates and both
parties' diaries show that for **20 of the 45 days**, the contractor's own fabricated spools were late from
its vendor — a contractor-risk delay independently driving the same critical path over the same weeks:
**concurrent delay** (Advanced 7.A.1). The contract resolves concurrency on the **time-but-no-money**
principle, and the assessment follows:

- **Extension of time: the full 45 days** — relieving liquidated damages across the whole window. At the
  sectional LD rate of USD 30,000/day, the EOT on the 20 concurrent days alone switches off `20 × 30,000 =
  600,000` of exposure — valuable even though it recovers nothing (the commercial asymmetry 7.A.1 notes).
- **Prolongation money: the 25 clean days only** — `25 × 60,000 = ` **USD 1,500,000**, on the reasoning that
  the time-related costs of the concurrent 20 days would have been incurred anyway through the contractor's
  own delay.

The instructive point is *how* the answer was reached: not advocacy, but records. The client's diaries proved
the late P&IDs; the contractor's own expediting reports proved the spool delay. Because both parties kept
contemporaneous records, untangling the overlap was arithmetic rather than litigation — the party with better
records shapes a concurrency analysis, and here the records were good enough that neither side could shape it
unfairly (Advanced 7.A.1).

### Retention and bonds at the approach to provisional acceptance (KA 7.2.4)

With provisional acceptance five months out, the securities register becomes a forecasting instrument.
Certified value to date is **USD 78,000,000**; retention is withheld at **5 %** — **USD 3,900,000** held —
with **half (1,950,000) releasing at provisional acceptance** and half at final acceptance after the defects
period. The **performance bond** of 10 % (`8,500,000`) steps down to 5 % at provisional acceptance, and costs
**1.2 % per annum** of its outstanding value.

1. **Setup.** Retention release at PA **USD 1,950,000**; contractor's cost of capital **8 %**; bond
   step-down at PA **USD 4,250,000** at 1.2 % p.a.
2. **Formula.** `Monthly cost of PA slippage = retention release × 8 % / 12 + step-down value × 1.2 % / 12`.
3. **Substitution.** Retention financing `= 1,950,000 × 8 % / 12 = 13,000`; bond fee
   `= 4,250,000 × 1.2 % = 51,000` per annum `→ 4,250` per month.
4. **Result.** Every month provisional acceptance slips costs the contractor about **USD 17,250** in cash —
   before counting the bank-facility headroom the un-stepped bond occupies (Advanced 7.A.3).
5. **Interpretation.** The punch list and the acceptance dossier are commercial documents, not engineering
   tidiness: the controls team diarises the PA triggers and chases them exactly as it chases milestones,
   because retention and bond releases are receivables with dates (KA 7.2.4).

### The pain/gain forecast update (KA 7.1.4)

At quarter end the `EAC` and the commercial position are presented as one table:

| Position | Last quarter (USD) | This quarter (USD) |
|---|---:|---:|
| Target cost (adjusted) | 85,000,000 | 88,109,000 |
| Forecast actual cost (`EAC`) | 86,200,000 | 89,609,000 |
| Forecast overrun | 1,200,000 | 1,500,000 |
| Contractor pain-share (40 %) | 480,000 | 600,000 |
| Forecast fee (target fee 6,000,000 − pain) | 5,520,000 | 5,400,000 |

The decomposition of the movement is the professional content. The target rose 3,109,000 through the agreed
CEs; the `EAC` rose 3,409,000 — the CE work at forecast cost **plus USD 300,000 of genuine performance
deterioration** on the tie-in productivity. It is only that 300,000 that costs fee: `40 % × 300,000 =
120,000`, taking the forecast fee from 5,520,000 to 5,400,000. An update that reported "EAC up 3.4m" without
splitting change from performance would tell the board nothing it can act on. The cap check completes the
picture: pain is capped at the 6,000,000 fee, so the cap bites at an overrun of `6,000,000 / 40 % =
15,000,000` — against a current forecast overrun of 1,500,000, the contractor's remaining fee exposure is
real but far from exhausted, and every dollar saved on the remaining work still returns forty cents of fee.

### What the credential expects

This is Domain 7 as it is actually practised — season by season, not clause by clause. From **KA 7.1**, the
target-cost mechanics run in both directions: the fee forecast moved by the pain-share, the 60/40 split, and
the cap located (`overrun of 15,000,000`) so the board knows where the exposure regime changes. From
**KA 7.3**, three pricing routes chosen to fit three changes — tendered BoQ rates where items exist, dayworks
where emergent work must start before it can be priced, a star rate built from first principles where the
bill is silent — and the discipline of knowing which route applies, including why the star rate carries no
profit element on this form. From **KA 7.2.2 and Advanced 7.A.1**, the claim: cause–effect–quantum on the way
up, concurrency on the way down, time-but-no-money applied, and the whole thing decided by whichever records
existed — the strongest possible advertisement for the daily diary. From **KA 7.2.4**, securities as live
cash items with dates, priced monthly. AI earns its keep on the paperwork — extracting CE clauses, digitising
and totalling daywork sheets, flagging rate misapplications (Domain 13, KA 13.5.7) — but entitlement,
concurrency and the pain/gain conversation are judgements. **AI proposes; the professional verifies, decides and remains accountable.**

---

## Executive perspective — Domain 7

**What the executive must hold onto.** The **contract type** decides who pays for an overrun before the
first variance exists — lump sum puts the risk on the contractor, cost-plus on the client, target cost
shares it — so it is the first commercial fact a board should establish, not the last (KA 7.1). On
incentivised forms the `EAC` **is** a commercial number: every unit of forecast overrun moves fee through
the pain/gain mechanism until the cap bites, which makes the controls forecast and the commercial position
one conversation. And billing, earned value and IFRS 15 revenue are **three values of the same physical
progress under three rule-sets** — they should reconcile with explanations, never be forced to match
(KA 7.5).

**Six questions to ask from the chair.**

1. Under this contract, who pays for the next dollar of overrun — and at what point does the cap or collar
   bite?
2. What does the current `EAC` do to our forecast fee and pain/gain position?
3. What is the liquidated-damages exposure at the forecast completion date, and how does it compare with
   the cost of acceleration?
4. Which variations and claims are notified but not yet agreed, and are the contemporaneous records in
   place to substantiate them?
5. What is the gap between applied and certified this period, and which way is the contract
   asset/liability position trending?
6. How much retention is outstanding, and when — and against what conditions — is it due for release?

**The traps at board level.**

- **Pain/gain caps misunderstood.** A 50/50 share reads as symmetric risk, but beyond the cap the exposure
  is no longer shared — knowing where the cap bites is knowing the real exposure being managed.
- **An optimistic `EAC` as a hidden commercial position.** On a target-cost form, understating the forecast
  understates a pain-share the organisation is already committed to; reporting `CPI` without translating it
  into fee answers half the question.
- **The three numbers forced to agree.** "Aligning" earned value, certified billing and recognised revenue
  by adjustment destroys three independent signals to manufacture one false one; the differences are the
  information.
- **Entitlement lost for want of records.** Claims are decided on notices and contemporaneous evidence
  (cause, effect, quantum); a claim assembled after the fact from memory is worth a fraction of one built
  as events occurred (KA 7.2).

**What good looks like.** The board can state, for each major contract, who bears cost risk, where the
caps bite, and what the current `EAC` means for fee — because the controls forecast and the commercial
calculation are presented as two halves of one number. Variations are instructed, priced and agreed before
the work; claims run on contemporaneous records; LD exposure is quantified against the schedule forecast
and weighed against recovery options. Billing, earned value and revenue are reconciled monthly, each
difference explained, with the contract asset/liability read as the cash-and-performance health signal it
is — and retention tracked to the last dollar released.

---

## Calculation exercises — Domain 7

*Work each exercise before reading its solution; every step uses only this domain's methods.*

**Exercise 7.1** — A CPIF contract sets a target cost of **USD 8,000,000**, a target fee of
**USD 640,000** and a **60/40** share ratio (client/contractor). On the same terms, compute the
contractor's fee and the client's total price in two cases: (a) actual cost **USD 7,400,000**;
(b) actual cost **USD 8,900,000**.

**Solution 7.1.**

1. Formula: `fee = target fee + contractor's share × (target cost − actual cost)`.
2. Case (a): underrun `= 8,000,000 − 7,400,000 = 600,000`; contractor's share `= 40 % × 600,000 =
   240,000`; fee `= 640,000 + 240,000 = 880,000`; client pays `7,400,000 + 880,000 = 8,280,000`.
3. Case (b): overrun `= 8,900,000 − 8,000,000 = 900,000`; fee reduction `= 40 % × 900,000 = 360,000`;
   fee `= 640,000 − 360,000 = 280,000`; client pays `8,900,000 + 280,000 = 9,180,000`.
4. Check against the target price of `8,640,000`: the client saves `360,000` (its 60 % of the
   underrun) in (a) and bears `540,000` extra (its 60 % of the overrun) in (b) — the incentive works
   symmetrically in both directions.

**Exercise 7.2** — A target-cost contract has a target cost of **USD 12,000,000**, a target fee of
**USD 900,000**, a **50/50** pain/gain share, and the contractor's pain-share **capped at
USD 800,000** (a fee floor of USD 100,000). Compute the contractor's pain-share, the resulting fee
and the client's share of the overrun when actual cost is (a) **USD 13,200,000** and
(b) **USD 14,500,000**.

**Solution 7.2.**

1. Case (a): overrun `= 13,200,000 − 12,000,000 = 1,200,000`; uncapped pain-share `= 50 % ×
   1,200,000 = 600,000` — inside the cap, so it stands. Fee `= 900,000 − 600,000 = 300,000`; the
   client bears the other `600,000`.
2. Case (b): overrun `= 14,500,000 − 12,000,000 = 2,500,000`; uncapped pain-share `= 50 % ×
   2,500,000 = 1,250,000` — **beyond the cap**, so the contractor bears only `800,000`. Fee
   `= 900,000 − 800,000 = 100,000` (the floor); the client bears `2,500,000 − 800,000 = 1,700,000`.
3. Interpretation: below the cap the parties share pain 50/50; beyond it every further dollar of
   overrun is the client's — a controls professional forecasting an `EAC` above target must know
   where the cap bites to state the real exposure.

**Exercise 7.3** — Price the following substructure BoQ. On completion the actual concrete quantity
is **1,350 m³** (all other quantities are as billed) and the contract is a remeasurement form —
compute the remeasured total and say who pays the difference.

| Item | Description | Qty | Unit | Rate (USD) |
|---|---|---:|---|---:|
| A | Excavation | 4,000 | m³ | 15 |
| B | Concrete | 1,200 | m³ | 180 |
| C | Reinforcement | 90 | t | 1,400 |
| P | Preliminaries | 1 | sum | 78,000 |

**Solution 7.3.**

1. Amounts (`quantity × rate`): A `4,000 × 15 = 60,000`; B `1,200 × 180 = 216,000`; C `90 × 1,400 =
   126,000`; P `78,000`.
2. Tender total `= 60,000 + 216,000 + 126,000 + 78,000 = 480,000`.
3. Remeasured concrete `= 1,350 × 180 = 243,000` — an extra `243,000 − 216,000 = 27,000`.
4. Remeasured total `= 480,000 + 27,000 = 507,000`. Under remeasurement the client pays the extra
   **USD 27,000** (quantity risk sits with the client); under lump sum the same volume, if within
   the defined scope, would have been the contractor's risk.

**Exercise 7.4** — On the BoQ of Exercise 7.3 (item amounts: A USD 60,000; B USD 216,000;
C USD 126,000; P USD 78,000), a valuation date shows progress of A **100 %**, B **60 %**, C **30 %**
and P **50 %**. Retention is **5 %** and previous payments total **USD 140,000**. Prepare the
interim application: gross value, retention, net certified value and amount due.

**Solution 7.4.**

1. Gross value per item: A `100 % × 60,000 = 60,000`; B `60 % × 216,000 = 129,600`; C `30 % ×
   126,000 = 37,800`; P `50 % × 78,000 = 39,000`.
2. Gross value `= 60,000 + 129,600 + 37,800 + 39,000 = 266,400`.
3. Retention `= 5 % × 266,400 = 13,320`; net certified `= 266,400 − 13,320 = 253,080`.
4. Amount due `= 253,080 − 140,000 = 113,080`.
5. **Amount due this application: USD 113,080.** The USD 13,320 retained is earned but uncollected
   cash — part of the funding trough until its staged release.

**Exercise 7.5** — A project carries two delay positions. First, a **25-day client-caused** delay is
established by contemporaneous records and a critical-path analysis; time-related preliminaries run
at **USD 7,000/day** and standby craneage at **USD 2,000/day**. Second, the forecast shows
completion **15 days late** for reasons that are the contractor's own risk; LDs are
**USD 12,000/day**, and an acceleration package costing **USD 100,000** would recover **10** of the
15 days. Compute the prolongation quantum, the unmitigated LD exposure, and whether accelerating
pays.

**Solution 7.5.**

1. Prolongation quantum `= 25 × 7,000 + 25 × 2,000 = 175,000 + 50,000 = 225,000` — presented with
   its cause, effect and evidence trail.
2. LD exposure without action `= 15 × 12,000 = 180,000`.
3. With acceleration: residual delay `= 15 − 10 = 5` days; residual LDs `= 5 × 12,000 = 60,000`;
   total cost `= 100,000 + 60,000 = 160,000`.
4. Accelerating saves `180,000 − 160,000 = 20,000` — worth doing, and exactly the
   accelerate-versus-LDs comparison the board should see priced.

---

## Practitioner's toolkit — Domain 7

*Adoption-ready artefacts; adapt the column headings and thresholds to your organisation, then keep them
stable.*

### Toolkit 7.T.1 — Contract commercial summary sheet

| Field | Entry | Worked example — rail tunnelling-and-stations package (case study) |
|---|---|---|
| Contract type & risk allocation (7.1) | Who bears cost risk, and why the form was chosen | Target cost — cost risk shared 50/50; ground risk too large for lump sum |
| Price / target & fee mechanics (7.1.3–7.1.4) | Contract sum or target cost; fee basis | Target cost USD 122,500,000 (original 120,000,000 + agreed variation 2,500,000); target fee USD 8,000,000 |
| Pain/gain share & caps (7.1.4) | Share ratio; where the cap/collar bites | 50/50 share; contractor pain-share capped at USD 10,000,000 (cap bites at a USD 20,000,000 overrun) |
| LD rate & cap (7.2.3) | Rate per day, the milestone it attaches to, any cap | USD 25,000/day on the sectional station-handover milestone |
| Retention % & release (7.2.4, 7.4.3) | Percentage withheld; release triggers | 3 % withheld; half at practical completion, half after the defects period |
| Bonds & expiry dates (7.2.4, 7.A.3) | Each instrument's form (on-demand/conditional), value, expiry, release condition | Performance bond, on-demand, 10 % of contract value; release at defects certificate — diarised |
| Key notice periods (7.2.1–7.2.2) | Contractual time limits for claims/variation notices | Claim notice within the contract's stated period of the event; records kept contemporaneously |
| Variation procedure refs (7.2.2) | Instruction, pricing basis, agreement point, target/baseline adjustment | Instructed in writing; priced from BoQ rates; agreed before work; target adjusted immediately |

**Usage note.** One sheet per contract, completed at award and kept current, so the first commercial fact —
who bears cost risk (7.1.1) — is never rediscovered mid-crisis. The example column echoes the rail
megaproject case: on a target-cost form the sheet is what lets the controls team translate an `EAC` into
fee (every USD 1,000,000 of overrun costs the contractor 500,000 until the cap bites, KA 7.1.4), and the LD
rate is what prices a forecast delay against acceleration (7.2.3). The bonds and notice-period rows are the
diary entries: securities cost fees for exactly as long as they run (7.A.3), and entitlement is lost for
want of a notice served in time (7.2.2). Review the sheet at every variation, since an adjusted target
moves every downstream calculation.

### Toolkit 7.T.2 — Claim/variation file checklist

- [ ] Instructing event or delay event identified, and the contractual notice served in time (7.2.1–7.2.2).
- [ ] Contemporaneous records assembled — daily diaries, programme updates, instructions, correspondence, photographs (7.2.1).
- [ ] Cause established event by event — not a global/total-cost assertion (7.2.2; Advanced 7.A.2).
- [ ] Effect demonstrated on the critical path by a proper delay analysis, not a bar-chart impression (Domain 10).
- [ ] Concurrent delay considered — records able to show what drove the path week by week (Advanced 7.A.1).
- [ ] Quantum built from contract/BoQ rates and time-related preliminaries (7.3.3), each cost isolated and evidenced.
- [ ] No double-count between the claim's quantum and priced variations (7.2.2).
- [ ] Extension-of-time and LD relief quantified alongside the money claim (7.2.3).
- [ ] Commercial forecast, billing and IFRS 15 revenue treatment aligned — the variation treated as a contract modification (Domain 2, KA 2.2.8; 7.5).
- [ ] File progressed to agreement — status and next action tracked, not parked for the final account.

**Usage note.** The checklist enforces the cause–effect–quantum discipline of 7.2.2: all three, or the
claim fails — and the antidote to the global claim of Advanced 7.A.2 is exactly this event-by-event build,
with cost coding fine enough to capture each event's cost as it is incurred (Domain 5, KA 5.4). The
records items matter most: whichever apportionment approach a concurrent-delay dispute adopts, the party
with the better contemporaneous records shapes the analysis (7.A.1). The alignment item closes the loop
this domain exists for — the same event flows through the change log, the target, billing and IFRS 15
revenue (7.5.4), and a file that agrees in one system but not the others will be argued four times at the
final account.

---

## Exam preparation — Domain 7

**How this domain is examined.** Domain 7 mixes commercial recall (who bears which risk under each contract
form; what makes LDs enforceable) with a dense band of application arithmetic: **pain/gain and CPIF fee
adjustments** (KA 7.1), **prolongation and LD quantums** (KA 7.2), **BoQ pricing, rate build-ups and
remeasurement** (KA 7.3) and the **interim-application cascade** (KA 7.4). Analysis items centre on reading
the contract asset/liability position and on reconciling the three progress measures. Nearly every
calculation is a short cascade of two or three steps — the marks are lost in step order, not in the
multiplication.

**Calculation traps.**

- **Applying the share ratio to the wrong base — or the wrong party.** The contractor's fee moves by *its*
  share of `(target cost − actual cost)`; a 70/30 client/contractor split means 30 % to the contractor, and
  an overrun *subtracts* from the fee (MCQ 7.1-D).
- **Forgetting the cap.** Compute the uncapped pain-share first, then test it against the cap; beyond the
  cap every further dollar of overrun is the client's (worked example 7.1.4b; Exercise 7.2).
- **Retention on the wrong base.** Retention applies to the *gross cumulative* value, not to the period
  movement or the net figure (MCQ 7.4-C's distractor D) — and the amount due nets off *previous payments*
  after retention, not before.
- **LD exposure day-counts.** Exposure = LD rate × forecast days late — swapped figures and misplaced zeros
  are the planted distractors (MCQ 7.2-A); with acceleration, cost the *residual* days plus the acceleration
  price (Exercise 7.5).
- **Remeasuring at the tendered instead of the actual quantity.** The remeasured amount is actual quantity ×
  rate; the "extra" alone, and the original tender amount, are both distractors (MCQ 7.3-A).
- **Profit on the wrong subtotal.** In a rate build-up, overheads apply to direct cost and profit to the
  subtotal *including* overheads (MCQ 7.3-C).

**Time management.** Work every payment cascade in ledger order — gross value, retention, net certified,
previous payments, amount due — writing each line down; skipping a step is where the distractors live.
Contract-asset/liability items take seconds once the direction rule (billing above revenue → liability) is
fixed in memory, so bank those marks early and spend the surplus on the pain/gain scenarios.

**Reflection questions.**

1. For the contracts you currently work under, can you state from memory who bears cost risk and where each
   cap or collar bites?
2. If your project slipped 30 days tomorrow, how quickly could you substantiate cause, effect and quantum
   from records that already exist?
3. What has your certified-versus-applied gap done over the last six months, and what is that trend telling
   you?
4. How does your organisation explain the monthly differences between earned value, certified billing and
   recognised revenue — reconciliation, or force-fit?

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

