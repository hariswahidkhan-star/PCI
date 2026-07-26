# Domain 15 — Programmes, Portfolios and Enterprise Delivery

> **Group:** Enterprise delivery and the digital future (Domain 15 of 3 in Part Four).
> **Target:** ~72 pages. **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain lifts the **Meridian Care Records** programme of Domains 1
> to 4 to the tier above it — the portfolio that funds it and the enterprise that governs it — and
> supplies the dependency-product, multi-period allocation, benefits-bridge and enterprise-latency
> arithmetic that Domain 16 consumes when it measures whether any of it landed. British English;
> USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Every domain before this one has, quite deliberately, held one thing constant: the boundary of the
thing being led. Domain 4 integrated the parts of a project into a whole. Domain 6 scheduled that
whole. Domain 8 put ranges round it. Domain 13 chose the delivery method for it. Domain 14 gave it a
data and AI estate. In all of them the project had an outside, and the outside was assumed to
cooperate.

It does not. The outside is where the other twelve initiatives live, where the same six integration
engineers are already committed, where four business cases have each independently promised the same
administrative post reduction, and where the decision the project needs will pass through tiers of
governance that no one has ever added up. **The central claim of this domain is that a portfolio is
not a bigger project, because the arithmetic that governs it is different in kind: at project scale
quantities add, and at enterprise scale they multiply, compete and double-count.** A leader who
carries project intuitions upward will be confidently wrong in four specific ways, each of which this
domain computes. Milestone confidence multiplies down rather than averaging out. Capacity that is
sufficient in aggregate is routinely infeasible period by period. Benefits summed across components
overstate what the enterprise can bank. And governance latency accumulates tier by tier into a bill
that appears in no budget line.

The domain proceeds outward. KA 15.1 builds programme architecture and shows that a programme
milestone with several individually likely predecessors is not likely at all — the same
multiplicative penalty Domain 8 found at a schedule merge point, now applied to whole components —
and identifies **decoupling**, not better estimating, as the lever that pays. KA 15.2 handles
portfolio balancing: the benefits register with its double counts eliminated, and allocation under a
constraint that binds *per period*, which is where the ranking heuristic Domain 2 warned about fails
expensively. KA 15.3 treats enterprise capacity as the real constraint it is, applies Domain 13's
flow arithmetic to a portfolio's work in progress, prices protective capacity honestly (it does not
always pay), and subjects the enterprise PMO to the same value test it applies to everyone else.
KA 15.4 governs transformation: enterprise decision latency summed across tiers using Domain 3's
formula, and strategic reporting — where the commonest portfolio report in circulation aggregates
performance in a way that reverses its sign.

**Learning objectives.** After this domain a candidate can: distinguish a project, a programme and a
portfolio by the decisions each exists to make rather than by size; design a programme architecture
in components, tranches and a target operating state, and state what each tranche buys;
**compute the probability that a programme milestone is met from the on-time probabilities of its
independent predecessors, and show how quickly that collapses as predecessors accumulate**; derive
the per-dependency reliability a stated milestone confidence demands, and use it to demonstrate when
better estimating cannot possibly deliver the target; rank the three structural levers — reduce the
count, decouple, buffer — and price each; build a portfolio benefits bridge that eliminates shared,
same-pool, cascade and already-committed double counts, and show what elimination does to payback;
**allocate a portfolio under a multi-period capacity constraint, demonstrate that aggregate
feasibility does not imply period feasibility, and quantify what a ranking heuristic leaves on the
table**; apply Domain 13's flow arithmetic to portfolio work in progress and price the benefit
deferral that excess concurrency causes; compute the survival probability of a capacity plan and the
breakeven failure cost at which protective capacity becomes worth its forgone value; state an
enterprise PMO's recurring value test and distinguish it from one-off gains; **sum enterprise
decision latency across programme and portfolio tiers, price it at a cost of delay, and rank the
available redesigns**; aggregate portfolio performance correctly and detect the unweighted-average
trap; and govern AI-assisted portfolio analysis without letting a model select a portfolio.

**The master programme, one tier up.** Meridian Care Records continues from Domains 1 to 4 — the
clinical-records rollout to **40 clinics**, approved cost **USD 2,400,000**, full-potential benefit
**USD 979,200** a year, realistic benefit **USD 685,440** a year at 70 % adoption, and the cost of
delay of **USD 14,280 per week** derived in Domain 1. Domain 3 established Meridian's steering-body
latency at `E[wait] = M/2 + L = 4/2 + 2 =` **4.0 weeks**. This domain places Meridian inside the
health group's **five-component portfolio** (records, referrals interoperability, clinician
scheduling, analytics platform, pharmacy stock automation) with a combined approved cost of
**USD 4,880,000**, and inside a **five-tier enterprise decision architecture**. Every figure in the
chapter descends from those.

---

## Knowledge Area 15.1 — Programme architecture and dependency management

*Topics: 15.1.1 what a programme is, and is not · 15.1.2 architecture — components, tranches and the
target state · 15.1.3 dependency arithmetic at programme scale · 15.1.4 decoupling as the primary
lever.*

### 15.1.1 What a programme is, and is not

**Definition.** A programme is a temporary organisation created to deliver a **coherent change in
outcomes** through a set of components — projects and enabling activities — that cannot deliver the
outcome individually. Its unit of decision is the **component set**: what to start, what to stop,
what to sequence, and what to fund next.

The definition that matters is comparative, and the comparison is by decision rather than by scale.
A **project** decides how to produce a defined output within a mandate. A **programme** decides which
components produce the outcome, and repeatedly re-decides it as evidence arrives. A **portfolio**
decides which programmes and projects the organisation should be doing at all, given a finite
capacity and a strategy — Domain 2's territory, revisited each cycle rather than once.

Three consequences follow, and each is a test a leader can apply on Monday morning.

**A large project is not a programme.** If the components cannot be usefully re-sequenced or stopped
independently, the programme layer is adding governance without adding optionality, and the workable
structure is one project with a good work breakdown structure (Domain 4, KA 4.2.1). The diagnostic
question is direct: *name a component this programme could stop next quarter without abandoning the
outcome.* If there is none, dissolve the layer.

**A collection of projects is not a programme.** If the components deliver independent outcomes, the
programme layer is a reporting convenience, and the honest structure is a portfolio grouping. The
cost of the pretence is real: components acquire a false interdependence, and each waits for
programme-level decisions it did not need.

**A programme's benefit is not the sum of its components' benefits.** This is the single most
consequential difference and KA 15.2 computes it. Component business cases are written to be
approved, and they are written separately, so they claim overlapping benefits as though they were
additive. At programme and portfolio level that claim has to be reconciled against reality, and the
reconciliation reliably removes a double-digit percentage of the total.

### 15.1.2 Architecture — components, tranches and the target state

**The target operating state.** A programme needs a stated description of the organisation as it will
be when the change has landed: the processes, roles, systems, data, locations and measures. Without
it the programme has an output list and no test of coherence, and every component optimises locally.
The description is a governance artefact, not a design document: its purpose is to make the question
"does this component still contribute?" answerable at a gate.

**Components.** Each component carries a named owner, an output, a contribution to the target state,
its dependencies in and out, and — this is the part usually missing — a statement of what happens to
the outcome if the component is cancelled. A component whose cancellation has no describable
consequence is not part of the programme.

**Tranches.** A tranche is a group of components delivered together to reach a **usable
intermediate state** — one at which benefit begins and at which the programme could stop with
something of value already in operation. Tranche design is therefore the programme's principal
option-creating decision, and it is governed by one rule: **a tranche boundary belongs where the
organisation can genuinely operate.** A boundary placed for reporting convenience produces the worst
of both worlds, a gate with nothing to decide and a partial state nobody can use.

Meridian's tranches illustrate the rule. Tranche 1 delivers the records application and the national
patient-index interface to **10** clinics in one region — a genuinely usable state, because those ten
clinics can operate on the new record and the remaining thirty continue as they are. Tranche 2 adds
the second and third regions and the migration of historical records. Tranche 3 completes the fourth
region and retires the legacy system, which is the only irreversible step in the programme and
therefore the only place a hard gate is warranted (Domain 3, KA 3.3.1).

**Programme-level dependencies.** Components depend on each other, and they also depend on things
outside the programme: an estate refurbishment, a regulatory approval, a supplier's product release,
another programme's platform. A programme dependency register records, per dependency, the giver, the
receiver, what is needed, the date needed, the date promised, the owner on both sides, and the
consequence of a breach. Domain 2's assumption and dependency management (KA 2.3.4) supplies the
discipline at project level; the programme addition is that dependencies now cross accountability
boundaries, so the register's most important column is *the owner on the giving side*, and a
dependency with no owner there is not a dependency, it is a hope.

### 15.1.3 Dependency arithmetic at programme scale

Here the domain earns its quantitative keep, and the result is one every programme leader should be
able to produce on a whiteboard.

**The multiplication rule.** A milestone that requires **all** of `k` independent predecessors to be
met is met only if every one of them is met. Its probability is therefore the **product** of theirs,
not their average and emphatically not their minimum:

```
P(milestone) = p1 × p2 × … × pk
```

Domain 8 established the schedule form of this at a merge point (8.A.2: two paths each 80 % likely
give the merged event 64 %, which is why deterministic critical-path analysis is systematically
optimistic at convergence). The programme form is the same theorem applied to whole components rather
than to activity paths, and it is more dangerous, because at programme level the predecessors are
owned by different people, each of whom reports honestly on their own item and none of whom is
looking at the product.

**Worked example 15.1.3 — Meridian's Region A go-live, and the four-region programme milestone.**

1. **Setup.** Meridian's Region A go-live requires six independent predecessors, each assessed by its
   own owner at the following probability of being met on the committed date: records application
   release **0.90**; national patient-index interface **0.88**; data migration and reconciliation
   **0.85**; clinic estate and network works **0.92**; clinician training and competency **0.95**;
   information-governance approval **0.90**. Every one of them is "on track" by the programme's own
   reporting convention. The programme milestone committed externally is the **simultaneous** go-live
   of all four regions, each with its own instance of the same six-dependency structure — 24
   dependencies in total. Independence is assumed and is discussed below.
2. **Formula.** `P(milestone) = Π pᵢ` over the predecessors. For the four-region milestone,
   `P = (Π pᵢ)⁴`, the four regions being structurally identical and assumed independent.
3. **Substitution.** `0.90 × 0.88 × 0.85 × 0.92 × 0.95 × 0.90`; then that result raised to the
   fourth power.
4. **Result.** Region A: **0.52953912**, that is **52.95 %**. The four-region programme milestone:
   **7.86 %**. Expected number of regions on time: `4 × 0.52953912 =` **2.1182** — so **1.8818**
   regions are expected to be late.
5. **Interpretation.** Six components, none worse than 0.85, produce a coin flip; four such regions
   produce a milestone that is **92.1 % likely to be missed** while every single component is
   reported as on track. That gap between component reporting and milestone probability is the
   defining pathology of programme delivery, and it is arithmetic rather than culture. Three
   consequences a leader should draw. First, **the product is the number to report**: a programme
   status report that lists twenty-four green components and a committed date is internally
   inconsistent, and the fix is one multiplication. Second, **better estimating cannot rescue the
   design.** To reach an 80 % confidence on the four-region milestone every one of the 24
   dependencies would have to be met with probability `0.80^(1/24) =` **99.07 %** — a standard no
   estate contractor, training programme or regulator has ever offered, and one the programme has no
   mechanism to enforce. Even across the six dependencies of a single region, 80 % confidence needs
   **96.35 %** each. Third, and following directly, **the lever is structural**: change `k`, not the
   `pᵢ`. To see why, note the sensitivity. Improving any single dependency by an absolute 0.03 moves
   the Region A probability by between **1.67** percentage points (training, 0.95 → 0.98) and
   **1.87** points (migration, 0.85 → 0.88) — the improvement is worth marginally most where the
   probability is lowest, but the whole range is under two points, and there is no combination of
   plausible estimating improvements that reaches 80 %. Removing one dependency from the milestone,
   by contrast, multiplies the probability by `1/pᵢ`, which for the 0.85 migration dependency is a
   gain of **9.34** points in one decision. **Two professional cautions.** The independence
   assumption is doing real work here and is usually optimistic: shared causes — one supplier, one
   scarce team, one regulator, one budget round — correlate the predecessors, and Domain 8's KA 8.A.1
   treatment of common drivers explains why a register with many entries and few drivers is
   concentrated rather than diversified. Correlation makes the *product* less pessimistic than shown
   for the good outcome and makes clustered failure far more likely, so the honest statement is that
   **52.95 % is a planning figure derived under an explicit assumption, and the assumption belongs in
   the report next to the number.** And the `pᵢ` are owner assessments, not measurements; their
   calibration is a data-quality question (Domain 9, and Domain 14's KA 14.1 on defect rates by
   class), which is why the register records who assessed each one and on what evidence.

> **Fig 15.1.1 — Why a programme milestone becomes improbable.** Line chart, x-axis the number of
> independent predecessors `k` from 1 to 24, y-axis the probability the merged milestone is met,
> four curves for uniform per-predecessor probabilities `p` = 0.99, 0.95, 0.90 and 0.85 plotting
> `p^k`. Meridian's Region A go-live is marked at `k` = 6 and **52.9539 %** with its six factors
> printed (0.90 · 0.88 · 0.85 · 0.92 · 0.95 · 0.90); the four-region programme milestone is marked
> at `k` = 24 and **7.8631 %**. A dashed rule at 80 % is annotated "at `k` = 24 it needs 99.0745 %
> on every dependency". The 50 % crossings are labelled: `k` = 5 at `p` = 0.85, `k` = 7 at
> `p` = 0.90, `k` = 14 at `p` = 0.95, and `k` = 69 at `p` = 0.99. Source: PCI original. Alt text:
> four decaying curves showing milestone probability falling steeply as the number of required
> predecessors rises, with even 0.95-reliable predecessors passing below one-half by fourteen.

### 15.1.4 Decoupling as the primary lever

If the milestone probability is a product, the only interventions that materially change it are
interventions on the *structure* of the product. Three exist, in descending order of value per unit
of effort.

**Reduce the count.** Ask of every predecessor whether the milestone genuinely requires it. Many
entries are there because someone drew an arrow, not because the outcome depends on the item. Each
removal multiplies the probability by `1/pᵢ`, and the removals compound.

**Decouple.** Where a predecessor is genuinely needed but not needed *by that date*, move it off the
milestone by designing an operable interim arrangement. Meridian did this twice. Historical **data
migration and reconciliation** (0.85) was removed from go-live by operating a documented two-week
paper-fallback for historical lookups, at a priced cost of **USD 18,000** in temporary clerical
cover — taking Region A from 52.95 % to **62.30 %**. **Information-governance approval** (0.90) was
obtained once at programme level for all four regions rather than four times regionally, removing it
from each regional milestone and taking Region A to **69.22 %**. The four-region milestone rose from
**7.86 %** to **22.96 %**, and the expected number of late regions fell from **1.8818** to
**1.2312**.

**Buffer.** Where neither reduction nor decoupling is available, the honest response is to commit to
a date that includes a buffer, sized by Domain 8's quantitative methods, and to hold internal dates
that are tighter. What is not available is committing to the unbuffered date and calling the
components green.

**Why the cost comparison settles the argument.** Programmes under milestone pressure reliably reach
for money instead of structure, and the two can be priced against each other. Meridian's alternative
proposal was **USD 240,000** of recovery spend — additional trainers and an accelerated estate
package — assessed to raise training from 0.95 to 0.98 and estate works from 0.92 to 0.96. That
raises Region A to **57.00 %** and the four-region milestone to **10.56 %**: a gain of **2.69**
percentage points on the committed milestone for 240,000, or **USD 89,093** per percentage point.
The structural changes gained **15.10** points for 18,000 — **USD 1,192** per point, or **1.34 %** of
the cost of the money route, a ratio of **74.7 to one**. The general result is worth stating plainly
because it is counter-intuitive to sponsors: **on a milestone whose probability is a product,
structure is roughly two orders of magnitude cheaper than effort.** Money buys movement in one factor;
decoupling removes a factor.

### AI in this KA

**Where it earns its place.** Building and reconciling a programme dependency register from
component plans, contracts and interface documents, and listing dependencies with no owner on the
giving side, no date, or a promised date later than the date needed — a document-comparison task that
is tedious, high-volume and has a checkable right answer. Computing the milestone product across
hundreds of dependency configurations, and searching for the smallest set of removals that reaches a
target confidence, which is a deterministic combinatorial problem and exactly the sort of thing to
delegate. Detecting shared causes across component registers by clustering dependency descriptions,
which surfaces the correlation that invalidates the independence assumption. Drafting the interim
operating arrangement options for a decoupling decision, for human evaluation.

**Where it must not go.** Assessing the `pᵢ`. A model asked how likely an estate contractor is to
meet a date will produce a confident number with no provenance, and that number will then be
multiplied into a board commitment. Probabilities come from owners with evidence, or from calibrated
history, and their source is recorded. Nor may a model decide a decoupling: an interim paper-fallback
in a clinical setting is a patient-safety decision belonging to clinical authority, and the domain's
arithmetic informs it without ever making it.

**Verification, concretely.** Reproduce the product by hand — six multiplications, and a leader
quoting 52.95 % who cannot do so should not be quoting it. Check every AI-extracted dependency
against its source document on a stated sample, and confirm each flagged defect with a human before
reporting it as a finding, per Domain 14's verification tier for outputs of this consequence
(KA 14.3.3). State the independence assumption every time the product is printed.

### Key terms — KA 15.1

| Term | Meaning |
|---|---|
| **Programme** | A temporary organisation delivering a coherent outcome through components that cannot deliver it individually. |
| **Portfolio** | The set of programmes and projects an organisation chooses to run within a finite capacity and a strategy. |
| **Target operating state** | The stated description of the organisation once the change has landed; the test of component coherence. |
| **Component** | A project or enabling activity within a programme, with a named owner and a stated consequence of cancellation. |
| **Tranche** | A group of components delivered together to reach a usable intermediate state at which benefit begins. |
| **Programme dependency register** | The record of cross-boundary dependencies, whose critical column is the owner on the giving side. |
| **Multiplication rule** | The probability a milestone requiring all of `k` independent predecessors is met is the product of their probabilities. |
| **Decoupling** | Removing a predecessor from a milestone by designing an operable interim arrangement, multiplying the probability by `1/pᵢ`. |
| **Required per-dependency reliability** | The uniform probability each of `k` dependencies must carry to reach a target milestone confidence: the `k`-th root of the target. |

### Sample MCQs — KA 15.1

**MCQ 15.1-A `[15.1.3 · Application]`** A programme milestone requires four independent
predecessors, assessed at 0.95, 0.90, 0.90 and 0.85. The probability the milestone is met is closest
to:
- A. 0.90
- B. 0.85
- C. 0.65 ✅
- D. 0.60

*Rationale:* `0.95 × 0.90 × 0.90 × 0.85 = 0.654075`, so 0.65 (15.1.3). A is the arithmetic mean of
the four, the commonest error; B is the minimum, the second commonest; D subtracts the summed
shortfalls from one (`1 − 0.40`), which treats the shortfalls as additive.

**MCQ 15.1-B `[15.1.3 · Analysis]`** Meridian's Region A go-live has six independent predecessors
giving a milestone probability of 52.95 %. Four structurally identical regions must go live
simultaneously. The programme milestone probability is:
- A. 52.95 %, because the regions are identical
- B. 13.24 %, being 52.95 % divided by four
- C. 7.86 % ✅
- D. 28.04 %

*Rationale:* `0.52953912⁴ = 7.86 %` (15.1.3). A ignores that all four must succeed; B divides
instead of taking the fourth power; D squares instead of raising to the fourth power
(`0.52953912² = 28.04 %`), the error of treating four regions as two.

**MCQ 15.1-C `[15.1.3 · Evaluation]`** A programme needs 80 % confidence in a milestone that
depends on 24 independent predecessors. The required probability on each is closest to:
- A. 80.0 %
- B. 96.3 %
- C. 99.1 % ✅
- D. 99.9 %

*Rationale:* `0.80^(1/24) = 99.0745 %` (15.1.3). A applies the target to each component; B is the
answer for six dependencies, not 24; D over-corrects. The teaching point is that C is unattainable,
which is why the lever is structural.

**MCQ 15.1-D `[15.1.4 · Evaluation]`** Removing the 0.85 data-migration dependency from Meridian's
regional go-live milestone raises the regional probability from 52.95 % to 62.30 %. The mechanism is
best described as:
- A. improving the migration team's performance
- B. multiplying the remaining product by `1/0.85` ✅
- C. adding 0.85 of buffer to the milestone
- D. averaging the remaining five probabilities

*Rationale:* Decoupling divides the product by the removed factor, which is the same as multiplying
by its reciprocal — `0.52953912/0.85 = 0.6229872` (15.1.4). A describes the money route, which bought
2.69 points on the programme milestone for 240,000; C and D misstate the arithmetic.

**MCQ 15.1-E `[15.1.2 · Analysis]`** The soundest test that a proposed tranche boundary is real is
whether:
- A. it falls at the end of a financial quarter
- B. the organisation can genuinely operate in the intermediate state ✅
- C. a governance body is available to meet on that date
- D. it divides the components into groups of similar size

*Rationale:* A tranche exists to reach a usable intermediate state at which benefit begins and the
programme could stop with value in operation (15.1.2). A, C and D are reporting conveniences that
produce gates with nothing to decide.

### Self-check — KA 15.1

1. *State the multiplication rule and the two wrong answers it displaces.* — The probability a
   milestone requiring all of `k` independent predecessors is met is the product of their
   probabilities; the wrong answers are the average and the minimum.
2. *Why can better estimating not deliver an 80 % four-region milestone at Meridian?* — It would
   require 99.07 % on each of 24 dependencies, which no supplier, trainer or regulator offers and
   the programme cannot enforce; the lever is to reduce or decouple dependencies.
3. *What distinguishes a programme from a large project?* — A programme's components can be
   usefully re-sequenced or stopped independently; if none can, the programme layer adds governance
   without optionality.

---

## Knowledge Area 15.2 — Benefits and portfolio balancing

*Topics: 15.2.1 the portfolio benefits register and the four double counts · 15.2.2 balancing under a
binding constraint · 15.2.3 the multi-period constraint and the aggregate-capacity illusion · 15.2.4
rebalancing, staging and stopping.*

### 15.2.1 The portfolio benefits register and the four double counts

**Definition.** A portfolio benefits register is the single authoritative statement of what the
portfolio will deliver, by benefit rather than by component, with each benefit carrying a measure, a
baseline, a receiving-organisation owner and the components that contribute to it. It is organised by
benefit precisely so that two components contributing to one benefit cannot each claim it.

Component business cases, written separately to be approved, overstate the portfolio's total for four
distinct and separately detectable reasons. Naming them matters because each has a different
correction.

**The shared benefit.** Two components claim the same saving because both genuinely contribute to it.
Neither case is dishonest; the sum is wrong. Correction: count the benefit once, in the register, with
both components recorded as contributors.

**The same-pool over-claim.** Components claim releases from a pool that cannot supply them all —
most often headcount, but equally floor space, licences or clinical session time. Correction: cap the
aggregate claim at the pool's size and allocate the cap.

**The cascade, or enabler, double count.** A platform or data component claims a share of the benefits
that flow through it, which the consuming components have already claimed. Correction: an enabler's
own benefit is only what does not pass through another component's claim; the rest is recorded as an
enabling contribution with no independent value.

**The already-committed benefit.** A component claims against a baseline that already assumes a
saving committed elsewhere — an operational efficiency target, a prior programme's residual, a
contractual price reduction. Correction: reset the baseline to the committed position.

**Worked example 15.2.1 — the Meridian portfolio benefits bridge.**

1. **Setup.** Five components, each claiming annual benefit at its own **full potential**:
   Meridian records **979,200**; referrals interoperability **264,000**; clinician scheduling
   replacement **318,000**; analytics platform upgrade **180,000**; pharmacy stock automation
   **240,000**. Review establishes four adjustments. (i) **96,000** of clinician time released from
   chasing paper referrals is claimed in both records and referrals. (ii) Records claims **5.0** and
   scheduling **3.4** administrative posts from one pool of **6.0**, valued at **42,000** per post.
   (iii) **108,000** of the analytics platform's 180,000 is "improved decision-making" realised
   through records and scheduling, already counted there. (iv) **60,000** of the pharmacy claim rests
   on a baseline that already assumes savings committed in an operational programme outside the
   portfolio. Portfolio-wide adoption, per Domain 2's realistic assumption, is **70 %**. Approved
   portfolio cost is **USD 4,880,000** (records 2,400,000, referrals 640,000, scheduling 890,000,
   analytics 520,000, pharmacy 430,000). The investment committee applies a **four-year** simple
   payback rule.
2. **Formula.** Net benefit = Σ claimed at full potential − Σ eliminations, then × adoption.
   Elimination for the same-pool case = (Σ claims − pool cap) × unit value. Simple payback =
   portfolio cost ÷ net annual benefit.
3. **Substitution.** Gross `979,200 + 264,000 + 318,000 + 180,000 + 240,000`. Eliminations
   `96,000 + (8.4 − 6.0) × 42,000 + 108,000 + 60,000`. Then `× 0.70`, and `4,880,000 ÷` the result.
4. **Result.** Gross claimed at full potential **USD 1,981,200**. Eliminations **USD 364,800** —
   96,000 shared, **100,800** same-pool (2.4 posts), 108,000 cascade, 60,000 already committed — an
   **elimination rate of 18.41 %**. Net full potential **USD 1,616,400**; at 70 % adoption, net
   realistic benefit **USD 1,131,480** a year. The unreconciled figure, `1,981,200 × 0.70 =`
   **USD 1,386,840**, overstates that by **USD 255,360**, which is **22.57 %** of the honest number.
   Simple payback moves from **3.5188 years** on the unreconciled figure to **4.3129 years** on the
   honest one — a difference of **0.7941 years**, or **9.53 months**.
5. **Interpretation.** The decision, not the number, is what moved: the unreconciled portfolio passes
   a four-year payback rule and the reconciled one fails it. That is the whole professional case for
   doing the bridge before approval rather than discovering it in Domain 16's benefits measurement,
   and it produces a threshold worth carrying: **the maximum elimination rate this portfolio can
   absorb and still clear four years is 12.03 %**, so an observed 18.41 % breaks the rule with room
   to spare, and a portfolio whose bridge has never been done should be assumed to be somewhere in
   that range. Three further points. First, an identity that makes the bridge auditable: because
   adoption scales the eliminations too, the **elimination rate is invariant to the adoption
   assumption** — 364,800/1,981,200 and 255,360/1,386,840 are both **18.41 %** — so the two
   adjustments can be argued independently and in either order, and a reviewer can check one without
   settling the other. Second, eliminating a double count **creates no value**; it prevents a wrong
   decision. A portfolio report that presents 364,800 of eliminations as a PMO saving is committing
   the same category error in the opposite direction, and KA 15.3 keeps it out of the PMO's value
   case for exactly that reason. Third, the same-pool correction is the one most often resisted,
   because both claimants are right about their own component; the register's answer is that the pool
   is an enterprise asset and its cap is an enterprise fact, which is why benefits are owned by the
   receiving organisation and not by the delivering component (Domain 2, KA 2.3.2; Domain 16 measures
   them).

### 15.2.2 Balancing under a binding constraint

**The principle.** A portfolio decision is an allocation under a constraint, and the first
professional act is to **name the constraint honestly**. It is rarely money. It is usually a scarce
capability — the integration engineers, the clinical safety reviewers, the one architect who
understands the interface layer — and a portfolio balanced against a budget while a capability binds
will be approved and will not be delivered.

Domain 2 established the base result at KA 2.2.3, and it is not re-derived here: **ranking candidates
by value per unit of the binding constraint is a heuristic, not an optimum**, and with lumpy
candidates the feasible sets must be enumerated — the Beta-plus-Gamma outcome that neither the raw
value ranking nor the scoring model selected. Domain 15's contribution is the extension that makes
this bite at enterprise scale: real constraints bind **per period**, not in aggregate.

**Balance across dimensions, not only value.** Before the arithmetic, a portfolio also has to be
balanced against exposures that no single ratio expresses: risk (Domain 8), time to benefit,
component novelty, dependence on one supplier or one team, and regulatory exposure. Domain 2 noted
that a set of individually sound investments can be collectively unbalanced and that no scoring model
detects it. The portfolio-level countermeasure is a small set of stated exposure limits — no more than
a given share of the portfolio dependent on one platform, one supplier, or one scarce team — applied
as feasibility constraints alongside capacity, so the enumeration returns only sets the organisation
can survive.

### 15.2.3 The multi-period constraint and the aggregate-capacity illusion

**The defect.** Portfolio capacity is almost always assessed annually — "we have 24 engineer-quarters
next year and the approved set needs 24, so it fits". That statement is not a feasibility test. Work
arrives when the components need it, and a set that fits in aggregate can be grossly infeasible in a
single period, which is the period in which the portfolio then fails.

**Worked example 15.2.3 — allocating Meridian's portfolio against a quarterly constraint.**

1. **Setup.** One scarce integration team supplies **6 units per quarter** for four quarters —
   **24 units** in aggregate. Six candidates compete, each with a quarterly demand profile and a net
   present value computed on PFL-AI Domain 4's methods:

   | Candidate | Q1 | Q2 | Q3 | Q4 | Units | NPV (USD) | NPV per unit |
   |---|---|---|---|---|---|---|---|
   | A records rollout continuation | 2 | 3 | 3 | 2 | 10 | 1,700,000 | 170,000 |
   | B referrals interoperability | 2 | 2 | 2 | 2 | 8 | 1,360,000 | 170,000 |
   | C pharmacy stock automation | 5 | 1 | 0 | 0 | 6 | 1,140,000 | **190,000** |
   | D clinician scheduling replacement | 0 | 1 | 2 | 3 | 6 | 1,020,000 | 170,000 |
   | E analytics platform upgrade | 1 | 1 | 1 | 1 | 4 | 640,000 | 160,000 |
   | F estate network refresh | 2 | 0 | 1 | 2 | 5 | 750,000 | 150,000 |

2. **Formula.** Three procedures compared. **Aggregate ranking:** take candidates in descending NPV
   per unit while total units remain. **Per-unit greedy with a period check:** take in the same
   order, skipping any candidate that breaches capacity in any quarter. **Enumeration:** evaluate all
   `2⁶ − 1 = 63` non-empty subsets, retain those satisfying `Σ demand ≤ 6` in **every** quarter, and
   select the maximum NPV.
3. **Substitution and result.**
   - **Aggregate ranking** takes C (6 units), A (10) and B (8) — exactly **24 of 24 units**, NPV
     **USD 4,200,000**, and full utilisation. It is **infeasible**: Q1 demand is `5 + 2 + 2 =` **9
     units against a capacity of 6**.
   - **Per-unit greedy** takes C first on its 190,000 ratio, leaving `(1, 5, 6, 6)`; A and B are then
     both blocked in Q1 and skipped; D and E fit; F is blocked. Result C + D + E, NPV
     **USD 2,800,000**, using **16 of 24** units.
   - **Enumeration** finds **26 feasible sets of 63** and selects **A + B + F**: quarterly demand
     `(6, 5, 6, 6)`, **23 of 24** units, NPV **USD 3,810,000**. The runner-up, A + B + E at
     **3,700,000** with demand `(5, 6, 6, 5)`, is the set to hold in reserve.
4. **Interpretation.** Two errors, two prices, and they point in opposite directions — which is what
   makes them hard to see together. The **aggregate illusion** overstates achievable value by
   `4,200,000 − 3,810,000 =` **USD 390,000**, or **10.24 %** of the true optimum, and it does so while
   reporting 100 % utilisation, which is why it survives review: a fully committed annual plan looks
   like good stewardship. The **greedy heuristic** is feasible but leaves **USD 1,010,000** on the
   table — **26.51 %** of the optimum, capturing only **73.49 %** — and it fails for a nameable
   reason: the highest-ratio candidate, C, consumes **five of the six Q1 units** and thereby excludes
   both of the two largest-value candidates. **A ratio cannot see when a demand profile is
   concentrated in the scarce period, because the ratio has already averaged the period away.** The
   professional conclusion is uncomfortable and correct: the portfolio-selection method most widely
   used in practice — rank by a value ratio, take until the budget is full — is wrong twice over, and
   the correction is not sophisticated. Enumerating 63 subsets is a spreadsheet exercise; at 25
   candidates it is `2²⁵ =` **33,554,432** subsets, which is a fraction of a second of computation and
   is the point: **this is not a hard computation, it is an ungoverned one**, and KA 15.3 makes it the
   enterprise PMO's job. Two cautions. The optimum uses **23 of 24 units — 95.83 % planned
   utilisation with slack of `(0, 1, 0, 0)`** — and KA 15.3 shows what that does to the plan's
   survival probability, so 3,810,000 is the value of a plan, not a forecast. And the NPVs are
   themselves estimates; a difference of 110,000 between A + B + F and A + B + E is inside anyone's
   estimating error, so the honest output of an enumeration is the **feasible frontier** — the top
   few sets and what distinguishes them — rather than a single winner presented as a computation.

> **Fig 15.2.1 — Aggregate capacity feasible, period capacity not.** Grouped bar chart, one group
> per quarter, three bars per group showing the demand on the scarce integration team under three
> plans, against a dashed capacity rule at **6 units a quarter**: the aggregate-only ranking
> C + A + B (`9, 6, 5, 4` — Q1 marked infeasible, NPV **4,200,000**, 24 of 24 units); the per-unit
> greedy C + D + E (`6, 4, 3, 3`, feasible, NPV **2,800,000**, 16 of 24 units, 73.49 % of the
> optimum); and the enumerated optimum A + B + F (`6, 5, 6, 6`, feasible, NPV **3,810,000**, 23 of
> 24 units). A side panel prints the three NPVs, the **390,000 (10.24 %)** aggregate illusion, the
> **1,010,000 (26.51 %)** greedy shortfall, and that 26 of 63 subsets are feasible. Source: PCI
> original. Alt text: three sets of quarterly bars against a capacity line, the first breaching the
> line in the opening quarter while the third sits just under it in every quarter.

### 15.2.4 Rebalancing, staging and stopping

**Rebalancing is the normal state.** A portfolio decided once is a budget, not a portfolio. The
enumeration above is a snapshot; capacity, value estimates and strategy all move, so the allocation
is re-run on a stated cycle with the components already in flight treated correctly — their remaining
demand and remaining value, never their original figures, which is the portfolio form of Domain 2's
sunk-cost warning (KA 2.4.2).

**Staging creates option value.** A component admitted as a small first tranche with a decision point
attached consumes less of the scarce period and preserves the right to stop. Meridian's tranche
design (15.1.2) is this device applied within a programme; at portfolio level the same move lets a
constrained period carry more candidates at lower commitment.

**Stopping is a portfolio capability, not an admission.** The portfolio's hardest arithmetic is that
stopping a component releases capacity in the *scarce period*, and that released capacity has a value
equal to the best displaced candidate's contribution. Domain 2's kill criteria (KA 2.4.3) supply the
test; the portfolio addition is that the case for stopping is strengthened, not weakened, by naming
what the released capacity will do next — because a portfolio board that stops something without
reallocating has taken a loss without buying anything.

### AI in this KA

**Where it earns its place.** Running the enumeration, including the exposure-limit constraints, and
returning the feasible frontier with the binding period identified for each rejected set — a
deterministic search that scales far past human patience. Reconciling component business cases against
a benefits register and proposing candidate double counts by matching benefit descriptions, measures
and receiving owners across cases, which is the highest-yield document task in this domain and one
nobody has time to do by hand. Testing whether a same-pool claim exceeds the pool by summing claims
against establishment data. Re-running the allocation under alternative capacity, value and adoption
assumptions to produce the sensitivity a board should be given.

**Where it must not go.** Selecting the portfolio. The enumeration returns sets; the choice among
them involves strategic weight, risk appetite, political feasibility and obligations that no
objective function contains, and it belongs to the accountable body. Nor may a model confirm a double
count: it proposes candidates, and a human with access to the establishment data and the component
owners decides which are real, because a wrongly eliminated benefit removes a component's case as
surely as a wrongly counted one inflates it. And AI must not generate the value estimates it then
optimises over — a closed loop of plausible numbers is the specific failure Domain 14 calls the
plausible wrong number (KA 14.3.4).

**Verification, concretely.** Re-compute the chosen set's per-period demand by hand against capacity,
which is a dozen additions and catches every infeasibility. Reconcile the benefits bridge with a
signed statement from each receiving-organisation owner that they accept the capped figure — the
elimination is only real when the person who must deliver it agrees. Publish the elimination rate and
the breakeven elimination rate together, so the board sees whether its decision rule is robust or
marginal.

### Key terms — KA 15.2

| Term | Meaning |
|---|---|
| **Portfolio benefits register** | The authoritative statement of portfolio benefits, organised by benefit rather than by component. |
| **Benefits bridge** | The reconciliation from gross claimed benefit through eliminations and adoption to net realistic benefit. |
| **Shared benefit** | One benefit claimed by two components that both genuinely contribute to it; counted once. |
| **Same-pool over-claim** | Aggregate claims on a resource pool exceeding the pool; capped at the pool and allocated. |
| **Cascade (enabler) double count** | An enabling component claiming benefit already claimed by the components it enables. |
| **Already-committed benefit** | Benefit claimed against a baseline that already assumes a saving committed elsewhere. |
| **Elimination rate** | Eliminations ÷ gross claimed benefit; invariant to the adoption assumption. |
| **Breakeven elimination rate** | The elimination rate at which a portfolio just satisfies its investment decision rule. |
| **Binding constraint** | The resource that actually limits the portfolio — usually a scarce capability, rarely money. |
| **Aggregate-capacity illusion** | Judging feasibility on total capacity when the constraint binds period by period. |
| **Feasible frontier** | The top few feasible sets and what distinguishes them — the honest output of an enumeration. |

### Sample MCQs — KA 15.2

**MCQ 15.2-A `[15.2.3 · Analysis]`** A portfolio's scarce team supplies 6 units a quarter for four
quarters. Three candidates demanding 24 units in total are selected on the basis that total capacity
is 24 units. The most accurate criticism is that:
- A. the portfolio is under-committed
- B. aggregate feasibility does not imply period feasibility; the set demands 9 units in Q1 ✅
- C. the team's capacity should have been expressed annually
- D. net present value is the wrong selection measure

*Rationale:* Constraints bind per period; the set is aggregate-feasible and Q1-infeasible by 3 units
(15.2.3). A inverts the problem; C names the cause of the error as its cure; D is a different debate.

**MCQ 15.2-B `[15.2.3 · Evaluation]`** In the Meridian portfolio allocation, ranking by NPV per unit
yields 2,800,000 while enumeration yields 3,810,000. The reason the ranking fails is that:
- A. the ratios were computed incorrectly
- B. enumeration uses a different objective function
- C. the highest-ratio candidate consumes five of the six units in the scarce quarter, excluding the
  two largest-value candidates ✅
- D. the candidates have equal ratios, so the ranking is arbitrary

*Rationale:* A ratio averages the period away and cannot see a demand profile concentrated in the
binding period (15.2.3). Both methods maximise NPV, so B is wrong; three candidates do share a
170,000 ratio, but that is not what causes the 1,010,000 shortfall.

**MCQ 15.2-C `[15.2.1 · Application]`** Five components claim 1,981,200 of annual benefit at full
potential. Eliminations total 364,800 and portfolio adoption is 70 %. The net realistic annual
benefit is:
- A. USD 1,616,400
- B. USD 1,386,840
- C. USD 1,131,480 ✅
- D. USD 792,036

*Rationale:* `(1,981,200 − 364,800) × 0.70 = 1,131,480` (15.2.1). A omits the adoption adjustment;
B omits the eliminations; D applies adoption twice (`1,131,480 × 0.70`), the error of taking a
component figure that is already adoption-adjusted and adjusting it again.

**MCQ 15.2-D `[15.2.1 · Analysis]`** Two components claim a combined 8.4 administrative posts from
a pool of 6.0, valued at 42,000 each. The elimination is:
- A. USD 352,800
- B. USD 252,000
- C. USD 100,800 ✅
- D. USD 42,000

*Rationale:* `(8.4 − 6.0) × 42,000 = 100,800` (15.2.1). A is the gross claim; B is the pool's total
value, which is the retained benefit, not the elimination; D is one post.

**MCQ 15.2-E `[15.2.1 · Evaluation]`** A portfolio's benefits bridge eliminates 18.41 % of gross
claimed benefit, and the breakeven elimination rate for its four-year payback rule is 12.03 %. The
correct conclusion is that:
- A. the portfolio clears the rule with a 6.4-point margin
- B. the portfolio fails the rule, and would still fail it at any elimination rate above 12.03 % ✅
- C. the rule should be relaxed to accommodate the eliminations
- D. the eliminations of 364,800 should be reported as a portfolio saving

*Rationale:* An observed rate above the breakeven rate means the rule is not met (15.2.1). A reads
the comparison backwards; C is a decision for the investment committee, not a conclusion from the
arithmetic; D commits the error the topic explicitly warns against — eliminating a double count
prevents a wrong decision and creates no value.

### Self-check — KA 15.2

1. *Name the four portfolio double counts and one correction each.* — Shared benefit (count once,
   record both contributors); same-pool over-claim (cap at the pool and allocate); cascade or enabler
   (an enabler's own benefit is only what does not pass through a consumer's claim); already
   committed (reset the baseline).
2. *Why is the elimination rate invariant to the adoption assumption?* — Adoption scales the
   eliminations and the gross claim identically, so their ratio is unchanged — which lets the two
   adjustments be argued and audited independently.
3. *What is the honest output of a portfolio enumeration?* — The feasible frontier: the top few sets,
   the binding period for each rejected one, and what distinguishes them — not a single winner, since
   the value estimates cannot support that precision.

---

## Knowledge Area 15.3 — Capacity and enterprise PMOs

*Topics: 15.3.1 enterprise capacity as the real constraint · 15.3.2 portfolio work in progress ·
15.3.3 protective capacity and its price · 15.3.4 the enterprise PMO and its value test.*

### 15.3.1 Enterprise capacity as the real constraint

**Definition.** Enterprise delivery capacity is the rate at which an organisation can complete
change, measured in the unit of its binding capability, at a stated quality. It is a **measured
rate**, not a budget and not an aspiration, and the measurement is historical: completions per period
over a period long enough to include the organisation's normal disruptions.

Three properties are routinely misunderstood, and each one costs money.

**Capacity is a rate, not a stock.** An organisation with 40 people does not have "40 people of
capacity"; it has a completion rate that its structure, dependencies and interruption load produce.
Adding people changes the rate by less than proportionally, and by less again where coordination cost
rises — Domain 12's coordination arithmetic (KA 12.2.2 and 12.A.2) and Domain 4's interface count
(`n(n−1)/2` against `n` to an integration layer, KA 4.2.3) both bear on this and neither is
re-derived here. The practical rule: **treat the completion rate as the primitive and the headcount
as one of its inputs.**

**Capacity is capability-specific.** A portfolio does not consume generic effort. It consumes
integration engineering, clinical safety review, data migration, change management and testing in
component-specific mixes, and the constraint is whichever of those binds first. A capacity model with
one number in it will approve a portfolio that is 60 % utilised on average and 190 % utilised on the
thing that matters.

**Capacity is consumed by the portfolio's own overheads.** Governance attendance, reporting,
onboarding, rework and the interruption load of work already in flight all draw on the same people.
Capacity measured from completions already includes these; capacity estimated from headcount and
availability does not, which is why the estimated figure is always higher and always wrong in the
same direction.

### 15.3.2 Portfolio work in progress

**The mechanism.** Domain 13 established the flow arithmetic and it is applied, not re-derived, here:
**Little's Law**, `W = T × C`, relates work in progress `W`, cycle time `T` and throughput `C` for
any system in steady state, independent of arrival and service distributions (KA 13.2.3). Its
portfolio reading is uncomfortable and exact: **starting more initiatives does not increase
throughput, which capacity sets; it increases cycle time, which is when benefit arrives.**

**Worked example 15.3.2 — the price of starting everything.**

1. **Setup.** The health group's delivery organisation completed **15** portfolio initiatives in the
   last **three** years, a measured throughput of **5.0 a year**. The portfolio board has approved
   and started **12**. The portfolio's net realistic benefit run rate, from KA 15.2.1, is
   **USD 1,131,480** a year across five components — **USD 226,296** an initiative.
2. **Formula.** `T = W / C` (Little's Law, KA 13.2.3). Benefit forgone from excess concurrency =
   annual benefit run rate × excess cycle time, where excess cycle time = `(W_actual − W_limit) / C`.
3. **Substitution.** `T = 12/5` against `T = 5/5`; excess `(12 − 5)/5`; then
   `1,131,480 × 1.4`.
4. **Result.** Average cycle time **2.40 years** at 12 in flight, against **1.00 year** at 5 — an
   excess of **1.40 years**. The portfolio's entire benefit stream therefore arrives 1.40 years
   later than it need, worth **USD 1,584,072** — cross-checked per initiative as
   `226,296 × 1.4 × 5 =` the same figure.
5. **Interpretation.** The number is large, and its character matters more than its size: this is
   **forgone once and never recovered**. Throughput is unchanged — five initiatives finish per year
   under either policy — so no annual report will show a loss, no variance will be raised, and the
   only visible symptom is that everything takes two and a half years. The identity behind it is worth
   memorising because it prices any concurrency argument in one line: **cost of excess work in
   progress = annual benefit run rate × excess work in progress ÷ throughput.** Three professional
   qualifications. The 1,584,072 is undiscounted and is therefore a **floor**; the rigorous treatment
   discounts the deferred stream (PFL-AI Domain 3), which reduces the headline and does not change the
   sign. Little's Law requires a steady state, so a portfolio in the middle of a step change in
   capacity is outside its scope — the law is a theorem about a system, not a forecast for a
   transition. And a work-in-progress limit is not a decision to do less; it is a decision to
   **finish** in a sequence, which requires the portfolio board to choose an order and defend it, and
   that is the reason the limit is resisted: `W = 12` is what a board does when it will not choose.
   The board that says "all twelve are priorities" has, arithmetically, said that none of them is.

### 15.3.3 Protective capacity and its price

**The problem.** KA 15.2.3's optimum plans **23 of 24 units — 95.83 % utilisation — with quarterly
slack of `(0, 1, 0, 0)`**. A plan with no slack in three of four quarters is a plan that survives only
if nothing goes wrong in those quarters, and the same multiplicative penalty KA 15.1.3 found for
milestones applies here to periods.

**Worked example 15.3.3 — does protective capacity pay?**

1. **Setup.** Measured over the last twelve quarters, the integration team loses units to unplanned
   support and absence with the following frequencies: **0 units 0.70**, **1 unit 0.22**, **2 units
   0.06**, **3 or more 0.02**. The aggressive plan A + B + F demands `(6, 5, 6, 6)` against a
   capacity of 6, so slack is `(0, 1, 0, 0)`. The alternative is to **reserve one unit a quarter** —
   plan to 5 — and re-run the enumeration at that capacity, which selects **A + E + F** with demand
   `(5, 4, 5, 5)`, slack against the true 6 of `(1, 2, 1, 1)`, and NPV **USD 3,090,000**. A quarter's
   capacity breach slips the affected work one quarter: **13 weeks at the programme cost of delay of
   14,280**, or **USD 185,640**.
2. **Formula.** A plan survives a quarter if the loss is no greater than that quarter's slack; the
   plan survives the year with probability `Π P(loss ≤ slackₜ)` over the four quarters — the
   multiplication rule of KA 15.1.3 applied to periods. Expected slippage cost =
   `(1 − P(survive)) ×` breach cost. Breakeven breach cost = NPV forgone ÷ the improvement in failure
   probability.
3. **Substitution.** Aggressive `0.70 × 0.92 × 0.70 × 0.70`; protective
   `0.92 × 0.98 × 0.92 × 0.92`. Then `(1 − P) × 185,640` for each, and
   `(3,810,000 − 3,090,000) ÷ (0.76311424 − 0.31556)`.
4. **Result.** The aggressive plan survives all four quarters with probability **31.56 %**; the
   protective plan, **76.31 %**. Protective capacity costs **USD 720,000** of NPV — **18.90 %** of
   the optimum. Expected slippage cost falls from **USD 127,059** to **USD 43,975**, a saving of
   **USD 83,084**. The breakeven breach cost is **USD 1,608,744**, equivalent to **112.66 weeks** of
   programme delay, or **8.67 quarters**.
5. **Interpretation.** On these numbers **protective capacity does not pay**, and a leader should be
   able to say so, because the received advice — always leave slack — is not a theorem. Spending
   720,000 of value to avoid an expected 83,084 of slippage is a poor trade by a factor of nearly
   nine, and the discipline the arithmetic imposes is to state the condition under which the
   conclusion reverses: **a breach must cost more than about 1.61 million for the reservation to be
   worth it.** That condition is not exotic. A breach that lands on a regulatory submission date, a
   clinical safety approval, a contractual liquidated-damages milestone or a go-live already announced
   to patients is easily worth more than eight quarters of ordinary delay, and in that case the answer
   flips decisively. So the professional output is not "reserve capacity" or "do not"; it is: **price
   the breach on the specific work in the zero-slack periods, then compare.** Two cautions. The
   31.56 % figure is itself the product of four period probabilities, so it inherits KA 15.1.3's
   independence assumption — correlated losses (one absence wave, one support incident spanning two
   quarters) make clustered breaches likelier than the product suggests, and where the loss driver is
   shared the calculation should be run on the pessimistic assumption as well. And a 31.56 % survival
   probability is not a 68 % chance of disaster: it is a high chance of *some* re-planning, which is
   normal, and the reason to compute it is to know in advance which quarters will need it, not to be
   alarmed.

### 15.3.4 The enterprise PMO and its value test

**Definition.** An enterprise portfolio management office is the function that maintains the
portfolio's authoritative data, runs the allocation and prioritisation process, measures capacity and
delivery performance, and provides second-line assurance (Domain 3, KA 3.3.2). It has no delivery
accountability and no decision rights; it exists to make the accountable bodies' decisions better.

The role has three honest variants and one dishonest one. A **reporting** office collects and
publishes; a **standards** office defines and assures method; a **delivery** office also supplies
delivery capability. The dishonest variant is the office that reports without improving any decision,
which is Domain 3's governance-artefact failure at enterprise scale (KA 3.1.1) — the organisation has
the portal, the template and the monthly pack, and no allocation decision has ever changed as a
result.

**The value test, and why it is usually failed on the second reading.** An enterprise PMO should be
subject to the arithmetic it applies to everyone else, and the test that matters distinguishes
**recurring** from **one-off** value.

**Worked example 15.3.4 — does the enterprise PMO pay?**

1. **Setup.** The health group's enterprise PMO costs **8** staff at a blended **132,000** plus
   **124,000** of tooling and other costs. Its claimed year-one contributions: **half** of KA 15.3.2's
   work-in-progress deferral recovered by imposing a limit; the **517,650** of annual latency saving
   computed in KA 15.4.1; **186,000** a year of duplicate tooling eliminated across three components;
   and the **364,800** of benefits eliminations from KA 15.2.1.
2. **Formula.** Cost = staff × rate + other. Separate the claimed value into recurring and one-off,
   and test the recurring value alone against the annual cost.
3. **Substitution.** `8 × 132,000 + 124,000`. Recurring `517,650 + 186,000`. One-off
   `1,131,480 × 0.5`. The eliminations are excluded — see below.
4. **Result.** Cost **USD 1,180,000** a year, which is **24.18 %** of the portfolio's 4,880,000
   approved cost. Recurring value **USD 703,650**. One-off value **USD 565,740**. Year one totals
   **USD 1,269,390** — a surplus of **USD 89,390**. Year two, with the one-off gone, is **703,650**
   against 1,180,000 — a **deficit of USD 476,350**.
5. **Interpretation.** The office pays in year one and fails in year two, and it will be defunded in
   year three by a finance director who has noticed. That pattern is the single commonest fate of
   enterprise PMOs and it is a **reporting** failure before it is a value failure: the year-one case
   was built on a one-off, and one-offs do not repeat. Note also what has been kept out. The
   **364,800** of benefits eliminations is excluded because eliminating a double count creates no
   value (KA 15.2.1) — it prevented a portfolio from being approved on a false payback, which is
   decision quality of the highest order and is not a cash benefit, and an office that books it as
   one has falsified its own case. The constructive output is a target rather than a verdict: the
   office needs **USD 476,350** of additional recurring value, and KA 15.2.3 identified exactly
   where it is available — the **1,010,000** allocation gap between the ranking heuristic and the
   enumerated optimum, which recurs at every allocation cycle. The office therefore breaks even
   recurringly if it captures **47.16 %** of that gap. That is a testable, monitorable commitment of
   the right shape: it names the mechanism, it is measurable at the next cycle by comparing the
   selected set against the enumerated frontier, and it fails visibly. **An enterprise PMO whose
   value case cannot be written in that form should be resized to the reporting it actually does.**

### AI in this KA

**Where it earns its place.** Measuring capacity honestly from completion history rather than from
availability assumptions, including the interruption load that headcount models omit. Maintaining a
capability-specific capacity model and flagging the constraint that binds first, per period, across a
whole portfolio — high-volume bookkeeping with a right answer. Computing plan survival probabilities
across candidate plans and loss distributions, and returning the breakeven breach cost, which is the
number a board needs. Detecting duplicate tooling and overlapping capability across component
architectures. Producing the enumeration and the feasible frontier each cycle, which is precisely the
recurring work the PMO's value case above depends on capturing.

**Where it must not go.** Setting the work-in-progress limit or choosing the sequence, which is a
prioritisation decision belonging to the portfolio board — and the decision the board is avoiding
when it starts twelve initiatives. Estimating the loss distribution from plausibility rather than
from twelve quarters of records. Assessing an enterprise PMO's own value, which is a self-interested
judgement no function should automate. And no AI-produced capacity number should reach a board without
its measurement window and its exclusions stated, because a capacity figure without those is not a
measurement.

**Verification, concretely.** Recompute throughput from the raw completion list, not from a
dashboard, and state the window — five a year from fifteen in three years is one division and it is
the foundation of every figure in this KA. Reproduce the survival product by hand for the chosen plan;
it is four multiplications. Require the PMO's value case to separate recurring from one-off in the
statement itself, and to name the mechanism and the measurement for each recurring item, since that
separation is the check that catches the year-two failure a year early.

### Key terms — KA 15.3

| Term | Meaning |
|---|---|
| **Enterprise delivery capacity** | The measured rate at which an organisation completes change, in the unit of its binding capability. |
| **Capability-specific capacity** | Capacity modelled per scarce capability, since the portfolio consumes them in component-specific mixes. |
| **Portfolio work in progress** | The number of initiatives in flight; by Little's Law it sets cycle time, not throughput. |
| **Excess work in progress cost** | Annual benefit run rate × excess work in progress ÷ throughput — forgone once and never recovered. |
| **Protective capacity** | Capacity deliberately withheld from allocation so that a period's plan can absorb variance. |
| **Plan survival probability** | The product over periods of the probability that each period's loss does not exceed its slack. |
| **Breakeven breach cost** | Value forgone by reserving capacity ÷ the improvement in failure probability it buys. |
| **Enterprise PMO** | The function that maintains portfolio data, runs allocation, measures capacity and gives second-line assurance; no decision rights. |
| **Recurring versus one-off value** | The distinction that decides whether a support function survives its second year. |

### Sample MCQs — KA 15.3

**MCQ 15.3-A `[15.3.2 · Application]`** A delivery organisation completes 5 initiatives a year and
has 12 in flight. Average cycle time is:
- A. 0.42 years
- B. 1.00 year
- C. 2.40 years ✅
- D. 5.00 years

*Rationale:* `T = W/C = 12/5 = 2.40` years (Little's Law, cited from KA 13.2.3). A inverts the
ratio; B is the cycle time at a work-in-progress limit of 5; D is the throughput read as a duration.

**MCQ 15.3-B `[15.3.2 · Evaluation]`** Reducing portfolio work in progress from 12 to 5 at an
unchanged throughput of 5 a year, with a net benefit run rate of 1,131,480, is best described as:
- A. increasing throughput by 140 %
- B. bringing the whole benefit stream forward by 1.40 years, worth 1,584,072 once ✅
- C. saving 1,584,072 every year thereafter
- D. reducing the portfolio's benefit, since fewer initiatives are in flight

*Rationale:* Throughput is capacity-set and unchanged; cycle time falls by `(12−5)/5 = 1.40` years
and the whole stream shifts forward once — `1,131,480 × 1.4 = 1,584,072` (15.3.2). A misreads
Little's Law; C converts a one-off shift into an annuity; D confuses starting with finishing.

**MCQ 15.3-C `[15.3.3 · Analysis]`** A four-quarter plan has slack of 0, 1, 0 and 0 units. Losses
of 0, 1 and 2 or more units occur with probabilities 0.70, 0.22 and 0.08. The probability the plan
survives all four quarters is closest to:
- A. 70.0 %
- B. 41.5 %
- C. 31.6 % ✅
- D. 24.0 %

*Rationale:* `0.70 × 0.92 × 0.70 × 0.70 = 31.56 %` (15.3.3). A applies the single-quarter figure to
the year; B uses 0.92 in two quarters rather than one; D applies 0.70 to all four quarters and
ignores the slack quarter's tolerance.

**MCQ 15.3-D `[15.3.3 · Evaluation]`** Reserving capacity costs 720,000 of NPV and lifts plan
survival from 31.56 % to 76.31 %. A quarter's breach costs 185,640. The correct conclusion is:
- A. reserve, because the survival probability more than doubles
- B. do not reserve unless a breach would cost more than about 1.61 million ✅
- C. reserve, because the expected saving of 83,084 is positive
- D. the comparison cannot be made without a discount rate

*Rationale:* `720,000 / (0.76311424 − 0.31556) = 1,608,744` is the breach cost at which the trade
breaks even (15.3.3). A argues from a ratio without a price; C is true and irrelevant, since 83,084
is far below 720,000; D is a refinement, not an obstacle.

**MCQ 15.3-E `[15.3.4 · Evaluation]`** An enterprise PMO costing 1,180,000 a year claims 703,650 of
recurring value and 565,740 of one-off value. The soundest assessment is that it:
- A. pays, with a year-one surplus of 89,390
- B. pays, because total claimed value exceeds cost
- C. fails from year two by 476,350 and needs a named recurring mechanism ✅
- D. should book the 364,800 of benefits eliminations to close the gap

*Rationale:* One-offs do not repeat, so the recurring test is the one that matters (15.3.4). A and B
are true of year one and irrelevant thereafter; D books a prevented wrong decision as a cash benefit,
which the domain rejects at KA 15.2.1.

### Self-check — KA 15.3

1. *Why is capacity a rate rather than a stock?* — Completion rate is produced by structure,
   dependencies and interruption load; headcount is one input to it, and adding people changes the
   rate by less than proportionally.
2. *State the cost of excess portfolio work in progress in one line.* — Annual benefit run rate ×
   excess work in progress ÷ throughput; at Meridian, `1,131,480 × 7/5 =` 1,584,072, forgone once.
3. *What makes an enterprise PMO's value case credible?* — Recurring value separated from one-off,
   each recurring item with a named mechanism and a measurement — for example capturing 47.16 % of
   the 1,010,000 allocation gap each cycle.

---

## Knowledge Area 15.4 — Transformation governance and strategic reporting

*Topics: 15.4.1 enterprise decision latency · 15.4.2 strategic reporting and the aggregation trap ·
15.4.3 the portfolio view and what it must not hide.*

### 15.4.1 Enterprise decision latency

**The extension.** Domain 3 established that a decision body's expected latency is
`E[wait] = M/2 + L` — half the meeting interval plus the whole paper lead time — that latency sums
across escalation tiers, and that a one-week cut in the paper lead time saves a full week while a
one-week cut in the meeting interval saves only half of one (KA 3.2.3, KA 3.3.3). None of that is
re-derived. The enterprise extension is that a transformation portfolio does not send **one** decision
up a path; it sends a **distribution** of decisions up different paths, and the bill is the weighted
sum.

**Worked example 15.4.1 — the annual cost of an enterprise decision architecture.**

1. **Setup.** The health group's five tiers, each with its meeting interval `M` and paper lead time
   `L` in weeks: **component board** `M` = 2, `L` = 1; **programme board** `M` = 4, `L` = 2;
   **portfolio board** `M` = 6, `L` = 2; **investment committee** `M` = 12, `L` = 3; **executive
   board** `M` = 13, `L` = 4. Decisions are routed by value class: class 1 (≤ 25,000) **48** a year
   through 1 tier; class 2 (25,001–150,000) **22** through 2; class 3 (150,001–500,000) **9** through
   3; class 4 (500,001–2,000,000) **4** through 4; class 5 (> 2,000,000, reserved matters) **2**
   through all 5. Reviewed history, per Domain 3's convention, shows **25 %** of escalated decisions
   sit on the critical path and convert their wait into delay. Cost of delay **USD 14,280** a week.
2. **Formula.** Tier latency `M/2 + L`; cumulative latency for a `t`-tier path is the sum over the
   first `t` tiers (Domain 3, KA 3.3.3). Gross latency-weeks = Σ over classes of count × cumulative
   latency. Delaying weeks = gross × critical-path share. Cost = delaying weeks × cost of delay.
3. **Substitution.** Tier latencies `2/2+1 = 2.0`, `4/2+2 = 4.0`, `6/2+2 = 5.0`, `12/2+3 = 9.0`,
   `13/2+4 = 10.5`; cumulative `2.0, 6.0, 11.0, 20.0, 30.5`. Then
   `48(2.0) + 22(6.0) + 9(11.0) + 4(20.0) + 2(30.5)`, `× 0.25`, `× 14,280`.
4. **Result.** A decision travelling all five tiers waits **30.5 weeks** and costs **USD 435,540**.
   Across the portfolio's **85** decisions: **468 gross latency-weeks**, an average of **5.51 weeks**
   a decision; **117.0** delaying weeks at the 25 % share; a bill of **USD 1,670,760 a year**.
5. **Interpretation.** One million six hundred and seventy thousand dollars a year is the running
   cost of a committee timetable, and it appears in no budget, is attributed to no decision, and is
   described in year-end reviews as the portfolio having been slow. Three results follow, ranked by
   value per unit of organisational pain, and the ranking is the useful part. **First, the paper
   deadline.** One week off every tier's paper lead time saves one week on every tier traversal, and
   the portfolio makes **145** traversals a year: `145 × 0.25 × 14,280 =` **USD 517,650**, or
   **30.98 %** of the entire bill, from an administrative change that costs nothing and removes no
   scrutiny anyone can name. This is Domain 3's lever result at enterprise scale, and the scale is
   what makes it a board-level item rather than a housekeeping one. **Second, the out-of-cycle
   route.** A written-resolution procedure completing classes 4 and 5 in 1.5 weeks saves
   **USD 471,240**, or **28.21 %** — nearly as much as the first lever, from a mechanism affecting
   only **6** of the 85 decisions. That is because those 6 decisions — **7.06 %** of the volume —
   carry **141 of the 468 gross weeks**, or **30.13 %** of the latency. **Seven per cent of the
   decisions carry thirty per cent of the delay**, which is why "add the executive board to the
   approval path" is such an expensive sentence and why the out-of-cycle mechanism belongs at the top
   of the architecture, not at the bottom. **Third, tier removal**, which is the intervention
   organisations reach for first and which is worth least here: removing the component board from
   classes 3 to 5 (raised directly, component board informed) saves 30 gross weeks —
   **USD 107,100**, or **6.41 %** — because the tier removed is the fast one. **The general rule:
   remove latency where the latency is, which is the slow tier and the paper deadline, not the tier
   that is easiest to remove.** Two cautions. The 25 % critical-path share is an assumption from
   history and the whole bill is proportional to it, so it must be stated as an assumption and
   re-estimated; and latency is not the only property of a governance tier — a reserved matter exists
   because someone must be answerable for it, and the arithmetic prices the tier without ever
   deciding whether it is legitimate. What the arithmetic does is force the trade to be explicit,
   which is a conversation governance reviews almost never have because until the latency is added up
   there is nothing to weigh the tier against.

### 15.4.2 Strategic reporting and the aggregation trap

**The principle.** A portfolio report exists to support three decisions and no others: **continue,
change or stop** each component; **reallocate** capacity between them; and **escalate** what the
portfolio cannot resolve. Every element that serves none of the three is decoration, and Domain 11's
executive-communication discipline (KA 11.2) applies unchanged — the portfolio addition is that
aggregation itself can invert a conclusion.

**The trap.** Performance indices are **ratios**, and ratios do not average. Averaging component
indices without weighting them by the quantity in their denominator gives an answer that is not merely
imprecise but frequently of the opposite sign — and this is the most common defect in circulating
portfolio reports, because the unweighted average is what a spreadsheet produces by default.

**Worked example 15.4.2 — the portfolio index that reversed its sign.**

1. **Setup.** At the end of Q2 the five components report, in USD:

   | Component | `PV` | `EV` | `AC` | `BAC` | `CPI` | Share of portfolio `AC` |
   |---|---|---|---|---|---|---|
   | Meridian records | 1,300,000 | 1,196,000 | 1,352,000 | 2,400,000 | 0.8846 | **54.67 %** |
   | Referrals interoperability | 380,000 | 361,000 | 372,000 | 640,000 | 0.9704 | 15.04 % |
   | Clinician scheduling | 250,000 | 262,500 | 245,000 | 890,000 | 1.0714 | 9.91 % |
   | Analytics platform | 300,000 | 306,000 | 291,000 | 520,000 | 1.0515 | 11.77 % |
   | Pharmacy automation | 220,000 | 224,400 | 213,000 | 430,000 | 1.0535 | 8.61 % |
   | **Portfolio** | **2,450,000** | **2,349,900** | **2,473,000** | **4,880,000** | | |

2. **Formula.** Portfolio `CPI = ΣEV / ΣAC` and `SPI = ΣEV / ΣPV` (the registered EVM definitions,
   Domain 7 KA 7.3). `EAC = BAC / CPI` by the index method. The defective alternative is the
   unweighted mean of the component `CPI`s.
3. **Substitution.** `2,349,900/2,473,000` and `2,349,900/2,450,000`; the mean of 0.8846, 0.9704,
   1.0714, 1.0515 and 1.0535; then `4,880,000` divided by each index.
4. **Result.** Portfolio `CPI` **0.95** (0.950222) and `SPI` **0.96** (0.959143). The unweighted mean
   of the component indices is **1.01** (1.006308). `EAC` at the portfolio `CPI` is
   **USD 5,135,639.81**, giving `VAC` of **(USD 255,639.81)**; at the unweighted mean it is
   **USD 4,849,408.40**, giving `VAC` of **USD 30,591.60**. The reported variance at completion swings
   by **USD 286,231** and changes sign. (Cents are carried here only so the swing reconciles exactly;
   the reporting convention remains whole units.)
5. **Interpretation.** One arithmetic choice moves the portfolio from 30,592 favourable to 255,640
   adverse, and the direction of the error is predictable: **the unweighted mean flatters whenever the
   large components are the troubled ones**, which is the normal case, because scale and difficulty
   correlate. Here one component holds **54.67 %** of portfolio cost incurred and is the only one
   below 0.95; four small components each slightly ahead outvote it four to one in an unweighted
   average and hold 45.33 % of the money. The corrections are two, and both belong in the standard.
   **Aggregate by summing the numerators and denominators, never by averaging the ratios** — a rule
   that generalises to every ratio a portfolio reports, including adoption, defect and utilisation
   rates. And **publish the concentration alongside the aggregate**: the share of portfolio cost
   sitting in components below the alert threshold, here **54.67 %** below `CPI` 0.95, which is the
   figure that tells a board whether one component is carrying the portfolio's problem or the problem
   is general. Two cautions. A single portfolio `CPI` is an aggregate and therefore always masks
   distribution — it is a headline for the concentration line beneath it, not a substitute for the
   component table. And `EAC = BAC/CPI` assumes performance to date continues, which is one of several
   defensible forecasts; Domain 7 (KA 7.2) governs the selection, and a portfolio report that presents
   one `EAC` without naming its method has reported an opinion as a measurement.

### 15.4.3 The portfolio view and what it must not hide

**What the one-page view carries.** Per component: the continue/change/stop recommendation with its
owner; benefit at stake and benefit realised to date (Domain 16 owns the measurement); performance
aggregated correctly with the concentration line; the binding-capability demand in the next two
periods; the top dependency with an owner on the giving side; and the decisions required with their
dates. At portfolio level: net benefit against the bridge, capacity utilisation by capability and
period, work in progress against its limit, and the open decisions with their expected latency.

**Four things it must not hide**, each of which this domain has priced. The **milestone product**,
rather than a list of green components against a committed date (15.1.3). The **binding period**,
rather than annual capacity (15.2.3). The **elimination rate** on claimed benefits, rather than the
gross claim (15.2.1). And the **latency of the open decisions**, rather than a list of items awaiting
approval (15.4.1). A portfolio report that omits all four is not a weak report; it is a report that
cannot support any of the three decisions it exists for.

**The information-age discipline.** Domain 14 established that a fact's age governs the decisions it
can support (KA 14.2.1). At portfolio level the constraint is sharper than at project level, because
a portfolio report is assembled from component reports that were themselves assembled earlier: a
monthly portfolio pack routinely presents facts six to eight weeks old as current. The remedy is to
**date every figure at its own source**, not at the pack, and to refuse to make a reallocation
decision on a capacity figure older than the period it constrains.

### AI in this KA

**Where it earns its place.** Computing the enterprise latency bill across every decision class and
candidate architecture, and ranking the redesigns by saving per unit of change — deterministic and
tedious in exactly the right proportions. Assembling the portfolio report from component data with
each figure carrying its own source date, and flagging figures older than a stated tolerance.
Recomputing aggregate indices correctly from the underlying `EV`, `PV` and `AC` sums and detecting
where a submitted report has averaged ratios — a mechanical check that catches the KA 15.4.2 defect
before a board sees it. Drafting the narrative around a computed exception set, for review.

**Where it must not go.** Writing the continue/change/stop recommendation. That is a judgement with
an accountable owner, and a portfolio board receiving a machine-written recommendation is being
invited to ratify rather than to decide — Domain 3's decidability failure (KA 3.1.1) reproduced at
enterprise scale. Nor may a model set a tolerance, a threshold or a decision class, all of which are
risk-appetite decisions. And no AI-assembled portfolio pack goes to a board without the register of
AI uses Domain 14 requires (KA 14.4.3), naming for each output what the tool did, whose decision it
informs and who verified it.

**Verification, concretely.** Recompute the aggregate indices from the sums by hand — two divisions —
and compare against whatever the submitted pack contains; a discrepancy is the averaged-ratio defect
and it is present more often than not. Reproduce the latency arithmetic for the two largest decision
classes. Confirm every figure's source date against its source system on a stated sample. And require
that each recommendation in the pack carries a named human owner, because a recommendation without
one is not a recommendation, it is a suggestion nobody has made.

### Key terms — KA 15.4

| Term | Meaning |
|---|---|
| **Enterprise decision architecture** | The set of governance tiers, the decision classes routed through them, and the resulting latency distribution. |
| **Gross latency-weeks** | Σ over decision classes of count × cumulative tier latency; the portfolio's total waiting. |
| **Tier traversal** | One decision passing one tier; the unit on which a paper-lead-time saving accrues. |
| **Out-of-cycle route** | Domain 3's mechanism (KA 3.3.3) applied at enterprise scale, where it is worth most: the saving is largest at the slowest tier, because `E[wait] = M/2 + L` grows with `M`. |
| **Aggregation trap** | Averaging component ratios instead of summing their numerators and denominators; frequently reverses the sign. |
| **Concentration line** | The share of portfolio cost sitting in components below the alert threshold; the companion to any aggregate index. |
| **Portfolio one-page view** | The report supporting exactly three decisions: continue/change/stop, reallocate, escalate. |
| **Source-dated figure** | A reported value carrying the date of its own source rather than the date of the pack. |

### Sample MCQs — KA 15.4

**MCQ 15.4-A `[15.4.1 · Application]`** Five tiers have `M` and `L` of (2, 1), (4, 2), (6, 2),
(12, 3) and (13, 4) weeks. The expected latency of a decision that must pass all five is:
- A. 18.5 weeks
- B. 30.5 weeks ✅
- C. 37.0 weeks
- D. 20.0 weeks

*Rationale:* `(2/2+1) + (4/2+2) + (6/2+2) + (12/2+3) + (13/2+4) = 2.0+4.0+5.0+9.0+10.5 = 30.5`
(15.4.1, formula from KA 3.3.3). A halves the intervals and omits the paper lead times entirely
(1.0+2.0+3.0+6.0+6.5); C sums the meeting intervals alone without halving them or adding the lead
times (2+4+6+12+13); D omits the executive tier.

**MCQ 15.4-B `[15.4.1 · Evaluation]`** A portfolio's 85 decisions generate 468 gross latency-weeks
and 145 tier traversals; 25 % sit on the critical path and delay costs 14,280 a week. Cutting one
week from every tier's paper lead time saves:
- A. USD 517,650 ✅
- B. USD 1,670,760
- C. USD 303,450
- D. USD 2,070,600

*Rationale:* A one-week cut saves one week per traversal: `145 × 0.25 × 14,280 = 517,650`, which is
30.98 % of the 1,670,760 total (15.4.1). B is the whole bill; C applies the saving to the 85
decisions rather than the 145 traversals; D omits the critical-path share.

**MCQ 15.4-C `[15.4.1 · Analysis]`** In that architecture, 6 of the 85 decisions carry 141 of the
468 gross latency-weeks. The strongest implication is that:
- A. those 6 decisions should be delegated downward
- B. an out-of-cycle route at the slow tiers is worth far more per decision affected than removing a
  fast tier ✅
- C. the executive board should meet more often
- D. the 6 decisions are the most important and their latency is justified

*Rationale:* 7.06 % of decisions carry 30.13 % of latency, so a written-resolution route for them
saves 471,240 against 107,100 from removing the fast component-board tier (15.4.1). A may breach the
reserved-matters rationale; C is the weaker lever, since a week off `M` saves half a week; D is a
legitimate position but not an implication of the arithmetic.

**MCQ 15.4-D `[15.4.2 · Analysis]`** A portfolio has `ΣEV` 2,349,900 and `ΣAC` 2,473,000. The
unweighted mean of its five component `CPI`s is 1.006. The portfolio `CPI` is:
- A. 1.01, the mean of the components
- B. 0.95 ✅
- C. 0.96
- D. 1.05

*Rationale:* `CPI = ΣEV/ΣAC = 0.950222` (15.4.2). A averages ratios, the defect the topic exists to
prevent; C is the portfolio `SPI` (`ΣEV/ΣPV`); D inverts the ratio.

**MCQ 15.4-E `[15.4.2 · Evaluation]`** The unweighted-average `CPI` flatters this portfolio because:
- A. there are five components and five is too few to average
- B. the component with 54.67 % of cost incurred is the only one below 0.95, and four small ahead-of-
  budget components outvote it ✅
- C. the components have different budgets at completion
- D. `EAC = BAC/CPI` is the wrong forecasting method

*Rationale:* An unweighted mean gives each component equal voice regardless of the money behind it,
so it flatters whenever the large component is the troubled one (15.4.2). A is not the mechanism; C is
true but not the cause, since the weighting that matters is cost incurred; D is a separate question
governed by Domain 7.

**MCQ 15.4-F `[15.4.3 · Comprehension]`** A portfolio report should support exactly three decisions.
They are:
- A. plan, monitor and control
- B. continue/change/stop, reallocate capacity, escalate ✅
- C. approve, reject and defer
- D. report, review and assure

*Rationale:* Those three are what a portfolio body can actually do; anything serving none of them is
decoration (15.4.3).

### Self-check — KA 15.4

1. *Which enterprise latency lever pays most, and why?* — One week off every paper lead time: it
   saves a full week on each of 145 traversals, 517,650 or 30.98 % of the bill, at no cost and with
   no scrutiny removed.
2. *How is a portfolio performance index aggregated correctly?* — By summing numerators and
   denominators (`ΣEV/ΣAC`), never by averaging component ratios — which here reverses the sign of the
   reported variance at completion.
3. *Name the four things a portfolio view must not hide.* — The milestone product, the binding
   period, the elimination rate on claimed benefits, and the latency of the open decisions.

---

## Advanced topics — Domain 15

### 15.A.1 Where the boundary belongs, and what it costs to get wrong

The project/programme/portfolio boundary is treated as a naming question and is in fact a cost
question, because each layer added imposes three recurring charges that can be estimated before the
layer exists. It adds **coordination cost** — Domain 4's interface arithmetic and Domain 12's
coordination results apply, and neither is re-derived. It adds **latency**, at exactly KA 15.4.1's
rates: a tier inserted into a path used by 22 decisions a year at 4.0 weeks of expected wait, with a
25 % critical-path share, costs `22 × 4.0 × 0.25 × 14,280 =` **USD 314,160** a year before it has
improved a single decision. And it adds **reporting load**, which consumes the delivery capacity
KA 15.3 has just shown to be the binding constraint.

Against those charges a layer must supply something nameable: optionality (components that can be
stopped or re-sequenced), a benefit that no component owns alone, or an authority that no component
holds. The test is therefore symmetrical and it can be applied to an existing structure as easily as
a proposed one: **name what this layer decides that no other layer can, and compare it with the layer's
latency bill.** Layers that fail this test are common, and they are usually created by an
organisational event rather than a delivery need — a reorganisation, a merger, an audit finding — which
is why nobody remembers what they were for.

The inverse error deserves equal attention. Running a genuine portfolio problem as a programme
produces the failure Case study B records: components acquire false interdependence, the allocation is
never re-run, and capacity is committed once and never rebalanced. The diagnostic is whether the
components' benefits are separable. If they are, it is a portfolio, and it needs the arithmetic of
KA 15.2, not the coherence machinery of KA 15.1.

### 15.A.2 Rolling horizons and the option value of deferral

KA 15.2.3's enumeration is a single-shot allocation over four quarters, and real portfolios allocate
on a **rolling horizon**: re-optimise each period over the remaining horizon, with committed work
treated as fixed and everything else free. Three consequences matter and none of them requires new
mathematics.

**Re-optimisation dominates a good one-shot plan**, because information arrives. A plan optimal on
January's estimates is not optimal on April's, and the value of re-running the enumeration is the
difference — which is precisely the allocation gap KA 15.3.4 asks the enterprise PMO to capture, now
recurring quarterly rather than annually.

**Deferral has option value that the enumeration does not price.** A candidate deferred one period is
not a candidate lost; it is a candidate whose value estimate will be better next period, and whose
demand profile may fit a period that is not yet committed. Staging a candidate into a small first
tranche (15.2.4) buys the same option more cheaply. The honest position is that this domain's
enumeration maximises value under stated estimates and does **not** value the option; a portfolio
board should be told that explicitly, because the un-priced option is the strongest available argument
for admitting a candidate at low commitment rather than rejecting it.

**Commitment discipline is what makes a rolling horizon work.** A portfolio that re-optimises freely
each period will churn: components stopped and restarted, teams re-formed, and the coordination cost
of Domain 12 paid repeatedly. The countermeasure is a stated commitment horizon — typically one to two
periods in which the allocation is fixed absent a material event — beyond which everything is
provisional and is described as provisional in every report.

### 15.A.3 The reviewer's portfolio eye

Invariants to test on any programme or portfolio, each cheap and each diagnostic.

Every committed milestone has a stated **product** of its predecessors' probabilities, with the
independence assumption disclosed, rather than a list of green components. Every dependency has an
owner **on the giving side**, a date needed and a date promised. Every tranche boundary corresponds to
a state the organisation can operate in. The benefits register is organised **by benefit**, not by
component, and carries a bridge showing the elimination rate and the breakeven elimination rate for
the applicable investment rule. Every same-pool claim reconciles to establishment data with the
receiving owner's signature. The binding constraint is named as a **capability**, measured as a
**rate** from completions, and modelled **per period** — and the selected portfolio's feasibility is
demonstrated in the tightest period, not in aggregate. The selected set is compared against an
enumerated frontier, and the gap is reported. Work in progress has a stated limit with the excess
priced. Every zero-slack period has its breach cost estimated and compared against the value of
reserving capacity. Every escalation path has its latency computed as `Σ (M/2 + L)` and priced, and
the paper lead times are the first thing examined. Aggregate ratios are computed from summed
numerators and denominators and are published with a concentration line. Every reported figure carries
its own source date. Every AI-produced portfolio output appears in the AI use register with its
verifier named. And the one test that subsumes several others: **the portfolio's approved benefit
total reconciles, line by line, to the receiving organisations' own budgets** — because a benefit no
budget holder has accepted is a benefit nobody will deliver.

---

## Industry variations — Domain 15

- **Public sector and government.** Annual appropriation cycles impose a hard multi-period
  constraint that cannot be smoothed by borrowing forward, so KA 15.2.3's period feasibility is
  binding in law rather than in practice; benefits are frequently non-cash and non-fungible, so the
  bridge eliminates against policy outcomes rather than budgets, and the same-pool correction is
  politically the hardest because establishment reductions are announced separately from the
  programmes that enable them.
- **Regulated industries (pharmaceutical, nuclear, aviation, financial services).** Some milestone
  predecessors are external approvals whose probabilities the portfolio cannot influence at any
  price, which makes decoupling the *only* structural lever available and makes KA 15.1.4's cost
  comparison decisive; reserved-matters classes are larger, so the latency concentration of
  KA 15.4.1 is more extreme and the out-of-cycle route correspondingly more valuable.
- **Construction and infrastructure.** The binding capability is often a physical resource — a
  tunnelling machine, a crane, a rail possession — whose period constraint is absolute and whose
  reallocation cost is large, so protective capacity usually *does* pay on KA 15.3.3's test because a
  breach cascades into a possession window that cannot be re-booked within the year.
- **Healthcare.** Clinical session time is the binding capability and it is not fungible with money;
  clinical authority is not delegable to a portfolio body, so Meridian's decoupling decisions run
  through clinical governance in parallel, and the same-pool over-claim is endemic because clinician
  time released is the benefit almost every digital component claims.
- **Energy and utilities.** Regulatory price-control periods create hard multi-period envelopes and a
  benefits register dominated by avoided costs, which double-count with unusual ease — Case study B
  is this pattern; commissioning crews are the classic capability constraint and their peak-month
  demand is where portfolios fail.
- **Technology and product organisations.** Capacity is measured in team-quarters and the portfolio
  is genuinely re-optimisable, so the rolling-horizon discipline of 15.A.2 is the dominant practice;
  the characteristic failure is unlimited work in progress, because starting is cheap and visible
  while finishing is neither — which makes KA 15.3.2's arithmetic the single highest-value
  conversation available.

---

## Case study — Domain 15: the milestone that was never going to happen (health, Meridian)

**Situation.** Meridian's programme board had committed, to its funder and to a patient-facing
communications campaign, that all four regions would go live on the same date. Eleven weeks out, the
programme's status report showed 24 component dependencies, every one rated green or amber-improving,
and a green overall milestone. The programme director asked a question nobody had asked: *what is the
probability of the committed date?*

**What the arithmetic showed.** The six dependencies of a single region, at the owners' own
assessments — 0.90, 0.88, 0.85, 0.92, 0.95 and 0.90 — multiply to **52.95 %**. Four structurally
identical regions on one date give **7.86 %**: the committed milestone was **92.1 % likely to be
missed**, and the expected number of on-time regions was **2.1182** of four. Nothing in the component
reporting was wrong. Every owner had assessed their own item honestly, and no one had multiplied.

**The two proposals, priced against each other.** The delivery directorate proposed **USD 240,000** of
recovery spend — additional trainers and an accelerated estate package — assessed to lift training
from 0.95 to 0.98 and estate works from 0.92 to 0.96. That takes a region to **57.00 %** and the
programme milestone to **10.56 %**: **2.69** percentage points for 240,000, or **USD 89,093 per
point**. The programme office proposed two structural changes instead. Historical data migration
(0.85) came off the go-live milestone, operated for two weeks on a documented paper fallback for
historical lookups at a priced **USD 18,000** of temporary clerical cover — taking a region to
**62.30 %**. Information-governance approval (0.90) was obtained **once at programme level** for all
four regions rather than four times regionally, taking a region to **69.22 %** and the four-region
milestone to **22.96 %**: **15.10** points for 18,000, or **USD 1,192 per point** — **1.34 %** of the
cost per point of the money route, a ratio of **74.7 to one**.

**How it resolved.** The board took both structural changes and declined the recovery spend. It then
did the thing the arithmetic made unavoidable: at 22.96 % the simultaneous commitment was still not
defensible, so the four regional dates became **internal** dates and the external commitment moved to
a single programme milestone set after the fourth region with a buffer sized by Domain 8's methods.
The paper fallback went through clinical governance before it went into the plan. Expected late
regions fell from **1.8818** to **1.2312**, and the programme reported a milestone probability, an
independence assumption and a buffer, in place of twenty-four green squares.

**What the domain teaches here.** A programme status report that lists green components against a
committed date is internally inconsistent, and the inconsistency is one multiplication away from being
visible. Once it is visible, the cheap lever is structural: **on a milestone whose probability is a
product, removing a factor is worth roughly seventy times what buying improvement in a factor is
worth.** And note what the board could not do — it could not estimate its way to 80 %, because that
would have required 99.07 % on each of 24 dependencies, a standard nobody in the supply chain had
offered or could.

## Case study B — Domain 15: the portfolio that was fully funded and could not deliver (energy and utilities)

**Situation.** A regional transmission utility, anonymised, approved **11** grid-connection projects
for a three-year regulatory period. The scarce capability was high-voltage commissioning crews, of
which the utility had **9 crew-weeks a month** — **324** crew-weeks over 36 months. The approved
portfolio demanded **311** crew-weeks, **95.99 %** of aggregate capacity, and the investment committee
approved it on precisely that basis: the work fitted. Claimed benefits were **USD 24,600,000** a year.

**What had happened.** Two defects, both detectable before approval. **The constraint was never
tested by period.** Demand peaked at **17 crew-weeks a month for six consecutive months** in months
14 to 19 — **188.89 %** of capacity — against which the annual view was silent. Unmet demand in that
window was `(17 − 9) × 6 =` **48 crew-weeks**, and clearing 48 crew-weeks at the observed throughput
of 9 a month takes **5.3333 months** of pure catch-up. **The portfolio was therefore a minimum of five
and a third months late in its approved form, before any project encountered a single problem.** And
**the benefits were never bridged.** Three projects each claimed the same **3,100,000** of avoided
constraint payments and two each claimed the same **1,850,000** of deferred reinforcement:
eliminations of **USD 8,050,000** against a claimed 24,600,000, an elimination rate of **32.72 %** and
a net portfolio benefit of **USD 16,550,000**.

**How it resolved.** Six projects completed; five slipped past the regulatory window and carried
**USD 7,400,000** a year of the net benefit. Benefit forgone over the 5.3333-month structural
catch-up was `7,400,000 × 5.3333/12 =` **USD 3,288,889**, and the portfolio delivered
**USD 9,150,000** of its 16,550,000 on time — **55.29 %**. A re-sequencing exercise then did what the
approval process had not: it enumerated feasible sets against the **monthly** constraint and deferred
two projects to the following period. Nine projects, carrying **USD 15,000,000**, landed inside the
window — **90.63 %** — with the deferred two carrying 1,550,000. **Re-sequencing alone moved
USD 5,850,000 of annual benefit from late to on time, and it changed no project's scope, budget or
team.**

**What the domain teaches here.** Aggregate capacity feasibility is not feasibility; a portfolio at
95.99 % of annual capacity was at 188.89 % of monthly capacity for half a year, and one afternoon of
period-by-period arithmetic before approval would have found it. A benefits register organised by
component rather than by benefit will let the same avoided cost be claimed three times, and a 32.72 %
elimination rate is not an outlier. And the largest single improvement available to this portfolio was
not more crews, more money or better project management: it was **sequence**, which is free.

---

## Executive perspective — Domain 15

What a portfolio director cannot delegate in this domain:

- **The product behind every committed milestone.** Insist on `Π pᵢ` with its independence assumption
  stated, not a count of green components. A programme that cannot produce it has not assessed its own
  commitment (15.1.3).
- **The named binding constraint, measured as a rate and modelled by period.** Not a budget, not an
  annual total, not a headcount. Feasibility is demonstrated in the tightest period or it is not
  demonstrated (15.2.3, 15.3.1).
- **The benefits bridge before approval.** The elimination rate, the breakeven elimination rate for
  your investment rule, and a receiving owner's signature against every capped figure. Meridian's
  18.41 % elimination rate broke a four-year payback rule whose breakeven was 12.03 % (15.2.1).
- **The work-in-progress decision, which is a prioritisation decision you are avoiding.** Twelve
  initiatives in flight at a throughput of five costs 1.40 years of the entire benefit stream —
  1,584,072, forgone once. Saying that all twelve are priorities is arithmetically saying none is
  (15.3.2).
- **Your own decision architecture's annual bill.** 468 gross latency-weeks, 1,670,760 a year, and a
  free 517,650 available from one week off every paper deadline. Examine the paper deadlines and the
  slow tiers, not the tier that is easiest to remove (15.4.1).
- **How your portfolio's performance is aggregated.** Sums of numerators and denominators, with a
  concentration line. An averaged index moved this portfolio from 30,592 favourable to 255,640
  adverse, and the flattering direction is the normal one (15.4.2).

---

## Calculation exercises — Domain 15

**Exercise 15.1** A programme milestone requires five independent predecessors, assessed at 0.96,
0.93, 0.90, 0.88 and 0.80. (a) Compute the milestone probability. (b) Compute it again with the 0.80
dependency decoupled. (c) If instead every dependency were assessed at 0.93, how many could the
milestone carry and remain at or above 0.50? (d) What uniform per-dependency probability would deliver
0.85 across five dependencies?
*Solution.* (a) `0.96 × 0.93 × 0.90 × 0.88 × 0.80 =` **56.5678 %**. (b) Divide by 0.80:
**70.7098 %** — a gain of **14.1420** percentage points from one structural change. (c) `0.93⁹ =`
**52.0411 %** and `0.93¹⁰ =` **48.3982 %**, so at most **nine**. (d) `0.85^(1/5) =` **96.8019 %**.
Common error: answering (a) with the minimum, **0.80**, or with the mean of the five, **89.40 %** —
both of which are above the true figure and both of which are what a component-by-component status
report implicitly asserts.

**Exercise 15.2** A portfolio's scarce capability supplies **5 units a period** for three periods
(15 units in aggregate). Four candidates: P demands (2, 2, 2) for an NPV of 1,020,000; Q demands
(2, 2, 1) for 850,000; R demands (4, 1, 0) for 950,000; S demands (1, 1, 2) for 640,000. Compute the
selection under (a) ranking by NPV per unit with a period feasibility check, (b) full enumeration, and
(c) ranking by NPV per unit against aggregate capacity only. State the cost of each error.
*Solution.* Ratios: R **190,000**, P **170,000**, Q **170,000**, S **160,000**. (a) Take R, leaving
(1, 4, 5); P and Q are both blocked in period 1 and skipped; S fits. **R + S = USD 1,590,000**, using
9 of 15 units. (b) Enumerating all `2⁴ − 1 = 15` subsets, the maximum feasible set is
**P + Q + S = USD 2,510,000** with demand exactly (5, 5, 5). The heuristic leaves **920,000**,
**36.65 %** of the optimum. (c) Aggregate-only ranking takes R, P and S for 15 of 15 units and
**2,610,000** — apparently the best answer and fully utilised — but period 1 demands `4 + 2 + 1 =`
**7 units against 5**, so it is infeasible and overstates achievable value by 100,000. Common error:
reporting (c) as the answer because it uses 100 % of capacity; full utilisation of an annual total is
evidence of nothing.

**Exercise 15.3** Four components claim **1,240,000** of annual benefit at full potential. Review
finds: 85,000 of benefit claimed twice; claims of 4.8 posts against a pool of 3.5 valued at 38,000
each; 64,000 of enabler benefit already counted downstream; and 45,000 already committed outside the
portfolio. Adoption is 65 %; portfolio cost is 3,150,000; the investment rule is a four-year simple
payback. Compute the elimination rate, the net realistic benefit, the payback on both the reconciled
and unreconciled figures, and the breakeven elimination rate.
*Solution.* Eliminations `85,000 + (4.8 − 3.5) × 38,000 + 64,000 + 45,000 = 85,000 + 49,400 + 64,000
+ 45,000 =` **USD 243,400**, an elimination rate of **19.6290 %**. Net full potential **996,600**; at
65 % adoption, **USD 647,790**. The unreconciled figure is `1,240,000 × 0.65 =` **806,000**,
overstating by **158,210** — **24.42 %** of the honest number. Payback: honest **4.8627 years**;
unreconciled **3.9082 years**, which clears the rule. Breakeven: the rule needs `3,150,000/4 =`
787,500, so `787,500/0.65 =` 1,211,538.46 of net full potential and at most **28,461.54** of
eliminations — a breakeven elimination rate of **2.2953 %**. Common error: computing the elimination
on the same-pool claim as the pool's value (3.5 × 38,000 = 133,000) rather than as the excess over it;
the excess is 1.3 posts.

**Exercise 15.4** Four governance tiers have `M` and `L` in weeks of (2, 1), (4, 1), (8, 2) and
(12, 4). Decision volumes: **40** through one tier, **18** through two, **7** through three, **3**
through four. 30 % of decisions sit on the critical path; cost of delay is **11,500** a week. Compute
the gross latency-weeks, the annual delay bill, and the saving from cutting one week from every paper
lead time. Then state what proportion of the latency the top class carries.
*Solution.* Tier latencies `2/2+1 = 2.0`, `4/2+1 = 3.0`, `8/2+2 = 6.0`, `12/2+4 = 10.0`; cumulative
**2.0, 5.0, 11.0, 21.0**. Gross `40(2.0) + 18(5.0) + 7(11.0) + 3(21.0) = 80 + 90 + 77 + 63 =`
**310 latency-weeks**. Delaying weeks `310 × 0.30 =` **93.0**; bill `93 × 11,500 =`
**USD 1,069,500**. Tier traversals `40 + 36 + 21 + 12 =` **109**, so a one-week cut in every paper
lead time saves `109 × 0.30 × 11,500 =` **USD 376,050** — **35.1613 %** of the bill. The top class is
**3 of 68** decisions (**4.4118 %**) carrying **63 of 310** weeks (**20.3226 %**). Common error:
adding meeting intervals and omitting paper lead times, which gives cumulative 1.0, 3.0, 7.0 and 13.0
and a total of **182** weeks — an understatement of **41.2903 %**.

**Exercise 15.5** A delivery organisation completes **8** initiatives a year at steady state and has
**20** in flight. Its portfolio's net benefit run rate is **2,240,000** a year. Compute the average
cycle time, the cycle time under a work-in-progress limit of 8, the benefit consequence of the excess,
and the limit that would deliver a 1.5-year cycle time.
*Solution.* `T = W/C = 20/8 =` **2.50 years**; at `W` = 8, `T =` **1.00 year**; excess **1.50
years**. The whole benefit stream therefore arrives 1.50 years later than it need, worth
`2,240,000 × 1.5 =` **USD 3,360,000**, forgone once and never recovered. For `T` = 1.5,
`W = C × T = 8 × 1.5 =` **12**. Common error: assuming that cutting work in progress from 20 to 8
cuts throughput in proportion — Little's Law says throughput is set by capacity, not by how much work
is started (KA 13.2.3), so the same eight initiatives finish each year under either policy and the
only thing that changes is when.

---

## Practitioner's toolkit — Domain 15

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 15.T.1 — Programme dependency register with a computed milestone probability

One row per dependency: reference · milestone it feeds · what is needed · **owner on the giving side**
(name, not function) · owner on the receiving side · date needed · date promised · assessed
probability of being met on the date needed · **evidence for that assessment** · shared cause or
driver · consequence of breach · decoupling option and its priced cost. A footer per milestone
computes `Π pᵢ` across its predecessors, states the independence assumption, lists the shared drivers
that violate it, and shows the probability after each candidate decoupling with its cost — so the
board sees the product, the assumption and the priced structural options on one page. The register's
purpose is to make the difference between component status and milestone probability **impossible to
overlook**; a programme whose report shows green components and no product has not filled it in.

### Toolkit 15.T.2 — Multi-period portfolio allocation sheet

A grid: candidates down the side; the binding capability's demand **per period** across the top, plus
total units, value, value per unit, and the exposure tags (supplier, platform, team, regulatory) used
as feasibility limits. Below it: capacity per period as a measured rate with its measurement window;
per-period demand and slack for the selected set; and the **feasible frontier** — the top three to
five feasible sets, their values, and the binding period for each rejected higher-value set. Three
mandatory lines: the value of the selected set, the value of the enumerated optimum, and the gap
between them; the aggregate-only answer and its infeasible period, recorded so the illusion is visible
rather than merely avoided; and the plan's survival probability from the period slacks with the breach
cost estimated. Re-run every cycle with committed work fixed and remaining demand and remaining value
used for work in flight.

### Toolkit 15.T.3 — Portfolio benefits bridge

A single reconciliation, top to bottom, published with every investment paper: gross claimed benefit
at full potential by component; then a line per elimination, each tagged **shared / same-pool /
cascade / already-committed**, with the components affected, the basis, and the receiving owner who
has accepted it; then net full-potential benefit and the **elimination rate**; then the adoption
adjustment with its source; then net realistic benefit. Beneath it: the investment rule being applied,
the result, and the **breakeven elimination rate** at which the rule just holds. Two integrity checks,
both counts: same-pool claims reconciled against establishment data, and benefits accepted in writing
by the receiving budget holder. A bridge whose eliminations no receiving owner has signed is a
calculation, not a reconciliation.

---

## Exam preparation — Domain 15

**What is assessed.** The project/programme/portfolio distinction by decision rather than scale;
programme architecture in components, tranches and a target operating state; the dependency
multiplication rule and the structural levers; the four portfolio double counts and the benefits
bridge; portfolio balancing under a constraint that binds per period; enterprise capacity as a
measured rate; portfolio work in progress and its cost; protective capacity and its breakeven;
the enterprise PMO's recurring value test; enterprise decision latency and its redesigns; and
correct aggregation of portfolio performance.

**The calculations to be able to do under time pressure.** `Π pᵢ` for a milestone, and the `k`-th
root of a target confidence to get the required per-dependency probability. The effect of decoupling,
as a multiplication by `1/pᵢ`. Per-period feasibility of a candidate set, and enumeration over a small
candidate list. A benefits bridge with all four elimination types, the elimination rate, and simple
payback before and after. `T = W/C` and the excess-work-in-progress cost as benefit run rate ×
excess ÷ throughput. Plan survival as a product over periods, and the breakeven breach cost. Gross
latency-weeks from decision classes and cumulative tier latency `Σ (M/2 + L)`, priced at a cost of
delay, and the saving from a one-week cut per traversal. Portfolio `CPI` and `SPI` from summed `EV`,
`AC` and `PV`, and `EAC = BAC/CPI`.

**The traps.** Taking a milestone probability as the average or the minimum of its predecessors'
rather than their product (Exercise 15.1) · treating aggregate capacity feasibility as feasibility
(Exercise 15.2c, Case study B) · ranking by value per unit of constraint and assuming the result is
optimal when demand is concentrated in the binding period (15.2.3, Exercise 15.2) · computing a
same-pool elimination as the pool's value rather than the excess over it (Exercise 15.3) · omitting
the adoption adjustment or applying it twice (MCQ 15.2-C) · booking eliminated double counts as a
saving (15.2.1, 15.3.4) · assuming a work-in-progress cut reduces throughput (Exercise 15.5) ·
converting a one-off benefit shift into an annuity (MCQ 15.3-B) · justifying a support function on
one-off value (15.3.4) · adding meeting intervals without paper lead times in a multi-tier path
(Exercise 15.4) · applying a per-traversal saving to the decision count instead of the traversal
count (MCQ 15.4-B) · averaging component ratios instead of summing numerators and denominators
(15.4.2) · reporting a single aggregate index with no concentration line · omitting the independence
assumption when a product is printed.

**How the domain connects.** Domain 1 supplies the accountability principle and the cost of delay
every figure here is priced at. Domain 2 supplies the business case, the benefits map, the kill
criteria and the greedy-versus-enumeration result this domain extends to multiple periods. Domain 3
supplies `E[wait] = M/2 + L`, the paper-deadline lever and the tier-summation this domain scales to a
whole decision architecture. Domain 4 supplies the interface arithmetic and the integrated baseline a
programme's components must reconcile to. Domain 6 consumes programme dependency dates as schedule
inputs. Domain 7 supplies the EVM definitions aggregated in KA 15.4.2. Domain 8 supplies merge bias —
the schedule form of this domain's multiplication rule — the common-driver treatment that qualifies
independence, and the methods that size a programme buffer. Domain 12 supplies the coordination cost
of every layer added. Domain 13 supplies Little's Law, applied here to portfolio work in progress.
Domain 14 supplies the AI use register, the verification tier and the information-age discipline.
Domain 16 measures whether the benefits this domain reconciled were realised. PFL-AI Domain 4 supplies
the appraisal methods behind the candidate values.

---

## Domain 15 summary
A portfolio is not a bigger project, because at enterprise scale quantities stop adding. They
multiply, they compete for the same period, and they double-count — and each of those has an
arithmetic this domain computes.

**They multiply.** A milestone requiring all of `k` independent predecessors is met with the
**product** of their probabilities. Meridian's Region A go-live, with six predecessors none worse than
0.85, stands at **52.95 %**; four such regions on one date stand at **7.86 %** — a commitment
**92.1 %** likely to be missed while all twenty-four components report green. Better estimating cannot
fix it: 80 % across 24 dependencies demands **99.07 %** on each. Structure can: decoupling migration
and centralising the information-governance approval took a region to **69.22 %** and the programme
milestone to **22.96 %** for **USD 18,000**, against **USD 240,000** of recovery spend that bought
2.69 points — **USD 1,192** a point against **USD 89,093**, a ratio of **74.7 to one**.

**They compete for the same period.** Meridian's portfolio has 24 units of scarce integration capacity
a year, and the annual view approves a set worth **4,200,000** that demands **9 units in a
6-unit quarter**. Ranking by value per unit is feasible but takes **2,800,000**, because the
highest-ratio candidate consumes five of six units in the binding quarter and excludes the two
largest; enumerating 63 subsets finds **26 feasible** and an optimum of **3,810,000**. The aggregate
illusion overstates by **390,000 (10.24 %)**; the heuristic leaves **1,010,000 (26.51 %)** on the
table. That optimum runs at **95.83 %** planned utilisation and survives all four quarters with
probability **31.56 %**; reserving a unit a quarter lifts survival to **76.31 %** at a cost of
**720,000** of value, which does **not** pay unless a breach costs more than **USD 1,608,744** — a
condition a regulatory or go-live date meets easily and an ordinary quarter does not. And starting
twelve initiatives against a measured throughput of five gives a **2.40-year** cycle time instead of
**1.00**, deferring the portfolio's whole **1,131,480** benefit stream by **1.40 years** —
**USD 1,584,072**, forgone once, invisible in every annual report.

**They double-count.** Five components claiming **1,981,200** at full potential contain **364,800** of
shared, same-pool, cascade and already-committed benefit — an **18.41 %** elimination rate, invariant
to the adoption assumption — leaving **1,131,480** a year net at 70 % adoption. That moves simple
payback from **3.5188** to **4.3129 years** and fails a four-year rule whose breakeven elimination
rate was **12.03 %**. Eliminating a double count creates no value; it prevents a wrong decision, which
is why it cannot appear in the enterprise PMO's value case — a case that here shows a **89,390**
year-one surplus on a one-off and a **476,350** year-two deficit, closable only by capturing
**47.16 %** of the recurring allocation gap.

**And the decisions themselves have a price.** Five tiers, 85 decisions and **468 gross
latency-weeks** cost **USD 1,670,760** a year at Meridian's cost of delay. One week off every paper
deadline saves **517,650 (30.98 %)** and costs nothing; a written-resolution route for the **6**
decisions that carry **30.13 %** of the latency saves **471,240**; removing the fast component-board
tier saves **107,100**. Remove latency where the latency is. Then report it correctly: the same
portfolio reads **CPI 0.95** when aggregated as `ΣEV/ΣAC` and **1.01** as an unweighted mean of
component indices, swinging variance at completion by **USD 286,231** and reversing its sign, because
the component holding **54.67 %** of the cost incurred is the only one in trouble.

The through-line: **project intuitions fail upward in four specific, computable ways — probabilities
multiply, constraints bind per period, benefits overlap, and governance tiers accumulate — and a
leader who computes all four can defend a portfolio to a board, while one who computes none of them
will be asked to commit to a milestone that is 7.86 % likely and to call it green.**
