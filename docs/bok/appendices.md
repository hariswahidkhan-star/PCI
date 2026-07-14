# Appendices — PCP-AI Body of Knowledge

> Back-matter assembled from the finished domains, per the Style Spine (§4, §9) and the consolidation plan.
> As further domains are revised through SME review, these indexes are regenerated from the source chapters.
> Contents: A) master formula sheet · B) global glossary · C) standards & frameworks referenced · D) figure &
> animation index · E) self-check answers · F) sample-MCQ bank.

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
| `EAC = AC + ETC` | Estimate at completion (identity) | 6.3.1 |
| `EAC = AC + (BAC − EV)` | EAC — remaining work at budgeted rate | 6.3.2 |
| `EAC = BAC / CPI` | EAC — remaining work at current CPI | 6.3.2 |
| `EAC = AC + (BAC − EV) / (CPI × SPI)` | EAC — cost & schedule compound | 6.3.2 |
| `VAC = BAC − EAC` | Variance at completion | 6.3.4 |
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

---

## Appendix B — Global glossary

Consolidated from every domain's key-terms box (representative; the full glossary grows with SME review).

| Term | Definition | First defined |
|---|---|---|
| **Accrual basis** | Recognising economic events when they occur, not when cash moves | 1.3 |
| **Accounting equation** | `A = L + E`; holds after every transaction | 1.1 |
| **AgileEVM** | Earned value applied to variable-scope adaptive delivery | 9.5 |
| **Basis of estimate (BoE)** | The auditable record of an estimate's scope, assumptions, rates, class | 3.2 |
| **Budget at completion (BAC)** | Total value of the cost baseline | 3.1 |
| **Contingency reserve** | Budget for identified risks; inside the baseline; PM-controlled | 3.1 / 12.3 |
| **Contract asset / liability** | Revenue recognised vs amounts billed (under-/over-billing) | 2.2 / 7.5 |
| **Control account (CA)** | The WBS×OBS integration point where scope, budget, cost, schedule meet | 1.5 / 5.3 |
| **Cost baseline / PMB** | Approved, time-phased budget; source of Planned Value | 3.1 |
| **Critical path** | The longest, zero-float chain; sets the project duration | 10.2 |
| **Earned value (EV)** | Budgeted cost of work performed | 6.1 |
| **Empirical process control** | Decisions from observation — transparency, inspection, adaptation | 9.1 |
| **Estimate at completion (EAC)** | Total forecast cost; `EAC = AC + ETC` | 6.3 |
| **Hallucination** | AI confidently producing false content | 13.1 |
| **Incremental / iterative** | Adding usable parts / refining the same product over passes | 8.6 |
| **Liquidated damages (LDs)** | Pre-agreed sum for a defined breach (usually late completion) | 7.2 |
| **Management reserve** | Budget for unidentified risk; outside the baseline; management-controlled | 3.1 / 12.3 |
| **Matching principle** | Recognise expenses in the period of the income they help earn | 1.3 |
| **Onerous contract** | Unavoidable costs exceed benefits; the loss is provided immediately | 1.4 / 2.2 |
| **Performance obligation** | An IFRS 15 promise to transfer a distinct good/service | 2.2 |
| **Planned value (PV)** | Budgeted cost of work scheduled — the cost-baseline S-curve | 3.3 / 6.1 |
| **Provision (IAS 37)** | Liability of uncertain timing/amount meeting the recognition tests | 1.4 |
| **RAG (retrieval-augmented generation)** | Grounding GenAI answers in retrieved source documents | 13.1 |
| **Retention** | Cash withheld as security, released in stages | 7.2 |
| **Segregation of duties (SoD)** | No single person controls a whole transaction | 11.3 |
| **Story point / velocity** | Relative size unit / average points completed per Sprint | 9.3 |
| **Three-way match** | Matching invoice to PO and goods-receipt note before payment | 11.2 |
| **Total / free float** | Slack without delaying the project / a successor | 10.2 |
| **Variance bridge** | A waterfall walking budget to actual by variance component | 4.2 |
| **Work breakdown structure (WBS)** | Hierarchical decomposition of scope into work packages | 1.5 / 8.2 |
| **"AI proposes, the professional disposes"** | The credential's AI governing principle | 13.6 |

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

Every Knowledge Area ends with **3–6 sample MCQs** (four options, correct answer marked, rationale, tagged with
topic number and cognitive level), per Style Spine §8. Consolidated counts (draft):

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
| 11 Process Cycles | 3 | 12 |
| 12 Risk Management | 3 | 13 |
| 13 AI for Project Controls | 7 | 31 |
| **Total (draft)** | **61** | **280** |

> **Blueprint separation.** These are **study/sample items** drawn from the same blueprint as, but kept
> **separate from**, the live examination bank; they are **not** to be reused verbatim as live exam questions
> (Style Spine §8). On consolidation they are tagged to topic numbers and cognitive levels to map to the exam
> blueprint, and reviewed by SMEs (finance, agile, AI) before any use.

---

*All appendices are draft, regenerated from the domains as they pass SME review. The formula sheet, glossary,
standards index and figure index above are authoritative for the currently-drafted domains (1–13).*
