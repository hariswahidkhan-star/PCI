# Domain 4 — Integration and Delivery Architecture

## Why this domain exists

Domain 1 established what a project leader is answerable for. Domain 2 established how work is
chosen and whether the promise was honest. Domain 3 established who may decide, how quickly, and at
what price. Each produced something real, and none of them produced a **project**.

That is what this domain does, and the word that names the gap is **integration**. A project is not a
collection of well-run parts; it is a set of parts that must fit, arrive in an order, share
interfaces, and change together. Every one of those relationships is a place where a project fails
without any single part having failed — which is why integration failure is the hardest kind to
attribute and the most expensive to repair. The specific and recurring form it takes: **the parts
are managed and the joins are not.** Scope is controlled, the schedule is controlled, cost is
controlled, and nobody owns the fact that a change to one moves all three.

The domain's second claim is that integration is **architectural** — a design choice with a price,
made once and paid for continuously. Interfaces do not multiply linearly with components; they
multiply combinatorially, and KA 4.2 computes the difference and what buying an integration layer
saves. Baselines are not three documents but one three-dimensional statement, and KA 4.3 shows what
the arithmetic of the hundred-per-cent rule catches that reading the documents does not. Changes do
not cost what a change form says they cost, and KA 4.4 shows that the quoted figure is typically the
smallest component of the true one — which is why a delegation threshold applied to a quoted cost is
not a control at all.

The through-line: **integration is where the parts become a project, and the joins are where the
money is.**

**Learning objectives.** After this domain a candidate can: state what a charter must contain to
confer authority, and distinguish a charter from a plan; assemble a coherent plan of plans, **audit a
plan set against itself and price the exposure the audit finds**; derive a rolling-wave horizon from
the longest commitment lead time in the plan rather than asserting it; tailor defensibly; build a WBS
that satisfies the hundred-per-cent rule and detect an unallocated or omitted element arithmetically;
distinguish work from product breakdown, **cross-match the two and separate a genuine orphan from a
matrix artefact**; **compute the interface count of a delivery architecture and price what an
integration layer buys, including its breakeven cost and its marginal advantage as the programme
grows**; integrate scope, schedule and cost into one baseline and **compute what a stale time-phased
baseline does to each earned-value index and which indices it leaves exactly correct**; apply
configuration management to a baseline, audit its integrity, and **derive the item-level audit
tolerance from an interface-level target through the pairwise exponent**; maintain a baseline through
change without losing traceability to the original, and **perform the original-to-current
reconciliation and decompose its residual gross rather than net**; design an integrated
change-control flow and **price a screening misclassification in both of its directions**;
**assess a change's true cost including schedule, rework, interface re-verification and regression,
and explain why a threshold set on quoted direct cost fails**; detect and quantify baseline drift by
accumulation and set a cumulative test that would actually have caught it; **price an untraced
rejection**; and govern AI-assisted impact assessment without letting it decide.

**The master programme, and the handover.** Meridian Care Records continues from Domains 1–3: the
clinical-records rollout to **40 clinics**, approved cost **USD 2,400,000** (Domain 2's business
case), benefits **USD 685,440** a year at 70 % adoption, cost of delay **USD 14,280 per week**
(Domain 1), and the governance design priced in Domain 3. This domain architects it. Part Two then
picks up **Project Auriga** — the 25-week control-systems upgrade — to work planning, cost and risk
at the scale of a single project, because the two scales need different illustrations and a book that
uses only one has taught only half the discipline.

**Reference points.** The international document most often named in connection with integration and
the management of a project as a whole is **ISO 21502**, which addresses guidance on project
management. It is voluntary guidance describing practice — not legislation, and not a certifiable
requirement — and it obliges nobody of itself unless an organisation, a contract or a regulator adopts
it. It is named here and not reproduced: no clause, table or checklist from it appears in this volume,
the treatment below is this book's own, and a reader who wants the document should obtain the current
edition from its publisher. Naming it implies no endorsement in either direction.

---

## Knowledge Area 4.1 — Charter and management plans

*Topics: 4.1.1 the charter · 4.1.2 the plan of plans · 4.1.3 tailoring.*

### 4.1.1 The charter

**Definition.** The charter is the document by which the organisation **authorises the project and
confers authority on its leader**. It is not a plan, not a business case, and not a summary of
either: the business case argues that the work is worth doing (Domain 2), the plan states how it will
be done, and the charter states that it *is* to be done, by whom, within what bounds.

**What a charter must contain to do its job.** Each item earns its place by being something a leader
cannot proceed without, and the test of a charter is whether its absence would be noticed:

| Element | Why it must be there |
|---|---|
| Purpose and objectives | The measurable statement of what success is (Domain 2's benefits, not activity). |
| Named sponsor | The accountable owner of the outcome (Domain 3, KA 3.2.1) — a role, and a person. |
| Named project leader and their authority | The **bounds** within which they decide without asking: spend, scope, resource, commitment (Domain 3's delegation schedule). |
| Scope boundary, including exclusions | Exclusions are the half most often omitted, and the half that prevents the argument. |
| Key deliverables and success criteria | What will be handed over, and how acceptance is judged. |
| Milestones and constraints | The dates that are genuinely fixed, distinguished from those that are planned. |
| Budget authority | The approved sum and the authority to commit against it. |
| High-level risks and assumptions | The material ones, carried forward into the risk register (Domain 8). |
| Governance and decision rights | Bodies, thresholds and escalation, by reference to the design of Domain 3. |
| Approval | Signature of the authority entitled to confer it. |

**The two charter failures.** *The charter that authorises nothing*: a document of aspiration with no
stated authority, so every decision is an ask, and the leader discovers their authority
incident by incident — which is the functional-organisation weakness of Domain 3, KA 3.1.2, arriving
through the front door. *The charter that is a plan*: dozens of pages of approach and schedule,
signed once, immediately stale, and thereafter neither read nor updated; its length conceals the
absence of the one paragraph that mattered.

**The test.** A usable charter answers, on one or two pages: *what is this for, who owns the outcome,
who leads it, what may they decide alone, what is in and out, and what is fixed?* A leader who cannot
answer the fourth of those from the charter does not have a charter; they have an announcement.

### 4.1.2 The plan of plans

**The concept.** The project management plan is not a single narrative but an **integrated set** of
subsidiary plans — scope, schedule, cost, quality, resource, communication, risk, procurement,
stakeholder, change and configuration — plus the baselines they produce. Its integration is the whole
point: the subsidiary plans are individually easy and collectively contradictory unless someone owns
the consistency.

**The consistency checks that make it a plan rather than a folder**, and each is a specific,
answerable question:

- Does the **schedule** contain the activities the **scope** requires — every deliverable traceable
  to work, and every work package to a deliverable?
- Does the **cost** baseline sum to the resourced schedule, at the resource rates the **resource**
  plan assumes (Domain 7's rate arithmetic)?
- Do the **quality** plan's verification activities appear in the schedule with duration and
  resource — or are they assumed to happen in gaps?
- Do the **risk** responses have owners, dates, and budget in the cost baseline (Domain 8's
  contingency)?
- Do the **procurement** lead times appear as schedule activities rather than as assumptions?
- Does the **change** plan's authority match the governance design's delegation schedule, on the same
  thresholds (Domain 3) — and does it read on assessed impact rather than quoted cost (KA 4.4)?
- Does the **communication** plan's reporting cadence fit the governance bodies' paper lead times
  (Domain 3's `L`), so that reports exist when papers close?

That last check is the kind this domain exists to surface: it is invisible in any single plan and
obvious the moment the plans are read against each other. A programme whose monthly report is
produced three days *after* the steering committee's papers close will report last month's position
every month, for its entire life, and will describe the problem as a reporting delay.

Those seven questions are usually treated as a review formality. They are an arithmetic exercise with
a price attached.

**Worked example 4.1.2 — auditing Meridian's plan set against itself, and pricing what it finds.**

1. **Setup.** Before baseline approval, Meridian's **eleven** subsidiary plans are read against one
   another using the checks above — two reviewers over three days, **6 person-days** at a blended
   **USD 880** per person-day (the USD 110 an hour of Domain 14, KA 14.2.1, over an eight-hour day).
   The plan set covers **172** work packages, **24** quality-verification activities, **19** risk
   responses with named owners and **7** procured items. Cost of delay **USD 14,280 per week**
   (Domain 1).
2. **Formula.** Run each check; convert each finding into money — unfunded work at the person-day
   rate or at its own stated cost, unscheduled elapsed time at the cost of delay, counting only what
   lands on the critical path. Total, then compare with the cost of the audit.
3. **Substitution.** Check by check. *Scope against schedule:* **4** deliverables with no scheduled
   activity — flagged, deliberately **not** priced. *Cost against the resourced schedule:* the cost
   baseline sums to the resourced schedule at the resource plan's rates — **this check passes**.
   *Quality:* of 24 verification activities, **9** appear nowhere in the schedule, carrying **46
   person-days** of resource → `46 × 880`; **4** of those nine sit on the critical path with **12**
   working days between them → `(12/5) × 14,280`. *Risk:* of 19 responses, **6** have no line in the
   cost baseline, together planned at **118,000**. *Procurement:* **3** items have lead times stated
   only in the procurement plan; the longest is **14 weeks** against **6** allowed in the schedule →
   `(14 − 6) × 14,280`. *Change:* the change plan's threshold reads on quoted direct cost — the
   defect KA 4.4.2 prices. *Communication:* the report is produced after papers close; the staleness
   arithmetic belongs to Domain 14, KA 14.2.1 and is not repeated here.
4. **Result.** Unfunded verification labour **USD 40,480**; unfunded risk responses **USD 118,000**;
   procurement lead-time exposure `8 weeks =` **USD 114,240**; critical-path verification exposure
   `2.4 weeks =` **USD 34,272**. Quantified total **USD 306,992** — **12.79 %** of the approved
   2,400,000 — found by an audit costing **USD 5,280**, a ratio of **58.14** to one. Three classes of
   finding are recorded unpriced — the four unscheduled deliverables, the threshold basis and the
   reporting cadence — and one check passes clean.
5. **Interpretation.** The first thing to be honest about is what the 306,992 is not. It is **not a
   saving**. Most of it is money that will be spent whatever anyone decides — the verification
   activities are needed, the risk responses were chosen deliberately, the lead times are what the
   market quotes. What the audit changes is whether that money is spent inside an authorised baseline
   or outside it as an unexplained overrun, and an assurance function that reports the figure as a
   saving will be correctly disbelieved and will lose the argument it was right about. The defensible
   sentence is: *306,992 of committed but unauthorised cost and schedule moved from invisible to
   visible, for 5,280.*

   Set that against WE 4.3.3. Meridian's baseline drifts **291,176** over a year, through 34
   individually authorised approvals, and everybody involved works hard for it. The plan-set audit
   finds **more** — 306,992, a ratio of **1.0543** — in three days, before any of the work has
   started, without a single change request being raised. **The cheapest change control is the one
   applied to the plan set before it is baselined**, because at that point a correction is a
   re-estimate rather than a change: no board, no assessment cost, no traceability obligation, no
   supplier conversation.

   Now notice *which* check passes. Cost against the resourced schedule is the one organisations
   habitually test, and it is the one where both sides are owned by the same function and read from
   the same tool. Every failing check spans two owners: quality and planning, risk and cost,
   procurement and planning, change and governance. That is the general rule this KA exists to state
   — **plan-set defects live on ownership boundaries**, and a check that either party could have run
   alone has almost certainly already been run. It follows that the audit should be sequenced by
   boundary rather than by plan, and that the reviewer must have standing with both owners, which is
   why this work fails when it is delegated to whoever has capacity.

   The sensitivity is concentrated in one component. The 114,240 of procurement exposure assumes the
   eight-week lead-time gap lands on work that gates the rollout. If the affected item has eight
   weeks of float the component is **zero** and the total falls to **USD 192,752** — still **8.03 %**
   of the baseline, so the conclusion survives, but the headline drops by **37.21 %**. Every
   schedule-priced finding must therefore carry its float position (KA 4.4.2's third assessment
   rule), and an audit that prices delay without stating float has quoted an upper bound as a point
   estimate.

   Finally, a caution about the four unpriced deliverables, because the instinct is to put a
   placeholder against them so that the total looks complete. Resist it. Those four are the findings
   a reviewer should care about most: an unpriced finding is one whose magnitude is unknown, and
   unknown magnitudes are where residual risk actually sits. A total that silently includes invented
   numbers for them is worse than a total that stops short and says why.

**Progressive elaboration and rolling wave.** Detail is added as it becomes knowable rather than
invented early: the near horizon is planned to work-package level, the far horizon to a coarser
level, and the boundary moves forward at a stated cadence. Two disciplines make this honest rather
than an excuse. The **horizon must be stated** — "detailed to work-package level for 12 weeks,
planning-package level beyond" — so that nobody mistakes a coarse plan for an absent one. And the
**far-horizon estimate must carry its uncertainty explicitly**, because a rolling-wave plan whose
outer years are stated as point numbers has hidden its uncertainty in exactly the place it is largest
(Domain 8's ranges).

**Setting the horizon rather than asserting it.** The horizon itself is usually chosen for
administrative tidiness — a quarter, a phase, twelve weeks — and it has a derivable lower bound:
**the horizon must be at least as long as the longest commitment lead time falling inside it**,
because a plan that does not yet exist at work-package level cannot support a commitment that must be
placed before it does. Meridian illustrates the failure exactly. Its stated horizon is **12 weeks**;
the longest procurement lead time WE 4.1.2 found is **14 weeks**. The horizon is **two weeks shorter
than its own supply chain**, and no amount of diligence inside the wave can close that gap, because
the order must be placed before the work package that specifies it has been written. Two responses
are legitimate: lengthen the horizon to at least 14 weeks on the affected path, or plan that one
procurement to work-package level ahead of the wave and label it a **partial horizon**. What is not
legitimate is to leave a 12-week horizon and a 14-week lead time in the same plan set and describe
the consequence as a supplier problem. The test generalises to every commitment class in the plan —
procurement, recruitment, regulatory submission, facility access, decommissioning permission —
compare each lead time with the horizon, and set the horizon requirement from the longest.

### 4.1.3 Tailoring

**The principle.** Every method requires tailoring, and tailoring is a **decision with a rationale
and an owner**, not the quiet omission of inconvenient parts. The distinction is procedural: a
tailoring decision is recorded, states what was removed or added and why, names who approved it, and
is reviewable. An omission is discovered later by someone else.

**What drives it.** Project size and value; novelty and technical uncertainty; regulatory and
contractual obligation; delivery approach (sequential, iterative, hybrid); organisational maturity;
and the receiving organisation's capacity to absorb process. A small, novel, publicly visible project
may need *more* assurance and *less* documentation than a large, familiar, internal one, and a
tailoring regime that cannot express that combination is not tailoring but scaling.

**The limits.** Three things are not tailorable and should be stated as such, because they are what
tailoring is most often used to remove: the **decision record** (Domain 3, KA 3.3.4) — nothing about
project size makes an unrecorded decision acceptable; **the traceability of a baseline change to an
authority** (KA 4.3.3); and any **legal, regulatory or contractual** obligation, which does not scale
with convenience. Everything else is a judgement, and every judgement is recorded.

### AI in this KA

**Where it earns its place.** Reading a plan set against itself and listing the inconsistencies above
— deliverables with no work packages, quality activities absent from the schedule, risk responses
without budget, reporting cadences that miss governance paper deadlines. This is precisely the work
integration requires, humans do it badly because it spans documents, and the answers are checkable.
Drafting a charter or a subsidiary plan from an approved template plus stated inputs, for human
completion. Proposing a tailoring set from project characteristics, as a starting position to be
argued with.

**Where it must not go.** Approving a tailoring decision, which requires an accountable authority.
Producing a charter's authority statement, which is a conferral of power and must be authored by the
conferring body. And no AI-produced plan should enter a baseline unreviewed: a plan is a commitment,
and Domain 1's principle requires a person to stand behind it.

**Verification, concretely.** Every flagged inconsistency is confirmed against the source documents
before it is reported; every AI-drafted plan section is reviewed by the accountable owner of that
plan; and the tailoring record states that the proposal was AI-assisted, which is an honesty
obligation rather than a disclaimer.

### Key terms — KA 4.1

| Term | Meaning |
|---|---|
| **Charter** | The document authorising the project and conferring bounded authority on its leader. |
| **Authority bounds** | The spend, scope, resource and commitment limits within which the leader decides alone. |
| **Plan of plans** | The integrated set of subsidiary plans and the baselines they produce. |
| **Consistency check** | A specific question testing whether two subsidiary plans agree. |
| **Plan-set consistency audit** | Reading the subsidiary plans against one another and pricing the findings, before baseline approval (WE 4.1.2). |
| **Progressive elaboration** | Adding detail as it becomes knowable, at a stated horizon and cadence. |
| **Planning package** | A far-horizon element planned above work-package level, with its uncertainty stated. |
| **Commitment lead-time rule** | The rolling-wave horizon must be at least as long as the longest commitment lead time inside it. |
| **Partial horizon** | One element planned to work-package level ahead of the wave because its lead time demands it, labelled as such. |
| **Tailoring** | A recorded, owned decision to adapt method — never a silent omission. |

### Sample MCQs — KA 4.1

**MCQ 4.1-A `[4.1.1 · Comprehension]`** The element of a charter whose absence most directly
disables the project leader is:
- A. the list of high-level risks
- B. the statement of the leader's authority bounds ✅
- C. the milestone schedule
- D. the communication approach

*Rationale:* Without stated bounds every decision becomes an ask and authority is discovered
incident by incident (4.1.1). The others are important and none of them confer power.

**MCQ 4.1-B `[4.1.2 · Analysis]`** A programme's monthly report is produced three days after its
steering committee's papers close. The consequence is that:
- A. the report is slightly late
- B. the committee will systematically consider last month's position, every month ✅
- C. the reporting cadence must be made weekly
- D. the paper lead time must be abolished

*Rationale:* The defect is a plan-consistency failure between the communication plan and the
governance design's paper lead time (4.1.2), and it recurs for the project's whole life until the
cadences are aligned.

**MCQ 4.1-C `[4.1.3 · Evaluation]`** Which is *not* legitimately tailorable?
- A. the number of subsidiary plans maintained
- B. the depth of the schedule beyond the planning horizon
- C. the traceability of a baseline change to an approving authority ✅
- D. the frequency of progress reporting

*Rationale:* Traceability of baseline change to authority, the decision record, and legal or
contractual obligations are outside tailoring (4.1.3).

**MCQ 4.1-D `[4.1.2 · Application]`** In WE 4.1.2 the plan-set audit finds unfunded verification
labour of 40,480, unfunded risk responses of 118,000, eight weeks of procurement lead-time exposure
and 2.4 weeks of critical-path verification exposure, at a cost of delay of 14,280 per week. The
quantified total is:
- A. USD 158,480
- B. USD 272,720
- C. USD 306,992 ✅
- D. USD 392,672

*Rationale:* `40,480 + 118,000 + (8 × 14,280) + (2.4 × 14,280) = 306,992` (4.1.2). A totals only the
two money classes; B omits the critical-path verification exposure; D prices the whole 14-week
procurement lead time rather than the 8-week gap against the schedule.

**MCQ 4.1-E `[4.1.2 · Evaluation]`** A rolling-wave plan states a 12-week detailed horizon; the
longest procurement lead time in the plan set is 14 weeks. The correct conclusion is that:
- A. the procurement should be expedited to fit the horizon
- B. the horizon cannot support its own commitments and must be lengthened on that path, or the item
  planned ahead of the wave as a stated partial horizon ✅
- C. the horizon is a planning convention and the lead time is a supplier matter
- D. the plan should carry a risk for late procurement

*Rationale:* The order has to be placed before the work package specifying it exists, so no diligence
inside the wave closes the gap (4.1.2). D converts a structural defect into a risk, which is the
commonest way this finding is disposed of without being fixed.

### Self-check — KA 4.1

1. *What distinguishes a charter from a plan?* — The charter authorises and bounds; the plan states
   how. The charter's irreplaceable content is the authority statement.
2. *Name two plan-consistency checks that only appear when plans are read against each other.* —
   Quality verification activities missing from the schedule; reporting cadence missing the
   governance paper deadline.
3. *Why do plan-set defects cluster on ownership boundaries?* — Because a check both parties could run
   alone has already been run; the failing checks are the ones spanning two owners (WE 4.1.2).
4. *What sets the lower bound on a rolling-wave horizon?* — The longest commitment lead time inside it,
   since a commitment cannot be placed from a plan that does not yet exist at work-package level.
5. *What makes a tailoring decision legitimate?* — It is recorded, states what changed and why, names
   its approver, and is reviewable.

---

## Knowledge Area 4.2 — Breakdown structures

*Topics: 4.2.1 WBS · 4.2.2 product and deliverable breakdown · 4.2.3 interfaces.*

### 4.2.1 The work breakdown structure

**Definition.** A WBS is a **deliverable-oriented hierarchical decomposition** of the total scope of
work. Two words carry the weight. *Deliverable-oriented*: it decomposes what will be produced, not
who will produce it or when — an organisational chart and a schedule are different artefacts, and a
WBS built by team or by phase loses the property that makes it useful. *Total*: the decomposition
covers the whole scope and nothing outside it.

**The hundred-per-cent rule.** The children of any element sum to exactly that element — no more, no
less. More means duplication or scope not authorised; less means work that will be done and is not
budgeted. The rule is not a style guideline; it is an arithmetic invariant, and because it is
arithmetic it can be **checked**, which is the point of the next worked example.

**Work packages.** The lowest level, and the unit that is estimated, scheduled, assigned and
measured. A work package should have a single accountable owner, a deliverable or verifiable
outcome, an estimate the owner endorses, and a duration that permits progress to be assessed
meaningfully — sized so that the answer to "is it done?" is not routinely "about half". Above the
work packages, **control accounts** are the level at which performance is measured and reported,
which is where Domain 7's earned value attaches.

**Worked example 4.2.1 — auditing Meridian's WBS against the hundred-per-cent rule.**

1. **Setup.** Meridian's approved cost baseline is **USD 2,400,000** (Domain 2). Its five level-2
   WBS elements are estimated at: records application build **780,000**; integration layer and
   interfaces **536,000** (the figure KA 4.2.3 derives); data migration **310,000**; clinic rollout
   across 40 clinics **520,000**; programme management and assurance **186,000**.
2. **Formula.** Hundred-per-cent test: Σ children − parent. Then re-test with any omitted element
   restored.
3. **Substitution.** `780,000 + 536,000 + 310,000 + 520,000 + 186,000 = 2,332,000`; against
   `2,400,000`.
4. **Result.** The children sum to **2,332,000**, leaving **68,000** (**2.83 %**) of the parent
   unallocated. The review then identifies the omitted element: **clinician training and enabling
   change**, estimated at **214,000** — the same column Domain 2's benefits map omitted. Restoring it
   gives an honest baseline of **2,546,000**, which is **146,000** — **6.1 %** — *above* the approved
   figure.
5. **Interpretation.** Both halves of that result matter and they are usually confused. The 68,000 of
   apparently spare budget is not spare; it is the residue of an incomplete decomposition, and in
   practice it will be consumed early by whatever arrives first, after which the omission becomes
   visible with no budget left to absorb it. And the omission is not random: it is the **enabling
   change** — the work that converts an output into an outcome — which Domain 2 identified as the
   column most benefits maps leave out, and which reappears here as the element most WBSs leave out,
   for the same reason. It belongs to somebody else, so nobody decomposes it. Note finally what the
   arithmetic does *not* say: at 2,546,000 the programme's NPV falls from Domain 2's **+1,332,898** to
   **+1,186,898**, a reduction of **11.0 %** — still comfortably positive. The omission bought
   nothing. It was not a decision to descope training in order to make the case work; it was a failure
   to look, and the honest baseline would have been approved.

### 4.2.2 Product and deliverable breakdown

**The distinction.** A **product breakdown structure** decomposes the *thing being delivered* into
its constituent products and components; a WBS decomposes the *work*. They answer different questions
and both are useful — but only one of them is a good starting point.

**Why to start with the product.** Building the product breakdown first and deriving the work from it
prevents the commonest scope defect: **work that produces nothing**, and its mirror, **a product with
no work**. If every product traces to work and every work package to a product, the two structures
verify each other, and the verification is a matter of matching lists rather than judgement.

**Where each belongs.** The product breakdown drives configuration management (KA 4.3.2) — because
configuration items are products, not activities — and acceptance, since acceptance is of products.
The WBS drives estimating, scheduling and performance measurement. Confusing them produces two
recognisable pathologies: configuration control applied to activities, which cannot work because
activities do not have versions; and estimating against products, which omits the work that has no
product (integration, testing, migration, training) and is therefore how the elements of the previous
worked example go missing.

**Worked example 4.2.2 — cross-matching Meridian's product breakdown against its WBS.**

1. **Setup.** Meridian's product breakdown identifies **148** products; its WBS contains **172** work
   packages. A traceability matrix relates the two — `148 × 172 =` **25,456** cells. The test is that
   every product traces to at least one work package and every work package to at least one product.
2. **Formula.** Product coverage = products with at least one work package ÷ products. Work-package
   coverage = work packages with at least one product ÷ work packages. Then **classify every
   exception**, because the raw counts are not the finding.
3. **Substitution.** Raw exceptions: **12** products with no work package and **9** work packages
   with no product — **21** in all. Classification, item by item, by the owner of each item. Of the
   12: **5** are the clinician-training and enabling-change product set — WE 4.2.1's omitted
   **214,000** — and **7** are produced inside work packages named for their parent product, so they
   are matrix granularity rather than omission. Of the 9: **7** are legitimately product-free
   (integration testing, data migration, training delivery, commissioning, interface verification) and
   **2** are genuine orphans, budgeted at **26,000**, producing nothing anyone has asked for.
4. **Result.** Product coverage `136/148 =` **91.89 %**; work-package coverage `163/172 =`
   **94.77 %**. Of 21 raw exceptions, **14** are legitimate and **7** are real findings — a
   false-positive rate of **66.67 %**. The real findings are worth **214,000** of omitted work and
   **26,000** of unnecessary work: **USD 240,000**, exactly **10.00 %** of the approved baseline.
5. **Interpretation.** The classification, not the matrix, is the professional work. Two thirds of the
   raw exceptions are artefacts, so a leader who takes raw counts to a steering committee will be
   corrected in the room on the seven granularity artefacts and will lose the five that mattered along
   with them. State the rule that way round: **a cross-match produces a candidate list, and the
   finding is the residue after each exception has been ruled on by the owner of the item.** Twenty-one
   rulings is an afternoon; 25,456 cells is not human work at all, which is exactly the division of
   labour the AI note below describes.

   The two real classes are not symmetric, and only one of them is visible later. **214,000 of
   products with no work is an omission**: it arrives as an overrun, at the end, in the
   enabling-change column that Domain 2's benefits map dropped and that WE 4.2.1's WBS dropped again —
   the third time the same column has gone missing in this programme, which is what a systematic
   defect looks like as opposed to an accident. **26,000 of work with no product is waste**: it will
   be executed diligently, reported as complete, and never questioned, because a work package that
   produces nothing still reports 100 %. Earned value will confirm it. That is the uncomfortable
   corollary of Domain 7's machinery — performance measurement measures conformance to the plan, so
   work that should not be in the plan is measured as success.

   Note what removing the orphans does to WE 4.2.1's arithmetic, because the direction surprises
   people. Allocated cost falls from 2,332,000 to **2,306,000**, so the unallocated residue **rises**
   from 68,000 to **94,000** — **3.92 %** of the parent. Cleaning a WBS usually widens the
   hundred-per-cent gap rather than closing it, because part of the gap was being filled by work that
   should not have been there. A leader who expects the 214,000 omission and the 26,000 of waste to
   offset each other has misread both: one is outside the baseline and one is inside it, and they land
   on different sides of the sum.

   A caution on the legitimate no-product class, which is where this check is most often
   misunderstood. It is the same class the paragraph above identified as the one a product-derived WBS
   omits by construction — so it has to be *added* rather than found, and the honest sequence is
   therefore: product breakdown first, for completeness of *what is delivered*; then the deliberate
   addition of the product-free work, as a decision; then the cross-match. On that sequence the seven
   legitimate exceptions are the **register of that addition**, not a defect list — and if that class
   comes back empty, the WBS is a product breakdown with different headings and the integration work
   has not been planned at all. An empty legitimate class is a worse finding than a large one.

   What a reviewer should test is not the coverage percentages but whether each of the 21 exceptions
   carries a recorded ruling with a named ruler. An unruled exception list is the commonest outcome of
   this check and is indistinguishable, six months later, from never having run it.

### 4.2.3 Interfaces — and why they are the expensive part

**The definition and the problem.** An interface is a defined relationship across a boundary —
between components, teams, contracts, organisations or phases — at which something must be agreed and
verified. Interfaces are where integration effort actually lives, and they are systematically
under-planned for one arithmetic reason: **components grow linearly and interfaces grow
combinatorially.** A programme that has doubled its component count has quadrupled its interface
count, and the plan that was written for the first count is still in use.

For `n` components each of which may connect to any other, the number of possible pairwise interfaces
is:

```
Mesh interfaces      = n(n − 1)/2          — every component to every other
Layered interfaces   = n                    — every component to one integration layer
```

**Worked example 4.2.3 — what an integration layer buys Meridian.**

1. **Setup.** Meridian must integrate **12** components: records core, patient master index,
   appointments, prescribing, laboratory results, imaging, billing, the national reporting gateway,
   identity and access management, the clinical document store, analytics, and the legacy records
   system during migration. Specifying, building, testing and documenting one interface costs
   **USD 18,000**. An integration layer would cost **USD 320,000** to build.
2. **Formula.** Mesh count `n(n−1)/2`; layered count `n`. Cost = count × unit cost (+ layer build).
3. **Substitution.** Mesh `12 × 11/2 = 66`; layered `12`. Mesh cost `66 × 18,000`; layered
   `12 × 18,000 + 320,000`.
4. **Result.** **66** point-to-point interfaces costing **USD 1,188,000**, against **12** interfaces
   plus the layer costing **USD 536,000** — the layer saves **USD 652,000**, or **54.9 %**. It
   remains worth building while its cost is below `1,188,000 − 216,000 =` **USD 972,000**.
5. **Interpretation.** The saving is large and it is not the most important part of the result. The
   decisive number is **marginal**: adding a thirteenth component costs `12 × 18,000 =`
   **USD 216,000** on a mesh architecture and **USD 18,000** on a layered one — a factor of **12**,
   which grows with every component added. Since programmes acquire components (a new department, an
   acquired system, a regulatory feed), the architecture is a bet on the *future* component count, and
   a mesh is a bet that the count will not grow. Three professional cautions, however, because this
   arithmetic is easy to over-apply. Real architectures are **partial meshes** — not every pair
   genuinely needs to connect, and the honest count is of *required* interfaces, which is why the
   interface register (Toolkit 4.T.2) is built from need rather than from the formula. The layer
   introduces a **single point of failure and a throughput constraint** that the point-to-point
   design does not have, and those are real costs not captured here. And the 18,000 unit cost is an
   average over interfaces of very different difficulty: an internal API and a national reporting
   gateway are not the same object, and a leader quoting one figure for both should say so.

> **Fig 4.2.1 — Interfaces grow combinatorially; components do not.** Line chart, x-axis number of
> components 2–20, y-axis interface count, two lines: mesh `n(n−1)/2` rising steeply to **190** at
> 20 components, and layered `n` rising linearly to **20**. Meridian's 12 components marked on both
> lines at **66** and **12**, with the gap annotated **"54 interfaces — USD 972,000 of avoidable
> work"**. A marginal callout at 12→13: **"+12 interfaces (216,000) on a mesh, +1 (18,000)
> layered"**. Source: PCI original. Alt text: a steeply rising quadratic curve far above a shallow
> straight line, showing interface count exploding with component count under a mesh architecture.

**Managing interfaces once counted.** The count is the beginning. Each required interface needs an
**interface agreement**: the two parties, the thing exchanged, its format and content, the direction,
the timing, the error and exception behaviour, who verifies it and when, and the version it is agreed
at. Two disciplines matter more than the document. An interface has **exactly one owner on each
side**, named — Domain 3's decidability test applied to a boundary, and the reason unowned interfaces
are resolved late and by whoever notices. And interface verification is a **scheduled activity with
duration and resource** (KA 4.1.2's consistency check), because the alternative is that verification
happens at integration testing, which is where interface defects are most expensive to find and where
they consume the schedule float that Domain 6 shows was never there.

### AI in this KA

**Where it earns its place.** Checking the hundred-per-cent rule across a large WBS and reporting
every element whose children do not sum — mechanical, exhaustive, and exactly the check humans skip.
Cross-matching a product breakdown against a WBS and listing products with no work and work with no
product — 25,456 matrix cells in Meridian's case, which is not human work at any level of diligence.
Extracting an interface register from architecture and design documents, which is tedious and
error-prone by hand. Flagging interfaces with no named owner on one or both sides, or no verification
activity in the schedule.

**Where it must not go.** Ruling on a cross-match exception. WE 4.2.2's arithmetic exists precisely
because two thirds of the raw exceptions were legitimate, and telling a granularity artefact from an
omission requires knowing what the work actually produces — the ruling belongs to the owner of the
item and must be recorded with their name. Deciding the architecture. The mesh-versus-layer choice
depends on availability requirements, throughput, organisational boundaries, contractual structure
and the expected growth in components — several of which are judgements about the future that no model has
grounds for, and all of which belong to an accountable architect and the governance body.

**Verification, concretely.** Reproduce the interface arithmetic by hand — it is one formula — and
state the assumed unit cost with its basis. Confirm each AI-derived interface against the source
design before it enters the register. And when a model reports a WBS as compliant, spot-check at
least the largest elements, because a hundred-per-cent check is only as good as the estimate values
it was given.

### Key terms — KA 4.2

| Term | Meaning |
|---|---|
| **WBS** | Deliverable-oriented hierarchical decomposition of the total scope of work. |
| **Hundred-per-cent rule** | The children of any element sum to exactly that element — an arithmetic invariant, and checkable. |
| **Work package** | The lowest WBS level: one owner, a verifiable outcome, an endorsed estimate, an assessable duration. |
| **Control account** | The level at which performance is measured and reported (Domain 7's earned value attaches here). |
| **Product breakdown structure** | Decomposition of the thing delivered; drives configuration management and acceptance. |
| **Product-to-work cross-match** | The traceability test that every product has work and every work package a product — a candidate list, not a finding. |
| **Orphan work package** | Budgeted work producing no product anyone asked for — waste that still reports 100 % complete. |
| **Legitimately product-free work** | Integration, testing, migration, training delivery, commissioning: real work with no product, and the class a product-derived WBS omits by construction. |
| **Interface** | A defined relationship across a boundary at which something must be agreed and verified. |
| **Mesh vs layered** | `n(n−1)/2` possible interfaces against `n` — the architecture choice priced in WE 4.2.3. |
| **Interface agreement** | The parties, content, format, direction, timing, exceptions, verifier and version of one interface. |

### Sample MCQs — KA 4.2

**MCQ 4.2-A `[4.2.3 · Application]`** A programme integrates 12 components. The number of possible
point-to-point interfaces is:
- A. 12
- B. 66 ✅
- C. 132
- D. 144

*Rationale:* `n(n−1)/2 = 12 × 11/2 = 66` (4.2.3). A is the layered count; C counts each pair twice;
D is `n²`.

**MCQ 4.2-B `[4.2.3 · Evaluation]`** In WE 4.2.3, the strongest argument for the integration layer
is:
- A. it saves 652,000 against the point-to-point design
- B. adding one further component costs 216,000 on a mesh and 18,000 layered — and programmes acquire
  components ✅
- C. it reduces the interface count from 66 to 12
- D. point-to-point interfaces are technically inferior

*Rationale:* The marginal cost of growth is decisive because it compounds, whereas the one-off saving
is a single number on today's component count (4.2.3). D is not established and would be an assertion.

**MCQ 4.2-C `[4.2.1 · Analysis]`** A WBS's five level-2 elements sum to 2,332,000 against an
approved 2,400,000. The correct reading is that:
- A. the project has 68,000 of contingency
- B. the decomposition is incomplete, and the 68,000 will be consumed by whatever arrives first ✅
- C. the baseline should be reduced to 2,332,000
- D. the hundred-per-cent rule permits a 3 % tolerance

*Rationale:* Unallocated budget is a symptom of incomplete decomposition, not contingency (4.2.1);
the rule admits no tolerance, and reducing the baseline would lock in the omission.

**MCQ 4.2-D `[4.2.2 · Comprehension]`** Configuration management applies naturally to a product
breakdown rather than a WBS because:
- A. products are more important than work
- B. configuration items are products, and activities do not have versions ✅
- C. a WBS changes more often
- D. product breakdowns are more detailed

*Rationale:* Versioning is a property of things, not of activities (4.2.2); applying configuration
control to activities is one of the two standard confusions.

**MCQ 4.2-E `[4.2.2 · Analysis]`** A product-to-work cross-match returns 21 raw exceptions, of which
classification shows 14 to be granularity artefacts or legitimately product-free work. The correct
reading is that:
- A. the matrix has 21 defects requiring correction
- B. the finding is the 7-item residue after each exception has been ruled on by the owner of the
  item ✅
- C. a 66.67 % false-positive rate makes the check unreliable and it should be dropped
- D. the false positives indicate the WBS is wrong at 14 places

*Rationale:* The cross-match produces a candidate list; the ruling produces the finding (4.2.2). A
mistakes the candidate list for a defect report; C discards a check for doing exactly what a
candidate-generation step is meant to do; D miscounts artefacts of matrix granularity as WBS defects.

**MCQ 4.2-F `[4.2.2 · Application]`** In WE 4.2.2 the classified findings are 214,000 of products with
no work and 26,000 of work with no product. Removing the orphan work packages from the WBS:
- A. reduces the unallocated residue from 68,000 to 42,000
- B. raises the unallocated residue from 68,000 to 94,000 ✅
- C. leaves the residue unchanged, since the two findings offset
- D. reduces the approved baseline by 26,000

*Rationale:* Allocated cost falls to 2,306,000, so the gap against the approved 2,400,000 widens to
94,000 (4.2.2). A subtracts on the wrong side; C assumes an offset between a finding outside the
baseline and one inside it; D confuses the estimate with the authorisation.

### Self-check — KA 4.2

1. *State the hundred-per-cent rule and why it is checkable.* — Children sum exactly to their parent;
   it is arithmetic, so it can be audited rather than reviewed.
2. *Why are interfaces systematically under-planned?* — Components grow linearly and interfaces
   combinatorially, so a plan written for an earlier component count is always an underestimate.
3. *What two disciplines matter more than the interface document?* — Exactly one named owner on each
   side, and verification as a scheduled activity with duration and resource.

---

## Knowledge Area 4.3 — Integrated baselines

*Topics: 4.3.1 scope–schedule–cost integration · 4.3.2 configuration management · 4.3.3 baseline
maintenance.*

### 4.3.1 Scope, schedule and cost as one baseline

**The principle.** The performance measurement baseline is a **single three-dimensional statement**:
the authorised scope, arranged in time, with cost attached. It is recorded in three documents for
convenience, and treating those documents as three baselines is the origin of most integration
failure.

**The integration invariants** — each testable, and each a real defect when it fails:

- Every scope element appears in the schedule; every schedule activity traces to a scope element.
- The cost baseline sums to the resourced schedule at the resource plan's rates (Domain 7).
- The **time-phased** cost baseline (`PV`, Domain 7's planned value) follows the schedule's dates — so
  a schedule change is a `PV` change, always, and a baseline whose `PV` curve did not move when the
  schedule did is not integrated.
- Contingency and management reserve are held explicitly, at a stated level, and are not distributed
  invisibly into estimates (Domain 8).
- Every deliverable has acceptance criteria; every acceptance activity has duration and resource.

**Why the third invariant is the one that fails.** It requires the cost and schedule tools to be
connected, or a person to maintain the connection, and it is the first thing abandoned under
pressure. The consequence is precise and severe: **earned value becomes meaningless.** Domain 7's
`SPI` and `CPI` compare achievement against a baseline; if the baseline's time-phasing no longer
matches the schedule the project is executing, the indices measure the gap between two documents
rather than the state of the work — and they will keep producing confident numbers while doing it.

By how much, and in which direction, is computable from the same three figures the report already
prints.

**Worked example 4.3.1 — what a stale time-phased baseline does to Auriga's indices, and what it
leaves exactly alone.**

1. **Setup.** Project Auriga, Part Two's single-project thread, carries **BAC USD 4,000,000** over 25
   weeks. At week 13 the reported position is **PV 2,080,000**, **EV 1,920,000**, **AC 2,120,000**,
   giving **CPI 0.91** and **SPI 0.92** (Domain 7, KA 7.3, whose definitions are used unchanged). At
   week 8 a change was approved that re-sequenced downstream work; its cost effect was posted but the
   `PV` curve was never re-phased, so the reported PV is still 2,080,000. Two variants are examined,
   each moving **USD 240,000** of planned value across the week-13 status date: **(a)** the approved
   change **deferred** that value beyond week 13; **(b)** it **pulled it forward** inside week 13.
2. **Formula.** `SPI = EV/PV`; `SV = EV − PV`; `CPI = EV/AC`; the composite forecast
   `EAC = AC + (BAC − EV)/(CPI × SPI)`, alongside the two CPI-only forms `BAC + AC − EV` and
   `BAC/CPI`.
3. **Substitution.** Reported `1,920,000/2,080,000`. Variant (a) `1,920,000/1,840,000`; variant (b)
   `1,920,000/2,320,000`. `CPI = 1,920,000/2,120,000` in all three columns.
4. **Result.**

   | Measure | Reported (stale PV) | (a) deferral — true PV 1,840,000 | (b) acceleration — true PV 2,320,000 |
   |---|---|---|---|
   | `SPI` | 0.9231 | **1.0435** | **0.8276** |
   | `SV` (USD) | (160,000) | **80,000** | **(400,000)** |
   | `CPI` | 0.9057 | 0.9057 | 0.9057 |
   | `EAC = BAC + AC − EV` | 4,200,000 | 4,200,000 | 4,200,000 |
   | `EAC = BAC/CPI` | 4,416,667 | 4,416,667 | 4,416,667 |
   | composite `EAC` | 4,608,056 | **4,320,972** | **4,895,139** |

   The same EV, the same AC and the same authorised scope produce an `SPI` anywhere between **0.8276**
   and **1.0435** — a spread of **0.2159** — according only to whether the PV curve was maintained.
   `CPI` and both CPI-only forecasts are **identical in all three columns**.
5. **Interpretation.** The identity that falls out of the table is the reason this defect is so
   durable: **a stale PV curve corrupts every schedule measure and leaves every cost-only measure
   exactly correct.** So the cost report reconciles to the ledger, the CPI-based forecast is
   defensible line by line, the finance function signs it — and the schedule indices printed on the
   facing page are wrong by up to a fifth of an index point. A reviewer who tests only the arithmetic
   *inside* the report will find nothing wrong with it, because nothing inside it is wrong. The defect
   is in the relationship between the report and the schedule, which is the one thing the report
   cannot show.

   The composite `EAC` is where the two halves meet, and it behaves in a way worth remembering because
   it makes the exposure quantifiable. Written as `AC + (BAC − EV) × PV/(CPI × EV)`, the composite is
   **linear in PV**, so its sensitivity is constant: `(BAC − EV)/(CPI × EV) =` **1.1962** here, meaning
   **each USD 1 of mis-phased planned value moves the forecast by USD 1.20**. The 240,000 of
   mis-phasing therefore moves the composite forecast by **USD 287,083** — **7.18 %** of BAC — and,
   because the relationship is linear, by *exactly* that amount in either direction. The error cannot
   be defended as conservative, which is the usual defence offered.

   The two directions have opposite management consequences, and the flattering one is the more common
   because deferral is the more common change. Variant (a) reports 0.92 on a project actually running
   at **1.0435** against the authorised schedule: a recovery plan will be launched, overtime
   authorised and float consumed to fix a problem the project does not have — real cost incurred to
   correct a reporting defect, and the composite forecast overstated by 287,083 into the bargain.
   Variant (b) reports the same 0.92 on a project running at **0.8276**: true schedule variance is
   (400,000) rather than (160,000), the composite forecast is understated by 287,083, and both facts
   surface at the next milestone rather than in the report designed to surface them. Neither variant
   contains a false statement and neither is detectable from the indices themselves.

   What makes it detectable is one subtraction, and it is the check KA 4.3.3 formalises: does the sum
   of the approved changes' schedule effects equal the movement in the PV curve? Here it plainly does
   not — the curve did not move at all — and the finding is available in minutes to anyone who asks
   the question. That is why the invariant is better stated as a **sequence obligation** than as good
   practice: **the PV curve is re-phased in the same transaction that approves the change, or it will
   not be re-phased at all.** Anything deferred to a later housekeeping pass competes with delivery
   and loses, for the same structural reason KA 4.4.2 gives for impact assessment.

   One caution against over-reading the 0.2159 spread. It is the consequence of a *single* 240,000
   re-sequencing on a project whose phasing is otherwise sound. A programme with a year of
   unmaintained phasing does not have a slightly wrong `SPI`; it has an `SPI` whose relationship to
   the work is unknown, and no sensitivity calculation recovers it. The honest treatment in that case
   is to **suspend the index and say so in the report**, restating it only when the baseline has been
   re-phased against the current schedule. Continuing to publish a schedule index with a footnote
   about baseline quality is the specific failure to avoid: readers take the number and leave the
   footnote.

**The two-dimensional trap.** The commonest expression of a non-integrated baseline is a change
approved on cost alone. A change is assessed for its cost, approved, and the schedule is not
re-baselined — so the project is delivered late against a baseline that never acknowledged the time,
and the lateness is attributed to execution. KA 4.4 prices the general case.

### 4.3.2 Configuration management

**Definition.** Configuration management is the discipline of **knowing, at any moment, exactly which
version of every controlled item is the approved one**, and how it came to be. Its four functions:
*identification* (what is controlled, and its identifier), *control* (how a version changes and on
whose authority), *status accounting* (what the current version of everything is), and *audit*
(verifying that the actual state matches the recorded state).

**Why it belongs in this domain rather than in quality.** Integration depends on it absolutely. When
two components are integrated, the question is not whether each works but whether **these versions**
work together — and an interface verified against version 3 of a specification tells you nothing
about version 5. Every interface agreement is therefore version-bound (KA 4.2.3), and configuration
management is what makes the binding meaningful.

**Worked example 4.3.2 — auditing Meridian's configuration register.**

1. **Setup.** A configuration audit covers Meridian's **340** controlled items. It finds **296** with
   a single identified current version and a complete change history; **28** with no version
   reference at all; **11** with two items marked current; and **5** whose recorded current version
   does not match the version actually deployed.
2. **Formula.** Defect rate = non-conforming items ÷ total. Classify by consequence.
3. **Substitution.** `(28 + 11 + 5)/340`.
4. **Result.** **44** non-conforming items — a **12.94 %** defect rate — of which 28 are
   unidentified, 11 ambiguous and **5 actively wrong**.
5. **Interpretation.** The three classes are not equally serious, and totalling them is the mistake a
   status report usually makes. The **28 unidentified** items cannot be integrated with confidence
   because no interface agreement can bind to them; the cost is rework at integration testing. The
   **11 ambiguous** items will each cause a decision by whoever picks first, and the wrong pick is
   discovered downstream. The **5 wrong** items are the serious finding: the register says something
   untrue, so verification performed against the register has verified nothing — and this is the class
   that produces the failure in which every component passed its own tests and the system did not
   work. A 12.94 % headline rate understates the position, because those five items alone can fail an
   integration.

**The pairwise consequence, which is where the register is actually used.** An item-level defect rate
answers a question nobody asks. What integration needs to know is whether **an interface** can be
verified, and an interface is verifiable only if *both* of its sides are on their recorded version.
That turns an item-level rate into a relationship-level rate through an exponent, and the exponent is
where the surprises are.

**Worked example 4.3.2b — why an 87 % register gives a 76 % integration, and what the audit tolerance
therefore has to be.**

1. **Setup.** WE 4.3.2's audit found **296** of **340** controlled items conforming — **87.06 %**.
   Meridian has **31** genuinely required interfaces (Case study A's count), each side of each interface
   bound to a controlled item at a stated version. Treat all 31 as two-sided, which is the optimistic
   case; the three-party form — programme, national reporting gateway and identity provider — is computed
   alongside for comparison.
2. **Formula.** For a relationship spanning `k` controlled items whose individual conformance is `p`,
   and whose defects are independent, the probability that the whole relationship is verifiable is
   `p^k`. Expected at-risk relationships = `count × (1 − p^k)`. Inverting for design: the item
   conformance required to achieve a relationship-conformance target `q` across `k` items is
   `p = q^(1/k)`.
3. **Substitution.** `p = 296/340`. Two-sided `p² = 0.870588²`; three-party `p³`. At-risk
   `31 × (1 − p²)`. For an expected at-risk count of one interface in 31: `p = (30/31)^(1/2)`.
4. **Result.** Item conformance **87.06 %** yields two-sided interface conformance **75.79 %** and
   three-party conformance **65.98 %**. Of 31 required interfaces treated as two-sided, **7.50** are
   expected to be at risk — more where any of them is three-party, since `1 − p³ =` **34.02 %** against
   `1 − p² =` 24.21 %. Interface-level failure (**24.21 %**) is **1.87 times** item-level failure
   (12.94 %). To hold the expected at-risk count at **one** interface, item conformance must reach
   **98.37 %** — at most **5** non-conforming items out of 340. The audit found **44**.
5. **Interpretation.** The exponent is the whole lesson and it is routinely missed: **integration
   quality is item quality raised to the number of parties**, so a register that looks tolerable where
   it is measured is materially worse where it is used. 87 % is the kind of number that earns a green
   status. 75.79 % is not, and 65.98 % on a three-party interface is a different conversation with a
   different audience. The direction never reverses — the assembled relationship is always worse than
   its parts — which is why an item-level dashboard over-reports integration readiness systematically
   rather than occasionally.

   The inversion is the practically useful half, because it converts an integration target into an
   audit tolerance instead of leaving the tolerance to be chosen by whoever writes the procedure.
   Meridian's audit had **no stated tolerance at all**; had it wanted 95 % of interfaces verifiable it
   needed `0.95^(1/2) =` **97.47 %** item conformance, which is at most **8** non-conforming items.
   State the rule in that direction: **the tolerance on the register is derived from the target on the
   interfaces, and the derivation runs backwards through the exponent.** A configuration tolerance set
   directly at item level — "under 10 % non-conforming is acceptable" — is a number with no argument
   behind it, and on a two-sided architecture a 10 % item tolerance silently permits a **19 %**
   interface failure rate that nobody has agreed to.

   The audit *interval* falls out of the same arithmetic, and the answer is instructive because it is
   impossible. WE 4.3.2's 44 non-conforming items accumulated over the **6 months** since the previous
   audit — **7.33** items a month. To stay inside even the loose 5-item tolerance the audit would have
   to run every `5/7.33 =` **0.68 months**, about **3 weeks**, which no manual configuration audit
   sustains. That is not an argument for a heroic audit schedule; it is the diagnostic. **When the
   derived audit interval is shorter than the audit can feasibly run, the finding is about
   identification discipline, not about audit frequency.** The register is being written badly and has
   to be fixed at the point of writing — which is the automation case in the AI note below, and the
   reason software delivery largely solved this problem through version control while manual registers
   largely have not (see Industry variations).

   Two cautions on the model, because it is a bound rather than a measurement. **Independence is a
   generous assumption**: register defects cluster by team, by system and by the week a migration
   happened, so where both sides of an interface are maintained by the same people with the same habit
   the real pairwise conformance is *worse* than `p^k`. Use `p^k` as an optimistic bound and label it
   as one. And `p` must be the conformance of the items **actually bound to interface agreements**, not
   of the whole register: the 340 items include many that no agreement references, and if the
   non-conforming 44 are concentrated among the referenced ones the interface exposure is worse again.
   A reviewer's first question is therefore not the headline defect rate but *which items the interface
   register binds to, and what their conformance is measured separately.*

> **Fig 4.3.1 — Pairwise integrity is the power of item integrity.** Line chart, x-axis item-level
> configuration conformance `p` from **80 %** to **100 %**, y-axis conformance of the assembled
> relationship from 50 % to 100 %. Three curves: the reference line `p` (dashed grey, one item), `p²`
> (brand blue, a two-sided interface) and `p³` (crimson dashed, a three-party interface). Meridian is
> marked on the `p²` curve at **87.06 % → 75.79 %**, annotated **"296/340 = 87.06 % of items, so only
> 75.79 % of two-sided interfaces"**. A second marker at **97.47 %** on the `p²` curve, annotated
> **"a 95 % interface target needs 97.47 % item conformance"**. Footnote: an interface is verified only
> if both sides are on their recorded version, so the curve — not the item rate — sets the audit
> tolerance. Source: PCI original. Alt text: three downward-bowing curves converging at 100 %, with the
> two- and three-party curves falling increasingly far below the item-level line as conformance drops.

### 4.3.3 Baseline maintenance

**The obligation.** A baseline is not a historical record; it is the current authorised statement of
the project, and maintaining it means that every change is applied **with traceability to the
authority that approved it**, while the original remains reconstructable.

**What must be preserved.** The original baseline, unaltered; every approved change with its
reference, date, authority and effect on scope, schedule and cost; the current baseline as the sum of
the original and the approved changes; and the ability to reproduce any intermediate state. That last
requirement is what makes variance analysis honest a year later — and it is the reason a baseline is
maintained by accumulation rather than by replacement.

**Re-baselining.** Occasionally a baseline becomes so distant from reality that variance against it
conveys nothing, and re-baselining is legitimate. It is also the most abused instrument in project
control, because it makes an adverse variance disappear without anything having improved. Three
disciplines keep it honest: re-baselining requires **the authority that approved the original**, not
the project's own; the **reason is recorded** and is a substantive change in circumstance rather than
accumulated variance; and the **original remains visible in reporting**, so that performance against
the original commitment can still be stated. A project on its third baseline with no visible original
has lost the ability to answer the only question that matters to the organisation that funded it.

**Drift: the failure mode with no decision in it.**

**Worked example 4.3.3 — how Meridian's baseline moved 12.1 % without a decision.**

1. **Setup.** Over year one, Meridian approved **34** changes, each below the project leader's then
   **10,000** authority, at an average direct cost of **6,800**. Of those, **14** carried
   critical-path impact averaging **0.3 weeks** each; cost of delay **14,280** per week; baseline
   **2,400,000**.
2. **Formula.** Direct drift = count × average cost. Schedule drift = affected count × average weeks
   × cost of delay. Express as a share of baseline, and per change.
3. **Substitution.** `34 × 6,800`; `14 × 0.3 × 14,280`.
4. **Result.** Direct **USD 231,200**; schedule impact `4.2 weeks =` **USD 59,976**; total
   **USD 291,176** — **12.1 %** of the baseline. Each individual change was **0.28 %** of it.
5. **Interpretation.** There was no point at which anyone decided to spend 291,176 or to accept 4.2
   weeks of delay, and no individual approval was wrong on its own terms. This is Domain 3's Case
   study B mechanism with a number attached, and it is why a delegation schedule needs a **cumulative
   test**. But the test's parameters have to be derived rather than chosen, and here the arithmetic is
   unforgiving: a rule of "related changes aggregating above 100,000 in a rolling 90 days requires
   steering authority" would **not** have caught this, because 34 changes a year is about `34/4 = 8.5`
   per quarter, or **57,800** — comfortably under 100,000. Catching it needs either a threshold below
   **57,800** on a 90-day window or the same 100,000 threshold on a **180-day** window
   (`17 × 6,800 =` **115,600**, which trips it). The professional point generalises past this example:
   **a cumulative test set at a round number without reference to the observed change rate provides
   the appearance of a control and none of the function** — and the observed change rate is available
   from the change log of any project that has run for a quarter.

**The check that finds what the cumulative test cannot.** Drift of the kind above is entirely inside
the change log, so summing the log detects it. The complementary failure — baseline movement that
never reached the log — is invisible to any cumulative test, because cumulative tests read the log. It
is caught instead by the reconciliation that maintenance by accumulation makes possible: *original
plus the sum of approved changes equals the current baseline, exactly.* It is one addition and one
subtraction, and it is habitually performed wrongly in the same way.

**Worked example 4.3.3b — Meridian's reconciliation, and the residual that netting hid.**

1. **Setup.** Meridian's original approved baseline is **USD 2,400,000**. The year-one change log holds
   the **34** small changes above (**231,200** in total) and **3** steering-approved changes at
   **86,000**, **142,000** and **57,500**. The cost tool reports a current baseline of
   **USD 2,948,900**.
2. **Formula.** Original + Σ approved changes = current baseline. Residual = current − expected. Then
   **decompose the residual by class before netting**: approved changes with no recorded baseline
   effect (which make the tool *lower* than the log implies) against baseline movements with no change
   reference (which make it *higher*).
3. **Substitution.** Approved total `231,200 + 86,000 + 142,000 + 57,500`. Expected current
   `2,400,000 + 516,700`. Residual `2,948,900 − 2,916,700`.
4. **Result.** Approved changes total **USD 516,700**; expected current baseline **USD 2,916,700**;
   residual **USD 32,200**, or **1.34 %** of the original. Investigation decomposes it: **2** approved
   changes worth **44,900** (18,400 and 26,500) carry no baseline effect, and baseline movements
   totalling **77,100** carry no change reference — `77,100 − 44,900 = 32,200`, which closes. The
   **gross** error is **USD 122,000**: **23.61 %** of the approved changes and **5.08 %** of the
   original baseline. Netting concealed **USD 89,800** of it.
5. **Interpretation.** The arithmetic that matters is that **the two classes have opposite signs, so
   netting them destroys the finding.** A residual of 32,200 on a 2.4 m baseline is 1.34 % and will be
   written off as timing or rounding by any reviewer working at that resolution, while the actual defect
   is **3.79 times** larger. Hence the single most useful sentence in this KA: **reconcile gross, never
   net.** A reconciliation that balances to zero is not evidence of integrity; it is evidence that the
   errors happened to be of similar size. The concealment is not random either — it is exactly twice the
   smaller class, `2 × 44,900 = 89,800`, which is a useful thing to be able to say out loud when someone
   proposes to report the net.

   The two classes are not equally serious and they have different remedies. **44,900 of
   approved-but-unrecorded changes** means work has authority and no budget line: it will be delivered
   and charged, and the variance will surface as an overrun against a baseline that never received the
   change. The remedy is bookkeeping — post the effects; the authority is already on file. **77,100 of
   movement with no change reference** is the governance finding: budget exists that no recorded
   decision created. Its remedy is not a posting but an investigation, because there are two possible
   answers and they are very different. Either a decision was taken and not recorded — a Domain 3, KA
   3.3.4 failure, and one of the three things KA 4.1.3 places outside tailoring — or no decision was
   taken at all, and someone has moved a baseline without authority. A reconciliation report that states
   the residual and not its classes has withheld precisely the information that distinguishes those two.

   Note the relationship to drift, because the two controls are complementary rather than overlapping.
   The 291,176 of drift above was **entirely inside the change log** — every dollar authorised,
   recorded and traceable, invisible only because nobody summed the column. The 77,100 here is
   **outside the log altogether**, so no cumulative test at any threshold or window would ever see it.
   **Drift controls and reconciliation controls catch disjoint failures**, and a programme with an
   excellently derived cumulative test and no reconciliation has covered one of the two and will
   believe it has covered both.

   Cost and cadence are worth stating because they are the usual objection. The reconciliation is one
   addition and one subtraction over three figures a programme already holds — call it an hour a month,
   **USD 1,320** a year at the blended rate, against 122,000 of error found in year one. Run it monthly
   rather than at gates, and for an arithmetic reason rather than a diligence one: the *check* does not
   get harder with delay, but the *decomposition* does. Finding which movements are unreferenced is a
   search over one month's transactions if run monthly and over a year's if run annually, and it is the
   decomposition that carries all the value.

   What a reviewer should test, in order: that the three figures are quoted from three systems of record
   rather than from one report that derives two of them; that the residual is decomposed before it is
   characterised; and that a **zero** residual has been tested rather than assumed — because the
   commonest way to produce one is to derive the current baseline *from* the change log, which makes the
   identity true by construction and the control worthless. A reconciliation that has never produced a
   non-zero residual is not a clean programme; it is an unexamined formula.

> **Fig 4.3.2 — Reconcile gross, never net.** Bridge (waterfall) chart, left to right: original
> baseline **2,400,000**; plus approved changes **+516,700**; equals expected current baseline
> **2,916,700** (dashed reference line); plus baseline movements with no change reference
> **+77,100**; less approved changes with no baseline effect **−44,900**; equals the cost tool's
> current baseline **2,948,900**. The two residual segments are crimson, the authorised segments brand
> blue. Header annotation: **"Net residual 32,200 (1.34 % of baseline) — gross error 122,000 (23.61 %
> of the approved changes)"**, with a second line: **"the two error classes have opposite signs, so
> netting them hides 89,800 of the 122,000"**. Source: PCI original. Alt text: a bridge chart in which
> two small opposite-signed steps almost cancel, so the final bar sits only slightly above the expected
> baseline despite a much larger underlying error.

### AI in this KA

**Where it earns its place.** Continuous baseline-integrity checking: does the time-phased cost
baseline still match the schedule's dates; do the change log's entries sum to the difference between
the original and current baselines; are there approved changes with no baseline effect recorded, or
baseline movements with no change reference? All four are reconciliations with definite answers, and
the fourth in particular is how drift is detected early, and the second is the reconciliation of
WE 4.3.3b, which must be decomposed by class rather than netted. Configuration status accounting —
comparing the register against a deployed inventory and reporting the discrepancy classes of WE 4.3.2.
Joining the configuration register to the interface register to compute the pairwise exposure of
WE 4.3.2b, which is the only way to know which *interfaces* the item defects actually threaten. Trend
analysis on the change log to derive the observed change rate a cumulative test should be set from, and
the register's defect accumulation rate an audit interval should be set from.

**Where it must not go.** Authorising a baseline change or a re-baseline. Reconciling a discrepancy
by adjusting the register to match reality, which destroys the evidence that the discrepancy existed
— the correction must be a recorded, authorised change, and this is a real and tempting automation
failure rather than a hypothetical one.

**Verification, concretely.** Every reported discrepancy is confirmed against both sources before it
is treated as a finding; the change-log-sums-to-baseline-delta reconciliation is reproduced by hand
at each reporting period, because it is a subtraction and it is the single most informative check in
this KA; and no register is amended by a tool without an authorised change reference.

### Key terms — KA 4.3

| Term | Meaning |
|---|---|
| **Performance measurement baseline** | The authorised scope, arranged in time, with cost attached — one statement in three documents. |
| **Time-phased cost baseline** | The `PV` curve; it must move whenever the schedule does. |
| **Configuration management** | Knowing which version of every controlled item is approved, and how it came to be. |
| **Status accounting** | The record of the current version of every controlled item. |
| **Configuration audit** | Verification that the actual state matches the recorded state. |
| **Pairwise configuration integrity** | `p^k` — the conformance of a relationship spanning `k` controlled items, always worse than the item rate `p`. |
| **Derived audit tolerance** | The item-level tolerance obtained by inverting `p^k` from an interface-level target: `p = q^(1/k)`. |
| **Original-to-current reconciliation** | Original baseline + Σ approved changes = current baseline, exactly, every reporting period. |
| **Gross residual** | The sum of the reconciliation's error classes before netting — the figure that carries the finding. |
| **Baseline maintenance** | Applying changes by accumulation, with traceability to authority, preserving the original. |
| **Re-baselining** | Replacing a baseline that no longer conveys information — legitimate, and the most abused instrument in project control. |
| **Baseline drift** | Cumulative movement through individually authorised small changes, with no decision on the total. |
| **Cumulative test** | A threshold on aggregated related changes over a stated period, set from the observed change rate. |

### Sample MCQs — KA 4.3

**MCQ 4.3-A `[4.3.1 · Analysis]`** A schedule change is approved and the time-phased cost baseline
is not updated. The most serious consequence is that:
- A. the cost baseline is slightly inaccurate
- B. earned value indices now measure the gap between two documents rather than the state of the work
  ✅
- C. the schedule must be re-baselined
- D. contingency is understated

*Rationale:* `SPI` and `CPI` compare achievement against the time-phased baseline; if it no longer
matches the executing schedule they produce confident numbers about nothing (4.3.1).

**MCQ 4.3-B `[4.3.2 · Evaluation]`** In a 340-item configuration audit, 28 items have no version
reference, 11 have two current versions and 5 have a recorded version that differs from what is
deployed. Which is the most serious class, and why?
- A. the 28, because they are the most numerous
- B. the 11, because ambiguity blocks decisions
- C. the 5, because verification against the register has verified nothing ✅
- D. all three are equally serious at a 12.94 % defect rate

*Rationale:* A register that states something untrue invalidates any verification performed against
it, which is the class that produces "every component passed and the system failed" (4.3.2).

**MCQ 4.3-C `[4.3.3 · Application]`** 34 changes averaging 6,800 direct cost were approved over a
year, 14 of them carrying 0.3 weeks of critical-path impact at a delay cost of 14,280 per week.
Total baseline drift is closest to:
- A. USD 231,200
- B. USD 291,176 ✅
- C. USD 59,976
- D. USD 376,856

*Rationale:* `34 × 6,800 = 231,200` direct plus `14 × 0.3 × 14,280 = 59,976` schedule impact
(4.3.3). A omits the schedule impact; C is the schedule impact alone; D applies the schedule impact
to all 34 changes rather than the 14 that carried it.

**MCQ 4.3-D `[4.3.3 · Evaluation]`** For the drift above, a cumulative test of "related changes above
100,000 in a rolling 90 days" would:
- A. have caught it, since total drift exceeds 100,000
- B. not have caught it, since a quarter's changes aggregate to about 57,800 ✅
- C. have caught it only if the threshold were raised
- D. be inapplicable to changes below the delegation threshold

*Rationale:* The 90-day aggregate is `(34/4) × 6,800 ≈ 57,800`, below the threshold (4.3.3) — a
cumulative test set at a round number without reference to the observed change rate has the
appearance of a control and none of the function.

**MCQ 4.3-E `[4.3.3 · Comprehension]`** The discipline that most protects re-baselining from abuse
is:
- A. re-baselining no more than once a year
- B. keeping the original baseline visible in reporting alongside the current one ✅
- C. having the project leader approve it
- D. recalculating contingency at each re-baseline

*Rationale:* Visibility of the original preserves the ability to state performance against the
original commitment (4.3.3). C is precisely the wrong authority.

**MCQ 4.3-F `[4.3.2 · Application]`** A configuration register holds 340 items of which 296 conform.
Assuming independent defects, the expected share of **two-sided interfaces** whose verification cannot
be relied on is closest to:
- A. 12.94 %
- B. 24.21 % ✅
- C. 25.88 %
- D. 75.79 %

*Rationale:* `1 − (296/340)² = 1 − 0.7579 = 0.2421` (4.3.2b). A applies the item failure rate directly
to interfaces; C doubles the item rate, which is the linear approximation and overstates slightly; D
quotes the conformance rather than the failure.

**MCQ 4.3-G `[4.3.3 · Analysis]`** A reconciliation shows a residual of 32,200, which decomposes into
77,100 of baseline movement with no change reference and 44,900 of approved changes with no baseline
effect. The correct reading is that:
- A. the residual is 1.34 % of the baseline and immaterial
- B. the gross error is 122,000 and must be reported as such, because the two classes have opposite
  signs ✅
- C. the cost tool is correct and the change log needs updating
- D. netting is appropriate because the classes offset within the same baseline

*Rationale:* Opposite-signed classes make the net understate the gross by twice the smaller class
(4.3.3b), so 89,800 of the 122,000 is concealed. A and D both read the net as the finding — A by
applying a materiality test to it, D by asserting the offset is legitimate. C prejudges which record is
wrong when the two classes have opposite remedies.

**MCQ 4.3-H `[4.3.1 · Analysis]`** A schedule change is approved and the `PV` curve is never re-phased.
Which reported measure remains exactly correct?
- A. `SPI`
- B. `SV`
- C. `CPI` ✅
- D. the composite `EAC = AC + (BAC − EV)/(CPI × SPI)`

*Rationale:* `CPI = EV/AC` contains no PV term, so it and the CPI-only forecasts are unaffected, which
is why the defect passes a cost review (4.3.1). A and B are computed against PV directly; D is linear in
PV and moves by USD 1.20 per USD 1 mis-phased on Auriga's figures.

### Self-check — KA 4.3

1. *Why is a schedule change always a cost-baseline change?* — Because the cost baseline is
   time-phased; if its curve does not move, earned value stops measuring the work.
2. *Which earned-value measures does a stale `PV` curve leave correct, and why does that matter?* —
   `CPI` and both CPI-only forecasts, because they contain no PV term; it matters because the cost
   review therefore passes and the defect survives.
3. *Which configuration-audit finding class is most serious, and why?* — Recorded versions that
   differ from deployed ones, because verification against the register then proves nothing.
4. *How is an item-level configuration tolerance derived?* — By inverting `p^k` from the interface-level
   target: `p = q^(1/k)`. Chosen directly at item level it has no argument behind it.
5. *Why must a reconciliation residual be reported gross?* — Because opposite-signed error classes make
   the net understate the gross by twice the smaller class, and a zero residual is not evidence of
   integrity.
6. *How should a cumulative-change threshold be set?* — From the observed change rate in the change
   log, not from a round number.

---

## Knowledge Area 4.4 — Integrated change control

*Topics: 4.4.1 change flow · 4.4.2 impact assessment · 4.4.3 the change board and the decision log.*

### 4.4.1 The change flow

**Why "integrated".** The word is the whole point: a change to scope is a change to schedule and to
cost, and a control process that assesses one dimension has not controlled the change. The flow that
does:

1. **Raise** — anyone may raise; the request states the change and its reason, and is logged on
   receipt with a reference, so that a rejected or withdrawn request still leaves a trace.
2. **Screen** — is it a change at all? Three outcomes are commonly confused and should be separated
   explicitly: a **change** (the baseline moves), a **clarification** (the baseline already covered
   it and someone misread it), and a **defect** (the work does not meet the baseline and must be
   corrected at no baseline movement). Misclassifying a defect as a change is how a supplier is paid
   twice; misclassifying a change as a clarification is how scope grows silently.
3. **Assess impact** — across all dimensions, per KA 4.4.2. This is the step that is
   under-resourced, and the reason is that assessment costs money before any decision has been taken
   to spend money.
4. **Decide** — at the authority the *assessed* impact requires, not the quoted cost (4.4.2's central
   point), with the decision recorded per Domain 3, KA 3.3.4.
5. **Implement and baseline** — update scope, schedule and cost together, with the change reference
   recorded against each, so that KA 4.3's reconciliation works.
6. **Verify and close** — confirm the change was implemented as approved, including its interface and
   documentation consequences.

Of the six steps, screening is the only one whose output is a **classification** rather than a number
or a decision, and classifications are the cheapest thing in the flow to get right and the most
expensive to reverse, because by the time a misclassification is discovered money has moved — in one
direction or the other, and the two directions do not cost the same.

**Worked example 4.4.1 — what Meridian's screening errors cost, in both directions.**

1. **Setup.** Meridian logged **76** requests in year one. Screening ruled **51** changes, **16**
   clarifications and **9** defects; of the 51 changes, **37** were subsequently approved — the 34 below
   the leader's threshold plus the 3 taken to steering — and **14** were rejected or withdrawn. An
   assurance re-screen against the baseline found **8** misclassifications: **5** of the approved
   "changes" were **defects** — work already owed under the baseline, at the change log's average
   approved value of **6,800** — and **3** of the 16 "clarifications" were **changes**, carrying direct
   work of **9,200**, **14,600** and **5,400**, one of them with **0.5 weeks** of critical-path impact
   (cost of delay **14,280** per week).
2. **Formula.** Error rate = misclassifications ÷ requests. Defect-as-change cost = count × average
   approved value (the sum paid a second time for an existing obligation). Change-as-clarification cost
   = Σ direct + schedule impact (unbudgeted, unlogged, and outside the baseline). Then compare the
   **net** effect on the recorded change total with the **gross** error.
3. **Substitution.** `8/76`. `5 × 6,800`. `9,200 + 14,600 + 5,400 + 0.5 × 14,280`.
4. **Result.** Screening error rate **10.53 %**. Defects paid as changes **USD 34,000** — which is
   **14.71 %** of the 231,200 of direct drift computed in WE 4.3.3, so that figure includes that much
   work which was never a change at all. Changes taken as clarifications **USD 36,340**, none of it in
   any baseline or log. Total misclassification cost **USD 70,340**, or **USD 8,792.50** per error. The
   **net** effect on the recorded change total is `36,340 − 34,000 =` **USD 2,340** — **0.10 %** of the
   baseline — giving a corrected drift of **293,516**, or **12.23 %**, against WE 4.3.3's 291,176.
5. **Interpretation.** The two errors point in opposite directions and — for the second time in this
   domain, from wholly independent data — **netting them hides both.** A corrected drift figure that
   moves by 2,340 is inside anyone's materiality threshold while concealing 70,340 of gross
   misclassification, and the concealment is again exactly twice the smaller class: `2 × 34,000 =`
   **68,000** of the 70,340, where WE 4.3.3b concealed `2 × 44,900 =` 89,800 of 122,000. That
   recurrence is a property rather than a coincidence, and it is worth carrying out of the domain as
   one: **wherever a reconciliation has two error classes of opposite sign, the net understates the
   gross by twice the smaller class.** Report both figures or report neither.

   The two errors also differ in **how they are discovered**, and that asymmetry decides where the
   control belongs. Paying twice for an existing obligation shows up in the cost report: money leaves,
   a variance appears, someone asks. Silent scope growth shows up nowhere — no change record, no
   baseline movement, no variance until the work is executed, at which point it is indistinguishable
   from ordinary productivity. So the direction that costs slightly *more* on this year's arithmetic
   (36,340 against 34,000) is also the direction with no downstream detector, and it is the one that
   needs the tighter control. A screening process designed only to stop overpayment has been designed
   against the visible half.

   Whether the 34,000 can actually be recovered is a contractual question, not a project one. It turns
   on the terms, on any notice or claim period, on whether the work was accepted and paid without
   reservation, and on the governing jurisdiction. **Take contractual advice before treating a
   misclassification finding as recoverable income**: the arithmetic establishes that value was
   transferred, not that it can be reclaimed, and a leader who reports the second when they have only
   established the first will be corrected expensively.

   The remedy is cheap and specific, and it is a change of form rather than of effort. Screening becomes
   a **recorded ruling** instead of an administrative sort: the baseline clause or requirement relied on
   is cited, a named technical authority signs, and the three outcomes are separate fields rather than a
   checkbox. Costed at half an hour of a technical authority and half an hour of the change manager per
   request — **USD 110** at the blended rate, **USD 8,360** across 76 requests — against 70,340 of
   error, a ratio of **8.41**. The ratio is not the point and will vary; the point is that the ruling is
   the only artefact in the flow that makes a classification **contestable later**, and an
   uncontestable classification is one nobody will revisit.

   One caution about the base rate, because a 10.53 % error rate invites the wrong conclusion. It is not
   in itself evidence of poor screening: some proportion of requests genuinely sits on the change/defect
   boundary and will be ruled differently by two competent reviewers, and the honest comparison is
   against that irreducible rate rather than against zero. What a reviewer should test is whether the
   disputed rulings were disputed **on the record**. A screening process that has reversed no ruling in a
   year is not accurate; it is unexamined.

**The discipline that holds it together.** No work begins on an unapproved change. That is easy to
write and hard to hold, because the pressure is always to start — and the specific damage is not the
work itself but that it removes the option to say no, converting a decision into a ratification.
Where genuine urgency exists, the answer is an **emergency change route** with a named authority, a
short deadline and mandatory retrospective ratification, which is the same instrument as Domain 3's
out-of-cycle mechanism. An emergency route that exists is used and recorded; one that does not exist
is used and not recorded.

**And the route's usage rate is a diagnostic, not a discipline problem.** Meridian's 76 requests
included **5** routed as emergencies — **6.58 %**. A rising share is almost never indiscipline; it is
the normal route's latency being priced by the people who have to live with it, and the correct
response is to reduce that latency — the paper deadline before the meeting interval, per Domain 3's
lever asymmetry — rather than to tighten the emergency criteria, which merely moves the same work into
unrecorded starts and removes the only trace it was leaving. So set a **review trigger** on the share
rather than a target for it: when the share rises, examine the normal route, not the requesters.

### 4.4.2 Impact assessment — what a change actually costs

**The central claim of this KA.** The figure on a change request is the *direct* cost of doing the new
work, and it is usually the smallest component of the change's true cost. Everything else — schedule
consequence, rework of completed work, interface re-verification, regression testing, documentation
and training — is real, incurred, and absent from the form.

A complete assessment covers: **direct cost** of the new work; **schedule** impact, on the critical
path or on float, priced at the cost of delay; **rework** of work already completed to the old
specification; **interface** re-verification for every affected interface (KA 4.2.3); **regression
testing** of what was already proven; **documentation, training and communication** updates;
**risk** profile change (Domain 8); and **benefit** change, since a change that reduces benefit has a
cost that appears nowhere in a cost assessment (Domain 2).

**Worked example 4.4.2 — the change that cost 3.29 times its quoted price.**

1. **Setup.** Meridian receives a change request to add a national-reporting field set. The quoted
   direct build cost is **USD 40,000**. Assessment establishes: **2 weeks** of critical-path impact
   (cost of delay **14,280** per week); **22,000** of rework to records-application screens already
   completed; **3** affected interfaces requiring re-verification at **6,000** each; **14,000** of
   regression testing; and **9,000** of documentation and clinician-training updates.
2. **Formula.** True cost = direct + (schedule weeks × cost of delay) + rework + (interfaces ×
   re-verification) + regression + documentation.
3. **Substitution.** `40,000 + 2 × 14,280 + 22,000 + 3 × 6,000 + 14,000 + 9,000`.
4. **Result.** `40,000 + 28,560 + 22,000 + 18,000 + 14,000 + 9,000 =` **USD 131,560** — **3.29
   times** the quoted figure. The quoted cost is **30.4 %** of the true cost.
5. **Interpretation.** Two consequences follow, and the second is the more important because it is
   structural. First, a change board deciding on the quoted 40,000 is deciding on 30 % of the
   information, and it will approve changes it would have refused — not through any failure of
   judgement but because the number in front of it was the wrong number. Second, and this connects
   directly to Domain 3: **a delegation threshold applied to quoted direct cost is not a control.**
   Meridian's threshold is 25,000, so this change escalates. But a change quoted at **22,000** with
   the same two weeks of critical-path impact carries a true cost of at least
   `22,000 + 28,560 =` **USD 50,560** — twice the threshold — and is decided by the project leader
   alone, because the form asked for the direct cost. The remedy is a single sentence in the
   delegation schedule — *the threshold reads on assessed total impact, not on quoted direct cost* —
   and it is worth more than most process improvements a programme will make. Note, finally, what
   this arithmetic is not: it is not an argument against change. Changes are frequently worth
   3.29 times their quoted cost, and Domain 2's business-case logic still decides. It is an argument
   for **assessing before deciding**.

> **Fig 4.4.1 — What a change actually costs.** Waterfall chart, left to right: quoted direct build
> **40,000**, then additive segments — schedule (2 weeks at 14,280) **+28,560**, rework **+22,000**,
> interface re-verification (3 × 6,000) **+18,000**, regression testing **+14,000**, documentation
> and training **+9,000** — to a total of **131,560**. The quoted bar is shaded and labelled
> **"30.4 % of the true cost — the only figure on the change form"**. A dashed horizontal line at
> the **25,000** delegation threshold shows that a change quoted below it can carry a true cost of
> **50,560**. Source: PCI original. Alt text: a waterfall rising from a small quoted cost bar
> through five additional cost components to a total more than three times the starting figure.

**Assessment discipline.** Three rules make assessment reliable rather than performative. The
assessment is done by the people who will do the work, not by the change board estimating on their
behalf. It is **timeboxed and resourced** — an assessment budget exists, because otherwise assessment
competes with delivery and loses, which is the actual reason changes are decided on quoted costs. And
its **assumptions are stated**, particularly the schedule assumption, since whether two weeks lands
on the critical path or on float is the difference between 28,560 and nothing, and it is a fact about
the schedule rather than a matter of opinion (Domain 6's float).

### 4.4.3 The change board and the decision log

**The change board.** A body with authority to approve baseline changes within stated limits and to
escalate beyond them. Its membership must include the authority to commit cost, the authority to
commit schedule, and the technical authority to judge feasibility — and the last of those is the one
usually missing, which is how changes are approved that cannot be implemented as described.

**Cadence and latency.** Domain 3's arithmetic applies unchanged: a change board meeting fortnightly
with a one-week paper deadline imposes `E[wait] = 2/2 + 1 =` **2 weeks** on every escalated change.
Where change volume is high, that latency is a delivery cost and is reduced the same way — a shorter
paper deadline first, then a written-resolution route for changes below a stated impact.

**The decision log, and the entry that must exist.** Every change decision — approved, rejected,
deferred or withdrawn — generates an entry: reference, date, decision-maker by name, the decision,
the assessed impact across all dimensions, the basis, and the baseline effect. **Rejections matter as
much as approvals**, and are the entries most often missing: a rejected change that leaves no trace
returns in three months as a new request, is assessed again at full cost, and may be approved by a
different body in ignorance of the first decision. The log is what prevents that, and it is the same
log as Domain 3's, not a second one — one register of record for every decision that moves the
baseline, whoever took it and at whatever value, which was Case study B's correction.

**Worked example 4.4.3 — pricing the rejection entries Meridian did not write.**

1. **Setup.** Of the **51** requests WE 4.4.1 ruled as changes, **14** were rejected or withdrawn. The
   decision log records **5** of them with a rationale; **9** left no trace. Within the same year, **4**
   of those 9 returned as fresh requests. A full impact assessment consumes **5 person-days** at the blended
   **USD 880** — **USD 4,400**. One of the four returning requests was approved by the programme board
   in ignorance of the earlier refusal, at an assessed impact of **USD 63,000**. Writing a rejection
   entry — reference, date, decision-maker, reason, and the assessment already performed — takes about a
   quarter of an hour, **USD 27.50** at the same rate.
2. **Formula.** Duplicated assessment = recurrences × assessment cost. Total loss = duplicated
   assessment + the assessed impact of any decision reversed in ignorance. Compare with the cost of
   recording every rejection, and compare the **mean** loss per recurrence with the **modal** one.
3. **Substitution.** `4 × 4,400`; `+ 63,000`; against `14 × 27.50`.
4. **Result.** Rejections recorded **35.71 %**; recurrence among the untraced **44.44 %**. Duplicated
   assessment **USD 17,600**; one decision reversed in ignorance **USD 63,000**; total **USD 80,600**,
   or **USD 20,150** per recurrence. Recording all 14 rejections would have cost **USD 385**.
5. **Interpretation.** The distribution matters more than the total, and a leader should present both
   figures or neither. Three of the four recurrences cost only the wasted re-assessment — **USD 4,400**
   each, irritating and survivable. The fourth cost **63,000**, so the mean of 20,150 is **4.58 times**
   the modal case. Strip the reversed decision and the year's loss is 17,600 against 385 of recording
   cost, a ratio of **45.71**; include it and the ratio is **209.35**. Quote only the second and a
   competent reviewer will discount the whole argument as anecdote; quote only the first and you have
   understated the exposure by the one case that mattered. **The honest statement is that the modal cost
   of an untraced rejection is a wasted assessment and its tail cost is a reversed decision, and the
   control is bought for the tail.**

   The tail case is also the only part the log uniquely prevents, and the mechanism is worth being
   precise about. Re-assessment waste can be attacked other ways — duplicate detection on request text,
   the third item in the AI note below, catches much of it. A **decision reversed in ignorance** cannot
   be caught that way: the second body was not careless, it simply had no record that the question had
   been asked and answered. That makes the rejection entry not an administrative courtesy but the
   instrument that makes a "no" **durable** — and a governance structure in which "no" does not persist
   is one where any sufficiently persistent requester eventually obtains a yes, from whichever authority
   happens to be sitting, at no point through anyone's misconduct.

   Note the interaction with KA 4.4.2's assessment budget, because it converts this into an argument the
   budget holder cares about. Assessment is chronically under-resourced precisely because it precedes the
   decision to spend, and **17,600 of the year's assessment budget was spent twice on questions already
   answered** — four assessments of capacity that the same money could have applied to new requests. The
   rejection entry is therefore the cheapest available increase in assessment capacity, and it is worth
   putting to the budget holder in exactly those terms rather than as a compliance point.

   The caution is about what the entry must contain. A rejection entry must carry the **assessment**, not
   merely the outcome: an entry reading "rejected — not affordable" saves nothing, because a returning
   request still has to be assessed from scratch to establish what it now costs. The entry that saves the
   4,400 records the assessed impact, its basis and its date, so that a recurrence becomes a question
   about *what has changed since* — usually answerable in an hour, and occasionally answerable with a
   legitimate "the circumstances have changed and the answer is now yes", which is the outcome the log
   exists to make possible rather than to prevent.

### AI in this KA

**Where it earns its place.** Assembling the *components* of an impact assessment — identifying which
interfaces an affected component touches (from the interface register), which completed work packages
are implicated, which test suites regress, which documents and training materials cite the changed
behaviour. This is traversal of structured relationships, it is where human assessment is
systematically incomplete, and it is checkable. Screening a request against the baseline to propose
whether it is a change, a clarification or a defect — a proposal, for human ruling. Detecting
duplicate or previously rejected requests in the log, which is exactly the failure the previous
paragraph describes. Computing the assessed total against the delegation schedule and flagging where
quoted and assessed costs fall on opposite sides of a threshold.

**Where it must not go.** Approving or rejecting a change. Estimating the direct cost or the schedule
impact, which belongs to the people who will do the work and to the schedule respectively — a model
asked for a schedule impact will produce a plausible number with no critical-path basis, and that
number is worth 28,560 in the example above. And no classification of a defect as a change without
human ruling, since that determination has commercial consequences.

**Verification, concretely.** Every AI-identified affected interface, work package and test suite is
confirmed by the accountable owner before it enters the assessment; the schedule impact is taken from
the schedule, by the planner, with the float position stated; the arithmetic of the total is
reproduced by hand; and the decision record names the human decision-maker, with the AI's
contribution recorded as assessment input rather than as authorship.

### Key terms — KA 4.4

| Term | Meaning |
|---|---|
| **Integrated change control** | Assessing and authorising a change across scope, schedule and cost together. |
| **Change vs clarification vs defect** | Baseline moves · baseline already covered it · work fails to meet the baseline. |
| **Screening ruling** | The recorded classification: the baseline clause relied on, the outcome, and a named technical authority — the only artefact that makes a classification contestable later. |
| **Emergency-route share** | Emergency changes ÷ total requests — a latency diagnostic, so set a review trigger on it rather than a target. |
| **Assessed total impact** | Direct + schedule + rework + interface re-verification + regression + documentation + risk + benefit. |
| **Quoted direct cost** | The figure on the form — typically a minority of the true cost, and the wrong basis for a threshold. |
| **Emergency change route** | A named authority, a short deadline and mandatory retrospective ratification. |
| **Change board** | The body authorised to approve baseline changes within limits, including a technical authority. |
| **Rejection entry** | The log record of a change not approved — the entry most often missing and the reason requests recur. |

### Sample MCQs — KA 4.4

**MCQ 4.4-A `[4.4.2 · Application]`** A change is quoted at 40,000 direct. Assessment adds 2 weeks
of critical-path impact at 14,280 per week, 22,000 of rework, 3 interfaces at 6,000 each, 14,000 of
regression testing and 9,000 of documentation. The assessed total is:
- A. USD 40,000
- B. USD 103,000
- C. USD 131,560 ✅
- D. USD 109,560

*Rationale:* `40,000 + 28,560 + 22,000 + 18,000 + 14,000 + 9,000 = 131,560` (4.4.2). B omits the
schedule impact; D omits the rework.

**MCQ 4.4-B `[4.4.2 · Evaluation]`** A programme's delegation threshold is 25,000 on quoted direct
cost. A change quoted at 22,000 carries 2 weeks of critical-path impact at 14,280 per week. The
structural defect is that:
- A. the threshold is too low
- B. a change with an assessed impact of at least 50,560 is decided without escalation, because the
  threshold reads on the quoted figure ✅
- C. the change should be split into smaller changes
- D. the cost of delay should be excluded from change assessment

*Rationale:* The threshold's *basis* is the defect, not its level (4.4.2); the remedy is that it reads
on assessed total impact.

**MCQ 4.4-C `[4.4.1 · Analysis]`** Classifying a defect as a change results in:
- A. the baseline correctly reflecting the work
- B. the supplier being paid twice for the same obligation ✅
- C. faster resolution at no cost
- D. an unnecessary escalation

*Rationale:* A defect is work already owed under the baseline; treating it as a change adds budget
for it (4.4.1). The mirror error — a change classified as a clarification — grows scope silently.

**MCQ 4.4-D `[4.4.3 · Comprehension]`** The change-log entry type most often missing and most
consequential is:
- A. the approval entry
- B. the rejection entry ✅
- C. the implementation confirmation
- D. the assessed cost

*Rationale:* An untraced rejection returns as a new request, is re-assessed at full cost, and may be
approved by a different body in ignorance of the first decision (4.4.3).

**MCQ 4.4-E `[4.4.2 · Analysis]`** Why is impact assessment systematically under-resourced?
- A. assessors lack the skill
- B. it costs money before any decision has been taken to spend money, so it competes with delivery
  and loses ✅
- C. change boards prefer quoted figures
- D. assessment adds no value to rejected changes

*Rationale:* The structural cause is that assessment is unfunded work preceding a decision (4.4.1,
4.4.2); the remedy is an explicit assessment budget.

**MCQ 4.4-F `[4.4.1 · Application]`** A re-screen finds 5 defects paid as changes at an average 6,800,
and 3 changes ruled as clarifications carrying 29,200 of direct work plus 0.5 weeks of critical-path
impact at 14,280 per week. The gross misclassification cost is:
- A. USD 34,000
- B. USD 36,340
- C. USD 70,340 ✅
- D. USD 2,340

*Rationale:* `34,000 + 36,340 = 70,340` (4.4.1). A and B are the two classes taken alone; D is the net,
which understates the gross by twice the smaller class and is the specific error the example exists to
name.

**MCQ 4.4-G `[4.4.3 · Evaluation]`** Four untraced rejections recurred: three cost only a duplicated
4,400 assessment, and one was approved by another body at an assessed impact of 63,000. The defensible
way to present the case for recording rejections is to:
- A. quote the 209.35 ratio of total loss to recording cost
- B. quote the 45.71 ratio excluding the reversed decision
- C. quote both, stating that the modal cost is a wasted assessment and the tail cost is a reversed
  decision ✅
- D. quote the mean loss of 20,150 per recurrence as typical

*Rationale:* The mean is 4.58 times the modal case, so either ratio alone misleads — one as anecdote,
the other by omitting the case that mattered (4.4.3). D presents a tail-dominated mean as typical.

### Self-check — KA 4.4

1. *Name the three screening outcomes and the cost of confusing two of them.* — Change,
   clarification, defect; a defect treated as a change pays twice, a change treated as a
   clarification grows scope silently.
2. *Which screening error needs the tighter control, and why?* — A change ruled as a clarification,
   because overpayment surfaces in the cost report while silent scope growth has no downstream detector.
3. *Why is a threshold on quoted direct cost not a control?* — Because the quoted figure is typically
   a minority of the assessed impact, so changes above the real threshold are decided below it.
4. *What must a rejection entry contain to be worth writing?* — The assessed impact, its basis and its
   date, so that a recurrence is a question about what has changed since.
5. *What makes a schedule impact assessment a fact rather than an opinion?* — Whether the impact
   lands on the critical path or on float is a property of the schedule, and the float position must
   be stated.

---

## Advanced topics — Domain 4

### 4.A.1 Integration across organisational boundaries

Where delivery spans organisations — client and supplier, joint venture partners, a programme and its
constituent projects — integration acquires a failure mode that no internal design has: **each party
integrates to its own baseline.** Version 4 of an interface specification on one side and version 5
on the other is not an argument about competence; it is the predictable consequence of two
configuration registers with no reconciliation, and it is discovered at integration testing where it
is most expensive.

Four provisions address it, and they belong in the contract rather than a plan (Domain 7; PFL-AI
Domain 11): a **single interface register** identified as the register of record, with named owners on
both sides of every interface; a **joint change process** for anything crossing the boundary, with
one decision log rather than two reconciled ones; **matched configuration identification**, so that
both parties refer to the same item by the same identifier at the same version; and **joint
verification** of interfaces at a scheduled point, with resource committed on both sides. The
recurring commercial mistake is to treat integration as each party's own responsibility up to its
own boundary, which leaves the join itself unowned — and the join is where the failure happens.

### 4.A.2 Architectural decisions as governance decisions

Some integration choices are irreversible in practice long before they are irreversible in
principle: the integration pattern of KA 4.2.3, the data model, the identity provider, a
platform dependency. They share three properties that make them governance matters rather than
technical ones — the cost of reversal rises steeply with time, the consequences fall outside the
project's boundary and often outside its life, and they are typically taken by people whose
delegated authority is defined in money, of which these decisions frequently involve very little.

That last point is the whole issue, and it is Domain 3's delegation schedule read on the wrong
dimension: a decision costing 40,000 to take and 2,000,000 to reverse is not a 40,000 decision, and a
schedule reading only on value cannot see it. The countermeasures are specific: an **architectural
decision record** for each such choice, with its alternatives, rationale and reversal cost stated;
those records reviewed by the governance body on the **reversibility** dimension rather than the value
one; and a standing question in gate criteria — *which decisions taken since the last gate are
expensive to reverse, and who took them?* Case study B of Domain 3 is exactly this failure with a
control attached rather than an architecture attached.

### 4.A.3 The reviewer's integration eye

Invariants to test on any delivery architecture, each cheap and each diagnostic:

The plan set has been **read against itself before baseline approval**, with the findings priced, and
the checks that failed are the ones spanning two owners. The **rolling-wave horizon is at least as long
as the longest commitment lead time** inside it, or the exception is labelled a partial horizon. The
WBS satisfies the **hundred-per-cent rule at every level**, and the value most likely to be missing is
enabling change. Every product traces to work and every work package to a product — and each
cross-match exception carries a **recorded ruling with a named ruler**, since two thirds of them will
be legitimate and an unruled exception list is indistinguishable from an unrun check. The **interface
register is built from need**, has a named owner on each side of every entry, and every entry has a
**scheduled** verification activity with resource. The **time-phased cost baseline matches the
schedule's current dates** — the single most informative check in the domain, the first thing abandoned
under pressure, and the one whose failure leaves every cost measure correct. The change log's approved
changes **sum to the difference** between the original and current baselines, and any residual is
**decomposed before it is netted**; there are no baseline movements without a change reference and no
approved changes without a baseline effect. Screening outcomes are **recorded rulings** citing the
baseline provision relied on. Rejections appear in the log, carrying their assessment rather than only
their outcome. The delegation threshold reads on **assessed total impact**. A **cumulative test** exists
and its threshold and period are derived from the observed change rate — and it is present *as well as*
the reconciliation above, the two being disjoint controls (4.3.3b). Configuration status accounting
has been audited against deployment within a stated period, and the item-level tolerance is **derived
from an interface-level target** rather than chosen. Re-baselines carry the original's authority, a
substantive reason, and the original still visible in reporting. And every architectural decision with a
reversal cost materially above its decision cost has a record and a named decision-maker.

---

## Industry variations — Domain 4

- **Construction and infrastructure.** Interfaces are physical and contractual simultaneously, and
  interface management is a named discipline with its own register and manager; configuration
  management extends to as-built records, whose divergence from design is the standard handover
  dispute (Domain 16).
- **Software and digital.** Configuration management is largely automated through version control and
  build pipelines, which solves identification and control while leaving the *interface agreement*
  and its ownership as human work — and integration failure migrates there accordingly. The pairwise
  arithmetic of WE 4.3.2b explains why the automation is worth so much: it lifts item conformance to
  where the exponent stops biting, so the interface-level rate follows without anyone having to
  compute it.
- **Regulated manufacturing (pharmaceutical, medical devices, aerospace).** Configuration and change
  control are regulatory obligations with validated processes; change assessment must include
  re-validation cost, which frequently exceeds every other component and reverses the ranking of
  WE 4.4.2.
- **Public-sector programmes.** Baselines are published commitments, so re-baselining is externally
  visible and politically costly, which makes drift the preferred failure mode — and therefore makes
  the cumulative test of KA 4.3.3 the highest-value control available.
- **Multi-party ventures.** The reconciliation provisions of 4.A.1 are the domain's principal content;
  a single interface register named in the agreement is worth more than any internal process either
  party runs. And the exponent `k` in `p^k` is the number of parties, so a three-party interface is
  materially harder to keep verifiable than a two-party one at the same register quality — 65.98 %
  against 75.79 % on Meridian's figures — which is a reason to reduce the number of parties to a join
  before it is a reason to audit harder.
- **Healthcare.** Clinical safety cases are configuration-controlled artefacts bound to specific
  system versions, so a version change is a safety-case change — which makes Meridian's 5
  mis-recorded configuration items a clinical-governance finding, not merely an administrative one.

---

## Case study — Domain 4: the interface nobody owned (health, Meridian)

**Situation.** Meridian's integration testing began four weeks late and took eleven weeks against a
planned five. The programme reported "integration complexity" and requested a schedule change. The
assurance review that followed found something more specific.

**What the review found.** Of the **66** possible interfaces among the 12 components, **31** were
genuinely required and had been specified. Of those 31, **9** had no named owner on one side — in
every case the side outside the programme's own team: the national reporting gateway, the identity
provider, the legacy records supplier. Those nine accounted for **all** of the four-week start delay
and **four of the six weeks** of overrun, because each had to be renegotiated at the point of
verification with someone who had never agreed to it. Separately, **5** configuration items were
verified against register entries that did not match what was deployed (WE 4.3.2's most serious
class), which produced two defects that each passed component testing.

**What the arithmetic had already said.** The programme had planned interface work at 12 interfaces —
the layered count — while operating a partial mesh of 31. The plan was not optimistic; it was
counting the wrong thing. At 18,000 an interface, 31 interfaces is **558,000** against a planned
**216,000**, and the 342,000 difference had been absorbed as unplanned effort and schedule.

**What the unowned interfaces cost, per interface.** Eight of the elapsed weeks lost — the four-week
late start and four of the six weeks of overrun — were attributed to the nine unowned interfaces, which
were **29.03 %** of the 31 required. At the programme's cost of delay that is `8 × 14,280 =`
**USD 114,240**, or **USD 12,693** per unowned interface, and none of it bought anything: the nine
agreements had to be negotiated either way, and their negotiation effort already sat inside the 18,000
unit cost. What the programme paid for was negotiating them at the moment of *verification* rather than
at the moment of *specification*. The register field that would have prevented it — *owner on side B,
named* — costs nothing to add and nothing to maintain, which makes this the cheapest control in the
domain and the one most often absent. The five mis-recorded configuration items compound it in exactly
the way WE 4.3.2b predicts: with item conformance at 87.06 %, roughly **7.50** of the 31 interfaces were
expected to be unverifiable on register grounds alone, and the two defects that passed component testing
came out of that population rather than out of bad luck.

**What changed.** An interface register built from **need** rather than from either formula, with a
named owner on **each** side of every entry and a scheduled verification activity for each — which
made the nine unowned interfaces visible as nine missing agreements rather than as future
complexity. Verification activities entered the schedule with duration and resource, which is what
Domain 6 needed in order to show the float they consumed. And the configuration register was audited
against deployment before integration rather than after.

**What the domain teaches here.** An unowned interface is not a risk; it is a scheduled failure with
an unknown date. And the interface arithmetic is only useful applied to the *required* count: 12 was
the architecture's promise, 66 was the theoretical maximum, and 31 was the number that had to be
managed. Planning to the promise is the error.

## Case study B — Domain 4: the baseline that could no longer answer the question (financial services)

**Situation.** A payments programme (the same organisation as Domain 3's Case study B, two years
later) reached its third baseline. Reported performance was `CPI` 0.99 and `SPI` 1.00 — on baseline
three. An audit asked a simpler question: what is performance against the **original approved
commitment**? Nobody could produce the answer within a fortnight.

**Why not.** Each re-baseline had been implemented by **replacement** rather than accumulation. The
new baseline was loaded, the old one archived as a file, and the change log referenced changes to
"the baseline" without stating which. Intermediate states were not reconstructable, so the
original-to-current reconciliation — approved changes summing to the difference between original and
current — could not be performed at all. Two of the three re-baselines had also been approved by the
programme board rather than by the investment committee that approved the original, and neither
recorded a substantive change in circumstance; both cited accumulated variance, which is the reason
that does not qualify.

**How it resolved.** The reconstruction took six weeks of a two-person effort against the change log,
the archived files and the finance ledger, and established performance against the original as `CPI`
**0.87** — a **14.9 %** cost overrun that three successive baselines had made invisible without ever
stating an untruth. Corrections: baselines thereafter maintained by **accumulation** with every change
referenced to its authority; the original baseline reported alongside the current one permanently;
re-baselining reserved to the authority that approved the original, with a recorded substantive
reason; and the original-to-current reconciliation performed and published every reporting period.

**What the domain teaches here.** A baseline exists to answer one question — how is this project
performing against what was approved? — and a baseline maintained by replacement cannot answer it.
Neither `CPI` 0.99 nor `CPI` 0.87 was a false statement; the first was true of a document and the
second of the commitment, and only one of them was what the organisation had asked.

---

## Executive perspective — Domain 4

What a programme director cannot delegate in this domain:

- **The authority statement in your own charter.** If you cannot state what you may decide alone
  without reading the document, you do not have it (4.1.1).
- **The interface count and its ownership.** Not the theoretical count and not the architecture's
  promise — the *required* count, with a named owner on each side and a scheduled verification for
  every entry. Unowned interfaces are scheduled failures with unknown dates (4.2.3, Case study A).
- **That the plan set has been read against itself before you sign the baseline.** Meridian's audit
  found 306,992 of unauthorised cost and schedule in three days for 5,280 — more than a year of
  authorised drift, before any work had started (4.1.2).
- **That the time-phased cost baseline still matches the schedule.** This is one check, it is the
  first thing abandoned under pressure, and without it earned value measures nothing (4.3.1). Be aware
  that its failure leaves `CPI` and the CPI-only forecasts exactly correct, so a clean cost review is
  not evidence that it has been done.
- **Reconciling gross, never net.** A residual of 32,200 concealed a gross error of 122,000, and the
  two classes had opposite remedies — one a posting, one an investigation (4.3.3b).
- **The tolerance on your configuration register, derived rather than chosen.** 87 % of items conforming
  is 76 % of two-sided interfaces verifiable; a 95 % interface target requires 97.47 % of items
  (4.3.2b).
- **The basis of your change threshold.** Assessed total impact, never quoted direct cost — a
  one-sentence change worth more than most process improvements (4.4.2).
- **A cumulative test derived from your own change log.** Meridian's baseline moved 12.1 % with no
  decision, and a 100,000/90-day rule would not have caught it (4.3.3).
- **Reversal cost as a governance dimension.** A decision costing 40,000 to take and 2,000,000 to
  reverse is not a 40,000 decision, and a value-only delegation schedule cannot see it (4.A.2).
- **Performance against the *original* baseline.** Keep it visible and reconcilable. It is the only
  question the funding organisation actually asked (4.3.3, Case study B).

---

## Calculation exercises — Domain 4

**Exercise 4.1** A programme integrates 9 components. One interface costs 24,000 to specify, build,
test and document; an integration layer would cost 200,000. Compute the mesh and layered interface
counts and costs, the saving, the breakeven layer cost, and the marginal cost of a tenth component
under each architecture.
*Solution.* Mesh `9 × 8/2 =` **36** interfaces at 24,000 = **USD 864,000**. Layered **9** interfaces
= 216,000 plus 200,000 = **USD 416,000**. Saving **USD 448,000**. The layer is worth building while
it costs below `864,000 − 216,000 =` **USD 648,000**. A tenth component adds `9 × 24,000 =`
**216,000** on a mesh and **24,000** layered. Common error: counting `n²` or `n(n−1)`, which double-
counts each pair.

**Exercise 4.2** A parent WBS element is approved at 6,500,000. Its five children are estimated at
1,900,000, 1,250,000, 880,000, 1,640,000 and 410,000. A review then identifies an omitted element —
commissioning and handover — estimated at 690,000. Compute the unallocated amount and its share of
the parent, then the shortfall once the omission is restored.
*Solution.* Children sum to **6,080,000**, leaving **420,000** unallocated — **6.46 %** of the
parent. Restoring the omitted 690,000 gives an honest total of **6,770,000**, which exceeds the
approved figure by **270,000**, or **4.15 %**. Common error: reading the 420,000 as contingency, which
locks in the omission and leaves nothing to absorb it.

**Exercise 4.3** A change is quoted at 75,000 direct. Assessment finds 3 weeks of critical-path
impact at a cost of delay of 9,500 per week, 34,000 of rework, 4 affected interfaces at 7,500 each,
19,000 of regression testing and 11,000 of documentation. Compute the assessed total, the ratio to
the quoted figure, and the quoted figure as a share of the true cost.
*Solution.* `75,000 + 28,500 + 34,000 + 30,000 + 19,000 + 11,000 =` **USD 197,500** — **2.633
times** the quoted figure, of which the quote is **38.0 %**. Common error: omitting the schedule
impact, which requires the float position from the schedule and is therefore the component most often
left out on the grounds that it is "not yet known".

**Exercise 4.4** Over a year a project approved 48 changes averaging 5,200 direct cost, 18 of which
carried 0.25 weeks of critical-path impact each. Cost of delay is 9,500 per week; the baseline is
3,200,000. Compute total drift, its share of the baseline, each change's individual share, and the
90-day cumulative threshold that would have caught it.
*Solution.* Direct `48 × 5,200 =` **249,600**; schedule `18 × 0.25 = 4.5` weeks at 9,500 =
**42,750**; total **USD 292,350** — **9.14 %** of the baseline, while each change was **0.16 %** of
it. A 90-day window holds about `48/4 = 12` changes, aggregating to **62,400**, so a cumulative
threshold at or below **62,400** on a 90-day window would trip; a 100,000 threshold would not.
Common error: setting the cumulative threshold at a round number without reference to the observed
change rate, which produces a control with the appearance of function and none of it.

**Exercise 4.5** A configuration register holds 500 controlled items, of which 468 conform. The
programme has 44 required interfaces, 6 of them three-party and the remainder two-sided. Assuming
independent defects, compute item conformance, two-party and three-party relationship conformance, the
expected number of at-risk interfaces, and the item conformance that a 98 % two-party target would
require.
*Solution.* Item conformance `468/500 =` **93.60 %**. Two-party `0.936² =` **87.6096 %**; three-party
`0.936³ =` **82.0026 %**. Expected at-risk: two-party `38 × (1 − 0.876096) =` **4.7084**, three-party
`6 × (1 − 0.820026) =` **1.0798**, total **5.7882** interfaces. For a 98 % two-party target,
`p = 0.98^(1/2) =` **98.99 %**, i.e. at most **5** non-conforming items against the 32 currently
non-conforming. Common error: applying the item failure rate straight to interfaces — `44 × 0.064 =`
**2.8160** — which understates the exposure by **51.35 %** because it ignores the exponent.

**Exercise 4.6** A programme's original approved baseline is 8,600,000. Its change log holds 27 approved
changes totalling 742,300. The cost tool reports a current baseline of 9,318,600. Investigation finds
61,400 of approved changes with no recorded baseline effect and 37,700 of baseline movement with no
change reference. Compute the expected current baseline, the net residual, the gross error, and the ratio
between them.
*Solution.* Expected current `8,600,000 + 742,300 =` **9,342,300**. Net residual
`9,318,600 − 9,342,300 =` **(23,700)** — only **0.2756 %** of the original. Check:
`37,700 − 61,400 = (23,700)` ✓. Gross error `61,400 + 37,700 =` **99,100** — **13.35 %** of the approved
changes and **4.18 times** the net. Common error: treating a residual of a quarter of one per cent as
rounding and closing the reconciliation, which discards a finding four times its size; note also that the
residual here is *negative*, which does not make it benign — it means the log is ahead of the tool rather
than behind it, and the two classes still require different remedies.

**Exercise 4.7** Of 84 change requests, screening ruled 46 changes, 24 clarifications and 14 defects. A
re-screen finds that 7 of the "changes" were defects, at an average approved value of 9,400, and that 4
of the "clarifications" were changes, with direct work of 12,000, 8,500, 6,200 and 15,300, one carrying
0.75 weeks of critical-path impact at a cost of delay of 9,500 per week. Compute the error rate, both
error classes, the gross total, the cost per error, and the net effect on the recorded change total.
*Solution.* Errors **11** of 84 — **13.10 %**. Defects paid as changes `7 × 9,400 =` **65,800**. Changes
taken as clarifications `42,000 + 0.75 × 9,500 =` **49,125**. Gross total **USD 114,925**, or
**USD 10,447.73** per error. Net effect on the recorded change total `49,125 − 65,800 =` **(16,675)** —
**6.8921 times** smaller than the gross, and understating it by twice the smaller class
(`2 × 49,125 = 98,250`). Common error: reporting the net, which here even has the wrong sign for the
narrative — the log overstates the change total while scope has silently grown.

**Exercise 4.8** At a status date a project reports PV 6,300,000, EV 5,900,000 and AC 6,150,000 against
a BAC of 12,000,000. A change approved two months earlier deferred 450,000 of planned value beyond the
status date and the `PV` curve was never re-phased. Compute reported and true `SPI` and `SV`, `CPI`, and
the composite `EAC` on each basis.
*Solution.* Reported `SPI = 5,900,000/6,300,000 =` **0.9365**, `SV =` **(400,000)**. True PV
`6,300,000 − 450,000 = 5,850,000`, so true `SPI =` **1.0085** and `SV =` **50,000** — the project is
marginally *ahead* of the authorised schedule while reporting 0.94. `CPI = 5,900,000/6,150,000 =`
**0.9593** on both bases, and the CPI-only forecasts are unchanged at `BAC + AC − EV =` **12,250,000** and
`BAC/CPI =` **12,508,475**. Composite `EAC = AC + (BAC − EV)/(CPI × SPI)`: reported
**USD 12,939,558**, true **USD 12,454,589** — an overstatement of **USD 484,968**, which is exactly the
sensitivity `(BAC − EV)/(CPI × EV) =` **1.0777** multiplied by the 450,000 of mis-phasing. Common error:
concluding that a stale baseline always flatters. Deferral makes performance look *worse* than it is, and
the response — a recovery plan on a project that does not need one — costs real money to correct a
reporting defect.

---

## Practitioner's toolkit — Domain 4

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 4.T.1 — Charter on one page

Fields, in this order, and nothing else: purpose and measurable objectives · named sponsor · named
project leader · **authority bounds** (spend, scope, resource, commitment — each a number or a
sentence, not a paragraph) · scope boundary **including exclusions** · key deliverables and
acceptance basis · fixed constraints, distinguished from planned dates · budget authority ·
material risks and assumptions · governance bodies and thresholds by reference · approval signature
and date. If it exceeds two pages, the plan has begun; move the excess.

### Toolkit 4.T.2 — Interface register

One row per **required** interface (built from need, not from `n(n−1)/2`): reference · the two
components or parties · **owner on side A** and **owner on side B**, both named · content exchanged ·
format and protocol · direction · timing and frequency · error and exception behaviour · agreed
specification **version** · verification activity, with its schedule reference, duration and resource
· verification status and date. The two owner columns and the schedule reference are the fields that
carry the value; an interface register without them is an inventory. Report monthly: entries with a
missing owner, entries with no scheduled verification, and entries verified against a superseded
version. Add one derived line to the same report — **expected at-risk entries**, `count × (1 − p^k)`
from the configuration register's current item conformance `p` and each entry's party count `k` — which
converts the configuration audit into a number the integration plan can act on (WE 4.3.2b).

### Toolkit 4.T.3 — Integrated change request and baseline reconciliation

*Request side.* Reference · date raised · raiser · description and reason · **screening outcome**
(change / clarification / defect, with the ruling recorded) · then the assessment, one line each:
direct cost · schedule impact **with the float position stated** · rework · interfaces affected and
re-verification cost · regression testing · documentation, training and communication · risk profile
change · benefit change. Then: **assessed total impact** · authority required **on the assessed
total** · decision, decision-maker by name and date · baseline effect on scope, schedule and cost ·
implementation and verification confirmation.

*Reconciliation side*, run every reporting period and published: original baseline · plus the sum of
approved changes · equals current baseline (**the three must reconcile exactly**, each figure quoted
from its own system of record rather than derived from another) · approved changes with no recorded
baseline effect · baseline movements with no change reference · **the gross residual, stated before any
netting, with each class shown separately** · rolling-window cumulative total against the derived
threshold · movement in the time-phased `PV` curve reconciled to the approved changes' schedule effects
· and current performance reported against **both** the current and the original baseline.

---

## Exam preparation — Domain 4

**What is assessed.** Charter content and the primacy of the authority statement; the plan of plans
and its consistency checks; tailoring as a recorded decision and its limits; WBS construction and the
hundred-per-cent rule; work packages and control accounts; product versus work breakdown and where
each belongs; **interface counting and integration architecture economics**; interface agreements and
ownership; the integrated three-dimensional baseline and its invariants; configuration management's
four functions and the audit finding classes; baseline maintenance by accumulation, and re-baselining
discipline; **baseline drift and cumulative tests**; the change flow and the change/clarification/
defect distinction; **assessed impact versus quoted cost**; the change board and the decision log
including rejections.

**The calculations to be able to do under time pressure.** Mesh and layered interface counts
(`n(n−1)/2` and `n`), their costs, the breakeven layer cost, and the marginal cost of one more
component under each. The hundred-per-cent test and the shortfall once an omission is restored.
Product and work-package coverage from a cross-match, and the classified residue after ruling. Plan-set
audit exposure, converting unfunded work at a person-day rate and unscheduled elapsed time at the cost
of delay, counting only the critical-path share. `SPI`, `SV`, `CPI` and the three `EAC` forms on both a
stale and a re-phased `PV`, plus the composite forecast's sensitivity `(BAC − EV)/(CPI × EV)` per unit of
mis-phased planned value. Pairwise configuration integrity `p^k`, expected at-risk relationships, and the
inversion `p = q^(1/k)` that turns an interface target into an item tolerance. The original-to-current
reconciliation, its residual, and the decomposition of that residual into its two opposite-signed
classes. Assessed total impact from its components, the ratio to the quoted figure, and the quoted share.
Baseline drift from a change count, average cost, affected count, weeks and cost of delay — and the
cumulative-window threshold that would catch it. Configuration defect rates by class.

**The traps.** Counting `n²` or `n(n−1)` for interfaces · planning interface work at the architecture's
promised count rather than the required count (Case study A) · reading unallocated budget as
contingency (Exercise 4.2) · acting on raw cross-match exception counts without ruling on each one
(WE 4.2.2) · assuming a stale `PV` curve always flatters, when deferral makes performance look worse
(Exercise 4.8) · applying an item-level configuration rate to interfaces instead of `p^k`
(Exercise 4.5) · choosing a configuration tolerance at item level rather than deriving it from an
interface target · **netting a reconciliation residual instead of decomposing it** (Exercises 4.6, 4.7,
WE 4.3.3b, WE 4.4.1) · omitting schedule impact from a change assessment because the float
position "is not yet known" (Exercise 4.3) · applying a delegation threshold to quoted direct cost
(4.4.2) · setting a cumulative threshold at a round number rather than from the observed change rate
(Exercise 4.4) · treating a defect as a change, or a change as a clarification · treating a rising
emergency-route share as indiscipline rather than as a latency symptom · leaving rejections
out of the decision log, or logging an outcome without the assessment · re-baselining by replacement
(Case study B) · applying configuration control to activities · setting a rolling-wave horizon shorter
than the longest commitment lead time inside it.

**How the domain connects.** Domain 1 supplies the accountability principle and the cost of delay
every calculation here is priced at. Domain 2 supplies the business case whose benefits the baseline
must still deliver, and the enabling-change column that WE 4.2.1 finds missing from the WBS. Domain 3
supplies the delegation schedule this domain corrects the basis of, the latency arithmetic the change
board inherits, and the decision log the change log is part of. Part Two then works the baseline:
Domain 6 schedules the WBS and supplies the float that change assessment needs, Domain 7 measures
against the time-phased baseline this domain insists on maintaining, Domain 8 attaches uncertainty to
it, and Domain 16 hands over the configuration record. PFL-AI Domain 11 handles the contractual face
of cross-boundary integration.

---

## Domain 4 summary
Domains 1 to 3 produced accountability, a justified choice and a working decision structure, and none
of them produced a project. Integration does — and integration fails in a characteristic way: the
parts are managed and the joins are not.

The charter authorises and bounds; its irreplaceable content is the statement of what the leader may
decide alone, and a charter that has become a plan has lost it. The plan of plans is integrated by a
short list of consistency checks that only appear when the plans are read against one another —
including the one that makes a programme report last month's position every month for its whole life.
Those checks are arithmetic, and running them on Meridian's eleven subsidiary plans found
**USD 306,992** of unfunded cost and unscheduled time — **12.79 %** of the baseline, more than a full
year of authorised drift — for an audit costing **USD 5,280**, three days before any work began. The
checks that failed all spanned two owners; the one that passed had a single owner on both sides, which is
the general shape of the finding. And the same audit exposed a horizon two weeks shorter than the
programme's longest procurement lead time — a plan that could not support its own commitments, which no
diligence inside the wave could have fixed.

The WBS obeys the hundred-per-cent rule, which is arithmetic and therefore auditable: Meridian's five
level-2 elements sum to **2,332,000** against an approved **2,400,000**, and the missing element is
**clinician training and enabling change** at **214,000** — the same column Domain 2's benefits map
omitted, leaving an honest baseline **6.1 %** above the approved figure and an NPV of **+1,186,898**
that would still have been approved. The omission bought nothing.

Interfaces are where integration effort lives, and they grow combinatorially while components grow
linearly: Meridian's 12 components admit **66** point-to-point interfaces costing **USD 1,188,000**,
against **12** plus a layer at **USD 536,000** — a **54.9 %** saving, worth building while the layer
costs below **972,000**, and decisive on the margin, where a thirteenth component costs **216,000**
meshed and **18,000** layered. Case study A supplies the professional correction: the number to plan
against is neither 66 nor 12 but the **31** genuinely required, of which **9** had no owner on the far
side and accounted for the entire integration overrun.

Products and work verify each other, and the verification is arithmetic too — but only after each
exception has been ruled on. Meridian's 148 products against 172 work packages produced **21** raw
exceptions of which **14** were legitimate, a **66.67 %** false-positive rate, leaving **214,000** of
omitted enabling change and **26,000** of work producing nothing: **USD 240,000**, exactly **10.00 %** of
the baseline. Removing the orphans widened the unallocated gap from 68,000 to **94,000**, which is the
counter-intuitive and correct direction.

The baseline is one three-dimensional statement, and its most informative invariant — that the
time-phased cost baseline still matches the schedule — is the first abandoned under pressure, after
which earned value measures the distance between two documents. How wrong is computable, and the shape
of the answer explains the defect's durability: on Auriga's week-13 position, 240,000 of unre-phased
planned value puts `SPI` anywhere between **0.8276** and **1.0435** against a reported **0.9231**, moves
the composite `EAC` by **USD 287,083** in either direction at **USD 1.20** per USD of mis-phasing — and
leaves `CPI` at **0.9057** and both CPI-only forecasts exactly correct. **A stale `PV` curve corrupts
every schedule measure and leaves every cost-only measure right**, so the cost review passes and the
defect survives.

Configuration management makes version-bound interface verification meaningful, and its audit classes are
not equally serious: five items whose recorded version differs from what is deployed will fail an
integration that a 12.94 % headline rate does not predict. Nor is the item rate the rate that matters.
Integration quality is item quality raised to the number of parties, so Meridian's **87.06 %** of items
gives **75.79 %** of two-sided interfaces and **65.98 %** of three-party ones — **7.50** of 31 interfaces
expected to be unverifiable — and a 95 % interface target therefore requires **97.47 %** item conformance,
at most **8** non-conforming items rather than the 44 found. **The tolerance on the register is derived
from the target on the interfaces, backwards through the exponent**, and where the derived audit interval
comes out at three weeks the finding is about how the register is written, not about how often it is
audited.

Baselines are maintained by accumulation, never replacement, because the only question the funding
organisation asked is performance against the original — the question Case study B's third baseline could
not answer, and to which the answer turned out to be `CPI` **0.87** rather than 0.99. Accumulation is what
makes the domain's cheapest control possible: original plus approved changes equals current, exactly.
Meridian's reconciliation showed a residual of **32,200** — 1.34 %, dismissible — concealing a gross error
of **USD 122,000**, since **77,100** of unreferenced movement and **44,900** of unposted approvals have
opposite signs and hid **89,800** between them. **Reconcile gross, never net.** And the two controls are
disjoint: summing the log catches drift inside it, the reconciliation catches what never entered it.

And change is integrated or it is not controlled. It begins with a classification, and Meridian got
**10.53 %** of them wrong in both directions at once: **USD 34,000** of defects paid as changes — 14.71 %
of the year's direct drift, work that was never a change at all — and **USD 36,340** of changes ruled as
clarifications, unbudgeted and unlogged. Gross **USD 70,340**; net **USD 2,340**. The same identity as the
reconciliation, from unrelated data: **the net understates the gross by twice the smaller class.** The two
errors also differ in detectability, and the one with no downstream detector is silent scope growth, which
is where the tighter control belongs. Rejections are the other cheap entry nobody writes: **9** of
Meridian's 14 left no trace, **4** returned, three costing a duplicated **USD 4,400** assessment and one
costing **USD 63,000** because a different body approved what had already been refused — a total of
**USD 80,600** against **USD 385** of writing, with a mean **4.58 times** the modal case, which is why the
control is bought for the tail and both figures must be quoted.

Meridian's baseline moved **12.1 %** through 34 individually authorised changes averaging
**0.28 %** each, with no decision anywhere on the total, and
a 100,000-in-90-days cumulative rule would not have caught it because a quarter's changes aggregate to
**57,800** — so a cumulative test must be derived from the observed change rate, not chosen for
roundness. A change quoted at **40,000** truly cost **131,560**, **3.29 times** the quoted figure, of
which the quote was **30.4 %**; and therefore the domain's single most valuable sentence, worth more
than most process improvements a programme will make: **the delegation threshold reads on assessed
total impact, not on quoted direct cost.**
