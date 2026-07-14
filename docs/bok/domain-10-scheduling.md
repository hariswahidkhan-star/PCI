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
- B. Finish-to-Start ✅
- C. Finish-to-Finish
- D. Start-to-Finish

*Rationale:* Finish-to-Start (the default). SS ties starts; FF ties finishes; SF is the rare start-to-finish.

**MCQ 10.1-C `[10.1.3 · Application]`** Activity A finishes at the end of day 10. Its successor B is linked
**FS + 3 days** (a lag for curing time). B's earliest start is:
- A. Day 10
- B. Day 13 ✅
- C. Day 7
- D. Day 3

*Rationale:* An FS + 3 lag delays the successor three days beyond the predecessor's finish: `10 + 3 = 13`. A
ignores the lag; C treats the lag as a lead (FS − 3); D reads the lag itself as the start date.

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
- B. 14 days ✅
- C. 10 days
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
- B. 0 ✅
- C. 14
- D. 5

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

- **Setup:** two 4-day activities, **C and E**, were planned in **parallel** but both need the **same single
  specialist crew** (only one available).
- **Formula:** with one crew they must run **in sequence**; the added duration is the second activity's
  duration, constrained by available float.
- **Substitution:** C then E in sequence adds up to **4 days** of work that cannot overlap; C has 4 days of
  total float (from the network), so if E is the constrained one, part of the delay is absorbed by float, but
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
- B. Critical-path activities with the lowest cost per time saved. ✅
- C. The longest-duration activity regardless of path.
- D. Non-critical activities.

*Rationale:* Only critical-path activities shorten the project; among those, pick the cheapest per time saved.
Crashing float/non-critical activities adds cost without shortening the project.

**MCQ 10.3-B `[10.3.2 · Analysis]`** Fast-tracking primarily trades:
- A. Cost for time.
- B. Time for risk (overlapping raises rework risk). ✅
- C. Scope for cost.
- D. Quality for schedule.

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

### Key terms — KA 10.4

| Term | Meaning |
|---|---|
| **Progressing / data date** | Recording actuals and remaining durations, then recalculating. |
| **Baseline comparison** | Current vs baseline milestones and critical path. |
| **Schedule increment** | A Sprint/release as a time-phased unit mapped to milestones. |

### Sample MCQs — KA 10.4

**MCQ 10.4-A `[10.4.2 · Analysis]`** Why read both the network view and the earned-value view of schedule?
- A. They always agree.
- B. Each covers the other's blind spot — EVM misses the critical path; the network does not aggregate cost/performance. ✅
- C. Only one is ever correct.
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
- B. Day 43 ✅
- C. Day 40
- D. Day 42

*Rationale:* A critical slip passes through day for day (`40 + 3 = 43`), while the non-critical slip is
absorbed within its 5 days of float. A wrongly adds both slips; C ignores the critical slip; D applies the
absorbed slip instead of the critical one.

**MCQ 10.4-D `[10.4.1 · Analysis]`** A schedule is updated with several actual finish dates missing and key
milestones held on fixed date constraints. The main consequence is:
- A. The forecast is more reliable because the milestone dates are protected.
- B. The network can no longer recalculate honestly — the forecast completion and current critical path are corrupted. ✅
- C. Total float increases across the network.
- D. The baseline is automatically re-approved.

*Rationale:* Missing actuals and forced constraints break the logic-driven recalculation, hiding slippage and
moving critical paths — the schedule analogue of the cost data-integrity failures of Domain 5. A mistakes
concealment for protection; C and D do not follow from a corrupted update.

### Self-check — KA 10.4

1. Why must the critical path be re-identified when progressing a schedule? *(Actual progress can move the
   critical path to a different chain.)*
2. How do agile releases relate to a classical schedule? *(As schedule increments mapped to milestones and
   reconciled with the CPM view.)*

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

*Domain 10 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
