# Domain 8 — Risk, Uncertainty and Resilience *(quantitative)*

> **Group:** Delivering the work (Part Two). **Target:** ~74 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain is the home of `EMV` and supplies the quantification
> behind the contingency that Domain 7 (KA 7.1.3) reserves and the schedule ranges Domain 6
> (KA 6.4.3) forecasts with. British English; USD (+SAR where useful, indicative
> `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Domains 6 and 7 built plans and budgets; both left an obligation outstanding. Domain 6 forecast
with ranges but did not say where the ranges come from. Domain 7 put a contingency reserve inside
the baseline and said it is "sized against identified risks" without showing how. This domain
closes both gaps and adds the part neither covers: what a leader does when the unexpected happens
anyway. It builds risk identification honestly, including opportunities (KA 8.1); works the
analysis from qualitative screening through `EMV`, decision trees and aggregated contingency
(KA 8.2); sizes and governs reserves (KA 8.3); and turns to resilience, crisis leadership,
cognitive bias and AI-enabled risk sensing (KA 8.4). The discipline throughout is that **risk
management is a decision-support activity, not a documentation activity** — a register that changes
no decision has cost money and bought nothing.

**Learning objectives.** After this domain a candidate can: distinguish risk from uncertainty and
issue; write a risk statement that supports a decision; identify opportunities as rigorously as
threats; run qualitative screening and state its limits; compute `EMV` for individual risks and
interpret the total; build and solve a decision tree, including the value of information; aggregate
independent risks to a mean and a confidence level rather than summing point estimates; size a
contingency reserve defensibly and distinguish it from management reserve; select and cost
responses; explain resilience as distinct from prediction; recognise the biases that corrupt
estimates and reviews; lead in a crisis; and govern AI-produced risk analysis.

**The master worked project.** Project Auriga continues from Domains 6 and 7 — the 25-week,
**`BAC` USD 4,000,000** control-systems upgrade. Its risk register, used throughout KA 8.2–8.3,
carries the ground-conditions risk that actually materialised in Domain 6's case study, so the
reader sees the same event before and after the fact.

---

## Knowledge Area 8.1 — Threats, opportunities and identification

*Topics: 8.1.1 risk, uncertainty and issues · 8.1.2 writing a usable risk statement ·
8.1.3 identification methods and their blind spots.*

### 8.1.1 Risk, uncertainty and issues

**Definitions.** A **risk** is an uncertain event that, if it occurs, affects an objective —
threats reduce achievement, **opportunities** improve it. **Uncertainty** in the broader sense
covers what cannot be enumerated with probabilities at all — the conditions where scenario
thinking and resilience substitute for calculation. An **issue** is a risk that has occurred; it is
managed, not analysed. Keeping these apart matters practically: registers clogged with issues stop
being forward-looking, and treating deep uncertainty as if it were quantifiable produces false
precision (KA 8.4.1).

**Risk appetite and thresholds** convert judgment into something operable: how much exposure the
organisation accepts, expressed as thresholds that trigger escalation or response — connecting this
domain to Domain 3's decision rights. Appetite that is never expressed numerically cannot be
applied consistently by different people, which is the usual reason two projects in one portfolio
treat the same exposure differently.

### 8.1.2 Writing a usable risk statement

Most registers fail at the sentence level. A usable statement has three parts —
**cause → event → consequence** — and names the affected objective:

> *Because* the controller is single-sourced with a volatile lead time (**cause**), *it may be*
> that delivery slips beyond the installation window (**event**), *resulting in* a delay to
> commissioning and additional preservation cost (**consequence**).

Compare "supplier risk", which supports no decision: it identifies no cause to attack, no event to
monitor and no consequence to size. The three-part form is not a formatting preference — each part
maps to a different response type (attack the cause, monitor the event, mitigate the consequence),
which is why KA 8.3's response selection depends on it.

### 8.1.3 Identification methods and their blind spots

Methods: structured workshops, checklists and prompt lists from prior projects, assumption
analysis (every assumption is a risk in disguise — Domain 2's assumption register), interviews for
what people will not say in a group, and lessons from comparable work (Domain 9's lessons-learned).
Each has a blind spot, and knowing them is the professional part:

- **Workshops** surface what the group already shares and suppress what a junior member suspects —
  so run them with an explicit invitation to dissent and a route for private input.
- **Checklists** find known risks and, by their comfort, discourage looking for novel ones.
- **Assumption analysis** is the highest-yield method and the least used, because it requires
  admitting how much of the plan is assumed.
- **Historical data** encodes the past's risks, not this project's novelties.

**Opportunities are systematically under-identified** — most registers are 90 % threats — because
the process is framed defensively and because nobody is rewarded for one. A leader who wants them
must ask for them separately and size them the same way (Auriga's `R5` below is one).

### AI in this KA

Generating candidate risk lists is a real AI strength: broad, fast, and free of the group's shared
blind spot — genuinely useful against the workshop failure mode above. It is equally a machine for
**plausible generic risk**, producing items that fit any project and support no decision. The
governed workflow: use AI to *widen* identification (especially by asking it to challenge the plan's
assumptions), then require every surviving item to pass the three-part statement test of 8.1.2 with
a named owner. **AI proposes; the professional verifies, decides and remains accountable** — and
here the verification is specifically that the risk is *this* project's, not a template's.

### Key terms — KA 8.1

| Term | Meaning |
|---|---|
| **Risk / opportunity** | Uncertain event affecting an objective, adversely / favourably. |
| **Uncertainty** | Conditions that cannot be meaningfully enumerated with probabilities. |
| **Issue** | A risk that has occurred; managed, not analysed. |
| **Risk appetite / threshold** | Accepted exposure, expressed operably enough to apply consistently. |
| **Cause → event → consequence** | The three-part risk statement; each part maps to a response type. |

### Sample MCQs — KA 8.1

**MCQ 8.1-A `[8.1.2 · Analysis]`** Which register entry best supports a decision?
- A. "Supplier risk — high"
- B. "Because the controller is single-sourced with volatile lead times, delivery may slip beyond the installation window, delaying commissioning and adding preservation cost" ✅
- C. "Delay to commissioning — probability 35 %"
- D. "Supplier may cause problems — owner: procurement"

*Rationale:* Only B names a cause to attack, an event to monitor and a consequence to size (8.1.2).
C states a consequence with a probability but no cause, so no response can be designed; A and D
support nothing at all.

**MCQ 8.1-B `[8.1.1 · Recall]`** The ground contamination has been discovered and remediation is
under way. In the register this is:
- A. a risk with probability 1.0
- B. an issue — it has occurred, and is managed rather than analysed ✅
- C. an opportunity, since remediation was funded
- D. removed entirely, with no further record

*Rationale:* Occurred risks become issues (8.1.1). A is the common fudge that clogs registers; D
loses the audit trail and the lesson; C is nonsense.

**MCQ 8.1-C `[8.1.3 · Analysis]`** A register of 60 items contains 3 opportunities. The soundest
inference is:
- A. this project genuinely has few opportunities
- B. the identification process is framed defensively; opportunities must be asked for separately and sized the same way ✅
- C. opportunities do not belong in a risk register
- D. the register is too long and should be cut

*Rationale:* A 95 % threat ratio reflects process framing rather than reality (8.1.3). C contradicts
the definition of risk; D addresses a different problem.

### Self-check — KA 8.1

1. *State the three parts of a usable risk statement.* — Cause, event, consequence, against a
   named objective.
2. *Why is assumption analysis the highest-yield identification method?* — Every assumption is a
   risk in disguise, and plans rest on more of them than teams admit.
3. *What is the register consequence of treating an occurred risk as a 100 % risk?* — It clogs the
   forward-looking register; occurred risks are issues.

---

## Knowledge Area 8.2 — Analysis: from screening to quantification

*Topics: 8.2.1 qualitative screening and its limits · 8.2.2 expected monetary value ·
8.2.3 decision trees and the value of information · 8.2.4 aggregating to a contingency.*

### 8.2.1 Qualitative screening and its limits

Probability-and-impact scoring on a matrix is a **screening** tool: cheap, fast, and adequate for
ranking a long list into an attention order. Its limits are structural and must be stated wherever
it is used. Ordinal scales cannot be arithmetic — a "4" is not twice a "2", so scores must not be
multiplied and summed as if they were money. Band boundaries create false distinctions (a risk at
the top of "medium" outranks one at the bottom of "high" in reality but not on the matrix). And the
matrix is blind to **correlation**: five risks driven by one supplier are one risk wearing five
costumes.

The professional practice: screen qualitatively, then **quantify what matters** — the items that
could breach a threshold, plus anything a decision now depends on.

### 8.2.2 Expected monetary value

```
EMV = probability × impact          (per risk; threats positive, opportunities negative cost)
```

**Worked example 8.2.2 — Auriga's risk register, quantified.**

1. **Setup.** Four threats and one opportunity, each with an assessed probability and cost impact
   (`BAC` USD 4,000,000).
2. **Formula.** `EMV` = `p × impact`; total exposure = Σ `EMV`, opportunities carried negative.
3. **Substitution and result.**

   | ID | Risk | `p` | Impact (USD) | `EMV` (USD) |
   |---|---|---|---|---|
   | R1 | Controller lead-time slip | 0.35 | 240,000 | 84,000 |
   | R2 | Ground conditions worse than surveyed | 0.50 | 180,000 | 90,000 |
   | R3 | Integration rework | 0.25 | 320,000 | 80,000 |
   | R4 | Permit delay | 0.15 | 400,000 | 60,000 |
   | R5 | Early-delivery rebate *(opportunity)* | 0.30 | (120,000) | **(36,000)** |
   | | **Total expected exposure** | | | **278,000** |

4. **Interpretation.** Total `EMV` is **USD 278,000** — **6.95 %** of `BAC`, and USD 314,000 if the
   opportunity is ignored, which is why opportunities must be in the same arithmetic rather than
   mentioned in prose. Two cautions the table itself teaches. **`EMV` is an average of outcomes
   that will not happen**: no single risk will cost 84,000 — R1 costs 240,000 or nothing.
   So `EMV` is the right basis for *funding a portfolio* of risks and the wrong basis for deciding
   whether one specific risk is survivable. And **ranking by `EMV` reorders the register**: R4 has
   the largest impact (400,000) but the smallest threat `EMV` (60,000), while R3's lower impact
   carries more expected cost. A leader managing by impact alone would attend to them in the wrong
   order — though see 8.3.1, because impact still governs *survivability*.

### 8.2.3 Decision trees and the value of information

A decision tree makes a choice under uncertainty explicit: decision nodes (what we control), chance
nodes (what we do not), and outcomes valued and rolled back to the present decision.

**Worked example 8.2.3 — should Auriga survey before excavating?**

1. **Setup.** Excavation may hit contamination: probability **0.40**, cost if hit
   **USD 300,000**. Option A: proceed directly. Option B: commission a **USD 25,000** ground
   survey first, which reliably reveals the condition; if contamination is present, a planned
   mitigation costs **USD 90,000** instead of the reactive 300,000.
2. **Formula.** Roll back each branch: EV(cost) = Σ probability × outcome cost. Value of
   information = EV(cost without) − EV(cost with).
3. **Substitution.** A: `0.40 × 300,000`. B: `25,000 + 0.40 × 90,000 = 25,000 + 36,000`.
4. **Result.** A **USD 120,000** expected cost; B **USD 61,000**. **Value of the survey =
   USD 59,000**, against its USD 25,000 price.
5. **Interpretation.** The survey is worth buying, and the arithmetic shows *why*: it converts a
   reactive 300,000 into a planned 90,000 in the 40 % of futures where the problem exists. This is
   the general shape of **buying information** — worth it when it changes what you would do, worth
   nothing when it does not, however reassuring it feels. Note the sensitivity: if mitigation once
   forewarned still cost 250,000, B's EV would be `25,000 + 100,000 = 125,000` and the survey would
   destroy value. **Information is valuable only in proportion to the better action it enables.**
   (Domain 6's case study is the counterfactual: Auriga did not survey, met the condition
   reactively, and paid the recovery this tree prices.)

> **Fig 8.2.1 — The survey decision tree.** Standard decision-tree diagram. A square decision node
> branches to "Proceed directly" and "Survey first (USD 25,000)". Each branch reaches a circular
> chance node splitting 0.40 / 0.60. Upper branch outcomes: 300,000 and 0 → EV **120,000**. Lower
> branch outcomes: 90,000 + 25,000 and 0 + 25,000 → EV **61,000**. The rejected branch struck
> through; the value of information (59,000) annotated between them in crimson. Source: PCI
> original. Alt text: a decision tree comparing proceeding directly at an expected cost of
> 120,000 with surveying first at 61,000, the difference labelled as the value of information.

### 8.2.4 Aggregating to a contingency

**The error to avoid.** Summing worst cases produces a number nobody will fund
(240,000 + 180,000 + 320,000 + 400,000 = 1,140,000, or 28.5 % of `BAC`); summing `EMV`s alone
(278,000) funds the *average*, which by construction is exceeded about half the time. Neither is a
contingency. What a leader needs is a **confidence level**: an amount that covers the aggregate
outcome with stated probability.

**Worked example 8.2.4 — Auriga's contingency at P80.**

1. **Setup.** The five register risks of 8.2.2, treated as independent (an assumption examined
   below).
2. **Formula.** For independent risks: mean = Σ `p × impact`; variance = Σ `p(1−p) × impact²`;
   standard deviation = √variance. A P80 amount ≈ mean + 0.8416 × σ (the 80th percentile of a
   normal approximation).
3. **Substitution.** Mean **278,000** (from 8.2.2). Variance
   `0.35×0.65×240,000² + 0.50×0.50×180,000² + 0.25×0.75×320,000² + 0.15×0.85×400,000² +
   0.30×0.70×120,000²` = **63,828,000,000**; σ = **252,642**. P80 =
   `278,000 + 0.8416 × 252,642`.
4. **Result.** σ **USD 252,642**; **P80 ≈ USD 490,624**. Against the worst-case sum of 1,140,000
   and the `EMV` sum of 278,000 — and against the "10 % of `BAC`" rule of thumb, which would give
   400,000 with no reasoning attached.
5. **Interpretation.** The P80 figure is defensible in a way the other three are not: it states
   what confidence it buys. Three professional caveats belong beside it. **Independence is an
   assumption, usually optimistic** — if R1 and R3 share a supplier they are correlated, the
   variance is larger, and the true P80 is higher; correlation is what turns a bad month into a
   crisis. **The normal approximation is a convenience** for a handful of Bernoulli risks; proper
   practice runs a Monte Carlo simulation over the register (and over the schedule, extending
   Domain 6's three-point durations) to produce a distribution rather than a formula. And **the
   confidence level is a policy choice**, not a technical one — P80 is common for contingency, P50
   for a target, and whoever chooses is making a risk-appetite decision (8.1.1) that belongs to
   governance.

### AI in this KA

Simulation, correlation analysis and scenario generation are legitimate machine work, and the
outputs are unusually seductive because a distribution *looks* like evidence. Three checks before
any simulated number reaches a board. **Interrogate the inputs**: a simulation is an amplifier of
its assumed distributions and correlations, and those are judgments — garbage in, precisely-shaped
garbage out. **Recompute a landmark by hand**: the mean should match Σ`EMV`; if it does not, the
model is not doing what you think. **Ask what would change the decision**: a P80 quoted to the
dollar with no sensitivity is decoration. The register's inputs remain human judgments with named
owners, whatever produced the output.

### Key terms — KA 8.2

| Term | Meaning |
|---|---|
| **`EMV`** | probability × impact; the average of outcomes that will not individually occur. |
| **Decision tree** | Explicit rollback of decisions and chance events to a present choice. |
| **Value of information** | The reduction in expected cost that knowing enables; zero if it changes no action. |
| **P50 / P80** | Confidence levels — amounts covering the aggregate outcome with stated probability. |
| **Correlation** | Shared drivers that raise aggregate variance; independence is usually optimistic. |
| **Monte Carlo simulation** | Repeated sampling to produce an outcome distribution rather than a point. |

### Sample MCQs — KA 8.2

**MCQ 8.2-A `[8.2.2 · Application]`** A risk has probability 0.15 and impact USD 400,000. Its
`EMV` is:
- A. USD 400,000
- B. USD 60,000 ✅
- C. USD 340,000
- D. USD 26,667

*Rationale:* `0.15 × 400,000 = 60,000`. A is the impact; C is impact less `EMV`; D divides instead
of multiplying.

**MCQ 8.2-B `[8.2.2 · Analysis]`** R4 has the register's largest impact (400,000) but its smallest
threat `EMV` (60,000). The correct managerial reading is:
- A. R4 should be ignored, having the lowest `EMV`
- B. `EMV` sets the funding priority, while impact still governs whether the event is survivable — both readings are needed ✅
- C. impact alone should drive priority
- D. the assessment must be wrong, since impact and `EMV` disagree

*Rationale:* `EMV` is the right basis for funding a portfolio of risks; a single large impact may
still be existential regardless of probability (8.2.2, 8.3.1). A and C each discard half the
information; D misunderstands that the two measure different things.

**MCQ 8.2-C `[8.2.3 · Application]`** Proceeding directly has an expected cost of
`0.40 × 300,000`. A USD 25,000 survey reduces the mitigated cost to USD 90,000. The value of the
information is:
- A. USD 25,000
- B. USD 59,000 ✅
- C. USD 84,000
- D. nil — the survey costs more than it saves

*Rationale:* `120,000 − (25,000 + 36,000) = 59,000`. A is the survey's price, not its value;
C omits the survey cost from branch B; D reverses the conclusion.

**MCQ 8.2-D `[8.2.4 · Analysis]`** A register's `EMV` sum is 278,000 and its worst-case sum is
1,140,000. Setting contingency at 278,000 means:
- A. an appropriately funded reserve
- B. funding the average outcome, which by construction is exceeded roughly half the time ✅
- C. a conservative reserve, since not all risks will occur
- D. the same as a P80 reserve

*Rationale:* The mean is the ~50th percentile of the aggregate (8.2.4), so it is exceeded about
half the time — the reason a confidence level is chosen explicitly. C mistakes the mean for
conservatism; D confuses two different statistics (490,624 here).

**MCQ 8.2-E `[8.2.1 · Analysis]`** Why must ordinal probability-impact scores not be multiplied and
summed as money?
- A. the matrix is only for threats
- B. ordinal bands are ranks, not quantities — a "4" is not twice a "2", so the arithmetic is meaningless ✅
- C. multiplication requires more than five bands
- D. scores may be summed provided they are weighted

*Rationale:* Ordinal scales support ordering, not arithmetic (8.2.1). Weighting (D) does not repair
a scale that never carried magnitude.

### Self-check — KA 8.2

1. *Why is `EMV` wrong for judging whether one risk is survivable?* — It averages outcomes that
   will not occur; the actual event is impact-or-nothing.
2. *When is information worth nothing however reassuring?* — When it would not change the action
   taken.
3. *Which assumption in the P80 calculation is usually optimistic, and why does it matter?* —
   Independence; correlated risks raise variance and the true confidence amount.

---

## Knowledge Area 8.3 — Responses, reserves and governance

*Topics: 8.3.1 selecting and costing responses · 8.3.2 reserves and their authority ·
8.3.3 monitoring and the register that earns its keep.*

### 8.3.1 Selecting and costing responses

The response families — **avoid** (remove the cause or the exposure), **reduce** (lower probability
or impact), **transfer** (insurance, contract terms — a *price*, not a disappearance; Domain 7,
KA 7.4.2), **accept** (with or without a fallback), and for opportunities **exploit, enhance,
share, ignore**. Two rules make selection professional rather than reflexive.

**A response is an investment.** It costs money and time and reduces `EMV`; if the reduction is
smaller than the cost, the response destroys value and acceptance is the correct answer. Auriga's
fast-track decision from Domain 6 is exactly this arithmetic: one week of client bonus
(USD 45,000) against an accepted rework risk of `0.20 × 60,000 = 12,000` — a net **+USD 33,000**,
which is why the recovery plan took it.

**Impact governs survivability even at low probability.** A 3 % chance of an event the project
cannot survive is not managed by its `EMV`; it is avoided, transferred, or the project's viability
is reconsidered. `EMV` funds portfolios; existential risks are handled by structure, and confusing
the two is how organisations optimise their way into single points of failure.

**Secondary risks.** Every response creates its own — fast-tracking creates rework risk, a transfer
creates counterparty risk, mitigation creates delivery risk on the mitigation itself. A register
whose responses have no secondary entries has not been thought through.

### 8.3.2 Reserves and their authority

Domain 7 (KA 7.1.3) established the structure; this domain supplies the sizing and the governance:

| Reserve | Covers | Sized by | Spent by |
|---|---|---|---|
| **Contingency** | Identified risks in the register | Aggregation to a stated confidence (8.2.4) | Project manager, under a published protocol |
| **Management reserve** | Unknown-unknowns and scope change | Judgment and organisational policy | Sponsor / change authority, via change control |

Three governance rules. **The draw protocol is published in advance** — which risk, what evidence,
what approval — because a reserve released ad hoc is indistinguishable from an overrun.
**Consumption is trended against risk retirement**, not against time: burning 60 % of contingency
while 20 % of the register has been retired is the signal MCQ 7.1-B describes. And **contingency
released by retired risks is returned, not reallocated** to convenient overspends; otherwise the
reserve silently becomes a slush fund and the next real risk is unfunded.

### 8.3.3 Monitoring and the register that earns its keep

A live register has movement: probabilities revised as evidence arrives, risks retired when their
window closes, new entries as the project learns, owners who report rather than merely appear.
The tests of whether it is working are behavioural, not documentary — **has a decision changed
because of it this month?**, **are the top items the ones the team actually worries about?**, and
**did anything that hurt us appear in it beforehand?** A register that answers no to all three is
theatre, and the honest response is to fix the process rather than reformat the document.
**Early-warning indicators** are the register's operational edge: leading measures tied to specific
risks (supplier confirmations slipping, defect discovery rate rising, permit queue lengthening)
that fire before the event, which is what makes a response affordable (Domain 6's lead-time point).

### Key terms — KA 8.3

| Term | Meaning |
|---|---|
| **Avoid / reduce / transfer / accept** | The threat-response families; transfer is a price, not a disappearance. |
| **Secondary risk** | Risk created by a response; a register without them is incomplete. |
| **Draw protocol** | The published rules for releasing contingency. |
| **Risk retirement** | Closing a risk whose window has passed; frees contingency for return. |
| **Early-warning indicator** | A leading measure tied to a risk, firing before the event. |

### Sample MCQs — KA 8.3

**MCQ 8.3-A `[8.3.1 · Application]`** A mitigation costs USD 50,000 and reduces a risk's `EMV`
from 84,000 to 20,000. The decision and its basis are:
- A. reject — 50,000 is a large outlay
- B. accept — the `EMV` reduction of 64,000 exceeds the 50,000 cost ✅
- C. accept — any reduction in `EMV` justifies a response
- D. indifferent — cost and benefit are equal

*Rationale:* Responses are investments: `84,000 − 20,000 = 64,000` of reduction for 50,000 of cost
is value-creating. A prices the outlay without the benefit; C would justify unlimited spend;
D miscomputes.

**MCQ 8.3-B `[8.3.1 · Analysis]`** A risk has probability 0.03 and an impact the project could not
survive. The correct treatment is:
- A. accept it — the `EMV` is small
- B. treat it structurally: avoid, transfer, or reconsider viability, because `EMV` funds portfolios while survivability is governed by impact ✅
- C. fund its `EMV` in contingency and monitor
- D. exclude it from the register as improbable

*Rationale:* Existential exposure is not an averaging problem (8.3.1). A and C both apply portfolio
logic to a single point of failure; D removes the entry that most needs governance attention.

**MCQ 8.3-C `[8.3.2 · Analysis]`** Contingency freed by a retired risk is used to cover an
unrelated overspend. This is:
- A. efficient reserve management
- B. a governance failure: the reserve silently becomes a slush fund and the next genuine risk is unfunded ✅
- C. acceptable if the total baseline is unchanged
- D. required, since contingency is inside the baseline

*Rationale:* Contingency is tied to identified risks; reallocating it to overspends destroys the
link between reserve and exposure (8.3.2). C is the reasoning that makes the failure invisible.

### Self-check — KA 8.3

1. *When is acceptance the professionally correct response?* — When the response's cost exceeds
   the `EMV` reduction it buys — and the risk is survivable.
2. *What does transfer actually achieve?* — It prices the risk to a counterparty and creates
   counterparty risk; it does not remove the exposure.
3. *What should contingency consumption be trended against?* — Risk retirement, not elapsed time.

---

## Knowledge Area 8.4 — Resilience, bias and crisis leadership

*Topics: 8.4.1 resilience versus prediction · 8.4.2 cognitive bias · 8.4.3 crisis leadership ·
8.4.4 AI-enabled risk sensing.*

### 8.4.1 Resilience versus prediction

Quantification (KA 8.2) handles risks you can name. **Resilience** is the capability to absorb what
you cannot: it assumes the register is incomplete, because it always is. Its levers are structural
rather than analytical — **buffers** (float placed deliberately, contingency, capacity headroom),
**optionality** (staged commitments, alternative suppliers qualified in advance, reversible
decisions — Domain 1, KA 1.3.3), **modularity** (a failure contained rather than propagated across
interfaces), **redundancy** where failure is intolerable, and **fast detection** (early-warning
indicators, short feedback loops), because response cost rises with delay.

The trade-off is real and should be stated rather than finessed: resilience costs efficiency. A
project optimised to the last dollar and week has no capacity to absorb surprise, and one buffered
everywhere is uncompetitive. The leadership judgment is *where* to be resilient — at the points
where failure propagates or cannot be tolerated — not whether.

### 8.4.2 Cognitive bias

Estimates and reviews are produced by people, and the predictable distortions are worth naming
because each has a countermeasure:

| Bias | Effect | Countermeasure |
|---|---|---|
| **Optimism bias / planning fallacy** | Systematic underestimation of duration and cost | Reference-class forecasting against comparable completed projects |
| **Anchoring** | First number dominates subsequent estimates | Independent estimates before any figure is shared |
| **Availability** | Recent or vivid events overweighted | Structured checklists and historical base rates |
| **Groupthink / social pressure** | Dissent suppressed in workshops | Pre-mortem, private input channels, explicit dissent invitation |
| **Sunk-cost / escalation of commitment** | Continuing because of what is spent | Decision framed on remaining cost and benefit only (Domain 2's kill criteria) |
| **Confirmation bias** | Evidence sought that supports the plan | Assign someone to argue the opposite; test the disconfirming case |

The **pre-mortem** deserves particular mention as the highest-yield, cheapest technique in this
domain: before committing, ask the team to imagine the project has failed and to write down why.
It licenses the dissent a status meeting suppresses, and it reliably surfaces risks the workshop of
8.1.3 missed.

### 8.4.3 Crisis leadership

When a serious risk materialises, the leader's job changes shape. The sequence that works:
**stabilise** (stop the harm; secure safety, then the situation), **establish facts** (separate
what is known from assumed — crises run on rumour), **decide with a clock** (a good decision now
beats an optimal one later, and say explicitly which decisions are reversible), **communicate
early and honestly** (bad news does not improve with age; Domain 11's stakeholder work is now
executing under compressed time), **protect the team** (crises consume people, and exhausted teams
make the second mistake), and **capture the lesson while it is vivid** (Domain 9's
lessons-learned). Domain 6's recovery machinery is the schedule expression of the same posture:
re-run the passes, price the options, escalate the trade with named authority.

### 8.4.4 AI-enabled risk sensing

The genuine capability is **detection at scale**: anomaly detection across cost, schedule, quality
and supplier data; pattern recognition against historical projects; and sentiment or volume signals
in issue logs and correspondence that precede formal escalation. Used well, this shortens the
detection half of 8.4.1's fast-detection lever — which is where its value actually lies.

The honest boundaries: models detect **patterns like the past**, so novel risk is invisible to them;
they produce **false positives** that erode attention if unfiltered; and a **calibration record is
mandatory** before their alerts influence decisions (Domain 6, KA 6.4; Domain 14). The failure mode
to watch for is not a wrong alert but **displaced judgment** — a team that stops thinking about
risk because a dashboard is watching. The register's owners remain human, the analysis stays
challengeable, and the principle holds: AI proposes; the professional verifies, decides and remains
accountable.

### Key terms — KA 8.4

| Term | Meaning |
|---|---|
| **Resilience** | Capability to absorb unidentified risk; buffers, optionality, modularity, redundancy, fast detection. |
| **Reference-class forecasting** | Estimating from comparable completed projects to counter optimism bias. |
| **Pre-mortem** | Imagining failure before committing, to license dissent and surface missed risk. |
| **Escalation of commitment** | Continuing because of sunk cost; countered by forward-looking framing. |
| **Displaced judgment** | Ceasing to think because a tool is watching. |

### Sample MCQs — KA 8.4

**MCQ 8.4-A `[8.4.2 · Application]`** A programme's estimates have run 25 % low across four
comparable past projects. The most effective countermeasure is:
- A. instruct estimators to be more careful
- B. reference-class forecasting — estimate from the distribution of comparable completed projects rather than from the plan ✅
- C. add a 25 % contingency and continue the same process
- D. escalate to the sponsor for a larger budget

*Rationale:* Optimism bias is systematic, so the fix is methodological (8.4.2). A relies on
exhortation against a structural effect; C treats the symptom while preserving the cause;
D funds it without correcting it.

**MCQ 8.4-B `[8.4.1 · Analysis]`** A project has been optimised to eliminate all float and
buffers. Its risk position is:
- A. improved — waste has been removed
- B. degraded — it now has no capacity to absorb the risks not in the register, and resilience is what covers those ✅
- C. unchanged, provided contingency is funded
- D. improved, since efficiency reduces exposure time

*Rationale:* Resilience assumes register incompleteness (8.4.1); removing all absorption capacity
maximises fragility. C confuses money with time and capacity — funded contingency cannot buy back a
schedule with nowhere to move.

**MCQ 8.4-C `[8.4.4 · Analysis]`** An AI monitor has raised 40 alerts this month, 3 of which
mattered. The correct response is:
- A. disable the monitor
- B. tune thresholds and require a calibration record, because unfiltered false positives erode the attention the tool exists to direct ✅
- C. investigate all 40 equally
- D. accept the ratio as inherent to anomaly detection

*Rationale:* False positives consume the scarce resource (attention) that detection is meant to
focus (8.4.4). A discards real capability, C guarantees the erosion, D abandons the calibration
duty.

**MCQ 8.4-D `[8.4.3 · Recall]`** In the first hour of a crisis, the leader's priority order is:
- A. communicate, then investigate, then stabilise
- B. stabilise and secure safety, establish facts, then decide with a clock and communicate early ✅
- C. establish blame, then stabilise
- D. wait for complete information before acting

*Rationale:* Stabilisation and factual grounding precede decisions, which are taken against a clock
(8.4.3). D is the failure the sequence exists to prevent; C is never a first-hour activity.

### Self-check — KA 8.4

1. *Why is resilience not a substitute for quantification, and vice versa?* — Quantification
   handles named risks; resilience absorbs the unnamed. Each fails at the other's job.
2. *What is a pre-mortem for?* — To license dissent before commitment and surface risks a workshop
   suppresses.
3. *What is AI risk sensing's structural blind spot?* — Novel risk: it detects patterns like the
   past.

---

## Advanced topics — Domain 8

### 8.A.1 Correlation and why aggregate risk is worse than it looks

Independence made 8.2.4's arithmetic tractable and understated the answer. When risks share a
driver — one supplier, one technology, one regulator, one labour market — they move together, and
correlated variance is additive in a way that widens the aggregate distribution's tail sharply. The
practical implications: identify **common drivers** explicitly and group risks by them; model
correlation in simulation rather than assuming it away; and treat a register with many entries and
few drivers as *concentrated*, not diversified. The organisational version of this error is
believing a portfolio of projects is diversified when every one depends on the same scarce
engineering resource.

### 8.A.2 Schedule risk analysis and the merge-bias trap

Applying this domain's methods to Domain 6's network produces **quantitative schedule risk
analysis**: three-point durations (KA 6.4.3) simulated across the logic to yield a completion
distribution and, valuably, a **criticality index** — how often each activity lands on the critical
path across simulations — which is more decision-useful than a single deterministic critical path.
The trap it exposes is **merge bias**: where paths converge (Domain 6's node E), the merged event
waits for the *latest* predecessor, so the probability of being on time is the product of the
paths' probabilities, not the best of them. Two paths each 80 % likely to meet a date give the
merge point 64 %. Deterministic CPM cannot see this, which is why convergence points are
systematically optimistic and why Domain 6 flagged them for attention.

### 8.A.3 The reviewer's risk eye

Invariants worth testing in an hour: every entry has cause, event and consequence and a named
owner; no occurred risks parked as high-probability entries; opportunities present in credible
proportion; Σ`EMV` reconciles to the register; contingency stated at a named confidence level with
its independence assumption disclosed; contingency consumption trended against risk retirement,
not time; responses costed against `EMV` reduction, with secondary risks recorded; existential
risks handled structurally rather than by `EMV`; early-warning indicators defined for the top
items; and at least one decision this month traceable to the register. Failure of the last one
matters most: it means the process is documentation, not decision support.

---

## Industry variations — Domain 8

- **Construction and infrastructure.** Ground conditions, weather and permits dominate;
  quantitative schedule risk analysis is contractually expected on major programmes, and risk
  allocation is largely executed through contract terms (Domain 10).
- **Energy and resources.** Commodity price and currency exposure sit alongside delivery risk and
  are managed with financial instruments as well as project responses (PFL-AI Domains 3, 11).
- **Technology and digital.** Requirement volatility and integration risk dominate; resilience is
  bought through modularity, staged release and reversibility rather than buffers, so 8.4.1's
  levers apply in a different mix.
- **Pharmaceutical and regulated development.** Low-probability/high-consequence events and
  regulatory outcomes make structural treatment (8.3.1) the norm; risk files are auditable
  artefacts, not internal management tools.
- **Public programmes.** Optimism bias is explicitly recognised in appraisal guidance, and
  reference-class adjustment may be mandated; political and reputational risk carries weight that
  no `EMV` captures well.

## Case study — Domain 8: the survey Auriga did not commission (utilities)

**Situation.** At planning, Auriga's team considered a USD 25,000 ground survey ahead of the civil
works. The register carried R2 — ground conditions worse than surveyed, `p` 0.50, impact
USD 180,000 — and the survey was cut in a cost-reduction pass as "nice to have". The decision was
not analysed; it was traded against a round-number savings target.

**What happened.** At week 13 the contamination was found. The reactive response cost more than the
planned one would have — a second civil crew at USD 35,000, a fast-track carrying
`0.20 × 60,000 = 12,000` of expected rework, and the exposure to a USD 45,000-per-week client
penalty window that made the whole recovery urgent (Domain 6's case study; Domain 7's forecast
consequence, `EAC` USD 4.2m against `BAC` 4.0m).

**The analysis, done afterwards.** On the decision-tree arithmetic of 8.2.3 — 0.40 probability at
the time of the decision, USD 300,000 reactive versus USD 90,000 planned — the survey was worth
**USD 59,000** against its USD 25,000 price. Cutting it saved 25,000 and cost the project
substantially more, and the register had contained the information needed to see that.

**What the domain teaches here.** Two things, both uncomfortable. **A cost-reduction pass that does
not consult the register is a risk decision taken blind** — savings targets and risk exposure must
be traded explicitly. And **the register was not the failure; its irrelevance to the decision
was** (8.3.3's test: did a decision change because of it?). The corrective adopted afterwards was
procedural: any proposed saving above a threshold must state which register entries it affects and
by how much.

## Case study B — Domain 8: the diversified portfolio that wasn't (technology programme)

**Situation.** A transformation programme's board reviewed 22 project-level risks spread across six
workstreams and concluded exposure was well diversified — no single item exceeded 8 % of the
aggregate `EMV`, and contingency was set at the aggregate mean plus a modest margin, assuming
independence.

**What happened.** Fourteen of the 22 risks depended on the same six-person integration team.
When two members left within a month, eleven risks materialised or worsened *together*. The
aggregate outcome landed far beyond the contingency, which had been sized on an independence
assumption nobody had tested — the correlated variance of 8.A.1 in practice. The programme's
recovery required a re-baseline and a management-reserve release.

**What was done differently.** The register was restructured by **common driver** rather than by
workstream, immediately revealing the concentration; the integration capability was treated as a
single structural risk with a structural response (cross-training, a retained partner, staged
scope) rather than as fourteen separate `EMV` line items; and contingency was re-derived by
simulation with correlation modelled explicitly.

**What the domain teaches here.** Counting entries measures paperwork; counting **drivers** measures
exposure. A register with many risks and few drivers is concentrated, and independence is the
assumption most likely to be both convenient and wrong.

---

## Executive perspective — Domain 8

What a project leader cannot delegate in this domain:

- **The confidence level.** Whether contingency is set at P50 or P80 is a risk-appetite decision
  belonging to governance, and the leader ensures it is chosen, stated and understood — not
  inherited from a spreadsheet default.
- **The independence question.** Asking, of any aggregate number, which risks share a driver. It is
  one sentence and it is where Case study B's programme lost control.
- **The structural exposures.** Existential risks handled by structure, never by `EMV` — and named
  personally by the leader, because low probability makes them easy to leave in the register.
- **The register's relevance.** Insisting on the 8.3.3 test: which decision changed because of it
  this month? A register that changes nothing gets fixed or stopped.
- **The bias countermeasures.** Running the pre-mortem, commissioning independent estimates,
  framing continuation decisions on remaining cost and benefit — because the leader is the only
  person positioned to make dissent safe.
- **The AI boundary.** Detection is delegable; judgment is not, and a team that has stopped
  thinking because a dashboard is watching is the outcome to prevent.

## Calculation exercises — Domain 8

**Exercise 8.1** Risks: A `p` 0.40 impact 150,000; B `p` 0.20 impact 500,000; C `p` 0.60 impact
80,000; D (opportunity) `p` 0.25 impact (200,000). Compute each `EMV` and the total exposure.
*Solution.* A **60,000**; B **100,000**; C **48,000**; D **(50,000)**. Total
**USD 158,000**. Common error: omitting the opportunity, giving 208,000 and overstating required
contingency by 31.6 %.

**Exercise 8.2** Using Exercise 8.1's four risks and assuming independence, compute σ and a P80
contingency (mean + 0.8416 σ).
*Solution.* Variances: `0.40×0.60×150,000² = 5.40e9`; `0.20×0.80×500,000² = 40.0e9`;
`0.60×0.40×80,000² = 1.536e9`; `0.25×0.75×200,000² = 7.50e9`. Total **54.436e9**;
σ = **USD 233,315**; P80 = `158,000 + 0.8416 × 233,315 =` **USD 354,358**. Common error: taking
√(Σσ) rather than √(Σvariance).

**Exercise 8.3** A risk (`p` 0.30, impact 400,000) can be mitigated for USD 70,000, reducing
probability to 0.10. Is the mitigation worthwhile?
*Solution.* `EMV` before `0.30 × 400,000 =` 120,000; after `0.10 × 400,000 =` 40,000; reduction
**80,000** against a cost of 70,000 → **net +USD 10,000, accept** (marginally — and the leader
should note that a 400,000 impact may warrant response on survivability grounds regardless).
Common error: comparing the mitigation cost against the *impact* rather than the `EMV` reduction.

**Exercise 8.4** Two independent paths each have a 0.80 probability of meeting a milestone date and
converge on it. What is the probability the milestone is met, and what is the effect called?
*Solution.* `0.80 × 0.80 =` **0.64**. **Merge bias** — the merged event waits for the later path,
so convergence points are systematically optimistic in deterministic CPM (8.A.2). Common error:
answering 0.80, the best of the two.

## Practitioner's toolkit — Domain 8

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 8.T.1 — Risk register columns that earn their keep

ID · **cause → event → consequence** statement · affected objective · **common driver** (8.A.1) ·
probability · impact (cost and time) · `EMV` · qualitative band (screening only) · response family
and description · **response cost and expected `EMV` reduction** · **secondary risks created** ·
owner (a person) · early-warning indicator · review date · status (open/retired/**issue**) ·
retirement evidence. Omit any column and the register loses a specific capability; the two most
commonly missing — common driver and response cost — are the two that make it decision-useful.

### Toolkit 8.T.2 — Contingency derivation sheet (one per baseline)

Register version and date · Σ`EMV` · variance and σ by risk · **independence assumption stated and
challenged** (which risks share drivers) · method (formula approximation or simulation, with
correlation treatment) · **confidence level chosen and by whom** · resulting contingency ·
management reserve separately, with its basis · draw protocol reference. A contingency figure
without this sheet is a number without a defence.

### Toolkit 8.T.3 — Pre-mortem session guide

Before commitment, one hour: (1) state the plan and the commitment about to be made; (2) each
participant writes privately for ten minutes — "it is twelve months on and this has failed; why?";
(3) round-robin with no challenge permitted during collection; (4) cluster into causes and identify
**common drivers**; (5) convert each into a three-part risk statement with an owner; (6) identify
the three cheapest early-warning indicators. Rule: the most senior person speaks last, always.

## Exam preparation — Domain 8

**The traps.** Multiplying and summing ordinal scores as if they were money (8.2.1) · omitting
opportunities from total exposure (Exercise 8.1) · setting contingency at Σ`EMV` and calling it
funded (MCQ 8.2-D) · √(Σσ) instead of √(Σvariance) (Exercise 8.2) · comparing response cost against
impact rather than `EMV` reduction (Exercise 8.3) · managing an existential low-probability risk by
its `EMV` (8.3.1) · assuming independence without testing common drivers (8.A.1) · answering the
best path probability at a merge point (Exercise 8.4) · leaving occurred risks in the register as
`p` = 1.0 · treating a transfer as removing exposure.

**Reflection questions.**
1. Take your project's contingency figure: what confidence level does it represent, who chose it,
   and what independence assumption sits underneath?
2. Group your register by common driver rather than by workstream. How concentrated is it actually?
3. Which decision changed last month because of your register — and if none, is the process
   decision support or documentation?

## Domain 8 summary

Risk management earns its place only when it changes decisions. That starts at the sentence level:
cause → event → consequence, against a named objective, with opportunities identified as
deliberately as threats because defensive framing otherwise buries them. Analysis then progresses
from qualitative screening — useful for ordering, invalid as arithmetic — to quantification:
`EMV` for each risk (Auriga's register totalling **USD 278,000**, 6.95 % of `BAC`, and USD 314,000
if the opportunity is ignored), decision trees that price choices and the **value of information**
(the survey worth **USD 59,000** against its 25,000 price, and worthless if it changed no action),
and aggregation to a **stated confidence** — Auriga's P80 of **USD 490,624** from a mean of 278,000
and σ of 252,642, which is defensible precisely because it says what confidence it buys, unlike a
worst-case sum, an `EMV` sum or a 10 % rule of thumb. Responses are investments judged against the
`EMV` reduction they buy, with secondary risks recorded and existential exposures handled
structurally rather than by averaging; reserves are sized by aggregation, separated by authority,
and trended against risk retirement rather than time. Beyond what can be named sits resilience —
buffers, optionality, modularity, redundancy and fast detection, bought at an efficiency cost that
should be chosen rather than stumbled into — together with the bias countermeasures (pre-mortem,
reference-class forecasting, independent estimates) and the crisis sequence of stabilise, establish
facts, decide against a clock, communicate early. Independence is the assumption most likely to be
convenient and wrong: count drivers, not entries. Domain 9 turns to quality and the assurance that
catches what risk analysis missed.
