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

**MCQ 11.1-C `[11.1.3 · Application]`** A business with annual revenue of USD 18,250,000 (USD 50,000 a day)
reduces its days sales outstanding (DSO) from 60 to 46 days through billing and dunning discipline. The cash
freed is approximately:
- A. USD 50,000
- B. USD 700,000 ✅
- C. USD 2,300,000
- D. USD 3,000,000

*Rationale:* `Cash freed ≈ DSO reduction × daily revenue = (60 − 46) × 50,000 = 14 × 50,000 = 700,000`. A is
one day's revenue; C values the whole remaining receivables book (46 days), not the reduction; D values the
old book (60 days).

**MCQ 11.1-D `[11.1.2 · Recall]`** Which O2C control also gates step 1 of the IFRS 15 revenue model?
- A. Cash application.
- B. Credit control — the collectability assessment. ✅
- C. The dunning cadence.
- D. The three-way match.

*Rationale:* Credit control assesses whether collection is probable — the same collectability test that must
pass before an IFRS 15 contract exists (Domain 2, KA 2.2.2). Cash application and dunning act after
invoicing; the three-way match is a P2P control.

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

**MCQ 11.2-C `[11.2.2 · Application]`** A PO orders 200 units at USD 30 (USD 6,000). The goods-receipt note
records 190 units received; the supplier invoices 200 units at USD 30 (USD 6,000). The amount properly
payable once the exception is resolved is:
- A. USD 6,000
- B. USD 5,700 ✅
- C. USD 300
- D. Nil — the whole invoice is rejected permanently.

*Rationale:* The match fails on quantity (invoice 200 vs receipt 190); the entity pays for goods received at
the agreed price: `190 × 30 = 5,700`. A pays for 10 undelivered units; C is the USD 300 difference, not the
payable; D confuses holding an exception with permanent rejection.

**MCQ 11.2-D `[11.2.3 · Analysis]`** A large goods-received-not-invoiced (GRNI) balance at period end
primarily drives:
- A. The commitment figure.
- B. The accrual that makes cost-to-date true. ✅
- C. The actual cost, since invoices will follow.
- D. A receivable from the supplier.

*Rationale:* Goods received but not yet invoiced represent work/goods consumed without an invoice — the
accrual basis of true cost-to-date (Domain 5, KA 5.2.2). Commitments arise at PO placement; actuals arise on
the matched invoice; GRNI is owed *to* the supplier, not by them.

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

**MCQ 11.3-C `[11.3.3 · Application]`** A process-mining pass over the ERP event log shows a number of
invoices were paid without the three-way match step ever occurring. This finding is best described as:
- A. A preventive control stopping the payments.
- B. Detective use of the audit trail, revealing that a control was bypassed. ✅
- C. Conclusive proof of fraud.
- D. A reason to disable the match, since payments went through anyway.

*Rationale:* Process mining reconstructs how transactions *actually* flowed from the logged audit trail — a
detective technique that surfaces skipped matches and bypassed approvals. The payments already happened, so
nothing was prevented (A); a bypass is the *condition* for fraud, not proof of it (C); D abandons the control
the finding shows is needed.

**MCQ 11.3-D `[11.3.2 · Recall]`** Under segregation of duties in the O2C cycle, the person who bills
customers should not also:
- A. Receive and apply the cash and write off debts. ✅
- B. Prepare the monthly cost report.
- C. Raise purchase requisitions.
- D. Maintain the schedule baseline.

*Rationale:* Billing combined with cash application and write-off lets one person control a whole O2C
transaction — receipts could be misapplied and the gap written off undetected. The other duties sit in
different processes and create no O2C conflict.

### Self-check — KA 11.3

1. State the segregation-of-duties principle and one P2P example. *(No one person controls a whole
   transaction; e.g. separate raise/approve/receive/pay.)*
2. What does process mining reveal? *(How transactions actually flowed — control breaches, skipped matches,
   bypassed approvals, bottlenecks.)*

---

## Case study — Domain 11: hardening the cycles at a scale-up (technology)

### Background

A fast-growing technology company delivers **hardware-plus-installation projects** — networking and
edge-computing equipment supplied, configured and installed at customer sites under fixed-price contracts. The
business has roughly tripled in three years, and its processes have not kept up: purchasing grew out of an
engineering team that "just ordered what it needed", billing grew out of a founder's spreadsheet, and the ERP
was implemented quickly with default roles. Nothing is on fire — but the finance director knows the symptoms:
payment runs are chaotic, receivables are drifting out, and nobody can say with confidence that every invoice
paid was for goods actually ordered and received.

A controls-led review is commissioned. Its brief is exactly this domain: harden the **procure-to-pay** cycle
(KA 11.2), harden **order-to-cash** (KA 11.1), and fix the **control environment** — segregation of duties and
the audit trail (KA 11.3). The review is deliberately unglamorous. It buys no new revenue and launches no
product. What it produces instead is measurable: fewer exceptions, closed control breaches, recovered cash, and
working capital released. This case study follows the numbers through.

### P2P: from manual matching to governed automation (KA 11.2)

The company processes **2,400 supplier invoices a month** — components, freight, subcontracted installation
labour. Before the review, the three-way match (11.2.2) is performed **manually**: an accounts-payable clerk
compares each invoice to its purchase order and goods-receipt note by eye. The first-pass match rate is
**78 %** — meaning **22 %** of invoices fail on first attempt, usually for trivial reasons: a freight charge
not on the PO, a unit price a few cents adrift, a receipt posted a day late. That is **528 exceptions a month**
clogging the payment runs, each needing a human to chase, and each delaying a supplier who has done nothing
wrong.

The review does two things, in the right order. **First**, it agrees **tolerance rules** with the finance
director: price variances within 1 % or USD 25 (whichever is lower), and quantity variances within 2 %, may
pass automatically; everything outside tolerance is a genuine exception. **Second**, it deploys an
**AI-assisted matcher** that reads invoices, matches them to POs and receipts, applies the agreed tolerances,
and classifies the residual exceptions by cause — operating *within governed tolerances*, exactly the
honest-automation pattern of Domain 13 (KA 13.5.4). The first-pass match rate rises to **92 %**.

**Worked example CS11-1 — the exception workload, before and after.**

1. **Setup.** Invoice volume **2,400 a month**. First-pass match rate **78 %** before hardening; **92 %**
   after tolerance rules and AI-assisted matching.
2. **Formula.** `monthly exceptions = volume × (1 − first-pass match rate)`.
3. **Substitution.** Before: `2,400 × 22 % = 528` exceptions a month. After: `2,400 × 8 % = 192` exceptions a
   month.
4. **Result.** **528 → 192** exceptions a month — **336 fewer**, a **64 %** reduction in exception workload.
5. **Interpretation.** The freed effort is not headcount removed; it is **redirected to the exceptions that
   matter**. The 192 that remain are disproportionately the real ones — unauthorised price rises, short
   deliveries, suspect invoices — and each now gets proper investigation instead of a hurried glance in a
   backlog of 528.

The caution belongs in the same breath as the benefit. The tolerances are the control's **design parameters**:
set them too wide and the match auto-approves the very discrepancies it exists to catch — a 5 % price
tolerance on high-volume components would wave through exactly the USD 50-vs-USD 55 exception of worked
example 11.2.2. An automated match with too-wide tolerances **re-creates the risk the control exists to
prevent** (11.3, KA 13.6). The professional owns the tolerance decision; the machine only applies it.

### The SoD breach process mining found (KA 11.3)

Hardening the match improves the **preventive** side. The review also runs a **detective** pass: a
**process-mining** analysis over the ERP's event log (11.3.3), reconstructing how the last year of P2P
transactions *actually* flowed — not how the procedure manual says they flow.

Most of the picture is reassuring. One finding is not: **14 purchase orders**, totalling **USD 86,000**, were
**raised and approved by the same user**. The ERP's default roles, never tightened since implementation, let a
senior buyer both create a PO and approve it — and under deadline pressure, fourteen times, they did. This is
a **segregation-of-duties breach** (11.3.2): not necessarily fraud, but the *condition* for it. One person
controlled the commit-the-company step end-to-end, with no second check.

The response follows the domain's playbook. The **system roles are corrected** using an SoD matrix of the
11.3.2b form — raise, approve, receive and pay mapped to different roles, with the raise-and-approve conflict
made impossible in the system rather than merely discouraged in the manual. The **14 POs are audited**
individually: two contain errors (a duplicated line item and a wrong cost code, both corrected), and **no
fraud** is found. And a **continuous-controls monitor** is left running — a standing query over the event log
that flags any future same-user raise-and-approve within a day, not a year.

The lesson the credential wants drawn: the **audit trail made the breach findable** — every raise and approval
was logged with user and timestamp, which is why the breach could be reconstructed at all (11.3.3). **Process
mining made finding it cheap** — one analytical pass over the log did what a manual sample-based audit might
have missed entirely, because 14 POs in a year of thousands is exactly the kind of needle sampling does not
reliably hit.

### Duplicate payments (KA 11.2/11.3)

The same detective pass surfaces a second, smaller finding: **3 duplicate payments a quarter**, averaging
**USD 5,200 each** — suppliers paid twice, typically when an emailed invoice copy was keyed in alongside the
original. That is `3 × 5,200 = 15,600` — **USD 15,600 a quarter** now recovered from suppliers and, once the
hardened match checks invoice references against payment history, prevented at source.

The numbers are small against a USD 14.6 million business, and honest reporting says so. But they are
**recurring** — roughly USD 62,000 a year left uncorrected — and they are **symptomatic**: duplicates slip
through where matching is weak and references are not checked, and where duplicates pass, worse things can
pass. A duplicate-detection routine is a classic detective control (11.3.1) and one of the cheapest AI-adjacent
wins in the cycle.

### O2C: collecting what was earned (KA 11.1)

The review then turns to the inflow side. Annual revenue is **USD 14,600,000** — approximately
**USD 40,000 a day** (`14,600,000 ÷ 365 = 40,000`). Receivables are drifting: **days sales outstanding (DSO)**
stands at **61 days**, and the ageing report shows the causes are process, not customers. Invoices go out late
and occasionally wrong (an installation milestone billed before it was certified, a hardware line at a
superseded price), disputes sit unchased, receipts sit **unapplied** so customers are dunned for invoices they
have already paid.

The fixes are the KA 11.1.2 controls, applied with discipline: **billing hygiene** — the right amount, in the
right period, tied to certified milestones (cross-ref 1.3.5); a **dunning cadence** — a standard escalation
timetable from reminder to hold, run every week without exception; and prompt **cash application**, so the
ageing report tells the truth and effort chases genuinely overdue balances. Within three quarters, **DSO falls
from 61 to 48 days**.

**Worked example CS11-2 — cash freed by DSO reduction.**

1. **Setup.** Annual revenue **USD 14,600,000**, so daily revenue `14,600,000 ÷ 365 = USD 40,000`. DSO falls
   from **61 days** to **48 days**.
2. **Formula.** `cash freed ≈ DSO reduction (days) × daily revenue`.
3. **Substitution.** `(61 − 48) × 40,000 = 13 × 40,000 = 520,000`.
4. **Result.** ≈ **USD 520,000** of cash permanently freed — receivables now carry thirteen fewer days of
   revenue than before.
5. **Interpretation.** This is **working capital released by process discipline alone** — no new revenue, no
   price rise, no financing. Half a million dollars that was sitting in other people's accounts is now in the
   company's, available to fund the next project's trough (cross-ref Domain 3, KA 3.5 — the funding trough
   shallows because cash arrives sooner).

For a scale-up funding its own growth, this is the headline number. It is also the domain's point in
miniature: O2C is not "sales admin" — it is the operational engine of the cash-inflow curve, and tightening it
moves cash the way no forecasting exercise can.

### The controls scorecard

The review closes with a one-page scorecard — the domain's cycles and controls, before and after:

| Measure | Before | After |
|---|---|---|
| First-pass three-way match rate | 78 % | 92 % |
| Match exceptions a month | 528 | 192 |
| SoD breaches open (same-user raise + approve) | 14 POs (USD 86,000) | 0 |
| Duplicate payments a quarter | 3 (≈ USD 15,600) | ~0 |
| DSO | 61 days | 48 days |
| Working capital freed | — | ≈ USD 520,000 |

Every line traces to a control in this domain: the match and its tolerances, the SoD matrix, the audit trail
mined at scale, and the O2C disciplines of billing, dunning and cash application.

### What the credential expects

A candidate reading this case should be able to name where each result came from. The **P2P cycle and the
three-way match** (KA 11.2) produced the exception reduction — and the crucial point is *what changed*: agreed
tolerances plus assisted matching, not the abolition of the control. **Segregation of duties and the audit
trail** (KA 11.3) produced the breach finding — SoD defines the conflict (raise versus approve), the audit
trail records who actually did what, and the 11.3.2b matrix is how the fix is designed into system roles.
**Process mining as a detective control at scale** (11.3.3) is what made a year of transactions auditable in
one pass — 14 breaching POs and 3-a-quarter duplicates are precisely the low-frequency patterns that
whole-population analysis catches and sampling misses. **O2C and receivables as cash** (KA 11.1) produced the
biggest number: DSO is days of revenue trapped in receivables, and thirteen days at USD 40,000 a day is
USD 520,000.

And running through all of it, the honest-automation caveat (KAs 13.5.4, 13.6): the AI matcher works *because*
the tolerances were set by a professional who understood what the match protects, and *because* every residual
exception lands with a human who owns the response. Set the tolerances wide to make the exception count look
good, and the 92 % becomes a vanity metric hiding unauthorised cost. The machine matches, mines and flags at a
scale no clerk can; the professional decides what tolerable means, what an exception costs, and what happens
next. **AI proposes, the professional disposes.**

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
