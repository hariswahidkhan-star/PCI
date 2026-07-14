# Domain 5 — Cost Management & Cost Control

> **Group:** Project management (Domain 5 of 8 in the PM group). **Target:** ~90 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). British English; USD (+SAR where useful); five-line
> worked examples.

## Why this domain exists

The finance group (Domains 1–4) established how cost is *recorded, reported and forecast*. This domain is
about **controlling** it on a live project: understanding the anatomy of cost (KA 5.1); running the **cost
control cycle** — commitment, accrual, actual — that gives a true cost-to-date long before invoices arrive
(KA 5.2); organising cost through the **cost breakdown structure** and **control accounts** (KA 5.3, building
on Domain 1's coding); and governing **change**, whose uncontrolled accumulation is the most common way a
baseline is quietly lost (KA 5.4). Cost control is where a controls professional most directly protects the
outcome: not by counting what has been spent, but by seeing what is *committed* and what is *coming* in time
to influence it.

**Learning objectives.** After this domain a candidate can: classify costs (direct/indirect, fixed/variable)
and compute overhead absorption and over/under-recovery; operate the commitment → accrual → actual cost
cycle and explain why committed cost matters to control; structure cost through a CBS and control accounts;
and run a disciplined change-control process, assessing the cost impact of trends, variations and change
orders.

---

## Knowledge Area 5.1 — The cost management framework

*Topics: 5.1.1 cost classifications · 5.1.2 cost drivers · 5.1.3 allocation, absorption and overhead
recovery.*

### 5.1.1 Cost classifications

**Definition & purpose.** Cost is classified along two independent axes that every controls professional
uses constantly:

- **Direct vs indirect** — a **direct** cost is traceable to a specific cost object (a work package's
  materials, the labour on it); an **indirect** cost (overhead) supports many objects and cannot be traced to
  one without an allocation rule (site management, insurance, head-office support).
- **Fixed vs variable** — a **fixed** cost does not change with activity volume in the short run (site
  establishment, supervision); a **variable** cost changes with volume (materials, plant hours). Many costs
  are **semi-variable** (a fixed element plus a variable element).

```
Total cost = Fixed cost + (Variable cost per unit × Volume)
```

**Why it matters.** The classification drives the analysis: variance decomposition (Domain 4) separates rate
from usage; forecasting a cost requires knowing whether it scales with volume; and the direct/indirect split
determines what must be *allocated* rather than *traced* (5.1.3).

**Worked example 5.1.1 — semi-variable cost.**

1. **Setup.** A site's monthly cost is **USD 200,000 fixed** plus **USD 50 per unit** produced; the month
   produces **1,000 units**.
2. **Formula.** `Total cost = Fixed + Variable per unit × Volume`.
3. **Substitution.** `200,000 + 50 × 1,000 = 200,000 + 50,000`.
4. **Result.** **USD 250,000**; of which USD 200,000 is fixed and USD 50,000 variable.
5. **Interpretation.** If volume doubles to 2,000 units, total cost is `200,000 + 100,000 = 300,000` — a 100 %
   volume rise gives only a 20 % cost rise, because the fixed element is spread over more units. Recognising
   the fixed/variable split is what lets a controls professional forecast cost for a changed volume correctly
   (and flex the budget, 4.2.2).

### 5.1.2 Cost drivers

**Definition & purpose.** A **cost driver** is the factor that *causes* a cost to change — labour hours,
tonnes placed, metres drilled, number of RFIs. Identifying the true driver is what makes a parametric
estimate (3.2.2), a forecast, and an allocation *causal* rather than arbitrary. Allocating an overhead on a
driver that does not actually cause it (spreading site cost by headcount when it is really driven by duration)
distorts every downstream unit cost.

### 5.1.3 Allocation, absorption and overhead recovery

**Definition & purpose.** Because indirect costs cannot be traced, they are **absorbed** into cost objects
using an **overhead absorption rate (OAR)** applied to a chosen activity base:

```
Overhead absorption rate (OAR) = Budgeted overhead / Budgeted activity base
Overhead absorbed              = OAR × Actual activity
Over/(under)-absorption        = Overhead absorbed − Actual overhead incurred
```

Because the OAR uses **budgeted** figures, the overhead **absorbed** rarely equals the overhead **actually
incurred**: the difference is **over-** or **under-absorption**, adjusted in the accounts. For a controls
professional the practical points are: the OAR is a planning device, not a precise truth; the choice of
activity base matters (5.1.2); and a large under-absorption is a signal (activity below plan, or overhead
above plan) worth investigating.

**Worked example 5.1.3 — overhead absorption and under-recovery.**

1. **Setup.** Budgeted overhead **USD 600,000**; budgeted activity **30,000 labour hours**. Actual overhead
   incurred **USD 610,000**; actual activity **28,000 hours**.
2. **Formulae.** As above.
3. **Substitution.** `OAR = 600,000 / 30,000 = USD 20/hour`; `absorbed = 20 × 28,000 = 560,000`; `over/(under)
   = 560,000 − 610,000 = (50,000)`.
4. **Result.** OAR **USD 20/hour**; overhead absorbed **USD 560,000**; **under-absorption USD 50,000**.
5. **Interpretation.** Overhead was under-recovered by USD 50,000 — driven by *both* lower activity (28,000
   vs 30,000 hours → USD 40,000 of the shortfall at USD 20/hr) *and* higher spend (USD 610k vs 600k → USD
   10,000). A controls professional splits and explains that shortfall exactly as a variance (Domain 4),
   rather than reporting a single under-recovery figure.

**Worked example 5.1.3b — the activity base changes the absorption.**

1. **Setup.** Budgeted overhead **USD 600,000**. Base A = **30,000 labour hours**; Base B = **20,000 machine
   hours**. A job uses **500 labour hours** and **200 machine hours**.
2. **Formula.** `OAR = budgeted overhead / base`; `absorbed = OAR × the job's activity on that base`.
3. **Substitution.** Base A: `OAR = 600,000 / 30,000 = USD 20/labour-hour` → job absorbs `20 × 500 = 10,000`.
   Base B: `OAR = 600,000 / 20,000 = USD 30/machine-hour` → job absorbs `30 × 200 = 6,000`.
4. **Result.** The **same job** absorbs **USD 10,000** under a labour-hour base but **USD 6,000** under a
   machine-hour base.
5. **Interpretation.** The choice of activity base materially changes the cost loaded onto a job. The base
   should reflect the true cost driver (5.1.2); a base that does not drive the cost mis-allocates it into
   every unit cost.

### Key terms — KA 5.1

| Term | Meaning |
|---|---|
| **Direct / indirect cost** | Traceable to one cost object / supporting many (overhead). |
| **Fixed / variable cost** | Independent of / changing with activity volume. |
| **Cost driver** | The factor that causes a cost to change. |
| **Overhead absorption rate (OAR)** | Budgeted overhead ÷ budgeted activity base. |
| **Over/under-absorption** | Overhead absorbed minus overhead actually incurred. |

### Sample MCQs — KA 5.1

**MCQ 5.1-A `[5.1.3 · Application]`** Budgeted overhead USD 600,000 over 30,000 hours; actual 28,000 hours;
actual overhead USD 610,000. The overhead absorbed is:
- A. USD 600,000
- B. USD 560,000 ✅
- C. USD 610,000
- D. USD 20

*Rationale:* `OAR = 600,000/30,000 = 20/hr`; `absorbed = 20 × 28,000 = 560,000`. A is budgeted overhead; C is
actual incurred; D is the rate.

**MCQ 5.1-B `[5.1.1 · Application]`** A cost is USD 200,000 fixed plus USD 50/unit. At 2,000 units the total
is:
- A. USD 100,000
- B. USD 250,000
- C. USD 300,000 ✅
- D. USD 400,000

*Rationale:* `200,000 + 50 × 2,000 = 300,000`. A omits fixed cost; B uses 1,000 units; D double-counts fixed.

### Self-check — KA 5.1

1. Give the two cost-classification axes and one use of each. *(Direct/indirect — what must be allocated;
   fixed/variable — how cost scales with volume / flexing the budget.)*
2. Why does the choice of activity base for overhead absorption matter? *(An OAR on a base that does not drive
   the cost mis-allocates it into every unit cost.)*

---

## Knowledge Area 5.2 — The cost control cycle

*Topics: 5.2.1 commitment → accrual → actual · 5.2.2 the cost ledger and cost-to-date · 5.2.3 cost extraction
and reconciliation · 5.2.4 data integrity.*

### 5.2.1 Commitment → accrual → actual

**Definition & purpose.** Cost passes through three states, and control depends on tracking **all three**, not
just the last:

1. **Commitment** — when a purchase order or subcontract is *raised*, the organisation is committed to the
   cost even though nothing has been received or paid. Committed cost is the earliest signal of future spend.
2. **Accrual** — when goods/services are *received* but not yet invoiced, the cost is accrued (Domain 1, KA
   1.3) so cost-to-date reflects work actually done.
3. **Actual** — when the invoice is received and processed, the cost becomes an actual in the ledger.

A cost engineer who watches only *actuals* is always looking at the past; watching **commitments** is what
gives lead time to act before the money is spent.

```
Cost-to-date (for control) = Actuals + Accruals
Forecast committed cost     = Cost-to-date + Open commitments (yet to be received) + Estimate for uncommitted scope
```

### 5.2.2 The cost ledger and cost-to-date

**The principle.** The **cost ledger** (the project's cost record, reconciled to the general ledger) holds
cost by control account and state. The controls **cost-to-date** must include **accruals** for received-but-
uninvoiced work — otherwise `AC` (actual cost) is understated and `CPI` flattered (the exact link made in
Domain 1, KA 1.3.5, and used in Domain 6). A disciplined month-end accrual process is therefore not
"accounting housekeeping" — it is what makes the earned-value cost figure true.

**Worked example 5.2.2 — a true cost-to-date.**

1. **Setup.** A control account: invoices processed (actuals) **USD 300,000**; goods received not yet invoiced
   **USD 40,000**; open purchase orders not yet received **USD 120,000**; uncommitted remaining scope
   estimated **USD 90,000**.
2. **Formulae.** As 5.2.1.
3. **Substitution.** `Cost-to-date = 300,000 + 40,000 = 340,000`; `forecast committed + remaining = 340,000 +
   120,000 + 90,000 = 550,000`.
4. **Result.** Controls **cost-to-date USD 340,000** (not the 300,000 in the invoice ledger); **forecast cost
   USD 550,000**.
5. **Interpretation.** Reporting only the USD 300,000 of invoices understates cost by USD 40,000 and ignores
   USD 120,000 already committed — a USD 160,000 blind spot. The commitment/accrual view turns that blind spot
   into a forecast the project can act on.

**Worked example 5.2.2b — a control account's cost states.**

1. **Setup.** A control account with budget **USD 400,000**. To date: purchase orders raised (commitments)
   **USD 250,000**; of these, invoices processed (actuals) **USD 90,000** and goods received but not invoiced
   (accruals) **USD 30,000**.
2. **Formulae.** `Open commitment = commitments − actuals − accruals`; `cost-to-date = actuals + accruals`;
   `forecast = cost-to-date + open commitment + uncommitted remaining scope`.
3. **Substitution.** Open commitment `= 250,000 − 90,000 − 30,000 = 130,000`; cost-to-date `= 90,000 + 30,000
   = 120,000`; uncommitted `= 400,000 − 250,000 = 150,000`; forecast `= 120,000 + 130,000 + 150,000 =
   400,000`.
4. **Result.** Cost-to-date **USD 120,000** (not the 90,000 of invoices), open commitment **USD 130,000**,
   forecast **USD 400,000**.
5. **Interpretation.** Reporting only the USD 90,000 of invoices hides USD 30,000 of accrued cost and USD
   130,000 already committed — a USD 160,000 blind spot the commitment/accrual view turns into a forecast.

### 5.2.3 Cost extraction and reconciliation

**The principle.** Cost is **extracted** from the ERP/source systems, **coded** to the project structure (if
not already), and **reconciled** back to the general ledger so the controls report and the statutory accounts
agree (Domain 1, KA 1.5.2; Domain 2, KA 2.5). Reconciliation is where mis-codes, duplicates and timing
differences surface — and where a trial balance's blind spot (1.1.4) is covered by tying to independent
sources. A recurring, automated-where-possible reconciliation is the backbone of trustworthy cost control.

### 5.2.4 Data integrity

**The principle.** Every figure above is only as good as the data behind it: cost coded correctly at source
(1.5), commitments raised and closed promptly, accruals complete, and one agreed source of truth. **Garbage
in, garbage out** applies with force to cost control — and with even more force to the AI applied to it
(Domain 13, KA 13.2). Data integrity is not a one-off clean-up but an ongoing control: stale open commitments,
un-reconciled feeds and duplicate codes each quietly corrupt the forecast.

**AI in this KA.** Cost extraction, coding and reconciliation are among the highest-value, lowest-risk AI
applications in project controls (Domain 1, KA 1.5; Domain 13, KA 13.5): models can auto-code cost from
invoice/PO narratives, match extracted cost to the ledger and flag exceptions, detect duplicate or anomalous
postings, and propose month-end accruals from goods-received-not-invoiced data. The professional owns the
mapping rules, the exceptions and the accrual judgements — an auto-accrual from a document date rather than a
service date reproduces a real cut-off error at scale. **AI proposes, the professional disposes.**

### Key terms — KA 5.2

| Term | Meaning |
|---|---|
| **Commitment** | Cost the organisation is bound to once a PO/subcontract is raised. |
| **Accrual** | Received-but-uninvoiced cost recognised so cost-to-date is true. |
| **Actual** | Cost booked once the invoice is processed. |
| **Cost-to-date (control)** | Actuals + accruals — the figure `AC` should reflect. |
| **Reconciliation** | Tying extracted/controls cost back to the general ledger. |
| **Data integrity** | Ongoing correctness of the cost data (coding, commitments, accruals, one source). |

### Sample MCQs — KA 5.2

**MCQ 5.2-A `[5.2.2 · Application]`** Actuals USD 300,000; goods received not invoiced USD 40,000; open POs
USD 120,000. The controls **cost-to-date** is:
- A. USD 300,000
- B. USD 340,000 ✅
- C. USD 420,000
- D. USD 460,000

*Rationale:* Cost-to-date = actuals + accruals = `300,000 + 40,000 = 340,000`. Open POs (120,000) are
committed but **not yet received**, so they are in the forecast, not cost-to-date. A omits the accrual; C and
D add commitments that have not been received.

**MCQ 5.2-B `[5.2.1 · Analysis]`** Why does watching *commitments* improve cost control over watching actuals
alone?
- A. Commitments are always smaller.
- B. Commitments give lead time — they signal future spend before it is received or paid. ✅
- C. Actuals are not recorded in the ledger.
- D. Commitments replace the need for a forecast.

*Rationale:* A commitment is the earliest state, so it warns of spend before goods are received or invoiced.
It is not necessarily smaller, actuals are recorded, and commitments inform (not replace) the forecast.

**MCQ 5.2-C `[5.2.2 · Analysis]`** Reporting only processed invoices as cost-to-date will:
- A. Overstate cost and understate CPI.
- B. Understate cost (by omitting accruals), flattering CPI. ✅
- C. Have no effect on CPI.
- D. Overstate earned value.

*Rationale:* Omitting accruals understates `AC`; since `CPI = EV/AC`, a lower `AC` inflates `CPI`. It does not
overstate cost or affect `EV`.

### Self-check — KA 5.2

1. Name the three states of cost and which two make up controls cost-to-date. *(Commitment, accrual, actual;
   cost-to-date = actuals + accruals.)*
2. What is the earned-value consequence of omitting accruals from cost-to-date? *(`AC` understated → `CPI`
   flattered → forecast corrupted.)*

---

## Knowledge Area 5.3 — Cost breakdown and control accounts

*Topics: 5.3.1 the cost breakdown structure · 5.3.2 control accounts and work packages · 5.3.3 the integration
point for earned value.*

### 5.3.1 The cost breakdown structure

**Definition & purpose.** The **cost breakdown structure (CBS)** decomposes cost by **element/type** (labour,
materials, plant, subcontract, overhead), giving the "by cost type" view of a project (Domain 1, KA 1.5.3).
Crossed with the WBS's "by scope" view, it is what lets the same postings answer both "how much on
foundations?" and "how much subcontract across the project?".

### 5.3.2 Control accounts and work packages

**Definition & purpose.** A **control account (CA)** is the management-control point where scope, budget, cost
and schedule integrate — the **WBS × OBS** intersection (Domain 1, KA 1.5.4). Beneath it sit **work packages**
(defined, schedulable, costable units of work) and, further down, planning packages (future work not yet
detailed). The control account is deliberately the level at which **earned value is measured and performance
managed**: high enough to be manageable in number, low enough to be meaningful. Choosing the control-account
level well is a real skill — too granular and the overhead of measurement swamps the value; too coarse and
problems hide inside a big account.

### 5.3.3 The integration point for earned value

**The principle.** The control account is the hinge between this domain and Domain 6: each CA has a
time-phased budget (its share of `PV`, Domain 3), earns value as its work packages complete (`EV`), and
accrues actual cost (`AC`, KA 5.2). Cost control at the control-account level *is* the data layer earned value
sits on; if the CAs are well-defined, coded (1.5) and their cost states tracked (5.2), earned value is
trustworthy — if not, no amount of EVM formula rigour rescues it.

> **Fig 5.3.1 — Control account as the integration point.** *Caption:* where scope, budget, cost and schedule
> meet. *Underlying data:* one control account CA-Civils-Foundations with budget (PV share), work packages,
> accrued actuals and schedule status. *Render-ready description:* a central brand-blue node "Control account"
> with four inputs converging — "Scope (WBS)", "Budget (PV)", "Actual cost (AC)", "Schedule (SPI)" — and one
> output "Earned value performance (CPI/SPI)". *Animation storyboard (digital-only):* the four inputs flow
> into the node in turn; the node then emits the CPI/SPI outputs, previewing Domain 6.

### Key terms — KA 5.3

| Term | Meaning |
|---|---|
| **Cost breakdown structure (CBS)** | Decomposition of cost by element/type. |
| **Control account (CA)** | WBS×OBS integration point; the level EV is measured at. |
| **Work package** | A defined, schedulable, costable unit of work beneath a CA. |
| **Planning package** | Future work within a CA not yet detailed into work packages. |

### Sample MCQs — KA 5.3

**MCQ 5.3-A `[5.3.2 · Analysis]`** Control accounts are set far too granular (hundreds of tiny accounts). The
main consequence is:
- A. Earned value cannot be computed at all.
- B. The measurement overhead swamps the value, without improving control. ✅
- C. The BAC changes.
- D. Cost coding becomes unnecessary.

*Rationale:* Over-granular CAs create measurement burden disproportionate to insight. EV can still be
computed; `BAC` is unaffected; coding is still needed.

**MCQ 5.3-B `[5.3.3 · Recall]`** At what level is earned value normally measured and managed?
- A. The whole project only.
- B. The individual invoice.
- C. The control account. ✅
- D. The company.

*Rationale:* The control account is the designed level for EV measurement and performance management —
manageable in number, meaningful in scope.

### Self-check — KA 5.3

1. What are the two "views" of cost that the CBS and WBS provide? *(By cost type / by scope.)*
2. Why is the control account the natural home for earned value? *(It integrates scope, budget, cost and
   schedule at a manageable, meaningful level.)*

---

## Knowledge Area 5.4 — Change control and cost impact

*Topics: 5.4.1 why change control matters · 5.4.2 trends, variations and change orders · 5.4.3 assessing cost
impact and protecting the baseline.*

### 5.4.1 Why change control matters

**The principle.** A baseline (Domain 3, KA 3.1.3) is only meaningful if it changes **only through control**.
Uncontrolled change — **scope creep** — is the most common way a baseline is quietly lost: a dozen small,
unmanaged additions accumulate until actual no longer relates to plan and variance becomes meaningless.
Change control is the disciplined process by which every proposed change is **identified, assessed
(cost/schedule/risk impact), approved or rejected, and — if approved — baselined**. Its purpose is not to
prevent change but to ensure change is *visible, costed and authorised*.

### 5.4.2 Trends, variations and change orders

**Definition & purpose.**

- A **trend** is an *early warning* of a potential change — a signal that cost or scope may move before it is
  formalised. Logging trends is what gives a project lead time (the cost-control analogue of a leading
  indicator, 4.1.2).
- A **variation / change order** is a *formal* change to the contract scope or price, raised, priced and
  agreed (the commercial mechanics are in Domain 7, KA 7.2).
- The accounting consequence of an agreed variation flows into revenue via the IFRS 15 contract-modification
  rules (Domain 2, KA 2.2.8) and into the cost baseline via re-baselining.

### 5.4.3 Assessing cost impact and protecting the baseline

**The principle.** Every change is assessed for its **full** impact — direct cost, knock-on cost (disruption,
acceleration), schedule effect, and risk — before approval, not after. Approved changes update the baseline
(and the forecast); rejected changes are recorded and closed. The controls professional maintains the
**change log** that reconciles the current baseline to the original, so at any point the project can answer
"how has the baseline moved, by how much, and why?" — the same auditable-movement discipline as a basis of
estimate (3.2.3) or a provision (1.4.6).

**Worked example 5.4.3 — a change's impact on the baseline and forecast.**

1. **Setup.** Original `BAC` **USD 9,700,000** (Domain 3, KA 3.1.4). An approved variation adds scope costing
   **USD 300,000**, funded from the **management reserve**; a separate identified risk materialises, drawing
   **USD 150,000** of the **contingency reserve**.
2. **Reasoning.** The variation is a **baseline change** (new scope) → `BAC` increases. The risk draw is
   **within** the contingency already inside the baseline → it is consumed, not added to `BAC`.
3. **Result.** New `BAC = 9,700,000 + 300,000 = 10,000,000`; contingency remaining `= 700,000 − 150,000 =
   550,000`; management reserve remaining `= 500,000 − 300,000 = 200,000`.
4. **Interpretation.** The change log shows the `BAC` moved from 9.70m to 10.00m for one authorised reason
   (the variation), while the risk draw is a *use* of existing baseline contingency, not a growth in scope.
   Keeping those two mechanisms distinct is exactly the discipline that stops a baseline drifting
   untraceably.
5. **Cross-check.** Total authorised budget was USD 10,200,000 (3.1.4); after the change it is `new BAC
   10,000,000 + remaining management reserve 200,000 = 10,200,000` — unchanged, because the variation moved
   money *from* reserve *into* the baseline rather than adding new funding. ✓

**AI in this KA.** AI can support change control by scanning correspondence, RFIs and site data to **surface
trends early** (a leading-indicator engine for change), estimating the likely cost impact of a proposed change
from historical analogues, and keeping the change log reconciled. The professional owns the impact assessment
and the approval — a model can flag a possible change but cannot weigh its full disruption/acceleration cost or
authorise it. **AI proposes, the professional disposes.**

### Key terms — KA 5.4

| Term | Meaning |
|---|---|
| **Change control** | The process to identify, assess, approve/reject and baseline change. |
| **Scope creep** | Uncontrolled accumulation of unmanaged change. |
| **Trend** | An early warning of a potential change. |
| **Variation / change order** | A formal, priced, agreed change to contract scope/price. |
| **Change log** | The record reconciling current baseline to original, by change. |

### Sample MCQs — KA 5.4

**MCQ 5.4-A `[5.4.3 · Application]`** `BAC` is USD 9,700,000. An approved new-scope variation of USD 300,000
(from management reserve) is baselined, and USD 150,000 of contingency is drawn for a materialised risk. The
new `BAC` is:
- A. USD 9,550,000
- B. USD 10,000,000 ✅
- C. USD 10,150,000
- D. USD 9,700,000

*Rationale:* The variation adds new scope to the baseline (`+300,000 → 10,000,000`); the contingency draw is a
*use* of reserve already inside the baseline and does not change `BAC`. A subtracts the risk draw; C adds it;
D ignores the variation.

**MCQ 5.4-B `[5.4.2 · Recall]`** A "trend" in cost control is best described as:
- A. A formal, agreed change order.
- B. An early warning of a potential change, logged for lead time. ✅
- C. A completed variance.
- D. A change to the risk appetite.

*Rationale:* A trend is an early signal of possible change — a leading indicator. A formal change order (A)
comes later; a variance (C) is a realised result; risk appetite (D) is unrelated.

**MCQ 5.4-C `[5.4.1 · Analysis]`** The primary purpose of change control is to:
- A. Prevent all change.
- B. Ensure every change is visible, costed and authorised before it affects the baseline. ✅
- C. Speed up the project.
- D. Replace the forecast.

*Rationale:* Change control governs change so it is transparent, assessed and approved — not to prevent it,
accelerate delivery, or replace forecasting.

### Self-check — KA 5.4

1. Distinguish a trend from a variation. *(Trend — early warning of a possible change; variation — a formal,
   priced, agreed change.)*
2. When a materialised risk draws on contingency, does `BAC` change? Why or why not? *(No — contingency is
   already inside the baseline; drawing it consumes reserve, it does not add scope.)*

---

## Domain 5 summary

Cost control begins with the anatomy of cost — direct/indirect and fixed/variable, driven by true cost
drivers, with indirect cost absorbed via an overhead rate that rarely matches actual (over/under-absorption).
Its engine is the **commitment → accrual → actual** cycle: watching commitments for lead time and including
accruals so **cost-to-date** — and therefore `AC` and `CPI` — is true, all reconciled to the ledger with
ongoing data integrity. Cost is organised through the **CBS** and **control accounts**, the WBS×OBS
integration points where earned value is measured. And **change control** protects the baseline: logging
trends early, formalising variations, assessing full cost impact, and re-baselining only through authorised
change — keeping every movement of the `BAC` traceable. This domain is the data-and-discipline layer beneath
the earned-value formulae of Domain 6.

**Cross-references.** Accruals and cut-off → 1.3.5; cost coding and control accounts → 1.5; contract
modifications in revenue → 2.2.8; the flexed budget → 4.2.2; reserves and the baseline → 3.1; the full EVM
treatment → Domain 6; variations and commercial change → 7.2; risk and contingency → Domain 12; automated
coding/reconciliation/change detection → Domain 13, KA 13.5.

*Domain 5 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
