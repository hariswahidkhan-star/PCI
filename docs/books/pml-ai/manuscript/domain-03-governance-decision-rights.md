# Domain 3 — Governance, Organization and Decision Rights

> **Group:** Leading projects (Domain 3 of 4 in Part One). **Target:** ~70 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain continues the **Meridian Care Records** programme from
> Domains 1 and 2 into the structures that decide things, and supplies the governance latency
> formula used later by Domain 6 (schedule), Domain 7 (change control) and Domain 8 (risk
> escalation). British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

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
overhead** — and a leader who cannot price it will be asked to accept a structure that guarantees
delay.

The domain proceeds from purpose to mechanism. KA 3.1 asks what governance is *for*, and separates it
from management, assurance and administration — the three things it is most often confused with —
then works structures across organisational forms and the specific problem of governing iterative
delivery. KA 3.2 covers the two roles that do the deciding: the sponsor, whose obligations are
almost always understated, and the steering body, whose failure modes are predictable; it closes
with decision authorities and thresholds, and prices the cost of setting them too low. KA 3.3 builds
the assurance and gate machinery, designs escalation as a *timed* pathway rather than an
organisation chart, and insists on the decision record — because an unrecorded decision is not a
decision, it is a memory.

**Learning objectives.** After this domain a candidate can: state what governance is for and
distinguish it from management and assurance; describe governance structures across functional,
matrix, projectised and multi-party organisational forms, and identify the accountability weakness
of each; govern iterative and hybrid delivery without either abandoning control or destroying
cadence; specify the sponsor role as a set of testable obligations; diagnose the standard
steering-committee failure modes; **compute the expected latency of a committee decision from its
meeting interval and paper lead time, and use it to price a governance design**; set delegation
thresholds and demonstrate arithmetically when a threshold is too low; design stage gates, compute
whether a gate is worth its elapsed time, and state its breakeven detection rate; distinguish the
three lines of assurance and stop them duplicating; design an escalation path with a stated total
latency and an out-of-cycle mechanism; maintain an auditable decision record; and govern
AI-assisted governance analysis without delegating any decision to it.

**The master programme.** Meridian Care Records continues from Domains 1 and 2 — the clinical-records
rollout to **40 clinics**, benefits **USD 685,440** per year at the realistic 70 % adoption, and the
**cost of delay of USD 14,280 per week** derived in Domain 1. That last figure is what makes this
domain quantitative: every week a decision waits has a price, and Meridian's governance design
spends it without ever appearing in a budget line.

---

## Knowledge Area 3.1 — Governance models

*Topics: 3.1.1 what governance is for · 3.1.2 structures across organisational forms · 3.1.3
governance in agile and hybrid environments.*

### 3.1.1 What governance is for

**Definition.** Project governance is the set of **decision rights, accountabilities and
information flows** through which an organisation directs and controls a project: who may decide
what, on whose authority, on what information, by when, and how the decision is recorded.

Every word of that carries weight, and the definition earns its keep mainly by exclusion.
Governance is **not management** — management runs the work inside the authority granted;
governance grants, bounds and withdraws that authority. It is **not assurance** — assurance forms an
independent opinion on whether the work is likely to succeed; governance decides what to do about
the opinion. And it is **not administration** — a portal, a report template and a monthly slide pack
are governance *artefacts*, and an organisation can have all of them and no governance at all,
because no one in the chain can actually decide anything. The confusion is not academic: the
commonest remedy applied to a struggling project is more reporting, which adds administration,
consumes the scarce attention of the people who could decide, and leaves the decision rights exactly
where they were.

**What governance exists to produce.** Four things, and a governance design should be testable
against each:

1. **Decidability.** Every decision the project will face has exactly one accountable decision-maker
   with sufficient authority. A decision with two owners is undecidable; one with none is unowned.
   This is a countable property, and KA 3.2 counts it.
2. **Timeliness.** The decision arrives while it can still change the outcome. This is the property
   most governance designs never examine, and KA 3.2 makes it computable.
3. **Legitimacy.** The decision is made by someone whose authority the organisation recognises, on
   information it can rely on, through a process it accepts — so the decision *sticks* rather than
   being relitigated by whoever dislikes it.
4. **Traceability.** The decision, its basis and its date can be reconstructed afterwards. This is
   the property that converts a decision from a memory into an institutional fact (3.3.4).

**The two failure directions.** Governance fails by being too heavy or too light, and both are
common in the same organisation at once — heavy on the small decisions that are easy to control and
light on the large ones that are uncomfortable. Heavy governance shows as latency: escalation for
decisions that could have been delegated, agendas too full to reach the difficult item, and a
reporting burden that consumes the delivery capacity it was meant to protect. Light governance shows
as drift: decisions made by whoever is present, scope and budget changing without a decision-maker,
and a project that no one can stop because no one is entitled to.

The professional discipline is therefore **proportionality, designed deliberately**. A governance
structure is a design artefact with inputs (the project's value, novelty, risk, reversibility and
external exposure) and outputs (tiers, thresholds, cadences, gates). Copying the structure used for
the last programme is not proportionality; it is inheritance.

**Governance and the leader's accountability.** Domain 1's distinction returns here with force.
Governance can allocate *responsibility* — the doing — freely, and should. It cannot dissolve the
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
functional manager and treated as breached when unilaterally changed — not a request, a commitment.

**Matrix (weak, balanced, strong).** Authority is shared on a spectrum. *Characteristic weakness:*
**dual reporting without a stated precedence rule**, which does not split authority evenly but
transfers it to whichever manager escalates harder. The countermeasure is a precedence rule written
in advance for the specific conflicts that will arise — who wins on priority of work, on technical
approach, on performance assessment — because a matrix without one is a functional organisation that
holds project meetings.

**Projectised.** The project owns its resources and the leader holds full authority.
*Characteristic weakness:* isolation from the enterprise — locally optimal decisions, divergent
standards, and a benefits chain that ends where the project ends, precisely the failure Domain 2's
benefits map addresses. The countermeasure is enterprise representation in the governance body with
a real veto on the standards that matter, and a receiving-organisation owner for each benefit.

**Multi-party (joint ventures, consortia, public–private).** Two or more organisations with distinct
interests, and often distinct legal duties. *Characteristic weakness:* **governance by unanimity**,
which is indistinguishable from an inability to decide, particularly under stress when interests
diverge. The countermeasures are structural and belong in the agreement, not in a terms of
reference: a defined majority for defined classes of decision, a reserved-matters list that genuinely
requires unanimity and is kept short, a deadlock-breaking mechanism with a deadline, and a single
integrating authority for day-to-day direction. PFL-AI Domain 11 treats the risk-allocation side of
the same problem; the governance side is that a structure where any party can stop everything will
eventually be used that way.

**Programme and portfolio layers.** Where a project sits inside a programme inside a portfolio, each
layer adds a tier of potential escalation — and, unmanaged, adds its latency to every decision that
travels up. KA 3.3 computes what that costs and why the design rule is **the fewest tiers a decision
must legitimately pass through**, with the others informed rather than consulted.

**A note on outsourced delivery.** Where a supplier delivers, governance has to span the contract
boundary, and the boundary is where decision rights are most often left undefined. The specific
questions to settle before mobilisation: who may approve a change (and at what value on each side —
the two thresholds are rarely equal and the mismatch is where change control fails); whose
governance body is authoritative when they disagree; what the supplier is contractually obliged to
escalate and within what period; and what information the client is entitled to, in what form, on
what cadence. Domain 7 handles the commercial mechanics; the governance point is that a contract
that does not allocate decision rights has not allocated the work.

### 3.1.3 Governance in agile and hybrid environments

Iterative delivery does not remove the need for governance; it changes what governance is asked to
decide, and a structure designed for sequential delivery applied unchanged to iterative delivery
fails in a specific, predictable way.

**What changes.** In sequential delivery, governance approves a plan and then controls variance
against it. In iterative delivery the increment is the unit of decision, the backlog is the plan, and
the governance question is not "is the project on plan?" but **"is the value being produced worth
the next increment's cost, and is the direction still right?"** That converts governance from
variance control to **continuation decisions at a cadence** — which is a more honest posture, and
also a more demanding one, because it requires a decision-maker who can say "stop" repeatedly rather
than once at a gate.

**The characteristic failure.** A monthly steering committee governing two-week sprints is *always
behind*, and worse, its latency (computed in KA 3.2) exceeds the cycle it is governing — so the team
either waits, destroying flow, or proceeds and seeks retrospective approval, destroying the
governance. Both outcomes are attributed to the team. Neither is the team's doing.

**The design response**, in four moves:

- **Delegate inside a bounded envelope.** The team decides freely within stated bounds — scope
  within the agreed increment, technical approach within the architecture, spend within a period
  budget — and the governance body sets and reviews the envelope rather than the decisions inside it.
  This is the single highest-value governance decision in iterative delivery, and KA 3.2 prices it.
- **Match cadence to the work.** Governance interaction at the increment's rhythm — increment
  reviews as the decision point, with the committee attending rather than being reported to.
- **Govern outcomes, not activity.** Working software or a released service demonstrated, with the
  benefit measure attached (Domain 2's map), rather than percentage-complete against a plan that the
  method deliberately does not have.
- **Keep the gates that carry real optionality.** Funding tranches, go-live authorisation and
  regulatory approvals remain genuine gates because they are genuinely irreversible. What should be
  removed are the gates that merely re-approve a decision already taken.

**Hybrid honestly.** Most real programmes are hybrid — an iterative build inside a sequential
infrastructure or regulatory frame — and the governance failure is to apply one model to both parts.
Meridian is exactly this shape: an iteratively developed records application, a sequential clinic
rollout with immovable estate and training dependencies, and a regulatory approval that is a genuine
gate. The workable design governs each part on its own terms and holds a **single integrated view of
the whole** for the decisions that span them — which is the topic of Domain 4.

### AI in this KA

AI is useful in governance analysis and dangerous in governance decisions, and the line is bright.

**Where it earns its place.** Reading a set of terms of reference, delegation schedules and contract
schedules against each other and listing the decisions with no named owner, two owners, or
conflicting thresholds — a document-comparison task at which it is fast and thorough, and one humans
do badly because it is tedious. Extracting the decision log from meeting minutes into a structured
register, flagging items recorded without an owner or a date. Summarising a decision's history for a
board paper. Modelling the latency of a proposed governance design (KA 3.2's arithmetic) across
alternative cadences, which is deterministic and verifiable.

**Where it must not go.** No decision, and no *recommendation presented as a decision*. Governance
authority is conferred on accountable people; it cannot be exercised by a system that cannot be
answerable, and an organisation that lets a model's output stand unchallenged as a governance
conclusion has created accountability without a holder — the exact defect Domain 1 identified.

**The governing principle, applied.** AI proposes; the professional verifies, decides and remains
accountable. Concretely: every AI-produced governance analysis is reviewed by the named
accountable person before it reaches a decision body, its inputs are stated, its conclusions are
independently checked on the material items, and the decision record shows a human decision-maker —
never a tool — as the author of the decision.

### Key terms — KA 3.1

| Term | Meaning |
|---|---|
| **Governance** | The decision rights, accountabilities and information flows through which an organisation directs and controls a project. |
| **Decision right** | The authority to make a specified class of decision, bounded by value, scope and reversibility. |
| **Decidability** | The property that every decision has exactly one accountable decision-maker with sufficient authority. |
| **Reserved matter** | A decision class that requires the highest (often unanimous) authority; kept deliberately short. |
| **Bounded envelope** | Stated limits within which a delivery team decides freely, reviewed rather than approved decision by decision. |
| **Precedence rule** | The pre-agreed rule for which authority prevails in a matrix conflict. |
| **Governance artefact** | A report, portal or template — evidence of governance, never a substitute for decision rights. |

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

### Self-check — KA 3.1

1. *Name the four things a governance design must produce.* — Decidability, timeliness, legitimacy,
   traceability.
2. *Why is "more reporting" the wrong remedy for a struggling project's governance?* — It adds
   administration and consumes decision-makers' attention while leaving decision rights unchanged.
3. *What does a project leader still owe after a steering committee decides badly?* — The
   obligations to have framed the decision honestly, surfaced the material information, stated the
   recommendation, and escalated in time — all assessable from the record.

---

## Knowledge Area 3.2 — Sponsorship and steering

*Topics: 3.2.1 the sponsor role · 3.2.2 steering committees that work · 3.2.3 decision authorities
and thresholds.*

### 3.2.1 The sponsor role

**Definition.** The sponsor is the individual accountable for the project's **business outcome** —
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
*The absent sponsor* — a name on a chart, no diary time; test: the count of sponsor decisions in the
last quarter, and their turnaround. *The delegating sponsor* — a deputy attends everything, so
authority is in the room but accountability is not; test: whether the last three material decisions
were made by the sponsor or reported to them afterwards. *The advocate sponsor* — committed to the
project rather than the outcome, and therefore unable to stop it; test: whether they can state a
condition under which they would recommend stopping (Domain 2's escalation of commitment, at the
governance level). *The operational sponsor* — drawn into managing delivery, which vacates the role
that only they can hold; test: whether the sponsor's contributions in the last three meetings were
about outcome and mandate, or about task sequence.

**What a project leader does with a weak sponsor.** This is a real and common situation, and the
professional response is neither to complain nor to absorb the gap silently — absorbing it is
attractive, because it feels like competence, and it is how a leader ends up accountable for a
mandate they were never given. The workable response is procedural: write the decisions needed and
their dates; send them with a recommendation and a stated consequence of non-decision; record the
non-decision when it occurs; escalate the *pattern* rather than the individual instance once it
recurs; and keep the record. That is not politics. It is the mechanism by which an organisation's
governance failure becomes visible to the organisation while there is still time to fix it — and,
incidentally, the only defensible position for the leader afterwards.

### 3.2.2 Steering committees that work

A steering committee exists to make the decisions that exceed the project leader's authority, in
time for them to matter. Almost every observed dysfunction traces to one of five design faults.

**Fault 1 — membership without authority.** Attendees who must consult before agreeing. The
committee's effective authority is the *minimum* of its members' authority on the decision in hand,
not the maximum, so one under-empowered member on a decision class blocks it. Test: for each
decision class in the delegation schedule, name the member who can commit.

**Fault 2 — membership too large.** Beyond roughly six to eight decision-makers the body becomes a
briefing audience: contributions become positional, dissent becomes private, and the difficult item
gets deferred. Attendance is not membership — the separation of *members* (who decide) from
*attendees* (who inform) is the cheapest available fix.

**Fault 3 — the agenda consumed by reporting.** Status occupies the meeting and the decisions arrive
in the last ten minutes, which is why the reliable diagnostic of committee health is not attendance
but the **share of agenda time spent on decisions**. Status is read in advance; the meeting decides.

**Fault 4 — cadence mismatched to the work.** Treated quantitatively in 3.2.3 — and it is the fault
with the largest measurable price.

**Fault 5 — no capacity model.** A committee has finite throughput, and a design that routes more
decisions to it than it can hear does not slow decisions down evenly: the items that get deferred are
the contentious ones, which are the ones that needed the decision. The capacity arithmetic is
elementary and almost never done.

**Worked example 3.2.2 — does Meridian's steering committee have the capacity to govern?**

1. **Setup.** Meridian's steering committee meets every **4 weeks** (**13** meetings a year) and can
   handle **8** substantive agenda items per meeting. Annual demand: **36** escalated change
   requests (the count above the current delegation threshold, from 3.2.3), **26** standing reports
   (two per meeting), and **15** gate and assurance decisions.
2. **Formula.** Capacity = meetings × items per meeting. Utilisation = demand ÷ capacity.
3. **Substitution.** Capacity `13 × 8 = 104`. Demand `36 + 26 + 15 = 77`. `77/104`.
4. **Result.** Capacity **104** item-slots; demand **77**; utilisation **74.0 %**.
5. **Interpretation.** Seventy-four per cent looks comfortable and is not, for two reasons a leader
   should be able to state. First, demand is **not uniform** — changes and gate decisions cluster
   around phase boundaries, so the peak months exceed capacity while the average does not, and it is
   in the peak months that decisions are deferred. Second, the 26 standing reports consume a quarter
   of the committee's total capacity producing no decisions at all (Fault 3): moving them to
   pre-reading releases 26 slots and drops utilisation to **49.0 %**, which is the headroom a
   contentious item needs. Raising the delegation threshold as 3.2.3 recommends removes a further 12
   items, taking demand to **65** and utilisation to **62.5 %** on the original agenda design. The
   professional point is that committee capacity is a *designed* quantity, and a governance structure
   proposed without this arithmetic has not been designed.

### 3.2.3 Decision authorities and thresholds

**The structure.** A delegation schedule states, for each decision class, the level of authority
required — typically as a monetary threshold, but the better schedules use three dimensions:
**value** (how much), **reversibility** (how hard to undo), and **externality** (who outside the
project is affected). A cheap, irreversible, externally visible decision may deserve more authority
than an expensive, reversible, internal one; a schedule that reads only on value cannot express that.

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
that says — a *monthly* committee with a two-week paper deadline imposes an expected wait of a full
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
4. **Result.** Annual governance delay cost **USD 514,080** at a 10,000 threshold and **USD 342,720**
   at 25,000 — a saving of **USD 171,360** a year. The band being delegated is 12 changes worth
   **204,000** in total. Even in the **worst imaginable case** — the delegate decides *every one of
   the twelve* wrongly and destroys **40 %** of the value each time — the loss is
   `12 × 17,000 × 0.40 =` **USD 81,600**, leaving the delegation ahead by **USD 89,760**. The
   threshold at which the two are equal is a value destruction of `171,360/204,000 =` **84 %** of
   every delegated decision.
5. **Interpretation.** The escalation cannot be justified at any plausible error rate — the delegate
   would have to destroy 84 % of the value of *every* delegated decision for the committee's
   involvement to break even, and a delegate who did that would be removed for reasons unrelated to
   thresholds. That is the general shape of the result, and it is why over-centralised delegation
   schedules are so common and so expensive: **the cost of escalation is certain, recurring and
   invisible, while the cost of a delegated error is uncertain, occasional and highly visible.**
   Organisations optimise against the visible cost. Three professional cautions, however. The
   calculation prices *delay*, not scrutiny — where a decision class carries irreversibility or
   external exposure the value test is the wrong test, which is why the schedule reads on three
   dimensions and not one. The 25 % critical-path share is an *assumption from history* and must be
   stated as one, since the conclusion is proportional to it — though note that it would have to
   fall below `81,600/(12 × 57,120) =` **11.9 %** before the worst case even competed. And a raised
   threshold requires the delegate to have the *information* to decide, which is a real
   prerequisite: delegation without information is abdication.

### AI in this KA

**Where it earns its place.** Modelling latency across candidate governance designs — the
arithmetic above, over dozens of cadence and threshold combinations, is deterministic and a natural
fit. Classifying a change-request history into value bands and testing whether a proposed threshold
would have altered any actual outcome — a genuinely useful retrospective test, and one nobody has
time to do by hand. Drafting a delegation schedule from an existing one plus a set of stated changes,
for human review. Detecting the decidability defects of 3.1.1 across a document set — decisions with
no owner, two owners, or thresholds that conflict between the contract and the terms of reference.

**Where it must not go.** Setting a threshold, which is a risk-appetite decision belonging to the
sponsor and the accountable body. Estimating the critical-path share or the error rate from
plausibility rather than data — a model asked for these will produce confident numbers with no
provenance, and they will then drive a real decision. Nor should its latency model be trusted
unverified: the formula is two operations, and a leader who cannot reproduce `M/2 + L` on paper
should not be quoting it.

**Verification, concretely.** Reproduce every number by hand, state the assumptions with their
sources, and put the sensitivity in the paper — the breakeven error rate and the breakeven
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
| **Committee capacity** | Meetings per year × substantive items per meeting; compared against decision demand. |
| **Member vs attendee** | Members decide; attendees inform. Conflating them enlarges the body and reduces its authority. |

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
cutting `M` to 3 gives 3.5 (saves 0.5) — a one-week cut in the paper lead time always saves a full
week, a one-week cut in the meeting interval only half of one (3.2.3). The administrative deadline is
also the cheaper lever to move.

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

### Self-check — KA 3.2

1. *State the governance latency formula and both of its levers.* — `E[wait] = M/2 + L`; shorten the
   paper lead time (a full week per week cut) or the meeting interval (half a week per week cut).
2. *Why do organisations systematically set delegation thresholds too low?* — The cost of escalation
   is certain, recurring and invisible; the cost of a delegated error is uncertain, occasional and
   highly visible, so optimisation runs against the visible cost.
3. *What must accompany a raised threshold for it to be delegation rather than abdication?* — The
   information, criteria and support the delegate needs to decide, plus a record of what they decided.

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
— before major commitment, before build, before go-live, before regulatory submission — and not at
regular calendar intervals, which is how they proliferate into re-approval ceremonies.

**Gate criteria.** Criteria must be set **in advance**, be **objectively assessable**, and cover
both the work and the decision: is the deliverable adequate; is the business case still valid
(Domain 2's benefits and assumptions, re-tested rather than restated); are the risks acceptable and
owned; is the receiving organisation ready; and is the plan for the next stage credible? Criteria
written after the evidence is available are not criteria.

**The honest gate.** A gate that has never held or stopped anything is not evidence of good
delivery, it is evidence of a gate that does not function — and its cost is being paid for nothing.
The pattern to watch for is the **conditional pass**: proceed subject to conditions, which are then
neither tracked nor enforced. A conditional pass is a genuine and useful instrument, and it is only
that if the conditions have owners, dates and a stated consequence of non-completion, verified at
the next gate.

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
5. **Interpretation.** The gate pays — and the useful output is not the 56,520 but the two
   **breakeven points** it implies, because those are what a leader negotiates with. Holding
   everything else, the gate stops paying once its elapsed time reaches
   `(270,000 − 45,000 − 82,800)/14,280 =` **9.96 weeks**: a gate that takes ten weeks destroys the
   value it exists to protect, which is the arithmetic behind the familiar and usually unquantified
   complaint that assurance has become an obstacle. And at the actual 6 weeks, the gate needs a
   detection probability above **55.85 %** to be worth holding — so a review staffed by people who
   cannot competently detect the defect is worse than no review, because it costs the money and the
   delay and returns nothing. Both results generalise: **gate value is destroyed by elapsed time and
   by weak detection**, and a leader whose gate is under challenge should compute which of the two is
   the actual problem before defending or abandoning it.

### 3.3.2 Assurance lines

**The three lines, and what each is for.**

- **First line — management.** The project's own controls: reviews, testing, quality checks,
  reporting. Owned by the project leader, and the only line that can prevent a defect rather than
  detect it.
- **Second line — oversight function.** A PMO, risk or quality function providing independent
  challenge while remaining inside management's chain. Its value is comparability across the
  portfolio and pattern detection no single project can see.
- **Third line — internal audit (and external assurance).** Independent of management, reporting to
  the audit committee or equivalent, forming an opinion on whether the whole control system works.

**The failure modes.** *Duplication* — three lines asking the same questions, which multiplies cost
and destroys the project team's willingness to engage. *Gap* — everyone assumes another line covers
a risk. *Capture* — the second line drafts the plan it later assures, and so cannot challenge it;
this is the most damaging and the least visible, because the assurance product still looks
independent. *Assurance as accountability transfer* — a leader treats a favourable assurance opinion
as a discharge of their own obligation; Domain 1's principle applies unchanged, an opinion is
information and not a transfer of accountability.

**The countermeasure** is an **assurance map**: risks and controls down the side, lines across the
top, and each cell marked as covered, not covered or duplicated — reviewed by the accountable
authority. It is a half-day artefact that surfaces both gaps and duplication, and its absence is the
usual reason a project is simultaneously over-assured and unassured.

**Proportionality.** Assurance effort should scale with novelty, value, irreversibility and external
exposure — not uniformly, and not with organisational anxiety. A high-value, low-novelty, reversible
project may need less assurance than a small, novel, irreversible, publicly visible one, and a
regime that cannot express that will over-assure the first and under-assure the second.

### 3.3.3 Escalation design

**The principle.** Escalation is a **designed pathway with stated latency**, not an instruction to
tell someone senior. A usable design states, for each escalation class: the trigger (objective, not
"if concerned"), the destination, the decision required, the **time within which the decision will
be made**, and the out-of-cycle mechanism if the ordinary cadence is too slow.

That fourth element is the one usually missing, and its absence has a computable cost.

**Worked example 3.3.3 — the total latency of Meridian's escalation path.**

1. **Setup.** A decision requiring executive authority passes three tiers: the **project board**
   (meets every 2 weeks, papers close 1 week ahead), the **programme board** (every 4 weeks, papers
   2 weeks ahead), and the **executive committee** (quarterly — every 13 weeks — papers 3 weeks
   ahead). Cost of delay **14,280** per week.
2. **Formula.** Total expected latency = Σ over tiers of `M/2 + L`. Cost = latency × cost of delay.
3. **Substitution.** Project board `2/2 + 1 = 2.0`; programme board `4/2 + 2 = 4.0`; executive
   committee `13/2 + 3 = 9.5`. Total `2.0 + 4.0 + 9.5`.
4. **Result.** **15.5 weeks** of expected latency for a single decision, costing **USD 221,340** in
   delay — before anyone in any of the three rooms disagrees with the recommendation.
5. **Interpretation.** Two hundred and twenty-one thousand dollars is the price of an organisation
   chart, and it is invisible: it appears in no budget, is attributed to no decision, and is
   generally described afterwards as the project having been slow. Notice the distribution — the
   quarterly committee alone accounts for **9.5 of the 15.5 weeks** (**61 %**) and **USD 135,660**,
   which is what makes "add the executive committee to the approval path" such an expensive sentence
   when spoken casually in a governance review. Two redesigns follow directly. **Reduce the tiers a
   decision must legitimately pass:** if the programme board can decide with the executive committee
   *informed*, latency falls to **4.0 weeks** and **USD 57,120** — a saving of **USD 164,220**, or
   **74.2 %**, from a change that removes no scrutiny that anyone can name. **Add an out-of-cycle
   mechanism:** a written-resolution procedure with a five-working-day turnaround makes the
   single-tier path **1.0 week** and **USD 14,280**, a **93.5 %** reduction against the original.
   The general rule this supports: **count the tiers, add the latency, price it, and then justify
   each tier against its cost** — which is a conversation governance reviews almost never have,
   because until the latency is added up there is nothing to weigh the tier against.

> **Fig 3.3.1 — The price of an escalation path.** Horizontal stacked bar chart. Bar 1, "as
> designed": three segments — project board **2.0 w**, programme board **4.0 w**, executive
> committee **9.5 w** — totalling **15.5 weeks / USD 221,340**, with the executive segment labelled
> "61 % of the latency". Bar 2, "one tier, executive informed": **4.0 w / USD 57,120**. Bar 3, "one
> tier with written resolution": **1.0 w / USD 14,280**. A right-hand column shows savings of
> **164,220 (74.2 %)** and **207,060 (93.5 %)**. Source: PCI original. Alt text: three horizontal
> bars of sharply decreasing length showing escalation latency falling from 15.5 weeks to 1 week as
> tiers are removed and an out-of-cycle mechanism is added.

**Designing the trigger.** Objective triggers only: a forecast variance beyond a stated tolerance,
a risk exposure above a stated threshold, a decision required above the delegated authority, a
dependency breached. "Escalate if concerned" produces two failure modes at once — late escalation by
those who fear it and constant escalation by those who fear the alternative — and both are then
treated as individual judgement problems rather than as the design defect they are.

**The escalation culture problem.** Escalation is only a functioning mechanism if using it is safe.
Where escalation is read as failure, it happens late, which is precisely when the decision can no
longer help. The countermeasures are governance ones and belong to the sponsor and the committee:
respond to early escalation visibly and well; separate the escalation of an *issue* from any
judgement about the person raising it; and track the **lead time** of escalations — how far before
the impact they arrive — because a shortening lead time is the earliest available signal that the
mechanism is decaying, and it appears in no standard report.

### 3.3.4 Auditability and the decision record

**The principle.** A decision that is not recorded has not been made — it has been remembered, and
memories diverge exactly when the stakes rise. The decision record is the mechanism that converts a
meeting into an institutional fact.

**What a decision record must contain**, per decision, and it is short enough that its absence is
never a resourcing problem: a unique reference; the date; the decision-maker by name and role (never
a committee alone — a body records the decision, a person is accountable for it); the decision, in
words that permit only one reading; the options considered and why the chosen one was chosen; the
information relied on, referenced to its version; the conditions attached, with owners and dates;
and the review date if the decision is provisional.

**Why the "information relied on, referenced to its version" line matters more than it looks.**
When a decision is examined afterwards — in a lessons review, a dispute, an audit or an inquiry —
the question is almost never "was the decision right?" but **"was it reasonable on what was known
at the time?"** Only a versioned reference can answer that, and its absence converts a defensible
decision into an indefensible one at exactly the moment the defence is needed.

**Two governance defects the record exposes**, and this is the practical case for keeping it well.
The **re-decided decision**: the same question arriving at the same body a third time is a symptom of
either an unrecorded decision or an illegitimate one, and the log makes the recurrence visible where
minutes do not. And the **decision nobody made**: a change that took effect without an entry, which
is how scope and budget move without anyone having decided — the most common finding of a
governance audit and the hardest to see from inside.

**RACI and its integrity check.** Responsibility assignment matrices — Responsible, Accountable,
Consulted, Informed — are the standard artefact for decision rights, and their standard failure is
being drafted, circulated and never checked. The checks are countable, and a matrix that fails any of
them is not a matrix but a document: **exactly one A** per decision (two make it undecidable, none
make it unowned); the A holds sufficient authority under the delegation schedule; C and R are
distinguished (consultation is not agreement); and the count of C's per decision is bounded, since
consultation is where latency accumulates invisibly.

**Worked example 3.3.4 — auditing Meridian's decision-rights matrix.**

1. **Setup.** Meridian's matrix covers **12** decision classes across 7 roles. On review: **9**
   classes carry exactly one Accountable, **2** carry two, and **1** carries none.
2. **Formula.** Defect rate = classes failing the single-A test ÷ total classes.
3. **Substitution.** `(2 + 1)/12`.
4. **Result.** **3** of 12 classes are defective — a **25.0 %** defect rate.
5. **Interpretation.** A quarter of the programme's decision classes cannot be decided as documented,
   and the two failure types behave differently under stress, which is why they are worth
   distinguishing rather than totalling. The **two-A** classes will be decided — by whichever holder
   acts first, or after a delay while they reconcile, and the delay is at 3.2.3's committee rates.
   The **zero-A** class will not be decided at all: it will drift until it becomes an escalation,
   which is the worst of the available outcomes because it arrives late and without preparation.
   The test costs an hour and it is the highest-yield governance check available before mobilisation.

### AI in this KA

**Where it earns its place.** Extracting a structured decision register from meeting minutes and
flagging entries missing an owner, a date or a versioned information reference — a genuinely tedious
task with a clear right answer. Running the RACI integrity checks above across a large matrix.
Detecting re-decided decisions by clustering decision text across a long log, which is exactly the
pattern humans miss because the recurrences are months apart. Testing gate criteria for
assessability and flagging the unmeasurable ones. Modelling gate and escalation economics across
alternative designs, as computed above.

**Where it must not go.** No gate decision, no assurance opinion, and no authorship of a decision
record entry — the record must show the accountable human, and a record generated wholesale by a
tool cannot evidence that a person applied judgement. Nor should a model's summary of a decision's
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
| **Conditional pass** | Proceeding subject to conditions — a real instrument only if the conditions have owners, dates and consequences. |
| **Three lines of assurance** | Management controls; independent oversight inside management; independent audit outside it. |
| **Assurance map** | Risks and controls against assurance lines, marking coverage, gaps and duplication. |
| **Assurance capture** | An assurance function assuring work it helped produce, and therefore unable to challenge it. |
| **Escalation class** | A defined trigger, destination, decision, latency and out-of-cycle route. |
| **Out-of-cycle mechanism** | A written-resolution or delegated-authority route used when the ordinary cadence is too slow. |
| **Escalation lead time** | How far before impact an escalation arrives; a shortening trend signals a decaying mechanism. |
| **Decision record** | The versioned, attributable log that converts a decision from a memory into an institutional fact. |
| **Single-A test** | The check that each decision class has exactly one Accountable role. |

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

*Rationale:* `(270,000 − 45,000 − 82,800)/14,280 = 9.96` weeks (3.3.1) — the arithmetic behind the
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

### Self-check — KA 3.3

1. *What does a gate buy, and what does that imply about where gates belong?* — Optionality against
   irreversibility; so gates belong where irreversibility steps up, not at calendar intervals.
2. *Name the assurance failure mode that is hardest to see, and why.* — Capture: the assurance
   product still looks independent while the function cannot challenge what it helped produce.
3. *Which element of an escalation class is most often missing?* — The stated time within which the
   decision will be made, and with it the out-of-cycle mechanism.

---

## Advanced topics — Domain 3

### 3.A.1 Governance under stress, and the recovery structure

Governance designed for steady state is tested in crisis, and the failures are consistent: the
cadence is too slow for the decision rate, authority fragments as senior figures intervene
individually, information becomes contested at the moment it must be relied on, and the decision
record — never robust — collapses first, so that a fortnight later nobody can reconstruct why a
choice was made.

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
because the mechanism's failure mode under stress is silence — and silence is the one condition
under which no governance design of any shape can work.

### 3.A.2 Governance of AI-assisted delivery

Where a project uses AI in its own delivery — estimating, scheduling, code generation, risk
analysis, document review — governance acquires obligations that most existing structures do not
express, and the honest position is that they must be added deliberately.

Four are minimal. **A register of AI uses**, stating for each what the tool does, whose decision it
informs, and who is accountable for the output — the same decidability test as 3.1.1, applied to
tools. **A verification standard proportional to consequence**: an AI-drafted internal summary and
an AI-produced estimate that will set a baseline are not the same object and must not carry the same
review. **A prohibition on unattributable authorship** in anything that will be relied on — a
decision record, an assurance opinion, a board paper conclusion — because Domain 1's accountability
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
externality**, not value alone. The **latency of every escalation path is stated**, computed as
`Σ (M/2 + L)`, and each tier can be justified against its cost. Every decision body has a **capacity
number** and a demand estimate. Committee agendas show the **share of time on decisions** rather than
reports. Every gate has criteria set in advance, the authority to stop, and a **computed value**
against its elapsed time and detection rate. Conditional passes have owners, dates and consequences,
verified at the following gate. The assurance map shows **no gaps and no duplication**. Every
escalation class has a stated decision deadline and an out-of-cycle route. The decision log's entries
carry a named decision-maker and a **versioned information reference**. And — the one test that
subsumes many of the others — the **re-decision count** is tracked, because a question arriving at
the same body a third time is proof that something upstream of it is broken.

---

## Industry variations — Domain 3

- **Public sector and government.** Statutory decision rights, published assurance regimes and
  mandatory gates that are genuinely non-negotiable; latency is high by design and the leader's
  lever is almost entirely the paper lead time and the out-of-cycle route, not the cadence.
- **Regulated industries (pharmaceutical, nuclear, aviation, financial services).** Some gates are
  external and unmovable, and the governance design must place the internal decision *before* the
  external submission with enough margin that a failed internal test does not force a submission.
- **Construction and infrastructure.** Multi-party governance is the norm; decision rights follow
  the contract structure, so a governance design inconsistent with the contract loses to the
  contract every time (PFL-AI Domain 11).
- **Technology and product organisations.** Bounded envelopes and continuous funding decisions
  dominate; the characteristic risk is light governance on genuinely irreversible choices —
  architecture, data model, third-party dependency — because they do not present as gates.
- **Healthcare.** Clinical governance runs in parallel with project governance and clinical
  authority is not delegable to a project body; Meridian's design must place clinical sign-off with
  clinical authority and integrate rather than absorb it.
- **Energy and resources.** Stage-gated capital processes with substantial assurance at each gate,
  and the standard pathology is gate proliferation — re-approval ceremonies whose elapsed time
  exceeds 3.3.1's breakeven.

---

## Case study — Domain 3: the four-week month (health, Meridian)

**Situation.** Twelve weeks into the clinic rollout, Meridian's steering committee was widely
described as "the bottleneck". The committee met monthly, was well attended, and had never refused a
request. The programme was six weeks behind, and the delay was attributed in the quarterly report to
"slower than expected clinic readiness".

**What the arithmetic showed.** The project leader computed three numbers before the next governance
review. Governance latency: `M/2 + L = 4/2 + 2 =` **4 weeks** — the committee that everyone called
monthly imposed a full month's expected wait, and no one in the organisation had believed it was more
than a fortnight. Escalation volume: with a **10,000** delegation threshold, **36** of the year's
**60** change requests required the committee, of which about a quarter sat on the critical path,
costing `9 × 4 × 14,280 =` **USD 514,080** a year in delay. Committee capacity: **104** item-slots
against **77** items of demand, of which **26** were standing reports producing no decision — so a
quarter of the scarcest resource in the programme was being spent on status.

**What changed.** Three changes, none of which removed any scrutiny anyone could name. The paper
lead time went from 2 weeks to 1, taking expected latency to **3.0 weeks** — a full week saved from a
change to an administrative deadline. The delegation threshold went from 10,000 to **25,000**, worth
**USD 171,360** a year against a worst case of 81,600 even if every delegated decision were decided
wrongly and destroyed 40 % of its value. And the standing reports moved to pre-reading, releasing 26
slots and taking utilisation from 74.0 % to **49.0 %**, which is what created room for the
contentious item to be reached.

**The outcome, and the part that mattered.** Escalated decisions began arriving in about three weeks
rather than four, and the peak-month deferrals stopped. But the durable change was to the
*conversation*: "the committee is a bottleneck" is an accusation, while "our governance design costs
514,080 a year in delay, and here are three changes worth most of it" is a proposal. The second was
approved in one meeting. The first had been raised, in various forms, for two quarters.

**What the domain teaches here.** Governance is a delivery variable with a computable price. Until
the price is computed, governance complaints are cultural and go nowhere; once computed, they are
engineering, and engineering gets approved. And note which lever was largest per unit of
organisational pain: the paper deadline — free, unglamorous, and twice as effective per week as
meeting more often.

## Case study B — Domain 3: the decision nobody made (financial services)

**Situation.** A payments-platform programme completed build 5 % over budget with no approved change
above 50,000 — a clean change-control record. A routine internal audit then found that the delivered
scope differed materially from the approved baseline in four respects, none of which appeared in any
decision record.

**What had happened.** Each of the four changes had been discussed in a working group, agreed in
substance, and implemented. None had been raised as a change, because each had been individually
below the project leader's **50,000** authority and had therefore, in the team's understanding,
required no decision at all. Cumulatively they were worth approximately 700,000 and one of them
altered a control the second line had assured on the basis of the original design. The programme's
governance was not weak in the ordinary sense: the committee functioned, the gates were held, the
delegation schedule existed. The defect was narrower and more instructive — **the schedule read on
individual value only**, and the decision log recorded decisions *taken by bodies* rather than
decisions *taken*.

**How it resolved.** Three corrections, each mapping to a specific defect. The delegation schedule
acquired **reversibility and externality** dimensions, so a change touching an assured control
required second-line agreement regardless of value. It acquired a **cumulative test**: related
changes aggregating above a threshold within a period required the authority appropriate to the
aggregate, which is the provision that would have caught all four. And the decision log was made the
**register of record for every change to the baseline**, whoever decided it and at whatever value, so
that a decision below a threshold still generated an entry. The audit finding was closed; the
platform went live four weeks late.

**What the domain teaches here.** A decision below a threshold is still a decision and still needs a
record. A delegation schedule that reads only on value cannot see irreversibility, externality or
accumulation — the three ways a small decision becomes a large one — and a decision log that records
only what committees decided will systematically miss the changes that matter most, because those are
exactly the ones that never reached a committee.

---

## Executive perspective — Domain 3

What a programme director cannot delegate in this domain:

- **The latency of your own governance.** Compute `Σ (M/2 + L)` for every escalation path you own,
  price it at your cost of delay, and be able to state it. An unquantified governance design is one
  you are not managing (3.2.3, 3.3.3).
- **The delegation schedule, on three dimensions.** Value, reversibility, externality — and a
  cumulative test. Read on value alone, it will miss exactly the decisions that matter (Case study B).
- **The sponsor's obligations, in writing.** Not the title. The seven testable obligations, agreed
  and evidenced, with turnaround on sponsor decisions monitored like any other lead time (3.2.1).
- **Whether your gates function.** A gate that has never held anything is not working, and its
  elapsed time may already exceed the point at which it destroys value (3.3.1).
- **The decision record's integrity.** Named decision-maker, versioned information reference,
  conditions with owners. This is what makes a defensible decision defensible a year later (3.3.4).
- **Escalation lead time as a leading indicator.** Track how far before impact escalations arrive. A
  shortening trend is the earliest signal that people no longer believe escalation is safe — and it
  appears in no standard report (3.3.3).

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
single-tier path at the portfolio board with a written-resolution procedure of one week.
*Solution.* `2/2 + 1 =` **2.0**; `6/2 + 2 =` **5.0**; `12/2 + 4 =` **10.0**. Total **17.0 weeks**,
costing **USD 161,500** — of which the investment committee alone is 10.0 weeks and **95,000**. A
one-week written-resolution path costs **USD 9,500**, a saving of **USD 152,000** (**94.1 %**). For
comparison, retaining the portfolio board on its ordinary cadence as the sole tier gives 5.0 weeks
and **47,500**, saving 114,000. Common error: adding only the meeting intervals and ignoring the
paper lead times, which understates latency here by 7 of the 17 weeks.

---

## Practitioner's toolkit — Domain 3

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 3.T.1 — Governance design sheet

One page, completed before mobilisation and reviewed at every gate. Rows: each decision body, with
its purpose, members (distinguished from attendees), decision classes it owns, meeting interval `M`,
paper lead time `L`, computed `E[wait]`, capacity (meetings × items) and estimated demand. Below it,
each escalation path with its tiers, the summed latency `Σ (M/2 + L)`, that latency priced at the
project's cost of delay, and the out-of-cycle mechanism. The sheet's purpose is to make latency and
capacity **visible at design time**; a governance design that cannot fill it in has not been designed.

### Toolkit 3.T.2 — Delegation schedule with a cumulative test

Columns: decision class · value threshold · reversibility rating (reversible / costly to reverse /
irreversible) · externality rating (internal / cross-functional / external or public) · authority
required · **cumulative rule** (related decisions aggregating above `X` within period `P` require the
authority appropriate to the aggregate) · information the delegate must hold to decide. The three
dimensions prevent the value-only failure of Case study B; the cumulative rule prevents its specific
mechanism; and the final column is what distinguishes delegation from abdication.

### Toolkit 3.T.3 — Decision record entry and its integrity checks

Per-decision fields: reference · date · decision-maker (**named person and role**) · decision (one
reading only) · options considered and why this one · information relied on **with versions** ·
conditions with owners and dates · review date if provisional. Monthly integrity checks, each a
count: entries missing a named decision-maker; entries missing a versioned reference; conditions past
their date; decision classes with zero or multiple Accountable roles in the RACI (the single-A test);
and **re-decisions** — the same question arriving at the same body more than twice. Every one of these
is a number, and a governance function that reports them monthly will find its defects before an
auditor does.

---

## Exam preparation — Domain 3

**What is assessed.** Governance purpose and its separation from management and assurance; structural
weaknesses by organisational form; governing iterative and hybrid delivery; the sponsor's obligations
and failure modes; steering-committee design faults; **the governance latency formula and its
application**; delegation thresholds and their economics; gate purpose, criteria and value; the three
assurance lines and their failure modes; escalation design with stated latency; and the decision
record with its integrity checks.

**The calculations to be able to do under time pressure.** `E[wait] = M/2 + L` for a single body and
`Σ (M/2 + L)` for a multi-tier path, priced at a cost of delay. Escalation volume and delay cost at
alternative thresholds, and the breakeven value-destruction rate on a delegated band. Gate net value,
breakeven elapsed time and breakeven detection probability. Committee capacity and utilisation. RACI
single-A defect rate.

**The traps.** Taking expected wait as half the meeting interval and forgetting the paper lead time
(Exercise 3.1) · adding meeting intervals without paper lead times in a multi-tier path
(Exercise 3.4) · treating governance artefacts as governance (3.1.1) · omitting elapsed-time cost
from gate value, which makes every gate look worthwhile (Exercise 3.3) · comparing a delegation
saving with the total value of the delegated band rather than a plausible loss on it (Exercise 3.2) ·
assuming a committee's authority is its highest member's rather than its binding minimum (3.2.2) ·
treating a favourable assurance opinion as a transfer of accountability (3.3.2) · recording only
committee decisions and missing the ones taken below a threshold (Case study B) · reading a delegation
schedule on value alone.

**How the domain connects.** Domain 1 supplies the accountability principle governance must respect
and the cost of delay every calculation here is priced at. Domain 2 supplies the business case and
kill criteria that gates re-test. Domain 4 integrates the governance of parts into a whole. Domain 6
consumes governance latency as schedule input — a decision path of 15.5 weeks is a 15.5-week
predecessor, whatever the plan says. Domain 7's change control is the delegation schedule in
operation. Domain 8's risk escalation uses the escalation classes designed here. And PFL-AI Domain 11
handles the risk-allocation face of the multi-party governance problem.

---

## Summary — Domain 3

Governance is the decision rights, accountabilities and information flows through which an
organisation directs a project — and it is not management, not assurance, and emphatically not the
reporting apparatus that is usually offered in its place. A governance design must produce
decidability, timeliness, legitimacy and traceability, and each of those is testable.

The domain's contribution is to make governance **computable**. The expected wait for a committee
decision is `M/2 + L`: half the meeting interval plus the whole paper lead time. That single formula
prices a governance design at design time, shows that the administrative deadline is twice the lever
meeting frequency is, and adds across tiers to reveal what an escalation path actually costs —
15.5 weeks and **USD 221,340** for Meridian's three-tier path, of which the quarterly committee alone
is 61 %. The same arithmetic prices a delegation threshold: Meridian's 10,000 threshold cost
**USD 514,080** a year, and raising it to 25,000 saves **USD 171,360** against a worst case of 81,600
even if every delegated decision were decided wrongly — a breakeven value destruction of **84 %** per
decision, which no plausible delegate approaches. And it prices a gate: Meridian's design gate is
worth **USD 56,520**, stops paying beyond **9.96 weeks** of elapsed time, and requires a detection
probability above **55.85 %** to be worth holding at all.

The sponsor's role is a set of testable obligations, not a title; steering committees fail in five
predictable ways, four of them designed in; gates buy optionality against irreversibility and belong
where irreversibility steps up; assurance has three lines whose worst failure is capture; escalation
is a timed pathway with an out-of-cycle route, not an instruction to tell someone senior; and a
decision that is not recorded, with a named decision-maker and a versioned information reference, has
not been made — it has been remembered. Case study B's four changes were each individually within
authority and cumulatively worth 700,000, which is the whole argument for a delegation schedule that
reads on reversibility and externality and aggregates.

The through-line: **governance has a price, the price is computable, and until it is computed
governance complaints are cultural and go nowhere.** Compute it, and they become engineering.
