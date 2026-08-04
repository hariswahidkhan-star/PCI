---
id: CMP-07
series: S03
series_name: Competency Frameworks
title: Planning, scheduling and risk competencies in depth
subtitle: What separates building a schedule from owning one, and maintaining a register from analysing risk
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, employer]
level: professional
reading_time_min: 15
summary: >
  Planning and risk are the two competencies most often evidenced by artefacts that prove nothing — a
  large schedule and a tidy register. This document sets out what distinguishes practitioner from
  professional in each: the four questions a schedule owner must be able to answer, why deterministic
  dates are systematically optimistic when paths merge, and why an expected-value contingency is exceeded
  far more often than people expect. Worked figures are included and independently recomputed.
linkedin:
  format: article
  hook: >
    Three parallel paths, each with a 50 % chance of hitting its date. Assume they are independent and the
    milestone they feed has a 12.5 % chance — which is why deterministic schedules are optimistic by
    construction, not by carelessness.
  tags: [Scheduling, RiskManagement, ProjectControls, Competency, QSRA]
  asset: carousel-8
gated: false
related: [CMP-03, CMP-05, BPG-05, BPG-10, BPG-17, TPL-11]
bok_domains: [6, 8, 9, 10, 12]
sources:
  - PCI platform certification catalogue — seeded competency sets for PCL-AI, PFL-AI and PML-AI, backend/Data/MultiCert.cs, verified August 2026
placeholders: 0
---

# Planning, scheduling and risk competencies in depth

> What separates building a schedule from owning one, and maintaining a register from analysing risk.

**In one paragraph.** Planning and risk are the two competencies most often evidenced by artefacts that
prove nothing — a large schedule and a tidy register. This document sets out what actually distinguishes a
practitioner from a professional in each: the four questions a schedule owner must be able to answer, the
structural reason deterministic dates are optimistic when paths merge, and why an expected-value
contingency is exceeded far more often than the arithmetic seems to suggest. The figures are illustrative
and every one is recomputed from first principles.

**Who this is for.** Planners, planning managers, risk managers and project controls leads; project
managers who commission schedule and risk analysis and have to decide whether to believe it; and assessors
reviewing evidence in this cluster.

---

## 1. What is in the cluster

Across the three PCI credentials this cluster comprises planning and scheduling, project risk and
performance measurement (PCL-AI); planning and execution, cost, schedule and risk integration, predictive
delivery, agile delivery and hybrid delivery (PML-AI); and commercial and financial risk in its
construction-period aspect (PFL-AI). Definitions and level descriptors are in *CMP-03*, *CMP-04* and
*CMP-05*; this document goes underneath them.

Acronyms used here: critical path method (CPM), quantitative schedule risk analysis (QSRA), programme
evaluation and review technique (PERT), extension of time (EOT).

## 2. The four questions that separate a schedule owner from a schedule builder

Building a network is a skill that a capable person acquires in months. Owning a schedule is a different
competency, and it is testable in a fifteen-minute conversation with four questions.

**"What is driving the completion date, and why?"** The competent answer names activities, states the
float position of the next-nearest paths, and explains what makes this path critical rather than merely
long. The incompetent answer names a software view. The follow-up that finds the truth is: *what is the
second critical path, and how much float separates them?* A path with three days of float is not a
comfort; it is a second critical path waiting for a delivery to slip.

**"Which of these dates are logic-driven and which are constrained?"** Every hard constraint in a
schedule is a decision that overrides logic, and each one hides float, distorts the critical path, and
converts a plan into a target. A professional can list the constraints in their schedule, justify each,
and say what the schedule would show without them. Where a constraint is really an instruction — a date
the client requires — the professional says so rather than allowing the network to imply it is achievable.

**"What is the difference between this plan and the target?"** A plan is what will happen if things go as
assumed; a target is what someone requires. They are frequently the same document, which is how a project
spends a year reporting to a plan nobody believes. The competency is the willingness to maintain and show
both, and to state the gap as a number.

**"What would change this date?"** The answer should be specific and short: a permit, a long-lead
delivery, an approval, a labour ramp. If the answer is a general statement about productivity, the schedule
is not being used to manage anything.

## 3. Logic quality: the checkable part

Schedule quality has an objective component, and it is the part an assessor can verify without knowing the
project. *BPG-05 — Schedule quality: a practical review* owns the full review; the competency-relevant
signals are:

- **Open ends.** Activities without a predecessor or successor float free of the network and cannot drive
  or be driven. A schedule with many of them is a list, not a model.
- **Excessive lags, especially negative ones.** A lag encodes a duration nobody has named or resourced.
  Negative lags create logical impossibilities that the software will nonetheless calculate.
- **Constraint density.** Counted, categorised and justified — or unexamined.
- **Negative float.** Its presence means the schedule is asserting the impossible. The competent response
  is to fix the plan or state the impossibility, not to reduce durations until the number disappears.
- **Duration distribution.** A network in which most activities are exactly 5, 10 or 20 days has been
  estimated by convention.
- **Resource credibility.** A peak labour requirement that the site cannot physically accommodate makes
  every date after it fictional.

A candidate whose evidence includes a schedule quality review they performed on someone else's programme,
with findings and the argument they had about them, is demonstrating this competency directly.

## 4. Estimating durations: the assumption inside the number

Duration estimates are usually presented as single values, which conceals the assumption that produced
them. Where a three-point estimate is used, the arithmetic is trivial and the assumption is not: the
formula chosen expresses a belief about the shape of the distribution, and different reasonable formulae
give different answers from the same three inputs. §8 shows the arithmetic and the size of the difference.

Two competency markers follow. First, the professional states which formula was used and why, because the
choice moves the answer. Second, the professional knows that a three-point estimate on each activity does
**not** produce a three-point view of the project, because durations combine — which is the subject of the
next section.

## 5. Merge bias: why deterministic schedules are optimistic by construction

When several independent paths feed one milestone, the milestone waits for the slowest of them. Even if
every path is equally likely to be early or late, the probability that *all* of them arrive on time is the
product of their individual probabilities, and that product falls fast. §8 works the numbers: three paths,
each with a 50 % chance of meeting its date, give the milestone a 12.5 % chance if the paths are
independent.

Three consequences a professional acts on:

1. **Deterministic completion dates at merge points are not "the expected date"; they are an optimistic
   case.** This is arithmetic, not pessimism, and it is why quantitative schedule risk analysis exists.
2. **Correlation matters enormously and is the analyst's hardest judgement.** If the paths share a cause —
   the same labour pool, the same weather window, the same approving authority — they move together, and
   the merge penalty shrinks toward nothing. A QSRA run with everything independent overstates the
   penalty; a QSRA run with correlation assumed away because the software defaults that way is
   unexamined. Stating the correlation assumption is a hallmark of professional-level competence.
3. **Adding parallel paths into a milestone increases risk even when each path is individually
   comfortable.** Recovery plans that parallelise work are trading one risk for another, and the trade
   should be stated.

## 6. Risk competence: register, analysis, ownership, contingency

The register is the artefact and the least informative part. Four things distinguish the levels.

**Whether the register contains the risks that matter.** The most consequential risks on most projects are
the ones with a political cost to writing down: the approval that will not arrive, the client decision
that has been outstanding for four months, the design that is not as complete as reported. A register full
of weather, labour availability and supply chain generalities has been populated, not compiled.

**Whether ownership is real.** A risk owned by "the project" is unowned. A risk owned by someone with no
authority over its cause is misallocated. The test is whether the owner can name the action they are
taking and the date it completes.

**Whether the analysis says anything that changes a decision.** Probability-times-impact scoring on a
five-by-five grid produces a heat map; it does not produce a contingency, and it cannot be aggregated
meaningfully because ordinal scores are not numbers. Quantitative analysis, done properly, answers a
question a decision-maker has: how much contingency, for what confidence, and against what.

**Whether contingency is connected to the analysis and drawn down as risk retires.** Contingency set by
percentage rule of thumb and never reduced is a budget line, not a risk response. A professional can show
the link: this contingency, at this confidence level, against these risks; and as this risk expires, this
much is released.

## 7. How this goes wrong

**The schedule is maintained but not owned.** Updates are applied, the critical path moves, and nobody
narrates it. Six months later the completion date has drifted by seven weeks in increments that were never
individually significant enough to escalate.

**Constraints replace logic.** A required date is entered as a hard constraint, the network then reports
achievability, and the project reports green until the constrained activity's predecessors physically run
out of time. The constraint said what the client wanted; the schedule was never asked what was possible.

**Progress updating is confused with re-planning.** Applying actual dates and remaining durations updates
the record. If the logic that was assumed is no longer how the work will be done, the update produces a
precise forecast from an obsolete model.

**Duration compression by instruction.** Durations are reduced until the completion date fits, without a
change to method, resource or scope. Every subsequent forecast inherits the fiction, and the schedule stops
being usable as evidence in any later EOT discussion — which is a commercial loss as well as a control one.

**The register is audited for completeness rather than content.** All fields populated, all dates current,
all owners named — and the three risks that will actually decide the outcome are absent because writing
them down would have been awkward.

**Quantitative analysis is run with default assumptions.** Distributions left at the tool's default,
correlation left at zero, and the resulting P80 number reported to two decimal places. The output is
precise, unexamined and worse than a considered judgement, because it carries unearned authority.

**Expected value is treated as a budget.** The expected value of a risk set is a long-run average across
many repetitions of the same project. There is only one project. §8 shows a case where the expected value
is 310,000 cost units, the chance of exceeding it is 37 %, and 80 % confidence needs 550,000.

**Contingency is held at the top and never released.** Nobody can say what it is for, so nobody can say
when it is no longer needed, so it is spent at the end. Tie contingency to named risks at the start and
the release becomes a decision rather than an accident.

**Agile delivery is assessed with predictive evidence, or the reverse.** Empirical throughput data is the
evidence for agile competence; baseline integrity and change control are the evidence for predictive
competence. Neither substitutes for the other, and *CMP-05* §3.6–3.8 sets out the boundary.

## 8. Worked example

*Illustrative figures.* Three short calculations, each independently recomputed. Durations in working
days; costs in currency-neutral cost units (cu). Probabilities are stated as decimals and assumed
independent where said; the independence assumption is itself discussed below.

### 8.1 A three-point duration estimate

An activity is estimated at optimistic 20 days, most likely 30 days, pessimistic 55 days.

Beta (PERT) approximation:
`te = (o + 4m + p) ÷ 6 = (20 + (4 × 30) + 55) ÷ 6 = (20 + 120 + 55) ÷ 6 = 195 ÷ 6 = 32.5 days`

Triangular mean:
`te = (o + m + p) ÷ 3 = (20 + 30 + 55) ÷ 3 = 105 ÷ 3 = 35.0 days`

Standard deviation, beta approximation:
`σ = (p − o) ÷ 6 = (55 − 20) ÷ 6 = 35 ÷ 6 = 5.83 days`

The two means differ by **2.5 days on a 30-day activity** — 8.3 % of the most likely duration — purely
because of the distribution assumed. Neither is more correct in the abstract; the beta form gives the most
likely value more weight, which suits work whose upside is limited and whose downside has a long tail.
*The competency is stating which was used and why, not preferring one.*

### 8.2 Merge bias at a milestone

Three paths feed one milestone. Each path's deterministic date is its median: a 50 % chance of arriving on
or before it.

If the paths are independent:
`P(all three on time) = 0.50 × 0.50 × 0.50 = 0.125` → **12.5 %**

Two paths: `0.50 × 0.50 = 0.25` → 25 %. Four paths: `0.50⁴ = 0.0625` → 6.25 %.

If instead the paths are perfectly correlated — they share a single dominant cause — they all arrive
together and the probability returns to 50 %. Real projects lie between these bounds, so the honest
statement is: **the milestone's probability is between 12.5 % and 50 %, and where it sits depends on a
correlation judgement that must be made explicitly.** Assumptions: each path's distribution is symmetric
about its deterministic date, and no path can recover another's delay.

### 8.3 Expected value is not a contingency

Three cost risks, with single-point impacts, assumed independent:

| Risk | Probability | Impact (cu) | Expected value (cu) |
|---|---|---|---|
| A | 0.30 | 400,000 | 0.30 × 400,000 = 120,000 |
| B | 0.60 | 150,000 | 0.60 × 150,000 = 90,000 |
| C | 0.10 | 1,000,000 | 0.10 × 1,000,000 = 100,000 |
| **Total** | | | **310,000** |

Total expected value: `120,000 + 90,000 + 100,000 = 310,000 cu`.

But no outcome is 310,000. The eight possible outcomes and their probabilities:

| Outcome | Cost (cu) | Probability | Working |
|---|---|---|---|
| None occurs | 0 | 0.252 | 0.70 × 0.40 × 0.90 |
| B only | 150,000 | 0.378 | 0.70 × 0.60 × 0.90 |
| A only | 400,000 | 0.108 | 0.30 × 0.40 × 0.90 |
| A and B | 550,000 | 0.162 | 0.30 × 0.60 × 0.90 |
| C only | 1,000,000 | 0.028 | 0.70 × 0.40 × 0.10 |
| B and C | 1,150,000 | 0.042 | 0.70 × 0.60 × 0.10 |
| A and C | 1,400,000 | 0.012 | 0.30 × 0.40 × 0.10 |
| A, B and C | 1,550,000 | 0.018 | 0.30 × 0.60 × 0.10 |

The probabilities sum to 1.000 (`0.252 + 0.378 + 0.108 + 0.162 + 0.028 + 0.042 + 0.012 + 0.018 = 1.000`),
which is the check that the enumeration is complete.

Three readings a decision-maker needs:

- **Probability of exceeding the expected value of 310,000:** every outcome above it —
  `0.108 + 0.162 + 0.028 + 0.042 + 0.012 + 0.018 = 0.370` → **37.0 %**. A contingency set at the expected
  value is exceeded more than one time in three.
- **Probability of spending nothing:** 25.2 %. The single most likely outcome is *B only* at 150,000
  (37.8 %) — neither of which is anywhere near the average.
- **Contingency for 80 % confidence:** accumulating the sorted outcomes gives 0.252 at 0 cu, 0.630 at
  150,000, 0.738 at 400,000 and **0.900 at 550,000**. The first level reaching at least 80 % confidence is
  therefore **550,000 cu** — `550,000 ÷ 310,000 = 1.77` times the expected value.

Assumptions this depends on: three risks only; independence; single-point impacts rather than ranges; no
correlation between A, B and C; and no risk not on the register. Every one of those assumptions is
generous, which is why real quantitative analysis uses simulation over impact ranges with stated
correlation — and why the number it produces is a decision input, not an answer.

## 9. Checklist

For a schedule and risk review, or for assessing evidence in this cluster.

Schedule:

- [ ] Can the owner name the current critical path, the second path, and the float between them?
- [ ] Are all hard constraints listed, justified, and their effect on float understood?
- [ ] Is there a stated difference between the plan and the required target date?
- [ ] Is there a basis-of-schedule document naming the assumptions, calendars and productivity basis?
- [ ] Open ends, negative lags, negative float and constraint counts — measured this period?
- [ ] Do peak resource requirements fit the physical and contractual reality of the site?
- [ ] Was the last update a progress update, a re-plan, or a re-plan presented as an update?

Risk:

- [ ] Which risks are missing because writing them down would be awkward?
- [ ] Does every risk owner have authority over the cause, and a dated action?
- [ ] Is the analysis quantitative where the decision needs a number, and qualitative only where it does not?
- [ ] Are correlation assumptions stated explicitly rather than left at the tool's default?
- [ ] Is contingency tied to named risks, at a stated confidence level?
- [ ] Is contingency released as risks expire, with a record of each release?
- [ ] Does anyone present expected value as though it were a budget?

---

## Related

- `CMP-03 — PCL-AI: the fourteen competencies` — planning and scheduling, project risk and performance measurement, defined and levelled
- `CMP-05 — PML-AI: the twenty-four competencies` — planning and execution, integration, and the three delivery approaches
- `BPG-05 — Schedule quality — a practical review` — the full review method behind §3
- `BPG-10 — Contingency and management reserve` — how contingency is set, held and released
- `BPG-17 — Quantitative schedule risk analysis` — the analysis method behind §5 and §8.2
- `TPL-11 — Quantitative schedule risk analysis input sheet` — the input instrument

## Sources and standards

- PCI platform certification catalogue (`backend/Data/MultiCert.cs`), verified August 2026 — the seeded
  competency sets from which this cluster is drawn.
- PCI Body of Knowledge domains 6, 8, 9, 10 and 12 (`docs/bok/`) — earned value and forecasting, project
  lifecycle, adaptive delivery, scheduling and risk management. Cited by domain; not reproduced.
- The three-point estimating approximations in §8.1 are long-standing published techniques described in
  our own words; no standard's text or tables are reproduced. All figures in §8 are illustrative and
  recomputed from first principles.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
