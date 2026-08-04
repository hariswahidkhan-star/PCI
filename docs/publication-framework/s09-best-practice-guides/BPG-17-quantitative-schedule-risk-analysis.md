---
id: BPG-17
series: S09
series_name: Best Practice Guides
title: Quantitative schedule risk analysis
subtitle: What a P80 date is confidence in, and what it is not
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: professional
reading_time_min: 16
summary: >
  Quantitative schedule risk analysis converts an uncertain network into a distribution of completion
  dates. This guide sets out what that distribution can honestly be said to mean, how three-point estimates
  are elicited without importing the estimator's anchor, why correlation is the assumption that most often
  determines the answer, the difference between duration uncertainty and register-driven risk events, how
  to read a P50 and a P80 as confidence in a model rather than in the world, and why running the analysis on
  a schedule that fails a quality review produces a distribution of the constraint set. Every figure in the
  worked example is hand-computed and checkable.
linkedin:
  format: article
  hook: >
    Four work fronts converge on one commissioning milestone. Each is independently 70 per cent likely to
    be ready. The chance all four are ready is 0.70 to the fourth power — 24 per cent. If one shared driver
    moves them together it is 70 per cent. Nothing in the schedule tells you which, and that assumption is
    worth more than the rest of the model.
  tags: [ProjectControls, ScheduleRisk, QSRA, MonteCarlo, RiskManagement]
  asset: carousel-8
gated: false
related: [BPG-05, BPG-10, BPG-16, TPL-11, TPL-14, AIG-05]
bok_domains: [10, 12]
sources:
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 10 — Project Scheduling, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 12 — Risk Management for Project Controls, first authored draft, August 2026"
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), verified August 2026"
placeholders: 0
---

# Quantitative schedule risk analysis

> What a P80 date is confidence in, and what it is not.

**In one paragraph.** Quantitative schedule risk analysis converts an uncertain network into a distribution
of completion dates. This guide sets out what that distribution can honestly be said to mean, how
three-point estimates are elicited without importing the estimator's anchor, why correlation is the
assumption that most often determines the answer, the difference between duration uncertainty and
register-driven risk events, how to read a P50 and a P80 as confidence in a model rather than in the world,
and why running the analysis on a schedule that fails a quality review produces a distribution of the
constraint set. Every figure in the worked example is hand-computed and checkable.

**Who this is for.** Planners and schedulers who run or commission the analysis; project controls managers
who must defend its output; and project and programme managers asked to commit to a date that came out of it.

---

## 1. What the analysis actually does

Quantitative schedule risk analysis — **QSRA** — takes a logic-driven schedule, replaces fixed durations with
ranges, adds discrete risk events from the risk register, samples the network many thousands of times and
reports the distribution. Its output is completion dates with probabilities attached: the **P50**, reached on
or before in half the runs; the **P80**, in eighty per cent of them; and the curve joining them.

Three things it does that a deterministic schedule cannot. **It exposes the optimism in the merge**: a
forward pass takes the latest arrival at each convergence point, which is correct arithmetically and
optimistic probabilistically, because the milestone is met only if *every* incoming path is met — and
simulation samples all paths together. **It ranks the drivers**: what changes behaviour is rarely the date
but the sensitivity ranking of which activities and register entries do the work in the tail, which is a
priced mitigation shopping list. **It separates commitment from target**, letting a project hold an internal
target and an external commitment at different confidence levels with the gap named.

And three it does not. **It does not tell you what the project will do** — only what the model does (§6). **It
does not find risks**: whatever the workshop missed is absent from the distribution, invisibly. **It does not
repair a schedule**: a network that cannot recalculate honestly cannot be simulated honestly (§5).

## 2. Three-point estimates and the elicitation problem

The input for duration uncertainty is a range per activity: optimistic, most likely and pessimistic values,
written *a*, *m* and *b*. Getting them is an interviewing problem before it is a modelling problem, and the
interviewing is where most QSRA quality is won or lost.

The dominant failure is **anchoring**. The estimator is shown the scheduled duration and asked for a range
around it. That number becomes the anchor, the range comes back as roughly plus or minus ten or fifteen per
cent, and the model inherits the schedule's optimism with a decorative band around it. Where the durations
were themselves set by working backwards from a required completion date — common, and rarely admitted — the
anchoring imports that too.

Countermeasures, in the order they matter. **Ask for the pessimistic value first, and as a story**: "what
would have to happen for this to take twelve weeks?" requires a mechanism, and if none can be described the
value is too low. **Elicit from the person who does the work, not the person accountable for the date** —
accountability narrows ranges, not dishonestly, because someone who has committed to a date genuinely finds
the pessimistic scenario less plausible than the crew supervisor does. **Do not show the scheduled duration
until the range is given**, and where that is impractical, record whether it was shown. **Hold everyone to
one definition of the pessimistic value.** **Calibrate against outturn** — showing estimators how their
previous ranges compared with what happened is the most effective correction available, and
`BPG-20 — Closeout, lessons learned and benchmarking` covers capturing that data.

Two conventions from the programme evaluation and review technique tradition summarise a three-point range:
the weighted mean `(a + 4m + b) ÷ 6` and the spread approximation `(b − a) ÷ 6`. Both are conventions rather
than requirements of any standard, and both approximate one family of distributions. They are used in §8
because they can be checked by hand.

## 3. Duration uncertainty and risk events are different inputs

Two distinct sources of schedule uncertainty are frequently conflated, and conflating them either
double-counts or omits.

**Duration uncertainty** is the variability in how long work takes when it goes ahead as planned:
productivity varies, weather varies, the crew is better or worse than assumed. It applies to every activity,
is modelled by the three-point range, and carries no probability of occurrence — the work happens, the
duration varies.

**Risk events** are discrete occurrences from the register that may or may not happen: a permit refused, a
vendor failure, contaminated ground. They carry a probability of occurrence *and* an impact range, strike
specific activities, and are modelled by mapping register entries onto the activities they would hit. That
mapping is the **risk-driver** approach, and its virtue is that the model inherits the register instead of
running on a parallel set of assumptions.

The double-count happens when an estimator, asked for a pessimistic duration, includes a register entry —
"twelve weeks if the permit is late" — and the modeller then loads the permit risk onto the same activity.
The omission runs the other way: ranges elicited as "how long if nothing goes wrong", nothing loaded, and an
analysis reported as showing a robust schedule. One instruction prevents both, given at elicitation and
recorded on the input sheet: *give the range assuming the identified risks do not occur; the register is
loaded separately.* `TPL-11 — Quantitative schedule risk analysis input sheet` carries the field structure,
and `BPG-16 — Risk registers that work` the register discipline it depends on.

## 4. Correlation is the assumption that decides the answer

Simulation software will happily sample every activity independently. Real projects do not: activities share
crews, a design authority, a weather season, a fabrication market, an approval process. When a shared driver
moves, it moves everything it touches in the same direction at once.

**Correlation does not move the mean. It fattens the tail.** The expected value of a sum equals the sum of
the expected values, correlated or not. What changes is the spread: independent components partly cancel —
some run long while others run short — and that cancellation is what correlation removes. The P50 barely
moves; the P80 and P90 move a lot.

Two structural cases matter, and they run in opposite directions. **Along a chain**, correlation increases
the total's variability: four independent activities of equal spread give a chain whose spread is twice one
activity's, because standard deviations add in quadrature, while perfectly correlated they give four times.
**At a merge**, correlation *reduces* the penalty: paths driven by one shared cause tend to be ready together
or late together, so the milestone's probability approaches that of a single path, whereas independent paths
multiply their probabilities. Section 8 computes both. Because they pull opposite ways, "assume some
correlation to be safe" is not a conservative default.

The practical method needs no mathematics. List the shared drivers explicitly before the model is built — one
weather season, one commissioning contractor, one approval authority, one specialist crew — and record which
activities each touches. That list, not a coefficient chosen by feel, belongs in the basis document. Where
the tool demands a coefficient, record where the number came from and treat it as an assumption to be tested
by sensitivity.

## 5. What the analysis needs before it is worth running

QSRA amplifies whatever is underneath it. Five preconditions, each checkable in an afternoon:

- **The schedule recalculates** — every activity except the first and last has a predecessor and a successor;
  date constraints are rare and justified; negative float is absent or explained; long lags are defended;
  progress is not out of sequence. `BPG-05 — Schedule quality — a practical review` and
  `TPL-14 — Schedule quality review checklist` own these checks.
- **The critical path is the work, not the constraints.** A network held together by imposed dates does not
  respond to sampled durations, so sampling it yields a distribution of the constraint set.
- **The level of detail suits simulation** — thousands of short activities give spurious precision and an
  elicitation effort nobody completes honestly; thirty summary bars hide the merges that drive the answer.
- **The register is current and loadable** — cause-event-effect entries, scored, mappable onto activities.
- **Someone owns the basis document** — distribution shapes, correlation drivers, what the ranges include and
  exclude, what was not modelled, written before the run.

The second is the most consequential and the most often skipped, because a heavily constrained schedule still
produces a convincing curve. Without the fifth, a QSRA cannot be reviewed, only believed.

## 6. Reading a P-value honestly

A P80 completion date is a statement about a model: *in eighty per cent of the simulated runs of this
network, with these ranges, these loaded risks and these correlation assumptions, completion occurred on or
before this date.* Every clause is load-bearing.

**A P80 is not an eighty per cent chance of finishing by that date.** It is an eighty per cent chance *within
the model's world* — a world excluding every risk not identified, every range not honestly given, every
correlation not declared and every structural failure the schedule contains. Each omission moves the real
probability down; none moves it up.

**The gap between the deterministic date and the P50 is diagnostic.** A deterministic date far below the P50
means optimistic durations, heavy merging, or both — a finding about the schedule, not about risk, and one to
report before any conversation about contingency.

**The shape matters more than the point.** A curve nearly vertical between P50 and P80 says the date is
fairly well determined; a long shallow tail says a small increase in required confidence costs a great deal
of time. Report the curve, not two numbers off it. And a P80 reported to the day, on ranges elicited to the
nearest fortnight, is false precision: round to a unit the inputs support.

**The honest defence of a P-value is its basis document, not its confidence level.** When a result is
challenged — usually by someone who dislikes the date — what works is the list of assumptions and what
happens when each is varied. What fails is "the model says so". The model proposes; the professional
disposes.

## 7. How this goes wrong

**Run on a schedule that fails its quality review.** Constraints hold the dates, dangling activities never
move, out-of-sequence progress has corrupted the critical path — and the model produces a smooth, plausible
curve regardless. That plausibility is the danger: nothing in the result announces that the network could not
respond. Run the quality checks first and publish them alongside the distribution.

**Ranges anchored on the deterministic durations.** Plus or minus ten per cent on every activity tells you
only that someone had a spreadsheet. Uniform ranges signal that elicitation did not happen.

**No correlation, because nobody asked.** Independence is most tools' default, is almost never true, and has
a large effect, as §8 shows. A basis document silent on correlation made the assumption without recording it.

**Modelling only the critical path.** Near-critical paths become critical under sampling; that is much of the
point, and a model built on the deterministic path cannot show it.

**Presenting the P80 as the answer.** The confidence level is a choice belonging to whoever carries the
consequence. Present the curve, state what each level costs in time, record the decision. Holding a target
and a commitment at different levels is often right; doing so by accident never is.

**Attributing a threshold to a standard.** The choice of level is organisational policy and appetite. Where a
convention exists — P50 internally with a higher level for external commitment is widespread — describe it as
a convention of the organisation that set it.

**Running it once, at sanction.** The distribution is a snapshot of an assumption set that ages immediately.
Re-run at gates, at re-baseline, and when a major register entry resolves either way.

## 8. Worked example

*Illustrative figures. Durations in working days. Two approximations are used deliberately so the arithmetic
is checkable by hand, and both are named where they occur.*

### 8.1 Part A — a merge, and what the correlation assumption is worth

Four work fronts converge on a commissioning readiness milestone, each assessed as **70 % likely** to be
ready on or before the target date.

**If the four are independent**, the milestone is met only if all four are met:

```
P(all four ready) = 0.70 × 0.70 × 0.70 × 0.70
                  = 0.49 × 0.70 × 0.70
                  = 0.343 × 0.70
                  = 0.2401   →  24.0 %
```

**If the four are perfectly correlated** — one shared driver, say a single commissioning contractor whose
resourcing determines all four fronts — they are ready together or late together:

```
P(all four ready) = 0.70   →  70.0 %
```

The honest answer to "what is the chance we make the milestone?" lies between **24 %** and **70 %**, and
which end it sits nearer is decided entirely by an assumption that appears nowhere on the schedule. No amount
of extra simulation resolves this; only naming the drivers does. Note the direction: at a merge, independence
is the *pessimistic* assumption — the opposite of the chain case in Part B.

### 8.2 Part B — a chain, with and without correlation

A four-activity chain on the critical path, each activity with the same three-point estimate in working days:

```
a (optimistic) = 20      m (most likely) = 30      b (pessimistic) = 70
```

The range is right-skewed, as construction and commissioning ranges typically are.

**Step 1 — the per-activity mean and spread**, using the conventional weighting described in §2:

```
mean = (a + 4m + b) ÷ 6
     = (20 + 4×30 + 70) ÷ 6
     = (20 + 120 + 70) ÷ 6
     = 210 ÷ 6
     = 35.0 days

standard deviation ≈ (b − a) ÷ 6
                   = (70 − 20) ÷ 6
                   = 50 ÷ 6
                   = 8.333 days
```

Note that the **most likely value of 30 days is not the mean of 35 days**: a schedule built on most-likely
durations is already five days optimistic on this activity before any risk event is considered.

**Step 2 — the deterministic chain**:

```
deterministic duration = 4 × 30 = 120 days
```

**Step 3 — the chain mean**:

```
chain mean = 4 × 35.0 = 140.0 days
```

**Step 4 — the chain spread if the four activities are independent.** Variances add:

```
variance per activity = 8.333² = 69.44
chain variance        = 4 × 69.44 = 277.78
chain std deviation   = √277.78 = 16.667 days
```

**Step 5 — the chain spread if the four are perfectly correlated.** Standard deviations add directly:

```
chain std deviation = 4 × 8.333 = 33.333 days
```

**Step 6 — read percentiles.** *Approximation: the sum of the four durations is treated as normally
distributed, deliberately, so the arithmetic can be checked by hand. The underlying activity distributions
are right-skewed and a real simulation samples them directly. The standard normal value for the 80th
percentile is 0.8416.*

Independent case:

```
P50 = 140.0 days
P80 = 140.0 + (0.8416 × 16.667)
    = 140.0 + 14.03
    = 154.0 days
```

Perfectly correlated case:

```
P50 = 140.0 days
P80 = 140.0 + (0.8416 × 33.333)
    = 140.0 + 28.05
    = 168.1 days
```

**Step 7 — how likely is the deterministic date?** Independent case:

```
z = (120 − 140.0) ÷ 16.667 = −20 ÷ 16.667 = −1.20
P(chain ≤ 120 days) = Φ(−1.20) ≈ 0.115  →  about 11.5 %
```

### 8.3 Reading the results

| Measure | Independent | Perfectly correlated |
|---|---:|---:|
| Deterministic duration | 120 days | 120 days |
| Mean / P50 | 140.0 days | 140.0 days |
| P80 | 154.0 days | 168.1 days |
| Chance of achieving the deterministic 120 days | ≈ 11.5 % | ≈ 27.4 % |

*The correlated case's 27.4 % is `Φ((120 − 140) ÷ 33.333) = Φ(−0.60) ≈ 0.274`. Higher spread makes the
optimistic outcome likelier too — a point lost when correlation is described only as "making things worse".*

**The mean is identical in both cases at 140 days.** Correlation moved the P80 by
`168.1 − 154.0 = 14.1 days` and the mean by nothing. Correlation is a statement about the tail.

**The deterministic date is not a forecast.** At 120 days it has roughly an 11.5 per cent chance in the
independent model — not a criticism of the planner, but the consequence of building a chain from most-likely
values when the ranges are skewed. The 20 days from 120 to the P50 is pure estimating asymmetry.

**The gap to a funded P80 is large.** In the independent case `154.0 − 120 = 34.0 days`, or
`34 ÷ 120 = 28.3 %` of the deterministic duration — a decision to be taken with that number in front of it.

### 8.4 What this example excludes, and why that matters

The exclusions are as instructive as the arithmetic:

- **One chain only** — merge bias is excluded entirely, and Part A shows how large that effect can be.
- **No loaded risk events** — register entries would add discrete jumps and typically lengthen the upper tail
  more than the body.
- **Identical activities**, so the independent-case spread simplifies to a clean multiple. Real chains do
  not.
- **Normality assumed at the sum** (step 6). A right-skewed input generally puts the true P80 slightly later
  than the approximation suggests, so 154.0 is a floor rather than a precise value.
- **No calendars, resource constraints or weather windows**, all of which a production model carries and all
  of which typically extend the result.

Reporting exclusions is not a disclaimer. It is the difference between a number a board can act on and one a
board will regret.

## 9. Checklist

**Before the model is built**

- [ ] Schedule quality checks run and recorded — logic, constraints, negative float, lags, out-of-sequence
      progress; constraint and dangle counts published with the basis.
- [ ] Model level of detail agreed and reconciled to the working schedule's critical path if different.
- [ ] Register current, cause-event-effect compliant, mapped to activities.
- [ ] Shared drivers listed by name, with the activities each one touches.
- [ ] Elicitation instruction written: what the ranges include and exclude.
- [ ] Basis document opened, not left to be written afterwards.

**During elicitation**

- [ ] Pessimistic value asked first, with a mechanism, from the people doing the work.
- [ ] Whether the estimator saw the scheduled duration recorded per activity.
- [ ] One definition of optimistic and pessimistic applied by everyone.
- [ ] Ranges sense-checked against outturn data where it exists.

**Before the output is issued**

- [ ] Deterministic date, P50 and P80 all reported, with the gaps explained.
- [ ] The full curve issued, not two points off it.
- [ ] Correlation assumptions stated, and the answer re-run with them varied.
- [ ] Sensitivity ranking published — which activities and register entries drive the tail.
- [ ] No entry both loaded and embedded in a duration range.
- [ ] Output rounded to a unit the inputs support; exclusions listed explicitly.
- [ ] The confidence level chosen for commitment recorded as a decision, with its owner.

A QSRA that passes this list will sometimes produce a date the project cannot accept. That is the analysis
working. The alternative is a date the project accepts and cannot achieve, discovered when nothing can be
done about it.

---

## Related

- `BPG-05 — Schedule quality — a practical review` — whether the network can be simulated at all
- `BPG-10 — Contingency and management reserve` — how a confidence level becomes funded time and money
- `BPG-16 — Risk registers that work` — the register discipline the loaded risk events depend on
- `TPL-11 — Quantitative schedule risk analysis input sheet` — field structure for elicitation and loading
- `TPL-14 — Schedule quality review checklist` — the precondition checks in usable form
- `AIG-05 — AI in scheduling — and what must not be automated` — where machine assistance helps here

## Sources and standards

- PCL-AI Body of Knowledge (`docs/bok/`), Domain 10 — Project Scheduling, first authored draft, August 2026:
  schedule risk analysis, merge bias, health checks and the target-versus-commitment posture.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 12 — Risk Management for Project Controls, first authored
  draft, August 2026: risk loading, correlation and shared drivers, register-to-model traceability.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  naming, status and claims policy.

The weighting `(a + 4m + b) ÷ 6` and the spread approximation `(b − a) ÷ 6` used in §8 are conventions from
the programme evaluation and review technique tradition, used here because they are hand-checkable; no
published standard is cited as their source, and no confidence level named in this guide is attributed to
one. No industry statistic, benchmark range or software capability is cited, because none was verified.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
