# Domain 5 — Scope, Requirements and Value Definition

## Why this domain exists

Part One produced a project. Domain 1 established what the leader is answerable for, Domain 2 whether
the work was worth choosing and whether the promise was honest, Domain 3 who may decide and how
quickly, and Domain 4 how the parts fit and how a change moves all three baseline dimensions. All four
assumed something they never established: that the **content** of the scope was known.

It usually is not, and the gap is not the one practitioners expect. The familiar complaint is that
requirements were incomplete. The more expensive and far commoner condition is that they were
**complete, agreed, delivered, verified; and did not produce the value the business case promised.**
Domain 4's WBS audit found a missing element worth USD 214,000, and what it found missing was
clinician training and enabling change: not a forgotten cost line but a **forgotten requirement**,
that the receiving organisation be able to use what it was given. The omission did not originate in
a spreadsheet. It originated in a room where nobody asked the clinicians anything.

The domain's central claim follows: **scope is not a list of things to build; it is a chain of
justified links from a benefit to an accepted deliverable, and the chain is only as good as its
weakest link, which is almost never the build.** Four consequences organise the chapter. An
unwritten boundary is decided later by whoever is most insistent: KA 5.1 prices one undrawn boundary
at USD 143,000, then shows the correct answer was to *include* the disputed sites, a decision the
boundary statement would have surfaced and its absence prevented. Requirement defects are not
equally expensive; they cost four times more at each stage they survive, and KA 5.2 shows a USD
84,000 elicitation programme returning USD 610,800, with **a single avoided live-service defect
paying for the whole of it**. Requirements do not carry equal value per unit of effort, and where
capacity binds KA 5.3 shows the intuitive ranking method losing USD 44,000 a year (USD 262,737 of
present value) to simple enumeration. And scope grows by accumulation with nobody approving
anything, a different failure from Domain 4's authorised drift needing a different detector: KA 5.4
finds USD 220,920 of movement that **left no trace in the change log at all**, and the only control
that catches it is a count.

The through-line: **the cheapest place to be wrong about scope is at the start, and the arithmetic of
how much cheaper is the most under-used business case in project management.**

**Learning objectives.** After this domain a candidate can: write a scope statement that is decidable
rather than descriptive, and distinguish deliverables from outcomes and from activity; assemble a
scope baseline and state which artefacts constitute it; write exclusions and assumptions that settle
boundaries in advance, and **price an undrawn boundary on both its cost and its forgone benefit,
including the payback period**; select elicitation techniques by the kind of requirement being sought
and identify the stakeholder groups whose absence causes systematic requirement loss; **compute the
correction cost of a requirement defect by the stage at which it is found, the saving from moving
detection earlier, the return on an elicitation investment, and the schedule delay at which that
investment stops paying**; specify a requirement so that it is unambiguous, testable, singular and
traceable; **audit a traceability matrix forward and backward, report the defect rate by class rather
than as a total, and say which class invalidates the record**; attribute benefit to requirement
bundles and re-test a "must have" classification; **rank requirements by value per unit of constrained
effort, and demonstrate where greedy ranking loses to enumeration and by how much**; distinguish
controlled change from uncontrolled creep, **quantify creep by count reconciliation and price it at
the cost of delay**; write acceptance criteria that are testable and **compute the breakeven dispute
probability that justifies writing them**; distinguish verification from validation and price a
conditional acceptance against the correction ladder; and govern AI-assisted requirements work without
letting a model author, prioritise or accept scope.

**The master programme.** Meridian Care Records continues from Part One: the clinical-records
rollout to **40 clinics**, approved cost **USD 2,400,000**, full-potential benefit **USD 979,200** a
year, realistic benefit **USD 685,440** a year at 70 % adoption, cost of delay **USD 14,280 per
week** (Domain 1), the ramped business case with NPV **+USD 1,332,898** (Domain 2, WE 2.2.2),
steering latency `E[wait] = M/2 + L =` **4.0 weeks** (Domain 3), and the integrated baseline,
interface architecture and authorised drift of **USD 291,176** (Domain 4). This domain works its
requirements baseline of **480** approved requirements, its **425**-element design register, its
Release 1 capacity of **20 development-weeks**, and the **USD 520,000** clinic-rollout element from
Domain 4's WBS, whose per-clinic figure of **USD 13,000** prices the boundary question KA 5.1 opens
with. From Domain 6, **Project Auriga** (the 25-week control-systems upgrade, BAC **USD 4,000,000**)
takes over as the single-project illustration.

---

## Knowledge Area 5.1 — Scope definition and the scope baseline

*Topics: 5.1.1 the scope statement · 5.1.2 the scope baseline · 5.1.3 exclusions, assumptions and the
boundary.*

### 5.1.1 The scope statement

**Definition.** The scope statement is the authorised description of **what the project will deliver
and what it will not**, expressed in terms specific enough that a competent third party could judge
whether a given item falls inside it. That last clause is the whole test, and it is the one most scope
statements fail.

**Three registers, and only one of them is scope.** Practitioners habitually conflate them, and the
conflation is where value definition goes wrong:

| Register | What it states | Example (Meridian) |
|---|---|---|
| **Outcome** | The change in the world the organisation is paying for | Clinicians retrieve a complete patient history in under 20 seconds at any of 40 clinics |
| **Deliverable** | The thing that will be handed over and accepted | A records application, 31 verified interfaces, migrated data, a trained user population |
| **Activity** | The work that produces the deliverable | Build, configure, test, migrate, train, cut over |

Scope is the **deliverable** register, bounded by the outcome register and decomposed into the
activity register (Domain 4's WBS). A scope statement written in the activity register, "implement a
records system", cannot be tested for completeness, because activities have no boundary of their
own. A scope statement written purely in the outcome register cannot be accepted, because an outcome
depends on behaviour the project does not control. The discipline is to write the deliverables,
state the outcome each serves, and keep the two columns visibly distinct.

**What makes a scope statement decidable.** Four properties, each of which converts an argument into a
lookup: **enumerated** deliverables (a countable list, not a category); **quantified** extent (40
clinics, not "the region"); **named** interfaces and dependencies, by reference to Domain 4's register;
and **explicit exclusions**, which are the subject of 5.1.3 and the half most often omitted. A scope
statement with all four is short. Length in a scope statement is almost always a symptom of the
activity register having crept in.

### 5.1.2 The scope baseline

**What it is.** The scope baseline is the **approved** scope statement together with the WBS and the
WBS dictionary: three artefacts, one authorised statement, and one of the three dimensions of the
performance measurement baseline Domain 4, KA 4.3.1 insists is single. It changes only through
integrated change control (Domain 4, KA 4.4), and every change to it is a change to schedule and
cost by construction.

**The WBS dictionary is where scope actually lives.** The scope statement gives the boundary and the
WBS gives the structure; the dictionary gives, per work package, the description of work, the
deliverable, the acceptance basis, the responsible owner, the estimate and the dependencies. It is the
document that answers "is this in scope?" at the level at which the question is actually asked, and it
is the document most often left as a set of names. A WBS whose dictionary is a list of element titles
has a structure and no content, and the deficit becomes visible at acceptance, when the acceptance
basis has to be invented by whoever is in the room.

**The four scope-baseline defects**, in increasing order of cost to repair: *unbounded* — a category
rather than a count, so extent is settled later by negotiation (5.1.3 prices one); *undocumented at
dictionary level*, so acceptance criteria are improvised (KA 5.4.2 prices it); *untraceable to benefit*
— deliverables no benefit requires, benefits no deliverable serves (KA 5.3.1); and *unversioned*, so
nobody can say what was agreed, which is Domain 4's configuration failure arriving through the scope
door.

### 5.1.3 Exclusions, assumptions and the boundary

**Why exclusions are the highest-return sentences in the document.** An exclusion costs a line to
write and settles a dispute that would otherwise be resolved under time pressure, by whoever is most
insistent, in month fourteen. It is Domain 3's decidability test applied to content rather than to
authority: *if this question arises, who answers it, and from what?* Where the answer is "from the
scope statement", the boundary holds. Where it is "from a conversation", the boundary is wherever the
conversation ends.

**Assumptions are exclusions with a deadline.** An assumption states something the project has taken
as true without controlling it: that the identity provider will be available, that clinical data
quality in the legacy system meets a stated standard, that a named team is released on a stated
date. Each carries an owner and a date by which it becomes a fact or a risk (Domain 8), because an
assumption with neither is a hope in a register.

**Worked example 5.1.3 — the boundary nobody drew, and the answer that was not "no".**

1. **Setup.** Meridian's approved scope statement says the records application will be rolled out to
   "all clinics in the region". The approved count in the business case is **40** clinics. The regional
   service register, however, also lists **6** branch surgeries, **3** out-of-hours hubs and **2**
   prison health units — **11** further sites, each of which has been told by its own management that
   it is included. From Domain 4's WBS the clinic-rollout element is **USD 520,000** across the 40
   clinics. Full-potential benefit is **USD 979,200** a year across the 40 clinics; realistic adoption
   is **70 %** (Domain 1).
2. **Formula.** Per-site rollout cost = rollout element ÷ approved site count. Boundary exposure =
   additional sites × per-site cost. Per-site benefit at realistic adoption = (full potential ÷ site
   count) × adoption. Payback period = exposure ÷ additional annual benefit.
3. **Substitution.** `520,000 / 40`; `11 × 13,000`; `(979,200 / 40) × 0.70`; `11 × 17,136`;
   `143,000 / 188,496`.
4. **Result.** Per-site rollout cost **USD 13,000**. The undrawn boundary carries an exposure of
   **USD 143,000**: **5.96 %** of the approved baseline. The same 11 sites would generate `11 ×
   17,136 =` **USD 188,496** a year at realistic adoption, against a full-potential per-site figure
   of **USD 24,480**. Payback on including them is **0.76 years, about 9.1 months**.
5. **Interpretation.** The instinct on seeing a 143,000 boundary exposure is to write the exclusion,
   and that instinct is wrong here, which is exactly why the example is worth working. Eleven sites
   costing 143,000 once and returning 188,496 a year pay for themselves in **nine months**;
   excluding them destroys value, and including them by accident in month fourteen destroys almost
   as much, because by then the rollout waves are scheduled, the training capacity is committed and
   the 11 sites arrive as a disruption rather than as a plan. The value of the boundary statement is
   therefore not that it says no. **It is that it forces the question to be asked while the answer
   is still cheap to act on**, which is the same argument Domain 2 made about options and the same
   one Domain 4 made about assessing before deciding. Three cautions before this arithmetic is
   reused. The 13,000 per-site figure is the *rollout element only*: it excludes licence cost,
   clinical safety assessment and the training element Domain 4 found missing at USD 214,000, so the
   true marginal cost per site is higher and the payback longer: a leader quoting nine months must
   say which cost basis it rests on. The 17,136 per-site benefit assumes the new sites adopt at the
   same 70 % as the original 40, which is a genuine assumption and not a fact: prison health units
   in particular have a different clinical workflow, and a lower adoption assumption for them would
   be defensible. And the decision is a **change** under Domain 4, KA 4.4 whichever way it goes: an
   11-site extension is not a clarification, and recording it as one is precisely the
   misclassification KA 5.4.1 quantifies.

### AI in this KA

**Where it earns its place.** Reporting every unbounded extent in a scope statement: every category
noun without a count, every "all", "as required" and "including but not limited to". It is
mechanical, exhaustive, and exactly the reading humans do not perform on a document they have
already approved. Cross-checking the deliverable list against an organisational asset or site
register to surface candidate boundary questions, which is how Meridian's 11 sites would have
appeared in week three rather than month fourteen. Drafting WBS dictionary entries from an approved
structure and existing estimates, for owner completion. Checking that every assumption carries an
owner and a resolution date.

**Where it must not go.** Deciding a boundary. The 11-site question turns on clinical workflow,
regional politics, adoption expectations and a benefit judgement, and it is a change decision belonging
to a named authority. Nor should a model author the exclusion list, because an exclusion is a
commitment about what will *not* be delivered and someone must be accountable for having made it.

**Verification, concretely.** Every flagged extent is confirmed against the source document, because
the flag rate is high and the false-positive rate is not zero. Every candidate boundary question is
put to the sponsor as a question, never resolved as an inference. The per-site arithmetic is
reproduced by hand (one division) with its cost basis stated. And any AI-drafted dictionary entry is
endorsed by the work-package owner before it enters the baseline, since the dictionary is the
acceptance basis.

### Key terms — KA 5.1

| Term | Meaning |
|---|---|
| **Scope statement** | The authorised description of what will and will not be delivered, specific enough to be tested against. |
| **Outcome / deliverable / activity registers** | The three levels routinely conflated; scope is the deliverable register, bounded by outcomes and decomposed into activities. |
| **Decidable scope** | Enumerated deliverables, quantified extent, named interfaces, explicit exclusions. |
| **Scope baseline** | The approved scope statement + WBS + WBS dictionary, one of Domain 4's three baseline dimensions. |
| **WBS dictionary** | Per work package: work, deliverable, acceptance basis, owner, estimate, dependencies, where scope actually lives. |
| **Exclusion** | A written statement of what is outside the boundary; the highest-return line in the document. |
| **Assumption** | Something taken as true without being controlled, carrying an owner and a resolution date. |
| **Boundary exposure** | The priced consequence of an unstated extent: additional units × unit cost. |

### Sample MCQs — KA 5.1

**MCQ 5.1-A `[5.1.1 · Comprehension]`** A scope statement reads "implement a regional records
system". Its principal defect is that:
- A. it is too short
- B. it is written in the activity register, so it has no boundary that can be tested ✅
- C. it does not name the sponsor
- D. it omits the schedule

*Rationale:* Activities have no boundary of their own, so completeness cannot be assessed (5.1.1).
Length is not the defect, decidability is; C and D belong to the charter and the schedule.

**MCQ 5.1-B `[5.1.3 · Application]`** A rollout element of USD 520,000 covers 40 approved sites. Eleven
further sites believe they are included. The boundary exposure is:
- A. USD 13,000
- B. USD 143,000 ✅
- C. USD 60,000
- D. USD 188,496

*Rationale:* `520,000/40 = 13,000` per site, `× 11 = 143,000` (5.1.3). A is the cost of one site,
not of eleven; C divides the whole 2,400,000 baseline by 40 rather than the rollout element, giving
a per-site 60,000: the wrong cost base, and a per-site figure rather than an exposure; D is the
annual *benefit* of the 11 sites, not their cost.

**MCQ 5.1-C `[5.1.3 · Evaluation]`** The 11 sites would cost USD 143,000 to include and generate
USD 188,496 a year at realistic adoption. The correct professional conclusion is that:
- A. they should be excluded, because the exposure exceeds the tolerance
- B. the boundary statement's value is that it forces the decision while it is still cheap to act on —
  and here the decision is probably to include them ✅
- C. they are in scope already, since the statement says "all clinics in the region"
- D. the exposure should be added to contingency

*Rationale:* A nine-month payback makes exclusion value-destroying (5.1.3); the boundary statement's
function is to surface the question in time, not to say no. C is the reading that causes the dispute;
D funds an unmade decision.

**MCQ 5.1-D `[5.1.2 · Analysis]`** A WBS dictionary consists only of element titles. The defect
becomes visible:
- A. immediately, at baseline approval
- B. at acceptance, when the acceptance basis has to be invented by whoever is present ✅
- C. during estimating
- D. only if the scope changes

*Rationale:* The dictionary carries the acceptance basis; its absence is invisible until acceptance is
attempted (5.1.2). That is what makes it an expensive defect rather than a documentation nicety.

### Self-check — KA 5.1

1. *Which register is scope, and what happens when the other two are used instead?* — The deliverable
   register; the activity register removes any testable boundary, and the outcome register cannot be
   accepted because it depends on behaviour the project does not control.
2. *Why is an exclusion the highest-return line in a scope statement?* — It costs a line and settles a
   dispute that would otherwise be resolved late, under pressure, by whoever is most insistent.
3. *What is the point of pricing a boundary if the answer turns out to be "include them"?* — The point
   is that the question gets asked while the answer is still cheap to act on; a nine-month payback
   accepted in week three is a plan, and the same decision in month fourteen is a disruption.

---

## Knowledge Area 5.2 — Requirements elicitation, analysis and traceability

*Topics: 5.2.1 elicitation and its economics · 5.2.2 analysis and specification quality · 5.2.3
traceability.*

### 5.2.1 Elicitation and its economics

**Definition.** Elicitation is the deliberate discovery of what stakeholders need, as distinct from
collection of what they ask for. The distinction is the discipline: a stated request is a proposed
solution to an unstated need, and a requirements process that records requests has captured other
people's designs without capturing the problem.

**Techniques, matched to what is being sought.** Each technique is good at one thing and blind to
another, which is why a single-technique programme fails predictably:

| Technique | Good at | Blind to |
|---|---|---|
| Structured interview | Depth, rationale, the *why* behind a request | Group disagreement; what the individual does not know they do |
| Facilitated workshop | Conflict surfacing, shared prioritisation | The quiet stakeholder and the absent one |
| Observation of work as done | The gap between documented and actual process | Rare cases, exceptions, and the volume-weighted picture |
| Document and data analysis | Volumes, exception rates, statutory obligations | Intent, and anything not already recorded |
| Prototype or walkthrough | Tacit needs made visible by reaction | Non-functional requirements (load, availability, security) |
| Existing-system defect and support logs | What actually fails today | What has never been attempted |

**The stakeholder groups whose absence causes systematic loss.** Requirement loss is not random; it
clusters on groups that are structurally easy to omit; the people who will *operate* the service
after handover, the people who will *support* it, the *external* users who never attend internal
meetings, and whoever owns the **enabling change** that converts an output into an outcome. Domain 4
found the last of these missing as a USD 214,000 WBS element; Case study B of this domain finds the
third of them missing at a cost of most of a programme's benefit.

**The economics, which is the part usually asserted rather than computed.** It is a commonplace that
finding a requirement defect early is cheaper. The professional question is *how much cheaper, and
therefore how much elicitation is worth buying*, and that is arithmetic.

**Worked example 5.2.1 — the correction ladder, and what elicitation is worth.**

1. **Setup.** Meridian's own change and defect records give the average cost of correcting one
   requirement defect by the stage at which it is found: **USD 400** at definition, **1,600** at
   design, **6,400** at build, **25,600** at test and integration, **102,400** in live service, a
   ladder rising by a factor of **four** at each stage. Projects of this type in the organisation
   surface about **96** requirement defects across a 480-requirement baseline (**0.2** per
   requirement), on this profile: **24** at definition, **22** at design, **26** at build, **18** at
   test, **6** in live service. A structured elicitation programme (clinician workshops across the
   40 clinics, observation of records use as actually performed, prototype walkthroughs) costs **USD
   84,000** (**USD 175** per requirement) and is expected to move **13** build-stage, **9**
   test-stage and **3** live-service defects into the definition stage.
2. **Formula.** Expected correction cost = Σ (defects at stage × cost at stage). Saving from earlier
   detection = Σ (defects moved × [cost at old stage − cost at definition]): the **difference**, not
   the old cost, because the defect still costs 400 to correct at definition. Return = saving ÷
   investment. Breakeven delay = (saving − investment) ÷ cost of delay.
3. **Substitution.** `24 × 400 + 22 × 1,600 + 26 × 6,400 + 18 × 25,600 + 6 × 102,400`; then
   `13 × 6,000 + 9 × 25,200 + 3 × 102,000`; then `610,800 / 84,000`; then
   `(610,800 − 84,000) / 14,280`.
4. **Result.** The unimproved expected correction cost is **USD 1,286,400**: an average of **USD
   13,400** per defect. The elicitation programme saves `78,000 + 226,800 + 306,000 =` **USD
   610,800**, for a net of **USD 526,800** and a return of **7.27 times** the investment. The
   expected correction cost falls to **USD 675,600**, or **USD 7,037.50** per defect. And the
   investment breaks even at a definition-phase extension of **36.89 weeks**.
5. **Interpretation.** Four things here matter more than the headline return. First, the single most
   useful sentence a leader can take from it: **moving one defect out of live service saves USD
   102,000, which by itself pays for the entire USD 84,000 programme.** Nothing about a 7.27-times
   return is needed to authorise the spend; one defect is. Second, the *breakeven delay* answers the
   objection this investment always attracts. That elicitation delays the start. Elicitation would
   have to extend the definition phase by **more than 36.89 weeks** before it destroyed value, and a
   realistic four-week extension costs `4 × 14,280 =` **USD 57,120**, leaving **USD 469,680** net.
   The objection is real, and the breakeven is **more than nine times** the extension anyone is
   proposing, an order of magnitude short of decisive; a leader who cannot say that has to argue
   about it instead. Third, the saving is computed on the **difference** between stages, never on
   the later cost alone: the gross figures give `13 × 6,400 + 9 × 25,600 + 3 × 102,400 =`
   **620,800**, overstating the saving by 10,000 by forgetting that the defect still has to be
   fixed. Fourth, two real limits rather than hedges. The ladder is **this organisation's observed
   ladder**, not a law of nature: technology, regulatory regime and release cadence all change the
   ratio, and a programme that has not measured its own should say it is using an assumed one. And
   the arithmetic assumes the moved defects were **detectable at definition**; emergent integration
   behaviour, load characteristics that appear only at volume and clinical workflow effects that no
   prototype predicts are genuinely not, which is why the example moves half of the build and test
   defects rather than all of them. The honest claim is a large return on a *subset*, not the
   elimination of late defects.

> **Fig 5.2.1 — What a requirement defect costs by the stage it is found.** Column chart on a
> logarithmic y-axis (USD 200 to 200,000), x-axis the five stages: definition **400** (×1), design
> **1,600** (×4), build **6,400** (×16), test and integration **25,600** (×64) and live service
> **102,400** (×256), the last column in crimson. A dashed horizontal line at **USD 84,000** marks the
> elicitation programme's cost, annotated **"one live defect moved to definition saves 102,000 — pays
> for it"**. A note on the plot states that the scale is logarithmic and each stage costs four times
> the last. Source: PCI original. Alt text: five columns rising steeply on a logarithmic scale from 400
> to 102,400, with a dashed line just below the tallest column showing that a single avoided
> live-service defect exceeds the whole elicitation budget.

### 5.2.2 Analysis and specification quality

**From elicited need to specified requirement.** Analysis is the work of turning what stakeholders said
into statements that can be designed against, built, tested and accepted. Six properties define a
usable requirement, and each has a specific failure:

- **Necessary**: it traces to a benefit or an obligation. A requirement traceable to neither is
  someone's preference, and it will be built.
- **Unambiguous**: one reading only. The standard offenders are comparatives with no referent
  ("faster", "user-friendly"), passive constructions that hide the actor, and the conjunction
  "and/or".
- **Singular**: one requirement per statement. A compound requirement is accepted in part and
  disputed in part, and its status is permanently "in progress".
- **Testable**: a stated condition and a stated observable result. Untestable requirements are the
  subject of KA 5.4.2's arithmetic.
- **Feasible**: achievable within the architecture, the budget and the law, which requires a
  technical reader in the analysis loop.
- **Traceable**: identified, and linked forwards and backwards (5.2.3).

**Requirement classes, and why they must be separated.** *Functional* (what the system does),
*non-functional* (how well — performance, availability, security, accessibility, usability),
*constraint* (an imposed platform, standard or deadline), *data* (content, quality, retention),
*transition* (migration, cutover, training, decommissioning) and *regulatory* (not negotiable and
not prioritisable). They fail in different ways, which is why 5.2.3 reports them separately; and the
two that go missing most reliably are **non-functional** requirements (because no user asks for
availability), and **transition** requirements, which are Domain 4's missing 214,000 in requirement
form.

**The two analysis pathologies.** *Solution capture*: recording the stakeholder's proposed solution as
the requirement, which locks the design before the problem is understood and forfeits every cheaper
option; the test is whether the statement expresses a need several designs could satisfy.
*Requirement inflation*: elaborating each need into derived statements until the count impresses and
the meaning disperses. A 480-requirement baseline for a records rollout is traceable; the same content
expressed as 3,000 is traceable by nobody, and the count itself then obstructs prioritisation (KA 5.3).

### 5.2.3 Traceability

**Definition.** Traceability is the maintained, bidirectional linkage from **benefit → requirement →
design element → test case → accepted deliverable**, such that any link can be followed in either
direction. Its purpose is not documentation. It is to make four questions answerable by lookup rather
than by opinion: what will happen to the benefit if this requirement is dropped; what must be re-tested
if this design element changes; is there anything being built that nobody asked for; and can this
deliverable be accepted.

**Forward and backward are different tests.** *Forward* traceability asks whether every requirement
reaches a design element, a test case and an accepted deliverable: it detects **unmet scope**.
*Backward* traceability asks whether every design element and every test case traces to an approved
requirement: it detects **unrequested scope**, which is the mechanism of gold-plating and one of the
routes by which creep enters (KA 5.4.1). Programmes overwhelmingly run the forward test only, and
the reverse test is the cheaper of the two.

**Worked example 5.2.3 — auditing Meridian's traceability matrix.**

1. **Setup.** At entry to integration testing, Meridian's traceability audit covers the **480**
   approved requirements and the **425**-element design register. Forward: **416** requirements trace
   to a design element, a test case and a deliverable; **34** have no design element at all; **21**
   have a design element but no test case; **9** are recorded as accepted with no evidence of any
   verification having been performed. Backward: of the 425 design elements, **17** trace to no
   approved requirement.
2. **Formula.** Forward defect rate = non-conforming requirements ÷ approved requirements, reported
   **by class**. Reverse defect rate = orphaned design elements ÷ design register: a **different
   denominator**, and therefore not combinable with the forward rate.
3. **Substitution.** `(34 + 21 + 9) / 480`; and separately `34/480`, `21/480`, `9/480`; then
   `17/425`.
4. **Result.** **64** forward non-conformances, a **13.33 %** forward defect rate, comprising **7.08
   %** with no design, **4.38 %** with no test and **1.88 %** accepted without verification. **86.67
   %** of requirements are fully traced. Separately, **17** orphaned design elements: a **4.00 %**
   reverse defect rate.
5. **Interpretation.** The classes are not totalled, for the reason Domain 4, KA 4.3.2 refused to
   total its configuration-audit classes: **they fail differently, and the total is the least
   informative number available.** The **34 with no design** are unmet scope that is still cheap,
   they surface at design review at 1,600 each on the ladder of 5.2.1, and the honest reading is
   that baseline and design are 34 requirements out of step. The **21 with no test case** will be
   delivered, accepted on assertion and found defective in live service at **102,400** each. The **9
   accepted without verification evidence** are the serious finding, for a reason that has nothing
   to do with their number: the acceptance record states something untrue, so **every decision that
   relied on it (including the release decision) rested on nothing**, the same structural failure as
   Domain 4's five mis-recorded configuration items. A programme reporting "13.33 % traceability
   defects" has buried that. Two further observations. The rounded class rates sum to 13.34 %
   against a combined 13.33 %, display rounding, and one more reason not to present a total. And the
   **17 orphans** are design and build effort spent on work nobody approved, worth `17 × 6,400 =`
   **USD 108,800** at build-stage correction cost if they must be removed: invisible to the forward
   test most programmes run alone, and the reverse test costs one query.

### AI in this KA

**Where it earns its place.** Traceability auditing is close to ideal AI work: exhaustive matrix
traversal with definite answers, done badly and partially by humans, every finding checkable against
a source — both directions, by class, with the orphan list produced as a matter of course. Ambiguity
detection in requirement text (comparatives without referents, passive constructions hiding the
actor, statements joined by "and", "and/or" and "as appropriate") is linguistic pattern matching
with a high true-positive rate, and is the highest-value AI application in the domain. Clustering
interview notes and support-log entries into candidate themes, and flagging near-duplicates in a
large baseline. Proposing candidate test conditions for analyst and tester ruling. And checking that
every requirement carries a class and that the non-functional and transition classes exist at all:
the absence of a whole class is a finding a model notices and a human does not.

**Where it must not go.** Authoring requirements. A requirement is a commitment made on behalf of
stakeholders who exist, and a model generating plausible requirements from a project description
produces exactly the failure this KA exists to prevent: a baseline that reads well and represents
nobody. Ruling that a requirement is unnecessary or duplicated. Closing a traceability gap by
creating the missing link, which converts a finding into a fabrication and is the most tempting
automation failure here. And no AI output may serve as verification evidence for the nine
requirements of WE 5.2.3; that is the class the audit exists to catch.

**Verification, concretely.** Every reported gap is confirmed against both artefacts before it becomes
a finding, and the orphan list is confirmed with the design owner, who can usually name the
conversation the element came from. Ambiguity flags are triaged by the analyst, since some flagged
constructions are correct in context. Clustered themes go back to the stakeholders who produced them,
never straight into the baseline. The ladder arithmetic is reproduced by hand and its provenance stated
as observed or assumed. And every AI contribution is recorded as an input, with the human author named.

### Key terms — KA 5.2

| Term | Meaning |
|---|---|
| **Elicitation** | Deliberate discovery of need, as distinct from collection of requests. |
| **Solution capture** | Recording a stakeholder's proposed solution as the requirement, locking the design before the problem is understood. |
| **Correction ladder** | The cost of correcting one requirement defect by the stage found; Meridian's observed ladder rises ×4 per stage. |
| **Breakeven elicitation spend** | The investment at which the saving from earlier detection is exactly recovered; expressed either as a sum or as a tolerable delay. |
| **Requirement classes** | Functional · non-functional · constraint · data · transition · regulatory: reported separately because they fail differently. |
| **Requirement inflation** | Elaborating need into a count that impresses and disperses meaning, defeating traceability and prioritisation. |
| **Forward traceability** | Requirement → design → test → accepted deliverable; detects unmet scope. |
| **Backward traceability** | Design or test → approved requirement; detects unrequested scope, and costs one query. |
| **Orphan** | A design element or test case tracing to no approved requirement. |

### Sample MCQs — KA 5.2

**MCQ 5.2-A `[5.2.1 · Application]`** On a ladder of 400 / 1,600 / 6,400 / 25,600 / 102,400 per defect
by stage, an elicitation programme moves 13 build-stage, 9 test-stage and 3 live-service defects to the
definition stage. The saving is:
- A. USD 620,800
- B. USD 610,800 ✅
- C. USD 526,800
- D. USD 1,286,400

*Rationale:* The saving is the **difference** at each stage: `13 × 6,000 + 9 × 25,200 + 3 × 102,000 =
610,800` (5.2.1). A uses the gross later-stage costs and forgets the defect still costs 400 to fix at
definition; C is the saving net of the 84,000 investment; D is the unimproved total correction cost.

**MCQ 5.2-B `[5.2.1 · Evaluation]`** The strongest single argument for the USD 84,000 elicitation
programme is that:
- A. it returns 7.27 times its cost
- B. one defect moved out of live service saves USD 102,000 and pays for the whole programme ✅
- C. average correction cost falls from 13,400 to 7,037.50 per defect
- D. early detection is recognised good practice

*Rationale:* The single-defect argument is decisive because it needs no assumption about how many
defects move (5.2.1); A and C are true but depend on the whole estimated shift, and D is an appeal to
practice rather than a computation.

**MCQ 5.2-C `[5.2.3 · Analysis]`** In a 480-requirement audit, 34 have no design element, 21 no test
case, and 9 are recorded as accepted with no verification evidence. Which finding most undermines the
**release decision itself**, and why?
- A. the 34, because unmet scope is the largest group
- B. the 21, because untested requirements reach live service
- C. the 9, because the acceptance record states something untrue, so every decision that relied on it
  rested on nothing ✅
- D. all three equally, at a combined 13.33 % defect rate

*Rationale:* A false record invalidates the decisions taken on it, including the release decision
(5.2.3) — the same structural failure as Domain 4's mis-recorded configuration items. A ranks by group
size and B by future correction cost; both are real findings and both leave the release record
truthful, which is why neither is the answer to *this* question. D is the reading the class breakdown
exists to prevent.

**MCQ 5.2-D `[5.2.3 · Comprehension]`** Seventeen design elements trace to no approved requirement out
of a 425-element register. This is:
- A. a forward traceability defect of 3.54 %
- B. a reverse traceability defect of 4.00 %, on a different denominator from the forward rate ✅
- C. immaterial, since the requirements are all still met
- D. evidence that 17 requirements were lost

*Rationale:* `17/425 = 4.00 %`, and the design register is not the requirements baseline, so the two
rates are not combinable (5.2.3). A puts the 17 over the 480-requirement baseline (`17/480 = 3.54
%`) and then mislabels a reverse finding as a forward one, the wrong denominator and the wrong test.
C ignores unrequested effort worth 108,800 at build-stage cost. D inverts the finding: an orphan is
work nobody asked for, not a requirement that went missing.

**MCQ 5.2-E `[5.2.2 · Evaluation]`** A requirement reads "the system shall provide faster record
retrieval and improved reporting". Its defects are:
- A. it is untestable only
- B. it is ambiguous ("faster", "improved" — no referent) and compound (two requirements in one) ✅
- C. it is infeasible
- D. it is untraceable

*Rationale:* Comparatives without a referent make it ambiguous and therefore untestable, and the
conjunction makes it two requirements that will be accepted in part and disputed in part (5.2.2).

### Self-check — KA 5.2

1. *Why must the elicitation saving be computed on the difference between stages?* — Because the defect
   still costs 400 to correct at definition; using gross later-stage costs overstates the saving, here
   by 10,000.
2. *Which two requirement classes go missing most reliably, and why?* — Non-functional, because no user
   asks for availability; and transition, because migration, cutover and training belong to somebody
   else.
3. *What does backward traceability detect that forward traceability cannot?* — Unrequested scope: 17
   design elements nobody approved, worth 108,800 at build-stage correction cost, invisible to the
   forward test most programmes run.

---

## Knowledge Area 5.3 — Value definition and prioritisation

*Topics: 5.3.1 attributing value to requirements · 5.3.2 prioritisation methods and their failure
modes · 5.3.3 capacity-constrained selection.*

### 5.3.1 Attributing value to requirements

**The link that closes the chain.** Domain 2 built the benefits map from strategic driver to benefit to
measure and owner. This KA connects the other end: from benefit to the **requirement bundle** that
delivers it. Without that link, prioritisation has nothing to prioritise on and the benefits register
has nothing to hold anyone to.

**Bundles, not individual requirements.** Value is rarely attributable to a single requirement,
because benefits arise from usable capability and a capability needs several requirements to exist
at all. The working unit is therefore the **requirement bundle**: the smallest set of requirements
that together delivers a measurable benefit. Attributing value at the individual requirement level
produces spuriously precise numbers and, worse, invites the delivery of half a capability, which
returns nothing, at full cost.

**Two tests that make attribution honest.** The *sum test*: attributed benefit across all bundles
must not exceed the business case's benefit, and where it falls short the balance must be explained
rather than distributed. Meridian's five Release 1 candidate bundles carry **USD 957,000** of the
**USD 979,200** full-potential annual benefit, and the remaining **USD 22,200** sits with minor
requirements that support no bundle on their own, a statement, not a rounding. The *counterfactual
test*: for each bundle, what happens to the benefit if it is not delivered? A bundle whose absence
costs nothing has no value attributed to it, however popular it is, and a bundle whose absence
collapses several other bundles' benefits is a dependency rather than a candidate.

**Where attribution is legitimately impossible.** Regulatory requirements, safety requirements and
requirements imposed by contract are not prioritisable and should be removed from the value ranking
entirely, held instead as constraints on capacity. Attempting to score them produces the absurdity of a
statutory obligation ranking eleventh, and the process then has to override itself, which discredits
the whole ranking.

### 5.3.2 Prioritisation methods and their failure modes

**The methods, and what each is actually for.** *Ordered ranking* (a strict sequence) is honest and
does not scale past a few dozen items. *Category schemes* (must / should / could / will not, or an
equivalent) scale and are the standard instrument in adaptive delivery (Domain 13), but they degrade
in a specific way described below. *Weighted scoring* against explicit criteria is the Domain 2, KA
2.2.3 instrument applied at requirement level: it forces criteria into the open and remains an ordinal
judgement, so its arithmetic must not be over-read. *Value per unit of effort* is the method that
actually engages the constraint, and 5.3.3 shows both its power and its limit.

**Must-have inflation, quantified.** Category schemes fail because the categorisation is done by the
requirement's originator, and every originator's requirement is a must-have. Of Meridian's **480**
requirements, **374**: **77.92 %**: were marked "must have" on first pass. A structured re-test, in
which each originator was asked to **name the consequence of releasing without it**, left **148**:
**30.83 %**: standing as genuine must-haves. The discretionary pool that prioritisation can actually
act on therefore grows from **106** requirements (**22.08 %**) to **332** (**69.17 %**), a
**3.13-fold** increase in the scope available to trade. That number is the whole argument for the
re-test: at 77.92 % must-have the classification carries almost no information and prioritisation is
theatre, because four requirements in five are outside its reach.

**Two further failure modes worth naming.** *Prioritising by cost rather than by value per unit of
cost*, which systematically favours cheap low-value work and is how a release ships twelve small
conveniences and no capability. And *prioritising without the dependency map*, which produces
sequences that cannot be built: the subject of the caution in 5.3.3 and of Domain 6's logic
networks.

### 5.3.3 Capacity-constrained selection

**The constraint decides the method.** Domain 2, KA 2.2.3 established the principle at portfolio level
and it holds unchanged here: **greedy ranking by value per unit of the scarce resource is a heuristic,
not an optimum, and lumpy candidates require enumerating the feasible sets.** What follows is that
result worked at requirement level, where the effect is larger because requirement bundles are lumpier
than projects and the capacity window is shorter.

**Worked example 5.3.3 — where the ranking loses to the enumeration.**

1. **Setup.** Meridian's Release 1 has **20 development-weeks** of capacity before the first clinic
   wave must begin, a hard constraint set by the clinical training calendar, not by money. Five
   candidate requirement bundles, with their attributed annual benefit and their effort:

   | Bundle | Requirement bundle | Effort (dev-weeks) | Annual benefit (USD) | **Value per week** |
   |---|---|---|---|---|
   | A | Unified patient record and clinical search | 13 | 299,000 | **23,000** |
   | B | e-Prescribing with decision support | 10 | 228,000 | **22,800** |
   | C | Appointments, referrals and clinic workflow | 10 | 225,000 | **22,500** |
   | D | Automated national reporting | 5 | 110,000 | **22,000** |
   | E | Analytics dashboards | 5 | 95,000 | **19,000** |

   Total effort **43** weeks against 20 available; total attributed benefit **957,000** of the
   979,200 full potential.
2. **Formula.** Greedy: rank by value per week, take each bundle that fits the remaining capacity.
   Enumeration: evaluate every subset whose total effort ≤ capacity and take the highest total value.
   Then value the difference: `annual difference × AF(r, n)`, with `AF(0.07, 8) = 5.971299` from
   Domain 2.
3. **Substitution.** Greedy takes **A** (23,000/week, 13 weeks, 7 remaining), skips B and C (10 weeks
   each, do not fit), takes **D** (5 weeks, 2 remaining), and cannot fit E. Enumeration compares every
   feasible subset, of which the material ones are `A+D = 18 weeks / 409,000`, `A+E = 18 / 394,000`,
   `B+D+E = 20 / 433,000`, `C+D+E = 20 / 430,000` and `B+C = 20 / 453,000`.
4. **Result.** Greedy selects **A + D**: **USD 409,000** a year, using **18 of 20** weeks — **90.0
   %** capacity utilisation, with **2 weeks idle and no bundle small enough to use them**.
   Enumeration selects **B + C**: **USD 453,000** a year, using **20 of 20** weeks, **100 %**
   utilisation. The difference is **USD 44,000** a year, **10.76 %** more benefit, worth `44,000 ×
   5.971299 =` **USD 262,737** of present value over the eight-year appraisal period at 7 %.
5. **Interpretation.** The mechanism generalises to every constrained selection a leader will make,
   so it is worth understanding rather than memorising. Greedy fails **because the highest-ratio
   bundle is lumpy**: A leads on value per week by only 200 (23,000 against B's 22,800, a **0.88 %**
   advantage), and taking it strands two weeks that nothing can use. A 0.88 % ratio advantage bought
   a 10.76 % value loss, and the closer the ratios, the likelier that is. The rule follows directly:
   **where the constraint binds and the candidate set is small enough to enumerate, enumerate;
   otherwise rank greedily and then test whether the residual capacity can be filled, because
   stranded capacity is the signature of the failure.** Three cautions, the first severe. The
   enumeration assumes the bundles are **independent**; if C's clinic-workflow requirements depend
   on A's unified record then `B+C` is infeasible, so the dependency map (Domain 6, KA 6.1) is a
   **precondition** for this arithmetic, not a refinement of it, and without it the enumeration
   yields a confident answer that cannot be built. Second, a 44,000 difference between figures of
   409,000 and 453,000 sits well inside the uncertainty of any benefit estimate, so the defensible
   claim is not "B+C is worth 44,000 more" but "the ranking method systematically strands capacity
   and the enumeration does not". The structural argument survives uncertainty that the point figure
   does not. Third, deferring A has a cost the table cannot show: the unified record is the bundle
   clinicians associate with the programme's purpose, and adoption is behavioural (Domain 2's 70 %).
   A sponsor may defensibly overrule the arithmetic on adoption grounds, and should then record that
   they are buying adoption confidence for 44,000 a year, which is a decision, whereas an unexamined
   greedy ranking is not.

> **Fig 5.3.1 — Greedy ranking strands capacity; enumeration does not.** Two horizontal capacity bars,
> each scaled to the 20 development-weeks of Release 1 capacity. The upper bar, "Greedy ranking",
> shows bundle A (unified record, 13 weeks, 299,000) and bundle D (reporting, 5 weeks, 110,000) with a
> dashed crimson outline over the final **2 weeks idle**, totalling **409,000** a year at **18 of 20
> weeks**. The lower bar, "Enumeration", shows bundle B (e-prescribing, 10 weeks, 228,000) and bundle
> C (appointments, 10 weeks, 225,000) filling the bar, totalling **453,000** a year at **20 of 20
> weeks**. A caption line states the difference: **+44,000 a year, 10.76 % more benefit, and USD
> 262,737 of present value over eight years at 7 %**. Source: PCI original. Alt text: two capacity
> bars of equal length, the upper one leaving a dashed empty segment at its right-hand end and carrying
> a lower annual value than the lower bar, which is completely filled.

**What no prioritisation method does.** It does not decide. Ranking and enumeration order candidates
against one stated constraint and one stated value measure; they cannot see option value, sequencing
dependencies, adoption behaviour, political commitment or the strategic cost of not doing something:
the same limits Domain 2, KA 2.2.3 recorded at portfolio level. A release board that treats an
enumeration output as its decision has automated its own accountability, which is Domain 1's
principle failing quietly.

### AI in this KA

**Where it earns its place.** Enumerating feasible subsets under a stated constraint is exactly the
work this KA shows humans getting wrong, and it is deterministic: correct enumeration removes the
greedy failure entirely and the answer is verifiable by re-computation. Constructing the dependency
graph among bundles from the traceability matrix and design register, and reporting which enumerated
sets it makes infeasible. Running the sum test against the business case and reporting the
shortfall. Running the must-have re-test as a first pass by flagging every "must have" whose stated
consequence of omission is absent or circular: how a 77.92 % figure becomes informative. And
sensitivity analysis: how far the benefit estimates must move before the selection changes, which is
more decision-relevant than the point answer.

**Where it must not go.** Attributing benefit. A benefit figure is a claim about the organisation's
future behaviour, owned by a named benefit owner (Domain 2), and a model producing plausible
attributions manufactures the precision the worked example warns is absent. Ruling on must-have status,
which is a stakeholder consequence judgement. Selecting release content, which is a governance decision
with adoption, political and sequencing dimensions no ranking captures. And no model re-weights the
value measure, because whoever controls the weights controls the outcome.

**Verification, concretely.** Both the greedy and the enumerated answer are computed and reported,
because the comparison is the finding: a single reported optimum hides whether the constraint bound
at all. Every enumerated set is checked against the dependency graph, itself confirmed by the
technical owner. The value-per-week arithmetic is reproduced by hand for the leading candidates.
Attributed benefits are confirmed with their owners and their ranges stated. And the discounting
reuses Domain 2's registered `AF(r, n)`, cited rather than re-derived.

### Key terms — KA 5.3

| Term | Meaning |
|---|---|
| **Requirement bundle** | The smallest set of requirements that together delivers a measurable benefit, the unit of value attribution. |
| **Sum test** | Attributed benefit across bundles must not exceed the business case, and any shortfall is explained, not distributed. |
| **Counterfactual test** | What happens to the benefit if this bundle is not delivered? A zero answer means zero attributed value. |
| **Must-have inflation** | Originator-assigned categories collapsing into near-universal "must have", destroying the scheme's information content. |
| **Value per unit of constrained effort** | Attributed benefit ÷ effort in the scarce resource: the ranking that engages the constraint. |
| **Greedy ranking** | Taking candidates in ratio order while they fit; a heuristic that strands capacity when candidates are lumpy. |
| **Enumeration** | Evaluating every feasible subset under the constraint; the optimum where the candidate set is small enough. |
| **Stranded capacity** | Unused constrained resource that no remaining candidate is small enough to use, the signature of greedy failure. |

### Sample MCQs — KA 5.3

**MCQ 5.3-A `[5.3.3 · Application]`** With 20 development-weeks available and bundles A (13 wk,
299,000), B (10 wk, 228,000), C (10 wk, 225,000), D (5 wk, 110,000) and E (5 wk, 95,000), greedy
ranking by value per week selects:
- A. A + D, worth 409,000 a year ✅
- B. B + C, worth 453,000 a year
- C. A + B, worth 527,000 a year
- D. B + D + E, worth 433,000 a year

*Rationale:* Greedy takes A first (23,000/week), then D, stranding 2 weeks — 409,000 (5.3.3). B is the
enumerated optimum, not the greedy result; C exceeds capacity at 23 weeks; D is a feasible set greedy
never reaches.

**MCQ 5.3-B `[5.3.3 · Analysis]`** In that selection, greedy loses to enumeration because:
- A. value per week is the wrong measure
- B. the highest-ratio bundle is lumpy and strands 2 of 20 weeks that nothing can fill ✅
- C. bundle A's benefit is overstated
- D. the capacity constraint is not binding

*Rationale:* A's ratio advantage is under 1 % and its size wastes 10 % of capacity (5.3.3). The measure
is not wrong; it is a heuristic, and the constraint is precisely what makes it fail.

**MCQ 5.3-C `[5.3.3 · Evaluation]`** Before the enumeration above can be relied upon, the
indispensable additional input is:
- A. a probability distribution on each benefit estimate
- B. the dependency map among bundles, since an infeasible set must not be selected ✅
- C. the sponsor's ranking preference
- D. a re-test of the must-have classification

*Rationale:* If C depends on A, `B+C` cannot be built and the answer is confidently wrong (5.3.3);
dependency feasibility is a precondition, not a refinement. A and D are valuable and not
indispensable.

**MCQ 5.3-D `[5.3.2 · Analysis]`** 374 of 480 requirements are marked "must have". The consequence for
prioritisation is that:
- A. the project is unusually well specified
- B. the discretionary pool is 106 requirements — 22.08 % — so prioritisation can act on a fifth of the
  scope, and the classification carries almost no information ✅
- C. the must-haves should be delivered first in ranked order
- D. capacity must be increased

*Rationale:* At 77.92 % must-have the category no longer discriminates; the re-test that asks for the
consequence of omission raises the actionable pool to 332, a 3.13-fold increase (5.3.2).

**MCQ 5.3-E `[5.3.1 · Comprehension]`** Regulatory and safety requirements should be:
- A. scored with the highest weight in the value model
- B. removed from the value ranking and held as constraints on capacity ✅
- C. prioritised last, since they generate no benefit
- D. attributed a nominal benefit for completeness

*Rationale:* They are not prioritisable; scoring them produces a statutory obligation ranking eleventh
and forces the process to override itself, discrediting the ranking (5.3.1).

### Self-check — KA 5.3

1. *Why is value attributed to bundles rather than to individual requirements?* — Because benefit
   arises from usable capability; attributing at requirement level manufactures precision and invites
   delivery of half a capability, which returns nothing at full cost.
2. *What is the signature of a greedy prioritisation failure?* — Stranded capacity: 2 of 20 weeks idle
   with no candidate small enough to fill them, costing 44,000 a year and 262,737 of present value.
3. *What must be true before an enumeration is trustworthy?* — The sets enumerated must be feasible
   against the dependency map; otherwise the optimum cannot be built.

---

## Knowledge Area 5.4 — Scope change, creep and verification/acceptance

*Topics: 5.4.1 change versus creep · 5.4.2 acceptance criteria and testability · 5.4.3 verification,
validation and acceptance.*

### 5.4.1 Change versus creep

**The distinction, stated precisely.** A **change** is a movement of the scope baseline that has
been raised, screened, assessed, decided by an authority and recorded — Domain 4's flow, and Domain
4 quantified what its accumulation costs. **Creep** is a movement of the delivered content with **no
corresponding baseline movement and no record**. The two are not degrees of the same thing. Change
is a control operating; creep is the absence of one, and it is invisible to every instrument built
to monitor change, including Domain 4's cumulative test, which reads a change log that by definition
does not contain it.

**The three routes creep takes**, all of them familiar and none of them dishonest at the point of
entry. *Misclassification as clarification*: the requester says the baseline already covered it, the
screening step agrees without testing the claim against the requirement text, and scope grows with a
paper trail that records no growth, the mirror error Domain 4, KA 4.4.1 named. *Acceptance-criteria
expansion*: the requirement text is unchanged and its acceptance criterion grows, which is a scope
change wearing a testing costume and is the commonest route where criteria are weak (5.4.2).
*Undocumented addition*: someone with access and good intentions adds capability, a clinician asks a
developer directly, or a developer improves something while in the code. This last is
"gold-plating", and 5.2.3's reverse traceability test is the only cheap way to see it.

**Worked example 5.4.1 — the movement that left no trace in the change log.**

1. **Setup.** Meridian's requirements baseline at design freeze was **480** approved requirements.
   Through the following 13 months, integrated change control approved **12** requirement additions. At
   entry to acceptance testing the traceability matrix carries **531** requirements. The average direct
   build cost of an uncontrolled requirement is **USD 4,200**, and **16** of them consumed critical-path
   time averaging **0.25 weeks** each. Cost of delay **USD 14,280** per week; baseline
   **USD 2,400,000**. Domain 4, KA 4.3.3 separately established **USD 291,176** of *authorised*
   baseline drift over the programme's first year, so the two figures set beside each other are
   Meridian's cumulative baseline movement to date rather than two readings of one identical window.
2. **Formula.** **Requirement-count reconciliation:** baseline count + approved additions = expected
   traced count; actual − expected = unexplained requirements. Creep cost = (unexplained × average
   direct cost) + (affected count × average weeks × cost of delay). Then combine with the authorised
   drift and compute what share of total movement the change log captured.
3. **Substitution.** `480 + 12 = 492`; `531 − 492`; `39 × 4,200`; `16 × 0.25 × 14,280`;
   `291,176 + 220,920`; `291,176 / 512,096`.
4. **Result.** **39** requirements are present in the delivered content and absent from every
   record: about **3 a month**, and **3.25 times** the number that came through change control. They
   cost **USD 163,800** direct plus `4.0` weeks of critical path at **USD 57,120**, a total of **USD
   220,920**: **9.20 %** of the baseline, at **USD 5,664.62** each. Combined with Domain 4's
   authorised drift, Meridian's total baseline movement is **USD 512,096**, or **21.34 %** of the
   approved baseline, of which the change log accounts for only **56.86 %**.
5. **Interpretation.** The decisive number is **56.86 %**, and it should change how a leader reads a
   change report. Meridian had a working change process, a decision log and (after Domain 4) a
   cumulative test derived from its own change rate, and all of that machinery was monitoring a
   little over half the movement it existed to control. The other 43 % was not hidden; it was simply
   never a change, because nobody raised it. Each crept requirement carries **USD 4,200** of direct
   cost (**0.175 %** of the baseline, the basis on which a delegation threshold reads a single
   item), so no threshold in Domain 3's delegation schedule can see one, and no cumulative test can
   aggregate what its log does not contain. Which gives the instrument, and it is almost
   embarrassingly cheap: **count the requirements.** Baseline count plus approved additions must
   equal the traced count, every reporting period, one addition and one subtraction against two
   registers that already exist, and the only control in this book that detects creep. Two further
   readings. Against Domain 2's approved NPV of **+USD 1,332,898**, total scope movement of 512,096
   consumed **38.42 %** of the programme's whole justification, leaving **USD 820,802**, still
   positive, which is exactly why nobody stopped: creep is survivable right up until it is not. And
   a caution on the arithmetic: 4,200 is an average over requirements of very different size, and
   the 16-of-39 critical-path attribution comes from the schedule rather than the change log,
   carrying the schedule's own uncertainty (Domain 6). Report the 220,920 as an estimate with a
   stated basis and the **39** as a fact; because the count is a fact, and the count is what
   produces the action.

### 5.4.2 Acceptance criteria and testability

**Definition.** An acceptance criterion states the **condition** under which the deliverable will be
judged to meet the requirement, in terms of an observable and measurable result. It is written when the
requirement is written, by the requirement's owner with the tester present, because a criterion written
later is written by whoever needs the deliverable accepted.

**The three states a criterion can be in**, and the middle one is the dangerous one:

| State | Appearance | Consequence |
|---|---|---|
| **Measurable** | States a condition, an action and an observable result with a threshold | Acceptance is a test |
| **Restated** | Reads like a criterion, restates the requirement in other words ("the system shall retrieve records correctly") | Acceptance is an argument, and the gap is invisible until it happens |
| **Absent** | No criterion at all | Acceptance is an argument, and the gap is visible now |

**Worked example 5.4.2 — what it is worth to write a testable criterion.**

1. **Setup.** Of Meridian's **480** requirements, **382** carry a measurable acceptance criterion,
   **71** carry a criterion that restates the requirement, and **27** carry none. Writing a measurable
   criterion, with a reviewer, costs **USD 320** of analyst and tester time. The organisation's history
   is that a deficient criterion produces a formal acceptance dispute in about **1 case in 4**; a
   dispute costs about **USD 9,600** in rework, re-test and clinical re-review; and about **1 dispute
   in 3** delays a clinic go-live by **0.5 weeks** at the cost of delay of **USD 14,280** per week.
2. **Formula.** Remediation cost = deficient count × cost per criterion. Expected cost per dispute =
   rework + (share delaying × weeks × cost of delay). Expected dispute cost = deficient count ×
   P(dispute) × cost per dispute. Breakeven probability = remediation cost ÷ (deficient count × cost
   per dispute).
3. **Substitution.** `(71 + 27) × 320`; `9,600 + (1/3 × 0.5 × 14,280)`; `98 × 0.25 × 11,980`;
   `31,360 / (98 × 11,980)`.
4. **Result.** **98** requirements are deficient — **20.42 %** of the baseline, comprising **14.79 %**
   restated and **5.62 %** absent (the two class rates rounding to 20.41 against the combined 20.42 —
   display rounding, as in 5.2.3, and one more reason the classes and not the total are the report).
   Remediating all of them costs **USD 31,360**. Expected cost per
   dispute is **USD 11,980** (9,600 of rework plus **USD 2,380** of expected delay). Expected disputes
   are **24.5**, at an expected cost of **USD 293,510**. Remediation therefore nets **USD 262,150**, a
   return of **9.36 times**. And the **breakeven dispute probability is 2.67 %** against an observed
   **25 %**.
5. **Interpretation.** The breakeven settles the argument this work always loses. Writing testable
   acceptance criteria pays for itself if deficient criteria cause disputes more than **2.67 %** of
   the time; the observed rate is nine times that, and an organisation would have to believe its
   acceptance process almost frictionless before declining to spend 320 a requirement. Note next
   which class is worse, because the count says the opposite of the truth. The **27 absent**
   criteria are a visible gap: they appear on every completeness report and someone raises them. The
   **71 restated** criteria look complete, pass every completeness check, and are discovered at
   acceptance (deliverable built, clinic scheduled, supplier invoicing), which is Domain 4's
   "recorded state differs from actual state" failure moved from configuration to acceptance, and
   the class WE 5.2.3's nine unverified acceptances came from. **A restated criterion is worse than
   a missing one, and it is commoner.** Three cautions. The 24.5 is an expectation, not a forecast
   of 24 or 25 events, and must not be reported as a count of disputes that will happen. The rates
   and costs are the organisation's own history; a programme without that history should say it is
   using assumed rates and test the breakeven's sensitivity rather than the point result, and here
   the breakeven sits so far below the observed rate that the conclusion survives any plausible
   error in either. And remediation is not free of schedule: 98 criteria at four a day is about five
   weeks of one analyst's time, which has to be planned rather than absorbed, or it will not happen.

### 5.4.3 Verification, validation and acceptance

**The distinction that Case study B turns on.** **Verification** asks *did we build what we
specified?* **Validation** asks *does what we built produce the outcome we needed?* They are
different questions with different evidence and, critically, different failure signatures: a project
can pass verification completely and fail validation completely, and it will report itself as a
success throughout, because verification is the test it has instrumented. Validation requires the
receiving organisation and real use, which is why it belongs to handover and benefits realisation
(Domain 16) as well as here, and why Domain 1 insisted the leader is answerable for the outcome and
not only the output.

**Acceptance** is the authorised act of taking a deliverable as meeting its criteria. It requires a
named acceptor with authority, evidence against each criterion, and a recorded decision. Two
provisions make it real rather than ceremonial: the acceptor is not the producer, and the evidence
is retained; because the nine requirements of WE 5.2.3 were "accepted" and the record could not say
by whom, on what evidence, or against what criterion.

**"Retained" needs three fields, or it is a wish.** Acceptance evidence is the primary evidence in
any later dispute, audit or defect claim, and those arrive years after the acceptance meeting, so
the record carries a **record class**, a **named custodian role** and a **retention period with the
source of that period named**: set at the longest of the contractual limitation period, any
retention requirement the organisation is subject to, the benefits-realisation horizon and the
organisation's records policy. Domain 3, KA 3.3.4 sets the custody machinery and Domain 16, KA
16.4.4 works the economics; the applicable retention requirements are jurisdiction- and
sector-specific and come from the records and legal functions, not from this book. Custody transfers
to a named continuing role at closure, because the acceptor's project will not exist when the
evidence is asked for.

**Conditional acceptance, and its price.** Deliverables are routinely accepted with open items, and
this is often correct: waiting for perfection forgoes benefit at Meridian's 14,280 a week. But
deferring an item moves it up the correction ladder of 5.2.1, and that movement is computable.

**Worked example 5.4.3 — deferring six severity-one items.**

1. **Setup.** At Meridian's Release 1 acceptance there are **41** open items, of which **6** are
   severity one, a defect against a requirement that materially affects clinical use. Fixing them
   before acceptance costs **USD 6,400** each on the correction ladder (build stage) and delays
   go-live by **1.5 weeks**. Deferring them to live service costs **USD 102,400** each. Cost of
   delay **USD 14,280** per week.
2. **Formula.** Fix-first cost = items × build-stage cost + delay weeks × cost of delay. Defer cost =
   items × live-service cost. Breakeven go-live delay = (defer cost − items × build-stage cost) ÷ cost
   of delay.
3. **Substitution.** `6 × 6,400 + 1.5 × 14,280`; `6 × 102,400`; `(614,400 − 38,400) / 14,280`.
4. **Result.** Fixing first costs **USD 59,820** (38,400 of correction plus **USD 21,420** of
   delay). Deferring costs **USD 614,400**: **10.27 times** as much, a difference of **USD
   554,580**. Go-live would have to be delayed by **more than 40.34 weeks** before deferral became
   the cheaper option.
5. **Interpretation.** The 40.34-week breakeven makes this decision arithmetic rather than
   temperament, which is why it is worth computing in the room: the instinct at acceptance is that a
   1.5-week delay is unaffordable, and the tolerable delay is about twenty-seven times that. But the
   result holds **per severity class, not per item**, and generalising it is the error to avoid. The
   **35** severity-two and severity-three items may be entirely correct to defer, because the
   live-service figure on the ladder prices correcting a *requirement* defect in a running clinical
   service (regression, re-release, re-training, clinical re-approval), and a cosmetic or
   low-frequency item does not incur most of that. A programme that applies the top of the ladder to
   all 41 items will refuse to go live at all, forgoing 14,280 a week for items worth a fraction of
   it. Two further points. The go-live delay already prices forgone benefit through the registered
   cost of delay (Domain 1), so a separate benefit-loss term would double-count. And the arithmetic
   assumes the six items *can* be fixed in 1.5 weeks; if they cannot, the question is not
   fix-versus-defer but what Release 1 contains, which belongs to KA 5.3, not to an acceptance
   meeting under time pressure.

### AI in this KA

**Where it earns its place.** The requirement-count reconciliation of WE 5.4.1, run every period and
producing the **list** of unexplained requirements rather than only the number. This is the domain's
highest-value automation: the check is trivially cheap, nobody runs it, and it catches the 43 % of
movement no change-log instrument can see. Classifying acceptance criteria as measurable, restated
or absent: a linguistic test with a definite answer, and how 71 invisible criteria become visible
before acceptance rather than during it. Detecting acceptance-criteria expansion by diffing
criterion text against its baselined version, which is the creep route that leaves the requirement
text untouched. Assembling the evidence pack per criterion and listing criteria with none. And
applying the correction ladder by severity class to an open-item list to produce the
fix-versus-defer comparison for decision.

**Where it must not go.** Accepting a deliverable: an authorised act by a named human who is not the
producer. Ruling that a request is a clarification rather than a change, the misclassification route
one of creep depends on, and a determination with commercial consequences. Deciding which open items
may be deferred, which here is a clinical, safety and commercial judgement. And no AI-generated text
may serve as acceptance evidence: a model asserting that a criterion is met is not evidence that it
is, and treating it as such recreates the defect WE 5.2.3 found nine instances of.

**Verification, concretely.** Every unexplained requirement is investigated by a human before it is
reported as creep, because some prove to be recorded changes with a broken reference, itself a
finding, of a different kind. Criterion classifications are sampled and confirmed by the tester,
particularly the restated class, since that judgement is the one with money attached. The
fix-versus-defer arithmetic is reproduced by hand with the severity class stated for each item,
because applying the live-service figure to a cosmetic defect is the error a tool makes silently.
And every acceptance decision names its human acceptor, with any AI contribution recorded as
analysis input.

### Key terms — KA 5.4

| Term | Meaning |
|---|---|
| **Change** | An assessed, authorised, recorded movement of the scope baseline (Domain 4, KA 4.4). |
| **Scope creep** | Movement of delivered content with no baseline movement and no record; invisible to every change-log instrument. |
| **Requirement-count reconciliation** | Baseline count + approved additions = traced count; the only cheap control that detects creep. |
| **Acceptance-criteria expansion** | Creep entering through a growing criterion while the requirement text is unchanged. |
| **Restated criterion** | A criterion that repeats the requirement instead of stating a measurable condition: worse than an absent one, and commoner. |
| **Breakeven dispute probability** | The dispute rate at which writing testable criteria exactly pays for itself; **2.67 %** at Meridian against an observed 25 %. |
| **Verification** | Did we build what we specified? |
| **Validation** | Does what we built produce the outcome we needed? A project can pass the first completely and fail the second completely. |
| **Conditional acceptance** | Acceptance with open items; priced by moving each item up the correction ladder **by severity class**. |

### Sample MCQs — KA 5.4

**MCQ 5.4-A `[5.4.1 · Application]`** A baseline of 480 requirements received 12 approved additions;
the traceability matrix at acceptance-test entry carries 531. Uncontrolled requirements average 4,200
direct, and 16 of them consumed 0.25 weeks of critical path each at a delay cost of 14,280 per week.
Total creep cost is:
- A. USD 163,800
- B. USD 220,920 ✅
- C. USD 214,200
- D. USD 57,120

*Rationale:* `531 − (480 + 12) = 39` unexplained; `39 × 4,200 = 163,800` plus
`16 × 0.25 × 14,280 = 57,120` gives 220,920 (5.4.1). A omits the schedule impact; C prices all 51
additions including the 12 that were properly approved; D is the schedule impact alone.

**MCQ 5.4-B `[5.4.1 · Evaluation]`** Domain 4 established USD 291,176 of authorised baseline drift over
the programme's first year, giving cumulative movement of USD 512,096 against a USD 2,400,000 baseline.
The most important consequence of the creep figure is that:
- A. total movement is 21.34 % of the baseline
- B. the change log captures only 56.86 % of the movement it exists to control, and no cumulative test
  on that log can see the rest ✅
- C. the average crept requirement cost 5,664.62
- D. the delegation threshold should be lowered below 4,200

*Rationale:* Creep leaves no change record, so change-log instruments, including Domain 4's
cumulative test, monitor a little over half the movement (5.4.1); the remedy is the count
reconciliation. A and C are true and subordinate; D cannot work, since no change is ever raised.

**MCQ 5.4-C `[5.4.2 · Application]`** Remediating 98 deficient criteria costs 320 each. A deficient
criterion causes a dispute 25 % of the time, at an expected cost of 11,980 per dispute. The breakeven
dispute probability is:
- A. 25.00 %
- B. 2.67 % ✅
- C. 9.36 %
- D. 20.42 %

*Rationale:* `31,360 / (98 × 11,980) = 2.67 %` (5.4.2). A is the observed rate; C is the return
multiple; D is the share of the baseline that is deficient.

**MCQ 5.4-D `[5.4.3 · Evaluation]`** Six severity-one items cost 6,400 each to fix before acceptance,
with a 1.5-week go-live delay at 14,280 per week, or 102,400 each in live service. The result and its
limit:
- A. defer; the go-live delay is unaffordable
- B. fix first — it costs 59,820 against 614,400, and the breakeven delay is 40.34 weeks — but the
  ladder must be applied by severity class, not to all 41 open items ✅
- C. fix first, and apply the same reasoning to all 41 open items
- D. the comparison cannot be made without the benefit forgone during the delay

*Rationale:* Fix-first is cheaper by 554,580 and would remain so up to a 40.34-week delay (5.4.3);
applying the live-service figure to minor items would prevent go-live altogether. D double-counts:
the cost of delay already prices forgone benefit (Domain 1).

**MCQ 5.4-E `[5.4.3 · Comprehension]`** A programme reports 100 % of requirements verified and its
benefits are 34 % of forecast. This is:
- A. a verification failure
- B. a validation failure — the specified thing was built, and it did not produce the outcome ✅
- C. a benefits-measurement error
- D. evidence of scope creep

*Rationale:* Verification asks whether the specified thing was built and validation whether it
produces the outcome; the two fail independently (5.4.3), which is Case study B.

### Self-check — KA 5.4

1. *Why can no cumulative change test detect creep?* — Because creep generates no change record; the
   test reads a log that by definition does not contain it. Only a count reconciliation catches it.
2. *Which acceptance-criterion class is most dangerous, and why?* — The restated criterion: it passes
   completeness checks and is discovered at acceptance, with the deliverable built and the supplier
   invoicing.
3. *What does the 40.34-week breakeven in WE 5.4.3 tell an acceptance meeting?* — That the instinct
   "we cannot afford 1.5 weeks" is wrong by a factor of about twenty-seven for severity-one items,
   and that the same arithmetic does not license fixing everything.

---

## Advanced topics — Domain 5

### 5.A.1 Scope in adaptive and hybrid delivery

The obvious objection to this domain is that adaptive delivery deliberately does not fix scope, so a
scope baseline is the wrong instrument. The objection is half right and the conclusion is wrong.
What adaptive delivery declines to fix is the **item list**; what it must still fix (more strictly
than predictive delivery) is the **capacity, the cadence and the value envelope**. A backlog is not
an absent baseline but a baseline on different variables, and Domain 13 works the mechanics.

Three of this domain's controls become *more* important, and they are the three adaptive programmes
most often drop as belonging to a predictive method. **Value attribution** (KA 5.3.1), because
iteration decisions are prioritisation decisions taken every few weeks and an unattributed backlog is
prioritised on advocacy. **Acceptance criteria** (KA 5.4.2), because the definition of done is the only
acceptance instrument available and a restated criterion fails inside a sprint exactly as it fails at a
formal gate, only faster. And **backward traceability** (KA 5.2.3), because the shorter the path from
idea to build, the easier it is for an item nobody approved to be delivered.

What genuinely changes is the creep instrument: a requirement-count reconciliation is meaningless
where the count is supposed to move. The equivalent is a **value-envelope reconciliation**,
attributed benefit of delivered items against the business case's benefit commitment, period by
period. It detects the failure adaptive programmes actually suffer, which is not item growth but a
backlog that has drifted away from the benefits it was funded to produce: delivering steadily,
validating badly. Hybrid delivery needs both, applied to the parts that are each kind, with a named
boundary between them, a hybrid programme running one control regime has silently chosen one method.

### 5.A.2 Requirements across a contractual boundary

Where the specification is written by one party and built by another, requirement quality becomes an
allocation of risk, and the allocation is usually made accidentally, by the choice of specification
style. An **output specification** states what the supplier must deliver, and the buyer retains the
risk that delivering it produces no outcome: verification passes, validation fails, and there is no
claim because the supplier built what was specified. An **outcome specification** transfers more of
that risk to a supplier who will price it and who cannot control the buyer's own enabling change, so
an outcome specification carrying no buyer obligations is either unpriceable or priced with a large
margin. Neither is right in general; what is always wrong is not knowing which one has been signed.

Four provisions follow, and they belong in the contract rather than a plan (Domain 10; PFL-AI Domain
11). Requirement defects and their remedy are **allocated explicitly**, who bears the cost when a
requirement turns out to be wrong rather than badly built, because on the ladder of 5.2.1 that
difference is 6,400 against 102,400 and it will be argued. Acceptance criteria are **agreed and
baselined before signature**, since a criterion negotiated after award is negotiated by the party
who wants payment. **Traceability is a deliverable**, with a stated format and an audit right, or
the buyer cannot run either test in KA 5.2.3 against work it did not do. And the buyer's own
obligations (data, access, decisions within stated times, where Domain 3's `E[wait]` becomes a
contractual exposure) are stated with the specificity demanded of the supplier, because an unstated
buyer obligation is a common root of a claim.

Two boundaries on this section. Whether a given specification style, remedy or acceptance provision has
the effect described depends on the contract's governing law and on the form of agreement used, and
those differ materially between jurisdictions and between standard forms; nothing here states the law of
any jurisdiction and none of it is legal advice. Drafting, risk allocation and any remedy question go to
qualified counsel (Domain 10). What is portable is the *method*: decide which specification style you
have signed, allocate requirement-defect risk deliberately rather than by default, baseline the
acceptance criteria before signature, make traceability a deliverable, and state the buyer's own
obligations as precisely as the supplier's.

### 5.A.3 The reviewer's scope eye

Invariants to test on any scope and requirements position, each cheap and each diagnostic.

The scope statement's extents are **counted, not categorised**, and every category noun has a number
against it. The exclusion list exists and is **non-empty**: an empty exclusion list means the
boundary questions have not been asked, not that there are none. Every assumption has an owner and a
resolution date. The **WBS dictionary has content**, not only titles, and carries the acceptance
basis. Every requirement carries a **class**, and the non-functional and transition classes are
**present at all**; their total absence is the single fastest defect to spot and among the most
expensive. Every requirement is **singular and testable**, and the acceptance criteria have been
classified as measurable, restated or absent: with the restated count reported, because it is the
one nobody has. **Traceability is audited in both directions**, reported **by class and never
totalled**, with the orphan list produced; a programme that has never run the reverse test does not
know what it is building. Every requirement bundle has an **attributed benefit with a named owner**,
the attribution **passes the sum test** against the business case, and regulatory requirements are
**held outside the value ranking** as constraints. Where a capacity constraint binds, both the
greedy and the enumerated selections are computed, the **dependency map** has been used to exclude
infeasible sets, and any **stranded capacity** is reported as a finding. The **requirement-count
reconciliation** is run every reporting period (baseline plus approved additions against the traced
count), and its unexplained list is investigated, not just counted. Criterion text is **diffed
against its baselined version**, because that is where creep hides. And on any conditional
acceptance, the correction ladder has been applied **by severity class** with the fix-versus-defer
breakeven stated, and the acceptor is not the producer.

---

## Industry variations — Domain 5

- **Construction and infrastructure.** Scope is employer's requirements plus a design-responsibility
  allocation, and the highest-value line states **who owns the design risk for each element**: a
  defect in a specification the contractor was told to design to is a claim, so the ladder of 5.2.1
  becomes a commercial argument rather than an internal cost. Physical scope also carries an
  irreversibility the software cases lack: a wall in the wrong place is not a build-stage defect.
- **Software and digital.** The backlog carries the scope and the definition of done carries the
  criteria, so 5.A.1's transfers apply and the top of the correction ladder genuinely compresses under
  continuous deployment, which is the honest argument for it. What does not compress is the cost of a
  *requirement* being wrong: a fast cadence ships the wrong thing sooner.
- **Regulated manufacturing (pharmaceutical, medical devices, aerospace).** Bidirectional
  traceability from requirement through design and verification to release evidence is a regulatory
  obligation with an inspection consequence, so the nine unverified acceptances of WE 5.2.3 are a
  finding against the quality system rather than a project defect. Requirements are frozen at
  defined points, making late elicitation expensive by design, which raises the KA 5.2.1 return well
  above Meridian's.
- **Public-sector programmes.** Requirements originate partly in policy and statute, so a material
  share of the baseline is non-prioritisable (5.3.1) and the discretionary pool is smaller than the
  count suggests. Consultation obligations make elicitation a legal process with a published record;
  the systematic omission is the **external user**, who attends no internal meeting, Case study B.
- **Defence and complex systems.** Requirements are verified against a formal cross-reference of
  requirement to verification method and evidence, over lifecycles long enough that the author has left.
  Traceability is the institutional memory, so a 4 % orphan rate is a configuration problem rather than
  an untidiness.
- **Healthcare.** Clinical safety requirements are bound to a safety case and to specific system
  versions (Domain 4's configuration point), so acceptance-criteria expansion is a safety-case
  change and the restated-criterion class of 5.4.2 carries clinical rather than merely commercial
  consequence, which makes Meridian's 71 restated criteria a clinical-governance finding.

---

## Case study — Domain 5: the requirement nobody could test (health, Meridian)

**Situation.** Meridian's Release 1 acceptance ran eight weeks against a planned three. The programme
reported "acceptance complexity" and requested a schedule change; the supplier reported that all
delivered functionality met the specification. Both statements were true, and the assurance review
found the mechanism.

**What the review found.** Of the 480 approved requirements, **98**, **20.42 %**, carried acceptance
criteria that could not settle a dispute: **71** restated the requirement and **27** had none.
Twenty-two of the 98 reached formal dispute during acceptance, against the **24.5** the
organisation's own dispute rate predicted, and six of those delayed a clinic go-live. The
traceability audit found the corollaries: **34** requirements with no design element, **21** with no
test case and **9** recorded as accepted with no verification evidence, meaning the release
recommendation had been assembled partly from a record that stated something untrue. The reverse
test, run for the first time, found **17** of the 425 design elements tracing to no approved
requirement.

**What the arithmetic had already said, and nobody had computed.** Remediating all 98 criteria would
have cost `98 × 320 =` **USD 31,360** against an expected dispute cost of **USD 293,510**: a
**9.36-times** return on a **breakeven dispute probability of 2.67 %** against an observed 25 %. The
71 restated criteria had passed every completeness check the programme ran, which is why the
deficiency reached acceptance intact: the programme measured whether criteria **existed**, never
whether they **said anything**.

**How it resolved.** Acceptance was suspended for four weeks (`4 × 14,280 =` **USD 57,120**), while
criteria were rewritten for the 41 requirements still in dispute or awaiting test, at `41 × 320 =`
**USD 13,120**. Six severity-one open items were fixed rather than deferred, at **USD 59,820**
against a deferral cost of **USD 614,400** (WE 5.4.3). The nine unverified acceptances were reopened
and verified. Of the 17 orphans, 11 were retained as retrospectively approved changes and 6 removed.
And the requirement-count reconciliation was run for the first time, exposing the **39** crept
requirements and **USD 220,920** of unrecorded movement that WE 5.4.1 quantifies.

**What the domain teaches here.** A criterion that exists and says nothing is worse than one that is
missing, because the missing one appears on a report and the empty one does not: the programme's own
completeness metric was the instrument that concealed it. Finding out at acceptance cost **57,120**
of suspension plus 22 disputes; preventing it would have cost **31,360**, decided a year earlier by
an analyst nobody would have escalated to. The second lesson is procedural: three controls
(criterion classification, reverse traceability, count reconciliation) had never been run once, and
each cost less than a day.

## Case study B — Domain 5: one hundred per cent complete and thirty-four per cent useful (public sector)

**Situation.** A national licensing authority replaced its permit-application platform. At closure
the programme reported **612** requirements delivered, **100 %** verified, on budget and two weeks
early. Fourteen months later the benefits review found realised annual benefit of **USD 391,000**
against a forecast **USD 1,150,000**: **34.0 %**. Nothing in the delivery record was untrue.

**Why.** Requirements had been elicited thoroughly, over four months, from a working group of **9**
internal officers. External applicants (about **4,200** a year, the people whose behaviour the
entire benefit case depended on) were not consulted: they were not employees and there was no
mechanism to convene them. Of the 612 requirements, **47**, **7.68 %**, addressed the applicant's
own journey; the rest described the officers' work. The platform therefore automated the authority's
processing beautifully and left applicants submitting on paper, so the digital-channel adoption on
which **78 %** of the forecast benefit rested barely arrived. The decomposition is worth stating,
because it is why the shortfall was 66 points and not 78: **USD 897,000** of the forecast depended
on digital adoption and **USD 253,000** did not, so a realised 391,000 implies that (taking the
non-digital benefit as landing in full) only **USD 138,000** of the digital-dependent benefit,
**15.4 %** of it, was ever delivered. Verification was complete because the specification was
internally coherent; validation had never been attempted, because nobody had defined what a
validated outcome looked like from outside the organisation.

**How it resolved.** Re-elicitation with applicant panels and observation of real submission
attempts produced **63** further requirements, of which **41** were implemented as changes costing
**USD 285,000**. Realised benefit reached **USD 816,500** (**71.0 %** of forecast, an uplift of
**USD 425,500** a year, a payback of **0.67 years) about 8.0 months**. The authority's standing
corrections: every benefit must name the population whose behaviour produces it, and that population
must appear in the elicitation record; and closure requires a **validation** statement, not only a
verification one.

**What the domain teaches here.** A hundred per cent verification rate against a specification
nobody outside the organisation contributed to measures internal consistency, and it is routinely
reported as success. The requirement worth 425,500 a year was available for the price of a
conversation before the specification was frozen and cost 285,000 to add afterwards, entirely
consistent with the ladder of KA 5.2.1. And the warning generalises past this sector: **the
stakeholder group that produces the benefit is frequently the one with no seat in the requirements
process**, because elicitation follows the organisational chart and benefits do not.

---

## Executive perspective — Domain 5

What a programme director cannot delegate in this domain:

- **The boundary, stated as counts.** Every extent in your scope statement carries a number, and the
  exclusion list is non-empty. Meridian's undrawn boundary was worth **143,000**. And and the right
  answer was to include the eleven sites, at a nine-month payback, which only the written boundary
  would have surfaced in time (5.1.3).
- **Who was in the room.** Requirement loss is not random: it clusters on operators, supporters,
  external users and whoever owns the enabling change. A licensing platform verified at **100 %**
  delivered **34 %** of its benefit because **4,200** applicants a year had no seat (Case study B).
- **The correction ladder, and that you have bought elicitation against it.** One defect moved out of
  live service saves **102,000** and pays for an **84,000** programme; the delay objection breaks even
  at **36.89 weeks** (5.2.1).
- **That the reverse traceability test has been run.** It costs one query, it is the only cheap detector
  of work nobody asked for, and Meridian's first run found **17** orphans worth **108,800** (5.2.3).
- **That capacity is not stranded.** Where a constraint binds, require both the ranked and the
  enumerated answer. The intuitive method left **2 of 20** weeks idle and lost **44,000** a year,
  **262,737** of present value (5.3.3).
- **The count reconciliation, monthly.** Baseline requirements plus approved additions against the
  traced count. Meridian's change log was monitoring **56.86 %** of the movement it existed to control;
  the missing **39** requirements cost **220,920** and no threshold could ever have seen one (5.4.1).
- **A validation statement at closure, not only a verification one.** They fail independently, and only
  one of them is the question the organisation asked (5.4.3).

---

## Calculation exercises — Domain 5

**Exercise 5.1** A traceability audit covers 640 approved requirements and a 578-element design
register. It finds 51 requirements with no design element, 38 with no test case, and 12 recorded as
accepted with no verification evidence; 23 design elements trace to no approved requirement. Compute
the forward defect rate overall and by class, and the reverse defect rate. State which class is most
serious and why. *Solution.* Forward non-conformances `51 + 38 + 12 =` **101**, a rate of `101/640
=` **15.78 %**, comprising **7.97 %** with no design, **5.94 %** with no test and **1.88 %**
accepted without evidence. Reverse rate `23/578 =` **3.98 %**. The 12 are most serious: the
acceptance record states something untrue, so every decision taken on it, including release, rested
on nothing. *Common error:* combining the forward and reverse rates, or presenting the forward
classes as a single 15.78 % figure. The denominators differ (640 requirements against 578 design
elements) and the classes fail differently, so neither total is informative.

**Exercise 5.2** An organisation's observed correction ladder is 500 / 2,000 / 8,000 / 32,000 /
128,000 per requirement defect by stage (definition, design, build, test, live). A review costing
USD 62,000 is expected to move 11 build-stage, 8 test-stage and 2 live-service defects to the
definition stage. Cost of delay is USD 11,500 per week. Compute the saving, the net, the return, and
the definition-phase extension at which the investment stops paying.
*Solution.* Saving `11 × 7,500 + 8 × 31,500 + 2 × 127,500 = 82,500 + 252,000 + 255,000 =`
**USD 589,500**. Net **USD 527,500**; return **9.51 times**. Breakeven extension
`527,500 / 11,500 =` **45.87 weeks**.
*Common error:* computing the saving on the gross later-stage costs (`11 × 8,000 + 8 × 32,000 +
2 × 128,000 =` 600,000), which forgets that each defect still costs 500 to correct at definition and
overstates the saving by 10,500.

**Exercise 5.3** A release has 24 development-weeks of capacity. Four candidate requirement bundles:
P (16 weeks, USD 496,000 a year), Q (12 weeks, 366,000), R (12 weeks, 360,000), S (6 weeks,
168,000). Compute the greedy selection by value per week and the enumerated optimum, their capacity
utilisation, the annual difference, and its present value over 6 years at 8 % (`AF(0.08, 6) =
4.622880`). *Solution.* Ratios: P **31,000**/week, Q **30,500**, R **30,000**, S **28,000**. Greedy
takes P (16 weeks), cannot fit Q or R, takes S (6 weeks) — **P + S = USD 664,000** using **22 of
24** weeks (**91.7 %**), with 2 weeks stranded. Enumeration finds **Q + R = USD 726,000** at **24 of
24** weeks (**100 %**). Difference **USD 62,000** a year — **9.34 %**: worth `62,000 × 4.622880 =`
**USD 286,619** of present value. *Common error:* ranking by value per week and stopping. The greedy
answer is defensible on the ratio and wrong on the outcome; the diagnostic is the stranded capacity,
and the check is one enumeration over four candidates. A second error is enumerating without a
dependency map: if R requires P, `Q + R` is not a feasible set.

**Exercise 5.4** At design freeze a project's requirements baseline held 560 requirements.
Integrated change control subsequently approved 19 additions. At acceptance-test entry the
traceability matrix carries 638. Uncontrolled requirements cost an average of USD 3,800 direct, and
21 of them consumed 0.2 weeks of critical path each. Cost of delay is USD 11,500 per week; the
baseline is USD 3,100,000. Compute the unexplained count, the direct and schedule cost, the total,
its share of the baseline, and each crept requirement's individual share. *Solution.* Expected
traced count `560 + 19 =` **579**; actual 638, so **59** requirements are unexplained. Direct `59 ×
3,800 =` **USD 224,200**; schedule `21 × 0.2 = 4.2` weeks at 11,500 = **USD 48,300**; total **USD
272,500**: **8.79 %** of the baseline, while each crept requirement is **0.12 %** of it. *Common
error:* reconciling cost and never counting requirements. The change log can balance to the penny
while 59 requirements enter without ever appearing in it; only the count reconciliation detects
them, and no delegation threshold can, because 0.12 % of a baseline is below every threshold anyone
sets.

**Exercise 5.5** Of 720 requirements, 126 carry acceptance criteria that restate the requirement and
41 carry none. Writing a measurable criterion costs USD 280. A deficient criterion produces a
dispute 20 % of the time; a dispute costs USD 8,400 in rework, and 1 dispute in 4 delays delivery by
0.5 weeks at a cost of delay of USD 11,500 per week. Compute the remediation cost, the expected cost
per dispute, the expected dispute cost, the net, and the breakeven dispute probability. *Solution.*
Deficient `126 + 41 =` **167**; remediation `167 × 280 =` **USD 46,760**. Expected delay per dispute
`0.25 × 0.5 × 11,500 =` **USD 1,437.50**, so cost per dispute **USD 9,837.50**. Expected disputes
`167 × 0.20 =` **33.4**, expected cost **USD 328,572.50**. Net **USD 281,812.50**. Breakeven
probability `46,760 / (167 × 9,837.50) =` **2.85 %**. *Common error:* two, and both understate the
case. Comparing the remediation cost against the rework cost alone omits the delay component
(1,437.50 of the 9,837.50). And applying the 20 % dispute rate to all 720 requirements rather than
to the 167 deficient ones inflates the expected cost by more than four times: an error that happens
to favour the right decision, which is why it survives.

---

## Practitioner's toolkit — Domain 5

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 5.T.1 — Requirements traceability matrix

One row per requirement: reference · statement · **class** (functional / non-functional / constraint /
data / transition / regulatory) · source, naming the **stakeholder or obligation**, not the document ·
benefit served and the **bundle** it belongs to (Toolkit 5.T.2) · design element · test case ·
deliverable · **acceptance criterion classification** (measurable / restated / absent) · verification
evidence reference · acceptance decision, acceptor by name and date · baseline version approved at, and
the change reference if added later.

Four reports every period carry the whole value of the matrix: rows missing a design, test or
deliverable link, **by class and never totalled**; rows accepted with no verification evidence, the
class that invalidates the record; **orphans**, from the reverse query over the design and test
registers; and criteria classified restated or absent, with the restated count shown separately
because it is the number nobody has. A matrix maintained and never queried is a cost with no control
attached.

### Toolkit 5.T.2 — Value attribution and constrained selection sheet

*Attribution side.* One row per **requirement bundle**: reference · requirements included · benefit
delivered, with its **named owner** (Domain 2's benefits register) · attributed annual value with its
range · effort in the **binding constrained resource**, named explicitly (usually a team, not money) ·
dependencies on other bundles · **value per unit of constrained effort** · and a flag for regulatory or
safety bundles, removed from the ranking and held as constraints. Foot the attribution column and run
the **sum test** against the business case, stating any shortfall rather than distributing it.

*Selection side.* State the constraint and its quantity. Compute and record **both** the greedy
result and the enumerated optimum over feasible sets, with the dependency map applied; report
capacity utilisation for each and flag any **stranded capacity** as a finding. Where the two differ,
state the annual difference and its present value at the appraisal rate. Then record the decision,
its maker by name, and (where it departs from the enumeration) what is being bought and at what
annual cost, because overruled arithmetic is a decision and an unexamined ranking is not.

### Toolkit 5.T.3 — Scope reconciliation and acceptance-criteria pattern set

*Reconciliation side*, run every reporting period and published on one page: baselined requirement
count · plus approved additions (with change references) · minus approved deletions · **equals expected
traced count** · against the **actual traced count** · equals **unexplained requirements**, listed
individually and investigated, not merely counted. Then: criterion text **diffed against its baselined
version**, with every expansion listed; the priced creep total, direct and schedule, at the cost of
delay; and the cumulative total set beside Domain 4's authorised drift, so the report states **what
share of total scope movement the change log actually captured**.

*Acceptance-criterion patterns*, four forms that make a criterion testable. **Threshold:** given
*[state]*, when *[action]*, the result is *[observable]* within *[limit]*, for performance, capacity
and time. **Enumeration:** the deliverable handles each of *[n]* listed cases, each individually
verifiable, for functional coverage, and it forces the case list into the open. **Negative:** given
*[invalid input or failure condition]*, the result is *[defined behaviour]*. The pattern that
catches the exception handling nobody specified. **Evidence:** acceptance requires *[named
artefact]* reviewed by *[named role]*, for requirements whose satisfaction is a judgement (clinical
safety, accessibility, regulatory conformance), which is how a judgement becomes auditable rather
than an opinion. Prohibit four constructions outright: "correctly", "appropriately", "as required",
and any comparative without a referent.

---

## Exam preparation — Domain 5

**What is assessed.** The three registers and why scope is the deliverable register; decidable scope;
the scope baseline's three artefacts and the primacy of the WBS dictionary; **pricing an undrawn
boundary on both cost and forgone benefit**; elicitation technique selection and the stakeholder groups
that cause systematic loss; **the correction ladder, the saving computed on stage differences, the
return on elicitation and the breakeven delay**; requirement quality properties and the six requirement
classes; solution capture and requirement inflation; **traceability in both directions, defect rates by
class on their own denominators, and which class invalidates the record**; value attribution to bundles
with the sum and counterfactual tests; **must-have inflation quantified**; **value per unit of
constrained effort, greedy against enumeration, stranded capacity and the dependency precondition**;
change against creep and the **count reconciliation**; acceptance-criterion states and the **breakeven
dispute probability**; verification against validation; and **conditional acceptance priced by severity
class**.

**The calculations to be able to do under time pressure.** Per-unit boundary exposure and its payback
from forgone benefit. Expected correction cost from a stage profile and a ladder; the saving from moving
`k` defects between stages, **always on the difference**; the return on an elicitation spend and the
breakeven delay. Forward defect rate overall and by class, and the reverse rate on the design-register
denominator. Value per unit of constrained effort; the greedy selection; the enumerated optimum over a
small candidate set; capacity utilisation; and the annual difference converted to present value with
`AF(r, n)`. The requirement-count reconciliation, and creep priced as direct plus schedule at the cost of
delay, with its share of baseline and of total movement. Remediation cost, expected cost per dispute
including its delay component, and the breakeven dispute probability. Fix-versus-defer against the
ladder, and the breakeven go-live delay.

**The traps.** Scope written in the activity register, so no boundary can be tested (5.1.1) · reading a
boundary statement as a way of saying no when the priced answer is to include (5.1.3) · computing an
elicitation saving on gross later-stage costs rather than stage differences (5.2.1, Exercise 5.2) ·
assuming every late defect was detectable at definition (5.2.1) · totalling traceability classes, or
combining forward and reverse rates on different denominators (5.2.3, Exercise 5.1) · running only the
forward test and never seeing the orphans (5.2.3) · attributing value to individual requirements rather
than bundles, and delivering half a capability (5.3.1) · scoring regulatory requirements in a value
ranking (5.3.1) · accepting a 78 % must-have classification as information (5.3.2) · ranking greedily
and stopping, ignoring stranded capacity (5.3.3, Exercise 5.3) · enumerating without a dependency map
(5.3.3) · monitoring change and never counting requirements, so 43 % of movement stays invisible
(5.4.1, Exercise 5.4) · measuring whether acceptance criteria exist rather than whether they say
anything (5.4.2) · applying the dispute probability to all requirements rather than the deficient ones
(Exercise 5.5) · applying the top of the ladder to every open item and so refusing to go live (5.4.3) ·
and reporting verification as though it answered the validation question (5.4.3, Case study B).

**How the domain connects.** Domain 1 supplies accountability for outcome rather than output and the
cost of delay every calculation is priced at. Domain 2 supplies the benefits map requirements attach to,
the ramped adoption profile the attributions inherit, the `AF(r, n)` used to discount a prioritisation
difference, and the capacity-constrained selection principle KA 5.3.3 works at requirement level.
Domain 3 supplies the decidability test the boundary discipline applies to content, and the thresholds
that cannot see a 0.175 % crept requirement. Domain 4 supplies the WBS the scope baseline includes, the
change flow that distinguishes change from creep, the authorised drift the creep figure is set beside,
and the "record states something untrue" failure class recurring here as unverified acceptance and as
the restated criterion. Forward: Domain 6 schedules these bundles and supplies the dependency map the
enumeration requires; Domain 7 costs them; Domain 8 attaches uncertainty to the attributions; Domain 9
owns verification and acceptance in depth; Domain 10 carries the contractual face of specification risk
(5.A.2); Domain 13 works the adaptive equivalents of 5.A.1; and Domain 16 answers the validation
question at handover.

---

## Domain 5 summary
Part One produced a project and assumed the content of its scope was known. It was not, and the
expensive failure is not incompleteness but a scope that is complete, agreed, delivered, verified
and valueless: the condition Case study B reports as **612 requirements, 100 % verified, 34.0 %** of
forecast benefit, because the **4,200** external applicants whose behaviour produced the benefit had
no seat in a requirements process built from the organisational chart. Only **47** of the 612
requirements, **7.68 %**: addressed them. The correction cost **USD 285,000** and returned **USD
425,500** a year, a payback of about **8.0 months**, for a conversation that had been available free
eighteen months earlier.

Scope is the deliverable register, bounded by outcomes and decomposed into activities, and it is
decidable only when its extents are counted. Meridian's boundary ("all clinics in the region"
against an approved 40) carried an exposure of **USD 143,000** at **USD 13,000** a site; and the
professional result is the inversion, because those 11 sites would return **USD 188,496** a year at
realistic adoption, a payback of about **9.1 months**. The boundary statement's value is not that it
refuses; it is that it forces the question while the answer is still cheap to act on.

Requirement defects cost four times more at every stage they survive: **400 · 1,600 · 6,400 · 25,600
· 102,400**. On Meridian's detection profile the unimproved correction bill is **USD 1,286,400**
(**USD 13,400** a defect), and an **USD 84,000** elicitation programme returns **USD 610,800**,
**7.27 times** its cost, computed on stage *differences* rather than gross later-stage costs, which
would have overstated it by 10,000. Two sentences carry the whole argument: **one defect moved out
of live service saves USD 102,000 and pays for the entire programme**, and the delay objection does
not break even until the definition phase is extended by **36.89 weeks**. Traceability then audits
the chain in both directions and reports **by class, never as a total**: **13.33 %** forward
non-conformance across **7.08 %** with no design, **4.38 %** with no test and **1.88 %** accepted
with no verification evidence (that last class being the one that makes every decision taken on the
record worthless) plus a **4.00 %** reverse rate, **17** orphans worth **USD 108,800**, invisible to
the forward test almost everyone runs alone.

Value is attributed to bundles, not requirements, and prioritisation only bites once the categories
mean something: **77.92 %** of Meridian's requirements were marked must-have, and asking each
originator to name the consequence of omission left **30.83 %**, raising the tradeable pool
**3.13-fold**. Against a hard **20 development-week** constraint, greedy ranking by value per week
takes the highest-ratio bundle, strands **2 of 20** weeks and returns **USD 409,000** a year;
enumeration returns **USD 453,000** (**10.76 %** more, **USD 262,737** of present value at 7 % over
eight years), and the ratio advantage that caused the loss was **under 1 %**. Stranded capacity is
the signature; a dependency map is the precondition; and no ranking is a decision.

Finally, scope moves without anyone approving anything. Meridian's baseline of **480** requirements
plus **12** approved additions should have traced **492**; it traced **531**, so **39** requirements
(about three a month, **3.25 times** the approved additions) entered with no record at all, costing
**USD 163,800** direct and **4.0** weeks of critical path, **USD 220,920** in total, **9.20 %** of
the baseline, at **USD 4,200** of direct cost each: **0.175 %** of baseline, below every threshold
any delegation schedule will ever set. Set beside Domain 4's **USD 291,176** of authorised drift,
total movement is **USD 512,096** (**21.34 %** of baseline and **38.42 %** of the approved NPV) of
which the change log captured **56.86 %**. No cumulative test on a change log can find the rest;
**only counting the requirements can**, and it is one addition and one subtraction a month. On the
way out, two more numbers worth keeping: writing testable acceptance criteria pays for itself above
a **2.67 %** dispute rate against an observed **25 %**, and the **71** criteria that restate their
requirement are more dangerous than the **27** that are missing, because completeness reports cannot
see them. And fixing six severity-one defects before acceptance costs **USD 59,820** against **USD
614,400** deferred, a breakeven go-live delay of **40.34 weeks**, applied by severity class and not
to all 41 open items.
