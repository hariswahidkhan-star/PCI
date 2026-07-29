# Domain 13 — Agile, Adaptive and Hybrid Delivery

> **Group:** Leading people and organisations (Domain 3 of 3 in Part Three — the part's closing
> domain). **Target:** ~70 pages. **Binds to:** the PCI Book Pattern Specification and the shared
> registries (`docs/books/registries/`). This domain closes Part Three by making adaptive delivery
> **arithmetic** rather than allegiance: it consumes Domain 3's governance latency formula, Domain 5's
> scope controls, Domain 6's flow-across-modes translation, Domain 10's risk-allocation frame and
> Domain 12's coordination overhead, and supplies the flow measures that Domains 14–16 report on.
> British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Part Three has been about people: who is engaged (Domain 11), who leads and how a team is built
(Domain 12). Both assumed a **way of working** — a cadence, a unit of delivery, a rhythm at which
decisions arrive — and neither specified it. Part Two specified one: a network, a baseline, a variance.
That specification is right for a great deal of project work and wrong for a great deal more, and the
gap between the two is where this domain lives.

The gap is usually discussed as a preference, which is why the discussion goes nowhere. Practitioners
declare for a method, defend it, and attribute failures to the other camp's contamination. The
professional position is narrower and duller: **adaptive delivery is a set of mechanisms with
measurable properties, and so is predictive delivery, and a leader's job is to know which properties
each supplies and what each costs.** That is the domain's central claim. It has a consequence a
candidate should expect to be tested on: almost every complaint made about agile delivery in large
organisations — "the team keeps changing the date", "we can't get a commitment", "governance is
constantly bypassed", "the contract doesn't fit" — is not a cultural complaint at all. Each is an
arithmetic statement about throughput, work in progress, latency or commercial exposure, and each has
a number attached that nobody has computed.

This domain computes them. KA 13.1 strips product ownership back to the decision right it is and
prices the sequencing decision that prioritisation *is* — worth **USD 155,040** on Meridian's release,
and routinely taken by whoever argues best. KA 13.2 builds the flow arithmetic and applies it to the
commonest management instruction in delivery, *start more work*, which on Meridian buys nothing and
costs **USD 180,379**; it then replaces the forecast date with a forecast range. KA 13.3 quantifies the
mismatch Domain 3 named — a monthly committee governing a fortnightly cadence parks **20.0 %** of a
team's work in progress and makes the affected items take **2.667 times** as long as the rest. KA 13.4
compares a capacity-based and a scope-based commercial model on the only test that discriminates
between them — what a change costs the buyer — and closes on metrics that survive incentives and the
anti-patterns that do not.

The through-line: **adaptive delivery is not a licence to stop measuring; it is a different set of
things to measure, and every one of them is a number.**

**Learning objectives.** After this domain a candidate can: state the conditions under which adaptive
delivery outperforms predictive delivery and the conditions under which it does not, in terms of
feedback cost and requirement volatility rather than preference; specify product ownership as a
decision right with a named holder, and apply the decidability test of Domain 3 to it; rank a backlog
by **delay-cost density** and compute the delay cost of a chosen sequence, its saving against the
intuitive ordering, and the breakeven estimate error that would change the ranking; state and apply
**Little's Law** in both directions — deriving cycle time from work in progress and throughput, and
deriving the throughput implied by a cycle-time change at fixed work in progress; compute what raising
work in progress does to throughput, cycle time, flow efficiency and the delivery date, and price it;
forecast from throughput history **as a range with a stated meaning**, adjusted for discovery
arrivals, and test a committed date against the best rate the team has ever sustained; compute
governance latency against iteration length using Domain 3's `E[wait] = M/2 + L`, express the mismatch
as blocked work in progress and as a cycle-time decomposition, and price the remedy; design hybrid
delivery with a named boundary, two honest control regimes and a translation rule for reporting;
compare capacity-based and scope-based commercial models on buyer exposure under a change, including
the breakeven number of changes; select flow metrics that resist gaming and explain why velocity is a
planning input and never a performance measure; diagnose the standard anti-patterns with their
arithmetic signature; and govern AI-assisted backlog work and forecasting without letting a model's
output become a commitment.

**The master programme.** Meridian Care Records continues from Domains 1–4 and 11–12: the
clinical-records rollout to **40 clinics**, approved cost **USD 2,400,000**, benefit **USD 685,440**
a year at the realistic 70 % adoption (against **USD 979,200** at full potential), and the **cost of
delay of USD 14,280 per week** derived in Domain 1 from 28 adopting clinics at 6 hours a week and
USD 85 an hour. Meridian's shape is exactly the shape this domain is about: an **iteratively developed
records application**, a **sequentially rolled-out** clinic programme with immovable estate and
training dependencies, and a regulatory approval that is a genuine gate (Domain 3, KA 3.1.3). The
application build carries a **240-item** release backlog delivered by a team whose measured throughput
is **6 items a week**, governed by a steering committee whose expected decision latency Domain 3
computed at **4.0 weeks** from `M = 4`, `L = 2`. Those five numbers — 240, 6, 4.0, 14,280 and the
2-week iteration — generate almost every result in this domain. Project Auriga, the 25-week
control-systems upgrade of Domains 6–8, reappears in the exercises where a physical commissioning
queue makes the same flow arithmetic behave differently.

---

## Knowledge Area 13.1 — Agile principles and product ownership

*Topics: 13.1.1 what adaptive delivery actually is · 13.1.2 product ownership as a decision right ·
13.1.3 prioritisation as a priced sequencing decision · 13.1.4 the increment and the definition of
done.*

### 13.1.1 What adaptive delivery actually is

**Definition.** Adaptive delivery is a delivery mode in which **capacity and cadence are fixed and
scope is variable**, direction is corrected from working output at short intervals, and commitment to
detail is deliberately deferred until the cost of deciding is lowest. Predictive delivery fixes scope
and varies capacity and time to achieve it. Neither is a philosophy; each is a choice about **which
variable absorbs uncertainty**.

That framing does the work that value statements cannot. The historical literature of agile methods —
the collaborative statement of values published by a group of software practitioners in 2001, and the
named frameworks that followed it (Scrum, Kanban, DSDM, and the various scaling frameworks) — is
worth reading and is not reproduced here; what a leader needs is the decision rule underneath it. And
the decision rule is economic. Adaptive delivery outperforms predictive delivery when three
conditions hold together, and a leader should be able to test each:

1. **Requirements are genuinely volatile or genuinely unknown.** Not "poorly documented" — unknowable
   in advance, because they depend on what users do with something that does not exist yet. Where
   requirements are stable and knowable, deferring commitment buys nothing and costs coordination.
2. **Feedback is cheap and fast.** An increment can be put in front of someone who can judge it,
   within the iteration, at a cost small relative to the work. Where feedback requires a commissioned
   plant, a regulatory submission or a construction season, the iteration cannot close its loop and
   the method loses its engine (13.A.1).
3. **The cost of a wrong decision falls with delay, or the cost of reversal is low.** Deferring
   commitment is only valuable if information arrives; if the decision is irreversible and the
   information will not improve, deferring it is procrastination with a vocabulary.

**What adaptive delivery does not do.** It does not remove the baseline; it moves it onto capacity,
cadence and the value envelope (Domain 5, KA 5.A.1). It does not remove governance; it changes the
governance question from variance to continuation (Domain 3, KA 3.1.3). It does not remove the date;
it changes what an honest date looks like, from a point to a range with a stated basis (13.2.4). And it
does not make a project cheaper by itself: the case for it rests on **not building the wrong thing**,
which is a value argument — so a programme that adopts adaptive delivery while retaining a fixed scope
has kept the cost and discarded the benefit.

**The honest sentence.** Every method is a rule about which of scope, time and cost floats, and the
useful professional habit is to say out loud which is floating on *this* work: "capacity and date
fixed, scope floats" (adaptive); "scope and date fixed, cost floats" (a crash decision, Domain 6
KA 6.4.2); "scope and cost fixed, date floats" (most infrastructure). An organisation that cannot say
the sentence has not chosen a method; it has chosen to be surprised.

### 13.1.2 Product ownership as a decision right

**Definition.** The product owner is the single individual accountable for the **order** of the
backlog — for deciding what is built next, and therefore what is not built next — within a value
envelope set by the sponsor. It is a decision right, not a facilitation role, and Domain 3's
decidability test (KA 3.1.1) applies to it unchanged: exactly one accountable holder, with sufficient
authority, or the decision is not made but taken by whoever is present.

The role fails in four recognisable ways, and each has a test a leader can run in an afternoon.

**The proxy owner** must consult a committee before ordering the backlog, so the effective authority
and the latency are the committee's — the whole subject of KA 13.3.2. *Test:* count the ordering
decisions made alone in the last quarter, and the elapsed time on the rest.

**The unfunded owner** holds authority over order but not over the value envelope, so they can sequence
but not decline, and backlogs grow monotonically because refusal has no owner. *Test:* count items
removed in the last quarter. A backlog from which nothing is ever removed is a wish list with a
burndown chart.

**The absent owner** is available for ceremonies and unavailable between them, so the team decides by
default and the ordering drifts to whatever is technically convenient. *Test:* the team's median wait
for a product-owner decision — a number worth putting on the wall.

**The specifier owner** writes solutions rather than ordering outcomes, becoming a bottleneck on design
and destroying the team's ability to propose. *Test:* the share of items stating a required outcome and
an acceptance criterion rather than an implementation.

**What the role must be able to do.** Four capabilities, and the absence of any one is a design
defect rather than a personal failing: state the value of an item in the terms of the benefits map
(Domain 2), so that ordering is arguable on evidence; decline an item and record the refusal; accept
or reject an increment against pre-agreed criteria (Domain 5, KA 5.4.2); and escalate a decision that
exceeds their envelope, within a stated latency (Domain 3, KA 3.3.3). Meridian's product owner holds
an envelope of **USD 25,000** per change — the threshold Domain 3's KA 3.2.3 arithmetic recommended —
and the 15 % of items that exceed it are what KA 13.3.2 prices.

### 13.1.3 Prioritisation as a priced sequencing decision

**The definition and the point.** Prioritisation is not ranking by importance; it is choosing a
**sequence**, and a sequence has a computable cost because value forgone accrues per unit of time.
The instrument is **delay-cost density**: the cost of delay of an item divided by the effort required
to deliver it.

```
delay-cost density = cost of delay per week ÷ effort in team-weeks
```

Sequencing in decreasing order of that ratio minimises the total cost of delay across a set of items
delivered one at a time by a single team. This is not a framework opinion but a classical result in
single-machine scheduling theory — the weighted-shortest-processing-time ordering, which minimises
total weighted completion time — and ratio rules of this shape appear under various names in the
scaling frameworks. What matters for a leader is that the rule is *derivable*, so its assumptions are
visible: one delivery resource, items independent, each item releasable on completion, and effort and
cost of delay both estimated. Every one of those assumptions is a place the rule can fail, and 13.1.3
closes by pricing the most important failure.

**Worked example 13.1.3 — sequencing Meridian's four release epics.**

1. **Setup.** Meridian's 240-item release comprises four epics. Domain 2's benefits map apportions all
   six hours per clinic per week of clinical time saved across the four capabilities, so the four
   epics between them carry the whole of the programme's **USD 14,280 per week** cost of delay
   (Domain 1). One team delivers them sequentially; total effort is **34 team-weeks**.

   | Epic | Cost of delay (USD/week) | Share of programme | Effort (team-weeks) | Density (USD/week per team-week) |
   |---|---|---|---|---|
   | E2 clinical notes | 6,120 | 42.9 % | 14 | 437.14 |
   | E1 appointment booking | 4,080 | 28.6 % | 6 | 680.00 |
   | E3 regulatory reporting | 2,720 | 19.0 % | 3 | 906.67 |
   | E4 patient portal | 1,360 | 9.5 % | 11 | 123.64 |

2. **Formula.** Density = cost of delay ÷ effort. Total delay cost of a sequence = `Σ (cost of delay
   of each epic × the week in which that epic completes)`.
3. **Substitution.** Density order is E3, E1, E2, E4, completing in weeks 3, 9, 23 and 34:
   `2,720 × 3 + 4,080 × 9 + 6,120 × 23 + 1,360 × 34`. The intuitive order — largest cost of delay
   first — is E2, E1, E3, E4, completing in weeks 14, 20, 23 and 34:
   `6,120 × 14 + 4,080 × 20 + 2,720 × 23 + 1,360 × 34`.
4. **Result.** Density order **USD 231,880**. Cost-of-delay-first order **USD 276,080**. The density
   order saves **USD 44,200**. The worst available order (E4, E2, E1, E3 — lowest density first)
   costs **USD 386,920**, so the spread between the best and worst sequences of the *same scope, same
   effort and same team* is **USD 155,040**.
5. **Interpretation.** The first thing to take from this is that sequencing is a **USD 155,040
   decision**, and in most organisations it is taken in a room by whoever advocates most effectively,
   with no number in front of anyone. The second is that the intuitive rule — do the biggest thing
   first — is wrong by **USD 44,200**, and wrong for a reason worth internalising: E2 carries the
   largest benefit but also fourteen weeks of effort, during which the three smaller epics each accrue
   their own cost of delay. Doing the cheap, high-value work first is not opportunism; it is
   arithmetic. Third, the ranking is **robust to moderate estimation error**, which is what makes it
   usable with imperfect inputs: E2 would have to carry a cost of delay above `680 × 14 =`
   **USD 9,520** per week — a **55.6 %** increase on its estimate — before it belonged at the front,
   so a leader can defend the order without defending the third decimal place. Fourth, and most
   important, the entire **USD 155,040** is created by **releasability**. If nothing can go live until
   all four epics are complete, every epic completes at week 34 and the total delay cost is
   `14,280 × 34 =` **USD 485,520** regardless of order — so incremental release is worth
   `485,520 − 231,880 =` **USD 253,640** on this release, which is a larger number than the
   sequencing decision it enables and is usually treated as a technical detail. Two cautions. The rule
   assumes independence, and where E1 must precede E3 technically, the dependency binds and the
   sequence is constrained — price the constraint rather than ignoring the rule. And a cost of delay
   estimated from advocacy rather than from the benefits map will produce a confidently wrong order;
   the density is only as good as its numerator, which is why Domain 2's value attribution is a
   prerequisite and not a nicety.

### 13.1.4 The increment and the definition of done

**Definition.** An increment is a slice of the product that is **complete against a pre-agreed
standard** — the definition of done — and therefore capable of being judged, released or rejected on
its own. The definition of done is that standard, written once, applied unchanged to every item.

The increment is the unit that makes adaptive governance possible, because it is the only thing a
governance body can inspect that is not a claim. A percentage complete is a claim; a working, tested,
documented slice of service is evidence. This is why Domain 3's KA 3.1.3 puts the increment review at
the centre of iterative governance and why the definition of done is a **governance artefact** rather
than a team convention: it defines what the word "done" means in every report the programme issues,
and a definition that is quietly relaxed under schedule pressure corrupts every subsequent number.

**What belongs in it.** The standard should be short, binary and testable: the acceptance criteria met
(Domain 5, KA 5.4.2); the tests written and passing; the security and privacy checks completed, which
for Meridian's clinical records is a regulatory obligation and not a preference; the documentation and
operational handover material produced (Domain 16); no known defects above an agreed severity; and the
item demonstrable in an environment representative of production. Each is either true or it is not.

**The two failure modes.** The **relaxed definition** — items counted as done with tests or
documentation deferred — creates a debt that Domain 9's KA 9.4 quantifies as a rework share of
capacity, degrading throughput at the multiplier `1/(1 − r)` established there. The **inflated
definition** — a standard so demanding that no item ever meets it — produces a team with a full board
and no throughput, which the flow measures of KA 13.2 detect immediately as a cycle time rising with
no change in work in progress.

### AI in this KA

**Where it earns its place.** Splitting a large item into candidate thin slices that each satisfy the
definition of done, for the team to accept, amend or reject — a generative task with cheap
verification, because a bad slice is obvious to the people who would build it. Drafting acceptance
criteria from an outcome statement, then being checked against Domain 5's criteria tests. Computing
delay-cost density and the total delay cost of alternative sequences across a large backlog, with the
sensitivity analysis of 13.1.3 swept automatically — deterministic arithmetic over many combinations,
which is exactly what a machine is for and no human has time to do. Detecting backlog items that
duplicate one another or that carry no stated value, which is a text-comparison task humans perform
badly at scale.

**Where it must not go.** It must not set the order. Ordering a backlog allocates the organisation's
capacity between competing claims on it, and that is a decision right held by a named accountable
person under 13.1.2; a model that produces an order has produced a recommendation, and a product owner
who adopts it unexamined has vacated the role rather than automated it. It must not estimate a cost of
delay from plausibility: asked for one, a model will produce a confident number with no provenance,
and because the density ranking is proportional to that numerator the fabrication propagates straight
into a sequencing decision worth six figures. And it must not write the definition of done, which
encodes the organisation's quality and regulatory obligations.

**Verification, concretely.** Every AI-produced sequence is re-derived by hand for the top five items
— five divisions — and the cost-of-delay inputs are traced to a benefits-map line with an owner, not
to a model output. Every AI-drafted acceptance criterion is tested against a real example and a real
counter-example before it is accepted. Where a model has split an item, the team confirms that each
slice independently satisfies the definition of done, because a slice that cannot be released alone
has re-created the big-bang release the arithmetic above shows to be worth **USD 253,640** to avoid.

### Key terms — KA 13.1

| Term | Meaning |
|---|---|
| **Adaptive delivery** | A delivery mode fixing capacity and cadence and varying scope, correcting direction from working output at short intervals. |
| **Hybrid delivery** | A deliberate combination of predictive and adaptive methods under one governance frame, with a named boundary between them. |
| **Product owner** | The single individual accountable for the order of the backlog within a value envelope set by the sponsor. |
| **Value envelope** | The bounded authority within which a product owner may commit capacity without escalation. |
| **Delay-cost density** | Cost of delay per week ÷ effort in team-weeks; sequencing in decreasing order minimises total delay cost. |
| **Increment** | A slice of product complete against the definition of done and therefore judgeable, releasable or rejectable alone. |
| **Definition of done** | The pre-agreed, binary, testable standard that gives the word "done" one meaning across every report. |
| **Releasability** | The property that an increment can go live alone; it is what converts sequencing into value. |

### Sample MCQs — KA 13.1

**MCQ 13.1-A `[13.1.3 · Application]`** Four epics carry costs of delay of 6,120, 4,080, 2,720 and
1,360 per week and efforts of 14, 6, 3 and 11 team-weeks respectively. The sequence that minimises
total delay cost is:
- A. 6,120 first, then 4,080, 2,720, 1,360 — largest cost of delay first
- B. 2,720 first, then 4,080, 6,120, 1,360 — decreasing cost of delay per team-week ✅
- C. 2,720 first, then 4,080, 1,360, 6,120 — shortest effort first
- D. 1,360 first, then 6,120, 4,080, 2,720 — lowest density first

*Rationale:* Densities are 906.67, 680.00, 437.14 and 123.64, so the order is 2,720 → 4,080 → 6,120 →
1,360, costing **231,880** (13.1.3). A is the intuitive largest-first error, costing **276,080** —
44,200 worse. C ranks on effort alone, which promotes the low-value 1,360 epic ahead of the 6,120 one and
costs **280,160**. D reverses the density rule and is the worst available order at **386,920**.

**MCQ 13.1-B `[13.1.3 · Evaluation]`** For the same four epics, if no epic can be released until all
four are complete at week 34, the total delay cost is:
- A. USD 231,880, unchanged — sequencing still helps
- B. USD 485,520, and sequencing is worth nothing ✅
- C. USD 155,040
- D. USD 253,640

*Rationale:* With a single release every epic completes at week 34, so the cost is
`14,280 × 34 =` **485,520** whatever the order — the whole value of sequencing comes from
releasability (13.1.3). C is the best-to-worst sequencing spread; D is the value of releasability
itself, `485,520 − 231,880`.

**MCQ 13.1-C `[13.1.2 · Analysis]`** A named product owner must obtain a steering committee's
agreement before changing the backlog order. The most accurate description is that:
- A. the arrangement strengthens governance by adding scrutiny
- B. the effective decision right, and therefore the latency, belongs to the committee ✅
- C. the product owner remains accountable and the arrangement is sound
- D. the team should escalate less often

*Rationale:* Authority is where the binding agreement sits, so the product owner is a proxy and the
committee's latency applies to every ordering decision (13.1.2, and Domain 3 KA 3.1.1's decidability
test). A confuses scrutiny with authority; C mistakes the title for the right.

**MCQ 13.1-D `[13.1.1 · Analysis]`** A programme adopts adaptive delivery but keeps its scope fixed
by contract. The predictable consequence is that:
- A. delivery becomes faster because iterations are short
- B. it retains the coordination cost of the method and forgoes its benefit, which comes from not
  building the wrong thing ✅
- C. quality improves because increments are tested
- D. the cost of delay falls

*Rationale:* The economic case for adaptive delivery rests on varying scope in response to feedback;
fixing scope removes the mechanism and leaves the overhead (13.1.1).

**MCQ 13.1-E `[13.1.4 · Comprehension]`** A team, under schedule pressure, begins counting items as
done with tests deferred. The measure that detects this soonest is:
- A. velocity, which will fall
- B. the count of items completed per iteration, which will rise
- C. the rework share of capacity in later iterations, which degrades throughput at `1/(1 − r)` ✅
- D. stakeholder satisfaction

*Rationale:* A relaxed definition of done initially *raises* reported completion and later shows as
rework consuming capacity, at Domain 9 KA 9.2.3's multiplier (13.1.4). A and B move the wrong way;
D is a lagging and non-specific signal.

### Self-check — KA 13.1

1. *State the three conditions under which adaptive delivery outperforms predictive delivery.* —
   Requirements genuinely volatile or unknowable; feedback cheap and fast enough to close inside the
   iteration; and deferral genuinely valuable because information arrives or reversal is cheap.
2. *Why does ranking by cost of delay alone give the wrong sequence?* — Because a large, slow item
   blocks several smaller ones that are each accruing their own cost of delay; the correct ranking is
   cost of delay per unit of effort.
3. *What single property creates the value of sequencing, and what happens without it?* —
   Releasability. Without it every item completes at the end and the total delay cost is the same for
   every order — on Meridian, **USD 485,520** instead of **USD 231,880**.

---

## Knowledge Area 13.2 — Backlogs, iteration planning, flow and Kanban

*Topics: 13.2.1 the backlog as an instrument · 13.2.2 iteration planning and the commitment fallacy ·
13.2.3 flow: throughput, work in progress and cycle time · 13.2.4 forecasting from throughput history.*

### 13.2.1 The backlog as an instrument

**Definition.** A backlog is an **ordered** list of items, each **attributed** to a benefit, each
**sized**, and each carrying an acceptance criterion. Remove any one of those four properties and it
stops being an instrument: an unordered backlog is a wish list, an unattributed one is prioritised on
advocacy (Domain 5, KA 5.3.1), an unsized one cannot be forecast, and one without criteria cannot be
accepted.

Three disciplines keep it usable, and each is cheap.

**Refinement is a capacity cost and should be budgeted as one.** Items near the front must be understood
well enough to start; items far back must not be, because elaborating them wastes effort on work that
may never be built and creates a sunk-cost pull towards building it — Domain 6's rolling-wave principle,
applied to a backlog: elaborate to an explicit horizon.

**The value-envelope reconciliation is the creep control.** A requirement count means nothing where the
count is supposed to move, so the adaptive equivalent of Domain 5's creep test is the reconciliation
Domain 5 KA 5.A.1 specifies — attributed benefit of delivered items against the business case's
commitment, period by period. It detects the failure adaptive programmes actually suffer: delivering
steadily against a backlog that has drifted from the benefits it was funded to produce. Case study B is
that failure, costed.

**Discovery arrivals must be counted.** Items found necessary during build are the method working, not
scope creep. The defect is failing to count them, because an uncounted arrival rate makes every forecast
optimistic by exactly its magnitude. Meridian's measured rate is **1.5 items per week**, and KA 13.2.4
shows what ignoring it does to a date.

### 13.2.2 Iteration planning and the commitment fallacy

Iteration planning answers one question — what will the team take on in the next iteration — and it is
routinely corrupted by being asked to answer a different one: what will the team promise. The
difference matters because the two have different arithmetic.

**Capacity is an observation, not an aspiration.** Meridian's team completes **6 items a week**, so a
2-week iteration has a capacity of **12 items**. That is a measurement, and it is the only defensible
input to a plan.

**The commitment fallacy.** Committing to the mean is committing to fail about half the time.
Meridian's last twelve weeks, paired into six iterations, produced **12, 10, 13, 13, 13 and 11** items.
The mean is exactly **12.00**, and **2 of the 6 iterations** — **33.3 %** — delivered fewer than 12. A
team held to the mean therefore misses its commitment in a third of iterations while performing exactly
to its own average, and the organisational response is invariably to question the team rather than the
arithmetic. Committing instead to the **lowest observed total, 10 items**, would have been met in every
observed iteration, at the cost of forecasting less throughput than the team actually has. The
professional resolution is to stop conflating the two numbers: **plan with the mean and commit with
the low end**, and report both. A commitment is a statement about reliability; a forecast is a
statement about expectation; and an organisation that has one number is using it for both purposes and
being misled at least once.

**What the iteration boundary is actually for.** Three things, none of which is a status report: a
**re-ordering opportunity**, the point at which new information legitimately changes the sequence — the
mechanism that makes KA 13.4.1's commercial comparison come out as it does; an **acceptance point**,
where the increment meets the definition of done or does not; and a **continuation decision** in Domain
3 KA 3.1.3's sense — is the next iteration's cost worth its expected value? A programme whose iteration
boundaries produce reports rather than decisions has bought the cadence and left the value on the table.

### 13.2.3 Flow: throughput, work in progress and cycle time

This is the quantitative core of the domain, and it rests on one relationship.

**Definitions.** **Throughput** (`T`) is completed items per unit of time. **Work in progress** (`W`)
is the number of items started and not finished. **Cycle time** (`C`) is the elapsed time from an
item starting to its finishing. For a system at a steady state, **Little's Law** relates the three:

```
W = T × C        equivalently        C = W / T        and        T = W / C
```

Little's Law is a theorem, not a heuristic — it holds for any queueing system in steady state,
independent of the arrival distribution, the service distribution or the queue discipline. Two
consequences follow that make it the most useful single relationship in delivery flow. First, **cycle
time is a consequence of a management decision, not an attribute of the work**: at a given throughput,
the elapsed time an item takes is determined by how much work is in progress alongside it. Second,
because the relationship can be read in either direction, a reduction in cycle time at unchanged work
in progress is **arithmetically identical** to an increase in throughput — which is the identity KA
13.3.2 uses to price a governance change.

**Flow management, and where Kanban fits.** A flow-managed system — the approach the Kanban method
names — does not commit to an iteration's contents at all. Work is made visible, work in progress is
explicitly limited at each stage, new work is **pulled** when capacity frees rather than pushed on a
cadence, and the system is managed by the three measures above rather than by iteration commitments. It
is therefore the natural regime wherever arrivals are genuinely unpredictable — support, operations,
regulatory response — and it can be combined with an iteration cadence, in which case the cadence serves
review and release while the work-in-progress limit governs flow. The arithmetic below applies to both:
Little's Law does not care whether the work was committed or pulled.

**Meridian's team at its working limit.** With `W = 18` items and `T = 6` items a week,
`C = 18 / 6 =` **3.00 weeks**. If a typical item requires about **0.9 weeks** of active work, the
**flow efficiency** — active time as a share of elapsed time — is `0.9 / 3.00 =` **30.0 %**, meaning
seven-tenths of an item's life is spent waiting. That is not unusual and it is the number most worth
knowing, because it says where the improvement is: not in working faster, but in waiting less.

**Worked example 13.2.3 — the instruction to start more work.**

1. **Setup.** Three clinics are waiting on the reporting module. Meridian's steering committee
   instructs the team to start it in parallel with existing work, raising work in progress from
   **18** to **30** items. Nothing else changes: same nine people, same cadence, same definition of
   done. The team's product owner objects and cannot price the objection. Assume the stated switching
   model, calibrated from the team's own records and to be recalibrated in any other setting: at or
   below the capacity point `W* = 18` the team is the constraint, so throughput is `6W/18` and cycle
   time sits on its floor; above it, **each additional concurrent item consumes 2 % of the team's
   productive capacity** in switching, handover and re-familiarisation, so
   `T(W) = 6 × (1 − 0.02 × (W − 18))`. Meridian's cost of delay is **USD 14,280 per week** (Domain 1)
   and the remaining release backlog is **240 items**.
2. **Formula.** `T(W)` as stated; `C = W / T` (Little's Law); time to clear the backlog = backlog ÷
   throughput; cost = additional weeks × cost of delay.
3. **Substitution.** `T(30) = 6 × (1 − 0.02 × 12) = 6 × 0.76`. `C(30) = 30 / 4.56`.
   `240 / 4.56` against `240 / 6.00`.
4. **Result.** Throughput falls from **6.00** to **4.56** items a week (**−24.0 %**). Cycle time rises
   from **3.00** to **6.58 weeks** (**+119.3 %**, a multiple of **2.193**). Flow efficiency falls from
   30.0 % to **13.7 %**. The 240-item backlog goes from **40.00 weeks** to **52.63 weeks**, and the
   extra **12.63 weeks** cost **USD 180,379**.
5. **Interpretation.** The headline is the one every delivery leader should be able to say in a
   sentence: **raising work in progress by 66.7 % bought 0 % more throughput and more than doubled the
   time any individual item takes.** Nothing arrived sooner. The reporting module the committee wanted
   accelerated arrives *later* than it would have if the team had finished its current work first,
   because it is now competing with that work for the same capacity — and so is everything else. The
   arithmetic also explains why the instruction is so persistently attractive: starting work is
   **visible and immediate**, while the cost lands weeks later, distributed across every item, and is
   attributed to the team's pace rather than to the decision. Three professional cautions, each of
   which a reviewer should press on. The 2 % switching coefficient is a **stated assumption calibrated
   locally**, not a law; the *shape* of the result — throughput flat then falling, cycle time rising
   more than proportionally — is robust to the coefficient, but the 180,379 is not, and a paper quoting
   it must quote the assumption beside it. The model is linear and therefore predicts zero throughput
   at `W = 68`, which is obviously false and is a useful reminder that the relationship is only
   calibrated near the observed range: **calibrate, do not extrapolate.** And Little's Law itself
   assumes a steady state, so it describes a team over several iterations and not a single week; a
   leader who applies it to one week's numbers will find it noisy and conclude wrongly that it does not
   work. The practical control that follows is a **work-in-progress limit** — an explicit number,
   visible, changed only deliberately — and the case for it is not tidiness. It is USD 180,379.

> **Fig 13.2.1 — Little's Law and the cost of starting more work.** Two-axis line chart, x-axis work in
> progress `W` from 6 to 42 items. Left axis, cycle time in weeks (brand blue): flat at **3.00** up to
> the capacity point `W* = 18`, then rising to **6.58** at `W` = 30 and **13.46** at `W` = 42. Right
> axis, throughput in items per week (crimson): rising linearly to **6.00** at `W` = 18, then falling
> to **4.56** at `W` = 30 and **3.12** at `W` = 42. A dashed vertical marks `W* = 18` with "starved"
> to its left and "switching losses" to its right; markers annotate both regimes at `W` = 18 and
> `W` = 30; a bracket between them is labelled "+66.7 % WIP bought 0 % more throughput". A side panel
> carries the derived consequence: the 240-item backlog takes **52.63** weeks instead of **40.00**,
> costing **USD 180,379** at 14,280 per week, and the warning that the model reaches zero throughput at
> `W` = 68 and must be calibrated rather than extrapolated. Source: PCI original. Alt text: a rising
> then falling throughput line crossing a flat-then-steeply-rising cycle time line at a marked capacity
> point, showing that work in progress beyond that point lengthens cycle time without adding
> throughput.

### 13.2.4 Forecasting from throughput history

**The principle.** A forecast produced from throughput history is a **range**, and the range means
something precise that must be stated: it is the span of completion times that the team's **own
recent behaviour** would have produced, on the assumptions that the future resembles the sampled
past, that item sizes are drawn from the same distribution, that the team composition is unchanged and
that the arrival of new work continues at the observed rate. It is **not** a confidence interval, it
carries no probability, and presenting it as though it did is a misrepresentation that will be
discovered.

**Why a range and not a date.** A single date drawn from an average is wrong roughly half the time by
construction (13.2.2), and its wrongness is not symmetric in its consequences: the organisation plans
irreversible things — training, clinic closures, communications — against it. A range with a stated
basis lets those things be planned against the slow end and celebrated at the fast end, which is the
only sequence that does not require an apology.

**Worked example 13.2.4 — Meridian's release date, honestly.**

1. **Setup.** Twelve weeks of completions: **5, 7, 4, 6, 8, 5, 7, 6, 5, 8, 6, 5** — total **72**, mean
   **6.00** items a week. Remaining release backlog **240 items**. Measured discovery arrivals
   **1.5 items a week**. The programme plan carries a committed date **34 weeks** away. Cost of delay
   **USD 14,280** per week.
2. **Formula.** Smooth the history into **four-week rolling totals** to remove single-week noise, and
   take the observed minimum and maximum of those as the sustained-rate bounds. Net drain = gross
   throughput − arrival rate. Forecast = remaining backlog ÷ net drain. Required rate = remaining
   backlog ÷ target weeks, plus the arrival rate.
3. **Substitution.** Rolling four-week totals: **22, 25, 23, 26, 26, 23, 26, 25, 24**, i.e. sustained
   rates of **5.50** to **6.50** items a week with a median of **6.25**. Net drains
   `6.50 − 1.5 = 5.00` and `5.50 − 1.5 = 4.00`. Forecasts `240 / 5.00` and `240 / 4.00`. Naive figure
   `240 / 6.00`. Required rate `240 / 34 + 1.5`.
4. **Result.** The naive forecast — mean rate, arrivals ignored — is **40.00 weeks**. At the mean rate
   net of arrivals it is **53.33 weeks**, so ignoring discovery understates the date by **13.33 weeks**
   and **USD 190,400**. The honest range is **48.00 to 60.00 weeks**. The plan's 34 weeks requires a
   net drain of **7.06** items a week and therefore a gross throughput of **8.56** items a week —
   **31.7 %** above the highest four-week rate the team has ever sustained and **42.6 %** above its
   twelve-week mean. Against the fast end of the range the plan is **14.00 weeks** and
   **USD 199,920** optimistic; against the slow end, **26.00 weeks** and **USD 371,280**.
5. **Interpretation.** The decisive sentence is not "the plan is optimistic" but **"the plan requires a
   throughput the team has never achieved, by 31.7 %, in its best four weeks on record"** — and that
   sentence changes a conversation, because it is not an opinion and it does not accuse anyone. Note
   the structure of the error, which is general: two independent optimisms compound. Using the mean
   rather than a range hides a **12-week** spread; ignoring arrivals adds **13.33 weeks** and
   **USD 190,400** on its own, and it is the more damaging of the two because it is invisible — the
   arrival rate appears in no burndown chart, and a team whose gross throughput is perfectly stable
   will still miss a date built on it. Three cautions belong in any paper carrying this range. The
   range is **empirical, not probabilistic**: it says what the last twelve weeks would have produced,
   and if the remaining items are systematically larger than the delivered ones the range is
   optimistic (KA 13.3.4 quantifies exactly that). Four-week smoothing is a **stated choice**; a
   two-week window widens the range and a six-week window narrows it, so the window must be declared
   and held constant rather than selected after seeing the answer. And a range is only honest if the
   organisation is allowed to receive one: where the culture demands a date, the range will be reduced
   to its midpoint by someone, and the professional protection is to publish the required rate — 8.56
   items a week — alongside it, because that number cannot be averaged away.

**What to do with the answer.** The 34-week date is not recoverable by exhortation, and the arithmetic
says so precisely: even the governance remedy of KA 13.3.2, which lifts gross throughput to **7.06**
items a week, leaves **43.17 weeks** once arrivals are subtracted. Meeting 34 weeks would require
arrivals of zero, which for a clinical records build is not a credible commitment. A **40-week**
forecast, by contrast, is achievable and can be stated as a conditional undertaking: it requires the
governance remedy *and* discovery arrivals held at or below **1.06 items a week**, a **29.4 %**
reduction on the observed rate, with a named owner for each condition. That is what a defensible
re-plan looks like — not a new date, but a date with its two conditions attached and both of them
measurable weekly.

### AI in this KA

**Where it earns its place.** Computing flow measures from a work-item history and flagging the
anomalies a human eye misses — items with cycle times several times the median, items that have been
started and abandoned, and the aged work in progress that no report shows. Sweeping the throughput
history across window lengths to show how the range depends on the smoothing choice, which is the
honest way to present 13.2.4's stated choice. Running the arrival-rate arithmetic across scenarios.
Detecting a relaxed definition of done from the pattern of items reopened after acceptance — a
correlation search over a large log, which is genuinely beyond manual capacity.

**Where it must not go.** It must not produce the forecast that goes to the board without the range's
meaning attached, and this is the specific and serious hazard in this KA: a model asked "when will this
finish?" will answer with a date, in confident prose, and that date will be quoted. The failure mode is
not inaccuracy — the arithmetic may be perfect — it is the **silent loss of the basis**, so a range with
four stated assumptions becomes a number with none. It must not be given a target date and asked to
justify it, which is the same fabrication risk in a more flattering costume. And it must not size items,
because sizing is the team's judgement about work it will do and a model's size estimate carries the
authority of a measurement with none of the substance.

**Verification, concretely.** Every forecast that leaves the team carries its window length, its
arrival rate, its assumption list and the required rate implied by any date it is compared against —
and the arithmetic behind all four is reproducible on one page. Little's Law results are checked in
both directions (`C = W/T` and `T = W/C`) as a consistency test, since the two must agree. And any
model-supplied flow anomaly is confirmed against the underlying items before it is raised, because a
false positive here accuses a person.

### Key terms — KA 13.2

| Term | Meaning |
|---|---|
| **Throughput (`T`)** | Completed items per unit of time — a measurement, never an aspiration. |
| **Work in progress (`W`)** | Items started and not finished; a managed quantity with an explicit limit. |
| **Cycle time (`C`)** | Elapsed time from an item starting to finishing; a consequence of `W` at a given `T`. |
| **Little's Law** | `W = T × C` for a system in steady state, independent of arrival and service distributions. |
| **Flow efficiency** | Active work time as a share of cycle time; typically low, and where the improvement lies. |
| **Work-in-progress limit** | An explicit cap on `W`, changed only deliberately; the control the flow arithmetic justifies. |
| **Discovery arrivals** | New items found necessary during build; subtracted from throughput to give the net drain. |
| **Net drain** | Gross throughput minus arrival rate — the rate at which a backlog actually shrinks. |
| **Range forecast** | Backlog ÷ the observed high and low sustained net drains, with its assumptions stated. |
| **Required rate** | Backlog ÷ target weeks, plus arrivals — the throughput a committed date presupposes. |

### Sample MCQs — KA 13.2

**MCQ 13.2-A `[13.2.3 · Application]`** A team completes 6 items a week with 18 items in progress. Its
average cycle time is:
- A. 0.33 weeks
- B. 3.00 weeks ✅
- C. 6.00 weeks
- D. 108 weeks

*Rationale:* `C = W/T = 18/6 = 3.00` weeks (13.2.3). A inverts the ratio; C reads the throughput as a
duration; D multiplies instead of dividing.

**MCQ 13.2-B `[13.2.3 · Analysis]`** The same team is instructed to raise work in progress from 18 to
30 items. Each concurrent item above 18 costs 2 % of capacity. Throughput and cycle time become:
- A. 6.00 items a week and 5.00 weeks
- B. 4.56 items a week and 6.58 weeks ✅
- C. 4.56 items a week and 3.00 weeks
- D. 7.20 items a week and 4.17 weeks

*Rationale:* `T = 6 × (1 − 0.24) = 4.56` and `C = 30/4.56 = 6.58` weeks (13.2.3). A is the common
error of holding throughput constant, giving `30/6 = 5.00`; C forgets that cycle time depends on both;
D assumes work in progress raises throughput.

**MCQ 13.2-C `[13.2.4 · Application]`** A 240-item backlog, sustained rates of 5.5 to 6.5 items a week,
and measured discovery arrivals of 1.5 items a week. The honest forecast range is:
- A. 36.9 to 43.6 weeks
- B. 48.0 to 60.0 weeks ✅
- C. 40.0 weeks
- D. 24.0 to 30.0 iterations, reported as weeks

*Rationale:* Net drains are `6.5 − 1.5 = 5.0` and `5.5 − 1.5 = 4.0`, giving `240/5.0 = 48.0` and
`240/4.0 = 60.0` (13.2.4). A ignores arrivals — the single most common forecasting error here, and it
understates the date by 11 weeks. C is the naive mean-rate point estimate. D is the correct range
expressed in two-week iterations and mislabelled as weeks, which halves it.

**MCQ 13.2-D `[13.2.4 · Evaluation]`** For the same team, a plan commits to 34 weeks. The most useful
single statement to put in front of the sponsor is that the plan requires:
- A. more effort from the team
- B. a gross throughput of 8.56 items a week, 31.7 % above the team's best sustained four-week rate ✅
- C. a throughput of 7.06 items a week
- D. the backlog to be reduced

*Rationale:* `240/34 = 7.06` net, plus 1.5 arrivals gives **8.56** gross, which is 31.7 % above the
6.5 best observed rate (13.2.4). C omits arrivals and so understates the requirement; A and D are
responses, not the diagnosis.

**MCQ 13.2-E `[13.2.2 · Analysis]`** Six iterations delivered 12, 10, 13, 13, 13 and 11 items, a mean
of exactly 12. A team committed to 12 items an iteration will:
- A. meet the commitment, since 12 is the mean
- B. miss it in 33.3 % of iterations while performing exactly to its own average ✅
- C. miss it in 50 % of iterations
- D. exceed it in every iteration

*Rationale:* Two of the six totals are below 12, so the commitment fails a third of the time at
unchanged performance (13.2.2). C assumes a symmetric distribution the data does not have; the
resolution is to plan with the mean and commit with the low end of 10.

**MCQ 13.2-F `[13.2.3 · Comprehension]`** A team's cycle time is 3.00 weeks and a typical item takes
0.9 weeks of active work. Flow efficiency is:
- A. 30.0 % ✅
- B. 70.0 %
- C. 3.33 %
- D. 333 %

*Rationale:* `0.9/3.00 = 30.0 %` (13.2.3). B is the waiting share; C and D invert the ratio. The low
figure is normal and locates the improvement in waiting, not in working faster.

### Self-check — KA 13.2

1. *State Little's Law in all three forms and say what each is used for.* — `W = T × C`, `C = W/T`,
   `T = W/C`: the second derives the elapsed time a management decision about work in progress has
   caused; the third converts a cycle-time improvement into a throughput improvement at unchanged
   work in progress, which is how KA 13.3.2 prices governance.
2. *Why is ignoring discovery arrivals worse than using a mean rather than a range?* — Because it is
   invisible: a team with perfectly stable gross throughput still misses the date, and on Meridian the
   error is **13.33 weeks** and **USD 190,400** on its own.
3. *What must accompany a range forecast for it to be honest?* — The smoothing window, the arrival
   rate, the four assumptions (sampled past, item-size distribution, team composition, arrival
   continuity) and the statement that it is empirical and carries no probability.

---

## Knowledge Area 13.3 — Scaling considerations and hybrid governance

*Topics: 13.3.1 what scaling actually costs · 13.3.2 governance latency against iteration length ·
13.3.3 designing hybrid delivery honestly · 13.3.4 reporting flow to a predictive governance body.*

### 13.3.1 What scaling actually costs

**The principle.** Adaptive delivery scales badly by default and well by design, and the difference is
entirely a matter of how many things must agree. Two costs rise as team count rises, and both have
already been computed in this book, so they are cited rather than re-derived.

**Coordination overhead within and between teams** rises with the square of the number of people
working as one group: Domain 12's KA 12.2.2 computes the fully connected case as `(n − 1)/160` of team
capacity on its stated link-cost assumption, giving **6.875 %** at Meridian's original twelve people
and **24.375 %** if forty were run as one team — against **4.6875 %** for the same forty in five
structured groups of eight, a release of **7.875 full-time equivalents**. **Interface count between
delivery streams** behaves the same way: Domain 4's KA 4.2.3 gives `n(n−1)/2` possible pairwise
interfaces against `n` to an integration layer, so five streams carry **10** potential interfaces
against **5**, and nine carry **36** against **9**.

**What this implies for scaling design** is short, because the arithmetic is decisive. Prefer **fewer,
larger increments of independence** to more teams: a team that can release without agreement costs `n`,
one that must agree with every other costs `n(n−1)/2`. Where teams must integrate, buy the **integration
layer** — an architecture, an interface contract, a shared platform — rather than paying the mesh, and
price it against the interface count it removes (Domain 4's method). And accept the honest consequence:
**scaling frameworks are largely mechanisms for reducing the number of things that must agree**, and any
scaling design should be assessed on exactly that question. A framework adopted without reducing
agreement requirements has added ceremony to a mesh.

**The dependency that matters most is on a shared scarce resource.** Domain 6's KA 6.A.1 drum applies
unchanged: where several streams need the same constrained capability — a security review, one test
environment, one clinical safety officer — that capability sets programme throughput, and every stream's
local flow measures look healthy while the programme's do not. The diagnostic is to compute cycle time
for items **requiring** the shared resource separately, which is the same decomposition KA 13.3.2
performs for governance decisions.

### 13.3.2 Governance latency against iteration length

Domain 3 named this mismatch and left it as a design defect: a monthly steering committee governing
two-week sprints is always behind, so the team either waits or proceeds and seeks retrospective
approval (Domain 3, KA 3.1.3). This topic quantifies it, and the quantification is what converts the
complaint into a proposal.

**The two numbers, and their ratio.** Domain 3's KA 3.2.3 gives the expected wait for a committee
decision as `E[wait] = M/2 + L`: half the meeting interval plus the whole paper lead time. Meridian's
steering committee has `M = 4` and `L = 2`, so `E[wait] =` **4.0 weeks**. The iteration is **2 weeks**.
The ratio is therefore **2.00** — every escalated decision costs **two entire iterations of waiting**,
which is the precise sense in which the governance body cannot keep up with the work it governs. Note
that this is not a criticism of the committee's diligence: the number is a property of two
administrative parameters and would be the same if every member were faultless.

**Worked example 13.3.2 — what governance latency does to Meridian's flow.**

1. **Setup.** Meridian's team runs at `T =` **6 items a week** with `W =` **18** and therefore
   `C =` **3.00 weeks** (13.2.3). Reviewed history shows **15 %** of items require a decision above
   the product owner's **USD 25,000** envelope and therefore go to the steering committee at
   `E[wait] =` **4.0 weeks**. What does that cost in flow terms, and what would a one-week
   out-of-cycle written-resolution route (Domain 3, KA 3.3.3) be worth?
2. **Formula.** Blocked arrival rate = share × throughput. **Blocked work in progress = blocked
   arrival rate × `E[wait]`** — Little's Law applied to the blocked sub-population. Decompose the
   average cycle time: `C = (1 − s) × Cu + s × (Cu + E[wait])`, so `Cu = C − s × E[wait]`. Then
   recompute `C` at the shorter wait and convert to throughput by `T = W / C`.
3. **Substitution.** Blocked arrivals `0.15 × 6 = 0.90` items a week; blocked work in progress
   `0.90 × 4.0`. Unblocked cycle time `3.00 − 0.15 × 4.0`. New average
   `0.85 × 2.40 + 0.15 × 3.40`. New throughput `18 / 2.55`.
4. **Result.** **1.80** of every 12-item iteration requires a committee decision. **3.60 items** of
   work in progress are permanently parked waiting for one — **20.0 %** of the team's entire work in
   progress. Unblocked items take **2.40 weeks**; blocked items take **6.40 weeks**, a multiple of
   **2.667**. With a one-week out-of-cycle route, parked work in progress falls to **0.90 items**
   (**5.0 %**), average cycle time falls from 3.00 to **2.550 weeks** (**−15.0 %**), and at unchanged
   work in progress throughput rises from 6.00 to **7.0588 items a week** — **+17.6 %**.
5. **Interpretation.** Three results are worth stating separately because they persuade different
   audiences. To a delivery manager, **one item in five that the team is holding is not being worked on
   at all** — it is waiting for a room, and no report shows it, because a blocked item and a
   work-in-progress item look identical on a board. To a product owner, **the items that need a
   decision take 2.667 times as long as the items that do not**, which is why the roadmap's high-value
   items — the ones large enough to exceed the envelope — are systematically the late ones; the
   governance design is quietly selecting against value. And to a sponsor, the remedy is a **17.6 %
   throughput increase for the cost of an administrative procedure**, achieved without hiring anyone,
   working faster, or removing any scrutiny that anyone can name — the decision is still made by the
   same authority, merely in writing and within five working days. Note the identity that makes the
   last claim exact rather than rhetorical: at fixed work in progress, a cycle-time reduction *is* a
   throughput increase, because Little's Law can be read in either direction (13.2.3), and
   `18 / 2.55 = 7.0588` is the same operation as `240 / 34`. Two cautions. The 15 % share must come
   from a counted history, not an impression; the whole result scales linearly with it, so a paper
   quoting 17.6 % must show how the share was counted. And the remedy is only available where the
   decision *class* can legitimately be decided out of cycle: a clinical safety approval or a
   regulatory submission cannot be written-resolved, and Domain 3's KA 3.3.1 rule stands — keep the
   gates that carry real optionality, remove the ones that re-approve a decision already taken. The
   general form of the lesson: **governance latency is a flow parameter, and it belongs in the flow
   arithmetic rather than in the culture chapter.**

> **Fig 13.3.1 — Governance latency as a flow parameter.** Two-axis line chart, x-axis governance
> latency `E[wait] = M/2 + L` from 0 to 10 weeks. Left axis (brand blue), average cycle time
> `2.40 + 0.15 × E[wait]` in weeks; right axis (crimson, dashed), blocked work in progress as a share
> of the team's 18 items, `0.9 × E[wait] / 18`. Markers at Meridian's designed **4.0 weeks** — average
> cycle time **3.00 weeks**, **20.0 %** of work in progress parked — and at a **1.0-week**
> written-resolution route — **2.55 weeks** and **5.0 %**. A dashed vertical at 2.0 weeks marks the
> point at which governance latency equals one iteration. An arrow between the two markers is labelled
> "a 1-week out-of-cycle route: cycle time −15.0 %, throughput +17.6 % at unchanged WIP", and a side
> panel records that blocked items take 6.40 weeks against 2.40 for the rest, a multiple of 2.667.
> Both series are linear in `E[wait]`, so the whole cost of a governance cadence is read off one
> horizontal axis. Source: PCI original. Alt text: two rising straight lines against governance
> latency, one showing average cycle time and one the share of work in progress blocked, with markers
> comparing a four-week committee wait against a one-week written-resolution route.

### 13.3.3 Designing hybrid delivery honestly

**Definition.** Hybrid delivery is a deliberate combination of predictive and adaptive methods under
one governance frame, **with a named boundary**. The words that matter are "deliberate" and "named":
most programmes are hybrid by circumstance and manage the whole of it with one control regime, which
means they have silently chosen a method for work it does not suit.

**The three design decisions.** First, **where the boundary runs**. It runs where the three conditions
of 13.1.1 change — where feedback stops being cheap and fast. Meridian's boundary is unusually clear:
the records application is adaptive (feedback within an iteration from clinicians using a working
increment), the clinic rollout is predictive (estate works, training cohorts, immovable dependencies,
feedback measured in months), and the regulatory approval is a genuine gate. Second, **what crosses
the boundary and in what form**. Domain 6's KA 6.4.1 gives the translation rule and it is not repeated
here: a throughput-based forecast enters the network as a **ranged duration on an integration
activity**, and the network returns the **latest acceptable delivery** read from the backward pass.
Third, **which control regime governs which side**, stated explicitly, so that neither side is measured
by the other's instruments — the adaptive side on flow and value delivered, the predictive side on
variance against baseline, and the boundary itself on the integration milestone's date range.

**The four hybrid failure modes**, each with its test. **One regime for both** — a single
percentage-complete report covering an adaptive stream, forcing the team to invent a denominator
(13.3.4); *test:* ask what it is derived from. **A boundary nobody owns** — an integration milestone
with no accountable holder on either side, so it slips without a decision, which is Domain 4's interface
failure in a new costume; *test:* name the owner. **Cadence imposed across the boundary** — the
predictive side's monthly reporting cycle becomes the adaptive side's decision cadence, re-creating
13.3.2's latency; *test:* compare `E[wait]` for each side against its own cycle. **Hybrid as a
euphemism** — a predictive programme that holds stand-up meetings, carrying the cost of both regimes and
the benefit of neither; *test:* ask which variable floats on the adaptive side.

### 13.3.4 Reporting flow to a predictive governance body

A governance body accustomed to variance against baseline will ask an adaptive stream for a percentage
complete, and the team will supply one by dividing items delivered by items known. That number is
almost always wrong in the same direction, and the error is computable.

**Worked example 13.3.4 — the count view and the size view.**

1. **Setup.** **96** of Meridian's 240 release items are complete, delivered over **16 weeks**. On
   re-sizing, the delivered items average **1.2** size units and the remaining **144** average **2.0**
   — the later items being the integration-heavy and regulatory ones deliberately deferred.
2. **Formula.** Percentage complete by count = items done ÷ total items. By size = size delivered ÷
   total size. Remaining duration on the count view = remaining items ÷ item throughput; on the size
   view = remaining size ÷ delivered size rate.
3. **Substitution.** `96/240` against `(96 × 1.2) / (96 × 1.2 + 144 × 2.0)`. Delivered size rate
   `115.2 / 16`. Remaining duration `144 / 6.00` against `288.0 / 7.20`.
4. **Result.** By count the release is **40.0 %** complete; by size, **28.6 %** — an overstatement of
   **11.4 percentage points**. The count view says **24.0 weeks** remain; the size view says **40.0
   weeks**, an understatement of **16.0 weeks** and **USD 228,480**.
5. **Interpretation.** The count view is not a lie and the team is not manipulating anything; it is
   simply the wrong denominator, and it fails in a **predictable direction** because teams
   rationally sequence the tractable work first — which is precisely what 13.1.3's density rule tells
   them to do. So the two disciplines of this domain interact: **optimal sequencing guarantees that an
   item count will overstate progress.** A leader who understands that will report count and size
   together, or report size alone, and will never let an item count travel to a board unlabelled. The
   defensive habit that follows is to report **three numbers and no percentage**: items and size
   delivered against items and size remaining; the net drain and the range it implies (13.2.4); and
   the conditions under which the range holds. Two cautions. Re-sizing remaining work is legitimate and
   must be **dated and recorded**, because a re-size that quietly moves a denominator is
   indistinguishable from a re-baseline without governance (Domain 6, KA 6.A.2). And size units are a
   team-relative measure with no meaning across teams, so they must never be aggregated across streams
   into a programme total — which is the single most common misuse of adaptive metrics at scale, and
   the reason KA 13.4.2 insists on throughput and cycle time as the portfolio-level measures.

### AI in this KA

**Where it earns its place.** Sweeping governance-design options — cadences, paper lead times,
delegation envelopes — and reporting the resulting `E[wait]`, blocked work in progress and throughput
for each, which is deterministic arithmetic over a large combination space. Classifying a decision
history to establish the 15 % share honestly from records rather than impression. Detecting
cross-stream dependencies from work-item text and commit metadata, producing a candidate interface
list for human confirmation against Domain 4's interface register. Reconciling a count view against a
size view automatically each period, so that 13.3.4's gap is a standing number rather than a discovery.

**Where it must not go.** It must not decide which decisions may be taken out of cycle: that is a
risk-appetite and, for clinical or regulatory classes, a legal judgement, and it belongs to the
accountable authority. It must not aggregate size units across teams, however plausible the resulting
programme total looks — the operation is meaningless and a model will perform it without complaint. And
it must not be the source of the hybrid boundary: where feedback is cheap and fast is a matter of fact
about the work, established by people who know the work.

**Verification, concretely.** `E[wait]` is two operations and is reproduced by hand for every design
that reaches a paper. The blocked share is traced to counted decisions with dates in and out. Any
cross-stream dependency a model proposes is confirmed with both stream leads before it enters the
interface register, because a false dependency creates real coordination cost — at Domain 12's rates.

### Key terms — KA 13.3

| Term | Meaning |
|---|---|
| **Blocked work in progress** | Blocked arrival rate × `E[wait]` — the items a team holds but cannot work on, invisible on a board. |
| **Cycle-time decomposition** | Splitting average cycle time into its blocked and unblocked populations to locate the delay. |
| **Out-of-cycle route** | Domain 3's mechanism (KA 3.3.3) applied where governance latency exceeds the iteration length — the specific mismatch KA 13.3 quantifies. |
| **Integration layer** | An architecture or interface contract that replaces `n(n−1)/2` pairwise agreements with `n`. |
| **Hybrid boundary** | The named line where feedback stops being cheap and fast, and the control regime changes. |
| **Integration milestone** | The boundary event where an adaptive stream's ranged forecast enters a predictive network. |
| **Count view / size view** | Progress by item count against progress by size delivered; the first overstates when sequencing is optimal. |

### Sample MCQs — KA 13.3

**MCQ 13.3-A `[13.3.2 · Application]`** A team runs at 6 items a week with 18 in progress; 15 % of
items need a committee decision with `E[wait] = 4.0` weeks. The work in progress parked waiting for a
decision is:
- A. 1.80 items
- B. 3.60 items, 20.0 % of the team's work in progress ✅
- C. 0.90 items
- D. 2.70 items

*Rationale:* Blocked arrivals are `0.15 × 6 = 0.90` a week, and by Little's Law the blocked population
is `0.90 × 4.0 = 3.60` items (13.3.2). A is the blocked items per iteration (`0.15 × 12`), not the
parked population; C is the weekly arrival rate; D applies the 15 % share to the work in progress of 18
instead of to the arrival rate, which omits the wait entirely.

**MCQ 13.3-B `[13.3.2 · Analysis]`** For the same team, average cycle time is 3.00 weeks. The cycle
times of the unblocked and blocked populations are:
- A. 3.00 and 3.00 weeks
- B. 2.40 and 6.40 weeks ✅
- C. 3.00 and 7.00 weeks
- D. 2.55 and 6.55 weeks

*Rationale:* `Cu = 3.00 − 0.15 × 4.0 = 2.40`, and blocked items add the whole 4.0-week wait, giving
6.40 — a multiple of 2.667 (13.3.2). C adds the wait to the average rather than to the unblocked
figure, double-counting the 0.60 weeks the blocked items already contribute; D uses the post-remedy
average.

**MCQ 13.3-C `[13.3.2 · Evaluation]`** Replacing the 4.0-week committee wait with a 1.0-week
written-resolution route changes cycle time to 2.550 weeks. At unchanged work in progress of 18, the
throughput effect is:
- A. none — throughput depends on capacity, not on cycle time
- B. an increase from 6.00 to 7.06 items a week, +17.6 % ✅
- C. an increase of 15.0 %, matching the cycle-time reduction
- D. an increase from 6.00 to 7.20 items a week

*Rationale:* `T = W/C = 18/2.55 = 7.0588`, an increase of 17.6 % (13.3.2). A forgets that Little's Law
reads both ways; C confuses the proportional fall in `C` with the proportional rise in `T`, which are
not equal because the relationship is reciprocal (`1/0.85 = 1.176`, not 1.15); D rounds the cycle time
to 2.50 before dividing, and rounding before the division moves the answer by 0.14 items a week.

**MCQ 13.3-D `[13.3.4 · Analysis]`** 96 of 240 items are done, averaging 1.2 size units, and the 144
remaining average 2.0. Reporting 40.0 % complete:
- A. is correct, since 96/240 = 40 %
- B. overstates progress by 11.4 percentage points, because optimal sequencing delivers the tractable
  items first ✅
- C. understates progress
- D. is correct provided the team's velocity is stable

*Rationale:* By size the release is `115.2/403.2 =` **28.6 %** complete (13.3.4). The direction of the
error is systematic, not accidental: the density rule of 13.1.3 tells teams to do the cheap high-value
work first, which guarantees that a count overstates.

**MCQ 13.3-E `[13.3.1 · Application]`** Nine delivery streams must integrate. The reduction in
potential pairwise interfaces from adopting an integration layer is:
- A. from 36 to 9 ✅
- B. from 81 to 9
- C. from 45 to 9
- D. from 36 to 18

*Rationale:* `n(n−1)/2 = 36` against `n = 9` (Domain 4, KA 4.2.3, cited at 13.3.1). B squares `n`; C
uses `n(n+1)/2`.

### Self-check — KA 13.3

1. *How is blocked work in progress computed, and why does no board show it?* — Blocked arrival rate ×
   `E[wait]`. A board shows an item as in progress whether it is being worked on or waiting for a
   committee, so 20.0 % of Meridian's work in progress is invisible.
2. *Why does an item count systematically overstate progress on an adaptive stream?* — Because optimal
   sequencing delivers the cheap, high-value items first, leaving the larger ones — on Meridian, 40.0 %
   by count against 28.6 % by size.
3. *What is the test for whether a programme is genuinely hybrid rather than nominally so?* — Name the
   boundary, name the person accountable for the integration milestone, and state which variable floats
   on the adaptive side. If nothing floats, it is not adaptive.

---

## Knowledge Area 13.4 — Contracting for adaptive delivery, metrics and anti-patterns

*Topics: 13.4.1 capacity-based against scope-based commercial models · 13.4.2 what the buyer must
control instead of price · 13.4.3 metrics that survive contact with incentives · 13.4.4 the
anti-patterns and their arithmetic signatures.*

### 13.4.1 Capacity-based against scope-based commercial models

**The two models.** A **scope-based** contract fixes a price for a defined body of work; the supplier
bears the risk that the work takes more effort than expected. A **capacity-based** contract buys a
funded, stable delivery capability for a period — a team, at a rate, for an agreed cadence — and the
buyer bears the risk of how much gets done. Domain 10's KA 10.3.1 establishes the general frame: a
contract is a risk allocation, and the only useful question about a mechanism is what it rewards at the
margin. This topic applies that frame to the one test that actually discriminates between the two
models in adaptive delivery, because on expected price at award they can be made identical: **what does
a change cost the buyer?**

**Worked example 13.4.1 — Meridian's build, priced two ways, then changed.**

1. **Setup.** The 240-item release, two commercial offers designed to be equivalent at plan.
   *Scope-based:* **USD 1,200,000** firm for the 240 items — **USD 5,000** an item. *Capacity-based:*
   **USD 60,000** per 2-week iteration for the supplier's squad; the plan of 20 iterations (40 weeks)
   totals **USD 1,200,000**, which at the planned 12 items an iteration is also **USD 5,000** an item.
   At week 14 a regulatory reporting requirement emerges. The buyer wants **30 items added** and 30
   lower-value items **dropped** — a net-zero change in item count. The scope contract's change
   schedule prices additions at **USD 7,500** an item (a 50 % premium on the bid rate, because change
   work is not competed) and credits omissions at **USD 3,000** an item (60 % of the bid rate, the
   supplier retaining margin and recovered overhead). A scope change must go to the change board at
   Domain 3's `E[wait] =` **4.0 weeks**; a capacity re-prioritisation happens at the next iteration
   boundary, an average wait of **1.0 week**. Cost of delay **USD 14,280** per week.
2. **Formula.** Scope exposure = (items added × change rate) − (items dropped × omission credit) +
   `E[wait]` × cost of delay. Capacity exposure = boundary wait × cost of delay. Then the buyer's
   quantity exposure under the capacity model = (worst-case iterations × iteration price) − the fixed
   price. Breakeven change count = quantity exposure ÷ the per-change difference.
3. **Substitution.** Additions `30 × 7,500 = 225,000`; credit `30 × 3,000 = 90,000`; latency
   `4.0 × 14,280 = 57,120`. Capacity `1.0 × 14,280`. Duration range from 13.2.4's honest forecast,
   48 to 60 weeks, i.e. **24 to 30 iterations**: `24 × 60,000` to `30 × 60,000`.
4. **Result.** The net-zero change moves the scope-based price by **USD 135,000** and costs a further
   **USD 57,120** in decision latency: total buyer exposure **USD 192,120** for a change that adds no
   items at all. Under the capacity model the same change costs **USD 14,280** — the one week to the
   next boundary — a difference of **USD 177,840** per change. Against that, the capacity model exposes
   the buyer to duration: **USD 1,440,000 to USD 1,800,000** across the honest range, so worst-case
   quantity exposure above the fixed price is **USD 600,000**. The breakeven is
   `600,000 / 177,840 =` **3.37**, so from the **fourth** material re-prioritisation the capacity model
   is cheaper even at the worst end of the observed throughput range: 4 changes cost the scope buyer
   `1,200,000 + 4 × 192,120 =` **USD 1,968,480** against the capacity model's worst case of
   **USD 1,800,000**. Measured against the capacity model's *expected* cost — 53.33 weeks, 26.67
   iterations, **USD 1,600,000** — the breakeven falls to **2.25** changes.
5. **Interpretation.** The result to carry away is the shape, not the numbers: **a scope-based contract
   converts every change into a priced commercial event with a governance latency attached, and a
   capacity-based contract converts every change into a free re-ordering at the next boundary — while
   transferring the quantity risk to the buyer.** Neither is better; they price different
   uncertainties, and the choice follows from which uncertainty the work actually has. The decisive
   diagnostic is therefore a *count*: how many material re-prioritisations does this work expect? Below
   about three, the fixed price is the cheaper instrument; above it, the capacity model is, and the gap
   widens by **USD 177,840** a change. Four cautions, and they matter more than the arithmetic. First,
   the **asymmetry in the change schedule** is where scope-based exposure really lives: additions at
   7,500 against credits at 3,000 means a net-zero change moves the price by 135,000, and a buyer who
   negotiates the addition rate without negotiating the omission credit has secured half a protection.
   Second, the **fixed price presupposes specifiability**. Where the 240 items genuinely cannot be
   specified at bid — which is the premise of adaptive delivery — a competent supplier prices the
   ambiguity into the bid, so the premium is paid whether or not a change occurs; the observed 1,200,000
   is then not a bargain but a contingency the buyer cannot see or reclaim. Third, the capacity model's
   **600,000 of quantity exposure is real** and it is not managed by the contract; it is managed by the
   continuation decision of 13.4.2, and a buyer who adopts the model without that control has bought
   an open-ended commitment, which is Case study B. Fourth, none of this is a legal analysis: the
   enforceability of change schedules, omission credits and termination-for-convenience rights varies by
   jurisdiction and by contract form, and the arithmetic above informs a negotiation rather than
   settling one.

| Test | Scope-based (firm price) | Capacity-based (funded team) |
|---|---|---|
| Expected price at award | USD 1,200,000 | USD 1,200,000 at plan |
| Buyer exposure to a net-zero change | **USD 192,120** (135,000 price + 57,120 latency) | **USD 14,280** (one week to the boundary) |
| Buyer exposure to throughput risk | none — supplier bears it | **up to USD 600,000** across the 48–60 week range |
| What the buyer must control | change control and the change schedule's asymmetry | the continuation decision and the value envelope |
| Breaks even against the other at | fewer than ~3 material changes | 4 or more material changes (2.25 against expected cost) |

### 13.4.2 What the buyer must control instead of price

Under a capacity model the price is no longer the control, and a buyer who does not replace it has
replaced a bounded commitment with an unbounded one. Three controls do the work, and all three already
exist in this book.

**The continuation decision at a stated cadence.** Domain 3's KA 3.1.3 posture — is the next
increment's cost worth its expected value — applied at a real interval with a real authority to stop.
The mechanism is a **bounded funding envelope**: authority to spend a stated number of iterations,
after which continuation requires an explicit decision. Meridian's would be four iterations —
**USD 240,000** at 60,000 an iteration — which is small enough to be a genuine decision and large
enough not to be a ceremony.

**The value-envelope reconciliation.** Domain 5's KA 5.A.1 test, run every funding cycle: attributed
benefit of delivered items against the business case's commitment. This is the control whose absence
Case study B costs at **USD 3,360,000**, and its price there is **USD 36,000**.

**The exit provision that is actually usable.** A termination or step-down right is only a control if
exercising it leaves the buyer with something: the code, the data, the environments, the documentation
and a supplier obligation to support transition, all of which belong in the definition of done
(13.1.4) rather than in a schedule nobody reads. An exit right without a transferable asset is a
theoretical right, and Domain 10's KA 10.3.3 supplier-governance treatment applies.

### 13.4.3 Metrics that survive contact with incentives

**The principle.** Goodhart's law — that a measure adopted as a target tends to cease being a good
measure — is not a reason to avoid measurement; it is a reason to choose measures whose gaming is
either impossible or visible. Four flow measures satisfy that test and one popular measure does not.

**The four that hold.** **Throughput** — completed items per period, where "completed" means the
definition of done. **Cycle time**, ideally as a distribution rather than a mean, because the tail is
where the failures are and the mean conceals it. **Work in progress**, including its aged tail, which
is where blocked items hide (13.3.2). And **flow efficiency**, which is the only one of the four that
points directly at the improvement. All four are counts of real events with timestamps, and all four
are cross-team comparable, which is what makes them the right measures at portfolio level.

**The one that does not: velocity as a performance measure.** Velocity is throughput weighted by
team-relative size units, and its weights are set by the team being measured. The arithmetic of gaming
it is trivial: Meridian's team delivers 12 items an iteration at an average size of 1.2, so
**14.4 size units**; inflate the sizing by 25 % and the same 12 items become **18.0 size units**, a
reported 25 % improvement with no change whatever in delivery. The inflation is not usually dishonest —
it is the predictable response to being measured on a number one controls — and it is undetectable from
the metric itself. Velocity is therefore a legitimate and useful **planning input inside a team** and
an illegitimate performance measure across teams, and the two uses must be separated explicitly rather
than by convention. Size units must never be aggregated into a programme total (13.3.4).

**What the governance body should actually receive**, and it fits on one page: throughput and its
trend; the cycle-time distribution with its tail; work in progress against its limit, and the blocked
share; the net drain and the range forecast with its conditions (13.2.4); and the value-envelope
reconciliation. Five numbers, none of them a percentage complete, all of them auditable.

### 13.4.4 The anti-patterns and their arithmetic signatures

Each of the recurring failures of adaptive delivery has a signature in the measures above, which is
what makes them diagnosable rather than debatable.

**Adaptive delivery with fixed scope.** The overhead of the method without its benefit (13.1.1).
*Signature:* a backlog from which nothing is ever removed; zero refusals by the product owner.

**Starting more to go faster.** *Signature:* work in progress rising, throughput flat or falling,
cycle time rising more than proportionally — worth **USD 180,379** on Meridian (13.2.3). This is the
most expensive and most common instruction in delivery.

**The proxy product owner.** *Signature:* a median product-owner decision wait of the same order as
the committee's `E[wait]`, and blocked work in progress well above the 5.0 % a delegated envelope
produces (13.1.2, 13.3.2).

**Governing a fortnightly cadence monthly.** *Signature:* **20.0 %** of work in progress parked;
blocked items at **2.667×** the cycle time of the rest (13.3.2).

**Reporting an item count as progress.** *Signature:* a widening gap between the count view and the
size view — **11.4 percentage points** and **16.0 weeks** on Meridian (13.3.4).

**Velocity as a target.** *Signature:* size units per iteration rising while throughput in items is
flat (13.4.2).

**The relaxed definition of done.** *Signature:* completion rising, then reopened items rising, then
throughput falling as rework consumes capacity at Domain 9's `1/(1 − r)` (13.1.4).

**Hybrid as a euphemism.** *Signature:* no named boundary, no floating variable, and an integration
milestone with no accountable owner (13.3.3).

**A capacity contract without a continuation decision.** *Signature:* healthy velocity reports and no
value-envelope reconciliation — Case study B, at **169.2 %** of planned cost for **41.0 %** of
committed benefit.

### AI in this KA

**Where it earns its place.** Modelling the commercial comparison of 13.4.1 across change counts,
change rates, omission credits and throughput ranges, which is exactly the deterministic sweep a
machine should perform and a human should not. Producing the cycle-time distribution and its tail from
a work-item log, including the aged work in progress that no standard report surfaces. Reconciling the
count and size views each period automatically. Screening a supplier's iteration reports for the
signatures above — sizing inflation against flat item throughput, for example — and raising a candidate
finding for a human to confirm.

**Where it must not go.** It must not select the commercial model, set a rate, or draft a change
schedule: those are risk-appetite and legal judgements belonging to the accountable buyer, and Domain
10's KA 10.3 position stands. It must not evaluate individual or team performance from flow data. The
measures of 13.4.2 are properties of a **system**, and applying them to people converts every one of
them into a target and destroys it — the gaming arithmetic above is the demonstration, and a model that
ranks teams on velocity has industrialised the error. And it must not be the source of the anti-pattern
finding itself: a signature is evidence for a conversation, not a verdict.

**Verification, concretely.** Every figure in a commercial comparison is reproduced by hand at one
change count, and the change schedule's rates are read from the contract rather than from a summary.
Every flow anomaly is traced to the underlying items before it is reported. And any model-produced
comparison states its assumed throughput range and its source, because the whole of 13.4.1's conclusion
turns on the 48-to-60-week range and a fabricated range produces a confident and wrong contracting
recommendation.

### Key terms — KA 13.4

| Term | Meaning |
|---|---|
| **Scope-based contract** | A firm price for a defined body of work; the supplier bears effort risk, the buyer pays for change. |
| **Capacity-based contract** | A funded team at a rate for a cadence; the buyer bears quantity risk, change is free at the boundary. |
| **Omission credit** | The rate at which removed scope is credited back — typically below the bid rate, and the asymmetry that prices a net-zero change. |
| **Bounded funding envelope** | Authority to spend a stated number of iterations before continuation requires an explicit decision. |
| **Continuation decision** | The periodic judgement that the next increment's cost is worth its expected value; the capacity model's brake. |
| **Velocity** | Throughput weighted by team-relative size units — a planning input inside a team, never a performance measure. |
| **Flow metric set** | Throughput, cycle-time distribution, work in progress with its blocked and aged shares, flow efficiency. |

### Sample MCQs — KA 13.4

**MCQ 13.4-A `[13.4.1 · Application]`** A firm-price contract of 1,200,000 for 240 items prices
additions at 7,500 and credits omissions at 3,000. The buyer adds 30 items and drops 30. The price
movement is:
- A. zero — the change is net-zero in item count
- B. an increase of USD 135,000 ✅
- C. an increase of USD 225,000
- D. a decrease of USD 90,000

*Rationale:* `30 × 7,500 − 30 × 3,000 = 135,000` (13.4.1). A is the error the item count invites; C
counts only the additions; D only the credit. The asymmetry between the two rates is where the exposure
lives.

**MCQ 13.4-B `[13.4.1 · Evaluation]`** The same change costs 57,120 in change-board latency, against
14,280 under a capacity model; the capacity model's worst-case quantity exposure is 600,000. The number
of material changes at which the capacity model becomes cheaper is:
- A. 2
- B. 3
- C. 4 ✅
- D. it never becomes cheaper

*Rationale:* The per-change difference is `192,120 − 14,280 = 177,840`, and
`600,000/177,840 = 3.37`, so the fourth change tips it: `1,200,000 + 4 × 192,120 = 1,968,480` against
1,800,000 (13.4.1). A and B are below the breakeven; against the *expected* capacity cost of 1,600,000
the breakeven is 2.25, which is a different and clearly-labelled comparison.

**MCQ 13.4-C `[13.4.2 · Analysis]`** A buyer adopts a capacity-based contract and keeps its existing
change-control process as the principal control. The defect is that:
- A. change control is unnecessary in adaptive delivery
- B. price is no longer the binding constraint, so the control must be the continuation decision and the
  value-envelope reconciliation ✅
- C. the iteration price should be renegotiated
- D. the supplier will slow down

*Rationale:* A capacity contract has no per-change price to control, so retaining change control leaves
the quantity exposure unmanaged (13.4.2). A overstates — the change *record* still matters (Domain 3,
KA 3.3.4); the missing controls are the brake and the value test.

**MCQ 13.4-D `[13.4.2 · Application]`** A team delivers 12 items an iteration at an average size of 1.2
units. Sizing is inflated by 25 %. The reported velocity and the actual throughput become:
- A. 18.0 units and 15 items
- B. 18.0 units and 12 items — a 25 % reported improvement with no change in delivery ✅
- C. 14.4 units and 12 items
- D. 14.4 units and 15 items

*Rationale:* `12 × 1.5 = 18.0` units against an unchanged 12 items (13.4.2). This is why velocity is a
planning input inside a team and not a performance measure: the team being measured sets the weights,
and the inflation is invisible in the metric itself.

**MCQ 13.4-E `[13.4.4 · Analysis]`** A supplier's reports show size units per iteration rising steadily
while items completed per iteration are flat. The most likely diagnosis is:
- A. the team is improving
- B. sizing inflation under a velocity target ✅
- C. the definition of done has been relaxed
- D. work in progress has been raised

*Rationale:* Rising units with flat item throughput is the specific signature of sizing inflation
(13.4.2, 13.4.4). A relaxed definition of done would show completion rising then reopened items rising;
raised work in progress would show cycle time rising.

### Self-check — KA 13.4

1. *What single question discriminates between a capacity-based and a scope-based model?* — How many
   material re-prioritisations the work expects. Below about three the fixed price is cheaper; above it
   the capacity model is, by **USD 177,840** a change.
2. *What replaces price as the buyer's control under a capacity contract?* — The continuation decision
   at a bounded funding envelope, the value-envelope reconciliation, and an exit right that leaves a
   transferable asset.
3. *Why must size units never be aggregated across teams?* — They are team-relative weights set by the
   team being measured, so the sum has no meaning; throughput and cycle time are the portfolio-level
   measures.

---

## Advanced topics — Domain 13

### 13.A.1 Adaptive delivery where the increment cannot be released

The honest limit of this domain's methods is reached when condition two of 13.1.1 fails: feedback is
neither cheap nor fast, because the increment cannot be put in front of anyone. A partially commissioned
substation, a half-migrated clinical dataset, a drug trial at interim, an aircraft modification awaiting
airworthiness sign-off — in each case the loop the iteration exists to close cannot close inside it, and
a team running two-week iterations against such work is performing the ceremony of adaptive delivery
without its engine.

What survives, and it is a great deal, is the flow arithmetic. Little's Law does not require
releasability; it requires a steady state. Project Auriga's commissioning queue is the illustration
worked in Exercise 13.1: test packs flow through a single commissioning team, work in progress can be
limited, cycle time can be measured, and raising work in progress damages throughput exactly as it does
on a software team — Auriga's numbers give **4.94 weeks** of delay worth **USD 222,353**. So the
practitioner's rule is to separate the two families of technique rather than adopting or rejecting
them together: **flow control travels everywhere; iterative release travels only where feedback closes.**

What must be replaced is the feedback mechanism: a **representative environment** (a simulator, a pilot
clinic, a digital twin — Domain 14, KA 14.2), a **partial-scope release to a limited population**, or an
explicit acceptance that the work is predictive and should be controlled as such. The failure to avoid
is the third option adopted implicitly — keeping the cadence and the vocabulary while quietly abandoning
the feedback, which is 13.1.1's warning in its most expensive form.

### 13.A.2 AI-assisted adaptive delivery, and the forecast that becomes a commitment

Where AI is used inside the delivery loop — generating candidate backlog items, drafting acceptance
criteria, producing code, forecasting throughput — three obligations arise that existing adaptive
practice does not express, and they must be added deliberately (the same conclusion Domain 3's KA 3.A.2
reaches for governance).

**The definition of done must extend to AI-produced work.** An AI-generated item is not done because it
compiles; it is done when it meets the same binary standard as anything else, including review by a
person who can be answerable for it. The specific hazard is **volume**: a model can produce candidate
items and code faster than a team can review them, which raises work in progress without raising
capacity — and 13.2.3 has already computed what that does. AI-assisted generation without a matched
increase in review capacity is a work-in-progress decision in disguise.

**Throughput history must be segmented when the tooling changes.** A team adopting a substantial new
tool has changed its own service-time distribution, so the twelve-week history of 13.2.4 is no longer a
sample of one system. State the change date, hold both histories, and widen the range until enough
post-change observations exist — rather than claiming the improvement in advance, which is the commonest
error and the one that produces the next missed date.

**The forecast must not become the commitment.** This is the most serious hazard in the domain and it is
sociological rather than technical. A machine-produced forecast arrives with more apparent authority
than a human one: it is precise, it is fast, and it does not hedge unless asked to. A range with four
stated assumptions becomes a date; the date is quoted upward; and by the time it is challenged it has
been repeated in three papers. The countermeasure is procedural and cheap: **no forecast leaves the
team without its required rate attached** (13.2.4's 8.56 items a week), because a required rate cannot be
rounded into a promise — it either exceeds the team's demonstrated capability or it does not, and the
question is answerable by anyone.

### 13.A.3 The reviewer's flow eye

Invariants to test on any adaptive or hybrid delivery design, each cheap and each diagnostic.

Every backlog is **ordered, attributed, sized and criterion-bearing**, and items have been *removed*
from it within the last quarter. Exactly **one** named product owner holds the ordering right, with a
stated value envelope, and the median wait for their decisions is measured. Prioritisation is on
**delay-cost density**, not on cost of delay or effort alone, and the cost-of-delay numerator traces to
a benefits-map line with an owner. The **definition of done is written, binary and unchanged** under
schedule pressure, and reopened-item counts are reported. A **work-in-progress limit exists as a
number**, is visible, and every change to it is a recorded decision. **Throughput, cycle-time
distribution, work in progress with its blocked and aged shares, and flow efficiency** are all
measured; velocity, if used at all, is confined to planning inside one team and never aggregated. Every
date is a **range with its window, its arrival rate and its assumptions stated**, and every committed
date is accompanied by its **required rate**. The **net drain** is computed — arrivals are counted, not
assumed away. `E[wait]` is computed for **each** decision body against **its own** cycle, the blocked
share of work in progress is reported, and out-of-cycle routes exist for every class that may legitimately
use one. Hybrid designs name the **boundary**, name the **integration milestone's owner**, and state
which variable floats on each side. Progress reaches governance as **count and size**, never as an
unlabelled percentage. And the commercial model is matched to the expected **change count**, with the
capacity model's continuation decision and value-envelope reconciliation actually in place — because a
capacity contract without a brake is the most expensive single arrangement in this domain.

---

## Industry variations — Domain 13

- **Healthcare.** Clinical safety and information-governance approvals are decision classes that cannot
  be written-resolved, so 13.3.2's remedy applies to the commercial and functional classes only;
  Meridian's design must delegate those it may and place clinical sign-off with clinical authority
  (Domain 3's healthcare variation). The compensating move is a standing clinical reviewer inside the
  cadence rather than a committee outside it.
- **Financial services and other regulated releases.** Release is gated by change-management and audit
  requirements, so the increment can be *complete* far more often than it can be *released*; the honest
  design separates a fortnightly definition of done from a monthly or quarterly release train, and the
  flow measures must then track both cycle time to done and cycle time to live — the gap between them
  is the regulatory cost, and it should be a stated number rather than a grievance.
- **Construction and infrastructure.** Physical works are predictive and the design and digital
  workstreams around them are frequently adaptive, so these programmes are hybrid by nature and fail at
  13.3.3's boundary rather than inside either regime. Flow control nonetheless travels: snagging,
  commissioning and inspection queues are pure Little's Law problems, and work-in-progress limits on
  them are among the cheapest interventions available (13.A.1).
- **Public sector.** Statutory approvals and annual appropriation cycles set `E[wait]` and the funding
  cadence from outside, so the leader's available levers are the paper lead time and the delegated
  envelope, exactly as Domain 3's public-sector variation concludes. The characteristic risk is the
  capacity contract without a continuation decision, because appropriation encourages spend-to-budget —
  which is Case study B's mechanism.
- **Energy and utilities.** Control-systems and operational-technology work of Auriga's kind cannot
  release increments into a live network at will, so iterations produce tested, staged increments that
  are cut over in windows; the flow measure that matters is the cycle time from "ready to cut over" to
  "cut over", which is usually dominated by outage-window availability rather than by the team.
- **Software, digital and product organisations.** The natural home of the methods and the natural home
  of their anti-patterns: the characteristic failures are velocity as a performance measure across teams
  (13.4.2), scaling by adding teams rather than reducing the number of things that must agree (13.3.1),
  and light governance on genuinely irreversible architectural and data-model choices, which do not
  present as gates (Domain 3's technology variation).

---

## Case study — Domain 13: the parallel start (health, Meridian)

**Situation.** At week 26 of Meridian's application build, three clinics were waiting on the regulatory
reporting module and the programme was under visible pressure. The steering committee, meeting on its
four-week cycle, instructed the team to start the reporting module in parallel with existing work,
taking work in progress from **18** to **30** items. The team's product owner objected. Asked why, they
said the team would slow down, which was heard as a preference. The instruction stood.

**What the arithmetic showed.** The delivery manager computed four numbers before the following
governance review, all from records the programme already held. **The parallel start.** On the team's own
switching coefficient of 2 % of capacity per extra concurrent item, throughput fell from **6.00** to
**4.56** items a week and cycle time rose from **3.00** to **6.58 weeks**; the 240-item backlog moved
from **40.00** to **52.63 weeks**, a cost of **USD 180,379** at the programme's 14,280 a week — and the
reporting module the committee wanted accelerated arrived later than it would have if the team had
finished its current work first. **Governance latency.** With `E[wait] = 4/2 + 2 =` **4.0 weeks**
(Domain 3, KA 3.2.3) and 15 % of items exceeding the product owner's envelope, **3.60 items** — **20.0 %**
of the team's work in progress — were parked waiting for the committee at any moment, and those items
took **6.40 weeks** against **2.40** for the rest. **The forecast.** The plan's 34-week date required a
gross throughput of **8.56** items a week, **31.7 %** above the best four-week rate the team had ever
sustained. **The combined position.** Left as instructed — work in progress at 30, arrivals at 1.5 a
week, governance at four weeks — the net drain was **3.06** items a week and the release would take
**78.43 weeks**. On the corrected design it would take **40.00**. The difference was **38.43 weeks** and
**USD 548,800**.

**How it resolved.** Three changes, none of which reduced any scrutiny anyone could name. The work in
progress limit was set at **18** and made visible, with any change to it recorded as a decision — which
also settled the reporting module, now sequenced rather than parallelised. A **written-resolution route
with a five-working-day turnaround** was adopted for commercial and functional decision classes,
excluding clinical safety and information governance, taking blocked work in progress from 3.60 to
**0.90 items** and average cycle time from 3.00 to **2.55 weeks** — a **17.6 %** throughput increase at
unchanged staffing. And the 34-week date was replaced with a **40-week conditional forecast**: conditional
on the governance route and on discovery arrivals being held at or below **1.06 items a week**, a
**29.4 %** reduction, with a named owner for each condition and both reported weekly.

**What the domain teaches here.** The product owner's objection was correct and unpriced, and an unpriced
objection loses to an urgent instruction every time. The same sentence in two forms: "starting the
reporting module in parallel will slow us down" is a preference, while "it costs USD 180,379, delivers
the reporting module later than sequencing it would, and takes the release from 40 to 52.63 weeks" is a
proposal — and the second was accepted at the first meeting at which it was made. Note also which of the
four numbers moved the room: not the 180,379, which was contested, but the observation that **one item in
five the team was holding was not being worked on at all**. Flow arithmetic persuades because it
describes something the audience can verify by walking to the board.

## Case study B — Domain 13: the capacity contract that lost its brake (public sector)

**Situation.** A public-service digital programme contracted two supplier squads on a capacity basis at
**USD 60,000** per squad per two-week iteration — **USD 120,000** an iteration — with a planned twelve
months of **26 iterations**, or **USD 3,120,000**. The commercial rationale was sound and matched
13.4.1's analysis: the requirement was genuinely unspecifiable at award and material re-prioritisation
was expected. The governance body received an iteration report every cycle showing velocity, items
completed and a burn-up chart. Velocity was stable and slightly improving throughout.

**What had happened.** The contract had been let for capacity and controlled as though it had been let
for scope. There was no bounded funding envelope, so continuation was never a decision — each iteration
followed the last because nothing stopped it. There was no value-envelope reconciliation, so nobody
compared what had been delivered against what the business case had promised. And the reports the body
received measured **activity that was genuinely healthy**: the squads were delivering, the definition of
done was holding, and no anti-pattern of the delivery kind was present. By **iteration 44** the
programme had spent **USD 5,280,000** — **169.2 %** of the planned cost, an overrun of
**USD 2,160,000** — when an assurance review finally ran Domain 5's KA 5.A.1 reconciliation. Against a
business-case benefit commitment of **USD 1,850,000** a year, the attributed benefit of everything
delivered was **USD 758,500** a year: **41.0 %**. The programme had bought **169.2 %** of its cost for
**41.0 %** of its benefit, and had done so while every delivery metric it monitored was green.

**How it resolved.** The reconciliation was made a standing control on a four-iteration cycle, alongside
a **bounded funding envelope of four iterations — USD 480,000** — requiring an explicit continuation
decision by a named authority at each boundary (13.4.2). The divergence between delivered value and
committed benefit had first become computable at around iteration 12; had the two controls been in place
from then, the natural stop point was **iteration 16** at **USD 1,920,000**, so the controls were worth
**USD 3,360,000**. Their cost was three days of analyst time per cycle — about **USD 4,500**, or
**USD 36,000** across the eight cycles — a protection ratio of **93.3 times**. The programme was
re-scoped to the two capabilities carrying most of the attributed benefit and closed nine iterations
later.

**What the domain teaches here.** A capacity-based contract is the right instrument for unspecifiable
work and it has no brake of its own: the price does not stop rising when the value stops arriving,
because price is a function of time and not of scope. Whatever else changes, **a capacity model must be
paired with a continuation decision and a value test, or it is an open-ended commitment with a delivery
report attached.** Note the specific trap, which is the reason this failure is so hard to see from
inside: every metric the governance body received was accurate, favourable and about the wrong thing.
Velocity, items completed and a burn-up chart measure whether a team is delivering; none of them can
measure whether what is being delivered is worth buying, and no amount of improvement in the first
answers the second. That is the whole case for the fifth number on 13.4.2's one-page report.

---

## Executive perspective — Domain 13

What a programme director cannot delegate in this domain:

- **The work-in-progress limit, and the authority to instruct past it.** "Start it in parallel" is the
  most expensive sentence available to an executive in adaptive delivery — **USD 180,379** on Meridian,
  for nothing delivered sooner. If you may override the limit, you own the arithmetic (13.2.3).
- **The product owner's authority, in writing.** One named holder, a stated value envelope, and a
  measured decision wait. A product owner who must consult a committee is a proxy, and the committee's
  latency is now your delivery cycle time (13.1.2, 13.3.2).
- **Your own governance cadence, computed against the work's cadence.** `E[wait] = M/2 + L` against the
  iteration length. Four weeks against a fortnight parks **20.0 %** of a team's work in progress and
  makes the high-value items the late ones (13.3.2).
- **Whether a date is a forecast or a commitment, and which one you are quoting.** Ask for the range,
  the arrival rate and the **required rate**. A committed date requiring 31.7 % more throughput than the
  team's best ever four weeks is not ambitious; it is unevidenced (13.2.4).
- **The commercial model against the expected change count.** Below about three material changes take
  the fixed price; above it take capacity — and if you take capacity, install the continuation decision
  and the value-envelope reconciliation before the first iteration, not after the forty-fourth
  (13.4.1, 13.4.2, Case study B).
- **The refusal to measure people with flow data.** Throughput, cycle time and work in progress are
  properties of a system you designed. Used as individual or team targets they become useless within a
  quarter, and velocity fastest of all — a 25 % sizing inflation reports a 25 % improvement and delivers
  nothing (13.4.2).

---

## Calculation exercises — Domain 13

**Exercise 13.1** Project Auriga's commissioning team completes **5** test packs a week with **20** in
progress. The site manager adds 10 packs, taking work in progress to 30; each pack in progress above 20
costs **1.5 %** of the team's capacity. The remaining queue is **140** packs and Auriga's schedule value
is **USD 45,000** a week (Domain 6, KA 6.4.2). Compute cycle time before and after, and the cost of the
decision.
*Solution.* Before: `C = 20/5 =` **4.00 weeks**. After: `T = 5 × (1 − 0.015 × 10) = 5 × 0.85 =`
**4.25 packs a week**, so `C = 30/4.25 =` **7.06 weeks**. Queue: `140/5 =` **28.00 weeks** against
`140/4.25 =` **32.94 weeks** — a delay of **4.94 weeks** costing **USD 222,353**. Note that the physical
setting changes nothing about the arithmetic: releasability is what does not travel to Auriga, not
Little's Law. Common error: holding throughput constant and reporting cycle time as `30/5 =` **6.00
weeks** with no effect on the queue, which treats work in progress as free and understates both the
cycle time and the whole of the cost.

**Exercise 13.2** A team's last ten weeks of completions are **7, 5, 8, 6, 9, 6, 7, 5, 8, 9**. The
remaining backlog is **180** items, discovery arrivals run at **1.25** items a week, and the cost of
delay is **USD 9,500** a week. Using four-week rolling totals, compute the honest range, the mean-rate
forecast, and the error made by ignoring arrivals.
*Solution.* Total **70**, mean **7.00** items a week. Four-week rolling totals **26, 28, 29, 28, 27, 26,
29** — sustained rates **6.50** to **7.25**. Net drains `7.25 − 1.25 = 6.00` and `6.50 − 1.25 = 5.25`,
giving **30.00** to **34.29 weeks**. At the mean rate net of arrivals, `180/5.75 =` **31.30 weeks**.
Ignoring arrivals gives `180/7.00 =` **25.71 weeks**, understating by **5.59 weeks** and
**USD 53,106**. Common error: forecasting from the single best week — `180/9 =` **20.00 weeks** — which
uses a rate the team has sustained for one week in ten and ignores arrivals as well, compounding two
optimisms into a figure 11 weeks below the fast end of the honest range.

**Exercise 13.3** A board meets every **6** weeks with a **2**-week paper lead time and governs a team
running **3**-week iterations at **10** items a week with **40** items in progress; **12 %** of items
require a board decision. Compute `E[wait]`, the blocked work in progress, the cycle-time decomposition,
and the effect of a one-week written-resolution route.
*Solution.* `E[wait] = 6/2 + 2 =` **5.0 weeks** — **1.67** iterations. `C = 40/10 =` **4.00 weeks**.
Blocked arrivals `0.12 × 10 = 1.20` a week, so blocked work in progress `1.20 × 5.0 =` **6.00 items**,
**15.0 %** of the total. Unblocked `4.00 − 0.12 × 5.0 =` **3.40 weeks**; blocked **8.40 weeks**, a
multiple of **2.471**. With a one-week route, `C = 0.88 × 3.40 + 0.12 × 4.40 =` **3.52 weeks**, a
**12.0 %** reduction, and at fixed work in progress `T = 40/3.52 =` **11.36 items a week**, up
**13.6 %**. Common error: computing blocked work in progress from the iteration length rather than from
`E[wait]` — `1.20 × 3 =` **3.60 items**, **9.0 %** — which understates the parked population by
**40.0 %** and hides most of the problem.

**Exercise 13.4** Three features carry costs of delay of **3,600**, **5,400** and **1,800** a week and
efforts of **4**, **9** and **6** team-weeks. Compute the delay cost of the density order, of the
cost-of-delay-first order and of the shortest-first order, and the estimate error that would change the
ranking.
*Solution.* Densities **900.00**, **600.00**, **300.00**, so the density order is F1, F2, F3, completing
in weeks 4, 13 and 19: `3,600 × 4 + 5,400 × 13 + 1,800 × 19 =` **USD 118,800**. Cost-of-delay-first
(F2, F1, F3), completing in weeks 9, 13, 19: **USD 129,600** — **USD 10,800** worse. Shortest-first
(F1, F3, F2), completing in weeks 4, 10, 19: **USD 135,000** — **USD 16,200** worse. The worst order
(F3, F2, F1) costs **USD 160,200**, a spread of **USD 41,400**. F2 would need a cost of delay above
`900 × 9 =` **USD 8,100** a week — **50.0 %** above its estimate — to belong first, so the ranking is
robust to moderate error. Common error: ranking on effort alone, which looks like a flow heuristic
("smallest first") and is **USD 16,200** worse here than ranking on the ratio; the ratio is the rule and
either component alone is a special case of it.

**Exercise 13.5** A firm price of **USD 900,000** covers **180** items; the alternative is capacity at
**USD 45,000** per two-week iteration over a planned 20 iterations. The change schedule prices additions
at **USD 7,500** and credits omissions at **USD 3,000**; the change board's `E[wait]` is **4.0** weeks
against a **1.0**-week iteration boundary, and the cost of delay is **USD 9,500** a week. A net-zero
change swaps **24** items. The honest duration range is **22 to 27 iterations**. Compute both buyer
exposures and the breakeven change count.
*Solution.* Both models price at **USD 900,000** (`900,000/180 =` **5,000** an item; `45,000 × 20 =`
**900,000**). Scope-based change: `24 × 7,500 = 180,000` less `24 × 3,000 = 72,000`, a net
**USD 108,000**, plus latency `4.0 × 9,500 =` **USD 38,000** — exposure **USD 146,000**. Capacity-based:
`1.0 × 9,500 =` **USD 9,500**. Difference **USD 136,500** a change. Capacity cost range
`22 × 45,000 =` **USD 990,000** to `27 × 45,000 =` **USD 1,215,000**, so worst-case quantity exposure is
**USD 315,000** and the breakeven is `315,000/136,500 =` **2.31** — from the **third** change the
capacity model is cheaper even at the worst end (`900,000 + 3 × 146,000 =` **USD 1,338,000** against
1,215,000). Common error: comparing the change order's **USD 108,000** against zero and concluding the
capacity model is free. It is not free: it transfers up to **USD 315,000** of quantity risk to the
buyer, and that risk is controlled by the continuation decision, not by the contract.

---

## Practitioner's toolkit — Domain 13

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 13.T.1 — Flow sheet (one page, one team, weekly)

Rows, all counts or timestamps and none of them a judgement: **throughput** this week and its four-week
rolling total; **work in progress** against its stated limit, split into *being worked*, *blocked* (with
what each is waiting for) and *aged beyond twice the median cycle time*; **cycle time** as a
distribution — median, 85th percentile and longest — never as a mean alone; **flow efficiency** from a
sampled touch-time estimate; **discovery arrivals** this week and their four-week rate; and the derived
**net drain**. Below the rows, three computed lines: the **range forecast** from the highest and lowest
observed four-week net drains, with the window length stated; the **required rate** implied by any date
the organisation is currently quoting; and `E[wait]` for each decision body the team depends on, with the
blocked share it produces. The sheet's purpose is that every one of these is a number the team can
verify by walking to the board, and a delivery design that cannot fill it in is not being measured.

### Toolkit 13.T.2 — Backlog ordering sheet with delay-cost density

Columns: item or epic · outcome statement · **benefits-map line and owner** (the numerator's provenance —
without it the density is advocacy with arithmetic on top) · cost of delay per week · effort in
team-weeks · **density** · dependency constraints · **releasable alone? (yes/no)** · acceptance
criterion · product-owner decision (order / defer / decline) with date. Two standing checks: the
**breakeven test** for the top three items — the cost-of-delay increase that would reorder them, which
tells you whether the order is robust or fragile; and the **releasability count**, because items that
cannot be released alone forfeit the sequencing value the sheet exists to capture (on Meridian,
**USD 253,640**). One rule of use: the "decline" cell must be used. A sheet with no declined items
records a backlog that only grows, which is 13.4.4's first anti-pattern.

### Toolkit 13.T.3 — Adaptive commercial and continuation control sheet

Part A, **the model choice**: expected count of material re-prioritisations · addition rate and omission
credit under a scope model, with the net movement on a representative net-zero change · `E[wait]` for the
change board · the resulting per-change buyer exposure for each model · the honest duration range and the
quantity exposure it implies under capacity · the **breakeven change count** · the recommendation and the
one assumption it is most sensitive to. Part B, **the brake** (mandatory whenever the model is capacity):
the **bounded funding envelope** in iterations and currency · the named authority for each continuation
decision and the date of the next one · the **value-envelope reconciliation** — attributed benefit of
delivered items against the business-case commitment, with its cycle and its owner · and the exit
provision with the list of assets that must transfer for it to be exercisable. Part B is the artefact
whose absence cost Case study B **USD 3,360,000** at a control cost of **USD 36,000**; it should be
completed before the first iteration is funded, and reviewed at every envelope boundary.

---

## Exam preparation — Domain 13

**What is assessed.** The conditions under which adaptive delivery outperforms predictive delivery, and
which variable floats in each mode; product ownership as a decision right with its four failure modes;
**delay-cost density and the cost of a sequence**, including the role of releasability; the backlog's
four properties and the value-envelope reconciliation; capacity as an observation and the commitment
fallacy; **Little's Law in all three forms**, work-in-progress effects on throughput, cycle time and
flow efficiency, and the price of raising work in progress; **range forecasting from throughput history,
net of discovery arrivals, and the required rate implied by a committed date**; the coordination and
interface arithmetic of scaling (cited from Domains 12 and 4); **governance latency as a flow parameter
— blocked work in progress, the cycle-time decomposition, and the throughput identity at fixed work in
progress**; hybrid design with a named boundary and its four failure modes; the count view against the
size view; **capacity-based against scope-based commercial models on buyer exposure under a change, and
the breakeven change count**; what replaces price as the buyer's control; flow metrics that resist
gaming and why velocity is not one; and the anti-patterns with their arithmetic signatures.

**The calculations to be able to do under time pressure.** `C = W/T`, `T = W/C` and `W = T × C`.
Throughput under a stated switching coefficient, and the resulting cycle time, flow efficiency and
backlog duration, priced at a cost of delay. Delay-cost density, the total delay cost of a sequence
(`Σ` cost of delay × completion week), the saving against an alternative order, and the breakeven
cost-of-delay change that reorders it. Four-week rolling throughput, the net drain, a range forecast,
and the required gross rate implied by a target date. `E[wait] = M/2 + L`, blocked arrival rate, blocked
work in progress, the cycle-time decomposition into blocked and unblocked populations, and the
throughput uplift from a shorter wait at fixed work in progress. Percentage complete by count against by
size, and the remaining duration each implies. Buyer exposure per change under both commercial models,
the quantity exposure of the capacity model, and the breakeven change count.

**The traps.** Holding throughput constant when work in progress rises, so cycle time is computed as
`W/T₀` and the backlog looks unaffected (Exercise 13.1) · forecasting from a mean rather than a range,
and from gross throughput rather than the net drain — the second is worth **13.33 weeks** and
**USD 190,400** on Meridian on its own (13.2.4) · forecasting from the single best week (Exercise 13.2) ·
computing blocked work in progress from the iteration length instead of `E[wait]`, understating it by
40 % (Exercise 13.3) · adding `E[wait]` to the *average* cycle time rather than to the *unblocked*
population, double-counting the blocked contribution (MCQ 13.3-B) · assuming a proportional cycle-time
fall gives an equal proportional throughput rise, when the relationship is reciprocal (MCQ 13.3-C) ·
ranking a backlog on cost of delay alone or on effort alone rather than on the ratio (Exercise 13.4) ·
forgetting that sequencing value requires releasability, so a big-bang release costs **USD 485,520**
whatever the order (MCQ 13.1-B) · reading a net-zero change as a zero-cost change, when the asymmetry
between addition rate and omission credit moves the price by **USD 135,000** (MCQ 13.4-A) · treating a
capacity contract as free of change cost while leaving its quantity exposure unmanaged (Exercise 13.5,
Case study B) · reporting an item count as progress when optimal sequencing guarantees it overstates
(13.3.4) · and aggregating team-relative size units into a programme total (13.4.2).

**How the domain connects.** Domain 1 supplies the cost of delay every result here is priced at and the
accountability principle that keeps a forecast owned by a person; Domain 2 the benefits map that makes
delay-cost density defensible and the kill criteria the continuation decision re-tests; Domain 3
`E[wait] = M/2 + L`, the bounded envelope and the out-of-cycle route, which this domain quantifies rather
than re-derives; Domain 4 the interface arithmetic and the change-control machinery the scope-based model
runs on; Domain 5 the acceptance criteria, value attribution and value-envelope reconciliation; Domain 6
the translation rule that lets a throughput forecast enter a CPM network, and the drum that explains why
a shared scarce resource sets programme throughput; Domain 9 the rework multiplier that prices a relaxed
definition of done; Domain 10 the risk-allocation frame; and Domain 12 the coordination arithmetic that
makes scaling expensive. Forward: Domain 14 takes the flow measures into dashboards and the AI-use
register, Domain 15 aggregates them at portfolio level — throughput and cycle time, never size units —
and Domain 16 receives the definition of done as the handover standard.

---

## Domain 13 summary
Adaptive delivery fixes capacity and cadence and lets scope float; predictive delivery does the reverse;
hybrid delivery does both, on named sides of a named boundary. None of that is a preference, and the
whole of this domain's contribution is to show that the arguments practitioners have about it are
arithmetic arguments in disguise.

Prioritisation is a sequencing decision with a price. Ranked by **delay-cost density** — cost of delay
per team-week — Meridian's four release epics cost **USD 231,880** in delay; ranked by cost of delay
alone, **USD 276,080**; in the worst available order, **USD 386,920**. The sequencing decision is
therefore worth **USD 155,040**, and the ranking is robust to a **55.6 %** error in the largest epic's
estimate. All of that value depends on **releasability**: a single release at week 34 costs
**USD 485,520** whatever the order, so incremental release is worth **USD 253,640** on this release
alone.

Flow rests on one theorem. **Little's Law** — `W = T × C` — says that cycle time is a consequence of a
management decision about work in progress, and that at fixed work in progress a cycle-time reduction
*is* a throughput increase. Meridian's team at `W = 18` and `T = 6` has a cycle time of **3.00 weeks** and
a flow efficiency of **30.0 %**. Instructed to raise work in progress to 30, throughput falls to **4.56**
(**−24.0 %**), cycle time rises to **6.58 weeks** (**+119.3 %**), flow efficiency falls to **13.7 %**,
and the 240-item backlog moves from **40.00** to **52.63 weeks** at a cost of **USD 180,379** — a
**66.7 %** increase in work in progress that bought **0 %** more throughput and delivered nothing sooner.

Forecasts are ranges with stated meanings. Meridian's twelve-week history of **72** completions smooths
into four-week sustained rates of **5.50 to 6.50** items a week; net of **1.5** items a week of discovery
arrivals the honest range is **48.00 to 60.00 weeks**. Ignoring arrivals alone understates the date by
**13.33 weeks** and **USD 190,400**. The plan's committed 34 weeks requires a gross throughput of
**8.56** items a week — **31.7 %** above the best four weeks the team has ever had — which is why the
defensible answer was not a new date but a **40-week** forecast with two named, measured conditions.

Governance latency is a flow parameter. Domain 3's `E[wait] = M/2 + L` gives Meridian's committee
**4.0 weeks**, which is **two entire iterations**; with 15 % of items exceeding the product owner's
envelope, **3.60 items — 20.0 % of the team's work in progress — are parked at any moment**, and blocked
items take **6.40 weeks** against **2.40** for the rest, a multiple of **2.667**. A one-week
written-resolution route takes average cycle time to **2.55 weeks** and, at unchanged work in progress,
throughput to **7.06** items a week: a **17.6 %** increase for the price of an administrative procedure.
Reported to a predictive body, the same release is **40.0 %** complete by item count and **28.6 %** by
size, an overstatement of **11.4 percentage points** and **16.0 weeks** — and the direction is
systematic, because optimal sequencing delivers the tractable items first.

Commercially, the two models price different uncertainties. On Meridian's build both cost
**USD 1,200,000** at plan; a net-zero change of 30 items exposes the scope-based buyer to **USD 192,120**
(a **USD 135,000** price movement from the asymmetry between a 7,500 addition rate and a 3,000 omission
credit, plus **USD 57,120** of decision latency) against **USD 14,280** under capacity — but capacity
carries up to **USD 600,000** of quantity exposure across the honest duration range. The breakeven is
**four** material changes against the worst case and **2.25** against the expected cost. And a capacity
model has no brake of its own: Case study B's programme reached **169.2 %** of planned cost for
**41.0 %** of committed benefit while every delivery metric it monitored was green, and the two controls
that would have stopped it at **USD 1,920,000** cost **USD 36,000** against **USD 3,360,000** protected —
a ratio of **93.3 times**.

The through-line: **adaptive delivery does not remove the numbers, it changes which numbers.** Throughput,
work in progress, cycle time, net drain, blocked share, required rate, delay-cost density, buyer exposure
per change — every complaint about agile delivery in a large organisation is one of these eight quantities
in ordinary language, and a leader who can compute them stops having the argument and starts making the
proposal.
