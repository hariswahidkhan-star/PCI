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
- B. Project charter. ✅
- C. Work breakdown structure.
- D. Cost baseline.

*Rationale:* The charter authorises the project and PM. The business case justifies it; the WBS and cost
baseline are planning artefacts.

**MCQ 8.1-B `[8.1.3 · Application]`** A high-power, low-interest regulator should be:
- A. Managed closely.
- B. Kept satisfied. ✅
- C. Merely monitored.
- D. Ignored.

*Rationale:* On the power/interest grid, high power + low interest = "keep satisfied". "Manage closely" is for
high power + high interest; "monitor" is low/low; ignoring a powerful stakeholder is never appropriate.

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
- B. Captures 100 % of the scope — no more, no less. ✅
- C. Guarantees 100 % on-time delivery.
- D. Requires 100 % resource loading.

*Rationale:* The 100 % rule concerns scope completeness and non-overlap. It is not about pre-start completion,
delivery certainty, or resource loading.

**MCQ 8.2-B `[8.2.2 · Analysis]`** Why must the cost baseline be phased over the schedule?
- A. To reduce the BAC.
- B. To produce Planned Value for earned-value measurement. ✅
- C. To satisfy IFRS 15.
- D. It need not be.

*Rationale:* Phasing the cost baseline over the schedule creates `PV` (Domain 3, KA 3.3), the basis of earned
value. It does not change `BAC`, is not an IFRS requirement, and is essential (not optional) for EVM.

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
- B. Integrated change control. ✅
- C. Earned value.
- D. Going concern.

*Rationale:* Integrated change control requires assessing a change across *all* constraints. The 100 % rule is
about WBS scope; earned value and going concern are unrelated to this failure.

**MCQ 8.4-B `[8.4.1 · Recall]`** Monitoring & controlling primarily exists to:
- A. Observe and document the project.
- B. Measure against the baselines and act to change the trajectory. ✅
- C. Replace executing.
- D. Produce the charter.

*Rationale:* Its purpose is corrective — to change the outcome, not merely observe. It runs alongside (not
instead of) executing, and the charter is an initiating artefact.

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
- B. It holds the performance data (actual CPI, productivity, unit costs) that feeds future estimates and models. ✅
- C. It approves the charter.
- D. It runs procurement.

*Rationale:* Controls holds the earned-value and cost performance data that becomes future estimating and
model-training input. Contracts, charter and procurement are other functions.

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
- B. Incremental delivery. ✅
- C. Predictive delivery.
- D. Waterfall.

*Rationale:* Incremental delivery adds new working parts (slices) to the whole. Iterative refines the *same*
product; predictive/waterfall delivers the full scope in sequence.

**MCQ 8.6-B `[8.6.2 · Analysis]`** Refining the *same* product over repeated passes, improving it each time,
describes:
- A. Incremental delivery.
- B. Iterative delivery. ✅
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

### Self-check — KA 8.6

1. State the incremental-vs-iterative distinction in one sentence each. *(Incremental — add new working parts
   to the whole; iterative — refine the same product over passes.)*
2. Why does the development approach matter to the controls professional? *(It determines how progress and cost
   are measured — fixed baseline vs velocity vs both.)*

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

*Domain 8 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
