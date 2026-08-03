# Appendices — PCL-AI Body of Knowledge

> Back-matter assembled from the finished domains, per the Style Spine (§4, §9) and the consolidation plan.
> As further domains are revised through SME review, these indexes are regenerated from the source chapters.
> Contents: A) master formula sheet · B) global glossary · C) standards & frameworks referenced · D) figure &
> animation index · E) self-check answers · F) sample-MCQ bank · G) the integrated capstone.

---

## Appendix A — Master formula sheet

All symbols are defined once here and used identically across the book (Style Spine §4). Currency in USD
(+SAR where useful); ratios/indices to 2 dp; adverse amounts in parentheses.

### A1. Accounting & financial reporting (Domains 1–2)

| Formula | Meaning | Domain |
|---|---|---|
| `A = L + E` | The accounting equation | 1.1.1 |
| `Retained earnings = Opening + Income − Expenses − Distributions` | Equity movement | 1.1.1 |
| `Σ Debits = Σ Credits` | Double-entry invariant | 1.1.3 |
| `Annual depreciation = (Cost − Residual) / Useful life` | Straight-line depreciation | 1.3.4 |
| `Carrying amount = Cost − Accumulated depreciation (− impairment)` | Net book value | 1.3.4 |
| `Expected value = Σ (probability_i × outcome_i)` | Provision (large population); variable consideration | 1.4.3 / 2.2.4 |
| `Present value = Future amount / (1 + r)^n` | Discounting a provision/amount | 1.4.3 |
| `PoC = Costs incurred to date / Total estimated costs` | Percentage of completion (cost-to-cost) | 2.2.6 |
| `Cumulative revenue = PoC × Transaction price` | IFRS 15 over-time revenue | 2.2.6 |
| `Allocated price_i = Transaction price × (SSP_i / Σ SSP)` | IFRS 15 allocation | 2.2.5 |
| `Capitalised borrowing cost = weighted-avg qualifying expenditure × rate` | IAS 23 | 2.4.4 |

### A2. Budgeting, cost & performance (Domains 3–5)

| Formula | Meaning | Domain |
|---|---|---|
| `BAC = Σ control-account budgets + contingency reserve` | Budget at completion (cost baseline) | 3.1.4 |
| `Total budget = BAC + management reserve` | Total authorised budget | 3.1.4 |
| `Analogous estimate = past cost × (this driver / past driver)` | Analogous estimating | 3.2.2 |
| `Parametric estimate = parameter × rate` | Parametric estimating | 3.2.2 |
| `Total cost = Fixed + (Variable per unit × Volume)` | Cost behaviour | 5.1.1 |
| `OAR = Budgeted overhead / Budgeted activity base` | Overhead absorption rate | 5.1.3 |
| `Over/(under)-absorption = Overhead absorbed − Overhead incurred` | Absorption variance | 5.1.3 |
| `Cost-to-date (control) = Actuals + Accruals` | True cost-to-date | 5.2.1 |
| `Price variance = (Actual price − Standard price) × Actual quantity` | Variance decomposition | 4.2.3 |
| `Quantity variance = (Actual quantity − Standard quantity) × Standard price` | Variance decomposition | 4.2.3 |

### A3. Earned value & forecasting (Domains 3, 6, 9)

| Formula | Meaning | Domain |
|---|---|---|
| `CV = EV − AC` | Cost variance | 6.2.1 |
| `SV = EV − PV` | Schedule variance | 6.2.1 |
| `CPI = EV / AC` | Cost performance index | 6.2.2 |
| `SPI = EV / PV` | Schedule performance index | 6.2.2 |
| `TCPI (to BAC) = (BAC − EV) / (BAC − AC)` | To-complete performance index | 6.2.3 |
| `TCPI (to EAC) = (BAC − EV) / (EAC − AC)` | To-complete performance index — to meet a revised EAC | 6.2.3 |
| `EAC = AC + ETC` | Estimate at completion (identity) | 6.3.1 |
| `EAC = AC + (BAC − EV)` | EAC — remaining work at budgeted rate | 6.3.2 |
| `EAC = BAC / CPI` | EAC — remaining work at current CPI | 6.3.2 |
| `EAC = AC + (BAC − EV) / (CPI × SPI)` | EAC — cost & schedule compound | 6.3.2 |
| `VAC = BAC − EAC` | Variance at completion | 6.3.4 |
| `ES = M + (EV − PV_M) / (PV_M+1 − PV_M)` | Earned schedule — interpolate between the months bracketing EV (cumulative PV_M ≤ EV ≤ PV_M+1) | 6.4.3 |
| `SV(t) = ES − AT`; `SPI(t) = ES / AT` | Earned schedule (time-based) | 6.4.3 |
| `% complete = points completed / total planned points`; `EV = % complete × BAC` | AgileEVM | 9.5.3 |

### A4. Scheduling & risk (Domains 10, 12)

| Formula | Meaning | Domain |
|---|---|---|
| `EF = ES + duration`; `LS = LF − duration` | Forward / backward pass | 10.2 |
| `Total float = LS − ES (= LF − EF)` | Slack without delaying the project | 10.2.4 |
| `Free float = min(successor ES) − EF` | Slack without delaying a successor | 10.2.4 |
| `tE = (O + 4M + P) / 6`; `σ = (P − O) / 6` | PERT three-point estimate | 10.1.4 |
| `EMV = probability × impact` | Expected monetary value (risk) | 12.2.3 |

### A5. Commercial (Domain 7)

| Formula | Meaning | Domain |
|---|---|---|
| `Fee = target fee + contractor share × (target cost − actual cost)` | CPIF incentive fee | 7.1.3 |
| `Pain/gain share = share ratio × (actual − target)` | Target-cost mechanism | 7.1.4 |
| `LD exposure = LD rate × days late` | Liquidated damages | 7.2.3 |
| `Amount due = (Σ % complete × item amount) − retention − previous payments` | Interim payment application | 7.4.3 |

### A6. Process cycles & working capital (Domain 11)

| Formula | Meaning | Domain |
|---|---|---|
| `DSO = Receivables / Daily revenue` | Days sales outstanding | 11.1.3 |
| `DIO = Inventory / Daily COGS` | Days inventory outstanding | 11.A.1 |
| `DPO = Payables / Daily COGS` | Days payables outstanding | 11.A.1 |
| `CCC = DSO + DIO − DPO` | Cash-conversion cycle | 11.A.1 |
| `Cash freed ≈ DSO reduction (days) × daily revenue` | Working capital released by cutting DSO | 11.A.1 |

---

## Appendix B — Global glossary

Consolidated from every domain's key-terms box — **255 terms**, each shown with the KA where it is first defined. Where a term recurs across domains, the first definition governs (Style Spine §3).

| Term | Definition | First defined |
|---|---|---|
| **100 % rule** | The WBS captures all of the scope and only the scope. | 8.2 |
| **`CPI` / `SPI`** | Cost / schedule performance index (`EV/AC`, `EV/PV`). | 3.4 |
| **`CV` / `SV`** | Cost variance (`EV − AC`) / schedule variance (`EV − PV`). | 6.2 |
| **`EAC` / `ETC`** | Estimate at completion / to complete; `EAC = AC + ETC`. | 3.4 |
| **`SPI` convergence** | The tendency of `SPI` → 1 at completion regardless of lateness. | 6.4 |
| **`TCPI`** | To-complete performance index — the efficiency remaining work must achieve for a target. | 6.2 |
| **`VAC`** | Variance at completion (`BAC − EAC`). | 3.4 |
| **Accounting equation** | `Assets = Liabilities + Equity`; holds after every transaction. | 1.1 |
| **Accrual** | Liability for goods/services received, amount/timing reasonably certain. | 1.4 |
| **Accrual basis** | Recognise economic events when they occur, not when cash moves. | 1.3 |
| **Accrued expense / income** | Recognised before the cash is paid / received. | 1.3 |
| **Accuracy range** | The expected low/high band around a point estimate for its class. | 3.2 |
| **Activity** | The unit of work sequenced and durated, traceable to the WBS. | 10.1 |
| **Actual** | Cost booked once the invoice is processed. | 5.2 |
| **Actual Cost (`AC`)** | Cost actually incurred (incl. accruals) for the work performed. | 6.1 |
| **Agent** | Arranges for another to provide; recognises net fee/commission. | 2.3 |
| **Agile audit trail** | Backlog/Sprint/Increment records providing contemporaneous evidence. | 9.6 |
| **Agile contracting** | T&M, capped T&M, target cost — forms that fit variable scope. | 9.6 |
| **Agile mindset / Manifesto** | Valuing working outcomes, collaboration, value and responsiveness to change. | 9.1 |
| **AgileEVM** | Earned value applied to variable-scope adaptive delivery (Domain 9). | 6.4 |
| **AI / ML / GenAI** | The field / learning-from-data subset / content-generating subset (nested). | 13.1 |
| **AI proposes; the professional verifies, decides and remains accountable** | AI drafts/predicts; a qualified professional decides and is accountable. | 13.6 |
| **AI-assisted disclosure/forecast** | AI-drafted output the professional verifies and signs off. | 13.5 |
| **AI-maturity model** | Ad-hoc → piloting → standardised → integrated → governed/optimised. | 13.7 |
| **AI-use policy / verification checklist** | The governance document and the operational assurance step. | 13.6 |
| **Amount due** | Net certified value less previous payments. | 7.4 |
| **Analogous / parametric / bottom-up** | Top-down scaling / rate × parameter / work-package build-up. | 3.2 |
| **Articulation** | The way the statements interlock through shared figures (profit, cash, working capital). | 1.2 |
| **Audit trail** | The verifiable record of who did what, when, with what authorisation. | 11.3 |
| **Auditability / sign-off** | Keeping the trail of what AI produced, who approved it, what changed and why. | 13.6 |
| **Auto-coding / reconciliation** | AI coding cost and matching it to the ledger. | 13.5 |
| **Baseline comparison** | Current vs baseline milestones and critical path. | 10.4 |
| **Baselines (scope/schedule/cost)** | The approved, integrated plans control measures against. | 8.2 |
| **Basis of estimate (BoE)** | The auditable record of scope, assumptions, rates, exclusions and class. | 3.2 |
| **Bill of quantities (BoQ)** | Itemised measured quantities priced at rates. | 7.3 |
| **Billing vs revenue** | Contract payment mechanism vs IFRS 15 performance-based recognition. | 7.5 |
| **Budget at Completion (`BAC`)** | The total value of the cost baseline. | 3.1 |
| **Bundle** | Multiple promises in one contract, allocated by SSP and recognised on their own patterns. | 2.3 |
| **Burndown / burnup** | Remaining work down to zero / completed scope up to a (moving) total. | 9.3 |
| **Business case** | The benefits-vs-cost-and-risk justification for the project. | 8.1 |
| **Cadence** | The reporting frequency matched to audience and decision rhythm. | 4.3 |
| **Carrying amount** | Cost less accumulated depreciation (and impairment). | 1.3 |
| **Cash application** | Matching cash received to invoices. | 11.1 |
| **Cash-flow forecast** | A time-phased projection of cash in and out, separate from profit. | 3.5 |
| **Category-to-task fit** | Matching the task and governance need to the right category. | 13.4 |
| **Certification** | The client's agreement of the amount payable. | 7.4 |
| **Change control** | The process to identify, assess, approve/reject and baseline change. | 5.4 |
| **Change log** | The record reconciling current baseline to original, by change. | 5.4 |
| **Change management** | Bringing people with you; honest about limits, resistant to hype. | 13.7 |
| **Chart of accounts (CoA)** | The structured list of all ledger accounts, coded by class. | 1.5 |
| **Chart-to-question fit** | Choosing the chart type that answers the actual question. | 4.4 |
| **Claim** | A notified, substantiated assertion of entitlement to time/money. | 7.2 |
| **Closing** | Formal completion — acceptance, contract closure, demobilisation, archiving. | 8.5 |
| **Commercial-to-accounting loop** | Scope → cost → EV → billing → revenue → statements → reporting. | 7.5 |
| **Commitment** | Cost the organisation is bound to once a PO/subcontract is raised. | 5.2 |
| **Constraint trade-offs** | Scope/schedule/cost/quality/risk balanced against each other. | 8.4 |
| **Contingency reserve** | For identified risks; inside the baseline; PM-controlled. | 3.1 |
| **Contingent liability / asset** | A possible obligation/inflow — disclosed, not recognised (subject to probability). | 1.4 |
| **Contract asset / liability** | Revenue recognised vs amounts billed — under-billing (asset) vs over-billing (liability). | 2.2 |
| **Control account (CA)** | The WBS×OBS intersection where scope, budget, cost and schedule integrate. | 1.5 |
| **Controls dashboard** | An integrated cost/schedule/forecast/risk view with RAG status and trend. | 4.3 |
| **Cost baseline / PMB** | The approved, time-phased budget; source of Planned Value; total = BAC. | 3.1 |
| **Cost breakdown structure (CBS)** | Decomposition of cost by element/type. | 1.5 |
| **Cost code** | A segmented code pinning a posting to project, scope, cost element and resource. | 1.5 |
| **Cost driver** | The factor that causes a cost to change. | 5.1 |
| **Cost extraction / reconciliation** | Pulling cost from source systems and tying it back to the ledger. | 1.5 |
| **Cost-plus (CPFF/CPIF/CPAF)** | Reimburse cost plus a fixed/incentive/award fee; client bears cost risk. | 7.1 |
| **Cost-schedule integration** | Measuring both from one `EV` so they cannot contradict. | 6.4 |
| **Cost-to-date (control)** | Actuals + accruals — the figure `AC` should reflect. | 5.2 |
| **Crashing** | Add resources to critical activities (cost for time). | 10.3 |
| **Credit control** | Assessing/limiting customer credit before committing. | 11.1 |
| **Critical path** | The longest, zero-float chain; sets the project duration. | 10.2 |
| **Cumulative flow / throughput / cycle time** | Work-state bands / items per period / start-to-done time. | 9.3 |
| **Current vs non-current** | The IAS 1 split by expected realisation/settlement within ~12 months. | 1.2 |
| **Cut-off** | Recording each transaction in the correct period. | 1.3 |
| **Data integrity** | Ongoing correctness of the cost data (coding, commitments, accruals, one source). | 5.2 |
| **Data quality dimensions** | Accuracy, completeness, consistency, timeliness, validity, uniqueness. | 13.2 |
| **Debit / Credit** | Left/right sides of an account; effect depends on account type. | 1.1 |
| **Dependency (FS/SS/FF/SF)** | The four logical relationships between activities. | 10.1 |
| **Depreciation** | Systematic spreading of a long-lived asset's cost over its useful life. | 1.3 |
| **Direct / indirect cost** | Traceable to one cost object / supporting many (overhead). | 5.1 |
| **Discount unwind** | The increase in a discounted provision as settlement nears, charged as finance cost. | 1.4 |
| **Double-entry** | Recording every transaction with `Σ Dr = Σ Cr`. | 1.1 |
| **Draw-down / re-baselining** | Consuming contingency as risks occur / escalating beyond it as a baseline change. | 12.3 |
| **Driver analysis** | AI explanation of *why* a metric is moving (e.g. an EAC). | 13.5 |
| **EAC (a)–(d)** | Budgeted-rate / current-CPI / CPI×SPI / bottom-up methods, each an assumption. | 6.3 |
| **Earned schedule (`ES`)** | Earned value expressed as a point on the time axis; gives `SV(t)`, `SPI(t)`. | 6.4 |
| **Earned Value (`EV`)** | Budgeted cost of work performed — progress valued at budget. | 6.1 |
| **Earning rule / measurement method** | The rule converting physical progress to `EV` (0/100, 50/50, % complete, units, milestones). | 6.1 |
| **Embedded AI** | AI features within the platforms controls already uses. | 13.4 |
| **Empirical process control** | Decisions from observation via transparency, inspection, adaptation. | 9.1 |
| **Enhancing characteristics** | Comparability, verifiability, timeliness, understandability. | 2.1 |
| **Entity concept** | The business is accounted for as separate from its owners. | 1.1 |
| **EPC / turnkey** | Single-contractor delivery of the whole asset. | 7.1 |
| **Estimate class (AACE 5–1)** | Maturity-based classification driving expected accuracy range. | 3.2 |
| **Exception report** | Only the out-of-tolerance items, with cause, impact and action. | 4.3 |
| **Executing** | Performing the work packages and producing deliverables. | 8.3 |
| **Expected monetary value (EMV)** | `probability × impact`; summed as a contingency basis. | 12.2 |
| **Expected value** | Probability-weighted average of outcomes (for large populations). | 1.4 |
| **Faithful representation** | Complete, neutral, error-free; substance over form. | 2.1 |
| **Fast-tracking** | Overlap sequential activities (time for risk). | 10.3 |
| **Final account** | The definitive commercial settlement of the project. | 8.5 |
| **Fixed / variable cost** | Independent of / changing with activity volume. | 5.1 |
| **Flexed budget** | The budget adjusted to the actual output level before comparison. | 4.2 |
| **Forward / backward pass** | Compute early dates (`ES/EF`) / late dates (`LS/LF`). | 10.2 |
| **Free float (`FF`)** | Slack without delaying any successor. | 10.2 |
| **Garbage in, garbage out** | AI outcomes are dominated by input-data quality. | 13.2 |
| **Going concern** | The assumption the entity continues in operation. | 2.1 |
| **Governance / lineage** | Ownership/definitions/access / traceability of a data point to source. | 13.2 |
| **Gross vs net revenue** | Whole consideration vs only the entity's margin/fee. | 2.3 |
| **Guardrails** | Rules of safe use (confidentiality, verification, disclosure, audit trail). | 13.3 |
| **Hallucination** | Confidently producing false content. | 13.1 |
| **Hallucination / bias / confidentiality** | The three principal AI risks and their mitigations. | 13.6 |
| **Hybrid** | Combining predictive governance with adaptive execution (predictive stage-gates around agile delivery). | 8.6 |
| **IAS 11 (legacy)** | Former construction-contract standard, superseded by IFRS 15. | 2.4 |
| **IAS 16 / capitalise** | PPE recognised at cost and depreciated; capitalise vs expense judgement. | 2.4 |
| **IAS 2 / NRV** | Inventories at the lower of cost and net realisable value. | 2.4 |
| **IAS 23 / qualifying asset** | Capitalise borrowing costs directly attributable to a qualifying asset. | 2.4 |
| **IFRS / local GAAP** | The global standards / national frameworks. | 2.1 |
| **IFRS 16 / right-of-use** | Most leases on balance sheet as a right-of-use asset and lease liability. | 2.4 |
| **Incremental** | Building the product in usable slices — adding parts to the whole. | 8.6 |
| **Input (cost-to-cost) / output method** | Ways of measuring progress toward complete satisfaction. | 2.2 |
| **Integrated change control** | Assessing every change across all constraints before approval. | 8.4 |
| **Integration / upskilling** | Embedding AI in the workflow / building data, prompting, verification, governance skills. | 13.7 |
| **Interim valuation / application** | Periodic assessment of value done, less retention and prior payments. | 7.4 |
| **Internal control** | Policies/procedures giving assurance over reporting, operations, compliance. | 11.3 |
| **Inverted iron triangle** | Fixed time & cost, variable scope. | 9.3 |
| **ISO 31000** | Principles/process for integrated, proportionate risk management. | 12.1 |
| **Issue** | A risk that has already occurred. | 12.1 |
| **Iterative** | Refining the same product over repeated passes. | 8.6 |
| **Iterative refinement** | Prompt → review → refine. | 13.3 |
| **Kanban** | Visualise flow, limit WIP, manage/measure flow, improve. | 9.4 |
| **KPI** | A measure chosen to reflect progress toward an objective. | 4.1 |
| **Lagging indicator** | Measures an outcome already realised (e.g. `CPI`). | 4.1 |
| **Lead / lag** | An overlap / a delay on a dependency. | 10.1 |
| **Leading indicator** | Measures a predictor of a future outcome (e.g. productivity trend). | 4.1 |
| **Lean / waste** | Maximise value, minimise non-value work. | 9.4 |
| **Ledger / T-account** | The set of accounts; each account drawn with debits left, credits right. | 1.1 |
| **Lessons learned** | Captured experience and performance data feeding future projects. | 8.5 |
| **Liquidated damages (LDs)** | A pre-agreed sum for a defined breach (usually late completion). | 7.2 |
| **Little's Law** | Cycle time ≈ WIP ÷ throughput (conceptually). | 9.4 |
| **Lump sum / fixed price** | Fixed price for defined scope; contractor bears cost risk. | 7.1 |
| **Management by exception** | Focusing on items outside tolerance. | 4.1 |
| **Management report** | A decision-support document, structured by the work and the audience. | 4.3 |
| **Management reporting** | Internal, flexible, timely, forward-looking reporting. | 2.5 |
| **Management reserve** | For unforeseen scope/risk; outside the baseline; management-controlled. | 3.1 |
| **Matching principle** | Recognise expenses in the same period as the income they help earn. | 1.3 |
| **Measurement** | Deriving quantities from design under a standard method. | 7.3 |
| **Milestone-to-Sprint mapping** | Translating Sprints/releases into gate milestones. | 9.6 |
| **Monitoring & controlling** | Measuring against baselines and acting to correct — parallel to executing. | 8.4 |
| **Normal balance** | The side on which an account type increases. | 1.1 |
| **Offsetting** | Netting assets/liabilities or income/expenses — generally prohibited. | 2.1 |
| **Onerous contract** | Unavoidable costs exceed expected benefits; the loss is provided immediately. | 1.4 |
| **Open PO / GRNI** | Commitment / goods-received-not-invoiced (accrual driver). | 11.2 |
| **Order-to-cash (O2C)** | Order → credit → fulfil → invoice → collect → apply cash. | 11.1 |
| **Over time vs point in time** | Whether an obligation is satisfied progressively or at a moment of control transfer. | 2.2 |
| **Over/under-absorption** | Overhead absorbed minus overhead actually incurred. | 5.1 |
| **Overhead absorption rate (OAR)** | Budgeted overhead ÷ budgeted activity base. | 5.1 |
| **Overview-first, detail-on-demand** | Summary on one view, drill-down to the detail behind any red. | 4.3 |
| **P80 contingency** | Contingency set at an 80 %-confidence outcome from a risk model. | 12.3 |
| **Payment terms** | The lag between billing and collection (and between receipt and paying suppliers). | 3.5 |
| **Peak funding requirement** | The deepest point of cumulative cash — the finance to arrange. | 3.5 |
| **Performance / advance-payment / retention bond** | Third-party security instruments. | 7.2 |
| **Performance obligation** | A promise to transfer a distinct good or service. | 2.2 |
| **PERT (three-point)** | `tE = (O + 4M + P)/6`; `σ = (P − O)/6`. | 10.1 |
| **Planned Value (`PV`/BCWS)** | Cumulative planned spend to date — the cost-baseline curve; the budgeted cost of work scheduled by the data date. | 3.3 |
| **Planning package** | Future work within a CA not yet detailed into work packages. | 5.3 |
| **Power/interest grid** | A tool to classify stakeholders and tailor engagement. | 8.1 |
| **Predictive / adaptive** | Plan-driven (fixed scope) / change-driven (evolving scope). | 8.6 |
| **Preliminaries** | Project-wide, often time-related costs not tied to a measured item. | 7.3 |
| **Prepayment / deferred income** | Cash paid / received before the expense / income is recognised. | 1.3 |
| **Present obligation** | A legal or constructive duty arising from a past event. | 1.4 |
| **Preventive / detective control** | Stops an error occurring / detects it afterwards. | 11.3 |
| **Price/rate variance** | `(Actual price − Standard price) × Actual quantity`. | 4.2 |
| **Principal** | Controls the good/service before transfer; recognises gross revenue. | 2.3 |
| **Process mining** | Reconstructing actual process flows from ERP event logs. | 11.3 |
| **Procure-to-pay (P2P)** | Requisition → PO → receipt → invoice → three-way match → pay. | 11.2 |
| **Procurement execution** | Placing and managing orders/subcontracts (commitments). | 8.3 |
| **Product / Sprint Backlog, Increment** | The three artefacts. | 9.2 |
| **Product Goal / Sprint Goal / Definition of Done** | Their respective commitments. | 9.2 |
| **Product Owner / Scrum Master / Developers** | Value / effectiveness / doing-the-work accountabilities. | 9.2 |
| **Progress reconciliation** | Tying `EV` (cost), BoQ valuation (billing) and IFRS 15 revenue. | 7.4 |
| **Progress reconciliation (agile)** | Tying AgileEVM %, cost-to-cost %, and billing basis. | 9.5 |
| **Progressing / data date** | Recording actuals and remaining durations, then recalculating. | 10.4 |
| **Project charter** | The document authorising the project and the project manager. | 8.1 |
| **Prompt** | The instruction/context/data given to a GenAI model. | 13.3 |
| **Prompt patterns** | Reusable shapes: extraction, analysis, drafting, summarisation, transformation. | 13.3 |
| **Propose → verify → own** | The universal AI-in-controls workflow shape. | 13.5 |
| **Provision (IAS 37)** | Liability of uncertain timing or amount, meeting the three recognition tests. | 1.4 |
| **Qualitative analysis** | Probability × impact rating on a matrix. | 12.2 |
| **Quality assurance** | Building quality into the process (vs inspecting it out). | 8.3 |
| **Quantity/efficiency variance** | `(Actual quantity − Standard quantity) × Standard price`. | 4.2 |
| **Rate** | Price per unit (labour, materials, plant, overhead, profit). | 7.3 |
| **Rebaselining** | Transparently resetting scope/`BAC` when scope is deliberately flexed. | 9.5 |
| **Receivables ageing** | The overdue profile of amounts owed — a cash/revenue leading indicator. | 11.1 |
| **Reconciliation** | Explaining the tie between the two views from one ledger. | 2.5 |
| **Relevance / materiality** | Capable of influencing decisions; material if its omission/misstatement would. | 2.1 |
| **Remeasurement (contract form) / unit-rate** | Priced at rates against re-measured actual quantities. | 7.1 |
| **Remeasurement (of a BoQ)** | Re-pricing at actual quantities. | 7.3 |
| **Resource levelling / smoothing** | Respect resource limits (may extend) / even peaks within float (no extension). | 10.3 |
| **Response strategies** | Avoid/transfer/mitigate/accept (threats); exploit/share/enhance/accept (opportunities). | 12.2 |
| **Retention** | Cash withheld from payments until completion/defects periods pass. | 3.5 |
| **Risk** | An uncertain event/condition affecting objectives — threat or opportunity. | 12.1 |
| **Risk appetite / tolerance** | Risk willingly accepted / acceptable variation around it. | 12.1 |
| **Risk register** | The living record: cause-event-effect, owner, assessment, response. | 12.2 |
| **Rolling forecast** | A forecast re-produced each period over a fixed forward horizon. | 3.4 |
| **Rolling wave** | Detailed near-term planning, outline further out. | 9.1 |
| **Run-rate / capacity funding** | Cost = cost per Sprint × Sprints; `ETC` from Sprints remaining. | 9.5 |
| **S-curve** | The characteristic cumulative-spend shape: slow–fast–slow. | 3.3 |
| **SAFe / LeSS / Scrum-of-Scrums / release train** | Scaling approaches (awareness level). | 9.4 |
| **Schedule increment** | A Sprint/release as a time-phased unit mapped to milestones. | 10.4 |
| **Schedule-risk analysis** | Model duration uncertainty (Monte Carlo) for a completion distribution. | 10.3 |
| **Scope** | What will be delivered. | 8.2 |
| **Scope creep** | Uncontrolled accumulation of unmanaged change. | 5.4 |
| **Segregation of duties (SoD)** | No single person controls a whole transaction. | 11.3 |
| **Small multiples** | Repeated small charts — must share consistent scales. | 4.4 |
| **SOCE** | Statement of changes in equity — reconciles opening to closing equity. | 1.2 |
| **SOFP** | Statement of financial position — assets, liabilities, equity at a point in time. | 1.2 |
| **SOPL & OCI** | Statement of profit or loss and other comprehensive income — performance over a period. | 1.2 |
| **Sprint** | The fixed short cycle containing all other events; produces a usable Increment. | 9.2 |
| **Stakeholder** | Anyone who affects or is affected by the project. | 8.1 |
| **Standalone selling price (SSP)** | The price of a good/service sold separately; the basis for allocation. | 2.2 |
| **Statement of cash flows** | Change in cash split into operating, investing and financing. | 1.2 |
| **Statutory reporting** | Standards-compliant, audited external reporting. | 2.5 |
| **Story point / velocity** | Relative size unit / average points completed per Sprint. | 9.3 |
| **Storytelling** | Ordering true facts (status → cause → forecast → action) to drive a decision. | 4.4 |
| **Structured / unstructured** | Tabular (ML) vs free-form (GenAI/RAG) data. | 13.2 |
| **Subsidiary plans** | Quality, resource, risk, procurement, communications, integration. | 8.2 |
| **Success criteria** | The definition of doing well — beyond scope/time/cost to benefit and quality. | 8.1 |
| **Supervised / unsupervised / reinforcement** | Learn from labels / structure / trial-and-reward. | 13.1 |
| **Tailoring** | Adapting the approach to the specific project. | 8.6 |
| **Target / threshold / tolerance** | The aim / the attention boundary / the allowable deviation. | 4.1 |
| **Target cost / pain-gain** | Shared cost risk against a target, within a cap/collar. | 7.1 |
| **Three-way match** | Matching invoice to PO and goods-receipt note before payment. | 11.2 |
| **Time-phased budget** | The `BAC` spread across the schedule by period. | 3.3 |
| **Token / context window / temperature** | Text unit / how much the model considers / randomness setting. | 13.1 |
| **Tool category** | A class of AI tool (assistant, RAG, analysis, BI, PM-suite, ML, RPA, CLM, meeting, coding). | 13.4 |
| **Total float (`TF`)** | Slack without delaying the project (`LS − ES`). | 10.2 |
| **Training / inference / fine-tuning / RAG** | Learn parameters / use the model / specialise it / ground it in your documents. | 13.1 |
| **Transaction price** | Consideration the entity expects to be entitled to (incl. variable consideration). | 2.2 |
| **Trend** | An early warning of a potential change. | 5.4 |
| **Trend analysis** | Reading the direction of indices/cost over successive periods. | 3.4 |
| **Trial balance** | A list of all account balances proving `Σ Dr = Σ Cr` (not correctness). | 1.1 |
| **Truncated axis** | A non-zero-based axis that exaggerates differences. | 4.4 |
| **User story / acceptance criteria / INVEST** | A value-framed backlog item, its "done" tests, and quality heuristics. | 9.3 |
| **Value measurement** | Honest measurement of time/error/warning/accuracy gains vs cost and risk. | 13.7 |
| **Variable consideration / constraint** | Estimated by expected value or most-likely amount; included only to the extent a significant reversal is highly improbable. | 2.2 |
| **Variance** | Planned minus actual; favourable (F) improves profit, adverse (A) worsens it. | 4.2 |
| **Variance bridge** | A waterfall walking budget to actual by variance component. | 4.2 |
| **Variation / change order** | A formal, priced, agreed change to contract scope/price. | 5.4 |
| **Verification** | Checking every AI output against source before use. | 13.3 |
| **WIP limit** | A cap on concurrent work-in-progress that speeds completion. | 9.4 |
| **Work breakdown structure (WBS)** | Decomposition of scope into deliverables/work packages. | 1.5 |
| **Work package** | A defined, schedulable, costable unit of work beneath a CA. | 5.3 |
| **Zero-based budgeting** | Justifying every cost from zero each cycle. | 3.1 |

---

## Appendix C — Standards & frameworks referenced

Named at principle level; **never reproduced verbatim** (Style Spine §9). Real frameworks only; no fabricated
citations.

| Standard / framework | Used for | Domains |
|---|---|---|
| **IAS 1** | Presentation of financial statements | 1.2, 2.1 |
| **IAS 2** | Inventories (lower of cost and NRV) | 2.4 |
| **IAS 16** | Property, plant & equipment (capitalise/depreciate) | 1.3, 2.4 |
| **IAS 23** | Borrowing costs (capitalise on qualifying assets) | 2.4 |
| **IAS 37** | Provisions, contingent liabilities/assets; onerous contracts | 1.4, 2.2, 2.4 |
| **IAS 11 (legacy)** | Construction contracts — superseded by IFRS 15 | 2.4 |
| **IFRS 15** | Revenue from contracts with customers (five-step model) | 2.2, 2.3, 7.5, 9.5 |
| **IFRS 16** | Leases (right-of-use asset & lease liability) | 2.4 |
| **PMBOK Guide** | Project management process groups & practices | 8 |
| **AACE TCM Framework / estimate classes** | Total cost management; estimate classification | 3.2 |
| **ISO 31000** | Risk management principles & process | 12 |
| **ISO/IEC 17024** | Personnel certification (credential design reference) | — |
| **Agile Manifesto** | Agile values & principles (described, not reproduced) | 9.1 |
| **Scrum Guide** | Scrum accountabilities, events, artefacts, commitments | 9.2 |
| **Kanban / Lean** | Flow and waste-reduction principles | 9.4 |
| **SAFe / LeSS / Scrum-of-Scrums** | Scaling frameworks (awareness level) | 9.4 |
| **FIDIC (and jurisdictional forms)** | Standard contract forms (awareness level) | 7.2 |

---

## Appendix D — Figure & animation index

Every numbered figure specification, with its digital-only animation storyboard where defined.

| Figure | Title | Domain |
|---|---|---|
| Fig 1.1.1 | The accounting equation as a balance | 1.1 |
| Fig 1.1.2 | From transaction to trial balance *(animated)* | 1.1 |
| Fig 1.2.1 | How the four statements articulate *(animated)* | 1.2 |
| Fig 1.3.1 | Accrual vs cash timing on one line *(animated)* | 1.3 |
| Fig 1.4.1 | IAS 37 recognition decision tree *(animated)* | 1.4 |
| Fig 1.5.1 | WBS × CBS × OBS: how a cost is coded *(animated)* | 1.5 |
| Fig 2.2.1 | The IFRS 15 five-step model *(animated)* | 2.2 |
| Fig 2.2.2 | Revenue and cost S-curves over the contract *(animated)* | 2.2 |
| Fig 3.1.1 | The budget waterfall | 3.1 |
| Fig 3.3.1 | The Planned Value S-curve *(animated)* | 3.3 |
| Fig 3.5.1 | Project cash-flow curve and the funding trough *(animated)* | 3.5 |
| Fig 4.2.1 | Budget-to-actual variance bridge *(animated)* | 4.2 |
| Fig 5.3.1 | Control account as the integration point *(animated)* | 5.3 |
| Fig 6.1.1 | The three EVM curves at the data date *(animated)* | 6.1 |
| Fig 6.3.1 | The EAC fan *(animated)* | 6.3 |
| Fig 7.1.1 | The contract risk-allocation spectrum *(animated)* | 7.1 |
| Fig 8.1.1 | Power/interest stakeholder grid *(animated)* | 8.1 |
| Fig 8.6.1 | The development-approach spectrum *(animated)* | 8.6 |
| Fig 8.6.2 | Incremental vs iterative *(animated)* | 8.6 |
| Fig 9.3.1 | Release burnup with a moving scope line *(animated)* | 9.3 |
| Fig 10.2.1 | The activity network and critical path *(animated)* | 10.2 |
| Fig 12.2.1 | Probability–impact matrix *(animated)* | 12.2 |
| Fig 13.1.1 | The AI landscape *(animated)* | 13.1 |
| Fig 13.2.1 | Data quality and lineage for a controls AI workflow *(animated)* | 13.2 |
| Fig 13.4.1 | Capability-vs-category matrix *(animated)* | 13.4 |
| Fig 13.5.1 | AI across the controls lifecycle *(animated)* | 13.5 |
| Fig 13.6.1 | AI-governance decision flow *(animated)* | 13.6 |
| Fig 13.7.1 | The AI-maturity ladder *(animated)* | 13.7 |

Illustration style throughout: brand blue `#1D4ED8`, Plus Jakarta Sans labels, clean professional diagrams
(Style Spine §6). Animations are **digital/LMS-only**; the print/PDF uses the static figure rendered from the
same spec.

---

## Appendix E — Self-check answers

Self-check questions are placed at the end of each Knowledge Area with their **answers inline in parentheses**
(so a reader can self-mark immediately). This appendix confirms that convention; on consolidation into the
print/PDF edition, the inline answers are collected here by KA number and removed from the body for a
"questions first, answers at the back" study format. Until then, refer to each KA's self-check block.

---

## Appendix F — Sample-MCQ bank

Every Knowledge Area ends with **3–8 sample MCQs** (four options, correct answer marked, rationale, tagged with
topic number and cognitive level), per Style Spine §8; Domains 11–13 carry additional items following the
review board's blueprint-alignment recommendation (the AI domain examines at 20 %). Consolidated counts
(draft):

| Domain | KAs | Sample MCQs |
|---|---:|---:|
| 1 Foundations of Accounting | 5 | 30 |
| 2 Financial Reporting (IFRS 15) | 5 | 27 |
| 3 Budgeting & Forecasting | 5 | 23 |
| 4 Performance/Variance/Reporting | 4 | 18 |
| 5 Cost Management | 4 | 18 |
| 6 EVM/EAC | 4 | 19 |
| 7 Contracts & Commercial | 5 | 22 |
| 8 PM Lifecycle | 6 | 23 |
| 9 Agile & Adaptive | 6 | 26 |
| 10 Scheduling | 4 | 18 |
| 11 Process Cycles | 3 | 16 |
| 12 Risk Management | 3 | 17 |
| 13 AI for Project Controls | 7 | 52 |
| **Total (draft)** | **61** | **309** |

> **Blueprint separation.** These are **study/sample items** drawn from the same blueprint as, but kept
> **separate from**, the live examination bank; they are **not** to be reused verbatim as live exam questions
> (Style Spine §8). On consolidation they are tagged to topic numbers and cognitive levels to map to the exam
> blueprint, and reviewed by SMEs (finance, agile, AI) before any use.

---

## Appendix G — The integrated capstone: one project, thirteen domains

The domains teach the machinery one discipline at a time; a real month-end exercises all of it at once. This
capstone follows a single project — the **master project** of Domain 6, with its baseline first phased in
Domain 3 (KA 3.3.3): `BAC` **USD 1,000,000** over **ten months** — through one full reporting cycle at the
end of **Month 5**, extended here with the commercial, revenue, risk and AI facts the other domains supply.
Every figure either comes from a domain worked example or is derived by that domain's method, and every step
is cross-referenced by topic number; nothing is introduced that the domains have not already taught. Work the
eight stations in order — each is what the professional actually does, in the order a close actually runs.

### Station 1 — Budget and baseline (Domain 3)

**What the professional does.** Assemble the budget bottom-up and make it a control. The control-account
budgets are summed; the **contingency reserve** — funding the identified risks of Station 6 — sits **inside**
the baseline; the **management reserve** sits **outside** it, management-controlled (3.1.4; 12.3.2). The
`BAC` is then phased across the ten-month schedule to the S-curve of 3.3.3, which *is* the Planned Value.

```
BAC           = Σ control-account budgets + contingency = 915,000 + 85,000 = 1,000,000     (3.1.4)
Total budget  = BAC + management reserve = 1,000,000 + 50,000 = 1,050,000                  (3.1.4)
PV at Month 5 = 40 + 70 + 110 + 140 + 160 = 520 (USD 000) → PV = 520,000                   (3.3.3)
```

The Month-5 `PV` of USD 520,000 is read straight off the 3.3.3 phasing table — the same S-curve on which
Domain 6 later draws `EV` and `AC` (6.1.3), and whose cash consequences Domain 3 forecasts separately (3.5).

### Station 2 — The cost states (Domain 5)

**What the professional does.** Before computing any index, make cost-to-date *true*. At Month 5 the cost
ledger shows purchase orders and subcontracts raised (commitments) of **USD 640,000**; invoices processed
(actuals) of **USD 410,000**; and goods/services received but not yet invoiced (accruals) of **USD 120,000**.
*Assumption (stated per the Style Spine §5): all accrued work is under the raised POs, so the accrual reduces
the open commitment.*

```
AC (cost-to-date)  = actuals + accruals = 410,000 + 120,000 = 530,000            (5.2.1–5.2.2)
Open commitment    = commitments − actuals − accruals
                   = 640,000 − 410,000 − 120,000 = 110,000                       (5.2.2)
Forecast committed = 530,000 + 110,000 + estimate for uncommitted scope          (5.2.1)
```

This is the `AC` = 530,000 that Domain 6 uses (6.1.1) — the accrual discipline is what makes it true. An
invoice-only view would report 410,000 and flatter the cost index to `480,000 / 410,000 = 1.17`; the
disciplined figure tells the real story at Station 3.

### Station 3 — Earned value and the forecast (Domain 6)

**What the professional does.** Measure `EV` from physical progress under the fixed earning rules (6.1.2) —
at Month 5, `EV` = **480,000** — then compute, exactly as Domain 6 publishes them:

```
CV  = EV − AC = 480,000 − 530,000 = (50,000)   over cost                          (6.2.1)
SV  = EV − PV = 480,000 − 520,000 = (40,000)   behind schedule                    (6.2.1)
CPI = 480,000 / 530,000 = 0.91 (0.9057 unrounded);  SPI = 480,000 / 520,000 = 0.92   (6.2.2)
EAC (a) budgeted rate = 530,000 + (1,000,000 − 480,000) = 1,050,000               (6.3.2)
EAC (b) current CPI   = 1,000,000 / 0.9057 = 1,104,167                            (6.3.2)
EAC (c) CPI × SPI     ≈ 1,152,010                                                 (6.3.2)
TCPI (to BAC) = 520,000 / 470,000 = 1.11;  VAC = 1,000,000 − 1,104,167 = (104,167)  (6.2.3, 6.3.4)
```

The cost inefficiency is systemic (a stable, sliding `CPI` trend), so method (b) is the defended forecast
(6.3.3): **`EAC` = USD 1,104,167**. The `TCPI` of 1.11 against a delivered 0.91 says the `BAC` is no longer
credible (6.2.3) — the number Stations 5 and 7 must now carry, not hide.

### Station 4 — The commercial position (Domain 7)

**What the professional does.** Run the payment cycle. The client contract is **remeasured** (7.1.2) at a
contract price of **USD 1,250,000**, with **5 % retention** and a **10 % mobilisation advance** recovered
pro-rata on each certificate (7.4.3). At Month 5 the client's QS certifies gross work done of **USD 460,000**
(7.4.2 — the certified measure, not the application, drives cash).

```
Retention        = 5 % × 460,000 = 23,000                                        (7.4.3)
Advance recovery = 10 % × 460,000 = 46,000                                       (7.4.3)
Net certified    = 460,000 − 23,000 − 46,000 = 391,000                           (7.4.3)
EV − certified gross = 480,000 − 460,000 = 20,000                                (7.4.4)
```

The USD 20,000 by which `EV` runs ahead of certification is work performed but not yet certified — unbilled
performance pointing to a **contract asset** (7.5.2), measured at accounting values in Station 5. `EV` (at
budget) and the valuation (at contract rates) are different measures of the same progress, so the gap is read
through the three-way reconciliation of 7.4.4, never forced to zero.

### Station 5 — Revenue recognised (Domain 2)

**What the professional does.** Hand finance a defensible input-method ratio. Revenue is recognised over time
by **cost-to-cost** (2.2.6), and the denominator is the controls forecast — the Station 3 `EAC` — so the
professional's forecast flows straight into reported revenue.

```
PoC            = costs to date / total estimated cost = 530,000 / 1,104,167 = 48.0 %   (2.2.6)
Revenue        = 48.0 % × 1,250,000 ≈ 600,000  (0.4800 × 1,250,000 = 600,000)          (2.2.6)
Contract asset = 600,000 − 460,000 = 140,000   (under-billed)                          (2.2.7, 7.5.2)
Margin to date = 600,000 − 530,000 = 70,000 (11.7 %)
Margin at completion = 1,250,000 − 1,104,167 = 145,833 (11.7 %)
```

The margin check ties: cost-to-cost makes the to-date margin equal the forecast completion margin — if they
diverge, the ratio and the `EAC` have come apart. The contract remains profitable, so no onerous-contract
provision arises (2.2.6 loss rule; 1.4.5); but the USD 140,000 contract asset — performance ahead of billing —
is aged and explained, not just reported (7.5.2).

### Station 6 — Risk and the contingency test (Domain 12)

**What the professional does.** Re-run the register, not the opening story. Contingency was set at sanction
at the Monte Carlo **P80 of USD 85,000** (12.3.1) — the Station 1 figure inside the `BAC` — and materialised
risks have drawn **USD 25,000** through change control (5.4). The Month-5 re-run prices the remaining
register at an **EMV of USD 45,000** (12.2.3) and a **P80 of USD 70,000** — the P80 sits above the EMV
because the register's risks share drivers, and correlation moves the tail, not the mean (12.3.1;
Exercise 12.5).

```
Remaining contingency = 85,000 − 25,000 = 60,000                                 (12.3.3)
Remaining exposure    = P80 70,000  (register EMV 45,000)                        (12.2.3, 12.3.1)
Adequacy test: 60,000 < 70,000 → shortfall (10,000) — flag and escalate          (12.3.3)
```

The test is always **remaining contingency versus remaining exposure** (12.3.3). The fund no longer covers
the analysed exposure, so the position is escalated visibly; if the register outgrows contingency, reaching
for the management reserve is a re-baselining event, not a silent overspend (12.3.2; 5.4).

### Station 7 — The report and the decisions (Domains 4 and 8)

**What the professional does.** Turn the numbers into a decision. Against the reporting tolerances declared
in advance — `CPI` amber at 0.95, red at 0.90 (4.1.1) — the month-end pack reports by exception (4.1.3, 4.3.5):

```
Exception entry : CPI 0.91 breaches the 0.95 amber threshold (just above the 0.90 red line)
                  — variance (50,000), cause, impact, action/owner                (4.1.1, 4.3.5)
EAC bridge line : BAC 1,000,000 → EAC 1,104,167 on the persisting-CPI basis; VAC (104,167)   (6.3.4)
Contingency line: remaining 60,000 vs remaining exposure 70,000 — escalation flagged         (12.3.3)
```

The narrative is decision-ready, not merely descriptive (4.3.3): status, cause, forecast, action. The gate
implication (8.4; Advanced 8.A.3) follows: the next gate cannot proceed on the `BAC` — the pack presents the
`EAC` with its method and assumption, the `TCPI` reality-check on any recovery claim, and the choice the
board actually owns: fund a specific recovery, or re-baseline through the management reserve.

### Station 8 — The AI-assisted close (Domain 13)

**What the professional does.** Run the same cycle faster — and sign it. Each station above used a governed
AI step, and at each one a named professional disposed of what the AI proposed (13.6.1):

| Station | Governed AI step | The professional signed |
|---|---|---|
| 1–2 | Auto-coding of cost, proposed month-end accruals, duplicate/anomaly flags (13.5.4) | The accrual judgements and every flagged exception |
| 3 | `EAC` driver analysis and the `CPI`-trend early warning (13.5.3) | The method choice (b) and the defended forecast |
| 4 | Extraction of certificate, retention and advance terms from the contract documents (13.5.7) | The certified measure and the valuation cascade |
| 5 | Drafted revenue workings and consistency checks tying cost, billing and revenue (13.5.10) | The PoC ratio, its `EAC` denominator and the recognition judgement |
| 6 | Register scoring and the Monte Carlo re-run (13.5.9) | The adequacy verdict and the escalation |
| 7 | Drafted exception narratives and dashboard assembly (13.5.8) | Accuracy, framing and the final sign-off |

Every step ran inside the guardrails: verified against source before use (13.6.5's checklist), with the audit
trail of what the AI produced, who approved it and what changed (13.6.2). **AI proposes; the professional verifies, decides and remains accountable** — at every station, without exception.

### The PCI control cycle

Read back-to-back, the eight stations show why the thirteen domains are one body of knowledge rather than a
shelf of techniques. A single month of one project generated every number above, and each domain valued the
*same physical progress* under its own rules: the budget phased it (Domain 3), the ledger costed it with
commitments and accruals (Domains 1, 5, 11), earned value measured it at budget (Domain 6) against a schedule
that sequenced it (Domains 8, 10), the valuation billed it at contract rates (Domain 7), IFRS 15 recognised it
as revenue (Domain 2), the register funded its uncertainty (Domain 12), the report turned it into a decision
(Domain 4), and AI accelerated every step under governance (Domain 13). The figures differ by design — `EV`
480,000, certified 460,000, revenue 600,000 — and the professional's craft is the reconciliation between them
(7.4.4; 7.5.3's commercial-to-accounting loop), not the pretence that they should agree. Notice, too, how the
forecast is the hinge: the `EAC` chosen in Station 3 set the revenue ratio in Station 5, sized the gate
conversation in Station 7, and framed the contingency question in Station 6. One defensible forecast,
consistently applied, is what makes the whole cycle honest; a flattered one corrupts every station downstream.
That interlock — cost, schedule, commercial, accounting, risk, reporting and AI governance meeting in one
data date — is precisely the integrated judgement the credential certifies.

### Reflection questions

1. The invoice-only ledger showed `AC` = 410,000 (a `CPI` of 1.17); the accrued figure showed 530,000 (0.91).
   Trace every later station that would have been corrupted by the flattering figure — forecast, revenue,
   margin, gate — and name the domain discipline that prevented it. *(5.2; then 6.3, 2.2.6, 4.3.5, 8.4.)*
2. `EV` (480,000), certified billing (460,000) and recognised revenue (600,000) all measure Month 5's
   progress. State what each values, why none is "wrong", and which balance-sheet line carries the gap
   between the last two. *(7.4.4; 2.2.7 / 7.5.2 — a USD 140,000 contract asset.)*
3. Remaining contingency (60,000) fails the test against remaining exposure (P80 70,000) even though it
   comfortably exceeds the register's EMV (45,000). Explain, using Exercise 12.5's logic, why the P80 — not
   the EMV — is the right comparator, and what governance route the shortfall takes. *(12.3.1, 12.3.3, 12.3.2.)*

---

*All appendices are draft, regenerated from the domains as they pass SME review. The formula sheet, glossary,
standards index and figure index above are authoritative for the currently-drafted domains (1–13).*
