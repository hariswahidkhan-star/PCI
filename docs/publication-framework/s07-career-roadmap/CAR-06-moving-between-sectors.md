---
id: CAR-06
series: S07
series_name: Career Roadmap
title: Moving between sectors
subtitle: What genuinely transfers between EPC, oil and gas, rail, data centre, power and building — and what has to be relearned
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, employer]
level: practitioner
reading_time_min: 13
summary: >
  Project controls method transfers between sectors almost completely; the intuition that makes a
  practitioner fast does not. This document gives five variables that characterise any sector — contract
  form, unit of production, cost shape, assurance overlay and decision cadence — then works through six
  sectors on those variables, naming what transfers, what must be relearned, and the mistake that
  identifies an incomer. It ends with the arithmetic of a benchmark that was correct in one sector and
  wrong in the next.
linkedin:
  format: post
  hook: >
    Your method transfers between sectors. Your sense of what a number should look like does not, and it
    is the first thing to distrust when you move.
  tags: [ProjectControls, EPC, Infrastructure, DataCentres, CareerDevelopment]
  asset: one-pager
gated: false
related: [CAR-01, CAR-02, CAR-08, BPG-06, BPG-18, SAL-03, SAL-06]
bok_domains: [5, 6, 7, 10]
sources: []
placeholders: 0
---

# Moving between sectors

> What a project controls professional carries across a sector boundary, and what they have to leave behind.

**In one paragraph.** Cut-off discipline, earned value mechanics, forecast defence and change control work
identically in every sector; what does not transfer is the intuition about magnitudes that makes an
experienced practitioner fast, and that intuition is the first thing to distrust after a move. This
document characterises a sector by five variables — the contract form and who carries risk, the unit of
production, the shape of the cost, the assurance overlay, and the decision cadence — then applies them to
six sectors, naming what transfers, what must be relearned, and the characteristic error of an incomer.
It contains no claims about demand, hiring or pay in any sector.

**Who this is for.** Cost engineers, planners, risk analysts and controls managers considering or making a
sector move, and the hiring managers deciding how much sector-specific knowledge genuinely matters.

---

## 1. Five variables that characterise any sector

Sectors feel very different from the inside and are structurally similar from the outside. If you can
answer these five questions about a sector, you can work in it; if you cannot, no amount of general
controls skill will stop you being surprised.

**What is the contract form, and who carries which risk?** Lump sum, remeasurable, reimbursable, target
cost with a pain-and-gain mechanism, or a package structure combining several. This determines what
change is, what may be claimed, what evidence is required, and therefore what the controls function
spends its time doing.

**What is the unit of production, and how is progress credited?** Metres of pipe, tonnes of steel,
engineering deliverables, procurement milestones, commissioning systems, or completed measured items in a
bill of quantities. This determines the rules of credit and where progress can be inflated.

**What shape is the cost?** Labour-dominated, equipment-dominated, materials-dominated, or dominated by
plant supply and long-lead items. A labour-dominated project is controlled through productivity; an
equipment-dominated one is controlled through procurement and delivery dates, and the two demand different
weekly attention.

**What is the assurance overlay, and who audits you?** Internal audit only, a client's assurance team, a
regulator, a public-funding stage-gate regime, or a lender's technical adviser. This determines how much
of your work is documentation of the work.

**What is the decision cadence?** Monthly reporting into a steering group, weekly into a delivery board,
or hourly during a fixed-duration outage. Cadence determines the design of the whole controls cycle, and
it is the variable incomers most often fail to adjust.

## 2. What transfers, and the one thing that does not

**Transfers essentially unchanged:** the data model and the control account as the unit of everything; the
period cycle and cut-off discipline; earned value mechanics and the distinction between physical progress,
certified value and recognised revenue; forecast method selection and defence; trend and change control;
contingency governance; the habit of auditability; and the professional behaviours that make a number
believable.

**Does not transfer:** your sense of what a number should look like. Every experienced practitioner runs a
continuous plausibility check against remembered magnitudes — a productivity rate, a proportion of cost
that is usually labour, a normal amount of scope growth in detailed design, a typical procurement lead
time. That check is what makes you fast, and after a sector move it is actively dangerous, because it is
now calibrated to a different physics and a different commercial structure. §8 works an example of exactly
this failure.

The practical response is to treat your own benchmarks as suspect for a full cycle, and to rebuild them
from the new project's own data at the first opportunity: measure the achieved rate yourself rather than
accepting either your old number or the new team's assertion.

## 3. Engineering, procurement and construction

Engineering, procurement and construction (EPC) is treated first because it contains the other sectors'
problems in one contract.

**Contract and risk.** Typically lump-sum or turnkey, with the contractor carrying interface risk across
three phases that behave completely differently.

**Unit of production.** Three units in one project: engineering measured in deliverables and their
revisions; procurement measured in order placement, fabrication and delivery milestones; construction
measured in installed quantities. The integrated progress figure is a weighted composite, and the
weighting is a decision that materially changes the reported percentage.

**Cost shape.** Front-loaded engineering, a very large procurement middle, and a labour-intensive
construction and commissioning tail.

**Assurance.** Client-side project management team, often with a technical adviser; heavy documentary
requirement around vendor data and certification.

**What transfers in easily.** Anyone from a construction-only background brings the installed-quantity
discipline. Anyone from procurement brings commitment control.

**What must be relearned.** Engineering progress. Deliverable-based credit is easy to inflate — a drawing
issued for review is not a drawing approved — and engineering slippage is the leading indicator of
everything that follows.

**Characteristic incomer error.** Reading procurement commitment as progress. Placing an order is not
achievement; it is exposure. See `BPG-18 — Interface and subcontractor controls` for the interface
mechanics.

## 4. Oil and gas

**Contract and risk.** Wide range, from reimbursable front-end engineering through to lump-sum
construction; heavy long-lead equipment content; turnarounds and shutdowns run to fixed windows with
severe consequences for overrun.

**Unit of production.** Conventional construction quantities in greenfield work; in a turnaround, the unit
is the job card and the constraint is the window.

**Cost shape.** Equipment and long-lead items dominate capital projects. In a turnaround the cost is
labour and the schedule is the money.

**Assurance.** Safety and permit regimes that directly constrain productive hours; extensive technical
assurance and, on operating assets, production-loss economics that dwarf the project cost.

**What transfers in easily.** Cost discipline and procurement control transfer well from any capital
project background.

**What must be relearned.** The productive-hour fraction under a permit regime, worked through in §8; and
the cadence of a turnaround, which is not monthly. A shutdown that reports monthly has reported once.

**Characteristic incomer error.** Applying a monthly controls cycle to a fixed-window outage. In that
environment the cycle is daily or by shift, the forecast is remade continuously, and the only question
that matters is whether the window will be met.

## 5. Rail and infrastructure

**Contract and risk.** Frequently public or regulated clients, with stage-gate funding, defined approval
points and formal change governance. Risk allocation is often explicit and heavily documented.

**Unit of production.** Physical quantities, but gated by access. The controlling unit is frequently the
possession or access window rather than the quantity itself.

**Cost shape.** Civil-heavy, with significant third-party costs — utilities, consents, land, protective
works — that are outside your organisation's control and inside your forecast.

**Assurance.** The heaviest of the six. Public funding brings stage gates, external review and an
expectation that every position can be evidenced long after the fact.

**What transfers in easily.** Documentation discipline from any regulated environment; risk quantification
skill, which is used seriously here.

**What must be relearned.** Access as the primary constraint, third-party dependency management, and float
ownership. In an access-constrained programme, float is not spare time; it is the buffer protecting a
window that cannot move, and treating it as free is how a recoverable position becomes an unrecoverable
one.

**Characteristic incomer error.** Forecasting third-party dependencies with the same confidence as
self-performed work, and treating consent processes as durations rather than as events with their own
governance.

## 6. Data centre

**Contract and risk.** Frequently a repeatable, modular scope delivered to a client whose commercial value
is concentrated in a single date — energisation and handover of capacity. Change late in the programme is
common because the end user's requirements evolve.

**Unit of production.** Repeatable units early — halls, modules, racks of infrastructure — and
commissioning systems late. Commissioning is a discipline with its own progressive levels of test, and it
is where the programme is actually won or lost.

**Cost shape.** Heavy mechanical and electrical plant content, with long-lead equipment driving the
programme, and an installation labour component that is intense and compressed.

**Assurance.** Client technical assurance and a rigorous witnessed-test regime; extensive documentation as
a condition of handover.

**What transfers in easily.** Any background with repeatable-unit production; strong procurement and
expediting skill, which matters more here than almost anywhere.

**What must be relearned.** Commissioning as a controlled programme with its own rules of credit, not as a
tail activity assigned a percentage. Also the primacy of the date: on a project where the date carries the
commercial value, a cost-optimal decision that risks the date is not a saving.

**Characteristic incomer error.** Crediting commissioning progress by elapsed effort rather than by
systems accepted, so the programme reports comfort until the witnessed tests begin.

## 7. Power, renewables, and building

Two further sectors, treated more briefly because their structures are widely understood.

**Power and renewables.** The work is packaged — civil works, plant supply, installation, and grid
connection — and the packages interlock at dates rather than continuously. Two features dominate the
controls job. First, grid connection is an external dependency you do not control and cannot expedite,
which makes it a risk position rather than a schedule activity. Second, installation is
weather-constrained: forecasting on an average rate rather than on the mechanics of workable windows will
be optimistic every time, and the error compounds as the season closes. Payment is usually tied to
performance testing and take-over, so the commercial position is concentrated at the back end where the
schedule risk also sits.

**Building.** Measured work against a bill of quantities, a valuation and payment-application cycle, very
high subcontract density, and a design responsibility split that varies by procurement route. The most
common incomer confusion is between certified valuation and earned value: the valuation is what the client
has agreed to pay this period, which is a commercial negotiation, and the earned value is what performance
has earned against budget. They rarely match, they should not be forced to, and the gap between them is
itself a management question. `BPG-06 — Progress measurement and rules of credit` treats the distinction
properly.

## 8. How this goes wrong

**Carrying benchmarks across the boundary.** The single largest source of error, and the subject of §9. A
rate that was conservative in one environment can be recklessly optimistic in another.

**Assuming the vocabulary means the same thing.** Float, progress, commitment, completion and even
"handover" carry different operational definitions between sectors. Ask for the definition in use before
you report against it; do not assume your own.

**Underestimating the assurance overlay.** Moving from a lightly assured environment into a heavily
regulated one, practitioners often treat documentation as bureaucracy and are then unable to evidence a
position they held correctly. In these environments an unevidenced correct answer scores zero.

**Overestimating it in the other direction.** Moving the other way, some practitioners import a governance
load the project cannot absorb and slow the cycle below the decision cadence. Both directions are failures
to read the fifth variable.

**Deferring to the local team on everything.** Sector knowledge is real and worth respecting, but "that is
how it is done here" is not an answer to "why is the accrual incomplete". The method is yours; the context
is theirs.

**Presenting your experience in the old sector's units.** A hiring manager cannot evaluate "managed the
cost position on a lump-sum EPC package" if they think in systems accepted and energisation dates.
Translate the evidence into their unit of production before the conversation, using the structure in
`CAR-07 — Building a portfolio of evidence`.

**Expecting the market to be described here.** This document says nothing about which sectors are growing,
hiring or paying, because the Institute has no survey data yet and will not assert what it cannot
evidence. See `SAL-01` for the instrument and `SAL-06` for how to read anyone else's numbers.

## 9. Worked example — a benchmark that survived the move and should not have

*Illustrative figures.* Currency-neutral, labour-hours only. A practitioner moves from a building fit-out
environment to a permit-controlled operating facility and carries one number with them: a cable
installation rate of 3.0 labour-hours per metre, which was reliable in the previous sector. The scope is
2,000 m. Assumptions: a 10-hour shift; a crew of 12; the same crew composition and the same physical
installation difficulty in both environments — that is, the *only* change is the working regime.

**The new environment's working regime.** Each shift loses 1.5 hours to permit issue, isolation
confirmation and gas testing, and 1.0 hour to travel and muster. Productive time per shift =
10 − 1.5 − 1.0 = **7.5 hours**.

**The productive-hour fraction.** 7.5 ÷ 10 = 0.75, so only 75 % of each paid shift hour is available at
the work face. The correction factor is the reciprocal: 10 ÷ 7.5 = **1.333**.

**The corrected rate.** 3.0 h/m × 1.333 = **4.0 labour-hours per metre**.

**The effect on the estimate.** At the carried rate: 2,000 m × 3.0 = **6,000 hours**. At the corrected
rate: 2,000 m × 4.0 = **8,000 hours**. The carried benchmark understates the labour by **2,000 hours**,
which is 2,000 ÷ 6,000 = **33.3 %** of the original estimate.

**The effect on the programme.** Crew capacity = 12 people × 10 hours = 120 site-hours per shift.
At 6,000 hours: 6,000 ÷ 120 = **50 shifts**. At 8,000 hours: 8,000 ÷ 120 = 66.7 → **67 shifts**. The
difference is **17 shifts** of duration that were never in the programme, on an activity that was priced
and sequenced with confidence because the practitioner had used that rate successfully many times.

**What should have transferred.** Not the rate — the method. The transferable technique is to measure the
productive-hour fraction in the new environment before committing to any rate: observe or reconstruct one
shift, subtract the regime losses, and derive the factor from the project's own data. Then verify it
against timesheets in the first full cycle and correct it. The professional who does this in their first
month is applying exactly the same skill that made their old benchmark reliable, which is the point: the
skill transferred, the number did not.

## 10. Checklist — making a sector move

- Answer the five questions in §1 about your new sector, in writing, before your first reporting cycle.
- List every benchmark you rely on — rates, percentages, lead times, typical growth — and mark each as
  unverified until you have derived it from this project's data.
- Get the definitions: what counts as started, complete, committed, handed over, and what float means
  contractually here.
- Find out who audits you, how often, and what they ask for. Read the last report they issued.
- Establish the decision cadence and design the cycle to serve it, not the cycle you are used to.
- Identify the single dominant constraint — access window, long-lead equipment, weather, permit regime,
  energisation date — and check that your reporting makes its status visible every period.
- Translate your prior evidence into the new sector's unit of production before your first interview or
  first internal presentation.
- Plan the entry itself with `CAR-08 — The first ninety days in a controls role`.

The practitioners who move sectors well are not the ones who knew the new sector already. They are the
ones who arrived assuming their instincts were wrong, and spent the first cycle rebuilding them from the
new project's own data — which is why the move costs a cycle rather than a career.

---

## Related

- `CAR-01 — The project controls career roadmap` — the six disciplines that transfer intact
- `CAR-02 — Routes into project controls` — the entry routes, whose gaps behave similarly to sector gaps
- `CAR-08 — The first ninety days in a controls role` — the plan for the first cycle after a move
- `BPG-06 — Progress measurement and rules of credit` — the distinction misread in §7
- `BPG-18 — Interface and subcontractor controls` — the interface problem that dominates EPC work
- `SAL-03 — Role taxonomy and levelling` — how roles are compared across sectors
- `SAL-06 — Using market data honestly` — why this document contains no sector market claims

## Sources and standards

Drawn from the PCI Body of Knowledge, principally Domain 5 (cost management and cost control), Domain 6
(earned value management and forecasting), Domain 7 (contracts, commercial management, bills of quantities,
invoicing and revenue) and Domain 10 (project scheduling). Sector characterisations are structural descriptions of
contract forms, production units and assurance regimes; no named project, organisation, market statistic
or demand claim is used, and none should be inferred.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
