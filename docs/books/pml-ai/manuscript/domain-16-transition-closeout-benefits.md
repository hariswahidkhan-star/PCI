# Domain 16 — Transition, Closeout and Benefits Realisation

## Why this domain exists

Fifteen domains have described how to choose work, structure it, plan it, price it, staff it, lead it
and control it. Every one of them assumed something that only this domain can supply: **that the
project ends, that something else takes it over, and that somebody eventually finds out whether it
was worth doing.** Until that happens, a project's entire claim to value rests on an estimate, and
the organisation's memory of it rests on whoever is still in post.

Closeout is, on this book's argument, the least respected phase in the delivery lifecycle and the
one in which value is most easily lost beyond recovery. The failure modes below are the ones a
reviewer meets repeatedly, and they are not primarily about paperwork. Handover happens on a date
rather than on a condition, so a receiving organisation that is 90 per cent ready absorbs a system
it cannot run. Contracts are left open because closing them requires a conversation nobody wants,
and the carrying cost of that avoidance is never added up. Lessons are captured diligently and never
retrieved, so the same defect is bought again at full price on the next programme. And benefits (the
entire justification for the expenditure) are measured, if at all, against a baseline that was never
taken, by an owner who was never appointed, over a period after everyone who could explain the
variance has moved on.

That is the domain's central claim: **the closing account is the only honest one, and it is settled
after the project ends, not at it.** A leader who cannot state what was realised, against what was
promised, decomposed into the reasons for the difference, has not completed the work. They have
merely stopped doing it. This domain makes that account computable. KA 16.1 replaces the handover
*date* with a handover *condition* and shows arithmetically why partial readiness is not
proportional readiness. KA 16.2 takes the receiving organisation's run cost seriously and prices the
final account, the retention mechanism and the specific cost of leaving a claim open. KA 16.3 treats
knowledge as an asset with a retrieval rate, and shows that the value of a post-project review is
destroyed at the retrieval step, not the capture step. KA 16.4 settles Meridian's account,
decomposes the variance, and sets the retention and deletion rules for the evidence, data and models
the programme leaves behind.

**Learning objectives.** After this domain a candidate can: specify handover as a set of conditions
rather than a date, and distinguish what handover transfers from what it cannot; design commissioning
and service-acceptance tests that test the service rather than the works; **separate the mandatory
preconditions of a transition, which are recorded met or not met and never traded, from the
discretionary conditions; compute the probability of a clean go-live as the conjunction of the
discretionary conditions, demonstrate why an averaged readiness dashboard systematically overstates
it, and rank remediation by proportional rather than absolute shortfall**; price a hold-or-go decision against remediation cost under both independent and
correlated failure assumptions; structure hypercare with exit criteria and a reversion plan; transfer
a service to a run organisation with its total cost of ownership stated; operate the retention
mechanism and strike a final account including variations, claim settlement and defect recovery;
**compute the monthly carrying cost of an open claim and the breakeven settlement premium**; run a
post-project review whose lessons are retrievable, and compute the breakeven reuse rate that makes it
worth holding; build a benefits measurement plan with a named owner, defined measures, a pre-change
baseline, a **fund type carried forward from the approval** and a stated duration; **decompose realised benefit against the business case into adoption,
benefit-per-unit and timing components, state the interaction term and the convention that assigns
it**; test attribution against a comparison cohort established at baseline; set a class-by-class
retention and deletion schedule for evidence, personal data and the models used in delivery
decisions; and govern AI use in measurement, review and archive without letting a tool author the
conclusion it is measuring.

**The master threads, and the figures this domain settles.** Meridian Care Records returns for the
last time: the clinical-records rollout to **40 clinics**, an approved cost of **USD 2,400,000**, a
business case claiming **USD 979,200** a year against Domain 1's honest **USD 685,440** at 70 per
cent adoption, a cost of delay of **USD 14,280** a week, and Domain 2's appraisal over eight years
at 7 per cent giving a present value of **USD 3,732,898** on the honest ramped profile and an NPV of
**+USD 1,332,898**. This domain measures what actually happened: **27 of 40 clinics** in daily use
at **5.4 clinician-hours** released a week, worth **USD 594,864** a year; and decomposes the **USD
90,576** shortfall against the honest case, prices the operating cost the case omitted, and computes
the realised NPV of **(USD 304,827)** and the **USD 180,000** of post-project work that turns it
positive. Project Auriga returns from Domains 6 to 8 (the 25-week control-systems upgrade for a
regional utility, **BAC USD 4,000,000**, `CPI` **0.91** and `SPI` **0.92** at week 13) to have its
commissioning test and final account settled in Case study B. The benefits measure used throughout
is the benefits register's benefit measure (`EVA(benefit)` in the shared symbol table, written in
words here to avoid the earned-value clash).

**Reference points.** Two areas of this domain have external reference points worth naming, and both
are named rather than relied upon. For the archive and retention material of KA 16.4.4, **ISO
15489** is the document usually named as addressing records management: what a record is, and the
controls that keep it authentic, reliable and usable over time. For the operational-transition
material of KA 16.2, **ISO/IEC 20000** is the document usually named as addressing service
management, which is the discipline the receiving organisation is running when it accepts a service
into support. Both are voluntary guidance: neither is legislation, neither obliges anyone of itself
unless an organisation, a contract or a regulator adopts it, and neither states any retention period
applicable to any organisation — retention requirements come from the applicable regime, the
contract and the organisation's records and legal functions, as KA 16.4.4 says throughout. They are
named here and not reproduced, and a reader who wants either should obtain the current edition from
its publisher. Naming them implies no endorsement in either direction.

---

## Knowledge Area 16.1 — Handover, commissioning and readiness

*Topics: 16.1.1 what handover transfers · 16.1.2 commissioning and service acceptance · 16.1.3
readiness as a conjunction of conditions · 16.1.4 the go-live decision, hypercare and reversion.*

### 16.1.1 What handover transfers

**Definition.** Handover is the transfer of a deliverable, and of the accountability for operating it,
from the delivering organisation to a named receiving organisation, on stated conditions, with the
supporting obligations of both parties recorded.

Three things transfer, and they are habitually confused. **Custody** of the asset or service, who
holds it, runs it, pays for it. **Accountability for its operation**, who answers when it fails at
02:00. And **the residual obligations of the deliverer**, defects liability, warranty,
documentation, training, transition support. Each has a different date, and a handover that names
one date for all three has hidden two of them.

Two things do **not** transfer, and pretending otherwise is the commonest and most expensive
handover error. **Accountability for the benefit does not transfer at handover, because it was never
the project's to begin with**: Domain 2 placed it with a benefits owner in the receiving
organisation from the outset, and handover is where that appointment is tested rather than where it
is made. And **the deliverer's accountability for what it built does not transfer either**: a defect
present at handover is a defect the deliverer owns, and the retention and defects-liability
mechanisms of 16.2.2 exist precisely because signature is not absolution.

**Handover on a condition, not a date.** The single structural choice that determines whether a
transition works is whether the handover trigger is a calendar date or a satisfied condition set. A
date creates an incentive to declare readiness; a condition set creates an incentive to achieve it.
The condition set must be agreed early enough that it can still influence the work — which in practice
means at the same gate that approves the delivery plan (Domain 3, KA 3.3.1) — and it must be written
so that each condition is objectively assessable by someone who is not the person delivering it.

### 16.1.2 Commissioning and service acceptance

**Definition.** Commissioning is the progressive proving of a deliverable in its operating
environment, from component test through integrated test to performance demonstration under
representative load; service acceptance is the receiving organisation's formal agreement that the
service, not merely the works, meets the agreed specification.

The distinction between **works tests** and **service tests** is where most acceptance regimes fail.
A works test asks whether the thing was built as specified. A service test asks whether the
organisation can run it: whether the runbook is correct, the alerts reach a person, the second-line
team can restore from backup within the stated window, and the fallback works when the primary does
not. A programme that has passed every works test and no service test has proved that it built the
right object and knows nothing about whether it delivered a service. Domain 9's acceptance and
nonconformance machinery supplies the discipline for both; the addition here is that the **service
test must be witnessed and run by the receiving organisation**, because a service test performed by
the project team tests the project team.

**Three commissioning disciplines** carry disproportionate weight. **Test on production
configuration**, since a test in a staging environment that differs from production tests the
staging environment: the defect most often found on the first live day is a configuration difference
nobody listed. **Test the failure paths, not only the success paths**: reversion, degraded mode,
manual fallback, and the restoration of service after an outage, because those are the paths that
will be used under stress by people who have never rehearsed them. And **record the evidence in a
form that survives the team**: witnessed test records, versioned configuration baselines (Domain 4,
KA 4.3.2), and the as-built documentation that 16.3.1 shows costs seven times as much to reconstruct
later.

### 16.1.3 Readiness as a conjunction of conditions

**Definition.** Readiness is the joint satisfaction of every condition necessary for a clean
transition. It has **two blocks**, and only one of them is arithmetic.

**The gate block: mandatory preconditions.** Some conditions of a transition are not assessments of
likelihood at all. A **clinical safety case and its hazard log closed by clinical governance**; a
**regulatory or licence approval granted** by the body empowered to grant it; a **data-protection or
privacy assessment signed** by the accountable authority; **notifications required of the
organisation made**; **contractual takeover certificates issued**. Each of these is recorded **met
or not met**, with the **approving authority named** and dated. None of them carries a probability,
none of them is weighted, and none of them is available for economic trade against the cost of
delay. The rule is one line and it has no exceptions clause: **while any item in the gate block is
not met, the transition does not proceed, whatever the arithmetic below says.** The gate block is
also not the project's to close. Each item is closed by the authority that owns it, which is the
point of naming that authority on the certificate rather than the project role that chased it.

**The discretionary block: conditions that admit degree.** What remains (training coverage,
migration reconciliation, device installation, workflow sign-off, champion recruitment, rehearsal of
the fallback) is genuinely uncertain at the decision date, genuinely a matter of more or less, and
genuinely tradeable against the cost of waiting. Readiness across *these* is a **conjunction**, and
its probability is the product of the conditions' probabilities: not their average, not their
weighted average, and not the percentage of conditions that are green. Everything that follows in
this KA is about the discretionary block; the gate block sits above it and is answered before it is
reached.

Keeping the two apart is the professional content of this section, because the failure is not that
anyone consciously prices a safety approval. It is that a single readiness template invites every
condition to be entered as a `p`, and a mandatory precondition given a probability at all has been
silently converted into a chance that something forbidden is permissible. Two consequences follow. A
gate-block item must never appear among the `pᵢ`, because a product that contains it can be raised
by improving something else, which is exactly the trade the item exists to forbid. And a gate-block
item must never appear in the go/hold economics of 16.1.4, because expressing it as a cost concedes
that some cost of delay would be large enough to buy it. Domain 14 states the same boundary for
security and privacy controls: expected annual loss is a legitimate test where consequences are
cost-compensable and the **wrong test** where the consequence is a penalty imposed by an authority,
a licence condition, a duty of confidence or harm to a person (KA 14.4.4); and Domain 9 states it
for quality, where the economic optimum is taken among *compliant* options rather than across them.
Domain 16 is where the boundary matters most, because this is the decision that is irreversible for
the people who did not take it.

The conjunction is then the most consequential piece of arithmetic in the domain, because the
artefact that organisations actually use (a readiness dashboard showing a weighted percentage) is
not a measurement of readiness at all. The product rule itself is Domain 15's (KA 15.1.3, where a
six-predecessor programme milestone with no predecessor assessed worse than 0.85 comes to 52.95 per
cent); what this KA adds is the contrast with the averaged dashboard, the remediation ranking that
follows from it, and the two correlation bounds the go-live decision must be tested against. With
`k` conditions each holding with probability `pᵢ`, and treating their failures as independent,

```
P(clean transition) = ∏ pᵢ  for i = 1 … k
```

whereas a dashboard reports `(Σ pᵢ)/k` or some weighted variant of it. Those two numbers diverge
quickly and in one direction: the product is always at or below the average, and the gap widens with
both the number of conditions and their dispersion. A programme with many conditions, most of them
nearly complete, will show a comfortable average and carry a poor probability of a clean transition,
which is the precise mechanism by which "we were 93 per cent ready" and "the go-live failed" are
both true statements about the same event.

**Worked example 16.1.3 — the readiness dashboard that said 90.71 per cent.**

1. **Setup.** Meridian's clinic go-live gate carries a gate block and **seven** discretionary
   conditions. The gate block (the clinical safety case and its hazard log closed by clinical
   governance, the information-governance assessment signed by the accountable authority, and the
   clinic's takeover certificate issued) was recorded **met**, authority named and dated, before the
   readiness assessment was convened; those items carry no probability and are not among the seven,
   which is why no clinical-safety condition appears in the list below. The transition manager's
   assessment of the probability that each *discretionary* condition is fully met at the planned
   go-live date, for a representative clinic, is: clinician training completed to the roster
   threshold **0.96**; data migration reconciled and signed off **0.90**; interfaces verified in
   production **0.94**; network and devices installed and load-tested **0.98**; workflow redesign
   signed off by the clinical lead **0.85**; clinical champion in post and trained **0.80**;
   fallback and business-continuity procedure rehearsed **0.92**. These are locally calibrated
   planning figures from the four pilot clinics, not constants. The programme dashboard reports
   readiness as the equal-weight average.
2. **Formula.** Dashboard reading `= (Σ pᵢ)/k`. Probability of a clean go-live `= ∏ pᵢ`.
3. **Substitution.** Average `= 6.35/7`. Product
   `= 0.96 × 0.90 × 0.94 × 0.98 × 0.85 × 0.80 × 0.92`.
4. **Result.** Dashboard reading **90.71 %**. Probability of a clean go-live **49.79 %**. The gap is
   **40.92 percentage points**.
5. **Interpretation.** The dashboard is not optimistic; it is measuring a different quantity, and
   the quantity it measures has no operational meaning. No one is interested in the average
   completeness of the conditions, only in whether all of them hold. Four consequences follow, and
   all four are actionable. **First, the running product is the useful display.** Reading the
   conditions in sequence, the conjunction falls 96.00 → 86.40 → 81.22 → 79.59 → 67.65 → 54.12 →
   **49.79 %**: the two conditions that are also the two least likely to be reported as blockers
   (workflow sign-off and the clinical champion, both owned outside the project, both part of Domain
   2's enabling change) together remove **25.47 percentage points**. **Second, remediation must be
   ranked by proportional shortfall, not absolute gap**, because lifting a condition from `p` to
   `p′` multiplies the conjunction by `p′/p`. Lifting the champion condition from 0.80 to 0.98
   multiplies by 1.2250 and gains **11.20 points**; lifting training from 0.96 to 0.98 multiplies by
   1.0208 and gains **1.04**. The weakest link is worth almost eleven times the strongest, and a
   remediation plan that works through the conditions in dashboard order will spend its budget on
   the wrong ones. **Third, certainty cannot be bought.** With all seven conditions at 0.98 the
   conjunction is only **86.81 %**, so **13.19 %** of go-lives still fail, which is the arithmetic
   case for the reversion plan of 16.1.4 rather than for more assurance. A **95 %** chance of a
   clean go-live would require **99.27 %** on every one of the seven, and a programme that claims
   that has almost certainly not assessed its conditions honestly. **Fourth, and the professional
   caution: independence is an assumption, and it must be stated rather than assumed.** If the
   failures share a root cause (an under-resourced change team that starves training, workflow and
   champion recruitment alike) then the conditions fail together and the conjunction rises towards
   `min(pᵢ) =` **80.00 %**. That is the optimistic bound, not the expected case, and 16.1.4 tests
   the go-live decision against both bounds rather than choosing between them.

> **Fig 16.1.1 — Readiness is a conjunction, not an average.** Seven go-live readiness conditions for
> one Meridian clinic, each with its assessed probability drawn as a grey column on a 0–100 % scale,
> with a brand-blue step line showing the running conjunction falling 96.00 → 86.40 → 81.22 → 79.59
> → 67.65 → 54.12 → **49.79 %**. A crimson dashed rule marks the equal-weight dashboard average of
> **90.71 %** and the **40.92-percentage-point** gap between the two is dimensioned. A right-hand
> panel ranks the gain in the probability of a clean go-live from lifting each single condition to
> 0.98 — champion **+11.20 pp** (`×1.2250`), workflow **+7.62 pp** (`×1.1529`), data migration
> **+4.43 pp**, fallback **+3.25 pp**, interfaces **+2.12 pp**, training **+1.04 pp** — showing the
> gain tracks the ratio `0.98/p` rather than the absolute gap. A footer states the two robustness
> anchors: all seven at 0.98 still gives only **86.81 %**, and a 95 % clean go-live needs **99.27 %**
> on every condition; the perfectly correlated bound is `min(p) =` **80.00 %**. Source: PCI original.
> Alt text: seven tall grey columns of similar height with a blue line descending steeply across them
> from ninety-six per cent to fifty per cent, a red dashed horizontal line near the top of the
> columns marking the averaged dashboard figure, and a side panel of six small blue bars ranking the
> value of fixing each condition.

### 16.1.4 The go-live decision, hypercare and reversion

**Definition.** The go-live decision is a gate in Domain 3's sense (a named authority, pre-set
criteria, and the power to hold as well as to proceed), whose distinguishing feature is that it is
substantially irreversible for the users, even where it is technically reversible for the system.

**The option set exists only above the gate block.** The three options below are available only once
**every** mandatory precondition of 16.1.3 is recorded met, with its approving authority named.
Where one is not (an unclosed safety case, an ungranted approval, an unsigned privacy assessment, a
notification not made, a takeover certificate not issued) there is no option set and there are no
economics: the only available decision is **hold**, and the arithmetic that follows does not apply
to it. A paper that prices going live against an open gate-block item has not presented a finely
balanced judgement; it has costed a decision that was not the decision-maker's to take, and the
correct response to it is to refuse the question rather than to answer it more carefully.

Above that line, the decision has three real options, not two: proceed, hold and remediate, or
proceed with a reduced scope of users. Pricing them requires the discretionary conjunction of 16.1.3
and a cost per failed transition, and it is worth doing because the intuitive answer is usually wrong
in the expensive direction.

**Worked example 16.1.4 — hold or go, tested against both correlation bounds.**

1. **Setup.** All **40** Meridian clinics face the readiness profile of 16.1.3. A clinic that goes
   live unready reverts to paper: remediation costs **USD 24,000** in re-training, re-migration and
   support, and the clinic loses **6 weeks** of benefit at `6 × 85 =` **USD 510** a week, so a failed
   go-live costs **USD 27,060**. Three options: **(1)** go now; **(2)** a targeted two-week uplift
   costing **USD 38,000** that lifts the two weakest conditions (champion and workflow) to 0.98;
   **(3)** a blanket three-week uplift costing **USD 96,000** that lifts all conditions to 0.98.
   Deferred benefit is priced at Domain 1's cost of delay, **USD 14,280** a week. Evaluate each option
   under both the independent conjunction and the perfectly correlated bound `min(pᵢ)`.
2. **Formula.** Total cost `= (weeks deferred × cost of delay) + uplift spend + 40 × (1 − P(clean)) ×
   27,060`, with `P(clean) = ∏ pᵢ` in the independent case and `min(pᵢ)` in the correlated case.
3. **Substitution.** Option 1 independent: `40 × (1 − 0.4979) × 27,060`. Option 3 independent:
   `3 × 14,280 + 96,000 + 40 × (1 − 0.98⁷) × 27,060`. Correlated cases substitute
   `min(pᵢ) =` 0.80, 0.90 and 0.98 respectively.
4. **Result.**

   | Option | Independent (`∏ pᵢ`) | Correlated bound (`min pᵢ`) |
   |---|---|---|
| 1, go now | **USD 543,445** (20.08 failed clinics) | **USD 216,480** (8.00 failed) |
| 2 (targeted uplift, 2 weeks, 38,000) | **USD 387,766** (11.87 failed) | **USD 174,800** (4.00 failed) |
| 3 (blanket uplift, 3 weeks, 96,000) | **USD 281,581** (5.27 failed) | **USD 160,488** (0.80 failed) |

5. **Interpretation.** The blanket uplift is the **dominant** option (best under both bounds), which
   is the result a leader wants, because the correlation structure is exactly what nobody knows at
   the time of the decision. It saves **USD 261,864** against going now if the failures are
   independent and **USD 55,992** if they share a single root cause, for a total commitment of **USD
   138,840** (three weeks of deferred benefit at 42,840 plus the 96,000 spend). Three points
   generalise beyond Meridian. **The delay cost is small relative to the failure cost, and it is the
   only one that is visible.** Forty-two thousand dollars of deferred benefit appears in a report as
   a slipped milestone; half a million dollars of remediation appears as twenty separate operational
   incidents attributed to change resistance. That asymmetry is a large part of why go-live
   decisions tend to be taken too early. **The breakeven uplift spend is enormous**: holding remains
   worthwhile up to **USD 357,864** of uplift cost under the independent case, which is 3.7 times
   what the blanket remediation actually costs, so the decision is not close, and a leader arguing
   it should say so rather than presenting it as a fine balance. **And the residual matters more
   than the improvement.** Even after the blanket uplift, **5.27** clinics are expected to fail,
   because 0.98 across seven conditions is 86.81 per cent and no amount of readiness work makes a
   conjunction certain. The professional response is not further assurance but a rehearsed reversion
   plan and a wave structure that limits the exposure of any single go-live, which is what the
   remaining cost buys.

**Hypercare** is the bounded period of elevated support immediately after go-live, and it needs
three things a project rarely gives it: a **stated duration**, **exit criteria expressed as a
measurement rather than a date**, and a **named owner in the receiving organisation from day one**
rather than at the end. Meridian's design used an incident rate per clinic per week with a threshold
of **0.8** sustained for two consecutive weeks; the observed series over weeks 4 to 7 ran 1.4, 0.9,
0.6, 0.5, so the two-consecutive-week condition was first satisfied at **week 7** and hypercare
exited then rather than at the planned week 6, which is the point of a measured exit criterion.
Hypercare that ends on a date ends by transferring an unstable service.

**The reversion plan** is the option the go-live arithmetic proves is needed. It must state the
decision-maker, the trigger, the maximum time to revert, what happens to data created since go-live,
and, the element most often missing, the **latest point at which reversion is still possible**,
because after that point the plan is a comfort document rather than a control. A reversion plan that
has never been rehearsed is an assumption, and 16.1.2's failure-path testing is where it stops being
one.

### AI in this KA

**Where it earns its place.** Assembling the readiness picture, which is a data-collection problem
across many sources (training records, migration reconciliation reports, device inventories, test
results), and reconciling them into one condition-by-condition status with the source of each
assertion named. Computing the conjunction and the proportional-shortfall ranking across dozens of
sites, which is deterministic and tedious. Reading test evidence packs against the agreed acceptance
criteria and listing criteria for which no evidence exists: a genuinely valuable gap-finding task,
and one humans do badly at scale. Drafting runbooks and reversion procedures from configuration
baselines, for verification by the people who will use them.

**Where it must not go.** It must not record a gate-block item as met. A mandatory precondition is
closed by the authority that grants it, and an assembled status line evidences that a document
exists, never that an approval was given. The distinction that makes the named approving authority,
rather than the collated status, the thing the certificate carries. It must not set the readiness
probabilities. Those are judgements about specific organisations and specific people, and a model
asked for them will supply confident, provenance-free numbers that then drive a real go-live
decision, precisely the failure Domain 14 KA 14.3 names. It must not issue a service acceptance,
which is an accountable act by the receiving organisation. And it must not author a test result: a
generated evidence record cannot evidence that a test was witnessed, and an acceptance regime
resting on unwitnessable evidence is worse than none, because it looks complete.

**Verification, concretely.** Every probability carries a named human assessor and a stated basis
(pilot data, comparable site, judgement). Every AI-assembled status line carries a link to its
source record and is sampled by the transition manager at a stated rate before the gate. The
conjunction is reproduced by hand for the go-live paper (it is six multiplications) and the paper
states both correlation bounds rather than one, so the decision body can see the robustness rather
than a point estimate.

### Key terms — KA 16.1

| Term | Meaning |
|---|---|
| **Handover** | Transfer of custody and operational accountability for a deliverable to a named receiving organisation, on stated conditions. |
| **Condition-based handover** | Handover triggered by a satisfied condition set rather than a calendar date. |
| **Commissioning** | Progressive proving of a deliverable in its operating environment, from component test to performance demonstration. |
| **Works test / service test** | Whether the thing was built as specified / whether the organisation can actually run it. |
| **Service acceptance** | The receiving organisation's formal agreement that the service meets specification. |
| **Mandatory precondition** | A go-live condition recorded met or not met, with its approving authority named; it carries no probability and admits no economic trade. |
| **Gate block** | The set of mandatory preconditions sitting above the readiness arithmetic; while any item is not met the only available decision is hold. |
| **Discretionary condition** | A readiness condition that admits degree, is genuinely uncertain at the decision date, and is therefore assessed as a probability. |
| **Readiness conjunction** | `P(clean transition) = ∏ pᵢ` over the `k` necessary **discretionary** conditions; always at or below the averaged dashboard figure. |
| **Proportional shortfall** | The ratio `p′/p` by which lifting a condition multiplies the conjunction: the correct remediation ranking. |
| **Hypercare** | A bounded period of elevated support after go-live, with measured exit criteria and a receiving-organisation owner. |
| **Reversion plan** | The rehearsed route back, with a trigger, a decision-maker, a time limit and a latest-possible-reversion point. |

### Sample MCQs — KA 16.1

**MCQ 16.1-A `[16.1.3 · Application]`** Seven readiness conditions, whose failures are assessed as
independent, hold with probabilities 0.96, 0.90, 0.94, 0.98, 0.85, 0.80 and 0.92. All seven are
necessary for a clean go-live. The probability of a clean go-live is closest to:
- A. 90.71 %
- B. 80.00 %
- C. 49.79 % ✅
- D. 47.83 %

*Rationale:* The conjunction is the product, `0.96 × 0.90 × 0.94 × 0.98 × 0.85 × 0.80 × 0.92 = 49.79
%` (16.1.3). A is the equal-weight average, the dashboard figure, and a different quantity. B is
`min(pᵢ)`, the perfectly correlated bound, which the stem's independence assumption excludes and
which is in any case the optimistic case. D rounds the dashboard reading to 0.90 and raises *that*
to the seventh power, `0.90⁷ = 47.83 %`, an answer that both rounds and double-counts the averaging.

**MCQ 16.1-B `[16.1.3 · Analysis]`** Seven independent readiness conditions, holding at 0.96, 0.90,
0.94, 0.98, 0.85, 0.80 and 0.92, give a conjunction of 49.79 %. The gain in the probability of a
clean go-live from lifting one condition is governed by:
- A. the absolute gap to the target, with an equal gain per percentage point of gap closed
- B. the ratio `p′/p`, so the same gap closed at a lower `p` yields a larger gain ✅
- C. the condition's weight on the readiness dashboard
- D. `1/k`, equally across all seven conditions

*Rationale:* Lifting a condition from `p` to `p′` multiplies the conjunction by `p′/p`, so the gain
is `∏pᵢ × (p′/p − 1)` (16.1.3). Closing two points of gap at 0.80 is worth **1.24 pp** while closing
the same two points at 0.96 is worth **1.04 pp**, which is exactly what A denies, a common target
ranks the conditions in the same order either way, but the magnitudes are set by the ratio. B is
therefore the statement that generalises. C substitutes an artefact for the arithmetic. D confuses
"a product" with "symmetric in its factors".

**MCQ 16.1-C `[16.1.3 · Evaluation]`** A programme wants a 95 % probability of a clean go-live across
seven independent readiness conditions. The minimum probability required on each condition is closest
to:
- A. 95.00 %
- B. 99.27 % ✅
- C. 98.00 %
- D. 99.90 %

*Rationale:* `p = 0.95^(1/7) = 99.27 %` (16.1.3). A applies the target to each condition rather than
to their conjunction. C gives only `0.98⁷ = 86.81 %`. D is stricter than required and would be
rejected as unachievable, losing the argument the arithmetic wins.

**MCQ 16.1-D `[16.1.4 · Application]`** Forty sites each face seven independent readiness conditions
giving a 49.79 % probability of a clean go-live; a failed go-live costs 27,060. A three-week hold
costs 42,840 in deferred benefit plus 96,000 of remediation and lifts all seven conditions to 0.98.
The expected saving from holding is closest to:
- A. USD 138,840
- B. USD 261,864 ✅
- C. USD 404,605
- D. USD 543,445

*Rationale:* Going now costs `40 × (1 − 0.4979) × 27,060 = 543,445`; holding costs `42,840 + 96,000
+ 40 × (1 − 0.8681) × 27,060 = 281,581`; the saving is **261,864** (16.1.4). A is the cost of
holding, not the saving. C omits the residual remediation after the uplift and so credits the hold
with the whole 543,445, giving `543,445 − 138,840 = 404,605`. The error that makes readiness work
look better than it is. D is the cost of going now with no credit for the hold at all.

**MCQ 16.1-E `[16.1.1 · Comprehension]`** Which of the following does **not** transfer at handover?
- A. custody of the asset
- B. accountability for operating the service
- C. accountability for realising the benefit ✅
- D. responsibility for paying the running costs

*Rationale:* The benefits owner sits in the receiving organisation from the outset (Domain 2,
KA 2.3.1), so handover tests that appointment rather than creating it; a benefit "handed over" at
closeout has no owner and no baseline (16.1.1).

### Self-check — KA 16.1

1. *Why does an averaged readiness dashboard overstate readiness, and in which direction is the
   error always signed?* — Readiness is a conjunction, so its probability is the product of the
   conditions' probabilities; a product is always at or below the average, so the dashboard error is
   always optimistic, and it grows with the number of conditions and their dispersion.
2. *A works test has passed and a service test has not been run. What has been proved?* — That the
   thing was built as specified, and nothing whatever about whether the organisation can run it.
3. *What must a hypercare period state that a date cannot?* — A measured exit criterion, so that the
   period ends when the service is stable rather than when the calendar says so.
4. *Which readiness conditions get a probability, and which do not?* — Discretionary conditions,
   which admit degree, get a `pᵢ`; mandatory preconditions are recorded met or not met with their
   approving authority named, and appear neither in the product nor in the go/hold economics (16.1.3).
5. *A safety case is not yet closed and the cost of delay is large. What is the option set?* — Hold.
   There is no option set below the gate block, and pricing the alternatives would state a figure at
   which the precondition could be waived (16.1.4).

---

## Knowledge Area 16.2 — Operational transition and contract closeout

*Topics: 16.2.1 the run organisation and total cost of ownership · 16.2.2 defects liability, warranty
and retention · 16.2.3 the final account · 16.2.4 the cost of leaving a claim open.*

### 16.2.1 The run organisation and total cost of ownership

**Definition.** Operational transition is the establishment of a permanent capability to run, support,
fund and improve what the project delivered, with the running cost recognised in the receiving
organisation's budget before the project closes.

The last clause is where business cases go to die. A project's cost is a capital number, visible,
approved and controlled; the service's running cost is an operating number that appears in someone
else's budget, in a later year, and is therefore very frequently **not in the business case at
all**. Domain 2's whole-life requirement (KA 2.3.3) exists to prevent this, and Meridian's approved
case failed it: it carried no operating-cost line whatever. The measured run cost of the records
service is **USD 108,000** a year (hosting **42,000**, licences **39,000**, second-line support
**27,000**), and over the case's own eight-year appraisal at 7 per cent that is a present value of
**USD 644,900**, which is 48.4 per cent of the honest case's entire NPV. A programme that hands over
a service without a funded run line has not transferred it; it has abandoned it in a place where
someone will find it.

**What the run organisation needs at transition**, stated as a checklist because its items are
countable and its gaps are cheap to find: a named service owner and a funded support model with stated
hours and response targets; runbooks and as-built documentation at the configuration version actually
deployed; a licence and contract register with renewal dates and the notice periods that govern them;
an access and privilege model with the project team's own access **removed** on a stated date; a
capacity and obsolescence view stating when the platform will next need investment; and the benefits
measurement duties of 16.4.1, which are operating duties, not project ones.

### 16.2.2 Defects liability, warranty and retention

**Definition.** Retention is a proportion of the contract sum withheld from payment as security for
the contractor's completion and defect-rectification obligations, released in stages against defined
events. A **defects-liability period** is the interval after acceptance during which the contractor
must rectify defects that appear; a **warranty** is a broader promise about the deliverable's
condition or performance, typically longer and narrower in scope.

The mechanism is simple and it is misused in two opposite directions. Held too long or released too
loosely, retention becomes either a commercial grievance that poisons the closing relationship or a
security that was never there when it was needed. The disciplines are: **release against events, not
dates** (takeover, end of defects liability, completion of outstanding documentation); **state what the
retention secures**, since a retention held against defects cannot be applied to a performance
shortfall unless the contract says so; and **track outstanding items as a priced list from the day of
takeover**, because a retention release argued from an unpriced snagging list is argued from nothing.
Domain 10's contract mechanisms supply the drafting; the closeout discipline is that the retention
release is a decision with a record (Domain 3, KA 3.3.4), not an accounts-payable event.

The jurisdictional caution is unavoidable and it is real: retention practice, its permissibility, any
requirement to hold it in trust, and the limitation periods on latent defects vary by
jurisdiction and by contract family. Nothing in this domain states a legal position; it states a
management discipline that must be operated within whatever legal frame applies.

### 16.2.3 The final account

**Definition.** The final account is the single agreed statement of everything owed under a contract
at its conclusion: the original sum, all approved variations, the settlement of any claims, all
deductions and recoveries, and the resulting final payment, signed by both parties as full and
final.

**Worked example 16.2.3 — Meridian's final account and retention release.**

1. **Setup.** The systems integrator's contract sum was **USD 1,680,000**. Approved variations total
   **USD 96,000**. Retention was **5 %** of the contract sum, half released at takeover of the last
   clinic. A prolongation claim was settled at **USD 74,000** (16.2.4). Three clinics' interface
   defects were rectified by another supplier at a cost of **USD 18,600**, recoverable from the
   integrator. Strike the final account, the final retention release and the final payment.
2. **Formula.** Final account `=` contract sum `+` variations `+` claim settlement `−` recoveries.
   Final payment `=` final account `−` amounts already paid.
3. **Substitution.** Retention `1,680,000 × 0.05 = 84,000`; certified works `1,680,000 + 96,000 =
   1,776,000`; paid on certification `1,776,000 − 84,000 = 1,692,000`; plus the takeover release of
   `84,000/2 = 42,000`, so **1,734,000** paid to date. Final account
   `1,680,000 + 96,000 + 74,000 − 18,600`.
4. **Result.** Final account **USD 1,831,400**: **9.01 %** above the original sum. Final retention
   release `42,000 − 18,600 =` **USD 23,400**. Final payment `1,831,400 − 1,734,000 =` **USD
   97,400**, which reconciles as `42,000 + 74,000 − 18,600 = 97,400`.
5. **Interpretation.** The reconciliation in the last line is the whole point of the example, and it
   is the check that a final account is actually closed rather than merely totalled: **the final
   payment must be explainable as the sum of the specific movements since the last certificate**,
   and a final payment that cannot be so explained conceals either an unrecorded variation or a
   double-counted recovery. Three professional observations. The **recovery is only collectable
   because it was made against the retention that was still held**: had the full 84,000 been
   released at takeover, the 18,600 would have become a debt to be pursued from a contractor with no
   remaining incentive, which is the practical argument for staged release and the reason "release
   it, they have been good to work with" is a decision and not a courtesy. The **9.01 % growth over
   the original sum is the number the organisation should carry into its next estimate**, not the
   change-control record of variations alone: variations were **5.71 %** of the original sum, the
   claim settlement added **4.40 %** and the defect recovery returned **1.11 %**, netting to 9.01 %,
   so an organisation that benchmarks only its approved variations will understate contract growth
   by roughly the net settlement rate every time. And the account must be closed with a **written
   full and final statement**, because the alternative (a balance agreed in correspondence and never
   formalised) is the state in which 16.2.4's carrying cost accrues.

### 16.2.4 The cost of leaving a claim open

**Definition.** The carrying cost of an open claim is the recurring cost, per period, of it
remaining unresolved: external advisory fees, internal management attention, and the opportunity
cost of the cash locked in retention and provision, none of which appears as a claim cost in any
ledger.

Open items are left open because closing them requires accepting a number, and accepting a number
feels like a loss while carrying an item feels like prudence. The arithmetic reverses that intuition
reliably, and it is worth being able to do in a meeting.

**Worked example 16.2.4 — what Meridian's open claim cost per month, and the premium worth paying.**

1. **Setup.** The integrator claimed **USD 148,000** for prolongation. Meridian's quantity assessment
   put the defensible value at **USD 62,000**, and a provision of the full claimed amount was held.
   Carrying the claim costs: external commercial advice **USD 3,250** a month; internal management
   **1.5 days** a month at **USD 700** a day; and the opportunity cost of the **USD 190,000** locked
   up (42,000 of retention plus the 148,000 provision) at **6 %** a year. Left to formal
   determination, the claim would close at **month 14**, with outcomes assessed at **USD 40,000**
   (probability 0.25), **USD 62,000** (0.50) and **USD 110,000** (0.25). The alternative is to settle
   at **month 2**. What settlement price is worth paying?
2. **Formula.** Carrying cost `c =` advisory `+` internal `+` locked cash `× rate/12`. Expected cost
   of determination `= EMV + c × 14`. Breakeven settlement price at month 2
   `= EMV + c × 14 − c × 2`. Breakeven premium over the assessed value
   `= c × Δmonths + (EMV − assessed)`.
3. **Substitution.** `c = 3,250 + 1.5 × 700 + 190,000 × 0.06/12 = 3,250 + 1,050 + 950`.
   `EMV = 0.25 × 40,000 + 0.50 × 62,000 + 0.25 × 110,000`.
4. **Result.** Carrying cost **USD 5,250 a month**: **USD 73,500** over fourteen months, more than
   the claim's own assessed value. `EMV` **USD 68,500**. Expected cost of fighting to determination
   **USD 142,000**. Breakeven settlement price at month 2 **USD 131,500**, a breakeven **premium**
   over the assessed 62,000 of **USD 69,500**, which the identity `12 × 5,250 + (68,500 − 62,000) =
   63,000 + 6,500` reproduces exactly. Meridian settled at **USD 74,000**, a premium of **12,000**,
   or **17.27 %** of the premium that would still have been worth paying, saving **USD 57,500**
   against the expected cost of determination.
5. **Interpretation.** The identity is the transferable result: **the breakeven early-settlement
   premium is the carrying cost times the months saved, plus the difference between the expected
   determination and your own assessment.** It says that time, not merit, dominates small and medium
   claims — twelve months of carrying cost at 5,250 is 63,000, almost exactly the assessed value of
   the claim itself, so an organisation that fights a 62,000 claim for a year has spent the claim to
   win the claim. It also identifies which lever to pull first: of the 5,250, **USD 4,300** is
   advisory and management attention that stops the moment the item closes, and USD 950 is
   opportunity cost that stops when the cash releases. Four cautions keep this honest. The
   arithmetic is **not an argument for settling everything**, where a claim raises a point of
   principle that will recur across a supplier's other contracts, the precedent value can exceed the
   carrying cost, and that must be quantified rather than asserted. The **internal management figure
   is real cost and is almost never counted**, which is exactly why the carrying cost is invisible;
   1.5 days a month of a senior commercial manager is a modest estimate and doubling it doubles the
   case for settling. The **assessment must precede the negotiation**, because a settlement premium
   can only be judged against a defensible assessed value, and an organisation that settles without
   one is guessing, not paying a premium. And a settlement must be **documented as full and final
   for defined matters** (drafting that belongs with qualified legal advice, since what a release
   covers and what it cannot reach is a legal question and not a management one), or the carrying
   cost simply resumes under a different heading.

### AI in this KA

**Where it earns its place.** Reconciling a final account against the change register, the payment
certificates and the variation approvals, and listing every item that appears in one and not the
others: a document-reconciliation task at which it is fast, thorough and genuinely better than a
tired human at month-end. Extracting a licence and contract register with renewal and notice dates
from a contract set, which is the single most useful artefact the run organisation receives and the
one most often missing. Building the carrying-cost model above across a portfolio of open items and
ranking them by monthly cost, which turns "we have eleven open claims" into a prioritised list.
Summarising a claim's correspondence history for a settlement paper, with every assertion
referenced.

**Where it must not go.** It must not value a claim. A claim valuation is an expert commercial and
technical judgement with contractual and evidential foundations, it will be tested by people who are
paid to test it, and a model's plausible number carries no provenance and no accountable author. It
must not agree a final account or authorise a retention release. Both are accountable decisions with
a record. And its summary of a correspondence trail must never be relied on in a dispute without
verification against the source documents: the summary is a convenience, the versioned original is
the evidence (Domain 3, KA 3.3.4).

**Verification, concretely.** Every reconciliation exception is confirmed against the source
certificate before it is reported. The carrying-cost model's components are each traceable to an
invoice, a timesheet or a stated cost-of-capital assumption, and the assumption is named in the
paper. The valuation on which any settlement rests is signed by the accountable commercial lead, not
by the analysis that supported it. And the final account carries a human signature on both sides:
the one part of closeout where the signature is the deliverable.

### Key terms — KA 16.2

| Term | Meaning |
|---|---|
| **Operational transition** | Establishment of a funded, staffed, permanent capability to run and improve the delivered service. |
| **Total cost of ownership** | Whole-life cost of the service, including the operating cost that must appear in the receiving organisation's budget before closeout. |
| **Retention** | A proportion of the contract sum withheld as security, released against defined events. |
| **Defects-liability period** | The interval after acceptance in which the contractor must rectify defects that appear. |
| **Final account** | The single agreed statement of the original sum, variations, settlements, deductions and final payment, signed as full and final. |
| **Contract growth** | Final account against original sum, variations *and* settlements, not variations alone. |
| **Carrying cost of an open item** | Advisory + internal management + opportunity cost of locked cash, per period; invisible in every ledger. |
| **Breakeven settlement premium** | `carrying cost × months saved + (EMV of determination − own assessed value)`. |

### Sample MCQs — KA 16.2

**MCQ 16.2-A `[16.2.3 · Application]`** A contract sum of 1,680,000 carries 96,000 of approved
variations, a claim settled at 74,000 and an 18,600 recovery for defects rectified by others. The
final account is:
- A. USD 1,776,000
- B. USD 1,831,400 ✅
- C. USD 1,850,000
- D. USD 1,757,400

*Rationale:* `1,680,000 + 96,000 + 74,000 − 18,600 = 1,831,400` (16.2.3). A is the certified works
value, omitting both the settlement and the recovery. C omits the recovery. D omits the settlement.

**MCQ 16.2-B `[16.2.4 · Application]`** Carrying an open claim costs 5,250 a month. Determination at
month 14 has an expected value of 68,500; the alternative is settlement at month 2. The breakeven
settlement price is:
- A. USD 68,500
- B. USD 131,500 ✅
- C. USD 142,000
- D. USD 73,500

*Rationale:* `68,500 + 14 × 5,250 − 2 × 5,250 = 131,500` (16.2.4). A ignores carrying cost entirely.
C is the expected cost of fighting, without crediting the two months still carried under the
settlement option. D is fourteen months of carrying cost alone.

**MCQ 16.2-C `[16.2.4 · Analysis]`** A claim costing 5,250 a month to carry, with an expected
determination of 68,500 at month 14 and settlement available at month 2, is assessed by the client at
62,000 against the contractor's claimed 148,000. The breakeven **premium** over the assessed value is
69,500. The most defensible reading is that:
- A. the claim should be conceded in full at 148,000
- B. up to 69,500 above the assessed value can be paid to close twelve months earlier, because
  carrying cost times months saved plus the gap between the expected determination and the assessment
  is 63,000 + 6,500 ✅
- C. the claim should be fought, since 62,000 is the defensible figure
- D. the premium equals the provision held

*Rationale:* The identity `c × Δm + (EMV − assessed)` gives 69,500 (16.2.4). A ignores the
assessment altogether. C is the intuition the arithmetic overturns, winning at 62,000 after fourteen
months costs 135,500. D confuses a provision with a price.

**MCQ 16.2-D `[16.2.1 · Evaluation]`** A business case with no operating-cost line hands over a
service whose measured run cost is 108,000 a year. Over an eight-year appraisal at 7 %
(`AF = 5.971299`), the omission is worth:
- A. USD 864,000
- B. USD 644,900 ✅
- C. USD 108,000
- D. USD 540,000

*Rationale:* `108,000 × 5.971299 = 644,900` (16.2.1). A is the undiscounted eight-year total, the
commonest error and an overstatement of 34.0 %. C is a single year. D applies a five-year horizon
undiscounted.

**MCQ 16.2-E `[16.2.2 · Analysis]`** A client holds 84,000 of retention, releases half of it at
takeover, and later recovers 18,600 for defects that another supplier had to rectify. Releasing the
whole retention at takeover rather than in stages would most directly have:
- A. improved the contractor's cash position at no cost to the client
- B. converted an 18,600 recovery into a debt to be pursued from a contractor with no remaining
  incentive ✅
- C. breached the defects-liability period
- D. reduced the final account by 18,600

*Rationale:* Retention is security, and its value is precisely that it is still held when a recovery
arises (16.2.2). C confuses a payment mechanism with a liability period; D reverses the direction of
the recovery.

### Self-check — KA 16.2

1. *Why is the operating cost the business-case line most often missing, and what does that cost
   Meridian?* — It falls in another budget in a later year, so nobody in the approval chain owns it;
   at 108,000 a year over eight years at 7 % it is a present value of 644,900, or 48.4 % of the
   honest case's whole NPV.
2. *What are the three components of the carrying cost of an open claim, and which is largest?* —
   External advisory, internal management attention and the opportunity cost of locked cash; the first
   two together are 4,300 of Meridian's 5,250 a month, and both stop the day the item closes.
3. *What check proves a final account is closed rather than merely totalled?* — The final payment
   must reconcile to the specific movements since the last certificate, here 42,000 of retention
   plus 74,000 of settlement less 18,600 of recovery equals the 97,400 due.

---

## Knowledge Area 16.3 — Knowledge transfer and post-project review

*Topics: 16.3.1 transferring knowledge to the run organisation · 16.3.2 the post-project review ·
16.3.3 the economics of a lesson · 16.3.4 closing the project organisation.*

### 16.3.1 Transferring knowledge to the run organisation

**Definition.** Knowledge transfer is the deliberate conversion of what the project team knows into
forms the receiving organisation can use without them: documentation at the deployed configuration
version, trained people, and rehearsed procedures.

The failure is rarely a refusal to document; it is that documentation is treated as a deliverable
rather than as a **test**. A runbook is adequate when someone who was not on the project can execute
it under supervision, and nothing short of that observation proves it. Two measurements make the gap
visible cheaply. **Key-person concentration**: of Meridian's 34 operational procedures, **11** were
executable by exactly one named individual at handover, a **32.35 %** single-point-of-knowledge
rate, and each of those 11 is an availability risk that no risk register had recorded because it was
not a project risk. And **documentation currency against the as-built baseline** (Domain 4, KA
4.3.2), since documentation written against the design and never updated against the deployment
describes a system that does not exist.

The economics are one-sided and worth stating because they are always resisted at the moment they
matter. Three of Meridian's clinics had interface configurations that were changed during
commissioning and never recorded. Reconstructing each afterwards (reading the live configuration,
re-testing, re-documenting) cost **USD 8,400**, a total of **USD 25,200**, against **USD 1,200**
each to document at the time, or **USD 3,600**: a ratio of **7.00**. The multiple is Meridian's own
and is not a constant, but its direction follows from a mechanism that does not change with the
context, at handover the knowledge is in someone's head and the cost is a note; afterwards it must
be re-derived from the artefact by someone who never had it. An organisation that wants the multiple
for its own estimating has to measure it, and the measurement is cheap because both costs are
already invoiced.

### 16.3.2 The post-project review

**Definition.** A post-project review is a structured examination, after delivery, of what happened
and why, conducted to improve the organisation's future decisions rather than to assess its people.

The design faults are predictable and each has a countermeasure. **Held too late**, when the team
has dispersed and only the outcome is remembered, not the reasoning; the countermeasure is to hold
it within a stated window of handover and to run interim reviews at gates rather than one review at
the end. **Held as an attribution exercise**, which guarantees defensive testimony; the
countermeasure is an explicit and enforced separation from performance assessment, and the
psychological-safety conditions Domain 12 sets out. **Focused on events rather than on decisions**,
so it records that the schedule slipped rather than which decision, taken on which information, made
the slip likely; the countermeasure is to start from the decision record (Domain 3, KA 3.3.4) and
ask of each material decision whether it was reasonable on what was known: a question that produces
transferable findings where "what went wrong" produces narrative. And **producing recommendations
with no owner**, which is how a review generates activity and no change; every finding needs a named
owner, a date and a place in a standing process that will be looked at again.

The most valuable single addition to a review agenda is the **estimate-versus-actual comparison on
the organisation's own parameters**: the contract growth of 16.2.3, the adoption ramp of 16.4.2, the
readiness probabilities of 16.1.3 against what actually happened. Those numbers are what calibrate
the next business case, and they are the only output of a review that improves an estimate rather
than a behaviour.

### 16.3.3 The economics of a lesson

**Definition.** A lesson's value is the cost it avoids when it is retrieved and applied, weighted by
the probability that it is retrieved and applied at all. Capture without retrieval has a value of
zero, and this is not a rhetorical point; it is where almost all of the value is lost.

**Worked example 16.3.3 — what Meridian's post-project review was worth.**

1. **Setup.** The review cost **USD 46,000** in workshop time, analysis and write-up, and produced
   **34** lessons. From the organisation's own history, a lesson that is retrieved before a comparable
   decision avoids an average of **USD 26,000** of rework or delay. In the current, unindexed
   repository, **12 %** of captured lessons are retrieved before a comparable decision. A proposal
   would spend a further **USD 18,000** indexing the lessons against the specific decision points at
   which they apply, raising retrieval to an estimated **35 %**.
2. **Formula.** Expected recovery `=` lessons `×` retrieval rate `×` avoided cost per applied lesson.
   Breakeven retrieval rate `=` review cost `÷` (lessons `×` avoided cost).
3. **Substitution.** At 12 %: `34 × 0.12 × 26,000`. At 35 %: `34 × 0.35 × 26,000`. Breakeven:
   `46,000/(34 × 26,000) = 46,000/884,000`.
4. **Result.** Expected recovery **USD 106,080** at the current retrieval rate and **USD 309,400**
   with indexing, an increment of **USD 203,320** for **USD 18,000**, a ratio of **11.30**. The
   review breaks even at a retrieval rate of **5.20 %**; with the index it must reach **7.24 %**.
   Net value of review plus index at 35 % retrieval: **USD 245,400**.
5. **Interpretation.** Three results, in ascending order of importance. The review is worth holding,
   at 2.31 times its cost even with a retrieval rate most organisations would be embarrassed to
   report. **The breakeven retrieval rate of 5.20 % is the number that should end the recurring
   argument about whether reviews are worth the time**: they are, comfortably, provided anyone ever
   reads them, and the honest reading of the widespread belief that they are not is that in many
   organisations the retrieval rate really is near zero, at which point the belief is correct and
   the remedy is not to stop reviewing. **And the highest-return intervention available is not
   better capture but better retrieval**: an 11.30-to-one return on indexing, against which any
   proposal to improve the workshop is not competitive. This inverts where effort normally goes:
   organisations invest in the review event, which is visible and social, and not in the retrieval
   mechanism, which is unglamorous infrastructure. The professional cautions are that the 26,000
   avoided cost and both retrieval rates are **organisational parameters that must be measured, not
   assumed**: the honest way to establish the retrieval rate is to instrument the repository and
   count, and an organisation that cannot state its retrieval rate is not in a position to argue
   about the value of its reviews; and that a lesson only avoids cost if it is **specific enough to
   act on**, since "communicate better" has an avoided cost of zero at any retrieval rate, which is
   why lesson quality and lesson count are different measurements.

### 16.3.4 Closing the project organisation

**Definition.** Closing the project organisation is the deliberate dissolution of the temporary
structure: its authority, its accounts, its access rights, its records and its people.

Four items are routinely left undone, and each has a specific consequence. **Authority is not
withdrawn**, so a change board with no project continues to approve changes to a live service that
now belongs to someone else: the governance defect of Domain 3 KA 3.1.1, arriving after the project
has ended. **Cost accounts are left open**, so late charges land in a closed period, the outturn
moves after it was reported, and the estimate-versus-actual comparison of 16.3.2 is calibrated on a
wrong number. **Access rights persist**, which is a security exposure with a named cause and a
trivial fix (Domain 14, KA 14.4). And **people are released without a record of what they did**,
which is both an individual injustice and the reason the organisation cannot find, two years later,
the person who knows why the interface was configured that way.

The leadership content of this topic is not administrative. A team that has spent two years on a
programme experiences its dissolution as an ending, and the manner of it materially affects whether
those people will volunteer for the next difficult programme. Recognition that is specific and
attributable, honest conversations about what each person did well and what they should develop
(Domain 12, KA 12.3), and a visible route to their next role are not courtesies. They are the
mechanism by which delivery capability survives the project that consumed it.

### AI in this KA

**Where it earns its place.** Indexing lessons against the decision points at which they apply,
which is exactly the intervention 16.3.3 prices at an 11.30-to-one return, and is a classification
and retrieval problem of the kind the technology is genuinely good at. Clustering findings across
many reviews to expose the recurring cause that no single review can see: the organisational
analogue of Domain 3's re-decision count. Drafting as-built documentation from configuration
baselines and change records for human verification. Generating candidate review questions from the
decision record so the review starts from decisions rather than from recollection. Producing the
first draft of a transition-out handbook from existing artefacts.

**Where it must not go.** It must not conduct the review. A post-project review is partly a
conversation about judgement under uncertainty, and its value comes from people saying things they
would not write down; a transcript-and-summarise exercise gets the record and loses the content. It
must not attribute cause, because causal attribution in a delivery failure is contested, consequential
for named individuals, and not a text-processing task. And it must not be the author of a lesson that
is then relied on: an unattributable lesson cannot be interrogated, and a repository of them decays
into plausible advice.

**Verification, concretely.** Every indexed lesson is checked against its source finding on a
sampled basis, and the retrieval rate is instrumented and reported rather than estimated. Every
AI-drafted document is executed by someone who was not on the project before it is accepted as
adequate, the same test as any other runbook (16.3.1). And every finding that names a cause is
confirmed by the accountable human reviewer before it leaves the room.

### Key terms — KA 16.3

| Term | Meaning |
|---|---|
| **Knowledge transfer** | Conversion of project knowledge into documentation, trained people and rehearsed procedures usable without the project team. |
| **Runbook adequacy test** | Whether someone who was not on the project can execute it under supervision: the only proof documentation works. |
| **Key-person concentration** | The share of procedures executable by exactly one named individual; an availability risk no project register records. |
| **Post-project review** | Structured examination of what happened and why, to improve future decisions, separated from performance assessment. |
| **Retrieval rate** | The proportion of captured lessons retrieved before a comparable decision; the term in which almost all lesson value is lost. |
| **Breakeven retrieval rate** | review cost ÷ (lessons × avoided cost per applied lesson): the threshold above which reviewing pays. |
| **Transition-out** | Dissolution of the project organisation: authority withdrawn, accounts closed, access revoked, records placed, people released well. |

### Sample MCQs — KA 16.3

**MCQ 16.3-A `[16.3.3 · Application]`** A review costing 46,000 produces 34 lessons; an applied
lesson avoids 26,000 on average. The breakeven retrieval rate is closest to:
- A. 5.20 % ✅
- B. 12.00 %
- C. 43.36 %
- D. 52.04 %

*Rationale:* `46,000/(34 × 26,000) = 46,000/884,000 = 5.20 %` (16.3.3). B is the organisation's
current retrieval rate, not the breakeven. C is `46,000/106,080`, the share of the *expected
recovery* consumed by the review, which is a cost-recovery ratio and not a rate. D misplaces the
decimal, a common slip on a small ratio.

**MCQ 16.3-B `[16.3.3 · Evaluation]`** A review costing 46,000 produces 34 lessons, an applied lesson
avoids 26,000 on average, and 12 % of captured lessons are currently retrieved before a comparable
decision. The highest-return improvement is to:
- A. run a longer, better-facilitated review workshop
- B. spend 18,000 indexing the lessons against the decision points where they apply, raising retrieval
  from 12 % to 35 % ✅
- C. capture more lessons per project
- D. shorten the review to reduce its 46,000 cost

*Rationale:* Indexing yields `34 × 0.23 × 26,000 = 203,320` for 18,000, a ratio of 11.30 (16.3.3). A
and C invest in capture, where the value is not being lost. D reduces a cost that is already
recovered 2.31 times over.

**MCQ 16.3-C `[16.3.1 · Application]`** Three undocumented interface configurations cost 8,400 each
to reconstruct after handover, against 1,200 each to document at the time. The ratio of avoidable to
incurred cost is:
- A. 7.00 ✅
- B. 3.00
- C. 21,600 to 1
- D. 2.33

*Rationale:* `25,200/3,600 = 7.00`, which is also the per-item ratio `8,400/1,200` (16.3.1). B counts
the three items rather than the cost ratio. C is the absolute saving, `25,200 − 3,600 = 21,600`,
mislabelled as a ratio. D divides the *per-item* reconstruction cost by the *total* documentation cost,
`8,400/3,600 = 2.33`, mixing a unit figure with a total.

**MCQ 16.3-D `[16.3.2 · Analysis]`** The review practice most likely to produce transferable findings
rather than narrative is to:
- A. begin from the schedule variance and work backwards
- B. begin from the decision record and ask of each material decision whether it was reasonable on
  what was known at the time ✅
- C. invite every stakeholder to describe their experience
- D. rank the events of the project by impact

*Rationale:* Decisions are the unit at which an organisation can improve; events are consequences
(16.3.2, and Domain 3 KA 3.3.4 on the retrospective question). A and D produce narrative; C produces
testimony without a comparison.

### Self-check — KA 16.3

1. *What is the only proof that a runbook is adequate?* — That someone who was not on the project can
   execute it under supervision.
2. *Where is the value of a post-project review actually lost, and what does that imply about where
   to invest?* — At retrieval, not at capture; so indexing lessons against the decisions they bear
   on returns far more than improving the workshop, 11.30 to one on Meridian's figures.
3. *Name two consequences of failing to close the project organisation.* — A change board with no
   project continues to authorise changes to someone else's live service, and open cost accounts let
   late charges move an already-reported outturn, corrupting the estimating calibration.

---

## Knowledge Area 16.4 — Benefits measurement, responsible archive and model/data retention

*Topics: 16.4.1 the benefits measurement plan · 16.4.2 the closing account · 16.4.3 attribution and
the comparison cohort · 16.4.4 responsible archive and retention · 16.4.5 model and data retention for
AI-assisted decisions.*

### 16.4.1 The benefits measurement plan

**Definition.** A benefits measurement plan states, for each benefit in the register, the measure and
its exact definition, the baseline value and the date it was taken, the target and its profile over
time, the frequency and method of measurement, the named owner accountable for realisation, the
**fund type** — cashable (with the budget line named and the finance owner's written confirmation),
capacity-released, cost-avoidance or non-financial (Domain 15, KA 15.2.1) — the reporting route, and
the date the measurement obligation ends.

**The fund type is not an accounting nicety, and it is the element most often missing.** It decides
*what the realised number can be compared against*, and therefore whether the closing account of
16.4.2 is answering the question the approval was granted on. A benefit approved on a cash test and
realised as capacity has not failed; it has been measured against the wrong comparator, and the two
readings will be argued about for a year. Carrying the classification from the approval into the
measurement plan is what stops that, and it also carries the **finance owner's confirmation** with
it: where a benefit was approved as cashable, the measurement plan names the budget line that was to
come off and the period it was to come off in, so the realisation review can ask the only question
that settles it: *did it?* An unconfirmed cashable claim is measured as capacity-released until the
confirmation exists, exactly as it is recorded in the portfolio register.

Every element earns its place by a failure it prevents, and three deserve emphasis because they are
the ones that make a measured number arguable.

**The measure's exact definition, including its denominator.** Meridian registered "clinics in daily
use" as its adoption measure, with "daily use" defined as at least one clinical episode recorded in
the system on at least four days of a working week, averaged over a month. At the twelve-month
benefits review the figure was **27 of 40 clinics, 67.50 %**. An informal figure of **68 %** had
circulated, the number Domain 1's case study records, computed on a *clinician* count rather than a
clinic count; the two measures differ because the non-adopting clinics are smaller than average.
Neither number is wrong; only one is the registered measure, and the discipline that matters is that
the definition was fixed before measurement and the review reports the registered measure with the
variant identified. A programme that discovers at measurement time that its measure has two readings
will end up reporting whichever reading is more comfortable.

**The baseline, taken before the change, at the date recorded.** Domain 2 KA 2.3.2 required this and
Meridian complied: clinician time on records tasks was sampled in all 40 clinics in the quarter before
the first go-live. Without that, every number in 16.4.2 would be a reconstruction, and a
reconstruction is an estimate shaped by the result it is used to justify.

**The end date of the measurement obligation.** Benefits measurement that runs indefinitely is not
measured at all; the plan should state the period (Meridian's is the eight years of the appraisal,
with formal reviews at 12, 24 and 48 months), and what happens at the end, which is normally that
the measure either becomes a permanent operational indicator or is retired with a reason.

### 16.4.2 The closing account

**Definition.** The closing account is the reconciliation of realised value against the value the
business case promised, decomposed into the components that explain the difference, and reported to the
body that approved the case.

This is the account the whole book has been keeping. Meridian's business case claimed **USD 979,200** a
year on an output basis; Domain 1 corrected that to **USD 685,440** at 70 per cent adoption, and
Domain 2 appraised the honest ramped profile at a present value of **USD 3,732,898** and an NPV of
**+USD 1,332,898**. The measurement is now in.

**Worked example 16.4.2 — the closing account of Meridian Care Records.**

1. **Setup.** At the twelve-month benefits review, with all 40 clinics live: **27 clinics** meet the
   registered daily-use definition (**67.50 %** adoption), and time sampling in the adopting clinics
   shows **5.4 clinician-hours** released a week rather than the **6.0** the case assumed. The
   valuation rate is unchanged at **USD 85** an hour over **48** operating weeks, and the comparison
   cohort of 16.4.3 shows no material change from other causes. Measured adoption by year, against
   the case's 40 % / 60 % / 70 % ramp, was **15 %**, **40 %** and **60 %**, reaching the measured
   steady state of 67.50 % only in year 4: the ramp arrived a full year later than the case assumed.
   The run cost is **USD 108,000** a year (16.2.1) against no operating-cost line in the case, and
   the capital outturn was **USD 2,514,000** against **USD 2,400,000** approved.
2. **Formula.** Annual benefit `= U × a × h`, where `U =` clinics `×` rate `×` weeks
   `= 40 × 85 × 48 =` **163,200** is the benefit per unit of adoption-hours, `a` is adoption and `h` is
   hours released per adopting clinic per week. Two-factor variance decomposition:
   adoption effect `= U × h_plan × (a_actual − a_plan)`; hours effect
   `= U × a_actual × (h_actual − h_plan)`.
3. **Substitution.** Case `163,200 × 0.70 × 6.0`; measured `163,200 × 0.675 × 5.4`. Adoption effect
   `163,200 × 6.0 × (0.675 − 0.700)`; hours effect `163,200 × 0.675 × (5.4 − 6.0)`.
4. **Result.** Measured steady-state benefit **USD 594,864** a year (≈ SAR 2.23 million
   indicatively) against the honest case's **USD 685,440**: a shortfall of **USD 90,576**, or
   **13.21 %**. The realised figure is **86.79 %** of the honest case and exactly **60.75 %** of the
   flat claim the case actually made. Decomposed, taking adoption first: adoption **(USD 24,480)**,
   hours per adopting clinic **(USD 66,096)**, total **(USD 90,576)**.

   The full present-value bridge, eight years at 7 % (`AF = 5.971299`):

   | Bridge line | NPV (USD) |
   |---|---|
   | Flat claim, as approved | **+3,447,096** |
   | Adoption term corrected (Domain 1's term, valued in Domain 2) | (2,114,198) |
   | **Honest ramped case (Domain 2)** | **+1,332,898** |
| Level variance (measured hours 5.4 and steady adoption 67.50 %) | (465,015) |
| Timing variance (the ramp arrived one year late) | (413,809) |
   | Operating cost omitted from the case (108,000 a year) | (644,900) |
   | Cost outturn variance (2,514,000 against 2,400,000) | (114,000) |
   | **Realised NPV** | **(304,827)** |
   | Post-project improvement plan | +340,156 |
   | **Post-plan NPV** | **+35,329** |

   *Rows are rounded to the nearest dollar; subtotals are struck at full precision, so a row-by-row
   addition may differ by USD 1. The two variance lines are counterfactuals and the convention is
   the same one used on the annual decomposition, first factor at plan, second at actual. The
   **level** line values the measured 5.4 hours and 67.50 % steady adoption on the **case's own**
   adoption timing (40 % in year 1, 60 % in year 2, steady from year 3), and the **timing** line is
   the further loss from the measured ramp of 15 % / 40 % / 60 % reaching steady state only in year
   4. Reversing the order would report timing at (479,771) and level at (399,053): the same total,
   with **USD 65,962** of level-and-timing interaction moved between the lines.*

5. **Interpretation.** Six readings, and they are the closing lessons of the book. **First, the
   largest single line is not a delivery failure at all.** The **2,114,198** correction is the
   adoption term the business case omitted — money that was never available to be realised, and
   which Domain 1's arithmetic removed before a single clinic was installed. An organisation that
   measures realisation against the approved claim will record a **USD 384,336** annual shortfall
   and conclude the programme failed; measured against the honest case the shortfall is **USD
   90,576**, and the difference between those two numbers is a defect in the case, not in the
   delivery. **Second, the decomposition's order matters and the convention must be stated.** Taking
   adoption first gives (24,480) adoption and (66,096) hours; taking hours first gives (68,544)
   hours and (22,032) adoption. Both sum to (90,576); the **USD 2,448** that moves between the lines
   is the interaction term, `163,200 × (−0.025) × (−0.6)`, and it exists because both factors moved.
   The convention used here (first factor at plan, second at actual) assigns it to the second line,
   and a report that does not say which convention it used has published two of its three numbers
   ambiguously. **Third, the hours error is nearly three times the adoption error**, which is the
   opposite of what the programme's own narrative said: everyone discussed adoption, because
   adoption is visible and Domain 1's case study had made it the headline. The benefit *per adopting
   unit* was never re-measured until this review, and it was wrong by 10 per cent: a reminder that a
   benefits case has two quantitative assumptions and organisations habitually monitor one.
   **Fourth, timing cost almost as much as level**: (413,809) against (465,015), from nothing more
   than the ramp arriving a year late. A benefit deferred is a benefit partly destroyed, and this is
   the same arithmetic as the cost of delay from Domain 1 seen from the other end. **Fifth, the
   realised NPV is negative. And and it is negative because of the line the project never
   controlled.** Strip the operating cost out and the realised position is **+USD 340,073**; the run
   cost the case never carried is what turns the account. That is the single most transferable
   finding in this domain: **the omission of an operating cost line is frequently the whole margin,
   not a presentational defect.** **Sixth, the account is not closed by the project.** The
   improvement plan (**USD 180,000** at the end of year 3 to complete the workflow redesign in the
   thirteen non-adopting clinics and re-run the champion programme, lifting adoption to **80 %** and
   restoring the full **6.0** hours from year 5) is worth `188,496 × 2.584087 =` **USD 487,090** in
   present value against a present cost of **USD 146,934**: an NPV of **+USD 340,156** and a
   benefit-cost ratio of **3.32**. It converts the programme's realised NPV from **(304,827)** to
   **+35,329**. The whole value of a USD 2.4 million programme came to rest on USD 180,000 of
   post-project adoption work that appeared in no project plan, and the professional caution
   attached is exact: that plan needs an owner, a budget line and a decision, and at the moment the
   project closes there is no project to provide any of the three. The breakeven is worth stating
   for the board: with the plan's USD 180,000 spent, the account needs an effective realisation
   level of **78.60 %** of full potential in years 5 to 8 to reach zero, against **60.75 %** on the
   current trajectory and the **80 %** the plan is designed to deliver.

> **Fig 16.4.1 — The closing account of Meridian Care Records.** Waterfall bridge in NPV terms, USD,
> eight years at 7 % (`AF = 5.971299`), with subtotal columns in solid brand blue, decrements in
> crimson, and the single increment outlined in blue: flat claim as approved **+3,447,096** →
> adoption term corrected **(2,114,198)** → honest ramped case **+1,332,898** → level variance
> **(465,015)** → timing variance **(413,809)** → operating cost omitted **(644,900)** → cost outturn
> variance **(114,000)** → **realised NPV (304,827)** → improvement plan **+340,156** → **post-plan
> NPV +35,329**. A zero rule crosses the plot so the realised column's position below it is
> unmistakable. A footer states the measured steady state — 27 of 40 clinics in daily use (67.50 %)
> releasing 5.4 clinician-hours a week, **USD 594,864** a year, **60.75 %** of the flat claim and
> **86.79 %** of the honest case — and notes that the account turns positive on USD 180,000 of
> post-project adoption work. Source: PCI original. Alt text: a waterfall chart descending in five
> crimson steps from a tall blue column of three and a half million dollars to a short blue column
> below the zero line, then rising through one outlined column to a small positive column.

### 16.4.3 Attribution and the comparison cohort

**Definition.** Attribution is the determination of how much of an observed change was caused by the
project rather than by anything else happening at the same time. A **comparison cohort** is a set of
comparable units, measured at baseline and at review but not receiving the change (or receiving it
later), against which the observed change is netted.

Attribution is the argument that destroys benefits claims, and it is unwinnable after the fact.
Meridian handled it correctly and cheaply: because the rollout ran in waves, the **8 clinics** in the
final wave were measured at the same baseline and again at the twelve-month review while still on the
old process. Their records-task time changed by **0.0 hours a week**, within the sampling tolerance of
**±0.3 hours**, so no attribution adjustment was made and the full 5.4 hours is claimed as
attributable.

The value of that cohort is best seen by supposing it absent. A reviewer could then assert that a
concurrent national workflow initiative accounted for, say, **0.6** of the 5.4 hours. That assertion
is unfalsifiable without a comparison, and it is expensive: at **4.8** attributable hours the
steady-state benefit falls to **USD 528,768** a year, the realised present value of benefits falls
to **USD 2,536,954**, and the realised NPV falls from **(304,827)** to **(621,946)**. The claim more
than doubles the measured loss. A comparison cohort that costs a few days of sampling therefore
protects a figure of that order, and the professional rule follows directly: **the attribution
question must be settled by design at baseline, not by argument at review**, because at review the
only available instruments are assertion and seniority.

Two honest limitations. A wave-based comparison cohort is not a randomised comparison, and the later
waves may differ systematically from the earlier ones (Meridian's did, being smaller), so the cohort
bounds the attribution question rather than closing it, and the review should say so. And where no
comparison is possible at all, the correct treatment is to report the benefit in its physical unit
with the attribution uncertainty stated, not to convert an uncertain quantity into a confident
monetary claim (Domain 2, KA 2.3.2 on cash-releasing and non-cash-releasing benefits).

**A correction the closing account forces on the rest of the book.** Every delay calculation in
Domains 3, 4 and 6 was priced at Domain 1's cost of delay of **USD 14,280** a week, derived from 28
adopting clinics releasing 6 hours at 85. The realised rate is `27 × 5.4 × 85 =` **USD 12,393** a
week: **86.79 %** of it, meaning the figure used throughout was **15.23 %** higher than realisation
justifies. The professional obligation is to re-test the conclusions, not to restate them, and the
result is reassuring: Domain 3's delegation-threshold analysis saved **USD 171,360** a year at
14,280 and saves **USD 148,716** at 12,393, moving the breakeven value-destruction rate on the
delegated band from 84 % to **72.90 %**: still far beyond any plausible delegate. **A conclusion
that survives a 15 per cent error in its principal input is a robust conclusion, and one that does
not survive should never have been presented as a point estimate.** That test is the most useful
thing a closing account gives back to the organisation's future decisions.

### 16.4.4 Responsible archive and retention

**Definition.** The archive is the retained record of the project: its decisions, contracts, evidence,
configurations and data, held for a stated period against stated purposes, with a defined disposal or
de-identification action at the end. Retention is a **schedule by class**, never a single rule.

Two forces pull in opposite directions and both are legitimate. Evidence must be kept, because the
questions that arrive later (a dispute, an audit, a latent defect, an inquiry) can only be answered
from records that exist, at the versions relied on. And personal data must **not** be kept, because
holding it creates obligations and exposure that grow with time and volume while its usefulness
falls. A single retention rule cannot serve both, which is why the schedule is class-by-class.
Meridian's 6.4 TB archive resolves into: contract and commercial evidence **1.9 TB**; the decision
and governance record **0.4 TB**; design, configuration and as-built **1.6 TB**; test and
commissioning evidence **1.4 TB**; and personal and patient-linked migration logs **1.1 TB**.

**Worked example 16.4.4 — retention as insurance, and retention as liability.**

1. **Setup.** Storing the archive costs **USD 21** per terabyte per month. The programme's assessed
   probability that a dispute, audit or latent-defect question will require the contractual and
   technical evidence within seven years is **0.18**, and the assessed cost of being unable to
   answer it (an adverse determination or an unrecoverable rectification) is **USD 310,000**.
   Separately, the **1.1 TB** of patient-linked migration logs can be de-identified for a one-off
   **USD 34,000**, removing an assessed breach exposure of probability **0.04** and consequence
   **USD 1,250,000**.
2. **Formula.** Cost of retention `=` volume `×` unit rate `×` months. Value of retention
   `= P(need) × cost of not having it`. Value of de-identification `=` exposure removed `+` storage
   avoided `−` de-identification cost. Breakeven need probability `=` retention cost `÷` consequence.
3. **Substitution.** `6.4 × 21 = 134.40` a month, `× 12 × 7`. `0.18 × 310,000`. De-identification:
   `0.04 × 1,250,000 + 1.1 × 21 × 12 × 7 − 34,000`.
4. **Result.** Seven years of retention costs **USD 11,290**; the expected value of holding the
   evidence is **USD 55,800**: a ratio of **4.94**, with a breakeven need probability of **3.64 %**.
   De-identifying the patient-linked logs removes **USD 50,000** of expected exposure and **USD
   1,940** of storage for **USD 34,000**, a net gain of **USD 17,940**.
5. **Interpretation.** The two halves of the result point in opposite directions and that is the
   whole lesson. **Retaining evidence is extraordinarily cheap insurance** (it pays at a need
   probability above 3.64 %) roughly one chance in twenty-seven (and no serious programme can claim
   its evidence is less likely than that to be wanted), so the reflex to purge storage for cost
   reasons is almost always wrong, and the correct argument for deleting is never cost. **Retaining
   personal data is the reverse trade**: the same volume that costs almost nothing to store carries
   an exposure that dwarfs its storage cost by a factor of 26 to one, and de-identification pays for
   itself several times over even before any regulatory dimension is considered. Three cautions. The
   breakeven is so low that the **decision is really about the consequence estimate**, and a
   programme that cannot articulate what it would lose by being unable to answer has not thought
   about the risk. Retention periods for personal and health data are **set by law and by contract
   in each jurisdiction and by data class**, they differ substantially, and nothing here states a
   legal minimum or maximum. The schedule must be agreed with whoever holds the data-protection
   accountability in the organisation, and the figures above are an illustration of the economics,
   not of the law. And **de-identification must be tested, not asserted**: a log that can be
   re-identified by joining it to another retained dataset has not been de-identified, and the test
   belongs in the schedule.

### 16.4.5 Model and data retention for AI-assisted decisions

**Definition.** Model and data retention is the retained record of what an AI system was asked, what
it returned, which model version returned it, who verified the output and what decision relied on
it: the minimum that permits an AI-informed decision to be explained after the fact.

Domain 14 established the AI-use register and the verification standard proportional to consequence;
this topic is the retention consequence, and it is the one closeout usually discovers too late. Of
the **214** AI-assisted outputs that Meridian's programme relied on in recorded decisions, **61**
cannot be reproduced because the model identifier and version were not captured at the time: **28.50
%** of the AI-informed decision base is therefore unexplainable, and re-deriving those outputs by
hand at **USD 1,850** each would cost **USD 112,850**, which nobody will spend. The fix costs
nothing and is three fields on a record that was being written anyway: **model identifier, model
version, date**, alongside the prompt or input, the output, and the named human verifier.

Retention here has its own class distinctions. Prompts and outputs relied on in a decision belong
with the decision record and inherit its retention period (Domain 3, KA 3.3.4). Training or
fine-tuning data, where the organisation created any, carries the personal-data considerations of
16.4.4 and usually the strictest treatment in the schedule. Model artefacts themselves may be
unretainable (a third-party service's version may simply cease to exist), which is precisely why the
*record of what it produced and who verified it* is the durable evidence, and why a retention plan
that assumes the model will still be available is not a plan. The closing obligation is a short one,
and it is assessable: **for every AI-informed decision in the record, a reader two years later can
identify the model, the input, the output, the verifier and the decision-maker.** Anything less
means the organisation cannot explain its own decisions, which is the accountability defect Domain 1
named and Domain 14 quantified, arriving at the archive.

### AI in this KA

**Where it earns its place.** Assembling the measurement data: extracting usage counts against the
registered definition, reconciling them to the source systems, and flagging units whose
classification changed between periods, which is where measured adoption series usually break.
Computing the variance decomposition across many benefits and both decomposition orders, and
reporting the interaction term explicitly. Classifying an archive by data class against a retention
schedule and listing records whose class cannot be determined: the highest-value output, because
unclassified records are the ones that get kept by default. Detecting records missing the
model-identifier fields of 16.4.5 across a large decision log.

**Where it must not go.** It must not decide what the measured benefit was. The measure's
definition, the treatment of edge cases and the attribution judgement are accountable determinations
that will be read as the organisation's own statement about its performance. It must not generate
the explanation of a variance: a model asked why realisation fell short will produce a fluent,
plausible causal account that is indistinguishable in tone from an evidenced one, and a benefits
report containing one has substituted narrative for measurement. And it must not authorise a
deletion: disposal under a retention schedule is an accountable, usually irreversible act, and it
must carry a human authorisation and a record of what was destroyed and on what authority.

**Verification, concretely.** Every measured figure traces to a source-system query that a second
person can re-run, and the query is retained with the figure. The decomposition is reproduced by
hand for the report (it is four multiplications) and both orders are computed so the interaction
term is visible rather than absorbed. Attribution rests on the comparison cohort's measured data,
not on an argument. And every disposal action generates a record identifying the class, the volume,
the authority and the date, which is itself retained under the longest applicable period.

### Key terms — KA 16.4

| Term | Meaning |
|---|---|
| **Benefits measurement plan** | Measure definition, baseline and date, target profile, method, frequency, owner, **fund type**, reporting route and end date, per benefit. |
| **Fund type (carried from approval)** | Cashable with the budget line named and the finance owner's written confirmation, capacity-released, cost-avoidance or non-financial (Domain 15, KA 15.2.1). It decides what the realised number may be compared against, and an unconfirmed cashable claim is measured as capacity-released. |
| **Registered measure** | The single definition of a benefit measure fixed before measurement; variants are identified, not substituted. |
| **Closing account** | Reconciliation of realised value against the promise, decomposed into its causes and reported to the approving body. |
| **Benefit per unit of adoption-hours (`U`)** | clinics × valuation rate × operating weeks, the constant in `benefit = U × a × h`. |
| **Level variance** | The value effect of a wrong steady-state adoption or benefit-per-unit assumption. |
| **Timing variance** | The value effect of the benefit profile arriving later than the case assumed. |
| **Interaction term** | The product of both factor movements; the amount that shifts between decomposition lines when the order changes. |
| **Comparison cohort** | Comparable units measured at baseline and review without receiving the change, used to net out other causes. |
| **Retention schedule** | Class-by-class statement of what is kept, for how long, on what authority, and what disposal or de-identification follows. |
| **Model retention record** | Model identifier, version and date alongside input, output, verifier and decision; the minimum that makes an AI-informed decision explainable. |

### Sample MCQs — KA 16.4

**MCQ 16.4-A `[16.4.2 · Application]`** Twenty-seven of 40 clinics are in daily use, each releasing
5.4 hours a week at USD 85 over 48 weeks. The measured annual benefit is:
- A. USD 685,440
- B. USD 594,864 ✅
- C. USD 881,280
- D. USD 660,960

*Rationale:* `27 × 5.4 × 85 × 48 = 594,864` (16.4.2). A is the honest business-case figure at 70 %
adoption and 6.0 hours. C omits the adoption term, counting all 40 clinics at the measured 5.4 hours.
D keeps the correct 27 clinics but the case's 6.0 hours, omitting the benefit-per-unit correction.

**MCQ 16.4-B `[16.4.2 · Analysis]`** With `U = 163,200`, planned adoption 0.700 and 6.0 hours, and
measured adoption 0.675 and 5.4 hours, the decomposition taking **adoption first** gives:
- A. adoption (24,480); hours (66,096) ✅
- B. adoption (22,032); hours (68,544)
- C. adoption (24,480); hours (68,544)
- D. adoption (66,096); hours (24,480)

*Rationale:* Adoption at planned hours is `163,200 × 6.0 × (−0.025) = (24,480)`; hours at measured
adoption is `163,200 × 0.675 × (−0.6) = (66,096)`; both sum to (90,576) (16.4.2). B is the reverse
order: correct arithmetic, wrong to the question asked. C mixes the two orders and over-states the
total by the 2,448 interaction term. D transposes the labels.

**MCQ 16.4-C `[16.4.2 · Evaluation]`** Meridian's realised NPV is (304,827). Which single line is
most responsible, and what does that imply?
- A. the adoption shortfall; the rollout under-delivered
- B. the operating cost omitted from the case, at (644,900); without it the realised position is
  +340,073, so the whole margin turned on a line the project never controlled ✅
- C. the cost outturn variance of (114,000); the programme overspent
- D. the timing variance; benefits arrived late

*Rationale:* The operating-cost line is the largest realised-period movement and exceeds the whole
negative position (16.4.2, 16.2.1). A and D are real but smaller, (465,015) and (413,809). C is the
smallest line at 4.75 % of approved cost.

**MCQ 16.4-D `[16.4.3 · Analysis]`** A reviewer claims a concurrent national initiative accounts for
0.6 of the 5.4 measured hours. The measurement design element that answers the claim is:
- A. a larger time-sampling exercise in the adopting clinics
- B. the comparison cohort of clinics measured at baseline and review without the change ✅
- C. a sensitivity analysis on the valuation rate
- D. the benefits owner's professional judgement

*Rationale:* Only a comparison can net out other causes, and it must exist from baseline (16.4.3);
without it the assertion is unfalsifiable and, at 4.8 attributable hours, would move the realised NPV
from (304,827) to (621,946). A measures the same population more precisely and answers a different
question.

**MCQ 16.4-E `[16.4.4 · Application]`** A 6.4 TB archive costs USD 21 per TB per month. If the
probability that the evidence is needed within seven years is 0.18 and the cost of being unable to
answer is 310,000, the breakeven need probability is closest to:
- A. 18.00 %
- B. 3.64 % ✅
- C. 0.52 %
- D. 36.42 %

*Rationale:* `6.4 × 21 × 12 × 7 = 11,290`; `11,290/310,000 = 3.64 %` (16.4.4). A is the assessed
probability, not the breakeven. C uses one year of storage. D misplaces the decimal.

**MCQ 16.4-F `[16.4.5 · Comprehension]`** The minimum retention that makes an AI-informed decision
explainable two years later is:
- A. the model artefact itself, retained locally
- B. the prompt and the output
- C. model identifier and version, date, input, output, named verifier and decision-maker ✅
- D. a summary of the analysis in the decision record

*Rationale:* A third-party model version may cease to exist, so the durable evidence is the record of
what it produced and who verified it (16.4.5). B omits the version and the verifier, which is exactly
the omission that made 28.50 % of Meridian's AI-informed decision base unexplainable.

### Self-check — KA 16.4

1. *Why must the measure's denominator be fixed before measurement?* — Because a measure with two
   readings will be reported at whichever reading is more comfortable; Meridian's 67.50 % of clinics
   and the informally circulated 68 % of clinicians are both defensible arithmetic and only one is the
   registered measure.
2. *Which decomposition line moves when the order is reversed, and by how much?* — The interaction
   term, `163,200 × (−0.025) × (−0.6) =` 2,448, which is why the convention must be stated.
3. *State the opposite economics of retaining evidence and retaining personal data.* — Evidence pays
   as insurance above a 3.64 % need probability, so cost is never the argument for deleting it;
   personal data carries exposure that dwarfs its storage cost, so de-identification pays for
   itself, 17,940 net on Meridian's 1.1 TB.
4. *Why does the measurement plan carry the fund type forward from the approval?* — Because the fund
   type decides what the realised number may be compared against. A benefit approved on a cash test
   and realised as capacity has not failed; it has been measured against the wrong comparator. Where
   a benefit was approved as cashable, the plan names the budget line and the period, so the review
   can ask whether it came off (16.4.1; Domain 15, KA 15.2.1).

---

## Advanced topics — Domain 16

### 16.A.1 Benefits decay and sustainment

Benefits are not a stock, they are a flow that depends on a behaviour continuing, and behaviours
revert. Where the enabling change is not maintained (champions move on, new staff are trained on the
old workflow because that is what the induction pack says, a system update degrades a step nobody
owns) realisation decays. Meridian's post-review assessment was a **4 % relative decline a year** in
the effective realisation level from year 4 in the absence of active sustainment: 0.6075 of full
potential falling to 0.5832, 0.5599, 0.5375, 0.5160 and 0.4953 across years 4 to 8. The present
value of that decay is **USD 216,824**, against a sustainment programme (refresher training,
champion succession, a quarterly workflow review), costing **USD 22,000** a year for those five
years, a present value of **USD 73,634**. Net value **USD 143,190**, a ratio of **2.94**, and the
breakeven sustainment spend is **USD 64,782** a year, almost three times what the programme
proposed.

Two structural points follow. Sustainment is an **operating** activity with a **capital-programme**
justification, which is exactly the kind of expenditure that organisations fail to fund because the
benefit accrues to a case that is already closed; the closing account of 16.4.2 is the instrument
that makes the argument, and it must be handed to the benefits owner with the number attached. And
decay is measurable long before it is visible in the money. The leading indicator is the registered
adoption measure, monitored monthly against its own trend rather than against its target, because a
measure at 95 per cent of target and falling is a different situation from one at 95 per cent and
steady, and a report that shows only the variance against target cannot distinguish them.

### 16.A.2 Closing a project that failed, and closing one that was cancelled

Both are legitimate closures and both are done badly for the same reason: the organisation wants the
episode over, and speed is mistaken for decisiveness.

**A cancelled project** (Domain 2 KA 2.4 made the case for stopping) must still be closed properly,
and the specific obligations are: secure and value whatever is salvageable, including work in
progress that another project can use, licences that can be redeployed and, most often overlooked,
**requirements and design work that retains value independently of the delivery**; settle contracts
on a termination basis rather than abandoning them, since an unterminated contract accrues the
carrying cost of 16.2.4 indefinitely: termination rights, their notice mechanics and the
compensation they trigger are contractual and jurisdictional, so take qualified legal advice before
serving anything; account for the sunk cost honestly and separately from the decision to stop,
because conflating them is how the next cancellation gets delayed; and hold the review, which is the
highest-value review the organisation will ever run and the one it is least likely to hold, because
the findings are uncomfortable and the participants have dispersed.

**A project that delivered but failed to realise** is the harder case, because there is no moment at
which anyone is obliged to say so. Meridian is close to this shape: outputs delivered, adoption
short, operating cost unbudgeted, realised NPV negative until a post-project intervention nobody had
planned. The closing discipline is to **report the closing account to the body that approved the
case**, with the variance decomposed and the improvement plan priced, rather than to close the
project and report delivery. That is an uncomfortable paper to write and it is the one that makes
the next business case honest, which is the whole return on writing it.

### 16.A.3 The reviewer's closeout eye

Invariants to test on any transition and closeout, each cheap and each diagnostic.

The handover trigger is a **condition set**, not a date, and each condition is assessable by someone
who is not delivering it. The condition set is **split into two blocks**: no mandatory precondition
(safety case, regulatory or licence approval, privacy assessment, statutory notification, takeover
certificate) is represented as a probability, each is recorded met or not met with its approving
authority named, and **no gate-block item appears anywhere in the go/hold economics**, since a cost
attached to it is a price at which it could be waived. The readiness figure presented to the go-live
decision is the **conjunction `∏ pᵢ`** over the *discretionary* conditions, not an average, and the
paper shows both the independent and the `min(pᵢ)` bounds. A **service test** exists and was run by
the receiving organisation. Hypercare has a **measured** exit criterion. A **rehearsed reversion
plan** exists with a latest-possible-reversion point. The receiving organisation has a **funded run
line** and the whole-life cost appears in the case. Retention releases against **events**, and
outstanding items are a priced list from the day of takeover. Every open claim has a **stated
monthly carrying cost** and a settlement authority. The final payment **reconciles** to the
movements since the last certificate, and the account is signed as full and final. Documentation has
passed the **execution test** by someone who was not on the project, and the key-person
concentration is counted. The lesson **retrieval rate is instrumented**, not assumed. Every benefit
has a **registered measure with a stated denominator**, a **pre-change baseline with a date**, a
named owner outside the project, and an end date for the obligation. The closing account is
**decomposed** into level, timing, per-unit and cost components with the **interaction term and its
convention stated**. A **comparison cohort** was established at baseline. The retention schedule is
**class-by-class** with a disposal action and a recorded authority. And for every AI-informed
decision, the **model identifier, version, verifier and decision-maker** are all in the record: the
test that 28.50 % of Meridian's decision base failed.

---

## Industry variations — Domain 16

- **Healthcare.** Clinical safety governs the go-live decision and is not delegable to a project
  authority: a clinical safety case and its hazard log must be closed by clinical governance before
  transition (the leading item of 16.1.3's gate block, recorded met or not met and never as a
  probability), and a reversion plan must be viable at any point because the fallback is patient
  care, not a batch job. Benefits are overwhelmingly capacity rather than cash (Domain 2, KA 2.3.2),
  which makes the measure definition of 16.4.1 the whole argument.
- **Public sector and government.** Closure reporting is frequently required and published, benefits
  are audited by an external body against the approved case rather than against the honest one, and the
  archive's retention period is commonly set by public-records requirements rather than by the economics of
  16.4.4. The practical consequence is that the adoption-term correction of 16.4.2 must be made and
  approved *before* delivery, because after it the published claim is the benchmark.
- **Construction and infrastructure.** Commissioning, takeover, defects liability and the final account
  are contractually formal, with certificates, dated periods and a defined dispute route; the closeout
  risk is concentrated in latent-defect exposure that outlives everyone involved, so the as-built
  documentation and test evidence of 16.1.2 are the retained assets that matter most.
- **Energy and utilities.** Performance demonstration under representative load is the acceptance
  event, and an availability or efficiency shortfall accepted at commissioning becomes a permanent
  operating annuity, the arithmetic of Case study B. Regulatory witnessing constrains when a retest
  can occur, so the retest window is a schedule constraint, not a contingency.
- **Technology and product organisations.** There is often no closeout at all, because the product
  continues: the risk inverts from a botched transition to an **absent** one, with the delivery team
  becoming a permanent unfunded support function and the run cost of 16.2.1 never surfacing as a
  decision. The discipline that substitutes for closeout is a funded service-ownership transfer with a
  stated date and an operating budget.
- **Financial services and regulated industries.** Model inventory, change evidence and decision
  auditability drive the archive: the retention schedule of 16.4.4 and the model records of 16.4.5 are
  supervisory expectations rather than good practice, and an unexplainable AI-informed decision is a
  finding rather than an inconvenience.

---

## Case study — Domain 16: the account nobody asked for (health, Meridian)

**Situation.** Meridian Care Records closed twenty-six months after mobilisation with all 40 clinics
live, the systems integrator's final account agreed at **USD 1,831,400**, and a capital outturn of
**USD 2,514,000** against **USD 2,400,000** approved, a 4.75 % overspend that the programme board
accepted without discussion. The closure report ran to nine pages, recorded delivery of every
output, and was noted. No one asked what had been realised, and the governance body that had
approved the business case had, by then, been reconstituted twice.

**What the project leader did anyway.** Twelve months after the last go-live, and with no obligation
to do so, the outgoing project leader and the clinical directorate's benefits owner produced a
closing account against the registered measures. Adoption: **27 of 40** clinics meeting the
daily-use definition, **67.50 %**. Time released, by sampling in the adopting clinics: **5.4** hours
a week against the case's 6.0. Comparison cohort, the final wave of 8 clinics, measured at the same
baseline and still on the old process: **no material change**, so no attribution adjustment.
Measured annual benefit **USD 594,864**: **86.79 %** of Domain 1's honest figure and **60.75 %** of
the claim the approved case had actually made.

**The decomposition, and what it changed.** The **USD 90,576** shortfall against the honest case
split into **(24,480)** of adoption and **(66,096)** of hours per adopting clinic, with the
**2,448** interaction term assigned to the second line under the stated convention. That split
reversed the programme's own narrative: eighteen months of governance attention had gone to
adoption, because adoption was visible and had already been the subject of a public failure, while
the benefit *per adopting clinic* (the other half of the case's arithmetic) had never been
re-measured and was wrong by 10 per cent. Extending the account to present value produced the harder
number: level variance **(465,015)**, timing variance **(413,809)** from a ramp that arrived a year
late, the omitted operating cost of 108,000 a year worth **(644,900)**, the cost outturn
**(114,000)**, and a realised NPV of **(USD 304,827)** against the **+1,332,898** the honest case
had promised.

**How it resolved.** The account was sent, unrequested, to the board that had approved the case,
with one proposal attached: **USD 180,000** to complete the workflow redesign in the thirteen
non-adopting clinics and re-run the champion programme, lifting adoption to 80 % and restoring the
full 6.0 hours from year 5. Its present value was **USD 487,090** against a present cost of **USD
146,934**: an NPV of **+340,156**, a benefit-cost ratio of **3.32**, and enough to move the
programme's realised NPV to **+USD 35,329**. It was approved in one meeting, funded from operating
budget, and given to the benefits owner with a monthly adoption report against the **78.60 %**
effective level the account identified as the programme's breakeven. Two further changes followed,
both about the next programme rather than this one: every business case in the portfolio acquired a
mandatory operating-cost line, and the standard benefits register template acquired a
**benefit-per-unit** measure alongside the adoption measure.

**What the domain teaches here.** The account nobody asks for is the only one that changes anything.
Meridian's largest single bridge line: **2,114,198**: was the adoption term the case omitted, money
that never existed, and an organisation that had measured realisation against the approved claim
would have recorded a 384,336 annual failure and learned nothing about its own case-writing. The
second largest was an operating cost the project could not have controlled and should have insisted
on. And the entire value of a 2.4 million dollar programme finally rested on 180,000 dollars of
adoption work that appeared in no project plan, had no owner at the moment the project closed, and
would not have been funded had nobody computed what it was worth.

## Case study B — Domain 16: the 0.8 percentage point that was worth a million (energy, Project Auriga)

**Situation.** Project Auriga, the 25-week control-systems upgrade for a regional utility, reached
its witnessed performance demonstration two weeks late. Domain 7's week-13 position had given `CPI`
**0.9057** and an estimate at completion of `BAC/CPI =` **USD 4,416,667**; the outturn was **USD
4,412,000**, **0.11 %** below that forecast and **10.30 %** above the **USD 4,000,000** budget, a
useful reminder that a CPI-based forecast made twelve weeks from the end was accurate to within five
thousand dollars while the optimistic recovery narrative running alongside it was not.

**What happened at commissioning.** The contract required demonstrated system availability of
**98.0 %** over the witnessed test period. The system achieved **97.2 %**. The contractor offered the
contractual reduced-performance deduction of **USD 240,000** in lieu of remediation, which was within
its liability cap and was recommended for acceptance by the project team on the grounds that it closed
the contract, released the **USD 160,000** of retention held on the 3,200,000 control-systems
subcontract, and avoided further delay.

**What the operations director asked for instead.** One number: what does 0.8 of a percentage point
of availability cost to operate. The utility's own switching-restriction model priced restricted
operation at **USD 1,900** an hour; 0.8 % of 8,760 hours is **70.08** hours a year, or **USD
133,152** annually, and over the asset's 12-year life at 7 % (`AF = 7.942686`) a present value of
**USD 1,057,585**. The remediation-and-retest alternative cost **USD 74,000** plus three weeks of
deferred cutover at the utility's calibrated **USD 18,000** a week — **USD 128,000** in total.
Accepting the deduction was therefore worth `240,000 − 1,057,585 =` **(USD 817,585)**; retesting was
worth **USD 689,585** more than accepting. The breakeven deduction (the payment at which acceptance
would have been the better trade) was **USD 1,185,585**, nearly five times what was offered;
equivalently, acceptance would only have been right if the restriction cost were below **USD 661**
an hour rather than 1,900.

**How it resolved.** The contractor remediated two controller configurations and one network path
and retested, achieving **98.3 %**; the retention was released against the passed test rather than
against a deduction; the cutover ran three weeks later than the failed test date. The readiness
conjunction for the cutover itself, computed on the five conditions the utility's own gate required
(0.97, 0.93, 0.99, 0.88, 0.96), stood at **75.45 %** against an averaged dashboard reading of
**94.60 %**; and the remaining exposure was carried by a rehearsed reversion to manual switching
rather than by further assurance.

**What the domain teaches here.** A performance shortfall accepted at commissioning is not a one-off
concession, it is a permanent annuity of operating cost paid by an organisation that was not in the
room. The contractual deduction prices the **contractor's** risk, capped by the contract; the
shortfall prices the **operator's** loss, uncapped and lasting as long as the asset; and the two are
not comparable quantities, which is why the acceptance decision belongs to the receiving
organisation and not to the project that is trying to finish. The transferable discipline is to
convert every proposed acceptance concession into an annual operating cost and then into a present
value over the asset life before answering, and to state the breakeven (here USD 661 an hour), so
that the decision can be argued on its actual sensitivity rather than on the attractiveness of
closing the contract.

---

## Executive perspective — Domain 16

What a programme director cannot delegate in this domain:

- **The handover condition set, and the arithmetic of readiness.** Insist on the conjunction `∏ pᵢ`
  with both correlation bounds, never an averaged dashboard. Meridian's dashboard read 90.71 % where
  the probability of a clean go-live was 49.79 %, and no other single number in closeout is so
  routinely misreported (16.1.3).
- **The acceptance of any performance concession.** Convert it to an annual operating cost and a
  present value over the asset life, and state the breakeven, before agreeing. A deduction prices the
  supplier's capped risk, not the operator's uncapped loss (Case study B).
- **The funded run line before the project closes.** No transition without a named service owner and
  an operating budget. Meridian's omitted 108,000 a year was worth 644,900 in present value and turned
  the whole account negative (16.2.1, 16.4.2).
- **The carrying cost of every open item.** A monthly number per open claim, and a settlement
  authority. At 5,250 a month, twelve months of avoidance cost Meridian more than the claim's assessed
  value (16.2.4).
- **The closing account, reported to the body that approved the case.** Decomposed into level, timing,
  per-unit and cost, with the interaction term and its convention stated, and the improvement plan
  priced. Nobody will ask for it, and it is the only document that makes the next business case honest
  (16.4.2, 16.A.2).
- **The retention schedule and the model record.** Class-by-class retention with disposal authority,
  and the model identifier, version and verifier on every AI-informed decision. Twenty-eight and a half
  per cent of Meridian's AI-informed decision base is unexplainable for want of three fields
  (16.4.4, 16.4.5).

---

## Calculation exercises — Domain 16

**Exercise 16.1** A transition gate carries six conditions, all necessary and assessed as
independent, at 0.95, 0.88, 0.92, 0.99, 0.86 and 0.90. Compute the averaged dashboard reading and
the probability of a clean transition; identify which single condition, lifted to 0.99, gains most
and by how much; and state the per-condition probability needed for a 90 % chance of a clean
transition. *Solution.* Average `= (0.95+0.88+0.92+0.99+0.86+0.90)/6 = 5.50/6 =` **91.67 %**.
Conjunction `= 0.95 × 0.88 × 0.92 × 0.99 × 0.86 × 0.90 =` **58.93 %**: a gap of **32.73 percentage
points**. Lifting the 0.86 condition to 0.99 multiplies the conjunction by `0.99/0.86 = 1.1512`,
giving **67.84 %**, a gain of **8.91 points**: against **2.48** for the 0.95 condition, **7.37** for
0.88, **4.48** for 0.92 and **5.89** for 0.90. For 90 % clean, `p = 0.90^(1/6) =` **98.26 %** on
every condition. *Common error:* treating the gain as proportional to the gap closed (an equal
return per percentage point), which understates the value of fixing the weak conditions, since the
conjunction is multiplied by `p′/p`, so the gain per point closed is proportional to `1/p` and a
point closed at 0.86 is worth `0.99/0.86 =` **1.15** times a point closed at 0.99. Reading the
dashboard average of 91.67 % as a probability loses the ranking entirely.

**Exercise 16.2** A contract sum of USD 2,450,000 carries approved variations of USD 138,000.
Retention was 5 % of the contract sum, half released at takeover. A claim settled at USD 55,000 and
USD 26,400 of defects were rectified by others and are recoverable. Compute the retention, the
amount paid to date, the final account, the final retention release and the final payment.
*Solution.* Retention `2,450,000 × 0.05 =` **USD 122,500**. Certified works `2,450,000 + 138,000 =`
**2,588,000**; paid on certification `2,588,000 − 122,500 =` 2,465,500; plus the takeover release of
`122,500/2 =` **61,250**, so **USD 2,526,750** paid to date. Final account `2,450,000 + 138,000 +
55,000 − 26,400 =` **USD 2,616,600**. Final retention release `61,250 − 26,400 =` **USD 34,850**.
Final payment `2,616,600 − 2,526,750 =` **USD 89,850**, which reconciles as `61,250 + 55,000 −
26,400`. *Common error:* stopping at the certified works value of 2,588,000 and calling it the final
account, which omits both the settlement and the recovery and understates contract growth: here by
28,600, or 1.17 % of the original sum.

**Exercise 16.3** An open claim costs USD 2,900 a month in external advice, 1.2 days a month of
internal management at USD 750 a day, and the opportunity cost of USD 240,000 of locked cash at 6 %
a year. Determination at month 11 has assessed outcomes of 30,000 (probability 0.30), 55,000 (0.45)
and 96,000 (0.25); the client's own assessment is 55,000. Settlement is available at month 1.
Compute the monthly carrying cost, the expected cost of determination, the breakeven settlement
price and the breakeven premium over the client's assessment. *Solution.* Carrying cost `= 2,900 +
1.2 × 750 + 240,000 × 0.06/12 = 2,900 + 900 + 1,200 =` **USD 5,000 a month**. `EMV = 0.30 × 30,000 +
0.45 × 55,000 + 0.25 × 96,000 = 9,000 + 24,750 + 24,000 =` **USD 57,750**. Expected cost of
determination `57,750 + 11 × 5,000 =` **USD 112,750**. Breakeven settlement price at month 1
`112,750 − 5,000 =` **USD 107,750**. Breakeven premium `107,750 − 55,000 =` **USD 52,750**, which
the identity `10 × 5,000 + (57,750 − 55,000)` reproduces. *Common error:* omitting the internal
management time (900 of the 5,000, or 18 %) on the grounds that the staff are paid anyway, which is
the reasoning that makes the carrying cost invisible and, at ten months, understates the breakeven
premium by 9,000.

**Exercise 16.4** A programme's case assumed 60 sites at 75 % adoption releasing 4.0 hours a week at
USD 92 an hour over 46 weeks. Measured: 66 % adoption and 3.6 hours. Compute the planned and
measured annual benefits and the total variance, then decompose it both ways and state the
interaction term. *Solution.* `U = 60 × 92 × 46 =` **USD 253,920** per unit of adoption-hours.
Planned `253,920 × 0.75 × 4.0 =` **USD 761,760**; measured `253,920 × 0.66 × 3.6 =` **USD
603,313.92**: **79.20 %** of plan, a variance of **(USD 158,446.08)**. Adoption first: adoption
`253,920 × 4.0 × (−0.09) =` **(91,411.20)**; hours at measured adoption `253,920 × 0.66 × (−0.4) =`
**(67,034.88)**. Hours first: hours `253,920 × 0.75 × (−0.4) =` **(76,176.00)**; adoption at
measured hours `253,920 × 3.6 × (−0.09) =` **(82,270.08)**. Both sum to (158,446.08); the
interaction term `253,920 × (−0.09) × (−0.4) =` **USD 9,141.12** is what moves between the lines.
*Common error:* computing both variances at plan — `(91,411.20)` and `(76,176.00)` — which sums to
(167,587.20) and over-states the shortfall by exactly the interaction term, because the same
movement has been counted twice.

**Exercise 16.5** A post-project review costs USD 62,000 and produces 41 lessons; an applied lesson
avoids USD 19,500 on average; the current retrieval rate is 9 %. Compute the expected recovery and
the breakeven retrieval rate, then evaluate spending a further USD 24,000 to raise retrieval to 30
%. *Solution.* Addressable value `41 × 19,500 =` **USD 799,500**. At 9 %: `799,500 × 0.09 =` **USD
71,955**, the review already returns 1.16 times its cost. Breakeven retrieval `62,000/799,500 =`
**7.75 %**. At 30 %: **USD 239,850**, an increment of `799,500 × 0.21 =` **USD 167,895** for 24,000,
a ratio of **7.00**. Combined breakeven `86,000/799,500 =` **10.76 %**, and the net value at 30 %
retrieval is **USD 153,850**. *Common error:* comparing the review's cost with the *addressable*
value of 799,500 rather than with the value weighted by retrieval, which makes every review look
overwhelmingly worthwhile and removes the reason to fix retrieval: the one intervention that
actually pays here at seven to one.

---

## Practitioner's toolkit — Domain 16

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 16.T.1 — Transition readiness certificate

One page per receiving unit, signed by the receiving organisation, in **two blocks that never share a
column**.

**Block A, mandatory preconditions (no probability column).** One row per item: the precondition,
the **external or independent authority** that grants it, the status recorded as **met / not met**
only, the date it was granted, and the reference to the granting instrument. The standing rows are
the safety case and its hazard log closed by the accountable safety authority, regulatory or licence
approval, the data-protection or privacy assessment, required notifications, and contractual
takeover certificates, with any further precondition the sector imposes. Beneath Block A, a
**signature line for the approving authority itself**, by name and role, distinct from and above the
project's and the receiving organisation's signatures: the certificate is not complete on the
project's signature alone. A single "not met" in Block A ends the certificate. There is no
weighting, no probability and no offsetting entry, and the certificate is issued as a **hold**.

**Block B, discretionary conditions (the arithmetic).** Rows: each readiness condition, its owner
(who must not be the person delivering it), its objective assessment method, its assessed
probability `pᵢ` with the basis of the assessment named, and its evidence reference. Footer,
computed and printed rather than described: the **conjunction `∏ pᵢ`**, the averaged figure
alongside it so the gap is visible, the `min(pᵢ)` correlated bound, the expected number of failed
transitions across the remaining units at the stated cost per failure, and the residual after any
proposed uplift. Below that: the hypercare duration with its **measured** exit criterion, the
reversion trigger, the reversion decision-maker, the time to revert and the latest point at which
reversion remains possible. A certificate that cannot be completed is itself the finding.

### Toolkit 16.T.2 — Closeout and final account pack

Four sections, each a table. **Commercial:** per contract, original sum, approved variations, claims
open with their **monthly carrying cost** and settlement authority, claims settled, recoveries,
retention held and released with the event that released it, final account, amounts paid, final
payment with its reconciliation to the movements since the last certificate, and the date the full
and final statement was signed. **Operational:** service owner, funded run line with its annual
amount, support model, licence and contract register with renewal and notice dates, access
revocation date for the project team, and the obsolescence horizon. **Knowledge:** documentation
items with the date each passed the execution test and by whom, key-person concentration count, and
the outstanding documentation list with owners and dates. **Organisational:** authority withdrawn
on, cost accounts closed on, records placed under the retention schedule, and people released with
their contribution recorded.

### Toolkit 16.T.3 — Benefits closing account and retention schedule

Two linked artefacts, owned by the benefits owner and reported to the body that approved the case.
The **closing account**: per benefit, registered measure with its exact definition and denominator,
baseline value and date, target profile, measured value and date, and the variance decomposed into
adoption (or volume), benefit per unit, and timing, with the interaction term shown and the
decomposition convention stated in a footnote. Then the present-value bridge from the approved case
to the realised position, with an explicit line for any operating cost the case omitted, and the
priced improvement plan with its owner and funding source. The **retention schedule**: per data
class (volume, retention period, legal or contractual basis, disposal or de-identification action,
the authority who may execute it, and the test that de-identification actually worked; plus, for
every AI-informed decision, the required fields of 16.4.5) model identifier, version, date, input,
output, named verifier, decision-maker, with a monthly count of records missing any of them.

---

## Exam preparation — Domain 16

**What is assessed.** What handover transfers and what it cannot; condition-based handover; the
distinction between works tests and service tests; **the split between the gate block of mandatory
preconditions and the discretionary conditions**; **readiness as a conjunction and its arithmetic**;
proportional-shortfall ranking of remediation; the hold-or-go decision under both correlation
assumptions; hypercare exit criteria and the reversion plan; operational transition with a funded run
line and whole-life cost; retention, defects liability and warranty; **the final account and its
reconciliation**; **the carrying cost of an open item and the breakeven settlement premium**; knowledge
transfer and the runbook execution test; the post-project review and **the economics of retrieval**;
the benefits measurement plan and the registered measure; **the closing account and its
decomposition**, including the interaction term and the convention that assigns it; attribution and
the comparison cohort; class-by-class retention economics; and the model-retention record.

**The calculations to be able to do under time pressure.** `∏ pᵢ` against `(Σ pᵢ)/k`, the gain
`p′/p` from lifting one condition, and `p = target^(1/k)`. Expected failed transitions and the
hold-or-go comparison. Final account, retention release and final payment with its reconciliation.
Monthly carrying cost, `EMV` of determination, breakeven settlement price and the identity
`c × Δm + (EMV − assessed)`. Annual benefit `U × a × h`, the two-factor decomposition in both orders,
and the interaction term. Present value of an operating cost over an appraisal horizon. Breakeven
retrieval rate. Retention cost against `P(need) × consequence`, and the breakeven need probability.

**The traps.** Entering a mandatory precondition (a safety case, a licence approval, a privacy
assessment) as a probability in the readiness product, or pricing it in the hold-or-go comparison,
which states a figure at which it could be waived (16.1.3, 16.1.4) · reporting an averaged readiness
figure as a probability (16.1.3, Exercise 16.1) · ranking readiness remediation by absolute gap
rather than by the ratio `p′/p` (Exercise 16.1) · omitting the residual failure rate after an
uplift, which makes readiness work look better than it is (MCQ 16.1-D) · treating `min(pᵢ)` as the
expected case rather than the correlated bound (16.1.3) · calling the certified works value the
final account (Exercise 16.2) · ignoring internal management time in a carrying cost (Exercise 16.3)
· comparing an early-settlement premium with the claim's assessed value rather than with the
expected cost of determination (16.2.4) · computing both benefit variances at plan and
double-counting the interaction term (Exercise 16.4) · using the undiscounted total for an
operating-cost omission (MCQ 16.2-D) · comparing a review's cost with the addressable rather than
the retrieval-weighted value of its lessons (Exercise 16.5) · measuring realisation against a claim
the case should never have made rather than against the honest figure (16.4.2) · and arguing
attribution at review instead of designing a comparison cohort at baseline (16.4.3).

**How the domain connects.** Domain 1 supplies the accountability principle, the outputs-to-benefits
chain and the cost of delay that this domain re-tests at the realised rate. Domain 2 supplies the
benefits map, the baseline discipline, the appraisal this account is measured against and the kill
criteria that 16.A.2 executes. Domain 3 supplies the gate at which the go-live decision sits and the
decision record the post-project review reads. Domain 4 supplies the configuration baseline that
as-built documentation must match. Domain 9 supplies the acceptance and nonconformance machinery
commissioning uses. Domain 10 supplies the contract mechanisms of retention, defects liability and
claim settlement. Domain 12 supplies the conditions under which a review produces candour and a team
dissolves well. Domain 14 supplies the AI-use register and verification standard that 16.4.5
retains. Domain 15 supplies the portfolio benefits register that must not double-count what this
domain measures. And PFL-AI Domain 4 supplies the appraisal machinery (equivalent annual value and
unequal lives) behind the whole-life comparison of 16.2.1 and Case study B.

---

## Domain 16 summary
The project ends; the account does not. This domain replaces the handover *date* with a handover
*condition*, and splits that condition set in two. Mandatory preconditions (the safety case and its
hazard log, regulatory and licence approvals, the privacy assessment, required notifications,
takeover certificates) are recorded met or not met with their approving authority named, carry no
probability and are never traded against the cost of delay; while one is open the only decision is
hold. Above that gate block, readiness is a **conjunction**: Meridian's seven discretionary go-live
conditions, averaging **90.71 %** on the programme dashboard, gave a probability of a clean go-live
of **49.79 %**, a gap of nearly forty-one percentage points between the number that was reported and
the number that mattered. The remediation ranking follows the ratio `p′/p`, so the clinical champion
at 0.80 was worth **11.20 points** against **1.04** for training at 0.96; even all seven conditions
at 0.98 leaves **13.19 %** of go-lives failing, which is why the answer to the residual is a
rehearsed reversion plan and not more assurance. Holding three weeks and spending **USD 96,000**
dominated going now under both the independent conjunction and the perfectly correlated bound,
saving **USD 261,864** and **USD 55,992** respectively against a commitment of **USD 138,840**.

Closeout is commercial as well as operational. Meridian's final account came to **USD 1,831,400**
(**9.01 %** above the original sum, being 5.71 % of variations plus 4.40 % of claim settlement less
1.11 % of defect recovery), and the final payment of **USD 97,400** reconciled exactly to the
movements since the last certificate, which is the check that an account is closed rather than
totalled. The open claim cost **USD 5,250 a month** to carry, so fourteen months of it would have
cost **USD 73,500**, more than the claim's own assessed value of 62,000; the breakeven
early-settlement premium was **USD 69,500**, given by the transferable identity **carrying cost ×
months saved + (expected determination − own assessment)**, and settling at 74,000 saved **USD
57,500**. Knowledge behaves the same way: three undocumented configurations cost **7.00** times as
much to reconstruct as to record, and a post-project review that breaks even at a **5.20 %**
retrieval rate returns **11.30 to one** on the indexing that raises retrieval, which is where the
value is lost, and where nobody spends.

The closing account settles what Domains 1 and 2 opened. Measured: **27 of 40** clinics in daily use
and **5.4** clinician-hours released, worth **USD 594,864** a year — **86.79 %** of Domain 1's
honest **685,440** and exactly **60.75 %** of the **979,200** the approved case claimed. The **USD
90,576** shortfall against the honest case decomposes into **(24,480)** of adoption and **(66,096)**
of benefit per adopting clinic, with **USD 2,448** of interaction assigned by a stated convention.
And and the larger of the two was the assumption nobody was monitoring. In present value the bridge
runs **+3,447,096** as claimed, **(2,114,198)** for the adoption term the case omitted,
**(465,015)** of level variance, **(413,809)** of timing, **(644,900)** for an operating cost of
108,000 a year that the case never carried, and **(114,000)** of cost outturn, to a realised NPV of
**(USD 304,827)**. Strip out the run cost and the realised position is **+340,073**: the omission of
an operating-cost line was the entire margin. And **USD 180,000** of post-project adoption work,
worth **+USD 340,156** at a benefit-cost ratio of **3.32**, moved the account to **+USD 35,329**:
the whole value of a 2.4 million dollar programme, resting on a plan no project document contained.
Auriga's closeout makes the same point in a different currency: 0.8 of a percentage point of
availability, offered for a **240,000** deduction, was worth **USD 1,057,585** over the asset life,
and the retest that cost **128,000** was worth **689,585** more than accepting the money.

What survives the project is what was written down. Retaining the evidence pays as insurance above a
**3.64 %** need probability; retaining personal data is the reverse trade, with de-identification
worth **USD 17,940** net on 1.1 TB; and **28.50 %** of Meridian's AI-informed decision base is
permanently unexplainable because three fields (model identifier, version, date) were not recorded
on records that were being written anyway. Benefits decay at **4 %** a year without sustainment,
worth **USD 216,824** in present value against a **USD 73,634** sustainment programme, so the
account has to be handed on rather than closed.

The through-line, and the last of the book: **outputs are delivered, benefits are realised, and the
two are separated by an organisation, a period of time and a measurement nobody is obliged to
take.** Take it anyway. Decompose it honestly, state the convention, price the recovery, and send it
to the people who approved the promise; because the closing account is the only document in the
delivery lifecycle that improves the next decision rather than defending the last one.
