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

**MCQ 9.1-C `[9.1.3 · Application]`** Under adaptive planning, a programme funds a stable team for **12
two-week Sprints at USD 90,000 per Sprint**, letting scope flex to fit. The fixed cost envelope is:
- A. USD 90,000
- B. USD 1,080,000 ✅
- C. USD 540,000
- D. USD 2,160,000

*Rationale:* Adaptive planning fixes time and cost: `12 × 90,000 = 1,080,000`, with scope the variable. A is a
single Sprint; C funds only half the cadence; D wrongly treats the per-Sprint rate as a weekly rate over 24
weeks.

**MCQ 9.1-D `[9.1.1 · Recall]`** The Agile Manifesto's stance on planning is best described as:
- A. Plans are prohibited in agile delivery.
- B. Responding to change is valued over following a plan, while the plan still has value. ✅
- C. Following the plan is valued over responding to change.
- D. Plans must be fixed before any work starts.

*Rationale:* Each Manifesto value statement prefers the left item *while recognising the item on the right
still has value* — change over plan-following, but not the abolition of plans. A and D overstate in opposite
directions; C inverts the value statement.

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

**MCQ 9.2-C `[9.2.3 · Application]`** A team runs **two-week Sprints**, each producing at least one usable
Increment, with a new Sprint starting immediately after the previous one. Over a 26-week release window, the
minimum number of Increments is:
- A. 26
- B. 13 ✅
- C. 12
- D. 6

*Rationale:* `26 weeks / 2 weeks per Sprint = 13` Sprints, each yielding at least one Increment — and because a
new Sprint starts immediately, there are no gap weeks. A assumes weekly Increments; C wrongly inserts a gap
between Sprints; D assumes four-week Sprints.

**MCQ 9.2-D `[9.2.2 · Analysis]`** A programme stakeholder asks the Scrum Master to assign this Sprint's tasks
to individual Developers. The request misreads Scrum because:
- A. Only the Product Owner assigns tasks.
- B. The Developers own the Sprint Backlog plan; the Scrum Master is a coach and impediment-remover, not a manager over the team. ✅
- C. Tasks may only be assigned at the Sprint Review.
- D. The Scrum Master may assign tasks but only in writing.

*Rationale:* The Developers are accountable for the Sprint plan and the daily work; the Scrum Master serves the
team's effectiveness rather than directing it. A shifts the error to the Product Owner (who owns *what and
why*, not task assignment); C and D invent rules Scrum does not contain.

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

**Worked example 9.3.3c — capacity planning is not velocity.**

1. **Setup.** A team of **5 Developers** has **9 working days** available each Sprint (10 days less
   ceremonies) at **6 focus-hours** a day; historical velocity is **30 points**. Next Sprint, two members
   are each out **2 days**.
2. **Formula.** `Planned capacity = people × days × focus-hours`; `adjusted commitment = velocity ×
   (adjusted capacity / normal capacity)`.
3. **Substitution.** Normal capacity `= 5 × 9 × 6 = 270 hours`; adjusted capacity `= 270 − (2 × 2 × 6) =
   246 hours`; commitment `= 30 × 246/270 ≈ 27 points` (round down).
4. **Result.** The team plans **246 hours** of capacity and commits **~27 points**, not its usual 30.
5. **Interpretation.** **Capacity** (hours available) and **velocity** (points delivered) are different
   instruments — capacity adjusts the commitment for known absences, while velocity remains the empirical
   forecast basis (9.3.3). Committing 30 points into a 246-hour Sprint is how teams start failing Sprints
   and inflating estimates to compensate.

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

**MCQ 9.3-D `[9.3.3 · Application]`** A team's velocity is **25 points/Sprint**. Original release scope was
300 points; by the end of Sprint 6, **150 points** are complete and **50 points of new scope** are approved.
The Sprints remaining are:
- A. 6
- B. 8 ✅
- C. 14
- D. 12

*Rationale:* New total scope `= 300 + 50 = 350`; remaining `= 350 − 150 = 200`; `200 / 25 = 8` Sprints. A
ignores the added scope (`150/25`); C divides the whole new scope by velocity, forgetting the 150 points
already done; D uses the original 300 and also forgets the completed work.

**MCQ 9.3-E `[9.3.4 · Analysis]`** On a cumulative flow diagram, the "in progress" band is steadily widening
while the "done" band's slope is flat. The best reading is:
- A. Throughput is rising healthily.
- B. Work is being started faster than it is finished — WIP is growing at a bottleneck. ✅
- C. Scope has been removed from the release.
- D. Cycle time is falling.

*Rationale:* A widening in-progress band with flat completion means items enter the state faster than they
leave — growing WIP queuing at a bottleneck, which lengthens (not shortens) cycle time. A and D describe the
opposite pattern; C would narrow the to-do band, not widen in-progress.

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

**Worked example 9.4.1b — Little's Law with numbers.**

1. **Setup.** A support/flow team carries **WIP = 12 items** with a **throughput of 3 items per week**;
   it then halves WIP to **6 items** at the same throughput.
2. **Formula.** `Average cycle time ≈ WIP / throughput`.
3. **Substitution.** Before: `12 / 3 = 4 weeks`. After: `6 / 3 = 2 weeks`.
4. **Result.** Average cycle time falls from **4 weeks to 2 weeks** — items complete in half the time
   with **no** extra capacity.
5. **Interpretation.** This is the counter-intuitive core of flow — starting less finishes faster, and the
   lever is the WIP limit, not working harder. The professional's check: throughput held at 3/week after
   the change (if the WIP cut also cut throughput, the gain evaporates). Cross-ref the qualitative
   treatment above and Advanced 9.A.3 (flow efficiency).

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

**MCQ 9.4-C `[9.4.1 · Application]`** A Kanban team carries **12 items** of work in progress and completes
**3 items per day**. By Little's Law (cycle time ≈ WIP ÷ throughput), the average cycle time is approximately:
- A. 4 days ✅
- B. 36 days
- C. 0.25 days
- D. 9 days

*Rationale:* `cycle time ≈ WIP / throughput = 12 / 3 = 4` days. B multiplies instead of dividing; C inverts the
ratio (`3/12`); D subtracts the figures.

**MCQ 9.4-D `[9.4.2 · Recall]`** In Lean thinking, "waste" is best defined as:
- A. Any activity the customer would not pay for, such as waiting, rework and hand-offs. ✅
- B. Only physical scrap material.
- C. Any spending above the original budget.
- D. All documentation.

*Rationale:* Lean defines waste as non-value work — anything the customer would not pay for — including
waiting, rework, hand-offs and excess WIP. B is too narrow; C confuses waste with cost variance; D overstates —
documentation the customer values (or regulation requires) is not waste.

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

**MCQ 9.5-D `[9.5.1 · Application]`** A capacity-funded team costs **USD 75,000 per Sprint**; `AC` to date is
USD 300,000; the velocity forecast shows **6 Sprints remaining**. The `ETC` and `EAC` are:
- A. `ETC` USD 450,000; `EAC` USD 750,000 ✅
- B. `ETC` USD 450,000; `EAC` USD 450,000
- C. `ETC` USD 300,000; `EAC` USD 750,000
- D. `ETC` USD 750,000; `EAC` USD 1,050,000

*Rationale:* Under run-rate funding `ETC = cost per Sprint × Sprints remaining = 75,000 × 6 = 450,000`, and
`EAC = AC + ETC = 300,000 + 450,000 = 750,000`. B confuses `ETC` with `EAC` (forgetting cost already spent); C
swaps `AC` into the `ETC`; D adds `AC` into the `ETC` and then double-counts it.

**MCQ 9.5-E `[9.5.3 · Recall]`** In AgileEVM, `EV` at a data date is computed as:
- A. (story points planned by the data date / total planned points) × `BAC`.
- B. (story points completed / total planned points) × `BAC`. ✅
- C. story points completed × cost per Sprint.
- D. `% complete` × `AC`.

*Rationale:* `EV = % complete × BAC`, with `% complete = points completed / total planned points` — progress
valued against the release budget. A is the `PV` formula; C mixes a scope measure with a capacity cost; D
values progress at actual cost, which is what `EV` must never do.

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

**MCQ 9.6-C `[9.6.3 · Application]`** An agile team is engaged on **capped T&M** at USD 100,000 per month with
a cap of USD 1,300,000. Delivery takes **14 months**. The client pays:
- A. USD 1,400,000
- B. USD 1,300,000 ✅
- C. USD 1,200,000
- D. USD 100,000

*Rationale:* Uncapped T&M would be `14 × 100,000 = 1,400,000`, but the cap limits the client's exposure to
**1,300,000** — the contractor bears the 100,000 beyond it. A forgets the cap; C wrongly bills only 12 months;
D is a single month.

**MCQ 9.6-D `[9.6.2 · Recall]`** To make agile work legible at a predictive phase gate, the controls
professional reports:
- A. Raw Sprint Backlogs for the board to interpret.
- B. Value delivered, run-rate and forecast completion, mapped from Sprints/releases to the gate's milestones. ✅
- C. Only the original fixed baseline.
- D. Nothing — agile work is exempt from gates.

*Rationale:* Hybrid governance translates Sprints into the milestone language a gate needs — value delivered,
run-rate, forecast and AgileEVM status — without forcing a false fixed baseline. A leaves the translation
undone; C imposes exactly the false baseline hybrid reporting avoids; D ignores the governance wrapper.

### Self-check — KA 9.6

1. How does a controls professional make agile work legible to a stage-gate board? *(Map Sprints/releases to
   milestones; report value delivered, run-rate, forecast and AgileEVM status.)*
2. Name two agile-friendly contract forms and why. *(Capped T&M and target cost — they fund capacity/share
   risk over a flexible scope.)*

---

## Advanced topics — Domain 9

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 9.A.1 — Definition of Ready, Definition of Done and acceptance criteria

**Three gates, distinguished.** Adaptive delivery controls quality through three distinct gates that are
often conflated. A **Definition of Ready (DoR)** is an *entry* gate — a team's working agreement on when a
backlog item is fit to pull into Sprint Planning (clear, sized, dependencies known, acceptance criteria
drafted); it is a useful convention, not a Scrum commitment (9.2.4). **Acceptance criteria** are *per-story*
tests: does **this** story do what was asked (9.3.1)? The **Definition of Done (DoD)** is the *exit* gate and
the Increment's commitment (9.2.4): the quality standard — tested, integrated, documented to the agreed
level — that **every** item must meet before it counts as part of a usable Increment.

**How a weak DoD inflates "done".** Velocity (9.3.3), the burnup (9.3.4) and AgileEVM (9.5.3) all count
*completed* points. If "done" quietly excludes integration or testing, points are claimed early: velocity
flatters, `EV` overstates progress — the agile analogue of `EV` inflated by optimistic progress claims
(Domain 6, KA 6.1.2) — and the deferred quality returns as rework that taxes later Sprints (technical debt,
9.A.2). The release looks fast and then stalls in an unplanned "hardening" phase at the end. A strong,
objective DoD plays the role that objective earning rules play in EVM: it is the main defence against
optimism in the progress measure itself.

**The quality floor auditors can test.** Because a DoD is written, stable and binary, assurance can sample
"done" items against it — do the tests exist and pass? was the item integrated? — which is exactly the
contemporaneous audit trail KA 9.6.4 describes. A controls professional therefore treats a change to the DoD
as they treat a change to an earning rule: legitimate, but **disclosed**, because it silently changes the
meaning of every velocity, burnup and AgileEVM figure downstream.

### Advanced 9.A.2 — Technical debt as a controls concept

**Definition.** **Technical debt** is the future work created when a team takes a shortcut — a design or
implementation good enough to ship now but not good enough to build on. It is **invisible scope**: it appears
on no backlog burnup, yet it **taxes future velocity** — each subsequent change costs more than it should,
the "interest" on the debt. In controls terms it is an unrecorded liability: the release looks complete at a
cost that omits what it will cost to live with.

**Making it visible.** The remedy is the same as for every invisible exposure in this reference: a register.
A **debt register kept beside the risk register** records each item — the shortcut taken, why, the estimated
repayment cost (in points), and the interest being paid (velocity drag, defect rate). The symptoms are
measurable with the domain's own metrics: falling velocity at constant team size and a stable DoD (9.A.1), a
rising defect ratio, lengthening cycle times (9.3.4). Mature teams service the debt visibly, allocating an
agreed share of each Sprint's capacity to repayment — a maintenance budget, not a hidden tax.

**Taking debt deliberately.** Deliberate debt can be a rational trade: shortcut now to hit a market window or
a regulatory date, repay later. The decision discipline is the accelerate-versus-LDs comparison of Domain 7
in agile clothing: price both sides — the value of the earlier date against the repayment cost *plus* the
interest paid until repayment — decide consciously, and record the decision in the register. What is never
acceptable is **invisible** debt: it is the agile analogue of an optimistic `EAC`, a forecast the board acts
on that omits a cost the team already knows about. The register is what keeps the scope-quality trade honest
and auditable (9.6.4).

### Advanced 9.A.3 — Flow efficiency and queueing

**Touch time vs wait time.** An item's cycle time (9.3.4) decomposes into **touch time** (someone is actually
working on it) and **wait time** (it is queued — awaiting review, a test environment, another team). **Flow
efficiency** is the ratio of touch time to total cycle time, and mature teams measuring it for the first time
are usually shocked: most of an item's life is spent waiting, which is precisely the Lean waste of 9.4.2.

**Worked example 9.A.3 — flow efficiency.**

1. **Setup.** Time-stamped board data shows an item's cycle time was **20 days**, of which it was actively
   worked for **4 days** and queued for the rest.
2. **Formula.** `Flow efficiency = touch time / cycle time`.
3. **Substitution.** `Flow efficiency = 4 / 20 = 20 %`.
4. **Result.** **20 % flow efficiency** — the item waited 16 of its 20 days.
5. **Interpretation.** The biggest lever on cycle time is removing **wait**, not working faster: halving the
   touch time saves 2 days; halving the wait saves 8.

**Why 100 % utilisation destroys flow.** A team loaded to full utilisation has no capacity to absorb the
variability that knowledge work always has, so arriving work queues. WIP grows, and by Little's law (9.4.1,
cycle time ≈ WIP ÷ throughput) cycle time stretches with it — and the queueing effect is non-linear: as
utilisation approaches capacity, wait times grow disproportionately, which is why the last few points of
utilisation buy enormous delay. **Slack is therefore a performance feature, not waste**: spare capacity is
what keeps queues short, absorbs surprises and funds improvement. This is the deliberate logic of the WIP
limits of 9.4.1, and it is genuinely counter-intuitive for a controls tradition that reads idle capacity as
inefficiency — in flow systems, chasing 100 % utilisation *is* the inefficiency.

### Advanced 9.A.4 — Scaling metrics and dependency management

**The problem.** When many teams build one product (9.4.3), programme status is not the sum of team statuses.
Story points are a **relative team measure** — explicitly not comparable across teams (the 9.5.3 caveat) — so
summing or comparing team velocities produces a number with no meaning, and publishing it invites inflation,
because points are cheap to reprice. Programme-level control needs programme-level machinery and
programme-level units.

**The machinery.** Three practices carry most of the weight. A **programme cadence** — synchronised Sprint
boundaries across teams — creates common inspect-and-adapt points, the release-train idea of 9.4.3 in
controls terms. **Integrated increment reviews** demonstrate a *combined, working* increment rather than
per-team demos: at scale, integrated working product is the only honest progress evidence, for the same
reason the DoD is the quality floor (9.A.1). A **cross-team dependency board** makes every dependency visible
with its provider, consumer and needed-by date, managed like a schedule interface — and **dependency ageing**
(how long items sit unresolved on that board) is a leading indicator: an ageing dependency queue predicts a
missed increment before any single team's velocity moves, the queueing logic of 9.A.3 applied across teams.

**Measure the programme, not the sum of the teams.** The programme's measures are stated in programme units:
a **feature (capability) burnup** — features completed to the programme's Definition of Done against total
feature scope, the 9.3.4 burnup one level up — plus dependency ageing and integrated-increment health. Team
velocity keeps its proper job as each team's private planning input (9.3.3); it is never an aggregation or
comparison unit. Reported this way, the adaptive programme slots directly into the hybrid gate reporting of
KA 9.6.2: value delivered, run-rate and forecast, in units a board can govern by.

---

## Case study — Domain 9: forecasting a telecom software release two ways

*This case study integrates the domain end-to-end: velocity and burnup forecasting (KAs 9.3.3–9.3.4), the
inverted iron triangle (KA 9.3.5), run-rate funding (KA 9.5.1), AgileEVM (KA 9.5.3) and hybrid stage-gate
governance (KA 9.6). Its central lesson is that two legitimate forecasting methods, applied to the same data,
give different answers — and that the controls professional's job is to reconcile them, not to hide the
disagreement.*

### Background

A telecom operator is replacing its operations-support software — the systems that provision customer
services, monitor the network and manage faults. The replacement is delivered as an **agile release inside a
stage-gated transformation programme**: the programme as a whole passes predictive funding and assurance
gates, while the software itself is built by a Scrum team in two-week Sprints — **hybrid governance** exactly
as described in KA 9.6. The stage-gate board thinks in milestones and budgets; the team thinks in Sprints and
story points; the controls professional sits between the two and must make each legible to the other
(KA 9.6.2).

The release was baselined as follows:

| Parameter | Value |
|---|---|
| Release budget `BAC` | USD 1,200,000 |
| Planned scope | 400 story points |
| Planned duration | 10 two-week Sprints |
| Implied budget per point | USD 3,000 (`= 1,200,000 / 400`) |
| Funded capacity per Sprint | USD 120,000 (`= 1,200,000 / 10`) |

At the end of **Sprint 6** — the data date, and shortly before a programme gate — the status is: **210 story
points** complete to the Definition of Done; actual cost **`AC` = USD 780,000**; and the release plan expected
**240 points** done by now. The board wants a cost and completion forecast. The controls professional prepares
it **two ways** — once with AgileEVM (KA 9.5.3) and once from velocity and run-rate (KAs 9.3.3 and 9.5.1) —
knowing the methods encode different assumptions and will not agree exactly.

### Forecast 1 — AgileEVM (KA 9.5.3)

1. **Setup.** `BAC` = USD 1,200,000; total planned scope 400 points; at the end of Sprint 6, 210 points are
   done, `AC` = USD 780,000, and 240 points were planned to be done. Scope has not been rebaselined, so the
   AgileEVM precondition — a defined release scope and budget — holds (9.5.3's caveat).
2. **Formula.** `EV = (points done / total points) × BAC`; `PV = (points planned / total points) × BAC`;
   `CPI = EV / AC`; `SPI = EV / PV`; `EAC = BAC / CPI`.
3. **Substitution.** `EV = (210/400) × 1,200,000 = 630,000`; `PV = (240/400) × 1,200,000 = 720,000`;
   `CPI = 630,000 / 780,000 = 0.8077 → 0.81`; `SPI = 630,000 / 720,000 = 0.875 → 0.88`;
   `EAC = BAC / CPI = 1,200,000 / 0.8077 ≈ 1,485,700`.
4. **Result.** The release is **over cost** (`CPI` 0.81 — each dollar spent earns about 81 cents of planned
   value) and **behind plan** (`SPI` 0.88). AgileEVM projects an `EAC` of **≈ USD 1.49m**, roughly
   **USD 285,700** over the release budget.
5. **Interpretation.** `EAC = BAC/CPI` assumes the cost efficiency to date persists to completion — the same
   assumption as the standard Domain 6 method it reuses. It is equivalent to
   `AC + (BAC − EV)/CPI = 780,000 + 570,000/0.8077 ≈ 1,485,700`: the remaining 190 points of value are bought
   at the demonstrated cost per value earned. The standing AgileEVM caveat applies: these metrics hold only
   against the defined 400-point scope and USD 1,200,000 `BAC`; if the board later flexes scope, the release
   must be rebaselined transparently and the metrics restated (9.5.3).

### Forecast 2 — velocity and run-rate (KAs 9.3.3, 9.5.1)

1. **Setup.** The same data, read the agile-native way: 210 points completed in 6 Sprints; 400 − 210 = 190
   points remain; USD 780,000 spent in 6 Sprints against a funded capacity of USD 120,000 per Sprint.
2. **Formula.** `Velocity = points completed / Sprints elapsed`; `Sprints remaining = remaining points /
   velocity` (rounded **up** — a partial Sprint is paid in full); `cost per Sprint = AC / Sprints elapsed`;
   `ETC = Sprints remaining × cost per Sprint`; `EAC = AC + ETC`.
3. **Substitution.** `Velocity = 210/6 = 35` points/Sprint; `Sprints remaining = 190/35 = 5.4 → ~6` more
   Sprints, so the release finishes around **Sprint 12** against the planned 10. `Cost per Sprint =
   780,000/6 = 130,000` — above the USD 120,000 funded capacity, because the team was enlarged early in the
   release. `ETC = 6 × 130,000 = 780,000`; `EAC = 780,000 + 780,000 = 1,560,000`.
4. **Result.** The run-rate view projects an `EAC` of **USD 1,560,000** — **USD 360,000** over budget — and a
   **two-Sprint (roughly one-month) overrun**, finishing Sprint 12 instead of Sprint 10.
5. **Interpretation.** This is capacity funding read forward (9.5.1): the team costs USD 130,000 per two-week
   Sprint whether it completes 35 points or 20, so the honest `ETC` is whole Sprints at the actual burn rate,
   not a fraction. Rounding 5.4 up to 6 is deliberate — the operator cannot buy 0.4 of a Sprint. Note also the
   two distinct signals inside this forecast: the *schedule* slip comes from velocity (35 achieved vs the 40
   points/Sprint the plan implied), while the *cost* pressure comes from the run-rate (130,000 vs 120,000
   funded). They compound.

### Reconciling the two forecasts

| View | Method | `EAC` | Finish | Key assumption |
|---|---|---|---|---|
| Forecast 1 | AgileEVM `EAC = BAC/CPI` | ≈ USD 1,485,700 | Behind plan (`SPI` 0.88) | Cost per value earned persists; scope/`BAC` fixed |
| Forecast 2 | Velocity + run-rate | USD 1,560,000 | Sprint 12 (~1 month late) | Whole-Sprint burn at USD 130,000; Sprints rounded up |

The two methods **bracket** the likely outcome at **≈ USD 1.49m–1.56m**, and the gap between them is
information, not error. AgileEVM extrapolates a *continuous* quantity — cost per unit of value earned — and so
implicitly allows the release to stop the moment the 400th point is done, mid-Sprint if need be. The run-rate
view extrapolates *whole-Sprint* burn with the Sprint count rounded up, recognising that the team is paid for
full Sprints: the tail end of Sprint 12 is funded even if the last points land early in it. That granularity —
partial Sprints are paid in full — is a real cash effect that AgileEVM's smooth arithmetic misses, which is
why the run-rate figure sits above the AgileEVM figure here.

The professional discipline is the one Domain 6 teaches for the `EAC` family (KA 6.3.3): **select methods
consciously, state each method's assumption, and report the range** rather than a falsely precise single
number. The report to the gate therefore reads: "forecast at completion **USD 1.49m–1.56m**, method
assumptions attached", and the range feeds the programme's rolling forecast (Domain 3) so that the corporate
cost picture moves with the release rather than discovering the overrun at the end.

### The governance conversation

At the gate (KA 9.6.2), the board hears the adaptive work translated into the language it governs by:

- **Value delivered to date:** 210 of 400 planned points — **52.5 %** of the release scope — is done to the
  Definition of Done and demonstrable as working software.
- **Forecast:** **USD 1.49m–1.56m** against the **USD 1.2m** release budget; completion moving from Sprint 10
  to **~Sprint 12**.
- **Decision options,** priced in scope terms because this is an inverted-triangle delivery (9.3.5) — time and
  cost are the fixed quantities, so the honest lever is scope:

| Option | What it means | Cost consequence |
|---|---|---|
| **(a) Fund the overrun** | Deliver all 400 points by ~Sprint 12 | **+USD 0.29m–0.36m** over `BAC` (`1,485,700 − 1,200,000 ≈ 285,700`; `1,560,000 − 1,200,000 = 360,000`) |
| **(b) Hold the budget and descope** | Remaining funds `1,200,000 − 780,000 = 420,000` buy **~3.5 Sprints** at the USD 120,000 funded capacity; at velocity 35 that is **~120 points** of the 190-point backlog | Within `BAC`; **~70 points dropped** |
| **(c) Stop at a viable increment** | Close the release on the working software already delivered | No further spend beyond an orderly close-out |

Under option (b), *which* ~120 points get built is the Product Owner's call, made by re-ordering the backlog —
MoSCoW's Must/Should before Could, with the ~70 dropped points explicitly recorded as Won't-haves for this
release (9.3.2). That is the inverted triangle working as designed: scope, not time or cost, absorbs the
variance, and the least valuable items drop first. (The board should also hear the sharper sub-caveat: at the
*actual* USD 130,000 burn rather than the funded 120,000, the remaining USD 420,000 buys nearer 3.2 Sprints —
option (b)'s ~120 points is the optimistic edge, which is exactly why the assumption is stated.)

The controls professional's job at this table is to make the trade-offs **explicit and priced — not to pick
for the board**. The board owns the value judgement; the professional owns the integrity of the numbers under
each option, and the audit trail of what was decided and why (9.6.4).

### What the credential expects

A candidate should be able to run this case unaided, because it exercises the domain's core knowledge areas in
one pass: computing **velocity and a Sprints-remaining forecast** and reading it as a burnup projection
(9.3.3–9.3.4); translating that into money through **run-rate capacity funding** (9.5.1); running **AgileEVM**
with the Domain 6 symbols unchanged and its rebaselining caveat stated (9.5.3); reporting both through
**hybrid stage-gate governance** so adaptive work is legible to a milestone-driven board (9.6.1–9.6.2); and
framing the decision options through the **inverted iron triangle**, with scope as the honest lever and the
Product Owner prioritising what remains (9.3.5, 9.3.2). The examinable habits are the quiet ones: rounding
Sprints **up** because capacity is bought whole; reporting a **range** with each method's assumption attached
rather than a single confident number; and keeping `EV`, `AC` and the scope baseline transparent so any later
rebaselining is visible. AI-assisted sprint-forecast models can generate this range from Sprint history in
seconds — but the professional owns the assumptions behind it (Domain 13, KA 13.5.6).

---

## Case study B — Domain 9: a regulated payments platform under quarterly gates (banking)

### Background

A retail bank is replacing its **payments platform** — ISO 20022 messaging, sanctions screening, fraud
monitoring — under the active scrutiny of its financial regulator. Delivery is adaptive: a Scrum team working
in **two-week Sprints** against a MoSCoW-prioritised backlog. Governance is not: the bank's risk function and
the regulator require **quarterly gates**, each releasing the next quarter's funding against an evidence pack —
hybrid stage-gate governance exactly as KA 9.6.1 describes, with the controls professional translating between
the Sprint cadence and the gate cadence (9.6.2). The release baseline:

| Parameter | Value |
|---|---|
| Release budget `BAC` | USD 4,800,000 |
| Planned scope | 800 story points |
| Planned duration | 24 two-week Sprints (four quarterly gates of 6 Sprints each) |
| Implied budget per point | USD 6,000 (`= 4,800,000 / 800`) |
| Funded capacity per Sprint | USD 200,000 (`= 4,800,000 / 24`) |

In a regulated environment the domain's quiet disciplines stop being optional: "done" must be provable to an
auditor, technical shortcuts are risk items, and any re-baselining must be transparent enough to show a
supervisor. This case follows the release through its first two gates.

### A Definition of Done that carries compliance evidence (Advanced 9.A.1)

The team's **Definition of Done** goes beyond "tested and integrated": every item must also have an
independent code review, a clean security scan, audit-trail logging verified, and a **compliance evidence
pack** — test results, approvals, traceability to the regulatory requirement — archived where internal audit
can find it. The DoD is the exit gate that makes every downstream metric mean something (9.A.1).

At the first quarterly gate, internal audit samples the "done" stories and finds that items totalling
**24 points** of the claimed work have no archived evidence packs. Under the DoD they are not done, and the
metrics are restated:

1. **Setup.** Sprints 1–6 claimed **216 points** (a reported velocity of 36/Sprint); **24 points** fail the
   evidence sample; `AC` at Sprint 6 = **USD 1,280,000**.
2. **Formula.** `EV = points done × budget per point`; `CPI = EV / AC`; `velocity = points done / Sprints`.
3. **Substitution.** Claimed: `EV = 216 × 6,000 = 1,296,000`; `CPI = 1,296,000 / 1,280,000 = 1.01`.
   Restated: points `= 216 − 24 = 192`; `EV = 192 × 6,000 = 1,152,000`; `CPI = 1,152,000 / 1,280,000 = 0.90`;
   velocity `= 192 / 6 = 32`.
4. **Result.** The release is not slightly ahead on cost (`CPI` 1.01) but meaningfully behind (**`CPI` 0.90**),
   at a true velocity of **32 points/Sprint**, not 36.
5. **Interpretation.** A weak "done" was flattering every metric — the agile analogue of `EV` inflated by
   optimistic progress claims (Domain 6, KA 6.1.2). The reverted items return to the backlog, the evidence
   discipline is fixed at source, and the team's DoD is placed under change control: any future amendment is
   disclosed like a change to an earning rule, because it silently redefines every velocity, burnup and
   AgileEVM figure downstream (9.A.1).

### The technical-debt drawdown decision (Advanced 9.A.2)

The debt register — kept beside the risk register — holds one dominant item: an **expedient adapter to the
legacy general ledger**, taken deliberately in Sprint 3 to hit a pilot date, recorded at the time with its
estimated repayment cost. It is now charging interest: defect rework traced to the adapter consumes about
**6 points of every Sprint** — a velocity tax of `6 × 6,000 = ` **USD 36,000 per Sprint** at budget value.
The second-quarter gate takes the drawdown decision, priced on both sides as 9.A.2 requires:

1. **Setup.** Repayment estimated at **24 points**, scheduled as ~6 points/Sprint across Sprints 9–12;
   interest ends when repayment completes; 12 funded Sprints (13–24) then remain.
2. **Formula.** `Cost = repayment points × budget per point`; `benefit = tax per Sprint × Sprints relieved`;
   `payback = repayment points / tax per Sprint`.
3. **Substitution.** Cost `= 24 × 6,000 = 144,000`; benefit `= 6 × 12 = 72` points `= 72 × 6,000 = 432,000`;
   payback `= 24 / 6 = 4` Sprints.
4. **Result.** Spending **24 points (USD 144,000)** of capacity now recovers **72 points (USD 432,000)** of
   taxed capacity across the remaining funded Sprints — net **+48 points**, breaking even four Sprints after
   repayment completes.
5. **Interpretation.** The debt was acceptable because it was *visible and deliberate* — taken for a dated
   reason, registered, and repaid by an explicit, priced decision recorded in the register (9.6.4). The same
   shortcut left invisible would have surfaced only as unexplained velocity decay and a forecast the board
   could not trust (9.A.2).

### The regulator's scope injection — AgileEVM re-baselined (KA 9.5.3)

At the end of **Sprint 12** the position is: **312 points done** (DoD-verified — Sprints 7–12 netted
20 points/Sprint, the raw ~32 less the ~6-point rework tax and ~6 points/Sprint of debt repayment);
`AC` = **USD 2,496,000** (an average burn of 208,000/Sprint — compliance engineers were added above the
funded rate); the original plan expected 400 points by now, so `SPI = 1,872,000 / 2,400,000 = 0.78` and
`CPI = 1,872,000 / 2,496,000 = 0.75`. Then the regulator lands new mandatory scope: enhanced
sanctions-screening coverage sized at **120 points**, with the board approving a funding uplift of
`120 × 6,000 = ` **USD 720,000**. The release is re-baselined transparently, on the record 9.T.2 prescribes:

| Parameter | Before injection | After re-baseline |
|---|---:|---:|
| Total planned scope | 800 points | 920 points |
| `BAC` | USD 4,800,000 | USD 5,520,000 |
| Points complete | 312 | 312 |
| `%` complete | 39.0 % | 33.9 % |
| `EV` | USD 1,872,000 | USD 1,872,000 |
| `AC` | USD 2,496,000 | USD 2,496,000 |
| `CPI` | 0.75 | 0.75 |
| `EAC = BAC / CPI` | USD 6,400,000 | USD 7,360,000 |

The mechanics are the 9.5.3b discipline: the **`EV` of work already done is untouched** (312 points at the
USD 6,000 per-point rate), `CPI` is therefore unchanged, `% complete` falls because the denominator grew, and
`EAC` rises because there is genuinely more to build — no pretence that the metrics were unaffected, and no
quiet absorption of 120 points into a fixed baseline. A velocity cross-check brackets the formula: with the
debt repaid and the tax gone, the team expects **~30 points/Sprint**, so the remaining `920 − 312 = 608`
points need `608 / 30 = 20.3 → ` **21 more Sprints** (rounded up — capacity is bought whole, 9.5.1), and
`ETC = 21 × 208,000 = 4,368,000` gives `EAC = 2,496,000 + 4,368,000 = ` **USD 6,864,000**. The gate hears a
**range — USD 6.86m–7.36m** — with each method's assumption attached: the `BAC/CPI` figure carries the
debt-and-rework-depressed history forward; the velocity figure assumes the post-repayment 30 points/Sprint
holds (KA 6.3.3 applied in agile clothing).

### Reporting through the quarterly gate (KA 9.6)

The gate pack renders the Sprint-level truth in the language the board and the supervisor govern by: value
delivered (**312 of 920 points — 33.9 % — done to a DoD whose evidence packs are archived and sampled**), the
forecast range against the uplifted envelope, and the decision options priced in scope terms, because this is
an inverted-triangle delivery (9.3.5). With one asymmetry: the injected 120 points are regulatory
**Must-haves**, so the flexible margin is the bank's own discretionary backlog — the Product Owner re-orders
so that any descope falls on the bank's 'Could' features, never the mandate. The audit trail behind the pack —
the DoD restatement, the debt register and drawdown decision, the re-baselining record — is precisely the
contemporaneous evidence 9.6.4 demands, and it is what lets two-week empiricism live comfortably under
quarterly regulatory governance: the team inspects and adapts every Sprint; the institution gets a defensible,
restatement-proof record every quarter.

### What the credential expects

This case is the domain's controls crux under regulatory pressure. From **Advanced 9.A.1**, the Definition of
Done as the integrity of the progress measure itself: compliance evidence inside the DoD, "done" sampled by
audit, and a restatement (`CPI` 1.01 → 0.90) that shows why a weak DoD is the agile route to a flattering
lie. From **Advanced 9.A.2**, technical debt as a controls object — registered, its interest quantified as a
velocity tax (6 points/Sprint; USD 36,000), and repaid through a priced drawdown decision with a four-Sprint
payback. From **KA 9.5.3**, AgileEVM re-baselined the only honest way: `EV` constant, `BAC` and scope moved
together, `EAC` restated, all on the record — plus the velocity cross-check and a **range reported with
assumptions**, Domain 6's method discipline transplanted whole. And from **KA 9.6**, hybrid governance made
to work: quarterly gates fed by Sprint-level evidence, the inverted triangle flexing only the scope that is
legally flexible. AI-assisted forecasting can produce the range, and evidence-pack tooling can flag the
missing compliance artefacts before audit does (Domain 13, KA 13.5.6) — but the DoD, the drawdown call and
the re-baselining conversation belong to the professional. **AI proposes, the professional disposes.**

---

## Executive perspective — Domain 9

**What the executive must hold onto.** Agile inverts the iron triangle: time and cost are fixed — a funded
cadence of Sprints — and **scope flexes** to fit, so "is it on the original scope?" is the wrong question;
the right one is what value the fixed capacity will deliver by the fixed date (KA 9.3). Cost control in
this world is a **run-rate**: the budget buys whole Sprints of a stable team, and the forecast is how many
Sprints the remaining work needs against how many the remaining money buys (KA 9.5). AgileEVM gives the
board the familiar `CPI`/`SPI`/`EAC` language — but its numbers hold only against a defined release scope
and `BAC`, and every deliberate scope change must be **rebaselined transparently**, not absorbed silently.

**Six questions to ask from the chair.**

1. What value will the fixed capacity deliver by the fixed date — and which items are explicitly dropping
   to the Won't-have list?
2. Is this forecast a range or a point, and what velocity assumptions bracket it?
3. When scope was added, was the `BAC` rebaselined transparently — or are we still reporting progress
   against the old total?
4. What is the actual burn per Sprint against the funded rate, and how many whole Sprints does the
   remaining budget really buy?
5. Is the burnup's total-scope line moving, and who approved each movement?
6. How do story-point `%` complete, cost-to-cost `%` and the billing position reconcile this period —
   and what explains the gaps?

**The traps at board level.**

- **Velocity treated as comparable — or as a target.** Story points are a relative, team-specific measure;
  comparing velocity across teams is meaningless, and setting it as a target simply inflates the estimates
  it is built from.
- **Hybrid programmes reported in two untranslated languages.** Earned value on the predictive scope and
  velocity on the adaptive scope must be reconciled into one programme position — or, worse, the agile work
  is forced into a false fixed baseline to look governable (KA 9.6).
- **The burndown that hides added scope.** A burndown to zero conceals a rising total; the burnup's moving
  scope line is what keeps "on track" honest when scope is deliberately flexed.
- **Smooth arithmetic missing whole-Sprint cash.** Capacity is bought in whole Sprints — a forecast that
  lets the release stop mid-Sprint understates the cash a partial final Sprint still costs.

**What good looks like.** Gates hear the adaptive work in the language they govern by — value delivered to
the Definition of Done, run-rate, and a ranged forecast with each method's assumption attached — without
the work being forced into a fixed-scope fiction. Scope change is routine and visible: the burnup's total
line moves, the `BAC` is rebaselined on the record, and the Product Owner's re-prioritisation shows which
value was traded away. The backlog, Sprint records and rebaselining decisions form a contemporaneous audit
trail that assurance can walk without translation. And when capacity and ambition diverge, the board
chooses between funding, descoping and stopping at a viable increment — with each option priced, because
scope is the honest lever.

---

## Calculation exercises — Domain 9

Work each exercise before reading its solution; every step uses only this domain's methods.

**Exercise 9.1** — A team's completed story points over its last five Sprints are **22, 26, 24, 25, 23**. The
remaining release backlog is **216 points**. Compute the average velocity and the likely Sprints remaining;
then express the forecast as a range using a pessimistic velocity of **20** and an optimistic velocity of
**27** points per Sprint.

**Solution 9.1.**

1. `Velocity = (22 + 26 + 24 + 25 + 23) / 5 = 120 / 5 = 24` points/Sprint (9.3.3).
2. Likely: `Sprints remaining = 216 / 24 = 9` Sprints.
3. Pessimistic (velocity 20): `216 / 20 = 10.8` → **11 Sprints** — round *up*, because capacity is bought in
   whole Sprints and the release cannot stop mid-Sprint.
4. Optimistic (velocity 27): `216 / 27 = 8` Sprints.

Report **8–11 Sprints, likely 9** — a range, not a false-precise point (9.3.3), because velocity is an
empirical measure that varies Sprint to Sprint.

**Exercise 9.2** — A team's velocity is **25 points per Sprint** and the initial release scope is **250
points**. By the end of **Sprint 6** the team has completed **150 points**, and **75 points of new scope**
are approved and added to the release. Compute the original forecast finish, the new total scope, the
remaining work and the new forecast finish Sprint.

**Solution 9.2.**

1. Original forecast `= 250 / 25 = 10` Sprints.
2. New total scope `= 250 + 75 = 325` points — the burnup's total-scope line steps up (9.3.4).
3. Remaining `= 325 − 150 = 175` points.
4. Sprints remaining `= 175 / 25 = 7` → new forecast finish **Sprint 13** (6 done + 7 remaining), versus
   Sprint 10 originally.

The burnup shows the three extra Sprints as a rising scope line; a burndown to zero would have hidden the
addition and reported a team "falling behind" a plan that no longer exists (9.3.3b).

**Exercise 9.3** — A release has a `BAC` of **USD 720,000** and **480 story points** planned over **10
Sprints**. At the end of **Sprint 5**, **216 points** are complete, the plan expected **240 points** done by
now, and `AC` is **USD 405,000**. Compute `% complete`, `EV`, `PV`, `CPI`, `SPI`, `EAC` and `VAC`.

**Solution 9.3.**

1. `% complete = 216 / 480 = 45 %` → `EV = 45 % × 720,000 = 324,000` (9.5.3).
2. `Planned % = 240 / 480 = 50 %` → `PV = 50 % × 720,000 = 360,000`.
3. `CPI = EV / AC = 324,000 / 405,000 = 0.80`; `SPI = EV / PV = 324,000 / 360,000 = 0.90`.
4. `EAC = BAC / CPI = 720,000 / 0.80 = 900,000`; `VAC = 720,000 − 900,000 = (180,000)`.

The release is over cost (`CPI` 0.80) and mildly behind (`SPI` 0.90), forecasting a **USD 180,000** overrun —
valid only against this defined release scope and `BAC`, and to be rebaselined transparently if scope flexes
(the 9.5.3 caveat).

**Exercise 9.4** — A release is baselined at `BAC` **USD 500,000** for **250 story points** (USD 2,000 per
point). At the end of Sprint 4, **100 points** are done and `AC` is **USD 250,000**. Now **50 points of new
scope** are approved, with the per-point rate held. Compute the new `BAC`, the `% complete` before and after,
`CPI`, and the `EAC` before and after the rebaseline.

**Solution 9.4.**

1. `EV = 100 × 2,000 = 200,000`; `CPI = 200,000 / 250,000 = 0.80`. Before the change: `% complete =
   100 / 250 = 40 %`; `EAC = 500,000 / 0.80 = 625,000`.
2. Rebaseline (9.5.3b): new total points `= 250 + 50 = 300`; `new BAC = 300 × 2,000 = 600,000`.
3. New `% complete = 100 / 300 = 33.3 %` — lower, though nothing has been un-done.
4. New `EAC = 600,000 / 0.80 = 750,000`.

The **`EV` of work already done is unchanged at 200,000** and `CPI` stays 0.80 — adding scope does not change
the value or efficiency of past work; it raises `BAC` and `EAC` because there is genuinely more to build, and
the rebaseline puts that on the record instead of hiding it.

**Exercise 9.5** — A delivery board carries **24 items** of work in progress, and measured throughput is
**8 items per week**. Time-stamped board data shows an average of **4 working days** of active work ("touch
time") per item; use a 5-day working week. (a) Apply Little's Law to find the average cycle time. (b) Compute
the flow efficiency and interpret it. (c) The team halves WIP to **12 items** with throughput unchanged:
state the new cycle time and — the controls point — what does *not* improve automatically.

**Solution 9.5.**

1. (a) `Cycle time = WIP / throughput = 24 / 8 = 3` weeks, i.e. **15 working days** (Little's Law, 9.4.1).
2. (b) `Flow efficiency = touch time / cycle time = 4 / 15 ≈ 27 %` — nearly three-quarters of each item's
   life is spent queuing, not being worked (Advanced 9.A.3).
3. (c) New cycle time `= 12 / 8 = 1.5` weeks — cycle time halves and feedback arrives twice as fast.
4. What does not improve: **throughput does not rise** — it is still 8 items/week.

Cutting WIP buys speed of learning and earlier value, not more output — the improvement target the flow
efficiency exposes is the *wait*, not the workers (Advanced 9.A.3) — and any business case claiming that
halving WIP doubles delivery has confused cycle time with throughput (9.4.1).

---

## Practitioner's toolkit — Domain 9

*Adoption-ready artefacts; adapt the column headings and thresholds to your organisation, then keep them
stable.*

### Toolkit 9.T.1 — Sprint/release health dashboard spec

| Metric | Definition | Healthy looks like | Source KA |
|---|---|---|---|
| Velocity trend | Average story points completed per Sprint over the last N Sprints, plotted period on period | Stable within a band; never a target, never compared across teams | 9.3.3 |
| Burnup vs scope line | Completed points rising against the (possibly moving) total-scope line | Completed line converging on the scope line; every scope-line step authorised and dated | 9.3.4 |
| Flow efficiency | Touch time ÷ total cycle time from time-stamped board data | Rising trend, with wait time (not touch time) the improvement target | Advanced 9.A.3 |
| DoD compliance | Share of sampled "done" items genuinely meeting the Definition of Done | 100 % of the sample; any DoD change disclosed like an earning-rule change | Advanced 9.A.1 |
| AgileEVM `CPI`/`SPI` | `EV/AC` and `EV/PV` on story-point % against the release `BAC` | Both near 1 and stable; rebaselined transparently whenever scope flexes | 9.5.3 |
| Dependency ageing | How long unresolved items sit on the cross-team dependency board | Short and falling; no dependency older than one Sprint at programme cadence | Advanced 9.A.4 |

**Usage note.** The six metrics cover the three ways adaptive progress lies: inflated "done" (a weak DoD
flatters velocity, the burnup and `EV` at once, 9.A.1), hidden scope movement (the burnup's moving total
line is what a burndown conceals, 9.3.4), and queueing that no per-team measure sees (flow efficiency and
dependency ageing are the leading indicators, 9.A.3–9.A.4). Read velocity as each team's private planning
input, never an aggregation unit — programme-level reporting uses feature burnup and programme units
(9.A.4). The AgileEVM row is only meaningful against a defined release scope and `BAC`; when the scope line
steps, the rebaselining record (Toolkit 9.T.2) must step with it.

### Toolkit 9.T.2 — Rebaselining record template

| Date | Points added/removed | New total points | New `BAC` (USD) | Rate held? | `EV` of done work (USD, unchanged) | New % complete | New `EAC` (USD) | Approved by |
|---|---:|---:|---:|---|---:|---:|---:|---|
| End Sprint 5 | +60 | 360 | 720,000 | Yes — USD 2,000/point | 240,000 | 33.3 % | 960,000 | Product Owner & programme board |

**Usage note.** One row per deliberate scope change, written the moment the change is approved — the row
above echoes worked example 9.5.3b, where 60 added points move the `BAC` from 600,000 to 720,000 and the
`EAC` to 960,000 while the `EV` of completed work stays at 240,000 and `CPI` holds at 0.75. The two
invariant columns are the discipline: the `EV` of done work never changes (adding scope does not un-do
anything), and the "rate held?" column makes any re-pricing of points explicit rather than silent (9.5.3's
caveat). The record is what keeps a falling % complete honest — 40 % to 33.3 % with nothing un-done reads
as failure unless the scope movement is on the record. Together with the burnup's stepped scope line
(9.3.4) it forms the contemporaneous audit trail hybrid governance relies on (9.6.4).

---

## Exam preparation — Domain 9

**How this domain is examined.** Domain 9 tests Scrum vocabulary at recall level (accountabilities, events,
artefacts and their commitments), flow reasoning at analysis level (burnup versus burndown, cumulative flow,
WIP limits), and concentrates its numerical items in **velocity and release forecasting** (KA 9.3) and
**AgileEVM with run-rate funding** (KA 9.5). Expect at least one rebaselining calculation and one
reconciliation item at the IFRS 15 boundary. The recurring theme is honesty of measurement — ranges over
points, transparent scope movement, and the standing AgileEVM caveat.

**Calculation traps.**

- **Comparing or summing velocity across teams.** Story points are a relative, team-specific measure; a
  cross-team velocity figure has no meaning, and setting velocity as a target inflates the estimates it is
  built from (Advanced 9.A.4).
- **Forecasting from the original scope after scope changed.** Recompute against the *new* total scope minus
  completed work; distractors use the original total, or divide the whole new scope by velocity and forget
  the points already done (MCQ 9.3-D; worked example 9.3.3b).
- **Getting rebaselining wrong.** Adding scope raises `BAC` and `EAC` and lowers `% complete`, but the `EV`
  of done work and the `CPI` are unchanged — nothing has been un-done (worked example 9.5.3b; Exercise 9.4).
- **Treating story-point `%` as IFRS 15 progress.** Story-point completion is a proxy for effort/scope;
  revenue follows the appropriate input basis (usually cost-to-cost), with the difference reconciled and
  explained, never averaged (MCQ 9.5-C).
- **`EV` from the wrong points.** `EV` = points *completed* ÷ total planned points × `BAC`; the
  planned-points version is `PV`, and `% complete × AC` values progress at actual cost — the cardinal EVM
  error (MCQ 9.5-E).
- **Fractional Sprints and forgotten caps.** Round Sprints-remaining *up*, because capacity is bought in
  whole Sprints (Exercise 9.1); and under capped T&M the client pays no more than the cap (MCQ 9.6-C).

**Time management.** Velocity arithmetic is fast; the marked skill is the setup — new total scope, remaining
points, whole Sprints. Reserve extra time for AgileEVM items that chain `EV → CPI → EAC`, and for any stem
containing the word "rebaselined", which usually changes two figures and deliberately leaves two unchanged.

**Reflection questions.**

1. Where in your organisation is velocity being compared across teams or set as a target — and what is that
   doing to the estimates beneath it?
2. If scope were added to your current release tomorrow, would the `BAC` be rebaselined on the record or
   absorbed silently?
3. How strong is your teams' Definition of Done, and what would sampling "done" items against it reveal
   about the `EV` you report?
4. Can you reconcile story-point progress, cost-to-cost progress and the billing position on an adaptive
   contract you know — and explain each gap?

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

