# Domain 5 — Cost Management & Cost Control

## Why this domain exists

The finance group (Domains 1–4) established how cost is *recorded, reported and forecast*. This domain is
about **controlling** it on a live project: understanding the anatomy of cost (KA 5.1); running the **cost
control cycle** (commitment, accrual, actual) that gives a true cost-to-date long before invoices arrive (KA
5.2); organising cost through the **cost breakdown structure** and **control accounts** (KA 5.3, building on
Domain 1's coding); and governing **change**, whose uncontrolled accumulation is the most common way a
baseline is quietly lost (KA 5.4). Cost control is where a controls professional most directly protects the
outcome: not by counting what has been spent, but by seeing what is *committed* and what is *coming* in time
to influence it.

**Learning objectives.** After this domain a candidate can: classify costs (direct/indirect, fixed/variable)
and compute overhead absorption and over/under-recovery; operate the commitment → accrual → actual cost cycle
and explain why committed cost matters to control; structure cost through a CBS and control accounts; run a
disciplined change-control process, assessing the cost impact of trends, variations and change orders; and
apply the change-authority ladder, who assesses, who approves, what evidence a change record must carry, and
when a change escalates before it is approved.

---

## Knowledge Area 5.1 — The cost management framework

*Topics: 5.1.1 cost classifications · 5.1.2 cost drivers · 5.1.3 allocation, absorption and overhead
recovery.*

### 5.1.1 Cost classifications

**Definition & purpose.** Cost is classified along two independent axes that every controls professional
uses constantly:

- **Direct vs indirect**: a **direct** cost is traceable to a specific cost object (a work package's
  materials, the labour on it); an **indirect** cost (overhead) supports many objects and cannot be traced to
  one without an allocation rule (site management, insurance, head-office support).
- **Fixed vs variable**: a **fixed** cost does not change with activity volume in the short run (site
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
5. **Interpretation.** If volume doubles to 2,000 units, total cost is `200,000 + 100,000 = 300,000`. A 100 %
   volume rise gives only a 20 % cost rise, because the fixed element is spread over more units. Recognising
   the fixed/variable split is what lets a controls professional forecast cost for a changed volume correctly
   (and flex the budget, 4.2.2).

### 5.1.2 Cost drivers

**Definition & purpose.** A **cost driver** is the factor that *causes* a cost to change: labour hours, tonnes
placed, metres drilled, number of RFIs. Identifying the true driver is what makes a parametric estimate
(3.2.2), a forecast, and an allocation *causal* rather than arbitrary. Allocating an overhead on a driver that
does not actually cause it (spreading site cost by headcount when it is really driven by duration) distorts
every downstream unit cost.

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
5. **Interpretation.** Overhead was under-recovered by USD 50,000: driven by *both* lower activity (28,000 vs
   30,000 hours → USD 40,000 of the shortfall at USD 20/hr) *and* higher spend (USD 610k vs 600k → USD
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

**AI in this KA.** Cost classification is a well-shaped machine-learning problem: given a coded history, a
model can propose the direct/indirect and fixed/variable split for new ledger lines, cluster spend to suggest
which activity genuinely drives an overhead pool (5.1.2), and flag lines whose classification looks
inconsistent with their description. The limits sit exactly where the judgement does. A model reproduces the
history it was trained on, so a pool that has always been absorbed on the wrong base will keep being absorbed
on the wrong base, confidently; it cannot know that a new contract has changed what "direct" means for this
project; and it has no view on whether an absorption rate is defensible to the client, only on what the
previous rate was. Verify by re-deriving the rate from the budgeted overhead and the budgeted base, and by
testing a sample of reclassified lines back to source. **AI proposes; the professional verifies, decides and
remains accountable.**

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

**MCQ 5.1-C `[5.1.3 · Application]`** Budgeted overhead is USD 480,000 over a budgeted 24,000 labour hours.
Actual activity is 25,000 hours and actual overhead incurred is USD 490,000. The over/(under)-absorption is:
- A. USD 10,000 over-absorbed ✅
- B. USD 10,000 under-absorbed
- C. USD 20,000 over-absorbed
- D. USD 490,000 under-absorbed

*Rationale:* `OAR = 480,000 / 24,000 = USD 20/hour`; `absorbed = 20 × 25,000 = 500,000`; `500,000 − 490,000 =
10,000` **over**-absorbed: higher-than-planned activity absorbed more than was incurred. B has the sign wrong;
C compares absorbed with *budgeted* overhead (`500,000 − 480,000`) instead of actual; D is the actual overhead
itself, not a variance.

**MCQ 5.1-D `[5.1.2 · Analysis]`** Site overhead is driven mainly by project *duration*, but is allocated to
work packages by *headcount*. The likely consequence is:
- A. None: the total overhead is unchanged, so the allocation does not matter.
- B. The trial balance will no longer balance.
- C. Labour-heavy packages carry overhead they do not cause, distorting every downstream unit cost. ✅
- D. The overhead becomes a direct cost.

*Rationale:* Allocating on a base that is not the true driver mis-loads cost onto the wrong objects: the
totals agree but each package's unit cost is distorted, misleading estimates and variances. A ignores the
distortion at package level; B confuses allocation with double-entry; D misstates the classification, which
depends on traceability, not the allocation base.

### Self-check — KA 5.1

1. Give the two cost-classification axes and one use of each. *(Direct/indirect: what must be allocated;
   fixed/variable: how cost scales with volume / flexing the budget.)*
2. Why does the choice of activity base for overhead absorption matter? *(An OAR on a base that does not drive
   the cost mis-allocates it into every unit cost.)*

---

## Knowledge Area 5.2 — The cost control cycle

*Topics: 5.2.1 commitment → accrual → actual · 5.2.2 the cost ledger and cost-to-date · 5.2.3 cost extraction
and reconciliation · 5.2.4 data integrity.*

### 5.2.1 Commitment → accrual → actual

**Definition & purpose.** Cost passes through three states, and control depends on tracking **all three**, not
just the last:

1. **Commitment**, when a purchase order or subcontract is *raised*, the organisation is committed to the cost
   even though nothing has been received or paid. Committed cost is the earliest signal of future spend.
2. **Accrual**, when goods/services are *received* but not yet invoiced, the cost is accrued (Domain 1, KA
   1.3) so cost-to-date reflects work actually done.
3. **Actual**, when the invoice is received and processed, the cost becomes an actual in the ledger.

A cost engineer who watches only *actuals* is always looking at the past; watching **commitments** is what
gives lead time to act before the money is spent.

```
Cost-to-date (for control) = Actuals + Accruals
Forecast committed cost     = Cost-to-date + Open commitments (yet to be received) + Estimate for uncommitted scope
```

### 5.2.2 The cost ledger and cost-to-date

**The principle.** The **cost ledger** (the project's cost record, reconciled to the general ledger) holds
cost by control account and state. The controls **cost-to-date** must include **accruals** for received-but-
uninvoiced work: otherwise `AC` (actual cost) is understated and `CPI` flattered (the exact link made in
Domain 1, KA 1.3.5, and used in Domain 6). A disciplined month-end accrual process is therefore not
"accounting housekeeping". It is what makes the earned-value cost figure true.

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
   USD 120,000 already committed: a USD 160,000 blind spot. The commitment/accrual view turns that blind spot
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
   130,000 already committed: a USD 160,000 blind spot the commitment/accrual view turns into a forecast.

**Value of work done (VOWD).** In oil-and-gas and EPC practice the number this section builds carries a field
name: **value of work done (VOWD)**; the cost of work *performed* to date, regardless of what has been
invoiced: `VOWD = invoiced-to-date + accrual for work done not invoiced`. The name matters because the monthly
cost report in those sectors is drawn as **three curves on one chart**: **commitment** at the top (stepping up
as purchase orders are placed), **VOWD** in the middle (smooth, following physical performance), and
**invoiced/paid** at the bottom (lagging by payment terms); the vertical gaps between the curves *are* the
accrual and the open commitment, made visible. The arithmetic is exactly Exercise 5.5's: there, VOWD `=
830,000 + 190,000 = 1,020,000`; the same actual-cost number, under its field name. One caveat: VOWD must
follow **physical** performance (quantity reports, progress certificates), not invoice arrival: a VOWD curve
that steps upward on invoice dates is just the bottom curve wearing the middle curve's label (KA 5.2.4).

> **Fig 5.2.2 — The three-curve cost report.** *Caption:* commitment steps ahead, VOWD follows performance, invoiced lags by payment terms; the vertical gaps are the open commitment and the accrual. *Data:* illustrative eight-month package to USD 1.0m.

### 5.2.3 Cost extraction and reconciliation

**The principle.** Cost is **extracted** from the ERP/source systems, **coded** to the project structure (if
not already), and **reconciled** back to the general ledger so the controls report and the statutory accounts
agree (Domain 1, KA 1.5.2; Domain 2, KA 2.5). Reconciliation is where mis-codes, duplicates and timing
differences surface, and where a trial balance's blind spot (1.1.4) is covered by tying to independent
sources. A recurring, automated-where-possible reconciliation is the backbone of trustworthy cost control.

### 5.2.4 Data integrity

**The principle.** Every figure above is only as good as the data behind it: cost coded correctly at source
(1.5), commitments raised and closed promptly, accruals complete, and one agreed source of truth. **Garbage
in, garbage out** applies with force to cost control, and with even more force to the AI applied to it (Domain
13, KA 13.2). Data integrity is not a one-off clean-up but an ongoing control: stale open commitments,
un-reconciled feeds and duplicate codes each quietly corrupt the forecast.

**AI in this KA.** Cost extraction, coding and reconciliation are among the highest-value, lowest-risk AI
applications in project controls (Domain 1, KA 1.5; Domain 13, KA 13.5): models can auto-code cost from
invoice/PO narratives, match extracted cost to the ledger and flag exceptions, detect duplicate or anomalous
postings, and propose month-end accruals from goods-received-not-invoiced data. The professional owns the
mapping rules, the exceptions and the accrual judgements: an auto-accrual from a document date rather than a
service date reproduces a real cut-off error at scale. And note what this data *is*: supplier and subcontract
pricing, rates, payment behaviour and the project's own cost position are confidential and commercially
sensitive, and some feeds (timesheets, labour records) carry personal data. This work belongs in a **governed
tool** only: supplier and cost-ledger data must never be entered into an ungoverned or public tool (Domain 13,
KAs 13.2.5 and 13.3.4; `PCI-FND-STD-09`). **AI proposes; the professional verifies, decides and remains
accountable.**

### Key terms — KA 5.2

| Term | Meaning |
|---|---|
| **Commitment** | Cost the organisation is bound to once a PO/subcontract is raised. |
| **Accrual** | Received-but-uninvoiced cost recognised so cost-to-date is true. |
| **Actual** | Cost booked once the invoice is processed. |
| **Cost-to-date (control)** | Actuals + accruals; the figure `AC` should reflect. |
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
- A. Commitments give lead time: they signal future spend before it is received or paid. ✅
- B. Commitments are always smaller.
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

**MCQ 5.2-D `[5.2.1 · Application]`** Purchase orders raised total USD 800,000; invoices processed (actuals)
are USD 350,000 and goods received but not yet invoiced (accruals) are USD 90,000. The **open commitment**
is:
- A. USD 360,000 ✅
- B. USD 440,000
- C. USD 450,000
- D. USD 800,000

*Rationale:* `Open commitment = commitments − actuals − accruals = 800,000 − 350,000 − 90,000 = 360,000`: the
ordered value not yet received. B is cost-to-date (`350,000 + 90,000`), the opposite portion; C forgets to
deduct the accruals; D is the total commitment including what has already been received.

**MCQ 5.2-E `[5.2.4 · Recall]`** Which of the following is a data-integrity failure that quietly corrupts
the cost forecast?
- A. An approved variation baselined through change control.
- B. A month-end accrual raised from goods-received records.
- C. A cost ledger reconciled to the general ledger each period.
- D. Open commitments left stale: purchase orders never closed after delivery. ✅

*Rationale:* Stale open commitments overstate the spend still to come, corrupting the forecast until they are
cleansed. Data integrity is an ongoing control, not a one-off clean-up. A, B and C are exactly the disciplines
that *protect* the numbers, not failures.

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
and schedule integrate: the **WBS × OBS** intersection (Domain 1, KA 1.5.4). Beneath it sit **work packages**
(defined, schedulable, costable units of work) and, further down, planning packages (future work not yet
detailed). The control account is deliberately the level at which **earned value is measured and performance
managed**: high enough to be manageable in number, low enough to be meaningful. Choosing the control-account
level well is a real skill: too granular and the overhead of measurement swamps the value; too coarse and
problems hide inside a big account.

**Worked example 5.3.2 — choosing the control-account level.**

1. **Setup.** A **USD 9,600,000** project could be structured as **12 control accounts** (~USD 800,000 each)
   or **96 control accounts** (~USD 100,000 each). Monthly review effort is roughly **30 minutes per CA**.
2. **Formula.** `Monthly review effort = number of CAs × 0.5 hours`.
3. **Substitution.** 12-CA design: `12 × 0.5 = 6 hours` a month. 96-CA design: `96 × 0.5 = 48 hours` a month:
   more than a full working week, every month, before any analysis is done. But a single USD 800,000 CA can
   hide a USD 100,000 problem inside itself for months.
4. **Result.** Most organisations land between the extremes: here, say **~25 control accounts** aligned to the
   WBS's natural work-package boundaries.
5. **Interpretation.** The control-account level is a *designed* trade between visibility and measurement
   cost: 5.3.2's "too granular vs too coarse" made quantitative. The test is whether a variance of the size
   you care about would surface at the level you chose.

**Work authorisation.** In a formal EVMS the budget in a control account is not spendable merely because it
exists; it must be **released**. A **work authorisation document (WAD)** opens the work package: it states the
scope, the budget, the period of performance and the charge codes, and carries the control-account manager's
signature before charging opens. The discipline this buys is threefold: no cost can be booked without an
authorised home (so mischarging becomes visible, KA 5.2.4); no work can start "on account" of a change that
has not been approved (KA 5.4.3); and every booked hour traces back through an auditable chain to an
authorised budget. The WAD is where the paper baseline meets the timesheet. The change-control analogue is
Domain 8, KA 8.4.2, and authorisation as a control is Domain 11, KA 11.3.1.

### 5.3.3 The integration point for earned value

**The principle.** The control account is the hinge between this domain and Domain 6: each CA has a
time-phased budget (its share of `PV`, Domain 3), earns value as its work packages complete (`EV`), and
accrues actual cost (`AC`, KA 5.2). Cost control at the control-account level *is* the data layer earned value
sits on; if the CAs are well-defined, coded (1.5) and their cost states tracked (5.2), earned value is
trustworthy, if not, no amount of EVM formula rigour rescues it.

> **Fig 5.3.1 — Control account as the integration point.** *Caption:* where scope, budget, cost and schedule
> meet. *Underlying data:* one control account CA-Civils-Foundations with budget (PV share), work packages,
> accrued actuals and schedule status. *Render-ready description:* a central brand-blue node "Control account"
> with four inputs converging — "Scope (WBS)", "Budget (PV)", "Actual cost (AC)", "Schedule (SPI)" — and one
> output "Earned value performance (CPI/SPI)". *Animation storyboard (digital-only):* the four inputs flow
> into the node in turn; the node then emits the CPI/SPI outputs, previewing Domain 6.

**AI in this KA.** Structural work is where AI earns its place here: proposing a CBS mapping for a new project
from the coding conventions of completed ones, testing a draft WBS×OBS matrix for control accounts that have
no owner, no budget or no schedulable work beneath them, and flagging where a CA's granularity looks out of
line with comparable accounts. What it cannot do is choose the level of control. Where a control account
should sit is a management judgement about who is accountable for what — too granular and the reporting
overhead swamps the insight, too coarse and a variance has no owner (5.3.2) — and that judgement belongs to
the people who will be held to the accounts. Verify a proposed structure the same way you would a manual one:
every CA has one accountable manager, a time-phased budget, and work packages whose completion can be
evidenced (6.1.2). **AI proposes; the professional verifies, decides and remains accountable.**

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
- B. The BAC changes.
- C. Cost coding becomes unnecessary.
- D. The measurement overhead swamps the value, without improving control. ✅

*Rationale:* Over-granular CAs create measurement burden disproportionate to insight. EV can still be
computed; `BAC` is unaffected; coding is still needed.

**MCQ 5.3-B `[5.3.3 · Recall]`** At what level is earned value normally measured and managed?
- A. The whole project only.
- B. The individual invoice.
- C. The control account. ✅
- D. The company.

*Rationale:* The control account is the designed level for EV measurement and performance management:
manageable in number, meaningful in scope.

**MCQ 5.3-C `[5.3.1 · Recall]`** The cost breakdown structure (CBS) decomposes a project's cost by:
- A. Scope deliverable and work package.
- B. Cost element/type: labour, materials, plant, subcontract, overhead. ✅
- C. Accountable organisational unit.
- D. Reporting period.

*Rationale:* The CBS gives the "by cost type" view; crossed with the WBS's "by scope" view it lets one set
of postings answer both questions. A describes the WBS, C the OBS, and D is a time dimension, not a
breakdown structure.

**MCQ 5.3-D `[5.3.2 · Application]`** A control account holds near-term work that is fully defined,
scheduled and costed, plus future work whose detail is not yet developed. The future work should be held as:
- A. A planning package. ✅
- B. A work package.
- C. A trend.
- D. An open commitment.

*Rationale:* Future work within a control account not yet detailed into schedulable, costable units is
carried as a planning package until it is defined. A work package requires that detail now; a trend is an
early warning of *change*; an open commitment is a raised purchase order, a cost state rather than a scope
element.

### Self-check — KA 5.3

1. What are the two "views" of cost that the CBS and WBS provide? *(By cost type / by scope.)*
2. Why is the control account the natural home for earned value? *(It integrates scope, budget, cost and
   schedule at a manageable, meaningful level.)*

---

## Knowledge Area 5.4 — Change control and cost impact

*Topics: 5.4.1 why change control matters · 5.4.2 trends, variations and change orders · 5.4.3 assessing cost
impact, the change-authority ladder and protecting the baseline.*

### 5.4.1 Why change control matters

**The principle.** A baseline (Domain 3, KA 3.1.3) is only meaningful if it changes **only through control**.
Uncontrolled change, **scope creep**, is the most common way a baseline is quietly lost: a dozen small,
unmanaged additions accumulate until actual no longer relates to plan and variance becomes meaningless. Change
control is the disciplined process by which every proposed change is **identified, assessed
(cost/schedule/risk impact), approved or rejected by a named change authority, and — if approved —
baselined**. Its purpose is not to prevent change but to ensure change is *visible, costed and authorised*.
Note the separation built into that sentence and developed in 5.4.3: the controls professional identifies and
assesses; someone else decides.

### 5.4.2 Trends, variations and change orders

**Definition & purpose.**

- A **trend** is an *early warning* of a potential change: a signal that cost or scope may move before it is
  formalised. Logging trends is what gives a project lead time (the cost-control analogue of a leading
  indicator, 4.1.2).
- A **variation / change order** is a *formal* change to the contract scope or price, raised, priced and
  agreed (the commercial mechanics are in Domain 7, KA 7.2).
- The accounting consequence of an agreed variation flows into revenue via the IFRS 15 contract-modification
  rules (Domain 2, KA 2.2.8) and into the cost baseline via re-baselining.

### 5.4.3 Assessing cost impact and protecting the baseline

**The principle.** Every change is assessed for its **full** impact — direct cost, knock-on cost (disruption,
acceleration), schedule effect, and risk — before approval, not after. Approved changes update the baseline
(and the forecast); rejected changes are recorded and closed. The controls professional maintains the **change
log** that reconciles the current baseline to the original, so at any point the project can answer "how has
the baseline moved, by how much, and why?": the same auditable-movement discipline as a basis of estimate
(3.2.3) or a provision (1.4.6).

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
   10,000,000 + remaining management reserve 200,000 = 10,200,000`: unchanged, because the variation moved
   money *from* reserve *into* the baseline rather than adding new funding. ✓

**Who approves — the change-authority ladder.** The controls professional owns the impact assessment and the
**recommendation**; a **change authority** approves. Those two roles are never held by the same person on the
same change: an assessor who also approves has put no second mind between a cost and the budget, which is the
whole purpose of the process. Authority is delegated by **value and by type**, and a project records its
ladder (the bands and the named holder of each) *before* the first change arrives, rather than when a
contentious one is already on the table. A workable default shape:

| Change | Where approval sits |
|---|---|
| A draw on **contingency** for a risk already carried on the register, within the project manager's recorded delegated limit | Project manager, within delegation |
| A **baseline-changing variation**, or any release from **management reserve** | Project board, or the body named as change authority |
| A change that alters a **contractual** obligation, price or completion date | Sponsor and the client's authorised representative, in whatever form the contract requires (Domain 7, KA 7.2) |

Two rules keep the ladder honest. Changes arising from one instruction or one cause are **aggregated** to
decide which band applies, so a change cannot be split into approvable pieces. And a change is **not executed
before it is authorised**; where genuine urgency forces work to start, that is an exception recorded as such
at the time and ratified at the next meeting of the authority that should have approved it, never a
retrospective approval presented as a routine one.

**What a change record carries before it can be approved.** A change authority can only approve what it can
see, so the minimum content of a change record is fixed rather than left to the assessor's discretion:

- a **unique reference** and the **originating instruction, trend or event** that gave rise to it;
- the **full impact assessment**: direct cost, knock-on cost (disruption, acceleration, time-related
  preliminaries), the schedule effect assessed *through the network* rather than estimated in isolation
  (Domain 10, KA 10.2), the risk and interface effect, and any effect stated as nil rather than omitted;
- the **funding source** proposed (contingency, management reserve, or client funding) and the balance
  remaining on that source before and after;
- the **options considered**, including doing nothing, and why the recommended one is recommended;
- the **approver's identity, authority band and date**, and any condition attached to the approval.

**The escalation trigger.** A change whose assessed cost exceeds the **remaining reserve of its intended
funding source** escalates to the next level of authority *before* it is approved, not after the reserve is
exhausted. A project that approves its way through the bottom of contingency and reports the fact afterwards
has converted a governance decision into an accounting one. The same trigger applies where a change would
take cumulative movement past a baseline tolerance, and where it alters an obligation under the contract.
These disciplines are the substance of `PCI-PCL-STD-05.03` (completeness of change impact assessment) and
`PCI-PCL-STD-05.04` (change authority and segregation of preparation from approval); the decision-owner
principle beneath them is `PCI-FND-STD-04`.

**AI in this KA.** AI can support change control by scanning correspondence, RFIs and site data to **surface
trends early** (a leading-indicator engine for change), estimating the likely cost impact of a proposed change
from historical analogues, and keeping the change log reconciled. The professional owns the impact assessment
and the recommendation; the change authority approves. A model can flag a possible change but cannot weigh its
full disruption/acceleration cost, cannot hold a delegated authority, and cannot authorise anything. Supplier
correspondence, subcontract rates and cost-ledger extracts are confidential and often commercially sensitive,
so this work belongs in a **governed tool** only, never in an ungoverned or public one (Domain 13, KAs 13.2.5
and 13.3.4; `PCI-FND-STD-09`). **AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 5.4

| Term | Meaning |
|---|---|
| **Change control** | The process to identify, assess, approve/reject and baseline change. |
| **Scope creep** | Uncontrolled accumulation of unmanaged change. |
| **Trend** | An early warning of a potential change. |
| **Variation / change order** | A formal, priced, agreed change to contract scope/price. |
| **Change log** | The record reconciling current baseline to original, by change. |
| **Change authority** | The person or body holding the delegated power to approve a change of a stated value and type, never the person who assessed it. |
| **Delegated limit** | The recorded value band within which a named role may approve; above it, the change escalates. |

### Sample MCQs — KA 5.4

**MCQ 5.4-A `[5.4.3 · Application]`** `BAC` is USD 9,700,000. An approved new-scope variation of USD 300,000
(from management reserve) is baselined, and USD 150,000 of contingency is drawn for a materialised risk. The
new `BAC` is:
- A. USD 9,550,000
- B. USD 10,150,000
- C. USD 9,700,000
- D. USD 10,000,000 ✅

*Rationale:* The variation adds new scope to the baseline (`+300,000 → 10,000,000`); the contingency draw is a
*use* of reserve already inside the baseline and does not change `BAC`. A subtracts the risk draw; B adds it;
C ignores the variation.

**MCQ 5.4-B `[5.4.2 · Recall]`** A "trend" in cost control is best described as:
- A. A formal, agreed change order.
- B. A completed variance.
- C. An early warning of a potential change, logged for lead time. ✅
- D. A change to the risk appetite.

*Rationale:* A trend is an early signal of possible change, a leading indicator. A formal change order (A)
comes later; a variance (B) is a realised result; risk appetite (D) is unrelated.

**MCQ 5.4-C `[5.4.1 · Analysis]`** The primary purpose of change control is to:
- A. Prevent all change.
- B. Ensure every change is visible, costed and authorised before it affects the baseline. ✅
- C. Speed up the project.
- D. Replace the forecast.

*Rationale:* Change control governs change so it is transparent, assessed and approved: not to prevent it,
accelerate delivery, or replace forecasting.

**MCQ 5.4-D `[5.4.2 · Application]`** A control account's budget is USD 1,750,000 and its forecast at
completion on committed and remaining scope is USD 1,800,000. The trend log holds one probable but
unformalised change of +USD 120,000. The **potential** variance against budget is:
- A. (USD 170,000) ✅
- B. (USD 50,000)
- C. (USD 290,000)
- D. USD 120,000

*Rationale:* Potential forecast `= 1,800,000 + 120,000 = 1,920,000`; potential variance `= 1,750,000 −
1,920,000 = (170,000)`. B ignores the logged trend, exactly the surprise the trend log exists to prevent; C
double-counts the trend; D is the trend alone, not the variance against budget.

**MCQ 5.4-E `[5.4.1 · Analysis]`** A project's actual cost steadily diverges from a baseline that has never
formally changed; investigation finds many small, unlogged scope additions. This situation is best described
as:
- A. Normal variance, to be managed by exception.
- B. An accrual cut-off error.
- C. Overhead under-absorption.
- D. Scope creep: uncontrolled change accumulating until variance against the baseline is meaningless. ✅

*Rationale:* Unmanaged additions that bypass change control are scope creep, the most common way a baseline is
quietly lost: the "variance" no longer measures performance against an agreed scope. A treats a control
failure as routine; B and C are recognition and allocation issues, not uncontrolled scope growth.

**MCQ 5.4-F `[5.4.3 · Analysis]`** A cost engineer assesses a client-instructed change, prices it, and (being
the delegated holder of a value band that covers it) signs the approval themselves. The change is correctly
priced and correctly baselined. The principal control weakness is:
- A. None; the pricing and the baseline update are both correct.
- B. The assessor and the approver are the same person, so no independent mind ever tested the assessment. ✅
- C. The change should have been logged as a trend before it was assessed.
- D. Client-instructed changes cannot be funded from contingency.

*Rationale:* Change control exists to put a second mind between a cost and the budget; where preparation and
approval sit with the same person, the process produces paperwork and prevents nothing, and that defect
survives a correct price and a correct baseline entry (5.4.3). A treats a correct outcome as evidence of a
sound control. C is wrong on the facts: an instruction already received is a change, not a trend (5.4.2). D
invents a funding rule. The funding-source question is separate from the authority question.

**MCQ 5.4-G `[5.4.3 · Application]`** A variation's assessed cost exceeds the contingency remaining on the
funding source proposed for it. The correct sequence is:
- A. Approve the change, then report the contingency overdraw in the period report.
- B. Approve the change and re-baseline, recording the reserve position afterwards.
- C. Escalate to the next level of authority before approval, with the funding position stated. ✅
- D. Split the change so each part falls within the remaining contingency.

*Rationale:* A change that would exhaust its funding source escalates *before* approval, so that the decision
to release further funding is taken by the body entitled to take it (5.4.3). A and B convert a governance
decision into an after-the-fact accounting entry. D is splitting, precisely the practice the aggregation rule
exists to prevent.

### Self-check — KA 5.4

1. Distinguish a trend from a variation. *(Trend: early warning of a possible change; variation: a formal,
   priced, agreed change.)*
2. When a materialised risk draws on contingency, does `BAC` change? Why or why not? *(No: contingency is
   already inside the baseline; drawing it consumes reserve, it does not add scope.)*
3. Who owns the impact assessment, and who approves the change? *(The controls professional assesses and
   recommends; a change authority holding the relevant delegated band approves, never the same person on the
   same change.)*
4. Name the event that sends a change up the ladder before it is approved. *(An assessed cost exceeding the
   remaining reserve of its proposed funding source: also a breach of a baseline tolerance, or a change
   altering a contractual obligation.)*

---

## Advanced topics — Domain 5

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 5.A.1 — Activity-based costing at a working level

**The principle.** Blanket absorption (5.1.3) spreads *all* overhead on one base. Defensible when overhead is
genuinely driven by that base, distorting when it is not (5.1.2). **Activity-based costing (ABC)** splits
overhead into **cost pools** (procurement, inspection, materials handling), each absorbed on its own **cost
driver** (purchase orders raised, inspections performed). Allocation follows *cause*, pool by pool.

**Worked example 5.A.1 — blanket rate versus ABC.**

1. **Setup.** Overhead **USD 400,000**; blanket base **20,000 labour hours**. Under ABC the same overhead
   splits into a **procurement pool USD 240,000** (driver: **1,200 purchase orders**) and an **inspection
   pool USD 160,000** (driver: **800 inspections**). Two jobs each use **1,000 labour hours**; job P raises
   **20 POs** and **10 inspections**; job Q raises **100 POs** and **70 inspections**.
2. **Formulae.** `Blanket OAR = overhead / hours`; `pool rate = pool / driver volume`; `absorbed = Σ rate ×
   the job's driver usage`.
3. **Substitution.** Blanket: `400,000 / 20,000 = USD 20/hour` → each job absorbs `20 × 1,000 = 20,000`.
   ABC: procurement `240,000 / 1,200 = USD 200/PO`; inspection `160,000 / 800 = USD 200/inspection`. Job P:
   `20 × 200 + 10 × 200 = 6,000`. Job Q: `100 × 200 + 70 × 200 = 34,000`.
4. **Result.** Blanket loads **20,000 on each**; ABC loads **6,000 on P and 34,000 on Q**: the blanket rate
   over-costs P and under-costs Q by USD 14,000 each.
5. **Interpretation.** Identical labour hours, radically different overhead *consumption*. Every unit cost,
   estimate and variance built on the blanket figure inherits the distortion (5.1.2).

**When ABC pays — and when it doesn't.** ABC earns its keep where overhead is large relative to direct
cost, jobs consume support activities very unevenly, and the resulting unit costs feed pricing, estimating
norms (3.2.2) or make-or-buy decisions. It does *not* pay where overhead is small or homogeneous: the pools
and driver counts are themselves an overhead, and a precise allocation of an immaterial number is effort
without a decision attached. The professional test is the one this book applies everywhere: does the extra
precision change a decision?

### Advanced 5.A.2 — Commitment accounting edge cases

**The principle.** The commitment state (5.2.1) is simple for a one-off purchase order; real procurement is
messier, and each edge case has a right answer.

**Frameworks and blanket POs.** A framework agreement (or blanket purchase order) sets terms and a ceiling
(say USD 5,000,000) but binds the organisation to nothing until a **call-off** is placed. The commitment is
recognised **at call-off, not at framework signature**: call-offs to date of USD 1,200,000 mean a commitment
of 1,200,000, not 5,000,000. Recognising the ceiling would swamp the commitment report with spend that may
never occur; recognising nothing until invoice reproduces the invoice-only illusion the cycle exists to
prevent.

**Part-received orders.** As deliveries arrive, cost migrates *along* the states: on a USD 300,000 purchase
order with USD 180,000 received (now sitting in actuals and accruals), the **open commitment** is `300,000 −
180,000 = 120,000` (the formula of 5.2.2b). Reporting the full 300,000 as still open double-counts the
received portion against the accrual.

**Retention.** Retention withheld from a subcontractor (say 5 % of USD 180,000 certified, USD 9,000) is a
**cash** matter, not a cost or commitment matter: the work is received, so the full 180,000 belongs in
cost-to-date, and the commitment reduces by the full amount. Netting retention out of cost understates `AC`
exactly as a missed accrual does (5.2.2).

**Keeping the report honest.** Open-commitment reports rot through **stale POs**: orders delivered or
cancelled but never closed (the data-integrity failure of 5.2.4, MCQ 5.2-E). A standing cleanse (ageing
review, no-movement flags, closure at final invoice) is part of the month-end cycle, because every stale
commitment overstates the spend still to come and corrupts the forecast.

### Advanced 5.A.3 — Cost transfers and journal discipline

**The principle.** A **cost transfer** (a journal moving cost between control accounts or codes) is sometimes
necessary: a genuine mis-code found at reconciliation (5.2.3), a scope re-allocation between accounts, a
correction of a duplicate. But every transfer rewrites history in two places at once: the sending account's
cost falls and the receiving account's rises, *after* the periods in which the trend, the variances and
possibly the `CPI` of both accounts were reported. Done silently, a transfer breaks the three things this
domain exists to protect: **trend** (last month's figure no longer reconciles to this month's opening
position), **variance attribution** (Domain 4's decomposition was computed on cost that has since moved), and
**audit** (the ledger says one thing, the controls history another).

**The discipline.** The remedy is the same discipline the domain applies to baseline change: a **transfer
log**, the direct analogue of the change log (5.4.3). Every transfer carries a **reason** (what was wrong and
how it arose), an **approval** at a level proportionate to its size, the **two sides** stated (from account,
to account, amount, period), and a reference that survives into both accounts' histories, so at any point each
account can answer "how has my cost-to-date moved other than through postings, and why?": the same
auditable-movement test as the change log's "how has the baseline moved?". Transfers should be **rare and
diminishing**: a high transfer volume is a symptom that coding at source (Domain 1, KA 1.5) is failing rather
than a sign of diligence, and the fix belongs upstream, not in ever-busier journals. Two red flags deserve standing
scrutiny: transfers that consistently move cost *off* accounts under pressure just before month-end (a gaming
pattern — see Advanced 4.A.3), and transfers between projects, which can shift cost across contracts with
commercial and revenue-recognition consequences (Domain 2, KA 2.2) far beyond the controls report.

### Advanced 5.A.4 — Integrating cost and schedule data structures

**The principle.** Earned value (Domain 6) demands that `PV`, `EV` and `AC` describe **the same work**. That
is a *data-structure* property, not a formula property: it holds when every schedule activity is **coded to a
control account** (5.3.2) through the same WBS that codes the cost (Domain 1, KA 1.5), so cost and schedule
aggregate to the CA level **by structure**, automatically, repeatably, auditable to source. The alternative
found on many projects is a **mapping spreadsheet**: a hand-maintained table pairing schedule activities with
cost codes. It works on day one and decays from day two: every schedule revision and every new cost code needs
a manual edit nobody owns, and each miss silently mis-states `EV` against `AC` for two control accounts at
once.

**What breaks when the structures diverge.** When cost and schedule run on **different WBS versions** (the
schedule re-organised in an update while the cost ledger kept the old coding) the damage is systematic, not
random. Control-account performance measures compare an `EV` earned on one definition of the work with an `AC`
collected on another, so `CPI` is wrong in *both* directions with no visible error. Variance analysis (Domain 4)
attributes causes to accounts whose boundaries no longer match the work being measured. The forecast
inherits all of it, because `EAC` scales a corrupted `CPI`. And the failure is invisible in each system
separately: schedule and ledger each reconcile internally; only the *join* is broken.

**The discipline.** Treat the shared structure as configuration under change control: one WBS, version-
controlled, with schedule and cost updated **together** through the same authorised change (5.4.3), never one
system re-structured "to be tidied up in the other later". The integration point is the control account
(5.3.3); protecting the structure that defines it is what makes everything Domain 6 computes trustworthy.

### Advanced 5.A.5 — Quantity control: the discipline beneath the money

**The principle.** Cost control watches money, but on measured work the money is `quantity × rate`, and the
two components fail differently. Rate variances are visible in procurement: a tender comes in above the
estimate and everyone sees it. **Quantity growth** (design development, remeasurement, site conditions)
arrives quietly, line by line, and is the classic slow killer of measured-work budgets. Quantity control
therefore tracks three numbers per line: **budget quantity**, **installed quantity** to date, and **forecast
final quantity**, with growth `= forecast final − budget quantity`, priced and reported monthly. It is the
trend discipline of KA 5.4.2 applied to quantities rather than money.

**Worked example 5.A.5 — quantity growth, caught and priced.**

1. **Setup.** A concrete line is budgeted at **2,400 m³ × USD 320/m³ = USD 768,000**. By month 8, **1,500 m³**
   is installed; design development has raised the forecast final quantity to **2,650 m³**.
2. **Formulae.** `growth = forecast final − budget quantity`; `forecast cost = forecast final × rate`;
   `% complete = installed ÷ forecast final`.
3. **Substitution.** Growth `= 2,650 − 2,400 = 250 m³` (`250 ÷ 2,400 ≈ 10.4 %`); forecast cost `= 2,650 × 320 =
   848,000`: **USD 80,000** over budget before any rate variance; `% complete = 1,500 ÷ 2,650 ≈ 56.6 %`.
4. **Result.** An 80,000 trend raised this month against quantity growth (KA 5.4.2), and a percent-complete of
   **56.6 %**, not the flattering `1,500 ÷ 2,400 = 62.5 %` that dividing by the budget quantity would give.
5. **Interpretation.** Dividing installed by *budget* quantity overstates progress the moment quantities grow:
   the same denominator error the rubber baseline makes with money (Domain 6, Advanced 6.A.1). Quantity
   control is also where the claim record starts: a re-measured line with a documented growth history prices
   itself (Domain 7, KA 7.3.4).

**Where the quantities come from.** Budget quantities come from take-offs and the bill of quantities (BoQ);
installed and final quantities from the quantity surveyor's remeasure and, increasingly, from 3D-model
quantity extraction. Extracting installed quantities from models, drone surveys and daily reports is a
fast-maturing machine task (Domain 13, KA 13.5.4), but the forecast *final* quantity is a judgement about
design and ground still to come, and it belongs to the professional.

---

## Case study — Domain 5: commitment blindness on a mining project

### Background

A copper-mining development in a remote region includes a **mine-processing-plant package** (structural,
mechanical and piping works for the primary crushing and flotation circuits) managed as a single control
account with a budget of **USD 2,400,000**. The package is nine months into an eighteen-month duration.
Procurement is heavy and long-lead: major equipment and fabricated steel are ordered many months before they
arrive at site, and the remote location means invoices routinely lag deliveries by six to ten weeks.

For those nine months the monthly cost report has shown the package comfortably "on budget". The report was
built the way many are built under pressure: a straight extract of **invoices processed** from the accounts-
payable ledger, set against the control-account budget. Nobody had falsified anything (the invoice figure was
accurate) but the report answered the question "what have we *paid for*?" when the question that matters for
control is "what have we *incurred and committed*?" (KA 5.2.1).

A new controls lead joins the project and, before signing the next month-end pack, rebuilds the control
account through the full cost-state model: commitments, accruals, actuals, and an estimate for the scope not
yet committed. The exercise takes two days with the procurement register, the goods-received records and the
invoice ledger. What it reveals changes the project's understanding of its own position.

### Rebuilding the cost states

The controls lead works through the three states of cost in order — commitment, accrual, actual (KA 5.2.1) —
and then adds the one figure no ledger holds: the estimate for remaining uncommitted scope.

| Cost state | Source | Amount (USD) |
|---|---|---|
| Invoices processed (actuals) | Accounts-payable ledger | 780,000 |
| Goods/services received, not yet invoiced (accruals) | Goods-received records vs invoice ledger | 140,000 |
| **True cost-to-date (actuals + accruals)** | | **920,000** |
| Purchase orders raised (total commitments) | Procurement register | 1,970,000 |
| Open commitments (raised, not yet received) | 1,970,000 − 920,000 | 1,050,000 |
| Uncommitted remaining scope (estimated) | Quantity take-off of residual scope | 380,000 |
| **Forecast cost at completion** | 920,000 + 1,050,000 + 380,000 | **2,350,000** |

**Worked calculation — the control account rebuilt.**

1. **Setup.** Control-account budget **USD 2,400,000**. Invoices processed (actuals) **USD 780,000**; goods
   and services received but not yet invoiced (accruals) **USD 140,000**; total purchase orders raised
   (commitments) **USD 1,970,000**; uncommitted remaining scope estimated at **USD 380,000**.
2. **Formulae.** `Cost-to-date = actuals + accruals`; `open commitments = total commitments − cost-to-date`;
   `forecast = cost-to-date + open commitments + uncommitted remaining scope` (KA 5.2.1).
3. **Substitution.** Cost-to-date `= 780,000 + 140,000 = 920,000`; open commitments `= 1,970,000 − 920,000 =
   1,050,000`; forecast `= 920,000 + 1,050,000 + 380,000 = 2,350,000`.
4. **Result.** True **cost-to-date USD 920,000** (not the 780,000 in the invoice ledger); **open commitments
   USD 1,050,000**; **forecast USD 2,350,000** against a budget of USD 2,400,000: apparent headroom of **USD
   50,000**.
5. **Interpretation.** The accrual of USD 140,000 is real cost of work already received (steel delivered to
   the laydown area, vendor engineering completed) that simply has no invoice yet; omitting it understates
   `AC` and flatters `CPI` (KA 5.2.2). The USD 1,050,000 of open commitments is money the organisation is
   already bound to pay: the purchase orders are signed, and the only question is *when* the goods arrive and
   the invoices land. Only the final USD 380,000 remains genuinely within the project's discretion.

Note what the rebuild does *not* change: not a single posting moved, and no cost was "found" that the
accounting system lacked. Every figure came from records that already existed: the procurement register, the
goods-received log, the invoice ledger. What changed is that the figures were assembled into the shape that
control requires (KA 5.2.3): by state, at the control-account level (KA 5.3.2), reconciled to one another.

### The invoice-only illusion

Set the two pictures side by side. The old report showed **USD 780,000 spent against USD 2,400,000**: 32.5 %
of budget, "only a third used" at the halfway point of the schedule, a message of comfort. The rebuilt picture
shows a committed-and-forecast position of **USD 2,350,000**, roughly **98 % of the budget already spoken
for**, with half the duration still to run.

**Worked calculation — quantifying the blind spot.**

1. **Setup.** Invoice-only "spend" **USD 780,000**; true committed-and-forecast position **USD 2,350,000**.
2. **Formula.** `Blind spot = forecast position − invoice-only view`.
3. **Substitution.** `2,350,000 − 780,000 = 1,570,000`.
4. **Result.** **USD 1,570,000** (accruals of 140,000, open commitments of 1,050,000 and uncommitted remaining
   scope of 380,000) was invisible to the invoice-only report.
5. **Interpretation.** Nearly two-thirds of the control account's forecast cost existed only in documents the
   invoice report never read. The blind spot is largest on exactly the projects where control matters most:
   long procurement lead times and slow invoice cycles push cost *backwards* through the states, so an
   invoice-only view is worse than late: it is systematically, predictably optimistic.

The illusion is worth dwelling on because nothing about it involved bad data. Each month's invoice total was
correct; the reconciliation to the accounts-payable ledger would have passed. The failure was structural: the
report measured the *last* state of cost (the one with the least lead time and the least control value) and
presented it as the position. A cost engineer watching only actuals is always looking at the past (KA 5.2.1);
on this package, the past was two cost states and USD 1,570,000 behind the present.

### The trend that changes the answer

The rebuild also forces a conversation the old report never prompted. In reviewing the package with the area
engineer, the controls lead learns that the client's review of the flotation-circuit layout is likely to
instruct additional access steelwork and platform modifications. No variation has been issued, but the
engineering team regards the instruction as probable and estimates its cost at **+USD 180,000**. Under KA
5.4.2 this is precisely a **trend**: an early warning of a potential change, not yet a formal variation, and
it belongs in the trend log now, not in next quarter's surprise.

**Worked calculation — the forecast with the pending trend.**

1. **Setup.** Forecast on committed and remaining scope **USD 2,350,000**; logged trend for the probable
   variation **+USD 180,000**; control-account budget **USD 2,400,000**.
2. **Formula.** `Potential forecast = forecast + pending trends`; `variance vs budget = budget − potential
   forecast`.
3. **Substitution.** `2,350,000 + 180,000 = 2,530,000`; `2,400,000 − 2,530,000 = (130,000)`.
4. **Result.** Potential forecast **USD 2,530,000**: a projected overrun of **USD (130,000)** against the USD
   2,400,000 budget if the variation is instructed without additional funding.
5. **Interpretation.** The apparent USD 50,000 of headroom was never real margin. It was headroom *before* a
   known, probable change. The trend log converts a future surprise into a present decision.

That decision has to be made **now**, while options still exist, and it runs through the change-control
machinery of KA 5.4. If the variation is client-instructed new scope, the project should seek a priced
variation and a corresponding baseline change, funding flowing through the authorised reserve structure of
Domain 3 (management reserve for out-of-scope change, contingency for in-scope risk), with the change log
keeping the movement traceable (KA 5.4.3). If additional funding will not be granted, the remaining discretion
is the **USD 380,000 of uncommitted scope**: it is the only part of the forecast the project can still
descope, re-specify or re-tender, and every week of delay converts more of it into commitments. Had the trend
surfaced after the residual scope was on purchase orders, the project would have discovered a USD 130,000
overrun with no levers left to pull.

### The month-end pack

The controls lead replaces the invoice extract with a control-account statement showing every cost state and
the trend position: the shape in which this control account should always have been reported:

| Line | Amount (USD) |
|---|---|
| Control-account budget | 2,400,000 |
| Cost-to-date (actuals 780,000 + accruals 140,000) | 920,000 |
| Open commitments | 1,050,000 |
| Uncommitted remaining scope (estimate) | 380,000 |
| **Forecast at completion** | **2,350,000** |
| Pending trends (probable variation) | +180,000 |
| **Potential forecast at completion** | **2,530,000** |
| **Variance vs budget (potential)** | **(130,000)** |

One page, eight lines, and every question a reviewer should ask is answerable from it. How much is truly
incurred? 920,000. How much is already bound? A further 1,050,000. Where does discretion remain? 380,000. What
is coming that is not yet formal? 180,000. Is the account in trouble? Not yet on approved scope (2,350,000 vs
2,400,000), but **yes** if the probable variation lands unfunded, and the pack says so a quarter before the
invoices would have.

### What the credential expects

This case is Domain 5 in miniature. The engine is the **commitment → accrual → actual** cycle (KA 5.2.1): cost
was visible in the procurement register the day each purchase order was signed, and the project's blindness
lasted exactly as long as its reporting ignored the earlier states. The **true cost-to-date** of actuals plus
accruals (KA 5.2.2) is what `AC` should carry into earned value, omitting the USD 140,000 of accruals would
understate `AC` and flatter `CPI`, corrupting every forecast built on it. The distinction between a **trend
and a variation** (KA 5.4.2) is what bought the project its lead time: logging the probable USD 180,000
instruction turned a future overrun into a present funding decision. **Protecting the baseline** (KA 5.4.3)
then governs how that decision is executed: a priced variation and an authorised baseline change, reconciled
in the change log, rather than a quiet absorption that leaves the budget meaning nothing. And the whole
exercise worked because it was done at the **control account** (KA 5.3.3), the integration point where scope,
budget, cost states and schedule meet: the data layer on which trustworthy earned value stands or falls.
Finally, the AI dimension: auto-coding, commitment-tracking and accrual-proposal tools reading the procurement
register and goods-received data would have surfaced this position months earlier and kept it current every
month since, with the professional owning the accrual judgements, the trend assessments and the funding
decision (Domain 13, KA 13.5.4). **AI proposes; the professional verifies, decides and remains accountable.**

---

## Case study B — Domain 5: eighteen-month lead times on a sterile-facility build (pharmaceuticals)

### Background

A pharmaceutical manufacturer is building a **sterile fill–finish facility** — two cleanroom filling suites, a
lyophiliser, and the water-for-injection (WFI) and HVAC infrastructure that keeps them classified — with a
**`BAC` of USD 58,000,000**, of which **USD 2,400,000 is contingency** inside the baseline. The civil and
building works are conventional; what makes the cost control distinctive is the equipment and the end-game.
The filling line, the lyophiliser, the isolator and the air-handling units are bespoke, vendor-engineered
packages with **lead times of up to eighteen months**: they are ordered near the start of a thirty-month
project, paid against vendor fabrication milestones, and arrive long after most of the concrete is poured. And
the project does not end at mechanical completion: a **validation phase** of commissioning, qualification and
validation (CQV) consultancy runs through the final six months, because a sterile facility that is built but
not qualified cannot release a single saleable vial.

Both features push cost away from the invoice ledger, in opposite directions. Long-lead equipment means
**commitments run far ahead of actuals** for most of the project; the validation phase means the last months'
cost arrives as **services whose invoices lag the work** by two months or more. The controls lead builds the
month-end pack accordingly, by cost state, not by invoice date (KA 5.2.1).

### The commitment ledger at Month 12 (KA 5.2)

The process-equipment control account carries a budget of **USD 18,000,000**. At Month 12 the purchase orders
for the four long-lead packages were placed months ago, the vendors are deep in fabrication, and almost
nothing has been delivered to site. The commitment ledger:

| Package | PO value (USD) | Actuals (USD) | Accruals (USD) | Open commitment (USD) |
|---|---:|---:|---:|---:|
| Filling line | 6,000,000 | 1,800,000 | 600,000 | 3,600,000 |
| Lyophiliser | 4,500,000 | 900,000 | 450,000 | 3,150,000 |
| Isolator | 2,500,000 | 500,000 | 0 | 2,000,000 |
| Air-handling units | 3,000,000 | 600,000 | 300,000 | 2,100,000 |
| **Total** | **16,000,000** | **3,800,000** | **1,350,000** | **10,850,000** |

The actuals are milestone payments already invoiced, down payments and design-approval milestones. The
accruals are vendor fabrication milestones **achieved but not yet invoiced**, evidenced by the inspection and
expediting reports rather than by paperwork in accounts payable (KA 5.2.2).

**Worked calculation — the control account by cost state.**

1. **Setup.** Actuals **USD 3,800,000**; accruals **USD 1,350,000**; total commitments **USD 16,000,000**;
   uncommitted remaining scope (site installation labour and commissioning spares, not yet ordered) estimated
   at **USD 1,700,000**.
2. **Formulae.** `Cost-to-date = actuals + accruals`; `open commitments = commitments − cost-to-date`;
   `forecast = cost-to-date + open commitments + uncommitted remaining scope` (KA 5.2.1).
3. **Substitution.** Cost-to-date `= 3,800,000 + 1,350,000 = 5,150,000`; open commitments `= 16,000,000 −
   5,150,000 = 10,850,000`; forecast `= 5,150,000 + 10,850,000 + 1,700,000 = 17,700,000`.
4. **Result.** Cost-to-date **USD 5,150,000**; open commitments **USD 10,850,000**; forecast **USD
   17,700,000** against the USD 18,000,000 budget: headroom **USD 300,000**.
5. **Interpretation.** An invoice-only report would show USD 3,800,000, **21.1 %** of budget "spent", at the
   project's mid-point. The truth is that **88.9 %** of the budget (16,000,000) is already bound and **98.3
   %** is spoken for; the project's remaining discretion is the USD 1,700,000 not yet on purchase orders. On
   an eighteen-month-lead procurement, the commitment *is* the decision: once the vendor cuts steel, descoping
   and re-tendering are gone, and the control account can only be managed forwards from the register, which is
   why the commitment ledger, not the invoice ledger, is this project's early-warning instrument.

### The validation accrual problem (KA 5.2.2)

Fast-forward to Month 27, deep in the validation phase. The CQV consultancy charges **USD 150 per hour** and
invoices roughly sixty days in arrears. At the September period end, the invoices processed cover **service
dates only up to the end of July** and total **USD 380,000**. The consultancy's approved timesheets show
**1,600 hours worked in August** and **1,400 hours in September**. The earned value claimed for the
validation control account, under its milestone earning rules, is **USD 810,000**.

**Worked calculation — accruing by service date, not invoice date.**

1. **Setup.** Invoiced actuals (service dates to July) **USD 380,000**; August timesheets `1,600 × 150 =
   240,000`; September timesheets `1,400 × 150 = 210,000`; `EV` **USD 810,000**.
2. **Formulae.** `Accrual = value of services received but not invoiced, by service date`; `AC = actuals +
   accruals`; `CPI = EV / AC`.
3. **Substitution.** Accrual `= 240,000 + 210,000 = 450,000`; `AC = 380,000 + 450,000 = 830,000`;
   `CPI = 810,000 / 830,000 = 0.976`; invoice-only "CPI" `= 810,000 / 380,000 = 2.13`.
4. **Result.** A **USD 450,000 accrual**; true `AC` **USD 830,000**; `CPI` **0.98**, not the absurd 2.13 the
   invoice ledger implies.
5. **Interpretation.** It is the **service date**, not the invoice date, that fixes which period a cost
   belongs to (Domain 1, KA 1.3). Two months of qualification work is real, incurred cost with no paperwork
   yet in accounts payable; omit it and the account looks spectacularly efficient in exactly the phase where
   consultancy burn is at its peak. Note the AI trap of KA 5.2.4 in live form: an auto-accrual tool keyed to
   **document dates** would accrue nothing here, reproducing the cut-off error at scale; the accrual judgement
   (which services, to which date, at which rate) stays with the professional.

### Sharing the cleanroom utilities — an ABC question (Advanced 5.A.1)

During the qualification quarter both filling suites draw on the same central utilities (cleanroom HVAC and
the WFI/clean-steam plant) at a shared cost of **USD 800,000** for the quarter. Suite 1 (the owner's vial
line) occupies **2,400 m²**; Suite 2 (a pre-filled-syringe suite being qualified for an external partner under
a **cost-reimbursable tolling agreement**) occupies **1,600 m²**. The allocation is not cosmetic: Suite 2's
share lands on a real third-party invoice.

A blanket allocation by floor area gives `800,000 / 4,000 = USD 200/m²` → Suite 1 **480,000**, Suite 2
**320,000**. The ABC view splits the overhead into pools, each on its causal driver:

| Pool | Cost (USD) | Driver | Volume | Rate | Suite 1 | Suite 2 |
|---|---:|---|---:|---:|---:|---:|
| HVAC & filtration | 500,000 | Air-handler operating hours | 10,000 h | 50/h | 4,000 h → 200,000 | 6,000 h → 300,000 |
| WFI & clean steam | 300,000 | Water drawn | 6,000 m³ | 50/m³ | 1,500 m³ → 75,000 | 4,500 m³ → 225,000 |
| **Total** | **800,000** | | | | **275,000** | **525,000** |

Suite 2 runs its Grade B rooms around the clock through media trials and draws three-quarters of the water:
its *consumption* of the utilities bears no relation to its floor area. The blanket rate over-costs the
owner's suite by **USD 205,000** (`480,000 − 275,000`) and under-charges the partner by the same amount. The
professional test of Advanced 5.A.1 — does the extra precision change a decision? — is met emphatically: a
mis-based allocation here is a USD 205,000 transfer from the partner to the owner every quarter, and the
driver data (air-handler hours, water meters) already exists in the building-management system.

### One change, two funding routes (KA 5.4.3)

In Month 14 the partner's new product requires a **modified isolator interface and additional
environmental-monitoring points**: priced at **USD 640,000**. In engineering the modification, the designer
also finds that the existing glove-port arrangement fails the ergonomic assessment and must be corrected
**regardless of the partner's change**: a materialised design-development risk priced at **USD 360,000**. One
physical work package of USD 1,000,000; two causes; two funding routes, kept traceably distinct:

- The **USD 640,000** is a **client variation** (new scope, instructed and priced) so it is added to the
  baseline: `new BAC = 58,000,000 + 640,000 = 58,640,000`, funded by the client.
- The **USD 360,000** is an **in-scope risk** drawing on contingency: `contingency remaining = 2,400,000 −
  360,000 = 2,040,000`; the `BAC` does not move for it.

The change log carries **one change number with two funding lines**, and the discipline is the point. Fund the
whole USD 1,000,000 from contingency and scope growth hides inside "risk" while the partner escapes a bill it
owes; fund it all as a variation and the partner is charged for the contractor's own design development.
Either blur survives the month unnoticed, and surfaces a year later at the final account, or in an audit, as
an untraceable baseline movement (KA 5.4.3).

### What the credential expects

The pharmaceutical setting stresses every mechanism this domain teaches, at its extreme. The **commitment →
accrual → actual cycle** (KA 5.2.1) is the whole story of long-lead procurement: with eighteen-month lead
times, an invoice-only view is not months but *years* behind the decisions, and the commitment ledger is where
control lives. The **accrual by service date** (KA 5.2.2, Domain 1 KA 1.3) is what keeps `AC` truthful through
a service-heavy validation phase, and is precisely the judgement an auto-accrual tool gets wrong when it reads
document dates (KA 5.2.4). The **ABC allocation** (Advanced 5.A.1) earns its keep because consumption of the
shared utilities is radically uneven *and* a chargeable third-party cost depends on the answer: cause-based
drivers changing a real invoice, not decorating a report. And the **change split** (KA 5.4.2–5.4.3) shows the
reserve architecture working: variation to the baseline, risk to contingency, one change number reconciling
both in the log, so the `BAC`'s movement from 58.00m to 58.64m has exactly one authorised explanation. AI
could keep the commitment ledger current from the procurement register, propose the CQV accruals from
timesheet feeds and meter the utilities allocation automatically, with the professional owning the accrual
cut-off, the driver choice and the funding split (Domain 13, KA 13.5.4). **AI proposes; the professional
verifies, decides and remains accountable.**

---

## Executive perspective — Domain 5

**What the executive must hold onto.** An invoice-only cost view is months out of date: cost is real from
the moment a purchase order is signed, and **commitments** are the early warning that buys a board time to
act (KA 5.2). The true cost-to-date is actuals **plus accruals** — leave the accruals out and `AC` is
understated, `CPI` flattered, and every forecast built on them corrupted. And a baseline is only meaningful
while every movement of the `BAC` is authorised and traceable through the **change log**; a dozen small,
unmanaged changes are how a budget quietly stops meaning anything (KA 5.4).

**Six questions to ask from the chair.**

1. How much is committed but not yet in the cost report, and what share of the budget is already spoken for?
2. Are this month's accruals complete, or is the reported cost-to-date simply the invoices that happen to
   have arrived?
3. How much of the forecast remains genuinely uncommitted, where do we still hold levers to descope,
   re-specify or re-tender?
4. What is in the trend log, and what does the forecast become if the probable trends land unfunded?
5. How has the `BAC` moved since the original baseline, and can every movement be traced to one authorised
   change?
6. When was the project cost ledger last reconciled to the general ledger, and which exceptions are still
   open?

**The traps at board level.**

- **The invoice-only illusion.** "Only a third of the budget spent" can coexist with nearly all of it
  committed; on long-lead, slow-invoicing projects the invoice view is not merely late but systematically
  optimistic.
- **Headroom that is pre-trend.** Apparent margin against budget, quoted before known probable changes, is not
  margin at all. It is the gap the trend log exists to close early.
- **Reserves and scope conflated.** A draw on contingency (in-scope risk) and a management-reserve-funded
  variation (new scope) are different mechanisms; blur them and scope growth hides inside "risk".
- **Comfort from accurate-but-wrong reports.** Every figure can reconcile to the accounts-payable ledger and
  still measure the wrong state of cost. The report was answering "what have we paid for?", not "what have we
  incurred and committed?".

**What good looks like.** Each control account is reported on one page showing every cost state (actuals,
accruals, open commitments, uncommitted scope) with pending trends priced beneath the forecast, and the pack
is read as a forecast, not a record of spend. Accruals are a routine month-end discipline, and the cost ledger
reconciles to the general ledger on a standing cycle with exceptions worked, not parked. The change log
reconciles the current baseline to the original at any moment, so "how has the `BAC` moved, by how much, and
why?" is answerable on demand. Boards in such organisations discuss decisions with lead time (funding a
probable variation, descoping while discretion remains) rather than explaining overruns after the invoices
land.

---

## Calculation exercises — Domain 5

Work each exercise before reading its solution; every step uses only this domain's methods.

**Exercise 5.1**: A site's monthly cost is semi-variable: **USD 150,000 fixed** plus **USD 40 per unit**
produced. The plan for next quarter shows one month at **2,500 units** and one at **4,000 units**. Forecast
the total cost for each month, split each into its fixed and variable elements, and compute the cost per unit
at each volume.

**Solution 5.1.**

1. `Total cost = Fixed + Variable per unit × Volume` (5.1.1).
2. At 2,500 units: variable `= 40 × 2,500 = 100,000`; total `= 150,000 + 100,000 = 250,000`.
3. At 4,000 units: variable `= 40 × 4,000 = 160,000`; total `= 150,000 + 160,000 = 310,000`.
4. Cost per unit: `250,000 / 2,500 = USD 100` at the lower volume; `310,000 / 4,000 = USD 77.50` at the
   higher.
5. Check the behaviour: volume rises 60 % (`2,500 → 4,000`) but total cost rises only 24 %
   (`60,000 / 250,000`), because the USD 150,000 fixed element is spread over more units.

Recognising the split is what lets the forecast scale correctly with volume, and what feeds the flexed budget
(4.2.2).

**Exercise 5.2**. Budgeted overhead is **USD 720,000** over a budgeted **36,000 machine hours**. The period
delivers **33,000 actual hours**, and actual overhead incurred is **USD 735,000**. Compute the OAR, the
overhead absorbed and the over/(under)-absorption: then split the under-absorption into its activity and
spending effects.

**Solution 5.2.**

1. `OAR = 720,000 / 36,000 = USD 20/hour` (5.1.3).
2. `Absorbed = 20 × 33,000 = 660,000`.
3. `Over/(under)-absorption = absorbed − actual incurred = 660,000 − 735,000 = (75,000)`: **under-absorbed USD
   75,000**.
4. Activity effect: `(36,000 − 33,000) × 20 = 60,000`: 3,000 hours below plan under-recover fixed overhead.
5. Spending effect: `735,000 − 720,000 = 15,000`, overhead itself overspent. Split reconciles: `60,000 +
   15,000 = 75,000`. ✓

The single under-recovery figure hides two different problems (activity below plan and spend above it) which a
controls professional splits and explains exactly as a variance (Domain 4).

**Exercise 5.3**. A control account has a budget of **USD 900,000**. To date, purchase orders raised
(commitments) total **USD 600,000**; of these, invoices processed (actuals) are **USD 280,000** and goods
received but not yet invoiced (accruals) are **USD 70,000**. The estimator prices the remaining uncommitted
scope at **USD 320,000**. Compute the controls cost-to-date, the open commitment and the forecast cost, and
the forecast variance against budget.

**Solution 5.3.**

1. `Cost-to-date = actuals + accruals = 280,000 + 70,000 = 350,000` (5.2.1), not the 280,000 in the invoice
   ledger.
2. `Open commitment = commitments − actuals − accruals = 600,000 − 280,000 − 70,000 = 250,000`.
3. `Forecast = cost-to-date + open commitment + uncommitted remaining scope = 350,000 + 250,000 + 320,000 =
   920,000`.
4. Forecast variance `= 900,000 − 920,000 = (20,000)` **(A)**.

Note the uncommitted estimate (320,000) exceeds the budget headroom (`900,000 − 600,000 = 300,000`) by exactly
the USD 20,000 overrun: visible now, while the scope is still uncommitted and the levers (descope, re-specify,
re-tender) still exist.

**Exercise 5.4**: A project's `BAC` is **USD 6,400,000**, which includes a **USD 400,000 contingency
reserve**; a **USD 350,000 management reserve** sits above it (total authorised budget **USD 6,750,000**). An
approved variation adds new scope costing **USD 250,000**, funded from the management reserve; separately, a
materialised risk draws **USD 120,000** of contingency. Compute the new `BAC`, both remaining reserves, and
cross-check the total authorised budget.

**Solution 5.4.**

1. The variation is a **baseline change** (new scope) → `new BAC = 6,400,000 + 250,000 = 6,650,000` (5.4.3).
2. The risk draw is a *use* of contingency already inside the baseline → `BAC` unaffected; contingency
   remaining `= 400,000 − 120,000 = 280,000`.
3. Management reserve remaining `= 350,000 − 250,000 = 100,000`.
4. Cross-check: total authorised budget `= new BAC 6,650,000 + remaining management reserve 100,000 =
   6,750,000`: unchanged, because the variation moved money *from* reserve *into* the baseline. ✓

The change log records one authorised `BAC` movement (the variation); blurring it with the contingency draw
is how scope growth hides inside "risk".

**Exercise 5.5**. A control account has a budget of **USD 2,000,000**. At the data date: purchase orders
raised total **USD 1,450,000**; invoices received against them total **USD 830,000**; a further **USD
190,000** of directly employed site labour (outside the purchase orders) has been performed but not yet
invoiced; cash paid to date is **USD 640,000**. A remaining scope of **USD 500,000** is planned but not yet
ordered. (a) Compute the actual cost to date, the open commitment, the total exposure and the apparent
headroom. (b) Compute the forecast final cost and reconcile it against the apparent headroom: what is the
"headroom illusion"? (c) Explain in one sentence why cash paid (640,000) appears nowhere in (a) or (b).

**Solution 5.5.**

1. (a) `Actual cost = invoiced + accrual = 830,000 + 190,000 = 1,020,000` (5.2.1); `open commitment = PO
   raised − invoiced = 1,450,000 − 830,000 = 620,000`; `total exposure = actual + open commitment =
   1,020,000 + 620,000 = 1,640,000`; apparent headroom `= 2,000,000 − 1,640,000 = 360,000`.
2. (b) `Forecast final cost = exposure + planned-not-ordered = 1,640,000 + 500,000 = 2,140,000`: a forecast
   **overrun of 140,000** against a screen that shows 360,000 of "headroom".
3. The illusion is reading headroom against commitments *made* instead of against the full remaining scope,
   exactly the commitment-blindness case study's failure, at exercise scale (KA 5.2.1).
4. (c) Cash paid lags actual cost by payables and retention timing (`1,020,000 − 640,000 = 380,000` of timing
   difference): cash measures the treasury position (Domain 3, KA 3.5), not cost performance (KA 5.2.2).

---

## Practitioner's toolkit — Domain 5

*Adoption-ready artefacts; adapt the column headings and thresholds to your organisation, then keep them
stable.*

### Toolkit 5.T.1 — Control-account status sheet

| CA | Budget (USD) | Commitments (USD) | Accruals (USD) | Actuals (USD) | Cost-to-date (USD) | Open commitment (USD) | Uncommitted (USD) | Forecast (USD) | Variance (USD) |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| CA-Civils-Foundations | 400,000 | 250,000 | 30,000 | 90,000 | 120,000 | 130,000 | 150,000 | 400,000 | 0 |
| CA-MEP-Installation | 900,000 | 600,000 | 70,000 | 280,000 | 350,000 | 250,000 | 320,000 | 920,000 | (20,000) |

**Usage note.** One row per control account (5.3.2), reported every period so all three cost states are
visible at once: the first row echoes worked example 5.2.2b, where the invoice ledger's USD 90,000 hides a
true cost-to-date of USD 120,000 and USD 130,000 already committed. The internal checks are fixed:
`cost-to-date = actuals + accruals` (5.2.1), `open commitment = commitments − actuals − accruals`, and
`forecast = cost-to-date + open commitment + uncommitted` (5.2.2b); the uncommitted figure is an estimate of
remaining scope, not a plug. Accruals must be complete at month-end or `AC` is understated and `CPI` flattered
(5.2.2), and stale open commitments must be cleansed on a standing cycle (5.2.4, Advanced 5.A.2). Read the
variance column with the trend log beside it: headroom quoted before known probable trends is not margin
(5.4.2).

### Toolkit 5.T.2 — Change/trend log template

| Ref | Description | Type (trend/variation) | Status | Cost impact (USD) | Schedule impact | Funded from (contingency/MR/client) | Assessed by | Approved by (authority band) | Evidence pack complete | Baseline updated |
|---|---|---|---|---:|---|---|---|---|---|---|
| CH-001 | Client-instructed new scope | Variation | Approved | +300,000 | +3 weeks | Management reserve | Cost engineer | Project board: reserve release | Yes | Yes: `BAC` 9,700,000 → 10,000,000 |
| CH-002 | Materialised ground risk | Risk draw (in-scope) | Closed | +150,000 | None | Contingency | Cost engineer | PM, within delegated limit | Yes | No: draw within baseline; contingency 700,000 → 550,000 |
| CH-003 | Probable instruction: additional access steelwork | Trend | Open | +180,000 (est.) | TBC | Not yet agreed | Cost engineer |, (not yet submitted) | No | No, not yet approved |

**Usage note.** The log holds every trend, variation and reserve draw so the current baseline reconciles to
the original at any moment: "how has the `BAC` moved, by how much, and why?" answerable on demand (5.4.3). The
three rows echo worked example 5.4.3 and the mining case: a management-reserve-funded variation *does* move
the `BAC`, a contingency draw for a materialised in-scope risk does *not*, and keeping the two mechanisms
distinct is what stops scope growth hiding inside "risk". Trends are logged the moment they are probable, not
when they are formalised (5.4.2): the potential forecast including open trends is the figure the pack reports
beneath the approved-scope forecast. Close every rejected change explicitly rather than deleting it, so the
audit trail survives.

**The two columns that carry the governance.** *Assessed by* and *Approved by* are separate columns because
they are separate people (5.4.3); a log in which one name fills both on the same row is showing a control
failure, not saving a column. The *Approved by* cell names the **authority band** the approval was given
under, not merely a job title, so a reader can test the approval against the recorded ladder: a PM's delegated
limit for an in-scope contingency draw, the project board for a reserve release or a baseline-changing
variation. *Evidence pack complete* is the gate: no row moves to Approved until the change record carries the
originating instruction, the full impact assessment (including the schedule effect run through the network and
any nil effects stated), the funding source with its balance before and after, and the options considered.
**Retention:** the change log and the assessments behind it are part of the project record and are retained
with it (Domain 8, KA 8.5.1); a baseline movement whose supporting assessment has been discarded can no longer
be explained to anyone.

---

## Exam preparation — Domain 5

**How this domain is examined.** Domain 5 is tested with a fairly even blend of recall (definitions such as
trend versus variation), application (short calculations) and analysis (reading a control failure from a
scenario). The numerical items concentrate in three places: **overhead absorption** and over/under-recovery
(KA 5.1), the **cost-state arithmetic** of commitments, accruals and actuals (KA 5.2), and the effect of
variations and reserve draws on `BAC` (KA 5.4). Scenario items typically describe a reporting practice (an
invoice-only extract, an unlogged trend) and ask what it conceals or which discipline it breaches.

**Calculation traps.**

- **Treating open commitments as cost-to-date.** Cost-to-date is *actuals + accruals*; open purchase orders
  belong in the forecast, not in `AC` (the distractors in MCQ 5.2-A add them in).
- **Reporting invoices only.** Omitting accruals understates `AC` and flatters `CPI`, and the exam likes
  asking for the *direction* of that distortion (MCQ 5.2-C).
- **Absorbing overhead on the wrong figures.** The OAR is *budgeted* overhead ÷ *budgeted* base, applied to
  **actual** activity; distractors offer the budgeted or actual overhead itself, or the bare rate (MCQ
  5.1-A).
- **Comparing absorbed with budgeted rather than actual overhead.** Over/(under)-absorption is absorbed −
  *actual incurred* (MCQ 5.1-C's distractor C), and watch the sign.
- **Adding a contingency draw to `BAC`.** A materialised-risk draw consumes reserve already inside the
  baseline; only new-scope variations move `BAC` (MCQ 5.4-A).
- **Ignoring — or double-counting — logged trends.** The potential variance adds each open trend once to the
  forecast, then compares with budget (MCQ 5.4-D).

**Time management.** The calculations in this domain are short (one or two lines each) so budget roughly a
minute for recall items and no more than two for the multi-step cost-state builds. If a scenario stem is long,
read the question line first: it usually asks for a single figure (cost-to-date, open commitment, new `BAC`)
that the stem's other numbers exist to distract from.

**Reflection questions.**

1. In your organisation's cost reports, is cost-to-date built from actuals plus accruals, or does it quietly
   default to processed invoices, and how would you prove which?
2. How stale are your project's open commitments, and when were they last cleansed against goods-received
   records?
3. When a risk draws on contingency, does your change log keep that visibly distinct from a scope variation,
   or do the two mechanisms blur?
4. Which of your current control accounts is too coarse, or too granular, to support meaningful earned value,
   and what would you change?

---

## Domain 5 summary

Cost control begins with the anatomy of cost: direct/indirect and fixed/variable, driven by true cost drivers,
with indirect cost absorbed via an overhead rate that rarely matches actual (over/under-absorption). Its
engine is the **commitment → accrual → actual** cycle: watching commitments for lead time and including
accruals so **cost-to-date** (and therefore `AC` and `CPI`) is true, all reconciled to the ledger with ongoing
data integrity. Cost is organised through the **CBS** and **control accounts**, the WBS×OBS integration points
where earned value is measured. And **change control** protects the baseline: logging trends early,
formalising variations, assessing full cost impact, and re-baselining only through authorised change, keeping
every movement of the `BAC` traceable. The authority beneath that discipline is explicit: the controls
professional assesses and recommends, a change authority approves within a recorded delegated band, the two
are never the same person on the same change, and a change that would exhaust its funding source goes up the
ladder before it is approved rather than after. This domain is the data-and-discipline layer beneath the
earned-value formulae of Domain 6.

**Cross-references.** Accruals and cut-off → 1.3.5; cost coding and control accounts → 1.5; contract
modifications in revenue → 2.2.8; the flexed budget → 4.2.2; reserves and the baseline → 3.1; the full EVM
treatment → Domain 6; variations and commercial change → 7.2; risk and contingency → Domain 12; automated
coding/reconciliation/change detection → Domain 13, KA 13.5.

**PCI Standards engaged by this domain.** The companion instrument described in the Conventions, section 11, anchors
four certification standards here: `PCI-PCL-STD-05.01` (completeness and reconciliation of the recorded cost
position), `PCI-PCL-STD-05.02` (identification and registration of change), `PCI-PCL-STD-05.03` (completeness
of change impact assessment) and `PCI-PCL-STD-05.04` (change authority and segregation of preparation from
approval), the last two being the instrument behind the change-authority ladder of 5.4.3. The foundational
standards binding on every PCI credential holder apply throughout, in particular `PCI-FND-STD-01`
(professional accountability), `PCI-FND-STD-02` (evidence before assertion), `PCI-FND-STD-05` (transparent
assumptions), `PCI-FND-STD-09` (confidentiality and approved technology), `PCI-FND-STD-11` (escalation of
material misstatement), `PCI-FND-STD-12` (record integrity) and `PCI-FND-STD-14` (responsible AI). The
published Standards govern their own wording; they are private professional requirements established by PCI,
not legislation, and where an applicable legal, regulatory, contractual or authoritative professional
requirement imposes a higher or different obligation, that requirement governs.
