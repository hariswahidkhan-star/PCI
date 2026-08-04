# Domain 6 — Planning, Scheduling and Delivery Flow
## Why this domain exists

A project leader's authority rests on one repeated act: making a credible statement about the
future — when the work will finish, what could move that date, and what it would cost to move it
back. This domain builds the machinery behind that credibility. It starts where planning actually
starts — with levels of plan and the logic that binds work together (KA 6.1); builds the critical
path method in full, forward and backward passes, and both kinds of float (KA 6.2); adds the
realities the textbook network ignores — resources, constraints, milestones and rolling-wave
elaboration (KA 6.3); and finishes with delivery flow across predictive, agile and hybrid worlds:
recovery, compression economics, and forecasting under uncertainty (KA 6.4). Cost joins schedule
in Domain 7 (earned value); risk quantification deepens in Domain 8. A leader who cannot read a
network diagram is hostage to whoever can; this domain removes the hostage-taking.

**Learning objectives.** After this domain a candidate can: choose the right planning level for
an audience and a decision; build a logic network with correct dependency types; **model an
approval as a dated event against a governance calendar and compute what mis-modelling it costs**;
run forward and backward passes and derive total and free float; identify the critical path and
near-critical paths, **enumerate every path with its float, choose a near-critical band and price
the monitoring decision**; explain why float is not spare time and **why activity floats must
never be summed**; plan resources against the schedule, **produce a resource-feasible schedule
under a hard cap, distinguish the critical chain from the critical path and price the peak that
binds**; apply rolling-wave elaboration honestly and **price the commitment exposure a ranged
planning package carries**; **build a compression menu with cost slopes, locate the least-cost
duration and state the range of week-values over which it holds**; **state the non-financial ceiling
on each compression lever and the second signature a shift, night-working or simultaneous-operations
move requires**; price fast-tracking as a
probabilistic decision with a **breakeven rework probability**; distinguish predictive, agile and
hybrid scheduling, **translate a throughput forecast into a required rate through the backward
pass**, and govern each; **size and manage a schedule buffer from aggregated safety, and forecast
from buffer consumption**; **convert a repetitive-delivery date into a takt, a crew count and a
benefit integral**; and use three-point estimates and scenario analysis to forecast completion
with stated uncertainty — with any AI-generated schedule subject to the family's verification
rule.

**The master worked project.** One project runs through this domain and returns in Domains 7 and
8: **Project Auriga**, a control-systems upgrade for a regional utility, planned in weeks:

| ID | Activity | Duration (wk) | Predecessors |
|---|---|---|---|
| A | Mobilise | 2 | — |
| B | Detailed design | 6 | A |
| C | Procure control hardware | 8 | B |
| D | Civil and cabling works | 7 | B |
| E | Installation | 5 | C, D |
| F | Testing and commissioning | 4 | E |
| G | Operator training and handover | 2 | D |

Every calculation below uses this network. Auriga's `BAC` is **USD 4,000,000** and the value of a
week — the client's early-completion bonus, and equally the cost of a week's delay — is
**USD 45,000**, the figure Domains 7, 8 and 9 use unchanged. An engineer-week costs **USD 5,225**
(Domain 7, KA 7.4.1).

**The second scale.** Scheduling questions change shape between one project and a rolling
programme, so this domain also works the family's programme case, **Meridian Care Records** — the
clinical-records rollout to **40 clinics**, approved cost **USD 2,400,000**, benefit
**USD 979,200** a year at full potential and **USD 685,440** at the realistic 70 % adoption, with a
**cost of delay of USD 14,280 per week** derived in Domain 1 and a steering committee whose
expected decision latency Domain 3 computes as `E[wait] = M/2 + L =` **4 weeks** from a
four-weekly meeting and a two-week paper deadline. Meridian supplies what Auriga cannot: a
governance calendar the network has to obey (KA 6.1.2), a ranged planning package (KA 6.3.3) and
forty near-identical units of repetitive work whose schedule is a *rate* rather than a network
(6.A.5). A deployment specialist-week costs **USD 4,200** (Domain 1, KA 1.3.3b).

---

## Knowledge Area 6.1 — Planning levels and logic networks

*Topics: 6.1.1 levels of plan · 6.1.2 dependencies and logic · 6.1.3 network rules and quality.*

### 6.1.1 Levels of plan

**The principle.** One schedule cannot serve a board, a site supervisor and a lender at once.
Mature delivery organisations maintain a **hierarchy of schedules**, each derived from the one
below, each honest with the one above:

| Level | Name | Audience & decision | Typical granularity |
|---|---|---|---|
| L1 | Executive summary | Board, sponsor — go/no-go, milestones | 10–30 bars |
| L2 | Management schedule | Steering, PMO — stage gates, interfaces | 50–300 activities |
| L3 | Control schedule | Project team — the CPM network; progress and float live here | 300–5,000 activities |
| L4 | Execution/lookahead | Site and squads — 2–6 week detail | daily/shift detail |

The **control schedule (L3)** is where this domain's machinery operates: it carries the logic,
the floats and the critical path, and it is the version under change control (Domain 4, KA 4.3).
An L1 milestone that cannot be traced to L3 logic is decoration, and steering committees should
treat it as such (Domain 3, KA 3.2).

> **Fig 6.1.1 — The schedule hierarchy: one logic, four honest views.** Layered diagram, four
> horizontal bands labelled L1 Executive (10–30 bars), L2 Management (50–300 activities),
> L3 Control — the CPM network (300–5,000 activities, highlighted in brand blue with "logic ·
> float · critical path live here"), L4 Execution/lookahead (daily detail). Upward arrows
> labelled "summarised from"; downward arrows labelled "traceable to"; a crimson side note at L3:
> "under change control". Source: PCI original. Alt text: four stacked schedule levels from
> executive summary to daily lookahead, with the control schedule highlighted as the single
> source all other views summarise.

### 6.1.2 Dependencies and logic

**Definitions.** Activities link through four dependency types — finish-to-start (**FS**, the
default: design finishes, then procurement starts), start-to-start (**SS**: cabling can start
once civils have started, often with a lag), finish-to-finish (**FF**: commissioning documentation
finishes when commissioning finishes), and the rare start-to-finish (**SF**). A **lag** delays a
successor (`FS+2` = start two weeks after the predecessor finishes); a **lead** (negative lag)
overlaps it. Logic expresses *physics and choice* — what must follow, and what the team has
chosen to sequence — and the two should be distinguishable in the schedule's notes, because only
chosen logic can be re-chosen when recovery demands it (KA 6.4).

**Common pitfall — the lag that hides work.** A 3-week lag labelled "curing" is physics; a 3-week
lag labelled nothing is often an activity somebody didn't model — unresourced, uncosted,
invisible to progress measurement. Audit rule: every lag has a stated reason, and long lags
(> 10 % of project duration) are activities in disguise.

**Worked example 6.1.2 — re-choosing a dependency.**

1. **Setup.** Survey work P (4 weeks) precedes report drafting Q (6 weeks), currently linked
   `FS+2` (two weeks of data cleaning after all surveying ends). The team proposes drafting in
   parallel as survey sections complete: `SS+1`. Compute Q's dates both ways (P starts week 0).
2. **Formula.** FS+lag: `ES(Q) = EF(P) + lag`. SS+lag: `ES(Q) = ES(P) + lag`.
3. **Substitution.** FS+2: `ES(Q) = 4 + 2 = 6`, `EF(Q) = 12`. SS+1: `ES(Q) = 0 + 1 = 1`,
   `EF(Q) = 7`.
4. **Result.** The chain finishes week **12** under FS+2 and week **7** under SS+1 — five weeks
   earlier from one logic choice.
5. **Interpretation.** Nothing was crashed and nobody works faster: the saving comes from
   overlapping, and its price is rework risk if early sections change after later surveying.
   This is why chosen logic must be visibly chosen (not fossilised as physics) — it is the
   cheapest recovery currency the project owns (KA 6.4.2), and spending it is a risk decision,
   not a scheduling trick.

   **The risk is priceable, and the breakeven is reassuring.** Suppose the worst credible
   consequence is that late survey sections invalidate the early drafting and Q must be rewritten
   in full — six weeks lost — with probability `p`. The expected saving is `5 − 6p` weeks, so the
   overlap stops paying at `p = 5/6 =` **83.33 %**. At a more plausible `p` = 30 % the expected
   saving is `5 − 1.8 =` **3.20 weeks**. Re-choosing a dependency is therefore an unusually
   favourable trade: the saving is banked immediately and in full, while the loss is contingent —
   which is exactly the asymmetry KA 6.4.2 exploits again for fast-tracking, and exactly the
   asymmetry that makes overlap the first move in any recovery.

   **Two things break that comfortable arithmetic, and a reviewer should ask about both.** The
   first is **detectability**. The model above assumes the rework is discovered while the report
   is still in draft. If the invalidated early sections reach a published report unnoticed, the
   consequence is not six schedule weeks but an escaped defect in a delivered product, priced by
   Domain 9's containment economics on a ladder an order of magnitude steeper. Overlap is safe in
   proportion to the strength of the check that sits between the overlapping activities. The
   second is that `p` is not a constant but a function of *how much* overlap is taken: `SS+1`
   drafts on one week of survey data, `SS+3` on three. Where a team can state the relationship it
   should optimise the overlap; where it cannot — the usual case — it should take the overlap that
   a downstream check can catch and say so, rather than pretending a single `p` describes every
   possible degree of overlap.

**Worked example 6.1.2b — the approval that was drawn as a lag.**

1. **Setup.** Meridian's wave-2 clinic fit-out design must be approved by the steering committee
   before the estate contractor mobilises. The planner has drawn this as `FS+3` on the
   design → mobilise link, labelled "approval". The committee meets every **M = 4** weeks —
   programme weeks 20, 24 and 28 — and papers are due **L = 2** weeks before each meeting
   (weeks 18, 22, 26). The design's forecast finish is **week 23**. The path carrying the approval
   has **2 weeks** of total float. Cost of delay **USD 14,280** per week (Domain 1, KA 1.3.3).
2. **Formula.** A submission finishing at `t` reaches the first meeting `m` for which
   `m − t ≥ L`; the wait is `m − t`. Its range across arrival times is `L` (best) to `M + L`
   (worst), and its mean for a decision arriving at a uniformly random point is `E[wait] = M/2 + L`
   (derived in Domain 3, KA 3.2.3 — not re-derived here).
3. **Substitution.** Finish week 23: the week-24 meeting closed its papers at week 22, so the first
   reachable meeting is **week 28** and the wait is `28 − 23 = 5` weeks. Finish week 22 instead:
   papers for the week-24 meeting are still open, so the wait is `24 − 22 = 2` weeks.
4. **Result.** The scheduled wait is **5 weeks**, not the 3 weeks drawn and not the 4-week
   expectation, so the approval lands in **week 28**. The 2-week understatement **exactly consumes
   the path's float**: `TF` falls from 2 to **0**. Pulling the design finish one week earlier, to
   week 22, cuts the wait to **2 weeks** and the approval to **week 24** — so the successor starts
   **4 weeks** earlier for **one week** of design work. Note the asymmetry: the *wait* shortens by
   3 weeks (5 → 2) while the *approval date* moves by 4 (28 → 24), because the design's own week is
   saved as well. Four weeks are worth `4 × 14,280 =` **USD 57,120**, or **USD 45,120** net of a
   USD 12,000 compression cost.
5. **Interpretation. A mis-modelled lag makes a path critical while every duration is still "as
   planned".** No
   activity slipped, no estimate changed, nobody was late — and the path went from two weeks of
   protection to none, purely because a three-week label stood in for a five-week event. This is
   the pitfall above made arithmetical: the schedule was not optimistic, it was *silent*, and
   silence is not conservative. The audit habit follows directly — for every lag, ask what event it
   represents, then ask what that event's own logic is.

   **Once the network exists, the wait is not a random variable — it is a choice.** Domain 3's
   `E[wait] = M/2 + L =` **4 weeks** is the correct planning figure for a decision arriving at an
   unknown time, which is what an escalation is. A *planned* approval is not that: its arrival date
   is a date the planner controls, so the honest model is deterministic against the meeting
   calendar, and the wait ranges from **2 to 6 weeks** depending on where the finish lands.
   Substituting the expectation for a planned submission is a category error in both directions —
   it flatters a submission that will miss its window and penalises one that will make it.

   **The highest-leverage week in a governance-bound schedule is the one immediately before a paper
   deadline.** One week of compression bought four weeks of schedule here: a **4:1** return, and it
   arises entirely from the discreteness of the calendar. Compare it with Domain 3's own lever —
   shortening the paper lead time `L` by one week saves one week of expected wait, worth
   **USD 14,280**. The scheduling lever is four times the governance lever *on this submission*,
   costs nothing to exercise, and needs nobody's permission. It is also the more fragile: it works
   only if the submission actually lands before the deadline, so the planner who claims it must
   also protect it, which means treating the paper deadline as a milestone with float, not as an
   administrative detail.

   **The cautions.** The 4:1 leverage is a property of this arrival date, not of the calendar —
   a design finishing at week 21 gains nothing from compression because it already makes the
   week-24 meeting, and a leader who reads "compress the design" as a general rule will buy weeks
   that convert into nothing. The gain is realised only if the approval sits on a binding path; on
   a path with four weeks of float the whole analysis is worth zero and the compression money
   should be spent elsewhere. And the wait is the *committee's* latency only — a deferral, a
   request for more information or a conditional approval restarts the cycle, which is why the
   register in toolkit 6.T.3 records approval events with their meeting dates rather than with
   nominal durations.

### 6.1.3 Network rules and quality

A network the passes can trust obeys checkable rules: every activity except the first has a
predecessor and except the last a successor (**no dangles**); no loops; durations estimated by
the owning discipline, not the scheduler; no date constraints doing logic's job (a "must start
on" pin that overrides logic converts the schedule from a model into a poster); and logic density
in a healthy range — too few links and float is fiction, too many and nothing can move. These
rules are the schedule-quality gate a leader can run without scheduling software: ask for the
dangle count, the constraint count and the reasons for both.

**Two of those rules are countable, and counting them is the whole skill.** *Logic density* is
links ÷ activities. Auriga carries **7** dependency links across **7** activities — a density of
**1.00** — and the number a reviewer compares it against is structural rather than conventional:
binding `n` activities into one connected network takes at least `n − 1` links, so the floor here is
**6** and Auriga sits exactly one link above it. That one surplus link is the second predecessor of
E, which is to say **the surplus over `n − 1` is the count of genuine convergences in the
network** — a directly meaningful figure, and the reason a density materially below 1 on a large
network is proof of dangles rather than evidence of elegant simplicity. At the other extreme a
density far above 2 usually means the same constraint has been expressed several times, which makes
float unresponsive because every activity is held by a redundant link. *Float distribution* is the
share of activities at or near zero float.
Auriga shows `5/7 =` **71.4 %** at zero float, which on a real network would be a warning that the
logic is over-tight or the constraints are doing the work.

Here the professional caution matters more than the metric: **Auriga is a seven-activity teaching
network, and neither number means anything at that size.** Both measures are properties of a
population of activities, and both take their meaning from an organisation's own calibrated
history — a construction contractor's healthy density is not a software programme's. The
transferable habit is therefore the arithmetic and the direction of the inference, not a benchmark:
compute density and the zero-float share on your own portfolio of accepted schedules, and treat a
new schedule that sits far outside your own distribution as a question to be answered rather than a
defect to be asserted. A reviewer quoting a universal threshold for either is quoting something
nobody derived.

### AI in this KA

Scheduling tools now draft networks from scope statements and historical libraries. Useful — the
draft logic is often 80 % right — and dangerous in a specific way: generated logic *looks*
authoritative while encoding assumptions nobody made. The governed workflow: AI proposes the
network; the discipline leads walk every link of their scope ("does B truly need all of A
finished?"); the scheduler runs the quality rules above; and the leader signs the logic as a
decision record (Domain 3, KA 3.3.4). **AI proposes; the professional verifies, decides and
remains accountable.**

### Key terms — KA 6.1

| Term | Meaning |
|---|---|
| **Schedule hierarchy (L1–L4)** | Nested plans for different audiences, derived from one logic. |
| **Control schedule** | The L3 CPM network under change control; home of float and progress. |
| **FS / SS / FF / SF** | The four dependency types. |
| **Lag / lead** | Imposed delay / overlap on a dependency. |
| **Dangle** | An activity missing a predecessor or successor; a network defect. |
| **Date constraint** | A pinned date overriding logic; each one weakens the model. |
| **Logic density** | Links ÷ activities; the surplus over `n − 1` counts the network's convergences. |
| **Governance calendar** | The meeting and paper-deadline dates an approval event must be scheduled against, rather than approximated by a lag. |

### Sample MCQs — KA 6.1

**MCQ 6.1-A `[6.1.2 · Application]`** Cabling may begin one week after civil works begin. The
correct dependency is:
- A. FS+1
- B. SS+1 ✅
- C. FF+1
- D. FS−1

*Rationale:* The condition binds the two *starts* with a one-week lag: start-to-start plus one.
A would wait for all civils to finish; C binds the finishes; D (a lead on FS) overlaps the finish,
which is not what was stated.

**MCQ 6.1-B `[6.1.3 · Analysis]`** A schedule shows 14 "must finish on" constraints, all on
milestones reported to the board. The most accurate reading is:
- A. the schedule is well-governed, because board dates are protected
- B. the milestones will be met, because the tool will honour the constraints
- C. the schedule may no longer model reality — pinned dates can mask logic-driven slippage that float analysis would otherwise reveal ✅
- D. constraints are neutral scheduling hygiene with no analytical effect

*Rationale:* Pins override logic: the network can be slipping while pinned milestones hold still,
hiding negative float until it is unrecoverable. A and B mistake suppression for control; D is
false — every pin degrades the model's predictive value.

**MCQ 6.1-E `[6.1.1 · Recall]`** The schedule level that carries the logic, the floats and the
critical path — and sits under formal change control — is:
- A. L1, because the board owns the schedule
- B. L2, because the PMO maintains it
- C. L3, the control schedule ✅
- D. L4, because it has the most detail

*Rationale:* The L3 control schedule is the analytical model; L1/L2 summarise it and L4
elaborates near-term execution from it. Detail (D) is not the same as control — L4 churns weekly
by design and is never the baselined network.

**MCQ 6.1-C `[6.1.2 · Application]`** Survey P (4 wk) feeds report Q (6 wk). Under `FS+2` the
chain completes week 12. Re-linked `SS+1`, it completes week:
- A. 7 ✅
- B. 11
- C. 12
- D. 5

*Rationale:* `ES(Q) = ES(P) + 1 = 1`, `EF(Q) = 7`. B subtracts one lag week from 12; C assumes
logic changes nothing; D forgets Q's own duration must still run.

**MCQ 6.1-D `[6.1.3 · Recall]`** A "dangle" in a schedule network is:
- A. an activity with negative float
- B. an activity missing a predecessor or successor link ✅
- C. a milestone with zero duration
- D. a lag longer than its predecessor

*Rationale:* Dangles are unbound activity ends — the passes cannot constrain them, so their
dates and float are unreliable. A describes constraint-driven lateness; C describes every
milestone; D is unusual but legal when justified.

**MCQ 6.1-F `[6.1.2 · Analysis]`** A design finishing in week 23 needs approval from a committee
meeting in weeks 20, 24 and 28 whose papers are due two weeks before each meeting. The planner has
drawn the approval as a three-week lag and the path carries two weeks of total float. The correct
reading is:
- A. the lag is conservative, since the average wait for this committee is four weeks
- B. the approval will actually take five weeks, the two-week understatement consumes the path's float, and the path is critical with no duration having changed ✅
- C. the wait is four weeks, so one week of float remains
- D. the lag is irrelevant because approvals are not project work

*Rationale:* Week 23 misses the week-24 meeting (papers closed at week 22), so the first reachable
meeting is week 28 and the wait is 5 weeks against 3 drawn (6.1.2b). A calls a three-week lag
conservative against a five-week event; C substitutes Domain 3's expectation for a *planned*
submission whose date is known, which is the category error 6.1.2b names; D is the lag-that-hides-work
pitfall stated as a principle.

**MCQ 6.1-G `[6.1.3 · Analysis]`** A 400-activity network reports 310 dependency links. Before
looking at anything else, a reviewer can conclude that:
- A. the logic is admirably lean
- B. the network cannot be fully connected, so it contains dangles ✅
- C. the network is over-linked and float will be unresponsive
- D. nothing at all — link counts carry no information

*Rationale:* Connecting 400 activities needs at least 399 links, so 310 proves unbound activities
exist (6.1.3). A mistakes a structural impossibility for economy; C is the opposite defect, which
appears at densities far above 2; D denies a countable check that costs one division.

### Self-check — KA 6.1

1. *Why must chosen logic be distinguishable from physical logic?* — Because recovery (KA 6.4)
   works by re-choosing choices; physics cannot be renegotiated.
2. *What three numbers give a fast schedule-quality read?* — Dangle count, constraint count,
   unexplained-lag count — each should be at or near zero, with reasons for the rest.
3. *When is Domain 3's `E[wait] = M/2 + L` the wrong figure for a schedule?* — Whenever the
   submission date is planned rather than random: the wait is then a deterministic function of the
   finish date against the meeting calendar, ranging from `L` to `M + L` (2 to 6 weeks on Meridian).
4. *What does a logic density of exactly 1.00 on a seven-activity network tell you?* — That the
   network has one link more than the six needed to connect it, so it contains exactly one
   convergence — and that the metric is meaningless at this size.

---

## Knowledge Area 6.2 — The critical path and float

*Topics: 6.2.1 the forward and backward passes · 6.2.2 total and free float · 6.2.3 reading the
critical path.*

### 6.2.1 The forward and backward passes

**Definitions.** For each activity: **`ES`/`EF`** — earliest start/finish (forward pass, left to
right: `ES` = latest `EF` of predecessors; `EF = ES + duration`); **`LS`/`LF`** — latest
start/finish that do not delay the project (backward pass, right to left: `LF` = earliest `LS` of
successors; `LS = LF − duration`). The project duration is the largest `EF` in the network.

**Worked example 6.2.1 — Auriga, both passes.** The passes are inherently tabular; the labelled
table replaces the five-step skeleton, and the interpretation follows (pattern rule).

| Act | Dur | ES | EF | LS | LF | `TF` | `FF` |
|---|---|---|---|---|---|---|---|
| A | 2 | 0 | 2 | 0 | 2 | 0 | 0 |
| B | 6 | 2 | 8 | 2 | 8 | 0 | 0 |
| C | 8 | 8 | 16 | 8 | 16 | 0 | 0 |
| D | 7 | 8 | 15 | 9 | 16 | 1 | 0 |
| E | 5 | 16 | 21 | 16 | 21 | 0 | 0 |
| F | 4 | 21 | 25 | 21 | 25 | 0 | 0 |
| G | 2 | 15 | 17 | 23 | 25 | 8 | 8 |

*Interpretation.* Project duration **25 weeks**; the **critical path is A–B–C–E–F**
(2+6+8+5+4 = 25), every activity on it with zero float. D can slip one week without moving the
end date; G can slip eight. The passes turn a drawing into an instrument: every schedule
statement a leader makes in this domain — "we finish in week 25", "D matters more than G",
"procurement drives the job" — is read directly off this table.

Three checks confirm the table before anything is read off it, and they are the same three a
reviewer runs on any pass, human or machine. **The critical path's durations sum exactly to the
project duration:** `2 + 6 + 8 + 5 + 4 = 25`. **Every critical activity shows `ES = LS` and
`EF = LF`:** A, B, C, E and F all do; D and G do not, and their gaps are precisely their floats.
**Every activity's `TF` equals both `LS − ES` and `LF − EF`:** for G, `23 − 15 = 8` and
`25 − 17 = 8`. A pass failing any of the three is arithmetically broken, and the failure is visible
in seconds without re-running anything.

What the table also shows, and what the eye misses, is that **the second path is one week from
binding**. A–B–D–E–F runs 24 weeks — 4.00 % below the project duration — so the network's whole
protection against a slip in civil works is a single week. That is the observation KA 6.2.3 turns
into a monitoring decision, and it is why float is the number to report rather than the critical
path's membership list.

> **Fig 6.2.1 — The Auriga network with both passes.** Activity-on-node diagram, seven nodes
> A–G laid left to right; each node shows Dur, ES/EF (top), LS/LF (bottom), TF. Critical path
> A–B–C–E–F drawn in brand blue with heavier arrows; near-critical D in a mid tone; G in light
> grey. Legend explains the node layout. Source: PCI original. Alt text: network diagram of seven
> linked activities with earliest and latest dates in each node and the zero-float path A, B, C,
> E, F highlighted as the critical path of twenty-five weeks.

### 6.2.2 Total and free float

**Definitions.**

```
TF = LS − ES          — how far an activity can slip without delaying the project
FF = min(ES of successors) − EF   — how far it can slip without delaying ANY successor
```

`FF ≤ TF` always. The Auriga table carries the domain's subtlest lesson: **D has `TF` = 1 but
`FF` = 0** — D can slip a week without moving the end date, but the moment it slips at all, G's
earliest start moves. Total float is a project-level commodity; free float is a courtesy to your
successors. A leader who grants a subcontractor "the float" without saying which kind has just
given away someone else's schedule.

**Float is not spare time.** Float belongs to the *path*, not the activity: consume D's week and
every activity sharing that path inherits zero. Contract regimes differ on who owns float
(Domain 10, KA 10.3, treats float ownership clauses); the delivery answer is simpler — float is
a risk buffer under the leader's governance, spent deliberately or not at all.

**Worked example 6.2.2 — why activity floats must never be added.**

1. **Setup.** A float report for Auriga lists two activities with float: D (`TF` = 1) and
   G (`TF` = 8). A subcontract manager reads the report as "the network carries nine weeks of
   slack" and proposes to spend it. How much cumulative slip can the network actually absorb on
   that chain without moving week 25, and what does the difference cost?
2. **Formula.** Float is a property of a **path**, not an activity. For a chain of activities whose
   only route to the end is one path of length `Lp`, the absorbable slip is the *path* float
   `PD − Lp`, shared among every activity on it. Where an activity also lies on a tighter path, its
   own slip is additionally capped by that path's float.
3. **Substitution.** Path A–B–D–G runs `2 + 6 + 7 + 2 = 17` weeks against `PD` = 25, so its path
   float is **8**. Let D slip `d` and G slip `g`: the path finishes at `17 + d + g`, so
   `d + g ≤ 8`. D also sits on A–B–D–E–F (24 weeks), which caps `d ≤ 1`.
4. **Result.** The maximum cumulative slip the chain can absorb is **8 weeks** — for example
   `d` = 1 and `g` = 7, or `d` = 0 and `g` = 8 — against the **9** weeks the report's arithmetic
   implies. The report overstates the network's absorption capacity by **one week**, which at
   Auriga's USD 45,000 a week is **USD 45,000** of false comfort.
5. **Interpretation. The overstatement factor is the number of activities on the path.** For `n` activities lying on
   a single non-critical path with path float `f`, every one of them reports `TF = f`, so the float
   register sums to `n · f` while the path can absorb only `f`. A ten-activity chain with five weeks
   of path float shows **50 weeks** of "float" and holds **five**. Summing an activity float column
   is therefore not an approximation but a category error, and it is a mistake that scales with the
   size of the schedule — which is why it is commonest on exactly the large networks where it does
   most damage.

   The practical consequence of float being shared is that two parties can each spend "their" float
   in good faith and jointly overrun, which is why float is a *governed* commodity (the float policy
   of the Executive perspective) rather than an entitlement recorded against each activity.

   **The reporting rule that follows is specific.** A float report is per **path**, not per
   activity: list each path with its length, its float and the activities on it, and state the float
   *remaining* after what has already been spent. Auriga's report has three lines, not seven. A
   report with one line per activity cannot be read correctly even by someone who knows all this,
   because the shared structure has been discarded before the reader sees it.

   **The caution against over-applying it.** Free float *is* additive in one narrow sense — an
   activity's `FF` is a genuinely local quantity, spendable without consulting anyone downstream —
   and this is exactly why `FF` is the right float to grant to a subcontractor and `TF` the wrong
   one. But `FF` is usually zero (D's is), so the honest answer to "how much float may we have?" is
   most often "none that is yours alone; here is what the path holds and who else is drawing on
   it". A counsel pointer belongs here, because float ownership is enforceability-sensitive:
   whether float is the contractor's, the employer's or the project's, and what a grant of it
   concedes about later delay claims, is a matter of the contract and the jurisdiction, varies
   between regimes and delay-analysis protocols, and should be taken from qualified legal advice
   (Domain 10, KA 10.3) rather than from a scheduling convention. The arithmetic above is
   contract-neutral; the answer given to the counterparty is not.

**Common pitfall — managing only the critical path.** D sits one week from critical. A team
watching only A–B–C–E–F will discover D's importance the week it becomes too late to matter.
Near-critical analysis (all paths within, say, 10 % of project duration) is standing practice;
KA 6.4's scenario work builds on it, and KA 6.2.3 prices it.

### 6.2.3 Reading the critical path

The critical path is the *longest* path and therefore the *shortest possible duration* — both
statements are the same fact, and fluency means holding both. Three leader-grade readings of the
Auriga table: **(1)** procurement (C) is the single most schedule-driving activity — an
expediting conversation is worth more than any amount of site pressure; **(2)** the C/D pair
converge on E, so E's start is hostage to the later of two chains — convergence points are where
slippage compounds and where progress review should focus (Domain 8, KA 8.1); **(3)** G's eight
weeks of float make it the natural donor of resources in any recovery (KA 6.4).

**Worked example 6.2.3 — choosing the near-critical band, and paying for it.**

1. **Setup.** Auriga's team must decide what to monitor weekly. Enumerate every path, choose a
   near-critical band, and price the decision. Extending the weekly review to a second path costs
   about **0.5 engineer-weeks** per cycle for the **12** remaining cycles at **USD 5,225** an
   engineer-week (Domain 7, KA 7.4.1). A week of lateness costs **USD 45,000**.
2. **Formula.** Path float = `PD − path length`. A path is near-critical if its float ≤ the chosen
   band, conventionally a percentage of `PD`. Monitoring pays if `cost ≤ weeks of lateness avoided ×
   cost of delay`.
3. **Substitution.** Paths: A–B–C–E–F `= 2+6+8+5+4 = 25`; A–B–D–E–F `= 2+6+7+5+4 = 24`;
   A–B–D–G `= 2+6+7+2 = 17`. Band at 10 %: `0.10 × 25 = 2.5` weeks. Monitoring cost
   `0.5 × 12 × 5,225`.
4. **Result.**

   | Path | Length (wk) | Float (wk) | Float as % of `PD` | Inside a 10 % band? |
   |---|---|---|---|---|
   | A–B–C–E–F | 25 | 0 | 0.00 % | yes — critical |
   | A–B–D–E–F | 24 | 1 | 4.00 % | **yes** |
   | A–B–D–G | 17 | 8 | 32.00 % | no |

   Two of three paths fall inside the band, and because they share A, B, E and F they cover
   **6 of 7** activities — **85.71 %** of the network. Monitoring cost
   `0.5 × 12 × 5,225 =` **USD 31,350**, recovered by avoiding
   `31,350 / 45,000 =` **0.6967 weeks** of lateness.
5. **Interpretation. The band captures almost everything, and that is the point.** On a well-formed network the
   near-critical set is not a short annexe to the critical path but most of the schedule — here
   85.71 % of activities. "Manage the critical path" is therefore not a management system; it is a
   *reporting* convention that survives because it fits on a slide. What the leader actually needs is
   the float column sorted ascending, with a line drawn where the band falls.

   **The band must be wider than the largest float you are unwilling to lose.** A–B–D–E–F's float is
   1 week, which is **4.00 %** of 25 — so any band of 4 % or more captures it and any band below 4 %
   does not. A team that had adopted a 2 % band (0.5 weeks) would monitor the critical path alone and
   would be blind to the path that is one week from binding. The percentage is not a convention to be
   inherited; it is a decision made by asking *which paths would I want to hear about?* and then
   setting the band to include them. Quoting a band without saying what it captures is the same
   defect as quoting a confidence level without a date (Domain 8, KA 8.A.2).

   **The monitoring economics are not close.** Watching a second path to handover costs USD 31,350
   and pays for itself if it prevents **0.70 weeks** of lateness once. If D slips two weeks
   undetected, E cannot start until week 17 and F finishes week 26 — a full week late,
   **USD 45,000**, or **1.44 times** the entire cost of monitoring. Near-critical analysis is one of
   the few schedule controls whose case is so lopsided that it should not need making; it is omitted
   for want of the arithmetic rather than for want of the money.

   **The cautions.** Path enumeration is trivial on seven activities and combinatorially hopeless on
   five thousand, where the practical instrument is the *float histogram* — count activities by float
   band and read the shape — rather than a path list. Path float is computed against the *current*
   network, so it changes with every crash and every slip (KA 6.4.2's path migration); a band chosen
   once and never revisited will be monitoring last quarter's near-critical set. And float measured
   against logic is not float against **capacity**: KA 6.3.1c shows an activity with eight weeks of
   logical float extending the project once a resource cap binds, so a watch-list drawn only from the
   float column can still miss what will actually make the project late.

> **Fig 6.2.2 — Auriga's three paths, their floats, and the near-critical band.** Horizontal bar
> chart, x-axis "path length (weeks)" 0–26. Three bars drawn to length with their durations summed
> beneath each label: A–B–C–E–F 25 weeks (brand blue, solid, "float 0"), A–B–D–E–F 24 weeks (brand
> blue at 55 % opacity, "float 1"), A–B–D–G 17 weeks (grey, "float 8"). A solid ink vertical line at
> 25 weeks labelled "project 25 wk"; a dashed brand-blue line at 22.50 weeks labelled "10 % band",
> with the band between them shaded. A side note records "inside the band: 2 of 3 paths, 6 of 7
> activities = 85.71 %" and "any band at or above 4.00 % of 25 wk captures A–B–D–E–F". Source: PCI
> original. Alt text: three horizontal bars of decreasing length showing path durations of
> twenty-five, twenty-four and seventeen weeks, with a shaded band covering the two longest paths to
> show which paths a ten per cent near-critical threshold captures.

### AI in this KA

CPM arithmetic is deterministic — ideal for machine computation and instant human verification.
The two checks that catch nearly every pass error: the critical path's durations must **sum
exactly** to the project duration, and every critical activity must show `TF` = 0 with
`ES = LS`. An AI-produced schedule that fails either is broken; one that passes both may still
have wrong *logic* — which is why KA 6.1's link-walking discipline precedes trust in any pass,
human or machine.

### Key terms — KA 6.2

| Term | Meaning |
|---|---|
| **Forward / backward pass** | Left-to-right ES/EF computation; right-to-left LS/LF computation. |
| **`ES` `EF` `LS` `LF`** | Earliest/latest start and finish per activity. |
| **Total float `TF`** | `LS − ES`; slip available without delaying the project. |
| **Free float `FF`** | Slip available without delaying any successor; `FF ≤ TF`. |
| **Critical path** | The zero-float longest path; the project's shortest possible duration. |
| **Convergence point** | A node where paths merge; slippage compounds there. |
| **Path float** | `PD` − path length; the slip a whole chain can absorb, shared by every activity on it. |
| **Near-critical band** | The float threshold below which a path joins the monitored set; a decision, not a convention. |

### Sample MCQs — KA 6.2

**MCQ 6.2-A `[6.2.1 · Application]`** In the Auriga network, activity D (duration 7, predecessor
B finishing week 8, successors E and G) has `LS` = 9 and `ES` = 8. Its total float is:
- A. 0
- B. 1 ✅
- C. 8
- D. 7

*Rationale:* `TF = LS − ES = 9 − 8 = 1`. A confuses D with the critical activities; C is G's
float; D is the activity's duration, not its float.

**MCQ 6.2-B `[6.2.2 · Analysis]`** An activity shows `TF` = 1 and `FF` = 0. Delaying it by one
week will:
- A. delay the project by one week
- B. delay nothing, because total float absorbs the slip
- C. delay at least one successor's earliest start while leaving the project end date unchanged ✅
- D. be impossible — free float can never be below total float

*Rationale:* Positive `TF` protects the end date (not A); zero `FF` means some successor moves
immediately (not B). D inverts the invariant — `FF ≤ TF` always.

**MCQ 6.2-C `[6.2.3 · Application]`** Auriga's procurement activity C is crashed from 8 weeks to
6. The new project duration is:
- A. 23 weeks
- B. 24 weeks ✅
- C. 25 weeks
- D. 21 weeks

*Rationale:* After one week of crashing, path A–B–D–E–F (2+6+7+5+4) becomes co-critical at 24;
the second crashed week buys nothing — duration stays 24. A assumes both weeks convert to project
weeks; C forgets the crash entirely; D subtracts from the wrong baseline. (The full economics:
KA 6.4.)

**MCQ 6.2-F `[6.2.1 · Application]`** Auriga's procurement C slips from 8 weeks to 9 (all else
unchanged). The new project duration is:
- A. 25 weeks
- B. 26 weeks ✅
- C. 27 weeks
- D. 24 weeks

*Rationale:* C was critical with zero float, so its extra week passes straight through:
E runs 17–22, F finishes week 26. A assumes float absorbed a critical activity's slip; C adds
the week twice; D subtracts it.

**MCQ 6.2-D `[6.2.1 · Application]`** Auriga's training activity G runs ES 15–EF 17 with
`LF` = 25. Its total float is:
- A. 8 ✅
- B. 10
- C. 2
- D. 0

*Rationale:* `TF = LF − EF = 25 − 17 = 8` (equivalently `LS − ES = 23 − 15`). B ignores G's
duration (25 − 15); C is G's duration; D confuses G with the critical path.

**MCQ 6.2-E `[6.2.3 · Analysis]`** A programme's binding path shows `TF` = −3 after a
constraint-honest pass. The professional reading is:
- A. the software has malfunctioned — float cannot be negative
- B. the plan, as constrained, is already three weeks late; the number sizes the recovery problem and must be reported now ✅
- C. delete the constraints so the float returns to zero
- D. the project will finish three weeks early

*Rationale:* Negative float is the gap between what logic needs and what constraints allow —
information, not error (A) and not good news (D). C is the pin-and-squeeze suppression this
domain's Case B dissects; it hides the problem until it is unrecoverable.

**MCQ 6.2-G `[6.2.2 · Analysis]`** A float report for Auriga lists D at `TF` = 1 and G at `TF` = 8.
The cumulative slip the D–G chain can absorb without moving week 25 is:
- A. 9 weeks — the sum of the two floats
- B. 8 weeks, because both activities draw on the same path float of 8 ✅
- C. 1 week, the smaller of the two floats
- D. 7 weeks, since D must retain a week

*Rationale:* Path A–B–D–G runs 17 weeks against a 25-week project, so `d + g ≤ 8` (6.2.2). A sums a
shared quantity, the error that scales with schedule size — a ten-activity chain with five weeks of
path float reports fifty. C takes the minimum instead of the shared total; D invents a reservation
rule that no pass produces.

**MCQ 6.2-H `[6.2.3 · Evaluation]`** A team adopts a near-critical band of 2 % of project duration
on Auriga. The consequence is:
- A. none — 2 % is a standard threshold
- B. path A–B–D–E–F, whose float is 4.00 % of the 25-week duration, falls outside the band and goes unmonitored ✅
- C. all three paths are monitored, since 2 % is stricter
- D. the band is invalid because bands must be at least 10 %

*Rationale:* The band is 0.5 weeks and A–B–D–E–F's float is 1 week, so the second path — one week
from binding — is excluded (6.2.3). A appeals to a convention nobody derived; C inverts what
"stricter" does to a threshold; D invents a rule, when the correct test is whether the band captures
the paths the leader wants to hear about.

### Self-check — KA 6.2

1. *State both definitions of the critical path and why they are equivalent.* — Longest path
   through the network; shortest possible project duration — the longest chain of unavoidable
   work is precisely what bounds how soon the whole can finish.
2. *Why is `FF ≤ TF` always?* — Delaying any successor is one of the ways of delaying the
   project; the constraint set defining `FF` is stricter.
3. *Auriga slips: D takes 10 weeks, not 7. New duration?* — 27 weeks: D finishes week 18, E waits
   for it (18 > 16), E–F add 9 → the critical path re-routes through D (KA 6.4 works the
   recovery).
4. *Why can a float column never be summed?* — Because every activity on a path reports that path's
   float: `n` activities on a path with float `f` report `n · f` and the path holds `f`. Auriga's
   report implies 9 weeks and the chain absorbs 8.
5. *Three checks that confirm a pass in seconds?* — Critical durations sum to the project duration;
   critical activities show `ES = LS` and `EF = LF`; every `TF` equals both `LS − ES` and `LF − EF`.

---

## Knowledge Area 6.3 — Resources, constraints, milestones and rolling wave

*Topics: 6.3.1 resource planning and levelling · 6.3.2 constraints and milestones · 6.3.3
rolling-wave planning.*

### 6.3.1 Resource planning and levelling

**The principle.** The passes of KA 6.2 assume infinite resources; reality staffs the network.
Loading resources onto the schedule produces a **histogram** — demand per period — and the
histogram almost always spikes. Two responses: **resource smoothing** uses float to flatten
demand *without* moving the end date (slip D and G inside their float to pull crews off the
peak); **resource levelling** accepts a later end date if the resource cap is hard. The order of
operations is a leadership decision in disguise: smoothing spends float — the project's risk
buffer — to save money, and someone accountable should price that trade explicitly, not discover
it in a progress meeting.

**Worked example 6.3.1 — smoothing Auriga's field crews.**

1. **Setup.** Weeks 9–15: civil works D needs 3 crews; early installation staging inside C needs
   2; the site cap is 4 crews. Overlap weeks 9–15 demand 5 — one over cap.
2. **Formula.** Smoothing test: can a demanding activity slip within `TF` (and, if successors
   matter, `FF`) to clear the peak?
3. **Substitution.** D has `TF` = 1, `FF` = 0: slipping D one week clears one peak week but
   consumes D's entire float and immediately moves G (`FF` = 0). The staging work inside C has
   discretionary timing across weeks 9–16.
4. **Result.** Re-sequence the staging within C to weeks 14–16 where D's demand tails off: peak
   demand falls to 4, the cap holds, the end date holds, and **no float is spent**.
5. **Interpretation.** The cheapest capacity is re-sequencing; the second cheapest is float; the
   expensive ones are overtime, second shifts and extension. A leader asks for the histogram
   *with the options priced*, not just the spike.

**Worked example 6.3.1b — when the cap is hard: levelling, priced.**

1. **Setup.** Now suppose the site cap is **3 crews** (a permit condition, not a preference).
   D needs all 3 for weeks 9–15; the 4-week staging package (2 crews) must finish before E can
   start (week 16). Delay costs USD 45,000 per week (the client's bonus forgone); a second-shift
   waiver for the staging crew costs USD 20,000 per week. What does the leader do?
2. **Formula.** Compare total cost of each feasible plan: pure levelling (extend) vs paid
   capacity (second shift) vs any float-funded re-sequence.
3. **Substitution.** Levelling: staging cannot start until D releases crews (week 16); it runs
   weeks 16–19, E starts week 20 — project 25 → 29, cost `4 × 45,000 = 180,000`. Second shift:
   staging runs weeks 12–15 off-shift, E starts on time — cost `4 × 20,000 = 80,000`. No
   float-funded option exists: D's `TF` = 1 cannot host a 4-week move.
4. **Result.** **Buy the second shift: USD 80,000** against USD 180,000 of extension — and the
   end date holds.
5. **Interpretation.** A hard cap converts a scheduling problem into a procurement problem. The
   histogram's job is to surface that conversion early enough for the cheap option to exist —
   second shifts, pre-assembly, off-site staging all have lead times of their own. Levelling
   that silently extends the project is a decision someone should have been asked to make.

   **The breakeven is wide, and saying so is what makes the recommendation robust.** The shift wins
   for any premium below **USD 45,000** a week — the cost of delay itself — so at USD 20,000 the
   shift is bought at **44.4 %** of the price at which the leader would become indifferent, and the
   quoted premium could rise **2.25 times** before extension became the better plan. That is the
   sentence for the steering paper, because it answers the only question worth asking about a quoted
   price: how wrong can it be before the decision changes? The corollary is equally useful in the
   other direction — where the cost of delay is small, levelling into an extension is the *correct*
   answer and paying a premium to hold a date is the error.

   **The comparison is only valid because both plans deliver the same scope.** Levelling to week 29
   and second-shifting to week 25 produce the same works; if the second shift also carried a quality
   consequence — night-shift error rates are not day-shift error rates — the comparison would need
   Domain 9's containment economics on the other side of the ledger, and USD 80,000 would not be the
   whole price. A reviewer's question here is always *what else changes when the shift pattern
   changes?*

   **Two cautions on the arithmetic.** The USD 45,000 assumes the extension actually costs four
   weeks of delay cost, which holds only because E is on the critical path with zero float — the same
   four-week levelling on a path with four weeks of float would cost nothing at all, and the
   histogram cannot tell you which case you are in. And the permit condition that fixes the cap at
   three crews is a genuine external constraint, not a preference: the option of simply exceeding it
   does not exist, and a plan that quietly assumes a waiver will be granted is a plan with an
   unpriced risk in it (Domain 8's register, not the schedule's float).

**Worked example 6.3.1c — the resource-feasible schedule, and why the critical path is not the
critical chain.**

1. **Setup.** Both examples above levelled one activity against a cap. This one levels the **whole
   network** against a hard cap, which is the problem a planner actually faces. Auriga's specialist
   **engineering pool** — a different resource from the field crews above, and levelled separately,
   which is itself part of the difficulty — is capped at **6 engineers**. Each activity's engineer
   demand while it runs:

   | Activity | A | B | C | D | E | F | G |
   |---|---|---|---|---|---|---|---|
   | Duration (wk) | 2 | 6 | 8 | 7 | 5 | 4 | 2 |
   | Engineers | 2 | 4 | 1 | 3 | 5 | 6 | 2 |
   | `TF` (KA 6.2.1) | 0 | 0 | 0 | 1 | 0 | 0 | 8 |

   Activities may not be split. What is the shortest resource-feasible duration, what does the
   standard priority rule give, and what should the leader buy?
2. **Formula.** Load the early-start schedule and read the histogram: demand in week `t` =
   `Σ engineers of activities running in t`. Where demand exceeds the cap, defer activities by
   priority — conventionally **minimum total float first** — and re-check. Total demand
   `= Σ (duration × engineers)`; average utilisation `= total demand ÷ (cap × duration)`.
3. **Substitution.** Total demand
   `2×2 + 6×4 + 8×1 + 7×3 + 5×5 + 4×6 + 2×2 = 4+24+8+21+25+24+4 = 110` engineer-weeks against
   `6 × 25 = 150` of capacity. Early-start profile: weeks 0–2 A alone (2); 2–8 B (4); 8–15 C+D
   (`1+3 = 4`); 15–16 C+G (`1+2 = 3`); **16–17 E+G (`5+2 = 7`)**; 17–21 E (5); 21–25 F (6).
4. **Result.** Average utilisation `110/150 =` **73.33 %**, and the cap is still breached — by
   **one engineer, in the single week 16–17**, where E (5) overlaps G (2). Three plans follow.

   | Plan | What it does | Duration | Cost consequence vs the 25-week plan |
   |---|---|---|---|
   | Minimum-float priority rule | G (`TF` 8) yields to E (`TF` 0); G then finds no 2-engineer window until F ends | **27 wk** | 2 weeks of delay = **USD 90,000** |
   | Best resource-feasible schedule | G runs weeks 15–17; **E is deferred one week** to 17–22; F 22–26 | **26 wk** | 1 week of delay = **USD 45,000** |
   | Buy the peak out | Hire **one** engineer for the single week 16–17; G 15–17, E 16–21, F 21–25 | **25 wk** | one engineer-week purchased = **USD 5,225** |

5. **Interpretation. Float against logic is not float against capacity.** G carries **eight weeks** of total float
   and is nevertheless the activity that makes the project late: under the cap it has nowhere to go,
   because every week between its earliest start and the project end is already spending five or six
   of the six available engineers. The set of activities that determines a resource-constrained
   duration is the **critical chain** — the binding sequence once both logic *and* capacity are
   honoured — and it need not be the critical path. Any statement of the form "G doesn't matter, it
   has eight weeks of float" is a statement about the logic network alone, and the logic network is
   not the project.

   **The standard priority rule loses money.** Protecting the zero-float activity is the instinctive
   and usually correct move, and here it produces **27 weeks** where deliberately slipping E by one
   week produces **26** — a difference of **USD 45,000** left on the table by following the rule.
   The reason is structural: deferring G instead pushes it past a **nine-week** block (E's five weeks
   then F's four) in which no capacity is ever free, so the rule's refusal to defer the critical
   activity by one week costs the project two. Whether protecting the critical path is right depends
   on what lies behind it, which no priority rule inspects. Resource-constrained scheduling is a
   combinatorial optimisation, not a rule
   application; priority rules are **heuristics**, they are known to be beatable, and a planner who
   presents a levelled schedule should say which rule produced it and whether anything better was
   sought. This is precisely where an optimiser earns its place (see *AI in this KA*), and it is the
   honest reason to use one: not speed, but the USD 45,000.

   **Averages cannot answer capacity questions.** The pool is **73.33 % utilised on average** across
   a schedule it cannot execute, and in the feasible 26-week plan it is only **70.51 %** utilised.
   Any resourcing conversation conducted in averages — "we have plenty of engineering capacity, we're
   only three-quarters loaded" — is a category error, because feasibility is a property of the
   **peak** and the peak is a property of the logic. The histogram is not a presentational nicety;
   it is the only instrument in the domain that answers the question the average appears to answer.

   **The whole overrun traces to one engineer-week, and that ratio is the lesson.** The breach is one
   engineer for one week. Buying it costs **USD 5,225** and saves **USD 45,000** — a return of
   **8.61 times** — because a single week of excess demand at the wrong point in the network converts
   into a full project week. The general habit: price the *excess area* of the histogram in
   resource-weeks, compare it with the cost of delay, and expect the ratio to be startling. The
   breakeven price for that engineer-week is **USD 45,000**, or **8.61 times** the internal rate,
   which is why hiring in at a premium is so often right and so rarely proposed.

   **And optimise before you buy, because a bad baseline inflates what capacity looks worth.**
   Measured against the priority rule's 27 weeks, that one engineer-week appears to save **two** weeks
   and to be worth up to **USD 90,000** — **17.22 times** the internal rate. Measured against the
   correct 26-week baseline it saves one and is worth up to **USD 45,000**. A leader negotiating a
   hire-in rate against the un-optimised schedule would rationally pay **twice** the defensible price,
   which is a general property of buying capacity against a heuristic baseline rather than a feature
   of this network.

   **Three cautions.** The
   no-splitting rule is doing real work here: allowing G to run as two separate single weeks changes
   the answer, and whether splitting is permissible is a physical and contractual question, not a
   software setting. The example levels one resource; real projects level several simultaneously,
   which is why the problem is hard rather than tedious, and why a plan feasible on engineers can be
   infeasible on cranes. And the 6-engineer cap must be *real* — a cap that is actually a budget line
   is negotiable, and levelling against a negotiable cap manufactures a delay that a conversation
   would have removed.

> **Fig 6.3.1 — Auriga field-crew histogram, before and after smoothing.** Paired bar chart,
> x-axis weeks 8–17, y-axis crews 0–6, cap line at 4. "Before" bars peak at 5 in weeks 9–15;
> "after" bars max at 4, with the staging demand moved to weeks 14–16. Source: PCI original.
> Alt text: two bar series showing a crew-demand peak above the four-crew cap smoothed under the
> cap by re-sequencing work, with the cap drawn as a horizontal line.

### 6.3.2 Constraints and milestones

**Constraints** are externally imposed dates — an outage window, a regulatory deadline, a
seasonal limit. Modelled honestly they are logic (an outage window is a predecessor with a date);
modelled lazily they are pins that falsify float (KA 6.1.3). **Milestones** are zero-duration
events that carry meaning: contractual obligations (Domain 10), gate decisions (Domain 3),
integration points with other projects (Domain 15). A milestone's date is an *output* of the
network, never an input — the moment a milestone is typed rather than computed, the schedule has
stopped forecasting and started decorating.

**Milestone hygiene:** each has an owner, an unambiguous done-definition (Domain 5's acceptance
criteria), traceable L3 logic, and — for contractual milestones — a stated float position
reported honestly to the counterparty (Domain 11's reporting ethics).

### 6.3.3 Rolling-wave planning

**The principle.** Detail decays with distance: estimating month 18's tasks at daily granularity
today manufactures precision, not knowledge. **Rolling wave** plans near-term work at execution
detail (L4) and far work at planning packages (L3 summary), elaborating each wave as it
approaches under change control.

> **Fig 6.3.2 — The rolling-wave horizon.** Horizontal timeline diagram, 0–24 months. Three
> bands: "Execution detail (L4)" covering months 0–3 with dense small task bars; "Control detail
> (L3)" months 3–9 with medium activity bars; "Planning packages" months 9–24 with three wide
> ranged bars carrying duration ranges (e.g. "14–18 wk"). A vertical "elaboration point" marker
> at month 3 with an arrow showing the wave advancing. Source: PCI original. Alt text: timeline
> showing fine-grained near-term schedule detail giving way to broad ranged planning packages in
> the far term, with an arrow marking where packages are elaborated into detail as they approach. The honesty conditions: the far waves carry *ranged* durations
consistent with the estimate class (bridging Domain 7's estimate classes and Domain 8's ranges);
elaboration is an *event* with a date, an owner and a re-baselining rule (Domain 4, KA 4.3); and
the total float shown to stakeholders reflects the planning-package uncertainty, not the false
crispness of the near wave.

**Worked example 6.3.3 — what a ranged planning package may be committed to.**

1. **Setup.** Meridian's wave-3 clinic tranche is a far-wave planning package with a ranged duration
   of **14–18 weeks**, starting at programme **week 40**. The sponsor asks for a go-live date for the
   tranche. Elaborating the package — a workshop with the estate, clinical and training leads,
   **3 planner-weeks** at **USD 4,200** — would narrow the range to **15.5–16.5 weeks**. Cost of
   delay **USD 14,280** per week. What date may be committed, and is the elaboration worth buying
   first?
2. **Formula.** Milestone range = start + duration range. Commitment exposure = range width × cost of
   delay. Elaboration is worth buying if the exposure it removes exceeds its cost — noting that
   elaboration changes the *precision* of the promise, not the duration of the work.
3. **Substitution.** Milestone range `40 + 14 =` week 54 to `40 + 18 =` week 58; mid-point week 56.
   Exposure before `= 4 × 14,280`; after `= 1 × 14,280`. Elaboration cost `= 3 × 4,200`.
4. **Result.** The tranche completes somewhere in **weeks 54–58**, mid-point **56**. Commitment
   exposure is **USD 57,120** before elaboration and **USD 14,280** after — a reduction of
   **USD 42,840** for **USD 12,600** of planning effort, a ratio of **3.40**. Committing to the
   mid-point and missing by two weeks costs **USD 28,560**.
5. **Interpretation. The mid-point is not a commitment, it is a coin toss.** Week 56 is the centre of a range, so a
   commitment to it is a commitment with roughly even odds — and the *appearance* of a firm date is
   what makes it dangerous, because nobody downstream will treat "week 56" as a 50 % statement.
   Exactly three honest options exist: commit to the upper bound (week 58, and hold the two weeks
   internally as the buffer they are), commit to the mid-point with the range stated beside it, or
   decline to commit until the package is elaborated. Which of the three is chosen is a governance
   decision (Domain 3), and Domain 8's methods set the confidence at which a buffered date should
   sit. What is not available is the fourth option everyone reaches for: quoting the mid-point as
   though it were the upper bound.

   **Elaboration is a purchasable reduction in commitment error, and it can be priced.** USD 12,600
   buys a **3.40-fold** return in exposure terms, and the arithmetic is available before the workshop
   is convened, which is what turns "we should elaborate wave 3" from an aspiration into a funded
   activity with an owner and a date. The general rule: elaborate when the exposure removed exceeds
   the elaboration cost, and elaborate *before* committing rather than after — the reverse order is
   how a programme acquires a public date it then has to defend.

   **Ranges must not be rolled up by adding mid-points.** Three consecutive tranches each ranged
   14–18 weeks do not make a 48-week programme with a 12-week range; the aggregate spread is narrower
   than the sum of the spreads where the tranches are independent, and wider where they share crews,
   estate or approvals. Domain 8, KA 8.2.4 supplies the aggregation, and this is the single most
   common quantitative error in rolling-wave reporting.

   **The warning.** Elaboration does not shorten the work, and a leader who reads the USD 42,840 as a
   saving has misread it: nothing was saved, a promise merely became more accurate. Nor does
   narrowing the range make the mid-point right — the elaborated range 15.5–16.5 could sit anywhere
   inside the original 14–18, so the elaborated *date* may be later than the date the sponsor was
   informally given. That is the moment the honesty of a rolling-wave regime is actually tested, and
   it is why the elaboration event needs a re-baselining rule agreed in advance (Domain 4, KA 4.3)
   rather than negotiated in the meeting where the number appears.

### AI in this KA

Resource optimisation is a genuine AI strength — levelling across thousands of activities and
dozens of calendars is combinatorial work machines do better than planners. Governance holds the
same shape: the optimiser *proposes* a levelled plan; the planner verifies the constraint set was
real (crew interchangeability, shift rules, site logistics); the leader owns the trade the
optimiser cannot see — whether spending float, money or scope is the right currency this month.
An optimiser given a wrong constraint produces a beautifully efficient fiction.

**Why the case is unusually strong here, and what still has to be checked.** KA 6.3.1c is the
argument in miniature: the priority rule every planner is taught produced 27 weeks where 26 was
available, and USD 45,000 turned on a search nobody had time to run by hand. Resource-constrained
scheduling is genuinely hard — the search space grows combinatorially with activities, resources and
calendars — so this is one of the few places in delivery where a machine does not merely accelerate
a human process but reaches answers humans systematically do not. The verification duties are
correspondingly specific. **Check feasibility, not optimality:** re-derive the resource histogram
from the proposed schedule and confirm no period exceeds any cap, which is one addition per period
and catches the commonest failure — an optimiser that has quietly relaxed a constraint it could not
satisfy. **Check that the objective was the right one:** minimising duration, minimising peak
demand and minimising cost give three different schedules, and a tool asked for the wrong one
returns an excellent answer to a question nobody had. **Check what was allowed to move:** splitting,
overtime, calendar exceptions and crew substitution are assumptions with contractual and physical
consequences, and an optimiser silently permitted to split an activity has produced a plan the site
cannot run. And **compare against a stated baseline** — the priority-rule schedule — so that the
improvement is a number rather than a claim.

### Key terms — KA 6.3

| Term | Meaning |
|---|---|
| **Resource histogram** | Demand per period from the loaded schedule. |
| **Smoothing / levelling** | Flattening demand within float / accepting a later date under a hard cap. |
| **Constraint** | An externally imposed date; modelled as logic, not as a pin. |
| **Milestone** | Zero-duration event with an owner and a done-definition; its date is computed. |
| **Rolling wave** | Near-term detail, far-term packages, elaborated under change control. |
| **Planning package** | A far-wave summary activity with ranged duration awaiting elaboration. |
| **Resource-feasible schedule** | A schedule in which no period's demand exceeds any resource cap. |
| **Critical chain** | The binding sequence once logic *and* capacity are honoured; need not be the critical path. |
| **Priority rule** | The heuristic (commonly minimum total float first) used to break resource conflicts; beatable, and known to be. |
| **Commitment exposure** | Range width × cost of delay — what a date quoted from a ranged package risks. |

### Sample MCQs — KA 6.3

**MCQ 6.3-A `[6.3.1 · Application]`** A planner clears a resource peak by delaying an activity
with `TF` = 1, `FF` = 0 by one week. The immediate consequences are:
- A. no schedule effect of any kind
- B. project delay of one week
- C. the end date holds, the activity's path float is exhausted, and at least one successor's earliest start moves ✅
- D. the resource peak worsens

*Rationale:* One week inside `TF` protects the end date (not B), but zero `FF` moves a successor
and the path's buffer is now spent (not A) — the smoothing was legal but not free.

**MCQ 6.3-B `[6.3.3 · Analysis]`** A 30-month programme shows daily-level tasks throughout,
including month 30. The strongest inference is:
- A. the planning team is unusually diligent
- B. far-horizon detail is manufactured precision that will churn on every update, obscuring real variance ✅
- C. the schedule will need no re-baselining
- D. rolling-wave planning has been correctly applied

*Rationale:* Detail beyond the knowable horizon creates update churn and false confidence —
the opposite of diligence in effect (A) and the opposite of rolling wave (D); constant churn
makes re-baselining more likely, not less (C).

**MCQ 6.3-E `[6.3.1 · Application]`** D needs 3 crews in weeks 9–15 and concurrent staging needs
2; the site cap is 4. The excess demand the histogram must clear is:
- A. 1 crew for 7 weeks ✅
- B. 5 crews for 7 weeks
- C. 2 crews for 7 weeks
- D. nothing — 3 + 2 = 5 is within a 4-crew cap across two activities

*Rationale:* Peak demand 5 against cap 4 leaves one excess crew-week in each of the seven
overlap weeks — the precise quantity smoothing must relocate. B is total demand, not excess;
C is the staging demand itself; D misreads the cap as per-activity when it is per-site.

**MCQ 6.3-C `[6.3.1 · Application]`** A hard 3-crew cap forces either a 4-week project extension
(delay cost USD 45,000/week) or a second-shift waiver at USD 20,000/week for the same 4 weeks
(end date held). The value-maximising choice and its saving are:
- A. extend: saves USD 100,000
- B. second shift: saves USD 100,000 ✅
- C. second shift: saves USD 25,000
- D. they cost the same

*Rationale:* Extension costs `4 × 45,000 = 180,000`; the shift premium `4 × 20,000 = 80,000` —
choosing the shift saves USD 100,000 and keeps the date. A picks the dearer plan; C compares one
week only; D ignores the 65,000-per-week difference... which compounds four times.

**MCQ 6.3-D `[6.3.3 · Recall]`** A legitimate planning package in a rolling-wave schedule must
carry:
- A. daily-level task detail for its whole span
- B. a ranged duration consistent with its estimate class and a dated elaboration event under change control ✅
- C. zero float, to keep pressure on the team
- D. a pinned finish date agreed with the sponsor

*Rationale:* Far-wave honesty is ranged duration plus a governed elaboration point. A is the
manufactured precision rolling wave exists to avoid; C fabricates criticality; D is the typed
milestone anti-pattern (Case study B).

**MCQ 6.3-F `[6.3.1 · Analysis]`** Auriga's engineering pool is capped at 6. Activity G needs
2 engineers for 2 weeks and carries 8 weeks of total float, but every week from its earliest start to
the project end already commits 5 or 6 engineers. The correct conclusion is:
- A. G is irrelevant to the end date, because it has eight weeks of float
- B. G determines the resource-constrained duration despite its logical float, because float against logic is not float against capacity ✅
- C. the cap must be wrong, since average utilisation is only 73.33 %
- D. G should be deleted from the network and tracked separately

*Rationale:* Under the cap G has nowhere to go, so it joins the critical chain even though its total
float is 8 (6.3.1c). A applies a logic-network statement to a capacity problem; C reasons from an
average when feasibility is a property of the peak; D removes the activity that is about to make the
project late from the model that would show it.

**MCQ 6.3-G `[6.3.1 · Evaluation]`** A levelled Auriga schedule produced by the minimum-total-float
priority rule runs 27 weeks; a search finds a feasible 26-week schedule that deliberately defers the
critical activity E by one week. The professional reading is:
- A. the 26-week schedule is invalid, because critical activities must never be deferred
- B. the priority rule is a heuristic and here costs USD 45,000; a levelled schedule should state the rule used and whether better was sought ✅
- C. both schedules are equally good, since both respect the cap
- D. the rule is wrong and should never be used

*Rationale:* Deferring E by one week releases G two weeks earlier, so the "protect the critical path"
instinct loses a week worth 45,000 (6.3.1c). A elevates a heuristic to a constraint; C ignores a
one-week difference in duration; D over-corrects — the rule is a sound default whose output needs a
stated baseline, not abolition.

### Self-check — KA 6.3

1. *Why is smoothing "spending the risk buffer"?* — It consumes float, and float is the
   project's absorption capacity for the unknown (KA 6.2.2).
2. *What makes a milestone honest?* — Computed date, named owner, unambiguous done-definition,
   traceable logic.
3. *What must a planning package carry to be legitimate?* — A ranged duration consistent with its
   estimate class and a dated elaboration event under change control.
4. *Why can average utilisation never establish that a resource plan is feasible?* — Because
   feasibility is a property of the peak: Auriga breaches a 6-engineer cap while only 73.33 % loaded
   on average.
5. *What is the difference between the critical path and the critical chain?* — The critical path
   binds on logic alone; the critical chain binds once capacity is honoured too, and on Auriga it
   includes G, an activity with eight weeks of logical float.

---

## Knowledge Area 6.4 — Delivery flow: predictive, agile and hybrid; recovery and forecasting

*Topics: 6.4.1 flow across delivery modes · 6.4.2 schedule compression and recovery · 6.4.3
forecasting and scenario analysis.*

### 6.4.1 Flow across delivery modes

**Predictive** scheduling (this domain so far) plans the whole network and measures variance
against baseline. **Agile** delivery (Domain 13 in full) fixes cadence and capacity and lets
scope flow: the schedule questions become *throughput* (items per iteration), *cycle time* and
*forecast by velocity*. **Hybrid** programmes — the commonest reality — run engineered streams
predictively and product streams by cadence, joined at **integration milestones** that appear in
the CPM network as fixed-capacity deliveries. The leader's job is translation: a velocity
forecast ("the reporting module completes in 4–6 sprints") enters the network as a ranged
duration on the integration activity, and the network returns what the programme needs from the
agile stream — the latest acceptable delivery, read from `LS`. Neither mode is senior; the
network prices *time*, the cadence protects *focus*, and governance (Domain 3, KA 3.1.3) holds
both to evidence.

**Worked example 6.4.1 — the network's answer to an agile stream is a rate, not a date.**

1. **Setup.** Auriga's operator-interface configuration is delivered by a cadence team working from
   a **72-item** backlog. Over its last eight iterations the team's throughput has run between
   **3.6** and **4.5** items a week, averaging **4.0**. The package must be complete before testing
   and commissioning **F** begins, and F's backward pass gives `LS(F)` = **21** (KA 6.2.1). The
   stream cannot start until the design freeze at **week 5**. Domain 13 supplies the throughput
   forecasting method; this example does the translation.
2. **Formula.** Forecast duration = backlog ÷ throughput, computed across the observed throughput
   range to give a ranged duration. Available window = `LS` of the successor − stream start.
   **Required throughput = backlog ÷ available window.** Deliverable scope at a given throughput =
   throughput × window.
3. **Substitution.** Durations: `72/4.5 = 16.0`, `72/4.0 = 18.0`, `72/3.6 = 20.0` weeks. Window
   `= 21 − 5 = 16` weeks. Required throughput `= 72/16`. At the mean rate the window delivers
   `4.0 × 16 = 64` items.
4. **Result.** The team's forecast duration is **16–20 weeks**, mean **18**. The window is
   **16 weeks**, so the required throughput is **4.5 items a week** — the team's **best observed
   iteration, sustained without variation for sixteen weeks**. At its mean rate the stream delivers
   **64** of 72 items, leaving **8 items — 11.11 % of the package — undelivered**, and finishing the
   full package two weeks late costs `2 × 45,000 =` **USD 90,000**. Starting at week 3 instead widens
   the window to 18 weeks and drops the required rate to **4.0** (the mean, and therefore roughly even
   odds); starting at week 1 widens it to 20 and drops the requirement to **3.6** — the worst rate the
   team has recorded.
5. **Interpretation. The backward pass does not ask the team when it will finish; it tells the team
   what rate it must hold.** "Deliver by week 21" is unactionable inside a cadence system and invites the team to
   agree to something it cannot forecast. "Sustain 4.5 items a week from week 5" is a statement the
   team can test against its own record — and in this case reject, which is the whole value of
   putting it that way. The conversion runs in both directions: the team's ranged forecast enters the
   network as a ranged duration on the integration activity, and the network returns a required rate
   read off `LS`. A hybrid programme that cannot perform this conversion has two plans and no plan.

   **A requirement equal to the best-ever observation is a plan with no margin, and saying so is the
   professional act.** 4.5 is not impossible — it happened — but a rate achieved once is not a rate
   sustained for eight iterations, and the honest statement to the steering committee is that the
   current plan requires the team's historical maximum with zero allowance for variation. Domain 13,
   KA 13.2.4 sets out how to express such a forecast as a range with a stated meaning rather than as
   a single number; what belongs here is that the *network* is what turned an ambitious plan into a
   checkable claim.

   **Of the three levers in the arithmetic — the start date, the rate and the scope — only the rate
   belongs to the team.** Moving the start and deferring items are the leader's to pull, which is why
   "the team must go faster" is at once the weakest available response and the most frequently
   chosen. It is also the one lever Domain 13 shows cannot be pulled by starting more work in
   parallel.

   **The cautions.** Backlog items are being treated as interchangeable units of work, which is
   acceptable for forecasting a large package and wrong for a small one — 72 items averages out, 7
   does not. `LS(F)` = 21 is read off the *current* network, so a crash or a slip anywhere on the
   critical path moves the required rate without the team touching anything, and the required rate
   must therefore be re-derived every cycle rather than fixed at planning. And the 16-week window
   assumes the integration itself is instantaneous: where the package needs a hardening or acceptance
   period before F can use it, that period is an activity in the network with its own duration, not a
   rounding allowance.

### 6.4.2 Schedule compression and recovery

**The two levers.** **Crashing** buys duration with money (more resources, overtime, expediting);
**fast-tracking** buys it with risk (overlapping activities that logic preferred in sequence).
Both obey the same law: work the critical path, and re-run the passes after every move, because
**the critical path migrates**.

**Worked example 6.4.2 — the crash that stopped paying.**

1. **Setup.** Auriga's client offers a bonus of USD 45,000 (≈ SAR 168,750) per week saved.
   Expediting C costs USD 30,000 per week, up to two weeks. Should the leader buy one week, two,
   or none?
2. **Formula.** Crash only while (weeks actually saved × value per week) > crash cost — checked
   against the *re-run passes*, not the original network.
3. **Substitution.** Crash C by 1 (8→7): duration 25→24; net gain `45,000 − 30,000 = +15,000`.
   Crash C by 2 (8→6): path B–D–E–F (2+6+7+5+4) is now co-critical at 24 — duration stays 24;
   second week's net `0 − 30,000 = −30,000`.
4. **Result.** **Crash one week only**: +USD 15,000. The second week is pure loss.
5. **Interpretation.** Compression is subject to sharply diminishing returns because every crash
   promotes the next-longest path. The disciplined sequence: identify the binding path → price
   the cheapest week on it → re-run the passes → repeat until the next week costs more than it
   is worth. Leaders who mandate "crash procurement by two weeks" from the original network pay
   for weeks that no longer exist.

> **Fig 6.4.1 — The economics of crashing Auriga's procurement.** Step chart, x-axis "weeks of
> crash bought on C" (0, 1, 2), left y-axis USD. Two series: cumulative cost (0 → 30,000 →
> 60,000, grey steps) and cumulative value of weeks actually saved (0 → 45,000 → 45,000, brand
> blue — flat after week 1 because path B–D–E–F becomes co-critical at 24). Net-gain annotations:
> +15,000 at one week, −15,000 at two. Crimson marker at the co-criticality point labelled "path
> migration — second week buys nothing". Source: PCI original. Alt text: step chart showing
> crash cost rising linearly while saved-week value flattens after the first week, so the second
> crashed week loses money.

**What bounds the menu, before any of it is priced.** A cost slope says what a week *costs*. It does
not say whether the week is **available**, and the menu below would be professionally misleading if
read as though it did. Every compression lever in it — overtime, a second shift, night working, crew
addition, overlapping two disciplines on the same plant — carries a **non-financial ceiling** that
sits above its cost:

- **Working time, rest and fatigue.** Limits on hours, rest periods and consecutive shifts, and the
  duty to manage fatigue as a hazard rather than as a productivity variable.
- **The safety case and the permit regime.** What the plant, the site or the system is authorised to
  have done to it, and under what conditions — permits to work, isolations, competency requirements
  and the conditions attached to any approval the asset holds.
- **Agreement terms.** Collective and individual employment agreements, and subcontract terms, which
  may set shift patterns, notice, rates and maximum hours independently of anything the schedule
  wants.

These ceilings are **jurisdiction-, sector- and site-specific**, they differ substantially, and they
are taken from the safety function, from human resources and from qualified counsel — never from a
cost slope, and never from this book, which states no legal position and characterises no
arrangement as permitted or otherwise. The professional consequence is a hard one and it is the point
of the paragraph: **a lever whose ceiling binds is not available at any price**, and it is struck
from the menu before the optimisation is run rather than priced into it. A compression plan that
contains a lever nobody asked the safety function about is not an aggressive plan; it is an
unpriced one.

**Which makes compression a two-signature decision.** Where a compression move changes a shift
pattern, adds night working, or overlaps disciplines on the same plant, the decision requires the
**named safety approver** in addition to the authority spending the money — the two are different
people and the second cannot speak for the first. The same holds for a change that touches a
condition of any approval the asset or the system holds: the approval's owner decides whether the
move is available, and that decision sits outside the project's authority. The recovery plan
(Toolkit 6.T.2) records **both** authorities, and a recovery plan naming only the funder has recorded
half the decision. Worked example 6.4.2c's overlap of installation and commissioning is exactly this
case: it is a simultaneous-operations question before it is an arithmetic one, and in many sectors it
requires a separate review and approval before the overlap can be bought at all.

**Worked example 6.4.2b — the whole compression menu, and the least-cost duration.**

1. **Setup.** The example above priced **one** lever. A leader planning a compression programme
   prices the **menu**. Auriga's available buys, each quoted by the owning discipline as a cost per
   week with a technical limit:

   | Activity | On paths | Cost slope (USD/wk) | Max weeks |
   |---|---|---|---|
   | C Procure hardware | A–B–C–E–F | 30,000 | 2 |
   | B Detailed design | all three | 40,000 | 2 |
   | D Civil and cabling | A–B–D–E–F, A–B–D–G | 35,000 | 1 |
   | E Installation | both long paths | 55,000 | 1 |
   | F Testing and commissioning | both long paths | 65,000 | 1 |

   A week is worth **USD 45,000**. What is the least-cost project duration?
2. **Formula.** For each target duration, find the **cheapest set of crashes that shortens every
   path** to that duration — not the cheapest activity, which is the standard error. Then net saving
   = weeks saved × 45,000 − cumulative crash cost, and the optimum is the duration maximising it.
   Equivalently: buy the next week while its **marginal** cost is below 45,000, and stop at the first
   week where it is not.
3. **Substitution.** 25 → 24 needs only A–B–C–E–F shortened: crash C ×1, **30,000**. 24 → 23 needs
   *both* long paths shortened (they are co-critical at 24), so the buy must be on a shared activity:
   B at **40,000** beats C ×1 + D ×1 at `30,000 + 35,000 = 65,000`. 23 → 22: B's second week,
   **40,000**. 22 → 21: B is exhausted, so E at **55,000** (again beating C + D at 65,000).
   21 → 20: F at **65,000**, tying with C + D. 20 → 19: C ×1 + D ×1, **65,000**, exhausting the menu.
4. **Result.**

   | Duration (wk) | Cheapest plan | Marginal cost | Cumulative crash cost | Weeks saved × 45,000 | **Net saving** |
   |---|---|---|---|---|---|
   | 25 | — | — | 0 | 0 | 0 |
   | 24 | C ×1 | 30,000 | 30,000 | 45,000 | +15,000 |
   | 23 | + B ×1 | 40,000 | 70,000 | 90,000 | +20,000 |
   | **22** | **+ B ×2** | **40,000** | **110,000** | **135,000** | **+25,000** |
   | 21 | + E ×1 | 55,000 | 165,000 | 180,000 | +15,000 |
   | 20 | + F ×1 | 65,000 | 230,000 | 225,000 | (5,000) |
   | 19 | + C ×2, D ×1 | 65,000 | 295,000 | 270,000 | (25,000) |

   The least-cost duration is **22 weeks**, three weeks compressed, for **USD 110,000** of crash spend
   and a net **USD 25,000** better than the 25-week plan. The shortest *technically* feasible duration
   is **19 weeks**, which destroys **USD 25,000**.
5. **Interpretation. The optimum is interior, and it is set by the marginal week.** The cost slopes rise —
   30,000, 40,000, 40,000, 55,000, 65,000, 65,000 — because each crash promotes the next-longest path
   and forces the buy onto a shared, dearer activity. Compression is therefore an optimisation with an
   interior solution, and the objective is not "as fast as possible" but "as fast as pays". The same
   shape appears in Domain 9's cost-of-quality minimum for the same underlying reason: the last
   increments of any good are the expensive ones.

   **Averages mislead here and marginal figures do not.** At 22 weeks the *average* crash cost is
   `110,000/3 =` **USD 36,666.67** a week, comfortably below the 45,000 a week is worth — and a
   manager reasoning from that average will buy the fourth week too, at a marginal 55,000, and lose
   **USD 10,000**. The rule is always marginal, never average, and the two diverge precisely because
   the slope rises.

   **The range of optimality is what makes the plan defensible.** Since only the marginal cost matters,
   the optimal duration as a function of the value of a week `v` is a step function:

   | Value of a week `v` | Least-cost duration |
   |---|---|
   | below 30,000 | 25 weeks — compress nothing |
   | 30,000–40,000 | 24 weeks |
   | **40,000–55,000** | **22 weeks** |
   | 55,000–65,000 | 21 weeks |
   | above 65,000 | 19 weeks |

   Auriga's 45,000 sits **12.50 %** above the lower bound of its band and **18.18 %** below the upper,
   so the three-week plan survives a substantial error in the cost of delay — which is the sentence to
   put in the steering paper. Note also what the table says about *other* projects: the identical
   network with a week worth 25,000 should be compressed not at all, and with a week worth 80,000
   should be compressed to its technical limit. **Compression policy is not a property of a schedule;
   it is a property of a schedule and a cost of delay together.** Where the project also carries a
   time-related indirect cost — site establishment, supervision, financing — that cost is *added* to
   the value of a week: an extra USD 18,000 a week takes `v` to 63,000 and moves the optimum from 22
   weeks to **21**.

   **The optimum is a property of the option set, not of the network, and option sets decay.**
   KA 6.4.2 concluded "crash one week only" and was right, because only C's expediting was on offer.
   With the full menu the answer is three weeks. The same effect runs the other way as delivery
   proceeds: 6.A.4 recovers two weeks late in the project, when B is complete and only C and E remain
   buyable, for **USD 85,000** — where the same two weeks bought from this menu at planning time cost
   **USD 70,000**. The **USD 15,000** difference is the price of the options that expired, and it is
   the quantitative case for pricing the compression menu at baseline rather than in the crisis. A
   compression menu is a perishable asset and should be reviewed on the same cycle as the risk
   register.

   **The cautions.** Cost slopes are treated as linear within each activity's limit, which is a
   simplification: the second crashed week of an activity is often dearer than the first, and where a
   discipline can say so the table should carry two slopes rather than one. The limits are technical
   claims and belong to the disciplines that made them — a "maximum two weeks" that is really a
   preference will hide a cheap week. Every crash is a *risk* decision as well as a cost one: more
   people on the same work raises coordination load (Domain 12) and error rates (Domain 9), neither of
   which appears in a cost slope. And the arithmetic assumes the crashes are independent; two crashes
   competing for the same scarce specialist are not, which is a resource-feasibility question
   (KA 6.3.1c) to be re-run after the plan is chosen, not before.

> **Fig 6.4.2 — The compression menu and the least-cost duration.** Line chart, x-axis project
> duration falling left to right (25, 24, 23, 22, 21, 20, 19) with the cumulative crash cost printed
> beneath each point (0 · 30k · 70k · 110k · 165k · 230k · 295k); y-axis "net saving vs the 25-week
> plan (USD)" from −30,000 to +30,000. A brand-blue line rises to a peak at 22 weeks and falls away;
> the peak is ringed and annotated **"least-cost: 22 wk, +25,000"** with a dashed drop line to zero.
> Above the plot, the marginal cost of each bought week is labelled between the points — 30k, 40k,
> 40k in brand blue (at or below the 45,000 a week is worth) and 55k, 65k, 65k in crimson (above it).
> A side note records "a week is worth USD 45,000 — buy while the step is below it" and "optimal at
> 22 wk for any week value 40,000–55,000". Source: PCI original. Alt text: an inverted-U curve of net
> saving against project duration peaking at twenty-two weeks, with the marginal cost of each
> compressed week labelled above and colour-coded by whether it is below or above the value of a week.

**Worked example 6.4.2c — fast-tracking priced as the probabilistic decision it is.**

1. **Setup.** The alternative to buying weeks with money is buying them with overlap. Auriga can begin
   testing and commissioning **F** when installation **E** is 60 % complete — commissioning the
   completed sections while the last are installed — an overlap of **2 weeks** (40 % of E's five),
   taking the project from 25 weeks to **23**. The risk is specific: if the last-installed sections
   change the commissioning basis, the affected sections must be re-commissioned, costing
   **USD 140,000** and **1.5 weeks**. The commissioning lead assesses the probability at **25 %**. A
   week is worth **USD 45,000**. The crash alternative from the menu above buys the same two weeks for
   a certain **USD 70,000**.
2. **Formula.** Value each outcome against the do-nothing baseline: good outcome = weeks saved ×
   cost of delay; bad outcome = weeks saved after rework × cost of delay − rework cost. Then
   `EV = good − p × (good − bad)`, and the breakeven probability is `p* = good ÷ (good − bad)`.
   Against a certain alternative worth `K`, the indifference probability is
   `p** = (good − K) ÷ (good − bad)`.
3. **Substitution.** Good: `2 × 45,000 = 90,000`. Bad: duration `23 + 1.5 = 24.5` weeks, so
   `0.5 × 45,000 − 140,000 = −117,500`. Swing `90,000 − (−117,500) = 207,500`.
   `p* = 90,000/207,500`; `p** = (90,000 − 20,000)/207,500` where the crash's net is
   `90,000 − 70,000 = 20,000`. `EV` at `p` = 0.25: `90,000 − 0.25 × 207,500`.
4. **Result.** The fast-track is worth **+USD 90,000** if the overlap holds and **−USD 117,500** if it
   does not — a swing of **USD 207,500**. It beats doing nothing while `p` < **43.37 %** and beats
   crashing while `p` < **33.73 %**. At the assessed 25 % its expected value is **+USD 38,125**,
   which is **USD 18,125** better than the crash plan's certain **+USD 20,000**.
5. **Interpretation. Expected value ranks the two options; variance is why a leader may still choose
   the other one.** The fast-track wins on expectation by 18,125 and carries a **USD 137,500** worse outcome in the
   quarter of futures where the overlap fails — the gap between a certain +20,000 and a possible
   −117,500. Nothing in the arithmetic settles which to take: that is a risk-appetite decision
   belonging to whoever carries the consequence, and Domain 8, KA 8.4.1 sets out when an
   expected-value comparison is the wrong test altogether. What the arithmetic does settle is that the
   decision must be *made* rather than drifted into, and by someone with the authority to accept the
   downside.

   **Two thresholds, not one, and their ratio is the useful part.** `p*` = 43.37 % answers "is
   overlapping better than accepting the date?"; `p**` = 33.73 % answers "is it better than paying
   cash?". Their ratio is `70,000/90,000 =` **0.7778** — the crash option removes 22.22 % of the
   probability headroom the fast-track had against doing nothing. Whenever a certain alternative
   exists, the relevant threshold is the lower one, and a leader who computes only `p*` will overlap
   in cases where writing a cheque was better.

   **Both remedies can be mixed, and the mixture is usually right.** One week of overlap plus one
   crashed week costs 30,000 with a smaller exposure than two weeks of overlap; the reason to prefer a
   hedge is that `p` is a **function of the overlap taken**, rising as the overlap deepens, and almost
   no team can state that function. Where `p(w)` is unknown — the usual case — the defensible practice
   is to take the smallest overlap that achieves the needed weeks, buy the remainder, and state both
   thresholds for the overlap actually taken.

   **The cautions.** The outcome is modelled as binary when reality is a distribution of partial
   rework, so the two thresholds are guides rather than boundaries; the honest reading of `p*` = 43.37 %
   is "this has room to be wrong", not "43.37 % is the answer". The 25 % is an expert judgement, and it
   should be recorded in Domain 8's register with its owner and its basis rather than embedded in a
   schedule. Overlap also consumes coordination and supervision that the cost slope of a crash does not
   — two disciplines working the same plant at once — so the fast-track is cheaper on paper than in the
   field. And note that even the bad outcome finishes at **24.5 weeks**, still inside the original date:
   fast-tracking that fails is usually not a schedule disaster but a cost one, which is exactly why the
   decision belongs to whoever owns the cost.

**Recovery.** When the network slips (self-check 6.2.3: D at 10 weeks → 27), recovery options
rank by cost-of-time: re-sequencing and logic re-choice (cheapest — KA 6.1's chosen logic);
float harvesting from non-critical paths (G's 8 weeks fund nothing on the new critical path, but
its resources might); crashing the *new* critical path (now through D); fast-tracking E against
D's tail with an explicit rework risk (Domain 8's risk register prices it); and scope or
acceptance re-negotiation (Domain 5) as the honest last resort. A **recovery plan** states the
target date, the moves, their costs and risks, each move's **non-financial ceiling**, and the
decision authorities: the one spending the money and, where a move changes shift patterns, adds
night working, overlaps disciplines on the same plant or touches a condition of an approval, the
**named safety or approval-holding authority** whose agreement the money cannot substitute for.
The template is toolkit 6.T.2.

### 6.4.3 Forecasting and scenario analysis

**Three-point estimates.** Uncertain durations are ranges, not points. The PERT expected
duration and spread for an activity estimated optimistic `o`, most-likely `m`, pessimistic `p`:

```
tₑ = (o + 4m + p) / 6          σ = (p − o) / 6
```

**Worked example 6.4.3 — Auriga's installation risk.**

1. **Setup.** Installation E is estimated `o` = 4, `m` = 5, `p` = 12 weeks (the long tail: a
   legacy-system integration problem discovered late).
2. **Formula.** `tₑ = (o + 4m + p)/6`; `σ = (p − o)/6`.
3. **Substitution.** `tₑ = (4 + 20 + 12)/6 = 36/6`; `σ = (12 − 4)/6 = 8/6`.
4. **Result.** `tₑ` = **6.0 weeks** (not 5); `σ` = **1.33 weeks**.
5. **Interpretation.** The deterministic network used 5 weeks; the risk-weighted expectation is
   6 — the skewed tail alone adds a week to the honest forecast, pushing expected completion
   toward 26 weeks before any risk event "happens". Deterministic dates are the *mode*, not the
   *mean*; boards deserve the mean and the spread. Full quantitative schedule risk analysis —
   correlating activities, simulating paths, criticality indices — is Domain 8 (KA 8.2), built
   on exactly this input.

   **The bias has a closed form, and it is worth memorising.** Subtracting `m` from `tₑ` gives

   ```
   tₑ − m = (o − 2m + p) / 6
   ```

   — the deterministic bias is one sixth of the estimate's **skew**, `(p − m) − (m − o)`. On E:
   `(4 − 10 + 12)/6 =` **1.0000 week**. On the o = 6, m = 8, p = 16 estimate of Exercise 6.4:
   `(6 − 16 + 16)/6 =` **1.0000 week** again, despite quite different numbers. Three consequences.
   A **symmetric** estimate has zero bias, so the deterministic plan is unbiased and PERT adds
   nothing — which is why applying it indiscriminately wastes effort. The bias depends only on how
   *lop-sided* the range is, not on how *wide* it is: a wide symmetric estimate (5, 9, 13) has a
   large σ of 1.3333 and no bias at all, so σ and bias answer different questions and neither
   substitutes for the other. And the bias is **positive whenever the tail is on the pessimistic
   side**, which is nearly always in delivery, because durations are bounded below by physics and
   unbounded above by circumstance. Right-skew is the normal condition, so the deterministic plan is
   normally optimistic — not because planners are optimists but because the mode of a right-skewed
   distribution is below its mean.

   **What this does and does not license.** It does not license adding a week to every activity: doing
   that on all seven of Auriga's activities would inflate the plan by far more than the network's
   expected completion moves, because only the activities on the binding path contribute, and because
   path-level aggregation is not the sum of activity-level expectations. That aggregation — variances
   adding, merge bias at convergence points, criticality indices — is Domain 8, KA 8.A.2, worked on
   this same network, and it finds that E's start is only **13.16 %** likely in week 16. Nor does it
   license treating `σ = (p − o)/6` as a derived quantity: it is a convention that assumes the range
   spans roughly six standard deviations, it understates spread on strongly skewed estimates, and
   probabilities computed from it should be read to the nearest percentage point at best.

   **The reviewer's question is where the three points came from.** `o`, `m` and `p` elicited from the
   same person in the same breath are usually one number with decorations: the range is anchored on
   the most likely value and adjusted by a habitual percentage, which produces symmetric estimates and
   therefore zero bias by construction. The tail that matters — the legacy-integration problem in E's
   `p` = 12 — comes from asking a different question ("what would have to go wrong, and how long would
   *that* take?"), which is a risk-identification act (Domain 8, KA 8.1) rather than an estimating
   one. An estimate whose `p` cannot be traced to a named scenario is not a three-point estimate.

**Scenario analysis.** Between the single date and the full simulation sits the leader's
workhorse: three coherent scenarios (base / threat / opportunity), each a *complete re-run of the
passes* under stated assumptions, each with owners for the assumptions that differ. Scenarios are
cheap, auditable, and — because each is a real network — they expose path migration that a
percentage-confidence number hides.

**The earned-schedule bridge.** Once cost joins schedule (Domain 7), progress data yields a
time-based forecast: **earned schedule `ES`** asks *when the value now earned was planned to
have been earned*, and `SPI(t) = ES / AT` (actual time) measures schedule efficiency in time
units. If Auriga's week-22 status shows work that the baseline expected by week 20,
`SPI(t) = 20/22 = 0.91` — the programme is running at 91 % of planned tempo, a signal the
currency-based `SPI` famously loses late in a project. On Auriga's own week-13 position the value
earned was planned to have been earned by week 12, so `ES` = **12.0000** and
`SPI(t) = 12/13 =` **0.9231** — identical to the currency-based `SPI` of `1,920,000/2,080,000`,
because the planned-value curve is locally linear there. Domain 7 (KA 7.3 and 7.A.1) builds the full
machinery, including the point at which the two measures separate; it is flagged here because
schedule forecasting belongs to whoever owns the network, not only to the cost engineer.

### AI in this KA

Machine forecasting earns trust in exactly one way: calibration. A model that ingests progress
data and produces completion forecasts must show its record — forecast vs actual across past
periods — before its numbers enter a board pack (Domain 14, KA 14.3's verification workflow).
The leader's questions are the auditor's: what data trained it, what does it do when the data
thins, who re-ran its arithmetic, and what would make it wrong? Fluent confidence without a
calibration record is the machine version of the site manager who is "sure it'll be fine".

### Key terms — KA 6.4

| Term | Meaning |
|---|---|
| **Crashing / fast-tracking** | Buying duration with money / with overlap risk. |
| **Path migration** | The critical path moving as durations change; why passes are re-run. |
| **Recovery plan** | Target date, ranked moves, costs, risks, non-financial ceilings, and **both** decision authorities — the one spending the money and the safety or approval-holding authority where one is engaged. |
| **Non-financial ceiling** | The working-time, rest, fatigue, safety-case, permit or agreement limit bounding a compression lever. Supplied by the safety function, HR or counsel, jurisdiction- and sector-specific, and where it binds the lever is unavailable at any price. |
| **Simultaneous operations** | Two disciplines working the same plant at once; a safety decision requiring its own review and approval before it is an arithmetic one. |
| **Three-point / PERT estimate** | `tₑ = (o+4m+p)/6`, `σ = (p−o)/6`. |
| **Scenario analysis** | Complete network re-runs under coherent stated assumptions. |
| **Integration milestone** | The CPM node where an agile stream's delivery enters the network. |
| **Cost slope** | An activity's crash cost per week, with a technical limit on weeks available. |
| **Least-cost duration** | The duration maximising net saving; found where the marginal week's cost first exceeds the value of a week. |
| **Required throughput** | Backlog ÷ (successor's `LS` − stream start): what the network asks a cadence team to sustain. |
| **Breakeven rework probability** | `p* = good outcome ÷ (good − bad)`; the probability at which an overlap stops paying. |
| **Deterministic bias** | `tₑ − m = (o − 2m + p)/6` — one sixth of the estimate's skew. |

### Sample MCQs — KA 6.4

**MCQ 6.4-A `[6.4.2 · Application]`** Saving a week is worth USD 45,000; crashing the critical
activity costs USD 30,000 per week for up to two weeks; after one week of crashing, a parallel
path becomes co-critical. The value-maximising decision is:
- A. crash two weeks (net +USD 30,000)
- B. crash one week (net +USD 15,000) ✅
- C. crash nothing (avoid all cost)
- D. crash two weeks and fast-track the parallel path at no cost

*Rationale:* Week one converts to a project week: +15,000. Week two is absorbed by the
co-critical path: −30,000. A prices weeks that don't materialise; C leaves +15,000 unclaimed;
D invents a free fast-track — overlap always carries rework risk.

**MCQ 6.4-B `[6.4.3 · Application]`** An activity is estimated o = 4, m = 5, p = 12 weeks. Its
PERT expected duration is:
- A. 5.0 weeks
- B. 7.0 weeks
- C. 6.0 weeks ✅
- D. 6.5 weeks

*Rationale:* `(4 + 4×5 + 12)/6 = 36/6 = 6.0`. A is the most-likely value (the mode); B is the
unweighted mean of the three points; D miscounts the weighting.

**MCQ 6.4-C `[6.4.1 · Analysis]`** A hybrid programme needs a software module from an agile
team for an integration milestone. The schedule-sound way to join the two worlds is:
- A. impose the CPM date on the team as a sprint deadline
- B. enter the team's velocity-based forecast as a ranged duration and read the latest acceptable delivery from the backward pass ✅
- C. exclude the module from the network since agile work cannot be scheduled
- D. convert the agile team to predictive planning for the integration period

*Rationale:* B translates in both directions — evidence-based forecast in, latest-start
requirement out. A dictates without evidence; C leaves the network blind at a convergence point;
D destroys the team's delivery system to decorate the network.

**MCQ 6.4-F `[6.4.3 · Application]`** An activity is estimated optimistic 3, most-likely 4,
pessimistic 8 weeks. Its PERT expected duration and standard deviation are:
- A. tₑ = 4.5, σ = 0.83 ✅
- B. tₑ = 4.0, σ = 0.83
- C. tₑ = 5.0, σ = 1.67
- D. tₑ = 4.5, σ = 5.0

*Rationale:* `tₑ = (3 + 16 + 8)/6 = 4.5`; `σ = (8 − 3)/6 = 0.83`. B reports the mode as the
mean; C is the unweighted three-point average with a doubled spread; D confuses σ with the
pessimistic-minus-optimistic range.

**MCQ 6.4-D `[6.4.3 · Application]`** At week 22, a programme has earned the value its baseline
planned to earn by week 20. Its time-based schedule performance index `SPI(t)` is:
- A. 1.10
- B. 0.91 ✅
- C. 0.80
- D. 20.0

*Rationale:* `SPI(t) = ES/AT = 20/22 = 0.91` — the programme delivers at 91 % of planned tempo.
A inverts the ratio; C subtracts the two weeks from the wrong base; D reports earned schedule
itself, not the index.

**MCQ 6.4-E `[6.4.3 · Analysis]`** Compared with quoting "78 % confidence of the date", giving
the board three fully re-run schedule scenarios is stronger because:
- A. three numbers always beat one
- B. each scenario is an auditable network with named assumptions, showing how the date moves and where the path migrates ✅
- C. percentages are always statistically invalid
- D. scenarios eliminate the need for risk analysis

*Rationale:* The scenario's power is auditability and mechanism — assumptions with owners,
visible path migration. A is numerology; C overclaims (a calibrated percentage is legitimate,
Domain 8); D reverses the relationship — scenarios are inputs to risk analysis, not substitutes.

**MCQ 6.4-G `[6.4.2 · Evaluation]`** A compression menu offers successive weeks at marginal costs of
30,000, 40,000, 40,000, 55,000, 65,000 and 65,000. A week is worth 45,000. The least-cost plan buys:
- A. one week, because only the first step is clearly cheap
- B. three weeks, stopping at the first marginal cost above 45,000 ✅
- C. six weeks, because the average cost of 49,167 is close to 45,000
- D. four weeks, because the average cost of the first four (41,250) is below 45,000

*Rationale:* Buy while the marginal step is at or below 45,000 — 30,000, 40,000, 40,000 — and stop at
55,000, giving 22 weeks and a net +25,000 (6.4.2b). C and D both reason from averages, the standard
error: the fourth week costs 55,000 whatever the average of the first four is, and buying it loses
10,000. A stops at the single-lever answer of 6.4.2, which was correct only for a menu of one.

**MCQ 6.4-H `[6.4.2 · Application]`** An overlap saves 2 weeks worth 45,000 each; if it fails it
costs 140,000 and gives back 1.5 weeks, and a certain crash plan would buy the same 2 weeks for
70,000. The probability of failure above which the overlap is worse than crashing is:
- A. 43.37 %
- B. 33.73 % ✅
- C. 50.00 %
- D. 25.00 %

*Rationale:* The swing is `90,000 − (−117,500) = 207,500` and the crash's net is 20,000, so
`p** = 70,000/207,500 = 33.73 %` (6.4.2c). A is the threshold against *doing nothing*, which is the
wrong comparator once a certain alternative exists; C treats the choice as a symmetric coin toss —
the answer if the two outcomes were equal in size, which they are not; D is the assessed probability,
the input rather than the threshold.

**MCQ 6.4-I `[6.4.1 · Application]`** A cadence team must clear a 72-item package between week 5 and
its successor's `LS` of week 21. Its observed throughput has ranged 3.6–4.5 items a week, averaging
4.0. The network's requirement is:
- A. delivery by week 21, which the team should commit to
- B. 4.5 items a week — the team's best observed rate, sustained for the full 16 weeks ✅
- C. 4.0 items a week, since the mean is the fair planning figure
- D. 3.6 items a week, since planning should use the worst observation

*Rationale:* `72 ÷ (21 − 5) = 4.5` items a week (6.4.1). C and D quote the team's own forecast rates
rather than deriving the requirement from the window — the mean fits an 18-week window and the worst
rate a 20-week one, neither of which exists here. A restates the date instead of translating it, which
is what leaves the requirement untested.

### Self-check — KA 6.4

1. *Why does compression show diminishing returns?* — Each crash promotes the next-longest path;
   saved weeks stop converting to project weeks (path migration).
2. *What makes a scenario more useful to a board than a confidence percentage?* — It is a
   complete, auditable network with named assumptions — it shows *how* the date moves, not just
   that it might.
3. *When may a machine forecast enter a board pack?* — With a calibration record, verified
   arithmetic, stated data lineage and a named human owner.
4. *Why is the average crash cost the wrong figure?* — Because the decision is marginal: Auriga's
   average at 22 weeks is 36,666.67 against a week worth 45,000, yet the next week costs 55,000 and
   buying it loses 10,000.
5. *What does the network give a cadence team, and in what units?* — A required throughput —
   backlog ÷ (successor's `LS` − start) — which on Auriga's interface package is 4.5 items a week, the
   team's best observed rate.
6. *When does a three-point estimate add nothing?* — When it is symmetric: the bias is
   `(o − 2m + p)/6`, which is zero, so `tₑ = m` and only the spread is new information.
7. *What does a cost slope not tell you about a compression lever?* — Whether the week is available.
   Working-time, rest and fatigue limits, safety-case and permit conditions and agreement terms each
   set a ceiling above the price; where a ceiling binds, the lever is struck from the menu rather
   than priced into it, and the ceilings come from the safety function, HR and counsel (6.4.2).
8. *Who signs a compression that changes a shift pattern or overlaps disciplines on live plant?* —
   The authority spending the money **and** the named safety or approval-holding authority. The
   recovery plan records both; money cannot substitute for the second signature.

---

## Advanced topics — Domain 6

### 6.A.1 The drum: constraint-aware flow

Where one resource is the system's constraint (a single commissioning team, one test rig),
throughput scheduling subordinates everything to that drum: buffer it, never starve it, and let
non-constraint efficiency go. This reframes utilisation worship — a 100 %-busy non-constraint
builds inventory, not progress. In schedule terms: protect the constraint's feed path with
deliberate float placement rather than spreading buffer evenly.

### 6.A.2 Schedule baselines and the update cycle

The control schedule lives on a cycle: status (actuals and remaining durations in), re-run
passes, variance vs baseline, forecast, and — rarely, formally — re-baseline under Domain 4's
change control. Two integrity rules carried from the family's controls tradition: never edit
history (actualised dates are records), and never re-baseline to hide variance — a re-baseline
is a governance decision with an audit trail, not a cosmetic one.

### 6.A.3 PDM edge cases: float under SS and FF links

Precedence-diagram (PDM) links change how float behaves, and two edge cases bite reviewers.
**(1) The SS-linked short successor.** P (10 weeks) `SS+2` Q (3 weeks): Q can run weeks 2–5,
finishing seven weeks before its predecessor — legal, and correct if the dependency truly binds
only the starts; but if Q *also* needs P's output to finish, the network is missing an FF link,
and Q's float is fiction. Paired `SS+FF` links express "starts together, finishes together"
honestly. **(2) Duration-driven float.** Under an FF link the successor's float depends on its
own duration: lengthen Q and its `LS` moves *earlier* — planners who "add duration for safety"
on FF-linked activities silently destroy their float. The audit habit: on every SS/FF link, ask
which end of each activity the logic genuinely binds, and whether the unbound end needs its own
link. A network is only as honest as its least-considered link type.

### 6.A.4 Negative float and the honest schedule

A constraint-bound network can compute `TF < 0`: the plan, as constrained, is late already.
Negative float is information — the size of the recovery problem — and suppressing it (deleting
constraints, shortening durations by fiat) is the scheduling equivalent of cooking the books.
The professional response mirrors KA 6.4.2: quantify it, trace the binding path, price the
recovery options, and escalate the decision to whoever owns the trade (Domain 3's escalation
design).

**Worked example 6.A.4 — buying back two weeks of negative float.**

1. **Setup.** A newly imposed outage constraint requires Auriga to finish by **week 23**; logic
   says 25 — so `TF` = −2 on A–B–C–E–F. Available buys: crash C at USD 30,000/week (max 2);
   crash E at USD 55,000/week (max 1). Find the cheapest feasible recovery to week 23.
2. **Formula.** Recover week by week on the *currently binding* path(s), re-running the passes
   after each buy (path migration, 6.4.2).
3. **Substitution.** Week one: crash C by 1 → duration 24, but B–D–E–F is now co-critical at 24.
   Week two must shorten *both* paths: the only shared activity available is E → crash E by 1
   (USD 55,000) → duration 23. (C's second week alone would leave B–D–E–F at 24 — spent money,
   no schedule.)
4. **Result.** Feasible recovery: **crash C ×1 + crash E ×1 = USD 85,000**, finishing week 23.
5. **Interpretation.** Negative float is bought back on the binding path *as it migrates* — the
   second-cheapest week on the original critical path (C's second week, USD 30,000) is worthless,
   while the dearest activity (E, on both paths) is the only week-two purchase that works. This
   is why recovery plans list options *in re-run order*, not in unit-cost order (toolkit 6.T.2).

   Note also what the restricted option set costs. Only C and E are buyable here because design B is
   complete and commissioning F cannot be accelerated at this stage; the same two weeks bought from the
   full planning-time menu of 6.4.2b — where B was still open at a 40,000 slope — cost **USD 70,000**
   against this **USD 85,000**. The **USD 15,000** gap is the price of options that expired, and it is
   the argument for pricing the compression menu at baseline and reviewing it on the risk register's
   cycle rather than assembling it in the crisis.

### 6.A.5 Repetitive delivery: takt, crews and the benefit integral

A rollout is not a network. Meridian installs the same clinic forty times, and a forty-times-repeated
activity-on-node diagram teaches nothing: the schedule's content is a **rate**. **Takt** is the
interval between successive completions — window ÷ units — and it is the quantity that converts a
programme date into a crew count, because the rate a crew can sustain is a productivity figure the
delivery team already owns. The arithmetic is three divisions, and the reason to do it is that the
*value* of a rate behaves quite differently from the value of a date.

**Worked example 6.A.5 — Meridian's rollout rate, and what accelerating it is worth.**

1. **Setup.** Meridian's **40** clinics are installed by a crew of **6** deployment specialists, each
   completing **0.1** clinics a week (Domain 1, KA 1.3.3b), so **0.6** a week in total. The estate and
   training window requires all 40 live within **50 weeks** of rollout start. Specialists cost
   **USD 4,200** a week; per-clinic work content is **10** specialist-weeks. Adding specialists incurs
   Domain 1's ramp: **4 weeks** at **25 %** productivity, each newcomer absorbing **50 %** of an
   existing specialist's time. Benefit accrues at `6 hours × USD 85 =` USD 510 a week per adopting
   clinic and adoption runs at **70 %**, so **USD 357** a week per installed clinic. What crew does the
   window need, and does it pay?
2. **Formula.** Required takt = window ÷ units; required rate = units ÷ window; crew = required rate ÷
   per-head rate. With reinforcement: ramp rate = base − newcomers × supervision × per-head + newcomers ×
   ramp productivity × per-head; post-ramp rate = base + newcomers × per-head; duration = ramp +
   (units − ramp × ramp rate) ÷ post-ramp rate. For a linear rollout over a fixed horizon, the
   **unit-weeks** gained by finishing `ΔT` sooner = `(units ÷ 2) × ΔT`.
3. **Substitution.** Required takt `50/40 = 1.25` weeks; required rate `40/50 = 0.8` a week; crew
   `0.8/0.1 = 8`. With 2 newcomers: ramp rate `0.6 − 2 × 0.5 × 0.1 + 2 × 0.25 × 0.1 = 0.55`; post-ramp
   `0.6 + 0.2 = 0.8`; duration `4 + (40 − 4 × 0.55)/0.8`. Baseline `40/0.6`. Clinic-weeks gained
   `20 × ΔT`.
4. **Result.** The 50-week window needs a takt of **1.25 weeks** and therefore **8** specialists. With
   the ramp, the eight-person crew finishes in **51.2500 weeks** — an actual takt of **1.2813 weeks** —
   against **66.6667 weeks** for the crew of six, a saving of **15.4167 weeks**. Effort is nearly
   invariant: `6 × 66.6667 =` **400.0** specialist-weeks against `8 × 51.25 =` **410.0**, so the
   reinforcement costs **10** extra specialist-weeks, or **USD 42,000**. The acceleration yields
   `20 × 15.4167 =` **308.3333** clinic-weeks at USD 357 — **USD 110,075** — for a net **+USD 68,075**.
5. **Interpretation. Effort is invariant to the rate; only the benefit integral moves.** Forty clinics at 10
   specialist-weeks each is **400 specialist-weeks** whether delivered by six people over 67 weeks or
   eight over 50. That is the structural reason repetitive-work acceleration is so often nearly free and
   so rarely attempted: the labour bill is set by the *work content*, not by the *rate*, and the only
   incremental cost is the transient of changing the rate — here 10 specialist-weeks of ramp. On a
   network, buying weeks costs a cost slope every time; on a rollout it costs a one-off. The two are
   different economies and must not be priced with the same instinct.

   **Accelerating a linear rollout is worth exactly half the fully-adopted cost of delay per week
   saved.** The clinic-weeks gained are `(N/2) × ΔT`, so the value per week saved is
   `(40/2) × 357 =` **USD 7,140** — precisely half of Meridian's **USD 14,280**, and the identity holds
   for any linear ramp because the average number of live units during the ramp is half the final
   number. This matters because the temptation is to value rollout acceleration at the steady-state cost
   of delay, which **doubles** it. The reconciling check is worth noting: `40 × 357 =` **USD 14,280**
   exactly, Domain 1's figure, because 70 % of 40 clinics is the 28 adopting clinics it was derived
   from.

   **Where benefit only switches on at completion the full figure applies — and here the decision is
   robust to which regime holds.** If the shared records system delivers nothing until coverage is
   complete, the 15.4167 weeks are worth `15.4167 × 14,280 =` **USD 220,150**, exactly twice the
   ramp-integral figure, and the net becomes **+USD 178,150**. Both readings say the same thing: buy the
   two specialists. That is the professionally important observation — a factor-of-two ambiguity in the
   valuation basis need not be resolved when the decision is unchanged either way, and the leader who
   notices this stops arguing about the benefits model and gets on with the rollout. Where the decision
   *is* sensitive to the factor of two, it is a benefits-realisation question (Domain 16) and must be
   settled by whoever owns the benefit, not by the planner.

   **The threshold is a volume, and it explains why Domain 1 reached the opposite verdict.** Domain 1,
   KA 1.3.3b added three specialists to the *last eight* clinics and destroyed **USD 23,333.33**. The
   reason is now visible: the extra specialist-weeks a reinforcement consumes are **constant** — 10 for
   two newcomers, 15 for three, independent of how much work remains — while the weeks saved grow
   linearly with the volume remaining, `0.4166667N − 1.25` for two newcomers. On the full cost-of-delay
   basis two newcomers break even at `N =` **10.0588** clinics and on the ramp-integral basis at
   `N =` **17.1176**. Domain 1's eight clinics sit below both; forty sits far above. **Reinforcement pays
   above a computable remaining volume, so the same arithmetic gives opposite answers at the two ends of
   one rollout** — which is why the decision is re-taken as the programme runs down rather than settled
   once.

   **And the rate is capped by the drum, not by the crew.** Suppose one data-migration specialist can
   handle only **0.7** clinics a week. The achievable rate is then `min(0.8, 0.7) =` **0.7**, the rollout
   takes `40/0.7 =` **57.1429 weeks**, and the eighth specialist buys nothing: the **5.8929 weeks** the
   drum costs, worth `20 × 5.8929 × 357 =` **USD 42,075**, can be recovered only by relieving the drum
   (6.A.1). This is the most expensive error available in repetitive scheduling — sizing the crew from
   the takt while a subordinate step sets the actual rate — and the only defence is to compute the
   sustainable rate of **every** step, not of the one whose people are most numerous.

   **Two further cautions.** The 0.1-clinics-per-specialist-week rate is locally calibrated and clinics are not
   identical, so a takt computed from an average unit will be missed by the difficult units; the honest
   plan carries a unit-difficulty distribution and a recovery rule rather than a single takt. And a takt
   is a commitment to a *cadence*, which requires estate readiness, trained staff and approvals to
   arrive at the same cadence — which is where rollouts actually fail, the crew rarely being the binding
   constraint by the third month.

### 6.A.6 Buffer management: aggregating safety and reading its consumption

Every duration in the Auriga network contains protection. An eight-week procurement estimate is not
anyone's honest expectation of eight weeks; it is an expectation with a margin folded in, sized so that
the estimator is comfortable committing to it. That is rational behaviour with an expensive property:
**safety held inside each activity cannot be shared, while safety held at the end of a chain can be.**
A week of protection inside C is available only to C, and if C finishes early the protection is usually
consumed anyway, since there is no reward for finishing early when the successor is not ready.
Aggregating the protection into an explicit **buffer** converts private, unusable margin into a shared,
managed and visible reserve. Buffer sizing and buffer-consumption control belong to the
**critical-chain** tradition in schedule management, whose vocabulary KA 6.3.1c already uses; the
arithmetic below is worked from first principles on Auriga, and the conventions are named as
conventions rather than as results.

**Worked example 6.A.6 — sizing and then managing Auriga's project buffer.**

1. **Setup.** Auriga's disciplines are asked for two figures per critical-path activity: the duration in
   the network, and the duration they would give with an even chance of meeting it.

   | Activity | Network duration | 50 % duration | Embedded safety |
   |---|---|---|---|
   | A Mobilise | 2 | 1.5 | 0.5 |
   | B Detailed design | 6 | 5 | 1.0 |
   | C Procure control hardware | 8 | 6.5 | 1.5 |
   | E Installation | 5 | 4 | 1.0 |
   | F Testing and commissioning | 4 | 3 | 1.0 |
   | **Chain** | **25** | **20** | **5.0** |

   Size a project buffer, then manage against it. A week is worth **USD 45,000**.
2. **Formula.** Chain `= Σ` 50 % durations. Two sizing conventions: **half the aggregated safety**,
   `Σsᵢ / 2`, or **root-sum-square**, `√(Σ sᵢ²)` — the same aggregation Domain 8 uses when variances add
   (KA 8.A.2). Committed duration = chain + buffer. In control: consumption ratio = (buffer consumed ÷
   buffer) ÷ (chain complete ÷ chain); projected final consumption = buffer × consumption ratio.
3. **Substitution.** Chain `1.5 + 5 + 6.5 + 4 + 3 = 20`. Safety `0.5 + 1 + 1.5 + 1 + 1 = 5.0`.
   Half-safety buffer `5.0/2`. Root-sum-square `√(0.25 + 1 + 2.25 + 1 + 1) = √5.5`. At a status date
   with **45 %** of the chain complete and **30 %** of the buffer consumed: ratio `0.30/0.45`.
4. **Result.** The half-safety method commits to `20 + 2.5 =` **22.5000 weeks**, **2.5 weeks** inside the
   25-week network and worth **USD 112,500**. The root-sum-square buffer is **2.3452 weeks**, a
   commitment of **22.3452 weeks**, **2.6548 weeks** inside the network and worth **USD 119,465.65**. In
   control, the consumption ratio is **0.6667**: projected final buffer use `2.5 × 0.6667 =`
   **1.6667 weeks**, a forecast chain completion at **21.6667 weeks** with **0.8333 weeks** of buffer
   unused — **USD 37,500** still in hand, and **3.3333 weeks** inside the original network date.
5. **Interpretation. Aggregation is where the weeks come from, and it is arithmetic rather than
   pressure.** Nothing was
   crashed, no estimate was challenged, nobody was told to work faster: the plan is 2.5 weeks shorter
   because five weeks of private protection became 2.5 weeks of shared protection, and shared protection
   covers more contingencies per week held. The two conventions differ by only **0.1548 weeks** here,
   and the root-sum-square form is the more defensible because it is the variance-addition result Domain
   8 derives — which means it holds under **independence**, so where the activities share a cause (one
   subcontractor, one site condition, one approval body) the correct buffer is larger and approaches the
   simple sum of 5.0. A root-sum-square buffer across correlated activities is the schedule version of
   the aggregation error Domain 8, KA 8.2.4 prices for contingency.

   **Buffer consumption forecasts earlier than variance against baseline.** A ratio below 1 means
   protection is being consumed more slowly than the chain is being completed; above 1, faster. Auriga's
   0.6667 projects a finish with a third of the buffer unspent — information available at 45 %
   completion, from two numbers, without re-running a pass. It is the schedule counterpart of Domain 7's
   `EAC` family: an index observed to date, projected over what remains. The control regime should state
   thresholds in advance — below **0.5**, no action; **0.5 to 1.0**, prepare a recovery plan; above
   **1.0**, execute it — set and published before the first status date rather than derived from the
   first uncomfortable reading.

   **The linear projection is only as good as the resemblance between what is done and what remains.** A
   chain whose risky activities sit at the *end* shows a comfortable ratio for months and consumes its
   whole buffer in the final quarter. That is the standard misuse, and it is detectable, because the same
   three-point estimates that sized the buffer say which activities carry the spread: where the remaining
   work is riskier than the completed work, weight the projection by remaining variance rather than
   remaining duration — and say that you have.

   **The method fails completely if the 50 % durations are not 50 % durations.** Halving network
   durations by fiat produces the same table with none of the meaning: the buffer then protects estimates
   nobody believes, it is consumed in the first third of the chain, and the team correctly concludes that
   the plan was a compression exercise wearing a technique's clothing. Eliciting an even-chance duration
   requires that missing it half the time carries no penalty, which is a leadership commitment (Domain
   12) rather than a scheduling one. Where that commitment cannot honestly be made, the aggregation
   should not be attempted and the schedule should run with visible float and a float policy instead.

   **And the buffer is a governed reserve, not a hiding place.** It appears in the schedule as an
   explicit activity with an owner, its consumption is reported every cycle in the same pack as float,
   and it is never silently topped up — that is 6.A.2's re-baselining integrity rule applied to a
   buffer. A buffer nobody reports is indistinguishable from padding and will be removed by the first
   cost review that finds it.

---

## Industry variations — Domain 6

The passes are universal; what counts as a schedule, and what discipline it needs, is sectoral:

- **Construction and EPC.** Deep L3/L4 hierarchies, thousand-activity networks, contractual
  float-ownership clauses (Domain 10) and delay-analysis protocols make float a legal quantity —
  the honesty settings of the Executive perspective are contract compliance here, not just
  culture.
- **Technology and product.** Hybrid is the default (KA 6.4.1): cadence-based streams joined to
  a thin predictive network at integration milestones; the commonest defect is a network so
  sparse that convergence points are invisible until they slip.
- **Shutdowns and turnarounds.** The network runs in *hours*; near-critical analysis widens to
  every path within minutes of critical, and the drum logic of 6.A.1 governs a single shared
  crane or permit desk. Compression economics (6.4.2) turn over in shifts, not weeks.
- **Pharmaceutical and regulated development.** Milestones are regulatory events with
  submission-window physics — genuine date constraints (6.3.2), modelled as logic, with the
  pin-and-squeeze temptation at its most dangerous because windows are unforgiving.
- **Rollouts and repetitive delivery** — retail estates, network upgrades, clinic and branch
  programmes. The schedule is a **rate**, not a network: takt, crew count and the sustainable rate of
  every step, with the drum found before the crew is sized, and acceleration priced as a benefit
  integral rather than a cost slope (6.A.5). The binding constraint is almost never the installation
  crew; it is the upstream supply of site readiness, trained staff and approvals arriving at the same
  cadence.
- **Public programmes.** Announced dates precede networks (Case study B); the leader's
  protection — milestones only after the backward pass — is hardest exactly where it matters
  most, and negative-float honesty (6.A.4) is the difference between an early re-plan and a
  public failure.

## Case study — Domain 6: recovering Auriga (utilities / technology)

**Situation.** End of week 13. Civil and cabling works D — planned at 7 weeks from week 8 — hits
contaminated-ground remediation; the discipline lead's honest re-estimate takes D to 10 weeks
(finish week 18). The client's outage window for final commissioning opens at week 25 and closing
the window costs USD 45,000 per week of delay. The team re-runs the passes.

**Analysis.** With D = 10: E starts week 18 (D now later than C's week 16), F finishes week 27 —
**two weeks into the penalty window**, and the critical path has migrated to **A–B–D–E–F**.
Procurement C, critical all project, now carries `TF` = 2. G still holds float.

**The recovery decision.** Options priced by the team:

| Option | Weeks recovered | Cost | Net vs doing nothing |
|---|---|---|---|
| (1) Crash the *old* critical path (C) | 0 | 30,000/wk | worthless — C is no longer binding |
| (2) Second civil crew on the remediation | 1 | 35,000 | +10,000 |
| (3) Fast-track E against D's tail | 1 | 12,000 expected (20 % × 60,000) | +33,000 |
| (4) Do nothing | 0 | 90,000 penalty exposure | baseline |
| **(2) + (3) taken together** | **2** | **47,000 expected** | **+43,000** |

The leader takes (2) + (3): `2 × 45,000 = 90,000` of penalty avoided for **USD 47,000** of expected
cost, a net **USD 43,000**, and week 25 with the window intact.

**What the arithmetic settles, and what it does not.** Three figures decide the case and are worth
extracting, because each is a general instrument rather than a feature of this story.

The **ranking is by net, not by cost**. Option (3) is the cheapest and option (2) the dearer, yet
both are bought because each recovers a week worth 45,000 and both clear that bar; a leader who
stops at the cheapest option buys one week and pays a 45,000 penalty to save 35,000. The stopping
rule is 6.4.2b's: buy while the marginal week costs less than a week is worth.

The **fast-track's breakeven probability** is `45,000/60,000 =` **75.00 %** — the rework would have
to be three times likelier than the assessed 20 % before the overlap stopped paying. That is the
sentence that makes the recommendation defensible to an assurance reviewer, and it is a stronger
statement than the expected cost, because the assessed probability is the softest number in the
analysis. Note also what the full treatment of 6.4.2c would add: the assessed 20 % gives an expected
cost of 12,000, but the *downside* is a certain 60,000 against a certain 45,000 penalty avoided, so
the bad outcome leaves the project 15,000 worse off than doing nothing — a small enough exposure
that the expected-value test is the right test here, which is not always so.

The **new float table is the deliverable, not the new date.** With D at 10 weeks, procurement C —
critical for the whole project until this week — carries `TF` = **2**, and the critical path is
A–B–D–E–F. Every subsequent expediting conversation, every progress question and every recovery
option must be aimed at the new binding path; the float table is what redirects the team, and issuing
it is a separate act from announcing the recovered date. Teams that announce the date and not the
table spend the following month managing the path that used to matter.

**What the domain teaches here.** Re-run the passes before spending a cent (the instinctive
"crash procurement" would have bought nothing); price recovery in expected cost including risk, and
state the breakeven for the probability you are least sure of; harvest overlap where the physics
genuinely allows it; and report the new float positions to the client honestly — the schedule
survived because the remediation estimate was honest a full five weeks before the window (Domain 11's
reporting culture, applied to time).

## Case study B — Domain 6: the milestone that was typed, not computed (public programme)

**Situation.** A government services programme publishes a go-live milestone eighteen months out —
programme **week 78** — set in a ministerial announcement before the L3 schedule existed. When the
network is finally built, the test environment cannot exist before **week 69** and user-acceptance
testing needs **12 weeks**, so honest logic finishes at week 81: the backward pass returns
`TF` = **−3** from day one. To show the pinned date, the plan carries a **9-week** UAT window —
**75.00 %** of the tested requirement — and calls it a plan.

**What happened, in numbers.** For six monthly reporting cycles — **24 weeks** — the programme's L1
view showed the milestone green. The date was pinned, so each upstream slip was absorbed by silently
shortening the testing window: by cycle seven the environment date had moved to **week 73** and the
window to **5 weeks**, or **41.67 %** of the 12 weeks the test plan required. Two consequences were
accumulating while the reporting said nothing.

**The recovery problem grew by a factor of 2.33.** At cycle one the gap between honest logic and the
pinned date was 3 weeks; at cycle seven it was `73 + 12 − 78 =` **7 weeks**. Negative float does not
sit still while it is being concealed — the upstream slips that the squeeze absorbed were real, so
the pin converted a three-week problem into a seven-week one and spent 24 weeks of available runway
doing it.

**And the concealment moved the risk from schedule to product.** If acceptance-test detection is
taken as roughly proportional to the share of the test plan actually executed — a stated
simplification, and exactly the kind of asserted detection rate Domain 9, KA 9.2.2 warns must be
measured rather than assumed — then a regime designed to detect **75 %** of defects reaching it
detects `0.75 × 5/12 =` **31.25 %** when only five twelfths of the plan is run. The escape fraction
rises from **0.25** to **0.6875**: **2.75 times** as many defects reach live service. The squeeze
did not remove the programme's problem; it converted a visible schedule variance into an invisible
quality one, priced not by the schedule but by Domain 9's external-failure economics and paid by
users.

In cycle seven an assurance review (Domain 3, KA 3.3) ran the passes without the pin, surfaced the
negative float, and forced the choice the pin had deferred: descope the first release (Domain 5), or
move the date. The minister moved the date. What that cost politically is not quantified here — it
cannot honestly be — but the two figures above can be, and they are the ones that would have made the
case at cycle one.

**What the domain teaches here.** A typed milestone is a promise without a plan. Negative float
is not embarrassment to be formatted away; it is the earliest, cheapest warning the programme
will ever get (6.A.4), and its value decays because the problem it names grows. The tell was
available every cycle and needed no assurance review to find: **a test window shorter than the test
plan is a negative-float report written in a different notation**, and any reviewer who compares the
two numbers finds it in one line. And the leader's protection is procedural, not heroic: milestones
enter public commitments only *after* the backward pass says they exist.

---

## Executive perspective — Domain 6

What a project leader cannot delegate in this domain:

- **The logic sign-off.** Discipline leads own their links; the leader owns the assembled claim
  that this network is how the project actually works.
- **Float policy.** Who may spend float, in what increments, reported how — decided before the
  first progress update, not during the first crisis.
- **The honesty settings.** No pins doing logic's work; negative float reported the cycle it
  appears; re-baselining only through governance. Schedules fail morally before they fail
  mathematically.
- **The feasibility question.** A schedule is approved as a logic network and executed with people.
  The leader asks one question before signing: *has this been levelled, against which caps, and
  what was the peak?* Auriga's plan was infeasible on engineering capacity while only 73.33 % loaded
  on average (6.3.1c), and no amount of logical float protected it. A baseline signed without a
  resource histogram is a baseline signed without knowing whether it can be run.
- **The compression chequebook.** Crash and fast-track decisions are investments under
  uncertainty — the leader signs the expected-value arithmetic (6.4.2) and the risk acceptance
  (Domain 8), never a bare instruction to "go faster". The chequebook has a price list: the
  compression menu with its cost slopes, priced at baseline while every option is still open,
  because options expire and the same two weeks cost USD 70,000 at planning and USD 85,000 in the
  crisis (6.A.4).
- **The translation duty.** Boards hear dates; teams live in passes, floats and scenarios. The
  leader is the honest converter between the two languages — both directions.

## Calculation exercises — Domain 6

**Exercise 6.1** Using the Auriga network, compute `ES`/`EF` for every activity and the project
duration, showing the convergence at E.
*Solution.* A 0–2 · B 2–8 · C 8–16 · D 8–15 · E max(16,15)=16–21 · F 21–25 · G 15–17. Duration
**25 weeks**; E's start is set by C (16), not D (15) — the convergence rule takes the later
predecessor. Common error: taking the earlier predecessor at a merge (giving E 15–20 and a false
24-week project).

**Exercise 6.2** Complete the backward pass and state `TF` and `FF` for D and G.
*Solution.* From `LF` = 25: F 21/25 · E 16/21 · C 8/16 · G 23/25 · D `LF` = min(E LS 16, G LS 23)
= 16, `LS` = 9. **D: `TF` = 1, `FF` = min(16,15) − 15 = 0. G: `TF` = 8, `FF` = 25 − 17 = 8.**
Common error: computing D's `FF` against only E (giving 1) — free float tests *every* successor.

**Exercise 6.3** The client offers USD 45,000 per week saved; crashing C costs USD 30,000/week
(max 2 weeks); crashing E costs USD 55,000/week (max 1 week). Find the value-maximising plan.
*Solution.* Crash C by 1: 25→24, +15,000. Second week of C: co-critical B–D–E–F holds 24 —
−30,000, reject. Crash E by 1 (E is on *both* paths): 24→23, `45,000 − 55,000 = −10,000`,
reject. **Plan: crash C by one week; net +USD 15,000.** Common error: crashing E first because
it is "on more paths" — path count doesn't change its negative unit economics. Note the option set:
with only C and E on offer the answer is one week, and Exercise 6.8 shows the same network reaching
**22 weeks** once design B and commissioning F are also buyable — the optimum is a property of the
menu, not of the network.

**Exercise 6.4** Activity durations: o = 6, m = 8, p = 16 weeks. Compute `tₑ` and `σ`, and state
the deterministic bias.
*Solution.* `tₑ = (6 + 32 + 16)/6 =` **9.0 weeks**; `σ = (16 − 6)/6 =` **1.67 weeks**. The
deterministic plan at m = 8 understates the expectation by a full week — right-skewed tails
always pull the mean above the mode.

**Exercise 6.5** A programme's network shows `TF` = −2 on the binding path. List, in cost order,
the recovery families and the governance step each requires.
*Solution.* (1) Logic re-choice/re-sequencing — planner's proposal, leader's sign-off; (2) float
and resource harvesting from non-binding paths — float-policy authority; (3) crashing the binding
path — expected-value case, budget authority; (4) fast-tracking — risk acceptance recorded in
the register; (5) scope/acceptance renegotiation — sponsor and change control. Reporting the −2
honestly precedes all five (6.A.4).

**Exercise 6.6** Enumerate Auriga's paths with their floats. Then state the cumulative slip the
D–G chain can absorb without moving week 25, and the error a float report makes by summing D's and
G's total floats.
*Solution.* A–B–C–E–F **25**, float **0**; A–B–D–E–F **24**, float **1**; A–B–D–G **17**, float
**8**. On the D–G chain `d + g ≤ 8` with `d ≤ 1` from the tighter path, so the absorbable cumulative
slip is **8 weeks**, not the **9** implied by `1 + 8` — an overstatement of one week, or
**USD 45,000** of false comfort at Auriga's cost of delay. Common error: adding an activity float
column. The general form is that `n` activities on a path with float `f` report `n · f` while the path
holds `f`, so the overstatement factor is the number of activities on the path.

**Exercise 6.7** Auriga's engineering pool is capped at **6**. Engineer demands: A 2, B 4, C 1, D 3,
E 5, F 6, G 2. Compute total demand, average utilisation against the 25-week plan, and find the
week in which the early-start schedule breaches the cap. Then state the cheapest fix and its return.
*Solution.* Total demand `4 + 24 + 8 + 21 + 25 + 24 + 4 =` **110** engineer-weeks against
`6 × 25 =` **150** of capacity — **73.33 %** average utilisation. The early-start profile is 2, 4,
4 (C+D), 3 (C+G), then **7 in week 16–17** where E (5) overlaps G (2) — a breach of **one engineer for
one week**. Hiring one engineer for that week costs **USD 5,225** and preserves the 25-week duration
worth **USD 45,000** — a return of **8.61 times**. Common error: concluding from the 73.33 %
utilisation that capacity is adequate; feasibility is a property of the peak, not the average.

**Exercise 6.8** From the compression menu — C 30,000/wk (max 2), B 40,000/wk (max 2), D 35,000/wk
(max 1), E 55,000/wk (max 1), F 65,000/wk (max 1) — find the least-cost duration when a week is
worth 45,000, and state the range of week-values over which that answer holds.
*Solution.* Marginal costs of successive weeks: **30,000** (C ×1), **40,000** (B ×1, the first buy
that must shorten *both* long paths), **40,000** (B ×2), **55,000** (E), **65,000** (F), **65,000**
(C ×2 + D). Buy while the step is at or below 45,000: three weeks, cumulative **USD 110,000**,
**22 weeks**, net **+USD 25,000**. The answer holds for any week-value between **40,000 and 55,000**;
below 40,000 the optimum is 24 weeks and above 55,000 it is 21. Common error: comparing the *average*
crash cost at 22 weeks (**USD 36,666.67**) with 45,000 and buying a fourth week that costs 55,000 —
a **USD 10,000** loss.

**Exercise 6.9** An overlap saves 2 weeks at 45,000 each; if the overlap fails it costs 140,000 and
returns 1.5 weeks of the saving. A certain crash plan buys the same 2 weeks for 70,000. Compute both
breakeven probabilities and the expected value at an assessed 25 %.
*Solution.* Good outcome **+90,000**; bad outcome `0.5 × 45,000 − 140,000 =` **−117,500**; swing
**207,500**. Against doing nothing, `p* = 90,000/207,500 =` **43.37 %**. Against the crash plan's
certain net of `90,000 − 70,000 = 20,000`, `p** = 70,000/207,500 =` **33.73 %**. At `p` = 0.25 the
expected value is `90,000 − 0.25 × 207,500 =` **+38,125**, beating the crash plan by **18,125** while
carrying a **137,500** worse downside. Common error: computing only `p*` and overlapping in cases
where a certain alternative was better — once a paid alternative exists, `p**` is the relevant
threshold.

**Exercise 6.10** A critical chain's 50 % durations are 1.5, 5, 6.5, 4 and 3 weeks against network
durations of 2, 6, 8, 5 and 4. Size the buffer both conventional ways, then forecast completion from
a status showing 45 % of the chain complete and 30 % of the buffer consumed.
*Solution.* Chain **20.0** weeks; embedded safety 0.5, 1, 1.5, 1, 1 = **5.0**. Half-safety buffer
**2.5** → commitment **22.5000** weeks; root-sum-square `√5.5 =` **2.3452** → **22.3452** weeks,
**2.6548** weeks inside the 25-week network, worth **USD 119,465.65**. Consumption ratio
`0.30/0.45 =` **0.6667**; projected buffer use `2.5 × 0.6667 =` **1.6667** weeks; forecast chain
completion **21.6667** weeks with **0.8333** weeks of buffer unused. Common error: applying
root-sum-square to correlated activities, which understates the buffer — under shared causes the
correct figure approaches the simple sum of 5.0 (Domain 8, KA 8.2.4).

**Exercise 6.11** Meridian must install 40 clinics in 50 weeks. A specialist completes 0.1 clinics a
week. Compute the required takt, the crew, and the value of the acceleration from the existing crew of
six on both valuation bases.
*Solution.* Takt `50/40 =` **1.25 weeks**; required rate **0.8** clinics a week; crew
`0.8/0.1 =` **8**. With Domain 1's ramp (4 weeks, 25 % productivity, 50 % supervision) two newcomers
give a ramp rate of **0.55** and a post-ramp rate of **0.8**, so duration is
`4 + (40 − 2.2)/0.8 =` **51.2500** weeks against `40/0.6 =` **66.6667** — **15.4167** weeks saved for
**10** extra specialist-weeks, **USD 42,000**. On the ramp-integral basis the gain is
`20 × 15.4167 × 357 =` **USD 110,075** (net **+68,075**); on the switch-on-at-completion basis
`15.4167 × 14,280 =` **USD 220,150** (net **+178,150**). Common error: valuing rollout acceleration at
the fully-adopted cost of delay when benefit accrues per clinic — that doubles it, because the value
per week of a linear ramp is `(N/2) ×` per-unit benefit, exactly half the steady-state figure.

## Practitioner's toolkit — Domain 6

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 6.T.1 — Schedule quality gate (run before trusting any network)

- [ ] Zero dangles; zero loops; every lag has a stated reason.
- [ ] Date constraints listed, each justified as genuine external physics — none doing logic's job.
- [ ] Durations owned by the executing discipline, estimate class stated (far waves: ranged).
- [ ] Passes re-run after every change; critical-path durations sum exactly to project duration.
- [ ] Near-critical paths listed with their floats, and the band stated with what it captures.
- [ ] Float report is **per path**, not per activity; no summed float column anywhere.
- [ ] Float report distinguishes `TF` and `FF`; float policy names who may spend it.
- [ ] Milestones computed, owned, done-defined; no typed dates.
- [ ] Every approval modelled against the governance calendar, not approximated by a lag.
- [ ] Resource histogram attached, caps named as real or negotiable, peak stated with the excess area
      in resource-weeks; levelling rule and any better schedule sought both recorded.
- [ ] Where a buffer is used: 50 % durations genuinely elicited, sizing convention stated,
      correlation considered, buffer visible with an owner and reported with float.
- [ ] AI-drafted logic, levelling or forecasts marked, verified against a stated baseline, and owned
      by a named human.

### Toolkit 6.T.4 — Compression menu (priced at baseline, reviewed on the risk cycle)

Per candidate activity: current duration · which paths it lies on · cost slope per week · technical
maximum weeks · owning discipline and the basis of the quote · risk consequence of taking it
(coordination, quality, rework) · **non-financial ceiling** — the working-time, rest, fatigue,
safety-case, permit or agreement limit that bounds this lever, with the function that supplied it ·
**the approval it needs** beyond the money, naming the safety or approval-holding authority where one
is engaged · expiry — the date after which the option no longer exists. A lever whose ceiling binds is
struck from the menu, not discounted in it (6.4.2). Then, on
one line: the value of a week (cost of delay plus any time-related indirect cost), the resulting
least-cost duration, and the range of week-values over which that duration stays optimal. Re-priced
whenever the network changes, because the cheapest week is a property of the current binding paths.

### Toolkit 6.T.2 — Recovery plan (one page)

Slip statement (activity, cause, size, date detected) · re-run pass results (new duration, new
critical path, float table) · options priced in expected cost (re-sequence / harvest / crash /
fast-track / renegotiate), each with risk and owner, **and each with its non-financial ceiling and
the approval it needs** (6.4.2) · selected plan and **both** its decision authorities — the authority
spending the money and, where a shift pattern, night working, an overlap of disciplines or a
condition of an approval is engaged, the **named safety or approval-holding authority** · revised
commitments and the stakeholder notice list · review date.

### Toolkit 6.T.3 — Milestone register

Per milestone: computed date and current `TF` · owner · done-definition (acceptance reference) ·
contractual status and counterparty · L3 logic trace · reporting treatment (internal /
contractual / public) · change history.

## Exam preparation — Domain 6

**The calculation traps.** Taking the earlier predecessor at a convergence (Exercise 6.1) ·
computing `FF` against one successor instead of all (Exercise 6.2) · quoting `TF` after it has
been spent · **summing an activity float column, which overstates absorption by the number of
activities on the path (6.2.2)** · crashing from the original network instead of re-running passes
(path migration, 6.4.2) · **comparing the average crash cost with the value of a week instead of the
marginal one (6.4.2b)** · **inferring resource feasibility from average utilisation when it is set by
the peak (6.3.1c)** · **valuing a rollout acceleration at the fully-adopted cost of delay when
benefit accrues per unit, which doubles it (6.A.5)** · **rolling up ranged planning packages by adding
mid-points (6.3.3)** · **substituting `E[wait] = M/2 + L` for a planned approval whose submission date
is known (6.1.2b)** · treating the PERT mode as the mean, and forgetting that a symmetric estimate has
no bias at all (6.4.3) · reading a pinned milestone as a forecast · confusing project-level float with
activity-level slack in reports.

**Reflection questions.**
1. Your programme's board pack shows every milestone green. What three questions establish
   whether that is information or formatting? *(Computed or typed? Passes re-run this cycle?
   Where is the float table?)*
2. A subcontractor asks for "the float" on their package. What must you settle before answering?
   *(Which float — `TF` or `FF`; the float policy; contractual float ownership — Domain 10; and
   whether anyone else is already drawing on the same path float — 6.2.2.)*
3. When did you last see money spent on compression that the passes would have shown was
   worthless — and which governance step was missing? *(6.4.2; toolkit 6.T.2.)*
4. Has your last approved baseline been levelled against a real resource cap, and do you know its
   peak? *(6.3.1c; toolkit 6.T.1. If the answer is an average utilisation figure, it is not an
   answer.)*
5. Which of your approvals is currently modelled as a lag rather than as a date against a meeting
   calendar, and how much float is that costing you? *(6.1.2b.)*

## Domain 6 summary

This domain turned "when will it finish?" from an opinion into an instrument. Planning levels
give each audience a true view of one logic; dependency discipline makes the network a model
rather than a drawing, and an approval modelled against its governance calendar rather than as a
three-week label was worth four weeks of Meridian's schedule from one week of design work. The
forward and backward passes yield the dates, the two floats and the critical path — with the master
project showing why free float can vanish while total float survives, why convergence points compound
slippage, why an activity float column can never be summed (nine weeks reported, eight available),
and why a near-critical band of 10 % captures 85.71 % of the network rather than a footnote to it.
Resources, constraints, milestones and rolling waves connect the network to the real world's crews,
windows and horizons — and connect it hard: a plan only 73.33 % loaded on average was infeasible on
its peak, an activity with eight weeks of logical float set the resource-constrained duration, and one
engineer-week of excess demand was worth USD 45,000. Delivery flow closes the loop: a compression menu
with rising cost slopes whose least-cost duration is 22 weeks and holds for any week worth
40,000–55,000 — each lever carrying a non-financial ceiling above its price, and a shift-pattern,
night-working or simultaneous-operations move carrying a second signature the money cannot buy —
fast-tracking priced with two breakeven probabilities rather than a hope, recovery
ranked from re-choice to renegotiation, buffers sized by aggregating safety and managed by their
consumption ratio, rollouts scheduled as a takt and valued as an integral, and agile streams joined to
the network through a required rate read off the backward pass. The leadership spine throughout:
schedules fail morally before they fail mathematically — computed dates, visible float, reported
negative float, resource feasibility tested before signature, and machine output verified against a
stated baseline by named humans are what make a programme's word worth something.
