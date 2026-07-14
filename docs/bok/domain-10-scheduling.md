# Domain 10 — Project Scheduling (in depth)

> **Group:** Project management. **Target:** ~65 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). British English; USD (+SAR where useful). Complements
> the schedule side of earned value (Domain 6, especially the critical-path limitation, KA 6.4.2).

## Why this domain exists

A schedule is the model of *how the work will happen in time* — and it is the half of project controls that
earned value, on its own, cannot see (Domain 6, KA 6.4.2). A controls professional must be able to build a
logic-driven schedule, find its **critical path**, compute **float**, compress it when needed, and control it
against a baseline. This domain covers schedule development — activities, dependencies, durations (KA 10.1);
**network analysis and the Critical Path Method** — the forward/backward pass, float and the critical path
(KA 10.2, worked end-to-end); schedule compression and resourcing — crashing, fast-tracking, levelling, and
schedule risk (KA 10.3); and progress measurement and schedule control, including how classical scheduling
relates to agile cadence (KA 10.4).

**Learning objectives.** After this domain a candidate can: define activities, sequence them with the four
dependency types and leads/lags, and estimate durations (including PERT); run a forward and backward pass to
find early/late dates, total and free float, and the critical path; apply crashing and fast-tracking and
explain their trade-offs, and describe resource levelling/smoothing and schedule-risk analysis; and measure
schedule progress and control it against a baseline, relating it to agile cadence.

---

## Knowledge Area 10.1 — Schedule development

*Topics: 10.1.1 activity definition · 10.1.2 sequencing and dependency types · 10.1.3 leads and lags · 10.1.4
estimating durations (incl. PERT).*

### 10.1.1 Activity definition

**Definition & purpose.** Scheduling begins by decomposing the **work packages** of the WBS (Domain 8, KA
8.2.1) into **activities** — the units of work that are sequenced and durated. An activity is small enough to
estimate and manage, large enough not to swamp the schedule with detail. Well-defined activities, traceable to
the WBS, are what make the schedule integrate with cost (control accounts, Domain 5) and earned value.

### 10.1.2 Sequencing and dependency types

**The principle.** Activities are linked by **logical dependencies** that define what must precede what. There
are four types, defined by which ends are tied:

| Dependency | Meaning |
|---|---|
| **Finish-to-Start (FS)** | B starts after A finishes (the default, most common) |
| **Start-to-Start (SS)** | B starts after A starts (parallel with an offset) |
| **Finish-to-Finish (FF)** | B finishes after A finishes |
| **Start-to-Finish (SF)** | B finishes after A starts (rare) |

Dependencies may be **mandatory** ("hard logic" — concrete must cure before loading), **discretionary**
("preferred" sequencing), or **external** (a permit, a client decision). Sound logic — not date constraints —
is what makes a schedule *dynamic*: change one duration and the network recalculates.

### 10.1.3 Leads and lags

**The principle.** A **lag** is a delay on a dependency (B starts 3 days *after* A finishes: FS + 3); a
**lead** is an overlap (B starts 2 days *before* A finishes: FS − 2, a form of fast-tracking, 10.3). Leads and
lags model reality (curing time, mobilisation) but should represent genuine logic, not be used to force dates —
hidden lags are a common way schedules are quietly manipulated.

**Worked example 10.1.3 — dates under an SS + lag.**

- **Setup:** activity **A** (duration **10 days**, starts day 0) drives activity **B** (duration **6 days**)
  through a **Start-to-Start + 4** link — B may start 4 days after A starts, e.g. following A's first
  completed section.
- **Formula:** forward pass under an SS link: `B.ES = A.ES + lag`; `B.EF = B.ES + duration`.
- **Substitution:** `B.ES = 0 + 4 = 4`; `B.EF = 4 + 6 = 10`. A finishes day 10 too.
- **Result:** the fragment completes on **day 10** — four days earlier than the FS sequence (A then B:
  `10 + 6 = 16`).
- **Interpretation:** SS links model genuine overlap and buy time without crashing — but they also mean B
  depends on A's **rate** of progress, not just its start; if A's first section is late, B follows. An SS + lag
  is the controlled form of fast-tracking (cross-ref 10.3.2).

### 10.1.4 Estimating durations (including PERT)

**The principle.** Activity **durations** are estimated from the work quantity, the assigned resources and
productivity. Where duration is uncertain, **three-point (PERT)** estimation captures the range:

```
Expected duration  tE = (O + 4M + P) / 6
Standard deviation  σ = (P − O) / 6
```
- `O`, `M`, `P` — optimistic, most-likely, pessimistic durations (time units).

**Worked example 10.1.4 — PERT expected duration.** For an activity with `O = 4`, `M = 6`, `P = 14` days:
`tE = (4 + 4×6 + 14)/6 = (4 + 24 + 14)/6 = 42/6 = ` **7 days**; `σ = (14 − 4)/6 = 1.67` days. The pessimistic
tail (14 vs a most-likely 6) pulls the expected duration to 7, above the most-likely — the value of a
three-point estimate over a single guess.

### Key terms — KA 10.1

| Term | Meaning |
|---|---|
| **Activity** | The unit of work sequenced and durated, traceable to the WBS. |
| **Dependency (FS/SS/FF/SF)** | The four logical relationships between activities. |
| **Lead / lag** | An overlap / a delay on a dependency. |
| **PERT (three-point)** | `tE = (O + 4M + P)/6`; `σ = (P − O)/6`. |

### Sample MCQs — KA 10.1

**MCQ 10.1-A `[10.1.4 · Application]`** With `O = 4`, `M = 6`, `P = 14`, the PERT expected duration is:
- A. 6 days
- B. 7 days ✅
- C. 8 days
- D. 24 days

*Rationale:* `(4 + 4×6 + 14)/6 = 42/6 = 7`. A is the most-likely; C misweights; D forgets to divide.

**MCQ 10.1-B `[10.1.2 · Recall]`** "B cannot start until A finishes" is which dependency?
- A. Start-to-Start
- B. Finish-to-Finish
- C. Finish-to-Start ✅
- D. Start-to-Finish

*Rationale:* Finish-to-Start (the default). SS ties starts; FF ties finishes; SF is the rare start-to-finish.

**MCQ 10.1-C `[10.1.3 · Application]`** Activity A finishes at the end of day 10. Its successor B is linked
**FS + 3 days** (a lag for curing time). B's earliest start is:
- A. Day 10
- B. Day 7
- C. Day 3
- D. Day 13 ✅

*Rationale:* An FS + 3 lag delays the successor three days beyond the predecessor's finish: `10 + 3 = 13`. A
ignores the lag; B treats the lag as a lead (FS − 3); C reads the lag itself as the start date.

**MCQ 10.1-D `[10.1.2 · Analysis]`** When fast-tracking a schedule, which dependency may legitimately be
relaxed or overlapped?
- A. A mandatory dependency (e.g. concrete curing before loading).
- B. A discretionary dependency reflecting preferred sequencing. ✅
- C. An external dependency (e.g. a permit).
- D. None — all dependencies are equally fixed.

*Rationale:* Discretionary ("preferred") logic is the legitimate target for overlap because nothing physical
enforces it. Mandatory logic is a physical constraint that cannot be relaxed; an external dependency is outside
the team's control; D ignores the distinction the three categories exist to make.

### Self-check — KA 10.1

1. Name the four dependency types and which is most common. *(FS, SS, FF, SF; FS.)*
2. Why prefer logic links over hard date constraints? *(Logic makes the schedule dynamic — it recalculates when
   a duration changes; constraints freeze dates and hide slippage.)*

---

## Knowledge Area 10.2 — Network analysis and the Critical Path Method *(worked in full)*

*Topics: 10.2.1 the network · 10.2.2 the forward pass (early dates) · 10.2.3 the backward pass (late dates) ·
10.2.4 total and free float · 10.2.5 the critical path.*

### 10.2.1 The network

**Definition & purpose.** A **network** (precedence diagram) shows activities as nodes linked by dependencies.
Analysing it by the **Critical Path Method (CPM)** finds the **longest path** through the network — which
determines the **shortest possible project duration** — and the **float** on every other activity. CPM is the
backbone of predictive scheduling and the thing earned value must be read alongside (Domain 6, KA 6.4.2).

**The worked network.** Six activities (durations in days):

| Activity | Duration | Predecessor(s) |
|---|---:|---|
| A | 3 | — (start) |
| B | 4 | A |
| C | 2 | A |
| D | 5 | B |
| E | 3 | C |
| F | 2 | D, E (finish) |

### 10.2.2 The forward pass — early dates

**Method.** Working left to right, `ES` (early start) of an activity = the latest `EF` of its predecessors;
`EF = ES + duration`. Start at time 0.

| Activity | `ES` | `EF` (= ES + dur) |
|---|---:|---:|
| A | 0 | 3 |
| B | 3 | 7 |
| C | 3 | 5 |
| D | 7 | 12 |
| E | 5 | 8 |
| F | max(12, 8) = 12 | 14 |

**Project duration = 14 days** (the `EF` of the final activity F).

### 10.2.3 The backward pass — late dates

**Method.** Working right to left from the project finish (`LF` of F = 14), `LF` of an activity = the earliest
`LS` of its successors; `LS = LF − duration`.

| Activity | `LF` | `LS` (= LF − dur) |
|---|---:|---:|
| F | 14 | 12 |
| D | 12 | 7 |
| E | 12 | 9 |
| B | 7 | 3 |
| C | 9 | 7 |
| A | min(3, 7) = 3 | 0 |

### 10.2.4 Total and free float

**Formulae.**
```
Total float (TF) = LS − ES  (= LF − EF)   — delay available without delaying the PROJECT
Free float  (FF) = min(ES of successors) − EF — delay available without delaying any SUCCESSOR
```

| Activity | `TF` | `FF` |
|---|---:|---:|
| A | 0 | 0 |
| B | 0 | 0 |
| C | 7 − 3 = **4** | 5 − 5 = **0** |
| D | 0 | 0 |
| E | 9 − 5 = **4** | 12 − 8 = **4** |
| F | 0 | 0 |

**Interpretation.** Activities with **zero total float** cannot slip without delaying the project — they are
**critical**. Note C: it has 4 days of *total* float but **0 free** float — delaying C uses up float that E
then relies on, so C's slack is shared, not private. This total-vs-free distinction is exactly what a controls
professional needs to answer "can this activity slip, and who does it hurt?"

### 10.2.5 The critical path

**The result.** The **critical path** is the chain of zero-float activities — the longest path — here **A → B
→ D → F**, length `3 + 4 + 5 + 2 = 14` days, equal to the project duration. Any delay on a critical activity
delays the whole project day-for-day; managing the critical path is therefore where schedule attention
concentrates.

> **Fig 10.2.1 — The activity network and critical path.** *Caption:* early/late dates, float and the critical
> path. *Underlying data:* the tables above. *Render-ready description:* six nodes A–F laid left to right with
> dependency arrows (A→B, A→C, B→D, C→E, D→F, E→F); each node shows `ES|EF` (top) and `LS|LF` (bottom) and its
> `TF`; the critical chain A–B–D–F drawn as a bold brand-blue path, non-critical C and E in grey with their
> float annotated. *Animation storyboard (digital-only):* the forward pass sweeps left-to-right filling `ES/EF`;
> the backward pass sweeps right-to-left filling `LS/LF`; float is computed per node; finally the zero-float
> path highlights as the critical path.

### Key terms — KA 10.2

| Term | Meaning |
|---|---|
| **Forward / backward pass** | Compute early dates (`ES/EF`) / late dates (`LS/LF`). |
| **Total float (`TF`)** | Slack without delaying the project (`LS − ES`). |
| **Free float (`FF`)** | Slack without delaying any successor. |
| **Critical path** | The longest, zero-float chain; sets the project duration. |

### Sample MCQs — KA 10.2

**MCQ 10.2-A `[10.2.5 · Application]`** In the worked network (A3, B4, C2, D5, E3, F2; A→B→D→F, A→C→E→F), the
project duration is:
- A. 12 days
- B. 10 days
- C. 14 days ✅
- D. 8 days

*Rationale:* The longest path A–B–D–F = `3+4+5+2 = 14`. The A–C–E–F path is `3+2+3+2 = 10`; the project takes
the longer, 14.

**MCQ 10.2-B `[10.2.4 · Analysis]`** Activity C has total float 4 but free float 0. This means delaying C:
- A. Delays the project by 4 days.
- B. Delays its successor E (uses shared float), but not the project — up to the limit. ✅
- C. Has no effect at all.
- D. Is impossible.

*Rationale:* Free float 0 means any delay to C immediately eats into the float E relies on; total float 4 means
the project has 4 days' buffer overall, but that slack is shared along the C–E path. It does not delay the
project (until the 4 days are used) and is certainly possible.

**MCQ 10.2-C `[10.2.4 · Application]`** Activity D has `ES` = 7 and `LS` = 7. Its total float is:
- A. 7
- B. 5
- C. 14
- D. 0 ✅

*Rationale:* `TF = LS − ES = 7 − 7 = 0` — D is critical. The other values confuse dates with float.

**MCQ 10.2-D `[10.2.4 · Application]`** An activity has `EF` = 14; its two successors have `ES` of 17 and 19.
Its free float is:
- A. 3 days ✅
- B. 5 days
- C. 0 days
- D. 4 days

*Rationale:* `FF = min(ES of successors) − EF = min(17, 19) − 14 = 17 − 14 = 3` days. B wrongly uses the later
successor (19); C assumes the activity is critical; D averages the two successor dates.

**MCQ 10.2-E `[10.2.3 · Recall]`** In the backward pass, an activity's `LF` equals:
- A. The earliest `LS` of its successors. ✅
- B. The latest `LS` of its successors.
- C. The latest `EF` of its predecessors.
- D. The project start date.

*Rationale:* Working right to left, an activity must finish in time for its most demanding successor, so `LF =`
the **earliest** `LS` among successors, and `LS = LF − duration`. B would let a successor start late; C is the
forward-pass rule for `ES`; D confuses the two ends of the network.

### Self-check — KA 10.2

1. Give the formulae for total and free float and the difference in what each protects. *(`TF = LS − ES`
   (project); `FF = min successor ES − EF` (successor).)*
2. What defines the critical path and why does it set the project duration? *(The longest, zero-float chain; any
   slip on it delays the whole project.)*

---

## Knowledge Area 10.3 — Schedule compression and resourcing

*Topics: 10.3.1 crashing · 10.3.2 fast-tracking · 10.3.3 resource levelling and smoothing · 10.3.4 schedule
risk (PERT/Monte Carlo).*

### 10.3.1 Crashing

**Definition & purpose.** **Crashing** shortens the schedule by adding resources to **critical** activities,
choosing those with the **lowest cost per time saved**. It trades **cost for time** and only helps on the
critical path (crashing a non-critical activity just adds cost and float). Crashing has diminishing returns and
can shift the critical path to another chain.

**Worked example 10.3.1 — crash the critical path.** In the worked network, activity **B** (on the critical
path) can be crashed from **4 to 2 days** at **USD 5,000/day**. Crashing B by 2 days costs `2 × 5,000 =
10,000` and shortens the project from 14 to **12 days** — *provided* A–B–D–F remains the longest path. Check:
the A–C–E–F path is 10 days, so shortening the critical path to 12 keeps it critical (12 > 10) — the crash is
effective. If the parallel path had been 13, crashing B by 2 would only save 1 day before that path became
critical.

**Worked example 10.3.1b — choosing the cheapest crash sequence.**

- **Setup:** to shorten the 14-day project (critical path A–B–D–F; parallel path A–C–E–F = 10 days), crash
  costs on the critical activities are: **B USD 5,000/day (max 2 days), D USD 8,000/day (max 3 days), F USD
  12,000/day (max 1 day)**. Target: shorten by **3 days** to 11 days.
- **Formula:** crash the **cheapest** critical day first, re-checking the parallel path does not become
  critical.
- **Substitution:** crash **B by 2 days** at 5,000/day = **10,000** (14 → 12 days); then crash **D by 1 day**
  at 8,000/day = **8,000** (12 → 11 days). Parallel path is still 10 days (< 11), so A–B–D–F stays critical.
- **Result:** 3 days saved for **USD 18,000** — the first 2 days at 5,000/day, the third at 8,000/day (rising
  marginal cost). Crashing F (12,000/day) is avoided as the most expensive.
- **Interpretation:** crashing follows the **marginal cost** of time, cheapest first, while watching for the
  critical path shifting to the parallel chain (here it would at 10 days). This is how a controls professional
  compresses a schedule at least cost (cross-ref 10.3.1).

**Worked example 10.3.3 — resource levelling extends a duration.**

- **Setup:** two 4-day activities, **X and Y**, were planned in **parallel** but both need the **same single
  specialist crew** (only one available).
- **Formula:** with one crew they must run **in sequence**; the added duration is the second activity's
  duration, constrained by available float.
- **Substitution:** X then Y in sequence adds up to **4 days** of work that cannot overlap; X has 4 days of
  total float, so if Y is the constrained one, part of the delay is absorbed by float, but
  any excess pushes the finish out.
- **Result:** resource levelling can **extend the project duration** and create a **resource-critical path**
  distinct from the logical critical path when float is exhausted.
- **Interpretation:** a schedule that ignores resource limits is optimistic fiction; levelling (respecting
  limits, may extend) and smoothing (within float, no extension) make it deliverable (cross-ref 10.3.3).

### 10.3.2 Fast-tracking

**Definition & purpose.** **Fast-tracking** shortens the schedule by **overlapping** activities normally done
in sequence (using leads, 10.1.3) — e.g. starting construction before design is fully complete. It trades
**time for risk**: overlapping increases the chance of rework if the earlier activity changes. Fast-tracking
costs little directly but raises risk; crashing costs money but keeps the logic. The choice depends on whether
money or risk tolerance is scarcer.

### 10.3.3 Resource levelling and smoothing

**The principle.**

- **Resource levelling** adjusts the schedule to respect **resource limits** (only three crews available), and
  **may extend the project duration** and change the critical path (a "resource-critical" path emerges).
- **Resource smoothing** adjusts activities **within their float** to even out resource peaks **without**
  extending the duration.

A schedule that ignores resource limits is optimistic fiction; levelling and smoothing make it deliverable.

### 10.3.4 Schedule risk (PERT / Monte Carlo)

**The principle.** Because durations are uncertain (10.1.4), the *deterministic* critical path understates
completion risk — especially where **near-critical** paths could become critical if their activities slip.
**Schedule-risk analysis** models duration uncertainty across the network — conceptually via **Monte Carlo
simulation** (run the network thousands of times with sampled durations to get a **distribution** of completion
dates and a **probability** of meeting a target) — giving a P50/P80 completion date rather than a single
deterministic one. This is the schedule analogue of contingency in cost (Domains 3, 12) and where much of AI's
scheduling value lies (below).

### Key terms — KA 10.3

| Term | Meaning |
|---|---|
| **Crashing** | Add resources to critical activities (cost for time). |
| **Fast-tracking** | Overlap sequential activities (time for risk). |
| **Resource levelling / smoothing** | Respect resource limits (may extend) / even peaks within float (no extension). |
| **Schedule-risk analysis** | Model duration uncertainty (Monte Carlo) for a completion distribution. |

### Sample MCQs — KA 10.3

**MCQ 10.3-A `[10.3.1 · Analysis]`** Crashing is most effective when applied to:
- A. Any activity with float.
- B. The longest-duration activity regardless of path.
- C. Critical-path activities with the lowest cost per time saved. ✅
- D. Non-critical activities.

*Rationale:* Only critical-path activities shorten the project; among those, pick the cheapest per time saved.
Crashing float/non-critical activities adds cost without shortening the project.

**MCQ 10.3-B `[10.3.2 · Analysis]`** Fast-tracking primarily trades:
- A. Cost for time.
- B. Quality for schedule.
- C. Scope for cost.
- D. Time for risk (overlapping raises rework risk). ✅

*Rationale:* Fast-tracking overlaps sequential work, saving time but increasing rework risk. Crashing (not
fast-tracking) trades cost for time.

**MCQ 10.3-C `[10.3.3 · Recall]`** Which technique may extend the project duration?
- A. Resource smoothing.
- B. Resource levelling. ✅
- C. Fast-tracking.
- D. Crashing.

*Rationale:* Levelling respects hard resource limits and may extend the schedule; smoothing works within float
(no extension); fast-tracking and crashing shorten it.

**MCQ 10.3-D `[10.3.1 · Application]`** A project's critical path is 20 days; the parallel path is 17 days.
Two critical activities can be crashed: **X** at USD 3,000/day (max 2 days) and **Y** at USD 7,000/day (max 2
days). The least-cost way to save **2 days** is:
- A. Crash X by 2 days for USD 6,000. ✅
- B. Crash Y by 2 days for USD 14,000.
- C. Crash X and Y by 1 day each for USD 10,000.
- D. Crash X by 1 day for USD 3,000.

*Rationale:* Cheapest critical day first: `2 × 3,000 = 6,000`, and the new 18-day path still exceeds the 17-day
parallel path, so both crashed days are effective. B and C buy the same 2 days at higher cost; D saves only 1
day, missing the target.

**MCQ 10.3-E `[10.3.4 · Analysis]`** A Monte Carlo schedule-risk analysis returns **P50 = 30 days** and
**P80 = 33 days** against a deterministic duration of 30 days. The professional posture is to:
- A. Commit externally at 33 days, manage internally to 30, and hold the 3-day gap as explicit schedule contingency. ✅
- B. Commit externally at 30 days, since that is the deterministic answer.
- C. Commit externally at 27 days to motivate the team.
- D. Ignore the simulation — the critical path is already known.

*Rationale:* The deterministic date is only about a coin toss (P50), so the external commitment is made at the
higher-confidence P80 while the team manages to the aggressive date, with the difference held as owned
contingency. B commits to a 50/50 outcome; C commits to a date *less* likely than the coin toss; D discards
exactly the uncertainty the analysis quantifies.

### Self-check — KA 10.3

1. Contrast crashing and fast-tracking by what each trades. *(Crashing — cost for time; fast-tracking — time
   for risk.)*
2. Why does a deterministic critical path understate completion risk? *(It ignores duration uncertainty and
   near-critical paths; Monte Carlo gives a completion distribution/probability.)*

---

## Knowledge Area 10.4 — Progress measurement and schedule control

*Topics: 10.4.1 updating and progressing the schedule · 10.4.2 schedule variance and baseline comparison ·
10.4.3 relating classical scheduling to agile cadence.*

### 10.4.1 Updating and progressing the schedule

**The principle.** A schedule is controlled by **progressing** it each period: recording actual start/finish
dates and remaining durations at the **data date**, then recalculating the network. Progressing reveals the
**current** critical path (which may have moved) and the forecast completion. Discipline matters: out-of-sequence
progress, missing actuals and unjustified constraint changes corrupt the forecast — the schedule equivalent of
the data-integrity issues in cost (Domain 5, KA 5.2.4).

**Worked example 10.4.1 — out-of-sequence progress, two answers two days apart.**

- **Setup:** activity **C** is linked **FS after B**. At the data date, B's forecast finish is **day 12**, but
  C has *already* started (day 10) — out-of-sequence progress. C has **4 days** of work remaining.
- **Formula:** two scheduling conventions give different forecasts — **retained logic** holds C's remaining
  work until B finishes; **progress override** lets C continue immediately.
- **Substitution:** retained logic: C finishes `12 + 4 = ` **day 16**; progress override: C finishes
  `10 + 4 = ` **day 14**.
- **Result:** the two answers differ by **2 days** `(16 − 14)` — from a software setting, not from the work.
- **Interpretation:** neither convention is "true" — the question is physical: can C genuinely continue without
  B? The planner resolves the logic (often by splitting C or correcting the link) rather than letting a software
  setting silently decide the forecast; unexplained out-of-sequence updates are a schedule health-check item
  (Advanced 10.A.1).

### 10.4.2 Schedule variance and baseline comparison

**The principle.** Schedule performance is measured by comparing the **progressed** schedule to the
**baseline**: the movement of key milestones, the current vs baseline critical path, and — in earned-value
terms — the schedule variance `SV` and index `SPI` (Domain 6), with **earned schedule** (Domain 6, KA 6.4.3)
giving a time-based measure that stays meaningful late in the project. A controls professional reads *both* the
network view (which activities and which path) and the earned-value view (how much, in aggregate), because each
covers the other's blind spot: EVM does not see the critical path; the network does not aggregate cost.

### 10.4.3 Relating classical scheduling to agile cadence

**The bridge.** Classical scheduling and agile cadence are not opposites — they are two ways of expressing
*time-phased delivery*. Sprints and releases (Domain 9) are **schedule increments**: a release plan maps to
milestones (Domain 9, KA 9.6.2), and velocity-based forecasting (Domain 9, KA 9.5) is the adaptive analogue of
critical-path completion forecasting. On a **hybrid** programme the controls professional runs a CPM schedule
for the predictive scope and a cadence-based forecast for the adaptive scope, and **reconciles both to the same
milestones** so the programme has one time picture.

**AI in this KA.** Scheduling is a strong AI use case (Domain 13, KA 13.5): AI-assisted schedule generation and
logic-checking (finding missing links, dangling activities, excessive constraints/lags), delay prediction from
progress trends and external data, and accelerating Monte Carlo risk analysis. The professional owns the logic
and the forecast — an AI-proposed schedule can embed hidden constraints or unrealistic durations, and a delay
prediction is an input to judgement, not a decision. **AI proposes, the professional disposes.**

### 10.4.4 Look-ahead planning

**The principle.** The CPM baseline steers the *project*; the **look-ahead schedule** — a rolling two-, four-
or six-week window extracted from it — steers the *work*. Each cycle, the window's activities are screened
for **make-ready constraints** — design information, materials on site, access, permits, crews, predecessor
completion — and only constraint-free work is committed to the field. This is **short-interval production
planning**: the bridge between the network and what crews actually do this week, and the discipline
(associated with lean-construction practice, described here in this book's own words) that stops the site
improvising its own sequence. It complements, not replaces, progressing (KA 10.4.1) and the schedule-quality
checks of Advanced 10.A.1.

**Worked example 10.4.4 — a four-week look-ahead cycle.**

1. **Setup.** The four-week window holds **24 activities**. Constraint screening finds **6** not make-ready
   (2 awaiting design revisions, 3 awaiting materials, 1 awaiting access), leaving `24 − 6 = ` **18
   committed** to the field. At the next cycle, **15** of the 18 are complete.
2. **Formula.** `commitment reliability = completed ÷ committed` — the percent-plan-complete measure of
   short-interval planning.
3. **Substitution.** `15 ÷ 18 ≈ 83 %`.
4. **Result.** A commitment reliability of **≈ 83 %**, with the 3 misses and the 6 screened-out constraints
   each carrying a named reason — the reasons list *is* the improvement agenda (materials lead times, design
   turnaround), not the percentage.
5. **Interpretation.** The look-ahead protects the CPM from noise — the network keeps the logic; the window
   absorbs the churn — and produces the most honest leading indicator a site has: a falling commitment
   reliability precedes a slipping critical path by weeks (Domain 4, KA 4.1.2). Track the reasons, not just
   the rate.

### Key terms — KA 10.4

| Term | Meaning |
|---|---|
| **Progressing / data date** | Recording actuals and remaining durations, then recalculating. |
| **Baseline comparison** | Current vs baseline milestones and critical path. |
| **Schedule increment** | A Sprint/release as a time-phased unit mapped to milestones. |

### Sample MCQs — KA 10.4

**MCQ 10.4-A `[10.4.2 · Analysis]`** Why read both the network view and the earned-value view of schedule?
- A. They always agree.
- B. Only one is ever correct.
- C. Each covers the other's blind spot — EVM misses the critical path; the network does not aggregate cost/performance. ✅
- D. To duplicate effort.

*Rationale:* Aggregate `SPI` can hide critical-path slippage (Domain 6, KA 6.4.2); the network shows the path
but not aggregate performance. Together they give the full schedule picture.

**MCQ 10.4-B `[10.4.3 · Recall]`** In a hybrid programme, Sprints and releases are best treated as:
- A. Incompatible with scheduling.
- B. Schedule increments mapped to milestones. ✅
- C. A replacement for the critical path.
- D. Cost accounts.

*Rationale:* Sprints/releases are time-phased increments that map to milestones, reconciled with the CPM
schedule. They complement, not replace, scheduling, and are not cost accounts.

**MCQ 10.4-C `[10.4.1 · Application]`** A baseline forecasts completion at day 40. At the data date, a
**critical** activity has finished **3 days late** and a non-critical activity with **5 days of total float**
has finished **2 days late**. After recalculating the network, the completion forecast is:
- A. Day 45
- B. Day 40
- C. Day 42
- D. Day 43 ✅

*Rationale:* A critical slip passes through day for day (`40 + 3 = 43`), while the non-critical slip is
absorbed within its 5 days of float. A wrongly adds both slips; B ignores the critical slip; C applies the
absorbed slip instead of the critical one.

**MCQ 10.4-D `[10.4.1 · Analysis]`** A schedule is updated with several actual finish dates missing and key
milestones held on fixed date constraints. The main consequence is:
- A. The forecast is more reliable because the milestone dates are protected.
- B. Total float increases across the network.
- C. The network can no longer recalculate honestly — the forecast completion and current critical path are corrupted. ✅
- D. The baseline is automatically re-approved.

*Rationale:* Missing actuals and forced constraints break the logic-driven recalculation, hiding slippage and
moving critical paths — the schedule analogue of the cost data-integrity failures of Domain 5. A mistakes
concealment for protection; B and D do not follow from a corrupted update.

### Self-check — KA 10.4

1. Why must the critical path be re-identified when progressing a schedule? *(Actual progress can move the
   critical path to a different chain.)*
2. How do agile releases relate to a classical schedule? *(As schedule increments mapped to milestones and
   reconciled with the CPM view.)*

---

## Advanced topics — Domain 10

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 10.A.1 — Schedule health checks

A professional who inherits a schedule — a contractor's, a predecessor's, an AI-proposed one (KA 13.5.5) —
runs a set of **quality metrics** before trusting a single date. Industry health-check conventions exist for
exactly this purpose; what follows is the generic core they share, with thresholds set by the organisation
rather than borrowed from any named standard.

- **Logic density and 'dangles'.** Every activity except the first and last should have at least one
  predecessor and one successor. Activities without them — **dangles** — float free of the network: their
  dates do not respond when reality changes, so the forward and backward passes (KA 10.2) quietly stop being
  true for them.
- **Constraint counts.** Date constraints should be rare and individually justified. A schedule held together
  by constraints is a bar chart wearing a network's clothes — it cannot recalculate honestly (KA 10.1.2).
- **Long lags.** Every lag above an agreed threshold is exposed and justified — the audit developed in
  Advanced 10.A.3.
- **Negative float.** A late date earlier than an early date means a constrained date the logic cannot meet:
  the schedule is already impossible as modelled, and someone has not said so.
- **High-float concentrations.** Clusters of very large total float usually signal **missing logic** rather
  than genuine slack — an activity linked to nothing that needs it will show enormous float and mean nothing.
- **Out-of-sequence progress.** Successors recorded as started before their predecessors finished mean either
  the logic was wrong or the update is corrupt (KA 10.4.1); either way, the current critical path is suspect.

None of these checks needs more than the scheduling tool itself. The interpretation discipline matters as
much as the metrics: a schedule that fails its health checks is not necessarily *wrong*, but it cannot be
*trusted to recalculate* — its dates are assertions, not results. The health check runs before a baseline is
accepted (KA 10.4.2) and periodically thereafter, because schedules degrade in service.

**The schedule basis document.** The schedule analogue of the basis of estimate (Domain 3, KA 3.2.3) is a
short controlled document recording what the baseline *assumes*: the calendars and their justification
(including weather, Advanced 10.A.3), the productivity and crew assumptions behind key durations, the
sequencing rationale and preferential logic, the exclusions, the interfaces and third-party dependencies,
and the approvals. Without it, every future health check argues with a ghost: the checker can see *what*
the schedule says but not *why*, and re-baselines lose the record of what changed. The basis is approved
with the baseline (KA 10.4.2) and updated at re-baseline — and it is where a reviewer looks first when a
duration seems heroic.

### Advanced 10.A.2 — Resource-critical paths and buffers

KA 10.3.3 established that resource levelling can extend the duration and produce a **resource-critical
path** — a pace set by scarce crews and specialists rather than by logic. The worked levelling example
showed the mechanism: two activities forced into sequence by a single specialist crew. When that happens, a
professional watching the *logical* critical path is watching the wrong constraint; the path must be
re-identified **after** levelling, not before.

The **critical-chain** school of scheduling — covered here at awareness level only — starts from a related
observation about safety. Estimators pad individual durations, and the padding is largely wasted: work
expands to fill it, and early finishes are rarely passed on. Critical chain therefore schedules aggressive
durations, identifies the longest resource-constrained chain, and **aggregates the stripped safety into
buffers** — a **project buffer** at the end of the chain and **feeding buffers** where other chains join it.
Control is then exercised by watching **buffer burn**: the rate at which the buffer is consumed relative to
the chain's progress. The full method is a distinct discipline with its own behavioural practices; this book
neither teaches it end-to-end nor endorses it wholesale, and the examination expects awareness, not
application.

What transfers cleanly into classical practice is worth having. First, the recognition that resources can set
the pace — the resource-critical path of KA 10.3.3 taken seriously. Second, **buffer-style contingency on
the critical path**: the commit-P80/manage-P50 posture of KA 10.3.4 is exactly an aggregated buffer — the
gap between the internal target and the external commitment, held at the end of the schedule, owned and
visible rather than smeared invisibly across activities. Third, **managing the burn**: schedule contingency
consumed faster than progress is earned is an early warning in its own right — the schedule analogue of
contingency drawdown against realised risk in cost (Domain 12, KA 12.3).

### Advanced 10.A.3 — Calendars, lags and manipulation

KA 10.1.3 warned that hidden lags are a common way schedules are quietly manipulated; this topic covers the
mechanics and the audit. The starting point is uncomfortable: a Gantt view looks identical whether its dates
come from sound logic or from steering, and two instruments do most of the steering.

**Lags.** An FS + 15 lag is invisible on the bar chart yet injects fifteen days that no activity owns — no
resource, no progress measurement, no scrutiny. Stretching or shrinking lags to land a milestone leaves the
logic *looking* untouched; a lead (a negative lag) can silently overlap work that physically cannot overlap.

**Calendars.** Activities can sit on different calendars (five-day, seven-day, shift-based), and moving an
activity to a more generous calendar shortens its elapsed time with no visible change to duration or logic.
Worse, conventions differ on which calendar a *lag* follows, so the same lag can represent different elapsed
times in different parts of the same schedule. Calendar effects do not show on a printed bar chart at all.

**The audit** is generic and threshold-based. Expose **every lag above an agreed threshold** and require a
written justification in physical terms (curing, mobilisation); anything that cannot be justified is
converted into a real activity that is resourced and progressed. **Justify every constraint** or remove it.
**Reconcile calendars**: list every calendar in use, confirm each assignment is deliberate, and confirm the
lag-calendar convention once, in writing.

Why the effort is worth it: a **clean-logic schedule is a governance artefact**, not a technical nicety.
Delay analysis and extension-of-time positions (Domain 7), the P-level commitment (KA 10.3.4) and every
progressed forecast (KA 10.4) all presume a network that recalculates honestly. A manipulated schedule does
not merely mislead the team — it defrauds every downstream decision made on its dates.

**Planning calendars: weather and seasonality.** Calendars are also where *realism* lives. Weather-sensitive
work — earthworks, concrete pours, lifts, marine and high-altitude work — is planned on calendars carrying
expected non-work days by season, built from historical weather data (e.g. rain days per month, temperature
or wind thresholds), agreed with the client, and stated in the schedule basis (Advanced 10.A.1). The effects
are mechanical but material: elapsed durations stretch across bad seasons, float differs by calendar, and a
critical path can *move* with the season. The commercial edge is sharper still: a contractual weather
calendar separates **normal** weather (the contractor's risk, already priced into the baseline) from
**exceptional** weather (a compensable or excusable event) — without one, every storm becomes an argument
(Domain 7, Advanced 7.A.1; Domain 12 risk register).

### Advanced 10.A.4 — Merge bias

The forward pass takes the **latest** arrival at every merge point — `ES = max(EF of predecessors)`, the
pivotal calculation at activity E in this domain's case study. Deterministically that is correct; under
uncertainty it makes the deterministic date **optimistic**, an effect known as **merge bias**.

A compact illustration shows why. Two independent parallel paths converge at a milestone, and each path is
**50 % likely** to finish by day 20. The milestone is achieved by day 20 only if **both** paths arrive by
day 20: `0.5 × 0.5 = 0.25` — a **25 %** chance, even though either path alone is a coin toss. Checked from
the other side: the milestone is late if *either* path is late, and `1 − 0.25 = 0.75` — a 75 % chance of
missing day 20. With four such paths converging, `0.5⁴ = 0.0625` — about **6 %**. Each additional merging
path multiplies the probabilities together, so uncertainty **accumulates at merges**: the more parallel the
network, the further the deterministic date drifts from the probable one, *even when every individual path's
estimate is unbiased*.

This is a structural blind spot of deterministic CPM, not an estimating error, and it is a core part of the
case for **Monte Carlo schedule-risk analysis** (KA 10.3.4): simulation samples all paths together, so merge
bias emerges naturally in the completion distribution rather than needing a separate correction. The case
study showed the effect in miniature — twin 24-day critical paths returned P50 = 24 but P80 = 26, because
the finish is the *later* of two uncertain chains. The practical instinct to build: look for heavily merged
nodes — systems testing, commissioning, handover, where many trades converge — and treat a schedule with
large merges near its end as a candidate for simulation even when a single-chain schedule of the same length
might not warrant it.

### Advanced 10.A.5 — Critical-path drag

Float answers one question precisely and another not at all. It measures how far a **non-critical** activity
can slip before it matters (KA 10.2.4); it says nothing about how much a **critical** activity is actually
costing the finish date — on the critical path, float is zero for every activity, however long or short.
**Drag** is the complement: the amount of time an activity on the critical path is *adding* to the project
duration — equivalently, the most the project could gain by shortening that activity alone. For a critical
activity with a parallel path, `drag = min(activity duration, total float of the most-constraining parallel
path)`; where no parallel path exists, the drag is the activity's own duration.

A compact example shows the mechanics. Start → **A (5 d)** → **C (10 d)** → End, with **B (9 d)** running
Start → End in parallel. The critical path A–C is **15 days**; B carries `15 − 9 = 6` days of total float.
C's drag `= min(10, 6) = 6`: shorten C by 6 days (to 4) and the path becomes `5 + 4 = 9` days — equal to B,
which goes critical, so further shortening of C buys nothing. A's drag `= min(5, 6) = 5`: remove A entirely
and the project is `max(10, 9) = 10` days, a 5-day gain. Compression effort therefore ranks C first (6 days
available) and A second (5 days) — a ranking float cannot give, because float is **zero for both**.

The reading: drag turns "which activities are critical" into "which critical activities are worth
attacking", and it is the missing number in most compression workshops. It prices the *ceiling* on each
crash candidate before any money is spent (KA 10.3.1), and it is the deterministic cousin of merge bias
(Advanced 10.A.4): a parallel path's float is exactly what caps the gain. Computing drag across a large
network is mechanical and a good tool task; choosing which drag to buy down — with money, risk or scope —
is the professional's call.

### Advanced 10.A.6 — Delay analysis methods

When delay becomes a claim (Domain 7, KA 7.2.2 and Advanced 7.A.1), the schedule becomes evidence, and the
**method** by which delay is measured decides the answer as much as the facts do. At awareness level, the
recognised families are these.

- **As-planned vs as-built** — compare the intended programme with what actually happened. Simple and
  cheap, but silent on causation: it shows *that* the project was late, not why or whose.
- **Impacted as-planned** — insert the delay events into the baseline and re-run it. Prospective in
  character, but it ignores how the project actually ran.
- **Collapsed as-built** — the "but-for" method: remove the delay events from the as-built programme to
  show what would have happened without them.
- **Time impact analysis (TIA)** — model each delay event as a **fragnet** and insert it into a
  contemporaneous *updated* programme at the time of the event. The most widely respected method for
  prospective extension-of-time (EOT) assessment.
- **Windows analysis** — divide the project into periods and analyse critical-path movement window by
  window against contemporaneous updates. The most forensically robust retrospective method, and the most
  expensive.

**Worked example 10.A.6 — one event, by TIA.**

1. **Setup.** At the data date, forecast completion is **day 120**. The client stops work on an area for
   **10 working days**; the delay is modelled as a fragnet and inserted into the *current* update.
2. **Formula.** `EOT entitlement = post-impact completion − pre-impact completion`.
3. **Substitution.** The re-run completes at **day 127**: `127 − 120 = 7 days` — three of the ten days are
   absorbed by float on the affected path before it goes critical.
4. **Result.** A **7-working-day** prospective EOT position, evidenced by the update pair.
5. **Interpretation.** The event lasted 10 days; the *entitlement* is 7 — the difference (`10 − 7 = 3`
   days) is float, and who owns float (KA 10.2.4) is exactly why the contract's float provisions matter.
   Method choice is not neutral: the same facts run through impacted-as-planned would claim the full 10.

Method selection is governed by the contract, the records available and the timing — prospective versus
retrospective. Contemporaneous updates are the raw material of *every* credible method: a project that
skipped honest updates (KA 10.4.1) has already lost the analysis before it begins, which is why the
record-keeping discipline of Domain 7 (Toolkit 7.T.2) and the treatment of concurrency (Advanced 7.A.1)
sit alongside this topic. Fragnet mechanics and window-by-window comparisons are automatable; choosing the
method the tribunal will accept, and owning the causation story, is the delay analyst's craft.

---

## Case study — Domain 10: scheduling an airport terminal fit-out (aviation)

### Background

An international airport is reconfiguring two contact gates in an operating terminal — an airside fit-out
covering structural modifications, mechanical/electrical/plumbing (MEP) services, drywall partitions, ceilings
and the systems testing that airport operations require before passengers can be boarded through the gates.
The airline that leases the gates has a **hard operational deadline**: a seasonal schedule change on which its
aircraft rotations depend. Airside work compounds the pressure — escorted access, night-shift restrictions and
security screening mean lost days are hard to buy back informally.

The contractor's controls professional is asked to do exactly what this domain teaches, in order: build a
**logic-driven network** from the defined activities (KA 10.1), run the **forward and backward pass** to find
the dates, float and critical path (KA 10.2), **compress** the schedule to the airline's deadline at least cost
(KA 10.3), **quantify the risk** in the compressed schedule (KA 10.3.4), and then **progress and control** it
as actuals arrive (KA 10.4). The case runs that full chain end-to-end, and — as in any real compression exercise
— the most important finding is not the answer to the first question but what the compression does to the
network afterwards.

### The network (KA 10.1–10.2)

Decomposing the fit-out work packages (Domain 8, KA 8.2.1) gives **eight activities**, all linked
Finish-to-Start (10.1.2), with durations estimated from crew productivity and airside access constraints
(10.1.4):

| Activity | Duration (days) | Predecessor(s) |
|---|---:|---|
| **A** Mobilise | 2 | — |
| **B** Structural mods | 6 | A |
| **C** MEP first fix | 8 | A |
| **D** Drywall | 5 | B |
| **E** MEP second fix | 6 | C, D |
| **F** Ceilings | 4 | E |
| **G** Systems test | 3 | F |
| **H** Handover | 1 | G |

The logic is sound construction sequence, not dates: structural modifications must precede drywall (mandatory,
"hard logic"); MEP first fix runs **in parallel** with the structural/drywall chain off the same mobilisation;
MEP second fix needs *both* the first fix (C) and closed walls (D); ceilings follow second fix; systems test
follows ceilings; handover closes the job. Two paths therefore run from A to E: **A–B–D** (the structural
chain) and **A–C** (the services chain), merging at E and continuing through F–G–H to handover. Because the
network is built on logic rather than constraints, it will recalculate honestly at every step that follows —
which is the property the whole case depends on.

### Forward and backward pass (KA 10.2)

**Forward pass** (left to right; `ES` = latest `EF` of predecessors; `EF = ES + duration`; start at time 0):

| Activity | `ES` | `EF` (= ES + dur) |
|---|---:|---:|
| A | 0 | 2 |
| B | 2 | 8 |
| C | 2 | 10 |
| D | 8 | 13 |
| E | max(10, 13) = 13 | 19 |
| F | 19 | 23 |
| G | 23 | 26 |
| H | 26 | 27 |

The merge at E is the pivotal calculation: E cannot start until *both* predecessors finish, so
`ES(E) = max(EF of C, EF of D) = max(10, 13) = 13`. **Project duration = 27 days** (the `EF` of H).

**Backward pass** (right to left from `LF` of H = 27; `LF` = earliest `LS` of successors; `LS = LF − duration`):

| Activity | `LF` | `LS` (= LF − dur) |
|---|---:|---:|
| H | 27 | 26 |
| G | 26 | 23 |
| F | 23 | 19 |
| E | 19 | 13 |
| D | 13 | 8 |
| C | 13 | 5 |
| B | 8 | 2 |
| A | 2 | 0 |

**Float.** `TF = LS − ES` (10.2.4). Every activity has `TF = 0` **except C**: `TF(C) = 5 − 2 = ` **3 days**.
The services chain can absorb three days of slippage before it touches the project finish; nothing else can
absorb any.

**The critical path** is the zero-float chain **A–B–D–E–F–G–H** `= 2 + 6 + 5 + 6 + 4 + 3 + 1 = ` **27 days**,
equal to the project duration (10.2.5). The parallel services path A–C–E–F–G–H is 24 days — three days shorter,
which is exactly where C's 3 days of float come from. Note the structure for later: the two chains differ
*only* in their middle segment (B–D at 11 days versus C at 8 days); everything from E onwards is **shared**.

### Compressing to the deadline (KA 10.3)

The airline's schedule change lands the deadline at **24 days** — **3 days** must come out of a 27-day
programme. Fast-tracking (10.3.2) is examined first and largely rejected: overlapping drywall into unfinished
structural work airside, or second fix into open first-fix zones, raises rework risk the operating terminal
cannot tolerate. The professional turns to **crashing** (10.3.1), and prices the crashable **critical**
activities (crashing C, the only floated activity, would buy nothing — it would merely add cost and reduce
float):

| Activity | Max crash (days) | Crash cost (USD/day) |
|---|---:|---:|
| **B** Structural mods | 2 | 4,000 |
| **D** Drywall | 1 | 6,000 |
| **E** MEP second fix | 2 | 9,000 |

**Setup:** shorten the 27-day critical path A–B–D–E–F–G–H by 3 days to meet the 24-day deadline; parallel path
A–C–E–F–G–H currently 24 days.
**Formula:** crash the **cheapest critical day first**, re-checking after each step whether the parallel path
has become critical (10.3.1).
**Substitution:** crash **B by 2 days** at 4,000/day `= 2 × 4,000 = ` **USD 8,000** (27 → 25 days); then crash
**D by 1 day** at 6,000/day `= ` **USD 6,000** (25 → 24 days).
**Result:** 3 days saved for **USD 14,000** `(8,000 + 6,000)`, avoiding E entirely at 9,000/day — the most
expensive option. New project duration `27 − 3 = ` **24 days**. Deadline met.
**Interpretation:** cheapest-first is only half the discipline; the other half is the re-check that follows.

**The twist — re-run the parallel path.** The compressed structural chain now reads A–B–D–E–F–G–H
`= 2 + 4 + 4 + 6 + 4 + 3 + 1 = 24` days. The services path was never touched: **A–C–E–F–G–H
`= 2 + 8 + 6 + 4 + 3 + 1 = ` 24 days.** C's total float is now `TF(C) = 2 − 2 = ` **zero** — the project has
**two parallel critical paths**. This is the shift the syllabus warns about (10.3.1, "crashing can shift the
critical path"): compression did not create time from nothing, it **consumed the float of the non-critical
chain**. Two consequences follow. First, any *further* compression must shorten **both** paths at once — which
in this network means the **shared** activities E, F, G or H, precisely the segment where crashing is most
expensive (E at USD 9,000/day) or operationally hardest (compressing a 3-day systems test or a 1-day handover
airside). The cheap days are gone; the marginal cost of time has stepped up. Second, the schedule is now
**brittle**: with every activity on a critical path, a one-day slip on **either** chain — a structural crew
short-staffed *or* a first-fix material delay — goes straight through to handover, day for day. A 27-day
schedule with 3 days of float on one chain and a 24-day schedule with none are very different risk positions,
even though only the second meets the deadline. That observation is what forces the next step.

### Quantifying the risk (KA 10.3.4)

A deterministic 24 days now says nothing about the *probability* of achieving 24 days — and with twin critical
paths, that probability is worse than either path alone would suggest, because the project finishes on the
**later** of two uncertain chains (the merge-point pessimism that deterministic CPM cannot see, 10.3.4). The
professional puts **three-point durations** (10.1.4) on every activity in the compressed network and runs a
**Monte Carlo simulation**: thousands of recalculations with sampled durations, yielding a distribution of
completion dates rather than a single number. The run returns **P50 = 24 days** — the deterministic date is
only a coin toss — but **P80 = 26 days**, the spread driven directly by the twin-critical-path brittleness:
in roughly half the iterations one chain or the other slips and drags handover with it.

The professional's move is the one this Body of Knowledge teaches for cost and repeats here for time: **commit
26 days externally** — the P80 date goes into the deadline conversation with the airline, with the reasoning
shown — while **managing to 24 internally**, and holding the 2-day difference **explicitly as schedule
contingency**, owned, visible and released only against realised risk. This is the schedule analogue of cost
contingency (Domain 12, KA 12.3): the gap between the aggressive internal target and the probabilistic external
commitment is not padding, it is priced risk. An airline told 24 days and delivered in 26 has a broken rotation
plan; an airline told 26 days and delivered in 25 has a day in hand. Same schedule — entirely different
professional outcome.

### Progressing it (KA 10.4)

At the **data date** of day 10, actuals show **B finished 1 day late** (structural surprises behind existing
finishes — the classic airside unknown). The network is **re-run**, not eyeballed: the B-side chain now reads
**25 days**, the C-side still **24** — the critical path has **moved** back to a **single chain** (the
structural side), and C has recovered a day of float it did not have the day before. The finding writes its own
action: the lost day must be **recovered on the B side** — resequencing drywall crews, weekend access — or the
external 26-day commitment starts absorbing contingency for a realised risk, logged as such. This is KA 10.4.1
in practice: progress and recalculate **every period**, because the critical path is a **living thing** — it
moved when the schedule was crashed, and it moved again when reality arrived. A controls professional who is
still watching last month's critical path is watching the wrong activities.

### What the credential expects

The case is the domain in miniature, and each step is a knowledge area doing its job. A **logic-driven
network** (10.1) is what made every later recalculation possible: eight activities, mandatory FS logic, a
parallel services chain — no date constraints to freeze the model. The **forward and backward pass** (10.2)
turned that logic into dates, exposed the merge at E as the governing calculation, and located all the float in
one place: `TF(C) = 3`, everything else critical on the 27-day path A–B–D–E–F–G–H. **Cheapest-first crashing
with the parallel-path check** (10.3.1) bought 3 days for USD 14,000 — and the check, not the arithmetic, was
the professional content: compression consumed C's float and left twin 24-day critical paths, so further
compression must attack the shared chain and the schedule is brittle. **Monte Carlo and P-level commitment**
(10.3.4) converted that brittleness into a number — P50 = 24, P80 = 26 — and into the commit-P80/manage-P50
posture, with the difference held as explicit schedule contingency. **Progressing** (10.4) then showed the
critical path moving under actuals, and the period-by-period recalculate-and-recover discipline that keeps the
forecast honest. Two closing connections complete the picture. Earned value alone would have missed most of
this story: an aggregate `SPI` near 1.0 can coexist with a critical-path slip on one of two parallel chains —
the **EVM blind spot** this domain exists to cover (Domain 6, KA 6.4.2). And AI-assisted scheduling (KA
13.5.5) would have earned its keep at three points — logic-checking the network for missing links and hidden
constraints, accelerating the Monte Carlo run, and predicting the day-10 delay from progress trends — but at
each one the logic, the P-level commitment and the recovery decision remain the professional's: **AI proposes,
the professional disposes.**

---

## Case study B — Domain 10: a 54-hour track-renewal possession (rail)

### Background

A national railway grants a track-renewal contractor a **54-hour weekend possession** of a twin-track main
line: the route closes to traffic at **02:00 Saturday (hour 0)** and must be handed back, safe and fit for
line-speed or agreed-restriction running, by **08:00 Monday (hour 54)**. Inside that window the contractor
strips a life-expired section of track, renews the ballast, lays new sleepers and rail, welds and stresses it,
tamps it to line and level and hands the railway back. The scheduling problem differs from an ordinary project
in one structural way: **the deadline is absolute**. Monday's first trains are sold, crewed and pathed; an
overrun does not slip a milestone, it stops a railway. The access agreement prices that reality at
**USD 40,000 per hour, or part hour, of late hand-back** — and money is the smaller consequence, because a
contractor who overruns argues from weakness for every future possession it requests. Everything in this case —
the units, the float, the mid-possession decision — is Domain 10's machinery run in **hours instead of days**,
against a finish line that cannot move.

### The possession network, built backwards from hand-back (KAs 10.1–10.2)

The planner defines **eight activities** (10.1.1), linked Finish-to-Start (10.1.2), with durations in hours
estimated from machine outputs and unit rates per metre of track (10.1.4):

| Activity | Duration (h) | Predecessor(s) |
|---|---:|---|
| **A** Take possession, isolate and protect | 3 | — |
| **B** Strip old track | 9 | A |
| **C** Excavate and renew ballast | 12 | B |
| **D** Lay new sleepers and rail | 11 | C |
| **E** Weld, stress and clip | 7 | D |
| **G** Renew cess drainage | 14 | A |
| **F** Tamp, line and level | 5 | E, G |
| **H** Test, inspect and hand back | 4 | F |

The renewal chain A–B–C–D–E is hard logic — nothing lays rail into unexcavated ballast. The **drainage renewal
G** is a separable work item running in parallel off the same possession take-up, added to the weekend because
the access was available; it must finish before final tamping because the tamper works over the completed cess.
Rail planning runs this network **backwards before it runs it forwards**: hand-back at hour 54 is fixed, so the
backward pass (10.2.3) is anchored on `LF(H) = 54` and the question the plan must answer is not "when do we
finish?" but "**how late can each activity start and still hand back?**" — and, once the forward pass is laid
alongside, "how much margin does the plan hold against the one date that cannot move?"

**Forward pass** (hours from possession start; `ES` = latest `EF` of predecessors; `EF = ES + duration`):

| Activity | `ES` | `EF` |
|---|---:|---:|
| A | 0 | 3 |
| B | 3 | 12 |
| C | 12 | 24 |
| D | 24 | 35 |
| E | 35 | 42 |
| G | 3 | 17 |
| F | max(42, 17) = 42 | 47 |
| H | 47 | 51 |

Planned duration **51 hours**: the renewal chain A–B–C–D–E–F–H `= 3 + 9 + 12 + 11 + 7 + 5 + 4 = 51`, against
the drainage path A–G–F–H `= 3 + 14 + 5 + 4 = 26`.

**Backward pass**, anchored on the hand-back constraint `LF(H) = 54`, not on the early finish of 51:

| Activity | `LF` | `LS` |
|---|---:|---:|
| H | 54 | 50 |
| F | 50 | 45 |
| E | 45 | 38 |
| D | 38 | 27 |
| C | 27 | 15 |
| B | 15 | 6 |
| G | 45 | 31 |
| A | min(6, 31) = 6 | 3 |

**Float in hours.** `TF = LS − ES` (10.2.4) gives every activity on the renewal chain **3 hours** — not
activity float in the ordinary sense but a single shared **hand-back margin**, the 54 − 51 gap, which any one
of them can consume exactly once. G carries `TF = 31 − 3 = ` **28 hours**, of which `FF = 42 − 17 = ` **25
hours** is free float — G can slip a full shift without touching anything. The planner prices the margin
before the weekend starts: 3 hours of buffer standing between the plan and a penalty running at USD 40,000 an
hour means every hour of it is worth USD 40,000 of avoided downside at the margin — which is why possession
plans report float in hours, by name, to the person who owns the hand-back.

### Hour 20: the wet bed (KA 10.4)

At the **hour-20 progress review** — Saturday evening — the excavation (C) has hit a **wet bed**: saturated
formation that must be dug out deeper and geotextiled. C started on time at hour 12 and should have 4 hours of
work left; the site engineer's re-estimate is **7 hours left** — a **3-hour slip**. The controls professional
re-runs the network from the data date (10.4.1) rather than arguing about it: C now finishes at hour 27, D runs
27→38, E 38→45, F 45→50, H 50→54.

**Setup:** at hour 20, remaining critical work `= 7 (C) + 11 (D) + 7 (E) + 5 (F) + 4 (H) = 34` hours; time
remaining to hand-back `= 54 − 20 = 34` hours.
**Formula:** `margin = time remaining − remaining critical work`.
**Substitution:** `34 − 34 = 0`.
**Result:** forecast hand-back **exactly 54:00** — the 3-hour margin is **fully consumed** with 34 hours of
single-shift-fragile work still to run.
**Interpretation:** the deterministic forecast still "meets" the deadline, and that is precisely what makes it
dangerous: a zero-margin plan against an absolute deadline is a coin toss, not a plan (10.3.4).

### De-scope or press on — pricing the decision (KAs 10.3, 10.3.4)

Two options go to the possession manager at hour 21, each priced. **Option 1 — de-scope a work item:** omit
the final tamp (F, 5 hours), hand back at hour 49 with a **temporary speed restriction** over the renewed
section, and complete tamping in two midweek night-time possessions already available in the access plan. Cost:
two night shifts at USD 32,000 `= 64,000`, plus USD 16,000 of speed-restriction delay charges — **USD 80,000,
near-certain**, and hand-back margin restored to `54 − 49 = ` **5 hours** (H follows E directly at 45→49).
**Option 2 — press on at full scope** and accept the overrun risk. A quick Monte Carlo over three-point
estimates for the remaining activities (10.1.4, 10.3.4) puts the probability of overrunning hour 54 at
**50 %**, with a mean overrun of about 2 hours when it happens, and a **P80 hand-back of hour 57**.

**Setup:** Option 2's exposure: `P(overrun) = 50 %`, mean overrun given overrun **2 hours**, penalty
**USD 40,000/hour**.
**Formula:** `EMV = probability × mean overrun × penalty rate`; compare with Option 1's near-certain cost.
**Substitution:** Option 2 EMV `= 50 % × 2 × 40,000 = 40,000`; Option 1 `= 80,000`.
**Result:** on **EMV**, pressing on is **USD 40,000 cheaper** (40,000 vs 80,000).
**Interpretation:** and the professional recommends **de-scoping anyway** — because EMV is the wrong sole test
against an absolute deadline. At **P80** the press-on case costs `3 × 40,000 = ` **USD 120,000** against the
de-scope's 80,000, and the tail carries what no rate card prices: Monday commuters stranded, the regulator's
attention, and the railway's willingness to grant the next possession. The de-scope, re-simulated, hands back
by hour 52 even at P80. This is risk appetite applied honestly (Domain 12, KA 12.1.3): a 50 % chance of
stopping a railway sits outside anyone's tolerance, so the organisation pays USD 40,000 above the average
outcome to buy certainty — the same commit-at-P80 logic the first case study applied to a completion date,
compressed here into a decision taken at hour 21 of 54. The de-scope is agreed with the client's possession
manager, logged with its price, and the tamp moves to midweek.

The weekend ends undramatically, which is the point: hand-back **20 minutes inside the hour-49 forecast**,
speed restriction posted,
tamping completed Tuesday and Wednesday nights, penalty **nil**.

### What the credential expects

The case re-runs Domain 10 with the units changed and one constraint hardened, and the candidate should be
able to name what survives the translation. The **network and passes** (10.1–10.2) work identically in hours:
forward pass to 51, backward pass anchored on the constrained `LF = 54`, and float read as `LS − ES` — with
the professional gloss that the renewal chain's uniform 3 hours is a **shared hand-back margin**, consumable
once, not slack scattered per activity. **Working backwards from hand-back** is the backward pass promoted to
the primary planning direction, which is exactly what an immovable deadline does to scheduling. The **hour-20
recompute** is KA 10.4.1's discipline at possession tempo: progress the network from the data date, and read a
zero-margin forecast as risk, not success. The **de-scope decision** joins this domain to Domain 12: price
both options, compute the EMV, then refuse to let the average decide when the distribution's tail is
intolerable — the P80 case, not the P50 case, is what justifies paying for certainty against an absolute
deadline. And the standing lesson generalises well beyond rail: wherever hand-back is absolute — a runway
re-opening, a plant restart, a retail trading date — schedule control is margin management in small units,
re-computed every few hours, with the de-scope option priced **before** it is needed. An AI scheduling
assistant (KA 13.5.5) could have re-run the network and the simulation in seconds at hour 20; the decision to
give up scope to protect the railway belonged, as always, to the professional. **AI proposes, the professional
disposes.**

---

## Executive perspective — Domain 10

**What the executive must hold onto.** The **critical path** sets the completion date, and it **moves** — it
moves when the schedule is compressed and again when actuals arrive, so the standing question is never "what is
the date" but "what is the path this period, and how confident is the date". A single deterministic finish is a
fiction: the honest commitment is a **P-level** from schedule-risk analysis (KA 10.3.4) — commit externally at
P80, manage internally to the aggressive date, and hold the difference as owned, visible schedule contingency.
And compression is never free: **crashing** trades money for time, **fast-tracking** trades risk for time, and
every day bought consumes float that was quietly making the schedule resilient.

**Six questions to ask from the chair.**

1. What P-level is this completion date — and what would P80 say?
2. Where is the critical path this period, and where was it last period?
3. How much float remains, who is consuming it, and which near-critical paths could take the lead if they slip?
4. If we must go faster, what does a day cost — in money (crashing) or in rework risk (fast-tracking)?
5. Is this schedule driven by logic, or held together by date constraints and hidden lags?
6. Has the schedule been resource-levelled — or does it assume crews and specialists we do not have?

**The traps at board level.**

- **A bar chart is not a schedule.** A Gantt view looks identical whether it is driven by sound logic or forced
  by constraints and hidden lags — and only the logic-driven version recalculates honestly when reality arrives
  (KA 10.1). Ask what is holding the dates up.
- **A healthy aggregate hides a critical slip.** An `SPI` near 1.0 can coexist with a slipping critical path —
  the earned-value blind spot (KA 10.4.2) — so insist on the network view alongside the aggregate one.
- **A compressed schedule is not the same schedule, shorter.** Compression consumes the float of the parallel
  chains; a schedule crashed to the deadline with twin critical paths is a far more brittle risk position than
  the longer schedule it replaced, even though only the shorter one "meets the date".
- **Deterministic precision reads as confidence.** A single completion day sounds firmer than a P-level range,
  but the deterministic date is typically about a coin toss — the P80 is the commitment.

**What good looks like.** The schedule is progressed at every data date and the critical path re-identified,
with its movement reported rather than smoothed away. External commitments are made at a stated P-level, the
gap to the internal target is held as explicit schedule contingency, and compression decisions arrive priced —
cheapest critical day first, with the parallel-path check shown. The board sees both the network view and the
earned-value view each period, and when the date moves it hears about it from the schedule, not from the site.

---

## Calculation exercises — Domain 10

*Work each exercise before reading its solution; every step uses only this domain's methods.*

**Exercise 10.1** — A six-activity network has the following logic and durations (days): A **2**
(start); B **5** (after A); C **4** (after A); D **3** (after B); E **6** (after C); F **3** (after
D and E). Run the forward and backward passes; state the project duration, each activity's
`ES/EF/LS/LF`, its `TF` and `FF`, and the critical path.

**Solution 10.1.**

1. Forward pass (`ES` = latest predecessor `EF`; `EF = ES + duration`): A 0/2; B 2/7; C 2/6;
   D 7/10; E 6/12; F `ES = max(10, 12) = 12`, `EF = 15`. **Project duration = 15 days.**
2. Backward pass (`LF` = earliest successor `LS`; `LS = LF − duration`): F 12/15; E 6/12; D 9/12;
   B 4/9; C 2/6; A `LF = min(4, 2) = 2`, `LS = 0`.
3. Floats (`TF = LS − ES`; `FF = min successor ES − EF`):

   | Activity | `ES` | `EF` | `LS` | `LF` | `TF` | `FF` |
   |---|---:|---:|---:|---:|---:|---:|
   | A | 0 | 2 | 0 | 2 | 0 | 0 |
   | B | 2 | 7 | 4 | 9 | 2 | 0 |
   | C | 2 | 6 | 2 | 6 | 0 | 0 |
   | D | 7 | 10 | 9 | 12 | 2 | 2 |
   | E | 6 | 12 | 6 | 12 | 0 | 0 |
   | F | 12 | 15 | 12 | 15 | 0 | 0 |

4. Critical path — the zero-float chain — is **A → C → E → F** `= 2 + 4 + 6 + 3 = 15` days; the
   parallel A–B–D–F path (`2 + 5 + 3 + 3 = 13`) carries 2 days of float, all of it free at D but
   none at B.

**Exercise 10.2** — Activity X has `ES = 8` and a duration of **4 days**; the backward pass gives
`LS = 11` and `LF = 15`. X has two successors: Y with `ES = 14` and Z with `ES = 16`. Compute X's
`EF`, its total float (two ways) and its free float, then state how many days X can slip before it
delays a successor, and before it delays the project.

**Solution 10.2.**

1. `EF = ES + duration = 8 + 4 = 12`.
2. Total float `TF = LS − ES = 11 − 8 = 3`; cross-check `TF = LF − EF = 15 − 12 = 3`. ✓
3. Free float `FF = min(ES of successors) − EF = min(14, 16) − 12 = 14 − 12 = 2`.
4. X can slip **2 days** without touching any successor (its free float); the **3rd** day delays Y —
   consuming float Y's chain relies on — but still not the project; only beyond **3 days** does the
   completion date move. Free float is private slack; the `TF − FF = 1` day is shared with the
   successor chain.

**Exercise 10.3** — Three activities G, H and J run in sequence and are estimated three-point:
G `O = 3, M = 5, P = 13`; H `O = 2, M = 4, P = 6`; J `O = 5, M = 8, P = 11` (days). A parallel
chain of fixed duration **17 days** runs alongside. Compute each activity's PERT expected duration
and `σ`, the chain's expected length, and identify the longest expected path.

**Solution 10.3.**

1. G: `tE = (3 + 4×5 + 13)/6 = 36/6 = 6` days; `σ = (13 − 3)/6 = 1.67` days.
2. H: `tE = (2 + 4×4 + 6)/6 = 24/6 = 4` days; `σ = (6 − 2)/6 = 0.67` days.
3. J: `tE = (5 + 4×8 + 11)/6 = 48/6 = 8` days; `σ = (11 − 5)/6 = 1.00` day.
4. Expected chain length `= 6 + 4 + 8 = 18` days — though the most-likely sum is only
   `5 + 4 + 8 = 17`.
5. On most-likely durations the two paths tie at 17 days; on expected durations G–H–J is the
   **longest expected path at 18 days**. G's pessimistic tail (13 against a most-likely 5) pulls
   the expectation up — exactly why three-point estimates can change which path deserves attention.

**Exercise 10.4** — A network starts with shared activity P (**4 days**), then splits: path 1 runs
P → Q (**7**) → R (**9**), 20 days in total; path 2 runs P → T (**6**) → U (**9**), 19 days. Crash
costs: P **USD 7,000/day** (max 1); Q **USD 4,000/day** (max 2); R **USD 6,000/day** (max 1);
T **USD 5,000/day** (max 2); U cannot be crashed. Build the cheapest plan to save **2 days**,
showing the parallel-path check at each step.

**Solution 10.4.**

1. Day 1: crash the cheapest day on the critical path P–Q–R — **Q for 4,000**. Path 1 falls to
   `4 + 6 + 9 = 19`; the check shows path 2 is also 19, so **both paths are now critical**.
2. Day 2: every critical path must shorten. Options: Q + T `= 4,000 + 5,000 = 9,000`; R + T
   `= 6,000 + 5,000 = 11,000`; or **P alone** — it sits on both paths — at `7,000`. Cheapest:
   **P for 7,000**.
3. Plan: Q by 1 day, P by 1 day; total cost `= 4,000 + 7,000 = 11,000`; new duration **18 days**
   (path 1 `3 + 6 + 9 = 18`; path 2 `3 + 6 + 9 = 18`).
4. The trap: crashing Q twice (`8,000`) looks cheapest on the menu, but the second day saves
   nothing once path 2 governs at 19 — the parallel-path check is what prices the plan honestly.

**Exercise 10.5** — A baseline network completes at **day 30** via a critical path through activity
M; a parallel chain through activity N is **26 days** long, giving N **4 days** of total float. At
the data date, M has finished **2 days late** and N is forecast to finish **3 days late**.
Recalculate: the new completion date, whether the critical path has moved, and N's float against
the new finish.

**Solution 10.5.**

1. The critical slip passes through day for day: new completion `= 30 + 2 = 32`.
2. N's chain after its slip `= 26 + 3 = 29` days — still shorter than 32, so the critical path
   **has not moved**; N's slip is absorbed within float.
3. N's float against the new finish `= 32 − 29 = 3` days (the original 4, less the 3 consumed,
   plus the 2 the critical slip added to the project end).
4. Reading: the project is 2 days late because of M alone; N cost nothing this period, but its
   buffer is being consumed — float is re-measured at every data date, never assumed from the
   baseline.

**Exercise 10.6** — A package has two paths from start to finish: path 1 is **A (6 d) → B (8 d)** —
14 days and critical; path 2 is **C (11 d)**, carrying 3 days of total float. Crash costs and
limits: A **USD 2,000/day** (max 2 days); B **USD 3,500/day** (max 3 days); C **USD 1,500/day**
(max 2 days). (a) Find the cheapest way to deliver at **day 11** and its total cost. (b) What would
the next day — day 10 — cost, and why does the answer change character?

**Solution 10.6.**

1. (a) The finish is set by path 1 (`6 + 8 = 14` days), so only A and B help until path 2 binds
   (C's float `= 14 − 11 = 3` days). Cheapest first: crash **A by 2 days** at `2 × 2,000 = 4,000`
   — path 1 falls to 12 days. A is exhausted; crash **B by 1 day** for `3,500` — path 1 = 11 days
   = path 2: C's float is used up and **both paths are now critical**.
2. Total cost `= 4,000 + 3,500 = USD 7,500`. Check: path 1 had to lose 3 days, and its three
   cheapest days are A, A, B — no cheaper combination exists.
3. (b) Day 10 requires **both** critical paths to lose a day: B (`3,500`; 2 of its max 3 days
   remain) **and** C (`1,500`) — `3,500 + 1,500 = 5,000` for that single day, against 3,500 for
   the day before.
4. The character change: once the paths merge at criticality, every further day must be bought on
   **every** critical path at once — the marginal cost of compression steps up (KA 10.3.1; and
   Advanced 10.A.5: at day 11, B's remaining drag is exactly what C caps).

---

## Practitioner's toolkit — Domain 10

*Adoption-ready artefacts; adapt the column headings and thresholds to your organisation, then keep them
stable.*

### Toolkit 10.T.1 — Schedule health-check sheet

Per Advanced 10.A.1, the health check runs before a baseline is accepted and periodically thereafter. Record
the organisational threshold once, then score every schedule against it.

| Check | Threshold (organisational) | This schedule | Pass? |
|---|---|---|---|
| Dangling activities (missing predecessor/successor) | 0, excluding start and finish | 3 dangles found | ✘ |
| Date constraints | ≤ 5, each justified in writing | 4, all justified | ✔ |
| Lags above threshold | None > 10 days without written physical justification | 1 × FS + 15 unjustified | ✘ |
| Negative float | 0 activities | | |
| High-float concentration | ≤ 5 % of activities with total float > 44 days | | |
| Out-of-sequence progress at last update | 0 unresolved | | |
| Calendars reconciled | Every assignment deliberate; lag-calendar convention confirmed in writing | | |

**Usage note.** This sheet is Advanced 10.A.1 made operational: run it on any inherited schedule — a
contractor's, a predecessor's, an AI-proposed one (KA 13.5.5) — before trusting a single date. A schedule
that fails is not necessarily wrong, but it cannot be trusted to *recalculate* (KA 10.2): its dates are
assertions, not results, so a failed line (the dangles and the unjustified lag above) blocks baseline
acceptance until fixed or justified. Set the thresholds once, at organisational level, and keep them stable so
scores compare across schedules and across periods. The lag and calendar lines are the audit of Advanced
10.A.3 in summary form — every exposed lag is justified in physical terms or converted into a real activity.

### Toolkit 10.T.2 — Progress-update checklist

Applied at every data date (KA 10.4.1), before the period's schedule report is issued.

- [ ] Data date set; actual start/finish dates recorded for all work started or finished in the period.
- [ ] Remaining durations reassessed for every in-progress activity — not inferred from % complete alone.
- [ ] Out-of-sequence progress identified and resolved: logic corrected, or the update queried.
- [ ] No new date constraints introduced; existing constraints re-justified or removed (Advanced 10.A.1).
- [ ] Network recalculated and the current critical path re-identified — it may have moved.
- [ ] Total and free float re-read; float consumption against last period noted, with who is consuming it.
- [ ] Forecast completion compared with the baseline and the committed P-level date (KA 10.3.4); schedule
      contingency burn noted (Advanced 10.A.2).
- [ ] Key milestone movement against baseline recorded (KA 10.4.2).
- [ ] Network view read alongside the earned-value view (`SV`/`SPI`, earned schedule) — each covers the
      other's blind spot (KA 10.4.2).
- [ ] Variance narrative drafted to Domain 4 standards — figure, cause, action — and signed off.

**Usage note.** This checklist is KA 10.4.1's progressing discipline as a repeatable routine: the schedule
equivalent of month-end cut-off in cost (Domain 5, KA 5.2.4). The remaining-duration line matters most —
updating % complete without reassessing remaining duration is how forecasts quietly stop being true. The
critical-path line encodes the case study's lesson: the path moved when the schedule was crashed and again
when actuals arrived, so a professional still watching last month's path is watching the wrong activities.
The P-level comparison keeps the commit-P80/manage-P50 posture honest, period after period.

---

## Exam preparation — Domain 10

**How this domain is examined.** Domain 10 leans towards **application**: recall items cover the four
dependency types, the float definitions and the compression vocabulary, but the marks concentrate on working a
network — forward and backward passes, total and free float, PERT and crash economics (KAs 10.2–10.3).
Analysis items test interpretation: what total-vs-free float means for who a slip hurts, what compression does
to the parallel chain, and why the network and earned-value views are read together (KA 10.4). Expect at least
one multi-step item that chains the pass, the float and the critical path from a single small network.

**Calculation traps.**

- **Taking the shorter path as the project duration.** The critical path is the **longest** path; the 10-day
  A–C–E–F chain in the worked network is a distractor, not the answer (MCQ 10.2-A).
- **Float sign errors.** `TF = LS − ES` (equivalently `LF − EF`) — computed early-minus-late, the float comes
  out negative and every later step is wrong from there.
- **Free float against the wrong successor.** `FF = min(ES of successors) − EF`: using the *later* successor,
  or averaging the successors, both appear as distractors (MCQ 10.2-D).
- **Crashing off the critical path.** Only critical activities shorten the project; crashing a floated
  activity adds cost and buys nothing (MCQ 10.3-A — and the fit-out case's activity C).
- **Skipping the parallel-path check.** After each crashed day, re-test whether a parallel chain has become
  critical — the cheapest activity's second day can be worthless once the other path governs (Exercise 10.4).
- **PERT weighting and lag direction.** `tE = (O + 4M + P)/6` — not `/3`, and not forgetting to divide (MCQ
  10.1-A); and FS + 3 *delays* the successor — reading the lag as a lead gives day 7 instead of day 13 (MCQ
  10.1-C).

**Time management.** For any network item, **draw the network before computing anything** — a thirty-second
sketch of nodes and arrows prevents nearly every merge and path error. Run the forward pass completely, then
the backward pass, then float; recomputing piecemeal wastes time. Recall items (dependency types, levelling vs
smoothing) should take seconds — bank them and spend the balance on the multi-step network.

**Reflection questions.**

1. Is your current schedule driven by logic, or held together by date constraints and hidden lags — and how
   would you demonstrate the difference to an auditor?
2. Where does the float live on your project this period, who is consuming it, and which near-critical path
   worries you most?
3. When your organisation last compressed a schedule, was the decision priced cheapest-critical-day-first with
   the parallel-path check shown — or bought on instinct?
4. At what P-level are your completion commitments made, and does the gap to the internal target exist as
   owned, visible schedule contingency?

---

## Domain 10 summary

Scheduling models the work in time: activities decomposed from the WBS, sequenced with the four dependency
types and leads/lags, and durated (three-point/PERT where uncertain). The **Critical Path Method** computes
early and late dates through a forward and backward pass, derives **total and free float**, and identifies the
**critical path** — the longest, zero-float chain that sets the project duration and concentrates management
attention. The schedule is compressed by **crashing** (cost for time, on the critical path) or **fast-tracking**
(time for risk, by overlap), made deliverable by **resource levelling and smoothing**, and its uncertainty
quantified by **schedule-risk (Monte Carlo)** analysis. It is controlled by progressing against the baseline
each period — reading both the network view (path) and the earned-value view (aggregate) because each covers
the other's blind spot — and it reconciles with agile cadence, treating Sprints and releases as schedule
increments mapped to milestones.

**Cross-references.** WBS/activities → 8.2; schedule variance/`SPI`/earned schedule and the critical-path
blind spot → 6.4; the time-phased cost baseline (PV) → 3.3; schedule risk and contingency → Domain 12; hybrid
Sprint-to-milestone mapping → 9.6; AI-assisted scheduling and delay prediction → 13.5.

