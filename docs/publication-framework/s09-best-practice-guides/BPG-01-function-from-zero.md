---
id: BPG-01
series: S09
series_name: Best Practice Guides
title: Building a project controls function from zero
subtitle: The order you stand things up in, and why the order is the whole decision
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: professional
reading_time_min: 16
summary: >
  How to establish a project controls function where none exists, in the order that works: coding
  structure before tools, cut-off before dashboards, and a defined control-account level before anyone
  is asked for a variance. The guide gives the dependency chain, a method for sizing the function
  against the smallest variance it has to detect, a worked reporting calendar derived backwards from
  the governance meeting, and an explicit list of what to defer and how to say so out loud.
linkedin:
  format: article
  hook: >
    A dashboard built before a cut-off date exists will report whatever the invoice clerk happened to
    process that week. Sequence is the decision; the tool is a consequence.
  tags: [ProjectControls, CostEngineering, ProjectGovernance, PMO]
  asset: one-pager
gated: false
related: [BPG-02, BPG-03, BPG-07, BPG-14, TPL-01, TPL-15]
bok_domains: [3, 5, 8]
sources: []
placeholders: 0
---

# Building a project controls function from zero

> What to stand up first when there is nothing, in what order, and what to leave until later on purpose.

**In one paragraph.** How to establish a project controls function where none exists, in the order that
works: coding structure before tools, cut-off before dashboards, and a defined control-account level
before anyone is asked for a variance. The guide gives the dependency chain, a method for sizing the
function against the smallest variance it has to detect, a worked reporting calendar derived backwards
from the governance meeting, and an explicit list of what to defer and how to say so out loud.

**Who this is for.** Project controls managers and heads of PMO asked to build a function on a new
programme or in an organisation that has never had one; project directors deciding what to fund first;
finance business partners who will have to live with whatever coding scheme gets chosen.

---

## 1. The only real decision is sequence

Almost every element of a controls function is uncontroversial. Nobody argues that a project should be
without a work breakdown structure, a cost baseline, a progress measurement method or a monthly report.
The arguments are about **order** — and order is where functions are won or lost, because most of these
elements depend on each other in one direction only.

A cost report cannot be produced before there is an agreed cut-off date. An agreed cut-off is worthless
if cost arrives coded to a structure nobody can roll up. A rollup needs a coding structure decided
before the first purchase order was raised, because a purchase order carries its code into every
transaction it later generates. And the coding structure cannot be designed until somebody has decided
what the reportable units of scope are. That chain runs one way; reversing a link is possible, but it
is paid for in rework, and `BPG-03` puts a number on the most common reversal.

The failure this guide is written against is not incompetence. It is the reasonable instinct to start
with what the sponsor asked for — usually a dashboard — and backfill the structure later. The dashboard
gets built. It reports whatever the accounts payable team processed that week, against a budget nobody
has time-phased, at a level of detail chosen by whoever configured the tool. Then it is very hard to
remove, because it exists and people look at it.

## 2. What "zero" looks like from the inside

Real starting positions are rarely blank. They are usually one of three states, and each needs a
different opening move.

**Genuinely nothing.** A new programme, no systems configured, no historical postings. The easiest and
rarest case: everything below applies in order.

**A finance system already posting.** Cost is recorded against a general ledger and perhaps a
rudimentary project code, but there is no scope structure, no baseline and no progress measurement.
Transactions already exist and carry codes, so the opening move is to count them — that number decides
whether you design a structure that maps onto the existing one or accept a retrofit (see
`BPG-03 — Cost breakdown structure and the code of accounts`).

**A controls function that has stopped working.** Reports are produced, nobody uses them, and the
schedule and the cost report disagree. This looks like the hardest case and is often the quickest to
fix, because the data usually exists — what is missing is a cut-off, a rules-of-credit register or an
agreed control-account level. Diagnose before you rebuild; `BPG-19 — Project controls assurance and
health checks` covers that diagnosis.

State which of the three you are in, in writing, in the first week. Pretending a rebuild is a start-up
is how a rebuild gets the wrong budget.

## 3. The dependency chain

The order below is not a preference. Each item is genuinely blocked by the one above it.

**3.1 Reportable scope.** Decide what the project is made of, expressed as deliverables rather than as
departments. This is the work breakdown structure, and it parents everything else because it determines
what a variance can be *about*. `BPG-02 — The work breakdown structure` owns the method.

**3.2 The coding structure.** Decide how cost will be classified — by scope element, by cost type, by
resource class — and fix the code format. This must happen before the first commitment is raised,
because every commitment stamps a code onto a stream of downstream transactions. `BPG-03` owns it.

**3.3 The control-account level.** Decide where scope, budget, cost and schedule meet, and therefore
where a named person is accountable for a variance. It cannot be settled before 3.1 and 3.2, because
the control account is defined by their intersection with the organisational breakdown structure. §7
gives the arithmetic test.

**3.4 The cut-off calendar.** Decide the date on which the month ends for controls purposes, and the
working-day sequence from that date to the governance meeting. §9 works it backwards from the meeting.

**3.5 The rules of credit.** Decide, per work package, how physical progress will be measured and what
evidence earns each increment — before anyone is asked for a percentage, because the first percentage
anybody gives you sets the precedent. `BPG-06 — Progress measurement and rules of credit` owns this.

**3.6 The baseline.** Freeze the scope, the time-phased budget and the schedule under a change-control
gate; `BPG-04 — Baselining and baseline change control` owns it. A baseline set before 3.1 to 3.5 is a
number, not a baseline: there is nothing holding it in place.

**3.7 The first report.** Produce it, then produce the second on the same day of the cycle in the same
shape, finished or not. Cadence is established by repetition, not by design.

**3.8 Tooling and visualisation.** Only now — because only now do you know the structure, the cadence,
the level and the measurement rules, which is to say the requirement. Buying earlier is buying somebody
else's requirement.

## 4. The minimum viable controls function

The smallest set of artefacts that constitutes a real function — not a good one, a real one — is
shorter than most implementation plans suggest.

| Artefact | What it must contain to count |
|---|---|
| Work breakdown structure and dictionary | Every element defined in a sentence, with an owner and an inclusion/exclusion note |
| Code of accounts | Segment definitions, the valid-combination rule, and who may create a new code |
| Control-account register | Account, WBS element, responsible manager, budget, measurement technique |
| Time-phased cost baseline | Budget spread across the schedule, at control-account level |
| Cut-off calendar | Cut-off date and the working-day sequence to issue |
| Rules-of-credit register | Technique and evidence per work package |
| Change register | Every trend, its status and its cost impact, from day one |
| Monthly report | One page of numbers, one page of narrative, control-account detail beneath |

Everything on that list is a document or a register; none needs a tool beyond a spreadsheet. That is
the point — the function is the discipline, and software is an efficiency layer over it.
`TPL-01 — Project controls execution plan` names all of them, their owners and their frequencies in one
controlled document, and that is the artefact a sponsor should approve.

## 5. Cut-off before dashboards

The cut-off date is the least glamorous item on the list and the one that most changes everything
downstream, because it is what makes a period a period.

Without a fixed cut-off, cost enters the report according to when invoices happened to be processed.
The period cost figure becomes a measure of the accounts payable team's throughput rather than of the
project's consumption — and, worse, the cost figure and the progress figure then describe different
windows of time, progress measured to the day the planner asked the field and cost to the day the
ledger was extracted, so every index computed from them compares two different months.

A cut-off is real only when three things hold: the date is fixed and published in advance; every data
source reports to that date and no other; and the gap between cut-off and complete data is filled by
accrual rather than by waiting. That last condition makes the cut-off enforceable, which is why
`BPG-07 — Accruals and cut-off discipline` is a prerequisite for a credible report rather than an
accounting refinement.

A dashboard adds nothing the underlying number does not already contain; it changes how quickly the
number is read. Over a sound cut-off and a stable coding structure it is genuinely valuable. Built
first, it industrialises whatever error is in the data and gives it a colour.

## 6. What to defer, and how to defer it honestly

Deferral is a decision and should be recorded as one. The defaults below are the Institute's
recommended practice for a new function; the grounds matter more than the list, because a different
risk profile will legitimately reorder it.

**Defer quantitative schedule risk analysis** until the schedule passes a quality review. Simulating a
network with open ends and unjustified constraints produces a distribution around a number that was
never a forecast. See `BPG-05 — Schedule quality — a practical review` and `BPG-17`.

**Defer earned value indices** until the rules of credit are written and signed. A cost performance
index derived from subjective percentages teaches the organisation that the index is negotiable, and
that lesson is expensive to unlearn.

**Defer resource-loaded scheduling** unless the project is genuinely resource-constrained and the data
will be maintained. An unmaintained resource-loaded schedule is worse than an unloaded one, because it
looks like it means something. **Defer integration** between the cost system and the scheduling tool
until both are stable alone, because integration multiplies the consequences of an error in either.

**Do not defer the change register.** It costs half a day to open and it is the only artefact that
cannot be reconstructed retrospectively — records can rebuild a document, but not the reasoning behind
a decision taken in month two.

State each deferral with a trigger: *"quantitative schedule risk analysis will be introduced once the
schedule quality review returns no open ends on the critical path and fewer than five unjustified
constraints."* A deferral with a trigger is a plan; one without is a gap somebody else will find.

## 7. Sizing the function against the work it has to detect

The most consequential sizing decision is the number of control accounts, because it sets both the
monthly effort of running the function and the smallest problem the function can see. Too few accounts
and a material overrun hides inside a large one, diluted below the reporting threshold until it is too
late to act. Too many and measurement overhead consumes the analysis time that justified the function.

The Institute's recommended practice is to derive the floor from the detection requirement, not from a
rule of thumb:

```
Minimum control accounts = (Budget at completion × variance reporting threshold)
                           ÷ the smallest variance you must act on
```

The logic is direct. If a variance is reported only when it exceeds a percentage of its account's
budget, the account has to be small enough that the smallest problem you care about clears that
percentage. Both inputs are explicit organisational decisions and both belong in the controls execution
plan. Set the floor from the calculation, then check the ceiling against capacity: accounts multiplied
by monthly effort per account must leave room for analysis. §9.2 runs both directions.

One qualification: the formula uses the *average* account size, so a portfolio with a wide spread of
account values needs the test applied to the largest accounts individually.

## 8. How this goes wrong

**The tool is procured first, and it defines the structure.** A configuration workshop asks what the
cost breakdown should look like three weeks before anyone has decided what the deliverables are. The
resulting structure is whatever the implementation consultant used last time, and it survives the life
of the project, because by the time anyone objects there are transactions posted against it.

**Cost coding is left to procurement.** Purchase orders are raised against whichever ledger account
gets them approved fastest, and six months later the same physical scope sits under three different
codes. `BPG-03` sets out the consequence.

**The cut-off moves to suit the invoice run.** Somebody notices a large invoice will land on the 2nd
and holds the cut-off open for it. That breaks the comparability of the period, and it establishes the
precedent that cut-off is negotiable whenever the number is unhelpful.

**The baseline is set before the scope is decomposed.** A total is agreed at sanction and called a
baseline. With no time-phasing and no control-account allocation, the first variance question — *is
this account over, or just early?* — has no answer, and the function spends its opening quarter
reverse-engineering the thing it was supposed to be measuring against.

**Everything is measured, nothing is owned.** Two hundred work packages, each with a percentage, none
with a named control-account manager. Progress collected by the controls team belongs to the controls
team, so the numbers get argued with rather than acted on. Accountability comes from who signs.

**The function reports to the wrong meeting.** The pack goes to a monthly review with no authority to
approve a change, so decisions defer to a body that meets quarterly and the variance is two months old
on arrival. Establish which meeting owns the change-control gate before designing the calendar.

**Level of effort creeps into the baseline.** Supervision, project management and the controls function
itself are budgeted as packages that earn by the calendar, cannot show a schedule variance, and dilute
every index they are aggregated into. Segregate them from the outset; retrofitting the separation means
restating the trend.

## 9. Worked example

*Illustrative figures.* All values are in generic currency units. No jurisdiction, sector or real
project is implied.

### 9.1 The reporting calendar, derived backwards

**Assumptions.** A monthly cycle; a five-day working week, holidays ignored. The steering committee,
which may approve a baseline change, meets on the tenth working day. Readers need three working days
with the pack. Cut-off is the last calendar day of the preceding month. Working backwards:

| Working day | Activity |
|---|---|
| WD 10 | Steering committee |
| WD 8–9 | Reading time |
| WD 7 | Pack issued |
| WD 6 | Project manager review and sign-off |
| WD 5 | Narrative drafted with control-account managers |
| WD 4 | Earned value computed; variances analysed |
| WD 3 | Accruals determined and posted; schedule progressed to the status date |
| WD 2 | Cost extracted from the ledger; goods-received-not-invoiced report pulled |
| WD 1 | Field progress returns and timesheets submitted against the cut-off |

Seven working days run from cut-off to issue, of which two are pure data collection and one is
judgement. That is the whole argument for sequence: **only four working days in the month are available
for analysis**, and they exist only because the three before them were protected.

Now move the committee to the seventh working day, as sponsors regularly request. Three working days
disappear and the pack must issue on WD 4, compressing the chain to data in, extract and accrue,
compute and draft, review and issue. The casualty is the accrual judgement, because it is the only step
with no external deadline attached to it. `BPG-07` sets out what that compression costs in reported
performance; the point here is that the reporting calendar is a controls design decision and should be
negotiated as one rather than inherited.

### 9.2 Sizing the control-account population

**Assumptions.** Budget at completion 24,000,000 currency units. A variance is reported when it exceeds
10 % of its account's budget. The smallest variance the project director requires to be visible is
150,000. Monthly effort per control account — data check, reconciliation, status conversation,
commentary — is 40 minutes. The function has 1.5 full-time equivalents at 150 productive hours a month.

**Floor, from the detection test.**

```
Minimum control accounts = (24,000,000 × 0.10) ÷ 150,000
                         = 2,400,000 ÷ 150,000
                         = 16 control accounts
```

At exactly 16 accounts the average account is 24,000,000 ÷ 16 = 1,500,000, and a 150,000 variance is
150,000 ÷ 1,500,000 = 10.0 % — sitting precisely on the threshold, which is not a margin. Take 20
accounts instead: the average account is 24,000,000 ÷ 20 = 1,200,000, and the same variance is
150,000 ÷ 1,200,000 = 12.5 %, clearing the threshold with room for an account that runs larger than
average.

**Ceiling, from capacity.**

```
Monthly review effort = 20 accounts × 40 minutes = 800 minutes = 13.3 hours
Available capacity    = 1.5 FTE × 150 hours      = 225 hours
Share consumed        = 13.3 ÷ 225               = 5.9 %
```

Roughly six per cent of the function's month goes on the control-account review pass, before any
analysis, forecasting or change assessment. That is affordable.

**The counterfactual.** Structured as 60 accounts — attractive, because the average account falls to
400,000 and a 150,000 variance becomes 37.5 % of its account, impossible to miss — the review pass
becomes 60 × 40 = 2,400 minutes = 40.0 hours, or 40.0 ÷ 225 = 17.8 % of capacity. The visibility is
genuinely better. Whether it is worth three times the monthly effort is a resourcing decision for the
project director, and it should be put to them in those terms rather than settled by whoever configures
the tool.

**What the answer depends on.** The 40-minute figure is the sensitive input, and it varies with data
quality and with whether control-account managers write their own commentary. Measure it after two
cycles and redo the ceiling calculation. The detection test itself depends on the threshold and the
materiality figure being set by someone with authority to set them — if they have not been, that is the
finding, not the arithmetic.

## 10. Checklist

Take this into the meeting where the function is being scoped. Every line is answerable yes or no by
someone in the room; a "we'll come back to that" is itself a finding.

**Before anything is procured**

- [ ] Which of the three starting positions in §2 are we in, and who has written that down?
- [ ] Is there a deliverable-oriented WBS, or only a list of departments and phases?
- [ ] Has the code of accounts been designed, with a named owner who may authorise a new code?
- [ ] How many cost transactions have already posted under a provisional code?
- [ ] Has anyone been asked to approve a tool before the four questions above were answered?

**Structure**

- [ ] Is the control-account count derived from the §7 detection test, with both inputs written down?
- [ ] Does every control account have a named manager who has agreed in person to own its variance?
- [ ] Is level-of-effort work segregated into its own accounts, with its share of the baseline recorded?
- [ ] Does every WBS element have a dictionary entry — a sentence, an owner, inclusions and exclusions?

**Cadence**

- [ ] Is the cut-off fixed, published, and identical for cost, progress and schedule?
- [ ] Has the working-day sequence from cut-off to issue been drawn, with the analysis days identified?
- [ ] Which meeting may approve a baseline change, and does the calendar reach it?
- [ ] What happens when a large invoice lands after cut-off? (The correct answer is: it is accrued.)

**Measurement and deferrals**

- [ ] Is there a signed rules-of-credit register covering every package that will be asked for a percentage?
- [ ] Is the evidence for each credit increment named — which document, signed by whom?
- [ ] Is every deferred capability listed with a trigger condition in the controls execution plan?
- [ ] Is the change register open today, even though there is nothing in it yet?
- [ ] Has the sponsor been told in writing what they are not getting in the first two cycles, and why?

---

## Related

- `BPG-02 — The work breakdown structure` — the first link in the chain, and what everything else is defined against.
- `BPG-03 — Cost breakdown structure and the code of accounts` — the coding decision that must precede the first commitment, and what reversing it costs.
- `BPG-07 — Accruals and cut-off discipline` — why the cut-off in §5 is enforceable only if the accrual process behind it is.
- `BPG-14 — Monthly reporting that gets read` — what the cadence in §3.7 should produce.
- `TPL-01 — Project controls execution plan` — the controlled document naming every §4 artefact, its owner and its frequency.
- `TPL-15 — Project controls health check` — for diagnosing the third starting position in §2.

## Sources and standards

Drawn from the Institute's Body of Knowledge: Domain 3 (Budgeting and Forecasting) for the time-phased
budget, Domain 5 (Cost Management and Cost Control) for the control account and the
commitment–accrual–actual cycle, and Domain 8 (Project Management Lifecycle) for scope decomposition
and work authorisation.

The control-account sizing method in §7 is PCI recommended practice, derived arithmetically in §9.2
rather than borrowed; the reporting threshold and materiality inputs are organisational decisions, not
published values. No external standard is reproduced here, and no threshold in this guide is attributed
to any named publication.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
