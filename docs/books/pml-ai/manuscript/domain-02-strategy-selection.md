# Domain 2 — Strategy, Selection and Business Alignment

## Why this domain exists

Domain 1 established that a project is judged on outcomes and benefits, not delivered outputs, and
proved it arithmetically. That leaves the prior question unanswered: **how does work get chosen at
all, and how does anyone know it was worth choosing?** This domain answers it. It sets the strategic
context and treats alignment as a test to be re-run rather than a box ticked at approval (KA 2.1),
then builds the business case as a decision instrument: the options analysis and appraisal that make
it one, and the selection models that rank competing candidates (KA 2.2). Benefits mapping,
measurement baselines and the sustainability dimension follow, with the assumption and dependency
management on which every forecast rests (KA 2.3). The domain closes on the hardest leadership act
in the discipline, **stopping** (KA 2.4). The through-line is that a business case is a *promise about the
future made to obtain money*, and the professional question is always whether anyone will be held to
it.

**Learning objectives.** After this domain a candidate can: explain how strategy becomes a
portfolio and why alignment decays; **compute a strategy-portfolio alignment index and the
reallocation distance it implies, on both a mapped and a total-spend denominator, and say which
denominator the finding belongs to**; **convert an annual supersession hazard into a survival
curve, an alignment half-life and an expected misaligned-spend figure, and price the re-test that
removes it**; describe the components of a decision-grade business case; construct and appraise a
genuine options set including the do-nothing baseline; **quantify a counterfactual do-nothing cost
and show how omitting it understates a case, in the opposite direction to the flat-profile error**;
build a benefits map from outputs through outcomes to benefits with owners and measures; **build a
benefits profile that ramps rather than assuming steady state from day one, and quantify the
overstatement a flat assumption produces**; **distinguish flat-equivalent from ramped-basis
breakeven adoption and convert between them**; define measurement baselines, **separate an
attributable improvement from a raw one with a comparison cohort, and state the invariant that
fixes the over-claim share**; avoid double-counting; **distinguish sustainability and social value as
a constraint, as a benefit and as a disclosed claim, and state the four provisions a disclosed claim
carries and who approves it**; rank candidates with weighted scoring and
under a binding constraint, and state each method's limits; **compute how far a criterion weight
must move to reverse a ranking, and which criteria can never decide anything**; **derive the
marginal value of a unit of the binding constraint by enumeration and show why it is lumpy and
non-monotone**; manage assumptions and dependencies as live risks, **price an assumption register
against the NPV it supports and order the tests by exposure per unit of test cost**; apply
sunk-cost-free reasoning to a continuation decision; **distinguish the forward breakeven at a gate
from the whole-investment breakeven and calibrate a kill criterion against the cost of its two
errors**; **decompose a staged commitment into the price of staging and the value of the
abandonment option, and state the probability of bad news at which staging begins to pay**; and set
kill criteria that make honest gates possible.

**The master programme.** Meridian Care Records continues from Domain 1: the shared clinical-records
rollout to **40 clinics**, whose verified benefit figures (full potential **USD 979,200** per year;
**USD 685,440** at the realistic 70 % adoption) are now used to build the business case that should
have been written for it.

**Reference points.** The portfolio material of KA 2.1 and the selection material of KA 2.2 have an
international reference point worth naming: **ISO 21504**, which addresses guidance on portfolio
management, sitting within the **ISO 21500** family Domain 1 introduced. Both are voluntary guidance
describing practice (neither is legislation and neither is a certifiable requirement), and neither
obliges anyone of itself unless an organisation, a contract or a regulator adopts it. They are named
here and not reproduced: no clause, table or checklist from either appears in this volume, the
treatment below is this book's own, and a reader who wants either document should obtain the current
edition from its publisher. Naming them implies no endorsement in either direction.

---

## Knowledge Area 2.1 — Strategic context and alignment

*Topics: 2.1.1 from strategy to portfolio · 2.1.2 drivers, constraints and environment ·
2.1.3 alignment as a repeated test.*

### 2.1.1 From strategy to portfolio

Strategy states intent; a portfolio is what an organisation is actually doing about it. The gap
between the two is where most strategic failure lives, and it is visible in three symptoms a leader
should be able to name: **a portfolio that does not reflect the stated priorities** (the strategy
says "digital first", the funded work is 80 % maintenance); **more work than capacity**, so
everything is slow and nothing is finished (Domain 15's capacity management); and **no route for
stopping**, so the portfolio only ever accretes.

The mechanism that closes the gap is a **portfolio process** with real teeth: candidate work
described comparably, ranked against explicit criteria, funded to capacity rather than to appetite,
and reviewed at intervals that allow re-decision. Where a project leader sits inside this, the
obligation is specific: **describe your work honestly enough to be compared**, including its
weaknesses, because a portfolio process fed by advocacy rather than evidence selects the best
storyteller.

The first symptom is the only one that can be measured directly from the ledger, and measuring it is
what converts a complaint into a governance paper.

**Worked example 2.1.1 — the alignment index, and the money it says must move.**

1. **Setup.** Meridian's parent health authority publishes four strategic objectives with declared
   weights (**access and capacity 40 %**, **safety and quality 25 %**, **digital foundation 20 %**,
   **cost efficiency 15 %**), and a discretionary programme budget of **USD 30,000,000** a year. The
   funded portfolio, read off the finance system and mapped objective by objective, is: access
   **4,800,000**; safety **6,600,000**; digital **1,800,000**; cost efficiency **4,800,000**; and
   **12,000,000** of mandatory sustainment (estate compliance, licence renewals, end-of-life
   replacement) that maps to no strategic objective at all. Compute how aligned this portfolio is,
   and what the answer implies in money.
2. **Formula.** For each objective, `funded share = spend ÷ denominator`. The **alignment index** is
   the overlap between the declared and funded distributions,
   `index = Σ min(declared weightᵢ, funded shareᵢ)`. The **reallocation distance** is the money that
   must move to close the gap, `Σ over under-funded objectives of (declared − funded) × denominator`,
   which is identically `(1 − index) × denominator`.
3. **Substitution.** On the strategically mapped **18,000,000**: shares
   `4,800,000/18,000,000 = 26.6667 %`, `6,600,000/18,000,000 = 36.6667 %`,
   `1,800,000/18,000,000 = 10.0000 %`, `4,800,000/18,000,000 = 26.6667 %`. Overlap
   `min(40, 26.6667) + min(25, 36.6667) + min(20, 10) + min(15, 26.6667)`. On the full
   **30,000,000**, shares are 16 %, 22 %, 6 %, 16 % and 40 % unmapped, and the declared weight on
   sustainment is zero.
4. **Result.**

   | Objective | Declared weight | Funded share of mapped spend | Gap | Money |
   |---|---|---|---|---|
   | Access and capacity | 40.0000 % | 26.6667 % | **−13.3333 pts** | **(2,400,000)** |
   | Safety and quality | 25.0000 % | 36.6667 % | +11.6667 pts | +2,100,000 |
   | Digital foundation | 20.0000 % | 10.0000 % | **−10.0000 pts** | **(1,800,000)** |
   | Cost efficiency | 15.0000 % | 26.6667 % | +11.6667 pts | +2,100,000 |

   **Alignment index on mapped spend 76.6667 %**; **on total discretionary spend 59.0000 %**.
   Reallocation distance **USD 4,200,000**; and the deficits (2,400,000 + 1,800,000) equal the
   surpluses (2,100,000 + 2,100,000) exactly, as they must.
5. **Interpretation.** Four readings, and the second is the one that decides whether this paper
   survives its first meeting.

   **The number is not the finding; the denominator is.** The two indices differ by **17.6667
   percentage points**, and the entire difference is the **40.0000 %** of discretionary money that
   serves no stated objective. Neither figure is wrong and neither is sufficient. Reported on mapped
   spend alone, the portfolio looks broadly aligned and the sustainment question never gets asked.
   Reported on total spend alone, the authority appears to be 41 % misaligned, which is unfair —
   sustainment is unstated strategy, not misalignment. The professional report gives both, and its
   recommendation is about the third figure: **either fund sustainment as a declared objective with
   a weight, or stop calling the other 60 % "the portfolio".** An organisation that will not put a
   weight on keeping the lights on has no portfolio process; it has a discretionary fund and a large
   unexamined subsidy.

   **The reallocation distance is the sentence that makes this actionable, and it has a scale a
   board already understands.** USD 4,200,000 is **1.75 times** Meridian's entire approved cost of
   2,400,000; and the access deficit alone, 2,400,000, is *exactly* one Meridian. That is the
   comparison to put in the paper, because "the portfolio is 76.6667 % aligned" invites a debate
   about the measure while "the access objective is short by one programme the size of Meridian"
   invites a decision. The digital objective is funded at **50.00 %** of its declared weight, which
   is the arithmetic behind the classic symptom named above: the strategy says digital first and the
   money says otherwise, by a factor of two.

   **The identity is worth holding.** Because the declared weights and the funded shares both sum to
   one, the positive gaps and the negative gaps are equal in magnitude, so `reallocation distance =
   (1 − index) × denominator` with no residue, here `0.233333 × 18,000,000 = 4,200,000`. One
   consequence: an index cannot be improved by adding money to a favoured objective without taking
   it from another, which is exactly the argument a portfolio board should be having and exactly the
   argument that "we will fund digital next year as well" avoids.

   **What the index cannot see, and where it is dangerous.** It is a measure of *money against
   declared intent*, not of value, deliverability or benefit. A portfolio can score 100 % and
   consist entirely of failing projects; it can score 60 % and be the best available set because the
   under-weighted objective has no fundable candidates, which is a **strategic coverage** finding
   (2.A.2), not an allocation one, and the two are routinely confused. The declared weights are
   themselves a governance artefact, often written for a strategy document rather than as spending
   targets, so the first question on seeing a low index is whether the weights were ever intended to
   bind. And the mapping of programmes to objectives is a judgement made by someone: a programme
   claiming three objectives can be counted once, split, or double-counted, and the index moves
   materially with that choice. State the mapping rule with the index, or the number is unauditable.
   Finally, the index is a **snapshot of commitments already made**; it changes slowly, because most
   of the portfolio is mid-flight. The lever that actually moves it is intake and stopping (KA 2.4),
   not exhortation.

> **Fig 2.1.1 — The strategy-portfolio alignment index.** Paired horizontal bars, one pair per
> objective, x-axis "share of the strategically mapped USD 18,000,000" from 0 to 45 %. For each
> objective the upper (slate) bar is the declared weight and the lower (brand blue) bar the funded
> share: access 40.0000 % / 26.6667 %, safety 25.0000 % / 36.6667 %, digital 20.0000 % / 10.0000 %,
> cost efficiency 15.0000 % / 26.6667 %. The two deficits are joined by a dashed crimson span. A
> right-hand panel prints the alignment index on mapped spend (**76.6667 %**), the index on all
> discretionary spend (**59.0000 %**), the unmapped sustainment share (**40.0000 %**) and the
> reallocation distance (**USD 4,200,000**, deficits equalling surpluses). Source: PCI original.
> Alt text: four pairs of horizontal bars comparing declared strategic weights with funded shares,
> two objectives under-funded and two over-funded, with the total money that must move printed
> alongside.

**Intake cadence is the other half of the process, and it has a price.** A portfolio board that
meets quarterly with a three-week paper deadline makes a candidate wait, on Domain 3's latency
formula `E[wait] = M/2 + L` (KA 3.2.3), an expected `13/2 + 3 =` **9.5 weeks** before it can be
decided at all; a monthly board with the same deadline makes it wait **5.0 weeks**. For a candidate
of Meridian's shape, whose cost of delay Domain 1 fixes at **USD 14,280 a week**, that is a
difference of `4.5 × 14,280 =` **USD 64,260** per candidate — paid before any work starts, and
invisible in every business case because no case has a line for the time it spent in a queue. The
counter-argument is real and must be stated: a board meeting more often decides on thinner evidence,
and a bad selection costs far more than 64,260. The resolution is not a cadence but a **threshold**
(small candidates decided continuously against pre-agreed criteria, large ones held for the full
board), which is Domain 3's delegation design applied to intake rather than to escalation.

### 2.1.2 Drivers, constraints and environment

A business case that does not name its **driver** cannot be tested. The families, and what each
implies:

| Driver | Success looks like | Failure mode if misidentified |
|---|---|---|
| **Compliance / regulatory** | The obligation is met by the date | Gold-plating a mandatory minimum |
| **Cost reduction** | Measured, sustained cost decrease | Cost moved rather than removed |
| **Revenue / growth** | New or protected income | Cannibalised existing revenue counted as new |
| **Capability / enabling** | A later benefit becomes possible | Benefits claimed twice, here and downstream |
| **Risk reduction** | Exposure measurably lower | Unquantified "insurance" spending |
| **Service / outcome improvement** | Users measurably better served | Output delivered, outcome unmeasured (Domain 1) |

**Constraints** (funding envelope, regulatory deadline, capacity, technical dependency) are not the
same as objectives, and conflating them corrupts option generation: a "constraint" that is actually
a preference silently eliminates the option that would have won. The professional habit is to write
constraints down and mark each **hard** (physics, law, contract) or **soft** (preference,
convention), because only soft ones can be traded.

### 2.1.3 Alignment as a repeated test

Alignment is granted at approval and **decays** thereafter: strategy moves, markets move, and the
project's own understanding of what it can deliver moves. A project can be perfectly executed and
strategically irrelevant by the time it lands, the most expensive form of success available.

Three practices keep alignment live. **Re-test at gates.** Each stage gate (Domain 3, KA 3.3.1) asks
not only "is delivery on track?" but "is this still the right thing?". **Track the assumption set**
rather than the conclusion: business cases fail through their assumptions (KA 2.3.4), and an
assumption that has been falsified invalidates the case whether or not delivery is green. **Name a
benefits owner outside the project** (usually the operational leader who will run the changed
service), because a project that owns its own benefits case will never be the one to report that it
has stopped making sense (Domain 1's accountability rule; Domain 16's realisation).

Decay is usually discussed as a mood. It is a hazard, and a hazard has a half-life.

**Worked example 2.1.3 — the alignment half-life, and what a re-test is worth.**

1. **Setup.** The health authority's planning team, reviewing eight years of its own board minutes,
   assesses that a stated strategic priority is superseded (replaced, merged or de-prioritised) at a
   rate of about **15 % a year**. This is a **locally calibrated planning figure, not a constant**;
   in a regulated utility on a five-year price control it would be far lower, and in a
   consumer-facing technology business far higher. Meridian is a **three-year** delivery spending
   **USD 800,000** a year. Compute how long alignment survives, what spend is incurred after the
   driver has gone, and what an annual alignment re-test at each gate is worth if it detects a
   superseded driver **80 %** of the time.
2. **Formula.** Survival of alignment `S(t) = (1 − h)ᵗ`. **Alignment half-life**
   `t½ = ln(0.5) ÷ ln(1 − h)`. Probability the driver is superseded during year `k` is
   `(1 − h)^(k−1) × h`. Expected misaligned spend is the probability-weighted spend incurred after
   supersession and before detection; with detection probability `d` at each subsequent gate, a
   supersession missed at `j` consecutive gates costs the spend of those `j` years.
3. **Substitution.** `S(1) = 0.85`, `S(2) = 0.85² = 0.7225`, `S(3) = 0.614125`.
   `t½ = ln(0.5)/ln(0.85)`. Without a re-test: `0.15 × 1,600,000 + 0.1275 × 800,000`. With a re-test:
   year-1 supersession costs `0.8 × 0 + 0.2 × 0.8 × 800,000 + 0.2² × 1,600,000 = 192,000` in
   expectation; year-2 supersession costs `0.2 × 800,000 = 160,000`.
4. **Result.**

   | | Year 1 | Year 2 | Year 3 | Year 4 | Year 5 |
   |---|---|---|---|---|---|
   | Probability the driver is still current | 85.00 % | 72.25 % | **61.41 %** | 52.20 % | 44.37 % |

   **Alignment half-life 4.2650 years.** Expected misaligned spend with **no** re-test `240,000 +
   102,000 =` **USD 342,000**: **14.25 %** of the programme's cost. With an annual alignment re-test
   at 80 % detection, `0.15 × 192,000 + 0.1275 × 160,000 =` **USD 49,200**. The re-test is therefore
   worth **USD 292,800**, and its **breakeven cost is USD 97,600 per gate** across the three gates.
5. **Interpretation.** Four readings, and the first is the one to remember because it is
   parameter-free once the hazard is stated.

   **The half-life is the sentence that changes how long programmes are allowed to be.** At a 15 %
   annual hazard, alignment is more likely than not to have survived a **four-year** delivery
   (52.20 %) and more likely than not to have failed a **five-year** one (44.37 %), with the crossing
   at **4.2650 years**. That is a design constraint on programme architecture, not a commentary on
   it: a delivery longer than its own alignment half-life is being asked to outlive the reason it
   exists, and the professional response is to tranche it so that something of value lands and is
   *judged* inside the half-life (Domain 15, KA 15.1.2 builds the tranche architecture; Domain 13's
   incremental delivery is the same instinct at iteration scale). Note what the half-life is *not*: a
   forecast that the strategy will change at year 4.27. It is the point at which the odds turn, which
   is the only thing a design decision needs.

   **The value of the re-test lives almost entirely in the early years.** A driver superseded during
   year 1 exposes two further years of spend; one superseded during year 3 exposes none. So of the
   342,000 of expected misaligned spend, **240,000 — 70.18 % — comes from the first year alone**, and
   a re-test that only starts at the second gate captures a fraction of the value. This inverts the
   usual gate emphasis, which tightens as the money grows: on *alignment*, the early gate is worth
   the most, because it is the only one with anything left to protect.

   **Detection quality is worth less than it looks, and this is the honest part.** Raising detection
   from 80 % to a perfect 100 % moves the expected misaligned spend from 49,200 to zero (a further
   **USD 49,200**), while introducing the re-test at all was worth 292,800, **5.95 times** as much.
   The large gain is in *asking the question*; the small gain is in asking it well. A leader who
   cannot get a rigorous strategic re-test into the gate pack should still get a crude one in: at `d
   = 0.5` the re-test is still worth **USD 201,000**, which is **58.77 %** of the 342,000 a perfect
   re-test would save, from a process amounting to one honest agenda item. The corresponding
   caution: the 80 % is a judgement about whether a committee reading a gate paper would notice that
   its own strategy had moved, and every organisation should suspect its own number is lower than it
   thinks.

   **The machinery is Domain 3's, the loss is not, and the difference matters.** Domain 3, KA 3.3.1
   prices a gate by its ability to detect a **delivery defect** and computes the remediation saved;
   this example prices the same gate by its ability to detect a **strategic** change and computes
   the irrelevant spend avoided. The two are additive (they detect different things and are not
   substitutes), which is why a gate paper that reports only delivery status is leaving the second
   value stream entirely uncollected. What the model omits is also worth naming: it assumes
   supersession is observable at a gate if looked for, that stopping on discovery is possible
   (Domain 3's decision rights, and the reason 2.4.1's "no route for stopping" symptom converts this
   value to zero), and that misaligned spend is a total loss, which overstates the case wherever the
   output has residual use. Where the residual value is material, replace the spend figure with the
   spend-less-salvage and the arithmetic runs unchanged.

### AI in this KA

Portfolio-level analysis is a legitimate AI application: normalising heterogeneous candidate
descriptions into comparable form, surfacing dependency clashes across dozens of cases, flagging
where a stated driver and the proposed benefits do not match. The governed limits are two.
**Strategic weighting is a governance judgment**, not an optimisation output, a model asked to
"optimise the portfolio" will faithfully optimise whatever proxy it was given, and the proxy is the
decision. And **advocacy is invisible to a model**: an AI reading business cases cannot tell an
evidenced benefit from a confident one, so it amplifies whatever the strongest writer submitted. AI
proposes; the professional verifies, decides and remains accountable.

**Verification, concretely.** The alignment index is where a model earns most of its keep and where
its output must be checked hardest, because the arithmetic is trivial and the **mapping** is not: a
model assigning programmes to objectives from their titles and summaries will produce a
defensible-looking index built on classifications nobody agreed. So publish the mapping rule with
the index, sample-check a stated proportion of the assignments by hand, and re-run the index under
the alternative treatment of any multi-objective programme; if the index moves materially, the
finding is about the mapping and not about the portfolio. The index itself is one addition of minima
and should be reproduced in a spreadsheet whose formulae are visible. For the decay arithmetic, the
supersession hazard must come from an organisation's own decision history, counted, and never from a
model's sense of how fast strategies usually change: a plausible-sounding hazard drives a half-life
that then drives programme architecture, which is a long way for an invented number to travel.

### Key terms — KA 2.1

| Term | Meaning |
|---|---|
| **Portfolio process** | Comparable description, explicit criteria, funding to capacity, periodic re-decision. |
| **Driver** | The reason the work exists; determines what success means and how it is measured. |
| **Hard / soft constraint** | Physics, law, contract vs preference and convention; only soft ones are tradeable. |
| **Alignment decay** | The erosion of strategic fit after approval, as strategy and understanding move. |
| **Alignment index** | `Σ min(declared weight, funded share)`, the overlap between stated priorities and funded spend. |
| **Reallocation distance** | The money that must move to close the gaps: `(1 − index) × denominator`; deficits equal surpluses. |
| **Supersession hazard** | The annual probability that a stated strategic priority is replaced, merged or de-prioritised. |
| **Alignment half-life** | `ln(0.5) ÷ ln(1 − h)`: the delivery duration beyond which alignment is more likely lost than kept. |
| **Benefits owner** | The accountable person outside the project who will realise the benefit. |

### Sample MCQs — KA 2.1

**MCQ 2.1-A `[2.1.3 · Analysis]`** A project is delivering to plan, but the strategic priority it
served has been superseded. The governance-sound response is:
- A. continue — the project is on track and the case was approved
- B. re-test the case at the next gate and decide on current strategy and remaining cost and benefit ✅
- C. continue but reduce scope proportionately
- D. transfer the project to another portfolio

*Rationale:* Alignment decays and gates exist to re-decide (2.1.3), on remaining cost and benefit
(KA 2.4.2). A treats approval as permanent; C is an arbitrary compromise that decides nothing;
D relocates the question without answering it.

**MCQ 2.1-B `[2.1.2 · Application]`** A case states its constraint as "must use the existing
platform", which reflects an architectural preference rather than a contractual or legal
requirement. The correct treatment is:
- A. accept it as a constraint, since the architects have stated it
- B. record it as a **soft** constraint, so it can be traded in option generation and its cost made visible ✅
- C. remove it from the case entirely
- D. re-classify it as an objective

*Rationale:* Only hard constraints (physics, law, contract) are untradeable; a soft one recorded as
hard silently eliminates options (2.1.2). C loses real information; D confuses a limit with a goal.

**MCQ 2.1-C `[2.1.1 · Application]`** Declared weights are 40 / 25 / 20 / 15 %; the strategically
mapped spend of USD 18,000,000 is funded 26.6667 / 36.6667 / 10.0000 / 26.6667 %. The reallocation
distance is:
- A. USD 8,400,000
- B. USD 4,200,000 ✅
- C. USD 2,400,000
- D. USD 12,000,000

*Rationale:* The positive deficits are `0.133333 × 18,000,000 = 2,400,000` and
`0.10 × 18,000,000 = 1,800,000`, totalling **4,200,000**, which equals `(1 − 0.766667) × 18,000,000`
(2.1.1). A adds the deficits to the surpluses, counting the same movement twice; C is the access
deficit alone; D is the unmapped sustainment line, which is a separate finding about the denominator.

**MCQ 2.1-D `[2.1.3 · Application]`** A strategic priority is superseded at an annual rate of 15 %.
The alignment half-life is:
- A. 6.6667 years
- B. 4.2650 years ✅
- C. 3.3333 years
- D. 4.6210 years

*Rationale:* `ln(0.5)/ln(0.85) = 4.2650` (2.1.3). A is `1/h`, the mean waiting time under a
different model, not the median; C halves linearly (`0.5/0.15`), ignoring compounding; D uses
`ln(0.5)/h`, treating the annual hazard as a continuous rate: a near miss that is systematically too
long.

**MCQ 2.1-E `[2.1.1 · Evaluation]`** An alignment index of 76.6667 % is reported on mapped spend
while the index on all discretionary spend is 59.0000 %, the difference being 40 % of spend that maps
to no objective. The soundest treatment is:
- A. report the higher figure, since sustainment is not discretionary in practice
- B. report the lower figure, since 40 % of the money is misaligned
- C. report both, and make the recommendation about the unmapped 40 % — either give sustainment a
  declared weight or stop describing the other 60 % as the portfolio ✅
- D. exclude sustainment and re-baseline the declared weights to the mapped spend

*Rationale:* The denominator is the finding (2.1.1). A hides the largest single item; B misnames
unstated strategy as misalignment; D quietly ratifies the omission by rebasing the weights to the
spend they were meant to test.

**MCQ 2.1-F `[2.1.1 · Comprehension]`** A portfolio scores 100 % on the alignment index. It is
therefore:
- A. well balanced, since every objective is funded to its declared weight
- B. correctly allocated against declared intent, which says nothing about risk profile, time to
  benefit, capability concentration or deliverability ✅
- C. optimal, since no money needs to move
- D. mis-measured, because a perfect score is not attainable in practice

*Rationale:* The index measures money against stated intent and nothing else (2.1.1, 2.A.2). A and C
read an allocation measure as a balance or optimality measure, a portfolio funded exactly to weight
can still be entirely high-risk, entirely long-dated and entirely dependent on one scarce team. D is
false: a perfect score simply means the funded shares match the declared weights.

### Self-check — KA 2.1

1. *Name the three symptoms of a strategy-portfolio gap.* — Funded work not reflecting priorities;
   more work than capacity; no route for stopping.
2. *Why must a benefits owner sit outside the project?* — A project owning its own benefits case
   will not be the one to report that the case has stopped making sense.
3. *What distinguishes a hard from a soft constraint?* — Physics, law or contract versus preference;
   only soft constraints can be traded.
4. *State the alignment-index identity and why it constrains the argument.* — Reallocation distance
   `= (1 − index) × denominator`, and deficits equal surpluses, so an objective can only be
   better funded by taking money from another.
5. *At a 15 % annual supersession hazard, what is the longest delivery whose alignment is more likely
   kept than lost?* — 4.2650 years; a five-year delivery has a 44.37 % chance of landing under the
   strategy that authorised it.
6. *Where does most of the value of an alignment re-test sit, and why?* — In the earliest gate:
   70.18 % of Meridian's expected misaligned spend arises from a year-1 supersession, because that is
   the only case with two further years of spend still to protect.

---

## Knowledge Area 2.2 — The business case and selection

*Topics: 2.2.1 the business case as a decision instrument · 2.2.2 options and appraisal ·
2.2.3 selection and prioritisation models.*

### 2.2.1 The business case as a decision instrument

A decision-grade business case answers six questions and nothing else: **why** (the driver and the
problem, with evidence); **what options** exist, including doing nothing; **what it costs**, with a
range and an accuracy class (Domain 7, KA 7.1.1); **what it delivers**, as outcomes and benefits
with owners and measures (KA 2.3); **what could go wrong**, with the exposure quantified
(Domain 8); and **how it will be judged**, with success criteria agreed in advance (Domain 1,
KA 1.A.2).

Its two failure modes are worth naming precisely. A **advocacy document** answers only "why" and
"what it delivers", omitting ranges, options and risk: designed to obtain approval rather than
support a decision. A **compliance artefact** answers all six in a template nobody reads, produced
after the decision has been taken informally. Both are common; the test that separates them from the
real thing is whether the case **could have concluded "no"**.

**The living-case principle.** The case is not filed at approval; it is the instrument the gates
re-test (2.1.3) and the source of the benefits register Domain 16 tracks. A business case whose
figures no longer match the project is an unmanaged forecast, not a historical document.

### 2.2.2 Options and appraisal

**A genuine options set** includes the **do-nothing / do-minimum baseline** (against which all value
is measured: without it, benefits are unmeasurable), at least one option that is materially cheaper
than the preferred one, and at least one that is materially more ambitious. An options set
constructed to make the preferred option look inevitable (the "straw options" pattern) is the
advocacy failure mode in structural form, and reviewers spot it by checking whether any option could
plausibly have won.

**Appraisal** applies the discounted machinery PFL-AI Domain 4 builds in full: NPV as the primary
value measure, with the range and sensitivity that make it honest. This book uses the results; the
finance book derives them. What belongs to a delivery leader is the **benefits profile**, because
that is where most business cases are wrong, and the error is systematic.

**Worked example 2.2.2 — the Meridian business case, twice.**

1. **Setup.** Meridian costs **USD 2,400,000** to deliver. Domain 1 established the arithmetic: full
   potential at 100 % adoption is **USD 979,200** per year; realistic steady-state adoption is **70
   %**, worth **USD 685,440** per year. The benefits are appraised over **8 years** at a **7 %**
   discount rate. Compare the case as commonly written (full potential from year one) with an honest
   **ramped** profile: 40 % adoption in year 1, 60 % in year 2, 70 % thereafter.
2. **Formula.** Flat case: `PV = annual benefit × AF(r, n)`. Ramped case:
   `PV = Σ (potential × adoptionₜ) / (1 + r)ᵗ`. NPV = PV − cost.
3. **Substitution.** `AF(0.07, 8) = 5.971299`. Flat: `979,200 × 5.971299`. Ramped:
   `391,680/1.07 + 587,520/1.07² + 685,440 × (years 3–8 discounted)`.
4. **Result.**

   | | Year 1 | Year 2 | Years 3–8 | PV of benefits | NPV |
   |---|---|---|---|---|---|
   | **Flat, full potential** | 979,200 | 979,200 | 979,200 | 5,847,096 | **+3,447,096** |
   | **Ramped, 70 % steady state** | 391,680 | 587,520 | 685,440 | 3,732,898 | **+1,332,898** |

   The flat case overstates NPV by **USD 2,114,198**: **158.6 %** of the honest figure.

5. **Interpretation.** Both cases approve the programme, which is precisely why the error survives:
   the decision is unchanged, so nobody checks, and the *promise* is what gets remembered. Meridian
   was later judged a failure against benefits it never could have produced (Domain 1's case study),
   and this table is where that judgement was actually created: at approval, two years before anyone
   noticed. Five further readings follow, and the third is the one most business cases get subtly
   wrong even after they have fixed the profile.

   **The ramp is not pessimism.** It is the adoption curve of the *same* 70 % figure, merely
   arriving when it actually arrives. Nothing in the honest column is a more cautious assumption
   than the flat column; the two differ only in *when* the identical steady state is reached. That
   is why the flat profile is not a judgement call a reasonable person might defend. It is an
   arithmetic claim that adoption is instantaneous, which nobody would sign if it were written in
   words.

   **The ramp costs a fixed fraction of value, and the fraction is worth knowing.** The ramped PV of
   3,732,898 is **91.2027 %** of the flat PV at the same 70 % steady state
   (`0.70 × 5,847,096 = 4,092,967`), so the two ramp years give up **8.7973 %** of the discounted
   value of the whole benefit stream. Because the profile 40 / 60 / 70 is the steady state scaled by
   `4/7, 6/7, 1`, that fraction is **independent of the steady-state level**: halve the adoption and
   both figures halve. A leader can therefore correct any flat case for a two-year ramp of this shape
   by multiplying by 0.912027, in their head, in the meeting.

   **Breakeven adoption comes in two flavours, and the difference is 3.9592 points.** The
   **flat-equivalent breakeven is 41.05 %**: `2,400,000 / 5,847,096`: the level a benefit stream
   *flat from year one* would have to reach. The **ramped-basis breakeven is 45.01 %** (`2,400,000 /
   5,332,711`, where 5,332,711 is the PV of the ramped profile per unit of sustained adoption) the
   *steady state* a proportionally ramped profile must reach. Both are correct answers to different
   questions, and reporting the first while meaning the second understates the requirement by nearly
   four points. This book uses the flat-equivalent figure as its headline, because it is comparable
   across profiles, and states the basis every time. **A breakeven without its basis is not a
   number.**

   **The breakeven, not the NPV, is the board sentence.** "This programme creates value at any
   sustained flat-equivalent adoption above 41.05 %" states the condition value depends on, in a unit
   the board can monitor monthly, and it survives every argument about the discount rate that an NPV
   invites. It also gives the sponsor a *falsifiable* commitment, which an NPV never does: an NPV can
   only be wrong in retrospect, while a breakeven can be tested every month from go-live.

   **What breaks the whole example.** Three things. The 8-year horizon is a choice, and a benefit
   stream that in fact decays (as clinical workflows change and the released hours are re-absorbed)
   is not captured by a flat tail; where decay is plausible, profile it and let the horizon fall
   out. The 7 % rate is an input the leader usually does not own, and the industry variations below
   compute what a point of it is worth. And the cost side is treated as a point estimate, which
   Domain 7, KA 7.1.1 forbids: the same table drawn with the cost at the top of its accuracy range
   moves every breakeven, and a case that ranges its benefits but not its costs has ranged the wrong
   side.

> **Fig 2.2.1 — Two Meridian business cases from identical facts.** Grouped bar chart, x-axis
> years 1–8, y-axis annual benefit USD 0–1.05m. Series A (grey) flat at 979,200 across all eight
> years. Series B (brand blue) ramping 391,680 → 587,520 → 685,440 and level thereafter. The area
> between them shaded and annotated "USD 2,114,198 of present value promised and never deliverable
> — 158.6 % of the honest NPV". Inset text: breakeven sustained adoption 41.05 %. Source: PCI
> original. Alt text: two benefit profiles over eight years, one flat at full potential and one
> ramping to a seventy per cent steady state, with the gap between them shaded as overstated value.

**The other half of the appraisal is the thing the project is being compared with**, and it is
almost never zero. "Do nothing" is a course of action with its own cash flows: contracts that
expire, equipment that fails, workarounds whose cost grows, obligations that fall due anyway. Where
those costs are omitted, the case understates its own value, which makes this the mirror image of
the flat-profile error, and the reason a reviewer must look for errors in *both* directions rather
than assuming every case is inflated.

**Worked example 2.2.2b — the do-nothing baseline that was not zero.**

1. **Setup.** Meridian's approved case compared the programme against a do-nothing option valued at
   **zero**. Two facts were available at the time and omitted. First, the legacy records system's
   vendor support ends at the end of **year 3**; continuing without Meridian requires a mandatory
   re-platforming of the legacy estate costing **USD 600,000**, falling in **year 4**. Second, the
   manual reconciliation the legacy system requires between clinics grows as clinic numbers rise,
   adding **USD 40,000 a year in years 5 to 8**. Both are avoided if Meridian proceeds. Same horizon,
   same 7 % rate. Restate the case.
2. **Formula.** `Incremental NPV = PV(project benefits) − project cost + PV(costs avoided in the
   do-nothing case)`. Equivalently, the do-nothing option is appraised in its own right and the
   project's value is the difference: `NPV(project) − NPV(do nothing)`. The flat-equivalent breakeven
   adoption becomes `(cost − PV of avoided cost) ÷ PV of full-potential benefits`.
3. **Substitution.** `600,000 / 1.07⁴ = 600,000 × 0.762895`. `40,000 × (AF(0.07,8) − AF(0.07,4)) =
   40,000 × (5.971299 − 3.387211) = 40,000 × 2.584087`. Breakeven
   `(2,400,000 − 561,101) / 5,847,096`.
4. **Result.**

   | Component | Undiscounted | Present value |
   |---|---|---|
   | Legacy re-platforming avoided (year 4) | 600,000 | **457,737** |
   | Manual reconciliation avoided (years 5–8) | 160,000 | **103,363** |
   | **Do-nothing cost, and therefore value created by avoiding it** | **760,000** | **561,101** |

   Incremental NPV rises from **+1,332,898** to **+1,893,998**: an uplift of **USD 561,101**, or
   **42.10 %** of the stated NPV. Flat-equivalent breakeven adoption falls from **41.0460 %** to
   **31.4498 %**, an improvement of **9.5962 percentage points**.
5. **Interpretation.** Five readings, and the third is the most consequential sentence in this
   domain.

   **A zero do-nothing baseline is a claim, and usually a false one.** It asserts that the current
   arrangement can continue indefinitely at its current cost. That is true of very little: support
   contracts expire, regulation tightens, volumes grow, and the workaround that costs nothing today
   costs a post holder tomorrow. The professional discipline is to appraise the do-nothing option
   with the same rigour as the preferred one (its own cost profile, its own risks, its own owner),
   and to record that appraisal, because that is what makes the comparison a comparison rather than
   an assertion.

   **The error runs in the opposite direction to the flat profile, which is why both must be looked
   for.** The flat profile flattered Meridian by 2,114,198 of present value; the missing
   counterfactual penalised it by 561,101. A reviewer who only knows that cases are optimistic will
   find the first and not the second, and will then be wrong in a way that is harder to correct;
   because arguing that a case is *understated* costs credibility that arguing the reverse does not.
   Net across the two corrections, Meridian's honest NPV is `3,732,898 − 2,400,000 + 561,101 =`
   **+1,893,998**, which is **54.94 %** of the 3,447,096 the approved case advertised. The case was
   wrong twice and still positive.

   **Meridian's actual outcome sits between the two breakevens, and that is the whole lesson.** At
   its achieved flat-equivalent adoption of **40 %**, the programme's NPV on the case as written is
   `0.40 × 5,847,096 − 2,400,000 =` **(USD 61,162)**: a small loss, and the arithmetic that made a
   public failure verdict defensible. On the honest baseline it is `−61,162 + 561,101 =` **+USD
   499,939**: a clear success. **The same programme, the same 40 % adoption, the same cost:
   value-destroying or value-creating according to whether anyone appraised the do-nothing option.**
   The judgement passed on Meridian was not a measurement of the programme. It was a measurement of
   its business case.

   **Do-minimum is usually the honest baseline, not do-nothing, and the difference is not
   presentational.** Pure inaction is often not permissible (the support contract cannot simply be
   allowed to lapse on a clinical system), so the correct comparator is the **cheapest compliant
   alternative**, here the 600,000 re-platforming on its own. Appraised that way, the 600,000 moves
   from "avoided cost" to "the do-minimum option's cost", and the incremental NPV is identical; what
   changes is that the options set now contains a genuine third option that could have won on a low
   adoption forecast. **Framing the counterfactual as an option rather than as an adjustment is what
   makes the options set genuine** (2.2.2's straw-options test), and it is the better habit for
   exactly that reason.

   **What breaks it, and where to be careful.** Avoided costs must be *genuinely* avoided and not
   merely deferred: if the legacy re-platforming would still be needed for another system on the
   same estate, nothing has been avoided and the 457,737 is fictitious, the same single-claimant
   discipline KA 2.3.2 applies to benefits, applied to costs. The avoided cost must not also be
   counted as a benefit elsewhere in the case, which is the commonest form of the double count here.
   The timing carries real weight: the 600,000 is worth 457,737 at year 4 and would be worth
   **560,748** at year 1, so a vaguely dated obligation is a materially different number, and the
   year is a fact to be evidenced from the contract rather than assumed. And an avoided cost is not
   cash released unless the budget holding it is actually removed: 2.3.2's cash-releasing test
   applies with full force, because a finance function that never held a provision for the 600,000
   will not recognise its avoidance. **This is an area where the accounting and budgetary treatment
   differs materially between organisations and jurisdictions; where the case turns on it, agree the
   treatment with the finance function in writing before the paper is written.**

### 2.2.3 Selection and prioritisation models

**Weighted scoring** ranks candidates against criteria carrying explicit weights. Its value is that
it forces the criteria and weights into the open, where they can be argued; its limits are that
scores are **ordinal judgments** (the arithmetic caution of Domain 8, KA 8.2.1 applies) and that
weights are chosen by whoever runs the model. So so the model can be steered by anyone who
understands it.

Which makes **who scores** a governance question rather than an administrative one. A scoring panel
is a decision body, so the declaration duty of Domain 1, KA 1.2.2a applies to it in full: each
scorer identifies any interest in the candidates before the criteria are issued, declares it in
writing to the chair, and takes no part in scoring the candidate concerned, with the record naming
who scored in their place. The interest to watch for here is rarely financial. It is usually a
sponsoring or delivering role in one of the candidates, which is precisely the person whose subject
knowledge makes them worth having on the panel. Note the sequence, because it is the part that gets
reversed under time pressure: interests are declared and the weights are fixed **before** any
candidate is scored, since a weight set after the scores are visible is a way of choosing the winner
while appearing to apply a model (Domain 10, KA 10.2.3 makes the same point for bids, where the
stakes are contractual).

**Worked example 2.2.3 — four candidates, two rankings.**

1. **Setup.** Four candidate programmes scored 1–5 against weighted criteria: strategic fit
   (0.35), benefit value (0.30), deliverability (0.20), risk — inverse, so higher is safer (0.15).
2. **Formula.** Weighted score = Σ(weight × score). Then, under a binding constraint, rank by
   **NPV per unit of the scarce resource** instead.
3. **Substitution and result.**

   | Candidate | Fit | Benefit | Deliver. | Risk | **Weighted score** |
   |---|---|---|---|---|---|
   | Meridian | 4 | 5 | 3 | 3 | **3.95** |
   | Beta | 5 | 3 | 4 | 4 | **4.05** |
   | Gamma | 3 | 4 | 5 | 4 | **3.85** |
   | Delta | 2 | 2 | 5 | 5 | **3.05** |

   Now suppose the binding constraint is **integration-team capacity**, of which only 3 units exist:
   Meridian needs 3 units for an NPV of 1,693,072; Beta 2 units for 1,200,000; Gamma 1 unit for
   900,000.

   | Candidate | NPV | Capacity | **NPV per unit** |
   |---|---|---|---|
   | Meridian | 1,693,072 | 3 | **564,357** |
   | Beta | 1,200,000 | 2 | **600,000** |
   | Gamma | 900,000 | 1 | **900,000** |

4. **Interpretation.** Two defensible methods give two different answers, and a leader who can say
   why commands the selection meeting. Four readings.

   **The constraint changes the answer, not merely the ordering.** The scoring model ranks **Beta
   first** (4.05 against Meridian's 3.95) on strategic fit and deliverability. Under the capacity
   constraint, **Beta + Gamma** together consume the 3 units for a combined NPV of **2,100,000**,
   beating Meridian's 1,693,072 alone, so the constrained answer is to run the two smaller
   programmes, which neither the raw NPV ranking nor the scoring model would have selected. This is
   the delivery-side twin of PFL-AI's capital rationing (its Domain 4, KA 4.3.2). The leadership
   content is that **the binding constraint decides the method**, and naming that constraint
   honestly (usually a scarce team, not money) is the whole game.

   **Enumeration is cheap and greedy ranking is not safe.** The feasible sets under 3 units are
   exactly `{}`, `{Gamma}` = 900,000, `{Beta}` = 1,200,000, `{Meridian}` = 1,693,072 and `{Beta,
   Gamma}` = **2,100,000**; `{Meridian, Beta}` needs 5 units and `{Meridian, Gamma}` needs 4, so
   both are infeasible. Here greedy ranking by NPV per unit (Gamma at 900,000, Beta at 600,000,
   Meridian at 564,357) happens to reach the optimum, and that is luck rather than method. Had a
   fifth candidate **Epsilon** needed all 3 units for an NPV of **2,200,000** (733,333 per unit),
   greedy would still have taken Gamma first and finished with Beta + Gamma at 2,100,000, missing
   the best set by **USD 100,000**. With `n` candidates there are `2ⁿ` subsets, which for the ten or
   twenty candidates a real portfolio board considers is a spreadsheet exercise, not a research
   problem. **Enumerate; rank only to explain the answer afterwards.**

   **Divisibility is the assumption that decides whether any of this is legitimate.** Ranking by
   value per unit is the correct optimum when candidates are *divisible*, when half of Beta can be
   bought for half the money and half the value. Projects are almost never divisible in that sense,
   which is what makes them lumpy and what makes greedy unsafe. Where a candidate genuinely can be
   **staged**, it becomes partly divisible and the arithmetic improves; that is one of the
   underrated benefits of the tranching argued for in 2.1.3 and priced in 2.A.1, and it belongs in
   the selection paper rather than being discovered later.

   **What no model does.** Scoring and ratios cannot see option value, sequencing dependencies, or
   the strategic cost of *not* doing something. They order candidates; they do not decide. A
   portfolio board that treats a scoring output as the decision has automated its own accountability
   (Domain 1, KA 1.2.1). Two further blindnesses are specific to the constrained form: it takes the
   constraint as given, when the most valuable move is often to *relax* it (priced immediately
   below), and it assumes the candidates are independent, when Gamma may be the enabler on which
   Meridian's benefits depend, in which case the sets are not free to be chosen (Domain 15, KA
   15.1.3 handles dependent candidates).

**Worked example 2.2.3b — what one unit of the binding constraint is worth.**

1. **Setup.** The three candidates above compete for one scarce integration team measured in
   **units**: Meridian needs 3 units for an NPV of 1,693,072; Beta needs 2 for 1,200,000; Gamma
   needs 1 for 900,000. The portfolio director can grow or shrink the team, and a unit of durable
   integration capacity (a qualified engineer with the platform knowledge, recruited and retained)
   costs **USD 400,000** a year all-in. How much capacity should the organisation hold?
2. **Formula.** For each capacity level `c`, solve `V(c) = max Σ NPVᵢ subject to Σ unitsᵢ ≤ c` by
   enumeration. The **marginal value of the nth unit** is `V(n) − V(n−1)`. Buy `k` further units when
   `V(c + k) − V(c) > k ×` unit cost.
3. **Substitution.** At `c = 3`, the feasible maximum is `{Beta, Gamma}` = 2,100,000. At `c = 4`, the
   admissible sets include `{Meridian, Gamma}` = `1,693,072 + 900,000`. At `c = 5`,
   `{Meridian, Beta}` = `1,693,072 + 1,200,000`. At `c = 6`, all three fit.
4. **Result.**

   | Capacity | Optimal set | Portfolio NPV | Marginal value of that unit |
   |---|---|---|---|
   | 1 | Gamma | 900,000 | **900,000** |
   | 2 | Beta | 1,200,000 | **300,000** |
   | 3 | Beta + Gamma | 2,100,000 | **900,000** |
   | 4 | Meridian + Gamma | 2,593,072 | **493,072** |
   | 5 | Meridian + Beta | 2,893,072 | **300,000** |
   | 6 | Meridian + Beta + Gamma | 3,793,072 | **900,000** |

   From a base of 3 units, at 400,000 a unit: **+1 unit** gains 493,072 for 400,000 (net
   **+93,072**; **+2 units** gains 793,072 for 800,000) net **(6,928)**; **+3 units** gains
   1,693,072 for 1,200,000 — net **+493,072**.
5. **Interpretation.** Four readings, and the second contradicts the way capacity decisions are almost
   always taken.

   **There is no such thing as "the value of a unit of integration capacity".** The marginal value
   runs 900,000 · 300,000 · 900,000 · 493,072 · 300,000 · 900,000. It is **lumpy and non-monotone**,
   and it is a property of the *candidate set*, not of the team. Anyone who quotes a single figure
   for what a scarce team is worth has averaged away the only information in the answer. The same
   applies to the shadow prices a solver reports: they are valid for the set that was solved and
   change when a candidate is added, withdrawn or re-estimated.

   **The right answer here is to add one unit or three, and never two — which no incremental process
   would find.** Adding units one at a time, each judged on its own, accepts the first (it gains
   493,072 for 400,000, a net **+93,072**) and then rejects the second, which gains only 300,000 for
   400,000 — a net **(100,000)** on its own, and (6,928) taken as a block of two. So the process
   stops there and never reaches the block of three, whose net is **+493,072**: **5.30 times** the
   gain the incremental process settles for. Capacity decisions are non-convex because the
   candidates are lumpy, so they must be taken as a **block decision against enumerated plans**,
   which is precisely what an annual headcount round asking each manager to justify the next post
   cannot do. This is the arithmetic behind Domain 15's treatment of enterprise capacity as a
   portfolio-level decision (KA 15.3) rather than a departmental one.

   **The comparison the portfolio board should actually be shown.** Not "should we hire?" but a
   table of feasible plans with their NPVs and their capacity costs, because on these figures the
   question "what is the best use of 1,200,000?" has the answer "three integration engineers", which
   no capital proposal would ever have contained. Note also the direction that is usually forgotten:
   at `c = 2` the marginal unit is worth only 300,000, **below its 400,000 cost**, so an
   organisation sitting at 2 units with these candidates should shrink rather than hold: a
   conclusion the same table produces and no advocacy paper ever reaches.

   **What breaks it.** The unit cost must be the **durable** cost of holding capacity, not a
   contractor day rate for a peak, and if the capacity can be rented for a season the whole question
   changes shape (Domain 7, KA 7.4 prices resource acquisition modes; Domain 10 prices the
   make-or-buy). Recruitment lag matters and is absent here: three engineers who arrive in nine
   months do not enable a decision taken today, and Domain 1's reinforcement-trough arithmetic
   prices the transient they bring with them. The NPVs are point estimates on candidates whose own
   ranges overlap, so a marginal value of 300,000 is not reliably different from one of 493,072. And
   capacity has value beyond this candidate set (optionality against next year's candidates, and
   resilience), which this model prices at zero and Domain 15, KA 15.3 prices properly as protective
   capacity.

**Worked example 2.2.3c — how far must a weight move to reverse the ranking?**

1. **Setup.** The same four candidates and the same weighted-scoring model: strategic fit 0.35,
   benefit value 0.30, deliverability 0.20, risk-inverse 0.15, scores on a 1–5 scale. Beta leads
   Meridian by 4.05 to 3.95. Two people who both accept the scores disagree about the weights: one
   believes strategic fit should carry less and benefit value more. **How much less?**
2. **Formula.** Shift `δ` of weight from criterion `a` to criterion `b`, holding the rest fixed. A
   candidate's total changes by `δ × (score_b − score_a)`. The ranking flips where `δ × [(scoreᵦ_b −
   scoreᵦ_a) − (score_M_b − score_M_a)] =` the current margin. Separately, the most any criterion
   can move a total (its **criterion influence**) is `(score range) × weight`.
3. **Substitution.** Meridian gains `δ(5 − 4) = +δ`; Beta gains `δ(3 − 5) = −2δ`. The margin is 0.10,
   so `δ + 2δ = 0.10`.
4. **Result.** `δ =` **0.033333**, i.e. **3.3333 percentage points** of weight. At fit 0.316667 and
   benefit 0.333333 both candidates score exactly **3.983333**. Criterion influence, on a 1–5 scale
   (range 4): fit **1.40**, benefit **1.20**, deliverability **0.80**, risk **0.60**, summing to the
   full 4.00 range of possible totals. The four candidates' actual totals span only **1.00**, which
   is **25.00 %** of that maximum swing.
5. **Interpretation.** Five readings, and the last is the governance conclusion the arithmetic forces.

   **A 3.33-point weight shift reverses the portfolio's top candidate, and 3.33 points is inside the
   noise of any weighting workshop.** Nobody can defend 0.35 against 0.3167 for strategic fit on any
   evidence; the two numbers are indistinguishable as statements of priority. So the model's answer
   here is not robust, and reporting "Beta ranks first" without reporting that it takes 3.33 points to
   change that is a material omission. The reviewer's question is exact: **what is the smallest
   defensible change of weight that reverses this ranking?**

   **The score side is worse.** Meridian needs only **+0.50** on deliverability (3 to 3.5) to draw
   level, because deliverability carries a weight of 0.20 and `0.10/0.20 = 0.50`. On a 1–5 ordinal
   scale nobody can distinguish 3 from 3.5, which is Domain 8, KA 8.2.1's arithmetic caution about
   ordinal scales in its sharpest form: **the model is being asked to resolve a difference finer
   than its own inputs can express.** On the risk criterion, weighted 0.15, the required move is
   **+0.6667**, and the general rule falls out: the score movement needed to close a gap is `margin
   ÷ weight`, so the lightly weighted criteria are the ones where scoring error does least damage.

   **Some weight shifts can never flip a ranking, and knowing which is free information.** Moving
   weight between strategic fit and *deliverability* changes Meridian's total by `δ(3 − 4) = −δ` and
   Beta's by `δ(4 − 5) = −δ`, identically, so the 0.10 margin is invariant to that trade no matter
   how large it is. A weighting argument between those two criteria is therefore not an argument
   about this decision at all, and a chair who knows it can stop the discussion. The pairs that
   matter are the ones where the two candidates' score *differences* differ.

   **Criterion influence tells you what the model cannot decide.** With a 1–5 scale, the risk
   criterion at 0.15 can move a total by at most **0.60** across its whole range. If the real
   disagreement in the room is about risk, and the candidates' risk scores differ by 1 point (worth
   0.15), the model literally cannot express it: `0.15` against a spread of totals of 1.00. The
   remedy is not a bigger weight but a different instrument, quantified exposure per Domain 8, KA
   8.2.2, reported beside the score rather than compressed into it.

   **Scoring models compress, and compression is what makes them steerable.** The four candidates
   occupy 25.00 % of the available range, because a good candidate scores well on some criteria and
   badly on others and weighting averages the differences away. Compression is not a defect to be
   engineered out (it is the honest consequence of averaging), but it has a governance implication:
   **when the outputs are close, the weights decide, and the weights are chosen by whoever runs the
   model.** The countermeasures are procedural and cheap: agree and minute the weights *before* the
   candidates are scored, publish the flip point with the ranking, and where two candidates lie
   within the flip point of each other, report them as **not separated by this method** and decide
   on something else: constraint economics (2.2.3b), option value (2.A.1) or portfolio balance
   (2.A.2). A model that cannot separate two candidates has still done useful work by saying so.

### AI in this KA

Business-case drafting is now routinely AI-assisted, and this KA identifies the specific hazard: a
model will produce a **fluent, complete-looking case with a flat benefits profile**, because that is
what most cases in its training data look like. It will also generate plausible options sets that
are structurally straw. The verification duty is therefore concrete: check the benefits profile
ramps, check at least one option could have won, check every benefit traces to a measured outcome
with an owner (Domain 1's benefits-chain test), and check the range and class on the cost. An
AI-drafted case that passes those four checks is genuinely useful work; one that has not been
checked against them is the advocacy document of 2.2.1 with better formatting.

Four further checks belong on that list once this KA's arithmetic is available, and each catches a
defect the four above do not. **A do-nothing option valued at zero** is as characteristic of
generated cases as a flat profile, and for the same reason (most cases in the corpus do it), so ask
the model explicitly what doing nothing costs in each year of the horizon, and then verify each item
against a contract, licence or asset register rather than accepting the list. And **a breakeven
quoted without its basis** is a near-certainty in generated text, because the distinction between
flat-equivalent and ramped-basis is finer than the language usually carries; recompute it and label
it. Where a model has been asked to run a scoring or constrained-selection model, the specific
verification is the **flip point** and the **enumeration**: a model will produce a ranking with
total confidence and will not volunteer that a 3.33-point weight shift reverses it, and a model
asked to "optimise" a constrained selection will frequently return the greedy per-unit answer, which
is not the optimum. Both are checkable in a spreadsheet in minutes, and both change recommendations.

### Key terms — KA 2.2

| Term | Meaning |
|---|---|
| **Do-nothing baseline** | The comparison against which all benefits are measured; without it value is unmeasurable. |
| **Do-minimum option** | The cheapest compliant alternative where inaction is not permissible; the honest comparator, and an option in its own right. |
| **Counterfactual cost** | Cost incurred *only* in the do-nothing case, and therefore value created by avoiding it. |
| **Straw options** | An options set built so the preferred option cannot lose. |
| **Benefits profile** | The time-phased benefit stream; ramps in reality, flat in most business cases. |
| **Breakeven adoption** | The adoption level at which NPV is zero, a more useful board sentence than NPV. |
| **Flat-equivalent basis** | Breakeven stated as the level a profile flat from year one would need (Meridian 41.05 %). |
| **Ramped basis** | Breakeven stated as the *steady state* a proportionally ramped profile must reach (Meridian 45.01 %). |
| **Weighted scoring** | Ranking by Σ(weight × ordinal score); forces criteria into the open, steerable by whoever sets weights. |
| **Value per unit of constraint** | Ranking basis when a scarce resource binds; a heuristic, not an optimum. |
| **Marginal value of the constraint** | `V(n) − V(n−1)` from enumeration; lumpy, non-monotone, and a property of the candidate set. |
| **Flip point** | The smallest weight shift that reverses a ranking; report it with the ranking or the ranking is unqualified. |
| **Criterion influence** | `(score range) × weight`: the most a criterion can move a total, and therefore what the model cannot decide. |

### Sample MCQs — KA 2.2

**MCQ 2.2-A `[2.2.2 · Application]`** Full benefit potential is 979,200 per year; steady-state
adoption is 70 %; the profile ramps 40 %/60 %/70 % and is appraised over 8 years at 7 %
(`AF` = 5.971299). Against a flat full-potential case, the ramped NPV is lower by:
- A. USD 979,200
- B. USD 2,114,198 ✅
- C. USD 1,332,898
- D. USD 293,760

*Rationale:* Flat PV 5,847,096 less ramped PV 3,732,898 = **2,114,198** of overstated present
value. C is the honest NPV itself; A is one year's potential; D is Domain 1's single-year
output-based overstatement, a different figure.

**MCQ 2.2-B `[2.2.2 · Analysis]`** Both the flat and ramped Meridian cases produce a positive NPV
and the same approval decision. Why does the flat case still matter?
- A. it does not — the decision is unchanged, so the error is harmless
- B. because the case becomes the promise the programme is later judged against, and benefits that were never deliverable are recorded as failure ✅
- C. because the discount rate must be recalculated
- D. because the flat case understates cost

*Rationale:* The approval is identical; the *commitment* is not (2.2.2, and Domain 1's case study
where Meridian was publicly called a failure). A is the reasoning that let the error survive.

**MCQ 2.2-C `[2.2.3 · Analysis]`** Integration capacity of 3 units is the binding constraint.
Meridian needs 3 units for NPV 1,693,072; Beta 2 for 1,200,000; Gamma 1 for 900,000. The
value-maximising selection is:
- A. Meridian alone — the highest single NPV
- B. Beta + Gamma — combined NPV 2,100,000 within the 3-unit constraint ✅
- C. Meridian + Gamma — the two highest NPVs
- D. all three, phased across two years

*Rationale:* Beta and Gamma together fit the constraint and beat Meridian's 1,693,072. C needs
4 units; D changes the premise rather than answering under it; A ignores the constraint's
implications.

**MCQ 2.2-D `[2.2.1 · Recall]`** The clearest test that a business case is a decision instrument
rather than an advocacy document is:
- A. whether it follows the corporate template
- B. whether it could have concluded "no" ✅
- C. whether it was approved
- D. whether its NPV is positive

*Rationale:* An options set and evidence that permit a negative conclusion are what make it a
decision (2.2.1). A is the compliance failure mode; C and D are outcomes, not tests.

**MCQ 2.2-E `[2.2.2 · Application]`** A case shows NPV +1,332,898 against a do-nothing option valued
at zero. In fact doing nothing forces a mandatory 600,000 spend in year 4 and 40,000 a year in years
5–8; the rate is 7 % (`1/1.07⁴ = 0.762895`; `AF(0.07,8) − AF(0.07,4) = 2.584087`). The incremental
NPV is:
- A. USD 1,332,898
- B. USD 1,893,998 ✅
- C. USD 2,092,898
- D. USD 771,797

*Rationale:* Avoided cost PV is `457,737 + 103,363 = 561,101`, added to the project's NPV (2.2.2b).
A omits the counterfactual entirely; C adds the **undiscounted** 760,000; D subtracts the avoided
cost instead of adding it, treating a cost the project removes as a cost it incurs.

**MCQ 2.2-F `[2.2.3 · Analysis]`** Beta leads Meridian 4.05 to 3.95. Scores on strategic fit are
Beta 5, Meridian 4; on benefit value Beta 3, Meridian 5. Shifting weight from strategic fit to
benefit value flips the ranking at a shift of:
- A. 10.00 percentage points
- B. 3.33 percentage points ✅
- C. 2.50 percentage points
- D. no shift between these two criteria can flip it

*Rationale:* Meridian gains `δ` and Beta loses `2δ`, so `3δ = 0.10` and `δ = 0.033333` (2.2.3c).
A removes weight from fit without adding it to benefit, so the weights no longer sum to one; C
divides the 0.10 margin by the 4-point score range; D is true of a fit-to-deliverability shift, where
both candidates move identically, but not of this pair.

**MCQ 2.2-G `[2.2.3 · Evaluation]`** With 3 units of integration capacity the optimal set is
Beta + Gamma at 2,100,000; with 4 units it is Meridian + Gamma at 2,593,072. The marginal value of
the fourth unit is:
- A. USD 900,000
- B. USD 1,693,072
- C. USD 493,072 ✅
- D. USD 564,357

*Rationale:* `2,593,072 − 2,100,000 = 493,072` (2.2.3b). A applies Gamma's best-in-set per-unit ratio
to the marginal unit; B counts the whole NPV of the candidate newly admitted while ignoring that Beta
is displaced; D is Meridian's own NPV per unit, which is an average and not a margin.

### Self-check — KA 2.2

1. *What must an options set contain to be genuine?* — A do-nothing baseline, a materially cheaper
   option, a materially more ambitious one; and at least one that could plausibly have won.
2. *Why is breakeven adoption a better board sentence than NPV?* — It states the condition the
   value depends on, which is what the board can actually influence and monitor.
3. *When does ranking by value per unit of constraint fail?* — With lumpy candidates: the feasible
   sets must be enumerated, as Beta + Gamma shows.
4. *State Meridian's breakeven adoption on both bases and the conversion between them.* — 41.0460 %
   flat-equivalent, 45.0053 % ramped-basis; the ramped PV is 91.2027 % of the flat PV at the same
   steady state, and `41.0460 = 0.912027 × 45.0053`.
5. *Which two errors in a business case run in opposite directions?* — A flat benefits profile
   flatters (Meridian by 2,114,198 of PV); a zero do-nothing baseline penalises (by 561,101).
6. *Why should a capacity decision be taken as a block?* — The marginal value of the constraint is
   non-monotone, so judging one unit at a time stops at +93,072 and never reaches the +493,072
   available from three.
7. *What should a scoring model report alongside its ranking?* — The flip point, and where two
   candidates lie within it, the statement that this method does not separate them.

---

## Knowledge Area 2.3 — Benefits, value and sustainability

*Topics: 2.3.1 benefits mapping · 2.3.2 measures and baselines · 2.3.3 sustainability and ESG
value · 2.3.4 assumptions and dependencies.*

### 2.3.1 Benefits mapping

A **benefits map** is a chain, drawn explicitly, from what the project delivers to why anyone
wanted it: **output → enabling change → outcome → benefit → strategic objective**. Domain 1
established the links; this KA draws them and assigns them.

The **enabling change** is the step most maps omit and most programmes fail on. Meridian's outputs
(installed clinics) produce no benefit without training, workflow redesign, clinical-champion
support and data migration — none of which are software, and several of which sit outside the
project's authority. A benefits map that leaps from output to benefit has hidden exactly the work
that determines whether anything is realised, which is why Meridian's adoption stalled at 40 %
while installation ran to plan.

Every element on the map carries an **owner** and, for outcomes and benefits, a **measure**. The
rule from Domain 1 (KA 1.2.1) applies unchanged: one accountable name per outcome, and the benefits
owner sits outside the project (2.1.3).

> **Fig 2.3.1 — Meridian's benefits map, with the enabling change restored.** Left-to-right map.
> Outputs column: "Records system installed (40 clinics)", "Data migrated", "Interfaces live".
> **Enabling change** column (highlighted, marked "usually omitted"): "Clinicians trained",
> "Workflows redesigned", "Clinical champions in place", "Legacy process retired". Outcomes:
> "28 clinics using it in daily practice (70 %)". Benefits: "6 clinician-hours/week released per
> adopting clinic → USD 685,440/yr". Objective: "Improved access and clinician capacity". Each box
> tagged with an owner role; the enabling-change column's owners are all *outside* the project.
> Source: PCI original. Alt text: a benefits map from installed outputs through a highlighted
> enabling-change column to adoption, released hours and the strategic objective.

### 2.3.2 Measures and baselines

A benefit is only claimable against a **baseline**: the measured position before the change. Three
disciplines make measurement honest:

- **Baseline before, not after.** Measure the current state before the change lands; a baseline
  reconstructed afterwards is an estimate shaped by the result it is used to justify.
- **Attribution.** Other things change too. Where a benefit could have other causes, say so and
  quantify what portion is plausibly attributable, over-claiming is the fastest way to lose the
  credibility that future cases depend on.
- **No double-counting.** The single commonest audit finding in benefits work: an enabling
  programme claims the benefit its dependent projects also claim, and the portfolio's total exceeds
  what the organisation could possibly realise. The portfolio benefits register (Domain 15) exists
  to catch this, and it only works if each benefit has exactly one claimant.

**Cash-releasing versus non-cash-releasing** benefits must be distinguished plainly. Meridian
releases *clinician hours*; that becomes cash only if the organisation reduces cost or converts the
hours into additional activity that has value. Reporting released hours as savings, when the
headcount is unchanged and the hours are absorbed, is the error that makes finance directors
distrust benefits cases. And and it is avoided by stating the conversion explicitly, or by claiming
the benefit in its true unit (capacity) rather than in money it never became.

Attribution is the discipline of the three that is most often waved at and least often done, because
doing it requires a **comparison cohort** (a group that did not receive the change), and choosing to
keep one feels like withholding a benefit. It is worth the discomfort, and it is worth arithmetic.

**Worked example 2.3.2 — the measured benefit and the attributable benefit.**

1. **Setup.** Twelve months after go-live, Meridian's benefits owner measures records-administration
   time per clinic per week. The pre-change baseline, measured before deployment, was **22.0
   hours**. In the **28** adopting clinics it is now **15.6 hours**. In the **12** clinics that have
   not adopted (the comparison cohort) the same measure has fallen from 22.0 to **20.9 hours**,
   because an unrelated triage redesign was rolled out across the whole authority in the same
   period. The valuation basis is Domain 1's: **USD 85** an hour over a **48-week** operating year.
   The programme's business case assumed **6.0 hours** released per clinic per week. What may be
   claimed?
2. **Formula.** `Raw improvement = baseline − post-change in the adopting group`.
   `Counterfactual improvement = baseline − post-change in the comparison group`.
   `Attributable improvement = raw − counterfactual` (the difference of the two differences).
   `Annual benefit = adopting clinics × attributable hours × rate × weeks`.
3. **Substitution.** Raw `22.0 − 15.6 = 6.4`; counterfactual `22.0 − 20.9 = 1.1`; attributable
   `6.4 − 1.1 = 5.3`. Claimable `28 × 5.3 × 85 × 48`; the raw claim would be `28 × 6.4 × 85 × 48`.
4. **Result.**

   | Basis | Hours per clinic per week | Annual benefit, 28 clinics |
   |---|---|---|
   | Raw measured improvement | 6.4 | **731,136** |
   | Business-case assumption | 6.0 | 685,440 |
   | **Attributable improvement** | **5.3** | **605,472** |

   The raw claim overstates the attributable benefit by **USD 125,664**, which is **17.1875 %** of the
   raw claim.
5. **Interpretation.** Five readings, and the first is the one that catches an unwary benefits owner
   in a good year.

   **A measurement that beats the case can still fail to support it.** The raw figure, 6.4 hours,
   exceeds the case's 6.0, so the natural report is "benefits exceeding forecast". The attributable
   figure, 5.3 hours, is **11.67 % below** the case, and the case's 6.0 is **13.21 % higher** than
   what can honestly be claimed. Raw and attributable are different quantities, and it is the raw
   one that programmes report because it is the one the programme's own data produces. **The
   comparison cohort is what converts a measurement into evidence**, and a programme without one
   cannot distinguish its own effect from everything else that happened that year, in either
   direction.

   **The over-claim share is an invariant, and it is worth knowing in a meeting.** Because the raw
   and attributable figures share the clinic count, the rate and the weeks, the over-claim as a
   fraction of the raw claim is `counterfactual ÷ raw = 1.1/6.4 =` **17.1875 %**: irrespective of
   the valuation rate, the operating year or how many clinics adopted. It is the exact structural
   twin of Domain 1's adoption identity, where an output-based claim overstates by `1 − a` whatever
   the other numbers are (KA 1.3.2). Both identities exist for the same reason and are used the same
   way: **you do not need the model to say how wrong a claim is, only the term it omitted.**

   **The correction moves the whole case, not just this year's report.** At 5.3 attributable hours,
   full potential falls from 979,200 to `40 × 5.3 × 85 × 48 =` **USD 864,960**, which is **88.3333
   %** of the original, exactly `5.3/6.0`, since nothing else changed. The flat-equivalent breakeven
   adoption therefore *rises* from 41.0460 % to `2,400,000 / (864,960 × 5.971299) =` **46.4672 %**,
   a deterioration of **5.4212 percentage points**. Taken together with the do-nothing correction of
   2.2.2b, which improved the breakeven by 9.5962 points, the fully corrected flat-equivalent
   breakeven is `(2,400,000 − 561,101) / (864,960 × 5.971299) =` **35.6035 %**; and Meridian's
   achieved 40 % adoption clears it with **4.3965 points** to spare, for an NPV of **+USD 227,074**.
   **Two errors of opposite sign, and you must fix both: fixing only the flattering one produces a
   verdict as wrong as fixing neither.**

   **Choosing a comparison cohort is a real decision with real costs, and it should be made
   deliberately.** The 12 non-adopting clinics were not held back for measurement (they simply had
   not adopted), which makes them a **convenience cohort**, and convenience cohorts differ
   systematically from the adopters: the clinics that adopt first are usually the better-led,
   better-staffed ones, so part of the 5.3 may still be selection rather than software. A
   deliberately staged rollout gives a defensible cohort as a free by-product of the sequencing
   decision, which is another benefit of the tranching argued in 2.1.3, and it is one of the
   strongest arguments a benefits owner has against a simultaneous switch-on. Where withholding a
   change is not acceptable (and in clinical or safety settings it frequently is not) the honest
   alternatives are a pre-change trend line extended forwards, or a stated range with the
   attribution assumption on the face of the report. **What is not acceptable is claiming the raw
   figure and calling the question unanswerable.**

   **What breaks it.** The two groups must be measured the **same way at the same time**; a
   post-change measurement method that is more thorough in adopting clinics manufactures an effect.
   A counterfactual improvement can also be *negative* (the comparison group getting worse) in which
   case the attributable figure exceeds the raw one and the honest report is a larger claim, which
   is the case a leader should be most careful to actually make. Attribution and adoption interact
   and must not be double-corrected: the 5.3 hours is a per-adopting-clinic figure, and multiplying
   by 28 already applies the adoption term, so applying 70 % again is a common and severe error. And
   none of this converts the benefit to cash: 605,472 of attributable released time is **capacity**,
   and the cash-releasing test above still has to be passed separately.

> **Fig 2.3.2 — Four breakevens from one programme.** A single horizontal axis of flat-equivalent
> sustained adoption from 25 % to 50 %, carrying four marked thresholds computed from identical
> Meridian facts under different baseline treatments: **31.4498 %** (honest do-nothing baseline,
> 6.0 hours), **35.6035 %** (honest baseline and attributable 5.3 hours), **41.0460 %** (the case as
> approved) and **46.4672 %** (attribution corrected, counterfactual still omitted). A heavy crimson
> rule marks Meridian's actual sustained adoption of **40.0000 %**, falling between them: it clears
> the first two thresholds and misses the last two. Footnote lines read "Clears 31.4498 % and
> 35.6035 % — value created" and "Misses 41.0460 % and 46.4672 % — value destroyed", above the
> statement that the verdict is set by which baseline errors were made rather than by the programme.
> Source: PCI original. Alt text: one adoption axis with four breakeven thresholds marked and the
> programme's actual adoption falling between them, so that the same result is a success or a failure
> depending on the baseline used.

### 2.3.3 Sustainability and ESG value

Environmental, social and governance considerations enter this domain in two distinct ways, and
conflating them causes most of the confusion.

**As constraints and obligations:** emissions limits, environmental permits, accessibility law,
labour standards in the supply chain (Domain 10's ethical sourcing). These are hard constraints
(2.1.2) and belong in the case as requirements, not benefits.

**As value:** whole-life carbon reduction, energy cost avoided, social value created, resilience to
climate risk. These belong in the benefits map with measures and owners like any other benefit; and
with the same honesty about attribution and monetisation. Where a carbon reduction has a price (a
compliance cost avoided, a levy, an internal carbon price), it can be valued directly; where it does
not, it should be reported in its physical unit alongside the financial case rather than converted
with an invented price. **Whole-life thinking** is the practical contribution: a cheaper capital
solution with higher operating energy and shorter life frequently loses on a whole-life basis, and
the appraisal horizon of KA 2.2.2 is what makes that visible (PFL-AI Domain 4's equivalent annual
value handles unequal lives).

**As a disclosed claim:** the third way, and the one most often missed, because it is not an
appraisal question at all. A benefit that stays inside the appraisal is an estimate a board may
accept or challenge on the evidence. The moment the same benefit is reported *outside* the
organisation (in a sustainability or annual report, in a bid or prequalification response, in a
submission to a regulator, or in a financing or investor document) it stops being an estimate and
becomes a **claim**, and a claim is held to a standard a forecast is not. Four provisions follow,
and they are stated as professional obligations rather than as anyone's legal position:

- **A stated boundary and method.** What is counted, over what period, against which baseline, on
  whose emission or valuation factors, and what is deliberately excluded. A carbon or social-value
  number without its boundary is not a measure: two organisations applying different boundaries
  produce different figures from identical facts, and neither figure is checkable by the reader.
- **A named owner.** One person accountable for the figure: the rule KA 2.3.2 applies to every other
  benefit measure, and it does not weaken because the unit is tonnes or apprenticeship weeks rather
  than currency.
- **Retained evidence sufficient for someone else to test it.** External assurance, where it is
  obtained, tests the evidence rather than the assertion, so a figure whose working papers were a
  spreadsheet on one laptop cannot be assured whatever its accuracy. Domain 16, KA 16.4.4's retention
  economics apply directly, and the record class, custodian and retention period are set *before* the
  claim is published rather than after it is questioned. The benefits measurement plan of Domain 16,
  KA 16.4.1 is where a claimed benefit acquires the measured evidence that later supports it.
- **Approval by whoever signs the disclosure.** The project does not approve an external claim. That
  approval belongs to the function that owns the report, the bid or the submission; the project's
  obligation is to supply the measure, the boundary, the method and the evidence in a form that
  function can sign, and to say plainly where the evidence stops.

**The professional prohibition, in one line.** *A benefit that cannot be evidenced to the standard
its intended audience requires is not reported as achieved.* Report the measured part as measured,
the estimated part as estimated with its method beside it, and the unevidenced part not at all.
Presenting a forecast as a result, counting a benefit the organisation did not cause, or moving the
boundary until the number improves are the attribution failures of KA 2.3.2 committed in front of an
audience that cannot see the working, which is why the profession treats the disclosed version as
the graver of the two, and why the leader who supplies the number needs a written record of what was
supplied and to whom.

**The standing caveat, in this volume's usual form.** Which external disclosure and assurance
obligations apply, to which entities, on what timetable, in what form, and with what consequence for
an unsupported claim, is jurisdiction- and entity-specific, changes, and is taken from the
organisation's reporting function and from qualified counsel, never from this book. Nothing here
states the position in any jurisdiction, states what any regime requires, or characterises any
organisation's disclosure as compliant or otherwise. One distinction is worth holding because it is
routinely blurred: a **voluntary reporting framework** is something an organisation chooses to
adopt, and a **disclosure regime** is something that applies on its own terms to those it reaches;
adopting the first tells you nothing about the second, and the two questions go to the same advisers
separately. Where a quantified greenhouse-gas figure is being prepared, **ISO 14064** is the
document usually named as addressing the quantification and reporting of greenhouse-gas emissions
and removals; it is voluntary guidance, it is named here and not reproduced, and it is not itself a
disclosure obligation.

### 2.3.4 Assumptions and dependencies

**Business cases fail through their assumptions**, and the assumptions are usually stated once and
never revisited. Meridian's case rested on an adoption assumption that was never tracked as a
measure: the direct cause of everything Domain 1's case study describes.

An **assumption register** carries, per assumption: the statement, why it is believed, the impact if
false, the owner, the date it will be tested, and the **trigger** that would falsify it. Its
relationship to the risk register is exact: **every assumption is a risk in disguise**
(Domain 8, KA 8.1.3), so material assumptions get register entries with `EMV` where quantifiable.

Quantifying them is the step that turns the register from a list into an instrument, because it
permits two questions no list can answer: *is the case's promised value large relative to the
exposure carried by its own assumptions?* and *which assumption should be tested first?*

**Worked example 2.3.4 — pricing Meridian's assumption register.**

1. **Setup.** Meridian's honest case (2.2.2) gives a ramped PV of benefits of **3,732,898** against
   a cost of 2,400,000 (an NPV of **+1,332,898**), and, per unit of sustained adoption, a PV of
   **5,332,711**. Five material assumptions underpin it. Each has an assessed impact if false, in
   present-value terms, and a probability of being false, both **assessed by the programme team and
   recorded as judgements, not measurements**. The cost of testing each is also known.
2. **Formula.** `EMV = probability × impact`. `Assumption exposure ratio = Σ EMV ÷ NPV`. Test priority
   ranks by `EMV ÷ cost to test` (infinite where the test is free, so those go first).
3. **Substitution.** A1: adoption falling from 70 % to 45 % costs `0.25 × 5,332,711 = 1,333,178`.
   A2: attributable hours 6.0 → 5.3 costs `3,732,898 × (1 − 5.3/6.0) = 435,505`. A3: the legacy
   remediation is not in fact avoided, `600,000/1.07⁴ = 457,737`. A4: the enabling change must be
   programme-funded, `320,000`. A5: finance values clinician time at 70 rather than 85,
   `3,732,898 × (1 − 70/85) = 658,747`.
4. **Result.**

   | | Assumption | Impact if false (PV) | P(false) | **EMV** | Cost to test | EMV per unit of test cost |
   |---|---|---|---|---|---|---|
   | A1 | Sustained adoption reaches 70 % | 1,333,178 | 0.35 | **466,612** | 40,000 | 11.67 |
   | A2 | 6.0 attributable hours per clinic-week | 435,505 | 0.50 | **217,752** | 15,000 | 14.52 |
   | A3 | Legacy remediation genuinely avoided | 457,737 | 0.25 | **114,434** | 0 | — (free) |
   | A4 | Enabling change funded outside the programme | 320,000 | 0.45 | **144,000** | 0 | — (free) |
   | A5 | Finance accepts USD 85 an hour | 658,747 | 0.20 | **131,749** | 0 | — (free) |
   | | **Total** | 3,205,166 | | **1,074,548** | 55,000 | |

   Total EMV is **USD 1,074,548**, which is **80.62 %** of the NPV the case promises. A1 alone is
   **35.01 %** of it. The three free tests together carry **USD 390,184**: **36.31 %** of the
   register's exposure.
5. **Interpretation.** Five readings, and the first is the single most useful sanity test on any
   business case.

   **The exposure ratio tells a board how thin its own case is.** A positive NPV of 1,332,898
   sitting above an assumption exposure of 1,074,548 is a ratio of **0.8062**: the case is
   right-side-up, but only just, and a board told "NPV +1.33m" hears something quite different from
   a board told "NPV +1.33m against 1.07m of expected exposure in the assumptions it rests on". The
   ratio also gives a usable rule of thumb for challenge: **where Σ EMV approaches or exceeds the
   NPV, the case is not a value proposition but a bet**, and it should be structured as one —
   staged, with kill criteria (2.4.3) and a real abandonment option (2.A.1) — rather than approved
   as a commitment.

   **The test-order rule is where the leader gets something for nothing.** Three of the five
   assumptions cost nothing to test: A3 is a letter to the vendor asking whether support will be
   extended; A4 is a written funding confirmation from the clinical directorate; A5 is a
   conversation with the finance business partner about the valuation rate. Together they carry
   **390,184** of exposure and can be resolved in a week, before approval. Ranking by `EMV ÷ cost to
   test` rather than by EMV alone is what surfaces that, and it inverts the usual instinct, which is
   to spend the analysis budget on the biggest number: A1; and to leave the cheap ones as
   "assumptions". **The professional habit: sort the register by exposure per unit of test cost, and
   clear the free rows before the paper is written.**

   **The one that must be structural, not analytical, is adoption.** A1 cannot be resolved by
   analysis, only by evidence from operation, which is why its test costs 40,000 (a measured pilot
   cohort) and why it remains the largest residual after the free rows are cleared: 466,612 of a
   remaining 684,365. An assumption that can only be tested by doing part of the work is the
   definition of a case for staging, and 2.A.1 prices exactly that trade for Meridian. Note the
   symmetry with the alignment re-test of 2.1.3: both convert an unmanaged belief into a scheduled
   observation, and both are worth far more than the sophistication of the analysis around them.

   **EMVs are not additive as a portfolio loss, and saying they are is a real error.** A1 (adoption)
   and A2 (attributable hours) are both statements about the same clinical behaviour and will fail
   together more often than independently; A5 (the valuation rate) is largely independent of both.
   So the 1,074,548 is the correct *expected* total (expectations add regardless of correlation),
   but it is **not** the right basis for a downside statement, because the correlated pair makes the
   tail worse than an independence assumption implies. Domain 8, KA 8.A.1 handles the correlation
   properly and KA 8.3 sizes contingency from it; the register's job here is to state the expected
   exposure and flag which entries move together.

   **What breaks it, and the honest limits.** Every probability in the table is a judgement, and a
   register whose probabilities were set by the case's author will be systematically low: the same
   advocacy problem 2.2.1 identifies, appearing here as calibration rather than as omission. Impacts
   assessed as single points hide their own ranges. EMV also treats all failures as equivalent
   losses, when some are recoverable and some are not: A4 failing costs 320,000 and is survivable,
   while A1 failing changes what the programme *is*, and an expected-value table cannot express that
   difference, Domain 8's irreversibility treatment must be read alongside it. And a register is
   only alive if the test dates are in someone's calendar with a named owner; an assumption register
   reviewed at the same cadence as the risk register, by the same forum, with the same escalation,
   is the version that works, and a separate document nobody opens is the version that exists in
   most organisations.

**Dependencies** (on other projects, on suppliers, on decisions not yet taken) are managed by naming
them, naming the owner on the *other* side, and stating the date by which the dependency must be
satisfied. Two failure modes recur: **assumed dependencies** (nobody on the other side knows they
owe you anything) and **unmanaged reciprocals** (they are depending on you too, on a different
date). Domain 15's programme dependency management handles the multi-project case; at project level
the discipline is simply that a dependency without a named counterpart is a hope.

### AI in this KA

Benefits mapping is judgment work about causation, which is where AI is weakest: a model will
happily assert that an output produces a benefit because most documents assert exactly that (Domain
1's KA 1.3 warning). Two legitimate uses. **Challenge**: ask a model to argue that the benefit will
*not* materialise, and the enabling changes it names are often the ones the map omitted.
**Cross-checking at portfolio scale**: detecting the same benefit claimed by two cases is
pattern-matching over documents, which machines do well and humans do badly across fifty cases. The
conclusions stay human, with named benefit owners.

**Verification, concretely.** Two specific hazards attach to the arithmetic of this KA. A model
asked to compute an attributable benefit will happily do the subtraction and will **not** ask
whether the comparison group is a fair counterfactual, so the professional check is on the cohort,
not on the subtraction: were both groups measured the same way at the same time, and is the
non-adopting group systematically different from the adopting one? That is a judgement about
selection, and no amount of arithmetic substitutes for it. And a model asked to populate an
assumption register will supply **probabilities**, fluently and without provenance, which then drive
an exposure ratio that a board reads as analysis. Where the organisation has no history to calibrate
from, the correct output is that it has none, plus the range of probability over which the
recommendation is unchanged. What is safe to delegate is the reconciliation work: checking that
every benefit in the register has exactly one claimant across the portfolio, that no avoided cost
appears also as a benefit, and that the adoption term has been applied once rather than twice: all
three are mechanical, all three are commonly wrong, and all three are reported with a location so a
human can confirm them.

### Key terms — KA 2.3

| Term | Meaning |
|---|---|
| **Benefits map** | Output → enabling change → outcome → benefit → objective, with owners and measures. |
| **Enabling change** | The non-project work (training, workflow, adoption support) without which no benefit occurs. |
| **Baseline** | The measured pre-change position; must be measured before, not reconstructed after. |
| **Attribution** | The portion of a measured improvement plausibly caused by this project. |
| **Comparison cohort** | A group that did not receive the change, used to measure the counterfactual improvement. |
| **Attributable improvement** | Raw improvement less the counterfactual improvement; the difference of the two differences. |
| **Over-claim share** | `counterfactual ÷ raw`, invariant to the valuation rate, the operating year and the number of adopters. |
| **Double-counting** | The same benefit claimed by more than one case; one claimant per benefit. |
| **Cash-releasing / non-cash-releasing** | Benefits that reduce spend versus those that release capacity. |
| **Assumption register** | Statement, basis, impact-if-false, owner, test date, falsifying trigger. |
| **Assumption exposure ratio** | `Σ EMV ÷ NPV`; as it approaches one, the case is a bet rather than a value proposition. |
| **Test-order rule** | Rank assumptions by `EMV ÷ cost to test`, so the free tests are cleared before the paper is written. |
| **Disclosed claim** | A benefit reported outside the organisation; needs a stated boundary and method, a named owner, retained evidence and the signing function's approval, and is never approved by the project. |
| **Claim boundary** | What a sustainability or social-value measure counts and excludes, over what period and against which baseline; without it the number is not checkable. |
| **Voluntary framework vs disclosure regime** | A framework is adopted by choice; a regime applies on its own terms. Adopting the first says nothing about the second, and both are questions for the reporting function and qualified counsel. |

### Sample MCQs — KA 2.3

**MCQ 2.3-A `[2.3.1 · Analysis]`** A benefits map runs directly from "system installed" to
"USD 685,440 released per year". Its principal defect is:
- A. the benefit figure is too precise
- B. the **enabling change** is missing — training, workflow redesign and adoption support, largely owned outside the project, are what convert the output into the outcome ✅
- C. it should be expressed in hours rather than money
- D. the map should start with the strategic objective

*Rationale:* Omitting enabling change hides the work that determines realisation (2.3.1), exactly
how Meridian stalled at 40 % adoption with installation on plan. C is a separate (also real)
question of unit; D is presentational.

**MCQ 2.3-B `[2.3.2 · Application]`** A programme releases 6 clinician-hours per week per clinic;
headcount is unchanged and the hours are absorbed by existing demand. Reporting this as a cash
saving is:
- A. correct — released time has a value
- B. incorrect: it is a non-cash-releasing (capacity) benefit unless cost is reduced or the capacity is converted to valued activity ✅
- C. correct if the hourly rate is documented
- D. acceptable provided it is discounted

*Rationale:* Value released is not cash released (2.3.2); claiming otherwise is the error that
discredits benefits cases. A and C mistake a valuation rate for a cash effect; D discounts a
figure that was never cash.

**MCQ 2.3-C `[2.3.4 · Analysis]`** An enabling platform programme and three dependent projects each
claim the same USD 4m of downstream benefit. The portfolio total is:
- A. USD 16m, since each case is individually valid
- B. overstated by double-counting — one benefit, one claimant, with the enabler credited through the dependents or vice versa but not both ✅
- C. USD 4m, and the dependent projects have no benefits
- D. indeterminate until delivery completes

*Rationale:* Double-counting is the standing audit finding (2.3.2); the fix is a single claimant per
benefit in the portfolio register. C overcorrects by denying the dependents any case; D defers a
question answerable now.

**MCQ 2.3-D `[2.3.3 · Application]`** A carbon reduction has no priced compliance consequence for
the organisation. The soundest treatment in the case is:
- A. omit it, since it cannot be valued
- B. report it in its physical unit alongside the financial case, rather than monetising it with an invented price ✅
- C. monetise it using a rate found in a published study
- D. record it as a constraint

*Rationale:* Unpriced benefits are reported honestly in their own unit (2.3.3). A discards real
value; C imports a price the organisation does not face; D confuses value with obligation.

**MCQ 2.3-E `[2.3.2 · Application]`** A pre-change baseline of 22.0 hours a week falls to 15.6 in the
28 adopting clinics and to 20.9 in the non-adopting comparison clinics. At USD 85 an hour over 48
weeks, the attributable annual benefit across the adopters is:
- A. USD 731,136
- B. USD 605,472 ✅
- C. USD 685,440
- D. USD 125,664

*Rationale:* Attributable release is `6.4 − 1.1 = 5.3` hours, so `28 × 5.3 × 85 × 48 = 605,472`
(2.3.2). A is the raw claim with no counterfactual deducted; C is the business case's 6.0-hour
assumption, which is a forecast and not a measurement; D is the over-claim itself, not the benefit.

**MCQ 2.3-F `[2.3.2 · Analysis]`** A programme measures a raw improvement above its business-case
assumption and reports benefits exceeding forecast. The comparison cohort improved by 1.1 of the 6.4
hours. The strongest professional statement is that:
- A. benefits exceed forecast, since the measured improvement is larger than assumed
- B. the attributable improvement is 5.3 hours — below the assumed 6.0 — so the case is not supported,
  and the over-claim is 17.1875 % of the raw figure whatever the valuation rate ✅
- C. the result is unusable because no randomised comparison exists
- D. the counterfactual should be added to the claim, since both improvements are real

*Rationale:* Raw and attributable are different quantities, and the over-claim share is
`counterfactual ÷ raw`, invariant to the rate, weeks and clinic count (2.3.2). A is the error the
raw figure invites in a good year; C over-reaches: a convenience cohort is weak evidence, not no
evidence; D claims an improvement the programme did not cause.

**MCQ 2.3-G `[2.3.4 · Evaluation]`** An assumption register carries EMVs of 466,612, 217,752, 114,434,
144,000 and 131,749 against a case NPV of 1,332,898; the last three cost nothing to test. The soundest
first action is:
- A. commission the 40,000 adoption study, since 466,612 is the largest exposure
- B. clear the three free tests, resolving USD 390,184 — 36.31 % of the register's exposure — before
  the paper is written ✅
- C. reduce the case's NPV by the total EMV of 1,074,548 and re-submit
- D. no action: an exposure ratio of 0.8062 is below 1, so the case stands

*Rationale:* Test priority ranks by EMV per unit of test cost, and a free test has no competitor
(2.3.4). A is right on magnitude and wrong on order; C double-counts, since the EMVs are exposures
around the NPV, not deductions from it; D treats a sanity ratio as an approval threshold.

### Self-check — KA 2.3

1. *Which element do most benefits maps omit, and why does it matter most?* — The enabling change;
   it is the work that converts outputs into outcomes and it usually sits outside the project.
2. *What makes a baseline trustworthy?* — Measurement before the change, not reconstruction after.
3. *State the relationship between assumptions and risks.* — Every assumption is a risk in
   disguise; material ones get register entries with quantified exposure.
4. *How is an attributable improvement computed, and what is the invariant?* — Raw improvement less
   the counterfactual improvement; the over-claim share is `counterfactual ÷ raw`, 17.1875 % on
   Meridian, whatever the rate, weeks or number of adopters.
5. *Why must both of Meridian's baseline errors be corrected, not one?* — The flat profile flatters
   and the missing counterfactual penalises; correcting only one gives a breakeven of 46.4672 % or
   31.4498 %, where the honest figure is **35.6035 %**.
6. *What does an assumption exposure ratio approaching one tell a board?* — That the case is a bet
   rather than a value proposition, and should be staged with kill criteria rather than committed.
7. *What changes when a sustainability or social-value benefit is reported outside the
   organisation?* — It becomes a claim rather than an estimate: it needs a stated boundary and
   method, a named owner, retained evidence someone else can test, and approval from the function
   signing the disclosure. The project supplies it; the project does not approve it.
8. *State the professional prohibition on disclosed benefits.* — A benefit that cannot be evidenced
   to the standard its intended audience requires is not reported as achieved. Which external
   obligations apply, and to whom, is for the reporting function and qualified counsel, not for this
   book.

---

## Knowledge Area 2.4 — Strategic termination

*Topics: 2.4.1 stopping as a strategic decision · 2.4.2 sunk cost and escalation of commitment ·
2.4.3 kill criteria and honest gates.*

### 2.4.1 Stopping as a strategic decision

Organisations that cannot stop projects cannot really select them either: a portfolio that only
accretes has no capacity for anything new, and its selection process becomes a queue. Stopping is
therefore a **portfolio capability**, not an admission of failure; and the leaders who can do it
create the capacity that funds everything else.

The barriers are cultural and structural, and both are addressable. **Culturally**, cancellation is
read as failure and punished, so nobody proposes it; the counter is to celebrate *early* stopping
explicitly and to distinguish "we learned this will not work" from "we managed this badly".
**Structurally**, no gate has authority to stop, or the decision is scheduled after the money is
committed; the counter is Domain 3's decision rights with a real stop option and gates placed before
irreversible commitments (Domain 1's irreversibility point).

### 2.4.2 Sunk cost and escalation of commitment

**The principle.** A continuation decision depends only on **remaining cost and remaining benefit**.
Money already spent is irrelevant to it; it cannot be recovered by spending more, and it carries no
information about the future beyond what is already in the forecast.

**Worked example 2.4.2 — Meridian at the reset point.**

1. **Setup.** A hypothetical reset: USD 1,800,000 of the 2,400,000 has been spent. Completion
   requires a further **USD 900,000** (the original 600,000 plus 300,000 of newly discovered
   integration work). The benefits case has been re-based on measured adoption, giving a remaining
   benefit present value of **USD 780,000**. Continue or stop?
2. **Formula.** Forward-looking NPV = remaining benefit PV − remaining cost. Sunk cost excluded.
3. **Substitution.** `780,000 − 900,000`.
4. **Result.** **Forward NPV = (USD 120,000).** The correct decision is to **stop** (or to descope
   to a variant whose remaining benefit exceeds its remaining cost).
5. **Interpretation.** Five readings, and the fourth is the one that keeps a leader from
   over-applying the rule.

   **The sunk-cost frame does not merely mislead; it multiplies the loss by 7.5.** The pull to
   continue is powerful and entirely arithmetical in appearance: "we have spent 1,800,000 — stopping
   wastes it." That reasoning adds a sunk 1,800,000 back into a decision it cannot affect, and it
   converts a **120,000** loss into a **900,000** one. The 1,800,000 is already gone under *either*
   choice; the only question is whether the *next* 900,000 buys 780,000 of value. It does not.

   **State the decision as a threshold, because a threshold can be checked.** The continuation is
   worth taking at any remaining cost below **780,000**, so the 900,000 estimate exceeds the
   justified ceiling by **USD 120,000**, or **15.38 %** of it. Equivalently, continuation needs the
   remaining benefit to be at least 900,000, which is **15.38 %** more than the re-based 780,000.
   That is a far better gate sentence than a negative NPV, because it tells the meeting exactly what
   would have to be true. And and it is testable: a descope that removes 120,000 of remaining cost
   without removing benefit turns the decision round, which is why "stop" and "descope" belong in
   the same paper (Case study B does exactly this at scale).

   **Stopping releases the money into the constrained selection problem, which is where its real
   value appears.** The 900,000 is redeployable to candidates the portfolio could not previously
   fund (KA 2.2.3's constraint arithmetic), and if the released capacity also includes the scarce
   team, 2.2.3b prices what that is worth, which may exceed the money. **This is why stopping is a
   strategic act and not merely a loss**, and it is also why the paper must say what the released
   resources will do next: a board asked to accept a write-off with no redeployment proposal is
   being asked to take the whole loss and none of the gain.

   **The rule is about *unrecoverable* spend, and three things are commonly mis-sorted into it.**
   Sunk cost is spend that cannot be recovered *and* carries no information about the future beyond
   what the forecast already holds. Recoverable amounts are not sunk and belong in the forward
   arithmetic: resale or redeployment value of equipment and licences, recoverable prepayments, and
   reusable assets such as cleaned data or a qualified team. Contractual exit costs are not sunk
   either: they are a **forward** cost of stopping, and if terminating the supplier costs 150,000
   the comparison is `780,000 − 900,000 = (120,000)` for continuing against `(150,000)` for
   stopping, and the correct decision reverses. And a cost already incurred *does* carry information
   where it revises the forecast: an overspend that reveals the work is harder than believed should
   change the estimate of the remaining 900,000, which is a legitimate use of history and not the
   sunk-cost fallacy. **The discipline is to exclude sunk cost from the comparison while using the
   evidence it generated.**

   **What breaks it.** The remaining-benefit figure must be re-based on measured evidence, not
   inherited from the original case: using the approval-time benefit here would give `3,732,898 −
   900,000` and a spurious continuation. Both sides must be on the same present-value basis and the
   same horizon. And the decision must be available: where an obligation is contractual or
   statutory, there is no stop option to exercise and the arithmetic is describing a choice nobody
   holds, in which case the honest output is the *size of the loss* and a claim or renegotiation
   strategy (Domain 10), not a recommendation to stop. **What a termination actually costs, and
   whether the exit right exists at all, are questions of the particular contract and the governing
   law: take legal advice on the exit position before the 150,000 goes into a gate paper as a
   number.**

**Escalation of commitment** is the behavioural pattern that makes this hard: personal
identification with the project, reputational exposure for the approver, and the asymmetry that
continuing defers the reckoning while stopping realises it now. Its countermeasures are structural,
not exhortative: pre-set kill criteria (2.4.3), decision-makers who did not author the case, and
forward-only framing in every gate paper (Domain 8's bias table).

### 2.4.3 Kill criteria and honest gates

**Kill criteria** are conditions, agreed **in advance**, under which the project will stop: "if
adoption in the pilot cohort is below 50 % at month six", "if the regulatory approval is not granted
by Q3", "if remaining cost exceeds remaining benefit at any gate". Their power is entirely in the
*advance* agreement: the same evidence assessed against a pre-agreed threshold is a decision, while
assessed against no threshold it is a negotiation, and negotiations are won by whoever has most
invested.

That establishes *why* a criterion works. It says nothing about **where the number goes**, and a
criterion set at the wrong level is worse than none, because it stops the wrong projects with full
procedural authority. Setting it is an arithmetic problem with two error costs.

**Worked example 2.4.3 — calibrating the kill criterion, and the two errors it trades.**

1. **Setup.** Meridian's successor programme carries a kill criterion at the **month-6** gate, by
   which point **USD 900,000** of the 2,400,000 has been committed and **USD 1,500,000** remains. The
   month-6 signal is early adoption in the first cohort, which classifies a programme as **Weak** or
   **Strong**. From the authority's own history of comparable rollouts, eventual sustained adoption
   falls into three states — **Low 25 %** (probability 0.25), **Mid 45 %** (0.45), **High 70 %**
   (0.30) — and the signal is imperfect: it reads Weak for **80 %** of Low programmes, **35 %** of Mid
   programmes and **10 %** of High programmes. All figures are **locally calibrated judgements from a
   small sample and must be labelled as such.** Should the criterion stop programmes flagged Weak?
2. **Formula.** Forward NPV at the gate `= PV of remaining benefit − remaining cost`, where the PV of
   the remaining benefit at sustained adoption `s` is `5,332,711 × s` (the ramped PV per unit of
   sustained adoption, 2.2.2). Forward breakeven adoption `= remaining cost ÷ 5,332,711`. Then compare
   two policies: **A** continue everything, `E[NPV] = Σ P(s) × NPV(s)`; **B** stop on Weak,
   `E[NPV] = Σ P(s) × P(Strong|s) × NPV(s)`, taking a stopped programme's forward value as zero.
3. **Substitution.** Forward breakeven `1,500,000 / 5,332,711`. `NPV(0.25) = 1,333,178 − 1,500,000`;
   `NPV(0.45) = 2,399,720 − 1,500,000`; `NPV(0.70) = 3,732,898 − 1,500,000`. Policy B:
   `0.25 × 0.20 × (−166,822) + 0.45 × 0.65 × 899,720 + 0.30 × 0.90 × 2,232,898`.
4. **Result.** **Forward breakeven adoption at this gate is 28.1283 %**, against a whole-investment
   breakeven on the same ramped basis of **45.0053 %**.

   | Eventual state | Prior | Forward NPV | P(Weak) | Effect of stopping on Weak |
   |---|---|---|---|---|
   | Low 25 % | 0.25 | **(166,822)** | 0.80 | loss avoided **+33,364** |
   | Mid 45 % | 0.45 | 899,720 | 0.35 | value destroyed **(141,706)** |
   | High 70 % | 0.30 | 2,232,898 | 0.10 | value destroyed **(66,987)** |

   Policy A (continue everything) **E[NPV] = USD 1,033,038**. Policy B (stop on Weak)
   **E[NPV] = USD 857,709**. The criterion **destroys USD 175,328** of expected value: it saves 33,364
   and costs 208,693.
5. **Interpretation.** Six readings. The first two are the calibration errors that produce most bad
   criteria; the last reconciles this result with everything 2.4.3 says in favour of kill criteria.

   **A criterion set at the investment breakeven kills programmes that are still worth completing.**
   The intuitive threshold ("stop unless it will clear the adoption the business case needed") is
   **45.0053 %** on this basis. The threshold that maximises value at *this* gate is **28.1283 %**,
   because 900,000 is already sunk and the decision is about the next 1,500,000 only. A criterion
   pitched at the investment breakeven condemns every programme whose eventual adoption lands
   between 28.1283 % and 45.0053 % (a **16.8770-point** band containing the entire Mid state) even
   though completing them returns 899,720 each. **The forward breakeven is the only defensible level
   for a criterion applied at a gate**, and the fact that it moves later in delivery is a feature: a
   criterion should get harder to trigger as the remaining cost falls, which is the exact opposite
   of how most escalating governance behaves.

   **The signal, not the threshold, is what makes this criterion fail.** Even at the right threshold,
   the criterion is applied to a proxy. Of the programmes it flags Weak, most are not Low: the flagged
   population is `0.25 × 0.80 = 0.20` Low, `0.45 × 0.35 = 0.1575` Mid and `0.30 × 0.10 = 0.03` High,
   so **48.39 %** of everything it stops would have returned positive value. The arithmetic requirement
   is severe and worth stating exactly: even a criterion that detected **every** Low programme and
   never flagged a High one could tolerate a false-flag rate on Mid programmes of no more than
   `41,706 / 404,874 =` **10.3009 %**. A proxy signal that good does not exist at month six on a
   two-year rollout, which is the honest reason early adoption thresholds so often do damage.

   **The asymmetry is structural, not a quirk of these numbers.** The loss avoided by stopping a bad
   programme is bounded by how negative its forward NPV is (here 166,822), while the value destroyed
   by stopping a good one is its whole remaining upside, 899,720 or 2,232,898. When the upside is an
   order of magnitude larger than the downside, a criterion has to be far more accurate than
   intuition suggests. The corollary, and it is important, is that **the asymmetry reverses where
   the downside is large**: a programme whose failure carries a regulatory penalty, a safety
   consequence or a contractual liability has a much more negative forward NPV, and the same signal
   quality then makes the criterion clearly worth having. **Kill criteria pay where failure is
   expensive, not where success is uncertain.**

   **The fix is not to abandon the criterion but to change what it does.** Three changes each make
   it value-positive. Set the threshold on the **decision-relevant quantity** (forward NPV computed
   from the re-based benefit case) rather than on a proxy, which is what the third of 2.4.3's
   example criteria ("if remaining cost exceeds remaining benefit at any gate") already does and is
   why it is the strongest of the three. Where a proxy is unavoidable, make the criterion trigger a
   **mandatory re-appraisal with a real stop option** rather than an automatic stop: the
   re-appraisal costs a few tens of thousands and removes the false-stop loss almost entirely, which
   on these figures is a trade of 208,693 against a fraction of it. And **improve the signal by
   buying information before the money is committed**, which is precisely the staged commitment
   2.A.1 prices, and the reason real options and kill criteria are two halves of one instrument
   rather than two topics.

   **What this does not license.** Nothing here argues against pre-agreed criteria, and the
   reasoning must not be borrowed to defend a portfolio that never stops anything. The result is
   narrower and sharper: **a criterion is an instrument with a computable expected value, and it
   must be calibrated rather than asserted.** The gate that has never stopped a project (MCQ 2.4-B)
   is failing a different test: it has no stop option at all, and this arithmetic assumes throughout
   that the option exists and can be exercised. A leader who cannot compute the two error costs
   should still insist on criteria; a leader who can should insist on the right ones.

   **The limits of the model.** It assumes a stopped programme has zero forward value, which
   understates Policy B wherever partial delivery is usable or the released capacity has an
   alternative use worth more than nothing: reinstating either shrinks the 175,328 and could reverse
   it, and the honest version of this table has a redeployment value in it. The three states and the
   signal rates come from a small internal sample, so the conclusion should be tested across a range
   rather than taken at a point; the direction of the result is robust to plausible variation, its
   magnitude is not. And it prices only expected value, so it says nothing about a board's
   willingness to carry the variance, which is a legitimate risk-appetite judgement belonging to the
   sponsor (Domain 8, KA 8.4).

> **Fig 2.4.1 — Why a kill criterion on a weak signal destroys value.** Two columns, y-axis expected
> value USD 0–240,000. The left column (brand blue) is the value saved by correctly stopping Low
> programmes, **33,364**. The right column (crimson) is the value destroyed by wrongly stopping good
> ones, **208,693**, split into Mid **141,706** and High **66,987** and labelled. A heading above the
> pair reads "Net effect of the criterion: **USD (175,328)**". A right-hand panel prints the forward
> breakeven adoption at the gate (**28.1283 %**), the ramped-basis investment breakeven
> (**45.0053 %**), Policy A's expected NPV (**1,033,038**), Policy B's (**857,709**) and the maximum
> false-flag rate on Mid programmes a perfect Low detector could tolerate (**10.3009 %**). Source: PCI
> original. Alt text: a small blue column of value saved beside a much larger crimson column of value
> destroyed, showing that a kill criterion based on an early proxy signal loses more than it protects.

An **honest gate** has four properties: a **real stop option** with someone authorised to exercise
it; **evidence prepared to a standard set beforehand**, not assembled to support a conclusion;
**decision-makers who did not write the case**; and **a recorded decision with reasons** (Domain 3's
auditability). A gate that has never stopped anything is a milestone with a meeting attached, not a
control.

**Stopping well** is its own competence: capture what was learned and make it findable (Domain 9's
lessons); harvest anything reusable (code, designs, cleaned data, supplier relationships, qualified
people); close contracts deliberately rather than by abandonment (Domain 10); redeploy the team with
their reputations intact; because how a cancellation is handled determines whether anyone proposes
the next one; and tell stakeholders honestly, early (Domain 11).

### AI in this KA

The relevant AI use here is monitoring: tracking assumption triggers and kill-criteria thresholds
across a portfolio, and flagging when a project's own reported data has crossed one. That is
genuinely valuable because the crossing is usually visible in data nobody is reading. What must
remain human is the decision and the *framing*: a model asked whether to continue will optimise
whatever objective it was handed, and the escalation-of-commitment bias lives in the humans around
it, not in the arithmetic. The leader's use of AI here is to make the trigger impossible to miss,
not to outsource the courage.

**Verification, concretely.** Monitoring is only as good as the threshold it monitors, so the first
check is on the criterion itself: is it set at the **forward** breakeven, recomputed at this gate,
or at the investment breakeven inherited from approval? A monitoring system faithfully alerting
against a mis-calibrated threshold industrialises the error of 2.4.3. Where a model is used to score
the two error costs of a candidate criterion, its signal-quality inputs (the false-flag rates) must
come from counted outcomes on comparable past projects, and where those do not exist the honest
output is the **breakeven signal quality** (10.3009 % on Meridian's successor) rather than a
recommendation: it tells a sponsor how good the signal would have to be, which is a decidable
question. Reproduce the two policy expectations by hand; each is three multiplications and a sum.
And no model output should ever be the trigger of an automatic stop: a stop is an attributable
decision by a named authority (Domain 3, KA 3.A.2), and the value of automation here is entirely in
making the crossing visible on the day it happens rather than in the quarter it is noticed.

### Key terms — KA 2.4

| Term | Meaning |
|---|---|
| **Forward-looking NPV** | Remaining benefit PV less remaining cost; the only basis for continuation. |
| **Sunk cost** | Unrecoverable spend carrying no information beyond the forecast; irrelevant to a continuation decision. |
| **Forward exit cost** | The cost of stopping: termination, demobilisation, disposal; a forward cost, never sunk. |
| **Forward breakeven** | The adoption or benefit level at which the *remaining* cost is just repaid; the only defensible level for a gate criterion. |
| **Escalation of commitment** | Continuing because of what is invested; countered structurally. |
| **Kill criteria** | Pre-agreed stop conditions; powerful only because agreed in advance, and only useful if calibrated. |
| **False stop / false continue** | Stopping a project that would have created value; continuing one that will not. The two errors a criterion trades. |
| **Honest gate** | Real stop option, pre-set evidence standard, independent deciders, recorded reasons. |

### Sample MCQs — KA 2.4

**MCQ 2.4-A `[2.4.2 · Application]`** 1,800,000 is spent; completion needs a further 900,000;
remaining benefit PV is 780,000. The correct decision and its basis are:
- A. continue — stopping wastes the 1,800,000 already spent
- B. stop — forward NPV is (120,000); the sunk 1,800,000 is irrelevant to the remaining choice ✅
- C. continue — total spend 2,700,000 against total benefits exceeds the original case
- D. continue at reduced pace to spread the cost

*Rationale:* Only remaining cost and benefit bear on the decision (2.4.2). A is the sunk-cost
fallacy stated plainly; C reintroduces sunk cost as "total"; D changes the schedule without
changing the negative economics.

**MCQ 2.4-B `[2.4.3 · Analysis]`** A portfolio's gates have never stopped a project in four years.
The soundest inference is:
- A. selection is excellent, so no project has needed stopping
- B. the gates are not functioning as controls — no real stop option, evidence assembled to support continuation, or deciders too close to the cases ✅
- C. the gate criteria are too lenient and should be tightened numerically
- D. stopping is unnecessary if delivery is well managed

*Rationale:* A control that never fires is not demonstrably a control (2.4.3). A is implausible
across a whole portfolio; C addresses thresholds when the defect is process and authority; D
mistakes delivery quality for strategic validity.

**MCQ 2.4-C `[2.4.3 · Recall]`** Kill criteria derive their power from:
- A. the severity of the thresholds
- B. being agreed in advance, so the same evidence produces a decision rather than a negotiation ✅
- C. being set by the project manager
- D. being confidential until invoked

*Rationale:* Advance agreement converts assessment into decision (2.4.3); without it, whoever has
most invested wins the argument. C removes independence; D prevents the behavioural effect entirely.

**MCQ 2.4-D `[2.4.3 · Application]`** At a gate, USD 900,000 is spent and USD 1,500,000 remains; the
PV of remaining benefit is `5,332,711 × s` for sustained adoption `s`. The adoption at which the
remaining spend is just repaid is:
- A. 45.0053 %
- B. 28.1283 % ✅
- C. 41.0460 %
- D. 62.5000 %

*Rationale:* `1,500,000 / 5,332,711 = 28.1283 %` (2.4.3). A is the *whole-investment* breakeven on the
same ramped basis, which reintroduces the sunk 900,000 into a forward decision; C is the
flat-equivalent whole-investment breakeven, wrong on both counts; D divides remaining cost by total
cost, which is a completion percentage and not a breakeven at all.

**MCQ 2.4-E `[2.4.3 · Evaluation]`** A criterion flags 80 % of Low-adoption programmes, 35 % of Mid and
10 % of High. Priors are 0.25 / 0.45 / 0.30 and forward NPVs (166,822) / 899,720 / 2,232,898. Stopping
on a flag:
- A. saves USD 33,364 of expected loss, so it is worth applying
- B. destroys USD 175,328 of expected value, because the value of the good programmes it stops exceeds
  the loss of the bad ones it prevents ✅
- C. is value-neutral, since the criterion is applied consistently to every programme
- D. cannot be assessed without knowing each programme's actual outcome

*Rationale:* `33,364 − (141,706 + 66,987) = (175,328)` (2.4.3). A counts only the benefit side of
the trade, the standard omission; C confuses procedural consistency with expected value; D is the
argument that prevents any criterion from ever being calibrated, since actual outcomes are by
definition unavailable at the gate.

**MCQ 2.4-F `[2.4.2 · Analysis]`** Remaining cost is 900,000 and remaining benefit PV is 780,000, so
forward NPV is (120,000). Terminating the principal supplier would cost 150,000. The correct decision
is:
- A. stop, since forward NPV is negative
- B. continue, because the 150,000 exit cost is a forward cost of stopping and exceeds the 120,000 cost
  of completing ✅
- C. stop, and treat the 150,000 as a sunk cost of the original decision
- D. continue, because the 1,800,000 already spent would otherwise be wasted

*Rationale:* Exit costs are forward costs, so the comparison is (120,000) against (150,000) (2.4.2).
A applies the rule without completing the comparison; C misclassifies a future payment as sunk; D is
the sunk-cost fallacy, which reaches the same answer here for entirely the wrong reason, and would
reach the wrong answer if the exit cost were 50,000.

### Self-check — KA 2.4

1. *What is the only correct basis for a continuation decision?* — Remaining benefit against
   remaining cost; sunk cost excluded, forward exit costs included.
2. *Why is stopping a strategic act rather than a loss?* — It releases capacity and funding for
   candidates the portfolio could not otherwise support.
3. *Name two properties of an honest gate.* — A real stop option with authority; deciders who did
   not write the case (also: pre-set evidence standards; recorded reasons).
4. *Which three things are commonly misclassified as sunk cost?* — Recoverable value (resale,
   redeployment, reusable assets); forward exit costs; and the *information* an overspend generated,
   which should revise the remaining estimate.
5. *At what level should a gate criterion be set, and why does it move?* — At the forward breakeven
   (28.1283 % on Meridian's successor at month six against a 45.0053 % investment breakeven), and it
   rises as remaining cost falls, so a criterion should get harder to trigger over time.
6. *When do kill criteria clearly pay?* — Where failure is expensive rather than where success is
   uncertain: the false-stop loss scales with the upside forgone, so the trade improves as the downside
   grows.

---

## Advanced topics — Domain 2

### 2.A.1 Real options thinking in selection

Static NPV values a committed plan; much project value lies in **flexibility**: the option to scale
if a pilot succeeds, to defer while uncertainty resolves, to abandon cheaply, to switch technology.
Treating these as options changes selection behaviour in a specific way: a small, staged first phase
with a negative standalone NPV can be the highest-value choice if it buys the right to a large
second phase at a decision point where far more is known. The practical rule for a delivery leader
is **structure the work so that decision points exist**: phases sized to reach a genuine learning
milestone, contracts that permit exit (Domain 10), architectures that permit switching. Formal
option valuation is specialist; the *thinking* is not, and its absence is why organisations
routinely commit fully to things they could have tested for a fraction.

The thinking is also arithmetic, and doing the arithmetic once cures the two opposite errors it
attracts: treating a pilot as automatically prudent, and treating it as automatically a delay.

**Worked example 2.A.1 — staging Meridian, and what the option to abandon is actually worth.**

1. **Setup.** Two ways to acquire Meridian's 40 clinics. Sustained adoption will turn out to be
   **High, 70 %** (probability 0.60) or **Low, 35 %** (0.40), an honest statement of the uncertainty
   the case carried. The PV of the ramped benefit stream per unit of sustained adoption is
   **5,332,711** (2.2.2), so High is worth 3,732,898 and Low 1,866,449 of benefit PV. **Option F —
   full commitment:** 2,400,000 now for all 40 clinics, at 60,000 a clinic. **Option S — staged:** a
   first phase of **12 clinics** now for **840,000** (70,000 a clinic, a 16.6667 % premium, because
   a small deployment loses the scale of a rollout), which reveals the adoption state at the end of
   year 1; then either commit the remaining 28 clinics for **1,680,000** at the original unit rate,
   or abandon. Staging delays the whole estate's benefit stream by **one year**; if abandoned, the
   12 installed clinics stay in service and earn their 30 % share at the Low adoption. Rate 7 %
   throughout.
2. **Formula.** Value each option as a probability-weighted NPV at `t = 0`, discounting the year-1
   commitment and the shifted benefit stream by `1/1.07`. Then decompose:
   **price of staging** `= E[NPV of F] − E[NPV of staging with no abandonment option]`, and
   **value of the option to abandon** `= E[NPV of staging] − E[NPV of staging committed to complete]`.
   The verdict is the option's value less its price, which is identically `E[NPV of S] − E[NPV of F]`.
3. **Substitution.** F: `0.60 × (3,732,898 − 2,400,000) + 0.40 × (1,866,449 − 2,400,000)`.
   S, High branch: `−840,000 − 1,680,000/1.07 + 3,732,898/1.07`. S, Low branch (abandon):
   `−840,000 + 0.30 × 1,866,449/1.07`. S, Low branch if committed anyway:
   `−840,000 − 1,680,000/1.07 + 1,866,449/1.07`.
4. **Result.**

   | | High (0.60) | Low (0.40) | **Expected NPV** |
   |---|---|---|---|
| **F (full commitment**) | +1,332,898 | (533,551) | **+586,318** |
| **S (staged, abandon on Low**) | +1,078,596 | (316,697) | **+520,479** |
| S (staged, committed to complete) | +1,078,596 | (665,749) | +380,858 |

   **Price of staging USD 205,460** (delay plus the 120,000 scale premium). **Value of the option to
   abandon USD 139,621.** Net **(USD 65,839)**: staging loses money on these figures. Staging
   becomes the better choice once the probability of the Low state exceeds **53.9740 %**. Priced
   instead at the programme's own unit rate (720,000 for 12 clinics, no premium), staging is worth
   **+640,479** and beats full commitment by **54,161**. So so the **maximum justifiable first-phase
   cost is USD 774,161**, a premium ceiling of **7.5223 %** against the 16.6667 % assumed.
5. **Interpretation.** Six readings. The decomposition in the second is the transferable technique;
   the fourth is the actionable finding.

   **The option to abandon is real, computable and worth USD 139,621; and that is not the answer.**
   Everything the received wisdom says about piloting is true: the option has positive value, it is
   worth more the more uncertain the adoption, and abandoning after 840,000 is enormously better
   than abandoning after 2,400,000 (the Low branch improves from (665,749) to (316,697), a gain of
   **349,052**). None of that settles the decision, because the option has a **price**, and here the
   price is larger.

   **Always separate the price of staging from the value of the option, because they have different
   owners and different fixes.** The price, 205,460, is delay plus scale premium — both consequences of
   *how* the staging is designed, and both reducible by design. The value, 139,621, comes from the
   uncertainty and the abandonment right — properties of the *situation*, largely not in the leader's
   gift. Reporting one net figure hides the only lever that exists. **The professional move is not
   "should we pilot?" but "what would the pilot have to cost and how long could it take?"**

   **The whole result turns on 120,000 of scale premium, which is the actionable finding.** Remove
   the premium and staging wins by 54,161; keep it and staging loses by 65,839. The ceiling is
   **7.5223 %**, so any pilot priced above about a 7.5 % premium over the programme's unit economics
   destroys value on these probabilities. That converts an abstract exhortation, "structure the work
   so decision points exist", into a procurement instruction: **the pilot must be bought at rollout
   unit rates, or it is not a pilot but a small expensive project.** In practice that means
   negotiating the whole-estate rate up front with a phased commitment, which is Domain 10's
   framework-with-call-off pattern (KA 10.3), and it is the single highest-value thing a leader can
   do to make staging viable.

   **Delay is the other half of the price, and it is priced by the cost of delay, not by
   sentiment.** Shifting the whole benefit stream by a year costs `3,732,898 × (1 − 1/1.07) =`
   **244,208** in the High state. That is why staging fails most often on benefit-generating work
   with a high cost of delay (Meridian's USD 14,280 a week), and succeeds most often where benefits
   are far off or the downside is catastrophic. **A pilot that does not shorten its own decision
   point is paying full price for the option and taking delivery late.**

   **The breakeven probability is the sentence for the board.** "Staging pays if we think there is
   more than a 53.9740 % chance this does not achieve the adoption the case needs" is answerable in
   a room; "the real option is worth 139,621" is not. And it exposes something uncomfortable and
   useful: an organisation that wants to pilot everything is asserting that it usually expects
   failure, which may be true, and if it is, the correct response is to fix the selection process
   rather than to pay a premium on every programme to hedge it.

   **What this is not, and the honest limits.** This is an **expected-value decision tree** (Domain
   8, KA 8.2.2), not an option valuation: it discounts both branches at the same 7 % and takes the
   probabilities as given, where formal option pricing would treat the risk differently, PFL-AI
   Domain 4 sets out the appraisal machinery and its assumptions, and a genuinely large staged
   commitment warrants specialist valuation. It also assumes the pilot **resolves** the uncertainty,
   which is generous: a 12-clinic pilot with early adopters may say very little about clinic 37, and
   a pilot that only partly resolves the question is worth proportionately less, the same
   signal-quality problem 2.4.3 computes for kill criteria, and the reason the two topics belong
   together. It ignores the learning that makes phase 2 cheaper or better, which biases *against*
   staging, and it ignores the organisational cost of restarting a stalled programme, which biases
   *for* it. Finally, the two states are a simplification of a continuum; the sensitivity that
   matters (the premium ceiling) should be recomputed across the plausible probability range before
   it is quoted.

### 2.A.2 Portfolio balance, not just portfolio ranking

Ranking selects the best individual candidates; **balance** asks whether the resulting set is
survivable and coherent. Four balance questions: **risk profile** (is everything high-risk, or
everything safe and incremental?), **time-to-benefit** (does anything land this year, or is all
value in years 3–5?), **capability spread** (does the whole set depend on one scarce team: the
correlation problem of Domain 8, KA 8.A.1, at portfolio level?), and **strategic coverage** (are
some stated objectives served by nothing at all?). A portfolio of individually optimal projects can
be collectively unbalanced, and no scoring model detects it, which is why Domain 15 treats portfolio
balancing as its own discipline.

The alignment index of 2.1.1 sits on the *allocation* side of this line and answers none of the four
questions: a portfolio funded exactly to weight can still be entirely high-risk, entirely long-dated,
and entirely queued behind the one integration team whose marginal value 2.2.3b computes. **Read the
index for allocation, then ask the four balance questions separately**, because each has a different
remedy and the index cannot see any of them.

### 2.A.3 The reviewer's business-case eye

Invariants testable in an hour: a do-nothing baseline exists and is quantified; at least one option
could plausibly have won; costs carry a range and an accuracy class; **the benefits profile ramps**;
every benefit traces to a measured outcome with an owner outside the project; enabling changes are
named with their owners; no benefit is claimed by another case in the portfolio; assumptions have
test dates and falsifying triggers; risk exposure is quantified and reconciles to the register; kill
criteria exist and are numeric; and success criteria were agreed in advance. The single
highest-yield check is the benefits profile. It is wrong in most cases, and it is wrong in the
direction that flatters.

Six further checks, each of which the domain's arithmetic makes into a one-line test, and each of
which a case can fail while passing all of the above.

- **The breakeven carries its basis.** A breakeven adoption stated without "flat-equivalent" or
  "ramped-basis" is ambiguous by 3.9592 points on Meridian's figures (2.2.2). Ask which.
- **The counterfactual is priced, not asserted as zero.** Ask what doing nothing costs in years 3 to 8,
  and whether the avoided costs are genuinely avoided rather than deferred or claimed elsewhere
  (2.2.2b). Meridian's omission was worth 561,101, in the case's favour.
- **The benefit measurement has a comparison cohort, or says it has not.** Raw and attributable are
  different quantities, and the over-claim share is `counterfactual ÷ raw` (2.3.2).
- **Σ EMV of the assumption register against the NPV.** Meridian's ratio is 0.8062; anything near or
  above 1 means the case should be staged rather than committed (2.3.4).
- **The ranking's flip point is reported.** If a 3.33-point weight shift reverses the recommendation,
  the recommendation is a preference (2.2.3c).
- **Gate criteria are set at the forward breakeven, not the investment breakeven.** A criterion pitched
  at the latter condemns everything in a 16.8770-point band that is still worth completing (2.4.3).

---

## Industry variations — Domain 2

- **Public sector.** Appraisal is often mandated (published guidance, prescribed discount rates,
  optimism-bias uplifts), social value and distributional effects sit beside financial NPV, and
  political commitment can precede the case, making 2.4's stopping problem acutely hard and 2.1.3's
  re-testing acutely necessary. The specific arithmetic consequence is that a mandated uplift is a
  **breakeven** change, not a presentational one: a 20 % optimism-bias uplift on Meridian's cost
  takes it to 2,880,000 and the flat-equivalent breakeven adoption from 41.0460 % to **49.2552 %**,
  a rise of **8.2092 points** that puts the achieved 40 % decisively out of reach. A leader working
  under a mandated uplift should therefore compute the breakeven *after* it and manage adoption to
  that figure from day one, rather than discovering at evaluation that the standard was moved before
  approval.
- **Regulated utilities.** Investment cases are made to a regulator on a periodic cycle; benefits
  are defined by the regulatory framework, and the "customer" for the case is external, so the
  discount rate is set by the regulatory determination rather than chosen. That makes rate
  sensitivity a live governance figure: on Meridian's shape, one percentage point of discount rate
  (7 % to 8 %, `AF` 5.971299 to 5.746639) moves the flat-equivalent breakeven from 41.0460 % to
  **42.6507 %**, a rise of **1.6047 points**. It also means the supersession hazard of 2.1.3 is low
  within a price control and spikes at its boundary, which is where the alignment half-life should
  be recomputed rather than assumed.
- **Technology and product.** Real options thinking (2.A.1) is the native mode (staged funding,
  pilots, kill criteria on usage metrics), and benefits are frequently non-cash-releasing capacity
  or optionality, so 2.3.2's honesty about units matters most here. The premium ceiling is the
  figure to carry across: staging pays only while the first phase can be bought near rollout unit
  economics (7.5223 % on Meridian's numbers), which in a product context usually holds, because a
  smaller first release is genuinely cheaper rather than merely smaller. And and that structural
  difference, not cultural preference, is why staging is the default here and not in construction.
- **Energy and infrastructure.** Whole-life and carbon considerations are first-order value (2.3.3),
  horizons are decades, and PFL-AI's bankability tests (its Domain 5) run alongside the internal
  case because external capital must also be convinced. Two of this domain's mechanisms invert here.
  The alignment half-life is short relative to everything else in the appraisal (a 30-year asset
  outlives every strategy that will ever be written about it), so alignment is managed by designing
  for optionality rather than by re-testing fit. And the scale premium on staging is usually
  prohibitive, because civil works do not divide, which is why the flexibility is bought in the
  *design* (spare capacity, provision for a second circuit) rather than in the commitment.
- **Health and social programmes.** Benefits are largely non-financial and attribution is genuinely
  hard; the professional response is to measure outcomes in their own units with pre-change
  baselines rather than to force monetisation. Meridian's own attribution correction is the worked
  illustration (a comparison cohort removed **17.1875 %** of a claim that had looked like an
  over-performance), and it is representative rather than exceptional: where a whole system is
  changing at once, the counterfactual is frequently a large fraction of the measured effect. Where
  withholding the change is not acceptable, a staged rollout gives the cohort as a by-product of the
  sequencing, which is usually the only defensible route available.

## Case study — Domain 2: the Meridian case that should have been written (public health)

**Situation.** Meridian's approved business case claimed **USD 979,200** of annual benefit from year
one (the full-potential figure), supporting an NPV of **+3,447,096** over eight years at 7 % against
a delivery cost of 2,400,000. Every number in it was arithmetically correct. No adoption term
appeared anywhere, and no benefit had an owner outside the programme.

**What the honest case looks like.** Same cost, same horizon, same rate, same 70 % steady-state
adoption Domain 1 establishes; but profiled as it would actually arrive (40 % / 60 % / 70 %): PV of
benefits **3,732,898**, NPV **+1,332,898**. The approved case overstated present value by **USD
2,114,198**, or **158.6 %** of the honest NPV. It also omitted the sentence that would have mattered
most: **breakeven sustained adoption is 41.05 %**, so the programme creates value at any adoption
above roughly 41 %, a threshold a board can monitor, unlike an NPV.

**What followed, and why it was predictable.** Both cases approve the programme, so the flat profile
changed no decision, and was therefore never challenged. Two years later, delivery complete and
adoption at 40 %, the programme was measured against a promise of 979,200 and publicly judged a
failure (Domain 1's case study). Actual annual benefit at 40 % adoption was **391,680**, and against
the case as approved the programme's NPV at that adoption is `0.40 × 5,847,096 − 2,400,000 =` **(USD
61,162)**: a small negative, and the arithmetic that made the failure verdict defensible.

**The verdict does not survive an honest baseline.** Two corrections were available at approval and
neither was made, and they run in opposite directions.

| Treatment of the same facts | Flat-equivalent breakeven adoption | NPV at the achieved 40 % |
|---|---|---|
| The case as approved (zero counterfactual, 6.0 h assumed) | 41.0460 % | **(61,162)** |
| Attribution corrected only (5.3 attributable hours) | 46.4672 % | (334,026) |
| Counterfactual priced only (do-nothing costs 561,101 of PV) | 31.4498 % | +499,939 |
| **Both corrections: the honest case** | **35.6035 %** | **+227,074** |

The programme was **1.05 percentage points short** of the breakeven it was judged against and
**4.3965 points clear** of the breakeven that was true. Meridian at 40 % adoption created **USD
227,074** of value and was publicly recorded as a failure. **The verdict measured the business case,
not the programme**. And and the two errors that produced it are of opposite sign, which is why a
reviewer who only knows that cases are optimistic would have found one of them and made the finding
worse.

**A third figure was available and would have changed the monthly conversation.** At the assumed 70
% adoption the honest case's assumption register carried **USD 1,074,548** of expected exposure
against an NPV of 1,332,898 (a ratio of **0.8062**) of which **USD 390,184** could have been
resolved for nothing before approval by a letter to the legacy vendor, a written funding
confirmation for the enabling change and one conversation with the finance business partner about
the valuation rate (KA 2.3.4). None of the three was done.

**What was done differently on the successor programme.** The case carried a ramped profile with
adoption as a tracked measure; a named benefits owner in the clinical directorate; the enabling
changes (training, workflow redesign, champions) itemised with their own owners and costs; the
breakeven adoption stated on the front page; and a kill criterion at 50 % adoption by month six.
The board's monthly question changed from "are clinics live?" to "what is adoption, against 41 %?".

**What the domain teaches here.** A business case is a promise, and the profile is the promise's
shape. An overstatement that changes no decision still changes the standard the programme will be
held to, which makes the flat-profile error uniquely dangerous, because nothing at approval time
creates any pressure to catch it.

## Case study B — Domain 2: the platform that could not be stopped (financial services)

**Situation.** A core-platform replacement, approved at USD 40m over three years, reached year four
having spent USD 62m with roughly 55 % of scope delivered. A re-based assessment put remaining cost
at USD 21m and remaining benefit present value at USD 14m, a forward NPV of **(USD 7m)**.

**What happened at the gate.** The paper presented total investment (62m spent, 21m to go) against
total lifetime benefits, showing a positive "programme return", and recommended continuation. Three
structural features made that recommendation almost inevitable: the gate had **no stop option**
defined; the paper was written by the programme's own director; and there were **no kill criteria**
from the original approval to test against. The sponsor who had championed the original case chaired
the gate.

**How it was resolved.** An independent review, commissioned only after a regulator asked about
delivery timescales, reframed the decision as forward-only: 21m to buy 14m. The programme was
descoped to a variant whose remaining benefit (9m) exceeded its remaining cost (6m), the balance of
capacity was redeployed, and the write-off was taken and disclosed. The descoped variant delivered
in eleven months.

**The arithmetic of the reframing, because it is the transferable part.** Continuing as planned had
a forward NPV of `14 − 21 =` **(7m)**. The descoped variant has `9 − 6 =` **+3m**. The swing between
the two decisions available at that gate is therefore **10m**, which is **25 %** of the programme's
original 40m approval and **16.13 %** of the 62m spent, a decision larger than most of the change
requests the programme had spent four years processing. Two secondary figures made the descope
defensible rather than merely attractive. The **discarded 5m of remaining benefit** costs 15m of
remaining spend, a ratio of **0.3333**: value per unit of spend far below the 1.5 the retained scope
achieves, which is the same value-per-unit-of-constraint reasoning as KA 2.2.3 applied inside a
single programme rather than across candidates. And the **released capacity had a named use**: the
15m and the integration team went to two funded candidates, so the board was not asked to accept a
write-off with nothing on the other side of it.

**The gate criterion that would have caught it years earlier.** A criterion set at the forward
breakeven (*remaining benefit must exceed remaining cost at every gate*) is the one criterion in
2.4.3's list that needs no proxy signal and no calibration, because it is stated in the decision's
own units. On this programme it would have been breached well before year four, and it is available
to any gate willing to compute two numbers.

**What the domain teaches here.** Escalation of commitment is structural before it is personal:
absent kill criteria, an independent decider and a real stop option, a rational actor presenting a
"total investment" frame will continue. Note that the resolution was not "stop everything":
descoping to the part where remaining benefit exceeds remaining cost is the same arithmetic applied
with more imagination.

---

## Executive perspective — Domain 2

What a project leader cannot delegate in this domain:

- **The benefits profile.** Not the NPV, the *shape*. The leader who lets a flat full-potential
  profile through has set the standard their programme will be judged against, and Meridian is what
  that costs.
- **The breakeven sentence, with its basis.** Stating the condition on which value depends (41.0460 %
  flat-equivalent, 45.0053 % ramped-basis) in a form the board can monitor monthly, and never quoting
  one while meaning the other.
- **The counterfactual.** Insisting that doing nothing is appraised rather than assumed to cost
  zero. It is the one error that runs in the leader's favour, which is exactly why nobody else will
  find it, and on Meridian it was the difference between a recorded failure and a 227,074 success.
- **The enabling change and who owns it.** Naming the non-project work that converts outputs into
  outcomes, and securing owners for it outside the project, before approval rather than after.
- **The honest options set.** Ensuring at least one option could have won, including doing nothing.
- **The priced assumption register.** Two acts nobody else will perform: clearing the zero-cost rows
  before the paper is written (390,184 of Meridian's 1,074,548, because after approval they stop
  being questions and become risks), and then saying the ratio out loud, since "NPV +1.33m against
  1.07m of expected exposure in the assumptions it rests on" is a different sentence from "NPV
  +1.33m".
- **Forward-only framing.** Refusing every "total investment" frame at every gate, and personally
  insisting on remaining cost against remaining benefit, including the forward exit cost, which is
  not sunk.
- **Kill criteria at approval, calibrated rather than asserted.** The leader is the only person who
  can get them agreed while goodwill is high; after the first slip, nobody will. And the level matters
  as much as the existence: set at the forward breakeven, not at the investment breakeven, or the
  criterion will stop programmes that are still worth completing.
- **The price of staging, separated from the value of the option.** Piloting is not automatically
  prudent. The leader owns the two numbers that decide it: what the first phase costs above rollout
  unit rates, and how long it delays everything.

## Calculation exercises — Domain 2

**Exercise 2.1** A programme's full benefit potential is USD 600,000 per year, steady-state adoption
75 %, profiled 30 % / 60 % / 75 % thereafter, appraised over 6 years at 8 %. Delivery costs
USD 1,300,000. Compute the ramped NPV and the flat full-potential NPV, and the overstatement.
*Solution.* `AF(0.08, 6) = 4.622880`. Flat: `600,000 × 4.622880 = 2,773,728`; NPV **+1,473,728**.
Ramped: `180,000/1.08 + 360,000/1.08² + 450,000 ×` (years 3–6 factors 0.793832 + 0.735030 +
0.680583 + 0.630170 = 2.839615) `= 166,667 + 308,642 + 1,277,827 = 1,753,136`; NPV **+453,136**.
Overstatement **USD 1,020,592**. Common error: applying the steady-state percentage to all years
including the ramp period.

**Exercise 2.2** Using Exercise 2.1's flat form, find the sustained adoption at which NPV is zero.
*Solution.* Required annual benefit `= 1,300,000 / 4.622880 = 281,210`; as a share of the 600,000
potential, **46.87 %**. Common error: dividing cost by total undiscounted benefits (1,300,000 /
3,600,000 = 36.1 %), which ignores discounting and understates the threshold.

**Exercise 2.3** Candidates scored 1–5 on strategic fit (0.40), benefit (0.30), deliverability
(0.20), risk-inverse (0.10): P = 4/5/2/2; Q = 5/3/4/3. Rank them. *Solution.* P: `1.60 + 1.50 + 0.40
+ 0.20 =` **3.70**. Q: `2.00 + 0.90 + 0.80 + 0.30 =` **4.00**. **Q ranks first.** Common error:
unweighted summation (P = 13, Q = 15, same order here, but it is coincidence, and the method loses
the weights that were argued over).

**Exercise 2.4** Spent USD 4,200,000; remaining cost USD 1,600,000; remaining benefit PV USD
1,950,000. Decide, and state what the sunk-cost frame would wrongly add. *Solution.* Forward NPV
`1,950,000 − 1,600,000 =` **+USD 350,000 → continue.** The sunk-cost frame would add the irrelevant
4,200,000 to the comparison (as "total spend of 5,800,000 against benefits"), which here happens to
reach the same decision: the point being that it reaches it for the wrong reason and would reach the
wrong one whenever forward NPV is negative.

**Exercise 2.5** A directorate declares strategic weights of 45 % / 30 % / 25 % across three
objectives. Its strategically mapped spend is USD 12,000,000, funded 3,600,000 / 5,400,000 /
3,000,000. Compute the alignment index and the reallocation distance.
*Solution.* Funded shares **30.0000 % / 45.0000 % / 25.0000 %**. Index
`min(45,30) + min(30,45) + min(25,25) = 30 + 30 + 25 =` **85.0000 %**. Reallocation distance
`(1 − 0.85) × 12,000,000 =` **USD 1,800,000**, which is the single deficit on objective one
(`0.15 × 12,000,000`) and equals the single surplus on objective two. Common error: adding the deficit
and the surplus to report 3,600,000, which counts one movement of money twice.

**Exercise 2.6** A programme costs USD 1,500,000 and has a full benefit potential of USD 540,000 a
year, appraised over 6 %, 8 years. Doing nothing forces a mandatory USD 450,000 replacement in year
3 and USD 25,000 a year of workaround cost in years 4 to 8. Compute the flat-equivalent breakeven
adoption with and without the counterfactual. *Solution.* `AF(0.06, 8) = 6.209794`, so
full-potential PV is `540,000 × 6.209794 = 3,353,289`. Without the counterfactual, breakeven
`1,500,000 / 3,353,289 =` **44.7322 %**. Avoided cost PV `450,000/1.06³ = 377,828.68` plus `25,000 ×
3.536782 = 88,419.55` (the years 4–8 discount factors), total **466,248**. With it, breakeven
`(1,500,000 − 466,248) / 3,353,289 =` **30.8280 %**: an improvement of **13.9042 percentage
points**. Common error: treating the avoided costs as additional *benefits* and adding them to the
numerator of the adoption term, which mixes an adoption-dependent stream with one that is not
adoption-dependent and understates the breakeven further.

**Exercise 2.7** Candidates P (fit 4, benefit 5, deliverability 2, risk-inverse 2) and Q (5, 3, 4,
3) are scored under weights 0.40 / 0.30 / 0.20 / 0.10. Q leads. Find the shift of weight from
strategic fit to benefit value that flips the ranking, and state which criterion can never decide
the outcome. *Solution.* Totals P **3.70**, Q **4.00**: a margin of **0.30**. Shifting `δ` from fit
to benefit, P gains `δ(5 − 4) = +δ` and Q gains `δ(3 − 5) = −2δ`, so `3δ = 0.30` and `δ =`
**0.100000 — 10.0000 percentage points** (weights become fit 0.30, benefit 0.40; both score
**3.80**). Criterion influence on a 1–5 scale is `4 × weight`: **1.60 / 1.20 / 0.80 / 0.40**, so the
risk criterion can move a total by at most **0.40** across its whole range, and closing the 0.30
margin through it would need a **3.00-point** swing in the risk scores (`0.30 / 0.10`):
three-quarters of the entire scale, which no assessor can defend. Common error: reporting only the
ranking. A 10-point flip point is defensible where a 3-point one is not, and the reviewer cannot
tell which case they are looking at unless it is stated.

**Exercise 2.8** A case with an NPV of USD 1,100,000 rests on four assumptions whose impacts if
false are 900,000 (P 0.30), 400,000 (0.45), 250,000 (0.20) and 150,000 (0.60). Compute the
assumption exposure ratio and state what it implies. *Solution.* EMVs **270,000 / 180,000 / 50,000 /
90,000**, total **USD 590,000**. Exposure ratio `590,000 / 1,100,000 =` **53.64 %**. The case is
comfortably right-side-up (about two-thirds of Meridian's 0.8062), so it can be committed rather
than staged, though the 270,000 entry still warrants a test date and a falsifying trigger. Common
error: deducting the 590,000 from the NPV to report a "risk-adjusted NPV" of 510,000. The EMVs
describe exposure *around* a forecast that already embodies its own expectations; deducting them
double-counts, and it also hides the ratio, which is the useful output.

**Exercise 2.9** Four candidates compete for a scarce integration team: R needs 3 units for an NPV
of 2,100,000; S needs 2 for 1,500,000; T needs 2 for 1,350,000; U needs 1 for 800,000. Capacity is 4
units. Find the optimal set, the shortfall a greedy per-unit ranking incurs, and the marginal value
of a fifth unit. *Solution.* Per-unit values: U **800,000**, S **750,000**, R **700,000**, T
**675,000**. Greedy takes U (1 unit), then S (2 units), then cannot fit R or T — total **2,300,000**
with one unit idle. Enumerating the feasible sets at 4 units gives `{R,U}` = **2,900,000**, `{S,T}`
= 2,850,000, `{S,U}` = 2,300,000, `{T,U}` = 2,150,000, `{R}` = 2,100,000. The optimum is **{R, U} =
2,900,000**, so greedy gives up **USD 600,000, 20.69 %** of the available value. At 5 units the best
set is `{S,T,U}` = **3,650,000**, so the marginal value of the fifth unit is **USD 750,000**. Common
error: stopping at the per-unit ranking because it is the method the constrained example in KA 2.2.3
introduces. Ranking explains an answer; enumeration finds it.

## Practitioner's toolkit — Domain 2

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 2.T.1 — Business-case skeleton (the six questions)

**Why** (driver named from 2.1.2, evidence, do-nothing consequence) · **Options** (including
do-nothing and one materially cheaper; why each was rejected) · **Cost** (range, accuracy class,
whole-life not just capital) · **Benefits** (map per 2.T.2; **profiled, not flat**; owners outside
the project; measures and baselines; cash-releasing or not, stated) · **Risk** (quantified exposure
reconciling to the register) · **Judgment** (success criteria agreed now, kill criteria numeric,
gate schedule). Front page carries the **breakeven sentence**, not only the NPV.

### Toolkit 2.T.2 — Benefits map and register row

Per benefit: description · **the outcome measure that produces it** and its owner (outside the
project) · **enabling changes required**, each with an owner and whether funded · baseline value,
measurement method and date measured · profile by year (**ramp explicit**) · cash-releasing or
capacity · attribution note · single-claimant confirmation against the portfolio register ·
review cadence. Three further fields wherever the benefit may leave the organisation: **whether it is
externally disclosed** and in what (report, bid, regulatory submission, financing document) · the
**stated boundary and method** on which the figure rests, with what is excluded · and the **approving
function and record class**, since the project supplies a disclosed claim and never approves one
(2.3.3; Domain 16, KA 16.4.4 for the retention).

### Toolkit 2.T.3 — Gate decision pack rules

One page of forward-only economics (**remaining cost, remaining benefit, forward NPV**, and the
**forward exit cost** of stopping: total spend may be reported for information but never as the
decision basis) · the **forward breakeven** at this gate, recomputed, with the current measure
against it · kill criteria with current values against thresholds · assumption register extract
showing which assumptions have been tested and which falsified · alignment re-test against current
strategy · options at this gate, including stop and descope, each with its own forward NPV · decider
names (none of whom authored the case) · recorded decision and reasons.

### Toolkit 2.T.4 — Assumption register row, and the test-order rule

Per assumption: the statement, in one falsifiable sentence · why it is believed, with the evidence
named · **impact if false, in present-value terms** · probability, with who assessed it (never the
case's author alone) · **EMV** · **cost to test**, including "nil" where the test is a letter or a
conversation · **EMV per unit of test cost**, which sets the order · owner · test date, in a named
person's calendar · the **falsifying trigger**: the observation that would settle it · whether the
entry is correlated with another (mark the pairs; expectations add, tails do not) · and whether
failure is recoverable or changes what the project is.

Two rules make the register an instrument rather than a list. **Sort by EMV per unit of test cost
and clear the free rows before the paper is written**: on Meridian that was 390,184 of exposure
resolvable in a week. And **report `Σ EMV ÷ NPV` on the case's front page**: as it approaches one,
the recommendation should change from "approve" to "stage" (2.3.4, 2.A.1).

## Exam preparation — Domain 2

**The traps.** A flat full-potential benefits profile (2.2.2 — the domain's central error) ·
applying steady-state adoption to the ramp years (Exercise 2.1) · computing breakeven on
undiscounted benefits (Exercise 2.2) · **quoting a flat-equivalent breakeven while meaning a
ramped-basis one, or the reverse** (2.2.2 — worth 3.9592 points on Meridian) · **valuing the
do-nothing option at zero** (2.2.2b) · **using undiscounted avoided costs** (MCQ 2.2-E) ·
**subtracting an avoided cost instead of adding it** (MCQ 2.2-E) · reading sunk cost into a
continuation decision (2.4.2, Exercise 2.4) · **misclassifying a forward exit cost as sunk**
(MCQ 2.4-F) · reporting non-cash-releasing benefits as savings (2.3.2) · **claiming a raw improvement
where a comparison cohort shows part of it was happening anyway** (2.3.2) · **applying the adoption
term twice, once through the clinic count and once as a percentage** (2.3.2) ·
double-counting a benefit across an enabler and its dependents (MCQ 2.3-C) ·
**deducting assumption EMVs from an NPV to produce a "risk-adjusted" figure** (Exercise 2.8) ·
treating a soft constraint as hard (2.1.2) · ranking by raw NPV when a resource constraint binds
(MCQ 2.2-C) · **taking a greedy per-unit ranking for an optimum** (2.2.3, Exercise 2.9) ·
**judging capacity one unit at a time when the marginal value is non-monotone** (2.2.3b) ·
**adding deficits to surpluses when reporting a reallocation distance** (Exercise 2.5) ·
**treating an annual hazard as a continuous rate when computing a half-life** (MCQ 2.1-D) ·
**setting a gate criterion at the investment breakeven rather than the forward breakeven** (2.4.3) ·
mistaking a gate that has never stopped anything for a functioning control (2.4.3).

**Reflection questions.**
1. Take your current business case: is the benefits profile flat? What is the breakeven condition,
   and could you state it in one sentence to a board, with its basis?
2. What does doing nothing actually cost your organisation over the appraisal horizon? If the answer
   is "nothing", name the contract, licence or asset that will not need attention in the next five
   years.
3. Which enabling changes does your case depend on, and does each have a funded owner outside your
   project? What happens to the benefits if they do not?
4. List your case's five material assumptions with an impact and a probability. What is `Σ EMV ÷ NPV`,
   and which of the five could you resolve this week for nothing?
5. What are your project's kill criteria, and if there are none, who would have to agree them, and
   would they agree today? If there are, are they set at the forward breakeven or at the investment
   breakeven, and what would each of the two errors cost?
6. If your programme were staged instead of committed, what premium would the first phase carry over
   your rollout unit rates, and how long would it delay the rest? Which of those two numbers could you
   negotiate away?

## Domain 2 summary

Work gets chosen through a portfolio process, and the quality of that choice depends on candidates
being described honestly enough to be compared, which makes a business case a decision instrument
rather than an advocacy document only when it could have concluded "no". How far the funded work
matches the stated strategy is measurable: an **alignment index** of `Σ min(declared, funded)` gave
76.6667 % on the mapped spend of Meridian's parent authority, 59.0000 % on all of it, and a
**reallocation distance** of **USD 4,200,000**, 1.75 Meridians, with the whole difference between
the two indices being the 40 % of money that serves no declared objective. Alignment granted at
approval then **decays**: at a 15 % annual supersession hazard the **alignment half-life is 4.2650
years**, so gates re-test the case and not merely delivery, and an annual re-test was worth **USD
292,800** against 342,000 of expected misaligned spend. The domain's central arithmetic is the
benefits profile: Meridian's approved case claimed full potential from year one for an NPV of
+3,447,096, where the same facts profiled as adoption actually arrives (40 % / 60 % / 70 % of a
979,200 potential) give **+1,332,898**: an overstatement of **USD 2,114,198**, 158.6 % of the honest
figure, which changed no approval decision and set the standard the programme was later judged to
have failed. The more useful board sentence is the breakeven, stated with its basis: **41.0460 %
flat-equivalent**, or 45.0053 % on the ramped basis. **The opposite error matters equally**: valuing
the do-nothing option at zero cost Meridian **USD 561,101** of present value and 9.5962 points of
breakeven headroom, while an honest attribution correction (a comparison cohort showing 1.1 of a
6.4-hour improvement was happening anyway, an over-claim of **17.1875 %**) moved it 5.4212 points
the other way. Corrected both ways the breakeven is **35.6035 %**, and Meridian at its achieved 40 %
adoption created **USD 227,074** of value while being publicly recorded as a failure. Selection
methods each have limits: weighted scoring exposes criteria and can be steered, and a
**3.3333-point** weight shift reverses this domain's ranking; ranking by value per unit of a binding
constraint beats raw NPV (Beta + Gamma's 2,100,000 over Meridian's 1,693,072) but remains a
heuristic for lumpy candidates, and the constraint's own marginal value is lumpy too — 900,000 ·
300,000 · 900,000 · 493,072 · 300,000 · 900,000, so capacity is a block decision, add one unit or
three and never two. Benefits require maps that include the **enabling change** most omit, baselines
measured before the change, comparison cohorts, single claimants, honesty about cash-releasing
versus capacity, and sustainability treated as constraint, as value or as a disclosed claim but
never confused between them, a disclosed claim carrying a stated boundary and method, a named owner,
retained evidence and the signing function's approval, and never being reported as achieved on
evidence its audience would not accept; and an assumption register is priced, not listed: meridian's
carried **USD 1,074,548** of expected exposure against a 1,332,898 NPV, a ratio of **0.8062**, of
which 390,184 was resolvable for nothing. And the discipline closes with stopping: continuation
depends only on remaining cost against remaining benefit, plus the forward exit cost (780,000 of
value for 900,000 of spend is a stop, whatever the 1,800,000 already gone suggests) made possible by
kill criteria agreed in advance, set at the **forward** breakeven (28.1283 % at Meridian's
successor's month-6 gate, not the investment breakeven of 45.0053 %) and calibrated against their
two errors, since a criterion on a weak proxy signal destroyed **USD 175,328** of expected value by
stopping good programmes to prevent bad ones. Where the uncertainty is genuine, stage instead:
Meridian's abandonment option was worth **USD 139,621** and its staging cost **USD 205,460**, so
staging paid only above a 53.9740 % chance of failure, or at a first-phase premium below **7.5223
%**. Domain 3 supplies the governance and decision rights this domain assumes; Domain 15 scales the
selection arithmetic to a portfolio; Domain 16 measures what it promised.
