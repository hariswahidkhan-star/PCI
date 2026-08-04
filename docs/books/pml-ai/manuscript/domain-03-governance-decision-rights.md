# Domain 3 — Governance, Organisation and Decision Rights

## Why this domain exists

Domain 1 established what a project leader is answerable for; Domain 2 established how work gets
chosen and whether the promise was honest. Both assumed something this domain has to supply:
**that someone, somewhere, can actually decide.**

Governance is where most delivery failures are finally located, and almost never in the terms
practitioners use for it. Post-project reviews record "poor governance" as though it were a culture
problem. It is usually a *design* problem, and a measurable one: authority set at the wrong level,
committees meeting on a cadence that cannot keep up with the work, escalation paths whose total
latency nobody has ever added up, and decisions that are made but not recorded, so they are made
again. Each of those has an arithmetic consequence, and this domain computes it. That is the
domain's central claim: **governance is a delivery variable with a price, not a compliance
overhead**, and a leader who cannot price it will be asked to accept a structure that guarantees
delay.

The domain proceeds from purpose to mechanism. KA 3.1 asks what governance is *for*, and separates
it from management, assurance and administration (the three things it is most often confused with)
then works structures across organisational forms and the specific problem of governing iterative
delivery. KA 3.2 covers the two roles that do the deciding: the sponsor, whose obligations are
almost always understated, and the steering body, whose failure modes are predictable; it closes
with decision authorities and thresholds, and prices the cost of setting them too low. KA 3.3 builds
the assurance and gate machinery, designs escalation as a *timed* pathway rather than an
organisation chart, and insists on the decision record; because an unrecorded decision is a memory,
not a decision.

**Learning objectives.** After this domain a candidate can: state what governance is for and
distinguish it from management and assurance; describe governance structures across functional,
matrix, projectised and multi-party organisational forms, and identify the accountability weakness
of each; **price governance by unanimity against a defined majority and show why its cost grows
exponentially in the number of parties**; govern iterative and hybrid delivery without either
abandoning control or destroying cadence, and **compute what share of an iterative stream's decisions
a periodic committee can serve at all**; specify the sponsor role as a set of testable obligations and
**size it in diary hours and decision turnaround**; diagnose the standard steering-committee failure
modes and compute a committee's average, peak and off-peak utilisation; **compute the expected latency
of a committee decision from its meeting interval and paper lead time, and use it to price a
governance design**; set delegation thresholds on value, reversibility and externality, and
demonstrate arithmetically when a threshold is too low; design stage gates, compute whether a gate is
worth its elapsed time, state its breakeven detection rate, and **show how an untracked conditional
pass converts a gate's detection rate into an effective one and can invert its value**; distinguish
the three lines of assurance, **compute residual exposure across an assurance map and reallocate
effort at constant cost**, and stop the lines duplicating; design an escalation path with a stated
total latency, an out-of-cycle mechanism and **a decision action window tested against the remaining
duration**; set objective escalation triggers on named forecast methods; maintain an auditable
decision record, **price its re-decision tax**, and **design a cumulative test that catches clusters
without re-centralising the delegated band**; **specify custody, version integrity, retention and
closure handover for each class of governance record, and say who sets the period**; and govern
AI-assisted governance analysis without delegating any decision to it.

**The master programme, and the master project.** Meridian Care Records continues from Domains 1 and
2: the clinical-records rollout to **40 clinics**, approved cost **USD 2,400,000**, benefits **USD
685,440** per year at the realistic 70 % adoption, and the **cost of delay of USD 14,280 per week**
derived in Domain 1. That last figure is what makes this domain quantitative: every week a decision
waits has a price, and Meridian's governance design spends it without ever appearing in a budget
line. Where governance has to act on a *forecast* rather than on a proposal, the domain uses the
volume's single-project thread, **Project Auriga** (25 weeks, BAC **USD 4,000,000**, and at week 13
the position PV 2,080,000 / EV 1,920,000 / AC 2,120,000 that Domains 6 and 7 develop), because an
escalation trigger has to be written against a forecast method, and 3.3.3 shows that the choice of
method decides whether the trigger fires.

**Reference points.** The international document most often named in connection with this subject is
**ISO 21505**, which addresses guidance on the governance of projects, programmes and portfolios. It
is voluntary guidance describing practice (not legislation, and not a certifiable requirement), and
it obliges nobody of itself unless an organisation, a contract or a regulator adopts it. It is named
here and not reproduced: no clause, table or checklist from it appears in this volume, everything
below is this book's own, and a reader who wants the document should obtain the current edition from
its publisher. Naming it implies no endorsement in either direction. KA 3.3.2 names a second
reference point, an assurance architecture published by a professional body, on the same terms.

---

## Knowledge Area 3.1 — Governance models

*Topics: 3.1.1 what governance is for · 3.1.2 structures across organisational forms · 3.1.3
governance in agile and hybrid environments.*

### 3.1.1 What governance is for

**Definition.** Project governance is the set of **decision rights, accountabilities and
information flows** through which an organisation directs and controls a project: who may decide
what, on whose authority, on what information, by when, and how the decision is recorded.

Every word of that carries weight, and the definition earns its keep mainly by exclusion. Governance
is **not management**: management runs the work inside the authority granted; governance grants,
bounds and withdraws that authority. It is **not assurance**: assurance forms an independent opinion
on whether the work is likely to succeed; governance decides what to do about the opinion. And it is
**not administration**: a portal, a report template and a monthly slide pack are governance
*artefacts*, and an organisation can have all of them and no governance at all, because no one in
the chain can actually decide anything. The confusion is not academic: the commonest remedy applied
to a struggling project is more reporting, which adds administration, consumes the scarce attention
of the people who could decide, and leaves the decision rights exactly where they were.

**What governance exists to produce.** Four things, and a governance design should be testable
against each:

1. **Decidability.** Every decision the project will face has exactly one accountable decision-maker
   with sufficient authority. A decision with two owners is undecidable; one with none is unowned.
   This is a countable property, and KA 3.2 counts it.
2. **Timeliness.** The decision arrives while it can still change the outcome. This is the property
   most governance designs never examine, and KA 3.2 makes it computable.
3. **Legitimacy.** The decision is made by someone whose authority the organisation recognises, on
   information it can rely on, through a process it accepts, so the decision *sticks* rather than
   being relitigated by whoever dislikes it.
4. **Traceability.** The decision, its basis and its date can be reconstructed afterwards. This is
   the property that converts a decision from a memory into an institutional fact (3.3.4).

**The two failure directions.** Governance fails by being too heavy or too light, and both are
common in the same organisation at once: heavy on the small decisions that are easy to control and
light on the large ones that are uncomfortable. Heavy governance shows as latency: escalation for
decisions that could have been delegated, agendas too full to reach the difficult item, and a
reporting burden that consumes the delivery capacity it was meant to protect. Light governance shows
as drift: decisions made by whoever is present, scope and budget changing without a decision-maker,
and a project that no one can stop because no one is entitled to.

The professional discipline is therefore **proportionality, designed deliberately**. A governance
structure is a design artefact with inputs (the project's value, novelty, risk, reversibility and
external exposure) and outputs (tiers, thresholds, cadences, gates). Copying the structure used for
the last programme is inheritance, not proportionality.

**Governance and the leader's accountability.** Domain 1's distinction returns here with force.
Governance can allocate *responsibility* (the doing) freely, and should. It cannot dissolve the
project leader's **obligation to answer**. A leader who says "the steering committee decided" has
described a fact, not a defence: the obligations to have framed the decision honestly, to have
surfaced the material information, to have stated the recommendation and the risk of the
alternative, and to have escalated in time all remain the leader's, and every one of them is
assessable afterwards from the decision record.

### 3.1.2 Structures across organisational forms

The same governance intent produces different structures depending on where authority sits in the
host organisation, and each form has a characteristic weakness a leader should assume is present.

**Functional.** Resources and authority stay with functional line managers; the project leader
coordinates. *Characteristic weakness:* the leader has accountability without authority over the
people doing the work, so every resource conflict is an escalation. The governance countermeasure is
explicit, written resource commitments with named individuals and dated availability, agreed by the
functional manager and treated as breached when unilaterally changed, not a request, a commitment.

**Matrix (weak, balanced, strong).** Authority is shared on a spectrum. *Characteristic weakness:*
**dual reporting without a stated precedence rule**, which does not split authority evenly but
transfers it to whichever manager escalates harder. The countermeasure is a precedence rule written
in advance for the specific conflicts that will arise (who wins on priority of work, on technical
approach, on performance assessment), because a matrix without one is a functional organisation that
holds project meetings.

**Projectised.** The project owns its resources and the leader holds full authority. *Characteristic
weakness:* isolation from the enterprise, locally optimal decisions, divergent standards, and a
benefits chain that ends where the project ends, precisely the failure Domain 2's benefits map
addresses. The countermeasure is enterprise representation in the governance body with a real veto
on the standards that matter, and a receiving-organisation owner for each benefit.

**Multi-party (joint ventures, consortia, public–private).** Two or more organisations with distinct
interests, and often distinct legal duties. *Characteristic weakness:* **governance by unanimity**,
which is indistinguishable from an inability to decide, particularly under stress when interests
diverge. The countermeasures are structural and belong in the agreement, not in a terms of
reference: a defined majority for defined classes of decision, a reserved-matters list that genuinely
requires unanimity and is kept short, a deadlock-breaking mechanism with a deadline, and a single
integrating authority for day-to-day direction. PFL-AI Domain 11 treats the risk-allocation side of
the same problem; the governance side is that a structure where any party can stop everything will
eventually be used that way.

**Unanimity has a price, and it is not linear.** Suppose a multi-party board meets on interval `M`
with paper lead time `L`, and that at any given meeting each of `n` parties independently arrives
able to approve with probability `p`: able, that is, in the practical sense of having its own
internal clearance, its own legal sign-off and no unresolved question. Under unanimity the decision
passes in a cycle with probability `pⁿ`; a failed cycle costs a whole further meeting interval. So

```
E[cycles] = 1 / pⁿ        E[wait] = M/2 + L + (E[cycles] − 1) × M
```

The first expression is the whole argument. Approval probability enters as a **power of the party
count**, so the expected wait rises geometrically as parties are added, and it rises much faster
still as `p` falls, which is exactly what happens under stress, when interests diverge and internal
clearances become slower and more conditional. A defined majority replaces `pⁿ` with the probability
that *enough* parties are ready, which is far less sensitive to both.

**Worked example 3.1.2 — pricing Meridian's consortium board: unanimity against a defined majority.**

1. **Setup.** Meridian's rollout is governed by a three-party board: the health provider, the clinical
   systems supplier and the regional commissioner. It meets every **4 weeks** with a **2-week** paper
   lead time, so a single-authority decision would wait `4/2 + 2 =` **4.0 weeks** (3.2.3). Reviewed
   history of the parties' behaviour on comparable items puts each party's per-cycle readiness at
   `p =` **0.90** in normal conditions and **0.70** once an item is contested. Cost of delay
   **USD 14,280** per week. Compare unanimity with a **two-of-three** defined majority.
2. **Formula.** Unanimity pass per cycle `= pⁿ`. Two-of-three pass per cycle
   `= 3p²(1 − p) + p³`. Then `E[wait] = M/2 + L + (1/pass − 1) × M`, costed at the cost of delay.
3. **Substitution.** Unanimity at `p` = 0.90: `0.90³ = 0.729`, `1/0.729 = 1.3717` cycles,
   `4.0 + 0.3717 × 4`. Majority at 0.90: `3 × 0.81 × 0.10 + 0.729 = 0.243 + 0.729 = 0.972`,
   `1/0.972 = 1.0288` cycles, `4.0 + 0.0288 × 4`. Contested, `p` = 0.70: unanimity
   `0.343`, `2.9155` cycles; majority `3 × 0.49 × 0.30 + 0.343 = 0.784`, `1.2755` cycles.
4. **Result.**

   | Rule | Pass per cycle | Expected cycles | `E[wait]` | Cost |
   |---|---:|---:|---:|---:|
   | Single integrating authority | 1.000 | 1.0000 | **4.0000 w** | **USD 57,120.00** |
   | Two-of-three majority, `p` = 0.90 | 0.9720 | 1.0288 | **4.1152 w** | **USD 58,765.43** |
   | Unanimity, `p` = 0.90 | 0.7290 | 1.3717 | **5.4870 w** | **USD 78,353.91** |
   | Two-of-three majority, `p` = 0.70 | 0.7840 | 1.2755 | **5.1020 w** | **USD 72,857.14** |
   | Unanimity, `p` = 0.70 | 0.3430 | 2.9155 | **11.6618 w** | **USD 166,530.61** |

   Unanimity costs **USD 19,588.48** more than a two-of-three majority per decision in normal
   conditions, and **USD 93,673.47** more once the item is contested.
5. **Interpretation.** Read the two rows that matter together. In normal conditions unanimity looks
   affordable: a **1.37-week** penalty, which is the sort of number a governance review waves
   through as the price of consensus. Under contest it becomes an **11.66-week** wait, a
   **2.13-fold** increase from a change in the parties' readiness of only 0.20, while the majority
   rule moves from 4.12 to 5.10 weeks, a rise of under a week. **The two rules are close in good
   conditions and divergent in bad ones, and governance is only load-bearing in bad ones.** That
   asymmetry is the reason the countermeasure is structural rather than behavioural: nothing about a
   consortium's good intentions in month one predicts its `p` in month fourteen.

   The identity to carry forward is that unanimity's cost is driven by `pⁿ`, so **each additional
   party multiplies the failure probability rather than adding to it.** At `p` = 0.90 the expected
   wait runs 4.94 weeks at two parties, 5.49 at three, 6.77 at five and **8.36 at seven**; and at
   `p` = 0.70 the same series runs 8.16, 11.66, 23.80 and **48.57 weeks**, at which point the
   structure has not slowed the decision, it has removed it. This is why the professional
   prescription is not "avoid unanimity" but **keep the reserved-matters list short**: unanimity is
   the correct rule for a handful of decisions that genuinely should not be taken over a party's
   objection (a change of purpose, a change of contribution, admission of a new member), and
   applying it to the ordinary traffic of a programme imposes `pⁿ` on decisions that never needed
   it.

   Three cautions, each of which a reviewer should raise. **Independence is the model's weak
   assumption.** Parties' positions are correlated: two commissioners aligned against a supplier
   fail together, so the realised pass probability under contest is worse than `pⁿ` in the direction
   of a bloc, and better than `pⁿ` where a single party's readiness genuinely drives the others'.
   State the assumption and treat the figures as a lower bound on the penalty, not a point forecast.
   **`p` is a measured quantity or it is nothing**. It is countable directly from a board's own
   minutes as the share of items approved at first presentation, and a consortium that has never
   counted it is guessing at the cost of its own constitution. And **the deadlock-breaking mechanism
   is what caps the tail**: with an expert-determination or escalation-to-principals route carrying
   a stated deadline, the series above is truncated rather than geometric, which is worth more than
   any improvement in `p`. Deadlock provisions and reserved-matters lists are
   enforceability-sensitive and sit in the shareholders' or consortium agreement rather than in a
   terms of reference; take local counsel on both, since what a deadlock clause can compel varies by
   jurisdiction.

> **Fig 3.1.1 — Why unanimity is a power law.** Line chart. Horizontal axis: number of parties `n`,
> 2 to 7. Vertical axis: expected wait for one decision in weeks, 0 to 50, on a logarithmic scale.
> Two crimson curves for unanimity (`p` = 0.90 rising **4.9383 · 5.4870 · 6.0966 · 6.7740 · 7.5267 ·
> 8.3630** weeks, and `p` = 0.70 rising **8.1633 · 11.6618 · 16.6597 · 23.7996 · 33.9994 · 48.5706**
> weeks) against two flat reference rules in brand blue: a single integrating authority at **4.0000
> weeks** and a two-of-three majority at **4.1152 weeks** (`p` = 0.90) and **5.1020 weeks** (`p` =
> 0.70). The three-party points are ringed and annotated with their costs at USD 14,280 per week:
> unanimity **78,353.91** and **166,530.61**; majority **58,765.43** and **72,857.14**. A footnote
> panel prints `E[wait] = M/2 + L + (1/pⁿ − 1)M` with `M` = 4 and `L` = 2. Source: PCI original. Alt
> text: two steeply rising curves for unanimity against two nearly flat lines for majority and
> single-authority rules, the gap widening sharply as parties are added and widening further at the
> lower readiness probability.

**Programme and portfolio layers.** Where a project sits inside a programme inside a portfolio, each
layer adds a tier of potential escalation, and, unmanaged, adds its latency to every decision that
travels up. KA 3.3 computes what that costs and why the design rule is **the fewest tiers a decision
must legitimately pass through**, with the others informed rather than consulted.

**A note on outsourced delivery.** Where a supplier delivers, governance has to span the contract
boundary, and the boundary is where decision rights are most often left undefined. The specific
questions to settle before mobilisation: who may approve a change (and at what value on each side.
The two thresholds are rarely equal and the mismatch is where change control fails); whose
governance body is authoritative when they disagree; what the supplier is contractually obliged to
escalate and within what period; and what information the client is entitled to, in what form, on
what cadence. Domain 7 handles the commercial mechanics; the governance point is that a contract
that does not allocate decision rights has not allocated the work.

### 3.1.3 Governance in agile and hybrid environments

Iterative delivery does not remove the need for governance; it changes what governance is asked to
decide, and a structure designed for sequential delivery applied unchanged to iterative delivery
fails in a specific, predictable way.

**What changes.** In sequential delivery, governance approves a plan and then controls variance
against it. In iterative delivery the increment is the unit of decision, the backlog is the plan,
and the governance question is not "is the project on plan?" but **"is the value being produced
worth the next increment's cost, and is the direction still right?"** That converts governance from
variance control to **continuation decisions at a cadence**, which is a more honest posture, and
also a more demanding one, because it requires a decision-maker who can say "stop" repeatedly rather
than once at a gate.

**The characteristic failure.** A monthly steering committee governing two-week sprints is *always
behind*, and worse, its latency (computed in KA 3.2) exceeds the cycle it is governing, so the team
either waits, destroying flow, or proceeds and seeks retrospective approval, destroying the
governance. Both outcomes are attributed to the team. Neither is the team's doing.

**The design response**, in four moves:

- **Delegate inside a bounded envelope.** The team decides freely within stated bounds (scope within
  the agreed increment, technical approach within the architecture, spend within a period budget),
  and the governance body sets and reviews the envelope rather than the decisions inside it. This is
  the single highest-value governance decision in iterative delivery, and the worked example below
  shows that it is not an efficiency measure but a **feasibility requirement**.
- **Match cadence to the work.** Governance interaction at the increment's rhythm, increment reviews
  as the decision point, with the committee attending rather than being reported to.
- **Govern outcomes, not activity.** Working software or a released service demonstrated, with the
  benefit measure attached (Domain 2's map), rather than percentage-complete against a plan that the
  method deliberately does not have.
- **Keep the gates that carry real optionality.** Funding tranches, go-live authorisation and
  regulatory approvals remain genuine gates because they are genuinely irreversible. What should be
  removed are the gates that merely re-approve a decision already taken.

**The test that settles the argument.** Envelope width is usually debated as a matter of trust, and
it can be settled as a matter of arithmetic. Each decision an iterative team raises has a **need-by
time**: the point beyond which the answer no longer changes what the team does, because the work has
either been done a different way or abandoned. A periodic body can serve a decision only if its
expected latency is shorter than that decision's need-by time. So the governance question is not
"how much should we delegate?" but **"what share of the stream's decisions can this body serve at
all?"**, and the answer is countable from a single sprint's decision log.

**Worked example 3.1.3 — how much of Meridian's iterative stream can a monthly committee serve?**

1. **Setup.** Meridian's records application is built in **2-week** increments over a **26-week**
   build, so **13** increments. The team's decision log records **5** decisions per increment that
   exceed its current authority, **65** in total. Each was tagged at the time with its need-by time:
   **41** needed within the current increment (≤ 2 weeks), **9** at 2–3 weeks, **7** at 3–4 weeks
   and **8** beyond 4 weeks. The steering committee's expected latency is **4.0 weeks** (`M` = 4,
   `L` = 2). Cost of delay **USD 14,280** per week; **25 %** of escalated decisions sit on the
   rollout critical path.
2. **Formula.** Decisions servable in time = count whose need-by time exceeds `E[wait]`. Servable
   share = that count ÷ total. Minimum envelope coverage = the share whose need-by time is inside the
   increment. Upper-bound wait cost = unservable count × critical-path share × `E[wait]` × cost of
   delay.
3. **Substitution.** At `E[wait]` = 4.0 only the `> 4` bucket is servable: `8/65`. At 3.0, the `3–4`
   bucket joins: `15/65`. At 2.0 (a fortnightly committee with a one-week deadline), the `2–3` bucket
   joins: `24/65`. Minimum envelope `41/65`. Wait cost `57 × 0.25 × 4 × 14,280`.
4. **Result.**

   | Governance design | `E[wait]` | Decisions servable in time | Share |
   |---|---:|---:|---:|
   | Monthly, 2-week papers (as designed) | 4.0 w | 8 of 65 | **12.31 %** |
   | Monthly, 1-week papers | 3.0 w | 15 of 65 | **23.08 %** |
   | Fortnightly, 1-week papers | 2.0 w | 24 of 65 | **36.92 %** |

   **87.69 %** of the stream's decisions cannot be served in time by the committee as designed. If
   the team waited for all 57 of them, the upper bound on delay is `57 × 0.25 = 14.25` decisions
   delaying 4.0 weeks each: **57.00 delay-weeks**, or **USD 813,960**. The envelope must therefore
   cover at least the **63.08 %** of decisions needed inside the current increment before the team
   can keep cadence at all.
5. **Interpretation.** The decisive number is the third column of the table, and specifically what
   happens to it as the cadence is tuned. **Doubling the committee's meeting frequency and halving
   its paper deadline together take servable coverage from 12.31 % to 36.92 %, still under two
   decisions in five.** Cadence tuning cannot solve this problem, because the binding constraint is
   not the committee's speed but the *ratio* of its latency to the increment: at `E[wait]` = 4.0
   weeks against a 2-week increment the ratio is **2.0**, meaning the answer arrives two increments
   after the question, by which time the increment that raised it has shipped. Only delegation
   changes the ratio, which is why the bounded envelope is a feasibility requirement rather than a
   courtesy.

   The **USD 813,960** is an upper bound and should be presented as one, because no team actually
   pays it. What a team does instead is the second failure of 3.1.3: it proceeds and seeks
   retrospective approval. So the honest reading is that the figure is the size of the pressure the
   design creates. And and the *governance* consequence of that pressure is not delay but **the
   quiet transfer of 57 decisions to people who were never given authority over them**, unrecorded,
   and therefore invisible until an audit reconstructs them (Case study B is exactly this mechanism
   at a different scale). A design that cannot serve 87.69 % of its stream's decisions has not
   delegated them; it has lost track of them.

   Three cautions. The upper bound **adds delays that in reality overlap** (two decisions waiting in
   the same four weeks do not cost eight weeks of programme), so the figure bounds the exposure and
   does not forecast it; where a defensible point estimate is needed, model the critical-path
   decisions serially and the rest as concurrent (Domain 6 supplies the machinery). The need-by tags
   are **the team's own judgement recorded at the time**, which is the right source and a biased
   one: a team under pressure to keep flow will tag optimistically, so the tag should be captured
   before the wait, never reconstructed after it. And the residual **8 decisions that the committee
   can serve are the ones it should keep**: they are, by construction, the decisions with time in
   them, and they will tend to be the irreversible and externally visible ones. The envelope's
   boundary is therefore not a value line but a **time-and-reversibility line**, which is the same
   conclusion 3.2.3 reaches from the other direction.

> **Fig 3.1.2 — What share of an iterative stream a periodic committee can serve.** Grouped column
> chart. Horizontal axis: three governance designs, monthly with 2-week papers (`E[wait]` **4.0
> w**), monthly with 1-week papers (**3.0 w**), fortnightly with 1-week papers (**2.0 w**). Vertical
> axis: share of Meridian's 65 above-authority decisions, 0–100 %. Each column split into servable
> in time (brand blue) — **12.31 % · 23.08 % · 36.92 %**, and unservable (slate) — **87.69 % · 76.92
> % · 63.08 %**. A crimson horizontal rule at **63.08 %** marks the minimum envelope coverage the
> same-increment decisions require, annotated "below this line the team cannot keep cadence whatever
> the cadence of the committee". A side panel prints the need-by distribution (41 · 9 · 7 · 8) and
> the upper-bound wait cost **USD 813,960** at the as-designed latency. Source: PCI original. Alt
> text: three columns in which the servable blue portion grows from about one-eighth to just over
> one-third as the committee's latency falls, never reaching the marked minimum-coverage line.

**Hybrid honestly.** Most real programmes are hybrid (an iterative build inside a sequential
infrastructure or regulatory frame), and the governance failure is to apply one model to both parts.
Meridian is exactly this shape: an iteratively developed records application, a sequential clinic
rollout with immovable estate and training dependencies, and a regulatory approval that is a genuine
gate. The workable design governs each part on its own terms and holds a **single integrated view of
the whole** for the decisions that span them, which is the topic of Domain 4.

### AI in this KA

AI is useful in governance analysis and dangerous in governance decisions, and the line is bright.

**Where it earns its place.** Reading a set of terms of reference, delegation schedules and contract
schedules against each other and listing the decisions with no named owner, two owners, or
conflicting thresholds: a document-comparison task at which it is fast and thorough, and one humans
do badly because it is tedious. Extracting the decision log from meeting minutes into a structured
register, flagging items recorded without an owner or a date. Summarising a decision's history for a
board paper. Modelling the latency of a proposed governance design (KA 3.2's arithmetic) across
alternative cadences, decision rules and party counts, which is deterministic and verifiable.
Tagging a sprint's decision log with need-by times drawn from the decision text and computing the
servable share of 3.1.3, which is a counting task nobody performs because it is dull rather than
difficult.

**Where it must not go.** No decision, and no *recommendation presented as a decision*. Governance
authority is conferred on accountable people; it cannot be exercised by a system that cannot be
answerable, and an organisation that lets a model's output stand unchallenged as a governance
conclusion has created accountability without a holder, the exact defect Domain 1 identified.

**The governing principle, applied.** AI proposes; the professional verifies, decides and remains
accountable. Concretely: every AI-produced governance analysis is reviewed by the named accountable
person before it reaches a decision body, its inputs are stated, its conclusions are independently
checked on the material items, and the decision record shows a human decision-maker (never a tool)
as the author of the decision.

### Key terms — KA 3.1

| Term | Meaning |
|---|---|
| **Governance** | The decision rights, accountabilities and information flows through which an organisation directs and controls a project. |
| **Decision right** | The authority to make a specified class of decision, bounded by value, scope and reversibility. |
| **Decidability** | The property that every decision has exactly one accountable decision-maker with sufficient authority. |
| **Reserved matter** | A decision class that requires the highest (often unanimous) authority; kept deliberately short. |
| **Bounded envelope** | Stated limits within which a delivery team decides freely, reviewed rather than approved decision by decision. |
| **Precedence rule** | The pre-agreed rule for which authority prevails in a matrix conflict. |
| **Governance artefact** | A report, portal or template, evidence of governance, never a substitute for decision rights. |
| **Party readiness (`p`)** | The probability that a party arrives at a cycle able to approve; countable as the share of items approved at first presentation. |
| **Need-by time** | The point beyond which a decision no longer changes what the team does; the test of whether a body can serve it. |
| **Envelope coverage** | The share of a stream's above-authority decisions the delegated envelope contains. |
| **Latency-to-cycle ratio** | Governance latency ÷ the delivery cadence it governs; above 1.0 the answer arrives after the increment that asked. |

### Sample MCQs — KA 3.1

**MCQ 3.1-A `[3.1.1 · Comprehension]`** Which statement best distinguishes governance from
management?
- A. governance is performed by senior people and management by junior people
- B. governance grants, bounds and withdraws the authority within which management runs the work ✅
- C. governance is concerned with reporting and management with delivery
- D. governance applies to programmes and management to projects

*Rationale:* The distinction is about authority, not seniority, reporting or scale (3.1.1). C
describes governance artefacts, which an organisation can have in full while having no governance.

**MCQ 3.1-B `[3.1.2 · Analysis]`** A balanced matrix has no written precedence rule for conflicts
between project and functional priorities. The most likely consequence is that authority:
- A. is shared evenly, as intended
- B. transfers to whichever manager escalates harder ✅
- C. defaults to the project leader
- D. defaults to the steering committee automatically

*Rationale:* Undefined precedence does not split authority; it awards it to escalation behaviour
(3.1.2). The countermeasure is a precedence rule written before the conflict arises.

**MCQ 3.1-C `[3.1.3 · Application]`** A monthly steering committee governs two-week sprints. The
predictable failure is that:
- A. the team will produce lower-quality increments
- B. governance latency exceeds the cycle being governed, so the team either waits or proceeds and
  seeks retrospective approval ✅
- C. the committee will meet too often
- D. the sprints must be lengthened to a month

*Rationale:* The mismatch is one of latency against cycle time (3.1.3); the design response is a
bounded envelope and cadence matching, not slower delivery.

**MCQ 3.1-D `[3.1.2 · Analysis]`** In a three-party consortium, the governance provision most
likely to produce paralysis under stress is:
- A. a defined majority for defined decision classes
- B. a short reserved-matters list
- C. unanimity for all substantive decisions ✅
- D. a deadlock-breaking mechanism with a deadline

*Rationale:* Unanimity is indistinguishable from an inability to decide once interests diverge
(3.1.2); the other three are the countermeasures.

**MCQ 3.1-E `[3.1.2 · Application]`** A three-party board meets every 4 weeks with a 2-week paper
lead time. Each party is independently ready to approve in a given cycle with probability 0.70.
Under unanimity, the expected wait for one decision is closest to:
- A. 4.0 weeks
- B. 5.7 weeks
- C. 11.7 weeks ✅
- D. 12.0 weeks

*Rationale:* `p³ = 0.343`, so `E[cycles] = 1/0.343 = 2.9155` and `E[wait] = 4/2 + 2 + 1.9155 × 4 =
11.6618` weeks (3.1.2). A is the single-authority wait, ignoring the failure probability altogether.
B applies the readiness probability once rather than to the power of three (`1/0.70 = 1.4286` cycles
→ 5.71 weeks): the commonest error, because it treats the board as one actor. D rounds the 2.9155
expected cycles up to 3 whole cycles and so adds two whole meeting intervals (`4 + 2 × 4 = 12.0`)
instead of the expected 1.9155 of them.

**MCQ 3.1-F `[3.1.3 · Evaluation]`** A monthly committee with a 2-week paper deadline governs 2-week
increments. Of 65 above-authority decisions, 41 are needed within the current increment, 9 at 2–3
weeks, 7 at 3–4 weeks and 8 beyond 4 weeks. Management proposes meeting fortnightly with a one-week
deadline. The strongest evaluation of that proposal is that it:
- A. solves the problem, because the committee will then be faster than the increment
- B. raises servable coverage from 12.31 % to 36.92 % and therefore still leaves the majority of
  decisions unservable — only delegation changes the outcome ✅
- C. fails, because committee latency is irrelevant to iterative delivery
- D. raises servable coverage to 63.08 %, which is the minimum required

*Rationale:* At `E[wait]` = 2.0 weeks the servable buckets are the 8, 7 and 9, i.e. 24 of 65 = 36.92
%, against 8 of 65 = 12.31 % as designed (3.1.3). A mistakes latency *equal* to the increment for
latency shorter than it: at `E[wait]` = 2.0 against a 2-week increment the latency-to-cycle ratio is
1.0, and the 41 same-increment decisions need their answer *inside* the increment, not at the end of
it. C overcorrects: latency matters, it is simply not sufficient. D misreads 63.08 %; that is the
minimum *envelope coverage* the same-increment decisions require, not a coverage the committee
achieves.

### Self-check — KA 3.1

1. *Name the four things a governance design must produce.* — Decidability, timeliness, legitimacy,
   traceability.
2. *Why is "more reporting" the wrong remedy for a struggling project's governance?* — It adds
   administration and consumes decision-makers' attention while leaving decision rights unchanged.
3. *What does a project leader still owe after a steering committee decides badly?* — The
   obligations to have framed the decision honestly, surfaced the material information, stated the
   recommendation, and escalated in time: all assessable from the record.
4. *Why does unanimity fail suddenly rather than gradually?* — Because pass probability is `pⁿ`:
   each party multiplies the failure probability, so a modest fall in readiness produces a large
   rise in expected wait (3.1.2, 5.49 weeks at `p` = 0.90, 11.66 at `p` = 0.70, three parties).
5. *What makes a bounded envelope a feasibility requirement rather than an efficiency measure?* —
   Governance latency of 4.0 weeks against a 2-week increment gives a latency-to-cycle ratio of 2.0, so
   87.69 % of Meridian's stream decisions cannot be served in time at any realistic cadence (3.1.3).

---

## Knowledge Area 3.2 — Sponsorship and steering

*Topics: 3.2.1 the sponsor role · 3.2.2 steering committees that work · 3.2.3 decision authorities
and thresholds.*

### 3.2.1 The sponsor role

**Definition.** The sponsor is the individual accountable for the project's **business outcome**:
for its being worth doing, remaining worth doing, and being adopted by the organisation that must
use it. The project leader is accountable for delivery within the mandate; the sponsor is
accountable for the mandate.

That is a demanding role and it is routinely treated as an honorific. The remedy is to state it as
**testable obligations**, each of which can be evidenced or found absent:

| Obligation | Evidence it is being met |
|---|---|
| Own the business case | The sponsor can state the benefit, its measure and its owner without notes (Domain 2). |
| Secure and defend funding | Funding decisions carry the sponsor's name; funding challenges are answered by the sponsor, not the project leader. |
| Decide within authority, promptly | Decisions escalated to the sponsor have recorded dates in and out; the interval is monitored. |
| Own the receiving organisation's readiness | The enabling change is resourced and progressing (Domain 2's omitted column). |
| Resolve conflicts between functions | Escalations that are genuinely inter-functional stop at the sponsor rather than travelling upward. |
| Hold the project leader to account, and support them | Recorded, dated performance conversations; and visible backing when the project is under pressure. |
| Be willing to stop it | Kill criteria agreed in advance and revisited at gates (Domain 2, KA 2.4). |

**The sponsor failure modes**, in descending order of frequency and each with its detection test.
*The absent sponsor*, a name on a chart, no diary time; test: the count of sponsor decisions in the
last quarter, and their turnaround. *The delegating sponsor*, a deputy attends everything, so
authority is in the room but accountability is not; test: whether the last three material decisions
were made by the sponsor or reported to them afterwards. *The advocate sponsor*, committed to the
project rather than the outcome, and therefore unable to stop it; test: whether they can state a
condition under which they would recommend stopping (Domain 2's escalation of commitment, at the
governance level). *The operational sponsor*, drawn into managing delivery, which vacates the role
that only they can hold; test: whether the sponsor's contributions in the last three meetings were
about outcome and mandate, or about task sequence.

**The absent sponsor is a capacity problem before it is a commitment problem.** "No time" is the
universal explanation for the first failure mode and it is almost never tested, which is
unfortunate, because the role's time demand is a small sum that anyone can do; and doing it changes
the conversation from an accusation about priorities into a question about portfolio load.

**Worked example 3.2.1a — what Meridian's sponsor role actually costs in diary time.**

1. **Setup.** The seven obligations above imply a specific set of commitments for Meridian: **13**
   steering meetings a year at **2 hours** (the 4-weekly cadence of 3.2.2); **22** sponsor-level
   decisions a year at **1.5 hours** each of reading, consultation and decision; **4** gate reviews at
   **4 hours**; **12** monthly one-to-one sessions with the project leader at **1 hour**; and enabling-
   change oversight with the receiving organisation at **2 hours** a month. Assume **46** working weeks
   and a nominal **40-hour** week.
2. **Formula.** Annual hours `= Σ (frequency × duration)`. Weekly load = annual hours ÷ working weeks.
   Share of a week = weekly load ÷ 40.
3. **Substitution.** `13 × 2 + 22 × 1.5 + 4 × 4 + 12 × 1 + 12 × 2 = 26 + 33 + 16 + 12 + 24`. Then
   `111/46`, then `2.4130/40`.
4. **Result.** **111.0 hours** a year, **2.4130 hours** a week, or **6.03 %** of a nominal working
   week. Held across **five** such sponsorships the same executive carries **555.0 hours**,
   **12.0652 hours** a week, **30.16 %** of the week.
5. **Interpretation.** Six per cent is the number that ends the argument. A role costing under two
   and a half hours a week cannot honestly be declined for want of time, and a sponsor who is absent
   at that price is making a **priority** choice, which is a legitimate thing to surface to whoever
   allocates sponsors. But the second half of the result is the more useful half, and it points away
   from the individual: at 2.4130 hours a week, an executive prepared to commit **20 %** of their
   week to sponsorship can carry `8/2.4130 =` **3.32**, that is **three** programmes of Meridian's
   size, and organisations routinely name the same four or five names across a dozen. **Sponsor
   capacity is a portfolio constraint, not a personal virtue**, and the portfolio body that
   allocates sponsors without a load model is manufacturing absent sponsors and then complaining
   about them (Domain 15 treats the allocation; the arithmetic here is what a project leader brings
   to it).

   Two cautions on using this. The estimate is **load, not latency**: 111 hours spread evenly is a
   different object from 111 hours that arrive in three clusters around the gates, and it is the
   clustering that produces the missed decision, which is why 3.2.1b measures turnaround separately
   rather than inferring it from availability. And the model **excludes the unschedulable half of
   the role**: defending the funding when it is challenged, standing behind the leader when the
   programme is under pressure, having the conversation about stopping. Those cost little time and
   most of the role's difficulty, so a sponsor who meets the 111 hours and none of them has met the
   diary and not the obligation.

**Turnaround is the sponsor's own latency, and it has a tail.** A sponsor is not a committee, so
`M/2 + L` does not apply; what applies is an empirical turnaround distribution, and the obligation
"decide within authority, promptly" is only testable once that distribution is counted. It is worth
counting because it behaves in a way the mean conceals.

**Worked example 3.2.1b — Meridian's sponsor decision turnaround, and where the mean comes from.**

1. **Setup.** The decision log records **22** decisions escalated to Meridian's sponsor over twelve
   months, with dates in and out: **9** returned in **1.0 week**, **8** in **2.5 weeks**, and **5** in
   **9.0 weeks**. Cost of delay **USD 14,280** per week; **25 %** of escalated decisions sit on the
   critical path. The steering committee's expected latency, for comparison, is **4.0 weeks** (3.2.3).
2. **Formula.** Mean turnaround `= Σ (count × weeks) ÷ count`. Expected delay-weeks
   `= critical-path share × Σ (count × weeks)`. Cost = delay-weeks × cost of delay.
3. **Substitution.** `9 × 1.0 + 8 × 2.5 + 5 × 9.0 = 9 + 20 + 45 = 74` decision-weeks; mean `74/22`.
   Delay-weeks `0.25 × 74`. Committee comparison `0.25 × 22 × 4.0`.
4. **Result.** Mean turnaround **3.3636 weeks**, median **2.5 weeks**. Expected delay **18.500
   weeks**, costing **USD 264,180.00**. The same 22 decisions taken by the committee at 4.0 weeks
   would cost `0.25 × 88 × 14,280 =` **USD 314,160.00**, so the sponsor route is **USD 49,980.00**
   cheaper. The **5** slow decisions (**22.73 %** of the count) carry **60.81 %** of the total wait.
   Bringing those five to the middle group's 2.5 weeks takes the total to **41.5** decision-weeks,
   the mean to **1.8864 weeks** and the cost to **USD 148,155.00**: a saving of **USD 116,025.00**,
   a **43.92 %** reduction, from changing the handling of five decisions.
5. **Interpretation.** Three results, in ascending order of usefulness. First, the sponsor route is
   *faster than the committee*, 3.36 weeks against 4.0, which is the arithmetic case for routing
   single-owner decisions to a single owner rather than to a body, and it is worth stating because
   organisations under stress do the reverse and add the decision to a committee agenda "for
   visibility". Second, **the mean is a tail phenomenon**: the median sponsor decision returns in
   2.5 weeks and the mean is 3.36, and the whole of that gap is five items. Reporting a mean
   turnaround therefore describes a distribution the sponsor does not experience and the project
   does not suffer from. The right metric is the **count and cause of decisions beyond a stated
   service level**, not the average.

   Third, and this is the finding that changes behaviour: **the fix is worth 43.92 % of the cost and
   touches 22.73 % of the decisions**, so a project leader arguing for a change to sponsor handling
   should argue about five items, not about diligence in general. And the five will have a nameable
   common cause; in practice one of three: the decision was framed without a recommendation, so the
   sponsor had to do the analysis; it required a peer's agreement the sponsor had to go and get,
   which is a decision-rights defect rather than a sponsor defect (3.1.2's precedence rule); or it
   was one the sponsor did not want to make, which is 3.2.1's advocate failure mode arriving as
   slowness rather than as refusal. Each has a different remedy, and none of them is "remind the
   sponsor".

   Two cautions. The delay figure **prices the wait as if each decision's weeks were additive**,
   which bounds rather than forecasts the programme effect, exactly as in 3.1.3. The defensible
   presentation is delay-weeks and the assumption alongside the currency figure. And a fast
   turnaround is not by itself evidence of a good sponsor: a sponsor who returns everything in a day
   may be approving without reading, which the record will show as decisions carrying no options
   considered and no versioned information reference (3.3.4). **Turnaround and record quality must
   be read together**, or the metric selects for the wrong behaviour.

**What a project leader does with a weak sponsor.** This is a real and common situation, and the
professional response is neither to complain nor to absorb the gap silently: absorbing it is
attractive, because it feels like competence, and it is how a leader ends up accountable for a
mandate they were never given. The workable response is procedural: write the decisions needed and
their dates; send them with a recommendation and a stated consequence of non-decision; record the
non-decision when it occurs; escalate the *pattern* rather than the individual instance once it
recurs; and keep the record. That is not politics. It is the mechanism by which an organisation's
governance failure becomes visible to the organisation while there is still time to fix it, and,
incidentally, the only defensible position for the leader afterwards.

### 3.2.2 Steering committees that work

A steering committee exists to make the decisions that exceed the project leader's authority, in
time for them to matter. Almost every observed dysfunction traces to one of five design faults.

**Fault 1: membership without authority.** Attendees who must consult before agreeing. The
committee's effective authority is the *minimum* of its members' authority on the decision in hand,
not the maximum, so one under-empowered member on a decision class blocks it. Test: for each
decision class in the delegation schedule, name the member who can commit.

**Fault 2, membership too large.** Beyond roughly six to eight decision-makers the body becomes a
briefing audience: contributions become positional, dissent becomes private, and the difficult item
gets deferred. Attendance is not membership, the separation of *members* (who decide) from
*attendees* (who inform) is the cheapest available fix.

**Membership carries a standing declaration.** Both faults above are about whether a member *can*
decide; the companion question is whether a member *may* decide the item in front of them.
Membership of a steering committee is an appointment, so it triggers the identification step of
Domain 1, KA 1.2.2a at the point of joining and again as each item is taken: an interest is declared
to the chair before the item, the interested member takes no part in it, and the minute records who
decided instead. The chair's practical instrument is a standing agenda item ahead of the first
substantive one, which costs a minute and makes the nil return routine rather than pointed, and a
body whose minutes never record a declaration is far more likely to be one that never asks than one
that never has any.

**Fault 3: the agenda consumed by reporting.** Status occupies the meeting and the decisions arrive
in the last ten minutes, which is why the reliable diagnostic of committee health is not attendance
but the **share of agenda time spent on decisions**. Status is read in advance; the meeting decides.

**Fault 4 (cadence mismatched to the work.** Treated quantitatively in 3.2.3), and it is the fault
with the largest measurable price.

**Fault 5, no capacity model.** A committee has finite throughput, and a design that routes more
decisions to it than it can hear does not slow decisions down evenly: the items that get deferred
are the contentious ones, which are the ones that needed the decision. The capacity arithmetic is
elementary and almost never done.

**Worked example 3.2.2 — does Meridian's steering committee have the capacity to govern?**

1. **Setup.** Meridian's steering committee meets every **4 weeks** (**13** meetings a year) and can
   handle **8** substantive agenda items per meeting. Annual demand: **36** escalated change
   requests (the count above the current delegation threshold, from 3.2.3), **26** standing reports
   (two per meeting), and **15** gate and assurance decisions.
2. **Formula.** Capacity = meetings × items per meeting. Utilisation = demand ÷ capacity.
3. **Substitution.** Capacity `13 × 8 = 104`. Demand `36 + 26 + 15 = 77`. `77/104`.
4. **Result.** Capacity **104** item-slots; demand **77**; utilisation **74.0 %**.
5. **Interpretation.** Seventy-four per cent looks comfortable and is not, and the reasons are
   computable rather than impressionistic.

   **First, demand is not uniform, and the peak is what a committee experiences.** The **51**
   decision items (36 changes plus 15 gate and assurance decisions), do not arrive evenly: they
   cluster at phase boundaries, and Meridian's history puts **40 %** of them in the **3** meetings
   that straddle the design-completion, first-clinic and full-rollout boundaries. That is `51 × 0.40
   = 20.40` items into 3 meetings, or **6.80** decision items per peak meeting, plus the 2 standing
   reports — **8.80 items against a capacity of 8**, a peak utilisation of **110.00 %**. The
   remaining **30.60** items spread over 10 meetings give `3.06 + 2 = 5.06`, an off-peak utilisation
   of **63.25 %**. So the true profile is not 74 % but **110 % at the boundaries and 63 % between
   them**, and a committee at 110 % does not defer items at random: it defers the ones that will
   take longest to discuss, which are the contentious ones, which are the ones that needed the
   meeting. **An average utilisation below capacity is not evidence of sufficient capacity; it is
   evidence that nobody has looked at the profile.**

   **Second, a quarter of the scarcest resource in the programme produces no decisions.** The 26
   standing reports consume `26/104 =` **25.0 %** of total capacity (Fault 3). Moving them to
   pre-reading releases 26 slots, drops average utilisation to **49.04 %** and (the number that
   matters) takes **peak** utilisation from 110.00 % to **85.00 %**, which is the first design in
   this sequence that can actually hear a contentious item at a phase boundary. Raising the
   delegation threshold as 3.2.3 recommends removes a further 12 items, taking demand to **65** and
   utilisation to **62.50 %** on the original agenda design, or to **37.50 %** if both changes are
   made.

   **Third, the demand figure above is incomplete, and knowing why is the point of 3.3.4.** It counts
   the decisions the committee is *asked* to make and omits the ones it makes twice; Worked example
   3.3.4b counts those at **25** further slots a year and takes real utilisation to **98.08 %**, which
   is the number that matches what the members report. A capacity model built only from the forward
   demand estimate will always understate, because re-decisions are invisible to everyone except the
   decision log.

   Two cautions on the model. Capacity in **item-slots assumes items are comparable**, and they are
   not: a contested threshold change and a routine assurance acceptance are not one slot each, so a
   committee near its limit should weight items by expected discussion time and re-run the sum. And
   **raising capacity is the weakest of the three available levers**: adding meetings or lengthening
   them increases the demand on exactly the people whose attention is scarcest, whereas pre-reading
   and delegation reduce demand at no cost in scrutiny. The professional point is that committee
   capacity is a *designed* quantity with an average, a peak and a hidden component, and a
   governance structure proposed without this arithmetic has not been designed.

### 3.2.3 Decision authorities and thresholds

**The structure.** A delegation schedule states, for each decision class, the level of authority
required, typically as a monetary threshold, but the better schedules use three dimensions:
**value** (how much), **reversibility** (how hard to undo), and **externality** (who outside the
project is affected). A cheap, irreversible, externally visible decision may deserve more authority
than an expensive, reversible, internal one; a schedule that reads only on value cannot express
that.

**Making reversibility a number, so the schedule can read on it.** Reversibility is usually left as
an adjective, which is why it drops out of practice; it becomes operable as a ratio. Define the
**reversal-cost ratio** `ρ = (cost to undo the decision) ÷ (value of the decision)`, and read the
required authority on `max(value, cost to undo)` rather than on value. The rule is one line and it
catches the decisions a value-only schedule is structurally blind to. On Meridian, a **USD 15,000**
change to the clinical template set that has already been deployed to the first cohort of clinics
carries a reversal cost of **USD 180,000** (re-configuration, re-validation and re-training across
those clinics), so `ρ =` **12**, and the authority appropriate to the decision is that of the
**100,001–500,000** band, not the delegated band its price tag puts it in. Externality takes the
same treatment as a three-point rating (internal / cross-functional / external or public) with a
stated escalation of one authority level per step, because it is the dimension least amenable to a
ratio: what an external decision costs is the cost of being wrong in public, which is not estimable
in advance and is precisely why it is handled by rule rather than by calculation. Both dimensions,
with the cumulative test of Worked example 3.3.4c, are the columns of Toolkit 3.T.2.

**Governance latency: the formula this domain contributes.** Committees meet periodically and
require papers in advance, and those two facts alone determine how long a decision waits. Let

```
M = the meeting interval (e.g. 4 weeks for a monthly committee)
L = the paper lead time — how far before the meeting submissions close
```

For a decision that arises at a uniformly random point in the cycle:

```
E[wait] = M/2 + L
```

The derivation is elementary. A decision arising more than `L` before the next meeting makes that
meeting, and waits on average `M/2` of the remaining interval; one arising inside the closed window
misses it and waits for the following meeting. Averaging over arrival times gives half the meeting
interval plus the *whole* paper lead time.

**Why the formula matters more than it looks.** It settles a question organisations habitually get
wrong. The two available levers do **not** have equal effect: cutting the paper lead time by one
week saves **one full week** of expected wait, while shortening the meeting interval by one week
saves only **half a week**. Administrative deadlines, which feel unchangeable and cost nothing to
change, are twice as powerful as meeting frequency, which is expensive and resisted. And a
governance design's latency can be computed at design time, from two numbers, before anyone has
waited.

Meridian's steering committee: `M = 4`, `L = 2`, so `E[wait] = 4/2 + 2 =` **4 weeks**. Note what
that says, a *monthly* committee with a two-week paper deadline imposes an expected wait of a full
month, not the fortnight most people assume. At Domain 1's cost of delay of 14,280 per week, one
escalated decision on the critical path costs **USD 57,120** in delay alone, before anyone in the
room disagrees about anything.

> **Fig 3.2.1 — Governance latency and its two levers.** Line chart, x-axis meeting interval `M`
> 1–13 weeks, y-axis expected wait `E[wait] = M/2 + L` in weeks, four lines for paper lead times
> `L` = 0.5, 1, 2 and 3 weeks. Meridian's design (`M` = 4, `L` = 2) marked at **4.0 weeks**, with
> its cost of **USD 57,120** annotated. Two arrows from that point: one down the `L` axis to
> `L` = 1 showing a **1.0-week** saving, one along the `M` axis to `M` = 2 showing a **0.5-week**
> saving, labelled "the paper deadline is twice the lever the meeting interval is". Source: PCI
> original. Alt text: four parallel rising lines of slope one-half, showing expected wait
> increasing with meeting interval and shifting upward with paper lead time.

**Setting the threshold.** The threshold question is an economic one and can be answered as one.
Setting a threshold too high risks decisions taken with insufficient authority or scrutiny; setting
it too low guarantees latency on decisions that did not need it. The second cost is certain and
computable; the first is probabilistic. Comparing them is the whole of the analysis.

**Worked example 3.2.3 — Meridian's delegation threshold, priced.**

1. **Setup.** Meridian's project leader may approve changes up to **USD 10,000**; everything above
   goes to the steering committee (`E[wait]` = **4 weeks**, cost of delay **14,280** per week).
   Year-one change requests, by value: **≤ 10,000 → 24**; **10,001–25,000 → 12** (average value
   **17,000**); **25,001–100,000 → 15**; **100,001–500,000 → 7**; **> 500,000 → 2**. Total **60**.
   Reviewed history shows **25 %** of escalated changes sit on the critical path and therefore
   convert their wait into programme delay; the rest do not. Should the threshold move to 25,000?
2. **Formula.** Escalated count × critical-path share × `E[wait]` × cost of delay, evaluated at each
   threshold. Then the worst-case cost of delegating the affected band.
3. **Substitution.** At 10,000: escalated `60 − 24 = 36`, delaying `36 × 0.25 = 9`, each costing
   `4 × 14,280 = 57,120`. At 25,000: escalated `36 − 12 = 24`, delaying `24 × 0.25 = 6`.
4. **Result.** Annual governance delay cost **USD 514,080** at a 10,000 threshold and **USD
   342,720** at 25,000, a saving of **USD 171,360** a year. The band being delegated is 12 changes
   worth **204,000** in total. Even in the **worst imaginable case** (the delegate decides *every
   one of the twelve* wrongly and destroys **40 %** of the value each time) the loss is `12 × 17,000
   × 0.40 =` **USD 81,600**, leaving the delegation ahead by **USD 89,760**. The threshold at which
   the two are equal is a value destruction of `171,360/204,000 =` **84 %** of every delegated
   decision.
5. **Interpretation.** The escalation cannot be justified at any plausible error rate. The delegate
   would have to destroy 84 % of the value of *every* delegated decision for the committee's
   involvement to break even, and a delegate who did that would be removed for reasons unrelated to
   thresholds. That is the general shape of the result, and it is why over-centralised delegation
   schedules are so common and so expensive: **the cost of escalation is certain, recurring and
   invisible, while the cost of a delegated error is uncertain, occasional and highly visible.**
   Organisations optimise against the visible cost. Three professional cautions, however. The
   calculation prices *delay*, not scrutiny; where a decision class carries irreversibility or
   external exposure the value test is the wrong test, which is why the schedule reads on three
   dimensions and not one. The 25 % critical-path share is an *assumption from history* and must be
   stated as one, since the conclusion is proportional to it; though note that it would have to fall
   below `81,600/(12 × 57,120) =` **11.9 %** before the worst case even competed. And a raised
   threshold requires the delegate to have the *information* to decide, which is a real
   prerequisite: delegation without information is abdication.

### AI in this KA

**Where it earns its place.** Modelling latency across candidate governance designs: the arithmetic
above, over dozens of cadence and threshold combinations, is deterministic and a natural fit.
Classifying a change-request history into value bands and testing whether a proposed threshold would
have altered any actual outcome, a genuinely useful retrospective test, and one nobody has time to
do by hand. Drafting a delegation schedule from an existing one plus a set of stated changes, for
human review. Detecting the decidability defects of 3.1.1 across a document set: decisions with no
owner, two owners, or thresholds that conflict between the contract and the terms of reference.

**Where it must not go.** Setting a threshold, which is a risk-appetite decision belonging to the
sponsor and the accountable body. Estimating the critical-path share or the error rate from
plausibility rather than data: a model asked for these will produce confident numbers with no
provenance, and they will then drive a real decision. Nor should its latency model be trusted
unverified: the formula is two operations, and a leader who cannot reproduce `M/2 + L` on paper
should not be quoting it.

**Verification, concretely.** Reproduce every number by hand, state the assumptions with their
sources, and put the sensitivity in the paper: the breakeven error rate and the breakeven
critical-path share, not merely the point estimate, because those are what tell a board whether the
recommendation is robust or fragile.

### Key terms — KA 3.2

| Term | Meaning |
|---|---|
| **Sponsor** | The individual accountable for the project's business outcome and mandate. |
| **Steering committee** | The body that makes decisions exceeding the project leader's authority, in time for them to matter. |
| **Delegation schedule** | The statement of authority by decision class, on value, reversibility and externality. |
| **Governance latency** | The expected wait for a committee decision: `E[wait] = M/2 + L`. |
| **Meeting interval (`M`)** | The period between meetings of a decision body. |
| **Paper lead time (`L`)** | How far before a meeting submissions close. |
| **Committee capacity** | Meetings per year × substantive items per meeting; compared against decision demand, average, peak and off-peak. |
| **Member vs attendee** | Members decide; attendees inform. Conflating them enlarges the body and reduces its authority. |
| **Sponsor turnaround** | The measured interval from a decision reaching the sponsor to its return; read as a distribution, not a mean. |
| **Reversal-cost ratio (`ρ`)** | Cost to undo a decision ÷ its value; authority reads on `max(value, cost to undo)`. |
| **Sponsor load** | The annual diary hours the sponsor obligations imply; a portfolio allocation constraint, not a personal one. |

### Sample MCQs — KA 3.2

**MCQ 3.2-A `[3.2.3 · Application]`** A committee meets every 6 weeks and closes papers 2 weeks
before each meeting. The expected wait for a decision arising at a random point in the cycle is:
- A. 3 weeks
- B. 5 weeks ✅
- C. 6 weeks
- D. 8 weeks

*Rationale:* `E[wait] = M/2 + L = 6/2 + 2 = 5` weeks (3.2.3). A counts only half the interval and
omits the paper lead time; C is the interval itself; D adds the whole interval to the lead time.

**MCQ 3.2-B `[3.2.3 · Analysis]`** A committee meets every 4 weeks with a 2-week paper lead time.
Two options are on the table: cut the paper lead time by one week, or cut the meeting interval by one
week. Which reduces expected latency more, and by how much?
- A. cutting the meeting interval, by 1.0 week
- B. cutting the paper lead time, by 1.0 week — twice the 0.5-week saving from the interval ✅
- C. both, equally, by 1.0 week
- D. both, equally, by 0.5 weeks

*Rationale:* `E[wait] = M/2 + L` is 4.0 weeks initially; cutting `L` to 1 gives 3.0 (saves 1.0),
cutting `M` to 3 gives 3.5 (saves 0.5), a one-week cut in the paper lead time always saves a full
week, a one-week cut in the meeting interval only half of one (3.2.3). The administrative deadline
is also the cheaper lever to move.

**MCQ 3.2-C `[3.2.3 · Evaluation]`** Raising Meridian's threshold from 10,000 to 25,000 saves
171,360 a year in delay. The band delegated comprises 12 changes worth 204,000. The strongest
argument for the change is that:
- A. the project leader is experienced
- B. the delegate would have to destroy 84 % of the value of every delegated decision for the
  escalation to break even ✅
- C. the committee is too busy
- D. 12 changes is a small number

*Rationale:* The decisive argument is the breakeven value-destruction rate, which makes the
comparison explicit and quantitative (3.2.3). A and D are assertions; C is a capacity argument, which
is real but secondary.

**MCQ 3.2-D `[3.2.2 · Analysis]`** A steering committee's effective authority on a given decision
class is best described as:
- A. the authority of its chair
- B. the highest authority among its members
- C. the lowest authority among the members whose agreement is required ✅
- D. the authority delegated to the project leader

*Rationale:* One member who must consult before agreeing blocks the class, so the binding
constraint is the minimum, not the maximum (3.2.2, Fault 1).

**MCQ 3.2-E `[3.2.1 · Analysis]`** The most reliable test of whether a sponsor is an *advocate*
sponsor rather than an effective one is whether they:
- A. attend every steering committee
- B. can state a condition under which they would recommend stopping the project ✅
- C. defend the project's funding
- D. know the project's schedule in detail

*Rationale:* An advocate sponsor is committed to the project rather than the outcome and therefore
cannot stop it (3.2.1). C is an obligation of the role; D is closer to the operational failure mode.

**MCQ 3.2-F `[3.2.1 · Analysis]`** A sponsor returned 22 decisions in a year: 9 in 1.0 week, 8 in
2.5 weeks and 5 in 9.0 weeks. Which statement best supports a proposal to change how sponsor
decisions are handled?
- A. the mean turnaround is 3.3636 weeks, which is too slow
- B. the median is 2.5 weeks, so performance is acceptable
- C. 22.73 % of the decisions carry 60.81 % of the total wait, so handling five items differently
  removes 43.92 % of the cost ✅
- D. the sponsor is slower than a 4-week committee and the decisions should go to the committee

*Rationale:* The mean (A) describes a distribution nobody experiences, and the median (B) hides the
tail; the decisive presentation is the tail's share of the wait and the saving available from it
(3.2.1). D is arithmetically wrong: 3.3636 weeks is *faster* than the committee's 4.0 weeks. Common
error: arguing from the average, which invites a debate about diligence instead of about five items.

**MCQ 3.2-G `[3.2.3 · Application]`** A delegated change is worth USD 15,000 but would cost
USD 180,000 to undo once deployed. Under a schedule that reads on value, reversibility and
externality, the authority required is that appropriate to:
- A. USD 15,000, since that is the change's value
- B. USD 165,000, the net exposure
- C. USD 180,000, because authority reads on `max(value, cost to undo)` ✅
- D. USD 195,000, the sum of value and reversal cost

*Rationale:* The reversal-cost ratio is `180,000/15,000 = 12`, and the rule reads authority on the
larger of the two figures (3.2.3). A is the value-only failure of Case study B. B nets the change's
value off the reversal cost as though the value earned offset the cost of undoing it; there is no
such offset. The reversal cost is what it costs to return to the prior state. D adds two quantities
that are not additive: the reversal cost already includes undoing the 15,000 of work.

### Self-check — KA 3.2

1. *State the governance latency formula and both of its levers.* — `E[wait] = M/2 + L`; shorten the
   paper lead time (a full week per week cut) or the meeting interval (half a week per week cut).
2. *Why do organisations systematically set delegation thresholds too low?* — The cost of escalation
   is certain, recurring and invisible; the cost of a delegated error is uncertain, occasional and
   highly visible, so optimisation runs against the visible cost.
3. *What must accompany a raised threshold for it to be delegation rather than abdication?* — The
   information, criteria and support the delegate needs to decide, plus a record of what they decided.
4. *Why is average committee utilisation a misleading measure?* — Demand clusters at phase boundaries;
   Meridian's 74.0 % average conceals **110.00 %** at the boundaries and **63.25 %** between them, and
   it is at the peak that contentious items are deferred (3.2.2).
5. *How does a delegation schedule express reversibility?* — Through the reversal-cost ratio `ρ =
   cost to undo ÷ value`, with authority read on `max(value, cost to undo)`, which put a USD 15,000
   Meridian template change into the 100,001–500,000 band at `ρ` = 12 (3.2.3).
6. *Roughly how much diary time does a Meridian-sized sponsorship require, and why does the answer
   matter?* — **111.0 hours a year**, about **6.03 %** of a working week; small enough that absence is a
   priority choice, and large enough that no executive carries more than about three (3.2.1).

---

## Knowledge Area 3.3 — Assurance, gates and escalation

*Topics: 3.3.1 stage gates · 3.3.2 assurance lines · 3.3.3 escalation design · 3.3.4 auditability
and the decision record.*

### 3.3.1 Stage gates

**Definition.** A stage gate is a point at which continuation requires an explicit decision by a
named authority against stated criteria, with authority to stop, hold or redirect as well as
proceed. A checkpoint without the authority to stop is a milestone with a meeting attached.

**What a gate is for.** Gates exist to convert **irreversibility into optionality**: before
committing the next tranche of cost, an organisation buys the chance to stop or change direction
while stopping is still cheap. It follows that gates belong where irreversibility genuinely steps up
(before major commitment, before build, before go-live, before regulatory submission), and not at
regular calendar intervals, which is how they proliferate into re-approval ceremonies.

**Gate criteria.** Criteria must be set **in advance**, be **objectively assessable**, and cover
both the work and the decision: is the deliverable adequate; is the business case still valid
(Domain 2's benefits and assumptions, re-tested rather than restated); are the risks acceptable and
owned; is the receiving organisation ready; and is the plan for the next stage credible? Criteria
written after the evidence is available are not criteria.

**The honest gate.** A gate that has never held or stopped anything is not evidence of good
delivery, it is evidence of a gate that does not function. And and its cost is being paid for
nothing. The pattern to watch for is the **conditional pass**: proceed subject to conditions, which
are then neither tracked nor enforced. A conditional pass is a genuine and useful instrument, and it
is only that if the conditions have owners, dates and a stated consequence of non-completion,
verified at the next gate.

**Gates cost time, and time has a price.** This is the part of gate design that is almost never
computed, and it decides whether the gate is worth having.

**Worked example 3.3.1 — is Meridian's design gate worth its elapsed time?**

1. **Setup.** Meridian's design-completion gate requires **USD 45,000** of review effort and
   **6 weeks** of elapsed time (cost of delay **14,280** per week). Historical evidence on
   comparable work: a material design defect is present with probability **0.30**; the gate detects
   it with probability **0.80**; a defect corrected at design costs **120,000**; the same defect
   found in build costs **900,000**.
2. **Formula.** Expected total cost with the gate = review + delay + `P(defect) ×`
   [`P(detect) ×` design-fix `+ P(miss) ×` build-fix]. Without the gate = `P(defect) ×` build-fix.
3. **Substitution.** Without: `0.30 × 900,000`. With: `45,000 + 6 × 14,280 + 0.30 × (0.80 × 120,000
   + 0.20 × 900,000)`.
4. **Result.** Without the gate **USD 270,000**. With the gate `45,000 + 85,680 + 82,800 =`
   **USD 213,480**. The gate is worth **USD 56,520**.
5. **Interpretation.** The gate pays, and the useful output is not the 56,520 but the two
   **breakeven points** it implies, because those are what a leader negotiates with. Holding
   everything else, the gate stops paying once its elapsed time reaches `(270,000 − 45,000 −
   82,800)/14,280 =` **9.96 weeks**: a gate that takes ten weeks destroys the value it exists to
   protect, which is the arithmetic behind the familiar and usually unquantified complaint that
   assurance has become an obstacle. And at the actual 6 weeks, the gate needs a detection
   probability above **55.85 %** to be worth holding, so a review staffed by people who cannot
   competently detect the defect is worse than no review, because it costs the money and the delay
   and returns nothing. Both results generalise: **gate value is destroyed by elapsed time and by
   weak detection**, and a leader whose gate is under challenge should compute which of the two is
   the actual problem before defending or abandoning it.

**The conditional pass, priced, and the identity that makes it dangerous.** The instrument was
described above as real only if its conditions have owners, dates and consequences. That is not a
counsel of tidiness; it follows from the gate arithmetic. A gate detects a defect and then *directs
a remedy*, and a directed remedy that is not completed leaves the defect in the work. So if a gate's
detection rate is `q` and the share of conditions actually closed is `r`, the gate's **effective
detection rate** is

```
q_eff = q × r
```

— an identity worth stating because it converts an administrative failure into the same variable the
gate's value already depends on. A gate that detects perfectly and closes half its conditions is a gate
that detects half.

**Worked example 3.3.1b — Meridian's conditional passes, and the gate that inverted.**

1. **Setup.** Meridian's four gates in the last twelve months issued **23** conditions between them.
   At the following gate: **9** were verified closed, **8** were open past their stated date, and
   **6** had no owner or date recorded and were never tracked at all. Everything else is Worked example
   3.3.1's design gate: review effort **USD 45,000**, elapsed **6 weeks** at **14,280** per week,
   `P(defect)` **0.30**, nominal detection `q` **0.80**, design fix **120,000**, build fix **900,000**;
   the gate's nominal value was **USD 56,520**.
2. **Formula.** Closure rate `r =` closed ÷ issued. `q_eff = q × r`. Then re-run 3.3.1's comparison
   with `q_eff` in place of `q`, and solve `q × r ≥ d*` for the closure rate the gate needs, where
   `d*` is 3.3.1's breakeven detection probability.
3. **Substitution.** `r = 9/23`. `q_eff = 0.80 × 0.3913`. Expected remediation
   `0.30 × (0.3130 × 120,000 + 0.6870 × 900,000)`. Total with gate
   `45,000 + 6 × 14,280 + that`. Breakeven `d* = 130,680/234,000`; `r* = d*/0.80`.
4. **Result.** Closure rate **39.13 %**; effective detection **31.30 %**, against a nominal 80 %.
   Expected remediation rises from 82,800 to **USD 196,747.83**, so the total with the gate is
   `45,000 + 85,680 + 196,747.83 =` **USD 327,427.83** against **USD 270,000** without it. **The
   gate now destroys USD 57,427.83**: a swing of **USD 113,947.83** from its nominal value, caused
   by no change whatever to the gate itself. The closure rate the gate needs to break even is `d*/q
   = 0.5585/0.80 =` **69.81 %**, i.e. **17** of the 23 conditions rather than 9; at 16 of 23 (69.57
   %) it is still **USD 453.91** underwater, and at 17 of 23 (73.91 %) it returns **USD 7,685.22**.
5. **Interpretation.** The headline is the swing. **A gate worth +56,520 became a gate worth −57,428
   without anyone changing its criteria, its staffing or its duration**: the whole of the movement
   is in a tracking failure that no governance report would have shown, because governance reports
   count gates held and conditions issued, not conditions closed. That is the practical case for
   making condition closure a *counted* field rather than an exhortation, and it is why Toolkit
   3.T.3 makes "conditions past their date" one of five monthly integers.

   The identity `q_eff = q × r` also relocates the remedy. When a gate is challenged as an obstacle,
   the instinct is to argue about its criteria or its duration; here neither is the problem. At the
   observed closure rate the gate's **breakeven elapsed time collapses from 9.96 weeks to 1.9784
   weeks**, so a leader defending a six-week gate on this evidence is defending the indefensible,
   and a leader *attacking* it would be attacking the wrong object, since the gate at full closure
   is worth 56,520. **The question to compute first is which of `q`, `r` and elapsed time is the
   binding constraint**, and only one of the three is cheap to fix.

   Note also the composition of the 14 unclosed conditions, because the two halves are different
   defects. The **8 open past their date** are a follow-through failure: the owner and date existed,
   so the register can chase them, and a monthly count will close most of them. The **6 with no
   owner or date** were never conditions at all. They were reservations minuted as conditions, and
   they are the more serious finding, because a gate that issues them has recorded a decision it did
   not make. Closing the 8 alone takes `r` to `17/23 =` **73.91 %** and the gate back into value,
   which is the cheapest available governance intervention in this domain.

   Three cautions. The identity assumes a condition's remedy, if completed, restores the detection
   the gate credited itself with — reasonable for a defined corrective action, wrong for a condition
   that merely defers the question ("proceed subject to confirming the interface design"), which
   detects nothing and should never have been a conditional pass. **A conditional pass whose
   condition is "find out" is a hold recorded as a pass.** The model also treats closure as binary,
   and partial closure is common; where it matters, weight `r` by the share of each condition's
   remedy completed rather than by count. And `r` should be computed **per gate and per condition
   type**, not as one programme number, because an aggregate of 39.13 % can conceal one gate closing
   everything and another closing nothing; and it is the second that will produce the escape.

> **Fig 3.3.2 — The conditional pass: how closure rate decides a gate's value.** Line chart.
> Horizontal axis: condition-closure rate `r`, 0 % to 100 %. Vertical axis: gate net value in USD,
> from &#8722;150,000 to +75,000. A single straight brand-blue line, `net = −130,680 + 187,200 r`
> (slope `P(defect) × q × (build fix − design fix) = 0.30 × 0.80 × 780,000`), crossing zero at
> **69.81 %**. Four marked points: `r` = 0 at **&#8722;130,680**; Meridian's observed
> `r` = 39.13 % at **&#8722;57,427.83**, ringed in crimson and annotated "9 of 23 conditions closed";
> `r` = 73.91 % at **+7,685.22**, annotated "17 of 23 — the 8 overdue conditions closed"; and
> `r` = 100 % at **+56,520**, the nominal value from Worked example 3.3.1. A shaded band below the
> zero line is labelled "the gate costs more than it returns". A footnote panel prints
> `q_eff = q × r` and the breakeven detection probability **55.85 %**. Source: PCI original. Alt text:
> a single rising straight line crossing the zero-value axis at just under seventy per cent closure,
> with the observed closure rate marked well inside the shaded value-destroying region.

### 3.3.2 Assurance lines

**Where the vocabulary comes from, before it is used.** The "three lines" language used below is not
generic professional vocabulary; it belongs to an assurance architecture published by the
**Institute of Internal Auditors**. It is voluntary guidance owned by a named body rather than a
standard, a regulation or a requirement, and its owner has revised it, so the current formulation is
better read as a model of *roles and their relationships* than as sequential lines of defence: the
older "three lines of defence" framing, with its implication that assurance is a series of barriers
an error passes through, is the reading its own publisher moved away from. What follows is described
in this book's own words; nothing from the model is reproduced, and a reader who wants it should
obtain the current version from its publisher, who has not reviewed or endorsed this volume.

It is also **one architecture among several.** The assurance structure that actually applies to a
project is set by the organisation's own governance and, in regulated sectors, by what the regulator
expects of that organisation, so three-line vocabulary is a *lens for finding gaps and duplication*,
not an obligation to arrange anything in threes. Where an organisation's structure does not map onto
it, the useful questions survive the translation: who prevents, who checks independently, who forms
an opinion on the whole, and who can be told no.

**The three lines, and what each is for.**

- **First line, management.** The project's own controls: reviews, testing, quality checks,
  reporting. Owned by the project leader, and the only line that can prevent a defect rather than
  detect it.
- **Second line, oversight function.** A PMO, risk or quality function providing independent
  challenge while **typically** remaining inside management's chain. Its value is comparability
  across the portfolio and pattern detection no single project can see.
- **Third line: internal audit (and external assurance).** Independent of management, **typically**
  reporting to an audit committee or equivalent body, forming an opinion on whether the whole
  control system works.

Both "typically" qualifications are load-bearing. Reporting lines vary with organisational form: the
functional, matrix, projectised and multi-party structures of KA 3.1.2 place the second and third
lines differently, and in a consortium or joint venture there may be two of each, answering to
different parents with different appetites. Establish where each line actually reports **before**
relying on its independence, because an assurance product's value is set by who can overrule it.

**The failure modes.** *Duplication*: three lines asking the same questions, which multiplies cost
and destroys the project team's willingness to engage. *Gap*: everyone assumes another line covers a
risk. *Capture*: the second line drafts the plan it later assures, and so cannot challenge it; this
is the most damaging and the least visible, because the assurance product still looks independent.
*Assurance as accountability transfer*: a leader treats a favourable assurance opinion as a
discharge of their own obligation; Domain 1's principle applies unchanged, an opinion is information
and not a transfer of accountability.

**Capture has an instrument, and it is not a reorganisation.** Because capture is a defect of
*independence* rather than of structure, it is reached by the declaration duty of Domain 1, KA
1.2.2a: the reviewer who drafted, advised on or is otherwise interested in the thing being assured
declares it to the commissioning authority before the review starts, and either the review passes to
someone who did not, or, where no independent alternative exists, the opinion states the authorship
on its face, so that a reader can discount it knowingly. Prior authorship of the assured artefact is
a **structural** interest, not an item-by-item one, so the honest remedy is usually to move the work
rather than to caveat the opinion. The countable check is on the assurance product itself: every
opinion carries the reviewer's declaration, nil returns included, and the count of opinions where
the declaration is missing is the measure of how much of the assurance programme is of unknown
independence.

**The countermeasure** is an **assurance map**: risks and controls down the side, lines across the
top, and each cell marked as covered, not covered or duplicated, reviewed by the accountable
authority. It is a half-day artefact that surfaces both gaps and duplication, and its absence is the
usual reason a project is simultaneously over-assured and unassured.

**The map becomes a decision tool when the cells carry numbers.** A map marked "covered / not covered"
identifies gaps and cannot rank them, which is why maps are drawn, admired and not acted on. Give each
material risk `i` a probability `pᵢ` that the control failure it describes is present, a consequence
`uᵢ` if it escapes to where it does damage, and each line `j` covering it a detection rate `qᵢⱼ`, and
the map computes the only quantity that ranks its rows:

```
residual exposure  =  Σᵢ  pᵢ × ∏ⱼ (1 − qᵢⱼ) × uᵢ
```

Two properties follow immediately and both are counter-intuitive. Because the miss probabilities
**multiply**, the marginal contribution of an additional line to a risk already covered twice is
small: the third line on a risk with two 0.50-plus detectors is buying the last slice of a small
number. And because uncovered risks contribute `pᵢuᵢ` in full, **a single gap almost always outranks
every duplication in the map**. The reallocation that follows is therefore available at *constant
cost*, which makes it the rare governance improvement that needs no budget decision.

**Worked example 3.3.2 — Meridian's assurance map, and the reallocation that costs nothing.**

1. **Setup.** Meridian's five material control risks are assured as follows, at a blended assurance
   day rate of **USD 950**. First line **90 days** (USD 85,500), second line **40 days**
   (USD 38,000), third line **30 days** (USD 28,500) — **160 days**, **USD 152,000** in total.

   | Risk | `p` | `u` (USD) | 1st line `q` | 2nd line `q` | 3rd line `q` |
   |---|---:|---:|---:|---:|---:|
   | R1 clinical data migration integrity | 0.30 | 900,000 | 0.60 | 0.50 | 0.40 |
   | R2 access control and confidentiality | 0.20 | 1,200,000 | 0.50 | — | — |
   | R3 benefits ownership in the receiving organisation | 0.35 | 685,440 | — | — | — |
   | R4 clinic readiness and training | 0.40 | 85,680 | 0.70 | 0.40 | — |
   | R5 third-party interface conformance | 0.25 | 214,200 | 0.55 | 0.45 | — |

   `u` for R3 is one year of the programme's realistic benefit (USD 685,440); for R4 and R5, six and
   fifteen weeks of delay at USD 14,280 per week.
2. **Formula.** Residual exposure `= Σ pᵢ ∏(1 − qᵢⱼ) uᵢ`. Marginal value of a line on a risk
   `= pᵢ × Δ(miss) × uᵢ`. Breakeven consequence for a line on a risk `= line cost ÷ (pᵢ × Δq)`.
3. **Substitution.** R1 `0.30 × (0.40 × 0.50 × 0.60) × 900,000`; R2 `0.20 × 0.50 × 1,200,000`;
   R3 `0.35 × 1.00 × 685,440`; R4 `0.40 × (0.30 × 0.60) × 85,680`;
   R5 `0.25 × (0.45 × 0.55) × 214,200`.
4. **Result (as assured.**)

   | Risk | Combined miss | Residual exposure (USD) | Share of residual |
   |---|---:|---:|---:|
   | R1 (all three lines) | 0.1200 | 32,400.00 | **7.87 %** |
   | R2 (first line only) | 0.5000 | 120,000.00 | **29.15 %** |
   | R3 (no line) | 1.0000 | **239,904.00** | **58.27 %** |
   | R4 | 0.1800 | 6,168.96 | 1.50 % |
   | R5 | 0.2475 | 13,253.63 | 3.22 % |
   | **Total** | | **411,726.59** | 100 % |

   Now move the third line's **30 days** off R1 (where all three lines sit) onto R3, at the same
   detection rate of 0.40 and the same cost. R1's residual rises to **USD 54,000.00**; R3's falls to
   **USD 143,942.40**. Total residual **USD 337,364.99**: a reduction of **USD 74,361.60** (**18.06
   %** of residual, **13.19 %** of residual plus assurance cost) for **no additional spend**.
5. **Interpretation.** The distribution in the fourth column is the finding, and it is the finding
   in almost every assurance map drawn for the first time. **All three lines sit on the risk
   carrying 7.87 % of residual exposure, while 87.41 % of it sits in the two risks with the least
   coverage**, and the largest single row, at **58.27 %**, has no assurance at all. This is the
   predictable result of assurance following *auditability*, not incompetence. Migration integrity
   is testable, sampleable and comfortable to review, so three functions review it. Benefits
   ownership in the receiving organisation is diffuse, politically awkward and owned outside the
   project, so nobody does, which is precisely Domain 2's omitted-column failure reappearing as an
   assurance gap. **Assurance concentrates where it is easy, not where residual exposure is, and
   only the arithmetic makes that visible.**

   The reallocation arithmetic explains itself once the marginal values are written down. The third
   line on R1 was buying `0.30 × (0.20 − 0.12) × 900,000 =` **USD 21,600** of exposure reduction; on
   R3 the same 30 days buys `0.35 × 0.40 × 685,440 =` **USD 95,961.60**: **4.44 times** as much.
   Against its **USD 28,500** cost, the third line on R1 was running at a net **loss of USD 6,900**,
   and on R3 it returns a net **USD 67,461.60**. The breakevens make the point transferable: for the
   third line to pay on R1, either the consequence would have to exceed `28,500/(0.30 × 0.08) =`
   **USD 1,187,500** (against R1's 900,000) or the failure probability would have to exceed
   `28,500/(0.08 × 900,000) =` **39.58 %** (against R1's 0.30). **Neither is close**, and both are
   the sort of test a reviewer can run on a single row in a minute.

   **The professional caution that must accompany this, because the arithmetic over-applies.**
   Nothing above justifies deleting the third line. Third-line assurance exists to form an
   **independent opinion on whether the control system works**: a portfolio-level and constitutional
   product, reporting outside management, whose value is not the marginal detection it contributes
   to any one project's risk register. Charging its cost to a single project and testing it on
   detection economics misprices it by construction, and the honest conclusion from the numbers
   above is narrower and more useful: **the third line's effort was pointed at the wrong risk on
   this project.** Where an organisation genuinely wants the detection contribution of a third pass
   on a single risk, the marginal arithmetic says it will rarely pay, which is an argument about
   *where* independent assurance looks, never about whether it should exist.

   Three further cautions. The multiplication of miss probabilities assumes the lines are
   **independent detectors**, and Domain 9, KA 9.2.2 shows why they usually are not: three functions
   reading the same design document with the same blind spot miss the same defect, so realised
   coverage on R1 is worse than 88.00 % and closer to the 80.00 % that the first two lines achieve
   alone. Make consecutive lines *methodologically different* or do not count them separately. The
   detection rates are **estimates**, and where no history exists the defensible presentation is a
   range with the reallocation decision computed across it: note that the R1-to-R3 conclusion
   survives any `q` for the third line above about 0.09 on R3, so it is robust in a way the point
   estimate does not show. And **capture invalidates a cell rather than reducing it**: a line
   assuring work it helped produce should be entered as `q = 0`, not as a lower `q`, because its
   opinion is not weak evidence, it is not evidence.

**Proportionality, and how to size it.** Assurance effort should scale with novelty, value,
irreversibility and external exposure, not uniformly, and not with organisational anxiety. A
high-value, low-novelty, reversible project may need less assurance than a small, novel,
irreversible, publicly visible one, and a regime that cannot express that will over-assure the first
and under-assure the second. The residual-exposure column above is what makes proportionality
operable *within* a project: allocate the next assurance day to the row with the largest `pᵢ ∏(1 −
qᵢⱼ) uᵢ`, and stop when the marginal reduction falls below the day rate. Across projects, the same
logic is a portfolio judgement and belongs to the second line, whose comparability across the
portfolio is the whole of its value.

### 3.3.3 Escalation design

**The principle.** Escalation is a **designed pathway with stated latency**, not an instruction to
tell someone senior. A usable design states, for each escalation class: the trigger (objective, not
"if concerned"), the destination, the decision required, the **time within which the decision will
be made**, and the out-of-cycle mechanism if the ordinary cadence is too slow.

That fourth element is the one usually missing, and its absence has a computable cost.

**Worked example 3.3.3 — the total latency of Meridian's escalation path.**

1. **Setup.** A decision requiring executive authority passes three tiers: the **project board**
   (meets every 2 weeks, papers close 1 week ahead), the **programme board** (every 4 weeks, papers
   2 weeks ahead), and the **executive committee** (quarterly (every 13 weeks) papers 3 weeks
   ahead). Cost of delay **14,280** per week.
2. **Formula.** Total expected latency = Σ over tiers of `M/2 + L`. Cost = latency × cost of delay.
3. **Substitution.** Project board `2/2 + 1 = 2.0`; programme board `4/2 + 2 = 4.0`; executive
   committee `13/2 + 3 = 9.5`. Total `2.0 + 4.0 + 9.5`.
4. **Result.** **15.5 weeks** of expected latency for a single decision, costing **USD 221,340** in
   delay; before anyone in any of the three rooms disagrees with the recommendation.
5. **Interpretation.** Two hundred and twenty-one thousand dollars is the price of an organisation
   chart, and it is invisible: it appears in no budget, is attributed to no decision, and is
   generally described afterwards as the project having been slow. Notice the distribution, the
   quarterly committee alone accounts for **9.5 of the 15.5 weeks** (**61 %**) and **USD 135,660**,
   which is what makes "add the executive committee to the approval path" such an expensive sentence
   when spoken casually in a governance review. Two redesigns follow directly. **Reduce the tiers a
   decision must legitimately pass:** if the programme board can decide with the executive committee
   *informed*, latency falls to **4.0 weeks** and **USD 57,120**, a saving of **USD 164,220**, or
   **74.2 %**, from a change that removes no scrutiny that anyone can name. **Add an out-of-cycle
   mechanism:** a written-resolution procedure with a five-working-day turnaround makes the
   single-tier path **1.0 week** and **USD 14,280**, a **93.5 %** reduction against the original.
   The general rule this supports: **count the tiers, add the latency, price it, and then justify
   each tier against its cost**, which is a conversation governance reviews almost never have,
   because until the latency is added up there is nothing to weigh the tier against.

> **Fig 3.3.1 — The price of an escalation path.** Horizontal stacked bar chart. Bar 1, "as
> designed": three segments (project board **2.0 w**, programme board **4.0 w**, executive committee
> **9.5 w**), totalling **15.5 weeks / USD 221,340**, with the executive segment labelled "61 % of
> the latency". Bar 2, "one tier, executive informed": **4.0 w / USD 57,120**. Bar 3, "one tier with
> written resolution": **1.0 w / USD 14,280**. A right-hand column shows savings of **164,220 (74.2
> %)** and **207,060 (93.5 %)**. Source: PCI original. Alt text: three horizontal bars of sharply
> decreasing length showing escalation latency falling from 15.5 weeks to 1 week as tiers are
> removed and an out-of-cycle mechanism is added.

**Designing the trigger.** Objective triggers only: a forecast variance beyond a stated tolerance, a
risk exposure above a stated threshold, a decision required above the delegated authority, a
dependency breached. "Escalate if concerned" produces two failure modes at once (late escalation by
those who fear it and constant escalation by those who fear the alternative), and both are then
treated as individual judgement problems rather than as the design defect they are.

**But an objective trigger is not yet a well-specified one.** "Escalate if the forecast overrun
exceeds 10 % of budget" reads as objective and is not, because *forecast* is not a single number:
the earned value family produces several legitimate estimates at completion from the same data
(Domain 7, KA 7.3), and they can sit on opposite sides of a tolerance. A trigger must therefore name
three things the usual wording omits (**the forecast method**, **the measurement point** and **the
confirmation rule**), and the cost of omitting them is not ambiguity in the abstract but a real
escalation that either fires on noise or fails to fire at all. The volume's single-project thread
makes this concrete.

**Worked example 3.3.3b — writing Auriga's escalation trigger, and testing whether it can act.**

1. **Setup.** Project Auriga is a **25-week** project with **BAC USD 4,000,000**. At the **week 13**
   data date: **PV 2,080,000**, **EV 1,920,000**, **AC 2,120,000**, giving **CPI 0.91** and **SPI 0.92**
   to two places. Domain 7's three estimates at completion are **4,200,000** (remaining work at plan),
   **4,416,667** (`BAC/CPI`) and **4,608,056** (`AC + (BAC − EV)/(CPI × SPI)`). The governance framework
   says: *escalate to the programme board where the forecast overrun exceeds 10 % of BAC.* The escalation
   path available is 3.3.3's three-tier route at **15.5 weeks**, with a single-tier alternative at
   **4.0 weeks** and a written-resolution route at **1.0 week**. Planned average burn is
   `4,000,000/25 =` **USD 160,000** a week.
2. **Formula.** Variance at completion `VAC = EAC − BAC`, expressed as a share of BAC and tested against
   the tolerance. Then the **decision action window** `= (remaining duration − escalation latency) ÷
   remaining duration`, and the last week at which a trigger can still produce an actionable decision
   `= planned finish − escalation latency`.
3. **Substitution.** `4,200,000 − 4,000,000`; `4,416,666.67 − 4,000,000`; `4,608,055.56 − 4,000,000`;
   each over 4,000,000. Tolerance `0.10 × 4,000,000 = 400,000`. Remaining duration `25 − 13 = 12`.
   Windows `(12 − 15.5)/12`, `(12 − 4.0)/12`, `(12 − 1.0)/12`.
4. **Result.**

   | Forecast method | EAC (USD) | VAC (USD) | VAC as % of BAC | Trigger at 10 %? |
   |---|---:|---:|---:|---|
   | Remaining work at plan | 4,200,000.00 | 200,000.00 | **5.00 %** | **No** |
   | `BAC/CPI` | 4,416,666.67 | 416,666.67 | **10.42 %** | **Yes** |
   | `AC + (BAC − EV)/(CPI × SPI)` | 4,608,055.56 | 608,055.56 | **15.20 %** | **Yes** |

   The three methods span **USD 408,055.56** on identical data, and the tolerance line at 400,000 falls
   inside that span. And the action windows:

   | Escalation route | Latency | Weeks left after the decision | Action window |
   |---|---:|---:|---:|
   | Three tiers as designed | 15.5 w | **−3.5** | **−29.17 %** |
   | Single tier, board decides | 4.0 w | 8.0 | **66.67 %** |
   | Written resolution | 1.0 w | 11.0 | **91.67 %** |

   The as-designed path returns its decision **3.5 weeks after Auriga's planned completion**.
   Working backwards, the latest week at which that path can still deliver an actionable answer is
   week `25 − 15.5 =` **9.5**: the first **38.00 %** of the project.
5. **Interpretation.** Two results, and the second is the more important.

   **The trigger's answer depends entirely on which forecast method it names, and nobody has named
   one.** `BAC/CPI` clears the tolerance by USD 16,666.67 and the plan-based estimate misses it by
   USD 200,000; the composite clears it by USD 208,055.56. A framework that says "the forecast" has
   therefore delegated the escalation decision to whoever prepares the report, which is the opposite
   of what an objective trigger is for, and it does so invisibly, because each of the three numbers
   is correctly calculated. Worse, the `BAC/CPI` trigger is **marginal to the point of being
   noise**. The cost performance index at which `BAC/CPI` exactly equals 1.10 × BAC is `1/1.10 =`
   **0.9091**, and Auriga's CPI is **0.9057**: below it by **0.0034**. In earned-value terms, `EV`
   would need to be **USD 1,927,272.73** rather than 1,920,000 for the trigger not to fire: **USD
   7,272.73**, or **0.18 %** of BAC, one accrual timing difference. **A trigger stated on a single
   ratio at a single data date fires on measurement noise**, which is how organisations acquire a
   reputation for escalating trivia and then stop escalating at all. The corrections are cheap and
   specific: name the method (`BAC/CPI` is the defensible default because it is the method whose
   assumption, that present cost performance persists, is the one a governance body should be asked
   to accept or reject); state the measurement point (a defined data date, not "when noticed"); and
   add a **confirmation rule** (two consecutive data dates beyond tolerance, or one date beyond a
   wider secondary tolerance), so that a single period's noise cannot fire it and a sustained trend
   cannot fail to.

   **The second result is that on this project the trigger cannot help, whatever it says.** An
   escalation latency of 15.5 weeks against 12 weeks of remaining duration gives a **negative action
   window**: the decision is delivered after the work it concerns has finished, so the escalation is
   **ceremonial**, not slow. This is the test that belongs beside every escalation path and is
   almost never applied (*is the latency shorter than the remaining duration of the thing being
   decided?*), and its diagnostic form is sharper still: the as-designed path is actionable only
   during the first **38.00 %** of a 25-week project, so from week 9.5 onward Auriga has an
   escalation route that can produce decisions and cannot produce *useful* ones. Note what this does
   to the two redesigns of Worked example 3.3.3. There they saved **USD 164,220** and **USD
   207,060**; here they do something that cannot be expressed in money at all. They convert a
   decision from impossible to possible, moving the action window from −29.17 % to **66.67 %** and
   **91.67 %**. **Latency priced in currency understates the case for a shorter path; latency
   compared with remaining duration is what shows when the path is not a cost but a nullity.**

   Three cautions. The action window uses **planned** remaining duration, and at SPI 0.92 Auriga's
   remaining duration is itself likely longer than 12 weeks, which flatters the as-designed path.
   The defensible practice is to compute the window on the current schedule forecast (Domain 6) and
   to state which duration was used. The window is a **necessary and not a sufficient** test: a
   decision returned inside the window still has to arrive early enough for the *response* to be
   implemented, so a fuller form subtracts the implementation lead time as well, and for an
   irreversible response that lead time can exceed the decision latency. And a negative window is
   **not an argument for skipping the escalation**: the decision may be needed for reasons that
   outlive the project, and the honest response is to escalate *and* record that the ordinary path
   cannot answer in time, which is the evidence that gets the out-of-cycle mechanism built before
   the next project needs it.

**The escalation culture problem.** Escalation is only a functioning mechanism if using it is safe.
Where escalation is read as failure, it happens late, which is precisely when the decision can no
longer help. The countermeasures are governance ones and belong to the sponsor and the committee:
respond to early escalation visibly and well; separate the escalation of an *issue* from any
judgement about the person raising it; and track the **lead time** of escalations (how far before
the impact they arrive), because a shortening lead time is the earliest available signal that the
mechanism is decaying, and it appears in no standard report.

### 3.3.4 Auditability and the decision record

**The principle.** A decision that is not recorded has not been made. It has been remembered, and
memories diverge exactly when the stakes rise. The decision record is the mechanism that converts a
meeting into an institutional fact.

**What a decision record must contain**, per decision, and it is short enough that its absence is
never a resourcing problem: a unique reference; the date; the decision-maker by name and role (never
a committee alone, a body records the decision, a person is accountable for it); the decision, in
words that permit only one reading; the options considered and why the chosen one was chosen; the
information relied on, referenced to its version; **the interests declared in relation to the
decision, naming who declared what and what was done about it, with a nil return recorded where
there were none**; the conditions attached, with owners and dates; and the review date if the
decision is provisional.

**The declared-interest field is the cheapest of those and the only one that is worthless when it is
optional.** A field completed only where there was something to declare cannot be distinguished, a
year later, from a field nobody filled in, so the nil return is the part that carries the evidence.
Where an interest was declared, the entry states the management action (most often that the
interested party took no part and who decided instead), because a declaration recorded without an
action is a record of a conflict rather than of its management. The duty itself, its four steps and
the case where abstention is not enough are Domain 1, KA 1.2.2a; this is where it lands in the
governance machinery.

**Why the "information relied on, referenced to its version" line matters more than it looks.** When
a decision is examined afterwards (in a lessons review, a dispute, an audit or an inquiry) the
question is almost never "was the decision right?" but **"was it reasonable on what was known at the
time?"** Only a versioned reference can answer that, and its absence converts a defensible decision
into an indefensible one at exactly the moment the defence is needed.

**Two governance defects the record exposes**, and this is the practical case for keeping it well.
The **re-decided decision**: the same question arriving at the same body a third time is a symptom
of either an unrecorded decision or an illegitimate one, and the log makes the recurrence visible
where minutes do not. And the **decision nobody made**: a change that took effect without an entry,
which is how scope and budget move without anyone having decided, the most common finding of a
governance audit and the hardest to see from inside.

**RACI and its integrity check.** Responsibility assignment matrices (Responsible, Accountable,
Consulted, Informed) are the standard artefact for decision rights, and their standard failure is
being drafted, circulated and never checked. The checks are countable, and a matrix that fails any
of them is not a matrix but a document: **exactly one A** per decision (two make it undecidable,
none make it unowned); the A holds sufficient authority under the delegation schedule; C and R are
distinguished (consultation is not agreement); and the count of C's per decision is bounded, since
consultation is where latency accumulates invisibly.

**Worked example 3.3.4 — auditing Meridian's decision-rights matrix.**

1. **Setup.** Meridian's matrix covers **12** decision classes across 7 roles. On review: **9**
   classes carry exactly one Accountable, **2** carry two, and **1** carries none.
2. **Formula.** Defect rate = classes failing the single-A test ÷ total classes.
3. **Substitution.** `(2 + 1)/12`.
4. **Result.** **3** of 12 classes are defective: a **25.0 %** defect rate.
5. **Interpretation.** A quarter of the programme's decision classes cannot be decided as
   documented, and the two failure types behave differently under stress, which is why they are
   worth distinguishing rather than totalling; and why the 25.0 % headline is the least useful
   number in the example.

   **Price them separately.** The **two-A** classes will be decided, badly: by whichever holder acts
   first, or after a delay while the two reconcile, and reconciliation in practice means the item
   returns to the next meeting. Those two classes generated **11** decisions over the year; each
   incurring one extra committee cycle at 3.2.3's rates, with the standing **25 %** critical-path
   share, costs `11 × 0.25 × 4 × 14,280 =` **USD 157,080**: **USD 78,540** per defective class. The
   **zero-A** class behaves differently and worse: nothing forces it to a decision at all, so it
   drifts until the consequence forces an escalation, and an escalation that arrives without an
   owner arrives at the top. Its **4** decisions, drifting to the full three-tier path of Worked
   example 3.3.3 at **15.5 weeks**, cost `4 × 0.25 × 15.5 × 14,280 =` **USD 221,340**:
   coincidentally exactly one expected three-tier escalation, and **2.82 times** the cost of a two-A
   class. Total documented cost of a 25.0 % defect rate: **USD 378,420**.

   That ordering is the professional content of the example. **A decision class with two accountable
   roles is expensive; a class with none is nearly three times as expensive**, because the two-A
   defect delays a decision and the zero-A defect *relocates* it: upward, late, and to people with
   no preparation. Yet a reviewer scanning a matrix finds the two-A cases first, since they are
   visible as duplicated letters, while the zero-A case is visible only as a blank in a row nobody
   thought to write. The practical instruction is to audit the matrix **by decision class, not by
   role**: list every decision the project will face, then look for its A, which finds the missing
   rows that a column-by-column read never will.

   Two cautions. The costing assumes each defective class's decisions are **independent occasions of
   delay**, which bounds rather than forecasts as elsewhere in this domain; present the counts and
   the delay-weeks alongside the currency. And a single A is **necessary and not sufficient**: the A
   must also hold authority sufficient for the class under the delegation schedule, which is a
   second and separate check, a matrix can pass the single-A test in full while every A sits below
   the threshold its class requires, at which point the matrix is decorative, not wrong. The test
   costs an hour and it is the highest-yield governance check available before mobilisation.

**The re-decided decision has a price, and it is the largest hidden number in this domain.** 3.3.4
named the defect; counting it is what makes it actionable, and the count comes from the decision log
because it comes from nowhere else: minutes record what a meeting decided, never that the meeting
had decided it before.

**Worked example 3.3.4b — Meridian's re-decision tax.**

1. **Setup.** Meridian's decision log holds **148** decisions over twelve months. Clustering the
   decision text shows **19** questions that arrived at the same body more than once, of which **6**
   arrived three or more times. Of the 19, **13** carried no versioned reference to the information
   relied on and **6** carried no named decision-maker. The committee's capacity is **104** item-slots a
   year against the **77** items of forward demand computed in 3.2.2; `E[wait]` is **4.0 weeks**, cost
   of delay **14,280** per week, critical-path share **25 %**.
2. **Formula.** Repeat slots consumed = second appearances + third appearances. Corrected demand =
   forward demand + repeat slots. Delay cost = repeat slots × critical-path share × `E[wait]` × cost of
   delay.
3. **Substitution.** Repeat slots `19 + 6 = 25`. Corrected utilisation `(77 + 25)/104`. Delay cost
   `25 × 0.25 × 4 × 14,280`.
4. **Result.** Re-decision rate **12.84 %** of all logged decisions. Repeat slots **25**, which is
   **24.04 %** of the committee's annual capacity. Corrected demand **102** against capacity 104,
   utilisation **98.08 %**, not the 74.0 % of 3.2.2. Delay cost **USD 357,000.00**, which is **2.08
   times** the entire annual saving from raising the delegation threshold (USD 171,360).
5. **Interpretation.** Start with the reconciliation, because it resolves a contradiction the domain has
   been carrying. Worked example 3.2.2 computed 74.0 % utilisation and the members reported a committee
   that was full; **98.08 %** is why. The forward demand model counted the decisions the committee was
   asked to make and could not count the ones it made twice, and a capacity model that omits
   re-decisions will always read low by roughly the re-decision rate. **The single most useful
   correction to a committee capacity estimate is to add last year's repeat count**, and it takes an
   afternoon with the log.

   Then the size of it. **USD 357,000 a year is spent deciding things that had already been
   decided**: more than twice the celebrated saving from the threshold change, and unlike that
   saving it requires no delegation, no risk appetite conversation and no approval from anybody. It
   is also, uniquely in this domain, a defect whose remedy costs nothing: the causal split says
   **68.42 %** of the re-decisions lacked a versioned information reference and **31.58 %** lacked a
   named decision-maker, and both are **fields on a template**. A decision re-arrives because its
   basis can be disputed ("that was decided on the old numbers") or its authority can be disputed
   ("the committee noted it, nobody decided it"), and the two fields close both routes. This is the
   concrete answer to the question 3.3.4 raises and most organisations treat as rhetorical: *what is
   the decision record actually worth?* Here, **USD 357,000 a year and 25 committee slots**, from
   two fields.

   Three cautions. Not every repeat is a defect: a **provisional** decision with a stated review
   date is supposed to return, and a decision returning because the world changed is governance
   working, not failing, so the count must exclude scheduled reviews and material-change
   re-openings, and the defensible metric is *unplanned* re-decisions. The clustering that produces
   the count is a judgement about whether two differently worded questions are the same question,
   which is exactly the task AI is good at and exactly the task where a false positive is
   embarrassing, so every flagged pair is confirmed by a human before it is reported (see the AI
   treatment below). And the delay cost, as always here, **adds waits that may overlap**; the slot
   count (25 of 104) is the harder number and the one to lead with, because nobody can argue with
   it.

**The cumulative test, designed rather than asserted.** Toolkit 3.T.2 and Case study B both call for
a rule that aggregates related decisions, and a rule of that shape has two parameters: the aggregate
threshold `X` and the **relatedness class** over which decisions are summed within a period `P`.
Neither can be set without the other, and setting them badly does more damage than having no rule,
because a cumulative test that trips constantly re-centralises the whole delegated band while
appearing to be a control improvement.

**Worked example 3.3.4c — sizing Meridian's cumulative test without re-centralising the delegation.**

1. **Setup.** Meridian has raised its delegation threshold to **USD 25,000** (3.2.3), delegating
   **36** changes a year: the 24 at or below 10,000 averaging **5,000**, and the 12 in the
   10,001–25,000 band averaging **17,000** — **USD 324,000** of delegated value a year. Separately, one
   genuine cluster occurred: **7** related changes to the clinical template set, in sequence
   **8,400 · 6,200 · 11,500 · 9,800 · 4,600 · 13,200 · 7,300**, none individually above the threshold.
   The programme has **3** workstreams and **14** deliverable-level change classes. Period `P` = one
   quarter. Escalating one change costs **4.0 weeks** at 25 % critical-path share, i.e. an expected
   `0.25 × 57,120 =` **USD 14,280**.
2. **Formula.** Cluster aggregate = Σ of the related changes; the rule trips at the first change whose
   running total exceeds `X`. Base-rate aggregate per relatedness class per period = delegated value ÷
   (classes × periods). Annual cost of the rule = expected trips × expected escalation cost.
3. **Substitution.** Running totals `8,400 → 14,600 → 26,100`. Broad relatedness (workstream):
   `324,000/(3 × 4)`. Narrow relatedness (deliverable class): `324,000/(14 × 4)`.
4. **Result.** The cluster totals **USD 61,000** and trips at the **third** change (running total
   **26,100** against `X` = 25,000), leaving **USD 34,900** of the cluster to be authorised at the
   aggregate's level.

   | Relatedness class | Classes × periods | Base-rate aggregate per class-period | `X` = 25,000 trips? | Trips a year | Annual cost |
   |---|---:|---:|---|---:|---:|
   | Workstream (broad) | 3 × 4 = 12 | **USD 27,000** | Yes, on the base rate alone | **12** | **USD 171,360** |
   | Deliverable class (narrow) | 14 × 4 = 56 | **USD 5,785.71** | No | **1** | **USD 14,280** |

   The broad rule costs **USD 171,360** a year (**exactly** the saving the threshold increase
   produced), while the narrow rule costs **USD 14,280**, a factor of **12** less, and catches the
   same cluster.
5. **Interpretation.** The equality in the first row is not a coincidence and is worth naming,
   because it generalises. Raising the threshold delegated 12 changes and saved `12 × 0.25 ×
   57,120`; a cumulative rule that trips 12 times a year escalates 12 changes and costs `12 × 0.25 ×
   57,120`. **A cumulative test cancels a delegation exactly when its annual trip count equals the
   number of decisions the delegation released.** So the trip count, not the threshold, is the
   quantity to design against; and the trip count is driven overwhelmingly by the *relatedness
   class*, because that is what determines how much unrelated traffic is being summed.

   The design rule that falls out is a **window**, and it is the transferable result: `X` must sit
   comfortably **above** the base-rate aggregate of its relatedness class per period and comfortably
   **below** the aggregates of the clusters it must catch. At deliverable-class relatedness, `X` =
   25,000 sits **4.32 times** above the base rate of 5,785.71 and **2.44 times** below the 61,000
   cluster, a working window. Broadening relatedness to the workstream multiplies the base rate by
   **4.67**, to 27,000, which closes the window from below: every workstream now trips every quarter
   on ordinary traffic, and the rule has become a threshold reduction wearing a control's clothing.
   **Widening the relatedness class is the error, and it is the natural error**, because a broad
   class feels safer and costs nothing to write.

   Two further observations belong in the record. The rule catches the cluster **after three of
   seven changes**: the first 26,100 was already decided under delegated authority, and the rule
   cannot undo it. So the cumulative test is a **containment** control, not a prevention control,
   and it must be paired with the provision Case study B actually needed: every change generates a
   **register entry regardless of value**, which costs no latency at all and is what makes the
   aggregate visible before it trips. A cumulative test without universal registration is a rule
   that cannot see its own inputs. And relatedness should be defined by **what the changes touch**
   (the same deliverable, the same assured control, the same interface) not by who requested them or
   which budget they hit, because the exposure the rule exists to catch is a coherent change to one
   thing, arriving in instalments.

**Custody and retention: the part that decides whether any of this survives.** Everything above
assumes the record still exists when it is wanted. The questions that make a decision record
valuable (*was it reasonable on what was known at the time?*) are asked in a lessons review, a
dispute, an audit or an inquiry, all of which happen years after the decision and long after the
temporary organisation that made it has been dissolved. A record with no custodian and no retention
period is the artefact most likely to be missing at the moment it is needed, and its absence
converts a defensible decision into an indefensible one exactly as reliably as a missing version
reference does.

Four provisions close that gap, and they are decided in advance rather than at closure.

**A named custodian, by class.** The decision log, the change log (Domain 4, KA 4.4), gate packs, the
baseline archive (Domain 4, KA 4.3.3), the assurance opinions of 3.3.2 and acceptance evidence
(Domain 5, KA 5.4.3) each have one named role accountable for their existence, completeness and
retrievability. A role, not a person, because people move; and one role per class, because a record
class custodied by "the project" is custodied by nobody once the project ends.

**A system that preserves version and prevents silent amendment.** The record is held where an
earlier version cannot be quietly replaced by a later one. Where a record is amended, the amendment
travels with it (**what changed, who changed it, when, and why**), because the difference between a
corrected record and a rewritten one is the whole of its evidential value. An "amended" record that
looks identical to an original is worse than no record, since it invites reliance it cannot support.

**A retention period set as a governance decision, taken in advance.** The period for each class is
set at the longest of: the limitation period under the contract, any retention requirement the
organisation is subject to, the benefits-realisation horizon the record must outlive (Domain 16, KA
16.4.1's measurement plan can run for years after closure), and the organisation's own records
policy. It is stated as a schedule by class, never as a single rule for everything: Domain 16, KA
16.4.4 works the economics, and finds that retaining contractual and technical evidence pays as
insurance at a very low probability of ever needing it while retaining personal data is the opposite
trade. **The standing caveat:** retention requirements, limitation periods and disposal obligations
are jurisdiction-, sector- and record-class-specific, they differ substantially, and they are taken
from the organisation's records and legal functions and from qualified counsel. Nothing here states
a legal minimum or maximum, and nothing here should be relied on as stating one.

**An explicit handover of custody at closure.** The project ends; the records do not. Closure
transfers each class to a named continuing custodian in a permanent organisation, with the transfer
itself recorded: what was transferred, in what form, to whom, and until when. This is Domain 16's
responsible-archive obligation seen from the governance end, and it is the provision most often
discovered to be missing at the point where the only people who knew where anything was kept have
left.

**Who may see it, and who may not.** Access to the decision record is a governance decision too.
Declared-interest entries, escalation notes and dissents name identified individuals and record
positions attributed to them, so the log is not a general-circulation document: it carries a stated
access list, and any entry about an identified person attracts the data-protection considerations
Domain 11, KA 11.1.2 sets out for the stakeholder register and Domain 16, KA 16.4.4 sets out for the
archive. Settle the wording, the holding and the access with whoever holds data-protection
accountability in the organisation **before** the log is built rather than after it is requested;
keeping the record itself remains non-optional.

### AI in this KA

**Where it earns its place.** Extracting a structured decision register from meeting minutes and
flagging entries missing an owner, a date or a versioned information reference, a genuinely tedious
task with a clear right answer. Running the RACI integrity checks above across a large matrix.
Detecting re-decided decisions by clustering decision text across a long log, which is exactly the
pattern humans miss because the recurrences are months apart. Testing gate criteria for
assessability and flagging the unmeasurable ones. Modelling gate and escalation economics across
alternative designs, as computed above.

**Where it must not go.** No gate decision, no assurance opinion, and no authorship of a decision
record entry. The record must show the accountable human, and a record generated wholesale by a tool
cannot evidence that a person applied judgement. Nor should a model's summary of a decision's
history be relied on for a dispute or an audit without verification against the source documents:
the summary is a convenience, the versioned source is the evidence.

**Verification, concretely.** Any AI-produced register or audit is checked against the source
minutes on a sampled basis with the sample size stated; every flagged defect is confirmed by a human
before it is reported as a finding; and the gate and latency arithmetic is reproduced by hand,
because all of it is a handful of operations and none of it should ever be taken on trust.

### Key terms — KA 3.3

| Term | Meaning |
|---|---|
| **Stage gate** | A continuation decision by a named authority against pre-set criteria, with power to stop, hold or redirect. |
| **Conditional pass** | Proceeding subject to conditions: a real instrument only if the conditions have owners, dates and consequences. |
| **Three lines of assurance** | Management controls; independent oversight typically inside management; independent audit typically reporting outside it. A model of roles published by a professional body, revised by its owner, voluntary rather than required, a lens for finding gaps and duplication, not an obligatory structure. |
| **Assurance map** | Risks and controls against assurance lines, marking coverage, gaps and duplication. |
| **Assurance capture** | An assurance function assuring work it helped produce, and therefore unable to challenge it. |
| **Escalation class** | A defined trigger, destination, decision, latency and out-of-cycle route. |
| **Out-of-cycle mechanism** | A written-resolution or delegated-authority route used when the ordinary cadence is too slow. |
| **Escalation lead time** | How far before impact an escalation arrives; a shortening trend signals a decaying mechanism. |
| **Decision record** | The versioned, attributable log that converts a decision from a memory into an institutional fact. |
| **Single-A test** | The check that each decision class has exactly one Accountable role. |
| **Effective detection (`q_eff`)** | A gate's detection rate multiplied by its condition-closure rate: `q_eff = q × r`. |
| **Condition-closure rate (`r`)** | The share of a gate's conditions verified closed by the following gate. |
| **Residual exposure** | `Σ pᵢ ∏(1 − qᵢⱼ) uᵢ` across an assurance map; the only quantity that ranks its rows. |
| **Decision action window** | `(remaining duration − escalation latency) ÷ remaining duration`; negative means the escalation is ceremonial. |
| **Confirmation rule** | The provision that stops a tolerance trigger firing on one period's measurement noise. |
| **Re-decision tax** | The committee slots and delay cost consumed by questions that had already been decided. |
| **Cumulative test** | The delegation-schedule provision that related decisions aggregating above `X` within period `P` require the authority appropriate to the aggregate. Setting `X` and `P` is arithmetic, not judgement, and is derived in Domain 4, KA 4.3.3, a round number chosen without reference to the observed decision rate provides the appearance of a control and none of the function. |
| **Relatedness class** | The set over which a cumulative test sums; widening it multiplies false trips and re-centralises the delegation. |
| **Record custodian** | The named role accountable for a record class existing, being complete and being retrievable: one role per class, so that no class is custodied by "the project". |
| **Retention period** | How long a record class is held, set in advance at the longest of contractual limitation, applicable retention requirement, benefits-realisation horizon and records policy; a schedule by class, never one rule, and never a legal position stated by this book. |
| **Custody handover** | The recorded transfer of each record class to a continuing custodian at closure, so records outlive the temporary organisation. |

### Sample MCQs — KA 3.3

**MCQ 3.3-A `[3.3.1 · Application]`** A gate costs 45,000 in review effort and 6 weeks of elapsed
time at a delay cost of 14,280 per week. Expected remediation cost with the gate is 82,800; without
the gate it is 270,000. The gate's net value is:
- A. USD 141,480
- B. USD 56,520 ✅
- C. USD 85,680
- D. USD 187,200

*Rationale:* `270,000 − (45,000 + 85,680 + 82,800) = 56,520` (3.3.1). A omits the delay cost; C is
the delay cost alone; D omits the remediation cost.

**MCQ 3.3-B `[3.3.1 · Evaluation]`** For the gate above, the elapsed time at which it stops adding
value is closest to:
- A. 6 weeks
- B. 8 weeks
- C. 10 weeks ✅
- D. 19 weeks

*Rationale:* `(270,000 − 45,000 − 82,800)/14,280 = 9.96` weeks (3.3.1), the arithmetic behind the
complaint that assurance has become an obstacle.

**MCQ 3.3-C `[3.3.3 · Application]`** Three tiers must approve a decision: 2-weekly with 1-week
papers, 4-weekly with 2-week papers, and 13-weekly with 3-week papers. Total expected latency is:
- A. 9.5 weeks
- B. 15.5 weeks ✅
- C. 19.0 weeks
- D. 6.0 weeks

*Rationale:* `(2/2+1) + (4/2+2) + (13/2+3) = 2.0 + 4.0 + 9.5 = 15.5` (3.3.3). A is the top tier
alone; D counts only half the intervals and omits the paper lead times.

**MCQ 3.3-D `[3.3.2 · Analysis]`** A PMO drafts the delivery plan and later provides second-line
assurance on it. The defect is:
- A. duplication of first-line controls
- B. assurance capture — the function cannot challenge what it produced ✅
- C. a proportionality failure
- D. an assurance gap

*Rationale:* Capture is the most damaging and least visible line failure, because the product still
looks independent (3.3.2).

**MCQ 3.3-E `[3.3.4 · Comprehension]`** The decision-record field most often missing and most
consequential when a decision is later examined is:
- A. the date
- B. the versioned reference to the information relied on ✅
- C. the decision-maker's role
- D. the decision reference number

*Rationale:* The retrospective question is whether the decision was reasonable on what was known at
the time, which only a versioned reference can answer (3.3.4).

**MCQ 3.3-F `[3.3.4 · Analysis]`** In a 12-class decision-rights matrix, 2 classes carry two
Accountable roles and 1 carries none. Which statement is most accurate?
- A. the defect rate is 8.3 % and only the zero-A class matters
- B. the defect rate is 25.0 %; the two-A classes will be decided late or twice, and the zero-A class
  will drift until it becomes an escalation ✅
- C. the matrix is acceptable because 9 of 12 classes are correct
- D. the defect rate is 25.0 % and all three classes fail identically

*Rationale:* `3/12 = 25.0 %`, and the two failure types behave differently under stress, which is
why they are distinguished rather than totalled (3.3.4).

**MCQ 3.3-G `[3.3.1 · Evaluation]`** A gate with a nominal detection rate of 0.80 has a net value of
USD 56,520 and a breakeven detection probability of 55.85 %. Of the 23 conditions it issued, 9 were
verified closed. The best evaluation of the gate is that it:
- A. remains worthwhile, since its detection rate is well above the breakeven
- B. has an effective detection rate of 31.30 %, is therefore below breakeven, and destroys
  USD 57,427.83 ✅
- C. is worthwhile but should be shortened from 6 weeks to 4
- D. has an effective detection rate of 39.13 % and is therefore below breakeven

*Rationale:* `q_eff = q × r = 0.80 × 9/23 = 0.3130`, which is below the 0.5585 breakeven, inverting
the gate's value (3.3.1b). A ignores closure altogether, the commonest error, because governance
reports count conditions issued, not closed. C treats elapsed time as the binding constraint when
the binding constraint is `r`. D mistakes the closure rate itself for the effective detection rate,
omitting the multiplication by `q`. It reaches the right conclusion from the wrong quantity, and
will not survive a gate where `q × r` and `r` fall on opposite sides of the breakeven.

**MCQ 3.3-H `[3.3.2 · Evaluation]`** An assurance map shows all three lines on a risk carrying 7.87 %
of residual exposure and no line on the risk carrying 58.27 % of it. The soundest conclusion is that:
- A. the third line should be discontinued, since its marginal detection value is negative
- B. the third line's effort is pointed at the wrong risk; reallocating it reduces residual exposure at
  constant cost, but third-line existence is not justified on project-level detection economics ✅
- C. the coverage is acceptable because the highest-consequence risk is the one with three lines
- D. the uncovered risk should be accepted, since no line has the competence to assure it

*Rationale:* Reallocating the third line's 30 days from R1 to R3 cut residual exposure by USD
74,361.60 for no extra spend, but third-line assurance exists to opine on the control system across
a portfolio and is mispriced by a single project's detection arithmetic (3.3.2). A over-applies the
arithmetic, the named error. C confuses consequence with residual exposure, which is consequence
*after* detection. D treats a coverage gap as a capability fact.

**MCQ 3.3-I `[3.3.3 · Application]`** A 25-week project reaches week 13 and a trigger fires. The
available escalation path has a total expected latency of 15.5 weeks. The decision action window is:
- A. 66.67 %
- B. 0 %
- C. −29.17 %, so the decision arrives 3.5 weeks after planned completion ✅
- D. 129.17 %

*Rationale:* `(12 − 15.5)/12 = −0.2917` (3.3.3b). A is the window for the 4.0-week single-tier path.
B assumes the window floors at zero, which hides the finding. D computes latency as a share of
remaining duration (15.5/12) rather than the window itself, a real ratio, but not this one.

**MCQ 3.3-J `[3.3.4 · Analysis]`** A programme delegates 12 changes a year by raising its threshold,
saving USD 171,360 in delay. It then adds a cumulative test at the same threshold, summed over
workstreams, which trips 12 times a year. The consequence is that:
- A. the control improves at negligible cost
- B. the trip count equals the number of decisions the delegation released, so the rule cancels the
  saving exactly ✅
- C. the saving falls by about a quarter
- D. the rule is sound but the threshold should be lowered again

*Rationale:* `12 × 0.25 × 57,120 = 171,360`, identical to the saving (3.3.4c). The trip count, driven by
the width of the relatedness class, is the quantity to design against. A is the assumption a governance
review makes when it does not count trips; C invents a partial figure; D compounds the error.

### Self-check — KA 3.3

1. *What does a gate buy, and what does that imply about where gates belong?* — Optionality against
   irreversibility; so gates belong where irreversibility steps up, not at calendar intervals.
2. *Name the assurance failure mode that is hardest to see, and why.* — Capture: the assurance
   product still looks independent while the function cannot challenge what it helped produce.
3. *Which element of an escalation class is most often missing?* — The stated time within which the
   decision will be made, and with it the out-of-cycle mechanism.
4. *State the identity that connects conditional passes to gate value.* — `q_eff = q × r`: an untracked
   condition leaves the defect in the work, so closure rate multiplies detection rate (3.3.1b).
5. *Which row of an assurance map should receive the next assurance day?* — The one with the largest
   `p ∏(1 − q) u`, which is almost always an uncovered risk rather than a duplicated one (3.3.2).
6. *Why must an escalation trigger name a forecast method?* — Because the earned-value family
   produces several legitimate estimates from the same data, and they can straddle the tolerance:
   auriga's three sit at 5.00 %, 10.42 % and 15.20 % of BAC against a 10 % trigger (3.3.3b).
7. *What are the two parameters of a cumulative test, and which one usually goes wrong?* — The aggregate
   threshold `X` and the relatedness class; widening the class is the natural error and it re-centralises
   the delegated band (3.3.4c).
8. *Whose is the "three lines" vocabulary, and what does that change?* — It is an assurance
   architecture published by a professional body, voluntary and since revised by its owner into a
   model of roles rather than sequential defences. It is a lens for finding gaps and duplication; the
   applicable structure is set by the organisation's own governance and, in regulated sectors, by
   what the regulator expects (3.3.2).
9. *Name the four provisions that keep a decision record usable years later.* — A named custodian by
   record class; a system that preserves version and carries any amendment with its amender and
   reason; a retention period set in advance at the longest of the applicable drivers, with its
   source named; and a recorded handover of custody at closure (3.3.4). Retention requirements
   themselves come from the records and legal functions, not from this book.

---

## Advanced topics — Domain 3

### 3.A.1 Governance under stress, and the recovery structure

Governance designed for steady state is tested in crisis, and the failures are consistent: the
cadence is too slow for the decision rate, authority fragments as senior figures intervene
individually, information becomes contested at the moment it must be relied on, and the decision
record (never robust) collapses first, so that a fortnight later nobody can reconstruct why a choice
was made.

A **recovery governance** design is a legitimate and different structure, and it should be prepared
in advance rather than improvised: a single accountable decision-maker with materially raised
authority; a short daily or twice-weekly decision cadence with a standing agenda of decisions rather
than reports; a *narrowed* membership, since crisis governance fails from too many participants
before it fails from too few; a single authoritative information source, with disputes about the data
resolved before decisions rather than during them; and a decision log kept in the room, in real
time, because reconstruction afterwards is precisely what will not be possible.

Two invariants are worth stating because they are what distinguishes recovery governance from an
abandonment of governance. Raised authority must remain **bounded and time-limited**, with a stated
review date, or it does not revert. And the escalation of *bad news* must be made safer, not harder,
because the mechanism's failure mode under stress is silence; and silence is the one condition under
which no governance design of any shape can work.

### 3.A.2 Governance of AI-assisted delivery

Where a project uses AI in its own delivery (estimating, scheduling, code generation, risk analysis,
document review) governance acquires obligations that most existing structures do not express, and
the honest position is that they must be added deliberately.

Four are minimal. **A register of AI uses**, stating for each what the tool does, whose decision it
informs, and who is accountable for the output: the same decidability test as 3.1.1, applied to
tools. **A verification standard proportional to consequence**: an AI-drafted internal summary and
an AI-produced estimate that will set a baseline are not the same object and must not carry the same
review. **A prohibition on unattributable authorship** in anything that will be relied on (a
decision record, an assurance opinion, a board paper conclusion), because Domain 1's accountability
principle requires a person to answer for it, and a tool cannot be that person. And **a data and
confidentiality boundary** stated before use, not after an incident.

The governance body's own duty here is the one most often skipped: to ask, of any AI-informed
recommendation it receives, *what was the model's input, who verified its output, and what would
change the conclusion?* A body that cannot get those three answers is not governing the
recommendation; it is ratifying it.

### 3.A.3 The reviewer's governance eye

Invariants to test on any governance design, each cheap and each diagnostic:

Every decision class has **exactly one** accountable role, and that role's authority is sufficient
under the delegation schedule. The delegation schedule reads on **value, reversibility and
externality**, not value alone. The **latency of every escalation path is stated**, computed as `Σ
(M/2 + L)`, and each tier can be justified against its cost. Every decision body has a **capacity
number** and a demand estimate. Committee agendas show the **share of time on decisions** rather
than reports. Every gate has criteria set in advance, the authority to stop, and a **computed
value** against its elapsed time and detection rate. Conditional passes have owners, dates and
consequences, verified at the following gate. The assurance map shows **no gaps and no
duplication**. Every escalation class has a stated decision deadline and an out-of-cycle route. The
decision log's entries carry a named decision-maker and a **versioned information reference**. And
(the one test that subsumes many of the others) the **re-decision count** is tracked, because a
question arriving at the same body a third time is proof that something upstream of it is broken.

Six further invariants, each answerable from documents that already exist:

Every decision body's utilisation is stated at **average, peak and off-peak**, and the peak is
computed from the actual clustering of change and gate decisions rather than assumed uniform (3.2.2).
Every escalation path carries a **decision action window** as well as a latency, tested against the
remaining duration of the thing being decided; a negative window is a finding, not a nuance (3.3.3b).
Every tolerance trigger names its **forecast method, measurement point and confirmation rule**, and a
trigger stated on a single ratio at a single data date is treated as unspecified (3.3.3b). Every gate's
**condition-closure rate** is counted and multiplied through its detection rate, because a gate reported
as held is not a gate that worked (3.3.1b). The assurance map carries **residual exposure per row**, so
that reallocation is arguable at constant cost rather than being a matter of taste (3.3.2). And any
**cumulative test** is presented with its expected annual trip count and the base-rate aggregate of its
relatedness class, since a rule that trips on ordinary traffic has reversed the delegation it was added
to protect (3.3.4c).

Where the structure is multi-party, two more: the board's **first-presentation approval rate** is
counted from its own minutes, because that is the `p` on which unanimity's cost depends; and the
**reserved-matters list** is read against it, since every item on that list is priced at `pⁿ` (3.1.2).

---

## Industry variations — Domain 3

- **Public sector and government.** Statutory decision rights, published assurance regimes and
  mandatory gates that are genuinely non-negotiable; latency is high by design and the leader's
  lever is almost entirely the paper lead time and the out-of-cycle route, not the cadence. Where a
  statutory path's latency exceeds the remaining duration of what it decides, the **action window is
  negative** and the honest report says so; because the alternative narrative, that the project was
  slow, is the one that will otherwise be recorded (3.3.3b).
- **Regulated industries (pharmaceutical, nuclear, aviation, financial services).** Some gates are
  external and unmovable, and the governance design must place the internal decision *before* the
  external submission with enough margin that a failed internal test does not force a submission. That
  margin is computable rather than a matter of comfort: it is the internal escalation path's action
  window against the time remaining to the submission date, and a negative window means the internal
  test cannot change the submission it was built to protect.
- **Construction and infrastructure.** Multi-party governance is the norm; decision rights follow
  the contract structure, so a governance design inconsistent with the contract loses to the
  contract every time (PFL-AI Domain 11). The **decision rule in the joint-venture agreement is
  usually the largest single latency term** in the whole structure: at four parties each 85 % ready,
  unanimity costs **10.4941 weeks** per decision against **5.7379** under a three-of-four majority
  (Exercise 3.5), and no schedule shows the difference.
- **Technology and product organisations.** Bounded envelopes and continuous funding decisions
  dominate; the characteristic risk is light governance on genuinely irreversible choices
  (architecture, data model, third-party dependency), because they do not present as gates. The
  **reversal-cost ratio** is the instrument that catches them, since it reads authority on
  `max(value, cost to undo)` and those decisions are cheap to take and expensive to unmake (3.2.3).
- **Healthcare.** Clinical governance runs in parallel with project governance and clinical
  authority is not delegable to a project body; Meridian's design must place clinical sign-off with
  clinical authority and integrate rather than absorb it. On the assurance map, clinical sign-off is
  a **line in its own right with its own detection rate**, not a component of the project's first
  line; and where the project helped produce what clinicians are asked to sign, the cell is entered
  as `q = 0` (3.3.2).
- **Energy and resources.** Stage-gated capital processes with substantial assurance at each gate,
  and the standard pathology is gate proliferation, re-approval ceremonies whose elapsed time
  exceeds 3.3.1's breakeven. The second pathology, less discussed and equally expensive, is the
  **unclosed condition**: it reduces a gate's effective detection to `q × r` while reducing its
  elapsed time by nothing at all, so a stage-gated estate that does not count condition closure is
  paying full price for partial gates (3.3.1b).

---

## Case study — Domain 3: the four-week month (health, Meridian)

**Situation.** Twelve weeks into the clinic rollout, Meridian's steering committee was widely
described as "the bottleneck". The committee met monthly, was well attended, and had never refused a
request. The programme was six weeks behind, and the delay was attributed in the quarterly report to
"slower than expected clinic readiness".

**What the arithmetic showed.** The project leader computed three numbers before the next governance
review. Governance latency: `M/2 + L = 4/2 + 2 =` **4 weeks**, the committee that everyone called
monthly imposed a full month's expected wait, and no one in the organisation had believed it was
more than a fortnight. Escalation volume: with a **10,000** delegation threshold, **36** of the
year's **60** change requests required the committee, of which about a quarter sat on the critical
path, costing `9 × 4 × 14,280 =` **USD 514,080** a year in delay. Committee capacity: **104**
item-slots against **77** items of demand, of which **26** were standing reports producing no
decision, so a quarter of the scarcest resource in the programme was being spent on status.

**What changed.** Three changes, none of which removed any scrutiny anyone could name. The paper
lead time went from 2 weeks to 1, taking expected latency to **3.0 weeks**, a full week saved from a
change to an administrative deadline. The delegation threshold went from 10,000 to **25,000**, worth
**USD 171,360** a year against a worst case of 81,600 even if every delegated decision were decided
wrongly and destroyed 40 % of its value. And the standing reports moved to pre-reading, releasing 26
slots and taking utilisation from 74.0 % to **49.0 %**, which is what created room for the
contentious item to be reached.

**The two findings that were not in the original three.** The committee's own members had said the
body was full, and the 74.0 % utilisation figure said it was not. Two further counts reconciled
them. First, demand was not uniform: **40 %** of the 51 decision items fell in the **3** meetings
straddling phase boundaries, giving `51 × 0.40/3 + 2 =` **8.80** items against a capacity of 8, a
peak utilisation of **110.00 %** against **63.25 %** between the boundaries. Second, a clustering of
the decision log's **148** entries found **19** questions that had arrived at the same body more
than once, **6** of them three or more times: **25** repeat slots, **24.04 %** of the committee's
annual capacity, which took real utilisation to **98.08 %** and cost **USD 357,000** a year in delay
on the same 25 % critical-path assumption. **68.42 %** of those repeats carried no versioned
reference to the information relied on. A fourth change followed, and it was the cheapest of the
four: two mandatory fields on the decision-record template.

**The outcome, and the part that mattered.** Escalated decisions began arriving in about three weeks
rather than four, and the peak-month deferrals stopped. But the durable change was to the
*conversation*: "the committee is a bottleneck" is an accusation, while "our governance design costs
514,080 a year in delay, plus 357,000 a year deciding things twice, and here are four changes worth
most of it" is a proposal. The second was approved in one meeting. The first had been raised, in
various forms, for two quarters. Note the ranking the arithmetic produced, which was not the ranking
anyone expected: the largest single item was not the threshold, the cadence or the agenda; it was
the **re-decision tax**, at **2.08 times** the entire saving from the threshold change, and it was
the only one of the four that required no risk-appetite decision from anybody.

**What the domain teaches here.** Governance is a delivery variable with a computable price. Until
the price is computed, governance complaints are cultural and go nowhere; once computed, they are
engineering, and engineering gets approved. And note which lever was largest per unit of
organisational pain: the paper deadline, free, unglamorous, and twice as effective per week as
meeting more often.

## Case study B — Domain 3: the decision nobody made (financial services)

**Situation.** A payments-platform programme completed build 5 % over budget with no approved change
above 50,000, a clean change-control record. A routine internal audit then found that the delivered
scope differed materially from the approved baseline in four respects, none of which appeared in any
decision record.

**What had happened.** Each of the four differences had been assembled from a handful of individual
changes: **20** in all, five per respect, averaging **35,000** and so **175,000** per respect and
**700,000** in total. Each had been discussed in a working group, agreed in substance, and
implemented. None had been raised as a change, because each had been individually below the project
leader's **50,000** authority and had therefore, in the team's understanding, required no decision
at all. One of the four altered a control the second line had assured on the basis of the original
design. The programme's governance was not weak in the ordinary sense: the committee functioned, the
gates were held, the delegation schedule existed. The defect was narrower and more instructive,
**the schedule read on individual value only**, and the decision log recorded decisions *taken by
bodies* rather than decisions *taken*.

**How it resolved.** Three corrections, each mapping to a specific defect. The delegation schedule
acquired **reversibility and externality** dimensions, so a change touching an assured control
required second-line agreement regardless of value. It acquired a **cumulative test**: related
changes aggregating above a threshold within a period required the authority appropriate to the
aggregate, which is the provision that would have caught all four. And the decision log was made the
**register of record for every change to the baseline**, whoever decided it and at whatever value, so
that a decision below a threshold still generated an entry. The audit finding was closed; the
platform went live four weeks late.

**The parameter the first draft of the fix got wrong.** The cumulative rule was initially written at
`X` = **50,000**, the same value as the individual threshold, and summed **over each of the
programme's four workstreams** each quarter, which sounded conservative and was the opposite. The
programme logged **84** sub-threshold changes a year averaging **18,000**, so **USD 1,512,000** of
delegated value, and a workstream-quarter therefore carried a base-rate aggregate of `1,512,000/16
=` **USD 94,500**: nearly twice the trip threshold on ordinary unrelated traffic alone. The rule
would have tripped in **all 16** workstream-quarters, re-escalating routine change and leaving the
delegation nominal. Redrawing the relatedness class at **deliverable level** (**22** classes rather
than 4) dropped the base-rate aggregate to **USD 17,181.82** per class-quarter, put `X` **2.91
times** above it and **3.5 times** below each respect's **175,000** aggregate, and cut the expected
trip count from 16 a year to about **4**: one per respect, a **four-fold** reduction that caught
every cluster the rule existed to catch. On that sizing each respect trips at its **second** change
(running total 70,000 against 50,000), leaving **105,000** of it to be authorised at the aggregate's
level. The lesson the audit report recorded was that **the relatedness class, not the threshold, is
the parameter that decides whether a cumulative test is a control or a re-centralisation.**

**What the domain teaches here.** A decision below a threshold is still a decision and still needs a
record. A delegation schedule that reads only on value cannot see irreversibility, externality or
accumulation (the three ways a small decision becomes a large one), and a decision log that records
only what committees decided will systematically miss the changes that matter most, because those
are exactly the ones that never reached a committee. And a control added to catch accumulation must
be sized against the base rate of the class it sums over, or it will trip on the ordinary traffic it
was never aimed at.

---

## Executive perspective — Domain 3

What a programme director cannot delegate in this domain:

- **The latency of your own governance.** Compute `Σ (M/2 + L)` for every escalation path you own,
  price it at your cost of delay, and be able to state it. An unquantified governance design is one
  you are not managing (3.2.3, 3.3.3).
- **The delegation schedule, on three dimensions.** Value, reversibility, externality, and a
  cumulative test. Read on value alone, it will miss exactly the decisions that matter (Case study
  B).
- **The sponsor's obligations, in writing.** Not the title. The seven testable obligations, agreed
  and evidenced, with turnaround on sponsor decisions monitored like any other lead time (3.2.1).
- **Whether your gates function.** A gate that has never held anything is not working, and its
  elapsed time may already exceed the point at which it destroys value (3.3.1).
- **The decision record's integrity.** Named decision-maker, versioned information reference,
  conditions with owners. This is what makes a defensible decision defensible a year later (3.3.4).
- **Escalation lead time as a leading indicator.** Track how far before impact escalations arrive. A
  shortening trend is the earliest signal that people no longer believe escalation is safe, and it
  appears in no standard report (3.3.3).
- **The action window, not just the latency.** For every escalation path you own, compare its latency
  with the remaining duration of what it decides. Auriga's 15.5-week path on a 25-week project is
  actionable for the first **38.00 %** of it and ceremonial thereafter, and no currency figure shows
  that (3.3.3b).
- **Sponsor allocation as a portfolio decision.** A Meridian-sized sponsorship costs **111.0 hours** a
  year, about **6.03 %** of a working week, so no executive carries more than about **three**. Naming the
  same four people across a dozen programmes manufactures the absent sponsor you will later diagnose as a
  behaviour (3.2.1).
- **What your gates' conditions did next.** Count condition closure, not conditions issued.
  `q_eff = q × r` turned a gate worth **+56,520** into one destroying **57,428** on nothing but a
  tracking failure, and the swing appears in no governance report (3.3.1b).
- **Where your assurance is pointed.** Require residual exposure per row on the assurance map. All three
  of Meridian's lines sat on **7.87 %** of it while **58.27 %** sat uncovered; the correction cost nothing
  (3.3.2).
- **The cost of deciding things twice.** Track unplanned re-decisions. Meridian's ran at **12.84 %** of
  logged decisions, **24.04 %** of committee capacity and **USD 357,000** a year — more than twice the
  saving from its threshold change, and remediable with two fields on a template (3.3.4b).

---

## Calculation exercises — Domain 3

**Exercise 3.1** A committee meets every 8 weeks and closes papers 2 weeks before each meeting; cost
of delay is 14,280 per week. Compute the expected latency and its cost, then compute the saving from
(a) halving the meeting interval to 4 weeks and (b) halving the paper lead time to 1 week.
*Solution.* `E[wait] = 8/2 + 2 =` **6.0 weeks**, costing `6 × 14,280 =` **USD 85,680**.
(a) `4/2 + 2 =` **4.0 weeks** — saves 2.0 weeks, **USD 28,560**. (b) `8/2 + 1 =` **5.0 weeks** —
saves 1.0 week, **USD 14,280**. Note the general rule the two results illustrate: a cut of `x` in
`L` always saves `x`; a cut of `x` in `M` saves `x/2`. Halving looks like the bigger intervention
here only because `M` is four times `L`. Common error: assuming the expected wait is half the
meeting interval, which omits the paper lead time entirely.

**Exercise 3.2** An organisation logs 80 change requests a year: **≤ 5,000 → 30**;
**5,001–20,000 → 20** (average value 11,000); **20,001–75,000 → 22**; **> 75,000 → 8**. The
committee meets every 4 weeks with a 1-week paper lead time; cost of delay is 9,500 per week; 30 % of
escalated changes sit on the critical path. Compute the annual governance delay cost at a 5,000
threshold and at a 20,000 threshold, and the breakeven value-destruction rate on the delegated band.
*Solution.* `E[wait] = 4/2 + 1 =` **3.0 weeks**, so each delaying change costs
`3 × 9,500 =` **28,500**. At 5,000: escalated `80 − 30 = 50`, delaying `50 × 0.30 = 15`, cost
`15 × 28,500 =` **USD 427,500**. At 20,000: escalated `50 − 20 = 30`, delaying **9**, cost
**USD 256,500**. Saving **USD 171,000**. The delegated band is worth `20 × 11,000 =` **220,000**, so
the escalation breaks even only if the delegate destroys `171,000/220,000 =` **77.7 %** of the value
of every delegated decision. Common error: comparing the saving with the *total value* of the
delegated band rather than with a plausible loss on it, which understates the case for delegating by
an order of magnitude.

**Exercise 3.3** A gate costs 30,000 in review effort and 4 weeks of elapsed time at a delay cost of
9,500 per week. A material defect is present with probability 0.25; the gate detects it with
probability 0.75; correction at design costs 80,000 and in build 600,000. Compute the gate's net
value, its breakeven elapsed time and its breakeven detection probability.
*Solution.* Without the gate: `0.25 × 600,000 =` **150,000**. With: review 30,000 + delay
`4 × 9,500 =` 38,000 + expected remediation
`0.25 × (0.75 × 80,000 + 0.25 × 600,000) = 0.25 × 210,000 =` 52,500, total **120,500**. Net value
**USD 29,500**. Breakeven elapsed time `(150,000 − 30,000 − 52,500)/9,500 =` **7.105 weeks**.
Breakeven detection probability: solve `30,000 + 38,000 + 0.25 × (600,000 − 520,000d) = 150,000`
→ `d =` **52.31 %**. Common error: omitting the elapsed-time cost, which makes every gate look
worthwhile and is the reason gates proliferate.

**Exercise 3.4** A decision must pass a delivery board (every 2 weeks, 1-week papers), a portfolio
board (every 6 weeks, 2-week papers) and an investment committee (every 12 weeks, 4-week papers).
Cost of delay is 9,500 per week. Compute the total expected latency and cost, and the saving from a
single-tier path at the portfolio board with a written-resolution procedure of one week. *Solution.*
`2/2 + 1 =` **2.0**; `6/2 + 2 =` **5.0**; `12/2 + 4 =` **10.0**. Total **17.0 weeks**, costing **USD
161,500**: of which the investment committee alone is 10.0 weeks and **95,000**. A one-week
written-resolution path costs **USD 9,500**, a saving of **USD 152,000** (**94.1 %**). For
comparison, retaining the portfolio board on its ordinary cadence as the sole tier gives 5.0 weeks
and **47,500**, saving 114,000. Common error: adding only the meeting intervals and ignoring the
paper lead times, which understates latency here by 7 of the 17 weeks.

**Exercise 3.5** A four-party consortium board meets every 6 weeks with a 2-week paper lead time.
Each party independently arrives able to approve in a given cycle with probability 0.85. Cost of
delay is 9,500 per week. Compute the expected wait and cost for one decision under (a) unanimity and
(b) a three-of-four majority, and compare both with a single integrating authority. *Solution.* Base
wait `M/2 + L = 6/2 + 2 =` **5.0 weeks**, the single-authority case, costing **USD 47,500**. (a)
Unanimity passes with `0.85⁴ =` **0.5220**, so `E[cycles] = 1/0.5220 =` **1.9157** and `E[wait] =
5.0 + 0.9157 × 6 =` **10.4941 weeks**, costing **USD 99,694.09**. (b) Three-of-four passes with `4 ×
0.85³ × 0.15 + 0.85⁴ = 0.3685 + 0.5220 =` **0.8905**, so `E[cycles] =` **1.1230** and `E[wait] = 5.0
+ 0.1230 × 6 =` **5.7379 weeks**, costing **USD 54,510.33**. Unanimity costs **4.7562 weeks** and
**USD 45,183.76** more per decision than the majority rule. Common error: applying the readiness
probability once instead of raising it to the power of the party count. That gives `1/0.85 = 1.1765`
cycles and 6.06 weeks, understating the unanimity penalty by more than four weeks, and it is the
error that makes multi-party boards look survivable on paper.

**Exercise 3.6** The gate of Exercise 3.3 (review 30,000; 4 weeks elapsed at 9,500 per week;
`P(defect)` 0.25; detection 0.75; design fix 80,000; build fix 600,000; net value 29,500; breakeven
detection 52.31 %) issued 18 conditions on a conditional pass, of which 7 were verified closed by
the following gate. Compute the effective detection rate, the gate's net value at that closure rate,
and the closure rate the gate needs to break even. *Solution.* `r = 7/18 =` **38.89 %**, so `q_eff =
0.75 × 0.3889 =` **29.17 %**. Expected remediation `0.25 × (0.2917 × 80,000 + 0.7083 × 600,000) =`
**112,083.33**, so the total with the gate is `30,000 + 38,000 + 112,083.33 =` **180,083.33**
against 150,000 without it: the gate now **destroys USD 30,083.33**, a swing of **USD 59,583.33**
from its nominal +29,500. Breakeven closure rate `r* = d*/q = 0.5231/0.75 =` **69.74 %**, i.e.
**13** of the 18 conditions: at 12 of 18 the gate is still 3,000 underwater, at 13 it returns **USD
2,416.67**. Common error: treating the closure rate as the effective detection rate and reporting
38.89 %, which omits the multiplication by `q` and makes the gate look closer to breakeven than it
is. Note that `r*` here (69.74 %) is close to Meridian's 69.81 % by coincidence, not by rule: `r* =
d*/q` and both terms differ between the two gates.

**Exercise 3.7** Four material risks are assured by two lines. R1: `p` 0.25, `u` 600,000, first line
0.60, second line 0.50. R2: `p` 0.30, `u` 450,000, first line 0.50 only. R3: `p` 0.40, `u` 250,000,
no line. R4: `p` 0.20, `u` 900,000, first line 0.40 only. The second line's contribution is 25 days
at USD 800. Compute total residual exposure, then the reduction available from moving the second
line's 25 days off R1 to (a) R4 and (b) R3, at the same detection rate of 0.50 and the same cost.
*Solution.* Residual `= Σ p ∏(1 − q) u`: R1 `0.25 × 0.20 × 600,000 =` **30,000**; R2 `0.30 × 0.50 ×
450,000 =` **67,500**; R3 `0.40 × 1.00 × 250,000 =` **100,000**; R4 `0.20 × 0.60 × 900,000 =`
**108,000**; total **USD 305,500**. (a) To R4: R1 rises to 60,000, R4 falls to `0.20 × 0.30 ×
900,000 =` **54,000**; total **USD 281,500**, a reduction of **USD 24,000**. (b) To R3: R3 falls to
**50,000**, R4 stays at 108,000; total **USD 285,500**, a reduction of **USD 20,000**. Both
reallocations cost nothing extra, and **(a) is better than (b) by USD 4,000, 16.67 % of the
available gain**. Common error: reallocating to the *uncovered* risk by reflex. The rule is the
largest **residual exposure**, not the largest gap: R4 at 108,000 outranks the wholly uncovered R3
at 100,000, because a high consequence with one weak detector can leave more exposure than a low
consequence with none. (Marginal values: 30,000 on R1, 54,000 on R4, 50,000 on R3, against the
line's 20,000 cost.)

**Exercise 3.8** A 40-week project has BAC 6,000,000. At week 22: PV 3,300,000, EV 3,000,000, AC
3,300,000. The governance framework escalates where "the forecast overrun exceeds 10 % of BAC". The
escalation path runs through three tiers: 3-weekly with 1-week papers, 5-weekly with 2-week papers,
and 13-weekly with 4-week papers. Compute the three standard estimates at completion and test each
against the tolerance; then compute the escalation path's total latency, the decision action window
and the last week at which the path can still deliver an actionable decision. *Solution.* `CPI =
3,000,000/3,300,000 =` **0.9091**; `SPI = 3,000,000/3,300,000 =` **0.9091**. `EAC = AC + (BAC − EV)
=` **6,300,000**, VAC 300,000 = **5.00 %** → no trigger. `EAC = BAC/CPI =` **6,600,000**, VAC
600,000 = **10.00 %** → exactly on the tolerance of 600,000. `EAC = AC + (BAC − EV)/(CPI × SPI) =`
**6,930,000**, VAC 930,000 = **15.50 %** → trigger. Latency `(3/2 + 1) + (5/2 + 2) + (13/2 + 4) =
2.5 + 4.5 + 10.5 =` **17.5 weeks** against a remaining duration of `40 − 22 =` **18 weeks**, so the
action window is `(18 − 17.5)/18 =` **2.78 %** (half a week), and the last week at which the path is
actionable at all is `40 − 17.5 =` **week 22.5**, or **56.25 %** of the duration. Common errors: two
of them. First, reporting "the forecast" without naming a method, when the three methods here
straddle the tolerance and one sits exactly on it, a framework with no named method and no tie-break
rule has not specified its trigger. Second, stopping at the latency and calling 17.5 weeks "slow";
it is not slow, it is **0.5 weeks from being impossible**, and only the action window shows that.

---

## Practitioner's toolkit — Domain 3

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 3.T.1 — Governance design sheet

One page, completed before mobilisation and reviewed at every gate. Rows: each decision body, with
its purpose, members (distinguished from attendees), decision classes it owns, meeting interval `M`,
paper lead time `L`, computed `E[wait]`, capacity (meetings × items) and estimated demand. Below it,
each escalation path with its tiers, the summed latency `Σ (M/2 + L)`, that latency priced at the
project's cost of delay, the **decision action window** against the remaining duration of what the path
decides, and the out-of-cycle mechanism. Two columns that are usually omitted and are worth more than
most of the others: **peak** utilisation as well as average, computed from the actual clustering of
change and gate decisions; and, for a multi-party body, the **first-presentation approval rate** and the
decision rule (unanimity / defined majority / single integrating authority), since `pⁿ` is what prices
the constitution. The sheet's purpose is to make latency and capacity **visible at design time**; a
governance design that cannot fill it in has not been designed.

### Toolkit 3.T.2 — Delegation schedule with a cumulative test

Columns: decision class · value threshold · **estimated cost to undo** and the reversal-cost ratio
`ρ`, with authority read on `max(value, cost to undo)` · externality rating (internal /
cross-functional / external or public), escalating one authority level per step · authority required
· **cumulative rule** (related decisions aggregating above `X` within period `P` require the
authority appropriate to the aggregate) · **relatedness class** for that rule, with its base-rate
aggregate per period and the expected annual trip count · information the delegate must hold to
decide. The three dimensions prevent the value-only failure of Case study B; the cumulative rule
prevents its specific mechanism; the base-rate and trip-count columns are what stop the rule
re-centralising the band it was added to protect (Worked example 3.3.4c); and the final column is
what distinguishes delegation from abdication. Sizing test to apply before the rule goes live: `X`
should sit comfortably above the relatedness class's base-rate aggregate per period and comfortably
below the aggregates of the clusters it must catch, if it cannot, narrow the class rather than
raising `X`.

### Toolkit 3.T.3 — Decision record entry and its integrity checks

Per-decision fields: reference · date · decision-maker (**named person and role**) · decision (one
reading only) · options considered and why this one · information relied on **with versions** ·
**declared interests, with the management action taken and a nil return where there were none**
(Domain 1, KA 1.2.2a) · conditions with owners and dates · review date if provisional. Monthly
integrity checks, each a count: entries missing a named decision-maker; entries missing a versioned
reference; **entries with the declared-interest field blank, neither a declaration nor a nil
return**; **decisions in which an interested party did not abstain, or abstained without the minute
naming who decided instead**; conditions past their date; decision classes with zero or multiple
Accountable roles in the RACI (the single-A test); and **re-decisions**: the same question arriving
at the same body more than twice, counted excluding scheduled reviews of provisional decisions and
re-openings forced by a material change, so that the number measures the defect and not governance
working properly. Every one of these is a number, and a governance function that reports them
monthly will find its defects before an auditor does. Two derived figures belong on the same monthly
page because they convert the counts into an argument: the **re-decision tax** (repeat slots ×
critical-path share × `E[wait]` × cost of delay) and the repeat slots as a **share of committee
capacity**, which is what corrects a capacity model that would otherwise read low by the re-decision
rate.

**The custody block, completed once per record class and reviewed at every gate** (3.3.4): record
class (decision log · change log · gate packs · baseline archive · assurance opinions · acceptance
evidence) · **named custodian role** · where it is held, and whether that system preserves version
and records amendments with the amender and the reason · **retention period**, with the source of
the period named, contractual limitation, an applicable retention requirement, the
benefits-realisation horizon or the organisation's records policy, whichever is longest · disposal
or de-identification action at the end of it · **access list**, since entries naming identified
individuals are not general-circulation material · and the **closure handover**: the continuing
custodian in the permanent organisation, the date custody transfers, and the record of the transfer
itself. Retention requirements and limitation periods are jurisdiction- and sector-specific and come
from the records and legal functions, never from this template. Two monthly counts: record classes
with no named custodian, and classes whose retention period has no stated source.

### Toolkit 3.T.4 — Gate and conditional-pass checklist

Two halves, and the second is the one that is usually missing. **Before the gate:** criteria written and
issued in advance, each objectively assessable, covering the deliverable, the business case re-tested
(not restated), the risks with named owners, the receiving organisation's readiness and the next stage's
plan · the named authority, with the power to stop, hold or redirect stated in writing · the gate's
elapsed time and review cost estimated · and the gate's **computed value** against its breakeven elapsed
time and breakeven detection probability, so that the decision to hold the gate is itself an evidenced
decision.

**After the gate, per condition on any conditional pass:** the condition in words that permit only
one reading (a condition that says "confirm" or "find out" is a hold, and should be recorded as one)
· the named owner · the date · the consequence of non-completion · the verification method · and, at
the following gate, the **closure state**. The three monthly integers this yields are conditions
issued, conditions verified closed and conditions past their date, from which the **closure rate
`r`** and the gate's **effective detection rate `q_eff = q × r`** follow: computed per gate and per
condition type, never as one programme average, because an aggregate closure rate hides the one gate
that closed nothing.

---

## Exam preparation — Domain 3

**What is assessed.** Governance purpose and its separation from management and assurance; structural
weaknesses by organisational form and **the price of a decision rule** (unanimity, defined majority,
single integrating authority); governing iterative and hybrid delivery, and **what share of an iterative
stream a periodic body can serve**; the sponsor's obligations, failure modes, **diary load and turnaround
distribution**; steering-committee design faults and average, peak and hidden utilisation; **the
governance latency formula and its application**; delegation thresholds and their economics on **three
dimensions**; gate purpose, criteria, value and **the conditional pass through `q_eff = q × r`**; the
three assurance lines, their failure modes and **residual exposure across an assurance map**; escalation
design with stated latency, **objective triggers that name a forecast method**, and **the decision action
window**; and the decision record with its integrity checks, its **re-decision tax** and its
**cumulative test**.

**The calculations to be able to do under time pressure.** `E[wait] = M/2 + L` for a single body and
`Σ (M/2 + L)` for a multi-tier path, priced at a cost of delay. Multi-party latency
`M/2 + L + (1/pⁿ − 1)M` under unanimity, and the majority-rule comparison. Servable share of a decision
stream against need-by times. Sponsor load in annual hours and share of a week; turnaround mean, median
and tail share. Escalation volume and delay cost at alternative thresholds, and the breakeven
value-destruction rate on a delegated band. Reversal-cost ratio and authority on `max(value, cost to
undo)`. Gate net value, breakeven elapsed time, breakeven detection probability, and effective detection
`q × r` with the breakeven closure rate `d*/q`. Residual exposure `Σ p ∏(1 − q) u`, the marginal value of
a line on a risk, and the reallocation gain at constant cost. VAC as a share of BAC across the EAC family
and the tolerance test. Decision action window `(remaining duration − latency)/remaining duration`.
Committee capacity, utilisation at average and peak, and repeat slots. RACI single-A defect rate and the
cost of each defect type. Cumulative-test base-rate aggregate and expected trip count.

**The traps.** Taking expected wait as half the meeting interval and forgetting the paper lead time
(Exercise 3.1) · adding meeting intervals without paper lead times in a multi-tier path
(Exercise 3.4) · applying a multi-party readiness probability once instead of as `pⁿ`, which makes
unanimity look survivable (Exercise 3.5) · treating governance artefacts as governance (3.1.1) ·
omitting elapsed-time cost from gate value, which makes every gate look worthwhile (Exercise 3.3) ·
mistaking a condition-closure rate for an effective detection rate, omitting the multiplication by `q`
(Exercise 3.6) · comparing a delegation saving with the total value of the delegated band rather than a
plausible loss on it (Exercise 3.2) · reading average committee utilisation as evidence of capacity when
demand clusters (3.2.2) · reallocating assurance to the *uncovered* risk rather than the largest
**residual** one (Exercise 3.7) · quoting "the forecast" against a tolerance without naming an EAC
method, when the family can straddle the line (Exercise 3.8) · stopping at an escalation path's latency
without testing it against the remaining duration (3.3.3b) · assuming a committee's authority is its
highest member's rather than its binding minimum (3.2.2) · treating a favourable assurance opinion as a
transfer of accountability (3.3.2) · recording only committee decisions and missing the ones taken below
a threshold (Case study B) · sizing a cumulative test on its threshold and ignoring the trip count its
relatedness class generates (3.3.4c) · reading a delegation schedule on value alone.

**How the domain connects.** Domain 1 supplies the accountability principle governance must respect
and the cost of delay every calculation here is priced at. Domain 2 supplies the business case and
kill criteria that gates re-test, and its omitted-column failure is what reappears here as the
largest uncovered row on Meridian's assurance map. Domain 4 integrates the governance of parts into
a whole. Domain 6 consumes governance latency as schedule input (a decision path of 15.5 weeks is a
15.5-week predecessor, whatever the plan says), and supplies the schedule forecast on which the
decision action window should properly be computed. Domain 7's change control is the delegation
schedule in operation, and its earned-value forecasting supplies the EAC family that an escalation
trigger must name. Domain 8's risk escalation uses the escalation classes designed here. Domain 9's
independence rule governs whether two assurance lines may be counted as two detectors. Domain 14, KA
14.3.3 derives the verification standard that 3.A.2 requires here. Domain 15 owns the portfolio
allocation of the scarce sponsor capacity that 3.2.1 sizes. And PFL-AI Domain 11 handles the
risk-allocation face of the multi-party governance problem whose decision-rule cost 3.1.2 prices.

---

## Domain 3 summary
Governance is the decision rights, accountabilities and information flows through which an
organisation directs a project, and it is not management, not assurance, and emphatically not the
reporting apparatus that is usually offered in its place. A governance design must produce
decidability, timeliness, legitimacy and traceability, and each of those is testable.

The domain's contribution is to make governance **computable**. The expected wait for a committee
decision is `M/2 + L`: half the meeting interval plus the whole paper lead time. That single formula
prices a governance design at design time, shows that the administrative deadline is twice the lever
meeting frequency is, and adds across tiers to reveal what an escalation path actually costs: 15.5
weeks and **USD 221,340** for Meridian's three-tier path, of which the quarterly committee alone is
61 %. Extended by the failure probability of a decision *rule*, it prices a constitution: `M/2 + L +
(1/pⁿ − 1)M` puts Meridian's three-party board at **5.4870 weeks** under unanimity against
**4.1152** under a two-of-three majority when the parties are 90 % ready, and at **11.6618** against
**5.1020** when they are 70 % ready. The two rules are close in good conditions and divergent in bad
ones, which is when governance is load-bearing. The same arithmetic prices a delegation threshold:
Meridian's 10,000 threshold cost **USD 514,080** a year, and raising it to 25,000 saves **USD
171,360** against a worst case of 81,600 even if every delegated decision were decided wrongly, a
breakeven value destruction of **84 %** per decision, which no plausible delegate approaches. And it
prices a gate: Meridian's design gate is worth **USD 56,520**, stops paying beyond **9.96 weeks** of
elapsed time, and requires a detection probability above **55.85 %** to be worth holding at all; but
only if its conditions are closed, because `q_eff = q × r` turned that same gate into one destroying
**USD 57,427.83** at an observed closure rate of **39.13 %**, a swing of **USD 113,947.83** with
nothing changed but the tracking.

Three further quantities decide whether a governance design can function at all. **Servable
coverage:** at 4.0 weeks of latency against 2-week increments, Meridian's committee can serve
**12.31 %** of its iterative stream's decisions in time, and doubling its frequency while halving
its paper deadline reaches only **36.92 %**, which is why a bounded envelope is a feasibility
requirement and not a courtesy. **Residual exposure:** `Σ p ∏(1 − q) u` showed all three of
Meridian's assurance lines sitting on the risk carrying **7.87 %** of residual exposure while
**58.27 %** sat wholly uncovered, and reallocating 30 days cut exposure by **USD 74,361.60** at no
additional cost. And the **decision action window:** Project Auriga's 15.5-week escalation path
against 12 weeks of remaining duration is **−29.17 %**. The decision arrives 3.5 weeks after planned
completion, so the path is not slow but ceremonial, and it was actionable only during the first
**38.00 %** of the project.

The sponsor's role is a set of testable obligations, not a title: **111.0 hours** a year, about
**6.03 %** of a working week, which is small enough that absence is a priority choice and large
enough that no executive carries more than about three; and the sponsor's own latency is a
distribution with a tail, where **22.73 %** of Meridian's decisions carried **60.81 %** of the wait
and handling five of them differently removed **43.92 %** of the cost. Steering committees fail in
five predictable ways, four of them designed in, and their true load is not the average: Meridian's
74.0 % concealed **110.00 %** at phase boundaries and, once re-decisions were counted, **98.08 %**
overall. Gates buy optionality against irreversibility and belong where irreversibility steps up.
Assurance is usefully read through the three-line model (a voluntary architecture published by a
professional body and since revised by it, a lens rather than an obligation), whose worst failure is
capture, and whose effort concentrates where review is comfortable rather than where exposure is.
Escalation is a timed pathway with an out-of-cycle route and an objective trigger that **names its
forecast method**: auriga's three estimates at completion sit at **5.00 %**, **10.42 %** and **15.20
%** of BAC against a 10 % tolerance, so an unnamed method delegates the escalation to whoever
prepares the report. And a decision that is not recorded, with a named decision-maker and a
versioned information reference, has not been made. It has been remembered, which cost Meridian
**USD 357,000** a year and **24.04 %** of its committee's capacity in questions decided twice,
remediable with two fields on a template. Case study B's four differences were assembled from **20**
individually authorised changes totalling **700,000**, which is the whole argument for a delegation
schedule that reads on reversibility and externality and aggregates; and its first cumulative rule,
summed over four workstreams, would have tripped in all **16** workstream-quarters on ordinary
traffic, which is why such a rule must be sized against the base rate of the class it sums over. And
a record only answers a question years later if somebody was made responsible for its still being
there: every class of governance record carries a named custodian, a system that preserves version
and carries any amendment with its reason, a retention period set in advance at the longest of the
applicable drivers with its source named, an access list because these records name identified
people, and a recorded handover of custody at closure: the retention requirements themselves coming
from the records and legal functions and from qualified counsel, never from this book.

The through-line: **governance has a price, the price is computable, and until it is computed
governance complaints are cultural and go nowhere.** Compute it, and they become engineering.
