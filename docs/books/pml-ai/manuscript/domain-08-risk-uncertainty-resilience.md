# Domain 8 — Risk, Uncertainty and Resilience
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

Beneath that lies the domain's central quantitative claim, and it is the one most often missed:
**aggregate risk is not the sum of its parts, and the term that decides the difference is
variance.** Every number a leader is actually asked for — a contingency, a confidence level, a
buffer, a date the board can commit to — is a statement about the *spread* of an aggregate
distribution, not about its average. Expected monetary value, which is where most training stops,
fixes only the average; it is silent on the shape, and the shape is what breaks projects. Three
consequences run through the rest of this domain. Two registers with identical total `EMV` can need
reserves that differ by a third, because one concentrates its exposure in a few large, unlikely
events and the other spreads it. Two responses of identical cost that remove identical `EMV` can
differ by a third in the reserve they release, because one attacks a probability and the other an
impact. And a reserve labelled P80 can be a P73 reserve the moment a shared driver is admitted —
the label failing before the money does. A leader who can compute those three things is doing risk
management; a leader who can only multiply probability by impact is doing arithmetic.

**Learning objectives.** After this domain a candidate can: distinguish risk from uncertainty and
issue; write a risk statement that supports a decision; identify opportunities as rigorously as
threats; **estimate how complete a register is from the overlap between two independent
identification methods, and use the answer to size resilience rather than to reassure**; run
qualitative screening, **compute the span of expected values a single matrix cell can conceal and
derive the band ratio a defensible scale needs**; compute `EMV` for individual risks and interpret
the total, including the sensitivity of its ranking to the probabilities assumed; build and solve a
decision tree, including the value of information; **value imperfect information, show that the
value depends on how the sample is designed as much as on what it costs, and compute the prior
probability below which buying it destroys value**; aggregate risks to a mean and a confidence
level rather than summing point estimates, **and check the normal approximation against the exact
enumerated distribution**; **quantify what one shared driver does to an aggregate, and state the
confidence an independence-based reserve actually buys**; **rank responses by the confidence-level
reduction they buy rather than by `EMV` reduction, and explain from the algebra why an impact lever
removes between 1.5 and 3 times the variance of a probability lever at equal `EMV` cost**; convert
a risk-appetite statement into an operable tolerance, a confidence level and an escalation
threshold; size a contingency reserve defensibly, distinguish it from management reserve, **and
test a part-consumed reserve against the register still open**; select and cost responses **across
the response families with their secondary risks priced, and show where omitting the secondary
risks selects the wrong response**; explain resilience as distinct from prediction and **price a
resilience measure on both its expected value and its tail**; recognise the biases that corrupt
estimates and reviews, **and reconcile a bottom-up register against a reference class of comparable
outturns**; lead in a crisis; **compute merge bias at a convergence point, correct it for shared
predecessors, and state the date range over which it stops mattering**; and govern AI-produced risk
analysis, **including deriving the base rate at which a sensitive monitor beats a specific one**.

**The master worked project — and its programme.** Project Auriga continues from Domains 6 and 7 —
the 25-week, **`BAC` USD 4,000,000** control-systems upgrade. Its risk register, used throughout
KA 8.2–8.3, carries the ground-conditions risk that actually materialised in Domain 6's case study,
so the reader sees the same event before and after the fact. Its five-risk register has a total
`EMV` of **USD 278,000**, an independence-based P80 of **USD 490,624**, and — once one shared
subcontractor is admitted — a σ of **USD 340,339** that turns that same reserve into a **73.4 %**
reserve.

Single-project arithmetic is not, however, where most contingency is lost, so this domain also
works **Meridian Care Records** at programme scale, the fictional public-health programme of
Domains 1, 2, 5, 15 and 16: **40 clinics**, approved cost **USD 2,400,000**, a cost of delay of
**USD 14,280 per week**, and a board risk appetite that this domain converts into a number. Its
five quantified risks total **USD 119,000** of `EMV` against a 5 % tolerance of **USD 120,000** —
which is the whole problem in one comparison, and the reason KA 8.3 exists.

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
treat the same exposure differently. The conversion is arithmetic and it has a result worth
anticipating: a well-formed appetite statement **determines the confidence level at which
contingency is held**, so P80 is not a convention to inherit but a consequence to derive. Worked
example 8.3.2 does that conversion on Meridian, because the reserve is where the number lands.

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

**How complete is the register?** Every method above leaves something out, and the resilience
argument of KA 8.4.1 rests on the claim that the register is *always* incomplete. That claim can be
measured rather than asserted, and the measurement costs nothing beyond running two identification
methods separately and comparing their lists. If method 1 finds `n₁` risks, method 2 finds `n₂`, and
`m` risks appear on both, then — treating the two methods as independent samples from the same
underlying population of risks — the population is estimated by the standard capture–recapture
estimator:

```
N̂ = (n₁ × n₂) / m            (Lincoln–Petersen)
N̂c = ((n₁+1)(n₂+1))/(m+1) − 1  (Chapman, less biased at small m)
```

The logic is the same as estimating the size of a fish population by tagging and re-sampling: the
smaller the overlap between two independent samples, the larger the population they were drawn
from. Applied to a risk register it converts an uncomfortable feeling into a number a leader can
act on.

**Worked example 8.1.3 — how much of Meridian's register is missing?**

1. **Setup.** Meridian's identification ran two methods separately and deliberately. A structured
   workshop with clinical, technical and operational representatives produced **34** distinct
   risks. Independently, and without seeing the workshop output, an assumption-analysis review of
   the programme plan (against Domain 2's assumption register) produced **22**. Comparing the lists,
   **14** risks appear on both. Each register entry is written in the three-part form of 8.1.2, so
   "the same risk" means the same cause-and-event pair, not similar wording. The assumption review
   cost **USD 12,000** of reviewer time.
2. **Formula.** `N̂ = n₁n₂/m`; distinct identified = `n₁ + n₂ − m`; estimated unidentified =
   `N̂ − (n₁ + n₂ − m)`; coverage = distinct ÷ `N̂`. Cross-check with Chapman's
   `N̂c = ((n₁+1)(n₂+1))/(m+1) − 1`.
3. **Substitution.** `N̂ = (34 × 22)/14 = 748/14`; distinct `= 34 + 22 − 14`;
   `N̂c = (35 × 23)/15 − 1 = 805/15 − 1`.
4. **Result.** Estimated population **53.4286** risks (Chapman: **52.6667**). Distinct identified
   **42**. Estimated unidentified **11.4286** risks — **21.39 %** of the estimated population, so
   coverage is **78.61 %** (Chapman: 10.6667 missing, **79.75 %** coverage). The workshop *on its
   own* covered **63.64 %** of the estimated population. The assumption review added **8** risks the
   workshop had missed, at **USD 1,500** each.
5. **Interpretation.** Four things follow, and the fourth is the one that changes a decision.

   **The workshop is not "the risk identification".** It found under two-thirds of the estimated
   population, and it is routinely the only method run. The eight risks the assumption review added
   are, by construction, exactly the ones the workshop's shared frame of reference could not see —
   the failure mode named at the top of this topic, now with a size attached. At USD 1,500 per new
   risk the review pays for itself if the average newly found risk carries more than USD 1,500 of
   avoidable exposure, which on a USD 2,400,000 programme is close to a certainty and is why the
   second method is not optional.

   **The bias runs one way, so the estimate is a floor.** Capture–recapture assumes the two methods
   are independent and that every risk is equally detectable. Neither holds exactly. Both methods
   were run by people inside the same programme, so they share some blind spots; shared blind spots
   raise `m`, and a higher `m` *lowers* `N̂`. Unequal detectability — obvious risks found by both
   methods, subtle ones by neither — pushes the same way. So **11.43 is a lower bound on what is
   missing**, and a leader should present it as "at least eleven risks we have not written down",
   never as a precise count. The Chapman cross-check moving the estimate by less than one risk is
   evidence that the arithmetic is stable, not that the assumptions are true.

   **The estimate does not license adding `EMV` for imaginary risks.** The unidentified 11.43 have
   no cause, no event, no consequence and no owner; they cannot be quantified, responded to or
   escalated. What they justify is **management reserve** (8.3.2) and **resilience** (8.4.1) — money
   and capacity held against exposure that has no line item. That is precisely the distinction
   between the two reserve types, now with a quantitative basis rather than a shrug, and it is why
   an organisation that folds management reserve into contingency has no way to fund what this
   arithmetic finds.

   **And the number is a decision rule, not a report line.** Meridian's programme office set a
   coverage threshold: below 85 %, commission a third independent method before the tranche gate.
   At 78.61 % the threshold fired, and a supplier-side review was run. The professional caution is
   the mirror image: **coverage above the threshold is not evidence that identification is
   finished** — it is evidence that two methods agree, which on a novel programme is as consistent
   with a shared blind spot as with completeness. Deep uncertainty (8.1.1) is invisible to this
   arithmetic by definition, because it cannot be enumerated at all.

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
| **Capture–recapture estimate** | Register completeness inferred from the overlap of two independent identification methods: `N̂ = n₁n₂/m`. |
| **Coverage** | Distinct risks identified as a share of the estimated population; an optimistic figure, since shared blind spots deflate the population and so inflate it. |

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

**MCQ 8.1-D `[8.1.3 · Application]`** A workshop finds 34 risks and an independent assumption
review finds 22, with 14 common to both. The estimated risk population and the estimated number
still unidentified are:
- A. 56 and 14
- B. 53.43 and 11.43 ✅
- C. 42 and nil — the two methods between them found everything
- D. 78.61 and 36.61

*Rationale:* `N̂ = (34 × 22)/14 = 53.4286`, and 42 distinct entries were identified, leaving
**11.4286** (8.1.3). A adds the two lists without removing the 14 duplicates and then treats the
overlap as the missing count; C assumes two methods exhaust the population, which is the assumption
the estimator exists to test; D takes the coverage percentage (78.61) as a population count and
subtracts the 42 from it.

**MCQ 8.1-E `[8.1.3 · Analysis]`** Two identification methods overlap heavily, giving a high
coverage estimate. The soundest professional reading is:
- A. identification is complete and can be closed
- B. a large overlap deflates the population estimate, so high coverage is as consistent with a shared blind spot as with completeness ✅
- C. one of the two methods was run incorrectly
- D. the estimate is invalid whenever the overlap exceeds half of either list

*Rationale:* `N̂ = n₁n₂/m` falls as `m` rises, so methods sharing a frame of reference produce
reassuring coverage; the estimate is a floor on what is missing (8.1.3). A is the error the
arithmetic is meant to prevent; C infers a process fault from a statistic that has other
explanations; D invents a threshold the estimator does not have.

### Self-check — KA 8.1

1. *State the three parts of a usable risk statement.* — Cause, event, consequence, against a
   named objective.
2. *Why is assumption analysis the highest-yield identification method?* — Every assumption is a
   risk in disguise, and plans rest on more of them than teams admit.
3. *What is the register consequence of treating an occurred risk as a 100 % risk?* — It clogs the
   forward-looking register; occurred risks are issues.
4. *What does a capture–recapture coverage estimate justify, and what does it not?* — It justifies
   management reserve and resilience sized against unidentified exposure; it does not justify adding
   `EMV` for risks that have no cause, event, consequence or owner.
5. *Which way does the bias run in a register-completeness estimate?* — Shared blind spots raise the
   overlap and lower the population estimate, so the count of missing risks is a floor.

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

How coarse a screen is it? That is answerable, and the answer is the argument for the quantification
threshold rather than a matter of taste. A matrix cell is a rectangle in probability-by-impact space,
so the range of expected values it contains follows directly from the widths of its two bands.

**Worked example 8.2.1 — what one matrix cell conceals.**

1. **Setup.** A delivery organisation screens on a 3 × 3 matrix. Probability bands: **Low**
   0.01–0.10, **Medium** 0.10–0.35, **High** 0.35–0.70. Cost-impact bands: **Low**
   USD 10,000–60,000, **Medium** USD 60,000–250,000, **High** USD 250,000–800,000. Two risks are
   both scored **Medium probability / Medium impact** and therefore receive the same attention, the
   same review cadence and the same response budget. How different can they actually be?
2. **Formula.** Within one cell, `EMV` ranges from `p_min × I_min` to `p_max × I_max`, so the span
   factor is `(p_max/p_min) × (I_max/I_min)` — the product of the two band ratios. Compare that
   against the step between adjacent cells, taken at band midpoints.
3. **Substitution.** Cell floor `0.10 × 60,000`; cell ceiling `0.35 × 250,000`. Span factor
   `(0.35/0.10) × (250,000/60,000) = 3.5 × 4.1667`. Midpoint of Medium/Medium
   `0.225 × 155,000`; of High/High `0.525 × 525,000`.
4. **Result.** The Medium/Medium cell contains expected values from **USD 6,000** to
   **USD 87,500** — a span factor of **14.5833**, exactly `3.5 × 4.1667`. The step from the
   Medium/Medium midpoint (**USD 34,875**) to the High/High midpoint (**USD 275,625**) is a factor
   of **7.9032**. So the range *inside* one cell is **1.8452 times** the whole step from that cell to
   the one a full band higher on **both** axes.
5. **Interpretation.** The identity is the useful part, and it converts a familiar complaint into a
   design specification.

   **Within-cell span = probability band ratio × impact band ratio.** Nothing about the shading, the
   wording of the bands or the number of colours changes it; only the ratios do. A matrix whose
   bands span factors of 3.5 and 4.17 cannot resolve two risks that differ by a factor of fourteen,
   which means **its internal resolution is coarser than its own band structure implies**. Two risks
   in the same cell here can differ by more than the distance the matrix draws between "medium" and
   "high". That is why ordinal scores must not be multiplied and summed (the point above), and it is
   also why "we manage all amber items the same way" is a funding error rather than a simplification:
   at the extremes of this cell, a response budget set by cell allocates the same money to an
   exposure of 6,000 and one of 87,500.

   **The design rule falls straight out.** To hold the within-cell span below a factor of 4, each
   axis needs a band ratio below 2. Covering probabilities from 0.01 to 0.70 — a range of **70** —
   in bands of ratio 2 requires **7** bands (`log₂ 70 = 6.1293`); covering impacts from 10,000 to
   800,000 — a range of **80** — requires **7** as well (`log₂ 80 = 6.3219`). A 7 × 7 matrix on
   ratio-constant bands is a defensible screen; a 3 × 3 matrix on the same total range is not, and no
   amount of careful colouring repairs it. Note what this also says about **linear** bands, which are
   commoner and worse: equal-width bands have enormous ratios at the bottom of the scale (a band
   from 0.01 to 0.10 is a ratio of 10) and negligible ones at the top, so a linear matrix is least
   reliable exactly where low-probability high-impact risks live — the class 8.3.1 says must be
   handled structurally.

   **What screening is still for.** The conclusion is not that matrices are useless; it is that they
   are a triage instrument whose output is an *attention order*, and that they must be paired with a
   stated quantification threshold. Meridian's threshold was set at the Medium/Medium floor: any risk
   whose assessed impact reaches USD 60,000 is quantified individually, **plus every identified
   opportunity whatever its size**, because the under-identification of 8.1.3 means an impact
   threshold applied symmetrically would remove the few opportunities a register contains. That took
   its 42-item register down to the five entries 8.2.2b aggregates — four threats above the floor
   and one opportunity of USD 40,000 below it. The reviewer's test is simply
   whether that threshold is written down: a screen with no threshold is a screen that has replaced
   quantification instead of ordering it. One caution. Recalibrating the bands to reduce span makes
   the matrix harder to complete quickly, which is the property that made it useful — so the
   trade-off should be made deliberately at the organisational level, not per project. (Nothing above
   touches the correlation blindness named at the top of this topic; band width cannot repair it.)

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
   opportunity is ignored, an overstatement of **12.95 %**, which is why opportunities must be in the
   same arithmetic rather than mentioned in prose. Five things the table teaches, in the order a
   reviewer should test them.

   **`EMV` is an average of outcomes that will not happen.** No single risk will cost 84,000 — R1
   costs 240,000 or nothing. So `EMV` is the right basis for *funding a portfolio* of risks and the
   wrong basis for deciding whether one specific risk is survivable. The strongest form of the point
   is arithmetic: the probability that all four threats occur together, producing the 1,140,000
   worst case, is `0.35 × 0.50 × 0.25 × 0.15 =` **0.65625 %**, while the probability that the
   register's net cost is **zero or better** — no threat occurs, or only the opportunity does — is
   **20.72 %** (enumerated in 8.2.4). One future in five costs nothing; the register's central
   estimate describes neither of those futures.

   **Ranking by `EMV` reorders the register.** R4 has the largest impact (400,000) but the smallest
   threat `EMV` (60,000), while R3's lower impact carries more expected cost. A leader managing by
   impact alone would attend to them in the wrong order — though see 8.3.1, because impact still
   governs *survivability*.

   **That ranking is fragile, and the fragility is computable.** R4's `EMV` overtakes R1's as soon as
   its probability reaches `84,000/400,000 =` **0.21**, and tops the whole register at
   `90,000/400,000 =` **0.225**. So a six-point revision in one probability — well inside the
   precision of any real assessment — reverses the top of the order. Likewise a plausible ±0.05 error
   on R2's probability moves its `EMV` by ±USD 9,000 and the total by **±3.24 %**. The professional
   consequence is a presentation rule: quote total exposure to the nearest thousand at most, and
   where a response decision turns on a rank, state the probability at which the rank flips rather
   than defending the rank itself.

   **The total is a mean, and means are additive whatever else is true.** Σ`EMV` is the mean of the
   aggregate distribution under *any* dependence structure between the risks — correlation moves the
   spread and not the average (8.2.4b proves it on this register, and the AI note at the end of this
   KA turns it into a hand-check).

   **And the sign convention has to be enforced, not assumed.** R5's `EMV` is carried as
   (36,000) — a reduction in expected cost — because it is an opportunity against the same objective
   (cost) as the four threats. Mixing objectives inside one total is the commoner and more damaging
   version of the same error, and 8.2.2b takes it up.

**Worked example 8.2.2b — Meridian's programme register, and the objective a total belongs to.**

1. **Setup.** Meridian's 42-item register (8.1.3) has five entries above the quantification
   threshold of 8.2.1. All five are assessed against a **single objective — cost against the
   approved USD 2,400,000** — and the programme separately holds benefit risks, schedule risks and
   reputational risks in their own aggregations.

   | ID | Risk (cause → event → consequence, abbreviated) | `p` | Impact (USD) | `EMV` (USD) |
   |---|---|---|---|---|
   | M1 | Legacy extract quality → second data run needed at clinics | 0.40 | 90,000 | 36,000 |
   | M2 | Clinician availability → extended parallel running at the 8 largest clinics | 0.35 | 120,000 | 42,000 |
   | M3 | National records-format revision → interface rebuild | 0.20 | 150,000 | 30,000 |
   | M4 | Trainer attrition → contracted backfill for the training programme | 0.30 | 70,000 | 21,000 |
   | M5 | Shared-licence rebate on completing the rollout early *(opportunity)* | 0.25 | (40,000) | **(10,000)** |
   | | **Total expected cost exposure** | | | **119,000** |

2. **Formula.** `EMV = p × impact`, summed within one objective. Express the total against the
   objective's tolerance, not against the whole approved cost, because the tolerance is what
   governance actually controls (8.3.2).
3. **Substitution.** `0.40 × 90,000`, `0.35 × 120,000`, `0.20 × 150,000`, `0.30 × 70,000`,
   `0.25 × (40,000)`. Tolerance = `5 % × 2,400,000`.
4. **Result.** Total expected cost exposure **USD 119,000**, which is **4.96 %** of the approved
   USD 2,400,000 — and **99.17 %** of the board's 5 % tolerance of **USD 120,000**.
5. **Interpretation.** The comparison in step 4 is the whole finding, and it is invisible if the
   total is expressed as a percentage of approved cost.

   **The mean alone consumes 99.17 % of the tolerance.** Because a mean is roughly the 50th
   percentile of an aggregate, that means Meridian has approximately a **coin-flip** chance of
   breaching its stated tolerance before any confidence level is discussed. No contingency figure can
   repair this, because contingency covers the risks; the tolerance is the space the risks have to
   fit inside. Only three things can: reduce the exposure by response (KA 8.3), widen the tolerance
   through governance, or reduce scope. Presenting a P80 contingency without saying this would be
   technically correct and professionally negligent, and 8.3.2 completes the arithmetic.

   **Why one objective per total, always.** M2's consequence is cost — paying for parallel running.
   Had it been written as "adoption below plan", its consequence would be *benefit*, measured against
   Meridian's USD 685,440 a year (Domain 1, KA 1.3.2) and against a different tolerance owned by a
   different body. Summing a cost impact and a benefit impact produces a number that is compared with
   nothing, cannot be funded from any reserve, and is the commonest reason a programme register's
   "total exposure" is quietly ignored by the board that receives it. The register may hold every
   objective; a **total** may hold only one. Where a single event hits two objectives — a delay that
   costs money *and* defers benefit at USD 14,280 a week — it appears in both aggregations with the
   relevant consequence, and the double appearance is disclosed rather than netted.

   **Scale changes which comparison matters, not the arithmetic.** Auriga's 278,000 against a
   4,000,000 `BAC` reads as comfortable at 6.95 %; Meridian's 119,000 against a 2,400,000 approved
   cost reads as comfortable at 4.96 % and is not, because the governing constraint is a 120,000
   tolerance rather than the budget. The reviewer's question is therefore never "what percentage of
   the budget is the exposure?" but **"what is the exposure as a share of the room we have?"** —
   and if nobody can state the room, that is the finding.

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
   nothing when it does not, however reassuring it feels. **Information is valuable only in
   proportion to the better action it enables.** (Domain 6's case study is the counterfactual: Auriga
   did not survey, met the condition reactively, and paid the recovery this tree prices.)

   Three breakevens make the conclusion defensible rather than merely correct, and each is one
   division.

   **How much could the survey have cost?** Branch B beats branch A while the survey price stays
   below `120,000 − 0.40 × 90,000 =` **USD 84,000** — **3.36 times** the actual price. That is the
   sentence for a procurement conversation: a survey quoted at three times the budget figure would
   still have been the right purchase, so the decision was never close and the cost-reduction pass
   that cut it (see the case study) was not making a marginal call.

   **At what prior does information stop paying?** The survey pays while
   `25,000 < p × (300,000 − 90,000)`, i.e. `p >` `25,000/210,000 =` **11.90 %**. Below a roughly
   one-in-eight belief in contamination, the survey destroys value. This is the number that governs
   whether the same tree is worth building on the *next* site, and it explains why an
   apparently identical decision can go the other way on ground everyone considers clean.

   **And what would make the informed action worthless?** If mitigation once forewarned still cost
   `M`, branch B is `25,000 + 0.40M`, which reaches branch A's 120,000 at `M =` **USD 237,500**. So
   the entire value of knowing rests on a planned response costing less than 237,500; at the 250,000
   the text above uses as an illustration, B's EV is `25,000 + 100,000 = 125,000` and the survey
   destroys 5,000 of value. **Information's value is bounded by the response it enables**, which is
   why "we should get more data" is not a proposal until the responder and the response are named.

   One further property is worth naming because 8.2.3b relaxes it. This survey is assumed to
   **reveal the condition reliably** — a perfect signal — so its gross value equals the value of
   perfect information, `120,000 − 36,000 =` **USD 84,000**, and it captures 100 % of what could
   possibly be known. Real signals are imperfect, and their value falls in a way that depends less on
   their price than on how they are designed.

**Worked example 8.2.3b — Meridian's pilot, and the value the sample design decides.**

1. **Setup.** Meridian's migration approach may be unfit for clinics running the **legacy patient
   index**; **12 of the 40** clinics run it, and which twelve is known from the asset data. Whether
   the approach fails on that configuration is *not* known: the programme assesses the probability at
   **0.30**. Discovered only at full rollout, the failure costs **USD 480,000** of rework plus
   **6 weeks** of programme delay at the cost of delay of **USD 14,280 per week**. Discovered in a
   pilot, the approach is redesigned for **USD 130,000** plus **2 weeks** of delay. A two-clinic
   pilot costs **USD 45,000**. Three options: no pilot; a pilot at two clinics chosen at random from
   the 40; a pilot at two clinics chosen so that at least one runs the legacy index.
2. **Formula.** Late total = rework + 6 × cost of delay. Early total = redesign + 2 × cost of delay.
   For a random pair, the flaw is detected only if at least one pilot clinic runs the legacy index:
   `P(detect) = 1 − C(28,2)/C(40,2)`. EV(cost) = pilot price + `p ×` [detect × early +
   (1 − detect) × late]. Breakeven prior: pilot price ÷ (late − expected cost given the pilot).
3. **Substitution.** Late `480,000 + 6 × 14,280`; early `130,000 + 2 × 14,280`. Random-pair
   detection `1 − (28 × 27)/(40 × 39) = 1 − 756/1,560`. Designed pair detection `= 1`.
4. **Result.** Late total **USD 565,680**; early total **USD 158,560**.

   | Option | Detection given a flaw | Expected cost (USD) | Value vs no pilot |
   |---|---|---|---|
   | No pilot | — | `0.30 × 565,680 =` **169,704.00** | — |
   | Random two clinics | **51.5385 %** | `45,000 + 0.30 × 355,856.62 =` **151,756.98** | **17,947.02** |
   | **Designed two clinics** | **100 %** | `45,000 + 0.30 × 158,560 =` **92,568.00** | **77,136.00** |

   The **design choice alone is worth USD 59,188.98** and costs nothing: both pilots run two clinics
   at the same USD 45,000. Perfect free information would cost `0.30 × 130,000 =` **USD 39,000**, so
   the value of perfect information is **USD 130,704** and the designed pilot captures **59.02 %** of
   it; the random pilot captures **13.73 %**.
5. **Interpretation.** The finding is not that pilots are valuable — everyone believes that — but
   that **the value of an imperfect signal is set by its design, and a badly designed pilot is
   roughly a quarter as valuable as a well designed one at identical cost.**

   **Where the loss comes from.** A random pair of clinics misses every legacy-index site nearly half
   the time (**48.4615 %**), and in those futures the programme pays the full 565,680 *plus* the
   45,000 it spent finding nothing. A pilot chosen to include the configuration under suspicion
   cannot miss. Nothing about the money changed; only the question the pilot was capable of answering.
   This is the same lesson Domain 9 (KA 9.3.2) states for acceptance sampling — a convenience sample
   of the accessible items establishes nothing — arriving here through the value of information
   rather than through a confidence bound, and it is worth recognising as one idea in two instruments.

   **The breakevens diverge sharply, which is the decision rule.** The designed pilot pays while the
   prior exceeds `45,000/(565,680 − 158,560) =` **11.0533 %**; the random pilot needs
   `45,000/209,823.38 =` **21.4466 %** — a prior **1.9403 times** higher. So there is a whole band of
   beliefs, from 11.05 % to 21.45 %, in which **the right decision is to pilot and the wrong decision
   is to pilot badly**: a programme that runs the random pilot in that band destroys value while
   appearing prudent. The professional habit that follows is to state, of any proposed investigation,
   *which uncertainty it can resolve and with what probability* — not merely what it costs.

   **Two cautions and one governance point.** The arithmetic treats the flaw as a single binary
   condition; if the approach could fail *partially*, or fail on a configuration nobody has thought
   of, the pilot's real detection probability is lower than 1 even when designed, and the honest
   figure is a range. The 2-week and 6-week delays are themselves estimates, and because the cost of
   delay is a rate, an error in the *weeks* propagates linearly into the answer: one week either way
   on the late case moves the no-pilot option by `0.30 × 14,280 =` USD 4,284. And deliberately
   selecting the pilot sites has a stakeholder consequence that no expected value captures — the
   clinics chosen are those most likely to have a bad experience, which is a Domain 11 conversation
   that has to be held before the arithmetic is acted on, not after.

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
   what confidence it buys. Five things belong beside it, and the first two are checks a reviewer
   can run in five minutes.

   **What confidence does the rule of thumb actually buy?** The "10 % of `BAC`" convention gives
   400,000, which on this distribution sits at `z = (400,000 − 278,000)/252,642 =` **0.4829**, i.e.
   the **68.54th** percentile. So the rule of thumb is not "conservative" or "prudent"; it is a
   **P68.5** reserve, and it happens to be one on this register only by coincidence — on a register
   with the same mean and half the σ it would be a **P83** reserve. The convention's real defect is not
   that it is wrong but that it **conceals which question it is answering**, and the two-line
   calculation above converts it into a number that can be argued with.

   **Check the approximation against the exact distribution, because with five risks you can.** Five
   independent binary risks have `2⁵ = 32` outcomes; enumerating them gives the aggregate exactly
   with no normal assumption at all. Doing so shows that **USD 490,624 in fact covers 78.60 %** of
   futures, not 80.00 % — the approximation overstates the confidence by **1.40 percentage points**,
   which is small enough to use and large enough to disclose. The exact distribution also shows why
   a percentile is a slippery object on a lumpy aggregate: the outcomes cluster at 32 discrete
   levels, and the smallest level whose cumulative probability reaches 0.80 is **USD 500,000**, at
   which point the cumulative probability has already jumped to **83.43 %**. There is no outcome at
   all whose probability of non-exceedance is exactly 80 %. The professional habit: quote the
   confidence level as an approximation with its method named, and never defend the fourth digit of
   a percentile on a register of five risks. (The rule of thumb fares similarly — 400,000 covers
   **68.58 %** exactly against the approximation's 68.54 %.)

   **The price of confidence is computable, and it is the conversation governance should be having.**
   The same register gives P50 = the mean = **USD 278,000** (6.95 % of `BAC`), P80 =
   **USD 490,624** (**12.27 %**), and P90 = `278,000 + 1.2816 × 252,642 =` **USD 601,786**
   (**15.04 %**). Moving from P80 to P90 therefore costs **USD 111,163**, or about
   **USD 11,116 per percentage point** of confidence. That is the number a sponsor needs in order to
   choose, and it is why "the confidence level is a policy choice" is a statement about a priced
   trade rather than a disclaimer.

   **Independence is an assumption, usually optimistic** — if R1 and R3 share a supplier they are
   correlated, the variance is larger, and the true P80 is higher; correlation is what turns a bad
   month into a crisis. Worked example 8.2.4b prices it on this register rather than leaving it as a
   warning.

   **And the normal approximation is a convenience** for a handful of Bernoulli risks; proper
   practice runs a Monte Carlo simulation over the register (and over the schedule, extending
   Domain 6's three-point durations) to produce a distribution rather than a formula. Note what the
   32-outcome enumeration establishes about that: the approximation's error here is a point and a
   half, so simulation on a five-risk register buys presentation rather than accuracy. Its value
   appears when the register is large, when impacts are themselves distributions rather than points,
   or when correlations must be modelled — and the enumeration above is the check that a simulation
   is doing what its owner thinks, because both must return the same mean of 278,000.

**Worked example 8.2.4b — what one shared subcontractor does to Auriga's reserve.**

1. **Setup.** Auriga's register is re-examined and one fact emerges from the procurement schedule:
   **R1** (controller lead time), **R3** (integration rework) and **R4** (permit delay, whose
   application package the same firm prepares) all depend on the same specialist subcontractor. The
   risk team assesses a pairwise correlation of **ρ = 0.5** among the three — a judgment, and one that
   must be recorded as such. R2 and R5 remain independent of everything.
2. **Formula.** For correlated risks, `Var(ΣX) = Σ Var(Xᵢ) + 2 Σᵢ<ⱼ ρᵢⱼ σᵢ σⱼ`. The mean is
   unchanged: `E(ΣX) = Σ E(Xᵢ)` whatever the dependence. Per-risk `σᵢ = √(pᵢ(1−pᵢ)) × impactᵢ`.
3. **Substitution.** Per-risk σ: R1 **114,472.70**, R2 **90,000.00**, R3 **138,564.06**,
   R4 **142,828.57**, R5 **54,990.91**. Correlation term
   `2 × 0.5 × (σ₁σ₃ + σ₁σ₄ + σ₃σ₄)` = `15,861,803,176.18 + 16,349,972,477.04 + 19,790,907,002.96`.
4. **Result.** Added variance **52,002,682,656**, taking total variance from 63,828,000,000 to
   **115,830,682,656** and σ from 252,642 to **USD 340,339.07** — a factor of **1.3471**. The
   correlated P80 is `278,000 + 0.8416 × 340,339.07 =` **USD 564,429.36**, **USD 73,805.82** (or
   **15.04 %**) above the independence figure, and **14.11 %** of `BAC` against 12.27 %. And the
   reserve already approved at 490,624 now covers `z = 0.6247` of the correlated distribution — a
   **73.39 %** reserve.
5. **Interpretation.** Three results, and the third is the one to take to a board.

   **The mean did not move.** Total `EMV` is still exactly 278,000, because correlation redistributes
   probability without changing an average. This is why a register can pass every `EMV` check,
   reconcile perfectly against a simulation's mean, and still be under-reserved: **the error lives
   entirely in the second moment, where no `EMV` review looks.** It is also the reason Σ`EMV`
   remains a valid hand-check on a simulation even when correlation is modelled.

   **The correlation term is large because it scales with σ products, not with `EMV`s.** Three
   risks — 60 % of the register by count — added 81.5 % as much variance again as all five risks
   contributed independently. The general form of that, and why concentration hurts faster than
   counting suggests, is 8.A.1's. A programme with twenty risks on one scarce resource is not twenty
   risks.

   **And the label fails before the money does.** Nobody wrote down a wrong reserve; they wrote down
   a reserve that was P80 under an assumption, and the assumption was never tested. The board approved
   "an 80 % confidence contingency" and holds a **73.4 %** one — a 6.6-point shortfall in confidence
   that appears nowhere in the financial statements and can only be found by asking which risks share
   a driver. Restoring 80 % costs the **USD 73,806** computed above; that is the priced version of
   8.A.1's warning and the exact question the Executive perspective says a leader must not delegate.

   One caution on the arithmetic itself. **ρ = 0.5 is a judgment, and the answer is sensitive to
   it**: the correlation term scales linearly in ρ, so halving the assumed correlation to 0.25 halves
   the added variance and the honest presentation is a range with the assumption named, not a single
   figure. Where the shared driver is identifiable at all, 8.A.1 argues for a different model
   altogether, and Case study B computes what that model does to the answer.

> **Fig 8.2.2 — Independence is worth 6.6 points of confidence.** Two normal density curves over
> aggregate risk outcome in USD, both centred on the mean of 278,000 (marked with a note that
> correlation does not move it). The solid brand-blue curve is the independent aggregate,
> σ 252,642; the dashed crimson curve is the same register with ρ = 0.5 across R1, R3 and R4,
> σ 340,339. A vertical ink rule at 490,624 is annotated "reserve 490,624 — P80 if independent,
> P73.4 with one shared driver", with the area to its left under the correlated curve lightly
> shaded. Source: PCI original. Alt text: two bell curves sharing a mean, the wider correlated one
> leaving more probability beyond the reserve line than the narrower independent one.

**Worked example 8.2.4c — the response that buys variance, on Meridian's register.**

1. **Setup.** Meridian's five quantified risks (8.2.2b) give a mean of **USD 119,000**, a variance
   of **10,149,000,000** and σ of **USD 100,742.25**, so the P90 the board's appetite implies (8.3.2)
   is `119,000 + 1.2816 × 100,742.25 =` **USD 248,111.26**. Two responses are proposed, each costing
   **USD 25,000**. **Response A** halves the probability of M2 from 0.35 to 0.175 by seconding
   clinical backfill so that parallel running is less likely to extend. **Response B** cuts the
   impact of M3 from USD 150,000 to USD 60,000 by building the interface behind an abstraction layer,
   so a records-format revision is a configuration change rather than a rebuild; the probability
   stays 0.20 because the national timetable is outside the programme's control.
2. **Formula.** `EMV` contribution `= p × I`; variance contribution `= p(1−p)I²`; recompute mean,
   variance, σ and P90 after each response and compare the **P90 reduction per USD spent**.
3. **Substitution.** A: `EMV` 42,000 → `0.175 × 120,000 = 21,000`; variance 3,276,000,000 →
   `0.175 × 0.825 × 120,000² = 2,079,000,000`. B: `EMV` 30,000 → `0.20 × 60,000 = 12,000`; variance
   3,600,000,000 → `0.20 × 0.80 × 60,000² = 576,000,000`.
4. **Result.**

   | | `EMV` removed | Variance removed | New mean | New σ | New P90 | P90 reduction |
   |---|---|---|---|---|---|---|
   | **A** halve M2's probability | **21,000** | 1,197,000,000 | 98,000 | 94,615.01 | **219,258.60** | **28,852.67** |
   | **B** cut M3's impact | 18,000 | **3,024,000,000** | 101,000 | 84,409.72 | **209,179.49** | **38,931.77** |

   A removes **USD 3,000 more `EMV`**; B removes **USD 10,079.11 more of the P90 requirement** — a
   reduction **34.93 %** larger, for the same USD 25,000. Per dollar spent, A returns **1.1541** of
   P90 reduction and B returns **1.5573**.
5. **Interpretation.** The two rankings disagree, and which one is right depends on what the
   organisation is short of.

   **If the binding constraint is the reserve, buy variance; if it is the expected cost, buy the
   mean.** Meridian's constraint is the tolerance (8.2.2b), so B is the correct purchase even though
   it looks worse on the metric most registers publish. A programme whose problem is a thin margin
   rather than a thin contingency would choose A. The professional error is not choosing wrongly but
   **choosing without stating which constraint is binding** — at which point the `EMV` column decides
   by default, and the default is wrong whenever a confidence level is the thing under pressure.

   **The algebra says why, and it generalises.** `EMV` is linear in both `p` and `I`, so halving
   either halves the `EMV` identically. Variance goes as `p(1−p)I²`, which is **quadratic in impact
   and non-monotonic in probability**. Halving an impact therefore removes exactly **75 %** of that
   risk's variance contribution, always. Halving a probability leaves `(1 − p/2)/(2(1 − p))` of it —
   0.5278 at `p` = 0.10, 0.6346 at `p` = 0.35, 0.75 at `p` = 0.50 — so it removes between 25 % and
   50 %. Dividing gives the invariant: **for any risk with `p ≤ 0.5`, halving the impact removes
   between 1.5 and 3 times as much variance as halving the probability, at identical `EMV` cost**
   (1.5882 at `p` = 0.10, 1.7143 at 0.20, 2.0526 at 0.35, exactly 3 at 0.50). Impact reduction is
   the reserve lever; probability reduction is the margin lever. This is also the arithmetic behind
   the intuition that "cap the downside" beats "make it less likely" — and it is the reason
   **modularity and staged commitment (8.4.1) release contingency**, since both are impact levers.

   **Two cautions.** Impact reduction is frequently harder to engineer than probability reduction and
   sometimes impossible — an abstraction layer exists to be built, but no design change makes a
   regulator less likely to publish. The invariant tells a leader where to *look* first, not what is
   available. And these responses were compared at equal cost precisely so that the levers could be
   isolated; in practice each option's own cost, secondary risks and schedule effect enter as well,
   which is the fuller comparison worked example 8.3.1 runs.

> **Fig 8.2.3 — Expected value and variance rank the same register differently.** Paired vertical
> bars for Meridian's five quantified risks M1–M5. The brand-blue bar is each risk's expected value
> (absolute), the crimson bar its σ contribution `√(p(1−p)) × impact`, both in USD on one axis.
> M3 carries the middle blue bar — third of five by height (30,000; **25.21 %** of total `EMV`) — and the tallest
> crimson bar (60,000; **35.47 %** of total variance), annotated "M3: 3rd by EMV, 1st by variance".
> M5, the opportunity, is shown at |`EMV`| 10,000 in a lighter tone because variance is unsigned.
> Source: PCI original. Alt text: a paired bar chart of five risks in which the third-largest
> expected value carries the largest variance contribution.

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
| **Within-cell span** | The `EMV` range a single matrix cell contains: the product of its two band ratios. |
| **Quantification threshold** | The impact above which a screened risk is quantified individually; a screen without one has replaced quantification. |
| **Variance contribution** | `p(1−p)I²` for one risk; quadratic in impact, so it ranks a register differently from `EMV`. |
| **Value of perfect information** | The most any signal could be worth: expected cost without information less expected cost knowing the truth free. |
| **Price of confidence** | The cost per percentage point of moving a reserve between confidence levels. |

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

**MCQ 8.2-F `[8.2.1 · Application]`** A matrix's medium probability band runs 0.10–0.35 and its
medium impact band USD 60,000–250,000. The span of expected values inside that single cell is a
factor of:
- A. 3.5
- B. 14.58 ✅
- C. 4.17
- D. 2.5

*Rationale:* Within-cell span is the product of the two band ratios, `3.5 × 4.1667 = 14.5833`
(8.2.1). A gives the probability ratio alone and C the impact ratio alone — each answers half the
question; D subtracts one from the probability band ratio (3.5 − 1), the slip that reads a ratio as
an increment.

**MCQ 8.2-G `[8.2.4 · Analysis]`** A register's contingency was set at a P80 of USD 490,624 on an
independence assumption. Three of its risks are then found to share a subcontractor, raising σ from
252,642 to 340,339. The reserve is now:
- A. still a P80 reserve, because the mean has not changed
- B. a 73.4 % reserve — correlation widens the distribution without moving the mean, so the same amount covers fewer futures ✅
- C. inadequate by USD 73,806 of expected cost
- D. unaffected, since correlation is a modelling choice rather than a fact

*Rationale:* `z = (490,624 − 278,000)/340,339 = 0.6247`, i.e. **73.4 %** (8.2.4b). A confuses the
mean's invariance with the percentile's; C misreads the USD 73,806 — it is the extra reserve needed to
restore 80 % confidence, not an increase in expected cost, which is unchanged at 278,000; D treats a
shared supplier as an opinion.

**MCQ 8.2-H `[8.2.3 · Analysis]`** Twelve of 40 clinics run the configuration under suspicion. A
two-clinic pilot chosen at random detects the problem with probability 0.5154; a two-clinic pilot
chosen to include a suspect configuration detects it with certainty. Both cost USD 45,000. The right
conclusion is:
- A. the pilots are equivalent, since the cost and sample size are identical
- B. the design choice is worth USD 59,189 at no additional cost, because the value of information depends on what the sample is capable of detecting ✅
- C. the random pilot is preferable because it is unbiased
- D. neither pilot is worthwhile, since 45,000 exceeds the redesign saving

*Rationale:* Expected costs are 151,756.98 random against 92,568.00 designed (8.2.3b). A prices
inputs rather than the information bought; C applies a sampling virtue that is irrelevant when a
specific condition is being tested and Domain 9 (KA 9.3.2) warns about in its own instrument; D
compares the pilot price with the wrong quantity — the comparison is against the 565,680 late case,
and the designed pilot pays above an 11.05 % prior.

**MCQ 8.2-I `[8.2.4 · Application]`** Two responses cost the same and each removes the same
proportion of a risk. Response A halves the probability; Response B halves the impact. Compared on
the reserve they release:
- A. they are equivalent, since both halve the `EMV`
- B. B releases more, because variance goes as impact squared — halving an impact removes 75 % of that risk's variance while halving a probability removes between 25 % and 50 % ✅
- C. A releases more, because probability drives whether the event happens at all
- D. neither releases reserve, since contingency is set from `EMV`

*Rationale:* `EMV` is linear in both, so A's premise is right and its conclusion wrong; variance is
`p(1−p)I²` (8.2.4c). C reverses the result; D confuses a mean-based reserve with a
confidence-based one, which is the error MCQ 8.2-D addresses.

### Self-check — KA 8.2

1. *Why is `EMV` wrong for judging whether one risk is survivable?* — It averages outcomes that
   will not occur; the actual event is impact-or-nothing.
2. *When is information worth nothing however reassuring?* — When it would not change the action
   taken.
3. *Which assumption in the P80 calculation is usually optimistic, and why does it matter?* —
   Independence; correlated risks raise variance and the true confidence amount.
4. *What determines the span of expected values inside one matrix cell?* — The product of its
   probability and impact band ratios; nothing else.
5. *What does correlation change and what does it leave alone?* — It widens the distribution and
   raises every percentile; the mean is unchanged, which is why `EMV` reviews cannot detect it.
6. *Which lever should a leader short of contingency pull, and why?* — Impact, because variance is
   quadratic in impact: at equal `EMV` reduction it removes 1.5 to 3 times as much variance.
7. *Why express total exposure against a tolerance rather than against the budget?* — The tolerance
   is the room governance controls; Meridian's 119,000 is 4.96 % of approved cost and 99.17 % of the
   tolerance.

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
whose responses have no secondary entries has not been thought through. That is usually asserted as a
completeness rule; it is in fact a selection rule, because the secondary risks can reverse which
response is cheapest.

**Worked example 8.3.1 — four responses to Auriga's R1, priced against each other.**

1. **Setup.** R1 — controller lead-time slip — carries `p` 0.35 and an impact of **USD 240,000**, so
   its `EMV` is **USD 84,000** and its impact is **6.00 %** of `BAC` (survivable, so the `EMV` logic
   below is legitimate; see the caution in step 5). Four options are costed:

   - **Accept**, with the slip absorbed by contingency.
   - **Reduce** — dual-qualify a second controller supplier for **USD 22,000**, cutting `p` to 0.15.
     Secondary risk: the alternate's first article fails inspection, `p` 0.10, impact USD 40,000.
   - **Transfer** — a delivery-guarantee term in the supply contract priced at **USD 18,000** that
     recovers **60 %** of the impact. Secondary risk: the supplier is a single-site specialist whose
     ability to pay is impaired by the very disruption that triggers the claim, assessed at
     **0.25** conditional on the event.
   - **Avoid** — redesign to a commodity controller for **USD 95,000**, removing R1 entirely.
     Secondary risk: the redesign introduces integration rework, `p` 0.20, impact USD 120,000.
2. **Formula.** Total expected cost of an option = response cost + residual `EMV` + Σ secondary
   `EMV`. Choose the minimum. For the transfer, residual `EMV` = `p × impact × (1 − recovery)` and
   the counterparty secondary = `p × P(cannot pay) × recovered amount`.
3. **Substitution.** Reduce `22,000 + 0.15 × 240,000 + 0.10 × 40,000`. Transfer
   `18,000 + 0.35 × 240,000 × 0.40 + 0.35 × 0.25 × (0.60 × 240,000)`. Avoid
   `95,000 + 0 + 0.20 × 120,000`.
4. **Result.**

   | Option | Response cost | Residual `EMV` | Secondary `EMV` | **Total** | Total ignoring secondaries |
   |---|---|---|---|---|---|
   | Accept | 0 | 84,000 | 0 | **84,000** | 84,000 |
   | **Reduce** | 22,000 | 36,000 | 4,000 | **62,000** | 58,000 |
   | Transfer | 18,000 | 33,600 | 12,600 | **64,200** | 51,600 |
   | Avoid | 95,000 | 0 | 24,000 | **119,000** | 95,000 |

   With secondary risks priced, **reduce wins at USD 62,000**. Ignoring them, transfer appears best
   at 51,600 — better than reduce by 6,400 — when in truth it is worse by 2,200: a **swing of
   USD 8,600** in the comparison, and a change of decision.
5. **Interpretation.** Four conclusions, and the first is the reason this example exists.

   **Omitting secondary risks does not make a response look slightly better; it changes which
   response is chosen.** The transfer's secondary is large precisely because it is *correlated with
   the event it covers* — the supplier's insolvency and the delivery failure have the same cause. That
   is the general case rather than an exotic one, and it is why "transfer is a price, not a
   disappearance" (Domain 7, KA 7.4.2; Domain 10, KA 10.3) needs a number beside it. The breakeven is
   checkable: the transfer ties the reduce option when the counterparty's probability of being unable
   to pay reaches `(62,000 − 18,000 − 33,600)/(0.35 × 144,000) =` **20.63 %**. Below that, transfer;
   above it, dual-qualify. A leader who cannot form a view on that one number has no basis for
   choosing between the two, and asking for it is a better use of a review than re-reading the
   register. One pointer, because this is an enforceability-sensitive area: whether a guarantee or
   indemnity is enforceable, what it actually reaches, and where the claim ranks if the counterparty
   fails are legal questions that turn on the wording and on the jurisdiction. Take the 60 % recovery
   this table multiplies from qualified counsel (Domain 10, KA 10.3), not from the register.

   **Avoidance is not the safe answer.** It is the most expensive option here by a wide margin —
   **USD 35,000 worse than doing nothing** — and it does not even eliminate exposure, because the
   redesign carries 24,000 of secondary `EMV` of its own. Avoidance moved the risk from procurement
   to engineering and paid 95,000 for the move. The instinct that the strongest-sounding response
   family is the most responsible one is the commonest failure in response selection, and the
   arithmetic is the only reliable corrective.

   **Acceptance is a priced position, not a default.** Reduce creates **USD 22,000** of value against
   accept — a **26.19 %** improvement on the 84,000 — so acceptance is wrong here. But it becomes
   right the moment the reduce option's cost rises above 44,000 (`84,000 − 36,000 − 4,000`), which a
   single change in the alternate supplier's qualification scope could do. Acceptance and response are
   the two ends of one calculation, and the register should record the cost at which the answer flips,
   not just the answer.

   **Two boundaries on all of it.** First, this comparison is on **expected cost only**. Worked
   example 8.2.4c's result applies here too: the reduce option is a probability lever and the transfer
   is effectively an impact lever, so if the binding constraint were the confidence level rather than
   the expected cost, the ranking would need recomputing on variance — dual-qualifying leaves
   `0.15 × 0.85 × 240,000² = 7,344,000,000` of variance where the transfer leaves
   `0.35 × 0.65 × 96,000² = 2,096,640,000`. Second, and overriding: **if R1's impact were
   existential rather than 6 % of `BAC`, the avoid option's 119,000 would be cheap** and the whole
   table would be the wrong instrument. That is the next rule, and it is not a refinement of this one
   but a limit on it.

### 8.3.2 Reserves and their authority

Domain 7 (KA 7.1.3) established the structure; this domain supplies the sizing and the governance:

| Reserve | Covers | Sized by | Spent by |
|---|---|---|---|
| **Contingency** | Identified risks in the register | Aggregation to a stated confidence (8.2.4) | Project manager, under a published protocol |
| **Management reserve** | Unknown-unknowns and scope change | Judgment and organisational policy | Sponsor / change authority, via change control |

Three governance rules. **The draw protocol is published in advance** — which risk, what evidence,
what approval — because a reserve released ad hoc is indistinguishable from an overrun. The protocol
also names who may not approve: a draw approved by the person whose overspend it covers is not a
controlled reserve whatever the paperwork says, so the approver's interest in the draw is declared
before the approval and an interested approver stands aside, with the record naming who approved
instead (Domain 1, KA 1.2.2a). This is the commonest live conflict in cost control, because the
person with the best information about the draw is usually the person it relieves.
**Consumption is trended against risk retirement**, not against time: burning 60 % of contingency
while 20 % of the register has been retired is the signal MCQ 7.1-B describes. And **contingency
released by retired risks is returned, not reallocated** to convenient overspends; otherwise the
reserve silently becomes a slush fund and the next real risk is unfunded.

Two questions remain, and both are arithmetic rather than procedural: *at what confidence should the
reserve be held*, and *is the reserve still adequate now that some of it has been spent*. The first
is answered from the appetite, not from convention.

**Worked example 8.3.2 — deriving Meridian's confidence level from its appetite.**

1. **Setup.** Meridian's board has expressed its appetite in one sentence: it will accept **no more
   than a 10 % chance** that the programme's outturn cost exceeds the approved **USD 2,400,000** by
   more than **5 %**. The quantified register (8.2.2b) has a mean of **USD 119,000**, σ of
   **USD 100,742.25**. No confidence level has yet been chosen for contingency.
2. **Formula.** Tolerance `= 5 % × approved cost`. "No more than a 10 % chance of exceeding"
   is the definition of a **P90** requirement, so the appetite requires
   `mean + 1.2816 σ ≤ tolerance`. Rearranged, the maximum σ the appetite permits at a given mean is
   `σ_max = (tolerance − mean)/1.2816`.
3. **Substitution.** Tolerance `0.05 × 2,400,000`. Required P90 `119,000 + 1.2816 × 100,742.25`.
   Permitted σ `(120,000 − 119,000)/1.2816`.
4. **Result.** Tolerance **USD 120,000**. The register's P90 is **USD 248,111.26** — **2.0676 times**
   the tolerance, and **10.34 %** of approved cost against an appetite of 5 %. The σ the appetite
   permits at the current mean is **USD 780.27**, against an actual σ of 100,742.25: the appetite is
   not merely breached but **unreachable by any contingency decision**.
5. **Interpretation.** Three findings, each of which is a different conversation.

   **The confidence level was never a choice.** The board said "10 % chance of exceeding", which *is*
   P90; nobody needed to prefer P80 or P90, and a programme that holds contingency at P80 while its
   board has stated a 10 % tolerance is inconsistent with its own governance. This is the general
   result: **a well-formed appetite statement determines the confidence level.** The corollary is more
   useful in practice — where a project has inherited "P80 by convention", the honest reconstruction
   is to ask what appetite statement P80 implies (a 20 % acceptance of exceedance) and put *that*
   sentence in front of the sponsor. Most sponsors have never seen it written down, and some do not
   agree with it.

   **The appetite constrains σ, not just the mean, and that is where it becomes unreachable.** With
   the mean at 119,000 the tolerance leaves USD 1,000 of room, so the permitted σ is 780 — a register
   with essentially no uncertainty at all. No reserve fixes this, because contingency is money held
   *inside* the tolerance; the tolerance is the room. The three real options are the ones 8.2.2b
   named, and the arithmetic now sizes them: reduce the exposure (responses, per 8.2.4c and 8.3.1),
   widen the tolerance (a governance decision the board must take knowingly), or reduce scope. The
   professional obligation is to present the choice, not to select the confidence level that makes the
   paper pass.

   **And the gap is the number that makes the case.** Restoring the appetite requires taking the P90
   from 248,111 to 120,000 — a reduction of **USD 128,111** — which on the evidence of 8.2.4c is
   bought mainly by impact levers. Response B there removed USD 38,932 of P90 for USD 25,000, so the
   order of magnitude is three to four such responses, and that is a fundable programme of work rather
   than an argument. Two cautions. The 1.2816 factor is the normal approximation again, and on a
   five-risk register it carries the sort of error 8.2.4 measured, so the target should be expressed
   as a range. And an appetite expressed against *cost* says nothing about schedule, benefit or
   reputation; each objective needs its own statement, and a board that has expressed only one has
   expressed less than it thinks (Domain 15, KA 15.4).

**Worked example 8.3.2b — is Auriga's reserve still adequate at week 13?**

1. **Setup.** Auriga's contingency was set at the P80 of **USD 490,624** (8.2.4). At the week-13
   review — `PV` 2,080,000, `EV` 1,920,000, `AC` 2,120,000, `CPI` 0.91, `SPI` 0.92 (Domain 7,
   KA 7.3) — three things have happened. **R2** (ground conditions) has **occurred** and is now an
   issue. **USD 232,000** of contingency has been drawn under the published protocol:
   **USD 195,000** against R2, whose outturn exceeded its 180,000 assessment, and **USD 37,000**
   between two register entries that sat below the quantification threshold of 8.2.1 and occurred.
   **R4**
   (permit delay) has been **retired**: the consenting window closed without incident. **R1**, **R3**
   and **R5** remain open at unchanged assessments. Is the remaining reserve still a P80 reserve?
2. **Formula.** Requirement = P80 of the **open** register (mean + 0.8416 σ, recomputed from the open
   entries only). Remaining reserve = original reserve − drawn. Adequacy ratio = remaining ÷
   requirement. Implied confidence = `Φ((remaining − open mean)/open σ)`. Release from a retired risk
   = original P80 − P80 recomputed without it.
3. **Substitution.** Open register: R1 `0.35 × 240,000`, R3 `0.25 × 320,000`, R5
   `0.30 × (120,000)`; variance `13,104,000,000 + 19,200,000,000 + 3,024,000,000`. Remaining
   `490,624 − 232,000`.
4. **Result.** Open mean **USD 128,000**, open variance **35,328,000,000**, open σ
   **USD 187,957.44**, so the P80 **requirement is USD 286,184.98**. Remaining reserve
   **USD 258,623.54** — **47.29 %** of the reserve has been drawn. Adequacy ratio
   **0.9037**, a **shortfall of USD 27,561.44**, and the remaining reserve in fact covers
   `z = 0.6950` of the open distribution: a **75.65 %** reserve, not an 80 % one.

   Separately, retiring R4 releases more than its `EMV`. The register without R4 has mean
   **218,000**, σ **208,393.86** and P80 **USD 393,384.27**, so R4's retirement releases
   `490,624 − 393,384 =` **USD 97,239.27** against an `EMV` of only 60,000 — a ratio of **1.6207**.
5. **Interpretation.** Two results, and together they replace the usual reserve test with a better
   one.

   **The naive test passes and the real test fails.** On the conventional comparison, 47.29 % of the
   reserve is drawn while R4's retirement alone accounts for **21.58 %** of the original `EMV` and
   R2's occurrence for a further 32.37 % — drawdown and register progress look broadly in step, and a
   status report saying so would not be challenged. Recomputing the requirement from the open register
   shows the reserve is **90.37 %** of what its own policy requires and is buying **75.65 %**
   confidence rather than 80 %. The discipline that follows: **a part-consumed reserve is tested
   against the register still open, at the stated confidence, not against elapsed time or against the
   original register.** The number to report each period is the adequacy ratio, and the trigger — a
   ratio below 1.00 — is a request to top up from management reserve or to reduce exposure, taken
   deliberately while USD 27,561 is a small decision.

   **Retiring a low-probability, high-impact risk releases far more than its `EMV`.** R4 carried
   32.0 % of the original variance on 21.6 % of the `EMV`, because variance is quadratic in impact
   (8.2.4c). A leader who returns only the retired risk's `EMV` of 60,000 leaves **USD 37,239**
   sitting in the reserve doing nothing — money that the "return, do not reallocate" rule above was
   never meant to trap. The corollary is uncomfortable and worth stating: **an unretired risk whose
   window has quietly passed is expensive**, because it holds variance-weighted reserve against an
   exposure that no longer exists. Register hygiene is a funding activity, which is not how it is
   usually presented.

   **Two cautions.** Recomputing the requirement each period tempts a project into re-deriving its
   contingency downwards whenever the arithmetic is convenient; the protocol should fix the
   *confidence level* and the *method* in advance, so that only the register changes. And the whole
   calculation still assumes independence among R1, R3 and R5 — on 8.2.4b's finding, the true
   requirement is higher and the adequacy ratio worse, which is the first question a reviewer should
   ask of this table rather than the last.

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
| **Risk retirement** | Closing a risk whose window has passed; frees contingency for return — often far more than the risk's `EMV`. |
| **Early-warning indicator** | A leading measure tied to a risk, firing before the event. |
| **Tolerance** | The amount by which an objective may be exceeded before appetite is breached; the room inside which contingency sits. |
| **Adequacy ratio** | Remaining reserve ÷ the confidence-level requirement of the register still open; below 1.00 is a governance trigger. |
| **Counterparty risk (priced)** | The secondary `EMV` of a transfer: `p × P(cannot pay) × amount recovered`, usually correlated with the event transferred. |

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

**MCQ 8.3-D `[8.3.1 · Analysis]`** On one risk, transferring appears cheaper than reducing —
USD 51,600 against 58,000 — until secondary risks are priced, after which reducing is cheaper at
62,000 against 64,200. The correct reading is:
- A. the second calculation is double-counting, since the transfer already covers the risk
- B. secondary risks are a selection rule, not only a completeness rule: here the transfer's counterparty exposure is correlated with the very event transferred, and pricing it changes the decision ✅
- C. the difference is immaterial at USD 2,200 and either option may be taken
- D. reducing is always preferable to transferring

*Rationale:* The transfer's secondary `EMV` of 12,600 arises because the supplier's ability to pay is
impaired by the disruption that triggers the claim (8.3.1). A misses that the counterparty may not
perform; C ignores that the 2,200 is a *reversal* of a 6,400 apparent advantage and that the breakeven
default probability, 20.63 %, is the thing to form a view on; D generalises one case into a rule.

**MCQ 8.3-E `[8.3.2 · Application]`** A risk with `p` 0.15 and impact USD 400,000 is retired when its
window closes. Its `EMV` was USD 60,000, and recomputing the register's P80 without it falls by
USD 97,239. The amount to return to the sponsor is:
- A. USD 60,000 — the risk's `EMV`
- B. USD 97,239, because the reserve was sized on a confidence level and the retired risk carried 31.96 % of the variance on 21.58 % of the `EMV` ✅
- C. nil, since contingency stays inside the baseline
- D. USD 37,239 — the difference between the two figures

*Rationale:* A confidence-based reserve releases what recomputing it releases; variance is quadratic
in impact, so a low-probability high-impact risk carries disproportionate reserve (8.3.2b). A returns
the mean-based amount and leaves USD 37,239 idle; D returns only that idle remainder; C confuses
where contingency sits with whether it may be released.

**MCQ 8.3-F `[8.3.2 · Analysis]`** A board states it will accept no more than a 10 % chance of
exceeding approved cost by more than 5 %. The confidence level for contingency is therefore:
- A. P80, the industry convention
- B. P90, because "no more than a 10 % chance of exceeding" is the definition of the 90th percentile, and the tolerance is 5 % of approved cost ✅
- C. P95, to be prudent
- D. undetermined — appetite statements are qualitative

*Rationale:* The appetite determines the confidence level (8.3.2); the leader's job is to derive it,
not to select it. A inherits a convention that implies a 20 % acceptance of exceedance the board did
not state; C substitutes caution for the board's decision; D is the position the arithmetic refutes.

### Self-check — KA 8.3

1. *When is acceptance the professionally correct response?* — When the response's cost exceeds
   the `EMV` reduction it buys — and the risk is survivable.
2. *What does transfer actually achieve?* — It prices the risk to a counterparty and creates
   counterparty risk; it does not remove the exposure.
3. *What should contingency consumption be trended against?* — Risk retirement, not elapsed time.
4. *State the adequacy test for a part-consumed reserve.* — Remaining reserve divided by the stated
   confidence level's requirement recomputed on the register still open; below 1.00 is a trigger.
5. *Where does an appetite statement's confidence level come from?* — The statement itself: "no more
   than a 10 % chance of exceeding" is P90.
6. *Why can avoidance be the most expensive response?* — It is paid in full whether or not the event
   would have occurred, and it creates its own secondary risks — Auriga's avoid option costs
   USD 35,000 more than accepting.

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

That judgment is usually left qualitative, and it need not be. A resilience measure has a price, a
probability of being needed and a recovery time it shortens, which is enough to price it twice: once
on expected value and once on the tail. The two answers frequently disagree, and knowing what to do
when they do is the substance of this topic.

**Worked example 8.4.1 — Meridian's standby trainer retainer, on the average and on the tail.**

1. **Setup.** Meridian's rollout depends on a single training provider. If that provider's capacity
   fails — attrition, a competing contract — clinics cannot go live, and the programme's cost of delay
   is **USD 14,280 per week**. The probability of a capacity failure over the rollout is assessed at
   **0.25**. Recovering unaided means re-tendering: **7 weeks**. A **USD 18,000** annual retainer with
   a second, pre-qualified provider reduces recovery to **2 weeks**. The programme separately notes the
   tail: unaided, the 95th-percentile recovery is **12 weeks**, which would miss the winter demand
   window and defer the remaining **40 %** of the benefit by a full **13-week** quarter; with the
   retainer the 95th-percentile recovery is **3 weeks** and the window holds.
2. **Formula.** Expected delay cost = `P(failure) × recovery weeks × cost of delay`; add the retainer
   where taken. Breakeven probability = retainer ÷ (weeks saved × cost of delay). Tail cost =
   95th-percentile weeks × cost of delay, plus any window-miss deferral.
3. **Substitution.** Without: `0.25 × 7 × 14,280`. With: `18,000 + 0.25 × 2 × 14,280`. Breakeven
   `18,000/(5 × 14,280)`. Tail deferral `0.40 × 14,280 × 13`.
4. **Result.** Expected cost **without** the retainer **USD 24,990.00**; **with** it
   **USD 25,140.00** — the retainer is a **net loss of USD 150.00**. It breaks even at a failure
   probability of `18,000/71,400 =` **25.2101 %**, against the assessed 25.00 %: it misses by
   **0.2101 of a percentage point**.

   On the tail the picture inverts. Without the retainer the 95th-percentile outcome costs
   `12 × 14,280 =` **USD 171,360** of delay **plus USD 74,256** of deferred benefit —
   **USD 245,616**. With it, `18,000 + 3 × 14,280 =` **USD 60,840**. The retainer removes
   **USD 184,776** of tail exposure and reduces it by a factor of **4.0371**.
5. **Interpretation.** The two calculations disagree, and the professional content of this example is
   what a leader does about that.

   **When the breakeven sits inside the estimating error, expected value has stopped being the
   deciding test.** The breakeven probability is 25.2101 % and the assessment is 25 %. Nobody can
   estimate a provider-capacity failure to a fifth of a percentage point; the difference is noise. The
   correct reading is not "reject, it loses USD 150" — that is false precision dressed as rigour — but
   **"on expected value this decision does not matter, so decide it on the tail and on
   survivability"**. That is exactly the boundary 8.3.1 draws between portfolio logic and structural
   logic, arriving here from the other direction: the arithmetic hands the decision back to judgment,
   and names why it is doing so.

   **On the tail it is not close.** The retainer costs USD 18,000 and removes USD 184,776 of
   95th-percentile exposure — a ratio of **10.27 to 1**. And the tail is not a smooth extension of the
   average, because a 12-week recovery crosses a **threshold**: it misses the winter window, at which
   point a further 74,256 of deferred benefit appears that no expected-value calculation containing
   only a weekly rate would ever show. **Resilience is bought where the consequence function has a
   step in it**, which is a sharper statement of "where failure cannot be tolerated" and a testable
   one: find the thresholds, then ask what recovery time clears them.

   **The efficiency cost is real and should be named, not hidden in the tail argument.** USD 18,000
   buys nothing at all in the 75 % of futures where the provider performs; that is what "resilience
   costs efficiency" means in cash, and it is the reason the lever has to be aimed rather than applied
   everywhere. The discriminator is the step: dependencies whose failure
   crosses a threshold get structural treatment, dependencies whose failure is priced linearly are
   accepted and funded through contingency. Two cautions. The 95th-percentile recovery times are
   themselves estimates with wider errors than the mean, so the tail argument should be presented as an
   order of magnitude — 10 to 1 — rather than as USD 184,776 to the dollar. And a retainer is itself a
   contract with a counterparty, so it carries the secondary risk 8.3.1 priced: a second provider that
   cannot mobilise in two weeks when called has sold an option that does not exercise, which is a
   procurement-terms question (Domain 10, KA 10.3) and not a risk-register one.

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

**Reference-class forecasting** is the countermeasure with arithmetic behind it, and the arithmetic
does something the table above cannot: it prices the risks nobody identified. The method is to stop
asking "what will this project cost?" and instead ask "what did comparable completed projects cost
against what they were approved for?" — then apply that distribution of *outturn ratios* to the
current estimate. The word doing the work is **completed**: the reference class contains the
overruns, the surprises and the unidentified risks of every project in it, which is precisely the
material a bottom-up register cannot contain.

**Worked example 8.4.2 — Meridian against its reference class.**

1. **Setup.** Meridian's approved cost is **USD 2,400,000**, built bottom-up, with a quantified risk
   register whose P80 contingency requirement is **USD 203,784.67** (8.2.2b, at the 0.8416 factor).
   The sponsoring authority holds records of **12** comparable completed clinical-system rollouts. Each
   is expressed as an **outturn ratio** — final cost ÷ approved cost at the equivalent gate — and
   sorted: 0.95, 1.02, 1.05, 1.08, 1.12, 1.15, 1.18, 1.22, 1.28, 1.35, 1.48, 1.72.
2. **Formula.** Mean ratio = Σ ratios ÷ n. Median = average of the two central values at even n. The
   80th percentile is read from the order statistics at position `(n + 1) × 0.80`, interpolating
   between neighbours. Reference-class forecast = approved cost × ratio; uplift = forecast − approved
   cost.
3. **Substitution.** Σ = 14.60, so mean `= 14.60/12`. Median `= (1.15 + 1.18)/2`. Order position
   `13 × 0.80 = 10.40`, so `P80 = 1.35 + 0.40 × (1.48 − 1.35)`.
4. **Result.**

   | Statistic | Ratio | Forecast (USD) | Uplift on 2,400,000 | Uplift % |
   |---|---|---|---|---|
   | Best case in class | 0.95 | 2,280,000 | (120,000) | (5.00 %) |
   | Median | 1.1650 | 2,796,000 | 396,000 | 16.50 % |
   | Mean | 1.216667 | **2,920,000** | 520,000 | 21.67 % |
   | P80 | 1.4020 | **3,364,800** | **964,800** | **40.20 %** |
   | Worst case in class | 1.72 | 4,128,000 | 1,728,000 | 72.00 % |

   **11 of the 12** projects (**91.67 %**) exceeded their approved cost. The reference-class P80 uplift
   of **USD 964,800** is **4.7344 times** the register-based P80 of 203,785 — a gap of
   **USD 761,015.33**.
5. **Interpretation.** The gap is the finding, and what to do with it is the professional skill.

   **Do not add the two numbers.** They are two estimates of the same quantity by different methods,
   not two separate exposures, and summing them double-counts every risk the register identified.
   The reference class already contains ground-condition surprises, interface rebuilds and trainer
   attrition, because the twelve projects in it experienced them.

   **Use the reference class as a challenge on the total, and treat the gap as an estimate of what the
   register cannot see.** Two causes account for it, and only one is measurable from the data here.
   The register-completeness arithmetic of 8.1.3 estimated **11.4286** unidentified risks; if each
   resembled the average quantified entry at `119,000/5 =` **USD 23,800** of `EMV`, they would add
   **USD 272,000.00** — **35.74 %** of the gap. The residual **USD 489,015.33** cannot be decomposed
   from the evidence available and must be presented as such: part of it is **optimism in the impacts
   already assessed** (8.4.2's subject), and part is **scope growth** rather than risk at all, because
   projects in a reference class typically had less well-defined scope at the equivalent gate and grew
   into their outturns (Domain 5). Presenting the residual as a single cause would be exactly the false
   precision this topic warns against.

   **What the numbers change.** Not the approved cost, which is a governance artefact, but three
   things: the **management reserve**, which now has a defensible basis (8.1.3's unidentified risks and
   the residual gap, held outside the baseline under sponsor control); the **funding requirement**
   presented to the authority, which should carry the reference-class range rather than a single
   figure; and the **tolerance conversation** of 8.3.2, since a 5 % tolerance against a class whose
   median outturn is 16.5 % is a statement about this programme being materially better managed than
   its peers — a claim that may be true and must then be evidenced, not assumed.

   **Three cautions on the method itself.** The class must be genuinely comparable: twelve rollouts
   of clinical systems, not twelve public-sector programmes of any kind, and the leader should be able
   to say what makes them comparable and what makes each different. Twelve is a small class, so the
   P80 read from order statistics is coarse — the interpolation between the 10th and 11th values moves
   the answer by USD 312,000 across that single interval, which is the honest measure of its precision.
   And the class's own approved costs were set under whatever appraisal regime applied at the time; a
   class drawn from an era of weaker scope definition overstates the uplift a well-defined project
   needs, which is a reason to prefer recent comparators and to say so.

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

"Tune the thresholds" is the standard remedy for false positives, and it is incomplete advice
because it does not say **which way**. The direction is decided by the base rate and by the ratio of
what a caught problem saves to what an alert costs to investigate — both of which a programme can
measure, and neither of which is a matter of opinion.

**Worked example 8.4.4 — which way to tune Meridian's risk monitor.**

1. **Setup.** Meridian runs an anomaly monitor over the weekly rollout data from its **40** clinics.
   In a given clinic-week a genuine emerging problem is present with probability **0.01** — the
   measured base rate over the first two quarters, i.e. **0.4** genuine problems a week across the
   estate. Caught early, a problem is remediated for USD 4,000; caught late it costs USD 26,000, so
   catching one **saves USD 22,000**. Investigating an alert costs **USD 900** — half a day of a
   delivery manager. The vendor offers two calibrations: **A, sensitive** (sensitivity 0.85,
   specificity 0.90) and **B, specific** (sensitivity 0.60, specificity 0.98).
2. **Formula.** With `N` clinic-weeks, base rate `b`, sensitivity `s` and specificity `k`:
   true positives `= Nbs`; false positives `= N(1−b)(1−k)`; alerts = their sum; precision = TP ÷
   alerts. Net value = `TP × saving − alerts × investigation cost`. An alert is worth investigating
   while `precision × saving ≥ cost`, so the breakeven precision is `cost ÷ saving`. Setting the two
   configurations' net value equal gives the crossover base rate.
3. **Substitution.** A: `TP = 40 × 0.01 × 0.85`; `FP = 40 × 0.99 × 0.10`. B: `TP = 40 × 0.01 × 0.60`;
   `FP = 40 × 0.99 × 0.02`. Breakeven precision `900/22,000`. Net value as a function of `b`:
   A `= 748,000b − (30b + 4) × 900 = 721,000b − 3,600`; B
   `= 528,000b − (23.2b + 0.8) × 900 = 507,120b − 720`.
4. **Result.**

   | | TP/week | FP/week | Alerts | Precision | Missed/week | Net value/week |
   |---|---|---|---|---|---|---|
   | **A** sensitive, at `b` = 1.0 % | 0.3400 | 3.9600 | 4.3000 | **7.91 %** | 0.0600 | **USD 3,610.00** |
   | **B** specific, at `b` = 1.0 % | 0.2400 | 0.7920 | 1.0320 | **23.26 %** | 0.1600 | **USD 4,351.20** |
   | A sensitive, at `b` = 7.5 % | 2.5500 | 3.7000 | 6.2500 | 40.80 % | 0.4500 | USD 50,475.00 |
   | B specific, at `b` = 7.5 % | 1.8000 | 0.7400 | 2.5400 | 70.87 % | 1.2000 | USD 37,314.00 |

   The breakeven precision is `900/22,000 =` **4.0909 %**. The crossover base rate is
   `(3,600 − 720)/(721,000 − 507,120) =` **1.3465 %**: below it the **specific** configuration wins,
   above it the **sensitive** one. Configuration A stops paying at all below a base rate of
   **0.4993 %**; B keeps paying down to **0.1420 %**. Over Meridian's 26-week rollout at its measured
   1.0 % base rate, B returns **USD 113,131.20** against A's **USD 93,860.00** — a difference of
   **USD 19,271.20** — at the cost of **2.60** more missed problems across the rollout
   (4.16 against 1.56).
5. **Interpretation.** Three results, and they are not the ones the received wisdom predicts.

   **A monitor whose alerts are wrong 92.09 % of the time still pays.** At configuration A's 7.91 %
   precision the monitor returns USD 3,610 a week, because precision only has to clear
   `cost ÷ saving = 4.0909 %` — and 7.91 % is **1.9328 times** that threshold. So "we get too many
   false alarms" is not, by itself, an economic argument for anything. It is an **attention** argument,
   and attention is the resource the calibration should be protecting; the arithmetic makes clear that
   the choice between A and B is about how a scarce delivery manager's time is spent, not about whether
   the monitor is worth running.

   **The direction of tuning depends on the base rate, and it flips.** At Meridian's 1.0 % base rate
   the specific configuration is better by USD 741.20 a week; at a 7.5 % base rate the sensitive one is
   better by USD 13,161 a week. The crossover — **1.3465 %** — is uncomfortably close to Meridian's
   measured 1.0 %, which means the recommendation is **fragile** and must be presented as such: a
   modest deterioration in the estate would reverse it. Two governing quantities, one ratio, one
   crossover; that is the whole of it, and it is why calibration is a measurement duty rather than a
   preference. The corollary for practice is that the **base rate must be measured before the
   threshold is set**, and re-measured when the delivery environment changes — which is the
   calibration record the paragraph above requires, now with a stated purpose.

   **And the expected-value answer can be overruled, for a nameable reason.** B wins at Meridian's
   base rate by missing more: **4.16** genuine problems across the rollout against A's 1.56. If a
   missed problem is a USD 22,000 remediation, that trade is already priced and B is right. If any
   single missed problem could be a clinical-safety event or an irreversible data loss, then 8.3.1's
   rule binds — **existential exposure is not managed by its expected value** — and the sensitive
   configuration is correct regardless of the USD 19,271 it forgoes. The professional obligation is to
   state which regime applies before quoting the arithmetic, and the failure to do so is how an
   optimisation becomes an incident. Two cautions on the model. Sensitivity and specificity are
   themselves estimates from a calibration sample and drift as the estate changes (Domain 14,
   14.A.2 on revalidation). And the arithmetic assumes alerts are independent draws; a monitor that
   fires repeatedly on one underlying condition inflates the alert count without adding information,
   which is the correlation problem of 8.A.1 appearing in the detection layer.

### Key terms — KA 8.4

| Term | Meaning |
|---|---|
| **Resilience** | Capability to absorb unidentified risk; buffers, optionality, modularity, redundancy, fast detection. |
| **Reference-class forecasting** | Estimating from comparable completed projects to counter optimism bias. |
| **Pre-mortem** | Imagining failure before committing, to license dissent and surface missed risk. |
| **Escalation of commitment** | Continuing because of sunk cost; countered by forward-looking framing. |
| **Displaced judgment** | Ceasing to think because a tool is watching. |
| **Outturn ratio** | Final cost ÷ approved cost for a comparable completed project; the unit of a reference class. |
| **Consequence threshold** | A point where the consequence function steps rather than scales; where resilience is bought. |
| **Base rate** | The prevalence of the condition a monitor looks for; with the saving-to-investigation ratio it decides how a monitor should be tuned. |
| **Precision** | True alerts ÷ all alerts; worth investigating while precision exceeds investigation cost ÷ saving. |

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

**MCQ 8.4-E `[8.4.1 · Analysis]`** A resilience measure costs USD 18,000, is needed with probability
0.25 and saves five weeks of a USD 14,280-per-week delay. Its breakeven probability is 25.2101 %. The
correct professional conclusion is:
- A. reject it — the expected loss is USD 150
- B. the breakeven lies inside the estimating error of the probability, so expected value cannot decide it; decide on the tail and on survivability ✅
- C. accept it — a resilience measure is always worth buying
- D. re-estimate the probability to three decimal places and re-run the comparison

*Rationale:* No one estimates a capacity failure to a fifth of a percentage point, and the tail
comparison is decisive at roughly 10 to 1 (8.4.1). A treats a noise-level difference as a result;
C abandons the arithmetic entirely; D is false precision — the input cannot support the digits.

**MCQ 8.4-F `[8.4.4 · Application]`** A monitor's alerts are correct 7.91 % of the time. Investigating
one costs USD 900 and catching a genuine problem saves USD 22,000. Investigating every alert is:
- A. wasteful — over 92 % of investigations find nothing
- B. worthwhile: the breakeven precision is 900/22,000 = 4.09 %, so 7.91 % returns 1.93 times the investigation cost ✅
- C. worthwhile only if precision exceeds 50 %
- D. impossible to assess without the monitor's sensitivity

*Rationale:* An alert is worth investigating while precision exceeds cost ÷ saving (8.4.4). A counts
the failures rather than valuing the successes — the arithmetic's whole point; C invents a threshold;
D confuses what is needed to compare two calibrations with what is needed to decide whether to
investigate a given alert.

**MCQ 8.4-G `[8.4.2 · Analysis]`** A bottom-up register gives a P80 contingency of USD 203,785 while a
reference class of twelve comparable completed projects gives a P80 uplift of USD 964,800. The
professional treatment is:
- A. add them, giving USD 1,168,585 of required funding
- B. treat them as two estimates of one quantity: use the reference class to challenge the total, attribute part of the gap to unidentified risks and the rest to optimism and scope growth, and hold the difference as management reserve ✅
- C. discard the reference class, since the register is specific to this project
- D. discard the register, since the reference class is evidence and the register is judgment

*Rationale:* The reference class already contains the register's risks, so adding double-counts
(8.4.2). C loses the only measurement of what the register cannot see; D discards the only basis for
response and ownership — the reference class names no risk, no owner and no action.

### Self-check — KA 8.4

1. *Why is resilience not a substitute for quantification, and vice versa?* — Quantification
   handles named risks; resilience absorbs the unnamed. Each fails at the other's job.
2. *What is a pre-mortem for?* — To license dissent before commitment and surface risks a workshop
   suppresses.
3. *What is AI risk sensing's structural blind spot?* — Novel risk: it detects patterns like the
   past.
4. *Where is resilience bought, expressed as a test?* — Where the consequence function has a step in
   it: find the thresholds, then ask what recovery time clears them.
5. *Why must a bottom-up contingency and a reference-class uplift never be added?* — They estimate the
   same quantity; the reference class already contains the register's risks.
6. *What two quantities decide whether a monitor should be tuned sensitive or specific?* — The base
   rate and the ratio of the saving from a caught problem to the cost of investigating an alert.

---

## Advanced topics — Domain 8

### 8.A.1 Correlation and why aggregate risk is worse than it looks

Independence made 8.2.4's arithmetic tractable and understated the answer; worked example 8.2.4b
prices the understatement on Auriga's own register — σ rising by a factor of 1.3471 and a P80 reserve
degrading to 73.4 % on one shared subcontractor. This topic takes the organisational form of the same
error, which is larger and less visible.

The mechanism is worth stating once in its general form. When risks share a driver — one supplier,
one technology, one regulator, one labour market — they move together, and the correlation term
`2 Σᵢ<ⱼ ρᵢⱼ σᵢ σⱼ` grows with the *number of pairs*, `k(k−1)/2`, while the independent term grows only
with `k`. **Concentration therefore hurts quadratically**: doubling the number of risks on one driver
roughly quadruples the variance it contributes. That is the arithmetic behind the observation that a
bad month becomes a crisis, and it is why counting entries is not a measure of exposure.

Three practical implications follow. Identify **common drivers** explicitly and give each a column in
the register, so that grouping by driver is a sort rather than a project. Model correlation in
simulation rather than assuming it away, and disclose the coefficients as the judgments they are.
And where a driver is identifiable, prefer to **model the driver itself as a single risk with a
structural response** (8.3.1) over modelling its symptoms as correlated line items — a correlation
coefficient is a poor description of a subcontractor that either performs or does not, and it
understates the extreme tail where all the symptoms occur together.

The organisational version of this error is believing a portfolio of projects is diversified when
every one depends on the same scarce engineering resource, and it is the subject of Case study B —
where the arithmetic is done.

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

That much is standard. What is usually left out is that the simple product is itself wrong when the
paths share predecessors — as converging paths almost always do — and that merge bias **fades as the
date becomes less aggressive**, so quoting it without a date is meaningless. Both are worth working
on the real network.

**Worked example 8.A.2 — merge bias at Auriga's node E, three ways.**

1. **Setup.** Domain 6's network reaches installation **E** through two paths: A–B–C (mobilise,
   design, procure hardware) and A–B–D (mobilise, design, civil and cabling). Deterministic durations
   put A–B–C at 16 weeks and A–B–D at 15, so `ES(E) = 16` with one week of float on D. The estimating
   team supplies three-point durations:

   | Activity | `o` | `m` | `p` | `tₑ = (o+4m+p)/6` | `σ = (p−o)/6` |
   |---|---|---|---|---|---|
   | A Mobilise | 1.5 | 2 | 3.5 | 2.166667 | 0.333333 |
   | B Detailed design | 5 | 6 | 10 | 6.500000 | 0.833333 |
   | C Procure control hardware | 6 | 8 | 14 | 8.666667 | 1.333333 |
   | D Civil and cabling works | 5 | 7 | 12 | 7.500000 | 1.166667 |

   What is the probability that E can actually start in week 16?
2. **Formula.** Path `tₑ` = Σ activity `tₑ`; path σ = `√(Σ σᵢ²)` (variances add). Probability of
   meeting a date `T` on one path = `Φ((T − tₑ)/σ)`. For a merge of **independent** paths, multiply.
   For paths sharing predecessors, decompose: `start_E = AB + max(C, D)`, and condition on the shared
   part — `P = ∫ f_AB(t) · Φ((T−t−tₑC)/σC) · Φ((T−t−tₑD)/σD) dt`.
3. **Substitution.** A–B–C: `tₑ = 2.166667 + 6.5 + 8.666667`, variance
   `0.111111 + 0.694444 + 1.777778`. A–B–D: `tₑ = 2.166667 + 6.5 + 7.5`, variance
   `0.111111 + 0.694444 + 1.361111`. Shared A+B: `tₑ = 8.666667`, σ `= √0.805556`.
4. **Result.** Path A–B–C `tₑ` **17.3333** weeks, σ **1.6073**; path A–B–D `tₑ` **16.1667**, σ
   **1.4720**; shared A+B σ **0.8975**.

   | Reading of the same network | P(E can start in week 16) |
   |---|---|
   | Deterministic CPM | 100 % (a date, not a probability) |
   | Critical path alone | **20.34 %** |
   | Independent-path product (textbook merge bias) | **9.25 %** |
   | **Correct, sharing A and B** | **13.16 %** |

   And the effect decays with the date:

   | Target for `ES(E)` | Dominant path alone | Correct merge | Reduction from the merge |
   |---|---|---|---|
   | Week 16 | 20.34 % | 13.16 % | **35.31 %** |
   | Week 17 | 41.79 % | 34.33 % | 17.85 % |
   | Week 18 | 66.08 % | 61.53 % | 6.90 % |
   | Week 18.686 (the dominant path's P80) | 80.00 % | **77.61 %** | 2.99 % |
   | Week 20 | 95.15 % | 94.80 % | **0.37 %** |

   The date at which E's start is 80 % likely is **week 18.8093** — a buffer of **2.8093 weeks** over
   the deterministic 16 — against **week 18.6860** if only the dominant path is considered. So
   ignoring the merge understates the buffer by **0.1233 weeks**, which at Auriga's cost of delay of
   USD 45,000 per week is **≈ USD 5,550** — quoted to the nearest fifty deliberately, because it is
   the difference between two numerically integrated dates built on PERT's `σ = (p − o)/6`
   convention, and a figure stated to the cent would claim a precision the method has not got.
5. **Interpretation.** Four conclusions, and the last two are the ones that stop merge bias being
   misapplied.

   **The deterministic date is not a forecast.** Week 16 has a **13.16 %** chance of being met. Nothing
   is wrong with the network; the date is the *mode* of a distribution and is being read as a
   commitment (Domain 6, KA 6.4.3 makes the same point on a single activity). The instrument that
   fixes it is a buffer chosen at a stated confidence, exactly as contingency is (8.2.4) — the
   schedule version of the same discipline, and the reason a project with a P80 cost reserve and a P0
   schedule date is internally inconsistent.

   **The textbook product overstates the bias.** Multiplying 20.34 % by 45.49 % gives 9.25 %, but the
   two paths share A and B: a good mobilisation and design help *both* paths simultaneously, so the
   failures are correlated and the true figure is **13.16 %** — 42.2 % higher than the naive product.
   The ordering is an invariant worth remembering: **naive product ≤ correct merge ≤ dominant path
   alone.** A leader given a merge-bias number should ask which of the three it is; a schedule
   simulation that honours the logic computes the middle one automatically, while a spreadsheet
   multiplying path probabilities computes the lower bound and calls it the answer.

   **Merge bias is a property of a date, not of a network.** At week 16 the merge removes 35.31 % of
   the probability; at week 20 it removes 0.37 %. The reason is structural: as the date becomes
   generous, the dominant path's probability approaches 1 and the secondary path's approaches 1 faster,
   so the product approaches the dominant path's own figure. **Convergence points are dangerous at
   aggressive dates and almost irrelevant at conservative ones** — which is why the buffer
   understatement above is only 0.12 weeks even though the week-16 distortion is severe. Quoting
   "merge bias adds X weeks" without the confidence level attached is therefore not a result.

   **And the practical consequence is where to look, not how much to add.** The decision-useful output
   of this analysis is not the 2.81-week buffer but the **criticality index**: D reaches E only 1.17
   weeks behind C on expectation with a σ of 1.47, so D lands on the critical path in a substantial
   minority of futures despite showing one week of float deterministically. That is what makes it worth
   monitoring, and it is the same finding Domain 6 reached by re-running its passes after a crash —
   here obtained without crashing anything. Two cautions. PERT's `σ = (p − o)/6` is a convention rather
   than a derivation and understates spread on strongly skewed estimates, so the probabilities above
   should be read to the nearest point. And the arithmetic treats C and D as independent given A and B,
   which fails if the same site conditions or the same subcontractor drive both — in which case
   8.2.4b's correlation treatment applies to the schedule exactly as it did to the cost aggregation.

### 8.A.3 The reviewer's risk eye

Invariants worth testing in an hour: every entry has cause, event and consequence and a named
owner; no occurred risks parked as high-probability entries; opportunities present in credible
proportion; Σ`EMV` reconciles to the register; contingency stated at a named confidence level with
its independence assumption disclosed; contingency consumption trended against risk retirement,
not time; responses costed against `EMV` reduction, with secondary risks recorded; existential
risks handled structurally rather than by `EMV`; early-warning indicators defined for the top
items; and at least one decision this month traceable to the register. Failure of the last one
matters most: it means the process is documentation, not decision support.

Seven further tests, each of which this domain has now made arithmetic, and each of which takes one
division:

- **Does every total belong to one objective?** A "total exposure" mixing cost and benefit impacts is
  compared with nothing and can be funded from nothing (8.2.2b).
- **Is total exposure stated against the tolerance, not the budget?** Meridian's 4.96 % of approved
  cost is 99.17 % of the room (8.2.2b).
- **Does the confidence level follow from an appetite statement, or from habit?** If the latter, write
  down the appetite that the inherited level implies and put it to the sponsor (8.3.2).
- **Is the reserve adequate against the register still open?** Remaining ÷ recomputed requirement; a
  ratio below 1.00 is a trigger, and Auriga's was 0.9037 while the naive drawdown test passed
  (8.3.2b).
- **Have retired risks released their variance share, not just their `EMV`?** R4's retirement released
  USD 97,239 against an `EMV` of 60,000 (8.3.2b).
- **Is there a quantification threshold, written down?** A screen with no threshold has replaced
  quantification rather than ordering it, and one matrix cell can conceal a 14.6-fold span of
  expected values (8.2.1).
- **Has register completeness been estimated rather than assumed?** Two independent identification
  methods and their overlap give a floor on what is missing, and that floor — not a percentage
  convention — is the basis for management reserve (8.1.3).

---

## Industry variations — Domain 8

- **Construction and infrastructure.** Ground conditions, weather and permits dominate, and they are
  **shared drivers** rather than independent risks: one wet season moves every external activity
  together, so the correlation term of 8.A.1 is the dominant part of the variance and an
  independence-based reserve is furthest from the truth here. Quantitative schedule risk analysis is
  contractually expected on major programmes, which makes 8.A.2's distinction — naive product against
  correct merge — a commercial point and not an academic one. Risk allocation is largely executed
  through contract terms (Domain 10), so the counterparty arithmetic of 8.3.1 is the profession's
  central skill in this sector: a transfer to a thinly capitalised subcontractor is priced at its
  breakeven default probability or it is not priced at all.
- **Energy and resources.** Commodity price and currency exposure sit alongside delivery risk and
  are managed with financial instruments as well as project responses (PFL-AI Domains 3, 11). Two
  consequences for this domain's methods: price risk is **continuous** rather than binary, so the
  Bernoulli variance of 8.2.4 is the wrong model and simulation over a price distribution is the
  right one; and because hedges are impact levers rather than probability levers, they release
  reserve at the favourable end of 8.2.4c's 1.5-to-3 range, which is a large part of why they are
  bought.
- **Technology and digital.** Requirement volatility and integration risk dominate; resilience is
  bought through modularity, staged release and reversibility rather than buffers, so 8.4.1's
  levers apply in a different mix. The reason is 8.2.4c's identity: modularity and reversibility are
  **impact** levers, and impact levers are what release contingency. Reversibility also makes
  information cheap to buy, so the value-of-information arithmetic of 8.2.3 favours acting and
  observing over surveying and deciding — the opposite conclusion to the utilities case study, from
  the same tree with a different reactive cost.
- **Pharmaceutical and regulated development.** Low-probability/high-consequence events and
  regulatory outcomes make structural treatment (8.3.1) the norm; risk files are auditable
  artefacts, not internal management tools. The characteristic quantitative failure is presenting an
  `EMV`-optimal response for an exposure that a regulator treats as unacceptable at any probability —
  the same error 8.4.4 names in monitor tuning, where a missed detection cannot be traded against
  investigation cost. The correct output is the optimum among admissible options, which is a
  smaller set than the arithmetic alone would suggest.
- **Public programmes.** Optimism bias is explicitly recognised in appraisal guidance, and
  reference-class adjustment may be mandated; where such guidance applies, the arithmetic of 8.4.2 is
  a submission requirement rather than an option, and the professional skill is the reconciliation — never adding a
  mandated uplift to a bottom-up contingency. Political and reputational risk carries weight that no
  `EMV` captures well, and it typically has the **step consequence function** of 8.4.1: a threshold
  crossed rather than a cost incurred, which is why public programmes buy resilience that looks
  uneconomic on expected value.
- **Financial services and operational change.** Regulatory deadlines make the consequence function
  almost purely a step, so 8.4.1's threshold test governs and buffer economics dominate expected-value
  economics. Detection is heavily instrumented, which puts 8.4.4's crossover arithmetic at the centre —
  and it lands the other way. Base rates for genuine control failures are very low, which favours the
  specific configuration, but the cost of an escaped failure is very high, which favours the sensitive
  one, and the second effect wins because the crossover base rate falls roughly in proportion to the
  saving-to-investigation ratio. On Meridian's two calibrations, raising that ratio tenfold — an
  escaped problem costing USD 220,000 rather than 22,000 — moves the crossover from **1.3465 %** to
  **0.1313 %**, so the sensitive configuration becomes correct at base rates ten times lower. Same two
  quantities, opposite answer, and it is the ratio rather than the rarity that decides.

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
substantially more, and the register had contained the information needed to see that. Two further
figures from the same tree show how far from marginal the call was: the survey would still have paid
at any price up to **USD 84,000** — **3.36 times** what it was quoted at — and it would only have
stopped paying had the team's belief in contamination fallen below **11.90 %**, against the 0.40 the
register itself carried. The cost-reduction pass therefore did not make a close judgment badly; it
made a decision whose arithmetic was not close in either direction, without doing the arithmetic.

**What the domain teaches here.** Two things, both uncomfortable. **A cost-reduction pass that does
not consult the register is a risk decision taken blind** — savings targets and risk exposure must
be traded explicitly. And **the register was not the failure; its irrelevance to the decision
was** (8.3.3's test: did a decision change because of it?). The corrective adopted afterwards was
procedural: any proposed saving above a threshold must state which register entries it affects and
by how much.

## Case study B — Domain 8: the diversified portfolio that wasn't (technology programme)

**Situation.** A transformation programme's board reviewed 22 project-level risks spread across six
workstreams and concluded exposure was well diversified. The register held **14** risks each assessed
at `p` 0.25 with an impact of **USD 60,000**, and **8** each at `p` 0.20 with an impact of
**USD 80,000** — a total `EMV` of **USD 338,000** in which no single item exceeded **4.73 %** of the
aggregate. Contingency was set at **USD 380,000**: the mean plus a **12.43 %** margin, assuming
independence. On that assumption the reserve covers **62.41 %** of futures; the independence-based
P80 would have been **USD 449,784**, so even on its own terms the reserve was thin, and nobody had
computed that either.

**What happened.** Fourteen of the 22 risks depended on the same six-person integration team.
When two members left within a month, **11 of the 14** materialised or worsened *together*, along
with 2 of the 8 independent risks: an outcome of `11 × 60,000 + 2 × 80,000 =` **USD 820,000**, or
**USD 440,000** beyond the reserve. The programme's recovery required a re-baseline and a
management-reserve release.

**The arithmetic, done afterwards.** The concentration was priced three ways, and the ladder is the
lesson.

| Model of the 14 shared risks | σ of the aggregate | P80 | Confidence the 380,000 reserve buys | Where the 820,000 outcome sits |
|---|---|---|---|---|
| Independent (as assumed) | 132,823 | 449,784 | 62.41 % | 99.99th percentile — **1 in 7,026** |
| Pairwise ρ = 0.6 | 302,245 | 592,369 | 55.53 % | 94.46th percentile — **1 in 18** |
| One driver, all 14 together | **374,823** | **653,451** | **54.46 %** | 90.08th percentile — **1 in 10** |

The mean is **USD 338,000** in all three columns: correlation moved nothing about the average and
everything about the answer (8.2.4b). The correlation term at ρ = 0.6 is
`2 × 0.6 × 91 pairs × 675,000,000 =` **USD 73,710,000,000** of variance — **4.18 times** the entire
independent variance of the 22-risk register — because 14 risks generate 91 pairs and the correlation
term grows with pairs while the independent term grows with entries (8.A.1).

**What the board had actually been told.** That the outcome was a one-in-seven-thousand event. It
was a **one-in-ten** event, and the difference between those two statements is a single untested
assumption. Note also that the ladder's third rung is the *lowest* confidence figure: modelling the
integration team as one risk with an impact of `14 × 60,000 =` USD 840,000 produces the widest
distribution of the three, because a perfectly shared driver is the limiting case of correlation.
That is the honest model of a six-person team, and it makes the exposure legible in a way fourteen
line items never did.

**What was done differently.** The register was restructured by **common driver** rather than by
workstream, immediately revealing the concentration; the integration capability was treated as a
single structural risk with a structural response (cross-training, a retained partner, staged
scope) rather than as fourteen separate `EMV` line items; and contingency was re-derived by
simulation with correlation modelled explicitly. The structural response is also the cheaper one on
8.2.4c's logic: cross-training and staged scope are **impact** levers on the driver, and impact
levers release reserve at up to three times the rate of probability levers.

**What the domain teaches here.** Counting entries measures paperwork; counting **drivers** measures
exposure. A register with many risks and few drivers is concentrated, and independence is the
assumption most likely to be both convenient and wrong. The one-sentence test the Executive
perspective demands — *which of these risks share a driver?* — would have moved this programme's
stated confidence by eight percentage points and its P80 by **USD 203,667** before a single event
occurred.

---

## Executive perspective — Domain 8

What a project leader cannot delegate in this domain:

- **The confidence level, and where it comes from.** Whether contingency is set at P50 or P80 is a
  risk-appetite decision belonging to governance, and the leader ensures it is chosen, stated and
  understood — not inherited from a spreadsheet default. The stronger version of the duty is to
  *derive* it: an appetite statement of the form "no more than a 10 % chance of exceeding by more than
  5 %" fixes the level at P90 and the tolerance at a number, and a leader who has never seen that
  sentence written down should write it and take it to the sponsor (8.3.2).
- **The independence question.** Asking, of any aggregate number, which risks share a driver. It is
  one sentence and it is where Case study B's programme lost control — worth 7.95 points of stated
  confidence and USD 203,667 of P80 before anything happened.
- **Which constraint the responses are buying against.** Expected cost and confidence level are
  different objectives, and the same USD 25,000 buys different amounts of each: on Meridian, an
  impact lever returned 34.93 % more P90 reduction than a probability lever that removed more `EMV`
  (8.2.4c). The leader states which constraint binds before the register's `EMV` column decides by
  default.
- **The room, not the budget.** Insisting that total exposure be presented against the objective's
  tolerance rather than as a percentage of cost — the comparison that showed Meridian's mean
  consuming 99.17 % of its 5 % tolerance, and that no percentage-of-budget statement would ever have
  surfaced.
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
answering 0.80, the best of the two. *Second common error, and the subtler one:* multiplying when the
two paths **share predecessors**, which correlates them and makes 0.64 a lower bound rather than the
answer — on Auriga's node E the naive product gives 9.25 % where the correct figure is 13.16 %.

**Exercise 8.5** Three risks: X `p` 0.30 impact 200,000; Y `p` 0.40 impact 150,000; Z `p` 0.20 impact
250,000. X and Z depend on the same supplier, with an assessed correlation of ρ = 0.6; Y is
independent of both. Compute the P80 contingency assuming independence, the P80 with the correlation
modelled, and the confidence the independence-based figure actually buys.
*Solution.* `EMV`s **60,000**, **60,000**, **50,000**; mean **USD 170,000**. Variances
`0.30×0.70×200,000² = 8.40e9`; `0.40×0.60×150,000² = 5.40e9`; `0.20×0.80×250,000² = 1.00e10`; total
**23.80e9**, σ **USD 154,272.49**, so P80 = `170,000 + 0.8416 × 154,272.49 =` **USD 299,835.72**.
Correlation term `2 × 0.6 × σₓ σz = 1.2 × 91,651.51 × 100,000 =` **10,998,181,667.89**, giving total
variance **34,798,181,667.89**, σ **USD 186,542.71** and a corrected P80 of **USD 326,994.34** —
**9.06 %** higher. The independence figure of 299,836 sits at `z = (299,836 − 170,000)/186,542.71 =`
0.6960 of the correlated distribution, i.e. it is a **75.68 %** reserve, not an 80 % one.
*Common error:* adding the two σ values (154,272 + a correlation adjustment) instead of adding
variances, or — more damaging — reporting the reserve as P80 after discovering the shared supplier,
which leaves the label attached to the wrong number.

**Exercise 8.6** A risk carries `p` 0.20 and an impact of USD 300,000. Two responses cost the same:
one halves the probability, the other halves the impact. Compare their effect on `EMV` and on
variance, and state the general rule.
*Solution.* Base `EMV` **60,000**; base variance `0.20 × 0.80 × 300,000² =` **14.40e9**. Halving the
probability: `EMV` **30,000**, variance `0.10 × 0.90 × 300,000² =` **8.10e9**, so **6.30e9** removed.
Halving the impact: `EMV` **30,000**, variance `0.20 × 0.80 × 150,000² =` **3.60e9**, so **10.80e9**
removed — a factor of **1.714286** more, at identical `EMV` reduction. The rule: `EMV` is linear in
both `p` and `I`, so either halving removes the same expected cost; variance is `p(1−p)I²`, so halving
the impact always removes 75 % of a risk's variance while halving the probability removes between
25 % and 50 %, giving a ratio between **1.5** and **3** for any `p ≤ 0.5` (8.2.4c).
*Common error:* concluding the two responses are equivalent because their `EMV` reduction is
identical — true, and irrelevant whenever the binding constraint is a confidence level rather than an
expected cost.

**Exercise 8.7** A risk has `p` 0.25 and an impact of USD 400,000. Three responses are offered.
**Reduce:** USD 28,000, cutting `p` to 0.10, with a secondary risk of `p` 0.15 and impact 40,000.
**Transfer:** USD 26,000 premium recovering 70 % of the impact, with the counterparty unable to pay
with probability 0.30 given the event. **Avoid:** USD 150,000, removing the risk, with a secondary
risk of `p` 0.20 and impact 90,000. Rank the four options including acceptance, first ignoring the
secondary risks and then including them, and compute the counterparty default probability at which
the transfer ties the best alternative.
*Solution.* Accept **USD 100,000**. Ignoring secondaries: transfer `26,000 + 0.25 × 400,000 × 0.30 =`
**56,000**; reduce `28,000 + 0.10 × 400,000 =` **68,000**; avoid **150,000** — transfer appears best by
12,000. Including secondaries: reduce `68,000 + 0.15 × 40,000 =` **74,000**; transfer
`56,000 + 0.25 × 0.30 × 280,000 =` **77,000**; avoid `150,000 + 0.20 × 90,000 =` **168,000** — so
**reduce wins**, and the ranking reversed. Reduce creates **USD 26,000** of value against acceptance,
a **26.00 %** improvement. The transfer ties reduce at
`(74,000 − 26,000 − 30,000)/(0.25 × 280,000) =` **25.71 %** of counterparty default.
*Common error:* omitting the secondary risks, which here selects the wrong response; and treating the
transfer's counterparty exposure as independent of the event, when the disruption that triggers the
claim is usually what impairs the counterparty.

**Exercise 8.8** A risk workshop identifies 28 risks; an independent checklist-and-interview review
identifies 19; 11 appear on both lists. Estimate the risk population, the number still unidentified
and the coverage achieved, cross-check with the Chapman estimator, and state what the answer may and
may not be used for.
*Solution.* `N̂ = (28 × 19)/11 = 532/11 =` **48.3636**. Distinct identified `= 28 + 19 − 11 =` **36**,
so **12.3636** risks are estimated unidentified — **25.56 %** of the population — and coverage is
**74.44 %**. Chapman: `(29 × 20)/12 − 1 =` **47.3333**, giving **11.3333** missing — the two
estimators differ by **1.0303**, which is the estimated missing count divided by `m + 1`
(`12.3636/12`), the identity Toolkit 8.T.4 uses as its divergence test. The workshop alone covered
**57.89 %**. The estimate justifies **management reserve
and resilience** sized against unidentified exposure and a decision on whether to run a third
identification method; it does **not** justify adding `EMV`, because the missing risks have no cause,
event, consequence or owner (8.1.3).
*Common error:* reading a high overlap as evidence of completeness — a large `m` deflates `N̂`, so
shared blind spots produce reassuring coverage; and adding the estimated missing count to contingency,
which funds risks that cannot be responded to or retired.

**Exercise 8.9** A monitor watches 60 sites weekly. A genuine emerging problem is present in a
site-week with probability 0.03. Catching one early saves USD 15,000; investigating an alert costs
USD 750. Configuration A has sensitivity 0.90 and specificity 0.88; configuration B has sensitivity
0.65 and specificity 0.97. Compute each configuration's weekly net value and A's precision, the
breakeven precision for investigating an alert, and the base rate at which the two configurations are
equally good.
*Solution.* At `b` = 0.03, A: true positives `60 × 0.03 × 0.90 =` **1.62**, false positives
`60 × 0.97 × 0.12 =` **6.984**, alerts **8.604**, precision **18.83 %**; net
`1.62 × 15,000 − 8.604 × 750 =` **USD 17,847.00**. B: true positives **1.17**, false positives
`60 × 0.97 × 0.03 =` **1.746**, alerts **2.916**; net `1.17 × 15,000 − 2.916 × 750 =`
**USD 15,363.00** — so **A wins by USD 2,484**. Breakeven precision `750/15,000 =` **5.00 %**, which
A's 18.83 % clears by a factor of 3.77. As functions of the base rate, `Net_A = 774,900b − 5,400` and
`Net_B = 557,100b − 1,350`; equating gives `b* = 4,050/217,800 =` **1.8595 %**. Below that the
specific configuration wins; above it the sensitive one. A stops paying below **0.6969 %**, B below
**0.2423 %**.
*Common error:* choosing the configuration with the higher precision — B's precision is better at
every base rate and its net value is worse above 1.86 %; and applying the crossover from one
programme to another, since it moves with both the base rate and the saving-to-investigation ratio.

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

Register version and date · **objective the total belongs to**, with the other objectives'
aggregations cross-referenced · Σ`EMV` · **the objective's tolerance, and Σ`EMV` as a percentage of
it** · variance and σ by risk, ranked by **variance contribution** as well as by `EMV` ·
**independence assumption stated and challenged** (which risks share drivers, with the assumed ρ and
the P80 both ways) · method (formula approximation or simulation, with correlation treatment) ·
**the appetite statement the confidence level is derived from**, and who owns it · resulting
contingency · **the price of confidence** (cost per percentage point between the adjacent levels) ·
management reserve separately, with its basis including the **register-completeness estimate** of
8.1.3 · draw protocol reference · and, from the second period onwards, the **adequacy ratio**:
remaining reserve ÷ the requirement recomputed on the register still open. A contingency figure
without this sheet is a number without a defence; a contingency figure without the last line is a
number that was defensible once.

### Toolkit 8.T.3 — Pre-mortem session guide

Before commitment, one hour: (1) state the plan and the commitment about to be made; (2) each
participant writes privately for ten minutes — "it is twelve months on and this has failed; why?";
(3) round-robin with no challenge permitted during collection; (4) cluster into causes and identify
**common drivers**; (5) convert each into a three-part risk statement with an owner; (6) identify
the three cheapest early-warning indicators. Rule: the most senior person speaks last, always.

### Toolkit 8.T.4 — Register-completeness check (one per gate)

A twenty-minute calculation that turns "the register is probably incomplete" into a funded position.
(1) Run two identification methods **separately**, by different people, with no sight of each other's
output — a workshop and an assumption-analysis review are the standard pair (8.1.3). (2) Reconcile the
two lists on **cause-and-event identity**, not wording; record `n₁`, `n₂` and the overlap `m`.
(3) Compute `N̂ = n₁n₂/m` and the Chapman cross-check `((n₁+1)(n₂+1))/(m+1) − 1`. The two differ by
exactly the estimated missing count divided by `m + 1`, so the divergence needs no threshold of its
own: read it as a fraction of the missing count, and where it approaches that count `m` is too small
to support the estimate and a third method is the answer rather than a third decimal place.
(4) Record distinct identified, estimated unidentified and coverage. (5) State the coverage threshold
that triggers a further method, and whether it fired. (6) Carry the estimated unidentified count into
the management-reserve basis on Toolkit 8.T.2 — **never** into contingency. Standing caution to print
on the sheet: the missing count is a floor, not an estimate (8.1.3).

### Toolkit 8.T.5 — Response option comparison (one per quantified risk)

One row per option — accept, avoid, reduce, transfer, and for opportunities exploit, enhance, share,
ignore — with five columns that must all be filled before a choice is recorded: **response cost** ·
**residual `EMV`** (`p` and impact after the response, both restated) · **secondary risks created**,
each with its own `p` and impact and its own owner · **total expected cost** · and **variance after**,
`p(1−p)I²` on the residual, because a response is also a reserve decision (8.2.4c). Two lines beneath
the table complete it: the **cost at which the ranking flips** to the next-best option, and — for any
transfer — the **counterparty default probability at which the transfer ceases to be preferred**,
noting whether that default is correlated with the event transferred. A comparison without the
secondary-risk column selects the wrong option often enough to be a known defect (8.3.1); one without
the flip point cannot be reviewed, because a reviewer cannot tell whether the choice was close.

## Exam preparation — Domain 8

**The traps.** Multiplying and summing ordinal scores as if they were money (8.2.1) · omitting
opportunities from total exposure (Exercise 8.1) · setting contingency at Σ`EMV` and calling it
funded (MCQ 8.2-D) · √(Σσ) instead of √(Σvariance) (Exercise 8.2) · comparing response cost against
impact rather than `EMV` reduction (Exercise 8.3) · managing an existential low-probability risk by
its `EMV` (8.3.1) · assuming independence without testing common drivers (8.A.1) · answering the
best path probability at a merge point (Exercise 8.4) · leaving occurred risks in the register as
`p` = 1.0 · treating a transfer as removing exposure.

**The traps this domain's later topics add.** Adding a correlation adjustment to σ rather than to the
variance, and — worse — leaving the "P80" label on a reserve after a shared driver has been found
(Exercise 8.5, MCQ 8.2-G) · treating two responses as equivalent because their `EMV` reduction is
equal, when one is an impact lever and the other a probability lever (Exercise 8.6) · omitting
secondary risks from a response comparison, which reverses the ranking whenever a transfer's
counterparty default is correlated with the event (Exercise 8.7, MCQ 8.3-D) · returning only a retired
risk's `EMV` when the reserve was sized on a confidence level (MCQ 8.3-E) · testing a part-consumed
reserve against elapsed time or the original register instead of the register still open (8.3.2b) ·
choosing a confidence level by convention when the appetite statement determines it (MCQ 8.3-F) ·
summing a bottom-up contingency and a reference-class uplift (MCQ 8.4-G) · reading a large overlap
between two identification methods as evidence of completeness (MCQ 8.1-E) · comparing a monitor's
precision against a target instead of against `cost ÷ saving`, or choosing the higher-precision
calibration without computing net value (MCQ 8.4-F, Exercise 8.9) · multiplying path probabilities at
a merge point when the paths share predecessors, and quoting a merge-bias figure without the date it
applies to (8.A.2) · treating a breakeven that falls inside the estimating error as a result
(MCQ 8.4-E) · and expressing total exposure as a percentage of budget when the governing constraint is
a tolerance (8.2.2b).

**The calculations to be able to do under time pressure.** `EMV` and Σ`EMV` with opportunities signed
correctly · variance `p(1−p)I²`, σ = √(Σvariance), and a percentile as mean + `z`σ · the correlation
term `2Σρσᵢσⱼ`, and inverting a reserve into the confidence it buys · decision-tree rollback and the
value of information, with its breakeven price and breakeven prior · `p*` for a response: the cost at
which acceptance becomes correct · the adequacy ratio · an appetite statement converted into a
tolerance and a confidence level · `N̂ = n₁n₂/m` · PERT `tₑ` and σ, path σ, and a merge probability
both ways · and a monitor's precision against `cost ÷ saving`.

**Reflection questions.**
1. Take your project's contingency figure: what confidence level does it represent, who chose it,
   and what independence assumption sits underneath?
2. Group your register by common driver rather than by workstream. How concentrated is it actually?
3. Which decision changed last month because of your register — and if none, is the process
   decision support or documentation?
4. Write down the appetite statement your confidence level implies — "we accept an X % chance of
   exceeding by more than Y" — and consider whether your sponsor would sign it.
5. Take your last response decision. What were its secondary risks, and would pricing them have
   changed the choice?
6. Rank your register by variance contribution rather than by `EMV`. Which entry moves furthest, and
   is it being managed as though it matters that much?

## Domain 8 summary

Risk management earns its place only when it changes decisions. That starts at the sentence level:
cause → event → consequence, against a named objective, with opportunities identified as
deliberately as threats because defensive framing otherwise buries them — and it can be tested for
completeness rather than assumed complete, two independent methods and their overlap putting a floor
under what is missing (Meridian's **11.43** unidentified risks, **78.61 %** coverage) and giving
management reserve a basis at last. Analysis then progresses from qualitative screening — useful for
ordering, invalid as arithmetic, and coarser than it looks, since one matrix cell conceals an `EMV`
span equal to the product of its band ratios (**14.5833** here, **1.8452 times** the step between
cells two bands apart) — to quantification: `EMV` for each risk (Auriga's register totalling
**USD 278,000**, 6.95 % of `BAC`, and USD 314,000 if the opportunity is ignored, an overstatement of
12.95 %), with the total always belonging to **one objective** and always stated against that
objective's **tolerance** rather than its budget, which is how Meridian's comfortable-looking 4.96 %
of approved cost turns out to consume **99.17 %** of the room the board allowed.

Decision trees price choices and the **value of information** — the survey worth **USD 59,000**
against its 25,000 price, still worth buying at any price up to **USD 84,000**, and worthless if it
changed no action — and, once the signal is imperfect, the value depends on the **design** of the
enquiry as much as its price: Meridian's designed two-clinic pilot was worth **USD 77,136** against
the random pilot's **USD 17,947** at identical cost, and needed only an **11.05 %** prior to pay
against the random pilot's 21.45 %. Aggregation then converts a register into a **stated confidence**
— Auriga's P80 of **USD 490,624** from a mean of 278,000 and σ of 252,642, defensible precisely
because it says what confidence it buys, unlike a worst-case sum, an `EMV` sum or a 10 % rule of thumb
that turns out on enumeration to be a **P68.5** reserve. Two refinements matter more than the base
calculation. **Correlation moves the spread and not the mean**: one shared subcontractor takes
Auriga's σ to **USD 340,339** and makes that same 490,624 a **73.4 %** reserve, the label failing
before the money does. And because variance goes as `p(1−p)I²`, **impact levers release between 1.5
and 3 times the reserve of probability levers at equal `EMV` cost** — which is why Meridian's second
response bought **34.93 %** more P90 reduction for the same USD 25,000 while removing less `EMV`.

Responses are investments judged against the reduction they buy, priced across the families with
their **secondary risks** included, because omitting them is not an incompleteness but a selection
error: on Auriga's R1 the transfer looks best at 51,600 and is worst-but-one at 64,200 once a
counterparty default correlated with the event is priced, and the reduce option wins at 62,000.
Existential exposures are handled structurally rather than by averaging. Reserves are sized by
aggregation at a confidence level **derived from the appetite statement** rather than inherited —
"no more than a 10 % chance of exceeding by more than 5 %" is P90, and on Meridian it is unreachable
by any contingency decision — separated by authority, and tested each period against the register
still open, which showed Auriga's part-consumed reserve at an adequacy ratio of **0.9037** and a real
confidence of **75.65 %** while the conventional drawdown test passed. Retirement releases
variance, not `EMV`: R4's closure freed **USD 97,239** against an `EMV` of 60,000.

Beyond what can be named sits resilience — buffers, optionality, modularity, redundancy and fast
detection, bought at an efficiency cost that should be chosen rather than stumbled into, and bought
specifically where the consequence function has a **step** in it: Meridian's trainer retainer loses
USD 150 on expected value, misses its breakeven by two-tenths of a percentage point, and removes
**USD 184,776** of tail exposure, which is what decides it. Alongside sit the bias countermeasures
(pre-mortem, independent estimates) and **reference-class forecasting**, whose uplift is never added
to a bottom-up contingency but used to challenge it — Meridian's class of twelve giving a P80 uplift
of **USD 964,800** against a register-based **USD 203,785**, a gap of which about a third is
attributable to the unidentified risks the completeness estimate found and the rest to optimism and
scope growth that must be named as undecomposable. AI-enabled detection is governed by the same
arithmetic: an alert is worth investigating while precision exceeds `cost ÷ saving` — **4.09 %** on
Meridian, cleared nearly twice over by a monitor wrong 92 % of the time — and the direction of tuning
is decided by a crossover base rate (**1.3465 %**), not by preference. Applied to the schedule, the
same tools give merge bias its honest form: Auriga's node E has a **13.16 %** chance of its
deterministic date, not the 20.34 % of its critical path nor the 9.25 % of a naive product, and the
effect fades from a 35.31 % distortion at week 16 to 0.37 % by week 20, so a merge-bias figure without
a date is not a result. Crisis leadership closes the domain: stabilise, establish facts, decide
against a clock, communicate early. Independence is the assumption most likely to be convenient and
wrong: count drivers, not entries. Domain 9 turns to quality and the assurance that catches what risk
analysis missed.
