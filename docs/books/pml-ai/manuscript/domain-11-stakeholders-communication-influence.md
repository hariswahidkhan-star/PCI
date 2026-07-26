# Domain 11 — Stakeholders, Communication and Influence

> **Group:** Leading people and organisations (the first of the three domains in Part Three,
> Domains 11–13). **Target:** ~70 pages. **Binds to:** the PCI Book Pattern Specification and the shared
> registries (`docs/books/registries/`). This domain returns to the **Meridian Care Records**
> programme at programme scale, applies Domain 4's interface arithmetic to people, prices Domain 3's
> governance latency as information latency, and supplies the engagement-allocation and negotiation
> arithmetic used by Domains 12, 13, 15 and 16. British English; USD (+SAR where useful, indicative
> `USD 1 ≈ SAR 3.75`). Internal effort in this domain is valued at a stated blended rate of
> **USD 110 per hour**; a neutral facilitator at **USD 150 per hour**.

## Why this domain exists

Part Two built the delivery machinery: scope, schedule, cost, risk, quality, supply. Every one of
those disciplines assumed that the people whose agreement the project needs would give it, roughly
when asked, on roughly the terms proposed. That assumption is the largest unpriced item in most
delivery plans, and this domain prices it.

Two facts sit behind the domain's central claim. The first is Domain 1's arithmetic, which nobody
disputes and few programmes act on: Meridian releases **USD 24,480 a year** for each clinic that
actually uses the system, so **moving adoption creates more value than moving the installation
date** — and adoption is a stakeholder outcome, not a delivery one. The second is that stakeholder
work is nevertheless the first thing cut, because it has no unit of account. A schedule has weeks, a
budget has dollars, and engagement has "relationships", which are not measurable and therefore not
defensible when capacity is short. The domain's central claim is the correction: **engagement is a
resource-allocation problem with a computable return, and communication is an architecture with a
computable load.** Both can be designed, priced and reviewed like any other part of the delivery
system, and where they genuinely cannot be — legitimacy, trust, the moral weight of a community's
objection — the professional obligation is to say so plainly rather than to manufacture a metric
that survives challenge better than the judgement it replaced.

The domain proceeds from the map to the mechanism. KA 11.1 treats the stakeholder set as a *system*
rather than a list, assesses it on influence, interest, attitude and **consent risk**, and allocates
a finite engagement capacity against those four things. KA 11.2 designs communication as an
architecture: it applies Domain 4's interface result to people, computes the load a mesh imposes,
and then computes something almost no reporting regime measures — **how old the number is when an
executive decides on it**. KA 11.3 makes negotiation preparation quantitative through reservation
values, the priced alternative and the zone of possible agreement, then shows that most of the value
in a multi-issue negotiation is created by trading rather than claimed by conceding. KA 11.4 handles
the stakeholders who cannot be managed — the public, communities, and audiences across languages and
decision conventions — and the newest failure mode in the domain: communication that a machine wrote
and nobody checked.

The through-line: **the stakeholder objection you have not paid for in engagement, you will pay for
in delay, at the cost of delay, with interest.**

**Learning objectives.** After this domain a candidate can: distinguish a stakeholder system from a
list and name the parties a register habitually omits; assess stakeholders on influence, interest,
attitude and consent risk, and state what each dimension predicts that the others do not; **price a
late objection at the cost of delay and compute the earlier engagement spend that would have avoided
it, with the breakeven probability that justifies it**; **allocate a finite engagement capacity as a
three-term design — value-led core, consent-risk floors, salience-proportional residual — and
demonstrate what a salience-only allocation forgoes**; apply Domain 4's interface arithmetic to
communication and state the maximum parties a mesh and a routed design each support within a stated
capacity; **compute the age of the information an executive decides on from the reporting period,
consolidation time and paper lead time, and price the reduction a shorter cut buys**; write an honest
status and an escalation-grade message; **build a reservation value from a fully priced alternative,
bound the zone of possible agreement, and compute what preparation is worth in transferred value**;
compute the joint gain a multi-issue trade produces against a split-the-difference compromise;
diagnose conflict by source rather than by personality; price a consultation against the consent risk
it reduces while stating what the arithmetic cannot settle; communicate across decision conventions
without assuming one is universal; and govern AI-assisted communication so that no external statement
is issued unverified and no synthetic voice speaks for the programme.

**The master programme.** Meridian Care Records continues from Domains 1–4: the clinical-records
rollout to **40 clinics**, approved cost **USD 2,400,000**, full-potential benefit **USD 979,200** a
year, realistic benefit **USD 685,440** a year at **70 %** adoption, and a **cost of delay of
USD 14,280 per week** (Domain 1, KA 1.3.2–1.3.3). Three inherited results do the work in this
domain. Each adopting clinic is worth **USD 24,480** a year, so adoption is the largest lever
available to the leader. Meridian's steering committee imposes a governance latency of
`E[wait] = M/2 + L = 4/2 + 2 =` **4.0 weeks** (Domain 3, KA 3.2.3), which is what makes a late
objection expensive. And one interface costs **USD 18,000** to specify, build, test and document
(Domain 4, WE 4.2.3), which is what makes a late objection expensive *twice*. This domain adds the
stakeholder register — **62** identified parties, of which **14** require an individually managed
relationship — and spends the programme's **480 hours** of annual engagement capacity three
different ways to show what the choice is worth.

---

## Knowledge Area 11.1 — Stakeholder systems and engagement strategies

*Topics: 11.1.1 the stakeholder system · 11.1.2 assessment — influence, interest, attitude and
consent risk · 11.1.3 engagement strategy under a capacity constraint.*

### 11.1.1 The stakeholder system

**Definition.** A **stakeholder** is any party whose decisions, consent, resources, behaviour or
objection can materially affect the project's outputs, outcomes or legitimacy, or who is materially
affected by them. A **stakeholder system** is that set of parties together with the relationships
*between* them — who influences whom, who speaks for whom, and which pairs will align or divide
under pressure.

The distinction is not pedantry. A list supports a communication schedule; only a system supports
prediction, because the events that damage projects are relational — a supplier and a regulator
reaching a shared view of a specification the client has not seen, a user group and a professional
body forming a coalition, a funder deferring to an operational director whose interests diverge from
its own. A leader working from a list is repeatedly surprised by conclusions formed in conversations
they were not part of, and each surprise arrives late.

**What registers habitually omit**, consistently enough to be a checklist. *Parties with no
organisational representation* — future users, patients, tenants, passengers, the public — whose
interests are real, whose consent may be legally required, and who have no one on the distribution
list. *Internal parties whose work the project changes without commissioning them*: finance,
procurement, legal, information security, service desks, training functions. *Parties visible only at
handover*, where Domain 16's readiness failures originate. *The supplier's own subcontractors*, whose
interests are not the supplier's (Domain 10). *Predecessors and successors* — the sponsor of the
previous attempt, the owner of the system being replaced, the programme inheriting the dependency.
And *the ones who left*: registers record roles, but relationships attach to people, a point 11.A.2
makes arithmetic.

**The four questions that make a register useful.** Not "who are the stakeholders?" but: what does
this party **decide, consent to, resource, or do** that the project needs? What does the project
change **for them**, and is that a gain or a loss in their own terms? What is their **relationship to
the other parties** — who do they follow, and who follows them? And what is the **cost if their
agreement arrives late**? The last question converts a register from an administrative artefact into
a planning input, because it produces a number, and 11.1.2 computes it.

Note the vocabulary. Stakeholders are not managed; work is managed, and stakeholders are engaged,
consulted, negotiated with and occasionally opposed. "Stakeholder management" quietly licenses the
posture — that the party's view is an obstacle to be handled — that guarantees the late objection
this domain prices. An objection is *information*, sometimes about the stakeholder and sometimes about
the project, and the leader's first task is to establish which.

### 11.1.2 Assessment — influence, interest, attitude and consent risk

**The four dimensions, and what each predicts.** Most assessment practice uses two; four are
needed, because two of the failure modes are invisible in the first pair.

- **Influence** — the party's capacity to affect the project's decisions, resources or permissions.
  Predicts *how much damage a disagreement does*.
- **Interest** — how much the party cares, measured by attention rather than by stake. Predicts *how
  quickly they will notice*, and it is emphatically not a measure of how much is at stake for them:
  a party can have an enormous stake and no attention, and that combination is the most dangerous in
  the register.
- **Attitude** — supportive, neutral, sceptical or opposed, assessed on evidence rather than on
  courtesy. Predicts *what an engagement contact will achieve*. A supportive party needs
  information; a sceptical one needs argument; an opposed one needs negotiation or containment, and
  sending each of them the same monthly bulletin serves none of them.
- **Consent risk** — whether the party holds a veto, a statutory approval, a licence, a signature, a
  clinical or safety authority, or a practical ability to withhold cooperation. Predicts *the
  consequence of being wrong about the other three*, and it is a property of the party's position
  rather than of its disposition. A helpful regulator still holds the veto.

The high-influence, low-interest, high-consent-risk quadrant is where projects are damaged. Such a
party is quiet for months, is therefore assessed as low priority by any allocation rule that reads
interest, and then reads the specification properly at the point of approval. What happens next has
a price.

**Worked example 11.1.2 — the objection that arrived in week 34.**

1. **Setup.** Meridian's national reporting body is assessed influence **5**, interest **2**,
   attitude *neutral*, consent risk **high** (it approves the statutory data extract). It received
   the monthly bulletin and nothing else. In **week 34** of the rollout it objects that the extract's
   record-level structure does not meet its submission requirement. Resolving the objection requires
   a specification change approved by the steering committee — Domain 3's `E[wait]` of **4.0 weeks**
   — followed by **5 weeks** of rework and re-verification, all on the critical path. The direct work
   is re-specification and re-verification of the reporting-gateway interface and the two interfaces
   that depend on it, at Domain 4's unit cost of **USD 18,000** each, plus **USD 42,000** of
   application rework. Cost of delay **USD 14,280 per week**. A technical pre-consultation at design
   stage would have taken **24 hours** of a data specialist's time and **6 hours** of the project
   leader's, at the blended internal rate of **USD 110 per hour**.
2. **Formula.** Assessed total = (governance latency + rework weeks) × cost of delay + interface
   re-verification + application rework. Avoidance cost = hours × rate. Breakeven probability =
   avoidance cost ÷ assessed total.
3. **Substitution.** Delay `(4.0 + 5) × 14,280`. Direct `3 × 18,000 + 42,000`. Avoidance
   `(24 + 6) × 110`. Breakeven `3,300 ÷ 224,520`.
4. **Result.** Delay **9.0 weeks**, costing **USD 128,520**; direct cost **USD 96,000**; **assessed
   total USD 224,520** (≈ SAR 841,950 indicatively). Avoidance cost **USD 3,300**. The consequence is
   **68.0 times** the avoidance cost, and the pre-consultation pays whenever the probability of the
   objection exceeds **1.47 %**.
5. **Interpretation.** The 68-fold ratio is arresting and it is not the useful number; **1.47 %** is.
   A leader arguing for engagement with a ratio is making a rhetorical case, and a sceptical sponsor
   will answer that the objection might not have happened. A leader arguing with the breakeven
   probability has made that answer impossible: the sponsor must now assert that the chance of a
   statutory approver objecting to a specification it has never been shown is *below one and a half
   per cent*, which nobody will say out loud. The asymmetry generalises far beyond this case, because
   engagement costs are hours and consequences are weeks of critical path — so the breakeven
   probability for engaging a high-consent-risk party is almost always in the low single figures.
   Three cautions belong with it. The arithmetic prices the *avoidable* objection; a pre-consultation
   that discovers a genuine incompatibility does not eliminate the cost, it moves it earlier, where it
   is cheaper (Domain 5's design-maturity argument, and Case study B's 7.45-fold escalation). The 5
   weeks of rework is an estimate and the 4.0 weeks of governance latency is not — it is a computed
   property of Meridian's committee design, which means **a governance design and an engagement design
   are the same conversation**: at Domain 3's redesigned latency of 3.0 weeks the same objection costs
   210,240 rather than 224,520. And the pre-consultation must be a *technical* engagement with the
   people who will apply the requirement, not a courtesy briefing of their director, because the
   director did not know either.

### 11.1.3 Engagement strategy under a capacity constraint

**The constraint, stated honestly.** Engagement capacity is finite and small. Meridian's is
**40 hours a month** — the project leader's own engagement time plus two part-time engagement
leads — giving **480 hours** over the twelve months of rollout. Every engagement strategy is an
allocation of those 480 hours whether or not anyone writes it down, and the strategies that are not
written down allocate by whoever asks most persistently — a default that correlates with neither value
nor risk.

**The conventional allocation and its defect.** Standard practice scores each party on influence and
interest, multiplies to a **salience** score, and allocates attention in proportion. It is a
reasonable first cut with one structural defect: it allocates by *power and attention*, and neither is
the same as *value at stake* or *consequence of refusal*. So it over-serves the loud and the powerful
and under-serves two groups at once — the users who determine whether any benefit is realised, and
the quiet approver of 11.1.2.

**The three-term design.** The correction is to allocate in three terms, in this order:

1. A **value-led core** — the effort that demonstrably moves the largest benefit lever, sized from
   the benefit arithmetic rather than from the score.
2. **Consent-risk floors** — a minimum allocation to each party that can stop the work, set by what
   the relationship actually requires, not by its interest score.
3. A **salience-proportional residual** — what is left, distributed by influence × interest.

**Worked example 11.1.3 — allocating Meridian's 480 engagement hours.**

1. **Setup.** Seven groups, scored influence × interest: regional health authority `5 × 3 = 15`;
   clinic directors `4 × 5 = 20`; clinicians `2 × 5 = 10`; practice administrators `2 × 4 = 8`;
   national reporting body `5 × 2 = 10`; patient representatives `3 × 3 = 9`; records-system supplier
   `2 × 4 = 8`. Total salience **80**. Pilot evidence supports a structured adoption intervention —
   a clinical-workflow session plus two follow-ups — costing **8 hours per clinic** and raising
   expected adoption by **12.5 percentage points**, from 70 % to 82.5 %. Each adopting clinic is
   worth **USD 24,480** a year (Domain 1). Consent-risk floors are set at **60 hours** for the
   regional health authority (which holds the funding), **40** for the national reporting body and
   **30** for the patient representative group.
2. **Formula.** Salience allocation = capacity × salience ÷ Σ salience. Three-term allocation =
   value core (8 h × clinics) + Σ floors + residual apportioned by salience. Benefit gain = uplift in
   adopting clinics × annual value per clinic. Value per engagement hour = gain ÷ core hours.
3. **Substitution.** Salience: `480 ÷ 80 =` 6 hours per point. Core `8 × 40`. Residual
   `480 − 320 − 130`, apportioned across salience `8 + 8 = 16`. Gain `40 × 0.125 × 24,480`.

| Group | Salience | Salience-proportional | Three-term design | Basis |
|---|---|---|---|---|
| Regional health authority | 15 | 90 h | **60 h** | consent floor |
| Clinic directors | 20 | 120 h | **160 h** | value core |
| Clinicians (users) | 10 | 60 h | **160 h** | value core |
| Practice administrators | 8 | 48 h | **15 h** | residual |
| National reporting body | 10 | 60 h | **40 h** | consent floor |
| Patient representatives | 9 | 54 h | **30 h** | consent floor |
| Records-system supplier | 8 | 48 h | **15 h** | residual |
| **Total** | **80** | **480 h** | **480 h** | |

4. **Result.** The value core is **320 hours**; floors take **130**; the residual is **30**. The core
   delivers **5** additional adopting clinics and **USD 122,400** a year of recurring benefit, at
   **USD 382.50 of benefit per engagement hour** in year one alone. The salience allocation gives the
   two adoption-determining groups only **180 hours**, enough for **22** of the 40 clinics — an
   uplift of **6.875 percentage points**, **2.75** clinics and **USD 67,320** a year — so it forgoes
   **USD 55,080 a year, permanently**. The core's opportunity cost is `320 × 110 =` **USD 35,200** of
   existing capacity, against which the breakeven uplift is `35,200 ÷ 24,480 =` **1.4379** clinics,
   or **3.59 percentage points** of adoption.
5. **Interpretation.** The decisive figure is the **3.59-point breakeven**, because it converts an
   argument about the value of engagement into a question a clinical lead can answer: does a workflow
   session plus two follow-ups, run in every clinic, move programme adoption by more than three and a
   half points? Almost certainly — and if it does not, the intervention should be redesigned rather
   than defended. Note also what the
   reallocation costs: nothing. The 480 hours are already paid for, so the 55,080 forgone is not an
   underspend but a **misallocation of capacity that was going to be spent anyway**, which is why this
   is the cheapest large improvement in the domain. Two second-order results matter more than they
   look. First, raising adoption to 82.5 % raises the **cost of delay** itself, because the benefit
   rate rises with adopting clinics: 33 clinics at 510 a week is **USD 16,830 per week**, up
   **17.86 %**. So engagement makes every schedule lever more valuable — Domain 1's 8-week compression
   for 60,000, worth a net **+USD 54,240** at 70 % adoption, becomes worth `8 × 16,830 − 60,000 =`
   **+USD 74,640** at 82.5 %. **Sequence engagement before acceleration** is therefore an arithmetic
   ordering, not a preference. Second, the floors cannot be justified by this arithmetic and must not
   be: their return is the avoided 224,520 of 11.1.2, which is a *risk* return with a breakeven
   probability, not a benefit return with a rate. A design that computed only the value core would
   fund adoption brilliantly and be stopped in week 34.

> **Fig 11.1.1 — Two allocations of the same 480 hours.** Paired horizontal bar chart, seven
> stakeholder groups, x-axis engagement hours 0–180. For each group, a blue bar for the
> salience-proportional allocation (influence × interest, 6 hours per point) and a crimson bar for
> the three-term design, with a right-hand column naming the basis of the crimson figure — value
> core, consent floor or residual. A bracket spans the two adoption-determining rows marking
> **320 h (was 180)**. Footnote in two lines: salience alone funds the 8-hour adoption intervention
> at **22 of 40** clinics — **6.875 pp**, **USD 67,320** a year; the three-term design funds all
> **40** — **12.5 pp**, **USD 122,400** a year; forgone by salience **USD 55,080 a year**. Source:
> PCI original. Alt text: paired horizontal bars for seven stakeholder groups showing engagement
> hours moving out of the powerful and attentive groups and into the two groups that determine
> adoption, with the statutory approver protected by a floor.

**From allocation to plan.** An allocation is not yet a strategy. Each group needs a stated
**objective** as a dated future state ("the reporting body has confirmed the extract specification in
writing by week 12", not "maintain good relations"), a **named owner** at a level the party will
engage with, a **method** matched to attitude, a **cadence**, and a **test of whether it worked**.
Objectives written as states rather than activities are what make an engagement plan reviewable:
"held four meetings" cannot fail, and therefore cannot inform.

### AI in this KA

**Where it earns its place.** Building a first-pass register from documents the project already has —
contract schedules, distribution lists, minutes, planning submissions, service-desk records — and
listing parties named in them that the register omits: a set-difference task over unstructured text,
the omission checklist of 11.1.1 mechanised, and one that routinely finds internal functions nobody
thought to list. Classifying a large volume of consultation responses into themes with counts, so
that a leader reads a structured summary of two thousand comments rather than a sample of twenty.
Drafting a salience and consent-risk assessment for human challenge, useful precisely because a draft
is easier to argue with than a blank page. Modelling allocations across capacity and floor
assumptions, which is arithmetic.

**Where it must not go.** Assigning **attitude** or **influence**. A model reads text; influence is
political and attitude is often deliberately unstated, so a fluent assessment of either will be
confidently wrong in the direction the source documents are polite. Inferring a party's negotiating
position from its published statements as though that settled it. And any "stakeholder sentiment
score" entering a report as a governance input — a number with no measurement behind it displaces the
judgement it was meant to support, permanently, because numbers survive committee meetings better
than qualifications do.

**Verification, concretely.** Every register entry carries a named human owner who has confirmed the
party's role, authority and consent risk against a source — the contract, the statutory instrument,
the delegation schedule — not against the model's summary of it. Classified consultation responses are
re-read on a stated sample (10 % is a defensible floor) with the disagreement rate recorded, and the
classification is rejected above a pre-set tolerance. The allocation arithmetic is reproduced by hand:
it is a division and two multiplications.

### Key terms — KA 11.1

| Term | Meaning |
|---|---|
| **Stakeholder** | A party whose decisions, consent, resources, behaviour or objection can materially affect the project's outputs, outcomes or legitimacy, or who is materially affected by them. |
| **Stakeholder system** | The stakeholder set together with the relationships between its members — who influences whom, and which pairs align under pressure. |
| **Influence** | Capacity to affect the project's decisions, resources or permissions; predicts the damage a disagreement does. |
| **Interest** | Attention paid, not stake held; predicts how quickly a party notices. |
| **Attitude** | Supportive, neutral, sceptical or opposed, assessed on evidence; predicts what an engagement contact will achieve. |
| **Consent risk** | Whether the party holds a veto, approval, licence, signature or practical ability to withhold cooperation; a property of position, not disposition. |
| **Salience** | Influence × interest — a useful first cut and an inadequate allocation rule. |
| **Value-led core** | Engagement effort sized from the benefit arithmetic rather than from the salience score. |
| **Consent-risk floor** | A minimum engagement allocation to a party that can stop the work, justified by breakeven probability rather than by benefit rate. |
| **Engagement objective** | A stated future *state* of a relationship with a date and a test, never an activity count. |

### Sample MCQs — KA 11.1

**MCQ 11.1-A `[11.1.1 · Comprehension]`** The practical difference between a stakeholder list and a
stakeholder system is that only the system:
- A. records contact details and communication preferences
- B. captures the relationships between parties, and so supports prediction of positions formed in
  conversations the project is not part of ✅
- C. is approved by the sponsor
- D. includes external as well as internal parties

*Rationale:* The system's added content is the relationships between members (11.1.1). A is
administrative; C is governance; D is a completeness property a list can also have.

**MCQ 11.1-B `[11.1.2 · Application]`** An objection causes 4.0 weeks of governance latency and
5 weeks of rework on the critical path at a cost of delay of 14,280 per week, plus three interface
re-verifications at 18,000 each and 42,000 of application rework. The assessed total is:
- A. USD 96,000
- B. USD 128,520
- C. USD 167,400
- D. USD 224,520 ✅

*Rationale:* `9.0 × 14,280 + 54,000 + 42,000 = 224,520` (11.1.2). A is the direct cost alone; B is
the delay alone; C omits the 4.0 weeks of governance latency and counts only the rework weeks.

**MCQ 11.1-C `[11.1.2 · Evaluation]`** A 30-hour pre-consultation costing 3,300 would have addressed
the objection above. The most persuasive argument to a sceptical sponsor is that:
- A. the consequence is 68.0 times the avoidance cost
- B. the pre-consultation pays whenever the probability of the objection exceeds 1.47 % ✅
- C. engaging regulators early is good practice
- D. 3,300 is immaterial against a 2,400,000 budget

*Rationale:* The breakeven probability forces the sceptic to assert something indefensible, whereas
a ratio invites the answer "it might not have happened" (11.1.2). D is true and irrelevant, since
immateriality is not a reason to spend.

**MCQ 11.1-D `[11.1.3 · Analysis]`** Allocating engagement capacity in proportion to influence ×
interest systematically under-serves:
- A. the most powerful stakeholders
- B. the parties with the highest interest
- C. the user group that determines benefit realisation and the quiet party holding a veto ✅
- D. the project's own delivery team

*Rationale:* Salience measures power and attention, not value at stake or consequence of refusal
(11.1.3) — which is why the design adds a value-led core and consent-risk floors.

**MCQ 11.1-E `[11.1.3 · Application]`** A 320-hour adoption core raises adoption by 12.5 percentage
points across 40 clinics, each adopting clinic being worth 24,480 a year. The benefit per engagement
hour in year one is:
- A. USD 76.50
- B. USD 382.50 ✅
- C. USD 3,060.00
- D. USD 2,142.00

*Rationale:* `40 × 0.125 × 24,480 = 122,400`; `122,400 ÷ 320 = 382.50` (11.1.3). A divides one
clinic's annual value by the 320 hours (`24,480 ÷ 320`); C divides the gain by the 40 clinics rather
than by hours; D divides the whole 685,440 annual benefit by 320 hours instead of the uplift.

### Self-check — KA 11.1

1. *Name the assessment quadrant that damages projects, and why an allocation rule misses it.* —
   High influence, low interest, high consent risk: any rule that reads interest scores it low, and
   it then reads the specification at the point of approval.
2. *State the three terms of an engagement allocation, in order.* — Value-led core sized from the
   benefit arithmetic; consent-risk floors for parties that can stop the work; salience-proportional
   residual.
3. *Why does raising adoption make schedule compression more valuable?* — The cost of delay is the
   benefit rate, and the benefit rate rises with adopting clinics: 14,280 a week at 70 % adoption,
   16,830 at 82.5 %.

---

## Knowledge Area 11.2 — Executive communication and reporting

*Topics: 11.2.1 communication as a designed architecture · 11.2.2 the executive report and the age of
its numbers · 11.2.3 honest status and the escalation-grade message.*

### 11.2.1 Communication as a designed architecture

**The claim.** A communication plan is usually a table of meetings and distribution lists. It should
be an **architecture**, because communication has the same combinatorial property as integration, and
for the same reason: what costs money is not the parties, it is the **channels between them**.

Domain 4, KA 4.2.3 established the arithmetic and this domain does not re-derive it. For `n` parties
each communicating directly with every other, the number of channels is `n(n − 1)/2`; routing every
party through a single integrating point gives `n`. Applied to people it explains something
practitioners experience and rarely diagnose: a programme that has added four stakeholder groups has
not added four relationships but a great many, and the engagement plan written for the original count
is still in use.

**Worked example 11.2.1 — the communication load Meridian was carrying.**

1. **Setup.** Meridian's register holds **62** parties, of which **14** require an individually
   managed relationship. In the design as found, each of those 14 dealt directly with each of the
   others and with the programme, and maintaining one such channel — preparation, contact,
   follow-up, recording — costs **1.5 hours per month**. The alternative routes every party through
   the programme's engagement function as the integrating point, at a hub overhead of **12 hours a
   month** (two stakeholder forums, all-in). Engagement capacity is **40 hours a month** (11.1.3).
2. **Formula.** Mesh channels `n(n − 1)/2`; routed channels `n` (Domain 4, KA 4.2.3). Load = channels
   × hours per channel (+ hub overhead). Sustainable party count = the largest `n` whose load fits
   the capacity.
3. **Substitution.** Mesh `14 × 13/2 = 91`, load `91 × 1.5`. Routed `14`, load `14 × 1.5 + 12`.
   Sustainable mesh count: largest `n` with `n(n − 1)/2 × 1.5 ≤ 40`.
4. **Result.** **91** channels demanding **136.5 hours a month** — **341.25 %** of capacity —
   against **14** channels demanding **33.0 hours**, or **82.5 %**. An unrouted design sustains at
   most **7** parties (21 channels, 31.5 hours); the eighth takes it to 42.0 hours and over capacity.
   A routed design sustains **18** (39.0 hours). Routing saves **103.5 hours a month**, **1,242
   hours a year**, worth **USD 136,620** at the blended rate.
5. **Interpretation.** The number to carry away is **seven**. An unrouted design supports seven
   actively managed parties within Meridian's capacity and Meridian has fourteen, so the design was
   never feasible — and infeasible communication designs do not fail by anyone reporting a shortfall.
   They fail silently: contacts are skipped, the skipped ones are the low-interest parties because
   they do not chase, and 11.1.2's objection is the result. The overload is therefore not a workload
   complaint but the **mechanism** by which the late objection is generated, which is the professional
   insight this arithmetic buys. Three cautions. Routing is not free of distortion: a hub is a single
   point through which every message is paraphrased, and the parties who must not be paraphrased — the
   sponsor, the statutory approver, the clinical authority — need bilateral channels regardless of
   load. Meridian's honest design is therefore **hybrid**: a deliberate three-party mesh among those
   three plus all 14 routed to the hub, giving `3 + 14 = 17` channels, **37.5 hours a month**,
   **93.75 %** of capacity. That is tight and it is the truth; a plan showing 82.5 % would understate
   the design actually chosen. Next, the 1.5-hour unit is an average over relationships of very
   different weight, and a leader quoting one figure for a regulator and a practice administrator
   should say so. And the hub overhead is real work that must be resourced as work: an engagement
   function that is nobody's named job is a mesh with a diagram on top of it.

### 11.2.2 The executive report and the age of its numbers

**What an executive report is for.** Not to describe the project: to enable a decision the reader has
authority to take, or to confirm that none is required. Everything serving neither purpose consumes
the scarcest resource in the governance system, the attention of people who can decide (Domain 3,
Fault 3). The structural consequence is a fixed order, the reverse of how most reports are written:
**the decision or confirmation first**, then the two or three facts that drive it, then the forecast
with its assumptions, then exceptions, then the rest by reference. A report that reaches its decision
on page four was written for the author's comfort.

**The property nobody measures.** A report's usefulness depends on more than its content: it depends
on **how old the facts in it are when the decision is taken**. Three quantities determine that, and
all three are known at design time. Let

```
P = the reporting period (e.g. 4 weeks for a monthly pack)
C = consolidation time — data cut-off to pack issue
L = the paper lead time — how far before the meeting submissions close (Domain 3)
```

Then, measuring back from the decision meeting:

```
Age of the newest fact   = C + L
Mean age of the facts    = C + L + P/2
Maximum blindness        = P + C + L
```

The third is the important one: **maximum blindness** is how long a problem arising just after a cut-off
remains invisible to the decision body. It is the number that should be quoted when anyone proposes
a longer reporting cycle, and it is almost never computed.

**Worked example 11.2.2 — how old is the number Meridian's committee decides on?**

1. **Setup.** Meridian reports monthly: `P` = **4 weeks**, data cut-off at period end, consolidation
   and internal review `C` = **1 week** from cut-off to pack issue, and the steering committee's
   paper lead time `L` = **2 weeks** (Domain 3, KA 3.2.3). The proposed redesign keeps the monthly
   committee but cuts data **weekly** into a standing dashboard — `P` = 1, `C` = 0.5, `L` = 1 — so
   the pack presents the latest weekly cut rather than a monthly consolidation. Cost of delay
   **USD 14,280 per week**.
2. **Formula.** Newest `= C + L`; mean `= C + L + P/2`; maximum blindness `= P + C + L`. Saving
   priced at the cost of delay.
3. **Substitution.** As found: `1 + 2`; `1 + 2 + 4/2`; `4 + 1 + 2`. Redesigned: `0.5 + 1`;
   `0.5 + 1 + 0.5`; `1 + 0.5 + 1`.
4. **Result.**

| | Newest fact | Mean fact age | Maximum blindness |
|---|---|---|---|
| Monthly pack (`P` 4, `C` 1, `L` 2) | 3.0 weeks | **5.0 weeks** | **7.0 weeks** |
| Weekly cut (`P` 1, `C` 0.5, `L` 1) | 1.5 weeks | **2.0 weeks** | **2.5 weeks** |
| Weeks saved | 1.5 | 3.0 | 4.5 |
| Priced at 14,280/week | USD 21,420 | **USD 42,840** | **USD 64,260** |

5. **Interpretation.** Meridian's committee has been deciding on a picture whose average fact is
   **five weeks old**, and a problem arising in the first week of a period can run **seven weeks**
   before the body that could stop it sees it — at 14,280 a week, **USD 99,960** consumed before the
   decision system is aware of it. That reframes the usual argument: the objection to monthly
   reporting is not that it is "not current enough", which is taste, but that its **maximum blindness
   is 7.0 weeks and can be 2.5**, which is arithmetic. Note the ordering of the levers, mirroring
   Domain 3's result about paper deadlines. Shortening the reporting *period* is the strongest lever on
   maximum blindness and costs almost nothing once the data layer exists; consolidation time is next
   and is usually pure process; the paper lead time is Domain 3's lever, shared with the governance
   design. The professional caution is that these savings are **conditional and not additive across
   the year** — they accrue only where a faster picture would have produced a faster decision *and*
   the delay was on the critical path. The honest claim is therefore a per-occurrence one, 42,840 on
   the mean and 64,260 in the worst case, with the number of qualifying occurrences stated as an
   assumption. Presenting it as an annual saving is the error that gets a good proposal rejected by a
   finance director who can see the double count.

> **Fig 11.2.1 — The age of the number an executive decides on.** Two horizontal timeline tracks,
> x-axis weeks before the decision meeting (7 → 0, meeting marked at 0). Each track shows the
> reporting period `P`, the consolidation time `C` and the paper lead time `L` as consecutive
> segments running up to the meeting, with three markers below: newest fact, mean fact age and
> maximum blindness. Track 1 "monthly pack" (`P` 4, `C` 1, `L` 2) marks **3.0 / 5.0 / 7.0 weeks**;
> track 2 "weekly cut" (`P` 1, `C` 0.5, `L` 1) marks **1.5 / 2.0 / 2.5 weeks**. A right-hand panel
> gives the weeks saved priced at USD 14,280 per week: **21,420 / 42,840 / 64,260**. Source: PCI
> original. Alt text: two timeline bars running back from a decision meeting, the monthly one nearly
> three times the length of the weekly one, with markers showing how old the reported facts are when
> the decision is taken.

### 11.2.3 Honest status and the escalation-grade message

**The status problem.** Traffic-light status is universal and structurally corrupt, for a reason worth
stating precisely: the light is set by the person whose performance it evaluates, against undefined
thresholds, for an audience that reacts to colour before content. The predictable result is **amber
compression** — nearly everything is amber, red is reserved for the unrecoverable, and the status
carries almost no information at the point it is most needed. The fix is not more honesty but
**pre-agreed objective thresholds** set at baseline and applied mechanically: "red where forecast
completion exceeds baseline by more than four weeks or forecast cost exceeds budget by more than 5 %"
removes the author's discretion and with it the incentive that corrupted the signal, and it makes the
status arguable on the facts, which is what a governance body needs.

**What a red status must carry**, and this is the difference between reporting a problem and
escalating one. The **exception**, quantified against its threshold. The **cause**, distinguished from
the symptom. The **impact** if nothing changes, priced. The **options**, each with cost, consequence
and risk. The **recommendation**, made by name. The **decision required**, from whom, **by when**,
with the consequence of missing that date. And the **date the leader knew**, which is the field that
makes Domain 3's escalation lead time measurable.

**Bad news.** Three rules earn their place. *Escalate the forecast, not the event* — a leader who
waits for a variance to occur has converted a decision into a report. *Never let a governance body
learn something material from a third party first*: once it happens, the leader's reporting is
discounted for the remainder of the project. And *separate the news from the plea*, because a message
mixing an exception with a request for sympathy invites a response to the second. Domain 12 handles
the personal dimension; the reporting obligation here is narrow and absolute — the material fact
reaches the accountable decision-maker, in writing, on the date it is known.

**The tailoring rule.** Different audiences need different *depth*, never different *facts*. Tailoring
selects what a reader needs to decide and expresses it in their vocabulary; it does not mean the
board's version and the delivery team's version disagree. Where two versions of one status exist, the
project has created a discovery problem for itself, and Domain 3's decision record is where the
discrepancy will be found.

### AI in this KA

**Where it earns its place.** Assembling a report to a fixed skeleton from the data layer — figures,
variances, trends, exception lists — which removes the copy-and-paste error class that infects manually
built packs. **Consistency checking**, the highest-value use in this KA and the least used: reading a
drafted report against its own numbers and flagging where the narrative and the arithmetic disagree,
where a status contradicts its stated threshold, or where a figure differs from the previous pack
without explanation. Rewriting a technical explanation for a non-specialist reader, for human
approval. Producing audience-specific depth variants from one factual base — the tailoring rule
mechanised.

**Where it must not go.** Setting the status, which is an accountable judgement against a threshold.
Writing the recommendation. And — the failure that is easy to walk into and hard to see afterwards —
**smoothing**. A model asked to make a report "clearer", "more balanced" or "less alarming" will
reliably produce a calmer document, because calm prose is what fluent business writing looks like in
the material it learned from, and the qualifier carrying the warning is the first thing to go. Any
instruction to soften a report is an instruction to alter its meaning, and must be treated as an
editorial decision taken by a person, on the record.

**Verification, concretely.** The status and the recommendation are entered by the accountable human
and excluded from any automated drafting step. Every figure traces to a query against the data layer,
available on request rather than reconstructed after a challenge. A named person signs the pack as
issued, and that signature means they have read it — which implies a report short enough to read. Every
consistency flag is resolved and the resolution recorded, because an unresolved flag in an issued
report is documented knowledge of an inconsistency, which is worse than no checker at all.

### Key terms — KA 11.2

| Term | Meaning |
|---|---|
| **Communication architecture** | The designed set of channels between parties — mesh, routed or hybrid — with its load computed against capacity. |
| **Channel load** | Channels × hours per channel per period; compared with engagement capacity to give the sustainable party count. |
| **Integrating point (hub)** | The function through which routed communication passes; carries its own overhead and its own distortion risk. |
| **Reporting period (`P`)** | The interval of activity a report covers. |
| **Consolidation time (`C`)** | Elapsed time from data cut-off to pack issue. |
| **Maximum blindness** | `P + C + L` — how long a problem arising just after a cut-off stays invisible to the decision body. |
| **Amber compression** | The collapse of traffic-light status into a single uninformative colour, caused by author discretion over undefined thresholds. |
| **Escalation-grade message** | Exception, cause, impact priced, options, recommendation by name, decision required by whom and by when, and the date the leader knew. |
| **Tailoring** (of a message) | Varying depth and vocabulary by audience while holding the facts identical. **Context flag:** distinct from *tailoring of method* (Domain 4, KA 4.1.3), which is a recorded decision to adapt process; the shared word carries two different concepts and neither reading substitutes for the other. |

### Sample MCQs — KA 11.2

**MCQ 11.2-A `[11.2.1 · Application]`** Fourteen parties each communicate directly with every other.
The number of channels is:
- A. 14
- B. 91 ✅
- C. 182
- D. 196

*Rationale:* `n(n − 1)/2 = 14 × 13/2 = 91` (11.2.1, citing Domain 4 KA 4.2.3). A is the routed
count; C counts each pair twice; D is `n²`.

**MCQ 11.2-B `[11.2.1 · Analysis]`** At 1.5 hours per channel per month and 40 hours of engagement
capacity, the largest number of parties an unrouted design sustains is:
- A. 7 ✅
- B. 8
- C. 14
- D. 26

*Rationale:* Seven parties give 21 channels and 31.5 hours; eight give 28 channels and 42.0 hours,
which exceeds capacity (11.2.1). C is the actual party count, which is why the design was infeasible;
D divides capacity by the unit cost and ignores the combinatorial term.

**MCQ 11.2-C `[11.2.2 · Application]`** A monthly report has a 4-week period, 1 week of consolidation
and a 2-week paper lead time. The mean age of the facts at the decision meeting is:
- A. 2.0 weeks
- B. 3.0 weeks
- C. 5.0 weeks ✅
- D. 7.0 weeks

*Rationale:* `C + L + P/2 = 1 + 2 + 2 = 5.0` weeks (11.2.2). B is the newest fact only; D is maximum
blindness; A counts only the period's half-life.

**MCQ 11.2-D `[11.2.2 · Evaluation]`** The strongest argument for cutting data weekly rather than
monthly is that it:
- A. makes the pack look more current to the committee
- B. reduces maximum blindness from 7.0 to 2.5 weeks — 4.5 weeks, or USD 64,260 at the cost of
  delay ✅
- C. saves consolidation effort
- D. reduces the paper lead time

*Rationale:* Maximum blindness is the decision-relevant quantity and the redesign is arguable
arithmetically (11.2.2). A is presentational; C is false, since more frequent cuts add consolidation
events; D is a separate governance lever (Domain 3).

**MCQ 11.2-E `[11.2.3 · Analysis]`** The root cause of amber compression is that:
- A. project leaders are optimistic by disposition
- B. the status is set by the person it evaluates, against undefined thresholds ✅
- C. boards react badly to red status
- D. reporting cycles are too long

*Rationale:* The corruption is structural — discretion plus undefined thresholds — so the remedy is
pre-agreed objective thresholds applied mechanically (11.2.3). A and C are contributing conditions,
not the cause.

### Self-check — KA 11.2

1. *Why is an overloaded communication architecture a risk mechanism rather than a workload
   complaint?* — Skipped contacts fall on the parties who do not chase, which are the low-interest,
   high-consent-risk parties whose late objection 11.1.2 prices.
2. *State the three components of report age and which one to quote against a longer cycle.* —
   `C + L` (newest), `C + L + P/2` (mean), `P + C + L` (maximum blindness); quote maximum blindness.
3. *What must a red status carry beyond the exception?* — Cause, priced impact, options, a named
   recommendation, the decision required from whom by when, and the date the leader knew.

---

## Knowledge Area 11.3 — Negotiation and conflict

*Topics: 11.3.1 preparation — reservation value, the priced alternative and the zone of possible
agreement · 11.3.2 creating value before claiming it · 11.3.3 conflict — sources, modes and the
resolution route.*

### 11.3.1 Preparation — reservation value, the priced alternative and the zone of possible agreement

**The three quantities.** Negotiation in a delivery context is not mainly a matter of technique. It is
almost entirely a matter of three numbers, two of which are knowable before the meeting.

- Your **best alternative** — what happens if no agreement is reached, **fully priced**, including
  delay at the cost of delay, internal effort, and the risk the alternative carries, valued as an
  expected monetary value (`EMV`, Domain 8).
- Your **reservation value** — the worst terms you would rationally accept, derived from the priced
  alternative. For a buyer it is a maximum; for a seller a minimum. A reservation value that is not
  derived from a priced alternative is a preference, and preferences move under pressure.
- The **zone of possible agreement (ZOPA)** — the interval between the two parties' reservation
  values. If it is empty there is no deal at any level of skill, and the professional act is to stop
  and change the alternatives rather than to keep meeting.

The single most consequential preparation failure is pricing the alternative on its **invoice** rather
than its **consequences**. A competitive re-tender does not cost the difference in licence fees; it
costs that plus the weeks it takes at the cost of delay, plus internal effort, plus risk. Omit those
and the reservation value is set too low — and a buyer whose reservation value is too low walks away
from deals it should accept, which is the opposite of the error everyone worries about.

**Worked example 11.3.1 — the Meridian support-and-enhancement extension.**

1. **Setup.** Meridian must extend the records-system supplier's support and enhancement contract
   across the rollout year. **Meridian's alternative** is to re-tender and transition to another
   supplier: licence and transition **USD 640,000**, **6 weeks** of transition delay at the cost of
   delay of **14,280** per week, **USD 74,320** of internal transition effort, and transition risk
   assessed at an `EMV` of **USD 45,000**. **The supplier's position**, as estimated by Meridian from
   the tender history and published rate cards: cost to serve **USD 520,000**, and a best alternative
   use of the same team worth **USD 95,000** of contribution. The supplier opens at **900,000**.
   Assume, as a stated benchmark, that a settlement splits the zone equally.
2. **Formula.** `RV(buyer)` = fully priced alternative. `RV(seller)` = cost to serve + forgone
   contribution. ZOPA width = `RV(buyer) − RV(seller)`. Midpoint settlement = mean of the two
   reservation values. Surplus = own reservation value less price (buyer) or price less own
   reservation value (seller). Share of ZOPA = surplus ÷ width.
3. **Substitution.** `RV(buyer) = 640,000 + 6 × 14,280 + 74,320 + 45,000`.
   `RV(seller) = 520,000 + 95,000`. Width `845,000 − 615,000`. Midpoint `(845,000 + 615,000)/2`.
4. **Result.** The alternative's delay component is **USD 85,680**, giving `RV(buyer)` =
   **USD 845,000**. `RV(seller)` = **USD 615,000**. The **ZOPA runs from 615,000 to 845,000**, a
   width of **USD 230,000**, and the supplier's 900,000 opening is **outside it** — there is no deal
   at that price. The midpoint settlement is **USD 730,000**, giving each side **USD 115,000** of
   surplus. Had Meridian not priced its alternative and instead anchored on the supplier's ask,
   settling at **780,000**, its surplus would be **USD 65,000** — **28.3 %** of the zone against the
   supplier's **71.7 %** — and **USD 50,000** of value would have transferred purely through the
   absence of preparation.
5. **Interpretation.** Fifty thousand dollars is the price of an unpriced alternative, available to
   whichever party has done the arithmetic — roughly fifteen times the 3,300 of 11.1.2, which is this
   domain's general pattern: preparation is always cheap relative to consequence. Now the subtler
   result. Suppose Meridian spends **USD 25,000** genuinely pre-qualifying a second supplier, cutting
   the transition delay from 6 weeks to **2** (saving 57,120) and the transition risk `EMV` from 45,000
   to **12,000** (saving 33,000). Its alternative now costs **USD 754,880**, so its reservation value
   **falls** to that figure, the ZOPA narrows to **USD 139,880**, and the midpoint settlement falls to
   **USD 684,940** — **USD 45,060** less cash, a net gain of **USD 20,060** after the spend, with
   **USD 33,000** less risk carried; the breakeven pre-qualification spend is therefore **USD 45,060**.
   And here is the trap: Meridian's *surplus* has fallen from 115,000 to **USD 69,940**, so a
   negotiator measured on surplus captured will report the improved position as a worse outcome.
   Surplus is measured against your own reservation value, which the investment deliberately moved.
   **The measures that govern are cash paid and risk carried, not surplus captured** — worth insisting
   on, because surplus-based incentives reward negotiators for keeping their alternatives bad. Three
   cautions. The counterparty's reservation value is an *estimate* from observable evidence and must be
   presented as a range with the assumptions that move it; a single confident figure invites a
   concession plan built on a guess. The equal-split benchmark is a benchmark, not a prediction — real
   splits turn on patience, alternatives, internal deadlines and who is measured on what. And a ZOPA
   computed on price alone is the smaller half of the problem, which is 11.3.2's subject.

### 11.3.2 Creating value before claiming it

**The distinction.** Claiming value moves a fixed quantity between the parties. Creating value
increases the quantity available, and it is possible whenever the parties value the issues
**differently** — which, across any real contract, they almost always do. A negotiation conducted on
price alone forgoes all of it, and a negotiation conducted by conceding equally on every issue
forgoes most of it.

**Worked example 11.3.2 — the trade that created 82,000.**

1. **Setup.** Beyond price, three issues are open in the Meridian extension. **A**: priority-incident
   response time cut from 4 hours to 2 — worth **USD 84,000** to Meridian, costing the supplier
   **USD 30,000**. **B**: two named engineers dedicated on site through the rollout — worth
   **USD 36,000** to Meridian, costing the supplier **USD 60,000**. **C**: extending the licence to
   8 additional clinics in year 2 — worth **USD 40,000** to Meridian, costing the supplier
   **USD 12,000**. Compare conceding half of each issue with trading whole issues.
2. **Formula.** Joint gain on a set of issues = Σ(value to buyer) − Σ(cost to seller). Compromise:
   half of every issue. Trade: take the issues with positive joint value, drop the rest. Share the
   joint gain through price.
3. **Substitution.** Compromise `0.5 × (84,000 + 36,000 + 40,000) − 0.5 × (30,000 + 60,000 +
   12,000)`. Trade `(84,000 + 40,000) − (30,000 + 12,000)`.
4. **Result.** The compromise gives Meridian **USD 80,000** of value at a cost to the supplier of
   **USD 51,000** — a joint gain of **USD 29,000**. Trading A and C in full and dropping B gives
   Meridian **USD 124,000** at a cost of **USD 42,000** — a joint gain of **USD 82,000**, which is
   **2.83 times** the compromise and **USD 53,000** better in absolute terms. Issue B has a joint
   value of `36,000 − 60,000 =` **−USD 24,000**: it destroys value, so any agreement containing it is
   jointly worse than the same agreement without it. Sharing the 82,000 equally means compensating
   the supplier its 42,000 of cost plus **USD 41,000**, so the price rises **USD 83,000** — from the
   684,940 of 11.3.1 to **USD 767,940** — and each side is **USD 41,000** better off than under the
   compromise-free baseline.
5. **Interpretation.** The compromise looks fair, feels reasonable and destroys **53,000** of joint
   value, which is the most useful thing here: **"splitting the difference" is not a neutral procedure,
   it is an expensive one.** Two further results are worth carrying. Issue B is the one a buyer
   instinctively pushes hardest, because on-site presence feels like commitment, and it is the one
   issue that should be traded away — the arithmetic says so before any relationship is spent on it. A
   leader who cannot compute joint value will spend negotiating capital on B and concede on C, which is
   worth 3.33 times its cost to the other side. And the price rise of 83,000 is what makes the trade
   acceptable to the supplier: a buyer capturing the whole 82,000 by refusing to move on price has
   converted a value-creating trade into a claiming exercise and will be met accordingly at renewal
   (Domain 10 makes the same point about gainshare). The cautions are real. These valuations are the
   buyer's own estimates of both sides' numbers, and the trade only survives if both parties disclose
   *relative priorities* — which is not disclosing reservation values, and the distinction is the whole
   of negotiation ethics in practice: revealing that response time matters more to you than on-site
   presence is cooperative, revealing the maximum you would pay is simply losing. Where a relationship
   has no history of reciprocity, an early unilateral disclosure of priorities will be used to claim
   rather than to create, which is why value creation is easier in a second contract than in a first.

### 11.3.3 Conflict — sources, modes and the resolution route

**Diagnose by source, not by personality.** Conflict on projects is routinely attributed to individuals
and is usually produced by structure. Five sources, each with a different remedy, and misdiagnosis
wastes the intervention:

- **Structural** — two people have been given incompatible objectives, or a matrix has no precedence
  rule (Domain 3, KA 3.1.2). No amount of facilitation resolves this; only a decision about authority
  does.
- **Resource** — two workstreams need the same scarce person. Resolved by priority at the level that
  owns both, and prevented by Domain 6's resource planning.
- **Information** — the parties hold different facts, or the same facts at different ages (11.2.2).
  The cheapest conflict to resolve if diagnosed early and the most damaging if left, because it teaches
  each party that the other is unreliable.
- **Interest** — the parties want genuinely different outcomes. This is a negotiation
  (11.3.1–11.3.2), not a misunderstanding, and treating it as one is condescending as well as
  ineffective.
- **Interpersonal** — genuine relational breakdown. Real, less common than attributed, and Domain 12's
  territory.

**Choosing a response.** The five familiar modes — competing, accommodating, avoiding, compromising,
collaborating — are situational rather than ranked, and the skill is selection. Collaboration is the
highest-value mode and the most expensive in elapsed time, so it fits a large stake and a continuing
relationship and not a decision needed on Friday. Competing is right where safety, legality or a
non-negotiable requirement is at stake, and a leader who cannot compete on those will be rolled on
them. Accommodating is right where the issue matters little to you and much to the other party — it is
how reciprocity is built and the mode most under-used by anxious leaders. Avoiding is right only where
events will genuinely resolve the issue, which is rarer than it is invoked. Compromising is the fast
default whose cost 11.3.2 has just computed: acceptable for small single-issue matters, expensive
everywhere else.

**The resolution route, and its cost.** Escalation is not a mode; it is what happens when the chosen
mode has failed, and it has a price. Domain 3, KA 3.3.3 computed escalation latency and Domain 10,
KA 10.4 the dispute ladder for contractual conflict; neither is re-derived here. The point specific to
this KA is the **threshold**: an unresolved conflict holding a critical-path decision consumes the cost
of delay every week, so the comparison is not "facilitation versus toughing it out" but the
intervention's cost against the delay it stops, divided by the probability it works. Exercise 11.5
computes one such case, and the shape of the answer is that breakeven success probabilities for cheap
facilitation sit in the low single figures — so the intervention is almost always worth attempting and
almost never attempted early.

Two obligations bind a leader who is party to the conflict. Separate the *position* from the *person*
in the written record as well as in the room, because the record is what the organisation reads later.
And hand the resolution to someone who is not a party: a leader chairing a conflict they are in has
converted a resolvable dispute into a legitimacy problem, and legitimacy is not recovered by being
right.

### AI in this KA

**Where it earns its place.** Preparation, which is where negotiations are decided. Building the
reservation-value model of 11.3.1 from its components and testing which assumptions move it — a
sensitivity task with a clear right answer. Enumerating trade combinations: with three issues there are
few and with twelve there are thousands, and finding the sets with positive joint value is genuine
combinatorial work humans do badly. Rehearsing counter-arguments, so that a leader has met the
strongest version of the other side's case before the room. Summarising a long contract and
correspondence history into a chronology with the source document cited for each entry.

**Where it must not go.** Estimating the counterparty's reservation value from plausibility: asked for
that number a model will supply a confident one with no provenance, it will enter the concession plan
as a fact, and real money will move against it. Conducting the negotiation, or drafting messages sent
without a human deciding every commitment they contain — a sentence that concedes an issue concedes it.
Generating a "fair" outcome, which is a judgement about legitimacy, not a computation. And any use in a
live conflict between named individuals: an assessment of a colleague's motives generated by a tool and
stored in a project record is an employment and dignity problem regardless of its accuracy.

**Verification, concretely.** Every input to a reservation value carries a source and a date, and the
alternative's delay component is priced at the project's own cost of delay rather than a rate someone
remembered. The counterparty's estimated reservation value is recorded as a range with the evidence for
each bound, and the concession plan is tested at both. Nothing model-derived is stated to the
counterparty as fact. And the ZOPA arithmetic is reproduced by hand before the meeting — two additions
and a subtraction, and the only number in the room that must be exactly right.

### Key terms — KA 11.3

| Term | Meaning |
|---|---|
| **Best alternative** | What happens absent agreement, priced fully — invoice cost plus delay at the cost of delay plus internal effort plus risk as an `EMV`. |
| **Reservation value** | The worst terms a party would rationally accept, derived from its priced alternative; a maximum for a buyer, a minimum for a seller. |
| **Zone of possible agreement (ZOPA)** | The interval between the two reservation values; empty means no deal at any level of skill. |
| **Surplus** | Own reservation value less the price agreed (buyer), or price less own reservation value (seller). |
| **Joint gain** | Σ(value to one party) − Σ(cost to the other) over the issues traded; negative for issues that destroy value. |
| **Logrolling (trading whole issues)** | Conceding entirely on issues valued asymmetrically, rather than partially on all of them. |
| **Conflict source** | Structural, resource, information, interest or interpersonal — each with a different remedy. |
| **Resolution route** | The chosen mode, its cost, and the escalation path with its latency if the mode fails. |

### Sample MCQs — KA 11.3

**MCQ 11.3-A `[11.3.1 · Application]`** A supplier's cost to serve a contract is 520,000 and the best
alternative use of the same team would earn 95,000 of contribution. Its reservation value is:
- A. USD 425,000
- B. USD 520,000
- C. USD 615,000 ✅
- D. USD 95,000

*Rationale:* `520,000 + 95,000 = 615,000` (11.3.1). B omits the forgone contribution, which is the
commonest error and would put the ZOPA floor 95,000 too low; A subtracts it.

**MCQ 11.3-B `[11.3.1 · Analysis]`** With a ZOPA of 615,000–845,000, a buyer that fails to price its
alternative and settles at 780,000 rather than the 730,000 midpoint has:
- A. captured 71.7 % of the zone
- B. transferred USD 50,000 of value to the seller through the absence of preparation ✅
- C. made an error of USD 165,000
- D. exceeded its reservation value

*Rationale:* `780,000 − 730,000 = 50,000`; the buyer's share falls to 28.3 % (11.3.1). A is the
seller's share; C is the seller's surplus; D is false, since 780,000 is inside the zone.

**MCQ 11.3-C `[11.3.1 · Evaluation]`** Pre-qualifying a second supplier for 25,000 cuts the buyer's
reservation value from 845,000 to 754,880 and the midpoint settlement from 730,000 to 684,940. The
correct reading is that the buyer is:
- A. worse off, because its surplus falls from 115,000 to 69,940
- B. better off by USD 20,060 in cash after the spend, and carrying USD 33,000 less risk ✅
- C. unaffected, since both reservation values moved
- D. better off by USD 45,060, ignoring the pre-qualification cost

*Rationale:* Surplus is measured against a reservation value the investment deliberately moved; cash
paid and risk carried govern (11.3.1). A is the trap; D omits the 25,000.

**MCQ 11.3-D `[11.3.2 · Application]`** Three issues are worth 84,000, 36,000 and 40,000 to the buyer
and cost the seller 30,000, 60,000 and 12,000. Conceding half of each yields a joint gain of 29,000.
Trading the two value-creating issues in full yields:
- A. USD 58,000
- B. USD 82,000 ✅
- C. USD 124,000
- D. USD 160,000

*Rationale:* `(84,000 + 40,000) − (30,000 + 12,000) = 82,000` (11.3.2). A trades all three issues,
including the one whose joint value is −24,000, giving `160,000 − 102,000 = 58,000`; C is the buyer's
value on the traded pair with no deduction of the seller's cost; D is the buyer's value across all
three issues.

**MCQ 11.3-E `[11.3.3 · Analysis]`** Two workstream leads are in open conflict; each has been given an
objective the other's success would prevent. Facilitation has failed twice. The diagnosis is:
- A. interpersonal conflict requiring coaching
- B. structural conflict requiring a decision about objectives and authority ✅
- C. information conflict requiring a single authoritative source
- D. interest conflict requiring negotiation

*Rationale:* Incompatible assigned objectives are structural, and no facilitation resolves a
structure (11.3.3) — the repeated failure of facilitation is itself the diagnostic.

### Self-check — KA 11.3

1. *What is the commonest error in setting a reservation value, and which way does it bite?* —
   Pricing the alternative on its invoice rather than its consequences, which sets the reservation
   value too low, so a buyer walks away from deals it should accept.
2. *Why is splitting the difference expensive?* — It concedes partially on every issue, including
   those with negative joint value; trading whole asymmetrically valued issues produced 82,000
   against the compromise's 29,000.
3. *What may be disclosed in a value-creating negotiation, and what may not?* — Relative priorities
   may; reservation values may not.

---

## Knowledge Area 11.4 — Public and community stakeholders, cross-cultural communication and AI-generated communication risk

*Topics: 11.4.1 public and community stakeholders and consent · 11.4.2 cross-cultural and multilingual
communication · 11.4.3 misinformation and AI-generated communication risk.*

### 11.4.1 Public and community stakeholders and consent

**What is different about them.** Four properties change the method. They are **unbounded** — there is
no membership list, so representativeness is a judgement rather than a fact. They are
**heterogeneous**, so a single "community view" is usually an artefact of who attended. Their
engagement is often **statutory**, with prescribed forms, periods and grounds of objection that vary
entirely by jurisdiction — and nothing here should be read as describing the requirements of any
particular one. And their influence is **discontinuous**: low for long periods, then decisive through a
planning objection, a legal challenge, a political intervention or simple non-cooperation.

**Consultation, and the distinction that decides its value.** Consultation before a decision is
engagement: it can change the design, so participants have a reason to invest in it. Consultation after
a decision is notification with a comment box, and it reliably produces objections rather than
absorbing them, because obstruction is the only remaining way to influence the outcome. Case study B
computes what that distinction was worth on one project — a factor of **3.90** on total cost — and
Domain 5's design-maturity argument explains the mechanism: accommodating a concern costs what it costs
to change the design at the maturity you have reached.

**Pricing consent risk.** Consent risk can be priced with Domain 8's `EMV`, provided the probabilities
are presented as judgements and the breakeven is stated.

**Worked example 11.4.1 — is Meridian's public consultation worth holding?**

1. **Setup.** Meridian's records system creates a regional data-sharing capability with a legitimate
   public interest in it. Without early public consultation, the programme assesses the probability
   of a formal privacy complaint that suspends the data-sharing element at **0.35**; with a
   structured consultation costing **USD 48,000** it assesses **0.10**. A complaint would cost
   **12 weeks** of delay at the cost of delay of **14,280** per week plus **USD 60,000** of legal and
   response cost.
2. **Formula.** `EMV` = probability × consequence (Domain 8). Compare `EMV` without consultation
   against consultation cost + `EMV` with it. Breakeven consultation cost = the difference in the two
   `EMV` terms. Required probability reduction = consultation cost ÷ consequence.
3. **Substitution.** Consequence `12 × 14,280 + 60,000`. Without `0.35 × 231,360`. With
   `48,000 + 0.10 × 231,360`. Breakeven `80,976 − 23,136`. Required reduction `48,000 ÷ 231,360`.
4. **Result.** The consequence is **USD 231,360** (delay **171,360** plus response **60,000**). `EMV`
   without consultation **USD 80,976**; with consultation **USD 71,136**. The consultation is worth
   **USD 9,840**, it remains worth holding while it costs less than **USD 57,840**, and it must
   reduce the probability of complaint by at least **20.75 percentage points** — from 0.35 to
   **0.1425** — to break even.
5. **Interpretation.** The point estimate of 9,840 is the weakest number here and should never be the
   one presented: it is the difference of two products of judged probabilities, so it is fragile in both
   directions and a reviewer can argue it away in a sentence. The **20.75-point required reduction** is
   the number to present, because it asks a question consultation practitioners can answer from
   experience — does a structured, pre-decision consultation cut the likelihood of a formal complaint by
   more than a fifth? — and because it exposes how much of the case rests on judgement rather than
   concealing it. The breakeven cost of **57,840** answers a different and often more useful question:
   how much *more* consultation could be justified on risk grounds alone. Two limits must then be stated
   plainly, and stating them is part of the professional standard rather than a caveat on it. First,
   **the probabilities are not measurements.** They are structured judgements, to be recorded with their
   basis and their author; a programme reporting 0.35 without saying who judged it and on what has
   manufactured a number. Second, and more important: **consultation is not only a risk control.** A
   community's entitlement to be heard about a change to its own services does not depend on whether
   hearing it reduces an expected cost, and a programme that would abandon consultation because the
   arithmetic came out at −9,840 has misunderstood what the arithmetic is for. The `EMV` supports the
   case for consultation where it is resisted; it is not a licence to withdraw it where it is not.
   Legitimacy is not quantifiable, and this book will not pretend otherwise.

### 11.4.2 Cross-cultural and multilingual communication

**What actually varies.** Language is the visible difference and rarely the expensive one. Four others
cost more, and each is *observable* rather than stereotypical — the discipline is to observe how a
specific organisation behaves rather than to predict it from nationality, which is both unreliable and
offensive.

- **Directness.** Whether disagreement is stated or signalled. Where it is signalled, a project that
  hears only stated objections has a silent risk register, and the counter-move is to seek disagreement
  in private and in writing rather than to ask for it in a meeting.
- **Decision convention.** Whether a meeting decides or ratifies a decision reached beforehand, and
  whether authority is individual or collective. A leader who brings a decision to a meeting in a
  consensus-forming organisation gets agreement in the room, no decision, and the discovery weeks later.
- **Attitude to hierarchy.** Whether bad news may travel directly to the accountable person or must
  pass through the chain — which determines the *actual* escalation latency, potentially several times
  the designed one of Domain 3, KA 3.3.3.
- **Time and commitment.** Whether a date is a commitment, an intention or an aspiration; and whether a
  written record is the agreement or a note about it.

**Working method.** Establish the convention explicitly and early — "a decision is recorded in the log
and takes effect then; agreement in a meeting is not yet a decision" — substituting an agreed project
convention for a clash of unexamined ones. Provide material in the language of the people who must act
on it, not merely of those who sign; for Meridian that means clinical-workflow material in the language
clinicians work in. Use **back-translation** for anything carrying a commitment, instruction or safety
implication. Confirm understanding by asking the recipient to state the action, not to confirm receipt.
And allow in the schedule for the elapsed time consensus formation actually takes: a plan assuming
decisions are made in meetings, in an organisation where they are not, carries a systematic and
invisible underestimate in every approval duration.

### 11.4.3 Misinformation and AI-generated communication risk

**The exposure.** A programme of any public significance operates in an information environment where
claims about it circulate faster than it can respond — some mistaken, some deliberate, and increasingly
some machine-generated. Three risks matter to a delivery leader, and none is a communications-department
problem.

**Misinformation about the project.** Incorrect claims about cost, purpose, data handling or safety
gather support because they arrive first and are more interesting than the truth. Corrections propagate
less well than the original claim — a real and well-observed asymmetry, and one this book will not attach
a multiplier to, because no honest general figure exists. The countermeasures are structural rather than
rhetorical: publish the authoritative facts *before* the questions arrive, so a correction is a
reference rather than a rebuttal; maintain a single named source of truth that journalists and community
groups can check; respond to the substance and never to the motivation; and monitor deliberately,
because the first a programme usually hears of a claim is when a governance member forwards it.

**Impersonation and synthetic content.** A programme's name, branding, spokespeople and letterhead can
be reproduced convincingly at negligible cost, including synthetic voice and video of real individuals.
Two controls carry most of the weight: a published, stable statement of the channels through which the
programme communicates officially, so a recipient can verify; and an internal rule that no commitment is
ever created by an inbound communication alone, whatever it appears to be — a fraud control as much as a
communications one.

**AI-drafted communication that nobody verified.** The risk most within the leader's control and most
often mishandled, because the failure does not look like a failure. A model drafting a stakeholder
bulletin produces fluent, plausible text containing dates, commitments and attributions it inferred
rather than knew. A bulletin stating that a clinic will go live in March, when no such commitment
exists, has created an expectation, possibly a representation, and certainly a dispute — and it will be
quoted back to the programme by the party it misled. **That is a governance defect, not a copy-editing
one**: it is an unauthorised commitment, and it warrants the seriousness Domain 3 attaches to a decision
taken without authority.

**Worked example 11.4.3 — the verification step, priced.**

1. **Setup.** Meridian issues **96** external communications a year: 48 weekly bulletins, 24
   clinic-specific notices, 12 authority updates and 12 public updates. AI-assisted drafting saves
   **1.2 hours** each. A mandatory verification step — every factual claim, date and commitment
   checked against a source by a named person — adds **0.5 hours** each. Blended rate **USD 110** per
   hour. Without verification, the programme assesses that **4 %** of items would carry a material
   misstatement, each costing **USD 6,000** in correction, clarification and relationship repair.
2. **Formula.** Net hours = items × (drafting saving − verification time). Verification cost =
   verification hours × rate. Unverified exposure = items × error rate × cost per error. Breakeven
   error rate = verification cost ÷ (items × cost per error).
3. **Substitution.** Net `96 × (1.2 − 0.5)`. Verification `96 × 0.5 × 110`. Exposure
   `96 × 0.04 × 6,000`.
4. **Result.** Drafting saves **115.2 hours**; verification consumes **48.0**; the net saving is
   **67.2 hours**, worth **USD 7,392**. Verification costs **USD 5,280** and stands against an
   unverified exposure of **USD 23,040** across an expected **3.84** defective items — a return of
   **4.36 times**. The verification step pays at any error rate above **0.92 %**.
5. **Interpretation.** The result contradicts the argument usually made in both directions. AI-assisted
   drafting **does** save time — 67.2 hours a year net, **1.4 hours a week** across the 48-week operating
   year, returned to the work 11.1.3 showed is worth 382.50 an hour — and the verification step **is not**
   what destroys the saving: it consumes **41.7 %** of it and returns 4.36 times its cost. The decisive
   figure is
   the **0.92 % breakeven error rate**, because nobody will claim that unverified machine-drafted
   communications about a health programme are accurate more than 99.08 % of the time at the level of
   individual dates and commitments — so the verification step is not a judgement call. Three
   qualifications. The 4 % error rate and the 6,000 per-error cost are assumptions and should be replaced
   with measured values as soon as the programme has any; the verification log produces the error rate as
   a by-product, which is the cheapest measurement available and almost never kept. The exposure prices
   correction and relationship repair only — a misstatement about patient data would not be a 6,000
   event, so the highest-consequence categories warrant a heavier check than 0.5 hours. And the saving is
   only real if drafting replaces work rather than generating *more* communication: 96 items a year is a
   designed number, and a programme issuing 150 because drafting became cheap has not saved 67 hours, it
   has spent 40 more and added 54 items of exposure.

### AI in this KA

**Where it earns its place.** Drafting in multiple languages with **back-translation** as a check — a
genuine capability improvement for programmes that previously could not afford multilingual material at
all. Monitoring public and social channels at volume for claims about the programme, so the first a
leader hears of a misstatement is not a governance member's forwarded message. Readability and
plain-language testing, which is objective and tedious. Producing accessible formats — summaries,
alternative text, simplified versions — from one approved factual base. And drafting the first version of
the factual-claims register that 11.4.3's verification step needs.

**Where it must not go.** External publication without human verification, for the reasons priced above.
Any **synthetic voice or likeness** of a real person, however convenient and however disclosed — a
programme cannot credibly campaign against impersonation while practising it. Automated replies to
community objections, which read exactly as they are and convert a substantive objection into a
legitimacy grievance. Personalised persuasive targeting of identified individuals, a manipulation risk no
delivery benefit justifies. And translation of safety-critical or legally operative text without
qualified human review, where a plausible error is worse than an obvious one.

**Verification, concretely.** A **two-person rule** on every external communication — a drafter and a
named verifier, recorded, the verifier accountable for every date, figure and commitment in the text. A
**factual-claims register** listing each claim and its source document, retained with the issued item so
that a challenge is answered from the record rather than from memory. Back-translation on anything
carrying a commitment or an instruction. A published statement of where the programme uses AI in its
communications, because asking a community to trust you while concealing how you speak is the wrong
economy. And the verification log's error rate reported monthly, since it is the only measurement that
says whether the controls work.

### Key terms — KA 11.4

| Term | Meaning |
|---|---|
| **Community stakeholder** | An unbounded, heterogeneous group with a legitimate interest in a project's effects, often with statutory engagement rights that vary by jurisdiction. |
| **Pre-decision consultation** | Consultation capable of changing the design — engagement; as distinct from post-decision notification. |
| **Consent risk pricing** | `EMV` of a consent failure, presented with its breakeven cost and required probability reduction, never as a point estimate alone. |
| **Decision convention** | Whether a meeting decides or ratifies, and whether authority is individual or collective. |
| **Back-translation** | Independent re-translation to the source language as a check on anything carrying a commitment, instruction or safety implication. |
| **Correction asymmetry** | The observed tendency of corrections to propagate less well than the original claim; real, and not reduced here to a multiplier. |
| **Synthetic content risk** | Convincing reproduction of a programme's identity, spokespeople, voice or documents at negligible cost. |
| **Factual-claims register** | Each claim in an external communication with its source document, retained with the issued item. |
| **Two-person rule** | A named drafter and a named verifier for every external communication. |

### Sample MCQs — KA 11.4

**MCQ 11.4-A `[11.4.1 · Application]`** A consent failure would cost 231,360. Its probability is 0.35
without consultation and 0.10 with a consultation costing 48,000. The consultation is worth:
- A. USD 57,840
- B. USD 9,840 ✅
- C. USD 80,976
- D. USD 23,136

*Rationale:* `0.35 × 231,360 = 80,976` against `48,000 + 0.10 × 231,360 = 71,136`; the difference is
9,840 (11.4.1). A is the breakeven consultation cost; C and D are the two `EMV` terms.

**MCQ 11.4-B `[11.4.1 · Evaluation]`** For the same case, the figure a leader should present to a
sceptical board is:
- A. the expected saving of 9,840
- B. the required probability reduction of 20.75 percentage points ✅
- C. the consequence of 231,360
- D. the consultation cost of 48,000

*Rationale:* The point estimate is the difference of two judged probabilities and is fragile; the
required reduction states exactly what must be believed and can be answered from experience
(11.4.1).

**MCQ 11.4-C `[11.4.2 · Analysis]`** A programme translates all its material accurately but assumes
that agreement reached in a meeting is a decision. In a consensus-forming counterpart organisation the
predictable consequence is:
- A. the translation will be misunderstood
- B. agreement in the room, no decision, and the difference discovered weeks later ✅
- C. escalation will be faster than designed
- D. the counterpart will refuse to meet

*Rationale:* Decision convention, not language, is the expensive variable (11.4.2); the plan's
approval durations are systematically understated as a result.

**MCQ 11.4-D `[11.4.3 · Application]`** Ninety-six external communications a year, verification at
0.5 hours each and USD 110 an hour, against an assessed 4 % error rate at USD 6,000 per error. The
return on the verification step is:
- A. 2.18 times
- B. 4.36 times ✅
- C. 0.23 times
- D. 3.12 times

*Rationale:* Exposure `96 × 0.04 × 6,000 = 23,040` against verification `96 × 0.5 × 110 = 5,280`, a
ratio of 4.36 (11.4.3). A uses 1.0 hour per item rather than 0.5 (`23,040 ÷ 10,560`); C inverts the
ratio (`5,280 ÷ 23,040`); D uses the 7,392 net hourly saving as if it were the verification cost.

**MCQ 11.4-E `[11.4.3 · Analysis]`** An AI-drafted bulletin states a go-live month to which the
programme has not committed. This is best treated as:
- A. a copy-editing error, corrected in the next issue
- B. a governance defect — an unauthorised commitment made outside the decision system ✅
- C. a supplier performance issue
- D. an acceptable risk given the drafting time saved

*Rationale:* The defect is the creation of an expectation and possibly a representation without
authority, which is Domain 3's category of a decision taken without authority (11.4.3).

### Self-check — KA 11.4

1. *What distinguishes consultation from notification, and why does it change the cost?* —
   Consultation can change the design, so accommodating a concern costs what a change costs at that
   design maturity; after the decision, the only remaining influence is obstruction.
2. *Name the cross-cultural variable that most distorts a schedule, and how.* — Decision convention:
   where meetings ratify rather than decide, every approval duration in the plan is understated.
3. *Why is an unverified AI-drafted commitment a governance matter?* — It is an unauthorised
   commitment, created outside the decision system, and it will be quoted back to the programme by
   the party it misled.

---

## Advanced topics — Domain 11

### 11.A.1 Influence without authority, and the arithmetic of a blocking minority

Project leaders routinely need agreement from people they cannot instruct, which is why influence is a
professional competence rather than a personality trait. Four sources require no authority:
**expertise** (being the person who knows, demonstrated rather than asserted); **information** (holding
the integrated picture nobody else holds — a by-product of the leader's position, squandered by hoarding
it); **reciprocity** (having accommodated others on issues that mattered to them and not to you —
11.3.3's under-used mode, and the reason accommodating is an investment rather than a weakness); and
**coalition** (the support of parties the target respects). All four are built before they are needed,
which is the practical difficulty: influence is a stock accumulated in quiet periods and spent in loud
ones, and a leader who begins building it when the difficult decision arrives has begun too late.

Coalition work has an arithmetic worth doing, because voting rules make the target countable. In a
nine-member body requiring a two-thirds majority, **6 of 9** votes carry a motion, so the smallest
blocking coalition is `9 − 6 + 1 =` **4**. Those two numbers answer different questions and are
habitually confused: a leader seeking approval must secure **six** supporters — not five, and not "a
majority" — while a leader assessing risk must ask whether any **four** parties share an interest in
refusal. Domain 3, KA 3.1.2 showed that unanimity is indistinguishable from an inability to decide; this
is the same result at finer resolution, and the consequence is that an influence plan for a
supermajority body targets a *named list of six*, with the four most likely refusers engaged first,
rather than a general campaign of goodwill.

### 11.A.2 The stakeholder register as a decaying asset

A register is treated as a document and behaves like a perishable one, because the entries are roles and
the relationships are with people. Meridian's register holds **62** entries; over twelve months **19**
changed role holder — an annual turnover of **30.6 %**. The consequence is computable from the refresh
cadence: with an annual refresh, entries are on average half a year old, so about **15.3 %** of the
register is wrong at any moment; with a quarterly refresh, about **3.8 %**. Fifteen per cent of 62 means
roughly nine relationships the programme believes it has and does not — and the nine are not distributed
evenly, because turnover is highest in exactly the junior and operational roles that carry the
day-to-day cooperation.

Three practices follow. Refresh on a **cadence tied to observed turnover** rather than annually by
habit; the arithmetic above is the justification and it costs an hour a quarter. Record, for each key
relationship, **who holds it on the programme side**, because the programme's own turnover does the same
damage in the other direction and a departing engagement lead takes the relationship with them unless it
is deliberately transferred. And treat a change of role holder as a **re-engagement trigger**, not an
administrative update: a new incumbent has agreed nothing, has not seen the specification, and is the
single most common source of the 11.1.2 objection arriving in week 34.

### 11.A.3 The reviewer's stakeholder eye

Invariants to test on any engagement and communication design, each cheap and each diagnostic.

Every register entry names a **decision, consent, resource or action** the project needs from that party,
and the **cost if it arrives late**. Every party is scored on **four** dimensions, not two, with
**consent risk** scored from position rather than disposition. The engagement allocation is **explicit
and sums to a stated capacity**, with a value-led core sized from the benefit arithmetic, consent-risk
floors and a residual — not a single proportional rule — and every floor is justified by a stated
**breakeven probability**. The communication architecture's **channel load is computed** against
capacity, and the design names which relationships are deliberately bilateral and why. Report **`P`, `C`
and `L` are stated and maximum blindness computed**, and any proposal to lengthen the cycle quotes the
new figure. Status thresholds are **objective and set at baseline**, and the report records **the date
the leader knew**. Every negotiation of material value has a **written reservation value derived from a
priced alternative** — including delay at the cost of delay and risk as an `EMV` — plus an estimated
counterparty range with its evidence, and multi-issue negotiations carry an **issue-by-issue value and
cost table** so that value-destroying issues are identified before capital is spent on them. Every
external communication has a **named verifier** and a factual-claims register. The register's **refresh
cadence is derived from observed turnover**. And the test that subsumes several others: for each of the
last three stakeholder surprises, ask **which dimension of the assessment would have predicted it** — a
surprise no dimension would have predicted indicates a missing dimension, and one all four would have
predicted indicates an allocation that was never made.

---

## Industry variations — Domain 11

- **Public sector and government.** Consultation is often statutory in form, period and grounds, so the
  engagement plan is partly a compliance instrument and the discretion sits in *how early* rather than
  *whether*; political stakeholders change on an electoral cycle unrelated to the project, making
  11.A.2's re-engagement trigger a scheduled event rather than a contingency.
- **Healthcare.** Clinical authority is not delegable to a project body (Domain 3), so clinical
  stakeholders hold consent risk regardless of their interest score — which is why Meridian's design
  floors them; and the benefit is almost entirely adoption-determined, so 11.1.3's value core dominates.
- **Construction and infrastructure.** Community and landowner objection rights convert directly into
  programme delay, so consent risk is the dominant dimension; the interface-to-people mapping of 11.2.1
  is at its most literal, with parties numerous, physically co-located and contractually separated.
- **Energy and resources.** Community consent is frequently the critical path rather than a risk to it
  (Case study B), long asset lives mean the engagement outlives the project team, and consultation held
  after a layout decision is the standard and expensive error.
- **Technology and product organisations.** Users are numerous, unrepresented and reachable directly,
  which makes 11.1.3's value core computable from telemetry rather than judgement; the characteristic
  failure is over-weighting the loud internal stakeholder whose interest score is high and whose consent
  risk is nil.
- **Financial services and regulated industries.** The regulator is the archetypal high-influence,
  low-interest, high-consent-risk party of 11.1.2, and engagement is bounded by rules about what may be
  discussed and when — so the floor is spent on formal, documented pre-submission engagement rather than
  relationship building, and the record of that engagement is itself a regulatory artefact.

---

## Case study — Domain 11: the stakeholder who was only informed (health, Meridian)

**Situation.** At week 36 of Meridian's rollout, 31 of 40 clinics were installed and the programme was
reported amber. Adoption among installed clinics was running at 62 %, below the 70 % the business case
assumed. The national reporting body had objected in week 34 to the structure of the statutory data
extract, and the objection was being handled as a technical issue by the integration team. The steering
committee had been told none of this in those terms: the week-32 pack described adoption as "building"
and the extract as "under discussion with the regulator".

**What the arithmetic showed.** The project leader computed four numbers before the next governance
review. The **objection**: 4.0 weeks of governance latency plus 5 weeks of rework, three interface
re-verifications at 18,000 and 42,000 of application rework — **USD 224,520**, against a technical
pre-consultation at design stage costing **USD 3,300**, a breakeven probability of **1.47 %**. The
**communication load**: 14 actively managed parties, unrouted, 91 channels at 1.5 hours a month —
**136.5 hours** against a capacity of 40, or **341.25 %** — which explained without anyone needing to be
blamed why the reporting body had received a bulletin and nothing else. The **report age**: `P` 4, `C` 1,
`L` 2, a mean fact age of **5.0 weeks** and a maximum blindness of **7.0 weeks**, so the extract issue
could have run seven weeks before the committee saw it, and had. And the **allocation**: the plan's 480
hours, distributed in proportion to influence × interest, gave the two adoption-determining groups
**180 hours** — enough to run the 8-hour adoption intervention in 22 of 40 clinics, **USD 67,320** a
year of the available **USD 122,400**, forgoing **USD 55,080** a year permanently.

**How it resolved.** Four changes, none requiring new money. The communication architecture became
**hybrid**: all 14 parties routed through a named engagement function, with a deliberate three-party
bilateral mesh for the sponsor, the clinical authority and the reporting body — 17 channels, **37.5 hours
a month**, **93.75 %** of capacity, stated as tight in the plan rather than presented as comfortable.
Data cut **weekly** into a standing dashboard with the monthly pack drawing the latest cut, taking
maximum blindness from **7.0 to 2.5 weeks** and mean fact age from 5.0 to 2.0 — worth **USD 42,840** on
the mean and **USD 64,260** in the worst case per qualifying occurrence. The 480 hours were
**reallocated** on the three-term design: a 320-hour adoption core, 130 hours of consent-risk floors
including 40 for the reporting body, and a 30-hour residual. And status thresholds were fixed at
baseline, so "amber" acquired a definition and the week-32 pack's language became impossible to repeat.

**The outcome, and the part that mattered.** The extract objection took 9 weeks to clear as computed;
nothing recovered that. Adoption among installed clinics reached 79 % by month 12 — short of the 82.5 %
the intervention was designed for, and the shortfall traced to four clinics where the workflow sessions
were run by the supplier rather than by clinical peers, a finding the programme would not have had
without an allocation explicit enough to audit. The durable change was elsewhere. "We need more
stakeholder engagement" had sat in the risk register for two quarters and produced nothing. "Our
communication design requires 341 % of our engagement capacity, our committee decides on five-week-old
facts, and our allocation forgoes 55,080 a year" was approved in one meeting.

**What the domain teaches here.** Engagement failures present as behaviour and are usually capacity and
allocation failures with an arithmetic signature. The overloaded architecture is not a workload
complaint; it is the *mechanism* that produced the week-34 objection, because the contacts an overloaded
design drops are precisely the low-interest, high-consent-risk ones. And note which correction was
largest for the least money: the 480 hours were already being spent.

## Case study B — Domain 11: the consultation that was held twice (energy)

**Situation.** A regional utility developed a 90 MW onshore wind project with a new grid connection
across farmland. Its financial model priced the cost of delay at **USD 62,000 per week** in forgone
contracted revenue net of variable cost. The development team ran a full public consultation costing
**USD 180,000** — after the turbine layout and connection corridor had been fixed, on the reasonable
view that consulting on an incomplete design would confuse people.

**What happened.** The consultation produced 340 responses, the substantive ones concentrating on two
turbine positions and one section of the corridor. Because the layout was fixed, the only available
responses were to refuse the objection or to change the design at detailed-design maturity. Two
objections proceeded formally. The developer ran a **second consultation** at **USD 240,000**, conceded a
layout change costing **USD 410,000** at detailed design, and lost **22 weeks** of planning
determination — **USD 1,364,000** at the project's cost of delay. Total: **USD 2,194,000**.

**How it resolved, and what the alternative would have cost.** The post-project review modelled the
counterfactual with the same objections arising. Consultation at **options** stage, on three candidate
layouts, would have cost **USD 260,000** — 80,000 more, because two rounds and three options are more
work than one round and one answer. The same two turbine positions and the same corridor section would
have been changed while the layout was still a set of options, at **USD 55,000** rather than 410,000 — a
**7.45-fold** escalation avoided, and Domain 5's design-maturity argument in money. Determination would
have run 4 weeks over plan rather than 22: **USD 248,000**. Total: **USD 563,000**. The early programme
costs **USD 80,000** more in consultation and **USD 1,711,000** less in everything else — a net saving of
**USD 1,631,000**, a total cost ratio of **3.90**, and **USD 21.39** avoided downstream for each
additional dollar of early consultation.

**What the domain teaches here.** Consultation before a decision is engagement; after a decision it is
notification, and it reliably produces objections because obstruction is the only influence left. The
mechanism is not goodwill but **design maturity**: the identical concern cost 55,000 to accommodate at
options stage and 410,000 at detailed design, and the 22 weeks of delay followed from having nothing to
offer. Note what the developer's reasoning got right — consulting on an incomplete design *is* harder —
and where it went wrong: it treated that difficulty as a reason to defer, when the arithmetic says the
difficulty is the price of the option. And note the number without which none of this is arguable:
without 62,000 a week, the 22 weeks are an inconvenience and the whole case dissolves into opinion.

---

## Executive perspective — Domain 11

What a programme director cannot delegate in this domain:

- **The engagement allocation, in hours, summing to your stated capacity.** Not a communication plan —
  an allocation, with a value-led core sized from the benefit arithmetic and a breakeven stated for
  every consent-risk floor. Meridian's misallocation forgave 55,080 a year of capacity it was
  spending anyway (11.1.3).
- **The channel load of your own communication design.** Compute it. An architecture demanding 341 %
  of capacity is not a workload problem, it is the mechanism that generates your next late objection,
  and it fails silently on the parties who do not chase (11.2.1).
- **Maximum blindness.** `P + C + L`. Know the number, quote it whenever anyone proposes a longer
  cycle, and be able to say how old the average fact in your last pack was when the committee decided
  (11.2.2).
- **The status definition, set at baseline, and the date you knew.** Discretion over an undefined
  threshold is what corrupted the signal; a definition and a knew-date restore it, and the knew-date
  is what makes your escalation lead time measurable (11.2.3, Domain 3).
- **Your reservation value on every material negotiation, in writing, before the meeting.** Derived
  from a fully priced alternative including delay at your cost of delay and risk as an `EMV`. An
  unpriced alternative cost Meridian 50,000 on one contract; and never let anyone measure your
  negotiators on surplus captured, which rewards them for keeping the alternative bad (11.3.1).
- **That no external communication leaves the programme unverified.** A named verifier and a
  factual-claims register on every item. The verification step returns 4.36 times its cost and pays at
  any error rate above 0.92 % — and an AI-drafted commitment nobody checked is an unauthorised
  commitment, not a typographical one (11.4.3).

---

## Calculation exercises — Domain 11

**Exercise 11.1** A programme actively manages 11 parties. Each maintained channel costs 2.0 hours a
month; engagement capacity is 45 hours a month; a routed design carries a hub overhead of 10 hours a
month. Compute the mesh and routed loads and their utilisation, the largest number of parties each
design sustains, and the annual hour saving from routing.
*Solution.* Mesh channels `11 × 10/2 =` **55**, load `55 × 2 =` **110 hours a month**, or
`110/45 =` **244.4 %** of capacity. Routed `11 × 2 + 10 =` **32 hours**, **71.11 %**. Largest
sustainable unrouted count: 7 parties give 21 channels and **42 hours** (within 45); 8 give 28
channels and **56 hours** (over). So **7**. Routed: `2n + 10 ≤ 45` gives `n ≤ 17.5`, so **17**
(44 hours). Annual saving `(110 − 32) × 12 =` **936 hours**. Common error: computing `n²` (121) or
`n(n − 1)` (110 channels rather than 55) — the second is doubly seductive here because it coincides
numerically with the correct hour figure.

**Exercise 11.2** A programme has 600 engagement hours over 15 months and five stakeholder groups
scored influence × interest: A `5 × 4`, B `3 × 5`, C `4 × 2`, D `2 × 4`, E `3 × 3`. Compute the
salience-proportional allocation. Then rebuild it as a three-term design in which C (the statutory
approver) takes a consent floor of 120 hours and D (the adopting user group across 50 sites) takes a
value-led core of 260 hours, with the residual apportioned by salience among A, B and E. Each
adopting site is worth USD 19,200 a year and the core is expected to raise adoption by 10 percentage
points. Compute the annual gain, the benefit per engagement hour and the core's hours per site.
*Solution.* Salience: A 20, B 15, C 8, D 8, E 9, total **60**; `600/60 =` **10 hours per point** →
A **200**, B **150**, C **80**, D **80**, E **90**. Three-term: C **120**, D **260**, residual
`600 − 120 − 260 =` **220** across salience `20 + 15 + 9 = 44` at 5 hours a point → A **100**,
B **75**, E **45**; total 600. Gain `50 × 0.10 × 19,200 =` **USD 96,000** a year; per engagement hour
`96,000/260 =` **USD 369.23**; core intensity `260/50 =` **5.20 hours per site**. Common error:
allocating by salience alone, which gives the statutory approver 80 hours against a required floor of
120 and the adopting group 80 against a required 260 — under-serving simultaneously the party that can
stop the work and the party that determines whether any benefit exists.

**Exercise 11.3** A monthly report has a 4-week period, 1.5 weeks of consolidation and a 1-week paper
lead time; cost of delay is 9,500 per week. Compute the newest fact age, mean fact age and maximum
blindness, then the same three under a fortnightly cycle with 0.5 weeks of consolidation and a
0.5-week paper lead time, and price each saving.
*Solution.* As found: newest `C + L = 1.5 + 1 =` **2.5 weeks**; mean `C + L + P/2 = 2.5 + 2 =`
**4.5 weeks**; maximum blindness `P + C + L =` **6.5 weeks**. Redesigned: newest `0.5 + 0.5 =`
**1.0**; mean `1.0 + 1.0 =` **2.0**; blindness `2 + 0.5 + 0.5 =` **3.0**. Savings **1.5**, **2.5** and
**3.5 weeks**, priced at **USD 14,250**, **USD 23,750** and **USD 33,250**. Common error: reporting
the newest fact age as the age of the picture, which understates the mean by `P/2` — 2.0 weeks here,
or 19,000 at this cost of delay.

**Exercise 11.4** A buyer's fully priced alternative to renewing a contract costs USD 1,200,000. The
incumbent's cost to serve is USD 820,000 and its best alternative use of the same team would earn
USD 130,000 of contribution. Compute both reservation values, the ZOPA and its width, the midpoint
settlement and each side's surplus there. Then compute the buyer's surplus and share of the zone if
it anchors on the incumbent's opening ask and settles at USD 1,090,000, and the value transferred
relative to the midpoint.
*Solution.* `RV(seller) = 820,000 + 130,000 =` **USD 950,000**; `RV(buyer) =` **USD 1,200,000**. ZOPA
**950,000 – 1,200,000**, width **USD 250,000**. Midpoint **USD 1,075,000**, surplus **USD 125,000**
each. At 1,090,000 the buyer's surplus is `1,200,000 − 1,090,000 =` **USD 110,000**, **44.0 %** of
the zone against the seller's **56.0 %**, and **USD 15,000** has transferred relative to the
midpoint. Common error: treating the seller's cost to serve as its reservation value, which puts the
ZOPA floor at 820,000 — 130,000 too low — and leads a buyer to pursue prices the seller will never
accept, or to conclude wrongly that a workable deal is unavailable.

**Exercise 11.5** An unresolved priority conflict between two workstreams has held a critical-path
decision for 3 weeks at a cost of delay of 14,280 a week, and escalation preparation has consumed
4 people × 5 hours at the blended rate of 110. A facilitated resolution session would take 6 hours of
a neutral facilitator at 150 an hour plus 14 hours of participant time at 110. Compute the cost
already incurred, the cost of the facilitated route, the ratio between them, and the minimum
probability of success that would have justified the facilitation.
*Solution.* Incurred: delay `3 × 14,280 =` **USD 42,840** plus preparation `20 × 110 =` **USD 2,200**,
total **USD 45,040**. Facilitation: `6 × 150 + 14 × 110 = 900 + 1,540 =` **USD 2,440**. Ratio
**18.46**. Breakeven success probability `2,440/45,040 =` **5.42 %**. Common error: comparing the
facilitator's fee alone (900) with the delay and concluding the intervention is trivially cheap —
participant time of 1,540 is the larger part of its cost and is the real reason facilitation is
resisted; conversely, omitting the 2,200 already spent on escalation preparation understates what the
conflict has cost by 5 % of the total.

---

## Practitioner's toolkit — Domain 11

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 11.T.1 — Stakeholder register and engagement allocation sheet

One row per party: reference · party and named individual · **what the project needs from them** (decide /
consent / resource / do) · **cost if their agreement arrives late**, priced at the cost of delay ·
influence (1–5) · interest (1–5) · attitude, with the evidence · **consent risk** (none / withholds
cooperation / holds approval or veto, with the instrument) · relationships (who they follow, who follows
them) · engagement objective as a **dated future state** · named owner on the programme side · method ·
cadence · **allocated hours** · test of whether it worked · date last verified.

Beneath the rows, the allocation summary: stated capacity in hours; the **value-led core** with the
benefit arithmetic that sizes it; each **consent-risk floor** with its breakeven probability; the
**residual** and its apportionment; and a total that must equal capacity. Report monthly: parties with no
named programme owner, floors underspent, objectives past their date, and entries unverified within the
refresh cadence derived from observed turnover (11.A.2). An allocation that does not sum to a stated
capacity is a wish list.

### Toolkit 11.T.2 — Communication architecture and report-age sheet

*Part one, the architecture.* Actively managed parties `n` · channels under a mesh `n(n − 1)/2` and under
routing `n` · hours per channel per period, with the basis · hub overhead · computed load and utilisation
against stated capacity · the **sustainable party count** under each design · and an explicit list of
**deliberately bilateral relationships** with the reason each cannot be intermediated. *Part two, report
age.* For each recurring report: `P` · `C` · `L` · computed newest fact age, mean fact age and **maximum
blindness** · the cost of delay · and the priced saving from each candidate reduction. The purpose is that
both quantities are **visible at design time**; a communication plan that cannot fill this in is a
distribution list.

### Toolkit 11.T.3 — Negotiation preparation sheet

*Our position.* The alternative, itemised: invoice or transition cost · elapsed weeks × cost of delay ·
internal effort · risk as an `EMV`, with its basis · **total = our reservation value**. What would improve
the alternative, at what cost, and the **breakeven spend** — the shift it produces in the reservation
value. *Their position, estimated.* Cost to serve · forgone contribution · **estimated reservation value
as a range**, with the evidence for each bound. *The zone.* ZOPA low and high, width, and the settlement
at the equal-split benchmark. *The issues.* One row per non-price issue: value to us · cost to them
(estimated) · **joint value** · trade or drop, with every negative-joint-value issue flagged as not to be
pursued. *Conduct.* Concession sequence and what each concession buys · what we will disclose (relative
priorities) and what we will not (reservation value) · who speaks · what requires escalation before
agreement. Reviewed against outcome afterwards, because the estimate of the counterparty's reservation
value is the input that most needs calibrating and is almost never checked against what happened.

---

## Exam preparation — Domain 11

**What is assessed.** The stakeholder system and the parties registers omit; four-dimensional assessment
and what each dimension predicts; **pricing a late objection and its breakeven probability**; **the
three-term engagement allocation under a capacity constraint** and what a salience-only rule forgoes;
communication as an architecture, with **channel load and sustainable party count**; **report age —
newest, mean and maximum blindness**; honest status, objective thresholds and the escalation-grade
message; **reservation values from priced alternatives, the ZOPA, surplus and share**; **joint gain from
trading whole issues against splitting the difference**; conflict diagnosis by source; consultation
before versus after a decision, and **consent risk priced as an `EMV` with its required probability
reduction**; cross-cultural decision conventions; and the verification of AI-drafted communication.

**The calculations to be able to do under time pressure.** Assessed cost of an objection from governance
latency plus rework at the cost of delay, plus interface and rework components, and its breakeven
probability. A salience-proportional allocation, then a three-term allocation to the same capacity;
benefit per engagement hour; the breakeven adoption uplift. Mesh and routed channel counts and loads, and
the sustainable party count under a stated capacity. `C + L`, `C + L + P/2` and `P + C + L`, each saving
priced. Reservation values from a fully priced alternative (including delay and `EMV`), ZOPA width,
midpoint, surplus, share of zone and value transferred against a benchmark. Joint gain for a compromise
and for a trade, and the price rise that shares it. `EMV` with and without a mitigation, the breakeven
mitigation cost and the required probability reduction. Verification cost against exposure, and the
breakeven error rate.

**The traps.** Computing `n²` or `n(n − 1)` for channels (Exercise 11.1) · quoting the newest fact age as
the age of the picture, understating the mean by `P/2` (Exercise 11.3) · omitting governance latency when
pricing an objection (MCQ 11.1-B) · allocating capacity by influence × interest alone, which under-serves
the benefit-determining group and the quiet approver simultaneously (11.1.3, Exercise 11.2) · treating a
seller's cost to serve as its reservation value and putting the ZOPA floor too low (Exercise 11.4) ·
pricing an alternative on its invoice rather than its consequences, which sets a reservation value too low
and forfeits deals that should be accepted (11.3.1) · reading a fall in surplus as a worse outcome when
the reservation value was deliberately moved (MCQ 11.3-C) · splitting the difference across issues
including those with negative joint value (11.3.2) · presenting an `EMV` point estimate rather than the
required probability reduction (11.4.1) · annualising a report-age saving that accrues only per qualifying
occurrence (11.2.2) · and treating an unverified AI-drafted commitment as a copy-editing error (11.4.3).

**How the domain connects.** Domain 1 supplies the cost of delay every calculation here is priced at, and
the adoption arithmetic that makes engagement the largest available lever. Domain 3 supplies the
governance latency `E[wait]` that makes a late objection expensive and the paper lead time `L` that enters
report age. Domain 4 supplies the interface arithmetic `n(n − 1)/2` applied here to people, and the 18,000
interface unit cost. Domain 5 supplies the design-maturity result behind Case study B. Domain 6 consumes
engagement durations as schedule input, since a consensus-forming approval is a longer predecessor than
the plan assumes (11.4.2). Domain 8 supplies `EMV`, used for the priced alternative and for consent risk.
Domain 10 supplies the procurement and dispute machinery this domain's negotiation behaviour operates
inside, and defers negotiation itself to here. Domain 12 takes the interpersonal and team dimension of
conflict and influence; Domain 14 the systematic treatment of AI governance whose communication face
appears in 11.4.3; Domain 15 the same arithmetic at portfolio scale; and Domain 16 the adoption
measurement this domain's value core exists to move.

---

## Domain 11 summary
Stakeholder work has no natural unit of account, which is why it is cut first and why it is the largest
unpriced item in most delivery plans. This domain gives it units. Meridian's national reporting body —
influence 5, interest 2, consent risk high, and sent a bulletin — objected in week 34 at an assessed cost
of **USD 224,520**: 9.0 weeks of critical path (4.0 of them Domain 3's governance latency) at 14,280 a
week, plus three interface re-verifications and application rework. The pre-consultation that would have
surfaced it cost **USD 3,300**, so engagement paid at any probability of objection above **1.47 %** — and
that breakeven, not the 68-fold ratio, is what wins the argument, because a sponsor can dismiss a ratio
and cannot assert one and a half per cent.

Engagement is an allocation of a stated capacity. Meridian's 480 hours, distributed in proportion to
influence × interest, gave the two adoption-determining groups 180 hours and forgave **USD 55,080 a year,
permanently**, of capacity that was being spent anyway; the three-term design — a **320-hour** value-led
core, **130 hours** of consent-risk floors, a **30-hour** residual — delivers **5** more adopting clinics
and **USD 122,400** a year at **USD 382.50 per engagement hour**, on a breakeven uplift of **3.59
percentage points**. It also raises the cost of delay to **USD 16,830** a week, which makes every schedule
lever more valuable and settles the sequencing question: engagement before acceleration.

Communication is an architecture with a computable load. Fourteen actively managed parties admit **91**
channels unrouted — **136.5 hours a month** against a capacity of 40, **341.25 %** — while routing gives
14 channels and 33.0 hours; an unrouted design sustains **7** parties and a routed one **18**, and
Meridian's honest hybrid runs 17 channels at **37.5 hours**, **93.75 %** of capacity. Reporting has an
age: `C + L` for the newest fact, `C + L + P/2` for the mean, `P + C + L` for **maximum blindness** — 3.0,
5.0 and **7.0 weeks** as found, 1.5, 2.0 and **2.5** after weekly cuts, saving up to **USD 64,260** per
qualifying occurrence. Status is corrupted by discretion over undefined thresholds and repaired by
thresholds set at baseline plus a recorded date the leader knew.

Negotiation is preparation. A reservation value derived from a fully priced alternative — 640,000 plus 6
weeks at 14,280 plus 74,320 of effort plus a 45,000 `EMV` — put Meridian's at **USD 845,000** against the
supplier's **USD 615,000**, a **USD 230,000** zone whose midpoint is **730,000**; failing to price the
alternative and settling at 780,000 would have transferred **USD 50,000** for nothing. Improving the
alternative for 25,000 cut the settlement to **USD 684,940** and the risk carried by 33,000 while
*reducing* surplus to 69,940 — the trap that reveals which measures govern. And most of the value was
never in the price: trading two asymmetrically valued issues produced a joint gain of **USD 82,000**
against the **USD 29,000** of a split-the-difference compromise, with issue B destroying **24,000** and
therefore never worth pursuing.

Public stakeholders cannot be managed, only engaged, and consultation before a decision is engagement
while consultation after it is notification: Case study B's project paid **USD 2,194,000** for the second
where **USD 563,000** would have bought the first, a ratio of **3.90**, because the identical concern cost
55,000 at options stage and **410,000** at detailed design. Consent risk prices as an `EMV` — 80,976
against 71,136, worth 9,840, breaking even at a **20.75-point** reduction in probability — with the plain
statement that legitimacy is not quantifiable and consultation is not only a risk control. And the newest
failure mode is the cheapest to close: verifying AI-drafted external communication costs **USD 5,280**
against an exposure of **USD 23,040**, returns **4.36 times**, pays at any error rate above **0.92 %**,
and still leaves **67.2 hours** of net saving — while an unverified machine-written commitment is not a
typographical error but a decision taken without authority.

The through-line: **the objection you have not paid for in engagement, you will pay for in delay, at the
cost of delay** — and every quantity in that sentence is computable at design time, from numbers a
programme already has.
