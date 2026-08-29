# Domain 9 — Quality, Assurance and Continuous Improvement

## Why this domain exists

Domain 5 defined what the project must produce and how acceptance would be tested. Domain 6 built
the schedule, Domain 7 the money, Domain 8 the risk. Every one of them assumed something this
domain has to supply: **that the work will be right, and that somebody has priced being wrong.**

Quality is the discipline most often reduced to a virtue. Organisations declare a commitment to "the
highest quality", write a plan nobody reads, and discover in commissioning that quality is a budget
line, a capacity constraint and a schedule driver: all three set months earlier by people who did
not know they were setting them. The recurring failure is not indifference. It is the belief that
quality is free at the top and infinitely expensive to give up, so the only two available positions
are "we do not compromise on quality" and an unadmitted compromise. Neither is a management
position, because neither can be costed.

The domain's central claim is that **quality is an optimisation with an interior solution, and the
optimum is not zero defects.** Prevention, appraisal, internal failure and external failure move
against each other; their sum has a minimum; that minimum sits at a positive escape rate; and where
it sits is fixed by one ratio: what an escaped defect costs against what finding it costs. An
organisation that cannot compute that ratio is not choosing its quality regime but inheriting one.
KA 9.1 computes it. KA 9.2 shows where the money actually goes (into layers of containment whose
economics differ by an order of magnitude), and why rework is a *capacity* problem before it is a
cost problem, with a schedule consequence no cost-of-quality model contains. KA 9.3 turns to the
decisions: accepting work, disposing of nonconformance, and the arithmetic of sampling, where the
central and uncomfortable result is that **a passing sample routinely fails to exclude the defect
rate at which its own plan is the wrong plan.** KA 9.4 closes on improvement and on the two data
problems that now dominate delivery quality: the quality of the data a project runs on, and the
quality of the output an AI system hands a professional who must sign for it.

**Learning objectives.** After this domain a candidate can: distinguish quality from grade and both
from fitness for purpose, and write a quality objective that is measurable rather than aspirational;
decompose the cost of quality into prevention, appraisal, internal failure and external failure and
explain why internal failure cost *rises* before it falls; **compute total cost of quality across
candidate regimes, locate the interior minimum and state the range of external-failure unit cost
over which it holds**; explain why zero defects is not the objective and what would make it one;
design a containment chain and compute its escape fraction as the product of layer escape rates;
**compute the expected containment cost per introduced defect and use it as the breakeven price of
prevention**; show arithmetically why equal money on prevention beats equal money on appraisal, and
name the condition under which it does not; **convert a rework share of capacity into a duration
multiplier and a delay cost and explain why the relationship is hyperbolic**; run a formal acceptance
decision and select a nonconformance disposition with its authority; **make the cumulative-effect
test operable by fixing its threshold source, derivation, period, relatedness test, owner and
consequence on breach, and recognise where a disposition additionally engages an approval body
outside the project**; **assign acceptance and disposition records a class, a custodian and a
retention period with the source of that period named**; **compute what a clean
acceptance sample does and does not establish, derive the breakeven defective fraction from the
ratio of verification cost to failure cost, and test an acceptance plan for self-consistency**;
conduct root-cause analysis to the level at which the cause can be removed and price removal against
expected recurrence; **compute rolled throughput yield as the product of stage yields, target
improvement at the binding step and derive the uniform step yield an end-to-end target requires**;
assess data quality across dimensions and compute composite fitness; and specify a verification
regime for AI-produced work whose sample size is derived from the consequence of an escaped error
rather than from convenience.

**The master worked project.** Project Auriga continues from Domains 6, 7 and 8: the 25-week
control-systems upgrade for a regional utility, `BAC` **USD 4,000,000**, at week 13 showing `CPI`
0.91 and `SPI` 0.92 (Domain 7, KA 7.3), with a **cost of delay of USD 45,000 per week** and a
critical path through installation and testing and commissioning at zero float (Domain 6). This
domain attaches quality economics to it: a cost of quality of **USD 364,000** at the chosen regime
(9.1 % of `BAC`), a containment chain that lets **8** of **80** introduced control-logic defects
escape at **USD 12,000** each, a commissioning phase whose 30 % rework share turns a 4-week activity
into **4.7619 weeks**, and an acceptance sample of 20 instrument loops that cannot exclude a defect
rate of **13.91 %**. Auriga's engineer-week costs **USD 5,225** throughout, from Domain 7's blended
engineering rate of USD 130.625 per hour over a 40-hour week.

---

## Knowledge Area 9.1 — Quality planning

*Topics: 9.1.1 quality, grade and fitness for purpose · 9.1.2 the cost of quality and its interior
minimum · 9.1.3 quality objectives, metrics and tolerances.*

### 9.1.1 Quality, grade and fitness for purpose

**Definition.** **Quality** is the degree to which delivered work conforms to its stated
requirements. **Grade** is the level of capability, feature richness or material specification those
requirements ask for. The two are independent, and confusing them is the commonest conceptual error
in delivery: a low-grade product built exactly to a low-grade specification is high quality, and a
high-grade product riddled with nonconformances is not. A leader may legitimately reduce grade; that
is a scope decision under Domain 5, taken with the sponsor and recorded. A leader may never
legitimately reduce quality, because quality is conformance to what was agreed, and reducing it is
delivering something other than the thing promised while saying nothing.

**Fitness for purpose** is the third and hardest test: whether the conforming, correctly graded
output serves the use it was built for. Domain 5's Case study B (a public-sector system one hundred
per cent complete against specification and thirty-four per cent useful) is the canonical failure,
and the reason quality management cannot stop at conformance. Conformance is verifiable against a
document; fitness for purpose only against a user, an operating context and a benefit measure
(Domain 2). A regime that tests only the first will pass work that fails the second with complete
documentary integrity.

The reference points are worth naming precisely and describing in the book's own words. The **ISO
9000 family** supplies the vocabulary and the management-system requirements for quality (ISO 9001
being the certifiable requirements standard); it evidences that an organisation has defined and
follows processes, not that any deliverable is correct. **Statistical process control** and the
**Six Sigma** family of improvement methods supply measurement machinery for repetitive processes;
projects borrow their arithmetic (yields, sampling, capability) with care, because a project is not
a repeating process and its samples are small. Neither tradition answers the question this domain
treats as primary: *how much conformance is it worth buying?*

**Three distinctions to hold.** *Quality assurance* is process-directed and preventive.
*Quality control* is product-directed and detective. *Quality improvement* is directed at the causes
of the answers the first two give. They have different owners, cadences and costs, and an
organisation funding only the second will pay for the same defect indefinitely.

### 9.1.2 The cost of quality and its interior minimum

**Definition.** The **cost of quality** (`CoQ`) is the total cost incurred because the work might not
conform, plus the total cost incurred because it did not. It has four categories, and every serious
quality decision is a trade among them:

| Category | What it buys | Auriga examples | Behaviour as quality rises |
|---|---|---|---|
| **Prevention** | Fewer defects introduced | Design standards, supplier qualification, training, model-based design review, configuration control | Rises |
| **Appraisal** | Defects found before the customer | Peer review, factory acceptance test, site acceptance test, inspection, audit | Rises |
| **Internal failure** | Nothing: it is loss | Rework, retest, scrap, document re-issue, disruption inside the project | Rises, then falls |
| **External failure** | Nothing (loss plus reputation) | Field defects, warranty attendance, outage credits, client disruption, claims | Falls |

The first two are the **cost of conformance**, the last two the **cost of nonconformance**. Two
features of that table are routinely got wrong. Internal failure cost is **not monotone**: an
organisation that improves testing before it improves engineering finds *more* defects internally
and its internal failure cost rises, a movement almost always misread as deterioration, and the
commonest reason quality programmes are abandoned in their first period. And external failure is the
only category whose unit cost the project does not control, because it is set by the operating
context: the same defect costs a trifle in a marketing website and a great deal in a live utility
control system.

**Worked example 9.1.2 — Auriga's cost of quality, and where its minimum actually is.**

1. **Setup.** Auriga's control-logic and configuration scope can be delivered under five candidate
   regimes. For each, the estimating team has stated prevention spend, appraisal spend, the defects
   the regime would allow to be **introduced**, the fraction it would **detect** before handover, and
   the observed average internal correction cost per defect found (which rises as detection reaches
   deeper, later layers). An escaped defect in live operation costs Auriga's client **USD 12,000** on
   average — diagnosis, a site attendance inside an outage window, retest and the contractual service
   credit. `BAC` is **USD 4,000,000**.

   | Regime | Prevention | Appraisal | Introduced | Detected | Internal unit cost |
   |---|---|---|---|---|---|
   | R0 test at the end | 12,000 | 48,000 | 160 | 75.00 % | 1,000 |
   | R1 basic | 40,000 | 60,000 | 120 | 85.00 % | 1,300 |
   | R2 planned | 96,000 | 64,000 | 80 | 90.00 % | 1,500 |
   | R3 enhanced | 170,000 | 80,000 | 64 | 93.75 % | 1,650 |
   | R4 maximum | 285,000 | 100,000 | 50 | 96.00 % | 1,750 |

2. **Formula.** Per regime: internal failures = introduced × detected; escapes = introduced −
   internal failures; internal failure cost = internal failures × internal unit cost; external
   failure cost = escapes × external unit cost; `CoQ` = prevention + appraisal + internal failure +
   external failure. Choose the regime minimising `CoQ`.
3. **Substitution.** R2: internal failures `80 × 0.90 = 72`; escapes `8`; internal failure cost
   `72 × 1,500 = 108,000`; external failure cost `8 × 12,000 = 96,000`; `CoQ` =
   `96,000 + 64,000 + 108,000 + 96,000`.
4. **Result.**

   | Regime | Escapes | Prevention | Appraisal | Internal failure | External failure | **Total `CoQ`** | % of `BAC` |
   |---|---|---|---|---|---|---|---|
   | R0 | 40 | 12,000 | 48,000 | 120,000 | 480,000 | **660,000** | 16.5 % |
   | R1 | 18 | 40,000 | 60,000 | 132,600 | 216,000 | **448,600** | 11.2 % |
   | **R2** | **8** | 96,000 | 64,000 | 108,000 | 96,000 | **364,000** | **9.1 %** |
   | R3 | 4 | 170,000 | 80,000 | 99,000 | 48,000 | **397,000** | 9.9 % |
   | R4 | 2 | 285,000 | 100,000 | 84,000 | 24,000 | **493,000** | 12.3 % |

   The minimum is **USD 364,000** at regime **R2**, with **8 escaped defects**, not at the lowest
   attainable defect count.
5. **Interpretation.** Five things follow, and a leader who can state all five commands the quality
   budget rather than spectating.

   **The optimum is interior, and not zero.** R4 costs **USD 129,000** more than R2 to remove six
   further escapes: **USD 21,500 per escape avoided** against damage of USD 12,000, or **1.79
   times** the harm it prevents. The last defects are the expensive ones, so an organisation
   treating zero defects as the objective is committing to buy protection at roughly twice its
   value. The honest objective is not "no defects" but **"no defect whose prevention costs less than
   its consequence"**: a computable instruction, and the one this table executes.

   **The marginal test is the decision, not the total.** R1 → R2 costs **USD 60,000** more in
   conformance and returns **USD 24,600** of internal and **USD 120,000** of external failure, a net
   **USD 84,600** gain. R2 → R3 costs **USD 90,000** more and returns **USD 9,000** and **USD
   48,000**: a net **USD 33,000** loss. The rule: spend the next increment while its conformance
   cost is below the failure cost it removes, and stop at the first increment where it is not.

   **The range of optimality is what makes the recommendation defensible.** R2 stays optimal for any
   external-failure unit cost between **USD 3,540** (below which R1 wins) and **USD 20,250** (above
   which R3 wins), a **5.72-fold** band. Auriga's assumed USD 12,000 sits **3.39 times** above the
   lower bound and at **59.3 %** of the upper. That is the sentence for a steering committee,
   because it answers the only question worth asking about an estimated input: how wrong can it be
   before the recommendation changes? In a safety-related or heavily regulated context the
   escaped-defect cost is a multiple of USD 12,000: above USD 20,250 the optimum moves to R3 and
   above **USD 60,000** to R4. Same arithmetic, different context, different regime, which is the
   quantitative statement of why quality regimes are not transferable between sectors.

   **Internal failure cost rises before it falls, and this must be said in advance.** R0 → R1 raises
   it from 120,000 to **132,600** while total `CoQ` falls by **USD 211,400**: better detection finds
   defects that were escaping, and the cost moves from the external column to the internal one at a
   fifth of the unit price. A programme reported on internal failure cost alone will look like a
   failure in its first period, which is where such programmes are cancelled.

   **The cautions.** The introduced-defect counts and detection rates are **estimates from
   comparable work** and must be labelled as such; the conclusion is robust to the external unit
   cost across a wide band but not to a detection rate that has been asserted rather than measured.
   The model prices only what is in it: it excludes the **schedule** consequence of rework (KA 9.2.3
   computes that separately and it is material), the tail risk of a single catastrophic escape
   rather than an average one, and any licence or reputational consequence not expressible per
   defect. And the four categories must be **defined once and counted consistently**, because the
   easiest way to make a quality programme look successful is to reclassify internal failure as
   appraisal.

> **Fig 9.1.1 — Auriga's cost of quality and its interior minimum.** Stacked column chart, five
> columns for regimes R0–R4 on the x-axis (rising prevention and appraisal spend), y-axis USD 0 to
> 700,000. Each column stacks prevention, appraisal, internal failure and external failure; totals
> are labelled **660,000 · 448,600 · 364,000 · 397,000 · 493,000** with the escaped-defect count
> beneath each (**40 · 18 · 8 · 4 · 2**). A dashed total line joins the column tops, showing the U
> shape; the R2 total is ringed as the minimum and annotated **USD 364,000 — 9.1 % of `BAC`**, "not
> at zero defects: 8 escapes is optimal", and "optimal for external unit cost 3,540–20,250 (assumed
> 12,000)". A note records that internal failure peaks at R1 because better detection finds more
> before prevention lowers the count. Source: PCI original. Alt text: five stacked columns whose
> totals fall and then rise, with the third column ringed as the lowest total cost of quality,
> showing that the cheapest regime allows eight defects to escape rather than none.

### 9.1.3 Quality objectives, metrics and tolerances

**The plan's job.** A quality management plan is the record of five decisions, each with an owner
and a number. *What conformance means* for each deliverable class: the acceptance criteria of Domain
5, KA 5.4.2, referenced not restated. *What tolerance applies*, because a specification without a
tolerance is untestable and will be argued about at acceptance. *Which containment layers exist*,
with their coverage and expected detection rates (KA 9.2). *Who accepts, and on what authority* (KA
9.3). And *what it costs*: the four-category budget of 9.1.2, reconcilable to Domain 7's cost
baseline rather than a parallel document.

**Metrics that survive contact with delivery.** A quality metric earns its place if a named person
changes a decision when it moves. Four do, on Auriga: **escape rate** (defects reaching the next
stage or the customer — the primary outcome measure, and what 9.1.2 optimises); **first-time-right
rate** per step and rolled end to end (KA 9.4.2 — the primary process measure); **rework share of
capacity** (KA 9.2.3: the primary *schedule* measure, and the one most often absent); and **cost of
quality as a percentage of `BAC`** split four ways, of which Auriga's 9.1 % is the figure the
sponsor should know.

**Tolerance and the two errors.** Every tolerance trades rejecting conforming work against accepting
nonconforming work. It is therefore the same class of decision as Domain 3's delegation threshold (a
certain, recurring, invisible cost against an uncertain, occasional, highly visible one), and
organisations reliably optimise against the visible one. The countermeasure is the same: compute
both, state the breakeven, and let the accountable person choose knowing the price.

### AI in this KA

**Where it earns its place.** Assembling a four-category cost-of-quality picture from accounting
data never coded for it (mapping rework time codes, inspection orders, warranty claims and service
credits into the four categories) is a classification task over messy records and the single largest
practical obstacle to doing cost-of-quality work at all. Testing acceptance criteria and tolerances
for measurability and flagging those that cannot be tested. Sweeping a specification set for missing
tolerances. Running the 9.1.2 model across dozens of regime and unit-cost combinations to produce
the range-of-optimality band rather than a point estimate.

**Where it must not go.** Setting the external-failure unit cost, which is a judgement about
consequence in an operating context and, where safety or licence is involved, not an expected-value
judgement at all. Choosing the regime, a risk-appetite decision belonging to the sponsor and, on a
safety-related system, to the accountable engineer. And supplying detection rates or
introduced-defect counts from plausibility: a model asked for these returns confident,
well-formatted numbers with no provenance which then drive a real budget. Where the history does not
exist, the correct output is that it does not exist, plus the range over which the conclusion holds.

**Verification, concretely.** Reproduce the whole `CoQ` table by hand or in a spreadsheet whose
formulae are visible (four multiplications and a sum per regime), and state every input's source
beside it. Publish the marginal step analysis and the range of optimality, not the point minimum,
because those tell a board whether the recommendation is robust or fragile. And where an AI system
has classified historical costs into the four categories, sample-check the classification on a
stated sample size: category misclassification makes the model wrong in a direction nobody notices.

### Key terms — KA 9.1

| Term | Meaning |
|---|---|
| **Quality** | The degree to which delivered work conforms to its stated requirements. |
| **Grade** | The level of capability or specification the requirements ask for; independent of quality. |
| **Fitness for purpose** | Whether conforming, correctly graded output serves the use it was built for. |
| **Cost of quality (`CoQ`)** | Prevention + appraisal + internal failure + external failure. |
| **Cost of conformance** | Prevention + appraisal: money spent so defects do not occur or do not escape. |
| **Cost of nonconformance** | Internal + external failure: money lost because they did. |
| **Escape** | A defect reaching the next stage, or the customer, undetected. |
| **External-failure unit cost** | The average cost of one escaped defect in the operating context; set by context, not the project. |
| **Range of optimality** | The band of an uncertain input over which the recommended option stays recommended. |
| **Tolerance** | The range within which output is conforming; a specification without one is untestable. |

### Sample MCQs — KA 9.1

**MCQ 9.1-A `[9.1.2 · Application]`** A regime allows 80 defects to be introduced and detects 90 %
before handover; internal correction averages 1,500 and an escaped defect costs 12,000. Prevention is
96,000 and appraisal 64,000. Total cost of quality is:
- A. USD 268,000
- B. USD 364,000 ✅
- C. USD 204,000
- D. USD 460,000

*Rationale:* `96,000 + 64,000 + (72 × 1,500) + (8 × 12,000) = 364,000` (9.1.2). A omits external
failure; C counts nonconformance only and drops conformance; D applies the 12,000 external unit cost
to all 80 introduced defects rather than to the 8 that escape.

**MCQ 9.1-B `[9.1.2 · Evaluation]`** Moving to the next-stricter regime costs 90,000 more in
conformance and removes 9,000 of internal failure and 4 escaped defects. At what external-failure
unit cost does the stricter regime become worth buying?
- A. USD 12,000
- B. USD 22,500
- C. USD 20,250 ✅
- D. USD 81,000

*Rationale:* The step pays when `90,000 ≤ 9,000 + 4u`, so `u ≥ 81,000/4 = 20,250` (9.1.2). B divides
the conformance cost by escapes avoided and forgets the internal-failure saving; D is the numerator,
not the unit cost; A is the assumed unit cost, which is the thing being tested.

**MCQ 9.1-C `[9.1.2 · Analysis]`** An organisation strengthens testing and its internal failure cost
rises in the first period while total cost of quality falls. The correct reading is that:
- A. the testing programme is failing and should be reversed
- B. defects have moved from the external column to the internal one at a much lower unit cost ✅
- C. internal and external failure have been misclassified
- D. prevention spending was set too low

*Rationale:* Internal failure cost is not monotone in quality, better detection finds defects that
were previously escaping (9.1.2). Reading the rise as deterioration is the standard reason such
programmes are cancelled in their first period.

**MCQ 9.1-D `[9.1.1 · Comprehension]`** A supplier delivers a product built exactly to a deliberately
basic specification. It is best described as:
- A. low grade and low quality
- B. low grade and high quality ✅
- C. high grade and low quality
- D. not assessable until the client uses it

*Rationale:* Grade is the specified level; quality is conformance to it (9.1.1). D confuses quality
with fitness for purpose, which is a third and separate test.

**MCQ 9.1-E `[9.1.2 · Evaluation]`** Removing the last 6 escaped defects costs 129,000 more in total
cost of quality, and each escape would have cost 12,000. The strongest professional statement is
that:
- A. the spend is justified because defects should be eliminated
- B. the spend buys protection at 21,500 per defect, 1.79 times the harm it prevents, so it is not
  justified on these figures ✅
- C. the spend is justified because 129,000 is small against a 4,000,000 budget
- D. the comparison cannot be made without a Monte Carlo simulation

*Rationale:* `129,000/6 = 21,500` against a 12,000 consequence, a ratio of 1.79 (9.1.2). A is the
zero-defects fallacy; C is affordability, not value; D over-reaches. The expected-value comparison
is valid on stated averages, though a safety case would also require the tail to be examined.

### Self-check — KA 9.1

1. *Why is the cost-of-quality minimum not at zero defects?* — Because the marginal conformance cost
   of removing the last defects exceeds their consequence; on Auriga the last six escapes cost
   21,500 each to remove against 12,000 of harm.
2. *What single input determines where the optimum sits, and who sets it?* — The external-failure
   unit cost, set by the operating context and not by the project; Auriga's optimum holds for
   3,540–20,250 and moves at 20,250 and again at 60,000.
3. *Which cost category is non-monotone, and why does it matter?* — Internal failure: it rises as
   detection improves before falling as introduction falls, and misreading that rise is why quality
   programmes get cancelled in their first period.

---

## Knowledge Area 9.2 — Assurance and control

*Topics: 9.2.1 assurance and control as different instruments · 9.2.2 containment layers and the
economics of detection · 9.2.3 rework, capacity and the schedule consequence.*

### 9.2.1 Assurance and control as different instruments

*Quality assurance* acts on the **process** and is preventive: it asks whether the way the work is
being done is capable of producing conforming output, and its evidence is process evidence,
competence records, controlled documents, a managed configuration, a followed method. *Quality
control* acts on the **product** and is detective: it asks whether this output conforms, against its
criteria. Both are the first line of the assurance model built in Domain 3, KA 3.3.2; the second and
third lines form opinions about whether the first line works, and Domain 3's failure modes
(duplication, gap, capture, and treating a favourable opinion as a transfer of accountability),
apply here unchanged and are not restated.

The distinction is operationally load-bearing because control cannot make output conform; it can
only report that it does not. Every unit of control spend therefore buys **information**, and
information is worth what the decision it changes is worth. An inspection whose result cannot change
anything (carried out after the only feasible correction window has closed, or reported to someone
with no authority to act) is pure cost, and the commonest wasted quality spend in delivery is not
too little inspection but inspection positioned too late. Domain 3, KA 3.3.1 computes the same
result for a governance gate and establishes that gate value is destroyed by elapsed time and by
weak detection; the containment arithmetic below is its product-level counterpart, and the two
should be read together.

### 9.2.2 Containment layers and the economics of detection

**Definition.** A **containment layer** is a review, test or inspection with a stated coverage, a
stated **detection rate** (the share of the defects reaching it that it finds) and a stated **unit
correction cost**. A delivery process is a chain of such layers, and the property that matters is the
**escape fraction**: the product of the layers' individual escape rates.

```
escape fraction = ∏ (1 − dᵢ)      over layers i, with dᵢ the detection rate of layer i
escapes         = defects introduced × escape fraction
```

The product form is the lesson. Layers are multiplicative, not additive, so three unimpressive
layers can outperform one strong one; and, symmetrically, removing one layer does not degrade the
result by its own detection rate but multiplies the escape fraction by `1/(1 − dᵢ)`, which for a 50
% layer is a doubling.

**The correction-cost ladder.** Correction cost rises with the stage at which a defect is found,
because later stages have built more on top of it and involve more parties. Auriga's observed ladder
for a control-logic or configuration defect:

| Layer | What it is | Detection rate `dᵢ` | Correction cost per defect | Ratio to the first layer |
|---|---|---|---|---|
| L1 | Design peer review | 0.50 | USD 800 | ×1.000 |
| L2 | Factory acceptance test | 0.60 | USD 2,000 | ×2.500 |
| L3 | Site acceptance test | 0.50 | USD 3,500 | ×4.375 |
| — | Live operation (escape) | — | USD 12,000 | ×15.000 |

Domain 5, KA 5.2.1 establishes the same shape for *requirement* defects on a much steeper ladder (a
factor of four per stage, from USD 400 at definition to USD 102,400 in live service), because a
requirement defect propagates through design, build and test before it is visible. The two ladders
are not interchangeable and must not be averaged: a single "cost per defect" figure that mixes them
will mis-price every containment decision made from it.

**Worked example 9.2.2 — Auriga's containment chain, and the price of one avoidable defect.**

1. **Setup.** Under regime R2 (9.1.2), **80** control-logic defects are introduced. They pass through
   the three layers above, with detection rates 0.50, 0.60 and 0.50 and correction costs of 800,
   2,000 and 3,500. An escape costs USD 12,000.
2. **Formula.** Escape fraction = `∏(1 − dᵢ)`. Defects found at layer `i` = defects reaching it ×
   `dᵢ`. Internal failure cost = `Σ (found at i × unit cost at i)`. Expected containment cost per
   **introduced** defect = (internal + external failure cost) ÷ defects introduced.
3. **Substitution.** `(1 − 0.50)(1 − 0.60)(1 − 0.50) = 0.50 × 0.40 × 0.50`. Flow: L1 finds
   `80 × 0.50 = 40`, leaving 40; L2 finds `40 × 0.60 = 24`, leaving 16; L3 finds `16 × 0.50 = 8`,
   leaving **8**. Internal cost `40 × 800 + 24 × 2,000 + 8 × 3,500`.
4. **Result.** Escape fraction **0.100** (detection **90.0 %**); escapes **8**. Internal failure
   cost `32,000 + 48,000 + 28,000 =` **USD 108,000**: an average of **USD 1,500** per defect found
   internally, reconciling to 9.1.2. External failure cost `8 × 12,000 =` **USD 96,000**. Total
   nonconformance **USD 204,000**, which over 80 introduced defects is an **expected containment
   cost of USD 2,550 per introduced defect**.
5. **Interpretation.** That last figure is the most useful number in the domain and is almost never
   computed. **USD 2,550 is what one avoidable Auriga defect costs, whoever eventually finds it**:
   the weighted average of being caught cheaply at peer review, expensively at site acceptance and
   catastrophically in service. It converts every prevention proposal into a one-line test: *does
   this remove a defect for less than 2,550?* It also prices the layers. Removing L3 raises the
   escape fraction to `0.50 × 0.40 =` 0.200, escapes double from 8 to 16, adding `8 × 12,000 =
   96,000` of external failure while saving only `8 × 3,500 = 28,000` of L3 correction, so **L3 must
   cost more than USD 68,000 to run** before its removal is arguable. That is a checkable claim
   rather than an opinion about whether site testing is worth it. Two cautions. The model assumes
   layers are **independent detectors**, and they are not: two layers staffed by the same team
   reading the same document with the same blind spot miss the same defects, so the real escape
   fraction is worse than the product. The countermeasure is to make consecutive layers
   *methodologically different* (a review, then an executed test, then a test in the target
   environment) rather than more of the same. And detection rates are estimates; where no history
   exists, state a range and compute the decision across it.

**Worked example 9.2.2b — prevention or appraisal, on the same USD 40,000.**

1. **Setup.** Auriga has USD 40,000 of uncommitted quality budget at the end of design and two
   proposals. **(a) Appraisal:** extend site acceptance test coverage, raising L3's detection rate
   from 0.50 to 0.75. **(b) Prevention:** a design-standards and supplier-qualification package
   cutting defects introduced from 80 to 60, detection rates unchanged. The baseline is the R2 chain:
   nonconformance USD 204,000.
2. **Formula.** Re-run the chain under each proposal; compare (added spend + internal + external
   failure) against the baseline 204,000.
3. **Substitution.** (a) 80 introduced; L1 finds 40, L2 finds 24, L3 finds `16 × 0.75 = 12`, escapes
   **4**; internal `32,000 + 48,000 + 12 × 3,500 = 122,000`; external `4 × 12,000 = 48,000`.
   (b) 60 introduced; L1 finds 30, L2 finds 18, L3 finds 6, escapes **6**; internal
   `24,000 + 36,000 + 21,000 = 81,000`; external `6 × 12,000 = 72,000`.
4. **Result.** **(a)** `40,000 + 122,000 + 48,000 =` **USD 210,000** — **USD 6,000 worse** than doing
   nothing. **(b)** `40,000 + 81,000 + 72,000 =` **USD 193,000** — **USD 11,000 better** than doing
   nothing, and **USD 17,000 better than (a)** on identical money.
5. **Interpretation.** Prevention wins **even though it removes fewer escapes**: (a) halves escapes
   from 8 to 4, (b) only cuts them from 8 to 6. The reason is the containment cost per introduced
   defect. Option (b) removes 20 defects before any layer has to pay to find them, worth `20 × 2,550
   =` **USD 51,000** against USD 40,000 of spend, a net **USD 11,000**, exactly the result above and
   the identity worth remembering. Option (a) removes no defect; it **relocates** detection to a
   later and dearer layer, so its saving on external failure is partly consumed by internal failure
   rising from 108,000 to 122,000. Both rules are computable: **prevention pays whenever it removes
   a defect for less than the expected containment cost per introduced defect** (USD 2,550 here, so
   a 40,000 programme must avoid at least `40,000/2,550 =` **15.69, i.e. 16 defects**), and
   **appraisal pays only when the layer it strengthens is cheaper than the layer or escape it
   displaces work from.** Raising detection at the *earliest* layer would have been a different and
   better proposition.

   Two observations belong in the record. Option (b) takes total `CoQ` to `160,000 + 40,000 + 81,000
   + 72,000 =` **USD 353,000**, below the USD 364,000 that 9.1.2 called optimal. That is not a
   contradiction: 9.1.2 optimised along a ladder of **pre-costed regimes**, and prevention more
   efficient than the ladder assumed shifts the whole curve down, so a cost-of-quality optimum is
   conditional on the prevention technology available and should be recomputed when that changes.
   And option (a) reaches 4 escapes for a total `CoQ` of **USD 370,000** where regime R3 reaches 4
   escapes at **USD 397,000**: two routes to the same escape count, USD 27,000 apart, because R3 got
   there partly by introducing fewer defects. **The escape count is not a sufficient description of
   a quality regime.**

### 9.2.3 Rework, capacity and the schedule consequence

**The principle no cost-of-quality model contains.** Rework is not only money; it is **capacity**.
Correcting a defect consumes the same people, equipment and test facility that would otherwise
produce new output, so a rework share of capacity is a direct reduction in throughput; and where the
activity is on the critical path, the reduction converts straight into elapsed time. The
relationship is not linear:

```
effective capacity  = nominal capacity × (1 − r)      r = rework share of capacity
duration            = first-pass work content ÷ effective capacity
duration multiplier = 1 / (1 − r)
```

`1/(1 − r)` is hyperbolic. The tenth percentage point of rework costs far less than the fortieth,
which is why a team absorbs a modest rate almost invisibly and then loses control of a phase over
what looks like a small further deterioration.

**Worked example 9.2.3 — Auriga's commissioning, and what 30 % rework costs.**

1. **Setup.** Auriga's activity **F, testing and commissioning**, is planned at **4 weeks** with
   **6** engineers, a nominal capacity of **24 engineer-weeks**. First-pass work content is **20
   engineer-weeks**, so the plan carries a rework allowance of **4 engineer-weeks**, or **16.67 %**
   of capacity. F is on the critical path with zero float (Domain 6, KA 6.2.1); Auriga's cost of
   delay is **USD 45,000 per week**; an engineer-week costs **USD 5,225** (Domain 7, KA 7.4.1). At
   the two-week point, rework is running at **30 %** of capacity.
2. **Formula.** duration = content ÷ (crew × (1 − `r`)); overrun = duration − planned duration; delay
   cost = overrun × cost of delay; rework consumed = `r` × crew × duration; labour above allowance =
   (rework consumed − allowance) × engineer-week cost.
3. **Substitution.** `20 ÷ (6 × 0.70) = 20 ÷ 4.2`. Rework consumed `0.30 × 6 × 4.7619`.
4. **Result.**

   | Rework share `r` | Multiplier `1/(1−r)` | Duration (weeks) | Overrun (weeks) | Delay cost (USD) |
   |---|---|---|---|---|
   | 16.67 % (as planned) | 1.2000 | 4.0000 | 0.0000 | 0 |
   | 25 % | 1.3333 | 4.4444 | 0.4444 | 20,000 |
   | **30 % (actual)** | **1.4286** | **4.7619** | **0.7619** | **34,286** |
   | 40 % | 1.6667 | 5.5556 | 1.5556 | 70,000 |
   | 50 % | 2.0000 | 6.6667 | 2.6667 | 120,000 |

   At 30 %: duration **4.7619 weeks**, overrun **0.7619 weeks**, delay cost **USD 34,286**. Rework
   consumes **8.5714 engineer-weeks** against a 4-engineer-week allowance — **4.5714 engineer-weeks**
   of unbudgeted labour at 5,225, or **USD 23,886**. Total consequence: **USD 58,171**.
5. **Interpretation.** **The cost is mostly schedule, and the schedule cost is invisible in the
   quality report.** Of the 58,171, **59 %** is delay and 41 % labour; a quality function reporting
   rework hours reports the smaller half. On a critical-path activity the rework share of capacity is
   a schedule metric that happens to be collected by the quality system, and it belongs in the same
   report as float.

   **The convexity is the management lesson.** Moving from 30 % to 40 % costs a further **USD
   35,714** of delay and 40 % to 50 % costs **USD 50,000**, where 10 % to 20 % would have cost only
   **USD 20,833**. Every additional point is dearer than the last, so intervene early (while the
   numbers still look tolerable), and trigger on the *rate*, not the accumulated slip.

   **The allowance is a decision and should be visible as one.** A 16.67 % allowance is legitimate
   and honest, and it is also a forecast that can be wrong; a plan with **no** allowance is not a
   plan without rework but a plan whose first defect creates a slip. State the allowance, measure
   against it weekly, and treat the variance as a leading indicator. The rework share reveals itself
   two weeks before the milestone does.

   **The easy-to-miss point:** the natural response, reduce test coverage to recover the schedule,
   is a decision to raise the escape fraction, and 9.2.2 prices it. Cutting L3's detection from 0.50
   to 0.25 raises escapes from 8 to 12, adding `4 × 12,000 =` **USD 48,000** of external failure
   against `4 × 3,500 =` USD 14,000 of internal correction saved. Any schedule recovery must beat
   that net USD 34,000, computed, before it is taken.

### AI in this KA

**Where it earns its place.** Detecting defects at scale: static analysis of control logic and code,
cross-checking a configuration set against a design baseline, comparing an as-installed instrument
schedule against the design register, sweeping test evidence for missing or contradictory results.
These have checkable right answers, and a model doing them is functioning as a **containment
layer**, and should be characterised as one, with a measured detection rate and a unit cost, so it
can be placed in the chain of 9.2.2 rather than trusted qualitatively. Also: classifying a rework
log by cause and computing the rework share of capacity per team and per week, which most
organisations cannot report because the time data is not coded for it.

**Where it must not go.** It must not be the **only** layer for anything consequential, because
9.2.2's independence caution applies with unusual force: a model's misses are systematic, so two
checks by the same model are one check, and a model that generated the artefact is not an
independent reviewer of it. It must not issue a test result, sign an inspection record or grant a
release. Those are attributable acts under Domain 3, KA 3.A.2. And its detection rate must not be
*asserted*: a claim that an automated check finds "most" defects is not a number, and a chain
computed from asserted rates gives a false escape fraction that is then used to justify removing a
human layer.

**Verification, concretely.** Measure the model's detection rate against a known, held-back defect
set before relying on it, and re-measure when the model, the prompt or the artefact type changes: a
detection rate is a property of a *configuration*, not of a tool. Record its false-positive rate
too, because a layer raising many false findings consumes the engineering attention the next layer
needs. Keep a human layer **methodologically different** from the automated one. And reproduce the
arithmetic by hand: the escape fraction is a product of three numbers and the duration multiplier is
one division.

### Key terms — KA 9.2

| Term | Meaning |
|---|---|
| **Quality assurance** | Process-directed, preventive activity: is the way we work capable of producing conforming output? |
| **Quality control** | Product-directed, detective activity: does this output conform? |
| **Containment layer** | A review, test or inspection with a stated coverage, detection rate and unit correction cost. |
| **Detection rate (`dᵢ`)** | The share of defects reaching a layer that the layer finds. |
| **Escape fraction** | `∏(1 − dᵢ)` across the chain, the share of introduced defects reaching the customer. |
| **Correction-cost ladder** | The rising cost of correcting a defect by the layer at which it is found. |
| **Expected containment cost per introduced defect** | (Internal + external failure) ÷ defects introduced; the breakeven price of prevention. |
| **Rework share of capacity (`r`)** | The fraction of a team's available effort consumed correcting output. |
| **Duration multiplier** | `1/(1 − r)`, the hyperbolic effect of rework on elapsed time. |
| **Rework allowance** | The capacity a plan reserves for rework; a forecast, and a decision. |

### Sample MCQs — KA 9.2

**MCQ 9.2-A `[9.2.2 · Application]`** Three containment layers have detection rates 0.50, 0.60 and
0.50. Of 80 introduced defects, how many escape?
- A. 8 ✅
- B. 3
- C. 12
- D. 27

*Rationale:* Escape fraction `0.50 × 0.40 × 0.50 = 0.10`, so `80 × 0.10 = 8` (9.2.2). B adds the
detection rates to 1.60 and treats the chain as detecting everything with a remainder; C uses two
layers only; D applies the *average* detection rate (0.5333) three times as if each layer saw all 80.

**MCQ 9.2-B `[9.2.2 · Analysis]`** On a chain where 80 defects are introduced, internal failure costs
108,000 and external failure 96,000. The expected containment cost per introduced defect is:
- A. USD 1,500
- B. USD 2,550 ✅
- C. USD 12,000
- D. USD 2,000

*Rationale:* `(108,000 + 96,000)/80 = 2,550` (9.2.2). A is the average cost per defect found
*internally*, omitting escapes; C is the escape unit cost; D divides internal failure alone by 80
and rounds: all three understate the breakeven price of prevention.

**MCQ 9.2-C `[9.2.2 · Evaluation]`** USD 40,000 will either raise the last layer's detection rate
from 0.50 to 0.75, or cut defects introduced from 80 to 60. On the figures of 9.2.2, the better choice
and its reason are:
- A. raising detection, because it halves escapes from 8 to 4
- B. cutting introduction, because it removes 20 defects at an expected containment cost of 2,550
  each — USD 51,000 of value for USD 40,000 ✅
- C. either, since both cost the same
- D. raising detection, because internal correction is cheaper than external failure

*Rationale:* Prevention returns 51,000 against 40,000 (a net 11,000), and beats the appraisal option
by 17,000 (9.2.2b). A counts escapes rather than cost and misses that raising late-layer detection
moves work to a dearer layer, pushing internal failure from 108,000 to 122,000; D states a true
premise that does not reach the conclusion.

**MCQ 9.2-D `[9.2.3 · Application]`** A 4-week activity with 6 engineers has 20 engineer-weeks of
first-pass content. Rework runs at 30 % of capacity. The activity will take:
- A. 4.00 weeks
- B. 4.76 weeks ✅
- C. 5.20 weeks
- D. 4.29 weeks

*Rationale:* `20 ÷ (6 × 0.70) = 4.7619` weeks (9.2.3). C adds 30 % to the 4-week plan, the linear
error; D mishandles the allowance by scaling the 3.33 weeks of pure content; A ignores the overrun.

**MCQ 9.2-E `[9.2.3 · Analysis]`** Why does the marginal cost of rework rise as the rework share
rises?
- A. because rework is charged at a premium rate
- B. because the duration multiplier `1/(1 − r)` is convex, so equal increments of `r` add increasing
  amounts of elapsed time ✅
- C. because defects found later cost more to fix
- D. because float is consumed first and then lost

*Rationale:* The convexity is the mechanism: on Auriga's figures 10 %→20 % adds 0.4630 weeks while
40 %→50 % adds 1.1111 (9.2.3). C is the correction-cost ladder, a different effect (9.2.2); A and D
are not general.

**MCQ 9.2-F `[9.2.2 · Analysis]`** An automated check and a manual review are placed as consecutive
containment layers, both driven from the same design document by the same team. The escape fraction
computed as the product of their detection rates will be:
- A. correct
- B. too optimistic, because the layers are not independent detectors ✅
- C. too pessimistic, because two layers always find more than one
- D. correct only if their detection rates are equal

*Rationale:* The product form assumes independence; layers sharing a source, a method or a blind spot
miss the same defects, so the true escape fraction is worse than the product (9.2.2). The
countermeasure is to make consecutive layers methodologically different.

### Self-check — KA 9.2

1. *What does removing a 50 % containment layer do to the escape fraction?* — Doubles it: the layer's
   escape rate of 0.50 leaves the product, multiplying escapes by `1/(1 − 0.50)`.
2. *State the breakeven test for a prevention proposal.* — It pays if it removes a defect for less
   than the expected containment cost per introduced defect: USD 2,550 on Auriga, so a USD 40,000
   programme must avoid 16 defects.
3. *Why is a rework share of capacity a schedule metric?* — Because effective capacity is
   `nominal × (1 − r)` and duration is content ÷ effective capacity, so on a zero-float activity every
   point of rework is elapsed time: 30 % turned Auriga's 4-week commissioning into 4.7619 weeks and
   USD 34,286 of delay.

---

## Knowledge Area 9.3 — Acceptance, nonconformance and root-cause analysis

*Topics: 9.3.1 the acceptance decision and nonconformance disposition · 9.3.2 acceptance sampling and
what a passing sample establishes · 9.3.3 root-cause analysis and recurrence.*

### 9.3.1 The acceptance decision and nonconformance disposition

**Definition.** **Acceptance** is a formal decision by a named authority that a deliverable meets its
acceptance criteria and may pass to the next stage, to the client, or into service. Domain 5,
KA 5.4.3 established the criteria and the verification/validation distinction; what belongs here is
the *decision*, its authority, and what happens when the answer is no.

Acceptance goes wrong in four recognisable ways, each with a countermeasure that costs nothing.
**Acceptance by silence.** Nobody rejects, so the work is deemed accepted, usually by the expiry of
a contractual review period; the countermeasure is a positive acceptance record with a named
signatory, since the absence of a rejection is not a decision. **Acceptance by exhaustion.** The
deliverable is accepted at the fourth submission because the reviewers have no more time, not
because it changed; the countermeasure is to record the open nonconformances at acceptance,
converting exhaustion into a visible conditional pass. **Acceptance without authority.** The
signatory is not entitled to bind the receiving organisation, which is Domain 3's single-A defect at
deliverable level. And **acceptance against criteria written after the evidence**, where the
criteria were adjusted to what the deliverable does, so the test is guaranteed to pass and evidences
nothing.

**Nonconformance disposition.** When output does not conform, exactly five dispositions exist, each
with a different authority and a different downstream obligation:

| Disposition | What it means | Authority required | Downstream obligation |
|---|---|---|---|
| **Rework** | Bring it into full conformance | Delivery management | Retest to the same criteria |
| **Repair** | Make it fit for use without full conformance | Design authority | The as-built record must show the repair |
| **Use as is (concession)** | Accept the nonconformance as it stands | The party bearing the consequence (client or design authority, never the producer alone) | Register it; assess cumulative effect |
| **Regrade** | Use it for a lesser purpose for which it conforms | Design authority plus the owner of that purpose | Traceability so it cannot migrate back |
| **Reject / scrap** | Do not use it | Delivery management | Root-cause analysis if value or recurrence warrants |

Two disciplines make this table work rather than decorate a procedure. **The producer never grants
its own concession**: a concession transfers a consequence, so its authority sits with whoever will
carry it, and a process in which the delivery team accepts its own nonconformances has no acceptance
function at all. And **concessions must be counted, not merely recorded**: individually minor
concessions accumulate into a delivered product materially different from the specified one, which
is Domain 3's Case study B at deliverable level. The cumulative-effect test (related concessions
aggregating above a threshold within a period require the authority appropriate to the aggregate) is
the countermeasure, and it is the instrument Domain 4, KA 4.4 applies to change.

**The authority column stops at the design authority, and sometimes the decision does not.** In
sectors operating under an approval regime (where a certifying body, a notified assessor or a
regulator has a standing in the item's fitness for use, as in aerospace, rail signalling, medical
devices, pressure systems and nuclear) a repair, a concession or a regrade may engage that body as
well, potentially **before the item is used**. The professional position is therefore that such a
disposition is **treated as unavailable until the external position has been established**, and that
establishing it sits *outside* the project's authority: no internal signature substitutes for it,
and it is not something a commercial pressure can be allowed to shorten. Whether any external
notification or acceptance is engaged, by whom, for what and in what form differs by sector and by
jurisdiction, and is established with the organisation's regulatory or technical-compliance function
and with the body itself. **Nothing here states the position anywhere, and nothing here
characterises any disposition as acceptable, or unacceptable, to any body.** The obligation this
domain does impose is that the disposition record names, for each disposition class, **whether an
external acceptance is engaged and who established that**: recorded before the disposition is
granted, not discovered at inspection.

**Making the cumulative-effect test operable.** As stated, the test has three undefined parameters
and no owner, which is why it is widely written into procedures and nowhere enforced. Five decisions
make it real, and all five belong in the quality management plan of 9.1.3 rather than in the
judgement of whoever is granting the concession:

- **Who sets the threshold, and when.** The **sponsor or design authority sets it at baseline**: the
  party that carries the consequence of an altered product, never the delivery team that would trip
  it. Setting it late, or setting it by whoever is asked first, produces a threshold shaped by the
  concessions already granted.
- **Where the threshold comes from.** It is derived, not chosen round: the same window logic Domain
  3, KA 3.3.4c derives for a delegation's cumulative rule applies unchanged. The threshold sits
  comfortably **above** the base-rate aggregate of ordinary concession traffic in that class per
  period, and comfortably **below** the aggregate at which the delivered product would differ
  materially from the specified one. A threshold below the base rate re-centralises every
  disposition and will be abandoned within two months; a threshold above the material-difference
  point is decorative.
- **The aggregation period.** Stated explicitly (a quarter, a release, or the deliverable's whole
  production run), and chosen so that a slow accumulation cannot escape by straddling two periods.
  Where a deliverable is produced once, the period is the deliverable.
- **The test for relatedness.** By **what the concessions touch** (the same deliverable, the same
  interface, the same assured control, the same requirement) never by who requested them or which
  budget carried them, because the exposure the rule exists to catch is a coherent change to one
  thing arriving in instalments. Widening the class is the natural error and it multiplies false
  trips.
- **The named owner of the running total.** One person maintains it (normally the quality function),
  and reports it monthly against the threshold whether or not anything moved, because a total
  reported only when it is interesting cannot be distinguished from a total nobody kept.

**And what a breach triggers, stated in advance.** On breach, further concessions **in that class**
are suspended pending a decision by the authority appropriate to the aggregate, taken through Domain
3's escalation machinery at its stated latency rather than by informal referral. The decision before
that authority is not "may this one be granted?" but the harder and correct question: *given what
has already been conceded, is the delivered product still the specified one, and if not, what is now
being accepted?* Suspension is the part that gives the rule teeth, and it is also the part most
often omitted: a cumulative test with no consequence on breach is a counter, and a counter is not a
control.

**The record, and how long it lives.** Acceptance records, concession authorisations, disposition
decisions and the verification evidence behind them are the primary evidence in any later dispute,
audit, defect claim or inspection; and all of those arrive years after the signature, when the
people are gone and the memory is not admissible. So each is assigned a **record class**, with a
**named custodian role**, a **retention period** and a stated **disposal authority**, on the custody
machinery of Domain 3, KA 3.3.4. The period is set by the applicable regime and by contract rather
than by the project's convenience, it differs by jurisdiction and by sector, and it is taken from
the organisation's records, regulatory and legal functions — **nothing in this domain states a legal
minimum or maximum, and nothing here should be relied on as stating one.** Domain 16, KA 16.4.4
works the economics and reaches a conclusion worth carrying here: retaining contract and technical
evidence pays as insurance at a very low probability of ever needing it, so the reflex to purge for
storage cost is almost always wrong, and retention is **a schedule by class, never a single rule**.
Custody transfers to a named continuing role at closure, because the quality function that signed
will not exist in the organisation that is asked.

### 9.3.2 Acceptance sampling and what a passing sample establishes

Verifying every item is often uneconomic, so acceptance is taken on a sample. The arithmetic is
elementary, almost never done, and produces two results practitioners find genuinely surprising.

For a population with true defective fraction `p`, the probability that a sample of `n` contains **no**
defective item is `(1 − p)ⁿ`. Turning that round gives the **confidence bound**: if a sample of `n` is
clean, the largest defective fraction consistent with that observation at confidence `1 − α` is

```
p_upper = 1 − α^(1/n)          (α = 0.05 for a 95 % upper bound)
```

And the economics of how much to verify reduce to a single ratio. With `c` the cost of verifying one
item and `u` the cost of one escaped defective item, full verification of `N` items costs `cN` while
verifying `n` and accepting the rest costs `cn + u·p·(N − n)`. Setting them equal, the `(N − n)` terms
cancel and

```
p* = c / u
```

**Full verification pays exactly when the defective fraction exceeds the ratio of verification cost
to failure cost, and that breakeven is independent of the sample size.** It is the number to put on
the table before anyone argues about sample sizes.

**Worked example 9.3.2 — Auriga's instrument loops: what a clean sample of 20 proves.**

1. **Setup.** Auriga's installation includes **120** instrument loops. Verifying one costs
   **USD 1,400** (a two-engineer point-to-point check and signal injection). A defective loop reaching
   service costs **USD 12,000**. The subcontractor proposes verifying **20** loops and accepting the
   batch if none is defective. The sample is drawn and is clean. What has been established?
2. **Formula.** `P(clean sample) = (1 − p)ⁿ`; 95 % bound `p_upper = 1 − 0.05^(1/n)`; breakeven
   `p* = c/u`; minimum self-consistent sample `n* = ln(0.05)/ln(1 − p*)`.
3. **Substitution.** `(1 − 0.05)²⁰`; `1 − 0.05^(1/20)`; `1,400/12,000`; `ln(0.05)/ln(1 − 0.116667)`.
4. **Result.** A population 5 % defective passes this plan **35.85 %** of the time (at 2 %
   defective, **66.76 %**; at 10 %, **12.16 %**). The 95 % upper bound after a clean sample of 20 is
   **13.91 %**: on 120 loops, up to **16.69**, i.e. as many as 16 defective loops. The breakeven
   defective fraction is `1,400/12,000 =` **11.67 %**. The smallest sample whose 95 % bound falls
   below that breakeven is **25** (the bound is 11.29 % at n = 25 and 11.73 % at n = 24), costing
   **USD 7,000** more than the proposed 20.
5. **Interpretation.** The decisive observation compares two of those numbers. **The plan's own
   confidence bound (13.91 %) is higher than the defective fraction at which its strategy is wrong
   (11.67 %).** A clean sample of 20 cannot exclude the very defect rate at which the correct
   decision would have been to verify all 120 loops. The plan is **not self-consistent**, and no
   amount of it passing changes that. Five more loops (USD 7,000 on a USD 4,000,000 project) repairs
   it, and that is the cheapest quality decision anywhere in this domain.

   What the sample *does* establish is worth stating precisely. It establishes that the population
   is probably not grossly defective: rates above about 14 % are unlikely given a clean 20. It does
   **not** establish conformance, and it does not establish any tighter rate, bounding `p` at 5 %
   with 95 % confidence needs **59** items (`ln 0.05 / ln 0.95 = 58.404`, and the familiar
   rule-of-three approximation `3/p` gives 60 as a mental check).

   The economics cut against instinct. At a true 5 % defective rate the sample-of-20 plan is the
   **cheapest** of the three options: `1,400 × 20 + 12,000 × 0.05 × 100 =` **USD 88,000**, against
   **USD 119,200** for a 59-loop plan and **USD 168,000** for verifying all 120. Sampling is not
   laziness and full inspection is not automatically responsible, below `p* = 11.67 %` it destroys
   value. But the plan must be able to *see* whether `p` is below the breakeven, and the 20-loop
   plan cannot. Hence the discipline: **choose `n` from the confidence bound the economics require,
   not from what feels proportionate.** Three cautions. The `(1 − p)ⁿ` form treats draws as
   independent, a good approximation when `n` is small relative to `N` and conservative here.
   Defects are often **clustered** (one mis-trained crew, one bad batch), so the sample must be
   random, and a convenience sample of the accessible loops establishes nothing. And zero-defect
   acceptance interacts badly with measurement error, so the disposition rules of 9.3.1 must cover a
   contested sample result before the sampling starts.

### 9.3.3 Root-cause analysis and recurrence

**The purpose, stated to exclude what usually happens.** Root-cause analysis exists to reach the
level at which a cause can be **removed**, so the defect class does not recur. It is not an
explanation, not an apology and not a search for a person. The standard techniques (iterative "why"
questioning, cause-and-effect decomposition, fault-tree reasoning backwards from an undesired event,
and change analysis comparing a working case with a failing one) share one failure mode: **stopping
at the level at which somebody can be blamed**, which is always shallower than the level at which
something can be fixed. "The engineer loaded the wrong parameter set" describes the event. "There
was no controlled baseline against which a parameter set could be checked, and no process step that
would have detected the mismatch" is a cause, because it names something a manager can change.

**The three tests of a completed analysis.** A root cause has been reached when removing it would have
prevented this occurrence; removing it prevents the whole **class**, not just the instance; and the
removal is within somebody's authority to enact. If the third test fails, the analysis has produced a
finding for a higher level, and Domain 3's escalation machinery is how it travels.

**Recurrence is the metric.** The **recurrence rate** (repeat nonconformances as a share of all)
measures whether the programme works; a stable or rising rate means causes are being described
rather than removed. **Cause concentration** (the share attributable to the largest single cause)
tells you where to spend, and is almost always higher than intuition suggests.

**Worked example 9.3.3 — Auriga's dominant cause, priced.**

1. **Setup.** Auriga's 8 escaped control-logic defects (9.2.2) trace to three root causes: **5** to
   an uncontrolled supplier firmware configuration baseline (two field devices shipped with a
   superseded parameter set and nothing in the process compared them to a controlled reference),
   **2** to an ambiguous interlock specification, **1** to a one-off wiring error. Removing the
   dominant cause (imposing configuration control on the supplier's firmware, re-baselining, and
   adding a baseline comparison to the factory acceptance test) costs **USD 18,000**. Over the
   remaining installation programme and the 12-month warranty period the same cause is expected to
   produce **5** further occurrences at **USD 12,000** each.
2. **Formula.** Cause concentration = defects from the largest cause ÷ total. Net value of removal =
   expected recurrences × unit consequence − removal cost. Breakeven recurrences = removal cost ÷ unit
   consequence.
3. **Substitution.** `5/8`; `5 × 12,000 − 18,000`; `18,000/12,000`.
4. **Result.** Cause concentration **62.5 %**. Expected avoided cost **USD 60,000** against a
   removal cost of **USD 18,000**: a net **USD 42,000** and a payback ratio of **3.33 times**.
   Removal breaks even at **1.5**, so **2** recurrences.
5. **Interpretation.** The payback ratio is not the interesting number; the **breakeven of 1.5
   recurrences** is, because it changes what the analysis must prove. Nobody needs to defend a forecast
   of five recurrences; the investment is justified if the cause recurs **twice**, which for a cause
   that has already recurred five times is an observation rather than a forecast. That is the general
   shape of a root-cause business case, and it is why such cases are easier to make than they are
   made: arguing about the expected recurrence count is arguing about the wrong number.

   Three professional points. **Cause concentration is the budget allocator**: with 62.5 % on one
   cause, a programme treating all three equally spends most of its effort on 37.5 % of the problem,
   and this concentration is the normal finding once causes are classified consistently rather than
   described individually. **The one-off must be tested, not assumed**: a single occurrence is what
   the fifth firmware defect also looked like at the time, so the honest test is whether a mechanism
   exists that would catch a recurrence. And **the removal must be verified as effective, on a
   date**: an action closed in a corrective-action log is not a cause removed, and the evidence is
   the absence of recurrence over a stated observation window, which is why the recurrence rate is
   reported and not merely the closure rate.

### AI in this KA

**Where it earns its place.** Clustering a nonconformance log by cause: the task producing the
concentration figure above, that humans do badly because recurrences are weeks apart and worded
differently every time, and that has a checkable answer. Extracting a structured acceptance and
concession register from correspondence and minutes, flagging deliverables accepted without a named
signatory or with open nonconformances unrecorded. Computing sampling plans and confidence bounds
across candidate `n` values, including the self-consistency test. Drafting the first pass of a
root-cause narrative from evidence, for a human to interrogate and own.

**Where it must not go.** It must not accept a deliverable, grant a concession or close a corrective
action: all three are attributable decisions with a consequence-bearing owner (9.3.1). It must not
*conclude* a root-cause analysis: a plausible causal narrative is exactly what a language model
produces most readily and what is least verifiable, and a fluent wrong cause is worse than no
analysis because it terminates the enquiry. And it must not estimate the recurrence count, the
defective fraction or the escaped-defect cost: the three inputs on which the calculation turns and
for which a model has no evidence.

**Verification, concretely.** For a clustered log, re-read a stated sample of records and confirm
the cause assignment, because the concentration figure that will drive the budget is only as good as
the classification. For any proposed cause, apply the three tests of 9.3.3 explicitly and in
writing, and reject a cause failing the second. Reproduce every sampling number by hand. The bound
is one power, the breakeven one division. And put the self-consistency comparison in the acceptance
paper, since it is the line that tells a reviewer whether the plan can support the decision it is
being used to make.

### Key terms — KA 9.3

| Term | Meaning |
|---|---|
| **Acceptance** | A formal decision by a named authority that a deliverable meets its criteria and may pass on. |
| **Acceptance by silence** | Deemed acceptance through the expiry of a review period; not a decision. |
| **Nonconformance** | Output that does not meet its specified requirement. |
| **Concession (use as is)** | Acceptance of a nonconformance as it stands, authorised by the party bearing the consequence. |
| **Regrade** | Use of nonconforming output for a lesser purpose for which it conforms, with traceability. |
| **Cumulative-effect test** | The rule that related concessions aggregating above a threshold require the authority appropriate to the aggregate. Operable only once five things are fixed in the quality plan: who sets the threshold (sponsor or design authority, at baseline), how it is derived, the aggregation period, the relatedness test, and the named owner of the running total. |
| **Relatedness class (concessions)** | The set over which the cumulative total is summed, defined by what the concessions touch, never by who requested them; widening it multiplies false trips. |
| **Suspension on breach** | The consequence that gives the cumulative test teeth: further concessions in that class stop pending a decision by the authority appropriate to the aggregate, taken through the escalation machinery. |
| **External acceptance** | Where an approval regime applies, a body outside the project may need to be notified of, or to accept, a repair, concession or regrade before use. It sits outside the project's authority and differs by sector and jurisdiction; the record names whether it is engaged and who established that. |
| **Record class, custodian, retention period** | The three fields that make "the evidence is retained" real: what class of record this is, the named role accountable for it existing, and how long it is held with the source of that period named. Periods come from the applicable regime and the contract, not from this book. |
| **Acceptance sampling** | Accepting or rejecting a population on the evidence of a sample. |
| **Confidence bound (`p_upper`)** | `1 − α^(1/n)`, the largest defective fraction consistent with a clean sample of `n`. |
| **Breakeven defective fraction (`p*`)** | `c/u`: verification cost ÷ escaped-defect cost; above it, full verification pays. Independent of `n`. |
| **Self-consistent sampling plan** | One whose confidence bound lies below its own breakeven defective fraction. |
| **Cause concentration** | The share of nonconformances attributable to the single largest root cause. |
| **Recurrence rate** | Repeat nonconformances as a share of all; the test of whether causes are being removed. |

### Sample MCQs — KA 9.3

**MCQ 9.3-A `[9.3.2 · Application]`** A sample of 20 items is drawn and none is defective. At 95 %
confidence, the largest defective fraction consistent with that result is closest to:
- A. 0 %
- B. 5 %
- C. 13.9 % ✅
- D. 2.5 %

*Rationale:* `1 − 0.05^(1/20) = 0.1391` (9.3.2). A treats a clean sample as proof of conformance; B is
the fraction a 59-item sample would bound; D halves the 5 % significance level.

**MCQ 9.3-B `[9.3.2 · Analysis]`** Verifying one item costs 1,400; an escaped defective item costs
12,000. Above what defective fraction does verifying the whole population beat sampling?
- A. 8.57 %
- B. 11.67 % ✅
- C. it depends on the sample size
- D. 1.4 %

*Rationale:* `p* = c/u = 1,400/12,000 = 11.67 %`, and the `(N − n)` terms cancel so the breakeven is
independent of `n` (9.3.2). A inverts the ratio; C is the intuition the algebra refutes.

**MCQ 9.3-C `[9.3.2 · Evaluation]`** A 20-item zero-defect acceptance plan has a 95 % bound of 13.91 %
and a breakeven defective fraction of 11.67 %. The most serious criticism of the plan is that:
- A. 20 items is too few to be statistically valid
- B. its confidence bound exceeds its own breakeven, so a clean sample cannot exclude the defect rate
  at which full verification would have been correct ✅
- C. it should use a one-defect acceptance number instead of zero
- D. sampling is inappropriate for safety-related work

*Rationale:* Self-consistency is the precise defect, repaired by raising `n` to 25 (9.3.2). A is an
unquantified assertion; C would weaken the plan further; D is a different argument these figures do
not establish.

**MCQ 9.3-D `[9.3.1 · Comprehension]`** A delivery team decides to accept its own nonconforming output
as fit for use without correction. The defect in that process is that:
- A. rework is always preferable to concession
- B. the authority for a concession belongs to the party that will bear the consequence, not the
  producer ✅
- C. concessions are never permissible
- D. the disposition should have been regrade

*Rationale:* A concession transfers a consequence, so its authority sits with whoever carries it
(9.3.1). A and C overstate, concession is a legitimate disposition; D presumes a lesser purpose
exists.

**MCQ 9.3-E `[9.3.3 · Application]`** Removing a root cause costs 18,000; each recurrence costs
12,000. The strongest way to present the case is that removal:
- A. has a payback ratio of 3.33 times on five expected recurrences
- B. breaks even at 1.5 recurrences, so needs the cause to recur only twice ✅
- C. saves 60,000
- D. reduces the cause concentration from 62.5 %

*Rationale:* The breakeven recurrence count is the robust argument because it does not depend on
forecasting the recurrence rate (9.3.3). A and C are true but rest on the five-recurrence estimate; D
describes a measure, not a value.

**MCQ 9.3-F `[9.3.3 · Analysis]`** Which statement identifies a root cause rather than an event
description?
- A. the engineer loaded the wrong parameter set
- B. the device was delivered with a superseded configuration
- C. there was no controlled baseline against which a parameter set could be checked, and no process
  step that would have detected the mismatch ✅
- D. the site acceptance test did not detect the fault

*Rationale:* Only C names something within a manager's authority to change whose removal prevents the
whole class (9.3.3). A stops where a person can be blamed; B and D describe the occurrence and a
layer's miss.

### Self-check — KA 9.3

1. *What does a clean sample of 20 establish?* — That the defective fraction is probably below
   13.91 % at 95 % confidence; neither conformance nor any tighter rate, and bounding `p` at 5 % would
   need 59 items.
2. *State the breakeven for full verification and what it depends on.* — `p* = c/u`, 11.67 % on
   Auriga, and it is independent of the sample size.
3. *Who may authorise a concession, and what must accompany it?* — The party bearing the consequence,
   never the producer alone; plus a register entry and a cumulative-effect test, since individually
   minor concessions aggregate into a different product.
4. *What five things must be fixed before a cumulative-effect test is a control rather than a
   sentence?* — Who sets the threshold and when (sponsor or design authority, at baseline); how it is
   derived (above the base-rate aggregate of the class, below the material-difference point); the
   aggregation period; the relatedness test, defined by what the concessions touch; and the named
   owner of the running total. Plus the consequence on breach: suspension of further concessions in
   that class pending the aggregate authority's decision (9.3.1).
5. *When does a disposition need something the project cannot give it?* — Where an approval regime
   applies, a repair, concession or regrade may need notification to or acceptance by a certifying
   body or regulator before use. That sits outside the project's authority, differs by sector and
   jurisdiction, and is established with the regulatory function and the body itself.
6. *What three fields turn "the evidence is retained" into a retention obligation?* — Record class,
   named custodian role, and retention period with the source of that period named: the source being
   the applicable regime and the contract, not the project's convenience. Nothing in this domain
   states a legal minimum (Domain 3, KA 3.3.4; Domain 16, KA 16.4.4).

---

## Knowledge Area 9.4 — Lessons learned, continuous improvement, data quality and AI-output quality

*Topics: 9.4.1 lessons that change behaviour · 9.4.2 first-time-right and the compounding of stage
yields · 9.4.3 data quality as a delivery constraint · 9.4.4 AI-output quality and the verification
that earns its cost.*

### 9.4.1 Lessons that change behaviour

Lessons-learned processes are near-universal and largely ineffective, for structural rather than
cultural reasons. A lesson is captured at the end of a project, by the people leaving it, into a
repository consulted by nobody, as a narrative rather than an instruction. Nothing in that chain
contains a mechanism by which a future decision changes.

**The four properties of a lesson that works**, each a test the entry passes or fails. It is
**specific to a decision**: it names the decision class it should alter, not a theme. It has an
**owner in the standing process**, so the change lands in a template, checklist, gate criterion,
estimating parameter or contract clause rather than in a document. It carries a **number** where one
exists — Auriga's expected containment cost per introduced defect (USD 2,550), its rework allowance
(16.67 %), the self-consistent sample size for a zero-defect plan — because a parameter propagates
and a paragraph does not. And it is **captured when it is learned**, not at closeout, since late
capture loses both fidelity and the chance to apply it on the same project.

**The improvement loop, and its only honest measure.** Continuous improvement is the cycle of
measuring a process, changing it and re-measuring: the plan–do–check–act discipline from quality
management, applied to delivery processes. Its integrity rests entirely on the last step, which is
the one usually skipped: an improvement implemented but never re-measured is a change, not an
improvement, and organisations accumulate large numbers of them. The measure that keeps the loop
honest is the **recurrence rate** of 9.3.3, because it is the only figure that cannot be satisfied
by activity.

### 9.4.2 First-time-right and the compounding of stage yields

**Definition.** The **first-time-right yield** of a step is the share of items passing it without
rework. The **rolled throughput yield** (`RTY`) of a chain of sequential steps is the product:

```
RTY = ∏ yᵢ            yᵢ = first-time-right yield of step i
```

Sequential steps compound multiplicatively, which is why chains of individually respectable steps
produce disreputable end-to-end results, and why step-level metrics can all look healthy while
delivery does not.

**Worked example 9.4.2 — Auriga's handover chain: six good steps, one coin flip.**

1. **Setup.** Auriga's 60 handover packages (one per plant area) pass through six sequential steps
   with observed first-time-right yields: design package issue **0.95**, supplier document review
   **0.92**, factory acceptance **0.90**, site installation **0.94**, site acceptance test **0.88**,
   client handover documentation **0.85**. A first-pass failure at any step triggers a rework loop
   costing on average **USD 2,600**.
2. **Formula.** `RTY = ∏ yᵢ`. First-pass failures at step `i` = items reaching it first time ×
   `(1 − yᵢ)`; total rework loops = `Σ` those failures. Uniform yield required for a target `RTY` over
   `k` steps = `RTY^(1/k)`.
3. **Substitution.** `0.95 × 0.92 × 0.90 × 0.94 × 0.88 × 0.85`. Flow from 60: failures
   `3.0000, 4.5600, 5.2440, 2.8318, 5.3237, 5.8561`.
4. **Result.** `RTY` = **55.3074 %**. The arithmetic mean of the six step yields is **90.6667 %**: a
   figure describing nothing anyone experiences. Of 60 packages, **33.18** pass the whole chain
   first time and **26.82** require at least one rework loop, costing **USD 69,720**.
5. **Interpretation.** The gap between 90.67 % and 55.31 % is the point, and it is the arithmetic
   behind a complaint heard in every delivery organisation: *every function reports good numbers and
   the handover is still chaos.* Both figures are correct; only one is relevant, because a package must
   survive all six steps and the customer experiences the product, not the mean.

   **Where to improve is determined by the product form, not by judgement.** Raising the weakest
   step (handover documentation, 0.85 → 0.95) multiplies `RTY` by `0.95/0.85` to **61.8142 %**, a
   gain of **6.507 percentage points**, cutting rework loops from 26.82 to 22.91 and saving **USD
   10,151** on this chain alone. Raising the strongest step, 0.95 → 0.98, gives **57.0540 %**: a
   gain of **1.747 points**, or **3.73 times less**, for effort that is usually harder because the
   step is already good. The binding step is identifiable from the yields with no judgement at all.

   **The target arithmetic should be said out loud to sponsors.** An end-to-end first-time-right
   target of **80 %** over six steps requires **96.35 %** at every single step (`0.80^(1/6)`); a 90 %
   target requires **98.26 %**. That is why "we are about 90 % right" organisations fail at handover,
   and why a target set without this calculation is an instruction nobody can execute.

   Three cautions. The failure counts are **first-pass failures**, counted on the flow reaching each
   step first time; a package failing twice contributes two loops in reality, and the model states
   its convention rather than hiding it. `RTY` assumes steps are **sequential and each must be
   passed**: parallel or optional steps need a different structure, and a chain drawn wrongly gives
   a confidently wrong number. And a step yield is meaningful only where the step has a **defined
   pass criterion**: yields measured against an undefined standard record the reviewer's mood, so
   Domain 5, KA 5.4.2's testability requirement is the prerequisite for this whole measure.

> **Fig 9.4.1 — Rolled throughput yield: six good steps, one coin flip.** Line chart, x-axis the six
> sequential steps of Auriga's handover chain with each step's own first-time-right yield printed
> beneath (0.95 · 0.92 · 0.90 · 0.94 · 0.88 · 0.85), y-axis cumulative first-time-right yield from
> 40 % to 100 %. A descending blue line starts at 100 % and falls through **95.00 · 87.40 · 78.66 ·
> 73.94 · 65.07 · 55.31 %**, ending at a crimson marker labelled **55.31 % first time right end to
> end**. A grey dashed horizontal line at **90.67 %** is labelled "arithmetic mean of the six step
> yields", showing how far the mean sits above the experienced result. A side note records that
> fixing the weakest step (0.85 → 0.95) reaches **61.81 % (+6.51 points)** while fixing the
> strongest (0.95 → 0.98) reaches only **57.05 % (+1.75 points)**: **3.73 times the gain for the
> same effort**. Source: PCI original. Alt text: a line descending in six steps from one hundred per
> cent to fifty-five per cent, well below a dashed line marking the ninety-one per cent average of
> the individual step yields.

### 9.4.3 Data quality as a delivery constraint

A project's decisions are made on its data (the asset register, the requirements baseline, the cost
ledger, the risk register, progress measurement), and data with defects produces decisions with
defects, at a scale and speed no deliverable inspection catches. Data quality is also what
determines whether the analytics, dashboards and AI systems of Domain 14 are usable at all, and the
standard sequencing error in digital delivery is to buy the analytics before fixing the data.

**The dimensions**, each measurable as a pass rate over records: **completeness** (required fields
populated), **accuracy** (values correspond to reality), **validity** (values conform to defined
format, type and range), **consistency** (the same fact agrees across records and systems),
**timeliness** (current enough for the decision), **uniqueness** (no unintended duplicates).
International reference points may be cited by name (the ISO 8000 series on data quality and the
ISO/IEC 25012 data quality model), and both are frameworks for defining and assessing dimensions
rather than sources of a target number; the target is a project decision.

**Worked example 9.4.3 — Auriga's asset register, and why 96 % is 78 %.**

1. **Setup.** Auriga must hand over an asset register of **3,200** equipment records to the utility's
   maintenance system. Sampling gives per-dimension conformance of completeness **0.960**, accuracy
   **0.930**, validity **0.980**, consistency **0.910**, timeliness **0.990**, uniqueness **0.995**. A
   record is fit for use only if it satisfies **all six**.
2. **Formula.** Composite fitness = `∏` dimension conformance rates (treating dimensions as
   independent). Conforming records = composite × record count.
3. **Substitution.** `0.960 × 0.930 × 0.980 × 0.910 × 0.990 × 0.995`.
4. **Result.** Composite fitness **78.43 %** against an arithmetic mean of the six dimensions of
   **96.08 %**. Of 3,200 records, approximately **2,510** are fit for use and **690** are not.
5. **Interpretation.** This is 9.4.2's compounding applied to data, producing the same surprise for
   the same reason: a dashboard reporting six dimensions all above 91 % is reporting a register in
   which nearly a quarter of records fail somewhere. The remediation logic is also the same, attack
   the weakest dimension. Raising **consistency** from 0.910 to 0.980 lifts the composite to **84.46
   %**, a gain of **6.03 points** and roughly **193** more usable records, for work that is usually
   reconciliation rather than re-survey. Three professional points. Independence across dimensions
   is an **assumption and usually optimistic**: records that are incomplete are often also
   inconsistent, so for correlated defects the true composite tends to be better than the naive
   product, and the figure should be sample-verified end to end rather than only dimension by
   dimension. The composite must be computed against **fitness for a named decision**: a register
   good enough for statutory reporting may be inadequate for condition-based maintenance, and a
   single "data quality percentage" divorced from a use is a number without a meaning. And data
   quality is an **acceptance criterion**, not a wish: the 690 non-conforming records are a
   nonconformance under 9.3.1, with a disposition, an owner and a date, or they will be discovered
   by the receiving organisation after Domain 16's transition, when the project team has dispersed.

### 9.4.4 AI-output quality and the verification that earns its cost

When an AI system produces work a professional must sign for (drafted commissioning procedures,
generated test cases, extracted requirements, a summarised evidence pack) the professional's
obligation is unchanged: they are accountable for the output (Domain 1; *AI proposes; the
professional verifies, decides and remains accountable*). The practical question is therefore not
whether to verify but **how much**, and that is a sampling question with an answer, which is where
9.3.2's arithmetic becomes the most useful tool in this domain.

Two properties of AI output make the answer differ from a supplier's. Errors are **systematic rather
than random**: a model that misunderstands an instruction misunderstands it consistently, so defects
cluster by type and stratified sampling across item types is essential. And output is **fluent
regardless of correctness**, which removes the informal signal reviewers rely on with human work: an
unsure human writes hesitantly, and a wrong model does not.

**Worked example 9.4.4 — verifying 240 AI-drafted commissioning steps.**

1. **Setup.** Auriga's commissioning team uses an AI assistant to draft **240** commissioning steps
   from the design documents. A reviewer checks a random **20** and finds no error. Reviewing one step
   costs **USD 40** of engineer time; an erroneous step reaching execution costs **USD 1,800** (an
   aborted test, a re-scheduled outage slot, a re-issued procedure). What verification is defensible?
2. **Formula.** As 9.3.2: 95 % bound `1 − 0.05^(1/n)`; breakeven error fraction `p* = c/u`; sample to
   bound the error rate at a target `p`: `n = ln(0.05)/ln(1 − p)`.
3. **Substitution.** `1 − 0.05^(1/20)`; `40/1,800`; `ln(0.05)/ln(0.98)`.
4. **Result.** The clean sample of 20 bounds the error rate at **13.91 %**: up to **33.39** of the
   240 steps, i.e. as many as 33 erroneous steps. The breakeven error fraction is `40/1,800 =`
   **2.22 %**. Bounding the error rate at **2 %** would require **149** items: **62.1 %** of the
   population, at a review cost of **USD 5,960** against **USD 9,600** to review all 240. If the
   error rate really were at the bound, the 220 unreviewed steps would carry an expected consequence
   of `0.1391 × 220 × 1,800 =` **USD 55,087**.
5. **Interpretation.** The conclusion is uncomfortable and it is arithmetic, not conservatism.
   **Because the breakeven error fraction (2.22 %) is far below what a small sample can bound (13.91
   %), and because the sample needed to get below the breakeven is 62 % of the population, the
   defensible regime here is full review**: USD 9,600 against a potential exposure of USD 55,087 on
   the unreviewed remainder. And that is precisely where the claimed productivity gain goes: **the
   saving from generating a deliverable is real only to the extent that the verification it requires
   is cheaper than producing it correctly in the first place.** A leader who accepts a time saving
   on generation without pricing the verification has not made a saving; they have moved a cost
   somewhere nobody is measuring it, and accepted an accountability whose evidence base is a sample
   of 20.

   Two refinements make this practical rather than merely discouraging. The ratio `c/u` is a
   **design variable**: reducing `u` (by placing an independent, cheap containment layer *after* the
   AI-drafted step and before execution, as 9.2.2 would) raises the breakeven error fraction and can
   make sampling defensible where full review was not. The economics reward **building a containment
   chain around AI output**, not reviewing harder. And the argument is **per item class, not per
   tool**: a low-consequence output may be sampled lightly and legitimately, so a verification
   policy applying one sample size to everything is wrong in both directions at once. The governance
   instrument is a **verification standard proportional to consequence** (Domain 3, KA 3.A.2's
   requirement with the arithmetic attached), and Domain 14 develops the wider frame, including the
   standards that may be cited by name (ISO/IEC 42001 for AI management systems, ISO/IEC 23894 for
   AI risk management).

### AI in this KA

**Where it earns its place.** Reading a large lessons repository and clustering entries into
recurring themes with counts, which is what makes a repository usable. Computing rolled throughput
yields and their sensitivity to each step, identifying the binding step and the uniform yield a
target implies. Profiling a data set across the six dimensions of 9.4.3 (a strong application,
because dimension profiling is mechanical, high-volume and exactly checkable), and proposing the
remediation rules that would fix the largest number of records. Generating candidate improvement
actions from a nonconformance pattern, for human triage.

**Where it must not go.** It must not decide which lessons enter the standing process; that is a
change to how the organisation works and needs an owner who can be held to it. It must not be the
sole judge of its own output quality: a model's self-assessed confidence is not a detection rate,
and a second pass by the same model is not an independent layer (9.2.2). It must not remediate data
silently: an inferred value that fills a completeness gap improves the metric and can degrade the
register, the worst combination available. And it must not set the verification sample size for its
own output, because that is the decision whose independence the whole argument of 9.4.4 rests on.

**Verification, concretely.** Characterise every AI containment layer with a **measured** detection
rate against a held-back defect set, and re-measure on any change of model, prompt or artefact type.
Compute the verification sample from the consequence ratio `c/u` and record that computation in the
quality plan, so the sample size has a stated basis rather than being a habit. Where data has been
machine-remediated, hold an untouched control sample and compare, since a plausible wrong fill is
detectable only against ground truth. And reproduce the yields, composites and bounds by hand: every
calculation in this KA is a product, a power or a division.

### Key terms — KA 9.4

| Term | Meaning |
|---|---|
| **First-time-right yield (`yᵢ`)** | The share of items passing a step without rework. |
| **Rolled throughput yield (`RTY`)** | `∏ yᵢ`, the share of items passing every sequential step first time. |
| **Binding step** | The step whose yield improvement raises `RTY` most; identifiable from the yields alone. |
| **Uniform yield requirement** | `RTY^(1/k)`: the per-step yield an end-to-end target implies over `k` steps. |
| **Data quality dimensions** | Completeness, accuracy, validity, consistency, timeliness, uniqueness. |
| **Composite fitness** | The product of dimension conformance rates; the share of records fit for use. |
| **Recurrence rate** | Repeat nonconformances as a share of all; the honest measure of an improvement loop. |
| **Verification standard proportional to consequence** | A rule setting review depth from the cost of an escaped error, not from convenience. |
| **Systematic error** | An error repeating consistently across similar items, the characteristic shape of AI error, requiring stratified sampling. |

### Sample MCQs — KA 9.4

**MCQ 9.4-A `[9.4.2 · Application]`** Six sequential steps have first-time-right yields 0.95, 0.92,
0.90, 0.94, 0.88 and 0.85. The rolled throughput yield is closest to:
- A. 90.7 %
- B. 55.3 % ✅
- C. 44.7 %
- D. 85.0 %

*Rationale:* `RTY` is the product, 0.55307 (9.4.2). A is the arithmetic mean of the six yields, which
describes nothing anyone experiences; C is the complement of the product; D is the worst single step.

**MCQ 9.4-B `[9.4.2 · Evaluation]`** On that chain, improvement effort is best directed at the step
with yield 0.85 rather than the one with 0.95 because:
- A. the 0.85 step is later in the chain
- B. raising 0.85 to 0.95 multiplies `RTY` by 0.95/0.85 and gains 6.51 points, against 1.75 points for
  raising 0.95 to 0.98 ✅
- C. late steps are always cheaper to improve
- D. the 0.95 step is already compliant

*Rationale:* The multiplicative form makes the gain proportional to the ratio of new to old yield,
so the weakest step gives the largest lift, here 3.73 times as much (9.4.2). A confuses position
with leverage; C is unsupported.

**MCQ 9.4-C `[9.4.2 · Analysis]`** An end-to-end first-time-right target of 80 % across six
sequential steps requires a per-step yield of about:
- A. 80.0 %
- B. 96.4 % ✅
- C. 93.3 %
- D. 88.9 %

*Rationale:* `0.80^(1/6) = 0.9635` (9.4.2). A applies the end-to-end figure per step; C divides 80 %
by 6 and adds it back; D treats the losses as additive.

**MCQ 9.4-D `[9.4.3 · Application]`** Six data quality dimensions score 0.960, 0.930, 0.980, 0.910,
0.990 and 0.995, and a record is fit for use only if it satisfies all six. Composite fitness is
closest to:
- A. 96.1 %
- B. 78.4 % ✅
- C. 91.0 %
- D. 21.6 %

*Rationale:* The product is 0.7843 (9.4.3). A is the arithmetic mean; C is the weakest dimension; D is
the complement of the product.

**MCQ 9.4-E `[9.4.4 · Evaluation]`** Reviewing one AI-drafted item costs 40; an escaped erroneous item
costs 1,800; a clean sample of 20 has been taken from 240 items. The defensible conclusion is:
- A. the sample is clean, so the output may be accepted
- B. the breakeven error fraction is 2.22 % while a clean sample of 20 bounds the rate only at
  13.91 %, and reaching the breakeven needs 149 items — so full review at 9,600 is indicated ✅
- C. the sample should be increased to 59 items, bounding the rate at 5 %
- D. the output should not be used at all

*Rationale:* The bound must lie below the breakeven for the plan to support its own decision (9.4.4,
applying 9.3.2). A accepts on a sample that cannot exclude 33 erroneous items; C bounds at 5 %,
still more than double the breakeven; D is not supported, the arithmetic supports full review, not
abandonment.

**MCQ 9.4-F `[9.4.4 · Analysis]`** Which measure most raises the breakeven error fraction and so makes
sampling AI output defensible?
- A. increasing the sample size
- B. adding an independent, cheap containment layer after the AI step and before execution, which
  lowers the cost of an escaped error ✅
- C. asking the model to check its own output
- D. improving the prompt

*Rationale:* The breakeven is `c/u`, so lowering `u` raises it (9.4.4 with 9.2.2). A changes the
bound, not the breakeven; C is not an independent layer; D may lower the error rate but does not
change the economics of verification.

### Self-check — KA 9.4

1. *Why does a chain of six steps averaging 90.67 % deliver 55.31 %?* — Because sequential yields
   compound multiplicatively; the mean describes no item's experience, and every item must pass all
   six.
2. *What per-step yield does an 80 % end-to-end target require over six steps?* — 96.35 %
   (`0.80^(1/6)`), which is why targets set without this calculation cannot be executed.
3. *How is the verification sample for an AI-produced deliverable set?* — From the consequence ratio:
   the sample must bound the error rate below `p* = c/u`; on Auriga's commissioning steps that is
   2.22 %, needing 149 of 240 items, so full review is the defensible regime.

---

## Advanced topics — Domain 9

### 9.A.1 Quality across a contractual boundary

Where a supplier produces the work, every mechanism in this domain must survive a commercial
boundary, and each acquires a failure mode a leader should assume present until disproved.

**Containment layers become contractual artefacts.** A layer exists only if the contract obliges the
supplier to run it, entitles the client to witness or audit it, and defines what its evidence must
show. Absent those, the client's chain is one layer shorter than the plan says and its escape
fraction is worse by a factor of `1/(1 − dᵢ)` for the layer that turns out not to be real.
**Detection rates become commercial information**: a supplier has no incentive to disclose a weak
internal detection rate, so the client's chain arithmetic runs on the supplier's optimism unless the
client's own layer is independently staffed. **Nonconformance dispositions become negotiations**: a
concession the client's design authority should grant is routinely proposed by the supplier
alongside a schedule benefit, and 9.3.1's rule about who bears the consequence has to hold against
commercial pressure. And **cost of quality relocates without falling**: a fixed-price contract moves
prevention and appraisal into the supplier's cost base, where the client cannot see them and can
only infer them from escapes, and the supplier's optimum, computed on *their* external-failure unit
cost (a warranty obligation) rather than the client's (an operating consequence), is systematically
lower. That divergence is the most important quality fact about fixed-price delivery, and the
countermeasures are contractual: specified containment layers with witness rights, defect-liability
terms that move `u` closer to the client's true figure, and acceptance criteria and sampling plans
agreed before award rather than argued at delivery. Domain 10 handles the contracting machinery; the
quality point is that a contract not specifying the containment chain has not specified the quality.

### 9.A.2 Governing quality in adaptive and hybrid delivery

Iterative delivery changes where quality is decided, not whether it is. **The definition of done
replaces the inspection**: quality is built into the step rather than inspected at the end, so the
definition of done *is* the containment layer and must be stated with the specificity of an
acceptance criterion and measured with the discipline of a detection rate, or it is a slogan.
**Technical debt is deferred internal failure, and it compounds**: work knowingly left
non-conforming raises the introduced-defect count of every subsequent increment, so the honest
treatment is a quantified backlog item that 9.2.2's arithmetic can see, not a cultural complaint.
**Rework capacity is the flow constraint**: KA 9.2.3's multiplier `1/(1 − r)` is exactly how
velocity degrades (a 30 % rework share is a 1.4286-times duration multiplier on everything,
presenting as a team that has become slower rather than as a quality problem), and the measure that
surfaces it is the rework share of capacity per iteration, reported alongside throughput.

The hybrid case, which is most real programmes, adds one requirement: parts of the work under
different methods have **different containment chains and different escape fractions**, and the escape
fraction of a deliverable is the product across the chain it actually traverses, not an average of the
two regimes. A component built iteratively with a strong definition of done and then installed under a
sequential commissioning regime inherits both chains; a leader reasoning about the average will
under-estimate escapes on the path that matters.

### 9.A.3 The reviewer's quality eye

Invariants to test on any quality regime, each cheap and each diagnostic.

The **four cost-of-quality categories are defined and counted consistently**, with internal failure
not quietly reclassified as appraisal. There is a **computed total cost of quality** with a stated
percentage of budget and a **marginal analysis**, not merely a chosen regime. The **external-failure
unit cost is stated with its source**, and the recommendation carries a **range of optimality**
rather than a point. The **escape fraction is computed as a product** of layer detection rates, and
consecutive layers are **methodologically different** rather than repetitions. Every layer has a
**measured** detection rate, a coverage statement and a unit correction cost, including automated
and AI layers, which are characterised, not trusted. There is an **expected containment cost per
introduced defect**, and every prevention proposal is tested against it. The plan states a **rework
allowance** as a percentage of capacity, actual rework share is measured against it weekly, and the
**schedule** consequence is priced at the cost of delay on any zero-float activity. Every acceptance
is a **positive record with a named signatory** and a list of open nonconformances. Every concession
is authorised by the **party bearing the consequence** and subject to a **cumulative-effect test**.
Every sampling plan is **self-consistent**: its confidence bound below its own breakeven `c/u`. Root
causes pass the **three tests**, and the **recurrence rate** is reported rather than the closure
rate. `RTY` is computed as a product, improvement is directed at the **binding step**, and any
end-to-end target carries the **uniform per-step yield** it implies. Data quality is expressed as a
**composite against a named use**, not an average of dimensions. And AI-produced work carries a
**verification sample derived from `c/u`**, recorded in the quality plan. The test that catches the
commonest new defect in modern delivery, which is an accountability accepted on the evidence of a
sample of twenty.

---

## Industry variations — Domain 9

- **Utilities, energy and process industries.** External failure is dominated by outage windows and
  availability obligations, so `u` is high, `p* = c/u` is low, and full verification is far more
  often the economic answer than elsewhere, which is why Auriga's regime sits at the strict end.
  Commissioning is the concentrated rework risk and is on the critical path almost by construction.
- **Regulated life sciences and medical devices.** Quality is a *licence* condition, not an
  optimisation: certain containment layers and record requirements are mandatory whatever the
  arithmetic says, and the cost-of-quality model runs *inside* that constraint. The professional error
  is presenting an economic optimum that breaches a regulatory requirement; the correct output is the
  optimum among compliant options.
- **Software and digital services.** Correction is cheap and deployment reversible, so `u` is low
  and rapid detection with fast rollback frequently beats heavy prevention. The characteristic
  failure is applying that logic to the irreversible parts (data migrations, external interfaces,
  published records), where a rollback does not undo the consequence.
- **Construction and infrastructure.** Physical rework is expensive and often infeasible, so
  prevention and early appraisal dominate and the correction-cost ladder is steeper than Auriga's.
  Concession management is central because a physical nonconformance frequently cannot be reworked at
  any acceptable cost, which puts real pressure on 9.3.1's rule about who may grant one.
- **Aerospace, rail and safety-critical transport.** Escapes carry consequences not expressible as an
  average cost, so 9.1.2's expected-value logic is bounded by a safety case and traceability
  obligations; sampling plans are typically fixed by an approval regime rather than derived, and the
  leader's task is to know the arithmetic well enough to see when a mandated plan is *weaker* than the
  economics would require.
- **Public services and administrative programmes.** Fitness for purpose dominates conformance,
  because the deliverable is a service experienced by citizens who did not write the specification;
  and data quality (9.4.3) is frequently the binding constraint on the whole benefit, since the
  records were assembled for a different purpose over decades.

---

## Case study — Domain 9: the fortnight in commissioning (utilities, Auriga)

**Situation.** Auriga entered testing and commissioning (activity F, 4 weeks, 6 engineers, on the
critical path with zero float), after the week-13 review that had already shown `CPI` 0.91 and `SPI`
0.92 (Domain 7). Two weeks into F, the commissioning manager reported that the team was "working
through a lot of retests" and asked for a decision by the end of the week. The programme had no
rework metric, so nobody could say how much.

**What the arithmetic showed.** The project leader had the time coding re-cut and computed four
numbers. Rework was consuming **30 %** of commissioning capacity against a plan that assumed **16.67
%**: 4 engineer-weeks of allowance in a 24-engineer-week phase. At 30 %, `20 ÷ (6 × 0.70)` gives
**4.7619 weeks**, an overrun of **0.7619 weeks** worth **USD 34,286** at Auriga's cost of delay of
45,000 per week, plus **4.5714 engineer-weeks** of unbudgeted labour at USD 5,225 — **USD 23,886**.
Total consequence: **USD 58,171**, of which 59 % was schedule and therefore invisible in every
report the programme produced. The trend mattered more than the level: at 40 % the phase would run
**5.5556 weeks** and the delay cost **USD 70,000**, a further **USD 35,714** for ten points, against
**USD 20,833** for the ten points between 10 % and 20 %.

**The proposal, and why it was rejected.** The commissioning manager recommended halving site
acceptance test coverage to recover the schedule. Priced against 9.2.2's chain, dropping L3's
detection rate from 0.50 to 0.25 raises escapes from **8** to **12**, adding `4 × 12,000 =` **USD
48,000** of external failure against `4 × 3,500 =` USD 14,000 of internal correction saved, a net
loss of **USD 34,000** before any schedule benefit. The recovery on offer could not close that gap,
and the leader was able to say so in one sentence with a number attached, which is the difference
between a governance conversation and an argument about commitment to quality.

**What was done instead.** Two decisions. **Buy capacity.** With 11.6 engineer-weeks of first-pass
content remaining, adding two engineers on an off-shift basis takes the remaining duration from
2.7619 to **2.0714 weeks**, a total phase duration of **4.0714 weeks** and an overrun of only
**0.0714 weeks**, or **USD 3,214** of delay instead of USD 34,286, a saving of **USD 31,071** for
4.1429 engineer-weeks of extra capacity costing USD 21,646 plus a 25 % off-shift premium of USD
5,412, or **USD 27,058** in total. Net **USD 4,013**: thin, worth doing, and worth recognising as
thin. **Remove the cause.** Root-cause analysis showed **5 of 8** escapes, a cause concentration of
**62.5 %**, traced to an uncontrolled supplier firmware configuration baseline. Removal cost **USD
18,000** against **5** expected recurrences at 12,000 — **USD 60,000** avoided, a net **USD
42,000**, breaking even at **1.5** recurrences.

**What the domain teaches here.** The most valuable calculation was the counterfactual. Had Auriga
taken option (b) of 9.2.2b (USD 40,000 of prevention at the end of design, cutting introduced
defects from 80 to 60) site acceptance would have found 6 defects rather than 8, removing **1.5
engineer-weeks** of rework, shortening F to **4.5119 weeks** and saving **USD 11,250** of delay *on
top of* the USD 11,000 of cost-of-quality saving the model had already shown. The prevention option
was worth **USD 22,250**, not USD 11,000, and the missing half was schedule. **The cost-of-quality
model systematically understates prevention, because it contains no clock.** The practical
consequence is that any prevention proposal touching a critical-path activity should be priced
twice, once in the `CoQ` model and once at the cost of delay. Note also which decisions were still
available at week 15: buying capacity at a net USD 4,013, and removing a cause for the *next*
occurrences. The decision worth USD 22,250 had expired at the end of design, and nobody had been
asked to make it.

## Case study B — Domain 9: the clean sample (rail signalling, transport)

**Situation.** A metropolitan signalling renewal programme accepted a supplier's batch of **480**
axle-counter installations on the evidence of a **30-item** zero-defect sample. Verifying one
installation cost **USD 900**: a possession slot, two technicians and a test set. A defective
installation reaching service cost the programme an estimated **USD 21,000**: an emergency
possession, retest, re-commissioning and the operator's performance regime. The sample was drawn at
random, was clean, and the batch was accepted. Over the following eleven months, **21** of the 450
unsampled installations were found defective in service (an observed defective fraction of **4.667
%**) at a realised cost of **USD 441,000**.

**What the arithmetic showed afterwards.** The plan had been unsound before it was run, and the test
that would have shown it takes two lines. The breakeven defective fraction was `p* = 900/21,000 =`
**4.286 %**. The 95 % upper confidence bound after a clean sample of 30 is **9.50 %**: more than
twice the breakeven. The plan could not exclude a defect rate at which verifying all 480
installations would have been correct; its bound exceeded its own breakeven by **5.22 percentage
points**. The self-consistent sample size, the smallest `n` whose 95 % bound falls below 4.286 %, is
**69** (the bound is 4.249 % at 69 and 4.310 % at 68), costing `69 × 900 =` **USD 62,100** against
the **USD 27,000** actually spent: an extra **USD 35,100**. At the defect rate that in fact
obtained, a 30-item plan passes the batch **23.84 %** of the time and a 69-item plan **3.70 %**, so
the extra 39 items would have converted a one-in-four chance of missing the problem into a
one-in-twenty-seven chance: an expected saving of `(0.2384 − 0.0370) × 441,000 =` **USD 88,838** for
USD 35,100 of additional testing, a net **USD 53,738**.

**How it resolved.** The realised escape cost of USD 441,000 slightly exceeded the **USD 432,000**
that verifying all 480 installations would have cost, a coincidence worth noticing, because it means
even the most conservative option would have been only marginally justified after the event and was
genuinely hard to justify before it. That is why the correction was not "inspect everything". Three
changes went into the framework contract for the following batches. Acceptance plans acquired a
**self-consistency test**: the plan's 95 % bound must lie below the breakeven `c/u`, with both
figures stated in the acceptance paper. The escaped-defect unit cost was **defined contractually**,
including the operator's performance regime, so `p*` could be computed rather than argued. And the
supplier's own containment layers were **specified with witness rights**, on the reasoning of 9.A.1.
The client's escape fraction had been computed on the assumption that the supplier ran an internal
check the contract had never required.

**What the domain teaches here.** A passing sample is not evidence of conformance; it is a bound, and
the bound has to be compared with something. The something is `p* = c/u`, and the comparison is the
whole of acceptance-sampling judgement: **a plan whose confidence bound exceeds its own breakeven
defective fraction cannot support the decision it is being used to make, however cleanly it passes.**
The remedy was cheap, available before any test was run, and needed no statistical sophistication
beyond one power and one division.

---

## Executive perspective — Domain 9

What a programme director cannot delegate in this domain:

- **The external-failure unit cost, and therefore the quality regime.** `u` sets where the
  cost-of-quality minimum sits and what `p* = c/u` is; it is a judgement about consequence in an
  operating context, and every quality decision below it inherits it (9.1.2, 9.3.2).
- **The decision that zero defects is not the objective, and the version that is.** "No defect whose
  prevention costs less than its consequence" is computable; a commitment to zero defects buys the
  last escapes at, on Auriga's figures, 1.79 times their harm (9.1.2).
- **The rework share of capacity as a schedule metric.** Require it weekly against a stated allowance
  on every critical-path activity, priced at the cost of delay. Its absence is why commissioning
  overruns arrive as surprises (9.2.3).
- **Who may grant a concession, and whether concessions are counted.** Never the producer alone, never
  without a cumulative-effect test; this is Domain 3's Case study B at deliverable level and it is how
  a delivered product ends up materially different from the specified one (9.3.1).
- **The self-consistency of every acceptance plan you rely on.** Two numbers (the 95 % bound and
  `c/u`), and the plan either supports its decision or does not. Ask for both in the acceptance
  paper (9.3.2, Case study B).
- **The verification standard for AI-produced work, derived rather than habitual.** The sample must
  bound the error rate below `c/u`; where it cannot, the productivity gain was never real and the
  accountability has been accepted on the evidence of a sample of twenty (9.4.4).

---

## Calculation exercises — Domain 9

**Exercise 9.1** A programme with a budget of USD 6,000,000 can adopt one of four quality regimes. An
escaped defect costs **USD 9,000**.

| Regime | Prevention | Appraisal | Introduced | Detected | Internal unit cost |
|---|---|---|---|---|---|
| Q1 | 15,000 | 45,000 | 150 | 80 % | 1,000 |
| Q2 | 55,000 | 65,000 | 110 | 90 % | 1,200 |
| Q3 | 120,000 | 85,000 | 80 | 95 % | 1,400 |
| Q4 | 220,000 | 110,000 | 50 | 96 % | 1,500 |

Compute the total cost of quality for each, identify the optimum, and state the external-failure
unit cost at which the optimum would move one regime stricter. *Solution.* Escapes 30, 11, 4, 2.
Internal failure `120 × 1,000 = 120,000`; `99 × 1,200 = 118,800`; `76 × 1,400 = 106,400`; `48 ×
1,500 = 72,000`. External failure 270,000; 99,000; 36,000; 18,000. Totals **USD 450,000** (7.50 % of
budget), **USD 337,800** (5.63 %), **USD 347,400** (5.79 %), **USD 420,000** (7.00 %). The optimum
is **Q2**. The Q2 → Q3 step costs `205,000 − 120,000 =` 85,000 more in conformance and returns
12,400 of internal failure and 7 escapes, so it pays once `u ≥ (85,000 − 12,400)/7 =` **USD
10,371.43**. The assumed 9,000 sits only **13.22 %** below that flip point, so the recommendation is
**fragile** and the paper must say so. (For completeness, Q1 → Q2 pays above **USD 3,094.74** and Q3
→ Q4 above **USD 45,300**.) *Common error:* choosing the regime with the fewest escapes, or
reporting the minimum without the range of optimality, which converts a fragile recommendation into
an apparently settled one.

**Exercise 9.2** A process introduces **150** defects. Three containment layers have detection rates
**0.40**, **0.50** and **0.60**, with correction costs of **USD 700**, **USD 2,200** and **USD
5,000**. An escaped defect costs **USD 16,000**. Compute the escape fraction, the escapes, the
internal and external failure costs, and the expected containment cost per introduced defect. Then
evaluate a prevention programme costing **USD 95,000** that would avoid **30** defects. *Solution.*
Escape fraction `0.60 × 0.50 × 0.40 =` **0.120** (detection 88.0 %). Flow: L1 finds `150 × 0.40 =
60` (90 remain), L2 finds `90 × 0.50 = 45` (45 remain), L3 finds `45 × 0.60 = 27`, escapes **18**.
Internal failure `60 × 700 + 45 × 2,200 + 27 × 5,000 = 42,000 + 99,000 + 135,000 =` **USD 276,000**.
External failure `18 × 16,000 =` **USD 288,000**. Total nonconformance **USD 564,000**, so the
expected containment cost per introduced defect is `564,000/150 =` **USD 3,760**. The prevention
programme returns `30 × 3,760 =` **USD 112,800** for USD 95,000, a net **USD 17,800**; it breaks
even at `95,000/3,760 =` **25.27, i.e. 26 defects avoided**. *Common error:* computing the escape
fraction as `1 −` the sum of the detection rates, or applying each detection rate to the original
150 rather than to the defects reaching that layer.

**Exercise 9.3** A test phase is planned at **6 weeks** with **8** engineers (48 engineer-weeks of
capacity) against **40** engineer-weeks of first-pass work content. Cost of delay is **USD 30,000**
per week and an engineer-week costs **USD 5,000**. Compute the planned rework allowance as a share
of capacity; then the duration, overrun, delay cost and unbudgeted labour if rework runs at **35
%**; and the rework share at which the phase would take twice its planned duration. *Solution.*
Allowance `48 − 40 = 8` engineer-weeks = **16.67 %** of capacity (equivalently `1 − 40/48`). At `r =
0.35`: duration `40 ÷ (8 × 0.65) =` **7.6923 weeks**, overrun **1.6923 weeks**, delay cost **USD
50,769**. Rework consumed `0.35 × 8 × 7.6923 =` **21.5385** engineer-weeks, **13.5385** above
allowance, costing **USD 67,692**. Total consequence **USD 118,462**. For a 12-week duration, `40 ÷
(8(1 − r)) = 12` gives `1 − r = 0.41667`, so `r =` **58.33 %**. *Common error:* adding 35 % to the
6-week plan (giving 8.1 weeks) instead of dividing by `(1 − r)`, and forgetting that the plan
already contained a 16.67 % allowance, so the *variance* is 18.33 points, not 35.

**Exercise 9.4** A population of **300** welds is to be accepted. Verifying one weld costs **USD
260**; a defective weld reaching service costs **USD 6,500**. The proposed plan verifies **30**
welds and accepts on zero defects. Compute (a) the probability the plan passes a population that is
4 % defective, (b) the 95 % upper confidence bound after a clean sample of 30, (c) the breakeven
defective fraction, and (d) the smallest self-consistent sample size and its cost. *Solution.* (a)
`0.96³⁰ =` **29.39 %**. (b) `1 − 0.05^(1/30) =` **9.50 %** — on 300 welds, up to **28.51**
defective. (c) `p* = c/u = 260/6,500 =` **4.00 %**. (d) `n = ln(0.05)/ln(0.96) = 73.385`, so **74**
(the bound is 3.967 % at 74 and 4.021 % at 73), costing `74 × 260 =` **USD 19,240** against USD
7,800 for the 30-weld plan and **USD 78,000** for verifying all 300 — the self-consistent plan costs
only **24.7 %** of full verification. Note that the proposed plan's bound (9.50 %) is more than
twice its breakeven (4.00 %): it cannot support the decision it is being used to make. *Common
error:* treating a clean sample as evidence of conformance, or comparing the bound with the *target*
defect rate instead of with `p* = c/u`. The target is an aspiration, the breakeven is the economics.

**Exercise 9.5** A document chain has five sequential steps with first-time-right yields **0.98**,
**0.94**, **0.91**, **0.96** and **0.89**. **200** items enter and each first-pass failure costs
**USD 1,900**. Compute the rolled throughput yield, the number of first-pass failures, the gain from
raising the weakest step to 0.96 against raising the strongest to 0.99, and the uniform per-step
yield a 90 % end-to-end target would require. *Solution.* `RTY = 0.98 × 0.94 × 0.91 × 0.96 × 0.89 =`
**71.6237 %** (the arithmetic mean of the five yields is 93.60 %, which describes nothing).
First-pass failures, computed on the flow reaching each step: `4.0000 + 11.7600 + 16.5816 + 6.7063 +
17.7047 =` **56.7527**, costing **USD 107,830**; **143.2473** items pass clean. Raising the weakest
step 0.89 → 0.96 multiplies `RTY` by `0.96/0.89` to **77.2570 %**, a gain of **5.633 points**,
cutting failures to **45.4860** and saving **USD 21,407**. Raising the strongest 0.98 → 0.99 gives
**72.3545 %**, a gain of **0.731 points**: **7.71 times less**. A 90 % end-to-end target requires
`0.90^(1/5) =` **97.91 %** at every step. *Common error:* averaging the yields rather than
multiplying them, and (more damaging in practice), directing improvement at the step that is easiest
to change rather than at the binding one, which here costs a factor of 7.71 in return.

---

## Practitioner's toolkit — Domain 9

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 9.T.1 — Cost-of-quality and containment sheet

One page per deliverable class, completed at planning and revisited at each gate. **Top block, the
four categories:** prevention, appraisal, internal failure and external failure, each with a budget,
an actual, and a definition of what is counted in it, the definitions matter more than the numbers,
because inconsistent classification is the commonest way the model is made to lie. **Middle block,
the containment chain:** one row per layer with its coverage, its *measured* detection rate and its
source, its unit correction cost, and a note of whether the next layer is methodologically
different; a computed escape fraction `∏(1 − dᵢ)`; escapes; and the **expected containment cost per
introduced defect**, which is what every prevention proposal is then tested against. **Bottom block,
the decision:** total cost of quality with its percentage of budget, the marginal step analysis for
one regime stricter and one looser, and the **range of optimality** on the external-failure unit
cost. A sheet whose bottom block is blank has recorded a quality regime rather than chosen one.

### Toolkit 9.T.2 — Acceptance and disposition record with the self-consistency test

Per deliverable: reference and version · acceptance criteria referenced to the requirements baseline
· the verification evidence · **the sampling plan, if any, with `n`, the acceptance number, the 95 %
bound `1 − 0.05^(1/n)`, the verification unit cost `c`, the escaped-defect unit cost `u`, the
breakeven `p* = c/u`, and a pass/fail on the test that the bound lies below the breakeven** · the
acceptance decision with a **named signatory and role** · the open nonconformances at acceptance,
each with its disposition (rework / repair / concession / regrade / reject), the authority that
granted it, the party bearing the consequence and **whether an external acceptance is engaged**,
naming the body and who established that (9.3.1) · and the running **cumulative concession total**
against its threshold, with the threshold's **source** (sponsor or design authority, set at
baseline), its **relatedness class**, its **aggregation period** and the **named owner of the
running total** printed on the same sheet, so the test can be audited rather than merely quoted.
Three record fields close it: **record class** · **custodian role** · **retention period with the
source of the period named**, that source being the applicable regime and the contract rather than
the project: the periods themselves come from the organisation's records, regulatory and legal
functions, and nothing in this template states a legal minimum (Domain 3, KA 3.3.4; Domain 16, KA
16.4.4). Monthly integrity counts, each a number: deliverables accepted without a named signatory;
concessions granted by the producer; sampling plans failing the self-consistency test; cumulative
concession value against threshold, **with the classes currently suspended on breach**; and records
with no custodian or no retention period. Those counts will find a quality function's defects before
an auditor does.

### Toolkit 9.T.3 — Improvement register: rework, yield and cause

Three linked sections, all numeric, reported at the same cadence as cost and schedule. **Rework and
capacity:** per activity, the planned rework allowance as a share of capacity, the measured share
this period, the implied duration multiplier `1/(1 − r)`, and (for any activity with zero or minimal
float) the overrun priced at the cost of delay. **Yield:** per process chain, each step's
first-time-right yield, the rolled throughput yield as their product, the identified binding step,
and the uniform per-step yield the current end-to-end target implies. **Cause:** per nonconformance,
the root cause tested against the three tests (prevents this occurrence, prevents the class, within
someone's authority to remove), the removal cost, the breakeven recurrence count `removal cost ÷
unit consequence`, the owner in the **standing process** the change lands in (template, checklist,
gate criterion, estimating parameter, contract clause) and the date effective; plus the two
portfolio measures, **cause concentration** and **recurrence rate**, which together answer the only
question that matters about an improvement programme: are causes being removed, or described?

---

## Exam preparation — Domain 9

**What is assessed.** Quality, grade and fitness for purpose as three distinct tests; the four
cost-of-quality categories and their opposing behaviour, including the non-monotonicity of internal
failure; computing total cost of quality across regimes, locating the interior minimum and stating the
range of optimality; why zero defects is not the objective; assurance versus control; containment
layers, the multiplicative escape fraction and the independence caveat; the correction-cost ladder and
its distinction from Domain 5's requirement ladder; the expected containment cost per introduced defect
as the breakeven price of prevention; prevention against appraisal at equal spend; rework as a share of
capacity, the duration multiplier and its convexity; acceptance decisions, their four failure modes and
the five nonconformance dispositions with their authorities; acceptance sampling, the confidence bound,
the breakeven defective fraction `c/u` and the self-consistency test; root-cause analysis, the three
tests, cause concentration and breakeven recurrence; lessons that change behaviour; rolled throughput
yield, the binding step and the uniform yield requirement; data quality dimensions and composite
fitness; and the derivation of a verification regime for AI-produced work.

**The calculations to be able to do under time pressure.** Total cost of quality for a regime and the
marginal step test between two regimes, including the breakeven external-failure unit cost. Escape
fraction as `∏(1 − dᵢ)` and the defect flow layer by layer. Expected containment cost per introduced
defect, and the breakeven number of defects a prevention spend must avoid. Duration from
`content ÷ (crew × (1 − r))`, the overrun, and its cost at a cost of delay.
`P(clean sample) = (1 − p)ⁿ`; the 95 % bound `1 − 0.05^(1/n)`; the breakeven `c/u`; and the smallest
`n` satisfying `1 − 0.05^(1/n) ≤ c/u`. Cause concentration and breakeven recurrence count. `RTY` as a
product, the gain from a step improvement as a ratio of yields, and the uniform yield `RTY^(1/k)`.
Composite data fitness as a product of dimension rates.

**The traps.** Treating zero defects as the objective and ignoring that the last escapes cost more
than they save (9.1.2, MCQ 9.1-E) · reporting the cost-of-quality minimum without the range of
optimality, which turns a fragile recommendation into a settled one (Exercise 9.1) · reading a rise in
internal failure cost as deterioration when quality is improving (9.1.2, MCQ 9.1-C) · adding detection
rates instead of multiplying escape rates (Exercise 9.2, MCQ 9.2-A) · applying each layer's detection
rate to the original defect count rather than to the defects reaching it (Exercise 9.2) · treating two
layers with a shared method or source as independent detectors (MCQ 9.2-F) · adding a rework percentage
to a duration instead of dividing by `(1 − r)` (Exercise 9.3, MCQ 9.2-D) · forgetting that a plan
already contains a rework allowance, so the variance is the difference and not the whole share
(Exercise 9.3) · comparing escape counts rather than costs when choosing between prevention and
appraisal (9.2.2b, MCQ 9.2-C) · treating a clean sample as evidence of conformance (9.3.2,
MCQ 9.3-A) · believing the breakeven defective fraction depends on the sample size (MCQ 9.3-B) ·
comparing the confidence bound with a target rate instead of with `c/u` (Exercise 9.4) · letting the
producer grant its own concession, or recording concessions without a cumulative test (9.3.1,
MCQ 9.3-D) · stopping root-cause analysis where a person can be blamed (9.3.3, MCQ 9.3-F) · averaging
stage yields instead of multiplying them (9.4.2, MCQ 9.4-A) · improving the strongest step rather than
the binding one (Exercise 9.5, MCQ 9.4-B) · setting an end-to-end yield target without computing the
per-step yield it requires (MCQ 9.4-C) · reporting a data quality average rather than a composite
against a named use (9.4.3, MCQ 9.4-D) · and accepting AI-produced work on a sample whose confidence
bound exceeds `c/u` (9.4.4, MCQ 9.4-E).

**How the domain connects.** Domain 3 supplies the assurance-line model and the gate economics whose
product-level counterpart is the containment chain, and the escalation machinery that carries a root
cause exceeding the project's authority. Domain 4's configuration management is the control whose
absence produced this domain's dominant cause, and its cumulative test is the instrument reused for
concessions. Domain 5 supplies acceptance criteria, testability and the requirement-defect ladder this
domain deliberately does not duplicate. Domain 6 supplies the float that determines whether a rework
overrun is a cost or a delay, and Domain 7 the blended rate and cost of delay that price both.
Domain 8's risk register is where an escape probability belongs once quantified. Domain 10 carries
quality across the contract boundary, where the supplier's optimum and the client's diverge. Domain 14
develops the AI governance frame that 9.4.4's sampling arithmetic serves. And Domain 16 receives the
consequences: a handover chain's rolled yield and an asset register's composite fitness are readiness
measures before they are quality measures.

---

## Domain 9 summary
Quality is conformance to what was agreed; grade is what was agreed; fitness for purpose is whether
the conforming, correctly graded thing works for its use. All three are testable, and only the first
is testable against a document.

The domain's contribution is to make quality an **optimisation with an interior solution.** Auriga's
cost of quality across five candidate regimes runs **660,000 · 448,600 · 364,000 · 397,000 ·
493,000**, and the minimum (**USD 364,000**, **9.1 % of `BAC`**) sits at a regime that lets **8** of
**80** introduced defects escape, not at the strictest one. Removing the last six escapes would cost
**USD 129,000**, or **USD 21,500 each**, against a consequence of USD 12,000, **1.79 times** the
harm prevented. The honest objective is therefore not zero defects but *no defect whose prevention
costs less than its consequence*, and the regime holds for any escaped-defect cost between **USD
3,540** and **USD 20,250**, which is what makes it defensible. Internal failure cost rises before it
falls (132,600 at R1 against 120,000 at R0) because better detection finds what was escaping: the
movement that gets quality programmes cancelled in their first period.

Where the money goes is a **chain of containment layers** whose escape fraction is the *product* of
their escape rates: Auriga's `0.50 × 0.40 × 0.50 =` **0.100**, giving 8 escapes, **USD 108,000** of
internal failure at an average of 1,500 and **USD 96,000** of external failure — **USD 2,550 of
expected containment cost per introduced defect**, which is the breakeven price of prevention and
the most useful single number in the domain. It settles the prevention-or-appraisal question
arithmetically: on the same USD 40,000, cutting introduced defects from 80 to 60 is worth **USD
11,000** while raising the last layer's detection from 0.50 to 0.75 is worth **minus USD 6,000**:
prevention wins by USD 17,000 despite removing fewer escapes, because appraisal relocates detection
to a dearer layer while prevention removes the cost entirely. And rework is capacity before it is
cost: a 30 % rework share against a 16.67 % allowance turned Auriga's 4-week commissioning into
**4.7619 weeks**, costing **USD 34,286** of delay and **USD 23,886** of unbudgeted labour, with the
multiplier `1/(1 − r)` convex enough that the next ten points cost **USD 35,714** and the ten after
that USD 50,000.

Acceptance is a decision with a named signatory, and the five dispositions carry different
authorities: a concession belongs to whoever bears the consequence, never to the producer, and must
be counted cumulatively, under a test whose threshold, derivation, period, relatedness class, owner
and consequence on breach are all fixed at baseline rather than asserted in a procedure. Two things
sit outside the table: where an approval regime applies, a repair, concession or regrade may
additionally need a body outside the project to be told or to accept it before use; and the records
this KA creates (acceptances, concessions, dispositions and their evidence) are the primary evidence
of any later dispute or inspection, so each carries a class, a custodian and a retention period
whose source is named, the periods themselves coming from the applicable regime, the contract and
the organisation's legal and records functions rather than from this book. Sampling yields the
domain's sharpest result: a clean sample of 20 instrument loops bounds the defective fraction only
at **13.91 %**, while the breakeven at which full verification pays is `p* = c/u = 1,400/12,000 =`
**11.67 %**, independent of the sample size, so **the plan cannot exclude the defect rate at which
its own strategy is wrong**, and five more loops (USD 7,000) repairs it. Case study B is the same
defect realised: a 30-item plan with a 9.50 % bound against a 4.286 % breakeven passed a batch that
was 4.667 % defective and cost **USD 441,000**, when **USD 35,100** of additional testing carried an
expected saving of **USD 88,838**. Root causes are reached only when removal prevents the class and
lies within somebody's authority; Auriga's dominant cause carried **62.5 %** of escapes and its
removal broke even at **1.5 recurrences**.

Improvement compounds, and so does its absence. Six handover steps yielding 0.95, 0.92, 0.90, 0.94,
0.88 and 0.85 average **90.67 %** and deliver **55.31 %** (26.82 of 60 packages needing rework at
**USD 69,720**), and the binding step, not the weakest-looking process, is where effort belongs:
fixing the 0.85 step gains **6.51 points** against **1.75** for fixing the 0.95 step, **3.73 times**
the return. An 80 % end-to-end target requires **96.35 %** at every one of six steps. The same
arithmetic governs data: six dimensions averaging **96.08 %** leave a composite fitness of **78.43
%**, about **690** of 3,200 asset records unfit for use. And it governs AI output, where the
breakeven error fraction of **2.22 %** sits far below the **13.91 %** a clean sample of 20 can bound
and the sample needed to reach it is **149 of 240 items**, so full review at **USD 9,600** is the
defensible regime, and the productivity gain claimed for generation was never real until the
verification was priced.

The through-line: **quality is a computable trade, its optimum is not zero, and every claim of
conformance is a bound that must be compared with the ratio of what checking costs to what being
wrong costs.** Compute that ratio, and quality stops being a virtue and becomes a decision.
