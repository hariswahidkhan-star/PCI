---
id: BPG-02
series: S09
series_name: Best Practice Guides
title: The work breakdown structure
subtitle: Decomposition rules, the 100 % rule, and why a WBS built to mirror the org chart cannot answer a cost question
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, student]
level: practitioner
reading_time_min: 17
summary: >
  How to decompose scope so that the resulting structure can carry a budget, a schedule and an owner:
  the 100 % rule and what it actually forbids, how far to decompose and the test that tells you when to
  stop, the difference between deliverable, phase and discipline structures, what belongs in a WBS
  dictionary entry, and how the WBS crosses the organisational breakdown structure to form control
  accounts. Includes a two-level decomposition with the 100 % rule verified at each level and a
  demonstration of why an organisation-shaped WBS cannot answer the question it will be asked most.
linkedin:
  format: carousel
  hook: >
    A WBS that mirrors the org chart cannot tell you what a deliverable cost. The number exists, but it
    is spread across four branches and only one person remembers which codes belong together.
  tags: [ProjectControls, WBS, ScopeManagement, CostEngineering, ProjectPlanning]
  asset: carousel-8
gated: false
related: [BPG-01, BPG-03, BPG-04, BPG-06, TPL-02, TPL-03]
bok_domains: [1, 5, 8]
sources: []
placeholders: 0
---

# The work breakdown structure

> How scope is decomposed so that the structure can carry a budget, a schedule and a named owner.

**In one paragraph.** How to decompose scope so that the resulting structure can carry a budget, a
schedule and an owner: the 100 % rule and what it actually forbids, how far to decompose and the test
that tells you when to stop, the difference between deliverable, phase and discipline structures, what
belongs in a WBS dictionary entry, and how the WBS crosses the organisational breakdown structure to
form control accounts. Includes a two-level decomposition with the 100 % rule verified at each level
and a demonstration of why an organisation-shaped WBS cannot answer the question it will be asked most.

**Who this is for.** Planners and cost engineers building or inheriting a WBS; project managers who
have to approve one; anyone who has been handed a structure and asked why the numbers will not add up.

---

## 1. What the structure has to do before it is elegant

A work breakdown structure (WBS) is judged by whether it can answer questions, not by whether it looks
tidy. Four questions are asked of it constantly, and a structure that cannot answer all four will be
worked around within a quarter:

- What is the total cost of *this thing we are building*?
- Who is accountable for that number?
- Is this element late, over, or both?
- If scope is added, where does it attach?

Every rule in this guide exists to keep one of those four answerable. The 100 % rule keeps the totals
true. Deliverable orientation keeps the first question answerable in one place rather than four.
Decomposition depth keeps the third answerable early enough to matter. The dictionary keeps the fourth
from being settled by whoever raises the purchase order.

The WBS is also the parent artefact of the whole controls function — the cost breakdown structure, the
schedule, the progress measurement register and the change register are all defined against it. That is
why `BPG-01 — Building a project controls function from zero` puts it first in the dependency chain,
and why changing it later is expensive in a way that changing almost anything else is not.

## 2. Decomposition: what it is and what it is not

Decomposition is the division of the total scope of work into progressively smaller components, each of
which is a *piece of the thing being delivered* rather than a piece of the effort to deliver it. The
distinction sounds pedantic until you try to cost something.

"Foundations" is a deliverable. "Civil engineering" is a discipline. "Detailed design phase" is a
period of time. All three appear in real structures, and only the first can carry a completion state
that anybody can verify. A deliverable is either there or it is not; a discipline is never finished
until the project is; a phase ends on a date that can be moved.

The practical rule is that each element should be describable as a **noun with a completion condition**.
"Inlet works structure — concrete poured, cured and surveyed to tolerance" is decomposable, costable,
schedulable and verifiable. "Engineering support" is none of those things.

This does not mean discipline and phase never appear. They appear as *codes*, not as *branches* — see
§7 and `BPG-03 — Cost breakdown structure and the code of accounts`. A transaction can be tagged with
its discipline, its phase, its cost type and its resource class simultaneously, and none of those tags
needs to be a level of the WBS. Confusing a tag with a branch is the single most common structural
error in project controls, and §8 shows what it costs.

## 3. The 100 % rule

The 100 % rule states that the children of any element together represent all of that element's scope —
no more, and no less. It is checked at every level, not once at the bottom.

What it forbids is more interesting than what it requires. It forbids **omission**: scope that exists
but has no home, which is how commissioning spares, temporary works and site clean-up disappear from
budgets. It forbids **overlap**: two elements that both plausibly contain the same work, which is how
the same activity gets costed twice in the estimate and then charged to whichever code is open. And it
forbids **invention**: elements that describe work not in the project's scope, which is how a
speculative future phase ends up carrying budget.

The rule applies to scope. Because budget follows scope, the arithmetic check is that each parent's
budget equals the sum of its children's budgets — but the check is a symptom test, not the rule itself.
A structure can balance perfectly and still omit scope, if the omitted scope was never estimated.

Three practices make the rule enforceable rather than aspirational:

**Check upward as well as downward.** Ask of each child, "what parent does this belong to?" — omissions
show up on the way up more reliably than on the way down.

**Give rework a home in advance.** Decide, and write down, that rework is charged to the element it
repairs rather than to a general remediation code. Without that decision, rework becomes a floating
cost that appears in whichever account has headroom.

**Name the exclusions.** Every dictionary entry states what the element does *not* include. Exclusions
are how the 100 % rule survives contact with an interface, because two adjacent elements that each
disclaim the same interface scope reveal the gap immediately.

## 4. Deliverable, phase and discipline structures

Three organising principles compete at level 2, and the choice has consequences that persist for the
life of the project.

**Deliverable-oriented.** Level 2 elements are the physical or logical things being produced. This is
the Institute's recommended default because it makes the most-asked question — *what did this cost?* —
answerable at a node rather than by a query. Its weakness is that it obscures phase: the design cost of
everything is scattered across every branch.

**Phase-oriented.** Level 2 is design, procurement, construction, commissioning. This suits projects
where the phases are contractually distinct, funded separately, or executed by different organisations,
and it makes stage-gate reporting natural. Its weakness is the mirror of the deliverable structure's:
the total cost of any one deliverable is spread across four branches.

**Discipline-oriented.** Level 2 is civil, mechanical, electrical, instrumentation. This is almost
always a mistake dressed as convenience, because a discipline is not a deliverable and never completes.
It survives because it matches how the estimate was built and how the resource pool is organised.

**The hybrid that usually wins** puts deliverables at level 2 and phase at level 3 *within* a
deliverable — so the design of the inlet works sits under the inlet works, not under a global design
branch. This keeps the deliverable total intact while preserving stage visibility, at the cost of
repeating the phase pattern across branches. Where phase is genuinely the primary funding and
accountability boundary, invert it — but do so deliberately, and record why in the WBS dictionary,
because the next planner will assume it was an accident.

## 5. How far to decompose

Decomposition stops when the lowest element — the work package — satisfies four conditions
simultaneously: it can be estimated with acceptable confidence; it can be scheduled as one or a small
number of activities; it has a single accountable owner; and its completion can be verified against
something other than an opinion.

That leaves a wide range, so use the **reporting-cycle test**: no work package should be able to run
for more than two reporting periods without producing an objective completion event. On a monthly
cycle, that means a package longer than about two months needs interim milestones with defined
evidence — which is a progress measurement decision, and belongs with the rules of credit in
`BPG-06 — Progress measurement and rules of credit`.

The counterweight is population. §9.3 shows the arithmetic: the number of work packages multiplied by
the effort of statusing each one is a fixed monthly tax, and a structure that produces four hundred
packages on a monthly cycle needs someone to status twenty of them every working day. That is
achievable with a mature field reporting process and impossible without one.

Where the scope is not yet definable, do not decompose it artificially. Hold it as a **planning
package** — a real element with a budget and a rough duration but no work-package detail — and convert
it later. That conversion is rolling wave, not re-baselining, and `BPG-04 — Baselining and baseline
change control` explains why the distinction matters to the variance.

## 6. The WBS dictionary

The dictionary is what turns a numbered list into a controlled artefact. Without it, the meaning of an
element lives in the head of whoever created it, and a WBS whose meaning is undocumented will be
interpreted differently by the estimator, the planner and the person coding the invoice.

A usable entry is short and contains, at minimum: the code and title; a one-sentence scope statement in
the form of a noun with a completion condition; explicit inclusions and, more importantly, exclusions;
the responsible organisational unit; the budget and its estimate basis reference; the measurement
technique that will be used to credit progress; and the acceptance evidence that closes the element.

Two of those fields do most of the work. **Exclusions** prevent the 100 % rule failing at interfaces.
**Acceptance evidence** prevents the argument at the end, because it fixes in advance what "done" is
going to look like. An entry without those two is a title with extra words.

`TPL-02 — Work breakdown structure and WBS dictionary` provides the instrument, with the fields defined
and a worked fragment.

## 7. WBS × OBS = the control account

The WBS says what. The organisational breakdown structure (OBS) says who. Their intersection is the
**control account** — the point where scope, budget, schedule and cost come together under a named
manager, and the level at which performance is actually measured.

The crossing matters because a single WBS element is often delivered by more than one organisation: a
structure built partly by a subcontractor and partly by direct labour, or a package where equipment
comes through a central procurement function and installation through the site team. Splitting the
element by organisation produces two control accounts whose budgets sum to the element's budget — the
100 % rule again, applied through the crossing rather than down the branch. §9.2 works this through.

Two consequences follow. First, the number of control accounts is not the number of WBS elements; it is
the number of populated intersections, and it is a design decision with an arithmetic test set out in
`BPG-01` §7. Second, a WBS element with no identifiable owning organisation is a warning: either the
scope has not been assigned, or the element is not a deliverable.

## 8. How this goes wrong

**The WBS mirrors the org chart.** Level 2 becomes Engineering, Procurement, Construction,
Commissioning — or worse, the names of the departments. Every deliverable's cost is then spread across
four branches, and the only person who can produce a deliverable total is whoever remembers which cost
codes belong together. §9.4 puts a number on it. This structure is chosen because it matches the way
the organisation is funded and the way the estimate was assembled, and it fails the moment someone asks
what a thing cost.

**Overlapping elements that both look right.** "Piping" and "Mechanical installation" both plausibly
contain pipe supports. Nobody notices at estimate stage because the estimator only used one of them.
The site team charges to whichever is open, and by month four the two accounts cannot be compared to
anything.

**The structure is changed after transactions exist.** An element is split, renamed or moved because
the original decomposition proved inconvenient. The historical postings stay where they were, so the
trend breaks at the change and every comparison across it needs a manual bridge. If the structure must
change, `BPG-03` sets out what the remapping actually costs.

**Level of effort branches.** "Project management", "Site establishment" and "Project controls" appear
as level 2 elements. They are real cost, they must be budgeted, and they are not deliverables — so they
never complete, cannot show a schedule variance, and dilute every index they are rolled into. Budget
them, segregate them, and keep their share of the baseline visible.

**Decomposition to the activity.** The WBS is taken down to individual schedule activities, producing
several thousand elements. The structure is now the schedule, the two artefacts can no longer be
maintained independently, and every schedule change becomes a scope change. The WBS stops at the work
package; the activity network sits beneath it.

**The dictionary is written after the estimate.** Elements are named during estimating and defined
afterwards, so the definitions are reverse-engineered from what the estimate happened to contain. The
exclusions are then whatever nobody priced, which is exactly the scope that will be argued about.

**A code is reused.** An element is deleted and its number given to something else. Every historical
report now refers to a different thing under the same code. Codes are permanent, even when the scope
they described was cancelled — retire them, never reissue them.

## 9. Worked example

*Illustrative figures.* Generic currency units. No real project, organisation or sector is implied. A
water treatment plant upgrade is used because it has clearly separable physical deliverables.

### 9.1 Two levels of decomposition, both checked

Project budget at completion: **12,400,000**.

**Level 2 — deliverable-oriented.**

| Code | Element | Budget |
|---|---|---:|
| 1.1 | Project management and controls (level of effort) | 900,000 |
| 1.2 | Design and engineering | 1,650,000 |
| 1.3 | Civil and structural works | 4,200,000 |
| 1.4 | Mechanical and process equipment | 3,850,000 |
| 1.5 | Electrical, control and commissioning | 1,800,000 |
| | **Total** | **12,400,000** |

Check: 900,000 + 1,650,000 + 4,200,000 + 3,850,000 + 1,800,000 = 12,400,000. The 100 % rule holds at
level 2. Note that 1.1 is segregated level of effort, not a deliverable — it is budgeted here so it
cannot leak into the deliverable accounts, and its share of the baseline is 900,000 ÷ 12,400,000 =
7.26 %, a figure worth stating explicitly because it is the proportion of the baseline that cannot show
a schedule variance.

**Level 3 — decomposition of 1.3 Civil and structural works (4,200,000).**

| Code | Element | Budget |
|---|---|---:|
| 1.3.1 | Site preparation and earthworks | 620,000 |
| 1.3.2 | Inlet works structure | 1,140,000 |
| 1.3.3 | Aeration basin structures | 1,580,000 |
| 1.3.4 | Access roads and hardstanding | 490,000 |
| 1.3.5 | Site drainage and containment | 370,000 |
| | **Total** | **4,200,000** |

Check: 620,000 + 1,140,000 + 1,580,000 + 490,000 + 370,000 = 4,200,000. The rule holds independently
at level 3; it is not inherited from the level-2 check.

### 9.2 Crossing the OBS to form control accounts

Element 1.3.3 (Aeration basin structures, 1,580,000) is delivered by two organisational units: a
subcontracted civils package, and a direct-works team responsible for the liner installation and
hydrostatic testing.

| Control account | WBS | Organisation | Budget |
|---|---|---|---:|
| CA-1.3.3-A | 1.3.3 | Civils subcontract | 1,310,000 |
| CA-1.3.3-B | 1.3.3 | Direct works | 270,000 |
| | | **Total** | **1,580,000** |

Check: 1,310,000 + 270,000 = 1,580,000. The 100 % rule survives the crossing, which is the property
that makes control-account performance roll up into WBS-element performance without adjustment.

### 9.3 Choosing the depth

**Assumption.** Statusing a work package at month end — collecting the progress claim, checking it
against the evidence named in the dictionary, and resolving queries — takes an average of 12 minutes.
The month has 20 working days.

At an average work package of 100,000: 12,400,000 ÷ 100,000 = **124 packages**. Statusing effort =
124 × 12 = 1,488 minutes = **24.8 hours** a month, or 124 ÷ 20 = **6.2 packages a working day**.

At an average work package of 30,000: 12,400,000 ÷ 30,000 = **413 packages** (rounded down). Statusing
effort = 413 × 12 = 4,956 minutes = **82.6 hours** a month, or 413 ÷ 20 = **20.65 packages a working
day**.

The finer structure is not wrong. It is a commitment to roughly 82.6 hours of statusing every month —
more than half a full-time equivalent doing nothing else — and that commitment should be made
knowingly, with the field reporting process that supports it, rather than discovered in month three.
The sensitive input is the 12 minutes: measure it after two cycles and redo the arithmetic.

### 9.4 What the org-chart WBS costs you

Suppose the same project had been decomposed at level 2 by department. The aeration basins' true cost
would then be spread as follows:

| Where the cost sits | Amount |
|---|---:|
| Engineering — basin design | 185,000 |
| Procurement — liner and embedded items | 240,000 |
| Construction — civils and direct works | 1,310,000 |
| Commissioning — hydrostatic testing and handover | 95,000 |
| **True deliverable cost** | **1,830,000** |

Check: 185,000 + 240,000 + 1,310,000 + 95,000 = 1,830,000.

In the deliverable structure, 1,830,000 is a node — visible, owned, comparable to its estimate, and
capable of showing a variance. In the department structure it is a query that has to be assembled from
four branches by someone who knows which cost codes belong to the basins. The number is not lost; it is
*unownable*. Nobody has a variance to explain, because nobody has the total.

The consequence is the point. When the basins overrun by, say, 140,000 — which is 140,000 ÷ 1,830,000 =
**7.7 %** of the deliverable — the department structure shows four small percentages against four large
departmental budgets, none of which trips a 10 % reporting threshold. The overrun is real, it is
material, and the structure has made it invisible.

## 10. Checklist

Take this into the WBS review. It is written to be answered by the person who built the structure,
in front of the people who will have to use it.

**Structure**

- [ ] Can every level 2 element be stated as a noun with a completion condition?
- [ ] Does any level 2 element name a department, a discipline or a phase? If so, why, and is it recorded?
- [ ] Is level-of-effort work segregated, and what percentage of the baseline does it represent?
- [ ] Has any code ever been reused for different scope?

**The 100 % rule**

- [ ] Does every parent's budget equal the sum of its children, at every level, checked independently?
- [ ] Working upward, does every element have an unambiguous parent?
- [ ] Where two elements could both contain the same work, which one does, and is it written down?
- [ ] Where does rework charge — to the element it repairs, or to a general code?
- [ ] Are commissioning spares, temporary works, site clean-up and interface scope each in exactly one place?

**Depth**

- [ ] Can any work package run more than two reporting periods without an objective completion event?
- [ ] How many work packages are there, and what is the monthly statusing effort at your measured rate?
- [ ] Is undefined future scope held as a planning package rather than decomposed speculatively?
- [ ] Has the WBS been taken down to activity level? (If yes, it has absorbed the schedule.)

**Dictionary and ownership**

- [ ] Does every element have a dictionary entry with inclusions *and* exclusions?
- [ ] Does every entry name the acceptance evidence that will close the element?
- [ ] Does every element cross to at least one organisational unit, and does every crossing have a named manager?
- [ ] Do the control-account budgets at each crossing sum to the WBS element's budget?

---

## Related

- `BPG-01 — Building a project controls function from zero` — where the WBS sits in the dependency chain, and the control-account sizing test.
- `BPG-03 — Cost breakdown structure and the code of accounts` — the second axis: how cost type and resource class are carried as codes rather than as branches.
- `BPG-04 — Baselining and baseline change control` — what happens to the structure when scope is added, and why rolling wave is not a re-baseline.
- `BPG-06 — Progress measurement and rules of credit` — how the work packages defined here are credited with progress.
- `TPL-02 — Work breakdown structure and WBS dictionary` — the instrument, with every dictionary field defined.
- `TPL-03 — Cost breakdown structure and code of accounts` — the companion template for the coding axis.

## Sources and standards

Drawn from the Institute's Body of Knowledge: Domain 8 (Project Management Lifecycle) for
decomposition and the 100 % rule, Domain 5 (Cost Management and Cost Control) for control accounts and
work packages, and Domain 1 (Foundations of Accounting for Project Controls) for the mapping between
scope structure and cost coding.

The 100 % rule is a long-established principle of scope decomposition, described here in the Institute's
own words. The reporting-cycle depth test in §5 and the statusing-effort arithmetic in §9.3 are PCI
recommended practice; the 12-minute input is an assumption of the example, not a benchmark. No external
standard, table or diagram is reproduced.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
