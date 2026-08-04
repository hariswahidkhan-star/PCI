# Appendices — PCL-AI Body of Knowledge

> Back-matter drawn from the thirteen domains and indexed to them, so that any formula, term, standard,
> figure, self-check answer or sample question can be found from one place and traced back to the Knowledge
> Area that teaches it. Contents: A) master formula sheet · B) global glossary · C) standards & frameworks
> referenced · D) figure & animation index · E) self-check answers · F) sample-MCQ bank · G) the integrated
> capstone.

---

## Appendix A — Master formula sheet

All symbols are defined once and used identically across the book (Conventions, §4). Currency in USD
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
| `TCPI (to EAC) = (BAC − EV) / (EAC − AC)` | To-complete performance index, to meet a revised EAC | 6.2.3 |
| `EAC = AC + ETC` | Estimate at completion (identity) | 6.3.1 |
| `EAC = AC + (BAC − EV)` | EAC, remaining work at budgeted rate | 6.3.2 |
| `EAC = BAC / CPI` | EAC, remaining work at current CPI | 6.3.2 |
| `EAC = AC + (BAC − EV) / (CPI × SPI)` | EAC: cost & schedule compound | 6.3.2 |
| `VAC = BAC − EAC` | Variance at completion | 6.3.4 |
| `ES = M + (EV − PV_M) / (PV_M+1 − PV_M)` | Earned schedule: interpolate between the months bracketing EV (cumulative PV_M ≤ EV ≤ PV_M+1) | 6.4.3 |
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

Consolidated from every domain's key-terms box: **280 terms**, each shown with the KA where it is first
defined. Where a term recurs across domains, the first definition governs (Conventions, §3).

| Term | Definition | First defined |
|---|---|---|
| **100 % rule** | The WBS captures all of the scope and only the scope. | 8.2 |
| **`CPI` / `SPI`** | Cost / schedule performance index (`EV/AC`, `EV/PV`). | 3.4 |
| **`CV` / `SV`** | Cost variance (`EV − AC`) / schedule variance (`EV − PV`). | 6.2 |
| **`EAC` / `ETC`** | Estimate at completion / to complete; `EAC = AC + ETC`. | 3.4 |
| **`SPI` convergence** | The tendency of `SPI` → 1 at completion regardless of lateness. | 6.4 |
| **`TCPI`** | To-complete performance index: the efficiency remaining work must achieve for a target. | 6.2 |
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
| **Agile contracting** | T&M, capped T&M, target cost: forms that fit variable scope. | 9.6 |
| **Agile mindset / Manifesto** | Valuing working outcomes, collaboration, value and responsiveness to change. | 9.1 |
| **AgileEVM** | Earned value applied to variable-scope adaptive delivery (Domain 9). | 6.4 |
| **AI / ML / GenAI** | The field / learning-from-data subset / content-generating subset (nested). | 13.1 |
| **AI proposes; the professional verifies, decides and remains accountable** | AI drafts/predicts; a qualified professional decides and is accountable. | 13.6 |
| **AI-assisted disclosure/forecast** | AI-drafted output the professional verifies and signs off. | 13.5 |
| **AI-maturity model** | Ad-hoc → piloting → standardised → integrated → governed/optimised. | 13.7 |
| **AI-use policy / verification checklist** | The governance document and the operational assurance step. | 13.6 |
| **Allowable (defined) cost** | The subset of incurred cost a contract makes reimbursable: the base for fee, share and pain/gain arithmetic. | 7.1 |
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
| **Change authority** | The person or body holding the delegated power to approve a change of a stated value and type, never the person who assessed it. | 5.4 |
| **Change control** | The process to identify, assess, approve/reject and baseline change. | 5.4 |
| **Change control board (CCB)** | The standing body, or named individual, holding authority to approve change within stated bands. | 8.4 |
| **Change log** | The record reconciling current baseline to original, by change. | 5.4 |
| **Change management** | Bringing people with you; honest about limits, resistant to hype. | 13.7 |
| **Change request** | The controlling artefact for a change: identifier, cause, cross-constraint assessment, options, funding source and approval record. | 8.4 |
| **Chart of accounts (CoA)** | The structured list of all ledger accounts, coded by class. | 1.5 |
| **Chart-to-question fit** | Choosing the chart type that answers the actual question. | 4.4 |
| **Claim** | A notified, substantiated assertion of entitlement to time/money. | 7.2 |
| **Closing** | Formal completion: acceptance, contract closure, demobilisation, archiving. | 8.5 |
| **Commercial-to-accounting loop** | Scope → cost → EV → billing → revenue → statements → reporting. | 7.5 |
| **Commitment** | Cost the organisation is bound to once a PO/subcontract is raised. | 5.2 |
| **Condition precedent (to a claim)** | A contractual requirement (commonly a notice within a stated window) drafted so that the entitlement does not survive if it is not met. | 7.2 |
| **Constraint trade-offs** | Scope/schedule/cost/quality/risk balanced against each other. | 8.4 |
| **Contingency reserve** | For identified risks; inside the baseline; PM-controlled. | 3.1 |
| **Contingent liability / asset** | A possible obligation/inflow: disclosed, not recognised (subject to probability). | 1.4 |
| **Contract asset / liability** | Revenue recognised vs amounts billed: under-billing (asset) vs over-billing (liability). | 2.2 |
| **Control account (CA)** | The WBS×OBS intersection where scope, budget, cost and schedule integrate. | 1.5 |
| **Controls dashboard** | An integrated cost/schedule/forecast/risk view with RAG status and trend. | 4.3 |
| **COSO Internal Control — Integrated Framework** | The most widely used articulation of internal-control objectives and components; a voluntary framework published by a private-sector body, adopted rather than imposed. | 11.3 |
| **Cost baseline / PMB** | The approved, time-phased budget; source of Planned Value; total = BAC. | 3.1 |
| **Cost breakdown structure (CBS)** | Decomposition of cost by element/type. | 1.5 |
| **Cost code** | A segmented code pinning a posting to project, scope, cost element and resource. | 1.5 |
| **Cost driver** | The factor that causes a cost to change. | 5.1 |
| **Cost extraction / reconciliation** | Pulling cost from source systems and tying it back to the ledger. | 1.5 |
| **Cost-plus (CPFF/CPIF/CPAF)** | Reimburse cost plus a fixed/incentive/award fee; client bears cost risk. | 7.1 |
| **Cost-schedule integration** | Measuring both from one `EV` so they cannot contradict. | 6.4 |
| **Cost-to-date (control)** | Actuals + accruals; the figure `AC` should reflect. | 5.2 |
| **Crashing** | Add resources to critical activities (cost for time). | 10.3 |
| **Credit control** | Assessing/limiting customer credit before committing. | 11.1 |
| **Critical path** | The longest, zero-float chain; sets the project duration. | 10.2 |
| **Cumulative flow / throughput / cycle time** | Work-state bands / items per period / start-to-done time. | 9.3 |
| **Current vs non-current** | The IAS 1 split by expected realisation/settlement within ~12 months. | 1.2 |
| **Cut-off** | Recording each transaction in the correct period. | 1.3 |
| **Data integrity** | Ongoing correctness of the cost data (coding, commitments, accruals, one source). | 5.2 |
| **Data minimisation** | Using only the fields a task needs, preferring aggregated or pseudonymised data. | 13.2 |
| **Data quality dimensions** | Accuracy, completeness, consistency, timeliness, validity, uniqueness. | 13.2 |
| **Data-protection impact assessment** | A formal assessment of a higher-risk processing activity, undertaken before it begins, on the data-protection function's determination. | 13.2 |
| **Debit / Credit** | Left/right sides of an account; effect depends on account type. | 1.1 |
| **Delegated draw limit** | The recorded value within which the project manager may approve a contingency draw; above it, the sponsor approves, and the requester is never the approver. | 12.3 |
| **Delegated limit** | The recorded value band within which a named role may approve a change; above it, the change escalates. | 5.4 |
| **Dependency (FS/SS/FF/SF)** | The four logical relationships between activities. | 10.1 |
| **Depreciation** | Systematic spreading of a long-lived asset's cost over its useful life. | 1.3 |
| **Direct / indirect cost** | Traceable to one cost object / supporting many (overhead). | 5.1 |
| **Disallowed cost** | Cost falling in a category a contract excludes; borne by the contractor and outside the share mechanism. | 7.1 |
| **Discount unwind** | The increase in a discounted provision as settlement nears, charged as finance cost. | 1.4 |
| **Double-entry** | Recording every transaction with `Σ Dr = Σ Cr`. | 1.1 |
| **Draw-down / re-baselining** | Consuming contingency as risks occur / escalating beyond it as a baseline change. | 12.3 |
| **Draw-down request** | The controlling artefact for a contingency draw: date, requester, register ID, evidence of materialisation, amount and its substantiation, and the revised remaining exposure. | 12.3 |
| **Driver analysis** | AI explanation of *why* a metric is moving (e.g. an EAC). | 13.5 |
| **Duty to escalate** | The professional obligation to raise, in writing and before issue, a figure or narrative that cannot be defended on the evidence. | 4.3 |
| **EAC (a)–(d)** | Budgeted-rate / current-CPI / CPI×SPI / bottom-up methods, each an assumption. | 6.3 |
| **Earned schedule (`ES`)** | Earned value expressed as a point on the time axis; gives `SV(t)`, `SPI(t)`. | 6.4 |
| **Earned Value (`EV`)** | Budgeted cost of work performed: progress valued at budget. | 6.1 |
| **Earning rule / measurement method** | The rule converting physical progress to `EV` (0/100, 50/50, % complete, units, milestones). | 6.1 |
| **Embedded AI** | AI features within the platforms controls already uses. | 13.4 |
| **Emergency change** | A change authorised out of sequence for genuine urgency, recorded as such at the time and ratified by the proper authority afterwards. | 8.4 |
| **Empirical process control** | Decisions from observation via transparency, inspection, adaptation. | 9.1 |
| **Enhancing characteristics** | Comparability, verifiability, timeliness, understandability. | 2.1 |
| **Entity concept** | The business is accounted for as separate from its owners. | 1.1 |
| **EPC / turnkey** | Single-contractor delivery of the whole asset. | 7.1 |
| **Escalation record** | The contemporaneous file of what was raised, on what evidence, to whom, when, and the response received. | 4.3 |
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
| **Gross-up** | A contractual entitlement to be paid an increased amount so that, after a withholding, the recipient still receives the invoiced sum. | 3.5 |
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
| **Incremental** | Building the product in usable slices, adding parts to the whole. | 8.6 |
| **Input (cost-to-cost) / output method** | Ways of measuring progress toward complete satisfaction. | 2.2 |
| **Integrated change control** | Assessing every change across all constraints before approval. | 8.4 |
| **Integration / upskilling** | Embedding AI in the workflow / building data, prompting, verification, governance skills. | 13.7 |
| **Interim valuation / application** | Periodic assessment of value done, less retention and prior payments. | 7.4 |
| **Internal control** | Policies/procedures giving assurance over reporting, operations, compliance. | 11.3 |
| **Inverted iron triangle** | Fixed time & cost, variable scope. | 9.3 |
| **Irrecoverable input tax** | Value-added or goods-and-services tax an entity cannot recover, and which is therefore a cost of the project rather than a timing effect. | 3.5 |
| **ISO 31000** | A voluntary international standard offering guidance — not requirements — on integrated, proportionate risk management; not intended for certification, and of no force unless adopted or imported by a contract or mandate. | 12.1 |
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
| **Measure of progress (recognition)** | The basis on which progress towards satisfying a performance obligation is measured: an accounting-policy judgement owned by finance, not by project controls. | 9.5 |
| **Measurement** | Deriving quantities from design under a standard method. | 7.3 |
| **Milestone-to-Sprint mapping** | Translating Sprints/releases into gate milestones. | 9.6 |
| **Monitoring & controlling** | Measuring against baselines and acting to correct: parallel to executing. | 8.4 |
| **Normal balance** | The side on which an account type increases. | 1.1 |
| **Notice register** | The commercial record of every notice due and served, with its trigger, window, owner, date and acknowledgement. | 7.2 |
| **Offsetting** | Netting assets/liabilities or income/expenses: generally prohibited. | 2.1 |
| **Onerous contract** | Unavoidable costs exceed expected benefits; the loss is provided immediately. | 1.4 |
| **Open book / audit rights** | A client's contractual right to inspect and audit the records evidencing a reimbursable claim, for a stated period. | 7.2 |
| **Open PO / GRNI** | Commitment / goods-received-not-invoiced (accrual driver). | 11.2 |
| **Order-to-cash (O2C)** | Order → credit → fulfil → invoice → collect → apply cash. | 11.1 |
| **Over time vs point in time** | Whether an obligation is satisfied progressively or at a moment of control transfer. | 2.2 |
| **Over/under-absorption** | Overhead absorbed minus overhead actually incurred. | 5.1 |
| **Overhead absorption rate (OAR)** | Budgeted overhead ÷ budgeted activity base. | 5.1 |
| **Overview-first, detail-on-demand** | Summary on one view, drill-down to the detail behind any red. | 4.3 |
| **P80 contingency** | Contingency set at an 80 %-confidence outcome from a risk model. | 12.3 |
| **Payment terms** | The lag between billing and collection (and between receipt and paying suppliers). | 3.5 |
| **Peak funding requirement** | The deepest point of cumulative cash: the finance to arrange. | 3.5 |
| **Performance / advance-payment / retention bond** | Third-party security instruments. | 7.2 |
| **Performance obligation** | A promise to transfer a distinct good or service. | 2.2 |
| **Personal data** | Information about identified or identifiable people; a separate test from confidentiality, carrying obligations that vary by jurisdiction. | 13.2 |
| **PERT (three-point)** | `tE = (O + 4M + P)/6`; `σ = (P − O)/6`. | 10.1 |
| **Planned Value (`PV`/BCWS)** | Cumulative planned spend to date: the cost-baseline curve; the budgeted cost of work scheduled by the data date. | 3.3 |
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
| **Purpose limitation** | The principle that data collected for one stated purpose is not automatically available for a different one. | 13.2 |
| **Qualitative analysis** | Probability × impact rating on a matrix. | 12.2 |
| **Quality assurance** | Building quality into the process (vs inspecting it out). | 8.3 |
| **Quantity/efficiency variance** | `(Actual quantity − Standard quantity) × Standard price`. | 4.2 |
| **Rate** | Price per unit (labour, materials, plant, overhead, profit). | 7.3 |
| **Rebaselining** | Transparently resetting scope/`BAC` when scope is deliberately flexed. | 9.5 |
| **Receivables ageing** | The overdue profile of amounts owed: a cash/revenue leading indicator. | 11.1 |
| **Reconciliation** | Explaining the tie between the two views from one ledger. | 2.5 |
| **Records custodian** | The named holder of a project archive in the permanent organisation once the project team disbands. | 8.5 |
| **Relevance / materiality** | Capable of influencing decisions; material if its omission/misstatement would. | 2.1 |
| **Remeasurement (contract form) / unit-rate** | Priced at rates against re-measured actual quantities. | 7.1 |
| **Remeasurement (of a BoQ)** | Re-pricing at actual quantities. | 7.3 |
| **Resource levelling / smoothing** | Respect resource limits (may extend) / even peaks within float (no extension). | 10.3 |
| **Response strategies** | Avoid/transfer/mitigate/accept (threats); exploit/share/enhance/accept (opportunities). | 12.2 |
| **Retention** | Cash withheld from payments until completion/defects periods pass. | 3.5 |
| **Retention period (records)** | The stated period for which project records are kept: the longest of the contract, claim-limitation, accounting/tax and funder requirements, confirmed rather than assumed. | 8.5 |
| **Risk** | An uncertain event/condition affecting objectives: threat or opportunity. | 12.1 |
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
| **SOCE** | Statement of changes in equity: reconciles opening to closing equity. | 1.2 |
| **SOFP** | Statement of financial position: assets, liabilities, equity at a point in time. | 1.2 |
| **SOPL & OCI** | Statement of profit or loss and other comprehensive income: performance over a period. | 1.2 |
| **Sprint** | The fixed short cycle containing all other events; produces a usable Increment. | 9.2 |
| **Stakeholder** | Anyone who affects or is affected by the project. | 8.1 |
| **Standalone selling price (SSP)** | The price of a good/service sold separately; the basis for allocation. | 2.2 |
| **Statement of cash flows** | Change in cash split into operating, investing and financing. | 1.2 |
| **Statutory reporting** | Standards-compliant, audited external reporting. | 2.5 |
| **Story point / velocity** | Relative size unit / average points completed per Sprint. | 9.3 |
| **Storytelling** | Ordering true facts (status → cause → forecast → action) to drive a decision. | 4.4 |
| **Structured / unstructured** | Tabular (ML) vs free-form (GenAI/RAG) data. | 13.2 |
| **Subsidiary plans** | Quality, resource, risk, procurement, communications, integration. | 8.2 |
| **Success criteria** | The definition of doing well, beyond scope/time/cost to benefit and quality. | 8.1 |
| **Supervised / unsupervised / reinforcement** | Learn from labels / structure / trial-and-reward. | 13.1 |
| **Tailoring** | Adapting the approach to the specific project. | 8.6 |
| **Target / threshold / tolerance** | The aim / the attention boundary / the allowable deviation. | 4.1 |
| **Target cost / pain-gain** | Shared cost risk against a target, within a cap/collar. | 7.1 |
| **Three-way match** | Matching invoice to PO and goods-receipt note before payment. | 11.2 |
| **Time bar** | The point at which a notice window closes, after which a claim may not be assessed on its merits at all. | 7.2 |
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

Named at principle level; **never reproduced verbatim** (Conventions, §9). Real frameworks only; no fabricated
citations. Each entry is tagged with what kind of authority it is, because the distinction changes what a
professional owes it: an accounting standard that governs a set of financial statements is not the same
obligation as a voluntary framework a project chooses to adopt. **The official publication governs in every
case**; the descriptions here are this book's own words, they are not authoritative, and no requirement
should be inferred from them. **No issuing body named in this table is associated with, endorses, has
accredited or has reviewed this book, the designation it supports or this programme.**

**Currency of these references.** Standards change. Editions, effective dates and titles stated anywhere in
this volume were checked against the issuing bodies' catalogues when this edition was prepared, and a reader
relying on any of them for a live decision should verify the current requirement with the issuing body.

**Not in this table: PCI's own instrument.** The identifiers `PCI-FND-STD-NN` and `PCI-PCL-STD-DD.NN` that
appear at the close of each domain are **PCI Standards**: the Institute's own companion instrument
(Conventions, §11), not external references. They are private professional requirements binding within PCI's
certification scope; they are not legislation, and they neither derive from nor stand behind any instrument
listed below. The two are indexed separately on purpose, so that a PCI requirement is never mistaken for an
external one, or the reverse.

| Standard / framework | Category | Used for | Domains |
|---|---|---|---|
| **IAS 1** | Authoritative accounting standard | Presentation of financial statements | 1.2, 2.1 |
| **IAS 2** | Authoritative accounting standard | Inventories (lower of cost and NRV) | 2.4 |
| **IAS 16** | Authoritative accounting standard | Property, plant & equipment (capitalise/depreciate) | 1.3, 2.4 |
| **IAS 23** | Authoritative accounting standard | Borrowing costs (capitalise on qualifying assets) | 2.4 |
| **IAS 37** | Authoritative accounting standard | Provisions, contingent liabilities/assets; onerous contracts | 1.4, 2.2, 2.4 |
| **IAS 11 (legacy)** | Superseded accounting standard | Construction contracts: superseded by IFRS 15 | 2.4 |
| **IFRS 15** | Authoritative accounting standard | Revenue from contracts with customers (five-step model) | 2.2, 2.3, 7.5, 9.5 |
| **IFRS 16** | Authoritative accounting standard | Leases (right-of-use asset & lease liability) | 2.4 |
| **ASC 606 (US GAAP)** | Authoritative accounting standard | The revenue model applied by US-GAAP preparers; named as the equivalent framework to IFRS 15, developed jointly with it | 9.5 |
| **PMBOK Guide** | Professional guidance | Project management process groups & practices | 8 |
| **AACE TCM Framework / estimate classes** | Professional guidance | Total cost management; estimate classification | 3.2 |
| **ANSI/EIA-748** | National standard *(voluntary in itself; reaches a project only where a contract or procurement regime imports it)* | The characteristics an earned-value management system is expected to exhibit: the source of the guideline-based "EVMS compliance" vocabulary and of the formal reprogramming practice (over-target baseline) | 6.A.1, 6.A.2 |
| **ISO 21508** | International standard (guidance, not certifiable) | Earned value management in project and programme management | 6.A.2 |
| **PMI practice guidance on earned value management** | Professional guidance | Recommended practice on earned value for practitioners | 6.A.2 |
| **SCL Delay and Disruption Protocol** | Professional guidance *(voluntary; binds nobody unless a contract imports it)* | A taxonomy of delay-analysis methods and the vocabulary practitioners use for them | 10.A.6 |
| **AACE recommended practice on forensic schedule analysis** | Professional guidance *(voluntary; binds nobody unless a contract imports it)* | A second widely cited taxonomy of forensic schedule-analysis methods | 10.A.6 |
| **COSO Internal Control — Integrated Framework** | Voluntary framework *(published by a private-sector body; adoption is the whole of its force)* | The most widely used articulation of internal-control objectives and components | 11.3.1, 11.T.1–11.T.2 |
| **ISO 31000** | International standard (guidance, not requirements; not intended for certification) | Risk management principles & process | 12 |
| **ISO/IEC 17024** | International standard | Personnel certification (credential design reference) | — |
| **Agile Manifesto** | Voluntary framework | Agile values & principles (described, not reproduced) | 9.1 |
| **Scrum Guide** | Voluntary framework | Scrum accountabilities, events, artefacts, commitments | 9.2 |
| **Kanban / Lean** | Industry practice | Flow and waste-reduction principles | 9.4 |
| **SAFe / LeSS / Scrum-of-Scrums** | Voluntary framework | Scaling frameworks (awareness level) | 9.4 |
| **FIDIC (and jurisdictional forms)** | Contract framework | Standard contract forms (awareness level) | 7.2 |
| **ISO/IEC 42001** | International standard (certifiable management-system standard) | An AI management system: governance, roles, controls and improvement around AI use | 13.6 |
| **ISO/IEC 23894** | International standard (guidance, not certifiable) | Guidance on managing risk in the context of AI | 13.6 |
| **NIST AI Risk Management Framework** | Voluntary framework | AI risk management by four functions (govern, map, measure, manage); not a standard and not a regulation | 13.6 |
| **OECD AI Principles** | Voluntary framework (an OECD Council Recommendation) | Intergovernmental principles for trustworthy AI; a statement of principle rather than legislation | 13.6 |
| **EU AI Act** | Illustrative reference *(understood to be legislation of the European Union)* | Named as the shape AI governance must anticipate; whether, when and how it reaches any organisation is a question for legal and compliance, not for this book | 13.6 |
| **EU General Data Protection Regulation** | Illustrative reference *(understood to be legislation of the European Union)* | Named once, to identify the kind of instrument meant by "data-protection law"; whether it or any other regime reaches a given organisation, activity or data set is a question for the data-protection function and qualified counsel, not for this book | 13.2.5 |
| **US banking supervisors' guidance on model risk management** | Illustrative reference *(understood to be supervisory guidance, jurisdiction-specific)* | Model risk management as an established discipline; addressed to the firms those supervisors supervise | 13.A.2 |

> **Forthcoming change — IFRS 18.** A further standard, IFRS 18 *Presentation and Disclosure in Financial
> Statements*, has been issued and is expected to change how financial statements are presented, including the
> treatment of management-defined performance measures. The presentation principles this book teaches at 1.2
> and 2.1 (that a reader must be able to see what an entity owns, owes, earned and spent, on a consistent
> basis between periods) are unaffected. Its scope, its effective date and what it means for any particular
> entity are matters to confirm with the issuing body and with the entity's auditors; nothing is stated here.
> This mirrors how §2.4 treats the IAS 11 → IFRS 15 transition.

> **Category definitions.** *Authoritative accounting standard*: issued by a standard-setter and applied by
> entities reporting under that framework. *International standard*: issued by ISO/IEC; some are certifiable
> management-system standards, others are guidance that cannot be certified against. *National standard*: a
> published standard issued through a single country's standards process; it is not an international standard
> and not merely industry practice, and it reaches a project only where a contract or a procurement regime
> imports it. *Contract framework*, a published family of contract forms adopted by agreement between parties.
> *Professional guidance*, a professional body's recommended practice. *Voluntary framework*: adopted by
> choice, imposing no legal obligation. *Industry practice*, a widely used approach with no single
> authoritative publisher. *Illustrative reference*: named to show the shape of an instrument or of a
> regulatory pattern, and relied on for no requirement in this book; where such an entry is understood to be
> legislation or supervisory guidance, that understanding is noted in its Category cell. Two entries (the EU
> AI Act and the EU General Data Protection Regulation) are understood to be legislation; both are named here
> as reference points, not as sources of obligation on any reader, and their reach is a question for qualified
> advice. No other entry in this table is legislation, and none is described as such. Applicability is
> jurisdiction-specific throughout, and where local law or a contract imposes a stricter requirement, that
> requirement governs. The programme's full cross-volume register, with verification dates, is maintained as
> the PCI External-Reference Register.

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
| Fig 5.2.2 | The three-curve cost report | 5.2 |
| Fig 5.3.1 | Control account as the integration point *(animated)* | 5.3 |
| Fig 6.1.1 | The three EVM curves at the data date *(animated)* | 6.1 |
| Fig 6.3.1 | The EAC fan *(animated)* | 6.3 |
| Fig 7.1.1 | The contract risk-allocation spectrum *(animated)* | 7.1 |
| Fig 7.2.1 | The contract lifecycle | 7.2 |
| Fig 8.1.1 | Power/interest stakeholder grid *(animated)* | 8.1 |
| Fig 8.2.2 | A manpower histogram against availability | 8.2 |
| Fig 8.6.1 | The development-approach spectrum *(animated)* | 8.6 |
| Fig 8.6.2 | Incremental vs iterative *(animated)* | 8.6 |
| Fig 9.3.1 | Release burnup with a moving scope line *(animated)* | 9.3 |
| Fig 9.3.4 | A burnup chart with a scope change | 9.3 |
| Fig 10.2.1 | The activity network and critical path *(animated)* | 10.2 |
| Fig 10.3.1 | The crash time–cost curve | 10.3 |
| Fig 12.2.1 | Probability–impact matrix *(animated)* | 12.2 |
| Fig 13.1.1 | The AI landscape *(animated)* | 13.1 |
| Fig 13.2.1 | Data quality and lineage for a controls AI workflow *(animated)* | 13.2 |
| Fig 13.4.1 | Capability-vs-category matrix *(animated)* | 13.4 |
| Fig 13.5.1 | AI across the controls lifecycle *(animated)* | 13.5 |
| Fig 13.6.1 | AI-governance decision flow *(animated)* | 13.6 |
| Fig 13.7.1 | The AI-maturity ladder *(animated)* | 13.7 |

Illustration style throughout: brand blue `#1D4ED8`, Plus Jakarta Sans labels, clean professional diagrams
(Conventions, §6). Animations are **digital/LMS-only**; the print/PDF uses the static figure rendered from the
same spec.

---

## Appendix E — Self-check answers

Each Knowledge Area closes with two or three **self-check questions**, and each carries its answer alongside
so a reader can self-mark on the spot. This appendix gathers all **146** of those answers into one key (by
domain, then by Knowledge Area and question number) for revision, for marking a study group, or for reading a
domain's questions cold and checking afterwards. The questions themselves stay where they are learned, at the
end of each Knowledge Area.

### Domain 1 — Foundations of Accounting for Project Controls

| KA | # | Answer |
|---|---:|---|
| 1.1 | 1 | `A = L + E`; under the entity concept the injection is a claim by owners on the business, financing, not earned performance. |
| 1.1 | 2 | Credit; normal balance credit. |
| 1.1 | 3 | A wholly omitted transaction; a transaction posted to the wrong account of the same type; a duplicated entry. |
| 1.2 | 1 | SOFP: position at a point; SOPL & OCI: performance over a period; cash flows: change in cash by activity; SOCE, movement in equity. |
| 1.2 | 2 | E.g. profit → retained earnings → equity; or closing cash ties SOFP to the cash-flow statement. |
| 1.2 | 3 | Working capital (growth in receivables/inventory exceeding payables) and investing/financing outflows consume cash the accrual profit does not reflect. |
| 1.3 | 1 | Recognise expense with the income it earns; e.g. only USD 3,500 of supplies consumed is expensed, the rest stays an asset. |
| 1.3 | 2 | Prepayment/asset; accrued expense/liability; deferred income/liability. |
| 1.3 | 3 | Omitting it understates actual cost `AC`, inflating `CPI` and corrupting the forecast: cross-ref Domain 6. |
| 1.4 | 1 | Present obligation from a past event; probable outflow; reliable estimate. |
| 1.4 | 2 | Expected value for a large population of similar items; most-likely for a single obligation, adjusted for other outcomes. |
| 1.4 | 3 | Recognise the full USD 80,000 expected loss immediately as an onerous-contract provision. |
| 1.5 | 1 | WBS: spend by deliverable/work package, e.g. Foundations; CBS: spend by cost type, e.g. total subcontract, across the project. |
| 1.5 | 2 | Mis-coding propagates into every downstream report and distorts project cost/CPI before it is caught; source discipline prevents it. |
| 1.5 | 3 | It is the WBS×OBS point where budget, cost and schedule integrate: the level at which EV is measured in Domain 6. |

### Domain 2 — Financial Reporting & the Standards

| KA | # | Answer |
|---|---:|---|
| 2.1 | 1 | Fundamental: relevance, faithful representation. Enhancing: comparability, verifiability, timeliness, understandability. |
| 2.1 | 2 | Account for economic reality not legal label: e.g. a lease dressed as a service contract is accounted for as a lease. |
| 2.2 | 1 | Identify contract; identify performance obligations; determine transaction price; allocate; recognise as/when satisfied. |
| 2.2 | 2 | Prospectively, as a change in estimate; PoC is re-struck on the new total cost, with a catch-up in the current period; prior periods are not restated. If total cost now exceeds price, recognise the whole loss immediately per IAS 37. |
| 2.2 | 3 | A contract asset is a conditional right to consideration for work performed but not yet billed; a receivable is an unconditional right, only time stands between it and payment. |
| 2.3 | 1 | It changes reported revenue and every margin/KPI derived from it, even though profit is identical. |
| 2.3 | 2 | The contract asset or contract liability. |
| 2.4 | 1 | Lower of cost and net realisable value. |
| 2.4 | 2 | Capitalise directly attributable borrowing costs into the asset's cost. |
| 2.4 | 3 | To bring construction into a single control-based revenue model applied consistently across industries, replacing the percentage-of-completion-by-default rule. |
| 2.5 | 1 | Audience; framework/audit; orientation: historical vs forecast; cadence; structure. |
| 2.5 | 2 | The contract asset/liability and timing/accrual differences. |

### Domain 3 — Budgeting & Forecasting

| KA | # | Answer |
|---|---:|---|
| 3.1 | 1 | Identified vs unidentified risk; inside vs outside the baseline; PM vs management. |
| 3.1 | 2 | So variance measured against it is meaningful; it changes only through approved change control. |
| 3.2 | 1 | It narrows, as scope definition matures. |
| 3.2 | 2 | Analogous: early/sanity; parametric, planning with good rates; bottom-up: definitive, needs a WBS. |
| 3.3 | 1 | Planned Value, `PV`/BCWS. |
| 3.3 | 2 | Otherwise early schedule variance is an artefact of the phasing, not of performance. |
| 3.4 | 1 | `EAC = AC + ETC`; remaining work at budgeted rate; at current `CPI`; at `CPI × SPI`. |
| 3.4 | 2 | It filters timing noise and reveals systemic drift early. |
| 3.5 | 1 | Costs are paid before billings are collected; retention and payment terms widen the gap. |
| 3.5 | 2 | Payment terms, retention, billing cadence, advances, margin. |

### Domain 4 — Performance Management, Variance Analysis & Management Reporting

| KA | # | Answer |
|---|---:|---|
| 4.1 | 1 | Leading to intervene in time; lagging to confirm the intervention worked. |
| 4.1 | 2 | Too tight: everything is an exception; too loose: real problems hide within tolerance. |
| 4.2 | 1 | Price/rate: `(AP−SP)×AQ`; quantity/efficiency: `(AQ−SQ)×SP`. |
| 4.2 | 2 | It attributes the gap to named causes and magnitudes, showing what to act on. |
| 4.3 | 1 | Where are we; where are we heading; what is off-track; what is being done about it. |
| 4.3 | 2 | The same data must aggregate automatically to each level; without source coding it becomes manual re-keying. |
| 4.3 | 3 | Disagreement is about judgement on shared evidence; the duty engages when the professional would be asserting, or silently permitting, something the evidence does not support. |
| 4.3 | 4 | Do not sign what you cannot defend; do not let a changed analysis be attributed to you; do not participate in suppression; do not go silent. |
| 4.4 | 1 | S-curve: how are we tracking over time; waterfall/variance bridge: what moved the number. |
| 4.4 | 2 | Truncated axis; dual axes; 3-D/decoration; cherry-picked baseline; inconsistent small-multiple scales. |

### Domain 5 — Cost Management & Cost Control

| KA | # | Answer |
|---|---:|---|
| 5.1 | 1 | Direct/indirect: what must be allocated; fixed/variable: how cost scales with volume / flexing the budget. |
| 5.1 | 2 | An OAR on a base that does not drive the cost mis-allocates it into every unit cost. |
| 5.2 | 1 | Commitment, accrual, actual; cost-to-date = actuals + accruals. |
| 5.2 | 2 | `AC` understated → `CPI` flattered → forecast corrupted. |
| 5.3 | 1 | By cost type / by scope. |
| 5.3 | 2 | It integrates scope, budget, cost and schedule at a manageable, meaningful level. |
| 5.4 | 1 | Trend: early warning of a possible change; variation: a formal, priced, agreed change. |
| 5.4 | 2 | No: contingency is already inside the baseline; drawing it consumes reserve, it does not add scope. |
| 5.4 | 3 | The controls professional assesses and recommends; a change authority holding the relevant delegated band approves, never the same person on the same change. |
| 5.4 | 4 | An assessed cost exceeding the remaining reserve of its proposed funding source: also a breach of a baseline tolerance, or a change altering a contractual obligation. |

### Domain 6 — Earned Value Management & Forecasting (EVM / EAC)

| KA | # | Answer |
|---|---:|---|
| 6.1 | 1 | So it is comparable to `PV` for schedule and to `AC` for cost; measuring at actual collapses the method. |
| 6.1 | 2 | The same physical state gives different `EV` by rule; fixing it prevents optimistic, inconsistent progress claims. |
| 6.2 | 1 | `CV=EV−AC`, `SV=EV−PV`; `CPI=EV/AC`, `SPI=EV/PV`; <0 or <1 adverse. |
| 6.2 | 2 | The target, usually `BAC`, is likely no longer credible. |
| 6.3 | 1 | Budgeted rate; current `CPI`; `CPI × SPI`; bottom-up. |
| 6.3 | 2 | Persisting → `BAC/CPI`; one-off → `AC + (BAC − EV)`. |
| 6.4 | 1 | Schedule indices time-blind/converge → earned schedule; no critical-path view → read with CPM; optimistic `EV` → objective earning rules; data quality → Domain 5 discipline. |
| 6.4 | 2 | Schedule performance in *time* units that stays meaningful to completion. |

### Domain 7 — Contracts, Commercial Management, BoQ, Invoicing & Revenue

| KA | # | Answer |
|---|---:|---|
| 7.1 | 1 | Lump sum → contractor; target cost → shared; cost-plus → client. |
| 7.1 | 2 | An `EAC` above target quantifies the contractor's pain-share now. |
| 7.1 | 3 | Audited allowable cost as the contract defines it, not the contractor's total recorded cost. |
| 7.1 | 4 | Code and segregate allowable from non-allowable cost at source; keep the supporting records audit-ready for as long as the client's inspection and audit rights run. |
| 7.2 | 1 | Cause, effect and quantum: notified and substantiated. |
| 7.2 | 2 | It withholds cash until completion/defects release, deepening and lengthening the funding trough. |
| 7.2 | 3 | A requirement (often notice within a window) drafted so the entitlement does not survive without it; so the notice is served before the substantiation is built, not with it. |
| 7.2 | 4 | The governing law, and local legal advice: payment and dispute mechanics are understood to be set by legislation in some jurisdictions, and the position varies. |
| 7.3 | 1 | Common tender basis; valuing work done; remeasurement/variation pricing. |
| 7.3 | 2 | They are time-related: a delay extends them, creating prolongation cost. |
| 7.4 | 1 | Gross value → less retention → net certified → less previous payments → amount due. |
| 7.4 | 2 | `EV`: budget; BoQ valuation, contract rates/billing; IFRS 15: recognised revenue. |
| 7.5 | 1 | Under-billing, performing ahead of collections; cash tied up in unbilled work. |
| 7.5 | 2 | Scope → cost → EV → billing → revenue → statements → reporting. |

### Domain 8 — Project Management Lifecycle

| KA | # | Answer |
|---|---:|---|
| 8.1 | 1 | Business case justifies; charter authorises. |
| 8.1 | 2 | A project can hit time/cost/scope yet fail to deliver the benefit or meet quality/stakeholder outcomes. |
| 8.2 | 1 | Schedule, cost coding, control accounts and earned value are all built on it. |
| 8.2 | 2 | Scope/schedule/cost; e.g. risk plan → contingency. |
| 8.3 | 1 | Keep true measurement flowing (progress against earning rules, cost committed/accrued) so controlling can act. |
| 8.3 | 2 | It returns as rework: a favourable variance hiding a quality liability. |
| 8.4 | 1 | Every change is assessed across all constraints and updates all affected baselines coherently. |
| 8.4 | 2 | Earned value: 6; variance/reporting: 4; forecasting: 3/6; risk — 12. |
| 8.4 | 3 | The controls professional assesses and recommends; a change control board or other named authority approves within a recorded band, never the same person on the same change. |
| 8.4 | 4 | A breached baseline tolerance; cost exceeding the remaining balance of its funding source; an altered contractual obligation; a change to the benefit case or the success criteria. |
| 8.5 | 1 | The definitive commercial position: final remeasurement, agreed variations/claims, retention release. |
| 8.5 | 2 | Actual performance data improves future estimates and trains forecasting models. |
| 8.5 | 3 | A stated period, a named custodian in the permanent organisation, a hold rule for live or foreseeable disputes, and retrieval tested in a readable format. |
| 8.5 | 4 | The longest of the contract's records/audit provisions, the claim-limitation and defects-liability periods under the governing law, accounting and tax retention, and any funder or regulator condition: confirmed with commercial, finance and legal, because the periods vary by jurisdiction and contract form. |
| 8.6 | 1 | Incremental: add new working parts to the whole; iterative, refine the same product over passes. |
| 8.6 | 2 | It determines how progress and cost are measured: fixed baseline vs velocity vs both. |

### Domain 9 — Agile, Scrum & Adaptive Delivery for Project Controls

| KA | # | Answer |
|---|---:|---|
| 9.1 | 1 | Agile expects scope to change; treating change as variance against a fixed baseline misreads healthy adaptation as failure. |
| 9.1 | 2 | Fix time and cost; flex scope: the inverted triangle. |
| 9.2 | 1 | PO: value/backlog order; SM, effectiveness/impediments; Developers: the Increment/plan/quality. |
| 9.2 | 2 | Product Backlog→Product Goal; Sprint Backlog→Sprint Goal; Increment→Definition of Done. |
| 9.3 | 1 | Velocity varies; a range (e.g. optimistic/pessimistic velocity) is more honest than false-precise single number. |
| 9.3 | 2 | Scope change: the moving total-scope line. |
| 9.4 | 1 | Cycle time ≈ WIP ÷ throughput; cutting WIP cuts cycle time. |
| 9.4 | 2 | SAFe, LeSS, Scrum-of-Scrums. |
| 9.5 | 1 | `EV = %complete × BAC`; `CPI = EV/AC`; valid only against a defined release scope/`BAC`, rebaselined transparently on scope change. |
| 9.5 | 2 | Points measure relative effort/scope, not cost; costs may lead or lag points; reconcile and explain. |
| 9.5 | 3 | Finance, as the entity's accounting-policy owner, applied consistently and tested by the external auditor; the controls professional supplies and evidences the inputs and reconciles the progress views, and does not select or change the basis. |
| 9.6 | 1 | Map Sprints/releases to milestones; report value delivered, run-rate, forecast and AgileEVM status. |
| 9.6 | 2 | Capped T&M and target cost: they fund capacity/share risk over a flexible scope. |

### Domain 10 — Project Scheduling (in depth)

| KA | # | Answer |
|---|---:|---|
| 10.1 | 1 | FS, SS, FF, SF; FS. |
| 10.1 | 2 | Logic makes the schedule dynamic: it recalculates when a duration changes; constraints freeze dates and hide slippage. |
| 10.2 | 1 | `TF = LS − ES` (project); `FF = min successor ES − EF` (successor). |
| 10.2 | 2 | The longest, zero-float chain; any slip on it delays the whole project. |
| 10.3 | 1 | Crashing: cost for time; fast-tracking: time for risk. |
| 10.3 | 2 | It ignores duration uncertainty and near-critical paths; Monte Carlo gives a completion distribution/probability. |
| 10.4 | 1 | Actual progress can move the critical path to a different chain. |
| 10.4 | 2 | As schedule increments mapped to milestones and reconciled with the CPM view. |

### Domain 11 — Business Process Cycles (O2C, P2P & the control environment)

| KA | # | Answer |
|---|---:|---|
| 11.1 | 1 | Order → credit check → fulfil → invoice → collect → apply cash. |
| 11.1 | 2 | Leading indicator of cash risk and billing/revenue disputes. |
| 11.2 | 1 | PO, goods-receipt note, invoice; paying for goods not ordered, not received, or mispriced. |
| 11.2 | 2 | PO → commitment; receipt → accrual; matched invoice → actual. |
| 11.3 | 1 | No one person controls a whole transaction; e.g. separate raise/approve/receive/pay. |
| 11.3 | 2 | How transactions actually flowed; control breaches, skipped matches, bypassed approvals, bottlenecks. |

### Domain 12 — Risk Management for Project Controls

| KA | # | Answer |
|---|---:|---|
| 12.1 | 1 | Risk: uncertain event affecting objectives; uncertainty: lack of knowledge; issue: a risk that has occurred. |
| 12.1 | 2 | The level of contingency and the thresholds for escalation/treatment. |
| 12.2 | 1 | "Because X, risk that Y, leading to Z"; `EMV = probability × impact`. |
| 12.2 | 2 | Avoid, transfer, mitigate/reduce, accept. |
| 12.3 | 1 | From quantified risk (an EMV sum or, better, a Monte Carlo P-level) documented against the register, not a flat percentage. |
| 12.3 | 2 | Contingency draw-down is normal for register risks; needing management reserve is a re-baselining event for unforeseen scope/risk. |
| 12.3 | 3 | Date, requester, register ID, evidence of materialisation, amount with its substantiation, and revised remaining exposure; approved by the project manager within a recorded delegated limit and by the sponsor above it, never by the requester. |
| 12.3 | 4 | It is closed and an issue is opened for the consequence, in the same transaction, so exposure is not counted twice. |

### Domain 13 — AI for Project Controls & Project Management

| KA | # | Answer |
|---|---:|---|
| 13.1 | 1 | Text unit the model reads/writes; how much text it considers at once; a randomness setting: low for factual work. |
| 13.1 | 2 | Rules: deterministic check; ML: pattern from data; GenAI: generate/transform language, verified. |
| 13.2 | 1 | Accuracy, completeness, consistency, timeliness, validity, uniqueness. |
| 13.2 | 2 | Auditability; an AI-influenced number must be traceable to source when challenged. |
| 13.2 | 3 | Confidentiality and personal data are separate tests: information about identified people carries its own obligations (basis, purpose, minimisation, notice, impact assessment, transfer) which vary by jurisdiction and are settled with the data-protection function before the data moves. |
| 13.2 | 4 | Data collected for one stated purpose is not automatically available for another; a new purpose is a new decision, and the privacy notice given to the people concerned has to cover it. |
| 13.3 | 1 | Role/context, task, data, format, constraints. |
| 13.3 | 2 | No confidential data in ungoverned tools; always verify figures/citations; disclose; keep the audit trail. |
| 13.4 | 1 | General LLM assistants; document/RAG; spreadsheet/data-analysis AI. |
| 13.4 | 2 | Over-reaching (e.g. an LLM for precise arithmetic or an ungrounded document question) invites hallucination. |
| 13.5 | 1 | Input → AI step (accelerate) → verification (assure) → owned output. |
| 13.5 | 2 | Lower-risk: cost coding/reconciliation; higher-judgement: provisioning/revenue recognition, contingency. |
| 13.6 | 1 | AI proposes; the professional verifies, decides and remains accountable; a model cannot be accountable; a named person is. |
| 13.6 | 2 | Source-checked; method/assumption sound; no hallucination; confidentiality; cross-checked; signed off. |
| 13.7 | 1 | Ad-hoc → piloting → standardised → integrated → governed/optimised. |
| 13.7 | 2 | A more capable model that is wrong is more convincingly wrong; the stakes of verification rise. |

---

## Appendix F — Sample-MCQ bank

Every sample multiple-choice question in the book, collected in one bank: **321 items** across the thirteen
domains, in book order, each with its four options, its correct answer, and its topic-and-level tag
(Conventions, §8). Items are numbered **`PCL-MCQ-DD-NN`** (domain, then position within the domain) and the
numbering is stable, so an item can be cited in study notes and found again. Each entry also names the
Knowledge Area item it comes from, where the **rationale** explaining the right answer and each distractor is
set out in full.

**On order and stability.** Items added after an edition was first numbered are **appended at the end of their
domain's block** rather than inserted at their chapter position, because a bank number that moves is worse
than a block that is not in strict chapter order: a citation in someone's study notes must still resolve. The
topic tag on each item, `[4.3.7 · Analysis]`, always shows where in the book it belongs.

| Domain | KAs | Sample MCQs | Bank numbers |
|---|---:|---:|---|
| 1 Foundations of Accounting | 5 | 30 | PCL-MCQ-01-01 – 30 |
| 2 Financial Reporting (IFRS 15) | 5 | 27 | PCL-MCQ-02-01 – 27 |
| 3 Budgeting & Forecasting | 5 | 23 | PCL-MCQ-03-01 – 23 |
| 4 Performance/Variance/Reporting | 4 | 20 | PCL-MCQ-04-01 – 20 |
| 5 Cost Management | 4 | 20 | PCL-MCQ-05-01 – 20 |
| 6 EVM/EAC | 4 | 19 | PCL-MCQ-06-01 – 19 |
| 7 Contracts & Commercial | 5 | 26 | PCL-MCQ-07-01 – 26 |
| 8 PM Lifecycle | 6 | 27 | PCL-MCQ-08-01 – 27 |
| 9 Agile & Adaptive | 6 | 26 | PCL-MCQ-09-01 – 26 |
| 10 Scheduling | 4 | 18 | PCL-MCQ-10-01 – 18 |
| 11 Process Cycles | 3 | 16 | PCL-MCQ-11-01 – 16 |
| 12 Risk Management | 3 | 17 | PCL-MCQ-12-01 – 17 |
| 13 AI for Project Controls | 7 | 52 | PCL-MCQ-13-01 – 52 |
| **Total** | **61** | **321** | |

The AI domain carries the largest share because it is examined at 20 % of the blueprint; Domains 11 and 12
carry items drawn from their advanced topics as well as their Knowledge Areas, which is why some tags read
`11.A.1` or `13.A.4`.

> **Blueprint separation.** These are **study items**, written from the same blueprint as the examination but
> maintained separately from any live examination bank, and tagged to topic numbers and cognitive levels so a
> weak result points straight back at the topic that fixes it. They are not reused as live examination
> questions (Conventions, §8).

### Domain 1 — Foundations of Accounting for Project Controls

**PCL-MCQ-01-01** `[1.1.2 · Recall]` Which statement about debits is correct?
- A. A debit always increases an account.
- B. A debit always means a decrease.
- C. Debits and credits are interchangeable labels for increases.
- D. A debit increases assets and expenses, and decreases liabilities, equity and income.

**Answer: D.** *(Rationale at MCQ 1.1-A, KA 1.1.)*

**PCL-MCQ-01-02** `[1.1.3 · Application]` A firm buys equipment for USD 24,000, paying cash. The correct entry is:
- A. Dr Equipment 24,000; Cr Cash 24,000
- B. Dr Cash 24,000; Cr Equipment 24,000
- C. Dr Equipment 24,000; Cr Share capital 24,000
- D. Dr Equipment 24,000; Cr Accounts payable 24,000

**Answer: A.** *(Rationale at MCQ 1.1-B, KA 1.1.)*

**PCL-MCQ-01-03** `[1.1.4 · Analysis]` A trial balance balances. Which error would it still fail to detect?
- A. A debit of 500 posted as 5,000 with no matching credit change.
- B. Total debits of 192,000 against total credits of 191,500.
- C. A sales invoice omitted from the ledger entirely.
- D. A credit balance recorded in the debit column.

**Answer: C.** *(Rationale at MCQ 1.1-C, KA 1.1.)*

**PCL-MCQ-01-04** `[1.1.5 · Application]` Using Meridian's month, what is the cash balance after transactions 1–9?
- A. USD 108,000
- B. USD 129,000
- C. USD 134,000
- D. USD 153,000

**Answer: B.** *(Rationale at MCQ 1.1-D, KA 1.1.)*

**PCL-MCQ-01-05** `[1.1.1 · Application]` A business controls assets of USD 250,000 and owes liabilities of USD 90,000. Its equity is:
- A. USD 90,000
- B. USD 160,000
- C. USD 250,000
- D. USD 340,000

**Answer: B.** *(Rationale at MCQ 1.1-E, KA 1.1.)*

**PCL-MCQ-01-06** `[1.1.4 · Analysis]` A bank statement shows USD 86,500. Outstanding cheques total USD 5,000 and a deposit in transit is USD 2,500. If the differences are purely timing, the ledger cash balance the reconciliation should agree to is:
- A. USD 79,000
- B. USD 84,000
- C. USD 89,000
- D. USD 94,000

**Answer: B.** *(Rationale at MCQ 1.1-F, KA 1.1.)*

**PCL-MCQ-01-07** `[1.2.6 · Analysis]` A company reports profit of USD 17,000 but net operating cash of USD 3,000 in the same period. The most likely explanation is:
- A. An accounting error, since profit should equal operating cash.
- B. Growth in working capital: receivables and inventory rose faster than payables.
- C. The company paid a dividend of USD 14,000.
- D. Depreciation of USD 14,000 was charged.

**Answer: B.** *(Rationale at MCQ 1.2-A, KA 1.2.)*

**PCL-MCQ-01-08** `[1.2.3 · Application]` Purchasing equipment for cash appears in the cash-flow statement as:
- A. An operating outflow.
- B. A financing outflow.
- C. It does not appear, being non-cash.
- D. An investing outflow.

**Answer: D.** *(Rationale at MCQ 1.2-B, KA 1.2.)*

**PCL-MCQ-01-09** `[1.2.6 · Recall]` Through which statement does the period's profit reach equity on the SOFP?
- A. The statement of changes in equity.
- B. The statement of cash flows.
- C. The notes.
- D. The trial balance.

**Answer: A.** *(Rationale at MCQ 1.2-C, KA 1.2.)*

**PCL-MCQ-01-10** `[1.2.1 · Recall]` Under IAS 1, the SOFP normally classifies assets and liabilities as:
- A. Tangible vs intangible.
- B. Monetary vs non-monetary.
- C. Current vs non-current.
- D. Operating vs financing.

**Answer: C.** *(Rationale at MCQ 1.2-D, KA 1.2.)*

**PCL-MCQ-01-11** `[1.2.3 · Application]` A company reports profit of USD 25,000, depreciation of USD 4,000, an increase in receivables of USD 9,000 and an increase in payables of USD 3,000. Under the indirect method, net operating cash is:
- A. USD 17,000
- B. USD 19,000
- C. USD 23,000
- D. USD 41,000

**Answer: C.** *(Rationale at MCQ 1.2-E, KA 1.2.)*

**PCL-MCQ-01-12** `[1.2.5 · Recall]` A controls professional wants to know *how* revenue was recognised on a long-term contract. That accounting policy is set out in:
- A. The face of the statement of profit or loss.
- B. The statement of changes in equity.
- C. The notes to the financial statements.
- D. The statement of cash flows.

**Answer: C.** *(Rationale at MCQ 1.2-F, KA 1.2.)*

**PCL-MCQ-01-13** `[1.3.4 · Application]` Equipment costs USD 24,000, residual USD 0, life 3 years, straight-line. The monthly depreciation is:
- A. USD 8,000
- B. USD 2,000
- C. USD 24,000
- D. USD 667

**Answer: D.** *(Rationale at MCQ 1.3-A, KA 1.3.)*

**PCL-MCQ-01-14** `[1.3.3 · Application]` A client pays USD 4,000 in advance for a workshop not yet delivered. On receipt the entry is:
- A. Dr Cash 4,000; Cr Deferred income 4,000
- B. Dr Cash 4,000; Cr Service revenue 4,000
- C. Dr Deferred income 4,000; Cr Cash 4,000
- D. Dr Accrued income 4,000; Cr Cash 4,000

**Answer: A.** *(Rationale at MCQ 1.3-B, KA 1.3.)*

**PCL-MCQ-01-15** `[1.3.5 · Analysis]` A subcontractor performed work on 29 March, invoiced 5 April, paid 30 April. On the accrual basis the cost belongs in:
- A. March
- B. April (invoice date)
- C. April (payment date)
- D. Split evenly across March and April

**Answer: A.** *(Rationale at MCQ 1.3-C, KA 1.3.)*

**PCL-MCQ-01-16** `[1.3.1 · Analysis]` Meridian's accrual profit is USD 17,000 but operating cash is USD 3,000. On a pure **cash basis**, the period's profit would be closest to:
- A. USD 17,000
- B. USD 20,000
- C. USD 14,000
- D. USD 3,000

**Answer: D.** *(Rationale at MCQ 1.3-D, KA 1.3.)*

**PCL-MCQ-01-17** `[1.3.3 · Application]` A firm pays USD 12,000 at the start of a month for a twelve-month insurance policy. Three months later, the prepaid insurance asset remaining on the SOFP is:
- A. USD 0
- B. USD 3,000
- C. USD 9,000
- D. USD 12,000

**Answer: C.** *(Rationale at MCQ 1.3-E, KA 1.3.)*

**PCL-MCQ-01-18** `[1.3.2 · Recall]` The matching concept requires that an expense be recognised:
- A. In the period the related cash is paid.
- B. In the same period as the income it helps to earn.
- C. In the period the supplier's invoice is received.
- D. In whichever period gives the smoothest profit trend.

**Answer: B.** *(Rationale at MCQ 1.3-F, KA 1.3.)*

**PCL-MCQ-01-19** `[1.4.3 · Application]` 2,000 units are sold under warranty; 5 % are expected to need a repair costing USD 120 on average. The warranty provision is:
- A. USD 240,000
- B. USD 120
- C. USD 12,000
- D. USD 6,000

**Answer: C.** *(Rationale at MCQ 1.4-A, KA 1.4.)*

**PCL-MCQ-01-20** `[1.4.2 · Analysis]` Which is **not** permitted to be recognised as a provision under IAS 37?
- A. A warranty obligation on units already sold.
- B. A probable, reliably estimable legal claim from a past event.
- C. The unavoidable loss on an onerous contract already signed.
- D. Expected operating losses of the next financial year.

**Answer: D.** *(Rationale at MCQ 1.4-B, KA 1.4.)*

**PCL-MCQ-01-21** `[1.4.5 · Application]` A fixed-price contract has a price of USD 500,000, cost to date USD 300,000 and estimated cost to complete USD 280,000. The loss to recognise now is:
- A. USD 80,000
- B. USD 0: recognise it as costs are incurred.
- C. USD 280,000
- D. USD 200,000

**Answer: A.** *(Rationale at MCQ 1.4-C, KA 1.4.)*

**PCL-MCQ-01-22** `[1.4.3 · Application]` A single obligation's best estimate is USD 20,000, payable in 3 years; the discount rate is 8 %. The provision's present value is closest to:
- A. USD 20,000
- B. USD 15,877
- C. USD 25,194
- D. USD 18,519

**Answer: B.** *(Rationale at MCQ 1.4-D, KA 1.4.)*

**PCL-MCQ-01-23** `[1.4.4 · Recall]` A contingent asset is recognised in the financial statements when an inflow is:
- A. Virtually certain.
- B. Possible.
- C. Probable.
- D. Merely estimable.

**Answer: A.** *(Rationale at MCQ 1.4-E, KA 1.4.)*

**PCL-MCQ-01-24** `[1.4.6 · Application]` A discounted provision is carried at USD 100,000 at the start of the year; the discount rate is 6 % and the estimate is unchanged. Its carrying amount at the year-end is:
- A. USD 6,000
- B. USD 94,000
- C. USD 100,000
- D. USD 106,000

**Answer: D.** *(Rationale at MCQ 1.4-F, KA 1.4.)*

**PCL-MCQ-01-25** `[1.4.1 · Analysis]` Which of the following is an **accrual** rather than a provision?
- A. A warranty obligation on units already sold, based on expected failure rates.
- B. A probable legal claim whose settlement amount is uncertain.
- C. A legal obligation to decommission a facility in several years' time.
- D. Electricity consumed last month for which no invoice has yet arrived.

**Answer: D.** *(Rationale at MCQ 1.4-G, KA 1.4.)*

**PCL-MCQ-01-26** `[1.5.4 · Recall]` A control account is best described as the intersection of:
- A. A WBS element and an OBS element.
- B. A cost element and a resource.
- C. Two ledger accounts.
- D. A project and a company code.

**Answer: A.** *(Rationale at MCQ 1.5-A, KA 1.5.)*

**PCL-MCQ-01-27** `[1.5.2 · Analysis]` A labour cost is correctly coded to account 6100 (labour) but to the wrong project. The consequence is:
- A. The trial balance will not balance.
- B. Statutory totals are wrong but project cost is right.
- C. Statutory totals are right but project cost: and any CPI derived from it — is wrong.
- D. No consequence; account classification is what matters.

**Answer: C.** *(Rationale at MCQ 1.5-B, KA 1.5.)*

**PCL-MCQ-01-28** `[1.5.5 · Application]` In the code `01-1420-120-6100-210`, which segment identifies the *scope* the cost belongs to?
- A. `1420` (project)
- B. `120` (WBS work package)
- C. `6100` (cost element)
- D. `210` (resource)

**Answer: B.** *(Rationale at MCQ 1.5-C, KA 1.5.)*

**PCL-MCQ-01-29** `[1.5.3 · Recall]` Which question does a **CBS** view of project cost answer that a WBS view does not?
- A. How much has been spent on the Foundations work package?
- B. How much has been spent on subcontract labour across the whole project?
- C. Which organisational unit is accountable for a piece of scope?
- D. Whether total debits equal total credits.

**Answer: B.** *(Rationale at MCQ 1.5-D, KA 1.5.)*

**PCL-MCQ-01-30** `[1.5.2 · Application]` At month-end a subcontractor has performed USD 150,000 of work on a package, but invoices on file total only USD 110,000. For project cost-to-date to reflect work performed, the accrual to raise is:
- A. USD 40,000
- B. USD 110,000
- C. USD 150,000
- D. USD 260,000

**Answer: A.** *(Rationale at MCQ 1.5-E, KA 1.5.)*

### Domain 2 — Financial Reporting & the Standards

**PCL-MCQ-02-01** `[2.1.2 · Recall]` The two *fundamental* qualitative characteristics of useful financial information are:
- A. Comparability and timeliness.
- B. Relevance and faithful representation.
- C. Verifiability and understandability.
- D. Prudence and consistency.

**Answer: B.** *(Rationale at MCQ 2.1-A, KA 2.1.)*

**PCL-MCQ-02-02** `[2.1.4 · Application]` A contractor shows one project's contract asset of USD 400,000 netted against another project's contract liability of USD 250,000, presenting USD 150,000. Under IAS 1 this is:
- A. Correct: both are contract balances.
- B. Correct if the same customer.
- C. Incorrect: offsetting is generally prohibited; each contract is presented separately.
- D. Incorrect only if the projects are in different segments.

**Answer: C.** *(Rationale at MCQ 2.1-B, KA 2.1.)*

**PCL-MCQ-02-03** `[2.1.1 · Recall]` The objective of general-purpose financial reporting is to provide information useful to:
- A. Existing and potential investors, lenders and other creditors.
- B. Management for day-to-day project decisions.
- C. Tax authorities computing taxable profit.
- D. Employees negotiating remuneration.

**Answer: A.** *(Rationale at MCQ 2.1-C, KA 2.1.)*

**PCL-MCQ-02-04** `[2.1.2 · Analysis]` An arrangement is legally titled a "service agreement" but gives the entity the right to control an identified crane for three years. Faithful representation requires it to be accounted for as:
- A. A service contract, because that is its legal title.
- B. Whichever treatment produces the lower reported liabilities.
- C. A contingent liability disclosed in the notes.
- D. A lease, because substance over form depicts the economic reality.

**Answer: D.** *(Rationale at MCQ 2.1-D, KA 2.1.)*

**PCL-MCQ-02-05** `[2.2.6 · Application]` A contract has a price of USD 12,000,000 and total estimated cost of USD 9,600,000. Cumulative cost to date is USD 5,400,000. Under the cost-to-cost method, cumulative revenue is:
- A. USD 5,400,000
- B. USD 6,750,000
- C. USD 6,000,000
- D. USD 7,000,000

**Answer: B.** *(Rationale at MCQ 2.2-A, KA 2.2.)*

**PCL-MCQ-02-06** `[2.2.7 · Analysis]` At a year-end, cumulative revenue recognised is USD 6,750,000 and cumulative amounts billed are USD 7,000,000. The contract shows:
- A. A contract liability of USD 250,000.
- B. A contract asset of USD 250,000.
- C. A receivable of USD 6,750,000.
- D. Nil, since both exceed USD 6m.

**Answer: A.** *(Rationale at MCQ 2.2-B, KA 2.2.)*

**PCL-MCQ-02-07** `[2.2.4 · Analysis]` An all-or-nothing completion bonus of USD 500,000 has an assessed 80 % chance of being earned; management judges a significant reversal is *not* highly improbable. The transaction price should:
- A. Include the full USD 500,000.
- B. Include USD 400,000 (80 %).
- C. Exclude the bonus until it becomes highly probable.
- D. Include USD 250,000 (half).

**Answer: C.** *(Rationale at MCQ 2.2-C, KA 2.2.)*

**PCL-MCQ-02-08** `[2.2.5 · Application]` A bundle priced at USD 10,000,000 comprises items with standalone selling prices of 1,000,000 / 8,000,000 / 1,500,000. The amount allocated to the USD 8,000,000 item is:
- A. USD 8,000,000
- B. USD 7,619,048
- C. USD 7,500,000
- D. USD 8,400,000

**Answer: B.** *(Rationale at MCQ 2.2-D, KA 2.2.)*

**PCL-MCQ-02-09** `[2.2.6 · Recall]` Which condition, on its own, is sufficient for revenue to be recognised *over time*?
- A. The contract lasts more than 12 months.
- B. The customer has paid a deposit.
- C. The entity expects to make a profit.
- D. The entity's performance creates an asset the customer controls as it is created.

**Answer: D.** *(Rationale at MCQ 2.2-E, KA 2.2.)*

**PCL-MCQ-02-10** `[2.2.4 · Application]` A performance adjustment has outcomes `+600,000` (25 %), `+300,000` (35 %), `0` (30 %), `−200,000` (10 %). By the expected-value method the variable consideration is:
- A. USD 235,000
- B. USD 600,000
- C. USD 300,000
- D. USD 175,000

**Answer: A.** *(Rationale at MCQ 2.2-F, KA 2.2.)*

**PCL-MCQ-02-11** `[2.2.4 · Application]` A customer pays USD 5,000,000 two years before control transfers; the financing rate is 6 %. Revenue recognised on transfer is closest to:
- A. USD 5,000,000
- B. USD 5,618,000
- C. USD 4,450,000
- D. USD 5,300,000

**Answer: B.** *(Rationale at MCQ 2.2-G, KA 2.2.)*

**PCL-MCQ-02-12** `[2.2.8 · Analysis]` Added scope that is **not distinct** from a single construction obligation is accounted for as:
- A. A separate contract.
- B. Deferred until completion.
- C. Other comprehensive income.
- D. A cumulative catch-up to the existing contract (re-strike PoC).

**Answer: D.** *(Rationale at MCQ 2.2-H, KA 2.2.)*

**PCL-MCQ-02-13** `[2.2.6 · Application]` A contract has a price of USD 20,000,000. At the end of Year 1, cumulative cost was USD 4,000,000 against a total estimated cost of USD 16,000,000, and revenue of USD 5,000,000 was recognised. During Year 2 the total estimated cost is revised to USD 18,000,000 and cumulative cost reaches USD 9,900,000. Year-2 revenue under the cost-to-cost method is:
- A. USD 6,000,000
- B. USD 11,000,000
- C. USD 7,375,000
- D. USD 5,900,000

**Answer: A.** *(Rationale at MCQ 2.2-I, KA 2.2.)*

**PCL-MCQ-02-14** `[2.2.6 · Analysis]` A contract priced at USD 30,000,000 has recognised cumulative profit of USD 1,500,000 to date. A revised forecast puts total cost at completion at USD 32,000,000. Applying the loss rule (IAS 37), the charge to recognise immediately is:
- A. USD 2,000,000
- B. USD 500,000
- C. USD 3,500,000
- D. Nil: the loss is spread over the remaining work.

**Answer: C.** *(Rationale at MCQ 2.2-J, KA 2.2.)*

**PCL-MCQ-02-15** `[2.3.1 · Analysis]` A firm bills a client USD 2,200,000 for subcontracted works costing USD 2,000,000, on which it added a 10 % margin. If it is an **agent**, it recognises revenue of:
- A. USD 2,200,000
- B. USD 2,000,000
- C. USD 200,000
- D. USD 220,000

**Answer: C.** *(Rationale at MCQ 2.3-A, KA 2.3.)*

**PCL-MCQ-02-16** `[2.3.1 · Recall]` The key indicator that an entity is a *principal* is that it:
- A. Issues the invoice to the customer.
- B. Controls the good or service before it is transferred to the customer.
- C. Earns a margin on the transaction.
- D. Is larger than the other party.

**Answer: B.** *(Rationale at MCQ 2.3-B, KA 2.3.)*

**PCL-MCQ-02-17** `[2.3.1 · Application]` An entity engages a subcontractor for USD 4,500,000 of specialist works and bills its client USD 4,950,000. It controls the works before transfer and is responsible for their delivery. It reports:
- A. Revenue USD 4,950,000 and gross profit USD 450,000.
- B. Revenue USD 450,000 and gross profit USD 450,000.
- C. Revenue USD 4,950,000 and gross profit USD 4,950,000.
- D. Revenue USD 4,500,000 and gross profit USD 450,000.

**Answer: A.** *(Rationale at MCQ 2.3-C, KA 2.3.)*

**PCL-MCQ-02-18** `[2.3.3 · Recall]` The accounting figure that reconciles IFRS 15 revenue to amounts billed against the bill of quantities is:
- A. The trade receivable.
- B. The contract asset or contract liability.
- C. Retained earnings.
- D. The onerous-contract provision.

**Answer: B.** *(Rationale at MCQ 2.3-D, KA 2.3.)*

**PCL-MCQ-02-19** `[2.4.4 · Application]` Weighted-average qualifying expenditure on a project under construction all year is USD 4,000,000, funded at 8 %. Borrowing costs to capitalise are:
- A. USD 320,000
- B. USD 0: all interest is expensed.
- C. USD 4,000,000
- D. USD 32,000

**Answer: A.** *(Rationale at MCQ 2.4-A, KA 2.4.)*

**PCL-MCQ-02-20** `[2.4.3 · Recall]` Under IFRS 16, a lessee typically recognises:
- A. Nothing until payments are made.
- B. Only a footnote disclosure.
- C. A right-of-use asset and a lease liability.
- D. Rent expense only, straight-line.

**Answer: C.** *(Rationale at MCQ 2.4-B, KA 2.4.)*

**PCL-MCQ-02-21** `[2.4.1 · Application]` Materials cost USD 300,000; after a design change their net realisable value is USD 220,000. Under IAS 2 they are carried at:
- A. USD 300,000
- B. USD 260,000
- C. USD 220,000
- D. USD 80,000

**Answer: C.** *(Rationale at MCQ 2.4-C, KA 2.4.)*

**PCL-MCQ-02-22** `[2.4.3 · Application]` A five-year plant lease is recognised at inception as a right-of-use asset and lease liability of USD 800,000; the rate is 5 % and the annual payment of USD 180,000 is made in arrears. The total P&L charge in Year 1 is:
- A. USD 180,000
- B. USD 160,000
- C. USD 40,000
- D. USD 200,000

**Answer: D.** *(Rationale at MCQ 2.4-D, KA 2.4.)*

**PCL-MCQ-02-23** `[2.4.6 · Recall]` IFRS 15 superseded IAS 11 principally in order to:
- A. Abolish over-time recognition for construction contracts.
- B. Allow contractors to choose between the two standards.
- C. Require all construction revenue to be recognised on completion.
- D. Apply a single control-based revenue model consistently across all industries.

**Answer: D.** *(Rationale at MCQ 2.4-E, KA 2.4.)*

**PCL-MCQ-02-24** `[2.5.1 · Recall]` Which is a distinguishing feature of *management* (vs statutory) reporting?
- A. It must comply with IFRS.
- B. It is audited annually.
- C. It is flexible, frequent and forward-looking.
- D. It is only historical.

**Answer: C.** *(Rationale at MCQ 2.5-A, KA 2.5.)*

**PCL-MCQ-02-25** `[2.5.2 · Analysis]` A project's management revenue (value earned) differs from its IFRS 15 statutory revenue. The most appropriate response is to:
- A. Adjust the management figure to match, always.
- B. Reconcile and explain the difference (e.g. via the contract asset/liability and timing).
- C. Report only the statutory figure.
- D. Treat them as unrelated.

**Answer: B.** *(Rationale at MCQ 2.5-B, KA 2.5.)*

**PCL-MCQ-02-26** `[2.5.3 · Recall]` In the split between statutory and management reporting, the controls professional typically owns:
- A. The management view and its reconciliation to the statutory view.
- B. The audit opinion on the statutory accounts.
- C. Only the statutory disclosures.
- D. Neither: both belong exclusively to the finance function.

**Answer: A.** *(Rationale at MCQ 2.5-C, KA 2.5.)*

**PCL-MCQ-02-27** `[2.5.2 · Analysis]` A project's management "value earned" is USD 7,500,000 but its statutory IFRS 15 revenue is USD 7,100,000. The most likely explanation is that:
- A. The ledger has been corrupted and must be rebuilt.
- B. The statutory figure should be increased to match the management view.
- C. The two figures are unrelated and need no reconciliation.
- D. The management figure includes an assumed incentive that the constraint excludes from recognised revenue.

**Answer: D.** *(Rationale at MCQ 2.5-D, KA 2.5.)*

### Domain 3 — Budgeting & Forecasting

**PCL-MCQ-03-01** `[3.1.4 · Analysis]` Which statement about management reserve is correct?
- A. It is part of the cost baseline and Planned Value.
- B. It sits outside the cost baseline; drawing on it is a baseline change, not a variance.
- C. It covers identified risks in the risk register.
- D. It is controlled by the project scheduler.

**Answer: B.** *(Rationale at MCQ 3.1-A, KA 3.1.)*

**PCL-MCQ-03-02** `[3.1.4 · Application]` Control-account budgets are USD 9,000,000, contingency reserve USD 700,000, management reserve USD 500,000. The BAC is:
- A. USD 9,700,000
- B. USD 9,000,000
- C. USD 10,200,000
- D. USD 500,000

**Answer: A.** *(Rationale at MCQ 3.1-B, KA 3.1.)*

**PCL-MCQ-03-03** `[3.1.2 · Recall]` Which budgeting approach requires every cost to be justified from a zero base each cycle rather than rolled forward with an increment?
- A. Top-down budgeting.
- B. Bottom-up budgeting.
- C. Rolling-wave budgeting.
- D. Zero-based budgeting.

**Answer: D.** *(Rationale at MCQ 3.1-C, KA 3.1.)*

**PCL-MCQ-03-04** `[3.1.4 · Application]` A project's cost baseline (`BAC`) is USD 12,400,000, of which USD 900,000 is contingency reserve; management reserve is USD 600,000. The total authorised project budget is:
- A. USD 11,500,000
- B. USD 12,400,000
- C. USD 13,000,000
- D. USD 13,900,000

**Answer: C.** *(Rationale at MCQ 3.1-D, KA 3.1.)*

**PCL-MCQ-03-05** `[3.2.2 · Application]` A 4,500 m² building cost USD 10,000,000. Estimated analogously, a comparable 5,000 m² building costs about:
- A. USD 9,000,000
- B. USD 10,000,000
- C. USD 11,111,111
- D. USD 12,500,000

**Answer: C.** *(Rationale at MCQ 3.2-A, KA 3.2.)*

**PCL-MCQ-03-06** `[3.2.1 · Analysis]` A concept-stage (Class 5) estimate is quoted to a board as a firm budget with no range. The main risk is:
- A. False precision: a wide-range early figure is treated as a commitment, so later refinement reads as an
  "overrun."
- B. The estimate is too conservative.
- C. It violates IFRS 15.
- D. Nothing, provided it was bottom-up.

**Answer: A.** *(Rationale at MCQ 3.2-B, KA 3.2.)*

**PCL-MCQ-03-07** `[3.2.3 · Recall]` The primary purpose of a basis of estimate is to:
- A. Replace the risk register.
- B. Set the pass mark for the estimate.
- C. Serve as the contract.
- D. Make the estimate auditable and defensible by recording scope, assumptions, rates and exclusions.

**Answer: D.** *(Rationale at MCQ 3.2-C, KA 3.2.)*

**PCL-MCQ-03-08** `[3.2.2 · Application]` A pipeline is estimated parametrically at USD 850,000 per km for 12 km, plus 15 % contingency on the base. The total estimate is:
- A. USD 1,530,000
- B. USD 8,670,000
- C. USD 10,200,000
- D. USD 11,730,000

**Answer: D.** *(Rationale at MCQ 3.2-D, KA 3.2.)*

**PCL-MCQ-03-09** `[3.2.1 · Recall]` Under the AACE estimate-classification framework, which class reflects near-complete scope definition and suits a definitive bid or check estimate?
- A. Class 5
- B. Class 4
- C. Class 3
- D. Class 1

**Answer: D.** *(Rationale at MCQ 3.2-E, KA 3.2.)*

**PCL-MCQ-03-10** `[3.3.3 · Application]` With the monthly plan {40, 70, 110, 140, 160, …} (USD 000), the Planned Value at the end of Month 4 is:
- A. USD 360,000
- B. USD 140,000
- C. USD 320,000
- D. USD 520,000

**Answer: A.** *(Rationale at MCQ 3.3-A, KA 3.3.)*

**PCL-MCQ-03-11** `[3.3.2 · Analysis]` A project's baseline is straight-lined even though execution ramps up slowly. The likely early effect is that the project will:
- A. Always appear ahead of schedule.
- B. Appear behind against Planned Value even when on plan, because PV is overstated early.
- C. Show no schedule variance ever.
- D. Have a higher BAC.

**Answer: B.** *(Rationale at MCQ 3.3-B, KA 3.3.)*

**PCL-MCQ-03-12** `[3.3.1 · Recall]` Spreading the `BAC` across the schedule period by period produces the time-phased cost baseline. Which earned-value quantity *is* that cumulative curve?
- A. Planned Value (`PV`).
- B. Earned Value (`EV`).
- C. Actual Cost (`AC`).
- D. Estimate at Completion (`EAC`).

**Answer: A.** *(Rationale at MCQ 3.3-C, KA 3.3.)*

**PCL-MCQ-03-13** `[3.3.3 · Application]` A baseline shows cumulative Planned Value of USD 670,000 at the end of Month 6 and USD 900,000 at the end of Month 8. The planned spend for Months 7 and 8 together is:
- A. USD 230,000
- B. USD 670,000
- C. USD 900,000
- D. USD 1,570,000

**Answer: A.** *(Rationale at MCQ 3.3-D, KA 3.3.)*

**PCL-MCQ-03-14** `[3.4.2 · Application]` `BAC` = 1,000,000; `AC` = 520,000; `EV` = 480,000. Using `EAC = BAC/CPI`, the forecast is closest to:
- A. USD 1,000,000
- B. USD 1,040,000
- C. USD 1,083,333
- D. USD 1,106,807

**Answer: C.** *(Rationale at MCQ 3.4-A, KA 3.4.)*

**PCL-MCQ-03-15** `[3.4.2 · Analysis]` A team is both over-cost and behind schedule, and believes the two will compound on the remaining work. The most appropriate EAC method is:
- A. `EAC = AC + (BAC − EV)`
- B. `EAC = BAC / CPI`
- C. `EAC = BAC`
- D. `EAC = AC + (BAC − EV)/(CPI × SPI)`

**Answer: D.** *(Rationale at MCQ 3.4-B, KA 3.4.)*

**PCL-MCQ-03-16** `[3.4.3 · Analysis]` Which is the strongest early-warning signal of a systemic cost problem?
- A. A single month's CPI below 1.0.
- B. A CPI that drifts down over several consecutive periods.
- C. Actual cost exceeding Planned Value in one month.
- D. A positive schedule variance.

**Answer: B.** *(Rationale at MCQ 3.4-C, KA 3.4.)*

**PCL-MCQ-03-17** `[3.4.2 · Application]` `BAC` = USD 800,000; `EV` = USD 300,000; `AC` = USD 320,000. The variance to date is judged a one-off, so remaining work will proceed at the budgeted rate. The EAC is:
- A. USD 500,000
- B. USD 800,000
- C. USD 820,000
- D. USD 853,333

**Answer: C.** *(Rationale at MCQ 3.4-D, KA 3.4.)*

**PCL-MCQ-03-18** `[3.4.1 · Recall]` The Estimate to Complete (`ETC`) is best defined as:
- A. The forecast total cost of the whole job at completion.
- B. The difference between `BAC` and `EAC`.
- C. The actual cost incurred to date.
- D. The current best estimate of the cost of the *remaining* work from now.

**Answer: D.** *(Rationale at MCQ 3.4-E, KA 3.4.)*

**PCL-MCQ-03-19** `[3.5.2 · Analysis]` In the worked forecast, cumulative cash is (200), (280), (250), (120), 0, 110 (USD 000). The peak funding requirement is:
- A. USD 200,000 in Month 1
- B. USD 280,000 in Month 2
- C. USD 120,000 in Month 4
- D. USD 110,000 in Month 6

**Answer: B.** *(Rationale at MCQ 3.5-A, KA 3.5.)*

**PCL-MCQ-03-20** `[3.5.3 · Analysis]` Which change would *deepen* a project's funding trough, all else equal?
- A. Longer client payment terms and higher retention.
- B. Shorter client payment terms.
- C. A mobilisation advance from the client.
- D. Monthly rather than milestone billing.

**Answer: A.** *(Rationale at MCQ 3.5-B, KA 3.5.)*

**PCL-MCQ-03-21** `[3.5.1 · Recall]` The main reason a profitable project can still need funding is:
- A. Depreciation.
- B. The timing gap between paying for work and being paid for it.
- C. Corporation tax.
- D. Management reserve.

**Answer: B.** *(Rationale at MCQ 3.5-C, KA 3.5.)*

**PCL-MCQ-03-22** `[3.5.2 · Application]` A package pays out costs of USD 150,000, USD 250,000 and USD 200,000 in Months 1–3, and collects receipts of USD 0, USD 165,000 and USD 275,000 in the same months. Cumulative cash at the end of Month 3 is:
- A. (USD 600,000)
- B. (USD 235,000)
- C. (USD 160,000)
- D. USD 75,000

**Answer: C.** *(Rationale at MCQ 3.5-D, KA 3.5.)*

**PCL-MCQ-03-23** `[3.5.3 · Recall]` The peak funding requirement of a project is:
- A. Its total cost at completion.
- B. The deepest negative point of the cumulative cash curve: the finance that must be arranged.
- C. The profit expected in cash at the end of the job.
- D. The retention withheld by the client over the job.

**Answer: B.** *(Rationale at MCQ 3.5-E, KA 3.5.)*

### Domain 4 — Performance Management, Variance Analysis & Management Reporting

**PCL-MCQ-04-01** `[4.1.2 · Analysis]` Which is a *leading* indicator for project cost performance?
- A. Cost performance index (`CPI`) to date.
- B. Actual cost incurred.
- C. Weekly installed-quantity productivity trend.
- D. Final cost variance at completion.

**Answer: C.** *(Rationale at MCQ 4.1-A, KA 4.1.)*

**PCL-MCQ-04-02** `[4.1.1 · Application]` A KPI reads `CPI` = 0.97 against a target of 1.00, amber threshold 0.95, tolerance ±0.05. The correct status is:
- A. Red: below target.
- B. Cannot be assessed without the schedule.
- C. Green: exactly on target.
- D. Within tolerance (green/watch): 0.97 is above the 0.95 amber threshold.

**Answer: D.** *(Rationale at MCQ 4.1-B, KA 4.1.)*

**PCL-MCQ-04-03** `[4.1.3 · Recall]` Management by exception means that management attention is focused on:
- A. The items outside their tolerance, while in-tolerance items are noted and left alone.
- B. Every control account equally, reviewed in full each period.
- C. Only the accounts reporting green.
- D. Lagging indicators in preference to leading ones.

**Answer: A.** *(Rationale at MCQ 4.1-C, KA 4.1.)*

**PCL-MCQ-04-04** `[4.1.1 · Analysis]` A team's reported KPI improves steadily while the underlying performance it is meant to reflect does not. The most likely KPI design failure is:
- A. The indicator is gameable: it can improve on paper without reality improving.
- B. Too few indicators are being reported.
- C. The tolerance is set too tight.
- D. The indicator is leading rather than lagging.

**Answer: A.** *(Rationale at MCQ 4.1-D, KA 4.1.)*

**PCL-MCQ-04-05** `[4.2.3 · Application]` Standard 1,000 units at USD 50; actual 1,100 units at USD 52. The material **price** variance is:
- A. USD 2,000 (A)
- B. USD 2,200 (A)
- C. USD 5,000 (A)
- D. USD 7,200 (A)

**Answer: B.** *(Rationale at MCQ 4.2-A, KA 4.2.)*

**PCL-MCQ-04-06** `[4.2.3 · Application]` Same data. The material **quantity** variance is:
- A. USD 5,000 (A)
- B. USD 5,200 (A)
- C. USD 2,200 (A)
- D. USD 200 (A)

**Answer: A.** *(Rationale at MCQ 4.2-B, KA 4.2.)*

**PCL-MCQ-04-07** `[4.2.2 · Analysis]` Why flex the budget to actual output before analysing variances?
- A. To make the budget larger.
- B. To separate efficiency/price effects from volume effects.
- C. To comply with IFRS 15.
- D. To avoid computing variances at all.

**Answer: B.** *(Rationale at MCQ 4.2-C, KA 4.2.)*

**PCL-MCQ-04-08** `[4.2.5 · Analysis]` A work package reports a large *favourable* cost variance. The best professional response is to:
- A. Report it as a saving and move on.
- B. Increase the budget.
- C. Treat it as an error.
- D. Investigate the cause: a favourable variance can hide skipped scope, deferred cost or quality risk.

**Answer: D.** *(Rationale at MCQ 4.2-D, KA 4.2.)*

**PCL-MCQ-04-09** `[4.2.4 · Application]` Budgeted fixed overhead is USD 120,000 over a budgeted output of 6,000 units. Actual output is 5,500 units and actual fixed overhead is USD 118,000. The fixed-overhead **volume** variance is:
- A. USD 2,000 (F)
- B. USD 8,000 (A)
- C. USD 10,000 (A)
- D. USD 10,000 (F)

**Answer: C.** *(Rationale at MCQ 4.2-E, KA 4.2.)*

**PCL-MCQ-04-10** `[4.2.1 · Recall]` A variance is classified as **favourable** when:
- A. Actual differs from budget by any amount.
- B. The quantity variance is larger than the price variance.
- C. It improves profit: actual cost below plan, or actual revenue above plan.
- D. It falls within the reporting tolerance.

**Answer: C.** *(Rationale at MCQ 4.2-F, KA 4.2.)*

**PCL-MCQ-04-11** `[4.3.1 · Analysis]` The best test of a management report's design is whether it:
- A. Lets the reader see status, direction, exceptions and actions in the time available.
- B. Contains every available data point.
- C. Is as long as possible.
- D. Uses the most advanced charts.

**Answer: A.** *(Rationale at MCQ 4.3-A, KA 4.3.)*

**PCL-MCQ-04-12** `[4.3.3 · Application]` Which is the most *decision-ready* reporting of a cost result?
- A. "`CPI` is 0.92."
- B. "Costs are over budget."
- C. "`CPI` 0.92, driven ~50/50 by a steel rate rise (now locked) and foundation rework (now closed); trend should recover next period."
- D. "See the attached 40-page cost ledger."

**Answer: C.** *(Rationale at MCQ 4.3-B, KA 4.3.)*

**PCL-MCQ-04-13** `[4.3.5 · Application]` A monthly dashboard shows eight control accounts: five green, two amber and one red against their tolerances. The exception report should present:
- A. All eight accounts in equal detail.
- B. Only the red account.
- C. The red and the two amber accounts, each with variance, root cause, impact and action/owner.
- D. The five green accounts, to evidence good performance.

**Answer: C.** *(Rationale at MCQ 4.3-C, KA 4.3.)*

**PCL-MCQ-04-14** `[4.3.4 · Recall]` Reporting to a *project board* is best characterised as:
- A. Weekly, granular and action-list focused.
- B. Monthly, summarised, exception-and-forecast focused.
- C. Periodic, highly aggregated and cross-project.
- D. Daily extracts of the raw cost ledger.

**Answer: B.** *(Rationale at MCQ 4.3-D, KA 4.3.)*

**PCL-MCQ-04-15** `[4.4.2 · Analysis]` A bar chart makes a 2 % cost difference look enormous. The most likely cause is:
- A. Too few bars.
- B. Using brand colours.
- C. A missing legend.
- D. A y-axis that does not start at zero (truncated axis).

**Answer: D.** *(Rationale at MCQ 4.4-A, KA 4.4.)*

**PCL-MCQ-04-16** `[4.4.1 · Application]` To explain *what drove* a cost result from budget to actual, the best chart is a:
- A. Pie chart.
- B. Scatter plot.
- C. 3-D column chart.
- D. Waterfall (variance bridge).

**Answer: D.** *(Rationale at MCQ 4.4-B, KA 4.4.)*

**PCL-MCQ-04-17** `[4.4.3 · Recall]` The disciplined ordering of a controls "story" for a decision-maker is:
- A. Status → what changed and why → where it takes us (forecast) → the decision (action).
- B. Action → forecast → status → cause.
- C. Forecast → status → action → cause.
- D. Cause → action → status → forecast.

**Answer: A.** *(Rationale at MCQ 4.4-C, KA 4.4.)*

**PCL-MCQ-04-18** `[4.4.2 · Analysis]` A chart plots cost on the left y-axis and RFI count on a second right y-axis, and the two lines track each other closely. The professional concern is that:
- A. RFIs should never appear on a cost chart.
- B. Dual axes let the scales be chosen so the apparent relationship is manufactured, not real.
- C. The chart uses too many colours.
- D. A pie chart would have been more appropriate.

**Answer: B.** *(Rationale at MCQ 4.4-D, KA 4.4.)*

**PCL-MCQ-04-19** `[4.3.7 · Analysis]` A controls professional's pack shows an adverse forecast movement with its cause. The version that reaches the board has had the cause paragraph removed and the professional's name left on it. The professional's first act is to:
- A. Say nothing; the numbers themselves are unchanged, so nothing has been misstated.
- B. Raise the removal in writing, with the evidence, to the report owner before the board meets, and keep the record.
- C. Ask verbally for the paragraph to be reinstated and, if refused, decline further involvement.
- D. Report the matter outside the organisation immediately.

**Answer: B.** *(Rationale at MCQ 4.3-E, KA 4.3.)*

**PCL-MCQ-04-20** `[4.3.7 · Application]` An escalation is raised to the project manager, who leaves the figure unchanged and does not address the evidence. The professional should:
- A. Accept the decision; the project manager owns the report.
- B. Re-issue the pack with their own figure substituted.
- C. Take the same evidence to the project board, and onward if the response there is also inadequate.
- D. Record the objection in a private note and take no further step.

**Answer: C.** *(Rationale at MCQ 4.3-F, KA 4.3.)*

### Domain 5 — Cost Management & Cost Control

**PCL-MCQ-05-01** `[5.1.3 · Application]` Budgeted overhead USD 600,000 over 30,000 hours; actual 28,000 hours; actual overhead USD 610,000. The overhead absorbed is:
- A. USD 600,000
- B. USD 560,000
- C. USD 610,000
- D. USD 20

**Answer: B.** *(Rationale at MCQ 5.1-A, KA 5.1.)*

**PCL-MCQ-05-02** `[5.1.1 · Application]` A cost is USD 200,000 fixed plus USD 50/unit. At 2,000 units the total is:
- A. USD 100,000
- B. USD 250,000
- C. USD 300,000
- D. USD 400,000

**Answer: C.** *(Rationale at MCQ 5.1-B, KA 5.1.)*

**PCL-MCQ-05-03** `[5.1.3 · Application]` Budgeted overhead is USD 480,000 over a budgeted 24,000 labour hours. Actual activity is 25,000 hours and actual overhead incurred is USD 490,000. The over/(under)-absorption is:
- A. USD 10,000 over-absorbed
- B. USD 10,000 under-absorbed
- C. USD 20,000 over-absorbed
- D. USD 490,000 under-absorbed

**Answer: A.** *(Rationale at MCQ 5.1-C, KA 5.1.)*

**PCL-MCQ-05-04** `[5.1.2 · Analysis]` Site overhead is driven mainly by project *duration*, but is allocated to work packages by *headcount*. The likely consequence is:
- A. None: the total overhead is unchanged, so the allocation does not matter.
- B. The trial balance will no longer balance.
- C. Labour-heavy packages carry overhead they do not cause, distorting every downstream unit cost.
- D. The overhead becomes a direct cost.

**Answer: C.** *(Rationale at MCQ 5.1-D, KA 5.1.)*

**PCL-MCQ-05-05** `[5.2.2 · Application]` Actuals USD 300,000; goods received not invoiced USD 40,000; open POs USD 120,000. The controls **cost-to-date** is:
- A. USD 300,000
- B. USD 340,000
- C. USD 420,000
- D. USD 460,000

**Answer: B.** *(Rationale at MCQ 5.2-A, KA 5.2.)*

**PCL-MCQ-05-06** `[5.2.1 · Analysis]` Why does watching *commitments* improve cost control over watching actuals alone?
- A. Commitments give lead time: they signal future spend before it is received or paid.
- B. Commitments are always smaller.
- C. Actuals are not recorded in the ledger.
- D. Commitments replace the need for a forecast.

**Answer: A.** *(Rationale at MCQ 5.2-B, KA 5.2.)*

**PCL-MCQ-05-07** `[5.2.2 · Analysis]` Reporting only processed invoices as cost-to-date will:
- A. Overstate cost and understate CPI.
- B. Understate cost (by omitting accruals), flattering CPI.
- C. Have no effect on CPI.
- D. Overstate earned value.

**Answer: B.** *(Rationale at MCQ 5.2-C, KA 5.2.)*

**PCL-MCQ-05-08** `[5.2.1 · Application]` Purchase orders raised total USD 800,000; invoices processed (actuals) are USD 350,000 and goods received but not yet invoiced (accruals) are USD 90,000. The **open commitment** is:
- A. USD 360,000
- B. USD 440,000
- C. USD 450,000
- D. USD 800,000

**Answer: A.** *(Rationale at MCQ 5.2-D, KA 5.2.)*

**PCL-MCQ-05-09** `[5.2.4 · Recall]` Which of the following is a data-integrity failure that quietly corrupts the cost forecast?
- A. An approved variation baselined through change control.
- B. A month-end accrual raised from goods-received records.
- C. A cost ledger reconciled to the general ledger each period.
- D. Open commitments left stale: purchase orders never closed after delivery.

**Answer: D.** *(Rationale at MCQ 5.2-E, KA 5.2.)*

**PCL-MCQ-05-10** `[5.3.2 · Analysis]` Control accounts are set far too granular (hundreds of tiny accounts). The main consequence is:
- A. Earned value cannot be computed at all.
- B. The BAC changes.
- C. Cost coding becomes unnecessary.
- D. The measurement overhead swamps the value, without improving control.

**Answer: D.** *(Rationale at MCQ 5.3-A, KA 5.3.)*

**PCL-MCQ-05-11** `[5.3.3 · Recall]` At what level is earned value normally measured and managed?
- A. The whole project only.
- B. The individual invoice.
- C. The control account.
- D. The company.

**Answer: C.** *(Rationale at MCQ 5.3-B, KA 5.3.)*

**PCL-MCQ-05-12** `[5.3.1 · Recall]` The cost breakdown structure (CBS) decomposes a project's cost by:
- A. Scope deliverable and work package.
- B. Cost element/type: labour, materials, plant, subcontract, overhead.
- C. Accountable organisational unit.
- D. Reporting period.

**Answer: B.** *(Rationale at MCQ 5.3-C, KA 5.3.)*

**PCL-MCQ-05-13** `[5.3.2 · Application]` A control account holds near-term work that is fully defined, scheduled and costed, plus future work whose detail is not yet developed. The future work should be held as:
- A. A planning package.
- B. A work package.
- C. A trend.
- D. An open commitment.

**Answer: A.** *(Rationale at MCQ 5.3-D, KA 5.3.)*

**PCL-MCQ-05-14** `[5.4.3 · Application]` `BAC` is USD 9,700,000. An approved new-scope variation of USD 300,000 (from management reserve) is baselined, and USD 150,000 of contingency is drawn for a materialised risk. The new `BAC` is:
- A. USD 9,550,000
- B. USD 10,150,000
- C. USD 9,700,000
- D. USD 10,000,000

**Answer: D.** *(Rationale at MCQ 5.4-A, KA 5.4.)*

**PCL-MCQ-05-15** `[5.4.2 · Recall]` A "trend" in cost control is best described as:
- A. A formal, agreed change order.
- B. A completed variance.
- C. An early warning of a potential change, logged for lead time.
- D. A change to the risk appetite.

**Answer: C.** *(Rationale at MCQ 5.4-B, KA 5.4.)*

**PCL-MCQ-05-16** `[5.4.1 · Analysis]` The primary purpose of change control is to:
- A. Prevent all change.
- B. Ensure every change is visible, costed and authorised before it affects the baseline.
- C. Speed up the project.
- D. Replace the forecast.

**Answer: B.** *(Rationale at MCQ 5.4-C, KA 5.4.)*

**PCL-MCQ-05-17** `[5.4.2 · Application]` A control account's budget is USD 1,750,000 and its forecast at completion on committed and remaining scope is USD 1,800,000. The trend log holds one probable but unformalised change of +USD 120,000. The **potential** variance against budget is:
- A. (USD 170,000)
- B. (USD 50,000)
- C. (USD 290,000)
- D. USD 120,000

**Answer: A.** *(Rationale at MCQ 5.4-D, KA 5.4.)*

**PCL-MCQ-05-18** `[5.4.1 · Analysis]` A project's actual cost steadily diverges from a baseline that has never formally changed; investigation finds many small, unlogged scope additions. This situation is best described as:
- A. Normal variance, to be managed by exception.
- B. An accrual cut-off error.
- C. Overhead under-absorption.
- D. Scope creep: uncontrolled change accumulating until variance against the baseline is meaningless.

**Answer: D.** *(Rationale at MCQ 5.4-E, KA 5.4.)*

**PCL-MCQ-05-19** `[5.4.3 · Analysis]` A cost engineer assesses a client-instructed change, prices it, and
(being the delegated holder of a value band that covers it) signs the approval themselves. The change is
correctly priced and correctly baselined. The principal control weakness is:
- A. None; the pricing and the baseline update are both correct.
- B. The assessor and the approver are the same person, so no independent mind ever tested the assessment.
- C. The change should have been logged as a trend before it was assessed.
- D. Client-instructed changes cannot be funded from contingency.

**Answer: B.** *(Rationale at MCQ 5.4-F, KA 5.4.)*

**PCL-MCQ-05-20** `[5.4.3 · Application]` A variation's assessed cost exceeds the contingency remaining on the funding source proposed for it. The correct sequence is:
- A. Approve the change, then report the contingency overdraw in the period report.
- B. Approve the change and re-baseline, recording the reserve position afterwards.
- C. Escalate to the next level of authority before approval, with the funding position stated.
- D. Split the change so each part falls within the remaining contingency.

**Answer: C.** *(Rationale at MCQ 5.4-G, KA 5.4.)*

### Domain 6 — Earned Value Management & Forecasting (EVM / EAC)

**PCL-MCQ-06-01** `[6.1.1 · Recall]` Earned value (`EV`) is:
- A. The actual cost of the work performed.
- B. The budgeted cost of the work performed.
- C. The budgeted cost of the work scheduled.
- D. The cash received to date.

**Answer: B.** *(Rationale at MCQ 6.1-A, KA 6.1.)*

**PCL-MCQ-06-02** `[6.1.2 · Application]` A USD 100,000 package is 40 % complete. Under the **50/50** rule (started, not finished), `EV` is:
- A. USD 0
- B. USD 40,000
- C. USD 50,000
- D. USD 100,000

**Answer: C.** *(Rationale at MCQ 6.1-B, KA 6.1.)*

**PCL-MCQ-06-03** `[6.1.2 · Application]` A work package with a budget of USD 250,000 earns under the **units completed** rule. At the data date **600 of 800 units** are done. `EV` is:
- A. USD 187,500
- B. USD 150,000
- C. USD 125,000
- D. USD 250,000

**Answer: A.** *(Rationale at MCQ 6.1-C, KA 6.1.)*

**PCL-MCQ-06-04** `[6.1.3 · Analysis]` At the data date a project shows `EV` **above** `PV` but `AC` **above** `EV`. The integrated picture is:
- A. Behind schedule and over cost.
- B. Ahead of schedule and under cost.
- C. Ahead of schedule but over cost.
- D. Behind schedule but under cost.

**Answer: C.** *(Rationale at MCQ 6.1-D, KA 6.1.)*

**PCL-MCQ-06-05** `[6.2.2 · Application]` `EV` = 480,000; `AC` = 530,000; `PV` = 520,000. The `CPI` and `SPI` are:
- A. 0.91 and 0.92
- B. 0.92 and 0.91
- C. 1.10 and 1.08
- D. 1.02 and 1.04

**Answer: A.** *(Rationale at MCQ 6.2-A, KA 6.2.)*

**PCL-MCQ-06-06** `[6.2.3 · Analysis]` `BAC` = 1,000,000; `EV` = 480,000; `AC` = 530,000. The `TCPI` to meet `BAC` is 1.11, while the `CPI` achieved is 0.91. This indicates:
- A. The BAC is comfortably achievable.
- B. The project is ahead of schedule.
- C. The EV is wrong.
- D. The remaining work must be far more efficient than achieved so far, so the BAC is likely not credible.

**Answer: D.** *(Rationale at MCQ 6.2-B, KA 6.2.)*

**PCL-MCQ-06-07** `[6.2.1 · Application]` With `EV` = 480,000 and `PV` = 520,000, the schedule variance is:
- A. USD +40,000
- B. USD (40,000)
- C. USD (50,000)
- D. USD 0

**Answer: B.** *(Rationale at MCQ 6.2-C, KA 6.2.)*

**PCL-MCQ-06-08** `[6.2.3 · Application]` `BAC` = 2,000,000; `EV` = 900,000; `AC` = 1,000,000. The `TCPI` to meet the `BAC` is:
- A. 0.90
- B. 0.91
- C. 1.00
- D. 1.10

**Answer: D.** *(Rationale at MCQ 6.2-D, KA 6.2.)*

**PCL-MCQ-06-09** `[6.2.4 · Analysis]` A project reports `CPI` = 0.88 and `SPI` = 1.06. The most likely reading is:
- A. The project is efficient but under-resourced.
- B. The project is over cost and behind schedule.
- C. The project is buying schedule with cost: accelerating at a cost premium.
- D. The `EV` must be inflated.

**Answer: C.** *(Rationale at MCQ 6.2-E, KA 6.2.)*

**PCL-MCQ-06-10** `[6.3.2 · Application]` `BAC` = 1,000,000; `AC` = 530,000; `EV` = 480,000. `EAC = AC + (BAC − EV)` gives:
- A. USD 1,000,000
- B. USD 1,050,000
- C. USD 1,104,167
- D. USD 1,152,010

**Answer: B.** *(Rationale at MCQ 6.3-A, KA 6.3.)*

**PCL-MCQ-06-11** `[6.3.3 · Analysis]` A cost overrun was caused by a one-off rate spike, now locked by a fixed supply agreement; the remaining work is expected to run to budget. The most appropriate EAC method is:
- A. `EAC = AC + (BAC − EV)`
- B. `EAC = BAC/CPI`
- C. `EAC = AC + (BAC − EV)/(CPI × SPI)`
- D. `EAC = BAC`

**Answer: A.** *(Rationale at MCQ 6.3-B, KA 6.3.)*

**PCL-MCQ-06-12** `[6.3.4 · Application]` With `EAC` = 1,104,167 and `BAC` = 1,000,000, the `VAC` is:
- A. USD +104,167
- B. USD (104,167)
- C. USD (50,000)
- D. USD 0

**Answer: B.** *(Rationale at MCQ 6.3-C, KA 6.3.)*

**PCL-MCQ-06-13** `[6.3.2 · Application]` `BAC` = 800,000; `EV` = 300,000; `AC` = 375,000. Assuming the cost variance **persists**, the `EAC` is:
- A. USD 875,000
- B. USD 640,000
- C. USD 1,000,000
- D. USD 800,000

**Answer: C.** *(Rationale at MCQ 6.3-D, KA 6.3.)*

**PCL-MCQ-06-14** `[6.3.1 · Recall]` In the identity `EAC = AC + ETC`, the `ETC` is:
- A. The forecast cost of the remaining work.
- B. The total forecast cost of the project.
- C. The cost actually incurred to date.
- D. The variance between budget and forecast at completion.

**Answer: A.** *(Rationale at MCQ 6.3-E, KA 6.3.)*

**PCL-MCQ-06-15** `[6.4.2 · Analysis]` Why is `SPI` misleading late in a project?
- A. It is measured in time units.
- B. It ignores actual cost.
- C. It converges to 1 at completion because all planned value is eventually earned, even if late.
- D. It cannot be computed after 50 % complete.

**Answer: C.** *(Rationale at MCQ 6.4-A, KA 6.4.)*

**PCL-MCQ-06-16** `[6.4.3 · Recall]` Earned schedule improves on `SV`/`SPI` by expressing progress in:
- A. Cost.
- B. Units of work.
- C. Risk exposure.
- D. Time.

**Answer: D.** *(Rationale at MCQ 6.4-B, KA 6.4.)*

**PCL-MCQ-06-17** `[6.4.2 · Analysis]` A project shows `SPI` = 1.02 overall, yet a critical-path activity is slipping. This illustrates that:
- A. `SPI` always detects critical-path slippage.
- B. EVM does not see the critical path; it must be read with critical-path analysis.
- C. The `EV` must be wrong.
- D. The project is definitely on time.

**Answer: B.** *(Rationale at MCQ 6.4-C, KA 6.4.)*

**PCL-MCQ-06-18** `[6.4.3 · Application]` At the end of Month 8 (`AT` = 8), `EV` = 440,000. The baseline shows cumulative `PV` of 400,000 at Month 6 and 480,000 at Month 7. `ES` and `SV(t)` are:
- A. `ES` = 6.5 months; `SV(t)` = (1.5) months
- B. `ES` = 6.5 months; `SV(t)` = +1.5 months
- C. `ES` = 7.0 months; `SV(t)` = (1.0) month
- D. `ES` = 6.0 months; `SV(t)` = (2.0) months

**Answer: A.** *(Rationale at MCQ 6.4-D, KA 6.4.)*

**PCL-MCQ-06-19** `[6.4.5 · Analysis]` A programme's `CPI` moves from 0.96 at Month 3 to 0.91 at Month 6, with neither value catastrophic in isolation. The strongest warning signal is:
- A. The Month 6 `CPI` level of 0.91 on its own.
- B. Nothing: both values round to about 1.
- C. The Month 3 `CPI`, because earlier data is always more reliable.
- D. The deteriorating period-on-period trend, which warrants escalation before a single bad month arrives.

**Answer: D.** *(Rationale at MCQ 6.4-E, KA 6.4.)*

### Domain 7 — Contracts, Commercial Management, BoQ, Invoicing & Revenue

**PCL-MCQ-07-01** `[7.1.1 · Analysis]` Under a **lump-sum** contract with well-defined scope, an unexpected cost overrun on that scope is primarily borne by:
- A. The client.
- B. The insurer.
- C. Shared 50/50.
- D. The contractor.

**Answer: D.** *(Rationale at MCQ 7.1-A, KA 7.1.)*

**PCL-MCQ-07-02** `[7.1.3 · Application]` CPIF: target cost USD 10,000,000, target fee USD 800,000, 50/50 share, actual cost USD 9,400,000. The contractor's fee is:
- A. USD 800,000
- B. USD 1,400,000
- C. USD 1,100,000
- D. USD 500,000

**Answer: C.** *(Rationale at MCQ 7.1-B, KA 7.1.)*

**PCL-MCQ-07-03** `[7.1.4 · Application]` Target cost USD 10,000,000, 50/50 pain-share; actual cost USD 10,800,000. The contractor's pain-share is:
- A. USD 800,000
- B. USD 400,000
- C. USD 0
- D. USD 10,800,000

**Answer: B.** *(Rationale at MCQ 7.1-C, KA 7.1.)*

**PCL-MCQ-07-04** `[7.1.3 · Application]` CPIF: target cost USD 6,000,000, target fee USD 500,000, share ratio **70/30** (client/contractor), actual cost USD 6,800,000. The contractor's fee is:
- A. USD 260,000
- B. USD 500,000
- C. USD 740,000
- D. USD 100,000

**Answer: A.** *(Rationale at MCQ 7.1-D, KA 7.1.)*

**PCL-MCQ-07-05** `[7.1.2 · Recall]` Under a **remeasurement** contract, the client bears ____ risk and the contractor bears ____ risk:
- A. quantity; rate
- B. rate; quantity
- C. all cost; no
- D. no; all cost

**Answer: A.** *(Rationale at MCQ 7.1-E, KA 7.1.)*

**PCL-MCQ-07-06** `[7.2.3 · Application]` LDs are USD 10,000/day; the forecast completion is 20 days late. The LD exposure is:
- A. USD 10,000
- B. USD 20,000
- C. USD 200,000
- D. USD 2,000,000

**Answer: C.** *(Rationale at MCQ 7.2-A, KA 7.2.)*

**PCL-MCQ-07-07** `[7.2.3 · Recall]` In professional practice a liquidated-damages rate is set to represent:
- A. A punitive penalty to deter breach.
- B. The contractor's total revenue.
- C. The retention amount.
- D. A genuine pre-estimate of the client's likely loss.

**Answer: D.** *(Rationale at MCQ 7.2-B, KA 7.2.)*

**PCL-MCQ-07-08** `[7.2.4 · Analysis]` Increasing retention from 5 % to 10 % on a project will, all else equal:
- A. Improve the contractor's cash position.
- B. Deepen the contractor's funding trough (more cash withheld for longer).
- C. Have no cash effect.
- D. Reduce liquidated damages.

**Answer: B.** *(Rationale at MCQ 7.2-C, KA 7.2.)*

**PCL-MCQ-07-09** `[7.2.2 · Application]` A substantiated client-caused delay of **45 days** extends time-related preliminaries at USD 8,000/day and keeps specialist plant on standby at USD 2,000/day. The prolongation quantum is:
- A. USD 360,000
- B. USD 90,000
- C. USD 10,000
- D. USD 450,000

**Answer: D.** *(Rationale at MCQ 7.2-D, KA 7.2.)*

**PCL-MCQ-07-10** `[7.2.4 · Analysis]` A contractor substitutes a **retention bond** for 5 % cash retention. The main commercial effect is:
- A. The contractor's cash position improves: payments are received in full, at the cost of the bond fee.
- B. The client loses all security for defects.
- C. The contractor's cash position worsens.
- D. Liquidated damages no longer apply.

**Answer: A.** *(Rationale at MCQ 7.2-E, KA 7.2.)*

**PCL-MCQ-07-11** `[7.3.4 · Application]` Excavation is 5,000 m³ at USD 12/m³. If actual quantity is 5,400 m³, the remeasured amount is:
- A. USD 60,000
- B. USD 4,800
- C. USD 66,000
- D. USD 64,800

**Answer: D.** *(Rationale at MCQ 7.3-A, KA 7.3.)*

**PCL-MCQ-07-12** `[7.3.3 · Analysis]` Preliminaries are significant on a project that is now forecast to finish late. The main commercial consequence is:
- A. Preliminaries fall automatically.
- B. Time-related preliminaries extend, creating prolongation cost.
- C. The BoQ rates change.
- D. Retention is released early.

**Answer: B.** *(Rationale at MCQ 7.3-B, KA 7.3.)*

**PCL-MCQ-07-13** `[7.3.3 · Application]` A unit rate is built up from first principles: labour 3 hours at USD 40/hour, materials USD 50, plant USD 30; overheads at **10 %** on direct cost; profit at **5 %** on the subtotal including overheads. The tendered rate is:
- A. USD 200
- B. USD 220
- C. USD 231
- D. USD 230

**Answer: C.** *(Rationale at MCQ 7.3-C, KA 7.3.)*

**PCL-MCQ-07-14** `[7.3.1 · Recall]` At tender stage, the principal purpose of issuing a BoQ to bidders is:
- A. To let each tenderer measure its own quantities.
- B. To fix the final contract sum regardless of quantities.
- C. To give all tenderers a common set of quantities to price, making bids comparable.
- D. To replace the drawings and specification.

**Answer: C.** *(Rationale at MCQ 7.3-D, KA 7.3.)*

**PCL-MCQ-07-15** `[7.4.3 · Application]` Gross value of work done is USD 157,200; retention 5 %; previous payments USD 90,000. The amount due this application is:
- A. USD 67,200
- B. USD 59,340
- C. USD 149,340
- D. USD 157,200

**Answer: B.** *(Rationale at MCQ 7.4-A, KA 7.4.)*

**PCL-MCQ-07-16** `[7.4.4 · Analysis]` Earned value, the BoQ valuation and IFRS 15 revenue for the same period differ. This is:
- A. An error to be corrected.
- B. Impossible.
- C. A breach of IFRS 15.
- D. Expected: they value the same progress at different values under different rules; the differences are
  reconciled and meaningful.

**Answer: D.** *(Rationale at MCQ 7.4-B, KA 7.4.)*

**PCL-MCQ-07-17** `[7.4.3 · Application]` Gross value of work done to date is USD 240,000; retention is 5 %; previous payments total USD 180,000. The amount due this application is:
- A. USD 48,000
- B. USD 60,000
- C. USD 228,000
- D. USD 57,000

**Answer: A.** *(Rationale at MCQ 7.4-C, KA 7.4.)*

**PCL-MCQ-07-18** `[7.4.2 · Recall]` The amount that actually drives the contractor's cash inflow each period is:
- A. The applied amount.
- B. The certified amount.
- C. The BoQ tender total.
- D. The earned value.

**Answer: B.** *(Rationale at MCQ 7.4-D, KA 7.4.)*

**PCL-MCQ-07-19** `[7.5.2 · Analysis]` IFRS 15 revenue recognised is USD 6,750,000; certified billing is USD 7,000,000. The position is:
- A. A contract asset of USD 250,000.
- B. A revenue error.
- C. A contract liability of USD 250,000.
- D. Nil.

**Answer: C.** *(Rationale at MCQ 7.5-A, KA 7.5.)*

**PCL-MCQ-07-20** `[7.5.1 · Recall]` Billing and IFRS 15 revenue differ because billing follows ____ while revenue follows ____:
- A. IFRS 15; the payment mechanism
- B. the contract's payment mechanism; IFRS 15 performance
- C. cash; cash
- D. the schedule; the schedule

**Answer: B.** *(Rationale at MCQ 7.5-B, KA 7.5.)*

**PCL-MCQ-07-21** `[7.5.2 · Application]` Cumulative IFRS 15 revenue recognised is USD 4,200,000; cumulative certified billing is USD 3,900,000. The balance-sheet position is:
- A. A contract asset of USD 300,000.
- B. A contract liability of USD 300,000.
- C. A contract asset of USD 8,100,000.
- D. Nil.

**Answer: A.** *(Rationale at MCQ 7.5-C, KA 7.5.)*

**PCL-MCQ-07-22** `[7.5.2 · Analysis]` A project shows a persistent and **growing contract liability** (over-billing). The best commercial reading is:
- A. The project has collected cash ahead of performance and still owes the work to earn it out.
- B. The project has under-billed and cash is tied up in unbilled work.
- C. Revenue has been recognised incorrectly.
- D. The project is certain to be profitable.

**Answer: A.** *(Rationale at MCQ 7.5-D, KA 7.5.)*

**PCL-MCQ-07-23** `[7.1.3 · Analysis]` On a target-cost contract, the site ledger includes the cost of
rectifying defective welding: a category the contract excludes from defined cost. The controls professional
computes the pain-share from the ledger total. The consequence is that:
- A. Nothing; the arithmetic is correct and the share ratio has been applied properly.
- B. The pain-share is computed on a base the contract does not recognise, so the client is charged a share of cost the contractor bears alone.
- C. The contractor's fee floor is breached.
- D. The rectification cost becomes a variation.

**Answer: B.** *(Rationale at MCQ 7.1-F, KA 7.1.)*

**PCL-MCQ-07-24** `[7.1.3 · Application]` The most effective control for keeping a reimbursable claim audit-ready is to:
- A. Reconcile allowable and non-allowable cost at the point each application is prepared.
- B. Code allowable and non-allowable cost separately at source, as cost is incurred.
- C. Retain all invoices in date order for the contract's audit period.
- D. Agree the total with the client's quantity surveyor each month.

**Answer: B.** *(Rationale at MCQ 7.1-G, KA 7.1.)*

**PCL-MCQ-07-25** `[7.2.6 · Analysis]` A client-caused disruption is fully evidenced: contemporaneous records, a critical-path analysis showing the effect, and a quantum built from contract preliminaries rates. The contract makes notice within a stated window a condition precedent, and the notice was given only when the claim was submitted, well after the window closed. The most likely outcome, and the lesson, are:
- A. The claim succeeds; the strength of the evidence cures the late notice.
- B. The claim is reduced in proportion to the delay in notifying.
- C. The entitlement may not survive the window at all, so the claim is never assessed on its merits: notice
  is served first, substantiation is built afterwards.
- D. The claim converts automatically into a variation.

**Answer: C.** *(Rationale at MCQ 7.2-F, KA 7.2.)*

**PCL-MCQ-07-26** `[7.2.6 · Application]` A controls professional learns of an event that *might* be compensable but is not yet quantifiable. The correct first step is to:
- A. Wait until the cost effect can be measured, then notify with the substantiation attached.
- B. Identify the applicable notice provision, diarise the window, and serve the notice in the form and time the contract requires.
- C. Raise it verbally at the next progress meeting and minute it.
- D. Open a claim file and begin the delay analysis.

**Answer: B.** *(Rationale at MCQ 7.2-G, KA 7.2.)*

### Domain 8 — Project Management Lifecycle

**PCL-MCQ-08-01** `[8.1.2 · Recall]` The document that formally authorises a project and the project manager is the:
- A. Business case.
- B. Work breakdown structure.
- C. Project charter.
- D. Cost baseline.

**Answer: C.** *(Rationale at MCQ 8.1-A, KA 8.1.)*

**PCL-MCQ-08-02** `[8.1.3 · Application]` A high-power, low-interest regulator should be:
- A. Kept satisfied.
- B. Managed closely.
- C. Merely monitored.
- D. Ignored.

**Answer: A.** *(Rationale at MCQ 8.1-B, KA 8.1.)*

**PCL-MCQ-08-03** `[8.1.4 · Analysis]` A project is delivered on time and within budget, but the benefit promised in the business case never materialises. The most accurate assessment is:
- A. Total success: the iron triangle was met.
- B. A failure of the charter to authorise the project.
- C. Proof that success criteria are irrelevant once delivery starts.
- D. Project success without benefit success: the two are distinct registers.

**Answer: D.** *(Rationale at MCQ 8.1-C, KA 8.1.)*

**PCL-MCQ-08-04** `[8.1.3 · Application]` A RACI chart for the monthly cost report shows two "A"s against the activity "Approve the report". The correction required is:
- A. Add a third "A" so approval is shared.
- B. Reduce it to exactly one Accountable: a single point of accountability per activity.
- C. Replace both with "R"s.
- D. Delete the activity from the chart.

**Answer: B.** *(Rationale at MCQ 8.1-D, KA 8.1.)*

**PCL-MCQ-08-05** `[8.2.1 · Recall]` The "100 % rule" for a WBS means it:
- A. Must be 100 % complete before work starts.
- B. Guarantees 100 % on-time delivery.
- C. Requires 100 % resource loading.
- D. Captures 100 % of the scope: no more, no less.

**Answer: D.** *(Rationale at MCQ 8.2-A, KA 8.2.)*

**PCL-MCQ-08-06** `[8.2.2 · Analysis]` Why must the cost baseline be phased over the schedule?
- A. To produce Planned Value for earned-value measurement.
- B. To reduce the BAC.
- C. To satisfy IFRS 15.
- D. It need not be.

**Answer: A.** *(Rationale at MCQ 8.2-B, KA 8.2.)*

**PCL-MCQ-08-07** `[8.2.2 · Application]` A cost baseline of USD 2,400,000 is phased evenly over a 12-month schedule. At the end of month 4, Planned Value (`PV`) is:
- A. USD 2,400,000
- B. USD 200,000
- C. USD 800,000
- D. USD 600,000

**Answer: C.** *(Rationale at MCQ 8.2-C, KA 8.2.)*

**PCL-MCQ-08-08** `[8.2.3 · Recall]` Which subsidiary management plan feeds the project's contingency?
- A. The communications plan.
- B. The risk plan.
- C. The quality plan.
- D. The procurement plan.

**Answer: B.** *(Rationale at MCQ 8.2-D, KA 8.2.)*

**PCL-MCQ-08-09** `[8.3.1 · Recall]` Most of a project's budget and actual cost is typically generated during:
- A. Initiating.
- B. Planning.
- C. Executing.
- D. Closing.

**Answer: C.** *(Rationale at MCQ 8.3-A, KA 8.3.)*

**PCL-MCQ-08-10** `[8.3.3 · Analysis]` A package shows a favourable cost variance achieved by cutting quality assurance activities. The controls professional should treat this as:
- A. A false economy likely to return as rework: keep the quality liability visible.
- B. A genuine saving to be banked in the forecast.
- C. Evidence that quality assurance was over-scoped.
- D. Grounds to raise the profit forecast immediately.

**Answer: A.** *(Rationale at MCQ 8.3-B, KA 8.3.)*

**PCL-MCQ-08-11** `[8.3.2 · Application]` During executing, a shortfall in skilled resources will typically show up first in:
- A. The reported `CPI`.
- B. Productivity: a leading indicator that precedes the cost indices.
- C. The final account.
- D. The project charter.

**Answer: B.** *(Rationale at MCQ 8.3-C, KA 8.3.)*

**PCL-MCQ-08-12** `[8.4.2 · Analysis]` A scope change is approved on its direct cost alone, without assessing schedule and risk impact. This violates the principle of:
- A. The 100 % rule.
- B. Earned value.
- C. Integrated change control.
- D. Going concern.

**Answer: C.** *(Rationale at MCQ 8.4-A, KA 8.4.)*

**PCL-MCQ-08-13** `[8.4.1 · Recall]` Monitoring & controlling primarily exists to:
- A. Observe and document the project.
- B. Produce the charter.
- C. Replace executing.
- D. Measure against the baselines and act to change the trajectory.

**Answer: D.** *(Rationale at MCQ 8.4-B, KA 8.4.)*

**PCL-MCQ-08-14** `[8.4.3 · Application]` A project manager proposes accelerating the works to recover schedule slippage. Before approval, the controls professional should:
- A. Assess only the schedule benefit: schedule is the priority.
- B. Assess the acceleration's cost, quality and risk impacts alongside the schedule gain: the constraints
  trade off.
- C. Decline any assessment, since acceleration is an executing matter.
- D. Reduce scope automatically to fund the acceleration.

**Answer: B.** *(Rationale at MCQ 8.4-C, KA 8.4.)*

**PCL-MCQ-08-15** `[8.4.1 · Recall]` The repeating monthly cycle of monitoring & controlling runs:
- A. Measure → analyse → forecast → act → report.
- B. Report → act → forecast → analyse → measure.
- C. Forecast → measure → report → analyse → act.
- D. Act → measure → report → forecast → analyse.

**Answer: A.** *(Rationale at MCQ 8.4-D, KA 8.4.)*

**PCL-MCQ-08-16** `[8.5.3 · Analysis]` Why is the controls function the natural custodian of quantitative lessons learned?
- A. It writes the contract.
- B. It approves the charter.
- C. It runs procurement.
- D. It holds the performance data (actual CPI, productivity, unit costs) that feeds future estimates and models.

**Answer: D.** *(Rationale at MCQ 8.5-A, KA 8.5.)*

**PCL-MCQ-08-17** `[8.5.2 · Application]` A contract closes with an original value of USD 800,000, approved variations of USD 60,000 and an agreed claim of USD 20,000. Retention of 5 % was withheld, with half released at practical completion. The retention still to collect after the defects period is:
- A. USD 44,000
- B. USD 22,000
- C. USD 21,500
- D. USD 20,000

**Answer: B.** *(Rationale at MCQ 8.5-B, KA 8.5.)*

**PCL-MCQ-08-18** `[8.5.1 · Recall]` Orderly closure matters chiefly because:
- A. It guarantees the project made a profit.
- B. Unclosed contracts and unresolved claims are liabilities that linger.
- C. It removes the need for lessons learned.
- D. It allows the baselines to be revised retrospectively.

**Answer: B.** *(Rationale at MCQ 8.5-C, KA 8.5.)*

**PCL-MCQ-08-19** `[8.6.2 · Analysis]` Building a product in usable slices, each adding a new working part to the whole, describes:
- A. Iterative delivery.
- B. Predictive delivery.
- C. Waterfall.
- D. Incremental delivery.

**Answer: D.** *(Rationale at MCQ 8.6-A, KA 8.6.)*

**PCL-MCQ-08-20** `[8.6.2 · Analysis]` Refining the *same* product over repeated passes, improving it each time, describes:
- A. Iterative delivery.
- B. Incremental delivery.
- C. Framework contracting.
- D. Remeasurement.

**Answer: A.** *(Rationale at MCQ 8.6-B, KA 8.6.)*

**PCL-MCQ-08-21** `[8.6.4 · Application]` A programme with well-defined, regulated civils and uncertain, change-prone software is best delivered:
- A. Fully predictive.
- B. Fully adaptive.
- C. Hybrid: predictive governance over the civils, adaptive execution of the software.
- D. Without any baseline.

**Answer: C.** *(Rationale at MCQ 8.6-C, KA 8.6.)*

**PCL-MCQ-08-22** `[8.6.1 · Recall]` The two project conditions that determine where a project should sit on the predictive-to-adaptive spectrum are:
- A. Team size and contract value.
- B. Client preference and industry custom.
- C. Requirements certainty and change rate.
- D. Budget size and project duration.

**Answer: C.** *(Rationale at MCQ 8.6-D, KA 8.6.)*

**PCL-MCQ-08-23** `[8.6.4 · Application]` An adaptive work stream is funded for 20 Sprints at USD 200,000 per Sprint. After 8 Sprints, achieved velocity is 25 points per Sprint and 600 backlog points remain. If velocity holds and no scope is cut, the funding gap is:
- A. USD 2,400,000
- B. USD 800,000
- C. USD 4,800,000
- D. USD 3,200,000

**Answer: A.** *(Rationale at MCQ 8.6-E, KA 8.6.)*

**PCL-MCQ-08-24** `[8.4.2 · Analysis]` Work on a client-instructed change begins immediately because the site team judges the delay of waiting to be more costly than the change itself. The change is later approved by the CCB at the assessed value. The correct treatment is:
- A. None needed; the approval regularised the position.
- B. Record it as an emergency change at the time, with the reason and the person who authorised it, and have
  it ratified by the CCB as an emergency: not folded into the ordinary sequence.
- C. Approve it retrospectively at the project manager's delegated level, since the CCB agreed the value.
- D. Treat the early start as scope creep and reverse the work.

**Answer: B.** *(Rationale at MCQ 8.4-E, KA 8.4.)*

**PCL-MCQ-08-25** `[8.4.2 · Application]` A change request arrives with a priced direct cost, a funding source and a recommendation, but no assessment of its effect on the schedule, on risk, or on the benefit case. The change authority should:
- A. Approve it, since the cost and the funding are both established.
- B. Approve it conditionally, with the missing assessments to follow.
- C. Return it: an incomplete request cannot be decided, only guessed at.
- D. Delegate the decision down to the project manager to save time.

**Answer: C.** *(Rationale at MCQ 8.4-F, KA 8.4.)*

**PCL-MCQ-08-26** `[8.5.1 · Analysis]` A closure checklist records "records archived" against a project file share, and the team demobilises. Years later a dispute arises and the schedule updates cannot be opened. The defect in the closure was that:
- A. The records should have been printed.
- B. The archive was never given a stated period, a named custodian in the permanent organisation, or a tested retrieval route.
- C. Archiving belongs to the operator, not the project.
- D. Nothing; by then the records are beyond any reasonable retention period.

**Answer: B.** *(Rationale at MCQ 8.5-D, KA 8.5.)*

**PCL-MCQ-08-27** `[8.5.1 · Application]` A project's records are subject to a contract audit provision, an organisational accounting-retention requirement, and a claim that is being prepared by a subcontractor. The retention period applied should be:
- A. The contract's audit period, because the contract governs the project.
- B. The organisation's accounting requirement, because it applies to all records.
- C. The shortest of the applicable periods, to limit storage cost and data held.
- D. The longest of the applicable periods, extended for the records touched by the foreseeable dispute until it is resolved.

**Answer: D.** *(Rationale at MCQ 8.5-E, KA 8.5.)*

### Domain 9 — Agile, Scrum & Adaptive Delivery for Project Controls

**PCL-MCQ-09-01** `[9.1.2 · Recall]` The three pillars of empirical process control are:
- A. Transparency, inspection, adaptation.
- B. Plan, do, check.
- C. Scope, time, cost.
- D. People, process, tools.

**Answer: A.** *(Rationale at MCQ 9.1-A, KA 9.1.)*

**PCL-MCQ-09-02** `[9.1.4 · Analysis]` Which work is *least* suited to a purely adaptive approach?
- A. A product with evolving requirements.
- B. Software delivered in valuable increments.
- C. A regulated civil structure with no value until complete.
- D. An R&D prototype.

**Answer: C.** *(Rationale at MCQ 9.1-B, KA 9.1.)*

**PCL-MCQ-09-03** `[9.1.3 · Application]` Under adaptive planning, a programme funds a stable team for **12 two-week Sprints at USD 90,000 per Sprint**, letting scope flex to fit. The fixed cost envelope is:
- A. USD 90,000
- B. USD 540,000
- C. USD 2,160,000
- D. USD 1,080,000

**Answer: D.** *(Rationale at MCQ 9.1-C, KA 9.1.)*

**PCL-MCQ-09-04** `[9.1.1 · Recall]` The Agile Manifesto's stance on planning is best described as:
- A. Plans are prohibited in agile delivery.
- B. Responding to change is valued over following a plan, while the plan still has value.
- C. Following the plan is valued over responding to change.
- D. Plans must be fixed before any work starts.

**Answer: B.** *(Rationale at MCQ 9.1-D, KA 9.1.)*

**PCL-MCQ-09-05** `[9.2.4 · Recall]` The commitment associated with the Increment is the:
- A. Sprint Goal.
- B. Product Goal.
- C. Definition of Done.
- D. Velocity.

**Answer: C.** *(Rationale at MCQ 9.2-A, KA 9.2.)*

**PCL-MCQ-09-06** `[9.2.2 · Recall]` Accountability for maximising the value of the product belongs to the:
- A. Product Owner.
- B. Scrum Master.
- C. Developers.
- D. Sponsor.

**Answer: A.** *(Rationale at MCQ 9.2-B, KA 9.2.)*

**PCL-MCQ-09-07** `[9.2.3 · Application]` A team runs **two-week Sprints**, each producing at least one usable Increment, with a new Sprint starting immediately after the previous one. Over a 26-week release window, the minimum number of Increments is:
- A. 26
- B. 13
- C. 12
- D. 6

**Answer: B.** *(Rationale at MCQ 9.2-C, KA 9.2.)*

**PCL-MCQ-09-08** `[9.2.2 · Analysis]` A programme stakeholder asks the Scrum Master to assign this Sprint's tasks to individual Developers. The request misreads Scrum because:
- A. Only the Product Owner assigns tasks.
- B. Tasks may only be assigned at the Sprint Review.
- C. The Scrum Master may assign tasks but only in writing.
- D. The Developers own the Sprint Backlog plan; the Scrum Master is a coach and impediment-remover, not a manager over the team.

**Answer: D.** *(Rationale at MCQ 9.2-D, KA 9.2.)*

**PCL-MCQ-09-09** `[9.3.3 · Application]` A team completes 28, 31, 30, 32, 29 points over five Sprints; 240 points remain. The expected Sprints remaining (at average velocity) is:
- A. 5
- B. 6
- C. 8
- D. 10

**Answer: C.** *(Rationale at MCQ 9.3-A, KA 9.3.)*

**PCL-MCQ-09-10** `[9.3.4 · Analysis]` Why is a burnup often preferred to a burndown for release forecasting?
- A. It is simpler.
- B. It hides added scope.
- C. It requires no velocity.
- D. It shows scope change (the moving total line), not just progress.

**Answer: D.** *(Rationale at MCQ 9.3-B, KA 9.3.)*

**PCL-MCQ-09-11** `[9.3.5 · Recall]` In the agile "inverted" iron triangle, what is fixed?
- A. Time and cost.
- B. Scope.
- C. Quality only.
- D. Nothing.

**Answer: A.** *(Rationale at MCQ 9.3-C, KA 9.3.)*

**PCL-MCQ-09-12** `[9.3.3 · Application]` A team's velocity is **25 points/Sprint**. Original release scope was 300 points; by the end of Sprint 6, **150 points** are complete and **50 points of new scope** are approved. The Sprints remaining are:
- A. 6
- B. 8
- C. 14
- D. 12

**Answer: B.** *(Rationale at MCQ 9.3-D, KA 9.3.)*

**PCL-MCQ-09-13** `[9.3.4 · Analysis]` On a cumulative flow diagram, the "in progress" band is steadily widening while the "done" band's slope is flat. The best reading is:
- A. Throughput is rising healthily.
- B. Scope has been removed from the release.
- C. Work is being started faster than it is finished: WIP is growing at a bottleneck.
- D. Cycle time is falling.

**Answer: C.** *(Rationale at MCQ 9.3-E, KA 9.3.)*

**PCL-MCQ-09-14** `[9.4.1 · Analysis]` Imposing a WIP limit typically:
- A. Slows delivery by restricting work.
- B. Has no effect on cycle time.
- C. Speeds completion by cutting context-switching and queueing.
- D. Increases work in progress.

**Answer: C.** *(Rationale at MCQ 9.4-A, KA 9.4.)*

**PCL-MCQ-09-15** `[9.4.3 · Recall]` The "Agile Release Train" is a concept most associated with:
- A. Kanban.
- B. Waterfall.
- C. Little's Law.
- D. SAFe.

**Answer: D.** *(Rationale at MCQ 9.4-B, KA 9.4.)*

**PCL-MCQ-09-16** `[9.4.1 · Application]` A Kanban team carries **12 items** of work in progress and completes **3 items per day**. By Little's Law (cycle time ≈ WIP ÷ throughput), the average cycle time is approximately:
- A. 4 days
- B. 36 days
- C. 0.25 days
- D. 9 days

**Answer: A.** *(Rationale at MCQ 9.4-C, KA 9.4.)*

**PCL-MCQ-09-17** `[9.4.2 · Recall]` In Lean thinking, "waste" is best defined as:
- A. Any activity the customer would not pay for, such as waiting, rework and hand-offs.
- B. Only physical scrap material.
- C. Any spending above the original budget.
- D. All documentation.

**Answer: A.** *(Rationale at MCQ 9.4-D, KA 9.4.)*

**PCL-MCQ-09-18** `[9.5.3 · Application]` Release `BAC` USD 600,000; 300 points planned; 120 done; `AC` USD 320,000; 150 planned done. The `CPI` is:
- A. 1.33
- B. 0.80
- C. 0.75
- D. 0.40

**Answer: C.** *(Rationale at MCQ 9.5-A, KA 9.5.)*

**PCL-MCQ-09-19** `[9.5.3 · Analysis]` The central assumption/limit when applying AgileEVM is that:
- A. Story points equal hours.
- B. The metrics are meaningful only against a defined release scope/budget and must be rebaselined transparently when scope flexes.
- C. Velocity never changes.
- D. It cannot use Domain 6 formulae.

**Answer: B.** *(Rationale at MCQ 9.5-B, KA 9.5.)*

**PCL-MCQ-09-20** `[9.5.4 · Analysis]` AgileEVM `% complete` is 40 % but cost-to-cost `% complete` is 35 % on an over-time contract. The controls professional should:
- A. Reconcile the two, evidence both sets of inputs, explain the cause of the difference, and refer the measure of progress used for recognition to finance as the accounting-policy owner.
- B. Recognise revenue at 40 %, the AgileEVM measure.
- C. Change the recognition basis to whichever view the contract's billing follows.
- D. Average the two to 37.5 %.

**Answer: A.** *(Rationale at MCQ 9.5-C, KA 9.5.)*

**PCL-MCQ-09-21** `[9.5.1 · Application]` A capacity-funded team costs **USD 75,000 per Sprint**; `AC` to date is USD 300,000; the velocity forecast shows **6 Sprints remaining**. The `ETC` and `EAC` are:
- A. `ETC` USD 450,000; `EAC` USD 750,000
- B. `ETC` USD 450,000; `EAC` USD 450,000
- C. `ETC` USD 300,000; `EAC` USD 750,000
- D. `ETC` USD 750,000; `EAC` USD 1,050,000

**Answer: A.** *(Rationale at MCQ 9.5-D, KA 9.5.)*

**PCL-MCQ-09-22** `[9.5.3 · Recall]` In AgileEVM, `EV` at a data date is computed as:
- A. (story points planned by the data date / total planned points) × `BAC`.
- B. story points completed × cost per Sprint.
- C. `% complete` × `AC`.
- D. (story points completed / total planned points) × `BAC`.

**Answer: D.** *(Rationale at MCQ 9.5-E, KA 9.5.)*

**PCL-MCQ-09-23** `[9.6.3 · Analysis]` Which contract form best fits agile's variable scope?
- A. Fixed-price, fixed-scope lump sum.
- B. Remeasurement of fixed civil quantities.
- C. Capped time & materials (pay for capacity, cap exposure).
- D. A performance bond.

**Answer: C.** *(Rationale at MCQ 9.6-A, KA 9.6.)*

**PCL-MCQ-09-24** `[9.6.4 · Analysis]` The claim that "agile has no audit trail" is:
- A. True: agile avoids documentation.
- B. False: backlog, Sprint records, Increments and Definition of Done form a contemporaneous trail.
- C. True for Scrum only.
- D. Irrelevant to controls.

**Answer: B.** *(Rationale at MCQ 9.6-B, KA 9.6.)*

**PCL-MCQ-09-25** `[9.6.3 · Application]` An agile team is engaged on **capped T&M** at USD 100,000 per month with a cap of USD 1,300,000. Delivery takes **14 months**. The client pays:
- A. USD 1,400,000
- B. USD 1,300,000
- C. USD 1,200,000
- D. USD 100,000

**Answer: B.** *(Rationale at MCQ 9.6-C, KA 9.6.)*

**PCL-MCQ-09-26** `[9.6.2 · Recall]` To make agile work legible at a predictive phase gate, the controls professional reports:
- A. Raw Sprint Backlogs for the board to interpret.
- B. Only the original fixed baseline.
- C. Nothing: agile work is exempt from gates.
- D. Value delivered, run-rate and forecast completion, mapped from Sprints/releases to the gate's milestones.

**Answer: D.** *(Rationale at MCQ 9.6-D, KA 9.6.)*

### Domain 10 — Project Scheduling (in depth)

**PCL-MCQ-10-01** `[10.1.4 · Application]` With `O = 4`, `M = 6`, `P = 14`, the PERT expected duration is:
- A. 6 days
- B. 7 days
- C. 8 days
- D. 24 days

**Answer: B.** *(Rationale at MCQ 10.1-A, KA 10.1.)*

**PCL-MCQ-10-02** `[10.1.2 · Recall]` "B cannot start until A finishes" is which dependency?
- A. Start-to-Start
- B. Finish-to-Finish
- C. Finish-to-Start
- D. Start-to-Finish

**Answer: C.** *(Rationale at MCQ 10.1-B, KA 10.1.)*

**PCL-MCQ-10-03** `[10.1.3 · Application]` Activity A finishes at the end of day 10. Its successor B is linked **FS + 3 days** (a lag for curing time). B's earliest start is:
- A. Day 10
- B. Day 7
- C. Day 3
- D. Day 13

**Answer: D.** *(Rationale at MCQ 10.1-C, KA 10.1.)*

**PCL-MCQ-10-04** `[10.1.2 · Analysis]` When fast-tracking a schedule, which dependency may legitimately be relaxed or overlapped?
- A. A mandatory dependency (e.g. concrete curing before loading).
- B. A discretionary dependency reflecting preferred sequencing.
- C. An external dependency (e.g. a permit).
- D. None: all dependencies are equally fixed.

**Answer: B.** *(Rationale at MCQ 10.1-D, KA 10.1.)*

**PCL-MCQ-10-05** `[10.2.5 · Application]` In the worked network (A3, B4, C2, D5, E3, F2; A→B→D→F, A→C→E→F), the project duration is:
- A. 12 days
- B. 10 days
- C. 14 days
- D. 8 days

**Answer: C.** *(Rationale at MCQ 10.2-A, KA 10.2.)*

**PCL-MCQ-10-06** `[10.2.4 · Analysis]` Activity C has total float 4 but free float 0. This means delaying C:
- A. Delays the project by 4 days.
- B. Delays its successor E (uses shared float), but not the project: up to the limit.
- C. Has no effect at all.
- D. Is impossible.

**Answer: B.** *(Rationale at MCQ 10.2-B, KA 10.2.)*

**PCL-MCQ-10-07** `[10.2.4 · Application]` Activity D has `ES` = 7 and `LS` = 7. Its total float is:
- A. 7
- B. 5
- C. 14
- D. 0

**Answer: D.** *(Rationale at MCQ 10.2-C, KA 10.2.)*

**PCL-MCQ-10-08** `[10.2.4 · Application]` An activity has `EF` = 14; its two successors have `ES` of 17 and 19. Its free float is:
- A. 3 days
- B. 5 days
- C. 0 days
- D. 4 days

**Answer: A.** *(Rationale at MCQ 10.2-D, KA 10.2.)*

**PCL-MCQ-10-09** `[10.2.3 · Recall]` In the backward pass, an activity's `LF` equals:
- A. The earliest `LS` of its successors.
- B. The latest `LS` of its successors.
- C. The latest `EF` of its predecessors.
- D. The project start date.

**Answer: A.** *(Rationale at MCQ 10.2-E, KA 10.2.)*

**PCL-MCQ-10-10** `[10.3.1 · Analysis]` Crashing is most effective when applied to:
- A. Any activity with float.
- B. The longest-duration activity regardless of path.
- C. Critical-path activities with the lowest cost per time saved.
- D. Non-critical activities.

**Answer: C.** *(Rationale at MCQ 10.3-A, KA 10.3.)*

**PCL-MCQ-10-11** `[10.3.2 · Analysis]` Fast-tracking primarily trades:
- A. Cost for time.
- B. Quality for schedule.
- C. Scope for cost.
- D. Time for risk (overlapping raises rework risk).

**Answer: D.** *(Rationale at MCQ 10.3-B, KA 10.3.)*

**PCL-MCQ-10-12** `[10.3.3 · Recall]` Which technique may extend the project duration?
- A. Resource smoothing.
- B. Resource levelling.
- C. Fast-tracking.
- D. Crashing.

**Answer: B.** *(Rationale at MCQ 10.3-C, KA 10.3.)*

**PCL-MCQ-10-13** `[10.3.1 · Application]` A project's critical path is 20 days; the parallel path is 17 days. Two critical activities can be crashed: **X** at USD 3,000/day (max 2 days) and **Y** at USD 7,000/day (max 2 days). The least-cost way to save **2 days** is:
- A. Crash X by 2 days for USD 6,000.
- B. Crash Y by 2 days for USD 14,000.
- C. Crash X and Y by 1 day each for USD 10,000.
- D. Crash X by 1 day for USD 3,000.

**Answer: A.** *(Rationale at MCQ 10.3-D, KA 10.3.)*

**PCL-MCQ-10-14** `[10.3.4 · Analysis]` A Monte Carlo schedule-risk analysis returns **P50 = 30 days** and **P80 = 33 days** against a deterministic duration of 30 days. The professional posture is to:
- A. Commit externally at 33 days, manage internally to 30, and hold the 3-day gap as explicit schedule contingency.
- B. Commit externally at 30 days, since that is the deterministic answer.
- C. Commit externally at 27 days to motivate the team.
- D. Ignore the simulation: the critical path is already known.

**Answer: A.** *(Rationale at MCQ 10.3-E, KA 10.3.)*

**PCL-MCQ-10-15** `[10.4.2 · Analysis]` Why read both the network view and the earned-value view of schedule?
- A. They always agree.
- B. Only one is ever correct.
- C. Each covers the other's blind spot: EVM misses the critical path; the network does not aggregate
  cost/performance.
- D. To duplicate effort.

**Answer: C.** *(Rationale at MCQ 10.4-A, KA 10.4.)*

**PCL-MCQ-10-16** `[10.4.3 · Recall]` In a hybrid programme, Sprints and releases are best treated as:
- A. Incompatible with scheduling.
- B. Schedule increments mapped to milestones.
- C. A replacement for the critical path.
- D. Cost accounts.

**Answer: B.** *(Rationale at MCQ 10.4-B, KA 10.4.)*

**PCL-MCQ-10-17** `[10.4.1 · Application]` A baseline forecasts completion at day 40. At the data date, a **critical** activity has finished **3 days late** and a non-critical activity with **5 days of total float** has finished **2 days late**. After recalculating the network, the completion forecast is:
- A. Day 45
- B. Day 40
- C. Day 42
- D. Day 43

**Answer: D.** *(Rationale at MCQ 10.4-C, KA 10.4.)*

**PCL-MCQ-10-18** `[10.4.1 · Analysis]` A schedule is updated with several actual finish dates missing and key milestones held on fixed date constraints. The main consequence is:
- A. The forecast is more reliable because the milestone dates are protected.
- B. Total float increases across the network.
- C. The network can no longer recalculate honestly: the forecast completion and current critical path are
  corrupted.
- D. The baseline is automatically re-approved.

**Answer: C.** *(Rationale at MCQ 10.4-D, KA 10.4.)*

### Domain 11 — Business Process Cycles (O2C, P2P & the control environment)

**PCL-MCQ-11-01** `[11.1.1 · Recall]` Which sequence correctly orders O2C stages?
- A. Invoice → order → collect → deliver.
- B. Order → credit check → fulfil → invoice → collect → apply cash.
- C. Order → pay → receive → invoice.
- D. Requisition → PO → receipt → payment.

**Answer: B.** *(Rationale at MCQ 11.1-A, KA 11.1.)*

**PCL-MCQ-11-02** `[11.1.3 · Analysis]` A growing overdue receivables balance is best read as:
- A. Always an accounting error.
- B. A reason to recognise more revenue.
- C. Irrelevant to controls.
- D. A leading indicator of cash risk and possible billing/revenue disputes.

**Answer: D.** *(Rationale at MCQ 11.1-B, KA 11.1.)*

**PCL-MCQ-11-03** `[11.1.3 · Application]` A business with annual revenue of USD 18,250,000 (USD 50,000 a day) reduces its days sales outstanding (DSO) from 60 to 46 days through billing and dunning discipline. The cash freed is approximately:
- A. USD 50,000
- B. USD 700,000
- C. USD 2,300,000
- D. USD 3,000,000

**Answer: B.** *(Rationale at MCQ 11.1-C, KA 11.1.)*

**PCL-MCQ-11-04** `[11.1.2 · Recall]` Which O2C control also gates step 1 of the IFRS 15 revenue model?
- A. Credit control: the collectability assessment.
- B. Cash application.
- C. The dunning cadence.
- D. The three-way match.

**Answer: A.** *(Rationale at MCQ 11.1-D, KA 11.1.)*

**PCL-MCQ-11-05** `[11.A.1 · Application]` A business runs a DSO of **50 days**, a DIO of **25 days** and a DPO of **35 days**. Its cash-conversion cycle is:
- A. 15 days
- B. 40 days
- C. 60 days
- D. 110 days

**Answer: B.** *(Rationale at MCQ 11.1-E, KA 11.1.)*

**PCL-MCQ-11-06** `[11.2.2 · Application]` A PO is 100 units at USD 50; goods receipt 100 units; invoice 100 units at USD 55. The three-way match:
- A. Passes: quantities agree.
- B. Fails on quantity.
- C. Fails on price: a USD 500 exception is held for investigation before payment.
- D. Is unnecessary.

**Answer: C.** *(Rationale at MCQ 11.2-A, KA 11.2.)*

**PCL-MCQ-11-07** `[11.2.1 · Analysis]` In P2P, the purchase order corresponds to which cost-control state?
- A. Actual.
- B. Accrual.
- C. Commitment.
- D. Payment only.

**Answer: C.** *(Rationale at MCQ 11.2-B, KA 11.2.)*

**PCL-MCQ-11-08** `[11.2.2 · Application]` A PO orders 200 units at USD 30 (USD 6,000). The goods-receipt note records 190 units received; the supplier invoices 200 units at USD 30 (USD 6,000). The amount properly payable once the exception is resolved is:
- A. USD 6,000
- B. USD 5,700
- C. USD 300
- D. Nil: the whole invoice is rejected permanently.

**Answer: B.** *(Rationale at MCQ 11.2-C, KA 11.2.)*

**PCL-MCQ-11-09** `[11.2.3 · Analysis]` A large goods-received-not-invoiced (GRNI) balance at period end primarily drives:
- A. The commitment figure.
- B. A receivable from the supplier.
- C. The actual cost, since invoices will follow.
- D. The accrual that makes cost-to-date true.

**Answer: D.** *(Rationale at MCQ 11.2-D, KA 11.2.)*

**PCL-MCQ-11-10** `[11.2.2 · Application]` A PO orders **300 units at USD 40** (USD 12,000). The goods-receipt note records **280 units received**; the supplier invoices **300 units at USD 42** (USD 12,600). The amount properly payable once the exceptions are resolved is:
- A. USD 12,600
- B. USD 12,000
- C. USD 11,760
- D. USD 11,200

**Answer: D.** *(Rationale at MCQ 11.2-E, KA 11.2.)*

**PCL-MCQ-11-11** `[11.2.3 · Application]` GRNI of **USD 180,000** was accrued at month-end. Next month, matched invoices arrive covering **USD 150,000** of it. The correct treatment of the remaining USD 30,000 is to:
- A. Investigate it: reverse it if it is an over-accrual, let it stand if the supplier is simply slow to
  invoice.
- B. Release the full USD 180,000 accrual, since invoices have started arriving.
- C. Reclassify it as a commitment.
- D. Write it off to a cost variance.

**Answer: A.** *(Rationale at MCQ 11.2-F, KA 11.2.)*

**PCL-MCQ-11-12** `[11.3.2 · Analysis]` Allowing one clerk to raise a PO, confirm receipt and approve the invoice for payment violates:
- A. The 100 % rule.
- B. IFRS 15.
- C. Segregation of duties.
- D. The three-point estimate.

**Answer: C.** *(Rationale at MCQ 11.3-A, KA 11.3.)*

**PCL-MCQ-11-13** `[11.3.1 · Recall]` A three-way match is an example of a ____ control; a monthly reconciliation is a ____ control.
- A. preventive; detective
- B. detective; preventive
- C. preventive; preventive
- D. detective; detective

**Answer: A.** *(Rationale at MCQ 11.3-B, KA 11.3.)*

**PCL-MCQ-11-14** `[11.3.3 · Application]` A process-mining pass over the ERP event log shows a number of invoices were paid without the three-way match step ever occurring. This finding is best described as:
- A. A preventive control stopping the payments.
- B. A reason to disable the match, since payments went through anyway.
- C. Conclusive proof of fraud.
- D. Detective use of the audit trail, revealing that a control was bypassed.

**Answer: D.** *(Rationale at MCQ 11.3-C, KA 11.3.)*

**PCL-MCQ-11-15** `[11.3.2 · Recall]` Under segregation of duties in the O2C cycle, the person who bills customers should not also:
- A. Receive and apply the cash and write off debts.
- B. Prepare the monthly cost report.
- C. Raise purchase requisitions.
- D. Maintain the schedule baseline.

**Answer: A.** *(Rationale at MCQ 11.3-D, KA 11.3.)*

**PCL-MCQ-11-16** `[11.A.4 · Analysis]` A duplicate-payment monitor fires 800 alerts a month, of which 16 are confirmed duplicates (2 % precision), and the team has begun skimming the queue. The best response is to:
- A. Switch the monitor off: 2 % precision proves the risk is not real.
- B. Widen the matching criteria so no duplicate can possibly be missed.
- C. Treat each threshold as a tolerance decision: measure the false-positive rate, retune the detection logic, and give every monitor a named owner and response path.
- D. Instruct the team to work all 800 alerts harder each month.

**Answer: C.** *(Rationale at MCQ 11.3-E, KA 11.3.)*

### Domain 12 — Risk Management for Project Controls

**PCL-MCQ-12-01** `[12.1.1 · Recall]` A risk that has already occurred is properly called:
- A. An opportunity.
- B. An issue.
- C. Appetite.
- D. Contingency.

**Answer: B.** *(Rationale at MCQ 12.1-A, KA 12.1.)*

**PCL-MCQ-12-02** `[12.1.3 · Analysis]` Setting contingency far below the organisation's risk appetite primarily:
- A. Ties up capital unnecessarily.
- B. Has no effect.
- C. Exposes the organisation to under-funded risk.
- D. Breaches ISO 31000 automatically.

**Answer: C.** *(Rationale at MCQ 12.1-B, KA 12.1.)*

**PCL-MCQ-12-03** `[12.1.1 · Recall]` An uncertain event that, if it occurs, would have a *positive* effect on objectives is:
- A. An issue.
- B. A constraint.
- C. A tolerance.
- D. An opportunity.

**Answer: D.** *(Rationale at MCQ 12.1-C, KA 12.1.)*

**PCL-MCQ-12-04** `[12.1.2 · Application]` A team updates its risk register once a quarter as a standalone exercise, disconnected from the estimate, schedule and forecast. The ISO 31000 principle this practice most clearly departs from is:
- A. Integration: risk management belongs inside decision-making, feeding the estimate, schedule and forecast.
- B. Proportionality: the effort exceeds the stakes.
- C. None: a quarterly cadence is a matter of judgement, not of principle.
- D. Appetite: the tolerance thresholds are set too low.

**Answer: A.** *(Rationale at MCQ 12.1-D, KA 12.1.)*

**PCL-MCQ-12-05** `[12.1.3 · Application]` The board's appetite statement caps any single project's P80 risk exposure at **1.5 %** of group profit of **USD 60,000,000**. A tender's quantified exposure comes out at **P80 = USD 1,020,000**. The correct conclusion is:
- A. Within appetite: USD 1,020,000 is far below the USD 60,000,000 profit.
- B. Above appetite: the ceiling is `1.5 % × 60,000,000 = USD 900,000`, so risks must be treated or
  transferred to bring the re-run P80 inside it before the bid proceeds.
- C. Within appetite: the register's EMV sum, not the P80, is tested against the ceiling.
- D. Indeterminate: appetite statements are qualitative and cannot be tested numerically.

**Answer: B.** *(Rationale at MCQ 12.1-E, KA 12.1.)*

**PCL-MCQ-12-06** `[12.2.3 · Application]` A risk has a 30 % probability and a USD 200,000 impact. Its EMV is:
- A. USD 200,000
- B. USD 30,000
- C. USD 60,000
- D. USD 230,000

**Answer: C.** *(Rationale at MCQ 12.2-A, KA 12.2.)*

**PCL-MCQ-12-07** `[12.2.3 · Analysis]` Why is a P80 contingency from Monte Carlo usually higher than the simple sum of EMVs?
- A. Monte Carlo ignores probability.
- B. It captures the chance that several risks coincide, beyond the expected average.
- C. EMV double-counts risks.
- D. They are always equal.

**Answer: B.** *(Rationale at MCQ 12.2-B, KA 12.2.)*

**PCL-MCQ-12-08** `[12.2.4 · Recall]` Buying insurance against a risk is which response strategy?
- A. Avoid.
- B. Accept.
- C. Mitigate.
- D. Transfer.

**Answer: D.** *(Rationale at MCQ 12.2-C, KA 12.2.)*

**PCL-MCQ-12-09** `[12.2.4 · Application]` A risk has a 40 % probability and a USD 250,000 impact. A proposed mitigation costs USD 35,000 and would cut the probability to 20 %. On EMV grounds the mitigation is:
- A. Worthwhile: the mitigate path costs USD 85,000 against USD 100,000 for accepting.
- B. Not worthwhile: it costs USD 35,000 with no return.
- C. Not worthwhile: the mitigate path costs USD 135,000.
- D. Worthwhile only if the probability falls to zero.

**Answer: A.** *(Rationale at MCQ 12.2-D, KA 12.2.)*

**PCL-MCQ-12-10** `[12.2.3 · Analysis]` A register holds two threats (30 % × USD 200,000 and 20 % × USD
150,000) and one opportunity: a 40 % chance of a USD 50,000 saving. The net EMV basis for contingency is:
- A. USD 90,000
- B. USD 110,000
- C. USD 400,000
- D. USD 70,000

**Answer: D.** *(Rationale at MCQ 12.2-E, KA 12.2.)*

**PCL-MCQ-12-11** `[12.2.3 · Analysis]` Two register risks (an earthworks delay and a paving-window loss)
share one driver: the same wet season. When the Monte Carlo model correlates them instead of treating them as
independent, the effect on the results is:
- A. The EMV sum rises and the P80 falls.
- B. Both the EMV sum and the P80 are unchanged.
- C. The EMV sum falls, because one risk now absorbs the other.
- D. The EMV sum is unchanged, but the P80 rises: coinciding risks fatten the upper tail.

**Answer: D.** *(Rationale at MCQ 12.2-F, KA 12.2.)*

**PCL-MCQ-12-12** `[12.3.2 · Analysis]` Which reserve is populated by the risk register?
- A. Management reserve.
- B. Contingency reserve.
- C. Neither.
- D. Both equally.

**Answer: B.** *(Rationale at MCQ 12.3-A, KA 12.3.)*

**PCL-MCQ-12-13** `[12.3.1 · Analysis]` Why might contingency be set at a Monte Carlo P80 rather than the simple EMV sum?
- A. P80 is always lower.
- B. EMV is not a risk measure.
- C. To fund an 80 %-confidence outcome that reflects risks coinciding, not just the average.
- D. They are identical.

**Answer: C.** *(Rationale at MCQ 12.3-B, KA 12.3.)*

**PCL-MCQ-12-14** `[12.3.3 · Application]` Contingency was set at a Monte Carlo P80 of USD 300,000. Materialised risks have drawn USD 80,000 and then USD 60,000, and a re-run of the register puts the remaining exposure at a P80 of USD 175,000. The position to report is:
- A. Remaining contingency USD 160,000: a USD 15,000 shortfall against remaining exposure, to escalate.
- B. Remaining contingency USD 160,000: adequate, since the original USD 300,000 exceeded USD 175,000.
- C. Remaining contingency USD 220,000: comfortable headroom.
- D. No reporting needed: drawing down contingency is normal.

**Answer: A.** *(Rationale at MCQ 12.3-C, KA 12.3.)*

**PCL-MCQ-12-15** `[12.3.2 · Recall]` The management reserve is best described as funding that is:
- A. Inside the cost baseline and controlled by the project manager.
- B. Drawn automatically whenever a register risk materialises.
- C. Derived directly from the risk register's EMV sum.
- D. Outside the baseline, management-controlled, for unidentified (unknown-unknown) risk.

**Answer: D.** *(Rationale at MCQ 12.3-D, KA 12.3.)*

**PCL-MCQ-12-16** `[12.3.1 · Application]` A quantified register has an EMV sum of **USD 220,000**; a Monte Carlo run of the same register returns a **P80 of USD 310,000**. The organisation's appetite requires contingency at the P80. The contingency to set is:
- A. USD 310,000: the P80, funding an 80 %-confidence outcome rather than the average.
- B. USD 220,000: the EMV sum is the analysed figure.
- C. USD 90,000: the difference between the two.
- D. USD 530,000: the EMV sum plus the P80.

**Answer: A.** *(Rationale at MCQ 12.3-E, KA 12.3.)*

**PCL-MCQ-12-17** `[12.3.3 · Application]` Contingency was set at a Monte Carlo **P80 of USD 400,000**. Materialised risks have drawn **USD 120,000** and then **USD 90,000**; a re-run of the register puts the remaining exposure at a **P80 of USD 240,000**. The correct governance action is to:
- A. Report adequate cover: the original USD 400,000 exceeds the USD 240,000 exposure.
- B. Report remaining contingency of USD 280,000 and comfortable headroom.
- C. Report remaining contingency of USD 190,000 against USD 240,000 of remaining exposure: a USD 50,000
  shortfall, and escalate towards the management reserve as a re-baselining event.
- D. Say nothing: drawing down contingency is normal and needs no report.

**Answer: C.** *(Rationale at MCQ 12.3-F, KA 12.3.)*

### Domain 13 — AI for Project Controls & Project Management

**PCL-MCQ-13-01** `[13.1.1 · Recall]` Which relationship is correct?
- A. GenAI ⊂ ML ⊂ AI
- B. AI ⊂ ML ⊂ GenAI
- C. ML ⊂ GenAI ⊂ AI
- D. They are unrelated fields.

**Answer: A.** *(Rationale at MCQ 13.1-A, KA 13.1.)*

**PCL-MCQ-13-02** `[13.1.6 · Analysis]` Flagging invoices whose PO price and invoice price differ is best done with:
- A. Generative AI.
- B. Reinforcement learning.
- C. Rules/automation (deterministic logic).
- D. A large language model.

**Answer: C.** *(Rationale at MCQ 13.1-B, KA 13.1.)*

**PCL-MCQ-13-03** `[13.1.4 · Recall]` Retrieval-augmented generation (RAG) primarily:
- A. Retrains the model on your data.
- B. Supplies relevant documents to the model at inference so answers are grounded in your content.
- C. Removes the need for verification.
- D. Increases temperature.

**Answer: B.** *(Rationale at MCQ 13.1-C, KA 13.1.)*

**PCL-MCQ-13-04** `[13.1.3 · Recall]` For a factual controls task such as extracting figures from a document, the temperature setting should be:
- A. High, to maximise creativity.
- B. Set equal to the context-window size.
- C. Irrelevant: temperature only affects cost.
- D. Low, to reduce randomness in the output.

**Answer: D.** *(Rationale at MCQ 13.1-D, KA 13.1.)*

**PCL-MCQ-13-05** `[13.1.5 · Analysis]` An LLM returns a fluent, confident multi-step cost calculation. The professional must still recompute it because:
- A. An LLM generates plausible text, not verified text: plausible ≠ correct, especially in multi-step
  calculation.
- B. LLMs always round figures incorrectly.
- C. Recomputation is only needed when temperature is high.
- D. The context window truncates all calculations.

**Answer: A.** *(Rationale at MCQ 13.1-E, KA 13.1.)*

**PCL-MCQ-13-06** `[13.1.2 · Application]` A controls team wants to group thousands of anomalous cost postings into families of similar cases, with no predefined categories or labelled examples. The best-fit approach is:
- A. Rules/automation.
- B. Supervised ML.
- C. Unsupervised ML: finding structure in unlabelled data.
- D. Reinforcement learning.

**Answer: C.** *(Rationale at MCQ 13.1-F, KA 13.1.)*

**PCL-MCQ-13-07** `[13.1.4 · Application]` A commercial team wants an assistant that answers questions from a contract set that changes weekly, with each answer citing its source clause. Between fine-tuning and RAG, the better fit is:
- A. Fine-tuning, because it permanently teaches the model the contracts.
- B. RAG: the current documents are retrieved and supplied at inference, so answers are grounded in this
  week's contract set and cited to source.
- C. Fine-tuning, because it removes hallucination.
- D. Neither: LLMs cannot work over documents.

**Answer: B.** *(Rationale at MCQ 13.1-G, KA 13.1.)*

**PCL-MCQ-13-08** `[13.1.3 · Recall]` The context window of an LLM is:
- A. The amount of text (in tokens) the model can consider at once: everything it "knows" for a task must fit
  in it or be retrieved into it.
- B. The setting that controls randomness in the output.
- C. The period after which a model's training data goes stale.
- D. The screen area of the assistant's interface.

**Answer: A.** *(Rationale at MCQ 13.1-H, KA 13.1.)*

**PCL-MCQ-13-09** `[13.2.1 · Analysis]` An ML cost-forecast model is trained on historically mis-coded project cost. The most likely outcome is:
- A. The model corrects the mis-coding.
- B. Better accuracy.
- C. No effect: models are robust to bad data.
- D. The model learns and reproduces the mis-coding, giving misleading forecasts.

**Answer: D.** *(Rationale at MCQ 13.2-A, KA 13.2.)*

**PCL-MCQ-13-10** `[13.2.5 · Recall]` Before using an external AI tool on project data, the professional must ensure the data is:
- A. As large as possible.
- B. Fit (quality) and safe (confidentiality: no sensitive data in ungoverned tools).
- C. Unstructured.
- D. Public.

**Answer: B.** *(Rationale at MCQ 13.2-B, KA 13.2.)*

**PCL-MCQ-13-11** `[13.2.2 · Application]` A 20,000-row cost dataset is profiled before an AI initiative: **4 %** of rows have invalid codes, **2 %** are duplicates and **5 %** are missing accrual flags. Assuming no overlap, the number of rows failing at least one check is:
- A. 800
- B. 1,000
- C. 2,200
- D. 4,000

**Answer: C.** *(Rationale at MCQ 13.2-C, KA 13.2.)*

**PCL-MCQ-13-12** `[13.2.3 · Recall]` Contracts, correspondence and free-form reports, unstructured data, are
primarily the domain of:
- A. GenAI / RAG.
- B. Supervised ML over tabular features.
- C. Rules-based validation only.
- D. No AI category.

**Answer: A.** *(Rationale at MCQ 13.2-D, KA 13.2.)*

**PCL-MCQ-13-13** `[13.2.2 · Application]` A 15,000-row cost dataset is profiled before an AI initiative: **2 %** of rows have invalid codes, **3 %** are duplicates and **4 %** are missing accrual flags. Assuming no overlap, the number of rows failing at least one check is:
- A. 300
- B. 600
- C. 900
- D. 1,350

**Answer: D.** *(Rationale at MCQ 13.2-E, KA 13.2.)*

**PCL-MCQ-13-14** `[13.2.2 · Application]` Profiling finds that (i) the controls system and the ledger disagree on several cost totals, and (ii) a number of postings appear twice. The data-quality dimensions failing are:
- A. Accuracy and timeliness.
- B. Validity and completeness.
- C. Consistency and uniqueness.
- D. Timeliness and validity.

**Answer: C.** *(Rationale at MCQ 13.2-F, KA 13.2.)*

**PCL-MCQ-13-15** `[13.2.3 · Analysis]` An auditor challenges a figure in an AI-assisted forecast. The discipline that lets the team trace that number back through its transformations to its source is:
- A. Temperature control.
- B. Data lineage.
- C. Fine-tuning.
- D. Prompt patterns.

**Answer: B.** *(Rationale at MCQ 13.2-G, KA 13.2.)*

**PCL-MCQ-13-16** `[13.3.3 · Analysis]` The single non-negotiable step after a GenAI model drafts a variance narrative is to:
- A. Publish it immediately to save time.
- B. Increase the temperature.
- C. Verify the figures and causal claims against source before use.
- D. Delete the source data.

**Answer: C.** *(Rationale at MCQ 13.3-A, KA 13.3.)*

**PCL-MCQ-13-17** `[13.3.1 · Recall]` Which most improves a professional GenAI prompt?
- A. Making it as short and vague as possible.
- B. Supplying role/context, a clear task, the data, the desired format and constraints.
- C. Omitting the audience.
- D. Requesting maximum creativity for factual tasks.

**Answer: B.** *(Rationale at MCQ 13.3-B, KA 13.3.)*

**PCL-MCQ-13-18** `[13.3.2 · Application]` "Convert this raw cost extract into the standard monthly report format" is an instance of which prompt pattern?
- A. Extraction.
- B. Summarisation.
- C. Transformation.
- D. Analysis.

**Answer: C.** *(Rationale at MCQ 13.3-C, KA 13.3.)*

**PCL-MCQ-13-19** `[13.3.4 · Analysis]` To meet a deadline, an analyst pastes a confidential subcontract into a public AI tool to extract its terms. The primary guardrail breached is:
- A. Iterative refinement.
- B. Desired-format specification.
- C. Temperature control.
- D. Confidentiality: sensitive data must never enter ungoverned tools.

**Answer: D.** *(Rationale at MCQ 13.3-D, KA 13.3.)*

**PCL-MCQ-13-20** `[13.3.1 · Application]` A commercial manager must extract retention % and the LD rate from a subcontract for the contract register. Which prompt best follows the domain's prompt discipline?
- A. "Acting as a commercial manager, extract from the attached subcontract the retention % and LD rate as a
  two-row table with the clause reference for each; if a term is absent, return 'not found': do not infer."
- B. "Tell me everything important about this contract."
- C. "Extract the retention % and LD rate; if either is missing, estimate a typical market value."
- D. "Be as creative as possible and summarise the contract's vibe."

**Answer: A.** *(Rationale at MCQ 13.3-E, KA 13.3.)*

**PCL-MCQ-13-21** `[13.3.2b · Application]` In the red-team "attack my EAC" pattern, the model's output must **not** contain:
- A. The strongest reason each assumption may not hold.
- B. The evidence that would falsify each assumption.
- C. A list of data it needed but was not given.
- D. A proposed new EAC figure.

**Answer: D.** *(Rationale at MCQ 13.3-F, KA 13.3.)*

**PCL-MCQ-13-22** `[13.3.3 · Application]` A minutes-to-actions extraction returns an action table in which one action carries an owner's name that appears nowhere in the minutes. The correct handling is to:
- A. Keep the name: the model probably knows the team.
- B. Circulate the list immediately to save time.
- C. Replace it with 'not stated' and resolve the owner with the meeting chair before circulating.
- D. Delete the action from the list.

**Answer: C.** *(Rationale at MCQ 13.3-G, KA 13.3.)*

**PCL-MCQ-13-23** `[13.4.4 · Analysis]` To answer "what retention and LD terms do our current contracts contain?" the best-fitting category is:
- A. A general LLM with no documents.
- B. A meeting assistant.
- C. Document / RAG grounded in the contract set.
- D. RPA.

**Answer: C.** *(Rationale at MCQ 13.4-A, KA 13.4.)*

**PCL-MCQ-13-24** `[13.4.3 · Recall]` A stated reason to note that "features change" when naming AI tools is:
- A. Tools never improve.
- B. To avoid using AI.
- C. All tools are identical.
- D. Capabilities evolve rapidly, so a professional validates current features rather than assuming claims.

**Answer: D.** *(Rationale at MCQ 13.4-B, KA 13.4.)*

**PCL-MCQ-13-25** `[13.4.4 · Analysis]` Asking a general LLM assistant to perform precise multi-step arithmetic over a large cost table, rather than using spreadsheet/data-analysis AI, is best described as:
- A. Good practice: one tool for everything.
- B. Over-reaching: a category-to-task mismatch that invites plausible but wrong computation.
- C. A governance requirement.
- D. RAG grounding.

**Answer: B.** *(Rationale at MCQ 13.4-C, KA 13.4.)*

**PCL-MCQ-13-26** `[13.4.2b · Recall]` The category-specific governance risk of document/RAG tools is that:
- A. The retrieval layer may not respect document permissions, and a stale corpus produces confidently outdated answers.
- B. They cannot cite sources.
- C. They work only on tabular data.
- D. They eliminate hallucination entirely.

**Answer: A.** *(Rationale at MCQ 13.4-D, KA 13.4.)*

**PCL-MCQ-13-27** `[13.4.4 · Application]` A portfolio office wants to predict, from its governed historical data, which of its live projects are most likely to overrun. The best-fitting tool category is:
- A. A general LLM assistant.
- B. A risk & forecasting / ML platform.
- C. A transcription / meeting assistant.
- D. Document / RAG.

**Answer: B.** *(Rationale at MCQ 13.4-E, KA 13.4.)*

**PCL-MCQ-13-28** `[13.4.2b · Application]` A director's natural-language query to the BI assistant returns a "% complete" figure that differs from the controlled monthly report. The category-specific failure most likely at work is:
- A. Metric-definition drift: the query was answered from a subtly different definition than the report's.
- B. A missing goods-receipt note.
- C. Too low a temperature setting.
- D. An expired tool licence.

**Answer: A.** *(Rationale at MCQ 13.4-F, KA 13.4.)*

**PCL-MCQ-13-29** `[13.A.6 · Application]` A controls team assembles a **monthly** cost pack for the board. Of the three integration patterns, the proportionate choice is:
- A. Manual export/import: spreadsheets and email are simplest.
- B. API integration: the freshest data is always best.
- C. No integration: retype the figures each month.
- D. Batch ETL/file transfer: a scheduled, auditable extract matches the monthly decision cadence.

**Answer: D.** *(Rationale at MCQ 13.4-G, KA 13.4.)*

**PCL-MCQ-13-30** `[13.5.1 · Recall]` The universal shape of an AI-in-controls workflow is:
- A. Input → AI step → professional verification/decision → owned output.
- B. AI decides → professional observes.
- C. Professional drafts → AI approves.
- D. AI both drafts and signs off.

**Answer: A.** *(Rationale at MCQ 13.5-A, KA 13.5.)*

**PCL-MCQ-13-31** `[13.5.3 · Analysis]` An AI model outputs an `EAC`. Before reporting it, the professional should **not**:
- A. Recompute it from `AC`/`EV`/`BAC` and the indices.
- B. Confirm the method's assumption matches the variance cause.
- C. Report it unchecked because the model is advanced.
- D. Run the `TCPI` reality check.

**Answer: C.** *(Rationale at MCQ 13.5-B, KA 13.5.)*

**PCL-MCQ-13-32** `[13.5.4 · Analysis]` An AI accrual tool accrues from the invoice date rather than the service date. This risks:
- A. Nothing.
- B. Violating IFRS 15 only.
- C. Improving cut-off accuracy.
- D. Reproducing a cut-off error at scale (Domain 1, KA 1.3.5).

**Answer: D.** *(Rationale at MCQ 13.5-C, KA 13.5.)*

**PCL-MCQ-13-33** `[13.5.7 · Analysis]` In an AI claims-exposure sweep across 60 subcontracts, the model's distinctive contribution is:
- A. Deciding entitlement on each claim.
- B. Coverage: it reads everything, surfacing candidates for the professional's judgement.
- C. Replacing legal review of material exposures.
- D. Setting the portfolio contingency.

**Answer: B.** *(Rationale at MCQ 13.5-D, KA 13.5.)*

**PCL-MCQ-13-34** `[13.5.3 · Application]` A control account has `BAC` USD 1,500,000 and a sustained `CPI` of 0.96. If the trend holds, the model's projected `EAC = BAC/CPI` is:
- A. USD 1,440,000
- B. USD 1,500,000
- C. USD 1,562,500
- D. USD 1,687,500

**Answer: C.** *(Rationale at MCQ 13.5-E, KA 13.5.)*

**PCL-MCQ-13-35** `[13.5.4 · Application]` An ML classifier auto-codes **900** of a month's **1,200** invoices above its confidence threshold; an audit of the 900 finds **855** correct. The model's precision at the threshold is:
- A. 71.25 %
- B. 75 %
- C. 95 %
- D. 5 %

**Answer: C.** *(Rationale at MCQ 13.5-F, KA 13.5.)*

**PCL-MCQ-13-36** `[13.A.7 · Application]` A duplicate-invoice detector is scored on a golden set of **300** invoices containing **60** known duplicates. It flags **50** invoices, of which **36** are genuine duplicates. Its precision and recall are:
- A. Precision 60 %, recall 72 %.
- B. Precision 72 %, recall 60 %.
- C. Precision 83.3 %, recall 83.3 %.
- D. Precision 12 %, recall 16.7 %.

**Answer: B.** *(Rationale at MCQ 13.5-G, KA 13.5.)*

**PCL-MCQ-13-37** `[13.5.5 · Application]` An AI logic-check on a contractor schedule flags dangling activities and a hard constraint. After the planner re-logics the dangles and removes the constraint, the recalculated finish slips 6 days. The best reading is:
- A. The constraint had been hiding a genuine slip: the repaired schedule is the honest one to take forward.
- B. The repair introduced the slip, so the constrained version should be restored to protect the date.
- C. The AI fabricated the defects, since the original schedule showed the earlier date.
- D. Dangling activities are cosmetic and the exercise was unnecessary.

**Answer: A.** *(Rationale at MCQ 13.5-H, KA 13.5.)*

**PCL-MCQ-13-38** `[13.6.2 · Analysis]` When an AI-assisted forecast is later challenged, an acceptable defence is:
- A. "It was the model's output."
- B. Deleting the audit trail.
- C. "The model is very advanced."
- D. The documented verification and named sign-off showing how it was checked and owned.

**Answer: D.** *(Rationale at MCQ 13.6-A, KA 13.6.)*

**PCL-MCQ-13-39** `[13.6.4 · Analysis]` Which is a legitimate reason **not** to use AI for a task?
- A. The logic is deterministic and a transparent rule handles it better.
- B. AI would save time.
- C. Colleagues use AI.
- D. The output looks impressive.

**Answer: A.** *(Rationale at MCQ 13.6-B, KA 13.6.)*

**PCL-MCQ-13-40** `[13.6.3 · Recall]` The mitigation for hallucination in a controls context is to:
- A. Verify every figure/citation against source, use RAG grounding and low temperature.
- B. Trust the model more.
- C. Increase temperature.
- D. Paste more confidential data.

**Answer: A.** *(Rationale at MCQ 13.6-C, KA 13.6.)*

**PCL-MCQ-13-41** `[13.6.5 · Application]` An AI-drafted forecast passes every line of the assurance checklist
except "cross-checked". It is inconsistent with the schedule's critical path. The correct action is to:
- A. Release it with a footnote noting the inconsistency.
- B. Withhold it until the failure is fixed.
- C. Release it because most lines passed.
- D. Remove the cross-check line from the checklist.

**Answer: B.** *(Rationale at MCQ 13.6-D, KA 13.6.)*

**PCL-MCQ-13-42** `[13.6.3 · Recall]` Bias arises in AI systems primarily because:
- A. Models are deliberately unfair.
- B. Temperature is set too low.
- C. Models reproduce the biases present in their training data.
- D. Verification introduces skew.

**Answer: C.** *(Rationale at MCQ 13.6-E, KA 13.6.)*

**PCL-MCQ-13-43** `[13.6.5 · Application]` An AI extraction reports an LD rate citing clause 14.3. The reviewer opens clause 14.3: the clause exists, but states a different rate. Applying the assurance checklist, the correct conclusion is:
- A. The output passes: the citation is real, so the grounding line is satisfied.
- B. Release it with a footnote recording the difference.
- C. Skip the checklist: legal review will catch it later.
- D. The source-check line fails: the value does not match the cited clause, so the output is withheld until
  fixed.

**Answer: D.** *(Rationale at MCQ 13.6-F, KA 13.6.)*

**PCL-MCQ-13-44** `[13.A.1 · Application]` A controls function deploys an agentic system that retrieves the month-end extract, computes the variances, drafts commentary and assembles the exception pack. The verification discipline should:
- A. Move from per-output to per-workflow: assure the chain's design and insert checkpoints where
  consequential intermediate outputs are inspected before the chain proceeds.
- B. Apply only to the final pack, since that is all anyone reads.
- C. Be dropped: an agent that checks its own work needs no reviewer.
- D. Be replaced by an annual audit of the vendor.

**Answer: A.** *(Rationale at MCQ 13.6-G, KA 13.6.)*

**PCL-MCQ-13-45** `[13.6.4 · Application]` A one-off, high-stakes external disclosure would take longer to verify line-by-line than to draft manually, and the drafting data is highly confidential. Under the "when not to use AI" tests, the professional should:
- A. Use AI anyway: it is the modern approach.
- B. Not use AI for this task: the verification burden negates the time saving and the stakes demand certainty
  the model cannot give.
- C. Use AI and skip verification to preserve the saving.
- D. Use a public tool, since the task is a one-off.

**Answer: B.** *(Rationale at MCQ 13.6-H, KA 13.6.)*

**PCL-MCQ-13-46** `[13.7.1 · Analysis]` Jumping straight to "AI integrated in the workflow" without governance primarily:
- A. Invites the risks of ungoverned AI (hallucination, confidentiality, no audit trail).
- B. Saves the most time safely.
- C. Is required by the maturity model.
- D. Has no downside.

**Answer: A.** *(Rationale at MCQ 13.7-A, KA 13.7.)*

**PCL-MCQ-13-47** `[13.7.5 · Analysis]` As AI capability advances, the need for verification and governance:
- A. Disappears.
- B. Increases: a more capable model that is wrong is more convincingly wrong.
- C. Stays irrelevant.
- D. Is replaced by the model.

**Answer: B.** *(Rationale at MCQ 13.7-B, KA 13.7.)*

**PCL-MCQ-13-48** `[13.7.3 · Application]` AI-assisted reconciliation cuts a three-person month-end close from **4 days to 2** (8-hour days, loaded cost **USD 100/hour**). Annual tooling and governance cost is **USD 30,000**. The honest **net** annual value is:
- A. USD 4,800
- B. USD 27,600
- C. USD 57,600
- D. USD 87,600

**Answer: B.** *(Rationale at MCQ 13.7-C, KA 13.7.)*

**PCL-MCQ-13-49** `[13.7.2 · Recall]` As AI is integrated into the controls workflow, the professional's role shifts toward:
- A. Being replaced by the model.
- B. Needing less domain knowledge.
- C. Producing every number manually to be safe.
- D. Directing and assuring AI-assisted production: a higher-judgement role.

**Answer: D.** *(Rationale at MCQ 13.7-D, KA 13.7.)*

**PCL-MCQ-13-50** `[13.A.5 · Application]` Reviewing one auto-coded line costs **USD 2**; an uncaught miscode costs **USD 400** downstream. Measured precision is **99.0 %**. Pricing the review step:
- A. Per-item review still pays: expected uncaught-error cost is `1 % × 400 = USD 4` per line, above the USD 2
  review cost; break-even sits at a precision of 99.5 %.
- B. Per-item review no longer pays: a 1 % error rate is negligible.
- C. Per-item review no longer pays: 99.0 % precision exceeds the USD 2 review cost.
- D. Per-item review always pays, whatever the precision.

**Answer: A.** *(Rationale at MCQ 13.7-E, KA 13.7.)*

**PCL-MCQ-13-51** `[13.7.3 · Application]` An extraction workflow runs **20,000 documents a year**, averaging **2,000 tokens in** and **500 tokens out**, priced at **USD 3.00 per million input tokens** and **USD 15.00 per million output tokens**. The annual compute cost is approximately:
- A. USD 120
- B. USD 150
- C. USD 2,700
- D. USD 270

**Answer: D.** *(Rationale at MCQ 13.7-F, KA 13.7.)*

**PCL-MCQ-13-52** `[13.A.4 · Analysis]` A function whose AI now drafts most narratives and codes most cost lines still requires analysts to work problems by hand on a regular rotation, with AI switched off. The primary purpose is to:
- A. Punish over-reliance on the tools.
- B. Reduce licence costs during the rotation.
- C. Maintain the first-principles judgement that verification of AI output depends on.
- D. Comply with a data-residency requirement.

**Answer: C.** *(Rationale at MCQ 13.7-G, KA 13.7.)*

---

## Appendix G — The integrated capstone: one project, thirteen domains

The domains teach the machinery one discipline at a time; a real month-end exercises all of it at once. This
capstone follows a single project: the **master project** of Domain 6, with its baseline first phased in
Domain 3 (KA 3.3.3): `BAC` **USD 1,000,000** over **ten months**, through one full reporting cycle at the end
of **Month 5**, extended here with the commercial, revenue, risk and AI facts the other domains supply. Every
figure either comes from a domain worked example or is derived by that domain's method, and every step is
cross-referenced by topic number; nothing is introduced that the domains have not already taught. Work the
eight stations in order. Each is what the professional actually does, in the order a close actually runs.

### Station 1 — Budget and baseline (Domain 3)

**What the professional does.** Assemble the budget bottom-up and make it a control. The control-account
budgets are summed; the **contingency reserve** (funding the identified risks of Station 6) sits **inside**
the baseline; the **management reserve** sits **outside** it, management-controlled (3.1.4; 12.3.2). The `BAC`
is then phased across the ten-month schedule to the S-curve of 3.3.3, which *is* the Planned Value.

```
BAC           = Σ control-account budgets + contingency = 915,000 + 85,000 = 1,000,000     (3.1.4)
Total budget  = BAC + management reserve = 1,000,000 + 50,000 = 1,050,000                  (3.1.4)
PV at Month 5 = 40 + 70 + 110 + 140 + 160 = 520 (USD 000) → PV = 520,000                   (3.3.3)
```

The Month-5 `PV` of USD 520,000 is read straight off the 3.3.3 phasing table: the same S-curve on which Domain
6 later draws `EV` and `AC` (6.1.3), and whose cash consequences Domain 3 forecasts separately (3.5).

### Station 2 — The cost states (Domain 5)

**What the professional does.** Before computing any index, make cost-to-date *true*. At Month 5 the cost
ledger shows purchase orders and subcontracts raised (commitments) of **USD 640,000**; invoices processed
(actuals) of **USD 410,000**; and goods/services received but not yet invoiced (accruals) of **USD 120,000**.
*Assumption (stated per the worked-example format, Conventions §5): all accrued work is under the raised POs, so the accrual reduces
the open commitment.*

```
AC (cost-to-date)  = actuals + accruals = 410,000 + 120,000 = 530,000            (5.2.1–5.2.2)
Open commitment    = commitments − actuals − accruals
                   = 640,000 − 410,000 − 120,000 = 110,000                       (5.2.2)
Forecast committed = 530,000 + 110,000 + estimate for uncommitted scope          (5.2.1)
```

This is the `AC` = 530,000 that Domain 6 uses (6.1.1). The accrual discipline is what makes it true. An
invoice-only view would report 410,000 and flatter the cost index to `480,000 / 410,000 = 1.17`; the
disciplined figure tells the real story at Station 3.

### Station 3 — Earned value and the forecast (Domain 6)

**What the professional does.** Measure `EV` from physical progress under the fixed earning rules (6.1.2) (at
Month 5, `EV` = **480,000**) then compute, exactly as Domain 6 publishes them:

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
credible (6.2.3): the number Stations 5 and 7 must now carry, not hide.

### Station 4 — The commercial position (Domain 7)

**What the professional does.** Run the payment cycle. The client contract is **remeasured** (7.1.2) at a
contract price of **USD 1,250,000**, with **5 % retention** and a **10 % mobilisation advance** recovered
pro-rata on each certificate (7.4.3). At Month 5 the client's QS certifies gross work done of **USD 460,000**
(7.4.2: the certified measure, not the application, drives cash).

```
Retention        = 5 % × 460,000 = 23,000                                        (7.4.3)
Advance recovery = 10 % × 460,000 = 46,000                                       (7.4.3)
Net certified    = 460,000 − 23,000 − 46,000 = 391,000                           (7.4.3)
EV − certified gross = 480,000 − 460,000 = 20,000                                (7.4.4)
```

The USD 20,000 by which `EV` runs ahead of certification is work performed but not yet certified: unbilled
performance pointing to a **contract asset** (7.5.2), measured at accounting values in Station 5. `EV` (at
budget) and the valuation (at contract rates) are different measures of the same progress, so the gap is read
through the three-way reconciliation of 7.4.4, never forced to zero.

### Station 5 — Revenue recognised (Domain 2)

**What the professional does.** Hand finance a defensible input-method ratio. Revenue is recognised over time
by **cost-to-cost** (2.2.6), and the denominator is the controls forecast, the Station 3 `EAC`, so the
professional's forecast flows straight into reported revenue.

```
PoC            = costs to date / total estimated cost = 530,000 / 1,104,167 = 48.0 %   (2.2.6)
Revenue        = 48.0 % × 1,250,000 ≈ 600,000  (0.4800 × 1,250,000 = 600,000)          (2.2.6)
Contract asset = 600,000 − 460,000 = 140,000   (under-billed)                          (2.2.7, 7.5.2)
Margin to date = 600,000 − 530,000 = 70,000 (11.7 %)
Margin at completion = 1,250,000 − 1,104,167 = 145,833 (11.7 %)
```

The margin check ties: cost-to-cost makes the to-date margin equal the forecast completion margin, if they
diverge, the ratio and the `EAC` have come apart. The contract remains profitable, so no onerous-contract
provision arises (2.2.6 loss rule; 1.4.5); but the USD 140,000 contract asset, performance ahead of billing,
is aged and explained, not just reported (7.5.2).

### Station 6 — Risk and the contingency test (Domain 12)

**What the professional does.** Re-run the register, not the opening story. Contingency was set at sanction at
the Monte Carlo **P80 of USD 85,000** (12.3.1) (the Station 1 figure inside the `BAC`) and materialised risks
have drawn **USD 25,000** through change control (5.4). The Month-5 re-run prices the remaining register at an
**EMV of USD 45,000** (12.2.3) and a **P80 of USD 70,000**: the P80 sits above the EMV because the register's
risks share drivers, and correlation moves the tail, not the mean (12.3.1; Exercise 12.5).

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
implication (8.4; Advanced 8.A.3) follows: the next gate cannot proceed on the `BAC`; the pack presents the
`EAC` with its method and assumption, the `TCPI` reality-check on any recovery claim, and the choice the board
actually owns: fund a specific recovery, or re-baseline through the management reserve.

### Station 8 — The AI-assisted close (Domain 13)

**What the professional does.** Run the same cycle faster, and sign it. Each station above used a governed AI
step, and at each one a named professional disposed of what the AI proposed (13.6.1):

| Station | Governed AI step | The professional signed |
|---|---|---|
| 1–2 | Auto-coding of cost, proposed month-end accruals, duplicate/anomaly flags (13.5.4) | The accrual judgements and every flagged exception |
| 3 | `EAC` driver analysis and the `CPI`-trend early warning (13.5.3) | The method choice (b) and the defended forecast |
| 4 | Extraction of certificate, retention and advance terms from the contract documents (13.5.7) | The certified measure and the valuation cascade |
| 5 | Drafted revenue workings and consistency checks tying cost, billing and revenue (13.5.10) | The PoC ratio, its `EAC` denominator and the recognition judgement |
| 6 | Register scoring and the Monte Carlo re-run (13.5.9) | The adequacy verdict and the escalation |
| 7 | Drafted exception narratives and dashboard assembly (13.5.8) | Accuracy, framing and the final sign-off |

Every step ran inside the guardrails: verified against source before use (13.6.5's checklist), with the audit
trail of what the AI produced, who approved it and what changed (13.6.2). **AI proposes; the professional
verifies, decides and remains accountable**, at every station, without exception.

### The PCI control cycle

Read back-to-back, the eight stations show why the thirteen domains are one body of knowledge rather than a
shelf of techniques. A single month of one project generated every number above, and each domain valued the
*same physical progress* under its own rules: the budget phased it (Domain 3), the ledger costed it with
commitments and accruals (Domains 1, 5, 11), earned value measured it at budget (Domain 6) against a schedule
that sequenced it (Domains 8, 10), the valuation billed it at contract rates (Domain 7), IFRS 15 recognised it
as revenue (Domain 2), the register funded its uncertainty (Domain 12), the report turned it into a decision
(Domain 4), and AI accelerated every step under governance (Domain 13). The figures differ by design (`EV`
480,000, certified 460,000, revenue 600,000) and the professional's craft is the reconciliation between them
(7.4.4; 7.5.3's commercial-to-accounting loop), not the pretence that they should agree. Notice, too, how the
forecast is the hinge: the `EAC` chosen in Station 3 set the revenue ratio in Station 5, sized the gate
conversation in Station 7, and framed the contingency question in Station 6. One defensible forecast,
consistently applied, is what makes the whole cycle honest; a flattered one corrupts every station downstream.
That interlock (cost, schedule, commercial, accounting, risk, reporting and AI governance meeting in one data
date) is precisely the integrated judgement the credential certifies.

### Reflection questions

1. The invoice-only ledger showed `AC` = 410,000 (a `CPI` of 1.17); the accrued figure showed 530,000 (0.91).
   Trace every later station that would have been corrupted by the flattering figure (forecast, revenue,
   margin, gate) and name the domain discipline that prevented it. *(5.2; then 6.3, 2.2.6, 4.3.5, 8.4.)*
2. `EV` (480,000), certified billing (460,000) and recognised revenue (600,000) all measure Month 5's
   progress. State what each values, why none is "wrong", and which balance-sheet line carries the gap between
   the last two. *(7.4.4; 2.2.7 / 7.5.2: a USD 140,000 contract asset.)*
3. Remaining contingency (60,000) fails the test against remaining exposure (P80 70,000) even though it
   comfortably exceeds the register's EMV (45,000). Explain, using Exercise 12.5's logic, why the P80 — not
   the EMV — is the right comparator, and what governance route the shortfall takes. *(12.3.1, 12.3.3, 12.3.2.)*

---

*The formula sheet, glossary, standards index, figure index, self-check key and MCQ bank above cover all
thirteen domains, and every entry is indexed to the Knowledge Area that teaches it.*
