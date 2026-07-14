# Domain 9 — Agile, Scrum & Adaptive Delivery for Project Controls

> **Group:** Project management. **Target:** ~90 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). Authored **after** Domain 6 so KA 9.5 reuses the
> EVM symbols (`EV`, `AC`, `PV`, `BAC`, `CPI`, `SPI`, `EAC`) unchanged. British English; USD (+SAR where
> useful). The Agile Manifesto and Scrum Guide are described in this reference's **own words**, never
> reproduced (Spine §9).

## Why this domain exists

Modern project controls must measure, forecast and report on **adaptive and hybrid** delivery, not only
predictive delivery. Increasingly, the software, systems and design elements of large programmes are run in
short, feedback-driven cycles where scope evolves — and a controls professional who can only measure a fixed
baseline is blind to half the work. This domain builds the agile knowledge a controls professional needs:
agile foundations and the empirical mindset (KA 9.1); the **Scrum framework** in depth (KA 9.2); backlogs,
estimation and **agile metrics** — velocity, burndown/burnup, flow (KA 9.3); **Kanban, Lean and scaling**
(KA 9.4); and the crux — **agile cost control, forecasting and earned value (AgileEVM)**, reconciled to the
finance and EVM domains (KA 9.5); closing with **hybrid delivery and agile governance** (KA 9.6). Throughout,
the emphasis is the controls professional's question: *how do I plan, measure cost/progress, forecast and
report in an agile or hybrid environment, and reconcile it with the EVM and IFRS 15 machinery of the finance
domains?*

**Learning objectives.** After this domain a candidate can: explain the agile mindset and empirical process
control and judge agile suitability; describe Scrum's accountabilities, events, artefacts and commitments;
compute and interpret velocity, burndown/burnup and flow metrics and forecast a release; describe Kanban, Lean
and scaling at the right level; apply **AgileEVM** (`EV`/`CPI`/`SPI`/`EAC` on variable scope) and reconcile
story-point progress to `%` complete and IFRS 15 revenue; and design a controls/reporting cadence for a hybrid
programme.

---

## Knowledge Area 9.1 — Agile foundations

*Topics: 9.1.1 the agile mindset and the Manifesto · 9.1.2 empirical process control · 9.1.3 adaptive vs
predictive planning · 9.1.4 when agile is (and isn't) appropriate.*

### 9.1.1 The agile mindset and the Manifesto

**Definition & purpose.** Agile is a **mindset** — favouring working outcomes, collaboration, customer value
and responsiveness to change over rigid, up-front plans — expressed in the **Agile Manifesto** (four value
statements and twelve supporting principles). Described in this reference's own words: agile values **working
product over comprehensive documentation, collaboration over contract negotiation, individuals and interactions
over process and tools, and responding to change over following a plan** — while recognising the items on the
right still have value. The controls consequence is profound: if scope is expected to *change*, a controls
system that treats change as failure (variance against a fixed baseline) fights the delivery model. Agile
controls measure **flow and value delivered**, not adherence to a fixed plan.

### 9.1.2 Empirical process control

**The principle.** Agile rests on **empiricism** — decisions are made on what is **observed**, not on a
predicted plan — operationalised through three pillars: **transparency** (make the real state visible),
**inspection** (frequently examine the work and progress), and **adaptation** (adjust as soon as the inspection
shows a need). Short cycles exist precisely to create frequent inspect-and-adapt points. For a controls
professional this is congenial: empirical process control *is* measurement-driven management — the same instinct
as earned value, applied to evolving scope.

### 9.1.3 Adaptive vs predictive planning

**The principle.** **Predictive** planning fixes scope and plans it in detail up front (Domain 8); **adaptive**
planning fixes **time and cost** (a cadence of funded iterations) and lets **scope flex** to fit — the
**inverted iron triangle** (9.3). Adaptive planning is **rolling-wave** in the extreme: the next iteration is
planned in detail, later ones only in outline. This changes the forecasting question from "will we deliver the
fixed scope on time?" to "how much of the valued scope will we deliver by the fixed date, at the current run
rate?" (9.5).

### 9.1.4 When agile is (and isn't) appropriate

**The principle.** Agile suits work with **uncertain or evolving requirements**, a high change rate, and the
ability to deliver value incrementally (software, product design, R&D). It is **less** suited to work with
**stable, well-understood requirements**, heavy regulatory/sequential constraints, or where a partial increment
has no value (a bridge half-built). Most large programmes are therefore **hybrid** (Domain 8, KA 8.6; KA 9.6).
Judging suitability — rather than adopting agile as dogma — is the professional stance.

**Worked example 9.1.4 — map suitability.** For Northwind (Domain 8): the **control software** has volatile
requirements and delivers value in increments → **agile-suited**; the **power/cooling civils** are defined and
regulated and have no value half-built → **predictive-suited**. The programme is hybrid.

### Key terms — KA 9.1

| Term | Meaning |
|---|---|
| **Agile mindset / Manifesto** | Valuing working outcomes, collaboration, value and responsiveness to change. |
| **Empirical process control** | Decisions from observation via transparency, inspection, adaptation. |
| **Adaptive vs predictive planning** | Fix time/cost, flex scope vs fix scope, plan up front. |
| **Rolling wave** | Detailed near-term planning, outline further out. |

### Sample MCQs — KA 9.1

**MCQ 9.1-A `[9.1.2 · Recall]`** The three pillars of empirical process control are:
- A. Plan, do, check.
- B. Transparency, inspection, adaptation. ✅
- C. Scope, time, cost.
- D. People, process, tools.

*Rationale:* Empiricism rests on transparency, inspection and adaptation. The others are a cycle, the iron
triangle, and a resourcing triad respectively.

**MCQ 9.1-B `[9.1.4 · Analysis]`** Which work is *least* suited to a purely adaptive approach?
- A. A product with evolving requirements.
- B. Software delivered in valuable increments.
- C. A regulated civil structure with no value until complete. ✅
- D. An R&D prototype.

*Rationale:* Work with stable/regulated requirements and no value in a partial increment fits predictive
delivery. The others have uncertainty and incremental value, suiting adaptive delivery.

### Self-check — KA 9.1

1. Why does a fixed-baseline controls system fight an agile delivery? *(Agile expects scope to change; treating
   change as variance against a fixed baseline misreads healthy adaptation as failure.)*
2. What does adaptive planning fix and flex? *(Fix time and cost; flex scope — the inverted triangle.)*

---

## Knowledge Area 9.2 — The Scrum framework in depth

*Topics: 9.2.1 Scrum theory and pillars · 9.2.2 the three accountabilities · 9.2.3 the five events · 9.2.4 the
three artefacts and their commitments.*

### 9.2.1 Scrum theory

**Definition & purpose.** **Scrum** is a lightweight framework for delivering value adaptively through
**empiricism** (9.1.2) and **lean thinking**, in short cycles called **Sprints**. It is deliberately minimal —
a few accountabilities, events and artefacts — and everything in it exists to create transparency and frequent
inspect-and-adapt points. (Described from the current Scrum Guide's concepts, in this reference's own words.)

### 9.2.2 The three accountabilities

- **Product Owner** — accountable for **maximising the value** of the product; owns and orders the **Product
  Backlog**; the single voice of "what and why."
- **Scrum Master** — accountable for the team's **effectiveness** and for the framework being understood and
  enacted; a coach and impediment-remover, not a project manager over the team.
- **Developers** — the professionals who do the work of creating a usable **Increment** each Sprint;
  accountable for the plan (Sprint Backlog), quality (Definition of Done) and the daily work.

*(Note the current terminology: "**Developers**", not "development team"; three **accountabilities**, not
"roles".)*

### 9.2.3 The five events

All events are **time-boxed** and occur within the container event, the Sprint:

- **The Sprint** — the container, a fixed short period (commonly two weeks) producing a usable Increment; a new
  Sprint starts immediately after the previous one.
- **Sprint Planning** — starts the Sprint; the team agrees **why** (the Sprint Goal), **what** (backlog items
  selected) and **how** (a plan).
- **Daily Scrum** — a short daily synchronisation for the Developers to inspect progress toward the Sprint Goal
  and adapt the plan.
- **Sprint Review** — near the end; inspect the Increment with stakeholders and adapt the Product Backlog.
- **Sprint Retrospective** — closes the Sprint; inspect how the team worked and plan improvements.

### 9.2.4 The three artefacts and their commitments

Each artefact has a **commitment** that gives it transparency and focus:

| Artefact | What it is | Commitment |
|---|---|---|
| **Product Backlog** | The ordered, emergent list of what the product needs | **Product Goal** (the long-term objective) |
| **Sprint Backlog** | The Sprint Goal + items selected + the plan to deliver them | **Sprint Goal** (the single Sprint objective) |
| **Increment** | A usable, "Done" step toward the Product Goal | **Definition of Done** (the quality standard for "Done") |

**Worked example 9.2.4 — a Sprint walk-through.** A team on Northwind's monitoring software: **Sprint Planning**
sets a **Sprint Goal** ("operators can view live rack temperatures"); the team pulls backlog items totalling
its capacity into the **Sprint Backlog**; the **Daily Scrum** tracks progress and surfaces a blocker (a sensor
API delay); the **Sprint Review** demonstrates the working temperature view (a "Done" **Increment** meeting the
**Definition of Done**) and adapts the backlog; the **Retrospective** agrees to mock the API earlier next time.
One Sprint, from backlog to usable increment.

### Key terms — KA 9.2

| Term | Meaning |
|---|---|
| **Sprint** | The fixed short cycle containing all other events; produces a usable Increment. |
| **Product Owner / Scrum Master / Developers** | Value / effectiveness / doing-the-work accountabilities. |
| **Product / Sprint Backlog, Increment** | The three artefacts. |
| **Product Goal / Sprint Goal / Definition of Done** | Their respective commitments. |

### Sample MCQs — KA 9.2

**MCQ 9.2-A `[9.2.4 · Recall]`** The commitment associated with the Increment is the:
- A. Sprint Goal.
- B. Product Goal.
- C. Definition of Done. ✅
- D. Velocity.

*Rationale:* Each artefact has a commitment: Product Backlog→Product Goal, Sprint Backlog→Sprint Goal,
Increment→Definition of Done. Velocity is a metric, not a commitment.

**MCQ 9.2-B `[9.2.2 · Recall]`** Accountability for maximising the value of the product belongs to the:
- A. Scrum Master.
- B. Product Owner. ✅
- C. Developers.
- D. Sponsor.

*Rationale:* The Product Owner maximises product value and orders the Product Backlog. The Scrum Master serves
effectiveness; the Developers build the Increment.

### Self-check — KA 9.2

1. Name the three accountabilities and one responsibility of each. *(PO — value/backlog order; SM —
   effectiveness/impediments; Developers — the Increment/plan/quality.)*
2. Match each artefact to its commitment. *(Product Backlog→Product Goal; Sprint Backlog→Sprint Goal;
   Increment→Definition of Done.)*

---

## Knowledge Area 9.3 — Backlogs, estimation and agile metrics

*Topics: 9.3.1 user stories, acceptance criteria and INVEST · 9.3.2 refinement and prioritisation · 9.3.3
relative estimation and velocity · 9.3.4 burndown, burnup and flow metrics · 9.3.5 the inverted iron triangle.*

### 9.3.1 User stories, acceptance criteria and INVEST

**The principle.** Backlog items are often written as **user stories** ("as a <user>, I want <capability> so
that <benefit>") with **acceptance criteria** defining "done" for that story. Good stories follow **INVEST** —
Independent, Negotiable, Valuable, Estimable, Small, Testable. Well-formed stories are what make estimation,
prioritisation and progress measurement possible.

### 9.3.2 Refinement and prioritisation

**The principle.** **Backlog refinement** is the ongoing activity of breaking down, clarifying and estimating
items so they are ready for a Sprint. **Prioritisation** orders the backlog by value/urgency — using techniques
such as **MoSCoW** (Must/Should/Could/Won't) or, conceptually, **WSJF** (weighted shortest job first:
prioritise by cost of delay relative to job size). Prioritisation is what makes the inverted triangle work: when
scope flexes, the *least* valuable items drop first.

### 9.3.3 Relative estimation and velocity

**Definition & purpose.** Agile teams estimate **relatively** — sizing items in **story points** (relative
effort/complexity/uncertainty) rather than hours — often via **planning poker**. **Velocity** is the number of
story points a team completes per Sprint, averaged over recent Sprints; it is an **empirical** capacity
measure, used to forecast (9.5), not a target to inflate.

```
Velocity (per Sprint) = average story points completed over the last N Sprints
Sprints remaining      = remaining backlog points / velocity
```

**Worked example 9.3.3 — forecast a release from velocity.**

1. **Setup.** A team's completed points over five Sprints: 28, 31, 30, 32, 29. Remaining backlog: **240
   points**.
2. **Formula.** `Velocity = average completed`; `sprints remaining = remaining points / velocity`.
3. **Substitution.** `Velocity = (28+31+30+32+29)/5 = 150/5 = 30`; `sprints remaining = 240 / 30 = 8`.
4. **Result.** **Velocity 30 pts/Sprint; ~8 Sprints remaining** (≈ 16 weeks at 2-week Sprints).
5. **Range.** Forecasting a *range* is more honest than a point: at a pessimistic velocity of 25 → `240/25 ≈
   10` Sprints; optimistic 35 → `240/35 ≈ 7` Sprints. Report **7–10 Sprints**, not a false-precise 8.

**Worked example 9.3.3b — a burnup when scope is added mid-release.**

1. **Setup.** A team's velocity is **20 story points per Sprint**; the initial release scope is **200 points**
   (so an original forecast of `200 / 20 = 10` Sprints). By the end of Sprint 4 the team has completed **80
   points**, and **40 points of new scope** are approved and added.
2. **Formula.** `remaining = new total scope − completed`; `Sprints remaining = remaining / velocity`.
3. **Substitution.** New total scope `= 200 + 40 = 240`; remaining `= 240 − 80 = 160`; Sprints remaining
   `= 160 / 20 = 8`.
4. **Result.** The forecast finish moves from Sprint 10 to **Sprint 12** (4 done + 8 remaining) — the burnup's
   rising total-scope line makes this visible; a burndown to zero would have hidden the added scope.
5. **Interpretation.** In adaptive delivery scope change is expected; the burnup shows it honestly, and the
   controls professional reports the moved forecast rather than a false "on track against the original"
   (cross-ref 9.3.4).

### 9.3.4 Burndown, burnup and flow metrics

**The measures.**

- **Sprint burndown** — remaining work in the Sprint plotted down to zero; shows within-Sprint progress.
- **Release burnup** — completed scope rising toward a (possibly moving) total-scope line; unlike a burndown,
  a burnup **shows scope change** (the total line moving) as well as progress — which is why it is preferred
  for release forecasting where scope flexes.
- **Cumulative flow diagram (CFD)** — bands of work in each state (to-do / in-progress / done) over time;
  widening bands reveal bottlenecks and growing WIP.
- **Throughput** (items completed per period) and **cycle time** (time from start to done per item) — flow
  metrics central to Kanban (9.4).

> **Fig 9.3.1 — Release burnup with a moving scope line.** *Caption:* completed scope vs total scope over
> Sprints. *Underlying data:* completed cumulative {30, 61, 91, 123, 152 points} over Sprints 1–5; total-scope
> line rising from 300 to 320 (scope added at Sprint 3). *Render-ready description:* x-axis Sprints, y-axis
> points; a rising "completed" area (brand blue) beneath a "total scope" line (grey) that steps up at Sprint 3;
> the vertical gap is remaining work; a dashed projection of the completed line meets the scope line at the
> forecast release Sprint. *Animation storyboard (digital-only):* the completed area grows each Sprint; at
> Sprint 3 the scope line jumps up (scope added), visibly pushing the forecast finish later — the thing a
> burndown would hide.

### 9.3.5 The inverted iron triangle

**The principle.** In predictive delivery, scope is fixed and time/cost vary; **agile inverts** it — **time
and cost are fixed** (a cadence of funded Sprints) and **scope varies**. This reframes control: you do not ask
"will we deliver everything on time?" but "how much of the prioritised value will we deliver by the fixed date
and budget?" — which is exactly what velocity and burnup forecast, and what AgileEVM measures (9.5).

### Key terms — KA 9.3

| Term | Meaning |
|---|---|
| **User story / acceptance criteria / INVEST** | A value-framed backlog item, its "done" tests, and quality heuristics. |
| **Story point / velocity** | Relative size unit / average points completed per Sprint. |
| **Burndown / burnup** | Remaining work down to zero / completed scope up to a (moving) total. |
| **Cumulative flow / throughput / cycle time** | Work-state bands / items per period / start-to-done time. |
| **Inverted iron triangle** | Fixed time & cost, variable scope. |

### Sample MCQs — KA 9.3

**MCQ 9.3-A `[9.3.3 · Application]`** A team completes 28, 31, 30, 32, 29 points over five Sprints; 240 points
remain. The expected Sprints remaining (at average velocity) is:
- A. 5
- B. 6
- C. 8 ✅
- D. 10

*Rationale:* `velocity = 150/5 = 30`; `240/30 = 8`. A/B/D use the wrong velocity; 10 would be the pessimistic
(velocity 25) bound.

**MCQ 9.3-B `[9.3.4 · Analysis]`** Why is a burnup often preferred to a burndown for release forecasting?
- A. It is simpler.
- B. It shows scope change (the moving total line), not just progress. ✅
- C. It hides added scope.
- D. It requires no velocity.

*Rationale:* A burnup plots completed work against a total-scope line, so scope additions are visible as the
line rises — a burndown to zero hides them. It is not simpler, does not hide scope, and still uses velocity to
project.

**MCQ 9.3-C `[9.3.5 · Recall]`** In the agile "inverted" iron triangle, what is fixed?
- A. Scope.
- B. Time and cost. ✅
- C. Quality only.
- D. Nothing.

*Rationale:* Agile fixes time and cost (a cadence of funded Sprints) and flexes scope — the inverse of the
predictive triangle.

### Self-check — KA 9.3

1. Why report a release forecast as a *range* of Sprints? *(Velocity varies; a range (e.g. optimistic/
   pessimistic velocity) is more honest than false-precise single number.)*
2. What does a burnup show that a burndown does not? *(Scope change — the moving total-scope line.)*

---

## Knowledge Area 9.4 — Kanban, Lean and scaling

*Topics: 9.4.1 Kanban and flow · 9.4.2 Lean and waste · 9.4.3 scaling frameworks at awareness level.*

### 9.4.1 Kanban and flow

**The principle.** **Kanban** manages work as a **flow**: **visualise** the workflow (a board with columns per
state), **limit work in progress (WIP)**, **manage and measure flow** (throughput, cycle time), and improve
continuously. Limiting WIP is the counter-intuitive core: **less** work started at once finishes **faster**,
because it reduces context-switching and queueing. Kanban's flow metrics (cycle time, throughput, CFD, 9.3.4)
give a controls professional a cadence-independent way to measure and forecast adaptive work.

**Worked example 9.4.1 — WIP limits and flow.** A team with no WIP limit has 15 items "in progress" and a
cycle time of 20 days. Imposing a WIP limit of 5 focuses the team; cycle time falls to ~8 days and throughput
rises, because items finish instead of stalling — illustrating **Little's Law** conceptually (average cycle
time ≈ WIP ÷ throughput): cut WIP and cycle time falls for the same throughput.

### 9.4.2 Lean and waste

**The principle.** **Lean** thinking maximises **value** and minimises **waste** — anything the customer would
not pay for (waiting, rework, hand-offs, over-production, excess inventory/WIP). Agile inherits much of its DNA
from Lean. For a controls professional the Lean lens is practical: much project "cost" is waste (rework from
poor quality assurance, Domain 8, KA 8.3.3; waiting from bottlenecks), and flow metrics make that waste visible.

### 9.4.3 Scaling frameworks at awareness level

**The principle (awareness only).** When many teams work on one product, **scaling** approaches coordinate
them: **SAFe** (a structured enterprise framework organising teams into an "Agile Release Train" delivering on
a common cadence), **LeSS** (large-scale Scrum — Scrum's rules extended to many teams with minimal addition),
and **Scrum-of-Scrums** (a coordination meeting across teams). A controls professional should recognise these
concepts and the **release-train** idea (multiple teams delivering to a synchronised cadence), without treating
any as prescriptive detail — the choice is a delivery decision, and the controls need is to measure and forecast
across the coordinated teams.

### Key terms — KA 9.4

| Term | Meaning |
|---|---|
| **Kanban** | Visualise flow, limit WIP, manage/measure flow, improve. |
| **WIP limit** | A cap on concurrent work-in-progress that speeds completion. |
| **Little's Law** | Cycle time ≈ WIP ÷ throughput (conceptually). |
| **Lean / waste** | Maximise value, minimise non-value work. |
| **SAFe / LeSS / Scrum-of-Scrums / release train** | Scaling approaches (awareness level). |

### Sample MCQs — KA 9.4

**MCQ 9.4-A `[9.4.1 · Analysis]`** Imposing a WIP limit typically:
- A. Slows delivery by restricting work.
- B. Speeds completion by cutting context-switching and queueing. ✅
- C. Has no effect on cycle time.
- D. Increases work in progress.

*Rationale:* Limiting WIP focuses the team so items finish faster (lower cycle time, per Little's Law). It does
not restrict useful throughput, is not neutral, and by definition lowers WIP.

**MCQ 9.4-B `[9.4.3 · Recall]`** The "Agile Release Train" is a concept most associated with:
- A. Kanban.
- B. SAFe. ✅
- C. Waterfall.
- D. Little's Law.

*Rationale:* The Agile Release Train is a SAFe construct (teams delivering to a common cadence). The others are
a flow method, a predictive approach, and a flow law.

### Self-check — KA 9.4

1. State Little's Law conceptually and its practical implication. *(Cycle time ≈ WIP ÷ throughput; cutting WIP
   cuts cycle time.)*
2. Name two scaling approaches at awareness level. *(SAFe, LeSS, Scrum-of-Scrums.)*

---

## Knowledge Area 9.5 — Agile cost control, forecasting & earned value (AgileEVM) *(the project-controls crux)*

*Topics: 9.5.1 funding and run-rate · 9.5.2 forecasting from velocity and burnup · 9.5.3 AgileEVM · 9.5.4
reconciling story points to % complete and IFRS 15.*

### 9.5.1 Funding models and run-rate

**The principle.** Adaptive delivery is usually funded by **capacity** (a stable team for a number of Sprints)
rather than by fixed scope, so its cost is a **run-rate**: `cost per Sprint × number of Sprints`. A stable
cross-functional team has a predictable Sprint cost, which makes the **`ETC`** a function of Sprints remaining
(9.3.3) rather than of scope-cost build-up.

**Worked example 9.5.1 — cost per Sprint and ETC.** A team of six costs **USD 60,000 per 2-week Sprint** (fully
loaded). From 9.3.3, ~8 Sprints remain → `ETC = 60,000 × 8 = 480,000` (range at 7–10 Sprints: **USD
420,000–600,000**). Reporting the range, not a point, is the honest forecast.

### 9.5.2 Forecasting from velocity and burnup

**The principle.** Completion is forecast from **velocity** (9.3.3) and read off the **burnup** (9.3.4),
projecting the completed line to meet the (possibly moving) scope line. Because both velocity and scope vary, a
**three-point** forecast (optimistic/likely/pessimistic) is standard. This is the adaptive analogue of the EAC
family (Domain 6): different assumptions, an honest range.

### 9.5.3 AgileEVM

**Definition & purpose.** **AgileEVM** applies earned value to adaptive delivery by using **story points (or
their budget value)** as the progress measure against a **release budget**. It reuses the Domain 6 formulae —
`EV`, `AC`, `PV`, `CPI`, `SPI`, `EAC` — with agile inputs, *and its assumptions and limits must be stated*.

```
% complete = story points completed / total planned points
EV  = % complete × BAC            (BAC = release budget)
PV  = planned % complete × BAC    (planned points by the data date / total planned points × BAC)
CPI = EV / AC     SPI = EV / PV    EAC = BAC / CPI   (and the other Domain 6 methods)
```

**Worked example 9.5.3 — AgileEVM on a release.**

1. **Setup.** Release **`BAC` = USD 600,000**; **300 story points** planned over **10 Sprints**. At the end of
   **Sprint 5**: **120 points completed**; **`AC` = USD 320,000**; the plan expected **150 points** done by
   Sprint 5.
2. **Substitution.**
   - `% complete = 120/300 = 40 %` → `EV = 40 % × 600,000 = 240,000`.
   - `planned % = 150/300 = 50 %` → `PV = 50 % × 600,000 = 300,000`.
   - `CPI = 240,000/320,000 = 0.75`; `SPI = 240,000/300,000 = 0.80`.
   - `EAC = BAC/CPI = 600,000/0.75 = 800,000`; `VAC = 600,000 − 800,000 = (200,000)`.
3. **Result.** The release is **over cost** (`CPI` 0.75) and **behind** (`SPI` 0.80), forecasting `EAC` **USD
   800,000** — a projected **USD 200,000** overrun of the release budget.
4. **The critical caveat.** If **scope changes** (points added/removed), the **total planned points and `BAC`
   change**, and `EV`/`PV`/`CPI`/`SPI` shift accordingly — the classic AgileEVM assumption/limit: the metrics
   are only meaningful **against a defined release scope and budget**, and must be **rebaselined transparently**
   when scope is deliberately flexed (the whole point of agile). Story points are also a **relative team
   measure**, not an absolute one — they cannot be compared across teams, and re-estimation must be handled
   consistently. Stated with these caveats, AgileEVM gives adaptive delivery a genuine cost-and-schedule
   forecast; asserted without them, it is misleading.

**Worked example 9.5.3b — AgileEVM when scope is rebaselined.**

1. **Setup.** Continuing the release (`BAC` USD 600,000; 300 planned points at USD 2,000/point). At the end of
   Sprint 5, **120 points** are done and `AC` = **USD 320,000**. Now **60 points of new scope** are approved.
2. **Formula.** Rebaseline transparently — `new BAC = new total points × per-point rate`; `EV` of completed
   work is unchanged (`points done × rate`); `CPI = EV/AC`; `EAC = new BAC / CPI`.
3. **Substitution.** New total points `= 300 + 60 = 360`; `new BAC = 360 × 2,000 = 720,000`; `EV = 120 × 2,000
   = 240,000` (unchanged); `% complete = 120/360 = 33.3 %`; `CPI = 240,000/320,000 = 0.75`;
   `EAC = 720,000/0.75 = 960,000`.
4. **Result.** Adding scope raises `BAC` (600,000 → 720,000) and `EAC` (to **USD 960,000**), and lowers
   `% complete` (40 % → 33.3 %), but the **`EV` of work already done is constant at 240,000** and `CPI` stays
   0.75.
5. **Interpretation.** This is the AgileEVM discipline — deliberate scope change is handled by **transparent
   rebaselining** of `BAC` and planned points, not by pretending the metrics are unaffected (cross-ref 9.5.3's
   caveat). The `EAC` rises because there is genuinely more to build.

### 9.5.4 Reconciling story points to % complete and IFRS 15

**The synthesis.** The `% complete` from AgileEVM (9.5.3) can inform the **input-method** progress for **IFRS
15 over-time revenue** (Domain 2, KA 2.2.6) on an agile contract — but with care: IFRS 15's input method is
usually **cost-to-cost**, and **story-point % is a proxy for effort/scope, not cost or value**. A controls
professional reconciles three progress views on an agile contract — **story-point/AgileEVM % complete**,
**cost-to-cost % complete**, and the **billing** basis (T&M/capped/milestone, KA 9.6/Domain 7) — and explains
their differences, exactly as for a predictive contract (Domain 7, KA 7.4.4). The reconciliation is what keeps
an agile contract's *performance*, *revenue* and *cash* consistent.

**Worked link.** With AgileEVM `% complete` = 40 % and a transaction price of USD 700,000, a naïve output-style
revenue would be `40 % × 700,000 = 280,000` — but if cost-to-cost `% complete` is only 35 % (costs lag points),
IFRS 15 revenue on the cost input method would be `35 % × 700,000 = 245,000`, a **USD 35,000** difference the
professional must reconcile and explain (and which becomes a contract asset/liability, Domain 2, KA 2.2.7).

**AI in this KA.** AI is strong here (Domain 13, KA 13.5): forecasting velocity and release completion from
Sprint history, generating three-point ranges, detecting flow anomalies, and projecting AgileEVM `EAC`. It can
also *mislead* if it treats story points as absolute or ignores scope rebaselining. The professional owns the
forecast, the scope-change transparency and the revenue reconciliation. **AI proposes, the professional
disposes.**

### Key terms — KA 9.5

| Term | Meaning |
|---|---|
| **Run-rate / capacity funding** | Cost = cost per Sprint × Sprints; `ETC` from Sprints remaining. |
| **AgileEVM** | Earned value on variable scope using story-point % against a release budget. |
| **Rebaselining** | Transparently resetting scope/`BAC` when scope is deliberately flexed. |
| **Progress reconciliation (agile)** | Tying AgileEVM %, cost-to-cost %, and billing basis. |

### Sample MCQs — KA 9.5

**MCQ 9.5-A `[9.5.3 · Application]`** Release `BAC` USD 600,000; 300 points planned; 120 done; `AC` USD 320,000;
150 planned done. The `CPI` is:
- A. 1.33
- B. 0.75 ✅
- C. 0.80
- D. 0.40

*Rationale:* `EV = (120/300)×600,000 = 240,000`; `CPI = EV/AC = 240,000/320,000 = 0.75`. C is the `SPI`; D is
`% complete`; A inverts the ratio.

**MCQ 9.5-B `[9.5.3 · Analysis]`** The central assumption/limit when applying AgileEVM is that:
- A. Story points equal hours.
- B. The metrics are meaningful only against a defined release scope/budget and must be rebaselined transparently when scope flexes. ✅
- C. Velocity never changes.
- D. It cannot use Domain 6 formulae.

*Rationale:* AgileEVM reuses the EVM formulae but depends on a defined release scope/`BAC`; deliberate scope
change requires transparent rebaselining. Points are not hours, velocity varies, and it *does* reuse the
formulae.

**MCQ 9.5-C `[9.5.4 · Analysis]`** AgileEVM `% complete` is 40 % but cost-to-cost `% complete` is 35 % on an
IFRS 15 over-time contract. The professional should:
- A. Recognise revenue at 40 %.
- B. Reconcile the two and recognise on the appropriate input basis (here cost-to-cost 35 %), explaining the difference. ✅
- C. Ignore IFRS 15.
- D. Average them to 37.5 %.

*Rationale:* IFRS 15's input method is typically cost-to-cost; story-point % is a proxy that must be
reconciled, not substituted. The difference (and any resulting contract asset/liability) is explained, not
averaged away.

### Self-check — KA 9.5

1. Write the AgileEVM `EV` and `CPI` formulae and the key caveat. *(`EV = %complete × BAC`; `CPI = EV/AC`;
   valid only against a defined release scope/`BAC`, rebaselined transparently on scope change.)*
2. Why can story-point % differ from IFRS 15 cost-to-cost %? *(Points measure relative effort/scope, not cost;
   costs may lead or lag points — reconcile and explain.)*

---

## Knowledge Area 9.6 — Hybrid delivery and agile governance

*Topics: 9.6.1 combining stage-gate and agile · 9.6.2 milestone and phase-gate reporting over Sprints · 9.6.3
contracting for agile · 9.6.4 assurance and audit trail.*

### 9.6.1 Combining stage-gate and agile

**The principle.** **Hybrid** governance wraps **predictive stage-gates and milestone reporting** around
**agile execution** (Domain 8, KA 8.6). The programme passes funding/assurance gates predictively, while the
work between gates is delivered in Sprints. The controls task is to present **one coherent picture**: earned
value on the predictive scope, AgileEVM/velocity on the adaptive scope, reconciled at the programme level so a
board sees a single status.

### 9.6.2 Milestone and phase-gate reporting over Sprints

**The principle.** Boards and gates think in **milestones**; agile teams think in **Sprints**. The controls
professional maps Sprints/releases to the milestones a gate needs — reporting, for a gate, the value delivered,
the run-rate and forecast completion (9.5), and the AgileEVM status — so the adaptive work is legible to
predictive governance without forcing it into a false fixed baseline.

### 9.6.3 Contracting for agile

**The principle.** Fixed-price/fixed-scope contracting fights agile's variable scope. Agile-friendly forms
(Domain 7, KA 7.1) include **T&M** and **capped T&M** (pay for capacity, cap the exposure) and **target-cost**
with agile delivery (shared incentive over a flexible scope). The controls consequence: **billing** follows the
contract (capacity/milestone), **revenue** follows IFRS 15 performance, and **AgileEVM** measures performance —
three views reconciled (9.5.4).

### 9.6.4 Assurance and audit trail

**The principle.** Agile is sometimes wrongly assumed to lack an audit trail. In fact its artefacts —
backlog, Sprint records, Increments, Definition of Done, review outcomes — form a **rich, contemporaneous
trail**. The controls/assurance task is to ensure that trail is **captured and auditable**: what was delivered
each Sprint, what scope changed and why (rebaselining, 9.5.3), and how forecasts were derived. Done well,
adaptive delivery is *more* transparent than a predictive plan that is updated quarterly.

**AI in this KA.** AI helps hybrid governance by reconciling predictive and adaptive status into one report,
mapping Sprints to milestones, and drafting gate submissions from Sprint data. The professional owns the
integrity of the combined picture and the scope-change narrative. **AI proposes, the professional disposes.**

### Key terms — KA 9.6

| Term | Meaning |
|---|---|
| **Hybrid governance** | Predictive stage-gates around agile execution. |
| **Milestone-to-Sprint mapping** | Translating Sprints/releases into gate milestones. |
| **Agile contracting** | T&M, capped T&M, target cost — forms that fit variable scope. |
| **Agile audit trail** | Backlog/Sprint/Increment records providing contemporaneous evidence. |

### Sample MCQs — KA 9.6

**MCQ 9.6-A `[9.6.3 · Analysis]`** Which contract form best fits agile's variable scope?
- A. Fixed-price, fixed-scope lump sum.
- B. Capped time & materials (pay for capacity, cap exposure). ✅
- C. Remeasurement of fixed civil quantities.
- D. A performance bond.

*Rationale:* Capped T&M funds capacity while limiting exposure — compatible with flexing scope. Fixed-price/
fixed-scope fights agile; remeasurement suits defined civil work; a bond is security, not a pricing form.

**MCQ 9.6-B `[9.6.4 · Analysis]`** The claim that "agile has no audit trail" is:
- A. True — agile avoids documentation.
- B. False — backlog, Sprint records, Increments and Definition of Done form a contemporaneous trail. ✅
- C. True for Scrum only.
- D. Irrelevant to controls.

*Rationale:* Agile's artefacts provide a rich, contemporaneous audit trail; the controls task is to capture it.
Agile values working product over *comprehensive* documentation, not the absence of records.

### Self-check — KA 9.6

1. How does a controls professional make agile work legible to a stage-gate board? *(Map Sprints/releases to
   milestones; report value delivered, run-rate, forecast and AgileEVM status.)*
2. Name two agile-friendly contract forms and why. *(Capped T&M and target cost — they fund capacity/share
   risk over a flexible scope.)*

---

## Domain 9 summary

Adaptive delivery rests on an **empirical mindset** — transparency, inspection, adaptation — and inverts the
iron triangle: fix time and cost, flex scope. **Scrum** operationalises it through three accountabilities
(Product Owner, Scrum Master, Developers), five time-boxed events within the Sprint, and three artefacts with
their commitments (Product Goal, Sprint Goal, Definition of Done). Progress is measured by **velocity**,
**burndown/burnup** and **flow metrics** (throughput, cycle time, CFD), and forecast as a **range**; **Kanban**
and **Lean** add flow discipline and waste reduction, with scaling frameworks understood at awareness level. The
project-controls crux is **AgileEVM** — applying `EV`/`CPI`/`SPI`/`EAC` to a release budget via story-point
`%`, *with the standing caveat* that the metrics hold only against a defined scope/`BAC` and must be rebaselined
transparently when scope flexes — reconciled to cost-to-cost `%` and **IFRS 15** revenue. Finally, **hybrid
governance** wraps predictive stage-gates around agile execution, mapping Sprints to milestones and reconciling
three progress views, with the agile artefacts themselves forming a rich audit trail.

**Cross-references.** Development approaches and incremental/iterative → 8.6; the EVM formulae reused → Domain
6; IFRS 15 over-time recognition and contract asset/liability → 2.2; agile contract forms → 7.1; billing
reconciliation → 7.4–7.5; three-point forecasting → 3.4; AI for backlog/velocity/flow → 13.5.

*Domain 9 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
