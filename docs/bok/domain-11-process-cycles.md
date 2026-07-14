# Domain 11 — Business Process Cycles (O2C, P2P & the control environment)

> **Group:** Project management. **Target:** ~45 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). British English; USD (+SAR where useful). Connects
> the commercial/cost cycles (Domains 5, 7) to the accounting cycles (Domains 1–2).

## Why this domain exists

Behind every project number is a **business process**: money comes in through the **order-to-cash (O2C)**
cycle and goes out through the **procure-to-pay (P2P)** cycle, and both run on **internal controls** that keep
them honest. A controls professional who understands these cycles knows *where* the cost and revenue figures
they rely on come from, *where* they can go wrong, and *what controls* protect them. This domain covers O2C
(KA 11.1), P2P (KA 11.2), and the internal-control environment — **segregation of duties** and the audit trail
(KA 11.3). It is short but load-bearing: it is where the accounting of Domains 1–2, the cost control of Domain
5, and the commercial cycle of Domain 7 meet the day-to-day transaction flow.

**Learning objectives.** After this domain a candidate can: describe the O2C and P2P cycles end-to-end and
their key controls; link them to revenue/receivables and cost/commitments/payables; explain the **three-way
match** and **segregation of duties**; and describe how the audit trail and process controls (and process
mining) protect the numbers.

---

## Knowledge Area 11.1 — Order-to-Cash (O2C)

*Topics: 11.1.1 the cycle end-to-end · 11.1.2 controls in O2C · 11.1.3 the link to revenue and receivables.*

### 11.1.1 The cycle end-to-end

**Definition & purpose.** **Order-to-cash** is the cycle by which the organisation turns a customer order into
collected cash. Its stages: **order/contract** → **credit check** → **fulfilment/delivery** (or, for projects,
work performed and certified) → **invoicing/billing** → **collection** → **cash application** (matching cash
received to invoices). On a project this is the interim-valuation-and-payment flow of Domain 7 (KA 7.4) seen as
a repeatable process.

### 11.1.2 Controls in O2C

**The key controls.**

- **Credit control** — assessing and limiting customer credit before committing, to manage the risk of
  non-payment (the collectability test that also gates IFRS 15 revenue, Domain 2, KA 2.2.2).
- **Billing accuracy and cut-off** — billing the right amount in the right period (Domain 1, KA 1.3.5).
- **Cash application and reconciliation** — matching receipts to invoices and reconciling the receivables
  ledger; unapplied cash and ageing receivables are the exceptions to chase.

### 11.1.3 The link to revenue and receivables

**The principle.** O2C is where **revenue** (Domain 2) and **receivables** are generated and collected. A
controls professional reads the **receivables ageing** as a leading indicator of both cash risk (Domain 3, KA
3.5) and possible revenue/billing problems: a growing overdue balance may signal a disputed valuation, a
delivery issue, or a customer in distress. The O2C cycle is the operational engine behind the cash-inflow side
of the funding curve.

### Key terms — KA 11.1

| Term | Meaning |
|---|---|
| **Order-to-cash (O2C)** | Order → credit → fulfil → invoice → collect → apply cash. |
| **Credit control** | Assessing/limiting customer credit before committing. |
| **Cash application** | Matching cash received to invoices. |
| **Receivables ageing** | The overdue profile of amounts owed — a cash/revenue leading indicator. |

### Sample MCQs — KA 11.1

**MCQ 11.1-A `[11.1.1 · Recall]`** Which sequence correctly orders O2C stages?
- A. Invoice → order → collect → deliver.
- B. Order → credit check → fulfil → invoice → collect → apply cash. ✅
- C. Order → pay → receive → invoice.
- D. Requisition → PO → receipt → payment.

*Rationale:* B is the O2C flow. D is the *P2P* cycle; A and C are out of sequence or mix the cycles.

**MCQ 11.1-B `[11.1.3 · Analysis]`** A growing overdue receivables balance is best read as:
- A. Always an accounting error.
- B. A leading indicator of cash risk and possible billing/revenue disputes. ✅
- C. Irrelevant to controls.
- D. A reason to recognise more revenue.

*Rationale:* Ageing receivables signal collection risk and can flag disputed valuations or customer distress —
a leading indicator. It is not automatically an error, is highly relevant, and is not a basis to recognise more
revenue.

### Self-check — KA 11.1

1. List the O2C stages in order. *(Order → credit check → fulfil → invoice → collect → apply cash.)*
2. Why does a controls professional watch receivables ageing? *(Leading indicator of cash risk and
   billing/revenue disputes.)*

---

## Knowledge Area 11.2 — Procure-to-Pay (P2P)

*Topics: 11.2.1 the cycle end-to-end · 11.2.2 the three-way match · 11.2.3 the link to cost, commitments and
payables.*

### 11.2.1 The cycle end-to-end

**Definition & purpose.** **Procure-to-pay** is the cycle by which the organisation turns a need into a paid
supplier. Its stages: **requisition** → **purchase order (PO)** → **goods/services receipt** → **invoice** →
**three-way match** → **payment**. These stages are exactly the **commitment → accrual → actual** states of
cost control (Domain 5, KA 5.2): the PO is the **commitment**, the receipt supports the **accrual**, and the
matched invoice becomes the **actual**.

### 11.2.2 The three-way match

**Definition & purpose.** The **three-way match** is the core P2P control: before an invoice is paid, it is
matched against the **purchase order** (was this ordered, at this price?) and the **goods-receipt note** (was
it received, in this quantity?). Only when all three agree — within tolerance — is the invoice approved for
payment. It prevents paying for goods not ordered, not received, or over-priced, and it is the control that
makes the accrual (from the receipt) and the actual (from the invoice) trustworthy.

**Worked example 11.2.2 — a three-way match exception.** A PO orders **100 units at USD 50** (USD 5,000). The
goods-receipt note records **100 units received**. The invoice bills **100 units at USD 55** (USD 5,500). The
match **fails on price** (PO USD 50 vs invoice USD 55): the USD 500 difference is held as an exception for
investigation — a price increase to authorise, or an error to reject — **before** payment. Without the match,
USD 500 of unauthorised cost would flow straight through to the project.

**Worked example 11.2.2b — a three-way match that passes.**

1. **Setup.** A purchase order is for **100 units at USD 50** (USD 5,000). Only **98 units are received**
   (short delivery); the supplier invoices **98 units at USD 50** (USD 4,900).
2. **Formula.** The invoice is matched to the **goods-receipt note** (quantity) and the **PO** (price), within
   tolerance.
3. **Substitution.** Invoice quantity 98 = goods-receipt 98 ✓; invoice price USD 50 = PO price USD 50 ✓;
   invoice value `98 × 50 = 4,900`.
4. **Result.** The match **passes** — the entity pays **USD 4,900** for what was actually received at the
   agreed price; the 2-unit short delivery is a delivery/expediting matter, not a payment block.
5. **Interpretation.** The three-way match pays for **goods received at the agreed price**, not for what was
   ordered — protecting against paying for undelivered units. Contrast worked example 11.2.2, where the price
   differed and the match failed (cross-ref 11.2.2).

### 11.2.3 The link to cost, commitments and payables

**The principle.** P2P generates the **commitments, accruals and actuals** a controls professional tracks
(Domain 5) and the **payables** the accounts record (Domain 1). Watching **open POs** (commitments) gives cost
lead time; watching **goods-received-not-invoiced** drives the accruals that make cost-to-date true (Domain 5,
KA 5.2.2). The P2P cycle is the operational engine behind the cost side of the project.

### Key terms — KA 11.2

| Term | Meaning |
|---|---|
| **Procure-to-pay (P2P)** | Requisition → PO → receipt → invoice → three-way match → pay. |
| **Three-way match** | Matching invoice to PO and goods-receipt note before payment. |
| **Open PO / GRNI** | Commitment / goods-received-not-invoiced (accrual driver). |

### Sample MCQs — KA 11.2

**MCQ 11.2-A `[11.2.2 · Application]`** A PO is 100 units at USD 50; goods receipt 100 units; invoice 100 units
at USD 55. The three-way match:
- A. Passes — quantities agree.
- B. Fails on price — a USD 500 exception is held for investigation before payment. ✅
- C. Fails on quantity.
- D. Is unnecessary.

*Rationale:* Quantities agree but the price differs (50 vs 55), so the match fails on price; the USD 500 is
held as an exception. Quantity is fine, and the match is exactly the control that catches this.

**MCQ 11.2-B `[11.2.1 · Analysis]`** In P2P, the purchase order corresponds to which cost-control state?
- A. Actual.
- B. Accrual.
- C. Commitment. ✅
- D. Payment only.

*Rationale:* Raising a PO creates a **commitment** (Domain 5). The receipt supports the accrual; the matched
invoice becomes the actual.

### Self-check — KA 11.2

1. What three documents does the three-way match compare, and what does it prevent? *(PO, goods-receipt note,
   invoice; paying for goods not ordered, not received, or mispriced.)*
2. Map the P2P stages to commitment/accrual/actual. *(PO → commitment; receipt → accrual; matched invoice →
   actual.)*

---

## Knowledge Area 11.3 — Internal control and segregation of duties

*Topics: 11.3.1 internal control · 11.3.2 segregation of duties · 11.3.3 the audit trail and process mining.*

### 11.3.1 Internal control

**Definition & purpose.** **Internal controls** are the policies and procedures that provide reasonable
assurance over the reliability of reporting, the effectiveness of operations, and compliance — including the
authorisation limits, matches, reconciliations and approvals embedded in O2C and P2P. Controls are
**preventive** (stop an error/fraud occurring — the three-way match, credit limits) or **detective** (find it
after — reconciliations, exception reports). A controls professional both **relies on** these controls (the
numbers are trustworthy because the controls work) and **operates** some of them (reconciliations, cut-off).

### 11.3.2 Segregation of duties

**The principle.** **Segregation of duties (SoD)** requires that **no single person controls a whole
transaction** end-to-end — because concentration of duties enables error and fraud to go undetected. In P2P,
the person who **raises** a PO should not also **approve** it, **receive** the goods, and **pay** the invoice;
in O2C, the person who **bills** should not also **receive and apply** the cash and **write off** the debt.
Splitting these duties means a second person's action is required, creating a check.

**Worked example 11.3.2 — an SoD weakness.** If one clerk can raise a purchase order, receive the goods
(confirm receipt), *and* approve the invoice for payment, they could create a fictitious supplier and pay
themselves with no second check. Segregating **raise / approve / receive / pay** across different people (or
system roles) removes that single point of failure — the same principle as keeping connector configuration and
delivery under different admin controls in a system trust boundary.

**11.3.2b Worked example — a segregation-of-duties matrix.** A small matrix makes the design concrete: four
P2P duties across two clerks and a manager, marking who may perform each.

| Duty | Clerk A | Clerk B | Manager |
|---|---|---|---|
| Raise PO | ✔ | | |
| Approve PO | | | ✔ |
| Confirm goods receipt | | ✔ | |
| Approve invoice for payment | | | ✔ |

No single person performs more than one **conflicting** duty in the raise→approve→receive→pay chain; raising
(Clerk A) is separated from receipt (Clerk B) and from approval/payment (Manager). A matrix like this is how
SoD is designed into system roles; the control fails the moment one person can both **raise and approve** or
**receive and pay** (cross-ref 11.3.2).

### 11.3.3 The audit trail and process mining

**The principle.** Every transaction leaves an **audit trail** — the record of who did what, when, and with
what authorisation (Domain 1's postings, the PO/receipt/invoice chain, the approvals). A complete, tamper-
evident audit trail is what makes controls *verifiable* after the fact. **Process mining** — analysing the
event logs of the ERP to reconstruct how transactions *actually* flowed — reveals control breaches (matches
skipped, approvals bypassed, SoD violated) and bottlenecks at scale, and is one of the strongest AI-adjacent
techniques in this area.

**AI in this KA.** AI is well-suited to the control environment (Domain 13, KAs 13.4–13.5): **process mining**
to find control breaches and bottlenecks; **invoice/PO matching** and exception classification to automate the
three-way match; **anomaly and duplicate detection** across postings; and continuous-controls monitoring. The
professional owns the control design and the response to exceptions — an automated match that "auto-approves"
within too-wide a tolerance re-creates the very risk the control exists to prevent. **AI proposes, the
professional disposes.**

### Key terms — KA 11.3

| Term | Meaning |
|---|---|
| **Internal control** | Policies/procedures giving assurance over reporting, operations, compliance. |
| **Preventive / detective control** | Stops an error occurring / detects it afterwards. |
| **Segregation of duties (SoD)** | No single person controls a whole transaction. |
| **Audit trail** | The verifiable record of who did what, when, with what authorisation. |
| **Process mining** | Reconstructing actual process flows from ERP event logs. |

### Sample MCQs — KA 11.3

**MCQ 11.3-A `[11.3.2 · Analysis]`** Allowing one clerk to raise a PO, confirm receipt and approve the invoice
for payment violates:
- A. The 100 % rule.
- B. Segregation of duties. ✅
- C. IFRS 15.
- D. The three-point estimate.

*Rationale:* One person controlling raise/receive/approve is a segregation-of-duties failure enabling
undetected fraud. The other options are unrelated concepts.

**MCQ 11.3-B `[11.3.1 · Recall]`** A three-way match is an example of a ____ control; a monthly reconciliation
is a ____ control.
- A. detective; preventive
- B. preventive; detective ✅
- C. preventive; preventive
- D. detective; detective

*Rationale:* The match *prevents* a bad payment before it happens (preventive); a reconciliation *detects*
errors after the fact (detective).

### Self-check — KA 11.3

1. State the segregation-of-duties principle and one P2P example. *(No one person controls a whole
   transaction; e.g. separate raise/approve/receive/pay.)*
2. What does process mining reveal? *(How transactions actually flowed — control breaches, skipped matches,
   bypassed approvals, bottlenecks.)*

---

## Domain 11 summary

Money flows in through **order-to-cash** (order → credit → fulfil → invoice → collect → apply cash) and out
through **procure-to-pay** (requisition → PO → receipt → invoice → three-way match → pay), and these cycles are
the operational engines behind the revenue/receivables (Domain 2) and cost/commitments/payables (Domains 1, 5)
a controls professional relies on. Their integrity rests on **internal controls** — preventive (the three-way
match, credit limits) and detective (reconciliations, exception reports) — and on **segregation of duties**, so
no one person controls a whole transaction. The **audit trail** makes the controls verifiable, and **process
mining** reconstructs how transactions actually flowed to surface breaches at scale. Understanding these cycles
tells a controls professional where their numbers come from and where they can fail.

**Cross-references.** Postings and the ledger → 1.1; accruals and cut-off → 1.3.5; revenue/receivables → 2.2,
2.3; the commitment→accrual→actual cost cycle → 5.2; interim valuations and billing → 7.4; cash-flow and the
funding trough → 3.5; process mining and matching AI → 13.4–13.5.

*Domain 11 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
