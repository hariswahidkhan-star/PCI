---
id: TPL-11
series: S10
series_name: Free Templates
title: Quantitative schedule risk analysis input sheet
subtitle: The inputs a simulation runs on, elicited so that the answer means something
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: professional
reading_time_min: 15
summary: >
  The three sheets a quantitative schedule risk analysis actually needs: duration uncertainty by activity
  with three-point estimates and a stated distribution, discrete risk events mapped from the risk register,
  and a run configuration that makes the result reproducible. Every input records who gave it, when, by
  what method and what would have to be true for the extremes — because an unattributed number cannot be
  challenged, and an unchallenged number is where anchoring hides.
linkedin:
  format: document
  hook: >
    If the pessimistic duration is ten per cent above the most likely on a forty-day activity, nobody
    estimated it — they anchored on the schedule. The fix is in how you ask: pessimistic first, deterministic
    duration hidden, and a written answer to "what would have to be true".
  tags: [ProjectControls, ScheduleRisk, QSRA, Planning, RiskManagement]
  asset: one-pager
gated: false
related: [BPG-17, BPG-05, TPL-10, TPL-14, BPG-10]
bok_domains: [10, 12]
sources: []
placeholders: 0
---

# Quantitative schedule risk analysis input sheet

> The inputs a simulation runs on, elicited so that the answer means something.

**In one paragraph.** The three sheets a quantitative schedule risk analysis actually needs: duration
uncertainty by activity with three-point estimates and a stated distribution, discrete risk events mapped
from the risk register, and a run configuration that makes the result reproducible. Every input records who
gave it, when, by what method and what would have to be true for the extremes — because an unattributed
number cannot be challenged, and an unchallenged number is where anchoring hides.

**Who this is for.** Planners, schedulers, risk analysts and project controls managers who run or
commission a quantitative schedule risk analysis; and the project managers who have to act on a P80 date
they did not produce.

---

## 1. When to use this

A quantitative schedule risk analysis — from here, QSRA — samples uncertain activity durations and discrete
risk events many times over the schedule network, and reports the distribution of completion dates that
results. This sheet is where its inputs are captured, argued over and recorded.

Use it at four points. **At sanction**, to state the confidence attached to the date being committed to.
**At baseline**, to size the schedule contingency and to identify which activities deserve the effort of
tighter control. **At a major re-plan**, because a re-plan changes the driving path and therefore changes
which uncertainties matter. **Before a completion date is contractually committed**, which is the one
occasion where the difference between the deterministic date and the P80 date has a price attached.

Do not run one on a schedule that has not passed a quality review. A simulation propagates whatever logic it
is given: with open ends, the uncertainty on an activity with no successor cannot reach the completion
milestone, and the model will report a reassuringly tight distribution because most of the network is not
connected to the answer. Run `TPL-14 — Schedule quality review checklist` first and record the outcome in
the run configuration.

A QSRA is not a forecast of what will happen. It is a statement about the schedule as modelled, under the
uncertainty as elicited. Both halves of that sentence belong in the report.

## 2. How to complete it

### 2.1 Decide what is in scope before you elicit anything

Model the driving paths and everything that could become one. A common and defensible approach is to model
every activity with any float below an agreed threshold, plus every long-duration activity regardless of
float, plus every activity a risk event attaches to. State the threshold and the reason for it in the run
configuration. Modelling every activity in a ten-thousand-line schedule is usually a way of spending three
weeks to produce a less trustworthy answer than modelling four hundred of them well.

For activities in progress at the data date, model the **remaining** duration, not the original. Column H
does this automatically from the status in column E.

### 2.2 Elicit the three-point estimate in a way that does not manufacture the answer

Anchoring is the dominant failure mode of duration elicitation, and it is not a character flaw in the
estimator. Show someone a number and their subsequent estimates cluster around it. The deterministic
duration in the schedule is a number, and it is usually on the screen. Six practices counter it, and they
cost nothing but discipline.

**Hide the deterministic duration during the elicitation.** If the estimator can see 40 days, the answers
will orbit 40 days. Ask for the three-point estimate cold, and only then reveal the schedule duration and
discuss the difference. Column X flags every row where the most likely equals the schedule duration exactly,
which is the signature of an anchored input.

**Ask for the pessimistic value first, then the optimistic, then the most likely.** Asking for the most
likely first plants the anchor yourself. Starting at the extremes produces wider and better-calibrated
ranges.

**Define the extremes as frequencies, not adjectives.** "Worst case" has no upper bound, so people either
give an unusable number or an embarrassed one. Ask instead: *if this activity were run ten times under the
conditions we expect, what duration would you beat in nine of them?* That is a question an experienced
person can answer. Do the same at the optimistic end.

**Require a written answer to "what would have to be true".** Columns M and N are mandatory. An extreme with
no story behind it is a number, not an estimate, and it cannot be reviewed by anyone else. This single
requirement removes more bad inputs than any amount of statistical checking.

**Elicit individually before the group converges.** Ask each estimator separately and record the spread of
answers before any discussion. Where individual answers differ by a factor of two, that disagreement is the
most useful information in the whole exercise, and a workshop will destroy it in four minutes.

**Name the person, not "the team".** Column O takes a name and a role. Attribution is not about blame; it is
about being able to go back and ask a specific person a specific question when the model produces something
surprising.

Two further traps. If a discrete risk from the register is being modelled as an event on sheet 2, it must
not also be baked into the pessimistic duration on sheet 1 — column L on sheet 2 flags candidates for that
double count. And if the estimator's most likely duration is materially longer than the schedule duration,
column AA will show it: that is not a risk finding, it is a schedule finding, and it should be dealt with as
one before the simulation runs.

### 2.3 Choose the distribution deliberately

Three shapes cover almost everything, and the choice affects the answer.

**Triangular** takes the three points literally and gives real weight to the extremes. It is the honest
default when the estimator has genuine views about how bad things can get, and it produces a wider spread
than the alternatives.

**BetaPERT** weights the most likely value four times as heavily as the extremes, producing a smoother,
tighter distribution. It suits activities where the most likely value is well evidenced and the extremes are
softer judgements.

**Uniform** says every duration between the optimistic and pessimistic values is equally likely. That is a
strong statement of ignorance and is occasionally the correct one — a permit determination with a statutory
window and no other information, for instance. It should be rare, and it should be justified in column AE.

Record the choice per activity in column L. A model in which every activity carries the same distribution
because that was the software default is not a modelling decision; it is an absence of one.

### 2.4 Set correlation, and record why

Activities that share a crew, a supplier, a design package, a weather window or a permitting authority do
not vary independently. If the crew is slow on one, it is slow on the others. Modelling them as independent
lets the simulation cancel the variation out, which narrows the distribution and produces a P80 that sits
implausibly close to the P50 — the classic symptom of an uncorrelated model.

Assign a correlation group in column AB and a coefficient in column AC. Assigning correlation is judgement,
not measurement, and the honest treatment is to record the reason for each group in column AE, run the model
at the assigned coefficients, then re-run at a materially different set and report how much the answer
moved. If it moves a lot, correlation is the dominant assumption in your analysis and the report should say
so.

## 3. The template

Three sheets. Headers in row 1, data from row 2, formulas written for row 2 and filled down.

### 3.1 Sheet 1 — `Durations`: input columns

| Col | Field | What goes in it |
|---|---|---|
| A | Activity ID | Exactly as it appears in the schedule, so the mapping can be verified |
| B | Activity name | As in the schedule |
| C | WBS or discipline | For grouping the results |
| D | Calendar ID | The activity's calendar in the schedule. Durations are in that calendar's working days |
| E | Status | Not started · In progress · Complete |
| F | Deterministic duration | The original duration in the current schedule, in working days |
| G | Remaining duration at the data date | For in-progress activities only |
| I | Optimistic duration (O) | The duration that would be beaten only about one time in ten |
| J | Most likely duration (M) | The single most probable duration |
| K | Pessimistic duration (P) | The duration that would be beaten about nine times in ten |
| L | Distribution | Triangular · PERT · Uniform |
| M | Basis of the optimistic value | What would have to be true. Mandatory |
| N | Basis of the pessimistic value | What would have to be true. Mandatory |
| O | Elicitation source | Name and role of the person who gave the estimate |
| P | Elicitation date | |
| Q | Elicitation method | Individual interview · Facilitated workshop · Reference class · Analyst assumption |
| R | Input confidence | High · Medium · Low. Drives the review order, not the model |
| AB | Correlation group | A short group code, e.g. `CIVILS-CREW-A` |
| AC | Correlation coefficient | The coefficient assigned within the group, as a decimal |
| AD | Risk drivers mapped | Risk IDs from the register that drive this activity's uncertainty |
| AE | Notes and assumptions | Including the reason for the correlation group and any unusual distribution choice |

### 3.2 Sheet 1 — `Durations`: calculated columns

| Col | Field | Formula in words | Spreadsheet expression |
|---|---|---|---|
| H | Duration modelled | The remaining duration if the activity is in progress, otherwise the deterministic duration | `=IF($E2="In progress",$G2,$F2)` |
| S | Order check | Confirms the three points are in ascending order; blank until all three are entered | `=IF(COUNT($I2:$K2)<3,"",IF(AND($I2<=$J2,$J2<=$K2),"OK","Check order"))` |
| T | Spread ratio | Pessimistic divided by most likely; blank if the most likely is zero or empty | `=IF(N($J2)=0,"",$K2/$J2)` |
| U | Downside days | Pessimistic less most likely | `=IF(COUNT($J2:$K2)<2,"",$K2-$J2)` |
| V | Upside days | Most likely less optimistic | `=IF(COUNT($I2:$J2)<2,"",$J2-$I2)` |
| W | Skew ratio | Downside days divided by upside days; reported as text where no upside was modelled | `=IF(OR($U2="",$V2=""),"",IF($V2=0,"No upside modelled",$U2/$V2))` |
| X | Anchoring flag | Flags rows where the most likely duration equals the duration in the schedule exactly | `=IF(OR($H2="",$J2=""),"",IF($J2=$H2,"Most likely equals schedule — challenge",""))` |
| Y | Distribution mean | Triangular: the three points averaged. PERT: the three points averaged with the most likely weighted four times. Uniform: the midpoint of the two extremes | `=IF($L2="Triangular",($I2+$J2+$K2)/3,IF($L2="PERT",($I2+4*$J2+$K2)/6,IF($L2="Uniform",($I2+$K2)/2,"")))` |
| Z | Distribution standard deviation | Triangular: the closed-form standard deviation of a triangular distribution. PERT: the classical approximation, the range divided by six. Uniform: the range divided by the square root of twelve | `=IF($L2="Triangular",SQRT(($I2^2+$J2^2+$K2^2-$I2*$J2-$I2*$K2-$J2*$K2)/18),IF($L2="PERT",($K2-$I2)/6,IF($L2="Uniform",($K2-$I2)/SQRT(12),"")))` |
| AA | Mean less modelled duration | The distribution mean less the duration currently in the schedule | `=IF(OR($Y2="",$H2=""),"",$Y2-$H2)` |

The mean and standard deviation columns are **sense checks on the inputs, not the model**. The simulation
samples the distribution itself; it does not use these figures. The PERT standard deviation in column Z is
the classical approximation and differs slightly from the exact beta-PERT value — it is here to let a
reviewer spot an input whose spread is implausible for the work described, which it does perfectly well.

Column AA is the one to read first at a review. If the sum of column AA over the driving path is large and
positive, the schedule's durations are systematically shorter than the people doing the work believe. That
is a planning problem masquerading as a risk problem, and no amount of simulation will fix it.

### 3.3 Sheet 2 — `Risk events`

Discrete events from the risk register that are modelled as occurrences rather than as duration spread.

| Col | Field | Input or calculated | What goes in it |
|---|---|---|---|
| A | Risk ID | Input | Matching the ID in `TPL-10 — Risk register`, so the two can be reconciled |
| B | Risk title | Input | The event, in the register's words |
| C | Modelled as | Input | Discrete event · New logic branch · Both |
| D | Probability of occurrence | Input | As a decimal. Where it differs from the register, state why in column N |
| E | Optimistic added duration | Input | Working days |
| F | Most likely added duration | Input | Working days |
| G | Pessimistic added duration | Input | Working days |
| H | Distribution | Input | Triangular · PERT · Uniform |
| I | Attaches to | Input | The activity ID or logic path the impact applies to |
| J | New logic created | Input | Describe any rework loop, re-tender or re-mobilisation the event introduces |
| K | State modelled | Input | Pre-response · Post-response. Which of the register's two assessments this row represents |
| L | Double-count check | Calculated | Flags where the same risk ID also appears against a duration input |
| M | Expected added duration | Calculated | Sense check only |
| N | Notes | Input | Including the reason for any divergence from the register |

| Col | Formula in words | Spreadsheet expression |
|---|---|---|
| L | Flags the row if this risk ID also appears in the risk-driver column of the durations sheet | `=IF(COUNTIF(Durations!$AD:$AD,"*"&$A2&"*")>0,"Also mapped to a duration input — check for double counting","")` |
| M | The probability multiplied by the mean of the impact distribution | `=IF(OR($D2="",$H2=""),"",$D2*IF($H2="Triangular",($E2+$F2+$G2)/3,IF($H2="PERT",($E2+4*$F2+$G2)/6,IF($H2="Uniform",($E2+$G2)/2,""))))` |

Column L is a prompt, not a proof — it catches the case where the same identifier appears in both places, not
the case where the same *uncertainty* has been described twice in different words. That one is found by
reading, at the review.

Column M is a sense check with a caveat that should be printed next to it: a discrete event does not add its
expected duration to anything. It either happens, in which case it adds its sampled impact, or it does not,
in which case it adds nothing. The expected value is useful for ranking the events and for nothing else.

### 3.4 Sheet 3 — `Run configuration and outputs`

Reproducibility is the point of this sheet. A QSRA that cannot be re-run to the same numbers cannot be
defended.

| Field | What goes in it |
|---|---|
| Schedule file name and version | The exact file analysed |
| Data date | |
| Schedule quality review reference and date | The `TPL-14` review this schedule passed, and any check it failed |
| Activities in schedule / activities modelled | Both numbers, with the in-scope rule stated |
| Float threshold used for scope selection | And the reason for it |
| Sampling method | Latin hypercube · Simple random |
| Iterations | With the stability check below |
| Random seed | Recorded so the run can be repeated exactly |
| Correlation matrix reference | Where the coefficients and their reasons are held |
| Calendars used | And any calendar overridden for the analysis, with the reason |
| Exclusions | What was left out of the model and why. The most-read line on this sheet |
| Analyst | Name and date |
| Reviewer | Name and date |

**Outputs record**, one row per milestone:

| Field | What goes in it |
|---|---|
| Milestone ID and name | |
| Deterministic date | The date in the schedule as analysed |
| P50 date · P80 date · P90 date | |
| Working days from deterministic to P80 | The number the project is actually being asked to fund or absorb |
| Confidence at the committed date | The percentile at which the currently committed date sits |
| Top five drivers | By sensitivity or correlation to the milestone, named |
| The sentence this supports | One sentence stating what the analysis does and does not show |

**Stability check.** Run the model twice with different random seeds and record both P80 dates. The absolute
difference between them, in working days, must be smaller than the precision you intend to report — if you
are going to report a date to the day, a two-day swing between runs means the iteration count is too low.
Increase iterations and re-run. State both runs and the difference on this sheet rather than reporting a
single number and hoping.

### 3.5 Pasting it into a spreadsheet

Copy each header line into cell A1 of its own sheet and split on the pipe character.

```
Activity ID|Activity name|WBS or discipline|Calendar ID|Status|Deterministic duration|Remaining duration|Duration modelled|Optimistic|Most likely|Pessimistic|Distribution|Basis of optimistic|Basis of pessimistic|Elicitation source|Elicitation date|Elicitation method|Input confidence|Order check|Spread ratio|Downside days|Upside days|Skew ratio|Anchoring flag|Distribution mean|Distribution SD|Mean less modelled|Correlation group|Correlation coefficient|Risk drivers mapped|Notes and assumptions
```

```
Risk ID|Risk title|Modelled as|Probability|Optimistic added|Most likely added|Pessimistic added|Distribution|Attaches to|New logic created|State modelled|Double-count check|Expected added duration|Notes
```

## 4. Worked fragment

*Illustrative figures.* Durations in working days on a five-day calendar. Three activities from a
`Durations` sheet, showing the checks doing their work.

| Activity | Modelled | O | M | P | Distribution | Spread ratio | Down | Up | Skew | Mean | SD | Mean less modelled | Anchoring flag |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| A-1420 Install and terminate switchgear | 20 | 18 | 22 | 34 | PERT | 1.55 | 12 | 4 | 3.00 | 23.33 | 2.67 | +3.33 | — |
| A-1510 Commission and energise | 15 | 12 | 16 | 30 | Triangular | 1.88 | 14 | 4 | 3.50 | 19.33 | 3.86 | +4.33 | — |
| A-1180 Civil works to substation base | 40 | 38 | 40 | 44 | Triangular | 1.10 | 4 | 2 | 2.00 | 40.67 | 1.25 | +0.67 | Most likely equals schedule — challenge |

**The substitutions.**

A-1420, PERT mean: `(18 + 4 × 22 + 34) ÷ 6 = (18 + 88 + 34) ÷ 6 = 140 ÷ 6 = 23.33` working days. Classical
PERT standard deviation: `(34 − 18) ÷ 6 = 16 ÷ 6 = 2.67` working days. Spread ratio: `34 ÷ 22 = 1.55`. Mean
less modelled duration: `23.33 − 20 = +3.33` working days.

A-1510, triangular mean: `(12 + 16 + 30) ÷ 3 = 58 ÷ 3 = 19.33` working days. Triangular standard deviation:
`SQRT((12² + 16² + 30² − 12×16 − 12×30 − 16×30) ÷ 18) = SQRT((1,300 − 1,032) ÷ 18) = SQRT(14.889) = 3.86`
working days.

A-1180, triangular mean: `(38 + 40 + 44) ÷ 3 = 122 ÷ 3 = 40.67` working days. Standard deviation:
`SQRT((1,444 + 1,600 + 1,936 − 1,520 − 1,672 − 1,760) ÷ 18) = SQRT(28 ÷ 18) = SQRT(1.556) = 1.25` working
days.

**What the checks found.** A-1180 is the row to challenge, and two columns say so independently. The most
likely duration is exactly the schedule duration, so the anchoring flag fires. The spread ratio is 1.10,
meaning the estimator believes a forty-day civils activity will finish within four days of plan nine times
out of ten — a claim that would be remarkable on a site with no weather, no ground and no other trades. The
standard deviation of 1.25 days on a forty-day activity says the same thing in a different unit. Almost
certainly the estimator was shown the schedule and asked to put a range around it. That row should be
re-elicited before the model is run, hiding the deterministic duration and asking for the pessimistic value
first.

Compare A-1510, which looks like a real estimate: a skew ratio of 3.5 says the estimator sees far more ways
for commissioning to run long than to run short, which is what commissioning is like. The basis fields on
that row should say what those ways are.

**What the fragment does not show.** The three activities have a combined mean-less-modelled figure of
`3.33 + 4.33 + 0.67 = 8.33` working days. That is **not** eight days of schedule slip. Uncertainty combines
through the network rather than adding along a list: some of these activities may not be on the driving
path, paths merge, and merge bias pushes the completion distribution later than any single path suggests.
Only the simulation over the network can produce the completion distribution, which is exactly why this
sheet is an input and not an answer.

## 5. Common mistakes

**Running the model on an unreviewed schedule.** Open ends, missing successors and constraints that override
logic all suppress the propagation of uncertainty. The result is a tight, confident and meaningless
distribution. This is the single most common way a QSRA misleads.

**Anchored three-point estimates.** Symptoms: most likely values that equal the schedule durations, spread
ratios near 1, and empty basis fields. Columns M, N, T and X are all pointed at this failure because it is
the one that quietly determines the answer.

**Symmetric ranges.** Duration risk is almost never symmetric — there are more ways for work to take longer
than to finish early, and the ways it can finish early are bounded by physics and the supply chain while the
ways it can run long are not. A register of symmetric triangles usually means the analyst applied a
percentage rather than eliciting anything.

**Percentage uncertainty applied globally.** Applying a blanket "minus 10 %, plus 30 %" to every activity is
fast, defensible-sounding and worthless: it says nothing the schedule did not already say, and it produces a
ranking of drivers that is simply a ranking of durations.

**Double counting.** A risk modelled as a discrete event on sheet 2 and also written into the pessimistic
duration on sheet 1 is counted twice. Column L catches the flagged case; the rest is found by reading the
basis fields against the risk descriptions.

**Ignoring correlation.** Independent sampling of activities that share a crew, a supplier or a weather
window produces a distribution that is too narrow. If your P80 is only a handful of days beyond your P50 on
a multi-year project, correlation is the first place to look.

**Reporting a P-value without the exclusions.** "P80 completion is 14 March" is not a finding until it is
followed by what was modelled, what was excluded, and which state — pre-response or post-response — the risk
events were modelled in. A P80 that excludes the consent risk and a P80 that includes it are different
numbers with the same name.

**Treating the P80 as a target.** It is a confidence statement about a model. Handing it to the delivery
team as the new date converts a risk analysis into a schedule extension and guarantees that the next one
will be gamed.

## 6. Adapting it

**Safe to change.** The distribution options, the in-scope selection rule, the correlation coding, the
addition of columns for anything your tool imports. If your software takes distribution parameters rather
than three points, add the parameter columns and keep the three points as the human-readable record of what
was elicited.

**Safe to add.** A resource-uncertainty block where labour availability, not duration, is the driver. A
weather block that models seasonal calendars as a distribution rather than an activity. A column recording
the individual answers before group convergence, which is often the most informative data the exercise
produces.

**Do not change.** The mandatory basis fields for both extremes; the named elicitation source and date; the
recording of the seed and the iteration count; and the separation between duration uncertainty and discrete
events. Each of those exists because a QSRA is an argument, and every one of them is a place where the
argument is either supported or is not.

### 6.1 Before the model is run

- The schedule has passed a quality review, and any failed check is recorded with its effect on the analysis.
- The in-scope rule is written down, and the count of activities modelled against activities in the schedule
  is stated.
- Every modelled activity has all three points, a stated distribution, and both basis fields completed.
- The order check reports OK on every row.
- Every anchoring flag has been either cleared by re-elicitation or annotated with why the estimate stands.
- Every spread ratio below an agreed floor has been reviewed and either corrected or justified.
- In-progress activities are modelled on remaining duration, and the data date matches the schedule.
- Every discrete risk event names a register ID, states which response state it models, and has been checked
  for double counting against the duration inputs.
- Correlation groups have a written reason each, and a sensitivity run at different coefficients is either
  done or scheduled.
- The seed, the iteration count and the stability check between two seeds are recorded.
- The exclusions line is complete and has been read out loud to whoever will act on the result.

---

## Related

- `BPG-17 — Quantitative schedule risk analysis` — the method: what the simulation does, how merge bias
  arises, and how to read a tornado chart without over-reading it
- `BPG-05 — Schedule quality — a practical review` — why an unreviewed schedule cannot carry a simulation
- `TPL-14 — Schedule quality review checklist` — the review that must precede the run, and the record the
  run configuration references
- `TPL-10 — Risk register` — the source of the discrete events on sheet 2, and the register that must
  reconcile to them
- `BPG-10 — Contingency and management reserve` — how the output distribution becomes a schedule contingency
  someone will actually approve

## Sources and standards

This is an original instrument developed by the Institute. It reproduces no third-party template, form or
worked example. The triangular, PERT and uniform distributions and their closed-form means and standard
deviations are standard results in probability, computed here rather than quoted, and every figure in §4 was
independently recomputed before publication. The classical PERT standard-deviation approximation is
identified as an approximation wherever it is used. The elicitation guidance in §2.2 describes practice in
the Institute's own words and does not reproduce any published protocol.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
