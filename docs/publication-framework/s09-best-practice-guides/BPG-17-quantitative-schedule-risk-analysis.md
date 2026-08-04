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
  to read a P50 and a P80 as confidence in a model rather than in the world, and why running the analysis
  on a schedule that fails a quality review produces a distribution of the constraint set. Every figure in
  the worked example is computed by hand and checkable.
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
constraint set. Every figure in the worked example is computed by hand and checkable.

**Who this is for.** Planners and schedulers who run or commission the analysis; project controls managers
who have to defend its output; and project and programme managers who are asked to commit to a date that
came out of it.

---

## 1. What the analysis actually does

Quantitative schedule risk analysis — **QSRA** — takes a logic-driven schedule, replaces fixed activity
durations with ranges, adds discrete risk events drawn from the risk register, samples the whole network
many thousands of times, and reports the distribution of the results. Its output is a set of completion
dates with probabilities attached: the **P50**, the date the model finishes on or before in half of the
runs; the **P80**, the date it finishes on or before in eighty per cent of them; and the curve joining them.

Three things it genuinely does, that a deterministic schedule cannot:

**It exposes the optimism in the merge.** A deterministic forward pass takes the latest arrival at each
convergence point. That is arithmetically correct and probabilistically optimistic, because the milestone is
only met if *every* incoming path is met. Simulation captures this automatically, because it samples all
paths together.

**It ranks the drivers.** The output that changes management behaviour is rarely the date; it is the
sensitivity ranking that says which activities and which register entries are doing the work in the tail.
That ranking is a mitigation shopping list, priced.

**It separates commitment from target.** Running the model gives a project the language to hold an internal
target and an external commitment at different confidence levels, with the gap between them named, owned and
visible rather than smeared invisibly across activity durations.

Three things it does not do, which are the source of most of the trouble:

**It does not tell you what the project will do.** It tells you what the model does. The distinction is the
subject of §6 and is not a technicality.

**It does not find risks.** It quantifies the ones on the register and the ranges people were willing to
state. Whatever the workshop did not surface is absent from the distribution, and its absence is invisible in
the output.

**It does not repair a schedule.** A network that cannot recalculate honestly cannot be simulated honestly.
§7 covers this, and it is the most common reason a QSRA result should be discarded rather than argued with.

## 2. Three-point estimates and the elicitation problem

The input for duration uncertainty is a range per activity: an optimistic value, a most likely value and a
pessimistic value, usually written as *a*, *m* and *b*. Getting these numbers is an interviewing problem
before it is a modelling problem, and the interviewing is where most QSRA quality is won or lost.

The dominant failure is **anchoring**. The estimator is shown the duration already in the schedule and asked
for a range around it. The number in the schedule becomes the anchor, the range comes back as roughly plus
or minus ten or fifteen per cent, and the model inherits the deterministic schedule's optimism with a
decorative band around it. If the schedule's durations were set by working backwards from a required
completion date — which is common and rarely admitted — the anchoring imports that too.

Practical countermeasures, in the order they matter:

**Ask for the pessimistic value first, and ask for it as a story.** "What would have to happen for this to
take twelve weeks?" produces a different answer from "what is your worst case?", because the first requires
a mechanism and the second invites a number the estimator feels comfortable defending. If no mechanism can be
described, the pessimistic value is probably too low, not too high.

**Elicit from the person who does the work, not the person accountable for the date.** Accountability
narrows ranges. It is not dishonesty; a manager who has committed to a date will genuinely find the
pessimistic scenario less plausible than the crew supervisor does.

**Do not show the schedule duration until after the range is given.** Where that is impractical, at least
record whether it was shown, so the reviewer knows which ranges to distrust.

**Define what the pessimistic value means and hold everyone to the same definition.** A pessimistic value
that means "the worst I have personally seen" and one that means "the worst credible outcome short of a
force-majeure event" are different quantities, and mixing them across a network produces a distribution that
means nothing in particular.

**Calibrate against outturn.** The most effective correction is showing estimators how their previous ranges
compared with what actually happened. Where the organisation captures that data at closeout, it is the
single highest-value input to the next model. `BPG-20 — Closeout, lessons learned and benchmarking` covers
capturing it.

Two conventions from the programme evaluation and review technique tradition are commonly used to summarise
a three-point range. The weighted mean `(a + 4m + b) ÷ 6` and the spread approximation `(b − a) ÷ 6` are
long-standing conventions, not requirements of any standard, and they are approximations to a particular
family of distributions. They are used in §8 because they can be checked by hand. A real simulation samples
from the chosen distribution directly and does not need them.

## 3. Duration uncertainty and risk events are different inputs

Two distinct sources of schedule uncertainty are frequently conflated, and conflating them either
double-counts or omits.

**Duration uncertainty** is the variability in how long a piece of work takes when it goes ahead as planned:
productivity varies, weather varies, the crew is better or worse than assumed. It applies to every activity,
it is modelled by the three-point range, and it does not have a probability of occurrence — the work happens,
the duration varies.

**Risk events** are discrete occurrences from the register that may or may not happen: a permit is refused, a
vendor fails, contaminated ground is found. They have a probability of occurrence *and* an impact range, they
strike specific activities, and they are modelled by mapping register entries onto the activities they would
hit. This mapping is the **risk-driver** approach, and its virtue is that the model inherits the register
rather than running on a parallel set of assumptions.

The double-count happens when an estimator, asked for a pessimistic duration, includes the effect of a
register entry — "twelve weeks if the permit is late" — and the modeller then also loads the permit risk onto
the same activity. The exposure is counted twice, the tail is overstated, and the resulting contingency
request will not survive scrutiny by anyone who traces it.

The omission happens in the other direction: the range is elicited as "how long if nothing goes wrong",
nothing is loaded from the register, and the model contains no discrete risk at all. The distribution is then
narrow, the P80 sits close to the P50, and the analysis is reported as showing the schedule is robust.

The discipline that prevents both is a single instruction, given at elicitation and recorded on the input
sheet: *give the range for this activity assuming the identified risks do not occur; the register is loaded
separately.* `TPL-11 — Quantitative schedule risk analysis input sheet` carries the field structure this
implies, and `BPG-16 — Risk registers that work` covers the register discipline the loading depends on.

## 4. Correlation is the assumption that decides the answer

Simulation software will happily sample every activity independently. Real projects do not behave that way.
Activities share crews, share a design authority, share a weather season, share a fabrication market, share
a client's approval process. When a shared driver moves, it moves everything it touches in the same
direction at the same time.

The effect on the output is specific and worth memorising, because it is the opposite of most people's
intuition:

**Correlation does not move the mean. It fattens the tail.** The expected value of a sum equals the sum of
the expected values whether the components are correlated or not. What changes is the spread. Independent
components partly cancel — some run long while others run short — and that cancellation is exactly what
correlation removes. The P50 barely moves; the P80 and P90 move a lot.

Two structural cases matter.

**Along a chain**, correlation increases the variability of the total. Four independent activities of equal
spread produce a chain whose spread is twice one activity's, because standard deviations add in quadrature —
the square root of four is two. The same four activities perfectly correlated produce a chain whose spread is
four times one activity's, because they now move together. Section 8 computes both.

**At a merge**, correlation *reduces* the penalty. If several converging paths are driven by one shared
cause, they tend to be ready together or late together, and the milestone's probability approaches the
probability of a single path. Independent paths multiply their probabilities and the milestone becomes far
less likely than any of them. Section 8 computes this too, and the gap between the two answers is large
enough to change a commitment.

Because the two effects run in opposite directions, "assume some correlation to be safe" is not a
conservative default; it is conservative on chains and optimistic at merges. There is no substitute for
naming the drivers.

The practical method needs no mathematics. Before the model is built, list the shared drivers explicitly —
one weather season, one commissioning contractor, one approval authority, one specialist crew — and record
which activities each one touches. That list, not a correlation coefficient chosen by feel, is what should
be entered and what should appear in the model's basis document. Where a coefficient is required by the tool,
record where the number came from and treat it as an assumption to be tested by sensitivity, not as data.

## 5. What the analysis needs before it is worth running

QSRA amplifies whatever is underneath it. Five preconditions, each of which can be checked in an afternoon:

**The schedule recalculates.** Every activity except the first and last has a predecessor and a successor.
Date constraints are rare and individually justified. There is no negative float, or it is understood and
explained. Long lags are exposed and defended. Progress is not recorded out of sequence. These are the
standard schedule quality checks, and `BPG-05 — Schedule quality — a practical review` and
`TPL-14 — Schedule quality review checklist` own them.

**The critical path is the work, not the constraints.** A network held together by imposed dates does not
respond to sampled durations, because the constraints hold the dates regardless. Sampling such a network
produces a distribution of the constraint set. This is the most consequential precondition and the one most
often skipped, because a heavily constrained schedule still produces a perfectly convincing output curve.

**The level of detail suits simulation.** A network of many thousands of short activities produces spurious
precision and takes an elicitation effort nobody will complete honestly; a network of thirty summary bars
hides the merges that drive the answer. Where the working schedule is too detailed, build a separate risk
model at an appropriate level and reconcile its critical path to the working schedule's.

**The register is current and loadable.** Entries are stated as cause, event and effect, are scored, and can
be mapped onto activities.

**Someone owns the basis document.** The model's assumptions — distribution shapes, correlation drivers,
what the ranges include and exclude, what was not modelled — are written down before the run, not
reconstructed afterwards. A QSRA without a basis document cannot be reviewed, only believed.

## 6. Reading a P-value honestly

A P80 completion date is a statement about a model. Said in full: *in eighty per cent of the simulated runs
of this network, with these ranges, these loaded risks and these correlation assumptions, completion occurred
on or before this date.* Every clause in that sentence is load-bearing.

What follows from it:

**A P80 is not an eighty per cent chance of finishing by that date.** It is an eighty per cent chance
*within the model's world*. The model's world excludes every risk not identified, every range not honestly
given, every correlation not declared and every structural failure the schedule already contains. Each of
those omissions moves the real probability down, and none of them moves it up.

**The gap between the deterministic date and the P50 is diagnostic.** If the deterministic date sits far
below the P50, the schedule's durations are optimistic, the network merges heavily, or both — and that is a
finding about the schedule, not about risk. It should be reported as such before any conversation about
contingency.

**The shape matters more than the point.** A curve that is nearly vertical between P50 and P80 says the
outcome is insensitive to confidence level and the date is fairly well determined. A curve with a long
shallow tail says a small increase in required confidence costs a great deal of time, which is a different
management conversation entirely. Report the curve, not two numbers off it.

**Precision is not accuracy.** A model that reports a P80 to the day, built on ranges elicited to the
nearest fortnight, is presenting false precision. Round the output to a unit the inputs can support, and say
why.

**The honest defence of a P-value is its basis document, not its confidence level.** When a QSRA result is
challenged — and it will be, usually by someone who does not like the date — the answer that works is the
list of assumptions and what happens to the answer when each one is varied. The answer that fails is "the
model says so".

The Institute's position on the wider question is the same as everywhere else in this library: the model
proposes, the professional disposes. A distribution is an input to a judgement about what to commit to, what
to fund and what to mitigate. It is not the judgement.

## 7. How this goes wrong

**Run on a schedule that fails its quality review.** Constraints hold the dates, dangling activities never
move, out-of-sequence progress has corrupted the current critical path — and the model produces a smooth,
plausible S-curve regardless. The output's plausibility is the danger; nothing in the result announces that
the network could not respond. Run the quality checks first and publish their result alongside the
distribution.

**Ranges anchored on the deterministic durations.** Plus or minus ten per cent on every activity, applied
uniformly, tells you only that someone had a spreadsheet. Uniform ranges are a signal that elicitation did
not happen.

**No correlation, because nobody asked.** The default in most tools is independence. Independence is almost
never true and its effect on the answer is large, as §8 shows. A model whose basis document does not mention
correlation has made the assumption without recording it.

**Duration ranges that already contain the register.** Double-counting, covered in §3. The tell is a
distribution whose tail is dominated by activities rather than by risk events, on a project whose register
is full of high-impact entries.

**Modelling only the critical path.** Near-critical paths become critical under sampling; that is much of the
point. A model built on the deterministic critical path alone cannot show it.

**Presenting the P80 as the answer.** The P80 is a choice about confidence, and the choice belongs to the
person who carries the consequence, not to the analyst. Present the curve, state what each level costs in
time, and let the decision be made and recorded.

**Committing to the P50 and reporting the P80 internally, or the reverse.** Both happen, both are
defensible in the right circumstances, and both are indefensible if not written down. What is never
defensible is a project whose external commitment and internal target are at different confidence levels
without anyone having decided that they should be.

**Attributing a threshold to a standard.** The choice of P50, P80 or any other level is a matter of
organisational policy and risk appetite. Where a project convention exists — and P50 for internal targets
with a higher level for external commitment is a widespread convention — it should be described as a
convention and attributed to the organisation that set it, not to a published standard.

**Running it once, at sanction.** The distribution is a snapshot of an assumption set that starts ageing
immediately. Re-run at gates, at re-baseline, and when a major register entry resolves either way.

## 8. Worked example

*Illustrative figures. Durations in working days. Probabilities are stated to three decimal places where
exact and rounded to one decimal place as percentages. Two approximations are used deliberately so the
arithmetic is checkable by hand, and both are named where they occur.*

### 8.1 Part A — a merge, and what the correlation assumption is worth

Four work fronts converge on a commissioning readiness milestone. Each front is independently assessed as
**70 % likely** to be ready on or before the target date.

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

The honest answer to "what is the chance we make the milestone?" is somewhere between **24 %** and **70 %**,
and which end it sits nearer is determined entirely by an assumption that appears nowhere on the schedule.
No amount of additional simulation resolves this; only naming the drivers does.

Note the direction. At a merge, independence is the *pessimistic* assumption. This is the opposite of the
chain case in Part B, which is why a blanket "assume correlation to be safe" is not safe.

### 8.2 Part B — a chain, with and without correlation

A four-activity chain on the critical path. Each activity has the same three-point estimate, in working days:

```
a (optimistic) = 20      m (most likely) = 30      b (pessimistic) = 70
```

The range is right-skewed — the downside is much longer than the upside — which is typical of construction
and commissioning work.

**Step 1 — the per-activity mean and spread**, using the conventional programme-evaluation weighting
described in §2:

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

Note immediately that the **most likely value of 30 days is not the mean of 35 days**. The deterministic
schedule, built on most-likely durations, is already five days optimistic on this activity before any risk
event is considered.

**Step 2 — the deterministic chain**:

```
deterministic duration = 4 × 30 = 120 days
```

**Step 3 — the chain mean**:

```
chain mean = 4 × 35.0 = 140.0 days
```

**Step 4 — the chain spread, assuming the four activities are independent.** Variances add:

```
variance per activity = 8.333² = 69.44
chain variance        = 4 × 69.44 = 277.78
chain std deviation   = √277.78 = 16.667 days

(equivalently: 8.333 × √4 = 8.333 × 2 = 16.667)
```

**Step 5 — the chain spread, assuming the four are perfectly correlated.** Standard deviations add directly:

```
chain std deviation = 4 × 8.333 = 33.333 days
```

**Step 6 — read percentiles.** *Approximation used here: the sum of the four activity durations is treated
as normally distributed. This is a deliberate simplification so the arithmetic can be checked by hand; the
underlying activity distributions are right-skewed, and a real simulation samples them directly rather than
assuming normality. The standard normal value for the 80th percentile is 0.8416.*

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

**Step 7 — how likely is the deterministic date?** In the independent case:

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
optimistic outcome more likely as well as the pessimistic one — a point often lost when correlation is
described only as "making things worse".*

Three readings follow.

**The mean is identical in both cases at 140 days.** Correlation moved the P80 by
`168.1 − 154.0 = 14.1 days` and moved the mean by nothing. This is the property to carry into every
conversation about a risk model: correlation is a statement about the tail.

**The deterministic date is not a forecast.** At 120 days it has roughly an 11.5 per cent chance in the
independent model. That figure is not a criticism of the planner; it is the arithmetic consequence of
building a chain from most-likely values when the ranges are skewed. The gap from 120 to the P50 of 140 is
20 days of pure estimating asymmetry, before any risk event is loaded.

**The gap between the deterministic date and a funded P80 is large.** In the independent case,
`154.0 − 120 = 34.0 days`, which is `34 ÷ 120 = 28.3 %` of the deterministic duration. Whether the project
holds that as schedule contingency, absorbs it, or re-plans the work is a decision — but it should be a
decision taken with the number in front of it.

### 8.4 What this example excludes, and why that matters

The result above is a teaching illustration and its exclusions are as instructive as its arithmetic:

- **One chain only.** Merge bias is excluded entirely, and Part A shows how large that effect can be. A real
  network with several converging paths would push the percentiles later still.
- **No loaded risk events.** Only duration uncertainty is modelled. Register entries would add discrete
  jumps, and typically lengthen the upper tail more than the body.
- **Identical activities.** Real chains have different ranges, so the independent-case spread would not
  simplify to a clean multiple.
- **Normality assumed at the sum.** Stated in step 6. The direction of the error is that a right-skewed
  input distribution generally puts the true P80 slightly later than the normal approximation suggests, so
  the 154.0 figure should be treated as a floor rather than a precise value.
- **No calendars, no resource constraints, no weather windows.** All of which a production model would carry
  and all of which typically extend rather than shorten the result.

Reporting those exclusions is not a disclaimer. It is the difference between a number a board can act on and
a number a board will regret.

## 9. Checklist

**Before the model is built**

- [ ] Schedule quality checks run and their result recorded — logic, constraints, negative float, lags,
      out-of-sequence progress.
- [ ] Constraint count and dangle count published alongside the model's basis.
- [ ] Model level of detail agreed, and reconciled to the working schedule's critical path if different.
- [ ] Risk register current, cause-event-effect compliant, and mapped to activities.
- [ ] Shared drivers listed by name, with the activities each one touches.
- [ ] Elicitation instruction agreed and written: what the ranges include and exclude.
- [ ] Basis document opened, not left to be written afterwards.

**During elicitation**

- [ ] Pessimistic value asked for first, and asked for with a mechanism.
- [ ] Ranges taken from the people doing the work.
- [ ] Whether the estimator saw the scheduled duration is recorded per activity.
- [ ] The same definition of optimistic and pessimistic applied by everyone.
- [ ] Ranges sense-checked against outturn data from previous projects where it exists.

**Before the output is issued**

- [ ] Deterministic date, P50 and P80 all reported, with the gaps explained.
- [ ] The full curve issued, not two points off it.
- [ ] Correlation assumptions stated, and the answer re-run with them varied.
- [ ] Sensitivity ranking published — which activities and which register entries drive the tail.
- [ ] Double-counting checked: no register entry both loaded and embedded in a duration range.
- [ ] Output rounded to a unit the inputs can support.
- [ ] Exclusions listed explicitly, including anything not modelled.
- [ ] The confidence level chosen for commitment is recorded as a decision, with its owner.

A QSRA that passes this list will sometimes produce a date the project cannot accept. That is the analysis
working: the alternative is a schedule that produces a date the project accepts and cannot achieve, and the
difference between the two is discovered at exactly the point when nothing can be done about it.

---

## Related

- `BPG-05 — Schedule quality — a practical review` — the checks that determine whether the network can be simulated at all
- `BPG-10 — Contingency and management reserve` — how a confidence level becomes funded time and money, and who may draw it
- `BPG-16 — Risk registers that work` — the register discipline the loaded risk events depend on
- `TPL-11 — Quantitative schedule risk analysis input sheet` — the field structure for elicitation and loading
- `TPL-14 — Schedule quality review checklist` — the precondition checks in usable form
- `AIG-05 — AI in scheduling — and what must not be automated` — where machine assistance helps in this workflow and where judgement must stay

## Sources and standards

- PCL-AI Body of Knowledge (`docs/bok/`), Domain 10 — Project Scheduling, first authored draft, August 2026:
  schedule risk analysis, merge bias, schedule health checks, buffers and the target-versus-commitment
  posture.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 12 — Risk Management for Project Controls, first authored
  draft, August 2026: risk loading, correlation and shared drivers, the relationship between a register and a
  quantified model.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  naming, status and claims policy.

The three-point weighting `(a + 4m + b) ÷ 6` and the spread approximation `(b − a) ÷ 6` used in §8 are
long-standing conventions from the programme evaluation and review technique tradition. They are used here
because they are hand-checkable, not because any standard requires them, and no published standard is cited
as their source. No confidence level named in this guide — P50, P80 or otherwise — is attributed to a
published standard; where a level is described as common practice it is described as a convention set by
organisations, which is what it is. No industry statistics, benchmark ranges or software capabilities are
cited, because none were verified for this guide.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
