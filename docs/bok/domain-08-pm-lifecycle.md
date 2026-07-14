# Domain 8 — Project Management Lifecycle

> **Group:** Project management (Domain 8 of 8 in the PM group). **Target:** ~120 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). British English; USD (+SAR where useful). Standards
> named (PMBOK, AACE TCM) at principle level, never reproduced.

## Why this domain exists

Project controls does not operate in a vacuum: it sits inside the wider discipline of **managing a project
through its life**. A controls professional must understand the whole lifecycle to know *where* their work
fits — a baseline is set in **planning**, measured in **monitoring & controlling**, and its lessons captured
in **closing**. This domain covers the project lifecycle through its process groups — **initiating**,
**planning**, **executing**, **monitoring & controlling**, and **closing** (KAs 8.1–8.5) — threaded by a
single mini-case; and the **development approaches** that shape how the lifecycle is applied, from predictive
through iterative, incremental and adaptive, including the crucial and often-confused distinction between
**incremental** and **iterative** delivery (KA 8.6, the bridge to Domain 9). The process groups are described
at principle level (consistent with widely used frameworks such as the PMBOK Guide and AACE's Total Cost
Management framework), never reproduced.

**Learning objectives.** After this domain a candidate can: describe the five process groups and the key
activities and artefacts of each; build a stakeholder analysis and a WBS; explain how monitoring & controlling
uses earned value and integrated change control; run a disciplined project closure; and distinguish
predictive, iterative, incremental, adaptive and hybrid approaches and select one for a project profile.

**The threaded mini-case.** One project — *the delivery of a regional data-centre fit-out for "Northwind"* —
runs through KAs 8.1–8.5, so the reader sees a single project pass through every process group.

---

## Knowledge Area 8.1 — Initiating

*Topics: 8.1.1 the business case and feasibility · 8.1.2 the project charter · 8.1.3 stakeholder
identification · 8.1.4 success criteria.*

### 8.1.1 The business case and feasibility

**Definition & purpose.** A project starts with a **business case** — the justification that the expected
benefits outweigh the costs and risks — tested by **feasibility** (is it technically, commercially and
operationally deliverable?). For a controls professional the business case is where the *first cost estimate*
and *funding envelope* originate (Domain 3), and the reference against which the project's value is later
judged. A project with no clear business case has no yardstick for its decisions.

### 8.1.2 The project charter

**Definition & purpose.** The **project charter** formally **authorises** the project and the project manager,
and captures at a high level the objectives, scope boundaries, key stakeholders, high-level budget and
schedule, and success criteria. It is the mandate everything downstream elaborates. Controls relevance: the
charter's high-level budget and milestones are the seed of the baseline that planning will detail.

### 8.1.3 Stakeholder identification

**The principle.** **Stakeholders** are anyone who affects, or is affected by, the project. Identifying and
analysing them early — commonly on a **power/interest grid** (manage closely / keep satisfied / keep informed
/ monitor) — shapes the communications and reporting plan (Domain 4, KA 4.3). A controls professional is a key
stakeholder-facing role: reporting is *for* stakeholders, so knowing who they are and what each needs is a
direct input to report design.

> **Fig 8.1.1 — Power/interest stakeholder grid.** *Caption:* how to engage each stakeholder group.
> *Underlying data:* Northwind's stakeholders — sponsor (high power, high interest), operations team (low
> power, high interest), regulator (high power, low interest), local community (low power, low interest).
> *Render-ready description:* a 2×2 grid, x-axis Interest (low→high), y-axis Power (low→high), four quadrants
> labelled "Monitor" / "Keep informed" / "Keep satisfied" / "Manage closely"; the four stakeholders plotted as
> brand-blue dots in their quadrants. *Animation storyboard (digital-only):* each stakeholder dot drops into
> its quadrant with its engagement label appearing.

**Worked example 8.1.3 — a RACI for a controls deliverable.** Knowing *who* the stakeholders are leads
directly to *who does what*. A **RACI** chart (Responsible / Accountable / Consulted / Informed) assigns each
activity exactly one owner. For producing Northwind's monthly cost report:

| Activity | Cost Engineer | Project Manager | Finance | Sponsor |
|---|---|---|---|---|
| Compile cost data | R | A | C | |
| Raise accruals | R | A | C | |
| Approve the report | | A | | I |
| Present at project board | | R | | A |

Reading the chart: every row has **exactly one A** — the single point of accountability. The cost engineer is
**Responsible** for compiling the data and raising the accruals (Domain 5); the project manager is
**Accountable** for the report through to its approval, with finance **Consulted** on the numbers; and at the
project board the accountability shifts — the PM *presents*, but the **sponsor** is Accountable for what the
board does with it. A RACI built during initiating makes reporting (Domain 4) and sign-off unambiguous before
the first report is ever due.

### 8.1.4 Success criteria

**The principle.** **Success criteria** define what "done well" means — and they are broader than the classic
**iron triangle** of scope, time and cost. A project can hit time and budget yet fail if it does not deliver
the **benefit** the business case promised, or if quality/safety/stakeholder outcomes are poor. Defining
success criteria up front (and distinguishing *project* success — delivered to plan — from *product/benefit*
success — the outcome realised) gives the controls function the right things to measure.

### Key terms — KA 8.1

| Term | Meaning |
|---|---|
| **Business case** | The benefits-vs-cost-and-risk justification for the project. |
| **Project charter** | The document authorising the project and the project manager. |
| **Stakeholder** | Anyone who affects or is affected by the project. |
| **Power/interest grid** | A tool to classify stakeholders and tailor engagement. |
| **Success criteria** | The definition of doing well — beyond scope/time/cost to benefit and quality. |

### Sample MCQs — KA 8.1

**MCQ 8.1-A `[8.1.2 · Recall]`** The document that formally authorises a project and the project manager is
the:
- A. Business case.
- B. Work breakdown structure.
- C. Project charter. ✅
- D. Cost baseline.

*Rationale:* The charter authorises the project and PM. The business case justifies it; the WBS and cost
baseline are planning artefacts.

**MCQ 8.1-B `[8.1.3 · Application]`** A high-power, low-interest regulator should be:
- A. Kept satisfied. ✅
- B. Managed closely.
- C. Merely monitored.
- D. Ignored.

*Rationale:* On the power/interest grid, high power + low interest = "keep satisfied". "Manage closely" is for
high power + high interest; "monitor" is low/low; ignoring a powerful stakeholder is never appropriate.

**MCQ 8.1-C `[8.1.4 · Analysis]`** A project is delivered on time and within budget, but the benefit promised
in the business case never materialises. The most accurate assessment is:
- A. Total success — the iron triangle was met.
- B. A failure of the charter to authorise the project.
- C. Proof that success criteria are irrelevant once delivery starts.
- D. Project success without benefit success — the two are distinct registers. ✅

*Rationale:* Success criteria distinguish *project* success (delivered to plan) from *product/benefit*
success (the outcome realised); hitting time and cost while missing the benefit is only the first. A stops at
the iron triangle; B misreads the charter's role; C inverts the lesson.

**MCQ 8.1-D `[8.1.3 · Application]`** A RACI chart for the monthly cost report shows two "A"s against the
activity "Approve the report". The correction required is:
- A. Add a third "A" so approval is shared.
- B. Reduce it to exactly one Accountable — a single point of accountability per activity. ✅
- C. Replace both with "R"s.
- D. Delete the activity from the chart.

*Rationale:* A RACI assigns each activity exactly one Accountable owner; duplicated "A"s dissolve
accountability. A worsens the defect; C leaves no one accountable; D removes a real deliverable instead of
fixing its ownership.

### Self-check — KA 8.1

1. Distinguish the business case from the project charter. *(Business case justifies; charter authorises.)*
2. Why are success criteria broader than the iron triangle? *(A project can hit time/cost/scope yet fail to
   deliver the benefit or meet quality/stakeholder outcomes.)*

---

## Knowledge Area 8.2 — Planning

*Topics: 8.2.1 scope and the WBS · 8.2.2 the integrated plan and the baselines · 8.2.3 the subsidiary plans.*

### 8.2.1 Scope and the WBS

**Definition & purpose.** Planning begins with **scope**: what will (and will not) be delivered, decomposed
into a **work breakdown structure (WBS)** — the hierarchical breakdown of the total scope into deliverables
and work packages (Domain 1, KA 1.5.3). The WBS is the backbone of everything: the schedule is built from it,
the cost is coded to it, control accounts are defined on it (Domain 5), and earned value is measured against
it (Domain 6). The **100 % rule** — the WBS captures 100 % of the scope, no more, no less — is what keeps the
plan complete and non-overlapping.

**Worked example 8.2.1 — a WBS extract for Northwind.**

```
1.  Data-centre fit-out
    1.1  Design
         1.1.1  Mechanical & electrical design
         1.1.2  Structural design
    1.2  Construction
         1.2.1  Power & cooling installation
         1.2.2  Racking & containment
    1.3  Commissioning
         1.3.1  Integrated systems test
         1.3.2  Handover
```

Each lowest-level item is a **work package** — schedulable, costable, and the unit `EV` is earned on. Cost
coded to `1.2.1` (Domain 1, KA 1.5.5) rolls up to `1.2`, to `1`, giving the scope view of cost.

### 8.2.2 The integrated plan and the baselines

**The principle.** Planning produces the three **baselines** that control depends on — **scope**, **schedule**
and **cost** — integrated so they are consistent (the schedule reflects the scope; the cost is phased over the
schedule to give `PV`, Domain 3). "Integrated" is the operative word: a schedule that does not reflect the WBS,
or a cost baseline not phased over the schedule, cannot support earned value. The controls professional is
typically the guardian of this integration.

**The resource-loaded schedule and the manpower histogram.** A schedule whose activities carry crew and hours
requirements is a **resource-loaded schedule**, and its summation per period is the **manpower histogram** —
the everyday bridge between schedule and cost. Three uses: the **ramp** (can the site actually mobilise from
40 to 120 in eight weeks — recruitment, camp, inductions?); the **peak** (a histogram peaking at 120 against
95 demonstrably available is a plan that fails before it starts — level it now, Domain 10, KA 10.3.3, or
re-sequence); and the **leading indicator** (actual headcount tracking below plan predicts a productivity and
progress shortfall weeks before `CPI` moves — Domain 4, KA 4.1.2; Domain 6's `PF`, KA 6.1.2). The histogram
is also the origin of the time-phased labour budget (Domain 3, KA 3.3): hours × rates per period *is* the
labour `PV`. Where no resource loading exists, the baseline's phasing is an assertion (Domain 10, Advanced
10.A.1's health check asks exactly this).

### 8.2.3 The subsidiary plans

**The principle.** Around the three baselines sit the subsidiary management plans — **quality, resource, risk,
procurement, communications** and their integration — each defining *how* that aspect will be managed. Controls
touches all of them: the risk plan feeds contingency (Domains 3, 12); the procurement plan drives commitments
(Domains 5, 7); the communications plan drives reporting (Domain 4). Planning is not a document-production
exercise; it is the assembly of the integrated system the rest of the lifecycle runs on.

### Key terms — KA 8.2

| Term | Meaning |
|---|---|
| **Scope / WBS** | What will be delivered / its hierarchical decomposition. |
| **100 % rule** | The WBS captures all of the scope and only the scope. |
| **Baselines (scope/schedule/cost)** | The approved, integrated plans control measures against. |
| **Subsidiary plans** | Quality, resource, risk, procurement, communications, integration. |

### Sample MCQs — KA 8.2

**MCQ 8.2-A `[8.2.1 · Recall]`** The "100 % rule" for a WBS means it:
- A. Must be 100 % complete before work starts.
- B. Guarantees 100 % on-time delivery.
- C. Requires 100 % resource loading.
- D. Captures 100 % of the scope — no more, no less. ✅

*Rationale:* The 100 % rule concerns scope completeness and non-overlap. It is not about pre-start completion,
delivery certainty, or resource loading.

**MCQ 8.2-B `[8.2.2 · Analysis]`** Why must the cost baseline be phased over the schedule?
- A. To produce Planned Value for earned-value measurement. ✅
- B. To reduce the BAC.
- C. To satisfy IFRS 15.
- D. It need not be.

*Rationale:* Phasing the cost baseline over the schedule creates `PV` (Domain 3, KA 3.3), the basis of earned
value. It does not change `BAC`, is not an IFRS requirement, and is essential (not optional) for EVM.

**MCQ 8.2-C `[8.2.2 · Application]`** A cost baseline of USD 2,400,000 is phased evenly over a 12-month
schedule. At the end of month 4, Planned Value (`PV`) is:
- A. USD 2,400,000
- B. USD 200,000
- C. USD 800,000 ✅
- D. USD 600,000

*Rationale:* Even phasing gives `2,400,000 / 12 = 200,000` per month; `PV` at month 4 `= 4 × 200,000 =
800,000`. A is the whole `BAC`; B is a single month; D stops at three months.

**MCQ 8.2-D `[8.2.3 · Recall]`** Which subsidiary management plan feeds the project's contingency?
- A. The communications plan.
- B. The risk plan. ✅
- C. The quality plan.
- D. The procurement plan.

*Rationale:* The risk plan feeds contingency (Domains 3 and 12). The communications plan drives reporting,
the procurement plan drives commitments, and the quality plan defines how quality is managed — none of them
sets contingency.

### Self-check — KA 8.2

1. Why is the WBS called the backbone of the plan? *(Schedule, cost coding, control accounts and earned value
   are all built on it.)*
2. Name the three baselines and one subsidiary plan and say what each controls. *(Scope/schedule/cost; e.g.
   risk plan → contingency.)*

---

## Knowledge Area 8.3 — Executing

*Topics: 8.3.1 directing the work · 8.3.2 managing resources and stakeholders · 8.3.3 quality assurance and
procurement execution.*

### 8.3.1 Directing the work

**The principle.** **Executing** is where the plan becomes deliverables: the team performs the work packages,
consuming most of the budget and generating the actual cost, progress and issues the controls function
measures. The controls professional's job here is to keep the measurement flowing — progress captured against
the earning rules (Domain 6), cost committed and accrued (Domain 5) — so that monitoring & controlling has
true data to act on.

### 8.3.2 Managing resources and stakeholders

**The principle.** Executing draws on people, plant and materials, and on continuous stakeholder engagement
(the communications plan in action). Resource availability is a leading indicator (Domain 4, KA 4.1.2): a
resource shortfall shows up in productivity before it shows in `CPI`. Keeping stakeholders informed through
disciplined reporting (Domain 4) is what maintains the trust a project needs when it must report bad news.

### 8.3.3 Quality assurance and procurement execution

**The principle.** **Quality assurance** (building quality into the process, not just inspecting it out) and
**procurement execution** (placing and managing the orders/subcontracts that become commitments, Domains 5, 7)
happen here. A favourable cost variance won by cutting quality assurance is a false economy that returns as
rework (Domain 4, KA 4.2.5) — a link the controls professional must keep visible.

### Key terms — KA 8.3

| Term | Meaning |
|---|---|
| **Executing** | Performing the work packages and producing deliverables. |
| **Quality assurance** | Building quality into the process (vs inspecting it out). |
| **Procurement execution** | Placing and managing orders/subcontracts (commitments). |

### Sample MCQs — KA 8.3

**MCQ 8.3-A `[8.3.1 · Recall]`** Most of a project's budget and actual cost is typically generated during:
- A. Initiating.
- B. Planning.
- C. Executing. ✅
- D. Closing.

*Rationale:* Executing is where the work is performed and most cost is incurred — hence the emphasis on
measurement discipline there. The other groups consume comparatively little budget.

**MCQ 8.3-B `[8.3.3 · Analysis]`** A package shows a favourable cost variance achieved by cutting quality
assurance activities. The controls professional should treat this as:
- A. A false economy likely to return as rework — keep the quality liability visible. ✅
- B. A genuine saving to be banked in the forecast.
- C. Evidence that quality assurance was over-scoped.
- D. Grounds to raise the profit forecast immediately.

*Rationale:* Quality is built into the process, not inspected out; a variance won by cutting assurance hides
a rework liability. B and D bank a saving that is likely to reverse; C draws a scoping conclusion the
variance does not support.

**MCQ 8.3-C `[8.3.2 · Application]`** During executing, a shortfall in skilled resources will typically show
up first in:
- A. The reported `CPI`.
- B. Productivity — a leading indicator that precedes the cost indices. ✅
- C. The final account.
- D. The project charter.

*Rationale:* Resource availability is a leading indicator: the shortfall degrades productivity before it
flows through to `CPI` (a lagging measure). The final account is a closing artefact and the charter is fixed
at initiating.

### Self-check — KA 8.3

1. What is the controls professional's core job during executing? *(Keep true measurement flowing — progress
   against earning rules, cost committed/accrued — so controlling can act.)*
2. Why is a cost saving from cutting quality assurance often false? *(It returns as rework — a favourable
   variance hiding a quality liability.)*

---

## Knowledge Area 8.4 — Monitoring & Controlling

*Topics: 8.4.1 performance measurement · 8.4.2 integrated change control · 8.4.3 controlling the constraints.*

### 8.4.1 Performance measurement

**The principle.** **Monitoring & controlling** runs *in parallel* with executing: it measures actual
performance against the baselines, forecasts outcomes, and triggers corrective action. This is the home ground
of project controls — it *is* earned value (Domain 6), variance analysis (Domain 4), forecasting (Domains 3,
6) and risk monitoring (Domain 12), assembled into the management report that drives decisions. The purpose is
not to *observe* the project but to *change its trajectory* in time.

### 8.4.2 Integrated change control

**The principle.** **Integrated change control** is the process that ensures every change is assessed for its
**full, cross-constraint** impact (scope, schedule, cost, quality, risk) before approval, and that approved
changes update **all** affected baselines coherently (Domain 5, KA 5.4). "Integrated" guards against the
common failure of approving a scope change for its direct cost while missing its schedule and risk knock-ons.
The controls professional runs the impact assessment and maintains the change log.

### 8.4.3 Controlling the constraints

**The principle.** Controlling covers every constraint — scope (against creep), schedule (against slippage),
cost (against overrun), quality (against defect), and risk (against emerging threats) — as one integrated
activity, because they trade off against each other (accelerating schedule costs money; cutting cost may cut
quality). The controls professional's integrated view — following a variance from a slipped activity through
its cost, forecast and commercial consequences (Domain 7, KA 7.5.3) — is exactly what monitoring & controlling
demands.

### 8.4.4 Worked example — one month of monitoring & controlling on Northwind

One monthly cycle on the data-centre fit-out, end to end:

1. **Measure.** Progress is captured against the earning rules for each work package; the cost ledger closes
   for the month with accruals raised for work done but not yet invoiced (Domain 5); the month's `EV`, `AC`
   and `PV` roll up by control account.
2. **Analyse.** Variance analysis attributes the month's adverse cost variance roughly 60/40 between a
   plant-rate rise and rework in the containment package (Domain 4, KA 4.2).
3. **Forecast.** The `EAC` is re-struck: the rate rise is now locked into the contract, so it is treated as
   atypical, but the rework cause persists — a blended forecasting method is chosen and defended (Domain 6,
   KA 6.3.3).
4. **Act.** The containment package is given a recovery plan and a tightened inspection regime; the change
   log records one approved variation from the month's change control (Domain 5, KA 5.4).
5. **Report.** The dashboard shows RAG status and trend; the exception report carries the two
   out-of-tolerance control accounts with cause, impact and action (Domain 4, KA 4.3).

This cycle — measure, analyse, forecast, act, report — repeated every period, *is* monitoring & controlling;
the artefacts differ from project to project, the loop does not.

### Key terms — KA 8.4

| Term | Meaning |
|---|---|
| **Monitoring & controlling** | Measuring against baselines and acting to correct — parallel to executing. |
| **Integrated change control** | Assessing every change across all constraints before approval. |
| **Constraint trade-offs** | Scope/schedule/cost/quality/risk balanced against each other. |

### Sample MCQs — KA 8.4

**MCQ 8.4-A `[8.4.2 · Analysis]`** A scope change is approved on its direct cost alone, without assessing
schedule and risk impact. This violates the principle of:
- A. The 100 % rule.
- B. Earned value.
- C. Integrated change control. ✅
- D. Going concern.

*Rationale:* Integrated change control requires assessing a change across *all* constraints. The 100 % rule is
about WBS scope; earned value and going concern are unrelated to this failure.

**MCQ 8.4-B `[8.4.1 · Recall]`** Monitoring & controlling primarily exists to:
- A. Observe and document the project.
- B. Produce the charter.
- C. Replace executing.
- D. Measure against the baselines and act to change the trajectory. ✅

*Rationale:* Its purpose is corrective — to change the outcome, not merely observe. It runs alongside (not
instead of) executing, and the charter is an initiating artefact.

**MCQ 8.4-C `[8.4.3 · Application]`** A project manager proposes accelerating the works to recover schedule
slippage. Before approval, the controls professional should:
- A. Assess only the schedule benefit — schedule is the priority.
- B. Assess the acceleration's cost, quality and risk impacts alongside the schedule gain — the constraints
  trade off. ✅
- C. Decline any assessment, since acceleration is an executing matter.
- D. Reduce scope automatically to fund the acceleration.

*Rationale:* Controlling treats the constraints as one integrated activity: accelerating schedule costs
money and can stress quality and risk, so the full trade-off is assessed. A ignores the trade-offs; C
abdicates the controlling role; D pre-empts a decision that belongs to change control.

**MCQ 8.4-D `[8.4.1 · Recall]`** The repeating monthly cycle of monitoring & controlling runs:
- A. Measure → analyse → forecast → act → report. ✅
- B. Report → act → forecast → analyse → measure.
- C. Forecast → measure → report → analyse → act.
- D. Act → measure → report → forecast → analyse.

*Rationale:* The loop starts from measured `EV`/`AC`/`PV`, attributes variances, re-strikes the forecast,
triggers corrective action, and reports the position. The other orderings act or report before anything has
been measured or analysed.

### Self-check — KA 8.4

1. Why is change control called "integrated"? *(Every change is assessed across all constraints and updates
   all affected baselines coherently.)*
2. Which controls-discipline domains come together in monitoring & controlling? *(Earned value — 6; variance/
   reporting — 4; forecasting — 3/6; risk — 12.)*

---

## Knowledge Area 8.5 — Closing

*Topics: 8.5.1 contract and project closure · 8.5.2 handover and the final account · 8.5.3 lessons learned.*

### 8.5.1 Contract and project closure

**The principle.** **Closing** formally completes the project (or a phase): confirming deliverables are
accepted, closing contracts (release of retention, resolution of claims, Domain 7), demobilising resources,
and archiving records. Orderly closure protects the organisation — unclosed contracts and unresolved claims
are liabilities that linger.

### 8.5.2 Handover and the final account

**The principle.** **Handover** transfers the completed asset to the operator/client with the documentation
needed to run it; the **final account** settles the commercial position — final remeasurement, agreed
variations and claims, retention release (Domain 7, KAs 7.3–7.4). The controls professional is central to the
final account: it is the last, definitive reconciliation of scope, cost, billing and revenue for the project.

**Worked example 8.5.2 — settling Northwind's final account.**

1. **Setup.** Original contract value **USD 500,000**; approved variations **USD 40,000**; an agreed claim
   settlement of **USD 15,000**. Retention of **5 %** was withheld through the job, with half released at
   practical completion and the balance due after the defects period.
2. **Formula.** `Final account = original value + variations + agreed claims`; `total retention = 5 % ×
   final account`; the remaining release is half of the total.
3. **Substitution.** `Final account = 500,000 + 40,000 + 15,000 = 555,000`; `total retention = 5 % × 555,000
   = 27,750`; released at practical completion `27,750 / 2 = 13,875`; remaining after defects `13,875`.
4. **Result.** A final account of **USD 555,000**, with **USD 13,875** of retention still to collect after
   the defects period.
5. **Interpretation.** The final account is the definitive reconciliation of scope, variations, claims and
   retention (Domain 7); until the last 13,875 is collected, closing is not finished — it is a real
   receivable the controls professional tracks to zero.

**Completions and turnover on systems-based projects.** On industrial and process projects, the end-game is
not run on WBS percentages but on a **completions system**: the plant is decomposed into **systems and
subsystems**, each walked down against checklists to **mechanical completion**, then pre-commissioned,
commissioned and **turned over** to the client/operations with a certificate and its **punch list** (A-items
blocking turnover; B-items completable after). Progress in the last 15 % of a project is honestly measured
here — subsystems turned over, punch items open/closed per system — not in earned value, which plateaus
exactly when this matters (the 90 %-complete plateau of Domain 6, Advanced 6.A.3). The completions database,
not the schedule, becomes the single source of truth for what remains.

**Worked example 8.5.2b — reading a turnover dashboard.**

1. **Setup.** A plant has **42 subsystems**. At the data date **28** are mechanically complete, of which
   **17** are turned over; open punch items: **61 A-items**, **214 B-items**.
2. **Formula.** Progress by count: `MC % = MC subsystems ÷ total`; `turnover % = turned over ÷ total`.
3. **Substitution.** `28 ÷ 42 ≈ 67 %` mechanically complete; `17 ÷ 42 ≈ 40 %` turned over.
4. **Result.** The gap between 67 % and 40 % — eleven subsystems stuck between MC and turnover — *is* the
   end-game workload, and the 61 A-items are its critical path.
5. **Interpretation.** A project reporting "94 % complete" by EV while only 40 % of subsystems have turned
   over is not lying — it is measuring the wrong thing for this phase. From MC onward, walk-downs, punch
   burn-down rate and A-item ageing are the honest progress measures (cross-ref Domain 4, KA 4.1.2 leading
   indicators; Domain 6, Advanced 6.A.3).

### 8.5.3 Lessons learned

**The principle.** **Lessons learned** capture what worked and what did not — including the **estimating and
performance data** (actual `CPI`, productivity, unit costs) that feed the next project's estimates (Domain 3,
KA 3.2) and, increasingly, the historical datasets that train forecasting models (Domain 13). A project that
does not capture its lessons forces the next one to relearn them; the controls function is the natural custodian
of the quantitative lessons, because it holds the performance data.

### Key terms — KA 8.5

| Term | Meaning |
|---|---|
| **Closing** | Formal completion — acceptance, contract closure, demobilisation, archiving. |
| **Final account** | The definitive commercial settlement of the project. |
| **Lessons learned** | Captured experience and performance data feeding future projects. |

### Sample MCQs — KA 8.5

**MCQ 8.5-A `[8.5.3 · Analysis]`** Why is the controls function the natural custodian of quantitative lessons
learned?
- A. It writes the contract.
- B. It approves the charter.
- C. It runs procurement.
- D. It holds the performance data (actual CPI, productivity, unit costs) that feeds future estimates and models. ✅

*Rationale:* Controls holds the earned-value and cost performance data that becomes future estimating and
model-training input. Contracts, charter and procurement are other functions.

**MCQ 8.5-B `[8.5.2 · Application]`** A contract closes with an original value of USD 800,000, approved
variations of USD 60,000 and an agreed claim of USD 20,000. Retention of 5 % was withheld, with half released
at practical completion. The retention still to collect after the defects period is:
- A. USD 44,000
- B. USD 22,000 ✅
- C. USD 21,500
- D. USD 20,000

*Rationale:* `Final account = 800,000 + 60,000 + 20,000 = 880,000`; `total retention = 5 % × 880,000 =
44,000`; half was released at practical completion, leaving `44,000 / 2 = 22,000`. A forgets the
practical-completion release; C omits the agreed claim from the retention base; D computes retention on the
original value only.

**MCQ 8.5-C `[8.5.1 · Recall]`** Orderly closure matters chiefly because:
- A. It guarantees the project made a profit.
- B. Unclosed contracts and unresolved claims are liabilities that linger. ✅
- C. It removes the need for lessons learned.
- D. It allows the baselines to be revised retrospectively.

*Rationale:* Closing protects the organisation — releasing retention, resolving claims and closing contracts
so no liabilities linger. It cannot manufacture profit (A), lessons learned are part of closing rather than
replaced by it (C), and baselines are never revised retrospectively (D).

### Self-check — KA 8.5

1. What does the final account settle? *(The definitive commercial position — final remeasurement, agreed
   variations/claims, retention release.)*
2. How do lessons learned connect to Domains 3 and 13? *(Actual performance data improves future estimates and
   trains forecasting models.)*

---

## Knowledge Area 8.6 — Development approaches: predictive, iterative, incremental & adaptive

*Topics: 8.6.1 the approach spectrum · 8.6.2 incremental vs iterative — the key distinction · 8.6.3 hybrid
delivery · 8.6.4 tailoring and choosing an approach.*

### 8.6.1 The approach spectrum

**The principle.** How the lifecycle is *applied* varies along a spectrum from **predictive** (plan-driven:
scope fixed up front, delivered in sequence — "waterfall") to **adaptive** (change-driven: scope evolves,
delivered in short cycles — agile). Between them sit **iterative** and **incremental** approaches. The right
point on the spectrum depends on **requirements certainty** and **change rate**: stable, well-understood scope
suits predictive; uncertain, evolving scope suits adaptive.

> **Fig 8.6.1 — The development-approach spectrum.** *Caption:* from predictive to adaptive. *Underlying
> data:* predictive → iterative → incremental → adaptive, mapped against requirements certainty (high→low) and
> change rate (low→high). *Render-ready description:* a horizontal spectrum with four labelled bands; beneath
> it two gradient bars — "Requirements certainty" (high on the left) and "Change rate" (high on the right) —
> showing why each approach fits its conditions. *Animation storyboard (digital-only):* a project profile
> (certainty/change sliders) is set, and a marker snaps to the recommended band on the spectrum.

### 8.6.2 Incremental versus iterative — the key distinction

**The principle (commonly confused).** These two words are *not* synonyms:

- **Incremental** delivery builds the product in **usable slices**, each adding a **new working part** to the
  whole. Think of building a house room by room — after each increment you have *more* finished product.
- **Iterative** delivery **refines the same product over repeated passes**, improving it each time. Think of
  sculpting — each pass reworks the *same* piece toward the final form.
- **Agile combines both:** it delivers **increments** of working product (incremental) while **refining** them
  through feedback each sprint (iterative).

A worked contrast: a reporting dashboard built **incrementally** adds a new complete chart each cycle (cost
chart, then schedule chart, then risk chart); built **iteratively**, the *same* draft dashboard is reworked
each cycle (rough layout → better layout → final layout). Most real adaptive delivery does both at once.

> **Fig 8.6.2 — Incremental vs iterative.** *Caption:* adding parts vs refining the whole. *Underlying data:*
> three cycles each. *Render-ready description:* two rows. Top ("Incremental"): three panels, each adding a new
> complete block to a growing bar (⬛ → ⬛⬛ → ⬛⬛⬛). Bottom ("Iterative"): three panels of the *same* shape
> getting progressively refined (rough outline → detailed → polished). Brand-blue fills. *Animation storyboard
> (digital-only):* top row grows by one block per step; bottom row morphs the same shape to higher fidelity per
> step — visually separating the two ideas.

### 8.6.3 Hybrid delivery

**The principle.** **Hybrid** delivery combines predictive and adaptive elements — for example, predictive
**stage-gate governance and milestone reporting** wrapped around **agile execution** of the software/design
elements, while civil works run predictively. Hybrids are the norm on large engineering programmes, where some
scope is stable (structures) and some is volatile (systems/software). The controls challenge is to **measure
and report coherently across both** — the subject of Domain 9, KA 9.6.

### 8.6.4 Tailoring and choosing an approach

**The principle.** **Tailoring** adapts the approach to the specific project — its size, risk, regulatory
context, and requirements stability — rather than applying a method dogmatically. The controls professional's
stake is direct: the approach determines *how progress and cost are measured*. Predictive delivery measures
against a fixed baseline (Domains 3, 6); adaptive delivery measures velocity and burn against evolving scope
(Domain 9); hybrids need both, reconciled. Choosing and tailoring well is what makes the controls approach fit
the delivery rather than fighting it.

**Worked example 8.6.4 — choose an approach.** For Northwind's data-centre fit-out: the **civil/mechanical**
scope is well-defined and regulated → **predictive** (fixed baseline, earned value); the **monitoring/control
software** integration is uncertain and change-prone → **adaptive** (sprints, velocity). The programme is
therefore **hybrid**, with predictive stage-gates over agile software execution — and the controls function
reports earned value on the civils and AgileEVM/velocity on the software, reconciled at the programme level.

**Worked example 8.6.4b — two project profiles, two approaches.** The same logic applied to three contrasting
profiles:

| Profile | Requirements certainty | Change rate | Best-fit approach |
|---|---|---|---|
| A regulated bridge replacement | High | Low | Predictive (fixed baseline, earned value) |
| A customer-facing mobile app | Low | High | Adaptive (Scrum, velocity/burnup) |
| A hospital IT + building programme | Mixed | Mixed | Hybrid (stage-gates over agile execution) |

The approach follows the **conditions**, not preference: high certainty and low change reward a fixed plan;
low certainty and high change reward short cycles and feedback; a mixed profile takes a mix. The controls
professional then measures each with the matching method — fixed-baseline earned value for the bridge
(Domain 6), velocity/AgileEVM for the app (Domain 9), and both reconciled at programme level for the hybrid.
Choosing the approach to fit the delivery rather than dogmatically is the professional stance (worked example
8.6.4).

**AI in this KA.** AI assists across the lifecycle (developed fully in Domain 13, KA 13.5): drafting charters
and plans, generating WBS candidates, analysing stakeholder and communications data, supporting change-impact
assessment, and mining lessons-learned archives. The governance boundary holds throughout: AI proposes plans,
estimates and analyses; the professional decides, and remains accountable for the baseline, the change
decision and the report. **AI proposes, the professional disposes.**

### Key terms — KA 8.6

| Term | Meaning |
|---|---|
| **Predictive / adaptive** | Plan-driven (fixed scope) / change-driven (evolving scope). |
| **Incremental** | Building the product in usable slices — adding parts to the whole. |
| **Iterative** | Refining the same product over repeated passes. |
| **Hybrid** | Combining predictive governance with adaptive execution. |
| **Tailoring** | Adapting the approach to the specific project. |

### Sample MCQs — KA 8.6

**MCQ 8.6-A `[8.6.2 · Analysis]`** Building a product in usable slices, each adding a new working part to the
whole, describes:
- A. Iterative delivery.
- B. Predictive delivery.
- C. Waterfall.
- D. Incremental delivery. ✅

*Rationale:* Incremental delivery adds new working parts (slices) to the whole. Iterative refines the *same*
product; predictive/waterfall delivers the full scope in sequence.

**MCQ 8.6-B `[8.6.2 · Analysis]`** Refining the *same* product over repeated passes, improving it each time,
describes:
- A. Iterative delivery. ✅
- B. Incremental delivery.
- C. Framework contracting.
- D. Remeasurement.

*Rationale:* Iterative delivery reworks the same product toward its final form. Incremental adds new parts; the
others are commercial concepts.

**MCQ 8.6-C `[8.6.4 · Application]`** A programme with well-defined, regulated civils and uncertain,
change-prone software is best delivered:
- A. Fully predictive.
- B. Fully adaptive.
- C. Hybrid — predictive governance over the civils, adaptive execution of the software. ✅
- D. Without any baseline.

*Rationale:* Mixed requirements certainty calls for a hybrid tailored to each part. Forcing one approach on
both, or abandoning baselines, fits neither.

**MCQ 8.6-D `[8.6.1 · Recall]`** The two project conditions that determine where a project should sit on the
predictive-to-adaptive spectrum are:
- A. Team size and contract value.
- B. Client preference and industry custom.
- C. Requirements certainty and change rate. ✅
- D. Budget size and project duration.

*Rationale:* The spectrum is mapped against requirements certainty and change rate — stable, well-understood
scope suits predictive; uncertain, evolving scope suits adaptive. Team size, value, duration and preference
do not determine the fit.

**MCQ 8.6-E `[8.6.4 · Application]`** An adaptive work stream is funded for 20 Sprints at USD 200,000 per
Sprint. After 8 Sprints, achieved velocity is 25 points per Sprint and 600 backlog points remain. If velocity
holds and no scope is cut, the funding gap is:
- A. USD 2,400,000 ✅
- B. USD 800,000
- C. USD 4,800,000
- D. USD 3,200,000

*Rationale:* `Sprints needed = 600 ÷ 25 = 24` against `20 − 8 = 12` funded Sprints remaining; `extra Sprints
= 24 − 12 = 12`; `gap = 12 × 200,000 = 2,400,000`. B compares the 24 Sprints needed with the 20 funded in
total rather than the 12 remaining; C funds all 24 remaining Sprints, ignoring the 12 already funded; D
wrongly nets off the 8 Sprints already completed.

### Self-check — KA 8.6

1. State the incremental-vs-iterative distinction in one sentence each. *(Incremental — add new working parts
   to the whole; iterative — refine the same product over passes.)*
2. Why does the development approach matter to the controls professional? *(It determines how progress and cost
   are measured — fixed baseline vs velocity vs both.)*

---

## Advanced topics — Domain 8

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 8.A.1 — Programme and portfolio governance

**The principle.** A **project** delivers outputs. A **programme** coordinates related projects (and the
business change around them) to realise **benefits none of them could deliver alone** — coordinated
benefit: the operations centre in this domain's case study needed a building, a platform *and* a
transition before a single response time improved. A **portfolio** is the organisation's whole set of
investments, selected and balanced against strategy — **investment selection**. Each tier carries a board
with distinct decisions. The **project board** decides *within* the project: tolerance breaches, baseline
changes (Domain 5, KA 5.4), gate proceed/hold (8.4). The **programme board** decides *between* projects:
sequencing, shared resources and contingency, and benefit trade-offs — accepting pain on one project to
protect a benefit carried by another. The **portfolio board** decides *whether investments exist at all*:
entry, exit, prioritisation, and the balance of risk and return across the set — the business-case
question (8.1.1) asked continuously, not once at authorisation.

**Where controls reports differently to each.** This is Domain 4's cadence-and-audience discipline
(KA 4.3.4) made structural. The project tier receives granular, exception-and-forecast reporting against
the baselines. The programme tier receives cross-project reporting: the same data aggregated so
inter-project dependencies and the benefit position are visible. The portfolio tier receives **comparable**
reporting — a consistent basis of estimate class, contingency confidence and forecast method across
projects, because a board choosing between investments is misled the moment one project's P80 is set
beside another's unstated optimism (the executive perspective's recurring concern). All three views only
work if data is coded once at source and aggregated automatically (Domain 1, KA 1.5). The tiers fail when
decisions leak across them: a portfolio board re-litigating a single variation, or a project board quietly
deciding an investment question, each signals governance that has lost its levels.

### Advanced 8.A.2 — Benefits realisation

**The principle.** KA 8.1.4 distinguishes *project* success (delivered to plan) from *benefit* success
(the outcome realised); **benefits realisation** is that distinction operationalised. Each benefit in the
business case gets a **benefit profile**: what the benefit is, the **measure** that will evidence it, the
**baseline** value of that measure today, the target, the date it becomes measurable — and, above all, a
named **benefit owner**. The case study did exactly this at initiation: current response times and energy
consumption baselined on day one, because an improvement with no "before" cannot be demonstrated after.

**Why benefits die without an owner.** Most benefits are realised **after handover** (8.5) — and at
closure the project team disbands, the budget line closes, and the sponsor moves to the next investment.
If measurement belongs to nobody who survives closure, it simply stops: the asset operates, but no one can
say whether the investment paid. The benefit owner must therefore sit in the **operating organisation**,
with the measures wired into its routine reporting rather than a project artefact that dies with the
project. The controls function's contributions are concrete: baselining the measures at initiation;
assessing **benefit impact inside change control** (8.4.2 — the case study's descope was tested against
the benefit case, not just cost and schedule); reporting project success and benefit success as separate
registers at closure; and handing a live measurement plan, not a good intention, to the named owners. A
project can be a delivery triumph and an investment failure; benefit profiles, owners and surviving
measures are what let the organisation know which it got. A benefit nobody owns is a forecast nobody
checks.

### Advanced 8.A.3 — Stage-gate design

**What a well-designed gate asks.** A gate is a **decision to keep investing**, and a good one demands
**evidence, not narrative**: questions whose answers are artefacts, not adjectives. What class is the
estimate (Domain 3, KA 3.2.1), and does its accuracy range fit the commitment now being made? What is the
quantified risk exposure (Domain 12), and does remaining contingency cover remaining exposure at the
stated confidence (12.3.1, 12.3.3)? Are the baselines genuinely integrated (8.2.2)? Is the benefit case
still positive at the current forecast — not the original one (8.A.2)?

**Criteria tied to maturity and exposure.** Gate criteria should grade with the decision. An early
concept gate can properly proceed on a Class 5/4 estimate, because the commitment is small and the range
is honest; the **sanction gate** — which commits the money — demands Class 3 or better, with the basis of
estimate documented (3.2.3). A project seeking full sanction on a Class 5 estimate fails the gate however
eloquent its pack, because the evidence cannot support the commitment. Risk works the same way: exposure
quantified against the register and a P-level, not a reassuring paragraph. Gates may pass **with
conditions** — as the case study's Gate 2 did — but conditions are tracked to closure, or they were
decoration.

**The failure mode: gates as theatre.** The date fixed by the schedule, the decision pre-made, the pack
curated to pass, the kill option never exercised. The diagnostic question is uncomfortable and simple:
*when did a gate last stop, descope or recycle a project?* A gate that cannot say no is a milestone
wearing governance clothing — all of the cost of a control, none of the function.

### Advanced 8.A.4 — The PMO as a controls institution

**Three postures.** A **project management office (PMO)** comes in three broad forms. An **enabling**
(supportive) PMO provides templates, methods, training, coaching and the lessons library — influence
without authority. A **controlling** PMO adds compliance: mandated methods, baseline and change-control
standards, gate assurance and independent review. A **directive** PMO goes further and runs delivery —
project managers report into it. The right posture is a tailoring decision (8.6.4): higher stakes and
lower organisational maturity justify more control.

**The PMO as owner of the methods in this book.** Someone must own the standards no single project can:
baseline discipline and change control (Domain 5, KA 5.4), reporting standards and tolerances (Domain 4),
estimating norms, and the **lessons and performance data** of 8.5.3 — the actual `CPI`s, productivities
and unit costs that turn one project's history into the next project's estimate (Domain 3, KA 3.2) and,
increasingly, into the datasets forecasting models train on (Domain 13). The PMO is the natural
institution for that ownership, and — serving 8.A.1's tiers — the aggregation engine for programme and
portfolio reporting.

**What makes PMOs fail.** Policing without value: collecting reports that feed no decision, enforcing
template compliance while forecast quality rots, measuring its own success by adherence rather than by
decision quality. The test is the one this book applies everywhere: *does it change a decision?* A PMO
earns its keep when projects' numbers become more trustworthy and their decisions faster; one that adds a
reporting layer without adding trust is overhead wearing a controls badge — and is, rightly, the first
thing cut.

### Advanced 8.A.5 — Decision latency: the cost of the gate itself

**The principle.** Governance sections usually price the risk of deciding *wrongly*; they rarely price the
risk of deciding *slowly*. Between "the pack is ready" and "the decision is made", the programme keeps
burning at its run rate — often without authorised direction, so the burn buys progress on a plan the gate
may be about to change. **Decision latency** is that interval, and it is a **governable quantity**: it is
set by committee cadence, pack cut-off dates and delegation thresholds, all of which are design choices of
the stage-gate architecture (8.A.3) and the PMO that administers it (8.A.4) — not facts of nature.

**Worked example 8.A.5 — pricing a slow gate.**

1. **Setup.** A programme runs at **USD 1,200,000 a month**. Its investment committee meets on an
   **eight-week cycle**; a gate pack misses one cut-off, and the decision lands **six weeks** after the
   programme was ready for it.
2. **Formula.** `Cost of latency = latency × run rate`.
3. **Substitution.** `1.5 months × 1,200,000 = USD 1,800,000`.
4. **Result.** **USD 1,800,000** of burn under a direction the gate may be about to change — in the worst
   case (a kill decision), 1,800,000 spent on a programme the organisation had already decided, in
   substance, not to continue.
5. **Interpretation.** The gate protected the organisation from a bad decision and charged it 1.8 million
   for a slow one; latency is a real number that belongs in the governance design, not an accident of the
   calendar.

**The design responses.** Align committee cadence to gate forecast dates — the schedule knows when gates
are coming (Domain 10), so the calendar can be built around them rather than against them. Set
**delegation thresholds** so decisions below a defined size do not queue for the main committee (8.A.1's
tiers, applied to speed). Allow conditional **proceed-at-risk** authorisations with explicit caps, logged
like any other authorisation (KA 8.4.2). Forecasting gate-readiness dates and flagging cadence mismatches
is a mechanical scan; deciding what may proceed at risk, and at what cap, is the accountable owner's call.

### Advanced 8.A.6 — Stakeholder and communication management as a controls discipline

**The principle.** KA 8.1.3 answers *who matters*; this topic is the operating discipline that follows —
assessing stakeholders, planning their engagement, and running communication as a managed process rather
than an inbox. For a controls function this is not soft-skills garnish. The controls professional's product
*is* communication — the report, the forecast, the variance narrative (Domain 4) — and most controls
failures that end in surprise were communication failures first: the number existed; it did not land.

**Assessment at working level.** The **power/interest grid** (8.1.3) gives four engagement postures —
**manage closely**, **keep satisfied**, **keep informed**, **monitor** — with two honest caveats. First,
positions move: a regulator dormant for a year moves to "manage closely" the week an incident occurs, and a
keep-informed operations team becomes manage-closely as handover (8.5.2) approaches. Second, the grid is a
conversation aid, not a filing system — its value is the argument it forces about who can stop the project,
not the tidiness of the quadrants. Salience is re-reviewed at gates (8.A.3), because the commitment — and
therefore who cares — changes with each one.

**The communication plan as a real artefact.** Not a paragraph of intent but a table with owners and
cadences, in the spirit of the toolkits:

| Audience | What they need | Cadence | Channel | Owner |
|---|---|---|---|---|
| Steering board | Decisions and exceptions | Monthly | Gate pack + dashboard | Project manager |
| Client commercial team | Variations and claims position | Monthly | Commercial report | Commercial manager |
| Site supervisors | Two-week lookahead | Weekly | Stand-up + plan extract | Planner |
| Finance | Accruals and forecast movements | Monthly close | Cost pack | Controls lead |

**The rules that make it work.** Each audience gets the decision *they* must take — Domain 4's discipline of
designing the report for the decision (KA 4.3.1), applied row by row. Cadence promises are kept or
renegotiated, never silently missed: a report that arrives when it arrives trains its readers to stop
relying on it. Escalation paths are named people, not job titles in an organisation chart. And bad news
travels *fastest* — the risk-culture point of Domain 12 (Advanced 12.A.4): how the messenger is treated this
quarter sets the quality of next quarter's information.

**Engagement is measured, not assumed.** Attendance and read rates are weak signals — a full room proves
diaries, not engagement. The honest measures are behavioural: decision latency at the forums the plan feeds
(8.A.5), the age of unanswered actions, and whether stakeholders raise issues *early* — a leading indicator
(Domain 4, KA 4.1.2) that the channel is trusted, because people escalate into channels they believe will
act. Drafting stakeholder-tailored versions of one controls truth — a board summary, a client letter and a
site briefing from the same variance data — is among the stronger and safer AI tasks (Domain 13, KA 13.5.8),
provided the numbers are locked before the words are drafted and the professional signs what goes out: AI
proposes, the professional disposes.

---

## Case study — Domain 8: a city operations centre, gate by gate (smart cities)

### Background

A city government commits to building an **integrated operations centre** — a single facility from
which traffic management, emergency dispatch, street lighting and environmental monitoring are
coordinated in real time. The programme combines three very different kinds of work: the
**refurbishment of an existing building** to house the centre (civil and building works —
well-defined, regulated, and therefore **predictive**); a **data and IoT platform** that ingests
feeds from thousands of sensors and cameras across the city (software — uncertain and change-prone,
and therefore **adaptive**); and the **systems integration and transition** that turns a building
and a platform into a working operation. Following KA 8.6.3, delivery is **hybrid**: agile execution
of the platform wrapped in predictive **stage-gate governance**, with formal gates at which the city
decides whether the programme proceeds. This case walks that lifecycle gate by gate — a deliberately
different project from the Northwind mini-case threaded through the KAs — so the reader sees every
process group exercised once more, this time on a smart-cities programme where the two halves of a
hybrid must be measured differently yet governed together.

### Initiating (KA 8.1)

The **business case** (8.1.1) rests on measurable benefits, not on the asset itself: a targeted
reduction in **emergency incident-response times** (dispatch coordinated from a single, live picture
of the city) and **energy optimisation** across municipal buildings and street lighting
(sensor-driven control). Feasibility confirms the building can be refurbished within heritage and
planning constraints, the platform can lawfully ingest the data it needs, and the city's operating
budget can sustain the centre once built. The **project charter** (8.1.2) then authorises the
programme and its manager with a high-level funding envelope of **USD 40,000,000** and outline
milestones — the seed the planning baselines will elaborate.

Stakeholder identification (8.1.3) places the key players on the power/interest grid:

| Stakeholder | Power | Interest | Engagement strategy |
|---|---|---|---|
| Mayor's office (sponsor) | High | High | Manage closely |
| Utility companies (data providers) | High | Low | Keep satisfied |
| Community groups (privacy concerns) | Low | High | Keep informed |

The mayor's office is managed closely as the political owner of the benefits; the utilities — whose
cooperation the sensor feeds depend on but whose day-to-day interest is limited — are kept
satisfied; and community groups concerned about camera data are kept informed through a deliberate
engagement plan, because a low-power stakeholder with high interest can become a high-power one if
ignored.

Crucially, the **success criteria** (8.1.4) are written in two registers from day one: *project*
success (deliver the centre within the envelope and schedule) and *benefit* success (the measured
response-time and energy improvements the business case promises). The benefit measures are given
owners and baselines — current response times, current energy consumption — because a benefit that
is not baselined at initiation cannot be demonstrated after closure. This distinction, set here,
returns at the end of the case.

### Planning (KA 8.2)

The **WBS** (8.2.1) splits the scope under the 100 % rule into three elements:

```
1.  Building refurbishment
2.  Platform & integration
3.  Commissioning & transition
```

Each element takes the baseline that fits its nature:

| WBS element | Approach | Baseline |
|---|---|---|
| 1. Building refurbishment | Predictive | `BAC` **USD 30,000,000**, milestone earning rules |
| 2. Platform & integration | Adaptive | **24 two-week Sprints × USD 250,000/Sprint = USD 6,000,000** |
| 3. Commissioning & transition | Predictive | **USD 4,000,000** |
| **Programme envelope** | Hybrid | **USD 40,000,000** |

The planning decision that defines this case is that **two measurement systems are designed in from
the start** — the tailoring decision of KA 8.6.4 made explicit, at planning, not improvised later.
The civils element carries a fixed, time-phased cost baseline measured by earned value with
milestone earning rules (Domain 6). The platform is funded by **capacity** — twenty-four two-week
Sprints at USD 250,000 per Sprint — and measured by **velocity and AgileEVM** against an evolving,
MoSCoW-prioritised backlog (Domain 9). Neither system is forced onto the other's scope: earned value
against a fixed software baseline would be fiction, and velocity against a brickwork programme would
be noise. Both instead report into a single **programme-level reconciliation** that expresses each
element's position in money and time, so the gates can compare like with like. The subsidiary plans
(8.2.3) follow: the risk plan feeds contingency, the procurement plan drives the civils packages and
their commitments (Domains 5, 7), and the communications plan commits the reporting cadence the
stakeholder grid demands.

### Executing and controlling to Gate 2 (KAs 8.3–8.4)

Executing (8.3) generates the data control runs on. The civils contractor progresses against
milestones, with cost committed at order placement and accrued as work is done (Domain 5); the
platform team delivers working software Sprint by Sprint, with the Product Owner continuously
re-ordering the backlog; quality assurance runs in both registers — inspection-and-test plans on the
civils, a definition of done and automated testing on the platform. Resource signals are watched as
leading indicators: a slow-mobilising façade subcontractor and a platform velocity settling below
plan both show up in the data well before the gate.

**Gate 2**, at mid-programme, is monitoring & controlling (8.4) in governance form: measurement
against the baselines, a forecast, and a decision that changes the trajectory. The civils element
reports `CPI` 0.97 and `SPI` 0.95 — broadly healthy, with one façade package on watch as the source
of most of the variance. The platform's position needs a re-forecast:

**Re-forecasting the platform at Gate 2.**

1. **Setup.** The platform has completed 12 of its 24 funded Sprints at an achieved velocity of
   **32 points/Sprint against a plan of 40** — 384 points delivered (12 × 32) of a 928-point
   backlog, leaving 544 points with 12 funded Sprints remaining.
2. **Formula.** `Sprints needed = remaining backlog ÷ achieved velocity`; `funding gap = extra
   Sprints × cost per Sprint`.
3. **Substitution.** `Sprints needed = 544 ÷ 32 = 17`; `extra Sprints = 17 − 12 = 5`; `funding gap
   = 5 × 250,000 = 1,250,000`.
4. **Result.** At the current rate the remaining backlog needs **+5 Sprints ≈ +USD 1,250,000**
   beyond the funded 24 — or a descope of the lowest-priority backlog to fit the funded capacity.
5. **Interpretation.** This is the adaptive trade in its purest form: capacity is fixed by funding,
   so the honest flex is scope — the **inverted triangle** of Domain 9 (KA 9.3.5).

The gate pack integrates both elements into one programme position:

| Element | Baseline | Position at Gate 2 | Unmitigated forecast | Gate action |
|---|---|---|---|---|
| 1. Building refurbishment (predictive) | `BAC` USD 30,000,000 | `CPI` 0.97, `SPI` 0.95; façade package on watch | `EAC` ≈ USD 30,900,000 (`BAC`/`CPI`) | Recovery plan on the façade package; target return to baseline |
| 2. Platform & integration (adaptive) | 24 Sprints = USD 6,000,000 | Velocity 32 points/Sprint vs 40 planned | 29 Sprints ≈ USD 7,250,000 | Descope 'Could'-priority backlog to the funded 24 Sprints |
| 3. Commissioning & transition | USD 4,000,000 | Not started; plan revalidated | USD 4,000,000 | Proceed as planned |
| **Programme** | **USD 40,000,000** | Recoverable pressure on two elements | ≈ USD 42,150,000 unmitigated | **Hold the envelope; proceed to Gate 3 with conditions** |

**The gate decision.** The board holds the funding envelope at USD 40,000,000. On the platform, the
Product Owner descopes **'Could'-priority backlog** so the remaining 'Must' and 'Should' scope fits
the funded 24 Sprints at the achieved velocity — fixed capacity, flexed scope, exactly as the
inverted triangle prescribes. On the civils, the façade package gets a formal **recovery action**
with a tightened surveillance regime. The programme proceeds to Gate 3 **with explicit conditions**:
velocity to be re-reviewed after three further Sprints, and façade recovery to be evidenced in the
next two periods.

**Integrated change control on the descope (8.4.2).** The descope is a change like any other, and it
passes through integrated change control *before* approval: its impact is assessed across **scope,
schedule, cost — and benefit**. The assessment re-checks the business case itself: the 'Could' items
proposed for removal (secondary analytics dashboards and a deferred citizen-portal release) do not
touch the sensor-fusion and dispatch features on which the **incident-response benefit** depends, so
the benefit case survives the descope intact. Had the cut reached into scope carrying the benefit,
the honest gate answer would have been to fund the five extra Sprints — a visible breach of the
envelope being cheaper than a silently broken business case. The change log records the decision and
every affected baseline is updated coherently (Domain 5, KA 5.4).

### Closing and benefits (KA 8.5)

At handover the centre goes live and the programme closes in an orderly sequence. The **final
account** settles the civils element (Domain 7): final remeasurement, agreed variations — including
the façade recovery works — and retention released on the defects-period schedule, tracked to zero
by the controls function. The platform closes by demonstrating its final increment against the
definition of done; the residual 'Could' backlog transfers to the operating organisation's product
roadmap rather than lingering as an unclosed liability. **Lessons learned** (8.5.3) capture the
quantitative record on both sides of the hybrid: the platform's Sprint-by-Sprint velocity history
and the civils productivity rates and unit costs both feed the city's estimating library for the
next programme (Domain 3) — and, increasingly, the datasets its forecasting models train on
(Domain 13).

The closure report then does the thing this case exists to show. It reports **project success** and
**benefit success** separately. Project success is demonstrable at closure: delivered on time,
within the USD 40,000,000 envelope after the descope. Benefit success is *not yet knowable*:
response times and energy consumption must be measured over the **first year of operation** against
the baselines set at initiation. The programme closes; **benefit realisation is tracked by the
operating organisation** against the business case, with the benefit owners named at initiation
reporting the measured improvements to the city. The distinction set at 8.1.4 closes the loop: what
was defined on day one is exactly what is measured after the last day.

### What the credential expects

This case is Domain 8 in a single pass, and the exam expects the links to be explicit. The business
case defines **benefit measures** and the charter authorises against them (8.1) — success has two
registers from the start. Planning turns the charter's envelope into a WBS with **dual baselines**,
each carrying the measurement system that fits its development approach (8.2), the tailoring
decision of 8.6.4 made at planning rather than discovered in crisis. The gate is **monitoring &
controlling in governance form** (8.4): earned value on the predictive element, a velocity
re-forecast on the adaptive one, reconciled into one programme position and one decision. The
descope passes through **integrated change control** (8.4.2), assessed across scope, schedule, cost
*and benefit* before approval. Closure settles the commercial position, captures the performance
data, and reports the two successes separately, handing benefit tracking to operations (8.5). The
whole is **hybrid delivery under stage-gate governance** (8.6.3): predictive and adaptive are not
rivals but tailored fits to their scope, measured each in its own register and governed together.
Throughout, AI assists — assembling the gate pack, reconciling the two measurement systems, flagging
the velocity trend early — but the gate decision is human and accountable (Domain 13, KA 13.5):
**AI proposes, the professional disposes.**

---

## Case study B — Domain 8: a hospital programme at Gate 3 (healthcare construction)

### Background

A regional health authority is building a new **acute hospital** to replace a century-old estate: clinical
wards, theatres, emergency department, imaging and the supporting energy centre. The business case was
approved with an original baseline of **USD 460,000,000**, and its benefits were written as measures, not
aspirations — chief among them a **ward-capacity benefit**: the new estate adds **120 acute beds**, which at
the authority's planned occupancy and average length of stay translates to **6,000 additional admissions per
year** (50 admissions per bed per year), baselined at initiation against the current estate's throughput
(KA 8.1.4).

The programme is mid-delivery, approaching **Gate 3** — the gate that releases the fit-out and
medical-equipment tranche, the largest remaining commitment. The gate board has adopted the discipline of
Advanced 8.A.3: evidence, not narrative, with the pack examined against the checklist of Toolkit 8.T.1. This
case follows the gate itself — what passes, what fails, and what the failures set in motion — because a gate
review is monitoring & controlling in governance form (KA 8.4), and a checklist only matters when it is
*used* rather than performed.

### The evidence pack, item by item (Toolkit 8.T.1)

| Checklist item (8.T.1) | Evidence offered | Verdict |
|---|---|---|
| Estimate class matched to commitment | Remaining works estimated at **Class 3**, basis of estimate documented (3.2.3) | **Pass** |
| Estimate as a range, not a point | **−10 %/+15 %** band stated and carried into the funding decision | **Pass** |
| Risk exposure vs contingency | QRA exposure **USD 42,000,000 (P80)** vs remaining contingency **USD 36,000,000** | **Fail — USD 6,000,000 gap** |
| Benefit case re-tested at current forecast | Ward-capacity benefit has **drifted** (below); other benefits intact | **Fail — condition** |
| Named benefit owners with baselined measures | Two of three benefits owned; the **capacity benefit's owner was lost** in a clinical reorganisation | **Fail — condition** |
| Baselines integrated | Cost phased over a schedule built from the WBS (8.2.2) | Pass |
| Change log reconciled | `460,000,000 → 471,300,000` through **14 authorised changes**, each traceable (5.4.3) | Pass |
| `EAC` with method and assumption | **USD 474,500,000** — bottom-up remaining work, cross-checked against `CPI` (6.3.3); `VAC` **(3,200,000)** | Pass |
| Prior-gate conditions closed | Gate 2 conditions evidenced and signed off | Pass |
| Stop/descope options genuinely open | Pack presents the bed-restoration options both ways (below) | Pass |

The risk item fails on arithmetic, not on narrative: a QRA refreshed for market escalation on the mechanical
and electrical packages puts remaining exposure at 42,000,000 against 36,000,000 of contingency — a
**USD 6,000,000** shortfall at the stated confidence. The board's response is the checklist working as
designed: the gate **passes with conditions** — a contingency-replenishment or descope plan within one
reporting period, and the two benefit items below — each condition dated, owned, and tracked to closure at
Gate 4, because a condition that is not tracked is decoration (Advanced 8.A.3).

### The drifting ward-capacity benefit (Advanced 8.A.2, KA 8.1.4)

The benefit re-test — the 8.T.1 item that examines the business case at the *current* forecast, not the
original — finds the capacity benefit quietly eroded by design development:

1. **Setup.** Business-case benefit: **+120 beds → 6,000 admissions/year** (50 admissions per bed-year).
   The approved clinical change CR-041 (below) reduced the design to **+104 beds**.
2. **Formula.** `Benefit now = beds now × admissions per bed-year`; `drift = planned benefit − benefit now`.
3. **Substitution.** `104 × 50 = 5,200`; `6,000 − 5,200 = 800`.
4. **Result.** The forecast benefit is **5,200 admissions/year** — a drift of **800 admissions/year**,
   **13.3 %** of the headline benefit the investment was approved to buy.
5. **Interpretation.** Nothing about the drift was hidden; it simply had no owner watching it — the benefit's
   named owner had been lost in a reorganisation, and a benefit nobody owns is a forecast nobody checks
   (Advanced 8.A.2). The gate catches it only because the checklist forces the re-test. The conditions
   attached are exactly 8.A.2's remedy: a **benefit owner re-appointed in the operating organisation** (the
   authority's director of operations, not the project), and the restoration decision below taken against
   the benefit's value rather than inside the project team.

### Integrated change control — the clinical change's knock-ons (KA 8.4.2)

CR-041, processed two months before the gate, is why the bed count moved. An infection-control review —
prompted by updated national guidance — required the proportion of **single rooms to rise from 50 % to
70 %**. The change is clinically mandatory, but integrated change control did its work *before* approval,
assessing the knock-ons across every baseline **and the benefit case**:

| Impact dimension | Assessment |
|---|---|
| Scope/cost — ward reconfiguration | **USD 6,800,000** |
| Knock-on — HVAC uprating (air-change rates, en-suite extracts) | **USD 1,900,000** |
| Schedule — fit-out prolongation, two months, with time-related cost | **USD 900,000** |
| **CR-041 total** | **USD 9,600,000** |
| Benefit — bed count | **−16 beds → −800 admissions/year** |

The USD 9,600,000 was authorised from programme reserve and is the largest of the fourteen changes in the
reconciled log (`460.0m + 9.6m + 1.7m of thirteen smaller changes = 471.3m`). The benefit line is the one an
undisciplined change process omits: a change assessed only on cost and schedule would have sailed through
approval with the business case silently holed. Because the benefit impact was quantified at assessment, the
gate pack could price the **restoration options** honestly: accept the −16 beds (benefit down 800
admissions/year), or add a shelled ward floor at **USD 28,000,000** — `28,000,000 / 16 = ` **USD 1,750,000
per restored bed** — a figure the authority can now weigh against what an admission is worth, at board level,
where that judgement belongs. The gate's condition is that the decision be taken within three months, by the
benefit owner and the board — not defaulted by the passage of time.

### Setting up closure years early (KA 8.5, Toolkit 8.T.2)

The final Gate 3 artefact looks far ahead: a **closure and transition plan**, drafted while the fit-out
tranche is only now being released. Hospitals punish improvised handover — a building can be finished while
the *hospital* is not — so the plan establishes now what 8.T.2 will demand at the end: **clinical
commissioning as its own WBS element** (staff training, equipment qualification, the phased migration of
live patients from the old estate); the benefit **measurement plan wired into the authority's routine
reporting** — admissions, occupancy and length-of-stay feeds agreed with the re-appointed owner — so
realisation tracking survives the project team's disbandment; **retention and defects-period tracking**
assigned to the estates function with release dates diarised (Domain 7, KA 7.2.4); the **data harvest**
committed in advance — cost per bed, rates per m², actual `CPI` by package — into the estimating library
(Domain 3); and the **post-opening benefits review diarised at twelve months** with a named chair. None of
this is closure brought forward as bureaucracy; it is the recognition that everything on that list is cheap
to arrange now and near-impossible to reconstruct after the budget line closes (KA 8.5.3).

### What the credential expects

This case examines the governance half of Domain 8, and the credential expects the candidate to run it as
evidence-work, not ceremony. From **Toolkit 8.T.1 and Advanced 8.A.3**, a gate pack examined item by item —
estimate class matched to the commitment, risk exposure set against contingency at a stated confidence, the
change log reconciling 460.0m to 471.3m — with the failures producing dated, owned, tracked conditions
rather than a softened narrative. From **KA 8.1.4 and Advanced 8.A.2**, benefits realisation as arithmetic:
a benefit baselined at initiation (6,000 admissions/year), re-tested at the current forecast, its 13.3 %
drift caught mid-programme while restoration is still buyable, and its ownership repaired in the operating
organisation. From **KA 8.4.2**, integrated change control assessing a clinically unavoidable change across
scope, schedule, cost *and benefit* — the fourth dimension being the one that preserved the business case's
honesty. And from **KA 8.5**, closure set up years early, because handover of a hospital is a project inside
the project. AI can assemble the pack, reconcile the log and flag the benefit drift from the design data long
before a human reviewer would (Domain 13, KA 13.5) — but the gate decision, and the accountability for it,
stay human. **AI proposes, the professional disposes.**

---

## Executive perspective — Domain 8

**What the executive must hold onto.** The baseline is set in **planning** and only measured afterwards —
by the time monitoring & controlling reports a variance, the decisions that caused it were taken months
earlier, so governance attention belongs early, where it is cheapest (KA 8.2). **Integrated change
control** is where scope creep dies or thrives: every change assessed across scope, schedule, cost,
quality and risk before approval, or approved for its direct cost while its knock-ons arrive unpriced
(KA 8.4). And success has two registers — delivered to plan and benefit realised — a project can hit time
and budget and still fail the business case it was authorised against (KA 8.1).

**Six questions to ask from the chair.**

1. What benefit does the business case promise, who owns measuring it, and against which success criteria —
   beyond time and cost?
2. Are the three baselines genuinely integrated — does the cost baseline phase over a schedule built from
   the WBS, or do we hold three documents that cannot support earned value?
3. For each change approved this period, was the full cross-constraint impact — including schedule, risk
   and benefit — assessed before approval, not after?
4. Who is the single accountable owner — the one "A" — for this report, this decision, this deliverable?
5. Which parts of the programme are predictive and which adaptive, and is each measured in the register
   that fits it — with the tailoring decision made at planning, not discovered in crisis?
6. What is still open from closing — unresolved claims, uncollected retention, unharvested lessons — and
   who is tracking each to zero?

**The traps at board level.**

- **Attention arriving too late.** Boards engage hardest in execution, when the levers are fewest; the
  charter, the WBS and the integrated baselines are where the outcome was largely set.
- **Change approved by instalments.** A dozen individually reasonable changes, each assessed only for
  direct cost, accumulate into an unpriced re-scope — the failure integrated change control exists to
  prevent.
- **Project success mistaken for benefit success.** "On time, on budget" answers the delivery question,
  not the investment question; the benefit measures defined at initiation are what close that loop.
- **Closure allowed to drift.** Unclosed contracts, unresolved claims and unreleased retention are
  liabilities that linger, and lessons never captured are paid for again on the next project (KA 8.5).

**What good looks like.** The organisation invests its scrutiny where the leverage is: business cases with
measurable benefits, charters that authorise against them, and planning that produces one integrated
system rather than a shelf of documents. Monitoring & controlling runs the same loop every period —
measure, analyse, forecast, act, report — and changes pass through one gate with their full impact priced.
Closure is treated as real work: final accounts settled, retention collected, performance data fed to the
next estimate, and benefits handed to named owners in operations. Such organisations can say, years later,
whether the project was worth doing — because they defined the answer before they started.

---

## Practitioner's toolkit — Domain 8

*Adoption-ready artefacts; adapt the column headings and thresholds to your organisation, then keep them
stable.*

### Toolkit 8.T.1 — Gate evidence pack checklist

A gate is a decision to keep investing, and a good one demands evidence, not narrative (Advanced 8.A.3) —
every item below is an artefact, not an adjective.

- [ ] Estimate class stated (Domain 3, KA 3.2.1) and matched to the commitment this gate makes — sanction demands Class 3 or better.
- [ ] Estimate presented as a range with its accuracy band and documented basis of estimate (3.2.3) — never a point.
- [ ] Quantified risk exposure (Domain 12) set against remaining contingency at a stated confidence level (12.3.1, 12.3.3).
- [ ] Benefit case re-tested at the *current* forecast, not the original (Advanced 8.A.2).
- [ ] Every benefit carries a named owner in the operating organisation, a baselined measure and a measurement date (8.1.4, 8.A.2).
- [ ] The three baselines genuinely integrated — cost phased over a schedule built from the WBS (8.2.2).
- [ ] Change log reconciled — the current baseline traceable to the original through authorised change only (Domain 5, KA 5.4.3).
- [ ] Forecast (`EAC`) stated with method and assumption (Domain 6, KA 6.3.3); hybrid elements each measured in their own register and reconciled (8.6.4).
- [ ] Conditions and lessons from the prior gate closed out with evidence — not carried as decoration (8.A.3).
- [ ] The stop/descope/recycle options genuinely open, with the pack presenting evidence either way — not curated to pass.

**Usage note.** The checklist grades with the gate: an early concept gate can properly proceed on a
Class 5/4 estimate because the commitment is small and the range honest, while the sanction gate — which
commits the money — fails on the same evidence however eloquent the pack (Advanced 8.A.3). The benefit
items keep the two registers of success separate from initiation onwards (8.1.4), and the change-log and
baseline items are what let the gate compare the current position to what was last authorised (8.2.2,
5.4.3). Apply the diagnostic question to the process itself: if no gate has stopped, descoped or recycled a
project in living memory, the checklist is being performed, not used.

### Toolkit 8.T.2 — Project closure checklist

Sequenced from KA 8.5 — closing is real work, and every unchecked box is a liability that lingers.

- [ ] Deliverables formally accepted and the acceptance recorded (8.5.1).
- [ ] Final account agreed — final remeasurement, approved variations and claim settlements (8.5.2; Domain 7, KAs 7.3–7.4).
- [ ] Retention release schedule diarised and tracked to zero — including the tranche due after the defects period (8.5.2; Domain 7, KA 7.2.4).
- [ ] All contracts and subcontracts formally closed; no open commitments left stale (8.5.1; Domain 5, KA 5.2.4).
- [ ] Bonds and guarantees released, or their expiry and release conditions logged and chased (Domain 7, KA 7.2.4).
- [ ] Warranties and defects-period obligations logged with owners and dates.
- [ ] Handover documentation complete and transferred to the operator/client (8.5.2).
- [ ] Records archived — cost ledger, change log, contemporaneous records, schedule history (8.5.1).
- [ ] Performance data — actual `CPI`, productivity rates, unit costs — fed into the estimating library (8.5.3; Domain 3, KA 3.2).
- [ ] Lessons learned captured in both registers, quantitative and qualitative, with the controls function as custodian of the numbers (8.5.3).
- [ ] Benefits measurement plan handed live to the named benefit owners in operations (Advanced 8.A.2).
- [ ] Team released in an orderly demobilisation, and the post-implementation review scheduled with a date and owner.

**Usage note.** The sequence matters: acceptance before the final account, the account before contract
closure, and the data harvest before the team that holds the knowledge disperses — because at closure the
budget line closes and whatever is not captured now is relearned at the next project's expense (8.5.3).
The retention and bond items are real receivables and real fees, tracked to zero rather than assumed away
(worked example 8.5.2; Domain 7, KA 7.A.3). The benefits item is the loop-closer: project success is
demonstrable at closure, benefit success only afterwards, and the handover to a named owner in the
operating organisation is what makes the second register measurable at all (8.1.4, 8.A.2).

---

## Exam preparation — Domain 8

**How this domain is examined.** Domain 8 is the most concept-heavy domain in the PM group: the balance
tilts towards recall (process groups, charter versus business case, artefact vocabulary) and analysis (which
principle a described governance failure violates), with a lighter but real numerical strand — `PV` phasing
(KA 8.2), the final-account and retention arithmetic of closing (KA 8.5), and adaptive funding-gap
calculations at the Domain 9 boundary (KA 8.6). Scenario stems often describe a defect and ask what it is,
so precise vocabulary — **integrated change control**, the **100 % rule**, the two registers of success —
earns direct marks.

**Calculation traps.**

- **Charter versus business case.** The business case *justifies*; the charter *authorises* — distractors
  swap them or offer planning artefacts such as the WBS (MCQ 8.1-A).
- **A RACI with two accountables.** Every activity carries exactly one "A"; the fix is to reduce to one, not
  to share the accountability or convert it to "R"s (MCQ 8.1-D).
- **Treating a management-reserve release as a variance.** Releasing reserve into the baseline is a formal,
  logged re-baseline through change control — `BAC` rises — not an adverse variance and never a quiet
  adjustment (KA 8.4.2; Domain 5, KA 5.4.3).
- **Retention on the wrong final-account base.** The retention base is the *full* final account — original
  value plus variations plus agreed claims — and half has usually already been released at practical
  completion (MCQ 8.5-B's distractors miss one or the other).
- **Funding-gap denominators.** Compare the Sprints *needed* with the Sprints *remaining* in the funding,
  not with the total originally funded, and do not net off completed Sprints twice (MCQ 8.6-E).
- **Project success mistaken for benefit success.** On time and on budget answers the delivery question
  only; distractors declare "total success" from the iron triangle alone (MCQ 8.1-C).

**Time management.** Recall items here should take well under a minute each — the domain rewards vocabulary
precision more than computation. Spend the saved time on the scenario-analysis stems, where a single phrase
("approved on its direct cost alone") carries the answer, and on checking the base of any retention or
funding-gap calculation before multiplying.

**Reflection questions.**

1. Could you trace your current project's baseline back to its charter through authorised change only — and
   how long would that take?
2. Which changes on your project were approved for direct cost alone, with their schedule, risk or benefit
   impacts arriving unpriced later?
3. Who owns measuring each benefit in your business case after the project team disbands, and were the
   baseline measures taken at initiation?
4. When did a gate in your organisation last stop, descope or recycle a project — and if never, what does
   that say about the gates?

---

## Domain 8 summary

The project lifecycle runs through five process groups: **initiating** (business case, charter, stakeholders,
success criteria), **planning** (scope/WBS and the three integrated baselines plus subsidiary plans),
**executing** (performing the work and generating the data), **monitoring & controlling** (measuring against
the baselines and correcting — the home of project controls, via earned value, variance and integrated change
control), and **closing** (contract closure, final account, lessons learned). How the lifecycle is *applied*
varies along the **predictive-to-adaptive spectrum**, with **incremental** (adding parts) and **iterative**
(refining the whole) as distinct ideas that agile combines, and **hybrid** delivery the norm on large
programmes. The controls professional's work sits inside this lifecycle — setting the baseline in planning,
measuring it in controlling, and capturing its lessons in closing — and must be tailored to the development
approach the project adopts.

**Cross-references.** WBS/cost coding → 1.5; the baselines and reserves → 3.1; the PV baseline → 3.3; earned
value in controlling → Domain 6; variance and reporting → Domain 4; integrated change control → 5.4; the final
account → 7.3–7.4; adaptive delivery and AgileEVM → Domain 9; risk in planning/controlling → Domain 12;
AI across the lifecycle → 13.5.

